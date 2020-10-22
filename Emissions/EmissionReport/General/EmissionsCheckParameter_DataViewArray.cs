using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ECMPS.Checks.Parameters
{

  /// <summary>
  /// The string value parameter object used by the check engine.
  /// </summary>
  public class cEmissionsCheckParameter_DataViewArray : cCheckParameterAbstractValue<DataView[], DataView[]>
  {
  
    #region Public Constructors

    /// <summary>
    /// Create and instance of a string value parameter object setting the Parameter name and data type.
    /// </summary>
    /// <param name="AParameterKey">The key of the parameter.</param>
    /// <param name="AParameterName">The name of the parameter.</param>
    /// <param name="ACheckParameters">The parent check parameters object.</param>
    public cEmissionsCheckParameter_DataViewArray(int AParameterKey, string AParameterName,
                                                  cCheckParameters ACheckParameters)
      : base(AParameterKey, AParameterName, eParameterDataType.DataView, ACheckParameters)
    {
      FIsArray = true;
    }

    #endregion


    #region Public Properties with Fields

    #region Property Fields

    private string FAggregateDelimiter = ",";
    private string FDefaultForNull = "";

    #endregion


    /// <summary>
    /// The deliniter used to separate aggregated (concatenated) strings.
    /// </summary>
    public string AggregateDelimiter
    {
      get { return FAggregateDelimiter; }
    }

    /// <summary>
    /// The default int value when the value is null or dbnull.
    /// </summary>
    public string DefaultForNull
    {
      get { return FDefaultForNull; }
    }

    #endregion


    #region Public Override Methods

    /// <summary>
    /// Returns this parameter's default string value for nulls.
    /// </summary>
    /// <returns>Returns the default string value for nulls.</returns>
    public override DataView[] GetDefaultForNull()
    {
      return (DataView[])null;
    }

    /// <summary>
    /// Retursn this parameter's default string value for nulls as an object.
    /// </summary>
    /// <returns>Returns the default string value for nulls as an object.</returns>
    public override object GetDefaultObjectForNull()
    {
      return GetDefaultForNull();
    }

    /// <summary>
    /// Returns null typed as a string.
    /// </summary>
    /// <returns>Returns null as a string.</returns>
    public override DataView[] GetNull()
    {
      return (DataView[])null;
    }

    /// <summary>
    /// Returns the passed value if it is not null, otherwise it returns the default string for nulls.
    /// </summary>
    /// <param name="ANullableValue">The value to convert.</param>
    /// <returns>The converted value.</returns>
    public override DataView[] GetTypeValue(DataView[] ANullableValue)
    {
      return (ANullableValue ?? GetDefaultForNull());
    }

    #endregion


    #region Protected Override Methods

    /// <summary>
    /// Converts the input object to a string.
    /// </summary>
    /// <param name="AInputValue">The object to convert.</param>
    /// <param name="AOutputValue">The converted value.</param>
    /// <param name="AErrorMessage">The error message returned if the conversion fails.</param>
    /// <returns>Returns false if the conversion failed.</returns>
    protected override bool ConvertObjectType(object AInputValue, ref DataView[] AOutputValue, ref string AErrorMessage)
    {
      try
      {
        AOutputValue = (DataView[])AInputValue;
        return true;
      }
      catch (Exception ex)
      {
        AErrorMessage = "[" + this.Name + ".ConvertObjectType]: " + ex.Message;
        return false;
      }
    }

    /// <summary>
    /// Sets the value property field to null, typed as a string.
    /// </summary>
    protected override void ResetValue()
    {
      FValue = (DataView[])null;
    }

    #endregion

  }

}
