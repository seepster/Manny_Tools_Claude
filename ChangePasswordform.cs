using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Manny_Tools_Claude
{
    public partial class ChangePasswordForm : Form
    {
        private Label lblTitle;
        private Label lblNewPassword;
        private TextBox txtNewPassword;
        private Label lblConfirmPassword;
        private TextBox txtConfirmPassword;
        private Button btnSave;
        private Button btnCancel;

        private string _username;
        private bool _requiredChange;

        public ChangePasswordForm(string username, string userDisplayName, bool requiredChange)
        {
            _username = username;
            _requiredChange = requiredChange;

            InitializeComponent();
            this.Text = $"Change {userDisplayName} Password";
            lblTitle.Text = $"Change {userDisplayName} Password";

            if (_requiredChange)
            {
                this.ControlBox = false; // No close button if change is required
                btnCancel.Text = "Exit Application";
            }
        }

        private void InitializeComponent()
        {
            this.Size = new Size(400, 300);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Create title
            lblTitle = new Label
            {
                Text = "Change Password",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(20, 20),
                Size = new Size(350, 30)
            };

            // New Password field
            lblNewPassword = new Label
            {
                Text = "New Password:",
                Location = new Point(50, 80),
                Size = new Size(120, 23)
            };

            txtNewPassword = new TextBox
            {
                Location = new Point(170, 80),
                Size = new Size(180, 23),
                PasswordChar = '*'
            };

            // Confirm Password field
            lblConfirmPassword = new Label
            {
                Text = "Confirm Password:",
                Location = new Point(50, 120),
                Size = new Size(120, 23)
            };

            txtConfirmPassword = new TextBox
            {
                Location = new Point(170, 120),
                Size = new Size(180, 23),
                PasswordChar = '*'
            };

            // Buttons
            btnSave = new Button
            {
                Text = "Save Password",
                Location = new Point(170, 180),
                Size = new Size(120, 30)
            };
            btnSave.Click += BtnSave_Click;

            btnCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(300, 180),
                Size = new Size(80, 30)
            };
            btnCancel.Click += BtnCancel_Click;

            // Add controls to form
            this.Controls.Add(lblTitle);
            this.Controls.Add(lblNewPassword);
            this.Controls.Add(txtNewPassword);
            this.Controls.Add(lblConfirmPassword);
            this.Controls.Add(txtConfirmPassword);
            this.Controls.Add(btnSave);
            this.Controls.Add(btnCancel);

            // Set accept and cancel buttons
            this.AcceptButton = btnSave;
            this.CancelButton = btnCancel;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                try
                {
                    // Update the password
                    UpdateUserPassword(_username, txtNewPassword.Text);

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error changing password: {ex.Message}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            if (_requiredChange)
            {
                if (MessageBox.Show("Exit application without changing password?",
                    "Confirm Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Application.Exit();
                }
            }
            else
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        private bool ValidateInput()
        {
            // Check if password is empty
            if (string.IsNullOrWhiteSpace(txtNewPassword.Text))
            {
                MessageBox.Show("Password cannot be empty.",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtNewPassword.Focus();
                return false;
            }

            // Check if passwords match
            if (txtNewPassword.Text != txtConfirmPassword.Text)
            {
                MessageBox.Show("Passwords do not match.",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtConfirmPassword.Text = "";
                txtConfirmPassword.Focus();
                return false;
            }

            return true;
        }

        private void UpdateUserPassword(string username, string newPassword)
        {
            string usersFile = LoginForm.GetUsersFilePath();
            if (!File.Exists(usersFile))
            {
                throw new Exception("Users file not found.");
            }

            List<string> lines = new List<string>();
            bool userFound = false;

            foreach (string line in File.ReadAllLines(usersFile))
            {
                string[] parts = line.Split('|');
                if (parts.Length >= 4 && parts[0] == username)
                {
                    // Update password and mark as not default (last parameter = 0)
                    string updatedLine = $"{parts[0]}|{LoginForm.HashPassword(newPassword)}|{parts[2]}|0";
                    lines.Add(updatedLine);
                    userFound = true;
                }
                else
                {
                    lines.Add(line);
                }
            }

            if (!userFound)
            {
                throw new Exception("User not found.");
            }

            File.WriteAllLines(usersFile, lines.ToArray());
        }
    }
}