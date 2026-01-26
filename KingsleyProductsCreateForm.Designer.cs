namespace Manny_Tools_Claude
{
    partial class KingsleyProductsCreateForm
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
            if (disposing)
            {
                // Unsubscribe from connection changed event
                DatabaseConnectionManager.Instance.ConnectionChanged -= OnConnectionChanged;

                if (components != null)
                {
                    components.Dispose();
                }
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
            pnlHeader = new Panel();
            grpTemplate = new GroupBox();
            btnLoadTemplate = new Button();
            txtTemplateCode = new TextBox();
            lblTemplateCode = new Label();
            grpProductCode = new GroupBox();
            btnClearForm = new Button();
            btnNextAvailableCode = new Button();
            txtProductCode = new TextBox();
            lblProductCode = new Label();
            pnlBody = new Panel();
            grpProductDetails = new GroupBox();
            cmbMinorDepartment = new ComboBox();
            lblMinorDepartment = new Label();
            txtTaxValue = new TextBox();
            lblTaxValue = new Label();
            cmbSizeLink = new ComboBox();
            lblSizeLink = new Label();
            cmbProductSize = new ComboBox();
            lblProductSize = new Label();
            txtPackSize = new TextBox();
            lblPackSize = new Label();
            chkTillItem = new CheckBox();
            chkStockItem = new CheckBox();
            chkDiscontinued = new CheckBox();
            cmbDepartment = new ComboBox();
            lblDepartment = new Label();
            txtUnitOfMeasure = new TextBox();
            lblUnitOfMeasure = new Label();
            txtUnitSize = new TextBox();
            lblUnitSize = new Label();
            cmbBrandName = new ComboBox();
            lblBrandName = new Label();
            txtDescription = new TextBox();
            lblDescription = new Label();
            grpPricing = new GroupBox();
            txtSellingPriceExcl = new TextBox();
            lblSellingPriceExclLabel = new Label();
            txtSellingPrice5 = new TextBox();
            lblSellingPrice5 = new Label();
            txtSellingPrice4 = new TextBox();
            lblSellingPrice4 = new Label();
            txtSellingPrice3 = new TextBox();
            lblSellingPrice3 = new Label();
            txtSellingPrice2 = new TextBox();
            lblSellingPrice2 = new Label();
            txtMarkup = new TextBox();
            lblMarkup = new Label();
            txtCostPriceIncl = new TextBox();
            lblCostPriceIncl = new Label();
            txtCostPriceExcl = new TextBox();
            lblCostPriceExcl = new Label();
            txtSellingPriceIncl = new TextBox();
            lblSellingPriceIncl = new Label();
            grpSupplier = new GroupBox();
            txtSupplierCode = new TextBox();
            lblSupplierCode = new Label();
            cmbSupplier = new ComboBox();
            lblSupplier = new Label();
            pnlFooter = new Panel();
            grpActions = new GroupBox();
            btnReviewProducts = new Button();
            btnAddProduct = new Button();
            grpSessionProducts = new GroupBox();
            dgvSessionProducts = new DataGridView();
            btnRemoveProduct = new Button();
            pnlHeader.SuspendLayout();
            grpTemplate.SuspendLayout();
            grpProductCode.SuspendLayout();
            pnlBody.SuspendLayout();
            grpProductDetails.SuspendLayout();
            grpPricing.SuspendLayout();
            grpSupplier.SuspendLayout();
            pnlFooter.SuspendLayout();
            grpActions.SuspendLayout();
            grpSessionProducts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvSessionProducts).BeginInit();
            SuspendLayout();
            // 
            // pnlHeader
            //
            pnlHeader.Controls.Add(grpProductDetails);
            pnlHeader.Controls.Add(grpSupplier);
            pnlHeader.Controls.Add(grpPricing);
            pnlHeader.Controls.Add(grpTemplate);
            pnlHeader.Controls.Add(grpProductCode);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Margin = new Padding(4, 3, 4, 3);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1130, 459);
            pnlHeader.TabIndex = 0;
            // 
            // grpTemplate
            //
            grpTemplate.Controls.Add(btnLoadTemplate);
            grpTemplate.Controls.Add(txtTemplateCode);
            grpTemplate.Controls.Add(lblTemplateCode);
            grpTemplate.Location = new Point(12, 12);
            grpTemplate.Margin = new Padding(4, 3, 4, 3);
            grpTemplate.Name = "grpTemplate";
            grpTemplate.Padding = new Padding(4, 3, 4, 3);
            grpTemplate.Size = new Size(310, 92);
            grpTemplate.TabIndex = 0;
            grpTemplate.TabStop = false;
            grpTemplate.Text = "Template Loading";
            // 
            // btnLoadTemplate
            //
            btnLoadTemplate.Location = new Point(170, 46);
            btnLoadTemplate.Margin = new Padding(4, 3, 4, 3);
            btnLoadTemplate.Name = "btnLoadTemplate";
            btnLoadTemplate.Size = new Size(125, 27);
            btnLoadTemplate.TabIndex = 2;
            btnLoadTemplate.Text = "Load Template";
            btnLoadTemplate.UseVisualStyleBackColor = true;
            btnLoadTemplate.Click += btnLoadTemplate_Click;
            // 
            // txtTemplateCode
            //
            txtTemplateCode.Location = new Point(12, 48);
            txtTemplateCode.Margin = new Padding(4, 3, 4, 3);
            txtTemplateCode.Name = "txtTemplateCode";
            txtTemplateCode.Size = new Size(146, 23);
            txtTemplateCode.TabIndex = 1;
            // 
            // lblTemplateCode
            // 
            lblTemplateCode.AutoSize = true;
            lblTemplateCode.Location = new Point(12, 29);
            lblTemplateCode.Margin = new Padding(4, 0, 4, 0);
            lblTemplateCode.Name = "lblTemplateCode";
            lblTemplateCode.Size = new Size(126, 15);
            lblTemplateCode.TabIndex = 0;
            lblTemplateCode.Text = "Existing Product Code:";
            // 
            // grpProductCode
            //
            grpProductCode.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            grpProductCode.Controls.Add(btnClearForm);
            grpProductCode.Controls.Add(btnNextAvailableCode);
            grpProductCode.Controls.Add(txtProductCode);
            grpProductCode.Controls.Add(lblProductCode);
            grpProductCode.Location = new Point(330, 12);
            grpProductCode.Margin = new Padding(4, 3, 4, 3);
            grpProductCode.Name = "grpProductCode";
            grpProductCode.Padding = new Padding(4, 3, 4, 3);
            grpProductCode.Size = new Size(347, 92);
            grpProductCode.TabIndex = 1;
            grpProductCode.TabStop = false;
            grpProductCode.Text = "Product Code";
            // 
            // btnClearForm
            //
            btnClearForm.Location = new Point(245, 46);
            btnClearForm.Margin = new Padding(4, 3, 4, 3);
            btnClearForm.Name = "btnClearForm";
            btnClearForm.Size = new Size(90, 27);
            btnClearForm.TabIndex = 3;
            btnClearForm.Text = "Clear Form";
            btnClearForm.UseVisualStyleBackColor = true;
            btnClearForm.Click += btnClearForm_Click;
            //
            // btnNextAvailableCode
            //
            btnNextAvailableCode.Location = new Point(170, 46);
            btnNextAvailableCode.Margin = new Padding(4, 3, 4, 3);
            btnNextAvailableCode.Name = "btnNextAvailableCode";
            btnNextAvailableCode.Size = new Size(120, 27);
            btnNextAvailableCode.TabIndex = 2;
            btnNextAvailableCode.Text = "Next Available";
            btnNextAvailableCode.UseVisualStyleBackColor = true;
            btnNextAvailableCode.Click += btnNextAvailableCode_Click;
            // 
            // txtProductCode
            //
            txtProductCode.Location = new Point(12, 48);
            txtProductCode.Margin = new Padding(4, 3, 4, 3);
            txtProductCode.Name = "txtProductCode";
            txtProductCode.Size = new Size(146, 23);
            txtProductCode.TabIndex = 1;
            // 
            // lblProductCode
            // 
            lblProductCode.AutoSize = true;
            lblProductCode.Location = new Point(12, 29);
            lblProductCode.Margin = new Padding(4, 0, 4, 0);
            lblProductCode.Name = "lblProductCode";
            lblProductCode.Size = new Size(83, 15);
            lblProductCode.TabIndex = 0;
            lblProductCode.Text = "Product Code:";
            // 
            // pnlBody
            //
            pnlBody.AutoScroll = true;
            pnlBody.Dock = DockStyle.Fill;
            pnlBody.Location = new Point(0, 459);
            pnlBody.Margin = new Padding(4, 3, 4, 3);
            pnlBody.Name = "pnlBody";
            pnlBody.Size = new Size(1130, 118);
            pnlBody.TabIndex = 1;
            // 
            // grpProductDetails
            // 
            grpProductDetails.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            grpProductDetails.Controls.Add(txtTaxValue);
            grpProductDetails.Controls.Add(lblTaxValue);
            grpProductDetails.Controls.Add(cmbMinorDepartment);
            grpProductDetails.Controls.Add(lblMinorDepartment);
            grpProductDetails.Controls.Add(cmbSizeLink);
            grpProductDetails.Controls.Add(lblSizeLink);
            grpProductDetails.Controls.Add(cmbProductSize);
            grpProductDetails.Controls.Add(lblProductSize);
            grpProductDetails.Controls.Add(txtPackSize);
            grpProductDetails.Controls.Add(lblPackSize);
            grpProductDetails.Controls.Add(chkTillItem);
            grpProductDetails.Controls.Add(chkStockItem);
            grpProductDetails.Controls.Add(chkDiscontinued);
            grpProductDetails.Controls.Add(cmbDepartment);
            grpProductDetails.Controls.Add(lblDepartment);
            grpProductDetails.Controls.Add(txtUnitOfMeasure);
            grpProductDetails.Controls.Add(lblUnitOfMeasure);
            grpProductDetails.Controls.Add(txtUnitSize);
            grpProductDetails.Controls.Add(lblUnitSize);
            grpProductDetails.Controls.Add(cmbBrandName);
            grpProductDetails.Controls.Add(lblBrandName);
            grpProductDetails.Controls.Add(txtDescription);
            grpProductDetails.Controls.Add(lblDescription);
            grpProductDetails.Location = new Point(12, 112);
            grpProductDetails.Margin = new Padding(4, 3, 4, 3);
            grpProductDetails.Name = "grpProductDetails";
            grpProductDetails.Padding = new Padding(4, 3, 4, 3);
            grpProductDetails.Size = new Size(665, 339);
            grpProductDetails.TabIndex = 4;
            grpProductDetails.TabStop = false;
            grpProductDetails.Text = "Product Details";
            // 
            // cmbMinorDepartment
            // 
            cmbMinorDepartment.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbMinorDepartment.FormattingEnabled = true;
            cmbMinorDepartment.Location = new Point(117, 312);
            cmbMinorDepartment.Margin = new Padding(4, 3, 4, 3);
            cmbMinorDepartment.Name = "cmbMinorDepartment";
            cmbMinorDepartment.Size = new Size(174, 23);
            cmbMinorDepartment.TabIndex = 22;
            cmbMinorDepartment.SelectedIndexChanged += cmbMinorDepartment_SelectedIndexChanged;
            // 
            // lblMinorDepartment
            //
            lblMinorDepartment.AutoSize = true;
            lblMinorDepartment.Location = new Point(12, 315);
            lblMinorDepartment.Margin = new Padding(4, 0, 4, 0);
            lblMinorDepartment.Name = "lblMinorDepartment";
            lblMinorDepartment.Size = new Size(108, 15);
            lblMinorDepartment.TabIndex = 21;
            lblMinorDepartment.Text = "Minor Department:";
            //
            // txtTaxValue
            //
            txtTaxValue.Location = new Point(455, 312);
            txtTaxValue.Margin = new Padding(4, 3, 4, 3);
            txtTaxValue.Name = "txtTaxValue";
            txtTaxValue.Size = new Size(174, 23);
            txtTaxValue.TabIndex = 24;
            txtTaxValue.Text = "15";
            //
            // lblTaxValue
            //
            lblTaxValue.AutoSize = true;
            lblTaxValue.Location = new Point(338, 315);
            lblTaxValue.Margin = new Padding(4, 0, 4, 0);
            lblTaxValue.Name = "lblTaxValue";
            lblTaxValue.Size = new Size(98, 15);
            lblTaxValue.TabIndex = 23;
            lblTaxValue.Text = "Tax Value (%):";
            //
            // cmbSizeLink
            // 
            cmbSizeLink.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSizeLink.FormattingEnabled = true;
            cmbSizeLink.Location = new Point(455, 277);
            cmbSizeLink.Margin = new Padding(4, 3, 4, 3);
            cmbSizeLink.Name = "cmbSizeLink";
            cmbSizeLink.Size = new Size(174, 23);
            cmbSizeLink.TabIndex = 18;
            cmbSizeLink.SelectedIndexChanged += cmbSizeLink_SelectedIndexChanged;
            // 
            // lblSizeLink
            // 
            lblSizeLink.AutoSize = true;
            lblSizeLink.Location = new Point(338, 280);
            lblSizeLink.Margin = new Padding(4, 0, 4, 0);
            lblSizeLink.Name = "lblSizeLink";
            lblSizeLink.Size = new Size(55, 15);
            lblSizeLink.TabIndex = 17;
            lblSizeLink.Text = "Size Link:";
            // 
            // cmbProductSize
            // 
            cmbProductSize.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbProductSize.FormattingEnabled = true;
            cmbProductSize.Location = new Point(117, 277);
            cmbProductSize.Margin = new Padding(4, 3, 4, 3);
            cmbProductSize.Name = "cmbProductSize";
            cmbProductSize.Size = new Size(174, 23);
            cmbProductSize.TabIndex = 16;
            cmbProductSize.SelectedIndexChanged += cmbProductSize_SelectedIndexChanged;
            // 
            // lblProductSize
            // 
            lblProductSize.AutoSize = true;
            lblProductSize.Location = new Point(12, 280);
            lblProductSize.Margin = new Padding(4, 0, 4, 0);
            lblProductSize.Name = "lblProductSize";
            lblProductSize.Size = new Size(75, 15);
            lblProductSize.TabIndex = 15;
            lblProductSize.Text = "Product Size:";
            // 
            // txtPackSize
            // 
            txtPackSize.Location = new Point(455, 242);
            txtPackSize.Margin = new Padding(4, 3, 4, 3);
            txtPackSize.Name = "txtPackSize";
            txtPackSize.Size = new Size(174, 23);
            txtPackSize.TabIndex = 14;
            // 
            // lblPackSize
            // 
            lblPackSize.AutoSize = true;
            lblPackSize.Location = new Point(338, 246);
            lblPackSize.Margin = new Padding(4, 0, 4, 0);
            lblPackSize.Name = "lblPackSize";
            lblPackSize.Size = new Size(58, 15);
            lblPackSize.TabIndex = 13;
            lblPackSize.Text = "Pack Size:";
            // 
            // chkTillItem
            // 
            chkTillItem.AutoSize = true;
            chkTillItem.Checked = true;
            chkTillItem.CheckState = CheckState.Checked;
            chkTillItem.Location = new Point(210, 246);
            chkTillItem.Margin = new Padding(4, 3, 4, 3);
            chkTillItem.Name = "chkTillItem";
            chkTillItem.Size = new Size(69, 19);
            chkTillItem.TabIndex = 12;
            chkTillItem.Text = "Till Item";
            chkTillItem.UseVisualStyleBackColor = true;
            // 
            // chkStockItem
            // 
            chkStockItem.AutoSize = true;
            chkStockItem.Checked = true;
            chkStockItem.CheckState = CheckState.Checked;
            chkStockItem.Location = new Point(117, 246);
            chkStockItem.Margin = new Padding(4, 3, 4, 3);
            chkStockItem.Name = "chkStockItem";
            chkStockItem.Size = new Size(82, 19);
            chkStockItem.TabIndex = 11;
            chkStockItem.Text = "Stock Item";
            chkStockItem.UseVisualStyleBackColor = true;
            // 
            // chkDiscontinued
            // 
            chkDiscontinued.AutoSize = true;
            chkDiscontinued.Location = new Point(12, 246);
            chkDiscontinued.Margin = new Padding(4, 3, 4, 3);
            chkDiscontinued.Name = "chkDiscontinued";
            chkDiscontinued.Size = new Size(96, 19);
            chkDiscontinued.TabIndex = 10;
            chkDiscontinued.Text = "Discontinued";
            chkDiscontinued.UseVisualStyleBackColor = true;
            // 
            // cmbDepartment
            // 
            cmbDepartment.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbDepartment.FormattingEnabled = true;
            cmbDepartment.Location = new Point(117, 208);
            cmbDepartment.Margin = new Padding(4, 3, 4, 3);
            cmbDepartment.Name = "cmbDepartment";
            cmbDepartment.Size = new Size(513, 23);
            cmbDepartment.TabIndex = 9;
            cmbDepartment.SelectedIndexChanged += cmbDepartment_SelectedIndexChanged;
            // 
            // lblDepartment
            // 
            lblDepartment.AutoSize = true;
            lblDepartment.Location = new Point(12, 211);
            lblDepartment.Margin = new Padding(4, 0, 4, 0);
            lblDepartment.Name = "lblDepartment";
            lblDepartment.Size = new Size(73, 15);
            lblDepartment.TabIndex = 8;
            lblDepartment.Text = "Department:";
            // 
            // txtUnitOfMeasure
            // 
            txtUnitOfMeasure.Location = new Point(455, 173);
            txtUnitOfMeasure.Margin = new Padding(4, 3, 4, 3);
            txtUnitOfMeasure.Name = "txtUnitOfMeasure";
            txtUnitOfMeasure.Size = new Size(174, 23);
            txtUnitOfMeasure.TabIndex = 7;
            // 
            // lblUnitOfMeasure
            // 
            lblUnitOfMeasure.AutoSize = true;
            lblUnitOfMeasure.Location = new Point(338, 177);
            lblUnitOfMeasure.Margin = new Padding(4, 0, 4, 0);
            lblUnitOfMeasure.Name = "lblUnitOfMeasure";
            lblUnitOfMeasure.Size = new Size(94, 15);
            lblUnitOfMeasure.TabIndex = 6;
            lblUnitOfMeasure.Text = "Unit of Measure:";
            // 
            // txtUnitSize
            // 
            txtUnitSize.Location = new Point(117, 173);
            txtUnitSize.Margin = new Padding(4, 3, 4, 3);
            txtUnitSize.Name = "txtUnitSize";
            txtUnitSize.Size = new Size(174, 23);
            txtUnitSize.TabIndex = 5;
            // 
            // lblUnitSize
            // 
            lblUnitSize.AutoSize = true;
            lblUnitSize.Location = new Point(12, 177);
            lblUnitSize.Margin = new Padding(4, 0, 4, 0);
            lblUnitSize.Name = "lblUnitSize";
            lblUnitSize.Size = new Size(55, 15);
            lblUnitSize.TabIndex = 4;
            lblUnitSize.Text = "Unit Size:";
            // 
            // cmbBrandName
            // 
            cmbBrandName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            cmbBrandName.FormattingEnabled = true;
            cmbBrandName.Location = new Point(117, 138);
            cmbBrandName.Margin = new Padding(4, 3, 4, 3);
            cmbBrandName.Name = "cmbBrandName";
            cmbBrandName.Size = new Size(513, 23);
            cmbBrandName.TabIndex = 3;
            cmbBrandName.SelectedIndexChanged += cmbBrandName_SelectedIndexChanged;
            // 
            // lblBrandName
            // 
            lblBrandName.AutoSize = true;
            lblBrandName.Location = new Point(12, 142);
            lblBrandName.Margin = new Padding(4, 0, 4, 0);
            lblBrandName.Name = "lblBrandName";
            lblBrandName.Size = new Size(76, 15);
            lblBrandName.TabIndex = 2;
            lblBrandName.Text = "Brand Name:";
            // 
            // txtDescription
            // 
            txtDescription.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtDescription.Location = new Point(117, 29);
            txtDescription.Margin = new Padding(4, 3, 4, 3);
            txtDescription.Multiline = true;
            txtDescription.Name = "txtDescription";
            txtDescription.Size = new Size(513, 92);
            txtDescription.TabIndex = 1;
            // 
            // lblDescription
            // 
            lblDescription.AutoSize = true;
            lblDescription.Location = new Point(12, 32);
            lblDescription.Margin = new Padding(4, 0, 4, 0);
            lblDescription.Name = "lblDescription";
            lblDescription.Size = new Size(70, 15);
            lblDescription.TabIndex = 0;
            lblDescription.Text = "Description:";
            // 
            // grpPricing
            //
            grpPricing.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            grpPricing.Controls.Add(txtSellingPriceExcl);
            grpPricing.Controls.Add(lblSellingPriceExclLabel);
            grpPricing.Controls.Add(txtSellingPrice5);
            grpPricing.Controls.Add(lblSellingPrice5);
            grpPricing.Controls.Add(txtSellingPrice4);
            grpPricing.Controls.Add(lblSellingPrice4);
            grpPricing.Controls.Add(txtSellingPrice3);
            grpPricing.Controls.Add(lblSellingPrice3);
            grpPricing.Controls.Add(txtSellingPrice2);
            grpPricing.Controls.Add(lblSellingPrice2);
            grpPricing.Controls.Add(txtMarkup);
            grpPricing.Controls.Add(lblMarkup);
            grpPricing.Controls.Add(txtCostPriceIncl);
            grpPricing.Controls.Add(lblCostPriceIncl);
            grpPricing.Controls.Add(txtCostPriceExcl);
            grpPricing.Controls.Add(lblCostPriceExcl);
            grpPricing.Controls.Add(txtSellingPriceIncl);
            grpPricing.Controls.Add(lblSellingPriceIncl);
            grpPricing.Location = new Point(690, 12);
            grpPricing.Margin = new Padding(4, 3, 4, 3);
            grpPricing.Name = "grpPricing";
            grpPricing.Padding = new Padding(4, 3, 4, 3);
            grpPricing.Size = new Size(361, 323);
            grpPricing.TabIndex = 2;
            grpPricing.TabStop = false;
            grpPricing.Text = "Pricing Information";
            // 
            // txtSellingPriceExcl
            //
            txtSellingPriceExcl.Location = new Point(175, 94);
            txtSellingPriceExcl.Margin = new Padding(4, 3, 4, 3);
            txtSellingPriceExcl.Name = "txtSellingPriceExcl";
            txtSellingPriceExcl.Size = new Size(116, 23);
            txtSellingPriceExcl.TabIndex = 15;
            txtSellingPriceExcl.Text = "0.00";
            txtSellingPriceExcl.TextAlign = HorizontalAlignment.Left;
            // 
            // lblSellingPriceExclLabel
            // 
            lblSellingPriceExclLabel.AutoSize = true;
            lblSellingPriceExclLabel.Location = new Point(6, 94);
            lblSellingPriceExclLabel.Margin = new Padding(4, 0, 4, 0);
            lblSellingPriceExclLabel.Name = "lblSellingPriceExclLabel";
            lblSellingPriceExclLabel.Size = new Size(97, 15);
            lblSellingPriceExclLabel.TabIndex = 14;
            lblSellingPriceExclLabel.Text = "Selling Price Excl:";
            // 
            // txtSellingPrice5
            //
            txtSellingPrice5.Location = new Point(175, 280);
            txtSellingPrice5.Margin = new Padding(4, 3, 4, 3);
            txtSellingPrice5.Name = "txtSellingPrice5";
            txtSellingPrice5.Size = new Size(116, 23);
            txtSellingPrice5.TabIndex = 13;
            txtSellingPrice5.Text = "0.00";
            txtSellingPrice5.TextAlign = HorizontalAlignment.Left;
            // 
            // lblSellingPrice5
            // 
            lblSellingPrice5.AutoSize = true;
            lblSellingPrice5.Location = new Point(6, 280);
            lblSellingPrice5.Margin = new Padding(4, 0, 4, 0);
            lblSellingPrice5.Name = "lblSellingPrice5";
            lblSellingPrice5.Size = new Size(83, 15);
            lblSellingPrice5.TabIndex = 12;
            lblSellingPrice5.Text = "Selling Price 5:";
            // 
            // txtSellingPrice4
            //
            txtSellingPrice4.Location = new Point(175, 249);
            txtSellingPrice4.Margin = new Padding(4, 3, 4, 3);
            txtSellingPrice4.Name = "txtSellingPrice4";
            txtSellingPrice4.Size = new Size(116, 23);
            txtSellingPrice4.TabIndex = 11;
            txtSellingPrice4.Text = "0.00";
            txtSellingPrice4.TextAlign = HorizontalAlignment.Left;
            // 
            // lblSellingPrice4
            // 
            lblSellingPrice4.AutoSize = true;
            lblSellingPrice4.Location = new Point(6, 249);
            lblSellingPrice4.Margin = new Padding(4, 0, 4, 0);
            lblSellingPrice4.Name = "lblSellingPrice4";
            lblSellingPrice4.Size = new Size(83, 15);
            lblSellingPrice4.TabIndex = 10;
            lblSellingPrice4.Text = "Selling Price 4:";
            // 
            // txtSellingPrice3
            //
            txtSellingPrice3.Location = new Point(175, 218);
            txtSellingPrice3.Margin = new Padding(4, 3, 4, 3);
            txtSellingPrice3.Name = "txtSellingPrice3";
            txtSellingPrice3.Size = new Size(116, 23);
            txtSellingPrice3.TabIndex = 9;
            txtSellingPrice3.Text = "0.00";
            txtSellingPrice3.TextAlign = HorizontalAlignment.Left;
            // 
            // lblSellingPrice3
            // 
            lblSellingPrice3.AutoSize = true;
            lblSellingPrice3.Location = new Point(6, 218);
            lblSellingPrice3.Margin = new Padding(4, 0, 4, 0);
            lblSellingPrice3.Name = "lblSellingPrice3";
            lblSellingPrice3.Size = new Size(83, 15);
            lblSellingPrice3.TabIndex = 8;
            lblSellingPrice3.Text = "Selling Price 3:";
            // 
            // txtSellingPrice2
            //
            txtSellingPrice2.Location = new Point(175, 187);
            txtSellingPrice2.Margin = new Padding(4, 3, 4, 3);
            txtSellingPrice2.Name = "txtSellingPrice2";
            txtSellingPrice2.Size = new Size(116, 23);
            txtSellingPrice2.TabIndex = 7;
            txtSellingPrice2.Text = "0.00";
            txtSellingPrice2.TextAlign = HorizontalAlignment.Left;
            // 
            // lblSellingPrice2
            // 
            lblSellingPrice2.AutoSize = true;
            lblSellingPrice2.Location = new Point(6, 187);
            lblSellingPrice2.Margin = new Padding(4, 0, 4, 0);
            lblSellingPrice2.Name = "lblSellingPrice2";
            lblSellingPrice2.Size = new Size(83, 15);
            lblSellingPrice2.TabIndex = 6;
            lblSellingPrice2.Text = "Selling Price 2:";
            // 
            // txtMarkup
            //
            txtMarkup.Location = new Point(175, 156);
            txtMarkup.Margin = new Padding(4, 3, 4, 3);
            txtMarkup.Name = "txtMarkup";
            txtMarkup.Size = new Size(116, 23);
            txtMarkup.TabIndex = 5;
            txtMarkup.Text = "0.00";
            txtMarkup.TextAlign = HorizontalAlignment.Left;
            // 
            // lblMarkup
            // 
            lblMarkup.AutoSize = true;
            lblMarkup.Location = new Point(6, 156);
            lblMarkup.Margin = new Padding(4, 0, 4, 0);
            lblMarkup.Name = "lblMarkup";
            lblMarkup.Size = new Size(51, 15);
            lblMarkup.TabIndex = 4;
            lblMarkup.Text = "Markup:";
            // 
            // txtCostPriceIncl
            //
            txtCostPriceIncl.Location = new Point(175, 32);
            txtCostPriceIncl.Margin = new Padding(4, 3, 4, 3);
            txtCostPriceIncl.Name = "txtCostPriceIncl";
            txtCostPriceIncl.Size = new Size(116, 23);
            txtCostPriceIncl.TabIndex = 17;
            txtCostPriceIncl.Text = "0.00";
            txtCostPriceIncl.TextAlign = HorizontalAlignment.Left;
            // 
            // lblCostPriceIncl
            // 
            lblCostPriceIncl.AutoSize = true;
            lblCostPriceIncl.Location = new Point(6, 32);
            lblCostPriceIncl.Margin = new Padding(4, 0, 4, 0);
            lblCostPriceIncl.Name = "lblCostPriceIncl";
            lblCostPriceIncl.Size = new Size(85, 15);
            lblCostPriceIncl.TabIndex = 16;
            lblCostPriceIncl.Text = "Cost Price Incl:";
            // 
            // txtCostPriceExcl
            //
            txtCostPriceExcl.Location = new Point(175, 63);
            txtCostPriceExcl.Margin = new Padding(4, 3, 4, 3);
            txtCostPriceExcl.Name = "txtCostPriceExcl";
            txtCostPriceExcl.Size = new Size(116, 23);
            txtCostPriceExcl.TabIndex = 3;
            txtCostPriceExcl.Text = "0.00";
            txtCostPriceExcl.TextAlign = HorizontalAlignment.Left;
            // 
            // lblCostPriceExcl
            // 
            lblCostPriceExcl.AutoSize = true;
            lblCostPriceExcl.Location = new Point(6, 63);
            lblCostPriceExcl.Margin = new Padding(4, 0, 4, 0);
            lblCostPriceExcl.Name = "lblCostPriceExcl";
            lblCostPriceExcl.Size = new Size(86, 15);
            lblCostPriceExcl.TabIndex = 2;
            lblCostPriceExcl.Text = "Cost Price Excl:";
            // 
            // txtSellingPriceIncl
            //
            txtSellingPriceIncl.Location = new Point(175, 125);
            txtSellingPriceIncl.Margin = new Padding(4, 3, 4, 3);
            txtSellingPriceIncl.Name = "txtSellingPriceIncl";
            txtSellingPriceIncl.Size = new Size(116, 23);
            txtSellingPriceIncl.TabIndex = 1;
            txtSellingPriceIncl.Text = "0.00";
            txtSellingPriceIncl.TextAlign = HorizontalAlignment.Left;
            // 
            // lblSellingPriceIncl
            // 
            lblSellingPriceIncl.AutoSize = true;
            lblSellingPriceIncl.Location = new Point(6, 125);
            lblSellingPriceIncl.Margin = new Padding(4, 0, 4, 0);
            lblSellingPriceIncl.Name = "lblSellingPriceIncl";
            lblSellingPriceIncl.Size = new Size(96, 15);
            lblSellingPriceIncl.TabIndex = 0;
            lblSellingPriceIncl.Text = "Selling Price Incl:";
            // 
            // grpSupplier
            //
            // grpSupplier
            //
            grpSupplier.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            grpSupplier.Controls.Add(txtSupplierCode);
            grpSupplier.Controls.Add(lblSupplierCode);
            grpSupplier.Controls.Add(cmbSupplier);
            grpSupplier.Controls.Add(lblSupplier);
            grpSupplier.Location = new Point(690, 343);
            grpSupplier.Margin = new Padding(4, 3, 4, 3);
            grpSupplier.Name = "grpSupplier";
            grpSupplier.Padding = new Padding(4, 3, 4, 3);
            grpSupplier.Size = new Size(361, 108);
            grpSupplier.TabIndex = 3;
            grpSupplier.TabStop = false;
            grpSupplier.Text = "Supplier Information";
            // 
            // txtSupplierCode
            //
            txtSupplierCode.Location = new Point(113, 65);
            txtSupplierCode.Margin = new Padding(4, 3, 4, 3);
            txtSupplierCode.Name = "txtSupplierCode";
            txtSupplierCode.Size = new Size(233, 23);
            txtSupplierCode.TabIndex = 3;
            //
            // lblSupplierCode
            //
            lblSupplierCode.AutoSize = true;
            lblSupplierCode.Location = new Point(12, 68);
            lblSupplierCode.Margin = new Padding(4, 0, 4, 0);
            lblSupplierCode.Name = "lblSupplierCode";
            lblSupplierCode.Size = new Size(89, 15);
            lblSupplierCode.TabIndex = 2;
            lblSupplierCode.Text = "Supplier Code:";
            //
            // cmbSupplier
            //
            cmbSupplier.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSupplier.FormattingEnabled = true;
            cmbSupplier.Location = new Point(113, 29);
            cmbSupplier.Margin = new Padding(4, 3, 4, 3);
            cmbSupplier.Name = "cmbSupplier";
            cmbSupplier.Size = new Size(233, 23);
            cmbSupplier.TabIndex = 1;
            cmbSupplier.SelectedIndexChanged += cmbSupplier_SelectedIndexChanged;
            //
            // lblSupplier
            //
            lblSupplier.AutoSize = true;
            lblSupplier.Location = new Point(12, 32);
            lblSupplier.Margin = new Padding(4, 0, 4, 0);
            lblSupplier.Name = "lblSupplier";
            lblSupplier.Size = new Size(53, 15);
            lblSupplier.TabIndex = 0;
            lblSupplier.Text = "Supplier:";
            // 
            // pnlFooter
            // 
            pnlFooter.Controls.Add(grpActions);
            pnlFooter.Controls.Add(grpSessionProducts);
            pnlFooter.Dock = DockStyle.Bottom;
            pnlFooter.Location = new Point(0, 577);
            pnlFooter.Margin = new Padding(4, 3, 4, 3);
            pnlFooter.Name = "pnlFooter";
            pnlFooter.Size = new Size(1130, 173);
            pnlFooter.TabIndex = 2;
            // 
            // grpActions
            //
            grpActions.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            grpActions.Controls.Add(btnReviewProducts);
            grpActions.Controls.Add(btnAddProduct);
            grpActions.Location = new Point(755, 12);
            grpActions.Margin = new Padding(4, 3, 4, 3);
            grpActions.Name = "grpActions";
            grpActions.Padding = new Padding(4, 3, 4, 3);
            grpActions.Size = new Size(360, 150);
            grpActions.TabIndex = 1;
            grpActions.TabStop = false;
            grpActions.Text = "Actions";
            // 
            // btnReviewProducts
            // 
            btnReviewProducts.Enabled = false;
            btnReviewProducts.Location = new Point(12, 69);
            btnReviewProducts.Margin = new Padding(4, 3, 4, 3);
            btnReviewProducts.Name = "btnReviewProducts";
            btnReviewProducts.Size = new Size(303, 35);
            btnReviewProducts.TabIndex = 1;
            btnReviewProducts.Text = "Review Products";
            btnReviewProducts.UseVisualStyleBackColor = true;
            btnReviewProducts.Click += btnReviewProducts_Click;
            // 
            // btnAddProduct
            // 
            btnAddProduct.Enabled = false;
            btnAddProduct.Location = new Point(12, 29);
            btnAddProduct.Margin = new Padding(4, 3, 4, 3);
            btnAddProduct.Name = "btnAddProduct";
            btnAddProduct.Size = new Size(303, 35);
            btnAddProduct.TabIndex = 0;
            btnAddProduct.Text = "Add Product to Session";
            btnAddProduct.UseVisualStyleBackColor = true;
            btnAddProduct.Click += btnAddProduct_Click;
            // 
            // grpSessionProducts
            //
            grpSessionProducts.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            grpSessionProducts.Controls.Add(btnRemoveProduct);
            grpSessionProducts.Controls.Add(dgvSessionProducts);
            grpSessionProducts.Location = new Point(18, 12);
            grpSessionProducts.Margin = new Padding(4, 3, 4, 3);
            grpSessionProducts.Name = "grpSessionProducts";
            grpSessionProducts.Padding = new Padding(4, 3, 4, 3);
            grpSessionProducts.Size = new Size(720, 150);
            grpSessionProducts.TabIndex = 0;
            grpSessionProducts.TabStop = false;
            grpSessionProducts.Text = "Products in Current Session";
            //
            // dgvSessionProducts
            //
            dgvSessionProducts.AllowUserToAddRows = false;
            dgvSessionProducts.AllowUserToDeleteRows = false;
            dgvSessionProducts.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvSessionProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvSessionProducts.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvSessionProducts.Location = new Point(12, 23);
            dgvSessionProducts.Margin = new Padding(4, 3, 4, 3);
            dgvSessionProducts.MultiSelect = false;
            dgvSessionProducts.Name = "dgvSessionProducts";
            dgvSessionProducts.ReadOnly = true;
            dgvSessionProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSessionProducts.Size = new Size(580, 114);
            dgvSessionProducts.TabIndex = 0;
            //
            // btnRemoveProduct
            //
            btnRemoveProduct.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnRemoveProduct.Location = new Point(600, 23);
            btnRemoveProduct.Margin = new Padding(4, 3, 4, 3);
            btnRemoveProduct.Name = "btnRemoveProduct";
            btnRemoveProduct.Size = new Size(110, 35);
            btnRemoveProduct.TabIndex = 1;
            btnRemoveProduct.Text = "Remove Selected";
            btnRemoveProduct.UseVisualStyleBackColor = true;
            btnRemoveProduct.Click += btnRemoveProduct_Click;
            // 
            // KingsleyProductsCreateForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(pnlBody);
            Controls.Add(pnlFooter);
            Controls.Add(pnlHeader);
            Margin = new Padding(4, 3, 4, 3);
            MinimumSize = new Size(1130, 600);
            Name = "KingsleyProductsCreateForm";
            Size = new Size(1130, 750);
            pnlHeader.ResumeLayout(false);
            grpTemplate.ResumeLayout(false);
            grpTemplate.PerformLayout();
            grpProductCode.ResumeLayout(false);
            grpProductCode.PerformLayout();
            pnlBody.ResumeLayout(false);
            grpProductDetails.ResumeLayout(false);
            grpProductDetails.PerformLayout();
            grpPricing.ResumeLayout(false);
            grpPricing.PerformLayout();
            grpSupplier.ResumeLayout(false);
            grpSupplier.PerformLayout();
            pnlFooter.ResumeLayout(false);
            grpActions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvSessionProducts).EndInit();
            grpSessionProducts.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.GroupBox grpTemplate;
        private System.Windows.Forms.Button btnLoadTemplate;
        private System.Windows.Forms.TextBox txtTemplateCode;
        private System.Windows.Forms.Label lblTemplateCode;
        private System.Windows.Forms.GroupBox grpProductCode;
        private System.Windows.Forms.Button btnClearForm;
        private System.Windows.Forms.Button btnNextAvailableCode;
        private System.Windows.Forms.TextBox txtProductCode;
        private System.Windows.Forms.Label lblProductCode;
        private System.Windows.Forms.Panel pnlBody;
        private System.Windows.Forms.GroupBox grpProductDetails;
        private System.Windows.Forms.ComboBox cmbMinorDepartment;
        private System.Windows.Forms.Label lblMinorDepartment;
        private System.Windows.Forms.TextBox txtTaxValue;
        private System.Windows.Forms.Label lblTaxValue;
        private System.Windows.Forms.ComboBox cmbSizeLink;
        private System.Windows.Forms.Label lblSizeLink;
        private System.Windows.Forms.ComboBox cmbProductSize;
        private System.Windows.Forms.Label lblProductSize;
        private System.Windows.Forms.TextBox txtPackSize;
        private System.Windows.Forms.Label lblPackSize;
        private System.Windows.Forms.CheckBox chkTillItem;
        private System.Windows.Forms.CheckBox chkStockItem;
        private System.Windows.Forms.CheckBox chkDiscontinued;
        private System.Windows.Forms.ComboBox cmbDepartment;
        private System.Windows.Forms.Label lblDepartment;
        private System.Windows.Forms.TextBox txtUnitOfMeasure;
        private System.Windows.Forms.Label lblUnitOfMeasure;
        private System.Windows.Forms.TextBox txtUnitSize;
        private System.Windows.Forms.Label lblUnitSize;
        private System.Windows.Forms.ComboBox cmbBrandName;
        private System.Windows.Forms.Label lblBrandName;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.GroupBox grpPricing;
        private System.Windows.Forms.TextBox txtSellingPriceExcl;
        private System.Windows.Forms.Label lblSellingPriceExclLabel;
        private System.Windows.Forms.TextBox txtSellingPrice5;
        private System.Windows.Forms.Label lblSellingPrice5;
        private System.Windows.Forms.TextBox txtSellingPrice4;
        private System.Windows.Forms.Label lblSellingPrice4;
        private System.Windows.Forms.TextBox txtSellingPrice3;
        private System.Windows.Forms.Label lblSellingPrice3;
        private System.Windows.Forms.TextBox txtSellingPrice2;
        private System.Windows.Forms.Label lblSellingPrice2;
        private System.Windows.Forms.TextBox txtMarkup;
        private System.Windows.Forms.Label lblMarkup;
        private System.Windows.Forms.TextBox txtCostPriceIncl;
        private System.Windows.Forms.Label lblCostPriceIncl;
        private System.Windows.Forms.TextBox txtCostPriceExcl;
        private System.Windows.Forms.Label lblCostPriceExcl;
        private System.Windows.Forms.TextBox txtSellingPriceIncl;
        private System.Windows.Forms.Label lblSellingPriceIncl;
        private System.Windows.Forms.GroupBox grpSupplier;
        private System.Windows.Forms.TextBox txtSupplierCode;
        private System.Windows.Forms.Label lblSupplierCode;
        private System.Windows.Forms.ComboBox cmbSupplier;
        private System.Windows.Forms.Label lblSupplier;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.GroupBox grpActions;
        private System.Windows.Forms.Button btnReviewProducts;
        private System.Windows.Forms.Button btnAddProduct;
        private System.Windows.Forms.GroupBox grpSessionProducts;
        private System.Windows.Forms.DataGridView dgvSessionProducts;
        private System.Windows.Forms.Button btnRemoveProduct;
    }
}