using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Manny_Tools_Claude.DTO;

namespace Manny_Tools_Claude
{
    public partial class ProductReviewForm : Form
    {
        #region Fields

        private List<KingsleyProductItem> _productsToCreate;
        private List<string> _existingProductCodes;
        private string _connectionString;
        private BindingSource _reviewBindingSource;
        private ProductCreationHelper _productCreationHelper;
        private List<ProductCreationResult> _creationResults;

        #endregion

        #region Constructor

        public ProductReviewForm(List<KingsleyProductItem> productsToCreate, string connectionString)
        {
            InitializeComponent();

            _productsToCreate = productsToCreate ?? new List<KingsleyProductItem>();
            _connectionString = connectionString;
            _existingProductCodes = new List<string>();
            _reviewBindingSource = new BindingSource();
            _productCreationHelper = new ProductCreationHelper(_connectionString, 1, 0); // Default user ID = 1, workstation = 0
            _creationResults = new List<ProductCreationResult>();

            InitializeForm();
            CheckForExistingProducts();
            LoadProductReview();
        }

        #endregion

        #region Initialization

        private void InitializeForm()
        {
            this.Text = $"Review Products - {_productsToCreate.Count} product(s) to create";
            
            // Setup the data grid view
            dgvProductReview.AutoGenerateColumns = false;
            dgvProductReview.AllowUserToAddRows = false;
            dgvProductReview.AllowUserToDeleteRows = false;
            dgvProductReview.ReadOnly = true;
            dgvProductReview.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProductReview.MultiSelect = false;
            
            // Create columns
            CreateDataGridColumns();
            
            // Bind data source
            _reviewBindingSource.DataSource = _productsToCreate;
            dgvProductReview.DataSource = _reviewBindingSource;
        }

        private void CreateDataGridColumns()
        {
            // Product Code column with override warning
            var productCodeColumn = new DataGridViewTextBoxColumn
            {
                Name = "ProductCode",
                HeaderText = "Product Code",
                DataPropertyName = "ProductCode",
                Width = 100,
                ReadOnly = true
            };
            dgvProductReview.Columns.Add(productCodeColumn);

            // Status column (New/Override)
            var statusColumn = new DataGridViewTextBoxColumn
            {
                Name = "Status",
                HeaderText = "Status",
                Width = 80,
                ReadOnly = true
            };
            dgvProductReview.Columns.Add(statusColumn);

            // Description column
            var descriptionColumn = new DataGridViewTextBoxColumn
            {
                Name = "Description",
                HeaderText = "Description",
                DataPropertyName = "Description",
                Width = 200,
                ReadOnly = true
            };
            dgvProductReview.Columns.Add(descriptionColumn);

            // Unit of Measure column
            var unitOfMeasureColumn = new DataGridViewTextBoxColumn
            {
                Name = "UnitOfMeasure",
                HeaderText = "Unit of Measure",
                DataPropertyName = "UnitOfMeasure",
                Width = 120,
                ReadOnly = true
            };
            dgvProductReview.Columns.Add(unitOfMeasureColumn);

            // Brand Name column
            var brandColumn = new DataGridViewTextBoxColumn
            {
                Name = "BrandName",
                HeaderText = "Brand",
                DataPropertyName = "BrandName",
                Width = 120,
                ReadOnly = true
            };
            dgvProductReview.Columns.Add(brandColumn);

            // Department column
            var departmentColumn = new DataGridViewTextBoxColumn
            {
                Name = "Department",
                HeaderText = "Department",
                DataPropertyName = "Department",
                Width = 100,
                ReadOnly = true
            };
            dgvProductReview.Columns.Add(departmentColumn);

            // Cost Price column - no formatting, display exact values
            var costPriceColumn = new DataGridViewTextBoxColumn
            {
                Name = "CostPriceExcl",
                HeaderText = "Cost Price",
                DataPropertyName = "CostPriceExcl",
                Width = 80,
                ReadOnly = true
            };
            dgvProductReview.Columns.Add(costPriceColumn);

            // Selling Price column - no formatting, display exact values
            var sellingPriceColumn = new DataGridViewTextBoxColumn
            {
                Name = "SellingPriceIncl",
                HeaderText = "Selling Price",
                DataPropertyName = "SellingPriceIncl",
                Width = 80,
                ReadOnly = true
            };
            dgvProductReview.Columns.Add(sellingPriceColumn);

            // Markup column - no formatting, display exact values
            var markupColumn = new DataGridViewTextBoxColumn
            {
                Name = "Markup",
                HeaderText = "Markup %",
                DataPropertyName = "Markup",
                Width = 80,
                ReadOnly = true
            };
            dgvProductReview.Columns.Add(markupColumn);

            // Supplier Number column
            var supplierColumn = new DataGridViewTextBoxColumn
            {
                Name = "SupplierNumber",
                HeaderText = "Supplier",
                DataPropertyName = "SupplierNumber",
                Width = 80,
                ReadOnly = true
            };
            dgvProductReview.Columns.Add(supplierColumn);
        }

        #endregion

        #region Data Loading

        private void CheckForExistingProducts()
        {
            if (string.IsNullOrEmpty(_connectionString))
                return;

            try
            {
                // Get all existing product codes
                string query = "SELECT ProductCode FROM ProductInfo";
                _existingProductCodes = SQL_Get_Generic_List.ExecuteQuery<string>(_connectionString, query, null) ?? new List<string>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error checking existing products: {ex.Message}", "Warning", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void LoadProductReview()
        {
            // Update summary labels
            lblTotalProducts.Text = $"Total Products: {_productsToCreate.Count}";
            
            int newProducts = 0;
            int overrideProducts = 0;

            foreach (var product in _productsToCreate)
            {
                if (_existingProductCodes.Contains(product.ProductCode))
                {
                    overrideProducts++;
                }
                else
                {
                    newProducts++;
                }
            }

            lblNewProducts.Text = $"New Products: {newProducts}";
            lblOverrideProducts.Text = $"Override Products: {overrideProducts}";

            // Show warning if there are overrides
            if (overrideProducts > 0)
            {
                lblWarning.Text = $"WARNING: {overrideProducts} product(s) will override existing data!";
                lblWarning.ForeColor = Color.Red;
                lblWarning.Visible = true;
            }
            else
            {
                lblWarning.Visible = false;
            }

            // Update the data grid
            _reviewBindingSource.ResetBindings(false);
            
            // Color rows based on status
            ColorRowsByStatus();
        }

        private void ColorRowsByStatus()
        {
            foreach (DataGridViewRow row in dgvProductReview.Rows)
            {
                if (row.DataBoundItem is KingsleyProductItem product)
                {
                    // Set status text
                    bool isOverride = _existingProductCodes.Contains(product.ProductCode);
                    row.Cells["Status"].Value = isOverride ? "OVERRIDE" : "NEW";

                    // Color the row
                    if (isOverride)
                    {
                        row.DefaultCellStyle.BackColor = Color.LightPink;
                        row.DefaultCellStyle.ForeColor = Color.DarkRed;
                        row.Cells["Status"].Style.BackColor = Color.Red;
                        row.Cells["Status"].Style.ForeColor = Color.White;
                    }
                    else
                    {
                        row.DefaultCellStyle.BackColor = Color.LightGreen;
                        row.DefaultCellStyle.ForeColor = Color.DarkGreen;
                        row.Cells["Status"].Style.BackColor = Color.Green;
                        row.Cells["Status"].Style.ForeColor = Color.White;
                    }
                }
            }
        }

        #endregion

        #region Event Handlers

        private void btnCreateProducts_Click(object sender, EventArgs e)
        {
            if (_productsToCreate.Count == 0)
            {
                MessageBox.Show("No products to create.", "Information", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Show confirmation dialog
            string message = $"Are you sure you want to create {_productsToCreate.Count} product(s)?";
            
            int overrideCount = _productsToCreate.Count(p => _existingProductCodes.Contains(p.ProductCode));
            if (overrideCount > 0)
            {
                message += $"\n\nThis will OVERRIDE {overrideCount} existing product(s).";
            }

            DialogResult result = MessageBox.Show(message, "Confirm Product Creation", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                CreateProducts();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void dgvProductReview_SelectionChanged(object sender, EventArgs e)
        {
            // Show detailed information for selected product
            if (dgvProductReview.SelectedRows.Count > 0)
            {
                var selectedProduct = dgvProductReview.SelectedRows[0].DataBoundItem as KingsleyProductItem;
                if (selectedProduct != null)
                {
                    ShowProductDetails(selectedProduct);
                }
            }
        }

        private void btnRemoveProduct_Click(object sender, EventArgs e)
        {
            // Check if a product is selected
            if (dgvProductReview.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a product to remove.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Get the selected product
            var selectedProduct = dgvProductReview.SelectedRows[0].DataBoundItem as KingsleyProductItem;
            if (selectedProduct == null)
                return;

            // Confirm removal
            DialogResult result = MessageBox.Show(
                $"Are you sure you want to remove this product from the review list?\n\nProduct Code: {selectedProduct.ProductCode}\nDescription: {selectedProduct.Description}",
                "Confirm Removal",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Remove from list
                _productsToCreate.Remove(selectedProduct);

                // Refresh the binding source
                _reviewBindingSource.ResetBindings(false);

                // Reload the review data to update counts and colors
                LoadProductReview();

                // Clear product details if list is empty
                if (_productsToCreate.Count == 0)
                {
                    txtProductDetails.Text = "No products to review.";
                    MessageBox.Show("All products have been removed. Closing review form.",
                        "No Products", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                }
            }
        }

        #endregion

        #region Product Creation

        private void CreateProducts()
        {
            btnCreateProducts.Enabled = false;
            btnCancel.Enabled = false;
            progressBar.Visible = true;
            progressBar.Maximum = _productsToCreate.Count;
            progressBar.Value = 0;

            _creationResults.Clear();
            int successCount = 0;
            int failureCount = 0;
            int newCount = 0;
            int updateCount = 0;

            try
            {
                foreach (var product in _productsToCreate)
                {
                    // Use ProductCreationHelper to create/update the product
                    ProductCreationResult result = _productCreationHelper.CreateOrUpdateProduct(product);
                    _creationResults.Add(result);

                    if (result.IsSuccess)
                    {
                        successCount++;
                        if (result.IsNew)
                            newCount++;
                        else
                            updateCount++;
                    }
                    else
                    {
                        failureCount++;
                    }

                    progressBar.Value++;
                    Application.DoEvents(); // Allow UI to update
                }

                // Show enhanced results
                ShowEnhancedCreationResults(successCount, failureCount, newCount, updateCount);

                if (failureCount == 0)
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            finally
            {
                btnCreateProducts.Enabled = true;
                btnCancel.Enabled = true;
                progressBar.Visible = false;
            }
        }

        #endregion

        #region UI Helpers

        private void ShowProductDetails(KingsleyProductItem product)
        {
            var details = new StringBuilder();
            details.AppendLine($"Product Code: {product.ProductCode}");
            details.AppendLine($"Description: {product.Description}");
            details.AppendLine($"Brand: {product.BrandName}");
            details.AppendLine($"Department: {product.Department}");
            details.AppendLine($"Unit Size: {product.UnitSize}");
            details.AppendLine($"Unit of Measure: {product.UnitOfMeasure}");
            details.AppendLine($"Cost Price Excl: {product.CostPriceExcl:F2}");
            details.AppendLine($"Selling Price Incl: {product.SellingPriceIncl:F2}");
            details.AppendLine($"Markup: {product.Markup:F2}%");
            details.AppendLine($"Stock Item: {(product.StockItem == "1" ? "Yes" : "No")}");
            details.AppendLine($"Till Item: {(product.TillItem == "1" ? "Yes" : "No")}");
            details.AppendLine($"Discontinued: {(product.Discontinued == "1" ? "Yes" : "No")}");
            
            if (!string.IsNullOrEmpty(product.SupplierNumber))
            {
                details.AppendLine($"Supplier: {product.SupplierNumber}");
                details.AppendLine($"Supplier Code: {product.SupplierCode}");
            }

            txtProductDetails.Text = details.ToString();
        }

        private void ShowEnhancedCreationResults(int successCount, int failureCount, int newCount, int updateCount)
        {
            var message = new StringBuilder();
            message.AppendLine("=== Product Creation Complete ===");
            message.AppendLine();
            message.AppendLine($"Total Products Processed: {_creationResults.Count}");
            message.AppendLine($"Successful: {successCount}");
            message.AppendLine($"  - New Products Created: {newCount}");
            message.AppendLine($"  - Existing Products Updated: {updateCount}");
            message.AppendLine($"Failed: {failureCount}");
            message.AppendLine();

            if (_creationResults.Count > 0)
            {
                message.AppendLine("=== Detailed Results ===");
                foreach (var result in _creationResults)
                {
                    string statusIcon = result.IsSuccess ? "✓" : "✗";
                    string status = result.IsNew ? "NEW" : "UPDATE";

                    if (result.IsSuccess)
                    {
                        message.AppendLine($"{statusIcon} [{status}] {result.ProductCode}: {result.Description}");
                    }
                    else
                    {
                        message.AppendLine($"{statusIcon} [{status}] {result.ProductCode}: FAILED - {result.ErrorMessage}");
                    }
                }
            }

            string title = failureCount == 0 ? "Success" : "Partial Success";
            MessageBoxIcon icon = failureCount == 0 ? MessageBoxIcon.Information : MessageBoxIcon.Warning;

            MessageBox.Show(message.ToString(), title, MessageBoxButtons.OK, icon);
        }

        #endregion
    }
}