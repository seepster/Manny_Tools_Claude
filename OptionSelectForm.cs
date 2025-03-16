using System;
using System.Drawing;
using System.Windows.Forms;

namespace Manny_Tools_Claude
{
    public partial class OptionSelectForm : Form
    {
        private Label lblTitle;
        private Label lblPrompt;
        private ListBox lstOptions;
        private Button btnOK;
        private Button btnCancel;

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

        private void InitializeComponent()
        {
            this.Size = new Size(400, 350);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Create title
            lblTitle = new Label
            {
                Text = "Select Option",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(20, 20),
                Size = new Size(350, 30)
            };

            // Prompt text
            lblPrompt = new Label
            {
                Text = "Please select an option:",
                Location = new Point(20, 60),
                Size = new Size(350, 20)
            };

            // Options listbox
            lstOptions = new ListBox
            {
                Location = new Point(20, 90),
                Size = new Size(350, 160),
                BorderStyle = BorderStyle.FixedSingle
            };
            lstOptions.DoubleClick += LstOptions_DoubleClick;

            // Buttons
            btnOK = new Button
            {
                Text = "OK",
                Location = new Point(200, 270),
                Size = new Size(80, 30),
                DialogResult = DialogResult.OK
            };
            btnOK.Click += BtnOK_Click;

            btnCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(290, 270),
                Size = new Size(80, 30),
                DialogResult = DialogResult.Cancel
            };

            // Add controls to form
            this.Controls.Add(lblTitle);
            this.Controls.Add(lblPrompt);
            this.Controls.Add(lstOptions);
            this.Controls.Add(btnOK);
            this.Controls.Add(btnCancel);

            // Set default button
            this.AcceptButton = btnOK;
            this.CancelButton = btnCancel;
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