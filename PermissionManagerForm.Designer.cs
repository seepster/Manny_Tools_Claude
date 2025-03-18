namespace Manny_Tools_Claude
{
    partial class PermissionManagerForm
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblSelectUser = new System.Windows.Forms.Label();
            this.cmbUsers = new System.Windows.Forms.ComboBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.lstPermissions = new System.Windows.Forms.CheckedListBox();
            this.panelColumns = new System.Windows.Forms.Panel();
            this.lblColumnOptions = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();

            // 
            // Form Layout
            // 
            this.Text = "Manage Permissions";
            this.Size = new System.Drawing.Size(700, 650);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // 
            // lblTitle
            // 
            this.lblTitle.Text = "User Permissions Management";
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.Size = new System.Drawing.Size(400, 30);

            // 
            // lblSelectUser
            // 
            this.lblSelectUser.Text = "Select User:";
            this.lblSelectUser.Location = new System.Drawing.Point(20, 60);
            this.lblSelectUser.Size = new System.Drawing.Size(100, 20);

            // 
            // cmbUsers
            // 
            this.cmbUsers.Location = new System.Drawing.Point(130, 60);
            this.cmbUsers.Size = new System.Drawing.Size(200, 25);
            this.cmbUsers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            // 
            // lblDescription
            // 
            this.lblDescription.Text = "Select which features the user can access:";
            this.lblDescription.Location = new System.Drawing.Point(20, 95);
            this.lblDescription.Size = new System.Drawing.Size(400, 20);

            // 
            // lstPermissions
            // 
            this.lstPermissions.Location = new System.Drawing.Point(20, 125);
            this.lstPermissions.Size = new System.Drawing.Size(300, 150);
            this.lstPermissions.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstPermissions.CheckOnClick = true;

            // 
            // panelColumns
            // 
            this.panelColumns.Location = new System.Drawing.Point(340, 125);
            this.panelColumns.Size = new System.Drawing.Size(320, 300);
            this.panelColumns.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelColumns.Visible = false;

            // 
            // lblColumnOptions
            // 
            this.lblColumnOptions.Text = "Select visible columns for Stock On Hand:";
            this.lblColumnOptions.Location = new System.Drawing.Point(10, 10);
            this.lblColumnOptions.Size = new System.Drawing.Size(290, 20);
            this.lblColumnOptions.Font = new System.Drawing.Font("Segoe UI", 9, System.Drawing.FontStyle.Bold);

            // 
            // btnSave
            // 
            this.btnSave.Text = "Save Permissions";
            this.btnSave.Location = new System.Drawing.Point(220, 550);
            this.btnSave.Size = new System.Drawing.Size(150, 40);
            this.btnSave.DialogResult = System.Windows.Forms.DialogResult.OK;

            // 
            // btnCancel
            // 
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Location = new System.Drawing.Point(380, 550);
            this.btnCancel.Size = new System.Drawing.Size(100, 40);
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;

            // 
            // Add controls to form
            // 
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblSelectUser);
            this.Controls.Add(this.cmbUsers);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.lstPermissions);
            this.Controls.Add(this.panelColumns);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);

            // 
            // Set accept and cancel buttons
            // 
            this.AcceptButton = this.btnSave;
            this.CancelButton = this.btnCancel;

            // Initialize components
            this.components = new System.ComponentModel.Container();
        }

        #endregion

        // Declare all form controls as private fields
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSelectUser;
        private System.Windows.Forms.ComboBox cmbUsers;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.CheckedListBox lstPermissions;
        private System.Windows.Forms.Panel panelColumns;
        private System.Windows.Forms.Label lblColumnOptions;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
    }
}