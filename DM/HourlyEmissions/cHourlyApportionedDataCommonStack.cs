using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECMPS.DM.Definitions;
using ECMPS.DM.Utilities;

namespace ECMPS.DM.HourlyEmissions
{

  /// <summary>
  /// Handles apportionment for common stack and multiple common stack emissions reports.
  /// </summary>
  public class cHourlyApportionedDataCommonStack : cHourlyApportionedData
  {

    #region Public Constructors

    /// <summary>
    /// Creates an instance of the Unit Hourly Apportioned Data class.
    /// </summary>
    /// <param name="hourlyRawData">The raw data on which the apportionment will occur.</param>
    public cHourlyApportionedDataCommonStack(cHourlyRawData hourlyRawData)
      : base(hourlyRawData)
    {
    }

    #endregion


    #region Public Override Properties

    /// <summary>
    /// The apportionment type of the class.
    /// </summary>
    public override eApportionmentType ApportionmentType { get { return eApportionmentType.CommonStack; } }

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

        bool[] so2mApportionValue;
        bool[] co2mApportionValue;
        bool[] noxmApportionValue;
        bool[] noxrCalculateValue;

        bool[] hgmApportionValue;
        bool[] hclmApportionValue;
        bool[] hfmApportionValue;

        decimal? so2mSubtract;
        decimal? co2mSubtract;
        decimal? noxmSubtract;

        decimal? hgmSubtract;
        decimal? hclmSubtract;
        decimal? hfmSubtract;

        decimal? so2mCommon; string so2mCommonMeasure;
        decimal? co2mCommon; string co2mCommonMeasure;
        decimal? noxmCommon; string noxmCommonMeasure;
        decimal? noxrCommon; string noxrCommonMeasure;
        
        decimal? hgmCommon; string hgmCommonMeasure;
        decimal? hclmCommon; string hclmCommonMeasure;
        decimal? hfmCommon; string hfmCommonMeasure;

        bool commonHasValues;

        GetCombinedValues(rawHourOffset.Value,
                          out commonHasValues,
                          out so2mCommon, out so2mCommonMeasure,
                          out co2mCommon, out co2mCommonMeasure,
                          out noxmCommon, out noxmCommonMeasure,
                          out noxrCommon, out noxrCommonMeasure,
                          out hgmCommon, out hgmCommonMeasure,
                          out hclmCommon, out hclmCommonMeasure,
                          out hfmCommon, out hfmCommonMeasure);


        if (commonHasValues)
        {
          // Get the apportionment factors
          GetApportionFactors(rawHourOffset.Value,
                              out massFactors,
                              out so2mApportionValue,
                              out so2mSubtract,
                              out co2mApportionValue,
                              out co2mSubtract,
                              out noxmApportionValue,
                              out noxmSubtract,
                              out noxrCalculateValue,
                              out hgmApportionValue,
                              out hgmSubtract,
                              out hclmApportionValue,
                              out hclmSubtract,
                              out hfmApportionValue,
                              out hfmSubtract);

          // Op Time, GLoad, SLoad and TLoad
          CopyOpData(apportionHourOffset.Value, rawHourOffset.Value);

          // HIT Value, Method and Measure
          CopyHit(apportionHourOffset.Value, rawHourOffset.Value);

          // SO2M Value and Measure Flag
          ApportionMass(So2mArray, So2mMeasureArray,
                        HourlyRawData.So2mArray, HourlyRawData.So2mMeasureArray,
                        so2mApportionValue, so2mSubtract,
                        so2mCommon, so2mCommonMeasure,
                        cDecimalPlaces.So2m,
                        massFactors, apportionHourOffset.Value, rawHourOffset.Value);

          // SO2R Value and Measure Flag - Always after apportioning SO2M and setting HIT
          ApportionRate(So2rArray,
                        So2rMeasureArray,
                        So2mArray,
                        HitArray,
                        cDecimalPlaces.So2r,
                        apportionHourOffset.Value);

          // CO2M Value and Measure Flag
          ApportionMass(Co2mArray, Co2mMeasureArray,
                        HourlyRawData.Co2mArray, HourlyRawData.Co2mMeasureArray,
                        co2mApportionValue, co2mSubtract,
                        co2mCommon, co2mCommonMeasure,
                        cDecimalPlaces.Co2m,
                        massFactors, apportionHourOffset.Value, rawHourOffset.Value);

          // CO2R Value and Measure Flag - Always after apportioning CO2M and setting HIT
          ApportionRate(Co2rArray,
                        Co2rMeasureArray,
                        Co2mArray,
                        HitArray,
                        cDecimalPlaces.Co2r,
                        apportionHourOffset.Value);

          // NoxM Value and Measure Flag
          ApportionMass(NoxmArray, NoxmMeasureArray,
                        HourlyRawData.NoxmArray, HourlyRawData.NoxmMeasureArray,
                        noxmApportionValue, noxmSubtract,
                        noxmCommon, noxmCommonMeasure,
                        cDecimalPlaces.Noxm,
                        massFactors, apportionHourOffset.Value, rawHourOffset.Value);

          // NOXR Value and Measure Flag - Always after apportioning NOXM and setting HIT
          ApportionNoxr(noxrCalculateValue,
                        noxrCommon, noxrCommonMeasure,
                        apportionHourOffset.Value, rawHourOffset.Value);

          // Try to calculate missing NOxM using calculated NOxR - Always after calcualting NOxR 
          RecheckNoxm(apportionHourOffset.Value);

          // Hg Mass Value and Measure Code
          ApportionMass(HgMassArray, HgMeasureArray,
                        HourlyRawData.HgMassArray, HourlyRawData.HgMeasureArray,
                        hgmApportionValue, hgmSubtract,
                        hgmCommon, hgmCommonMeasure,
                        cDecimalPlaces.Hgm,
                        massFactors, apportionHourOffset.Value, rawHourOffset.Value);

          // HCl Mass Value and Measure Code
          ApportionMass(HclMassArray, HclMeasureArray,
                        HourlyRawData.HclMassArray, HourlyRawData.HclMeasureArray,
                        hclmApportionValue, hclmSubtract,
                        hclmCommon, hclmCommonMeasure,
                        cDecimalPlaces.Hclm,
                        massFactors, apportionHourOffset.Value, rawHourOffset.Value);

          // HF Mass Value and Measure Code
          ApportionMass(HfMassArray, HfMeasureArray,
                        HourlyRawData.HfMassArray, HourlyRawData.HfMeasureArray,
                        hfmApportionValue, hfmSubtract,
                        hfmCommon, hfmCommonMeasure,
                        cDecimalPlaces.Hfm,
                        massFactors, apportionHourOffset.Value, rawHourOffset.Value);

          // Calculate and fill missing electrical output and heat input based rates.
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
    /// Apportions (combined) common stack mass among the units.
    /// </summary>
    /// <param name="apportionValueArray">The array for the mass's apportioned values.</param>
    /// <param name="apportionMeasureArray">The array for the mass's apporioned measure code.</param>
    /// <param name="rawValueArray">The array for the mass's raw values.</param>
    /// <param name="rawMeasureArray">The array for the mass's raw measure codes.</param>
    /// <param name="apportionValue">A boolean array indicating whether a particular unit should receive apportioned values.</param>
    /// <param name="subtractValue">The sum of the mass reported at units.</param>
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
                               bool[] apportionValue,
                               decimal? subtractValue,
                               decimal? combinedValue,
                               string combinedMeasure,
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
          {
            if (apportionValue[unitDex])
              factorTotal += massFactors[unitDex].Value;
          }
        }
      }

      for (int unitDex = 0; unitDex < UnitCount; unitDex++)
      {
        int apportionPos = unitDex + apportionHourOffset;
        int rawPos = UnitInfo[unitDex].LocationPosition.Value + rawHourOffset;

        if (combinedValue.HasValue && apportionValue[unitDex] && (factorTotal != 0))
        {
          decimal adjustedValue = (subtractValue.HasValue)
                                ? combinedValue.Value - subtractValue.Value
                                : combinedValue.Value;

          apportionValueArray[apportionPos]
            = Math.Round(adjustedValue * (massFactors[unitDex].Value / factorTotal),
                         decimalPlaces,
                         MidpointRounding.AwayFromZero);

          apportionMeasureArray[apportionPos] = combinedMeasure;
        }
        else
        {
          apportionValueArray[apportionPos] = rawValueArray[rawPos];
          apportionMeasureArray[apportionPos] = rawMeasureArray[rawPos];
        }
      }
    }

    /// <summary>
    /// Apportions (combined) common stack NOXR among the units.
    /// </summary>
    /// <param name="calculateValue">A boolean array indicating whether a particular unit should receive calculated values.</param>
    /// <param name="combinedValue">The (combined) common stack NOXR value.</param>
    /// <param name="combinedMeasure">The (combined) common stack NOXR measure code.</param>
    /// <param name="apportionHourOffset">Hour offset into the apportionment data arrays.</param>
    /// <param name="rawHourOffset">Hour offset into the raw daa arrays.</param>
    private void ApportionNoxr(bool[] calculateValue,
                               decimal? combinedValue,
                               string combinedMeasure,
                               int apportionHourOffset,
                               int rawHourOffset)
    {
      for (int unitDex = 0; unitDex < UnitCount; unitDex++)
      {
        int apportionPos = unitDex + apportionHourOffset;
        int rawPos = UnitInfo[unitDex].LocationPosition.Value + rawHourOffset;

        if (calculateValue[unitDex])
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
          else if (combinedValue.HasValue)
          {
            NoxrArray[apportionPos]
              = Math.Round(combinedValue.Value,
                           cDecimalPlaces.Noxr,
                           MidpointRounding.AwayFromZero);

            NoxrMeasureArray[apportionPos] = combinedMeasure;
          }
          else
          {
            NoxrArray[apportionPos] = HourlyRawData.NoxrArray[rawPos];
            NoxrMeasureArray[apportionPos] = HourlyRawData.NoxrMeasureArray[rawPos];
          }
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
    /// <param name="so2mApportionValue">A boolean array indicating whether a particular unit should receive apportioned values.</param>
    /// <param name="so2mSubtract">The sum of the value reported at units.</param>
    /// <param name="co2mApportionValue">A boolean array indicating whether a particular unit should receive apportioned values.</param>
    /// <param name="co2mSubtract">The sum of the value reported at units.</param>
    /// <param name="noxmApportionValue">A boolean array indicating whether a particular unit should receive apportioned values.</param>
    /// <param name="noxmSubtract">The sum of the value reported at units.</param>
    /// <param name="noxrCalculateValue">A boolean array indicating whether a particular unit should receive apportioned values.</param>
    /// <param name="hgmApportionValue">A boolean array indicating whether a particular unit should receive apportioned values.</param>
    /// <param name="hgmSubtract">The sum of the value reported at units.</param>
    /// <param name="hclmApportionValue">A boolean array indicating whether a particular unit should receive apportioned values.</param>
    /// <param name="hclmSubtract">The sum of the value reported at units.</param>
    /// <param name="hfmApportionValue">A boolean array indicating whether a particular unit should receive apportioned values.</param>
    /// <param name="hfmSubtract">The sum of the value reported at units.</param>
    private void GetApportionFactors(int rawHourOffset,
                                     out decimal?[] massFactors,
                                     out bool[] so2mApportionValue,
                                     out decimal? so2mSubtract,
                                     out bool[] co2mApportionValue,
                                     out decimal? co2mSubtract,
                                     out bool[] noxmApportionValue,
                                     out decimal? noxmSubtract,
                                     out bool[] noxrCalculateValue,
                                     out bool[] hgmApportionValue,
                                     out decimal? hgmSubtract,
                                     out bool[] hclmApportionValue,
                                     out decimal? hclmSubtract,
                                     out bool[] hfmApportionValue,
                                     out decimal? hfmSubtract)
    {
      so2mApportionValue = new bool[UnitCount];
      so2mSubtract = 0;
      co2mApportionValue = new bool[UnitCount];
      co2mSubtract = 0;
      noxmApportionValue = new bool[UnitCount];
      noxmSubtract = 0;
      noxrCalculateValue = new bool[UnitCount];

      hgmApportionValue = new bool[UnitCount];
      hgmSubtract = 0;
      hclmApportionValue = new bool[UnitCount];
      hclmSubtract = 0;
      hfmApportionValue = new bool[UnitCount];
      hfmSubtract = 0;

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
          hitFactors[unitDex] = HourlyRawData.HitArray[rawPos];
          gLoadFactors[unitDex] = HourlyRawData.GLoadArray[rawPos];
          sLoadFactors[unitDex] = HourlyRawData.SLoadArray[rawPos];
          tLoadFactors[unitDex] = HourlyRawData.TLoadArray[rawPos];
          opTimeFactors[unitDex] = HourlyRawData.OpTimeArray[rawPos];

          bool mustUseUnit = false;

          if (HourlyRawData.So2mArray[rawPos].HasValue)
          {
            so2mSubtract += HourlyRawData.So2mArray[rawPos].Value;
            so2mApportionValue[unitDex] = false;
          }
          else
          {
            so2mApportionValue[unitDex] = true;
            mustUseUnit = true;
          }

          if (HourlyRawData.Co2mArray[rawPos].HasValue)
          {
            co2mSubtract += HourlyRawData.Co2mArray[rawPos].Value;
            co2mApportionValue[unitDex] = false;
          }
          else
          {
            co2mApportionValue[unitDex] = true;
            mustUseUnit = true;
          }

          if (HourlyRawData.NoxmArray[rawPos].HasValue)
          {
            noxmSubtract += HourlyRawData.NoxmArray[rawPos].Value;
            noxmApportionValue[unitDex] = false;
          }
          else if (HourlyRawData.HitArray[rawPos].HasValue &&
                   HourlyRawData.NoxrArray[rawPos].HasValue)
          {
            noxmApportionValue[unitDex] = false;
            noxmSubtract += Math.Round(HourlyRawData.HitArray[rawPos].Value * HourlyRawData.NoxrArray[rawPos].Value,
                                       cDecimalPlaces.Noxm,
                                       MidpointRounding.AwayFromZero);
          }
          else
          {
            noxmApportionValue[unitDex] = true;
            mustUseUnit = true;
          }

          noxrCalculateValue[unitDex] = !HourlyRawData.NoxrArray[rawPos].HasValue;

          /* Hg */
          if (HourlyRawData.HgMassArray[rawPos].HasValue)
          {
            hgmSubtract += HourlyRawData.HgMassArray[rawPos].Value;
            hgmApportionValue[unitDex] = false;
          }
          else
          {
            hgmApportionValue[unitDex] = true;
            mustUseUnit = true;
          }

          /* HCl */
          if (HourlyRawData.HclMassArray[rawPos].HasValue)
          {
            hclmSubtract += HourlyRawData.HclMassArray[rawPos].Value;
            hclmApportionValue[unitDex] = false;
          }
          else
          {
            hclmApportionValue[unitDex] = true;
            mustUseUnit = true;
          }

          /* HF */
          if (HourlyRawData.HfMassArray[rawPos].HasValue)
          {
            hfmSubtract += HourlyRawData.HfMassArray[rawPos].Value;
            hfmApportionValue[unitDex] = false;
          }
          else
          {
            hfmApportionValue[unitDex] = true;
            mustUseUnit = true;
          }

          if (mustUseUnit)
          {
            sumHit += hitFactors[unitDex].GetValueOrDefault(0);
            sumGLoad += gLoadFactors[unitDex].GetValueOrDefault(0);
            sumSLoad += sLoadFactors[unitDex].GetValueOrDefault(0);
            sumTLoad += tLoadFactors[unitDex].GetValueOrDefault(0);

            if (!hitFactors[unitDex].HasValue) canUseHit = false;
            if (!gLoadFactors[unitDex].HasValue) canUseGLoad = false;
            if (!sLoadFactors[unitDex].HasValue) canUseSLoad = false;
            if (!tLoadFactors[unitDex].HasValue) canUseTLoad = false;
          }
        }
        else
        {
          opTimeFactors[unitDex] = 0; // Since this is the default, explicitly set all elements
          so2mApportionValue[unitDex] = false;
          co2mApportionValue[unitDex] = false;
          noxmApportionValue[unitDex] = false;
          noxrCalculateValue[unitDex] = false;
          hgmApportionValue[unitDex] = false;
          hclmApportionValue[unitDex] = false;
          hfmApportionValue[unitDex] = false;
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
    /// <param name="noxrCombinedValue">The (combined) common value.</param>
    /// <param name="noxrCombinedMeasure">The (combined) common measure flag.</param>
    /// <param name="hgmCombinedValue">The (combined) common value.</param>
    /// <param name="hgmCombinedMeasure">The (combined) common measure flag.</param>
    /// <param name="hclmCombinedValue">The (combined) common value.</param>
    /// <param name="hclmCombinedMeasure">The (combined) common measure flag.</param>
    /// <param name="hfmCombinedValue">The (combined) common value.</param>
    /// <param name="hfmCombinedMeasure">The (combined) common measure flag.</param>
    private void GetCombinedValues(int rawHourOffset,
                                   out bool hasValues,
                                   out decimal? so2mCombinedValue,
                                   out string so2mCombinedMeasure,
                                   out decimal? co2mCombinedValue,
                                   out string co2mCombinedMeasure,
                                   out decimal? noxmCombinedValue,
                                   out string noxmCombinedMeasure,
                                   out decimal? noxrCombinedValue,
                                   out string noxrCombinedMeasure,
                                   out decimal? hgmCombinedValue,
                                   out string hgmCombinedMeasure,
                                   out decimal? hclmCombinedValue,
                                   out string hclmCombinedMeasure,
                                   out decimal? hfmCombinedValue,
                                   out string hfmCombinedMeasure)
    {
      hasValues = false;

      so2mCombinedValue = null; 
      so2mCombinedMeasure = null;
      co2mCombinedValue = null; 
      co2mCombinedMeasure = null;
      noxmCombinedValue = null; 
      noxmCombinedMeasure = null;
      noxrCombinedValue = null; 
      noxrCombinedMeasure = null;

      hgmCombinedValue = null;
      hgmCombinedMeasure = null;
      hclmCombinedValue = null;
      hclmCombinedMeasure = null;
      hfmCombinedValue = null;
      hfmCombinedMeasure = null;

      cCombineMeasure so2mMeasureObject = new cCombineMeasure();
      cCombineMeasure co2mMeasureObject = new cCombineMeasure();
      cCombineMeasure noxmMeasureObject = new cCombineMeasure();
      cCombineMeasure noxrMeasureObject = new cCombineMeasure();


      int noxrCount = 0;
      decimal noxrSum = 0;
      decimal noxrWeighted = 0;
      decimal noxrHit = 0;
      bool noxrCanWeight = true;

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

            so2mMeasureObject.CombineAndSet(HourlyRawData.So2mMeasureArray[dataPos], out so2mCombinedMeasure);

            hasValues = true;
          }

          // CO2M
          if (HourlyRawData.Co2mArray[dataPos].HasValue)
          {
            if (co2mCombinedValue.HasValue)
              co2mCombinedValue += HourlyRawData.Co2mArray[dataPos].Value;
            else
              co2mCombinedValue = HourlyRawData.Co2mArray[dataPos].Value;

            co2mMeasureObject.CombineAndSet(HourlyRawData.Co2mMeasureArray[dataPos], out co2mCombinedMeasure);

            hasValues = true;
          }

          // NOxM
          if (HourlyRawData.NoxmArray[dataPos].HasValue)
          {
            if (noxmCombinedValue.HasValue)
              noxmCombinedValue += HourlyRawData.NoxmArray[dataPos].Value;
            else
              noxmCombinedValue = HourlyRawData.NoxmArray[dataPos].Value;

            noxmMeasureObject.CombineAndSet(HourlyRawData.NoxmMeasureArray[dataPos], out noxmCombinedMeasure);

            hasValues = true;
          }
          else if (HourlyRawData.HitArray[dataPos].HasValue &&
                   HourlyRawData.NoxrArray[dataPos].HasValue)
          {
            decimal noxmCalc = Math.Round(HourlyRawData.HitArray[dataPos].Value * HourlyRawData.NoxrArray[dataPos].Value,
                                          cDecimalPlaces.Noxm,
                                          MidpointRounding.AwayFromZero);

            if (noxmCombinedValue.HasValue)
              noxmCombinedValue += noxmCalc;
            else
              noxmCombinedValue = noxmCalc;

            noxmMeasureObject.CombineAndSet("CALC", out noxmCombinedMeasure);

            hasValues = true;
          }

          // NOxR
          if (HourlyRawData.NoxrArray[dataPos].HasValue)
          {
            noxrCount += 1;
            noxrSum += HourlyRawData.NoxrArray[dataPos].Value;

            if (noxrCanWeight)
            {
              if (HourlyRawData.HitArray[dataPos].HasValue)
              {
                noxrWeighted += Math.Round(HourlyRawData.HitArray[dataPos].Value * HourlyRawData.NoxrArray[dataPos].Value,
                                           cDecimalPlaces.Noxm,
                                           MidpointRounding.AwayFromZero);
                noxrHit += HourlyRawData.HitArray[dataPos].Value;
              }
              else
              {
                noxrCanWeight = false;
              }
            }

            noxrMeasureObject.CombineAndSet(HourlyRawData.NoxrMeasureArray[dataPos], out noxrCombinedMeasure);

            hasValues = true;
          }

          /* Combine Hg, HCl and HF mass values and measure codes */
          cCombineMeasure hgmMeasureObject = new cCombineMeasure();
          cCombineMeasure hclmMeasureObject = new cCombineMeasure();
          cCombineMeasure hfmMeasureObject = new cCombineMeasure();

          CombineMatsValue(ref hgmCombinedValue, ref hgmMeasureObject, ref hasValues, dataPos, HourlyRawData.HgMassArray, HourlyRawData.HgMeasureArray);
          CombineMatsValue(ref hclmCombinedValue, ref hclmMeasureObject, ref hasValues, dataPos, HourlyRawData.HclMassArray, HourlyRawData.HclMeasureArray);
          CombineMatsValue(ref hfmCombinedValue, ref hfmMeasureObject, ref hasValues, dataPos, HourlyRawData.HfMassArray, HourlyRawData.HfMeasureArray);

          hgmCombinedMeasure = hgmMeasureObject.GetCombined();
          hclmCombinedMeasure = hclmMeasureObject.GetCombined();
          hfmCombinedMeasure = hfmMeasureObject.GetCombined();
        }
      }

      // Finish NOxR
      {
        switch (noxrCount)
        {
          case 0:
            {
              noxrCombinedValue = null;
            }
            break;

          case 1:
            {
              noxrCombinedValue = noxrSum;
            }
            break;

          default:
            {
              if (noxrCanWeight)
              {
                noxrCombinedValue = Math.Round(noxrWeighted / noxrHit,
                                        cDecimalPlaces.Noxr,
                                        MidpointRounding.AwayFromZero);
              }
              else
              {
                noxrCombinedValue = Math.Round(noxrSum / noxrCount,
                                        cDecimalPlaces.Noxr,
                                        MidpointRounding.AwayFromZero);
              }
            }
            break;
        }
      }
    }

    #endregion

  }

}
