using System;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace Manny_Tools_Claude
{
    /// <summary>
    /// Centralized manager for database connections across the application
    /// Handles connection creation, testing, and management with standardized timeouts
    /// </summary>
    public class DatabaseConnectionManager
    {
        // Singleton instance
        private static DatabaseConnectionManager _instance;
        private static readonly object _lock = new object();

        // Connection string
        private string _connectionString;

        // Standard timeout for all connections (5 seconds)
        private const int CONNECTION_TIMEOUT_SECONDS = 5;

        // Events
        public event EventHandler<ConnectionChangedEventArgs> ConnectionChanged;

        // Private constructor (singleton pattern)
        private DatabaseConnectionManager()
        {
            LoadConnectionString();
        }

        /// <summary>
        /// Gets the singleton instance with thread safety
        /// </summary>
        public static DatabaseConnectionManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new DatabaseConnectionManager();
                        }
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Gets the current connection string
        /// </summary>
        public string ConnectionString
        {
            get { return _connectionString; }
        }

        /// <summary>
        /// Loads the connection string from storage
        /// </summary>
        /// <returns>True if loaded successfully, false otherwise</returns>
        public bool LoadConnectionString()
        {
            try
            {
                string appDataPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "MannyTools");

                string configPath = Path.Combine(appDataPath, DataEncryptionHelper.ConfigFiles.ConnectionFile);

                if (File.Exists(configPath))
                {
                    // Read and decrypt connection string
                    string connectionString = DataEncryptionHelper.ReadEncryptedFile(configPath);

                    // Test the connection
                    if (!string.IsNullOrEmpty(connectionString) && TestConnection(connectionString))
                    {
                        _connectionString = connectionString;

                        // Update connection status manager
                        ConnectionStatusManager.Instance.CheckConnection(_connectionString);

                        // Notify listeners
                        OnConnectionChanged(new ConnectionChangedEventArgs(_connectionString));

                        return true;
                    }
                }

                // Connection not found or invalid
                _connectionString = null;
                return false;
            }
            catch
            {
                // Error loading connection
                _connectionString = null;
                return false;
            }
        }

        /// <summary>
        /// Updates the connection string and saves it to storage
        /// </summary>
        /// <param name="connectionString">The new connection string</param>
        /// <returns>True if updated successfully, false otherwise</returns>
        public bool UpdateConnectionString(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString) || !TestConnection(connectionString))
            {
                return false;
            }

            try
            {
                string appDataPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "MannyTools");

                Directory.CreateDirectory(appDataPath);
                string configPath = Path.Combine(appDataPath, DataEncryptionHelper.ConfigFiles.ConnectionFile);

                // Encrypt and save connection string
                DataEncryptionHelper.WriteEncryptedFile(configPath, connectionString);

                // Update local connection string
                _connectionString = connectionString;

                // Update connection status manager
                ConnectionStatusManager.Instance.CheckConnection(_connectionString);

                // Notify listeners
                OnConnectionChanged(new ConnectionChangedEventArgs(_connectionString));

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Tests a connection string with a 5-second timeout
        /// </summary>
        /// <param name="connectionString">The connection string to test</param>
        /// <returns>True if connection successful, false otherwise</returns>
        public bool TestConnection(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                return false;
            }

            return ExecuteWithTimeout(async () =>
            {
                using (var connection = CreateConnection(connectionString))
                {
                    await connection.OpenAsync();
                    return true;
                }
            });
        }

        /// <summary>
        /// Gets a new connection to the database
        /// </summary>
        /// <returns>An open SqlConnection or null if connection fails</returns>
        public SqlConnection GetConnection()
        {
            if (string.IsNullOrEmpty(_connectionString))
            {
                LoadConnectionString();

                if (string.IsNullOrEmpty(_connectionString))
                {
                    return null;
                }
            }

            try
            {
                var connection = CreateConnection(_connectionString);

                bool connected = ExecuteWithTimeout(async () =>
                {
                    await connection.OpenAsync();
                    return true;
                });

                if (connected)
                {
                    return connection;
                }
                else
                {
                    connection.Dispose();
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Executes an action with a database connection, handling the connection lifecycle
        /// </summary>
        /// <param name="action">The action to execute</param>
        /// <returns>True if successful, false if connection error</returns>
        public bool ExecuteWithConnection(Action<SqlConnection> action)
        {
            if (string.IsNullOrEmpty(_connectionString))
            {
                LoadConnectionString();

                if (string.IsNullOrEmpty(_connectionString))
                {
                    return false;
                }
            }

            try
            {
                using (var connection = CreateConnection(_connectionString))
                {
                    bool connected = ExecuteWithTimeout(async () =>
                    {
                        await connection.OpenAsync();
                        return true;
                    });

                    if (!connected)
                    {
                        return false;
                    }

                    action(connection);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Creates a SQL connection with the standard 5-second timeout
        /// </summary>
        /// <param name="connectionString">The connection string</param>
        /// <returns>A configured SqlConnection object (not yet opened)</returns>
        public static SqlConnection CreateConnection(string connectionString)
        {
            // Create a SqlConnectionStringBuilder to safely modify the connection string
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);

            // Set standard 5-second timeout
            builder.ConnectTimeout = CONNECTION_TIMEOUT_SECONDS;

            // Create and return connection with the modified connection string
            return new SqlConnection(builder.ConnectionString);
        }

        /// <summary>
        /// Executes a function with a 5-second timeout
        /// </summary>
        /// <typeparam name="T">The return type of the function</typeparam>
        /// <param name="function">The async function to execute</param>
        /// <returns>The result of the function or default if timeout occurred</returns>
        private T ExecuteWithTimeout<T>(Func<Task<T>> function)
        {
            try
            {
                using (var tokenSource = new CancellationTokenSource())
                {
                    // Create a task that completes after the timeout
                    var timeoutTask = Task.Delay(CONNECTION_TIMEOUT_SECONDS * 1000, tokenSource.Token);

                    // Start the actual function
                    var functionTask = function();

                    // Wait for either the function to complete or the timeout to occur
                    var completedTask = Task.WhenAny(functionTask, timeoutTask).GetAwaiter().GetResult();

                    // If the function completed first, cancel the timeout and return the result
                    if (completedTask == functionTask)
                    {
                        tokenSource.Cancel(); // Cancel the timeout task
                        return functionTask.GetAwaiter().GetResult();
                    }

                    // If we got here, the timeout occurred first
                    return default;
                }
            }
            catch
            {
                return default;
            }
        }

        /// <summary>
        /// Raises the ConnectionChanged event
        /// </summary>
        protected virtual void OnConnectionChanged(ConnectionChangedEventArgs e)
        {
            ConnectionChanged?.Invoke(this, e);
        }
    }

    /// <summary>
    /// Event arguments for connection changes
    /// </summary>
    public class ConnectionChangedEventArgs : EventArgs
    {
        public string ConnectionString { get; private set; }

        public ConnectionChangedEventArgs(string connectionString)
        {
            ConnectionString = connectionString;
        }
    }
}