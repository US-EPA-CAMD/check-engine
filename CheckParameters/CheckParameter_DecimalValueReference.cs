using System;
using System.Collections.Generic;
using System.Text;

namespace ECMPS.Checks.Parameters
{

  /// <summary>
  /// The decimal value reference parameter object used by the check engine.
  /// </summary>
  public class cCheckParameterDecimalValueReference : cCheckParameterAbstractValueReference<decimal, decimal?>
  {

    #region Public Constructors

    /// <summary>
    /// Create and instance of a decimal value reference parameter object setting the Parameter name and data type,
    /// and setting the default value to use in place of a null or dbnull to decimal.MinValue.
    /// </summary>
    /// <param name="AParameterKey">The key of the parameter.</param>
    /// <param name="AParameterName">The name of the parameter.</param>
    /// <param name="ACheckParameters">The parent check parameters object.</param>
    public cCheckParameterDecimalValueReference(int? AParameterKey, string AParameterName,
                                                cCheckParameters ACheckParameters)
      : base(AParameterKey, AParameterName, eParameterDataType.Decimal, ACheckParameters)
    {
    }

    #endregion


    #region Public Override Methods

    /// <summary>
    /// Returns the referenced parameter's default decimal value for nulls.
    /// </summary>
    /// <returns>Returns the referenced parameter's default decimal value for nulls.</returns>
    public override decimal GetDefaultForNull()
    {
      if (FReferencedParameter != null)
        return ReferencedParameter.GetDefaultForNull();
      else
        return decimal.MinValue;
    }

    /// <summary>
    /// Return the referenced parameter's default decimal value for nulls as an object.
    /// </summary>
    /// <returns>Returns the referenced parameter's the default decimal value for nulls as an object.</returns>
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

  }

}
