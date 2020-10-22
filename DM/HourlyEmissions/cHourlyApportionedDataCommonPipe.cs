using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECMPS.DM.Definitions;
using ECMPS.DM.Utilities;

namespace ECMPS.DM.HourlyEmissions
{

  /// <summary>
  /// Handles apportionment for common pipe and multiple common pipe emissions reports.
  /// </summary>
  public class cHourlyApportionedDataCommonPipe : cHourlyApportionedData
  {

    #region Public Constructors

    /// <summary>
    /// Creates an instance of the Unit Hourly Apportioned Data class.
    /// </summary>
    /// <param name="hourlyRawData">The raw data on which the apportionment will occur.</param>
    public cHourlyApportionedDataCommonPipe(cHourlyRawData hourlyRawData)
      : base(hourlyRawData)
    {
    }

    #endregion


    #region Public Override Properties

    /// <summary>
    /// The apportionment type of the class.
    /// </summary>
    public override eApportionmentType ApportionmentType { get { return eApportionmentType.CommonPipe; } }

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

      int? rawHourOffset, apportionHourOffset;

      if (GetHourOffset(date, hour, out apportionHourOffset) &&
          HourlyRawData.GetHourOffset(date, hour, out rawHourOffset))
      {
        decimal?[] massFactors;

        decimal? so2mCommon, co2mCommon, noxmCommon;

        cCombineMeasure so2mCommonMeasure, co2mCommonMeasure, noxmCommonMeasure;

        bool commonHasValues;

        GetCombinedValues(rawHourOffset.Value,
                          out commonHasValues,
                          out so2mCommon, out so2mCommonMeasure,
                          out co2mCommon, out co2mCommonMeasure,
                          out noxmCommon, out noxmCommonMeasure);

        if (commonHasValues)
        {
          GetApportionFactors(rawHourOffset.Value, out massFactors);

          // Op Time, GLoad, SLoad and TLoad
          CopyOpData(apportionHourOffset.Value, rawHourOffset.Value);

          // HIT Value, Method and Measure
          CopyHit(apportionHourOffset.Value, rawHourOffset.Value);

          // SO2M Value, Method and Measure
          ApportionMass(So2mArray, 
                        So2mMeasureArray,
                        HourlyRawData.So2mArray,
                        HourlyRawData.So2mMeasureArray,
                        so2mCommon,
                        so2mCommonMeasure,
                        cDecimalPlaces.So2m,
                        massFactors,
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
                        co2mCommon,
                        co2mCommonMeasure,
                        cDecimalPlaces.Co2m,
                        massFactors,
                        apportionHourOffset.Value,
                        rawHourOffset.Value);

          // CO2R Value and Measure Flag - Always after apportioning CO2M and setting HIT
          ApportionRate(Co2rArray,
                        Co2rMeasureArray,
                        Co2mArray,
                        HitArray,
                        cDecimalPlaces.Co2r,
                        apportionHourOffset.Value);

          // NoxM Value, Method and Measure
          ApportionMass(NoxmArray,
                        NoxmMeasureArray,
                        HourlyRawData.NoxmArray,
                        HourlyRawData.NoxmMeasureArray,
                        noxmCommon,
                        noxmCommonMeasure,
                        cDecimalPlaces.Noxm,
                        massFactors,
                        apportionHourOffset.Value,
                        rawHourOffset.Value);

          // NoxR Value, Method and Measure - Always after apportioning NOxM
          ApportionNoxr(apportionHourOffset.Value, rawHourOffset.Value);

          // Try to calculate missing NOxM using calculated NOxR - Always after calcualting NOxR 
          RecheckNoxm(apportionHourOffset.Value);

          /* Handle MATS values, which are not reported at pipes. */
          CopyMats(apportionHourOffset.Value, rawHourOffset.Value);
          FillMissingMatsRates(apportionHourOffset.Value);
        }
        else
        {
          CopyHour(apportionHourOffset.Value, rawHourOffset.Value);

          // Calculate and fill missing electrical output and heat input based rates.
          FillMissingMatsRates(apportionHourOffset.Value);
        }

        result = true;
      }
      else
        result = false;

      return result;
    }

    #endregion


    #region Private Methods

    /// <summary>
    /// Apportions (combined) common pipe mass among the units.
    /// </summary>
    /// <param name="apportionValueArray">The array for the mass's apportioned values.</param>
    /// <param name="apportionMeasureArray">The array for the mass's apporioned measure code.</param>
    /// <param name="rawValueArray">The array for the mass's raw values.</param>
    /// <param name="rawMeasureArray">The array for the mass's raw measure codes.</param>
    /// <param name="combinedValue">The (combined) common stack mass value.</param>
    /// <param name="combinedMeasure">The (combined) common stack mass measure code.</param>
    /// <param name="decimalPlaces">The rounding decimal places for the mass.</param>
    /// <param name="massFactors">An array of the unit factors to use in apportionment.</param>
    /// <param name="apportionHourOffset">Hour offset into the apportionment data arrays.</param>
    /// <param name="rawHourOffset">Hour offset into the raw daa arrays.</param>
    private void ApportionMass(decimal?[] apportionValueArray,
                               string[] apportionMeasureArray,
                               decimal?[] rawValueArray,
                               string[] rawMeasureArray,
                               decimal? combinedValue,
                               cCombineMeasure combinedMeasure,
                               int decimalPlaces,
                               decimal?[] massFactors,
                               int apportionHourOffset,
                               int rawHourOffset)
    {
      decimal factorTotal = 0;
      {
        if (combinedValue.HasValue)
        {
          for (int unitDex = 0; unitDex < UnitCount; unitDex++)
            if (massFactors[unitDex].HasValue)
              factorTotal += massFactors[unitDex].Value;
        }
      }

      for (int unitDex = 0; unitDex < UnitCount; unitDex++)
      {
        int apportionPos = unitDex + apportionHourOffset;
        int rawPos = UnitInfo[unitDex].LocationPosition.Value + rawHourOffset;

        if (combinedValue.HasValue && (factorTotal != 0))
        {
          if (rawValueArray[rawPos].HasValue)
          {
            apportionValueArray[apportionPos]
              = Math.Round(rawValueArray[rawPos].Value
                             + (combinedValue.Value * (massFactors[unitDex].Value / factorTotal)),
                           decimalPlaces,
                           MidpointRounding.AwayFromZero);

            apportionMeasureArray[apportionPos] = combinedMeasure.Preview(rawMeasureArray[rawPos]);
          }
          else if (massFactors[unitDex].HasValue)
          {
            apportionValueArray[apportionPos]
              = Math.Round(combinedValue.Value * (massFactors[unitDex].Value / factorTotal),
                           decimalPlaces,
                           MidpointRounding.AwayFromZero);

            apportionMeasureArray[apportionPos] = combinedMeasure.Result;
          }
          else
          {
            apportionValueArray[apportionPos] = rawValueArray[rawPos];
            apportionMeasureArray[apportionPos] = rawMeasureArray[rawPos];
          }
        }
        else
        {
          apportionValueArray[apportionPos] = rawValueArray[rawPos];
          apportionMeasureArray[apportionPos] = rawMeasureArray[rawPos];
        }
      }
    }

    /// <summary>
    /// Apportions (combined) common pipe NOXR among the units.
    /// </summary>
    /// <param name="apportionHourOffset">Hour offset into the apportionment data arrays.</param>
    /// <param name="rawHourOffset">Hour offset into the raw daa arrays.</param>
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
        else if (HitArray[apportionPos].HasValue && (HitArray[apportionPos].Value > 0) &&
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

    /// <summary>
    /// Returns the apportionment factors, apportion indicators and subtractive values for an hour.
    /// </summary>
    /// <param name="rawHourOffset">Hour offset into the raw daa arrays.</param>
    /// <param name="massFactors">The apportionment factors for the hour.</param>
    private void GetApportionFactors(int rawHourOffset,
                                     out decimal?[] massFactors)
    {
      decimal?[] hitFactors = new decimal?[UnitCount];
      decimal?[] gLoadFactors = new decimal?[UnitCount];
      decimal?[] sLoadFactors = new decimal?[UnitCount];
      decimal?[] tLoadFactors = new decimal?[UnitCount];
      decimal?[] opTimeFactors = new decimal?[UnitCount];

      bool canUseHit = true;
      bool canUseGLoad = true;
      bool canUseSLoad = true;
      bool canUseTLoad = true;

      decimal sumHit = 0;
      decimal sumGLoad = 0;
      decimal sumSLoad = 0;
      decimal sumTLoad = 0;

      for (int unitDex = 0; unitDex < UnitCount; unitDex++)
      {
        int rawPos = UnitInfo[unitDex].LocationPosition.Value + rawHourOffset;
        int locationPos = UnitInfo[unitDex].LocationPosition.Value;

        if (HourlyRawData.OpTimeArray[rawPos] > 0)
        {
          if (HourlyRawData.HitArray[rawPos].HasValue &&
              HourlyRawData.HitffArray[rawPos].HasValue)
            hitFactors[unitDex] = HourlyRawData.HitArray[rawPos].Value
                                - HourlyRawData.HitffArray[rawPos].Value;
          else
            hitFactors[unitDex] = HourlyRawData.HitArray[rawPos];

          gLoadFactors[unitDex] = HourlyRawData.GLoadArray[rawPos];
          sLoadFactors[unitDex] = HourlyRawData.SLoadArray[rawPos];
          tLoadFactors[unitDex] = HourlyRawData.TLoadArray[rawPos];
          opTimeFactors[unitDex] = HourlyRawData.OpTimeArray[rawPos];

          sumHit += hitFactors[unitDex].GetValueOrDefault(0);
          sumGLoad += gLoadFactors[unitDex].GetValueOrDefault(0);
          sumSLoad += sLoadFactors[unitDex].GetValueOrDefault(0);
          sumTLoad += tLoadFactors[unitDex].GetValueOrDefault(0);

          if (!hitFactors[unitDex].HasValue) canUseHit = false;
          if (!gLoadFactors[unitDex].HasValue) canUseGLoad = false;
          if (!sLoadFactors[unitDex].HasValue) canUseSLoad = false;
          if (!tLoadFactors[unitDex].HasValue) canUseTLoad = false;
        }
        else
        {
          opTimeFactors[unitDex] = 0; // Since this is the default, explicitly set all elements
        }
      }

      if (canUseHit && (sumHit > 0))
        massFactors = hitFactors;
      else if (canUseGLoad && (sumGLoad > 0))
        massFactors = gLoadFactors;
      else if (canUseSLoad && (sumSLoad > 0))
        massFactors = sLoadFactors;
      else if (canUseTLoad && (sumTLoad > 0))
        massFactors = tLoadFactors;
      else
        massFactors = opTimeFactors;
    }

    /// <summary>
    /// Gets the (combined) common stack values and measure flags.
    /// </summary>
    /// <param name="rawHourOffset">Hour offset into the raw daa arrays.</param>
    /// <param name="hasValues">Indicates that combined values exist.</param>
    /// <param name="so2mCombinedValue">The (combined) common value.</param>
    /// <param name="so2mCombinedMeasure">The (combined) common measure flag.</param>
    /// <param name="co2mCombinedValue">The (combined) common value.</param>
    /// <param name="co2mCombinedMeasure">The (combined) common measure flag.</param>
    /// <param name="noxmCombinedValue">The (combined) common value.</param>
    /// <param name="noxmCombinedMeasure">The (combined) common measure flag.</param>
    private void GetCombinedValues(int rawHourOffset,
                                   out bool hasValues,
                                   out decimal? so2mCombinedValue,
                                   out cCombineMeasure so2mCombinedMeasure,
                                   out decimal? co2mCombinedValue,
                                   out cCombineMeasure co2mCombinedMeasure,
                                   out decimal? noxmCombinedValue,
                                   out cCombineMeasure noxmCombinedMeasure)
    {
      hasValues = false;

      so2mCombinedValue = null;
      so2mCombinedMeasure = new cCombineMeasure();

      co2mCombinedValue = null;
      co2mCombinedMeasure = new cCombineMeasure();

      noxmCombinedValue = null;
      noxmCombinedMeasure = new cCombineMeasure();


      for (int locationDex = 0; locationDex < HourlyRawData.LocationCount; locationDex++)
      {
        if (!HourlyRawData.LocationInfo[locationDex].UnitKey.HasValue) //not unit
        {
          int dataPos = locationDex + rawHourOffset;

          // SO2M
          if (HourlyRawData.So2mArray[dataPos].HasValue)
          {
            if (so2mCombinedValue.HasValue)
              so2mCombinedValue += HourlyRawData.So2mArray[dataPos].Value;
            else
              so2mCombinedValue = HourlyRawData.So2mArray[dataPos].Value;

            so2mCombinedMeasure.Combine(HourlyRawData.So2mMeasureArray[dataPos]);

            hasValues = true;
          }

          // CO2M
          if (HourlyRawData.Co2mArray[dataPos].HasValue)
          {
            if (co2mCombinedValue.HasValue)
              co2mCombinedValue += HourlyRawData.Co2mArray[dataPos].Value;
            else
              co2mCombinedValue = HourlyRawData.Co2mArray[dataPos].Value;

            co2mCombinedMeasure.Combine(HourlyRawData.Co2mMeasureArray[dataPos]);

            hasValues = true;
          }

          // NOxM
          if (HourlyRawData.NoxmArray[dataPos].HasValue)
          {
            if (noxmCombinedValue.HasValue)
              noxmCombinedValue += HourlyRawData.NoxmArray[dataPos].Value;
            else
              noxmCombinedValue = HourlyRawData.NoxmArray[dataPos].Value;

            noxmCombinedMeasure.Combine(HourlyRawData.NoxmMeasureArray[dataPos]);

            hasValues = true;
          }
        }
      }
    }

    #endregion

  }

}
