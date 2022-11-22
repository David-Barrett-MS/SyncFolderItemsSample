using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SyncFolderItemsSample
{
    public partial class FormSyncViewer : Form
    {
        private Dictionary<string, TreeNode> _folderIdToNode = new Dictionary<string, TreeNode>();
        public FormSyncViewer()
        {
            InitializeComponent();
        }

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
                }
                else
                {
                    folderNode = _folderIdToNode[ParentFolderId].Nodes.Add(DisplayName);
                }

                _folderIdToNode.Add(FolderId, folderNode);

            });

            if (treeViewMailboxView.InvokeRequired)
                treeViewMailboxView.Invoke(action);
            else
                action();
            return true;
        }

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
            return true;
        }

        private void treeViewMailboxView_AfterSelect(object sender, TreeViewEventArgs e)
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
    }
}
