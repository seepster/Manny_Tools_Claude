using System;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Manny_Tools_Claude
{
    public partial class LoginForm : Form
    {
        // Form controls
        private Label lblTitle;
        private Label lblUsername;
        private TextBox txtUsername;
        private Label lblPassword;
        private TextBox txtPassword;
        private Button btnLogin;
        private Button btnCancel;
        private Button btnChangeAdminPassword;
        private Button btnChangeUserPassword;

        // User authentication result
        public UserType AuthenticatedUserType { get; private set; }
        public string AuthenticatedUsername { get; private set; }
        public bool IsDefaultPassword { get; private set; }

        // Event to notify that user has logged in
        public event EventHandler<UserAuthenticatedEventArgs> UserAuthenticated;

        public LoginForm()
        {
            InitializeComponent();
            AuthenticatedUserType = UserType.None;
            AuthenticatedUsername = string.Empty;
            IsDefaultPassword = false;

            // Create default users if they don't exist
            CreateDefaultUsers();
        }

        private void InitializeComponent()
        {
            this.Text = "Manny Tools - Login";
            this.Size = new Size(400, 380);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Create title
            lblTitle = new Label
            {
                Text = "Manny Tools Login",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(20, 20),
                Size = new Size(350, 40)
            };

            // Username field
            lblUsername = new Label
            {
                Text = "Username:",
                Location = new Point(50, 80),
                Size = new Size(100, 23)
            };

            txtUsername = new TextBox
            {
                Location = new Point(160, 80),
                Size = new Size(180, 23)
            };

            // Password field
            lblPassword = new Label
            {
                Text = "Password:",
                Location = new Point(50, 120),
                Size = new Size(100, 23)
            };

            txtPassword = new TextBox
            {
                Location = new Point(160, 120),
                Size = new Size(180, 23),
                PasswordChar = '*'
            };

            // Buttons
            btnLogin = new Button
            {
                Text = "Login",
                Location = new Point(160, 180),
                Size = new Size(100, 30)
            };
            btnLogin.Click += BtnLogin_Click;

            btnCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(270, 180),
                Size = new Size(80, 30)
            };
            btnCancel.Click += BtnCancel_Click;

            // Change Admin Password button
            btnChangeAdminPassword = new Button
            {
                Text = "Change Admin Password",
                Location = new Point(50, 240),
                Size = new Size(300, 30)
            };
            btnChangeAdminPassword.Click += BtnChangeAdminPassword_Click;

            // Change User Password button
            btnChangeUserPassword = new Button
            {
                Text = "Change Standard User Password",
                Location = new Point(50, 280),
                Size = new Size(300, 30)
            };
            btnChangeUserPassword.Click += BtnChangeUserPassword_Click;

            // Add controls to the form
            this.Controls.Add(lblTitle);
            this.Controls.Add(lblUsername);
            this.Controls.Add(txtUsername);
            this.Controls.Add(lblPassword);
            this.Controls.Add(txtPassword);
            this.Controls.Add(btnLogin);
            this.Controls.Add(btnCancel);
            this.Controls.Add(btnChangeAdminPassword);
            this.Controls.Add(btnChangeUserPassword);

            // Set the accept and cancel buttons
            this.AcceptButton = btnLogin;
            this.CancelButton = btnCancel;
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            if (ValidateCredentials(txtUsername.Text, txtPassword.Text, out UserType userType, out bool isDefault))
            {
                AuthenticatedUserType = userType;
                AuthenticatedUsername = txtUsername.Text;
                IsDefaultPassword = isDefault;

                // If it's the admin with default password, require password change
                if (isDefault && userType == UserType.SuperUser)
                {
                    MessageBox.Show("You are using the default password. You must change it before proceeding.",
                        "Security Notice", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ChangePassword("admin", "Admin", true);
                }

                UserAuthenticated?.Invoke(this, new UserAuthenticatedEventArgs(txtUsername.Text, userType, isDefault));
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Text = string.Empty;
                txtPassword.Focus();
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void BtnChangeAdminPassword_Click(object sender, EventArgs e)
        {
            // Verify admin credentials first
            if (ValidateCredentials(txtUsername.Text, txtPassword.Text, out UserType userType, out _) &&
                userType == UserType.SuperUser)
            {
                ChangePassword("admin", "Admin", false);
            }
            else
            {
                MessageBox.Show("Please enter valid admin credentials first.",
                    "Authentication Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtUsername.Text = "admin";
                txtPassword.Text = string.Empty;
                txtPassword.Focus();
            }
        }

        private void BtnChangeUserPassword_Click(object sender, EventArgs e)
        {
            // Verify admin credentials first
            if (ValidateCredentials(txtUsername.Text, txtPassword.Text, out UserType userType, out _) &&
                userType == UserType.SuperUser)
            {
                ChangePassword("user", "Standard User", false);
            }
            else
            {
                MessageBox.Show("Please enter valid admin credentials first to change user password.",
                    "Authentication Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtUsername.Text = "admin";
                txtPassword.Text = string.Empty;
                txtPassword.Focus();
            }
        }

        private bool ValidateCredentials(string username, string password, out UserType userType, out bool isDefaultPassword)
        {
            userType = UserType.None;
            isDefaultPassword = false;

            try
            {
                string usersFile = GetUsersFilePath();
                if (!File.Exists(usersFile))
                {
                    return false;
                }

                string[] lines = File.ReadAllLines(usersFile);
                foreach (string line in lines)
                {
                    string[] parts = line.Split('|');
                    if (parts.Length >= 4 && parts[0] == username)
                    {
                        string storedPasswordHash = parts[1];
                        string hashedPassword = HashPassword(password);

                        // Check if using default password (part 3 is 1 for default, 0 for changed)
                        isDefaultPassword = parts[3] == "1";

                        if (storedPasswordHash == hashedPassword)
                        {
                            // Parse the user type
                            if (Enum.TryParse(parts[2], out UserType type))
                            {
                                userType = type;
                                return true;
                            }
                        }
                        break;
                    }
                }
            }
            catch
            {
                // If any error occurs during validation, authentication fails
                return false;
            }

            return false;
        }

        private bool ChangePassword(string username, string userDisplayName, bool requiredChange)
        {
            using (var changeForm = new ChangePasswordForm(username, userDisplayName, requiredChange))
            {
                if (changeForm.ShowDialog() == DialogResult.OK)
                {
                    MessageBox.Show($"{userDisplayName} password has been changed successfully.",
                        "Password Changed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return true;
                }
                else if (requiredChange)
                {
                    // If password change was required but canceled, exit application
                    Application.Exit();
                }
            }

            return false;
        }

        private void CreateDefaultUsers()
        {
            try
            {
                string usersFile = GetUsersFilePath();
                Directory.CreateDirectory(Path.GetDirectoryName(usersFile));

                // Check if the users file already exists
                if (!File.Exists(usersFile))
                {
                    // Create the default users - with the 4th parameter indicating default password (1=default, 0=changed)
                    string superUserLine = $"admin|{HashPassword("admin")}|{UserType.SuperUser}|1";
                    string standardUserLine = $"user|{HashPassword("user")}|{UserType.StandardUser}|1";

                    File.WriteAllLines(usersFile, new[] { superUserLine, standardUserLine });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating default users: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public static string GetUsersFilePath()
        {
            string appDataPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "MannyTools");

            return Path.Combine(appDataPath, "users.dat");
        }
    }

    public enum UserType
    {
        None,
        StandardUser,
        SuperUser
    }

    public class UserAuthenticatedEventArgs : EventArgs
    {
        public string Username { get; private set; }
        public UserType UserType { get; private set; }
        public bool IsDefaultPassword { get; private set; }

        public UserAuthenticatedEventArgs(string username, UserType userType, bool isDefaultPassword)
        {
            Username = username;
            UserType = userType;
            IsDefaultPassword = isDefaultPassword;
        }
    }
}