using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using ECMPS.Checks.TypeUtilities;

namespace ECMPS.Checks.Parameters
{

  /// <summary>
  /// Implements the data view parameter for generic data.
  /// </summary>
  public class cCheckParameterDataViewGeneric : cCheckParameterAbstractDataView
  {

    #region Public Constructors

    /// <summary>
    /// Creates and instance of a cCheckParameterDataViewGeneric object, setting the array property 
    /// to false, and the parameter name and types, and the source data table properties.
    /// </summary>
    /// <param name="AParameterKey">The key of the parameter.</param>
    /// <param name="AParameterName">The name of the parameter.</param>
    /// <param name="ASourceDataTable">The source data table for the parameter.</param>
    /// <param name="ACheckParameters">The parent check parameters object.</param>
    public cCheckParameterDataViewGeneric(int? AParameterKey, string AParameterName,
                                          DataTable ASourceDataTable,
                                          cCheckParameters ACheckParameters)
      : base(AParameterKey, AParameterName, ASourceDataTable, ACheckParameters)
    {
    }

    #endregion


    #region Public Virtual Methods

    /// <summary>
    /// Scopes the source data to the filter specifications.
    /// </summary>
    /// <param name="ARowFilter">The filter condtions to apply to the parameter rows.</param>
    /// <param name="ANotFilter">The negates the result from applying the row filter.</param>
    /// <param name="ADataSort">The sort to apply to the resulting data's default view.</param>
    /// <param name="AOwner">The check category making the request.</param>
    public virtual void InitValue(cFilterCondition[] ARowFilter, bool ANotFilter, string ADataSort, cCheckCategory AOwner)
    {
      if (SetValueCheckAndPrep(AOwner))
      {
        Scope(ARowFilter, ANotFilter);
        ApplySort(ADataSort, AOwner);
      }
    }

    /// <summary>
    /// Scopes the source data to the filter specifications.
    /// </summary>
    /// <param name="ARowFilter">The filter condtions to apply to the parameter rows.</param>
    /// <param name="ANotFilter">The negates the result from applying the row filter.</param>
    /// <param name="ADataSort">The sort to apply to the resulting data's default view.</param>
    public virtual void InitValue(cFilterCondition[] ARowFilter, bool ANotFilter, string ADataSort)
    {
      InitValue(ARowFilter, ANotFilter, ADataSort, (cCheckCategory)null);
    }

    /// <summary>
    /// Scopes the source data to the filter specifications.
    /// </summary>
    /// <param name="ARowFilter">The filter condtions to apply to the parameter rows.</param>
    /// <param name="AOwner">The check category making the request.</param>
    /// <param name="ADataSort">The sort to apply to the resulting data's default view.</param>
    public virtual void InitValue(cFilterCondition[] ARowFilter, string ADataSort, cCheckCategory AOwner)
    {
      if (SetValueCheckAndPrep(AOwner))
      {
        Scope(ARowFilter, false);
        ApplySort(ADataSort, AOwner);
      }
    }

    /// <summary>
    /// Scopes the source data to the filter specifications.
    /// </summary>
    /// <param name="ARowFilter">The filter condtions to apply to the parameter rows.</param>
    /// <param name="ADataSort">The sort to apply to the resulting data's default view.</param>
    public virtual void InitValue(cFilterCondition[] ARowFilter, string ADataSort)
    {
      InitValue(ARowFilter, ADataSort, (cCheckCategory)null);
    }

    /// <summary>
    /// Nulls the parameter value.
    /// </summary>
    /// <param name="AOwner">The check category making the request.</param>
    public virtual void InitValue(cCheckCategory AOwner)
    {
      if (SetValueCheckAndPrep(AOwner))
      {
        FScopedDataTable = null;
      }
    }

    /// <summary>
    /// Nulls the parameter value.
    /// </summary>
    public virtual void InitValue()
    {
      InitValue((cCheckCategory)null);
    }

    /// <summary>
    /// Scopes the source data to the filter specifications.
    /// </summary>
    /// <param name="AOwner">The check category making the request.</param>
    /// <param name="ADataSort">The sort to apply to the resulting data's default view.</param>
    /// <param name="ANotFilter">The negates the result from applying the row filter.</param>
    /// <param name="ARowFilter">The filter condtions to apply to the parameter rows.</param>
    public virtual void InitValue(cCheckCategory AOwner, string ADataSort, bool ANotFilter, params cFilterCondition[] ARowFilter)
    {
      InitValue(ARowFilter, ANotFilter, ADataSort, AOwner);
    }

    /// <summary>
    /// Scopes the source data to the filter specifications.
    /// </summary>
    /// <param name="ADataSort">The sort to apply to the resulting data's default view.</param>
    /// <param name="ANotFilter">The negates the result from applying the row filter.</param>
    /// <param name="ARowFilter">The filter condtions to apply to the parameter rows.</param>
    public virtual void InitValue(string ADataSort, bool ANotFilter, params cFilterCondition[] ARowFilter)
    {
      InitValue(ARowFilter, ANotFilter, ADataSort, (cCheckCategory)null);
    }

    #endregion


    #region Protected Methods

    /// <summary>
    /// Apply a sort to the Scoped Data Table's default view, producing a debug line if the
    /// application fails.
    /// </summary>
    /// <param name="ADataSort">The sort to apply</param>
    /// <param name="AOwner">The category requesting the sort.</param>
    protected void ApplySort(string ADataSort, cCheckCategory AOwner)
    {
      if ((FScopedDataTable != null) && !string.IsNullOrEmpty(ADataSort))
      {
        try
        {
          FScopedDataTable.DefaultView.Sort = ADataSort;
        }
        catch (Exception ex)
        {
          if (AOwner != null)
            System.Diagnostics.Debug.WriteLine(string.Format("Problem Sort Specification: Category - {0}, Parameter - {1}: {2}", AOwner.CategoryCd, this.Name, ex.Message));
          else
            System.Diagnostics.Debug.WriteLine(string.Format("Problem Sort Specification: For Process, Parameter - {0}: {1}", this.Name, ex.Message));
        }
      }
    }

    /// <summary>
    /// Apply a sort to the Scoped Data Table's default view, producing a debug line if the
    /// application fails.
    /// </summary>
    /// <param name="ADataSort">The sort to apply</param>
    protected void ApplySort(string ADataSort)
    {
      ApplySort(ADataSort, (cCheckCategory)null);
    }

    /// <summary>
    /// Sets the Scoped Data Table from Source Data Table and based on the passed filter specification.
    /// </summary>
    /// <param name="ARowFilter">The filter condtions to apply to the parameter rows.</param>
    /// <param name="ANotFilter">The negates the result from applying the row filter.</param>
    private void Scope(cFilterCondition[] ARowFilter, bool ANotFilter)
    {
      if (FSourceDataTable == null)
      {
        FLastErrorAction = "Scope";
        FLastErrorMessage = "Scope failed because source table has not been populated.";
        FScopedDataTable = null;
      }
      else
      {
        DataView ScopedDataView = cRowFilter.FindRows(FSourceDataTable.DefaultView, ARowFilter, ANotFilter);

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
