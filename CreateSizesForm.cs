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
        private CreateSizesProductSizeItem selectedProductSize = new CreateSizesProductSizeItem();
        private CreateSizesSupplierItem selectedSupplier = new CreateSizesSupplierItem();
        private CreateSizesTemplateItem activeTemplate = new CreateSizesTemplateItem();

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
            //load Product Size dropdown from sql connection
            string queryForProductSize = "select Number, Description from ProductSize";
            try
            {
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
            string queryForSizeLink = $"select SizeCode, Description from SizeLink where Holder = '{productSizes[selectedProductSize].Number}'";
            sizeLinks = SQL_Get_Generic_List.ExecuteQuery<CreateSizesSizeLinkItem>(DatabaseConnectionManager.Instance.ConnectionString, queryForSizeLink, null);

            foreach (CreateSizesSizeLinkItem item in sizeLinks)
            {
                SizeLinksListBox.Items.Add(item.ToString());
            }
        }

        public void LoadSuppliers()
        {
            SupplierComboBox.Items.Clear();

            //load SizeLink list from sql connection using selected Product Size
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
            LoadProductSizes();
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

            for (int i = 0; i < productSizes.Count; i++)
            {
                if (productSizes[i].Number == activeTemplate.ProductSize.Number)
                {
                    ProductSizeComboBox.SelectedIndex = i;
                }
            }

            DescriptionTextBox.Text = activeTemplate.Description;
            MarkupTextBox.Text = activeTemplate.Markup.ToString();
            CostPriceExclTextBox.Text = activeTemplate.CostPriceExcl.ToString();
            SupplierCodeTextBox.Text = activeTemplate.SupplierCode;
            for (int i = 0; i < suppliers.Count; i++)
            {
                if (suppliers[i].SupplierNumber == activeTemplate.Supplier.SupplierNumber)
                {
                    SupplierComboBox.SelectedIndex = i;
                }
            }
            
        }

       

        private void SupplierNumberComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedSupplier = suppliers[SupplierComboBox.SelectedIndex];
        }
    }
}