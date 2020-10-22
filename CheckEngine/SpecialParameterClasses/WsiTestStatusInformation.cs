using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECMPS.Checks.Data.Ecmps.CheckEm.Function;


namespace ECMPS.Checks.CheckEngine.SpecialParameterClasses
{

  /// <summary>
  /// Class used to store weekly system integrity test information for individual components.
  /// The information is updated in the weekly system integrity test checks and used in the 
  /// weekly system integrity status checks.
  /// </summary>
  public class WsiTestStatusInformation
  {

    /// <summary>
    /// Creates an new instance of the class with OperatingDateList initialized to an empty date list.
    /// All other instance variables are initialized to null.
    /// </summary>
    public WsiTestStatusInformation()
    {
      MostRecentTestRecord = null;
      OperatingDateList = new List<DateTime>();
    }


    /// <summary>
    /// The most recent WSI test record before the current hour.
    /// </summary>
    public WeeklySystemIntegrity MostRecentTestRecord { get; set; }

    /// <summary>
    /// The list of operating dates for the MostRecentTestRecrod.
    /// </summary>
    public List<DateTime> OperatingDateList { get; set; }
 
  }

}
