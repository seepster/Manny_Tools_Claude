using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.ComponentModel;

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
        /// Gets or sets the timeout in milliseconds for network operations
        /// </summary>
        public int Timeout { get; set; } = 500;

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
        /// Cancellation token source for cancelling discovery operations
        /// </summary>
        private CancellationTokenSource _cts;

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
        /// Starts the discovery of network nodes and SQL Server instances asynchronously
        /// </summary>
        /// <param name="localIP">Optional specific local IP to use for scanning</param>
        public async Task StartDiscoveryAsync(IPAddress localIP = null)
        {
            if (IsDiscoveryInProgress)
                return;

            IsDiscoveryInProgress = true;
            DiscoveredNodes.Clear();

            _cts = new CancellationTokenSource();

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

                // List to hold tasks for each IP ping operation
                List<Task<NetworkNode>> pingTasks = new List<Task<NetworkNode>>();

                // Find all active IP addresses in the subnet
                SemaphoreSlim semaphore = new SemaphoreSlim(MaxConcurrentPings);

                for (int i = 1; i <= 255; i++)
                {
                    if (_cts.Token.IsCancellationRequested)
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

                    await semaphore.WaitAsync(_cts.Token);

                    pingTasks.Add(Task.Run(async () =>
                    {
                        try
                        {
                            NetworkNode node = await PingHostAsync(ipAddress, _cts.Token);

                            if (node.IsResponding)
                            {
                                lock (DiscoveredNodes)
                                {
                                    DiscoveredNodes.Add(node);
                                }

                                int progress = (int)((float)DiscoveredNodes.Count / 255 * 100);
                                OnDiscoveryProgress(new NetworkDiscoveryProgressEventArgs(
                                    DiscoveredNodes.Count, 255, $"Discovered: {ipAddress} ({node.Hostname})", node));
                            }

                            return node;
                        }
                        finally
                        {
                            semaphore.Release();
                        }
                    }, _cts.Token));
                }

                // Wait for all ping tasks to complete or cancel
                try
                {
                    await Task.WhenAll(pingTasks.ToArray());
                }
                catch (OperationCanceledException)
                {
                    // Cancellation is handled below
                }

                if (_cts.Token.IsCancellationRequested)
                {
                    OnDiscoveryCompleted(new NetworkDiscoveryCompletedEventArgs(
                        DiscoveredNodes, "Discovery was cancelled", false));
                    IsDiscoveryInProgress = false;
                    return;
                }

                // Now scan for SQL Server instances on responding hosts
                await ScanForSqlServersAsync(_cts.Token);

                // Also try to discover SQL Server instances via UDP broadcast
                if (ScanForNamedInstances)
                {
                    await DiscoverNamedSqlInstancesAsync(_cts.Token);
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
            if (IsDiscoveryInProgress && _cts != null)
            {
                _cts.Cancel();
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
        private async Task<NetworkNode> PingHostAsync(string ipAddress, CancellationToken cancellationToken)
        {
            NetworkNode node = new NetworkNode { IPAddress = ipAddress };

            try
            {
                using (Ping ping = new Ping())
                {
                    PingReply reply = await ping.SendPingAsync(ipAddress, Timeout);

                    if (reply.Status == IPStatus.Success)
                    {
                        node.IsResponding = true;
                        node.ResponseTime = reply.RoundtripTime;

                        try
                        {
                            IPHostEntry hostEntry = await Dns.GetHostEntryAsync(ipAddress);
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
        private async Task ScanForSqlServersAsync(CancellationToken cancellationToken)
        {
            if (DiscoveredNodes.Count == 0)
                return;

            OnDiscoveryProgress(new NetworkDiscoveryProgressEventArgs(
                0, DiscoveredNodes.Count, "Scanning for SQL Server instances...", null));

            SemaphoreSlim sqlSemaphore = new SemaphoreSlim(MaxConcurrentSqlChecks);
            List<Task> sqlCheckTasks = new List<Task>();

            foreach (var node in DiscoveredNodes.Where(n => n.IsResponding))
            {
                if (cancellationToken.IsCancellationRequested)
                    break;

                await sqlSemaphore.WaitAsync(cancellationToken);

                sqlCheckTasks.Add(Task.Run(async () =>
                {
                    try
                    {
                        if (ScanCommonSqlPorts)
                        {
                            // Check all common SQL ports
                            foreach (int port in _commonSqlPorts)
                            {
                                if (cancellationToken.IsCancellationRequested)
                                    break;

                                if (await CheckPortAsync(node.IPAddress, port, cancellationToken))
                                {
                                    await TryConnectToSqlServerAsync(node, port, cancellationToken);
                                }
                            }
                        }
                        else
                        {
                            // Check only the default SQL Server port
                            if (await CheckPortAsync(node.IPAddress, SqlServerPort, cancellationToken))
                            {
                                await TryConnectToSqlServerAsync(node, SqlServerPort, cancellationToken);
                            }
                        }

                        OnDiscoveryProgress(new NetworkDiscoveryProgressEventArgs(
                            DiscoveredNodes.IndexOf(node) + 1,
                            DiscoveredNodes.Count,
                            $"Scanned {node.IPAddress} ({node.Hostname}), found {node.SqlInstances.Count} SQL instances",
                            node));
                    }
                    finally
                    {
                        sqlSemaphore.Release();
                    }
                }, cancellationToken));
            }

            try
            {
                await Task.WhenAll(sqlCheckTasks.ToArray());
            }
            catch (OperationCanceledException)
            {
                // Cancellation is expected and handled by the caller
            }
        }

        /// <summary>
        /// Checks if a TCP port is open on a host
        /// </summary>
        private async Task<bool> CheckPortAsync(string ipAddress, int port, CancellationToken cancellationToken)
        {
            try
            {
                using (TcpClient client = new TcpClient())
                {
                    var connectTask = client.ConnectAsync(ipAddress, port);
                    var timeoutTask = Task.Delay(Timeout, cancellationToken);

                    var completedTask = await Task.WhenAny(connectTask, timeoutTask);

                    if (completedTask == connectTask && client.Connected)
                    {
                        return true;
                    }
                }
            }
            catch
            {
                // Any error means the port is not accessible
            }

            return false;
        }

        /// <summary>
        /// Tries to connect to a SQL Server instance on a specific port
        /// </summary>
        private async Task TryConnectToSqlServerAsync(NetworkNode node, int port, CancellationToken cancellationToken)
        {
            try
            {
                string connectionString = $"Server={node.IPAddress},{port};Encrypt=False;TrustServerCertificate=True;Connection Timeout={Timeout / 1000};";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync(cancellationToken);

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
        private async Task DiscoverNamedSqlInstancesAsync(CancellationToken cancellationToken)
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
                        if (cancellationToken.IsCancellationRequested)
                            break;

                        try
                        {
                            // Send the SQL Server browser request packet (single byte 0x02)
                            udpClient.Client.ReceiveTimeout = Timeout;
                            udpClient.Connect(node.IPAddress, 1434);
                            byte[] requestData = { 0x02 };
                            await udpClient.SendAsync(requestData, requestData.Length);

                            // Try to receive a response
                            var receiveTask = udpClient.ReceiveAsync();
                            var timeoutTask = Task.Delay(Timeout, cancellationToken);

                            var completedTask = await Task.WhenAny(receiveTask, timeoutTask);

                            if (completedTask == receiveTask)
                            {
                                var result = await receiveTask;
                                byte[] responseData = result.Buffer;

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
                                            await TryConnectToSqlInstanceAsync(node, sqlInstance, cancellationToken);
                                        }
                                    }
                                }
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
        private async Task TryConnectToSqlInstanceAsync(NetworkNode node, SqlInstance instance, CancellationToken cancellationToken)
        {
            try
            {
                string connectionString;

                if (instance.IsNamedInstance)
                {
                    // For named instances, use the instance name and port if available
                    if (instance.Port > 0)
                    {
                        connectionString = $"Server={node.IPAddress}\\{instance.InstanceName},{instance.Port};Encrypt=False;TrustServerCertificate=True;Connection Timeout={Timeout / 1000};";
                    }
                    else
                    {
                        connectionString = $"Server={node.IPAddress}\\{instance.InstanceName};Encrypt=False;TrustServerCertificate=True;Connection Timeout={Timeout / 1000};";
                    }
                }
                else
                {
                    // For default instances, just use the IP and port
                    connectionString = $"Server={node.IPAddress},{instance.Port};Encrypt=False;TrustServerCertificate=True;Connection Timeout={Timeout / 1000};";
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync(cancellationToken);

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

    #region Supporting Classes

    /// <summary>
    /// Class to hold IP address information
    /// </summary>
    public class IPAddressInfo
    {
        public IPAddress IPAddress { get; set; }
        public string InterfaceName { get; set; }
        public string Description { get; set; }

        public override string ToString()
        {
            return $"{IPAddress} ({InterfaceName})";
        }
    }

    /// <summary>
    /// Represents a network node discovered during network scanning
    /// </summary>
    public class NetworkNode
    {
        /// <summary>
        /// Gets or sets the IP address of the node
        /// </summary>
        public string IPAddress { get; set; }

        /// <summary>
        /// Gets or sets the hostname of the node
        /// </summary>
        public string Hostname { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the node is responding to ping
        /// </summary>
        public bool IsResponding { get; set; }

        /// <summary>
        /// Gets or sets the ping response time in milliseconds
        /// </summary>
        public long ResponseTime { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is the local machine
        /// </summary>
        public bool IsLocalMachine { get; set; }

        /// <summary>
        /// Gets or sets the list of SQL Server instances on this node
        /// </summary>
        public List<SqlInstance> SqlInstances { get; set; } = new List<SqlInstance>();

        /// <summary>
        /// Returns a string representation of the network node
        /// </summary>
        public override string ToString()
        {
            return $"{IPAddress} ({Hostname}) - {SqlInstances.Count} SQL instances";
        }
    }

    /// <summary>
    /// Represents a SQL Server instance discovered during network scanning
    /// </summary>
    public class SqlInstance
    {
        /// <summary>
        /// Gets or sets the name of the server hosting this instance
        /// </summary>
        public string ServerName { get; set; }

        /// <summary>
        /// Gets or sets the instance name
        /// </summary>
        public string InstanceName { get; set; }

        /// <summary>
        /// Gets or sets the TCP port the instance is listening on
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets the SQL Server version
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the instance is accessible
        /// </summary>
        public bool IsAccessible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is a named instance
        /// </summary>
        public bool IsNamedInstance { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance was discovered via SQL Browser service
        /// </summary>
        public bool IsDiscoveredViaBrowser { get; set; }

        /// <summary>
        /// Gets or sets the last error message when attempting to connect
        /// </summary>
        public string LastError { get; set; }

        /// <summary>
        /// Returns a string representation of the SQL Server instance
        /// </summary>
        public override string ToString()
        {
            if (IsNamedInstance)
            {
                return $"{ServerName}\\{InstanceName} ({Port}) - {(IsAccessible ? "Accessible" : "Not accessible")}";
            }
            else
            {
                return $"{ServerName}:{Port} - {(IsAccessible ? "Accessible" : "Not accessible")}";
            }
        }
    }

    /// <summary>
    /// Provides event data for the DiscoveryProgress event
    /// </summary>
    public class NetworkDiscoveryProgressEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the current progress value
        /// </summary>
        public int CurrentProgress { get; }

        /// <summary>
        /// Gets the total expected progress value
        /// </summary>
        public int TotalProgress { get; }

        /// <summary>
        /// Gets the status message
        /// </summary>
        public string StatusMessage { get; }

        /// <summary>
        /// Gets the currently discovered node, if applicable
        /// </summary>
        public NetworkNode CurrentNode { get; }

        /// <summary>
        /// Initializes a new instance of the NetworkDiscoveryProgressEventArgs class
        /// </summary>
        public NetworkDiscoveryProgressEventArgs(int currentProgress, int totalProgress, string statusMessage, NetworkNode currentNode)
        {
            CurrentProgress = currentProgress;
            TotalProgress = totalProgress;
            StatusMessage = statusMessage;
            CurrentNode = currentNode;
        }
    }

    /// <summary>
    /// Provides event data for the DiscoveryCompleted event
    /// </summary>
    public class NetworkDiscoveryCompletedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the list of discovered network nodes
        /// </summary>
        public List<NetworkNode> DiscoveredNodes { get; }

        /// <summary>
        /// Gets the completion message
        /// </summary>
        public string CompletionMessage { get; }

        /// <summary>
        /// Gets a value indicating whether the discovery completed successfully
        /// </summary>
        public bool IsSuccessful { get; }

        /// <summary>
        /// Initializes a new instance of the NetworkDiscoveryCompletedEventArgs class
        /// </summary>
        public NetworkDiscoveryCompletedEventArgs(List<NetworkNode> discoveredNodes, string completionMessage, bool isSuccessful)
        {
            DiscoveredNodes = discoveredNodes;
            CompletionMessage = completionMessage;
            IsSuccessful = isSuccessful;
        }
    }
    #endregion
}

