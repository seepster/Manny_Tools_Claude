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

namespace Manny_Tools_Claude
{
    public partial class CreateSizesForm : UserControl
    {
        // Define fields to store form data
        private List<ProductSizeItem> productSizes = new List<ProductSizeItem>();
        private List<SizeLinkItem> sizeLinks = new List<SizeLinkItem>();
        private List<Supplier> suppliers = new List<Supplier>();
        private Supplier selectedSupplier;
        private TemplateItem activeTemplate;


        // Class to hold ParentCode Template
        private class TemplateItem
        {
            public string ParentCode { get; set; }
            public string Description { get; set; }
            public ProductSizeItem ProductSize { get; set; }
            public List<SizeLinkItem> SizeLinks { get; set; }

            public double Markup { get; set; }
            public double CostPriceExcl { get; set; }
            public string SupplierCode { get; set; }
            public Supplier Supplier { get; set; }

        }

        private class Supplier
        {
            public string SupplierNumber { get; set; }
            public string SupplierName { get; set; }
            public override string ToString()
            {
                return $"{SupplierNumber} - {SupplierName}";
            }
        }

        // Class to hold ProductSize data
        private class ProductSizeItem
        {
            public int Number { get; set; }
            public string Description { get; set; }

            public override string ToString()
            {
                return $"{Number} {Description}";
            }
        }

        // Class to hold SizeLink data
        private class SizeLinkItem
        {
            public string SizeCode { get; set; }
            public string Description { get; set; }

            public override string ToString()
            {
                return $"{SizeCode} - {Description}";
            }
        }

        public CreateSizesForm()
        {
            InitializeComponent();
            LoadProductSizes();
            LoadSizeLinks();
            LoadSuppliers();
        }

        public void LoadProductSizes()
        {
            //load Product Size dropdown from sql connection
            string queryForProductSize = "select Number, Description from ProductSize";
            productSizes = SQL_Get_Generic_List.ExecuteQuery<ProductSizeItem>(DatabaseConnectionManager.Instance.ConnectionString, queryForProductSize, null);

            foreach (ProductSizeItem item in productSizes)
            {
                ProductSizeComboBox.Items.Add(item.ToString());
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
            sizeLinks = SQL_Get_Generic_List.ExecuteQuery<SizeLinkItem>(DatabaseConnectionManager.Instance.ConnectionString, queryForSizeLink, null);

            foreach (SizeLinkItem item in sizeLinks)
            {
                SizeLinksListBox.Items.Add(item.ToString());
            }
        }

        public void LoadSuppliers()
        {
            SupplierComboBox.Items.Clear();

            //load SizeLink list from sql connection using selected Product Size
            string queryForSuppliers = $"select AccountNumber, AccountName from Supplier";
            suppliers = SQL_Get_Generic_List.ExecuteQuery<Supplier>(DatabaseConnectionManager.Instance.ConnectionString, queryForSuppliers, null);

            foreach (Supplier supplier in suppliers)
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
                activeTemplate = new TemplateItem();
                activeTemplate.ParentCode = productInfo.ParentCode;
                activeTemplate.Description = productInfo.Description;
                activeTemplate.ProductSize = new ProductSizeItem();
                activeTemplate.ProductSize.Number = productInfo.ProductSize;

                string querySizeLink = $"select SizeCode, Description from SizeLink where Holder = '{activeTemplate.ProductSize.Number}'";
                activeTemplate.SizeLinks = SQL_Get_Generic_List.ExecuteQuery<SizeLinkItem>(DatabaseConnectionManager.Instance.ConnectionString, querySizeLink, null);
                activeTemplate.Markup = productInfo.Markup;
                activeTemplate.CostPriceExcl = productInfo.CostPriceExcl;

                string querySupplierProductLinks = $"select top 1 SupplierProductcode from supplierproductlinks where ProductCode = '{ParentCode}'";
                string supplierProductCode = SQL_Get_Generic_List.ExecuteSingle<string>(DatabaseConnectionManager.Instance.ConnectionString, querySupplierProductLinks, null);
                activeTemplate.SupplierCode = supplierProductCode;

                string querySupplierNumber = $"select top 1 CreditorNumber from supplierproductlinks where ProductCode = '{ParentCode}'";
                string supplierNumber = SQL_Get_Generic_List.ExecuteSingle<string>(DatabaseConnectionManager.Instance.ConnectionString, querySupplierNumber, null);
                string querySupplierDetails = $"select top 1 AccountNumber, AccountName from Supplier where AccountNumber = '{supplierNumber}'";
                activeTemplate.Supplier = SQL_Get_Generic_List.ExecuteSingle<Supplier>(DatabaseConnectionManager.Instance.ConnectionString, querySupplierDetails, null);


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

        private class ProductInfo
        {
            public string ParentCode { get; set; }
            public string Description { get; set; }
            public float UnitSize { get; set; }
            public float CostPriceExcl { get; set; }
            public float Markup { get; set; }
            public int ProductSize { get; set; }
            public int SizeLink { get; set; }
        }

        private void SupplierNumberComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedSupplier = suppliers[SupplierComboBox.SelectedIndex];
        }
    }
}