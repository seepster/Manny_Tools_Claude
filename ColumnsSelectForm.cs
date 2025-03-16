using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Linq;

namespace Manny_Tools_Claude
{
    public partial class ColumnSelectForm : Form
    {
        private List<int> _selectedColumns;
        private Dictionary<int, string> _columnMap;

        // Form controls
        private Label lblTitle;
        private Button btnSave;
        private Button btnCancel;
        private FlowLayoutPanel panelCheckboxes;
        private Dictionary<int, CheckBox> _checkboxes = new Dictionary<int, CheckBox>();

        public List<int> SelectedColumns => _selectedColumns;

        public ColumnSelectForm(List<int> currentColumns, Dictionary<int, string> columnMap)
        {
            _selectedColumns = new List<int>(currentColumns);
            _columnMap = columnMap;

            InitializeComponent();
            LoadColumnCheckboxes();
        }

        private void InitializeComponent()
        {
            this.Text = "Select Visible Columns";
            this.Size = new Size(400, 500);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = SystemColors.Control;

            // Create title label
            lblTitle = new Label
            {
                Text = "Select Columns to Display",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Dock = DockStyle.Top,
                TextAlign = ContentAlignment.MiddleCenter,
                Height = 40
            };

            // Create panel for checkboxes
            panelCheckboxes = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                AutoScroll = true,
                Padding = new Padding(20, 10, 20, 10)
            };

            // Create save button
            btnSave = new Button
            {
                Text = "Save",
                Dock = DockStyle.Bottom,
                Height = 40
            };
            btnSave.Click += BtnSave_Click;

            // Create cancel button
            btnCancel = new Button
            {
                Text = "Cancel",
                Dock = DockStyle.Bottom,
                Height = 30
            };
            btnCancel.Click += BtnCancel_Click;

            // Create layout container
            Panel panelButtons = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 80,
                Padding = new Padding(10)
            };

            // Add buttons to panel
            btnSave.Location = new Point(panelButtons.Width / 2 - btnSave.Width - 5, 10);
            btnSave.Size = new Size(100, 35);
            btnSave.Anchor = AnchorStyles.None;

            btnCancel.Location = new Point(panelButtons.Width / 2 + 5, 10);
            btnCancel.Size = new Size(100, 35);
            btnCancel.Anchor = AnchorStyles.None;

            panelButtons.Controls.Add(btnSave);
            panelButtons.Controls.Add(btnCancel);

            // Add controls to form
            this.Controls.Add(panelCheckboxes);
            this.Controls.Add(panelButtons);
            this.Controls.Add(lblTitle);

            // Set button positions
            panelButtons.Resize += (s, e) => {
                btnSave.Location = new Point(panelButtons.Width / 2 - btnSave.Width - 5, 10);
                btnCancel.Location = new Point(panelButtons.Width / 2 + 5, 10);
            };
        }

        private void LoadColumnCheckboxes()
        {
            // Clear existing checkboxes
            panelCheckboxes.Controls.Clear();
            _checkboxes.Clear();

            // Sort columns by ID to ensure consistent order
            var orderedColumns = _columnMap.OrderBy(pair => pair.Key).ToList();

            // Create a checkbox for each column
            foreach (var column in orderedColumns)
            {
                CheckBox checkbox = new CheckBox
                {
                    Text = column.Value,
                    Tag = column.Key,
                    AutoSize = true,
                    Margin = new Padding(10, 5, 10, 5),
                    Checked = _selectedColumns.Contains(column.Key)
                };

                _checkboxes.Add(column.Key, checkbox);
                panelCheckboxes.Controls.Add(checkbox);
            }

            // Add "Select All" / "Deselect All" buttons
            Button btnSelectAll = new Button
            {
                Text = "Select All",
                Size = new Size(100, 30),
                Margin = new Padding(10, 15, 10, 3)
            };
            btnSelectAll.Click += (s, e) => {
                foreach (CheckBox cb in _checkboxes.Values)
                {
                    cb.Checked = true;
                }
            };

            Button btnDeselectAll = new Button
            {
                Text = "Deselect All",
                Size = new Size(100, 30),
                Margin = new Padding(10, 3, 10, 15)
            };
            btnDeselectAll.Click += (s, e) => {
                foreach (CheckBox cb in _checkboxes.Values)
                {
                    cb.Checked = false;
                }
            };

            panelCheckboxes.Controls.Add(btnSelectAll);
            panelCheckboxes.Controls.Add(btnDeselectAll);
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            // Check if at least one column is selected
            bool anyChecked = _checkboxes.Values.Any(cb => cb.Checked);
            if (!anyChecked)
            {
                MessageBox.Show("Please select at least one column to display.",
                    "Column Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Update selected columns
            _selectedColumns.Clear();
            foreach (var checkbox in _checkboxes)
            {
                if (checkbox.Value.Checked)
                {
                    _selectedColumns.Add(checkbox.Key);
                }
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}