namespace Manny_Tools_Claude
{
    partial class CreateSizesForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            topPanel = new Panel();
            CreateCSVFileButton = new Button();
            NextAvailableParentCodeButton = new Button();
            ClearTemplateButton = new Button();
            LoadTemplateButton = new Button();
            Product_ParentCodeTextBox = new TextBox();
            ParentProductCodeButton = new Button();
            leftPanel = new Panel();
            SizeLinksLabel = new Label();
            ProductSizeLabel = new Label();
            SelectAllCheckBox = new CheckBox();
            SizeLinksListBox = new CheckedListBox();
            ProductSizeComboBox = new ComboBox();
            rightPanel = new Panel();
            SupplierComboBox = new ComboBox();
            SupplierNumberLabel = new Label();
            SupplierCodeTextBox = new TextBox();
            SupplierCodeLabel = new Label();
            CostPriceExclTextBox = new TextBox();
            CostPriceExclLabel = new Label();
            MarkupTextBox = new TextBox();
            MarkupLabel = new Label();
            SizeSuffixTextBox = new TextBox();
            SizeSuffixLabel = new Label();
            DescriptionTextBox = new TextBox();
            DescriptionLabel = new Label();
            topPanel.SuspendLayout();
            leftPanel.SuspendLayout();
            rightPanel.SuspendLayout();
            SuspendLayout();
            // 
            // topPanel
            // 
            topPanel.Controls.Add(CreateCSVFileButton);
            topPanel.Controls.Add(NextAvailableParentCodeButton);
            topPanel.Controls.Add(ClearTemplateButton);
            topPanel.Controls.Add(LoadTemplateButton);
            topPanel.Controls.Add(Product_ParentCodeTextBox);
            topPanel.Controls.Add(ParentProductCodeButton);
            topPanel.Dock = DockStyle.Top;
            topPanel.Location = new Point(0, 0);
            topPanel.Name = "topPanel";
            topPanel.Size = new Size(800, 100);
            topPanel.TabIndex = 0;
            // 
            // CreateCSVFileButton
            // 
            CreateCSVFileButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            CreateCSVFileButton.Location = new Point(673, 18);
            CreateCSVFileButton.Name = "CreateCSVFileButton";
            CreateCSVFileButton.Size = new Size(124, 32);
            CreateCSVFileButton.TabIndex = 5;
            CreateCSVFileButton.Text = "Create CSV File";
            CreateCSVFileButton.UseVisualStyleBackColor = true;
            CreateCSVFileButton.Click += CreateCSVFileButton_Click;
            // 
            // NextAvailableParentCodeButton
            // 
            NextAvailableParentCodeButton.Location = new Point(273, 69);
            NextAvailableParentCodeButton.Name = "NextAvailableParentCodeButton";
            NextAvailableParentCodeButton.Size = new Size(203, 23);
            NextAvailableParentCodeButton.TabIndex = 4;
            NextAvailableParentCodeButton.Text = "Next available Parent Code";
            NextAvailableParentCodeButton.UseVisualStyleBackColor = true;
            NextAvailableParentCodeButton.Click += NextAvailableParentCodeButton_Click;
            // 
            // ClearTemplateButton
            // 
            ClearTemplateButton.Location = new Point(165, 69);
            ClearTemplateButton.Name = "ClearTemplateButton";
            ClearTemplateButton.Size = new Size(102, 23);
            ClearTemplateButton.TabIndex = 3;
            ClearTemplateButton.Text = "Clear Template";
            ClearTemplateButton.UseVisualStyleBackColor = true;
            ClearTemplateButton.Click += ClearTemplateButton_Click;
            // 
            // LoadTemplateButton
            // 
            LoadTemplateButton.Location = new Point(58, 69);
            LoadTemplateButton.Name = "LoadTemplateButton";
            LoadTemplateButton.Size = new Size(101, 23);
            LoadTemplateButton.TabIndex = 2;
            LoadTemplateButton.Text = "Load Template";
            LoadTemplateButton.UseVisualStyleBackColor = true;
            LoadTemplateButton.Click += LoadTemplateButton_Click;
            // 
            // Product_ParentCodeTextBox
            // 
            Product_ParentCodeTextBox.Location = new Point(165, 18);
            Product_ParentCodeTextBox.Name = "Product_ParentCodeTextBox";
            Product_ParentCodeTextBox.Size = new Size(311, 23);
            Product_ParentCodeTextBox.TabIndex = 1;
            // 
            // ParentProductCodeButton
            // 
            ParentProductCodeButton.Location = new Point(12, 12);
            ParentProductCodeButton.Name = "ParentProductCodeButton";
            ParentProductCodeButton.Size = new Size(147, 32);
            ParentProductCodeButton.TabIndex = 0;
            ParentProductCodeButton.Text = "Parent/Product Code";
            ParentProductCodeButton.UseVisualStyleBackColor = true;
            // 
            // leftPanel
            // 
            leftPanel.Controls.Add(SizeLinksLabel);
            leftPanel.Controls.Add(ProductSizeLabel);
            leftPanel.Controls.Add(SelectAllCheckBox);
            leftPanel.Controls.Add(SizeLinksListBox);
            leftPanel.Controls.Add(ProductSizeComboBox);
            leftPanel.Dock = DockStyle.Left;
            leftPanel.Location = new Point(0, 100);
            leftPanel.Name = "leftPanel";
            leftPanel.Size = new Size(272, 350);
            leftPanel.TabIndex = 1;
            // 
            // SizeLinksLabel
            // 
            SizeLinksLabel.AutoSize = true;
            SizeLinksLabel.Location = new Point(12, 94);
            SizeLinksLabel.Name = "SizeLinksLabel";
            SizeLinksLabel.Size = new Size(57, 15);
            SizeLinksLabel.TabIndex = 5;
            SizeLinksLabel.Text = "Size Links";
            // 
            // ProductSizeLabel
            // 
            ProductSizeLabel.AutoSize = true;
            ProductSizeLabel.Location = new Point(12, 20);
            ProductSizeLabel.Name = "ProductSizeLabel";
            ProductSizeLabel.Size = new Size(72, 15);
            ProductSizeLabel.TabIndex = 4;
            ProductSizeLabel.Text = "Product Size";
            // 
            // SelectAllCheckBox
            // 
            SelectAllCheckBox.AutoSize = true;
            SelectAllCheckBox.Location = new Point(12, 299);
            SelectAllCheckBox.Name = "SelectAllCheckBox";
            SelectAllCheckBox.Size = new Size(74, 19);
            SelectAllCheckBox.TabIndex = 2;
            SelectAllCheckBox.Text = "Select All";
            SelectAllCheckBox.UseVisualStyleBackColor = true;
            SelectAllCheckBox.CheckedChanged += SelectAllCheckBox_CheckedChanged;
            // 
            // SizeLinksListBox
            // 
            SizeLinksListBox.FormattingEnabled = true;
            SizeLinksListBox.Location = new Point(12, 112);
            SizeLinksListBox.Name = "SizeLinksListBox";
            SizeLinksListBox.Size = new Size(241, 166);
            SizeLinksListBox.TabIndex = 1;
            // 
            // ProductSizeComboBox
            // 
            ProductSizeComboBox.FormattingEnabled = true;
            ProductSizeComboBox.Location = new Point(12, 38);
            ProductSizeComboBox.Name = "ProductSizeComboBox";
            ProductSizeComboBox.Size = new Size(241, 23);
            ProductSizeComboBox.TabIndex = 0;
            ProductSizeComboBox.SelectedIndexChanged += ProductSizeComboBox_SelectedIndexChanged;
            // 
            // rightPanel
            // 
            rightPanel.Controls.Add(SupplierComboBox);
            rightPanel.Controls.Add(SupplierNumberLabel);
            rightPanel.Controls.Add(SupplierCodeTextBox);
            rightPanel.Controls.Add(SupplierCodeLabel);
            rightPanel.Controls.Add(CostPriceExclTextBox);
            rightPanel.Controls.Add(CostPriceExclLabel);
            rightPanel.Controls.Add(MarkupTextBox);
            rightPanel.Controls.Add(MarkupLabel);
            rightPanel.Controls.Add(SizeSuffixTextBox);
            rightPanel.Controls.Add(SizeSuffixLabel);
            rightPanel.Controls.Add(DescriptionTextBox);
            rightPanel.Controls.Add(DescriptionLabel);
            rightPanel.Dock = DockStyle.Fill;
            rightPanel.Location = new Point(272, 100);
            rightPanel.Name = "rightPanel";
            rightPanel.Size = new Size(528, 350);
            rightPanel.TabIndex = 2;
            // 
            // SupplierComboBox
            // 
            SupplierComboBox.FormattingEnabled = true;
            SupplierComboBox.Location = new Point(181, 237);
            SupplierComboBox.Name = "SupplierComboBox";
            SupplierComboBox.Size = new Size(311, 23);
            SupplierComboBox.TabIndex = 11;
            SupplierComboBox.SelectedIndexChanged += SupplierNumberComboBox_SelectedIndexChanged;
            // 
            // SupplierNumberLabel
            // 
            SupplierNumberLabel.AutoSize = true;
            SupplierNumberLabel.Location = new Point(22, 240);
            SupplierNumberLabel.Name = "SupplierNumberLabel";
            SupplierNumberLabel.Size = new Size(50, 15);
            SupplierNumberLabel.TabIndex = 10;
            SupplierNumberLabel.Text = "Supplier";
            // 
            // SupplierCodeTextBox
            // 
            SupplierCodeTextBox.Location = new Point(181, 190);
            SupplierCodeTextBox.Name = "SupplierCodeTextBox";
            SupplierCodeTextBox.Size = new Size(311, 23);
            SupplierCodeTextBox.TabIndex = 9;
            // 
            // SupplierCodeLabel
            // 
            SupplierCodeLabel.AutoSize = true;
            SupplierCodeLabel.Location = new Point(22, 193);
            SupplierCodeLabel.Name = "SupplierCodeLabel";
            SupplierCodeLabel.Size = new Size(81, 15);
            SupplierCodeLabel.TabIndex = 8;
            SupplierCodeLabel.Text = "Supplier Code";
            // 
            // CostPriceExclTextBox
            // 
            CostPriceExclTextBox.Location = new Point(181, 143);
            CostPriceExclTextBox.Name = "CostPriceExclTextBox";
            CostPriceExclTextBox.Size = new Size(311, 23);
            CostPriceExclTextBox.TabIndex = 7;
            // 
            // CostPriceExclLabel
            // 
            CostPriceExclLabel.AutoSize = true;
            CostPriceExclLabel.Location = new Point(22, 146);
            CostPriceExclLabel.Name = "CostPriceExclLabel";
            CostPriceExclLabel.Size = new Size(77, 15);
            CostPriceExclLabel.TabIndex = 6;
            CostPriceExclLabel.Text = "CostPriceExcl";
            // 
            // MarkupTextBox
            // 
            MarkupTextBox.Location = new Point(181, 96);
            MarkupTextBox.Name = "MarkupTextBox";
            MarkupTextBox.Size = new Size(311, 23);
            MarkupTextBox.TabIndex = 5;
            // 
            // MarkupLabel
            // 
            MarkupLabel.AutoSize = true;
            MarkupLabel.Location = new Point(22, 99);
            MarkupLabel.Name = "MarkupLabel";
            MarkupLabel.Size = new Size(48, 15);
            MarkupLabel.TabIndex = 4;
            MarkupLabel.Text = "Markup";
            // 
            // SizeSuffixTextBox
            // 
            SizeSuffixTextBox.Location = new Point(181, 49);
            SizeSuffixTextBox.Name = "SizeSuffixTextBox";
            SizeSuffixTextBox.ReadOnly = true;
            SizeSuffixTextBox.Size = new Size(311, 23);
            SizeSuffixTextBox.TabIndex = 3;
            // 
            // SizeSuffixLabel
            // 
            SizeSuffixLabel.AutoSize = true;
            SizeSuffixLabel.Location = new Point(22, 52);
            SizeSuffixLabel.Name = "SizeSuffixLabel";
            SizeSuffixLabel.Size = new Size(59, 15);
            SizeSuffixLabel.TabIndex = 2;
            SizeSuffixLabel.Text = "Size Suffix";
            // 
            // DescriptionTextBox
            // 
            DescriptionTextBox.Location = new Point(181, 6);
            DescriptionTextBox.Name = "DescriptionTextBox";
            DescriptionTextBox.Size = new Size(311, 23);
            DescriptionTextBox.TabIndex = 1;
            // 
            // DescriptionLabel
            // 
            DescriptionLabel.AutoSize = true;
            DescriptionLabel.Location = new Point(22, 9);
            DescriptionLabel.Name = "DescriptionLabel";
            DescriptionLabel.Size = new Size(67, 15);
            DescriptionLabel.TabIndex = 0;
            DescriptionLabel.Text = "Description";
            // 
            // CreateSizesForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(rightPanel);
            Controls.Add(leftPanel);
            Controls.Add(topPanel);
            Name = "CreateSizesForm";
            Size = new Size(800, 450);
            topPanel.ResumeLayout(false);
            topPanel.PerformLayout();
            leftPanel.ResumeLayout(false);
            leftPanel.PerformLayout();
            rightPanel.ResumeLayout(false);
            rightPanel.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel topPanel;
        private System.Windows.Forms.Panel leftPanel;
        private System.Windows.Forms.Panel rightPanel;
        private System.Windows.Forms.Button ParentProductCodeButton;
        private System.Windows.Forms.TextBox Product_ParentCodeTextBox;
        private System.Windows.Forms.Button LoadTemplateButton;
        private System.Windows.Forms.Button ClearTemplateButton;
        private System.Windows.Forms.Button NextAvailableParentCodeButton;
        private System.Windows.Forms.Button CreateCSVFileButton;
        private System.Windows.Forms.ComboBox ProductSizeComboBox;
        private System.Windows.Forms.CheckedListBox SizeLinksListBox;
        private System.Windows.Forms.CheckBox SelectAllCheckBox;
        private System.Windows.Forms.Label DescriptionLabel;
        private System.Windows.Forms.TextBox DescriptionTextBox;
        private System.Windows.Forms.Label SizeSuffixLabel;
        private System.Windows.Forms.TextBox SizeSuffixTextBox;
        private System.Windows.Forms.Label MarkupLabel;
        private System.Windows.Forms.TextBox MarkupTextBox;
        private System.Windows.Forms.Label CostPriceExclLabel;
        private System.Windows.Forms.TextBox CostPriceExclTextBox;
        private System.Windows.Forms.Label SupplierCodeLabel;
        private System.Windows.Forms.TextBox SupplierCodeTextBox;
        private System.Windows.Forms.Label SupplierNumberLabel;
        private System.Windows.Forms.ComboBox SupplierComboBox;
        private System.Windows.Forms.Label ProductSizeLabel;
        private System.Windows.Forms.Label SizeLinksLabel;
    }
}