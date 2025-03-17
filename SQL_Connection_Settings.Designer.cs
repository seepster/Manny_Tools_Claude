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
            this.components = new System.ComponentModel.Container();

            // Form controls
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblServer = new System.Windows.Forms.Label();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.lblDatabase = new System.Windows.Forms.Label();
            this.txtDatabase = new System.Windows.Forms.TextBox();
            this.lblUsername = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.chkIntegratedSecurity = new System.Windows.Forms.CheckBox();
            this.btnTest = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();

            // 
            // Form Layout
            // 
            this.Text = "Database Connection Settings";
            this.Size = new System.Drawing.Size(450, 400);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // 
            // lblTitle
            // 
            this.lblTitle.Text = "SQL Server Connection Settings";
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.Size = new System.Drawing.Size(400, 30);

            // 
            // lblServer
            // 
            this.lblServer.Text = "Server Name/IP:";
            this.lblServer.Location = new System.Drawing.Point(20, 70);
            this.lblServer.Size = new System.Drawing.Size(120, 23);

            // 
            // txtServer
            // 
            this.txtServer.Location = new System.Drawing.Point(150, 70);
            this.txtServer.Size = new System.Drawing.Size(250, 23);

            // 
            // lblDatabase
            // 
            this.lblDatabase.Text = "Database Name:";
            this.lblDatabase.Location = new System.Drawing.Point(20, 110);
            this.lblDatabase.Size = new System.Drawing.Size(120, 23);

            // 
            // txtDatabase
            // 
            this.txtDatabase.Location = new System.Drawing.Point(150, 110);
            this.txtDatabase.Size = new System.Drawing.Size(250, 23);

            // 
            // chkIntegratedSecurity
            // 
            this.chkIntegratedSecurity.Text = "Use Windows Authentication";
            this.chkIntegratedSecurity.Location = new System.Drawing.Point(150, 150);
            this.chkIntegratedSecurity.Size = new System.Drawing.Size(250, 23);
            this.chkIntegratedSecurity.Checked = true;

            // 
            // lblUsername
            // 
            this.lblUsername.Text = "Username:";
            this.lblUsername.Location = new System.Drawing.Point(20, 180);
            this.lblUsername.Size = new System.Drawing.Size(120, 23);
            this.lblUsername.Enabled = false;

            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(150, 180);
            this.txtUsername.Size = new System.Drawing.Size(250, 23);
            this.txtUsername.Enabled = false;

            // 
            // lblPassword
            // 
            this.lblPassword.Text = "Password:";
            this.lblPassword.Location = new System.Drawing.Point(20, 220);
            this.lblPassword.Size = new System.Drawing.Size(120, 23);
            this.lblPassword.Enabled = false;

            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(150, 220);
            this.txtPassword.Size = new System.Drawing.Size(250, 23);
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Enabled = false;

            // 
            // lblStatus
            // 
            this.lblStatus.Location = new System.Drawing.Point(20, 260);
            this.lblStatus.Size = new System.Drawing.Size(380, 40);
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblStatus.ForeColor = System.Drawing.Color.DarkBlue;

            // 
            // btnTest
            // 
            this.btnTest.Text = "Test Connection";
            this.btnTest.Location = new System.Drawing.Point(20, 310);
            this.btnTest.Size = new System.Drawing.Size(120, 30);

            // 
            // btnSave
            // 
            this.btnSave.Text = "Save & Connect";
            this.btnSave.Location = new System.Drawing.Point(150, 310);
            this.btnSave.Size = new System.Drawing.Size(120, 30);
            this.btnSave.DialogResult = System.Windows.Forms.DialogResult.OK;

            // 
            // btnCancel
            // 
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Location = new System.Drawing.Point(280, 310);
            this.btnCancel.Size = new System.Drawing.Size(120, 30);
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;

            // 
            // Add controls to form
            // 
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblServer);
            this.Controls.Add(this.txtServer);
            this.Controls.Add(this.lblDatabase);
            this.Controls.Add(this.txtDatabase);
            this.Controls.Add(this.chkIntegratedSecurity);
            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);

            // 
            // Set accept and cancel buttons
            // 
            this.AcceptButton = this.btnSave;
            this.CancelButton = this.btnCancel;
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
    }
}