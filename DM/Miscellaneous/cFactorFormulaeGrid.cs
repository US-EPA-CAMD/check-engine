using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECMPS.DM.Utilities;

namespace ECMPS.DM.Miscellaneous
{
  /// <summary>
  /// Contains a Unit to Location factor formula grid.
  /// </summary>
  public class cFactorFormulaeGrid
  {

    #region Public Constructors

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="parent">The parent cFactorFormulae object.</param>
    /// <param name="unitCount">The number of units in the configuration.</param>
    /// <param name="locationCount">The number of locations in the configuration.</param>
    public cFactorFormulaeGrid(cFactorFormulae parent, int unitCount, int locationCount)
    {
      Parent = parent;

      UnitCount = unitCount;
      LocationCount = locationCount;

      Formulae = new string[unitCount, locationCount];
      UsedInFormula = new cLocationPositionList[unitCount, locationCount];
    }

    #endregion


    #region Public Properties

    /// <summary>
    /// Contains a grid of formulae for producing apportionment factors from a location to a unit.
    /// </summary>
    public string[,] Formulae { get; private set; }

    /// <summary>
    /// The count of location columns
    /// </summary>
    public int LocationCount { get; private set; }

    /// <summary>
    /// The parent cFactorFormulae object.
    /// </summary>
    public cFactorFormulae Parent { get; private set; }

    /// <summary>
    /// The count of unit rows
    /// </summary>
    public int UnitCount { get; private set; }

    /// <summary>
    /// Contains a grid of elements indicating whether a unit and location combination
    /// use a particular location in a formula.
    /// </summary>
    public cLocationPositionList[,] UsedInFormula { get; private set; }

    #endregion


    #region Public Methods

    /// <summary>
    /// Gets the formula for a unit and location.
    /// </summary>
    /// <param name="unitIndex">The count of unit rows</param>
    /// <param name="locationIndex">The count of location columns</param>
    /// <returns>The factor formula for the unit and location at the given indexes.</returns>
    public string GetFormula(int unitIndex, int locationIndex)
    {
      string result;

      result = Formulae[unitIndex, locationIndex];

      return result;
    }

    /// <summary>
    /// Gets the location positions used in the formula for a unit and location.
    /// </summary>
    /// <param name="unitIndex">The count of unit rows</param>
    /// <param name="locationIndex">The count of location columns</param>
    /// <returns>The list of location positions used in the formula at the given indexes.</returns>
    public cLocationPositionList GetUsedInFormula(int unitIndex, int locationIndex)
    {
      cLocationPositionList result;

      result = UsedInFormula[unitIndex, locationIndex];

      return result;
    }

    /// <summary>
    /// Sets a specific factor formula for a unit and location.
    /// </summary>
    /// <param name="unitIndex">The count of unit rows</param>
    /// <param name="locationIndex">The count of location columns</param>
    /// <param name="formula">The factor formula for the unit and location at the given indexes.</param>
    public void SetFormula(int unitIndex, int locationIndex, string formula)
    {
      Formulae[unitIndex, locationIndex] = cCalculator.StandardToReversePolish(formula);
      SetUsedInFormula(unitIndex, locationIndex, formula);
      Parent.SetUsedInHiLoadFormula();
    }

    #endregion


    #region Private Method

    /// <summary>
    /// Sets Used In Formula.
    /// </summary>
    private void SetUsedInFormula(int unitIndex, int locationIndex, string formula)
    {
      int[] usedList;

      bool[] usedLocationIndexes = cCalculator.GetIndexUse(formula);

      int usedCount = Math.Min(usedLocationIndexes.Length, LocationCount);

      int count = 0;

      for (int usedDex = 0; usedDex < usedCount; usedDex++)
        if (usedLocationIndexes[usedDex])
          count += 1;

      if (count > 0)
      {
        usedList = new int[count];

        int usedCnt = 0;

        for (int usedDex = 0; usedDex < usedCount; usedDex++)
        {
          if (usedLocationIndexes[usedDex])
          {
            usedList[usedCnt] = usedDex;
            usedCnt += 1;
          }
        }
      }
      else
        usedList = new int[0];

      UsedInFormula[unitIndex, locationIndex] = new cLocationPositionList(usedList);
    }

    #endregion

  }
}
