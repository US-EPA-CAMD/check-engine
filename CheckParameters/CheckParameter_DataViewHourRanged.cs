using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using ECMPS.Checks.TypeUtilities;

namespace ECMPS.Checks.Parameters
{

  /// <summary>
  /// Implements the data view parameter for hour ranged data.
  /// </summary>
  public class cCheckParameterDataViewHourRanged : cCheckParameterDataViewDateRanged
  {

    #region Public Constructors

    /// <summary>
    /// Creates and instance of a cCheckParameterDataViewHourRanged object, setting the array property 
    /// to false, and the parameter name and types, and the source data table properties.  Also sets
    /// the began and ended date and hour field name properties.
    /// </summary>
    /// <param name="AParameterKey">The key of the parameter.</param>
    /// <param name="AParameterName">The name of the parameter.</param>
    /// <param name="ASourceDataTable">The source data table for the parameter.</param>
    /// <param name="ABeganDateFieldName">The name of the began date field.</param>
    /// <param name="ABeganHourFieldName">The name of the began hour field.</param>
    /// <param name="AEndedDateFieldName">The name of the ended date field.</param>
    /// <param name="AEndedHourFieldName">The name of the ended hour field.</param>
    /// <param name="ACheckParameters">The parent check parameters object.</param>
    public cCheckParameterDataViewHourRanged(int? AParameterKey, string AParameterName,
                                             DataTable ASourceDataTable,
                                             string ABeganDateFieldName, string ABeganHourFieldName,
                                             string AEndedDateFieldName, string AEndedHourFieldName,
                                             cCheckParameters ACheckParameters)
      : base(AParameterKey, AParameterName, ASourceDataTable,
             ABeganDateFieldName, AEndedDateFieldName, ACheckParameters)
    {
      FBeganHourFieldName = ABeganHourFieldName;
      FEndedHourFieldName = AEndedHourFieldName;
    }

    /// <summary>
    /// Creates and instance of a cCheckParameterDataViewHourRanged object, setting the array property 
    /// to false, and the parameter name and types, and the source data table properties.  Also sets
    /// the began and ended date and hour field name properties.
    /// </summary>
    /// <param name="AParameterKey">The key of the parameter.</param>
    /// <param name="AParameterName">The name of the parameter.</param>
    /// <param name="ASourceDataTable">The source data table for the parameter.</param>
    /// <param name="ACheckParameters">The parent check parameters object.</param>
    public cCheckParameterDataViewHourRanged(int? AParameterKey, string AParameterName,
                                             DataTable ASourceDataTable,
                                             cCheckParameters ACheckParameters)
      : base(AParameterKey, AParameterName, ASourceDataTable, ACheckParameters)
    {
      FBeganHourFieldName = "BEGIN_HOUR";
      FEndedHourFieldName = "END_HOUR";
    }

    #endregion


    #region Public Property with Fields

    #region Property Fields

    /// <summary>
    /// The name of the began hour field.
    /// </summary>
    protected string FBeganHourFieldName = null;

    /// <summary>
    /// The name of the ended hour field.
    /// </summary>
    protected string FEndedHourFieldName = null;

    #endregion


    /// <summary>
    /// The name of the began hour field.
    /// </summary>
    public string BeganHourFieldName { get { return FBeganHourFieldName; } }

    /// <summary>
    /// The name of the ended hour field.
    /// </summary>
    public string EndedHourFieldName { get { return FEndedHourFieldName; } }

    #endregion


    #region Public Virtual Methods: Filter

    /// <summary>
    /// Filters the scoped data using the active and filter specifications.
    /// </summary>
    /// <param name="ARowFilter">The filter condtions to apply to the parameter rows.</param>
    /// <param name="ANotFilter">The negates the result from applying the row filter.</param>
    /// <param name="ABeganDate">The active period began date.</param>
    /// <param name="ABeganHour">The active period began hour.</param>
    /// <param name="AEndedDate">The active period ended date.</param>
    /// <param name="AEndedHour">The active period ended hour.</param>
    /// <returns>A data view of the filter results.</returns>
    public virtual DataView Filter(cFilterCondition[] ARowFilter, bool ANotFilter,
                                   DateTime ABeganDate, int ABeganHour,
                                   DateTime AEndedDate, int AEndedHour)
    {
      if (FScopedDataTable != null)
        return cRowFilter.FindActiveRows(FScopedDataTable.DefaultView,
                                         ABeganDate, ABeganHour,
                                         AEndedDate, AEndedHour,
                                         FBeganDateFieldName, FBeganHourFieldName,
                                         FEndedDateFieldName, FEndedHourFieldName,
                                         ANotFilter, ARowFilter);
      else
        return null;
    }

    /// <summary>
    /// Filters the scoped data using the active and filter specifications.
    /// </summary>
    /// <param name="ARowFilter">The filter condtions to apply to the parameter rows.</param>
    /// <param name="ABeganDate">The active period began date.</param>
    /// <param name="ABeganHour">The active period began hour.</param>
    /// <param name="AEndedDate">The active period ended date.</param>
    /// <param name="AEndedHour">The active period ended hour.</param>
    /// <returns>A data view of the filter results.</returns>
    public virtual DataView Filter(cFilterCondition[] ARowFilter,
                                   DateTime ABeganDate, int ABeganHour,
                                   DateTime AEndedDate, int AEndedHour)
    {
      if (FScopedDataTable != null)
        return cRowFilter.FindActiveRows(FScopedDataTable.DefaultView,
                                         ABeganDate, ABeganHour,
                                         AEndedDate, AEndedHour,
                                         FBeganDateFieldName, FBeganHourFieldName,
                                         FEndedDateFieldName, FEndedHourFieldName,
                                         ARowFilter);
      else
        return null;
    }

    /// <summary>
    /// Filters the scoped data using the active and filter specifications.
    /// </summary>
    /// <param name="ABeganDate">The active period began date.</param>
    /// <param name="ABeganHour">The active period began hour.</param>
    /// <param name="AEndedDate">The active period ended date.</param>
    /// <param name="AEndedHour">The active period ended hour.</param>
    /// <returns>A data view of the filter results.</returns>
    public virtual DataView Filter(DateTime ABeganDate, int ABeganHour,
                                   DateTime AEndedDate, int AEndedHour)
    {
      if (FScopedDataTable != null)
        return cRowFilter.FindActiveRows(FScopedDataTable.DefaultView,
                                         ABeganDate, ABeganHour,
                                         AEndedDate, AEndedHour,
                                         FBeganDateFieldName, FBeganHourFieldName,
                                         FEndedDateFieldName, FEndedHourFieldName);
      else
        return null;
    }

    /// <summary>
    /// Filters the scoped data using the active and filter specifications.
    /// </summary>
    /// <param name="ABeganDate">The active period began date.</param>
    /// <param name="ABeganHour">The active period began hour.</param>
    /// <param name="AEndedDate">The active period ended date.</param>
    /// <param name="AEndedHour">The active period ended hour.</param>
    /// <param name="ANotFilter">The negates the result from applying the row filter.</param>
    /// <param name="ARowFilter">The filter condtions to apply to the parameter rows.</param>
    /// <returns>A data view of the filter results.</returns>
    public virtual DataView Filter(DateTime ABeganDate, int ABeganHour,
                                   DateTime AEndedDate, int AEndedHour, 
                                   bool ANotFilter, params cFilterCondition[] ARowFilter)
    {
      if (FScopedDataTable != null)
        return cRowFilter.FindActiveRows(FScopedDataTable.DefaultView,
                                         ABeganDate, ABeganHour,
                                         AEndedDate, AEndedHour,
                                         FBeganDateFieldName, FBeganHourFieldName,
                                         FEndedDateFieldName, FEndedHourFieldName,
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
    /// <param name="AOpHour">The operating hour to test rows against</param>
    /// <returns>A data view of the filter results.</returns>
    public virtual DataView Filter(cFilterCondition[] ARowFilter, bool ANotFilter,
                                   DateTime AOpDate, int AOpHour)
    {
      if (FScopedDataTable != null)
        return cRowFilter.FindActiveRows(FScopedDataTable.DefaultView,
                                         AOpDate, AOpHour,
                                         FBeganDateFieldName, FBeganHourFieldName,
                                         FEndedDateFieldName, FEndedHourFieldName,
                                         ANotFilter, ARowFilter);
      else
        return null;
    }

    /// <summary>
    /// Filters the scoped data using the active and filter specifications.
    /// </summary>
    /// <param name="ARowFilter">The filter condtions to apply to the parameter rows.</param>
    /// <param name="AOpDate">The operating date to test rows against</param>
    /// <param name="AOpHour">The operating hour to test rows against</param>
    /// <returns>A data view of the filter results.</returns>
    public virtual DataView Filter(cFilterCondition[] ARowFilter,
                                   DateTime AOpDate, int AOpHour)
    {
      if (FScopedDataTable != null)
        return cRowFilter.FindActiveRows(FScopedDataTable.DefaultView,
                                         AOpDate, AOpHour,
                                         FBeganDateFieldName, FBeganHourFieldName,
                                         FEndedDateFieldName, FEndedHourFieldName,
                                         ARowFilter);
      else
        return null;
    }

    /// <summary>
    /// Filters the scoped data using the active and filter specifications.
    /// </summary>
    /// <param name="AOpDate">The operating date to test rows against</param>
    /// <param name="AOpHour">The operating hour to test rows against</param>
    /// <returns>A data view of the filter results.</returns>
    public virtual DataView Filter(DateTime AOpDate, int AOpHour)
    {
      if (FScopedDataTable != null)
        return cRowFilter.FindActiveRows(FScopedDataTable.DefaultView,
                                         AOpDate, AOpHour,
                                         FBeganDateFieldName, FBeganHourFieldName,
                                         FEndedDateFieldName, FEndedHourFieldName);
      else
        return null;
    }

    /// <summary>
    /// Filters the scoped data using the active and filter specifications.
    /// </summary>
    /// <param name="AOpDate">The operating date to test rows against</param>
    /// <param name="AOpHour">The operating hour to test rows against</param>
    /// <param name="ANotFilter">The negates the result from applying the row filter.</param>
    /// <param name="ARowFilter">The filter condtions to apply to the parameter rows.</param>
    /// <returns>A data view of the filter results.</returns>
    public virtual DataView Filter(DateTime AOpDate, int AOpHour,
                                   bool ANotFilter, params cFilterCondition[] ARowFilter)
    {
      if (FScopedDataTable != null)
        return cRowFilter.FindActiveRows(FScopedDataTable.DefaultView,
                                         AOpDate, AOpHour,
                                         FBeganDateFieldName, FBeganHourFieldName,
                                         FEndedDateFieldName, FEndedHourFieldName,
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
    /// <param name="ABeganHour">The active period began hour.</param>
    /// <param name="AEndedDate">The active period ended date.</param>
    /// <param name="AEndedHour">The active period ended hour.</param>
    /// <param name="ADataSort">The sort to apply to the resulting data's default view.</param>
    /// <param name="AOwner">The check category making the request.</param>
    public virtual void InitValue(cFilterCondition[] ARowFilter, bool ANotFilter,
                                  DateTime ABeganDate, int ABeganHour,
                                  DateTime AEndedDate, int AEndedHour, 
                                  string ADataSort, cCheckCategory AOwner)
    {
      if (SetValueCheckAndPrep(AOwner))
      {
        Scope(ARowFilter, ANotFilter, ABeganDate, ABeganHour, AEndedDate, AEndedHour);
        ApplySort(ADataSort, AOwner);
      }
    }

    /// <summary>
    /// Scopes the source data to the active and filter specifications.
    /// </summary>
    /// <param name="ARowFilter">The filter condtions to apply to the parameter rows.</param>
    /// <param name="ANotFilter">The negates the result from applying the row filter.</param>
    /// <param name="ABeganDate">The active period began date.</param>
    /// <param name="ABeganHour">The active period began hour.</param>
    /// <param name="AEndedDate">The active period ended date.</param>
    /// <param name="AEndedHour">The active period ended hour.</param>
    /// <param name="ADataSort">The sort to apply to the resulting data's default view.</param>
    public virtual void InitValue(cFilterCondition[] ARowFilter, bool ANotFilter,
                                  DateTime ABeganDate, int ABeganHour,
                                  DateTime AEndedDate, int AEndedHour,
                                  string ADataSort)
    {
      InitValue(ARowFilter, ANotFilter, ABeganDate, ABeganHour, AEndedDate, AEndedHour, ADataSort, (cCheckCategory)null);
    }

    /// <summary>
    /// Scopes the source data to the active and filter specifications.
    /// </summary>
    /// <param name="ARowFilter">The filter condtions to apply to the parameter rows.</param>
    /// <param name="ABeganDate">The active period began date.</param>
    /// <param name="ABeganHour">The active period began hour.</param>
    /// <param name="AEndedDate">The active period ended date.</param>
    /// <param name="AEndedHour">The active period ended hour.</param>
    /// <param name="ADataSort">The sort to apply to the resulting data's default view.</param>
    /// <param name="AOwner">The check category making the request.</param>
    public virtual void InitValue(cFilterCondition[] ARowFilter,
                                  DateTime ABeganDate, int ABeganHour,
                                  DateTime AEndedDate, int AEndedHour,
                                  string ADataSort, cCheckCategory AOwner)
    {
      if (SetValueCheckAndPrep(AOwner))
      {
        Scope(ARowFilter, false, ABeganDate, ABeganHour, AEndedDate, AEndedHour);
        ApplySort(ADataSort, AOwner);
      }
    }

    /// <summary>
    /// Scopes the source data to the active and filter specifications.
    /// </summary>
    /// <param name="ARowFilter">The filter condtions to apply to the parameter rows.</param>
    /// <param name="ABeganDate">The active period began date.</param>
    /// <param name="ABeganHour">The active period began hour.</param>
    /// <param name="AEndedDate">The active period ended date.</param>
    /// <param name="AEndedHour">The active period ended hour.</param>
    /// <param name="ADataSort">The sort to apply to the resulting data's default view.</param>
    public virtual void InitValue(cFilterCondition[] ARowFilter,
                                  DateTime ABeganDate, int ABeganHour,
                                  DateTime AEndedDate, int AEndedHour,
                                  string ADataSort)
    {
      InitValue(ARowFilter, ABeganDate, ABeganHour, AEndedDate, AEndedHour, ADataSort, (cCheckCategory)null);
    }

    /// <summary>
    /// Scopes the source data to the active and filter specifications.
    /// </summary>
    /// <param name="ABeganDate">The active period began date.</param>
    /// <param name="ABeganHour">The active period began hour.</param>
    /// <param name="AEndedDate">The active period ended date.</param>
    /// <param name="AEndedHour">The active period ended hour.</param>
    /// <param name="ADataSort">The sort to apply to the resulting data's default view.</param>
    /// <param name="AOwner">The check category making the request.</param>
    public virtual void InitValue(DateTime ABeganDate, int ABeganHour,
                                  DateTime AEndedDate, int AEndedHour,
                                  string ADataSort, cCheckCategory AOwner)
    {
      if (SetValueCheckAndPrep(AOwner))
      {
        Scope(ABeganDate, ABeganHour, AEndedDate, AEndedHour);
        ApplySort(ADataSort, AOwner);
      }
    }

    /// <summary>
    /// Scopes the source data to the active and filter specifications.
    /// </summary>
    /// <param name="ABeganDate">The active period began date.</param>
    /// <param name="ABeganHour">The active period began hour.</param>
    /// <param name="AEndedDate">The active period ended date.</param>
    /// <param name="AEndedHour">The active period ended hour.</param>
    /// <param name="ADataSort">The sort to apply to the resulting data's default view.</param>
    public virtual void InitValue(DateTime ABeganDate, int ABeganHour,
                                  DateTime AEndedDate, int AEndedHour,
                                  string ADataSort)
    {
      InitValue(ABeganDate, ABeganHour, AEndedDate, AEndedHour, ADataSort, (cCheckCategory)null);
    }

    /// <summary>
    /// Scopes the source data to the active and filter specifications.
    /// </summary>
    /// <param name="AOwner">The check category making the request.</param>
    /// <param name="ADataSort">The sort to apply to the resulting data's default view.</param>
    /// <param name="ABeganDate">The active period began date.</param>
    /// <param name="ABeganHour">The active period began hour.</param>
    /// <param name="AEndedDate">The active period ended date.</param>
    /// <param name="AEndedHour">The active period ended hour.</param>
    /// <param name="ANotFilter">The negates the result from applying the row filter.</param>
    /// <param name="ARowFilter">The filter condtions to apply to the parameter rows.</param>
    public virtual void InitValue(cCheckCategory AOwner, string ADataSort,
                                  DateTime ABeganDate, int ABeganHour,
                                  DateTime AEndedDate, int AEndedHour,
                                  bool ANotFilter, params cFilterCondition[] ARowFilter)
    {
      InitValue(ARowFilter, ANotFilter, ABeganDate, ABeganHour, AEndedDate, AEndedHour, ADataSort, AOwner);
    }

    /// <summary>
    /// Scopes the source data to the active and filter specifications.
    /// </summary>
    /// <param name="ADataSort">The sort to apply to the resulting data's default view.</param>
    /// <param name="ABeganDate">The active period began date.</param>
    /// <param name="ABeganHour">The active period began hour.</param>
    /// <param name="AEndedDate">The active period ended date.</param>
    /// <param name="AEndedHour">The active period ended hour.</param>
    /// <param name="ANotFilter">The negates the result from applying the row filter.</param>
    /// <param name="ARowFilter">The filter condtions to apply to the parameter rows.</param>
    public virtual void InitValue(string ADataSort,
                                  DateTime ABeganDate, int ABeganHour,
                                  DateTime AEndedDate, int AEndedHour,
                                  bool ANotFilter, params cFilterCondition[] ARowFilter)
    {
      InitValue(ARowFilter, ANotFilter, ABeganDate, ABeganHour, AEndedDate, AEndedHour, ADataSort, (cCheckCategory)null);
    }



    /// <summary>
    /// Scopes the source data to the active and filter specifications.
    /// </summary>
    /// <param name="ARowFilter">The filter condtions to apply to the parameter rows.</param>
    /// <param name="ANotFilter">The negates the result from applying the row filter.</param>
    /// <param name="AOpDate">The operating date to test rows against</param>
    /// <param name="AOpHour">The operating hour to test rows against</param>
    /// <param name="ADataSort">The sort to apply to the resulting data's default view.</param>
    /// <param name="AOwner">The check category making the request.</param>
    public virtual void InitValue(cFilterCondition[] ARowFilter, bool ANotFilter,
                                  DateTime AOpDate, int AOpHour,
                                  string ADataSort, cCheckCategory AOwner)
    {
      if (SetValueCheckAndPrep(AOwner))
      {
        Scope(ARowFilter, ANotFilter, AOpDate, AOpHour);
        ApplySort(ADataSort, AOwner);
      }
    }

    /// <summary>
    /// Scopes the source data to the active and filter specifications.
    /// </summary>
    /// <param name="ARowFilter">The filter condtions to apply to the parameter rows.</param>
    /// <param name="ANotFilter">The negates the result from applying the row filter.</param>
    /// <param name="AOpDate">The operating date to test rows against</param>
    /// <param name="AOpHour">The operating hour to test rows against</param>
    /// <param name="ADataSort">The sort to apply to the resulting data's default view.</param>
    public virtual void InitValue(cFilterCondition[] ARowFilter, bool ANotFilter,
                                  DateTime AOpDate, int AOpHour,
                                  string ADataSort)
    {
      InitValue(ARowFilter, ANotFilter, AOpDate, AOpHour, ADataSort, (cCheckCategory)null);
    }

    /// <summary>
    /// Scopes the source data to the active and filter specifications.
    /// </summary>
    /// <param name="ARowFilter">The filter condtions to apply to the parameter rows.</param>
    /// <param name="AOpDate">The operating date to test rows against</param>
    /// <param name="AOpHour">The operating hour to test rows against</param>
    /// <param name="ADataSort">The sort to apply to the resulting data's default view.</param>
    /// <param name="AOwner">The check category making the request.</param>
    public virtual void InitValue(cFilterCondition[] ARowFilter,
                                  DateTime AOpDate, int AOpHour,
                                  string ADataSort, cCheckCategory AOwner)
    {
      if (SetValueCheckAndPrep(AOwner))
      {
        Scope(ARowFilter, false, AOpDate, AOpHour);
        ApplySort(ADataSort, AOwner);
      }
    }

    /// <summary>
    /// Scopes the source data to the active and filter specifications.
    /// </summary>
    /// <param name="ARowFilter">The filter condtions to apply to the parameter rows.</param>
    /// <param name="AOpDate">The operating date to test rows against</param>
    /// <param name="AOpHour">The operating hour to test rows against</param>
    /// <param name="ADataSort">The sort to apply to the resulting data's default view.</param>
    public virtual void InitValue(cFilterCondition[] ARowFilter,
                                  DateTime AOpDate, int AOpHour,
                                  string ADataSort)
    {
      InitValue(ARowFilter, AOpDate, AOpHour, ADataSort, (cCheckCategory)null);
    }

    /// <summary>
    /// Scopes the source data to the active and filter specifications.
    /// </summary>
    /// <param name="AOpDate">The operating date to test rows against</param>
    /// <param name="AOpHour">The operating hour to test rows against</param>
    /// <param name="ADataSort">The sort to apply to the resulting data's default view.</param>
    /// <param name="AOwner">The check category making the request.</param>
    public virtual void InitValue(DateTime AOpDate, int AOpHour,
                                  string ADataSort, cCheckCategory AOwner)
    {
      if (SetValueCheckAndPrep(AOwner))
      {
        Scope(AOpDate, AOpHour);
        ApplySort(ADataSort, AOwner);
      }
    }

    /// <summary>
    /// Scopes the source data to the active and filter specifications.
    /// </summary>
    /// <param name="AOpDate">The operating date to test rows against</param>
    /// <param name="AOpHour">The operating hour to test rows against</param>
    /// <param name="ADataSort">The sort to apply to the resulting data's default view.</param>
    public virtual void InitValue(DateTime AOpDate, int AOpHour,
                                  string ADataSort)
    {
      InitValue(AOpDate, AOpHour, ADataSort, (cCheckCategory)null);
    }

    /// <summary>
    /// Scopes the source data to the active and filter specifications.
    /// </summary>
    /// <param name="AOwner">The check category making the request.</param>
    /// <param name="ADataSort">The sort to apply to the resulting data's default view.</param>
    /// <param name="AOpDate">The operating date to test rows against</param>
    /// <param name="AOpHour">The operating hour to test rows against</param>
    /// <param name="ANotFilter">The negates the result from applying the row filter.</param>
    /// <param name="ARowFilter">The filter condtions to apply to the parameter rows.</param>
    public virtual void InitValue(cCheckCategory AOwner, string ADataSort,
                                  DateTime AOpDate, int AOpHour,
                                  bool ANotFilter, params cFilterCondition[] ARowFilter)
    {
      InitValue(ARowFilter, ANotFilter, AOpDate, AOpHour, ADataSort, AOwner);
    }

    /// <summary>
    /// Scopes the source data to the active and filter specifications.
    /// </summary>
    /// <param name="ADataSort">The sort to apply to the resulting data's default view.</param>
    /// <param name="AOpDate">The operating date to test rows against</param>
    /// <param name="AOpHour">The operating hour to test rows against</param>
    /// <param name="ANotFilter">The negates the result from applying the row filter.</param>
    /// <param name="ARowFilter">The filter condtions to apply to the parameter rows.</param>
    public virtual void InitValue(string ADataSort,
                                  DateTime AOpDate, int AOpHour,
                                  bool ANotFilter, params cFilterCondition[] ARowFilter)
    {
      InitValue(ARowFilter, ANotFilter, AOpDate, AOpHour, ADataSort, (cCheckCategory)null);
    }

    #endregion


    #region Protected Methods

    /// <summary>
    /// Sets the Scoped Data Table from Source Data Table and based on the passed active and 
    /// filter specification.
    /// </summary>
    /// <param name="ARowFilter">The filter condtions to apply to the parameter rows.</param>
    /// <param name="ANotFilter">The negates the result from applying the row filter.</param>
    /// <param name="ABeganDate">The active period began date.</param>
    /// <param name="ABeganHour">The active period began hour.</param>
    /// <param name="AEndedDate">The end of the active date range.</param>
    /// <param name="AEndedHour">The end of the active hour range.</param>
    protected void Scope(cFilterCondition[] ARowFilter, bool ANotFilter,
                         DateTime ABeganDate, int ABeganHour, 
                         DateTime AEndedDate, int AEndedHour)
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
                                                            ABeganDate, ABeganHour, 
                                                            AEndedDate, AEndedHour,
                                                            FBeganDateFieldName, FBeganHourFieldName,
                                                            FEndedDateFieldName, FEndedHourFieldName,
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
    /// <param name="ABeganHour">The active period began hour.</param>
    /// <param name="AEndedDate">The end of the active date range.</param>
    /// <param name="AEndedHour">The end of the active hour range.</param>
    protected void Scope(DateTime ABeganDate, int ABeganHour,
                         DateTime AEndedDate, int AEndedHour)
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
                                                            ABeganDate, ABeganHour,
                                                            AEndedDate, AEndedHour,
                                                            FBeganDateFieldName, FBeganHourFieldName,
                                                            FEndedDateFieldName, FEndedHourFieldName);

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
    /// <param name="AOpHour">The operating hour to test rows against</param>
    protected void Scope(cFilterCondition[] ARowFilter, bool ANotFilter,
                         DateTime AOpDate, int AOpHour)
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
                                                            AOpDate, AOpHour,
                                                            FBeganDateFieldName, FBeganHourFieldName,
                                                            FEndedDateFieldName, FEndedHourFieldName,
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
    /// <param name="AOpHour">The operating hour to test rows against</param>
    protected void Scope(DateTime AOpDate, int AOpHour)
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
                                                            AOpDate, AOpHour,
                                                            FBeganDateFieldName, FBeganHourFieldName,
                                                            FEndedDateFieldName, FEndedHourFieldName);

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
