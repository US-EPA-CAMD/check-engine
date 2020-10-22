using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using ECMPS.Checks.DatabaseAccess;

namespace ECMPS.Checks.Parameters
{

  /// <summary>
  /// Indicates whether a database is closed, by another process, or opened by current process.
  /// </summary>
  public enum eDatabaseOpenType
  {
    /// <summary>
    /// Indicates that the process that called the database open closed method 
    /// acutally opened a closed database.
    /// </summary>
    Opened,

    /// <summary>
    /// Indicates that the database open closed method did not open the database
    /// because it was already open.
    /// </summary>
    AlreadyOpened,

    /// <summary>
    /// Indicates that an error occured while trying to open a database.
    /// </summary>
    Error
  }

  /// <summary>
  /// Contains utility methods used by check parameters and its user.
  /// </summary>
  public class cUtilities
  {

    #region Public Static Methods: Database with Types

    /// <summary>
    /// Opens the database if it was closed.
    /// </summary>
    /// <param name="ADatabase">The database to open</param>
    /// <param name="AErrorMessage">The error message corresponding to a return value of Error</param>
    /// <returns>Opened if the database was opened, AlreadyOpened if it was open on the call to the method and Error if an error occured while opening the database.</returns>
    public static eDatabaseOpenType Database_OpenClosed(cDatabase ADatabase, ref string AErrorMessage)
    {
      eDatabaseOpenType Result;

      if (ADatabase.State == ConnectionState.Closed)
      {
        if (ADatabase.Open())
          Result = eDatabaseOpenType.Opened;
        else
        {
          AErrorMessage = ADatabase.LastError;
          Result = eDatabaseOpenType.Error;
        }
      }
      else
        Result = eDatabaseOpenType.AlreadyOpened;

      return Result;
    }

    /// <summary>
    /// Closes a database if it was opened by the calling process.
    /// </summary>
    /// <param name="ADatabase">The database to close.</param>
    /// <param name="ADatabaseOpenType">Indicator of whether the calling process opened the database.</param>
    public static void Database_CloseOpened(cDatabase ADatabase, eDatabaseOpenType ADatabaseOpenType)
    {
      if (ADatabaseOpenType == eDatabaseOpenType.Opened)
      {
        ADatabase.Close();
      }
    }

    /// <summary>
    /// Calls cDatabase.ExecuteScalar, returning cDatabase.InternalError as its error message if an
    /// error occurs.
    /// </summary>
    /// <param name="ASql">The SQL statement to execute.</param>
    /// <param name="ADatabase">The database to execute the SQL against.</param>
    /// <param name="ADefault">The default value returned if the value returned is null or dbnull.</param>
    /// <param name="AResultScalar">The resulting value</param>
    /// <param name="AErrorMessage">The error message returned when the return value is false.</param>
    /// <returns>True if the SQL executed correctly.</returns>
    public static bool Database_ExecuteScalar(string ASql, cDatabase ADatabase,
                                              int ADefault, out int AResultScalar, ref string AErrorMessage)
    {
      try
      {
        eDatabaseOpenType DatabaseOpenType = cUtilities.Database_OpenClosed(ADatabase, ref AErrorMessage);

        if (DatabaseOpenType != eDatabaseOpenType.Error)
        {
          bool Result;

          object Scalar = ADatabase.ExecuteScalar(ASql);

          if ((Scalar != null) && (Scalar != DBNull.Value))
          {
            AResultScalar = Convert.ToInt32(Scalar);
            Result = true;
          }
          else
          {
            AResultScalar = ADefault;
            AErrorMessage = ADatabase.InternalError + " (" + ASql + ")";
            Result = false;
          }

          cUtilities.Database_CloseOpened(ADatabase, DatabaseOpenType);

          return Result;
        }
        else
        {
          AResultScalar = ADefault;
          return false;
        }
      }
      catch (Exception ex)
      {
        AResultScalar = ADefault;
        AErrorMessage = ex.Message + " (" + ASql + ")";
        return false;
      }
    }

    /// <summary>
    /// Calls cDatabase.GetDataTable, returning cDatabase.InternalError as its error message if an
    /// error occurs.
    /// </summary>
    /// <param name="ASql">The SQL statement to execute.</param>
    /// <param name="ADatabase">The database to execute the SQL against.</param>
    /// <param name="AResultTable">The resulting table.</param>
    /// <param name="AErrorMessage">The error message returned when the return value is false.</param>
    /// <returns>True if the SQL executed correctly.</returns>
    public static bool Database_GetDataTable(string ASql, cDatabase ADatabase,
                                             out DataTable AResultTable, ref string AErrorMessage)
    {
      try
      {
        eDatabaseOpenType DatabaseOpenType = cUtilities.Database_OpenClosed(ADatabase, ref AErrorMessage);

        if (DatabaseOpenType != eDatabaseOpenType.Error)
        {
          AResultTable = ADatabase.GetDataTable(ASql);

          if (AResultTable == null) AErrorMessage = ADatabase.InternalError + " (" + ASql + ")";

          cUtilities.Database_CloseOpened(ADatabase, DatabaseOpenType);

          return (AResultTable != null);
        }
        else
        {
          AResultTable = null;
          return false;
        }
      }
      catch (Exception ex)
      {
        AErrorMessage = ex.Message + " (" + ASql + ")";
        AResultTable = null;
        return false;
      }
    }

    #endregion


    #region Public Static Methods: Miscellaneous

    /// <summary>
    /// Creates an empty clone of the passed table.
    /// </summary>
    /// <param name="ASourceTable"></param>
    /// <returns></returns>
    public static DataTable CloneTable(DataTable ASourceTable)
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

    #endregion

  }

}
