using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using ECMPS.Checks.DatabaseAccess;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;
using ECMPS.Checks.Mp.Parameters;
using ECMPS.Checks.Qa.Parameters;
using ECMPS.Checks.Em.Parameters;
using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.MonitorPlan;
using ECMPS.Checks.QA;
using ECMPS.Checks.EmissionsReport;

namespace UnitTest.UtilityClasses
{

  /// <summary>
  /// Implements a check parameter object used by CHET
  /// </summary>
  public class UnitTestCheckParameters : cCheckParametersCheckEngine
  {

    #region Public Constructors

    /// <summary>
    /// Instantiates an instance of cChetProcess
    /// </summary>
    /// <param name="AProcess">The parent process object.</param>
    /// <param name="ADatabaseAux">Connection to the aux database</param>
    public UnitTestCheckParameters()
    {
    }

    #endregion

    #region Public Methods

    public void RegisterParameter(string parameterName)
    {
      if (!ContainsCheckParameter(parameterName))
      {
        cCheckParameterLegacy Parameter = new cCheckParameterLegacy(0, parameterName, this);
        string errorMessage = null;
        AddCheckParameter(parameterName, Parameter, ref errorMessage);
      }
    }

	/// <summary>
	/// Initializes check parameters for unit testing
	/// </summary>
	public static cMpCheckParameters InstantiateMpParameters()
	{
		cMpCheckParameters mpCheckParameters = new cMpCheckParameters();
		cCategory category = new UnitTest.UtilityClasses.UnitTestCategory(mpCheckParameters);
		//target = new cMethodChecks(mpCheckParameters);

		MpParameters.Init(category.Process);
		MpParameters.Category = category;

		return mpCheckParameters;
	}

	/// <summary>
	/// Initializes check parameters for unit testing
	/// </summary>
	public static cQaCheckParameters InstantiateQaParameters()
	{
		cQaCheckParameters qaCheckParameters = new cQaCheckParameters();
		cCategory category = new UnitTest.UtilityClasses.UnitTestCategory(qaCheckParameters);

		QaParameters.Init(category.Process);
		QaParameters.Category = category;

		return qaCheckParameters;
	}

	/// <summary>
	/// Initializes check parameters for unit testing
	/// </summary>
	public static cEmissionsCheckParameters InstantiateEmParameters()
	{
		cEmissionsCheckParameters emCheckParameters = new cEmissionsCheckParameters();
		cCategory category = new UnitTest.UtilityClasses.UnitTestCategory(emCheckParameters);

		EmParameters.Init(category.Process);
		EmParameters.Category = category;

		return emCheckParameters;
	}

    #endregion

    #region Protected Override Methods

    /// <summary>
    /// Instantiate the check parameter properties
    /// </summary>
    protected override void InstantiateCheckParameterProperties()
    {
    }

    #endregion

  }
}

