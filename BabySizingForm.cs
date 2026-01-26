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
    public partial class BabySizingForm : Form
    {
        // Properties to store data from the parent form
        public string ParentCode { get; private set; }
        public bool IsNewParentCode { get; private set; }
        public string Description { get; private set; }
        public float Markup { get; private set; }
        public float CostPriceExcl { get; private set; }
        public string SupplierCode { get; private set; }
        public CreateSizesSupplierItem Supplier { get; private set; }
        public CreateSizesProductSizeItem ProductSize { get; private set; }

        // Properties to store generated sizes
        public List<Orbit_ProductInfo> ProductsToCreate { get; private set; } = new List<Orbit_ProductInfo>();
        public List<string> ExistingProductCodes { get; private set; } = new List<string>();
        public string Department { get; private set; }
        public string TaxValue { get; private set; }
        public string SizeLink { get; private set; }

        // Baby sizes
        private readonly string[] babySizes = new string[]
        {
            "0-3 months",
            "3-6 months",
            "6-9 months",
            "9-12 months",
            "1-2 years",
            "2-3 years",
            "3-4 years",
            "4-5 years"
        };

        // Size codes to use
        private readonly string[] sizeCodes = new string[]
        {
            "10",
            "20",
            "30",
            "40",
            "50",
            "60",
            "70",
            "80"
        };

        public BabySizingForm(
            string parentCode,
            bool isNewParentCode,
            string description,
            float markup,
            float costPriceExcl,
            string supplierCode,
            CreateSizesSupplierItem supplier,
            CreateSizesProductSizeItem productSize,
            string department,
            string taxValue,
            string sizeLink = null)
        {
            InitializeComponent();

            // Store the provided data
            ParentCode = parentCode;
            IsNewParentCode = isNewParentCode;
            Description = description;
            Markup = markup;
            CostPriceExcl = costPriceExcl;
            SupplierCode = supplierCode;
            Supplier = supplier;
            ProductSize = productSize;
            Department = department;
            TaxValue = taxValue;

            // Set form title based on whether we're creating a new parent code
            this.Text = IsNewParentCode
                ? $"Baby Sizing for New Parent Code {ParentCode}"
                : $"Baby Sizing for Existing Parent Code {ParentCode}";

            // Load existing product codes
            LoadExistingProductCodes();

            // Initialize sizes list
            InitializeSizesList();
        }

        private void LoadExistingProductCodes()
        {
            try
            {
                // Get existing product codes from the database
                string query = "SELECT ProductCode FROM ProductInfo";
                ExistingProductCodes = SQL_Get_Generic_List.ExecuteQuery<string>(
                    DatabaseConnectionManager.Instance.ConnectionString,
                    query,
                    null);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading existing product codes: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeSizesList()
        {
            // Add all baby sizes to the checklist
            for (int i = 0; i < babySizes.Length; i++)
            {
                listSizes.Items.Add($"{babySizes[i]} ({sizeCodes[i]})");
            }

            // Select items by default (all)
            for (int i = 0; i < listSizes.Items.Count; i++)
            {
                listSizes.SetItemChecked(i, true);
            }
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listSizes.Items.Count; i++)
            {
                listSizes.SetItemChecked(i, true);
            }
        }

        private void btnSelectNone_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listSizes.Items.Count; i++)
            {
                listSizes.SetItemChecked(i, false);
            }
        }

        private void btnCreateProducts_Click(object sender, EventArgs e)
        {
            if (listSizes.CheckedItems.Count == 0)
            {
                MessageBox.Show("Please select at least one size to create.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Generate product codes and create products
            GenerateProductsToCreate();

            // Show confirmation form with products to create
            ShowProductConfirmationWindow();
        }

        private void GenerateProductsToCreate()
        {
            ProductsToCreate.Clear();

            foreach (var item in listSizes.CheckedIndices)
            {
                int index = (int)item;
                string sizeName = babySizes[index];
                string sizeCode = sizeCodes[index];

                // Create a new product
                Orbit_ProductInfo product = Orbit_DTO_Create.CreateProductInfoEntry
                                                                                    (string.Format(sizeCode + ParentCode),
                                                                                    ParentCode,
                                                                                    Description,
                                                                                    sizeName,
                                                                                    Department,
                                                                                    ProductSize.Number.ToString(),
                                                                                    SupplierCode,
                                                                                    Supplier.AccountNumber.ToString(),
                                                                                    Markup.ToString(),
                                                                                    CostPriceExcl.ToString(),
                                                                                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                                                                    TaxValue
                                                                                    );

                ProductsToCreate.Add(product);
            }
        }

        private void ShowProductConfirmationWindow()
        {
            // We'll use the main form's method to show the confirmation window
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}