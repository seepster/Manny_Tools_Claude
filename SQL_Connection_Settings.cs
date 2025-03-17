using System;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.Data.SqlClient;
using System.IO;

namespace Manny_Tools_Claude
{
    public partial class SQL_Connection_Settings : Form
    {
        // Form controls
        private Label lblTitle;
        private Label lblServer;
        private TextBox txtServer;
        private Label lblDatabase;
        private TextBox txtDatabase;
        private Label lblUsername;
        private TextBox txtUsername;
        private Label lblPassword;
        private TextBox txtPassword;
        private CheckBox chkIntegratedSecurity;
        private Button btnTest;
        private Button btnSave;
        private Button btnCancel;
        private Label lblStatus;

        // Standard timeout (5 seconds)
        private const int CONNECTION_TIMEOUT_SECONDS = 5;

        // Event to notify that connection settings are saved
        public event EventHandler<ConnectionSettingsEventArgs> ConnectionSettingsSaved;

        public SQL_Connection_Settings()
        {
            InitializeComponent();
            LoadSavedSettings();
        }

        private void InitializeComponent()
        {
            this.Text = "Database Connection Settings";
            this.Size = new Size(450, 400);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Create title
            lblTitle = new Label
            {
                Text = "SQL Server Connection Settings",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Location = new Point(20, 20),
                Size = new Size(400, 30)
            };

            // Server settings
            lblServer = new Label
            {
                Text = "Server Name/IP:",
                Location = new Point(20, 70),
                Size = new Size(120, 23)
            };

            txtServer = new TextBox
            {
                Location = new Point(150, 70),
                Size = new Size(250, 23)
            };

            // Database settings
            lblDatabase = new Label
            {
                Text = "Database Name:",
                Location = new Point(20, 110),
                Size = new Size(120, 23)
            };

            txtDatabase = new TextBox
            {
                Location = new Point(150, 110),
                Size = new Size(250, 23)
            };

            // Authentication settings
            chkIntegratedSecurity = new CheckBox
            {
                Text = "Use Windows Authentication",
                Location = new Point(150, 150),
                Size = new Size(250, 23),
                Checked = true
            };
            chkIntegratedSecurity.CheckedChanged += ChkIntegratedSecurity_CheckedChanged;

            // Username settings
            lblUsername = new Label
            {
                Text = "Username:",
                Location = new Point(20, 180),
                Size = new Size(120, 23),
                Enabled = false
            };

            txtUsername = new TextBox
            {
                Location = new Point(150, 180),
                Size = new Size(250, 23),
                Enabled = false
            };

            // Password settings
            lblPassword = new Label
            {
                Text = "Password:",
                Location = new Point(20, 220),
                Size = new Size(120, 23),
                Enabled = false
            };

            txtPassword = new TextBox
            {
                Location = new Point(150, 220),
                Size = new Size(250, 23),
                PasswordChar = '*',
                Enabled = false
            };

            // Status label (for displaying connection test results)
            lblStatus = new Label
            {
                Location = new Point(20, 260),
                Size = new Size(380, 40),
                TextAlign = ContentAlignment.MiddleLeft,
                ForeColor = Color.DarkBlue
            };

            // Buttons
            btnTest = new Button
            {
                Text = "Test Connection",
                Location = new Point(20, 310),
                Size = new Size(120, 30)
            };
            btnTest.Click += BtnTest_Click;

            btnSave = new Button
            {
                Text = "Save & Connect",
                Location = new Point(150, 310),
                Size = new Size(120, 30)
            };
            btnSave.Click += BtnSave_Click;

            btnCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(280, 310),
                Size = new Size(120, 30),
                DialogResult = DialogResult.Cancel
            };
            btnCancel.Click += BtnCancel_Click;

            // Add all controls to the form
            this.Controls.Add(lblTitle);
            this.Controls.Add(lblServer);
            this.Controls.Add(txtServer);
            this.Controls.Add(lblDatabase);
            this.Controls.Add(txtDatabase);
            this.Controls.Add(chkIntegratedSecurity);
            this.Controls.Add(lblUsername);
            this.Controls.Add(txtUsername);
            this.Controls.Add(lblPassword);
            this.Controls.Add(txtPassword);
            this.Controls.Add(lblStatus);
            this.Controls.Add(btnTest);
            this.Controls.Add(btnSave);
            this.Controls.Add(btnCancel);

            // Set accept and cancel buttons
            this.AcceptButton = btnSave;
            this.CancelButton = btnCancel;
        }

        private void ChkIntegratedSecurity_CheckedChanged(object sender, EventArgs e)
        {
            bool useSqlAuth = !chkIntegratedSecurity.Checked;
            lblUsername.Enabled = useSqlAuth;
            txtUsername.Enabled = useSqlAuth;
            lblPassword.Enabled = useSqlAuth;
            txtPassword.Enabled = useSqlAuth;
        }

        private void BtnTest_Click(object sender, EventArgs e)
        {
            if (ValidateInputs())
            {
                string connectionString = BuildConnectionString();

                lblStatus.Text = "Testing connection...";
                lblStatus.ForeColor = Color.DarkBlue;
                btnTest.Enabled = false;
                btnSave.Enabled = false;
                Application.DoEvents();

                bool connectionSuccessful = TestConnection(connectionString);

                btnTest.Enabled = true;
                btnSave.Enabled = true;

                if (connectionSuccessful)
                {
                    lblStatus.Text = "Connection successful!";
                    lblStatus.ForeColor = Color.Green;
                }
                else
                {
                    lblStatus.Text = "Connection failed. Please check your settings and try again.";
                    lblStatus.ForeColor = Color.Red;
                }
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (ValidateInputs())
            {
                string connectionString = BuildConnectionString();

                lblStatus.Text = "Testing connection before saving...";
                lblStatus.ForeColor = Color.DarkBlue;
                btnTest.Enabled = false;
                btnSave.Enabled = false;
                Application.DoEvents();

                bool connectionSuccessful = TestConnection(connectionString);

                if (connectionSuccessful)
                {
                    lblStatus.Text = "Connection successful! Saving settings...";
                    lblStatus.ForeColor = Color.Green;
                    Application.DoEvents();

                    // Update and save connection string with the manager
                    if (DatabaseConnectionManager.Instance.UpdateConnectionString(connectionString))
                    {
                        // Notify listeners
                        ConnectionSettingsSaved?.Invoke(this, new ConnectionSettingsEventArgs(connectionString));

                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        lblStatus.Text = "Failed to save connection settings. Please try again.";
                        lblStatus.ForeColor = Color.Red;
                        btnTest.Enabled = true;
                        btnSave.Enabled = true;
                    }
                }
                else
                {
                    lblStatus.Text = "Connection failed. Please check your settings and try again.";
                    lblStatus.ForeColor = Color.Red;
                    btnTest.Enabled = true;
                    btnSave.Enabled = true;
                }
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtServer.Text))
            {
                MessageBox.Show("Please enter a server name or IP address.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtServer.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtDatabase.Text))
            {
                MessageBox.Show("Please enter a database name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtDatabase.Focus();
                return false;
            }

            if (!chkIntegratedSecurity.Checked)
            {
                if (string.IsNullOrWhiteSpace(txtUsername.Text))
                {
                    MessageBox.Show("Please enter a username.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtUsername.Focus();
                    return false;
                }
            }

            return true;
        }

        private string BuildConnectionString()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder
            {
                DataSource = txtServer.Text,
                InitialCatalog = txtDatabase.Text,
                TrustServerCertificate = true,
                ConnectTimeout = CONNECTION_TIMEOUT_SECONDS // Set 5-second timeout
            };

            if (chkIntegratedSecurity.Checked)
            {
                builder.IntegratedSecurity = true;
            }
            else
            {
                builder.UserID = txtUsername.Text;
                builder.Password = txtPassword.Text;
            }

            return builder.ConnectionString;
        }

        private bool TestConnection(string connectionString)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        private void LoadSavedSettings()
        {
            try
            {
                // First try to get connection string from the manager
                string connectionString = DatabaseConnectionManager.Instance.ConnectionString;

                // If that's empty, try to load directly from file
                if (string.IsNullOrEmpty(connectionString))
                {
                    string appDataPath = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                        "MannyTools");

                    string configPath = Path.Combine(appDataPath, "connection.cfg");

                    if (File.Exists(configPath))
                    {
                        // Read connection string
                        connectionString = File.ReadAllText(configPath);
                    }
                }

                if (!string.IsNullOrEmpty(connectionString))
                {
                    SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);

                    txtServer.Text = builder.DataSource;
                    txtDatabase.Text = builder.InitialCatalog;

                    if (builder.IntegratedSecurity)
                    {
                        chkIntegratedSecurity.Checked = true;
                    }
                    else
                    {
                        chkIntegratedSecurity.Checked = false;
                        txtUsername.Text = builder.UserID;
                        txtPassword.Text = builder.Password;
                    }

                    // Display informational message
                    lblStatus.Text = "Loaded existing connection settings";
                    lblStatus.ForeColor = Color.DarkBlue;
                }
            }
            catch (Exception ex)
            {
                // Log the error but continue with default/empty values
                lblStatus.Text = "Could not load saved settings";
                lblStatus.ForeColor = Color.Red;
                System.Diagnostics.Debug.WriteLine($"Error loading saved settings: {ex.Message}");
            }
        }
    }

    // EventArgs class to pass connection string information
    public class ConnectionSettingsEventArgs : EventArgs
    {
        public string ConnectionString { get; private set; }

        public ConnectionSettingsEventArgs(string connectionString)
        {
            ConnectionString = connectionString;
        }
    }
}