using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ECMPS.Checks.Parameters
{
  /// <summary>
  /// The parent type of all type parameters used by the check engine, that represent a
  /// non referenced non array value.
  /// </summary>
  /// <typeparam name="Type">The base type of the parameter value</typeparam>
  /// <typeparam name="NullableType">The nullable type of the parameter value</typeparam>
  public abstract class cCheckParameterAbstractValue<Type, NullableType> : cCheckParameterCheckEngineTyped<Type, NullableType>
  {

    #region Protected Constructors

    /// <summary>
    /// Creates an instance of cCheckParameterAbstractValue and sets the Name and DataType property.
    /// </summary>
    /// <param name="AParameterKey">The key of the parameter.</param>
    /// <param name="AParameterName">The name of the parameter.</param>
    /// <param name="AParameterDataType">The data type of the parameter.</param>
    /// <param name="ACheckParameters">The parent check parameters object.</param>
    protected cCheckParameterAbstractValue(int? AParameterKey, string AParameterName, eParameterDataType AParameterDataType,
                                           cCheckParameters ACheckParameters)
      : base(AParameterKey, AParameterName, AParameterDataType, ACheckParameters)
    {
      FIsArray = false;
    }

    #endregion


    #region Public Virtual Get Properties

    #region Property Fields

    /// <summary>
    /// The value as the base type and defaulted when null or dbnull.
    /// </summary>
    protected Type FNullDefaultedValue;

    /// <summary>
    /// The value with elements as a nullable type.
    /// </summary>
    protected NullableType FValue;

    #endregion


    /// <summary>
    /// The value as the base type and defaulted when null or dbnull.
    /// </summary>
    public virtual Type NullDefaultedValue { get { return FNullDefaultedValue; } }

    /// <summary>
    /// The value with elements as a nullable type.
    /// </summary>
    public virtual NullableType Value { get { return (IsSet ? FValue : GetNull()); } }

    #endregion


    #region Public Override Get Properties

    /// <summary>
    /// Returns property NullDefaultedValue as an object.
    /// </summary>
    public override object NullDefaultedObject { get { return NullDefaultedValue; } }

    /// <summary>
    /// Returns property Value as an object.
    /// </summary>
    public override object LegacyValue { get { return GetTypeValue(Value); } }

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


    #region Public Methods

    /// <summary>
    /// Sets the parameter to null and assigns a category owner.
    /// </summary>
    /// <param name="AOwner">The category setting the parameter</param>
    public void SetValue(cCheckCategory AOwner)
    {
      if (SetValueCheckAndPrep(AOwner))
      {
        FValue = GetNull();
        FNullDefaultedValue = GetDefaultForNull();
      }
    }

    /// <summary>
    /// Sets the parameter to null and leaves the category owner null, indicating the process
    /// set the parameter.
    /// </summary>
    public void SetValue()
    {
      FOwner = null;

      SetValue(null);
    }

    /// <summary>
    /// Sets the parameter to the passed value and assigns a category owner.
    /// </summary>
    /// <param name="AValue">The value to assign to the parameter.</param>
    /// <param name="AOwner">The category setting the parameter</param>
    public void SetValue(NullableType AValue, cCheckCategory AOwner)
    {
      if (SetValueCheckAndPrep(AOwner))
      {
        if (AValue == null)
        {
          FValue = GetNull();
        }
        else
        {
          FValue = AValue;
          FNullDefaultedValue = GetTypeValue(FValue);
        }
      }
    }

    /// <summary>
    /// Sets the parameter to the passed value and leaves the category owner null, indicating 
    /// the process set the parameter.
    /// </summary>
    /// <param name="AValue">The value to assign to the parameter.</param>
    public void SetValue(NullableType AValue)
    {
      FOwner = null;

      SetValue(AValue, null);
    }

    /// <summary>
    /// Replaces a current parameter value with the passed value.
    /// </summary>
    /// <param name="AValue">The value that should replace the current parameter value array element.</param>
    /// <param name="ACategory">The category updating the parameter</param>
    public void UpdateValue(NullableType AValue, cCheckCategory ACategory)
    {
      if (UpdateValueCheck(ACategory))
      {
        FValue = AValue;
      }
      else
      {
        if (FOwner != null)
          System.Diagnostics.Debug.WriteLine(string.Format("Update of uninitialized parameters is not allowed: Category - {0}, Parameter - {1}", FOwner.CategoryCd, this.Name));
        else
          System.Diagnostics.Debug.WriteLine(string.Format("Update of uninitialized parameters is not allowed: Parameter - {0}", this.Name));
      }
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
    protected abstract bool ConvertObjectType(object AInputValue, ref NullableType AOutputValue, ref string AErrorMessage);

    #endregion

  }
}
