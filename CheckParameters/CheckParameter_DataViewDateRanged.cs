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
  public class cCheckParameterDataViewDateRanged : cCheckParameterDataViewGeneric
  {

    #region Public Constructors

    /// <summary>
    /// Creates and instance of a cCheckParameterDataViewDateRange object, setting the array property 
    /// to false, and the parameter name and types, and the source data table properties.  Also sets
    /// the began and ended date field name properties.
    /// </summary>
    /// <param name="AParameterKey">The key of the parameter.</param>
    /// <param name="AParameterName">The name of the parameter.</param>
    /// <param name="ASourceDataTable">The source data table for the parameter.</param>
    /// <param name="ABeganDateFieldName">The name of the began date field.</param>
    /// <param name="AEndedDateFieldName">The name of the ended date field.</param>
    /// <param name="ACheckParameters">The parent check parameters object.</param>
    public cCheckParameterDataViewDateRanged(int? AParameterKey, string AParameterName,
                                             DataTable ASourceDataTable,
                                             string ABeganDateFieldName, string AEndedDateFieldName,
                                             cCheckParameters ACheckParameters)
      : base(AParameterKey, AParameterName, ASourceDataTable, ACheckParameters)
    {
      FBeganDateFieldName = ABeganDateFieldName;
      FEndedDateFieldName = AEndedDateFieldName;
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
    public cCheckParameterDataViewDateRanged(int? AParameterKey, string AParameterName,
                                             DataTable ASourceDataTable,
                                             cCheckParameters ACheckParameters)
      : base(AParameterKey, AParameterName, ASourceDataTable, ACheckParameters)
    {
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

    #endregion


    /// <summary>
    /// The name of the began date field.
    /// </summary>
    public string BeganDateFieldName { get { return FBeganDateFieldName; } }

    /// <summary>
    /// The name of the ended date field.
    /// </summary>
    public string EndedDateFieldName { get { return FEndedDateFieldName; } }

    #endregion


    #region Public Virtual Methods: Filter

    /// <summary>
    /// Filters the scoped data using the active and filter specifications.
    /// </summary>
    /// <param name="ARowFilter">The filter condtions to apply to the parameter rows.</param>
    /// <param name="ANotFilter">The negates the result from applying the row filter.</param>
    /// <param name="ABeganDate">The active period began date.</param>
    /// <param name="AEndedDate">The active period ended date.</param>
    /// <returns>A data view of the filter results.</returns>
    public virtual DataView Filter(cFilterCondition[] ARowFilter, bool ANotFilter, 
                                   DateTime ABeganDate, DateTime AEndedDate)
    {
      if (FScopedDataTable != null)
        return cRowFilter.FindActiveRows(FScopedDataTable.DefaultView,
                                         ABeganDate, AEndedDate,
                                         FBeganDateFieldName, FEndedDateFieldName,
                                         ANotFilter, ARowFilter);
      else
        return null;
    }

    /// <summary>
    /// Filters the scoped data using the active and filter specifications.
    /// </summary>
    /// <param name="ARowFilter">The filter condtions to apply to the parameter rows.</param>
    /// <param name="ABeganDate">The active period began date.</param>
    /// <param name="AEndedDate">The active period ended date.</param>
    /// <returns>A data view of the filter results.</returns>
    public virtual DataView Filter(cFilterCondition[] ARowFilter, 
                                   DateTime ABeganDate, DateTime AEndedDate)
    {
      if (FScopedDataTable != null)
        return cRowFilter.FindActiveRows(FScopedDataTable.DefaultView,
                                         ABeganDate, AEndedDate,
                                         FBeganDateFieldName, FEndedDateFieldName,
                                         false, ARowFilter);
      else
        return null;
    }

    /// <summary>
    /// Filters the scoped data using the active specifications.
    /// </summary>
    /// <param name="ABeganDate">The active period began date.</param>
    /// <param name="AEndedDate">The active period ended date.</param>
    /// <returns>A data view of the filter results.</returns>
    public virtual DataView Filter(DateTime ABeganDate, DateTime AEndedDate)
    {
      if (FScopedDataTable != null)
        return cRowFilter.FindActiveRows(FScopedDataTable.DefaultView,
                                         ABeganDate, AEndedDate,
                                         FBeganDateFieldName, FEndedDateFieldName);
      else
        return null;
    }

    /// <summary>
    /// Filters the scoped data using the active and filter specifications.
    /// </summary>
    /// <param name="ABeganDate">The active period began date.</param>
    /// <param name="AEndedDate">The active period ended date.</param>
    /// <param name="ANotFilter">The negates the result from applying the row filter.</param>
    /// <param name="ARowFilter">The filter condtions to apply to the parameter rows.</param>
    /// <returns>A data view of the filter results.</returns>
    public virtual DataView Filter(DateTime ABeganDate, DateTime AEndedDate,
                                   bool ANotFilter, params cFilterCondition[] ARowFilter)
    {
      if (FScopedDataTable != null)
        return cRowFilter.FindActiveRows(FSourceDataTable.DefaultView,
                                         ABeganDate, AEndedDate,
                                         FBeganDateFieldName, FEndedDateFieldName,
                                         ANotFilter, ARowFilter);
      else
        return null;
    }

    /// <summary>
    /// Filters the scoped data using the active and filter specifications.
    /// </summary>
    /// <param name="ARowFilter">The filter condtions to apply to the parameter rows.</param>
    /// <param name="ANotFilter">The negates the result from applying the row filter.</param>
    /// <param name="AOpDate">The operating date to test rows against</param>
    /// <returns>A data view of the filter results.</returns>
    public virtual DataView Filter(cFilterCondition[] ARowFilter, bool ANotFilter,
                                   DateTime AOpDate)
    {
      if (FScopedDataTable != null)
        return cRowFilter.FindActiveRows(FScopedDataTable.DefaultView,
                                         AOpDate,
                                         FBeganDateFieldName, FEndedDateFieldName,
                                         ANotFilter, ARowFilter);
      else
        return null;
    }

    /// <summary>
    /// Filters the scoped data using the active and filter specifications.
    /// </summary>
    /// <param name="ARowFilter">The filter condtions to apply to the parameter rows.</param>
    /// <param name="AOpDate">The operating date to test rows against</param>
    /// <returns>A data view of the filter results.</returns>
    public virtual DataView Filter(cFilterCondition[] ARowFilter,
                                   DateTime AOpDate)
    {
      if (FScopedDataTable != null)
        return cRowFilter.FindActiveRows(FScopedDataTable.DefaultView,
                                         AOpDate,
                                         FBeganDateFieldName, FEndedDateFieldName,
                                         false, ARowFilter);
      else
        return null;
    }

    /// <summary>
    /// Filters the scoped data using the active specifications.
    /// </summary>
    /// <param name="AOpDate">The operating date to test rows against</param>
    /// <returns>A data view of the filter results.</returns>
    public virtual DataView Filter(DateTime AOpDate)
    {
      if (FScopedDataTable != null)
        return cRowFilter.FindActiveRows(FScopedDataTable.DefaultView,
                                         AOpDate,
                                         FBeganDateFieldName, FEndedDateFieldName);
      else
        return null;
    }

    /// <summary>
    /// Filters the scoped data using the active and filter specifications.
    /// </summary>
    /// <param name="AOpDate">The operating date to test rows against</param>
    /// <param name="ANotFilter">The negates the result from applying the row filter.</param>
    /// <param name="ARowFilter">The filter condtions to apply to the parameter rows.</param>
    /// <returns>A data view of the filter results.</returns>
    public virtual DataView Filter(DateTime AOpDate,
                                   bool ANotFilter, params cFilterCondition[] ARowFilter)
    {
      if (FScopedDataTable != null)
        return cRowFilter.FindActiveRows(FSourceDataTable.DefaultView,
                                         AOpDate,
                                         FBeganDateFieldName, FEndedDateFieldName,
                                         ANotFilter, ARowFilter);
      else
        return null;
    }

    #endregion


    #region Public Virtual Methods: InitValue

    /// <summary>
    /// Scopes the source data to the active and filter specifications.
    /// </summary>
    /// <param name="ARowFilter">The filter condtions to apply to the parameter rows.</param>
    /// <param name="ANotFilter">The negates the result from applying the row filter.</param>
    /// <param name="ABeganDate">The active period began date.</param>
    /// <param name="AEndedDate">The active period ended date.</param>
    /// <param name="ADataSort">The sort to apply to the resulting data's default view.</param>
    /// <param name="AOwner">The check category making the request.</param>
    public virtual void InitValue(cFilterCondition[] ARowFilter, bool ANotFilter,
                                  DateTime ABeganDate, DateTime AEndedDate, 
                                  string ADataSort, cCheckCategory AOwner)
    {
      if (SetValueCheckAndPrep(AOwner))
      {
        Scope(ARowFilter, ANotFilter, ABeganDate, AEndedDate);
        ApplySort(ADataSort, AOwner);
      }
    }

    /// <summary>
    /// Scopes the source data to the active and filter specifications.
    /// </summary>
    /// <param name="ARowFilter">The filter condtions to apply to the parameter rows.</param>
    /// <param name="ANotFilter">The negates the result from applying the row filter.</param>
    /// <param name="ABeganDate">The active period began date.</param>
    /// <param name="AEndedDate">The active period ended date.</param>
    /// <param name="ADataSort">The sort to apply to the resulting data's default view.</param>
    public virtual void InitValue(cFilterCondition[] ARowFilter, bool ANotFilter,
                                  DateTime ABeganDate, DateTime AEndedDate,
                                  string ADataSort)
    {
      InitValue(ARowFilter, ANotFilter, ABeganDate, AEndedDate, ADataSort, (cCheckCategory)null);
    }

    /// <summary>
    /// Scopes the source data to the active and filter specifications.
    /// </summary>
    /// <param name="ARowFilter">The filter condtions to apply to the parameter rows.</param>
    /// <param name="ABeganDate">The active period began date.</param>
    /// <param name="AEndedDate">The active period ended date.</param>
    /// <param name="ADataSort">The sort to apply to the resulting data's default view.</param>
    /// <param name="AOwner">The check category making the request.</param>
    public virtual void InitValue(cFilterCondition[] ARowFilter,
                                  DateTime ABeganDate, DateTime AEndedDate,
                                  string ADataSort, cCheckCategory AOwner)
    {
      if (SetValueCheckAndPrep(AOwner))
      {
        Scope(ARowFilter, false, ABeganDate, AEndedDate);
        ApplySort(ADataSort, AOwner);
      }
    }

    /// <summary>
    /// Scopes the source data to the active and filter specifications.
    /// </summary>
    /// <param name="ARowFilter">The filter condtions to apply to the parameter rows.</param>
    /// <param name="ABeganDate">The active period began date.</param>
    /// <param name="AEndedDate">The active period ended date.</param>
    /// <param name="ADataSort">The sort to apply to the resulting data's default view.</param>
    public virtual void InitValue(cFilterCondition[] ARowFilter,
                                  DateTime ABeganDate, DateTime AEndedDate,
                                  string ADataSort)
    {
      InitValue(ARowFilter, ABeganDate, AEndedDate, ADataSort, (cCheckCategory)null);
    }

    /// <summary>
    /// Scopes the source data to the active and filter specifications.
    /// </summary>
    /// <param name="ABeganDate">The active period began date.</param>
    /// <param name="AEndedDate">The active period ended date.</param>
    /// <param name="ADataSort">The sort to apply to the resulting data's default view.</param>
    /// <param name="AOwner">The check category making the request.</param>
    public virtual void InitValue(DateTime ABeganDate, DateTime AEndedDate,
                                  string ADataSort, cCheckCategory AOwner)
    {
      if (SetValueCheckAndPrep(AOwner))
      {
        Scope(ABeganDate, AEndedDate);
        ApplySort(ADataSort, AOwner);
      }
    }

    /// <summary>
    /// Scopes the source data to the active and filter specifications.
    /// </summary>
    /// <param name="ABeganDate">The active period began date.</param>
    /// <param name="AEndedDate">The active period ended date.</param>
    /// <param name="ADataSort">The sort to apply to the resulting data's default view.</param>
    public virtual void InitValue(DateTime ABeganDate, DateTime AEndedDate,
                                  string ADataSort)
    {
      InitValue(ABeganDate, AEndedDate, ADataSort, (cCheckCategory)null);
    }

    /// <summary>
    /// Scopes the source data to the active and filter specifications.
    /// </summary>
    /// <param name="AOwner">The check category making the request.</param>
    /// <param name="ADataSort">The sort to apply to the resulting data's default view.</param>
    /// <param name="ABeganDate">The active period began date.</param>
    /// <param name="AEndedDate">The active period ended date.</param>
    /// <param name="ANotFilter">The negates the result from applying the row filter.</param>
    /// <param name="ARowFilter">The filter condtions to apply to the parameter rows.</param>
    public virtual void InitValue(cCheckCategory AOwner, string ADataSort,
                                  DateTime ABeganDate, DateTime AEndedDate,
                                  bool ANotFilter, params cFilterCondition[] ARowFilter)
    {
      InitValue(ARowFilter, ANotFilter, ABeganDate, AEndedDate, ADataSort, AOwner);
    }

    /// <summary>
    /// Scopes the source data to the active and filter specifications.
    /// </summary>
    /// <param name="ADataSort">The sort to apply to the resulting data's default view.</param>
    /// <param name="ABeganDate">The active period began date.</param>
    /// <param name="AEndedDate">The active period ended date.</param>
    /// <param name="ANotFilter">The negates the result from applying the row filter.</param>
    /// <param name="ARowFilter">The filter condtions to apply to the parameter rows.</param>
    public virtual void InitValue(string ADataSort,
                                  DateTime ABeganDate, DateTime AEndedDate,
                                  bool ANotFilter, params cFilterCondition[] ARowFilter)
    {
      InitValue(ARowFilter, ANotFilter, ABeganDate, AEndedDate, ADataSort, (cCheckCategory)null);
    }

    /// <summary>
    /// Scopes the source data to the active and filter specifications.
    /// </summary>
    /// <param name="ARowFilter">The filter condtions to apply to the parameter rows.</param>
    /// <param name="ANotFilter">The negates the result from applying the row filter.</param>
    /// <param name="AOpDate">The operating date to test rows against</param>
    /// <param name="ADataSort">The sort to apply to the resulting data's default view.</param>
    /// <param name="AOwner">The check category making the request.</param>
    public virtual void InitValue(cFilterCondition[] ARowFilter, bool ANotFilter,
                                  DateTime AOpDate,
                                  string ADataSort, cCheckCategory AOwner)
    {
      if (SetValueCheckAndPrep(AOwner))
      {
        Scope(ARowFilter, ANotFilter, AOpDate);
        ApplySort(ADataSort, AOwner);
      }
    }

    /// <summary>
    /// Scopes the source data to the active and filter specifications.
    /// </summary>
    /// <param name="ARowFilter">The filter condtions to apply to the parameter rows.</param>
    /// <param name="ANotFilter">The negates the result from applying the row filter.</param>
    /// <param name="AOpDate">The operating date to test rows against</param>
    /// <param name="ADataSort">The sort to apply to the resulting data's default view.</param>
    public virtual void InitValue(cFilterCondition[] ARowFilter, bool ANotFilter,
                                  DateTime AOpDate,
                                  string ADataSort)
    {
      InitValue(ARowFilter, ANotFilter, AOpDate, ADataSort, (cCheckCategory)null);
    }

    /// <summary>
    /// Scopes the source data to the active and filter specifications.
    /// </summary>
    /// <param name="ARowFilter">The filter condtions to apply to the parameter rows.</param>
    /// <param name="AOpDate">The operating date to test rows against</param>
    /// <param name="ADataSort">The sort to apply to the resulting data's default view.</param>
    /// <param name="AOwner">The check category making the request.</param>
    public virtual void InitValue(cFilterCondition[] ARowFilter,
                                  DateTime AOpDate,
                                  string ADataSort, cCheckCategory AOwner)
    {
      if (SetValueCheckAndPrep(AOwner))
      {
        Scope(ARowFilter, false, AOpDate);
        ApplySort(ADataSort, AOwner);
      }
    }

    /// <summary>
    /// Scopes the source data to the active and filter specifications.
    /// </summary>
    /// <param name="ARowFilter">The filter condtions to apply to the parameter rows.</param>
    /// <param name="AOpDate">The operating date to test rows against</param>
    /// <param name="ADataSort">The sort to apply to the resulting data's default view.</param>
    public virtual void InitValue(cFilterCondition[] ARowFilter,
                                  DateTime AOpDate,
                                  string ADataSort)
    {
      InitValue(ARowFilter, AOpDate, ADataSort, (cCheckCategory)null);
    }

    /// <summary>
    /// Scopes the source data to the active and filter specifications.
    /// </summary>
    /// <param name="AOpDate">The operating date to test rows against</param>
    /// <param name="ADataSort">The sort to apply to the resulting data's default view.</param>
    /// <param name="AOwner">The check category making the request.</param>
    public virtual void InitValue(DateTime AOpDate,
                                  string ADataSort, cCheckCategory AOwner)
    {
      if (SetValueCheckAndPrep(AOwner))
      {
        Scope(AOpDate);
        ApplySort(ADataSort, AOwner);
      }
    }

    /// <summary>
    /// Scopes the source data to the active and filter specifications.
    /// </summary>
    /// <param name="AOpDate">The operating date to test rows against</param>
    /// <param name="ADataSort">The sort to apply to the resulting data's default view.</param>
    public virtual void InitValue(DateTime AOpDate,
                                  string ADataSort)
    {
      InitValue(AOpDate, ADataSort, (cCheckCategory)null);
    }

    /// <summary>
    /// Scopes the source data to the active and filter specifications.
    /// </summary>
    /// <param name="AOwner">The check category making the request.</param>
    /// <param name="ADataSort">The sort to apply to the resulting data's default view.</param>
    /// <param name="AOpDate">The operating date to test rows against</param>
    /// <param name="ANotFilter">The negates the result from applying the row filter.</param>
    /// <param name="ARowFilter">The filter condtions to apply to the parameter rows.</param>
    public virtual void InitValue(cCheckCategory AOwner, string ADataSort,
                                  DateTime AOpDate,
                                  bool ANotFilter, params cFilterCondition[] ARowFilter)
    {
      InitValue(ARowFilter, ANotFilter, AOpDate, ADataSort, AOwner);
    }

    /// <summary>
    /// Scopes the source data to the active and filter specifications.
    /// </summary>
    /// <param name="ADataSort">The sort to apply to the resulting data's default view.</param>
    /// <param name="AOpDate">The operating date to test rows against</param>
    /// <param name="ANotFilter">The negates the result from applying the row filter.</param>
    /// <param name="ARowFilter">The filter condtions to apply to the parameter rows.</param>
    public virtual void InitValue(string ADataSort,
                                  DateTime AOpDate,
                                  bool ANotFilter, params cFilterCondition[] ARowFilter)
    {
      InitValue(ARowFilter, ANotFilter, AOpDate, ADataSort, (cCheckCategory)null);
    }

    #endregion


    #region Protected Methods

    /// <summary>
    /// Sets the Scoped Data Table from Source Data Table and based on the passed active and 
    /// filter specification.
    /// </summary>
    /// <param name="ARowFilter">The filter condtions to apply to the parameter rows.</param>
    /// <param name="ANotFilter">The negates the result from applying the row filter.</param>
    /// <param name="ABeganDate">The beginning of the active date range.</param>
    /// <param name="AEndedDate">The end of the active date range.</param>
    protected void Scope(cFilterCondition[] ARowFilter, bool ANotFilter,
                         DateTime ABeganDate, DateTime AEndedDate)
    {
      if (FSourceDataTable == null)
      {
        FLastErrorAction = "Scope";
        FLastErrorMessage = "Scope failed because source table has not been populated.";
        FScopedDataTable = null;
      }
      else
      {
        DataView ScopedDataView = cRowFilter.FindActiveRows(FSourceDataTable.DefaultView, 
                                                            ABeganDate, AEndedDate, 
                                                            FBeganDateFieldName, FEndedDateFieldName,
                                                            ANotFilter, ARowFilter);

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
    /// <param name="ABeganDate">The active period began date.</param>
    /// <param name="AEndedDate">The active period ended date.</param>
    protected void Scope(DateTime ABeganDate, DateTime AEndedDate)
    {
      if (FSourceDataTable == null)
      {
        FLastErrorAction = "Scope";
        FLastErrorMessage = "Scope failed because source table has not been populated.";
        FScopedDataTable = null;
      }
      else
      {
        DataView ScopedDataView = cRowFilter.FindActiveRows(FSourceDataTable.DefaultView,
                                                            ABeganDate, AEndedDate,
                                                            FBeganDateFieldName, FEndedDateFieldName);

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
    /// Sets the Scoped Data Table from Source Data Table and based on the passed active and 
    /// filter specification.
    /// </summary>
    /// <param name="ARowFilter">The filter condtions to apply to the parameter rows.</param>
    /// <param name="ANotFilter">The negates the result from applying the row filter.</param>
    /// <param name="AOpDate">The operating date to test rows against</param>
    protected void Scope(cFilterCondition[] ARowFilter, bool ANotFilter,
                         DateTime AOpDate)
    {
      if (FSourceDataTable == null)
      {
        FLastErrorAction = "Scope";
        FLastErrorMessage = "Scope failed because source table has not been populated.";
        FScopedDataTable = null;
      }
      else
      {
        
        DataView ScopedDataView = cRowFilter.FindActiveRows(FSourceDataTable.DefaultView,
                                                            AOpDate,
                                                            FBeganDateFieldName, FEndedDateFieldName,
                                                            ANotFilter, ARowFilter);

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
    /// <param name="AOpDate">The operating date to test rows against</param>
    protected void Scope(DateTime AOpDate)
    {
      if (FSourceDataTable == null)
      {
        FLastErrorAction = "Scope";
        FLastErrorMessage = "Scope failed because source table has not been populated.";
        FScopedDataTable = null;
      }
      else
      {
        DataView ScopedDataView = cRowFilter.FindActiveRows(FSourceDataTable.DefaultView,
                                                            AOpDate,
                                                            FBeganDateFieldName, FEndedDateFieldName);

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
