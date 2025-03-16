using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Manny_Tools_Claude
{
    public partial class UserManagementForm : Form
    {
        // Simple form with buttons laid out plainly
        private ListView lstUsers;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnChangePassword;
        private Button btnDelete;
        private Button btnClose;
        private Button btnPermissions;

        private string _currentUsername;
        private UserType _userType;

        public UserManagementForm(string username, UserType userType)
        {
            _currentUsername = username;
            _userType = userType;
            InitializeForm();
            LoadUsers();
        }

        private void InitializeForm()
        {
            // Basic form setup
            this.Text = "User Management";
            this.Size = new Size(600, 400);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Title
            Label lblTitle = new Label();
            lblTitle.Text = "User Management";
            lblTitle.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblTitle.Location = new Point(20, 20);
            lblTitle.Size = new Size(200, 30);
            this.Controls.Add(lblTitle);

            // User list
            lstUsers = new ListView();
            lstUsers.Location = new Point(20, 60);
            lstUsers.Size = new Size(550, 200);
            lstUsers.View = View.Details;
            lstUsers.FullRowSelect = true;
            lstUsers.Columns.Add("Username", 150);
            lstUsers.Columns.Add("User Type", 150);
            lstUsers.Columns.Add("Default Password", 150);
            lstUsers.SelectedIndexChanged += LstUsers_SelectedIndexChanged;
            this.Controls.Add(lstUsers);

            // Add User button
            btnAdd = new Button();
            btnAdd.Text = "Add User";
            btnAdd.Location = new Point(20, 270);
            btnAdd.Size = new Size(100, 30);
            btnAdd.Click += BtnAdd_Click;
            this.Controls.Add(btnAdd);

            // Edit User button
            btnEdit = new Button();
            btnEdit.Text = "Edit User";
            btnEdit.Location = new Point(130, 270);
            btnEdit.Size = new Size(100, 30);
            btnEdit.Enabled = false;
            btnEdit.Click += BtnEdit_Click;
            this.Controls.Add(btnEdit);

            // Change Password button
            btnChangePassword = new Button();
            btnChangePassword.Text = "Change Password";
            btnChangePassword.Location = new Point(240, 270);
            btnChangePassword.Size = new Size(120, 30);
            btnChangePassword.Enabled = false;
            btnChangePassword.Click += BtnChangePassword_Click;
            this.Controls.Add(btnChangePassword);

            // Delete User button
            btnDelete = new Button();
            btnDelete.Text = "Delete User";
            btnDelete.Location = new Point(370, 270);
            btnDelete.Size = new Size(100, 30);
            btnDelete.Enabled = false;
            btnDelete.Click += BtnDelete_Click;
            this.Controls.Add(btnDelete);

            // Permissions button
            btnPermissions = new Button();
            btnPermissions.Text = "Manage Permissions";
            btnPermissions.Location = new Point(20, 320);
            btnPermissions.Size = new Size(150, 30);
            btnPermissions.Click += BtnPermissions_Click;
            this.Controls.Add(btnPermissions);

            // Close button
            btnClose = new Button();
            btnClose.Text = "Close";
            btnClose.Location = new Point(470, 320);
            btnClose.Size = new Size(100, 30);
            btnClose.DialogResult = DialogResult.Cancel;
            this.Controls.Add(btnClose);

            // Set DialogResult
            this.CancelButton = btnClose;
        }

        private void LoadUsers()
        {
            lstUsers.Items.Clear();

            try
            {
                string usersFile = LoginForm.GetUsersFilePath();
                if (File.Exists(usersFile))
                {
                    string[] lines = DataEncryptionHelper.ReadEncryptedLines(usersFile);
                    if (lines != null)
                    {
                        foreach (string line in lines)
                        {
                            string[] parts = line.Split('|');
                            if (parts.Length >= 4)
                            {
                                ListViewItem item = new ListViewItem(parts[0]);

                                // User type
                                if (Enum.TryParse(parts[2], out UserType userType))
                                {
                                    item.SubItems.Add(userType.ToString());
                                }
                                else
                                {
                                    item.SubItems.Add("Unknown");
                                }

                                // Default password flag
                                bool isDefault = parts[3] == "1";
                                item.SubItems.Add(isDefault ? "Yes" : "No");

                                lstUsers.Items.Add(item);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading users: " + ex.Message);
            }
        }

        private void LstUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool hasSelection = lstUsers.SelectedItems.Count > 0;
            btnEdit.Enabled = hasSelection;
            btnChangePassword.Enabled = hasSelection;

            if (hasSelection)
            {
                string selectedUser = lstUsers.SelectedItems[0].Text;
                btnDelete.Enabled = selectedUser != "admin" && selectedUser != _currentUsername;
            }
            else
            {
                btnDelete.Enabled = false;
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            using (UserEditForm form = new UserEditForm(null))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadUsers();
                }
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (lstUsers.SelectedItems.Count > 0)
            {
                string username = lstUsers.SelectedItems[0].Text;
                using (UserEditForm form = new UserEditForm(username))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        LoadUsers();
                    }
                }
            }
        }

        private void BtnChangePassword_Click(object sender, EventArgs e)
        {
            if (lstUsers.SelectedItems.Count > 0)
            {
                string username = lstUsers.SelectedItems[0].Text;
                string userType = lstUsers.SelectedItems[0].SubItems[1].Text;
                string displayName = userType == "SuperUser" ? "Admin" : "Standard User";

                using (ChangePasswordForm form = new ChangePasswordForm(username, displayName, false))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        MessageBox.Show("Password has been changed successfully.");
                        LoadUsers();
                    }
                }
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (lstUsers.SelectedItems.Count > 0)
            {
                string username = lstUsers.SelectedItems[0].Text;

                if (username == "admin" || username == _currentUsername)
                {
                    MessageBox.Show("You cannot delete the admin account or your own account.");
                    return;
                }

                if (MessageBox.Show($"Are you sure you want to delete user '{username}'?",
                                  "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        string usersFile = LoginForm.GetUsersFilePath();
                        string[] lines = DataEncryptionHelper.ReadEncryptedLines(usersFile);

                        if (lines != null)
                        {
                            List<string> newLines = new List<string>();
                            foreach (string line in lines)
                            {
                                string[] parts = line.Split('|');
                                if (parts.Length < 4 || parts[0] != username)
                                {
                                    newLines.Add(line);
                                }
                            }

                            DataEncryptionHelper.WriteEncryptedLines(usersFile, newLines.ToArray());

                            // Try to delete permissions too
                            try
                            {
                                UserPermissions permissions = new UserPermissions();
                                permissions.DeleteUserPermissions(username);
                            }
                            catch { /* Not critical */ }

                            LoadUsers();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error deleting user: " + ex.Message);
                    }
                }
            }
        }

        private void BtnPermissions_Click(object sender, EventArgs e)
        {
            if (_userType != UserType.SuperUser)
            {
                MessageBox.Show("Only administrators can manage permissions.",
                    "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Get the currently selected user, or default to the first user if none selected
            string selectedUser = "user";
            if (lstUsers.SelectedItems.Count > 0)
            {
                selectedUser = lstUsers.SelectedItems[0].Text;
            }

            using (PermissionManagerForm form = new PermissionManagerForm(_currentUsername))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    // Refresh the current form to show the updated permissions
                    LoadUsers();
                }
            }
        }
    }
}