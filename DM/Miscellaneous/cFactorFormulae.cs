using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECMPS.DM.Definitions;
using ECMPS.DM.Utilities;

namespace ECMPS.DM.Miscellaneous
{
  /// <summary>
  /// Contains the factor formulae for a configuration
  /// </summary>
  public class cFactorFormulae
  {

    #region Public Constructor

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="unitInfo">Unit Info</param>
    /// <param name="locationInfo">Location Info</param>
    /// <param name="beganDate">Began Date</param>
    /// <param name="beganHour">Began Hour</param>
    /// <param name="endedDate">Ended Date</param>
    /// <param name="endedHour">Ended Hour</param>
    public cFactorFormulae(cUnitInfo[] unitInfo, cLocationInfo[] locationInfo,
                           DateTime beganDate, int beganHour, DateTime endedDate, int endedHour)
    {
      UnitInfo = unitInfo;
      LocationInfo = locationInfo;

      BeganDate = beganDate;
      BeganHour = beganHour;
      EndedDate = endedDate;
      EndedHour = endedHour;

      OperatingCondition = new cLocationBoolList(locationInfo);

      ComplexParameterCount = Enum.GetValues(typeof(eComplexParameter)).Length;

      // Initialize Factor Formulae Grids
      {
        HiLoadFormulaeGrids = new cFactorFormulaeGrid[ComplexParameterCount];
        OpTimeFormulaeGrids = new cFactorFormulaeGrid[ComplexParameterCount];

        for (int parameterDex = 0; parameterDex < ComplexParameterCount; parameterDex++)
        {
          HiLoadFormulaeGrids[parameterDex] = new cFactorFormulaeGrid(this, UnitCount, LocationCount);
          OpTimeFormulaeGrids[parameterDex] = new cFactorFormulaeGrid(this, UnitCount, LocationCount);
        }
      }

      UsedInHiLoadFormula = new bool[LocationCount];

      ReduceBy = new cReduceBy(locationInfo);
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="unitInfo">Unit Info</param>
    /// <param name="locationInfo">Location Info</param>
    /// <param name="beganDate">Began Date</param>
    /// <param name="beganHour">Began Hour</param>
    /// <param name="endedDate">Ended Date</param>
    /// <param name="endedHour">Ended Hour</param>
    /// <param name="operatingCondition">The operating condition that must match to use this formual.</param>
    public cFactorFormulae(cUnitInfo[] unitInfo, cLocationInfo[] locationInfo,
                           DateTime beganDate, int beganHour, DateTime endedDate, int endedHour,
                           cLocationBoolList operatingCondition)
    {
      UnitInfo = unitInfo;
      LocationInfo = locationInfo;

      BeganDate = beganDate;
      BeganHour = beganHour;
      EndedDate = endedDate;
      EndedHour = endedHour;

      OperatingCondition = operatingCondition;

      ComplexParameterCount = Enum.GetValues(typeof(eComplexParameter)).Length;

      // Initialize Factor Formulae Grids
      {
        HiLoadFormulaeGrids = new cFactorFormulaeGrid[ComplexParameterCount];
        OpTimeFormulaeGrids = new cFactorFormulaeGrid[ComplexParameterCount];

        for (int parameterDex = 0; parameterDex < ComplexParameterCount; parameterDex++)
        {
          HiLoadFormulaeGrids[parameterDex] = new cFactorFormulaeGrid(this, UnitCount, LocationCount);
          OpTimeFormulaeGrids[parameterDex] = new cFactorFormulaeGrid(this, UnitCount, LocationCount);
        }
      }

      UsedInHiLoadFormula = new bool[LocationCount];

      ReduceBy = new cReduceBy(locationInfo);
    }

    #endregion


    #region Public Properties

    /// <summary>
    /// The Began Date of the hour range covered by the formulae
    /// </summary>
    public DateTime BeganDate { get; private set; }

    /// <summary>
    /// The Began Hour of the hour range covered by the formulae
    /// </summary>
    public int BeganHour { get; private set; }

    /// <summary>
    /// The count of Complex Parameter enumerations
    /// </summary>
    public int ComplexParameterCount { get; private set; }

    /// <summary>
    /// The Ended Date of the hour range covered by the formulae
    /// </summary>
    public DateTime EndedDate { get; private set; }

    /// <summary>
    /// The Ended Hour of the hour range covered by the formulae
    /// </summary>
    public int EndedHour { get; private set; }

    /// <summary>
    /// The array of factor formula grids for each apportionment parameter.
    /// </summary>
    public cFactorFormulaeGrid[] HiLoadFormulaeGrids { get; private set; }

    /// <summary>
    /// The array of even factor formula grids for each apportionment parameter.
    /// </summary>
    public cFactorFormulaeGrid[] OpTimeFormulaeGrids { get; private set; }

    /// <summary>
    /// The count of locations
    /// </summary>
    public int LocationCount { get { return ((LocationInfo != null) ? LocationInfo.Length : 0); } }

    /// <summary>
    /// The Location Info represented by the factor formulae.
    /// </summary>
    public cLocationInfo[] LocationInfo { get; private set; }

    /// <summary>
    /// The operating condition spec that must exist to use these formulae.
    /// </summary>
    public cLocationBoolList OperatingCondition { get; private set; }

    /// <summary>
    /// The reduce by information for the complex apportionment.
    /// </summary>
    public cReduceBy ReduceBy { get; private set; }

    /// <summary>
    /// The count of units
    /// </summary>
    public int UnitCount { get { return ((UnitInfo != null) ? UnitInfo.Length : 0); } }

    /// <summary>
    /// The Unit Info represented by the factor formulae.
    /// </summary>
    public cUnitInfo[] UnitInfo { get; private set; }

    /// <summary>
    /// Indicates whether a particular location is used the formulae
    /// </summary>
    public bool[] UsedInHiLoadFormula { get; private set; }

    #endregion


    #region Public Methods: Formula

    /// <summary>
    /// Gets the even formula for a unit and location.
    /// </summary>
    /// <param name="complexParameter">The complex parameter of the formula</param>
    /// <param name="unitIndex">The count of unit rows</param>
    /// <param name="locationIndex">The count of location columns</param>
    /// <returns>The factor formula for the unit and location at the given indexes.</returns>
    public string GetEvenFormula(eComplexParameter complexParameter, int unitIndex, int locationIndex)
    {
      string result;

      result = OpTimeFormulaeGrids[(int)complexParameter].GetFormula(unitIndex, locationIndex);

      return result;
    }

    /// <summary>
    /// Gets the even formula for a unit and location.
    /// </summary>
    /// <param name="complexParameter">The complex parameter of the formula</param>
    /// <param name="unitName">The name of the unit for which to return a formula</param>
    /// <param name="locationName">The name of the location for which to return a formula</param>
    /// <returns>The factor formula for the unit and location at the given indexes.</returns>
    public string GetEvenFormula(eComplexParameter complexParameter, string unitName, string locationName)
    {
      string result;

      int? unitDex;
      int? locationDex;

      if (UnitInfo.GetPosition(unitName, out unitDex) &&
          LocationInfo.GetPosition(locationName, out locationDex))
      {
        result = GetEvenFormula(complexParameter, unitDex.Value, locationDex.Value);
      }
      else
      {
        result = null;
      }

      return result;
    }

    /// <summary>
    /// Gets the factor formula for a unit and location.
    /// </summary>
    /// <param name="complexParameter">The complex parameter of the formula</param>
    /// <param name="unitIndex">The count of unit rows</param>
    /// <param name="locationIndex">The count of location columns</param>
    /// <returns>The factor formula for the unit and location at the given indexes.</returns>
    public string GetFactorFormula(eComplexParameter complexParameter, int unitIndex, int locationIndex)
    {
      string result;

      result = HiLoadFormulaeGrids[(int)complexParameter].GetFormula(unitIndex, locationIndex);

      return result;
    }

    /// <summary>
    /// Gets the factor formula for a unit and location.
    /// </summary>
    /// <param name="complexParameter">The complex parameter of the formula</param>
    /// <param name="unitName">The name of the unit for which to return a formula</param>
    /// <param name="locationName">The name of the location for which to return a formula</param>
    /// <returns>The factor formula for the unit and location at the given indexes.</returns>
    public string GetFactorFormula(eComplexParameter complexParameter, string unitName, string locationName)
    {
      string result;

      int? unitDex;
      int? locationDex;

      if (UnitInfo.GetPosition(unitName, out unitDex) &&
          LocationInfo.GetPosition(locationName, out locationDex))
      {
        result = GetFactorFormula(complexParameter, unitDex.Value, locationDex.Value);
      }
      else
      {
        result = null;
      }

      return result;
    }

    /// <summary>
    /// Sets a specific factor formula for a unit and location.
    /// </summary>
    /// <param name="unitIndex">The count of unit rows</param>
    /// <param name="locationIndex">The count of location columns</param>
    /// <param name="hiLoadFormula">The factor formula for the unit and location at the given indexes.</param>
    /// <param name="opTimeFormula">The factor formula for the unit and location at the given indexes.</param>
    public void SetFormula(int unitIndex, int locationIndex, string hiLoadFormula, string opTimeFormula)
    {
      for (int dex = 0; dex < ComplexParameterCount; dex++)
      {
        HiLoadFormulaeGrids[dex].SetFormula(unitIndex, locationIndex, hiLoadFormula);
        OpTimeFormulaeGrids[dex].SetFormula(unitIndex, locationIndex, opTimeFormula);
      }
    }

    /// <summary>
    /// Sets a specific factor formula for a unit and location.
    /// </summary>
    /// <param name="unitName">The name of the unit for which to return a formula</param>
    /// <param name="locationName">The name of the location for which to return a formula</param>
    /// <param name="hiLoadFormula">The factor formula for the unit and location at the given indexes.</param>
    /// <param name="opTimeFormula">The factor formula for the unit and location at the given indexes.</param>
    public void SetFormula(string unitName, string locationName, string hiLoadFormula, string opTimeFormula)
    {
      int? unitDex;
      int? locationDex;

      if (UnitInfo.GetPosition(unitName, out unitDex) &&
          LocationInfo.GetPosition(locationName, out locationDex))
      {
        for (int dex = 0; dex < ComplexParameterCount; dex++)
        {
          HiLoadFormulaeGrids[dex].SetFormula(unitDex.Value, locationDex.Value, ChangeLocationNamesToPositions(hiLoadFormula));
          OpTimeFormulaeGrids[dex].SetFormula(unitDex.Value, locationDex.Value, ChangeLocationNamesToPositions(opTimeFormula));
        }
      }
    }

    /// <summary>
    /// Sets a specific factor formula for a unit and location.
    /// </summary>
    /// <param name="complexParameter">The complex parameter of the formula</param>
    /// <param name="unitIndex">The count of unit rows</param>
    /// <param name="locationIndex">The count of location columns</param>
    /// <param name="hiLoadFormula">The factor formula for the unit and location at the given indexes.</param>
    /// <param name="opTimeFormula">The factor formula for the unit and location at the given indexes.</param>
    public void SetFormula(eComplexParameter complexParameter, int unitIndex, int locationIndex, string hiLoadFormula, string opTimeFormula)
    {
      HiLoadFormulaeGrids[(int)complexParameter].SetFormula(unitIndex, locationIndex, hiLoadFormula);
      OpTimeFormulaeGrids[(int)complexParameter].SetFormula(unitIndex, locationIndex, opTimeFormula);
    }

    /// <summary>
    /// Sets a specific factor formula for a unit and location.
    /// </summary>
    /// <param name="complexParameter">The complex parameter of the formula</param>
    /// <param name="unitName">The name of the unit for which to return a formula</param>
    /// <param name="locationName">The name of the location for which to return a formula</param>
    /// <param name="hiLoadFormula">The factor formula for the unit and location at the given indexes.</param>
    /// <param name="opTimeFormula">The factor formula for the unit and location at the given indexes.</param>
    public void SetFormula(eComplexParameter complexParameter, string unitName, string locationName, string hiLoadFormula, string opTimeFormula)
    {
      int? unitDex;
      int? locationDex;

      if (UnitInfo.GetPosition(unitName, out unitDex) &&
          LocationInfo.GetPosition(locationName, out locationDex))
      {
        HiLoadFormulaeGrids[(int)complexParameter].SetFormula(unitDex.Value, locationDex.Value, ChangeLocationNamesToPositions(hiLoadFormula));
        OpTimeFormulaeGrids[(int)complexParameter].SetFormula(unitDex.Value, locationDex.Value, ChangeLocationNamesToPositions(opTimeFormula));
      }
    }

    #endregion


    #region Public Methods: Operating Condition

    /// <summary>
    /// 
    /// </summary>
    /// <param name="locationName"></param>
    /// <param name="operatingCondition"></param>
    public void UpdateOperatingCondition(string locationName, bool? operatingCondition)
    {
      int? locationDex;

      if (LocationInfo.GetPosition(locationName, out locationDex))
      {
        OperatingCondition.Set(locationDex.Value, operatingCondition);
      }
    }

    #endregion


    #region Public Methods: Reduce By

    /// <summary>
    /// Adds a list of reduce by location positions to the spec for all parameters and a specific a location.
    /// </summary>
    /// <param name="locationPosition">The location to reduce.</param>
    /// <param name="reduceByPositions">The locations to reduce by.</param>
    public void AddReduceBy(int locationPosition, int[] reduceByPositions)
    {
      ReduceBy.Add(locationPosition, reduceByPositions);
    }

    /// <summary>
    /// Adds a list of reduce by location positions to the spec for all parameters and a specific a location.
    /// </summary>
    /// <param name="locationName">The name of the location to reduce.</param>
    /// <param name="reduceByNames">The name of the locations to reduce by.</param>
    public void AddReduceBy(string locationName, string[] reduceByNames)
    {
      int? locationDex;

      if (LocationInfo.GetPosition(locationName, out locationDex))
      {
        bool success = true;

        int[] reduceByPositions = new int[reduceByNames.Length];
        int reduceByCnt = 0;

        foreach (string reduceByName in reduceByNames)
        {
          int? reduceByDex;

          if (LocationInfo.GetPosition(reduceByName, out reduceByDex))
          {
            reduceByPositions[reduceByCnt] = reduceByDex.Value;
            reduceByCnt += 1;
          }
          else
            success = false;
        }

        if (success)
          ReduceBy.Add(locationDex.Value, reduceByPositions);
      }
    }

    /// <summary>
    /// Adds a list of reduce by location positions to the spec for a parameter and location.
    /// </summary>
    /// <param name="complexParameter">The parameter to reduce./</param>
    /// <param name="locationPosition">The location to reduce.</param>
    /// <param name="reduceByPositions">The locations to reduce by.</param>
    public void AddReduceBy(eComplexParameter complexParameter, int locationPosition, int[] reduceByPositions)
    {
      ReduceBy.Add(complexParameter, locationPosition, reduceByPositions);
    }

    /// <summary>
    /// Adds a list of reduce by location positions to the spec for a parameter and location.
    /// </summary>
    /// <param name="complexParameter">The parameter to reduce./</param>
    /// <param name="locationName">The name of the location to reduce.</param>
    /// <param name="reduceByNames">The name of the locations to reduce by.</param>
    public void AddReduceBy(eComplexParameter complexParameter, string locationName, string[] reduceByNames)
    {
      int? locationDex;

      if (LocationInfo.GetPosition(locationName, out locationDex))
      {
        bool success = true;

        int[] reduceByPositions = new int[reduceByNames.Length];
        int reduceByCnt = 0;

        foreach (string reduceByName in reduceByNames)
        {
          int? reduceByDex;

          if (LocationInfo.GetPosition(reduceByName, out reduceByDex))
          {
            reduceByPositions[reduceByCnt] = reduceByDex.Value;
            reduceByCnt += 1;
          }
          else
            success = false;
        }

        if (success)
          ReduceBy.Add(complexParameter, locationDex.Value, reduceByPositions);
      }
    }

    /// <summary>
    /// Remove a list of reduce by location positions.
    /// </summary>
    /// <param name="complexParameter">The parameter to reduce.</param>
    /// <param name="locationPosition">The location to reduce.</param>
    public void RemoveReduceBy(eComplexParameter complexParameter, int locationPosition)
    {
      ReduceBy.Remove(complexParameter, locationPosition);
    }

    /// <summary>
    /// Remove a list of reduce by location positions.
    /// </summary>
    /// <param name="complexParameter">The parameter to reduce.</param>
    /// <param name="locationName">The name of the location to reduce.</param>
    public void RemoveReduceBy(eComplexParameter complexParameter, string locationName)
    {
      int? locationDex;

      if (LocationInfo.GetPosition(locationName, out locationDex))
      {
        ReduceBy.Remove(complexParameter, locationDex.Value);
      }
    }

    #endregion


    #region Public Methods: Miscellaneous

    /// <summary>
    /// Replaces location names in a formula with location positions.
    /// </summary>
    /// <param name="formula">The formula to manipulate.</param>
    /// <returns></returns>
    public string ChangeLocationNamesToPositions(string formula)
    {
      string result;

      result = formula;

      for (int locationDex = 0; locationDex < LocationInfo.Length; locationDex++)
      {
        result = result.Replace("[" + LocationInfo[locationDex].LocationName + "]", "<" + locationDex.ToString() + ">");
      }

      for (int locationDex = 0; locationDex < LocationInfo.Length; locationDex++)
      {
        result = result.Replace("<" + locationDex.ToString() + ">", "[" + locationDex.ToString() + "]");
      }

      return result;
    }

    /// <summary>
    /// Check all the HI/Load formulas against the passed factors to determine whether any error occurs.
    /// </summary>
    /// <param name="factors">The factors to test.</param>
    /// <param name="errorResult">Calculation error result.</param>
    /// <returns>True if no formulas failed.</returns>
    public bool CheckHiLoadFormulae(decimal?[] factors, out cCalculator.eCalculatorError? errorResult)
    {
      bool result = true;

      errorResult = null;

      foreach (cFactorFormulaeGrid factorFormulaGrid in HiLoadFormulaeGrids)
        for (int unitDex = 0; unitDex < UnitInfo.Length; unitDex++)
          for (int locationDex = 0; locationDex < LocationInfo.Length; locationDex++)
          {
            string formula = factorFormulaGrid.GetFormula(unitDex, locationDex);

            if (!string.IsNullOrEmpty(formula))
            {
              cCalculator.eCalculatorError? tempResult;

              if (!cCalculator.EvaluateReversePolish(formula, factors, out tempResult).HasValue)
              {
                errorResult = tempResult;
                result = false;
              }
            }
          }

      return result;
    }

    /// <summary>
    /// 
    /// </summary>
    public void SetUsedInHiLoadFormula()
    {
      // Set Used in Factor Formulae
      for (int locationDex = 0; locationDex < LocationCount; locationDex++)
        UsedInHiLoadFormula[locationDex] = false;

      for (int parameterDex = 0; parameterDex < ComplexParameterCount; parameterDex++)
        for (int unitDex = 0; unitDex < UnitCount; unitDex++)
          for (int locationDex = 0; locationDex < LocationCount; locationDex++)
          {
            bool[] usedInFormula = cCalculator.GetIndexUse(HiLoadFormulaeGrids[parameterDex].GetFormula(unitDex, locationDex));
            int usedCount = Math.Min(usedInFormula.Length, LocationCount);

            for (int usedDex = 0; usedDex < usedCount; usedDex++)
              UsedInHiLoadFormula[usedDex] = true;
          }
    }

    #endregion

  }
}
