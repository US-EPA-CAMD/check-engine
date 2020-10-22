using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using ECMPS.Checks.TypeUtilities;

namespace ECMPS.Checks.Parameters
{
  /// <summary>
  /// Implements the data view parameter for date ranged data.
  /// </summary>
  public class cCheckParameterDataViewLocationBased : cCheckParameterDataViewGeneric
  {

    #region Public Constructors

    /// <summary>
    /// Creates and instance of a cCheckParameterDataViewLocationBased object, setting the array property 
    /// to false, and the parameter name and types, and the source data table properties.  Also sets
    /// the began and ended date field name properties.
    /// </summary>
    /// <param name="AParameterKey">The key of the parameter.</param>
    /// <param name="AParameterName">The name of the parameter.</param>
    /// <param name="ASourceDataTable">The source data table for the parameter.</param>
    /// <param name="AMonitorLocationIdFieldName">The name of the monitor location id field.</param>
    /// <param name="ACheckParameters">The parent check parameters object.</param>
    public cCheckParameterDataViewLocationBased(int? AParameterKey, string AParameterName,
                                                DataTable ASourceDataTable,
                                                string AMonitorLocationIdFieldName,
                                                cCheckParameters ACheckParameters)
      : base(AParameterKey, AParameterName, ASourceDataTable, ACheckParameters)
    {
      FMonitorLocationIdFieldName = AMonitorLocationIdFieldName;
    }

    /// <summary>
    /// Creates and instance of a cCheckParameterDataViewDateRange object, setting the array property 
    /// to false, and the parameter name and types, and the source data table properties.  Also sets
    /// the began and ended date field name properties to "BEGAN_DATE" AND "END_DATE".
    /// </summary>
    /// <param name="AParameterKey">The key of the parameter.</param>
    /// <param name="AParameterName">The name of the parameter.</param>
    /// <param name="ASourceDataTable">The source data table for the parameter.</param>
    /// <param name="ACheckParameters">The parent check parameters object.</param>
    public cCheckParameterDataViewLocationBased(int? AParameterKey, string AParameterName,
                                                DataTable ASourceDataTable,
                                                cCheckParameters ACheckParameters)
      : base(AParameterKey, AParameterName, ASourceDataTable, ACheckParameters)
    {
      FMonitorLocationIdFieldName = "MON_LOC_ID";
    }

    #endregion


    #region Public Property with Fields

    #region Property Fields

    /// <summary>
    /// The name of the monitor location id field.
    /// </summary>
    protected string FMonitorLocationIdFieldName = null;

    #endregion


    /// <summary>
    /// The name of the monitor location id field.
    /// </summary>
    public string MonitorLocationIdFieldName { get { return FMonitorLocationIdFieldName; } }

    #endregion


    #region Public Virtual Methods: InitValue

    /// <summary>
    /// Scopes the source data to the active and filter specifications.
    /// </summary>
    /// <param name="ARowFilter">The filter condtions to apply to the parameter rows.</param>
    /// <param name="ANotFilter">The negates the result from applying the row filter.</param>
    /// <param name="AMonitorLocationId">The monitor location id to locate.</param>
    /// <param name="ADataSort">The sort to apply to the resulting data's default view.</param>
    /// <param name="AOwner">The check category making the request.</param>
    public virtual void InitValue(cFilterCondition[] ARowFilter, bool ANotFilter,
                                  string AMonitorLocationId, 
                                  string ADataSort, cCheckCategory AOwner)
    {
      if (SetValueCheckAndPrep(AOwner))
      {
        Scope(ARowFilter, ANotFilter, AMonitorLocationId);
        ApplySort(ADataSort, AOwner);
      }
    }

    /// <summary>
    /// Scopes the source data to the active and filter specifications.
    /// </summary>
    /// <param name="ARowFilter">The filter condtions to apply to the parameter rows.</param>
    /// <param name="ANotFilter">The negates the result from applying the row filter.</param>
    /// <param name="AMonitorLocationId">The monitor location id to locate.</param>
    /// <param name="ADataSort">The sort to apply to the resulting data's default view.</param>
    public virtual void InitValue(cFilterCondition[] ARowFilter, bool ANotFilter,
                                  string AMonitorLocationId,
                                  string ADataSort)
    {
      InitValue(ARowFilter, ANotFilter, AMonitorLocationId, ADataSort, (cCheckCategory)null);
    }

    /// <summary>
    /// Scopes the source data to the active and filter specifications.
    /// </summary>
    /// <param name="ARowFilter">The filter condtions to apply to the parameter rows.</param>
    /// <param name="AMonitorLocationId">The monitor location id to locate.</param>
    /// <param name="ADataSort">The sort to apply to the resulting data's default view.</param>
    /// <param name="AOwner">The check category making the request.</param>
    public virtual void InitValue(cFilterCondition[] ARowFilter,
                                  string AMonitorLocationId,
                                  string ADataSort, cCheckCategory AOwner)
    {
      if (SetValueCheckAndPrep(AOwner))
      {
        Scope(ARowFilter, false, AMonitorLocationId);
        ApplySort(ADataSort, AOwner);
      }
    }

    /// <summary>
    /// Scopes the source data to the active and filter specifications.
    /// </summary>
    /// <param name="ARowFilter">The filter condtions to apply to the parameter rows.</param>
    /// <param name="AMonitorLocationId">The monitor location id to locate.</param>
    /// <param name="ADataSort">The sort to apply to the resulting data's default view.</param>
    public virtual void InitValue(cFilterCondition[] ARowFilter,
                                  string AMonitorLocationId,
                                  string ADataSort)
    {
      InitValue(ARowFilter, AMonitorLocationId, ADataSort, (cCheckCategory)null);
    }

    /// <summary>
    /// Scopes the source data to the active and filter specifications.
    /// </summary>
    /// <param name="AMonitorLocationId">The monitor location id to locate.</param>
    /// <param name="ADataSort">The sort to apply to the resulting data's default view.</param>
    /// <param name="AOwner">The check category making the request.</param>
    public virtual void InitValue(string AMonitorLocationId,
                                  string ADataSort, cCheckCategory AOwner)
    {
      if (SetValueCheckAndPrep(AOwner))
      {
        Scope(AMonitorLocationId);
        ApplySort(ADataSort, AOwner);
      }
    }

    /// <summary>
    /// Scopes the source data to the active and filter specifications.
    /// </summary>
    /// <param name="AMonitorLocationId">The monitor location id to locate.</param>
    /// <param name="ADataSort">The sort to apply to the resulting data's default view.</param>
    public virtual void InitValue(string AMonitorLocationId,
                                  string ADataSort)
    {
      InitValue(AMonitorLocationId, ADataSort, (cCheckCategory)null);
    }

    /// <summary>
    /// Scopes the source data to the active and filter specifications.
    /// </summary>
    /// <param name="AOwner">The check category making the request.</param>
    /// <param name="ADataSort">The sort to apply to the resulting data's default view.</param>
    /// <param name="AMonitorLocationId">The monitor location id to locate.</param>
    /// <param name="ANotFilter">The negates the result from applying the row filter.</param>
    /// <param name="ARowFilter">The filter condtions to apply to the parameter rows.</param>
    public virtual void InitValue(cCheckCategory AOwner, string ADataSort,
                                  string AMonitorLocationId,
                                  bool ANotFilter, params cFilterCondition[] ARowFilter)
    {
      InitValue(ARowFilter, ANotFilter, AMonitorLocationId, ADataSort, AOwner);
    }

    /// <summary>
    /// Scopes the source data to the active and filter specifications.
    /// </summary>
    /// <param name="ADataSort">The sort to apply to the resulting data's default view.</param>
    /// <param name="AMonitorLocationId">The monitor location id to locate.</param>
    /// <param name="ANotFilter">The negates the result from applying the row filter.</param>
    /// <param name="ARowFilter">The filter condtions to apply to the parameter rows.</param>
    public virtual void InitValue(string ADataSort,
                                  string AMonitorLocationId,
                                  bool ANotFilter, params cFilterCondition[] ARowFilter)
    {
      InitValue(ARowFilter, ANotFilter, AMonitorLocationId, ADataSort, (cCheckCategory)null);
    }

    #endregion


    #region Protected Methods

    /// <summary>
    /// Sets the Scoped Data Table from Source Data Table and based on the passed active and 
    /// filter specification.
    /// </summary>
    /// <param name="ARowFilter">The filter condtions to apply to the parameter rows.</param>
    /// <param name="ANotFilter">The negates the result from applying the row filter.</param>
    /// <param name="AMonitorLocationId">The name of the monitor location id field.</param>
    protected void Scope(cFilterCondition[] ARowFilter, bool ANotFilter,
                         string AMonitorLocationId)
    {
      if (FSourceDataTable == null)
      {
        FLastErrorAction = "Scope";
        FLastErrorMessage = "Scope failed because source table has not been populated.";
        FScopedDataTable = null;
      }
      else
      {
        cFilterCondition[] TempFilter = { new cFilterCondition(FMonitorLocationIdFieldName, AMonitorLocationId) };
        DataView TempDataView = cRowFilter.FindRows(FSourceDataTable.DefaultView, TempFilter);

        DataView ScopedDataView = cRowFilter.FindRows(TempDataView,
                                                      ARowFilter, ANotFilter);

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
    /// <param name="AMonitorLocationId">The name of the monitor location id field.</param>
    protected void Scope(string AMonitorLocationId)
    {
      if (FSourceDataTable == null)
      {
        FLastErrorAction = "Scope";
        FLastErrorMessage = "Scope failed because source table has not been populated.";
        FScopedDataTable = null;
      }
      else
      {
        cFilterCondition[] RowFilter = { new cFilterCondition(FMonitorLocationIdFieldName, AMonitorLocationId) };

        DataView ScopedDataView = cRowFilter.FindRows(FSourceDataTable.DefaultView,
                                                      RowFilter);

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

    #endregion

  }
}
