using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Data.Ecmps.CheckEm.Function;
using ECMPS.Checks.Em.Parameters;
using ECMPS.Checks.EmissionsReport;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Extensions;


namespace ECMPS.Checks.EmissionsChecks
{
    public class cHourlyAggregationChecks : cEmissionsChecks
    {

        #region Constructors

        public cHourlyAggregationChecks(cEmissionsReportProcess emissionReportProcess) : base(emissionReportProcess)
        {
            CheckProcedures = new dCheckProcedure[19];

            CheckProcedures[1] = new dCheckProcedure(HOURAGG1);
            CheckProcedures[2] = new dCheckProcedure(HOURAGG2);
            CheckProcedures[3] = new dCheckProcedure(HOURAGG3);
            CheckProcedures[4] = new dCheckProcedure(HOURAGG4);
            CheckProcedures[5] = new dCheckProcedure(HOURAGG5);
            CheckProcedures[6] = new dCheckProcedure(HOURAGG6);
            CheckProcedures[7] = new dCheckProcedure(HOURAGG7);
            CheckProcedures[8] = new dCheckProcedure(HOURAGG8);
            CheckProcedures[10] = new dCheckProcedure(HOURAGG10);

            CheckProcedures[11] = new dCheckProcedure(HOURAGG11);
            CheckProcedures[12] = new dCheckProcedure(HOURAGG12);
            CheckProcedures[13] = new dCheckProcedure(HOURAGG13);
            CheckProcedures[14] = new dCheckProcedure(HOURAGG14);
            CheckProcedures[15] = new dCheckProcedure(HOURAGG15);
            CheckProcedures[16] = new dCheckProcedure(HOURAGG16);
            CheckProcedures[17] = new dCheckProcedure(HOURAGG17);
            CheckProcedures[18] = new dCheckProcedure(HOURAGG18);
        }

        #endregion


        #region Public Static Methods: Checks

        #region Checks 1 - 10

        public static string HOURAGG1(cCategory category, ref bool log)
        //Compare SO2 Mass Accumulator Values 
        {
            string returnVal = "";

            try
            {
                category.SetCheckParameter("Start_Quarter", null, eParameterDataType.Integer);
                category.SetCheckParameter("SO2_Start_Quarter", null, eParameterDataType.Integer);
                category.SetCheckParameter("NOXR_Start_Quarter", null, eParameterDataType.Integer);
                category.SetCheckParameter("CO2_Start_Quarter", null, eParameterDataType.Integer);
                category.SetCheckParameter("Heat_Input_Start_Quarter", null, eParameterDataType.Integer);
                category.SetCheckParameter("NOX_Start_Quarter", null, eParameterDataType.Integer);
                category.SetCheckParameter("Emissions_Tolerance_Deviators", null, eParameterDataType.String);

                bool annualReportingRequirement = category.GetCheckParameter("Annual_Reporting_Requirement").ValueAsBool();

                cReportingPeriod currentReportingPeriodObject = (cReportingPeriod)category.GetCheckParameter("Current_Reporting_Period_Object").ParameterValue;

                DataView methodRecords = category.GetCheckParameter("Method_Records").ValueAsDataView();

                DateTime FirstDayofYear = new DateTime(currentReportingPeriodObject.Year, 1, 1);

                DataView methodYtdView
                  = cRowFilter.FindActiveRows(methodRecords, FirstDayofYear, currentReportingPeriodObject.EndedDate, "BEGIN_DATE", "END_DATE", true, true);
                //new cFilterCondition[] 
                //  { new cFilterCondition("BEGIN_DATE", 
                //                         currentReportingPeriodObject.EndedDate, 
                //                         eFilterDataType.DateBegan,  
                //                         eFilterConditionRelativeCompare.LessThanOrEqual)});

                if (currentReportingPeriodObject.Quarter > eQuarter.q1)
                    // Handle General Start Quarter
                    HourAgg1_SetStartQuarter("Start_Quarter", methodYtdView, "BEGIN_DATE", annualReportingRequirement, currentReportingPeriodObject, category);

                // I am applying the EXP filter to all methods, 
                // even though the spec just says to apply this to Heat Input.
                // This is deliberate.  Currently only HI has an EXP method,
                // but if other parameters had EXP methods (i.e., exempt), we would want to apply this filter to them too.
                sFilterPair[] FilterDef = new sFilterPair[1];
                FilterDef[0].Set("METHOD_CD", "EXP", true);
                DataView methodRecords2 = FindRows(methodRecords, FilterDef);
                methodYtdView
                  = cRowFilter.FindActiveRows(methodRecords2, FirstDayofYear, currentReportingPeriodObject.EndedDate, "BEGIN_DATE", "END_DATE", true, true);

                if (currentReportingPeriodObject.Quarter > eQuarter.q1)
                {
                    // Handle SO2 Start Quarter
                    {
                        DataView methodSo2View
                          = cRowFilter.FindRows(methodYtdView,
                                                new cFilterCondition[] { new cFilterCondition("PARAMETER_CD", "SO2,SO2M",
                                                                                  eFilterConditionStringCompare.InList) });

                        HourAgg1_SetStartQuarter("SO2_Start_Quarter", methodSo2View, "BEGIN_DATE", true,
                                                 currentReportingPeriodObject, category);
                    }


                    // NOxR Start Quarter
                    {
                        // Location Method Records
                        DateTime? earliestMethodBeginDate;
                        {
                            if (category.GetCheckParameter("LME_Annual").AsBoolean(false))
                            {
                                CheckDataView<NoxrSummaryRequiredForLmeAnnual> SummaryRequiredRecords
                                    = EmParameters.NoxrSummaryRequiredForLmeAnnualRecords.FindRows(
                                                                                                    new cFilterCondition("MON_LOC_ID", EmParameters.CurrentMonitorPlanLocationRecord.MonLocId),
                                                                                                    new cFilterCondition("LME_NOXR_SUMMARY_IND", eFilterConditionRelativeCompare.Equals, 1),
                                                                                                    new cFilterCondition("LME_NOXR_BEGIN", null, true)
                                                                                                  );
                                if (SummaryRequiredRecords.Count > 0)
                                {
                                    SummaryRequiredRecords.SourceView.Sort = "QUARTER";
                                    earliestMethodBeginDate = SummaryRequiredRecords[0].LmeNoxrBegin.Value.Date;
                                }
                                else
                                {
                                    earliestMethodBeginDate = null;
                                }
                            }
                            else
                            {
                                DataRowView currentMonitorPlanLocationRecord = category.GetCheckParameter("Current_Monitor_Plan_Location_Record").AsDataRowView();

                                DataView methodNoxrNoxmSourceView;
                                string methodNoxrNoxmParameterCd;

                                if (((currentMonitorPlanLocationRecord != null) &&
                                     currentMonitorPlanLocationRecord["LOCATION_NAME"].AsString().Default().StartsWith("MS")) ||
                                    !category.GetCheckParameter("Multiple_Stack_Configuration").AsBoolean(false))
                                {
                                    methodNoxrNoxmSourceView = methodYtdView;
                                    methodNoxrNoxmParameterCd = "NOXR";
                                }
                                else
                                {
                                    methodNoxrNoxmSourceView = category.GetCheckParameter("MP_Method_Records").ValueAsDataView();
                                    methodNoxrNoxmParameterCd = "NOXR";
                                }

                                DataView methodNoxrNoxmView
                                  = cRowFilter.FindActiveRows(methodNoxrNoxmSourceView,
                                                              FirstDayofYear,
                                                              currentReportingPeriodObject.EndedDate,
                                                              new cFilterCondition[] { new cFilterCondition("PARAMETER_CD", methodNoxrNoxmParameterCd) });

                                if (methodNoxrNoxmView.Count > 0)
                                {
                                    earliestMethodBeginDate = (cRowFilter.FindEarliestRow(methodNoxrNoxmView))["BEGIN_DATE"].AsDateTime(DateTime.MinValue);
                                }
                                else
                                {
                                    earliestMethodBeginDate = null;
                                }
                            }
                        }

                        if (earliestMethodBeginDate.HasValue)
                        {
                            // Locate ARP Unit Program Records
                            DataView locationProgramView;
                            {
                                DataView locationProgramRecords = category.GetCheckParameter("Location_Program_Records").AsDataView();

                                locationProgramView
                                  = cRowFilter.FindActiveRows(locationProgramRecords,
                                                              FirstDayofYear,
                                                              currentReportingPeriodObject.EndedDate,
                                                              "UNIT_MONITOR_CERT_BEGIN_DATE",
                                                              "END_DATE",
                                                              new cFilterCondition[]
                                                              {
                                                new cFilterCondition("PRG_CD", "ARP"),
                                                new cFilterCondition("CLASS", "NA", true)
                                                              });
                            }

                            if (locationProgramView.Count > 0)
                            {
                                // Sort Unit Program records so that earliest if first
                                locationProgramView.Sort = "UNIT_MONITOR_CERT_BEGIN_DATE";

                                // Determine Later Date of method begin, UMCB and ERB
                                DateTime laterDate;
                                {
                                    DateTime methodBeginDate = earliestMethodBeginDate.Value;
                                    DateTime umcbDate = locationProgramView[0]["UNIT_MONITOR_CERT_BEGIN_DATE"].AsDateTime(DateTime.MinValue);
                                    DateTime? erbDate = locationProgramView[0]["EMISSIONS_RECORDING_BEGIN_DATE"].AsDateTime();

                                    if (erbDate.IsNull())
                                    {
                                        if (umcbDate > methodBeginDate)
                                            laterDate = umcbDate;
                                        else
                                            laterDate = methodBeginDate;
                                    }
                                    else
                                    {
                                        if (erbDate > methodBeginDate)
                                            laterDate = erbDate.Value;
                                        else
                                            laterDate = methodBeginDate;
                                    }
                                }

                                // Set NOXR Start Quarter
                                if (laterDate.Year < currentReportingPeriodObject.Year)
                                    category.SetCheckParameter("NOXR_Start_Quarter", 1, eParameterDataType.Integer);
                                else
                                    category.SetCheckParameter("NOXR_Start_Quarter", laterDate.Quarter(), eParameterDataType.Integer);
                            }
                        }
                    }

                    // CO2 Start Quarter
                    {
                        DataView methodCo2View
                          = cRowFilter.FindRows(methodYtdView,
                                                new cFilterCondition[] { new cFilterCondition("PARAMETER_CD", "CO2,CO2M",
                                                                                  eFilterConditionStringCompare.InList) });

                        HourAgg1_SetStartQuarter("CO2_Start_Quarter", methodCo2View, "BEGIN_DATE", true,
                                                 currentReportingPeriodObject, category);
                    }
                }

                if ((currentReportingPeriodObject.Quarter > eQuarter.q2) ||
                    (annualReportingRequirement &&
                     (currentReportingPeriodObject.Quarter == eQuarter.q2)))
                {
                    // HI Start Quarter
                    {
                        DataView methodHiView
                          = cRowFilter.FindRows(methodYtdView,
                                                new cFilterCondition[] { new cFilterCondition("PARAMETER_CD", "HI,HIT",
                                                                                  eFilterConditionStringCompare.InList) });

                        HourAgg1_SetStartQuarter("Heat_Input_Start_Quarter", methodHiView, "BEGIN_DATE",
                                                 annualReportingRequirement,
                                                 currentReportingPeriodObject, category);
                    }

                    // NOx Start Quarter
                    {
                        DataView methodNoxNoxmView
                          = cRowFilter.FindRows(methodYtdView,
                                                new cFilterCondition[] { new cFilterCondition("PARAMETER_CD", "NOX,NOXM",
                                                                                  eFilterConditionStringCompare.InList) });

                        HourAgg1_SetStartQuarter("NOX_Start_Quarter", methodNoxNoxmView, "BEGIN_DATE",
                                                 annualReportingRequirement,
                                                 currentReportingPeriodObject, category);
                    }
                }
            }
            catch (Exception ex)
            {
                returnVal = category.CheckEngine.FormatError(ex, "HOURAGG1");
            }

            return returnVal;
        }

        public static string HOURAGG2(cCategory Category, ref bool Log)
        //Compare SO2 Mass Accumulator Values 
        {
            string ReturnVal = "";

            try
            {
                decimal[] CalculatedArray = Category.GetCheckParameter("Rpt_Period_So2_Mass_Calculated_Accumulator_Array").ValueAsDecimalArray();
                decimal[] ReportedArray = Category.GetCheckParameter("Rpt_Period_So2_Mass_Reported_Accumulator_Array").ValueAsDecimalArray();
                int CurrentPosition = Category.GetCheckParameter("Current_Monitor_Plan_Location_Postion").ValueAsInt();
                bool[] ExpectedSummaryValueArray = (bool[])Category.GetCheckParameter("Expected_Summary_Value_So2_Array").ParameterValue;

                if (CalculatedArray[CurrentPosition] == -1 || !ExpectedSummaryValueArray[CurrentPosition])
                    Category.SetCheckParameter("Rpt_Period_SO2_Mass_Calculated_Value", null, eParameterDataType.Decimal);
                else
                    Category.SetCheckParameter("Rpt_Period_SO2_Mass_Calculated_Value", Math.Round(CalculatedArray[CurrentPosition] / 2000, 1, MidpointRounding.AwayFromZero), eParameterDataType.Decimal);

                if (ReportedArray[CurrentPosition] >= 0)
                    Category.SetArrayParameter("Rpt_Period_So2_Mass_Reported_Accumulator_Array", CurrentPosition, Math.Round(ReportedArray[CurrentPosition] / 2000, 1, MidpointRounding.AwayFromZero));
                int[] PeriodArray = Category.GetCheckParameter("Rpt_Period_Op_Hours_Accumulator_Array").ValueAsIntArray();
                bool OpHoursEqualZero = cDBConvert.ToInteger(PeriodArray[CurrentPosition]) == 0;

                CompareAccumulatorValues1("Current_SO2_Summary_Value_Record",
                                         "Expected_Summary_Value_So2_Array",
                                         "Rpt_Period_So2_Mass_Calculated_Value",
                                         "Rpt_Period_So2_Mass_Reported_Accumulator_Array", OpHoursEqualZero,
                                         "SO2M", "TON",
                                         Category);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURAGG2");
            }

            return ReturnVal;
        }

        public static string HOURAGG3(cCategory Category, ref bool Log)
        //Compare CO2 Mass Accumulator Values 
        {
            string ReturnVal = "";

            try
            {
                string toleranceParameterCd = ((Category.CheckEngine.ReportingPeriod.Year >= 2012) ? "CO2M" : "CO2M-OLD"); ;

                decimal[] CalculatedArray = Category.GetCheckParameter("Rpt_Period_Co2_Mass_Calculated_Accumulator_Array").ValueAsDecimalArray();
                int CurrentPosition = Category.GetCheckParameter("Current_Monitor_Plan_Location_Postion").ValueAsInt();
                bool[] ExpectedSummaryValueArray = (bool[])Category.GetCheckParameter("Expected_Summary_Value_CO2_Array").ParameterValue;

                if (CalculatedArray[CurrentPosition] < 0 || !ExpectedSummaryValueArray[CurrentPosition])
                    Category.SetCheckParameter("Rpt_Period_CO2_Mass_Calculated_Value", null, eParameterDataType.Decimal);
                else
                    Category.SetCheckParameter("Rpt_Period_CO2_Mass_Calculated_Value", Math.Round(CalculatedArray[CurrentPosition], 1, MidpointRounding.AwayFromZero), eParameterDataType.Decimal);

                int[] PeriodArray = Category.GetCheckParameter("Rpt_Period_Op_Hours_Accumulator_Array").ValueAsIntArray();
                bool OpHoursEqualZero = cDBConvert.ToInteger(PeriodArray[CurrentPosition]) == 0;

                CompareAccumulatorValues2("Current_CO2_Summary_Value_Record",
                                         "Expected_Summary_Value_CO2_Array",
                                         "Rpt_Period_CO2_Mass_Calculated_Value",
                                         "Rpt_Period_CO2_Mass_Reported_Accumulator_Array",
                                         "Rpt_Period_Co2_Mass_Calculated_Accumulator_Array", OpHoursEqualZero,
                                         "CO2M", toleranceParameterCd, "TON",
                                         Category);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURAGG3");
            }

            return ReturnVal;
        }

        public static string HOURAGG4(cCategory Category, ref bool Log)
        //Compare HI Accumulator Values 
        {
            string ReturnVal = "";

            try
            {
                decimal[] CalculatedArray = Category.GetCheckParameter("Rpt_Period_HI_Calculated_Accumulator_Array").ValueAsDecimalArray();
                int CurrentPosition = Category.GetCheckParameter("Current_Monitor_Plan_Location_Postion").ValueAsInt();
                bool[] ExpectedSummaryValueArray = (bool[])Category.GetCheckParameter("Expected_Summary_Value_HI_Array").ParameterValue;

                if (CalculatedArray[CurrentPosition] == -1 || !ExpectedSummaryValueArray[CurrentPosition])
                    Category.SetCheckParameter("Rpt_Period_HI_Calculated_Value", null, eParameterDataType.Decimal);
                else
                    Category.SetCheckParameter("Rpt_Period_HI_Calculated_Value", Math.Round(CalculatedArray[CurrentPosition], MidpointRounding.AwayFromZero), eParameterDataType.Decimal);
                int[] PeriodArray = Category.GetCheckParameter("Rpt_Period_Op_Hours_Accumulator_Array").ValueAsIntArray();
                bool OpHoursEqualZero = cDBConvert.ToInteger(PeriodArray[CurrentPosition]) == 0;
                CompareAccumulatorValues3("Current_HI_Summary_Value_Record",
                                         "Expected_Summary_Value_HI_Array",
                                         "Rpt_Period_HI_Calculated_Value",
                                         "Rpt_Period_HI_Reported_Accumulator_Array", OpHoursEqualZero,
                                         "HIT", "MMBTU",
                                         0,
                                         Category);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURAGG4");
            }

            return ReturnVal;
        }

        public static string HOURAGG5(cCategory Category, ref bool Log)
        //Compare Op Hours Values
        {
            string ReturnVal = "";

            try
            {
                int[] OpHrsAccumArray = Category.GetCheckParameter("Rpt_Period_Op_Hours_Accumulator_Array").ValueAsIntArray();
                int[] OpDaysAccumArray = Category.GetCheckParameter("Rpt_Period_Op_Days_Accumulator_Array").ValueAsIntArray();

                DataRowView CurrentMonitorPlanLocationRecord = Category.GetCheckParameter("Current_Monitor_Plan_Location_Record").ValueAsDataRowView();
                int CurrentPosition = Category.GetCheckParameter("Current_Monitor_Plan_Location_Postion").ValueAsInt();
                if (OpHrsAccumArray[CurrentPosition] == -1 ||
                     (Category.GetCheckParameter("LME_HI_Method").ParameterValue != null &&
                     cDBConvert.ToString(CurrentMonitorPlanLocationRecord["LOCATION_NAME"]).StartsWith("CP")))
                {
                    Category.SetCheckParameter("Rpt_Period_Op_Hours_Calculated_Value", null, eParameterDataType.Integer);
                    Category.SetCheckParameter("Rpt_Period_Op_Days_Calculated_Value", null, eParameterDataType.Integer);
                }
                else
                {
                    Category.SetCheckParameter("Rpt_Period_Op_Hours_Calculated_Value", OpHrsAccumArray[CurrentPosition], eParameterDataType.Integer);
                    Category.SetCheckParameter("Rpt_Period_Op_Days_Calculated_Value", OpDaysAccumArray[CurrentPosition], eParameterDataType.Integer);
                }

                DataView SummaryValueRecords = Category.GetCheckParameter("Summary_Value_Records_By_Reporting_Period_Location").ValueAsDataView();
                sFilterPair[] RowFilter = new sFilterPair[1];
                RowFilter[0].Set("PARAMETER_CD", "OPHOURS");

                DataRowView SummaryValueRow;

                if (!FindRow(SummaryValueRecords, RowFilter, out SummaryValueRow) || SummaryValueRow["CURRENT_RPT_PERIOD_TOTAL"] == DBNull.Value)
                {
                    Category.SetCheckParameter("Current_Op_Hours_Summary_Value_Record", SummaryValueRow, eParameterDataType.DataRowView);
                    if (Category.GetCheckParameter("LME_HI_Method").ParameterValue == null || !cDBConvert.ToString(CurrentMonitorPlanLocationRecord["LOCATION_NAME"]).StartsWith("CP"))
                        Category.CheckCatalogResult = "B";
                }
                else
                {
                    Category.SetCheckParameter("Current_Op_Hours_Summary_Value_Record", SummaryValueRow, eParameterDataType.DataRowView);

                    decimal SummaryValue = cDBConvert.ToDecimal(SummaryValueRow["CURRENT_RPT_PERIOD_TOTAL"]);
                    if (SummaryValue < 0)
                        Category.CheckCatalogResult = "D";
                    else
                      if (SummaryValue != Math.Round(SummaryValue, 0, MidpointRounding.AwayFromZero) && SummaryValue != decimal.MinValue)//check for null redundant but included to uncouple this line from previous line)
                        Category.CheckCatalogResult = "E";
                    else
                        if (OpHrsAccumArray[CurrentPosition] != -1)
                    {
                        decimal QuarterlyTolerance = GetQuarterlyTolerance("OPHOURS", "HR", Category);
                        if (OpHrsAccumArray[CurrentPosition] != SummaryValue)
                        {
                            if (Math.Abs(OpHrsAccumArray[CurrentPosition] - SummaryValue) > QuarterlyTolerance)
                                Category.CheckCatalogResult = "A";
                            else
                                Category.SetCheckParameter("Emissions_Tolerance_Deviators",
                                  Category.GetCheckParameter("Emissions_Tolerance_Deviators").ValueAsString().ListAdd("OPHOURS"),
                                  eParameterDataType.String);
                        }
                    }
                    else
                        Category.CheckCatalogResult = "C";
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURAGG5");
            }

            return ReturnVal;
        }

        public static string HOURAGG6(cCategory Category, ref bool Log)
        //Compare Op Time Values
        {
            string ReturnVal = "";

            try
            {
                decimal[] OpTimeAccumArray = Category.GetCheckParameter("Rpt_Period_Op_Time_Accumulator_Array").ValueAsDecimalArray();
                DataRowView CurrentMonitorPlanLocationRecord = Category.GetCheckParameter("Current_Monitor_Plan_Location_Record").ValueAsDataRowView();
                int CurrentPosition = Category.GetCheckParameter("Current_Monitor_Plan_Location_Postion").ValueAsInt();
                if (OpTimeAccumArray[CurrentPosition] == -1 ||
                     (Category.GetCheckParameter("LME_HI_Method").ParameterValue != null &&
                     cDBConvert.ToString(CurrentMonitorPlanLocationRecord["LOCATION_NAME"]).StartsWith("CP")))
                    Category.SetCheckParameter("Rpt_Period_Op_Time_Calculated_Value", null, eParameterDataType.Decimal);
                else
                    Category.SetCheckParameter("Rpt_Period_Op_Time_Calculated_Value", OpTimeAccumArray[CurrentPosition], eParameterDataType.Decimal);

                DataView SummaryValueRecords = Category.GetCheckParameter("Summary_Value_Records_By_Reporting_Period_Location").ValueAsDataView();
                sFilterPair[] RowFilter = new sFilterPair[1];
                RowFilter[0].Set("PARAMETER_CD", "OPTIME");

                DataRowView SummaryValueRow;

                if (!FindRow(SummaryValueRecords, RowFilter, out SummaryValueRow) || SummaryValueRow["CURRENT_RPT_PERIOD_TOTAL"] == DBNull.Value)
                {
                    Category.SetCheckParameter("Current_Op_Time_Summary_Value_Record", null, eParameterDataType.DataRowView);
                    if (Category.GetCheckParameter("LME_HI_Method").ParameterValue == null || !cDBConvert.ToString(CurrentMonitorPlanLocationRecord["LOCATION_NAME"]).StartsWith("CP"))
                        if (Category.GetCheckParameter("Legacy_Data_Evaluation").ValueAsBool())
                            Category.CheckCatalogResult = "B";
                        else
                            Category.CheckCatalogResult = "E";
                }
                else
                {
                    Category.SetCheckParameter("Current_Op_Time_Summary_Value_Record", SummaryValueRow, eParameterDataType.DataRowView);

                    decimal SummaryValue = cDBConvert.ToDecimal(SummaryValueRow["CURRENT_RPT_PERIOD_TOTAL"]);

                    if (SummaryValue >= 0)
                    {
                        if (SummaryValue != Math.Round(SummaryValue, 2, MidpointRounding.AwayFromZero) && SummaryValue != decimal.MinValue)//check for null redundant but included to uncouple this line from previous line)
                            Category.CheckCatalogResult = "F";
                        else
                          if (OpTimeAccumArray[CurrentPosition] != -1)
                        {
                            decimal QuarterlyTolerance = GetQuarterlyTolerance("OPTIME", "HR", Category);
                            if (OpTimeAccumArray[CurrentPosition] != SummaryValue)
                            {
                                if (Math.Abs(OpTimeAccumArray[CurrentPosition] - SummaryValue) > QuarterlyTolerance)
                                    Category.CheckCatalogResult = "A";
                                else
                                    Category.SetCheckParameter("Emissions_Tolerance_Deviators",
                                      Category.GetCheckParameter("Emissions_Tolerance_Deviators").ValueAsString().ListAdd("OPTIME"), eParameterDataType.String);
                            }
                        }
                        else
                            Category.CheckCatalogResult = "D";
                    }
                    else
                        Category.CheckCatalogResult = "C";
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURAGG6");
            }

            return ReturnVal;
        }

        public static string HOURAGG7(cCategory Category, ref bool Log)
        //Compare NOx Rate Accumulator Values 
        {
            string ReturnVal = "";

            try
            {
                decimal[] CalculatedArray = Category.GetCheckParameter("Rpt_Period_Nox_Rate_Calculated_Accumulator_Array").ValueAsDecimalArray();
                decimal[] ReportedArray = Category.GetCheckParameter("Rpt_Period_Nox_Rate_Reported_Accumulator_Array").ValueAsDecimalArray();

                int CurrentPosition = Category.GetCheckParameter("Current_Monitor_Plan_Location_Postion").ValueAsInt();
                int[] OpHoursArray = Category.GetCheckParameter("Rpt_Period_Nox_Rate_Hours_Accumulator_Array").ValueAsIntArray();
                bool[] ExpectedSummaryValueArray = (bool[])Category.GetCheckParameter("Expected_Summary_Value_Nox_Rate_Array").ParameterValue;

                if (ExpectedSummaryValueArray[CurrentPosition])
                    if (Category.GetCheckParameter("LME_Annual").ValueAsBool())
                    {
                        decimal HICalcVal = Category.GetCheckParameter("Rpt_Period_HI_Calculated_Value").ValueAsDecimal();
                        decimal NOxCalcVal = Category.GetCheckParameter("Rpt_Period_NOx_Mass_Calculated_Value").ValueAsDecimal();
                        decimal[] NoxMassCalculatedArray = Category.GetCheckParameter("Rpt_Period_Nox_Mass_Calculated_Accumulator_Array").ValueAsDecimalArray();

                        if (HICalcVal != decimal.MinValue && NOxCalcVal != decimal.MinValue)
                        {
                            if (NoxMassCalculatedArray[CurrentPosition] == 0)
                                Category.SetCheckParameter("Rpt_Period_NOx_Rate_Calculated_Value", 0m, eParameterDataType.Decimal);
                            else
                                Category.SetCheckParameter("Rpt_Period_NOx_Rate_Calculated_Value", Math.Round(NoxMassCalculatedArray[CurrentPosition] / HICalcVal, 3, MidpointRounding.AwayFromZero), eParameterDataType.Decimal);
                        }
                        else
                            Category.SetCheckParameter("Rpt_Period_NOx_Rate_Calculated_Value", null, eParameterDataType.Decimal);
                    }
                    else
                    {
                        if (OpHoursArray[CurrentPosition] > 0 && CalculatedArray[CurrentPosition] >= 0)
                        {
                            Category.SetCheckParameter("Rpt_Period_NOx_Rate_Calculated_Value", Math.Round(CalculatedArray[CurrentPosition] / OpHoursArray[CurrentPosition], 3, MidpointRounding.AwayFromZero), eParameterDataType.Decimal);
                            Category.SetCheckParameter("Rpt_Period_NOx_Rate_Sum", CalculatedArray[CurrentPosition], eParameterDataType.Decimal);
                            Category.SetCheckParameter("Rpt_Period_NOx_Rate_Hours", OpHoursArray[CurrentPosition], eParameterDataType.Integer);
                        }
                        else
                        {
                            if (OpHoursArray[CurrentPosition] == 0 && CalculatedArray[CurrentPosition] == 0)
                            {
                                Category.SetCheckParameter("Rpt_Period_NOx_Rate_Calculated_Value", 0m, eParameterDataType.Decimal);
                                Category.SetCheckParameter("Rpt_Period_NOx_Rate_Sum", 0m, eParameterDataType.Decimal);
                                Category.SetCheckParameter("Rpt_Period_NOx_Rate_Hours", 0, eParameterDataType.Integer);
                            }
                            else
                            {
                                Category.SetCheckParameter("Rpt_Period_NOx_Rate_Calculated_Value", null, eParameterDataType.Decimal);
                                Category.SetCheckParameter("Rpt_Period_NOx_Rate_Sum", null, eParameterDataType.Decimal);
                                Category.SetCheckParameter("Rpt_Period_NOx_Rate_Hours", null, eParameterDataType.Integer);
                            }
                        }
                        if (OpHoursArray[CurrentPosition] > 0 && ReportedArray[CurrentPosition] >= 0)
                            ReportedArray[CurrentPosition] = Math.Round(ReportedArray[CurrentPosition] / OpHoursArray[CurrentPosition], 3, MidpointRounding.AwayFromZero);
                        else
                            ReportedArray[CurrentPosition] = -1m;
                        Category.SetArrayParameter("Rpt_Period_Nox_Rate_Reported_Accumulator_Array", ReportedArray);//redundant
                    }
                else
                    Category.SetCheckParameter("Rpt_Period_NOx_Rate_Calculated_Value", null, eParameterDataType.Decimal);

                int[] PeriodArray = Category.GetCheckParameter("Rpt_Period_NOx_Rate_Hours_Accumulator_Array").ValueAsIntArray();
                bool OpHoursEqualZero = cDBConvert.ToInteger(PeriodArray[CurrentPosition]) == 0;

                HourAgg7_CompareAccumValues("Current_NOx_Rate_Summary_Value_Record",
                                            "Expected_Summary_Value_Nox_Rate_Array",
                                            "Rpt_Period_Nox_Rate_Calculated_Value",
                                            "Rpt_Period_Nox_Rate_Reported_Accumulator_Array", OpHoursEqualZero,
                                            "NOXR", "LBMMBTU",
                                            Category);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURAGG7");
            }

            return ReturnVal;
        }

        public static string HOURAGG8(cCategory Category, ref bool Log)
        //Compare NOx Mass Accumulator Values 
        {
            string ReturnVal = "";

            try
            {
                decimal[] CalculatedArray = Category.GetCheckParameter("Rpt_Period_Nox_Mass_Calculated_Accumulator_Array").ValueAsDecimalArray();
                decimal[] ReportedArray = Category.GetCheckParameter("Rpt_Period_Nox_Mass_Reported_Accumulator_Array").ValueAsDecimalArray();
                int CurrentPosition = Category.GetCheckParameter("Current_Monitor_Plan_Location_Postion").ValueAsInt();
                bool[] ExpectedSummaryValueArray = (bool[])Category.GetCheckParameter("Expected_Summary_Value_Nox_Mass_Array").ParameterValue;

                if (CalculatedArray[CurrentPosition] == -1 || !ExpectedSummaryValueArray[CurrentPosition])
                    Category.SetCheckParameter("Rpt_Period_Nox_Mass_Calculated_Value", null, eParameterDataType.Decimal);
                else
                    Category.SetCheckParameter("Rpt_Period_Nox_Mass_Calculated_Value", Math.Round(CalculatedArray[CurrentPosition] / 2000, 1, MidpointRounding.AwayFromZero), eParameterDataType.Decimal);
                if (ReportedArray[CurrentPosition] >= 0)
                    Category.SetArrayParameter("Rpt_Period_Nox_Mass_Reported_Accumulator_Array", CurrentPosition, Math.Round(ReportedArray[CurrentPosition] / 2000, 1, MidpointRounding.AwayFromZero));
                int[] PeriodArray = Category.GetCheckParameter("Rpt_Period_Op_Hours_Accumulator_Array").ValueAsIntArray();
                bool OpHoursEqualZero = cDBConvert.ToInteger(PeriodArray[CurrentPosition]) == 0;

                CompareAccumulatorValues1("Current_NOx_Mass_Summary_Value_Record",
                                         "Expected_Summary_Value_Nox_Mass_Array",
                                         "Rpt_Period_Nox_Mass_Calculated_Value",
                                         "Rpt_Period_Nox_Mass_Reported_Accumulator_Array", OpHoursEqualZero,
                                         "NOXM", "TON",
                                         Category);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURAGG8");
            }

            return ReturnVal;
        }

        public static string HOURAGG10(cCategory Category, ref bool Log)
        //Compare CO2 Mass YTD Values
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("Annual_CO2M_Calculated_Value", null, eParameterDataType.Decimal);

                bool[] ExpectedSummaryValueArray = Category.GetCheckParameter("Expected_Summary_Value_CO2_Array").ValueAsBoolArray();
                int CurrentPosition = Category.GetCheckParameter("Current_Monitor_Plan_Location_Postion").ValueAsInt();
                decimal RptPerCO2MassCalcVal = cDBConvert.ToDecimal(Category.GetCheckParameter("Rpt_Period_CO2_Mass_Calculated_Value").ParameterValue);
                decimal AnnCO2MassCalcVal = decimal.MinValue;

                if (RptPerCO2MassCalcVal != decimal.MinValue || !ExpectedSummaryValueArray[CurrentPosition])
                {
                    int? CurrentReportingPeriodYear, CurrentReportingPeriodQuarter;
                    {
                        int RptPer = Convert.ToInt16(Category.GetCheckParameter("Current_Reporting_Period").ParameterValue);
                        DataView RptPerLookup = (DataView)Category.GetCheckParameter("Reporting_Period_Lookup_Table").ParameterValue;
                        DataRowView RptPerRow;

                        sFilterPair[] RptPerFilter = new sFilterPair[1];
                        RptPerFilter[0].Set("RPT_PERIOD_ID", RptPer, eFilterDataType.Integer);

                        if (FindRow(RptPerLookup, RptPerFilter, out RptPerRow) && cDBConvert.ToInteger(RptPerRow["QUARTER"]) > 1)
                        {
                            CurrentReportingPeriodYear = cDBConvert.ToInteger(RptPerRow["CALENDAR_YEAR"]);
                            CurrentReportingPeriodQuarter = cDBConvert.ToInteger(RptPerRow["QUARTER"]);
                        }
                        else
                        {
                            CurrentReportingPeriodYear = null;
                            CurrentReportingPeriodQuarter = null;
                        }
                    }


                    if (ExpectedSummaryValueArray[CurrentPosition])
                    {
                        if ("CO2M".InList(Category.GetCheckParameter("Emissions_Tolerance_Deviators").ValueAsString()))
                            AnnCO2MassCalcVal = cDBConvert.ToDecimal(((DataRowView)Category.GetCheckParameter("Current_CO2_Summary_Value_Record").ParameterValue)["CURRENT_RPT_PERIOD_TOTAL"]);
                        else
                            AnnCO2MassCalcVal = RptPerCO2MassCalcVal;//for readability
                        Category.SetCheckParameter("Annual_CO2M_Calculated_Value", AnnCO2MassCalcVal, eParameterDataType.Decimal);
                    }
                    else if (CurrentReportingPeriodQuarter.HasValue && (CurrentReportingPeriodQuarter.Value > 1))
                    {
                        AnnCO2MassCalcVal = 0;
                        Category.SetCheckParameter("Annual_CO2M_Calculated_Value", AnnCO2MassCalcVal, eParameterDataType.Decimal);
                    }

                    if (CurrentReportingPeriodQuarter.HasValue && (CurrentReportingPeriodQuarter.Value > 1))
                    {
                        int? Co2StartQuarter = Category.GetCheckParameter("CO2_Start_Quarter").AsInteger();

                        if (Co2StartQuarter.HasValue)
                        {
                            DataView SuppRecs = (DataView)Category.GetCheckParameter("Operating_Supp_Data_Records_by_Location").ParameterValue;
                            DataRowView SuppRecRow;

                            sFilterPair[] SuppFilter = new sFilterPair[3];
                            SuppFilter[0].Set("PARAMETER_CD", "CO2M");
                            SuppFilter[1].Set("CALENDAR_YEAR", CurrentReportingPeriodYear.Value, eFilterDataType.Integer);

                            for (int i = Co2StartQuarter.Value; i < CurrentReportingPeriodQuarter.Value; i++)
                            {
                                SuppFilter[2].Set("QUARTER", i, eFilterDataType.Integer);

                                if (!FindRow(SuppRecs, SuppFilter, out SuppRecRow))
                                {
                                    if (ExpectedSummaryValueArray[CurrentPosition])
                                    {
                                        Category.SetCheckParameter("Annual_CO2M_Calculated_Value", null, eParameterDataType.Decimal);
                                        Category.CheckCatalogResult = "A";
                                        break;
                                    }
                                }
                                else
                                    AnnCO2MassCalcVal += cDBConvert.ToDecimal(SuppRecRow["OP_VALUE"]);
                            }

                            if (string.IsNullOrEmpty(Category.CheckCatalogResult))
                                Category.SetCheckParameter("Annual_CO2M_Calculated_Value", AnnCO2MassCalcVal, eParameterDataType.Decimal);
                        }
                        else
                        {
                            AnnCO2MassCalcVal = decimal.MinValue;
                            Category.SetCheckParameter("Annual_CO2M_Calculated_Value", null, eParameterDataType.Decimal);
                        }
                    }

                    if (string.IsNullOrEmpty(Category.CheckCatalogResult))
                    {
                        DataRowView CO2SummaryValueRec = (DataRowView)Category.GetCheckParameter("Current_CO2_Summary_Value_Record").ParameterValue;

                        if (CO2SummaryValueRec != null)
                        {
                            decimal YTDTotal = cDBConvert.ToDecimal(CO2SummaryValueRec["YEAR_TOTAL"]);

                            if (AnnCO2MassCalcVal == Decimal.MinValue && !ExpectedSummaryValueArray[CurrentPosition])
                            {
                                Category.CheckCatalogResult = "G";
                            }
                            else if (YTDTotal < 0)
                                Category.CheckCatalogResult = "B";
                            else if (YTDTotal != Math.Round(YTDTotal, 1, MidpointRounding.AwayFromZero) && YTDTotal != decimal.MinValue)//check for null redundant but included to uncouple this line from previous line
                                Category.CheckCatalogResult = "D";
                            else
                            {
                                AnnCO2MassCalcVal = cDBConvert.ToDecimal(Category.GetCheckParameter("Annual_CO2M_Calculated_Value").ParameterValue);

                                if (AnnCO2MassCalcVal != decimal.MinValue)
                                    if (AnnCO2MassCalcVal != YTDTotal)
                                    {
                                        Category.CheckCatalogResult = "C";
                                    }
                            }

                            if (string.IsNullOrEmpty(Category.CheckCatalogResult))
                            {
                                if (CO2SummaryValueRec["OS_TOTAL"].HasDbValue())
                                    Category.CheckCatalogResult = "E";
                            }
                        }
                        else
                        {
                            if (AnnCO2MassCalcVal > 0 && !ExpectedSummaryValueArray[CurrentPosition])
                                Category.CheckCatalogResult = "F";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURAGG10");
            }

            return ReturnVal;
        }

        #endregion


        #region Check 11 = 20

        public static string HOURAGG11(cCategory Category, ref bool Log)
        //Compare SO2 Mass YTD Values 
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("Annual_SO2_Mass_Calculated_Value", null, eParameterDataType.Decimal);
                bool[] ExpectedSummaryValueArray = Category.GetCheckParameter("Expected_Summary_Value_SO2_Array").ValueAsBoolArray();
                int CurrentPosition = Category.GetCheckParameter("Current_Monitor_Plan_Location_Postion").ValueAsInt();
                decimal RptPerSO2MassCalcVal = cDBConvert.ToDecimal(Category.GetCheckParameter("Rpt_Period_SO2_Mass_Calculated_Value").ParameterValue);
                decimal AnnSO2MassCalcVal = decimal.MinValue;

                if (RptPerSO2MassCalcVal != decimal.MinValue || !ExpectedSummaryValueArray[CurrentPosition])
                {
                    int? CurrentReportingPeriodYear, CurrentReportingPeriodQuarter;
                    {
                        int RptPer = Convert.ToInt16(Category.GetCheckParameter("Current_Reporting_Period").ParameterValue);
                        DataView RptPerLookup = (DataView)Category.GetCheckParameter("Reporting_Period_Lookup_Table").ParameterValue;
                        DataRowView RptPerRow;

                        sFilterPair[] RptPerFilter = new sFilterPair[1];
                        RptPerFilter[0].Set("RPT_PERIOD_ID", RptPer, eFilterDataType.Integer);

                        if (FindRow(RptPerLookup, RptPerFilter, out RptPerRow) && cDBConvert.ToInteger(RptPerRow["QUARTER"]) > 1)
                        {
                            CurrentReportingPeriodYear = cDBConvert.ToInteger(RptPerRow["CALENDAR_YEAR"]);
                            CurrentReportingPeriodQuarter = cDBConvert.ToInteger(RptPerRow["QUARTER"]);
                        }
                        else
                        {
                            CurrentReportingPeriodYear = null;
                            CurrentReportingPeriodQuarter = null;
                        }
                    }


                    if (ExpectedSummaryValueArray[CurrentPosition])
                    {
                        if ("SO2M".InList(Category.GetCheckParameter("Emissions_Tolerance_Deviators").ValueAsString()))
                            AnnSO2MassCalcVal = cDBConvert.ToDecimal(((DataRowView)Category.GetCheckParameter("Current_SO2_Summary_Value_Record").ParameterValue)["CURRENT_RPT_PERIOD_TOTAL"]);
                        else
                            AnnSO2MassCalcVal = RptPerSO2MassCalcVal;//for readability
                        Category.SetCheckParameter("Annual_SO2_Mass_Calculated_Value", AnnSO2MassCalcVal, eParameterDataType.Decimal);
                    }
                    else if (CurrentReportingPeriodQuarter.HasValue && (CurrentReportingPeriodQuarter.Value > 1))
                    {
                        AnnSO2MassCalcVal = 0;
                        Category.SetCheckParameter("Annual_SO2_Mass_Calculated_Value", AnnSO2MassCalcVal, eParameterDataType.Decimal);
                    }

                    if (CurrentReportingPeriodQuarter.HasValue && (CurrentReportingPeriodQuarter.Value > 1))
                    {
                        int? So2StartQuarter = Category.GetCheckParameter("SO2_Start_Quarter").AsInteger();

                        if (So2StartQuarter.HasValue)
                        {
                            DataView SuppRecs = (DataView)Category.GetCheckParameter("Operating_Supp_Data_Records_by_Location").ParameterValue;
                            DataRowView SuppRecRow;

                            sFilterPair[] SuppFilter = new sFilterPair[3];
                            SuppFilter[0].Set("PARAMETER_CD", "SO2M");
                            SuppFilter[1].Set("CALENDAR_YEAR", CurrentReportingPeriodYear.Value, eFilterDataType.Integer);

                            for (int i = So2StartQuarter.Value; i < CurrentReportingPeriodQuarter.Value; i++)
                            {
                                SuppFilter[2].Set("QUARTER", i, eFilterDataType.Integer);

                                if (!FindRow(SuppRecs, SuppFilter, out SuppRecRow))
                                {
                                    if (ExpectedSummaryValueArray[CurrentPosition])
                                    {
                                        Category.SetCheckParameter("Annual_SO2_Mass_Calculated_Value", null, eParameterDataType.Decimal);
                                        Category.CheckCatalogResult = "A";
                                        break;
                                    }
                                }
                                else
                                    AnnSO2MassCalcVal += cDBConvert.ToDecimal(SuppRecRow["OP_VALUE"]);
                            }

                            if (string.IsNullOrEmpty(Category.CheckCatalogResult))
                                Category.SetCheckParameter("Annual_SO2_Mass_Calculated_Value", AnnSO2MassCalcVal, eParameterDataType.Decimal);
                        }
                        else
                        {
                            AnnSO2MassCalcVal = decimal.MinValue;
                            Category.SetCheckParameter("Annual_SO2_Mass_Calculated_Value", null, eParameterDataType.Decimal);
                        }
                    }

                    if (string.IsNullOrEmpty(Category.CheckCatalogResult))
                    {
                        DataRowView SO2SummaryValueRec = (DataRowView)Category.GetCheckParameter("Current_SO2_Summary_Value_Record").ParameterValue;
                        if (SO2SummaryValueRec != null)
                        {
                            decimal YTDTotal = cDBConvert.ToDecimal(SO2SummaryValueRec["YEAR_TOTAL"]);

                            if (AnnSO2MassCalcVal == decimal.MinValue && !ExpectedSummaryValueArray[CurrentPosition])
                            {
                                Category.CheckCatalogResult = "H";
                            }
                            else if (YTDTotal < 0)
                                Category.CheckCatalogResult = "B";
                            else
                              if (YTDTotal != Math.Round(YTDTotal, 1, MidpointRounding.AwayFromZero) && YTDTotal != decimal.MinValue)//check for null redundant but included to uncouple this line from previous line
                                Category.CheckCatalogResult = "D";
                            else
                            {
                                decimal AnnualSO2MCalcVal = cDBConvert.ToDecimal(Category.GetCheckParameter("Annual_SO2_Mass_Calculated_Value").ParameterValue);
                                if (AnnualSO2MCalcVal != decimal.MinValue)
                                    if (AnnualSO2MCalcVal != YTDTotal)
                                    {
                                        Category.CheckCatalogResult = "C";
                                    }
                            }

                            if (string.IsNullOrEmpty(Category.CheckCatalogResult))
                            {
                                if (SO2SummaryValueRec["OS_TOTAL"].HasDbValue())
                                    Category.CheckCatalogResult = "F";
                                else if (Category.GetCheckParameter("LME_Annual").ValueAsBool() && YTDTotal > 25)
                                    Category.CheckCatalogResult = "E";
                            }
                        }
                        else
                        {
                            if (AnnSO2MassCalcVal > 0 && !ExpectedSummaryValueArray[CurrentPosition])
                                Category.CheckCatalogResult = "G";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURAGG11");
            }

            return ReturnVal;
        }

        public static string HOURAGG12(cCategory Category, ref bool Log)
        //Compare NOx Mass YTD and OS Values
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("Annual_NOXM_Calculated_Value", null, eParameterDataType.Decimal);
                Category.SetCheckParameter("OS_NOx_Mass_Calculated_Value", null, eParameterDataType.Decimal);
                Category.SetCheckParameter("NOXM_Summary_Invalid_Fields", null, eParameterDataType.String);

                bool[] ExpectedSummaryValueArray = Category.GetCheckParameter("Expected_Summary_Value_NOx_Mass_Array").ValueAsBoolArray();
                int CurrentPosition = Category.GetCheckParameter("Current_Monitor_Plan_Location_Postion").ValueAsInt();
                decimal RptPerNOxMassCalcVal = cDBConvert.ToDecimal(Category.GetCheckParameter("Rpt_Period_NOx_Mass_Calculated_Value").ParameterValue);
                decimal AnnNOxMCalcVal = decimal.MinValue;
                decimal OSNOxMCalcVal = decimal.MinValue;
                string ImpreciseFields = "";

                int RptPer = Convert.ToInt16(Category.GetCheckParameter("Current_Reporting_Period").ParameterValue);
                DataView RptPerLookup = (DataView)Category.GetCheckParameter("Reporting_Period_Lookup_Table").ParameterValue;
                DataRowView RptPerRow;

                sFilterPair[] RptPerFilter = new sFilterPair[1];
                RptPerFilter[0].Set("RPT_PERIOD_ID", RptPer, eFilterDataType.Integer);

                int RptPerRowQuarter = 0;

                if (FindRow(RptPerLookup, RptPerFilter, out RptPerRow))
                    RptPerRowQuarter = cDBConvert.ToInteger(RptPerRow["QUARTER"]);

                bool OSRptReq = Convert.ToBoolean(Category.GetCheckParameter("OS_Reporting_Requirement").ParameterValue);
                bool AnnRptPerRequ = Convert.ToBoolean(Category.GetCheckParameter("Annual_Reporting_Requirement").ParameterValue);

                if (RptPerNOxMassCalcVal != decimal.MinValue || !ExpectedSummaryValueArray[CurrentPosition])
                {
                    if (ExpectedSummaryValueArray[CurrentPosition])
                    {
                        if (AnnRptPerRequ)
                        {
                            if ("NOXM".InList(Category.GetCheckParameter("Emissions_Tolerance_Deviators").ValueAsString()))
                                AnnNOxMCalcVal = cDBConvert.ToDecimal(((DataRowView)Category.GetCheckParameter("Current_NOx_Mass_Summary_Value_Record").ParameterValue)["CURRENT_RPT_PERIOD_TOTAL"]);
                            else
                                AnnNOxMCalcVal = RptPerNOxMassCalcVal;//for readability
                            Category.SetCheckParameter("Annual_NOXM_Calculated_Value", AnnNOxMCalcVal, eParameterDataType.Decimal);
                        }


                        if (OSRptReq)
                        {
                            if (RptPerRowQuarter == 2 || RptPerRowQuarter == 3)
                            {
                                if (AnnRptPerRequ && RptPerRowQuarter == 2)
                                {
                                    decimal[] PeriodCalculatedArray = Category.GetCheckParameter("Rpt_Period_Nox_Mass_Calculated_Accumulator_Array").ValueAsDecimalArray();
                                    decimal[] AprilCalculatedArray = Category.GetCheckParameter("April_Nox_Mass_Calculated_Accumulator_Array").ValueAsDecimalArray();
                                    OSNOxMCalcVal = Math.Round((PeriodCalculatedArray[CurrentPosition] - AprilCalculatedArray[CurrentPosition]) / 2000, 1, MidpointRounding.AwayFromZero);
                                    Category.SetCheckParameter("OS_NOx_Mass_Calculated_Value", OSNOxMCalcVal, eParameterDataType.Decimal);
                                }
                                else
                                {
                                    if ("NOXM".InList(Category.GetCheckParameter("Emissions_Tolerance_Deviators").ValueAsString()))
                                        OSNOxMCalcVal = cDBConvert.ToDecimal(((DataRowView)Category.GetCheckParameter("Current_NOx_Mass_Summary_Value_Record").ParameterValue)["CURRENT_RPT_PERIOD_TOTAL"]);
                                    else
                                        OSNOxMCalcVal = RptPerNOxMassCalcVal;
                                    Category.SetCheckParameter("OS_NOx_Mass_Calculated_Value", RptPerNOxMassCalcVal, eParameterDataType.Decimal);
                                }
                            }
                            else
                            {
                                if (RptPerRowQuarter == 4)
                                {
                                    OSNOxMCalcVal = 0;
                                    Category.SetCheckParameter("OS_NOx_Mass_Calculated_Value", 0, eParameterDataType.Decimal);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (AnnRptPerRequ && RptPerRowQuarter > 1)
                        {
                            AnnNOxMCalcVal = 0;
                            Category.SetCheckParameter("Annual_NOXM_Calculated_Value", AnnNOxMCalcVal, eParameterDataType.Decimal);
                        }
                        if (OSRptReq && RptPerRowQuarter > 2)
                        {
                            OSNOxMCalcVal = 0;
                            Category.SetCheckParameter("OS_NOx_Mass_Calculated_Value", RptPerNOxMassCalcVal, eParameterDataType.Decimal);
                        }
                    }

                    if (RptPerRowQuarter > 2 || (AnnRptPerRequ && RptPerRowQuarter == 2))
                    {
                        int RptPerRowYear = cDBConvert.ToInteger(RptPerRow["CALENDAR_YEAR"]);

                        int? NoxStartQuarter = Category.GetCheckParameter("NOX_Start_Quarter").AsInteger();

                        if (NoxStartQuarter.HasValue)
                        {
                            DataView SuppRecs = (DataView)Category.GetCheckParameter("Operating_Supp_Data_Records_by_Location").ParameterValue;
                            DataRowView SuppRecRow;

                            sFilterPair[] SuppFilter = new sFilterPair[3];
                            SuppFilter[2].Set("CALENDAR_YEAR", RptPerRowYear, eFilterDataType.Integer);

                            for (int i = NoxStartQuarter.Value; i < RptPerRowQuarter; i++)
                            {
                                SuppFilter[1].Set("QUARTER", i, eFilterDataType.Integer);

                                if (i == 2 && OSRptReq)
                                {
                                    SuppFilter[0].Set("PARAMETER_CD", "NOXMOS");

                                    if (FindRow(SuppRecs, SuppFilter, out SuppRecRow))
                                    {
                                        OSNOxMCalcVal += cDBConvert.ToDecimal(SuppRecRow["OP_VALUE"]);
                                        Category.SetCheckParameter("OS_NOx_Mass_Calculated_Value", OSNOxMCalcVal, eParameterDataType.Decimal);
                                    }
                                    else
                                    {
                                        if (ExpectedSummaryValueArray[CurrentPosition])
                                        {
                                            Category.SetCheckParameter("Annual_NOxM_Calculated_Value", null, eParameterDataType.Decimal);
                                            Category.SetCheckParameter("OS_NOx_Mass_Calculated_Value", null, eParameterDataType.Decimal);
                                            Category.CheckCatalogResult = "A";
                                            break;
                                        }
                                        else
                                        {
                                            SuppFilter[0].Set("PARAMETER_CD", "NOXM");
                                            if (FindRow(SuppRecs, SuppFilter, out SuppRecRow))
                                            {
                                                Category.SetCheckParameter("Annual_NOxM_Calculated_Value", null, eParameterDataType.Decimal);
                                                Category.SetCheckParameter("OS_NOx_Mass_Calculated_Value", null, eParameterDataType.Decimal);
                                                Category.CheckCatalogResult = "A";
                                                break;
                                            }
                                        }
                                    }
                                }

                                if (i != 2 || AnnRptPerRequ)
                                {
                                    SuppFilter[0].Set("PARAMETER_CD", "NOXM");

                                    if (FindRow(SuppRecs, SuppFilter, out SuppRecRow))
                                    {
                                        if (AnnRptPerRequ)
                                        {
                                            AnnNOxMCalcVal += cDBConvert.ToDecimal(SuppRecRow["OP_VALUE"]);
                                            Category.SetCheckParameter("Annual_NOxM_Calculated_Value", AnnNOxMCalcVal, eParameterDataType.Decimal);
                                        }

                                        if (i == 3 && OSRptReq)
                                        {
                                            OSNOxMCalcVal += cDBConvert.ToDecimal(SuppRecRow["OP_VALUE"]);
                                            Category.SetCheckParameter("OS_NOx_Mass_Calculated_Value", OSNOxMCalcVal, eParameterDataType.Decimal);
                                        }
                                    }
                                    else
                                    {
                                        if (ExpectedSummaryValueArray[CurrentPosition])
                                        {
                                            Category.SetCheckParameter("Annual_NOxM_Calculated_Value", null, eParameterDataType.Decimal);
                                            Category.SetCheckParameter("OS_NOx_Mass_Calculated_Value", null, eParameterDataType.Decimal);
                                            Category.CheckCatalogResult = "B";
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            AnnNOxMCalcVal = decimal.MinValue;
                            Category.SetCheckParameter("Annual_NOXM_Calculated_Value", null, eParameterDataType.Decimal);
                            OSNOxMCalcVal = decimal.MinValue;
                            Category.SetCheckParameter("OS_NOx_Mass_Calculated_Value", null, eParameterDataType.Decimal);
                        }
                    }

                    if (string.IsNullOrEmpty(Category.CheckCatalogResult))
                    {
                        DataRowView NOxMassSummaryValueRec = (DataRowView)Category.GetCheckParameter("Current_NOX_Mass_Summary_Value_Record").ParameterValue;
                        if (NOxMassSummaryValueRec != null)
                        {
                            if ((AnnNOxMCalcVal == decimal.MinValue && OSNOxMCalcVal == decimal.MinValue) && !ExpectedSummaryValueArray[CurrentPosition])
                            {
                                Category.CheckCatalogResult = "K";
                            }
                            else
                            {
                                string noxmSummaryInvalidFields = Convert.ToString(Category.GetCheckParameter("NOXM_Summary_Invalid_Fields").ParameterValue);
                                decimal YTDTotal = cDBConvert.ToDecimal(NOxMassSummaryValueRec["YEAR_TOTAL"]);
                                decimal OSTotal = cDBConvert.ToDecimal(NOxMassSummaryValueRec["OS_TOTAL"]);

                                if ((YTDTotal == decimal.MinValue && AnnRptPerRequ) || (YTDTotal != decimal.MinValue && YTDTotal < 0))
                                    noxmSummaryInvalidFields = noxmSummaryInvalidFields.ListAdd("YearToDateTotal");
                                if ((OSTotal == decimal.MinValue && OSRptReq && (RptPerRowQuarter == 2 || RptPerRowQuarter == 3 || RptPerRowQuarter == 4)) || (OSTotal != decimal.MinValue && OSTotal < 0))
                                    noxmSummaryInvalidFields = noxmSummaryInvalidFields.ListAdd("OzoneSeasonToDateTotal");
                                if (YTDTotal != Math.Round(YTDTotal, 1, MidpointRounding.AwayFromZero) && YTDTotal != decimal.MinValue)
                                    ImpreciseFields = ImpreciseFields.ListAdd("YearToDateTotal");
                                if (OSTotal != Math.Round(OSTotal, 1, MidpointRounding.AwayFromZero) && OSTotal != decimal.MinValue)
                                    ImpreciseFields = ImpreciseFields.ListAdd("OzoneSeasonToDateTotal");
                                if (noxmSummaryInvalidFields != "")
                                    Category.CheckCatalogResult = "C";
                                else
                                  if (ImpreciseFields != "")
                                {
                                    noxmSummaryInvalidFields = ImpreciseFields;
                                    Category.CheckCatalogResult = "E";
                                }
                                else
                                {
                                    AnnNOxMCalcVal = cDBConvert.ToDecimal(Category.GetCheckParameter("Annual_NOXM_Calculated_Value").ParameterValue);
                                    OSNOxMCalcVal = cDBConvert.ToDecimal(Category.GetCheckParameter("OS_NOx_Mass_Calculated_Value").ParameterValue);
                                    if (AnnNOxMCalcVal != decimal.MinValue || OSNOxMCalcVal != decimal.MinValue)
                                    {
                                        decimal Tolerance = GetQuarterlyTolerance("NOXM", "TON", Category);
                                        if (AnnNOxMCalcVal != decimal.MinValue && AnnNOxMCalcVal != YTDTotal)
                                            noxmSummaryInvalidFields = noxmSummaryInvalidFields.ListAdd("YearToDateTotal");
                                        if (OSNOxMCalcVal != decimal.MinValue && OSNOxMCalcVal != OSTotal)
                                            if ((Math.Abs(OSNOxMCalcVal - OSTotal) > Tolerance) || (RptPerRowQuarter > 2))
                                                noxmSummaryInvalidFields = noxmSummaryInvalidFields.ListAdd("OzoneSeasonToDateTotal");
                                        if (noxmSummaryInvalidFields != "")
                                        {
                                            if (noxmSummaryInvalidFields.Contains("Year"))
                                            {
                                                if (noxmSummaryInvalidFields.Contains("Ozone"))
                                                    Category.CheckCatalogResult = "D";
                                                else
                                                    Category.CheckCatalogResult = "H";
                                            }
                                            else
                                                Category.CheckCatalogResult = "I";
                                        }
                                    }
                                }

                                if (noxmSummaryInvalidFields != "")
                                    Category.SetCheckParameter("NOXM_Summary_Invalid_Fields", noxmSummaryInvalidFields, eParameterDataType.String);

                                if (string.IsNullOrEmpty(Category.CheckCatalogResult))
                                {
                                    if (!Category.GetCheckParameter("OS_Reporting_Requirement").ValueAsBool() &&
                                        NOxMassSummaryValueRec["OS_TOTAL"].HasDbValue())
                                        Category.CheckCatalogResult = "G";
                                    else if (!Category.GetCheckParameter("Annual_Reporting_Requirement").ValueAsBool() &&
                                        NOxMassSummaryValueRec["YEAR_TOTAL"].HasDbValue())
                                        Category.CheckCatalogResult = "L";
                                    else if (((Category.GetCheckParameter("LME_Annual").ValueAsBool() && (YTDTotal > 100)) ||
                                              (Category.GetCheckParameter("LME_OS").ValueAsBool() && (OSTotal > 50))))
                                        Category.CheckCatalogResult = "F";
                                }
                            }
                        }
                        else
                        {
                            if ((AnnNOxMCalcVal > 0 || OSNOxMCalcVal > 0) && !ExpectedSummaryValueArray[CurrentPosition])
                                Category.CheckCatalogResult = "J";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURAGG12");
            }

            return ReturnVal;
        }

        public static string HOURAGG13(cCategory Category, ref bool Log)
        //Compare NOx Rate YTD Values 
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("Annual_NOXR_Calculated_Value", null, eParameterDataType.Decimal);
                bool[] ExpectedSummaryValueArray = Category.GetCheckParameter("Expected_Summary_Value_Nox_Rate_Array").ValueAsBoolArray();
                int CurrentPosition = Category.GetCheckParameter("Current_Monitor_Plan_Location_Postion").ValueAsInt();
                decimal RptPerNOXRCalcVal = cDBConvert.ToDecimal(Category.GetCheckParameter("Rpt_Period_NOx_Rate_Calculated_Value").ParameterValue);
                int OpHrsCalcVal = Convert.ToInt16(Category.GetCheckParameter("Rpt_Period_NOx_Rate_Hours").ParameterValue);
                decimal RptPerNOXRSum = Convert.ToDecimal(Category.GetCheckParameter("Rpt_Period_NOx_Rate_Sum").ParameterValue);

                decimal TotalNOxMass = decimal.MinValue;
                decimal TotalHI = decimal.MinValue;
                decimal AnnNOXRCalcVal = 0;


                int? CurrentReportingPeriodYear, CurrentReportingPeriodQuarter;
                {
                    int RptPer = Convert.ToInt16(Category.GetCheckParameter("Current_Reporting_Period").ParameterValue);
                    DataView RptPerLookup = (DataView)Category.GetCheckParameter("Reporting_Period_Lookup_Table").ParameterValue;
                    DataRowView RptPerRow;

                    sFilterPair[] RptPerFilter = new sFilterPair[1];
                    RptPerFilter[0].Set("RPT_PERIOD_ID", RptPer, eFilterDataType.Integer);

                    if (FindRow(RptPerLookup, RptPerFilter, out RptPerRow) && cDBConvert.ToInteger(RptPerRow["QUARTER"]) > 1)
                    {
                        CurrentReportingPeriodYear = cDBConvert.ToInteger(RptPerRow["CALENDAR_YEAR"]);
                        CurrentReportingPeriodQuarter = cDBConvert.ToInteger(RptPerRow["QUARTER"]);
                    }
                    else
                    {
                        CurrentReportingPeriodYear = null;
                        CurrentReportingPeriodQuarter = null;
                    }
                }
                bool QTR234 = CurrentReportingPeriodQuarter.HasValue && (CurrentReportingPeriodQuarter.Value > 1);


                if (Category.GetCheckParameter("LME_Annual").ValueAsBool())
                {
                    if (ExpectedSummaryValueArray[CurrentPosition])
                    {
                        decimal[] NoxMassCalculatedArray = Category.GetCheckParameter("Rpt_Period_Nox_Mass_Calculated_Accumulator_Array").ValueAsDecimalArray();
                        decimal RptPerNOXMCalcVal = NoxMassCalculatedArray[CurrentPosition];
                        decimal RptPerHICalcVal = cDBConvert.ToDecimal(Category.GetCheckParameter("Rpt_Period_HI_Calculated_Value").ParameterValue);
                        if (RptPerNOXMCalcVal >= 0 && RptPerHICalcVal != decimal.MinValue)
                        {
                            TotalNOxMass = RptPerNOXMCalcVal;
                            TotalHI = RptPerHICalcVal;
                        }
                    }
                    else if (QTR234)
                    {
                        TotalNOxMass = 0;
                        TotalHI = 0;
                    }
                    if (QTR234 && TotalNOxMass != decimal.MinValue)
                    {
                        int? NoxrStartQuarter = Category.GetCheckParameter("NOXR_Start_Quarter").AsInteger();

                        if (NoxrStartQuarter.HasValue)
                        {
                            DataView SuppRecs = (DataView)Category.GetCheckParameter("Operating_Supp_Data_Records_by_Location").ParameterValue;
                            DataRowView SuppRecRow;

                            sFilterPair[] SuppFilter = new sFilterPair[3];
                            SuppFilter[2].Set("CALENDAR_YEAR", CurrentReportingPeriodYear.Value, eFilterDataType.Integer);

                            decimal OpVal;
                            decimal NoxVal;

                            for (int i = NoxrStartQuarter.Value; i < CurrentReportingPeriodQuarter.Value; i++)
                            {
                                SuppFilter[0].Set("PARAMETER_CD", "NOXR");
                                SuppFilter[1].Set("QUARTER", i, eFilterDataType.Integer);
                                if (FindRow(SuppRecs, SuppFilter, out SuppRecRow))
                                {
                                    NoxVal = cDBConvert.ToDecimal(SuppRecRow["OP_VALUE"]);
                                    SuppFilter[0].Set("PARAMETER_CD", "HIT");
                                    if (FindRow(SuppRecs, SuppFilter, out SuppRecRow))
                                    {
                                        OpVal = cDBConvert.ToDecimal(SuppRecRow["OP_VALUE"]);
                                        TotalHI += OpVal;
                                        NoxVal *= OpVal;
                                        NoxVal = Math.Round(NoxVal, 1, MidpointRounding.AwayFromZero);
                                        TotalNOxMass += NoxVal;
                                    }
                                    else
                                    {
                                        if (ExpectedSummaryValueArray[CurrentPosition])
                                        {
                                            TotalHI = decimal.MinValue;
                                            Category.CheckCatalogResult = "E";
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    if (ExpectedSummaryValueArray[CurrentPosition])
                                    {
                                        TotalNOxMass = decimal.MinValue;
                                        Category.CheckCatalogResult = "A";
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            TotalNOxMass = decimal.MinValue;
                        }
                    }
                    if (TotalNOxMass != decimal.MinValue && TotalHI != decimal.MinValue)
                    {
                        if (TotalNOxMass == 0)
                            Category.SetCheckParameter("Annual_NOXR_Calculated_Value", 0m, eParameterDataType.Decimal);
                        else
                            Category.SetCheckParameter("Annual_NOXR_Calculated_Value", Math.Round(TotalNOxMass / TotalHI, 3, MidpointRounding.AwayFromZero), eParameterDataType.Decimal);
                    }
                }
                else
                {
                    decimal TotalOpHrs = decimal.MinValue;

                    if (ExpectedSummaryValueArray[CurrentPosition])
                    {
                        if (RptPerNOXRCalcVal != decimal.MinValue)
                        {
                            AnnNOXRCalcVal = RptPerNOXRSum;
                            TotalOpHrs = OpHrsCalcVal;
                        }
                    }
                    else if (QTR234)
                        TotalOpHrs = 0;

                    if (QTR234 && TotalOpHrs != decimal.MinValue)
                    {
                        int? NoxrStartQuarter = Category.GetCheckParameter("NOXR_Start_Quarter").AsInteger();

                        if (NoxrStartQuarter.HasValue)
                        {
                            DataView SuppRecs = (DataView)Category.GetCheckParameter("Operating_Supp_Data_Records_by_Location").ParameterValue;
                            DataRowView SuppRecRow;
                            decimal NOxVal;
                            decimal OpVal;
                            sFilterPair[] SuppFilter = new sFilterPair[4];
                            SuppFilter[2].Set("CALENDAR_YEAR", CurrentReportingPeriodYear.Value, eFilterDataType.Integer);
                            SuppFilter[3].Set("FUEL_CD", DBNull.Value, eFilterDataType.String);
                            for (int i = NoxrStartQuarter.Value; i < CurrentReportingPeriodQuarter.Value; i++)
                            {
                                SuppFilter[0].Set("PARAMETER_CD", "NOXRSUM");
                                SuppFilter[1].Set("QUARTER", i, eFilterDataType.Integer);
                                if (FindRow(SuppRecs, SuppFilter, out SuppRecRow))
                                {
                                    OpVal = cDBConvert.ToDecimal(SuppRecRow["OP_VALUE"]);
                                    AnnNOXRCalcVal += OpVal;
                                    SuppFilter[0].Set("PARAMETER_CD", "NOXRHRS");
                                    if (FindRow(SuppRecs, SuppFilter, out SuppRecRow))
                                    {
                                        OpVal = cDBConvert.ToDecimal(SuppRecRow["OP_VALUE"]);
                                        TotalOpHrs += OpVal;
                                    }
                                    else
                                    {
                                        Category.CheckCatalogResult = "A";
                                        break;
                                    }
                                }
                                else
                                {
                                    SuppFilter[0].Set("PARAMETER_CD", "NOXR");
                                    if (FindRow(SuppRecs, SuppFilter, out SuppRecRow))
                                    {
                                        OpVal = cDBConvert.ToDecimal(SuppRecRow["OP_VALUE"]);
                                        NOxVal = OpVal;
                                        SuppFilter[0].Set("PARAMETER_CD", "OPHOURS");
                                        if (FindRow(SuppRecs, SuppFilter, out SuppRecRow))
                                        {
                                            OpVal = cDBConvert.ToDecimal(SuppRecRow["OP_VALUE"]);
                                            TotalOpHrs += OpVal;
                                            AnnNOXRCalcVal += OpVal * NOxVal;
                                        }
                                        else
                                        {
                                            if (ExpectedSummaryValueArray[CurrentPosition])
                                            {
                                                AnnNOXRCalcVal = decimal.MinValue;
                                                Category.CheckCatalogResult = "B";
                                                break;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (ExpectedSummaryValueArray[CurrentPosition])
                                        {
                                            AnnNOXRCalcVal = decimal.MinValue;
                                            Category.CheckCatalogResult = "A";
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            AnnNOXRCalcVal = decimal.MinValue;
                            Category.SetCheckParameter("Annual_NOXR_Calculated_Value", null, eParameterDataType.Decimal);
                        }

                    }
                    if (AnnNOXRCalcVal != decimal.MinValue)
                        if (TotalOpHrs == 0)
                            Category.SetCheckParameter("Annual_NOXR_Calculated_Value", 0m, eParameterDataType.Decimal);
                        else if (AnnNOXRCalcVal > 0)
                            Category.SetCheckParameter("Annual_NOXR_Calculated_Value", Math.Round(AnnNOXRCalcVal / TotalOpHrs, 3, MidpointRounding.AwayFromZero), eParameterDataType.Decimal);
                }

                if (string.IsNullOrEmpty(Category.CheckCatalogResult))
                {
                    DataRowView NOxRSummaryValueRec = (DataRowView)Category.GetCheckParameter("Current_NOX_Rate_Summary_Value_Record").ParameterValue;
                    AnnNOXRCalcVal = cDBConvert.ToDecimal(Category.GetCheckParameter("Annual_NOXR_Calculated_Value").ParameterValue);

                    if (NOxRSummaryValueRec != null)
                    {
                        decimal YTDTotal = cDBConvert.ToDecimal(NOxRSummaryValueRec["YEAR_TOTAL"]);

                        if (AnnNOXRCalcVal == decimal.MinValue && !ExpectedSummaryValueArray[CurrentPosition])
                        {
                            Category.CheckCatalogResult = "H";
                        }
                        else if (YTDTotal < 0)
                            Category.CheckCatalogResult = "C";
                        else
                        {
                            if (AnnNOXRCalcVal != decimal.MinValue)
                            {
                                decimal Tolerance = GetQuarterlyTolerance("NOXR", "LBMMBTU", Category);
                                if (Math.Abs(AnnNOXRCalcVal - YTDTotal) > Tolerance)
                                    Category.CheckCatalogResult = "D";
                            }
                        }

                        if (string.IsNullOrEmpty(Category.CheckCatalogResult))
                        {
                            if (NOxRSummaryValueRec["OS_TOTAL"].HasDbValue())
                                Category.CheckCatalogResult = "F";
                        }
                    }
                    else
                    {
                        if (AnnNOXRCalcVal > 0 && !ExpectedSummaryValueArray[CurrentPosition])
                            Category.CheckCatalogResult = "G";
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURAGG13");
            }

            return ReturnVal;
        }

        public static string HOURAGG14(cCategory Category, ref bool Log)
        //Compare Total Heat Input YTD and OS Values 
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("Annual_HIT_Calculated_Value", null, eParameterDataType.Decimal);
                Category.SetCheckParameter("OS_HIT_Calculated_Value", null, eParameterDataType.Decimal);
                Category.SetCheckParameter("HIT_Summary_Invalid_Fields", null, eParameterDataType.String);

                bool[] ExpectedSummaryValueArray = Category.GetCheckParameter("Expected_Summary_Value_HI_Array").ValueAsBoolArray();
                int CurrentPosition = Category.GetCheckParameter("Current_Monitor_Plan_Location_Postion").ValueAsInt();
                decimal RptPerHICalcVal = cDBConvert.ToDecimal(Category.GetCheckParameter("Rpt_Period_HI_Calculated_Value").ParameterValue);
                decimal AnnHICalcVal = decimal.MinValue;
                decimal OSHITCalcVal = decimal.MinValue;
                string ImpreciseFields = "";
                int RptPer = Convert.ToInt16(Category.GetCheckParameter("Current_Reporting_Period").ParameterValue);
                DataView RptPerLookup = (DataView)Category.GetCheckParameter("Reporting_Period_Lookup_Table").ParameterValue;
                DataRowView RptPerRow;
                sFilterPair[] RptPerFilter = new sFilterPair[1];
                RptPerFilter[0].Set("RPT_PERIOD_ID", RptPer, eFilterDataType.Integer);
                int RptPerRowQuarter = 0;
                if (FindRow(RptPerLookup, RptPerFilter, out RptPerRow))
                    RptPerRowQuarter = cDBConvert.ToInteger(RptPerRow["QUARTER"]);
                bool OSRptReq = Convert.ToBoolean(Category.GetCheckParameter("OS_Reporting_Requirement").ParameterValue);
                bool AnnRptPerRequ = Convert.ToBoolean(Category.GetCheckParameter("Annual_Reporting_Requirement").ParameterValue);

                if (RptPerHICalcVal != decimal.MinValue || !ExpectedSummaryValueArray[CurrentPosition])
                {
                    if (ExpectedSummaryValueArray[CurrentPosition])
                    {
                        if (AnnRptPerRequ)
                        {
                            if ("HIT".InList(Category.GetCheckParameter("Emissions_Tolerance_Deviators").ValueAsString()))
                                AnnHICalcVal = cDBConvert.ToDecimal(((DataRowView)Category.GetCheckParameter("Current_HI_Summary_Value_Record").ParameterValue)["CURRENT_RPT_PERIOD_TOTAL"]);
                            else
                                AnnHICalcVal = RptPerHICalcVal;//for readability
                            Category.SetCheckParameter("Annual_HIT_Calculated_Value", AnnHICalcVal, eParameterDataType.Decimal);
                        }

                        if (OSRptReq)
                        {
                            if (RptPerRowQuarter == 2 || RptPerRowQuarter == 3)
                            {
                                if (AnnRptPerRequ && RptPerRowQuarter == 2)
                                {
                                    decimal[] PeriodCalculatedArray = Category.GetCheckParameter("Rpt_Period_Hi_Calculated_Accumulator_Array").ValueAsDecimalArray();
                                    decimal[] AprilCalculatedArray = Category.GetCheckParameter("April_HI_Calculated_Accumulator_Array").ValueAsDecimalArray();
                                    OSHITCalcVal = Math.Round(PeriodCalculatedArray[CurrentPosition] - AprilCalculatedArray[CurrentPosition], MidpointRounding.AwayFromZero);
                                    Category.SetCheckParameter("OS_HIT_Calculated_Value", OSHITCalcVal, eParameterDataType.Decimal);
                                }
                                else
                                {
                                    if ("HIT".InList(Category.GetCheckParameter("Emissions_Tolerance_Deviators").ValueAsString()))
                                        OSHITCalcVal = cDBConvert.ToDecimal(((DataRowView)Category.GetCheckParameter("Current_HI_Summary_Value_Record").ParameterValue)["CURRENT_RPT_PERIOD_TOTAL"]);
                                    else
                                        OSHITCalcVal = RptPerHICalcVal;
                                    Category.SetCheckParameter("OS_HIT_Calculated_Value", RptPerHICalcVal, eParameterDataType.Decimal);
                                }
                            }
                            else
                            {
                                if (RptPerRowQuarter == 4)
                                {
                                    OSHITCalcVal = 0;
                                    Category.SetCheckParameter("OS_HIT_Calculated_Value", 0, eParameterDataType.Decimal);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (AnnRptPerRequ && RptPerRowQuarter > 1)
                        {
                            AnnHICalcVal = 0;
                            Category.SetCheckParameter("Annual_HIT_Calculated_Value", AnnHICalcVal, eParameterDataType.Decimal);
                        }
                        if (OSRptReq && RptPerRowQuarter > 2)
                        {
                            OSHITCalcVal = 0;
                            Category.SetCheckParameter("OS_HIT_Calculated_Value", RptPerHICalcVal, eParameterDataType.Decimal);
                        }
                    }

                    if (RptPerRowQuarter > 2 || (AnnRptPerRequ && RptPerRowQuarter == 2))
                    {
                        int RptPerRowYear = cDBConvert.ToInteger(RptPerRow["CALENDAR_YEAR"]);

                        int? HeatInputStartQuarter = Category.GetCheckParameter("Heat_Input_Start_Quarter").AsInteger();

                        if (HeatInputStartQuarter.HasValue)
                        {
                            DataView SuppRecs = (DataView)Category.GetCheckParameter("Operating_Supp_Data_Records_by_Location").ParameterValue;
                            DataRowView SuppRecRow;

                            sFilterPair[] SuppFilter = new sFilterPair[3];
                            SuppFilter[2].Set("CALENDAR_YEAR", RptPerRowYear, eFilterDataType.Integer);

                            for (int i = HeatInputStartQuarter.Value; i < RptPerRowQuarter; i++)
                            {
                                SuppFilter[1].Set("QUARTER", i, eFilterDataType.Integer);

                                if (i == 2 && OSRptReq)
                                {
                                    SuppFilter[0].Set("PARAMETER_CD", "HITOS");

                                    if (FindRow(SuppRecs, SuppFilter, out SuppRecRow))
                                    {
                                        OSHITCalcVal += cDBConvert.ToDecimal(SuppRecRow["OP_VALUE"]);
                                        Category.SetCheckParameter("OS_HIT_Calculated_Value", OSHITCalcVal, eParameterDataType.Decimal);
                                    }
                                    else
                                    {
                                        if (ExpectedSummaryValueArray[CurrentPosition])
                                        {
                                            Category.SetCheckParameter("Annual_HIT_Calculated_Value", null, eParameterDataType.Decimal);
                                            Category.SetCheckParameter("OS_HIT_Calculated_Value", null, eParameterDataType.Decimal);
                                            Category.CheckCatalogResult = "A";
                                            break;
                                        }
                                        else
                                        {
                                            SuppFilter[0].Set("PARAMETER_CD", "HIT");
                                            if (FindRow(SuppRecs, SuppFilter, out SuppRecRow))
                                            {
                                                Category.SetCheckParameter("Annual_HIT_Calculated_Value", null, eParameterDataType.Decimal);
                                                Category.SetCheckParameter("OS_HIT_Calculated_Value", null, eParameterDataType.Decimal);
                                                Category.CheckCatalogResult = "A";
                                                break;
                                            }
                                        }
                                    }
                                }
                                if (i != 2 || AnnRptPerRequ)
                                {
                                    SuppFilter[0].Set("PARAMETER_CD", "HIT");

                                    if (FindRow(SuppRecs, SuppFilter, out SuppRecRow))
                                    {
                                        if (AnnRptPerRequ)
                                        {
                                            AnnHICalcVal += cDBConvert.ToDecimal(SuppRecRow["OP_VALUE"]);
                                            Category.SetCheckParameter("Annual_HIT_Calculated_Value", AnnHICalcVal, eParameterDataType.Decimal);
                                        }

                                        if (i == 3 && OSRptReq)
                                        {
                                            OSHITCalcVal += cDBConvert.ToDecimal(SuppRecRow["OP_VALUE"]);
                                            Category.SetCheckParameter("OS_HIT_Calculated_Value", OSHITCalcVal, eParameterDataType.Decimal);
                                        }
                                    }
                                    else
                                    {
                                        if (ExpectedSummaryValueArray[CurrentPosition])
                                        {
                                            Category.SetCheckParameter("Annual_HIT_Calculated_Value", null, eParameterDataType.Decimal);
                                            Category.SetCheckParameter("OS_HIT_Calculated_Value", null, eParameterDataType.Decimal);
                                            Category.CheckCatalogResult = "B";
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            AnnHICalcVal = decimal.MinValue;
                            Category.SetCheckParameter("Annual_HIT_Calculated_Value", null, eParameterDataType.Decimal);
                            OSHITCalcVal = decimal.MinValue;
                            Category.SetCheckParameter("OS_HIT_Calculated_Value", null, eParameterDataType.Decimal);
                        }
                    }

                    if (string.IsNullOrEmpty(Category.CheckCatalogResult))
                    {
                        DataRowView HISummaryValueRec = (DataRowView)Category.GetCheckParameter("Current_HI_Summary_Value_Record").ParameterValue;

                        if (HISummaryValueRec != null)
                        {
                            DataRowView CurrentMonitorPlanLocationRecord = Category.GetCheckParameter("Current_Monitor_Plan_Location_Record").ValueAsDataRowView();
                            if (((AnnHICalcVal == decimal.MinValue && OSHITCalcVal == decimal.MinValue) && !ExpectedSummaryValueArray[CurrentPosition]) && (Category.GetCheckParameter("LME_HI_Method").ValueAsString() != "LTFF" || !CurrentMonitorPlanLocationRecord["LOCATION_NAME"].ToString().StartsWith("CP")))
                            {
                                Category.CheckCatalogResult = "K";
                            }
                            else
                            {
                                string InvalidFields = Convert.ToString(Category.GetCheckParameter("HIT_Summary_Invalid_Fields").ParameterValue);
                                decimal YTDTotal = cDBConvert.ToDecimal(HISummaryValueRec["YEAR_TOTAL"]);
                                decimal OSTotal = cDBConvert.ToDecimal(HISummaryValueRec["OS_TOTAL"]);
                                bool IsLegacy = Category.GetCheckParameter("Legacy_Data_Evaluation").ValueAsBool();
                                if ((YTDTotal == decimal.MinValue && AnnRptPerRequ) || (YTDTotal != decimal.MinValue && YTDTotal < 0))
                                    InvalidFields = InvalidFields.ListAdd("YearToDateTotal");
                                if ((OSTotal == decimal.MinValue && OSRptReq && (RptPerRowQuarter == 2 || RptPerRowQuarter == 3 || RptPerRowQuarter == 4)) || (OSTotal != decimal.MinValue && OSTotal < 0))
                                    InvalidFields = InvalidFields.ListAdd("OzoneSeasonToDateTotal");
                                if (YTDTotal != Math.Round(YTDTotal, 0, MidpointRounding.AwayFromZero) && YTDTotal != decimal.MinValue)
                                    ImpreciseFields = ImpreciseFields.ListAdd("YearToDateTotal");
                                if (OSTotal != Math.Round(OSTotal, 0, MidpointRounding.AwayFromZero) && OSTotal != decimal.MinValue)
                                    if (!IsLegacy || (OSTotal != Math.Round(OSTotal, 1, MidpointRounding.AwayFromZero) && OSTotal != decimal.MinValue))
                                        ImpreciseFields = ImpreciseFields.ListAdd("OzoneSeasonToDateTotal");
                                if (InvalidFields != "")
                                    Category.CheckCatalogResult = "C";
                                else
                                  if (ImpreciseFields != "")
                                {
                                    InvalidFields = ImpreciseFields;
                                    Category.CheckCatalogResult = "E";
                                }
                                else
                                {
                                    AnnHICalcVal = cDBConvert.ToDecimal(Category.GetCheckParameter("Annual_HIT_Calculated_Value").ParameterValue);
                                    OSHITCalcVal = cDBConvert.ToDecimal(Category.GetCheckParameter("OS_HIT_Calculated_Value").ParameterValue);
                                    if (AnnHICalcVal != decimal.MinValue || OSHITCalcVal != decimal.MinValue)
                                    {
                                        decimal Tolerance = GetQuarterlyTolerance("HIT", "MMBTU", Category);
                                        if (AnnHICalcVal != decimal.MinValue && AnnHICalcVal != YTDTotal)
                                            InvalidFields = InvalidFields.ListAdd("YearToDateTotal");
                                        if (OSHITCalcVal != decimal.MinValue && OSHITCalcVal != OSTotal)
                                            if (!Category.GetCheckParameter("Legacy_Data_Evaluation").ValueAsBool())
                                            {
                                                if ((Math.Abs(OSHITCalcVal - OSTotal) > Tolerance) || (RptPerRowQuarter > 2))
                                                    InvalidFields = InvalidFields.ListAdd("OzoneSeasonToDateTotal");
                                            }
                                            else
                                              if ((Math.Abs(OSHITCalcVal - Math.Round(OSTotal, MidpointRounding.AwayFromZero)) > Tolerance)
                                                  || (RptPerRowQuarter > 2))
                                                InvalidFields = InvalidFields.ListAdd("OzoneSeasonToDateTotal");
                                        if (InvalidFields != "")
                                            if (InvalidFields.Contains("YearToDateTotal"))
                                            {
                                                if (InvalidFields.Contains("OzoneSeasonToDateTotal"))
                                                    Category.CheckCatalogResult = "D";
                                                else
                                                    Category.CheckCatalogResult = "H";
                                            }
                                            else
                                            {
                                                if (IsLegacy)
                                                    Category.CheckCatalogResult = "F";
                                                else
                                                    Category.CheckCatalogResult = "I";
                                            }
                                    }
                                }
                                if (InvalidFields != "")
                                    Category.SetCheckParameter("HIT_Summary_Invalid_Fields", InvalidFields, eParameterDataType.String);

                                if (string.IsNullOrEmpty(Category.CheckCatalogResult))
                                {
                                    if (!Category.GetCheckParameter("OS_Reporting_Requirement").ValueAsBool() &&
                                        HISummaryValueRec["OS_TOTAL"].HasDbValue())
                                        Category.CheckCatalogResult = "G";
                                    else if (!Category.GetCheckParameter("Annual_Reporting_Requirement").ValueAsBool() &&
                                        HISummaryValueRec["YEAR_TOTAL"].HasDbValue())
                                        Category.CheckCatalogResult = "L";
                                }
                            }
                        }
                        else
                        {
                            if ((AnnHICalcVal > 0 || OSHITCalcVal > 0) && !ExpectedSummaryValueArray[CurrentPosition])
                                Category.CheckCatalogResult = "J";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURAGG14");
            }

            return ReturnVal;
        }

        public static string HOURAGG15(cCategory Category, ref bool Log)
        //Compare Operating Time YTD and OS Values 
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("Annual_OPTIME_Calculated_Value", null, eParameterDataType.Decimal);
                Category.SetCheckParameter("OS_OPTIME_Calculated_Value", null, eParameterDataType.Decimal);
                Category.SetCheckParameter("OPTIME_Summary_Invalid_Fields", null, eParameterDataType.String);

                decimal RptPerOpTimeCalcVal = cDBConvert.ToDecimal(Category.GetCheckParameter("Rpt_Period_Op_Time_Calculated_Value").ParameterValue);
                decimal AnnOpTimeCalcVal = 0;
                decimal OSOpTimeCalcVal = 0;
                string ImpreciseFields = "";

                if (RptPerOpTimeCalcVal != decimal.MinValue)
                {
                    bool AnnRptPerRequ = Convert.ToBoolean(Category.GetCheckParameter("Annual_Reporting_Requirement").ParameterValue);

                    if (AnnRptPerRequ)
                    {
                        if ("OPTIME".InList(Category.GetCheckParameter("Emissions_Tolerance_Deviators").ValueAsString()))
                            AnnOpTimeCalcVal = cDBConvert.ToDecimal(((DataRowView)Category.GetCheckParameter("Current_Op_Time_Summary_Value_Record").ParameterValue)["CURRENT_RPT_PERIOD_TOTAL"]);
                        else
                            AnnOpTimeCalcVal = RptPerOpTimeCalcVal;//for readability
                        Category.SetCheckParameter("Annual_OPTIME_Calculated_Value", AnnOpTimeCalcVal, eParameterDataType.Decimal);
                    }

                    int RptPer = Convert.ToInt16(Category.GetCheckParameter("Current_Reporting_Period").ParameterValue);
                    DataView RptPerLookup = (DataView)Category.GetCheckParameter("Reporting_Period_Lookup_Table").ParameterValue;
                    DataRowView RptPerRow;
                    sFilterPair[] RptPerFilter = new sFilterPair[1];
                    RptPerFilter[0].Set("RPT_PERIOD_ID", RptPer, eFilterDataType.Integer);
                    int RptPerRowQuarter = 0;

                    if (FindRow(RptPerLookup, RptPerFilter, out RptPerRow))
                        RptPerRowQuarter = cDBConvert.ToInteger(RptPerRow["QUARTER"]);

                    bool OSRptReq = Convert.ToBoolean(Category.GetCheckParameter("OS_Reporting_Requirement").ParameterValue);

                    if (OSRptReq)
                    {
                        if (RptPerRowQuarter == 2 || RptPerRowQuarter == 3)
                        {
                            if (AnnRptPerRequ && RptPerRowQuarter == 2)
                            {
                                int CurrentPosition = Category.GetCheckParameter("Current_Monitor_Plan_Location_Postion").ValueAsInt();
                                decimal[] PeriodArray = Category.GetCheckParameter("Rpt_Period_Op_Time_Accumulator_Array").ValueAsDecimalArray();
                                decimal[] AprilArray = Category.GetCheckParameter("April_Op_Time_Accumulator_Array").ValueAsDecimalArray();
                                OSOpTimeCalcVal = PeriodArray[CurrentPosition] - AprilArray[CurrentPosition];
                                Category.SetCheckParameter("OS_OPTIME_Calculated_Value", OSOpTimeCalcVal, eParameterDataType.Decimal);
                            }
                            else
                            {
                                if ("OPTIME".InList(Category.GetCheckParameter("Emissions_Tolerance_Deviators").ValueAsString()))
                                    OSOpTimeCalcVal = cDBConvert.ToDecimal(((DataRowView)Category.GetCheckParameter("Current_Op_Time_Summary_Value_Record").ParameterValue)["CURRENT_RPT_PERIOD_TOTAL"]);
                                else
                                    OSOpTimeCalcVal = RptPerOpTimeCalcVal;
                                Category.SetCheckParameter("OS_OPTIME_Calculated_Value", RptPerOpTimeCalcVal, eParameterDataType.Decimal);
                            }
                        }
                        else
                        {
                            if (RptPerRowQuarter == 4)
                            {
                                OSOpTimeCalcVal = 0;
                                Category.SetCheckParameter("OS_OPTIME_Calculated_Value", 0, eParameterDataType.Decimal);
                            }
                        }
                    }

                    if (RptPerRowQuarter > 2 || (AnnRptPerRequ && RptPerRowQuarter == 2))
                    {
                        int RptPerRowYear = cDBConvert.ToInteger(RptPerRow["CALENDAR_YEAR"]);

                        int? StartQuarter = Category.GetCheckParameter("Start_Quarter").AsInteger();

                        if (StartQuarter.HasValue)
                        {
                            cReportingPeriod ecmpsQuarter = Category.GetCheckParameter("First_ECMPS_Reporting_Period_Object").ParameterValue.AsReportingPeriod();
                            DataView SuppRecs = (DataView)Category.GetCheckParameter("Operating_Supp_Data_Records_by_Location").ParameterValue;

                            DataRowView SuppRecRow;
                            sFilterPair[] SuppFilter = new sFilterPair[3];
                            SuppFilter[2].Set("CALENDAR_YEAR", RptPerRowYear, eFilterDataType.Integer);

                            for (int i = StartQuarter.Value; i < RptPerRowQuarter; i++)
                            {
                                SuppFilter[1].Set("QUARTER", i, eFilterDataType.Integer);

                                if (i == 2 && OSRptReq)
                                {
                                    SuppFilter[0].Set("PARAMETER_CD", "OSTIME");

                                    if (!FindRow(SuppRecs, SuppFilter, out SuppRecRow))
                                    {
                                        Category.SetCheckParameter("Annual_OPTIME_Calculated_Value", null, eParameterDataType.Decimal);
                                        Category.SetCheckParameter("OS_OPTIME_Calculated_Value", null, eParameterDataType.Decimal);

                                        cReportingPeriod secondQuarter = cReportingPeriod.GetReportingPeriod(RptPerRowYear, 2);

                                        if ((ecmpsQuarter != null) &&
                                            (secondQuarter != null) && //This should never be null
                                            (ecmpsQuarter.CompareTo(secondQuarter) <= 0))
                                        {
                                            Category.CheckCatalogResult = "A";
                                        }
                                        break;
                                    }
                                    else
                                    {
                                        OSOpTimeCalcVal += cDBConvert.ToDecimal(SuppRecRow["OP_VALUE"]);
                                        Category.SetCheckParameter("OS_OPTIME_Calculated_Value", OSOpTimeCalcVal, eParameterDataType.Decimal);
                                    }
                                }

                                if (i != 2 || AnnRptPerRequ)
                                {
                                    SuppFilter[0].Set("PARAMETER_CD", "OPTIME");

                                    if (!FindRow(SuppRecs, SuppFilter, out SuppRecRow))
                                    {
                                        if (AnnRptPerRequ)
                                            Category.SetCheckParameter("Annual_OPTIME_Calculated_Value", null, eParameterDataType.Decimal);

                                        if (OSRptReq)
                                            Category.SetCheckParameter("OS_OPTIME_Calculated_Value", null, eParameterDataType.Decimal);

                                        cReportingPeriod startQuarter = cReportingPeriod.GetReportingPeriod(RptPerRowYear, StartQuarter.Value);

                                        if ((ecmpsQuarter != null) &&
                                            (startQuarter != null) && //This should never be null
                                            (ecmpsQuarter.CompareTo(startQuarter) <= 0))
                                        {
                                            Category.CheckCatalogResult = "B";
                                        }
                                        break;
                                    }
                                    else
                                    {
                                        if (AnnRptPerRequ)
                                        {
                                            AnnOpTimeCalcVal += cDBConvert.ToDecimal(SuppRecRow["OP_VALUE"]);
                                            Category.SetCheckParameter("Annual_OPTIME_Calculated_Value", AnnOpTimeCalcVal, eParameterDataType.Decimal);
                                        }

                                        if (i == 3 && OSRptReq)
                                        {
                                            OSOpTimeCalcVal += cDBConvert.ToDecimal(SuppRecRow["OP_VALUE"]);
                                            Category.SetCheckParameter("OS_OPTIME_Calculated_Value", OSOpTimeCalcVal, eParameterDataType.Decimal);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            Category.SetCheckParameter("Annual_OPTIME_Calculated_Value", null, eParameterDataType.Decimal);
                            Category.SetCheckParameter("OS_OPTIME_Calculated_Value", null, eParameterDataType.Decimal);
                        }
                    }
                    if (string.IsNullOrEmpty(Category.CheckCatalogResult))
                    {
                        DataRowView OpTimeSummaryValueRec = (DataRowView)Category.GetCheckParameter("Current_Op_Time_Summary_Value_Record").ParameterValue;

                        if (OpTimeSummaryValueRec != null)
                        {
                            string InvalidFields = Convert.ToString(Category.GetCheckParameter("OPTIME_Summary_Invalid_Fields").ParameterValue);
                            decimal YTDTotal = cDBConvert.ToDecimal(OpTimeSummaryValueRec["YEAR_TOTAL"]);
                            decimal OSTotal = cDBConvert.ToDecimal(OpTimeSummaryValueRec["OS_TOTAL"]);
                            if ((YTDTotal == decimal.MinValue && AnnRptPerRequ) || (YTDTotal != decimal.MinValue && YTDTotal < 0))
                                InvalidFields = InvalidFields.ListAdd("YearToDateTotal");
                            if ((OSTotal == decimal.MinValue && OSRptReq && (RptPerRowQuarter == 2 || RptPerRowQuarter == 3 || RptPerRowQuarter == 4)) || (OSTotal != decimal.MinValue && OSTotal < 0))
                                InvalidFields = InvalidFields.ListAdd("OzoneSeasonToDateTotal");
                            if (YTDTotal != Math.Round(YTDTotal, 2, MidpointRounding.AwayFromZero) && YTDTotal != decimal.MinValue)
                                ImpreciseFields = ImpreciseFields.ListAdd("YearToDateTotal");
                            if (OSTotal != Math.Round(OSTotal, 2, MidpointRounding.AwayFromZero) && OSTotal != decimal.MinValue)
                                ImpreciseFields = ImpreciseFields.ListAdd("OzoneSeasonToDateTotal");
                            if (InvalidFields != "")
                                Category.CheckCatalogResult = "C";
                            else
                              if (ImpreciseFields != "")
                            {
                                InvalidFields = ImpreciseFields;
                                Category.CheckCatalogResult = "E";
                            }
                            else
                            {
                                AnnOpTimeCalcVal = cDBConvert.ToDecimal(Category.GetCheckParameter("Annual_OPTIME_Calculated_Value").ParameterValue);
                                OSOpTimeCalcVal = cDBConvert.ToDecimal(Category.GetCheckParameter("OS_OPTIME_Calculated_Value").ParameterValue);
                                if (AnnOpTimeCalcVal != decimal.MinValue || OSOpTimeCalcVal != decimal.MinValue)
                                {
                                    decimal Tolerance = GetQuarterlyTolerance("OPTIME", "HR", Category);
                                    if (AnnOpTimeCalcVal != decimal.MinValue && AnnOpTimeCalcVal != YTDTotal)
                                        InvalidFields = InvalidFields.ListAdd("YearToDateTotal");
                                    if (OSOpTimeCalcVal != decimal.MinValue && OSOpTimeCalcVal != OSTotal)
                                        if ((Math.Abs(OSOpTimeCalcVal - OSTotal) > Tolerance) || (RptPerRowQuarter > 2))
                                            InvalidFields = InvalidFields.ListAdd("OzoneSeasonToDateTotal");
                                    if (InvalidFields != "")
                                    {
                                        if (InvalidFields.Contains("YearToDateTotal"))
                                        {
                                            if (InvalidFields.Contains("OzoneSeasonToDateTotal"))
                                                Category.CheckCatalogResult = "D";
                                            else
                                                Category.CheckCatalogResult = "G";
                                        }
                                        else
                                        {
                                            Category.CheckCatalogResult = "H";
                                        }
                                    }
                                }
                            }
                            if (InvalidFields != "")
                                Category.SetCheckParameter("OPTIME_Summary_Invalid_Fields", InvalidFields, eParameterDataType.String);

                            if (string.IsNullOrEmpty(Category.CheckCatalogResult))
                            {
                                if (!Category.GetCheckParameter("OS_Reporting_Requirement").ValueAsBool() &&
                                    OpTimeSummaryValueRec["OS_TOTAL"].HasDbValue())
                                    Category.CheckCatalogResult = "F";
                                else if (!Category.GetCheckParameter("Annual_Reporting_Requirement").ValueAsBool() &&
                                      OpTimeSummaryValueRec["YEAR_TOTAL"].HasDbValue())
                                    Category.CheckCatalogResult = "I";

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURAGG15");
            }

            return ReturnVal;
        }

        public static string HOURAGG16(cCategory Category, ref bool Log)
        //Compare Operating Hours YTD and OS Values 
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("Annual_OPHOURS_Calculated_Value", null, eParameterDataType.Integer);
                Category.SetCheckParameter("OS_OPHOURS_Calculated_Value", null, eParameterDataType.Integer);
                Category.SetCheckParameter("OPHOURS_Summary_Invalid_Fields", null, eParameterDataType.String);

                int CurrentPosition = Category.GetCheckParameter("Current_Monitor_Plan_Location_Postion").ValueAsInt();
                int RptPerOpHrsCalcVal = cDBConvert.ToInteger(Category.GetCheckParameter("Rpt_Period_Op_Hours_Calculated_Value").ParameterValue);
                int AnnOpHrsCalcVal = 0;
                int OSOpHrsCalcVal = 0;
                string ImpreciseFields = "";

                if (RptPerOpHrsCalcVal != int.MinValue)
                {
                    bool AnnRptPerRequ = Convert.ToBoolean(Category.GetCheckParameter("Annual_Reporting_Requirement").ParameterValue);

                    if (AnnRptPerRequ)
                    {
                        if ("OPHOURS".InList(Category.GetCheckParameter("Emissions_Tolerance_Deviators").ValueAsString()))
                            AnnOpHrsCalcVal = cDBConvert.ToInteger(((DataRowView)Category.GetCheckParameter("Current_Op_Hours_Summary_Value_Record").ParameterValue)["CURRENT_RPT_PERIOD_TOTAL"]);
                        else
                            AnnOpHrsCalcVal = RptPerOpHrsCalcVal;//for readability
                        Category.SetCheckParameter("Annual_OPHOURS_Calculated_Value", AnnOpHrsCalcVal, eParameterDataType.Integer);
                    }

                    int RptPer = Convert.ToInt16(Category.GetCheckParameter("Current_Reporting_Period").ParameterValue);
                    DataView RptPerLookup = (DataView)Category.GetCheckParameter("Reporting_Period_Lookup_Table").ParameterValue;
                    DataRowView RptPerRow;
                    sFilterPair[] RptPerFilter = new sFilterPair[1];
                    RptPerFilter[0].Set("RPT_PERIOD_ID", RptPer, eFilterDataType.Integer);
                    int RptPerRowQuarter = 0;

                    if (FindRow(RptPerLookup, RptPerFilter, out RptPerRow))
                        RptPerRowQuarter = cDBConvert.ToInteger(RptPerRow["QUARTER"]);

                    bool OSRptReq = Convert.ToBoolean(Category.GetCheckParameter("OS_Reporting_Requirement").ParameterValue);

                    if (OSRptReq)
                    {
                        if (RptPerRowQuarter == 2 || RptPerRowQuarter == 3)
                        {
                            if (AnnRptPerRequ && RptPerRowQuarter == 2)
                            {
                                int[] PeriodArray = Category.GetCheckParameter("Rpt_Period_Op_Hours_Accumulator_Array").ValueAsIntArray();
                                int[] AprilArray = Category.GetCheckParameter("April_Op_Hours_Accumulator_Array").ValueAsIntArray();
                                OSOpHrsCalcVal = PeriodArray[CurrentPosition] - AprilArray[CurrentPosition];
                                Category.SetCheckParameter("OS_OPHOURS_Calculated_Value", OSOpHrsCalcVal, eParameterDataType.Integer);
                            }
                            else
                            {
                                if ("OPHOURS".InList(Category.GetCheckParameter("Emissions_Tolerance_Deviators").ValueAsString()))
                                    OSOpHrsCalcVal = cDBConvert.ToInteger(((DataRowView)Category.GetCheckParameter("Current_Op_Hours_Summary_Value_Record").ParameterValue)["CURRENT_RPT_PERIOD_TOTAL"]);
                                else
                                    OSOpHrsCalcVal = RptPerOpHrsCalcVal;
                                Category.SetCheckParameter("OS_OPHOURS_Calculated_Value", RptPerOpHrsCalcVal, eParameterDataType.Integer);
                            }
                        }
                        else
                        {
                            if (RptPerRowQuarter == 4)
                            {
                                OSOpHrsCalcVal = 0;
                                Category.SetCheckParameter("OS_OPHOURS_Calculated_Value", 0, eParameterDataType.Integer);
                            }
                        }
                    }

                    if (RptPerRowQuarter > 2 || (AnnRptPerRequ && RptPerRowQuarter == 2))
                    {
                        int RptPerRowYear = cDBConvert.ToInteger(RptPerRow["CALENDAR_YEAR"]);

                        int? StartQuarter = Category.GetCheckParameter("Start_Quarter").AsInteger();

                        if (StartQuarter.HasValue)
                        {
                            DataView SuppRecs = (DataView)Category.GetCheckParameter("Operating_Supp_Data_Records_by_Location").ParameterValue;
                            DataRowView SuppRecRow;
                            sFilterPair[] SuppFilter = new sFilterPair[4];

                            SuppFilter[2].Set("CALENDAR_YEAR", RptPerRowYear, eFilterDataType.Integer);
                            SuppFilter[3].Set("FUEL_CD", DBNull.Value, eFilterDataType.String);

                            for (int i = StartQuarter.Value; i < RptPerRowQuarter; i++)
                            {
                                SuppFilter[1].Set("QUARTER", i, eFilterDataType.Integer);

                                if (i == 2 && OSRptReq)
                                {
                                    SuppFilter[0].Set("PARAMETER_CD", "OSHOURS");

                                    if (FindRow(SuppRecs, SuppFilter, out SuppRecRow))
                                    {
                                        OSOpHrsCalcVal += cDBConvert.ToInteger(SuppRecRow["OP_VALUE"]);
                                        Category.SetCheckParameter("OS_OPHOURS_Calculated_Value", OSOpHrsCalcVal, eParameterDataType.Integer);
                                    }
                                    else
                                    {
                                        Category.SetCheckParameter("Annual_OPHOURS_Calculated_Value", null, eParameterDataType.Integer);
                                        Category.SetCheckParameter("OS_OPHOURS_Calculated_Value", null, eParameterDataType.Integer);
                                        Category.CheckCatalogResult = "A";
                                        break;
                                    }
                                }

                                if (i != 2 || AnnRptPerRequ)
                                {
                                    SuppFilter[0].Set("PARAMETER_CD", "OPHOURS");

                                    if (FindRow(SuppRecs, SuppFilter, out SuppRecRow))
                                    {
                                        if (AnnRptPerRequ)
                                        {
                                            AnnOpHrsCalcVal += cDBConvert.ToInteger(SuppRecRow["OP_VALUE"]);
                                            Category.SetCheckParameter("Annual_OPHOURS_Calculated_Value", AnnOpHrsCalcVal, eParameterDataType.Integer);
                                        }
                                        if (i == 3 && OSRptReq)
                                        {
                                            OSOpHrsCalcVal += cDBConvert.ToInteger(SuppRecRow["OP_VALUE"]);
                                            Category.SetCheckParameter("OS_OPHOURS_Calculated_Value", OSOpHrsCalcVal, eParameterDataType.Integer);
                                        }
                                    }
                                    else
                                    {
                                        Category.SetCheckParameter("Annual_OPHOURS_Calculated_Value", null, eParameterDataType.Integer);
                                        Category.SetCheckParameter("OS_OPHOURS_Calculated_Value", null, eParameterDataType.Integer);
                                        Category.CheckCatalogResult = "B";
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            Category.SetCheckParameter("Annual_OPHOURS_Calculated_Value", null, eParameterDataType.Integer);
                            Category.SetCheckParameter("OS_OPHOURS_Calculated_Value", null, eParameterDataType.Integer);
                        }
                    }

                    if (string.IsNullOrEmpty(Category.CheckCatalogResult))
                    {
                        DataRowView OpHrsSummaryValueRec = (DataRowView)Category.GetCheckParameter("Current_Op_Hours_Summary_Value_Record").ParameterValue;

                        if (OpHrsSummaryValueRec != null)
                        {
                            string InvalidFields = Convert.ToString(Category.GetCheckParameter("OPHOURS_Summary_Invalid_Fields").ParameterValue);
                            int YTDTotal = cDBConvert.ToInteger(OpHrsSummaryValueRec["YEAR_TOTAL"]);//truncated version
                            int OSTotal = cDBConvert.ToInteger(OpHrsSummaryValueRec["OS_TOTAL"]);//truncated
                            decimal YTDTotalAsDecimal = cDBConvert.ToDecimal(OpHrsSummaryValueRec["YEAR_TOTAL"]);//non-truncated version
                            decimal OSTotalAsDecimal = cDBConvert.ToDecimal(OpHrsSummaryValueRec["OS_TOTAL"]);//non-truncated

                            if ((YTDTotal == int.MinValue && AnnRptPerRequ) || (YTDTotal != int.MinValue && YTDTotal < 0))
                                InvalidFields = InvalidFields.ListAdd("YearToDateTotal");
                            if ((OSTotal == int.MinValue && OSRptReq && (RptPerRowQuarter == 2 || RptPerRowQuarter == 3 || RptPerRowQuarter == 4)) || (OSTotal != int.MinValue && OSTotal < 0))
                                InvalidFields = InvalidFields.ListAdd("OzoneSeasonToDateTotal");
                            if (YTDTotalAsDecimal != Math.Round(YTDTotalAsDecimal, 0, MidpointRounding.AwayFromZero) && YTDTotal != decimal.MinValue)
                                ImpreciseFields = ImpreciseFields.ListAdd("YearToDateTotal");
                            if (OSTotalAsDecimal != Math.Round(OSTotalAsDecimal, 0, MidpointRounding.AwayFromZero) && OSTotal != decimal.MinValue)
                                ImpreciseFields = ImpreciseFields.ListAdd("OzoneSeasonToDateTotal");
                            if (InvalidFields != "")
                                Category.CheckCatalogResult = "C";
                            else
                              if (ImpreciseFields != "")
                            {
                                InvalidFields = ImpreciseFields;
                                Category.CheckCatalogResult = "E";
                            }
                            else
                            {
                                AnnOpHrsCalcVal = cDBConvert.ToInteger(Category.GetCheckParameter("Annual_OPHOURS_Calculated_Value").ParameterValue);
                                OSOpHrsCalcVal = cDBConvert.ToInteger(Category.GetCheckParameter("OS_OPHOURS_Calculated_Value").ParameterValue);

                                if (AnnOpHrsCalcVal != decimal.MinValue || OSOpHrsCalcVal != decimal.MinValue)
                                {
                                    decimal Tolerance = GetQuarterlyTolerance("OPHOURS", "HR", Category);
                                    if (AnnOpHrsCalcVal != int.MinValue && AnnOpHrsCalcVal != YTDTotal)
                                        InvalidFields = InvalidFields.ListAdd("YearToDateTotal");
                                    if (OSOpHrsCalcVal != int.MinValue && OSOpHrsCalcVal != OSTotal)
                                        if ((Math.Abs(OSOpHrsCalcVal - OSTotal) > Tolerance) || (RptPerRowQuarter > 2))
                                            InvalidFields = InvalidFields.ListAdd("OzoneSeasonToDateTotal");
                                    if (InvalidFields != "")
                                    {
                                        if (InvalidFields.Contains("YearToDateTotal"))
                                        {
                                            if (InvalidFields.Contains("OzoneSeasonToDateTotal"))
                                                Category.CheckCatalogResult = "D";
                                            else
                                                Category.CheckCatalogResult = "G";
                                        }
                                        else
                                        {
                                            Category.CheckCatalogResult = "H";
                                        }
                                    }
                                }
                            }

                            if (InvalidFields != "")
                                Category.SetCheckParameter("OPHOURS_Summary_Invalid_Fields", InvalidFields, eParameterDataType.String);

                            if (string.IsNullOrEmpty(Category.CheckCatalogResult))
                            {
                                if (!Category.GetCheckParameter("OS_Reporting_Requirement").ValueAsBool() &&
                                    OpHrsSummaryValueRec["OS_TOTAL"].HasDbValue())
                                    Category.CheckCatalogResult = "F";
                                else if (!Category.GetCheckParameter("Annual_Reporting_Requirement").ValueAsBool() &&
                                    OpHrsSummaryValueRec["YEAR_TOTAL"].HasDbValue())
                                    Category.CheckCatalogResult = "I";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURAGG16");
            }

            return ReturnVal;
        }

        public static string HOURAGG17(cCategory category, ref bool log)
        // Check BCO2 Summary Value
        {
            string returnVal = "";

            try
            {
                // Initialize Current_BCO2_Summary_Value_Record parameter
                DataRowView currentBco2SummaryValueRecord = null;
                {
                    DataView summaryValueRecords = category.GetCheckParameter("Summary_Value_Records_By_Reporting_Period_Location").ValueAsDataView();

                    currentBco2SummaryValueRecord
                      = cRowFilter.FindRow(summaryValueRecords,
                                           new cFilterCondition[] { new cFilterCondition("PARAMETER_CD", "BCO2") });

                    category.SetCheckParameter("Current_BCO2_Summary_Value_Record", currentBco2SummaryValueRecord, eParameterDataType.DataRowView);
                }

                // Initialize out parameters
                DateTime? rggiBeginDate = null;
                int? rggiStartQuarter = null;
                decimal? bco2QuarterlyReportedValue = null;
                {
                    category.SetCheckParameter("RGGI_Begin_Date", rggiBeginDate, eParameterDataType.Date);
                    category.SetCheckParameter("RGGI_Start_Quarter", rggiStartQuarter, eParameterDataType.Integer);
                    category.SetCheckParameter("BCO2_Quarterly_Reported_Value", bco2QuarterlyReportedValue, eParameterDataType.Decimal);
                }

                if (currentBco2SummaryValueRecord != null)
                {
                    DataRowView currentMonitorPlanLocationRecord = category.GetCheckParameter("Current_Monitor_Plan_Location_Record").AsDataRowView();

                    if (currentMonitorPlanLocationRecord["STACK_PIPE_ID"].HasDbValue())
                    {
                        category.CheckCatalogResult = "A";
                    }
                    else
                    {
                        cReportingPeriod currentReportingPeriodObject
                          = (cReportingPeriod)category.GetCheckParameter("Current_Reporting_Period_Object").ParameterValue;

                        DataView locationProgramRecords
                          = category.GetCheckParameter("Location_Program_Records").ValueAsDataView();

                        DataRowView locationProgramRow
                          = cRowFilter.FindRow(locationProgramRecords,
                                               new cFilterCondition[]
                                               { new cFilterCondition("PRG_CD", "RGGI"),
                                     new cFilterCondition("UNIT_MONITOR_CERT_BEGIN_DATE",
                                                          currentReportingPeriodObject.EndedDate,
                                                          eFilterDataType.DateBegan,
                                                          eFilterConditionRelativeCompare.LessThanOrEqual),
                                     new cFilterCondition("END_DATE",
                                                          currentReportingPeriodObject.BeganDate,
                                                          eFilterDataType.DateEnded,
                                                          eFilterConditionRelativeCompare.GreaterThanOrEqual)});

                        if (locationProgramRow == null)
                        {
                            category.CheckCatalogResult = "B";
                        }
                        else
                        {
                            DateTime? umcbDate = locationProgramRow["UNIT_MONITOR_CERT_BEGIN_DATE"].AsDateTime();
                            DateTime? erbDate = locationProgramRow["EMISSIONS_RECORDING_BEGIN_DATE"].AsDateTime();

                            // Set RGGI Begin Date
                            {
                                if (umcbDate.Default(DateTypes.START) >= erbDate.Default(DateTypes.START))
                                    rggiBeginDate = umcbDate;
                                else
                                    rggiBeginDate = erbDate;
                            }
                            category.SetCheckParameter("RGGI_Begin_Date", rggiBeginDate, eParameterDataType.Date);

                            // Set RGGI Start Quarter
                            {
                                if (rggiBeginDate.Default(DateTypes.START).Year < currentReportingPeriodObject.Year)
                                    rggiStartQuarter = 1;
                                else
                                    rggiStartQuarter = rggiBeginDate.Quarter();
                            }
                            category.SetCheckParameter("RGGI_Start_Quarter", rggiStartQuarter, eParameterDataType.Integer);

                            // Attempt to set BCO2 Quarterly Reported Value
                            {
                                decimal? currentRptPeriodTotal
                                  = currentBco2SummaryValueRecord["CURRENT_RPT_PERIOD_TOTAL"].AsDecimal();

                                decimal? rptPeriodCo2MassCalculatedValue
                                  = category.GetCheckParameter("Rpt_Period_CO2_Mass_Calculated_Value").AsDecimal();

                                if (currentRptPeriodTotal.Default() < 0)
                                    category.CheckCatalogResult = "C";
                                else if (currentRptPeriodTotal.Value
                                           != Math.Round(currentRptPeriodTotal.Value, 1, MidpointRounding.AwayFromZero))
                                    category.CheckCatalogResult = "D";
                                else
                                {
                                    bco2QuarterlyReportedValue = currentRptPeriodTotal;
                                    category.SetCheckParameter("BCO2_Quarterly_Reported_Value", bco2QuarterlyReportedValue, eParameterDataType.Decimal);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                returnVal = category.CheckEngine.FormatError(ex, "HOURAGG17");
            }

            return returnVal;
        }

        public static string HOURAGG18(cCategory category, ref bool log)
        // Compare BCO2 Mass YTD Values
        {
            string returnVal = "";

            try
            {
                decimal? annualBco2CalculatedValue = -1;
                category.SetCheckParameter("Annual_BCO2_Calculated_Value", null, eParameterDataType.Decimal);

                decimal? bco2QuarterlyReportedValue
                  = category.GetCheckParameter("BCO2_Quarterly_Reported_Value").AsDecimal();

                if (bco2QuarterlyReportedValue != null)
                {
                    // Set Annual BCO2 Calculated Value
                    annualBco2CalculatedValue = bco2QuarterlyReportedValue;
                }

                cReportingPeriod currentReportingPeriodObject
                  = (cReportingPeriod)category.GetCheckParameter("Current_Reporting_Period_Object").ParameterValue;

                int rggiStartQuarter = category.GetCheckParameter("RGGI_Start_Quarter").AsInteger().Default();
                if (rggiStartQuarter != int.MinValue && currentReportingPeriodObject.Quarter > eQuarter.q1)
                {
                    DataView operatingSuppDataRecords = category.GetCheckParameter("Operating_Supp_Data_Records_by_Location").AsDataView();

                    cFilterCondition[] rowFilter = new cFilterCondition[3];
                    {
                        rowFilter[0] = new cFilterCondition("PARAMETER_CD", "BCO2");
                        rowFilter[1] = new cFilterCondition("CALENDAR_YEAR", currentReportingPeriodObject.Year, eFilterDataType.Integer);
                    }

                    int rptPeriodQuarter = currentReportingPeriodObject.Quarter.AsInteger();

                    for (int quarter = rggiStartQuarter; quarter < rptPeriodQuarter; quarter++)
                    {
                        rowFilter[2] = new cFilterCondition("QUARTER", quarter, eFilterDataType.Integer);

                        DataRowView operatingSuppDataRow
                          = cRowFilter.FindRow(operatingSuppDataRecords, rowFilter);

                        if (operatingSuppDataRow == null)
                        {
                            if (bco2QuarterlyReportedValue != null)
                            {
                                annualBco2CalculatedValue = null;
                                category.CheckCatalogResult = "A";
                                break;
                            }
                        }
                        else
                        {
                            if (annualBco2CalculatedValue == -1)
                                annualBco2CalculatedValue = operatingSuppDataRow["OP_VALUE"].AsDecimal();
                            else
                                annualBco2CalculatedValue += operatingSuppDataRow["OP_VALUE"].AsDecimal();
                        }
                    }
                }
                category.SetCheckParameter("Annual_BCO2_Calculated_Value", annualBco2CalculatedValue, eParameterDataType.Decimal);

                if (category.CheckCatalogResult.IsEmpty())
                {
                    DataRowView currentBco2SummaryValueRecord
                      = category.GetCheckParameter("Current_BCO2_Summary_Value_Record").AsDataRowView();
                    if (currentBco2SummaryValueRecord != null)
                    {


                        decimal? yearToDateTotal = currentBco2SummaryValueRecord["YEAR_TOTAL"].AsDecimal();
                        if (annualBco2CalculatedValue == -1)
                        {
                            category.SetCheckParameter("Annual_BCO2_Calculated_Value", null, eParameterDataType.Decimal);
                            category.CheckCatalogResult = "G";
                        }
                        else if ((yearToDateTotal == null) || (yearToDateTotal.Value < 0))
                            category.CheckCatalogResult = "B";
                        else if (yearToDateTotal.Value != Math.Round(yearToDateTotal.Value, 1, MidpointRounding.AwayFromZero))
                            category.CheckCatalogResult = "C";
                        else if (annualBco2CalculatedValue != null)
                        {
                            if (annualBco2CalculatedValue.Value != yearToDateTotal.Value)
                            {
                                decimal tolerance = GetQuarterlyTolerance("CO2M", "TON", category);

                                if (Math.Abs(annualBco2CalculatedValue.Value - yearToDateTotal.Value) > tolerance)
                                    category.CheckCatalogResult = "D";
                            }
                        }

                        if (category.CheckCatalogResult.IsEmpty())
                        {
                            if (currentBco2SummaryValueRecord["OS_TOTAL"].AsDecimal() != null)
                                category.CheckCatalogResult = "E";
                        }
                    }
                    else
                    {
                        if (annualBco2CalculatedValue == -1)
                            category.SetCheckParameter("Annual_BCO2_Calculated_Value", null, eParameterDataType.Decimal);
                        if (annualBco2CalculatedValue > 0)
                            category.CheckCatalogResult = "F";
                    }
                }
            }
            catch (Exception ex)
            {
                returnVal = category.CheckEngine.FormatError(ex, "HOURAGG18");
            }

            return returnVal;
        }

        #endregion

        #endregion


        #region Private Methods

        private static int? HourAgg1_GetStartQuarter(DataView view,
                                                     string dateField,
                                                     bool allowFirstQuarter,
                                                     cReportingPeriod reportingPeriod)
        {
            int? result;

            if (view.Count > 0)
            {
                view.Sort = dateField;

                DateTime methodBeginDate = view[0][dateField].AsDateTime().Default(DateTypes.START);

                if (methodBeginDate.Year < reportingPeriod.Year)
                    result = 1;
                else
                    result = methodBeginDate.Quarter();

                if (!allowFirstQuarter && (result == 1))
                    result = 2;
            }
            else
                result = null;

            return result;
        }

        private static void HourAgg1_SetStartQuarter(string startQuarterParameterName,
                                                     DataView methodView,
                                                     string dateField,
                                                     bool allowFirstQuarter,
                                                     cReportingPeriod reportingPeriod,
                                                     cCategory category)
        {
            int? startQuarter = HourAgg1_GetStartQuarter(methodView, dateField, allowFirstQuarter, reportingPeriod);

            if (startQuarter.HasValue)
                category.SetCheckParameter(startQuarterParameterName, startQuarter, eParameterDataType.Integer);
        }

        #endregion


        #region Private Methods: Utilities

        private static decimal GetQuarterlyTolerance(string AParameterCd, String AUom, cCategory ACategory)
        {
            DataView ToleranceView = (DataView)ACategory.GetCheckParameter("Quarterly_Emissions_Tolerances_Cross_Check_Table").ParameterValue;
            DataRowView ToleranceRow;
            sFilterPair[] ToleranceFilter = new sFilterPair[2];

            ToleranceFilter[0].Set("Parameter", AParameterCd);
            ToleranceFilter[1].Set("UOM", AUom);

            if (FindRow(ToleranceView, ToleranceFilter, out ToleranceRow))
                return cDBConvert.ToDecimal(ToleranceRow["Tolerance"]);
            else
                return decimal.MinValue;
        }

        public static void CompareAccumulatorValues1(string ASummaryValueRowName,
                                                    string AExpectedSummaryValueArrayParameterName,
                                                    string AAccumCalculatedValueParameterName,
                                                    string AAccumReportedValueArrayParameterName, bool OpHoursEqualZero,
                                                    string AParameterCd, string AUomCd,
                                                    cCategory ACategory)
        {
            sFilterPair[] RowFilter;

            decimal QuarterlyTolerance = GetQuarterlyTolerance(AParameterCd, AUomCd, ACategory);

            int CurrentMonitorPlanLocationPosition = ACategory.GetCheckParameter("Current_Monitor_Plan_Location_Postion").ValueAsInt();
            DataView SummaryValueRecords = ACategory.GetCheckParameter("Summary_Value_Records_By_Reporting_Period_Location").ValueAsDataView();
            decimal AccumCalcVal = cDBConvert.ToDecimal(ACategory.GetCheckParameter(AAccumCalculatedValueParameterName).ParameterValue);

            decimal[] AccumReportedValueArray = (decimal[])ACategory.GetCheckParameter(AAccumReportedValueArrayParameterName).ParameterValue;
            bool[] ExpectedSummaryValueArray = (bool[])ACategory.GetCheckParameter(AExpectedSummaryValueArrayParameterName).ParameterValue;

            RowFilter = new sFilterPair[1];
            RowFilter[0].Set("PARAMETER_CD", AParameterCd);

            DataRowView SummaryValueRow;

            if (!FindRow(SummaryValueRecords, RowFilter, out SummaryValueRow) || SummaryValueRow["CURRENT_RPT_PERIOD_TOTAL"] == DBNull.Value)
            {
                ACategory.SetCheckParameter(ASummaryValueRowName, SummaryValueRow, eParameterDataType.DataRowView);
                if (ExpectedSummaryValueArray[CurrentMonitorPlanLocationPosition])
                    ACategory.CheckCatalogResult = "C";
            }
            else
            {
                decimal SumValRecRptPerTotal = cDBConvert.ToDecimal(SummaryValueRow["CURRENT_RPT_PERIOD_TOTAL"]);
                ACategory.SetCheckParameter(ASummaryValueRowName, SummaryValueRow, eParameterDataType.DataRowView);
                if (!ExpectedSummaryValueArray[CurrentMonitorPlanLocationPosition])
                {
                    if (!OpHoursEqualZero || SumValRecRptPerTotal != 0)
                        ACategory.CheckCatalogResult = "D";
                }
                else
                {
                    decimal SummaryValue = SumValRecRptPerTotal;
                    if (SummaryValue < 0)
                        ACategory.CheckCatalogResult = "F";
                    else
                      if (Math.Round(SummaryValue, 1, MidpointRounding.AwayFromZero) != SummaryValue && SummaryValue != decimal.MinValue)//check for null redundant but included to uncouple this line from previous line
                        ACategory.CheckCatalogResult = "G";
                    else
                        if (AccumCalcVal != decimal.MinValue)
                    {
                        if (AccumCalcVal != SummaryValue)
                        {
                            if (Math.Abs(AccumCalcVal - SummaryValue) > QuarterlyTolerance)
                                ACategory.CheckCatalogResult = "A";
                            else
                                ACategory.SetCheckParameter("Emissions_Tolerance_Deviators", ACategory.GetCheckParameter("Emissions_Tolerance_Deviators").ValueAsString().ListAdd(AParameterCd), eParameterDataType.String);
                        }
                    }
                    else
                        ACategory.CheckCatalogResult = "E";
                    if (AccumReportedValueArray[CurrentMonitorPlanLocationPosition] >= 0 && string.IsNullOrEmpty(ACategory.CheckCatalogResult) &&
                        Math.Abs(AccumReportedValueArray[CurrentMonitorPlanLocationPosition] - SummaryValue) > QuarterlyTolerance)
                    {
                        string RepVal = AccumReportedValueArray[CurrentMonitorPlanLocationPosition].ToString().TrimEnd("0".ToCharArray());
                        ACategory.SetCheckParameter("Reported_Emissions_Value", RepVal, eParameterDataType.String);
                        ACategory.CheckCatalogResult = "B";
                    }
                }
            }
        }

        public static void CompareAccumulatorValues2(string ASummaryValueRowName,
                                                    string AExpectedSummaryValueArrayParameterName,
                                                    string AAccumCalculatedValueParameterName,
                                                    string AAccumReportedValueArrayParameterName,
                                                    string AAccumCalcValueArrayParameterName, bool OpHoursEqualZero,
                                                    string AParameterCd, string AToleranceParameterCd, string AUomCd,
                                                    cCategory ACategory)
        {
            sFilterPair[] RowFilter;

            decimal QuarterlyTolerance = GetQuarterlyTolerance(AToleranceParameterCd, AUomCd, ACategory);

            int CurrentMonitorPlanLocationPosition = ACategory.GetCheckParameter("Current_Monitor_Plan_Location_Postion").ValueAsInt();
            DataView SummaryValueRecords = ACategory.GetCheckParameter("Summary_Value_Records_By_Reporting_Period_Location").ValueAsDataView();
            decimal AccumCalcVal = cDBConvert.ToDecimal(ACategory.GetCheckParameter(AAccumCalculatedValueParameterName).ParameterValue);

            decimal[] AccumReportedValueArray = (decimal[])ACategory.GetCheckParameter(AAccumReportedValueArrayParameterName).ParameterValue;
            bool[] ExpectedSummaryValueArray = (bool[])ACategory.GetCheckParameter(AExpectedSummaryValueArrayParameterName).ParameterValue;

            RowFilter = new sFilterPair[1];
            RowFilter[0].Set("PARAMETER_CD", AParameterCd);

            DataRowView SummaryValueRow;

            if (AccumReportedValueArray[CurrentMonitorPlanLocationPosition] >= 0)
                AccumReportedValueArray[CurrentMonitorPlanLocationPosition] = Math.Round(AccumReportedValueArray[CurrentMonitorPlanLocationPosition], 1, MidpointRounding.AwayFromZero);

            if (!FindRow(SummaryValueRecords, RowFilter, out SummaryValueRow) || SummaryValueRow["Current_Rpt_Period_Total"] == DBNull.Value)
            {
                ACategory.SetCheckParameter(ASummaryValueRowName, SummaryValueRow, eParameterDataType.DataRowView);
                if (ExpectedSummaryValueArray[CurrentMonitorPlanLocationPosition])
                    ACategory.CheckCatalogResult = "C";
            }
            else
            {
                decimal SumValRecRptPerTotal = cDBConvert.ToDecimal(SummaryValueRow["CURRENT_RPT_PERIOD_TOTAL"]);
                ACategory.SetCheckParameter(ASummaryValueRowName, SummaryValueRow, eParameterDataType.DataRowView);
                if (!ExpectedSummaryValueArray[CurrentMonitorPlanLocationPosition])
                {
                    if (!OpHoursEqualZero || SumValRecRptPerTotal != 0)
                        ACategory.CheckCatalogResult = "D";
                }
                else
                {
                    decimal SummaryValue = SumValRecRptPerTotal;
                    if (SummaryValue < 0)
                        ACategory.CheckCatalogResult = "F";
                    else
                      if (Math.Round(SummaryValue, 1, MidpointRounding.AwayFromZero) != SummaryValue && SummaryValue != decimal.MinValue)//check for null redundant but included to uncouple this line from previous line)
                        ACategory.CheckCatalogResult = "G";
                    else
                        if (AccumCalcVal != decimal.MinValue)
                    {
                        if (AccumCalcVal != SummaryValue)
                        {
                            if (Math.Abs(AccumCalcVal - SummaryValue) > QuarterlyTolerance)
                                ACategory.CheckCatalogResult = "A";
                            else
                                ACategory.SetCheckParameter("Emissions_Tolerance_Deviators", ACategory.GetCheckParameter("Emissions_Tolerance_Deviators").ValueAsString().ListAdd(AParameterCd), eParameterDataType.String);
                        }
                    }
                    else
                    {
                        decimal[] AccumCalcValueArray = (decimal[])ACategory.GetCheckParameter(AAccumCalcValueArrayParameterName).ParameterValue;
                        if (AccumCalcValueArray[CurrentMonitorPlanLocationPosition] == -1)
                            ACategory.CheckCatalogResult = "E";
                    }
                    if (AccumReportedValueArray[CurrentMonitorPlanLocationPosition] >= 0 && string.IsNullOrEmpty(ACategory.CheckCatalogResult) &&
                        Math.Abs(AccumReportedValueArray[CurrentMonitorPlanLocationPosition] - SummaryValue) > QuarterlyTolerance)
                    {
                        string RepVal = AccumReportedValueArray[CurrentMonitorPlanLocationPosition].ToString().TrimEnd("0".ToCharArray());
                        ACategory.SetCheckParameter("Reported_Emissions_Value", RepVal, eParameterDataType.String);
                        ACategory.CheckCatalogResult = "B";
                    }
                }
            }
        }

        public static void CompareAccumulatorValues3(string ASummaryValueRowName,
                                                    string AExpectedSummaryValueArrayParameterName,
                                                    string AAccumCalculatedValueParameterName,
                                                    string AAccumReportedValueArrayParameterName, bool OpHoursEqualZero,
                                                    string AParameterCd, string AUomCd,
                                                    int decimalPlaces,
                                                    cCategory ACategory)
        {
            sFilterPair[] RowFilter;

            decimal QuarterlyTolerance = GetQuarterlyTolerance(AParameterCd, AUomCd, ACategory);

            int CurrentMonitorPlanLocationPosition = ACategory.GetCheckParameter("Current_Monitor_Plan_Location_Postion").ValueAsInt();
            DataView SummaryValueRecords = ACategory.GetCheckParameter("Summary_Value_Records_By_Reporting_Period_Location").ValueAsDataView();
            decimal AccumCalcVal = cDBConvert.ToDecimal(ACategory.GetCheckParameter(AAccumCalculatedValueParameterName).ParameterValue);

            decimal[] AccumReportedValueArray = (decimal[])ACategory.GetCheckParameter(AAccumReportedValueArrayParameterName).ParameterValue;
            bool[] ExpectedSummaryValueArray = (bool[])ACategory.GetCheckParameter(AExpectedSummaryValueArrayParameterName).ParameterValue;

            RowFilter = new sFilterPair[1];
            RowFilter[0].Set("PARAMETER_CD", AParameterCd);

            DataRowView SummaryValueRow;

            if (AccumReportedValueArray[CurrentMonitorPlanLocationPosition] >= 0)
                AccumReportedValueArray[CurrentMonitorPlanLocationPosition] = Math.Round(AccumReportedValueArray[CurrentMonitorPlanLocationPosition], 1, MidpointRounding.AwayFromZero);

            if (!FindRow(SummaryValueRecords, RowFilter, out SummaryValueRow) || SummaryValueRow["CURRENT_RPT_PERIOD_TOTAL"] == DBNull.Value)
            {
                ACategory.SetCheckParameter(ASummaryValueRowName, SummaryValueRow, eParameterDataType.DataRowView);
                if (ExpectedSummaryValueArray[CurrentMonitorPlanLocationPosition])
                    ACategory.CheckCatalogResult = "C";
            }
            else
            {
                decimal SumValRecRptPerTotal = cDBConvert.ToDecimal(SummaryValueRow["CURRENT_RPT_PERIOD_TOTAL"]);
                ACategory.SetCheckParameter(ASummaryValueRowName, SummaryValueRow, eParameterDataType.DataRowView);
                if (!ExpectedSummaryValueArray[CurrentMonitorPlanLocationPosition])
                {
                    if (!OpHoursEqualZero || SumValRecRptPerTotal != 0)
                        ACategory.CheckCatalogResult = "D";
                }
                else
                {
                    decimal SummaryValue = SumValRecRptPerTotal;
                    if (SummaryValue < 0)
                        ACategory.CheckCatalogResult = "F";
                    else
                      if (Math.Round(SummaryValue, decimalPlaces, MidpointRounding.AwayFromZero) != SummaryValue && SummaryValue != decimal.MinValue)//check for null redundant but included to uncouple this line from previous line)
                        ACategory.CheckCatalogResult = "G";
                    else
                        if (AccumCalcVal != decimal.MinValue)
                    {
                        if (AccumCalcVal != SummaryValue)
                        {
                            if (Math.Abs(AccumCalcVal - SummaryValue) > QuarterlyTolerance)
                                ACategory.CheckCatalogResult = "A";
                            else
                                ACategory.SetCheckParameter("Emissions_Tolerance_Deviators",
                                  ACategory.GetCheckParameter("Emissions_Tolerance_Deviators").ValueAsString().ListAdd(AParameterCd),
                                  eParameterDataType.String);
                        }
                    }
                    else
                        ACategory.CheckCatalogResult = "E";
                    if (AccumReportedValueArray[CurrentMonitorPlanLocationPosition] >= 0 && string.IsNullOrEmpty(ACategory.CheckCatalogResult) &&
                        Math.Abs(Math.Round(AccumReportedValueArray[CurrentMonitorPlanLocationPosition], MidpointRounding.AwayFromZero) - SummaryValue) > QuarterlyTolerance)
                    {
                        string RepVal = AccumReportedValueArray[CurrentMonitorPlanLocationPosition].ToString().TrimEnd("0".ToCharArray());
                        ACategory.SetCheckParameter("Reported_Emissions_Value", RepVal, eParameterDataType.String);
                        ACategory.CheckCatalogResult = "B";
                    }
                }
            }
        }

        public static void HourAgg7_CompareAccumValues(string ASummaryValueRowName,
                                                       string AExpectedSummaryValueArrayParameterName,
                                                       string AAccumCalculatedValueParameterName,
                                                       string AAccumReportedValueArrayParameterName, bool OpHoursEqualZero,
                                                       string AParameterCd, string AUomCd,
                                                       cCategory ACategory)
        {
            sFilterPair[] RowFilter;

            decimal QuarterlyTolerance = GetQuarterlyTolerance(AParameterCd, AUomCd, ACategory);

            int CurrentMonitorPlanLocationPosition = ACategory.GetCheckParameter("Current_Monitor_Plan_Location_Postion").ValueAsInt();
            DataView SummaryValueRecords = ACategory.GetCheckParameter("Summary_Value_Records_By_Reporting_Period_Location").ValueAsDataView();
            decimal AccumCalcVal = cDBConvert.ToDecimal(ACategory.GetCheckParameter(AAccumCalculatedValueParameterName).ParameterValue);

            decimal[] AccumReportedValueArray = (decimal[])ACategory.GetCheckParameter(AAccumReportedValueArrayParameterName).ParameterValue;
            bool[] ExpectedSummaryValueArray = (bool[])ACategory.GetCheckParameter(AExpectedSummaryValueArrayParameterName).ParameterValue;

            RowFilter = new sFilterPair[1];
            RowFilter[0].Set("PARAMETER_CD", AParameterCd);

            DataRowView SummaryValueRow;

            if (!FindRow(SummaryValueRecords, RowFilter, out SummaryValueRow) || SummaryValueRow["CURRENT_RPT_PERIOD_TOTAL"] == DBNull.Value)
            {
                ACategory.SetCheckParameter(ASummaryValueRowName, SummaryValueRow, eParameterDataType.DataRowView);
                if (ExpectedSummaryValueArray[CurrentMonitorPlanLocationPosition])
                    ACategory.CheckCatalogResult = "C";
            }
            else
            {
                decimal SumValRecRptPerTotal = cDBConvert.ToDecimal(SummaryValueRow["CURRENT_RPT_PERIOD_TOTAL"]);
                ACategory.SetCheckParameter(ASummaryValueRowName, SummaryValueRow, eParameterDataType.DataRowView);
                if (!ExpectedSummaryValueArray[CurrentMonitorPlanLocationPosition])
                {
                    if (!OpHoursEqualZero || SumValRecRptPerTotal != decimal.MinValue)
                        ACategory.CheckCatalogResult = "D";
                }
                else
                {
                    decimal SummaryValue = SumValRecRptPerTotal;
                    if (SummaryValue < 0)
                        ACategory.CheckCatalogResult = "F";
                    else
                      if (AccumCalcVal != decimal.MinValue)
                    {
                        if (Math.Abs(AccumCalcVal - SummaryValue) > QuarterlyTolerance)
                            ACategory.CheckCatalogResult = "A";
                    }
                    else
                        ACategory.CheckCatalogResult = "E";
                    if (!ACategory.GetCheckParameter("LME_Annual").ValueAsBool() && string.IsNullOrEmpty(ACategory.CheckCatalogResult))
                    {
                        DataRowView CurrentMonitorPlanLocationRecord = ACategory.GetCheckParameter("Current_Monitor_Plan_Location_Record").ValueAsDataRowView();
                        if (cDBConvert.ToString(CurrentMonitorPlanLocationRecord["Location_Name"]).PadRight(2).Substring(0, 2) == "MS" ||
                              !cDBConvert.ToBoolean(ACategory.GetCheckParameter("Multiple_Stack_Configuration").ParameterValue))

                            if (AccumReportedValueArray[CurrentMonitorPlanLocationPosition] >= 0 && Math.Abs(AccumReportedValueArray[CurrentMonitorPlanLocationPosition] - SummaryValue) > QuarterlyTolerance)
                            {
                                string RepVal = AccumReportedValueArray[CurrentMonitorPlanLocationPosition].ToString().TrimEnd("0".ToCharArray());
                                ACategory.SetCheckParameter("Reported_Emissions_Value", RepVal, eParameterDataType.String);
                                ACategory.CheckCatalogResult = "B";
                            }
                    }
                }
            }
        }

        #endregion

    }
}
