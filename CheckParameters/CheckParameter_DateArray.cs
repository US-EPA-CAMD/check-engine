using System;
using System.Collections.Generic;
using System.Text;

namespace ECMPS.Checks.Parameters
{

  /// <summary>
  /// The date array parameter object used by the check engine.
  /// </summary>
  public class cCheckParameterDateArray : cCheckParameterAbstractArray<DateTime, DateTime?>
  {

    #region Public Constructors

    /// <summary>
    /// Create and instance of a date array parameter object setting the Parameter name and data type,
    /// and setting the default value to use in place of a null or dbnull to the passed value.
    /// </summary>
    /// <param name="AParameterKey">The key of the parameter.</param>
    /// <param name="AParameterName">The name of the parameter.</param>
    /// <param name="ADefaultForNull">The default value to use in place of a null or dbnull.</param>
    /// <param name="ACheckParameters">The parent check parameters object.</param>
    public cCheckParameterDateArray(int? AParameterKey, string AParameterName,
                                    DateTime ADefaultForNull,
                                    cCheckParameters ACheckParameters)
      : base(AParameterKey, AParameterName, eParameterDataType.Date, ACheckParameters)
    {
      FDefaultForNull = ADefaultForNull;
    }

    /// <summary>
    /// Create and instance of a date array parameter object setting the Parameter name and data type,
    /// and setting the default value to use in place of a null or dbnull to DateTime.MinValue if the
    /// passed ADateBorderType is Began and DateTime.MaxValue if the ADateBorderType is Ended.
    /// </summary>
    /// <param name="AParameterKey">The key of the parameter.</param>
    /// <param name="AParameterName">The name of the parameter.</param>
    /// <param name="ADateBorderType">Indicates whether the date represents a begin or end date in a range.</param>
    /// <param name="ACheckParameters">The parent check parameters object.</param>
    public cCheckParameterDateArray(int? AParameterKey, string AParameterName,
                                    eDateBorderType ADateBorderType,
                                    cCheckParameters ACheckParameters)
      : base(AParameterKey, AParameterName, eParameterDataType.Date, ACheckParameters)
    {
      if (ADateBorderType == eDateBorderType.Ended)
        FDefaultForNull = DateTime.MaxValue;
      else
        FDefaultForNull = DateTime.MinValue;
    }

    /// <summary>
    /// Create and instance of a date array parameter object setting the Parameter name and data type,
    /// and setting the default value to use in place of a null or dbnull to DateTime.MinValue.
    /// </summary>
    /// <param name="AParameterKey">The key of the parameter.</param>
    /// <param name="AParameterName">The name of the parameter.</param>
    /// <param name="ACheckParameters">The parent check parameters object.</param>
    public cCheckParameterDateArray(int? AParameterKey, string AParameterName,
                                    cCheckParameters ACheckParameters)
      : base(AParameterKey, AParameterName, eParameterDataType.Date, ACheckParameters)
    {
      FDefaultForNull = DateTime.MinValue;
    }

    #endregion


    #region Public Properties with Fields

    #region Property Fields

    private DateTime FDefaultForNull = DateTime.MinValue;

    #endregion


    /// <summary>
    /// The default DateTime value when the value is null or dbnull.
    /// </summary>
    public DateTime DefaultForNull
    {
      get { return FDefaultForNull; }
    }

    #endregion


    #region Public Override Methods

    /// <summary>
    /// Returns this array parameter with each element's value as the default DateTime value for nulls.
    /// </summary>
    /// <returns>Returns the default DateTime value for nulls.</returns>
    public override DateTime GetDefaultForNull()
    {
      return FDefaultForNull;
    }

    /// <summary>
    /// Returns this array parameter with each element's value as the default DateTime value for nulls,
    /// as an object.
    /// </summary>
    /// <returns>Returns the default DateTime value for nulls as an object.</returns>
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


    #region Protected Override Methods

    /// <summary>
    /// Converts the input object to a nullable DateTime array.
    /// </summary>
    /// <param name="AInputValue">The object to convert.</param>
    /// <param name="AOutputValue">The converted value.</param>
    /// <param name="AErrorMessage">The error message returned if the conversion fails.</param>
    /// <returns>Returns false if the conversion failed.</returns>
    protected override bool ConvertObjectType(object AInputValue, ref DateTime?[] AOutputValue, ref string AErrorMessage)
    {
      try
      {
        if (typeof(DateTime[]).IsAssignableFrom(AInputValue.GetType()))
        {
          DateTime[] TempArray = (DateTime[])AInputValue;

          AOutputValue = new DateTime?[TempArray.Length];

          for (int Dex = 0; Dex < TempArray.Length; Dex++)
          {
            AOutputValue[Dex] = TempArray[Dex];
          }

          return true;
        }
        else if (typeof(DateTime?[]).IsAssignableFrom(AInputValue.GetType()))
        {
          AOutputValue = (DateTime?[])AInputValue;
          return true;
        }
        else
        {
          AErrorMessage = "[" + this.Name + ".ConvertObjectType]: Cannot convert type '" + AInputValue.GetType().Name + "' to 'DateTime?[]'";
          return false;
        }
      }
      catch (Exception ex)
      {
        AErrorMessage = "[" + this.Name + ".ConvertObjectType]: " + ex.Message;
        return false;
      }
    }

    /// <summary>
    /// Sets the value property field to null, typed as a nullable DateTime array.
    /// </summary>
    protected override void ResetValue()
    {
      FValue = (DateTime?[])null;
    }

    #endregion

  }

}
