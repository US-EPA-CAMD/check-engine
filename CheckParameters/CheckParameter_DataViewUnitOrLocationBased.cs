using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Extensions;


namespace ECMPS.Checks.Parameters
{
  /// <summary>
  /// Implements the data view parameter for date ranged data.
  /// </summary>
  public class cCheckParameterDataViewUnitOrLocationBased : cCheckParameterDataViewGeneric
  {

    #region Public Constructors

    /// <summary>
    /// Creates and instance of a cCheckParameterDataViewUnitOrLocationBased object, setting the array property 
    /// to false, and the parameter name and types, and the source data table properties.  Also sets
    /// the began and ended date field name properties.
    /// </summary>
    /// <param name="AParameterKey">The key of the parameter.</param>
    /// <param name="AParameterName">The name of the parameter.</param>
    /// <param name="ASourceDataTable">The source data table for the parameter.</param>
    /// <param name="AUnitMonLocIdFieldName">The name of the unit monitor location id field.</param>
    /// <param name="AStackMonLocIdFieldName">The name of the stack monitor location id field.</param>
    /// <param name="ABeganDateFieldName">The name of the began date field.</param>
    /// <param name="AEndedDateFieldName">The name of the ended date field.</param>
    /// <param name="ACheckParameters">The parent check parameters object.</param>
    public cCheckParameterDataViewUnitOrLocationBased(int? AParameterKey, string AParameterName,
                                                      DataTable ASourceDataTable,
                                                      string AUnitMonLocIdFieldName,
                                                      string AStackMonLocIdFieldName,
                                                      string ABeganDateFieldName,
                                                      string AEndedDateFieldName,
                                                      cCheckParameters ACheckParameters)
      : base(AParameterKey, AParameterName, ASourceDataTable, ACheckParameters)
    {
      FUnitMonLocIdFieldName = AUnitMonLocIdFieldName;
      FStackMonLocIdFieldName = AStackMonLocIdFieldName;
      FBeganDateFieldName = ABeganDateFieldName;
      FEndedDateFieldName = AEndedDateFieldName;
    }

    /// <summary>
    /// Creates and instance of a cCheckParameterDataViewUnitOrLocationBased object, setting the array property 
    /// to false, and the parameter name and types, and the source data table properties.  Also sets
    /// the began and ended date field name properties.
    /// </summary>
    /// <param name="AParameterKey">The key of the parameter.</param>
    /// <param name="AParameterName">The name of the parameter.</param>
    /// <param name="ASourceDataTable">The source data table for the parameter.</param>
    /// <param name="AUnitMonLocIdFieldName">The name of the unit monitor location id field.</param>
    /// <param name="AStackMonLocIdFieldName">The name of the stack monitor location id field.</param>
    /// <param name="ACheckParameters">The parent check parameters object.</param>
    public cCheckParameterDataViewUnitOrLocationBased(int? AParameterKey, string AParameterName,
                                                      DataTable ASourceDataTable,
                                                      string AUnitMonLocIdFieldName,
                                                      string AStackMonLocIdFieldName,
                                                      cCheckParameters ACheckParameters)
      : base(AParameterKey, AParameterName, ASourceDataTable, ACheckParameters)
    {
      FUnitMonLocIdFieldName = AUnitMonLocIdFieldName;
      FStackMonLocIdFieldName = AStackMonLocIdFieldName;
      FBeganDateFieldName = "BEGIN_DATE";
      FEndedDateFieldName = "END_DATE";
    }

    #endregion


    #region Public Property with Fields

    #region Property Fields

    /// <summary>
    /// The name of the began date field.
    /// </summary>
    protected string FBeganDateFieldName = null;

    /// <summary>
    /// The name of the ended date field.
    /// </summary>
    protected string FEndedDateFieldName = null;

    /// <summary>
    /// The name of the stack monitor location id field.
    /// </summary>
    protected string FStackMonLocIdFieldName = null;

    /// <summary>
    /// The name of the unit monitor location id field.
    /// </summary>
    protected string FUnitMonLocIdFieldName = null;

    #endregion


    /// <summary>
    /// The name of the began date field.
    /// </summary>
    public string BeganDateFieldName { get { return FBeganDateFieldName; } }

    /// <summary>
    /// The name of the ended date field.
    /// </summary>
    public string EndedDateFieldName { get { return FEndedDateFieldName; } }

    /// <summary>
    /// The name of the stack monitor location id field.
    /// </summary>
    public string StackMonLocIdFieldName { get { return FStackMonLocIdFieldName; } }

    /// <summary>
    /// The name of the unit monitor location id field.
    /// </summary>
    public string UnitMonLocIdFieldName { get { return FUnitMonLocIdFieldName; } }

    #endregion


    #region Public Virtual Methods: InitValue

    /// <summary>
    /// Scopes the source data to the active and filter specifications.
    /// </summary>
    /// <param name="AMonitorLocationIdList">A comma separated list of monitor location ids to locate.</param>
    /// <param name="ABeganDate">The active period began date.</param>
    /// <param name="AEndedDate">The active period ended date.</param>
    /// <param name="ADataSort">The sort to apply to the resulting data's default view.</param>
    /// <param name="AOwner">The check category making the request.</param>
    public virtual void InitValue(string AMonitorLocationIdList,
                                  DateTime ABeganDate, DateTime AEndedDate,
                                  string ADataSort, cCheckCategory AOwner)
    {
      if (SetValueCheckAndPrep(AOwner))
      {
        Scope(AMonitorLocationIdList, ABeganDate, AEndedDate);
        ApplySort(ADataSort, AOwner);
      }
    }

    /// <summary>
    /// Scopes the source data to the active and filter specifications.
    /// </summary>
    /// <param name="AMonitorLocationIdList">A comma separated list of monitor location ids to locate.</param>
    /// <param name="ABeganDate">The active period began date.</param>
    /// <param name="AEndedDate">The active period ended date.</param>
    /// <param name="ADataSort">The sort to apply to the resulting data's default view.</param>
    public virtual void InitValue(string AMonitorLocationIdList,
                                  DateTime ABeganDate, DateTime AEndedDate,
                                  string ADataSort)
    {
      InitValue(AMonitorLocationIdList, ABeganDate, AEndedDate, ADataSort, (cCheckCategory)null);
    }

    /// <summary>
    /// Scopes the source data to the active and filter specifications.
    /// </summary>
    /// <param name="AMonitorLocationIdList">A comma separated list of monitor location ids to locate.</param>
    /// <param name="AOpDate">The operating date to test rows against</param>
    /// <param name="ADataSort">The sort to apply to the resulting data's default view.</param>
    /// <param name="AOwner">The check category making the request.</param>
    public virtual void InitValue(string AMonitorLocationIdList,
                                  DateTime AOpDate,
                                  string ADataSort, cCheckCategory AOwner)
    {
      if (SetValueCheckAndPrep(AOwner))
      {
        Scope(AMonitorLocationIdList, AOpDate);
        ApplySort(ADataSort, AOwner);
      }
    }

    /// <summary>
    /// Scopes the source data to the active and filter specifications.
    /// </summary>
    /// <param name="AMonitorLocationIdList">A comma separated list of monitor location ids to locate.</param>
    /// <param name="AOpDate">The operating date to test rows against</param>
    /// <param name="ADataSort">The sort to apply to the resulting data's default view.</param>
    public virtual void InitValue(string AMonitorLocationIdList,
                                  DateTime AOpDate,
                                  string ADataSort)
    {
      InitValue(AMonitorLocationIdList, AOpDate, ADataSort, (cCheckCategory)null);
    }

    #endregion


    #region Protected Methods

    /// <summary>
    /// Sets the Scoped Data Table from Source Data Table and based on the passed active and 
    /// filter specification.
    /// </summary>
    /// <param name="AMonitorLocationIdList">A comma separated list of monitor location ids to locate.</param>
    /// <param name="AOpDate">The operating date to test rows against</param>
    protected void Scope(string AMonitorLocationIdList, DateTime AOpDate)
    {
      if (FSourceDataTable == null)
      {
        FLastErrorAction = "Scope";
        FLastErrorMessage = "Scope failed because source table has not been populated.";
        FScopedDataTable = null;
      }
      else
      {
        DataView ScopedDataView = ScopeFilter(AOpDate, AMonitorLocationIdList);

        if (ScopedDataView != null)
          FScopedDataTable = ScopedDataView.Table;
        else
        {
          FLastErrorAction = "Scope";
          FLastErrorMessage = "Scope failed because source table has not been populated.";
          FScopedDataTable = null;
        }
      }
    }

    /// <summary>
    /// Sets the Scoped Data Table from Source Data Table and based on the passed active specification.
    /// </summary>
    /// <param name="AMonitorLocationIdList">A comma separated list of monitor location ids to locate.</param>
    /// <param name="ABeganDate">The active period began date.</param>
    /// <param name="AEndedDate">The active period ended date.</param>
    protected void Scope(string AMonitorLocationIdList, DateTime ABeganDate, DateTime AEndedDate)
    {
      if (FSourceDataTable == null)
      {
        FLastErrorAction = "Scope";
        FLastErrorMessage = "Scope failed because source table has not been populated.";
        FScopedDataTable = null;
      }
      else
      {
        DataView ScopedDataView = ScopeFilter(ABeganDate, AEndedDate, AMonitorLocationIdList);

        if (ScopedDataView != null)
          FScopedDataTable = ScopedDataView.Table;
        else
        {
          FLastErrorAction = "Scope";
          FLastErrorMessage = "Scope failed because source table has not been populated.";
          FScopedDataTable = null;
        }
      }
    }

    /// <summary>
    /// Filters from Source Data Table and based on the passed active and 
    /// filter specification.
    /// </summary>
    /// <param name="AMonitorLocationIdList">A comma separated list of monitor location ids to locate.</param>
    /// <param name="AOpDate">The operating date to test rows against</param>
    protected DataView ScopeFilter(DateTime AOpDate, string AMonitorLocationIdList)
    {
      DataRow FilterRow;
      DateTime BeganDate; DateTime EndedDate;
      string StackPipeMonLocId, UnitMonLocId;

      DataTable FilterTable = cUtilities.CloneTable(FSourceDataTable);

      FilterTable.Rows.Clear();

      foreach (DataRow SourceRow in FSourceDataTable.Rows)
      {
        BeganDate = cDBConvert.ToDate(SourceRow[FBeganDateFieldName], DateTypes.START);
        EndedDate = cDBConvert.ToDate(SourceRow[FEndedDateFieldName], DateTypes.END);
        UnitMonLocId = cDBConvert.ToString(SourceRow[FUnitMonLocIdFieldName]);
        StackPipeMonLocId = cDBConvert.ToString(SourceRow[FStackMonLocIdFieldName]);

        if ((UnitMonLocId.InList(AMonitorLocationIdList) ||
             StackPipeMonLocId.InList(AMonitorLocationIdList)) &&
            ((BeganDate <= AOpDate) && (AOpDate <= EndedDate)))
        {
          FilterRow = FilterTable.NewRow();

          foreach (DataColumn Column in FilterTable.Columns)
            FilterRow[Column.ColumnName] = SourceRow[Column.ColumnName];

          FilterTable.Rows.Add(FilterRow);
        }
      }

      FilterTable.AcceptChanges();

      return FilterTable.DefaultView;
    }

    /// <summary>
    /// Filter from Source Data Table and based on the passed active specification.
    /// </summary>
    /// <param name="AMonitorLocationIdList">A comma separated list of monitor location ids to locate.</param>
    /// <param name="ABeganDate">The active period began date.</param>
    /// <param name="AEndedDate">The active period ended date.</param>
    protected DataView ScopeFilter(DateTime ABeganDate, DateTime AEndedDate, string AMonitorLocationIdList)
    {
      DataRow FilterRow;
      DateTime BeganDate; DateTime EndedDate;
      string StackPipeMonLocId, UnitMonLocId;

      DataTable FilterTable = cUtilities.CloneTable(FSourceDataTable);

      FilterTable.Rows.Clear();

      foreach (DataRow SourceRow in FSourceDataTable.Rows)
      {
        BeganDate = cDBConvert.ToDate(SourceRow[FBeganDateFieldName], DateTypes.START);
        EndedDate = cDBConvert.ToDate(SourceRow[FEndedDateFieldName], DateTypes.END);
        UnitMonLocId = cDBConvert.ToString(SourceRow[FUnitMonLocIdFieldName]);
        StackPipeMonLocId = cDBConvert.ToString(SourceRow[FStackMonLocIdFieldName]);

        if ((UnitMonLocId.InList(AMonitorLocationIdList) ||
             StackPipeMonLocId.InList(AMonitorLocationIdList)) &&
            ((BeganDate <= AEndedDate) && (ABeganDate <= EndedDate)))
        {
          FilterRow = FilterTable.NewRow();

          foreach (DataColumn Column in FilterTable.Columns)
            FilterRow[Column.ColumnName] = SourceRow[Column.ColumnName];

          FilterTable.Rows.Add(FilterRow);
        }
      }

      FilterTable.AcceptChanges();

      return FilterTable.DefaultView;
    }

    #endregion

  }
}
