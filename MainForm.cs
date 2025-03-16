using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

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

        // Add to the constructor after other initialization
        private void SetupConnectionTimer()
        {
            _connectionCheckTimer = new System.Windows.Forms.Timer();
            _connectionCheckTimer.Interval = 60000; // Check every minute
            _connectionCheckTimer.Tick += ConnectionCheckTimer_Tick;
            _connectionCheckTimer.Start();
        }

        // Add this event handler to MainForm
        private void ConnectionCheckTimer_Tick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_connectionString))
            {
                // Use async version to avoid UI freezing
                _ = ConnectionStatusManager.Instance.CheckConnectionAsync(_connectionString);
            }
        }

        // Don't forget to clean up
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

        // Form controls
        private TabControl tabControl;
        private TabPage tabViewSQL;
        private TabPage tabCreateSizes;
        private TabPage tabStockOnHand;
        private TabPage tabOrbitSizing;
        private Button btnSettings;
        private Button btnUserManagement;
        private ToolStripStatusLabel lblStatus;
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
            }
        }

        private void InitializeComponent()
        {
            this.Text = "Manny Tools";
            this.Size = new Size(1200, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Padding = new Padding(5);
            this.BackColor = Color.FromArgb(240, 240, 240);
            this.Icon = SystemIcons.Application;

            // Create header panel with FlowLayout for better control positioning
            Panel headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.FromArgb(230, 230, 230),
                Padding = new Padding(10)
            };

            // Add title label
            Label lblTitle = new Label
            {
                Text = "Manny Tools",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Location = new Point(15, 15),
                AutoSize = true
            };
            headerPanel.Controls.Add(lblTitle);

            // Add settings button
            btnSettings = new Button
            {
                Text = "Connection Settings",
                Size = new Size(150, 30),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                FlatStyle = FlatStyle.Flat
            };
            btnSettings.FlatAppearance.BorderSize = 1;
            btnSettings.Click += BtnSettings_Click;
            headerPanel.Controls.Add(btnSettings);

            // Add user management button
            btnUserManagement = new Button
            {
                Text = "User Management",
                Size = new Size(150, 30),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
                // Location will be set in PositionHeaderButtons
            };
            btnUserManagement.Click += BtnUserManagement_Click;
            headerPanel.Controls.Add(btnUserManagement);

            // Create tab control
            tabControl = new TabControl
            {
                Dock = DockStyle.Fill,
                Padding = new Point(15, 8)
            };

            // Create SQL Viewer tab
            tabViewSQL = new TabPage
            {
                Text = "View SQL",
                Padding = new Padding(5)
            };

            // Create Sizes tab
            tabCreateSizes = new TabPage
            {
                Text = "Create Sizes",
                Padding = new Padding(5)
            };

            // Create Stock On Hand tab
            tabStockOnHand = new TabPage
            {
                Text = "Stock On Hand",
                Padding = new Padding(5)
            };

            // Create Orbit Sizing tab
            tabOrbitSizing = new TabPage
            {
                Text = "Orbit Sizing Method",
                Padding = new Padding(5)
            };

            // Add tabs to tab control (will be filtered based on permissions)
            tabControl.TabPages.Add(tabViewSQL);
            tabControl.TabPages.Add(tabCreateSizes);
            tabControl.TabPages.Add(tabStockOnHand);
            tabControl.TabPages.Add(tabOrbitSizing);

            // Create status bar
            StatusStrip statusStrip = new StatusStrip();
            lblStatus = new ToolStripStatusLabel
            {
                Text = $"Logged in as: {_currentUsername} | User Type: {_currentUserType.ToString()}"
            };
            statusStrip.Items.Add(lblStatus);

            // Add controls to form
            this.Controls.Add(tabControl);
            this.Controls.Add(headerPanel);
            this.Controls.Add(statusStrip);

            // Initialize content for each tab
            InitializeSQLViewer();
            InitializeCreateSizes();
            InitializeStockOnHand();
            InitializeOrbitSizing();

            // Handle form resize
            this.Resize += MainForm_Resize;
            this.SizeChanged += MainForm_SizeChanged;
        }

        private void InitializeSQLViewer()
        {
            sqlViewer = new SQL_Viewer_Schema(_connectionString)
            {
                Dock = DockStyle.Fill,
                Visible = true
            };

            tabViewSQL.Controls.Add(sqlViewer);
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
            stockOnHandForm = new StockOnHandForm(_connectionString)
            {
                Dock = DockStyle.Fill,
                Visible = true
            };

            tabStockOnHand.Controls.Add(stockOnHandForm);
        }

        private void InitializeOrbitSizing()
        {
            orbitSizingForm = new OrbitSizingMethodForm(_connectionString)
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

            // For standard user, apply permission-based access
            if (_currentUserType == UserType.StandardUser)
            {
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

                // Hide settings button for standard user
                btnSettings.Visible = false;
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

                string configPath = Path.Combine(appDataPath, "connection.config");

                if (File.Exists(configPath))
                {
                    _connectionString = File.ReadAllText(configPath);
                    ConnectionStatusManager.Instance.CheckConnection(_connectionString);
                    ConnectionStatusManager.Instance.ApplyButtonStyling(btnSettings);
                    return !string.IsNullOrEmpty(_connectionString);
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

            // Update Orbit Sizing form with new connection string
            if (orbitSizingForm != null)
            {
                orbitSizingForm.UpdateConnectionString(_connectionString);
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