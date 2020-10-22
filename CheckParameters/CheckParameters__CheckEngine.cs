using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using ECMPS.Checks.DatabaseAccess;
using ECMPS.Checks.TypeUtilities;

namespace ECMPS.Checks.Parameters
{

  /// <summary>
  /// An abstract class the contains the collection of parameters for a process for
  /// all check engine check parameters classes.
  /// </summary>
  public abstract class cCheckParametersCheckEngine : cCheckParameters
  {
 
    #region Protected Constructors

    /// <summary>
    /// Creates and instance of a cCheckParameters__CheckEngine object
    /// </summary>
    /// <param name="ACheckProcess">The check process associated with the check parameters.</param>
    /// <param name="ADatabaseAux">The database object for the AUX schema.</param>
    protected cCheckParametersCheckEngine(cCheckProcess ACheckProcess, cDatabase ADatabaseAux)
      : base(ACheckProcess, ADatabaseAux)
    {
    }


    /// <summary>
    /// Constructor used for testing.
    /// </summary>
    public cCheckParametersCheckEngine()
    {
      InstantiateCheckParameterProperties();
      base.InitializeCheckParameterList(); // Skips initializing parameters from the database.
    }

    #endregion


    #region Public Methods

    /// <summary>
    /// Determines whether a parameter by name exists in the check parameters object.
    /// </summary>
    /// <param name="AParameterName">The parameter name for the parameter in CHECK_PARAM_ID_NAME </param>
    /// <returns>True if the parameter exists.</returns>
    public bool ContainsLegacyParameter(string AParameterName)
    {
      return (GetLegacyParameter(AParameterName) != null);
    }

    /// <summary>
    /// Determines whether a parameter by name exists in the check parameters object.
    /// </summary>
    /// <param name="AParameterName">The parameter name for the parameter in CHECK_PARAM_ID_NAME </param>
    /// <param name="AOwner">Limits the check to the this category if not null.</param>
    /// <returns>True if the parameter exists.</returns>
    public bool ContainsLegacyParameter(string AParameterName, cCheckCategory AOwner)
    {
      return (GetLegacyParameter(AParameterName, AOwner) != null);
    }

    /// <summary>
    /// Gets the check parameter for the check engine corresponding to the passed parameter name.
    /// </summary>
    /// <param name="AParameterName">The parameter name for the parameter in CHECK_PARAM_ID_NAME </param>
    /// <returns>The parameter matching the parameter name if found, otherwise null.</returns>
    public cCheckParameterCheckEngine GetLegacyParameter(string AParameterName)
    {
      cCheckParameterCheckEngine CheckParameterCheckEngine;
      cCheckParameter CheckParameter;

      if (FCheckParameterCollection.TryGetValue(AParameterName.Trim().ToUpper(), out CheckParameter))
      {
        if (typeof(cCheckParameterCheckEngine).IsAssignableFrom(CheckParameter.GetType()))
          CheckParameterCheckEngine = (cCheckParameterCheckEngine)CheckParameter;
        else
          CheckParameterCheckEngine = null;
      }
      else
        CheckParameterCheckEngine = null;


      return CheckParameterCheckEngine;
    }

    /// <summary>
    /// Gets the check parameter for the check engine corresponding to the passed parameter name.
    /// </summary>
    /// <param name="AParameterName">The parameter name for the parameter in CHECK_PARAM_ID_NAME </param>
    /// <param name="AOwner">Limits the get to the this category if not null.</param>
    /// <returns>The parameter matching the parameter name if found, otherwise null.</returns>
    public cCheckParameterCheckEngine GetLegacyParameter(string AParameterName, cCheckCategory AOwner)
    {
      cCheckParameterCheckEngine Result = GetLegacyParameter(AParameterName);

      if (!Result.OwnerIsOrAncestorOf(AOwner))
        Result = null;

      return Result;
    }

    /// <summary>
    /// Adds the passed parameter to the parameter dictionary.
    /// </summary>
    /// <param name="parameterKey">The primary key (Check_Param_Id) of the parameter.</param>
    /// <param name="parameterName">The internal name (Check_Param_Id_Name) of the parameter.</param>
    public void RegisterParameter(int parameterKey, string parameterName)
    {
      if (!ContainsCheckParameter(parameterName))
      {
        cCheckParameter Parameter;
        {
          Parameter = new cCheckParameterLegacy(parameterKey, parameterName, this);
        }

        string errorMessage = null;
        AddCheckParameter(parameterName, Parameter, ref errorMessage);
      }
    }

    #endregion


    #region Protected Override Methods

    /// <summary>
    /// Calls the base version of the method to add parameters implemented as properties of
    /// this object, and then adds parameters to the parameter list for parameters not 
    /// implemented as properties of this object.
    /// </summary>
    protected override void InitializeCheckParameterList()
    {
      base.InitializeCheckParameterList();

      // Add Unimplemented Legacy Parameters
      string Sql = string.Format("select distinct CHECK_PARAM_ID, CHECK_PARAM_ID_NAME FROM VW_RULE_CHECK_PARAMETER where PROCESS_CD = '{0}' " +
                                 "union " +
                                 "select distinct CHECK_PARAM_ID, CHECK_PARAM_ID_NAME FROM VW_RULE_CHECK_CONDITION where PROCESS_CD = '{0}' " +
                                 "order by CHECK_PARAM_ID_NAME",
                                 CheckProcess.ProcessCd);
      DataTable ParameterTable;
      string ErrorMessage = "";

      if (cUtilities.Database_GetDataTable(Sql, FDatabaseAux, out ParameterTable, ref ErrorMessage))
      {
        foreach (DataRow ParameterRow in ParameterTable.Rows)
        {
          int ParameterKey = cDBConvert.ToInteger(ParameterRow["CHECK_PARAM_ID"]);
          string ParameterName = cDBConvert.ToString(ParameterRow["CHECK_PARAM_ID_NAME"]);

          if (!ContainsCheckParameter(ParameterName))
          {
            cCheckParameterLegacy Parameter = new cCheckParameterLegacy(ParameterKey, ParameterName, this);
            AddCheckParameter(ParameterName, Parameter, ref ErrorMessage);
          }
        }
      }
      else
        System.Diagnostics.Debug.WriteLine(string.Format("Legacy Parameter Addition Error ({0}): {1}", CheckProcess.ProcessCd, ErrorMessage));
    }

    #endregion

  }

}
