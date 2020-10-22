using System;
using System.Collections.Generic;
using System.Text;

namespace ECMPS.Checks.Parameters
{
  /// <summary>
  /// The base for all Process objects
  /// </summary>
  public class cCheckProcess
  {
 
    #region Constructors

    /// <summary>
    /// Instantiates a cCheckProcess object
    /// </summary>
    /// <param name="AProcessCd">The process code associated with the check process object</param>
    public cCheckProcess(string AProcessCd)
    {
      FProcessCd = AProcessCd;
    }


    /// <summary>
    /// Instantiates a cCheckProcess object primarily for unit testing purposes.
    /// </summary>
    protected cCheckProcess()
    {
    }

    #endregion


    #region Public Properties

    #region Property Fields

    private string FProcessCd;

    #endregion

    /// <summary>
    /// The process code of the category object instance.
    /// </summary>
    public string ProcessCd { get { return FProcessCd; } }

    #endregion

 }
}
