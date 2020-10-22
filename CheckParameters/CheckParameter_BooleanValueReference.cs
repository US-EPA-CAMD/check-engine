using System;
using System.Collections.Generic;
using System.Text;

namespace ECMPS.Checks.Parameters
{

  /// <summary>
  /// The boolean value reference parameter object used by the check engine.
  /// </summary>
  public class cCheckParameterBooleanValueReference : cCheckParameterAbstractValueReference<bool, bool?>
  {

    #region Public Constructors

    /// <summary>
    /// Create and instance of a boolean value reference parameter object setting the Parameter name and data type,
    /// and setting the default value to use in place of a null or dbnull to false.
    /// </summary>
    /// <param name="AParameterKey">The key of the parameter.</param>
    /// <param name="AParameterName">The name of the parameter.</param>
    /// <param name="ACheckParameters">The parent check parameters object.</param>
    public cCheckParameterBooleanValueReference(int? AParameterKey, string AParameterName,
                                                cCheckParameters ACheckParameters)
      : base(AParameterKey, AParameterName, eParameterDataType.Boolean, ACheckParameters)
    {
    }

    #endregion


    #region Public Override Methods

    /// <summary>
    /// Returns the referenced parameter's default bool value for nulls.
    /// </summary>
    /// <returns>Returns the referenced parameter's default bool value for nulls.</returns>
    public override bool GetDefaultForNull()
    {
      if (FReferencedParameter != null)
        return ReferencedParameter.GetDefaultForNull();
      else
        return false;
    }

    /// <summary>
    /// Return the referenced parameter's default bool value for nulls as an object.
    /// </summary>
    /// <returns>Returns the referenced parameter's the default bool value for nulls as an object.</returns>
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

  }

}
