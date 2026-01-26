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
    public partial class DescriptiveSizingForm : Form
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
        public string Department { get; private set; }
        public string TaxValue { get; private set; }
        public string SizeLink { get; private set; }

        // Properties to store generated sizes
        public List<Orbit_ProductInfo> ProductsToCreate { get; private set; } = new List<Orbit_ProductInfo>();
        public List<string> ExistingProductCodes { get; private set; } = new List<string>();

        // Descriptive sizes
        private readonly string[] descriptiveSizes = new string[]
        {
            "ExtraSmall", "Small", "Medium", "Large", "XLarge",
            "XXLarge", "XXXLarge", "XXXXLarge", "XXXXXLarge"
        };

        // Size codes to use
        private readonly string[] sizeCodes = new string[]
        {
            "10", "20", "30", "40", "50", "60", "70", "80", "90"
        };

        public DescriptiveSizingForm(
            string parentCode,
            bool isNewParentCode,
            string description,
            float markup,
            float costPriceExcl,
            string supplierCode,
            CreateSizesSupplierItem supplier,
            CreateSizesProductSizeItem productSize,
            string Department,
            string TaxValue)
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

            // Set form title based on whether we're creating a new parent code
            this.Text = IsNewParentCode
                ? $"Descriptive Sizing for New Parent Code {ParentCode}"
                : $"Descriptive Sizing for Existing Parent Code {ParentCode}";

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
            // Add all descriptive sizes to the checklist
            for (int i = 0; i < descriptiveSizes.Length; i++)
            {
                listSizes.Items.Add($"{descriptiveSizes[i]} ({sizeCodes[i]})");
            }

            // Select items by default
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
                string sizeName = descriptiveSizes[index];
                string sizeCode = sizeCodes[index];

                // Create a new product
                Orbit_ProductInfo product = Orbit_DTO_Create.CreateProductInfoEntry(string.Format(sizeCode + ParentCode),
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