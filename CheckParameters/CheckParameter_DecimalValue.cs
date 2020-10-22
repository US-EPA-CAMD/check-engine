using System;
using System.Collections.Generic;
using System.Text;

namespace ECMPS.Checks.Parameters
{

  /// <summary>
  /// The decimal value parameter object used by the check engine.
  /// </summary>
  public class cCheckParameterDecimalValue : cCheckParameterAbstractValue<decimal, decimal?>
  {
  
    #region Public Constructors

    /// <summary>
    /// Create and instance of a decimal value parameter object setting the Parameter name and data type,
    /// and setting the default value to use in place of a null or dbnull to the passed value.
    /// </summary>
    /// <param name="AParameterKey">The key of the parameter.</param>
    /// <param name="AParameterName">The name of the parameter.</param>
    /// <param name="ADefaultForNull">The default value to use in place of a null or dbnull.</param>
    /// <param name="ACheckParameters">The parent check parameters object.</param>
    public cCheckParameterDecimalValue(int? AParameterKey, string AParameterName,
                                       decimal ADefaultForNull,
                                       cCheckParameters ACheckParameters)
      : base(AParameterKey, AParameterName, eParameterDataType.Decimal, ACheckParameters)
    {
      FDefaultForNull = ADefaultForNull;
    }

    /// <summary>
    /// Create and instance of a decimal value parameter object setting the Parameter name and data type,
    /// and setting the default value to use in place of a null or dbnull to decimal.MinValue.
    /// </summary>
    /// <param name="AParameterKey">The key of the parameter.</param>
    /// <param name="AParameterName">The name of the parameter.</param>
    /// <param name="ACheckParameters">The parent check parameters object.</param>
    public cCheckParameterDecimalValue(int? AParameterKey, string AParameterName,
                                       cCheckParameters ACheckParameters)
      : base(AParameterKey, AParameterName, eParameterDataType.Decimal, ACheckParameters)
    {
      FDefaultForNull = decimal.MinValue;
    }

    #endregion


    #region Public Properties with Fields

    #region Property Fields

    private decimal FDefaultForNull = decimal.MinValue;

    #endregion


    /// <summary>
    /// The default decimal value when the value is null or dbnull.
    /// </summary>
    public decimal DefaultForNull
    {
      get { return FDefaultForNull; }
    }

    #endregion


    #region Public Methods

    /// <summary>
    /// Sums the current parameter value and the passed value.  If the current parameter value
    /// is not set or is null then the parameter value is assinged the passed value.
    /// </summary>
    /// <param name="AValue">The value to sum to the current parameter value.</param>
    /// <param name="ACategory">The category updating the parameter</param>
    /// <returns>Returns the result of the summation.</returns>
    public decimal AggregateValue(decimal AValue, cCheckCategory ACategory)
    {
      if (UpdateValueCheck(ACategory))
      {
        if (!FValue.HasValue)
          FValue = AValue;
        else
          FValue = (AValue + FValue.Value);

        return (FValue.Value);
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

    #endregion


    #region Public Override Methods

    /// <summary>
    /// Returns this parameter's default decimal value for nulls.
    /// </summary>
    /// <returns>Returns the default decimal value for nulls.</returns>
    public override decimal GetDefaultForNull()
    {
      return FDefaultForNull;
    }

    /// <summary>
    /// Retursn this parameter's default decimal value for nulls as an object.
    /// </summary>
    /// <returns>Returns the default decimal value for nulls as an object.</returns>
    public override object GetDefaultObjectForNull()
    {
      return GetDefaultForNull();
    }

    /// <summary>
    /// Returns null typed as a nullable decimal.
    /// </summary>
    /// <returns>Returns null as a nullable decimal.</returns>
    public override decimal? GetNull()
    {
      return null;
    }

    /// <summary>
    /// Converts the passed value from the nullable decimal to decimal by returning the
    /// passed value if it is not null, otherwise returning the default decimal for null.
    /// </summary>
    /// <param name="ANullableValue">The value to convert.</param>
    /// <returns>The converted value.</returns>
    public override decimal GetTypeValue(decimal? ANullableValue)
    {
      return (ANullableValue ?? GetDefaultForNull());
    }

    #endregion


    #region Protected Override Methods

    /// <summary>
    /// Converts the input object to a nullable decimal.
    /// </summary>
    /// <param name="AInputValue">The object to convert.</param>
    /// <param name="AOutputValue">The converted value.</param>
    /// <param name="AErrorMessage">The error message returned if the conversion fails.</param>
    /// <returns>Returns false if the conversion failed.</returns>
    protected override bool ConvertObjectType(object AInputValue, ref decimal? AOutputValue, ref string AErrorMessage)
    {
      try
      {
        AOutputValue = (decimal?)AInputValue;

        if (AOutputValue == FDefaultForNull)
          AOutputValue = null;

        return true;
      }
      catch (Exception ex)
      {
        AErrorMessage = "[" + this.Name + ".ConvertObjectType]: " + ex.Message;
        return false;
      }
    }

    /// <summary>
    /// Sets the value property field to null, typed as a nullable decimal.
    /// </summary>
    protected override void ResetValue()
    {
      FValue = (decimal?)null;
    }

    #endregion

  }

}
