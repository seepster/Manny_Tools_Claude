namespace Manny_Tools_Claude
{
    partial class DoubleSizingForm
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
            this.lblFirstStartRange = new System.Windows.Forms.Label();
            this.comboFirstStart = new System.Windows.Forms.ComboBox();
            this.lblFirstEndRange = new System.Windows.Forms.Label();
            this.comboFirstEnd = new System.Windows.Forms.ComboBox();
            this.lblFirstIncrement = new System.Windows.Forms.Label();
            this.comboFirstIncrement = new System.Windows.Forms.ComboBox();
            this.lblSecondStartRange = new System.Windows.Forms.Label();
            this.comboSecondStart = new System.Windows.Forms.ComboBox();
            this.lblSecondEndRange = new System.Windows.Forms.Label();
            this.comboSecondEnd = new System.Windows.Forms.ComboBox();
            this.lblSecondIncrement = new System.Windows.Forms.Label();
            this.comboSecondIncrement = new System.Windows.Forms.ComboBox();
            this.btnGenerateSizes = new System.Windows.Forms.Button();
            this.listSizes = new System.Windows.Forms.CheckedListBox();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.btnSelectNone = new System.Windows.Forms.Button();
            this.btnCreateProducts = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblSizesDescription = new System.Windows.Forms.Label();
            this.lblFirstSizing = new System.Windows.Forms.Label();
            this.lblSecondSizing = new System.Windows.Forms.Label();
            this.lblExampleDescription = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(12, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(133, 25);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Double Sizing";
            // 
            // lblFirstSizing
            // 
            this.lblFirstSizing.AutoSize = true;
            this.lblFirstSizing.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblFirstSizing.Location = new System.Drawing.Point(13, 50);
            this.lblFirstSizing.Name = "lblFirstSizing";
            this.lblFirstSizing.Size = new System.Drawing.Size(165, 19);
            this.lblFirstSizing.TabIndex = 1;
            this.lblFirstSizing.Text = "First Sizing (e.g., Waist):";
            // 
            // lblFirstStartRange
            // 
            this.lblFirstStartRange.AutoSize = true;
            this.lblFirstStartRange.Location = new System.Drawing.Point(14, 80);
            this.lblFirstStartRange.Name = "lblFirstStartRange";
            this.lblFirstStartRange.Size = new System.Drawing.Size(73, 15);
            this.lblFirstStartRange.TabIndex = 2;
            this.lblFirstStartRange.Text = "Start Range:";
            // 
            // comboFirstStart
            // 
            this.comboFirstStart.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboFirstStart.FormattingEnabled = true;
            this.comboFirstStart.Location = new System.Drawing.Point(93, 77);
            this.comboFirstStart.Name = "comboFirstStart";
            this.comboFirstStart.Size = new System.Drawing.Size(85, 23);
            this.comboFirstStart.TabIndex = 3;
            // 
            // lblFirstEndRange
            // 
            this.lblFirstEndRange.AutoSize = true;
            this.lblFirstEndRange.Location = new System.Drawing.Point(193, 80);
            this.lblFirstEndRange.Name = "lblFirstEndRange";
            this.lblFirstEndRange.Size = new System.Drawing.Size(67, 15);
            this.lblFirstEndRange.TabIndex = 4;
            this.lblFirstEndRange.Text = "End Range:";
            // 
            // comboFirstEnd
            // 
            this.comboFirstEnd.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboFirstEnd.FormattingEnabled = true;
            this.comboFirstEnd.Location = new System.Drawing.Point(266, 77);
            this.comboFirstEnd.Name = "comboFirstEnd";
            this.comboFirstEnd.Size = new System.Drawing.Size(85, 23);
            this.comboFirstEnd.TabIndex = 5;
            // 
            // lblFirstIncrement
            // 
            this.lblFirstIncrement.AutoSize = true;
            this.lblFirstIncrement.Location = new System.Drawing.Point(366, 80);
            this.lblFirstIncrement.Name = "lblFirstIncrement";
            this.lblFirstIncrement.Size = new System.Drawing.Size(65, 15);
            this.lblFirstIncrement.TabIndex = 6;
            this.lblFirstIncrement.Text = "Increment:";
            // 
            // comboFirstIncrement
            // 
            this.comboFirstIncrement.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboFirstIncrement.FormattingEnabled = true;
            this.comboFirstIncrement.Location = new System.Drawing.Point(437, 77);
            this.comboFirstIncrement.Name = "comboFirstIncrement";
            this.comboFirstIncrement.Size = new System.Drawing.Size(85, 23);
            this.comboFirstIncrement.TabIndex = 7;
            // 
            // lblSecondSizing
            // 
            this.lblSecondSizing.AutoSize = true;
            this.lblSecondSizing.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblSecondSizing.Location = new System.Drawing.Point(13, 120);
            this.lblSecondSizing.Name = "lblSecondSizing";
            this.lblSecondSizing.Size = new System.Drawing.Size(181, 19);
            this.lblSecondSizing.TabIndex = 8;
            this.lblSecondSizing.Text = "Second Sizing (e.g., Inseam):";
            // 
            // lblSecondStartRange
            // 
            this.lblSecondStartRange.AutoSize = true;
            this.lblSecondStartRange.Location = new System.Drawing.Point(14, 150);
            this.lblSecondStartRange.Name = "lblSecondStartRange";
            this.lblSecondStartRange.Size = new System.Drawing.Size(73, 15);
            this.lblSecondStartRange.TabIndex = 9;
            this.lblSecondStartRange.Text = "Start Range:";
            // 
            // comboSecondStart
            // 
            this.comboSecondStart.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboSecondStart.FormattingEnabled = true;
            this.comboSecondStart.Location = new System.Drawing.Point(93, 147);
            this.comboSecondStart.Name = "comboSecondStart";
            this.comboSecondStart.Size = new System.Drawing.Size(85, 23);
            this.comboSecondStart.TabIndex = 10;
            // 
            // lblSecondEndRange
            // 
            this.lblSecondEndRange.AutoSize = true;
            this.lblSecondEndRange.Location = new System.Drawing.Point(193, 150);
            this.lblSecondEndRange.Name = "lblSecondEndRange";
            this.lblSecondEndRange.Size = new System.Drawing.Size(67, 15);
            this.lblSecondEndRange.TabIndex = 11;
            this.lblSecondEndRange.Text = "End Range:";
            // 
            // comboSecondEnd
            // 
            this.comboSecondEnd.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboSecondEnd.FormattingEnabled = true;
            this.comboSecondEnd.Location = new System.Drawing.Point(266, 147);
            this.comboSecondEnd.Name = "comboSecondEnd";
            this.comboSecondEnd.Size = new System.Drawing.Size(85, 23);
            this.comboSecondEnd.TabIndex = 12;
            // 
            // lblSecondIncrement
            // 
            this.lblSecondIncrement.AutoSize = true;
            this.lblSecondIncrement.Location = new System.Drawing.Point(366, 150);
            this.lblSecondIncrement.Name = "lblSecondIncrement";
            this.lblSecondIncrement.Size = new System.Drawing.Size(65, 15);
            this.lblSecondIncrement.TabIndex = 13;
            this.lblSecondIncrement.Text = "Increment:";
            // 
            // comboSecondIncrement
            // 
            this.comboSecondIncrement.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboSecondIncrement.FormattingEnabled = true;
            this.comboSecondIncrement.Location = new System.Drawing.Point(437, 147);
            this.comboSecondIncrement.Name = "comboSecondIncrement";
            this.comboSecondIncrement.Size = new System.Drawing.Size(85, 23);
            this.comboSecondIncrement.TabIndex = 14;
            // 
            // btnGenerateSizes
            // 
            this.btnGenerateSizes.Location = new System.Drawing.Point(14, 188);
            this.btnGenerateSizes.Name = "btnGenerateSizes";
            this.btnGenerateSizes.Size = new System.Drawing.Size(508, 30);
            this.btnGenerateSizes.TabIndex = 15;
            this.btnGenerateSizes.Text = "Generate Sizes";
            this.btnGenerateSizes.UseVisualStyleBackColor = true;
            this.btnGenerateSizes.Click += new System.EventHandler(this.btnGenerateSizes_Click);
            // 
            // listSizes
            // 
            this.listSizes.CheckOnClick = true;
            this.listSizes.FormattingEnabled = true;
            this.listSizes.Location = new System.Drawing.Point(14, 255);
            this.listSizes.Name = "listSizes";
            this.listSizes.Size = new System.Drawing.Size(508, 184);
            this.listSizes.TabIndex = 17;
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Location = new System.Drawing.Point(14, 445);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(100, 30);
            this.btnSelectAll.TabIndex = 18;
            this.btnSelectAll.Text = "Select All";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // btnSelectNone
            // 
            this.btnSelectNone.Location = new System.Drawing.Point(120, 445);
            this.btnSelectNone.Name = "btnSelectNone";
            this.btnSelectNone.Size = new System.Drawing.Size(100, 30);
            this.btnSelectNone.TabIndex = 19;
            this.btnSelectNone.Text = "Select None";
            this.btnSelectNone.UseVisualStyleBackColor = true;
            this.btnSelectNone.Click += new System.EventHandler(this.btnSelectNone_Click);
            // 
            // btnCreateProducts
            // 
            this.btnCreateProducts.Enabled = false;
            this.btnCreateProducts.Location = new System.Drawing.Point(422, 445);
            this.btnCreateProducts.Name = "btnCreateProducts";
            this.btnCreateProducts.Size = new System.Drawing.Size(100, 30);
            this.btnCreateProducts.TabIndex = 21;
            this.btnCreateProducts.Text = "Create Products";
            this.btnCreateProducts.UseVisualStyleBackColor = true;
            this.btnCreateProducts.Click += new System.EventHandler(this.btnCreateProducts_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(316, 445);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 30);
            this.btnCancel.TabIndex = 20;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblSizesDescription
            // 
            this.lblSizesDescription.AutoSize = true;
            this.lblSizesDescription.Location = new System.Drawing.Point(14, 237);
            this.lblSizesDescription.Name = "lblSizesDescription";
            this.lblSizesDescription.Size = new System.Drawing.Size(218, 15);
            this.lblSizesDescription.TabIndex = 16;
            this.lblSizesDescription.Text = "Select the sizes you would like to create:";
            // 
            // lblExampleDescription
            // 
            this.lblExampleDescription.AutoSize = true;
            this.lblExampleDescription.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblExampleDescription.Location = new System.Drawing.Point(151, 15);
            this.lblExampleDescription.Name = "lblExampleDescription";
            this.lblExampleDescription.Size = new System.Drawing.Size(231, 15);
            this.lblExampleDescription.TabIndex = 22;
            this.lblExampleDescription.Text = "(e.g., 32/30 where 32=waist and 30=inseam)";
            // 
            // DoubleSizingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 491);
            this.Controls.Add(this.lblExampleDescription);
            this.Controls.Add(this.lblSizesDescription);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnCreateProducts);
            this.Controls.Add(this.btnSelectNone);
            this.Controls.Add(this.btnSelectAll);
            this.Controls.Add(this.listSizes);
            this.Controls.Add(this.btnGenerateSizes);
            this.Controls.Add(this.comboSecondIncrement);
            this.Controls.Add(this.lblSecondIncrement);
            this.Controls.Add(this.comboSecondEnd);
            this.Controls.Add(this.lblSecondEndRange);
            this.Controls.Add(this.comboSecondStart);
            this.Controls.Add(this.lblSecondStartRange);
            this.Controls.Add(this.lblSecondSizing);
            this.Controls.Add(this.comboFirstIncrement);
            this.Controls.Add(this.lblFirstIncrement);
            this.Controls.Add(this.comboFirstEnd);
            this.Controls.Add(this.lblFirstEndRange);
            this.Controls.Add(this.comboFirstStart);
            this.Controls.Add(this.lblFirstStartRange);
            this.Controls.Add(this.lblFirstSizing);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DoubleSizingForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Double Sizing";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblFirstSizing;
        private System.Windows.Forms.Label lblFirstStartRange;
        private System.Windows.Forms.ComboBox comboFirstStart;
        private System.Windows.Forms.Label lblFirstEndRange;
        private System.Windows.Forms.ComboBox comboFirstEnd;
        private System.Windows.Forms.Label lblFirstIncrement;
        private System.Windows.Forms.ComboBox comboFirstIncrement;
        private System.Windows.Forms.Label lblSecondSizing;
        private System.Windows.Forms.Label lblSecondStartRange;
        private System.Windows.Forms.ComboBox comboSecondStart;
        private System.Windows.Forms.Label lblSecondEndRange;
        private System.Windows.Forms.ComboBox comboSecondEnd;
        private System.Windows.Forms.Label lblSecondIncrement;
        private System.Windows.Forms.ComboBox comboSecondIncrement;
        private System.Windows.Forms.Button btnGenerateSizes;
        private System.Windows.Forms.CheckedListBox listSizes;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.Button btnSelectNone;
        private System.Windows.Forms.Button btnCreateProducts;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblSizesDescription;
        private System.Windows.Forms.Label lblExampleDescription;
    }
}