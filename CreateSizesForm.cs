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
using System.Drawing.Text;
using System.Globalization;
using Manny_Tools_Claude.DTO;

namespace Manny_Tools_Claude
{
    public partial class CreateSizesForm : UserControl
    {
        // Define fields to store form data
        private List<CreateSizesProductSizeItem> productSizes = new List<CreateSizesProductSizeItem>();
        private List<CreateSizesSupplierItem> suppliers = new List<CreateSizesSupplierItem>();
        private List<string> productCodes = new List<string>();
        private List<string> parentCodes = new List<string>();
        private List<string> sixDigitParentCodes = new List<string>();
        private CreateSizesProductSizeItem selectedProductSize = new CreateSizesProductSizeItem();
        private CreateSizesSupplierItem selectedSupplier = new CreateSizesSupplierItem();
        private CreateSizesTemplateItem activeTemplate = new CreateSizesTemplateItem();
        private int nextAvailableParentCode = 100000;
        private string Department = string.Empty;
        private string TaxValue = string.Empty;
        private string _connectionString;


        public CreateSizesForm()
        {
            InitializeComponent();

            // Get connection string from DatabaseConnectionManager
            _connectionString = DatabaseConnectionManager.Instance.ConnectionString;

            try
            {
                // If connection string is empty, try to load it first
                if (string.IsNullOrEmpty(_connectionString))
                {
                    TryLoadConnectionString();
                }

                LoadProductSizes();
                LoadSuppliers();
            }
            catch
            {
                // Silently handle errors without showing popup
                System.Diagnostics.Debug.WriteLine("Some elements could not be loaded in CreateSizesForm");
            }
        }

        // Method to update connection string
        public void UpdateConnectionString(string connectionString)
        {
            _connectionString = connectionString;
        }

        private bool TryLoadConnectionString()
        {
            try
            {
                string appDataPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "MannyTools");

                string configPath = Path.Combine(appDataPath, "connection.cfg");

                if (File.Exists(configPath))
                {
                    // Read connection string
                    _connectionString = File.ReadAllText(configPath);

                    // Update the connection manager instance
                    DatabaseConnectionManager.Instance.UpdateConnectionString(_connectionString);

                    return true;
                }
            }
            catch
            {
                // Ignore errors - connection settings will be requested if needed
            }

            return false;
        }

        public void LoadProductSizes()
        {
            ProductSizeComboBox.Items.Clear();

            // Check if connection string is available
            if (string.IsNullOrEmpty(_connectionString))
            {
                if (!TryLoadConnectionString())
                {
                    // Log error instead of showing popup
                    System.Diagnostics.Debug.WriteLine("Database connection not configured for loading product sizes");
                    return;
                }
            }

            //load Product Size dropdown from sql connection
            string queryForProductSize = "select OID, Number, Description from ProductSize";
            try
            {
                // Use the local connection string rather than retrieving again
                productSizes = SQL_Get_Generic_List.ExecuteQuery<CreateSizesProductSizeItem>(_connectionString, queryForProductSize, null);

                if (productSizes != null && productSizes.Count > 0)
                {
                    foreach (CreateSizesProductSizeItem item in productSizes)
                    {
                        ProductSizeComboBox.Items.Add(item.ToString());
                    }
                }
                else
                {
                    // Log instead of throwing exception
                    System.Diagnostics.Debug.WriteLine("No product sizes found in database");
                }
            }
            catch (Exception ex)
            {
                // Log error instead of showing popup
                System.Diagnostics.Debug.WriteLine($"Error loading product sizes: {ex.Message}");
            }
        }



        public void LoadSuppliers()
        {
            SupplierComboBox.Items.Clear();

            // Check if connection string is available
            if (string.IsNullOrEmpty(_connectionString))
            {
                if (!TryLoadConnectionString())
                {
                    // Log error instead of showing popup
                    System.Diagnostics.Debug.WriteLine("Database connection not configured for loading suppliers");
                    return;
                }
            }

            try
            {
                //load suppliers list from sql connection - ensure we get numeric account numbers
                string queryForSuppliers = @"
                    SELECT DISTINCT s.AccountNumber, s.AccountName 
                    FROM Supplier s
                    INNER JOIN SupplierProductLinks spl ON s.AccountNumber = CAST(spl.CreditorNumber AS VARCHAR(50))
                    ORDER BY s.AccountNumber";
                suppliers = SQL_Get_Generic_List.ExecuteQuery<CreateSizesSupplierItem>(_connectionString, queryForSuppliers, null);

                if (suppliers != null && suppliers.Count > 0)
                {
                    foreach (CreateSizesSupplierItem supplier in suppliers)
                    {
                        SupplierComboBox.Items.Add(supplier.ToString());
                    }
                    selectedSupplier = suppliers[0];
                }
                else
                {
                    // Log instead of throwing exception
                    System.Diagnostics.Debug.WriteLine("No suppliers found in database");
                }
            }
            catch (Exception ex)
            {
                // Log error instead of showing popup
                System.Diagnostics.Debug.WriteLine($"Error loading suppliers: {ex.Message}");
            }
        }

        public void LoadParentCodes()
        {
            // Check if connection string is available
            if (string.IsNullOrEmpty(_connectionString))
            {
                if (!TryLoadConnectionString())
                {
                    MessageBox.Show("Database connection not configured. Please set up the connection in settings.",
                        "Connection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            try
            {
                string queryForParentCodes = "select ParentCode from ProductInfo order by ParentCode asc";
                parentCodes = SQL_Get_Generic_List.ExecuteQuery<string>(_connectionString, queryForParentCodes, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading parent codes: {ex.Message}",
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void LoadProductCodes()
        {
            // Check if connection string is available
            if (string.IsNullOrEmpty(_connectionString))
            {
                if (!TryLoadConnectionString())
                {
                    MessageBox.Show("Database connection not configured. Please set up the connection in settings.",
                        "Connection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            try
            {
                string queryForProductCodes = "select ProductCode from ProductInfo";
                productCodes = SQL_Get_Generic_List.ExecuteQuery<string>(_connectionString, queryForProductCodes, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading product codes: {ex.Message}",
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void CreateActiveTemplate(string ProductParentCode)
        {
            // Check if connection string is available
            if (string.IsNullOrEmpty(_connectionString))
            {
                if (!TryLoadConnectionString())
                {
                    MessageBox.Show("Database connection not configured. Please set up the connection in settings.",
                        "Connection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            string productCode = ProductParentCode;
            if (productCode != null && productCode != string.Empty)
            {
                try
                {
                    string queryProductCodeParentCode = $"select top 1 ParentCode from ProductInfo where ProductCode = '{productCode}'";
                    int querySingleResultParentCode = SQL_Get_Generic_List.ExecuteSingle<int>(_connectionString, queryProductCodeParentCode, null);
                    if (!(querySingleResultParentCode >= 0))
                    {
                        MessageBox.Show("Invalid Product/Parent Code for template.");
                        return;
                    }

                    int ParentCode = querySingleResultParentCode;
                    string queryProductInfoUsingParentCode = $"select ParentCode, Description, ProductSize, Department, Markup, CostPriceExcl, SizeLink from ProductInfo where ProductCode = '{ParentCode}' ";
                    CreateSizesTemplateItem productInfo = SQL_Get_Generic_List.ExecuteSingle<CreateSizesTemplateItem>(_connectionString, queryProductInfoUsingParentCode, null);
                    if (productInfo == null)
                    {
                        MessageBox.Show("Invalid Product/Parent Code for template.");
                        return;
                    }

                    activeTemplate.ParentCode = productInfo.ParentCode;
                    activeTemplate.Description = productInfo.Description;
                    activeTemplate.Markup = productInfo.Markup;
                    activeTemplate.CostPriceExcl = productInfo.CostPriceExcl;
                    activeTemplate.Department = productInfo.Department;
                    activeTemplate.SizeLink = productInfo.SizeLink;

                    activeTemplate.ProductSize = productInfo.ProductSize;

                    string querySupplierProductLinks = $"select top 1 SupplierProductcode from supplierproductlinks where ProductCode = '{ParentCode}'";
                    string supplierProductCode = SQL_Get_Generic_List.ExecuteSingle<string>(_connectionString, querySupplierProductLinks, null);
                    activeTemplate.SupplierCode = supplierProductCode;

                    string querySupplierNumber = $"select top 1 CreditorNumber from supplierproductlinks where ProductCode = '{ParentCode}'";
                    string supplierNumber = SQL_Get_Generic_List.ExecuteSingle<string>(_connectionString, querySupplierNumber, null);
                    string querySupplierDetails = $"select top 1 AccountNumber, AccountName from Supplier where AccountNumber = '{supplierNumber}'";
                    activeTemplate.Supplier = SQL_Get_Generic_List.ExecuteSingle<CreateSizesSupplierItem>(_connectionString, querySupplierDetails, null);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading template: {ex.Message}",
                        "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ProductSizeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedProductSize = productSizes[ProductSizeComboBox.SelectedIndex];

        }



        private void LoadTemplateButton_Click(object sender, EventArgs e)
        {
            CreateActiveTemplate(Product_ParentCodeTextBox.Text);

            LoadTemplate();

        }

        private void LoadTemplate()
        {
            if (activeTemplate.ParentCode != null)
            {
                if (activeTemplate.Description != null)
                {
                    DescriptionTextBox.Text = activeTemplate.Description;

                }
                if (activeTemplate.SupplierCode != null)
                {
                    SupplierCodeTextBox.Text = activeTemplate.SupplierCode;
                }
                if (activeTemplate.ProductSize != string.Empty)
                {
                    for (int i = 0; i < productSizes.Count; i++)
                    {
                        try
                        {
                            if (productSizes[i].Number == int.Parse(activeTemplate.ProductSize))
                            {
                                ProductSizeComboBox.SelectedIndex = i;
                                break;
                            }
                        }
                        catch
                        {
                            MessageBox.Show("Invalid Product Size.");
                            break;
                        }
                        
                    }


                }
                MarkupTextBox.Text = activeTemplate.Markup.ToString();
                CostPriceExclTextBox.Text = activeTemplate.CostPriceExcl.ToString();

                if (activeTemplate.Supplier.AccountNumber != string.Empty)
                {
                    for (int i = 0; i < suppliers.Count; i++)
                    {
                        if (suppliers[i].AccountNumber == activeTemplate.Supplier.AccountNumber)
                        {
                            SupplierComboBox.SelectedIndex = i;
                            break;
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("No template to load.");
            }


        }

        private void SupplierNumberComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedSupplier = suppliers[SupplierComboBox.SelectedIndex];
        }

        private void ClearTemplateButton_Click(object sender, EventArgs e)
        {

            DescriptionTextBox.Text = string.Empty;
            SupplierCodeTextBox.Text = string.Empty;
            MarkupTextBox.Text = string.Empty;
            CostPriceExclTextBox.Text = string.Empty;
            if (ProductSizeComboBox.Items.Count >= 0)
                ProductSizeComboBox.SelectedIndex = 0;
            if (SupplierComboBox.Items.Count >= 0)
                SupplierComboBox.SelectedIndex = 0;
        }

        private void NextAvailableParentCodeButton_Click(object sender, EventArgs e)
        {
            // Check if connection string is available
            if (string.IsNullOrEmpty(_connectionString))
            {
                if (!TryLoadConnectionString())
                {
                    MessageBox.Show("Database connection not configured. Please set up the connection in settings.",
                        "Connection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            try
            {
                string queryParentCode = "select ParentCode from ProductInfo";
                parentCodes = SQL_Get_Generic_List.ExecuteQuery<string>(_connectionString, queryParentCode, null);

                // Clear existing list to avoid duplicates
                sixDigitParentCodes.Clear();

                for (int i = 0; i < parentCodes.Count; i++)
                {
                    if (parentCodes[i] != null)
                    {
                        if (parentCodes[i].Length == 6)
                        {
                            if (sixDigitParentCodes.Contains(parentCodes[i]) == false)
                            {
                                sixDigitParentCodes.Add(parentCodes[i]);
                            }
                        }
                    }
                }

                //first we need to sort the sixDigitParentCodes list in ascending order
                sixDigitParentCodes.Sort();

                //check what is the next available integer in the sixDigitParentCodes list starting at 100000
                LoadProductCodes();
                nextAvailableParentCode = 100000; // Reset to start value

                while (true)
                {
                    if (parentCodes.Contains(nextAvailableParentCode.ToString()))
                    {
                        nextAvailableParentCode++;
                    }
                    else
                    {
                        if (!productCodes.Contains(nextAvailableParentCode.ToString()))
                        {
                            break;
                        }
                        else
                        {
                            nextAvailableParentCode++;
                        }
                    }
                }

                Product_ParentCodeTextBox.Text = nextAvailableParentCode.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error finding next available parent code: {ex.Message}",
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CreateSizesButton_Click(object sender, EventArgs e)
        {
            // 1. Check for required fields
            if (string.IsNullOrWhiteSpace(DescriptionTextBox.Text))
            {
                MessageBox.Show("Description is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                DescriptionTextBox.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(MarkupTextBox.Text) || !double.TryParse(MarkupTextBox.Text, CultureInfo.InvariantCulture,  out double markup))
            {
                MessageBox.Show("Valid Markup is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                MarkupTextBox.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(CostPriceExclTextBox.Text) || !double.TryParse(CostPriceExclTextBox.Text, CultureInfo.InvariantCulture, out double costPrice))
            {
                MessageBox.Show("Valid Cost Price Excl is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CostPriceExclTextBox.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(Product_ParentCodeTextBox.Text))
            {
                MessageBox.Show("Parent/Product Code is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Product_ParentCodeTextBox.Focus();
                return;
            }

            // 2, 3, 4. Handle different code scenarios
            string enteredCode = Product_ParentCodeTextBox.Text.Trim();

            // Check if connection string is available
            if (string.IsNullOrEmpty(_connectionString))
            {
                if (!TryLoadConnectionString())
                {
                    MessageBox.Show("Database connection not configured. Please set up the connection in settings.",
                        "Connection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            // Check if the entered code is a valid ParentCode
            string checkParentCodeQuery = $"SELECT COUNT(*) FROM ProductInfo WHERE ParentCode = '{enteredCode}'";
            int parentCodeCount = SQL_Get_Generic_List.ExecuteScalar<int>(
                _connectionString,
                checkParentCodeQuery,
                null);

            if (parentCodeCount > 0)
            {
                // 2. It's a valid ParentCode
                DialogResult result = MessageBox.Show(
                    "This is an existing Parent Code. Would you like to expand on the existing sizes?\n\n" +
                    "Click 'Yes' to add sizes to this Parent Code.\n" +
                    "Click 'No' to create a new Parent Code using this as a template.",
                    "Existing Parent Code",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // User wants to expand existing ParentCode's sizes
                    List<string> existingSizes = GetExistingSizes(enteredCode);
                    ShowExistingSizesAndContinue(enteredCode, existingSizes, false);
                }
                else if (result == DialogResult.No)
                {
                    // User wants to use existing ParentCode as template for a new one
                    NextAvailableParentCodeButton_Click(sender, e);
                    ShowSizingMethodWindow(nextAvailableParentCode.ToString(), true);
                }

                return;
            }

            // Check if the entered code is a valid ProductCode
            string checkProductCodeQuery = $"SELECT ParentCode FROM ProductInfo WHERE ProductCode = '{enteredCode}'";
            string parentCodeFromProduct = SQL_Get_Generic_List.ExecuteSingle<string>(
                _connectionString,
                checkProductCodeQuery,
                null);

            if (!string.IsNullOrEmpty(parentCodeFromProduct))
            {
                // 4. It's a valid ProductCode
                DialogResult result = MessageBox.Show(
                    "This is an existing Product Code. Would you like to expand on its Parent Code sizes?\n\n" +
                    "Click 'Yes' to add sizes to this Product's Parent Code.\n" +
                    "Click 'No' to create a new Parent Code using this Product's Parent Code as a template.",
                    "Existing Product Code",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // User wants to expand existing ParentCode's sizes
                    List<string> existingSizes = GetExistingSizes(parentCodeFromProduct);
                    ShowExistingSizesAndContinue(parentCodeFromProduct, existingSizes, false);
                }
                else if (result == DialogResult.No)
                {
                    // User wants to use existing ParentCode as template for a new one
                    NextAvailableParentCodeButton_Click(sender, e);
                    ShowSizingMethodWindow(nextAvailableParentCode.ToString(), true);
                }

                return;
            }

            // 3. Check if it's a valid 6-digit new Parent Code
            if (enteredCode.Length == 6 && int.TryParse(enteredCode, out int codeValue))
            {
                // Assume the user wants to create this new ParentCode
                ShowSizingMethodWindow(enteredCode, true);
                return;
            }

            // If we get here, the code is invalid
            MessageBox.Show(
                "The entered code is not a valid Parent Code, Product Code, or 6-digit new Parent Code.",
                "Invalid Code",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        private List<string> GetExistingSizes(string parentCode)
        {
            List<string> existingSizes = new List<string>();

            // Check if connection string is available
            if (string.IsNullOrEmpty(_connectionString))
            {
                if (!TryLoadConnectionString())
                {
                    MessageBox.Show("Database connection not configured. Please set up the connection in settings.",
                        "Connection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return existingSizes;
                }
            }

            try
            {
                string query = $"SELECT ProductCode, Description, UnitOfMeasure FROM ProductInfo WHERE ParentCode = '{parentCode}' and ProductCode like '%{parentCode}' order by Convert(numeric, ProductCode)";

                // Define an anonymous class to match the query results
                var results = SQL_Get_Generic_List.ExecuteQuery<dynamic>(
                    _connectionString,
                    query,
                    null);

                // Convert the results to strings
                foreach (var result in results)
                {
                    string sizeDisplay = $"{result.ProductCode} - {result.Description}";
                    if (!string.IsNullOrEmpty(result.UnitOfMeasure))
                    {
                        sizeDisplay += $" ({result.UnitOfMeasure})";
                    }
                    existingSizes.Add(sizeDisplay);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving existing sizes: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return existingSizes;
        }

        private void ShowExistingSizesAndContinue(string parentCode, List<string> existingSizes, bool isNew)
        {
            try
            {
                // Create and show the existing sizes form
                using (ExistingSizesForm existingSizesForm = new ExistingSizesForm(parentCode, existingSizes))
                {
                    if (existingSizesForm.ShowDialog() == DialogResult.OK)
                    {
                        // Continue to the sizing method window
                        ShowSizingMethodWindow(parentCode, isNew);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error showing existing sizes: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowSizingMethodWindow(string parentCode, bool isNew)
        {
            try
            {
               

                // Create and show the sizing method form
                using (SizingMethodForm sizingMethodForm = new SizingMethodForm(
                    parentCode,
                    isNew,
                    DescriptionTextBox.Text,
                    float.Parse(MarkupTextBox.Text, CultureInfo.InvariantCulture),
                    float.Parse(CostPriceExclTextBox.Text, CultureInfo.InvariantCulture),
                    SupplierCodeTextBox.Text,
                    selectedSupplier,
                    selectedProductSize,
                    activeTemplate.Department
                    
                    ))
                {
                    if (sizingMethodForm.ShowDialog() == DialogResult.OK)
                    {
                        // Based on the selected method, show the appropriate sizing window
                        switch (sizingMethodForm.SelectedMethod)
                        {
                            case 1:
                                ShowNumberedSizingWindow(
                                    sizingMethodForm.ParentCode,
                                    sizingMethodForm.IsNewParentCode,
                                    sizingMethodForm.Description,
                                    sizingMethodForm.Markup,
                                    sizingMethodForm.CostPriceExcl,
                                    sizingMethodForm.SupplierCode,
                                    sizingMethodForm.Supplier,
                                    sizingMethodForm.ProductSize,
                                    sizingMethodForm.Department);
                                break;
                            case 2:
                                ShowDescriptiveSizingWindow(
                                    sizingMethodForm.ParentCode,
                                    sizingMethodForm.IsNewParentCode,
                                    sizingMethodForm.Description,
                                    sizingMethodForm.Markup,
                                    sizingMethodForm.CostPriceExcl,
                                    sizingMethodForm.SupplierCode,
                                    sizingMethodForm.Supplier,
                                    sizingMethodForm.ProductSize,
                                    sizingMethodForm.Department);
                                break;
                            case 3:
                                ShowBabySizingWindow(
                                    sizingMethodForm.ParentCode,
                                    sizingMethodForm.IsNewParentCode,
                                    sizingMethodForm.Description,
                                    sizingMethodForm.Markup,
                                    sizingMethodForm.CostPriceExcl,
                                    sizingMethodForm.SupplierCode,
                                    sizingMethodForm.Supplier,
                                    sizingMethodForm.ProductSize,
                                     sizingMethodForm.Department);
                                break;
                            case 4:
                                ShowDoubleSizingWindow(
                                    sizingMethodForm.ParentCode,
                                    sizingMethodForm.IsNewParentCode,
                                    sizingMethodForm.Description,
                                    sizingMethodForm.Markup,
                                    sizingMethodForm.CostPriceExcl,
                                    sizingMethodForm.SupplierCode,
                                    sizingMethodForm.Supplier,
                                    sizingMethodForm.ProductSize,
                                    sizingMethodForm.Department);
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error showing sizing method: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowNumberedSizingWindow(string parentCode, bool isNewParentCode, string description, float markup, float costPriceExcl, string supplierCode, CreateSizesSupplierItem supplier, CreateSizesProductSizeItem productSize, string Department)
        {
            try
            {
                // Create and show the numbered sizing form
                using (NumberedSizingForm numberedSizingForm = new NumberedSizingForm(
                    parentCode,
                    isNewParentCode,
                    description,
                    markup,
                    costPriceExcl,
                    supplierCode,
                    supplier,
                    productSize,
                    Department,
                    TaxValue,
                    activeTemplate.SizeLink
                    ))
                {
                    if (numberedSizingForm.ShowDialog() == DialogResult.OK)
                    {
                        // Show product confirmation window to create products
                        ShowProductConfirmationWindow(numberedSizingForm.ProductsToCreate, numberedSizingForm.ExistingProductCodes);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error showing numbered sizing window: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowDescriptiveSizingWindow(string parentCode, bool isNewParentCode, string description, float markup, float costPriceExcl, string supplierCode, CreateSizesSupplierItem supplier, CreateSizesProductSizeItem productSize, string Department)
        {
            try
            {
                // Create and show the descriptive sizing form
                using (DescriptiveSizingForm descriptiveSizingForm = new DescriptiveSizingForm(
                    parentCode,
                    isNewParentCode,
                    description,
                    markup,
                    costPriceExcl,
                    supplierCode,
                    supplier,
                    productSize,
                    Department,
                    TaxValue
                    ))
                {
                    if (descriptiveSizingForm.ShowDialog() == DialogResult.OK)
                    {
                        // Show product confirmation window to create products
                        ShowProductConfirmationWindow(descriptiveSizingForm.ProductsToCreate, descriptiveSizingForm.ExistingProductCodes);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error showing descriptive sizing window: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowBabySizingWindow(string parentCode, bool isNewParentCode, string description, float markup, float costPriceExcl, string supplierCode, CreateSizesSupplierItem supplier, CreateSizesProductSizeItem productSize, string Department)
        {
            try
            {
                // Create and show the baby sizing form
                using (BabySizingForm babySizingForm = new BabySizingForm(
                    parentCode,
                    isNewParentCode,
                    description,
                    markup,
                    costPriceExcl,
                    supplierCode,
                    supplier,
                    productSize,
                    Department,
                    TaxValue
                    ))
                {
                    if (babySizingForm.ShowDialog() == DialogResult.OK)
                    {
                        // Show product confirmation window to create products
                        ShowProductConfirmationWindow(babySizingForm.ProductsToCreate, babySizingForm.ExistingProductCodes);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error showing baby sizing window: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowDoubleSizingWindow(string parentCode, bool isNewParentCode, string description, float markup, float costPriceExcl, string supplierCode, CreateSizesSupplierItem supplier, CreateSizesProductSizeItem productSize, string Department)
        {
            try
            {
                // Create and show the double sizing form
                using (DoubleSizingForm doubleSizingForm = new DoubleSizingForm(
                    parentCode,
                    isNewParentCode,
                    description,
                    markup,
                    costPriceExcl,
                    supplierCode,
                    supplier,
                    productSize,
                    Department,
                    TaxValue
                    ))
                {
                    if (doubleSizingForm.ShowDialog() == DialogResult.OK)
                    {
                        // Show product confirmation window to create products
                        ShowProductConfirmationWindow(doubleSizingForm.ProductsToCreate, doubleSizingForm.ExistingProductCodes);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error showing double sizing window: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowProductConfirmationWindow(List<Orbit_ProductInfo> productsToCreate, List<string> existingProductCodes)
        {
            try
            {
                // Create and show the product confirmation form
                using (ProductConfirmationForm confirmationForm = new ProductConfirmationForm(
                    productsToCreate,
                    existingProductCodes))
                {
                    if (confirmationForm.ShowDialog() == DialogResult.OK)
                    {
                        MessageBox.Show(
                            "Products created successfully!",
                            "Success",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error showing product confirmation window: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void DisableForm()
        {
            // Show a wait cursor
            this.Cursor = Cursors.WaitCursor;

            // Disable all controls on the form
            foreach (Control control in this.Controls)
            {
                control.Enabled = false;
            }

            // Alternative approach: use the form's Enabled property
            // this.Enabled = false;

            // Optional: Show a progress indicator
            // progressBar1.Visible = true;

            // Force UI to update
            Application.DoEvents();
        }

        private void EnableForm()
        {
            // Re-enable all controls on the form
            foreach (Control control in this.Controls)
            {
                control.Enabled = true;
            }

            // Alternative approach: use the form's Enabled property
            // this.Enabled = true;

            // Hide any progress indicator
            // progressBar1.Visible = false;

            // Restore normal cursor
            this.Cursor = Cursors.Default;
        }
    }
}