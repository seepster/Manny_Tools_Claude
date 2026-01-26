using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manny_Tools_Claude
{
    internal class CreateSizesBin
    {
        //    private void CreatePreFixedProductCodes()
        //    {
        //        // Clear previous products
        //        productsToCreate.Clear();

        //        // Validate required fields
        //        if (string.IsNullOrEmpty(Product_ParentCodeTextBox.Text))
        //        {
        //            MessageBox.Show("Parent Code is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //            return;
        //        }

        //        if (Product_ParentCodeTextBox.Text.Length != 6 || !int.TryParse(Product_ParentCodeTextBox.Text, out _))
        //        {
        //            MessageBox.Show("Parent Code must be a 6-digit number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //            return;
        //        }

        //        if (string.IsNullOrEmpty(DescriptionTextBox.Text))
        //        {
        //            MessageBox.Show("Description is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //            return;
        //        }

        //        decimal markup;
        //        if (!decimal.TryParse(MarkupTextBox.Text, out markup))
        //        {
        //            MessageBox.Show("Invalid Markup value.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //            return;
        //        }

        //        decimal costPriceExcl;
        //        if (!decimal.TryParse(CostPriceExclTextBox.Text, out costPriceExcl))
        //        {
        //            MessageBox.Show("Invalid Cost Price value.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //            return;
        //        }

        //        if (SupplierComboBox.SelectedIndex < 0)
        //        {
        //            MessageBox.Show("Please select a Supplier.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //            return;
        //        }

        //        // Check if any size links are selected
        //        if (SizeLinksListBox.CheckedItems.Count == 0)
        //        {
        //            MessageBox.Show("Please select at least one Size Link.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //            return;
        //        }

        //        string parentCode = Product_ParentCodeTextBox.Text;

        //        // Load product codes to check for duplicates
        //        LoadProductCodes();

        //        // Generate products with prefixes
        //        string preFix = string.Empty;
        //        int selectedIndexProductSize = ProductSizeComboBox.SelectedIndex;
        //        if (productSizes[selectedIndexProductSize].Number == 21)
        //        {

        //        }

        //        for (int i = 0; i < SizeLinksListBox.Items.Count; i++)
        //        {
        //            if (SizeLinksListBox.GetItemChecked(i))
        //            {
        //                CreateSizesSizeLinkItem checkedSizeLinkItem = sizeLinks[i];
        //                string productCode = preFix + parentCode;

        //                CSVFileProduct csvFileProduct = new CSVFileProduct();
        //                csvFileProduct.ProductCode = productCode;
        //                csvFileProduct.Description = $"{DescriptionTextBox.Text}";
        //                csvFileProduct.SizeCode = checkedSizeLinkItem.SizeCode;
        //                csvFileProduct.ProductSize = selectedProductSize.OID.ToString();
        //                csvFileProduct.SupplierCode = SupplierCodeTextBox.Text;
        //                csvFileProduct.SupplierNumber = selectedSupplier.AccountNumber;
        //                csvFileProduct.Markup = MarkupTextBox.Text;
        //                csvFileProduct.CostPriceExcl = CostPriceExclTextBox.Text;

        //                // Set default fields as in your commented code
        //                csvFileProduct.fieldTwo = null;
        //                csvFileProduct.fieldEight = "0";
        //                csvFileProduct.fieldNine = "0";
        //                csvFileProduct.fieldTen = "1";
        //                csvFileProduct.Markup2 = MarkupTextBox.Text;
        //                csvFileProduct.CostPriceExcl2 = CostPriceExclTextBox.Text;
        //                csvFileProduct.CostPriceExcl3 = CostPriceExclTextBox.Text;
        //                csvFileProduct.SizeCode2 = checkedSizeLinkItem.SizeCode;
        //                csvFileProduct.fieldThirteen = "0";
        //                // Add all other default fields...
        //                csvFileProduct.fieldFourteen = "0";
        //                csvFileProduct.fieldFifteen = "0";
        //                csvFileProduct.fieldEighteen = "0";
        //                csvFileProduct.fieldNineteen = "0";
        //                csvFileProduct.fieldTwenty = "0";
        //                csvFileProduct.fieldTwentyOne = "1";
        //                csvFileProduct.fieldTwentyTwo = "0";
        //                csvFileProduct.fieldTwentyThree = "1";
        //                csvFileProduct.fieldTwentyFour = "1";
        //                csvFileProduct.fieldTwentyFive = "1";
        //                csvFileProduct.fieldTwentySix = "1";
        //                csvFileProduct.fieldTwentySeven = "0";
        //                csvFileProduct.fieldTwentyEight = "1";
        //                csvFileProduct.fieldTwentyNine = "0";
        //                csvFileProduct.fieldThirty = "0";
        //                csvFileProduct.fieldThirtyOne = null;
        //                csvFileProduct.fieldThirtyTwo = "1";
        //                csvFileProduct.fieldThirtyThree = "1";
        //                csvFileProduct.fieldThirtyFour = "1";
        //                csvFileProduct.fieldThirtyFive = "1";
        //                csvFileProduct.fieldThirtySix = "0";
        //                csvFileProduct.fieldThirtySeven = "0";
        //                csvFileProduct.fieldThirtyEight = "0";
        //                csvFileProduct.fieldThirtyNine = "0";
        //                csvFileProduct.fieldForty = "0";
        //                csvFileProduct.fieldFortyOne = "0";
        //                csvFileProduct.fieldFortyThree = "0";
        //                csvFileProduct.fieldFortyFour = "0";
        //                csvFileProduct.fieldFortyFive = "1";
        //                csvFileProduct.fieldFortySix = "0";
        //                csvFileProduct.fieldFortySeven = "0";
        //                csvFileProduct.fieldFortyEight = "0";
        //                csvFileProduct.fieldFortyNine = "0";
        //                csvFileProduct.fieldFifty = "0";
        //                csvFileProduct.fieldFiftyOne = "0";
        //                csvFileProduct.fieldFiftyThree = "1";
        //                csvFileProduct.fieldFiftyFour = "0";

        //                // Check if product already exists
        //                //csvFileProduct.Exists = productCodes.Contains(productCode);

        //                // Add to the list of products to create
        //                productsToCreate.Add(csvFileProduct);

        //            }
        //        }
        //    }
        //}

    // Determine which method to use based on the selected index (reading from left to right)
    //the available methods are :
    //FourDigitPantsPreFix
    // -- First two digits are waist size (ex. 26-56)
    // -- Last two digits are leg size (ex. 28-32)
    //ThreeOrTwoDigitShoePreFix
    //--First Two/One digits are the size (ex 9, 12)
    //--Last digit is 0 for whole number size unless half size shoe which is 5
    //TwoDigitShirtPreFix
    //-- Starts at smallest size (ex. xtra small) using '10' prefix and increments 10 at a time
    //TwoDigitPantsPreFix
    //--two digit waist size OR leg size (ex. 26-56)
    //TwoDigitBabyPreFix
    //-- Starts at smalles size (ex. 0-3months), Prefix would be 10, increments 10 at a time
    //GenericTwoDigitPreFix
    //--simply start at 10 and increment as desired (ex. 1/5/10)


    //private void CreateCSVFileButton_Click(object sender, EventArgs e)
    //{
    //    // Clear previous products
    //    productsToCreate.Clear();

    //    // Check which product size is selected
    //    int selectedIndex = ProductSizeComboBox.SelectedIndex;
    //    if (selectedIndex < 0)
    //    {
    //        MessageBox.Show("Please select a Product Size before creating CSV file.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
    //        return;
    //    }


    //    if (productSizes[selectedIndex].Number == 21)
    //    {
    //        for (int i = 0; i < SizeLinksListBox.SelectedIndices.Count; i++)
    //        {

    //        }
    //    }

    //    else
    //    {
    //        MessageBox.Show("Selected Product Size is not supported for CSV creation.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    //        return;
    //    }

    //    // Show the created products for approval
    //    if (productsToCreate.Count > 0)
    //    {
    //        ShowProductsForApproval();
    //    }
    //    else
    //    {
    //        MessageBox.Show("No products were created. Please select at least one Size Link.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
    //    }
    //}

    //private void CreateCSVProductsWithPrefix(string parentCode)
    //{
    //    // Check if any size links are selected
    //    if (SizeLinksListBox.CheckedItems.Count == 0)
    //    {
    //        MessageBox.Show("Please select at least one Size Link before creating products.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
    //        return;
    //    }

    //    // Validate required fields
    //    if (string.IsNullOrEmpty(DescriptionTextBox.Text))
    //    {
    //        MessageBox.Show("Description is required.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
    //        return;
    //    }

    //    decimal markup;
    //    if (!decimal.TryParse(MarkupTextBox.Text, out markup))
    //    {
    //        MessageBox.Show("Invalid Markup value.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
    //        return;
    //    }

    //    decimal costPriceExcl;
    //    if (!decimal.TryParse(CostPriceExclTextBox.Text, out costPriceExcl))
    //    {
    //        MessageBox.Show("Invalid Cost Price value.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
    //        return;
    //    }

    //    if (SupplierComboBox.SelectedIndex < 0)
    //    {
    //        MessageBox.Show("Please select a Supplier.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
    //        return;
    //    }

    //    for (int i = 0; i < SizeLinksListBox.Items.Count; i++)
    //    {
    //        if (SizeLinksListBox.GetItemChecked(i))
    //        {
    //            CreateSizesSizeLinkItem checkedSizeLinkItem = sizeLinks[i];
    //            string productCode = checkedSizeLinkItem.SizeCode + parentCode;
    //            if (productCodes.Contains(productCode) || parentCodes.Contains(productCode))
    //            {
    //                MessageBox.Show($"Generated Product Code {productCode} is in use. Choose a new parent Code.");
    //                this.EnableForm();
    //                return;
    //            }
    //            CSVFileProduct cSVFileProduct = new CSVFileProduct();
    //            cSVFileProduct.ProductCode = productCode;
    //            cSVFileProduct.Description = $"{DescriptionTextBox.Text}";
    //            cSVFileProduct.SizeCode = checkedSizeLinkItem.OID.ToString();
    //            cSVFileProduct.ProductSize = selectedProductSize.OID.ToString();
    //            cSVFileProduct.SupplierCode = SupplierCodeTextBox.Text;
    //            cSVFileProduct.SupplierNumber = selectedSupplier.AccountNumber;
    //            cSVFileProduct.Markup = MarkupTextBox.Text;
    //            cSVFileProduct.CostPriceExcl = CostPriceExclTextBox.Text;
    //            cSVFileProduct.fieldTwo = null;
    //            cSVFileProduct.fieldEight = "0";
    //            cSVFileProduct.fieldNine = "0";
    //            cSVFileProduct.fieldTen = "1";
    //            cSVFileProduct.Markup2 = MarkupTextBox.Text;
    //            cSVFileProduct.CostPriceExcl2 = CostPriceExclTextBox.Text;
    //            cSVFileProduct.CostPriceExcl3 = CostPriceExclTextBox.Text;
    //            cSVFileProduct.SizeCode2 = checkedSizeLinkItem.SizeCode;
    //            cSVFileProduct.fieldThirteen = "0";
    //            cSVFileProduct.fieldFourteen = "0";
    //            cSVFileProduct.fieldFifteen = "0";
    //            cSVFileProduct.fieldEighteen = "0";
    //            cSVFileProduct.fieldNineteen = "0";
    //            cSVFileProduct.fieldTwenty = "0";
    //            cSVFileProduct.fieldTwentyOne = "1";
    //            cSVFileProduct.fieldTwentyTwo = "0";
    //            cSVFileProduct.fieldTwentyThree = "1";
    //            cSVFileProduct.fieldTwentyFour = "1";
    //            cSVFileProduct.fieldTwentyFive = "1";
    //            cSVFileProduct.fieldTwentySix = "1";
    //            cSVFileProduct.fieldTwentySeven = "0";
    //            cSVFileProduct.fieldTwentyEight = "1";
    //            cSVFileProduct.fieldTwentyNine = "0";
    //            cSVFileProduct.fieldThirty = "0";
    //            cSVFileProduct.fieldThirtyOne = null;
    //            cSVFileProduct.fieldThirtyTwo = "1";
    //            cSVFileProduct.fieldThirtyThree = "1";
    //            cSVFileProduct.fieldThirtyFour = "1";
    //            cSVFileProduct.fieldThirtyFive = "1";
    //            cSVFileProduct.fieldThirtySix = "0";
    //            cSVFileProduct.fieldThirtySeven = "0";
    //            cSVFileProduct.fieldThirtyEight = "0";
    //            cSVFileProduct.fieldThirtyNine = "0";
    //            cSVFileProduct.fieldForty = "0";
    //            cSVFileProduct.fieldFortyOne = "0";
    //            cSVFileProduct.fieldFortyThree = "0";
    //            cSVFileProduct.fieldFortyFour = "0";
    //            cSVFileProduct.fieldFortyFive = "1";
    //            cSVFileProduct.fieldFortySix = "0";
    //            cSVFileProduct.fieldFortySeven = "0";
    //            cSVFileProduct.fieldFortyEight = "0";
    //            cSVFileProduct.fieldFortyNine = "0";
    //            cSVFileProduct.fieldFifty = "0";
    //            cSVFileProduct.fieldFiftyOne = "0";
    //            cSVFileProduct.fieldFiftyThree = "1";
    //            cSVFileProduct.fieldFiftyFour = "0";
    //            createdProducts.Add(cSVFileProduct);

    //            //check if the generated product codes exist in either the ProductCode or ParentCode fields in the ProductInfo table
    //            //build each product into a line for the csv file
    //        }
    //    }

    //}
    //private void ShowProductsForApproval()
    //{
    //    // Create a dialog to show the products
    //    Form approvalForm = new Form
    //    {
    //        Text = "Products for Approval",
    //        Size = new System.Drawing.Size(600, 400),
    //        StartPosition = FormStartPosition.CenterParent,
    //        FormBorderStyle = FormBorderStyle.FixedDialog,
    //        MaximizeBox = false,
    //        MinimizeBox = false
    //    };

    //    // Create a list box to display the products
    //    ListBox productListBox = new ListBox
    //    {
    //        Dock = DockStyle.Fill,
    //        FormattingEnabled = true
    //    };

    //    // Add the products to the list box
    //    foreach (CSVFileProduct product in productsToCreate)
    //    {
    //        productListBox.Items.Add(product.ToString());
    //    }

    //    // Create buttons for approval
    //    Button approveButton = new Button
    //    {
    //        Text = "Approve",
    //        DialogResult = DialogResult.OK
    //    };
    //    Button cancelButton = new Button
    //    {
    //        Text = "Cancel",
    //        DialogResult = DialogResult.Cancel
    //    };

    //    // Create a panel for the buttons
    //    Panel buttonPanel = new Panel
    //    {
    //        Dock = DockStyle.Bottom,
    //        Height = 50
    //    };

    //    // Add the buttons to the panel
    //    approveButton.Location = new System.Drawing.Point(approvalForm.Width - 170, 10);
    //    approveButton.Size = new System.Drawing.Size(75, 30);
    //    cancelButton.Location = new System.Drawing.Point(approvalForm.Width - 90, 10);
    //    cancelButton.Size = new System.Drawing.Size(75, 30);

    //    buttonPanel.Controls.Add(approveButton);
    //    buttonPanel.Controls.Add(cancelButton);

    //    // Add the controls to the form
    //    approvalForm.Controls.Add(productListBox);
    //    approvalForm.Controls.Add(buttonPanel);

    //    // Show the form and handle the result
    //    if (approvalForm.ShowDialog() == DialogResult.OK)
    //    {
    //        //CreateCSVFile();
    //    }
    //}

    //private void CreateCSVFileButton__Click(object sender, EventArgs e)
    //{
    //    this.DisableForm();
    //    //check if parentcode textbox.text is valid parent code
    //    if (parentCodes.Contains(Product_ParentCodeTextBox.Text) || productCodes.Contains(Product_ParentCodeTextBox.Text) || Product_ParentCodeTextBox.Text.Length != 6)
    //    {
    //        MessageBox.Show("Invalid Parent Code.");
    //        this.EnableForm();
    //        return;
    //    }
    //    string validParentCode = Product_ParentCodeTextBox.Text;

    //    //first we check which size links are selected

    //    this.EnableForm();
    //    //write the csv file and prompt the user for the output location
    //    using (SaveFileDialog saveFileDialog = new SaveFileDialog())
    //    {
    //        saveFileDialog.Filter = "CSV files (*.csv)|*.csv";
    //        saveFileDialog.Title = "Save NewPLU.csv";
    //        saveFileDialog.DefaultExt = "csv";

    //        if (saveFileDialog.ShowDialog() == DialogResult.OK)
    //        {
    //            try
    //            {
    //                // Get the file path from dialog
    //                string filePath = saveFileDialog.FileName;

    //                // Create file and write headers (property names)
    //                using (StreamWriter writer = new StreamWriter(filePath))
    //                {
    //                    // Get properties of the type
    //                    var properties = typeof(CSVFileProduct).GetProperties();

    //                    // Write header row
    //                    string headerLine = string.Join(",", properties.Select(p => p.Name));
    //                    writer.WriteLine(headerLine);

    //                    // Write each item as a CSV line
    //                    foreach (var item in createdProducts)
    //                    {
    //                        // Get value of each property and join with commas
    //                        string line = string.Join(",", properties.Select(p =>
    //                        {
    //                            var value = p.GetValue(item);
    //                            return value != null ? value.ToString() : string.Empty;
    //                        }));

    //                        writer.WriteLine(line);
    //                    }
    //                }

    //                MessageBox.Show($"Successfully exported {createdProducts.Count} items to CSV.",
    //                               "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
    //            }
    //            catch (Exception ex)
    //            {
    //                MessageBox.Show($"Error exporting to CSV: {ex.Message}",
    //                               "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    //            }
    //        }
    //    }




    //second we check if the parentcode is valid
    //third we generate product codes for each size link
    //fourth we check if the generated product codes exist in either the ProductCode or ParentCode fields in the ProductInfo table
    //fifth we build each product into a line for the csv file

}
}
