using System.Data;
using System.Data.SqlClient;


namespace ECMPS.Checks.TypeUtilities
{
    /// <summary>
    /// Data handling class for checks
    /// </summary>
    public class cDataFunctions
    {
        /// <summary>
        /// Creates a comma delimited list of the values from a particular column in a dataview.
        /// </summary>
        /// <param name="ADataView">The data view</param>
        /// <param name="AColumnName">The column from which the data is extracted</param>
        /// <returns>The list of values from the specified column</returns>
        public static string ColumnToDatalist(DataView ADataView, string AColumnName)
        {
            if (ADataView.Table.Columns.Contains(AColumnName))
            {
                string List = "";
                string Delim = "";

                foreach (DataRowView Row in ADataView)
                {
                    List += Delim + cDBConvert.ToString(Row[AColumnName]);
                    Delim = ",";
                }

                return List;
            }
            else
                return "";
        }


        /// <summary>
        /// Returns a Data View containing the rows from the source view with the indicated key field value.
        /// </summary>
        /// <param name="ASourceView">The data view to search</param>
        /// <param name="AKeyField">The name of the key field</param>
        /// <param name="AKeyValue">The value expected in the key field</param>
        /// <returns>The data view containing the matching rows</returns>
        public static DataView RowsForMatchingKey(DataView ASourceView, string AKeyField, string AKeyValue)
        {
            if (ASourceView.Count > 0)
            {
                DataTable FilterTable = CloneTable(ASourceView.Table);
                DataRow FilterRow;

                foreach (DataRowView SourceRow in ASourceView)
                {
                    if (cDBConvert.ToString(SourceRow[AKeyField]) == AKeyValue)
                    {
                        FilterRow = FilterTable.NewRow();

                        foreach (DataColumn Column in FilterTable.Columns)
                            FilterRow[Column.ColumnName] = SourceRow[Column.ColumnName];

                        FilterTable.Rows.Add(FilterRow);
                    }
                }

                return FilterTable.DefaultView;
            }
            else
                return ASourceView;
        }

        private static DataTable CloneTable(DataTable ASourceTable)
        {
            DataTable FilterTable;

            try
            {
                DataColumn FilterColumn;

                FilterTable = new DataTable(ASourceTable.TableName);

                foreach (DataColumn SourceColumn in ASourceTable.Columns)
                {
                    FilterColumn = new DataColumn();

                    FilterColumn.ColumnName = SourceColumn.ColumnName;
                    FilterColumn.DataType = SourceColumn.DataType;
                    FilterColumn.MaxLength = SourceColumn.MaxLength;
                    FilterColumn.Unique = SourceColumn.Unique;

                    FilterTable.Columns.Add(FilterColumn);
                }
            }
            catch
            {
                FilterTable = null;
            }

            return FilterTable;
        }

        /// <summary>
        /// Returns an empty DataTable matching the catalog 
        /// </summary>
        /// <param name="databaseName">The name of the database containing the template table for the datatable.</param>
        /// <param name="schemaName">The name of the schema containing the template table for the datatable.</param>
        /// <param name="tableName">The name of the template table for the datatable.</param>
        /// <param name="connection">The connection to use to access the template table.</param>
        /// <param name="commandTimeout">The override command timeout.</param>
        /// <returns>Returns a database if successful, otherwise returns a null.</returns>
        public static DataTable CreateDataTable(string databaseName, string schemaName, string tableName, SqlConnection connection, int? commandTimeout = null)
        {
            try
            {
                string sql = "select * from " + databaseName + "." + schemaName + "." + tableName + " where null is not null";

                SqlDataAdapter DataAdapter = new SqlDataAdapter(sql, connection);

                if (commandTimeout != null) DataAdapter.SelectCommand.CommandTimeout = commandTimeout.Value;

                DataSet Dataset = new DataSet();

                DataAdapter.Fill(Dataset);

                if (Dataset.Tables.Count == 1)
                    return Dataset.Tables[0];
                else
                    return null;
            }
            catch
            {
                return null;
            }
        }
    }
}
