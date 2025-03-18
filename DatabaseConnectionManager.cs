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
        private static readonly object _lock = new object();

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
                    // Read connection string
                    string connectionString = File.ReadAllText(configPath);

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

                // Save connection string
                File.WriteAllText(configPath, connectionString);

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

            try
            {
                using (var connection = CreateConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
            catch
            {
                return false;
            }
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

                try
                {
                    connection.Open();
                    return connection;
                }
                catch
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

            SqlConnection connection = null;

            try
            {
                connection = CreateConnection(_connectionString);
                connection.Open();
                action(connection);
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                connection?.Dispose();
            }
        }

        /// <summary>
        /// Creates a SQL connection
        /// </summary>
        /// <param name="connectionString">The connection string</param>
        /// <returns>A configured SqlConnection object (not yet opened)</returns>
        public static SqlConnection CreateConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
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