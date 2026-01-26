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
using System.Globalization;
using Manny_Tools_Claude.DTO;

namespace Manny_Tools_Claude
{
    public partial class KingsleyProductsCreateForm : UserControl
    {
        #region Fields

        private string _connectionString;
        private KingsleyProductItem _currentProduct;
        private List<KingsleyProductItem> _sessionProducts;
        private List<CreateSizesSupplierItem> _suppliers;
        private List<CreateSizesProductSizeItem> _productSizes;
        private List<string> _brandNames;
        private List<DepartmentItem> _departments;
        private List<MinorDepartmentItem> _minorDepartments;
        private List<SizeLinkItem> _sizeLinks;
        private CreateSizesSupplierItem _selectedSupplier;
        private CreateSizesProductSizeItem _selectedProductSize;
        private DepartmentItem _selectedDepartment;
        private MinorDepartmentItem _selectedMinorDepartment;
        private SizeLinkItem _selectedSizeLink;
        private BindingSource _sessionProductsBindingSource;
        private bool _isLoadingTemplate = false;

        #endregion

        #region Constructor

        public KingsleyProductsCreateForm()
        {
            InitializeComponent();
            
            // Initialize collections
            _sessionProducts = new List<KingsleyProductItem>();
            _suppliers = new List<CreateSizesSupplierItem>();
            _productSizes = new List<CreateSizesProductSizeItem>();
            _brandNames = new List<string>();
            _departments = new List<DepartmentItem>();
            _minorDepartments = new List<MinorDepartmentItem>();
            _sizeLinks = new List<SizeLinkItem>();
            _sessionProductsBindingSource = new BindingSource();

            // Initialize current product
            _currentProduct = new KingsleyProductItem();
            
            // Get connection string from DatabaseConnectionManager
            _connectionString = DatabaseConnectionManager.Instance.ConnectionString;

            try
            {
                // If connection string is empty, try to load it first
                if (string.IsNullOrEmpty(_connectionString))
                {
                    TryLoadConnectionString();
                }

                InitializeForm();
                LoadSuppliers();
                LoadProductSizes();
                LoadBrandNames();
                LoadDepartments();
            }
            catch
            {
                // Silently handle errors without showing popup
                System.Diagnostics.Debug.WriteLine("Some elements could not be loaded in KingsleyProductsCreateForm");
            }

            // Subscribe to connection changed event
            DatabaseConnectionManager.Instance.ConnectionChanged += OnConnectionChanged;
        }

        #endregion

        #region Initialization

        private void InitializeForm()
        {
            // Setup DataGridView for session products
            ConfigureSessionProductsGrid();
            _sessionProductsBindingSource.DataSource = _sessionProducts;
            dgvSessionProducts.DataSource = _sessionProductsBindingSource;

            // Bind current product to form controls
            BindCurrentProductToControls();
            
            // Set up event handlers for validation
            SetupValidationEventHandlers();
            
            // Initially disable review button
            btnReviewProducts.Enabled = false;
        }

        private void ConfigureSessionProductsGrid()
        {
            dgvSessionProducts.AutoGenerateColumns = false;
            dgvSessionProducts.Columns.Clear();

            // ProductCode column
            dgvSessionProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ProductCode",
                HeaderText = "Product Code",
                Name = "ProductCode",
                FillWeight = 15
            });

            // Description column
            dgvSessionProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Description",
                HeaderText = "Description",
                Name = "Description",
                FillWeight = 35
            });

            // UnitOfMeasure column
            dgvSessionProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "UnitOfMeasure",
                HeaderText = "Unit of Measure",
                Name = "UnitOfMeasure",
                FillWeight = 20
            });

            // CostPriceExcl column - no formatting, display exact values
            dgvSessionProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "CostPriceExcl",
                HeaderText = "Cost Price",
                Name = "CostPriceExcl",
                FillWeight = 15
            });

            // SellingPriceIncl column - no formatting, display exact values
            dgvSessionProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "SellingPriceIncl",
                HeaderText = "Selling Price",
                Name = "SellingPriceIncl",
                FillWeight = 15
            });
        }

        private void BindCurrentProductToControls()
        {
            // Bind text fields
            txtProductCode.DataBindings.Clear();
            txtProductCode.DataBindings.Add("Text", _currentProduct, "ProductCode", false, DataSourceUpdateMode.OnPropertyChanged);
            
            txtDescription.DataBindings.Clear();
            txtDescription.DataBindings.Add("Text", _currentProduct, "Description", false, DataSourceUpdateMode.OnPropertyChanged);
            
            cmbBrandName.DataBindings.Clear();
            cmbBrandName.DataBindings.Add("Text", _currentProduct, "BrandName", false, DataSourceUpdateMode.OnPropertyChanged);
            
            txtUnitSize.DataBindings.Clear();
            txtUnitSize.DataBindings.Add("Text", _currentProduct, "UnitSize", false, DataSourceUpdateMode.OnPropertyChanged);
            
            txtUnitOfMeasure.DataBindings.Clear();
            txtUnitOfMeasure.DataBindings.Add("Text", _currentProduct, "UnitOfMeasure", false, DataSourceUpdateMode.OnPropertyChanged);

            txtTaxValue.DataBindings.Clear();
            var taxValueBinding = new Binding("Text", _currentProduct, "TaxValue", true, DataSourceUpdateMode.OnPropertyChanged);
            taxValueBinding.Parse += (s, e) => e.Value = ParseDecimalWithCommaOrDot(e.Value.ToString());
            txtTaxValue.DataBindings.Add(taxValueBinding);

            // Department binding removed - will be handled by SelectedIndexChanged event only

            // Bind numeric fields without formatting - display exact values
            txtSellingPriceIncl.DataBindings.Clear();
            var sellingPriceBinding = new Binding("Text", _currentProduct, "SellingPriceIncl", true, DataSourceUpdateMode.OnPropertyChanged);
            sellingPriceBinding.Parse += (s, e) => e.Value = ParseDecimalWithCommaOrDot(e.Value.ToString());
            txtSellingPriceIncl.DataBindings.Add(sellingPriceBinding);
            
            txtCostPriceIncl.DataBindings.Clear();
            var costPriceInclBinding = new Binding("Text", _currentProduct, "CostPriceIncl", true, DataSourceUpdateMode.OnPropertyChanged);
            costPriceInclBinding.Parse += (s, e) => e.Value = ParseDecimalWithCommaOrDot(e.Value.ToString());
            txtCostPriceIncl.DataBindings.Add(costPriceInclBinding);
            
            txtCostPriceExcl.DataBindings.Clear();
            var costPriceExclBinding = new Binding("Text", _currentProduct, "CostPriceExcl", true, DataSourceUpdateMode.OnPropertyChanged);
            costPriceExclBinding.Parse += (s, e) => e.Value = ParseDecimalWithCommaOrDot(e.Value.ToString());
            txtCostPriceExcl.DataBindings.Add(costPriceExclBinding);
            
            txtSellingPriceExcl.DataBindings.Clear();
            var sellingPriceExclBinding = new Binding("Text", _currentProduct, "SellingPriceExcl", true, DataSourceUpdateMode.OnPropertyChanged);
            sellingPriceExclBinding.Parse += (s, e) => e.Value = ParseDecimalWithCommaOrDot(e.Value.ToString());
            txtSellingPriceExcl.DataBindings.Add(sellingPriceExclBinding);
            
            txtMarkup.DataBindings.Clear();
            var markupBinding = new Binding("Text", _currentProduct, "Markup", true, DataSourceUpdateMode.OnPropertyChanged);
            markupBinding.Parse += (s, e) => e.Value = ParseDecimalWithCommaOrDot(e.Value.ToString());
            txtMarkup.DataBindings.Add(markupBinding);
            
            // Additional price fields
            txtSellingPrice2.DataBindings.Clear();
            var selling2Binding = new Binding("Text", _currentProduct, "SellingPrice2", true, DataSourceUpdateMode.OnPropertyChanged);
            selling2Binding.Parse += (s, e) => e.Value = ParseDecimalWithCommaOrDot(e.Value.ToString());
            txtSellingPrice2.DataBindings.Add(selling2Binding);
            
            txtSellingPrice3.DataBindings.Clear();
            var selling3Binding = new Binding("Text", _currentProduct, "SellingPrice3", true, DataSourceUpdateMode.OnPropertyChanged);
            selling3Binding.Parse += (s, e) => e.Value = ParseDecimalWithCommaOrDot(e.Value.ToString());
            txtSellingPrice3.DataBindings.Add(selling3Binding);
            
            txtSellingPrice4.DataBindings.Clear();
            var selling4Binding = new Binding("Text", _currentProduct, "SellingPrice4", true, DataSourceUpdateMode.OnPropertyChanged);
            selling4Binding.Parse += (s, e) => e.Value = ParseDecimalWithCommaOrDot(e.Value.ToString());
            txtSellingPrice4.DataBindings.Add(selling4Binding);
            
            txtSellingPrice5.DataBindings.Clear();
            var selling5Binding = new Binding("Text", _currentProduct, "SellingPrice5", true, DataSourceUpdateMode.OnPropertyChanged);
            selling5Binding.Parse += (s, e) => e.Value = ParseDecimalWithCommaOrDot(e.Value.ToString());
            txtSellingPrice5.DataBindings.Add(selling5Binding);
            
            // Other fields
            txtPackSize.DataBindings.Clear();
            txtPackSize.DataBindings.Add("Text", _currentProduct, "PackSize", false, DataSourceUpdateMode.OnPropertyChanged);
            
            // SizeLink dropdown - handled manually in SetDropdownsFromTemplate, no data binding needed
            
            
            // MinorDepartment binding removed - will be handled by SelectedIndexChanged event only
            
            chkDiscontinued.DataBindings.Clear();
            var discontinuedBinding = new Binding("Checked", _currentProduct, "Discontinued", true, DataSourceUpdateMode.OnPropertyChanged);
            discontinuedBinding.Format += (s, e) => e.Value = e.Value?.ToString() == "1";
            discontinuedBinding.Parse += (s, e) => e.Value = (bool)e.Value ? "1" : "0";
            chkDiscontinued.DataBindings.Add(discontinuedBinding);
            chkDiscontinued.CheckedChanged += (s, e) => _currentProduct.Discontinued = chkDiscontinued.Checked ? "1" : "0";
            
            chkStockItem.DataBindings.Clear();
            var stockItemBinding = new Binding("Checked", _currentProduct, "StockItem", true, DataSourceUpdateMode.OnPropertyChanged);
            stockItemBinding.Format += (s, e) => e.Value = e.Value?.ToString() == "1";
            stockItemBinding.Parse += (s, e) => e.Value = (bool)e.Value ? "1" : "0";
            chkStockItem.DataBindings.Add(stockItemBinding);
            chkStockItem.CheckedChanged += (s, e) => _currentProduct.StockItem = chkStockItem.Checked ? "1" : "0";
            
            chkTillItem.DataBindings.Clear();
            var tillItemBinding = new Binding("Checked", _currentProduct, "TillItem", true, DataSourceUpdateMode.OnPropertyChanged);
            tillItemBinding.Format += (s, e) => e.Value = e.Value?.ToString() == "1";
            tillItemBinding.Parse += (s, e) => e.Value = (bool)e.Value ? "1" : "0";
            chkTillItem.DataBindings.Add(tillItemBinding);
            chkTillItem.CheckedChanged += (s, e) => _currentProduct.TillItem = chkTillItem.Checked ? "1" : "0";

            // Bind Supplier Code (read-only)
            txtSupplierCode.DataBindings.Clear();
            txtSupplierCode.DataBindings.Add("Text", _currentProduct, "SupplierCode", false, DataSourceUpdateMode.OnPropertyChanged);

            // Subscribe to property changed events for validation
            _currentProduct.PropertyChanged += CurrentProduct_PropertyChanged;
        }

        private void SetupValidationEventHandlers()
        {
            // Add validation event handlers for all text fields
            txtProductCode.TextChanged += ValidateForm;
            txtDescription.TextChanged += ValidateForm;
            txtCostPriceIncl.TextChanged += ValidateForm;
            txtCostPriceExcl.TextChanged += ValidateForm;
            txtSellingPriceIncl.TextChanged += ValidateForm;
            txtSellingPriceExcl.TextChanged += ValidateForm;
            txtMarkup.TextChanged += ValidateForm;
            txtTaxValue.TextChanged += ValidateForm;

            // Add recalculation event handlers for pricing fields
            txtCostPriceIncl.TextChanged += txtCostPriceIncl_TextChanged;
            txtCostPriceExcl.TextChanged += txtCostPriceExcl_TextChanged;
            txtSellingPriceIncl.TextChanged += txtSellingPriceIncl_TextChanged;
            txtSellingPriceExcl.TextChanged += txtSellingPriceExcl_TextChanged;
            txtMarkup.TextChanged += txtMarkup_TextChanged;

            // Automatic recalculation disabled - prices will be saved exactly as entered
            // txtSellingPriceIncl.Leave += txtSellingPriceIncl_Leave;
            // txtSellingPriceExcl.Leave += txtSellingPriceExcl_Leave;
            // txtMarkup.Leave += txtMarkup_Leave;
            // txtCostPriceExcl.Leave += txtCostPriceExcl_Leave;
            // txtCostPriceIncl.Leave += txtCostPriceIncl_Leave;
        }

        #endregion

        #region Connection Management

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
                    _connectionString = File.ReadAllText(configPath);
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

        public void UpdateConnectionString(string connectionString)
        {
            _connectionString = connectionString;
        }

        #endregion

        #region Data Loading

        private void LoadSuppliers()
        {
            cmbSupplier.Items.Clear();

            if (string.IsNullOrEmpty(_connectionString))
            {
                if (!TryLoadConnectionString())
                {
                    System.Diagnostics.Debug.WriteLine("Database connection not configured for loading suppliers");
                    return;
                }
            }

            try
            {
                string queryForSuppliers = @"
                    SELECT DISTINCT s.AccountNumber, s.AccountName 
                    FROM Supplier s
                    INNER JOIN SupplierProductLinks spl ON s.AccountNumber = CAST(spl.CreditorNumber AS VARCHAR(50))
                    ORDER BY s.AccountNumber";
                    
                _suppliers = SQL_Get_Generic_List.ExecuteQuery<CreateSizesSupplierItem>(_connectionString, queryForSuppliers, null);

                if (_suppliers != null && _suppliers.Count > 0)
                {
                    foreach (CreateSizesSupplierItem supplier in _suppliers)
                    {
                        cmbSupplier.Items.Add(supplier.ToString());
                    }
                    _selectedSupplier = _suppliers[0];
                    cmbSupplier.SelectedIndex = 0;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("No suppliers found in database");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading suppliers: {ex.Message}");
            }
        }

        private void LoadProductSizes()
        {
            cmbProductSize.Items.Clear();

            if (string.IsNullOrEmpty(_connectionString))
            {
                if (!TryLoadConnectionString())
                {
                    System.Diagnostics.Debug.WriteLine("Database connection not configured for loading product sizes");
                    return;
                }
            }

            try
            {
                string queryForProductSize = "select OID, Number, Description from ProductSize ORDER BY Number";
                _productSizes = SQL_Get_Generic_List.ExecuteQuery<CreateSizesProductSizeItem>(_connectionString, queryForProductSize, null);

                if (_productSizes != null && _productSizes.Count > 0)
                {
                    foreach (CreateSizesProductSizeItem item in _productSizes)
                    {
                        cmbProductSize.Items.Add(item.ToString());
                    }
                    _selectedProductSize = _productSizes[0];
                    cmbProductSize.SelectedIndex = 0;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("No product sizes found in database");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading product sizes: {ex.Message}");
            }
        }

        private void LoadBrandNames()
        {
            cmbBrandName.Items.Clear();

            if (string.IsNullOrEmpty(_connectionString))
            {
                if (!TryLoadConnectionString())
                {
                    System.Diagnostics.Debug.WriteLine("Database connection not configured for loading brand names");
                    return;
                }
            }

            try
            {
                string queryForBrandNames = @"
                    SELECT DISTINCT BrandName 
                    FROM ProductInfo 
                    WHERE BrandName IS NOT NULL AND BrandName != '' 
                    ORDER BY BrandName";
                    
                _brandNames = SQL_Get_Generic_List.ExecuteQuery<string>(_connectionString, queryForBrandNames, null);

                if (_brandNames != null && _brandNames.Count > 0)
                {
                    foreach (string brandName in _brandNames)
                    {
                        cmbBrandName.Items.Add(brandName);
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("No brand names found in database");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading brand names: {ex.Message}");
            }
        }

        private void LoadDepartments()
        {
            cmbDepartment.Items.Clear();

            if (string.IsNullOrEmpty(_connectionString))
            {
                if (!TryLoadConnectionString())
                {
                    System.Diagnostics.Debug.WriteLine("Database connection not configured for loading departments");
                    return;
                }
            }

            try
            {
                string queryForDepartments = @"
                    SELECT DepartmentNumber, DepartmentName 
                    FROM Department 
                    ORDER BY DepartmentNumber";
                    
                _departments = SQL_Get_Generic_List.ExecuteQuery<DepartmentItem>(_connectionString, queryForDepartments, null);

                if (_departments != null && _departments.Count > 0)
                {
                    foreach (DepartmentItem department in _departments)
                    {
                        cmbDepartment.Items.Add(department.ToString());
                    }
                    _selectedDepartment = _departments[0];
                    cmbDepartment.SelectedIndex = 0;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("No departments found in database");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading departments: {ex.Message}");
            }
        }

        private void LoadSizeLinksForProductSize(int productSizeNumber)
        {
            cmbSizeLink.Items.Clear();
            _sizeLinks.Clear();

            if (string.IsNullOrEmpty(_connectionString))
            {
                if (!TryLoadConnectionString())
                {
                    System.Diagnostics.Debug.WriteLine("Database connection not configured for loading size links");
                    return;
                }
            }

            try
            {
                string queryForSizeLinks = @"
                    SELECT OID, Description, Holder
                    FROM SizeLink
                    WHERE Holder = @ProductSizeNumber
                    ORDER BY OID";

                var parameters = new { ProductSizeNumber = productSizeNumber };
                _sizeLinks = SQL_Get_Generic_List.ExecuteQuery<SizeLinkItem>(_connectionString, queryForSizeLinks, parameters);

                if (_sizeLinks != null && _sizeLinks.Count > 0)
                {
                    System.Diagnostics.Debug.WriteLine($"Loaded {_sizeLinks.Count} SizeLinks for ProductSize Number {productSizeNumber}:");
                    foreach (SizeLinkItem sizeLink in _sizeLinks)
                    {
                        System.Diagnostics.Debug.WriteLine($"  - OID={sizeLink.OID}, Description={sizeLink.Description}, Holder={sizeLink.Holder}");
                        cmbSizeLink.Items.Add(sizeLink.ToString());
                    }
                    // Don't auto-select index 0 - let the caller set the correct selection
                    // _selectedSizeLink = _sizeLinks[0];
                    // cmbSizeLink.SelectedIndex = 0;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"No size links found for ProductSize Number: {productSizeNumber}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading size links: {ex.Message}");
            }
        }

        private void LoadMinorDepartmentsForDepartment(string departmentNumber)
        {
            cmbMinorDepartment.Items.Clear();
            _minorDepartments.Clear();

            if (string.IsNullOrEmpty(_connectionString))
            {
                if (!TryLoadConnectionString())
                {
                    System.Diagnostics.Debug.WriteLine("Database connection not configured for loading minor departments");
                    return;
                }
            }

            try
            {
                string queryForMinorDepartments = @"
                    SELECT OID, ParentDepartment, MinorDepartmentName, MinorDepartmentNumber 
                    FROM MinorDepartment 
                    WHERE ParentDepartment = @DepartmentNumber 
                    ORDER BY MinorDepartmentNumber";
                    
                var parameters = new { DepartmentNumber = departmentNumber };
                _minorDepartments = SQL_Get_Generic_List.ExecuteQuery<MinorDepartmentItem>(_connectionString, queryForMinorDepartments, parameters);

                if (_minorDepartments != null && _minorDepartments.Count > 0)
                {
                    foreach (MinorDepartmentItem minorDepartment in _minorDepartments)
                    {
                        cmbMinorDepartment.Items.Add(minorDepartment.ToString());
                    }
                    _selectedMinorDepartment = _minorDepartments[0];
                    cmbMinorDepartment.SelectedIndex = 0;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"No minor departments found for Department: {departmentNumber}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading minor departments: {ex.Message}");
            }
        }

        #endregion

        #region Event Handlers

        private void CurrentProduct_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Update validation when any property changes
            ValidateForm(sender, EventArgs.Empty);
        }

        private void cmbSupplier_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSupplier.SelectedIndex >= 0 && cmbSupplier.SelectedIndex < _suppliers.Count)
            {
                _selectedSupplier = _suppliers[cmbSupplier.SelectedIndex];
                _currentProduct.SupplierNumber = _selectedSupplier.AccountNumber;
            }
        }

        private void cmbProductSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbProductSize.SelectedIndex >= 0 && cmbProductSize.SelectedIndex < _productSizes.Count)
            {
                _selectedProductSize = _productSizes[cmbProductSize.SelectedIndex];
                _currentProduct.ProductSize = _selectedProductSize.Number.ToString();

                // Load size links for the selected product size (using Number, not OID)
                LoadSizeLinksForProductSize(_selectedProductSize.Number);

                // When user manually changes ProductSize (not during template load), select the first SizeLink by default
                if (!_isLoadingTemplate && _sizeLinks != null && _sizeLinks.Count > 0)
                {
                    cmbSizeLink.SelectedIndex = 0;
                    _selectedSizeLink = _sizeLinks[0];
                    _currentProduct.SizeLink = _selectedSizeLink.OID.ToString();

                    // Populate UnitOfMeasure from SizeLink Description
                    _currentProduct.UnitOfMeasure = _selectedSizeLink.Description ?? string.Empty;
                }
            }
        }

        private void cmbBrandName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbBrandName.SelectedIndex >= 0)
            {
                _currentProduct.BrandName = cmbBrandName.Text;
            }
        }

        private void cmbDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbDepartment.SelectedIndex >= 0 && cmbDepartment.SelectedIndex < _departments.Count)
            {
                _selectedDepartment = _departments[cmbDepartment.SelectedIndex];
                _currentProduct.Department = _selectedDepartment.DepartmentNumber;
                
                // Load minor departments for the selected department
                LoadMinorDepartmentsForDepartment(_selectedDepartment.DepartmentNumber);
            }
        }

        private void cmbMinorDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbMinorDepartment.SelectedIndex >= 0 && cmbMinorDepartment.SelectedIndex < _minorDepartments.Count)
            {
                _selectedMinorDepartment = _minorDepartments[cmbMinorDepartment.SelectedIndex];
                _currentProduct.MinorDepartment = _selectedMinorDepartment.MinorDepartmentNumber;
            }
        }

        private void cmbSizeLink_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSizeLink.SelectedIndex >= 0 && cmbSizeLink.SelectedIndex < _sizeLinks.Count)
            {
                _selectedSizeLink = _sizeLinks[cmbSizeLink.SelectedIndex];
                _currentProduct.SizeLink = _selectedSizeLink.OID.ToString();

                // Populate UnitOfMeasure from SizeLink Description
                _currentProduct.UnitOfMeasure = _selectedSizeLink.Description ?? string.Empty;
                System.Diagnostics.Debug.WriteLine($"SizeLink changed: OID={_selectedSizeLink.OID}, Description={_selectedSizeLink.Description}, UnitOfMeasure set to '{_currentProduct.UnitOfMeasure}'");
            }
        }


        private void ValidateForm(object sender, EventArgs e)
        {
            bool isValid = true;

            // Reset all field colors
            ResetFieldColors();

            // ========== REQUIRED FIELD VALIDATION (Hard Errors) ==========

            // Product Code - Required
            if (string.IsNullOrWhiteSpace(_currentProduct.ProductCode))
            {
                SetFieldError(txtProductCode, "Product Code is required");
                isValid = false;
            }

            // Description - Required
            if (string.IsNullOrWhiteSpace(_currentProduct.Description))
            {
                SetFieldError(txtDescription, "Description is required");
                isValid = false;
            }

            // Department - Required
            if (cmbDepartment.SelectedIndex < 0 || string.IsNullOrWhiteSpace(_currentProduct.Department))
            {
                SetFieldError(cmbDepartment, "Department is required");
                isValid = false;
            }

            // Minor Department - Required
            if (cmbMinorDepartment.SelectedIndex < 0 || string.IsNullOrWhiteSpace(_currentProduct.MinorDepartment))
            {
                SetFieldError(cmbMinorDepartment, "Minor Department is required");
                isValid = false;
            }

            // Product Size - Required
            if (cmbProductSize.SelectedIndex < 0 || string.IsNullOrWhiteSpace(_currentProduct.ProductSize))
            {
                SetFieldError(cmbProductSize, "Product Size is required");
                isValid = false;
            }

            // SizeLink - Required
            if (cmbSizeLink.SelectedIndex < 0 || string.IsNullOrWhiteSpace(_currentProduct.SizeLink))
            {
                SetFieldError(cmbSizeLink, "Size Link is required");
                isValid = false;
            }

            // Supplier - Required
            if (cmbSupplier.SelectedIndex < 0 || string.IsNullOrWhiteSpace(_currentProduct.SupplierNumber))
            {
                SetFieldError(cmbSupplier, "Supplier is required");
                isValid = false;
            }

            // Supplier Code - Required when supplier is selected
            if (cmbSupplier.SelectedIndex >= 0 && string.IsNullOrWhiteSpace(_currentProduct.SupplierCode))
            {
                SetFieldError(txtSupplierCode, "Supplier Code is required");
                isValid = false;
            }

            // Unit of Measure - Required (usually auto-populated from SizeLink)
            if (string.IsNullOrWhiteSpace(_currentProduct.UnitOfMeasure))
            {
                SetFieldError(txtUnitOfMeasure, "Unit of Measure is required");
                isValid = false;
            }

            // Cost Price Excl - Must be greater than 0
            if (_currentProduct.CostPriceExcl <= 0)
            {
                SetFieldError(txtCostPriceExcl, "Cost Price (Excl) must be greater than 0");
                isValid = false;
            }

            // Cost Price Incl - Must be greater than 0
            if (_currentProduct.CostPriceIncl <= 0)
            {
                SetFieldError(txtCostPriceIncl, "Cost Price (Incl) must be greater than 0");
                isValid = false;
            }

            // Selling Price Incl - Must be greater than 0
            if (_currentProduct.SellingPriceIncl <= 0)
            {
                SetFieldError(txtSellingPriceIncl, "Selling Price (Incl) must be greater than 0");
                isValid = false;
            }

            // Selling Price Excl - Must be greater than 0
            if (_currentProduct.SellingPriceExcl <= 0)
            {
                SetFieldError(txtSellingPriceExcl, "Selling Price (Excl) must be greater than 0");
                isValid = false;
            }

            // ========== DATA TYPE VALIDATION (Hard Errors) ==========

            // Pack Size - Must be valid integer >= 1
            if (!string.IsNullOrWhiteSpace(_currentProduct.PackSize))
            {
                if (!int.TryParse(_currentProduct.PackSize, out int packSize) || packSize < 1)
                {
                    SetFieldError(txtPackSize, "Pack Size must be a valid number >= 1");
                    isValid = false;
                }
            }
            else
            {
                // PackSize defaults to 1 if empty
                _currentProduct.PackSize = "1";
            }

            // Unit Size - Defaults to 1 if empty or 0
            if (!string.IsNullOrWhiteSpace(_currentProduct.UnitSize))
            {
                // If user entered a value, check if it's 0 and default to 1
                if (decimal.TryParse(_currentProduct.UnitSize, out decimal unitSize) && unitSize == 0)
                {
                    _currentProduct.UnitSize = "1";
                }
            }
            else
            {
                // UnitSize defaults to 1 if empty
                _currentProduct.UnitSize = "1";
            }

            // Tax Value - Must be between 0 and 100
            if (_currentProduct.TaxValue < 0 || _currentProduct.TaxValue > 100)
            {
                SetFieldError(txtTaxValue, "Tax Value must be between 0 and 100");
                isValid = false;
            }

            // ========== BUSINESS LOGIC VALIDATION (Hard Errors) ==========

            // Selling Price Incl must be greater than Cost Price Incl (profit margin check)
            if (_currentProduct.SellingPriceIncl > 0 && _currentProduct.CostPriceIncl > 0)
            {
                if (_currentProduct.SellingPriceIncl <= _currentProduct.CostPriceIncl)
                {
                    SetFieldError(txtSellingPriceIncl, "Selling Price must be greater than Cost Price");
                    isValid = false;
                }
            }

            // Selling Price Excl must be greater than Cost Price Excl (profit margin check)
            if (_currentProduct.SellingPriceExcl > 0 && _currentProduct.CostPriceExcl > 0)
            {
                if (_currentProduct.SellingPriceExcl <= _currentProduct.CostPriceExcl)
                {
                    SetFieldError(txtSellingPriceExcl, "Selling Price (Excl) must be greater than Cost Price (Excl)");
                    isValid = false;
                }
            }

            // Additional selling prices - Must be >= 0 if entered
            if (_currentProduct.SellingPrice2 < 0)
            {
                SetFieldError(txtSellingPrice2, "Selling Price 2 cannot be negative");
                isValid = false;
            }

            if (_currentProduct.SellingPrice3 < 0)
            {
                SetFieldError(txtSellingPrice3, "Selling Price 3 cannot be negative");
                isValid = false;
            }

            if (_currentProduct.SellingPrice4 < 0)
            {
                SetFieldError(txtSellingPrice4, "Selling Price 4 cannot be negative");
                isValid = false;
            }

            if (_currentProduct.SellingPrice5 < 0)
            {
                SetFieldError(txtSellingPrice5, "Selling Price 5 cannot be negative");
                isValid = false;
            }

            // ========== WARNING VALIDATION (Soft Warnings - Yellow) ==========

            // Markup - Warn if unusually high
            if (_currentProduct.Markup > 1000)
            {
                SetFieldWarning(txtMarkup, "Markup seems unusually high (>1000%)");
            }

            // Markup - Warn if negative (selling below cost)
            if (_currentProduct.Markup < 0)
            {
                SetFieldWarning(txtMarkup, "Negative markup - selling below cost");
            }

            // Cost Price - Warn if unusually high
            if (_currentProduct.CostPriceExcl > 10000)
            {
                SetFieldWarning(txtCostPriceExcl, "Cost price seems unusually high (>10000)");
            }

            // Selling Price - Warn if unusually high
            if (_currentProduct.SellingPriceIncl > 15000)
            {
                SetFieldWarning(txtSellingPriceIncl, "Selling price seems unusually high (>15000)");
            }

            // ========== UPDATE BUTTON STATES ==========

            // Enable/disable Add Product button based on validation
            btnAddProduct.Enabled = isValid;

            // Enable Review Products button if there are products in session
            btnReviewProducts.Enabled = _sessionProducts.Count > 0;
        }

        #region Pricing Recalculation Event Handlers

        private void txtSellingPriceIncl_Leave(object sender, EventArgs e)
        {
            // Recalculate markup based on new selling price inclusive
            _currentProduct.RecalculateMarkupFromPrices();
        }

        private void txtSellingPriceExcl_Leave(object sender, EventArgs e)
        {
            // Recalculate markup based on new selling price exclusive
            _currentProduct.RecalculateMarkupFromPrices();
        }

        private void txtMarkup_Leave(object sender, EventArgs e)
        {
            // Recalculate selling prices based on new markup
            _currentProduct.RecalculateSellingPricesFromMarkup();
        }

        private void txtCostPriceExcl_Leave(object sender, EventArgs e)
        {
            // Recalculate selling prices based on new cost price exclusive
            _currentProduct.RecalculateSellingPricesFromMarkup();
        }

        private void txtCostPriceIncl_Leave(object sender, EventArgs e)
        {
            // The DTO will handle recalculating CostPriceExcl and then selling prices
            // This is handled automatically in the CostPriceIncl setter
        }

        private void txtCostPriceIncl_TextChanged(object sender, EventArgs e)
        {
            // When user edits Cost Price Incl, recalculate Cost Price Excl and Selling Prices
            if (_currentProduct != null && !string.IsNullOrWhiteSpace(txtCostPriceIncl.Text))
            {
                // Parse the value using the same method as data binding
                decimal value;
                if (decimal.TryParse(txtCostPriceIncl.Text.Replace(',', '.'), out value))
                {
                    // Only recalculate if the value actually changed
                    if (_currentProduct.CostPriceIncl != value)
                    {
                        // Update the property (this won't trigger automatic recalculation)
                        _currentProduct.CostPriceIncl = value;
                        // Manually trigger recalculation of CostPriceExcl and Selling Prices
                        _currentProduct.CalculateCostPriceExclFromIncl();
                    }
                }
            }
        }

        private void txtCostPriceExcl_TextChanged(object sender, EventArgs e)
        {
            // When user edits Cost Price Excl, recalculate Cost Price Incl and Selling Prices
            if (_currentProduct != null && !string.IsNullOrWhiteSpace(txtCostPriceExcl.Text))
            {
                // Parse the value using the same method as data binding
                decimal value;
                if (decimal.TryParse(txtCostPriceExcl.Text.Replace(',', '.'), out value))
                {
                    // Only recalculate if the value actually changed
                    if (_currentProduct.CostPriceExcl != value)
                    {
                        // Update the property (this won't trigger automatic recalculation)
                        _currentProduct.CostPriceExcl = value;
                        // Manually trigger recalculation of CostPriceIncl
                        _currentProduct.CalculateCostPriceIncl();
                        // Also recalculate Selling Prices based on new cost
                        _currentProduct.RecalculateSellingPricesFromMarkup();
                    }
                }
            }
        }

        private void txtSellingPriceIncl_TextChanged(object sender, EventArgs e)
        {
            // When user edits Selling Price Incl, recalculate Selling Price Excl and Markup
            if (_currentProduct != null && !string.IsNullOrWhiteSpace(txtSellingPriceIncl.Text))
            {
                // Parse the value using the same method as data binding
                decimal value;
                if (decimal.TryParse(txtSellingPriceIncl.Text.Replace(',', '.'), out value))
                {
                    // Only recalculate if the value actually changed
                    if (_currentProduct.SellingPriceIncl != value)
                    {
                        // Update the property (this won't trigger automatic recalculation)
                        _currentProduct.SellingPriceIncl = value;
                        // Manually trigger recalculation of SellingPriceExcl
                        _currentProduct.CalculateSellingPriceExcl();
                        // Recalculate Markup based on new selling price
                        _currentProduct.RecalculateMarkupFromPrices();
                    }
                }
            }
        }

        private void txtSellingPriceExcl_TextChanged(object sender, EventArgs e)
        {
            // When user edits Selling Price Excl, recalculate Selling Price Incl and Markup
            if (_currentProduct != null && !string.IsNullOrWhiteSpace(txtSellingPriceExcl.Text))
            {
                // Parse the value using the same method as data binding
                decimal value;
                if (decimal.TryParse(txtSellingPriceExcl.Text.Replace(',', '.'), out value))
                {
                    // Only recalculate if the value actually changed
                    if (_currentProduct.SellingPriceExcl != value)
                    {
                        // Update the property (this won't trigger automatic recalculation)
                        _currentProduct.SellingPriceExcl = value;
                        // Manually trigger recalculation of SellingPriceIncl
                        _currentProduct.CalculateSellingPriceInclFromExcl();
                        // Recalculate Markup based on new selling price
                        _currentProduct.RecalculateMarkupFromPrices();
                    }
                }
            }
        }

        private void txtMarkup_TextChanged(object sender, EventArgs e)
        {
            // When user edits Markup, recalculate Selling Price Excl and Selling Price Incl
            if (_currentProduct != null && !string.IsNullOrWhiteSpace(txtMarkup.Text))
            {
                // Parse the value using the same method as data binding
                decimal value;
                if (decimal.TryParse(txtMarkup.Text.Replace(',', '.'), out value))
                {
                    // Only recalculate if the value actually changed
                    if (_currentProduct.Markup != value)
                    {
                        // Update the property - this will automatically trigger RecalculateSellingPricesFromMarkup
                        // because the Markup property setter has automatic recalculation enabled
                        _currentProduct.Markup = value;
                    }
                }
            }
        }

        #endregion

        #endregion

        #region Validation UI

        private void ResetFieldColors()
        {
            // Reset TextBox controls
            var textBoxes = new[] {
                txtProductCode, txtDescription, txtCostPriceExcl, txtCostPriceIncl,
                txtSellingPriceIncl, txtSellingPriceExcl, txtMarkup, txtUnitOfMeasure,
                txtPackSize, txtUnitSize, txtSupplierCode, txtTaxValue,
                txtSellingPrice2, txtSellingPrice3, txtSellingPrice4, txtSellingPrice5
            };
            foreach (var textBox in textBoxes)
            {
                textBox.BackColor = SystemColors.Window;
                textBox.ForeColor = SystemColors.WindowText;
            }

            // Reset ComboBox controls
            var comboBoxes = new[] {
                cmbDepartment, cmbMinorDepartment, cmbProductSize,
                cmbSizeLink, cmbSupplier, cmbBrandName
            };
            foreach (var comboBox in comboBoxes)
            {
                comboBox.BackColor = SystemColors.Window;
                comboBox.ForeColor = SystemColors.WindowText;
            }
        }

        private void SetFieldError(TextBox textBox, string message)
        {
            textBox.BackColor = Color.LightPink;
            textBox.ForeColor = Color.DarkRed;
            // You could also show tooltip or status message here
        }

        private void SetFieldError(ComboBox comboBox, string message)
        {
            comboBox.BackColor = Color.LightPink;
            comboBox.ForeColor = Color.DarkRed;
        }

        private void SetFieldWarning(TextBox textBox, string message)
        {
            textBox.BackColor = Color.LightYellow;
            textBox.ForeColor = Color.DarkGoldenrod;
        }

        private void SetFieldWarning(ComboBox comboBox, string message)
        {
            comboBox.BackColor = Color.LightYellow;
            comboBox.ForeColor = Color.DarkGoldenrod;
        }

        #endregion

        #region Button Event Handlers

        private void btnLoadTemplate_Click(object sender, EventArgs e)
        {
            LoadTemplate();
        }

        private void btnNextAvailableCode_Click(object sender, EventArgs e)
        {
            GenerateNextAvailableProductCode();
        }

        private void btnClearForm_Click(object sender, EventArgs e)
        {
            ClearCurrentProduct();
        }

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            AddCurrentProductToSession();
        }

        private void btnReviewProducts_Click(object sender, EventArgs e)
        {
            ShowProductReview();
        }

        private void btnRemoveProduct_Click(object sender, EventArgs e)
        {
            if (dgvSessionProducts.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a product to remove.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var selectedRow = dgvSessionProducts.SelectedRows[0];
            var productToRemove = selectedRow.DataBoundItem as KingsleyProductItem;

            if (productToRemove != null)
            {
                var result = MessageBox.Show(
                    $"Are you sure you want to remove product {productToRemove.ProductCode} from the session?",
                    "Confirm Removal",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    _sessionProducts.Remove(productToRemove);
                    _sessionProductsBindingSource.ResetBindings(false);

                    MessageBox.Show($"Product {productToRemove.ProductCode} removed from session.",
                        "Product Removed", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Update review button state
                    btnReviewProducts.Enabled = _sessionProducts.Count > 0;
                }
            }
        }

        #endregion

        #region Business Logic

        private void LoadTemplate()
        {
            string templateCode = txtTemplateCode.Text.Trim();
            if (string.IsNullOrEmpty(templateCode))
            {
                MessageBox.Show("Please enter a Product Code to load as template.", "Template Required",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(_connectionString))
            {
                if (!TryLoadConnectionString())
                {
                    MessageBox.Show("Database connection not configured. Please set up the connection in settings.",
                        "Connection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            _isLoadingTemplate = true; // Prevent dropdown event handlers from interfering
            try
            {
                string query = @"SELECT ProductCode, ParentCode, IsPack, PackSize, Description, BrandName, UnitSize, UnitOfMeasure,
                                Department, SellingPriceIncl, SellingPriceExcl, CostPriceExcl, CostPriceIncl, Markup,
                                SellingPrice2, SellingPrice3, SellingPrice4, SellingPrice5, TaxValue, StockItem, TillItem,
                                SizeLink, ProductSize, ClothingItem, MinorDepartment, Discontinued
                                FROM ProductInfo WHERE ProductCode = @ProductCode";
                var parameters = new { ProductCode = templateCode };

                var templateProduct = SQL_Get_Generic_List.ExecuteSingle<Orbit_ProductInfo>(_connectionString, query, parameters);

                if (templateProduct != null)
                {
                    // DEBUG: Log raw values from SQL query BEFORE LoadFromTemplate
                    System.Diagnostics.Debug.WriteLine($"=== RAW SQL QUERY RESULTS ===");
                    System.Diagnostics.Debug.WriteLine($"StockItem from SQL: '{templateProduct.StockItem}' (type: {templateProduct.StockItem?.GetType().Name ?? "null"})");
                    System.Diagnostics.Debug.WriteLine($"TillItem from SQL: '{templateProduct.TillItem}' (type: {templateProduct.TillItem?.GetType().Name ?? "null"})");
                    System.Diagnostics.Debug.WriteLine($"Discontinued from SQL: '{templateProduct.Discontinued}' (type: {templateProduct.Discontinued?.GetType().Name ?? "null"})");
                    System.Diagnostics.Debug.WriteLine($"SizeLink from SQL: '{templateProduct.SizeLink}'");
                    System.Diagnostics.Debug.WriteLine($"ProductSize from SQL: '{templateProduct.ProductSize}'");

                    _currentProduct.LoadFromTemplate(templateProduct);

                    // Explicitly set checkbox states from loaded values
                    chkStockItem.Checked = (_currentProduct.StockItem == "1");
                    chkTillItem.Checked = (_currentProduct.TillItem == "1");
                    chkDiscontinued.Checked = (_currentProduct.Discontinued == "1");

                    System.Diagnostics.Debug.WriteLine($"=== AFTER LoadFromTemplate ===");
                    System.Diagnostics.Debug.WriteLine($"StockItem in _currentProduct: '{_currentProduct.StockItem}' (checked={chkStockItem.Checked})");
                    System.Diagnostics.Debug.WriteLine($"TillItem in _currentProduct: '{_currentProduct.TillItem}' (checked={chkTillItem.Checked})");
                    System.Diagnostics.Debug.WriteLine($"Discontinued in _currentProduct: '{_currentProduct.Discontinued}' (checked={chkDiscontinued.Checked})");

                    System.Diagnostics.Debug.WriteLine($"");
                    System.Diagnostics.Debug.WriteLine($"=== ABOUT TO CALL LoadSupplierForTemplate ===");
                    System.Diagnostics.Debug.WriteLine($"Current _currentProduct.SupplierCode BEFORE: '{_currentProduct.SupplierCode}'");
                    System.Diagnostics.Debug.WriteLine($"Current txtSupplierCode.Text BEFORE: '{txtSupplierCode.Text}'");

                    // Also load supplier information if available
                    LoadSupplierForTemplate(templateCode);

                    System.Diagnostics.Debug.WriteLine($"");
                    System.Diagnostics.Debug.WriteLine($"=== AFTER LoadSupplierForTemplate RETURNED ===");
                    System.Diagnostics.Debug.WriteLine($"Current _currentProduct.SupplierCode AFTER: '{_currentProduct.SupplierCode}'");
                    System.Diagnostics.Debug.WriteLine($"Current txtSupplierCode.Text AFTER: '{txtSupplierCode.Text}'");

                    // Set all dropdown fields to match loaded template values
                    SetDropdownsFromTemplate();

                    MessageBox.Show($"Template loaded from Product Code: {templateCode}", "Template Loaded",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show($"No product found with code: {templateCode}", "Template Not Found", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading template: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _isLoadingTemplate = false; // Reset flag
            }
        }

        private void LoadSupplierForTemplate(string productCode)
        {
            System.Diagnostics.Debug.WriteLine($"=== LoadSupplierForTemplate START ===");
            System.Diagnostics.Debug.WriteLine($"ProductCode: '{productCode}'");
            System.Diagnostics.Debug.WriteLine($"Connection String: {(_connectionString != null ? "EXISTS" : "NULL")}");

            try
            {
                string supplierQuery = @"
                    SELECT spl.SupplierProductCode, spl.CreditorNumber, s.AccountName
                    FROM SupplierProductLinks spl
                    INNER JOIN Supplier s ON s.AccountNumber = CAST(spl.CreditorNumber AS VARCHAR(50))
                    WHERE spl.ProductCode = @ProductCode";

                System.Diagnostics.Debug.WriteLine($"Executing supplier query...");
                System.Diagnostics.Debug.WriteLine($"Query: {supplierQuery}");

                var supplierResult = SQL_Get_Generic_List.ExecuteSingle<dynamic>(_connectionString, supplierQuery, new { ProductCode = productCode });

                System.Diagnostics.Debug.WriteLine($"Query executed. Result is {(supplierResult != null ? "NOT NULL" : "NULL")}");

                if (supplierResult != null)
                {
                    System.Diagnostics.Debug.WriteLine($"SupplierProductCode from DB: '{supplierResult.SupplierProductCode}'");
                    System.Diagnostics.Debug.WriteLine($"CreditorNumber from DB: '{supplierResult.CreditorNumber}'");
                    System.Diagnostics.Debug.WriteLine($"AccountName from DB: '{supplierResult.AccountName}'");

                    _currentProduct.SupplierCode = supplierResult.SupplierProductCode?.ToString() ?? string.Empty;
                    _currentProduct.SupplierNumber = supplierResult.CreditorNumber?.ToString() ?? string.Empty;

                    System.Diagnostics.Debug.WriteLine($"Set _currentProduct.SupplierCode to: '{_currentProduct.SupplierCode}'");
                    System.Diagnostics.Debug.WriteLine($"Set _currentProduct.SupplierNumber to: '{_currentProduct.SupplierNumber}'");
                    System.Diagnostics.Debug.WriteLine($"txtSupplierCode.Text is: '{txtSupplierCode.Text}'");

                    // Select the supplier in the combo box
                    System.Diagnostics.Debug.WriteLine($"Searching for supplier in combo box. Total suppliers: {_suppliers.Count}");
                    bool supplierFound = false;
                    for (int i = 0; i < _suppliers.Count; i++)
                    {
                        if (_suppliers[i].AccountNumber == _currentProduct.SupplierNumber)
                        {
                            System.Diagnostics.Debug.WriteLine($"Found matching supplier at index {i}: {_suppliers[i].AccountName}");
                            cmbSupplier.SelectedIndex = i;
                            supplierFound = true;
                            break;
                        }
                    }
                    if (!supplierFound)
                    {
                        System.Diagnostics.Debug.WriteLine($"WARNING: No matching supplier found in combo box for SupplierNumber: '{_currentProduct.SupplierNumber}'");
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"WARNING: No supplier data found for ProductCode: '{productCode}'");
                    System.Diagnostics.Debug.WriteLine($"This could mean:");
                    System.Diagnostics.Debug.WriteLine($"  1. No record exists in SupplierProductLinks for this product");
                    System.Diagnostics.Debug.WriteLine($"  2. The JOIN with Supplier table failed (no matching AccountNumber)");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERROR in LoadSupplierForTemplate: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
            }
            finally
            {
                System.Diagnostics.Debug.WriteLine($"=== LoadSupplierForTemplate END ===");
            }
        }

        private void GenerateNextAvailableProductCode()
        {
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
                // Get all existing product codes and parent codes from database
                string productCodeQuery = "SELECT ProductCode FROM ProductInfo";
                string parentCodeQuery = "SELECT ParentCode FROM ProductInfo";
                
                var existingProductCodes = SQL_Get_Generic_List.ExecuteQuery<string>(_connectionString, productCodeQuery, null);
                var existingParentCodes = SQL_Get_Generic_List.ExecuteQuery<string>(_connectionString, parentCodeQuery, null);
                
                // Combine all codes to check for conflicts
                var allCodes = new HashSet<string>();
                if (existingProductCodes != null) allCodes.UnionWith(existingProductCodes);
                if (existingParentCodes != null) allCodes.UnionWith(existingParentCodes);
                
                // Add product codes from current session to prevent duplicates
                if (_sessionProducts != null && _sessionProducts.Count > 0)
                {
                    foreach (var sessionProduct in _sessionProducts)
                    {
                        if (!string.IsNullOrEmpty(sessionProduct.ProductCode))
                        {
                            allCodes.Add(sessionProduct.ProductCode);
                            // Since ParentCode = ProductCode for this implementation, add it too
                            allCodes.Add(sessionProduct.ProductCode);
                        }
                    }
                }
                
                // Find next available 6-digit code starting from 100000
                int nextCode = 100000;
                while (allCodes.Contains(nextCode.ToString()))
                {
                    nextCode++;
                }
                
                _currentProduct.ProductCode = nextCode.ToString();
                txtProductCode.Text = nextCode.ToString();
                
                MessageBox.Show($"Next available code: {nextCode}", "Code Generated", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating next available code: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearCurrentProduct()
        {
            _currentProduct = new KingsleyProductItem();
            BindCurrentProductToControls();
            txtTemplateCode.Text = string.Empty;
            cmbSupplier.SelectedIndex = _suppliers.Count > 0 ? 0 : -1;
            cmbProductSize.SelectedIndex = _productSizes.Count > 0 ? 0 : -1;
        }

        private void AddCurrentProductToSession()
        {
            // Check if product code already exists in session
            if (_sessionProducts.Any(p => p.ProductCode == _currentProduct.ProductCode))
            {
                MessageBox.Show("A product with this code already exists in the current session.", 
                    "Duplicate Product", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Create a copy of the current product
            var productCopy = new KingsleyProductItem();
            productCopy.LoadFromTemplate(_currentProduct.ToOrbitProductInfo());
            productCopy.ProductCode = _currentProduct.ProductCode; // Restore the product code
            productCopy.SupplierCode = _currentProduct.SupplierCode;
            productCopy.SupplierNumber = _currentProduct.SupplierNumber;

            // Add to session
            _sessionProducts.Add(productCopy);
            _sessionProductsBindingSource.ResetBindings(false);

            MessageBox.Show($"Product {_currentProduct.ProductCode} added to session.", "Product Added", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Clear form for next product
            ClearCurrentProduct();
        }

        private void ShowProductReview()
        {
            if (_sessionProducts.Count == 0)
            {
                MessageBox.Show("No products to review. Please add products to the session first.", 
                    "No Products", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                using (ProductReviewForm reviewForm = new ProductReviewForm(_sessionProducts, _connectionString))
                {
                    if (reviewForm.ShowDialog() == DialogResult.OK)
                    {
                        // Products were created successfully, clear the session
                        _sessionProducts.Clear();
                        _sessionProductsBindingSource.ResetBindings(false);
                        
                        // Clear the current form
                        ClearCurrentProduct();
                        
                        MessageBox.Show("Products created successfully! Session has been cleared.", 
                            "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error showing product review: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Helper Methods

        private void SetDropdownsFromTemplate()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("=== SetDropdownsFromTemplate Started ===");
                
                // Set Department dropdown
                if (!string.IsNullOrEmpty(_currentProduct.Department))
                {
                    System.Diagnostics.Debug.WriteLine($"Looking for Department: {_currentProduct.Department}");
                    var department = _departments.FirstOrDefault(d => d.DepartmentNumber == _currentProduct.Department);
                    if (department != null)
                    {
                        // Find the index in the ComboBox items
                        int departmentIndex = -1;
                        for (int i = 0; i < cmbDepartment.Items.Count; i++)
                        {
                            if (cmbDepartment.Items[i].ToString() == department.ToString())
                            {
                                departmentIndex = i;
                                break;
                            }
                        }
                        
                        if (departmentIndex >= 0)
                        {
                            cmbDepartment.SelectedIndex = departmentIndex;
                            _selectedDepartment = department;
                            System.Diagnostics.Debug.WriteLine($"Department set to index {departmentIndex}: {department}");
                            
                            // Load MinorDepartments for this department
                            LoadMinorDepartmentsForDepartment(department.DepartmentNumber);
                            
                            // Set MinorDepartment dropdown
                            if (!string.IsNullOrEmpty(_currentProduct.MinorDepartment))
                            {
                                System.Diagnostics.Debug.WriteLine($"Looking for MinorDepartment: {_currentProduct.MinorDepartment}");
                                var minorDept = _minorDepartments.FirstOrDefault(md => md.MinorDepartmentNumber == _currentProduct.MinorDepartment);
                                if (minorDept != null)
                                {
                                    // Find the index in the ComboBox items
                                    int minorDeptIndex = -1;
                                    for (int i = 0; i < cmbMinorDepartment.Items.Count; i++)
                                    {
                                        if (cmbMinorDepartment.Items[i].ToString() == minorDept.ToString())
                                        {
                                            minorDeptIndex = i;
                                            break;
                                        }
                                    }
                                    
                                    if (minorDeptIndex >= 0)
                                    {
                                        cmbMinorDepartment.SelectedIndex = minorDeptIndex;
                                        _selectedMinorDepartment = minorDept;
                                        System.Diagnostics.Debug.WriteLine($"MinorDepartment set to index {minorDeptIndex}: {minorDept}");
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"Department not found: {_currentProduct.Department}");
                    }
                }

                // Set ProductSize dropdown
                if (!string.IsNullOrEmpty(_currentProduct.ProductSize))
                {
                    System.Diagnostics.Debug.WriteLine($"Looking for ProductSize: {_currentProduct.ProductSize}");
                    // Try parsing the ProductSize as Number first, then as OID if that fails
                    CreateSizesProductSizeItem productSize = null;
                    
                    // First attempt: match by Number field (preferred)
                    if (int.TryParse(_currentProduct.ProductSize, out int productSizeNumber))
                    {
                        productSize = _productSizes.FirstOrDefault(ps => ps.Number == productSizeNumber);
                        System.Diagnostics.Debug.WriteLine($"Searching by Number: {productSizeNumber}, Found: {productSize != null}");
                    }
                    
                    // Fallback: match by OID if Number matching failed
                    if (productSize == null && int.TryParse(_currentProduct.ProductSize, out int productSizeOID))
                    {
                        productSize = _productSizes.FirstOrDefault(ps => ps.OID == productSizeOID);
                        System.Diagnostics.Debug.WriteLine($"Fallback search by OID: {productSizeOID}, Found: {productSize != null}");
                    }
                    
                    if (productSize != null)
                    {
                        // Find the index in the ComboBox items
                        int productSizeIndex = -1;
                        for (int i = 0; i < cmbProductSize.Items.Count; i++)
                        {
                            if (cmbProductSize.Items[i].ToString() == productSize.ToString())
                            {
                                productSizeIndex = i;
                                break;
                            }
                        }
                        
                        if (productSizeIndex >= 0)
                        {
                            cmbProductSize.SelectedIndex = productSizeIndex;
                            _selectedProductSize = productSize;
                            System.Diagnostics.Debug.WriteLine($"ProductSize set to index {productSizeIndex}: Number={productSize.Number}, OID={productSize.OID}");

                            // Load SizeLinks for this ProductSize using the Number value (not OID!)
                            LoadSizeLinksForProductSize(productSize.Number);

                            // Set SizeLink dropdown by matching OID directly
                            if (!string.IsNullOrEmpty(_currentProduct.SizeLink))
                            {
                                System.Diagnostics.Debug.WriteLine($"Looking for SizeLink OID: {_currentProduct.SizeLink}");
                                if (int.TryParse(_currentProduct.SizeLink, out int sizeLinkOID))
                                {
                                    // Find the index directly in the _sizeLinks list by OID
                                    int sizeLinkIndex = _sizeLinks.FindIndex(sl => sl.OID == sizeLinkOID);

                                    if (sizeLinkIndex >= 0 && sizeLinkIndex < _sizeLinks.Count)
                                    {
                                        cmbSizeLink.SelectedIndex = sizeLinkIndex;
                                        _selectedSizeLink = _sizeLinks[sizeLinkIndex];

                                        // Populate UnitOfMeasure from SizeLink Description
                                        _currentProduct.UnitOfMeasure = _selectedSizeLink.Description ?? string.Empty;

                                        System.Diagnostics.Debug.WriteLine($"SizeLink set to index {sizeLinkIndex}: OID={_selectedSizeLink.OID}, Description={_selectedSizeLink.Description}");
                                        System.Diagnostics.Debug.WriteLine($"UnitOfMeasure set to: '{_currentProduct.UnitOfMeasure}'");
                                    }
                                    else
                                    {
                                        System.Diagnostics.Debug.WriteLine($"SizeLink OID {sizeLinkOID} not found in loaded size links. Available SizeLinks count: {_sizeLinks.Count}");
                                    }
                                }
                                else
                                {
                                    System.Diagnostics.Debug.WriteLine($"Could not parse SizeLink value as integer: {_currentProduct.SizeLink}");
                                }
                            }
                        }
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"ProductSize not found: {_currentProduct.ProductSize}");
                    }
                }

                // Set Brand Name dropdown
                if (!string.IsNullOrEmpty(_currentProduct.BrandName))
                {
                    System.Diagnostics.Debug.WriteLine($"Looking for BrandName: {_currentProduct.BrandName}");
                    var brandExists = _brandNames.Contains(_currentProduct.BrandName);
                    if (brandExists)
                    {
                        int brandIndex = cmbBrandName.Items.IndexOf(_currentProduct.BrandName);
                        if (brandIndex >= 0)
                        {
                            cmbBrandName.SelectedIndex = brandIndex;
                            System.Diagnostics.Debug.WriteLine($"BrandName set to index {brandIndex}: {_currentProduct.BrandName}");
                        }
                    }
                    else
                    {
                        // If brand doesn't exist in dropdown, set it as text
                        cmbBrandName.Text = _currentProduct.BrandName;
                        System.Diagnostics.Debug.WriteLine($"BrandName set as text: {_currentProduct.BrandName}");
                    }
                }
                
                System.Diagnostics.Debug.WriteLine("=== SetDropdownsFromTemplate Completed ===");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in SetDropdownsFromTemplate: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
            }
        }

        private decimal ParseDecimalWithCommaOrDot(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return 0m;

            // Replace comma with dot for parsing
            string normalizedValue = value.Trim().Replace(',', '.');
            
            if (decimal.TryParse(normalizedValue, NumberStyles.Float, CultureInfo.InvariantCulture, out decimal result))
                return result;
            
            return 0m;
        }

        #endregion

        #region Connection Management

        /// <summary>
        /// Handles database connection change event - reloads all dropdown data from the new database
        /// </summary>
        private void OnConnectionChanged(object sender, ConnectionChangedEventArgs e)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"KingsleyProductsCreateForm: Connection changed to {e.ConnectionString}");

                // Update the connection string
                _connectionString = e.ConnectionString;

                // Clear existing session products since they're from the old database
                _sessionProducts.Clear();
                _sessionProductsBindingSource.ResetBindings(false);

                // Reload all dropdown data from the new database
                LoadSuppliers();
                LoadProductSizes();
                LoadBrandNames();
                LoadDepartments();

                // Clear the current product and reset form
                ClearCurrentProduct();

                System.Diagnostics.Debug.WriteLine("KingsleyProductsCreateForm: Successfully reloaded data from new database");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"KingsleyProductsCreateForm: Error reloading data after connection change: {ex.Message}");
                MessageBox.Show($"Error loading data from new database: {ex.Message}", "Connection Change Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion
    }
}