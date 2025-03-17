using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;

namespace Manny_Tools_Claude
{
    /// <summary>
    /// A helper class for executing SQL queries using Dapper and mapping the results to a generic type.
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
                try
                {
                    connection.Open();
                    var results = connection.Query<T>(query, parameters);
                    return results.AsList();
                }
                catch (Exception)
                {
                    return new List<T>();
                }
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
                try
                {
                    connection.Open();
                    return connection.QuerySingleOrDefault<T>(query, parameters);
                }
                catch (Exception)
                {
                    return default;
                }
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
                try
                {
                    connection.Open();
                    return connection.ExecuteScalar<T>(query, parameters);
                }
                catch (Exception)
                {
                    return default;
                }
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
                try
                {
                    connection.Open();
                    return connection.Execute(query, parameters);
                }
                catch (Exception)
                {
                    return 0;
                }
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
                try
                {
                    connection.Open();

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
                catch
                {
                    return false;
                }
            }
        }
    }
}