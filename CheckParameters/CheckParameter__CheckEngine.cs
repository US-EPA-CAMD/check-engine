using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ECMPS.Checks.Parameters
{
  /// <summary>
  /// The root class for all parameters.  The class is abstract and cannot be instantiated.
  /// </summary>
  public abstract class cCheckParameterCheckEngine : cCheckParameter
  {

    #region Protected Constructors

    /// <summary>
    /// Instantiates a cCheckParameterCheckEngine class and sets the name and data type properties
    /// of the parameter
    /// </summary>
    /// <param name="AParameterKey">The key of the parameter.</param>
    /// <param name="AParameterName">The name of the parameter.</param>
    /// <param name="AParameterDataType">The data type of the parameter.</param>
    /// <param name="ACheckParameters">The parent check parameters object.</param>
    protected cCheckParameterCheckEngine(int? AParameterKey, string AParameterName, eParameterDataType AParameterDataType,
                                         cCheckParameters ACheckParameters)
      : base(AParameterKey, AParameterName, AParameterDataType, ACheckParameters)
    {
    }

    #endregion


    #region Public Virtual Properties

    /// <summary>
    /// Indicates whether this parameter type is used to bypass the implementation of the
    /// underlying parameter as a property of the check parameters object.
    /// </summary>
    public virtual bool IsLegacyType { get { return false; } }

    #endregion


    #region Public Methods

    /// <summary>
    /// This method sets the value of the parameter using an object value.
    /// If the object value is not of a correct type the method returns a false.
    /// </summary>
    /// <param name="ALegacyValue">The object value by which to set the value.</param>
    /// <returns>True if the setting was successful.</returns>
    public bool LegacySetValue(object ALegacyValue)
    {
      return LegacySetValue(ALegacyValue, null);
    }

    /// <summary>
    /// This method sets the value of the parameter using an object value.
    /// If the object value is not of a correct type the method returns a false.
    /// </summary>
    /// <param name="ALegacyValue">The object value by which to set the value.</param>
    /// <returns>True if the setting was successful.</returns>
    public bool LegacyUpdateValue(object ALegacyValue)
    {
      return LegacyUpdateValue(ALegacyValue, null);
    }

    #endregion


    #region Public Abstract Methods

    /// <summary>
    /// This method sets the value of the parameter using an object value.
    /// If the object value is not of a correct type the method returns a false.
    /// </summary>
    /// <param name="ALegacyValue">The object value by which to set the value.</param>
    /// <param name="AOwner">The category setting the parameter</param>
    /// <returns>True if the setting was successful.</returns>
    public abstract bool LegacySetValue(object ALegacyValue, cCheckCategory AOwner);

    /// <summary>
    /// This method updates the value of the parameter using an object value.
    /// If the object value is not of a correct type the method returns a false.
    /// </summary>
    /// <param name="ALegacyValue">The object value by which to set the value.</param>
    /// <param name="AOwner">The category setting the parameter</param>
    /// <returns>True if the setting was successful.</returns>
    public abstract bool LegacyUpdateValue(object ALegacyValue, cCheckCategory AOwner);

    #endregion

  }
}
