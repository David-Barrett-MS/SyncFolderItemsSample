namespace SyncFolderItemsSample
{
    partial class FormMain
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
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButtonOffice365 = new System.Windows.Forms.RadioButton();
            this.radioButtonUseAutodiscover = new System.Windows.Forms.RadioButton();
            this.textBoxEWSUrl = new System.Windows.Forms.TextBox();
            this.radioButtonEwsUrl = new System.Windows.Forms.RadioButton();
            this.checkBoxImpersonate = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxMailbox = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.comboBoxExchangeVersion = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBoxShowMailboxViewer = new System.Windows.Forms.CheckBox();
            this.checkBoxDetectCopiedItems = new System.Windows.Forms.CheckBox();
            this.textBoxUniqueIdPropName = new System.Windows.Forms.TextBox();
            this.checkBoxAddCustomId = new System.Windows.Forms.CheckBox();
            this.buttonStopTimedSync = new System.Windows.Forms.Button();
            this.buttonStartTimedSync = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.dtpSyncInterval = new System.Windows.Forms.DateTimePicker();
            this.buttonSyncNow = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.listBoxEvents = new System.Windows.Forms.ListBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.radioButtonAuthOther = new System.Windows.Forms.RadioButton();
            this.buttonAcquireToken = new System.Windows.Forms.Button();
            this.textBoxOAuthToken = new System.Windows.Forms.TextBox();
            this.radioButtonOAuth = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.textBoxUsername = new System.Windows.Forms.TextBox();
            this.timerSync = new System.Windows.Forms.Timer(this.components);
            this.checkBoxClearLogFile = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButtonOffice365);
            this.groupBox1.Controls.Add(this.radioButtonUseAutodiscover);
            this.groupBox1.Controls.Add(this.textBoxEWSUrl);
            this.groupBox1.Controls.Add(this.radioButtonEwsUrl);
            this.groupBox1.Controls.Add(this.checkBoxImpersonate);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.textBoxMailbox);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.comboBoxExchangeVersion);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(817, 79);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "EWS Configuration";
            // 
            // radioButtonOffice365
            // 
            this.radioButtonOffice365.AutoSize = true;
            this.radioButtonOffice365.Checked = true;
            this.radioButtonOffice365.Location = new System.Drawing.Point(9, 20);
            this.radioButtonOffice365.Name = "radioButtonOffice365";
            this.radioButtonOffice365.Size = new System.Drawing.Size(74, 17);
            this.radioButtonOffice365.TabIndex = 24;
            this.radioButtonOffice365.TabStop = true;
            this.radioButtonOffice365.Text = "Office 365";
            this.radioButtonOffice365.UseVisualStyleBackColor = true;
            // 
            // radioButtonUseAutodiscover
            // 
            this.radioButtonUseAutodiscover.AutoSize = true;
            this.radioButtonUseAutodiscover.Location = new System.Drawing.Point(696, 20);
            this.radioButtonUseAutodiscover.Name = "radioButtonUseAutodiscover";
            this.radioButtonUseAutodiscover.Size = new System.Drawing.Size(109, 17);
            this.radioButtonUseAutodiscover.TabIndex = 23;
            this.radioButtonUseAutodiscover.Text = "Use Autodiscover";
            this.radioButtonUseAutodiscover.UseVisualStyleBackColor = true;
            this.radioButtonUseAutodiscover.CheckedChanged += new System.EventHandler(this.radioButtonUseAutodiscover_CheckedChanged);
            // 
            // textBoxEWSUrl
            // 
            this.textBoxEWSUrl.Enabled = false;
            this.textBoxEWSUrl.Location = new System.Drawing.Point(164, 19);
            this.textBoxEWSUrl.Name = "textBoxEWSUrl";
            this.textBoxEWSUrl.Size = new System.Drawing.Size(522, 20);
            this.textBoxEWSUrl.TabIndex = 22;
            this.textBoxEWSUrl.Text = "https://e1.e19.local/EWS/Exchange.asmx";
            // 
            // radioButtonEwsUrl
            // 
            this.radioButtonEwsUrl.AutoSize = true;
            this.radioButtonEwsUrl.Location = new System.Drawing.Point(89, 20);
            this.radioButtonEwsUrl.Name = "radioButtonEwsUrl";
            this.radioButtonEwsUrl.Size = new System.Drawing.Size(69, 17);
            this.radioButtonEwsUrl.TabIndex = 21;
            this.radioButtonEwsUrl.Text = "EWS Url:";
            this.radioButtonEwsUrl.UseVisualStyleBackColor = true;
            this.radioButtonEwsUrl.CheckedChanged += new System.EventHandler(this.radioButtonEwsUrl_CheckedChanged);
            // 
            // checkBoxImpersonate
            // 
            this.checkBoxImpersonate.AutoSize = true;
            this.checkBoxImpersonate.Location = new System.Drawing.Point(348, 47);
            this.checkBoxImpersonate.Name = "checkBoxImpersonate";
            this.checkBoxImpersonate.Size = new System.Drawing.Size(84, 17);
            this.checkBoxImpersonate.TabIndex = 20;
            this.checkBoxImpersonate.Text = "Impersonate";
            this.checkBoxImpersonate.UseVisualStyleBackColor = true;
            this.checkBoxImpersonate.CheckedChanged += new System.EventHandler(this.checkBoxImpersonate_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 19;
            this.label3.Text = "Mailbox:";
            // 
            // textBoxMailbox
            // 
            this.textBoxMailbox.Location = new System.Drawing.Point(58, 45);
            this.textBoxMailbox.Name = "textBoxMailbox";
            this.textBoxMailbox.Size = new System.Drawing.Size(284, 20);
            this.textBoxMailbox.TabIndex = 18;
            this.textBoxMailbox.TextChanged += new System.EventHandler(this.textBoxMailbox_TextChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(553, 48);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(95, 13);
            this.label9.TabIndex = 17;
            this.label9.Text = "Exchange version:";
            // 
            // comboBoxExchangeVersion
            // 
            this.comboBoxExchangeVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxExchangeVersion.FormattingEnabled = true;
            this.comboBoxExchangeVersion.Items.AddRange(new object[] {
            "Not Set",
            "Exchange2007",
            "Exchange2007_SP1",
            "Exchange2010",
            "Exchange2010_SP1",
            "Exchange2010_SP2",
            "Exchange2013",
            "Exchange2013_SP1",
            "Exchange2016"});
            this.comboBoxExchangeVersion.Location = new System.Drawing.Point(654, 45);
            this.comboBoxExchangeVersion.Name = "comboBoxExchangeVersion";
            this.comboBoxExchangeVersion.Size = new System.Drawing.Size(157, 21);
            this.comboBoxExchangeVersion.TabIndex = 16;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBoxClearLogFile);
            this.groupBox2.Controls.Add(this.checkBoxShowMailboxViewer);
            this.groupBox2.Controls.Add(this.checkBoxDetectCopiedItems);
            this.groupBox2.Controls.Add(this.textBoxUniqueIdPropName);
            this.groupBox2.Controls.Add(this.checkBoxAddCustomId);
            this.groupBox2.Controls.Add(this.buttonStopTimedSync);
            this.groupBox2.Controls.Add(this.buttonStartTimedSync);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.dtpSyncInterval);
            this.groupBox2.Controls.Add(this.buttonSyncNow);
            this.groupBox2.Location = new System.Drawing.Point(12, 155);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(817, 77);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Synchronisation";
            // 
            // checkBoxShowMailboxViewer
            // 
            this.checkBoxShowMailboxViewer.AutoSize = true;
            this.checkBoxShowMailboxViewer.Checked = true;
            this.checkBoxShowMailboxViewer.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxShowMailboxViewer.Location = new System.Drawing.Point(449, 52);
            this.checkBoxShowMailboxViewer.Name = "checkBoxShowMailboxViewer";
            this.checkBoxShowMailboxViewer.Size = new System.Drawing.Size(195, 17);
            this.checkBoxShowMailboxViewer.TabIndex = 8;
            this.checkBoxShowMailboxViewer.Text = "Show Mailbox Viewer (experimental)";
            this.checkBoxShowMailboxViewer.UseVisualStyleBackColor = true;
            // 
            // checkBoxDetectCopiedItems
            // 
            this.checkBoxDetectCopiedItems.AutoSize = true;
            this.checkBoxDetectCopiedItems.Location = new System.Drawing.Point(6, 46);
            this.checkBoxDetectCopiedItems.Name = "checkBoxDetectCopiedItems";
            this.checkBoxDetectCopiedItems.Size = new System.Drawing.Size(222, 17);
            this.checkBoxDetectCopiedItems.TabIndex = 7;
            this.checkBoxDetectCopiedItems.Text = "Detect copied items and ensure unique id";
            this.checkBoxDetectCopiedItems.UseVisualStyleBackColor = true;
            // 
            // textBoxUniqueIdPropName
            // 
            this.textBoxUniqueIdPropName.Location = new System.Drawing.Point(198, 21);
            this.textBoxUniqueIdPropName.Name = "textBoxUniqueIdPropName";
            this.textBoxUniqueIdPropName.Size = new System.Drawing.Size(100, 20);
            this.textBoxUniqueIdPropName.TabIndex = 6;
            this.textBoxUniqueIdPropName.Text = "SyncTest";
            // 
            // checkBoxAddCustomId
            // 
            this.checkBoxAddCustomId.AutoSize = true;
            this.checkBoxAddCustomId.Location = new System.Drawing.Point(6, 23);
            this.checkBoxAddCustomId.Name = "checkBoxAddCustomId";
            this.checkBoxAddCustomId.Size = new System.Drawing.Size(195, 17);
            this.checkBoxAddCustomId.TabIndex = 5;
            this.checkBoxAddCustomId.Text = "Add custom property with unique Id:";
            this.checkBoxAddCustomId.UseVisualStyleBackColor = true;
            // 
            // buttonStopTimedSync
            // 
            this.buttonStopTimedSync.Enabled = false;
            this.buttonStopTimedSync.Location = new System.Drawing.Point(730, 19);
            this.buttonStopTimedSync.Name = "buttonStopTimedSync";
            this.buttonStopTimedSync.Size = new System.Drawing.Size(75, 23);
            this.buttonStopTimedSync.TabIndex = 4;
            this.buttonStopTimedSync.Text = "Stop";
            this.buttonStopTimedSync.UseVisualStyleBackColor = true;
            this.buttonStopTimedSync.Click += new System.EventHandler(this.buttonStopTimedSync_Click);
            // 
            // buttonStartTimedSync
            // 
            this.buttonStartTimedSync.Location = new System.Drawing.Point(649, 19);
            this.buttonStartTimedSync.Name = "buttonStartTimedSync";
            this.buttonStartTimedSync.Size = new System.Drawing.Size(75, 23);
            this.buttonStartTimedSync.TabIndex = 3;
            this.buttonStartTimedSync.Text = "Start";
            this.buttonStartTimedSync.UseVisualStyleBackColor = true;
            this.buttonStartTimedSync.Click += new System.EventHandler(this.buttonStartTimedSync_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(446, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(117, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Synchronize at interval:";
            // 
            // dtpSyncInterval
            // 
            this.dtpSyncInterval.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpSyncInterval.Location = new System.Drawing.Point(569, 20);
            this.dtpSyncInterval.Name = "dtpSyncInterval";
            this.dtpSyncInterval.ShowUpDown = true;
            this.dtpSyncInterval.Size = new System.Drawing.Size(73, 20);
            this.dtpSyncInterval.TabIndex = 1;
            this.dtpSyncInterval.Value = new System.DateTime(2022, 11, 21, 0, 0, 30, 0);
            // 
            // buttonSyncNow
            // 
            this.buttonSyncNow.Location = new System.Drawing.Point(694, 48);
            this.buttonSyncNow.Name = "buttonSyncNow";
            this.buttonSyncNow.Size = new System.Drawing.Size(111, 23);
            this.buttonSyncNow.TabIndex = 0;
            this.buttonSyncNow.Text = "Synchronize Now";
            this.buttonSyncNow.UseVisualStyleBackColor = true;
            this.buttonSyncNow.Click += new System.EventHandler(this.buttonSyncNow_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.listBoxEvents);
            this.groupBox3.Location = new System.Drawing.Point(12, 238);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(817, 279);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Events";
            // 
            // listBoxEvents
            // 
            this.listBoxEvents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxEvents.FormattingEnabled = true;
            this.listBoxEvents.Location = new System.Drawing.Point(3, 16);
            this.listBoxEvents.Name = "listBoxEvents";
            this.listBoxEvents.Size = new System.Drawing.Size(811, 260);
            this.listBoxEvents.TabIndex = 0;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.radioButtonAuthOther);
            this.groupBox4.Controls.Add(this.buttonAcquireToken);
            this.groupBox4.Controls.Add(this.textBoxOAuthToken);
            this.groupBox4.Controls.Add(this.radioButtonOAuth);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Controls.Add(this.textBoxPassword);
            this.groupBox4.Controls.Add(this.textBoxUsername);
            this.groupBox4.Location = new System.Drawing.Point(12, 97);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(817, 52);
            this.groupBox4.TabIndex = 4;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Authentication";
            // 
            // radioButtonAuthOther
            // 
            this.radioButtonAuthOther.AutoSize = true;
            this.radioButtonAuthOther.Location = new System.Drawing.Point(428, 17);
            this.radioButtonAuthOther.Name = "radioButtonAuthOther";
            this.radioButtonAuthOther.Size = new System.Drawing.Size(54, 17);
            this.radioButtonAuthOther.TabIndex = 11;
            this.radioButtonAuthOther.TabStop = true;
            this.radioButtonAuthOther.Text = "Other:";
            this.radioButtonAuthOther.UseVisualStyleBackColor = true;
            // 
            // buttonAcquireToken
            // 
            this.buttonAcquireToken.Location = new System.Drawing.Point(304, 14);
            this.buttonAcquireToken.Name = "buttonAcquireToken";
            this.buttonAcquireToken.Size = new System.Drawing.Size(92, 23);
            this.buttonAcquireToken.TabIndex = 10;
            this.buttonAcquireToken.Text = "Acquire Token";
            this.buttonAcquireToken.UseVisualStyleBackColor = true;
            this.buttonAcquireToken.Click += new System.EventHandler(this.buttonAcquireToken_Click);
            // 
            // textBoxOAuthToken
            // 
            this.textBoxOAuthToken.Location = new System.Drawing.Point(70, 16);
            this.textBoxOAuthToken.Name = "textBoxOAuthToken";
            this.textBoxOAuthToken.Size = new System.Drawing.Size(228, 20);
            this.textBoxOAuthToken.TabIndex = 9;
            // 
            // radioButtonOAuth
            // 
            this.radioButtonOAuth.AutoSize = true;
            this.radioButtonOAuth.Checked = true;
            this.radioButtonOAuth.Location = new System.Drawing.Point(9, 17);
            this.radioButtonOAuth.Name = "radioButtonOAuth";
            this.radioButtonOAuth.Size = new System.Drawing.Size(55, 17);
            this.radioButtonOAuth.TabIndex = 8;
            this.radioButtonOAuth.TabStop = true;
            this.radioButtonOAuth.Text = "OAuth";
            this.radioButtonOAuth.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(657, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Password:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(488, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Username:";
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Location = new System.Drawing.Point(719, 16);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.Size = new System.Drawing.Size(84, 20);
            this.textBoxPassword.TabIndex = 5;
            this.textBoxPassword.UseSystemPasswordChar = true;
            // 
            // textBoxUsername
            // 
            this.textBoxUsername.Location = new System.Drawing.Point(552, 16);
            this.textBoxUsername.Name = "textBoxUsername";
            this.textBoxUsername.Size = new System.Drawing.Size(99, 20);
            this.textBoxUsername.TabIndex = 4;
            // 
            // timerSync
            // 
            this.timerSync.Interval = 30000;
            this.timerSync.Tick += new System.EventHandler(this.timerSync_Tick);
            // 
            // checkBoxClearLogFile
            // 
            this.checkBoxClearLogFile.AutoSize = true;
            this.checkBoxClearLogFile.Checked = true;
            this.checkBoxClearLogFile.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxClearLogFile.Location = new System.Drawing.Point(322, 52);
            this.checkBoxClearLogFile.Name = "checkBoxClearLogFile";
            this.checkBoxClearLogFile.Size = new System.Drawing.Size(121, 17);
            this.checkBoxClearLogFile.TabIndex = 9;
            this.checkBoxClearLogFile.Text = "Clear log file on start";
            this.checkBoxClearLogFile.UseVisualStyleBackColor = true;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(843, 529);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormMain";
            this.Text = "SyncFolderItems Sample";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox comboBoxExchangeVersion;
        private System.Windows.Forms.CheckBox checkBoxImpersonate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxMailbox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button buttonStopTimedSync;
        private System.Windows.Forms.Button buttonStartTimedSync;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dtpSyncInterval;
        private System.Windows.Forms.Button buttonSyncNow;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ListBox listBoxEvents;
        private System.Windows.Forms.RadioButton radioButtonUseAutodiscover;
        private System.Windows.Forms.TextBox textBoxEWSUrl;
        private System.Windows.Forms.RadioButton radioButtonEwsUrl;
        private System.Windows.Forms.CheckBox checkBoxDetectCopiedItems;
        private System.Windows.Forms.TextBox textBoxUniqueIdPropName;
        private System.Windows.Forms.CheckBox checkBoxAddCustomId;
        private System.Windows.Forms.RadioButton radioButtonOffice365;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RadioButton radioButtonAuthOther;
        private System.Windows.Forms.Button buttonAcquireToken;
        private System.Windows.Forms.TextBox textBoxOAuthToken;
        private System.Windows.Forms.RadioButton radioButtonOAuth;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.TextBox textBoxUsername;
        private System.Windows.Forms.Timer timerSync;
        private System.Windows.Forms.CheckBox checkBoxShowMailboxViewer;
        private System.Windows.Forms.CheckBox checkBoxClearLogFile;
    }
}

