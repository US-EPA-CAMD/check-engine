using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;


namespace UnitTest.UtilityClasses
{
  public class UnitTestCategory : cCategory
  {

    #region Constructors

    /// <summary>
    /// Instaintiates a test version of cCategory.
    /// 
    /// Additionally the constructor sets the Process property to the test version of cProcess.
    /// </summary>
    /// <param name="checkParameters">The object implementing the old style of instantiated check parameters.</param>
    public UnitTestCategory(cCheckParametersCheckEngine checkParameters = null)
    {
      Process = new UnitTestProcess(checkParameters);
      CheckEngine = Process.CheckEngine;
    }

    #endregion


    #region Abstract Overrides: No Logic

    /// <summary>
    /// Normally used to filter Data parameters and to assign some DataRow parameters for a check category.
    /// </summary>
    protected override void FilterData()
    {
      return;
    }


    /// <summary>
    /// Normally grabs the values compared to error suppression selection parameters for a check category.
    /// </summary>
    /// <returns></returns>
    protected override bool SetErrorSuppressValues()
    {
      return true;
    }


    /// <summary>
    /// Normally sets the Record Identifier used in check result messages for the target "current" row.
    /// </summary>
    protected override void SetRecordIdentifier()
    {
      return;
    }

    #endregion
  
  }
}
