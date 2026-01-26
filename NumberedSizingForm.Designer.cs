namespace Manny_Tools_Claude
{
    partial class NumberedSizingForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblStartRange = new System.Windows.Forms.Label();
            this.comboStartRange = new System.Windows.Forms.ComboBox();
            this.lblEndRange = new System.Windows.Forms.Label();
            this.comboEndRange = new System.Windows.Forms.ComboBox();
            this.lblIncrement = new System.Windows.Forms.Label();
            this.comboIncrement = new System.Windows.Forms.ComboBox();
            this.btnGenerateSizes = new System.Windows.Forms.Button();
            this.listSizes = new System.Windows.Forms.CheckedListBox();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.btnSelectNone = new System.Windows.Forms.Button();
            this.btnCreateProducts = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblSizesDescription = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(12, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(164, 25);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Numbered Sizing";
            // 
            // lblStartRange
            // 
            this.lblStartRange.AutoSize = true;
            this.lblStartRange.Location = new System.Drawing.Point(14, 55);
            this.lblStartRange.Name = "lblStartRange";
            this.lblStartRange.Size = new System.Drawing.Size(73, 15);
            this.lblStartRange.TabIndex = 1;
            this.lblStartRange.Text = "Start Range:";
            // 
            // comboStartRange
            // 
            this.comboStartRange.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboStartRange.FormattingEnabled = true;
            this.comboStartRange.Location = new System.Drawing.Point(93, 52);
            this.comboStartRange.Name = "comboStartRange";
            this.comboStartRange.Size = new System.Drawing.Size(85, 23);
            this.comboStartRange.TabIndex = 2;
            // 
            // lblEndRange
            // 
            this.lblEndRange.AutoSize = true;
            this.lblEndRange.Location = new System.Drawing.Point(193, 55);
            this.lblEndRange.Name = "lblEndRange";
            this.lblEndRange.Size = new System.Drawing.Size(67, 15);
            this.lblEndRange.TabIndex = 3;
            this.lblEndRange.Text = "End Range:";
            // 
            // comboEndRange
            // 
            this.comboEndRange.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboEndRange.FormattingEnabled = true;
            this.comboEndRange.Location = new System.Drawing.Point(266, 52);
            this.comboEndRange.Name = "comboEndRange";
            this.comboEndRange.Size = new System.Drawing.Size(85, 23);
            this.comboEndRange.TabIndex = 4;
            // 
            // lblIncrement
            // 
            this.lblIncrement.AutoSize = true;
            this.lblIncrement.Location = new System.Drawing.Point(366, 55);
            this.lblIncrement.Name = "lblIncrement";
            this.lblIncrement.Size = new System.Drawing.Size(65, 15);
            this.lblIncrement.TabIndex = 5;
            this.lblIncrement.Text = "Increment:";
            // 
            // comboIncrement
            // 
            this.comboIncrement.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboIncrement.FormattingEnabled = true;
            this.comboIncrement.Location = new System.Drawing.Point(437, 52);
            this.comboIncrement.Name = "comboIncrement";
            this.comboIncrement.Size = new System.Drawing.Size(85, 23);
            this.comboIncrement.TabIndex = 6;
            // 
            // btnGenerateSizes
            // 
            this.btnGenerateSizes.Location = new System.Drawing.Point(14, 88);
            this.btnGenerateSizes.Name = "btnGenerateSizes";
            this.btnGenerateSizes.Size = new System.Drawing.Size(508, 30);
            this.btnGenerateSizes.TabIndex = 7;
            this.btnGenerateSizes.Text = "Generate Sizes";
            this.btnGenerateSizes.UseVisualStyleBackColor = true;
            this.btnGenerateSizes.Click += new System.EventHandler(this.btnGenerateSizes_Click);
            // 
            // listSizes
            // 
            this.listSizes.CheckOnClick = true;
            this.listSizes.FormattingEnabled = true;
            this.listSizes.Location = new System.Drawing.Point(14, 154);
            this.listSizes.Name = "listSizes";
            this.listSizes.Size = new System.Drawing.Size(508, 184);
            this.listSizes.TabIndex = 8;
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Location = new System.Drawing.Point(14, 344);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(100, 30);
            this.btnSelectAll.TabIndex = 9;
            this.btnSelectAll.Text = "Select All";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // btnSelectNone
            // 
            this.btnSelectNone.Location = new System.Drawing.Point(120, 344);
            this.btnSelectNone.Name = "btnSelectNone";
            this.btnSelectNone.Size = new System.Drawing.Size(100, 30);
            this.btnSelectNone.TabIndex = 10;
            this.btnSelectNone.Text = "Select None";
            this.btnSelectNone.UseVisualStyleBackColor = true;
            this.btnSelectNone.Click += new System.EventHandler(this.btnSelectNone_Click);
            // 
            // btnCreateProducts
            // 
            this.btnCreateProducts.Enabled = false;
            this.btnCreateProducts.Location = new System.Drawing.Point(422, 344);
            this.btnCreateProducts.Name = "btnCreateProducts";
            this.btnCreateProducts.Size = new System.Drawing.Size(100, 30);
            this.btnCreateProducts.TabIndex = 11;
            this.btnCreateProducts.Text = "Create Products";
            this.btnCreateProducts.UseVisualStyleBackColor = true;
            this.btnCreateProducts.Click += new System.EventHandler(this.btnCreateProducts_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(316, 344);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 30);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblSizesDescription
            // 
            this.lblSizesDescription.AutoSize = true;
            this.lblSizesDescription.Location = new System.Drawing.Point(14, 136);
            this.lblSizesDescription.Name = "lblSizesDescription";
            this.lblSizesDescription.Size = new System.Drawing.Size(218, 15);
            this.lblSizesDescription.TabIndex = 13;
            this.lblSizesDescription.Text = "Select the sizes you would like to create:";
            // 
            // NumberedSizingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 391);
            this.Controls.Add(this.lblSizesDescription);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnCreateProducts);
            this.Controls.Add(this.btnSelectNone);
            this.Controls.Add(this.btnSelectAll);
            this.Controls.Add(this.listSizes);
            this.Controls.Add(this.btnGenerateSizes);
            this.Controls.Add(this.comboIncrement);
            this.Controls.Add(this.lblIncrement);
            this.Controls.Add(this.comboEndRange);
            this.Controls.Add(this.lblEndRange);
            this.Controls.Add(this.comboStartRange);
            this.Controls.Add(this.lblStartRange);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NumberedSizingForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Numbered Sizing";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblStartRange;
        private System.Windows.Forms.ComboBox comboStartRange;
        private System.Windows.Forms.Label lblEndRange;
        private System.Windows.Forms.ComboBox comboEndRange;
        private System.Windows.Forms.Label lblIncrement;
        private System.Windows.Forms.ComboBox comboIncrement;
        private System.Windows.Forms.Button btnGenerateSizes;
        private System.Windows.Forms.CheckedListBox listSizes;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.Button btnSelectNone;
        private System.Windows.Forms.Button btnCreateProducts;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblSizesDescription;
    }
}