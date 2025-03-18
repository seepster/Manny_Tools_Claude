using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.ComponentModel;
using Microsoft.Data.SqlClient;

namespace Manny_Tools_Claude
{
    /// <summary>
    /// Provides methods for discovering network nodes and SQL Server instances
    /// </summary>
    public class NetworkDiscovery
    {
        #region Events

        public event EventHandler<NetworkDiscoveryProgressEventArgs> DiscoveryProgress;
        public event EventHandler<NetworkDiscoveryCompletedEventArgs> DiscoveryCompleted;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the list of discovered network nodes
        /// </summary>
        public List<NetworkNode> DiscoveredNodes { get; private set; } = new List<NetworkNode>();

        /// <summary>
        /// Gets a value indicating whether discovery is in progress
        /// </summary>
        public bool IsDiscoveryInProgress { get; private set; }

        /// <summary>
        /// Gets or sets the maximum number of concurrent ping operations
        /// </summary>
        public int MaxConcurrentPings { get; set; } = 50;

        /// <summary>
        /// Gets or sets the maximum number of concurrent SQL connection attempts
        /// </summary>
        public int MaxConcurrentSqlChecks { get; set; } = 10;

        /// <summary>
        /// Gets or sets the SQL Server port to check
        /// Default is 1433 (default SQL Server port)
        /// </summary>
        public int SqlServerPort { get; set; } = 1433;

        /// <summary>
        /// Gets or sets whether to scan common SQL Server ports beyond the default
        /// </summary>
        public bool ScanCommonSqlPorts { get; set; } = true;

        /// <summary>
        /// Common SQL Server ports to check if ScanCommonSqlPorts is true
        /// </summary>
        private readonly int[] _commonSqlPorts = { 1433, 1434, 2433, 4022 };

        /// <summary>
        /// Gets or sets whether to scan for named instances using UDP broadcast
        /// </summary>
        public bool ScanForNamedInstances { get; set; } = true;

        /// <summary>
        /// Gets or sets whether to scan only the local subnet
        /// </summary>
        public bool ScanLocalSubnetOnly { get; set; } = true;

        /// <summary>
        /// Flag to indicate the operation should be cancelled
        /// </summary>
        private bool _cancelRequested = false;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the NetworkDiscovery class
        /// </summary>
        public NetworkDiscovery()
        {
            DiscoveredNodes = new List<NetworkNode>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets all local IP addresses for network interfaces that are up
        /// </summary>
        public List<IPAddressInfo> GetLocalIPAddresses()
        {
            List<IPAddressInfo> addresses = new List<IPAddressInfo>();

            try
            {
                // Get all network interfaces
                NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();

                foreach (NetworkInterface adapter in interfaces.Where(i =>
                    i.OperationalStatus == OperationalStatus.Up &&
                    i.NetworkInterfaceType != NetworkInterfaceType.Loopback))
                {
                    string adapterName = adapter.Name;
                    string description = adapter.Description;

                    var props = adapter.GetIPProperties();

                    // Find all IPv4 addresses
                    foreach (UnicastIPAddressInformation ip in props.UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            addresses.Add(new IPAddressInfo
                            {
                                IPAddress = ip.Address,
                                InterfaceName = adapterName,
                                Description = description
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error getting local IP addresses: {ex.Message}");
            }

            return addresses;
        }

        /// <summary>
        /// Starts the discovery of network nodes and SQL Server instances
        /// </summary>
        /// <param name="localIP">Optional specific local IP to use for scanning</param>
        public void StartDiscovery(IPAddress localIP = null)
        {
            if (IsDiscoveryInProgress)
                return;

            IsDiscoveryInProgress = true;
            DiscoveredNodes.Clear();
            _cancelRequested = false;

            try
            {
                // Get the local machine's IP addresses and subnet information
                if (localIP == null)
                    localIP = GetLocalIPAddress();

                if (localIP == null)
                {
                    OnDiscoveryCompleted(new NetworkDiscoveryCompletedEventArgs(
                        DiscoveredNodes,
                        "Failed to determine local IP address. Please check your network connection.",
                        false));
                    IsDiscoveryInProgress = false;
                    return;
                }

                string localIPString = localIP.ToString();
                string[] ipParts = localIPString.Split('.');
                string subnet = $"{ipParts[0]}.{ipParts[1]}.{ipParts[2]}";

                OnDiscoveryProgress(new NetworkDiscoveryProgressEventArgs(
                    0, 256, $"Starting network discovery from {localIPString}", null));

                // Find all active IP addresses in the subnet
                for (int i = 1; i <= 255; i++)
                {
                    if (_cancelRequested)
                        break;

                    string ipAddress = $"{subnet}.{i}";

                    // Skip local IP to avoid unnecessary pinging
                    if (ipAddress == localIPString)
                    {
                        NetworkNode localNode = new NetworkNode
                        {
                            IPAddress = ipAddress,
                            IsResponding = true,
                            Hostname = Environment.MachineName,
                            IsLocalMachine = true
                        };

                        DiscoveredNodes.Add(localNode);
                        continue;
                    }

                    NetworkNode node = PingHost(ipAddress);

                    if (node.IsResponding)
                    {
                        DiscoveredNodes.Add(node);

                        int progress = (int)((float)DiscoveredNodes.Count / 255 * 100);
                        OnDiscoveryProgress(new NetworkDiscoveryProgressEventArgs(
                            DiscoveredNodes.Count, 255, $"Discovered: {ipAddress} ({node.Hostname})", node));
                    }
                }

                if (_cancelRequested)
                {
                    OnDiscoveryCompleted(new NetworkDiscoveryCompletedEventArgs(
                        DiscoveredNodes, "Discovery was cancelled", false));
                    IsDiscoveryInProgress = false;
                    return;
                }

                // Now scan for SQL Server instances on responding hosts
                ScanForSqlServers();

                // Also try to discover SQL Server instances via UDP broadcast
                if (ScanForNamedInstances)
                {
                    DiscoverNamedSqlInstances();
                }

                // Complete the discovery process
                OnDiscoveryCompleted(new NetworkDiscoveryCompletedEventArgs(
                    DiscoveredNodes,
                    $"Discovery completed. Found {DiscoveredNodes.Count} nodes and {DiscoveredNodes.Sum(n => n.SqlInstances.Count)} SQL instances.",
                    true));
            }
            catch (Exception ex)
            {
                OnDiscoveryCompleted(new NetworkDiscoveryCompletedEventArgs(
                    DiscoveredNodes, $"Discovery error: {ex.Message}", false));
            }
            finally
            {
                IsDiscoveryInProgress = false;
            }
        }

        /// <summary>
        /// Cancels the discovery operation
        /// </summary>
        public void CancelDiscovery()
        {
            if (IsDiscoveryInProgress)
            {
                _cancelRequested = true;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets the local IP address
        /// </summary>
        private IPAddress GetLocalIPAddress()
        {
            try
            {
                // Get the primary network interface that's connected to the network
                NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();

                foreach (NetworkInterface adapter in interfaces.Where(i =>
                    i.OperationalStatus == OperationalStatus.Up &&
                    i.NetworkInterfaceType != NetworkInterfaceType.Loopback &&
                    i.GetIPProperties().GatewayAddresses.Count > 0))
                {
                    var props = adapter.GetIPProperties();

                    // Find the first IPv4 address that's not a loopback address
                    foreach (UnicastIPAddressInformation ip in props.UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            return ip.Address;
                        }
                    }
                }

                // Fallback if no suitable interface is found
                using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
                {
                    socket.Connect("8.8.8.8", 65530);
                    IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                    return endPoint.Address;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error getting local IP address: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Pings a host and gets its details if it responds
        /// </summary>
        private NetworkNode PingHost(string ipAddress)
        {
            NetworkNode node = new NetworkNode { IPAddress = ipAddress };

            try
            {
                using (Ping ping = new Ping())
                {
                    PingReply reply = ping.Send(ipAddress);

                    if (reply.Status == IPStatus.Success)
                    {
                        node.IsResponding = true;
                        node.ResponseTime = reply.RoundtripTime;

                        try
                        {
                            IPHostEntry hostEntry = Dns.GetHostEntry(ipAddress);
                            node.Hostname = hostEntry.HostName;
                        }
                        catch
                        {
                            // If hostname resolution fails, just use the IP address
                            node.Hostname = ipAddress;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error pinging {ipAddress}: {ex.Message}");
                // Don't rethrow - we'll just mark this node as not responding
            }

            return node;
        }

        /// <summary>
        /// Checks for SQL Server instances on all discovered nodes
        /// </summary>
        private void ScanForSqlServers()
        {
            if (DiscoveredNodes.Count == 0)
                return;

            OnDiscoveryProgress(new NetworkDiscoveryProgressEventArgs(
                0, DiscoveredNodes.Count, "Scanning for SQL Server instances...", null));

            foreach (var node in DiscoveredNodes.Where(n => n.IsResponding))
            {
                if (_cancelRequested)
                    break;

                if (ScanCommonSqlPorts)
                {
                    // Check all common SQL ports
                    foreach (int port in _commonSqlPorts)
                    {
                        if (_cancelRequested)
                            break;

                        if (CheckPort(node.IPAddress, port))
                        {
                            TryConnectToSqlServer(node, port);
                        }
                    }
                }
                else
                {
                    // Check only the default SQL Server port
                    if (CheckPort(node.IPAddress, SqlServerPort))
                    {
                        TryConnectToSqlServer(node, SqlServerPort);
                    }
                }

                OnDiscoveryProgress(new NetworkDiscoveryProgressEventArgs(
                    DiscoveredNodes.IndexOf(node) + 1,
                    DiscoveredNodes.Count,
                    $"Scanned {node.IPAddress} ({node.Hostname}), found {node.SqlInstances.Count} SQL instances",
                    node));
            }
        }

        /// <summary>
        /// Checks if a TCP port is open on a host
        /// </summary>
        private bool CheckPort(string ipAddress, int port)
        {
            try
            {
                using (TcpClient client = new TcpClient())
                {
                    client.Connect(ipAddress, port);
                    return client.Connected;
                }
            }
            catch
            {
                // Any error means the port is not accessible
                return false;
            }
        }

        /// <summary>
        /// Tries to connect to a SQL Server instance on a specific port
        /// </summary>
        private void TryConnectToSqlServer(NetworkNode node, int port)
        {
            try
            {
                string connectionString = $"Server={node.IPAddress},{port};Encrypt=False;TrustServerCertificate=True;";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlInstance sqlInstance = new SqlInstance
                    {
                        ServerName = node.IPAddress,
                        InstanceName = "DEFAULT",
                        Port = port,
                        Version = GetSqlServerVersion(connection),
                        IsAccessible = true
                    };

                    // Only add if an instance with this port doesn't already exist for this node
                    if (!node.SqlInstances.Any(i => i.Port == port))
                    {
                        node.SqlInstances.Add(sqlInstance);
                    }
                }
            }
            catch (SqlException ex)
            {
                // Record information about the SQL error
                if (ex.Number == 4060 || ex.Number == 18456) // Login failed or database not accessible
                {
                    // This is still a SQL Server, but we don't have access
                    SqlInstance sqlInstance = new SqlInstance
                    {
                        ServerName = node.IPAddress,
                        InstanceName = "DEFAULT",
                        Port = port,
                        IsAccessible = false,
                        LastError = ex.Message
                    };

                    // Only add if an instance with this port doesn't already exist for this node
                    if (!node.SqlInstances.Any(i => i.Port == port))
                    {
                        node.SqlInstances.Add(sqlInstance);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error connecting to SQL Server at {node.IPAddress}:{port}: {ex.Message}");
                // Don't rethrow - just means this isn't a SQL Server or it's not accessible
            }
        }

        /// <summary>
        /// Gets SQL Server version from an open connection
        /// </summary>
        private string GetSqlServerVersion(SqlConnection connection)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("SELECT @@VERSION", connection))
                {
                    string version = cmd.ExecuteScalar().ToString();

                    // Extract just the first line of the version string
                    int newLinePos = version.IndexOf('\n');
                    if (newLinePos > 0)
                    {
                        version = version.Substring(0, newLinePos);
                    }

                    return version;
                }
            }
            catch
            {
                return "Unknown";
            }
        }

        /// <summary>
        /// Discovers SQL Server named instances using UDP broadcast method
        /// </summary>
        private void DiscoverNamedSqlInstances()
        {
            OnDiscoveryProgress(new NetworkDiscoveryProgressEventArgs(
                0, 100, "Scanning for named SQL Server instances...", null));

            try
            {
                // Use SQL Browser service to discover instances (UDP port 1434)
                using (UdpClient udpClient = new UdpClient())
                {
                    foreach (var node in DiscoveredNodes.Where(n => n.IsResponding))
                    {
                        if (_cancelRequested)
                            break;

                        try
                        {
                            // Send the SQL Server browser request packet (single byte 0x02)
                            udpClient.Connect(node.IPAddress, 1434);
                            byte[] requestData = { 0x02 };
                            udpClient.Send(requestData, requestData.Length);

                            // Try to receive a response
                            udpClient.Client.ReceiveTimeout = 500;
                            try
                            {
                                IPEndPoint remoteEndPoint = null;
                                byte[] responseData = udpClient.Receive(ref remoteEndPoint);

                                if (responseData != null && responseData.Length > 3 && responseData[0] == 0x05)
                                {
                                    // Parse the response to get instance information
                                    string response = Encoding.ASCII.GetString(responseData, 3, responseData.Length - 3);
                                    string[] instanceInfos = response.Split(new[] { ";;;" }, StringSplitOptions.RemoveEmptyEntries);

                                    foreach (string instanceInfo in instanceInfos)
                                    {
                                        Dictionary<string, string> instanceProperties = ParseInstanceProperties(instanceInfo);

                                        if (instanceProperties.ContainsKey("ServerName") &&
                                            instanceProperties.ContainsKey("InstanceName") &&
                                            instanceProperties.ContainsKey("tcp"))
                                        {
                                            SqlInstance sqlInstance = new SqlInstance
                                            {
                                                ServerName = instanceProperties["ServerName"],
                                                InstanceName = instanceProperties["InstanceName"],
                                                Port = int.Parse(instanceProperties["tcp"]),
                                                IsNamedInstance = true,
                                                IsDiscoveredViaBrowser = true
                                            };

                                            // Check if we can connect to this instance
                                            TryConnectToSqlInstance(node, sqlInstance);
                                        }
                                    }
                                }
                            }
                            catch (SocketException)
                            {
                                // Timeout or other socket error, move on to next node
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"Error discovering named instances on {node.IPAddress}: {ex.Message}");
                            // Continue with next node
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in named instance discovery: {ex.Message}");
                // Don't rethrow - this is an optional step
            }
        }

        /// <summary>
        /// Parses SQL Server Browser service response into instance properties
        /// </summary>
        private Dictionary<string, string> ParseInstanceProperties(string instanceInfo)
        {
            Dictionary<string, string> properties = new Dictionary<string, string>();

            string[] pairs = instanceInfo.Split(';');
            for (int i = 0; i < pairs.Length - 1; i += 2)
            {
                string key = pairs[i];
                string value = pairs[i + 1];
                properties[key] = value;
            }

            return properties;
        }

        /// <summary>
        /// Tries to connect to a SQL Server instance using its full information
        /// </summary>
        private void TryConnectToSqlInstance(NetworkNode node, SqlInstance instance)
        {
            try
            {
                string connectionString;

                if (instance.IsNamedInstance)
                {
                    // For named instances, use the instance name and port if available
                    if (instance.Port > 0)
                    {
                        connectionString = $"Server={node.IPAddress}\\{instance.InstanceName},{instance.Port};Encrypt=False;TrustServerCertificate=True;";
                    }
                    else
                    {
                        connectionString = $"Server={node.IPAddress}\\{instance.InstanceName};Encrypt=False;TrustServerCertificate=True;";
                    }
                }
                else
                {
                    // For default instances, just use the IP and port
                    connectionString = $"Server={node.IPAddress},{instance.Port};Encrypt=False;TrustServerCertificate=True;";
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    instance.IsAccessible = true;
                    instance.Version = GetSqlServerVersion(connection);

                    // Only add if an instance with this name/port doesn't already exist for this node
                    if (!node.SqlInstances.Any(i => i.InstanceName == instance.InstanceName && i.Port == instance.Port))
                    {
                        node.SqlInstances.Add(instance);
                    }
                }
            }
            catch (SqlException ex)
            {
                // Record information about the SQL error
                if (ex.Number == 4060 || ex.Number == 18456) // Login failed or database not accessible
                {
                    // This is still a SQL Server, but we don't have access
                    instance.IsAccessible = false;
                    instance.LastError = ex.Message;

                    // Only add if an instance with this name/port doesn't already exist for this node
                    if (!node.SqlInstances.Any(i => i.InstanceName == instance.InstanceName && i.Port == instance.Port))
                    {
                        node.SqlInstances.Add(instance);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error connecting to SQL instance {instance.InstanceName} at {node.IPAddress}: {ex.Message}");
                // Don't rethrow - just means this isn't a SQL Server or it's not accessible
            }
        }

        /// <summary>
        /// Raises the DiscoveryProgress event
        /// </summary>
        private void OnDiscoveryProgress(NetworkDiscoveryProgressEventArgs e)
        {
            DiscoveryProgress?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the DiscoveryCompleted event
        /// </summary>
        private void OnDiscoveryCompleted(NetworkDiscoveryCompletedEventArgs e)
        {
            DiscoveryCompleted?.Invoke(this, e);
        }

        #endregion
    }
}