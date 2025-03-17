namespace Manny_Tools_Claude
{
    partial class OrbitSizingMethodForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.lstTables = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lblTables = new System.Windows.Forms.Label();
            this.panelRight = new System.Windows.Forms.Panel();
            this.dgvTableData = new System.Windows.Forms.DataGridView();
            this.lblTableData = new System.Windows.Forms.Label();
            this.headerPanel = new System.Windows.Forms.Panel();
            this.lblConnectionStatus = new System.Windows.Forms.Label();
            this.mainPanel = new System.Windows.Forms.Panel();
            this.panelLeft.SuspendLayout();
            this.panelRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTableData)).BeginInit();
            this.headerPanel.SuspendLayout();
            this.mainPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(15, 10);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(184, 25);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Orbit Sizing Method";
            // 
            // panelLeft
            // 
            this.panelLeft.Controls.Add(this.lstTables);
            this.panelLeft.Controls.Add(this.lblTables);
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLeft.Location = new System.Drawing.Point(0, 0);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Padding = new System.Windows.Forms.Padding(10);
            this.panelLeft.Size = new System.Drawing.Size(200, 450);
            this.panelLeft.TabIndex = 1;
            // 
            // lstTables
            // 
            this.lstTables.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.lstTables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstTables.FullRowSelect = true;
            this.lstTables.GridLines = true;
            this.lstTables.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lstTables.HideSelection = false;
            this.lstTables.Location = new System.Drawing.Point(10, 35);
            this.lstTables.Name = "lstTables";
            this.lstTables.Size = new System.Drawing.Size(180, 405);
            this.lstTables.TabIndex = 1;
            this.lstTables.UseCompatibleStateImageBehavior = false;
            this.lstTables.View = System.Windows.Forms.View.Details;
            this.lstTables.SelectedIndexChanged += new System.EventHandler(this.LstTables_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Table Name";
            this.columnHeader1.Width = 150;
            // 
            // lblTables
            // 
            this.lblTables.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTables.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTables.Location = new System.Drawing.Point(10, 10);
            this.lblTables.Name = "lblTables";
            this.lblTables.Size = new System.Drawing.Size(180, 25);
            this.lblTables.TabIndex = 0;
            this.lblTables.Text = "Database Tables";
            // 
            // panelRight
            // 
            this.panelRight.Controls.Add(this.dgvTableData);
            this.panelRight.Controls.Add(this.lblTableData);
            this.panelRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRight.Location = new System.Drawing.Point(200, 0);
            this.panelRight.Name = "panelRight";
            this.panelRight.Padding = new System.Windows.Forms.Padding(10);
            this.panelRight.Size = new System.Drawing.Size(600, 450);
            this.panelRight.TabIndex = 2;
            // 
            // dgvTableData
            // 
            this.dgvTableData.AllowUserToAddRows = false;
            this.dgvTableData.AllowUserToDeleteRows = false;
            this.dgvTableData.AllowUserToResizeColumns = true;
            this.dgvTableData.AlternatingRowsDefaultCellStyle = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvTableData.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgvTableData.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dgvTableData.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.dgvTableData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTableData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTableData.Location = new System.Drawing.Point(10, 35);
            this.dgvTableData.Name = "dgvTableData";
            this.dgvTableData.ReadOnly = true;
            this.dgvTableData.RowHeadersVisible = false;
            this.dgvTableData.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.dgvTableData.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTableData.Size = new System.Drawing.Size(580, 405);
            this.dgvTableData.TabIndex = 1;
            // 
            // lblTableData
            // 
            this.lblTableData.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTableData.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTableData.Location = new System.Drawing.Point(10, 10);
            this.lblTableData.Name = "lblTableData";
            this.lblTableData.Size = new System.Drawing.Size(580, 25);
            this.lblTableData.TabIndex = 0;
            this.lblTableData.Text = "Table Data";
            // 
            // headerPanel
            // 
            this.headerPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.headerPanel.Controls.Add(this.lblConnectionStatus);
            this.headerPanel.Controls.Add(this.lblTitle);
            this.headerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerPanel.Location = new System.Drawing.Point(0, 0);
            this.headerPanel.Name = "headerPanel";
            this.headerPanel.Size = new System.Drawing.Size(800, 50);
            this.headerPanel.TabIndex = 3;
            // 
            // lblConnectionStatus
            // 
            this.lblConnectionStatus.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblConnectionStatus.Location = new System.Drawing.Point(300, 15);
            this.lblConnectionStatus.Name = "lblConnectionStatus";
            this.lblConnectionStatus.Size = new System.Drawing.Size(200, 20);
            this.lblConnectionStatus.TabIndex = 1;
            this.lblConnectionStatus.Text = "Connection: Unknown";
            this.lblConnectionStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // mainPanel
            // 
            this.mainPanel.Controls.Add(this.panelRight);
            this.mainPanel.Controls.Add(this.panelLeft);
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Location = new System.Drawing.Point(0, 50);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(800, 450);
            this.mainPanel.TabIndex = 4;
            // 
            // OrbitSizingMethodForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.mainPanel);
            this.Controls.Add(this.headerPanel);
            this.Name = "OrbitSizingMethodForm";
            this.Size = new System.Drawing.Size(800, 500);
            this.panelLeft.ResumeLayout(false);
            this.panelRight.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTableData)).EndInit();
            this.headerPanel.ResumeLayout(false);
            this.headerPanel.PerformLayout();
            this.mainPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.Label lblTables;
        private System.Windows.Forms.ListView lstTables;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Panel panelRight;
        private System.Windows.Forms.Label lblTableData;
        private System.Windows.Forms.DataGridView dgvTableData;
        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.Label lblConnectionStatus;
        private System.Windows.Forms.Panel mainPanel;
    }
}