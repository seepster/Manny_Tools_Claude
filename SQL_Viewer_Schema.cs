using System;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Collections.Generic;
using System.Data;
using Manny_Tools_Claude;
using System.ComponentModel;

namespace Manny_Tools_Claude
{
    public partial class SQL_Viewer_Schema : UserControl
    {
        #region Fields

        private string _connectionString;
        private SQL_Mapper_Schema _schemaMapper;

        // Form controls
        private Label lblTitle;
        private Label lblInstructions;
        private Label lblTableList;
        private ListBox lstTables;
        private Label lblTableFields;
        private DataGridView dgvFields;
        private Label lblTableData;
        private DataGridView dgvTableData;
        private Button btnRefresh;
        private Panel panelHeader;
        private Panel panelLeft;
        private Panel panelRight;
        private SplitContainer splitContainer;

        #endregion

        #region Constructor & Initialization

        public SQL_Viewer_Schema(string connectionString = null)
        {
            _connectionString = connectionString;
            InitializeComponent();

            // Configure the table data grid
            ConfigureTableDataGrid();

            // Check if we have a connection string
            if (!string.IsNullOrEmpty(_connectionString))
            {
                InitializeSchemaMapper();
            }

            // Subscribe to connection changes
            DatabaseConnectionManager.Instance.ConnectionChanged += DatabaseConnection_Changed;
            ConnectionStatusManager.Instance.ConnectionStatusChanged += ConnectionStatus_Changed;
        }

        private void ConnectionStatus_Changed(object sender, ConnectionStatusEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => CheckAndLoadTables()));
            }
            else
            {
                CheckAndLoadTables();
            }
        }

        private void DatabaseConnection_Changed(object sender, ConnectionChangedEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => {
                    _connectionString = e.ConnectionString;
                    InitializeSchemaMapper();
                }));
            }
            else
            {
                _connectionString = e.ConnectionString;
                InitializeSchemaMapper();
            }
        }

        private void CheckAndLoadTables()
        {
            if (ConnectionStatusManager.Instance.IsConnected &&
                string.IsNullOrEmpty(_connectionString))
            {
                // Try to get the connection string from the manager
                _connectionString = DatabaseConnectionManager.Instance.ConnectionString;
                if (!string.IsNullOrEmpty(_connectionString))
                {
                    InitializeSchemaMapper();
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Unsubscribe from events
                DatabaseConnectionManager.Instance.ConnectionChanged -= DatabaseConnection_Changed;
                ConnectionStatusManager.Instance.ConnectionStatusChanged -= ConnectionStatus_Changed;
            }

            base.Dispose(disposing);
        }

        public void UpdateConnectionString(string connectionString)
        {
            _connectionString = connectionString;
            InitializeSchemaMapper();
        }

        private void InitializeComponent()
        {
            // Create main container with white background
            this.BackColor = SystemColors.Control;

            // Create header panel
            panelHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.FromArgb(240, 240, 240)
            };

            // Create title and instructions
            lblTitle = new Label
            {
                Text = "Database Schema Viewer",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Location = new Point(10, 10),
                AutoSize = true
            };

            lblInstructions = new Label
            {
                Text = "Select a table from the list to view its structure and data.",
                Location = new Point(10, 35),
                Size = new Size(950, 20),
                Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top
            };

            // Add refresh button
            btnRefresh = new Button
            {
                Text = "Refresh Tables",
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(950, 15),
                Size = new Size(100, 30)
            };

            // Add controls to header panel
            panelHeader.Controls.Add(lblTitle);
            panelHeader.Controls.Add(lblInstructions);
            panelHeader.Controls.Add(btnRefresh);

            // Content container to hold both the left panel and right content
            Panel contentContainer = new Panel
            {
                Dock = DockStyle.Fill
            };

            // Create split container for main content
            splitContainer = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Horizontal,
                SplitterDistance = 300,
                Panel1MinSize = 100,
                Panel2MinSize = 100
            };

            // Create left panel for table list
            panelLeft = new Panel
            {
                Dock = DockStyle.Left,
                Width = 250,
                BackColor = Color.FromArgb(245, 245, 245)
            };

            lblTableList = new Label
            {
                Text = "Database Tables",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Dock = DockStyle.Top,
                Padding = new Padding(10, 10, 10, 5),
                Height = 30
            };

            lstTables = new ListBox
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(10, 0, 10, 10),
                BorderStyle = BorderStyle.FixedSingle
            };

            panelLeft.Controls.Add(lstTables);
            panelLeft.Controls.Add(lblTableList);

            // Create right panel for table details
            panelRight = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(10)
            };

            // Table Fields Section
            lblTableFields = new Label
            {
                Text = "Table Fields",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Dock = DockStyle.Top,
                Padding = new Padding(0, 0, 0, 5)
            };

            dgvFields = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                RowHeadersVisible = false,
                AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.AliceBlue }
            };

            Panel panel1Container = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(0, 25, 0, 0)
            };

            panel1Container.Controls.Add(dgvFields);
            splitContainer.Panel1.Controls.Add(panel1Container);
            splitContainer.Panel1.Controls.Add(lblTableFields);

            // Table Data Section
            lblTableData = new Label
            {
                Text = "Table Data (Last 10 Records)",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Dock = DockStyle.Top,
                Padding = new Padding(0, 0, 0, 5)
            };

            dgvTableData = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                RowHeadersVisible = false,
                AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.AliceBlue },
                AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells,
                AllowUserToResizeColumns = true,
                AllowUserToResizeRows = false,
                ScrollBars = ScrollBars.Both
            };

            Panel panel2Container = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(0, 25, 0, 0)
            };

            panel2Container.Controls.Add(dgvTableData);
            splitContainer.Panel2.Controls.Add(panel2Container);
            splitContainer.Panel2.Controls.Add(lblTableData);

            // Add panels to the form
            panelRight.Controls.Add(splitContainer);
            contentContainer.Controls.Add(panelRight);
            contentContainer.Controls.Add(panelLeft);

            this.Controls.Add(contentContainer);
            this.Controls.Add(panelHeader);

            // Wire up events
            lstTables.SelectedIndexChanged += LstTables_SelectedIndexChanged;
            btnRefresh.Click += BtnRefresh_Click;

            // Load tables on control shown
            this.HandleCreated += SQL_Viewer_Schema_HandleCreated;
        }

        private void SQL_Viewer_Schema_HandleCreated(object sender, EventArgs e)
        {
            // Try to load tables if we have a connection
            if (string.IsNullOrEmpty(_connectionString))
            {
                // Try to get connection string from the manager
                _connectionString = DatabaseConnectionManager.Instance.ConnectionString;
            }

            if (!string.IsNullOrEmpty(_connectionString))
            {
                InitializeSchemaMapper();
            }
        }

        private void ConfigureTableDataGrid()
        {
            // Add a handler for DataBindingComplete to adjust column widths after data binding
            dgvTableData.DataBindingComplete += (sender, e) =>
            {
                if (e.ListChangedType != ListChangedType.ItemDeleted)
                {
                    // First auto-size columns based on header and content
                    dgvTableData.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);

                    // Apply formatting to monetary columns
                    FormatDataGridViewColumns(dgvTableData);

                    // Ensure there's at least one scrollable column
                    bool hasScrollableColumn = false;

                    foreach (DataGridViewColumn col in dgvTableData.Columns)
                    {
                        // If any column width is greater than 300, limit it and ensure scrollbars
                        if (col.Width > 300)
                        {
                            col.Width = 300;
                            hasScrollableColumn = true;
                        }
                    }

                    // If no column needed scrolling, make the last column fill available space
                    if (!hasScrollableColumn && dgvTableData.Columns.Count > 0)
                    {
                        dgvTableData.Columns[dgvTableData.Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    }
                }
            };
        }

        #endregion

        #region Data Loading & Display

        private void InitializeSchemaMapper()
        {
            try
            {
                if (string.IsNullOrEmpty(_connectionString))
                {
                    return;
                }

                _schemaMapper = new SQL_Mapper_Schema(_connectionString);
                LoadTableList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing schema mapper: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadTableList()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                // Check if connection is active
                if (!ConnectionStatusManager.Instance.IsConnected)
                {
                    ConnectionStatusManager.Instance.CheckConnection(_connectionString);
                    if (!ConnectionStatusManager.Instance.IsConnected)
                    {
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }

                // Get table names
                var tables = SQL_Get_Generic_List.ExecuteQuery<SQL_Mapper_Schema.TableInfo>(
                    _connectionString,
                    "SELECT name AS TableName, SCHEMA_NAME(schema_id) AS SchemaName FROM sys.tables ORDER BY name"
                );

                lstTables.Items.Clear();
                foreach (var table in tables)
                {
                    lstTables.Items.Add(table.TableName);
                }

                // Clear displays
                dgvFields.DataSource = null;
                dgvTableData.DataSource = null;

                lblTableFields.Text = "Table Fields";
                lblTableData.Text = "Table Data (Latest 10 Records)";

                // Auto select the first table if available
                if (lstTables.Items.Count > 0)
                {
                    lstTables.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading tables: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadTableList();
        }

        private void LstTables_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstTables.SelectedItem != null)
            {
                string selectedTable = lstTables.SelectedItem.ToString();
                ShowTableDetails(selectedTable);
            }
        }

        private void ShowTableDetails(string tableName)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                // Update labels
                lblTableFields.Text = $"Table Fields: {tableName}";
                lblTableData.Text = $"Table Data: {tableName} (Latest 10 Records)";

                // Get table columns
                string columnsQuery = @"
                    SELECT 
                        c.name AS ColumnName,
                        TYPE_NAME(c.user_type_id) AS DataType,
                        c.max_length AS MaxLength,
                        c.is_nullable AS IsNullable,
                        CASE WHEN pk.column_id IS NOT NULL THEN 'Yes' ELSE 'No' END AS IsPrimaryKey,
                        COLUMNPROPERTY(c.object_id, c.name, 'IsIdentity') AS IsIdentity,
                        c.column_id AS ColumnOrder
                    FROM 
                        sys.columns c
                        LEFT JOIN (
                            SELECT index_columns.object_id, index_columns.column_id
                            FROM sys.index_columns
                            INNER JOIN sys.indexes ON indexes.object_id = index_columns.object_id 
                            AND indexes.index_id = index_columns.index_id
                            WHERE indexes.is_primary_key = 1
                        ) pk ON pk.object_id = OBJECT_ID(@TableName) AND pk.column_id = c.column_id
                    WHERE
                        c.object_id = OBJECT_ID(@TableName)
                    ORDER BY 
                        c.column_id";

                var columns = SQL_Get_Generic_List.ExecuteQuery<ColumnDetail>(
                    _connectionString,
                    columnsQuery,
                    new { TableName = tableName }
                );

                // Display columns in grid
                dgvFields.DataSource = columns;

                // Get and display sample data - get LATEST 10 records
                string dataQuery;

                // Try to identify primary key or identity column for proper ordering
                string orderByColumn = GetTableIdentityOrKeyColumn(tableName);

                if (!string.IsNullOrEmpty(orderByColumn))
                {
                    // If we found an ID or key column, order by it descending to get latest
                    dataQuery = $"SELECT TOP 10 * FROM [{tableName}] ORDER BY [{orderByColumn}] DESC";
                }
                else
                {
                    // Fallback - try to order by first column descending
                    dataQuery = $"SELECT TOP 10 * FROM [{tableName}] ORDER BY (SELECT TOP 1 column_name FROM information_schema.columns WHERE table_name = '{tableName}') DESC";
                }

                var sampleData = GetTableData(tableName, dataQuery);
                dgvTableData.DataSource = sampleData;

                // Apply formatting to monetary columns
                FormatDataGridViewColumns(dgvTableData);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading table details: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private string GetTableIdentityOrKeyColumn(string tableName)
        {
            try
            {
                // First try to get an identity column
                string identityQuery = @"
                    SELECT TOP 1 c.name
                    FROM sys.columns c
                    WHERE 
                        c.object_id = OBJECT_ID(@TableName)
                        AND COLUMNPROPERTY(c.object_id, c.name, 'IsIdentity') = 1
                    ORDER BY c.column_id";

                string identityColumn = SQL_Get_Generic_List.ExecuteScalar<string>(
                    _connectionString,
                    identityQuery,
                    new { TableName = tableName }
                );

                if (!string.IsNullOrEmpty(identityColumn))
                {
                    return identityColumn;
                }

                // If no identity column, try to get primary key
                string pkQuery = @"
                    SELECT TOP 1 c.name
                    FROM sys.columns c
                    INNER JOIN sys.index_columns ic ON ic.object_id = c.object_id AND ic.column_id = c.column_id
                    INNER JOIN sys.indexes i ON i.object_id = ic.object_id AND i.index_id = ic.index_id
                    WHERE 
                        c.object_id = OBJECT_ID(@TableName)
                        AND i.is_primary_key = 1
                    ORDER BY ic.key_ordinal";

                string pkColumn = SQL_Get_Generic_List.ExecuteScalar<string>(
                    _connectionString,
                    pkQuery,
                    new { TableName = tableName }
                );

                return pkColumn;
            }
            catch
            {
                return null; // Return null if any error occurs
            }
        }

        private DataTable GetTableData(string tableName, string query)
        {
            using (var connection = new Microsoft.Data.SqlClient.SqlConnection(_connectionString))
            {
                connection.Open();
                using (var adapter = new Microsoft.Data.SqlClient.SqlDataAdapter(query, connection))
                {
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    return dataTable;
                }
            }
        }

        #endregion

        #region Monetary Column Formatting

        /// <summary>
        /// Determines if a column is likely to contain monetary values based on its name
        /// </summary>
        /// <param name="columnName">The name of the column to check</param>
        /// <returns>True if the column likely contains monetary values</returns>
        private bool IsMonetaryColumn(string columnName)
        {
            if (string.IsNullOrEmpty(columnName))
                return false;

            // Convert to lowercase for case-insensitive comparison
            string lowerName = columnName.ToLower();

            // Common patterns for monetary column names
            string[] monetaryPatterns = new[]
            {
                "price", "amount", "cost", "fee", "payment", "salary", "wage",
                "budget", "revenue", "expense", "income", "profit", "margin",
                "balance", "total", "subtotal", "discount", "tax", "charge",
                "money", "cash", "fund", "dollar", "currency", "usd", "eur",
                "gbp", "jpy", "cny", "inr", "value", "debit", "credit"
            };

            // Check if the column name contains any of the monetary patterns
            foreach (string pattern in monetaryPatterns)
            {
                if (lowerName.Contains(pattern) || lowerName.EndsWith(pattern))
                    return true;
            }

            // Check for column names that end with common monetary suffixes
            string[] monetarySuffixes = new[] { "_$", "$", "_amt", "_amount", "_price" };
            foreach (string suffix in monetarySuffixes)
            {
                if (lowerName.EndsWith(suffix))
                    return true;
            }

            // Check for column names that start with common monetary prefixes
            string[] monetaryPrefixes = new[] { "$", "amt_", "amount_", "price_" };
            foreach (string prefix in monetaryPrefixes)
            {
                if (lowerName.StartsWith(prefix))
                    return true;
            }

            // Check for column names that match data type patterns
            string[] dataTypePatterns = new[] { "money", "decimal", "currency" };
            foreach (string pattern in dataTypePatterns)
            {
                if (lowerName.Equals(pattern))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Formats the DataGridView columns based on their content type
        /// </summary>
        /// <param name="gridView">The DataGridView to format</param>
        private void FormatDataGridViewColumns(DataGridView gridView)
        {
            if (gridView == null || gridView.Columns.Count == 0)
                return;

            foreach (DataGridViewColumn column in gridView.Columns)
            {
                // Check if this is a monetary column based on name
                if (IsMonetaryColumn(column.HeaderText))
                {
                    // Right-align header
                    column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;

                    // Right-align cells and format as currency if it's a numeric type
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                    // Set currency format if the column type is appropriate
                    Type columnType = gridView.Columns[column.Index].ValueType;
                    if (columnType == typeof(decimal) || columnType == typeof(double) ||
                        columnType == typeof(float) || columnType == typeof(int) ||
                        columnType == typeof(long) || columnType == typeof(short))
                    {
                        column.DefaultCellStyle.Format = "C2"; // Currency format with 2 decimal places
                    }
                }
            }
        }

        #endregion

        #region Helper Classes

        // Helper class for displaying column details
        private class ColumnDetail
        {
            public string ColumnName { get; set; }
            public string DataType { get; set; }
            public int MaxLength { get; set; }
            public bool IsNullable { get; set; }
            public string IsPrimaryKey { get; set; }
            public bool IsIdentity { get; set; }
            public int ColumnOrder { get; set; }
        }

        #endregion
    }
}