using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Exchange.WebServices.Data;
using System.Net;
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
        private ClassTraceListener _traceListener = null;
        private bool _amSyncing = false;
        private FormSyncViewer _mailboxViewer = null;

        public FormMain()
        {
            InitializeComponent();
            //ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            _oAuthAppRegForm = new Auth.FormAzureApplicationRegistration();
        }

        private void LogEvent(string eventDescription)
        {
            string log = String.Format("{0:H:mm:ss}: {1}", DateTime.Now, eventDescription);
            if (listBoxEvents.InvokeRequired)
            {
                listBoxEvents.Invoke(new MethodInvoker(delegate()
                {
                    listBoxEvents.Items.Add(log);
                    listBoxEvents.SelectedIndex = listBoxEvents.Items.Count - 1;
                }));
            }
            else
            {
                listBoxEvents.Items.Add(log);
                listBoxEvents.SelectedIndex = listBoxEvents.Items.Count - 1;
            }
        }

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

        private void StartSync()
        {
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

        private bool InitCredentialHandler()
        {
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

        private bool GetExchangeService()
        {
            if (_service != null)
            {
                _credentialHandler.ApplyCredentialsToExchangeService(_service);
                return true;
            }

            try
            {
                if (!InitCredentialHandler())
                    return false;
                _service = new ExchangeService(EWSVersion());
                _credentialHandler.ApplyCredentialsToExchangeService(_service);
                if (checkBoxImpersonate.Checked)
                    _service.ImpersonatedUserId = new ImpersonatedUserId(ConnectingIdType.SmtpAddress, textBoxMailbox.Text);
            }
            catch (Exception ex)
            {
                LogEvent(String.Format("Failed to instantiate Exchange service: {0}", ex.Message));
                return false;
            }

            // Attach trace listener
            if (_traceListener == null)
                _traceListener = new ClassTraceListener($"{Application.ProductName}.log");
            _service.TraceListener = _traceListener;
            _service.TraceFlags = TraceFlags.All;
            _service.TraceEnabled = true;

            // Enable instrumentation
            _service.ReturnClientRequestId = true;

            // Now perform autodiscover (if needed)
            if (radioButtonEwsUrl.Checked)
            {
                _service.Url = new Uri(textBoxEWSUrl.Text);
                return true;
            }
            if (radioButtonOffice365.Checked)
            {
                _service.Url = new Uri("https://outlook.office365.com/EWS/Exchange.asmx");
                return true;
            }

            if (_autodiscoverCache.ContainsKey(textBoxMailbox.Text))
            {
                _service.Url = new Uri(_autodiscoverCache[textBoxMailbox.Text]);
                return true;
            }

            try
            {
                _service.AutodiscoverUrl(textBoxMailbox.Text);
            }
            catch (Exception ex)
            {
                LogEvent(String.Format("Autodiscover for {0} failed: {1}", textBoxMailbox.Text, ex.Message));
                _service = null;
                return false;
            }
            _autodiscoverCache.Add(textBoxMailbox.Text, _service.Url.AbsoluteUri);
            return true;
        }

        private ExchangeVersion EWSVersion()
        {
            ExchangeVersion exchangeVersion = ExchangeVersion.Exchange2016;
            try
            {
                exchangeVersion = (ExchangeVersion)Enum.Parse(typeof(ExchangeVersion), comboBoxExchangeVersion.Text);
            }
            catch { }
            return exchangeVersion;
        }

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

            while (bMoreEvents)
            {
                ChangeCollection<FolderChange> folderChangeCollection;
                try
                {
                    _credentialHandler.UpdateOAuthCredentialsForExchangeService(_service);
                    SetClientRequestId();
                    folderChangeCollection = _service.SyncFolderHierarchy(new FolderId(WellKnownFolderName.MsgFolderRoot),
                        new PropertySet(BasePropertySet.IdOnly, FolderSchema.DisplayName, FolderSchema.ParentFolderId),
                        _folderHeirarchySyncState);
                }
                catch (Exception ex)
                {
                    LogEvent(String.Format("Error calling SyncFolderHierarchy: {0}", ex.Message));
                    return;
                }
                _folderHeirarchySyncState = folderChangeCollection.SyncState;

                // Display changes, if any. Note that instead of displaying the changes, 
                // you can create, update, or delete folders based on the changes retrieved from the server. 
                if (folderChangeCollection.Count != 0)
                {
                    EventListBoxBeginUpdate();
                    foreach (FolderChange fc in folderChangeCollection)
                    {
                        LogEvent(String.Format("Folder {0}: {1}", fc.Folder.DisplayName, fc.ChangeType.ToString()));
                        switch (fc.ChangeType)
                        {
                            case ChangeType.Create:
                                {
                                    _folderSyncState.Add(fc.Folder.Id.UniqueId, null);
                                    _mailboxViewer?.AddFolder(fc.Folder.Id.UniqueId, fc.Folder.ParentFolderId.UniqueId, fc.Folder.DisplayName);
                                    break;
                                }

                            case ChangeType.Delete:
                                {
                                    if (_folderSyncState.ContainsKey(fc.FolderId.UniqueId))
                                    {
                                        _folderSyncState.Remove(fc.FolderId.UniqueId);
                                    }
                                    break;
                                }
                        }
                    }
                    EventListBoxEndUpdate();
                }
                bMoreEvents = folderChangeCollection.MoreChangesAvailable;
            }

            SyncFolders();
            LogEvent("Synchronisation complete");
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

        private void SyncFolders()
        {
            // Process each folder in the heirarchy and sync
            List<string> folderIds = _folderSyncState.Keys.ToList<string>();
            foreach (string folderid in folderIds)
            {
                _folderSyncState[folderid] = SyncFolder(folderid, _folderSyncState[folderid]);
            }
        }

        private string SyncFolder(string folderId, string syncState)
        {
            ChangeCollection<ItemChange> itemChangeCollection = null;
            bool bMoreEvents = true;
            _credentialHandler.UpdateOAuthCredentialsForExchangeService(_service);
            SetClientRequestId();
            Folder folder = Folder.Bind(_service, new FolderId(folderId), new PropertySet(BasePropertySet.IdOnly, FolderSchema.DisplayName));
            PropertySet itemProperties = new PropertySet(BasePropertySet.IdOnly, ItemSchema.Subject);

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
                    LogEvent(String.Format("Error calling SyncFolderItems: {0}", ex.Message));
                    return syncState;
                }
                if (itemChangeCollection.Count != 0)
                {
                    EventListBoxBeginUpdate();
                    foreach (ItemChange ic in itemChangeCollection)
                    {
                        if (ic.ChangeType == ChangeType.Create)
                        {
                            _mailboxViewer?.AddMessage(folderId, ic.Item.Id.UniqueId, ic.Item.Subject);
                            if (checkBoxAddCustomId.Checked)
                            {
                                // Item was created.  Check if it has our Id on it (if it does, it was a copy and we need to assign a new Id)
                                ExtendedPropertyDefinition propId = new ExtendedPropertyDefinition(DefaultExtendedPropertySet.PublicStrings, textBoxUniqueIdPropName.Text, MapiPropertyType.String);
                                try
                                {
                                    _credentialHandler.UpdateOAuthCredentialsForExchangeService(_service);
                                    Item item = Item.Bind(_service, ic.ItemId, new PropertySet(BasePropertySet.IdOnly, propId));
                                    bool bApplyId = true;

                                    if (item.ExtendedProperties.Count > 0)
                                    {
                                        if (checkBoxDetectCopiedItems.Checked)
                                        {
                                            // This is a copy, delete the existing Id
                                            LogEvent(String.Format("Folder {0}, Item {1}: Copy", folder.DisplayName, ic.Item.Subject));
                                        }
                                        else
                                        {
                                            bApplyId = false;
                                            LogEvent(String.Format("Folder {0}, Item {1}: Create", folder.DisplayName, ic.Item.Subject));
                                        }
                                    }
                                    else
                                        LogEvent(String.Format("Folder {0}, Item {1}: Create", folder.DisplayName, ic.Item.Subject));

                                    // Create new Id and apply to item
                                    if (bApplyId)
                                    {
                                        string sId = DateTime.Now.Ticks.ToString();
                                        item.SetExtendedProperty(propId, sId);
                                        _credentialHandler.UpdateOAuthCredentialsForExchangeService(_service);
                                        SetClientRequestId();
                                        item.Update(ConflictResolutionMode.AlwaysOverwrite);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    LogEvent($"Folder {folder.DisplayName}, Item {ic.Item.Subject}: Create - unexpected error: {ex.Message} ");
                                }
                            }
                            else
                                LogEvent($"Folder {folder.DisplayName}, Item {ic.Item.Subject}: Create");
                        }
                        else
                        {
                            if (ic.ChangeType != ChangeType.Delete)
                            {
                                LogEvent($"Folder {folder.DisplayName}, Item {ic.Item.Subject}: {ic.ChangeType}");
                            }
                            else
                                LogEvent($"Folder {folder.DisplayName}, Item deleted: {ic.ItemId.UniqueId}");
                        }
                    }
                    EventListBoxEndUpdate();
                }
                bMoreEvents = itemChangeCollection.MoreChangesAvailable;                
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
    }
}
