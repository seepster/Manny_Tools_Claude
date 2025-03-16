using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Manny_Tools_Claude
{
    public partial class UserManagementForm : Form
    {
        private TabControl tabControl;
        private TabPage tabUsers;
        private TabPage tabPermissions;
        private ListView lstUsers;
        private Button btnAddUser;
        private Button btnEditUser;
        private Button btnDeleteUser;
        private Button btnClose;
        private string _authenticatedUsername;
        private UserType _authenticatedUserType;

        // For permissions management
        private string _selectedUsername = string.Empty;
        private CheckedListBox lstPermissions;
        private Panel panelColumns;
        private Label lblColumnOptions;
        private Label lblPermissionsTitle;
        private Label lblUsersTitle;
        private Dictionary<int, CheckBox> columnCheckboxes = new Dictionary<int, CheckBox>();
        private UserPermissions _permissionManager;
        private List<string> _userPermissions;

        // Stock columns definitions
        private Dictionary<int, string> _columnMap = new Dictionary<int, string>
        {
            { 1, "PLU" },
            { 2, "Purchases" },
            { 3, "Claims" },
            { 4, "Sold" },
            { 5, "LayBuyStarted" },
            { 6, "LayBuyFinished" },
            { 7, "SOH" },
            { 8, "InLayBuy" },
            { 9, "Description" },
            { 10, "SellPrice1" }
        };

        private List<int> _visibleColumns = new List<int>();

        public UserManagementForm(string authenticatedUsername, UserType authenticatedUserType)
        {
            _authenticatedUsername = authenticatedUsername;
            _authenticatedUserType = authenticatedUserType;
            _permissionManager = new UserPermissions();

            InitializeComponent();
            LoadUsers();
        }

        private void InitializeComponent()
        {
            this.Text = "User Management";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Create tab control
            tabControl = new TabControl
            {
                Dock = DockStyle.Fill,
                Padding = new Point(15, 8)
            };

            // Create Users tab
            tabUsers = new TabPage
            {
                Text = "Users",
                Padding = new Padding(10)
            };

            // Create Permissions tab
            tabPermissions = new TabPage
            {
                Text = "Permissions",
                Padding = new Padding(10)
            };

            // Setup Users tab content
            SetupUsersTab();

            // Setup Permissions tab content
            SetupPermissionsTab();

            // Add tabs to tab control
            tabControl.TabPages.Add(tabUsers);
            tabControl.TabPages.Add(tabPermissions);

            // Add tab control to form
            this.Controls.Add(tabControl);

            // Close button at the bottom
            btnClose = new Button
            {
                Text = "Close",
                DialogResult = DialogResult.Cancel,
                Size = new Size(100, 30),
                Location = new Point(this.ClientSize.Width - 120, this.ClientSize.Height - 50)
            };
            this.Controls.Add(btnClose);

            // Handle tab changes
            tabControl.SelectedIndexChanged += TabControl_SelectedIndexChanged;

            // Set the form's cancel button
            this.CancelButton = btnClose;

            // Set a minimum size for the form
            this.MinimumSize = new Size(600, 500);

            // Handle form resize
            this.Resize += UserManagementForm_Resize;
        }

        private void SetupUsersTab()
        {
            // Title for Users tab
            lblUsersTitle = new Label
            {
                Text = "Manage Users",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Location = new Point(10, 10),
                Size = new Size(400, 30)
            };
            tabUsers.Controls.Add(lblUsersTitle);

            // Users list
            lstUsers = new ListView
            {
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                Location = new Point(10, 50),
                Size = new Size(550, 350),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom
            };
            lstUsers.Columns.Add("Username", 180);
            lstUsers.Columns.Add("User Type", 120);
            lstUsers.Columns.Add("Default Password", 120);
            lstUsers.SelectedIndexChanged += LstUsers_SelectedIndexChanged;
            tabUsers.Controls.Add(lstUsers);

            // Buttons
            btnAddUser = new Button
            {
                Text = "Add User",
                Location = new Point(10, 410),
                Size = new Size(100, 30),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left
            };
            btnAddUser.Click += BtnAddUser_Click;
            tabUsers.Controls.Add(btnAddUser);

            btnEditUser = new Button
            {
                Text = "Edit User",
                Location = new Point(120, 410),
                Size = new Size(100, 30),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left,
                Enabled = false
            };
            btnEditUser.Click += BtnEditUser_Click;
            tabUsers.Controls.Add(btnEditUser);

            btnDeleteUser = new Button
            {
                Text = "Delete User",
                Location = new Point(230, 410),
                Size = new Size(100, 30),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left,
                Enabled = false
            };
            btnDeleteUser.Click += BtnDeleteUser_Click;
            tabUsers.Controls.Add(btnDeleteUser);
        }

        private void SetupPermissionsTab()
        {
            // Title for Permissions tab
            lblPermissionsTitle = new Label
            {
                Text = "User Permissions",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Location = new Point(10, 10),
                Size = new Size(400, 30)
            };
            tabPermissions.Controls.Add(lblPermissionsTitle);

            // Description label
            Label lblDescription = new Label
            {
                Text = "Select which features the standard user can access:",
                Location = new Point(10, 45),
                Size = new Size(400, 20)
            };
            tabPermissions.Controls.Add(lblDescription);

            // User dropdown
            Label lblSelectUser = new Label
            {
                Text = "Select User:",
                Location = new Point(10, 75),
                Size = new Size(100, 20)
            };
            tabPermissions.Controls.Add(lblSelectUser);

            ComboBox cmbUsers = new ComboBox
            {
                Location = new Point(120, 75),
                Size = new Size(200, 23),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbUsers.SelectedIndexChanged += CmbUsers_SelectedIndexChanged;
            tabPermissions.Controls.Add(cmbUsers);

            // Permissions checklist
            lstPermissions = new CheckedListBox
            {
                Location = new Point(10, 110),
                Size = new Size(300, 200),
                BorderStyle = BorderStyle.FixedSingle,
                CheckOnClick = true
            };
            tabPermissions.Controls.Add(lstPermissions);

            // Column options panel
            panelColumns = new Panel
            {
                Location = new Point(330, 110),
                Size = new Size(320, 300),
                BorderStyle = BorderStyle.FixedSingle,
                Visible = false
            };

            lblColumnOptions = new Label
            {
                Text = "Select visible columns for Stock On Hand:",
                Location = new Point(10, 10),
                Size = new Size(290, 20),
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            panelColumns.Controls.Add(lblColumnOptions);

            // Create column checkboxes
            int yPos = 40;
            foreach (var column in _columnMap)
            {
                CheckBox chk = new CheckBox
                {
                    Text = column.Value,
                    Location = new Point(20, yPos),
                    Size = new Size(200, 20),
                    Tag = column.Key
                };

                // Special handling for PLU - it's always required
                if (column.Key == 1) // PLU
                {
                    chk.Checked = true;
                    chk.Enabled = false;
                    chk.Text += " (Required)";
                }

                columnCheckboxes.Add(column.Key, chk);
                panelColumns.Controls.Add(chk);
                yPos += 25;
            }

            // Add Select/Deselect All buttons for columns
            Button btnSelectAll = new Button
            {
                Text = "Select All Columns",
                Location = new Point(20, yPos + 10),
                Size = new Size(130, 30)
            };
            btnSelectAll.Click += (s, e) => {
                foreach (var checkbox in columnCheckboxes)
                {
                    if (checkbox.Key != 1) // Skip PLU as it's required
                        checkbox.Value.Checked = true;
                }
            };
            panelColumns.Controls.Add(btnSelectAll);

            Button btnDeselectAll = new Button
            {
                Text = "Deselect All",
                Location = new Point(160, yPos + 10),
                Size = new Size(130, 30)
            };
            btnDeselectAll.Click += (s, e) => {
                foreach (var checkbox in columnCheckboxes)
                {
                    if (checkbox.Key != 1) // Skip PLU as it's required
                        checkbox.Value.Checked = false;
                }
            };
            panelColumns.Controls.Add(btnDeselectAll);

            tabPermissions.Controls.Add(panelColumns);

            // Save permissions button
            Button btnSavePermissions = new Button
            {
                Text = "Save Permissions",
                Location = new Point(10, 320),
                Size = new Size(150, 30)
            };
            btnSavePermissions.Click += BtnSavePermissions_Click;
            tabPermissions.Controls.Add(btnSavePermissions);

            // Populate user dropdown
            PopulateUserDropdown(cmbUsers);
        }

        private void PopulateUserDropdown(ComboBox cmbUsers)
        {
            cmbUsers.Items.Clear();

            // Add all standard users (not including admin)
            var usersFile = LoginForm.GetUsersFilePath();
            if (File.Exists(usersFile))
            {
                foreach (string line in File.ReadAllLines(usersFile))
                {
                    string[] parts = line.Split('|');
                    if (parts.Length >= 4 && parts[0] != "admin")
                    {
                        cmbUsers.Items.Add(parts[0]);
                    }
                }
            }

            if (cmbUsers.Items.Count > 0)
            {
                cmbUsers.SelectedIndex = 0;
            }
        }

        private void LoadUsers()
        {
            lstUsers.Items.Clear();

            try
            {
                string usersFile = LoginForm.GetUsersFilePath();
                if (File.Exists(usersFile))
                {
                    foreach (string line in File.ReadAllLines(usersFile))
                    {
                        string[] parts = line.Split('|');
                        if (parts.Length >= 4)
                        {
                            ListViewItem item = new ListViewItem(parts[0]);

                            // Add user type
                            if (Enum.TryParse(parts[2], out UserType userType))
                            {
                                item.SubItems.Add(userType.ToString());
                            }
                            else
                            {
                                item.SubItems.Add("Unknown");
                            }

                            // Add default password status
                            bool isDefaultPassword = parts[3] == "1";
                            item.SubItems.Add(isDefaultPassword ? "Yes" : "No");

                            lstUsers.Items.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading users: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadUserPermissions(string username)
        {
            _selectedUsername = username;
            _userPermissions = _permissionManager.GetUserPermissions(username);

            // Update permissions checklist
            lstPermissions.Items.Clear();
            foreach (var permission in UserPermissions.AvailablePermissions)
            {
                int index = lstPermissions.Items.Add(permission.Value);

                // Check if the user has this permission
                if (_userPermissions.Contains(permission.Key))
                {
                    lstPermissions.SetItemChecked(index, true);

                    // Show column options if stock on hand is checked
                    if (permission.Key == UserPermissions.STOCK_ON_HAND_TAB)
                    {
                        panelColumns.Visible = true;
                    }
                }
            }

            // Load column settings
            LoadVisibleColumns();
        }

        /// <summary>
        /// Loads the visible columns for the Stock On Hand view
        /// </summary>
        private void LoadVisibleColumns()
        {
            try
            {
                string filePath = GetColumnSettingsFilePath();
                if (File.Exists(filePath))
                {
                    string[] lines = File.ReadAllLines(filePath);
                    _visibleColumns.Clear();

                    foreach (string line in lines)
                    {
                        if (int.TryParse(line, out int columnId))
                        {
                            _visibleColumns.Add(columnId);
                        }
                    }
                }
                else
                {
                    // Default to all columns visible
                    _visibleColumns = new List<int>(_columnMap.Keys);
                    SaveVisibleColumns();
                }

                // Update column checkboxes
                foreach (var checkbox in columnCheckboxes)
                {
                    checkbox.Value.Checked = _visibleColumns.Contains(checkbox.Key);
                }

                // Ensure PLU is always checked
                if (columnCheckboxes.ContainsKey(1))
                {
                    columnCheckboxes[1].Checked = true;
                }
            }
            catch
            {
                // If error, use all columns
                _visibleColumns = new List<int>(_columnMap.Keys);

                // Check all checkboxes
                foreach (var checkbox in columnCheckboxes)
                {
                    checkbox.Value.Checked = true;
                }
            }
        }

        private void SaveVisibleColumns()
        {
            try
            {
                _visibleColumns.Clear();

                // Get selected columns
                foreach (var checkbox in columnCheckboxes)
                {
                    if (checkbox.Value.Checked)
                    {
                        _visibleColumns.Add(checkbox.Key);
                    }
                }

                // Ensure PLU is always included
                if (!_visibleColumns.Contains(1))
                {
                    _visibleColumns.Add(1);
                }

                // Save to file
                string filePath = GetColumnSettingsFilePath();
                string directory = Path.GetDirectoryName(filePath);

                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                File.WriteAllLines(filePath, _visibleColumns.Select(x => x.ToString()));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving column settings: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetColumnSettingsFilePath()
        {
            string appDataPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "MannyTools");

            return Path.Combine(appDataPath, "stockonhand_columns.dat");
        }

        private void CmbUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cmb = (ComboBox)sender;
            if (cmb.SelectedItem != null)
            {
                string username = cmb.SelectedItem.ToString();
                LoadUserPermissions(username);
            }
        }

        private void BtnSavePermissions_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_selectedUsername))
            {
                MessageBox.Show("Please select a user first.", "No User Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Update all permissions
                foreach (var permission in UserPermissions.AvailablePermissions)
                {
                    int index = lstPermissions.Items.IndexOf(permission.Value);
                    bool isChecked = lstPermissions.GetItemChecked(index);

                    _permissionManager.SetPermission(_selectedUsername, permission.Key, isChecked);
                }

                // Save column settings if Stock On Hand is enabled
                if (lstPermissions.GetItemChecked(lstPermissions.Items.IndexOf(UserPermissions.AvailablePermissions[UserPermissions.STOCK_ON_HAND_TAB])))
                {
                    SaveVisibleColumns();
                }

                MessageBox.Show($"Permissions for {_selectedUsername} have been updated successfully.",
                    "Permissions Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving permissions: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LstUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool hasSelection = lstUsers.SelectedItems.Count > 0;
            btnEditUser.Enabled = hasSelection;

            // Only allow deleting users other than admin and self
            if (hasSelection)
            {
                string selectedUsername = lstUsers.SelectedItems[0].Text;
                btnDeleteUser.Enabled = selectedUsername != "admin" && selectedUsername != _authenticatedUsername;
            }
            else
            {
                btnDeleteUser.Enabled = false;
            }
        }

        private void BtnAddUser_Click(object sender, EventArgs e)
        {
            using (UserEditForm editForm = new UserEditForm(null))
            {
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    LoadUsers();

                    // Refresh user dropdown in permissions tab
                    foreach (Control control in tabPermissions.Controls)
                    {
                        if (control is ComboBox comboBox)
                        {
                            string previousSelection = comboBox.SelectedItem?.ToString();
                            PopulateUserDropdown(comboBox);

                            // Try to restore previous selection
                            if (!string.IsNullOrEmpty(previousSelection))
                            {
                                int index = comboBox.Items.IndexOf(previousSelection);
                                if (index >= 0)
                                {
                                    comboBox.SelectedIndex = index;
                                }
                            }
                            break;
                        }
                    }
                }
            }
        }

        private void BtnEditUser_Click(object sender, EventArgs e)
        {
            if (lstUsers.SelectedItems.Count > 0)
            {
                string username = lstUsers.SelectedItems[0].Text;

                using (UserEditForm editForm = new UserEditForm(username))
                {
                    if (editForm.ShowDialog() == DialogResult.OK)
                    {
                        LoadUsers();
                    }
                }
            }
        }

        private void BtnDeleteUser_Click(object sender, EventArgs e)
        {
            if (lstUsers.SelectedItems.Count > 0)
            {
                string username = lstUsers.SelectedItems[0].Text;

                // Prevent deleting admin or current user
                if (username == "admin" || username == _authenticatedUsername)
                {
                    MessageBox.Show("You cannot delete the admin account or your own account.",
                        "Delete Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (MessageBox.Show($"Are you sure you want to delete user '{username}'?",
                    "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        DeleteUser(username);
                        LoadUsers();

                        // Refresh user dropdown in permissions tab
                        foreach (Control control in tabPermissions.Controls)
                        {
                            if (control is ComboBox comboBox)
                            {
                                PopulateUserDropdown(comboBox);
                                break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting user: {ex.Message}",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void DeleteUser(string username)
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

            // Also delete user's permissions
            try
            {
                _permissionManager.DeleteUserPermissions(username);
            }
            catch
            {
                // Continue if this fails - not critical
            }
        }

        private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab == tabPermissions)
            {
                // Refresh the combobox when switching to the permissions tab
                foreach (Control control in tabPermissions.Controls)
                {
                    if (control is ComboBox comboBox)
                    {
                        string previousSelection = comboBox.SelectedItem?.ToString();
                        PopulateUserDropdown(comboBox);

                        // Try to restore previous selection
                        if (!string.IsNullOrEmpty(previousSelection))
                        {
                            int index = comboBox.Items.IndexOf(previousSelection);
                            if (index >= 0)
                            {
                                comboBox.SelectedIndex = index;
                            }
                        }
                        break;
                    }
                }
            }
        }

        private void UserManagementForm_Resize(object sender, EventArgs e)
        {
            // Adjust close button position
            if (btnClose != null)
            {
                btnClose.Location = new Point(this.ClientSize.Width - 120, this.ClientSize.Height - 50);
            }
        }
    }
}