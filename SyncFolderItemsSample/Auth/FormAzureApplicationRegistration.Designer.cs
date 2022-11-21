namespace SyncFolderItemsSample.Auth
{
    partial class FormAzureApplicationRegistration
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAzureApplicationRegistration));
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBoxResourceUrl = new System.Windows.Forms.TextBox();
            this.textBoxTenantId = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxApplicationId = new System.Windows.Forms.TextBox();
            this.groupBoxAuth = new System.Windows.Forms.GroupBox();
            this.radioButtonAuthAsNativeApp = new System.Windows.Forms.RadioButton();
            this.textBoxAuthCertificate = new System.Windows.Forms.TextBox();
            this.buttonLoadCertificate = new System.Windows.Forms.Button();
            this.radioButtonAuthWithCertificate = new System.Windows.Forms.RadioButton();
            this.radioButtonAuthWithClientSecret = new System.Windows.Forms.RadioButton();
            this.textBoxClientSecret = new System.Windows.Forms.TextBox();
            this.buttonAcquireToken = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.groupBox2.SuspendLayout();
            this.groupBoxAuth.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBoxResourceUrl);
            this.groupBox2.Controls.Add(this.textBoxTenantId);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.textBoxApplicationId);
            this.groupBox2.Location = new System.Drawing.Point(14, 12);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox2.Size = new System.Drawing.Size(802, 158);
            this.groupBox2.TabIndex = 36;
            this.groupBox2.TabStop = false;
            this.groupBox2.Tag = "NoConfigSave";
            this.groupBox2.Text = "Application Information";
            // 
            // textBoxResourceUrl
            // 
            this.textBoxResourceUrl.Location = new System.Drawing.Point(184, 72);
            this.textBoxResourceUrl.Name = "textBoxResourceUrl";
            this.textBoxResourceUrl.Size = new System.Drawing.Size(602, 26);
            this.textBoxResourceUrl.TabIndex = 22;
            this.textBoxResourceUrl.Text = "https://outlook.office365.com/";
            // 
            // textBoxTenantId
            // 
            this.textBoxTenantId.Location = new System.Drawing.Point(184, 31);
            this.textBoxTenantId.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.textBoxTenantId.Name = "textBoxTenantId";
            this.textBoxTenantId.Size = new System.Drawing.Size(602, 26);
            this.textBoxTenantId.TabIndex = 20;
            this.textBoxTenantId.Text = "common";
            this.textBoxTenantId.TextChanged += new System.EventHandler(this.textBoxTenantId_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 114);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(118, 20);
            this.label1.TabIndex = 14;
            this.label1.Text = "Application ID*:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 35);
            this.label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 20);
            this.label2.TabIndex = 15;
            this.label2.Tag = "";
            this.label2.Text = "Tenant ID*:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 77);
            this.label3.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(112, 20);
            this.label3.TabIndex = 16;
            this.label3.Text = "Resource Url*:";
            // 
            // textBoxApplicationId
            // 
            this.textBoxApplicationId.Location = new System.Drawing.Point(184, 111);
            this.textBoxApplicationId.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.textBoxApplicationId.Name = "textBoxApplicationId";
            this.textBoxApplicationId.Size = new System.Drawing.Size(602, 26);
            this.textBoxApplicationId.TabIndex = 21;
            this.textBoxApplicationId.Text = "4a03b746-45be-488c-bfe5-0ffdac557d68";
            // 
            // groupBoxAuth
            // 
            this.groupBoxAuth.Controls.Add(this.radioButtonAuthAsNativeApp);
            this.groupBoxAuth.Controls.Add(this.textBoxAuthCertificate);
            this.groupBoxAuth.Controls.Add(this.buttonLoadCertificate);
            this.groupBoxAuth.Controls.Add(this.radioButtonAuthWithCertificate);
            this.groupBoxAuth.Controls.Add(this.radioButtonAuthWithClientSecret);
            this.groupBoxAuth.Controls.Add(this.textBoxClientSecret);
            this.groupBoxAuth.Location = new System.Drawing.Point(12, 175);
            this.groupBoxAuth.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBoxAuth.Name = "groupBoxAuth";
            this.groupBoxAuth.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBoxAuth.Size = new System.Drawing.Size(802, 134);
            this.groupBoxAuth.TabIndex = 40;
            this.groupBoxAuth.TabStop = false;
            this.groupBoxAuth.Tag = "NoConfigSave";
            this.groupBoxAuth.Text = "Authentication";
            // 
            // radioButtonAuthAsNativeApp
            // 
            this.radioButtonAuthAsNativeApp.AutoSize = true;
            this.radioButtonAuthAsNativeApp.Checked = true;
            this.radioButtonAuthAsNativeApp.Location = new System.Drawing.Point(10, 25);
            this.radioButtonAuthAsNativeApp.Name = "radioButtonAuthAsNativeApp";
            this.radioButtonAuthAsNativeApp.Size = new System.Drawing.Size(573, 24);
            this.radioButtonAuthAsNativeApp.TabIndex = 32;
            this.radioButtonAuthAsNativeApp.TabStop = true;
            this.radioButtonAuthAsNativeApp.Tag = "NoTextSave";
            this.radioButtonAuthAsNativeApp.Text = "As native application (user prompted to log-in, will trigger consent if required)" +
    ")";
            this.radioButtonAuthAsNativeApp.UseVisualStyleBackColor = true;
            this.radioButtonAuthAsNativeApp.CheckedChanged += new System.EventHandler(this.radioButtonAuthAsNativeApp_CheckedChanged);
            // 
            // textBoxAuthCertificate
            // 
            this.textBoxAuthCertificate.Location = new System.Drawing.Point(144, 89);
            this.textBoxAuthCertificate.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBoxAuthCertificate.Name = "textBoxAuthCertificate";
            this.textBoxAuthCertificate.Size = new System.Drawing.Size(560, 26);
            this.textBoxAuthCertificate.TabIndex = 31;
            // 
            // buttonLoadCertificate
            // 
            this.buttonLoadCertificate.Location = new System.Drawing.Point(711, 86);
            this.buttonLoadCertificate.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonLoadCertificate.Name = "buttonLoadCertificate";
            this.buttonLoadCertificate.Size = new System.Drawing.Size(75, 32);
            this.buttonLoadCertificate.TabIndex = 30;
            this.buttonLoadCertificate.Text = "Select...";
            this.buttonLoadCertificate.UseVisualStyleBackColor = true;
            this.buttonLoadCertificate.Click += new System.EventHandler(this.buttonLoadCertificate_Click);
            // 
            // radioButtonAuthWithCertificate
            // 
            this.radioButtonAuthWithCertificate.AutoSize = true;
            this.radioButtonAuthWithCertificate.Location = new System.Drawing.Point(10, 89);
            this.radioButtonAuthWithCertificate.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.radioButtonAuthWithCertificate.Name = "radioButtonAuthWithCertificate";
            this.radioButtonAuthWithCertificate.Size = new System.Drawing.Size(110, 24);
            this.radioButtonAuthWithCertificate.TabIndex = 28;
            this.radioButtonAuthWithCertificate.Tag = "NoTextSave";
            this.radioButtonAuthWithCertificate.Text = "Certificate:";
            this.radioButtonAuthWithCertificate.UseVisualStyleBackColor = true;
            this.radioButtonAuthWithCertificate.CheckedChanged += new System.EventHandler(this.radioButtonAuthWithCertificate_CheckedChanged);
            // 
            // radioButtonAuthWithClientSecret
            // 
            this.radioButtonAuthWithClientSecret.AutoSize = true;
            this.radioButtonAuthWithClientSecret.Location = new System.Drawing.Point(10, 57);
            this.radioButtonAuthWithClientSecret.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.radioButtonAuthWithClientSecret.Name = "radioButtonAuthWithClientSecret";
            this.radioButtonAuthWithClientSecret.Size = new System.Drawing.Size(126, 24);
            this.radioButtonAuthWithClientSecret.TabIndex = 0;
            this.radioButtonAuthWithClientSecret.Tag = "NoTextSave";
            this.radioButtonAuthWithClientSecret.Text = "Client secret:";
            this.radioButtonAuthWithClientSecret.UseVisualStyleBackColor = true;
            this.radioButtonAuthWithClientSecret.CheckedChanged += new System.EventHandler(this.radioButtonAuthWithClientSecret_CheckedChanged);
            // 
            // textBoxClientSecret
            // 
            this.textBoxClientSecret.Location = new System.Drawing.Point(144, 55);
            this.textBoxClientSecret.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBoxClientSecret.Name = "textBoxClientSecret";
            this.textBoxClientSecret.Size = new System.Drawing.Size(642, 26);
            this.textBoxClientSecret.TabIndex = 27;
            this.textBoxClientSecret.UseSystemPasswordChar = true;
            // 
            // buttonAcquireToken
            // 
            this.buttonAcquireToken.Location = new System.Drawing.Point(12, 314);
            this.buttonAcquireToken.Name = "buttonAcquireToken";
            this.buttonAcquireToken.Size = new System.Drawing.Size(138, 34);
            this.buttonAcquireToken.TabIndex = 37;
            this.buttonAcquireToken.Text = "Acquire token";
            this.buttonAcquireToken.UseVisualStyleBackColor = true;
            this.buttonAcquireToken.Click += new System.EventHandler(this.buttonAcquireToken_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(738, 315);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 32);
            this.buttonClose.TabIndex = 50;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(278, 395);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(252, 26);
            this.textBox1.TabIndex = 51;
            this.textBox1.Text = "http://localhost/SOAPe";
            // 
            // FormAzureApplicationRegistration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(830, 359);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.buttonAcquireToken);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.groupBoxAuth);
            this.Controls.Add(this.groupBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAzureApplicationRegistration";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Azure Application Registration";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBoxAuth.ResumeLayout(false);
            this.groupBoxAuth.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBoxTenantId;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxApplicationId;
        private System.Windows.Forms.GroupBox groupBoxAuth;
        private System.Windows.Forms.TextBox textBoxAuthCertificate;
        private System.Windows.Forms.Button buttonLoadCertificate;
        private System.Windows.Forms.RadioButton radioButtonAuthWithCertificate;
        private System.Windows.Forms.RadioButton radioButtonAuthWithClientSecret;
        private System.Windows.Forms.TextBox textBoxClientSecret;
        private System.Windows.Forms.Button buttonAcquireToken;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.RadioButton radioButtonAuthAsNativeApp;
        private System.Windows.Forms.TextBox textBoxResourceUrl;
        private System.Windows.Forms.TextBox textBox1;
    }
}