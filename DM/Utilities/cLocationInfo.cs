using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECMPS.Definitions.Extensions;


namespace ECMPS.DM.Utilities
{

  /// <summary>
  /// Contains the location and unit keys for a location.
  /// 
  /// The unit key will be null if the location is not a unit.
  /// </summary>
  public class cLocationInfo
  {

    #region Public Constructors

    /// <summary>
    /// Creates a cLocationInfo object.
    /// </summary>
    /// <param name="locationKey">The location key of the location.</param>
    /// <param name="unitKey">The unit key if the location is a unit.</param>
    /// <param name="locationName">The name of the location.</param>
    public cLocationInfo(string locationKey, int? unitKey, string locationName)
    {
      if (!locationKey.HasValue() && !locationName.HasValue())
        throw new System.ArgumentException("Argument cannot be null", "locationKey, locationName");
      else if (!locationKey.HasValue())
        throw new System.ArgumentException("Argument cannot be null", "locationKey");
      else if (!locationName.HasValue())
        throw new System.ArgumentException("Argument cannot be null", "locationName");

      LocationKey = locationKey;
      LocationName = locationName;
      UnitKey = unitKey;
    }

    #endregion


    #region Public Properties

    /// <summary>
    /// The Location Key of the location.
    /// </summary>
    public string LocationKey { get; private set; }

    /// <summary>
    /// The Name of the location.
    /// </summary>
    public string LocationName { get; private set; }

    /// <summary>
    /// The Unit Key of the location.
    /// </summary>
    public int? UnitKey { get; private set; }

    #endregion

  }

}
