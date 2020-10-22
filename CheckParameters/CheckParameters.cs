using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

using ECMPS.Checks.DatabaseAccess;

namespace ECMPS.Checks.Parameters
{
  /// <summary>
  /// An abstract class the contains the collection of parameters for a process.
  /// </summary>
  public abstract class cCheckParameters
  {

    #region Protected Constructors

    /// <summary>
    /// Creates and instance of a cCheckParameters object
    /// </summary>
    protected cCheckParameters(cCheckProcess ACheckProcess, cDatabase ADatabaseAux)
    {
      FCheckProcess = ACheckProcess;
      FDatabaseAux = ADatabaseAux;

      InstantiateCheckParameterProperties();
      InitializeCheckParameterList();
    }


    /// <summary>
    /// Constructor used for testing.
    /// </summary>
    public cCheckParameters()
    {
    }

    #endregion


    #region Protected Fields

    /// <summary>
    /// A dictionary of check parameters contained in this check parameters object.
    /// </summary>
    protected cCheckParameterCollection FCheckParameterCollection = new cCheckParameterCollection();

    /// <summary>
    /// The AUX database connection object.
    /// </summary>
    protected cDatabase FDatabaseAux;

    #endregion


    #region Public Properties

    #region Property Fields

    private cCheckProcess FCheckProcess;

    #endregion


    /// <summary>
    /// A dictionary of check parameters contained in this check parameters object.
    /// </summary>
    public Dictionary<string, cCheckParameter>.ValueCollection CheckParameterList { get { return FCheckParameterCollection.Values; } }

    /// <summary>
    /// The check process associated with the check parameters
    /// </summary>
    public cCheckProcess CheckProcess { get { return FCheckProcess; } }

    /// <summary>
    /// The process code of the check process assiciated with the check parameters
    /// </summary>
    public string ProcessCd { get { return (FCheckProcess == null) ? null : FCheckProcess.ProcessCd; } }

    #endregion


    #region Public Methods

    /// <summary>
    /// Determines whether a parameter by name exists in the check parameters object.
    /// </summary>
    /// <param name="AParameterName">The parameter name for the parameter in CHECK_PARAM_ID_NAME </param>
    /// <returns>True if the parameter exists.</returns>
    public bool ContainsCheckParameter(string AParameterName)
    {
      return FCheckParameterCollection.ContainsKey(AParameterName.Trim().ToUpper());
    }

    /// <summary>
    /// Gets the check parameter corresponding to the passed parameter name.
    /// </summary>
    /// <param name="AParameterName">The parameter name for the parameter in CHECK_PARAM_ID_NAME </param>
    /// <returns>The parameter matching the parameter name if found, otherwise null.</returns>
    public cCheckParameter GetCheckParameter(string AParameterName)
    {
      cCheckParameter Result;

      if (!FCheckParameterCollection.TryGetValue(AParameterName.Trim().ToUpper(), out Result))
        Result = null;

      return Result;
    }

    /// <summary>
    /// Resets the value of each parameter for the passed owner, resetting all parameters
    /// if the owner is null.
    /// </summary>
    /// <param name="AOwner">The owner of check parameters that should be reset.</param>
    public void Reset(cCheckCategory AOwner)
    {
      foreach (cCheckParameter CheckParameter in FCheckParameterCollection.Values)
      {
        if ((AOwner == null) || 
            ((CheckParameter.Owner != null) && (CheckParameter.Owner.CategoryCd == AOwner.CategoryCd)))
          CheckParameter.Reset();
      }
    }
    
    /// <summary>
    /// Resets the value of all parameters.
    /// </summary>
    public void Reset()
    {
      Reset(null);
    }

    #endregion


    #region Protected Abstract Methods

    /// <summary>
    /// This method should instantiate each of the check parameter properties implemented in
    /// the child check parameters objects.
    /// </summary>
    protected abstract void InstantiateCheckParameterProperties();

    #endregion


    #region Protected Virtual Methods

    /// <summary>
    /// Populates the dictionary of check parameters used to lookup check parameters by name.
    /// </summary>
    protected virtual void InitializeCheckParameterList()
    {
      FCheckParameterCollection.Clear();

      PropertyInfo[] CheckParameterProperties = this.GetType().GetProperties();

      foreach (PropertyInfo CheckParameterProperty in CheckParameterProperties)
      {
        if (CheckParameterProperty.PropertyType.IsSubclassOf(typeof(cCheckParameter)))
        {
          cCheckParameter CheckParameter = (cCheckParameter)CheckParameterProperty.GetValue(this, null);

          if (CheckParameter != null)
          {
            FCheckParameterCollection.Add(CheckParameter.Name.Trim().ToUpper(), CheckParameter);
          }
        }
      }
    }

    #endregion


    #region Protected Methods

    /// <summary>
    /// Adds a check parameter to the dictionary of check parameters used to lookup check parameters by name.
    /// </summary>
    /// <param name="AParameterName">The name of the parameter.</param>
    /// <param name="ACheckParameter">The check parameter.</param>
    /// <param name="AErrorMessage">The error message returned if the result is false.</param>
    /// <returns>Returns true unless the parameter already exists or an error occurred during the update.</returns>
    protected bool AddCheckParameter(string AParameterName, cCheckParameter ACheckParameter, 
                                     ref string AErrorMessage)
    {
        if (!FCheckParameterCollection.ContainsKey(AParameterName.Trim().ToUpper()))
      {
        FCheckParameterCollection.Add(AParameterName.Trim().ToUpper(), ACheckParameter);
        return true;
      }
      else
      {
        AErrorMessage = "[cCheckParameters.AddCheckParameter]: Parameter already exists in parameter list.";
        return false;
      }
    }

    #endregion

  }
}
