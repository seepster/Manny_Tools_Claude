using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;

namespace Manny_Tools_Claude
{
    public partial class CreateSizesForm : UserControl
    {
        #region Fields

        // Form controls
        private Label lblTitle;
        private ComboBox cmbProductType;
        private Label lblProductType;
        private ComboBox cmbSizeType;
        private Label lblSizeType;
        private Label lblSizeInfo;
        private TextBox txtBaseSize;
        private Label lblBaseSize;
        private NumericUpDown numIncrement;
        private Label lblIncrement;
        private NumericUpDown numQuantity;
        private Label lblQuantity;
        private Button btnGenerate;
        private DataGridView dgvSizes;
        private Button btnExport;

        // Data
        private List<SizeInfo> _generatedSizes;

        #endregion

        #region Constructor & Initialization

        public CreateSizesForm()
        {
            InitializeComponent();
            _generatedSizes = new List<SizeInfo>();
        }

        private void InitializeComponent()
        {
            this.BackColor = SystemColors.Control;

            // Create header panel
            Panel panelHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.FromArgb(240, 240, 240)
            };

            // Title
            lblTitle = new Label
            {
                Text = "Size Generator",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Location = new Point(10, 10),
                AutoSize = true
            };

            lblSizeInfo = new Label
            {
                Text = "Generate standard sizes based on product type and parameters",
                Location = new Point(10, 35),
                Size = new Size(950, 20),
                Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top
            };

            panelHeader.Controls.Add(lblTitle);
            panelHeader.Controls.Add(lblSizeInfo);

            // Create controls panel
            Panel panelControls = new Panel
            {
                Dock = DockStyle.Top,
                Height = 180,
                Padding = new Padding(10)
            };

            // Product Type
            lblProductType = new Label
            {
                Text = "Product Type:",
                Location = new Point(20, 20),
                AutoSize = true
            };

            cmbProductType = new ComboBox
            {
                Location = new Point(150, 18),
                Width = 200,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            // Populate product types
            cmbProductType.Items.AddRange(new object[] {
                "Apparel",
                "Footwear",
                "Accessories",
                "Furniture"
            });
            cmbProductType.SelectedIndex = 0;
            cmbProductType.SelectedIndexChanged += CmbProductType_SelectedIndexChanged;

            // Size Type
            lblSizeType = new Label
            {
                Text = "Size Type:",
                Location = new Point(20, 55),
                AutoSize = true
            };

            cmbSizeType = new ComboBox
            {
                Location = new Point(150, 53),
                Width = 200,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            // Base Size
            lblBaseSize = new Label
            {
                Text = "Base Size:",
                Location = new Point(20, 90),
                AutoSize = true
            };

            txtBaseSize = new TextBox
            {
                Location = new Point(150, 88),
                Width = 200
            };

            // Increment
            lblIncrement = new Label
            {
                Text = "Increment:",
                Location = new Point(400, 20),
                AutoSize = true
            };

            numIncrement = new NumericUpDown
            {
                Location = new Point(530, 18),
                Width = 200,
                Minimum = 0.1m,
                Maximum = 10m,
                DecimalPlaces = 1,
                Increment = 0.1m,
                Value = 1.0m
            };

            // Quantity
            lblQuantity = new Label
            {
                Text = "Quantity:",
                Location = new Point(400, 55),
                AutoSize = true
            };

            numQuantity = new NumericUpDown
            {
                Location = new Point(530, 53),
                Width = 200,
                Minimum = 1,
                Maximum = 100,
                Value = 10
            };

            // Generate Button
            btnGenerate = new Button
            {
                Text = "Generate Sizes",
                Location = new Point(530, 88),
                Width = 200,
                Height = 30
            };
            btnGenerate.Click += BtnGenerate_Click;

            // Add controls to panel
            panelControls.Controls.Add(lblProductType);
            panelControls.Controls.Add(cmbProductType);
            panelControls.Controls.Add(lblSizeType);
            panelControls.Controls.Add(cmbSizeType);
            panelControls.Controls.Add(lblBaseSize);
            panelControls.Controls.Add(txtBaseSize);
            panelControls.Controls.Add(lblIncrement);
            panelControls.Controls.Add(numIncrement);
            panelControls.Controls.Add(lblQuantity);
            panelControls.Controls.Add(numQuantity);
            panelControls.Controls.Add(btnGenerate);

            // Create data panel
            Panel panelData = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(10)
            };

            // DataGridView for sizes
            dgvSizes = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                RowHeadersVisible = false,
                AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.AliceBlue },
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = true
            };

            // Export Button
            btnExport = new Button
            {
                Text = "Export Sizes",
                Dock = DockStyle.Bottom,
                Height = 30,
                Margin = new Padding(0, 10, 0, 0)
            };
            btnExport.Click += BtnExport_Click;

            // Add controls to data panel
            panelData.Controls.Add(dgvSizes);
            panelData.Controls.Add(btnExport);

            // Add panels to main control
            this.Controls.Add(panelData);
            this.Controls.Add(panelControls);
            this.Controls.Add(panelHeader);

            // Initialize size types
            PopulateSizeTypes("Apparel");
        }

        #endregion

        #region Event Handlers

        private void CmbProductType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedProductType = cmbProductType.SelectedItem.ToString();
            PopulateSizeTypes(selectedProductType);
        }

        private void BtnGenerate_Click(object sender, EventArgs e)
        {
            GenerateSizes();
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            ExportSizes();
        }

        #endregion

        #region Size Generation Logic

        private void PopulateSizeTypes(string productType)
        {
            cmbSizeType.Items.Clear();

            switch (productType)
            {
                case "Apparel":
                    cmbSizeType.Items.AddRange(new object[] {
                        "US Standard (S,M,L,XL)",
                        "US Numeric (2,4,6,8)",
                        "European (36,38,40,42)",
                        "UK (8,10,12,14)"
                    });
                    break;

                case "Footwear":
                    cmbSizeType.Items.AddRange(new object[] {
                        "US Men's",
                        "US Women's",
                        "European",
                        "UK"
                    });
                    break;

                case "Accessories":
                    cmbSizeType.Items.AddRange(new object[] {
                        "Numeric (S,M,L)",
                        "Inches",
                        "Centimeters"
                    });
                    break;

                case "Furniture":
                    cmbSizeType.Items.AddRange(new object[] {
                        "Inches",
                        "Centimeters",
                        "Custom"
                    });
                    break;
            }

            if (cmbSizeType.Items.Count > 0)
            {
                cmbSizeType.SelectedIndex = 0;
                SetDefaultBaseSize();
            }
        }

        private void SetDefaultBaseSize()
        {
            string productType = cmbProductType.SelectedItem.ToString();
            string sizeType = cmbSizeType.SelectedItem.ToString();

            switch (productType)
            {
                case "Apparel":
                    if (sizeType == "US Standard (S,M,L,XL)")
                        txtBaseSize.Text = "S";
                    else if (sizeType == "US Numeric (2,4,6,8)")
                        txtBaseSize.Text = "2";
                    else if (sizeType == "European (36,38,40,42)")
                        txtBaseSize.Text = "36";
                    else if (sizeType == "UK (8,10,12,14)")
                        txtBaseSize.Text = "8";
                    break;

                case "Footwear":
                    if (sizeType == "US Men's")
                        txtBaseSize.Text = "7";
                    else if (sizeType == "US Women's")
                        txtBaseSize.Text = "5";
                    else if (sizeType == "European")
                        txtBaseSize.Text = "38";
                    else if (sizeType == "UK")
                        txtBaseSize.Text = "6";
                    break;

                case "Accessories":
                    if (sizeType == "Numeric (S,M,L)")
                        txtBaseSize.Text = "S";
                    else if (sizeType == "Inches")
                        txtBaseSize.Text = "24";
                    else if (sizeType == "Centimeters")
                        txtBaseSize.Text = "60";
                    break;

                case "Furniture":
                    if (sizeType == "Inches")
                        txtBaseSize.Text = "36";
                    else if (sizeType == "Centimeters")
                        txtBaseSize.Text = "90";
                    else if (sizeType == "Custom")
                        txtBaseSize.Text = "Small";
                    break;
            }
        }

        private void GenerateSizes()
        {
            try
            {
                string productType = cmbProductType.SelectedItem.ToString();
                string sizeType = cmbSizeType.SelectedItem.ToString();
                string baseSize = txtBaseSize.Text.Trim();
                decimal increment = numIncrement.Value;
                int quantity = (int)numQuantity.Value;

                if (string.IsNullOrEmpty(baseSize))
                {
                    MessageBox.Show("Please enter a base size.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                _generatedSizes = new List<SizeInfo>();

                // Generate sizes based on product type and size type
                if (productType == "Apparel" && sizeType == "US Standard (S,M,L,XL)")
                {
                    string[] standardSizes = { "XS", "S", "M", "L", "XL", "2XL", "3XL", "4XL" };
                    int startIndex = Array.IndexOf(standardSizes, baseSize);

                    if (startIndex < 0)
                    {
                        MessageBox.Show("Invalid base size for US Standard sizes. Please use XS, S, M, L, XL, 2XL, 3XL, or 4XL.",
                            "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    for (int i = 0; i < quantity && (startIndex + i) < standardSizes.Length; i++)
                    {
                        _generatedSizes.Add(new SizeInfo
                        {
                            SizeCode = standardSizes[startIndex + i],
                            Description = GetSizeDescription(productType, sizeType, standardSizes[startIndex + i]),
                            SortOrder = i + 1
                        });
                    }
                }
                else if (decimal.TryParse(baseSize, out decimal numericBaseSize))
                {
                    // For numeric sizes
                    for (int i = 0; i < quantity; i++)
                    {
                        decimal currentSize = numericBaseSize + (i * increment);
                        string sizeCode = currentSize.ToString();

                        // Format size code appropriately
                        if (currentSize == Math.Floor(currentSize))
                            sizeCode = Math.Floor(currentSize).ToString();

                        _generatedSizes.Add(new SizeInfo
                        {
                            SizeCode = sizeCode,
                            Description = GetSizeDescription(productType, sizeType, sizeCode),
                            SortOrder = i + 1
                        });
                    }
                }
                else
                {
                    // For custom text-based sizes
                    _generatedSizes.Add(new SizeInfo
                    {
                        SizeCode = baseSize,
                        Description = GetSizeDescription(productType, sizeType, baseSize),
                        SortOrder = 1
                    });

                    switch (baseSize.ToUpper())
                    {
                        case "S":
                            if (quantity > 1) _generatedSizes.Add(new SizeInfo { SizeCode = "M", Description = GetSizeDescription(productType, sizeType, "M"), SortOrder = 2 });
                            if (quantity > 2) _generatedSizes.Add(new SizeInfo { SizeCode = "L", Description = GetSizeDescription(productType, sizeType, "L"), SortOrder = 3 });
                            if (quantity > 3) _generatedSizes.Add(new SizeInfo { SizeCode = "XL", Description = GetSizeDescription(productType, sizeType, "XL"), SortOrder = 4 });
                            break;

                        case "SMALL":
                            if (quantity > 1) _generatedSizes.Add(new SizeInfo { SizeCode = "Medium", Description = GetSizeDescription(productType, sizeType, "Medium"), SortOrder = 2 });
                            if (quantity > 2) _generatedSizes.Add(new SizeInfo { SizeCode = "Large", Description = GetSizeDescription(productType, sizeType, "Large"), SortOrder = 3 });
                            if (quantity > 3) _generatedSizes.Add(new SizeInfo { SizeCode = "X-Large", Description = GetSizeDescription(productType, sizeType, "X-Large"), SortOrder = 4 });
                            break;

                        default:
                            MessageBox.Show("For text-based sizes, please start with a standard size like 'S' or 'Small'.",
                                "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                    }
                }

                // Display generated sizes in the grid
                DisplaySizes();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating sizes: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetSizeDescription(string productType, string sizeType, string sizeCode)
        {
            switch (productType)
            {
                case "Apparel":
                    if (sizeType == "US Standard (S,M,L,XL)")
                    {
                        switch (sizeCode)
                        {
                            case "XS": return "Extra Small";
                            case "S": return "Small";
                            case "M": return "Medium";
                            case "L": return "Large";
                            case "XL": return "Extra Large";
                            case "2XL": return "2X Large";
                            case "3XL": return "3X Large";
                            case "4XL": return "4X Large";
                            default: return sizeCode;
                        }
                    }
                    else if (sizeType == "US Numeric (2,4,6,8)")
                    {
                        return $"US Size {sizeCode}";
                    }
                    else if (sizeType == "European (36,38,40,42)")
                    {
                        return $"EU Size {sizeCode}";
                    }
                    else if (sizeType == "UK (8,10,12,14)")
                    {
                        return $"UK Size {sizeCode}";
                    }
                    break;

                case "Footwear":
                    if (sizeType == "US Men's")
                    {
                        return $"US Men's Size {sizeCode}";
                    }
                    else if (sizeType == "US Women's")
                    {
                        return $"US Women's Size {sizeCode}";
                    }
                    else if (sizeType == "European")
                    {
                        return $"EU Size {sizeCode}";
                    }
                    else if (sizeType == "UK")
                    {
                        return $"UK Size {sizeCode}";
                    }
                    break;

                case "Accessories":
                    if (sizeType == "Numeric (S,M,L)")
                    {
                        switch (sizeCode)
                        {
                            case "S": return "Small";
                            case "M": return "Medium";
                            case "L": return "Large";
                            default: return sizeCode;
                        }
                    }
                    else if (sizeType == "Inches")
                    {
                        return $"{sizeCode}\"";
                    }
                    else if (sizeType == "Centimeters")
                    {
                        return $"{sizeCode} cm";
                    }
                    break;

                case "Furniture":
                    if (sizeType == "Inches")
                    {
                        return $"{sizeCode}\"";
                    }
                    else if (sizeType == "Centimeters")
                    {
                        return $"{sizeCode} cm";
                    }
                    break;
            }

            return sizeCode;
        }

        private void DisplaySizes()
        {
            // Create a DataTable for binding to the grid
            var bindingSource = new BindingSource();
            bindingSource.DataSource = _generatedSizes;

            dgvSizes.DataSource = bindingSource;

            // Format columns
            if (dgvSizes.Columns.Count > 0)
            {
                dgvSizes.Columns["SortOrder"].HeaderText = "Sequence";
                dgvSizes.Columns["SizeCode"].HeaderText = "Size Code";
                dgvSizes.Columns["Description"].HeaderText = "Description";

                dgvSizes.Columns["SortOrder"].Width = 80;
                dgvSizes.Columns["SizeCode"].Width = 100;
                dgvSizes.Columns["Description"].Width = 250;
            }
        }

        private void ExportSizes()
        {
            if (_generatedSizes.Count == 0)
            {
                MessageBox.Show("Please generate sizes first.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            SaveFileDialog saveDialog = new SaveFileDialog
            {
                Filter = "CSV Files (*.csv)|*.csv|Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
                DefaultExt = "csv",
                FileName = $"Sizes_{cmbProductType.Text}_{DateTime.Now:yyyyMMdd}.csv"
            };

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (System.IO.StreamWriter writer = new System.IO.StreamWriter(saveDialog.FileName))
                    {
                        // Write header
                        writer.WriteLine("Sequence,SizeCode,Description");

                        // Write data
                        foreach (var size in _generatedSizes)
                        {
                            writer.WriteLine($"{size.SortOrder},{size.SizeCode},{size.Description}");
                        }
                    }

                    MessageBox.Show($"Size data exported successfully to {saveDialog.FileName}",
                        "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error exporting data: {ex.Message}", "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        #endregion

        #region Helper Classes

        public class SizeInfo
        {
            public string SizeCode { get; set; }
            public string Description { get; set; }
            public int SortOrder { get; set; }
        }

        #endregion
    }
}