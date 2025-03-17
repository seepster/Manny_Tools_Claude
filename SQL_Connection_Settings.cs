using System;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using Microsoft.Data.SqlClient;

namespace Manny_Tools_Claude
{
    public partial class SQL_Connection_Settings : Form
    {
        // Standard timeout (5 seconds)
        private const int CONNECTION_TIMEOUT_SECONDS = 5;

        // Event to notify that connection settings are saved
        public event EventHandler<ConnectionSettingsEventArgs> ConnectionSettingsSaved;

        public SQL_Connection_Settings()
        {
            InitializeComponent();
            SetupEventHandlers();
            LoadSavedSettings();
        }

        private void SetupEventHandlers()
        {
            chkIntegratedSecurity.CheckedChanged += ChkIntegratedSecurity_CheckedChanged;
            btnTest.Click += BtnTest_Click;
            btnSave.Click += BtnSave_Click;
            btnCancel.Click += BtnCancel_Click;
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

                if (string.IsNullOrWhiteSpace(txtPassword.Text))
                {
                    MessageBox.Show("Please enter a password.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPassword.Focus();
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
            catch (Exception ex)
            {
                // Optionally log the detailed exception for debugging
                System.Diagnostics.Debug.WriteLine($"Connection Test Failed: {ex.Message}");
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
                        txtUsername.Text = string.Empty;
                        txtPassword.Text = string.Empty;
                    }
                    else
                    {
                        chkIntegratedSecurity.Checked = false;
                        txtUsername.Text = builder.UserID;
                        txtPassword.Text = string.Empty; // Do not store passwords
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