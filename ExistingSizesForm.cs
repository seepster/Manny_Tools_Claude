using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Manny_Tools_Claude
{
    public partial class ExistingSizesForm : Form
    {
        public string ParentCode { get; private set; }
        public List<string> ExistingSizes { get; private set; }

        public ExistingSizesForm(string parentCode, List<string> existingSizes)
        {
            InitializeComponent();

            ParentCode = parentCode;
            ExistingSizes = existingSizes;

            // Set form title
            this.Text = $"Existing Sizes for Parent Code {parentCode}";
            lblTitle.Text = $"Existing Sizes for Parent Code {parentCode}";

            // Load existing sizes
            LoadExistingSizes();
        }

        private void LoadExistingSizes()
        {
            // Clear the list
            listExistingSizes.Items.Clear();

            // Add existing sizes to the list
            if (ExistingSizes.Count > 0)
            {
                foreach (string size in ExistingSizes)
                {
                    listExistingSizes.Items.Add(size);
                }

                lblNoSizes.Visible = false;
            }
            else
            {
                lblNoSizes.Visible = true;
            }
        }

        private void btnContinue_Click(object sender, EventArgs e)
        {
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