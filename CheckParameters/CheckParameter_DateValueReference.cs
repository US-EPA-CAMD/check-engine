using System;
using System.Collections.Generic;
using System.Text;

namespace ECMPS.Checks.Parameters
{

  /// <summary>
  /// The date value reference parameter object used by the check engine.
  /// </summary>
  public class cCheckParameterDateValueReference : cCheckParameterAbstractValueReference<DateTime, DateTime?>
  {

    #region Public Constructors

    /// <summary>
    /// Create and instance of a date value reference parameter object setting the Parameter name and data type,
    /// and setting the default value to use in place of a null or dbnull to DateTime.MinValue.
    /// </summary>
    /// <param name="AParameterKey">The key of the parameter.</param>
    /// <param name="AParameterName">The name of the parameter.</param>
    /// <param name="ACheckParameters">The parent check parameters object.</param>
    public cCheckParameterDateValueReference(int? AParameterKey, string AParameterName,
                                             cCheckParameters ACheckParameters)
      : base(AParameterKey, AParameterName, eParameterDataType.Date, ACheckParameters)
    {
    }

    #endregion


    #region Public Override Methods

    /// <summary>
    /// Returns the referenced parameter's default DateTime value for nulls.
    /// </summary>
    /// <returns>Returns the referenced parameter's default DateTime value for nulls.</returns>
    public override DateTime GetDefaultForNull()
    {
      if (FReferencedParameter != null)
        return ReferencedParameter.GetDefaultForNull();
      else
        return DateTime.MinValue;
    }

    /// <summary>
    /// Return the referenced parameter's default DateTime value for nulls as an object.
    /// </summary>
    /// <returns>Returns the referenced parameter's the default DateTime value for nulls as an object.</returns>
    public override object GetDefaultObjectForNull()
    {
      return GetDefaultForNull();
    }

    /// <summary>
    /// Returns null typed as a nullable DateTime.
    /// </summary>
    /// <returns>Returns null as a nullable DateTime.</returns>
    public override DateTime? GetNull()
    {
      return null;
    }

    /// <summary>
    /// Converts the passed value from the nullable DateTime to DateTime by returning the
    /// passed value if it is not null, otherwise returning the default DateTime for null.
    /// </summary>
    /// <param name="ANullableValue">The value to convert.</param>
    /// <returns>The converted value.</returns>
    public override DateTime GetTypeValue(DateTime? ANullableValue)
    {
      return (ANullableValue ?? GetDefaultForNull());
    }

    #endregion

  }

}
