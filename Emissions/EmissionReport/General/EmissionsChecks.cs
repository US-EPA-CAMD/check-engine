using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.EmissionsReport;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;
using ECMPS.Definitions.Extensions;


namespace ECMPS.Checks.EmissionsChecks
{

  /// <summary>
  /// The underlying class for all emissions checks
  /// </summary>
  public abstract class cEmissionsChecks : cChecks
  {

    #region Protected Constructor

    /// <summary>
    /// Creates an instance using the passed Emissions Report Process
    /// to populate properties.
    /// </summary>
    /// <param name="emissionsReportProcess">The Emissions Report Process used to populate properties.</param>
    protected cEmissionsChecks(cEmissionsReportProcess emissionsReportProcess)
    {
      CheckEngine = emissionsReportProcess.CheckEngine;
      EmManualParameters = emissionsReportProcess.EmManualParameters;
      ReportingPeriod = emissionsReportProcess.CheckEngine.ReportingPeriod;
    }


    /// <summary>
    /// Creates an instance.
    /// </summary>
    /// <param name="checkEngine">The current check engine.</param>
    /// <param name="emissionParameters">The current emission parameters.</param>
    protected cEmissionsChecks(cCheckEngine checkEngine, cEmissionsCheckParameters emissionParameters)
    {
      CheckEngine = checkEngine;
      EmManualParameters = emissionParameters;
      ReportingPeriod = checkEngine.ReportingPeriod;
    }


    /// <summary>
    /// Constructor used for testing.
    /// </summary>
    protected cEmissionsChecks()
    {
    }

    #endregion


    #region Public Properties: General

    /// <summary>
    /// The Check Engine object of the parent check process.
    /// </summary>
    public cCheckEngine CheckEngine { get; private set; }

    /// <summary>
    /// The Generation Check Parameters for this category.
    /// </summary>
    public cEmissionsCheckParameters EmManualParameters { get; protected set; }

    /// <summary>
    /// The Check Engine object of the parent check process.
    /// </summary>
    public cReportingPeriod ReportingPeriod { get; private set; }

    #endregion


    #region Protected Properties: Check Parameters

    protected cCheckParameterBooleanValue AnnualReportingRequirement { get { return EmManualParameters.AnnualReportingRequirement; } }
    protected cCheckParameterBooleanValue DailyIntNoPriorTestCheckIndicator { get { return EmManualParameters.DailyIntNoPriorTestCheckIndicator; } }
    protected cCheckParameterBooleanValue DailyIntStatusRequired { get { return EmManualParameters.DailyIntStatusRequired; } }
    protected cCheckParameterBooleanValue EmTestDateValid { get { return EmManualParameters.EmTestDateValid; } }
    protected cCheckParameterBooleanValue EmTestHourValid { get { return EmManualParameters.EmTestHourValid; } }
    protected cCheckParameterBooleanValue EvaluateUpscaleInjection { get { return EmManualParameters.EvaluateUpscaleInjection; } }
    protected cCheckParameterBooleanValue F2lStatusEventRequiresAbbreviatedCheck { get { return EmManualParameters.F2lStatusEventRequiresAbbreviatedCheck; } }
    protected cCheckParameterBooleanValue F2lStatusEventRequiresRata { get { return EmManualParameters.F2lStatusEventRequiresRata; } }
    protected cCheckParameterBooleanValue F2lStatusRequired { get { return EmManualParameters.F2lStatusRequired; } }
    protected cCheckParameterBooleanValue IgnoredDailyInterferenceTests { get { return EmManualParameters.IgnoredDailyInterferenceTests; } }
    protected cCheckParameterBooleanValue LeakStatusRequired { get { return EmManualParameters.LeakStatusRequired; } }
    protected cCheckParameterBooleanValue MpSuccessfullyEvaluated { get { return EmManualParameters.MpSuccessfullyEvaluated; } }
    protected cCheckParameterBooleanValue UpscaleGasTypeValid { get { return EmManualParameters.UpscaleGasTypeValid; } }

    protected cCheckParameterDataRowViewValue CurrentDailyCalibrationTest { get { return EmManualParameters.CurrentDailyCalibrationTest; } }
    protected cCheckParameterDataRowViewValue CurrentDailyEmissionTest { get { return EmManualParameters.CurrentDailyEmissionTest; } }
    protected cCheckParameterDataRowViewValue CurrentDhvRecord { get { return EmManualParameters.CurrentDhvRecord; } }
    protected cCheckParameterDataRowViewValue CurrentFlowToLoadStatusCheck { get { return EmManualParameters.CurrentFlowToLoadStatusCheck; } }
    protected cCheckParameterDataRowViewValue CurrentHourlyRecordforRataStatus { get { return EmManualParameters.CurrentHourlyRecordforRataStatus; } }
    protected cCheckParameterDataRowViewValue CurrentMhvRecord { get { return EmManualParameters.CurrentMhvRecord; } }
    protected cCheckParameterDataRowViewValue F2lStatusEarliestValidRequiredTest { get { return EmManualParameters.F2lStatusEarliestValidRequiredTest; } }
    protected cCheckParameterDataRowViewValue F2lStatusInterveningRata { get { return EmManualParameters.F2lStatusInterveningRata; } }
    protected cCheckParameterDataRowViewValue F2lStatusMostRecentConditionalDataEvent { get { return EmManualParameters.F2lStatusMostRecentConditionalDataEvent; } }
    protected cCheckParameterDataRowViewValue F2lStatusQaCertEvent { get { return EmManualParameters.F2lStatusQaCertEvent; } }
    protected cCheckParameterDataRowViewValue OfflineDailyIntRecord { get { return EmManualParameters.OfflineDailyIntRecord; } }
    protected cCheckParameterDataRowViewValue OnlineDailyIntRecord { get { return EmManualParameters.OnlineDailyIntRecord; } }
    protected cCheckParameterDataRowViewValue PriorLeakEventRecord { get { return EmManualParameters.PriorLeakEventRecord; } }
    protected cCheckParameterDataRowViewValue PriorLeakRecord { get { return EmManualParameters.PriorLeakRecord; } }
    
    protected cCheckParameterDataViewLegacy AppendixETestRecordsByLocationForQaStatus { get { return EmManualParameters.AppendixETestRecordsByLocationForQaStatus; } }
    protected cCheckParameterDataViewLegacy CrossCheckProtocolGasParameterToType { get { return EmManualParameters.CrossCheckProtocolGasParameterToType; } }
    protected cCheckParameterDataViewLegacy CrossCheckTestTypeToRequiredTestCode { get { return EmManualParameters.CrossCheckTestTypeToRequiredTestCode; } }
    protected cCheckParameterDataViewLegacy F2lCheckRecordsForQaStatus { get { return EmManualParameters.F2lCheckRecordsForQaStatus; } }
    protected cCheckParameterDataViewLegacy GasComponentCodeLookupTable { get { return EmManualParameters.GasComponentCodeLookupTable; } }
    protected cCheckParameterDataViewLegacy GasTypeCodeLookupTable { get { return EmManualParameters.GasTypeCodeLookupTable; } }
    protected cCheckParameterDataViewLegacy HourlyNonOperatingDataRecordsforLocation { get { return EmManualParameters.HourlyNonOperatingDataRecordsforLocation; } }
    protected cCheckParameterDataViewLegacy HourlyOperatingDataRecordsByHourLocation { get { return EmManualParameters.HourlyOperatingDataRecordsByHourLocation; } }
    protected cCheckParameterDataViewLegacy HourlyOperatingDataRecordsforLocation { get { return EmManualParameters.HourlyOperatingDataRecordsforLocation; } }
    protected cCheckParameterDataViewLegacy HourlyParamFuelFlowRecordsForHourLocation { get { return EmManualParameters.HourlyParamFuelFlowRecordsForHourLocation; } }
    protected cCheckParameterDataViewLegacy LeakCheckRecordsByLocationForQAStatus { get { return EmManualParameters.LeakCheckRecordsByLocationForQAStatus; } }
    protected cCheckParameterDataViewLegacy LocationReportingFrequencyRecords { get { return EmManualParameters.LocationReportingFrequencyRecords; } }
    protected cCheckParameterDataViewLegacy MpLocationNonLoadBasedRecords { get { return EmManualParameters.MpLocationNonLoadBasedRecords; } }
    protected cCheckParameterDataViewLegacy MpQualificationPercentRecords { get { return EmManualParameters.MpQualificationPercentRecords; } }
    protected cCheckParameterDataViewLegacy MpQualificationRecords { get { return EmManualParameters.MpQualificationRecords; } }
    protected cCheckParameterDataViewLegacy MpSystemComponentRecords { get { return EmManualParameters.MpSystemComponentRecords; } }
    protected cCheckParameterDataViewLegacy OperatingSuppDataRecordsByLocation { get { return EmManualParameters.OperatingSuppDataRecordsByLocation; } }
    protected cCheckParameterDataViewLegacy ProtocolGasVendorLookupTable { get { return EmManualParameters.ProtocolGasVendorLookupTable; } }
    protected cCheckParameterDataViewLegacy QaCertificationEventRecords { get { return EmManualParameters.QaCertificationEventRecords; } }
    protected cCheckParameterDataViewLegacy RataTestRecordsByLocationForQaStatus { get { return EmManualParameters.RataTestRecordsByLocationForQaStatus; } }
    protected cCheckParameterDataViewLegacy SystemParameterLookupTable { get { return EmManualParameters.SystemParameterLookupTable; } }
    protected cCheckParameterDataViewLegacy TestExtensionExemptionRecords { get { return EmManualParameters.TestExtensionExemptionRecords; } }
    protected cCheckParameterDataViewLegacy UnitDefaultTestRecordsByLocationForQaStatus { get { return EmManualParameters.UnitDefaultTestRecordsByLocationForQaStatus; } }
    protected cCheckParameterDataViewLegacy FuelTypeRealityChecksforGCVCrossCheckTable { get { return EmManualParameters.FuelTypeRealityChecksforGCVCrossCheckTable; } }
    protected cCheckParameterDataViewLegacy FuelTypeWarningLevelsforGCVCrossCheckTable { get { return EmManualParameters.FuelTypeWarningLevelsforGCVCrossCheckTable; } }

    protected cCheckParameterDateValue CurrentOperatingDate { get { return EmManualParameters.CurrentOperatingDate; } }
    protected cCheckParameterDateValue CurrentReportingPeriodBeginHour { get { return EmManualParameters.CurrentReportingPeriodBeginHour; } }
    protected cCheckParameterDateValue CurrentReportingPeriodEndHour { get { return EmManualParameters.CurrentReportingPeriodEndHour; } }
    protected cCheckParameterDateValue DailyCalPgvpRuleDate { get { return EmManualParameters.DailyCalPgvpRuleDate; } }
    protected cCheckParameterDateValue EarliestLocationReportDate { get { return EmManualParameters.EarliestLocationReportDate; } }
    protected cCheckParameterDateValue FirstDayofOperation { get { return EmManualParameters.FirstDayofOperation; } }
    protected cCheckParameterDateValue PriorLeakExpirationDate { get { return EmManualParameters.PriorLeakExpirationDate; } }
    protected cCheckParameterDateValue UdefExpirationDate { get { return EmManualParameters.UdefExpirationDate; } }

    protected cCheckParameterDecimalValue CurrentOperatingTime { get { return EmManualParameters.CurrentOperatingTime; } }
    protected cCheckParameterDecimalValue OverrideRATABAF { get { return EmManualParameters.OverrideRATABAF; } }

    protected cCheckParameterIntegerValue CurrentMonitorPlanLocationPostion { get { return EmManualParameters.CurrentMonitorPlanLocationPostion; } }
    protected cCheckParameterIntegerValue CurrentOperatingHour { get { return EmManualParameters.CurrentOperatingHour; } }
    protected cCheckParameterIntegerValue CurrentReportingPeriod { get { return EmManualParameters.CurrentReportingPeriod; } }
    protected cCheckParameterIntegerValue CurrentReportingPeriodQuarter { get { return EmManualParameters.CurrentReportingPeriodQuarter; } }
    protected cCheckParameterIntegerValue CurrentReportingPeriodYear { get { return EmManualParameters.CurrentReportingPeriodYear; } }
    protected cCheckParameterIntegerValue F2lStatusPriorTestRequiredQuarter { get { return EmManualParameters.F2lStatusPriorTestRequiredQuarter; } }
    protected cCheckParameterIntegerValue FirstEcmpsReportingPeriod { get { return EmManualParameters.FirstEcmpsReportingPeriod; } }
    protected cCheckParameterIntegerValue FirstHourofOperation { get { return EmManualParameters.FirstHourofOperation; } }
    protected cCheckParameterIntegerValue LocationPos { get { return EmManualParameters.LocationPos; } }

    protected cCheckParameterObjectValue CurrentReportingPeriodObject { get { return EmManualParameters.CurrentReportingPeriodObject; } }
    protected cCheckParameterObjectValue F2lStatusSystemCheckDictionary { get { return EmManualParameters.F2lStatusSystemCheckDictionary; } }
    protected cCheckParameterObjectValue F2lStatusSystemMissingOpDictionary { get { return EmManualParameters.F2lStatusSystemMissingOpDictionary; } }
    protected cCheckParameterObjectValue F2lStatusSystemResultDictionary { get { return EmManualParameters.F2lStatusSystemResultDictionary; } }
    protected cCheckParameterObjectValue LastFailedOrAbortedDailyCalObject { get { return EmManualParameters.LastFailedOrAbortedDailyCalObject; } }
    protected cCheckParameterObjectValue LatestDailyInterferenceCheckObject { get { return EmManualParameters.LatestDailyInterferenceCheckObject; } }
    protected cCheckParameterObjectValue NonOperatingHoursByLocation { get { return EmManualParameters.NonOperatingHoursByLocation; } }
    protected cCheckParameterObjectValue OperatingHoursByLocation { get { return EmManualParameters.OperatingHoursByLocation; } }

    protected cCheckParameterStringArray FlowSystemIdArray { get { return EmManualParameters.FlowSystemIdArray; } }
    protected cCheckParameterStringArray LmeFuelArray { get { return EmManualParameters.LmeFuelArray; } }
    protected cCheckParameterStringArray NoxeSystemIdArray { get { return EmManualParameters.NoxeSystemIdArray; } }

    protected cCheckParameterStringValue CurrentMonitorLocationId { get { return EmManualParameters.CurrentMonitorLocationId; } }
    protected cCheckParameterStringValue DailyIntStatusResult { get { return EmManualParameters.DailyIntStatusResult; } }
    protected cCheckParameterStringValue EmTestCalcResult { get { return EmManualParameters.EmTestCalcResult; } }
    protected cCheckParameterStringValue ExpirationText { get { return EmManualParameters.ExpirationText; } }
    protected cCheckParameterStringValue ExpiredSystems { get { return EmManualParameters.ExpiredSystems; } }
    protected cCheckParameterStringValue ExpiringSystems { get { return EmManualParameters.ExpiringSystems; } }
    protected cCheckParameterStringValue F2lStatusMissingOpDataInfo { get { return EmManualParameters.F2lStatusMissingOpDataInfo; } }
    protected cCheckParameterStringValue F2lStatusPriorTestRequiredQuarterMissingOpData { get { return EmManualParameters.F2lStatusPriorTestRequiredQuarterMissingOpData; } }
    protected cCheckParameterStringValue F2lStatusResult { get { return EmManualParameters.F2lStatusResult; } }
    protected cCheckParameterStringValue LeakMissingOpDataInfo { get { return EmManualParameters.LeakMissingOpDataInfo; } }
    protected cCheckParameterStringValue LeakStatusResult { get { return EmManualParameters.LeakStatusResult; } }
    protected cCheckParameterStringValue QualificationPercentMissingList { get { return EmManualParameters.QualificationPercentMissingList; } }
    protected cCheckParameterStringValue ProtocolGasBalanceComponentList { get { return EmManualParameters.ProtocolGasBalanceComponentList; } }
    protected cCheckParameterStringValue ProtocolGasDuplicateComponentList { get { return EmManualParameters.ProtocolGasDuplicateComponentList; } }
    protected cCheckParameterStringValue ProtocolGasExclusiveComponentList { get { return EmManualParameters.ProtocolGasExclusiveComponentList; } }
    protected cCheckParameterStringValue ProtocolGasInvalidComponentList { get { return EmManualParameters.ProtocolGasInvalidComponentList; } }
    protected cCheckParameterStringValue UdefStatus { get { return EmManualParameters.UdefStatus; } }
    
    #endregion


    #region Protected Properties: Miscellaneous

    /// <summary>
    /// The PRVP/AETB Rule Date from the System Parameter table.
    /// </summary>
    protected DateTime? PgvpAetbRuleDate
    {
      get
      {
        DateTime? result;
        {
          if (SystemParameterLookupTable.Value != null)
          {
            DataRowView pgvpAetbRuleDateRow = cRowFilter.FindRow(SystemParameterLookupTable.Value, new cFilterCondition[] { new cFilterCondition("SYS_PARAM_NAME", "PGVP_AETB_RULE_DATE") });

            DateTime pgvpAetbRuleDate;

            if ((pgvpAetbRuleDateRow != null) &&
                DateTime.TryParse(pgvpAetbRuleDateRow["PARAM_VALUE1"].AsString(), out pgvpAetbRuleDate))
              result = pgvpAetbRuleDate;
            else
              result = null;
          }
          else
            result = null;
        }

        return result;
      }
    }

    #endregion


    #region Protected Methods: Tolerance Utilities

    /// <summary>
    /// Returns the Hourly Emissions Tolerance for the given parameter and UOM.
    /// </summary>
    /// <param name="parameterCd">The parameter of the tolerance.</param>
    /// <param name="uom">The unit of measure of the tolerance.</param>
    /// <param name="category">The category of the calling check.</param>
    /// <returns>The tolerance or null if none is found.</returns>
    protected static decimal? GetHourlyEmissionsTolerance(string parameterCd, String uom, cCategory category)
    {
      decimal? result;

      DataView toleranceView = category.GetCheckParameter("Hourly_Emissions_Tolerances_Cross_Check_Table").AsDataView();
      DataRowView toleranceRow;

      if ((toleranceView != null) &&
          cRowFilter.FindRow(toleranceView,
                             new cFilterCondition[] { new cFilterCondition("Parameter", parameterCd),
                                                      new cFilterCondition("UOM", uom)},
                             out toleranceRow))
      {
        result = toleranceRow["Tolerance"].AsDecimal();
      }
      else
      {
        result = null;
      }

      return result;
    }


    /// <summary>
    /// Determines whether the two passed values are within the tolerance for the passed parameter code and unit of measure.
    /// </summary>
    /// <param name="value1">One of two values to compare.</param>
    /// <param name="value2">One of two values to compare.</param>
    /// <param name="parameterCd">The parameter of the tolerance.</param>
    /// <param name="uom">The unit of measure of the tolerance.</param>
    /// <param name="category">The category of the calling check.</param>
    /// <returns>Returns true if the values are with tolerance.</returns>
    protected static bool TestHourlyEmissionsTolerance(decimal value1, decimal value2, string parameterCd, string uom, cCategory category)
    {
      bool result;

      decimal? tolerance = GetHourlyEmissionsTolerance(parameterCd, uom, category);

      result = tolerance.HasValue && (Math.Abs(value1 - value2) <= tolerance);

      return result;
    }

    #endregion

  }

}
