using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Manny_Tools_Claude
{
    public partial class CreateSizesForm : UserControl
    {
        // Define fields to store form data
        private List<ProductSizeItem> productSizes = new List<ProductSizeItem>();
        private List<SizeLinkItem> sizeLinks = new List<SizeLinkItem>();
        private Dictionary<string, string> suppliers = new Dictionary<string, string>();

        // Class to hold ProductSize data
        private class ProductSizeItem
        {
            public string Number { get; set; }
            public string Description { get; set; }

            public override string ToString()
            {
                return $"{Number} {Description}";
            }
        }

        // Class to hold SizeLink data
        private class SizeLinkItem
        {
            public string SizeCode { get; set; }
            public string Description { get; set; }

            public override string ToString()
            {
                return $"{SizeCode} - {Description}";
            }
        }

        public CreateSizesForm()
        {
            InitializeComponent();
            LoadDataFromDatabase();
            RegisterEventHandlers();
        }

        private async void LoadDataFromDatabase()
        {
            try
            {
                // Get database connection string
                string connectionString = DatabaseConnectionManager.Instance.ConnectionString;
                if (string.IsNullOrEmpty(connectionString))
                {
                    MessageBox.Show("Database connection is not configured. Using sample data instead.",
                        "Connection Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Show loading indicator
                Cursor.Current = Cursors.WaitCursor;

                // Load product sizes from database
                await LoadProductSizesAsync(connectionString);

                // If a product size is selected, load related size links
                if (ProductSizeComboBox.SelectedItem != null)
                {
                    var selectedItem = (ProductSizeItem)ProductSizeComboBox.SelectedItem;
                    await LoadSizeLinksAsync(connectionString, selectedItem.Number);
                }

                // Load sample supplier data (replace with actual database query if needed)
                LoadSupplierData();

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show($"Error loading data: {ex.Message}\nUsing sample data instead.",
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadProductSizesAsync(string connectionString)
        {
            try
            {
                // Clear existing items
                productSizes.Clear();

                // Query for product sizes
                string query = "SELECT Number, Description FROM ProductSize ORDER BY Number";
                var results = await Task.Run(() => SQL_Get_Generic_List.ExecuteQuery<dynamic>(connectionString, query));

                if (results != null && results.Count > 0)
                {
                    // Convert results to ProductSizeItem objects
                    foreach (var item in results)
                    {
                        productSizes.Add(new ProductSizeItem
                        {
                            Number = item.Number.ToString(),
                            Description = item.Description.ToString()
                        });
                    }

                    // Update the ComboBox - Use a properly formatted string with both values
                    ProductSizeComboBox.DataSource = null;
                    ProductSizeComboBox.Items.Clear();

                    foreach (var item in productSizes)
                    {
                        // Format as "Number Description" as requested
                        ProductSizeComboBox.Items.Add($"{item.Number} {item.Description}");
                    }

                    // Select first item by default
                    if (ProductSizeComboBox.Items.Count > 0)
                    {
                        ProductSizeComboBox.SelectedIndex = 0;
                    }
                }
                else
                {
                    // No results found, load sample data
                    MessageBox.Show("No product sizes found in the database. Using sample data.",
                        "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadSampleProductSizes();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading product sizes: {ex.Message}");
                LoadSampleProductSizes();
                throw; // Re-throw to be handled by the caller
            }
        }

        private async Task LoadSizeLinksAsync(string connectionString, string productSizeNumber)
        {
            try
            {
                // Clear existing items
                sizeLinks.Clear();

                // Query for size links related to the selected product size
                string query = "SELECT SizeCode, Description FROM SizeLink WHERE Holder = @Holder ORDER BY SizeCode";
                var results = await Task.Run(() => SQL_Get_Generic_List.ExecuteQuery<dynamic>(
                    connectionString, query, new { Holder = productSizeNumber }));

                if (results != null && results.Count > 0)
                {
                    // Convert results to SizeLinkItem objects
                    foreach (var item in results)
                    {
                        sizeLinks.Add(new SizeLinkItem
                        {
                            SizeCode = item.SizeCode.ToString(),
                            Description = item.Description.ToString()
                        });
                    }
                }
                else
                {
                    // No results found, use empty list
                    System.Diagnostics.Debug.WriteLine($"No size links found for product size: {productSizeNumber}");
                }

                // Populate the CheckedListBox
                PopulateSizeLinks();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading size links: {ex.Message}");
                throw; // Re-throw to be handled by the caller
            }
        }

        private void LoadSupplierData()
        {
            // Clear existing suppliers
            suppliers.Clear();

            // Add sample supplier data (this could be replaced with a database query)
            suppliers.Add("SUP001", "Acme Clothing Co.");
            suppliers.Add("SUP002", "Fashion Wholesale Ltd");
            suppliers.Add("SUP003", "Textile Supplies Inc.");

            // Update the ComboBox
            SupplierNumberComboBox.DataSource = null;
            SupplierNumberComboBox.DataSource = new List<string>(suppliers.Keys);

            // Select first item by default
            if (SupplierNumberComboBox.Items.Count > 0)
            {
                SupplierNumberComboBox.SelectedIndex = 0;
            }
        }


        private void LoadSampleProductSizes()
        {
            // Clear existing items
            productSizes.Clear();
            ProductSizeComboBox.Items.Clear();

            // Populate the combobox directly with formatted strings
            foreach (var item in productSizes)
            {
                ProductSizeComboBox.Items.Add($"{item.Number} {item.Description}");
            }

            // Select first item by default
            if (ProductSizeComboBox.Items.Count > 0)
            {
                ProductSizeComboBox.SelectedIndex = 0;
            }
        }

        private void LoadSampleSizeLinks()
        {
            // Clear existing items
            sizeLinks.Clear();

            // Add sample size links
            sizeLinks.Add(new SizeLinkItem { SizeCode = "UK 8", Description = "UK Size 8" });
            sizeLinks.Add(new SizeLinkItem { SizeCode = "UK 10", Description = "UK Size 10" });
            sizeLinks.Add(new SizeLinkItem { SizeCode = "UK 12", Description = "UK Size 12" });
            sizeLinks.Add(new SizeLinkItem { SizeCode = "UK 14", Description = "UK Size 14" });
            sizeLinks.Add(new SizeLinkItem { SizeCode = "UK 16", Description = "UK Size 16" });

            // Populate the CheckedListBox
            PopulateSizeLinks();
        }

        private void PopulateSizeLinks()
        {
            SizeLinksListBox.Items.Clear();
            foreach (var sizeLink in sizeLinks)
            {
                SizeLinksListBox.Items.Add(sizeLink, false);
            }
        }

        private void RegisterEventHandlers()
        {
            // Register event handlers for the controls
            ParentProductCodeButton.Click += ParentProductCodeButton_Click;
            LoadTemplateButton.Click += LoadTemplateButton_Click;
            ClearTemplateButton.Click += ClearTemplateButton_Click;
            NextAvailableParentCodeButton.Click += NextAvailableParentCodeButton_Click;
            CreateCSVFileButton.Click += CreateCSVFileButton_Click;

            // Use separate method to handle product size selection to load size links
            ProductSizeComboBox.SelectedIndexChanged += ProductSizeComboBox_SelectedIndexChanged;

            SelectAllCheckBox.CheckedChanged += SelectAllCheckBox_CheckedChanged;
            SelectNoneCheckBox.CheckedChanged += SelectNoneCheckBox_CheckedChanged;

            // Add handler for supplier dropdown
            SupplierNumberComboBox.SelectedIndexChanged += SupplierNumberComboBox_SelectedIndexChanged;
        }

        #region Event Handlers

        private void ParentProductCodeButton_Click(object sender, EventArgs e)
        {
            // Implementation for Parent/Product Code button click
            MessageBox.Show("Parent/Product Code button clicked");
        }

        private void LoadTemplateButton_Click(object sender, EventArgs e)
        {
            // Implementation for Load Template button click
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
                Title = "Select a template file"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    LoadTemplateFromFile(openFileDialog.FileName);
                    MessageBox.Show("Template loaded successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading template: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ClearTemplateButton_Click(object sender, EventArgs e)
        {
            // Implementation for Clear Template button click
            ClearForm();
            MessageBox.Show("Form cleared");
        }

        private async void NextAvailableParentCodeButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Show loading indicator
                Cursor.Current = Cursors.WaitCursor;
                NextAvailableParentCodeButton.Enabled = false;
                NextAvailableParentCodeButton.Text = "Loading...";

                // Get database connection string
                string connectionString = DatabaseConnectionManager.Instance.ConnectionString;
                if (string.IsNullOrEmpty(connectionString))
                {
                    MessageBox.Show("Database connection is not configured. Please check the connection settings.",
                        "Connection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Get the next available parent code
                string nextCode = await GetNextAvailableParentCodeAsync(connectionString);

                // Update the textbox with the result
                if (!string.IsNullOrEmpty(nextCode))
                {
                    Product_ParentCodeTextBox.Text = nextCode;
                }
                else
                {
                    MessageBox.Show("No existing parent codes found. You must enter the next parent code manually.",
                        "Manual Entry Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error finding next available parent code: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Reset button state
                Cursor.Current = Cursors.Default;
                NextAvailableParentCodeButton.Enabled = true;
                NextAvailableParentCodeButton.Text = "Next available Parent Code";
            }
        }

        private void CreateCSVFileButton_Click(object sender, EventArgs e)
        {
            // Implementation for Create CSV File button click
            if (ValidateForm())
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "CSV files (*.csv)|*.csv",
                    Title = "Save Size Data"
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        CreateCSVFile(saveFileDialog.FileName);
                        MessageBox.Show("CSV file created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error creating CSV file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private async void ProductSizeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ProductSizeComboBox.SelectedItem != null)
                {
                    // Parse out the Number from the selected string
                    string selectedText = ProductSizeComboBox.SelectedItem.ToString();
                    string sizeNumber = selectedText.Split(' ')[0]; // Get the first part (the Number)

                    // Update size suffix based on selected product size
                    SizeSuffixTextBox.Text = sizeNumber;

                    // Load related size links from database
                    string connectionString = DatabaseConnectionManager.Instance.ConnectionString;
                    if (!string.IsNullOrEmpty(connectionString))
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        await LoadSizeLinksAsync(connectionString, sizeNumber);
                        Cursor.Current = Cursors.Default;
                    }
                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                System.Diagnostics.Debug.WriteLine($"Error in ProductSizeComboBox_SelectedIndexChanged: {ex.Message}");
                // Don't show message box to avoid disrupting the UI flow
            }
        }

        private void SupplierNumberComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Update supplier name based on selected supplier number
            string selectedSupplier = SupplierNumberComboBox.SelectedItem?.ToString();
            if (!string.IsNullOrEmpty(selectedSupplier) && suppliers.ContainsKey(selectedSupplier))
            {
                SupplierNameTextBox.Text = suppliers[selectedSupplier];
            }
            else
            {
                SupplierNameTextBox.Text = string.Empty;
            }
        }

        private void SelectAllCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (SelectAllCheckBox.Checked)
            {
                // Check all items in the size links list
                for (int i = 0; i < SizeLinksListBox.Items.Count; i++)
                {
                    SizeLinksListBox.SetItemChecked(i, true);
                }

                // Uncheck the "Select None" checkbox
                SelectNoneCheckBox.Checked = false;
            }
        }

        private void SelectNoneCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (SelectNoneCheckBox.Checked)
            {
                // Uncheck all items in the size links list
                for (int i = 0; i < SizeLinksListBox.Items.Count; i++)
                {
                    SizeLinksListBox.SetItemChecked(i, false);
                }

                // Uncheck the "Select All" checkbox
                SelectAllCheckBox.Checked = false;
            }
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Gets the next available parent code by querying the database for existing codes
        /// and finding the next available 5 or 6 digit integer.
        /// </summary>
        /// <param name="connectionString">Database connection string</param>
        /// <returns>The next available parent code, or null if no codes are found</returns>
        private async Task<string> GetNextAvailableParentCodeAsync(string connectionString)
        {
            try
            {
                // Use the SQL_Get_Generic_List class to query for parent codes
                string query = @"
                    SELECT ParentCode 
                    FROM ProductInfo 
                    WHERE LEN(ParentCode) <= 6
                    AND ISNUMERIC(ParentCode) = 1
                    ORDER BY CAST(ParentCode AS INT)";

                var results = await Task.Run(() => SQL_Get_Generic_List.ExecuteQuery<string>(connectionString, query));

                if (results == null || results.Count == 0)
                {
                    // No results found
                    return null;
                }

                // Convert results to integers (filtering out any non-numeric values)
                List<int> parentCodes = new List<int>();
                foreach (var code in results)
                {
                    if (int.TryParse(code, out int numericCode))
                    {
                        parentCodes.Add(numericCode);
                    }
                }

                // If no valid numeric codes found after filtering
                if (parentCodes.Count == 0)
                {
                    return "10000"; // Start with first 5-digit number
                }

                // Sort the codes to ensure proper ordering
                parentCodes.Sort();

                // Find the next available 5-digit number
                int nextCode = 10000; // Start with first 5-digit number

                foreach (int code in parentCodes)
                {
                    if (code == nextCode)
                    {
                        nextCode++;
                    }
                    else if (code > nextCode)
                    {
                        // Found a gap
                        break;
                    }
                }

                // If we've exhausted all 5-digit numbers, move to 6-digit
                if (nextCode >= 100000)
                {
                    // Ensure we don't exceed 6 digits
                    if (nextCode > 999999)
                    {
                        // This would be extremely rare - inform user
                        MessageBox.Show("All 6-digit parent codes are in use. Please create a new numbering scheme.",
                            "Code Limit Reached", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return null;
                    }
                }

                return nextCode.ToString();
            }
            catch (Exception ex)
            {
                // Log the error
                System.Diagnostics.Debug.WriteLine($"Error in GetNextAvailableParentCodeAsync: {ex.Message}");
                throw; // Re-throw to be handled by the caller
            }
        }

        private void ClearForm()
        {
            // Clear all form fields
            Product_ParentCodeTextBox.Clear();
            DescriptionTextBox.Clear();
            SizeSuffixTextBox.Clear();
            MarkupTextBox.Clear();
            CostPriceExclTextBox.Clear();
            SupplierCodeTextBox.Clear();

            // Reset dropdowns to first item
            if (ProductSizeComboBox.Items.Count > 0)
                ProductSizeComboBox.SelectedIndex = 0;

            if (SupplierNumberComboBox.Items.Count > 0)
                SupplierNumberComboBox.SelectedIndex = 0;

            // Uncheck all size links
            for (int i = 0; i < SizeLinksListBox.Items.Count; i++)
            {
                SizeLinksListBox.SetItemChecked(i, false);
            }

            // Reset checkboxes
            SelectAllCheckBox.Checked = false;
            SelectNoneCheckBox.Checked = false;
        }

        private void LoadTemplateFromFile(string filePath)
        {
            // Clear current form data
            ClearForm();

            // Read the first line of the CSV file
            string[] lines = File.ReadAllLines(filePath);
            if (lines.Length < 2)
            {
                throw new Exception("Template file is empty or does not contain data");
            }

            // Skip header row and use first data row as template
            string[] fields = lines[1].Split(',');
            if (fields.Length < 8)
            {
                throw new Exception("Template file does not have the expected format");
            }

            // Populate the form with template data
            Product_ParentCodeTextBox.Text = fields[0];
            SizeSuffixTextBox.Text = fields[1];
            DescriptionTextBox.Text = fields[2];

            // Set the product size based on the size suffix
            string sizeNumber = fields[1];
            for (int i = 0; i < ProductSizeComboBox.Items.Count; i++)
            {
                var item = (ProductSizeItem)ProductSizeComboBox.Items[i];
                if (item.Number == sizeNumber)
                {
                    ProductSizeComboBox.SelectedIndex = i;
                    break;
                }
            }

            // Check the corresponding size link
            string sizeLink = fields[3];
            for (int i = 0; i < SizeLinksListBox.Items.Count; i++)
            {
                var item = SizeLinksListBox.Items[i].ToString();
                if (item.StartsWith(sizeLink))
                {
                    SizeLinksListBox.SetItemChecked(i, true);
                    break;
                }
            }

            MarkupTextBox.Text = fields[4];
            CostPriceExclTextBox.Text = fields[5];
            SupplierCodeTextBox.Text = fields[6];

            // Set the supplier number
            string supplierNumber = fields[7];
            for (int i = 0; i < SupplierNumberComboBox.Items.Count; i++)
            {
                if (SupplierNumberComboBox.Items[i].ToString() == supplierNumber)
                {
                    SupplierNumberComboBox.SelectedIndex = i;
                    break;
                }
            }
        }

        private void CreateCSVFile(string filePath)
        {
            // Generate CSV content
            StringBuilder csv = new StringBuilder();

            // Add header row
            csv.AppendLine("Parent_Code,Size_Suffix,Description,Size_Link,Markup,Cost_Price_Excl,Supplier_Code,Supplier_Number");

            // Get form data
            string parentCode = Product_ParentCodeTextBox.Text.Trim();
            string description = DescriptionTextBox.Text.Trim();
            string sizeSuffix = SizeSuffixTextBox.Text.Trim();
            string markup = MarkupTextBox.Text.Trim();
            string costPriceExcl = CostPriceExclTextBox.Text.Trim();
            string supplierCode = SupplierCodeTextBox.Text.Trim();
            string supplierNumber = SupplierNumberComboBox.SelectedItem?.ToString() ?? "";

            // Create a row for each checked size link
            foreach (var checkedItem in SizeLinksListBox.CheckedItems)
            {
                // Extract just the size code part (before the dash)
                string fullText = checkedItem.ToString();
                string sizeCode = fullText.Contains("-") ?
                    fullText.Substring(0, fullText.IndexOf("-")).Trim() :
                    fullText;

                string line = $"{parentCode},{sizeSuffix},{description},{sizeCode},{markup},{costPriceExcl},{supplierCode},{supplierNumber}";
                csv.AppendLine(line);
            }

            // Write the CSV file
            File.WriteAllText(filePath, csv.ToString());
        }

        private bool ValidateForm()
        {
            // Perform validation on form data
            if (string.IsNullOrWhiteSpace(Product_ParentCodeTextBox.Text))
            {
                MessageBox.Show("Parent/Product Code is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(DescriptionTextBox.Text))
            {
                MessageBox.Show("Description is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (ProductSizeComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please select a Product Size.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (SizeLinksListBox.CheckedItems.Count == 0)
            {
                MessageBox.Show("Please select at least one Size Link.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Validate numeric fields
            if (!string.IsNullOrWhiteSpace(MarkupTextBox.Text) && !decimal.TryParse(MarkupTextBox.Text, out decimal markup))
            {
                MessageBox.Show("Markup must be a valid number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!string.IsNullOrWhiteSpace(CostPriceExclTextBox.Text) && !decimal.TryParse(CostPriceExclTextBox.Text, out decimal costPrice))
            {
                MessageBox.Show("Cost Price Excl must be a valid number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        #endregion
    }
}