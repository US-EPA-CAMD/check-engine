using System;
using System.Collections.Generic;
using System.Text;

namespace ECMPS.Checks.Parameters
{

  /// <summary>
  /// The boolean array parameter object used by the check engine.
  /// </summary>
  public class cCheckParameterBooleanArray : cCheckParameterAbstractArray<bool, bool?>
  {
   
    #region Public Constructors

    /// <summary>
    /// Create and instance of a boolean array parameter object setting the Parameter name and data type,
    /// and setting the default value to use in place of a null or dbnull to the passed value.
    /// </summary>
    /// <param name="AParameterKey">The key of the parameter.</param>
    /// <param name="AParameterName">The name of the parameter.</param>
    /// <param name="ADefaultForNull">The default value to use in place of a null or dbnull.</param>
    /// <param name="ACheckParameters">The parent check parameters object.</param>
    public cCheckParameterBooleanArray(int? AParameterKey, string AParameterName,
                                       bool ADefaultForNull,
                                       cCheckParameters ACheckParameters)
      : base(AParameterKey, AParameterName, eParameterDataType.Boolean, ACheckParameters)
    {
      FDefaultForNull = ADefaultForNull;
    }

    /// <summary>
    /// Create and instance of a boolean array parameter object setting the Parameter name and data type,
    /// and setting the default value to use in place of a null or dbnull to false.
    /// </summary>
    /// <param name="AParameterKey">The key of the parameter.</param>
    /// <param name="AParameterName">The name of the parameter.</param>
    /// <param name="ACheckParameters">The parent check parameters object.</param>
    public cCheckParameterBooleanArray(int? AParameterKey, string AParameterName,
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
    public bool DefaultForNull
    {
      get { return FDefaultForNull; }
    }

    #endregion


    #region Public Methods

    /// <summary>
    /// Peforms a boolean and or a boolean or on the specified element of the current parameter 
    /// array and the passed value.  If the current parameter array element is not set or is null 
    /// then the parameter array element is assinged the passed value.
    /// </summary>
    /// <param name="AValue">The value to 'or' or 'and' to the current parameter value array element.</param>
    /// <param name="AIndex">The current parameter array element index.</param>
    /// <param name="AOrTogether">Perform a boolean or if true and a boolean and if false.</param>
    /// <param name="ACategory">The category updating the parameter</param>
    /// <returns>Returns the result of the boolean operation.</returns>
    public bool AggregateValue(bool AValue, int AIndex, bool AOrTogether, cCheckCategory ACategory)
    {
      if (UpdateValueCheck(ACategory))
      {
        if (!FValue[AIndex].HasValue)
          FValue[AIndex] = AValue;
        else if (AOrTogether)
          FValue[AIndex] = (AValue || FValue[AIndex].Value);
        else
          FValue[AIndex] = (AValue && FValue[AIndex].Value);

        return (FValue[AIndex].Value);
      }
      else
      {
        return FDefaultForNull;
      }
    }

    #endregion


    #region Public Override Methods

    /// <summary>
    /// Returns this array parameter with each element's value as the default bool value for nulls.
    /// </summary>
    /// <returns>Returns the default bool value for nulls.</returns>
    public override bool GetDefaultForNull()
    {
      return FDefaultForNull;
    }

    /// <summary>
    /// Returns this array parameter with each element's value as the default bool value for nulls,
    /// as an object.
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
    /// Converts the input object to a nullable bool array.
    /// </summary>
    /// <param name="AInputValue">The object to convert.</param>
    /// <param name="AOutputValue">The converted value.</param>
    /// <param name="AErrorMessage">The error message returned if the conversion fails.</param>
    /// <returns>Returns false if the conversion failed.</returns>
    protected override bool ConvertObjectType(object AInputValue, ref bool?[] AOutputValue, ref string AErrorMessage)
    {
      try
      {
        if (typeof(bool[]).IsAssignableFrom(AInputValue.GetType()))
        {
          bool[] TempArray = (bool[])AInputValue;

          AOutputValue = new bool?[TempArray.Length];

          for (int Dex = 0; Dex < TempArray.Length; Dex++)
          {
            AOutputValue[Dex] = TempArray[Dex];
          }

          return true;
        }
        else if (typeof(bool?[]).IsAssignableFrom(AInputValue.GetType()))
        {
          AOutputValue = (bool?[])AInputValue;
          return true;
        }
        else
        {
          AErrorMessage = "[" + this.Name + ".ConvertObjectType]: Cannot convert type '" + AInputValue.GetType().Name + "' to 'bool?[]'";
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
    /// Sets the value property field to null, typed as a nullable bool array.
    /// </summary>
    protected override void ResetValue()
    {
      FValue = (bool?[])null;
    }

    #endregion

 }

}
