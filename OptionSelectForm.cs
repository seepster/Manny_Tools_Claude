using System;
using System.Drawing;
using System.Windows.Forms;

namespace Manny_Tools_Claude
{
    public partial class OptionSelectForm : Form
    {
        public int SelectedIndex { get; private set; }

        public OptionSelectForm(string title, string prompt, string[] options)
        {
            InitializeComponent();

            this.Text = title;
            lblTitle.Text = title;
            lblPrompt.Text = prompt;

            // Add options to the listbox
            lstOptions.Items.AddRange(options);
            if (lstOptions.Items.Count > 0)
                lstOptions.SelectedIndex = 0;

            SelectedIndex = -1;
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            if (lstOptions.SelectedIndex >= 0)
            {
                SelectedIndex = lstOptions.SelectedIndex;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Please select an option.", "Selection Required",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void LstOptions_DoubleClick(object sender, EventArgs e)
        {
            if (lstOptions.SelectedIndex >= 0)
            {
                SelectedIndex = lstOptions.SelectedIndex;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}