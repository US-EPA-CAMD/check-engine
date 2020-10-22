using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECMPS.Definitions.Extensions;
using ECMPS.DM.Definitions;


namespace ECMPS.DM.Utilities
{

  /// <summary>
  /// Extensions used within ECMPS.DM
  /// </summary>
  public static class cExtensions
  {

    #region Typing To, Inspection Of, and Manipulation Of

    #region Array

    /// <summary>
    /// Returns cLocationInfo.LocationKey in a cLocationInfo array as a delimited list.
    /// </summary>
    /// <param name="array">The cLocatiionInfo array to convert.</param>
    /// <param name="delimeter">The delimiter of the resulting list.</param>
    /// <returns>A delimited string list.</returns>
    public static string AsList(this cLocationInfo[] array, char delimeter)
    {
      string result = null;

      bool first = true;

      foreach (cLocationInfo item in array)
      {
        if (first)
          result = item.LocationKey;
        else
          result += delimeter.ToString() + item.LocationKey;
      }

      return result;
    }

    /// <summary>
    /// Returns cLocationInfo.LocationKey in a cLocationInfo array as a comma delimited list.
    /// </summary>
    /// <param name="array">The cLocatiionInfo array to convert.</param>
    /// <returns>A delimited string list.</returns>
    public static string AsList(this cLocationInfo[] array)
    {
      string result = null;

      result = array.AsList(',');

      return result;
    }
    #endregion

    #region ApportionmentType

    /// <summary>
    /// Returns the database code for the apportionment type enum value.
    /// </summary>
    /// <param name="value">The enumeration values</param>
    /// <returns>Database apportionment code.</returns>
    public static string DbCode(this eApportionmentType value)
    {
      string result;
      {
        switch (value)
        {
          case eApportionmentType.Changed: result = "CHANGED"; break;
          case eApportionmentType.CommonPipe: result = "CP"; break;
          case eApportionmentType.CommonPipeLtff: result = "CPLTFF"; break;
          case eApportionmentType.CommonStack: result = "CS"; break;
          case eApportionmentType.CommonStackAndPipe: result = "CSP"; break;
          case eApportionmentType.Complex: result = "COMPLEX"; break;
          case eApportionmentType.MultiplePipe: result = "MP"; break;
          case eApportionmentType.MultiplePipeInvolved: result = "MPI"; break;
          case eApportionmentType.MultipleStack: result = "MS"; break;
          case eApportionmentType.Unit: result = "UN"; break;
          default: result = "NOT"; break;
        }
      }

      return result;
    }

    /// <summary>
    /// Returns the database code for the apportionment type enum value.
    /// </summary>
    /// <param name="value">The enumeration values</param>
    /// <returns>Database apportionment code.</returns>
    public static string DbCode(this eApportionmentType? value)
    {
      string result;
      {
        if (value.HasValue)
          result = value.Value.DbCode();
        else
          result = null;
      }

      return result;
    }

    #endregion

    #region LocationInfo

    /// <summary>
    /// Gets the position of a location name in a location info array.
    /// </summary>
    /// <param name="array">The location info array</param>
    /// <param name="locationName">The name of the location</param>
    /// <param name="position">The position of the location in the array, null if it does not exist.</param>
    /// <returns>Returns true if the postion was found.</returns>
    public static bool GetPosition(this cLocationInfo[] array, string locationName, out int? position)
    {
      bool result;

      position = null;

      if (array != null)
      {
        for (int dex = 0; dex < array.Length; dex++)
        {
          if (array[dex].LocationName == locationName)
          {
            position = dex;
            break;
          }
        }
      }

      result = position.HasValue;

      return result;
    }

    #endregion

    #endregion

  }

}
