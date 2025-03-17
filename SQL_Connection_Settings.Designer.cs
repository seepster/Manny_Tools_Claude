namespace Manny_Tools_Claude
{
    partial class SQL_Connection_Settings
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
            lblTitle = new Label();
            lblServer = new Label();
            txtServer = new TextBox();
            lblDatabase = new Label();
            txtDatabase = new TextBox();
            lblUsername = new Label();
            txtUsername = new TextBox();
            lblPassword = new Label();
            txtPassword = new TextBox();
            chkIntegratedSecurity = new CheckBox();
            btnTest = new Button();
            btnSave = new Button();
            btnCancel = new Button();
            lblStatus = new Label();
            btnDiscover = new Button();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitle.Location = new Point(20, 20);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(319, 30);
            lblTitle.TabIndex = 1;
            lblTitle.Text = "SQL Server Connection Settings";
            // 
            // lblServer
            // 
            lblServer.Location = new Point(20, 70);
            lblServer.Name = "lblServer";
            lblServer.Size = new Size(120, 23);
            lblServer.TabIndex = 2;
            lblServer.Text = "Server Name/IP:";
            // 
            // txtServer
            // 
            txtServer.Location = new Point(150, 70);
            txtServer.Name = "txtServer";
            txtServer.Size = new Size(250, 23);
            txtServer.TabIndex = 3;
            // 
            // lblDatabase
            // 
            lblDatabase.Location = new Point(20, 110);
            lblDatabase.Name = "lblDatabase";
            lblDatabase.Size = new Size(120, 23);
            lblDatabase.TabIndex = 4;
            lblDatabase.Text = "Database Name:";
            // 
            // txtDatabase
            // 
            txtDatabase.Location = new Point(150, 110);
            txtDatabase.Name = "txtDatabase";
            txtDatabase.Size = new Size(250, 23);
            txtDatabase.TabIndex = 5;
            // 
            // lblUsername
            // 
            lblUsername.Enabled = false;
            lblUsername.Location = new Point(20, 180);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(120, 23);
            lblUsername.TabIndex = 7;
            lblUsername.Text = "Username:";
            // 
            // txtUsername
            // 
            txtUsername.Enabled = false;
            txtUsername.Location = new Point(150, 180);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(250, 23);
            txtUsername.TabIndex = 8;
            // 
            // lblPassword
            // 
            lblPassword.Enabled = false;
            lblPassword.Location = new Point(20, 220);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(120, 23);
            lblPassword.TabIndex = 9;
            lblPassword.Text = "Password:";
            // 
            // txtPassword
            // 
            txtPassword.Enabled = false;
            txtPassword.Location = new Point(150, 220);
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '*';
            txtPassword.Size = new Size(250, 23);
            txtPassword.TabIndex = 10;
            // 
            // chkIntegratedSecurity
            // 
            chkIntegratedSecurity.Checked = true;
            chkIntegratedSecurity.CheckState = CheckState.Checked;
            chkIntegratedSecurity.Location = new Point(150, 150);
            chkIntegratedSecurity.Name = "chkIntegratedSecurity";
            chkIntegratedSecurity.Size = new Size(250, 23);
            chkIntegratedSecurity.TabIndex = 6;
            chkIntegratedSecurity.Text = "Use Windows Authentication";
            // 
            // btnTest
            // 
            btnTest.Location = new Point(20, 310);
            btnTest.Name = "btnTest";
            btnTest.Size = new Size(120, 30);
            btnTest.TabIndex = 12;
            btnTest.Text = "Test Connection";
            // 
            // btnSave
            // 
            btnSave.DialogResult = DialogResult.OK;
            btnSave.Location = new Point(150, 310);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(120, 30);
            btnSave.TabIndex = 13;
            btnSave.Text = "Save & Connect";
            // 
            // btnCancel
            // 
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new Point(280, 310);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(120, 30);
            btnCancel.TabIndex = 14;
            btnCancel.Text = "Cancel";
            // 
            // lblStatus
            // 
            lblStatus.ForeColor = Color.DarkBlue;
            lblStatus.Location = new Point(20, 260);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(380, 40);
            lblStatus.TabIndex = 11;
            lblStatus.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // btnDiscover
            // 
            this.btnDiscover = new System.Windows.Forms.Button();
            this.btnDiscover.Text = "Discover Servers";
            this.btnDiscover.Location = new System.Drawing.Point(300, 70);
            this.btnDiscover.Size = new System.Drawing.Size(120, 30);
            this.btnDiscover.Name = "btnDiscover";
            this.btnDiscover.UseVisualStyleBackColor = true;
            // 
            // SQL_Connection_Settings
            // 
            AcceptButton = btnSave;
            CancelButton = btnCancel;
            ClientSize = new Size(491, 361);
            Controls.Add(lblTitle);
            Controls.Add(lblServer);
            Controls.Add(txtServer);
            Controls.Add(lblDatabase);
            Controls.Add(txtDatabase);
            Controls.Add(chkIntegratedSecurity);
            Controls.Add(lblUsername);
            Controls.Add(txtUsername);
            Controls.Add(lblPassword);
            Controls.Add(txtPassword);
            Controls.Add(lblStatus);
            Controls.Add(btnTest);
            Controls.Add(btnSave);
            Controls.Add(btnCancel);
            Controls.Add(btnDiscover);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "SQL_Connection_Settings";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Database Connection Settings";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        // Declare all form controls as private fields
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblServer;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.Label lblDatabase;
        private System.Windows.Forms.TextBox txtDatabase;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.CheckBox chkIntegratedSecurity;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button btnDiscover;
    }
}