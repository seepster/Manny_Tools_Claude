using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Data.SqlClient;
using Dapper;

namespace Manny_Tools_Claude
{
    /// <summary>
    /// A helper class for executing SQL queries using Dapper and mapping the results to a generic type.
    /// Implements consistent 5-second timeout for all operations.
    /// </summary>
    public static class SQL_Get_Generic_List
    {
        // Standard timeout for all connections (5 seconds)
        private const int CONNECTION_TIMEOUT_SECONDS = 5;

        /// <summary>
        /// Executes a SQL query and maps the results to a list of objects of type T using Dapper.
        /// </summary>
        /// <typeparam name="T">The type to map the query results to.</typeparam>
        /// <param name="connectionString">The SQL connection string.</param>
        /// <param name="query">The SQL query to execute.</param>
        /// <param name="parameters">Optional object containing query parameters.</param>
        /// <returns>A list of objects of type T populated with the query results.</returns>
        public static List<T> ExecuteQuery<T>(string connectionString, string query, object parameters = null)
        {
            using (var connection = DatabaseConnectionManager.CreateConnection(connectionString))
            {
                var result = ExecuteWithTimeout(async () =>
                {
                    await connection.OpenAsync();
                    return await connection.QueryAsync<T>(query, parameters);
                });

                return result != null ? result.AsList() : new List<T>();
            }
        }

        /// <summary>
        /// Executes a SQL query that returns a single object.
        /// </summary>
        /// <typeparam name="T">The type of the return value.</typeparam>
        /// <param name="connectionString">The SQL connection string.</param>
        /// <param name="query">The SQL query to execute.</param>
        /// <param name="parameters">Optional object containing query parameters.</param>
        /// <returns>A single object of type T.</returns>
        public static T ExecuteSingle<T>(string connectionString, string query, object parameters = null)
        {
            using (var connection = DatabaseConnectionManager.CreateConnection(connectionString))
            {
                return ExecuteWithTimeout(async () =>
                {
                    await connection.OpenAsync();
                    return await connection.QuerySingleOrDefaultAsync<T>(query, parameters);
                });
            }
        }

        /// <summary>
        /// Executes a SQL query that returns a single value.
        /// </summary>
        /// <typeparam name="T">The type of the return value.</typeparam>
        /// <param name="connectionString">The SQL connection string.</param>
        /// <param name="query">The SQL query to execute.</param>
        /// <param name="parameters">Optional object containing query parameters.</param>
        /// <returns>The scalar value returned by the query.</returns>
        public static T ExecuteScalar<T>(string connectionString, string query, object parameters = null)
        {
            using (var connection = DatabaseConnectionManager.CreateConnection(connectionString))
            {
                return ExecuteWithTimeout(async () =>
                {
                    await connection.OpenAsync();
                    return await connection.ExecuteScalarAsync<T>(query, parameters);
                });
            }
        }

        /// <summary>
        /// Executes a non-query SQL command (INSERT, UPDATE, DELETE).
        /// </summary>
        /// <param name="connectionString">The SQL connection string.</param>
        /// <param name="query">The SQL command to execute.</param>
        /// <param name="parameters">Optional object containing query parameters.</param>
        /// <returns>The number of rows affected.</returns>
        public static int ExecuteNonQuery(string connectionString, string query, object parameters = null)
        {
            using (var connection = DatabaseConnectionManager.CreateConnection(connectionString))
            {
                return ExecuteWithTimeout(async () =>
                {
                    await connection.OpenAsync();
                    return await connection.ExecuteAsync(query, parameters);
                });
            }
        }

        /// <summary>
        /// Executes multiple SQL commands within a transaction.
        /// </summary>
        /// <param name="connectionString">The SQL connection string.</param>
        /// <param name="actions">Action to perform within the transaction.</param>
        /// <returns>True if the transaction completed successfully, false otherwise.</returns>
        public static bool ExecuteInTransaction(string connectionString, Action<IDbConnection, IDbTransaction> actions)
        {
            using (var connection = DatabaseConnectionManager.CreateConnection(connectionString))
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

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        actions(connection, transaction);
                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Executes a function with a 5-second timeout
        /// </summary>
        /// <typeparam name="T">The return type of the function</typeparam>
        /// <param name="function">The async function to execute</param>
        /// <returns>The result of the function or default if timeout occurred</returns>
        private static T ExecuteWithTimeout<T>(Func<Task<T>> function)
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
    }
}