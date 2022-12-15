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
using System.Windows.Forms;

namespace SyncFolderItemsSample
{
    public partial class FormSyncViewer : Form
    {
        private Dictionary<string, TreeNode> _folderIdToNode = new Dictionary<string, TreeNode>();
        private Dictionary<string,string> _parentFolderIds = new Dictionary<string,string>();
        public FormSyncViewer()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Checks if we have the given folder Id tracked, and if so return its DisplayName (useful for Delete events)
        /// </summary>
        /// <param name="FolderId">FolderId to retrieve DisplayName for</param>
        /// <returns>DisplayName of folder if found, otherwise FolderId</returns>
        public string FolderNameFromId(string FolderId)
        {
            if (!_folderIdToNode.ContainsKey(FolderId))
                return FolderId;
            return _folderIdToNode[FolderId].Text;
        }

        /// <summary>
        /// Add the specified folder to the folder tree
        /// </summary>
        /// <param name="FolderId">Id of the folder being added</param>
        /// <param name="ParentFolderId">Parent Id of the folder.  If null or empty, the new folder is the root folder (and the tree will be cleared).</param>
        /// <param name="DisplayName">Display name of the folder being added</param>
        /// <returns>True if successful</returns>
        public bool AddFolder(string FolderId, string ParentFolderId, string DisplayName)
        {
            if (!String.IsNullOrEmpty(ParentFolderId) && !_folderIdToNode.ContainsKey(ParentFolderId))
                return false;

            Action action = new Action(() =>
            {
                TreeNode folderNode;
                if (String.IsNullOrEmpty(ParentFolderId) || _folderIdToNode == null)
                {
                    // This is the root folder
                    treeViewMailboxView.Nodes.Clear();
                    _folderIdToNode = new Dictionary<string, TreeNode>();
                    folderNode = treeViewMailboxView.Nodes.Add(DisplayName);
                    _parentFolderIds.Add(FolderId, "0");
                }
                else
                {
                    folderNode = _folderIdToNode[ParentFolderId].Nodes.Add(DisplayName);
                    _parentFolderIds.Add(FolderId, ParentFolderId);
                }

                _folderIdToNode.Add(FolderId, folderNode);

            });

            if (treeViewMailboxView.InvokeRequired)
                treeViewMailboxView.Invoke(action);
            else
                action();
            return true;
        }

        /// <summary>
        /// Delete the specified folder from the treeview
        /// </summary>
        /// <param name="FolderId">Id of the folder to delete</param>
        /// <returns>True if successful, false if folder not found</returns>
        public bool DeleteFolder(string FolderId)
        {
            if (!_folderIdToNode.ContainsKey(FolderId))
                return false;

            Action action = new Action(() =>
            {
                treeViewMailboxView.Nodes.Remove(_folderIdToNode[FolderId]);
                _folderIdToNode.Remove(FolderId);
                if (_parentFolderIds.ContainsKey(FolderId))
                    _parentFolderIds.Remove(FolderId);
            });

            if (treeViewMailboxView.InvokeRequired)
                treeViewMailboxView.Invoke(action);
            else
                action();
            return true;
        }

        /// <summary>
        /// Update the specified folder with the provided information
        /// </summary>
        /// <param name="FolderId">FolderId of the folder being updated</param>
        /// <param name="ParentFolderId">Updated ParentFolderId for the given folder</param>
        /// <param name="DisplayName">Updated display name for the given folder</param>
        /// <returns>True if successful, otherwise false</returns>
        public bool UpdateFolder(string FolderId, string ParentFolderId, string DisplayName)
        {
            if (!_folderIdToNode.ContainsKey(FolderId))
                return false;

            if (String.IsNullOrEmpty(ParentFolderId))
                ParentFolderId = "0";

            Action action = new Action(() =>
            {
                TreeNode folderNode = _folderIdToNode[FolderId];
                if (_parentFolderIds[FolderId] != ParentFolderId)
                {
                    // Folder has moved (it has a new parent)
                    TreeNode newParent = _folderIdToNode[ParentFolderId];
                    treeViewMailboxView.Nodes.Remove(folderNode);
                    newParent.Nodes.Add(folderNode);
                    _parentFolderIds[FolderId] = ParentFolderId;
                }
                if (folderNode.Text != DisplayName)
                    folderNode.Text = DisplayName;

            });

            if (treeViewMailboxView.InvokeRequired)
                treeViewMailboxView.Invoke(action);
            else
                action();
            return true;
        }

        /// <summary>
        /// Add new message to the given folder
        /// </summary>
        /// <param name="FolderId">Id of the folder containing this message</param>
        /// <param name="MessageId">Id of the message</param>
        /// <param name="Subject">Message subject</param>
        /// <returns>True if successful, false otherwise (or item already exists)</returns>
        public bool AddMessage(string FolderId, string MessageId, string Subject)
        {
            if (!_folderIdToNode.ContainsKey(FolderId))
                return false;

            if (_folderIdToNode[FolderId].Tag == null)
                _folderIdToNode[FolderId].Tag = new Dictionary<string,string>();

            Dictionary<string, string> folderMessages = (Dictionary<string, string>)_folderIdToNode[FolderId].Tag;
            if (folderMessages.ContainsKey(MessageId))
                return false;

            folderMessages.Add(MessageId, Subject);
            UpdateFolderDisplay(FolderId);
            return true;
        }

        /// <summary>
        /// Updates the specified message
        /// </summary>
        /// <param name="FolderId">Id of the folder containing this message</param>
        /// <param name="MessageId">Id of the message</param>
        /// <param name="Subject">Message subject</param>
        /// <returns>True if message existed and was updated, false otherwise</returns>
        public bool UpdateMessage(string FolderId, string MessageId, string Subject)
        {
            if (!_folderIdToNode.ContainsKey(FolderId))
                return false;

            if (_folderIdToNode[FolderId].Tag == null)
                _folderIdToNode[FolderId].Tag = new Dictionary<string, string>();

            Dictionary<string, string> folderMessages = (Dictionary<string, string>)_folderIdToNode[FolderId].Tag;
            if (!folderMessages.ContainsKey(MessageId))
                return false;

            if (folderMessages[MessageId] != Subject)
            {
                folderMessages[MessageId] = Subject;
                UpdateFolderDisplay(FolderId);
            }
            return true;
        }

        /// <summary>
        /// Delete the specified message from the given folder
        /// </summary>
        /// <param name="FolderId">Id of the folder containing the message</param>
        /// <param name="MessageId">Id of the message</param>
        public void DeleteMessage(string FolderId, string MessageId)
        {
            if (!_folderIdToNode.ContainsKey(FolderId) || _folderIdToNode[FolderId].Tag == null)
                return;

            Dictionary<string, string> folderMessages = (Dictionary<string, string>)_folderIdToNode[FolderId].Tag;
            if (folderMessages.ContainsKey(MessageId))
                folderMessages.Remove(MessageId);

            UpdateFolderDisplay(FolderId);
        }

        /// <summary>
        /// Determines if the given folder is currently selected (showing messages), and if so, trigger an update
        /// This should be called when the contents of a folder change (to ensure UI is up-to-date)
        /// </summary>
        /// <param name="FolderId"></param>
        private void UpdateFolderDisplay(string FolderId)
        {
            if (!_folderIdToNode.ContainsKey(FolderId))
                return;

            Action action = new Action(() =>
            {
                if (_folderIdToNode[FolderId] == treeViewMailboxView.SelectedNode)
                    ShowSelectedFolderMessages();
            });
            if (treeViewMailboxView.InvokeRequired)
                treeViewMailboxView.Invoke(action);
            else
                action();
        }

        /// <summary>
        /// Update the UI to show the messages contained in the selected folder
        /// </summary>
        public void ShowSelectedFolderMessages()
        {
            listBoxFolderMessages.Items.Clear();
            if (treeViewMailboxView.SelectedNode.Tag != null)
            {
                Dictionary<string, string> folderMessages = (Dictionary<string, string>)treeViewMailboxView.SelectedNode.Tag;
                listBoxFolderMessages.BeginUpdate();
                foreach (string message in folderMessages.Values)
                    if (message != null)
                        listBoxFolderMessages.Items.Add(message);
                listBoxFolderMessages.EndUpdate();
            }
        }

        private void treeViewMailboxView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ShowSelectedFolderMessages();
        }
    }
}
