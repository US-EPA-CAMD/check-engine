using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECMPS.DM.Definitions;
using ECMPS.DM.Utilities;

namespace ECMPS.DM.Miscellaneous
{
  /// <summary>
  /// Indicates the locations whose value should be reduced by another location for a specific complex parameter.
  /// </summary>
  public class cReduceBy
  {

    #region Public Constructor

    /// <summary>
    /// Constructor for a reduce by spec.
    /// </summary>
    /// <param name="locationInfo"></param>
    public cReduceBy(cLocationInfo[] locationInfo)
    {
      LocationInfo = locationInfo;

      ComplexParameterCount = Enum.GetValues(typeof(eComplexParameter)).Length;

      // Initialize Spec Grid
      {
        SpecGrid = new cLocationPositionList[ComplexParameterCount, LocationCount];

        for (int parameterDex = 0; parameterDex < ComplexParameterCount; parameterDex++)
          for (int locationDex = 0; locationDex < LocationCount; locationDex++)
            SpecGrid[parameterDex, locationDex] = new cLocationPositionList();
      }
    }

    #endregion


    #region Public Properties

    /// <summary>
    /// The count of Complex Parameter enumerations
    /// </summary>
    public int ComplexParameterCount { get; private set; }

    /// <summary>
    /// The count of locations
    /// </summary>
    public int LocationCount { get { return ((LocationInfo != null) ? LocationInfo.Length : 0); } }

    /// <summary>
    /// The location info the Reduce By Spec references.
    /// </summary>
    public cLocationInfo[] LocationInfo { get; private set; }

    /// <summary>
    /// The grid of reduce by specs by complex parameter and location dex.
    /// </summary>
    public cLocationPositionList[,] SpecGrid { get; private set; }

    #endregion


    #region Public Methods

    /// <summary>
    /// Adds a list of reduce by location positions to the spec for all parameters and a specific a location.
    /// </summary>
    /// <param name="locationPosition">The location to reduce.</param>
    /// <param name="reduceByPositions">The locations to reduce by.</param>
    public void Add(int locationPosition, int[] reduceByPositions)
    {
      for (int parameterDex = 0; parameterDex < ComplexParameterCount; parameterDex++)
      {
        SpecGrid[parameterDex, locationPosition] = new cLocationPositionList(reduceByPositions);
      }
    }

    /// <summary>
    /// Adds a list of reduce by location positions to the spec for a parameter and location.
    /// </summary>
    /// <param name="complexParameter">The parameter to reduce./</param>
    /// <param name="locationPosition">The location to reduce.</param>
    /// <param name="reduceByPositions">The locations to reduce by.</param>
    public void Add(eComplexParameter complexParameter, int locationPosition, int[] reduceByPositions)
    {
      SpecGrid[(int)complexParameter, locationPosition] = new cLocationPositionList(reduceByPositions);
    }

    /// <summary>
    /// Returns the reduce by location list for the passed parameter and location position.
    /// </summary>
    /// <param name="complexParameter">The affected parameter.</param>
    /// <param name="locationPosition">The location position for potential reduction.</param>
    /// <returns>The list of location to reduce by.</returns>
    public cLocationPositionList Get(eComplexParameter complexParameter, int locationPosition)
    {
      cLocationPositionList result;

      result = SpecGrid[(int)complexParameter, locationPosition];

      return result;
    }

    /// <summary>
    /// Remove a list of reduce by location positions.
    /// </summary>
    /// <param name="locationPosition">The location to reduce.</param>
    public void Remove(int locationPosition)
    {
      for (int parameterDex = 0; parameterDex < ComplexParameterCount; parameterDex++)
      {
        SpecGrid[parameterDex, locationPosition] = new cLocationPositionList();
      }
    }

    /// <summary>
    /// Remove a list of reduce by location positions.
    /// </summary>
    /// <param name="complexParameter">The parameter to reduce.</param>
    /// <param name="locationPosition">The location to reduce.</param>
    public void Remove(eComplexParameter complexParameter, int locationPosition)
    {
      SpecGrid[(int)complexParameter, locationPosition] = new cLocationPositionList();
    }

    #endregion

  }
}
