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

            // Create default users if they don't exist
            CreateDefaultUsers();
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

                // Read users file
                string[] lines = File.ReadAllLines(usersFile);
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

                    // Write data to file
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