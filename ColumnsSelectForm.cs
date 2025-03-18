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
        private Dictionary<int, CheckBox> _checkboxes = new Dictionary<int, CheckBox>();

        public List<int> SelectedColumns => _selectedColumns;

        public ColumnSelectForm(List<int> currentColumns, Dictionary<int, string> columnMap)
        {
            _selectedColumns = new List<int>(currentColumns);
            _columnMap = columnMap;

            InitializeComponent();
            LoadColumnCheckboxes();
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