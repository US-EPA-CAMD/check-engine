using System;
using System.Collections.Generic;
using System.Text;

namespace ECMPS.Checks.Parameters
{

  /// <summary>
  /// The string value parameter object used by the check engine.
  /// </summary>
  public class cCheckParameterStringValue : cCheckParameterAbstractValue<string, string>
  {
  
    #region Public Constructors

    /// <summary>
    /// Create and instance of a string value parameter object setting the Parameter name and data type,
    /// setting the default value to use in place of a null or dbnull to the passed value,
    /// and setting the aggregate delimiter property to the passed value.
    /// </summary>
    /// <param name="AParameterKey">The key of the parameter.</param>
    /// <param name="AParameterName">The name of the parameter.</param>
    /// <param name="ADefaultForNull">The default value to use in place of a null or dbnull.</param>
    /// <param name="AAggregateDelimiter">The delimiter between strings aggregated into the value.</param>
    /// <param name="ACheckParameters">The parent check parameters object.</param>
    public cCheckParameterStringValue(int? AParameterKey, string AParameterName,
                                      string ADefaultForNull, string AAggregateDelimiter,
                                      cCheckParameters ACheckParameters)
      : base(AParameterKey, AParameterName, eParameterDataType.String, ACheckParameters)
    {
      FIsArray = false;
      FDefaultForNull = ADefaultForNull;
      FAggregateDelimiter = AAggregateDelimiter;
    }

    /// <summary>
    /// Create and instance of a string value parameter object setting the Parameter name and data type,
    /// setting the default value to use in place of a null or dbnull to a null string,
    /// and setting the aggregate delimiter property to the passed value.
    /// </summary>
    /// <param name="AParameterKey">The key of the parameter.</param>
    /// <param name="AParameterName">The name of the parameter.</param>
    /// <param name="AAggregateDelimiter">The delimiter between strings aggregated into the value.</param>
    /// <param name="ACheckParameters">The parent check parameters object.</param>
    public cCheckParameterStringValue(int? AParameterKey, string AParameterName,
                                      string AAggregateDelimiter,
                                      cCheckParameters ACheckParameters)
      : base(AParameterKey, AParameterName, eParameterDataType.String, ACheckParameters)
    {
      FIsArray = false;
      FDefaultForNull = "";
      FAggregateDelimiter = AAggregateDelimiter;
    }

    /// <summary>
    /// Create and instance of a string value parameter object setting the Parameter name and data type,
    /// setting the default value to use in place of a null or dbnull to a null string,
    /// and setting the aggregate delimiter property to a comma.
    /// </summary>
    /// <param name="AParameterKey">The key of the parameter.</param>
    /// <param name="AParameterName">The name of the parameter.</param>
    /// <param name="ACheckParameters">The parent check parameters object.</param>
    public cCheckParameterStringValue(int? AParameterKey, string AParameterName,
                                      cCheckParameters ACheckParameters)
      : base(AParameterKey, AParameterName, eParameterDataType.String, ACheckParameters)
    {
      FIsArray = false;
      FDefaultForNull = "";
      FAggregateDelimiter = ",";
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


    #region Public Methods

    /// <summary>
    /// Concatenates the current parameter value and the passed value using the passed delimiter.  
    /// If the current parameter value is not set or is null then the parameter value is assinged 
    /// the passed value.
    /// </summary>
    /// <param name="AValue">The value to concatenate to the current parameter value.</param>
    /// <param name="AAggregateDelimiter">The delimiter used in the concatenation.</param>
    /// <returns>Returns the result of the concatenation.</returns>
    public string AggregateValue(string AValue, string AAggregateDelimiter)
    {
      if (IsSet)
      {
        if (string.IsNullOrEmpty(FValue))
          FValue = AValue;
        else
          FValue += (AAggregateDelimiter + AValue);

        return (FValue);
      }
      else
      {
        if (FOwner != null)
          System.Diagnostics.Debug.WriteLine(string.Format("Aggregation of uninitialized parameters is not allowed: Category - {0}, Parameter - {1}", FOwner.CategoryCd, this.Name));
        else
          System.Diagnostics.Debug.WriteLine(string.Format("Aggregation of uninitialized parameters is not allowed: Parameter - {0}", this.Name));

        return GetDefaultForNull();
      }
    }

    /// <summary>
    /// Concatenates the current parameter value and the passed value using the default delimiter
    /// for the parameter.  If the current parameter value is not set or is null then the parameter
    /// value is assinged the passed value.
    /// </summary>
    /// <param name="AValue">The value to concatenate to the current parameter value.</param>
    /// <returns>Returns the result of the concatenation.</returns>
    public string AggregateValue(string AValue)
    {
      return AggregateValue(AValue, FAggregateDelimiter);
    }

    #endregion


    #region Public Override Methods

    /// <summary>
    /// Returns this parameter's default string value for nulls.
    /// </summary>
    /// <returns>Returns the default string value for nulls.</returns>
    public override string GetDefaultForNull()
    {
      return FDefaultForNull;
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
    public override string GetNull()
    {
      return null;
    }

    /// <summary>
    /// Returns the passed value if it is not null, otherwise it returns the default string for nulls.
    /// </summary>
    /// <param name="ANullableValue">The value to convert.</param>
    /// <returns>The converted value.</returns>
    public override string GetTypeValue(string ANullableValue)
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
    protected override bool ConvertObjectType(object AInputValue, ref string AOutputValue, ref string AErrorMessage)
    {
      try
      {
        AOutputValue = (string)AInputValue;
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
      FValue = (string)null;
    }

    #endregion

  }

}
