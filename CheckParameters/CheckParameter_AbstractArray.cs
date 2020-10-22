using System;


namespace ECMPS.Checks.Parameters
{
  /// <summary>
  /// The parent type of all type parameters used by the check engine, that represent an array.
  /// </summary>
  /// <typeparam name="Type">The base type of the parameter value</typeparam>
  /// <typeparam name="NullableType">The nullable type of the parameter value</typeparam>
  public abstract class cCheckParameterAbstractArray<Type, NullableType> : cCheckParameterCheckEngineTyped<Type, NullableType>
  {

    #region Protected Constructors

    /// <summary>
    /// Creates an instance of cCheckParameterAbstractArray and sets the Name and DataType property.
    /// </summary>
    /// <param name="AParameterKey">The key of the parameter.</param>
    /// <param name="AParameterName">The name of the parameter.</param>
    /// <param name="AParameterDataType">The data type of the parameter.</param>
    /// <param name="ACheckParameters">The parent check parameters object.</param>
    protected cCheckParameterAbstractArray(int? AParameterKey, string AParameterName, eParameterDataType AParameterDataType,
                                           cCheckParameters ACheckParameters)
      : base(AParameterKey, AParameterName, AParameterDataType, ACheckParameters)
    {
      FIsArray = true;
    }

    #endregion


    #region Public Virtual Get Properties

    #region Property Fields

    /// <summary>
    /// The value of the array with elements as the base type and defaulted when null or dbnull.
    /// </summary>
    protected Type[] FNullDefaultedValue;

    /// <summary>
    /// The value of the array property with elements as a nullable type.
    /// </summary>
    protected NullableType[] FValue;

    #endregion


    /// <summary>
    /// The value of the array with elements as the base type and defaulted when null or dbnull.
    /// </summary>
    public Type[] NullDefaultedValue { get { return (IsSet ? FNullDefaultedValue : null); } }

    /// <summary>
    /// The value of the array property with elements as a nullable type.
    /// </summary>
    public NullableType[] Value { get { return (IsSet ? FValue : null); } }

    #endregion


    #region Public Override Get Properties

    /// <summary>
    /// Returns the array property NullDefaultedValue as an object. 
    /// </summary>
    public override object NullDefaultedObject { get { return NullDefaultedValue; } }

    /// <summary>
    /// Returns the array property Value as an object.
    /// </summary>
    public override object LegacyValue { get { return FValue; } }

    #endregion


    #region Public Methods

    /// <summary>
    /// Sets the parameter to null and assigns a category owner.
    /// </summary>
    /// <param name="AOwner">The category setting the parameter</param>
    public void SetValue(cCheckCategory AOwner)
    {
      if (SetValueCheckAndPrep(AOwner))
      {
        FValue = null;
        FNullDefaultedValue = null;
      }
    }

    /// <summary>
    /// Sets the parameter to null and leaves the category owner null, indicating the process
    /// set the parameter.
    /// </summary>
    public void SetValue()
    {
      cCheckCategory Category = null; //Needed to prevent ambigious call to SetValue

      SetValue(Category);
    }

    /// <summary>
    /// Sets the parameter to the passed array and assigns a category owner.
    /// </summary>
    /// <param name="AArray">The array to assign to the parameter.</param>
    /// <param name="AOwner">The category setting the parameter</param>
    public void SetValue(NullableType[] AArray, cCheckCategory AOwner)
    {
      if (SetValueCheckAndPrep(AOwner))
      {
        FValue = AArray;

        if (FValue == null)
          FNullDefaultedValue = null;
        else
        {
          FNullDefaultedValue = new Type[FValue.Length];

          for (int Dex = 0; Dex < FValue.Length; Dex++)
          {
            if (GetTypeValue(FValue[Dex]).Equals(GetDefaultForNull()))
              FValue[Dex] = GetNull();

            FNullDefaultedValue[Dex] = GetTypeValue(FValue[Dex]);
          }
        }
      }
    }

    /// <summary>
    /// Sets the parameter to the passed array and leaves the category owner null, indicating 
    /// the process set the parameter.
    /// </summary>
    /// <param name="AArray">The array to assign to the parameter.</param>
    public void SetValue(NullableType[] AArray)
    {
      SetValue(AArray, null);
    }

    /// <summary>
    /// Sets the parameter to an array of the specified length and null elements.
    /// </summary>
    /// <param name="ALength">The length of the resulting array.</param>
    /// <param name="AOwner">The category setting the parameter.</param>
    public void SetValue(int ALength, cCheckCategory AOwner)
    {
      if (SetValueCheckAndPrep(AOwner))
      {
        FValue = new NullableType[ALength];
        FNullDefaultedValue = new Type[ALength];

        for (int Dex = 0; Dex < FValue.Length; Dex++)
        {
          FValue[Dex] = GetNull();
          FNullDefaultedValue[Dex] = GetDefaultForNull();
        }
      }
    }

    /// <summary>
    /// Sets the parameter to an array of the specified length and null elements, 
    /// and leaves the category owner null, indicating the process set the parameter.
    /// </summary>
    /// <param name="ALength">The length of the resulting array.</param>
    public void SetValue(int ALength)
    {
      SetValue(ALength, null);
    }

    /// <summary>
    /// Sets the parameter to an array of the specified length and elements valued to AValue.
    /// </summary>
    /// <param name="ALength">The length of the resulting array.</param>
    /// <param name="AArray">The initial value of each array element.</param>
    /// <param name="AOwner">The category setting the parameter.</param>
    public void SetValue(int ALength, NullableType AArray, cCheckCategory AOwner)
    {
      if (SetValueCheckAndPrep(AOwner))
      {
        FValue = new NullableType[ALength];
        FNullDefaultedValue = new Type[ALength];

        Type NullDefaultedValue = GetTypeValue(AArray); ;

        for (int Dex = 0; Dex < FValue.Length; Dex++)
        {
          FValue[Dex] = AArray;
          FNullDefaultedValue[Dex] = NullDefaultedValue;
        }
      }
    }

    /// <summary>
    /// Sets the parameter to an array of the specified length and elements valued to AValue, 
    /// and leaves the category owner null, indicating the process set the parameter.
    /// </summary>
    /// <param name="ALength">The length of the resulting array.</param>
    /// <param name="AArray">The initial value of each array element.</param>
    public void SetValue(int ALength, NullableType AArray)
    {
      SetValue(ALength, AArray, null);
    }

    /// <summary>
    /// Replaces a current parameter array element with the passed value.
    /// </summary>
    /// <param name="AValue">The value that should replace the current parameter value array element.</param>
    /// <param name="AIndex">The current parameter array element index.</param>
    /// <param name="ACategory">The category updating the parameter</param>
    public void UpdateValue(NullableType AValue, int AIndex, cCheckCategory ACategory)
    {
      if (UpdateValueCheck(ACategory))
      {
        FValue[AIndex] = AValue;
      }
    }

    /// <summary>
    /// Replaces a current parameter array element with the passed value.
    /// </summary>
    /// <param name="AArray">The value that should replace the current parameter value array element.</param>
    /// <param name="ACategory">The category updating the parameter</param>
    public void UpdateValue(NullableType[] AArray, cCheckCategory ACategory)
    {
      if (UpdateValueCheck(ACategory))
      {
        FValue = AArray;
      }
    }

    #endregion


    #region Public Override Methods

    /// <summary>
    /// This method sets the value of the parameter using an object value.
    /// If the object value is not of a correct type the method returns a false.
    /// </summary>
    /// <param name="ALegacyValue">The object value by which to set the value.</param>
    /// <param name="AOwner">The category setting the parameter</param>
    /// <returns>True if the setting was successful.</returns>
    public override bool LegacySetValue(object ALegacyValue, cCheckCategory AOwner)
    {
      try
      {
        if (SetValueCheckAndPrep(AOwner))
        {
          string ErrorMessage = "";

          if (ConvertObjectType(ALegacyValue, ref FValue, ref ErrorMessage))
            return true;
          else
          {
            if (AOwner != null)
              System.Diagnostics.Debug.WriteLine(string.Format("Error legacy setting of value: Category - {0}, Parameter - {1} : {2}", AOwner.CategoryCd, this.Name, ErrorMessage));
            else
              System.Diagnostics.Debug.WriteLine(string.Format("Error legacy setting of value: For Process, Parameter - {0} : {1}", this.Name, ErrorMessage));

            return false;
          }
        }
        else
          return false;
      }
      catch (Exception ex)
      {
        if (AOwner != null)
          System.Diagnostics.Debug.WriteLine(string.Format("Error legacy setting of value: Category - {0}, Parameter - {1} : {2}", AOwner.CategoryCd, this.Name, ex.Message));
        else
          System.Diagnostics.Debug.WriteLine(string.Format("Error legacy setting of value: For Process, Parameter - {0} : {1}", this.Name, ex.Message));

        return false;
      }
    }

    /// <summary>
    /// This method updates the value of the parameter using an object value.
    /// If the object value is not of a correct type the method returns a false.
    /// </summary>
    /// <param name="ALegacyValue">The object value by which to set the value.</param>
    /// <param name="ACategory">The category setting the parameter</param>
    /// <returns>True if the setting was successful.</returns>
    public override bool LegacyUpdateValue(object ALegacyValue, cCheckCategory ACategory)
    {
      try
      {
        if (UpdateValueCheck(ACategory))
        {
          string ErrorMessage = "";

          if (ConvertObjectType(ALegacyValue, ref FValue, ref ErrorMessage))
            return true;
          else
          {
            if (ACategory != null)
              System.Diagnostics.Debug.WriteLine(string.Format("Error legacy setting of value: Category - {0}, Parameter - {1} : {2}", ACategory.CategoryCd, this.Name, ErrorMessage));
            else
              System.Diagnostics.Debug.WriteLine(string.Format("Error legacy setting of value: For Process, Parameter - {0} : {1}", this.Name, ErrorMessage));

            return false;
          }
        }
        else
          return false;
      }
      catch (Exception ex)
      {
        if (ACategory != null)
          System.Diagnostics.Debug.WriteLine(string.Format("Error legacy setting of value: Category - {0}, Parameter - {1} : {2}", ACategory.CategoryCd, this.Name, ex.Message));
        else
          System.Diagnostics.Debug.WriteLine(string.Format("Error legacy setting of value: For Process, Parameter - {0} : {1}", this.Name, ex.Message));

        return false;
      }
    }

    /// <summary>
    /// Returns the current value of the parameter as an object.
    /// </summary>
    /// <returns>The current value of the parameter as an object.</returns>
    public override object ValueAsObject()
    {
      return Value;
    }

    #endregion


    #region Protected Abtract Methods

    /// <summary>
    /// Converts the input object to the nullable type of the parameter.
    /// </summary>
    /// <param name="AInputValue">The object to convert.</param>
    /// <param name="AOutputValue">The converted value.</param>
    /// <param name="AErrorMessage">Set to the exception message if the conversion fails.</param>
    /// <returns>Returns false if the conversion failed.</returns>
    protected abstract bool ConvertObjectType(object AInputValue, ref NullableType[] AOutputValue, ref string AErrorMessage);

    #endregion

  }
}
