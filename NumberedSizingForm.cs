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
    public partial class NumberedSizingForm : Form
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

        public NumberedSizingForm(
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
            SizeLink = sizeLink;


            // Set form title based on whether we're creating a new parent code
            this.Text = IsNewParentCode
                ? $"Numbered Sizing for New Parent Code {ParentCode}"
                : $"Numbered Sizing for Existing Parent Code {ParentCode}";

            // Initialize comboboxes
            InitializeControls();
        }

        private void InitializeControls()
        {
            // Initialize start range
            for (int i = 1; i <= 99; i++)
            {
                comboStartRange.Items.Add(i.ToString());
            }
            comboStartRange.SelectedIndex = 0; // Default to 1

            // Initialize end range
            for (int i = 2; i <= 99; i++)
            {
                comboEndRange.Items.Add(i.ToString());
            }
            comboEndRange.SelectedIndex = 11; // Default to 13 (index 11 is value 13)

            // Initialize increment
            comboIncrement.Items.Add("0.5");
            comboIncrement.Items.Add("1");
            comboIncrement.Items.Add("2");
            comboIncrement.SelectedIndex = 1; // Default to 1

            // Load existing product codes
            LoadExistingProductCodes();
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

        private void btnGenerateSizes_Click(object sender, EventArgs e)
        {
            // Clear previous items
            listSizes.Items.Clear();

            if (!ValidateRanges())
            {
                return;
            }

            // Get the range and increment values
            int start = int.Parse(comboStartRange.SelectedItem.ToString());
            int end = int.Parse(comboEndRange.SelectedItem.ToString());
            float increment = float.Parse(comboIncrement.SelectedItem.ToString(), System.Globalization.CultureInfo.InvariantCulture);

            // Generate sizes based on range and increment
            for (float size = start; size <= end; size += increment)
            {
                string sizeText = size.ToString();
                if (size == (int)size)
                {
                    sizeText = ((int)size).ToString(); // Remove .0 for whole numbers
                }
                listSizes.Items.Add($"Size {sizeText}");
            }

            // Select all items by default
            for (int i = 0; i < listSizes.Items.Count; i++)
            {
                listSizes.SetItemChecked(i, true);
            }

            // Enable the product creation button
            btnCreateProducts.Enabled = true;
        }

        private bool ValidateRanges()
        {
            if (comboStartRange.SelectedIndex == -1 || comboEndRange.SelectedIndex == -1 || comboIncrement.SelectedIndex == -1)
            {
                MessageBox.Show("Please select all range and increment values.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            int start = int.Parse(comboStartRange.SelectedItem.ToString());
            int end = int.Parse(comboEndRange.SelectedItem.ToString());

            if (start >= end)
            {
                MessageBox.Show("Start range must be less than end range.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
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

            foreach (var item in listSizes.CheckedItems)
            {
                string? UnitOfMeasure = item.ToString();
                string? PreFixRaw = UnitOfMeasure.Substring(5);
                string PreFixFinal = string.Empty;
                if (PreFixRaw.Length == 1 || PreFixRaw.Length == 2)
                {
                    PreFixFinal = PreFixRaw + "0";
                }
                else
                    if (PreFixRaw.Contains(".5") || PreFixRaw.Contains(",5"))
                {
                    PreFixFinal = PreFixRaw.Replace(".", "").Replace(",", "") + "5";
                    UnitOfMeasure += "-";
                }


                // Create a new product
                Orbit_ProductInfo product = Orbit_DTO_Create.CreateProductInfoEntry
                                                                                    (string.Format(PreFixFinal + ParentCode),
                                                                                    ParentCode,
                                                                                    Description,
                                                                                    UnitOfMeasure,
                                                                                    Department,
                                                                                    ProductSize.Number.ToString(),
                                                                                    SupplierCode,
                                                                                    Supplier.AccountNumber.ToString(),
                                                                                    Markup.ToString(),
                                                                                    CostPriceExcl.ToString(),
                                                                                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                                                                    TaxValue,
                                                                                    SizeLink
                                                                                    );

                ProductsToCreate.Add(product);
            }
        }

        private void ShowProductConfirmationWindow()
        {
            // We'll implement this in the next class
            string message = $"Ready to create {ProductsToCreate.Count} products. Proceed to confirmation?";
            if (MessageBox.Show(message, "Create Products", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // For now, we'll just close this form with OK result
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}