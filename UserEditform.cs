using System;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

namespace Manny_Tools_Claude
{
    public partial class UserEditForm : Form
    {
        private string _username;
        private bool _isNewUser;

        private Label lblTitle;
        private Label lblUsername;
        private TextBox txtUsername;
        private Label lblPassword;
        private TextBox txtPassword;
        private Label lblConfirmPassword;
        private TextBox txtConfirmPassword;
        private ComboBox cmbUserType;
        private Label lblUserType;
        private Button btnSave;
        private Button btnCancel;

        public UserEditForm(string username)
        {
            _username = username;
            _isNewUser = string.IsNullOrEmpty(username);

            InitializeComponent();

            if (!_isNewUser)
            {
                LoadUserData();
            }
        }

        private void InitializeComponent()
        {
            this.Text = _isNewUser ? "Add User" : "Edit User";
            this.Size = new Size(400, 350);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Create title
            lblTitle = new Label
            {
                Text = _isNewUser ? "Add New User" : "Edit User",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(20, 20),
                Size = new Size(350, 30)
            };

            // Username field
            lblUsername = new Label
            {
                Text = "Username:",
                Location = new Point(30, 70),
                Size = new Size(100, 23)
            };

            txtUsername = new TextBox
            {
                Location = new Point(140, 70),
                Size = new Size(200, 23),
                Enabled = _isNewUser // Only enable for new users
            };

            // Password field
            lblPassword = new Label
            {
                Text = "Password:",
                Location = new Point(30, 110),
                Size = new Size(100, 23)
            };

            txtPassword = new TextBox
            {
                Location = new Point(140, 110),
                Size = new Size(200, 23),
                PasswordChar = '*'
            };

            // Confirm Password field
            lblConfirmPassword = new Label
            {
                Text = "Confirm Password:",
                Location = new Point(30, 150),
                Size = new Size(110, 23)
            };

            txtConfirmPassword = new TextBox
            {
                Location = new Point(140, 150),
                Size = new Size(200, 23),
                PasswordChar = '*'
            };

            // User Type dropdown
            lblUserType = new Label
            {
                Text = "User Type:",
                Location = new Point(30, 190),
                Size = new Size(100, 23)
            };

            cmbUserType = new ComboBox
            {
                Location = new Point(140, 190),
                Size = new Size(200, 23),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            // Add user types
            cmbUserType.Items.Add(UserType.StandardUser);
            cmbUserType.Items.Add(UserType.SuperUser);
            cmbUserType.SelectedIndex = 0; // Default to standard user

            // Buttons
            btnSave = new Button
            {
                Text = "Save",
                Location = new Point(140, 240),
                Size = new Size(100, 30),
                DialogResult = DialogResult.OK
            };
            btnSave.Click += BtnSave_Click;

            btnCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(250, 240),
                Size = new Size(100, 30),
                DialogResult = DialogResult.Cancel
            };

            // Add controls to form
            this.Controls.Add(lblTitle);
            this.Controls.Add(lblUsername);
            this.Controls.Add(txtUsername);
            this.Controls.Add(lblPassword);
            this.Controls.Add(txtPassword);
            this.Controls.Add(lblConfirmPassword);
            this.Controls.Add(txtConfirmPassword);
            this.Controls.Add(lblUserType);
            this.Controls.Add(cmbUserType);
            this.Controls.Add(btnSave);
            this.Controls.Add(btnCancel);

            // Set accept and cancel buttons
            this.AcceptButton = btnSave;
            this.CancelButton = btnCancel;
        }

        private void LoadUserData()
        {
            try
            {
                string usersFile = LoginForm.GetUsersFilePath();
                if (File.Exists(usersFile))
                {
                    foreach (string line in File.ReadAllLines(usersFile))
                    {
                        string[] parts = line.Split('|');
                        if (parts.Length >= 4 && parts[0] == _username)
                        {
                            txtUsername.Text = parts[0];

                            // Don't load the password hash
                            txtPassword.Text = string.Empty;
                            txtConfirmPassword.Text = string.Empty;

                            // Set user type
                            if (Enum.TryParse(parts[2], out UserType userType))
                            {
                                cmbUserType.SelectedItem = userType;
                            }
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading user data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                try
                {
                    SaveUser();
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving user: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool ValidateInput()
        {
            // Validate username
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("Username cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtUsername.Focus();
                return false;
            }

            // If new user or password is being changed
            if (_isNewUser || !string.IsNullOrEmpty(txtPassword.Text))
            {
                // Validate password
                if (string.IsNullOrWhiteSpace(txtPassword.Text))
                {
                    MessageBox.Show("Password cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPassword.Focus();
                    return false;
                }

                // Validate password confirmation
                if (txtPassword.Text != txtConfirmPassword.Text)
                {
                    MessageBox.Show("Passwords do not match.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtConfirmPassword.Focus();
                    return false;
                }
            }

            // Check for duplicate username if adding new user
            if (_isNewUser)
            {
                string usersFile = LoginForm.GetUsersFilePath();
                if (File.Exists(usersFile))
                {
                    foreach (string line in File.ReadAllLines(usersFile))
                    {
                        string[] parts = line.Split('|');
                        if (parts.Length >= 4 && parts[0] == txtUsername.Text)
                        {
                            MessageBox.Show("Username already exists. Please choose a different username.",
                                "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtUsername.Focus();
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        private void SaveUser()
        {
            string usersFile = LoginForm.GetUsersFilePath();
            string directory = Path.GetDirectoryName(usersFile);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            List<string> lines = new List<string>();
            bool userUpdated = false;

            // Get selected user type
            UserType selectedUserType = (UserType)cmbUserType.SelectedItem;

            if (File.Exists(usersFile))
            {
                foreach (string line in File.ReadAllLines(usersFile))
                {
                    string[] parts = line.Split('|');

                    if (parts.Length >= 4 && parts[0] == (_isNewUser ? txtUsername.Text : _username))
                    {
                        // Update existing user
                        if (!_isNewUser)
                        {
                            string passwordHash = string.IsNullOrEmpty(txtPassword.Text)
                                ? parts[1] // Keep existing password if not changed
                                : LoginForm.HashPassword(txtPassword.Text);

                            // Use default password flag (1) if password is changed, otherwise keep existing flag
                            string defaultPasswordFlag = string.IsNullOrEmpty(txtPassword.Text) ? parts[3] : "1";

                            string updatedLine = $"{parts[0]}|{passwordHash}|{(int)selectedUserType}|{defaultPasswordFlag}";
                            lines.Add(updatedLine);
                            userUpdated = true;
                        }
                    }
                    else
                    {
                        lines.Add(line);
                    }
                }
            }

            // Add new user if not updating existing one
            if (_isNewUser || !userUpdated)
            {
                string passwordHash = LoginForm.HashPassword(txtPassword.Text);
                string newLine = $"{txtUsername.Text}|{passwordHash}|{(int)selectedUserType}|1"; // 1 = default password
                lines.Add(newLine);
            }

            File.WriteAllLines(usersFile, lines.ToArray());
        }
    }
}