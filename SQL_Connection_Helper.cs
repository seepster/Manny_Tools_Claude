using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace Manny_Tools_Claude
{
    /// <summary>
    /// Helper class for SQL connections with standardized timeout handling
    /// </summary>
    public static class SQL_Connection_Helper
    {
        // Standard timeout for all connections (5 seconds)
        public const int CONNECTION_TIMEOUT_SECONDS = 5;

        /// <summary>
        /// Executes an async function with a 5-second timeout
        /// </summary>
        /// <typeparam name="T">The return type of the function</typeparam>
        /// <param name="connectionStringOrConnection">Connection string or existing SqlConnection</param>
        /// <param name="action">The function to execute with the open connection</param>
        /// <returns>Result of the action or default value if timeout</returns>
        public static async Task<T> ExecuteWithTimeoutAsync<T>(string connectionString, Func<SqlConnection, Task<T>> action)
        {
            using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(CONNECTION_TIMEOUT_SECONDS)))
            {
                try
                {
                    using (var connection = DatabaseConnectionManager.CreateConnection(connectionString))
                    {
                        try
                        {
                            await connection.OpenAsync(cts.Token);
                            return await action(connection);
                        }
                        catch (TaskCanceledException)
                        {
                            // Connection timeout
                            return default;
                        }
                    }
                }
                catch
                {
                    return default;
                }
            }
        }

        /// <summary>
        /// Executes an action using an existing connection, with timeout
        /// </summary>
        /// <typeparam name="T">The return type of the function</typeparam>
        /// <param name="connection">Existing SQL connection (must be open)</param>
        /// <param name="action">The function to execute with the connection</param>
        /// <returns>Result of the action or default value if timeout</returns>
        public static async Task<T> ExecuteWithTimeoutAsync<T>(SqlConnection connection, Func<SqlConnection, Task<T>> action)
        {
            using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(CONNECTION_TIMEOUT_SECONDS)))
            {
                try
                {
                    return await action(connection);
                }
                catch (TaskCanceledException)
                {
                    // Operation timeout
                    return default;
                }
                catch
                {
                    // Other error
                    return default;
                }
            }
        }

        /// <summary>
        /// Tests a connection asynchronously with a 5-second timeout
        /// </summary>
        /// <param name="connectionString">The connection string to test</param>
        /// <returns>True if connection successful, false otherwise</returns>
        public static async Task<bool> TestConnectionAsync(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                return false;
            }

            using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(CONNECTION_TIMEOUT_SECONDS)))
            {
                try
                {
                    using (var connection = DatabaseConnectionManager.CreateConnection(connectionString))
                    {
                        try
                        {
                            await connection.OpenAsync(cts.Token);
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
        }

        /// <summary>
        /// Tests a connection synchronously with a 5-second timeout
        /// </summary>
        /// <param name="connectionString">The connection string to test</param>
        /// <returns>True if connection successful, false otherwise</returns>
        public static bool TestConnection(string connectionString)
        {
            return Task.Run(() => TestConnectionAsync(connectionString)).GetAwaiter().GetResult();
        }
    }
}