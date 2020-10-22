using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECMPS.DM.Definitions;
using ECMPS.DM.Miscellaneous;
using ECMPS.DM.Utilities;

namespace ECMPS.DM.HourlyEmissions
{
  /// <summary>
  /// Handles apportionment for Complex.
  /// </summary>
  public class cHourlyApportionedDataComplex : cHourlyApportionedData
  {
 
    #region Public Constructors

    /// <summary>
    /// Creates an instance of the Unit Hourly Apportioned Data class.
    /// </summary>
    /// <param name="hourlyRawData">The raw data on which the apportionment will occur.</param>
    /// <param name="factorFormulae">The factor formula for the complex configuration.</param>
    public cHourlyApportionedDataComplex(cHourlyRawData hourlyRawData, cFactorFormulae[] factorFormulae)
      : base(hourlyRawData)
    {
      FactorFormulaeList = factorFormulae;
    }

    #endregion


    #region Public Override Properties

    /// <summary>
    /// The apportionment type of the class.
    /// </summary>
    public override eApportionmentType ApportionmentType { get { return eApportionmentType.Complex; } }

    #endregion


    #region Public Properties

    /// <summary>
    /// The factor formulae list for the complex apportionment.
    /// </summary>
    public cFactorFormulae[] FactorFormulaeList { get; private set; }

    #endregion


    #region Public Override Methods

    /// <summary>
    /// The method that apportions an individual hour.
    /// </summary>
    /// <param name="date">The date of the hour to apportion.</param>
    /// <param name="hour">The hour to apportion.</param>
    /// <returns></returns>
    public override bool Apportion(DateTime date, int hour)
    {
      bool result;

      cFactorFormulae factorFormulae;
      int? rawHourOffset, apportionHourOffset;

      if (GetHourOffset(date, hour, out apportionHourOffset) &&
          HourlyRawData.GetHourOffset(date, hour, out rawHourOffset) &&
          GetFactorFormulae(date, hour, rawHourOffset.Value, out factorFormulae))
      {
        result = true;

        decimal?[] massFactors;
        bool useHiLoadFormulae;

        // Get Apportionment Factor Information
        GetApportionFactors(factorFormulae,
                            date, 
                            hour,
                            rawHourOffset.Value,
                            out massFactors,
                            out useHiLoadFormulae);

        // Op Time, GLoad and SLoad
        CopyOpData(apportionHourOffset.Value, rawHourOffset.Value);

        // HIT Value, Method and Measure
        CopyHit(apportionHourOffset.Value, rawHourOffset.Value);

        // SO2M Value, Method and Measure
        ApportionMass(So2mArray,
                      So2mMeasureArray,
                      HourlyRawData.So2mArray,
                      HourlyRawData.So2mMeasureArray,
                      HourlyRawData.OpTimeArray,
                      eComplexParameter.So2m,
                      cDecimalPlaces.So2m,
                      massFactors,
                      useHiLoadFormulae,
                      factorFormulae,
                      date, hour,
                      apportionHourOffset.Value,
                      rawHourOffset.Value);

        // SO2R Value and Measure Flag - Always after apportioning SO2M and setting HIT
        ApportionRate(So2rArray,
                      So2rMeasureArray,
                      So2mArray,
                      HitArray,
                      cDecimalPlaces.So2r,
                      apportionHourOffset.Value);

        // CO2M Value, Method and Measure
        ApportionMass(Co2mArray,
                      Co2mMeasureArray,
                      HourlyRawData.Co2mArray,
                      HourlyRawData.Co2mMeasureArray,
                      HourlyRawData.OpTimeArray,
                      eComplexParameter.Co2m,
                      cDecimalPlaces.Co2m,
                      massFactors,
                      useHiLoadFormulae,
                      factorFormulae,
                      date, hour,
                      apportionHourOffset.Value,
                      rawHourOffset.Value);

        // CO2R Value and Measure Flag - Always after apportioning CO2M and setting HIT
        ApportionRate(Co2rArray,
                      Co2rMeasureArray,
                      Co2mArray,
                      HitArray,
                      cDecimalPlaces.Co2r,
                      apportionHourOffset.Value);

        // NOxM Value, Method and Measure
        ApportionMass(NoxmArray,
                      NoxmMeasureArray,
                      HourlyRawData.NoxmArray,
                      HourlyRawData.NoxmMeasureArray,
                      HourlyRawData.OpTimeArray,
                      eComplexParameter.Noxm,
                      cDecimalPlaces.Noxm,
                      massFactors,
                      useHiLoadFormulae,
                      factorFormulae,
                      date, hour,
                      apportionHourOffset.Value,
                      rawHourOffset.Value);

        // NOxR Value, Method and Measure - Always after apportioning NOxM
        ApportionNoxr(apportionHourOffset.Value,
                      rawHourOffset.Value);

        // Try to calculate missing NOxM using calculated NOxR - Always after calcualting NOxR 
        RecheckNoxm(apportionHourOffset.Value);

        // Hg Mass Value and Measure Code
        ApportionMass(HgMassArray,
                      HgMeasureArray,
                      HourlyRawData.HgMassArray,
                      HourlyRawData.HgMeasureArray,
                      HourlyRawData.OpTimeArray,
                      eComplexParameter.Hgm,
                      cDecimalPlaces.Hgm,
                      massFactors,
                      useHiLoadFormulae,
                      factorFormulae,
                      date, hour,
                      apportionHourOffset.Value,
                      rawHourOffset.Value);

        // HCl Mass Value and Measure Code
        ApportionMass(HclMassArray,
                      HclMeasureArray,
                      HourlyRawData.HclMassArray,
                      HourlyRawData.HclMeasureArray,
                      HourlyRawData.OpTimeArray,
                      eComplexParameter.Hclm,
                      cDecimalPlaces.Hclm,
                      massFactors,
                      useHiLoadFormulae,
                      factorFormulae,
                      date, hour,
                      apportionHourOffset.Value,
                      rawHourOffset.Value);

        // HF Mass Value and Measure Code
        ApportionMass(HfMassArray,
                      HfMeasureArray,
                      HourlyRawData.HfMassArray,
                      HourlyRawData.HfMeasureArray,
                      HourlyRawData.OpTimeArray,
                      eComplexParameter.Hfm,
                      cDecimalPlaces.Hfm,
                      massFactors,
                      useHiLoadFormulae,
                      factorFormulae,
                      date, hour,
                      apportionHourOffset.Value,
                      rawHourOffset.Value);

        // Calculate and fill missing electrical output and heat input based rates.
        FillMissingMatsRates(apportionHourOffset.Value);
      }
      else
        result = false;

      return result;
    }

    #endregion


    #region Private Methods

    /// <summary>
    /// Apportions (combined) common stack mass among the units.
    /// </summary>
    /// <param name="apportionValueArray">The array for the mass's apportioned values.</param>
    /// <param name="apportionMeasureArray">The array for the mass's apporioned measure code.</param>
    /// <param name="rawValueArray">The array for the mass's raw values.</param>
    /// <param name="rawMeasureArray">The array for the mass's raw measure codes.</param>
    /// <param name="rawOpTimeArray">The array for the raw op time values.</param>
    /// <param name="complexParameter">Complex parameter type.</param>
    /// <param name="decimalPlaces">The rounding decimal places for the mass.</param>
    /// <param name="massFactors">An array of the unit factors to use in apportionment.</param>
    /// <param name="useHiLoadFormulae">If true use Hi/Load formulas over Op Time formulas.</param>
    /// <param name="factorFormulae">The factor formulae to apply.</param>
    /// <param name="date">The date of the hour to apportion.</param>
    /// <param name="hour">The hour to apportion.</param>
    /// <param name="apportionHourOffset">Hour offset into the apportionment data arrays.</param>
    /// <param name="rawHourOffset">Hour offset into the raw daa arrays.</param>
    private bool ApportionMass(decimal?[] apportionValueArray,
                               string[] apportionMeasureArray,
                               decimal?[] rawValueArray,
                               string[] rawMeasureArray,
                               decimal?[] rawOpTimeArray,
                               eComplexParameter complexParameter,
                               int decimalPlaces,
                               decimal?[] massFactors,
                               bool useHiLoadFormulae,
                               cFactorFormulae factorFormulae,
                               DateTime date, 
                               int hour,
                               int apportionHourOffset,
                               int rawHourOffset)
    {
      bool result = true;

      // Get the Factor Formula Grid
      cFactorFormulaeGrid formulaGrid;
      {
        if (useHiLoadFormulae)
        {
          formulaGrid = factorFormulae.HiLoadFormulaeGrids[(int)complexParameter];
        }
        else
        {
          formulaGrid = factorFormulae.OpTimeFormulaeGrids[(int)complexParameter];
        }
      }

      //Apportion Data
      for (int unitDex = 0; unitDex < UnitCount; unitDex++)
      {
        int apportionPos = unitDex + apportionHourOffset;
        int rawPos = UnitInfo[unitDex].LocationPosition.Value + rawHourOffset;

        if (!(rawOpTimeArray[rawPos].HasValue && (rawOpTimeArray[rawPos].Value > 0)))
        {
          apportionValueArray[apportionPos] = null;
          apportionMeasureArray[apportionPos] = null;
        }
        else if (rawValueArray[rawPos].HasValue)
        {
          apportionValueArray[apportionPos] = rawValueArray[rawPos];
          apportionMeasureArray[apportionPos] = rawMeasureArray[rawPos];
        }
        else
        {
          decimal? total = null;
          cCombineMeasure measure = new cCombineMeasure();

          for (int locationDex = 0; (locationDex < LocationCount) && result; locationDex++)
          {
            int dataPos = locationDex + rawHourOffset;

            if (rawValueArray[dataPos].HasValue)
            {
              string equation = formulaGrid.GetFormula(unitDex, locationDex);

              if (!string.IsNullOrEmpty(equation))
              {
                cCalculator.eCalculatorError? errorResult;

                decimal? factor = cCalculator.EvaluateReversePolish(equation, massFactors, out errorResult);

                if (factor.HasValue)
                {
                  decimal value = rawValueArray[dataPos].Value;

                  cLocationPositionList reduceByList = factorFormulae.ReduceBy.Get(complexParameter, locationDex);

                  foreach (int reduceLocationDex in reduceByList.LocationPositionList)
                  {
                    decimal? reduceByValue = rawValueArray[reduceLocationDex + rawHourOffset];

                    if (reduceByValue.HasValue)
                      value -= reduceByValue.Value;
                  }

                  if (total.HasValue)
                    total += factor.Value * value;
                  else
                    total = factor.Value * value;

                  measure.Combine(rawMeasureArray[dataPos]);
                }
                else
                {
                  total = null;

                  cErrorHandler.LogError(string.Format("Formula evaluation failed for unit '{0}' and location '{1}': {2} [{3}] ({4})",
                                                       UnitInfo[unitDex].LocationName,
                                                       LocationInfo[locationDex].LocationName,
                                                       errorResult,
                                                       equation,
                                                       "cHourlyApportionedData.GetDataPosition"),
                                         date, hour);

                  result = false;
                }
              }
            }
          }

          if (total.HasValue)
          {
            apportionValueArray[apportionPos] = Math.Round(total.Value, decimalPlaces, MidpointRounding.AwayFromZero);
            apportionMeasureArray[apportionPos] = measure.GetCombined();
          }
          else if (result)
          {
            apportionValueArray[apportionPos] = rawValueArray[rawPos];
            apportionMeasureArray[apportionPos] = rawMeasureArray[rawPos];
          }
          else
          {
            apportionValueArray[apportionPos] = null;
            apportionMeasureArray[apportionPos] = null;
          }
        }
      }

      return result;
    }

    private void ApportionNoxr(int apportionHourOffset,
                               int rawHourOffset)
    {
      for (int unitDex = 0; unitDex < UnitCount; unitDex++)
      {
        int apportionPos = unitDex + apportionHourOffset;
        int rawPos = UnitInfo[unitDex].LocationPosition.Value + rawHourOffset;

        if (HourlyRawData.NoxrArray[rawPos].HasValue)
        {
          NoxrArray[apportionPos] = HourlyRawData.NoxrArray[rawPos];
          NoxrMeasureArray[apportionPos] = HourlyRawData.NoxrMeasureArray[rawPos];
        }
        else
        {
          if (HitArray[apportionPos].HasValue && (HitArray[apportionPos].Value > 0) &&
              NoxmArray[apportionPos].HasValue)
          {
            NoxrArray[apportionPos]
              = Math.Round(NoxmArray[apportionPos].Value / HitArray[apportionPos].Value,
                           cDecimalPlaces.Noxr,
                           MidpointRounding.AwayFromZero);

            NoxrMeasureArray[apportionPos] = "CALC";
          }
          else
          {
            NoxrArray[apportionPos] = HourlyRawData.NoxrArray[rawPos];
            NoxrMeasureArray[apportionPos] = HourlyRawData.NoxrMeasureArray[rawPos];
          }
        }
      }
    }

    private void GetApportionFactors(cFactorFormulae factorFormulae,
                                     DateTime date,
                                     int hour,
                                     int rawHourOffset,
                                     out decimal?[] massFactors,
                                     out bool useHiLoadFormulae)
    {
      decimal?[] hitFactors = new decimal?[LocationCount];
      decimal?[] gLoadFactors = new decimal?[LocationCount];
      decimal?[] sLoadFactors = new decimal?[LocationCount];
      decimal?[] tLoadFactors = new decimal?[LocationCount];
      decimal?[] opTimeFactors = new decimal?[LocationCount];

      bool canUseHit = true;
      bool canUseGLoad = true;
      bool canUseSLoad = true;
      bool canUseTLoad = true;

      for (int locationDex = 0; locationDex < LocationCount; locationDex++)
      {
        int rawPos = locationDex + rawHourOffset;

        if (HourlyRawData.OpTimeArray[rawPos] > 0)
        {
          hitFactors[locationDex] = HourlyRawData.HitArray[rawPos];
          gLoadFactors[locationDex] = HourlyRawData.GLoadArray[rawPos];
          sLoadFactors[locationDex] = HourlyRawData.SLoadArray[rawPos];
          tLoadFactors[locationDex] = HourlyRawData.TLoadArray[rawPos];
          opTimeFactors[locationDex] = HourlyRawData.OpTimeArray[rawPos];

          if (factorFormulae.UsedInHiLoadFormula[locationDex])
          {
            if (!hitFactors[locationDex].HasValue) canUseHit = false;
            if (!gLoadFactors[locationDex].HasValue) canUseGLoad = false;
            if (!sLoadFactors[locationDex].HasValue) canUseSLoad = false;
            if (!tLoadFactors[locationDex].HasValue) canUseTLoad = false;
          }
        }
        else
        {
          hitFactors[locationDex] = 0;
          gLoadFactors[locationDex] = 0;
          sLoadFactors[locationDex] = 0;
          tLoadFactors[locationDex] = 0;
          opTimeFactors[locationDex] = 0;
        }
      }

      if (canUseHit && GetApportionFactors_CheckHiLoadFormulae(hitFactors, opTimeFactors, "HIT", date, hour, factorFormulae))
      {
        massFactors = hitFactors;
        useHiLoadFormulae = true;
      }
      else if (canUseGLoad && GetApportionFactors_CheckHiLoadFormulae(gLoadFactors, opTimeFactors, "G-Load", date, hour, factorFormulae))
      {
        massFactors = gLoadFactors;
        useHiLoadFormulae = true;
      }
      else if (canUseSLoad && GetApportionFactors_CheckHiLoadFormulae(sLoadFactors, opTimeFactors, "S-Load", date, hour, factorFormulae))
      {
        massFactors = sLoadFactors;
        useHiLoadFormulae = true;
      }
      else if (canUseTLoad && GetApportionFactors_CheckHiLoadFormulae(tLoadFactors, opTimeFactors, "T-Load", date, hour, factorFormulae))
      {
        massFactors = tLoadFactors;
        useHiLoadFormulae = true;
      }
      else
      {
        massFactors = opTimeFactors;
        useHiLoadFormulae = false;

        cErrorHandler.LogError(string.Format("Op Time used for formula factors: {0}-{1} ({2})",
                                             date.ToShortDateString(), hour,
                                             "cHourlyApportionedDataComplex.GetApportionFactors"),
                               date, hour);
      }
    }

    /// <summary>
    /// Check all the HI/Load formulas against the passed factors to determine whether any error occurs.
    /// </summary>
    /// <param name="factors">The factors to test.</param>
    /// <param name="opTimes">The op times for the locations.</param>
    /// <param name="factorType">The name of the factor being tested.</param>
    /// <param name="date">The name of the factor being tested.</param>
    /// <param name="hour">The name of the factor being tested.</param>
    /// <param name="factorFormulae">The name of the factor formulae being tested.</param>
    /// <returns>True if no formulas failed.</returns>
    public bool GetApportionFactors_CheckHiLoadFormulae(decimal?[] factors, 
                                                        decimal?[] opTimes,
                                                        string factorType,
                                                        DateTime date, int hour,
                                                        cFactorFormulae factorFormulae)
    {
      bool result = true;

      string errorMessage = null;

      for (int unitDex = 0; unitDex < UnitInfo.Length; unitDex++)
      {
        decimal? unitOpTime = opTimes[UnitInfo[unitDex].LocationPosition.Value];

        if (unitOpTime.HasValue && (unitOpTime.Value > 0))
        {
          for (int locationDex = 0; locationDex < LocationInfo.Length; locationDex++)
            foreach (cFactorFormulaeGrid factorFormulaGrid in factorFormulae.HiLoadFormulaeGrids)
            {
              string formula = factorFormulaGrid.GetFormula(unitDex, locationDex);

              if (!string.IsNullOrEmpty(formula))
              {
                cCalculator.eCalculatorError? errorResult;

                decimal? value = cCalculator.EvaluateReversePolish(formula, factors, out errorResult);

                if (!value.HasValue)
                {
                  errorMessage = errorResult.ToString();
                  result = false;
                }
              }
            }
        }
      }

      return result;
    }

    /// <summary>
    /// Returns the Factor Formulae to use with the current date and hour
    /// </summary>
    /// <param name="date">The date of the hour to apportion.</param>
    /// <param name="hour">The hour to apportion.</param>
    /// <param name="rawHourOffset">Hour offset into the raw daa arrays.</param>
    /// <param name="factorFormulae">The factor formula for the hour.</param>
    /// <returns>True if factor formulae were found.</returns>
    private bool GetFactorFormulae(DateTime date, int hour, int rawHourOffset, out cFactorFormulae factorFormulae)
    {
      bool result;

      factorFormulae = null;

      bool found = false;

      for (int formulaeDex = 0; (formulaeDex < FactorFormulaeList.Length) && !found; formulaeDex++)
      {
        if (((FactorFormulaeList[formulaeDex].BeganDate < date) ||
             ((FactorFormulaeList[formulaeDex].BeganDate == date) && (FactorFormulaeList[formulaeDex].BeganHour <= hour))) &&
            ((FactorFormulaeList[formulaeDex].EndedDate > date) ||
             ((FactorFormulaeList[formulaeDex].EndedDate == date) && (FactorFormulaeList[formulaeDex].EndedHour >= hour))))
        {
          bool operatingTest = true;

          cLocationBoolList operatingCondition = FactorFormulaeList[formulaeDex].OperatingCondition;

          for (int locationDex = 0; locationDex < LocationCount; locationDex++)
          {
            decimal? opTime = HourlyRawData.OpTimeArray[locationDex + rawHourOffset];
            bool operating = opTime.HasValue && (opTime.Value > 0);

            if (operatingCondition.Get(locationDex).HasValue &&
                (operatingCondition.Get(locationDex).Value != operating))
              operatingTest = false;
          }

          if (operatingTest)
          {
            factorFormulae = FactorFormulaeList[formulaeDex];
            found = true;
          }
        }
      }

      if (factorFormulae != null)
        result = true;
      else
      {
        cErrorHandler.LogError(string.Format("No complex formula covers hour {0}-{1} ({2})",
                                             date.ToShortDateString(), hour,
                                             "cHourlyApportionedData.GetDataPosition"),
                               date, hour);
        result = false;
      }

      return result;
    }

    #endregion

  }
}
