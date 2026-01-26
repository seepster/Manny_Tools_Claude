using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;
using System.Linq;
using System.IO;
using System.Text;

namespace Manny_Tools_Claude
{
    public partial class StockCountForm : UserControl
    {
        #region Fields

        private string _connectionString;
        private string _currentUsername = "user";

        #endregion

        #region Constructor & Initialization

        public StockCountForm(string connectionString = null, string username = "user")
        {
            _currentUsername = username;

            // Check if the provided connection string is valid
            if (!string.IsNullOrEmpty(connectionString))
            {
                _connectionString = connectionString;
            }
            else
            {
                // Try to get the connection string from the DatabaseConnectionManager
                _connectionString = DatabaseConnectionManager.Instance.ConnectionString;
            }

            InitializeComponent();

            // If connection string is still null or empty, try to load it
            if (string.IsNullOrEmpty(_connectionString))
            {
                TryLoadConnectionString();
            }
            else
            {
                // Always validate the current connection
                ConnectionStatusManager.Instance.CheckConnection(_connectionString);
            }

            // Update connection status display
            UpdateConnectionStatusDisplay();

            // Subscribe to connection status changes
            ConnectionStatusManager.Instance.ConnectionStatusChanged += Instance_ConnectionStatusChanged;

            // Add event handlers
            dgvStockCount.CellValidating += DgvStockCount_CellValidating;
            dgvStockCount.CellClick += DgvStockCount_CellClick;
            dgvStockCount.CellEndEdit += DgvStockCount_CellEndEdit;
            this.VisibleChanged += StockCountForm_VisibleChanged;

            // Setup the DataGridView columns
            SetupDataGridColumns();
        }

        private void StockCountForm_VisibleChanged(object sender, EventArgs e)
        {
            // Auto-load products when form becomes visible, but only if connection is valid
            if (this.Visible && ConnectionStatusManager.Instance.IsConnected)
            {
                // Add a short delay to allow UI to initialize before loading data
                System.Windows.Forms.Timer initTimer = new System.Windows.Forms.Timer();
                initTimer.Interval = 500; // Short delay
                initTimer.Tick += (s, args) => {
                    initTimer.Stop();
                    initTimer.Dispose();

                    // Load the products if the grid is empty
                    if (dgvStockCount.Rows.Count == 0)
                    {
                        FetchProductsFromDatabase();
                    }
                };
                initTimer.Start();
            }
        }

        private void Instance_ConnectionStatusChanged(object sender, ConnectionStatusEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => {
                    UpdateConnectionStatusDisplay();

                    // If we gained connection and the form is visible, load data
                    if (ConnectionStatusManager.Instance.IsConnected && this.Visible && dgvStockCount.Rows.Count == 0)
                    {
                        // Add a short delay to allow UI to update
                        System.Windows.Forms.Timer initTimer = new System.Windows.Forms.Timer();
                        initTimer.Interval = 300; // Short delay
                        initTimer.Tick += (s, args) => {
                            initTimer.Stop();
                            initTimer.Dispose();
                            FetchProductsFromDatabase();
                        };
                        initTimer.Start();
                    }
                }));
            }
            else
            {
                UpdateConnectionStatusDisplay();

                // If we gained connection and the form is visible, load data
                if (ConnectionStatusManager.Instance.IsConnected && this.Visible && dgvStockCount.Rows.Count == 0)
                {
                    // Add a short delay to allow UI to update
                    System.Windows.Forms.Timer initTimer = new System.Windows.Forms.Timer();
                    initTimer.Interval = 300; // Short delay
                    initTimer.Tick += (s, args) => {
                        initTimer.Stop();
                        initTimer.Dispose();
                        FetchProductsFromDatabase();
                    };
                    initTimer.Start();
                }
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
                // Use the DatabaseConnectionManager singleton instead of reading directly from file
                _connectionString = DatabaseConnectionManager.Instance.ConnectionString;

                // If the connection string is still null or empty, try to load it from the manager
                if (string.IsNullOrWhiteSpace(_connectionString))
                {
                    if (DatabaseConnectionManager.Instance.LoadConnectionString())
                    {
                        _connectionString = DatabaseConnectionManager.Instance.ConnectionString;
                    }
                }

                // Only proceed if we have a valid connection string
                if (!string.IsNullOrWhiteSpace(_connectionString))
                {
                    // Update the connection status manager
                    ConnectionStatusManager.Instance.CheckConnection(_connectionString);

                    // Test the connection
                    bool connectionValid = TestConnection();

                    // If valid, update the display
                    if (connectionValid)
                    {
                        UpdateConnectionStatusDisplay();
                    }

                    return connectionValid;
                }
            }
            catch (Exception ex)
            {
                // Log the error
                System.Diagnostics.Debug.WriteLine($"Error loading connection string: {ex.Message}");
                _connectionString = null;
            }

            // Update connection status to show disconnected
            ConnectionStatusManager.Instance.CheckConnection(string.Empty);
            UpdateConnectionStatusDisplay();

            return false;
        }

        public void UpdateConnectionString(string connectionString)
        {
            bool connectionChanged = _connectionString != connectionString;
            _connectionString = connectionString;

            // Update the connection status display
            UpdateConnectionStatusDisplay();

            // If connection string changed and we're visible, reload data
            if (connectionChanged && this.Visible && ConnectionStatusManager.Instance.IsConnected)
            {
                // Clear existing data
                dgvStockCount.Rows.Clear();

                // Add a short delay to allow UI to update
                System.Windows.Forms.Timer initTimer = new System.Windows.Forms.Timer();
                initTimer.Interval = 300; // Short delay
                initTimer.Tick += (s, e) => {
                    initTimer.Stop();
                    initTimer.Dispose();
                    FetchProductsFromDatabase();
                };
                initTimer.Start();
            }
        }

        private void SetupDataGridColumns()
        {
            // Clear existing columns
            dgvStockCount.Columns.Clear();

            // Add ProductCode column
            DataGridViewTextBoxColumn productCodeColumn = new DataGridViewTextBoxColumn
            {
                Name = "ProductCode",
                HeaderText = "Product Code",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                ReadOnly = true // Can't edit product code
            };
            dgvStockCount.Columns.Add(productCodeColumn);

            // Add Count column
            DataGridViewTextBoxColumn countColumn = new DataGridViewTextBoxColumn
            {
                Name = "Count",
                HeaderText = "Count",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                ReadOnly = false // User can enter count
            };
            countColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            // Add visual cue for editable cells (light yellow background)
            countColumn.DefaultCellStyle.BackColor = Color.LightYellow;
            dgvStockCount.Columns.Add(countColumn);

            // Add Description column
            DataGridViewTextBoxColumn descriptionColumn = new DataGridViewTextBoxColumn
            {
                Name = "Description",
                HeaderText = "Description",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                ReadOnly = true // Can't edit description
            };
            dgvStockCount.Columns.Add(descriptionColumn);

            // Set column widths - Product Code 20%, Count 20%, Description 60%
            productCodeColumn.FillWeight = 20;
            countColumn.FillWeight = 20;
            descriptionColumn.FillWeight = 60;
        }

        #endregion

        #region Event Handlers

        private void BtnGetProducts_Click(object sender, EventArgs e)
        {
            FetchProductsFromDatabase();
        }

        private void BtnExportToCsv_Click(object sender, EventArgs e)
        {
            ExportToCsv();
        }

        private void BtnClearGrid_Click(object sender, EventArgs e)
        {
            if (dgvStockCount.Rows.Count > 0)
            {
                DialogResult result = MessageBox.Show("Are you sure you want to clear all data in the grid?",
                    "Confirm Clear", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    dgvStockCount.Rows.Clear();
                    lblQueryStatus.Text = string.Empty;
                }
            }
        }

        private void DgvStockCount_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            // Only validate the Count column (index 1)
            if (e.ColumnIndex == 1 && !string.IsNullOrWhiteSpace(e.FormattedValue.ToString()))
            {
                if (!int.TryParse(e.FormattedValue.ToString(), out _))
                {
                    e.Cancel = true;
                    MessageBox.Show("Please enter a valid numeric value for Count.",
                        "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void DgvStockCount_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // When clicking on a Count cell, enter edit mode
            if (e.RowIndex >= 0 && e.ColumnIndex == 1)
            {
                dgvStockCount.BeginEdit(true);
            }
        }

        private void DgvStockCount_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // Format the value when editing is done
            if (e.RowIndex >= 0 && e.ColumnIndex == 1)
            {
                var cell = dgvStockCount.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (string.IsNullOrWhiteSpace(cell.Value?.ToString()))
                {
                    cell.Value = string.Empty; // Keep it empty for display but it will be 0 when exported
                }
            }
        }

        #endregion

        #region Data Access Methods

        private void FetchProductsFromDatabase()
        {
            // Always get the latest connection string from the DatabaseConnectionManager
            _connectionString = DatabaseConnectionManager.Instance.ConnectionString;

            // If connection string is empty, try loading it again
            if (string.IsNullOrWhiteSpace(_connectionString))
            {
                if (!TryLoadConnectionString())
                {
                    lblQueryStatus.Text = "No database connection available.";
                    lblQueryStatus.ForeColor = Color.Red;

                    // Only show message if user explicitly clicked the button
                    if (btnGetProducts.Enabled)
                    {
                        MessageBox.Show("Database connection not configured. Please set up the connection in Settings.",
                            "Connection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    return;
                }
            }

            try
            {
                // Update UI to show query is in progress
                lblQueryStatus.Text = "Querying database...";
                lblQueryStatus.ForeColor = Color.DarkBlue;
                btnGetProducts.Enabled = false;
                Application.DoEvents();

                // Validate the connection before proceeding
                bool connectionValid = TestConnection();
                if (!connectionValid)
                {
                    lblQueryStatus.Text = "Connection failed. Please check settings.";
                    lblQueryStatus.ForeColor = Color.Red;
                    ConnectionStatusManager.Instance.CheckConnection(_connectionString); // Update connection status
                    btnGetProducts.Enabled = true;
                    return;
                }

                // Clear existing data
                dgvStockCount.Rows.Clear();

                // New query per requirement
                string query = @"
                    select [Packaging and Sizes].[Product Code],' 'Counted,[Item].[Description]
                    from [Packaging and Sizes]
                    join [Quantities] on [Packaging and Sizes].[Link Code]=[Quantities].[product code]
                    join [item] on[Packaging and Sizes].[Product Code]=[Item].[product code]
                    where LEN([Packaging and Sizes].[Product Code])=3
                    order by [Product Code]";

                using (var connection = DatabaseConnectionManager.CreateConnection(_connectionString))
                {
                    // Set a reasonable timeout
                    connection.Open();

                    // Execute the query with a timeout
                    var products = connection.Query(query, commandTimeout: 30);

                    if (products == null || !products.Any())
                    {
                        lblQueryStatus.Text = "No products found.";
                        lblQueryStatus.ForeColor = Color.DarkBlue;
                        return;
                    }

                    // Add products to the DataGridView
                    foreach (var product in products)
                    {
                        try
                        {
                            int rowIndex = dgvStockCount.Rows.Add();
                            dgvStockCount.Rows[rowIndex].Cells["ProductCode"].Value = ((IDictionary<string, object>)product)["Product Code"];
                            dgvStockCount.Rows[rowIndex].Cells["Count"].Value = ((IDictionary<string, object>)product)["Counted"];
                            dgvStockCount.Rows[rowIndex].Cells["Description"].Value = ((IDictionary<string, object>)product)["Description"];
                        }
                        catch
                        {
                            // Skip any rows that cause errors instead of failing the entire load
                            continue;
                        }
                    }

                    int productCount = products.Count();
                    lblQueryStatus.Text = $"Found {productCount} products.";
                    lblQueryStatus.ForeColor = Color.Green;

                    // Only notify if data was loaded successfully
                    if (productCount > 0)
                    {
                        // Briefly highlight the first row to indicate successful loading
                        if (dgvStockCount.Rows.Count > 0)
                        {
                            dgvStockCount.CurrentCell = dgvStockCount.Rows[0].Cells[0];
                            dgvStockCount.FirstDisplayedScrollingRowIndex = 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblQueryStatus.Text = "Error retrieving products.";
                lblQueryStatus.ForeColor = Color.Red;

                // Log the error details
                System.Diagnostics.Debug.WriteLine($"Error in FetchProductsFromDatabase: {ex.Message}");

                // Show user-friendly error message
                MessageBox.Show($"Error retrieving products: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // Update connection status after error
                ConnectionStatusManager.Instance.CheckConnection(_connectionString);
            }
            finally
            {
                btnGetProducts.Enabled = true;
            }
        }

        private bool TestConnection()
        {
            if (string.IsNullOrWhiteSpace(_connectionString))
            {
                return false;
            }

            try
            {
                using (var connection = DatabaseConnectionManager.CreateConnection(_connectionString))
                {
                    // Set a short timeout for the connection test
                    connection.Open();

                    // Execute a simple query to truly test the connection
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT 1";
                        command.CommandTimeout = 5; // Short timeout
                        command.ExecuteScalar();
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                // Log the connection error
                System.Diagnostics.Debug.WriteLine($"Connection test failed: {ex.Message}");
                return false;
            }
        }

        private void ExportToCsv()
        {
            if (dgvStockCount.Rows.Count == 0)
            {
                MessageBox.Show("No data to export.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                // Get current date and time for the filename
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string filename = $"ArchStockTake_{timestamp}.csv";

                // Get MyDocuments folder
                string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string filePath = Path.Combine(documentsPath, filename);

                // Build CSV content
                StringBuilder csv = new StringBuilder();

                foreach (DataGridViewRow row in dgvStockCount.Rows)
                {
                    string productCode = row.Cells["ProductCode"].Value?.ToString() ?? "";
                    string count = row.Cells["Count"].Value?.ToString() ?? "0"; // Default to 0 if empty

                    // If count is empty, use 0
                    if (string.IsNullOrWhiteSpace(count))
                    {
                        count = "0";
                    }

                    csv.AppendLine($"{productCode},{count}");
                }

                // Write to file
                File.WriteAllText(filePath, csv.ToString());

                MessageBox.Show($"Stock count data exported to:\n{filePath}",
                    "Export Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting data: {ex.Message}",
                    "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion
    }
}