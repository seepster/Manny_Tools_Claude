using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Manny_Tools_Claude;
using Microsoft.Data.SqlClient;

namespace Manny_Tools_Claude
{
    /// <summary>
    /// Utility class to map database schema to a DataGridView
    /// </summary>
    public class SQL_Mapper_Schema
    {
        private readonly string _connectionString;

        public SQL_Mapper_Schema(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        /// <summary>
        /// Maps the database schema to a DataGridView
        /// </summary>
        /// <param name="dataGridView">The DataGridView to populate</param>
        public void MapSchemaToGrid(DataGridView dataGridView)
        {
            if (dataGridView == null) throw new ArgumentNullException(nameof(dataGridView));

            // Get the database tables and their columns
            var tables = GetDatabaseTables();
            var columnsData = GetTableColumns();

            // Create a grouped view of the columns by table
            var tableColumns = columnsData
                .GroupBy(c => c.TableName)
                .ToDictionary(g => g.Key, g => g.ToList());

            // Find the maximum number of columns in any table to determine grid size
            int maxColumnCount = tableColumns.Values.Max(cols => cols.Count);

            // Create DataTable to hold our view
            DataTable schemaTable = new DataTable();

            // Add table name row
            schemaTable.Columns.Add("Property", typeof(string));

            // Add columns for each table
            foreach (var table in tables)
            {
                schemaTable.Columns.Add(table.TableName, typeof(string));
            }

            // Add column name rows
            for (int i = 0; i < maxColumnCount; i++)
            {
                DataRow row = schemaTable.NewRow();
                row["Property"] = $"Column {i + 1}";

                foreach (var table in tables)
                {
                    if (tableColumns.TryGetValue(table.TableName, out var columns) && i < columns.Count)
                    {
                        row[table.TableName] = columns[i].ColumnName;
                    }
                    else
                    {
                        row[table.TableName] = string.Empty;
                    }
                }

                schemaTable.Rows.Add(row);
            }

            // Add data type row
            DataRow dataTypeRow = schemaTable.NewRow();
            dataTypeRow["Property"] = "Data Types";

            foreach (var table in tables)
            {
                if (tableColumns.TryGetValue(table.TableName, out var columns) && columns.Count > 0)
                {
                    var dataTypes = string.Join(", ", columns.Select(c => $"{c.ColumnName}: {c.DataType}"));
                    dataTypeRow[table.TableName] = dataTypes;
                }
                else
                {
                    dataTypeRow[table.TableName] = string.Empty;
                }
            }
            schemaTable.Rows.Add(dataTypeRow);

            // Add primary key row
            DataRow pkRow = schemaTable.NewRow();
            pkRow["Property"] = "Primary Keys";

            foreach (var table in tables)
            {
                if (tableColumns.TryGetValue(table.TableName, out var columns) && columns.Count > 0)
                {
                    var pks = string.Join(", ", columns.Where(c => c.IsPrimaryKey).Select(c => c.ColumnName));
                    pkRow[table.TableName] = pks;
                }
                else
                {
                    pkRow[table.TableName] = string.Empty;
                }
            }
            schemaTable.Rows.Add(pkRow);

            // Add row count row
            DataRow rowCountRow = schemaTable.NewRow();
            rowCountRow["Property"] = "Row Count";

            foreach (var table in tables)
            {
                var rowCount = GetTableRowCount(table.TableName);
                rowCountRow[table.TableName] = rowCount.ToString();
            }
            schemaTable.Rows.Add(rowCountRow);

            // Set the DataGridView's DataSource
            dataGridView.DataSource = schemaTable;

            // Format the DataGridView
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.ReadOnly = true;
            dataGridView.AllowUserToAddRows = false;
            dataGridView.AllowUserToDeleteRows = false;
            dataGridView.RowHeadersVisible = false;
            dataGridView.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.AliceBlue;
        }

        /// <summary>
        /// Get first 10 rows of data for a specific table and display in a separate DataGridView
        /// </summary>
        /// <param name="tableName">Name of table to get sample data from</param>
        /// <param name="dataGridView">DataGridView to populate with the sample data</param>
        public void ShowTableData(string tableName, DataGridView dataGridView)
        {
            if (string.IsNullOrEmpty(tableName)) throw new ArgumentNullException(nameof(tableName));
            if (dataGridView == null) throw new ArgumentNullException(nameof(dataGridView));

            // Get sample data (first 10 rows)
            string query = $"SELECT TOP 10 * FROM [{tableName}]";
            var sampleData = SQL_Get_Generic_List.ExecuteQuery<dynamic>(_connectionString, query);

            // Create a DataTable
            DataTable dataTable = new DataTable();

            // If we have data, populate the grid
            if (sampleData != null && sampleData.Count > 0)
            {
                // Get the first row to determine columns
                var firstRow = sampleData.First();

                // Add columns to DataTable based on the properties in the dynamic object
                foreach (var prop in firstRow.GetType().GetProperties())
                {
                    dataTable.Columns.Add(prop.Name, prop.PropertyType);
                }

                // Add rows
                foreach (var item in sampleData)
                {
                    DataRow row = dataTable.NewRow();
                    foreach (var prop in item.GetType().GetProperties())
                    {
                        row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                    }
                    dataTable.Rows.Add(row);
                }
            }

            // Set the DataGridView's DataSource
            dataGridView.DataSource = dataTable;

            // Format the DataGridView
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.ReadOnly = true;
            dataGridView.AllowUserToAddRows = false;
            dataGridView.AllowUserToDeleteRows = false;
            dataGridView.RowHeadersVisible = false;
            dataGridView.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.AliceBlue;
        }

        /// <summary>
        /// Get all tables in the database
        /// </summary>
        private List<TableInfo> GetDatabaseTables()
        {
            string query = @"
                SELECT 
                    t.name AS TableName,
                    SCHEMA_NAME(t.schema_id) AS SchemaName
                FROM 
                    sys.tables t
                ORDER BY 
                    SchemaName, TableName";

            return SQL_Get_Generic_List.ExecuteQuery<TableInfo>(_connectionString, query);
        }

        /// <summary>
        /// Get all columns for all tables
        /// </summary>
        private List<ColumnInfo> GetTableColumns()
        {
            string query = @"
                SELECT 
                    t.name AS TableName,
                    c.name AS ColumnName,
                    TYPE_NAME(c.user_type_id) AS DataType,
                    c.max_length AS MaxLength,
                    c.precision AS Precision,
                    c.scale AS Scale,
                    c.is_nullable AS IsNullable,
                    CASE WHEN pk.column_id IS NOT NULL THEN 1 ELSE 0 END AS IsPrimaryKey,
                    c.column_id AS ColumnOrder
                FROM 
                    sys.tables t
                    INNER JOIN sys.columns c ON c.object_id = t.object_id
                    LEFT JOIN sys.index_columns ic ON ic.object_id = t.object_id AND ic.column_id = c.column_id
                    LEFT JOIN sys.indexes i ON i.object_id = t.object_id AND i.index_id = ic.index_id AND i.is_primary_key = 1
                    LEFT JOIN (
                        SELECT index_columns.object_id, index_columns.column_id
                        FROM sys.index_columns
                        INNER JOIN sys.indexes ON indexes.object_id = index_columns.object_id 
                        AND indexes.index_id = index_columns.index_id
                        WHERE indexes.is_primary_key = 1
                    ) pk ON pk.object_id = t.object_id AND pk.column_id = c.column_id
                ORDER BY 
                    TableName, ColumnOrder";

            return SQL_Get_Generic_List.ExecuteQuery<ColumnInfo>(_connectionString, query);
        }

        /// <summary>
        /// Get row count for a specific table
        /// </summary>
        private int GetTableRowCount(string tableName)
        {
            string query = $"SELECT COUNT(*) FROM [{tableName}]";
            return SQL_Get_Generic_List.ExecuteScalar<int>(_connectionString, query);
        }

        /// <summary>
        /// Class representing a database table
        /// </summary>
        public class TableInfo
        {
            public string TableName { get; set; }
            public string SchemaName { get; set; }
        }

        /// <summary>
        /// Class representing a database column
        /// </summary>
        public class ColumnInfo
        {
            public string TableName { get; set; }
            public string ColumnName { get; set; }
            public string DataType { get; set; }
            public short MaxLength { get; set; }
            public byte Precision { get; set; }
            public byte Scale { get; set; }
            public bool IsNullable { get; set; }
            public bool IsPrimaryKey { get; set; }
            public int ColumnOrder { get; set; }
        }
    }
}