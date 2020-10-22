using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECMPS.DM.Definitions;
using ECMPS.DM.Utilities;


namespace ECMPS.DM.Miscellaneous
{

  /// <summary>
  /// Contains a list of bool? for the locations in a report.
  /// </summary>
  public class cLocationBoolList
  {

    #region Public Constructor

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="locationInfo">List of reduce by locaiton information</param>
    public cLocationBoolList(cLocationInfo[] locationInfo)
    {
      LocationInfo = locationInfo;

      LocationBoolList = new bool?[locationInfo.Length];
      {
        for (int dex = 0; dex < locationInfo.Length; dex++)
          LocationBoolList[dex] = null;
      }
    }

    #endregion


    #region Public Methods

    /// <summary>
    /// Gets the bool? for the passed location position.
    /// </summary>
    /// <param name="locationPostion">The location position.</param>
    /// <returns>The bool? value for the location.</returns>
    public bool? Get(int locationPostion)
    {
      bool? result;

      result = LocationBoolList[locationPostion];

      return result;
    }

    /// <summary>
    /// Sets the bool? value based on the passed location position.
    /// </summary>
    /// <param name="locationPostion">The location position.</param>
    /// <param name="value">The value to set.</param>
    public void Set(int locationPostion, bool? value)
    {
      LocationBoolList[locationPostion] = value;
    }

    /// <summary>
    /// Sets the bool? value based on the passed location name.
    /// </summary>
    /// <param name="locationName">The location name.</param>
    /// <param name="value">The value to set.</param>
    public void Set(string locationName, bool? value)
    {
      int? locationPostion;

      if (LocationInfo.GetPosition(locationName, out locationPostion))
        LocationBoolList[locationPostion.Value] = value;
    }

    #endregion


    #region Private Properties

    /// <summary>
    /// The list of reduce by location positions.
    /// </summary>
    private bool?[] LocationBoolList { get; set; }

    /// <summary>
    /// The location info the Reduce By Spec references.
    /// </summary>
    private cLocationInfo[] LocationInfo { get; set; }

    #endregion

  }
}
