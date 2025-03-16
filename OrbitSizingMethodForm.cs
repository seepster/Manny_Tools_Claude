using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using Dapper;

namespace Manny_Tools_Claude
{
    public partial class OrbitSizingMethodForm : UserControl
    {
        #region Fields

        // Form controls
        private Label lblTitle;
        private Panel panelLeft;
        private Panel panelRight;
        private Label lblTables;
        private ListView lstTables;
        private Label lblTableData;
        private DataGridView dgvTableData;
        private SplitContainer splitContainer;
        private Label lblConnectionStatus;

        // Table names to display
        private readonly string[] _tablesToShow = { "Department", "MinorDepartment", "ProductSize", "SizeLink" };

        #endregion

        #region Constructor & Initialization

        public OrbitSizingMethodForm()
        {
            InitializeComponent();

            // Subscribe to connection status changes
            ConnectionStatusManager.Instance.ConnectionStatusChanged += Instance_ConnectionStatusChanged;

            // Subscribe to database connection changes
            DatabaseConnectionManager.Instance.ConnectionChanged += DatabaseConnection_Changed;

            // Update connection status and populate tables
            UpdateConnectionStatusDisplay();
            PopulateTablesList();
        }

        private void DatabaseConnection_Changed(object sender, ConnectionChangedEventArgs e)
        {
            // Connection has changed, update the display
            UpdateConnectionStatusDisplay();

            // Reload current table if one is selected
            if (lstTables.SelectedItems.Count > 0)
            {
                string selectedTable = lstTables.SelectedItems[0].Text;
                LoadTableData(selectedTable);
            }
        }

        private void Instance_ConnectionStatusChanged(object sender, ConnectionStatusEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => UpdateConnectionStatusDisplay()));
            }
            else
            {
                UpdateConnectionStatusDisplay();
            }
        }

        private void UpdateConnectionStatusDisplay()
        {
            if (lblConnectionStatus != null)
            {
                lblConnectionStatus.Text = ConnectionStatusManager.Instance.IsConnected ?
                    "Connection: Connected" : "Connection: Not Connected";
                lblConnectionStatus.ForeColor = ConnectionStatusManager.Instance.IsConnected ?
                    Color.Green : Color.Red;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Unsubscribe from events
                ConnectionStatusManager.Instance.ConnectionStatusChanged -= Instance_ConnectionStatusChanged;
                DatabaseConnectionManager.Instance.ConnectionChanged -= DatabaseConnection_Changed;
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.BackColor = SystemColors.Control;

            // Create title panel
            Panel headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 50,
                BackColor = Color.FromArgb(240, 240, 240)
            };

            // Create title
            lblTitle = new Label
            {
                Text = "Orbit Sizing Method",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Location = new Point(15, 10),
                AutoSize = true
            };

            // Add connection status label
            lblConnectionStatus = new Label
            {
                Text = "Connection: Unknown",
                Location = new Point(300, 15),
                Size = new Size(200, 20),
                TextAlign = ContentAlignment.MiddleRight,
                Font = new Font("Segoe UI", 9, FontStyle.Regular)
            };

            headerPanel.Controls.Add(lblTitle);
            headerPanel.Controls.Add(lblConnectionStatus);

            // Create main layout - left panel for tables and right panel for data
            Panel mainPanel = new Panel
            {
                Dock = DockStyle.Fill
            };

            // Create left panel for table list
            panelLeft = new Panel
            {
                Dock = DockStyle.Left,
                Width = 200,
                Padding = new Padding(10)
            };

            lblTables = new Label
            {
                Text = "Database Tables",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 25
            };

            // Create tables listview
            lstTables = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                HeaderStyle = ColumnHeaderStyle.Nonclickable
            };
            lstTables.Columns.Add("Table Name", -2);
            lstTables.SelectedIndexChanged += LstTables_SelectedIndexChanged;

            // Add left panel controls
            panelLeft.Controls.Add(lstTables);
            panelLeft.Controls.Add(lblTables);

            // Create right panel for table data
            panelRight = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(10)
            };

            lblTableData = new Label
            {
                Text = "Table Data",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 25
            };

            // Create data grid with improved settings for horizontal scrolling
            dgvTableData = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells,
                AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells,
                AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.AliceBlue },
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText,
                AllowUserToResizeColumns = true,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
            };

            // Enable both horizontal and vertical scrolling
            dgvTableData.ScrollBars = ScrollBars.Both;

            // Add right panel controls
            panelRight.Controls.Add(dgvTableData);
            panelRight.Controls.Add(lblTableData);

            // Add panels to main panel
            mainPanel.Controls.Add(panelRight);
            mainPanel.Controls.Add(panelLeft);

            // Add all panels to form
            this.Controls.Add(mainPanel);
            this.Controls.Add(headerPanel);

            // Add context menu for copying cell content
            ContextMenuStrip contextMenu = new ContextMenuStrip();

            ToolStripMenuItem copyItem = new ToolStripMenuItem("Copy Selected Cell(s)");
            copyItem.Click += (s, e) => {
                if (dgvTableData.GetCellCount(DataGridViewElementStates.Selected) > 0)
                {
                    Clipboard.SetDataObject(dgvTableData.GetClipboardContent());
                }
            };

            contextMenu.Items.Add(copyItem);
            dgvTableData.ContextMenuStrip = contextMenu;
        }

        #endregion

        #region Data Loading & Display

        private void PopulateTablesList()
        {
            lstTables.Items.Clear();

            foreach (string tableName in _tablesToShow)
            {
                var item = new ListViewItem(tableName);
                lstTables.Items.Add(item);
            }

            // Auto-size column
            lstTables.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        private void LstTables_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstTables.SelectedItems.Count > 0)
            {
                string selectedTable = lstTables.SelectedItems[0].Text;
                LoadTableData(selectedTable);
            }
        }

        private void LoadTableData(string tableName)
        {
            try
            {
                // Check database connection
                if (!ConnectionStatusManager.Instance.IsConnected)
                {
                    MessageBox.Show("Database connection is not available. Please check the connection settings.",
                        "Connection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Cursor.Current = Cursors.WaitCursor;
                lblTableData.Text = $"Table Data: {tableName}";

                // Create and execute query using the database connection manager
                using (var connection = DatabaseConnectionManager.Instance.GetConnection())
                {
                    if (connection == null)
                    {
                        MessageBox.Show("Could not connect to the database.",
                            "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Create the query
                    string query = $"SELECT * FROM [dbo].[{tableName}]";

                    // Create data adapter and fill data table
                    using (var adapter = new SqlDataAdapter(query, connection))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        dgvTableData.DataSource = dataTable;
                    }
                }

                // Format the columns
                FormatDataGrid();

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show($"Error loading table data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormatDataGrid()
        {
            if (dgvTableData.Columns.Count == 0)
                return;

            // First set column AutoSizeMode to allow initial sizing based on content
            dgvTableData.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

            // Adjust specific columns based on content type
            foreach (DataGridViewColumn column in dgvTableData.Columns)
            {
                string columnName = column.HeaderText.ToLower();

                // For monetary columns, format as currency
                if (columnName.Contains("price") || columnName.Contains("cost") ||
                    columnName.Contains("value") || columnName.Contains("amount"))
                {
                    column.DefaultCellStyle.Format = "C2";
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }

                // ID columns typically should be right-aligned
                if (columnName.Contains("id") || columnName.EndsWith("id"))
                {
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }

                // Cap max column width to ensure horizontal scrolling for very wide values
                if (column.Width > 300)
                {
                    column.Width = 300;
                }

                // Ensure minimum column width for readability
                if (column.Width < 50)
                {
                    column.Width = 50;
                }
            }

            // Set data grid properties to ensure horizontal scrolling when needed
            dgvTableData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            // Calculate if we need horizontal scrolling
            int totalColumnWidth = 0;
            foreach (DataGridViewColumn col in dgvTableData.Columns)
            {
                totalColumnWidth += col.Width;
            }

            // Only apply Fill mode to last column if all columns can fit without scrolling
            if (totalColumnWidth < dgvTableData.Width)
            {
                dgvTableData.Columns[dgvTableData.Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }

        #endregion
    }
}