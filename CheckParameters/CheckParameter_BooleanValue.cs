using System;
using System.Collections.Generic;
using System.Text;

namespace ECMPS.Checks.Parameters
{

  /// <summary>
  /// The boolean value parameter object used by the check engine.
  /// </summary>
  public class cCheckParameterBooleanValue : cCheckParameterAbstractValue<bool, bool?>
  {
  
    #region Public Constructors

    /// <summary>
    /// Create and instance of a boolean value parameter object setting the Parameter name and data type,
    /// and setting the default value to use in place of a null or dbnull to the passed value.
    /// </summary>
    /// <param name="AParameterKey">The key of the parameter.</param>
    /// <param name="AParameterName">The name of the parameter.</param>
    /// <param name="ADefaultForNull">The default value to use in place of a null or dbnull.</param>
    /// <param name="ACheckParameters">The parent check parameters object.</param>
    public cCheckParameterBooleanValue(int? AParameterKey, string AParameterName,
                                       bool ADefaultForNull,
                                       cCheckParameters ACheckParameters)
      : base(AParameterKey, AParameterName, eParameterDataType.Boolean, ACheckParameters)
    {
      FDefaultForNull = ADefaultForNull;
    }

    /// <summary>
    /// Create and instance of a boolean value parameter object setting the Parameter name and data type,
    /// and setting the default value to use in place of a null or dbnull to false.
    /// </summary>
    /// <param name="AParameterKey">The key of the parameter.</param>
    /// <param name="AParameterName">The name of the parameter.</param>
    /// <param name="ACheckParameters">The parent check parameters object.</param>
    public cCheckParameterBooleanValue(int? AParameterKey, string AParameterName,
                                       cCheckParameters ACheckParameters)
      : base(AParameterKey, AParameterName, eParameterDataType.Boolean, ACheckParameters)
    {
      FDefaultForNull = false;
    }

    #endregion


    #region Public Properties with Fields

    #region Property Fields

    private bool FDefaultForNull = false;

    #endregion


    /// <summary>
    /// The default bool value when the value is null or dbnull.
    /// </summary>
    public bool DefaultForNull { get { return FDefaultForNull; } }

    #endregion


    #region Public Methods

    /// <summary>
    /// Peforms a boolean and or a boolean or on the current parameter value and the passed value.
    /// If the current parameter value is not set or is null then the parameter value is assinged
    /// the passed value.
    /// </summary>
    /// <param name="AValue">The value to 'or' and 'and' to the current parameter value.</param>
    /// <param name="AOrTogether">Perform a boolean or if true and a boolean and if false.</param>
    /// <param name="ACategory">The category updating the parameter</param>
    /// <returns>Returns the result of the boolean operation.</returns>
    public bool AggregateValue(bool AValue, bool AOrTogether, cCheckCategory ACategory)
    {
      if (UpdateValueCheck(ACategory))
      {
        if (!FValue.HasValue)
          FValue = AValue;
        else if (AOrTogether)
          FValue = (AValue || FValue.Value);
        else
          FValue = (AValue && FValue.Value);

        return FValue.Value;
      }
      else
      {
        return GetDefaultForNull();
      }
    }

    #endregion


    #region Public Override Methods

    /// <summary>
    /// Returns this parameter's default bool value for nulls.
    /// </summary>
    /// <returns>Returns the default bool value for nulls.</returns>
    public override bool GetDefaultForNull()
    {
      return FDefaultForNull;
    }

    /// <summary>
    /// Retursn this parameter's default bool value for nulls as an object.
    /// </summary>
    /// <returns>Returns the default bool value for nulls as an object.</returns>
    public override object GetDefaultObjectForNull()
    {
      return GetDefaultForNull();
    }

    /// <summary>
    /// Returns null typed as a nullable bool.
    /// </summary>
    /// <returns>Returns null as a nullable bool.</returns>
    public override bool? GetNull()
    {
      return null;
    }

    /// <summary>
    /// Converts the passed value from the nullable bool to bool by returning the
    /// passed value if it is not null, otherwise returning the default bool for null.
    /// </summary>
    /// <param name="ANullableValue">The value to convert.</param>
    /// <returns>The converted value.</returns>
    public override bool GetTypeValue(bool? ANullableValue)
    {
      return (ANullableValue ?? GetDefaultForNull());
    }

    #endregion


    #region Protected Override Methods

    /// <summary>
    /// Converts the input object to a nullable bool.
    /// </summary>
    /// <param name="AInputValue">The object to convert.</param>
    /// <param name="AOutputValue">The converted value.</param>
    /// <param name="AErrorMessage">The error message returned if the conversion fails.</param>
    /// <returns>Returns false if the conversion failed.</returns>
    protected override bool ConvertObjectType(object AInputValue, ref bool? AOutputValue, ref string AErrorMessage)
    {
      try
      {
        AOutputValue = (bool?)AInputValue;

        return true;
      }
      catch (Exception ex)
      {
        AErrorMessage = "[" + this.Name + ".ConvertObjectType]: " + ex.Message;
        return false;
      }
    }

    /// <summary>
    /// Sets the value property field to null, typed as a nullable bool.
    /// </summary>
    protected override void ResetValue()
    {
      FValue = (bool?)null;
    }

    #endregion

  }

}
