using System;
using System.Collections.Generic;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.CheckEngine.SpecialParameterClasses;
using ECMPS.Checks.Data.Ecmps.Dbo.View;
using ECMPS.Checks.Data.Ecmps.Lookup.Table;
using ECMPS.Checks.Data.EcmpsAux.CrossCheck.Virtual;
using ECMPS.Checks.Em.Parameters;
using ECMPS.Checks.EmissionsReport;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Enumerations;
using ECMPS.Definitions.Extensions;


namespace ECMPS.Checks.EmissionsChecks
{
    public class cHourlyGeneralChecks : cEmissionsChecks
    {

        #region Constructors

        /// <summary>
        /// Creates an instance using the passed Emissions Report Process
        /// to populate properties.
        /// </summary>
        /// <param name="emissionsReportProcess">The Emissions Report Process used to populate properties.</param>
        public cHourlyGeneralChecks(cEmissionsReportProcess emissionReportProcess)
          : base(emissionReportProcess)
        {
            InitCheckProcedures();
        }


        /// <summary>
        /// Creates an instance.
        /// </summary>
        /// <param name="checkEngine">The current check engine.</param>
        /// <param name="emissionParameters">The current emission parameters.</param>
        public cHourlyGeneralChecks(cCheckEngine checkEngine, cEmissionsCheckParameters emissionParameters)
          : base(checkEngine, emissionParameters)
        {
            InitCheckProcedures();
        }


        /// <summary>
        /// Constructor used for testing.
        /// </summary>
        /// <param name="mpManualParameters"></param>
        public cHourlyGeneralChecks(cEmissionsCheckParameters emissionParameters)
        {
            EmManualParameters = emissionParameters;
        }


        #region Helper Methods

        /// <summary>
        /// Initializes the CheckProcedures array.
        /// </summary>
        private void InitCheckProcedures()
        {
            CheckProcedures = new dCheckProcedure[31];

            CheckProcedures[1] = new dCheckProcedure(HOURGEN1);
            CheckProcedures[2] = new dCheckProcedure(HOURGEN2);
            CheckProcedures[3] = new dCheckProcedure(HOURGEN3);
            CheckProcedures[4] = new dCheckProcedure(HOURGEN4);
            CheckProcedures[5] = new dCheckProcedure(HOURGEN5);
            CheckProcedures[6] = new dCheckProcedure(HOURGEN6);
            CheckProcedures[7] = new dCheckProcedure(HOURGEN7);
            CheckProcedures[8] = new dCheckProcedure(HOURGEN8);
            CheckProcedures[9] = new dCheckProcedure(HOURGEN9);
            CheckProcedures[10] = new dCheckProcedure(HOURGEN10);

            CheckProcedures[11] = new dCheckProcedure(HOURGEN11);
            CheckProcedures[12] = new dCheckProcedure(HOURGEN12);
            CheckProcedures[13] = new dCheckProcedure(HOURGEN13);
            CheckProcedures[14] = new dCheckProcedure(HOURGEN14);
            CheckProcedures[15] = new dCheckProcedure(HOURGEN15);
            CheckProcedures[16] = new dCheckProcedure(HOURGEN16);
            CheckProcedures[17] = new dCheckProcedure(HOURGEN17);
            CheckProcedures[18] = new dCheckProcedure(HOURGEN18);
            CheckProcedures[19] = new dCheckProcedure(HOURGEN19);
            CheckProcedures[20] = new dCheckProcedure(HOURGEN20);

            CheckProcedures[21] = new dCheckProcedure(HOURGEN21);
            CheckProcedures[22] = new dCheckProcedure(HOURGEN22);
            CheckProcedures[22] = new dCheckProcedure(HOURGEN22);
            CheckProcedures[23] = new dCheckProcedure(HOURGEN23);
            CheckProcedures[24] = new dCheckProcedure(HOURGEN24);
            CheckProcedures[25] = new dCheckProcedure(HOURGEN25);
            CheckProcedures[26] = new dCheckProcedure(HOURGEN26);
            CheckProcedures[27] = new dCheckProcedure(HOURGEN27);
            CheckProcedures[28] = new dCheckProcedure(HOURGEN28);
            CheckProcedures[29] = new dCheckProcedure(HOURGEN29);
            CheckProcedures[30] = new dCheckProcedure(HOURGEN30);
        }

        #endregion

        #endregion


        #region Public Static Methods: Checks

        public string HOURGEN1(cCategory Category, ref bool Log)
        // Initialize Accumulators for Summary Value Data 
        // (Formerly HourAgg-1)
        {
            string ReturnVal = "";

            try
            {
                int MonitorLocationCount = cDBConvert.ToInteger(Category.GetCheckParameter("Current_Location_Count").ParameterValue);

                decimal[] Co2MassCalculatedAccumulatorArray = new decimal[MonitorLocationCount];
                decimal[] Co2MassReportedAccumulatorArray = new decimal[MonitorLocationCount];
                bool[] ExpectedSummaryValueCo2Array = new bool[MonitorLocationCount];

                decimal[] HiCalculatedAccumulatorArray = new decimal[MonitorLocationCount];
                decimal[] HiReportedAccumulatorArray = new decimal[MonitorLocationCount];
                bool[] ExpectedSummaryValueHiArray = new bool[MonitorLocationCount];

                decimal[] NoxRateCalculatedAccumulatorArray = new decimal[MonitorLocationCount];
                decimal[] NoxRateReportedAccumulatorArray = new decimal[MonitorLocationCount];
                int[] NoxRateHoursAccumulatorArray = new int[MonitorLocationCount];
                bool[] ExpectedSummaryValueNoxRateArray = new bool[MonitorLocationCount];

                decimal[] So2MassCalculatedAccumulatorArray = new decimal[MonitorLocationCount];
                decimal[] So2MassReportedAccumulatorArray = new decimal[MonitorLocationCount];
                bool[] ExpectedSummaryValueSo2Array = new bool[MonitorLocationCount];

                decimal[] NoxMassCalculatedAccumulatorArray = new decimal[MonitorLocationCount];
                decimal[] NoxMassReportedAccumulatorArray = new decimal[MonitorLocationCount];
                bool[] ExpectedSummaryValueNoxMassArray = new bool[MonitorLocationCount];

                decimal[] OpTimeAccumulatorArray = new decimal[MonitorLocationCount];
                int[] OpHoursAccumulatorArray = new int[MonitorLocationCount];
                int[] OpDaysAccumulatorArray = new int[MonitorLocationCount];
                decimal[] LoadAccumulatorArray = new decimal[MonitorLocationCount];

                decimal[] DailyOpTimeAccumulatorArray = new decimal[MonitorLocationCount];

                decimal[] AprilOpTimeAccumulatorArray = new decimal[MonitorLocationCount];
                int[] AprilOpHoursAccumulatorArray = new int[MonitorLocationCount];
                int[] AprilOpDaysAccumulatorArray = new int[MonitorLocationCount];
                decimal[] AprilHiCalculatedAccumulatorArray = new decimal[MonitorLocationCount];
                decimal[] AprilNoxMassCalculatedAccumulatorArray = new decimal[MonitorLocationCount];

                decimal[] LMETotalLoadArray = new decimal[MonitorLocationCount];
                decimal[] LMEAprilLoadArray = new decimal[MonitorLocationCount];
                decimal[] LMETotalHiArray = new decimal[MonitorLocationCount];
                decimal[] LMEAprilHiArray = new decimal[MonitorLocationCount];
                decimal[] LMETotalOpTimeArray = new decimal[MonitorLocationCount];
                decimal[] LMEAprilOpTimeArray = new decimal[MonitorLocationCount];

                string[] LastDayOpArray = new string[MonitorLocationCount];

                EmParameters.OperatingDateArray = new List<DateTime>[MonitorLocationCount];

                for (int MonitorLocationDex = 0; MonitorLocationDex < MonitorLocationCount; MonitorLocationDex++)
                {
                    Co2MassReportedAccumulatorArray[MonitorLocationDex] = 0;
                    Co2MassCalculatedAccumulatorArray[MonitorLocationDex] = 0;
                    ExpectedSummaryValueCo2Array[MonitorLocationDex] = false;

                    HiReportedAccumulatorArray[MonitorLocationDex] = 0;
                    HiCalculatedAccumulatorArray[MonitorLocationDex] = 0;
                    ExpectedSummaryValueHiArray[MonitorLocationDex] = false;

                    NoxRateReportedAccumulatorArray[MonitorLocationDex] = 0;
                    NoxRateCalculatedAccumulatorArray[MonitorLocationDex] = 0;
                    NoxRateHoursAccumulatorArray[MonitorLocationDex] = 0;
                    ExpectedSummaryValueNoxRateArray[MonitorLocationDex] = false;

                    So2MassReportedAccumulatorArray[MonitorLocationDex] = 0;
                    So2MassCalculatedAccumulatorArray[MonitorLocationDex] = 0;
                    ExpectedSummaryValueSo2Array[MonitorLocationDex] = false;

                    NoxMassReportedAccumulatorArray[MonitorLocationDex] = 0;
                    NoxMassCalculatedAccumulatorArray[MonitorLocationDex] = 0;
                    ExpectedSummaryValueNoxMassArray[MonitorLocationDex] = false;

                    OpTimeAccumulatorArray[MonitorLocationDex] = 0;
                    OpHoursAccumulatorArray[MonitorLocationDex] = 0;
                    OpDaysAccumulatorArray[MonitorLocationDex] = 0;
                    LoadAccumulatorArray[MonitorLocationDex] = 0;

                    DailyOpTimeAccumulatorArray[MonitorLocationDex] = 0;

                    AprilOpTimeAccumulatorArray[MonitorLocationDex] = 0;
                    AprilOpHoursAccumulatorArray[MonitorLocationDex] = 0;
                    AprilOpDaysAccumulatorArray[MonitorLocationDex] = 0;
                    AprilHiCalculatedAccumulatorArray[MonitorLocationDex] = 0;
                    AprilNoxMassCalculatedAccumulatorArray[MonitorLocationDex] = 0;

                    LMETotalLoadArray[MonitorLocationDex] = 0;
                    LMEAprilLoadArray[MonitorLocationDex] = 0;
                    LMETotalHiArray[MonitorLocationDex] = 0;
                    LMEAprilHiArray[MonitorLocationDex] = 0;
                    LMETotalOpTimeArray[MonitorLocationDex] = 0;
                    LMEAprilOpTimeArray[MonitorLocationDex] = 0;

                    LastDayOpArray[MonitorLocationDex] = null;
                    EmParameters.OperatingDateArray[MonitorLocationDex] = new List<DateTime>();
                }

                Category.SetCheckParameter("Rpt_Period_Co2_Mass_Reported_Accumulator_Array", Co2MassReportedAccumulatorArray, eParameterDataType.Decimal, true, true);
                Category.SetCheckParameter("Rpt_Period_Co2_Mass_Calculated_Accumulator_Array", Co2MassCalculatedAccumulatorArray, eParameterDataType.Decimal, true, true);
                Category.SetCheckParameter("Expected_Summary_Value_Co2_Array", ExpectedSummaryValueCo2Array, eParameterDataType.Boolean, true, true);

                Category.SetCheckParameter("Rpt_Period_Hi_Reported_Accumulator_Array", HiReportedAccumulatorArray, eParameterDataType.Decimal, true, true);
                Category.SetCheckParameter("Rpt_Period_Hi_Calculated_Accumulator_Array", HiCalculatedAccumulatorArray, eParameterDataType.Decimal, true, true);
                Category.SetCheckParameter("Expected_Summary_Value_Hi_Array", ExpectedSummaryValueHiArray, eParameterDataType.Boolean, true, true);

                Category.SetCheckParameter("Rpt_Period_Nox_Rate_Reported_Accumulator_Array", NoxRateReportedAccumulatorArray, eParameterDataType.Decimal, true, true);
                Category.SetCheckParameter("Rpt_Period_Nox_Rate_Calculated_Accumulator_Array", NoxRateCalculatedAccumulatorArray, eParameterDataType.Decimal, true, true);
                Category.SetCheckParameter("Rpt_Period_Nox_Rate_Hours_Accumulator_Array", NoxRateHoursAccumulatorArray, eParameterDataType.Integer, true, true);
                Category.SetCheckParameter("Expected_Summary_Value_Nox_Rate_Array", ExpectedSummaryValueNoxRateArray, eParameterDataType.Boolean, true, true);

                Category.SetCheckParameter("Rpt_Period_So2_Mass_Reported_Accumulator_Array", So2MassReportedAccumulatorArray, eParameterDataType.Decimal, true, true);
                Category.SetCheckParameter("Rpt_Period_So2_Mass_Calculated_Accumulator_Array", So2MassCalculatedAccumulatorArray, eParameterDataType.Decimal, true, true);
                Category.SetCheckParameter("Expected_Summary_Value_So2_Array", ExpectedSummaryValueSo2Array, eParameterDataType.Boolean, true, true);

                Category.SetCheckParameter("Rpt_Period_Nox_Mass_Reported_Accumulator_Array", NoxMassReportedAccumulatorArray, eParameterDataType.Decimal, true, true);
                Category.SetCheckParameter("Rpt_Period_Nox_Mass_Calculated_Accumulator_Array", NoxMassCalculatedAccumulatorArray, eParameterDataType.Decimal, true, true);
                Category.SetCheckParameter("Expected_Summary_Value_Nox_Mass_Array", ExpectedSummaryValueNoxMassArray, eParameterDataType.Boolean, true, true);

                Category.SetCheckParameter("Rpt_Period_Op_Time_Accumulator_Array", OpTimeAccumulatorArray, eParameterDataType.Decimal, true, true);
                Category.SetCheckParameter("Rpt_Period_Op_Hours_Accumulator_Array", OpHoursAccumulatorArray, eParameterDataType.Integer, true, true);
                Category.SetCheckParameter("Rpt_Period_Op_Days_Accumulator_Array", OpDaysAccumulatorArray, eParameterDataType.Integer, true, true);
                Category.SetCheckParameter("Rpt_Period_Load_Accumulator_Array", LoadAccumulatorArray, eParameterDataType.Decimal, true, true);

                Category.SetCheckParameter("Daily_Op_Time_Accumulator_Array", DailyOpTimeAccumulatorArray, eParameterDataType.Decimal, true, true);

                Category.SetCheckParameter("April_Op_Time_Accumulator_Array", AprilOpTimeAccumulatorArray, eParameterDataType.Decimal, true, true);
                Category.SetCheckParameter("April_Op_Hours_Accumulator_Array", AprilOpHoursAccumulatorArray, eParameterDataType.Integer, true, true);
                Category.SetCheckParameter("April_Op_Days_Accumulator_Array", AprilOpDaysAccumulatorArray, eParameterDataType.Integer, true, true);
                Category.SetCheckParameter("April_HI_Calculated_Accumulator_Array", AprilHiCalculatedAccumulatorArray, eParameterDataType.Decimal, true, true);
                Category.SetCheckParameter("April_NOX_Mass_Calculated_Accumulator_Array", AprilNoxMassCalculatedAccumulatorArray, eParameterDataType.Decimal, true, true);

                Category.SetCheckParameter("LME_Total_Load_Array", LMETotalLoadArray, eParameterDataType.Decimal, true, true);
                Category.SetCheckParameter("LME_April_Load_Array", LMEAprilLoadArray, eParameterDataType.Decimal, true, true);
                Category.SetCheckParameter("LME_Total_Heat_Input_Array", LMETotalHiArray, eParameterDataType.Decimal, true, true);
                Category.SetCheckParameter("LME_April_Heat_Input_Array", LMEAprilHiArray, eParameterDataType.Decimal, true, true);
                Category.SetCheckParameter("LME_Total_OpTime_Array", LMETotalOpTimeArray, eParameterDataType.Decimal, true, true);
                Category.SetCheckParameter("LME_April_OpTime_Array", LMEAprilOpTimeArray, eParameterDataType.Decimal, true, true);

                Category.SetCheckParameter("Last_Day_of_Operation_Array", LastDayOpArray, eParameterDataType.String, false, true);
                Category.SetCheckParameter("First_Day_of_Operation", null, eParameterDataType.Date);
                Category.SetCheckParameter("First_Hour_of_Operation", null, eParameterDataType.Integer);

                FlowSystemIdArray.SetValue(MonitorLocationCount, Category);
                NoxeSystemIdArray.SetValue(MonitorLocationCount, Category);
                LmeFuelArray.SetValue(MonitorLocationCount, Category);

                //Create a dictionary of unique "MON_LOC_ID,FUEL_CD" string indexes and corresponding integer values (initialized to 0)
                Dictionary<string, int>[] FuelOpHoursAccumulatorArray = new Dictionary<string, int>[MonitorLocationCount];

                for (int Dex = 0; Dex < FuelOpHoursAccumulatorArray.Length; Dex++)
                {
                    FuelOpHoursAccumulatorArray[Dex] = new Dictionary<string, int>();
                }

                Category.SetCheckParameter("Fuel_Op_Hours_Accumulator_Array", FuelOpHoursAccumulatorArray, eParameterDataType.Object);

                // Create dictionaries for Flow-to-Load Status checks
                {
                    F2lStatusSystemResultDictionary.SetValue(new Dictionary<string, string>(), Category);
                    F2lStatusSystemCheckDictionary.SetValue(new Dictionary<string, DataRowView>(), Category);
                    F2lStatusSystemMissingOpDictionary.SetValue(new Dictionary<string, string>(), Category);
                }

                // Create List
                {
                    EmParameters.InvalidCylinderIdList = new List<string>();
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURGEN1");
            }

            return ReturnVal;
        }

        public string HOURGEN2(cCategory Category, ref bool Log)
        // Reporting Period Details
        {
            string ReturnVal = "";

            try
            {
                sFilterPair[] RowFilter, RowFilter2;

                bool AbortHourlyChecks = false;
                bool LegacyDataEvaluation = false;
                string LmeHiMethod = null;
                string LmeHiSubstituteDataCode = null;
                bool AnnualReportingRequirement = false;
                bool OsReportingRequirement = false;

                bool LmeAnnual = false;
                bool LmeOs = false;

                Category.SetCheckParameter("Reported_Emissions_Value", null, eParameterDataType.String);
                Category.SetCheckParameter("Multiple_Stack_Configuration", false, eParameterDataType.Boolean);
                Category.SetCheckParameter("Ignored_Daily_Calibration_Tests", false, eParameterDataType.Boolean);
                IgnoredDailyInterferenceTests.SetValue(false, Category);

                DataRowView CurrentMonitoringPlanRecord = Category.GetCheckParameter("Current_Monitoring_Plan_Record").ValueAsDataRowView();
                int CurrentReportingPeriod = Category.GetCheckParameter("Current_Reporting_Period").ValueAsInt();

                int MonitorPlanBeganPeriod = cDBConvert.ToInteger(CurrentMonitoringPlanRecord["Begin_Rpt_Period_Id"]);
                int MonitorPlanEndedPeriod = cDBConvert.ToInteger(CurrentMonitoringPlanRecord["End_Rpt_Period_Id"]);

                int CurrentReportingPeriodYear = Category.GetCheckParameter("Current_Reporting_Period_Year").ValueAsInt();
                int CurrentReportingPeriodQuarter = Category.GetCheckParameter("Current_Reporting_Period_Quarter").ValueAsInt();
                string CurrentReportingQuarter = cDBConvert.ToString(CurrentReportingPeriodYear) + " QTR " + cDBConvert.ToString(CurrentReportingPeriodQuarter);

                DateTime LastDayCurrentRptPeriod = cDateFunctions.LastDateThisQuarter(CurrentReportingPeriodYear, CurrentReportingPeriodQuarter);
                DateTime FirstDayCurrentRptPeriod = cDateFunctions.StartDateThisQuarter(CurrentReportingPeriod);

                DataView MpProgramRecords = Category.GetCheckParameter("MP_Program_Records").ValueAsDataView();
                RowFilter = new sFilterPair[1];

                if ((CurrentReportingPeriod < MonitorPlanBeganPeriod) ||
                    ((MonitorPlanEndedPeriod != int.MinValue) &&
                     (MonitorPlanEndedPeriod < CurrentReportingPeriod)))
                {
                    AbortHourlyChecks = true;
                    Category.CheckCatalogResult = "A";
                }
                else
                {
                    RowFilter[0].Set("PRG_CD", EmParameters.ProgramIsOzoneSeasonList, eFilterPairStringCompare.InList);
                    //RowFilter[1].Set("UNIT_MONITOR_CERT_BEGIN_DATE", LastDayCurrentRptPeriod, eFilterDataType.DateBegan, eFilterPairRelativeCompare.LessThanOrEqual);
                    //RowFilter[2].Set("END_DATE", null, eFilterDataType.DateEnded);
                    //RowFilter[2].Set("END_DATE", FirstDayCurrentRptPeriod, eFilterDataType.DateEnded, eFilterPairRelativeCompare.GreaterThanOrEqual);

                    DateTime CurrentReportingYearBegan = new DateTime(CurrentReportingPeriodYear, 1, 1);
                    DateTime CurrentReportingYearEnded = new DateTime(CurrentReportingPeriodYear, 12, 31);

                    if (CountActiveRows(MpProgramRecords,
                                        CurrentReportingYearBegan, CurrentReportingYearEnded,
                                        "UNIT_MONITOR_CERT_BEGIN_DATE", "END_DATE",
                                        RowFilter) > 0)
                        OsReportingRequirement = true;

                    DataView MpReportingFrequencyRecords = Category.GetCheckParameter("MP_Reporting_Frequency_Records").ValueAsDataView();
                    string sOldFilter = MpReportingFrequencyRecords.RowFilter;

                    MpReportingFrequencyRecords.RowFilter = AddToDataViewFilter(MpReportingFrequencyRecords.RowFilter, "begin_quarter <= '" + CurrentReportingQuarter +
                        "' and (end_rpt_period_id is null or end_quarter >= '" + CurrentReportingQuarter + "')");
                    int count = MpReportingFrequencyRecords.Count;

                    sFilterPair[] FilterRepFreq = new sFilterPair[1];
                    FilterRepFreq[0].Set("REPORT_FREQ_CD", "Q");
                    if (count > 0 && CountRows(MpReportingFrequencyRecords, FilterRepFreq) == count)
                        AnnualReportingRequirement = true;
                    else
                    {
                        FilterRepFreq[0].Set("REPORT_FREQ_CD", "OS");
                        if (count > 0 && CountRows(MpReportingFrequencyRecords, FilterRepFreq) == count)
                        {
                            if (!OsReportingRequirement)
                            {
                                Category.CheckCatalogResult = "B";
                                AbortHourlyChecks = true;
                            }
                            else
                              if (CurrentReportingPeriodQuarter == 1 || CurrentReportingPeriodQuarter == 4)
                            {
                                Category.CheckCatalogResult = "C";
                                AbortHourlyChecks = true;
                            }
                        }
                        else
                        {
                            Category.CheckCatalogResult = "B";
                            AbortHourlyChecks = true;
                        }
                    }

                    MpReportingFrequencyRecords.RowFilter = sOldFilter;
                }

                if (!AbortHourlyChecks)
                {
                    if (Category.CheckEngine.FirstEcmpsReportingPeriodId == null)
                    {
                        if (CurrentReportingPeriodYear <= 2008)
                            LegacyDataEvaluation = true;
                    }
                    else
                    {
                        if (CurrentReportingPeriod < Category.CheckEngine.FirstEcmpsReportingPeriodId)
                            LegacyDataEvaluation = true;
                    }

                    DataView HrlyOpRecs = Category.GetCheckParameter("Hourly_Operating_Data_Records_for_Configuration").ValueAsDataView();
                    string HrlyOpFilter = HrlyOpRecs.RowFilter;
                    HrlyOpRecs.RowFilter = AddToDataViewFilter(HrlyOpFilter, "op_time > 0 and rpt_period_id = " + CurrentReportingPeriod);
                    if (HrlyOpRecs.Count > 0)
                        Category.SetCheckParameter("Reporting_Period_Operating", true, eParameterDataType.Boolean);
                    else
                        Category.SetCheckParameter("Reporting_Period_Operating", false, eParameterDataType.Boolean);

                    HrlyOpRecs.RowFilter = HrlyOpFilter;


                    // Perform LME Checks and Setup Associated Check Parameters
                    {
                        DataView MpMethodRecords = Category.GetCheckParameter("MP_Method_Records").ValueAsDataView();
                        DataView MpQualificationRecords = Category.GetCheckParameter("MP_Qualification_Records").ValueAsDataView();

                        LmeAnnual = false;
                        LmeOs = false;

                        bool anyMonitoringMethodFound = false;
                        bool osMonitoringMethodFound = false;

                        /* Find LME methods that span the current quarter and if they exist set LME Annual if an annual LME qualification exists. */
                        RowFilter = new sFilterPair[4];
                        RowFilter[0].Set("PARAMETER_CD", "SO2M,NOXM,CO2M", eFilterPairStringCompare.InList);
                        RowFilter[1].Set("METHOD_CD", "LME");
                        RowFilter[2].Set("BEGIN_DATE", FirstDayCurrentRptPeriod, eFilterDataType.DateBegan, eFilterPairRelativeCompare.LessThanOrEqual);
                        RowFilter[3].Set("END_DATE", LastDayCurrentRptPeriod, eFilterDataType.DateEnded, eFilterPairRelativeCompare.GreaterThanOrEqual);

                        if (CountRows(MpMethodRecords, RowFilter) > 0)
                        {
                            anyMonitoringMethodFound = true;

                            if ((CurrentReportingPeriodQuarter == 2) || (CurrentReportingPeriodQuarter == 3))
                            {
                                osMonitoringMethodFound = true;
                            }

                            RowFilter = new sFilterPair[3];
                            RowFilter[0].Set("QUAL_TYPE_CD", "LMEA");
                            RowFilter[1].Set("BEGIN_DATE", LastDayCurrentRptPeriod, eFilterDataType.DateBegan, eFilterPairRelativeCompare.LessThanOrEqual);
                            RowFilter[2].Set("END_DATE", new DateTime(FirstDayCurrentRptPeriod.Year, 1, 1), eFilterDataType.DateEnded, eFilterPairRelativeCompare.GreaterThanOrEqual);

                            if (CountRows(MpQualificationRecords, RowFilter) > 0)
                            {
                                LmeAnnual = true;
                            }
                        }

                        /* Find OS LME methods if check Q2 and methods not found that cover whole quarter.  Check for covering just May and June instead. */
                        if (!osMonitoringMethodFound && (CurrentReportingPeriodQuarter == 2))
                        {
                            RowFilter = new sFilterPair[4];
                            RowFilter[0].Set("PARAMETER_CD", "SO2M,NOXM,CO2M", eFilterPairStringCompare.InList);
                            RowFilter[1].Set("METHOD_CD", "LME");
                            RowFilter[2].Set("BEGIN_DATE", new DateTime(FirstDayCurrentRptPeriod.Year, 5, 1), eFilterDataType.DateBegan, eFilterPairRelativeCompare.LessThanOrEqual);
                            RowFilter[3].Set("END_DATE", LastDayCurrentRptPeriod, eFilterDataType.DateEnded, eFilterPairRelativeCompare.GreaterThanOrEqual);

                            if (CountRows(MpMethodRecords, RowFilter) > 0)
                            {
                                anyMonitoringMethodFound = true;
                                osMonitoringMethodFound = true;
                            }
                        }

                        /* If OS methods found, set LME OS to true if an LME OS qualification exists. */
                        if (osMonitoringMethodFound)
                        {
                            RowFilter = new sFilterPair[3];
                            RowFilter[0].Set("QUAL_TYPE_CD", "LMES");
                            RowFilter[1].Set("BEGIN_DATE", LastDayCurrentRptPeriod, eFilterDataType.DateBegan, eFilterPairRelativeCompare.LessThanOrEqual);
                            RowFilter[2].Set("END_DATE", new DateTime(FirstDayCurrentRptPeriod.Year, 1, 1), eFilterDataType.DateEnded, eFilterPairRelativeCompare.GreaterThanOrEqual);

                            if (CountRows(MpQualificationRecords, RowFilter) > 0)
                            {
                                LmeOs = true;
                            }
                        }

                        /* If either annual or OS methods, perofrm LME checking and continue setting check parameters */
                        if (anyMonitoringMethodFound)
                        {
                            if (LmeAnnual && !AnnualReportingRequirement)
                            {
                                AbortHourlyChecks = true;
                                Category.CheckCatalogResult = "D";
                            }
                            else if (LmeOs && !OsReportingRequirement)
                            {
                                AbortHourlyChecks = true;
                                Category.CheckCatalogResult = "E";
                            }
                            else if (!LmeAnnual && !LmeOs)
                            {
                                AbortHourlyChecks = true;
                                Category.CheckCatalogResult = "F";
                            }
                            else
                            {
                                DateTime coverBeginDate = ((CurrentReportingPeriodQuarter == 2) && !LmeAnnual) ? new DateTime(FirstDayCurrentRptPeriod.Year, 5, 1) : FirstDayCurrentRptPeriod;

                                RowFilter = new sFilterPair[3];
                                RowFilter[0].Set("PARAMETER_CD", "HIT");
                                RowFilter[1].Set("BEGIN_DATE", coverBeginDate, eFilterDataType.DateBegan, eFilterPairRelativeCompare.LessThanOrEqual);
                                RowFilter[2].Set("END_DATE", LastDayCurrentRptPeriod, eFilterDataType.DateEnded, eFilterPairRelativeCompare.GreaterThanOrEqual);


                                DataView MpMethodView = FindRows(MpMethodRecords, RowFilter);

                                if (MpMethodView.Count == 0)
                                {
                                    AbortHourlyChecks = true;
                                    Category.CheckCatalogResult = "G";
                                }
                                else
                                {
                                    bool MhhiMethodExists = false;
                                    bool LtffMethodExists = false;
                                    bool DiffMethodExists = false;
                                    bool MhhiSubstituteDataExists = false;

                                    foreach (DataRowView MethodRow in MpMethodView)
                                    {
                                        string MethodCd = cDBConvert.ToString(MethodRow["METHOD_CD"]);

                                        if (MethodCd == "MHHI")
                                            MhhiMethodExists = true;
                                        else if (MethodCd.InList("LTFF,CALC,LTFCALC"))
                                            LtffMethodExists = true;
                                        else
                                            DiffMethodExists = true;

                                        if (cDBConvert.ToString(MethodRow["SUB_DATA_CD"]) == "MHHI")
                                            MhhiSubstituteDataExists = true;
                                    }

                                    if (MhhiMethodExists && !LtffMethodExists && !DiffMethodExists)
                                    {
                                        LmeHiMethod = "MHHI";
                                    }
                                    else if (LtffMethodExists && !MhhiMethodExists && !DiffMethodExists)
                                    {
                                        LmeHiMethod = "LTFF";

                                        if (MhhiSubstituteDataExists)
                                            LmeHiSubstituteDataCode = "MHHI";
                                    }
                                    else
                                    {
                                        AbortHourlyChecks = true;
                                        Category.CheckCatalogResult = "H";
                                    }
                                }
                            }
                        }
                    }
                }

                if (!AbortHourlyChecks)
                {
                    RowFilter = new sFilterPair[1];
                    RowFilter[0].Set("UNIT_MONITOR_CERT_BEGIN_DATE", LastDayCurrentRptPeriod, eFilterDataType.DateBegan, eFilterPairRelativeCompare.LessThanOrEqual);
                    DataView MpProgramRecordsFound = FindRows(MpProgramRecords, RowFilter);
                    bool allProgramsAreNonRue = true;
                    bool retired = true;
                    string PrgCd;
                    string UPID;
                    bool prgretired;
                    foreach (DataRowView drv in MpProgramRecordsFound)
                    {
                        if (cDBConvert.ToDate(drv["END_DATE"], DateTypes.END) >= FirstDayCurrentRptPeriod)
                        {
                            PrgCd = cDBConvert.ToString(drv["PRG_CD"]);
                            UPID = cDBConvert.ToString(drv["UP_ID"]);
                            if (PrgCd.InList(EmParameters.ProgramUsesRueList))
                            {
                                allProgramsAreNonRue = false;
                                RowFilter2 = new sFilterPair[3];
                                DataView ProgExemptRecs = Category.GetCheckParameter("MP_Program_Exemption_Records").ValueAsDataView();
                                RowFilter2[0].Set("EXEMPT_TYPE_CD", "RUE");
                                if (PrgCd.InList(EmParameters.ProgramIsOzoneSeasonList) && CurrentReportingPeriodQuarter <= 2)
                                    RowFilter2[1].Set("BEGIN_DATE", new DateTime(CurrentReportingPeriodYear, 5, 1), eFilterDataType.DateBegan, eFilterPairRelativeCompare.LessThanOrEqual);
                                else
                                    RowFilter2[1].Set("BEGIN_DATE", FirstDayCurrentRptPeriod, eFilterDataType.DateBegan, eFilterPairRelativeCompare.LessThanOrEqual);
                                RowFilter2[2].Set("UP_ID", UPID);
                                DataView ProgExemptRecsFound = FindRows(ProgExemptRecs, RowFilter2);
                                if (ProgExemptRecsFound.Count == 0)
                                {
                                    retired = false;
                                    break;
                                }
                                else
                                {
                                    prgretired = false;
                                    foreach (DataRowView drv2 in ProgExemptRecsFound)
                                    {
                                        if (cDBConvert.ToDate(drv2["END_DATE"], DateTypes.END) >= LastDayCurrentRptPeriod)
                                            prgretired = true;
                                    }
                                    if (!prgretired)
                                    {
                                        retired = false;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    if (allProgramsAreNonRue)
                    {
                        RowFilter = new sFilterPair[1];
                        DataView OpStatusRecs = Category.GetCheckParameter("MP_Operating_Status_Records").ValueAsDataView();
                        RowFilter[0].Set("OP_STATUS_CD", "RET");
                        DataView OpStatusRecsFound = FindRows(OpStatusRecs, RowFilter);
                        foreach (DataRowView drv in OpStatusRecsFound)
                            if (cDBConvert.ToDate(drv["BEGIN_DATE"], DateTypes.START).Year < CurrentReportingPeriodYear &&
                                cDBConvert.ToDate(drv["END_DATE"], DateTypes.END) >= LastDayCurrentRptPeriod)
                            {
                                Category.CheckCatalogResult = "I";
                                break;
                            }
                    }
                    else if (retired)
                        Category.CheckCatalogResult = "I";
                }

                Category.SetCheckParameter("Abort_Hourly_Checks", AbortHourlyChecks, eParameterDataType.Boolean);
                Category.SetCheckParameter("Legacy_Data_Evaluation", LegacyDataEvaluation, eParameterDataType.Boolean);
                Category.SetCheckParameter("LME_HI_Method", LmeHiMethod, eParameterDataType.String);
                Category.SetCheckParameter("LME_HI_Substitute_Data_Code", LmeHiSubstituteDataCode, eParameterDataType.String);
                Category.SetCheckParameter("LME_Annual", LmeAnnual, eParameterDataType.Boolean);
                Category.SetCheckParameter("LME_OS", LmeOs, eParameterDataType.Boolean);
                Category.SetCheckParameter("Annual_Reporting_Requirement", AnnualReportingRequirement, eParameterDataType.Boolean);
                Category.SetCheckParameter("OS_Reporting_Requirement", OsReportingRequirement, eParameterDataType.Boolean);
                Category.SetCheckParameter("Multiple_Stack_Configuration", false, eParameterDataType.Boolean);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURGEN2");
            }

            return ReturnVal;
        }

        /// <summary>
        /// Calculate Total Load for LME Configuration for Reporting Period
        /// </summary>
        /// <param name="Category"></param>
        /// <param name="Log"></param>
        /// <returns></returns>
        /// <remarks>(Formerly HourAgg-9)</remarks>
        public string HOURGEN3(cCategory Category, ref bool Log)
        {
            string ReturnVal = "";

            try
            {
                decimal TotalLoad = 0;
                decimal AprilLoad = 0;
                decimal TotalOpTime = 0;
                decimal AprilOpTime = 0;

                Category.SetCheckParameter("LME_Total_Load", TotalLoad, eParameterDataType.Decimal);
                Category.SetCheckParameter("LME_April_Load", AprilLoad, eParameterDataType.Decimal);
                Category.SetCheckParameter("LME_CP_April_Heat_Input", 0m, eParameterDataType.Decimal);
                Category.SetCheckParameter("LME_CP_Total_Heat_Input", 0m, eParameterDataType.Decimal);
                Category.SetCheckParameter("LME_Total_Optime", 0m, eParameterDataType.Decimal);
                Category.SetCheckParameter("LME_April_OpTime", 0m, eParameterDataType.Decimal);

                string LMEHIMethod = Convert.ToString(Category.GetCheckParameter("LME_HI_Method").ParameterValue);

                if (LMEHIMethod != "")
                {
                    DataView LTFFRecs = (DataView)Category.GetCheckParameter("LTFF_Records").ParameterValue;
                    if (LMEHIMethod == "MHHI")
                    {
                        if (LTFFRecs.Count > 0)
                        {
                            Category.CheckCatalogResult = "A";
                            Category.SetCheckParameter("Abort_Hourly_Checks", true, eParameterDataType.Boolean);
                        }
                    }
                    else
                    {
                        DataView MonitorPlanLocationRecords = Category.GetCheckParameter("Monitoring_Plan_Location_Records").ValueAsDataView();
                        DataView HitDhvLoadSumRecords = Category.GetCheckParameter("HIT_DHV_Load_Sum_Records").ValueAsDataView();

                        for (int MonitorLocationDex = 0; (MonitorLocationDex < MonitorPlanLocationRecords.Count) && (TotalLoad != -1); MonitorLocationDex++)
                        {
                            string LocationName = cDBConvert.ToString(((DataRowView)MonitorPlanLocationRecords[MonitorLocationDex])["LOCATION_NAME"]);

                            if (!LocationName.StartsWith("CP") && !LocationName.StartsWith("CS") &&
                                !LocationName.StartsWith("MP") && !LocationName.StartsWith("MS"))
                            {
                                string MonLocId = cDBConvert.ToString(((DataRowView)MonitorPlanLocationRecords[MonitorLocationDex])["MON_LOC_ID"]);

                                DataView HitDhvLoadSumView = cRowFilter.FindRows(HitDhvLoadSumRecords, new cFilterCondition[] { new cFilterCondition("MON_LOC_ID", MonLocId) });

                                foreach (DataRowView HitDhvLoadSumRow in HitDhvLoadSumView)
                                {
                                    int? ProblemOccurred = cDBConvert.ToNullableInteger(HitDhvLoadSumRow["PROBLEM_OCCURRED"]);

                                    if (ProblemOccurred == 0)
                                    {
                                        string LocationModc = cDBConvert.ToNullableString(HitDhvLoadSumRow["MODC_CD"]);

                                        if (LocationModc == null)
                                        {
                                            decimal? LocationOpTime = HitDhvLoadSumRow["TOTAL_OPTIME"].AsDecimal();
                                            decimal? LocationTotalLoad = HitDhvLoadSumRow["TOTAL"].AsDecimal();

                                            if (LocationTotalLoad.HasValue)
                                            {
                                                Category.AccumCheckAggregate("LME_Total_Load_Array", MonitorLocationDex, LocationTotalLoad.Value);
                                                TotalLoad += LocationTotalLoad.Value;
                                            }

                                            if (LocationOpTime.HasValue)
                                            {
                                                Category.AccumCheckAggregate("LME_Total_OpTime_Array", MonitorLocationDex, LocationOpTime.Value);
                                                TotalOpTime += LocationOpTime.Value;
                                            }

                                            if (Category.GetCheckParameter("LME_OS").ValueAsBool())
                                            {
                                                decimal? LocationAprilOpTime = HitDhvLoadSumRow["APRIL_OPTIME"].AsDecimal();
                                                decimal? LocationAprilLoad = HitDhvLoadSumRow["APRIL"].AsDecimal();

                                                if (LocationAprilLoad.HasValue)
                                                {
                                                    Category.AccumCheckAggregate("LME_April_Load_Array", MonitorLocationDex, LocationAprilLoad.Value);
                                                    AprilLoad += LocationAprilLoad.Value;
                                                }

                                                if (LocationAprilOpTime.HasValue)
                                                {
                                                    Category.AccumCheckAggregate("LME_April_OpTime_Array", MonitorLocationDex, LocationAprilOpTime.Value);
                                                    AprilOpTime += LocationAprilOpTime.Value;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        TotalLoad = -1;
                                        break;
                                    }
                                }
                            }
                        }

                        int RptPeriodId = Category.GetCheckParameter("Current_Reporting_Period").ValueAsInt();
                        DataView RptPeriodTable = Category.GetCheckParameter("Reporting_Period_Lookup_Table").ValueAsDataView();
                        sFilterPair[] RptPeriodCriteria = new sFilterPair[1];
                        RptPeriodCriteria[0].Field = "rpt_period_id";
                        RptPeriodCriteria[0].Value = RptPeriodId;
                        DataRowView RptPeriodRecord = FindRow(RptPeriodTable, RptPeriodCriteria);
                        int rptPrdQrtr = cDBConvert.ToInteger(RptPeriodRecord["quarter"]);

                        //
                        if (Category.GetCheckParameter("LME_OS").ValueAsBool() && rptPrdQrtr == 2)
                        {
                            LTFFRecs = Category.GetCheckParameter("LTFF_Records").ValueAsDataView();
                            string LTFFFilter = LTFFRecs.RowFilter;

                            LTFFRecs.RowFilter = AddToDataViewFilter(LTFFFilter, string.Format("rpt_period_id = {0} and FUEL_FLOW_PERIOD_CD='A'", RptPeriodId));
                            if (LTFFRecs.Count > 0 && AprilLoad == 0m && AprilOpTime == 0m)
                            {
                                Category.CheckCatalogResult = "C";
                                Category.SetCheckParameter("Abort_Hourly_Checks", true, eParameterDataType.Boolean);
                            }
                            else if (LTFFRecs.Count == 0 && (AprilLoad > 0m || AprilOpTime > 1m))
                            {
                                Category.CheckCatalogResult = "F";
                            }
                            else
                            {
                                LTFFRecs.RowFilter = AddToDataViewFilter(LTFFFilter, string.Format("rpt_period_id = {0} and FUEL_FLOW_PERIOD_CD='MJ'", RptPeriodId));
                                if (LTFFRecs.Count > 0)
                                {
                                    if (((TotalLoad - AprilLoad) == 0m) && ((TotalOpTime - AprilOpTime) == 0m))
                                    {
                                        Category.CheckCatalogResult = "E";
                                        Category.SetCheckParameter("Abort_Hourly_Checks", true, eParameterDataType.Boolean);
                                    }
                                }
                                else
                                {
                                    if (((TotalLoad - AprilLoad) > 0m) || ((TotalOpTime - AprilOpTime) > 1m))
                                        Category.CheckCatalogResult = "G";
                                }
                            }

                            LTFFRecs.RowFilter = LTFFFilter;
                        }
                        else
                        {
                            string LTFFFilter = LTFFRecs.RowFilter;
                            LTFFRecs.RowFilter = AddToDataViewFilter(LTFFFilter, string.Format("rpt_period_id = {0}", RptPeriodId));

                            if (LTFFRecs.Count > 0)
                            {
                                if (TotalLoad == 0 && TotalOpTime == 0)
                                {
                                    Category.CheckCatalogResult = "B";
                                    Category.SetCheckParameter("Abort_Hourly_Checks", true, eParameterDataType.Boolean);
                                }
                            }
                            else if (TotalLoad > 0 || TotalOpTime > 1)
                            {
                                Category.CheckCatalogResult = "D";
                            }
                        }

                        Category.SetCheckParameter("LME_Total_Load", TotalLoad, eParameterDataType.Decimal);
                        Category.SetCheckParameter("LME_April_Load", AprilLoad, eParameterDataType.Decimal);
                        Category.SetCheckParameter("LME_Total_Optime", TotalOpTime, eParameterDataType.Decimal);
                        Category.SetCheckParameter("LME_April_OpTime", AprilOpTime, eParameterDataType.Decimal);
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURGEN3");
            }

            return ReturnVal;
        }

        public string HOURGEN4(cCategory Category, ref bool Log)
        // Emission Comment Reporting Period Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentEmissionComment = Category.GetCheckParameter("Current_Emission_Comment").ValueAsDataRowView();

                if (CurrentEmissionComment["RPT_PERIOD_ID"] == DBNull.Value)
                    Category.CheckCatalogResult = "A";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURGEN4");
            }

            return ReturnVal;
        }

        public string HOURGEN5(cCategory Category, ref bool Log)
        // Submission Comment Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentEmissionComment = Category.GetCheckParameter("Current_Emission_Comment").ValueAsDataRowView();

                if (CurrentEmissionComment["SUBMISSION_COMMENT"] == DBNull.Value)
                    Category.CheckCatalogResult = "A";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURGEN5");
            }

            return ReturnVal;
        }

        public string HOURGEN6(cCategory Category, ref bool Log)
        // Duplicate Emission Comment Records
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentEmissionComment = Category.GetCheckParameter("Current_Emission_Comment").ValueAsDataRowView();
                DataView EmissionCommentRecords = Category.GetCheckParameter("Emission_Comment_Records").ValueAsDataView();

                string HourlySubmissionCommentId = cDBConvert.ToString(CurrentEmissionComment["HRLY_SUB_COMMENT_ID"]);
                int RptPeriodId = cDBConvert.ToInteger(CurrentEmissionComment["RPT_PERIOD_ID"]);
                //string SubmissionComment = cDBConvert.ToString(CurrentEmissionComment["SUBMISSION_COMMENT"]);

                if (cRowFilter.CountRows(EmissionCommentRecords, new cFilterCondition[] { new cFilterCondition("HRLY_SUB_COMMENT_ID", HourlySubmissionCommentId, true),
                                                                                  new cFilterCondition("RPT_PERIOD_ID", RptPeriodId, eFilterDataType.Integer)}) > 0)
                    Category.CheckCatalogResult = "A";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURGEN6");
            }

            return ReturnVal;
        }

        public string HOURGEN7(cCategory Category, ref bool Log)
        // Validate LME Eligibility
        {
            string ReturnVal = "";

            try
            {
                if (Category.GetCheckParameter("LME_HI_Method").ValueAsString() != "")
                {
                    bool FinalLMEYear = false;
                    string LMEExceedingParameters = "";

                    DataView MonitoringConfiguration = Category.GetCheckParameter("Monitoring_Plan_Location_Records").ValueAsDataView();

                    DataView MonitorQualifications = Category.GetCheckParameter("MP_Qualification_Records").ValueAsDataView();
                    string sQualFilter = MonitorQualifications.RowFilter;
                    string sQualSort = MonitorQualifications.Sort;

                    DataView OpSuppData = Category.GetCheckParameter("Facility_Operating_Supp_Data_Records").ValueAsDataView();
                    string sOpSuppFilter = OpSuppData.RowFilter;

                    foreach (DataRowView Location in MonitoringConfiguration)
                    {
                        if (Location["unit_id"] != DBNull.Value)
                        {
                            //DataView MonitorQualifications = Category.GetCheckParameter("MP_Qualification_Records").ValueAsDataView();
                            //string sQualFilter = MonitorQualifications.RowFilter;
                            //string sQualSort = MonitorQualifications.Sort;

                            //DataView OpSuppData = Category.GetCheckParameter("Facility_Operating_Supp_Data_Records").ValueAsDataView();
                            //string sOpSuppFilter = OpSuppData.RowFilter;

                            string sMonLocID = cDBConvert.ToString(Location["mon_loc_id"]);

                            int CurrentRptPeriodYear = Category.GetCheckParameter("Current_Reporting_Period_Year").ValueAsInt();
                            DateTime FirstDayCurrentRptPeriod = cDateFunctions.StartDateThisQuarter(Category.GetCheckParameter("Current_Reporting_Period").ValueAsInt());
                            DateTime Dec31YearPriorRptPeriod = new DateTime(CurrentRptPeriodYear - 1, 12, 31);
                            DateTime FirstOSDayCurrentRptPeriod = new DateTime(CurrentRptPeriodYear, 5, 1);
                            if (FirstDayCurrentRptPeriod > FirstOSDayCurrentRptPeriod)
                                FirstOSDayCurrentRptPeriod = FirstDayCurrentRptPeriod;

                            if (Category.GetCheckParameter("LME_Annual").ValueAsBool())
                            {
                                MonitorQualifications.RowFilter = AddToDataViewFilter(sQualFilter, "mon_loc_id = '" + sMonLocID + "'and qual_type_cd = 'LMEA'");
                                MonitorQualifications.RowFilter = AddEvaluationDateRangeToDataViewFilter(MonitorQualifications.RowFilter, FirstDayCurrentRptPeriod, Dec31YearPriorRptPeriod, true, true, false);
                                if (MonitorQualifications.Count == 0)
                                {
                                    Category.CheckCatalogResult = "A";
                                    break;
                                }
                                MonitorQualifications.Sort = "begin_date DESC";
                                if (cDBConvert.ToDate(MonitorQualifications[0]["begin_date"], DateTypes.START).Year < CurrentRptPeriodYear)
                                {
                                    decimal AnnualNox, AnnualSO2;
                                    for (int year = CurrentRptPeriodYear - 3; year <= CurrentRptPeriodYear - 1; year++)
                                    {
                                        AnnualNox = 0;
                                        AnnualSO2 = 0;

                                        for (int quarter = 1; quarter <= 4; quarter++)
                                        {
                                            OpSuppData.RowFilter = AddToDataViewFilter(sOpSuppFilter,
                                                "mon_loc_id = '" + sMonLocID + "' and quarter = " + quarter + " and calendar_year = " + year + "and op_type_cd = 'NOXM'");
                                            if (OpSuppData.Count == 1)
                                                AnnualNox += cDBConvert.ToDecimal(OpSuppData[0]["op_value"]);

                                            OpSuppData.RowFilter = AddToDataViewFilter(sOpSuppFilter,
                                               "mon_loc_id = '" + sMonLocID + "' and quarter = " + quarter + " and calendar_year = " + year + "and op_type_cd = 'SO2M'");
                                            if (OpSuppData.Count == 1)
                                                AnnualSO2 += cDBConvert.ToDecimal(OpSuppData[0]["op_value"]);

                                        }

                                        if (year == CurrentRptPeriodYear - 1)
                                        {
                                            if (AnnualNox > 100 || AnnualSO2 > 25)
                                                FinalLMEYear = true;
                                        }
                                        else
                                        {
                                            if (AnnualNox > 100)
                                                LMEExceedingParameters = LMEExceedingParameters.ListAdd("Annual NOx", false);
                                            if (AnnualSO2 > 25)
                                                LMEExceedingParameters = LMEExceedingParameters.ListAdd("Annual SO2", false);

                                        }
                                    }
                                }
                                //MonitorQualifications.Sort = sQualSort;
                            }

                            if (Category.GetCheckParameter("LME_OS").ValueAsBool())
                            {
                                MonitorQualifications.RowFilter = AddToDataViewFilter(sQualFilter, "mon_loc_id = '" + sMonLocID + "'and qual_type_cd = 'LMES'");
                                MonitorQualifications.RowFilter = AddEvaluationDateRangeToDataViewFilter(MonitorQualifications.RowFilter, FirstOSDayCurrentRptPeriod, Dec31YearPriorRptPeriod, true, true, false);
                                if (MonitorQualifications.Count == 0)
                                {
                                    Category.CheckCatalogResult = "B";
                                    break;
                                }
                                MonitorQualifications.Sort = "begin_date DESC";
                                if (cDBConvert.ToDate(MonitorQualifications[0]["begin_date"], DateTypes.START).Year < CurrentRptPeriodYear)
                                {
                                    decimal OSNox;
                                    for (int year = CurrentRptPeriodYear - 3; year <= CurrentRptPeriodYear - 1; year++)
                                    {
                                        OSNox = 0;


                                        OpSuppData.RowFilter = AddToDataViewFilter(sOpSuppFilter,
                                            "mon_loc_id = '" + sMonLocID + "' and quarter = 2 and calendar_year = " + year + "and op_type_cd = 'NOXMOS'");
                                        if (OpSuppData.Count == 1)
                                            OSNox += cDBConvert.ToDecimal(OpSuppData[0]["op_value"]);

                                        OpSuppData.RowFilter = AddToDataViewFilter(sOpSuppFilter,
                                           "mon_loc_id = '" + sMonLocID + "' and quarter = 3 and calendar_year = " + year + "and op_type_cd = 'NOXM'");
                                        if (OpSuppData.Count == 1)
                                            OSNox += cDBConvert.ToDecimal(OpSuppData[0]["op_value"]);



                                        if (year == CurrentRptPeriodYear - 1)
                                        {
                                            if (OSNox > 50)
                                                FinalLMEYear = true;
                                        }
                                        else
                                        {
                                            if (OSNox > 50)
                                                LMEExceedingParameters = LMEExceedingParameters.ListAdd("Ozone Season NOx", false);

                                        }
                                    }
                                }
                                //MonitorQualifications.Sort = sQualSort;
                            }

                            MonitorQualifications.Sort = sQualSort;
                            MonitorQualifications.RowFilter = sQualFilter;
                            OpSuppData.RowFilter = sOpSuppFilter;
                        }
                    }
                    if (string.IsNullOrEmpty(Category.CheckCatalogResult) && LMEExceedingParameters != "")
                        Category.CheckCatalogResult = "C";
                    else if (string.IsNullOrEmpty(Category.CheckCatalogResult) && FinalLMEYear)
                        Category.CheckCatalogResult = "D";

                    Category.SetCheckParameter("LME_Exceeding_Parameters", LMEExceedingParameters.FormatList(), eParameterDataType.String);

                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURGEN7");
            }

            return ReturnVal;
        }

        public string HOURGEN8(cCategory Category, ref bool Log)
        // Validate LME Eligibility
        {
            string ReturnVal = "";

            try
            {
                MpSuccessfullyEvaluated.SetValue(false, Category);

                DataRowView CurrentMonitoringPlanRecord = Category.GetCheckParameter("Current_Monitoring_Plan_Record").ValueAsDataRowView();

                if (cDBConvert.ToString(CurrentMonitoringPlanRecord["SEVERITY_CD"]).InList("CRIT1,FATAL"))
                    Category.CheckCatalogResult = "A";
                else
                {
                    if (cDBConvert.ToString(CurrentMonitoringPlanRecord["NEEDS_EVAL_FLG"]) == "Y" &&
                      cDBConvert.ToString(CurrentMonitoringPlanRecord["MUST_SUBMIT"]) == "Y")
                        Category.CheckCatalogResult = "B";
                    else
                    {
                        MpSuccessfullyEvaluated.SetValue(true, Category);
                    }
                }

            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURGEN8");
            }

            return ReturnVal;
        }

        public string HOURGEN9(cCategory Category, ref bool Log)
        // Validate LME Eligibility
        {
            string ReturnVal = "";

            try
            {
                DataView QATests = Category.GetCheckParameter("QA_Cert_Tests_for_EM_Evaluation").ValueAsDataView();
                string sOldFilter = QATests.RowFilter;
                int CurrentReportingPeriodYear = Category.GetCheckParameter("Current_Reporting_Period_Year").ValueAsInt();
                int CurrentReportingPeriodQuarter = Category.GetCheckParameter("Current_Reporting_Period_Quarter").ValueAsInt();
                DateTime LastDayCurrentRptPeriod = cDateFunctions.LastDateThisQuarter(CurrentReportingPeriodYear, CurrentReportingPeriodQuarter);
                QATests.RowFilter = AddToDataViewFilter(sOldFilter, "MUST_SUBMIT = 'Y'");
                QATests.RowFilter = AddToDataViewFilter(QATests.RowFilter, "SEVERITY_CD IN ('CRIT1','FATAL')");
                QATests.RowFilter = AddToDataViewFilter(QATests.RowFilter, "END_DATE <= '" + LastDayCurrentRptPeriod.ToShortDateString() + "'");

                if (QATests.Count > 0)
                    Category.CheckCatalogResult = "A";
                else
                {
                    QATests.RowFilter = AddToDataViewFilter(sOldFilter, "CAN_SUBMIT = 'Y'");
                    QATests.RowFilter = AddToDataViewFilter(QATests.RowFilter, "UPDATED_STATUS_FLG = 'Y'");
                    QATests.RowFilter = AddToDataViewFilter(QATests.RowFilter, "SEVERITY_CD IN ('CRIT1','FATAL')");
                    QATests.RowFilter = AddToDataViewFilter(QATests.RowFilter, "END_DATE <= '" + LastDayCurrentRptPeriod.ToShortDateString() + "'");
                    if (QATests.Count > 0)
                        Category.CheckCatalogResult = "A";
                    else
                    {
                        QATests.RowFilter = AddToDataViewFilter(sOldFilter, "MUST_SUBMIT = 'Y'");
                        QATests.RowFilter = AddToDataViewFilter(QATests.RowFilter, "NEEDS_EVAL_FLG = 'Y'");
                        QATests.RowFilter = AddToDataViewFilter(QATests.RowFilter, "END_DATE <= '" + LastDayCurrentRptPeriod.ToShortDateString() + "'");

                        if (QATests.Count > 0)
                            Category.CheckCatalogResult = "B";
                        else
                        {
                            QATests.RowFilter = AddToDataViewFilter(sOldFilter, "CAN_SUBMIT = 'Y'");
                            QATests.RowFilter = AddToDataViewFilter(QATests.RowFilter, "UPDATED_STATUS_FLG = 'Y'");
                            QATests.RowFilter = AddToDataViewFilter(QATests.RowFilter, "NEEDS_EVAL_FLG = 'Y'");
                            QATests.RowFilter = AddToDataViewFilter(QATests.RowFilter, "END_DATE <= '" + LastDayCurrentRptPeriod.ToShortDateString() + "'");

                            if (QATests.Count > 0)
                                Category.CheckCatalogResult = "B";
                            else
                            {
                                QATests.RowFilter = AddToDataViewFilter(sOldFilter, "MUST_SUBMIT = 'Y'");
                                QATests.RowFilter = AddToDataViewFilter(QATests.RowFilter, "TEST_SUM_ID is null");
                                QATests.RowFilter = AddToDataViewFilter(QATests.RowFilter, "END_DATE <= '" + LastDayCurrentRptPeriod.ToShortDateString() + "'");

                                if (QATests.Count > 0)
                                    Category.CheckCatalogResult = "C";
                            }
                        }
                    }
                }
                QATests.RowFilter = sOldFilter;

            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURGEN9");
            }

            return ReturnVal;
        }

        public string HOURGEN10(cCategory Category, ref bool Log)
        // Validate LME Eligibility
        {
            string ReturnVal = "";

            try
            {
                DataView QAEvents = Category.GetCheckParameter("QA_Cert_Events_for_EM_Evaluation").ValueAsDataView();
                string sOldFilter = QAEvents.RowFilter;
                int CurrentReportingPeriodYear = Category.GetCheckParameter("Current_Reporting_Period_Year").ValueAsInt();
                int CurrentReportingPeriodQuarter = Category.GetCheckParameter("Current_Reporting_Period_Quarter").ValueAsInt();
                DateTime LastDayCurrentRptPeriod = cDateFunctions.LastDateThisQuarter(CurrentReportingPeriodYear, CurrentReportingPeriodQuarter);
                QAEvents.RowFilter = AddToDataViewFilter(sOldFilter, "SEVERITY_CD IN ('CRIT1','FATAL')");
                QAEvents.RowFilter = AddToDataViewFilter(QAEvents.RowFilter, "MUST_SUBMIT = 'Y'");
                QAEvents.RowFilter = AddToDataViewFilter(QAEvents.RowFilter, "QA_CERT_EVENT_DATE <= '" + LastDayCurrentRptPeriod.ToShortDateString() + "'");

                if (QAEvents.Count > 0)
                    Category.CheckCatalogResult = "A";
                else
                {
                    QAEvents.RowFilter = AddToDataViewFilter(sOldFilter, "NEEDS_EVAL_FLG = 'Y'");
                    QAEvents.RowFilter = AddToDataViewFilter(QAEvents.RowFilter, "MUST_SUBMIT = 'Y'");
                    QAEvents.RowFilter = AddToDataViewFilter(QAEvents.RowFilter, "QA_CERT_EVENT_DATE <= '" + LastDayCurrentRptPeriod.ToShortDateString() + "'");

                    if (QAEvents.Count > 0)
                        Category.CheckCatalogResult = "B";
                }
                QAEvents.RowFilter = sOldFilter;

            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURGEN10");
            }

            return ReturnVal;
        }

        public string HOURGEN11(cCategory Category, ref bool Log)
        // Validate LME Eligibility
        {
            string ReturnVal = "";

            try
            {
                DataView TEEs = Category.GetCheckParameter("Test_Extension_Exemptions_for_EM_Evaluation").ValueAsDataView();
                string sOldFilter = TEEs.RowFilter;
                int CurrentReportingPeriodYear = Category.GetCheckParameter("Current_Reporting_Period_Year").ValueAsInt();
                int CurrentReportingPeriodQuarter = Category.GetCheckParameter("Current_Reporting_Period_Quarter").ValueAsInt();
                TEEs.RowFilter = AddToDataViewFilter(sOldFilter, "SEVERITY_CD IN ('CRIT1','FATAL')");
                TEEs.RowFilter = AddToDataViewFilter(TEEs.RowFilter, "MUST_SUBMIT = 'Y'");
                TEEs.RowFilter = AddToDataViewFilter(TEEs.RowFilter, "CALENDAR_YEAR < " + CurrentReportingPeriodYear.ToString() +
                    " OR (CALENDAR_YEAR = " + CurrentReportingPeriodYear.ToString() + " AND QUARTER <= " + CurrentReportingPeriodQuarter.ToString() + ")");

                if (TEEs.Count > 0)
                    Category.CheckCatalogResult = "A";
                else
                {
                    TEEs.RowFilter = AddToDataViewFilter(sOldFilter, "NEEDS_EVAL_FLG = 'Y'");
                    TEEs.RowFilter = AddToDataViewFilter(TEEs.RowFilter, "MUST_SUBMIT = 'Y'");
                    TEEs.RowFilter = AddToDataViewFilter(TEEs.RowFilter, "CALENDAR_YEAR < " + CurrentReportingPeriodYear.ToString() +
                      " OR (CALENDAR_YEAR = " + CurrentReportingPeriodYear.ToString() + " AND QUARTER <= " + CurrentReportingPeriodQuarter.ToString() + ")");

                    if (TEEs.Count > 0)
                        Category.CheckCatalogResult = "B";
                }
                TEEs.RowFilter = sOldFilter;

            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURGEN11");
            }

            return ReturnVal;
        }

        public string HOURGEN12(cCategory Category, ref bool Log)
        // Validate LME Eligibility
        {
            string ReturnVal = "";

            try
            {
                DataView EMFiles = Category.GetCheckParameter("Configuration_Emissions_File").ValueAsDataView();
                string sOldFilter = EMFiles.RowFilter;
                int CurrentReportingPeriodYear = Category.GetCheckParameter("Current_Reporting_Period_Year").ValueAsInt();
                int CurrentReportingPeriodQuarter = Category.GetCheckParameter("Current_Reporting_Period_Quarter").ValueAsInt();
                EMFiles.RowFilter = AddToDataViewFilter(sOldFilter, "CAN_SUBMIT = 'Y'");
                EMFiles.RowFilter = AddToDataViewFilter(EMFiles.RowFilter, "SEVERITY_CD IN ('CRIT1','FATAL')");
                EMFiles.RowFilter = AddToDataViewFilter(EMFiles.RowFilter, "CALENDAR_YEAR < " + CurrentReportingPeriodYear.ToString() +
                    " OR (CALENDAR_YEAR = " + CurrentReportingPeriodYear.ToString() + " AND QUARTER < " + CurrentReportingPeriodQuarter.ToString() + ")");

                if (EMFiles.Count > 0)
                    Category.CheckCatalogResult = "A";
                else
                {

                    EMFiles.RowFilter = AddToDataViewFilter(sOldFilter, "SUBMISSION_AVAILABILITY_CD = 'CRITERR'");
                    EMFiles.RowFilter = AddToDataViewFilter(EMFiles.RowFilter, "CALENDAR_YEAR < " + CurrentReportingPeriodYear.ToString() +
                      " OR (CALENDAR_YEAR = " + CurrentReportingPeriodYear.ToString() + " AND QUARTER < " + CurrentReportingPeriodQuarter.ToString() + ")");
                    if (EMFiles.Count > 0)
                        Category.CheckCatalogResult = "A";
                    else
                    {
                        EMFiles.RowFilter = AddToDataViewFilter(sOldFilter, "CAN_SUBMIT = 'Y'");
                        EMFiles.RowFilter = AddToDataViewFilter(EMFiles.RowFilter, "NEEDS_EVAL_FLG = 'Y'");
                        EMFiles.RowFilter = AddToDataViewFilter(EMFiles.RowFilter, "CALENDAR_YEAR < " + CurrentReportingPeriodYear.ToString() +
                          " OR (CALENDAR_YEAR = " + CurrentReportingPeriodYear.ToString() + " AND QUARTER < " + CurrentReportingPeriodQuarter.ToString() + ")");

                        if (EMFiles.Count > 0)
                            Category.CheckCatalogResult = "B";
                        else
                        {
                            EMFiles.RowFilter = AddToDataViewFilter(sOldFilter, "CAN_SUBMIT = 'Y'");
                            EMFiles.RowFilter = AddToDataViewFilter(EMFiles.RowFilter, "UPDATED_STATUS_FLG = 'NODATA'");
                            EMFiles.RowFilter = AddToDataViewFilter(EMFiles.RowFilter, "CALENDAR_YEAR < " + CurrentReportingPeriodYear.ToString() +
                              " OR (CALENDAR_YEAR = " + CurrentReportingPeriodYear.ToString() + " AND QUARTER < " + CurrentReportingPeriodQuarter.ToString() + ")");
                            if (EMFiles.Count > 0)
                                Category.CheckCatalogResult = "C";
                            else
                            {
                                EMFiles.RowFilter = AddToDataViewFilter(sOldFilter, "SUBMISSION_AVAILABILITY_CD = 'NOTSUB'");
                                EMFiles.RowFilter = AddToDataViewFilter(EMFiles.RowFilter, "CALENDAR_YEAR < " + CurrentReportingPeriodYear.ToString() +
                                  " OR (CALENDAR_YEAR = " + CurrentReportingPeriodYear.ToString() + " AND QUARTER < " + CurrentReportingPeriodQuarter.ToString() + ")");
                                if (EMFiles.Count > 0)
                                    Category.CheckCatalogResult = "C";
                            }
                        }
                    }
                }
                EMFiles.RowFilter = sOldFilter;

            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURGEN12");
            }

            return ReturnVal;
        }

        public string HOURGEN13(cCategory Category, ref bool Log)
        // Determine If File Can Be Submitted
        {
            string ReturnVal = "";

            try
            {
                DataView EmissionsFileRecords = Category.GetCheckParameter("Emissions_File_Records").ValueAsDataView();
                int CurrentReportingPeriod = Category.GetCheckParameter("Current_Reporting_Period").ValueAsInt();

                cFilterCondition[] RowFilter = new cFilterCondition[] { new cFilterCondition("RPT_PERIOD_ID", CurrentReportingPeriod.AsString()) };

                DataRowView EmissionsFileRow = cRowFilter.FindRow(EmissionsFileRecords, RowFilter);

                if ((EmissionsFileRow == null) || EmissionsFileRow["SUBMISSION_AVAILABILITY_CD"].IsDbNull())
                    Category.CheckCatalogResult = "A";
                else if (!EmissionsFileRow["SUBMISSION_AVAILABILITY_CD"].AsString().InList("GRANTED,REQUIRE"))
                    Category.CheckCatalogResult = "B";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURGEN13");
            }

            return ReturnVal;
        }

        public string HOURGEN14(cCategory Category, ref bool Log)
        // Ignored Offline Daily Calibration Check
        {
            string ReturnVal = "";

            try
            {
                if (Category.GetCheckParameter("Ignored_Daily_Calibration_Tests").ValueAsBool())
                {
                    Category.SetCheckParameter("Ignored_Daily_Calibration_Tests", false, eParameterDataType.Boolean);
                    Category.CheckCatalogResult = "A";
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURGEN14");
            }

            return ReturnVal;
        }

        /// <summary>
        /// HOURGEN-15: Expiring Test Check
        /// </summary>
        /// <param name="category">Category Object</param>
        /// <param name="log">Indicates whether to log results.</param>
        /// <returns>Returns error message if check fails to run correctly.</returns>
        public string HOURGEN15(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                ExpiredSystems.SetValue(null, category);
                ExpiringSystems.SetValue(null, category);
                ExpirationText.SetValue("have expired", category);

                cReportingPeriod currentReportingPeriod = new cReportingPeriod(CurrentReportingPeriod.Value.Value);

                // Flow System
                if (FlowSystemIdArray.Value[LocationPos.Value.Value].IsNotNull())
                {
                    foreach (string monSysId in FlowSystemIdArray.Value[LocationPos.Value.Value].Split(','))
                    {
                        DataView rataTestRecordsView
                          = cRowFilter.FindRows(RataTestRecordsByLocationForQaStatus.Value,
                                                new cFilterCondition[] { new cFilterCondition("MON_LOC_ID", CurrentMonitorLocationId.Value),
                                                             new cFilterCondition("MON_SYS_ID", monSysId) });

                        DateTime? latestEndDate = null;
                        string testResultCd = null;
                        {
                            foreach (DataRowView rataTestRow in rataTestRecordsView)
                            {
                                string opLevelCdList = rataTestRow["OP_LEVEL_CD_LIST"].AsString();
                                DateTime? endDate = rataTestRow["END_DATE"].AsDateTime();

                                if ((opLevelCdList.ListCount() == 3) &&
                                    (endDate.Default(DateTime.MinValue) > latestEndDate.Default(DateTime.MinValue)))
                                {
                                    latestEndDate = endDate;
                                    testResultCd = rataTestRow["TEST_REASON_CD"].AsString();
                                }
                            }
                        }

                        if (latestEndDate.HasValue)
                        {
                            DateTime? expirationDate = null;

                            if (testResultCd == "INITIAL")
                            {
                                DataView QACertEventsforEMEvaluation = category.GetCheckParameter("QA_Cert_Events_for_EM_Evaluation").ValueAsDataView();
                                DataRowView qaCertEventEvalRecord = cRowFilter.FindMostRecentRow(QACertEventsforEMEvaluation, currentReportingPeriod.EndedDate, 23, "LAST_TEST_COMPLETED_DATE", "LAST_TEST_COMPLETED_HOUR",
                                        new cFilterCondition[] { new cFilterCondition("MON_LOC_ID", CurrentMonitorLocationId.Value),
                                                     new cFilterCondition("MON_SYS_ID", monSysId),
                                                     new cFilterCondition("QA_CERT_EVENT_CD", "305")});


                                if (qaCertEventEvalRecord != null && (qaCertEventEvalRecord["LAST_TEST_COMPLETED_DATE"].AsEndDateTime() > latestEndDate))
                                {
                                    expirationDate = cDateFunctions.LastDateThisQuarter(qaCertEventEvalRecord["LAST_TEST_COMPLETED_DATE"].AsBeginDateTime()).AddYears(5);
                                }
                                else
                                {
                                    expirationDate = cDateFunctions.LastDateThisQuarter(latestEndDate.Value).AddYears(5);
                                }
                            }
                            else
                            {
                                expirationDate = cDateFunctions.LastDateThisQuarter(latestEndDate.Value).AddYears(5);
                            }
                            string systemIdentifier = rataTestRecordsView[0]["SYSTEM_IDENTIFIER"].AsString();

                            if (expirationDate < CheckEngine.NowDate)
                            {
                                ExpiredSystems.AggregateValue(systemIdentifier);
                            }
                            else if (expirationDate <= currentReportingPeriod.EndedDate)
                            {
                                ExpirationText.SetValue("will be expiring at the end of the reporting period", category);
                                ExpiredSystems.AggregateValue(systemIdentifier);
                            }
                            else if (expirationDate <= cDateFunctions.LastDateNextQuarter(currentReportingPeriod.EndedDate))
                            {
                                ExpiringSystems.AggregateValue(systemIdentifier);
                            }
                        }
                    }

                    if (ExpiredSystems.Value.IsNotNull() && ExpiringSystems.Value.IsNotNull())
                        category.CheckCatalogResult = "A";
                    else if (ExpiredSystems.Value.IsNotNull())
                        category.CheckCatalogResult = "B";
                    else if (ExpiringSystems.Value.IsNotNull())
                        category.CheckCatalogResult = "C";
                }

                // NOxE System
                else if (NoxeSystemIdArray.Value[LocationPos.Value.Value].IsNotNull())
                {
                    foreach (string monSysId in NoxeSystemIdArray.Value[LocationPos.Value.Value].Split(','))
                    {
                        DataView aeTestRecordsView
                          = cRowFilter.FindRows(AppendixETestRecordsByLocationForQaStatus.Value,
                                                new cFilterCondition[] { new cFilterCondition("MON_LOC_ID", CurrentMonitorLocationId.Value),
                                                             new cFilterCondition("MON_SYS_ID", monSysId) });

                        DateTime? latestEndDate = null;
                        {
                            foreach (DataRowView aeTestRow in aeTestRecordsView)
                            {
                                DateTime? endDate = aeTestRow["END_DATE"].AsDateTime();

                                if (endDate.Default(DateTime.MinValue) > latestEndDate.Default(DateTime.MinValue))
                                {
                                    latestEndDate = endDate;
                                }
                            }
                        }

                        if (latestEndDate.HasValue)
                        {
                            DateTime expirationDate = cDateFunctions.LastDateThisQuarter(latestEndDate.Value).AddYears(5);
                            string systemIdentifier = aeTestRecordsView[0]["SYSTEM_IDENTIFIER"].AsString();

                            if (expirationDate < CheckEngine.NowDate)
                            {
                                ExpiredSystems.AggregateValue(systemIdentifier);
                            }
                            else if (expirationDate <= currentReportingPeriod.EndedDate)
                            {
                                ExpirationText.SetValue("will be expiring at the end of the reporting period", category);
                                ExpiredSystems.AggregateValue(systemIdentifier);
                            }
                            else if (expirationDate <= cDateFunctions.LastDateNextQuarter(currentReportingPeriod.EndedDate))
                            {
                                ExpiringSystems.AggregateValue(systemIdentifier);
                            }
                        }
                    }

                    if (ExpiredSystems.Value.IsNotNull() && ExpiringSystems.Value.IsNotNull())
                        category.CheckCatalogResult = "D";
                    else if (ExpiredSystems.Value.IsNotNull())
                        category.CheckCatalogResult = "E";
                    else if (ExpiringSystems.Value.IsNotNull())
                        category.CheckCatalogResult = "F";
                }

                // LME Fuel
                else if (LmeFuelArray.Value[LocationPos.Value.Value].IsNotNull())
                {
                    foreach (string fuelCd in LmeFuelArray.Value[LocationPos.Value.Value].Split(','))
                    {
                        DataView unitDefaultTestRecordsView
                          = cRowFilter.FindRows(UnitDefaultTestRecordsByLocationForQaStatus.Value,
                                                new cFilterCondition[] { new cFilterCondition("MON_LOC_ID", CurrentMonitorLocationId.Value),
                                                             new cFilterCondition("FUEL_CD", fuelCd) });

                        DateTime? latestEndDate = null;
                        {
                            foreach (DataRowView unitDefaultTestRow in unitDefaultTestRecordsView)
                            {
                                DateTime? endDate = unitDefaultTestRow["END_DATE"].AsDateTime();

                                if (endDate.Default(DateTime.MinValue) > latestEndDate.Default(DateTime.MinValue))
                                {
                                    latestEndDate = endDate;
                                }
                            }
                        }

                        if (latestEndDate.HasValue)
                        {
                            DateTime expirationDate = cDateFunctions.LastDateThisQuarter(latestEndDate.Value).AddYears(5);

                            if (expirationDate < CheckEngine.NowDate)
                            {
                                ExpiredSystems.AggregateValue(fuelCd);
                            }
                            else if (expirationDate <= currentReportingPeriod.EndedDate)
                            {
                                ExpirationText.SetValue("will be expiring at the end of the reporting period", category);
                                ExpiredSystems.AggregateValue(fuelCd);
                            }
                            else if (expirationDate <= cDateFunctions.LastDateNextQuarter(currentReportingPeriod.EndedDate))
                            {
                                ExpiringSystems.AggregateValue(fuelCd);
                            }
                        }
                    }

                    if (ExpiredSystems.Value.IsNotNull() && ExpiringSystems.Value.IsNotNull())
                        category.CheckCatalogResult = "G";
                    else if (ExpiredSystems.Value.IsNotNull())
                        category.CheckCatalogResult = "H";
                    else if (ExpiringSystems.Value.IsNotNull())
                        category.CheckCatalogResult = "I";
                }
            }
            catch (Exception ex)
            {
                returnVal = category.CheckEngine.FormatError(ex);
            }

            return returnVal;
        }

        /// <summary>
        /// HOURGEN-16: Ignored Offline Daily Interference Check
        /// </summary>
        /// <param name="category">Category Object</param>
        /// <param name="log">Indicates whether to log results.</param>
        /// <returns>Returns error message if check fails to run correctly.</returns>
        public string HOURGEN16(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                if (IgnoredDailyInterferenceTests.Value.Default(false))
                    category.CheckCatalogResult = "A";
            }
            catch (Exception ex)
            {
                returnVal = category.CheckEngine.FormatError(ex);
            }

            return returnVal;
        }

        /// <summary>
        /// HOURGEN-17: Missing Peaking Qualification Percent Check
        /// </summary>
        /// <param name="category">Category Object</param>
        /// <param name="log">Indicates whether to log results.</param>
        /// <returns>Returns error message if check fails to run correctly.</returns>
        public string HOURGEN17(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                QualificationPercentMissingList.SetValue("", category);

                if (MpSuccessfullyEvaluated.Value.Default(false))
                {
                    DataView mpQualificationView = cRowFilter.FindActiveRows(
                                                                              MpQualificationRecords.Value,
                                                                              CurrentReportingPeriodBeginHour.Value.Default(DateTime.MinValue),
                                                                              CurrentReportingPeriodEndHour.Value.Default(DateTime.MaxValue),
                                                                              new cFilterCondition[]
                                                                              {
                                                                      new cFilterCondition("QUAL_TYPE_CD", "PK,SK,GF", eFilterConditionStringCompare.InList)
                                                                              }
                                                                            );

                    foreach (DataRowView mpQualificationRow in mpQualificationView)
                    {
                        string monQualId = mpQualificationRow["MON_QUAL_ID"].AsString();

                        DataRowView mpQualificationPercentRow = cRowFilter.FindRow(
                                                                                    MpQualificationPercentRecords.Value,
                                                                                    new cFilterCondition[]
                                                                                    {
                                                                          new cFilterCondition("MON_QUAL_ID", monQualId),
                                                                          new cFilterCondition("QUAL_YEAR", CurrentReportingPeriodYear.Value.Default(0), eFilterDataType.Integer)
                                                                                    }
                                                                                  );

                        if (mpQualificationPercentRow == null)
                        {
                            switch (mpQualificationRow["QUAL_TYPE_CD"].AsString())
                            {
                                case "GF": QualificationPercentMissingList.AggregateValue("Gas-Fired Unit " + mpQualificationRow["LOCATION_ID"].AsString()); break;
                                case "PK": QualificationPercentMissingList.AggregateValue("Year-Round Peaking Unit " + mpQualificationRow["LOCATION_ID"].AsString()); break;
                                case "SK": QualificationPercentMissingList.AggregateValue("Ozone-Season Peaking Unit " + mpQualificationRow["LOCATION_ID"].AsString()); break;
                            }
                        }
                    }

                    if (QualificationPercentMissingList.Value != "")
                    {
                        QualificationPercentMissingList.SetValue(QualificationPercentMissingList.Value.FormatList(), category);
                        category.CheckCatalogResult = "A";
                    }
                }
            }
            catch (Exception ex)
            {
                returnVal = category.CheckEngine.FormatError(ex);
            }

            return returnVal;
        }

        /// <summary>
        /// HOURGEN-18: Validate Unit Fuel
        /// </summary>
        /// <param name="category">Category Object</param>
        /// <param name="log">Indicates whether to log results.</param>
        /// <returns>Returns error message if check fails to run correctly.</returns>
        public string HOURGEN18(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                int? CurrentReportingPeriodQuarter = category.GetCheckParameter("Current_Reporting_Period_Quarter").AsInteger();

                if (CurrentReportingPeriodQuarter == 1)
                {
                    DataView MPLocationRecords = category.GetCheckParameter("Monitoring_Plan_Location_Records").AsDataView();
                    DataView FacUnitFuelRecords = category.GetCheckParameter("Facility_Unit_Fuel_Records").AsDataView();

                    foreach (DataRowView MonitorPlanLocationRow in MPLocationRecords)
                    {
                        string LocationName = MonitorPlanLocationRow["LOCATION_NAME"].AsString();

                        if (!LocationName.StartsWith("CP") && !LocationName.StartsWith("CS") &&
                            !LocationName.StartsWith("MP") && !LocationName.StartsWith("MS"))
                        {
                            string MonLocId = MonitorPlanLocationRow["MON_LOC_ID"].AsString();

                            DataView FacUnitFuelRows = cRowFilter.FindActiveRows(FacUnitFuelRecords,
                                                                                 EmParameters.CurrentReportingPeriodBeginHour.Value.Date,
                                                                                 EmParameters.CurrentReportingPeriodEndHour.Value.Date,
                                                                                 new cFilterCondition[]
                                                                                 {
                                                                     new cFilterCondition("INDICATOR_CD", "P"),
                                                                     new cFilterCondition("MON_LOC_ID", MonLocId)
                                                                                 });

                            if (FacUnitFuelRows.Count > 0)
                            {
                                int? SumOfOpHrs = 0;
                                DataView OpSuppDataRecords = category.GetCheckParameter("Facility_Operating_Supp_Data_Records").AsDataView();
                                DataView OpSuppDataFilteredRows = cRowFilter.FindRows(OpSuppDataRecords,
                                                                                    new cFilterCondition[]
                                                                                        {
                                                                          new cFilterCondition("PARAMETER_CD", "OPHOURS"),
                                                                          new cFilterCondition("FUEL_CD", null),
                                                                          new cFilterCondition("MON_LOC_ID", MonLocId),
                                                                          new cFilterCondition("CALENDAR_YEAR", cDBConvert.ToString(category.GetCheckParameter("Current_Reporting_Period_Year").ValueAsInt() - 1)),
                                                                                        });

                                if (OpSuppDataFilteredRows.Count > 0)
                                {
                                    foreach (DataRowView OpSuppDataRow in OpSuppDataFilteredRows)
                                    {
                                        SumOfOpHrs += cDBConvert.ToInteger(OpSuppDataRow["OP_VALUE"]);
                                    }
                                }

                                if (SumOfOpHrs > 168)
                                {
                                    Dictionary<string, int> SumOfOpHrsByFuel = new Dictionary<string, int>();
                                    OpSuppDataFilteredRows = cRowFilter.FindRows(OpSuppDataRecords,
                                                                                    new cFilterCondition[]
                                                                                          {
                                                                          new cFilterCondition("PARAMETER_CD", "OPHOURS"),
                                                                          new cFilterCondition("MON_LOC_ID", MonLocId),
                                                                          new cFilterCondition("FUEL_CD", null, true),
                                                                          new cFilterCondition("CALENDAR_YEAR", cDBConvert.ToString(category.GetCheckParameter("Current_Reporting_Period_Year").ValueAsInt() - 1)),
                                                                                          });

                                    if (OpSuppDataFilteredRows.Count > 0)
                                    {
                                        foreach (DataRowView OpSuppDataRow in OpSuppDataFilteredRows)
                                        {
                                            if (!SumOfOpHrsByFuel.ContainsKey(OpSuppDataRow["FUEL_CD"].AsString()))
                                            {
                                                SumOfOpHrsByFuel.Add(OpSuppDataRow["FUEL_CD"].AsString(), cDBConvert.ToInteger(OpSuppDataRow["OP_VALUE"]));
                                            }
                                            else
                                                SumOfOpHrsByFuel[OpSuppDataRow["FUEL_CD"].AsString()] += cDBConvert.ToInteger(OpSuppDataRow["OP_VALUE"]);
                                        }
                                    }

                                    foreach (string key in SumOfOpHrsByFuel.Keys)
                                    {
                                        if ((SumOfOpHrsByFuel[key] / SumOfOpHrs) > .60)
                                        {
                                            foreach (DataRowView row in FacUnitFuelRows)
                                            {
                                                if (row["FUEL_CD"].AsString() != key)
                                                {
                                                    category.CheckCatalogResult = "A";
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                returnVal = category.CheckEngine.FormatError(ex);
            }

            return returnVal;

        }

        /// <summary>
        /// HOURGEN-19: Initialize Sorbent Trap Check Parameters
        /// </summary>
        /// <param name="category">Category Object</param>
        /// <param name="log">Indicates whether to log results.</param>
        /// <returns>Returns error message if check fails to run correctly.</returns>
        public string HOURGEN19(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                EmParameters.MatsSorbentTrapDictionary = new Dictionary<string, SorbentTrapEvalInformation>();
                EmParameters.MatsSamplingTrainDictionary = new Dictionary<string, SamplingTrainEvalInformation>();

                EmParameters.MatsSorbentTrapListByLocationArray = new List<SorbentTrapEvalInformation>[EmParameters.CurrentLocationCount.Value];
                {
                    for (int dex = 0; dex < EmParameters.MatsSorbentTrapListByLocationArray.Length; dex++)
                        EmParameters.MatsSorbentTrapListByLocationArray[dex] = new List<SorbentTrapEvalInformation>();
                }

                EmParameters.MatsSorbentTrapEvaluationNeeded = false;

                // Update MatsSorbentTrapEvaluationNeeded if sorbent traps exist in emission report and is not from supplemental data.
                if (EmParameters.MatsSorbentTrapRecords.CountRows(new cFilterCondition[] { new cFilterCondition("SUPP_DATA_IND", "1", true) }) > 0)
                {
                    EmParameters.MatsSorbentTrapEvaluationNeeded = true;
                }
            }
            catch (Exception ex)
            {
                returnVal = category.CheckEngine.FormatError(ex);
            }

            return returnVal;
        }

        /// <summary>
        /// HOURGEN-20: Initialize Weekly System Integrity Test Dictionary
        /// </summary>
        /// <param name="category">Category Object</param>
        /// <param name="log">Indicates whether to log results.</param>
        /// <returns>Returns error message if check fails to run correctly.</returns>
        public string HOURGEN20(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                EmParameters.WsiTestDictionary = new Dictionary<string, WsiTestStatusInformation>();
            }
            catch (Exception ex)
            {
                returnVal = category.CheckEngine.FormatError(ex);
            }

            return returnVal;
        }

        /// <summary>
        /// HOURGEN-21: Initialize General Lists
        /// </summary>
        /// <param name="category">Category Object</param>
        /// <param name="log">Indicates whether to log results.</param>
        /// <returns>Returns error message if check fails to run correctly.</returns>
        public string HOURGEN21(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                EmParameters.TestResultCodeList = EmParameters.TestResultCodeLookupTable.SourceView.DistinctValues("TEST_RESULT_CD").DelimitedList();
            }
            catch (Exception ex)
            {
                returnVal = category.CheckEngine.FormatError(ex);
            }

            return returnVal;
        }

        /// <summary>
        /// HOURGEN-22: Initialize System Parameters
        /// </summary>
        /// <param name="category">Category Object</param>
        /// <param name="log">Indicates whether to log results.</param>
        /// <returns>Returns error message if check fails to run correctly.</returns>
        public string HOURGEN22(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                EmParameters.MatsDailyCalRequiredDatehour = null;

                VwSystemParameterRow systemParameterRow;
                DateTime dateTimeValue;

                // MATS_RULE System Parameter
                {
                    systemParameterRow = EmParameters.SystemParameterLookupTable.FindRow(new cFilterCondition("SYS_PARAM_NAME", "MATS_RULE"));

                    if (systemParameterRow != null)
                    {
                        // DailyCalibrationRequiredDatehour System Parameter Value
                        if (DateTime.TryParse(systemParameterRow.ParamValue2, out dateTimeValue))
                        {
                            EmParameters.MatsDailyCalRequiredDatehour = dateTimeValue;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                returnVal = category.CheckEngine.FormatError(ex);
            }

            return returnVal;
        }

        /// <summary>
        /// HOURGEN-23
        /// 
        /// Initializes program code lists that contain programs that:
        /// 
        /// 1) Are ozone season programs
        /// 2) Use RUEs.
        /// 3) Require SO2 System certification.
        /// 4) Require NOX System certification.
        /// 5) Require NOX Concentration System certification.
        /// </summary>
        /// <param name="category"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public string HOURGEN23(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                EmParameters.ProgramIsOzoneSeasonList = "";
                EmParameters.ProgramRequiresNoxSystemCertificationList = "";
                EmParameters.ProgramRequiresNoxcSystemCertificationList = "";
                EmParameters.ProgramRequiresSo2SystemCertificationList = "";
                EmParameters.ProgramUsesRueList = "";

                foreach (ProgramCodeRow programCodeRow in EmParameters.ProgramCodeTable)
                {
                    if (programCodeRow.OsInd == 1)
                        EmParameters.ProgramIsOzoneSeasonList = EmParameters.ProgramIsOzoneSeasonList.ListAdd(programCodeRow.PrgCd);

                    if (programCodeRow.NoxCertInd == 1)
                        EmParameters.ProgramRequiresNoxSystemCertificationList = EmParameters.ProgramRequiresNoxSystemCertificationList.ListAdd(programCodeRow.PrgCd);

                    if (programCodeRow.NoxcCertInd == 1)
                        EmParameters.ProgramRequiresNoxcSystemCertificationList = EmParameters.ProgramRequiresNoxcSystemCertificationList.ListAdd(programCodeRow.PrgCd);

                    if (programCodeRow.So2CertInd == 1)
                        EmParameters.ProgramRequiresSo2SystemCertificationList = EmParameters.ProgramRequiresSo2SystemCertificationList.ListAdd(programCodeRow.PrgCd);

                    if (programCodeRow.RueInd == 1)
                        EmParameters.ProgramUsesRueList = EmParameters.ProgramUsesRueList.ListAdd(programCodeRow.PrgCd);
                }
            }
            catch (Exception ex)
            {
                returnVal = category.CheckEngine.FormatError(ex);
            }

            return returnVal;
        }

        /// <summary>
        /// This check stores values that will not change during the evaluation session, but are not readily accessible.
        /// </summary>
        /// <param name="category"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public string HOURGEN24(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                HourlyEmissionsTolerancesRow hourlyEmissionsTolerancesRow = EmParameters.HourlyEmissionsTolerancesCrossCheckTable.FindRow(new cFilterCondition("Parameter", "LOAD"), 
                                                                                                                                          new cFilterCondition("UOM", "MW"));
                {
                    if (hourlyEmissionsTolerancesRow != null)
                    {
                        try
                        {
                            EmParameters.MwLoadHourlyTolerance = Int32.Parse(hourlyEmissionsTolerancesRow.Tolerance);
                        }
                        catch
                        {
                            EmParameters.MwLoadHourlyTolerance = null;
                        }
                    }
                }

                string[] locationNameArray = new string[EmParameters.MonitoringPlanLocationRecords.Count];
                {
                    for (int dex = 0; dex < locationNameArray.Length; dex++)
                        locationNameArray[dex] = EmParameters.MonitoringPlanLocationRecords[dex].LocationName;

                    category.SetCheckParameter("Location_Name_Array", locationNameArray, eParameterDataType.String, false, true);
                }

            }
            catch (Exception ex)
            {
                returnVal = category.CheckEngine.FormatError(ex);
            }

            return returnVal;
        }

        /// <summary>
        /// HOURGEN-25
        /// 
        /// Produces result A if InvalidCylinderIdList contains cylinder ids, 
        /// and returns the ids in InvalidCylinderIdList as a formal list.
        /// </summary>
        /// <param name="category">The category in which the check is running.</param>
        /// <param name="log">Obsolete.</param>
        /// <returns></returns>
        public string HOURGEN25(cCategory category, ref bool log)
        {

            string returnVal = ""; ;

            try
            {
                EmParameters.FormattedCylinderIdList = "";

                if ((EmParameters.InvalidCylinderIdList != null) && (EmParameters.InvalidCylinderIdList.Count > 0))
                {
                    EmParameters.InvalidCylinderIdList.Sort();
                    EmParameters.FormattedCylinderIdList = EmParameters.InvalidCylinderIdList.DelimitedList(",").FormatList();
                    category.CheckCatalogResult = "A";
                }
            }
            catch (Exception ex)
            {
                returnVal = category.CheckEngine.FormatError(ex);
            }

            return returnVal;
        }

        /// <summary>
        /// HOURGEN-26
        /// 
        /// Initializes the storage for  QA Certification Event Supplemental Data.
        /// </summary>
        /// <param name="category">The category in which the check is running.</param>
        /// <param name="log">Obsolete.</param>
        /// <returns></returns>
        public string HOURGEN26(cCategory category, ref bool log)
        {

            string returnVal = ""; ;

            try
            {
                int currentReportingPeriod = (ushort)EmParameters.CurrentReportingPeriod.Value;
                int currentReportingPeriodQuarter = EmParameters.CurrentReportingPeriodQuarter.Value;
                int currentReportingPeriodYear = EmParameters.CurrentReportingPeriodYear.Value;
                DateTime currentReportingPeriodBeginDate = EmParameters.CurrentReportingPeriodBeginDate.Value;
                DateTime currentReportingPeriodEndDate = EmParameters.CurrentReportingPeriodEndDate.Value;
                CheckDataView<VwMpMonitorLocationRow> monitoringPlanLocationRecords = EmParameters.MonitoringPlanLocationRecords;

                int locationCount = monitoringPlanLocationRecords.Count;

                Dictionary<string, QaCertificationSupplementalData> qaCertEventSuppDataDictionary;
                VwMpMonitorLocationRow monitoringPlanLocationRecord;
                CheckDataView<VwQaCertEventRow> qaCertEventRecords;

                EmParameters.QaCertEventSuppDataDictionaryArray = new Dictionary<string, QaCertificationSupplementalData>[locationCount];

                Dictionary<string, List<QaCertificationSupplementalData>> qaCertEventSuppDataDictionaryBySystem = new Dictionary<string, List<QaCertificationSupplementalData>>();
                {
                    EmParameters.QaCertEventSuppDataDictionaryBySystem = qaCertEventSuppDataDictionaryBySystem;
                }

                Dictionary<string, List<QaCertificationSupplementalData>> qaCertEventSuppDataDictionaryByComponent = new Dictionary<string, List<QaCertificationSupplementalData>>();
                {
                    EmParameters.QaCertEventSuppDataDictionaryByComponent = qaCertEventSuppDataDictionaryByComponent;
                }

                for (int locationPosition = 0; locationPosition < locationCount; locationPosition++)
                {
                    monitoringPlanLocationRecord = monitoringPlanLocationRecords[locationPosition];

                    qaCertEventSuppDataDictionary = new Dictionary<string, QaCertificationSupplementalData>();
                    EmParameters.QaCertEventSuppDataDictionaryArray[locationPosition] = qaCertEventSuppDataDictionary;

                    qaCertEventRecords
                        = EmParameters.QaCertEventsForEmEvaluation.FindRows(
                                                                                new cFilterCondition[]
                                                                                {
                                                                                    new cFilterCondition("MON_LOC_ID", monitoringPlanLocationRecord.MonLocId),
                                                                                    new cFilterCondition("QA_CERT_EVENT_DATE", eFilterConditionRelativeCompare.GreaterThanOrEqual, currentReportingPeriodBeginDate, eNullDateDefault.Max),
                                                                                    new cFilterCondition("QA_CERT_EVENT_DATE", eFilterConditionRelativeCompare.LessThanOrEqual, currentReportingPeriodEndDate, eNullDateDefault.Max)
                                                                                },
                                                                                new cFilterCondition[]
                                                                                {
                                                                                    new cFilterCondition("MON_LOC_ID", monitoringPlanLocationRecord.MonLocId),
                                                                                    new cFilterCondition("CONDITIONAL_DATA_BEGIN_DATE", eFilterConditionRelativeCompare.GreaterThanOrEqual, currentReportingPeriodBeginDate, eNullDateDefault.Max),
                                                                                    new cFilterCondition("CONDITIONAL_DATA_BEGIN_DATE", eFilterConditionRelativeCompare.LessThanOrEqual, currentReportingPeriodEndDate, eNullDateDefault.Max)
                                                                                }
                                                                           );
                    {
                        foreach (VwQaCertEventRow qaCertEventRow in qaCertEventRecords)
                        {
                            HOURGEN26_Helper(eQaCertificationSupplementalDataTargetDateHour.QaCertEventDate, qaCertEventRow.QaCertEventDatehour, 
                                             qaCertEventRow,
                                             currentReportingPeriod, currentReportingPeriodYear, currentReportingPeriodQuarter,
                                             qaCertEventSuppDataDictionary, qaCertEventSuppDataDictionaryBySystem, qaCertEventSuppDataDictionaryByComponent);

                            HOURGEN26_Helper(eQaCertificationSupplementalDataTargetDateHour.ConditionalDataBeginHour, qaCertEventRow.ConditionalDataBeginDatehour, 
                                             qaCertEventRow,
                                             currentReportingPeriod, currentReportingPeriodYear, currentReportingPeriodQuarter,
                                             qaCertEventSuppDataDictionary, qaCertEventSuppDataDictionaryBySystem, qaCertEventSuppDataDictionaryByComponent);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                returnVal = category.CheckEngine.FormatError(ex);
            }

            return returnVal;
        }

        /// <summary>
        /// Performs the HOURGEN-26 initialization for a specific QA Cert Event date/hour.
        /// </summary>
        /// <param name="targetDatetimeCode"></param>
        /// <param name="targetDatetimeValue"></param>
        /// <param name="qaCertEventRow"></param>
        /// <param name="currentReportingPeriod"></param>
        /// <param name="currentReportingPeriodYear"></param>
        /// <param name="currentReportingPeriodQuarter"></param>
        /// <param name="qaCertEventSuppDataDictionary"></param>
        /// <param name="qaCertEventSuppDataDictionaryBySystem"></param>
        /// <param name="qaCertEventSuppDataDictionaryByComponent"></param>
        public void HOURGEN26_Helper(eQaCertificationSupplementalDataTargetDateHour targetDatetimeCode, DateTime? targetDatetimeValue, VwQaCertEventRow qaCertEventRow,
                                     int currentReportingPeriod, int currentReportingPeriodYear, int currentReportingPeriodQuarter,
                                     Dictionary<string, QaCertificationSupplementalData> qaCertEventSuppDataDictionary,
                                     Dictionary<string, List<QaCertificationSupplementalData>> qaCertEventSuppDataDictionaryBySystem,
                                     Dictionary<string, List<QaCertificationSupplementalData>> qaCertEventSuppDataDictionaryByComponent)
        {

            // Add supplemental data object for ConditionalDataBeginHour if it is not null and is in the quarter.
            if (targetDatetimeValue.HasValue &&
                (targetDatetimeValue.Value.Year == currentReportingPeriodYear) &&
                (targetDatetimeValue.Value.Quarter() == currentReportingPeriodQuarter))
            {
                QaCertificationSupplementalData qaCertificationSupplementalData
                    = new QaCertificationSupplementalData(currentReportingPeriod,
                                                          qaCertEventRow.MonLocId,
                                                          qaCertEventRow.QaCertEventId,
                                                          targetDatetimeCode,
                                                          targetDatetimeValue.Value);

                string dictionaryKey = QaCertificationSupplementalData.FormatKey(qaCertEventRow.QaCertEventId, targetDatetimeCode);

                if (!qaCertEventSuppDataDictionary.ContainsKey(dictionaryKey))
                    qaCertEventSuppDataDictionary.Add(dictionaryKey, qaCertificationSupplementalData);

                if (qaCertEventRow.MonSysId != null)
                {
                    if (!qaCertEventSuppDataDictionaryBySystem.ContainsKey(qaCertEventRow.MonSysId))
                        qaCertEventSuppDataDictionaryBySystem[qaCertEventRow.MonSysId] = new List<QaCertificationSupplementalData>();

                    qaCertEventSuppDataDictionaryBySystem[qaCertEventRow.MonSysId].Add(qaCertificationSupplementalData);
                }

                if (qaCertEventRow.ComponentId != null)
                {
                    if (!qaCertEventSuppDataDictionaryByComponent.ContainsKey(qaCertEventRow.ComponentId))
                        qaCertEventSuppDataDictionaryByComponent[qaCertEventRow.ComponentId] = new List<QaCertificationSupplementalData>();

                    qaCertEventSuppDataDictionaryByComponent[qaCertEventRow.ComponentId].Add(qaCertificationSupplementalData);
                }
            }
        }

        /// <summary>
        /// HOURGEN-27
        /// 
        /// Initializes the storage for  System Operating Supplemental Data.
        /// </summary>
        /// <param name="category">The category in which the check is running.</param>
        /// <param name="log">Obsolete.</param>
        /// <returns></returns>
        public string HOURGEN27(cCategory category, ref bool log)
        {

            string returnVal = ""; ;

            try
            {
                int currentReportingPeriod = (ushort)EmParameters.CurrentReportingPeriod.Value;
                DateTime currentReportingPeriodBeginDate = EmParameters.CurrentReportingPeriodBeginDate.Value;
                DateTime currentReportingPeriodEndDate = EmParameters.CurrentReportingPeriodEndDate.Value;
                CheckDataView<VwMpMonitorLocationRow> monitoringPlanLocationRecords = EmParameters.MonitoringPlanLocationRecords;

                int locationCount = monitoringPlanLocationRecords.Count;

                Dictionary<string, SystemOperatingSupplementalData> systemOperatingSuppDataDictionary;
                VwMpMonitorLocationRow monitoringPlanLocationRecord;
                CheckDataView<VwMpMonitorSystemRow> monitorSystemRecords;

                EmParameters.SystemOperatingSuppDataDictionaryArray = new Dictionary<string, SystemOperatingSupplementalData>[locationCount];

                for (int locationPosition = 0; locationPosition < locationCount; locationPosition++)
                {
                    monitoringPlanLocationRecord = monitoringPlanLocationRecords[locationPosition];

                    systemOperatingSuppDataDictionary = new Dictionary<string, SystemOperatingSupplementalData>();
                    EmParameters.SystemOperatingSuppDataDictionaryArray[locationPosition] = systemOperatingSuppDataDictionary;

                    monitorSystemRecords
                        = EmParameters.MonitorSystemsForEmEvaluation.FindRows(
                                                                                new cFilterCondition[]
                                                                                {
                                                                                    new cFilterCondition("MON_LOC_ID", monitoringPlanLocationRecord.MonLocId),
                                                                                    new cFilterCondition("SYS_TYPE_CD", "CO2,FLOW,GAS,H2O,H2OM,H2OT,HCL,HF,HG,NOX,NOXC,NOXE,NOXP,O2,SO2,ST,OILM,OILV", eFilterConditionStringCompare.InList),
                                                                                    new cFilterCondition("BEGIN_DATE", eFilterConditionRelativeCompare.LessThanOrEqual, currentReportingPeriodEndDate, eNullDateDefault.Min),
                                                                                    new cFilterCondition("END_DATE", eFilterConditionRelativeCompare.GreaterThanOrEqual, currentReportingPeriodBeginDate, eNullDateDefault.Max)
                                                                                }
                                                                             );
                    {
                        foreach (VwMpMonitorSystemRow monitorSystemRow in monitorSystemRecords)
                        {
                            bool skipSavingModcCounts = monitorSystemRow.SysTypeCd.InList("GAS,NOXE,OILM,OILV");

                            if (!systemOperatingSuppDataDictionary.ContainsKey(monitorSystemRow.MonSysId))
                                systemOperatingSuppDataDictionary.Add(monitorSystemRow.MonSysId, new SystemOperatingSupplementalData(currentReportingPeriod, monitorSystemRow.MonSysId, monitorSystemRow.MonLocId, skipSavingModcCounts));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                returnVal = category.CheckEngine.FormatError(ex);
            }

            return returnVal;
        }

        /// <summary>
        /// HOURGEN-28
        /// 
        /// Initializes the storage for  Component Operating Supplemental Data.
        /// </summary>
        /// <param name="category">The category in which the check is running.</param>
        /// <param name="log">Obsolete.</param>
        /// <returns></returns>
        public string HOURGEN28(cCategory category, ref bool log)
        {

            string returnVal = ""; ;

            try
            {
                int currentReportingPeriod = (ushort)EmParameters.CurrentReportingPeriod.Value;
                DateTime currentReportingPeriodBeginDate = EmParameters.CurrentReportingPeriodBeginDate.Value;
                DateTime currentReportingPeriodEndDate = EmParameters.CurrentReportingPeriodEndDate.Value;
                CheckDataView<VwMpMonitorLocationRow> monitoringPlanLocationRecords = EmParameters.MonitoringPlanLocationRecords;

                int locationCount = monitoringPlanLocationRecords.Count;

                Dictionary<string, ComponentOperatingSupplementalData> componentOperatingSuppDataDictionary;
                VwMpMonitorLocationRow monitoringPlanLocationRecord;
                CheckDataView<VwMpMonitorSystemComponentRow> monitorSystemComponentRecords;

                EmParameters.ComponentOperatingSuppDataDictionaryArray = new Dictionary<string, ComponentOperatingSupplementalData>[locationCount];

                for (int locationPosition = 0; locationPosition < locationCount; locationPosition++)
                {
                    monitoringPlanLocationRecord = monitoringPlanLocationRecords[locationPosition];

                    componentOperatingSuppDataDictionary = new Dictionary<string, ComponentOperatingSupplementalData>();
                    EmParameters.ComponentOperatingSuppDataDictionaryArray[locationPosition] = componentOperatingSuppDataDictionary;

                    monitorSystemComponentRecords
                        = EmParameters.MonitorSystemComponentsForEmEvaluation.FindRows(
                                                                                        new cFilterCondition[]
                                                                                        {
                                                                                            new cFilterCondition("MON_LOC_ID", monitoringPlanLocationRecord.MonLocId),
                                                                                            new cFilterCondition("COMPONENT_TYPE_CD", "CO2,FLOW,H2O,HCL,HF,HG,NOX,O2,SO2", eFilterConditionStringCompare.InList),
                                                                                            new cFilterCondition("BEGIN_DATE", eFilterConditionRelativeCompare.LessThanOrEqual, currentReportingPeriodEndDate, eNullDateDefault.Min),
                                                                                            new cFilterCondition("END_DATE", eFilterConditionRelativeCompare.GreaterThanOrEqual, currentReportingPeriodBeginDate, eNullDateDefault.Max)
                                                                                        }
                                                                                      );
                    {
                        foreach (VwMpMonitorSystemComponentRow monitorSystemComponentRow in monitorSystemComponentRecords)
                            if (!componentOperatingSuppDataDictionary.ContainsKey(monitorSystemComponentRow.ComponentId))
                                componentOperatingSuppDataDictionary.Add(monitorSystemComponentRow.ComponentId, new ComponentOperatingSupplementalData(currentReportingPeriod, monitorSystemComponentRow.ComponentId, monitorSystemComponentRow.MonLocId));
                    }
                }
            }
            catch (Exception ex)
            {
                returnVal = category.CheckEngine.FormatError(ex);
            }

            return returnVal;
        }

        /// <summary>
        /// Initializes the storage for Last Quality Assured Value Supplemental Data.
        /// </summary>
        /// <param name="category">The category in which the check is running.</param>
        /// <param name="log">Obsolete.</param>
        /// <returns></returns>
        public string HOURGEN29(cCategory category, ref bool log)
        {

            string returnVal = ""; ;

            try
            {
                int locationCount = EmParameters.MonitoringPlanLocationRecords.Count;


                EmParameters.LastQualityAssuredValueSuppDataDictionaryArray = new Dictionary<string, LastQualityAssuredValueSupplementalData>[locationCount];


                for (int locationPosition = 0; locationPosition < locationCount; locationPosition++)
                {
                    EmParameters.LastQualityAssuredValueSuppDataDictionaryArray[locationPosition] = new Dictionary<string, LastQualityAssuredValueSupplementalData>();
                }
            }
            catch (Exception ex)
            {
                returnVal = category.CheckEngine.FormatError(ex);
            }

            return returnVal;
        }

        /// <summary>
        /// Initializes check parameters used to indicate whether processing primary bypass stacks 
        /// implemented as systems should occur, and initializes information needed by that processing.
        /// </summary>
        /// <param name="category">The category in which the check is running.</param>
        /// <param name="log">Obsolete.</param>
        /// <returns></returns>
        public string HOURGEN30(cCategory category, ref bool log)
        {

            string returnVal = ""; ;

            try
            {
                EmParameters.PrimaryBypassActiveInQuarter = false;

                int recordCount = EmParameters.MonitorSystemsForEmEvaluation.CountRows(new cFilterCondition[] 
                                                                                       {
                                                                                           new cFilterCondition("SYS_DESIGNATION_CD", "PB"),
                                                                                           new cFilterCondition("BEGIN_DATE", eFilterConditionRelativeCompare.LessThanOrEqual, EmParameters.CurrentReportingPeriodEndDate.Value, eNullDateDefault.Min),
                                                                                           new cFilterCondition("END_DATE", eFilterConditionRelativeCompare.GreaterThanOrEqual, EmParameters.CurrentReportingPeriodBeginDate.Value, eNullDateDefault.Max)
                                                                                       });

                if (recordCount > 0)
                {
                    EmParameters.PrimaryBypassActiveInQuarter = true;
                }
            }
            catch (Exception ex)
            {
                returnVal = category.CheckEngine.FormatError(ex);
            }

            return returnVal;
        }

        #endregion

    }

}
