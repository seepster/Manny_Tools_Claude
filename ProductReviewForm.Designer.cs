namespace Manny_Tools_Claude
{
    partial class ProductReviewForm
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
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblWarning = new System.Windows.Forms.Label();
            this.lblOverrideProducts = new System.Windows.Forms.Label();
            this.lblNewProducts = new System.Windows.Forms.Label();
            this.lblTotalProducts = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.grpProductList = new System.Windows.Forms.GroupBox();
            this.dgvProductReview = new System.Windows.Forms.DataGridView();
            this.grpProductDetails = new System.Windows.Forms.GroupBox();
            this.txtProductDetails = new System.Windows.Forms.TextBox();
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnCreateProducts = new System.Windows.Forms.Button();
            this.btnRemoveProduct = new System.Windows.Forms.Button();
            this.pnlHeader.SuspendLayout();
            this.pnlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.grpProductList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProductReview)).BeginInit();
            this.grpProductDetails.SuspendLayout();
            this.pnlFooter.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.SystemColors.Control;
            this.pnlHeader.Controls.Add(this.lblWarning);
            this.pnlHeader.Controls.Add(this.lblOverrideProducts);
            this.pnlHeader.Controls.Add(this.lblNewProducts);
            this.pnlHeader.Controls.Add(this.lblTotalProducts);
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1000, 120);
            this.pnlHeader.TabIndex = 0;
            // 
            // lblWarning
            // 
            this.lblWarning.AutoSize = true;
            this.lblWarning.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWarning.ForeColor = System.Drawing.Color.Red;
            this.lblWarning.Location = new System.Drawing.Point(15, 90);
            this.lblWarning.Name = "lblWarning";
            this.lblWarning.Size = new System.Drawing.Size(0, 17);
            this.lblWarning.TabIndex = 4;
            this.lblWarning.Visible = false;
            // 
            // lblOverrideProducts
            // 
            this.lblOverrideProducts.AutoSize = true;
            this.lblOverrideProducts.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOverrideProducts.ForeColor = System.Drawing.Color.Red;
            this.lblOverrideProducts.Location = new System.Drawing.Point(200, 65);
            this.lblOverrideProducts.Name = "lblOverrideProducts";
            this.lblOverrideProducts.Size = new System.Drawing.Size(112, 15);
            this.lblOverrideProducts.TabIndex = 3;
            this.lblOverrideProducts.Text = "Override Products: 0";
            // 
            // lblNewProducts
            // 
            this.lblNewProducts.AutoSize = true;
            this.lblNewProducts.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNewProducts.ForeColor = System.Drawing.Color.Green;
            this.lblNewProducts.Location = new System.Drawing.Point(15, 65);
            this.lblNewProducts.Name = "lblNewProducts";
            this.lblNewProducts.Size = new System.Drawing.Size(95, 15);
            this.lblNewProducts.TabIndex = 2;
            this.lblNewProducts.Text = "New Products: 0";
            // 
            // lblTotalProducts
            // 
            this.lblTotalProducts.AutoSize = true;
            this.lblTotalProducts.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalProducts.Location = new System.Drawing.Point(15, 40);
            this.lblTotalProducts.Name = "lblTotalProducts";
            this.lblTotalProducts.Size = new System.Drawing.Size(124, 17);
            this.lblTotalProducts.TabIndex = 1;
            this.lblTotalProducts.Text = "Total Products: 0";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(15, 15);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(294, 24);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Review Products Before Creation";
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.splitContainer);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 120);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(1000, 430);
            this.pnlMain.TabIndex = 1;
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.grpProductList);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.grpProductDetails);
            this.splitContainer.Size = new System.Drawing.Size(1000, 430);
            this.splitContainer.SplitterDistance = 280;
            this.splitContainer.TabIndex = 0;
            // 
            // grpProductList
            // 
            this.grpProductList.Controls.Add(this.dgvProductReview);
            this.grpProductList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpProductList.Location = new System.Drawing.Point(0, 0);
            this.grpProductList.Name = "grpProductList";
            this.grpProductList.Size = new System.Drawing.Size(1000, 280);
            this.grpProductList.TabIndex = 0;
            this.grpProductList.TabStop = false;
            this.grpProductList.Text = "Products to Create (Green = New, Red = Override Existing)";
            // 
            // dgvProductReview
            // 
            this.dgvProductReview.AllowUserToAddRows = false;
            this.dgvProductReview.AllowUserToDeleteRows = false;
            this.dgvProductReview.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvProductReview.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProductReview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvProductReview.Location = new System.Drawing.Point(3, 16);
            this.dgvProductReview.MultiSelect = false;
            this.dgvProductReview.Name = "dgvProductReview";
            this.dgvProductReview.ReadOnly = true;
            this.dgvProductReview.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvProductReview.Size = new System.Drawing.Size(994, 261);
            this.dgvProductReview.TabIndex = 0;
            this.dgvProductReview.SelectionChanged += new System.EventHandler(this.dgvProductReview_SelectionChanged);
            // 
            // grpProductDetails
            // 
            this.grpProductDetails.Controls.Add(this.txtProductDetails);
            this.grpProductDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpProductDetails.Location = new System.Drawing.Point(0, 0);
            this.grpProductDetails.Name = "grpProductDetails";
            this.grpProductDetails.Size = new System.Drawing.Size(1000, 146);
            this.grpProductDetails.TabIndex = 0;
            this.grpProductDetails.TabStop = false;
            this.grpProductDetails.Text = "Product Details (Select a product above to view details)";
            // 
            // txtProductDetails
            // 
            this.txtProductDetails.BackColor = System.Drawing.SystemColors.Window;
            this.txtProductDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtProductDetails.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProductDetails.Location = new System.Drawing.Point(3, 16);
            this.txtProductDetails.Multiline = true;
            this.txtProductDetails.Name = "txtProductDetails";
            this.txtProductDetails.ReadOnly = true;
            this.txtProductDetails.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtProductDetails.Size = new System.Drawing.Size(994, 127);
            this.txtProductDetails.TabIndex = 0;
            // 
            // pnlFooter
            // 
            this.pnlFooter.BackColor = System.Drawing.SystemColors.Control;
            this.pnlFooter.Controls.Add(this.progressBar);
            this.pnlFooter.Controls.Add(this.btnRemoveProduct);
            this.pnlFooter.Controls.Add(this.btnCancel);
            this.pnlFooter.Controls.Add(this.btnCreateProducts);
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Location = new System.Drawing.Point(0, 550);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Size = new System.Drawing.Size(1000, 60);
            this.pnlFooter.TabIndex = 2;
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(15, 15);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(600, 30);
            this.progressBar.TabIndex = 2;
            this.progressBar.Visible = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(900, 15);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 30);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnCreateProducts
            //
            this.btnCreateProducts.BackColor = System.Drawing.Color.Green;
            this.btnCreateProducts.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCreateProducts.ForeColor = System.Drawing.Color.White;
            this.btnCreateProducts.Location = new System.Drawing.Point(800, 15);
            this.btnCreateProducts.Name = "btnCreateProducts";
            this.btnCreateProducts.Size = new System.Drawing.Size(90, 30);
            this.btnCreateProducts.TabIndex = 0;
            this.btnCreateProducts.Text = "Create All";
            this.btnCreateProducts.UseVisualStyleBackColor = false;
            this.btnCreateProducts.Click += new System.EventHandler(this.btnCreateProducts_Click);
            //
            // btnRemoveProduct
            //
            this.btnRemoveProduct.BackColor = System.Drawing.Color.IndianRed;
            this.btnRemoveProduct.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRemoveProduct.ForeColor = System.Drawing.Color.White;
            this.btnRemoveProduct.Location = new System.Drawing.Point(675, 15);
            this.btnRemoveProduct.Name = "btnRemoveProduct";
            this.btnRemoveProduct.Size = new System.Drawing.Size(115, 30);
            this.btnRemoveProduct.TabIndex = 3;
            this.btnRemoveProduct.Text = "Remove Selected";
            this.btnRemoveProduct.UseVisualStyleBackColor = false;
            this.btnRemoveProduct.Click += new System.EventHandler(this.btnRemoveProduct_Click);
            //
            // ProductReviewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 610);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnlFooter);
            this.Controls.Add(this.pnlHeader);
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "ProductReviewForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Product Review";
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlMain.ResumeLayout(false);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.grpProductList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvProductReview)).EndInit();
            this.grpProductDetails.ResumeLayout(false);
            this.grpProductDetails.PerformLayout();
            this.pnlFooter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblTotalProducts;
        private System.Windows.Forms.Label lblNewProducts;
        private System.Windows.Forms.Label lblOverrideProducts;
        private System.Windows.Forms.Label lblWarning;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.GroupBox grpProductList;
        private System.Windows.Forms.DataGridView dgvProductReview;
        private System.Windows.Forms.GroupBox grpProductDetails;
        private System.Windows.Forms.TextBox txtProductDetails;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.Button btnCreateProducts;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Button btnRemoveProduct;
    }
}