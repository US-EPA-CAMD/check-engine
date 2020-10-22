using System;
using System.Data;

using ECMPS.DM.Miscellaneous;
using ECMPS.DM.Utilities;

namespace ECMPS.DM.Definitions
{

  /// <summary>
  /// Delegate used for methods that apportion hours.
  /// </summary>
  /// <param name="date">The date of the hour to apportion.</param>
  /// <param name="hour">The hour to apportion.</param>
  /// <returns>Returns false if the apportionment did not complete,
  ///          but true otherwise even if apportionment errors occurred.</returns>
  public delegate bool dApportion(DateTime date, int hour);

  /// <summary>
  /// Loads the Monitor Data Mart Emissions tables, and returns the data needed to
  /// load the Unit Hour table.
  /// </summary>
  /// <param name="monPlanId">MON_PLAN_ID of the emission report.</param>
  /// <param name="rptPeriodId">RPT_PERIOD_ID of the emission report.</param>
  /// <param name="unitInfo"></param>
  /// <param name="locationInfo"></param>
  /// <param name="factorFormulaeArray"></param>
  /// <param name="errorMessage">The error message if an error occurs.</param>
  /// <returns>True if the SP executed without error.</returns>
  public delegate bool dGetFactorFormulaeArray(string monPlanId, int rptPeriodId,
                                               cUnitInfo[] unitInfo, cLocationInfo[] locationInfo,
                                               out cFactorFormulae[] factorFormulaeArray,
                                               ref string errorMessage);

  /// <summary>
  /// Delegate used for methods that log errors.
  /// </summary>
  /// <param name="message">The error message to log.</param>
  /// <param name="detail">Additional detail about the error.</param>
  /// <param name="opDate">The op date of the data that caused the error.</param>
  /// <param name="opHour">The op hour of the data that caused the error.</param>
  /// <param name="locationKey">The location key of the data that caused the error.</param>
  /// <param name="erroredMethod">The method that caused the error.</param>
  /// <returns>Returns false if the error was not logged.</returns>
  public delegate bool dLogError(string message, string detail,
                                 DateTime? opDate, int? opHour, string locationKey,
                                 string erroredMethod);

  /// <summary>
  /// Delegate for the method called to update the check log, set apportionment type, and set emissions created to 'N'.
  /// </summary>
  /// <param name="dmEmissionsId">The DM_EMISSIONS_ID of the update.</param>
  /// <param name="apportionmentType">The apportionment type of the update.</param>
  public delegate void dUpdateFailure(string dmEmissionsId,
                                      eApportionmentType? apportionmentType);

  /// <summary>
  /// Delegate for the method called to initialize the DM Update process.
  /// </summary>
  /// <param name="monPlanId">The monitor plan of the emissions report to update.</param>
  /// <param name="rptPeriodId">The reporting period of the emissions report to update.</param>
  /// <param name="userId">The user name to use for any created rows.</param>
  /// <param name="dmEmissionsId">The DM_EMISSIONS_ID to store in created rows.</param>
  /// <param name="dataSource">The Data Source to store in created rows.</param>
  /// <param name="isMatsEmissionReport">Indicates whether the emission report is a MATS report.</param>
  /// <param name="locationTable">Table of location information for the monitor plan.</param>
  /// <param name="rptPeriodInfoTable">Table with a row containing reporting period information.</param>
  /// <param name="locationTypeCountTable">Table with a row containing counts of the location types.</param>
  /// <param name="locationLinkSpanCountTable">Table with a row containing counts of location links that span the reporting period.</param>
  /// <param name="locationLinkActiveTable">Table with a row containing counts of location links active during the reporting period.</param>
  /// <param name="specialMethodTable">Table with a row containg information about special methods.</param>
  /// <param name="monitorHourTable">The monitor hour information for the emission report.</param>
  /// <param name="errorMessage">The error message returned if the init failed.</param>
  /// <returns>False if the init failed.</returns>
  public delegate bool dUpdateInit(string monPlanId, int rptPeriodId, string userId,
                                   out string dmEmissionsId,
                                   out string dataSource,
                                   out bool? isMatsEmissionReport,
                                   out DataTable locationTable,
                                   out DataTable rptPeriodInfoTable,
                                   out DataTable locationTypeCountTable,
                                   out DataTable locationLinkSpanCountTable,
                                   out DataTable locationLinkActiveTable,
                                   out DataTable specialMethodTable,
                                   out DataTable monitorHourTable,
                                   ref string errorMessage);

  /// <summary>
  /// Delegate for the method called to update the apportionment type, unit hour data 
  /// and check log.
  /// </summary>
  /// <param name="dmEmissionsId">The DM_EMISSIONS_ID of the update.</param>
  /// <param name="apportionmentType">The apportionment type of the update.</param>
  /// <param name="isMatsEmissionReport">Indicates whether the emission report is a MATS report.</param>
  /// <param name="unitKeyArray">The UNIT_ID update array.</param>
  /// <param name="dmEmissionsIdArray">The DM_EMISSIONS_ID update array.</param>
  /// <param name="opDateArray">The op date update array.</param>
  /// <param name="opHourArray">The op hour update array.</param>
  /// <param name="opTimeArray">The op time update array.</param>
  /// <param name="gLoadArray">The g-load update array.</param>
  /// <param name="mLoadArray">The MATS load update array.</param>
  /// <param name="sLoadArray">The s-load update array.</param>
  /// <param name="tLoadArray">The t-load update array.</param>
  /// <param name="hitArray">The HIT update array.</param>
  /// <param name="hitMeasureArray">The HIT hour measure update array.</param>
  /// <param name="so2mArray">The SO2M update array.</param>
  /// <param name="so2mMeasureArray">The SO2M hour measure update array.</param>
  /// <param name="so2rArray">The SO2R update array.</param>
  /// <param name="so2rMeasureArray">The SO2R hour measure update array.</param>
  /// <param name="co2mArray">The CO2M update array.</param>
  /// <param name="co2mMeasureArray">The CO2M hour measure update array.</param>
  /// <param name="co2rArray">The CO2R update array.</param>
  /// <param name="co2rMeasureArray">The CO2R hour measure update array.</param>
  /// <param name="noxmArray">The NOXM update array.</param>
  /// <param name="noxmMeasureArray">The NOXM hour measure update array.</param>
  /// <param name="noxrArray">The NOXR update array.</param>
  /// <param name="noxrMeasureArray">The NOXR hour measure update array.</param>
  /// <param name="hgRateEoArray">The Hg Rate Electrical Output update array.</param>
  /// <param name="hgRateHiArray">The Hg Rate Heat Input update array.</param>
  /// <param name="hgMassArray">The Hg Mass update array.</param>
  /// <param name="hgMeasureArray">The Hg hour measure update array.</param>
  /// <param name="hclRateEoArray">The HCl Rate Electrical Output update array.</param>
  /// <param name="hclRateHiArray">The HCl Rate Heat Input update array.</param>
  /// <param name="hclMassArray">The HCl Mass update array.</param>
  /// <param name="hclMeasureArray">The HCl hour measure update array.</param>
  /// <param name="hfRateEoArray">The HF Rate Electrical Output update array.</param>
  /// <param name="hfRateHiArray">The HF Rate Heat Input update array.</param>
  /// <param name="hfMassArray">The HF Mass update array.</param>
  /// <param name="hfMeasureArray">The HF hour measure update array.</param>
  /// <param name="monPlanIdArray">The MON_PLAN_ID update array.</param>
  /// <param name="rptPeriodIdArray">The RPT_PERIOD_ID update array.</param>
  /// <param name="opYearArray">The op year update array.</param>
  /// <param name="dataSourceArray">The data source update array.</param>
  /// <param name="userIdArray">The user id update array.</param>
  public delegate void dUpdateSuccess(string dmEmissionsId,
                                      eApportionmentType apportionmentType,
                                      bool? isMatsEmissionReport,
                                      string[] dmEmissionsIdArray,
                                      int?[] unitKeyArray,
                                      DateTime?[] opDateArray,
                                      int?[] opHourArray,
                                      decimal?[] opTimeArray,
                                      decimal?[] gLoadArray,
                                      decimal?[] mLoadArray,
                                      decimal?[] sLoadArray,
                                      decimal?[] tLoadArray,
                                      decimal?[] hitArray,
                                      string[] hitMeasureArray,
                                      decimal?[] so2mArray,
                                      string[] so2mMeasureArray,
                                      decimal?[] so2rArray,
                                      string[] so2rMeasureArray,
                                      decimal?[] co2mArray,
                                      string[] co2mMeasureArray,
                                      decimal?[] co2rArray,
                                      string[] co2rMeasureArray,
                                      decimal?[] noxmArray,
                                      string[] noxmMeasureArray,
                                      decimal?[] noxrArray,
                                      string[] noxrMeasureArray,
                                      decimal?[] hgRateEoArray,
                                      decimal?[] hgRateHiArray,
                                      decimal?[] hgMassArray,
                                      string[] hgMeasureArray,
                                      decimal?[] hclRateEoArray,
                                      decimal?[] hclRateHiArray,
                                      decimal?[] hclMassArray,
                                      string[] hclMeasureArray,
                                      decimal?[] hfRateEoArray,
                                      decimal?[] hfRateHiArray,
                                      decimal?[] hfMassArray,
                                      string[] hfMeasureArray,
                                      string[] monPlanIdArray,
                                      int?[] rptPeriodIdArray,
                                      int?[] opYearArray,
                                      string[] dataSourceArray,
                                      string[] userIdArray);

}
