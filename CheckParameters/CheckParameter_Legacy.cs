using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ECMPS.Checks.Parameters
{
  /// <summary>
  /// The root class for all parameters.  The class is abstract and cannot be instantiated.
  /// </summary>
  public class cCheckParameterLegacy : cCheckParameterCheckEngine
  {

    #region Public Constructors

    /// <summary>
    /// Instantiates a cCheckParameterCheckEngine class and sets the name and data type properties
    /// of the parameter
    /// </summary>
    /// <param name="AParameterKey">The key of the parameter.</param>
    /// <param name="AParameterName">The name of the parameter.</param>
    /// <param name="ACheckParameters">The parent check parameters object.</param>
    public cCheckParameterLegacy(int? AParameterKey, string AParameterName, cCheckParameters ACheckParameters)
      : base(AParameterKey, AParameterName, eParameterDataType.Object, ACheckParameters)
    {
		ResetValue();
    }

    #endregion


    #region Public Virtual Properties: Get

    #region Property Fields

    /// <summary>
    /// The value as the base type and defaulted when null or dbnull.
    /// </summary>
    protected Type FNullDefaultedValue;

    /// <summary>
    /// The legacy value.
    /// </summary>
    protected object FLegacyValue;

    #endregion


    /// <summary>
    /// The value as the base type and defaulted when null or dbnull.
    /// </summary>
    public virtual Type NullDefaultedValue { get { return FNullDefaultedValue; } }

    #endregion


    #region Public Override Properties: Get

    /// <summary>
    /// Indicates whether this parameter type is used to bypass the implementation of the
    /// underlying parameter as a property of the check parameters object.
    /// </summary>
    public override bool IsLegacyType { get { return true; } }

    /// <summary>
    /// Returns property Value as an object.
    /// </summary>
    public override object LegacyValue { get { return (IsSet ? FLegacyValue : null); } }

    /// <summary>
    /// Returns property NullDefaultedValue as an object.
    /// </summary>
    public override object NullDefaultedObject { get { return NullDefaultedValue; } }

    #endregion


    #region Public Override Methods

    /// <summary>
    /// Retursn this parameter's default bool value for nulls as an object.
    /// </summary>
    /// <returns>Returns the default bool value for nulls as an object.</returns>
    public override object GetDefaultObjectForNull()
    {
      return null;
    }

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
          FLegacyValue = ALegacyValue;
          SetLegacyTypeInformation(FLegacyValue);

          return true;
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
    /// Replaces a current parameter value with the passed value.
    /// </summary>
    /// <param name="ALegacyValue">The value that should replace the current parameter value array element.</param>
    /// <param name="ACategory">The category updating the parameter</param>
    public override bool LegacyUpdateValue(object ALegacyValue, cCheckCategory ACategory)
    {
      try
      {
        if (UpdateValueCheck(ACategory))
        {
          bool IsArray;
          eParameterDataType ParameterDataType;

          GetLegacyTypeInformation(ALegacyValue, out IsArray, out ParameterDataType);

          if ((IsArray == FIsArray) && (ParameterDataType == FDataType))
          {
            FLegacyValue = ALegacyValue;
            return true;
          }
          else
          {
            if (FOwner != null)
              System.Diagnostics.Debug.WriteLine(string.Format("Error legacy updating of value: Category - {0}, Parameter - {1} : {2}", ACategory.CategoryCd, this.Name, "Cannot update a parameter of a different type"));
            else
              System.Diagnostics.Debug.WriteLine(string.Format("Error legacy updating of value: Parameter - {0} : {1}", this.Name, "Cannot update a parameter of a different type"));

            return false;
          }
        }
        else
          return false;
      }
      catch (Exception ex)
      {
        if (FOwner != null)
          System.Diagnostics.Debug.WriteLine(string.Format("Error legacy updating of value: Category - {0}, Parameter - {1} : {2}", FOwner.CategoryCd, this.Name, ex.Message));
        else
          System.Diagnostics.Debug.WriteLine(string.Format("Error legacy updating of value: For Process, Parameter - {0} : {1}", this.Name, ex.Message));

        return false;
      }
    }

    /// <summary>
    /// Returns the current value of the parameter as an object.
    /// </summary>
    /// <returns>The current value of the parameter as an object.</returns>
    public override object ValueAsObject()
    {
      return LegacyValue;
    }

    #endregion


    #region Protected Override Methods

    /// <summary>
    /// Sets the value property field to null, typed as a nullable bool.
    /// </summary>
    protected override void ResetValue()
    {
      FLegacyValue = null;
    }

    #endregion


    #region Private Methods

    private void GetLegacyTypeInformation(object ALegacyValue, 
                                          out bool AIsArray, 
                                          out eParameterDataType AParameterDataType)
    {
      if (ALegacyValue != null)
      {
        Type LegacyValueType = ALegacyValue.GetType();

        // Set IsArray property
        AIsArray = LegacyValueType.IsArray;

        // Adjust Value Type if passed balue is an array.
        if (AIsArray)
          LegacyValueType = LegacyValueType.GetElementType();

        // Set DataType property
        if (LegacyValueType.Name == "Boolean")
          AParameterDataType = eParameterDataType.Boolean;
        else if (LegacyValueType.Name == "DataRowView")
          AParameterDataType = eParameterDataType.DataRowView;
        else if (LegacyValueType.Name == "DataView")
          AParameterDataType = eParameterDataType.DataView;
        else if (LegacyValueType.Name == "DateTime")
          AParameterDataType = eParameterDataType.Date;
        else if (LegacyValueType.Name == "Decimal")
          AParameterDataType = eParameterDataType.Decimal;
        else if (LegacyValueType.Name == "Int32")
          AParameterDataType = eParameterDataType.Integer;
        else if (LegacyValueType.Name == "String")
          AParameterDataType = eParameterDataType.String;
        else
          AParameterDataType = eParameterDataType.Object;
      }
      else
      {
        AIsArray = false;
        AParameterDataType = eParameterDataType.Object;
      }
    }

    private void SetLegacyTypeInformation(object ALegacyValue)
    {
      GetLegacyTypeInformation(ALegacyValue, out FIsArray, out FDataType);
    }

    #endregion

  }
}
