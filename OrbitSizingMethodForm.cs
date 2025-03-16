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

        private string _connectionString;

        // Form controls
        private Label lblTitle;
        private Panel panelLeft;
        private Panel panelRight;
        private Label lblTables;
        private ListView lstTables;
        private Label lblTableData;
        private DataGridView dgvTableData;
        private SplitContainer splitContainer;

        // Table names to display
        private readonly string[] _tablesToShow = { "Department", "MinorDepartment", "ProductSize", "SizeLink" };

        #endregion

        #region Constructor & Initialization

        public OrbitSizingMethodForm(string connectionString = null)
        {
            _connectionString = connectionString;
            InitializeComponent();
            PopulateTablesList();
        }

        public void UpdateConnectionString(string connectionString)
        {
            _connectionString = connectionString;
            if (lstTables.SelectedItems.Count > 0)
            {
                // Refresh the current view if a table is selected
                string selectedTable = lstTables.SelectedItems[0].Text;
                LoadTableData(selectedTable);
            }
        }

        private void InitializeComponent()
        {
            this.BackColor = SystemColors.Control;

            // Create main layout with split container
            // Create main layout with split container
            // Create main layout with split container without specifying problematic properties initially
            splitContainer = new SplitContainer
            {
                Dock = DockStyle.Fill
            };

            // Add additional properties after it's added to the control hierarchy
            this.Controls.Add(splitContainer);

           

            // Create left panel for table list
            panelLeft = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(10)
            };

            // Create header
            lblTitle = new Label
            {
                Text = "Orbit Sizing Method",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 40
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
            splitContainer.Panel1.Controls.Add(panelLeft);

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

            // Create data grid
            dgvTableData = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                RowHeadersVisible = false,
                AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.AliceBlue },
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            // Add right panel controls
            panelRight.Controls.Add(dgvTableData);
            panelRight.Controls.Add(lblTableData);
            splitContainer.Panel2.Controls.Add(panelRight);

            // Add controls to form
            this.Controls.Add(splitContainer);
            this.Controls.Add(lblTitle);
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
                if (string.IsNullOrEmpty(_connectionString))
                {
                    MessageBox.Show("Database connection not configured. Please set up the connection in Settings.",
                        "Connection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Cursor.Current = Cursors.WaitCursor;
                lblTableData.Text = $"Table Data: {tableName}";

                // Create and execute query
                string query = $"SELECT * FROM [dbo].[{tableName}]";
                var dataTable = GetTableData(query);

                // Display data in grid
                dgvTableData.DataSource = dataTable;

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

        private DataTable GetTableData(string query)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var adapter = new SqlDataAdapter(query, connection))
                {
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    return dataTable;
                }
            }
        }

        private void FormatDataGrid()
        {
            if (dgvTableData.Columns.Count == 0)
                return;

            // Auto-size columns to fit content
            dgvTableData.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

            // For monetary columns, format as currency
            foreach (DataGridViewColumn column in dgvTableData.Columns)
            {
                string columnName = column.HeaderText.ToLower();
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

                // For columns that are too large, cap the width
                if (column.Width > 300)
                {
                    column.Width = 300;
                }
            }

            // Set the last column to fill mode
            if (dgvTableData.Columns.Count > 0)
            {
                DataGridViewColumn lastCol = dgvTableData.Columns[dgvTableData.Columns.Count - 1];

                // Only apply Fill mode if the total width of all columns is less than the available width
                int totalColumnsWidth = 0;
                foreach (DataGridViewColumn col in dgvTableData.Columns)
                {
                    totalColumnsWidth += col.Width;
                }

                if (totalColumnsWidth < dgvTableData.ClientSize.Width)
                {
                    lastCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
        }

        #endregion
    }

    
}