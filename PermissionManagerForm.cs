using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Manny_Tools_Claude;

namespace Manny_Tools_Claude
{
    public partial class PermissionManagerForm : Form
    {
        private CheckedListBox lstPermissions;
        private Button btnSave;
        private Button btnCancel;
        private Label lblTitle;
        private Label lblDescription;
        private Panel panelColumns;
        private Label lblColumnOptions;
        private Dictionary<int, CheckBox> columnCheckboxes = new Dictionary<int, CheckBox>();

        private UserPermissions _permissionManager;
        private string _username;
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
        private CheckBox chkStockOnHand;

        public PermissionManagerForm(string username)
        {
            _username = username;
            _permissionManager = new UserPermissions();
            _userPermissions = _permissionManager.GetUserPermissions(username);

            InitializeComponent();
            LoadPermissions();
            LoadVisibleColumns();
        }

        private void InitializeComponent()
        {
            this.Text = $"Manage Permissions for {_username}";
            this.Size = new Size(700, 600);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Title label
            lblTitle = new Label
            {
                Text = $"User Permissions: {_username}",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Location = new Point(20, 20),
                Size = new Size(400, 30)
            };

            // Description label
            lblDescription = new Label
            {
                Text = "Select which features the standard user can access:",
                Location = new Point(20, 55),
                Size = new Size(400, 20)
            };

            // Permissions checklist
            lstPermissions = new CheckedListBox
            {
                Location = new Point(20, 85),
                Size = new Size(300, 150),
                BorderStyle = BorderStyle.FixedSingle,
                CheckOnClick = true
            };

            // Column options panel (initially hidden)
            panelColumns = new Panel
            {
                Location = new Point(340, 85),
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

            // Save button
            btnSave = new Button
            {
                Text = "Save Permissions",
                Location = new Point(220, 500),
                Size = new Size(150, 40),
                DialogResult = DialogResult.OK
            };
            btnSave.Click += BtnSave_Click;

            // Cancel button
            btnCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(380, 500),
                Size = new Size(100, 40),
                DialogResult = DialogResult.Cancel
            };

            // Add controls to form
            this.Controls.Add(lblTitle);
            this.Controls.Add(lblDescription);
            this.Controls.Add(lstPermissions);
            this.Controls.Add(panelColumns);
            this.Controls.Add(btnSave);
            this.Controls.Add(btnCancel);

            // Set accept and cancel buttons
            this.AcceptButton = btnSave;
            this.CancelButton = btnCancel;
        }

        private void LoadPermissions()
        {
            lstPermissions.Items.Clear();

            // Add available permissions
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

                // Handle Stock On Hand permission specially
                if (permission.Key == UserPermissions.STOCK_ON_HAND_TAB)
                {
                    // Store reference to this checkbox for later
                    int stockOnHandIndex = index;

                    // Add handler to show/hide column options
                    lstPermissions.ItemCheck += (sender, e) =>
                    {
                        if (e.Index == stockOnHandIndex)
                        {
                            panelColumns.Visible = e.NewValue == CheckState.Checked;
                        }
                    };
                }
            }
        }

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

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Update all permissions
                foreach (var permission in UserPermissions.AvailablePermissions)
                {
                    int index = lstPermissions.Items.IndexOf(permission.Value);
                    bool isChecked = lstPermissions.GetItemChecked(index);

                    _permissionManager.SetPermission(_username, permission.Key, isChecked);
                }

                // Save column settings if Stock On Hand is enabled
                if (lstPermissions.GetItemChecked(lstPermissions.Items.IndexOf(UserPermissions.AvailablePermissions[UserPermissions.STOCK_ON_HAND_TAB])))
                {
                    SaveVisibleColumns();
                }

                MessageBox.Show($"Permissions for {_username} have been updated successfully.",
                    "Permissions Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving permissions: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}