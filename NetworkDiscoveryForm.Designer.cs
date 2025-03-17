namespace Manny_Tools_Claude
{
    partial class NetworkDiscoveryForm
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
            splitContainer = new SplitContainer();
            treeViewNetwork = new TreeView();
            panel1 = new Panel();
            lblNodesFound = new Label();
            checkBoxScanCommonPorts = new CheckBox();
            checkBoxNamedInstances = new CheckBox();
            lblStatus = new Label();
            btnCancel = new Button();
            btnScan = new Button();
            lblTitle = new Label();
            detailsPanel = new Panel();
            lblInfoHeader = new Label();
            panelNodeDetails = new Panel();
            btnUseConnection = new Button();
            btnTestConnection = new Button();
            dgvSqlInstances = new DataGridView();
            colInstanceName = new DataGridViewTextBoxColumn();
            colPort = new DataGridViewTextBoxColumn();
            colVersion = new DataGridViewTextBoxColumn();
            colStatus = new DataGridViewTextBoxColumn();
            lblSqlInstances = new Label();
            txtMachineName = new TextBox();
            lblMachineName = new Label();
            txtIPAddress = new TextBox();
            lblIPAddress = new Label();
            progressBar = new ProgressBar();
            statusStrip = new StatusStrip();
            toolStripStatusLabel = new ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)splitContainer).BeginInit();
            splitContainer.Panel1.SuspendLayout();
            splitContainer.Panel2.SuspendLayout();
            splitContainer.SuspendLayout();
            panel1.SuspendLayout();
            detailsPanel.SuspendLayout();
            panelNodeDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvSqlInstances).BeginInit();
            statusStrip.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer
            // 
            splitContainer.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            splitContainer.Location = new Point(12, 12);
            splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            splitContainer.Panel1.Controls.Add(treeViewNetwork);
            splitContainer.Panel1.Controls.Add(panel1);
            // 
            // splitContainer.Panel2
            // 
            splitContainer.Panel2.Controls.Add(detailsPanel);
            splitContainer.Size = new Size(976, 526);
            splitContainer.SplitterDistance = 325;
            splitContainer.TabIndex = 0;
            // 
            // treeViewNetwork
            // 
            treeViewNetwork.Dock = DockStyle.Fill;
            treeViewNetwork.Location = new Point(0, 150);
            treeViewNetwork.Name = "treeViewNetwork";
            treeViewNetwork.Size = new Size(325, 376);
            treeViewNetwork.TabIndex = 1;
            treeViewNetwork.AfterSelect += TreeViewNetwork_AfterSelect;
            // 
            // panel1
            // 
            panel1.Controls.Add(lblNodesFound);
            panel1.Controls.Add(checkBoxScanCommonPorts);
            panel1.Controls.Add(checkBoxNamedInstances);
            panel1.Controls.Add(lblStatus);
            panel1.Controls.Add(btnCancel);
            panel1.Controls.Add(btnScan);
            panel1.Controls.Add(lblTitle);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(325, 150);
            panel1.TabIndex = 0;
            // 
            // lblNodesFound
            // 
            lblNodesFound.AutoSize = true;
            lblNodesFound.Location = new Point(13, 127);
            lblNodesFound.Name = "lblNodesFound";
            lblNodesFound.Size = new Size(90, 15);
            lblNodesFound.TabIndex = 6;
            lblNodesFound.Text = "Nodes Found: 0";
            // 
            // checkBoxScanCommonPorts
            // 
            checkBoxScanCommonPorts.AutoSize = true;
            checkBoxScanCommonPorts.Checked = true;
            checkBoxScanCommonPorts.CheckState = CheckState.Checked;
            checkBoxScanCommonPorts.Location = new Point(13, 70);
            checkBoxScanCommonPorts.Name = "checkBoxScanCommonPorts";
            checkBoxScanCommonPorts.Size = new Size(159, 19);
            checkBoxScanCommonPorts.TabIndex = 5;
            checkBoxScanCommonPorts.Text = "Scan Common SQL Ports";
            checkBoxScanCommonPorts.UseVisualStyleBackColor = true;
            // 
            // checkBoxNamedInstances
            // 
            checkBoxNamedInstances.AutoSize = true;
            checkBoxNamedInstances.Checked = true;
            checkBoxNamedInstances.CheckState = CheckState.Checked;
            checkBoxNamedInstances.Location = new Point(13, 45);
            checkBoxNamedInstances.Name = "checkBoxNamedInstances";
            checkBoxNamedInstances.Size = new Size(163, 19);
            checkBoxNamedInstances.TabIndex = 4;
            checkBoxNamedInstances.Text = "Scan for Named Instances";
            checkBoxNamedInstances.UseVisualStyleBackColor = true;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(13, 103);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(146, 15);
            lblStatus.TabIndex = 3;
            lblStatus.Text = "Ready to scan for servers...";
            // 
            // btnCancel
            // 
            btnCancel.Enabled = false;
            btnCancel.Location = new Point(236, 113);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 29);
            btnCancel.TabIndex = 2;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += BtnCancel_Click;
            // 
            // btnScan
            // 
            btnScan.Location = new Point(178, 45);
            btnScan.Name = "btnScan";
            btnScan.Size = new Size(133, 39);
            btnScan.TabIndex = 1;
            btnScan.Text = "Start Network Scan";
            btnScan.UseVisualStyleBackColor = true;
            btnScan.Click += BtnScan_Click;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitle.Location = new Point(13, 9);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(177, 21);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "SQL Server Discoverer";
            // 
            // detailsPanel
            // 
            detailsPanel.Controls.Add(lblInfoHeader);
            detailsPanel.Controls.Add(panelNodeDetails);
            detailsPanel.Dock = DockStyle.Fill;
            detailsPanel.Location = new Point(0, 0);
            detailsPanel.Name = "detailsPanel";
            detailsPanel.Size = new Size(647, 526);
            detailsPanel.TabIndex = 0;
            // 
            // lblInfoHeader
            // 
            lblInfoHeader.AutoSize = true;
            lblInfoHeader.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblInfoHeader.Location = new Point(16, 9);
            lblInfoHeader.Name = "lblInfoHeader";
            lblInfoHeader.Size = new Size(195, 21);
            lblInfoHeader.TabIndex = 1;
            lblInfoHeader.Text = "Node and Server Details";
            // 
            // panelNodeDetails
            // 
            panelNodeDetails.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panelNodeDetails.Controls.Add(btnUseConnection);
            panelNodeDetails.Controls.Add(btnTestConnection);
            panelNodeDetails.Controls.Add(dgvSqlInstances);
            panelNodeDetails.Controls.Add(lblSqlInstances);
            panelNodeDetails.Controls.Add(txtMachineName);
            panelNodeDetails.Controls.Add(lblMachineName);
            panelNodeDetails.Controls.Add(txtIPAddress);
            panelNodeDetails.Controls.Add(lblIPAddress);
            panelNodeDetails.Enabled = false;
            panelNodeDetails.Location = new Point(20, 45);
            panelNodeDetails.Name = "panelNodeDetails";
            panelNodeDetails.Size = new Size(614, 468);
            panelNodeDetails.TabIndex = 0;
            // 
            // btnUseConnection
            // 
            btnUseConnection.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnUseConnection.Enabled = false;
            btnUseConnection.Location = new Point(457, 427);
            btnUseConnection.Name = "btnUseConnection";
            btnUseConnection.Size = new Size(145, 29);
            btnUseConnection.TabIndex = 7;
            btnUseConnection.Text = "Use Connection";
            btnUseConnection.UseVisualStyleBackColor = true;
            btnUseConnection.Click += BtnUseConnection_Click;
            // 
            // btnTestConnection
            // 
            btnTestConnection.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnTestConnection.Enabled = false;
            btnTestConnection.Location = new Point(306, 427);
            btnTestConnection.Name = "btnTestConnection";
            btnTestConnection.Size = new Size(145, 29);
            btnTestConnection.TabIndex = 6;
            btnTestConnection.Text = "Test Connection";
            btnTestConnection.UseVisualStyleBackColor = true;
            btnTestConnection.Click += BtnTestConnection_Click;
            // 
            // dgvSqlInstances
            // 
            dgvSqlInstances.AllowUserToAddRows = false;
            dgvSqlInstances.AllowUserToDeleteRows = false;
            dgvSqlInstances.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvSqlInstances.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvSqlInstances.Columns.AddRange(new DataGridViewColumn[] { colInstanceName, colPort, colVersion, colStatus });
            dgvSqlInstances.Location = new Point(16, 119);
            dgvSqlInstances.Name = "dgvSqlInstances";
            dgvSqlInstances.ReadOnly = true;
            dgvSqlInstances.RowHeadersVisible = false;
            dgvSqlInstances.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSqlInstances.Size = new Size(586, 293);
            dgvSqlInstances.TabIndex = 5;
            dgvSqlInstances.SelectionChanged += DgvSqlInstances_SelectionChanged;
            // 
            // colInstanceName
            // 
            colInstanceName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colInstanceName.HeaderText = "Instance Name";
            colInstanceName.Name = "colInstanceName";
            colInstanceName.ReadOnly = true;
            // 
            // colPort
            // 
            colPort.HeaderText = "Port";
            colPort.Name = "colPort";
            colPort.ReadOnly = true;
            colPort.Width = 60;
            // 
            // colVersion
            // 
            colVersion.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colVersion.HeaderText = "Version";
            colVersion.Name = "colVersion";
            colVersion.ReadOnly = true;
            // 
            // colStatus
            // 
            colStatus.HeaderText = "Status";
            colStatus.Name = "colStatus";
            colStatus.ReadOnly = true;
            colStatus.Width = 120;
            // 
            // lblSqlInstances
            // 
            lblSqlInstances.AutoSize = true;
            lblSqlInstances.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblSqlInstances.Location = new Point(13, 101);
            lblSqlInstances.Name = "lblSqlInstances";
            lblSqlInstances.Size = new Size(175, 15);
            lblSqlInstances.TabIndex = 4;
            lblSqlInstances.Text = "SQL Server Instances Found: 0";
            // 
            // txtMachineName
            // 
            txtMachineName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtMachineName.Location = new Point(104, 49);
            txtMachineName.Name = "txtMachineName";
            txtMachineName.ReadOnly = true;
            txtMachineName.Size = new Size(498, 23);
            txtMachineName.TabIndex = 3;
            // 
            // lblMachineName
            // 
            lblMachineName.AutoSize = true;
            lblMachineName.Location = new Point(13, 52);
            lblMachineName.Name = "lblMachineName";
            lblMachineName.Size = new Size(91, 15);
            lblMachineName.TabIndex = 2;
            lblMachineName.Text = "Machine Name:";
            // 
            // txtIPAddress
            // 
            txtIPAddress.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtIPAddress.Location = new Point(104, 17);
            txtIPAddress.Name = "txtIPAddress";
            txtIPAddress.ReadOnly = true;
            txtIPAddress.Size = new Size(498, 23);
            txtIPAddress.TabIndex = 1;
            // 
            // lblIPAddress
            // 
            lblIPAddress.AutoSize = true;
            lblIPAddress.Location = new Point(13, 20);
            lblIPAddress.Name = "lblIPAddress";
            lblIPAddress.Size = new Size(65, 15);
            lblIPAddress.TabIndex = 0;
            lblIPAddress.Text = "IP Address:";
            // 
            // progressBar
            // 
            progressBar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            progressBar.Location = new Point(12, 544);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(976, 23);
            progressBar.TabIndex = 1;
            // 
            // statusStrip
            // 
            statusStrip.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel });
            statusStrip.Location = new Point(0, 578);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new Size(1000, 22);
            statusStrip.TabIndex = 2;
            statusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            toolStripStatusLabel.Name = "toolStripStatusLabel";
            toolStripStatusLabel.Size = new Size(39, 17);
            toolStripStatusLabel.Text = "Ready";
            // 
            // NetworkDiscoveryForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1000, 600);
            Controls.Add(statusStrip);
            Controls.Add(progressBar);
            Controls.Add(splitContainer);
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            MinimumSize = new Size(800, 600);
            Name = "NetworkDiscoveryForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "SQL Server Network Discovery";
            FormClosing += NetworkDiscoveryForm_FormClosing;
            splitContainer.Panel1.ResumeLayout(false);
            splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer).EndInit();
            splitContainer.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            detailsPanel.ResumeLayout(false);
            detailsPanel.PerformLayout();
            panelNodeDetails.ResumeLayout(false);
            panelNodeDetails.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvSqlInstances).EndInit();
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.TreeView treeViewNetwork;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnScan;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.CheckBox checkBoxNamedInstances;
        private System.Windows.Forms.CheckBox checkBoxScanCommonPorts;
        private System.Windows.Forms.Panel detailsPanel;
        private System.Windows.Forms.Panel panelNodeDetails;
        private System.Windows.Forms.TextBox txtIPAddress;
        private System.Windows.Forms.Label lblIPAddress;
        private System.Windows.Forms.TextBox txtMachineName;
        private System.Windows.Forms.Label lblMachineName;
        private System.Windows.Forms.Label lblSqlInstances;
        private System.Windows.Forms.DataGridView dgvSqlInstances;
        private System.Windows.Forms.Label lblInfoHeader;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.Button btnUseConnection;
        private System.Windows.Forms.Button btnTestConnection;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInstanceName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPort;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVersion;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStatus;
        private System.Windows.Forms.Label lblNodesFound;
    }
}