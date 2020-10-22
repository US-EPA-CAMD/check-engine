using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;


namespace UnitTest.UtilityClasses
{
  public class UnitTestProcess : cProcess
  {

    #region Constructors

    /// <summary>
    /// Instaintiates a test version of cCategory.
    /// 
    /// Additionally the constructor sets the Process property to the test version of cProcess.
    /// </summary>
    /// <param name="checkParameters">The object implementing the old style of instantiated check parameters.</param>
    public UnitTestProcess(cCheckParametersCheckEngine checkParameters = null)
    {
      mCheckEngine = new UnitTestCheckEngine();
      ProcessParameters = (checkParameters == null) ? new UnitTestCheckParameters() : checkParameters;
    }

    #endregion


    #region Abstract and Virtual Overrides: No Logic

    /// <summary>
    /// Normally used to initialize the checks array with the checks needed by the process.
    /// </summary>
    /// <param name="checksDllPath"></param>
    /// <param name="errorMessage"></param>
    /// <returns></returns>
    public override bool CheckProcedureInit(string checksDllPath, ref string errorMessage)
    {
      return true;
    }

    /// <summary>
    /// Normally used to execute the check looping.
    /// </summary>
    /// <returns></returns>
    protected override string ExecuteChecksWork()
    {
      return "";
    }

    /// <summary>
    /// Initiates objects needed to handle calculated data.
    /// </summary>
    protected override void InitCalculatedData()
    {
      return;
    }

    /// <summary>
    /// Normally loads data tables used to populate data check parameters.
    /// </summary>
    protected override void InitSourceData()
    {
      return;
    }

    /// <summary>
    /// This method initializes the class containing static properties enabling strongly typed access to the parameters used by the process.
    /// </summary>
    protected override void InitStaticParameterClass()
    {
      return;
    }

    /// <summary>
    /// Allows the setting of the current category for which parameters will be set.
    /// </summary>
    /// <param name="category"></param>
    public override void SetStaticParameterCategory(cCategory category)
    {
      return;
    }

    #endregion

  }
}
