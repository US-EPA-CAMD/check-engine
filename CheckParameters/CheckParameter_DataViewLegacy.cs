using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using ECMPS.Checks.TypeUtilities;

namespace ECMPS.Checks.Parameters
{

  /// <summary>
  /// Implements the data view parameter for generic data.
  /// </summary>
  public class cCheckParameterDataViewLegacy : cCheckParameterCheckEngineTyped<DataView, DataView>
  {

    #region Public Constructors

    /// <summary>
    /// Creates and instance of a cCheckParameterDataViewLegacy object, setting the array property 
    /// to false, and the parameter name and types.
    /// </summary>
    /// <param name="AParameterKey">The key of the parameter.</param>
    /// <param name="AParameterName">The name of the parameter.</param>
    /// <param name="ACheckParameters">The parent check parameters object.</param>
    public cCheckParameterDataViewLegacy(int? AParameterKey, string AParameterName,
                                         cCheckParameters ACheckParameters)
      : base(AParameterKey, AParameterName, eParameterDataType.Object, ACheckParameters)
    {
      FIsArray = false;
    }

    #endregion


    #region Protected Fields

    /// <summary>
    /// The data view assigned to the parameter.
    /// </summary>
    protected DataView FDataView = null;

    #endregion


    #region Public Virtual Get Properties

    /// <summary>
    /// Returns the value since the base value type (DataView) excepts nulls.
    /// </summary>
    public virtual DataView NullDefaultedValue { get { return Value; } }

    /// <summary>
    /// Returns the default view of the Scoped Table 
    /// </summary>
    public virtual DataView Value { get { return ((IsSet && (FDataView != null)) ? FDataView : GetNull()); } }

    #endregion


    #region Public Override Get Properties

    /// <summary>
    /// Returns NullDefaultedValue as an object.
    /// </summary>
    public override object NullDefaultedObject { get { return NullDefaultedValue; } }

    /// <summary>
    /// Returns Value as an object.
    /// </summary>
    public override object LegacyValue { get { return Value; } }

    #endregion


    #region Public Virtual Methods

    /// <summary>
    /// Sets the parameter value.
    /// </summary>
    /// <param name="AValue">The dataview to which the parameter should be set.</param>
    /// <param name="AOwner">The check category making the request.</param>
    public virtual void InitValue(DataView AValue, cCheckCategory AOwner)
    {
      if (SetValueCheckAndPrep(AOwner))
      {
        FDataView = AValue;
      }
    }

    /// <summary>
    /// Sets the parameter value.
    /// </summary>
    /// <param name="AValue">The dataview to which the parameter should be set.</param>
    public virtual void InitValue(DataView AValue)
    {
      InitValue(AValue, (cCheckCategory)null);
    }

    /// <summary>
    /// Nulls the parameter value.
    /// </summary>
    /// <param name="AOwner">The check category making the request.</param>
    public virtual void InitValue(cCheckCategory AOwner)
    {
      if (SetValueCheckAndPrep(AOwner))
      {
        FDataView = null;
      }
    }

    /// <summary>
    /// Nulls the parameter value.
    /// </summary>
    public virtual void InitValue()
    {
      InitValue((cCheckCategory)null);
    }

    #endregion


    #region Public Override Methods

    /// <summary>
    /// Returns this parameter's default DataView value for nulls.
    /// </summary>
    /// <returns>Returns the default DataView value for nulls.</returns>
    public override DataView GetDefaultForNull()
    {
      return null;
    }

    /// <summary>
    /// Retursn this parameter's default DataView value for nulls as an object.
    /// </summary>
    /// <returns>Returns the default DataView value for nulls as an object.</returns>
    public override object GetDefaultObjectForNull()
    {
      return GetDefaultForNull();
    }

    /// <summary>
    /// Returns null typed as a DataView.
    /// </summary>
    /// <returns>Returns null as a DataView.</returns>
    public override DataView GetNull()
    {
      return null;
    }

    /// <summary>
    /// Returns the passed value if it is not null, otherwise it returns the default DataView for nulls.
    /// </summary>
    /// <param name="ANullableValue">The value to convert.</param>
    /// <returns>The converted value.</returns>
    public override DataView GetTypeValue(DataView ANullableValue)
    {
      return ANullableValue;
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
          if (ALegacyValue == null)
          {
            FDataView = null;
            return true;
          }
          else if (typeof(DataView).IsAssignableFrom(ALegacyValue.GetType()))
          {
            FDataView = (DataView)ALegacyValue;
            return true;
          }
          else
          {
            string ErrorMessage = "Cannot convert type '" + ALegacyValue.GetType().Name + "' to 'DateTime'";

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
          if (ALegacyValue == null)
          {
            FDataView = null;
            return true;
          }
          else if (typeof(DataView).IsAssignableFrom(ALegacyValue.GetType()))
          {
            FDataView = (DataView)ALegacyValue;
            return true;
          }
          else
          {
            string ErrorMessage = "Cannot convert type '" + ALegacyValue.GetType().Name + "' to 'DateTime'";

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


    #region Protected Override Methods

    /// <summary>
    /// Sets the value property field to null, typed as a DataView.
    /// </summary>
    protected override void ResetValue()
    {
      FDataView = null;
    }

    #endregion

  }

}
