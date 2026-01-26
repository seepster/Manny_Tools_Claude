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
using System.IO;
using Microsoft.Data.SqlClient;

namespace Manny_Tools_Claude
{
    public partial class ProductConfirmationForm : Form
    {
        private List<Orbit_ProductInfo> productsToCreate;
        private List<string> existingProductCodes;
        private List<Orbit_ProductInfo> successfullyCreatedProducts = new List<Orbit_ProductInfo>();
        private List<Orbit_ProductInfo> failedToCreateProducts = new List<Orbit_ProductInfo>();
        private List<Orbit_ProductInfo> productsToCreateConfirmed = new List<Orbit_ProductInfo>();
        private static readonly string LogFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "ProductCreation_Log.txt");

        public ProductConfirmationForm(List<Orbit_ProductInfo> productsToCreate, List<string> existingProductCodes)
        {
            InitializeComponent();
            this.productsToCreate = productsToCreate;
            this.existingProductCodes = existingProductCodes;

            LogToFile($"ProductConfirmationForm initialized with {productsToCreate.Count} products to create");
            QueryDatabaseSchema();

            SetupListView();
            LoadProductsToListView();
            CheckForConflicts();

            foreach (ListViewItem item in listViewProducts.Items)
            {
                item.Checked = true;
            }
        }

        private void SetupListView()
        {
            listViewProducts.View = View.Details;
            listViewProducts.FullRowSelect = true;
            listViewProducts.GridLines = true;
            listViewProducts.CheckBoxes = true;

            listViewProducts.Columns.Add("Product Code", 100);
            listViewProducts.Columns.Add("Description", 200);
            listViewProducts.Columns.Add("Size", 80);
            listViewProducts.Columns.Add("Cost Price Excl", 80);
            listViewProducts.Columns.Add("Markup", 80);
            listViewProducts.Columns.Add("Status", 100);
        }

        private void LoadProductsToListView()
        {
            listViewProducts.Items.Clear();

            foreach (var product in productsToCreate)
            {
                ListViewItem item = new ListViewItem(product.ProductCode);
                item.SubItems.Add(product.Description);
                item.SubItems.Add(product.UnitOfMeasure);
                item.SubItems.Add(product.CostPriceExcl);
                item.SubItems.Add(product.Markup);

                if (existingProductCodes.Contains(product.ProductCode))
                {
                    item.SubItems.Add("Conflict");
                    item.ForeColor = Color.Red;
                    item.Tag = "Conflict";
                }
                else
                {
                    item.SubItems.Add("Ready");
                    item.Tag = "Ready";
                }

                listViewProducts.Items.Add(item);
            }

            for (int i = 0; i < listViewProducts.Columns.Count; i++)
            {
                listViewProducts.Columns[i].Width = -2;
            }
        }

        private void CheckForConflicts()
        {
            int conflictCount = productsToCreate.Count(p => existingProductCodes.Contains(p.ProductCode));

            if (conflictCount > 0)
            {
                lblConflictWarning.Text = $"Warning: {conflictCount} product(s) already exist in the database. " +
                    "These are highlighted in red. Creating these products will override existing data.";
                lblConflictWarning.Visible = true;
            }
            else
            {
                lblConflictWarning.Visible = false;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnCreateProducts_Click(object sender, EventArgs e)
        {
            productsToCreateConfirmed.Clear();
            for (int i = 0; i < listViewProducts.Items.Count; i++)
            {
                if (listViewProducts.Items[i].Checked)
                {
                    productsToCreateConfirmed.Add(productsToCreate[i]);
                }
            }

            DialogResult confirmResult = MessageBox.Show(
                $"Are you sure you want to create {productsToCreateConfirmed.Count} products?\n\n" +
                (lblConflictWarning.Visible ? lblConflictWarning.Text + "\n\n" : "") +
                "This operation cannot be undone.",
                "Confirm Product Creation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmResult == DialogResult.Yes)
            {
                btnCreateProducts.Enabled = false;
                btnCancel.Enabled = false;
                progressBar.Visible = true;
                lblProgress.Visible = true;
                lblProgress.Text = "Creating products...";
                Application.DoEvents();

                CreateProducts();

                btnCancel.Enabled = true;
                progressBar.Visible = false;
                lblProgress.Text = $"Created {successfullyCreatedProducts.Count} of {productsToCreate.Count} products.";

                ShowResults();
            }
        }

        private void CreateProducts()
        {
            successfullyCreatedProducts.Clear();
            failedToCreateProducts.Clear();
            progressBar.Minimum = 0;
            progressBar.Maximum = productsToCreate.Count;
            progressBar.Value = 0;

            bool success = SQL_Get_Generic_List.ExecuteInTransaction(
                DatabaseConnectionManager.Instance.ConnectionString,
                (connection, transaction) =>
                {
                    for (int i = 0; i < productsToCreateConfirmed.Count; i++)
                    {
                        Orbit_ProductInfo product = productsToCreateConfirmed[i];
                        try
                        {
                            progressBar.Value = i + 1;
                            lblProgress.Text = $"Creating product {i + 1} of {productsToCreateConfirmed.Count}...";
                            Application.DoEvents();

                            LogToFile($"Processing product {product.ProductCode}: Description='{product.Description}', CostPrice={product.CostPriceExcl}, Markup={product.Markup}, Size={product.ProductSize}, SizeLink={product.SizeLink}");

                            bool productExists = existingProductCodes.Contains(product.ProductCode);
                            if (productExists)
                            {
                                LogToFile($"Product {product.ProductCode} exists - performing UPDATE operation");
                                UpdateProduct(connection, transaction, product);
                            }
                            else
                            {
                                LogToFile($"Product {product.ProductCode} is new - performing INSERT operation");
                                InsertProduct(connection, transaction, product);
                            }

                            successfullyCreatedProducts.Add(product);
                            listViewProducts.Items[i].SubItems[5].Text = "Created";
                            listViewProducts.Items[i].ForeColor = Color.Green;
                            LogToFile($"Successfully processed product {product.ProductCode}");
                        }
                        catch (Exception ex)
                        {
                            string errorMsg = $"Error creating product {product.ProductCode}: {ex.Message}";
                            if (ex.InnerException != null)
                                errorMsg += $" Inner Exception: {ex.InnerException.Message}";
                            
                            System.Diagnostics.Debug.WriteLine(errorMsg);
                            LogToFile($"ERROR: {errorMsg}");
                            LogToFile($"Stack Trace: {ex.StackTrace}");
                            
                            failedToCreateProducts.Add(product);
                            listViewProducts.Items[i].SubItems[5].Text = "Failed";
                            listViewProducts.Items[i].ForeColor = Color.Red;
                            throw;
                        }
                    }
                });

            if (!success)
            {
                for (int i = 0; i < productsToCreateConfirmed.Count; i++)
                {
                    if (listViewProducts.Items[i].SubItems[5].Text != "Failed")
                    {
                        listViewProducts.Items[i].SubItems[5].Text = "Failed (Transaction)";
                        listViewProducts.Items[i].ForeColor = Color.Red;
                    }
                }

                MessageBox.Show(
                    "There was an error creating products. The transaction was rolled back.",
                    "Creation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void UpdateProduct(IDbConnection connection, IDbTransaction transaction, Orbit_ProductInfo product)
        {
            // First verify SizeLink exists if provided
            int? sizeLink = null;
            if (!string.IsNullOrEmpty(product.SizeLink))
            {
                // Check if SizeLink is a valid number first
                if (!int.TryParse(product.SizeLink, out int sizeLinkId))
                {
                    throw new Exception($"Invalid SizeLink value: {product.SizeLink} - Must be a valid number");
                }

                string checkSizeLinkQuery = "SELECT OID FROM SizeLink WHERE OID = @SizeLink";
                var existingSizeLink = Dapper.SqlMapper.ExecuteScalar<int?>(
                    connection,
                    checkSizeLinkQuery,
                    new { SizeLink = sizeLinkId },
                    transaction);

                if (!existingSizeLink.HasValue)
                {
                    throw new Exception($"Invalid SizeLink value: {product.SizeLink} - Must reference a valid SizeLink.OID");
                }
                sizeLink = existingSizeLink.Value;
            }

            // Update ProductInfo table
            string updateProductInfoQuery = @"
                UPDATE ProductInfo
                SET 
                    ParentCode = @ParentCode,
                    Description = @Description,
                    CostPriceExcl = @CostPriceExcl,
                    Markup = @Markup,
                    ProductSize = @ProductSize,
                    SizeLink = @SizeLink,
                    DateUpdate = @DateUpdate
                WHERE ProductCode = @ProductCode";

            var productInfoParams = new
            {
                ParentCode = product.ParentCode,
                Description = product.Description,
                CostPriceExcl = float.Parse(product.CostPriceExcl),
                Markup = float.Parse(product.Markup),
                ProductSize = int.Parse(product.ProductSize),
                SizeLink = sizeLink,  // Validated ID or null
                DateUpdate = DateTime.Now,
                ProductCode = product.ProductCode
            };

            LogToFile($"Executing UPDATE ProductInfo for {product.ProductCode} with params: ParentCode={productInfoParams.ParentCode}, CostPriceExcl={productInfoParams.CostPriceExcl} (type: {productInfoParams.CostPriceExcl.GetType()}), Markup={productInfoParams.Markup} (type: {productInfoParams.Markup.GetType()}), ProductSize={productInfoParams.ProductSize} (type: {productInfoParams.ProductSize.GetType()}), SizeLink={productInfoParams.SizeLink}");
            
            int rowsAffected = Dapper.SqlMapper.Execute(connection, updateProductInfoQuery, productInfoParams, transaction);
            LogToFile($"UPDATE ProductInfo affected {rowsAffected} rows for product {product.ProductCode}");

            // Delete existing supplier links
            string deleteSupplierLinksQuery = @"
                DELETE FROM SupplierProductLinks 
                WHERE ProductCode = @ProductCode";

            int deletedRows = Dapper.SqlMapper.Execute(connection, deleteSupplierLinksQuery, new { ProductCode = product.ProductCode }, transaction);
            LogToFile($"Deleted {deletedRows} existing supplier links for product {product.ProductCode}");

            // Create a parameterized insert query
            string insertSupplierLinksQuery = @"
                INSERT INTO SupplierProductLinks
                (CreditorNumber, ProductCode, SupplierProductCode, ListedCostPrice, DefaultSupplier, DateCreated, DateUpdated)
                VALUES
                (CAST(@CreditorNumber AS VARCHAR(50)), @ProductCode, @SupplierProductCode, @ListedCostPrice, @DefaultSupplier, @DateCreated, @DateUpdated)";

            var supplierLinksParams = new
            {
                CreditorNumber = product.SupplierNumber,  // Keep as string
                ProductCode = product.ProductCode,
                SupplierProductCode = product.SupplierCode,
                ListedCostPrice = float.Parse(product.CostPriceExcl),
                DefaultSupplier = true,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now
            };

            LogToFile($"Executing INSERT SupplierProductLinks (UPDATE method) for {product.ProductCode} with params: CreditorNumber={supplierLinksParams.CreditorNumber} (type: {supplierLinksParams.CreditorNumber.GetType()}), ListedCostPrice={supplierLinksParams.ListedCostPrice} (type: {supplierLinksParams.ListedCostPrice.GetType()}), DefaultSupplier={supplierLinksParams.DefaultSupplier} (type: {supplierLinksParams.DefaultSupplier.GetType()})");
            
            int supplierRowsAffected = Dapper.SqlMapper.Execute(connection, insertSupplierLinksQuery, supplierLinksParams, transaction);
            LogToFile($"INSERT SupplierProductLinks (UPDATE method) affected {supplierRowsAffected} rows for product {product.ProductCode}");

            // Log price update
            string logQuery = @"
                INSERT INTO ProductPriceUpdateLog
                (Product, PriceNumber, OldPrice, NewPrice, Date, [User], WorkStation)
                VALUES
                (@Product, @PriceNumber, @OldPrice, @NewPrice, @Date, @User, @WorkStation)";

            var logParams = new
            {
                Product = product.ProductCode,
                PriceNumber = 1,
                OldPrice = 0.0f,
                NewPrice = float.Parse(product.CostPriceExcl),
                Date = DateTime.Now,
                User = 1, // Default user ID - database expects int, not string
                WorkStation = 1 // Default workstation ID - database expects int, not string
            };

            LogToFile($"Executing INSERT ProductPriceUpdateLog (UPDATE method) for {product.ProductCode} with params: PriceNumber={logParams.PriceNumber} (type: {logParams.PriceNumber.GetType()}), OldPrice={logParams.OldPrice} (type: {logParams.OldPrice.GetType()}), NewPrice={logParams.NewPrice} (type: {logParams.NewPrice.GetType()})");
            
            int logRowsAffected = Dapper.SqlMapper.Execute(connection, logQuery, logParams, transaction);
            LogToFile($"INSERT ProductPriceUpdateLog (UPDATE method) affected {logRowsAffected} rows for product {product.ProductCode}");
        }

        private void InsertProduct(IDbConnection connection, IDbTransaction transaction, Orbit_ProductInfo product)
        {
            if (product.SizeLink == "")
            {
                product.SizeLink = null;
            }


            // Verify SizeLink exists if provided
            int? sizeLink = null;
            if (!string.IsNullOrEmpty(product.SizeLink))
            {
                // Check if SizeLink is a valid number first
                if (!int.TryParse(product.SizeLink, out int sizeLinkId))
                {
                    throw new Exception($"Invalid SizeLink value: {product.SizeLink} - Must be a valid number");
                }

                string checkSizeLinkQuery = "SELECT OID FROM SizeLink WHERE OID = @SizeLink";
                var existingSizeLink = Dapper.SqlMapper.ExecuteScalar<int?>(
                    connection,
                    checkSizeLinkQuery,
                    new { SizeLink = sizeLinkId },
                    transaction);

                if (!existingSizeLink.HasValue)
                {
                    throw new Exception($"Invalid SizeLink value: {product.SizeLink} - Must reference a valid SizeLink.OID");
                }
                sizeLink = existingSizeLink.Value;
            }

            // Insert into ProductInfo table
            string insertProductInfoQuery = @"
                INSERT INTO ProductInfo
                (ProductCode, ParentCode, Description, UnitSize, CostPriceExcl, Markup, ProductSize, SizeLink, DateCreated, DateUpdate)
                VALUES
                (@ProductCode, @ParentCode, @Description, @UnitSize, @CostPriceExcl, @Markup, @ProductSize, @SizeLink, @DateCreated, @DateUpdate)";
           
            var productInfoParams = new
            {
                ProductCode = product.ProductCode,
                ParentCode = product.ParentCode,
                Description = product.Description,
                UnitSize = 1.0f,
                CostPriceExcl = float.Parse(product.CostPriceExcl),
                Markup = float.Parse(product.Markup),
                ProductSize = int.Parse(product.ProductSize),
                SizeLink = sizeLink,  // Validated ID or null
                DateCreated = DateTime.Now,
                DateUpdate = DateTime.Now
            };

            LogToFile($"Executing INSERT ProductInfo for {product.ProductCode} with params: ParentCode={productInfoParams.ParentCode}, UnitSize={productInfoParams.UnitSize} (type: {productInfoParams.UnitSize.GetType()}), CostPriceExcl={productInfoParams.CostPriceExcl} (type: {productInfoParams.CostPriceExcl.GetType()}), Markup={productInfoParams.Markup} (type: {productInfoParams.Markup.GetType()}), ProductSize={productInfoParams.ProductSize} (type: {productInfoParams.ProductSize.GetType()}), SizeLink={productInfoParams.SizeLink}");
            
            int rowsAffected = Dapper.SqlMapper.Execute(connection, insertProductInfoQuery, productInfoParams, transaction);
            LogToFile($"INSERT ProductInfo affected {rowsAffected} rows for product {product.ProductCode}");

            // Insert supplier link
            string insertSupplierLinksQuery = @"
                INSERT INTO SupplierProductLinks
                (CreditorNumber, ProductCode, SupplierProductCode, ListedCostPrice, DefaultSupplier, DateCreated, DateUpdated)
                VALUES
                (CAST(@CreditorNumber AS VARCHAR(50)), @ProductCode, @SupplierProductCode, @ListedCostPrice, @DefaultSupplier, @DateCreated, @DateUpdated)";

            var supplierLinksParams = new
            {
                CreditorNumber = product.SupplierNumber,  // Keep as string
                ProductCode = product.ProductCode,
                SupplierProductCode = product.SupplierCode,
                ListedCostPrice = float.Parse(product.CostPriceExcl),
                DefaultSupplier = true,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now
            };

            LogToFile($"Executing INSERT SupplierProductLinks (INSERT method) for {product.ProductCode} with params: CreditorNumber={supplierLinksParams.CreditorNumber} (type: {supplierLinksParams.CreditorNumber.GetType()}), ListedCostPrice={supplierLinksParams.ListedCostPrice} (type: {supplierLinksParams.ListedCostPrice.GetType()}), DefaultSupplier={supplierLinksParams.DefaultSupplier} (type: {supplierLinksParams.DefaultSupplier.GetType()})");
            
            int supplierRowsAffected = Dapper.SqlMapper.Execute(connection, insertSupplierLinksQuery, supplierLinksParams, transaction);
            LogToFile($"INSERT SupplierProductLinks (INSERT method) affected {supplierRowsAffected} rows for product {product.ProductCode}");

            // Log price update
            string logQuery = @"
                INSERT INTO ProductPriceUpdateLog
                (Product, PriceNumber, OldPrice, NewPrice, Date, [User], WorkStation)
                VALUES
                (@Product, @PriceNumber, @OldPrice, @NewPrice, @Date, @User, @WorkStation)";

            var logParams = new
            {
                Product = product.ProductCode,
                PriceNumber = 1,
                OldPrice = 0.0f,
                NewPrice = float.Parse(product.CostPriceExcl),
                Date = DateTime.Now,
                User = 1, // Default user ID - database expects int, not string
                WorkStation = 1 // Default workstation ID - database expects int, not string
            };

            LogToFile($"Executing INSERT ProductPriceUpdateLog (INSERT method) for {product.ProductCode} with params: PriceNumber={logParams.PriceNumber} (type: {logParams.PriceNumber.GetType()}), OldPrice={logParams.OldPrice} (type: {logParams.OldPrice.GetType()}), NewPrice={logParams.NewPrice} (type: {logParams.NewPrice.GetType()})");
            
            int logRowsAffected = Dapper.SqlMapper.Execute(connection, logQuery, logParams, transaction);
            LogToFile($"INSERT ProductPriceUpdateLog (INSERT method) affected {logRowsAffected} rows for product {product.ProductCode}");
        }

        private void ShowResults()
        {
            string message = $"Product Creation Results:\n\n" +
                             $"Successfully created: {successfullyCreatedProducts.Count}\n" +
                             $"Failed to create: {failedToCreateProducts.Count}";

            if (failedToCreateProducts.Count > 0)
            {
                message += "\n\nThe following products could not be created:";
                foreach (var failedProduct in failedToCreateProducts)
                {
                    message += $"\n- {failedProduct.ProductCode}: {failedProduct.Description}";
                }
            }

            MessageBox.Show(
                message,
                "Product Creation Complete",
                MessageBoxButtons.OK,
                failedToCreateProducts.Count > 0 ? MessageBoxIcon.Warning : MessageBoxIcon.Information);

            if (failedToCreateProducts.Count == 0)
            {
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                btnCreateProducts.Enabled = true;
                btnCreateProducts.Text = "Retry Failed Products";
            }
        }

        private void LogToFile(string message)
        {
            try
            {
                string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] {message}";
                File.AppendAllText(LogFilePath, logEntry + Environment.NewLine);
                System.Diagnostics.Debug.WriteLine(logEntry);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to write to log file: {ex.Message}");
            }
        }

        private void QueryDatabaseSchema()
        {
            try
            {
                LogToFile("=== Querying Database Schema ===");
                
                using (SqlConnection connection = new SqlConnection(DatabaseConnectionManager.Instance.ConnectionString))
                {
                    connection.Open();
                    
                    LogToFile("=== ProductInfo Table Schema ===");
                    QueryTableSchema(connection, "ProductInfo");
                    
                    LogToFile("=== SupplierProductLinks Table Schema ===");
                    QueryTableSchema(connection, "SupplierProductLinks");
                    
                    LogToFile("=== ProductPriceUpdateLog Table Schema ===");
                    QueryTableSchema(connection, "ProductPriceUpdateLog");
                }
            }
            catch (Exception ex)
            {
                LogToFile($"Error querying database schema: {ex.Message}");
            }
        }
        
        private void QueryTableSchema(SqlConnection connection, string tableName)
        {
            string query = @"
                SELECT 
                    COLUMN_NAME,
                    DATA_TYPE,
                    CHARACTER_MAXIMUM_LENGTH,
                    NUMERIC_PRECISION,
                    NUMERIC_SCALE,
                    IS_NULLABLE
                FROM INFORMATION_SCHEMA.COLUMNS
                WHERE TABLE_NAME = @TableName
                ORDER BY ORDINAL_POSITION";
                
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@TableName", tableName);
                
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string columnName = reader["COLUMN_NAME"].ToString();
                        string dataType = reader["DATA_TYPE"].ToString();
                        object maxLength = reader["CHARACTER_MAXIMUM_LENGTH"];
                        object precision = reader["NUMERIC_PRECISION"];
                        object scale = reader["NUMERIC_SCALE"];
                        string nullable = reader["IS_NULLABLE"].ToString();
                        
                        string columnInfo = $"{columnName}: {dataType}";
                        
                        if (maxLength != DBNull.Value)
                            columnInfo += $"({maxLength})";
                        else if (precision != DBNull.Value && scale != DBNull.Value)
                            columnInfo += $"({precision},{scale})";
                            
                        columnInfo += $" - Nullable: {nullable}";
                        LogToFile(columnInfo);
                    }
                }
            }
        }
    }
}