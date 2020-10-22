using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECMPS.DM.Definitions;
using ECMPS.DM.Utilities;

namespace ECMPS.DM.HourlyEmissions
{

  /// <summary>
  /// Handles apportionment for multiple stack emissions reports.
  /// </summary>
  public class cHourlyApportionedDataMultipleStack : cHourlyApportionedData
  {

    #region Public Constructors

    /// <summary>
    /// Creates an instance of the Unit Hourly Apportioned Data class.
    /// </summary>
    /// <param name="hourlyRawData">The raw data on which the apportionment will occur.</param>
    public cHourlyApportionedDataMultipleStack(cHourlyRawData hourlyRawData)
      : base(hourlyRawData)
    {
    }

    #endregion


    #region Public Override Properties

    /// <summary>
    /// The apportionment type of the class.
    /// </summary>
    public override eApportionmentType ApportionmentType { get { return eApportionmentType.MultipleStack; } }

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
        bool so2mApportionValue;
        bool co2mApportionValue;
        bool noxmApportionValue;
        bool noxrCalculateValue;

        bool hgmApportionValue;
        bool hclmApportionValue;
        bool hfmApportionValue;

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
          GetApportionFactors(rawHourOffset.Value,
                              out so2mApportionValue,
                              out co2mApportionValue,
                              out noxmApportionValue,
                              out noxrCalculateValue,
                              out hgmApportionValue,
                              out hclmApportionValue,
                              out hfmApportionValue);

          // Op Time, GLoad, SLoad and TLoad
          CopyOpData(apportionHourOffset.Value, rawHourOffset.Value);

          // HIT Value, Method and Measure
          CopyHit(apportionHourOffset.Value, rawHourOffset.Value);

          // SO2M Value, Method and Measure
          ApportionMass(So2mArray, 
                        So2mMeasureArray,
                        HourlyRawData.So2mArray,
                        HourlyRawData.So2mMeasureArray,
                        so2mApportionValue,
                        so2mCommon,
                        so2mCommonMeasure,
                        cDecimalPlaces.So2m,
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
                        co2mApportionValue,
                        co2mCommon,
                        co2mCommonMeasure,
                        cDecimalPlaces.Co2m,
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
                        noxmApportionValue,
                        noxmCommon,
                        noxmCommonMeasure,
                        cDecimalPlaces.Noxm,
                        apportionHourOffset.Value,
                        rawHourOffset.Value);

          // NoxR Value, Method and Measure - Always after apportioning NOxM
          ApportionNoxr(noxrCalculateValue,
                        noxrCommon,
                        noxrCommonMeasure,
                        apportionHourOffset.Value,
                        rawHourOffset.Value);

          // Try to calculate missing NOxM using calculated NOxR - Always after calcualting NOxR 
          RecheckNoxm(apportionHourOffset.Value);

          // Hg Mass Value and Measure Code
          ApportionMass(HgMassArray,
                        HgMeasureArray,
                        HourlyRawData.HgMassArray,
                        HourlyRawData.HgMeasureArray,
                        hgmApportionValue,
                        hgmCommon,
                        hgmCommonMeasure,
                        cDecimalPlaces.Hgm,
                        apportionHourOffset.Value,
                        rawHourOffset.Value);

          // HCl Mass Value and Measure Code
          ApportionMass(HclMassArray,
                        HclMeasureArray,
                        HourlyRawData.HclMassArray,
                        HourlyRawData.HclMeasureArray,
                        hclmApportionValue,
                        hclmCommon,
                        hclmCommonMeasure,
                        cDecimalPlaces.Hclm,
                        apportionHourOffset.Value,
                        rawHourOffset.Value);

          // HF Mass Value and Measure Code
          ApportionMass(HfMassArray,
                        HfMeasureArray,
                        HourlyRawData.HfMassArray,
                        HourlyRawData.HfMeasureArray,
                        hfmApportionValue,
                        hfmCommon,
                        hfmCommonMeasure,
                        cDecimalPlaces.Hfm,
                        apportionHourOffset.Value,
                        rawHourOffset.Value);

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
    /// Assigns mass value to the unit.
    /// </summary>
    /// <param name="apportionValueArray">The array for the mass's apportioned values.</param>
    /// <param name="apportionMeasureArray">The array for the mass's apporioned measure code.</param>
    /// <param name="rawValueArray">The array for the mass's raw values.</param>
    /// <param name="rawMeasureArray">The array for the mass's raw measure codes.</param>
    /// <param name="apportionValue">A boolean array indicating whether a particular unit should receive apportioned values.</param>
    /// <param name="combinedValue">The combined multiple stack mass value.</param>
    /// <param name="combinedMeasure">The combined multiple stack mass measure code.</param>
    /// <param name="decimalPlaces">The rounding decimal places for the mass.</param>
    /// <param name="apportionHourOffset">Hour offset into the apportionment data arrays.</param>
    /// <param name="rawHourOffset">Hour offset into the raw daa arrays.</param>
    private void ApportionMass(decimal?[] apportionValueArray,
                               string[] apportionMeasureArray,
                               decimal?[] rawValueArray,
                               string[] rawMeasureArray,
                               bool apportionValue,
                               decimal? combinedValue,
                               string combinedMeasure,
                               int decimalPlaces,
                               int apportionHourOffset,
                               int rawHourOffset)
    {
      int apportionPos = apportionHourOffset;
      int rawPos = UnitInfo[0].LocationPosition.Value + rawHourOffset;

      if (combinedValue.HasValue && apportionValue)
      {
        apportionValueArray[apportionPos]
          = Math.Round(combinedValue.Value,
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

    /// <summary>
    /// Assigns NOXR value to the unit.
    /// </summary>
    /// <param name="calculateValue">Indicates whether the NOXR is calculated from the MS.</param>
    /// <param name="combinedValue">The combined multiple stack mass value.</param>
    /// <param name="combinedMeasure">The combined multiple stack mass measure code.</param>
    /// <param name="apportionHourOffset">Hour offset into the apportionment data arrays.</param>
    /// <param name="rawHourOffset">Hour offset into the raw daa arrays.</param>
    private void ApportionNoxr(bool calculateValue,
                               decimal? combinedValue,
                               string combinedMeasure,
                               int apportionHourOffset,
                               int rawHourOffset)
    {
      int apportionPos = apportionHourOffset;
      int rawPos = UnitInfo[0].LocationPosition.Value + rawHourOffset;

      if (calculateValue)
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

    /// <summary>
    /// Returns value indicates whether to apportion values from MS.
    /// </summary>
    /// <param name="rawHourOffset">Hour offset into the raw daa arrays.</param>
    /// <param name="so2mApportionValue">Indicates whether to apportione the value.</param>
    /// <param name="co2mApportionValue">Indicates whether to apportione the value.</param>
    /// <param name="noxmApportionValue">Indicates whether to apportione the value.</param>
    /// <param name="noxrCalculateValue">Indicates whether to apportione the value.</param>
    /// <param name="hgmApportionValue">A boolean array indicating whether a particular unit should receive apportioned values.</param>
    /// <param name="hclmApportionValue">A boolean array indicating whether a particular unit should receive apportioned values.</param>
    /// <param name="hfmApportionValue">A boolean array indicating whether a particular unit should receive apportioned values.</param>
    private void GetApportionFactors(int rawHourOffset,
                                     out bool so2mApportionValue,
                                     out bool co2mApportionValue,
                                     out bool noxmApportionValue,
                                     out bool noxrCalculateValue,
                                     out bool hgmApportionValue,
                                     out bool hclmApportionValue,
                                     out bool hfmApportionValue)
    {
      int dataPos = UnitInfo[0].LocationPosition.Value + rawHourOffset;
      int locationPos = UnitInfo[0].LocationPosition.Value;

      if (HourlyRawData.OpTimeArray[dataPos] > 0)
      {
        so2mApportionValue = !HourlyRawData.So2mArray[dataPos].HasValue;
        co2mApportionValue = !HourlyRawData.Co2mArray[dataPos].HasValue;

        noxmApportionValue = !HourlyRawData.NoxmArray[dataPos].HasValue &&
                             !(HourlyRawData.HitArray[dataPos].HasValue &&
                               HourlyRawData.NoxrArray[dataPos].HasValue);

        noxrCalculateValue = !HourlyRawData.NoxrArray[dataPos].HasValue;

        hgmApportionValue = !HourlyRawData.HgMassArray[dataPos].HasValue;
        hclmApportionValue = !HourlyRawData.HclMassArray[dataPos].HasValue;
        hfmApportionValue = !HourlyRawData.HfMassArray[dataPos].HasValue;
      }
      else
      {
        so2mApportionValue = false;
        co2mApportionValue = false;
        noxmApportionValue = false;
        noxrCalculateValue = false;
        hgmApportionValue = false;
        hclmApportionValue = false;
        hfmApportionValue = false;
      }
    }

    /// <summary>
    /// Gets the multiple stack values and measure flags.
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

      so2mCombinedValue = null; so2mCombinedMeasure = null;
      co2mCombinedValue = null; co2mCombinedMeasure = null;
      noxmCombinedValue = null; noxmCombinedMeasure = null;
      noxrCombinedValue = null; noxrCombinedMeasure = null;

      hgmCombinedValue = null;  hgmCombinedMeasure = null;
      hclmCombinedValue = null; hclmCombinedMeasure = null;
      hfmCombinedValue = null;  hfmCombinedMeasure = null;

      cCombineMeasure so2mMeasureObject = new cCombineMeasure();
      cCombineMeasure co2mMeasureObject = new cCombineMeasure();
      cCombineMeasure noxmMeasureObject = new cCombineMeasure();
      cCombineMeasure noxrMeasureObject = new cCombineMeasure();

      int noxrCount = 0;
      decimal noxrMax = 0;
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

            if (HourlyRawData.NoxrArray[dataPos].Value > noxrMax)
              noxrMax = HourlyRawData.NoxrArray[dataPos].Value;

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
              noxrCombinedValue = noxrMax;
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
                noxrCombinedValue = noxrMax;
              }
            }
            break;
        }
      }
    }

    #endregion

  }

}
