using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;
using System.Linq;
using System.IO;

namespace Manny_Tools_Claude
{
    public partial class StockOnHandForm : UserControl
    {
        #region Fields

        private string _connectionString;
        private List<int> _visibleColumns = new List<int>();
        private string _currentUsername = "user";

        // Form controls
        private Label lblTitle;
        private Label lblEnterPLU;
        private TextBox txtPLU;
        private Button btnGetSOH;
        private Button btnClearGrid;
        private DataGridView dgvStockInfo;
        private Panel panelControls;
        private Label lblConnectionStatus;

        // Column definitions
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

        #endregion

        #region Constructor & Initialization

        public StockOnHandForm(string connectionString = null, string username = "user")
        {
            _connectionString = connectionString;
            _currentUsername = username;

            InitializeComponent();
            LoadVisibleColumns();
            ConfigureDataGridColumns();

            // If connection string is null or empty, try to load it
            if (string.IsNullOrEmpty(_connectionString))
            {
                TryLoadConnectionString();
            }

            // Update connection status display
            UpdateConnectionStatusDisplay();

            // Subscribe to connection status changes
            ConnectionStatusManager.Instance.ConnectionStatusChanged += Instance_ConnectionStatusChanged;
        }

        public void SetCurrentUsername(string username)
        {
            if (_currentUsername != username)
            {
                _currentUsername = username;
                LoadVisibleColumns();
                ConfigureDataGridColumns();
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

        private bool TryLoadConnectionString()
        {
            try
            {
                string appDataPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "MannyTools");

                string configPath = Path.Combine(appDataPath, DataEncryptionHelper.ConfigFiles.ConnectionFile);

                if (File.Exists(configPath))
                {
                    // Read and decrypt connection string
                    _connectionString = DataEncryptionHelper.ReadEncryptedFile(configPath);

                    // Test the connection
                    if (!string.IsNullOrEmpty(_connectionString))
                    {
                        // Check connection synchronously here
                        try
                        {
                            using (var connection = DatabaseConnectionManager.CreateConnectionWithTimeout(_connectionString))
                            {
                                connection.Open();
                                // Connection successful
                                return true;
                            }
                        }
                        catch
                        {
                            // Connection failed
                            _connectionString = null;
                            return false;
                        }
                    }
                }
            }
            catch
            {
                // Error reading file
                _connectionString = null;
            }

            return false;
        }

        public void UpdateConnectionString(string connectionString)
        {
            _connectionString = connectionString;
            UpdateConnectionStatusDisplay();
        }

        // Method to refresh columns after permissions change
        public void RefreshColumns()
        {
            LoadVisibleColumns();
            ConfigureDataGridColumns();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Unsubscribe from events
                ConnectionStatusManager.Instance.ConnectionStatusChanged -= Instance_ConnectionStatusChanged;
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            // Create main container with white background
            this.BackColor = SystemColors.Control;

            // Create a control panel for buttons and inputs
            panelControls = new Panel
            {
                Dock = DockStyle.Top,
                Height = 160,
                BackColor = SystemColors.Control
            };

            // Title label
            lblTitle = new Label
            {
                Text = "Stock On Hand Information",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Location = new Point(15, 10),
                AutoSize = true
            };
            panelControls.Controls.Add(lblTitle);

            // Add connection status label
            lblConnectionStatus = new Label
            {
                Text = "Connection: Unknown",
                Location = new Point(300, 15),
                Size = new Size(200, 20),
                TextAlign = ContentAlignment.MiddleRight,
                Font = new Font("Segoe UI", 9, FontStyle.Regular)
            };
            panelControls.Controls.Add(lblConnectionStatus);

            // PLU Input section
            lblEnterPLU = new Label
            {
                Text = "Enter PLU below...",
                Location = new Point(15, 50),
                Size = new Size(150, 20)
            };
            panelControls.Controls.Add(lblEnterPLU);

            txtPLU = new TextBox
            {
                Location = new Point(15, 75),
                Size = new Size(150, 23)
            };
            // Add KeyDown event handler for Enter key
            txtPLU.KeyDown += TxtPLU_KeyDown;
            panelControls.Controls.Add(txtPLU);

            btnGetSOH = new Button
            {
                Text = "Get SOH",
                Location = new Point(15, 110),
                Size = new Size(85, 40)
            };
            btnGetSOH.Click += BtnGetSOH_Click;
            panelControls.Controls.Add(btnGetSOH);

            btnClearGrid = new Button
            {
                Text = "Clear",
                Location = new Point(110, 110),
                Size = new Size(55, 30)
            };
            btnClearGrid.Click += BtnClearGrid_Click;
            panelControls.Controls.Add(btnClearGrid);

            // Add a message about column configuration
            Label lblColumnInfo = new Label
            {
                Text = "Column visibility is configured in Manage Permissions",
                Location = new Point(200, 110),
                Size = new Size(300, 30),
                ForeColor = Color.DarkBlue,
                Font = new Font("Segoe UI", 8, FontStyle.Italic)
            };
            panelControls.Controls.Add(lblColumnInfo);

            // Stock information grid
            dgvStockInfo = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToOrderColumns = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                RowHeadersVisible = false,
                AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.AliceBlue },
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            // Add controls to form
            this.Controls.Add(dgvStockInfo);
            this.Controls.Add(panelControls);

            // Set tab index
            txtPLU.TabIndex = 0;
            btnGetSOH.TabIndex = 1;
            btnClearGrid.TabIndex = 2;
        }

        #endregion

        #region Event Handlers

        // Handle Enter key in PLU textbox
        private void TxtPLU_KeyDown(object sender, KeyEventArgs e)
        {
            // If Enter key was pressed
            if (e.KeyCode == Keys.Enter)
            {
                // Prevent the ding sound
                e.SuppressKeyPress = true;

                // Trigger the Get SOH button
                PerformSearch();
            }
        }

        private void BtnGetSOH_Click(object sender, EventArgs e)
        {
            PerformSearch();
        }

        // Extract search logic to a separate method so it can be called
        // from both button click and Enter key press
        private void PerformSearch()
        {
            // If connection string is empty, try loading it again
            if (string.IsNullOrWhiteSpace(_connectionString))
            {
                if (!TryLoadConnectionString())
                {
                    MessageBox.Show("Database connection not configured. Please set up the connection in Settings.",
                        "Connection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            string productCode = txtPLU.Text.Trim();
            if (string.IsNullOrWhiteSpace(productCode))
            {
                MessageBox.Show("Please enter a PLU code.", "Missing Input", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtPLU.Focus();
                return;
            }

            try
            {
                // Test connection before proceeding
                using (var connection = DatabaseConnectionManager.CreateConnectionWithTimeout(_connectionString))
                {
                    try
                    {
                        connection.Open();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to connect to database: {ex.Message}",
                            "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                if (ValidateProductCode(productCode))
                {
                    string productDescription = GetDescription(productCode);
                    double sellPrice1 = GetSellPriceOne(productCode);
                    List<double> stockNumbers = GetStockOnHandResults(productCode);

                    double purchases = stockNumbers[0];
                    double sold = stockNumbers[1];
                    double claims = stockNumbers[2];
                    double layBuyStart = stockNumbers[3];
                    double layBuyFinish = stockNumbers[4];

                    double soh = purchases - claims - sold;
                    double inLayBuy = layBuyStart - layBuyFinish;

                    // Create a new row
                    int rowIndex = dgvStockInfo.Rows.Add();
                    DataGridViewRow row = dgvStockInfo.Rows[rowIndex];

                    // Fill data for all columns that exist
                    foreach (DataGridViewColumn col in dgvStockInfo.Columns)
                    {
                        switch (col.Name)
                        {
                            case "PLU": row.Cells[col.Index].Value = productCode; break;
                            case "Purchases": row.Cells[col.Index].Value = purchases; break;
                            case "Claims": row.Cells[col.Index].Value = claims; break;
                            case "Sold": row.Cells[col.Index].Value = sold; break;
                            case "LayBuyStarted": row.Cells[col.Index].Value = layBuyStart; break;
                            case "LayBuyFinished": row.Cells[col.Index].Value = layBuyFinish; break;
                            case "SOH": row.Cells[col.Index].Value = soh; break;
                            case "InLayBuy": row.Cells[col.Index].Value = inLayBuy; break;
                            case "Description": row.Cells[col.Index].Value = productDescription; break;
                            case "SellPrice1": row.Cells[col.Index].Value = sellPrice1; break;
                        }
                    }

                    // Format monetary columns - right align and currency format
                    foreach (DataGridViewColumn column in dgvStockInfo.Columns)
                    {
                        if (column.Name == "SellPrice1")
                        {
                            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            column.DefaultCellStyle.Format = "C2"; // Currency format with 2 decimal places
                        }
                    }

                    txtPLU.Clear();
                    txtPLU.Focus();
                }
                else
                {
                    MessageBox.Show("Invalid PLU code. Please enter a valid product code.",
                        "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPLU.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving stock information: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnClearGrid_Click(object sender, EventArgs e)
        {
            if (dgvStockInfo.Rows.Count > 0)
            {
                DialogResult result = MessageBox.Show("Are you sure you want to clear all data in the grid?",
                    "Confirm Clear", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    dgvStockInfo.Rows.Clear();
                }
            }
        }

        #endregion

        #region Data Access Methods

        private bool ValidateProductCode(string productCode)
        {
            if (string.IsNullOrEmpty(productCode))
                return false;

            try
            {
                string queryCheckPLUValid = $"SELECT ProductCode FROM ProductInfo WHERE ProductCode = @ProductCode";

                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    var result = connection.QueryFirstOrDefault<string>(queryCheckPLUValid,
                        new { ProductCode = productCode });

                    return !string.IsNullOrEmpty(result);
                }
            }
            catch
            {
                return false;
            }
        }

        private List<double> GetStockOnHandResults(string productCode)
        {
            // Define all the necessary stock queries
            string query1 = @"
                EXEC sp_executesql N'SELECT TOP 1 SUM(N0.""ReceivedQty"") FROM (""dbo"".""Purchase"" N0 
                INNER JOIN ""dbo"".""PurcaseLedger"" N1 ON (N0.""Ledger"" = N1.""OID""))
                WHERE (N0.""GCRecord"" IS NULL AND (N0.""Product"" = @p0) AND (N1.""Status"" = @p1) 
                AND NOT (N0.""Ledger"" IS NULL))',
                N'@p0 nvarchar(4000),@p1 int',@p0 = @ProductCode,@p1 = 2";

            string query2 = @"
                EXEC sp_executesql N'SELECT TOP 1 SUM(N0.""quantity"") FROM (""dbo"".""sales_journal"" N0 
                INNER JOIN ""dbo"".""SalesLedger"" N1 ON (N0.""Ledger"" = N1.""OID""))
                WHERE (N0.""GCRecord"" IS NULL AND (N0.""ProductCode"" = @p0) AND (N0.""function_key"" <> @p1) 
                AND (N0.""function_key"" <> @p2) AND (N1.""FunctionKey"" <> @p3))',
                N'@p0 nvarchar(4000),@p1 int,@p2 int,@p3 int',@p0=@ProductCode,@p1=4,@p2=13,@p3=10";

            string query4 = @"
                EXEC sp_executesql N'SELECT TOP 1 SUM(N0.""ReturnQuantity"") FROM (""dbo"".""ClaimsJournal"" N0 
                INNER JOIN ""dbo"".""ClaimLedger"" N1 ON (N0.""Ledger"" = N1.""OID""))
                WHERE (N0.""GCRecord"" IS NULL AND (N0.""Product"" = @p0) AND (N1.""Status"" <> @p1) 
                AND (N1.""Status"" <> @p2))',
                N'@p0 nvarchar(4000),@p1 int,@p2 int',@p0=@ProductCode,@p1=0,@p2=1";

            string query6 = @"
                EXEC sp_executesql N'SELECT TOP 1 SUM(N0.""quantity"") FROM (""dbo"".""sales_journal"" N0 
                INNER JOIN ""dbo"".""SalesLedger"" N1 ON (N0.""Ledger"" = N1.""OID""))
                WHERE (N0.""GCRecord"" IS NULL AND (N0.""ProductCode"" = @p0) AND (N1.""FunctionKey"" = @p1) 
                AND (N0.""function_key"" <> @p2) AND (N0.""function_key"" <> @p3) AND (N0.""function_key"" <> @p4))',
                N'@p0 nvarchar(4000),@p1 int,@p2 int,@p3 int,@p4 int',@p0=@ProductCode,@p1=10,@p2=4,@p3=6,@p4=13";

            string query7 = @"
                EXEC sp_executesql N'SELECT TOP 1 SUM(N0.""quantity"") FROM (""dbo"".""sales_journal"" N0 
                INNER JOIN ""dbo"".""SalesLedger"" N1 ON (N0.""Ledger"" = N1.""OID""))
                WHERE (N0.""GCRecord"" IS NULL AND (N0.""ProductCode"" = @p0) AND (N1.""FunctionKey"" = @p1) 
                AND (N0.""function_key"" <> @p2) AND (N0.""function_key"" <> @p3) AND (N0.""function_key"" <> @p4))',
                N'@p0 nvarchar(4000),@p1 int,@p2 int,@p3 int,@p4 int',@p0=@ProductCode,@p1=8,@p2=4,@p3=6,@p4=13";

            string[] queries = { query1, query2, query4, query6, query7 };
            List<double> results = new List<double>();

            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                foreach (string query in queries)
                {
                    object result = connection.ExecuteScalar(query, new { ProductCode = productCode });

                    if (result == null || result == DBNull.Value)
                    {
                        results.Add(0);
                    }
                    else if (double.TryParse(result.ToString(), out double value))
                    {
                        results.Add(value);
                    }
                    else
                    {
                        results.Add(0);
                    }
                }
            }

            return results;
        }

        private string GetDescription(string productCode)
        {
            try
            {
                string query = "SELECT Description FROM ProductInfo WHERE ProductCode = @ProductCode";

                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    string description = connection.QueryFirstOrDefault<string>(query,
                        new { ProductCode = productCode });

                    return string.IsNullOrEmpty(description) ? "No Description available" : description;
                }
            }
            catch
            {
                return "No Description available";
            }
        }

        private double GetSellPriceOne(string productCode)
        {
            try
            {
                string query = "SELECT SellingPriceIncl FROM ProductInfo WHERE ProductCode = @ProductCode";

                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    var result = connection.QueryFirstOrDefault<double?>(query,
                        new { ProductCode = productCode });

                    return result ?? 0;
                }
            }
            catch
            {
                return 0;
            }
        }

        #endregion

        #region Column Management

        private void LoadVisibleColumns()
        {
            try
            {
                string filePath = GetColumnSettingsFilePath(_currentUsername);
                if (File.Exists(filePath))
                {
                    string[] lines = DataEncryptionHelper.ReadEncryptedLines(filePath);
                    _visibleColumns.Clear();

                    if (lines != null)
                    {
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
                        // If decryption fails, use default columns file
                        filePath = GetColumnSettingsFilePath();
                        if (File.Exists(filePath))
                        {
                            lines = DataEncryptionHelper.ReadEncryptedLines(filePath);
                            if (lines != null)
                            {
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
                            }
                        }
                        else
                        {
                            // Default to all columns visible
                            _visibleColumns = new List<int>(_columnMap.Keys);
                        }
                    }
                }
                else
                {
                    // Try to use the default columns file
                    filePath = GetColumnSettingsFilePath();
                    if (File.Exists(filePath))
                    {
                        string[] lines = DataEncryptionHelper.ReadEncryptedLines(filePath);
                        _visibleColumns.Clear();

                        if (lines != null)
                        {
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
                        }
                    }
                    else
                    {
                        // Default to all columns visible
                        _visibleColumns = new List<int>(_columnMap.Keys);
                    }
                }
            }
            catch
            {
                // If error, use all columns
                _visibleColumns = new List<int>(_columnMap.Keys);
            }

            // Always ensure PLU is visible
            if (!_visibleColumns.Contains(1))
            {
                _visibleColumns.Add(1);
            }
        }
        private string GetColumnSettingsFilePath(string username = null)
        {
            string appDataPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "MannyTools");

            if (!string.IsNullOrEmpty(username))
            {
                return Path.Combine(appDataPath, $"columns_{username.ToLower()}.dat");
            }
            else
            {
                return Path.Combine(appDataPath, DataEncryptionHelper.ConfigFiles.ColumnsFile);
            }
        }

        private void ConfigureDataGridColumns()
        {
            // Preserve existing data
            var existingData = new List<Dictionary<string, object>>();
            foreach (DataGridViewRow row in dgvStockInfo.Rows)
            {
                var rowData = new Dictionary<string, object>();
                foreach (DataGridViewColumn col in dgvStockInfo.Columns)
                {
                    object value = row.Cells[col.Name].Value;
                    if (value != null)
                    {
                        rowData[col.Name] = value;
                    }
                }
                existingData.Add(rowData);
            }

            // Clear existing columns
            dgvStockInfo.Rows.Clear();
            dgvStockInfo.Columns.Clear();

            // Add columns based on visible settings
            foreach (int columnId in _visibleColumns)
            {
                if (_columnMap.TryGetValue(columnId, out string columnName))
                {
                    DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn
                    {
                        Name = columnName,
                        HeaderText = columnName,
                        AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                    };

                    // Right align numeric columns
                    if (columnId != 1 && columnId != 9) // Not PLU or Description
                    {
                        column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    }

                    // Apply currency format to SellPrice1
                    if (columnId == 10) // SellPrice1
                    {
                        column.DefaultCellStyle.Format = "C2"; // Currency format with 2 decimal places
                    }

                    dgvStockInfo.Columns.Add(column);
                }
            }

            // Restore data
            foreach (var rowData in existingData)
            {
                int rowIndex = dgvStockInfo.Rows.Add();
                DataGridViewRow row = dgvStockInfo.Rows[rowIndex];

                foreach (DataGridViewColumn col in dgvStockInfo.Columns)
                {
                    if (rowData.TryGetValue(col.Name, out object value))
                    {
                        row.Cells[col.Name].Value = value;
                    }
                }
            }
        }

        private string GetColumnSettingsFilePath()
        {
            string appDataPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "MannyTools");

            return Path.Combine(appDataPath, DataEncryptionHelper.ConfigFiles.ColumnsFile);
        }

        #endregion
    }
}