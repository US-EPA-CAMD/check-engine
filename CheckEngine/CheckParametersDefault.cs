using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using ECMPS.Checks.DatabaseAccess;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

namespace ECMPS.Checks.CheckEngine
{

  /// <summary>
  /// Implements a check parameter object used by CHET
  /// </summary>
  public class cCheckParametersDefault : cCheckParametersCheckEngine
  {

    #region Public Constructors

    /// <summary>
    /// Instantiates an instance of cChetProcess
    /// </summary>
    /// <param name="AProcess">The parent process object.</param>
    /// <param name="ADatabaseAux">Connection to the aux database</param>
    public cCheckParametersDefault(cProcess AProcess, cDatabase ADatabaseAux)
      : base((cCheckProcess)AProcess, ADatabaseAux)
    {
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
