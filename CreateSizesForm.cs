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
using Manny_Tools_Claude.DTO;

namespace Manny_Tools_Claude
{
    public partial class CreateSizesForm : UserControl
    {
        // Define fields to store form data
        private List<CreateSizesProductSizeItem> productSizes = new List<CreateSizesProductSizeItem>();
        private List<CreateSizesSizeLinkItem> sizeLinks = new List<CreateSizesSizeLinkItem>();
        private List<CreateSizesSupplierItem> suppliers = new List<CreateSizesSupplierItem>();
        private List<string> productCodes = new List<string>();
        private List<string> parentCodes = new List<string>();
        private List<string> sixDigitParentCodes = new List<string>();
        private List<CSVFileProduct> createdProducts = new List<CSVFileProduct>();
        private CreateSizesProductSizeItem selectedProductSize = new CreateSizesProductSizeItem();
        private CreateSizesSupplierItem selectedSupplier = new CreateSizesSupplierItem();
        private CreateSizesTemplateItem activeTemplate = new CreateSizesTemplateItem();
        private int nextAvailableParentCode = 100000;

        public CreateSizesForm()
        {
            InitializeComponent();
            try
            {
                LoadProductSizes();
                LoadSizeLinks();
                LoadSuppliers();
            }
            catch
            {
                MessageBox.Show("Some elements could not be loaded.");
            }

        }

        public void LoadProductSizes()
        {
            ProductSizeComboBox.Items.Clear();
            //load Product Size dropdown from sql connection
            string queryForProductSize = "select OID, Number, Description from ProductSize";
            try
            {
                DatabaseConnectionManager.Instance.LoadConnectionString();
                productSizes = SQL_Get_Generic_List.ExecuteQuery<CreateSizesProductSizeItem>(DatabaseConnectionManager.Instance.ConnectionString, queryForProductSize, null);

                foreach (CreateSizesProductSizeItem item in productSizes)
                {
                    ProductSizeComboBox.Items.Add(item.ToString());
                }
            }
            catch
            {
                MessageBox.Show("No element could be loaded.");
            }
        }

        public void LoadSizeLinks()
        {
            SizeLinksListBox.Items.Clear();
            int selectedProductSize = ProductSizeComboBox.SelectedIndex;
            if (selectedProductSize < 0 || selectedProductSize > ProductSizeComboBox.Items.Count)
            {
                selectedProductSize = 0;
            }
            //load SizeLink list from sql connection using selected Product Size
            string queryForSizeLink = $"select SizeCode, Description from SizeLink where Holder = '{productSizes[selectedProductSize].OID}'";
            sizeLinks = SQL_Get_Generic_List.ExecuteQuery<CreateSizesSizeLinkItem>(DatabaseConnectionManager.Instance.ConnectionString, queryForSizeLink, null);

            foreach (CreateSizesSizeLinkItem item in sizeLinks)
            {
                SizeLinksListBox.Items.Add(item.ToString());
            }
        }

        public void LoadSuppliers()
        {
            SupplierComboBox.Items.Clear();

            //load suppliers list from sql connection 
            string queryForSuppliers = $"select AccountNumber, AccountName from Supplier";
            suppliers = SQL_Get_Generic_List.ExecuteQuery<CreateSizesSupplierItem>(DatabaseConnectionManager.Instance.ConnectionString, queryForSuppliers, null);

            foreach (CreateSizesSupplierItem supplier in suppliers)
            {
                SupplierComboBox.Items.Add(supplier.ToString());
            }
            if (suppliers.Count > 0)
            {
                selectedSupplier = suppliers[0];
            }

        }

        public void LoadParentCodes()
        {
            string queryForParentCodes = "select ParentCode from ProductInfo sort asc";
            parentCodes = SQL_Get_Generic_List.ExecuteQuery<string>(DatabaseConnectionManager.Instance.ConnectionString, queryForParentCodes, null);

        }

        public void LoadProductCodes()
        {
            string queryForProductCodes = "select ProductCode from ProductInfo";
            productCodes = SQL_Get_Generic_List.ExecuteQuery<string>(DatabaseConnectionManager.Instance.ConnectionString, queryForProductCodes, null);
        }

        public void CreateActiveTemplate(string ProductParentCode)
        {
            string productCode = ProductParentCode;
            if (productCode != null && productCode != string.Empty)
            {
                try
                {
                    string queryProductCodeParentCode = $"select top 1 ParentCode from ProductInfo where ProductCode = '{productCode}'";
                    int querySingleResultParentCode = SQL_Get_Generic_List.ExecuteSingle<int>(DatabaseConnectionManager.Instance.ConnectionString, queryProductCodeParentCode, null);
                    if (!(querySingleResultParentCode >= 0))
                    {
                        MessageBox.Show("Invalid Product/Parent Code for template.");
                        return;
                    }

                    int ParentCode = querySingleResultParentCode;
                    string queryProductInfoUsingParentCode = $"select top 1 ParentCode, Description, UnitSize, CostPriceExcl, Markup, ProductSize, SizeLink from ProductInfo where ParentCode = '{ParentCode}' ";
                    ProductInfo productInfo = SQL_Get_Generic_List.ExecuteSingle<ProductInfo>(DatabaseConnectionManager.Instance.ConnectionString, queryProductInfoUsingParentCode, null);
                    if (productInfo == null)
                    {
                        MessageBox.Show("Invalid Product/Parent Code for template.");
                        return;
                    }

                    activeTemplate.ParentCode = productInfo.ParentCode;
                    activeTemplate.Description = productInfo.Description;
                    activeTemplate.Markup = productInfo.Markup;
                    activeTemplate.CostPriceExcl = productInfo.CostPriceExcl;

                    activeTemplate.ProductSize = new CreateSizesProductSizeItem();
                    activeTemplate.ProductSize.Number = productInfo.ProductSize;

                    string querySizeLink = $"select SizeCode, Description from SizeLink where Holder = '{activeTemplate.ProductSize.Number}'";
                    activeTemplate.SizeLinks = SQL_Get_Generic_List.ExecuteQuery<CreateSizesSizeLinkItem>(DatabaseConnectionManager.Instance.ConnectionString, querySizeLink, null);


                    string querySupplierProductLinks = $"select top 1 SupplierProductcode from supplierproductlinks where ProductCode = '{ParentCode}'";
                    string supplierProductCode = SQL_Get_Generic_List.ExecuteSingle<string>(DatabaseConnectionManager.Instance.ConnectionString, querySupplierProductLinks, null);
                    activeTemplate.SupplierCode = supplierProductCode;

                    string querySupplierNumber = $"select top 1 CreditorNumber from supplierproductlinks where ProductCode = '{ParentCode}'";
                    string supplierNumber = SQL_Get_Generic_List.ExecuteSingle<string>(DatabaseConnectionManager.Instance.ConnectionString, querySupplierNumber, null);
                    string querySupplierDetails = $"select top 1 AccountNumber, AccountName from Supplier where AccountNumber = '{supplierNumber}'";
                    activeTemplate.Supplier = SQL_Get_Generic_List.ExecuteSingle<CreateSizesSupplierItem>(DatabaseConnectionManager.Instance.ConnectionString, querySupplierDetails, null);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading template: ");

                }
            }

        }

        private void ProductSizeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedProductSize = productSizes[ProductSizeComboBox.SelectedIndex];
            LoadSizeLinks();
        }

        private void SelectAllCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (SelectAllCheckBox.Checked == true)
            {
                for (int i = 0; i < SizeLinksListBox.Items.Count; i++)
                {
                    SizeLinksListBox.SetItemChecked(i, true);
                }
            }
            else
            {
                for (int i = 0; i < SizeLinksListBox.Items.Count; i++)
                {
                    SizeLinksListBox.SetItemChecked(i, false);
                }
            }
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
                if (activeTemplate.ProductSize != null && activeTemplate.ProductSize.Description != null)
                {
                    for (int i = 0; i < productSizes.Count; i++)
                    {
                        if (productSizes[i].Number == activeTemplate.ProductSize.Number)
                        {
                            ProductSizeComboBox.SelectedIndex = i;
                        }
                    }


                }
                MarkupTextBox.Text = activeTemplate.Markup.ToString();
                CostPriceExclTextBox.Text = activeTemplate.CostPriceExcl.ToString();

                if (activeTemplate.Supplier.AccountNumber != null)
                {
                    for (int i = 0; i < suppliers.Count; i++)
                    {
                        if (suppliers[i].AccountNumber == activeTemplate.Supplier.AccountNumber)
                        {
                            SupplierComboBox.SelectedIndex = i;
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
            string queryParentCode = "select ParentCode from ProductInfo";
            parentCodes = SQL_Get_Generic_List.ExecuteQuery<string>(DatabaseConnectionManager.Instance.ConnectionString, queryParentCode, null);

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

        private void CreateCSVFileButton_Click(object sender, EventArgs e)
        {
            this.DisableForm();
            //check if parentcode textbox.text is valid parent code
            if(parentCodes.Contains(Product_ParentCodeTextBox.Text) || productCodes.Contains(Product_ParentCodeTextBox.Text) || Product_ParentCodeTextBox.Text.Length != 6)
            {
                MessageBox.Show("Invalid Parent Code.");
                this.EnableForm();
                return;
            }
            string validParentCode = Product_ParentCodeTextBox.Text;
            //first we check which size links are selected
            for (int i = 0; i < SizeLinksListBox.Items.Count; i++)
            {
                if (SizeLinksListBox.GetItemChecked(i))
                {
                    CreateSizesSizeLinkItem checkedSizeLinkItem = sizeLinks[i];
                    string productCode = checkedSizeLinkItem.SizeCode + validParentCode;
                    if (productCodes.Contains(productCode) || parentCodes.Contains(productCode))
                    {
                        MessageBox.Show($"Generated Product Code {productCode} is in use. Choose a new parent Code.");
                        this.EnableForm();
                        return;
                    }
                    CSVFileProduct cSVFileProduct = new CSVFileProduct();
                    cSVFileProduct.ProductCode = productCode;
                    cSVFileProduct.Description =  $"{DescriptionTextBox.Text}";
                    cSVFileProduct.SizeCode = checkedSizeLinkItem.OID.ToString();
                    cSVFileProduct.ProductSize = selectedProductSize.OID.ToString();
                    cSVFileProduct.SupplierCode = SupplierCodeTextBox.Text;
                    cSVFileProduct.SupplierNumber = selectedSupplier.AccountNumber;
                    cSVFileProduct.Markup = MarkupTextBox.Text;
                    cSVFileProduct.CostPriceExcl = CostPriceExclTextBox.Text;
                    cSVFileProduct.fieldTwo = null;
                    cSVFileProduct.fieldEight = "0";
                    cSVFileProduct.fieldNine = "0";
                    cSVFileProduct.fieldTen = "1";
                    cSVFileProduct.Markup2 = MarkupTextBox.Text;
                    cSVFileProduct.CostPriceExcl2 = CostPriceExclTextBox.Text;
                    cSVFileProduct.CostPriceExcl3 = CostPriceExclTextBox.Text;
                    cSVFileProduct.SizeCode2 = checkedSizeLinkItem.SizeCode;
                    cSVFileProduct.fieldThirteen = "0";
                    cSVFileProduct.fieldFourteen = "0";
                    cSVFileProduct.fieldFifteen = "0";
                    cSVFileProduct.fieldEighteen = "0";
                    cSVFileProduct.fieldNineteen = "0";
                    cSVFileProduct.fieldTwenty = "0";
                    cSVFileProduct.fieldTwentyOne = "1";
                    cSVFileProduct.fieldTwentyTwo = "0";
                    cSVFileProduct.fieldTwentyThree = "1";
                    cSVFileProduct.fieldTwentyFour = "1";
                    cSVFileProduct.fieldTwentyFive = "1";
                    cSVFileProduct.fieldTwentySix = "1";
                    cSVFileProduct.fieldTwentySeven = "0";
                    cSVFileProduct.fieldTwentyEight = "1";
                    cSVFileProduct.fieldTwentyNine = "0";
                    cSVFileProduct.fieldThirty = "0";
                    cSVFileProduct.fieldThirtyOne = null;
                    cSVFileProduct.fieldThirtyTwo = "1";
                    cSVFileProduct.fieldThirtyThree = "1";
                    cSVFileProduct.fieldThirtyFour = "1";
                    cSVFileProduct.fieldThirtyFive = "1";
                    cSVFileProduct.fieldThirtySix = "0";
                    cSVFileProduct.fieldThirtySeven = "0";
                    cSVFileProduct.fieldThirtyEight = "0";
                    cSVFileProduct.fieldThirtyNine = "0";
                    cSVFileProduct.fieldForty = "0";
                    cSVFileProduct.fieldFortyOne = "0";
                    cSVFileProduct.fieldFortyThree = "0";
                    cSVFileProduct.fieldFortyFour = "0";
                    cSVFileProduct.fieldFortyFive = "1";
                    cSVFileProduct.fieldFortySix = "0";
                    cSVFileProduct.fieldFortySeven = "0";
                    cSVFileProduct.fieldFortyEight = "0";
                    cSVFileProduct.fieldFortyNine = "0";
                    cSVFileProduct.fieldFifty = "0";
                    cSVFileProduct.fieldFiftyOne = "0";
                    cSVFileProduct.fieldFiftyThree = "1";
                    cSVFileProduct.fieldFiftyFour = "0";
                    createdProducts.Add(cSVFileProduct);

                    //check if the generated product codes exist in either the ProductCode or ParentCode fields in the ProductInfo table
                    //build each product into a line for the csv file
                }
            }
            this.EnableForm();
            //write the csv file and prompt the user for the output location
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "CSV files (*.csv)|*.csv";
                saveFileDialog.Title = "Save NewPLU.csv";
                saveFileDialog.DefaultExt = "csv";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Get the file path from dialog
                        string filePath = saveFileDialog.FileName;

                        // Create file and write headers (property names)
                        using (StreamWriter writer = new StreamWriter(filePath))
                        {
                            // Get properties of the type
                            var properties = typeof(CSVFileProduct).GetProperties();

                            // Write header row
                            string headerLine = string.Join(",", properties.Select(p => p.Name));
                            writer.WriteLine(headerLine);

                            // Write each item as a CSV line
                            foreach (var item in createdProducts)
                            {
                                // Get value of each property and join with commas
                                string line = string.Join(",", properties.Select(p =>
                                {
                                    var value = p.GetValue(item);
                                    return value != null ? value.ToString() : string.Empty;
                                }));

                                writer.WriteLine(line);
                            }
                        }

                        MessageBox.Show($"Successfully exported {createdProducts.Count} items to CSV.",
                                       "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error exporting to CSV: {ex.Message}",
                                       "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }




            //second we check if the parentcode is valid
            //third we generate product codes for each size link
            //fourth we check if the generated product codes exist in either the ProductCode or ParentCode fields in the ProductInfo table
            //fifth we build each product into a line for the csv file
        }

        private void BtnDetermineSizingMethod_Click(object sender, EventArgs e)
        {
            //DetermineSizingMethod();
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