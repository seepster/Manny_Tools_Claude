using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Dapper;

namespace Manny_Tools_Claude
{
    /// <summary>
    /// A helper class for executing SQL queries using Dapper and mapping the results to a generic type.
    /// </summary>
    public static class SQL_Get_Generic_List
    {
        // Standard timeout for all operations (reduced to 10 seconds)
        private const int OPERATION_TIMEOUT_SECONDS = 10;

        /// <summary>
        /// Executes a SQL query and maps the results to a list of objects of type T using Dapper.
        /// Uses timeout control to avoid hanging the UI.
        /// </summary>
        /// <typeparam name="T">The type to map the query results to.</typeparam>
        /// <param name="connectionString">The SQL connection string.</param>
        /// <param name="query">The SQL query to execute.</param>
        /// <param name="parameters">Optional object containing query parameters.</param>
        /// <returns>A list of objects of type T populated with the query results.</returns>
        public static List<T> ExecuteQuery<T>(string connectionString, string query, object parameters = null)
        {
            using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(OPERATION_TIMEOUT_SECONDS)))
            {
                try
                {
                    using (var connection = DatabaseConnectionManager.CreateConnection(connectionString))
                    {
                        // Open connection with timeout
                        var openTask = Task.Run(() => {
                            try
                            {
                                connection.Open();
                                return true;
                            }
                            catch
                            {
                                return false;
                            }
                        });

                        if (!openTask.Wait(TimeSpan.FromSeconds(OPERATION_TIMEOUT_SECONDS)) || !openTask.Result)
                        {
                            // Connection timeout or failure
                            return new List<T>();
                        }

                        // Execute query with timeout
                        var queryTask = Task.Run(() => {
                            try
                            {
                                var results = connection.Query<T>(query, parameters,
                                    commandTimeout: OPERATION_TIMEOUT_SECONDS);
                                return results.AsList();
                            }
                            catch
                            {
                                return new List<T>();
                            }
                        });

                        if (queryTask.Wait(TimeSpan.FromSeconds(OPERATION_TIMEOUT_SECONDS)))
                        {
                            return queryTask.Result;
                        }

                        // Query timeout
                        return new List<T>();
                    }
                }
                catch (Exception)
                {
                    return new List<T>();
                }
            }
        }

        /// <summary>
        /// Executes a SQL query that returns a single object.
        /// Uses timeout control to avoid hanging the UI.
        /// </summary>
        /// <typeparam name="T">The type of the return value.</typeparam>
        /// <param name="connectionString">The SQL connection string.</param>
        /// <param name="query">The SQL query to execute.</param>
        /// <param name="parameters">Optional object containing query parameters.</param>
        /// <returns>A single object of type T.</returns>
        public static T ExecuteSingle<T>(string connectionString, string query, object parameters = null)
        {
            using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(OPERATION_TIMEOUT_SECONDS)))
            {
                try
                {
                    using (var connection = DatabaseConnectionManager.CreateConnection(connectionString))
                    {
                        // Open connection with timeout
                        var openTask = Task.Run(() => {
                            try
                            {
                                connection.Open();
                                return true;
                            }
                            catch
                            {
                                return false;
                            }
                        });

                        if (!openTask.Wait(TimeSpan.FromSeconds(OPERATION_TIMEOUT_SECONDS)) || !openTask.Result)
                        {
                            // Connection timeout or failure
                            return default;
                        }

                        // Execute query with timeout
                        var queryTask = Task.Run(() => {
                            try
                            {
                                return connection.QuerySingleOrDefault<T>(query, parameters,
                                    commandTimeout: OPERATION_TIMEOUT_SECONDS);
                            }
                            catch
                            {
                                return default(T);
                            }
                        });

                        if (queryTask.Wait(TimeSpan.FromSeconds(OPERATION_TIMEOUT_SECONDS)))
                        {
                            return queryTask.Result;
                        }

                        // Query timeout
                        return default;
                    }
                }
                catch (Exception)
                {
                    return default;
                }
            }
        }

        /// <summary>
        /// Executes a SQL query that returns a single value.
        /// Uses timeout control to avoid hanging the UI.
        /// </summary>
        /// <typeparam name="T">The type of the return value.</typeparam>
        /// <param name="connectionString">The SQL connection string.</param>
        /// <param name="query">The SQL query to execute.</param>
        /// <param name="parameters">Optional object containing query parameters.</param>
        /// <returns>The scalar value returned by the query.</returns>
        public static T ExecuteScalar<T>(string connectionString, string query, object parameters = null)
        {
            using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(OPERATION_TIMEOUT_SECONDS)))
            {
                try
                {
                    using (var connection = DatabaseConnectionManager.CreateConnection(connectionString))
                    {
                        // Open connection with timeout
                        var openTask = Task.Run(() => {
                            try
                            {
                                connection.Open();
                                return true;
                            }
                            catch
                            {
                                return false;
                            }
                        });

                        if (!openTask.Wait(TimeSpan.FromSeconds(OPERATION_TIMEOUT_SECONDS)) || !openTask.Result)
                        {
                            // Connection timeout or failure
                            return default;
                        }

                        // Execute query with timeout
                        var queryTask = Task.Run(() => {
                            try
                            {
                                return connection.ExecuteScalar<T>(query, parameters,
                                    commandTimeout: OPERATION_TIMEOUT_SECONDS);
                            }
                            catch
                            {
                                return default(T);
                            }
                        });

                        if (queryTask.Wait(TimeSpan.FromSeconds(OPERATION_TIMEOUT_SECONDS)))
                        {
                            return queryTask.Result;
                        }

                        // Query timeout
                        return default;
                    }
                }
                catch (Exception)
                {
                    return default;
                }
            }
        }

        /// <summary>
        /// Executes a non-query SQL command (INSERT, UPDATE, DELETE).
        /// Uses timeout control to avoid hanging the UI.
        /// </summary>
        /// <param name="connectionString">The SQL connection string.</param>
        /// <param name="query">The SQL command to execute.</param>
        /// <param name="parameters">Optional object containing query parameters.</param>
        /// <returns>The number of rows affected.</returns>
        public static int ExecuteNonQuery(string connectionString, string query, object parameters = null)
        {
            using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(OPERATION_TIMEOUT_SECONDS)))
            {
                try
                {
                    using (var connection = DatabaseConnectionManager.CreateConnection(connectionString))
                    {
                        // Open connection with timeout
                        var openTask = Task.Run(() => {
                            try
                            {
                                connection.Open();
                                return true;
                            }
                            catch
                            {
                                return false;
                            }
                        });

                        if (!openTask.Wait(TimeSpan.FromSeconds(OPERATION_TIMEOUT_SECONDS)) || !openTask.Result)
                        {
                            // Connection timeout or failure
                            return 0;
                        }

                        // Execute query with timeout
                        var queryTask = Task.Run(() => {
                            try
                            {
                                return connection.Execute(query, parameters,
                                    commandTimeout: OPERATION_TIMEOUT_SECONDS);
                            }
                            catch
                            {
                                return 0;
                            }
                        });

                        if (queryTask.Wait(TimeSpan.FromSeconds(OPERATION_TIMEOUT_SECONDS)))
                        {
                            return queryTask.Result;
                        }

                        // Query timeout
                        return 0;
                    }
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Executes multiple SQL commands within a transaction.
        /// Uses timeout control to avoid hanging the UI.
        /// </summary>
        /// <param name="connectionString">The SQL connection string.</param>
        /// <param name="actions">Action to perform within the transaction.</param>
        /// <returns>True if the transaction completed successfully, false otherwise.</returns>
        public static bool ExecuteInTransaction(string connectionString, Action<IDbConnection, IDbTransaction> actions)
        {
            using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(OPERATION_TIMEOUT_SECONDS * 2))) // Double timeout for transactions
            {
                try
                {
                    using (var connection = DatabaseConnectionManager.CreateConnection(connectionString))
                    {
                        // Open connection with timeout
                        var openTask = Task.Run(() => {
                            try
                            {
                                connection.Open();
                                return true;
                            }
                            catch
                            {
                                return false;
                            }
                        });

                        if (!openTask.Wait(TimeSpan.FromSeconds(OPERATION_TIMEOUT_SECONDS)) || !openTask.Result)
                        {
                            // Connection timeout or failure
                            return false;
                        }

                        using (var transaction = connection.BeginTransaction())
                        {
                            try
                            {
                                // Execute transaction with timeout
                                var transactionTask = Task.Run(() => {
                                    try
                                    {
                                        actions(connection, transaction);
                                        transaction.Commit();
                                        return true;
                                    }
                                    catch
                                    {
                                        transaction.Rollback();
                                        return false;
                                    }
                                });

                                if (transactionTask.Wait(TimeSpan.FromSeconds(OPERATION_TIMEOUT_SECONDS * 2)) &&
                                    transactionTask.Result)
                                {
                                    return true;
                                }

                                // Transaction timeout or failure
                                try { transaction.Rollback(); } catch { }
                                return false;
                            }
                            catch
                            {
                                try { transaction.Rollback(); } catch { }
                                return false;
                            }
                        }
                    }
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}