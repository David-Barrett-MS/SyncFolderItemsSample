namespace SyncFolderItemsSample
{
    partial class FormSyncViewer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeViewMailboxView = new System.Windows.Forms.TreeView();
            this.listBoxFolderMessages = new System.Windows.Forms.ListBox();
            this.richTextBoxItemView = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeViewMailboxView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.richTextBoxItemView);
            this.splitContainer1.Panel2.Controls.Add(this.listBoxFolderMessages);
            this.splitContainer1.Size = new System.Drawing.Size(798, 422);
            this.splitContainer1.SplitterDistance = 359;
            this.splitContainer1.TabIndex = 1;
            // 
            // treeViewMailboxView
            // 
            this.treeViewMailboxView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewMailboxView.Location = new System.Drawing.Point(0, 0);
            this.treeViewMailboxView.Name = "treeViewMailboxView";
            this.treeViewMailboxView.Size = new System.Drawing.Size(359, 422);
            this.treeViewMailboxView.TabIndex = 1;
            this.treeViewMailboxView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewMailboxView_AfterSelect);
            // 
            // listBoxFolderMessages
            // 
            this.listBoxFolderMessages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxFolderMessages.FormattingEnabled = true;
            this.listBoxFolderMessages.Location = new System.Drawing.Point(0, 0);
            this.listBoxFolderMessages.Name = "listBoxFolderMessages";
            this.listBoxFolderMessages.Size = new System.Drawing.Size(432, 238);
            this.listBoxFolderMessages.TabIndex = 0;
            this.listBoxFolderMessages.SelectedIndexChanged += new System.EventHandler(this.listBoxFolderMessages_SelectedIndexChanged);
            // 
            // richTextBoxItemView
            // 
            this.richTextBoxItemView.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.richTextBoxItemView.Location = new System.Drawing.Point(0, 242);
            this.richTextBoxItemView.Name = "richTextBoxItemView";
            this.richTextBoxItemView.Size = new System.Drawing.Size(435, 180);
            this.richTextBoxItemView.TabIndex = 1;
            this.richTextBoxItemView.Text = "";
            // 
            // FormSyncViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(798, 422);
            this.Controls.Add(this.splitContainer1);
            this.Name = "FormSyncViewer";
            this.Text = "Mailbox Viewer";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView treeViewMailboxView;
        private System.Windows.Forms.ListBox listBoxFolderMessages;
        private System.Windows.Forms.RichTextBox richTextBoxItemView;
    }
}