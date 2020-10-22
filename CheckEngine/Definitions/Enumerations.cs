using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECMPS.Checks.CheckEngine.Definitions
{

  /// <summary>
  /// Indicates the type of check run to perform
  /// </summary>
  public enum eCheckEngineRunMode
  {

    /// <summary>
    /// Run checks evaluation
    /// </summary>
    Normal,

    /// <summary>
    /// Run checks evaluation in debug mode
    /// </summary>
    Debug,

    /// <summary>
    /// Run a direct check test initialization
    /// </summary>
    CheckTestInit

  }

}
