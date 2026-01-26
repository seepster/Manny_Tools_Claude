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
    public partial class SizingMethodForm : Form
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

        // Property to store the selected method
        public int SelectedMethod { get; private set; }

        public SizingMethodForm(
            string parentCode,
            bool isNewParentCode,
            string description,
            float markup,
            float costPriceExcl,
            string supplierCode,
            CreateSizesSupplierItem supplier,
            CreateSizesProductSizeItem productSize,
            string department)
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


            // Set form title based on whether we're creating a new parent code
            this.Text = IsNewParentCode
                ? $"Select Sizing Method for New Parent Code {ParentCode}"
                : $"Select Sizing Method for Existing Parent Code {ParentCode}";

            // Initialize the radio buttons
            InitializeRadioButtons();
        }

        private void InitializeRadioButtons()
        {
            // Select the first option by default
            radioButtonNumbered.Checked = true;
            SelectedMethod = 1;
        }

        private void radioButtonNumbered_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonNumbered.Checked)
            {
                SelectedMethod = 1;
            }
        }

        private void radioButtonDescriptive_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonDescriptive.Checked)
            {
                SelectedMethod = 2;
            }
        }

        private void radioButtonBaby_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonBaby.Checked)
            {
                SelectedMethod = 3;
            }
        }

        private void radioButtonDouble_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonDouble.Checked)
            {
                SelectedMethod = 4;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnContinue_Click(object sender, EventArgs e)
        {
            // Set the dialog result and close the form
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}