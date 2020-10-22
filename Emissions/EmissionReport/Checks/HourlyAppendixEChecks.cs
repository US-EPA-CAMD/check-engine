using System;
using System.Collections.Generic;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.CheckEngine.SpecialParameterClasses;
using ECMPS.Checks.Em.Parameters;
using ECMPS.Checks.EmissionsReport;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;
using ECMPS.Definitions.Extensions;

namespace
    ECMPS.Checks.EmissionsChecks
{
    public class cHourlyAppendixEChecks : cEmissionsChecks
    {
        #region Constructors

        public cHourlyAppendixEChecks(cEmissionsReportProcess emissionReportProcess)
          : base(emissionReportProcess)
        {
            CheckProcedures = new dCheckProcedure[17];

            CheckProcedures[1] = new dCheckProcedure(HOURAE1);
            CheckProcedures[2] = new dCheckProcedure(HOURAE2);
            CheckProcedures[3] = new dCheckProcedure(HOURAE3);
            CheckProcedures[4] = new dCheckProcedure(HOURAE4);
            CheckProcedures[5] = new dCheckProcedure(HOURAE5);
            CheckProcedures[6] = new dCheckProcedure(HOURAE6);
            CheckProcedures[7] = new dCheckProcedure(HOURAE7);
            CheckProcedures[8] = new dCheckProcedure(HOURAE8);
            CheckProcedures[9] = new dCheckProcedure(HOURAE9);
            CheckProcedures[10] = new dCheckProcedure(HOURAE10);

            CheckProcedures[11] = new dCheckProcedure(HOURAE11);
            CheckProcedures[13] = new dCheckProcedure(HOURAE13);
            CheckProcedures[14] = new dCheckProcedure(HOURAE14);
            CheckProcedures[15] = new dCheckProcedure(HOURAE15);
            CheckProcedures[16] = new dCheckProcedure(HOURAE16);
        }

        #endregion


        #region Public Static Methods: Checks

        public static string HOURAE1(cCategory Category, ref bool Log)
        //Initialize AE Reporting Method
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("App_E_Reporting_Method", null, eParameterDataType.String);
                Category.SetCheckParameter("App_E_Op_Code", null, eParameterDataType.String);
                Category.SetCheckParameter("App_E_Segment_Number", null, eParameterDataType.Integer);
                Category.SetCheckParameter("App_E_Reported_Value", null, eParameterDataType.Decimal);
                Category.SetCheckParameter("App_E_Fuel_Code", null, eParameterDataType.String);
                Category.SetCheckParameter("App_E_Calc_Hi", null, eParameterDataType.Decimal);

                if (Category.GetCheckParameter("Current_NOxR_Method_Code").ValueAsString() == "AE")
                {
                    int TotalFuelSources = Category.GetCheckParameter("Hourly_Fuel_Flow_Count_For_Gas").ValueAsInt(0)
                                         + Category.GetCheckParameter("Hourly_Fuel_Flow_Count_For_Oil").ValueAsInt(0);

                    if (TotalFuelSources > 1)
                        Category.SetCheckParameter("App_E_Reporting_Method", "MULTIPLE", eParameterDataType.String);
                    else if (TotalFuelSources == 1)
                        Category.SetCheckParameter("App_E_Reporting_Method", "SINGLE", eParameterDataType.String);
                }
                else
                    Log = false;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURAE1");
            }

            return ReturnVal;
        }

        public static string HOURAE2(cCategory Category, ref bool Log)
        // Validate NOXR Record
        {
            string ReturnVal = "";

            try
            {
                sFilterPair[] RowFilter;

                Category.SetCheckParameter("Current_App_E_Noxr_Record", null, eParameterDataType.DataRowView);

                string AppeReportingMethod = Category.GetCheckParameter("App_E_Reporting_Method").ValueAsString();
                DataRowView CurrentFuelFlowRecord = Category.GetCheckParameter("Current_Fuel_Flow_Record").ValueAsDataRowView();

                RowFilter = new sFilterPair[2];
                RowFilter[0].Set("HRLY_FUEL_FLOW_ID", cDBConvert.ToString(CurrentFuelFlowRecord["HRLY_FUEL_FLOW_ID"]));
                RowFilter[1].Set("PARAMETER_CD", "NOXR");

                DataView HourlyParamFuelFlowRecords
                  = FindRows(Category.GetCheckParameter("Hourly_Param_Fuel_Flow_Records_For_Current_Fuel_Flow").ValueAsDataView(),
                                                        RowFilter);

                if (HourlyParamFuelFlowRecords.Count == 0)
                {
                    if (AppeReportingMethod.InList("MULTIPLE,SINGLE"))
                    {
                        Category.SetCheckParameter("Noxr_App_E_Accumulator", -1m, eParameterDataType.Decimal);
                        Category.CheckCatalogResult = "A";
                    }
                }
                else if (HourlyParamFuelFlowRecords.Count > 1)
                {
                    if (AppeReportingMethod.InList("MULTIPLE,SINGLE"))
                    {
                        Category.SetCheckParameter("Noxr_App_E_Accumulator", -1m, eParameterDataType.Decimal);
                        Category.CheckCatalogResult = "B";
                    }
                    else
                        Category.CheckCatalogResult = "D";
                }
                else if (AppeReportingMethod.InList("MULTIPLE,SINGLE"))
                {
                    DataRowView CurrentAppeNoxrRecord = HourlyParamFuelFlowRecords[0];

                    Category.SetCheckParameter("Current_App_E_Noxr_Record", CurrentAppeNoxrRecord, eParameterDataType.DataRowView);
                    Category.SetCheckParameter("App_E_Segment_Number", cDBConvert.ToInteger(CurrentAppeNoxrRecord["SEGMENT_NUM"]), eParameterDataType.Integer);
                    Category.SetCheckParameter("App_E_Reported_Value", cDBConvert.ToDecimal(CurrentAppeNoxrRecord["PARAM_VAL_FUEL"]), eParameterDataType.Decimal);
                    Category.SetCheckParameter("App_E_Calc_Hi", Category.GetCheckParameter("HFF_Calc_HI_Rate").ValueAsDecimal(), eParameterDataType.Decimal);
                    Category.SetCheckParameter("App_E_Fuel_Code", cDBConvert.ToString(CurrentFuelFlowRecord["FUEL_CD"]), eParameterDataType.String);

                    string OperatingConditionCd = cDBConvert.ToString(CurrentAppeNoxrRecord["OPERATING_CONDITION_CD"]);

                    if (OperatingConditionCd.InList("E,X,Y,Z,U,W,N,M"))
                        Category.SetCheckParameter("App_E_Op_Code", OperatingConditionCd, eParameterDataType.String);
                    else
                    {
                        Category.SetCheckParameter("App_E_Op_Code", null, eParameterDataType.String);
                        Category.CheckCatalogResult = "C";
                    }
                }
                else if (AppeReportingMethod == "CONSTANT")
                    Category.CheckCatalogResult = "D";
                else
                    Category.CheckCatalogResult = "E";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURAE2");
            }

            return ReturnVal;
        }

        public static string HOURAE4(cCategory Category, ref bool Log)
        // Check for Extraneous Fields in NOXR Record
        {
            string ReturnVal = "";

            try
            {
                string HourlyExtraneousFields = "";

                DataRowView CurrentAppeNoxrRecord = Category.GetCheckParameter("Current_App_E_Noxr_Record").ValueAsDataRowView();

                if (CurrentAppeNoxrRecord != null)
                {
                    if (CurrentAppeNoxrRecord["SAMPLE_TYPE_CD"] != DBNull.Value)
                        HourlyExtraneousFields = HourlyExtraneousFields.ListAdd("SampleTypeCode");

                    if (CurrentAppeNoxrRecord["MON_FORM_ID"] != DBNull.Value)
                        HourlyExtraneousFields = HourlyExtraneousFields.ListAdd("MonitoringFormulaID");

                    if (HourlyExtraneousFields != "")
                        Category.CheckCatalogResult = "A";
                }

                Category.SetCheckParameter("Hourly_Extraneous_Fields", HourlyExtraneousFields.FormatList(), eParameterDataType.String);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURAE4");
            }

            return ReturnVal;
        }

        public static string HOURAE5(cCategory category, ref bool log)
        // Check Monitoring System Data for Appendix E NOXR
        {
            string returnVal = "";

            try
            {
                DataRowView currentAppeNoxrRecord = category.GetCheckParameter("Current_App_E_Noxr_Record").ValueAsDataRowView();

                if (currentAppeNoxrRecord != null)
                {
                    DataRowView currentFuelFlowRecord = category.GetCheckParameter("Current_Fuel_Flow_Record").ValueAsDataRowView();

                    category.SetCheckParameter("App_E_NOXE_System_ID", null, eParameterDataType.String);
                    category.SetCheckParameter("App_E_NOXE_System_Identifier", null, eParameterDataType.String);

                    string monitoringSysemId = currentAppeNoxrRecord["MON_SYS_ID"].AsString();

                    if (monitoringSysemId.IsNull())
                    {
                        if (currentAppeNoxrRecord["OPERATING_CONDITION_CD"].AsString() == "E")
                        {
                            if (category.GetCheckParameter("HFF_Fuel_Indicator_Code").AsString() != "E")
                                category.CheckCatalogResult = "A";
                        }
                        else
                            category.CheckCatalogResult = "B";
                    }
                    else
                    {
                        DataRowView currentAppeNoxrMonSysRecord
                          = cRowFilter.FindRow(category.GetCheckParameter("Monitor_System_Records_By_Hour_Location").AsDataView(),
                                               new cFilterCondition[] { new cFilterCondition("MON_SYS_ID", monitoringSysemId) });

                        if (currentAppeNoxrMonSysRecord == null)
                            category.CheckCatalogResult = "C";
                        else if (currentAppeNoxrMonSysRecord["SYS_TYPE_CD"].AsString() != "NOXE")
                            category.CheckCatalogResult = "D";
                        else if (currentAppeNoxrMonSysRecord["FUEL_CD"].AsString()
                                 != currentFuelFlowRecord["FUEL_CD"].AsString())
                            category.CheckCatalogResult = "E";
                        else
                        {
                            category.SetCheckParameter("App_E_NOXE_System_ID", monitoringSysemId, eParameterDataType.String);
                            category.SetCheckParameter("App_E_NOXE_System_Identifier", cDBConvert.ToString(currentAppeNoxrMonSysRecord["SYSTEM_IDENTIFIER"]), eParameterDataType.String);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                returnVal = category.CheckEngine.FormatError(ex, "HOURAE5");
            }

            return returnVal;
        }

        public static string HOURAE7(cCategory Category, ref bool Log)
        // Retrieve Appendix E Correlation Test Results or Default  Value
        {
            string ReturnVal = "";

            try
            {
                sFilterPair[] RowFilter;

                decimal MaximumAppeCurveNoxEmissionRate = decimal.MinValue;

                Category.SetCheckParameter("Maximum_App_E_Curve_Nox_Emission_Rate", MaximumAppeCurveNoxEmissionRate, eParameterDataType.Decimal);
                Category.SetCheckParameter("App_E_NOx_MER", null, eParameterDataType.Decimal);
                Category.SetCheckParameter("App_E_Segment_Total", null, eParameterDataType.Decimal);

                string AppeOpCode = Category.GetCheckParameter("App_E_Op_Code").ValueAsString();

                if (AppeOpCode.InList("N,W,X,Y,Z"))
                {
                    string AppEStatus = Category.GetCheckParameter("Current_Appendix_E_Status").ValueAsString();
                    if (AppEStatus.StartsWith("IC") || AppEStatus.StartsWith("Undetermined"))
                    {
                        DataRowView PriorAppERec = Category.GetCheckParameter("Prior_Appendix_E_Record").ValueAsDataRowView();

                        DataView QaSupplementalAttributeRecords = Category.GetCheckParameter("QA_Supplemental_Attribute_Records").ValueAsDataView();
                        DataRowView QaSuppAttributeRecord;

                        RowFilter = new sFilterPair[2];
                        RowFilter[0].Set("QA_SUPP_DATA_ID", cDBConvert.ToString(PriorAppERec["QA_SUPP_DATA_ID"]));
                        RowFilter[1].Set("ATTRIBUTE_NAME", "SEGMENT_COUNT");

                        QaSuppAttributeRecord = FindRow(QaSupplementalAttributeRecords, RowFilter);

                        if (QaSuppAttributeRecord != null)
                        {
                            int AppeSegmentTotal = cDBConvert.ToInteger(QaSuppAttributeRecord["ATTRIBUTE_VALUE"]);
                            Category.SetCheckParameter("App_E_Segment_Total", AppeSegmentTotal, eParameterDataType.Integer);
                            decimal[] AppeCorrelationNoxrArray = new decimal[AppeSegmentTotal + 1];
                            decimal[] AppeCorrelationHiArray = new decimal[AppeSegmentTotal + 1];

                            Category.SetCheckParameter("App_E_Correlation_Nox_Rate_Array", AppeCorrelationNoxrArray, eParameterDataType.Decimal, false, true);
                            Category.SetCheckParameter("App_E_Correlation_Heat_Input_Array", AppeCorrelationHiArray, eParameterDataType.Decimal, false, true);

                            for (int Segment = 1; Segment <= AppeSegmentTotal; Segment++)
                            {
                                // Get NOxR value for segment
                                RowFilter[1].Set("ATTRIBUTE_NAME", "NOX_RATE_" + cDBConvert.ToString(Segment));

                                QaSuppAttributeRecord = FindRow(QaSupplementalAttributeRecords, RowFilter);

                                if (QaSuppAttributeRecord != null)
                                {
                                    decimal AttributeValue = cDBConvert.ToDecimal(QaSuppAttributeRecord["ATTRIBUTE_VALUE"]);

                                    if (AttributeValue > MaximumAppeCurveNoxEmissionRate)
                                    {
                                        MaximumAppeCurveNoxEmissionRate = AttributeValue;
                                        Category.SetCheckParameter("Maximum_App_E_Curve_Nox_Emission_Rate", MaximumAppeCurveNoxEmissionRate, eParameterDataType.Decimal);
                                    }
                                    Category.SetArrayParameter("App_E_Correlation_Nox_Rate_Array", Segment, AttributeValue);
                                }


                                // Get HI value for segment
                                RowFilter[1].Set("ATTRIBUTE_NAME", "HI_RATE_" + cDBConvert.ToString(Segment));

                                QaSuppAttributeRecord = FindRow(QaSupplementalAttributeRecords, RowFilter);

                                if (QaSuppAttributeRecord != null)
                                {
                                    decimal AttributeValue = cDBConvert.ToDecimal(QaSuppAttributeRecord["ATTRIBUTE_VALUE"]);
                                    Category.SetArrayParameter("App_E_Correlation_Heat_Input_Array", Segment, AttributeValue);
                                }

                            }
                        }
                    }
                }
                else if (AppeOpCode.InList("E,M,U"))
                {
                    DataView MonitorDefaultRecords = Category.GetCheckParameter("Monitor_Default_Records_by_Hour_Location").ValueAsDataView();

                    RowFilter = new sFilterPair[3];
                    RowFilter[0].Set("PARAMETER_CD", "NORX");
                    RowFilter[1].Set("DEFAULT_PURPOSE_CD", "MD");
                    RowFilter[2].Set("FUEL_CD", Category.GetCheckParameter("App_E_Fuel_Code").ValueAsString());

                    DataView AppeNoxMerDefaultView = FindRows(MonitorDefaultRecords, RowFilter);

                    if (AppeNoxMerDefaultView.Count != 1)
                        Category.CheckCatalogResult = "A";
                    else
                    {
                        decimal DefaultValue = cDBConvert.ToDecimal(AppeNoxMerDefaultView[0]["DEFAULT_VALUE"]);

                        if (DefaultValue > 0)
                            Category.SetCheckParameter("App_E_NOx_MER", DefaultValue, eParameterDataType.Decimal);
                        else
                            Category.CheckCatalogResult = "B";
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURAE7");
            }

            return ReturnVal;
        }

        public static string HOURAE8(cCategory Category, ref bool Log)
        // Determine Appendix E Curve Segment
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("App_E_Calc_Segment_Num", null, eParameterDataType.Integer);

                string AppeOpCode = Category.GetCheckParameter("App_E_Op_Code").ValueAsString();
                int AppeSegmentTotal = Category.GetCheckParameter("App_E_Segment_Total").ValueAsInt();
                int AppeSegmentNumber = Category.GetCheckParameter("App_E_Segment_Number").ValueAsInt();

                if (AppeOpCode != "")
                {
                    if (AppeOpCode.InList("E,U,M,W"))
                    {
                        if (AppeSegmentNumber != int.MinValue)
                            Category.CheckCatalogResult = "A";
                    }
                    else if (AppeOpCode.InList("N,X"))
                    {
                        if (AppeSegmentTotal != int.MinValue)
                        {
                            if (AppeSegmentNumber != int.MinValue)
                            {
                                if ((int)Category.GetCheckParameter("App_E_Correlation_Nox_Rate_Array").ValueAsDecimalArray().Length - 1 < AppeSegmentNumber)
                                    Category.CheckCatalogResult = "B";
                                else if (Category.GetCheckParameter("App_E_Correlation_Nox_Rate_Array").ValueAsDecimalArray()[AppeSegmentNumber]
                                      != Category.GetCheckParameter("Maximum_App_E_Curve_Nox_Emission_Rate").ValueAsDecimal())
                                    Category.CheckCatalogResult = "B";
                            }
                            else
                            {
                                if (!Category.GetCheckParameter("Legacy_Data_Evaluation").ValueAsBool())
                                    Category.CheckCatalogResult = "G";
                            }
                        }
                    }
                    else if (AppeOpCode.InList("Y,Z"))
                    {
                        decimal AppeCalcHi = Category.GetCheckParameter("App_E_Calc_Hi").ValueAsDecimal();

                        if ((AppeCalcHi != decimal.MinValue) && (AppeSegmentTotal != int.MinValue))
                        {
                            decimal[] AppeCorrelationHeatInputRateArray = Category.GetCheckParameter("App_E_Correlation_Heat_Input_Array").ValueAsDecimalArray();
                            int AppeCalcSegmentNumber = 1;

                            while ((AppeCalcSegmentNumber <= AppeSegmentTotal) && (AppeCalcHi > AppeCorrelationHeatInputRateArray[AppeCalcSegmentNumber]))
                                AppeCalcSegmentNumber += 1;

                            if ((AppeCalcSegmentNumber <= AppeSegmentTotal) && (AppeCalcHi <= AppeCorrelationHeatInputRateArray[AppeCalcSegmentNumber]))
                            {
                                Category.SetCheckParameter("App_E_Calc_Segment_Num", AppeCalcSegmentNumber, eParameterDataType.Integer);

                                if (AppeOpCode == "Z")
                                {
                                    if (AppeCalcSegmentNumber != 1)
                                        Category.CheckCatalogResult = "C";
                                    else if (AppeSegmentNumber == int.MinValue)
                                    {
                                        if (!Category.GetCheckParameter("Legacy_Data_Evaluation").ValueAsBool())
                                            Category.CheckCatalogResult = "G";
                                    }
                                    else if (AppeSegmentNumber != 1)
                                        Category.CheckCatalogResult = "D";
                                }
                                else if (AppeSegmentNumber == int.MinValue)
                                {
                                    if (!Category.GetCheckParameter("Legacy_Data_Evaluation").ValueAsBool())
                                        Category.CheckCatalogResult = "G";
                                }
                                else if (AppeCalcHi == AppeCorrelationHeatInputRateArray[AppeCalcSegmentNumber])
                                {
                                    if ((AppeSegmentNumber != AppeCalcSegmentNumber) &&
                                        (AppeSegmentNumber != (AppeCalcSegmentNumber + 1)))
                                        Category.CheckCatalogResult = "E";
                                }
                                else
                                {
                                    if (AppeSegmentNumber != AppeCalcSegmentNumber)
                                        Category.CheckCatalogResult = "E";
                                }
                            }
                            else
                                Category.CheckCatalogResult = "F";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURAE8");
            }

            return ReturnVal;
        }

        public static string HOURAE9(cCategory Category, ref bool Log)
        // Validate Appendix E NOXR Calculation
        {
            string ReturnVal = "";

            try
            {
                sFilterPair[] RowFilter;

                decimal AppeCalcHi = Category.GetCheckParameter("App_E_Calc_Hi").ValueAsDecimal();
                string AppeFuelCode = Category.GetCheckParameter("App_E_Fuel_Code").ValueAsString();
                string AppeOpCode = Category.GetCheckParameter("App_E_Op_Code").ValueAsString();
                decimal AppeReportedValue = Category.GetCheckParameter("App_E_Reported_Value").ValueAsDecimal();
                string AppeRepMethod = Category.GetCheckParameter("App_E_Reporting_Method").ValueAsString();

                decimal AppeCalculatedNoxrForSource = decimal.MinValue;
                Category.SetCheckParameter("App_E_Calculated_Nox_Rate_For_Source", null, eParameterDataType.Decimal);

                if (AppeOpCode.InList("Y,Z"))
                {
                    int AppeCalcSegmentNumber = Category.GetCheckParameter("App_E_Calc_Segment_Num").ValueAsInt();

                    if (AppeCalcSegmentNumber != int.MinValue)
                    {
                        decimal[] AppeCorrelationNoxrArray = Category.GetCheckParameter("App_E_Correlation_Nox_Rate_Array").ValueAsDecimalArray();
                        decimal[] AppeCorrelationHiArray = Category.GetCheckParameter("App_E_Correlation_Heat_Input_Array").ValueAsDecimalArray();

                        if (AppeCalcSegmentNumber == 1)
                        {
                            AppeCalculatedNoxrForSource = AppeCorrelationNoxrArray[AppeCalcSegmentNumber];
                            Category.SetCheckParameter("App_E_Calculated_Nox_Rate_For_Source", AppeCalculatedNoxrForSource, eParameterDataType.Decimal);
                        }
                        else
                        {
                            decimal Noxr2 = AppeCorrelationNoxrArray[AppeCalcSegmentNumber];
                            decimal Hi2 = AppeCorrelationHiArray[AppeCalcSegmentNumber];
                            decimal Noxr1 = AppeCorrelationNoxrArray[AppeCalcSegmentNumber - 1];
                            decimal Hi1 = AppeCorrelationHiArray[AppeCalcSegmentNumber - 1];

                            AppeCalculatedNoxrForSource = Math.Round(((AppeCalcHi - Hi1) * (Noxr2 - Noxr1) / (Hi2 - Hi1)) + Noxr1, 3, MidpointRounding.AwayFromZero);
                            Category.SetCheckParameter("App_E_Calculated_Nox_Rate_For_Source", AppeCalculatedNoxrForSource, eParameterDataType.Decimal);
                        }
                    }
                }
                else if (AppeOpCode.InList("N,X"))
                {
                    AppeCalculatedNoxrForSource = Category.GetCheckParameter("Maximum_App_E_Curve_Nox_Emission_Rate").ValueAsDecimal();
                    Category.SetCheckParameter("App_E_Calculated_Nox_Rate_For_Source", AppeCalculatedNoxrForSource, eParameterDataType.Decimal);
                }
                else if (AppeOpCode.InList("E,M,U"))
                {
                    AppeCalculatedNoxrForSource = Category.GetCheckParameter("App_E_NOx_MER").ValueAsDecimal();
                    Category.SetCheckParameter("App_E_Calculated_Nox_Rate_For_Source", AppeCalculatedNoxrForSource, eParameterDataType.Decimal);
                }
                else if (AppeOpCode == "W")
                {
                    decimal MaximumAppeCurveNoxEmissionRate = Category.GetCheckParameter("Maximum_App_E_Curve_Nox_Emission_Rate").ValueAsDecimal();

                    if (MaximumAppeCurveNoxEmissionRate != decimal.MinValue && AppeReportedValue >= 0 && Math.Round(AppeReportedValue, 3, MidpointRounding.AwayFromZero) == AppeReportedValue)
                    {
                        if (AppeReportedValue >= Math.Round(MaximumAppeCurveNoxEmissionRate * 1.25m, 3, MidpointRounding.AwayFromZero))
                        {
                            AppeCalculatedNoxrForSource = AppeReportedValue;
                            Category.SetCheckParameter("App_E_Calculated_Nox_Rate_For_Source", AppeCalculatedNoxrForSource, eParameterDataType.Decimal);
                        }
                        else
                        {
                            DataView MonitorDefaultRecords = Category.GetCheckParameter("Monitor_Default_Records_by_Hour_Location").ValueAsDataView();

                            RowFilter = new sFilterPair[3];
                            RowFilter[0].Set("PARAMETER_CD", "NORX");
                            RowFilter[1].Set("DEFAULT_PURPOSE_CD", "MD");
                            RowFilter[2].Set("FUEL_CD", Category.GetCheckParameter("App_E_Fuel_Code").ValueAsString());

                            DataView NoxMerDefaultView = FindRows(MonitorDefaultRecords, RowFilter);

                            if (NoxMerDefaultView.Count != 1)
                                Category.CheckCatalogResult = "A";
                            else
                            {
                                decimal DefaultValue = cDBConvert.ToDecimal(NoxMerDefaultView[0]["DEFAULT_VALUE"]);

                                if (DefaultValue > 0)
                                {
                                    if (AppeReportedValue >= DefaultValue)
                                    {
                                        AppeCalculatedNoxrForSource = AppeReportedValue;
                                        Category.SetCheckParameter("App_E_Calculated_Nox_Rate_For_Source", AppeCalculatedNoxrForSource, eParameterDataType.Decimal);
                                    }
                                    else
                                    {
                                        if (AppeRepMethod == "CONSTANT" || AppeRepMethod == "APPORTIONED")
                                            Category.CheckCatalogResult = "B";
                                        else
                                        {
                                            Category.SetCheckParameter("Noxr_App_E_Accumulator", -1, eParameterDataType.Decimal);
                                            Category.CheckCatalogResult = "C";
                                        }
                                    }
                                }
                                else
                                  if (AppeRepMethod == "CONSTANT" || AppeRepMethod == "APPORTIONED")
                                    Category.CheckCatalogResult = "D";
                                else
                                {
                                    Category.SetCheckParameter("Noxr_App_E_Accumulator", -1, eParameterDataType.Decimal);
                                    Category.CheckCatalogResult = "D";
                                }
                            }
                        }
                    }
                }

                if (string.IsNullOrEmpty(Category.CheckCatalogResult))
                {
                    if (AppeCalculatedNoxrForSource != decimal.MinValue)
                    {
                        decimal Tolerance = GetTolerance("NOXR", "LBMMBTU", Category);

                        if (AppeRepMethod == "CONSTANT")
                        {
                            if ((AppeReportedValue >= 0) && (Math.Abs(AppeCalculatedNoxrForSource - AppeReportedValue) > Tolerance))
                                Category.CheckCatalogResult = "E";
                        }
                        else if (AppeRepMethod == "APPORTIONED")
                        {

                            int thisLocation = Category.GetCheckParameter("Current_Monitor_Plan_Location_Postion").ValueAsInt();
                            Category.SetArrayParameter("Apportionment_Calc_NOXR_Array", thisLocation, AppeCalculatedNoxrForSource);
                            decimal[] NOxRateCalcAccum = Category.GetCheckParameter("Rpt_Period_Nox_Rate_Calculated_Accumulator_Array").ValueAsDecimalArray();
                            if (NOxRateCalcAccum[thisLocation] != decimal.MinValue)
                            {
                                if (NOxRateCalcAccum[thisLocation] >= 0)
                                    Category.AccumCheckAggregate("Rpt_Period_Nox_Rate_Calculated_Accumulator_Array", thisLocation, AppeCalculatedNoxrForSource);
                            }
                            else
                                Category.SetArrayParameter("Rpt_Period_Nox_Rate_Calculated_Accumulator_Array", thisLocation, AppeCalculatedNoxrForSource);

                            Category.AccumCheckAggregate("Rpt_Period_Nox_Rate_Hours_Accumulator_Array", thisLocation, 1);
                            string[] monitorMeasureCodeArray = Category.GetCheckParameter("Monitor_Measure_Code_Array").ValueAsStringArray();
                            Category.SetCheckParameter("Current_Measure_Code", monitorMeasureCodeArray[(int)eHourMeasureParameter.Noxr], eParameterDataType.String);

                            if ((AppeReportedValue >= 0) && (Math.Abs(AppeCalculatedNoxrForSource - AppeReportedValue) > Tolerance))
                                Category.CheckCatalogResult = "E";
                        }
                        else
                        {
                            DataRowView CurrentFuelFlowRecord = Category.GetCheckParameter("Current_Fuel_Flow_Record").ValueAsDataRowView();
                            decimal NoxrAppeAccumulator = Category.GetCheckParameter("Noxr_App_E_Accumulator").ValueAsDecimal();

                            decimal FuelUsageTime = cDBConvert.ToDecimal(CurrentFuelFlowRecord["FUEL_USAGE_TIME"]);

                            if (((FuelUsageTime > 0) && (FuelUsageTime <= 1)) &&
                                (NoxrAppeAccumulator >= 0) && (AppeCalcHi != decimal.MinValue))
                            {
                                NoxrAppeAccumulator += (AppeCalculatedNoxrForSource * FuelUsageTime * AppeCalcHi);
                                Category.SetCheckParameter("Noxr_App_E_Accumulator", NoxrAppeAccumulator, eParameterDataType.Decimal);
                            }
                            else
                                Category.SetCheckParameter("Noxr_App_E_Accumulator", -1.0m, eParameterDataType.Decimal);

                            if ((AppeReportedValue >= 0) &&
                                (Math.Abs(AppeCalculatedNoxrForSource - AppeReportedValue) > Tolerance))
                                Category.CheckCatalogResult = "F";
                        }
                    }
                    else
                    {
                        if (AppeRepMethod == "CONSTANT")
                            Category.CheckCatalogResult = "G";
                        else
                        {
                            if (AppeRepMethod == "APPORTIONED")
                            {
                                int thisLocation = Category.GetCheckParameter("Current_Monitor_Plan_Location_Postion").ValueAsInt();
                                Category.SetArrayParameter("Apportionment_Calc_NOXR_Array", thisLocation, -1.0m);
                                Category.SetArrayParameter("Rpt_Period_Nox_Rate_Calculated_Accumulator_Array", thisLocation, -1.0m);
                                Category.CheckCatalogResult = "G";
                            }
                            else
                            {
                                if (AppeOpCode != "")
                                {
                                    Category.SetCheckParameter("Noxr_App_E_Accumulator", -1.0m, eParameterDataType.Decimal);
                                    Category.CheckCatalogResult = "H";
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURAE9");
            }

            return ReturnVal;
        }

        public static string HOURAE13(cCategory Category, ref bool Log)
        // Check Reported NOx Emission Rate
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentAppeNoxrRecord = Category.GetCheckParameter("Current_App_E_Noxr_Record").ValueAsDataRowView();

                if (CurrentAppeNoxrRecord != null)
                {
                    decimal ParamValFuel = cDBConvert.ToDecimal(CurrentAppeNoxrRecord["PARAM_VAL_FUEL"]);
                    if (ParamValFuel < 0)
                        Category.CheckCatalogResult = "A";
                    else
                        if (ParamValFuel != Math.Round(ParamValFuel, 3, MidpointRounding.AwayFromZero) && ParamValFuel != decimal.MinValue)
                        Category.CheckCatalogResult = "B";
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURAE13");
            }

            return ReturnVal;
        }

        public static string HOURAE14(cCategory Category, ref bool Log)
        // Check NOXR Units Of Measure
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentAppeNoxrRecord = Category.GetCheckParameter("Current_App_E_Noxr_Record").ValueAsDataRowView();

                if (CurrentAppeNoxrRecord != null)
                {
                    if (cDBConvert.ToString(CurrentAppeNoxrRecord["PARAMETER_UOM_CD"]) != "LBMMBTU")
                        Category.CheckCatalogResult = "A";
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURAE14");
            }

            return ReturnVal;
        }

        public static string HOURAE15(cCategory category, ref bool log)
        // Determine Appendix E Measure Code
        {
            string ReturnVal = "";

            try
            {
                string appeOpCode = category.GetCheckParameter("App_E_Op_Code").AsString();

                if (!appeOpCode.IsEmpty())
                {
                    string[] monitorMeasureCodeArray = category.GetCheckParameter("Monitor_Measure_Code_Array").ValueAsStringArray();

                    if ((appeOpCode == "E") || (monitorMeasureCodeArray[(int)eHourMeasureParameter.Noxr] == "OTHER"))
                        monitorMeasureCodeArray[(int)eHourMeasureParameter.Noxr] = "OTHER";
                    else if (appeOpCode.InList("M,U,N"))
                    {
                        if (monitorMeasureCodeArray[(int)eHourMeasureParameter.Noxr].IsNotNull() &&
                            monitorMeasureCodeArray[(int)eHourMeasureParameter.Noxr].StartsWith("MEAS"))
                            monitorMeasureCodeArray[(int)eHourMeasureParameter.Noxr] = "MEASSUB";
                        else
                            monitorMeasureCodeArray[(int)eHourMeasureParameter.Noxr] = "SUB";
                    }
                    else if (appeOpCode.InList("W,X,Y,Z"))
                    {
                        if (monitorMeasureCodeArray[(int)eHourMeasureParameter.Noxr].IsNotNull() &&
                            monitorMeasureCodeArray[(int)eHourMeasureParameter.Noxr].Contains("SUB"))
                            monitorMeasureCodeArray[(int)eHourMeasureParameter.Noxr] = "MEASSUB";
                        else
                            monitorMeasureCodeArray[(int)eHourMeasureParameter.Noxr] = "MEASURE";
                    }

                    category.SetArrayParameter("Monitor_Measure_Code_Array", monitorMeasureCodeArray);
                }
            }
            catch (Exception ex)
            {
                ReturnVal = category.CheckEngine.FormatError(ex, "HOURAE15");
            }

            return ReturnVal;
        }

        /// <summary>
        /// Updates the System Operarting Supplemental Data for the system reported with the Current Hourly Fuel Flow reocrd.
        /// </summary>
        /// <param name="category"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public static string HOURAE16(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                if (EmParameters.DerivedHourlyChecksNeeded.Default(false) && (EmParameters.CurrentOperatingTime.Value > 0))
                {
                    string monSysId = (EmParameters.CurrentAppENoxrRecord != null) ? EmParameters.CurrentAppENoxrRecord.MonSysId : null;

                    if (monSysId.IsNotEmpty())
                    {
                        Dictionary<string, SystemOperatingSupplementalData> supplementalDataDictionary = EmParameters.SystemOperatingSuppDataDictionaryArray[EmParameters.CurrentMonitorPlanLocationPostion.Value];

                        // Get or created supplemental data record
                        SystemOperatingSupplementalData supplementalDataRecord;
                        {
                            if (supplementalDataDictionary.ContainsKey(monSysId))
                            {
                                supplementalDataRecord = supplementalDataDictionary[monSysId];
                            }
                            else
                            {
                                supplementalDataRecord = new SystemOperatingSupplementalData(EmParameters.CurrentReportingPeriod.Value,
                                                                                             monSysId,
                                                                                             EmParameters.CurrentAppENoxrRecord.MonLocId,
                                                                                             true);

                                supplementalDataDictionary.Add(monSysId, supplementalDataRecord);
                            }
                        }

                        // Update with null modcCd will skip Quality Assured and Monitor Available counting, witch are not needed.
                        supplementalDataRecord.IncreamentForCurrentHour(EmParameters.CurrentOperatingDatehour.Value, null);
                    }
                }
            }
            catch (Exception ex)
            {
                returnVal = category.CheckEngine.FormatError(ex);
            }

            return returnVal;
        }

        #endregion


        #region Private Methods: Utilities

        private static decimal GetTolerance(string AParameterCd, String AUom, cCategory ACategory)
        {
            DataView ToleranceView = (DataView)ACategory.GetCheckParameter("Hourly_Emissions_Tolerances_Cross_Check_Table").ParameterValue;
            DataRowView ToleranceRow;
            sFilterPair[] ToleranceFilter = new sFilterPair[2];

            ToleranceFilter[0].Set("Parameter", AParameterCd);
            ToleranceFilter[1].Set("UOM", AUom);

            if (FindRow(ToleranceView, ToleranceFilter, out ToleranceRow))
                return cDBConvert.ToDecimal(ToleranceRow["Tolerance"]);
            else
                return decimal.MinValue;
        }

        #endregion


        #region Obsolete

        public static string HOURAE3(cCategory Category, ref bool Log)
        // Set Current Fuel Source Record and NOXR record for App E
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("Begin_Common_App_E_Checks", true, eParameterDataType.Boolean);
                if (Category.CategoryCd == "AEHRGAS")
                {
                    DataRowView AppENoxrRecGas = Category.GetCheckParameter("Current_App_E_Noxr_Record_For_Gas").ValueAsDataRowView();
                    DataRowView GasFFRec = Category.GetCheckParameter("Current_Gas_Fuel_Flow_Record").ValueAsDataRowView();
                    decimal CalcHIGas = Category.GetCheckParameter("Calc_Hi_For_Gas").ValueAsDecimal();
                    if (AppENoxrRecGas != null)
                    {
                        Category.SetCheckParameter("Current_App_E_Fuel_Flow_Record", GasFFRec, eParameterDataType.DataRowView);
                        Category.SetCheckParameter("Current_App_E_Noxr_Record", AppENoxrRecGas, eParameterDataType.DataRowView);
                        Category.SetCheckParameter("App_E_Calc_Hi", CalcHIGas, eParameterDataType.Decimal);
                    }
                    else
                        Category.SetCheckParameter("Begin_Common_App_E_Checks", false, eParameterDataType.Boolean);
                }
                else if (Category.CategoryCd == "AEHROIL")
                {
                    DataRowView AppENoxrRecOil = Category.GetCheckParameter("Current_App_E_Noxr_Record_For_Oil").ValueAsDataRowView();
                    DataRowView OilFFRec = Category.GetCheckParameter("Current_Oil_Fuel_Flow_Record").ValueAsDataRowView();
                    decimal CalcHIOil = Category.GetCheckParameter("Calc_Hi_For_Oil").ValueAsDecimal();
                    if (AppENoxrRecOil != null)
                    {
                        Category.SetCheckParameter("Current_App_E_Fuel_Flow_Record", OilFFRec, eParameterDataType.DataRowView);
                        Category.SetCheckParameter("Current_App_E_Noxr_Record", AppENoxrRecOil, eParameterDataType.DataRowView);
                        Category.SetCheckParameter("App_E_Calc_Hi", CalcHIOil, eParameterDataType.Decimal);
                    }
                    else
                        Category.SetCheckParameter("Begin_Common_App_E_Checks", false, eParameterDataType.Boolean);
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURAE3");
            }

            return ReturnVal;
        }

        public static string HOURAE6(cCategory Category, ref bool Log)
        // Set up Operating Condition Code Check for Appendix E NOXR Record
        {
            string ReturnVal = "";

            try
            {
                if (Category.GetCheckParameter("App_E_Noxr_Mon_Sys_Is_Valid").ValueAsBool() == true)
                {
                    DataRowView CurrentAppENoxrRecord = (DataRowView)Category.GetCheckParameter("Current_App_E_Noxr_Record").ParameterValue;
                    Category.SetCheckParameter("App_E_Hpff_Op_Code", cDBConvert.ToString(CurrentAppENoxrRecord["Operating_Condition_Cd"]), eParameterDataType.String);
                    Category.SetCheckParameter("App_E_Hpff_Reported_Value", cDBConvert.ToDecimal(CurrentAppENoxrRecord["Param_Val_Fuel"]), eParameterDataType.Decimal);
                    Category.SetCheckParameter("App_E_Hpff_Segment_Number", cDBConvert.ToInteger(CurrentAppENoxrRecord["Segment_Num"]), eParameterDataType.Integer);
                }
                else
                    Log = false;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURAE6");
            }

            return ReturnVal;
        }

        public static string HOURAE10(cCategory Category, ref bool Log)
        //Find NOXR record associated with current oil fuel flow
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("Current_App_E_Noxr_Record_For_Oil", null, eParameterDataType.DataRowView);
                if (Category.GetCheckParameter("Oil_Fuel_Flow_Mon_Sys_Status").ValueAsBool())
                {
                    DataRowView OilFFRec = Category.GetCheckParameter("Current_Oil_Fuel_Flow_Record").ValueAsDataRowView();
                    int HrlyFFID = cDBConvert.ToInteger(OilFFRec["HRLY_FUEL_FLOW_ID"]);

                    DataView HrlyParamFFRecs = Category.GetCheckParameter("Hourly_Param_Fuel_Flow_Records_For_Current_Fuel_Flow").ValueAsDataView();
                    //string FFFilter = HrlyParamFFRecs.RowFilter;
                    //HrlyParamFFRecs.RowFilter = AddToDataViewFilter(FFFilter, "HRLY_FUEL_FLOW_ID = '" + HrlyFFID + "' and Parameter_Cd = 'NOXR'");
                    sFilterPair[] HPFFOilFilter = new sFilterPair[2];
                    HPFFOilFilter[0].Field = "HRLY_FUEL_FLOW_ID";
                    HPFFOilFilter[0].Value = HrlyFFID;
                    HPFFOilFilter[1].Set("Parameter_Cd", "NOXR");

                    DataView HPFFOilRecs = FindRows(HrlyParamFFRecs, HPFFOilFilter);
                    int HPFFCountOil = HPFFOilRecs.Count; //HrlyParamFFRecs.Count;
                    if (HPFFCountOil == 0)
                    {
                        if (Category.GetCheckParameter("App_E_Reporting_Method").ValueAsString() != "CONSTANT")
                        {
                            Category.CheckCatalogResult = "A";
                        }
                    }
                    else if (HPFFCountOil > 1)
                        Category.CheckCatalogResult = "B";
                    else
                    {
                        Category.SetCheckParameter("Current_App_E_Noxr_Record_For_Oil", HPFFOilRecs[0], eParameterDataType.DataRowView);
                    }

                    //HrlyParamFFRecs.RowFilter = FFFilter;
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURAE10");
            }

            return ReturnVal;
        }

        public static string HOURAE11(cCategory Category, ref bool Log)
        //Verify Overall NOx Emission Rate at Unit
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("Update_App_E_Total_NOXR_DHV", false, eParameterDataType.Boolean);
                string AppERepMeth = Category.GetCheckParameter("App_E_Reporting_Method").ValueAsString();

                if (AppERepMeth == "SINGLE" || AppERepMeth == "MULTIPLE")
                {
                    decimal CalcTotalHI = Category.GetCheckParameter("Calculated_Total_Heat_Input").ValueAsDecimal();
                    decimal CalculatedTotalNOXR;
                    if (CalcTotalHI > 0)
                        CalculatedTotalNOXR = Category.GetCheckParameter("Noxr_App_E_Accumulator").ValueAsDecimal() / CalcTotalHI;
                    else
                        CalculatedTotalNOXR = 0;

                    Category.SetCheckParameter("Calculated_Total_Noxr", CalculatedTotalNOXR, eParameterDataType.Decimal);
                    Category.SetCheckParameter("Update_App_E_Total_NOXR_DHV", true, eParameterDataType.Boolean);

                    DataView HrlyEmTlrncXCheck = (DataView)Category.GetCheckParameter("Hourly_Emissions_Tolerances_Cross_Check_Table").ParameterValue;
                    string XCheckFilter = HrlyEmTlrncXCheck.RowFilter;
                    HrlyEmTlrncXCheck.RowFilter = AddToDataViewFilter(XCheckFilter, "Parameter = 'NOXR' and UOM = 'LBMMBTU'");
                    decimal NoxrTolrnc = cDBConvert.ToDecimal(HrlyEmTlrncXCheck[0]["Tolerance"]);
                    HrlyEmTlrncXCheck.RowFilter = XCheckFilter;

                    DataRowView NoxrDHRec = Category.GetCheckParameter("Current_NoxR_Derived_Hourly_Record").ValueAsDataRowView();
                    if (Math.Abs(cDBConvert.ToDecimal(NoxrDHRec["Adjusted_Hrly_Value"]) - CalculatedTotalNOXR) > NoxrTolrnc)
                        Category.CheckCatalogResult = "A";
                }
                else
                    Log = false;

            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURAE11");
            }

            return ReturnVal;
        }

        #endregion
    }
}
