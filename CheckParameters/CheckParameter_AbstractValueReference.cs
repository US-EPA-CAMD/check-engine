using System;
using System.Collections.Generic;
using System.Data;
using System.Text;


namespace ECMPS.Checks.Parameters
{
  /// <summary>
  /// The parent type of all type parameters used by the check engine, that represent a
  /// referenced value.
  /// </summary>
  /// <typeparam name="Type">The base type of the parameter value</typeparam>
  /// <typeparam name="NullableType">The nullable type of the parameter value</typeparam>
  public abstract class cCheckParameterAbstractValueReference<Type, NullableType> : cCheckParameterCheckEngineTyped<Type, NullableType>
  {

    #region Protected Constructors

    /// <summary>
    /// Creates an instance of cCheckParameterAbstractValueReference and sets the Name and DataType property.
    /// </summary>
    /// <param name="AParameterKey">The key of the parameter.</param>
    /// <param name="AParameterName">The name of the parameter.</param>
    /// <param name="AParameterDataType">The data type of the parameter.</param>
    /// <param name="ACheckParameters">The parent check parameters object.</param>
    protected cCheckParameterAbstractValueReference(int? AParameterKey, string AParameterName, eParameterDataType AParameterDataType,
                                                    cCheckParameters ACheckParameters)
      : base(AParameterKey, AParameterName, AParameterDataType, ACheckParameters)
    {
      FIsArray = false;
    }

    #endregion


    #region Public Virtual Get Properties

    #region Property Fields

    /// <summary>
    /// The parameter referenced by this parameter.
    /// </summary>
    protected cCheckParameterAbstractValue<Type, NullableType> FReferencedParameter;

    #endregion

    /// <summary>
    /// The NullDefaultedValue of the referenced parameter, when set,
    /// otherwise the null defaulted value.
    /// </summary>
    public virtual Type NullDefaultedValue
    {
      get
      {
        if (IsSet && (FReferencedParameter != null))
          return FReferencedParameter.NullDefaultedValue;
        else
          return GetDefaultForNull();
      }
    }

    /// <summary>
    /// The parameter referenced by this parameter.
    /// </summary>
    public virtual cCheckParameterAbstractValue<Type, NullableType> ReferencedParameter { get { return (IsSet ? FReferencedParameter : null); } }

    /// <summary>
    /// The Value of the referenced parameter, when set, otherwise null.
    /// </summary>
    public virtual NullableType Value 
    { 
      get
      {
        if (IsSet && (FReferencedParameter != null))
          return FReferencedParameter.Value;
        else
          return GetNull();
      }
    }

    #endregion


    #region Public Override Get Properties

    /// <summary>
    /// Returns the property NullDefaultedValue as an object.
    /// </summary>
    public override object NullDefaultedObject { get { return NullDefaultedValue; } }

    /// <summary>
    /// Returns the property Value as an object.
    /// </summary>
    public override object LegacyValue { get { return Value; } }

    #endregion


    #region Public Methods

    /// <summary>
    /// Sets the parameter reference to the passed value and assigns a category owner.
    /// </summary>
    /// <param name="AParameter">The parameter to reference.</param>
    /// <param name="AOwner">The category setting the parameter</param>
    public void SetValue(cCheckParameterAbstractValue<Type, NullableType> AParameter, cCheckCategory AOwner)
    {
      if (SetValueCheckAndPrep(AOwner))
        FReferencedParameter = AParameter;
    }

    /// <summary>
    /// Sets the parameter reference to the passed value and leaves the category owner 
    /// null, indicating the process set the parameter.
    /// </summary>
    /// <param name="AParameter">The parameter to reference.</param>
    public void SetValue(cCheckParameterAbstractValue<Type, NullableType> AParameter)
    {
      SetValue(AParameter, null);
    }

    #endregion


    #region Public Override Methods

    /// <summary>
    /// Returns the current value of the parameter as an object.
    /// </summary>
    /// <returns>The current value of the parameter as an object.</returns>
    public override object ValueAsObject()
    {
      return Value;
    }

    #endregion


    #region Protected Override Methods

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
          if (typeof(string).IsAssignableFrom(ALegacyValue.GetType()))
          {
            string ReferenceParameterName = Convert.ToString(ALegacyValue);

            if (FCheckParameters.ContainsCheckParameter(ReferenceParameterName))
            {
              cCheckParameter CheckParameter = FCheckParameters.GetCheckParameter(ReferenceParameterName);

              if (FReferencedParameter.GetType().IsAssignableFrom(CheckParameter.GetType()))
              {
                FReferencedParameter = (cCheckParameterAbstractValue<Type, NullableType>)CheckParameter;
                return true;
              }
              else
              {
                string ErrorMessage = "[" + this.Name + ".ConvertObjectType]: Cannot assign parameter of type '" + CheckParameter.GetType().Name + "' to reference of typ '" + typeof(cCheckParameterAbstractValue<Type, NullableType>) + "'";

                if (AOwner != null)
                  System.Diagnostics.Debug.WriteLine(string.Format("Error legacy setting of value: Category - {0}, Parameter - {1} : {2}", AOwner.CategoryCd, this.Name, "Referenced Parameter does not exist"));
                else
                  System.Diagnostics.Debug.WriteLine(string.Format("Error legacy setting of value: For Process, Parameter - {0} : {1}", this.Name, "Referenced Parameter does not exist"));

                return false;
              }
            }
            else
            {
              if (AOwner != null)
                System.Diagnostics.Debug.WriteLine(string.Format("Error legacy setting of value: Category - {0}, Parameter - {1} : {2}", AOwner.CategoryCd, this.Name, "Referenced Parameter does not exist"));
              else
                System.Diagnostics.Debug.WriteLine(string.Format("Error legacy setting of value: For Process, Parameter - {0} : {1}", this.Name, "Referenced Parameter does not exist"));

              return false;
            }
          }
          else
          {
            string ErrorMessage = "[" + this.Name + ".ConvertObjectType]: Cannot convert type '" + ALegacyValue.GetType().Name + "' to 'string'";

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
          if (typeof(string).IsAssignableFrom(ALegacyValue.GetType()))
          {
            string ReferenceParameterName = Convert.ToString(ALegacyValue);

            if (FCheckParameters.ContainsCheckParameter(ReferenceParameterName))
            {
              cCheckParameter CheckParameter = FCheckParameters.GetCheckParameter(ReferenceParameterName);

              if (FReferencedParameter.GetType().IsAssignableFrom(CheckParameter.GetType()))
              {
                FReferencedParameter = (cCheckParameterAbstractValue<Type, NullableType>)CheckParameter;
                return true;
              }
              else
              {
                string ErrorMessage = "[" + this.Name + ".ConvertObjectType]: Cannot assign parameter of type '" + CheckParameter.GetType().Name + "' to reference of typ '" + typeof(cCheckParameterAbstractValue<Type, NullableType>) + "'";

                if (ACategory != null)
                  System.Diagnostics.Debug.WriteLine(string.Format("Error legacy updating of value: Category - {0}, Parameter - {1} : {2}", ACategory.CategoryCd, this.Name, "Referenced Parameter does not exist"));
                else
                  System.Diagnostics.Debug.WriteLine(string.Format("Error legacy updating of value: For Process, Parameter - {0} : {1}", this.Name, "Referenced Parameter does not exist"));

                return false;
              }
            }
            else
            {
              if (ACategory != null)
                System.Diagnostics.Debug.WriteLine(string.Format("Error legacy updating of value: Category - {0}, Parameter - {1} : {2}", ACategory.CategoryCd, this.Name, "Referenced Parameter does not exist"));
              else
                System.Diagnostics.Debug.WriteLine(string.Format("Error legacy updating of value: For Process, Parameter - {0} : {1}", this.Name, "Referenced Parameter does not exist"));

              return false;
            }
          }
          else
          {
            string ErrorMessage = "[" + this.Name + ".ConvertObjectType]: Cannot convert type '" + ALegacyValue.GetType().Name + "' to 'string'";

            if (ACategory != null)
              System.Diagnostics.Debug.WriteLine(string.Format("Error legacy updating of value: Category - {0}, Parameter - {1} : {2}", ACategory.CategoryCd, this.Name, ErrorMessage));
            else
              System.Diagnostics.Debug.WriteLine(string.Format("Error legacy updating of value: For Process, Parameter - {0} : {1}", this.Name, ErrorMessage));

            return false;
          }
        }
        else
          return false;
      }
      catch (Exception ex)
      {
        if (ACategory != null)
          System.Diagnostics.Debug.WriteLine(string.Format("Error legacy updating of value: Category - {0}, Parameter - {1} : {2}", ACategory.CategoryCd, this.Name, ex.Message));
        else
          System.Diagnostics.Debug.WriteLine(string.Format("Error legacy updating of value: For Process, Parameter - {0} : {1}", this.Name, ex.Message));

        return false;
      }
    }

    /// <summary>
    /// Sets the ReferencedParameter property to null.
    /// </summary>
    protected override void ResetValue()
    {
      if (IsSet)
      {
        FReferencedParameter = null;
      }
    }

    #endregion

  }
}
