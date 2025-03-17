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

namespace Manny_Tools_Claude
{
    public partial class CreateSizesForm : UserControl
    {
        // Define fields to store form data
        private List<ProductSizeItem> productSizes = new List<ProductSizeItem>();
        private List<SizeLinkItem> sizeLinks = new List<SizeLinkItem>();
        private Dictionary<int, string> suppliers = new Dictionary<int, string>();

        // Class to hold ProductSize data
        private class ProductSizeItem
        {
            public string Number { get; set; }
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
            LoadDefaults();
        }

        public void LoadDefaults()
        {
            string queryForProductSize = "select Number, Description from ProductSize";
            List<ProductSizeItem>  productSizeItems= SQL_Get_Generic_List.ExecuteQuery<ProductSizeItem>(DatabaseConnectionManager.Instance.ConnectionString , queryForProductSize, null);

            foreach (ProductSizeItem item in productSizeItems)
            {
                ProductSizeComboBox.Items.Add(item.ToString());
            }
            
            
        }

    }
}