using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;

using ECMPS.Definitions.Extensions;
using ECMPS.DM.Miscellaneous;
using ECMPS.DM.Utilities;

namespace ECMPS.DM.HourlyEmissions
{

  /// <summary>
  /// Class used to contain monitor hour data needed
  /// to create unit hour data.
  /// </summary>
  public class cHourlyRawData
  {

    #region Public Constructors

    /// <summary>
    /// Creates an object to hold the monitor hour data needed
    /// to create unit hour data.
    /// </summary>
    /// <param name="dmEmissionsId">The DM_EMISSIONS_ID for the emission report.</param>
    /// <param name="monPlanId">The montior plan id of the emission report.</param>
    /// <param name="rptPeriodId">The reporting period of the emission report.</param>
    /// <param name="locationInfo">Array of location information objects.</param>
    /// <param name="year">The year of the reporting period.</param>
    /// <param name="quarter">The quarter of the reporting period.</param>
    /// <param name="dataSource">The data source to assign to the unit hour data.</param>
    /// <param name="userId">The user id to assign to the unit hour data.</param>
    public cHourlyRawData(string dmEmissionsId, 
                          string monPlanId, int rptPeriodId,
                          cLocationInfo[] locationInfo, 
                          int year, int quarter,
                          string dataSource, string userId)
    {
      LocationInfo = locationInfo;

      // Init UnitInfo and UnitCount
      {
        cUnitInfo[] unitInfo;

        int unitCount = 0;
        {
          foreach (cLocationInfo locationItem in LocationInfo)
            if (locationItem.UnitKey.HasValue) unitCount += 1;
        }

        unitInfo = new cUnitInfo[unitCount];

        int unitDex = 0;

        for (int locationDex = 0; locationDex < LocationInfo.Length; locationDex++)
        {
          cLocationInfo locationItem = LocationInfo[locationDex];

          if (locationItem.UnitKey.HasValue)
          {
            unitInfo[unitDex] = new cUnitInfo(locationItem.LocationKey,
                                              locationItem.UnitKey,
                                              locationItem.LocationName,
                                              locationDex);

            unitDex += 1;
          }
        }

        UnitInfo = unitInfo;
        UnitCount = unitCount;
      }

      DmEmissionsId = dmEmissionsId;
      MonPlanId = monPlanId;
      RptPeriodId = rptPeriodId;
      Year = year;
      Quarter = quarter;
      DataSource = dataSource;
      UserId = userId;

      QuarterBeganDate = (new DateTime(year, 3 * (quarter - 1) + 1, 1));
      QuarterEndedDate = QuarterBeganDate.AddMonths(3).AddDays(-1); ;

      DateCount = cUtilities.ElapsedDays(QuarterBeganDate, QuarterEndedDate) + 1;
      HourCount = DateCount * 24;
      LocationCount = locationInfo.Length;
      DataLength = LocationCount * HourCount;

      // Initialize General Data Arrays
      {
        OpTimeArray = new decimal?[DataLength];

        GLoadArray = new decimal?[DataLength];
        MLoadArray = new decimal?[DataLength];
        SLoadArray = new decimal?[DataLength];
        TLoadArray = new decimal?[DataLength];

        HitArray = new decimal?[DataLength];
        HitMeasureArray = new string[DataLength];
        HitffArray = new decimal?[DataLength];
      }

      // Initialize Part 75 Data Array
      {

        So2mArray = new decimal?[DataLength];
        So2mMeasureArray = new string[DataLength];
        Co2mArray = new decimal?[DataLength];
        Co2mMeasureArray = new string[DataLength];
        NoxmArray = new decimal?[DataLength];
        NoxmMeasureArray = new string[DataLength];
        NoxrArray = new decimal?[DataLength];
        NoxrMeasureArray = new string[DataLength];
      }

      // Initialize MATS Data Array
      {
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
      }
    }

    #endregion


    #region Public Properties

    /// <summary>
    /// The length of the arrays used to store the monitor hour data.
    /// </summary>
    public int DataLength { get; private set; }

    /// <summary>
    /// The data source to assign to the unit hour data.
    /// </summary>
    public string DataSource { get; private set; }

    /// <summary>
    /// The number of days in the emission report's quarter.
    /// </summary>
    public int DateCount { get; private set; }

    /// <summary>
    /// The DM_EMISSIONS_ID to assign to new rows.
    /// </summary>
    public string DmEmissionsId { get; private set; }

    /// <summary>
    /// The number of hours in the emission report's quarter.
    /// </summary>
    public int HourCount { get; private set; }

    /// <summary>
    /// The number of locations involved in the emissions report.
    /// 
    /// Basically the number of items in LocationInfo.
    /// </summary>
    public int LocationCount { get; private set; }

    /// <summary>
    /// Array of location information objects.
    /// </summary>
    public cLocationInfo[] LocationInfo { get; private set; }

    /// <summary>
    /// The Monitor Plan Id of the emissions report.
    /// </summary>
    public string MonPlanId { get; private set; }

    /// <summary>
    /// The quarter of the emissions report.
    /// </summary>
    public int Quarter { get; private set; }

    /// <summary>
    /// The began date of the emissions report quarter.
    /// </summary>
    public DateTime QuarterBeganDate { get; private set; }

    /// <summary>
    /// The ended date of the emissions report quarter.
    /// </summary>
    public DateTime QuarterEndedDate { get; private set; }

    /// <summary>
    /// The reporting period id of the emissions report.
    /// </summary>
    public int RptPeriodId { get; private set; }

    /// <summary>
    /// The number of units involved in the emissions report.
    /// 
    /// Basically the number of items in LocationInfo.
    /// </summary>
    public int UnitCount { get; private set; }

    /// <summary>
    /// Array of unit information objects.
    /// </summary>
    public cUnitInfo[] UnitInfo { get; private set; }

    /// <summary>
    /// The user id to assign to the unit hour data.
    /// </summary>
    public string UserId { get; private set; }

    /// <summary>
    /// The year of the emissions report.
    /// </summary>
    public int Year { get; private set; }

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

    /// <summary>
    /// The array of Monitor Hour HIT From Fuel Flow
    /// </summary>
    public decimal?[] HitffArray { get; private set; }

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
    /// The array of Monitor Hour CO2M
    /// </summary>
    public decimal?[] Co2mArray { get; private set; }

    /// <summary>
    /// The array of Monitor Hour CO2M Hour Measure Code
    /// </summary>
    public string[] Co2mMeasureArray { get; private set; }

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
    /// Returns the offset into the data arrays for a particular date and hour.
    /// </summary>
    /// <param name="date">The date of the offest.</param>
    /// <param name="hour">The hour of the offest.</param>
    /// <returns>The offset if the date is in the quarter 
    /// and the hour is between 0 and 23 inclusive.</returns>
    public int? GetHourOffset(DateTime date, int hour)
    {
      int? result;

      int dateOffset = cUtilities.ElapsedDays(QuarterBeganDate, date);

      if ((dateOffset < 0) || (dateOffset >= DateCount))
      {
        cErrorHandler.LogError(string.Format("Date is not in quarter {1}q{2}: '{0}' ({3})",
                                                date.ToShortDateString(), Year, Quarter,
                                                "cHourlyRawData.GetDataPosition"),
                                  date, hour);
        result = null;
      }
      else if ((hour < 0) || (hour >= 24))
      {
        cErrorHandler.LogError(string.Format("Hour value is not an hour: '{0}' ({1})",
                                                hour, "cHourlyRawData.GetDataPosition"),
                                  date, hour);
        result = null;
      }
      else
      {
        result = LocationCount * ((24 * dateOffset) + hour);
      }

      return result;
    }

    /// <summary>
    /// Gets the offset into the data arrays for a particular date and hour.
    /// </summary>
    /// <param name="date">The date of the offest.</param>
    /// <param name="hour">The hour of the offest.</param>
    /// <param name="hourOffset">The offset if the date is in the quarter 
    ///                          and the hour is between 0 and 23 inclusive.</param>
    /// <returns>True if an offset was returned.</returns>
    public bool GetHourOffset(DateTime date, int hour, out int? hourOffset)
    {
      bool result;

      hourOffset = GetHourOffset(date, hour);

      result = hourOffset.HasValue;

      return result;
    }

    /// <summary>
    /// Returns the postion for a date, hour and location in the data arrays.
    /// </summary>
    /// <param name="date">The date of the offest.</param>
    /// <param name="hour">The hour of the offest.</param>
    /// <param name="locationKey">The MON_LOC_ID of the location.</param>
    /// <returns>The position of the hour and location in the data arrays.</returns>
    public int? GetLocationPosition(DateTime date, int hour, string locationKey)
    {
      int? result;

      int dateOffset = cUtilities.ElapsedDays(QuarterBeganDate, date);

      int? locationPos = null;
      {
        for (int locationDex = 0; locationDex < LocationCount; locationDex++)
        {
          if (LocationInfo[locationDex].LocationKey == locationKey)
            locationPos = locationDex;
        }
      }

      if ((dateOffset < 0) || (dateOffset >= DateCount))
      {
        cErrorHandler.LogError(string.Format("Date is not in quarter {1}q{2}: '{0}' ({3})",
                                                date.ToShortDateString(), Year, Quarter,
                                                "cHourlyRawData.GetDataPosition"),
                                  date, hour, locationKey);
        result = null;
      }
      else if ((hour < 0) || (hour >= 24))
      {
        cErrorHandler.LogError(string.Format("Hour value is not an hour: '{0}' ({1})",
                                                hour, "cHourlyRawData.GetDataPosition"),
                                  date, hour, locationKey);
        result = null;
      }
      else if (!locationPos.HasValue)
      {
        cErrorHandler.LogError(string.Format("Location key not in [{1}]: '{0}' ({2})",
                                                locationKey, LocationInfo.AsList(),
                                                "cHourlyRawData.GetDataPosition"),
                                  date, hour, locationKey);
        result = null;
      }
      else
      {
        result = LocationCount * ((24 * dateOffset) + hour) + locationPos;
      }

      return result;
    }

    /// <summary>
    /// Gets the postion for a date, hour and location in the data arrays.
    /// </summary>
    /// <param name="date">The date of the offest.</param>
    /// <param name="hour">The hour of the offest.</param>
    /// <param name="locationKey">The MON_LOC_ID of the location.</param>
    /// <param name="locationPosition">The position of the hour and location in the data arrays.</param>
    /// <returns>True if a position was determined.</returns>
    public bool GetLocationPosition(DateTime date, int hour, string locationKey, out int? locationPosition)
    {
      bool result;

      locationPosition = GetLocationPosition(date, hour, locationKey);

      result = locationPosition.HasValue;

      return result;
    }

    /// <summary>
    /// Loads a monitor hour row into the data arrays.
    /// </summary>
    /// <param name="locationKey">The MON_LOC_ID value.</param>
    /// <param name="opDate">The OP_DATE value.</param>
    /// <param name="opHour">The OP_HOUR value.</param>
    /// <param name="opTime">The OP_TIME value.</param>
    /// <param name="gLoad">The GLOAD value.</param>
    /// <param name="mLoad">The MATS LOAD value.</param>
    /// <param name="sLoad">The SLOAD value.</param>
    /// <param name="tLoad">The TLOAD value.</param>
    /// <param name="hit">The HIT value.</param>
    /// <param name="hitMeasure">The HIT_HOUR_MEASURE_CD value.</param>
    /// <param name="hitFF">The HIT_FROM_FUEL_FLOW value.</param>
    /// <param name="so2m">The SO2M value.</param>
    /// <param name="so2mMeasure">The SO2M_HOUR_MEASURE_CD value.</param>
    /// <param name="co2m">The CO2M value.</param>
    /// <param name="co2mMeasure">The CO2M_HOUR_MEASURE_CD value.</param>
    /// <param name="noxm">The NOXM value.</param>
    /// <param name="noxmMeasure">The NOXM_HOUR_MEASURE_CD value.</param>
    /// <param name="noxr">The NOXR value.</param>
    /// <param name="noxrMeasure">The NOXR_HOUR_MEASURE_CD value.</param>
    /// <param name="hgRateEo">Hg Rate Electrical Output value.</param>
    /// <param name="hgRateHi">Hg Rate Heat Input value.</param>
    /// <param name="hgMass">Hg Mass value.</param>
    /// <param name="hgMeasure">Hg Measure Code value.</param>
    /// <param name="hclRateEo">HCl Rate Electrical Output value.</param>
    /// <param name="hclRateHi">HCl Rate Heat Input value.</param>
    /// <param name="hclMass">HCl Mass value.</param>
    /// <param name="hclMeasure">HCl Measure Code value.</param>
    /// <param name="hfRateEo">HF Rate Electrical Output value.</param>
    /// <param name="hfRateHi">HF Rate Heat Input value.</param>
    /// <param name="hfMass">HF Mass value.</param>
    /// <param name="hfMeasure">HF Measure Code value.</param>
    /// <returns>False if the row was not loaded.</returns>
    public bool Update(string locationKey,
                       DateTime? opDate,
                       int? opHour,
                       decimal? opTime,
                       decimal? gLoad,
                       decimal? mLoad,
                       decimal? sLoad,
                       decimal? tLoad,
                       decimal? hit,
                       string hitMeasure,
                       decimal? hitFF,
                       decimal? so2m,
                       string so2mMeasure,
                       decimal? co2m,
                       string co2mMeasure,
                       decimal? noxm,
                       string noxmMeasure,
                       decimal? noxr,
                       string noxrMeasure,
                       decimal? hgRateEo,
                       decimal? hgRateHi,
                       decimal? hgMass,
                       string hgMeasure,
                       decimal? hclRateEo,
                       decimal? hclRateHi,
                       decimal? hclMass,
                       string hclMeasure,
                       decimal? hfRateEo,
                       decimal? hfRateHi,
                       decimal? hfMass,
                       string hfMeasure)
    {
      bool result;

      if (!opDate.HasValue)
      {
        cErrorHandler.LogError(string.Format("Date is null ({0})", "cHourlyRawData.Update"));
        result = false;
      }
      else if (!opHour.HasValue)
      {
        cErrorHandler.LogError(string.Format("Hour is null ({0})", "cHourlyRawData.Update"));
        result = false;
      }
      else if (!locationKey.HasValue())
      {
        cErrorHandler.LogError(string.Format("Location name is null ({0})", "cHourlyRawData.Update"));
        result = false;
      }
      else
      {
        int? dataDex = GetLocationPosition(opDate.Value, opHour.Value, locationKey);

        if (dataDex.HasValue)
        {
          OpTimeArray[dataDex.Value] = opTime;
          GLoadArray[dataDex.Value] = gLoad;
          MLoadArray[dataDex.Value] = mLoad;
          SLoadArray[dataDex.Value] = sLoad;
          TLoadArray[dataDex.Value] = tLoad;

          HitArray[dataDex.Value] = hit;
          HitMeasureArray[dataDex.Value] = hitMeasure;
          HitffArray[dataDex.Value] = hitFF;

          So2mArray[dataDex.Value] = so2m;
          So2mMeasureArray[dataDex.Value] = so2mMeasure;

          Co2mArray[dataDex.Value] = co2m;
          Co2mMeasureArray[dataDex.Value] = co2mMeasure;

          NoxmArray[dataDex.Value] = noxm;
          NoxmMeasureArray[dataDex.Value] = noxmMeasure;
          NoxrArray[dataDex.Value] = noxr;
          NoxrMeasureArray[dataDex.Value] = noxrMeasure;

          HgRateEoArray[dataDex.Value] = hgRateEo;
          HgRateHiArray[dataDex.Value] = hgRateHi;
          HgMassArray[dataDex.Value] = hgMass;
          HgMeasureArray[dataDex.Value] = hgMeasure;

          HclRateEoArray[dataDex.Value] = hclRateEo;
          HclRateHiArray[dataDex.Value] = hclRateHi;
          HclMassArray[dataDex.Value] = hclMass;
          HclMeasureArray[dataDex.Value] = hclMeasure;

          HfRateEoArray[dataDex.Value] = hfRateEo;
          HfRateHiArray[dataDex.Value] = hfRateHi;
          HfMassArray[dataDex.Value] = hfMass;
          HfMeasureArray[dataDex.Value] = hfMeasure;

          result = true;
        }
        else
          result = false;
      }

      return result;
    }

    /// <summary>
    /// Loads a monitor hour table into the data arrays.
    /// </summary>
    /// <param name="table">The table to load</param>
    /// <returns>False if the table was not completely loaded.</returns>
    public bool Update(DataTable table)
    {
      bool result;

      if (table == null)
      {
        cErrorHandler.LogError("Data reader is null (cHourlyData.Update)");
        result = false;
      }
      else
      {
        result = true;

        foreach (DataRow row in table.Rows)
        {
          try
          {
            if (!Update(row["MON_LOC_ID"].AsString(),
                        row["OP_DATE"].AsDateTime(),
                        row["OP_HOUR"].AsInteger(),
                        row["OP_TIME"].AsDecimal(),
                        row["GLOAD"].AsDecimal(),
                        row["MATS_LOAD"].AsDecimal(),
                        row["SLOAD"].AsDecimal(),
                        row["TLOAD"].AsDecimal(),
                        row["HIT"].AsDecimal(),
                        row["HIT_HOUR_MEASURE_CD"].AsString(),
                        row["HIT_FROM_FUEL_FLOW"].AsDecimal(),
                        row["SO2M"].AsDecimal(),
                        row["SO2M_HOUR_MEASURE_CD"].AsString(),
                        row["CO2M"].AsDecimal(),
                        row["CO2M_HOUR_MEASURE_CD"].AsString(),
                        row["NOXM"].AsDecimal(),
                        row["NOXM_HOUR_MEASURE_CD"].AsString(),
                        row["NOXR"].AsDecimal(),
                        row["NOXR_HOUR_MEASURE_CD"].AsString(),
                        row["HG_RATE_EO"].AsDecimal(),
                        row["HG_RATE_HI"].AsDecimal(),
                        row["HG_MASS"].AsDecimal(),
                        row["HG_HOUR_MEASURE_CD"].AsString(),
                        row["HCL_RATE_EO"].AsDecimal(),
                        row["HCL_RATE_HI"].AsDecimal(),
                        row["HCL_MASS"].AsDecimal(),
                        row["HCL_HOUR_MEASURE_CD"].AsString(),
                        row["HF_RATE_EO"].AsDecimal(),
                        row["HF_RATE_HI"].AsDecimal(),
                        row["HF_MASS"].AsDecimal(),
                        row["HF_HOUR_MEASURE_CD"].AsString()))
            {
              result = false;
            }
          }
          catch (Exception ex)
          {
            cErrorHandler.LogError(string.Format("{0} (cHourlyData.Update)", ex.Message));
            result = false;
          }
        }
      }

      return result;
    }

    #endregion

  }

}
