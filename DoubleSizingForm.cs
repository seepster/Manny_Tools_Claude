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
    public partial class DoubleSizingForm : Form
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

        // Lists to store the generated double sizes
        private List<string> firstSizes = new List<string>();
        private List<string> secondSizes = new List<string>();

        public DoubleSizingForm(
            string parentCode,
            bool isNewParentCode,
            string description,
            float markup,
            float costPriceExcl,
            string supplierCode,
            CreateSizesSupplierItem supplier,
            CreateSizesProductSizeItem productSize,
            string department,
            string taxValue)
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
                ? $"Double Sizing for New Parent Code {ParentCode}"
                : $"Double Sizing for Existing Parent Code {ParentCode}";

            // Load existing product codes
            LoadExistingProductCodes();

            // Initialize comboboxes
            InitializeControls();
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

        private void InitializeControls()
        {
            // Initialize first sizing start range
            for (int i = 12; i <= 88; i += 2)
            {
                comboFirstStart.Items.Add(i.ToString());
            }
            comboFirstStart.SelectedIndex = 10; // Default to 32 (index 10 is value 32)

            // Initialize first sizing end range
            for (int i = 12; i <= 88; i += 2)
            {
                comboFirstEnd.Items.Add(i.ToString());
            }
            comboFirstEnd.SelectedIndex = 16; // Default to 44 (index 16 is value 44)

            // Initialize first sizing increment
            comboFirstIncrement.Items.Add("1");
            comboFirstIncrement.Items.Add("2");
            comboFirstIncrement.SelectedIndex = 1; // Default to 2

            // Initialize second sizing start range
            for (int i = 12; i <= 88; i += 2)
            {
                comboSecondStart.Items.Add(i.ToString());
            }
            comboSecondStart.SelectedIndex = 9; // Default to 30 (index 9 is value 30)

            // Initialize second sizing end range
            for (int i = 12; i <= 88; i += 2)
            {
                comboSecondEnd.Items.Add(i.ToString());
            }
            comboSecondEnd.SelectedIndex = 12; // Default to 36 (index 12 is value 36)

            // Initialize second sizing increment
            comboSecondIncrement.Items.Add("1");
            comboSecondIncrement.Items.Add("2");
            comboSecondIncrement.SelectedIndex = 1; // Default to 2
        }

        private void btnGenerateSizes_Click(object sender, EventArgs e)
        {
            // Clear previous items
            listSizes.Items.Clear();
            firstSizes.Clear();
            secondSizes.Clear();

            if (!ValidateRanges())
            {
                return;
            }

            // Get the range and increment values for first sizing
            int firstStart = int.Parse(comboFirstStart.SelectedItem.ToString());
            int firstEnd = int.Parse(comboFirstEnd.SelectedItem.ToString());
            int firstIncrement = int.Parse(comboFirstIncrement.SelectedItem.ToString());

            // Get the range and increment values for second sizing
            int secondStart = int.Parse(comboSecondStart.SelectedItem.ToString());
            int secondEnd = int.Parse(comboSecondEnd.SelectedItem.ToString());
            int secondIncrement = int.Parse(comboSecondIncrement.SelectedItem.ToString());

            // Generate first sizes
            for (int size = firstStart; size <= firstEnd; size += firstIncrement)
            {
                firstSizes.Add(size.ToString());
            }

            // Generate second sizes
            for (int size = secondStart; size <= secondEnd; size += secondIncrement)
            {
                secondSizes.Add(size.ToString());
            }

            // Generate combined sizes
            foreach (string firstSize in firstSizes)
            {
                foreach (string secondSize in secondSizes)
                {
                    listSizes.Items.Add($"{firstSize}/{secondSize}");
                }
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
            if (comboFirstStart.SelectedIndex == -1 || comboFirstEnd.SelectedIndex == -1 || comboFirstIncrement.SelectedIndex == -1 ||
                comboSecondStart.SelectedIndex == -1 || comboSecondEnd.SelectedIndex == -1 || comboSecondIncrement.SelectedIndex == -1)
            {
                MessageBox.Show("Please select all range and increment values.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            int firstStart = int.Parse(comboFirstStart.SelectedItem.ToString());
            int firstEnd = int.Parse(comboFirstEnd.SelectedItem.ToString());
            int secondStart = int.Parse(comboSecondStart.SelectedItem.ToString());
            int secondEnd = int.Parse(comboSecondEnd.SelectedItem.ToString());

            if (firstStart >= firstEnd)
            {
                MessageBox.Show("First sizing start range must be less than end range.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (secondStart >= secondEnd)
            {
                MessageBox.Show("Second sizing start range must be less than end range.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                string sizeText = item.ToString();
                string[] sizeParts = sizeText.Split('/');

                // Use the combined size parts as the size code
                string sizeCode = $"{sizeParts[0]}{sizeParts[1]}";

                // Create a new product
                Orbit_ProductInfo product = Orbit_DTO_Create.CreateProductInfoEntry
                                                                                     (string.Format(sizeParts[0] + sizeParts[1] + ParentCode),
                                                                                     ParentCode,
                                                                                     Description,
                                                                                     sizeText,
                                                                                     Department,
                                                                                     ProductSize.OID.ToString(),
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