using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECMPS.DM.Utilities
{

  /// <summary>
  /// Contains the location and unit keys for a unit.
  /// </summary>
  public class cUnitInfo : cLocationInfo
  {

    /// <summary>
    /// Create a Unit Info class that stores the location key, unit key, and location position.
    /// </summary>
    /// <param name="locationKey">The location (MON_LOC_ID) key.</param>
    /// <param name="unitKey">The unit (UNIT_ID) key.</param>
    /// <param name="unitName">The name of the unit.</param>
    /// <param name="locationPosition">The position of the location in Location Info.</param>
    public cUnitInfo(string locationKey, int? unitKey, string unitName, int? locationPosition)
      : base(locationKey, unitKey, unitName)
    {
      LocationPosition = locationPosition;
    }

    /// <summary>
    /// The location of the unit in Location Info
    /// </summary>
    public int? LocationPosition { get; private set; }

  }

}
