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
        private Button btnUserManagement;

        // User authentication result
        public UserType AuthenticatedUserType { get; private set; }
        public string AuthenticatedUsername { get; private set; }
        public bool IsDefaultPassword { get; private set; }

        // Event to notify that user has logged in
        public event EventHandler<UserAuthenticatedEventArgs> UserAuthenticated;

        public LoginForm()
        {
            InitializeComponent();

            // Reset user authentication properties
            AuthenticatedUserType = UserType.None;
            AuthenticatedUsername = string.Empty;
            IsDefaultPassword = false;

            // Reload all configuration data
            ReloadConfigurationData();

            // Create default users if they don't exist
            CreateDefaultUsers();
        }

        /// <summary>
        /// Reloads all configuration data when the login form is shown
        /// </summary>
        private void ReloadConfigurationData()
        {
            try
            {
                // Reset singletons to force reloading of configuration data
                ResetSingletons();

                // Re-initialize permissions
                UserPermissions permissions = new UserPermissions();

                // Check connection status to ensure it's refreshed
                ConnectionStatusManager.Instance.CheckConnection(string.Empty);

                // Clear database connection manager
                DatabaseConnectionManager.Instance.LoadConnectionString();
            }
            catch (Exception ex)
            {
                // Log error but continue - not critical for login
                Console.WriteLine($"Error reloading configuration: {ex.Message}");
            }
        }

        /// <summary>
        /// Resets singleton instances to force configuration reload
        /// </summary>
        private void ResetSingletons()
        {
            // Use reflection to reset singleton instances
            // This is a bit of a hack but necessary to force reloading of configuration
            try
            {
                // Reset ConnectionStatusManager singleton
                var fieldInfo = typeof(ConnectionStatusManager).GetField("_instance",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
                fieldInfo?.SetValue(null, null);

                // Reset DatabaseConnectionManager singleton
                fieldInfo = typeof(DatabaseConnectionManager).GetField("_instance",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
                fieldInfo?.SetValue(null, null);
            }
            catch
            {
                // Ignore errors - this is just an attempt to force refresh
            }
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
                Location = new Point(160, 170),
                Size = new Size(100, 30)
            };
            btnLogin.Click += BtnLogin_Click;

            btnCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(270, 170),
                Size = new Size(80, 30)
            };
            btnCancel.Click += BtnCancel_Click;

            // User Management button (Replace the old separate buttons)
            btnUserManagement = new Button
            {
                Text = "User Management",
                Location = new Point(50, 240),
                Size = new Size(300, 30)
            };
            btnUserManagement.Click += BtnUserManagement_Click;

            // Add controls to the form
            this.Controls.Add(lblTitle);
            this.Controls.Add(lblUsername);
            this.Controls.Add(txtUsername);
            this.Controls.Add(lblPassword);
            this.Controls.Add(txtPassword);
            this.Controls.Add(btnLogin);
            this.Controls.Add(btnCancel);
            this.Controls.Add(btnUserManagement);

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

                    // Use the user management form to change password
                    using (var userManagementForm = new UserManagementForm(txtUsername.Text, userType))
                    {
                        userManagementForm.ShowDialog();
                    }
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

        private void BtnUserManagement_Click(object sender, EventArgs e)
        {
            // Verify superuser credentials first
            if (ValidateCredentials(txtUsername.Text, txtPassword.Text, out UserType userType, out _) &&
                userType == UserType.SuperUser)
            {
                // Authenticated as superuser, show the user management form
                using (var userManagementForm = new UserManagementForm(txtUsername.Text, userType))
                {
                    userManagementForm.ShowDialog();
                }
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

                // Read and decrypt the users file
                string[] lines = DataEncryptionHelper.ReadEncryptedLines(usersFile);
                if (lines == null)
                    return false;

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

                    // Write encrypted data to file
                    DataEncryptionHelper.WriteEncryptedLines(usersFile, new[] { superUserLine, standardUserLine });
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

            return Path.Combine(appDataPath, DataEncryptionHelper.ConfigFiles.UsersFile);
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