using System;
using System.Collections.Generic;
using System.Text;

namespace ECMPS.Checks.Parameters
{

  /// <summary>
  /// The string value reference parameter object used by the check engine.
  /// </summary>
  public class cCheckParameterStringValueReference : cCheckParameterAbstractValueReference<string, string>
  {

    #region Public Constructors

    /// <summary>
    /// Create and instance of a string value parameter object setting the Parameter name and data type,
    /// setting the default value reference to use in place of a null or dbnull to a null string,
    /// and setting the aggregate delimiter property to a comma.
    /// </summary>
    /// <param name="AParameterKey">The key of the parameter.</param>
    /// <param name="AParameterName">The name of the parameter.</param>
    /// <param name="ACheckParameters">The parent check parameters object.</param>
    public cCheckParameterStringValueReference(int? AParameterKey, string AParameterName,
                                               cCheckParameters ACheckParameters)
      : base(AParameterKey, AParameterName, eParameterDataType.String, ACheckParameters)
    {
    }

    #endregion


    #region Public Override Methods

    /// <summary>
    /// Returns the referenced parameter's default string value for nulls.
    /// </summary>
    /// <returns>Returns the referenced parameter's default string value for nulls.</returns>
    public override string GetDefaultForNull()
    {
      if (FReferencedParameter != null)
        return ReferencedParameter.GetDefaultForNull();
      else
        return "";
    }

    /// <summary>
    /// Return the referenced parameter's default string value for nulls as an object.
    /// </summary>
    /// <returns>Returns the referenced parameter's the default string value for nulls as an object.</returns>
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

  }

}
