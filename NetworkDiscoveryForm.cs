using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using Microsoft.Data.SqlClient;

namespace Manny_Tools_Claude
{
    public partial class NetworkDiscoveryForm : Form
    {
        #region Fields

        private NetworkDiscovery _networkDiscovery;
        private bool _isScanning = false;
        private NetworkNode _selectedNode;
        private SqlInstance _selectedInstance;
        private string _connectionString;

        // Constants for tree view node types
        private const string NODE_TYPE_LOCAL = "LocalNode";
        private const string NODE_TYPE_REMOTE = "RemoteNode";
        private const string NODE_TYPE_SQL = "SqlInstance";

        #endregion

        #region Constructor

        public NetworkDiscoveryForm()
        {
            InitializeComponent();
            InitializeNetworkDiscovery();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the selected connection string (used when returning a selection)
        /// </summary>
        public string SelectedConnectionString
        {
            get { return _connectionString; }
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Initializes the network discovery components
        /// </summary>
        private void InitializeNetworkDiscovery()
        {
            _networkDiscovery = new NetworkDiscovery();

            // Explicitly unsubscribe first to avoid duplicate event handlers
            _networkDiscovery.DiscoveryProgress -= NetworkDiscovery_DiscoveryProgress;
            _networkDiscovery.DiscoveryCompleted -= NetworkDiscovery_DiscoveryCompleted;

            // Now subscribe to events
            _networkDiscovery.DiscoveryProgress += NetworkDiscovery_DiscoveryProgress;
            _networkDiscovery.DiscoveryCompleted += NetworkDiscovery_DiscoveryCompleted;
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handles the discovery progress event
        /// </summary>
        private void NetworkDiscovery_DiscoveryProgress(object sender, NetworkDiscoveryProgressEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => NetworkDiscovery_DiscoveryProgress(sender, e)));
                return;
            }

            // Update status label
            lblStatus.Text = e.StatusMessage;
            toolStripStatusLabel.Text = e.StatusMessage;

            // Update progress bar
            if (e.TotalProgress > 0)
            {
                progressBar.Maximum = e.TotalProgress;
                progressBar.Value = Math.Min(e.CurrentProgress, e.TotalProgress);
            }

            // Update nodes found count
            lblNodesFound.Text = $"Nodes Found: {_networkDiscovery.DiscoveredNodes.Count}";

            // If a new node was discovered, add it to the tree view
            if (e.CurrentNode != null && e.CurrentNode.IsResponding)
            {
                // Check if the node is already in the tree
                string nodeKey = $"Node_{e.CurrentNode.IPAddress}";
                TreeNode[] existingNodes = treeViewNetwork.Nodes.Find(nodeKey, false);

                if (existingNodes.Length == 0)
                {
                    // Add node to tree regardless of whether it has SQL instances
                    AddNodeToTreeView(e.CurrentNode);
                }
                else
                {
                    // Update existing node
                    UpdateNodeInTreeView(existingNodes[0], e.CurrentNode);
                }

                // Force UI update to show the node immediately
                treeViewNetwork.Update();
            }

            Application.DoEvents();
        }

        /// <summary>
        /// Handles the discovery completed event
        /// </summary>
        private void NetworkDiscovery_DiscoveryCompleted(object sender, NetworkDiscoveryCompletedEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => NetworkDiscovery_DiscoveryCompleted(sender, e)));
                return;
            }

            // Update UI
            _isScanning = false;
            btnScan.Enabled = true;
            btnCancel.Enabled = false;
            progressBar.Value = progressBar.Maximum;

            // Update status
            int nodesWithSql = e.DiscoveredNodes.Count(n => n.SqlInstances.Count > 0);
            string completionMessage = $"{e.CompletionMessage} Found {nodesWithSql} nodes with SQL instances.";

            lblStatus.Text = completionMessage;
            toolStripStatusLabel.Text = completionMessage;

            // Update nodes found count
            lblNodesFound.Text = $"Nodes Found: {e.DiscoveredNodes.Count}";

            // Populate tree with all discovered nodes
            if (e.IsSuccessful)
            {
                treeViewNetwork.Nodes.Clear();

                // Include all responding nodes, not just those with SQL instances
                PopulateTreeView(e.DiscoveredNodes.Where(n => n.IsResponding).ToList());
            }
        }

        /// <summary>
        /// Handles the Start Scan button click
        /// </summary>
        private void BtnScan_Click(object sender, EventArgs e)
        {
            if (_isScanning)
                return;

            // Get local IP addresses
            List<IPAddressInfo> ipAddresses = _networkDiscovery.GetLocalIPAddresses();

            if (ipAddresses.Count == 0)
            {
                MessageBox.Show("No network adapters found. Cannot perform network discovery.",
                    "Network Discovery", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // If there's only one IP address, use it directly
            if (ipAddresses.Count == 1)
            {
                StartDiscovery(ipAddresses[0].IPAddress);
                return;
            }

            // Show dialog to select IP address
            using (IPAddressSelectForm ipForm = new IPAddressSelectForm(ipAddresses))
            {
                if (ipForm.ShowDialog() == DialogResult.OK)
                {
                    StartDiscovery(ipForm.SelectedIPAddress);
                }
            }
        }

        /// <summary>
        /// Starts the discovery process
        /// </summary>
        private void StartDiscovery(IPAddress ipAddress)
        {
            // Clear existing tree
            treeViewNetwork.Nodes.Clear();

            // Reset details panel
            ClearDetailsPanel();

            // Configure discovery options
            _networkDiscovery.ScanForNamedInstances = checkBoxNamedInstances.Checked;
            _networkDiscovery.ScanCommonSqlPorts = checkBoxScanCommonPorts.Checked;

            // Update UI
            _isScanning = true;
            btnScan.Enabled = false;
            btnCancel.Enabled = true;
            progressBar.Value = 0;
            lblStatus.Text = $"Starting network discovery from {ipAddress}...";
            toolStripStatusLabel.Text = $"Starting network discovery from {ipAddress}...";

            // Start the discovery process
            Task.Run(async () => await _networkDiscovery.StartDiscoveryAsync(ipAddress));
        }

        /// <summary>
        /// Handles the Cancel button click
        /// </summary>
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            if (!_isScanning)
                return;

            lblStatus.Text = "Cancelling discovery...";
            toolStripStatusLabel.Text = "Cancelling discovery...";
            _networkDiscovery.CancelDiscovery();

            btnCancel.Enabled = false;
        }

        /// <summary>
        /// Handles the tree view node selection
        /// </summary>
        private void TreeViewNetwork_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // Clear current selection
            _selectedNode = null;
            _selectedInstance = null;

            // Determine the type of the selected node
            if (e.Node.Tag is TreeNodeTag tag)
            {
                switch (tag.NodeType)
                {
                    case NODE_TYPE_LOCAL:
                    case NODE_TYPE_REMOTE:
                        // Network node selected
                        _selectedNode = tag.Node;
                        DisplayNodeDetails(_selectedNode);
                        break;

                    case NODE_TYPE_SQL:
                        // SQL instance selected
                        _selectedNode = tag.Node;
                        _selectedInstance = tag.SqlInstance;
                        DisplayNodeDetails(_selectedNode, _selectedInstance);
                        break;
                }
            }
        }

        /// <summary>
        /// Handles the SQL instances data grid selection
        /// </summary>
        private void DgvSqlInstances_SelectionChanged(object sender, EventArgs e)
        {
            btnTestConnection.Enabled = dgvSqlInstances.SelectedRows.Count > 0;
            btnUseConnection.Enabled = false;

            if (dgvSqlInstances.SelectedRows.Count > 0 && _selectedNode != null)
            {
                int index = dgvSqlInstances.SelectedRows[0].Index;
                if (index >= 0 && index < _selectedNode.SqlInstances.Count)
                {
                    _selectedInstance = _selectedNode.SqlInstances[index];
                }
            }
        }

        /// <summary>
        /// Handles the Test Connection button click
        /// </summary>
        private void BtnTestConnection_Click(object sender, EventArgs e)
        {
            if (_selectedNode != null && _selectedInstance != null)
            {
                TestSqlConnection(_selectedNode, _selectedInstance);
            }
        }

        /// <summary>
        /// Handles the Use Connection button click
        /// </summary>
        private void BtnUseConnection_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_connectionString))
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        /// <summary>
        /// Ensures we cancel any running discovery when the form is closing
        /// </summary>
        private void NetworkDiscoveryForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_isScanning)
            {
                _networkDiscovery.CancelDiscovery();
            }
        }

        #endregion

        #region Tree View Utilities

        /// <summary>
        /// Populates the tree view with discovered nodes
        /// </summary>
        private void PopulateTreeView(List<NetworkNode> nodes)
        {
            // Make sure we're on the UI thread
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => PopulateTreeView(nodes)));
                return;
            }

            if (nodes.Count == 0)
            {
                // No nodes to display
                lblStatus.Text = "No responding hosts were found on the network.";
                toolStripStatusLabel.Text = "No responding hosts were found on the network.";
                return;
            }

            treeViewNetwork.BeginUpdate();

            try
            {
                // First add local nodes
                foreach (var node in nodes.Where(n => n.IsLocalMachine))
                {
                    AddNodeToTreeView(node);
                }

                // Then add remote nodes
                foreach (var node in nodes.Where(n => !n.IsLocalMachine))
                {
                    AddNodeToTreeView(node);
                }

                // Expand all nodes
                treeViewNetwork.ExpandAll();
            }
            finally
            {
                treeViewNetwork.EndUpdate();
            }
        }

        /// <summary>
        /// Adds a network node to the tree view
        /// </summary>
        private void AddNodeToTreeView(NetworkNode node)
        {
            // Create a new tree node
            string nodeKey = $"Node_{node.IPAddress}";
            string nodeType = node.IsLocalMachine ? NODE_TYPE_LOCAL : NODE_TYPE_REMOTE;
            string nodeText = node.IsLocalMachine ?
                $"{node.Hostname} (Local Machine - {node.IPAddress})" :
                $"{node.Hostname} ({node.IPAddress})";

            TreeNode treeNode = new TreeNode(nodeText);
            treeNode.Name = nodeKey;
            treeNode.Tag = new TreeNodeTag { NodeType = nodeType, Node = node };

            // For local machine nodes, use a different style
            if (node.IsLocalMachine)
            {
                treeNode.ForeColor = Color.Green;
                treeNode.NodeFont = new Font(treeViewNetwork.Font, FontStyle.Bold);
            }

            // Add SQL instances as child nodes (if any)
            foreach (var instance in node.SqlInstances)
            {
                AddSqlInstanceToTreeNode(treeNode, instance);
            }

            // If node has SQL instances, use blue to highlight it
            if (node.SqlInstances.Count > 0)
            {
                treeNode.ForeColor = Color.Blue;
                if (!node.IsLocalMachine) // Don't override local machine styling
                {
                    treeNode.NodeFont = new Font(treeViewNetwork.Font, FontStyle.Bold);
                }
            }

            // Add the node to the tree
            treeViewNetwork.Nodes.Add(treeNode);
        }

        /// <summary>
        /// Updates an existing node in the tree view
        /// </summary>
        private void UpdateNodeInTreeView(TreeNode treeNode, NetworkNode node)
        {
            if (treeNode.Tag is TreeNodeTag tag && tag.Node == node)
            {
                // Clear existing SQL instance nodes
                treeNode.Nodes.Clear();

                // Add SQL instances as child nodes
                foreach (var instance in node.SqlInstances)
                {
                    AddSqlInstanceToTreeNode(treeNode, instance);
                }

                // Update node appearance based on SQL instances
                if (node.SqlInstances.Count > 0 && !node.IsLocalMachine)
                {
                    treeNode.ForeColor = Color.Blue;
                    treeNode.NodeFont = new Font(treeViewNetwork.Font, FontStyle.Bold);
                }
            }
        }

        /// <summary>
        /// Adds a SQL instance as a child node to a tree node
        /// </summary>
        private void AddSqlInstanceToTreeNode(TreeNode parentNode, SqlInstance instance)
        {
            string instanceText = instance.IsNamedInstance ?
                $"{instance.InstanceName} (Port: {instance.Port})" :
                $"Default Instance (Port: {instance.Port})";

            TreeNode instanceNode = new TreeNode(instanceText);
            instanceNode.Tag = new TreeNodeTag
            {
                NodeType = NODE_TYPE_SQL,
                Node = (parentNode.Tag as TreeNodeTag)?.Node,
                SqlInstance = instance
            };

            // Set color based on accessibility
            instanceNode.ForeColor = instance.IsAccessible ? Color.Blue : Color.Red;

            parentNode.Nodes.Add(instanceNode);
        }

        #endregion

        #region Details Panel Utilities

        /// <summary>
        /// Clears the details panel
        /// </summary>
        private void ClearDetailsPanel()
        {
            txtIPAddress.Text = string.Empty;
            txtMachineName.Text = string.Empty;
            lblSqlInstances.Text = "SQL Server Instances Found: 0";

            dgvSqlInstances.Rows.Clear();

            btnTestConnection.Enabled = false;
            btnUseConnection.Enabled = false;

            panelNodeDetails.Enabled = false;
        }

        /// <summary>
        /// Displays details for a selected network node
        /// </summary>
        private void DisplayNodeDetails(NetworkNode node)
        {
            if (node == null)
            {
                ClearDetailsPanel();
                return;
            }

            // Update node information
            txtIPAddress.Text = node.IPAddress;
            txtMachineName.Text = node.Hostname;
            lblSqlInstances.Text = $"SQL Server Instances Found: {node.SqlInstances.Count}";

            // Populate instances grid
            dgvSqlInstances.Rows.Clear();
            foreach (var instance in node.SqlInstances)
            {
                int rowIndex = dgvSqlInstances.Rows.Add();
                var row = dgvSqlInstances.Rows[rowIndex];

                row.Cells[colInstanceName.Index].Value = instance.IsNamedInstance ?
                    $"{instance.ServerName}\\{instance.InstanceName}" :
                    instance.ServerName;

                row.Cells[colPort.Index].Value = instance.Port;
                row.Cells[colVersion.Index].Value = instance.Version ?? "Unknown";
                row.Cells[colStatus.Index].Value = instance.IsAccessible ? "Accessible" : "Not accessible";

                if (!instance.IsAccessible)
                {
                    row.DefaultCellStyle.ForeColor = Color.Red;
                }
            }

            // Enable panel
            panelNodeDetails.Enabled = true;
            btnTestConnection.Enabled = dgvSqlInstances.SelectedRows.Count > 0;
            btnUseConnection.Enabled = false;
        }

        /// <summary>
        /// Displays details for a selected network node and SQL instance
        /// </summary>
        private void DisplayNodeDetails(NetworkNode node, SqlInstance instance)
        {
            // First display the node details
            DisplayNodeDetails(node);

            // Then highlight the selected instance in the grid
            if (instance != null)
            {
                for (int i = 0; i < dgvSqlInstances.Rows.Count; i++)
                {
                    var row = dgvSqlInstances.Rows[i];
                    if (i < node.SqlInstances.Count && node.SqlInstances[i] == instance)
                    {
                        dgvSqlInstances.CurrentCell = row.Cells[0];
                        row.Selected = true;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Tests a connection to a SQL Server instance
        /// </summary>
        private void TestSqlConnection(NetworkNode node, SqlInstance instance)
        {
            Cursor = Cursors.WaitCursor;
            lblStatus.Text = $"Testing connection to {instance.ServerName}...";
            toolStripStatusLabel.Text = $"Testing connection to {instance.ServerName}...";

            try
            {
                // Build the connection string
                string connectionString;
                if (instance.IsNamedInstance)
                {
                    connectionString = $"Server={node.IPAddress}\\{instance.InstanceName};Encrypt=False;TrustServerCertificate=True;Connection Timeout=5;";
                }
                else
                {
                    connectionString = $"Server={node.IPAddress},{instance.Port};Encrypt=False;TrustServerCertificate=True;Connection Timeout=5;";
                }

                // Try to connect
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Check if connected successfully
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        lblStatus.Text = "Connection successful!";
                        toolStripStatusLabel.Text = "Connection successful!";

                        // Store the connection string for later use
                        _connectionString = connectionString;

                        btnUseConnection.Enabled = true;

                        // Show success message
                        MessageBox.Show(
                            "Successfully connected to SQL Server!\n\n" +
                            $"Server: {instance.ServerName}\n" +
                            $"Instance: {instance.InstanceName}\n" +
                            $"Version: {instance.Version}",
                            "Connection Successful",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Connection failed.";
                toolStripStatusLabel.Text = "Connection failed.";

                // Show error message
                MessageBox.Show(
                    $"Failed to connect to SQL Server.\n\nError: {ex.Message}",
                    "Connection Failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        #endregion
    }

    /// <summary>
    /// Helper class for storing node information in tree view nodes
    /// </summary>
    public class TreeNodeTag
    {
        /// <summary>
        /// Gets or sets the type of the node
        /// </summary>
        public string NodeType { get; set; }

        /// <summary>
        /// Gets or sets the network node
        /// </summary>
        public NetworkNode Node { get; set; }

        /// <summary>
        /// Gets or sets the SQL instance
        /// </summary>
        public SqlInstance SqlInstance { get; set; }
    }
}