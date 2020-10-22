using System;
using System.Collections.Generic;
using System.Text;

using System.Data;

namespace ECMPS.Checks.Parameters
{

  /// <summary>
  /// The DataRowView value reference parameter object used by the check engine.
  /// </summary>
  public class cCheckParameterDataRowViewValueReference : cCheckParameterAbstractValueReference<DataRowView, DataRowView>
  {

    #region Public Constructors)

    /// <summary>
    /// Create and instance of a DataRowView value parameter object setting the Parameter name and 
    /// data type.
    /// </summary>
    /// <param name="AParameterKey">The key of the parameter.</param>
    /// <param name="AParameterName">The name of the parameter.</param>
    /// <param name="ACheckParameters">The parent check parameters object.</param>
    public cCheckParameterDataRowViewValueReference(int? AParameterKey, string AParameterName,
                                                    cCheckParameters ACheckParameters)
      : base(AParameterKey, AParameterName, eParameterDataType.DataRowView, ACheckParameters)
    {
    }

    #endregion


    #region Public Methods

    /// <summary>
    /// Returns true if the passed column name is a column in the DataRowView of the referenced 
    /// parameter.
    /// </summary>
    /// <param name="AColumnName"></param>
    /// <returns></returns>
    public bool IsColumn(string AColumnName)
    {
      if ((FReferencedParameter != null) && (FReferencedParameter is cCheckParameterDataRowViewValue))
        return ((cCheckParameterDataRowViewValue)FReferencedParameter).IsColumn(AColumnName);
      else
        return false;
    }

    #endregion


    #region Public Override Methods

    /// <summary>
    /// Returns the referenced parameter's default DataRowView value for nulls.
    /// </summary>
    /// <returns>Returns the referenced parameter's default DataRowView value for nulls.</returns>
    public override DataRowView GetDefaultForNull()
    {
      if (FReferencedParameter != null)
        return FReferencedParameter.GetDefaultForNull();
      else
        return null;
    }

    /// <summary>
    /// Return the referenced parameter's default DataRowView value for nulls as an object.
    /// </summary>
    /// <returns>Returns the referenced parameter's the default DataRowView value for nulls as an object.</returns>
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

  }

}
