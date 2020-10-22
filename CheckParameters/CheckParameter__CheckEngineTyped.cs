using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ECMPS.Checks.Parameters
{
  /// <summary>
  /// The parent type of all type parameters used by the check engine.
  /// </summary>
  /// <typeparam name="Type">The base type of the parameter value</typeparam>
  /// <typeparam name="NullableType">The nullable type of the parameter value</typeparam>
  public abstract class cCheckParameterCheckEngineTyped<Type, NullableType> : cCheckParameterCheckEngine
  {

    #region Protected Constructors

    /// <summary>
    /// Creates an instance of cCheckParameterCheckEngineTyped and sets the Name and DataType property.
    /// </summary>
    /// <param name="AParameterKey">The key of the parameter.</param>
    /// <param name="AParameterName">The name of the parameter.</param>
    /// <param name="AParameterDataType">The data type of the parameter.</param>
    /// <param name="ACheckParameters">The parent check parameters object.</param>
    protected cCheckParameterCheckEngineTyped(int? AParameterKey, string AParameterName, eParameterDataType AParameterDataType,
                                              cCheckParameters ACheckParameters)
      : base(AParameterKey, AParameterName, AParameterDataType, ACheckParameters)
    {
      if (typeof(Type) == typeof(bool))
        FDataType = eParameterDataType.Boolean;
      else if (typeof(Type) == typeof(DataRowView))
        FDataType = eParameterDataType.DataRowView;
      else if (typeof(Type) == typeof(DataView))
        FDataType = eParameterDataType.DataView;
      else if (typeof(Type) == typeof(DateTime))
        FDataType = eParameterDataType.Date;
      else if (typeof(Type) == typeof(decimal))
        FDataType = eParameterDataType.Decimal;
      else if (typeof(Type) == typeof(int))
        FDataType = eParameterDataType.Integer;
      else if (typeof(Type) == typeof(string))
        FDataType = eParameterDataType.String;
      else
        FDataType = eParameterDataType.Object;
    }

    #endregion


    #region Public Abstract Methods

    /// <summary>
    /// Returns the default value for representing Null or DBNULL values.
    /// </summary>
    /// <returns>The default value for representing Null or DBNULL values.</returns>
    public abstract Type GetDefaultForNull();

    /// <summary>
    /// Returns null as the nullable type of the parameter.
    /// </summary>
    /// <returns>Null as the nullable type of the parameter.</returns>
    public abstract NullableType GetNull();

    /// <summary>
    /// Returns the base type value reflecting the passed nullable type value.
    /// </summary>
    /// <param name="ANullableValue">The nullabe type value to convert.</param>
    /// <returns>The base type value corresponding to the passed nullable type value.</returns>
    public abstract Type GetTypeValue(NullableType ANullableValue);

    #endregion

  }
}
