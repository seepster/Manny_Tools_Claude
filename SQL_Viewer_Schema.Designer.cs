namespace Manny_Tools_Claude
{
    partial class SQL_Viewer_Schema
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
            // Create main container with white background
            this.BackColor = SystemColors.Control;

            // Create header panel
            panelHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.FromArgb(240, 240, 240)
            };

            // Create title and instructions
            lblTitle = new Label
            {
                Text = "Database Schema Viewer",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Location = new Point(10, 10),
                AutoSize = true
            };

            lblInstructions = new Label
            {
                Text = "Select a table from the list to view its structure and data.",
                Location = new Point(10, 35),
                Size = new Size(850, 20),
                Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top
            };

            // Add refresh button
            btnRefresh = new Button
            {
                Text = "Refresh Tables",
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(950, 15),
                Size = new Size(100, 30)
            };

            // Add status label
            lblStatus = new Label
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                TextAlign = ContentAlignment.MiddleRight,
                Location = new Point(600, 35),
                Size = new Size(350, 20),
                ForeColor = Color.DarkBlue
            };

            // Add controls to header panel
            panelHeader.Controls.Add(lblTitle);
            panelHeader.Controls.Add(lblInstructions);
            panelHeader.Controls.Add(btnRefresh);
            panelHeader.Controls.Add(lblStatus);

            // Content container to hold both the left panel and right content
            Panel contentContainer = new Panel
            {
                Dock = DockStyle.Fill
            };

            // Create split container for main content
            splitContainer = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Horizontal,
                SplitterDistance = 300,
                Panel1MinSize = 100,
                Panel2MinSize = 100
            };

            // Create left panel for table list
            panelLeft = new Panel
            {
                Dock = DockStyle.Left,
                Width = 250,
                BackColor = Color.FromArgb(245, 245, 245)
            };

            lblTableList = new Label
            {
                Text = "Database Tables",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Dock = DockStyle.Top,
                Padding = new Padding(10, 10, 10, 5),
                Height = 30
            };

            lstTables = new ListBox
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(10, 0, 10, 10),
                BorderStyle = BorderStyle.FixedSingle
            };

            panelLeft.Controls.Add(lstTables);
            panelLeft.Controls.Add(lblTableList);

            // Create right panel for table details
            panelRight = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(10)
            };

            // Table Fields Section
            lblTableFields = new Label
            {
                Text = "Table Fields",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Dock = DockStyle.Top,
                Padding = new Padding(0, 0, 0, 5)
            };

            dgvFields = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                RowHeadersVisible = false,
                AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.AliceBlue }
            };

            Panel panel1Container = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(0, 25, 0, 0)
            };

            panel1Container.Controls.Add(dgvFields);
            splitContainer.Panel1.Controls.Add(panel1Container);
            splitContainer.Panel1.Controls.Add(lblTableFields);

            // Table Data Section
            lblTableData = new Label
            {
                Text = "Table Data (Last 10 Records)",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Dock = DockStyle.Top,
                Padding = new Padding(0, 0, 0, 5)
            };

            dgvTableData = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                RowHeadersVisible = false,
                AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.AliceBlue },
                AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells,
                AllowUserToResizeColumns = true,
                AllowUserToResizeRows = false,
                ScrollBars = ScrollBars.Both
            };

            Panel panel2Container = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(0, 25, 0, 0)
            };

            panel2Container.Controls.Add(dgvTableData);
            splitContainer.Panel2.Controls.Add(panel2Container);
            splitContainer.Panel2.Controls.Add(lblTableData);

            // Add panels to the form
            panelRight.Controls.Add(splitContainer);
            contentContainer.Controls.Add(panelRight);
            contentContainer.Controls.Add(panelLeft);

            this.Controls.Add(contentContainer);
            this.Controls.Add(panelHeader);

            // Wire up events  
            lstTables.SelectedIndexChanged += LstTables_SelectedIndexChanged;
            btnRefresh.Click += BtnRefresh_Click;
        }

        #endregion

        // Form controls
        private Label lblTitle;
        private Label lblInstructions;
        private Label lblTableList;
        private ListBox lstTables;
        private Label lblTableFields;
        private DataGridView dgvFields;
        private Label lblTableData;
        private DataGridView dgvTableData;
        private Button btnRefresh;
        private Panel panelHeader;
        private Panel panelLeft;
        private Panel panelRight;
        private SplitContainer splitContainer;
        private Label lblStatus;
    }
}