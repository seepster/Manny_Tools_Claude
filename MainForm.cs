using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace Manny_Tools_Claude
{
    public partial class MainForm : Form
    {
        #region Fields

        private string _connectionString;
        private UserType _currentUserType;
        private string _currentUsername;
        private bool _isDefaultPassword;
        private UserPermissions _permissionManager;

        //Timer to check connection status
        private System.Windows.Forms.Timer _connectionCheckTimer;

        // Form controls for content
        private SQL_Viewer_Schema sqlViewer;
        private CreateSizesForm sizesForm;
        private StockOnHandForm stockOnHandForm;
        private OrbitSizingMethodForm orbitSizingForm;

        #endregion

        #region Constructor & Initialization

        public MainForm(string username, UserType userType, bool isDefaultPassword)
        {
            _currentUsername = username;
            _currentUserType = userType;
            _isDefaultPassword = isDefaultPassword;
            _permissionManager = new UserPermissions();

            InitializeComponent();

            ConnectionStatusManager.Instance.ConnectionStatusChanged += ConnectionStatus_Changed;

            // Check if standard user has default password after admin has changed theirs
            if (_currentUserType == UserType.StandardUser && _isDefaultPassword)
            {
                if (MessageBox.Show("You are using the default standard user password. Would you like to change it now?",
                    "Security Recommendation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    using (var userManagementForm = new UserManagementForm(_currentUsername, _currentUserType))
                    {
                        userManagementForm.ShowDialog();
                    }
                }
            }
            SetupConnectionTimer();

            // Check if we have saved connection settings
            TryLoadConnectionString();

            // Adjust UI based on user permissions
            ConfigureUIForUserRole();

            // Position the buttons initially
            PositionHeaderButtons();

            // Check connection status
            if (!string.IsNullOrEmpty(_connectionString))
            {
                ConnectionStatusManager.Instance.CheckConnection(_connectionString);
                ConnectionStatusManager.Instance.ApplyButtonStyling(btnSettings);

                // Force SQL Viewer to initialize immediately if the tab exists and user has permission
                if (tabControl.TabPages.Contains(tabViewSQL) && sqlViewer != null)
                {
                    // Add a short delay to allow UI to initialize before loading tables
                    Timer initTimer = new Timer();
                    initTimer.Interval = 500; // Short delay to allow UI to initialize
                    initTimer.Tick += (s, e) => {
                        initTimer.Stop();
                        initTimer.Dispose();

                        // Make sure we have the latest connection string
                        sqlViewer.UpdateConnectionString(_connectionString);
                        // Explicitly load database tables
                        sqlViewer.LoadDatabaseTables();
                    };
                    initTimer.Start();
                }
            }
        }

        private void SetupConnectionTimer()
        {
            _connectionCheckTimer = new System.Windows.Forms.Timer();
            _connectionCheckTimer.Interval = 60000; // Check every minute
            _connectionCheckTimer.Tick += ConnectionCheckTimer_Tick;
            _connectionCheckTimer.Start();
        }

        private void ConnectionCheckTimer_Tick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_connectionString))
            {
                // Check connection status
                ConnectionStatusManager.Instance.CheckConnection(_connectionString);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_connectionCheckTimer != null)
                {
                    _connectionCheckTimer.Stop();
                    _connectionCheckTimer.Dispose();
                }

                // Unsubscribe from events
                if (ConnectionStatusManager.Instance != null)
                {
                    ConnectionStatusManager.Instance.ConnectionStatusChanged -= ConnectionStatus_Changed;
                }
            }

            base.Dispose(disposing);
        }

        private void InitializeSQLViewer()
        {
            sqlViewer = new SQL_Viewer_Schema(_connectionString)
            {
                Dock = DockStyle.Fill,
                Visible = true
            };

            tabViewSQL.Controls.Add(sqlViewer);

            // Force the SQL viewer to initialize if this is the selected tab
            if (tabControl.SelectedTab == tabViewSQL)
            {
                // First ensure that connection status is up to date
                ConnectionStatusManager.Instance.CheckConnection(_connectionString);

                // Send a fake visibility changed event by toggling visibility
                sqlViewer.Visible = false;
                sqlViewer.Visible = true;
            }
        }

        private void InitializeCreateSizes()
        {
            sizesForm = new CreateSizesForm()
            {
                Dock = DockStyle.Fill,
                Visible = true
            };

            tabCreateSizes.Controls.Add(sizesForm);
        }

        private void InitializeStockOnHand()
        {
            // Pass both connection string and current username to StockOnHandForm
            stockOnHandForm = new StockOnHandForm(_connectionString, _currentUsername)
            {
                Dock = DockStyle.Fill,
                Visible = true
            };

            tabStockOnHand.Controls.Add(stockOnHandForm);
        }

        private void InitializeOrbitSizing()
        {
            orbitSizingForm = new OrbitSizingMethodForm()
            {
                Dock = DockStyle.Fill,
                Visible = true
            };

            tabOrbitSizing.Controls.Add(orbitSizingForm);
        }

        // Dynamic positioning of buttons based on form size
        private void PositionHeaderButtons()
        {
            // Calculate positions based on available width
            int rightMargin = 15;
            int buttonSpacing = 10;
            int availableWidth = this.ClientSize.Width;

            // Position from right to left
            btnSettings.Location = new Point(availableWidth - btnSettings.Width - rightMargin, 15);
            btnUserManagement.Location = new Point(btnSettings.Left - btnUserManagement.Width - buttonSpacing, 15);
            btnLogout.Location = new Point(btnUserManagement.Left - btnLogout.Width - buttonSpacing, 15);
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            PositionHeaderButtons();
        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            PositionHeaderButtons();
        }

        private void ConfigureUIForUserRole()
        {
            // Hide user management button for standard users
            btnUserManagement.Visible = _currentUserType == UserType.SuperUser;

            // For all users, including superusers, apply permission-based access
            // This allows for customizing which tabs superusers see

            // Remove tabs based on permissions
            if (!_permissionManager.HasPermission(_currentUsername, UserPermissions.VIEW_SQL_TAB))
            {
                tabControl.TabPages.Remove(tabViewSQL);
            }

            if (!_permissionManager.HasPermission(_currentUsername, UserPermissions.CREATE_SIZES_TAB))
            {
                tabControl.TabPages.Remove(tabCreateSizes);
            }

            if (!_permissionManager.HasPermission(_currentUsername, UserPermissions.STOCK_ON_HAND_TAB))
            {
                tabControl.TabPages.Remove(tabStockOnHand);
            }

            if (!_permissionManager.HasPermission(_currentUsername, UserPermissions.ORBIT_SIZING_METHOD_TAB))
            {
                tabControl.TabPages.Remove(tabOrbitSizing);
            }

            // For standard users, hide settings button
            if (_currentUserType == UserType.StandardUser)
            {
                btnSettings.Visible = false;
            }
        }

        #endregion

        #region Logout Functionality

        // Event handler for logout button
        private void BtnLogout_Click(object sender, EventArgs e)
        {
            // Confirm logout
            if (MessageBox.Show("Are you sure you want to logout?",
                "Confirm Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // Close the main form with a special result to indicate logout
                this.DialogResult = DialogResult.Retry; // Using Retry to indicate logout operation
                this.Close();
            }
        }

        #endregion

        #region Settings Management

        private bool TryLoadConnectionString()
        {
            try
            {
                string appDataPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "MannyTools");

                string configPath = Path.Combine(appDataPath, "connection.cfg");

                if (File.Exists(configPath))
                {
                    // Read connection string
                    _connectionString = File.ReadAllText(configPath);

                    if (string.IsNullOrEmpty(_connectionString))
                    {
                        ConnectionStatusManager.Instance.CheckConnection(string.Empty);
                        ConnectionStatusManager.Instance.ApplyButtonStyling(btnSettings);
                        return false;
                    }

                    ConnectionStatusManager.Instance.CheckConnection(_connectionString);
                    ConnectionStatusManager.Instance.ApplyButtonStyling(btnSettings);
                    return true;
                }
            }
            catch
            {
                // Ignore errors - connection settings will be requested if needed
            }

            ConnectionStatusManager.Instance.CheckConnection(string.Empty);
            ConnectionStatusManager.Instance.ApplyButtonStyling(btnSettings);

            return false;
        }

        private void ShowConnectionSettings()
        {
            SQL_Connection_Settings settingsForm = new SQL_Connection_Settings();
            settingsForm.ConnectionSettingsSaved += SettingsForm_ConnectionSettingsSaved;

            settingsForm.ShowDialog();
        }

        private void SettingsForm_ConnectionSettingsSaved(object sender, ConnectionSettingsEventArgs e)
        {
            _connectionString = e.ConnectionString;

            // Check connection status with new connection string
            ConnectionStatusManager.Instance.CheckConnection(_connectionString);
            ConnectionStatusManager.Instance.ApplyButtonStyling(btnSettings);

            // Update SQL Viewer with new connection string
            if (sqlViewer != null)
            {
                sqlViewer.UpdateConnectionString(_connectionString);
            }

            // Update Stock On Hand form with new connection string
            if (stockOnHandForm != null)
            {
                stockOnHandForm.UpdateConnectionString(_connectionString);
            }
        }

        private void BtnSettings_Click(object sender, EventArgs e)
        {
            ShowConnectionSettings();
        }

        private void BtnUserManagement_Click(object sender, EventArgs e)
        {
            if (_currentUserType == UserType.SuperUser)
            {
                using (UserManagementForm userManagementForm = new UserManagementForm(_currentUsername, _currentUserType))
                {
                    if (userManagementForm.ShowDialog() == DialogResult.OK)
                    {
                        // Refresh any components that depend on user settings
                        if (stockOnHandForm != null)
                        {
                            stockOnHandForm.RefreshColumns();
                        }
                    }
                }
            }
        }

        private void ConnectionStatus_Changed(object sender, ConnectionStatusEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => ConnectionStatus_Changed(sender, e)));
                return;
            }

            ConnectionStatusManager.Instance.ApplyButtonStyling(btnSettings);
        }

        #endregion
    }
}