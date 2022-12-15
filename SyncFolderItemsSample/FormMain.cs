/*
 * By David Barrett, Microsoft Ltd. 2022. Use at your own risk.  No warranties are given.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Exchange.WebServices.Data;
using System.Threading;

namespace SyncFolderItemsSample
{
    public partial class FormMain : Form
    {
        private ExchangeService _service = null;
        private Dictionary<string, string> _autodiscoverCache = new Dictionary<string, string>();
        private string _folderHeirarchySyncState = null;
        private Dictionary<string, string> _folderSyncState = new Dictionary<string, string>();
        private Auth.FormAzureApplicationRegistration _oAuthAppRegForm = null;
        private Auth.CredentialHandler _credentialHandler = null;
        /// <summary>
        /// Trace listener used to capture EWS calls (thread safe)
        /// </summary>
        private ClassTraceListener _traceListener = null;
        /// <summary>
        /// Set to true when synchronisation is active (to ensure we don't trigger more than one sync process at the same time)
        /// </summary>
        private bool _amSyncing = false;
        /// <summary>
        /// When true, the current synchronisation is the first (i.e. to get the current contents of the mailbox)
        /// </summary>
        private bool _firstSync = true;
        private FormSyncViewer _mailboxViewer = null;
        List<System.Threading.Tasks.Task> _backGroundEWSTasks = new List<System.Threading.Tasks.Task>();

        private ExtendedPropertyDefinition PidTagMessageFlags = new ExtendedPropertyDefinition(0xE07, MapiPropertyType.Integer);


        public FormMain()
        {
            InitializeComponent();
            //ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            _oAuthAppRegForm = new Auth.FormAzureApplicationRegistration();
            _oAuthAppRegForm.TokenTextBox = textBoxOAuthToken;
        }

        /// <summary>
        /// Add the given event to the Events listbox.  If more than 1000 events already displayed, the oldest one will be removed.
        /// </summary>
        /// <param name="eventDescription">Event to be added</param>
        private void LogEvent(string eventDescription)
        {
            string log = String.Format("{0:H:mm:ss}: {1}", DateTime.Now, eventDescription);

            Action action = new Action(() =>
            {
                if (listBoxEvents.Items.Count > 1000)
                    listBoxEvents.Items.RemoveAt(0);
                listBoxEvents.Items.Add(log);
                if (listBoxEvents.SelectedIndex < 0)
                    listBoxEvents.TopIndex = listBoxEvents.Items.Count - 1;
            });
            if (listBoxEvents.InvokeRequired)
                listBoxEvents.Invoke(action);
            else
                action();
        }

        /// <summary>
        /// Call listBoxEvents.BeginUpdate() (invoking as required)
        /// </summary>
        private void EventListBoxBeginUpdate()
        {
            Action action = new Action(() =>
            {
                listBoxEvents.BeginUpdate();
            });
            if (listBoxEvents.InvokeRequired)
                listBoxEvents.Invoke(action);
            else
                action();
        }

        /// <summary>
        /// Call listBoxEvents.EndUpdate() (invoking as required)
        /// </summary>
        private void EventListBoxEndUpdate()
        {
            Action action = new Action(() =>
            {
                listBoxEvents.EndUpdate();
                listBoxEvents.Refresh();
            });
            if (listBoxEvents.InvokeRequired)
                listBoxEvents.Invoke(action);
            else
                action();
        }

        /// <summary>
        /// Start the synchronisation process (continues on background thread)
        /// </summary>
        private void StartSync()
        {
            if (checkBoxClearLogFile.Checked && String.IsNullOrEmpty(_folderHeirarchySyncState))
                _traceListener?.Clear();
            if (checkBoxShowMailboxViewer.Checked && _mailboxViewer == null)
            {
                _mailboxViewer = new FormSyncViewer();
                _mailboxViewer.Show();
            }

            ThreadPool.QueueUserWorkItem(new WaitCallback(Synchronize), null);
        }

        private void buttonSyncNow_Click(object sender, EventArgs e)
        {
            StartSync();
        }

        private bool InitCredentialHandler(bool Reset = false)
        {
            if (Reset)
                _credentialHandler = null;
            if (radioButtonAuthOther.Checked)
            {
                _credentialHandler = new Auth.CredentialHandler(Auth.AuthType.Basic);
                _credentialHandler.Username = textBoxUsername.Text;
                _credentialHandler.Password = textBoxPassword.Text;
            }
            else if (_oAuthAppRegForm.HaveValidAppConfig())
            {
                _credentialHandler = new Auth.CredentialHandler(_oAuthAppRegForm);
            }
            return (_credentialHandler != null);
        }

        private void SetClientRequestId()
        {
            _service.ClientRequestId = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Create an ExchangeService object with the currently selected settings (Auth, etc.).
        /// Always returns a new ExchangeService object, so can be used on a new thread
        /// </summary>
        /// <returns>ExchangeService object configured to send requests, or null if creation failed.</returns>
        private ExchangeService CreateExchangeService()
        {
            ExchangeService service = new ExchangeService();
            try
            {
                if (!InitCredentialHandler())
                    return null;
                service = new ExchangeService(EWSVersion());
                if (!_credentialHandler.ApplyCredentialsToExchangeService(service))
                    return null;
                if (checkBoxImpersonate.Checked)
                    service.ImpersonatedUserId = new ImpersonatedUserId(ConnectingIdType.SmtpAddress, textBoxMailbox.Text);
            }
            catch (Exception ex)
            {
                LogEvent(String.Format("Failed to instantiate Exchange service: {0}", ex.Message));
                return null;
            }

            // Attach trace listener
            if (_traceListener == null)
                _traceListener = new ClassTraceListener($"{Application.ProductName}.log");
            service.TraceListener = _traceListener;
            service.TraceFlags = TraceFlags.All;
            service.TraceEnabled = true;

            // Enable instrumentation
            service.ReturnClientRequestId = true;

            // Now perform autodiscover (if needed)
            if (radioButtonEwsUrl.Checked)
                service.Url = new Uri(textBoxEWSUrl.Text);
            else if (radioButtonOffice365.Checked)
                service.Url = new Uri("https://outlook.office365.com/EWS/Exchange.asmx");
            else
            {
                if (_autodiscoverCache.ContainsKey(textBoxMailbox.Text))
                    service.Url = new Uri(_autodiscoverCache[textBoxMailbox.Text]);
                else
                {
                    try
                    {
                        service.AutodiscoverUrl(textBoxMailbox.Text);
                    }
                    catch (Exception ex)
                    {
                        LogEvent(String.Format("Autodiscover for {0} failed: {1}", textBoxMailbox.Text, ex.Message));
                        return null;
                    }
                    _autodiscoverCache.Add(textBoxMailbox.Text, _service.Url.AbsoluteUri);
                }
            }
            return service;
        }

        /// <summary>
        /// Try to obtain a valid ExchangeService object (will create as needed, and validates OAuth credentials)
        /// </summary>
        /// <returns>True if a valid ExchangeService object is available</returns>
        private bool GetExchangeService()
        {
            if (_service != null)
            {
                _credentialHandler.ApplyCredentialsToExchangeService(_service);
                return true;
            }

            _service = CreateExchangeService();
            return (_service != null);
        }

        /// <summary>
        /// Return the currently selected Exchange version
        /// </summary>
        /// <returns></returns>
        private ExchangeVersion EWSVersion()
        {
            ExchangeVersion exchangeVersion = ExchangeVersion.Exchange2016;
            Action action = new Action(() =>
            {
                try
                {
                    if (comboBoxExchangeVersion.SelectedIndex != -1)
                        exchangeVersion = (ExchangeVersion)Enum.Parse(typeof(ExchangeVersion), comboBoxExchangeVersion.Text);
                }
                catch { }
            });
            if (comboBoxExchangeVersion.InvokeRequired)
                comboBoxExchangeVersion.Invoke(action);
            else
                action();
            return exchangeVersion;
        }

        /// <summary>
        /// Update availability of Sync Now button
        /// </summary>
        /// <param name="Enable">True if button is enabled, false otherwise</param>
        private void ToggleSyncButtons(bool Enable)
        {
            if (buttonSyncNow.InvokeRequired)
            {
                buttonSyncNow.Invoke(new MethodInvoker(delegate()
                {
                    buttonSyncNow.Enabled = Enable;
                }));
            }
            else
                buttonSyncNow.Enabled = Enable;
        }

        /// <summary>
        /// Synchronise the mailbox
        /// </summary>
        /// <param name="e">Ignored</param>
        private void Synchronize(object e)
        {
            if (_amSyncing)
                return;

            _amSyncing = true;
            ToggleSyncButtons(false);

            if (!GetExchangeService())
            {
                ToggleSyncButtons(true);
                _amSyncing = false;
                return;
            }

            bool bMoreEvents = true;
            LogEvent("Synchronisation started");

            // Check we can get root folder
            try
            {
                _credentialHandler.UpdateOAuthCredentialsForExchangeService(_service);
                SetClientRequestId();
                // Add root folder to mailbox view
                Folder msgFolderRoot = Folder.Bind(_service, WellKnownFolderName.MsgFolderRoot,
                    new PropertySet(BasePropertySet.IdOnly, FolderSchema.DisplayName));
                if (String.IsNullOrEmpty(_folderHeirarchySyncState))
                    _mailboxViewer?.AddFolder(msgFolderRoot.Id.UniqueId, null, msgFolderRoot.DisplayName);
            }
            catch (Exception ex)
            {
                LogEvent(String.Format("Error binding to MsgFolderRoot: {0}", ex.Message));
                return;
            }

            ExtendedPropertyDefinition PidTagAttributeHidden = new ExtendedPropertyDefinition(0x10f4, MapiPropertyType.Boolean);

            while (bMoreEvents)
            {
                ChangeCollection<FolderChange> folderChangeCollection;
                try
                {
                    _credentialHandler.UpdateOAuthCredentialsForExchangeService(_service);
                    SetClientRequestId();
                    folderChangeCollection = _service.SyncFolderHierarchy(new FolderId(WellKnownFolderName.MsgFolderRoot),
                        new PropertySet(BasePropertySet.IdOnly, FolderSchema.DisplayName, FolderSchema.ParentFolderId, PidTagAttributeHidden),
                        _folderHeirarchySyncState);
                }
                catch (Exception ex)
                {
                    LogEvent(String.Format("Error calling SyncFolderHierarchy: {0}", ex.Message));
                    return;
                }
                _folderHeirarchySyncState = folderChangeCollection.SyncState;

                // Process any changes (in case of first sync, we receive a Create event for each folder).
                if (folderChangeCollection.Count != 0)
                {
                    EventListBoxBeginUpdate();
                    foreach (FolderChange fc in folderChangeCollection)
                    {
                        // We aren't interested in any hidden folders (the user can't see these, and usually they contain configuration information for Exchange or mail clients)
                        bool isFolderHidden = false;
                        if (fc.Folder?.ExtendedProperties.Count>0)
                            foreach (ExtendedProperty prop in fc.Folder.ExtendedProperties)
                                if (prop.PropertyDefinition==PidTagAttributeHidden)
                                {
                                    isFolderHidden = (bool)prop.Value;
                                }

                        if (isFolderHidden)
                            LogEvent($"Hidden folder {fc.Folder?.DisplayName}: {fc.ChangeType} (will not be tracked)");
                        else
                        {
                            switch (fc.ChangeType)
                            {
                                case ChangeType.Create:
                                    {
                                        _folderSyncState.Add(fc.Folder.Id.UniqueId, null);
                                        _mailboxViewer?.AddFolder(fc.Folder.Id.UniqueId, fc.Folder.ParentFolderId.UniqueId, fc.Folder.DisplayName);
                                        LogEvent($"Folder {fc.Folder.DisplayName}: {fc.ChangeType}");
                                        break;
                                    }

                                case ChangeType.Delete:
                                    {
                                        string folderName = fc.FolderId.UniqueId;
                                        if (_mailboxViewer != null)
                                        {
                                            folderName = _mailboxViewer.FolderNameFromId(folderName);
                                            _mailboxViewer.DeleteFolder(fc.FolderId.UniqueId);
                                        }
                                        if (_folderSyncState.ContainsKey(fc.FolderId.UniqueId))
                                            _folderSyncState.Remove(fc.FolderId.UniqueId);
                                        LogEvent($"Folder {folderName}: {fc.ChangeType}");
                                        break;
                                    }

                                case ChangeType.Update:
                                    {
                                        _mailboxViewer?.UpdateFolder(fc.Folder.Id.UniqueId, fc.Folder.ParentFolderId.UniqueId, fc.Folder.DisplayName);
                                        LogEvent($"Folder {fc.Folder.DisplayName}: {fc.ChangeType}");
                                        break;
                                    }
                            }
                        }
                    }
                    EventListBoxEndUpdate();
                }
                bMoreEvents = folderChangeCollection.MoreChangesAvailable;
            }

            SyncFolders();
            LogEvent("Synchronisation complete");
            _firstSync = false;
            ToggleSyncButtons(true);
            _amSyncing = false;
            if (buttonStopTimedSync.Enabled)
            {
                Action action = new Action(() =>
                {
                    timerSync.Start();
                });
                if (InvokeRequired)
                    Invoke(action);
                else
                    action();
            }
        }

        /// <summary>
        /// Iterate through all the folders and trigger a synchronisation
        /// </summary>
        private void SyncFolders()
        {
            List<string> folderIds = _folderSyncState.Keys.ToList<string>();
            foreach (string folderid in folderIds)
            {
                _folderSyncState[folderid] = SyncFolder(folderid, _folderSyncState[folderid]);
            }
        }

        /// <summary>
        /// Process the given ItemChange Create event
        /// </summary>
        /// <param name="folder">Folder to which this item change applied</param>
        /// <param name="itemChange">Item change information</param>
        private void ProcessItemCreate(Folder folder, ItemChange itemChange)
        {
            _mailboxViewer?.AddMessage(folder.Id.UniqueId, itemChange.Item.Id.UniqueId, itemChange.Item.Subject);

            if (checkBoxAddCustomId.Checked)
            {
                Action action = new Action(() =>
                {
                    // We add a custom Id to each item the first time we see it (so that we know if we've seen it before/already synchronised).
                    // In the context of this sample, this isn't useful as we don't persist the data anywhere.
                    // This sample creates a named property in the PublicStrings namespace.
                    ExchangeService service = CreateExchangeService();
                    ExtendedPropertyDefinition propId = new ExtendedPropertyDefinition(DefaultExtendedPropertySet.PublicStrings, textBoxUniqueIdPropName.Text, MapiPropertyType.String);
                    try
                    {
                        Item item = Item.Bind(service, itemChange.ItemId, new PropertySet(BasePropertySet.IdOnly, propId));
                        bool bApplyId = true;

                        if (item.ExtendedProperties.Count > 0)
                        {
                            if (!_firstSync && checkBoxDetectCopiedItems.Checked)
                            {
                                // This is a copy, delete the existing Id
                                LogEvent($"Folder {folder.DisplayName}, Item {itemChange.Item.Subject}: Copy (updating tracking info){ReportMessageFlagUnmodified(itemChange.Item)}");
                            }
                            else
                            {
                                bApplyId = false;
                                LogEvent($"Folder {folder.DisplayName}, Item {itemChange.Item.Subject}: Create (already tracked){ReportMessageFlagUnmodified(itemChange.Item)}");
                            }
                        }
                        else
                            LogEvent($"Folder {folder.DisplayName}, Item {itemChange.Item.Subject}: Create (new item, adding tracking info){ReportMessageFlagUnmodified(itemChange.Item)}");

                        // Create new Id and apply to item
                        if (bApplyId)
                        {
                            string sId = DateTime.Now.Ticks.ToString();
                            item.SetExtendedProperty(propId, sId);
                            _credentialHandler.UpdateOAuthCredentialsForExchangeService(service);
                            SetClientRequestId();
                            // Note that writing the property will trigger a ChangeType.Update event in the next sync for this folder (as we have updated the item)
                            item.Update(ConflictResolutionMode.AlwaysOverwrite);
                        }
                    }
                    catch (Exception ex)
                    {
                        LogEvent($"Folder {folder.DisplayName}, Item {itemChange.Item.Subject}: Create - unexpected error: {ex.Message} ");
                    }

                });

                // This is a very crude way to update the property on a background thread.  More than 3 threads could hit concurrency throttling.
                _backGroundEWSTasks.Add(System.Threading.Tasks.Task.Run(() => action()));
            }
            else
                LogEvent($"Folder {folder.DisplayName}, Item {itemChange.Item.Subject}: Create{ReportMessageFlagUnmodified(itemChange.Item)}");

        }

        /// <summary>
        /// Check the item and report if MSGFLAG_UNMODIFIED is set
        /// </summary>
        /// <param name="item">The item to check message flags for (message flags must be present)</param>
        private string ReportMessageFlagUnmodified(Item item)
        {
            if (checkBoxQueryPidTagMessageFlags.Checked)
            {
                bool itemIsUnmodified = false;
                if (item.ExtendedProperties.Count > 0)
                {
                    // Check MSGFLAG_UNMODIFIED (0x00000002)
                    foreach (ExtendedProperty prop in item.ExtendedProperties)
                        if (prop.PropertyDefinition == PidTagMessageFlags)
                        {
                            itemIsUnmodified = (((int)prop.Value) & 0x00000002) == 0x00000002;
                            break;
                        }
                }
                if (itemIsUnmodified)
                    return " (mfUnmodified)";
            }
            return String.Empty;
        }

        /// <summary>
        /// Synchronise the specified folder
        /// </summary>
        /// <param name="folderId">Id of the folder to synchronise</param>
        /// <param name="syncState">Previous SyncState</param>
        /// <returns>New SyncState</returns>
        private string SyncFolder(string folderId, string syncState)
        {
            ChangeCollection<ItemChange> itemChangeCollection = null;
            bool bMoreEvents = true;
            _credentialHandler.UpdateOAuthCredentialsForExchangeService(_service);
            SetClientRequestId();
            Folder folder = Folder.Bind(_service, new FolderId(folderId), new PropertySet(BasePropertySet.IdOnly, FolderSchema.DisplayName));

            // We can use PidTagMessageFlags to determine if the contents of the message have been updated:
            // https://learn.microsoft.com/fi-fi/openspecs/exchange_server_protocols/ms-oxcmsg/a0c52fe2-3014-43a7-942d-f43f6f91c366
            // mfUnmodified
            // The message has not been modified since it was first saved (if unsent) or it was delivered (if sent).
            PropertySet itemProperties = new PropertySet(BasePropertySet.IdOnly, ItemSchema.Subject, PidTagMessageFlags);

            while (bMoreEvents)
            {
                try
                {
                    _credentialHandler.UpdateOAuthCredentialsForExchangeService(_service);
                    SetClientRequestId();
                    itemChangeCollection = _service.SyncFolderItems(new FolderId(folderId), itemProperties, null, 512, SyncFolderItemsScope.NormalItems, syncState);
                    syncState = itemChangeCollection.SyncState;
                }
                catch (Exception ex)
                {
                    LogEvent($"Error calling SyncFolderItems: {ex.Message}");
                    return syncState;
                }
                if (itemChangeCollection.Count != 0)
                {
                    EventListBoxBeginUpdate();
                    foreach (ItemChange ic in itemChangeCollection)
                    {
                        switch (ic.ChangeType)
                        {
                            case ChangeType.Create:
                                {
                                    ProcessItemCreate(folder, ic);
                                    break;
                                }

                            case ChangeType.Update:
                                {
                                    LogEvent($"Folder {folder.DisplayName}, Item {ic.Item?.Subject}: Update{ReportMessageFlagUnmodified(ic.Item)}");
                                    
                                    _mailboxViewer?.UpdateMessage(folderId, ic.Item.Id.UniqueId, ic.Item.Subject);
                                    break;
                                }

                            case ChangeType.Delete:
                                {
                                    if (ic.Item != null && ic.Item.Id != null)
                                        _mailboxViewer?.DeleteMessage(folderId, ic.Item.Id.UniqueId);
                                    LogEvent($"Folder {folder.DisplayName}, Item deleted: {ic.ItemId.UniqueId}");
                                    break;
                                }

                            default:
                                {
                                    LogEvent($"Folder {folder.DisplayName}, Item {ic.Item?.Subject}: {ic.ChangeType}");
                                    break;
                                }
                        }
                    }
                    EventListBoxEndUpdate();
                }
                bMoreEvents = itemChangeCollection.MoreChangesAvailable;                
            }

            if (_backGroundEWSTasks.Count>0)
            {
                // Wait for any background EWS tasks to finish before moving onto next folder
                while (_backGroundEWSTasks.Count > 0)
                {
                    while (_backGroundEWSTasks[0].Status == System.Threading.Tasks.TaskStatus.Running || _backGroundEWSTasks[0].Status == System.Threading.Tasks.TaskStatus.WaitingToRun)
                        Thread.Yield();
                    _backGroundEWSTasks.RemoveAt(0);
                }
            }
            return syncState;
        }

        private void textBoxUsername_TextChanged(object sender, EventArgs e)
        {
            _service = null;
        }

        private void textBoxPassword_TextChanged(object sender, EventArgs e)
        {
            _service = null;
        }

        private void textBoxMailbox_TextChanged(object sender, EventArgs e)
        {
            _service = null;
        }

        private void checkBoxImpersonate_CheckedChanged(object sender, EventArgs e)
        {
            _service = null;
        }

        private void UpdateEWSUrlControls()
        {
            textBoxEWSUrl.Enabled = radioButtonEwsUrl.Checked;
            _service = null;
        }
        private void radioButtonUseAutodiscover_CheckedChanged(object sender, EventArgs e)
        {
            UpdateEWSUrlControls();
        }

        private void radioButtonEwsUrl_CheckedChanged(object sender, EventArgs e)
        {
            UpdateEWSUrlControls();
        }

        private void buttonStartTimedSync_Click(object sender, EventArgs e)
        {
            DateTime interval = (DateTime)dtpSyncInterval.Value;
            timerSync.Interval = (interval.Second*1000) + (interval.Minute * 60000) + (interval.Hour * 60000 * 24);
            buttonStartTimedSync.Enabled = false;
            buttonStopTimedSync.Enabled = true;
            StartSync();
        }

        private void buttonAcquireToken_Click(object sender, EventArgs e)
        {
            if (_oAuthAppRegForm is null)
            {
                _oAuthAppRegForm = new Auth.FormAzureApplicationRegistration();
                _oAuthAppRegForm.TokenTextBox = textBoxOAuthToken;
            }
            _oAuthAppRegForm.AcquireToken();
            textBoxOAuthToken.Text = _oAuthAppRegForm.AccessToken;
        }

        private void timerSync_Tick(object sender, EventArgs e)
        {
            if (_amSyncing)
                return;

            timerSync.Stop();
            StartSync();
        }

        private void buttonStopTimedSync_Click(object sender, EventArgs e)
        {
            buttonStopTimedSync.Enabled = false;
            buttonStartTimedSync.Enabled = true;
            timerSync.Stop();
        }

        private void listBoxEvents_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxEvents.SelectedIndex < 0 || listBoxEvents.SelectedItems.Count > 1)
                return;

            if (MessageBox.Show(this, listBoxEvents.Items[listBoxEvents.SelectedIndex].ToString(), "Event information", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.Cancel)
                listBoxEvents.SelectedIndex = -1;
        }

        private void buttonOAuthAppRegistration_Click(object sender, EventArgs e)
        {
            _oAuthAppRegForm.TokenTextBox = textBoxOAuthToken; // So that the app reg form knows where to send any acquired tokens
            _oAuthAppRegForm.ShowDialog(this);
            this.Activate();
        }
    }
}
