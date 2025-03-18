namespace Manny_Tools_Claude
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabViewSQL = new System.Windows.Forms.TabPage();
            this.tabCreateSizes = new System.Windows.Forms.TabPage();
            this.tabStockOnHand = new System.Windows.Forms.TabPage();
            this.tabOrbitSizing = new System.Windows.Forms.TabPage();
            this.headerPanel = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnSettings = new System.Windows.Forms.Button();
            this.btnUserManagement = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabControl.SuspendLayout();
            this.headerPanel.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabViewSQL);
            this.tabControl.Controls.Add(this.tabCreateSizes);
            this.tabControl.Controls.Add(this.tabStockOnHand);
            this.tabControl.Controls.Add(this.tabOrbitSizing);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(5, 65);
            this.tabControl.Name = "tabControl";
            this.tabControl.Padding = new System.Drawing.Point(15, 8);
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1190, 708);
            this.tabControl.TabIndex = 0;
            // 
            // tabViewSQL
            // 
            this.tabViewSQL.Location = new System.Drawing.Point(4, 33);
            this.tabViewSQL.Name = "tabViewSQL";
            this.tabViewSQL.Padding = new System.Windows.Forms.Padding(5);
            this.tabViewSQL.Size = new System.Drawing.Size(1182, 671);
            this.tabViewSQL.TabIndex = 0;
            this.tabViewSQL.Text = "View SQL";
            this.tabViewSQL.UseVisualStyleBackColor = true;
            // 
            // tabCreateSizes
            // 
            this.tabCreateSizes.Location = new System.Drawing.Point(4, 33);
            this.tabCreateSizes.Name = "tabCreateSizes";
            this.tabCreateSizes.Padding = new System.Windows.Forms.Padding(5);
            this.tabCreateSizes.Size = new System.Drawing.Size(1182, 671);
            this.tabCreateSizes.TabIndex = 1;
            this.tabCreateSizes.Text = "Create Sizes";
            this.tabCreateSizes.UseVisualStyleBackColor = true;
            // 
            // tabStockOnHand
            // 
            this.tabStockOnHand.Location = new System.Drawing.Point(4, 33);
            this.tabStockOnHand.Name = "tabStockOnHand";
            this.tabStockOnHand.Padding = new System.Windows.Forms.Padding(5);
            this.tabStockOnHand.Size = new System.Drawing.Size(1182, 671);
            this.tabStockOnHand.TabIndex = 2;
            this.tabStockOnHand.Text = "Stock On Hand";
            this.tabStockOnHand.UseVisualStyleBackColor = true;
            // 
            // tabOrbitSizing
            // 
            this.tabOrbitSizing.Location = new System.Drawing.Point(4, 33);
            this.tabOrbitSizing.Name = "tabOrbitSizing";
            this.tabOrbitSizing.Padding = new System.Windows.Forms.Padding(5);
            this.tabOrbitSizing.Size = new System.Drawing.Size(1182, 671);
            this.tabOrbitSizing.TabIndex = 3;
            this.tabOrbitSizing.Text = "Orbit Sizing Method";
            this.tabOrbitSizing.UseVisualStyleBackColor = true;
            // 
            // headerPanel
            // 
            this.headerPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.headerPanel.Controls.Add(this.lblTitle);
            this.headerPanel.Controls.Add(this.btnSettings);
            this.headerPanel.Controls.Add(this.btnUserManagement);
            this.headerPanel.Controls.Add(this.btnLogout);
            this.headerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerPanel.Location = new System.Drawing.Point(5, 5);
            this.headerPanel.Name = "headerPanel";
            this.headerPanel.Padding = new System.Windows.Forms.Padding(10);
            this.headerPanel.Size = new System.Drawing.Size(1190, 60);
            this.headerPanel.TabIndex = 1;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(15, 15);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(156, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Manny Tools";
            // 
            // btnSettings
            // 
            this.btnSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSettings.FlatAppearance.BorderSize = 1;
            this.btnSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSettings.Location = new System.Drawing.Point(1025, 15);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(150, 30);
            this.btnSettings.TabIndex = 1;
            this.btnSettings.Text = "Connection Settings";
            this.btnSettings.UseVisualStyleBackColor = true;
            this.btnSettings.Click += new System.EventHandler(this.BtnSettings_Click);
            // 
            // btnUserManagement
            // 
            this.btnUserManagement.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUserManagement.Location = new System.Drawing.Point(865, 15);
            this.btnUserManagement.Name = "btnUserManagement";
            this.btnUserManagement.Size = new System.Drawing.Size(150, 30);
            this.btnUserManagement.TabIndex = 2;
            this.btnUserManagement.Text = "User Management";
            this.btnUserManagement.UseVisualStyleBackColor = true;
            this.btnUserManagement.Click += new System.EventHandler(this.BtnUserManagement_Click);
            // 
            // btnLogout
            // 
            this.btnLogout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLogout.Location = new System.Drawing.Point(755, 15);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(100, 30);
            this.btnLogout.TabIndex = 3;
            this.btnLogout.Text = "Logout";
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.BtnLogout_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.statusStrip.Location = new System.Drawing.Point(5, 773);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1190, 22);
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(39, 17);
            this.lblStatus.Text = "Status";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ClientSize = new System.Drawing.Size(1200, 800);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.headerPanel);
            this.Controls.Add(this.statusStrip);
            this.Name = "MainForm";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Manny Tools";
            this.SizeChanged += new System.EventHandler(this.MainForm_SizeChanged);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.tabControl.ResumeLayout(false);
            this.headerPanel.ResumeLayout(false);
            this.headerPanel.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabViewSQL;
        private System.Windows.Forms.TabPage tabCreateSizes;
        private System.Windows.Forms.TabPage tabStockOnHand;
        private System.Windows.Forms.TabPage tabOrbitSizing;
        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.Button btnUserManagement;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
    }
}