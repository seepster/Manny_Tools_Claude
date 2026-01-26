namespace Manny_Tools_Claude
{
    partial class BabySizingForm
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
            this.lblTitle.Size = new System.Drawing.Size(110, 25);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Baby Sizing";
            // 
            // listSizes
            // 
            this.listSizes.CheckOnClick = true;
            this.listSizes.FormattingEnabled = true;
            this.listSizes.Location = new System.Drawing.Point(12, 79);
            this.listSizes.Name = "listSizes";
            this.listSizes.Size = new System.Drawing.Size(311, 184);
            this.listSizes.TabIndex = 2;
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Location = new System.Drawing.Point(12, 269);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(100, 30);
            this.btnSelectAll.TabIndex = 3;
            this.btnSelectAll.Text = "Select All";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // btnSelectNone
            // 
            this.btnSelectNone.Location = new System.Drawing.Point(118, 269);
            this.btnSelectNone.Name = "btnSelectNone";
            this.btnSelectNone.Size = new System.Drawing.Size(100, 30);
            this.btnSelectNone.TabIndex = 4;
            this.btnSelectNone.Text = "Select None";
            this.btnSelectNone.UseVisualStyleBackColor = true;
            this.btnSelectNone.Click += new System.EventHandler(this.btnSelectNone_Click);
            // 
            // btnCreateProducts
            // 
            this.btnCreateProducts.Location = new System.Drawing.Point(223, 313);
            this.btnCreateProducts.Name = "btnCreateProducts";
            this.btnCreateProducts.Size = new System.Drawing.Size(100, 30);
            this.btnCreateProducts.TabIndex = 6;
            this.btnCreateProducts.Text = "Create Products";
            this.btnCreateProducts.UseVisualStyleBackColor = true;
            this.btnCreateProducts.Click += new System.EventHandler(this.btnCreateProducts_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(117, 313);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 30);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblSizesDescription
            // 
            this.lblSizesDescription.AutoSize = true;
            this.lblSizesDescription.Location = new System.Drawing.Point(12, 60);
            this.lblSizesDescription.Name = "lblSizesDescription";
            this.lblSizesDescription.Size = new System.Drawing.Size(218, 15);
            this.lblSizesDescription.TabIndex = 1;
            this.lblSizesDescription.Text = "Select the sizes you would like to create:";
            // 
            // BabySizingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 356);
            this.Controls.Add(this.lblSizesDescription);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnCreateProducts);
            this.Controls.Add(this.btnSelectNone);
            this.Controls.Add(this.btnSelectAll);
            this.Controls.Add(this.listSizes);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BabySizingForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Baby Sizing";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.CheckedListBox listSizes;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.Button btnSelectNone;
        private System.Windows.Forms.Button btnCreateProducts;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblSizesDescription;
    }
}