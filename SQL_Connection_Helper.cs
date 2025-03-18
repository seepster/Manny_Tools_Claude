using System;
using Microsoft.Data.SqlClient;

namespace Manny_Tools_Claude
{
    /// <summary>
    /// Helper class for SQL connections with standardized handling
    /// </summary>
    public static class SQL_Connection_Helper
    {
        /// <summary>
        /// Executes a function with a connection
        /// </summary>
        /// <typeparam name="T">The return type of the function</typeparam>
        /// <param name="connectionString">Connection string</param>
        /// <param name="action">The function to execute with the open connection</param>
        /// <returns>Result of the action or default value</returns>
        public static T ExecuteWithConnection<T>(string connectionString, Func<SqlConnection, T> action)
        {
            try
            {
                using (var connection = DatabaseConnectionManager.CreateConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        return action(connection);
                    }
                    catch
                    {
                        return default;
                    }
                }
            }
            catch
            {
                return default;
            }
        }

        /// <summary>
        /// Tests a connection
        /// </summary>
        /// <param name="connectionString">The connection string to test</param>
        /// <returns>True if connection successful, false otherwise</returns>
        public static bool TestConnection(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                return false;
            }

            try
            {
                using (var connection = DatabaseConnectionManager.CreateConnection(connectionString))
                {
                    connection.Open();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}