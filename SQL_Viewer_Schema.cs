﻿using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Data;
using System.ComponentModel;
using Microsoft.Data.SqlClient;
using Timer = System.Windows.Forms.Timer;

namespace Manny_Tools_Claude
{
    public partial class SQL_Viewer_Schema : UserControl
    {
        #region Fields

        private string _connectionString;
        private SQL_Mapper_Schema _schemaMapper;
        private bool _isInitialized = false;

        #endregion

        #region Constructor & Initialization

        public SQL_Viewer_Schema(string connectionString = null)
        {
            _connectionString = connectionString;
            InitializeComponent();

            // Configure the table data grid
            ConfigureTableDataGrid();

            // Subscribe to connection changes
            DatabaseConnectionManager.Instance.ConnectionChanged += DatabaseConnection_Changed;
            ConnectionStatusManager.Instance.ConnectionStatusChanged += ConnectionStatus_Changed;

            // Subscribe to visibility changes to load data when tab becomes visible
            this.VisibleChanged += SQL_Viewer_Schema_VisibleChanged;

            // Attempt immediate initialization if connection is available
            if (!string.IsNullOrEmpty(_connectionString) ||
                !string.IsNullOrEmpty(DatabaseConnectionManager.Instance.ConnectionString))
            {
                // Use timer to allow UI to render first  
                Timer initTimer = new Timer();
                initTimer.Interval = 100; // Short delay
                initTimer.Tick += (s, e) => {
                    initTimer.Stop();
                    initTimer.Dispose();
                    InitializeData();
                };
                initTimer.Start();
            }
        }

        private void SQL_Viewer_Schema_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                // If the connection string is empty, try to get it from the manager
                if (string.IsNullOrEmpty(_connectionString))
                {
                    _connectionString = DatabaseConnectionManager.Instance.ConnectionString;
                }

                // If we have a connection string and connection is active, load tables
                if (!string.IsNullOrEmpty(_connectionString) && ConnectionStatusManager.Instance.IsConnected)
                {
                    // Reset the initialization flag to force reload
                    _isInitialized = false;
                    InitializeData();
                }
                else if (!_isInitialized)
                {
                    InitializeData();
                }
            }
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
            if (ConnectionStatusManager.Instance.IsConnected)
            {
                if (string.IsNullOrEmpty(_connectionString))
                {
                    // Try to get the connection string from the manager
                    _connectionString = DatabaseConnectionManager.Instance.ConnectionString;
                }

                if (!string.IsNullOrEmpty(_connectionString) && (lstTables.Items.Count == 0 || !_isInitialized))
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
                this.VisibleChanged -= SQL_Viewer_Schema_VisibleChanged;
            }

            base.Dispose(disposing);
        }

        public void UpdateConnectionString(string connectionString)
        {
            _connectionString = connectionString;
            InitializeSchemaMapper();
        }

        // Explicitly load tables
        public void LoadDatabaseTables()
        {
            // Make sure we have the latest connection string
            if (string.IsNullOrEmpty(_connectionString))
            {
                _connectionString = DatabaseConnectionManager.Instance.ConnectionString;
            }

            // If we still don't have a connection string, try to prompt the user
            if (string.IsNullOrEmpty(_connectionString))
            {
                UpdateStatus("No database connection configured. Please set up connection settings.", Color.Red);
                return;
            }

            // Make sure connection status is up to date
            ConnectionStatusManager.Instance.CheckConnection(_connectionString);

            if (ConnectionStatusManager.Instance.IsConnected)
            {
                try
                {
                    UpdateStatus("Loading database tables...", Color.DarkBlue);

                    // Initialize schema mapper and load tables
                    _schemaMapper = new SQL_Mapper_Schema(_connectionString);
                    LoadTableList();

                    _isInitialized = true;
                }
                catch (Exception ex)
                {
                    UpdateStatus($"Error loading tables: {ex.Message}", Color.Red);
                }
            }
            else
            {
                UpdateStatus("Database connection failed. Please check connection settings.", Color.Red);
            }
        }

        #endregion

        #region Data Loading & Display  

        private void InitializeData()
        {
            // Try to get connection if needed
            if (string.IsNullOrEmpty(_connectionString))
            {
                _connectionString = DatabaseConnectionManager.Instance.ConnectionString;
            }

            if (!string.IsNullOrEmpty(_connectionString))
            {
                UpdateStatus("Initializing database connection...", Color.DarkBlue);

                // Check connection status
                if (!ConnectionStatusManager.Instance.IsConnected)
                {
                    // Force a connection check
                    ConnectionStatusManager.Instance.CheckConnection(_connectionString);
                }

                if (ConnectionStatusManager.Instance.IsConnected)
                {
                    InitializeSchemaMapper();
                    _isInitialized = true; // Set initialization flag to prevent redundant loading
                }
                else
                {
                    UpdateStatus("Unable to connect to database. Please check connection settings.", Color.Red);
                }
            }
            else
            {
                UpdateStatus("No database connection configured. Please set up connection settings.", Color.Red);
            }
        }

        private void InitializeSchemaMapper()
        {
            try
            {
                if (string.IsNullOrEmpty(_connectionString))
                {
                    return;
                }

                UpdateStatus("Initializing schema mapper...", Color.DarkBlue);
                _schemaMapper = new SQL_Mapper_Schema(_connectionString);
                LoadTableList();
            }
            catch (Exception ex)
            {
                UpdateStatus($"Error initializing schema mapper: {ex.Message}", Color.Red);
            }
        }

        private void LoadTableList()
        {
            try
            {
                UpdateStatus("Loading tables...", Color.DarkBlue);
                Cursor.Current = Cursors.WaitCursor;

                // Check if connection is active
                bool connectionActive = TestConnection();
                if (!connectionActive)
                {
                    UpdateStatus("Database connection failed. Please check connection settings.", Color.Red);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                // Get table names
                var tables = GetTables();
                if (tables == null || tables.Count == 0)
                {
                    UpdateStatus("No tables found in database.", Color.DarkBlue);
                    Cursor.Current = Cursors.Default;
                    return;
                }

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

                UpdateStatus($"Loaded {tables.Count} tables", Color.Green);
            }
            catch (Exception ex)
            {
                UpdateStatus($"Error loading tables: {ex.Message}", Color.Red);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private bool TestConnection()
        {
            try
            {
                using (var connection = DatabaseConnectionManager.CreateConnection(_connectionString))
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

        private List<SQL_Mapper_Schema.TableInfo> GetTables()
        {
            try
            {
                return SQL_Get_Generic_List.ExecuteQuery<SQL_Mapper_Schema.TableInfo>(
                    _connectionString,
                    "SELECT name AS TableName, SCHEMA_NAME(schema_id) AS SchemaName FROM sys.tables ORDER BY name"
                );
            }
            catch
            {
                return new List<SQL_Mapper_Schema.TableInfo>();
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
                UpdateStatus($"Loading details for table: {tableName}...", Color.DarkBlue);
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

                if (columns == null || columns.Count == 0)
                {
                    UpdateStatus($"No columns found for table: {tableName}", Color.Red);
                    return;
                }

                // Display columns in grid
                dgvFields.DataSource = columns;

                // Get and display sample data - get LATEST 10 records
                string orderByColumn = GetTableIdentityOrKeyColumn(tableName);
                string dataQuery;

                if (!string.IsNullOrEmpty(orderByColumn))
                {
                    // If we found an ID or key column, order by it descending to get latest
                    dataQuery = $"SELECT TOP 10 * FROM [{tableName}] ORDER BY [{orderByColumn}] DESC";
                }
                else
                {
                    // Fallback - just get top 10 records
                    dataQuery = $"SELECT TOP 10 * FROM [{tableName}]";
                }

                DataTable sampleData = GetTableData(tableName, dataQuery);
                dgvTableData.DataSource = sampleData;

                // Apply formatting to monetary columns  
                FormatDataGridViewColumns(dgvTableData);

                UpdateStatus($"Loaded table details for: {tableName}", Color.Green);
            }
            catch (Exception ex)
            {
                UpdateStatus($"Error loading table details: {ex.Message}", Color.Red);
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
            DataTable dataTable = new DataTable();

            try
            {
                using (var connection = DatabaseConnectionManager.CreateConnection(_connectionString))
                {
                    connection.Open();

                    using (var adapter = new SqlDataAdapter(query, connection))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }
            catch (Exception ex)
            {
                UpdateStatus($"Error loading table data: {ex.Message}", Color.Red);
            }

            return dataTable;
        }

        private void UpdateStatus(string message, Color color)
        {
            if (lblStatus.InvokeRequired)
            {
                lblStatus.Invoke(new Action(() => {
                    lblStatus.Text = message;
                    lblStatus.ForeColor = color;
                }));
            }
            else
            {
                lblStatus.Text = message;
                lblStatus.ForeColor = color;
            }
        }

        #endregion

        #region Monetary Column Formatting

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

        /// &lt;summary&gt;
        /// Determines if a column is likely to contain monetary values based on its name
        /// &lt;/summary&gt;
        /// &lt;param name="columnName"&gt;The name of the column to check&lt;/param&gt;
        /// &lt;returns&gt;True if the column likely contains monetary values&lt;/returns&gt;
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

        /// &lt;summary&gt;
        /// Formats the DataGridView columns based on their content type
        /// &lt;/summary&gt;
        /// &lt;param name="gridView"&gt;The DataGridView to format&lt;/param&gt;
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