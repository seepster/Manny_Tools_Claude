using System;
using System.Collections.Generic;
using System.Net;
using System.Windows.Forms;

namespace Manny_Tools_Claude
{
    public partial class IPAddressSelectForm : Form
    {
        private List<IPAddressInfo> _ipAddresses;

        /// <summary>
        /// Gets the selected IP address
        /// </summary>
        public IPAddress SelectedIPAddress { get; private set; }

        /// <summary>
        /// Initializes a new instance of the IPAddressSelectForm class
        /// </summary>
        /// <param name="ipAddresses">The list of available IP addresses</param>
        public IPAddressSelectForm(List<IPAddressInfo> ipAddresses)
        {
            _ipAddresses = ipAddresses;
            InitializeComponent();
            PopulateIPAddresses();
        }

        /// <summary>
        /// Populates the list of IP addresses
        /// </summary>
        private void PopulateIPAddresses()
        {
            foreach (var ip in _ipAddresses)
            {
                lstIPAddresses.Items.Add($"{ip.IPAddress} - {ip.InterfaceName} ({ip.Description})");
            }

            lstIPAddresses.SelectedIndexChanged += (sender, e) => {
                btnOK.Enabled = lstIPAddresses.SelectedIndex >= 0;
            };
        }

        /// <summary>
        /// Handles the OK button click
        /// </summary>
        private void BtnOK_Click(object sender, EventArgs e)
        {
            if (lstIPAddresses.SelectedIndex >= 0)
            {
                SelectedIPAddress = _ipAddresses[lstIPAddresses.SelectedIndex].IPAddress;
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        /// <summary>
        /// Handles double-clicking an item in the list
        /// </summary>
        private void LstIPAddresses_DoubleClick(object sender, EventArgs e)
        {
            if (lstIPAddresses.SelectedIndex >= 0)
            {
                SelectedIPAddress = _ipAddresses[lstIPAddresses.SelectedIndex].IPAddress;
                DialogResult = DialogResult.OK;
                Close();
            }
        }
    }
}