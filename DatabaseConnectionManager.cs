using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace Manny_Tools_Claude
{
    /// <summary>
    /// Centralized manager for database connections across the application
    /// </summary>
    public class DatabaseConnectionManager
    {
        // Singleton instance
        private static DatabaseConnectionManager _instance;

        // Connection string
        private string _connectionString;

        // Events
        public event EventHandler<ConnectionChangedEventArgs> ConnectionChanged;

        // Private constructor (singleton pattern)
        private DatabaseConnectionManager()
        {
            LoadConnectionString();
        }

        /// <summary>
        /// Gets the singleton instance
        /// </summary>
        public static DatabaseConnectionManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DatabaseConnectionManager();
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
        /// Tests a connection string
        /// </summary>
        /// <param name="connectionString">The connection string to test</param>
        /// <returns>True if connection successful, false otherwise</returns>
        public bool TestConnection(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                return false;
            }

            bool connectionSuccessful = false;
            bool timeoutOccurred = false;
            Exception connectionException = null;

            // Create a timer to enforce the 3-second timeout
            System.Threading.Timer timeoutTimer = null;

            // Use ManualResetEvent to properly wait for either connection success or timeout
            using (var connectionCompleted = new System.Threading.ManualResetEvent(false))
            {
                // Create the timeout timer
                timeoutTimer = new System.Threading.Timer(_ =>
                {
                    timeoutOccurred = true;
                    connectionCompleted.Set(); // Signal to continue execution
                }, null, 3000, System.Threading.Timeout.Infinite); // 3000ms = 3 seconds

                // Try to establish the connection in a separate thread
                System.Threading.ThreadPool.QueueUserWorkItem(_ =>
                {
                    try
                    {
                        using (var connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            connection.Close();
                            connectionSuccessful = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        connectionException = ex;
                    }

                });

                // Wait for either the connection to complete or the timeout to occur
                connectionCompleted.WaitOne();
            }

            // Dispose of the timer
            timeoutTimer?.Dispose();

            // If we timed out, log the event
            if (timeoutOccurred && !connectionSuccessful)
            {
                // Optionally log the timeout
                System.Diagnostics.Debug.WriteLine("Database connection attempt timed out after 3 seconds");
            }
            else if (connectionException != null)
            {
                // Optionally log the exception
                System.Diagnostics.Debug.WriteLine($"Database connection error: {connectionException.Message}");
            }

            return connectionSuccessful;
        }

        /// <summary>
        /// Raises the ConnectionChanged event
        /// </summary>
        protected virtual void OnConnectionChanged(ConnectionChangedEventArgs e)
        {
            ConnectionChanged?.Invoke(this, e);
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
                var connection = CreateConnectionWithTimeout(_connectionString);
                connection.Open();
                return connection;
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
                using (var connection = CreateConnectionWithTimeout(_connectionString))
                {
                    connection.Open();
                    action(connection);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        public static SqlConnection CreateConnectionWithTimeout(string connectionString)
        {
            // Create a SqlConnectionStringBuilder to safely modify the connection string
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);

            // Set 3-second timeout
            builder.ConnectTimeout = 3;

            // Create and return connection with the modified connection string
            return new SqlConnection(builder.ConnectionString);
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