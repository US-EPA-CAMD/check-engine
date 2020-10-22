using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using ECMPS.Checks.TypeUtilities;

namespace ECMPS.Checks.Parameters
{
  /// <summary>
  /// The base class for any DataView property.
  /// </summary>
  public abstract class cCheckParameterAbstractDataView : cCheckParameterCheckEngineTyped<DataView, DataView>
  {

    #region Protected Constructors

    /// <summary>
    /// Instantiates a cCheckParameterAbstractDataView object, setting the array property to false,
    /// and the parameter name and types, and the source data table properties.
    /// </summary>
    /// <param name="AParameterKey">The key of the parameter.</param>
    /// <param name="AParameterName">The name of the parameter.</param>
    /// <param name="ASourceDataTable">The source data table for the parameter.</param>
    /// <param name="ACheckParameters">The parent check parameters object.</param>
    protected cCheckParameterAbstractDataView(int? AParameterKey, string AParameterName,
                                              DataTable ASourceDataTable,
                                              cCheckParameters ACheckParameters)
      : base(AParameterKey, AParameterName, eParameterDataType.DataView, ACheckParameters)
    {
      FIsArray = false;

      FSourceDataTable = ASourceDataTable;
    }

    #endregion


    #region Protected Fields

    /// <summary>
    /// Contains a subset of the data in the source table that is returned as the data 
    /// for the parameter.
    /// </summary>
    protected DataTable FScopedDataTable = null;

    /// <summary>
    /// The source data table.
    /// </summary>
    protected DataTable FSourceDataTable = null;

    #endregion


    #region Public Properties

    /// <summary>
    /// The name of the source data table for the parameter.
    /// </summary>
    public string SourceTableName { get { return (FSourceDataTable != null) ? FSourceDataTable.TableName : null; } }

    #endregion


    #region Public Virtual Get Properties

    /// <summary>
    /// Returns the value since the base value type (DataView) excepts nulls.
    /// </summary>
    public virtual DataView NullDefaultedValue { get { return Value; } }

    /// <summary>
    /// Returns the default view of the Scoped Table 
    /// </summary>
    public virtual DataView Value { get { return ((IsSet && (FScopedDataTable != null)) ? FScopedDataTable.DefaultView : GetNull()); } }

    #endregion


    #region Public Override Get Properties

    /// <summary>
    /// Returns NullDefaultedValue as an object.
    /// </summary>
    public override object NullDefaultedObject { get { return NullDefaultedValue; } }

    /// <summary>
    /// Returns Value as an object.
    /// </summary>
    public override object LegacyValue { get { return Value; } }

    #endregion


    #region Public Methods

    #region Filter

    /// <summary>
    /// Returns a data view containing records from the parameter that match the filter specifications.
    /// </summary>
    /// <param name="ARowFilter">The filter condtions to apply to the parameter rows.</param>
    /// <param name="ANotFilter">The negates the result from applying the row filter.</param>
    /// <returns>Returns the parameter rows that match the filter specifications</returns>
    public DataView Filter(cFilterCondition[] ARowFilter, bool ANotFilter)
    {
      if (FScopedDataTable != null)
        return cRowFilter.FindRows(FScopedDataTable.DefaultView, ARowFilter, ANotFilter);
      else
        return null;
    }

    /// <summary>
    /// Returns a data view containing records from the parameter that match the filter specifications.
    /// </summary>
    /// <param name="ARowFilter">The filter condtions to apply to the parameter rows.</param>
    /// <returns>Returns the parameter rows that match the filter specifications</returns>
    public DataView Filter(cFilterCondition[] ARowFilter)
    {
      return Filter(ARowFilter, false);
    }

    /// <summary>
    /// Returns a data view containing records from the parameter that match the filter specifications.
    /// </summary>
    /// <param name="ANotFilter">The negates the result from applying the row filter.</param>
    /// <param name="ARowFilter">The filter condtions to apply to the parameter rows.</param>
    /// <returns>Returns the parameter rows that match the filter specifications</returns>
    public DataView Filter(bool ANotFilter, params cFilterCondition[] ARowFilter)
    {
      return Filter(ARowFilter, ANotFilter);
    }

    #endregion

    #region Other

    /// <summary>
    /// Returns true if the passed column name is a column in the DataView of the parameter.
    /// </summary>
    /// <param name="AColumnName">The column name to test for existence in the DataView.</param>
    /// <returns>Returns true if the column name is in the DataView.</returns>
    public bool IsColumn(string AColumnName)
    {
      if (FSourceDataTable != null)
        return FSourceDataTable.Columns.Contains(AColumnName);
      else
        return false;
    }

    #endregion

    #endregion


    #region Public Override Methods

    /// <summary>
    /// Returns this parameter's default DataView value for nulls.
    /// </summary>
    /// <returns>Returns the default DataView value for nulls.</returns>
    public override DataView GetDefaultForNull()
    {
      return null;
    }

    /// <summary>
    /// Retursn this parameter's default DataView value for nulls as an object.
    /// </summary>
    /// <returns>Returns the default DataView value for nulls as an object.</returns>
    public override object GetDefaultObjectForNull()
    {
      return GetDefaultForNull();
    }

    /// <summary>
    /// Returns null typed as a DataView.
    /// </summary>
    /// <returns>Returns null as a DataView.</returns>
    public override DataView GetNull()
    {
      return null;
    }

    /// <summary>
    /// Returns the passed value if it is not null, otherwise it returns the default DataView for nulls.
    /// </summary>
    /// <param name="ANullableValue">The value to convert.</param>
    /// <returns>The converted value.</returns>
    public override DataView GetTypeValue(DataView ANullableValue)
    {
      return ANullableValue;
    }

    /// <summary>
    /// This method sets the value of the parameter using an object value.
    /// If the object value is not of a correct type the method returns a false.
    /// </summary>
    /// <param name="ALegacyValue">The object value by which to set the value.</param>
    /// <param name="AOwner">The category setting the parameter</param>
    /// <returns>True if the setting was successful.</returns>
    public override bool LegacySetValue(object ALegacyValue, cCheckCategory AOwner)
    {
      if (AOwner != null)
        System.Diagnostics.Debug.WriteLine(string.Format("Error legacy setting of value: Category - {0}, Parameter - {1} : {2}", AOwner.CategoryCd, this.Name, "Legacy setting of this parameter type is not allowed."));
      else
        System.Diagnostics.Debug.WriteLine(string.Format("Error legacy setting of value: For Process, Parameter - {0} : {1}", this.Name, "Legacy setting of this parameter type is not allowed."));

      return false;
    }

    /// <summary>
    /// This method updates the value of the parameter using an object value.
    /// If the object value is not of a correct type the method returns a false.
    /// </summary>
    /// <param name="ALegacyValue">The object value by which to set the value.</param>
    /// <param name="ACategory">The category setting the parameter</param>
    /// <returns>True if the setting was successful.</returns>
    public override bool LegacyUpdateValue(object ALegacyValue, cCheckCategory ACategory)
    {
      if (ACategory != null)
        System.Diagnostics.Debug.WriteLine(string.Format("Error legacy setting of value: Category - {0}, Parameter - {1} : {2}", ACategory.CategoryCd, this.Name, "Legacy setting of this parameter type is not allowed."));
      else
        System.Diagnostics.Debug.WriteLine(string.Format("Error legacy setting of value: For Process, Parameter - {0} : {1}", this.Name, "Legacy setting of this parameter type is not allowed."));

      return false;
    }

    /// <summary>
    /// Returns the current value of the parameter as an object.
    /// </summary>
    /// <returns>The current value of the parameter as an object.</returns>
    public override object ValueAsObject()
    {
      return Value;
    }

    #endregion


    #region Protected Override Methods

    /// <summary>
    /// Sets the value property field to null, typed as a DataView.
    /// </summary>
    protected override void ResetValue()
    {
      FScopedDataTable = null;
    }

    #endregion

  }
}
