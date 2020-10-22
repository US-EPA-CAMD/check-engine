using System;
using System.Collections.Generic;
using System.Text;

using ECMPS.Checks.Parameters;
using ECMPS.Checks.DatabaseAccess;

namespace ECMPS.Checks.EmissionsReport
{
  public class cEmissionsCheckParameters : cCheckParametersCheckEngine
  {

    #region Public Constructor

    public cEmissionsCheckParameters(cCheckProcess ACheckProcess, cDatabase ADatabaseAux)
      : base(ACheckProcess, ADatabaseAux)
    {
    }

	/// <summary>
	/// Constructor used for testing.
	/// </summary>
	public cEmissionsCheckParameters()
	{ }

    #endregion


    #region Public Properties: Boolean Parameters

    private void InstantiateCheckParameterProperties_Boolean()
    {
      AnnualReportingRequirement = new cCheckParameterBooleanValue(2850, "Annual_Reporting_Requirement", this);
      DailyIntNoPriorTestCheckIndicator = new cCheckParameterBooleanValue(3293, "Daily_Int_No_Prior_Test_Check_Indicator", this);
      DailyIntStatusRequired = new cCheckParameterBooleanValue(3288, "Daily_Int_Status_Required", this);
      EmTestDateValid = new cCheckParameterBooleanValue(2748, "EM_Test_Date_Valid", this);
      EmTestHourValid = new cCheckParameterBooleanValue(2749, "EM_Test_Hour_Valid", this);
      EvaluateUpscaleInjection = new cCheckParameterBooleanValue(2746, "Evaluate_Upscale_Injection", this);
      F2lStatusEventRequiresAbbreviatedCheck = new cCheckParameterBooleanValue(3285, "F2L_Status_Event_Requires_Abbreviated_Check", this);
      F2lStatusEventRequiresRata = new cCheckParameterBooleanValue(3282, "F2L_Status_Event_Requires_Rata", this);
      F2lStatusRequired = new cCheckParameterBooleanValue(3279, "F2L_Status_Required", this);
      IgnoredDailyInterferenceTests = new cCheckParameterBooleanValue(3248, "Ignored_Daily_Interference_Tests", this);
      LeakStatusRequired = new cCheckParameterBooleanValue(3294, "Leak_Status_Required", this);
      MpSuccessfullyEvaluated = new cCheckParameterBooleanValue(3263, "MP_Successfully_Evaluated", this);
      UpscaleGasTypeValid = new cCheckParameterBooleanValue(3223, "Upscale_Gas_Type_Valid", this);
    }


    /// <summary>
    /// Implements check parameter Annual_Reporting_Requirement (id = 2850)
    /// </summary>
    public cCheckParameterBooleanValue AnnualReportingRequirement { get; private set; }

    /// <summary>
    /// Implements check parameter Daily_Int_No_Prior_Test_Check_Indicator (id = 3293)
    /// </summary>
    public cCheckParameterBooleanValue DailyIntNoPriorTestCheckIndicator { get; private set; }

    /// <summary>
    /// Implements check parameter Daily_Int_Status_Required (id = 3288)
    /// </summary>
    public cCheckParameterBooleanValue DailyIntStatusRequired { get; private set; }

    /// <summary>
    /// Implements check parameter EM_Test_Date_Valid (id = 2748)
    /// </summary>
    public cCheckParameterBooleanValue EmTestDateValid { get; private set; }

    /// <summary>
    /// Implements check parameter EM_Test_Hour_Valid (id = 2749)
    /// </summary>
    public cCheckParameterBooleanValue EmTestHourValid { get; private set; }

    /// <summary>
    /// Implements check parameter Evaluate_Upscale_Injection (id = 2746)
    /// </summary>
    public cCheckParameterBooleanValue EvaluateUpscaleInjection { get; private set; }

    /// <summary>
    /// Implements check parameter F2L_Status_Event_Requires_Abbreviated_Check (id = 3285)
    /// </summary>
    public cCheckParameterBooleanValue F2lStatusEventRequiresAbbreviatedCheck { get; private set; }

    /// <summary>
    /// Implements check parameter F2l_Status_Event_Requires_Rata (id = 3282)
    /// </summary>
    public cCheckParameterBooleanValue F2lStatusEventRequiresRata { get; private set; }

    /// <summary>
    /// Implements check parameter F2L_Status_Required (id = 3279)
    /// </summary>
    public cCheckParameterBooleanValue F2lStatusRequired { get; private set; }

    /// <summary>
    /// Implements check parameter Ignored_Daily_Interference_Tests (id = 3248)
    /// </summary>
    public cCheckParameterBooleanValue IgnoredDailyInterferenceTests { get; private set; }

    /// <summary>
    /// Implements check parameter Leak_Status_Required (id = 3294)
    /// </summary>
    public cCheckParameterBooleanValue LeakStatusRequired { get; private set; }

    /// <summary>
    /// Implements check parameter MP_Successfully_Evaluated (id = 3263)
    /// </summary>
    public cCheckParameterBooleanValue MpSuccessfullyEvaluated { get; private set; }

    /// <summary>
    /// Implements check parameter Upscale_Gas_Type_Valid (id = 3223)
    /// </summary>
    public cCheckParameterBooleanValue UpscaleGasTypeValid { get; private set; }

    #endregion


    #region Public Properties: Data Row View Parameters

    private void InstantiateCheckParameterProperties_DataRowView()
    {
      CurrentDailyCalibrationTest = new cCheckParameterDataRowViewValue(2744, "Current_Daily_Calibration_Test", this);
      CurrentDailyEmissionTest = new cCheckParameterDataRowViewValue(2747, "Current_Daily_Emission_Test", this);
      CurrentDhvRecord = new cCheckParameterDataRowViewValue(2598, "Current_DHV_Record", this);
      CurrentFlowToLoadStatusCheck = new cCheckParameterDataRowViewValue(3272, "Current_Flow_to_Load_Status_Check", this);
      CurrentHourlyRecordforRataStatus = new cCheckParameterDataRowViewValue(2999, "Current_Hourly_Record_for_RATA_Status", this);
      CurrentMhvRecord = new cCheckParameterDataRowViewValue(2554, "Current_MHV_Record", this);
      F2lStatusEarliestValidRequiredTest = new cCheckParameterDataRowViewValue(3277, "F2L_Status_Earliest_Valid_Required_Test", this);
      F2lStatusQaCertEvent = new cCheckParameterDataRowViewValue(3275, "F2L_Status_Qa_Cert_Event ", this);
      F2lStatusInterveningRata = new cCheckParameterDataRowViewValue(3274, "F2L_Status_Intervening_Rata", this);
      F2lStatusMostRecentConditionalDataEvent = new cCheckParameterDataRowViewValue(3276, "F2L_Status_Most_Recent_Conditional_Data_Event", this);
      OfflineDailyIntRecord = new cCheckParameterDataRowViewValue(3291, "Offline_Daily_Int_Record", this);
      OnlineDailyIntRecord = new cCheckParameterDataRowViewValue(3290, "Online_Daily_Int_Record", this);
      PriorLeakEventRecord = new cCheckParameterDataRowViewValue(3299, "Prior_Leak_Event_Record", this);
      PriorLeakRecord = new cCheckParameterDataRowViewValue(3296, "Prior_Leak_Record", this);
    }


    /// <summary>
    /// Implements check parameter Current_Daily_Calibration_Test (id = 2744)
    /// </summary>
    public cCheckParameterDataRowViewValue CurrentDailyCalibrationTest { get; private set; }

    /// <summary>
    /// Implements check parameter Current_Daily_Emission_Test (id = 2747)
    /// </summary>
    public cCheckParameterDataRowViewValue CurrentDailyEmissionTest { get; private set; }

    /// <summary>
    /// Implements check parameter Current_Hourly_Record_for_RATA_Status (id = 2999)
    /// </summary>
    public cCheckParameterDataRowViewValue CurrentHourlyRecordforRataStatus { get; private set; }

    /// <summary>
    /// Implements check parameter Current_DHV_Record (id = 2598)
    /// </summary>
    public cCheckParameterDataRowViewValue CurrentDhvRecord { get; private set; }

    /// <summary>
    /// Implements check parameter Current_Flow_to_Load_Status_Check (id = 3272)
    /// </summary>
    public cCheckParameterDataRowViewValue CurrentFlowToLoadStatusCheck { get; private set; }

    /// <summary>
    /// Implements check parameter Current_MHV_Record (id = 2554)
    /// </summary>
    public cCheckParameterDataRowViewValue CurrentMhvRecord { get; private set; }

    /// <summary>
    /// Implements check parameter F2L_Status_Earliest_Valid_Required_Test (id = 3277)
    /// </summary>
    public cCheckParameterDataRowViewValue F2lStatusEarliestValidRequiredTest { get; private set; }

    /// <summary>
    /// Implements check parameter F2L_Status_Qa_Cert_Event  (id = 3275)
    /// </summary>
    public cCheckParameterDataRowViewValue F2lStatusQaCertEvent { get; private set; }

    /// <summary>
    /// Implements check parameter F2L_Status_Intervening_Rata (id = 3274)
    /// </summary>
    public cCheckParameterDataRowViewValue F2lStatusInterveningRata { get; private set; }

    /// <summary>
    /// Implements check parameter F2L_Status_Most_Recent_Conditional_Data_Event (id = 3276)
    /// </summary>
    public cCheckParameterDataRowViewValue F2lStatusMostRecentConditionalDataEvent { get; private set; }

    /// <summary>
    /// Implements check parameter Offline_Daily_Int_Record (id = 3291)
    /// </summary>
    public cCheckParameterDataRowViewValue OfflineDailyIntRecord { get; private set; }

    /// <summary>
    /// Implements check parameter Online_Daily_Int_Record (id = 3290)
    /// </summary>
    public cCheckParameterDataRowViewValue OnlineDailyIntRecord { get; private set; }

    /// <summary>
    /// Implements check parameter Prior_Leak_Event_Record (id = 3299) 
    /// </summary>
    public cCheckParameterDataRowViewValue PriorLeakEventRecord { get; private set; }

    /// <summary>
    /// Implements check parameter Prior_Leak_Record (id = 3296) 
    /// </summary>
    public cCheckParameterDataRowViewValue PriorLeakRecord { get; private set; }

    #endregion


    #region Public Properties: Data View Parameters

    private void InstantiateCheckParameterProperties_DataView()
    {
      AppendixETestRecordsByLocationForQaStatus = new cCheckParameterDataViewLegacy(2914, "Appendix_E_Test_Records_By_Location_For_QA_Status", this);
      CrossCheckProtocolGasParameterToType = new cCheckParameterDataViewLegacy(3262, "CrossCheck_ProtocolGasParameterToType", this);
      CrossCheckTestTypeToRequiredTestCode = new cCheckParameterDataViewLegacy(3272, "CrossCheck_TestTypeToRequiredTestCode", this);
      F2lCheckRecordsForQaStatus = new cCheckParameterDataViewLegacy(3270, "F2L_Check_Records_For_QA_Status", this);
      FuelTypeRealityChecksforGCVCrossCheckTable = new cCheckParameterDataViewLegacy(2667, "Fuel_Type_Reality_Checks_for_GCV_Cross_Check_Table", this);
      FuelTypeWarningLevelsforGCVCrossCheckTable = new cCheckParameterDataViewLegacy(2668, "Fuel_Type_Warning_Levels_for_GCV_Cross_Check_Table", this);
      GasComponentCodeLookupTable = new cCheckParameterDataViewLegacy(3305, "Gas_Component_Code_Lookup_Table", this);
      GasTypeCodeLookupTable = new cCheckParameterDataViewLegacy(3222, "Gas_Type_Code_Lookup_Table", this);
      HourlyNonOperatingDataRecordsforLocation = new cCheckParameterDataViewLegacy(3139, "Hourly_Non_Operating_Data_Records_for_Location", this);
      HourlyOperatingDataRecordsByHourLocation = new cCheckParameterDataViewLegacy(918, "Hourly_Operating_Data_Records_By_Hour_Location", this);
      HourlyOperatingDataRecordsforLocation = new cCheckParameterDataViewLegacy(2741, "Hourly_Operating_Data_Records_for_Location", this);
      HourlyParamFuelFlowRecordsForHourLocation = new cCheckParameterDataViewLegacy(2896, "Hourly_Param_Fuel_Flow_Records_For_Hour_Location", this);
      LeakCheckRecordsByLocationForQAStatus = new cCheckParameterDataViewLegacy(3298, "Leak_Check_Records_By_Location_For_QA_Status", this);
      LocationReportingFrequencyRecords = new cCheckParameterDataViewLegacy(859, "Location_Reporting_Frequency_Records", this);
      MpLocationNonLoadBasedRecords = new cCheckParameterDataViewLegacy(3273, "Mp_Location_Non_Load_Based_Records", this);
      MpQualificationPercentRecords = new cCheckParameterDataViewLegacy(3267, "MP_Qualification_Percent_Records", this);
      MpQualificationRecords = new cCheckParameterDataViewLegacy(2848, "MP_Qualification_Records", this);
      MpSystemComponentRecords = new cCheckParameterDataViewLegacy(3310, "MP_System_Component_Records", this);
      OperatingSuppDataRecordsByLocation = new cCheckParameterDataViewLegacy(2740, "Operating_Supp_Data_Records_by_Location", this);
      OverrideRATABAF = new cCheckParameterDecimalValue(3315, "Override_RATA_BAF", this);
      ProtocolGasVendorLookupTable = new cCheckParameterDataViewLegacy(3224, "Protocol_Gas_Vendor_Lookup_Table", this);
      QaCertificationEventRecords = new cCheckParameterDataViewLegacy(1877, "Qa_Certification_Event_Records", this);
      RataTestRecordsByLocationForQaStatus = new cCheckParameterDataViewLegacy(2927, "RATA_Test_Records_By_Location_For_QA_Status", this);
      SystemParameterLookupTable = new cCheckParameterDataViewLegacy(3220, "System_Parameter_Lookup_Table", this);
      TestExtensionExemptionRecords = new cCheckParameterDataViewLegacy(1878, "Test_Extension_Exemption_Records", this);
      UnitDefaultTestRecordsByLocationForQaStatus = new cCheckParameterDataViewLegacy(3250, "Unit_Default_Test_Records_By_Location_For_QA_Status", this);
    }


    /// <summary>
    /// Implements check parameter Appendix_E_Test_Records_By_Location_For_QA_Status (id = 2914)
    /// </summary>
    public cCheckParameterDataViewLegacy AppendixETestRecordsByLocationForQaStatus { get; private set; }

    /// <summary>
    /// Implements check parameter CrossCheck_ProtocolGasParameterToType (id = 3262)
    /// </summary>
    public cCheckParameterDataViewLegacy CrossCheckProtocolGasParameterToType { get; private set; }

    /// <summary>
    /// Implements check parameter CrossCheck_TestTypeToRequiredTestCode (id = 3272)
    /// </summary>
    public cCheckParameterDataViewLegacy CrossCheckTestTypeToRequiredTestCode { get; private set; }

    /// <summary>
    /// Implements check parameter F2L_Check_Records_For_QA_Status (id = 3270)
    /// </summary>
    public cCheckParameterDataViewLegacy F2lCheckRecordsForQaStatus { get; private set; }

    /// <summary>
    /// Implements check parameter Fuel_Type_Reality_Checks_for_GCV_Cross_Check_Table (id = 2667)
    /// </summary>
    public cCheckParameterDataViewLegacy FuelTypeRealityChecksforGCVCrossCheckTable { get; private set; }

    /// <summary>
    /// Implements check parameter Fuel_Type_Warning_Levels_for_GCV_Cross_Check_Table (id = 2668)
    /// </summary>
    public cCheckParameterDataViewLegacy FuelTypeWarningLevelsforGCVCrossCheckTable { get; private set; }

    /// <summary>
    /// Implements check parameter Gas_Component_Code_Lookup_Table (id = 3305)
    /// </summary>
    public cCheckParameterDataViewLegacy GasComponentCodeLookupTable { get; private set; }

    /// <summary>
    /// Implements check parameter Gas_Type_Code_Lookup_Table (id = 3222)
    /// </summary>
    public cCheckParameterDataViewLegacy GasTypeCodeLookupTable { get; private set; }

    /// <summary>
    /// Implements check parameter Hourly_Non_Operating_Data_Records_for_Location (id = 3139)
    /// </summary>
    public cCheckParameterDataViewLegacy HourlyNonOperatingDataRecordsforLocation { get; private set; }

    /// <summary>
    /// Implements check parameter Hourly_Operating_Data_Records_By_Hour_Location (id = 918)
    /// </summary>
    public cCheckParameterDataViewLegacy HourlyOperatingDataRecordsByHourLocation { get; private set; }

    /// <summary>
    /// Implements check parameter Hourly_Operating_Data_Records_for_Location (id = 2741) 
    /// </summary>
    public cCheckParameterDataViewLegacy HourlyOperatingDataRecordsforLocation { get; private set; }

    /// <summary>
    /// Implements check parameter Hourly_Param_Fuel_Flow_Records_For_Hour_Location (id = 2896)
    /// </summary>
    public cCheckParameterDataViewLegacy HourlyParamFuelFlowRecordsForHourLocation { get; private set; }

    /// <summary>
    /// Implements check parameter Leak_Check_Records_By_Location_For_QA_Status (id = 3298) 
    /// </summary>
    public cCheckParameterDataViewLegacy LeakCheckRecordsByLocationForQAStatus { get; private set; }

    /// <summary>
    /// Implements check parameter Location_Reporting_Frequency_Records (id = 859) 
    /// </summary>
    public cCheckParameterDataViewLegacy LocationReportingFrequencyRecords { get; private set; }

    /// <summary>
    /// Implements check parameter Mp_Location_Non_Load_Based_Records (id = 3273)
    /// </summary>
    public cCheckParameterDataViewLegacy MpLocationNonLoadBasedRecords { get; private set; }

    /// <summary>
    /// Implements check parameter MP_Qualification_Percent_Records (id = 3267)
    /// </summary>
    public cCheckParameterDataViewLegacy MpQualificationPercentRecords { get; private set; }

    /// <summary>
    /// Implements check parameter MP_Qualification_Records (id = 2848)
    /// </summary>
    public cCheckParameterDataViewLegacy MpQualificationRecords { get; private set; }

    /// <summary>
    /// Implements check parameter MP_System_Component_Records (id = 3310) 
    /// </summary>
    public cCheckParameterDataViewLegacy MpSystemComponentRecords { get; private set; }

    /// <summary>
    /// Implements check parameter Operating_Supp_Data_Records_by_Location (id = 2740)
    /// </summary>
    public cCheckParameterDataViewLegacy OperatingSuppDataRecordsByLocation { get; private set; }

    /// <summary>
    /// Implements check parameter Override_RATA_BAF (id = 3315)
    /// </summary>
    public cCheckParameterDecimalValue OverrideRATABAF { get; private set; }

    /// <summary>
    /// Implements check parameter Protocol_Gas_Vendor_Lookup_Table (id = 3224)
    /// </summary>
    public cCheckParameterDataViewLegacy ProtocolGasVendorLookupTable { get; private set; }

    /// <summary>
    /// Implements check parameter Qa_Certification_Event_Records (id = 1877)
    /// </summary>
    public cCheckParameterDataViewLegacy QaCertificationEventRecords { get; private set; }

    /// <summary>
    /// Implements check parameter RATA_Test_Records_By_Location_For_QA_Status (id = 2927)
    /// </summary>
    public cCheckParameterDataViewLegacy RataTestRecordsByLocationForQaStatus { get; private set; }

    /// <summary>
    /// Implements check parameter System_Parameter_Lookup_Table (id = 3220)
    /// </summary>
    public cCheckParameterDataViewLegacy SystemParameterLookupTable { get; private set; }

    /// <summary>
    /// Implements check parameter Test_Extension_Exemption_Records (id = 1878)
    /// </summary>
    public cCheckParameterDataViewLegacy TestExtensionExemptionRecords { get; private set; }

    /// <summary>
    /// Implements check parameter Unit_Default_Test_Records (id = 3250)
    /// </summary>
    public cCheckParameterDataViewLegacy UnitDefaultTestRecordsByLocationForQaStatus { get; private set; }

    #endregion


    #region Public Properties: DateTime Parameters

    private void InstantiateCheckParameterProperties_Date()
    {
      CurrentOperatingDate = new cCheckParameterDateValue(1011, "Current_Operating_Date", this);
      CurrentReportingPeriodBeginHour = new cCheckParameterDateValue(3265, "Current_Reporting_Period_Begin_Hour", this);
      CurrentReportingPeriodEndHour = new cCheckParameterDateValue(3266, "Current_Reporting_Period_End_Hour", this);
      DailyCalPgvpRuleDate = new cCheckParameterDateValue(3221, "Daily_Cal_PGVP_Rule_Date", this);
      EarliestLocationReportDate = new cCheckParameterDateValue(3111, "Earliest_Location_Report_Date", this);
      FirstDayofOperation = new cCheckParameterDateValue(3135, "First_Day_of_Operation", this);
      PriorLeakExpirationDate = new cCheckParameterDateValue(3301, "Prior_Leak_Expiration_Date", this);
      UdefExpirationDate = new cCheckParameterDateValue(3241, "UDEF_Expiration_Date", this);
    }


    /// <summary>
    /// Implements check parameter Current_Operating_Date (id = 1011)
    /// </summary>
    public cCheckParameterDateValue CurrentOperatingDate { get; private set; }

    /// <summary>
    /// Implements check parameter Current_Reporting_Period_Begin_Hour (id = 3265)
    /// </summary>
    public cCheckParameterDateValue CurrentReportingPeriodBeginHour { get; private set; }

    /// <summary>
    /// Implements check parameter Current_Reporting_Period_End_Hour (id = 3266)
    /// </summary>
    public cCheckParameterDateValue CurrentReportingPeriodEndHour { get; private set; }

    /// <summary>
    /// Implements check parameter Daily_Cal_PGVP_Rule_Date (id = 3221)
    /// </summary>
    public cCheckParameterDateValue DailyCalPgvpRuleDate { get; private set; }

    /// <summary>
    /// Implements check parameter Earliest_Location_Report_Date (id = 3111)
    /// </summary>
    public cCheckParameterDateValue EarliestLocationReportDate { get; private set; }

    /// <summary>
    /// Implements check parameter First_Day_of_Operation (id = 3135)
    /// </summary>
    public cCheckParameterDateValue FirstDayofOperation { get; private set; }

    /// <summary>
    /// Implements check parameter Prior_Leak_Expiration_Date (id = 3301) 
    /// </summary>
    public cCheckParameterDateValue PriorLeakExpirationDate { get; private set; }

    /// <summary>
    /// Implements check parameter UDEF_Expiration_Date (id = 3241)
    /// </summary>
    public cCheckParameterDateValue UdefExpirationDate { get; private set; }

    #endregion


    #region Public Properties: Decimal Parameters

    private void InstantiateCheckParameterProperties_Decimal()
    {
      CurrentOperatingTime = new cCheckParameterDecimalValue(2356, "Current_Operating_Time", this);
    }


    /// <summary>
    /// Implements check parameter Current_Operating_Time (id = 2356)
    /// </summary>
    public cCheckParameterDecimalValue CurrentOperatingTime { get; private set; }

    #endregion


    #region Public Properties: Integer Parameters

    private void InstantiateCheckParameterProperties_Integer()
    {
      CurrentMonitorPlanLocationPostion = new cCheckParameterIntegerValue(2238, "Current_Monitor_Plan_Location_Postion", this);
      CurrentOperatingHour = new cCheckParameterIntegerValue(1012, "Current_Operating_Hour", this);
      CurrentReportingPeriod = new cCheckParameterIntegerValue(2228, "Current_Reporting_Period", this);
      CurrentReportingPeriodQuarter = new cCheckParameterIntegerValue(2899, "Current_Reporting_Period_Quarter", this);
      CurrentReportingPeriodYear = new cCheckParameterIntegerValue(2900, "Current_Reporting_Period_Year", this);
      F2lStatusPriorTestRequiredQuarter = new cCheckParameterIntegerValue(3268, "F2L_Status_Prior_Test_Required_Quarter", this);
      FirstEcmpsReportingPeriod = new cCheckParameterIntegerValue(3089, "First_ECMPS_Reporting_Period", this);
      FirstHourofOperation = new cCheckParameterIntegerValue(3136, "First_Hour_of_Operation", this);
      LocationPos = new cCheckParameterIntegerValue(null, "LocationPos", this);
    }

    /// <summary>
    /// Implements check parameter Current_Monitor_Plan_Location_Postion (id = 2238)
    /// </summary>
    public cCheckParameterIntegerValue CurrentMonitorPlanLocationPostion { get; private set; }

    /// <summary>
    /// Implements check parameter Current_Operating_Hour (id = 1012)
    /// </summary>
    public cCheckParameterIntegerValue CurrentOperatingHour { get; private set; }

    /// <summary>
    /// Implements check parameter Current_Reporting_Period (id = 2228)
    /// </summary>
    public cCheckParameterIntegerValue CurrentReportingPeriod { get; private set; }

    /// <summary>
    /// Implements check parameter Current_Reporting_Period_Quarter (id = 2899)
    /// </summary>
    public cCheckParameterIntegerValue CurrentReportingPeriodQuarter { get; private set; }

    /// <summary>
    /// Implements check parameter Current_Reporting_Period_Year (id = 2900)
    /// </summary>
    public cCheckParameterIntegerValue CurrentReportingPeriodYear { get; private set; }

    /// <summary>
    /// Implements check parameter F2L_Status_Prior_Test_Required_Quarter (id = 3268)
    /// </summary>
    public cCheckParameterIntegerValue F2lStatusPriorTestRequiredQuarter { get; private set; }

    /// <summary>
    /// Implements check parameter First_ECMPS_Reporting_Period (id = 3089)
    /// </summary>
    public cCheckParameterIntegerValue FirstEcmpsReportingPeriod { get; private set; }

    /// <summary>
    /// Implements check parameter First_Hour_of_Operation (id = 3136)
    /// </summary>
    public cCheckParameterIntegerValue FirstHourofOperation { get; private set; }

    /// <summary>
    /// Implements check parameter LocationPos
    /// </summary>
    public cCheckParameterIntegerValue LocationPos { get; private set; }

    #endregion


    #region Public Properties: Object Parameters

    private void InstantiateCheckParameterProperties_Object()
    {
      CurrentReportingPeriodObject = new cCheckParameterObjectValue(3113, "Current_Reporting_Period_Object", this);
      F2lStatusSystemCheckDictionary = new cCheckParameterObjectValue(3281, "F2L_Status_System_Check_Dictionary", this);
      F2lStatusSystemMissingOpDictionary = new cCheckParameterObjectValue(3287, "F2L_Status_System_Missing_Op_Dictionary", this);
      F2lStatusSystemResultDictionary = new cCheckParameterObjectValue(3269, "F2L_Status_System_Result_Dictionary", this);
      LastFailedOrAbortedDailyCalObject = new cCheckParameterObjectValue(3249, "Last_Failed_Or_Aborted_Daily_Cal_Object", this);
      LatestDailyInterferenceCheckObject = new cCheckParameterObjectValue(3289, "Latest_Daily_Interference_Check_Object", this);
      NonOperatingHoursByLocation = new cCheckParameterObjectValue(3139, "Non_Operating_Hours_By_Location", this);
      OperatingHoursByLocation = new cCheckParameterObjectValue(2907, "Operating_Hours_By_Location", this);
    }


    /// <summary>
    /// Implements check parameter Current_Reporting_Period_Object (id = 3113)
    /// </summary>
    public cCheckParameterObjectValue CurrentReportingPeriodObject { get; private set; }

    /// <summary>
    /// Implements check parameter F2L_Status_System_Check_Dictionary (id = 3281)
    /// </summary>
    public cCheckParameterObjectValue F2lStatusSystemCheckDictionary { get; private set; }

    /// <summary>
    /// Implements check parameter F2L_Status_System_Missing_Op_Dictionary (id = 3287)
    /// </summary>
    public cCheckParameterObjectValue F2lStatusSystemMissingOpDictionary { get; private set; }

    /// <summary>
    /// Implements check parameter F2L_Status_System_Result_Dictionary (id = 3269)
    /// </summary>
    public cCheckParameterObjectValue F2lStatusSystemResultDictionary { get; private set; }

    /// <summary>
    /// Implements check parameter Last_Failed_Or_Aborted_Daily_Cal_Object (id = 3249)
    /// </summary>
    public cCheckParameterObjectValue LastFailedOrAbortedDailyCalObject { get; private set; }

    /// <summary>
    /// Implements check parameter Latest_Daily_Interference_Check_Object (id = 3289)
    /// </summary>
    public cCheckParameterObjectValue LatestDailyInterferenceCheckObject { get; private set; }

    /// <summary>
    /// Implements check parameter Non_Operating_Hours_By_Location (id = 3139)
    /// </summary>
    public cCheckParameterObjectValue NonOperatingHoursByLocation { get; private set; }

    /// <summary>
    /// Implements check parameter Operating_Hours_By_Location (id = 2907)
    /// </summary>
    public cCheckParameterObjectValue OperatingHoursByLocation { get; private set; }
    
    #endregion


    #region Public Properties: String Parameters

    private void InstantiateCheckParameterProperties_String()
    {
      CurrentMonitorLocationId = new cCheckParameterStringValue(2894, "Current_Monitor_Location_Id", this);
      DailyIntStatusResult = new cCheckParameterStringValue(3292, "Daily_Int_Status_Result", this);
      EmTestCalcResult = new cCheckParameterStringValue(3247, "EM_Test_Calc_Result", this);
      ExpirationText = new cCheckParameterStringValue(3245, "Expiration_Text", this);
      ExpiredSystems = new cCheckParameterStringValue(3242, "Expired_Systems", this);
      ExpiringSystems = new cCheckParameterStringValue(3244, "Expiring_Systems", this);
      F2lStatusMissingOpDataInfo = new cCheckParameterStringValue(3278, "F2L_Status_Missing_Op_Data_Info", this);
      F2lStatusPriorTestRequiredQuarterMissingOpData = new cCheckParameterStringValue(3286, "F2L_Status_Prior_Test_Required_Quarter_Missing_Op_Data", this);
      F2lStatusResult = new cCheckParameterStringValue(3271, "F2L_Status_Result", this);
      LeakMissingOpDataInfo = new cCheckParameterStringValue(3302, "Leak_Missing_Op_Data_Info", this);
      LeakStatusResult = new cCheckParameterStringValue(3300, "Leak_Status_Result", this);
      ProtocolGasBalanceComponentList = new cCheckParameterStringValue(3313, "Protocol_Gas_Balance_Component_List", this);
      ProtocolGasDuplicateComponentList = new cCheckParameterStringValue(3314, "Protocol_Gas_Duplicate_Component_List", this);
      ProtocolGasExclusiveComponentList = new cCheckParameterStringValue(3309, "Protocol_Gas_Exclusive_Component_List", this);
      ProtocolGasInvalidComponentList = new cCheckParameterStringValue(3308, "Protocol_Gas_Invalid_Component_List", this);
      QualificationPercentMissingList = new cCheckParameterStringValue(3264, "Qualification_Percent_Missing_List", this);
      UdefStatus = new cCheckParameterStringValue(3240, "UDEF_Status", this);
    }


    /// <summary>
    /// Implements check parameter Current_Monitor_Location_Id (id = 2894)
    /// </summary>
    public cCheckParameterStringValue CurrentMonitorLocationId { get; private set; }

    /// <summary>
    /// Implements check parameter Daily_Int_Status_Result (id = 3292)
    /// </summary>
    public cCheckParameterStringValue DailyIntStatusResult { get; private set; }

    /// <summary>
    /// Implements check parameter EM_Test_Calc_Result (id = 3247)
    /// </summary>
    public cCheckParameterStringValue EmTestCalcResult { get; private set; }

    /// <summary>
    /// Implements check parameter Expiration_Text (id = 3245)
    /// </summary>
    public cCheckParameterStringValue ExpirationText { get; private set; }

    /// <summary>
    /// Implements check parameter Expired_Systems (id = 3242)
    /// </summary>
    public cCheckParameterStringValue ExpiredSystems { get; private set; }

    /// <summary>
    /// Implements check parameter Expiring_Systems (id = 3244)
    /// </summary>
    public cCheckParameterStringValue ExpiringSystems { get; private set; }

    /// <summary>
    /// Implements check parameter F2L_Status_RATA_Missing_Op_Data_Info (id = 3278)
    /// </summary>
    public cCheckParameterStringValue F2lStatusMissingOpDataInfo { get; private set; }

    /// <summary>
    /// Implements check parameter F2L_Status_Prior_Test_Required_Quarter_Missing_Op_Data (id = 3286)
    /// </summary>
    public cCheckParameterStringValue F2lStatusPriorTestRequiredQuarterMissingOpData { get; private set; }

    /// <summary>
    /// Implements check parameter F2L_Status_Result (id = 3271)
    /// </summary>
    public cCheckParameterStringValue F2lStatusResult { get; private set; }

    /// <summary>
    /// Implements check parameter Leak_Missing_Op_Data_Info (id = 3302) 
    /// </summary>
    public cCheckParameterStringValue LeakMissingOpDataInfo { get; private set; }

    /// <summary>
    /// Implements check parameter Leak_Status_Result (id = 3300) 
    /// </summary>
    public cCheckParameterStringValue LeakStatusResult { get; private set; }

    /// <summary>
    /// Implements check parameter Protocol_Gas_Balance_Component_List (id = 3313)
    /// </summary>
    public cCheckParameterStringValue ProtocolGasBalanceComponentList { get; private set; }

    /// <summary>
    /// Implements check parameter Protocol_Gas_Duplicate_Component_List (id = 3314)
    /// </summary>
    public cCheckParameterStringValue ProtocolGasDuplicateComponentList { get; private set; }

    /// <summary>
    /// Implements check parameter Protocol_Gas_Exclusive_Component_List (id = 3309)
    /// </summary>
    public cCheckParameterStringValue ProtocolGasExclusiveComponentList { get; private set; }

    /// <summary>
    /// Implements check parameter Protocol_Gas_Invalid_Component_List (id = 3308)
    /// </summary>
    public cCheckParameterStringValue ProtocolGasInvalidComponentList { get; private set; }

    /// <summary>
    /// Implements check parameter Qualification_Percent_Missing_List (id = 3264)
    /// </summary>
    public cCheckParameterStringValue QualificationPercentMissingList { get; private set; }

    /// <summary>
    /// Implements check parameter UDEF_Status (id = 3240)
    /// </summary>
    public cCheckParameterStringValue UdefStatus { get; private set; }

    #endregion


    #region Public Properties: String Array Parameters

    private void InstantiateCheckParameterProperties_StringArray()
    {
      FlowSystemIdArray = new cCheckParameterStringArray(3238, "FLOW_System_ID_Array", this);
      LmeFuelArray = new cCheckParameterStringArray(3239, "LME_Fuel_Array", this);
      NoxeSystemIdArray = new cCheckParameterStringArray(3237, "NOXE_System_ID_Array", this);
    }


    /// <summary>
    /// Implements check parameter FLOW_System_ID_Array (id = 3238)
    /// </summary>
    public cCheckParameterStringArray FlowSystemIdArray { get; private set; }

    /// <summary>
    /// Implements check parameter LME_Fuel_Array (id = 3239)
    /// </summary>
    public cCheckParameterStringArray LmeFuelArray { get; private set; }

    /// <summary>
    /// Implements check parameter NOXE_System_ID_Array (id = 3237)
    /// </summary>
    public cCheckParameterStringArray NoxeSystemIdArray { get; private set; }

    #endregion


    #region Protected Abstract Methods

    /// <summary>
    /// This method should instantiate each of the check parameter properties implemented in
    /// the child check parameters objects.
    /// </summary>
    protected override void InstantiateCheckParameterProperties()
    {
      InstantiateCheckParameterProperties_Boolean();
      InstantiateCheckParameterProperties_DataRowView();
      InstantiateCheckParameterProperties_DataView();
      InstantiateCheckParameterProperties_Date();
      InstantiateCheckParameterProperties_Decimal();
      InstantiateCheckParameterProperties_Integer();
      InstantiateCheckParameterProperties_Object();
      InstantiateCheckParameterProperties_String();
      InstantiateCheckParameterProperties_StringArray();
    }

    #endregion

  }
}
