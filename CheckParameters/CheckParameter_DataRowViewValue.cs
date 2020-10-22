using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ECMPS.Checks.Parameters
{

  /// <summary>
  /// The DataRowView value parameter object used by the check engine.
  /// </summary>
  public class cCheckParameterDataRowViewValue : cCheckParameterAbstractValue<DataRowView, DataRowView>
  {
  
    #region Public Constructors

    /// <summary>
    /// Create and instance of a DataRowView value reference parameter object setting the Parameter 
    /// name and data type.
    /// </summary>
    /// <param name="AParameterKey">The key of the parameter.</param>
    /// <param name="AParameterName">The name of the parameter.</param>
    /// <param name="ACheckParameters">The parent check parameters object.</param>
    public cCheckParameterDataRowViewValue(int? AParameterKey, string AParameterName,
                                           cCheckParameters ACheckParameters)
      : base(AParameterKey, AParameterName, eParameterDataType.DataRowView, ACheckParameters)
    {
    }

    #endregion


    #region Public Methods

    /// <summary>
    /// Returns true if the passed column name is a column in the DataRowView of the parameter.
    /// </summary>
    /// <param name="AColumnName">The column name to test for existence in the DataRowView.</param>
    /// <returns>Returns true if the column name is in the DataRowView.</returns>
    public bool IsColumn(string AColumnName)
    {
      if (FValue != null)
        return FValue.Row.Table.Columns.Contains(AColumnName);
      else
        return false;
    }

    #endregion


    #region Public Override Methods

    /// <summary>
    /// Returns this parameter's default DataRowView value for nulls.
    /// </summary>
    /// <returns>Returns the default DataRowView value for nulls.</returns>
    public override DataRowView GetDefaultForNull()
    {
      return null;
    }

    /// <summary>
    /// Retursn this parameter's default DataRowView value for nulls as an object.
    /// </summary>
    /// <returns>Returns the default DataRowView value for nulls as an object.</returns>
    public override object GetDefaultObjectForNull()
    {
      return GetDefaultForNull();
    }

    /// <summary>
    /// Returns null typed as a DataRowView.
    /// </summary>
    /// <returns>Returns null as a DataRowView.</returns>
    public override DataRowView GetNull()
    {
      return null;
    }

    /// <summary>
    /// Returns the passed value if it is not null, otherwise it returns the default DataRowView for nulls.
    /// </summary>
    /// <param name="ANullableValue">The value to convert.</param>
    /// <returns>The converted value.</returns>
    public override DataRowView GetTypeValue(DataRowView ANullableValue)
    {
      return ANullableValue;
    }

    #endregion


    #region Protected Override Methods

    /// <summary>
    /// Converts the input object to a data row.
    /// </summary>
    /// <param name="AInputValue">The object to convert.</param>
    /// <param name="AOutputValue">The converted value.</param>
    /// <param name="AErrorMessage">The error message returned if the conversion fails.</param>
    /// <returns>Returns false if the conversion failed.</returns>
    protected override bool ConvertObjectType(object AInputValue, ref DataRowView AOutputValue, ref string AErrorMessage)
    {
      try
      {
        AOutputValue = (DataRowView)AInputValue;
        return true;
      }
      catch (Exception ex)
      {
        AErrorMessage = "[" + this.Name + ".ConvertObjectType]: " + ex.Message;
        return false;
      }
    }

    /// <summary>
    /// Sets the value property field to null, typed as a DataRowView.
    /// </summary>
    protected override void ResetValue()
    {
      FValue = null;
    }

    #endregion

  }

}
