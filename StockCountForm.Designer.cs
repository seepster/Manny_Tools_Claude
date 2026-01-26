namespace Manny_Tools_Claude
{
    partial class StockCountForm
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
            this.components = new System.ComponentModel.Container();

            // Form controls
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblConnectionStatus = new System.Windows.Forms.Label();
            this.btnGetProducts = new System.Windows.Forms.Button();
            this.btnExportToCsv = new System.Windows.Forms.Button();
            this.btnClearGrid = new System.Windows.Forms.Button();
            this.dgvStockCount = new System.Windows.Forms.DataGridView();
            this.panelControls = new System.Windows.Forms.Panel();
            this.lblQueryStatus = new System.Windows.Forms.Label();

            ((System.ComponentModel.ISupportInitialize)(this.dgvStockCount)).BeginInit();
            this.panelControls.SuspendLayout();
            this.SuspendLayout();

            // Create main container with white background

            // Create a control panel for buttons and inputs
            this.panelControls.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControls.Height = 100;

            // Title label
            this.lblTitle.Text = "Stock Count";
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(15, 10);
            this.lblTitle.AutoSize = true;
            this.panelControls.Controls.Add(this.lblTitle);

            // Add connection status label
            this.lblConnectionStatus.Text = "Connection: Unknown";
            this.lblConnectionStatus.Location = new System.Drawing.Point(300, 15);
            this.lblConnectionStatus.Size = new System.Drawing.Size(200, 20);
            this.lblConnectionStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblConnectionStatus.Font = new System.Drawing.Font("Segoe UI", 9, System.Drawing.FontStyle.Regular);
            this.panelControls.Controls.Add(this.lblConnectionStatus);

            // Button controls
            this.btnGetProducts.Text = "Get Products";
            this.btnGetProducts.Location = new System.Drawing.Point(15, 50);
            this.btnGetProducts.Size = new System.Drawing.Size(120, 40);
            this.btnGetProducts.Click += new System.EventHandler(this.BtnGetProducts_Click);
            this.panelControls.Controls.Add(this.btnGetProducts);

            this.btnExportToCsv.Text = "Export to CSV";
            this.btnExportToCsv.Location = new System.Drawing.Point(145, 50);
            this.btnExportToCsv.Size = new System.Drawing.Size(120, 40);
            this.btnExportToCsv.Click += new System.EventHandler(this.BtnExportToCsv_Click);
            this.panelControls.Controls.Add(this.btnExportToCsv);

            this.btnClearGrid.Text = "Clear";
            this.btnClearGrid.Location = new System.Drawing.Point(275, 50);
            this.btnClearGrid.Size = new System.Drawing.Size(80, 40);
            this.btnClearGrid.Click += new System.EventHandler(this.BtnClearGrid_Click);
            this.panelControls.Controls.Add(this.btnClearGrid);

            // Add status label for query progress
            this.lblQueryStatus.Location = new System.Drawing.Point(365, 50);
            this.lblQueryStatus.Size = new System.Drawing.Size(300, 40);
            this.lblQueryStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblQueryStatus.ForeColor = System.Drawing.Color.DarkBlue;
            this.panelControls.Controls.Add(this.lblQueryStatus);

            // Stock Count information grid
            this.dgvStockCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvStockCount.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvStockCount.AllowUserToOrderColumns = false;
            this.dgvStockCount.AllowUserToAddRows = false;
            this.dgvStockCount.AllowUserToDeleteRows = false;
            this.dgvStockCount.ReadOnly = false; // This must be false to allow editing the Count column
            this.dgvStockCount.RowHeadersVisible = false;
            this.dgvStockCount.AlternatingRowsDefaultCellStyle = new System.Windows.Forms.DataGridViewCellStyle { BackColor = System.Drawing.Color.AliceBlue };
            this.dgvStockCount.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect; // Use CellSelect to make it easier to edit cells
            this.dgvStockCount.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter; // Start editing when cell is clicked
            this.dgvStockCount.EnableHeadersVisualStyles = true; // Enable styling for headers

            // Add controls to form
            this.Controls.Add(this.dgvStockCount);
            this.Controls.Add(this.panelControls);

            // Set tab index
            this.btnGetProducts.TabIndex = 0;
            this.btnExportToCsv.TabIndex = 1;
            this.btnClearGrid.TabIndex = 2;

            ((System.ComponentModel.ISupportInitialize)(this.dgvStockCount)).EndInit();
            this.panelControls.ResumeLayout(false);
            this.panelControls.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        // Form controls
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblConnectionStatus;
        private System.Windows.Forms.Button btnGetProducts;
        private System.Windows.Forms.Button btnExportToCsv;
        private System.Windows.Forms.Button btnClearGrid;
        private System.Windows.Forms.DataGridView dgvStockCount;
        private System.Windows.Forms.Panel panelControls;
        private System.Windows.Forms.Label lblQueryStatus;
    }
}