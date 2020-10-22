using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;
using ECMPS.Definitions.Extensions;

namespace ECMPS.Checks.CycleTimeChecks
{
    public class cCycleTimeChecks : cChecks
    {
        #region Constructors

        public cCycleTimeChecks()
        {
            CheckProcedures = new dCheckProcedure[23];
            CheckProcedures[1] = new dCheckProcedure(CYCLE1);
            CheckProcedures[2] = new dCheckProcedure(CYCLE2);
            CheckProcedures[3] = new dCheckProcedure(CYCLE3);
            CheckProcedures[4] = new dCheckProcedure(CYCLE4);
            CheckProcedures[5] = new dCheckProcedure(CYCLE5);
            CheckProcedures[6] = new dCheckProcedure(CYCLE6);
            CheckProcedures[7] = new dCheckProcedure(CYCLE7);
            CheckProcedures[8] = new dCheckProcedure(CYCLE8);
            CheckProcedures[9] = new dCheckProcedure(CYCLE9);
            CheckProcedures[10] = new dCheckProcedure(CYCLE10);
            CheckProcedures[11] = new dCheckProcedure(CYCLE11);
            CheckProcedures[12] = new dCheckProcedure(CYCLE12);
            CheckProcedures[13] = new dCheckProcedure(CYCLE13);
            CheckProcedures[14] = new dCheckProcedure(CYCLE14);
            CheckProcedures[15] = new dCheckProcedure(CYCLE15);
            CheckProcedures[16] = new dCheckProcedure(CYCLE16);
            CheckProcedures[17] = new dCheckProcedure(CYCLE17);
            CheckProcedures[18] = new dCheckProcedure(CYCLE18);
            CheckProcedures[19] = new dCheckProcedure(CYCLE19);
            CheckProcedures[20] = new dCheckProcedure(CYCLE20);
            CheckProcedures[21] = new dCheckProcedure(CYCLE21);
            CheckProcedures[22] = new dCheckProcedure(CYCLE22);
        }


        #endregion


        #region Cycle Time Checks

        public static string CYCLE1(cCategory Category, ref bool Log)
        //Initialize Cycle Time Test Variables
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("Cycle_Time_Zero_Injection_Count", 0, eParameterDataType.Integer);
                Category.SetCheckParameter("Cycle_Time_High_Injection_Count", 0, eParameterDataType.Integer);
                Category.SetCheckParameter("Cycle_Time_Calc_Total_Cycle_Time", -1, eParameterDataType.Integer);
                Category.SetCheckParameter("Cycle_Time_Calc_Test_Result", null, eParameterDataType.String);
                Category.SetCheckParameter("Cycle_Time_Zero_Reference_Value", null, eParameterDataType.Decimal);
                Category.SetCheckParameter("Cycle_Time_High_Reference_Value", null, eParameterDataType.Decimal);
                Category.SetCheckParameter("Cycle_Time_Injection_Times_Valid", true, eParameterDataType.Boolean);
                Category.SetCheckParameter("Cycle_Time_Test_Begin_Date", null, eParameterDataType.Date);
                Category.SetCheckParameter("Cycle_Time_Test_Begin_Hour", null, eParameterDataType.Integer);
                Category.SetCheckParameter("Cycle_Time_Test_Begin_Minute", null, eParameterDataType.Integer);
                Category.SetCheckParameter("Cycle_Time_Test_End_Date", null, eParameterDataType.Date);
                Category.SetCheckParameter("Cycle_Time_Test_End_Hour", null, eParameterDataType.Integer);
                Category.SetCheckParameter("Cycle_Time_Test_End_Minute", null, eParameterDataType.Integer);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "CYCLE1");
            }

            return ReturnVal;
        }

        public static string CYCLE2(cCategory Category, ref bool Log)
        //Component Type Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentCycleTimeTest = (DataRowView)Category.GetCheckParameter("Current_Cycle_Time_Test").ParameterValue;
                if (CurrentCycleTimeTest["COMPONENT_ID"] == DBNull.Value)
                {
                    Category.SetCheckParameter("Cycle_Time_Test_Component_Type_Valid", false, eParameterDataType.Boolean);
                    Category.CheckCatalogResult = "A";
                }
                else
                    if( cDBConvert.ToString( CurrentCycleTimeTest["COMPONENT_TYPE_CD"] ).InList( "SO2,NOX,CO2,O2,HG" ) )
                        Category.SetCheckParameter("Cycle_Time_Test_Component_Type_Valid", true, eParameterDataType.Boolean);
                    else
                    {
                        Category.SetCheckParameter("Cycle_Time_Test_Component_Type_Valid", false, eParameterDataType.Boolean);
                        Category.CheckCatalogResult = "B";
                    }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "CYCLE2");
            }

            return ReturnVal;
        }

        public static string CYCLE3(cCategory Category, ref bool Log)
        //Aborted Cycle Time Test Not Evaluated
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentCycleTimeTest = (DataRowView)Category.GetCheckParameter("Current_Cycle_Time_Test").ParameterValue;
                if (cDBConvert.ToString(CurrentCycleTimeTest["TEST_RESULT_CD"]) == "ABORTED")
                {
                    Category.SetCheckParameter("Cycle_Time_Test_Aborted", true, eParameterDataType.Boolean);
                    Category.SetCheckParameter("Cycle_Time_Calc_Test_Result", "ABORTED", eParameterDataType.Boolean);
                    Category.CheckCatalogResult = "A";
                }
                else
                    Category.SetCheckParameter("Cycle_Time_Test_Aborted", false, eParameterDataType.Boolean);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "CYCLE3");
            }

            return ReturnVal;
        }

        public static string CYCLE4(cCategory Category, ref bool Log)
        //Cycle Time Test Test Reason Code Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentCycleTimeTest = (DataRowView)Category.GetCheckParameter("Current_Cycle_Time_Test").ParameterValue;
                string TestReasonCd = cDBConvert.ToString(CurrentCycleTimeTest["TEST_REASON_CD"]);
                if (TestReasonCd == "")
                    Category.CheckCatalogResult = "A";
                else
                    if( !TestReasonCd.InList( "INITIAL,RECERT,DIAG" ) )
                    {
                        DataView TestReasonCodes = (DataView)Category.GetCheckParameter("Test_Reason_Code_Lookup_Table").ParameterValue;
                        string OldFilter = TestReasonCodes.RowFilter;
                        TestReasonCodes.RowFilter = AddToDataViewFilter(OldFilter, "TEST_REASON_CD = '" + TestReasonCd + "'");
                        if (TestReasonCodes.Count == 0)
                            Category.CheckCatalogResult = "B";
                        else
                               Category.CheckCatalogResult = "C";
                    }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "CYCLE4");
            }

            return ReturnVal;
        }

        public static string CYCLE5(cCategory Category, ref bool Log)
        //Identification of Previously Reported Test or Test Number for Cycle Time Test
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentCycleTimeTest = (DataRowView)Category.GetCheckParameter("Current_Cycle_Time_Test").ParameterValue;
                string CompID = cDBConvert.ToString(CurrentCycleTimeTest["COMPONENT_ID"]);
                if (Convert.ToBoolean(Category.GetCheckParameter("Test_Span_Scale_Valid").ParameterValue) &&
                    Convert.ToBoolean(Category.GetCheckParameter("Test_End_Date_Valid").ParameterValue) &&
                    Convert.ToBoolean(Category.GetCheckParameter("Test_End_Hour_Valid").ParameterValue) &&
                    Convert.ToBoolean(Category.GetCheckParameter("Test_End_Minute_Valid").ParameterValue) && CompID != "")
                {
                    Category.SetCheckParameter("Cycle_Time_Test_Supp_Data_Id", null, eParameterDataType.String);                    
                    DataView TestRecs = (DataView)Category.GetCheckParameter("Cycle_Time_Test_Records").ParameterValue;
                    string OldFilter1 = TestRecs.RowFilter;
                    string SpanScaleCd = cDBConvert.ToString(CurrentCycleTimeTest["SPAN_SCALE_CD"]);
                    DateTime EndDate = cDBConvert.ToDate(CurrentCycleTimeTest["END_DATE"], DateTypes.END);
                    int EndHour = cDBConvert.ToHour(CurrentCycleTimeTest["END_HOUR"], DateTypes.END);
                    int EndMin = cDBConvert.ToInteger(CurrentCycleTimeTest["END_MIN"]);
                    string TestNum = cDBConvert.ToString(CurrentCycleTimeTest["TEST_NUM"]);
                    if (EndMin != int.MinValue)
                        TestRecs.RowFilter = AddToDataViewFilter(OldFilter1, "SPAN_SCALE_CD = '" + SpanScaleCd + "'" + " AND END_DATE = '" + 
                            EndDate.ToShortDateString() + "'" + " AND END_HOUR = " + EndHour + " AND END_MIN = " + EndMin + 
                            " AND TEST_NUM <> '" + TestNum + "'");
                    else
                        TestRecs.RowFilter = AddToDataViewFilter(OldFilter1, "SPAN_SCALE_CD = '" + SpanScaleCd + "'" + " AND END_DATE = '" + 
                            EndDate.ToShortDateString() + "'" + " AND END_HOUR = " + EndHour + " AND END_MIN IS NULL AND TEST_NUM <> '" + 
                            TestNum + "'");
                    if ((TestRecs.Count > 0 && CurrentCycleTimeTest["TEST_SUM_ID"] == DBNull.Value) ||
                        (TestRecs.Count > 1 && CurrentCycleTimeTest["TEST_SUM_ID"] != DBNull.Value) ||
                        (TestRecs.Count == 1 && CurrentCycleTimeTest["TEST_SUM_ID"] != DBNull.Value && CurrentCycleTimeTest["TEST_SUM_ID"].ToString() != TestRecs[0]["TEST_SUM_ID"].ToString()))
                        Category.CheckCatalogResult = "A";
                    else
                    {
                        DataView QASuppRecs = (DataView)Category.GetCheckParameter("QA_Supplemental_Data_Records").ParameterValue;
                        string OldFilter2 = QASuppRecs.RowFilter;                        
                        if (EndMin != int.MinValue)
                            QASuppRecs.RowFilter = AddToDataViewFilter(OldFilter2, "TEST_TYPE_CD = 'CYCLE' AND COMPONENT_ID = '" +
                                CompID + "'" + " AND SPAN_SCALE = '" + SpanScaleCd + "'" + " AND END_DATE = '" + EndDate.ToShortDateString() +
                                "'" + " AND END_HOUR = " + EndHour + " AND END_MIN = " + EndMin + " AND TEST_NUM <> '" + TestNum + "'");
                        else
                            QASuppRecs.RowFilter = AddToDataViewFilter(OldFilter2, "TEST_TYPE_CD = 'CYCLE' AND COMPONENT_ID = '" +
                                CompID + "'" + " AND SPAN_SCALE = '" + SpanScaleCd + "'" + " AND END_DATE = '" + EndDate.ToShortDateString() +
                                "'" + " AND END_HOUR = " + EndHour + " AND END_MIN IS NULL AND TEST_NUM <> '" + TestNum + "'");
                        if ((QASuppRecs.Count > 0 && CurrentCycleTimeTest["TEST_SUM_ID"] == DBNull.Value) ||
                            (QASuppRecs.Count > 1 && CurrentCycleTimeTest["TEST_SUM_ID"] != DBNull.Value) ||
                            (QASuppRecs.Count == 1 && CurrentCycleTimeTest["TEST_SUM_ID"] != DBNull.Value && CurrentCycleTimeTest["TEST_SUM_ID"].ToString() != QASuppRecs[0]["TEST_SUM_ID"].ToString()))
                            Category.CheckCatalogResult = "A";
                        else
                        {
                            QASuppRecs.RowFilter = AddToDataViewFilter(OldFilter2, "TEST_TYPE_CD = 'CYCLE' AND TEST_NUM = '" + TestNum + "'");
                            if (QASuppRecs.Count > 0)
                            {
                                Category.SetCheckParameter("Cycle_Time_Test_Supp_Data_ID", cDBConvert.ToString(((DataRowView)QASuppRecs[0])["QA_Supp_Data_ID"]), eParameterDataType.String);
                                if (cDBConvert.ToString(((DataRowView)QASuppRecs[0])["CAN_SUBMIT"]) == "N")
                                {
                                    QASuppRecs.RowFilter = AddToDataViewFilter(OldFilter2, "TEST_TYPE_CD = 'CYCLE' AND TEST_NUM = '" +
                                        TestNum + "'" + " AND (COMPONENT_ID <> '" + CompID + "' OR SPAN_SCALE <> '" + SpanScaleCd + "'" +
                                        " OR END_DATE <> '" + EndDate.ToShortDateString() + "'" + " OR END_HOUR <> " + EndHour + " OR END_MIN <> " + EndMin + ")");
                                    if ((QASuppRecs.Count > 0 && CurrentCycleTimeTest["TEST_SUM_ID"] == DBNull.Value) ||
                                        (QASuppRecs.Count > 1 && CurrentCycleTimeTest["TEST_SUM_ID"] != DBNull.Value) ||
                                        (QASuppRecs.Count == 1 && CurrentCycleTimeTest["TEST_SUM_ID"] != DBNull.Value && CurrentCycleTimeTest["TEST_SUM_ID"].ToString() != QASuppRecs[0]["TEST_SUM_ID"].ToString()))
                                        Category.CheckCatalogResult = "B";
                                    else
                                        Category.CheckCatalogResult = "C";
                                }
                            }
                        }
                        QASuppRecs.RowFilter = OldFilter2;
                    }
                    TestRecs.RowFilter = OldFilter1;
                }
                else
                    Log = false;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "CYCLE5");
            }

            return ReturnVal;
        }

        public static string CYCLE6(cCategory Category, ref bool Log)
        //Cycle Time Injection Begin Time Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentCycleTimeInjection = (DataRowView)Category.GetCheckParameter("Current_Cycle_Time_Injection").ParameterValue;
                string GasLvlCd = cDBConvert.ToString(CurrentCycleTimeInjection["GAS_LEVEL_CD"]);
                if (GasLvlCd == "ZERO")
                    Category.SetCheckParameter("Cycle_Time_Zero_Injection_Count", (1 + Convert.ToInt32(Category.GetCheckParameter("Cycle_Time_Zero_Injection_Count").ParameterValue)), eParameterDataType.Integer);
                else
                    if (GasLvlCd == "HIGH")
                        Category.SetCheckParameter("Cycle_Time_High_Injection_Count", (1 + Convert.ToInt32(Category.GetCheckParameter("Cycle_Time_High_Injection_Count").ParameterValue)), eParameterDataType.Integer);
                DateTime BeginDate = cDBConvert.ToDate(CurrentCycleTimeInjection["BEGIN_DATE"], DateTypes.START);
                int BeginHour = cDBConvert.ToInteger(CurrentCycleTimeInjection["BEGIN_HOUR"]);
                int BeginMin = cDBConvert.ToInteger(CurrentCycleTimeInjection["BEGIN_MIN"]);
                if (BeginDate == DateTime.MinValue || BeginHour == int.MinValue || (BeginHour < 0 || BeginHour > 23) || BeginMin == int.MinValue || (BeginMin < 0 || BeginMin > 59))
                {
                    Category.SetCheckParameter("Cycle_Time_Injection_Dates_Consistent", false, eParameterDataType.Boolean);
                    Category.SetCheckParameter("Cycle_Time_Injection_Times_Valid", false, eParameterDataType.Boolean);
                    Category.CheckCatalogResult = "A";
                }
                else
                    Category.SetCheckParameter("Cycle_Time_Injection_Dates_Consistent", true, eParameterDataType.Boolean);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "CYCLE6");
            }

            return ReturnVal;
        }

        public static string CYCLE7(cCategory Category, ref bool Log)
        //Cycle Time Injection End Time Valid
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("Cycle_Time_Calc_Injection_Cycle_Time", null, eParameterDataType.Integer);
                DataRowView CurrentCycleTimeInjection = (DataRowView)Category.GetCheckParameter("Current_Cycle_Time_Injection").ParameterValue;
                DateTime EndDate = cDBConvert.ToDate(CurrentCycleTimeInjection["END_DATE"], DateTypes.END);
                int EndHour = cDBConvert.ToInteger(CurrentCycleTimeInjection["END_HOUR"]);
                int EndMin = cDBConvert.ToInteger(CurrentCycleTimeInjection["END_MIN"]);
                if (EndDate == DateTime.MaxValue || EndHour == int.MinValue || (EndHour < 0 || 23 < EndHour) || EndMin == int.MinValue || (EndMin < 0 || 59 < EndMin))
                {
                    Category.SetCheckParameter("Cycle_Time_Injection_Dates_Consistent", false, eParameterDataType.Boolean);
                    Category.SetCheckParameter("Cycle_Time_Injection_Times_Valid", false, eParameterDataType.Boolean);
                    Category.CheckCatalogResult = "A";
                }
                else
                {
                    DateTime BeginDate = cDBConvert.ToDate(CurrentCycleTimeInjection["BEGIN_DATE"], DateTypes.START);
                    int BeginHour = cDBConvert.ToInteger(CurrentCycleTimeInjection["BEGIN_HOUR"]);
                    int BeginMin = cDBConvert.ToInteger(CurrentCycleTimeInjection["BEGIN_MIN"]);
                    if (BeginDate != DateTime.MinValue && !(BeginHour < 0 || 23 < BeginHour) && !(BeginMin < 0 || 59 < BeginMin))
                        if (BeginDate > EndDate || BeginDate == EndDate && (BeginHour > EndHour || (BeginHour == EndHour && BeginMin > EndMin)))
                        {
                            Category.SetCheckParameter("Cycle_Time_Injection_Dates_Consistent", false, eParameterDataType.Boolean);
                            Category.SetCheckParameter("Cycle_Time_Injection_Times_Valid", false, eParameterDataType.Boolean);
                            Category.CheckCatalogResult = "B";
                        }
                        else
                        {
                            DateTime TestBegDate = Convert.ToDateTime(Category.GetCheckParameter("Cycle_Time_Test_Begin_Date").ParameterValue);
                            int TestBegHour = Convert.ToInt32(Category.GetCheckParameter("Cycle_Time_Test_Begin_Hour").ParameterValue);
                            int TestBegMin = Convert.ToInt32(Category.GetCheckParameter("Cycle_Time_Test_Begin_Minute").ParameterValue);
                            DateTime TestEndDate = Convert.ToDateTime(Category.GetCheckParameter("Cycle_Time_Test_End_Date").ParameterValue);
                            int TestEndHour = Convert.ToInt32(Category.GetCheckParameter("Cycle_Time_Test_End_Hour").ParameterValue);
                            int TestEndMin = Convert.ToInt32(Category.GetCheckParameter("Cycle_Time_Test_End_Minute").ParameterValue);
                            if (Category.GetCheckParameter("Cycle_Time_Test_Begin_Date").ParameterValue == null || Category.GetCheckParameter("Cycle_Time_Test_Begin_Hour").ParameterValue == null ||
                                Category.GetCheckParameter("Cycle_Time_Test_Begin_Minute").ParameterValue == null || TestBegDate > BeginDate ||
                                (TestBegDate == BeginDate && (TestBegHour > BeginHour || (TestBegHour == BeginHour && (TestBegMin > BeginMin)))))
                            {
                                Category.SetCheckParameter("Cycle_Time_Test_Begin_Date", BeginDate, eParameterDataType.Date);
                                Category.SetCheckParameter("Cycle_Time_Test_Begin_Hour", BeginHour, eParameterDataType.Integer);
                                Category.SetCheckParameter("Cycle_Time_Test_Begin_Minute", BeginMin, eParameterDataType.Integer);
                            }
                            if (Category.GetCheckParameter("Cycle_Time_Test_End_Date").ParameterValue == null || Category.GetCheckParameter("Cycle_Time_Test_End_Hour").ParameterValue == null ||
                                Category.GetCheckParameter("Cycle_Time_Test_End_Minute").ParameterValue == null || TestEndDate < EndDate ||
                                (TestEndDate == EndDate && (TestEndHour < EndHour || (TestEndHour == EndHour && (TestEndMin < EndMin)))))
                            {
                                Category.SetCheckParameter("Cycle_Time_Test_End_Date", EndDate, eParameterDataType.Date);
                                Category.SetCheckParameter("Cycle_Time_Test_End_Hour", EndHour, eParameterDataType.Integer);
                                Category.SetCheckParameter("Cycle_Time_Test_End_Minute", EndMin, eParameterDataType.Integer);
                            }
                        }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "CYCLE7");
            }

            return ReturnVal;
        }

        public static string CYCLE8(cCategory Category, ref bool Log)
        //Cycle Time Injection Calibration Gas Value Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentCycleTimeInjection = (DataRowView)Category.GetCheckParameter("Current_Cycle_Time_Injection").ParameterValue;
                decimal CalGasVal = cDBConvert.ToDecimal(CurrentCycleTimeInjection["CAL_GAS_VALUE"]);
                if (CalGasVal == decimal.MinValue)
                    Category.CheckCatalogResult = "A";
                else
                    if (CalGasVal < 0)
                        Category.CheckCatalogResult = "B";
                    else
                    {
                        string GasLvlCd = cDBConvert.ToString(CurrentCycleTimeInjection["GAS_LEVEL_CD"]);
                        if (GasLvlCd == "ZERO")
                            Category.SetCheckParameter("Cycle_Time_Zero_Reference_Value", CalGasVal, eParameterDataType.Decimal);
                        else
                            if (GasLvlCd == "HIGH")
                                Category.SetCheckParameter("Cycle_Time_High_Reference_Value", CalGasVal, eParameterDataType.Decimal);
                    }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "CYCLE8");
            }

            return ReturnVal;
        }

        public static string CYCLE9(cCategory Category, ref bool Log)
        //Calibration Gas Value Consistent with Span
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentCycleTimeInjection = (DataRowView)Category.GetCheckParameter("Current_Cycle_Time_Injection").ParameterValue;
                decimal CalGasVal = cDBConvert.ToDecimal(CurrentCycleTimeInjection["CAL_GAS_VALUE"]);
                decimal TestSpanVal = Convert.ToDecimal(Category.GetCheckParameter("Test_Span_Value").ParameterValue);
                if (CalGasVal >= 0 && TestSpanVal > 0)
                {
                    decimal CycTimeRefPercSpan = Math.Round(100 * CalGasVal / TestSpanVal, 1, MidpointRounding.AwayFromZero);
                    Category.SetCheckParameter("Cycle_Time_Reference_Percent_of_Span", CycTimeRefPercSpan, eParameterDataType.Decimal);
                    string GasLvlCd = cDBConvert.ToString(CurrentCycleTimeInjection["GAS_LEVEL_CD"]);
                    DataView TestToleranceRecords = (DataView)Category.GetCheckParameter("Test_Tolerances_Cross_Check_Table").ParameterValue;
                    string OldFilter = TestToleranceRecords.RowFilter;
                    TestToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, "FieldDescription = '" + "GasPercentOfSpan'");
                    decimal Tolerance = cDBConvert.ToDecimal(((DataRowView)TestToleranceRecords[0])["Tolerance"]);
                    TestToleranceRecords.RowFilter = OldFilter;
                    if (GasLvlCd == "ZERO")
                    {
                        if (CycTimeRefPercSpan > (decimal)20.0)
                            if (CycTimeRefPercSpan > (decimal)20.0 + Tolerance)
                                Category.CheckCatalogResult = "A";
                            else
                                Category.CheckCatalogResult = "B";
                    }
                    else
                        if (GasLvlCd == "HIGH")
                            if (CycTimeRefPercSpan > (decimal)100.0)
                                Category.CheckCatalogResult = "C";
                            else
                                if (CycTimeRefPercSpan < (decimal)80.0)
                                    if (CycTimeRefPercSpan < (decimal)80.0 - Tolerance)
                                        Category.CheckCatalogResult = "C";
                                    else
                                        Category.CheckCatalogResult = "D";
                }
                else
                    Log = false;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "CYCLE9");
            }

            return ReturnVal;
        }

        public static string CYCLE10(cCategory Category, ref bool Log)
        //Cycle Time Injection Begin Monitor Value Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentCycleTimeInjection = (DataRowView)Category.GetCheckParameter("Current_Cycle_Time_Injection").ParameterValue;
                if (CurrentCycleTimeInjection["BEGIN_MONITOR_VALUE"] == DBNull.Value)
                    Category.CheckCatalogResult = "A";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "CYCLE10");
            }

            return ReturnVal;
        }

        public static string CYCLE11(cCategory Category, ref bool Log)
        //Cycle Time Injection End Monitor Value Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentCycleTimeInjection = (DataRowView)Category.GetCheckParameter("Current_Cycle_Time_Injection").ParameterValue;
                decimal EndMonVal = cDBConvert.ToDecimal(CurrentCycleTimeInjection["END_MONITOR_VALUE"]);
                if (EndMonVal == decimal.MinValue)
                    Category.CheckCatalogResult = "A";
                else
                {
                    decimal BegMonVal = cDBConvert.ToDecimal(CurrentCycleTimeInjection["BEGIN_MONITOR_VALUE"]);
                    if (BegMonVal != decimal.MinValue)
                    {
                        string GasLvlCd = cDBConvert.ToString(CurrentCycleTimeInjection["GAS_LEVEL_CD"]);
                        DateTime Jan242008 = new DateTime(2008,1,24);
                        if (cDBConvert.ToDate(CurrentCycleTimeInjection["END_DATE"], DateTypes.END) < Jan242008)
                        {
                            if (GasLvlCd == "ZERO" && BegMonVal >= EndMonVal)
                                Category.CheckCatalogResult = "B";
                            else
                                if (GasLvlCd == "HIGH" && BegMonVal <= EndMonVal)
                                    Category.CheckCatalogResult = "C";
                        }
                        else
                            if (GasLvlCd == "HIGH" && BegMonVal >= EndMonVal)
                                Category.CheckCatalogResult = "D";
                            else
                                if (GasLvlCd == "ZERO" && BegMonVal <= EndMonVal)
                                    Category.CheckCatalogResult = "E";
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "CYCLE11");
            }

            return ReturnVal;
        }

        public static string CYCLE12(cCategory Category, ref bool Log)
        //Cycle Time Injection Injection Cycle Time Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentCycleTimeInjection = (DataRowView)Category.GetCheckParameter("Current_Cycle_Time_Injection").ParameterValue;
                int InjCycTime = cDBConvert.ToInteger(CurrentCycleTimeInjection["INJECTION_CYCLE_TIME"]);
                if (Convert.ToBoolean(Category.GetCheckParameter("Cycle_Time_Injection_Dates_Consistent").ParameterValue))
                {
                    DateTime EndDate = cDBConvert.ToDate(CurrentCycleTimeInjection["END_DATE"], DateTypes.END);
                    int EndHour = cDBConvert.ToInteger(CurrentCycleTimeInjection["END_HOUR"]);
                    int EndMin = cDBConvert.ToInteger(CurrentCycleTimeInjection["END_MIN"]);
                    DateTime BeginDate = cDBConvert.ToDate(CurrentCycleTimeInjection["BEGIN_DATE"], DateTypes.START);
                    int BeginHour = cDBConvert.ToHour(CurrentCycleTimeInjection["BEGIN_HOUR"], DateTypes.START);
                    int BeginMin = cDBConvert.ToInteger(CurrentCycleTimeInjection["BEGIN_MIN"]);
                    DateTime EndDateTime = DateTime.Parse(string.Format("{0} {1}:{2}",EndDate.ToShortDateString(),EndHour,EndMin));
                    DateTime BeginDateTime = DateTime.Parse(string.Format("{0} {1}:{2}",BeginDate.ToShortDateString(),BeginHour,BeginMin));
                    TimeSpan ts = EndDateTime - BeginDateTime;
                    int DiffInMinutes = Math.Min((int)ts.TotalMinutes, 99);
                    Category.SetCheckParameter("Cycle_Time_Calc_Injection_Cycle_Time", DiffInMinutes, eParameterDataType.Integer);
                    if (Category.GetCheckParameter("Cycle_Time_Calc_Total_Cycle_Time").ParameterValue != null)
                    {
                        int CycTimeCalcTotCycTime = Convert.ToInt32(Category.GetCheckParameter("Cycle_Time_Calc_Total_Cycle_Time").ParameterValue);
                        if (InjCycTime != int.MinValue && InjCycTime == (1 + DiffInMinutes))
                            Category.SetCheckParameter("Cycle_Time_Calc_Injection_Cycle_Time", InjCycTime, eParameterDataType.Integer);
                        if (DiffInMinutes > CycTimeCalcTotCycTime)
                            Category.SetCheckParameter("Cycle_Time_Calc_Total_Cycle_Time", DiffInMinutes, eParameterDataType.Integer);
                    }
                }
                else
                {
                    Category.SetCheckParameter("Cycle_Time_Calc_Total_Cycle_Time", null, eParameterDataType.Integer);
                    Category.SetCheckParameter("Cycle_Time_Calc_Test_Result", "INVALID", eParameterDataType.String);
                }
                if (InjCycTime == int.MinValue)
                    Category.CheckCatalogResult = "A";
                else
                    if (InjCycTime < 0)
                        Category.CheckCatalogResult = "B";
                    else
                        if (Category.GetCheckParameter("Cycle_Time_Calc_Injection_Cycle_Time").ParameterValue != null &&
                            Math.Abs(InjCycTime - Convert.ToInt32(Category.GetCheckParameter("Cycle_Time_Calc_Injection_Cycle_Time").ParameterValue)) > 1)
                            Category.CheckCatalogResult = "C";

            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "CYCLE12");
            }

            return ReturnVal;
        }

        public static string CYCLE13(cCategory Category, ref bool Log)
        //Cycle Time Test Begin Time Consistent with Injection Times
        {
            string ReturnVal = "";

            try
            {
                if (Convert.ToBoolean(Category.GetCheckParameter("Cycle_Time_Injection_Times_Valid").ParameterValue) &&
                    Convert.ToBoolean(Category.GetCheckParameter("Test_Begin_Date_Valid").ParameterValue) &&
                    Convert.ToBoolean(Category.GetCheckParameter("Test_Begin_Hour_Valid").ParameterValue) &&
                    Convert.ToBoolean(Category.GetCheckParameter("Test_Begin_Minute_Valid").ParameterValue) &&
                    (Convert.ToInt32(Category.GetCheckParameter("Cycle_Time_High_Injection_Count").ParameterValue) > 0 || Convert.ToInt32(Category.GetCheckParameter("Cycle_Time_Zero_Injection_Count").ParameterValue) > 0))
                {
                    DataRowView CurrentCycleTimeTest = (DataRowView)Category.GetCheckParameter("Current_Cycle_Time_Test").ParameterValue;
                    DateTime BeginDate = cDBConvert.ToDate(CurrentCycleTimeTest["BEGIN_DATE"], DateTypes.START);
                    int BeginHour = cDBConvert.ToInteger(CurrentCycleTimeTest["BEGIN_HOUR"]);
                    int BeginMin = cDBConvert.ToInteger(CurrentCycleTimeTest["BEGIN_MIN"]);
                    DateTime TestBegDate = Convert.ToDateTime(Category.GetCheckParameter("Cycle_Time_Test_Begin_Date").ParameterValue);
                    int TestBegHour = Convert.ToInt32(Category.GetCheckParameter("Cycle_Time_Test_Begin_Hour").ParameterValue);
                    int TestBegMin = Convert.ToInt32(Category.GetCheckParameter("Cycle_Time_Test_Begin_Minute").ParameterValue);
                    if (BeginDate != TestBegDate || BeginHour != TestBegHour || BeginMin != TestBegMin)
                        Category.CheckCatalogResult = "A";
                }
                else
                    Log = false;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "CYCLE13");
            }

            return ReturnVal;
        }

        public static string CYCLE14(cCategory Category, ref bool Log)
        //Cycle Time Test End Time Consistent with Injection Times
        {
            string ReturnVal = "";

            try
            {
                if (Convert.ToBoolean(Category.GetCheckParameter("Cycle_Time_Injection_Times_Valid").ParameterValue) &&
                    Convert.ToBoolean(Category.GetCheckParameter("Test_End_Date_Valid").ParameterValue) &&
                    Convert.ToBoolean(Category.GetCheckParameter("Test_End_Hour_Valid").ParameterValue) &&
                    Convert.ToBoolean(Category.GetCheckParameter("Test_End_Minute_Valid").ParameterValue) &&
                    (Convert.ToInt32(Category.GetCheckParameter("Cycle_Time_High_Injection_Count").ParameterValue) > 0 || Convert.ToInt32(Category.GetCheckParameter("Cycle_Time_Zero_Injection_Count").ParameterValue) > 0))
                {
                    DataRowView CurrentCycleTimeTest = (DataRowView)Category.GetCheckParameter("Current_Cycle_Time_Test").ParameterValue;
                    DateTime EndDate = cDBConvert.ToDate(CurrentCycleTimeTest["END_DATE"], DateTypes.START);
                    int EndHour = cDBConvert.ToInteger(CurrentCycleTimeTest["END_HOUR"]);
                    int EndMin = cDBConvert.ToInteger(CurrentCycleTimeTest["END_MIN"]);
                    DateTime TestEndDate = Convert.ToDateTime(Category.GetCheckParameter("Cycle_Time_Test_End_Date").ParameterValue);
                    int TestEndHour = Convert.ToInt32(Category.GetCheckParameter("Cycle_Time_Test_End_Hour").ParameterValue);
                    int TestEndMin = Convert.ToInt32(Category.GetCheckParameter("Cycle_Time_Test_End_Minute").ParameterValue);
                    if (EndDate != TestEndDate || EndHour != TestEndHour || EndMin != TestEndMin)
                        Category.CheckCatalogResult = "A";
                }
                else
                    Log = false;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "CYCLE14");
            }

            return ReturnVal;
        }

        public static string CYCLE15(cCategory Category, ref bool Log)
        //Calibration Gas Value Consistent with Gas Level
        {
            string ReturnVal = "";

            try
            {
                if (Category.GetCheckParameter("Cycle_Time_Zero_Reference_Value").ParameterValue != null &&
                    Category.GetCheckParameter("Cycle_Time_High_Reference_Value").ParameterValue != null)
                {
                    if (Convert.ToDecimal(Category.GetCheckParameter("Cycle_Time_Zero_Reference_Value").ParameterValue) >=
                        Convert.ToDecimal(Category.GetCheckParameter("Cycle_Time_High_Reference_Value").ParameterValue))
                    {
                        Category.SetCheckParameter("Cycle_Time_Calc_Test_Result", "INVALID", eParameterDataType.String);
                        Category.CheckCatalogResult = "A";
                    }
                }
                else
                    Category.SetCheckParameter("Cycle_Time_Calc_Test_Result", "INVALID", eParameterDataType.String);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "CYCLE15");
            }

            return ReturnVal;
        }

        public static string CYCLE16(cCategory Category, ref bool Log)
        //Correct Number of Cycle Time Injections
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("Cycle_Time_Test_Validity_Determined", true, eParameterDataType.Boolean);
                if (Convert.ToInt32(Category.GetCheckParameter("Cycle_Time_Zero_Injection_Count").ParameterValue) != 1 ||
                    Convert.ToInt32(Category.GetCheckParameter("Cycle_Time_High_Injection_Count").ParameterValue) != 1)
                {
                    Category.SetCheckParameter("Cycle_Time_Calc_Test_Result", "INVALID", eParameterDataType.String);
                    Category.CheckCatalogResult = "A";
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "CYCLE16");
            }

            return ReturnVal;
        }

        public static string CYCLE17(cCategory Category, ref bool Log)
        //Cycle Time Test Total Cycle Time Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentCycleTimeTest = (DataRowView)Category.GetCheckParameter("Current_Cycle_Time_Test").ParameterValue;
                int TotalTime = cDBConvert.ToInteger(CurrentCycleTimeTest["TOTAL_TIME"]);
                int CycTimeCalcTotCycTime = Convert.ToInt32(Category.GetCheckParameter("Cycle_Time_Calc_Total_Cycle_Time").ParameterValue);
                if (Convert.ToString(Category.GetCheckParameter("Cycle_Time_Calc_Test_Result").ParameterValue) != "INVALID" && CycTimeCalcTotCycTime >= 0)
                {
                    if (TotalTime > CycTimeCalcTotCycTime)
                    {
                        CycTimeCalcTotCycTime = TotalTime;
                        Category.SetCheckParameter("Cycle_Time_Calc_Total_Cycle_Time", TotalTime, eParameterDataType.Integer);
                    }
                    if (CycTimeCalcTotCycTime <= 15)
                        Category.SetCheckParameter("Cycle_Time_Calc_Test_Result", "PASSED", eParameterDataType.String);
                    else
                        Category.SetCheckParameter("Cycle_Time_Calc_Test_Result", "FAILED", eParameterDataType.String);
                }
                else
                    Category.SetCheckParameter("Cycle_Time_Calc_Total_Cycle_Time", null, eParameterDataType.Integer);
                Category.SetCheckParameter("Cycle_Time_Test_Total_Time_Calculated", true, eParameterDataType.Boolean);
                if (TotalTime == int.MinValue)
                    Category.CheckCatalogResult = "A";
                else
                    if (TotalTime < 0)
                        Category.CheckCatalogResult = "B";
                    else
                        if (Category.GetCheckParameter("Cycle_Time_Calc_Total_Cycle_Time").ParameterValue != null && CycTimeCalcTotCycTime > TotalTime)
                            Category.CheckCatalogResult = "C";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "CYCLE17");
            }

            return ReturnVal;
        }

        public static string CYCLE18(cCategory Category, ref bool Log)
        //Cycle Time Test Test Result Code Valid
        {
            string ReturnVal = "";

            try
            {
                if (Convert.ToString(Category.GetCheckParameter("Cycle_Time_Calc_Test_Result").ParameterValue) == "INVALID" || Category.GetCheckParameter("Cycle_Time_Calc_Total_Cycle_Time").ValueAsInt() < 0)
                    Category.SetCheckParameter("Cycle_Time_Calc_Test_Result", null, eParameterDataType.String);
                DataRowView CurrentCycleTimeTest = (DataRowView)Category.GetCheckParameter("Current_Cycle_Time_Test").ParameterValue;
                string TestResCd = cDBConvert.ToString(CurrentCycleTimeTest["TEST_RESULT_CD"]);
                if (TestResCd == "")
                    Category.CheckCatalogResult = "A";
                else
                    if( !TestResCd.InList( "ABORTED,PASSED,FAILED" ) )
                    {
                        DataView TestResultLookup = (DataView)Category.GetCheckParameter("Test_Result_Code_Lookup_Table").ParameterValue;
                        string OldFilter = TestResultLookup.RowFilter;
                        TestResultLookup.RowFilter = AddToDataViewFilter(OldFilter, "TEST_RESULT_CD = '" + TestResCd + "'");
                        if (TestResultLookup.Count == 0)
                            Category.CheckCatalogResult = "B";
                        else
                            Category.CheckCatalogResult = "C";
                        TestResultLookup.RowFilter = OldFilter;
                    }
                if (string.IsNullOrEmpty(Category.CheckCatalogResult))
                {
                    string CalcTestRes = Convert.ToString(Category.GetCheckParameter("Cycle_Time_Calc_Test_Result").ParameterValue);
                    if (CalcTestRes == "FAILED" && TestResCd == "PASSED")
                        Category.CheckCatalogResult = "D";
                    else
                        if (CalcTestRes == "PASSED" && TestResCd == "FAILED")
                            Category.CheckCatalogResult = "E";
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "CYCLE18");
            }

            return ReturnVal;
        }

        public static string CYCLE19(cCategory Category, ref bool Log)
        //Duplicate Cycle Time Test
        {
            string ReturnVal = "";

            try
            {
                if (Convert.ToBoolean(Category.GetCheckParameter("Test_Number_Valid").ParameterValue))
                {
                    DataRowView CurrentCycleTimeTest = (DataRowView)Category.GetCheckParameter("Current_Cycle_Time_Test").ParameterValue;
                    string TestNum = cDBConvert.ToString(CurrentCycleTimeTest["TEST_NUM"]);
                    DataView TestRecs = (DataView)Category.GetCheckParameter("Location_Test_Records").ParameterValue;
                    string OldFilter = TestRecs.RowFilter;
                    TestRecs.RowFilter = AddToDataViewFilter(OldFilter, "TEST_TYPE_CD = 'CYCLE' AND TEST_NUM = '" + TestNum + "'");
                    if ((TestRecs.Count > 0 && CurrentCycleTimeTest["TEST_SUM_ID"] == DBNull.Value) ||
                        (TestRecs.Count > 1 && CurrentCycleTimeTest["TEST_SUM_ID"] != DBNull.Value) ||
                        (TestRecs.Count == 1 && CurrentCycleTimeTest["TEST_SUM_ID"] != DBNull.Value && CurrentCycleTimeTest["TEST_SUM_ID"].ToString() != TestRecs[0]["TEST_SUM_ID"].ToString()))
                    {
                        Category.SetCheckParameter("Duplicate_Cycle_Time", true, eParameterDataType.Boolean);
                        Category.CheckCatalogResult = "A";
                    }
                    else
                    {
                      string TestSumID = cDBConvert.ToString(CurrentCycleTimeTest["TEST_SUM_ID"]);
                      if (TestSumID != "")
                      {
                        DataView QASuppRecords = (DataView)Category.GetCheckParameter("QA_Supplemental_Data_Records").ParameterValue;
                        string OldFilter2 = QASuppRecords.RowFilter;
                        QASuppRecords.RowFilter = AddToDataViewFilter(OldFilter2, "TEST_NUM = '" + TestNum + "' AND TEST_TYPE_CD = 'CYCLE'");
                        if (QASuppRecords.Count > 0 && cDBConvert.ToString(QASuppRecords[0]["TEST_SUM_ID"]) != TestSumID)
                        {
                          Category.SetCheckParameter("Duplicate_Cycle_Time", true, eParameterDataType.Boolean);
                          Category.CheckCatalogResult = "B";
                        }
                        else
                          Category.SetCheckParameter("Duplicate_Cycle_Time", false, eParameterDataType.Boolean);
                        QASuppRecords.RowFilter = OldFilter2;
                      }
                      else
                        Category.SetCheckParameter("Duplicate_Cycle_Time", false, eParameterDataType.Boolean);
                    }
                }
                else
                    Log = false;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "CYCLE19");
            }

            return ReturnVal;
        }
        
        public static string CYCLE20(cCategory Category, ref bool Log)
        //Duplicate Cycle Time Injection
        {
            string ReturnVal = "";

            try
            {
                if (Convert.ToBoolean(Category.GetCheckParameter("Cycle_Time_Injection_Gas_Level_Valid").ParameterValue))
                {
                    DataRowView CurrentCycleTimeInjection = (DataRowView)Category.GetCheckParameter("Current_Cycle_Time_Injection").ParameterValue;
                    string GasLvlCd = cDBConvert.ToString(CurrentCycleTimeInjection["GAS_LEVEL_CD"]);
                    string CycTimeSumID = cDBConvert.ToString(CurrentCycleTimeInjection["CYCLE_TIME_SUM_ID"]);
                    DataView CycTimeInjRecs = (DataView)Category.GetCheckParameter("Cycle_Time_Injection_Records").ParameterValue;
                    string OldFilter = CycTimeInjRecs.RowFilter;
                    CycTimeInjRecs.RowFilter = AddToDataViewFilter(OldFilter, "CYCLE_TIME_SUM_ID = '" + CycTimeSumID + "' AND GAS_LEVEL_CD = '" + GasLvlCd + "'");
                    if ((CycTimeInjRecs.Count > 0 && CurrentCycleTimeInjection["CYCLE_TIME_INJ_ID"] == DBNull.Value) ||
                        (CycTimeInjRecs.Count > 1 && CurrentCycleTimeInjection["CYCLE_TIME_INJ_ID"] != DBNull.Value) ||
                        (CycTimeInjRecs.Count == 1 && CurrentCycleTimeInjection["CYCLE_TIME_INJ_ID"] != DBNull.Value && CurrentCycleTimeInjection["CYCLE_TIME_INJ_ID"].ToString() != CycTimeInjRecs[0]["CYCLE_TIME_INJ_ID"].ToString()))
                        Category.CheckCatalogResult = "A";
                    CycTimeInjRecs.RowFilter = OldFilter;
                }
                else
                    Log = false;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "CYCLE20");
            }

            return ReturnVal;
        }

        public static string CYCLE21(cCategory Category, ref bool Log)
        //Cycle Time Injection Gas Level Code Valid
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("Cycle_Time_Injection_Gas_Level_Valid", false, eParameterDataType.Boolean);
                DataRowView CurrentCycleTimeInjection = (DataRowView)Category.GetCheckParameter("Current_Cycle_Time_Injection").ParameterValue;
                string GasLvlCd = cDBConvert.ToString(CurrentCycleTimeInjection["GAS_LEVEL_CD"]);
                if (GasLvlCd == "")
                    Category.CheckCatalogResult = "A";
                else
                    if( !GasLvlCd.InList( "ZERO,HIGH" ) )
                        Category.CheckCatalogResult = "B";
                    else
                        Category.SetCheckParameter("Cycle_Time_Injection_Gas_Level_Valid", true, eParameterDataType.Boolean);

            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "CYCLE21");
            }

            return ReturnVal;
        }

        public static string CYCLE22(cCategory Category, ref bool Log)
        //Component ID Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentCycleTimeTest = (DataRowView)Category.GetCheckParameter("Current_Cycle_Time_Test").ParameterValue;
                if (CurrentCycleTimeTest["COMPONENT_ID"] == DBNull.Value)
                    Category.CheckCatalogResult = "A";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "CYCLE22");
            }

            return ReturnVal;
        }
        #endregion
    }

}
