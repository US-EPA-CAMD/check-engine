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
  /// The base class of the classes used to apportion data.
  /// </summary>
  public abstract class cHourlyApportionedData
  {

    #region Protected Constructors

    /// <summary>
    /// Creates and instance of the Hourly Apportioned Data class.
    /// </summary>
    /// <param name="hourlyRawData">The raw data on which the apportionment will occur.</param>
    protected cHourlyApportionedData(cHourlyRawData hourlyRawData)
    {
      // Set Hourly Raw Data
      HourlyRawData = hourlyRawData;

      // Set UnitCount and DataLength
      DataLength = UnitCount * HourCount;

      // Initialize data arrays
      {
        UnitKeyArray = new int?[DataLength];
        DmEmissionsIdArray = new string[DataLength];
        OpDateArray = new DateTime?[DataLength];
        OpHourArray = new int?[DataLength];

        OpTimeArray = new decimal?[DataLength];

        GLoadArray = new decimal?[DataLength];
        MLoadArray = new decimal?[DataLength];
        SLoadArray = new decimal?[DataLength];
        TLoadArray = new decimal?[DataLength];

        HitArray = new decimal?[DataLength];
        HitMeasureArray = new string[DataLength];

        So2mArray = new decimal?[DataLength];
        So2mMeasureArray = new string[DataLength];
        So2rArray = new decimal?[DataLength];
        So2rMeasureArray = new string[DataLength];

        Co2mArray = new decimal?[DataLength];
        Co2mMeasureArray = new string[DataLength];
        Co2rArray = new decimal?[DataLength];
        Co2rMeasureArray = new string[DataLength];

        NoxmArray = new decimal?[DataLength];
        NoxmMeasureArray = new string[DataLength];
        NoxrArray = new decimal?[DataLength];
        NoxrMeasureArray = new string[DataLength];

        // Hg
        HgRateEoArray = new decimal?[DataLength];
        HgRateHiArray = new decimal?[DataLength];
        HgMassArray = new decimal?[DataLength];
        HgMeasureArray = new string[DataLength];

        // HCl
        HclRateEoArray = new decimal?[DataLength];
        HclRateHiArray = new decimal?[DataLength];
        HclMassArray = new decimal?[DataLength];
        HclMeasureArray = new string[DataLength];

        // HF
        HfRateEoArray = new decimal?[DataLength];
        HfRateHiArray = new decimal?[DataLength];
        HfMassArray = new decimal?[DataLength];
        HfMeasureArray = new string[DataLength];

        MonPlanIdArray = new string[DataLength];
        RptPeriodIdArray = new int?[DataLength];
        OpYearArray = new int?[DataLength];
        DataSourceArray = new string[DataLength];
        UserIdArray = new string[DataLength];
      }

      // Populate location and operating hour information
      for (int unitDex = 0; unitDex < UnitCount; unitDex++)
      {
        for (int hourDex = 0; hourDex < HourCount; hourDex++)
        {
          int dataDex = (hourDex * UnitCount) + unitDex;

          DateTime date = QuarterBeganDate.AddDays(hourDex / 24);
          int hour = hourDex % 24;

          UnitKeyArray[dataDex] = UnitInfo[unitDex].UnitKey;
          OpDateArray[dataDex] = date;
          OpHourArray[dataDex] = hour;
        }
      }

      // Populate the information that does not change
      for (int dataDex = 0; dataDex < DataLength; dataDex++)
      {
        DmEmissionsIdArray[dataDex] = hourlyRawData.DmEmissionsId;
        MonPlanIdArray[dataDex] = hourlyRawData.MonPlanId;
        RptPeriodIdArray[dataDex] = hourlyRawData.RptPeriodId;
        OpYearArray[dataDex] = hourlyRawData.Year;
        DataSourceArray[dataDex] = hourlyRawData.DataSource;
        UserIdArray[dataDex] = hourlyRawData.UserId;
      }
    }

    #endregion


    #region Public Abstract Properties

    /// <summary>
    /// The apportionment type of the class.
    /// </summary>
    public abstract eApportionmentType ApportionmentType { get; }

    #endregion


    #region Public Static Methods

    /// <summary>
    /// Returns an apportionment class corresponding to the indicated apportionment type.
    /// </summary>
    /// <param name="apportionmentType">The apportionment type of the apportionment class.</param>
    /// <param name="hourlyRawData">The raw data to apportion.</param>
    /// <param name="factorFormulaeArray"></param>
    /// <param name="hourlyApportionedData">The resulting apportionment class.</param>
    /// <returns>False if the apportionment type is not handled or an unhandled exception occurs.</returns>
    public static bool GetApportionedData(eApportionmentType apportionmentType,
                                          cHourlyRawData hourlyRawData,
                                          cFactorFormulae[] factorFormulaeArray,
                                          out cHourlyApportionedData hourlyApportionedData)
    {
      bool result;

      try
      {
        if ((factorFormulaeArray != null) && (factorFormulaeArray.Length > 0))
        {
          hourlyApportionedData = new cHourlyApportionedDataComplex(hourlyRawData, factorFormulaeArray);
        }
        else
        {
          switch (apportionmentType)
          {
            case eApportionmentType.CommonPipe:
              hourlyApportionedData = new cHourlyApportionedDataCommonPipe(hourlyRawData);
              break;

            case eApportionmentType.CommonPipeLtff:
              hourlyApportionedData = new cHourlyApportionedDataUnit(hourlyRawData);
              break;

            case eApportionmentType.CommonStack:
              hourlyApportionedData = new cHourlyApportionedDataCommonStack(hourlyRawData);
              break;

            case eApportionmentType.CommonStackAndPipe:
              hourlyApportionedData = new cHourlyApportionedDataCommonStackAndPipe(hourlyRawData);
              break;

            case eApportionmentType.MultipleStack:
              hourlyApportionedData = new cHourlyApportionedDataMultipleStack(hourlyRawData);
              break;

            case eApportionmentType.Unit:
              hourlyApportionedData = new cHourlyApportionedDataUnit(hourlyRawData);
              break;

            default:
              hourlyApportionedData = null;
              break;
          }
        }

        if (hourlyApportionedData != null)
        {
          result = hourlyApportionedData.Apportion();
        }
        else
        {
          cErrorHandler.LogError(string.Format("Apportionment Type '{0}' not handled.", apportionmentType));
          result = false;
        }
      }
      catch (Exception ex)
      {
        cErrorHandler.LogError(ex.Message);
        hourlyApportionedData = null;
        result = false;
      }

      return result;
    }

    #endregion


    #region Public Properties

    /// <summary>
    /// The length of the arrays used to store the monitor hour data.
    /// </summary>
    public int DataLength { get; private set; }

    /// <summary>
    /// The raw monitor data on which apportionment will occur.
    /// </summary>
    public cHourlyRawData HourlyRawData { get; private set; }

    #endregion


    #region Public Properties: Referenced

    /// <summary>
    /// The number of days in the emission report's quarter.
    /// </summary>
    public int DateCount { get { return HourlyRawData.DateCount; } }

    /// <summary>
    /// The number of hours in the emission report's quarter.
    /// </summary>
    public int HourCount { get { return HourlyRawData.HourCount; } }

    /// <summary>
    /// The number of location in the emission report.
    /// </summary>
    public int LocationCount { get { return HourlyRawData.LocationCount; } }

    /// <summary>
    /// The location info for the emission report.
    /// </summary>
    public cLocationInfo[] LocationInfo { get { return HourlyRawData.LocationInfo; } }

    /// <summary>
    /// The quarter of the emissions report.
    /// </summary>
    public int Quarter { get { return HourlyRawData.Quarter; } }

    /// <summary>
    /// The began date of the emissions report quarter.
    /// </summary>
    public DateTime QuarterBeganDate { get { return HourlyRawData.QuarterBeganDate; } }

    /// <summary>
    /// The ended date of the emissions report quarter.
    /// </summary>
    public DateTime QuarterEndedDate { get { return HourlyRawData.QuarterEndedDate; } }

    /// <summary>
    /// The number of units involved in the emissions report.
    /// 
    /// Basically the number of items in LocationInfo.
    /// </summary>
    public int UnitCount { get { return HourlyRawData.UnitCount; } }

    /// <summary>
    /// Array of unit information objects.
    /// </summary>
    public cUnitInfo[] UnitInfo { get { return HourlyRawData.UnitInfo; } }

    /// <summary>
    /// The user id to assign to the unit hour data.
    /// </summary>
    public string UserId { get { return HourlyRawData.UserId; } }

    /// <summary>
    /// The year of the emissions report.
    /// </summary>
    public int Year { get { return HourlyRawData.Year; } }

    #endregion


    #region Public Properties: Global Data Arrays

    /// <summary>
    /// The array of Data Sources
    /// </summary>
    public string[] DataSourceArray { get; private set; }

    /// <summary>
    /// The array of DM_EMISSIONS_ID
    /// </summary>
    public string[] DmEmissionsIdArray { get; private set; }

    /// <summary>
    /// The array of Monitor Plan Ids.
    /// </summary>
    public string[] MonPlanIdArray { get; private set; }

    /// <summary>
    /// The array of Report Period Ids.
    /// </summary>
    public int?[] RptPeriodIdArray { get; private set; }

    /// <summary>
    /// The array of Years
    /// </summary>
    public int?[] OpYearArray { get; private set; }

    /// <summary>
    /// The array of User Ids.
    /// </summary>
    public string[] UserIdArray { get; private set; }

    #endregion


    #region Public Properties: Location and Operating Hour Data Arrays

    /// <summary>
    /// The arrau of Unit Keys (UNIT_ID).
    /// </summary>
    public int?[] UnitKeyArray { get; private set; }

    /// <summary>
    /// The array of Op Dates.
    /// </summary>
    public DateTime?[] OpDateArray { get; private set; }
    
    /// <summary>
    /// The array of Op Hours.
    /// </summary>
    public int?[] OpHourArray { get; private set; }

    #endregion


    #region Public Properties: General Value Data Arrays

    /// <summary>
    /// The array of Monitor Hour Op Time
    /// </summary>
    public decimal?[] OpTimeArray { get; private set; }

    /// <summary>
    /// The array of Monitor Hour G-Load
    /// </summary>
    public decimal?[] GLoadArray { get; private set; }

    /// <summary>
    /// The array of Monitor Hour MATS Load
    /// </summary>
    public decimal?[] MLoadArray { get; private set; }

    /// <summary>
    /// The array of Monitor Hour S-Load
    /// </summary>
    public decimal?[] SLoadArray { get; private set; }

    /// <summary>
    /// The array of Monitor Hour T-Load
    /// </summary>
    public decimal?[] TLoadArray { get; private set; }

    /// <summary>
    /// The array of Monitor Hour HIT
    /// </summary>
    public decimal?[] HitArray { get; private set; }

    /// <summary>
    /// The array of Monitor Hour HIT Hour Measure Code
    /// </summary>
    public string[] HitMeasureArray { get; private set; }

    #endregion


    #region Public Properties: Part 75 Value Data Arrays

    /// <summary>
    /// The array of Monitor Hour SO2M
    /// </summary>
    public decimal?[] So2mArray { get; private set; }

    /// <summary>
    /// The array of Monitor Hour SO2M Hour Measure Code
    /// </summary>
    public string[] So2mMeasureArray { get; private set; }

    /// <summary>
    /// The array of Monitor Hour SO2R
    /// </summary>
    public decimal?[] So2rArray { get; private set; }

    /// <summary>
    /// The array of Monitor Hour SO2R Hour Measure Code
    /// </summary>
    public string[] So2rMeasureArray { get; private set; }

    /// <summary>
    /// The array of Monitor Hour CO2M
    /// </summary>
    public decimal?[] Co2mArray { get; private set; }

    /// <summary>
    /// The array of Monitor Hour CO2M Hour Measure Code
    /// </summary>
    public string[] Co2mMeasureArray { get; private set; }

    /// <summary>
    /// The array of Monitor Hour CO2R
    /// </summary>
    public decimal?[] Co2rArray { get; private set; }

    /// <summary>
    /// The array of Monitor Hour CO2R Hour Measure Code
    /// </summary>
    public string[] Co2rMeasureArray { get; private set; }

    /// <summary>
    /// The array of Monitor Hour NOXM
    /// </summary>
    public decimal?[] NoxmArray { get; private set; }

    /// <summary>
    /// The array of Monitor Hour NOXM Hour Measure Code
    /// </summary>
    public string[] NoxmMeasureArray { get; private set; }

    /// <summary>
    /// The array of Monitor Hour NOXR
    /// </summary>
    public decimal?[] NoxrArray { get; private set; }

    /// <summary>
    /// The array of Monitor Hour NOXR Hour Measure Code
    /// </summary>
    public string[] NoxrMeasureArray { get; private set; }

    #endregion


    #region Public Properties: MATS Value Data Arrays

    /// <summary>
    /// Hg Electrical Output Based Rate array
    /// </summary>
    public decimal?[] HgRateEoArray { get; private set; }

    /// <summary>
    /// Hg Heat Input Based Rate array
    /// </summary>
    public decimal?[] HgRateHiArray { get; private set; }

    /// <summary>
    /// Hg Mass array
    /// </summary>
    public decimal?[] HgMassArray { get; private set; }

    /// <summary>
    /// Hg Hour Measure Code array
    /// </summary>
    public string[] HgMeasureArray { get; private set; }

    /// <summary>
    /// HCl Electrical Output Based Rate array
    /// </summary>
    public decimal?[] HclRateEoArray { get; private set; }

    /// <summary>
    /// HCl Heat Input Based Rate array
    /// </summary>
    public decimal?[] HclRateHiArray { get; private set; }

    /// <summary>
    /// HCl Mass array
    /// </summary>
    public decimal?[] HclMassArray { get; private set; }

    /// <summary>
    /// HCl Hour Measure Code array
    /// </summary>
    public string[] HclMeasureArray { get; private set; }

    /// <summary>
    /// HF Electrical Output Based Rate array
    /// </summary>
    public decimal?[] HfRateEoArray { get; private set; }

    /// <summary>
    /// HF Heat Input Based Rate array
    /// </summary>
    public decimal?[] HfRateHiArray { get; private set; }

    /// <summary>
    /// HF Mass array
    /// </summary>
    public decimal?[] HfMassArray { get; private set; }

    /// <summary>
    /// HF Hour Measure Code array
    /// </summary>
    public string[] HfMeasureArray { get; private set; }

    #endregion


    #region Public Methods

    /// <summary>
    /// Performs apportionment for all hours
    /// </summary>
    /// <returns>False if an unhandled exception occurs.</returns>
    public bool Apportion()
    {
      bool result = true;

      for (int hourDex = 0; hourDex < HourCount; hourDex++)
      {
        DateTime date = QuarterBeganDate.AddDays(hourDex / 24);
        int hour = hourDex % 24;

        try
        {
          result = Apportion(date, hour);
        }
        catch (Exception ex)
        {
          cErrorHandler.LogError(ex.Message, date, hour);
          result = false;
        }
      }

      return result;
    }

    #endregion


    #region Public Abstract Methods

    /// <summary>
    /// The method that apportions an individual hour.
    /// </summary>
    /// <param name="date">The date of the hour to apportion.</param>
    /// <param name="hour">The hour to apportion.</param>
    /// <returns></returns>
    public abstract bool Apportion(DateTime date, int hour);

    #endregion


    #region Protected Methods: Utilities

    /// <summary>
    /// Calculates and sets the rate and rate measure values.
    /// </summary>
    /// <param name="apportionRateValueArray">The array of apportion rate values to set.</param>
    /// <param name="apportionRateMeasureArray">The array of apportion rate measure flags to set.</param>
    /// <param name="apportionMassValueArray">The array of apportion mass values used to calculate the rate.</param>
    /// <param name="apportionHitValueArray">The array of apportion HIT values used to calculate the rate.</param>
    /// <param name="decimalPlaces">The number of rounding decimal places for the result.</param>
    /// <param name="apportionHourOffset">The hour offset into the apportion arrays.</param>
    protected void ApportionRate(decimal?[] apportionRateValueArray,
                                 string[] apportionRateMeasureArray,
                                 decimal?[] apportionMassValueArray,
                                 decimal?[] apportionHitValueArray,
                                 int decimalPlaces,
                                 int apportionHourOffset)
    {
      for (int unitDex = 0; unitDex < UnitCount; unitDex++)
      {
        int apportionPos = unitDex + apportionHourOffset;

        if (apportionHitValueArray[apportionPos].HasValue && (apportionHitValueArray[apportionPos].Value > 0) &&
            apportionMassValueArray[apportionPos].HasValue)
        {
          apportionRateValueArray[apportionPos]
            = Math.Round(apportionMassValueArray[apportionPos].Value / apportionHitValueArray[apportionPos].Value,
                         decimalPlaces,
                         MidpointRounding.AwayFromZero);

          apportionRateMeasureArray[apportionPos] = "CALC";
        }
        else
        {
          apportionRateValueArray[apportionPos] = null;
          apportionRateMeasureArray[apportionPos] = null;
        }
      }
    }

    /// <summary>
    /// Combines common values and their measure codes.
    /// 
    /// If any combined value has a measure code of "Unavailable", the combined measure code is unavailable and the value is null.
    /// </summary>
    /// <param name="combinedValue">Value to combine.</param>
    /// <param name="combinedMeasure">Measure code to combine.</param>
    /// <param name="hasValues">Has value indicator initialized to null in caller and set to true if value exist at rawPos.</param>
    /// <param name="rawPos">The position of the location/hour in the raw arrays.</param>
    /// <param name="rawMassArray">The raw array of the mass value to combine.</param>
    /// <param name="rawMeasureArray">The raw array of the measure value to combine.</param>
    protected void CombineMatsValue(ref decimal? combinedValue, ref cCombineMeasure combinedMeasure, ref bool hasValues,
                                    int rawPos, decimal?[] rawMassArray, string[] rawMeasureArray)
    {
      combinedMeasure.Combine(rawMeasureArray[rawPos]);

      if (combinedMeasure.UnavailableFound)
      {
        combinedValue = null;
        hasValues = true;
      }
      else
      {
        if (rawMassArray[rawPos].HasValue)
        {
          if (combinedValue.HasValue)
            combinedValue += rawMassArray[rawPos].Value;
          else
            combinedValue = rawMassArray[rawPos].Value;

          hasValues = true;
        }
      }
    }

    /// <summary>
    /// Moves HIT data from the specified raw position to the specified apportion position.
    /// </summary>
    /// <param name="apportionHourOffset">Hour offset into the apportionment data arrays.</param>
    /// <param name="rawHourOffset">Hour offset into the raw daa arrays.</param>
    protected void CopyHit(int apportionHourOffset, int rawHourOffset)
    {
      for (int unitDex = 0; unitDex < UnitCount; unitDex++)
      {
        int apportionPos = unitDex + apportionHourOffset;
        int rawPos = UnitInfo[unitDex].LocationPosition.Value + rawHourOffset;

        HitArray[apportionPos] = HourlyRawData.HitArray[rawPos];
        HitMeasureArray[apportionPos] = HourlyRawData.HitMeasureArray[rawPos];
      }
    }

    /// <summary>
    /// Moves data from the specified raw position to the specified apportion position.
    /// </summary>
    /// <param name="apportionHourOffset">Hour offset into the apportionment data arrays.</param>
    /// <param name="rawHourOffset">Hour offset into the raw daa arrays.</param>
    protected void CopyHour(int apportionHourOffset, int rawHourOffset)
    {
      for (int unitDex = 0; unitDex < UnitCount; unitDex++)
      {
        int apportionPos = unitDex + apportionHourOffset;
        int rawPos = UnitInfo[unitDex].LocationPosition.Value + rawHourOffset;

        CopyUnit(apportionPos, rawPos);
      }
    }

    /// <summary>
    /// Moves op time and load data from the specified raw position to the specified apportion position.
    /// </summary>
    /// <param name="apportionHourOffset">Hour offset into the apportionment data arrays.</param>
    /// <param name="rawHourOffset">Hour offset into the raw daa arrays.</param>
    protected void CopyOpData(int apportionHourOffset, int rawHourOffset)
    {
      for (int unitDex = 0; unitDex < UnitCount; unitDex++)
      {
        int apportionPos = unitDex + apportionHourOffset;
        int rawPos = UnitInfo[unitDex].LocationPosition.Value + rawHourOffset;

        OpTimeArray[apportionPos] = HourlyRawData.OpTimeArray[rawPos];
        GLoadArray[apportionPos] = HourlyRawData.GLoadArray[rawPos];
        MLoadArray[apportionPos] = HourlyRawData.MLoadArray[rawPos];
        SLoadArray[apportionPos] = HourlyRawData.SLoadArray[rawPos];
        TLoadArray[apportionPos] = HourlyRawData.TLoadArray[rawPos];
      }
    }

    /// <summary>
    /// Moved MATS values from the specified raw position to the specified apportion position.
    /// </summary>
    /// <param name="apportionHourOffset">Hour offset into the apportionment data arrays.</param>
    /// <param name="rawHourOffset">Hour offset into the raw daa arrays.</param>
    protected void CopyMats(int apportionHourOffset, int rawHourOffset)
    {
      for (int unitDex = 0; unitDex < UnitCount; unitDex++)
      {
        int apportionPos = unitDex + apportionHourOffset;
        int rawPos = UnitInfo[unitDex].LocationPosition.Value + rawHourOffset;

        HgMassArray[apportionPos] = HourlyRawData.HgMassArray[rawPos];
        HgRateEoArray[apportionPos] = HourlyRawData.HgRateEoArray[rawPos];
        HgRateHiArray[apportionPos] = HourlyRawData.HgRateHiArray[rawPos];
        HgMeasureArray[apportionPos] = HourlyRawData.HgMeasureArray[rawPos];

        HclMassArray[apportionPos] = HourlyRawData.HclMassArray[rawPos];
        HclRateEoArray[apportionPos] = HourlyRawData.HclRateEoArray[rawPos];
        HclRateHiArray[apportionPos] = HourlyRawData.HclRateHiArray[rawPos];
        HclMeasureArray[apportionPos] = HourlyRawData.HclMeasureArray[rawPos];

        HfMassArray[apportionPos] = HourlyRawData.HfMassArray[rawPos];
        HfRateEoArray[apportionPos] = HourlyRawData.HfRateEoArray[rawPos];
        HfRateHiArray[apportionPos] = HourlyRawData.HfRateHiArray[rawPos];
        HfMeasureArray[apportionPos] = HourlyRawData.HfMeasureArray[rawPos];
      }
    }

    /// <summary>
    /// Moves unit data from the specified raw position to the specified apportion position.
    /// The move also calculates SO2R and CO2R data as well as null NOXM and NOXR data.
    /// </summary>
    /// <param name="apportionPos">The position of the unit in apportionment data.</param>
    /// <param name="rawPos">The position of the unit in raw data.</param>
    protected void CopyUnit(int apportionPos, int rawPos)
    {
      // Set Op Time
      OpTimeArray[apportionPos] = HourlyRawData.OpTimeArray[rawPos];

      // Set Loads
      GLoadArray[apportionPos] = HourlyRawData.GLoadArray[rawPos];
      MLoadArray[apportionPos] = HourlyRawData.MLoadArray[rawPos];
      SLoadArray[apportionPos] = HourlyRawData.SLoadArray[rawPos];
      TLoadArray[apportionPos] = HourlyRawData.TLoadArray[rawPos];

      // Set HIT value and measure
      HitArray[apportionPos] = HourlyRawData.HitArray[rawPos];
      HitMeasureArray[apportionPos] = HourlyRawData.HitMeasureArray[rawPos];

      // Set SO2M value and measure
      So2mArray[apportionPos] = HourlyRawData.So2mArray[rawPos];
      So2mMeasureArray[apportionPos] = HourlyRawData.So2mMeasureArray[rawPos];

      // Calculate SO2R value and measure
      if (So2mArray[apportionPos].HasValue && HitArray[apportionPos].HasValue && (HitArray[apportionPos].Value > 0))
      {
        So2rArray[apportionPos] = Math.Round(So2mArray[apportionPos].Value / HitArray[apportionPos].Value,
                                             cDecimalPlaces.So2r,
                                             MidpointRounding.AwayFromZero);
        So2rMeasureArray[apportionPos] = "CALC";
      }
      else
      {
        So2rArray[apportionPos] = null;
        So2rMeasureArray[apportionPos] = null;
      }

      // Set CO2M value and measure
      Co2mArray[apportionPos] = HourlyRawData.Co2mArray[rawPos];
      Co2mMeasureArray[apportionPos] = HourlyRawData.Co2mMeasureArray[rawPos];

      // Calculate CO2R value and measure
      if (Co2mArray[apportionPos].HasValue && HitArray[apportionPos].HasValue && (HitArray[apportionPos].Value > 0))
      {
        Co2rArray[apportionPos] = Math.Round(Co2mArray[apportionPos].Value / HitArray[apportionPos].Value,
                                             cDecimalPlaces.Co2r,
                                             MidpointRounding.AwayFromZero);
        Co2rMeasureArray[apportionPos] = "CALC";
      }
      else
      {
        Co2rArray[apportionPos] = null;
        Co2rMeasureArray[apportionPos] = null;
      }

      // Set NOXM value and measure
      NoxmArray[apportionPos] = HourlyRawData.NoxmArray[rawPos];
      NoxmMeasureArray[apportionPos] = HourlyRawData.NoxmMeasureArray[rawPos];

      // Set NOXR value and measure
      NoxrArray[apportionPos] = HourlyRawData.NoxrArray[rawPos];
      NoxrMeasureArray[apportionPos] = HourlyRawData.NoxrMeasureArray[rawPos];

      // Calculate null NOXM and NOXR values
      if (HitArray[apportionPos] != null)
      {
        if ((NoxmArray[apportionPos] == null) && (NoxrArray[apportionPos] != null))
        {
          NoxmArray[apportionPos]
            = Math.Round(NoxrArray[apportionPos].Value * HitArray[apportionPos].Value,
                         cDecimalPlaces.Noxm,
                         MidpointRounding.AwayFromZero);
          NoxmMeasureArray[apportionPos] = "CALC";
        }
        else if ((NoxrArray[apportionPos] == null) && (NoxmArray[apportionPos] != null) &&
                 (HitArray[apportionPos].Value != 0))
        {
          NoxrArray[apportionPos]
            = Math.Round(NoxmArray[apportionPos].Value / HitArray[apportionPos].Value,
                         cDecimalPlaces.Noxr,
                         MidpointRounding.AwayFromZero);
          NoxrMeasureArray[apportionPos] = "CALC";
        }
      }

      // Hg
      HgMassArray[apportionPos] = HourlyRawData.HgMassArray[rawPos];
      HgRateEoArray[apportionPos] = HourlyRawData.HgRateEoArray[rawPos];
      HgRateHiArray[apportionPos] = HourlyRawData.HgRateHiArray[rawPos];
      HgMeasureArray[apportionPos] = HourlyRawData.HgMeasureArray[rawPos];

      // HCl
      HclMassArray[apportionPos] = HourlyRawData.HclMassArray[rawPos];
      HclRateEoArray[apportionPos] = HourlyRawData.HclRateEoArray[rawPos];
      HclRateHiArray[apportionPos] = HourlyRawData.HclRateHiArray[rawPos];
      HclMeasureArray[apportionPos] = HourlyRawData.HclMeasureArray[rawPos];

      // HF
      HfMassArray[apportionPos] = HourlyRawData.HfMassArray[rawPos];
      HfRateEoArray[apportionPos] = HourlyRawData.HfRateEoArray[rawPos];
      HfRateHiArray[apportionPos] = HourlyRawData.HfRateHiArray[rawPos];
      HfMeasureArray[apportionPos] = HourlyRawData.HfMeasureArray[rawPos];
    }

    /// <summary>
    /// Gets the offset for the specified hour in apportionment data.
    /// </summary>
    /// <param name="date">The date of the hour of the offest.</param>
    /// <param name="hour">The hour of the offest.</param>
    /// <returns></returns>
    protected int? GetHourOffset(DateTime date, int hour)
    {
      int? result;

      int dateOffset = cUtilities.ElapsedDays(QuarterBeganDate, date);

      if ((dateOffset < 0) || (dateOffset >= DateCount))
      {
        cErrorHandler.LogError(string.Format("Date is not in quarter {1}q{2}: '{0}' ({3})",
                                             date.ToShortDateString(), Year, Quarter,
                                             "cHourlyApportionedData.GetDataPosition"),
                               date, hour);
        result = null;
      }
      else if ((hour < 0) || (hour >= 24))
      {
        cErrorHandler.LogError(string.Format("Hour value is not an hour: '{0}' ({1})",
                                             hour, "cHourlyApportionedData.GetDataPosition"),
                               date, hour);
        result = null;
      }
      else
      {
        result = UnitCount * ((24 * dateOffset) + hour);
      }

      return result;
    }

    /// <summary>
    /// Gets the offset for the specified hour in apportionment data.
    /// </summary>
    /// <param name="date">The date of the hour of the offest.</param>
    /// <param name="hour">The hour of the offest.</param>
    /// <param name="hourOffset">The offset of the hour.</param>
    /// <returns>True if the offset was successfully determined.</returns>
    protected bool GetHourOffset(DateTime date, int hour, out int? hourOffset)
    {
      bool result;

      hourOffset = GetHourOffset(date, hour);

      result = hourOffset.HasValue;

      return result;
    }

    /// <summary>
    /// Attempts to calculate null NOXM from HIT and NOXR.
    /// </summary>
    /// <param name="apportionHourOffset">Hour offset into the apportionment data arrays.</param>
    protected void RecheckNoxm(int apportionHourOffset)
    {
      for (int unitDex = 0; unitDex < UnitCount; unitDex++)
      {
        int apportionPos = unitDex + apportionHourOffset;

        if (!NoxmArray[apportionPos].HasValue &&
            HitArray[apportionPos].HasValue &&
            NoxrArray[apportionPos].HasValue)
        {
          NoxmArray[apportionPos]
            = Math.Round(HitArray[apportionPos].Value * NoxrArray[apportionPos].Value,
                         cDecimalPlaces.Noxm,
                         MidpointRounding.AwayFromZero);

          NoxmMeasureArray[apportionPos] = "CALC";
        }
      }
    }

    /// <summary>
    /// Populates missing MATS rates when the mass and either MATS Load and Heat Input exists.
    /// </summary>
    /// <param name="apportionHourOffset">Hour offset into the apportionment data arrays.</param>
    protected void FillMissingMatsRates(int apportionHourOffset)
    {
      for (int unitDex = 0; unitDex < UnitCount; unitDex++)
      {
        int apportionPos = unitDex + apportionHourOffset;

        FillMissingMatsRates(apportionPos, HgMassArray, MLoadArray, HitArray, cDecimalPlaces.Hgreo, cDecimalPlaces.Hgrhi, HgRateEoArray, HgRateHiArray);
        FillMissingMatsRates(apportionPos, HclMassArray, MLoadArray, HitArray, cDecimalPlaces.Hclreo, cDecimalPlaces.Hclrhi, HclRateEoArray, HclRateHiArray);
        FillMissingMatsRates(apportionPos, HfMassArray, MLoadArray, HitArray, cDecimalPlaces.Hfreo, cDecimalPlaces.Hfrhi, HfRateEoArray, HfRateHiArray);
      }
    }

    /// <summary>
    /// Calculates and fills the given electrical output or heat input based rates if they are missing and values exist to calculate them.
    /// </summary>
    /// <param name="apportionPos">The position of the apportionment data to calculated.</param>
    /// <param name="massArray">The array of mass values to use in the calculation.</param>
    /// <param name="matsLoadArray">The array of MATS loads to use in the electrical output based calculations.</param>
    /// <param name="hitArray">The array of heat inputs to use in the heat input based calculations.</param>
    /// <param name="eoBasedRateDecimalPlaces">The number of decimals for the electrical output based rate.</param>
    /// <param name="hiBasedRateDecimalPlaces">The number of decimals for the heat input based rate.</param>
    /// <param name="eoBasedRateArray">The array of electrical output based rates containing the value to calculate.</param>
    /// <param name="hiBasedRateArray">The array of heat input based rates containing the value to calculate.</param>
    protected void FillMissingMatsRates(int apportionPos, 
                                        decimal?[] massArray, decimal?[] matsLoadArray, decimal?[] hitArray,
                                        int eoBasedRateDecimalPlaces, int hiBasedRateDecimalPlaces,
                                        decimal?[] eoBasedRateArray, decimal?[] hiBasedRateArray)
    {
      /* Set Electrical Output rate if it does not exist and the mass and MATS load values exist. */
      if (!eoBasedRateArray[apportionPos].HasValue)
      {
        if (massArray[apportionPos].HasValue && matsLoadArray[apportionPos].HasValue && (matsLoadArray[apportionPos].Value > 0) && (OpTimeArray[apportionPos].Value > 0))
          eoBasedRateArray[apportionPos] = Math.Round(1000 * massArray[apportionPos].Value / (matsLoadArray[apportionPos].Value * OpTimeArray[apportionPos].Value), eoBasedRateDecimalPlaces, MidpointRounding.AwayFromZero);
      }

      /* Set Heat Input rate if it is null and the mass and Heat Input values exist. */
      if (!hiBasedRateArray[apportionPos].HasValue)
      {
        if (massArray[apportionPos].HasValue && hitArray[apportionPos].HasValue && (hitArray[apportionPos].Value > 0))
          hiBasedRateArray[apportionPos] = Math.Round(1000000 * massArray[apportionPos].Value / hitArray[apportionPos].Value, hiBasedRateDecimalPlaces, MidpointRounding.AwayFromZero);
      }
    }

    #endregion

  }

}
