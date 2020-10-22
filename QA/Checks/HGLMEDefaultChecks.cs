using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Extensions;

namespace ECMPS.Checks.HGLMEDefaultChecks
{
    public class cHGLMEDefaultChecks : cChecks
    {
        #region Constructors

        public cHGLMEDefaultChecks()
        {
            CheckProcedures = new dCheckProcedure[44];

            CheckProcedures[1] = new dCheckProcedure(HGLME1);
            CheckProcedures[2] = new dCheckProcedure(HGLME2);
            CheckProcedures[3] = new dCheckProcedure(HGLME3);
            CheckProcedures[4] = new dCheckProcedure(HGLME4);
            CheckProcedures[5] = new dCheckProcedure(HGLME5);
            CheckProcedures[6] = new dCheckProcedure(HGLME6);
            CheckProcedures[7] = new dCheckProcedure(HGLME7);
            CheckProcedures[8] = new dCheckProcedure(HGLME8);
            CheckProcedures[9] = new dCheckProcedure(HGLME9);
            CheckProcedures[10] = new dCheckProcedure(HGLME10);
            CheckProcedures[11] = new dCheckProcedure(HGLME11);
            CheckProcedures[12] = new dCheckProcedure(HGLME12);
            CheckProcedures[13] = new dCheckProcedure(HGLME13);
            CheckProcedures[14] = new dCheckProcedure(HGLME14);
            CheckProcedures[15] = new dCheckProcedure(HGLME15);
            CheckProcedures[16] = new dCheckProcedure(HGLME16);
            CheckProcedures[17] = new dCheckProcedure(HGLME17);
            CheckProcedures[18] = new dCheckProcedure(HGLME18);
            CheckProcedures[19] = new dCheckProcedure(HGLME19);
            CheckProcedures[20] = new dCheckProcedure(HGLME20);
            CheckProcedures[21] = new dCheckProcedure(HGLME21);
            CheckProcedures[22] = new dCheckProcedure(HGLME22);
            CheckProcedures[23] = new dCheckProcedure(HGLME23);
            CheckProcedures[24] = new dCheckProcedure(HGLME24);
            CheckProcedures[25] = new dCheckProcedure(HGLME25);
            CheckProcedures[26] = new dCheckProcedure(HGLME26);
            CheckProcedures[27] = new dCheckProcedure(HGLME27);
            CheckProcedures[28] = new dCheckProcedure(HGLME28);
            CheckProcedures[29] = new dCheckProcedure(HGLME29);
            CheckProcedures[30] = new dCheckProcedure(HGLME30);
            CheckProcedures[31] = new dCheckProcedure(HGLME31);
            CheckProcedures[32] = new dCheckProcedure(HGLME32);
            CheckProcedures[33] = new dCheckProcedure(HGLME33);
            CheckProcedures[34] = new dCheckProcedure(HGLME34);
            CheckProcedures[35] = new dCheckProcedure(HGLME35);
            CheckProcedures[36] = new dCheckProcedure(HGLME36);
            CheckProcedures[37] = new dCheckProcedure(HGLME37);
            CheckProcedures[38] = new dCheckProcedure(HGLME38);
            CheckProcedures[39] = new dCheckProcedure(HGLME39);
            CheckProcedures[40] = new dCheckProcedure(HGLME40);
            CheckProcedures[41] = new dCheckProcedure(HGLME41);
            CheckProcedures[42] = new dCheckProcedure(HGLME42);
            CheckProcedures[43] = new dCheckProcedure(HGLME43);
        }

        #endregion

        #region HGLMEDefaultChecks Checks

        public static string HGLME1(cCategory Category, ref bool Log)
        //Initialize Hg LME Default Test Variables
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("Hg_LME_Default_Units_Tested", null, eParameterDataType.String);
                Category.SetCheckParameter("Hg_LME_Default_Calc_Maximum_Concentration", 0m, eParameterDataType.Decimal);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HGLME1");
            }

            return ReturnVal;
        }

        public static string HGLME2(cCategory Category, ref bool Log)
        //Hg LME Default Test Reason Code Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentHGLMETest = Category.GetCheckParameter("Current_Hg_LME_Default_Test").ValueAsDataRowView();
                string TestReasCd = cDBConvert.ToString(CurrentHGLMETest["TEST_REASON_CD"]);
                if (!TestReasCd.InList("INITIAL,QA,RECERT"))
                {
                    DataView TestReasLookup = Category.GetCheckParameter("Test_Reason_Code_Lookup_Table").ValueAsDataView();
                    sFilterPair[] Filter = new sFilterPair[1];
                    Filter[0].Set("TEST_REASON_CD", TestReasCd);
                    if (CountRows(TestReasLookup, Filter) == 0)
                        Category.CheckCatalogResult = "A";
                    else
                        Category.CheckCatalogResult = "B";
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HGLME2");
            }

            return ReturnVal;
        }

        public static string HGLME3(cCategory Category, ref bool Log)
        //Determine Test Begin and End Time
        {
            string ReturnVal = "";

            try
            {
                DataView HGLMERunRecs = Category.GetCheckParameter("Hg_LME_Default_Run_Records").ValueAsDataView();
                Category.SetCheckParameter("Hg_LME_Default_Test_Begin_Date", null, eParameterDataType.Date);
                Category.SetCheckParameter("Hg_LME_Default_Test_Begin_Hour", null, eParameterDataType.Integer);
                Category.SetCheckParameter("Hg_LME_Default_Test_Begin_Minute", null, eParameterDataType.Integer);
                Category.SetCheckParameter("Hg_LME_Default_Test_End_Date", null, eParameterDataType.Date);
                Category.SetCheckParameter("Hg_LME_Default_Test_End_Hour", null, eParameterDataType.Integer);
                Category.SetCheckParameter("Hg_LME_Default_Test_End_Minute", null, eParameterDataType.Integer);
                int RecsCount = HGLMERunRecs.Count;
                bool RunTimesValid;
                if (RecsCount == 0)
                    RunTimesValid = false;
                else
                {
                    RunTimesValid = true;
                    HGLMERunRecs.Sort = "BEGIN_DATE, BEGIN_HOUR, BEGIN_MIN";
                    DateTime BeginDate;
                    int BeginHour;
                    int BeginMin;
                    DateTime EndDate;
                    int EndHour;
                    int EndMin;
                    DateTime LastEndDate = DateTime.MinValue;

                    for (int i = 0; i < RecsCount; i++)
                    {
                        BeginDate = cDBConvert.ToDate(HGLMERunRecs[i]["BEGIN_DATE"], DateTypes.START);
                        BeginHour = cDBConvert.ToInteger(HGLMERunRecs[i]["BEGIN_HOUR"]);
                        BeginMin = cDBConvert.ToInteger(HGLMERunRecs[i]["BEGIN_MIN"]);
                        EndDate = cDBConvert.ToDate(HGLMERunRecs[i]["END_DATE"], DateTypes.END);
                        EndHour = cDBConvert.ToInteger(HGLMERunRecs[i]["END_HOUR"]);
                        EndMin = cDBConvert.ToInteger(HGLMERunRecs[i]["END_MIN"]);
                        if (i == 0)//first run of test
                        {
                            if (BeginDate == DateTime.MinValue || BeginHour < 0 || 23 < BeginHour || BeginMin < 0 || 59 < BeginMin)
                                RunTimesValid = false;
                            else
                            {
                                Category.SetCheckParameter("Hg_LME_Default_Test_Begin_Date", BeginDate, eParameterDataType.Date);
                                Category.SetCheckParameter("Hg_LME_Default_Test_Begin_Hour", BeginHour, eParameterDataType.Integer);
                                Category.SetCheckParameter("Hg_LME_Default_Test_Begin_Minute", BeginMin, eParameterDataType.Integer);
                            }
                            if (EndDate == DateTime.MaxValue || EndHour < 0 || 23 < EndHour || EndMin < 0 || 59 < EndMin)
                                RunTimesValid = false;
                            else
                            {
                                Category.SetCheckParameter("Hg_LME_Default_Test_End_Date", EndDate, eParameterDataType.Date);
                                Category.SetCheckParameter("Hg_LME_Default_Test_End_Hour", EndHour, eParameterDataType.Integer);
                                Category.SetCheckParameter("Hg_LME_Default_Test_End_Minute", EndMin, eParameterDataType.Integer);
                            }
                        }
                        else
                        {
                            if (EndDate == DateTime.MaxValue || EndHour < 0 || 23 < EndHour || EndMin < 0 || 59 < EndMin)
                                RunTimesValid = false;
                            else
                            {
                                Category.SetCheckParameter("Hg_LME_Default_Test_End_Date", EndDate, eParameterDataType.Date);
                                Category.SetCheckParameter("Hg_LME_Default_Test_End_Hour", EndHour, eParameterDataType.Integer);
                                Category.SetCheckParameter("Hg_LME_Default_Test_End_Minute", EndMin, eParameterDataType.Integer);
                            }
                        }
                    }
                }
                Category.SetCheckParameter("Hg_LME_Default_Run_Times_Valid", RunTimesValid, eParameterDataType.Boolean);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HGLME3");
            }

            return ReturnVal;
        }

        public static string HGLME4(cCategory Category, ref bool Log)
        //Maximum Operating Hours Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentHGLMETest = Category.GetCheckParameter("Current_Hg_LME_Default_Test").ValueAsDataRowView();
                int MaxOpHrs = cDBConvert.ToInteger(CurrentHGLMETest["MAX_OPERATING_HOURS"]);
                if (MaxOpHrs == int.MinValue)
                    Category.CheckCatalogResult = "A";
                else
                    if (MaxOpHrs <= 0 || 8760 < MaxOpHrs)
                        Category.CheckCatalogResult = "B";
                    else
                        if (MaxOpHrs < 8760)
                            Category.CheckCatalogResult = "C";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HGLME4");
            }

            return ReturnVal;
        }

        public static string HGLME5(cCategory Category, ref bool Log)
        //Reference Method Code Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView HGLMEDefaultLevel = Category.GetCheckParameter("Current_Hg_LME_Default_Test_Level").ValueAsDataRowView();
                string RefMethCd = cDBConvert.ToString(HGLMEDefaultLevel["REF_METHOD_CD"]);
                if (RefMethCd == "")
                    Category.CheckCatalogResult = "A";
                else
                {
                    DataView RefMethLookup = Category.GetCheckParameter("Reference_Method_Code_Lookup_Table").ValueAsDataView();
                    sFilterPair[] Filter = new sFilterPair[1];
                    Filter[0].Set("REF_METHOD_CD", RefMethCd);
                    DataRowView RowFound;
                    if (!FindRow(RefMethLookup, Filter, out RowFound))
                        Category.CheckCatalogResult = "B";
                    else
                        if (!cDBConvert.ToString(RowFound["PARAMETER_CD"]).Contains("HG"))
                            Category.CheckCatalogResult = "C";
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HGLME5");
            }

            return ReturnVal;
        }

        public static string HGLME6(cCategory Category, ref bool Log)
        //Max Potential Hg Mass Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentHGLMETest = Category.GetCheckParameter("Current_Hg_LME_Default_Test").ValueAsDataRowView();
                decimal MaxPotHGMass = cDBConvert.ToDecimal(CurrentHGLMETest["MAX_POTENTIAL_HG_MASS"]);
                if (MaxPotHGMass == decimal.MinValue)
                    Category.CheckCatalogResult = "A";
                else
                    if (MaxPotHGMass <= 0)
                        Category.CheckCatalogResult = "B";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HGLME6");
            }

            return ReturnVal;
        }

        public static string HGLME7(cCategory Category, ref bool Log)
        //Reported Test Frequency Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentHGLMETest = Category.GetCheckParameter("Current_Hg_LME_Default_Test").ValueAsDataRowView();
                string TestFreqCd = cDBConvert.ToString(CurrentHGLMETest["TEST_FREQUENCY_CD"]);
                if (TestFreqCd == "")
                    Category.CheckCatalogResult = "A";
                else
                    if (!TestFreqCd.InList("4QTRS,2QTRS"))
                        Category.CheckCatalogResult = "B";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HGLME7");
            }

            return ReturnVal;
        }

        public static string HGLME8(cCategory Category, ref bool Log)
        //Hg LME Default Test Number of Units in Group Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentHGLMETest = Category.GetCheckParameter("Current_Hg_LME_Default_Test").ValueAsDataRowView();
                string GroupID = cDBConvert.ToString(CurrentHGLMETest["GROUP_ID"]);
                int NumUnits = cDBConvert.ToInteger(CurrentHGLMETest["NUM_UNITS_IN_GROUP"]);
                if (GroupID != "")
                {
                    DataRowView MonitorLocationRec = Category.GetCheckParameter("Current_Monitor_Location").ValueAsDataRowView();
                    string LocId = cDBConvert.ToString(MonitorLocationRec["LOCATION_IDENTIFIER"]);
                    if (LocId.StartsWith("MS") || LocId.StartsWith("CS"))
                        Category.CheckCatalogResult = "A";
                    else
                        if (NumUnits == int.MinValue)
                            Category.CheckCatalogResult = "B";
                        else
                            if (NumUnits < 2)
                                Category.CheckCatalogResult = "C";
                }
                else
                    if (NumUnits != int.MinValue)
                        Category.CheckCatalogResult = "D";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HGLME8");
            }

            return ReturnVal;
        }

        public static string HGLME9(cCategory Category, ref bool Log)
        //Hg LME Default Test Number of Tests for Group Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentHGLMETest = Category.GetCheckParameter("Current_Hg_LME_Default_Test").ValueAsDataRowView();
                int NumTests = cDBConvert.ToInteger(CurrentHGLMETest["NUM_TESTS_FOR_GROUP"]);
                if (CurrentHGLMETest["GROUP_ID"] != DBNull.Value)
                {
                    int NumUnits = cDBConvert.ToInteger(CurrentHGLMETest["NUM_UNITS_IN_GROUP"]);
                    DataRowView MonitorLocationRec = Category.GetCheckParameter("Current_Monitor_Location").ValueAsDataRowView();
                    string LocId = cDBConvert.ToString(MonitorLocationRec["LOCATION_IDENTIFIER"]);
                    if (!LocId.StartsWith("CS") && !LocId.StartsWith("MS"))
                        if (NumTests == int.MinValue)
                            Category.CheckCatalogResult = "A";
                        else
                            if (NumUnits < 4)
                            {
                                if (NumTests < 1)
                                    Category.CheckCatalogResult = "B";
                            }
                            else
                                if (NumUnits < 7)
                                {
                                    if (NumTests < 2)
                                        Category.CheckCatalogResult = "B";
                                }
                                else
                                    if (NumUnits < 11)
                                    {
                                        if (NumTests < 3)
                                            Category.CheckCatalogResult = "B";
                                    }
                                    else
                                    {
                                        int TempVal = (int)Math.Round((decimal)(NumUnits / 3), MidpointRounding.AwayFromZero);
                                        if (NumTests < TempVal)
                                            Category.CheckCatalogResult = "B";
                                    }
                }
                else
                    if (NumTests != int.MinValue)
                        Category.CheckCatalogResult = "C";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HGLME9");
            }

            return ReturnVal;
        }

        public static string HGLME10(cCategory Category, ref bool Log)
        //Hg LME Default Test Begin Time Consistent with Run Times
        {
            string ReturnVal = "";

            try
            {
                 if (Category.GetCheckParameter("Hg_LME_Default_Run_Times_Valid").ValueAsBool() &&
                    Category.GetCheckParameter("Test_Begin_Date_Valid").ValueAsBool() &&
                    Category.GetCheckParameter("Test_Begin_Hour_Valid").ValueAsBool() &&
                    Category.GetCheckParameter("Test_Begin_Minute_Valid").ValueAsBool())
                {
                    DataRowView CurrentHGLMETest = Category.GetCheckParameter("Current_Hg_LME_Default_Test").ValueAsDataRowView();
                    if (cDBConvert.ToDate(CurrentHGLMETest["BEGIN_DATE"], DateTypes.START) != Category.GetCheckParameter("Hg_LME_Default_Test_Begin_Date").ValueAsDateTime(DateTypes.START) ||
                        cDBConvert.ToInteger(CurrentHGLMETest["BEGIN_HOUR"]) != Category.GetCheckParameter("Hg_LME_Default_Test_Begin_Hour").ValueAsInt() ||
                        cDBConvert.ToInteger(CurrentHGLMETest["BEGIN_MIN"]) != Category.GetCheckParameter("Hg_LME_Default_Test_Begin_Minute").ValueAsInt())
                        Category.CheckCatalogResult = "A";
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HGLME10");
            }

            return ReturnVal;
        }

        public static string HGLME11(cCategory Category, ref bool Log)
        //Hg LME Default Test End Time Consistent with Run Times
        {
            string ReturnVal = "";

            try
            {
                if (Category.GetCheckParameter("Hg_LME_Default_Run_Times_Valid").ValueAsBool() &&
                    Category.GetCheckParameter("Test_End_Date_Valid").ValueAsBool() &&
                    Category.GetCheckParameter("Test_End_Hour_Valid").ValueAsBool() &&
                    Category.GetCheckParameter("Test_End_Minute_Valid").ValueAsBool())
                {
                    DataRowView CurrentHGLMETest = Category.GetCheckParameter("Current_Hg_LME_Default_Test").ValueAsDataRowView();
                    if (cDBConvert.ToDate(CurrentHGLMETest["END_DATE"], DateTypes.END) != Category.GetCheckParameter("Hg_LME_Default_Test_End_Date").ValueAsDateTime(DateTypes.END) ||
                        cDBConvert.ToInteger(CurrentHGLMETest["END_HOUR"]) != Category.GetCheckParameter("Hg_LME_Default_Test_End_Hour").ValueAsInt() ||
                        cDBConvert.ToInteger(CurrentHGLMETest["END_MIN"]) != Category.GetCheckParameter("Hg_LME_Default_Test_End_Minute").ValueAsInt())
                        Category.CheckCatalogResult = "A";
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HGLME11");
            }

            return ReturnVal;
        }

        public static string HGLME12(cCategory Category, ref bool Log)
        //Identification of Previously Reported Test or Test Number for Hg LME Default Test
        {
            string ReturnVal = "";

            try
            {
                if (Category.GetCheckParameter("Test_End_Date_Valid").ValueAsBool() &&
                    Category.GetCheckParameter("Test_End_Hour_Valid").ValueAsBool() &&
                    Category.GetCheckParameter("Test_End_Minute_Valid").ValueAsBool())
                {
                    Category.SetCheckParameter("Extra_Hg_LME_Default_Test", false, eParameterDataType.Boolean);
                    DataRowView CurrentHGLMETest = Category.GetCheckParameter("Current_Hg_LME_Default_Test").ValueAsDataRowView();
                    DateTime EndDate = cDBConvert.ToDate(CurrentHGLMETest["END_DATE"], DateTypes.END);
                    int EndHour = cDBConvert.ToInteger(CurrentHGLMETest["END_HOUR"]);
                    int EndMin = cDBConvert.ToInteger(CurrentHGLMETest["END_MIN"]);
                    string TestNum = cDBConvert.ToString(CurrentHGLMETest["TEST_NUM"]);
                    DataView HGLMETestRecs = Category.GetCheckParameter("Hg_LME_Default_Test_Records").ValueAsDataView();
                    sFilterPair[] Filter = new sFilterPair[4];
                    Filter[0].Set("END_DATE", EndDate, eFilterDataType.DateEnded);
                    Filter[1].Set("END_HOUR", EndHour, eFilterDataType.Integer);
                    if (EndMin != int.MinValue)
                        Filter[2].Set("END_MIN", EndMin, eFilterDataType.Integer);
                    else
                        Filter[2].Set("END_MIN", DBNull.Value, eFilterDataType.Integer);
                    Filter[3].Set("TEST_NUM", TestNum, true);
                    DataView TestRecsFound = FindRows(HGLMETestRecs, Filter);
                    if ((TestRecsFound.Count > 0 && CurrentHGLMETest["TEST_SUM_ID"] == DBNull.Value) ||
                        (TestRecsFound.Count > 1 && CurrentHGLMETest["TEST_SUM_ID"] != DBNull.Value) ||
                        (TestRecsFound.Count == 1 && CurrentHGLMETest["TEST_SUM_ID"] != DBNull.Value && CurrentHGLMETest["TEST_SUM_ID"].ToString() != TestRecsFound[0]["TEST_SUM_ID"].ToString()))
                    {
                        Category.SetCheckParameter("Extra_Hg_LME_Default_Test", true, eParameterDataType.Boolean);
                        Category.CheckCatalogResult = "A";
                    }
                    else
                    {
                        DataView QASuppRecs = Category.GetCheckParameter("QA_Supplemental_Data_Records").ValueAsDataView();
                        Filter = new sFilterPair[5];
                        Filter[0].Set("END_DATE", EndDate, eFilterDataType.DateEnded);
                        Filter[1].Set("END_HOUR", EndHour, eFilterDataType.Integer);
                        if (EndMin != int.MinValue)
                            Filter[2].Set("END_MIN", EndMin, eFilterDataType.Integer);
                        else
                            Filter[2].Set("END_MIN", DBNull.Value, eFilterDataType.Integer);
                        Filter[3].Set("TEST_TYPE_CD", "HGLME");
                        Filter[4].Set("TEST_NUM", TestNum, true);
                        DataView QASuppRecsFound = FindRows(QASuppRecs, Filter);
                        if ((QASuppRecsFound.Count > 0 && CurrentHGLMETest["TEST_SUM_ID"] == DBNull.Value) ||
                            (QASuppRecsFound.Count > 1 && CurrentHGLMETest["TEST_SUM_ID"] != DBNull.Value) ||
                            (QASuppRecsFound.Count == 1 && CurrentHGLMETest["TEST_SUM_ID"] != DBNull.Value && CurrentHGLMETest["TEST_SUM_ID"].ToString() != QASuppRecsFound[0]["TEST_SUM_ID"].ToString()))
                        {
                            Category.SetCheckParameter("Extra_Hg_LME_Default_Test", true, eParameterDataType.Boolean);
                            Category.CheckCatalogResult = "A";
                        }
                        else
                        {
                            Filter = new sFilterPair[2];
                            Filter[0].Set("TEST_TYPE_CD", "HGLME");
                            Filter[1].Set("TEST_NUM", TestNum);
                            QASuppRecsFound = FindRows(QASuppRecs, Filter);
                            if (QASuppRecsFound.Count > 0)
                                if (cDBConvert.ToString(QASuppRecsFound[0]["CAN_SUBMIT"]) == "N")
                                {
                                    if ((QASuppRecsFound.Count > 0 && CurrentHGLMETest["TEST_SUM_ID"] == DBNull.Value) ||
                                        (QASuppRecsFound.Count > 1 && CurrentHGLMETest["TEST_SUM_ID"] != DBNull.Value) ||
                                        (QASuppRecsFound.Count == 1 && CurrentHGLMETest["TEST_SUM_ID"] != DBNull.Value && CurrentHGLMETest["TEST_SUM_ID"].ToString() != QASuppRecsFound[0]["TEST_SUM_ID"].ToString()) &&
                                        EndDate != cDBConvert.ToDate(QASuppRecsFound[0]["END_DATE"], DateTypes.END) ||
                                        EndHour != cDBConvert.ToInteger(QASuppRecsFound[0]["END_HOUR"]) ||
                                        EndMin != cDBConvert.ToInteger(QASuppRecsFound[0]["END_MIN"]))
                                        Category.CheckCatalogResult = "B";
                                    else
                                        Category.CheckCatalogResult = "C";
                                }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HGLME12");
            }

            return ReturnVal;
        }

        public static string HGLME13(cCategory Category, ref bool Log)
        //Concurrent Hg LME Default Tests
        {
            string ReturnVal = "";

            try
            {
                if (Category.GetCheckParameter("Test_Dates_Consistent").ValueAsBool())
                {
                    DataRowView CurrentHGLMETest = Category.GetCheckParameter("Current_Hg_LME_Default_Test").ValueAsDataRowView();

                    DateTime BeginDate = cDBConvert.ToDate(CurrentHGLMETest["BEGIN_DATE"], DateTypes.START);
                    int BeginHour = cDBConvert.ToInteger(CurrentHGLMETest["BEGIN_HOUR"]);
                    int BeginMin = cDBConvert.ToInteger(CurrentHGLMETest["BEGIN_MIN"]);
                    DateTime EndDate = cDBConvert.ToDate(CurrentHGLMETest["END_DATE"], DateTypes.END);
                    int EndHour = cDBConvert.ToInteger(CurrentHGLMETest["END_HOUR"]);
                    int EndMin = cDBConvert.ToInteger(CurrentHGLMETest["END_MIN"]);
                    string TestNum = cDBConvert.ToString(CurrentHGLMETest["TEST_NUM"]);

                    DataView HGLMETestRecs = Category.GetCheckParameter("Hg_LME_Default_Test_Records").ValueAsDataView();
                    sFilterPair[] Filter = new sFilterPair[1];
                    Filter[0].Set("TEST_NUM", TestNum, true);
                    DataView TestRecsFound = FindActiveRows(HGLMETestRecs, BeginDate, BeginHour, BeginMin, EndDate, EndHour, EndMin,
                        "BEGIN_DATE", "BEGIN_HOUR", "BEGIN_MIN", "END_DATE", "END_HOUR", "END_MIN", false, false, Filter);
                    if (TestRecsFound.Count > 0)
                        Category.CheckCatalogResult = "A";
                    else
                    {
                        DataView QASuppRecs = Category.GetCheckParameter("QA_Supplemental_Data_Records").ValueAsDataView();
                        Filter = new sFilterPair[3];
                        Filter[0].Set("TEST_SUM_ID", cDBConvert.ToString(CurrentHGLMETest["TEST_SUM_ID"]), true);
                        Filter[1].Set("TEST_NUM", TestNum, true);
                        Filter[2].Set("TEST_TYPE_CD", "HGLME");
                        DataView QASuppRecsFound = FindActiveRows(QASuppRecs, BeginDate, BeginHour, BeginMin, EndDate, EndHour, EndMin,
                            "BEGIN_DATE", "BEGIN_HOUR", "BEGIN_MIN", "END_DATE", "END_HOUR", "END_MIN", false, false, Filter);
                        if (QASuppRecsFound.Count > 0)
                            Category.CheckCatalogResult = "A";
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HGLME13");
            }

            return ReturnVal;
        }

        public static string HGLME14(cCategory Category, ref bool Log)
        //Operating Level Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView HGLMEDefaultLevel = Category.GetCheckParameter("Current_Hg_LME_Default_Test_Level").ValueAsDataRowView();
                Category.SetCheckParameter("Hg_LME_Default_Operating_Level_Valid", false, eParameterDataType.Boolean);
                string OpLvlCd = cDBConvert.ToString(HGLMEDefaultLevel["HG_OP_LEVEL_CD"]);
                if (OpLvlCd == "")
                    Category.CheckCatalogResult = "A";
                else
                {
                    DataView OpLvlLookup = Category.GetCheckParameter("Hg_LME_Operating_Level_Code_Lookup_Table").ValueAsDataView();
                    sFilterPair[] Filter = new sFilterPair[1];
                    Filter[0].Set("OP_LEVEL_CD", OpLvlCd);
                    if (CountRows(OpLvlLookup, Filter) == 0)
                        Category.CheckCatalogResult = "B";
                    else
                        if (cDBConvert.ToInteger(HGLMEDefaultLevel["CS_UNIT_TEST_IND"]) == 1)
                            if (OpLvlCd == "T")
                                Category.SetCheckParameter("Hg_LME_Default_Operating_Level_Valid", true, eParameterDataType.Boolean);
                            else
                                Category.CheckCatalogResult = "C";
                        else
                            if (OpLvlCd == "T")
                                Category.CheckCatalogResult = "B";
                            else
                                if (Category.GetCheckParameter("Test_Dates_Consistent").ValueAsBool())
                                {
                                    DataView MonitorQualificationRecords = (DataView)Category.GetCheckParameter("Qualification_Records").ParameterValue;
                                    string OldFilter = MonitorQualificationRecords.RowFilter;
                                    DateTime BeginDate = cDBConvert.ToDate(HGLMEDefaultLevel["BEGIN_DATE"], DateTypes.START);
                                    DateTime EndDate = cDBConvert.ToDate(HGLMEDefaultLevel["END_DATE"], DateTypes.END);
                                    string LocID = cDBConvert.ToString(HGLMEDefaultLevel["LOCATION_IDENTIFIER"]);
                                    string MonLocID = cDBConvert.ToString(HGLMEDefaultLevel["MON_LOC_ID"]);
                                    if (LocID.StartsWith("CS") || LocID.StartsWith("MS"))
                                    {
                                        Boolean FoundSome = false;
                                        Boolean SomeNoHits = false;
                                        DataView UnitStackConfigurationRecords = (DataView)Category.GetCheckParameter("Unit_Stack_Configuration_Records").ParameterValue;
                                        string OldFilter2 = UnitStackConfigurationRecords.RowFilter;
                                        UnitStackConfigurationRecords.RowFilter = AddToDataViewFilter(OldFilter2,
                                            "BEGIN_DATE < '" + BeginDate.AddDays(1).ToShortDateString() + "'" + " AND (END_DATE IS NULL OR END_DATE >= '" +
                                            EndDate.ToShortDateString() + "')");
                                        if (UnitStackConfigurationRecords.Count > 0)
                                        {
                                            foreach (DataRowView USCRecord in UnitStackConfigurationRecords)
                                            {
                                                MonitorQualificationRecords.RowFilter = AddToDataViewFilter(OldFilter,
                                                  "MON_LOC_ID = '" + cDBConvert.ToString(USCRecord["STACK_PIPE_MON_LOC_ID"]) +
                                                  "' and QUAL_TYPE_CD IN ('PK', 'SK')" + " AND BEGIN_DATE <= '" + BeginDate.ToShortDateString() +
                                                  "'" + " AND (END_DATE IS NULL OR END_DATE >= '" + EndDate.ToShortDateString() + "')");
                                                if (MonitorQualificationRecords.Count > 0)
                                                    FoundSome = true;
                                                else
                                                    SomeNoHits = true;
                                            }
                                            if (!FoundSome && OpLvlCd == "N")
                                                Category.CheckCatalogResult = "D";
                                            else
                                                if (!SomeNoHits && OpLvlCd != "N")
                                                    Category.CheckCatalogResult = "E";
                                                else
                                                    Category.SetCheckParameter("Hg_LME_Default_Operating_Level_Valid", true, eParameterDataType.Boolean);
                                        }
                                        UnitStackConfigurationRecords.RowFilter = OldFilter2;
                                    }
                                    else
                                    {
                                        MonitorQualificationRecords.RowFilter = AddToDataViewFilter(OldFilter,
                                          "MON_LOC_ID = '" + MonLocID + "'" + " AND QUAL_TYPE_CD IN ('PK','SK') AND BEGIN_DATE <= '" + BeginDate.ToShortDateString() +
                                          "'" + " AND (END_DATE IS NULL OR END_DATE >= '" + EndDate.ToShortDateString() + "')");
                                        if (MonitorQualificationRecords.Count == 0 && OpLvlCd == "N")
                                            Category.CheckCatalogResult = "D";
                                        else
                                            if (MonitorQualificationRecords.Count > 0 && OpLvlCd != "N")
                                                Category.CheckCatalogResult = "E";
                                            else
                                                Category.SetCheckParameter("Hg_LME_Default_Operating_Level_Valid", true, eParameterDataType.Boolean);
                                    }
                                    MonitorQualificationRecords.RowFilter = OldFilter;
                                }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HGLME14");
            }

            return ReturnVal;
        }

        public static string HGLME15(cCategory Category, ref bool Log)
        //Initialize Hg Default Test Level Data Variables
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("Hg_LME_Default_Run_Count", 0, eParameterDataType.Integer);
                Category.SetCheckParameter("Hg_LME_Default_Calc_Max_Run_Value", 0, eParameterDataType.Decimal);
                Category.SetCheckParameter("Hg_LME_Default_Runs_Sequential", true, eParameterDataType.Boolean);
                Category.SetCheckParameter("Hg_LME_Default_Last_End_Date", null, eParameterDataType.Date);
                Category.SetCheckParameter("Hg_LME_Default_Last_End_Hour", null, eParameterDataType.Integer);
                Category.SetCheckParameter("Hg_LME_Default_Last_End_Minute", null, eParameterDataType.Integer);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HGLME15");
            }

            return ReturnVal;
        }

        public static string HGLME16(cCategory Category, ref bool Log)
        //Test Location ID Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentHGLMETest = Category.GetCheckParameter("Current_Hg_LME_Default_Test").ValueAsDataRowView();
                DataRowView HGLMEDefaultLevel = Category.GetCheckParameter("Current_Hg_LME_Default_Test_Level").ValueAsDataRowView();
                string TestLocId = cDBConvert.ToString(HGLMEDefaultLevel["TEST_LOCATION_ID"]);
                int CSUnitTestInd = cDBConvert.ToInteger(CurrentHGLMETest["CS_UNIT_TEST_IND"]);
                if (TestLocId == "")
                    Category.CheckCatalogResult = "A";
                else
                {
                    string LocId = cDBConvert.ToString(CurrentHGLMETest["LOCATION_IDENTIFIER"]);
                    if (TestLocId == LocId)
                    {
                        if (LocId.StartsWith("CS") && CSUnitTestInd == 1)
                            Category.CheckCatalogResult = "B";
                    }
                    else
                        if (LocId.StartsWith("CS"))
                            if (CSUnitTestInd == 1)
                            {
                                DataView UnitStackConfigRecs = Category.GetCheckParameter("Unit_Stack_Configuration_Records").ValueAsDataView();
                                sFilterPair[] Filter = new sFilterPair[2];
                                Filter[0].Set("STACK_PIPE_MON_LOC_ID", cDBConvert.ToString(HGLMEDefaultLevel["MON_LOC_ID"]));
                                Filter[1].Set("UNITID", TestLocId);
                                DataView UnitStackConfigRecsFound = FindRows(UnitStackConfigRecs, Filter);
                                DateTime EndDate = cDBConvert.ToDate(CurrentHGLMETest["END_DATE"], DateTypes.END);
                                bool foundOne = false;
                                foreach (DataRowView drv in UnitStackConfigRecsFound)
                                    if (drv["END_DATE"] == null || cDBConvert.ToDate(drv["END_DATE"], DateTypes.END) > EndDate)
                                    {
                                        foundOne = true;
                                        string UnitsTested = Category.GetCheckParameter("Hg_LME_Default_Units_Tested").ValueAsString();
                                        UnitsTested = UnitsTested.ListAdd(TestLocId);
                                        Category.SetCheckParameter("Hg_LME_Default_Units_Tested", UnitsTested, eParameterDataType.String);
                                        break;
                                    }
                                if (!foundOne)
                                    Category.CheckCatalogResult = "C";
                            }
                            else
                                Category.CheckCatalogResult = "D";
                        else
                            Category.CheckCatalogResult = "E";
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HGLME16");
            }

            return ReturnVal;
        }

        public static string HGLME17(cCategory Category, ref bool Log)
        //Max Run Value Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView HGLMEDefaultLevel = Category.GetCheckParameter("Current_Hg_LME_Default_Test_Level").ValueAsDataRowView();
                decimal MaxRunVal = cDBConvert.ToDecimal(HGLMEDefaultLevel["MAX_RUN_VALUE"]);
                if (MaxRunVal == decimal.MinValue)
                    Category.CheckCatalogResult = "A";
                else
                    if (MaxRunVal <= 0)
                        Category.CheckCatalogResult = "B";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HGLME17");
            }

            return ReturnVal;
        }

        public static string HGLME18(cCategory Category, ref bool Log)
        //Run Number Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView HGLMEDefaultRun = Category.GetCheckParameter("Current_Hg_LME_Default_Run").ValueAsDataRowView();
                int RunNum = cDBConvert.ToInteger(HGLMEDefaultRun["RUN_NUM"]);
                if (RunNum == int.MinValue)
                    Category.CheckCatalogResult = "A";
                else
                    if (RunNum <= 0)
                        Category.CheckCatalogResult = "B";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HGLME18");
            }

            return ReturnVal;
        }

        public static string HGLME19(cCategory Category, ref bool Log)
        //Run Begin Time Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView HGLMEDefaultRun = Category.GetCheckParameter("Current_Hg_LME_Default_Run").ValueAsDataRowView();
                DateTime BeginDate = cDBConvert.ToDate(HGLMEDefaultRun["BEGIN_DATE"], DateTypes.START);
                int BeginHour = cDBConvert.ToInteger(HGLMEDefaultRun["BEGIN_HOUR"]);
                int BeginMin = cDBConvert.ToInteger(HGLMEDefaultRun["BEGIN_MIN"]);

                if (BeginDate == DateTime.MinValue || BeginHour < 0 || 23 < BeginHour || BeginMin < 0 || 59 < BeginMin)
                {
                    Category.CheckCatalogResult = "A";
                    Category.SetCheckParameter("Hg_LME_Default_Run_Begin_Time_Valid", false, eParameterDataType.Boolean);
                }
                else
                    Category.SetCheckParameter("Hg_LME_Default_Run_Begin_Time_Valid", true, eParameterDataType.Boolean);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HGLME19");
            }

            return ReturnVal;
        }

        public static string HGLME20(cCategory Category, ref bool Log)
        //Train1 Value Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView HGLMEDefaultRun = Category.GetCheckParameter("Current_Hg_LME_Default_Run").ValueAsDataRowView();                
                string RefMethCd = ""; 
                if (Category.GetCheckParameter("Current_Hg_LME_Default_Test_Level").ParameterValue != null)
                {
                    DataRowView HGLMEDefaultLevel = Category.GetCheckParameter("Current_Hg_LME_Default_Test_Level").ValueAsDataRowView();
                    RefMethCd = cDBConvert.ToString(HGLMEDefaultLevel["REF_METHOD_CD"]);
                }
                decimal Train1Val = cDBConvert.ToDecimal(HGLMEDefaultRun["TRAIN_VALUE_1"]);
                switch (RefMethCd)
                {
                    case "OH":
                    case "29":
                    case "30B":
                        if (Train1Val == decimal.MinValue)
                            Category.CheckCatalogResult = "A";
                        else
                            if (Train1Val <= 0)
                                Category.CheckCatalogResult = "B";
                        break;
                    case "30A":
                        if (Train1Val != decimal.MinValue)
                            Category.CheckCatalogResult = "C";
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HGLME20");
            }

            return ReturnVal;
        }

        public static string HGLME21(cCategory Category, ref bool Log)
        //Train2 Value Valid
        {
            string ReturnVal = "";

            try
            {                
                DataRowView HGLMEDefaultRun = Category.GetCheckParameter("Current_Hg_LME_Default_Run").ValueAsDataRowView();
                string RefMethCd = "";
                if (Category.GetCheckParameter("Current_Hg_LME_Default_Test_Level").ParameterValue != null)
                {
                    DataRowView HGLMEDefaultLevel = Category.GetCheckParameter("Current_Hg_LME_Default_Test_Level").ValueAsDataRowView();
                    RefMethCd = cDBConvert.ToString(HGLMEDefaultLevel["REF_METHOD_CD"]);
                }              
                decimal Train2Val = cDBConvert.ToDecimal(HGLMEDefaultRun["TRAIN_VALUE_2"]);
                switch (RefMethCd)
                {
                    case "OH":
                    case "29":
                    case "30B":
                        if (Train2Val == decimal.MinValue)
                            Category.CheckCatalogResult = "A";
                        else
                            if (Train2Val <= 0)
                                Category.CheckCatalogResult = "B";
                        break;
                    case "30A":
                        if (Train2Val != decimal.MinValue)
                            Category.CheckCatalogResult = "C";
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HGLME21");
            }

            return ReturnVal;
        }

        public static string HGLME22(cCategory Category, ref bool Log)
        //Reference Value for Run Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView HGLMEDefaultRun = Category.GetCheckParameter("Current_Hg_LME_Default_Run").ValueAsDataRowView();
                decimal RefVal = cDBConvert.ToDecimal(HGLMEDefaultRun["REFERENCE_VALUE_AVERAGE"]);
                decimal CalcMaxRunVal = Category.GetCheckParameter("Hg_LME_Default_Calc_Max_Run_Value").ValueAsDecimal();
                if (RefVal == decimal.MinValue)
                {
                    CalcMaxRunVal = -1;
                    Category.CheckCatalogResult = "A";
                }
                else
                    if (RefVal <= 0)
                    {
                        CalcMaxRunVal = -1m;
                        Category.CheckCatalogResult = "B";
                    }
                    else
                    {
                        string RefMethCd = "";
                        if (Category.GetCheckParameter("Current_Hg_LME_Default_Test_Level").ParameterValue != null)
                        {
                            DataRowView HGLMEDefaultLevel = Category.GetCheckParameter("Current_Hg_LME_Default_Test_Level").ValueAsDataRowView();
                            RefMethCd = cDBConvert.ToString(HGLMEDefaultLevel["REF_METHOD_CD"]);
                        }
                        if (RefMethCd.InList("OH,29,30B"))
                        {
                            decimal Train1 = cDBConvert.ToDecimal(HGLMEDefaultRun["TRAIN_VALUE_1"]);
                            decimal Train2 = cDBConvert.ToDecimal(HGLMEDefaultRun["TRAIN_VALUE_2"]);
                            if (Train1 > 0 && Train2 > 0)
                            {
                                if (RefVal != Math.Round((Train1 + Train2) / 2, 2, MidpointRounding.AwayFromZero))
                                {
                                    CalcMaxRunVal = -1m;
                                    Category.CheckCatalogResult = "C";
                                }
                                else
                                    if (CalcMaxRunVal >= 0 && CalcMaxRunVal < RefVal)
                                        CalcMaxRunVal = RefVal;
                            }
                            else
                                CalcMaxRunVal = -1m;
                        }
                        else
                            if (CalcMaxRunVal >= 0 && CalcMaxRunVal < RefVal)
                                CalcMaxRunVal = RefVal;
                    }
                Category.SetCheckParameter("Hg_LME_Default_Calc_Max_Run_Value", CalcMaxRunVal, eParameterDataType.Decimal);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HGLME22");
            }

            return ReturnVal;
        }

        public static string HGLME23(cCategory Category, ref bool Log)
        //Calculate Relative Deviation
        {
            string ReturnVal = "";

            try
            {
                DataRowView HGLMEDefaultRun = Category.GetCheckParameter("Current_Hg_LME_Default_Run").ValueAsDataRowView();
                Category.SetCheckParameter("Hg_LME_Default_RD", null, eParameterDataType.Decimal);
                string RefMethCd = "";
                if (Category.GetCheckParameter("Current_Hg_LME_Default_Test_Level").ParameterValue != null)
                {
                    DataRowView HGLMEDefaultLevel = Category.GetCheckParameter("Current_Hg_LME_Default_Test_Level").ValueAsDataRowView();
                    RefMethCd = cDBConvert.ToString(HGLMEDefaultLevel["REF_METHOD_CD"]);
                }  
                decimal Train1Val = cDBConvert.ToDecimal(HGLMEDefaultRun["TRAIN_VALUE_1"]);
                decimal Train2Val = cDBConvert.ToDecimal(HGLMEDefaultRun["TRAIN_VALUE_2"]);

                if (RefMethCd.InList("OH,29,30B") && Train1Val > 0 && Train2Val > 0)
                {
                    decimal tempDiff = Math.Abs(Train1Val - Train2Val);
                    decimal RD = Math.Round(tempDiff / (Train1Val + Train2Val) * 100, MidpointRounding.AwayFromZero);
                    Category.SetCheckParameter("Hg_LME_Default_RD", RD, eParameterDataType.Decimal);
                    if (RD <= 10)
                    {
                        if (cDBConvert.ToInteger(HGLMEDefaultRun["APS_IND"]) == 1)
                            Category.CheckCatalogResult = "A";
                    }
                    else
                        if (tempDiff > 0.03m && (tempDiff > 0.2m || RefMethCd != "30B"))
                        {
                            if (Math.Round((Train1Val + Train2Val) / 2, 2, MidpointRounding.AwayFromZero) > 1 || RD > 20)
                                Category.CheckCatalogResult = "B";
                            else
                                if (cDBConvert.ToInteger(HGLMEDefaultRun["APS_IND"]) != 1)
                                    Category.CheckCatalogResult = "C";
                        }
                        else
                            if (cDBConvert.ToInteger(HGLMEDefaultRun["APS_IND"]) != 1)
                                Category.CheckCatalogResult = "C";
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HGLME23");
            }

            return ReturnVal;
        }

        public static string HGLME24(cCategory Category, ref bool Log)
        //Gross Unit Load Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView HGLMEDefaultRun = Category.GetCheckParameter("Current_Hg_LME_Default_Run").ValueAsDataRowView();
                decimal GrUnitLoad = cDBConvert.ToDecimal(HGLMEDefaultRun["GROSS_UNIT_LOAD"]);
                if (GrUnitLoad == decimal.MinValue)
                    Category.CheckCatalogResult = "A";
                else
                    if (GrUnitLoad <= 0)
                        Category.CheckCatalogResult = "B";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HGLME24");
            }

            return ReturnVal;
        }

        public static string HGLME25(cCategory Category, ref bool Log)
        //Run End Time Valid
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("Hg_LME_Default_Run_Dates_Consistent", false, eParameterDataType.Boolean);
                DataRowView HGLMEDefaultRun = Category.GetCheckParameter("Current_Hg_LME_Default_Run").ValueAsDataRowView();
                DateTime EndDate = cDBConvert.ToDate(HGLMEDefaultRun["END_DATE"], DateTypes.END);
                int EndHour = cDBConvert.ToInteger(HGLMEDefaultRun["END_HOUR"]);
                int EndMin = cDBConvert.ToInteger(HGLMEDefaultRun["END_MIN"]);

                if (EndDate == DateTime.MinValue || EndHour < 0 || 23 < EndHour || EndMin < 0 || 59 < EndMin)
                    Category.CheckCatalogResult = "A";
                else
                    if (Category.GetCheckParameter("Hg_LME_Default_Run_Begin_Time_Valid").ValueAsBool())
                    {
                        DateTime BeginDate = cDBConvert.ToDate(HGLMEDefaultRun["BEGIN_DATE"], DateTypes.START);
                        int BeginHour = cDBConvert.ToInteger(HGLMEDefaultRun["BEGIN_HOUR"]);
                        int BeginMin = cDBConvert.ToInteger(HGLMEDefaultRun["BEGIN_MIN"]);

                        if (BeginDate > EndDate || (BeginDate == EndDate && (BeginHour > EndHour || (BeginHour == EndHour && BeginMin >= EndMin))))
                            Category.CheckCatalogResult = "B";
                        else
                        {
                            Category.SetCheckParameter("Hg_LME_Default_Run_Dates_Consistent", true, eParameterDataType.Boolean);                            
                            string RefMethCd = "";
                            if (Category.GetCheckParameter("Current_Hg_LME_Default_Test_Level").ParameterValue != null)
                            {
                                DataRowView HGLMEDefaultLevel = Category.GetCheckParameter("Current_Hg_LME_Default_Test_Level").ValueAsDataRowView();
                                RefMethCd = cDBConvert.ToString(HGLMEDefaultLevel["REF_METHOD_CD"]);
                            }
                            if (RefMethCd.InList("30A,30B"))
                            {
                                TimeSpan ts = EndDate - BeginDate;
                                int RunLength = (ts.Days * 24 * 60) + ((EndHour - BeginHour) * 60) + (EndMin - BeginMin);
                                if (RunLength < 59)
                                    Category.CheckCatalogResult = "C";
                            }
                        }
                    }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HGLME25");
            }

            return ReturnVal;
        }

        public static string HGLME26(cCategory Category, ref bool Log)
        //Run Relative Deviation Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView HGLMEDefaultLevel = Category.GetCheckParameter("Current_Hg_LME_Default_Test_Level").ValueAsDataRowView();
                DataRowView HGLMEDefaultRun = Category.GetCheckParameter("Current_Hg_LME_Default_Run").ValueAsDataRowView();
                string RefMethCd = cDBConvert.ToString(HGLMEDefaultLevel["REF_METHOD_CD"]);
                decimal RunRD = cDBConvert.ToDecimal(HGLMEDefaultRun["RUN_RELATIVE_DEVIATION"]);
                switch (RefMethCd)
                {
                    case "OH":
                    case "29":
                    case "30B":
                        if (RunRD == decimal.MinValue)
                            Category.CheckCatalogResult = "A";
                        else
                            if (RunRD < 0)
                                Category.CheckCatalogResult = "B";
                            else
                            {
                                decimal DefaultRD = Category.GetCheckParameter("Hg_LME_Default_RD").ValueAsDecimal();
                                if (RunRD != DefaultRD && DefaultRD != decimal.MinValue)
                                    Category.CheckCatalogResult = "C";
                            }
                        break;
                    case "30A":
                        if (RunRD != decimal.MinValue)
                            Category.CheckCatalogResult = "D";
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HGLME26");
            }

            return ReturnVal;
        }

        public static string HGLME27(cCategory Category, ref bool Log)
        //Gross Unit Load Consistent with Range of Operation
        {
            string ReturnVal = "";

            try
            {
                DataRowView HGLMEDefaultRun = Category.GetCheckParameter("Current_Hg_LME_Default_Run").ValueAsDataRowView();
                decimal GrossUnitLoad = cDBConvert.ToInteger(HGLMEDefaultRun["GROSS_UNIT_LOAD"]);
                string OpLvlCd = cDBConvert.ToString(HGLMEDefaultRun["HG_OP_LEVEL_CD"]);
                if (Category.GetCheckParameter("Hg_LME_Default_Run_Dates_Consistent").ValueAsBool() && GrossUnitLoad > 0 && OpLvlCd.InList("L,H,M"))
                {
                    DateTime BeginDate = cDBConvert.ToDate(HGLMEDefaultRun["BEGIN_DATE"], DateTypes.START);
                    int BeginHour = cDBConvert.ToHour(HGLMEDefaultRun["BEGIN_HOUR"], DateTypes.START);
                    DateTime EndDate = cDBConvert.ToDate(HGLMEDefaultRun["END_DATE"], DateTypes.END);
                    int EndHour = cDBConvert.ToHour(HGLMEDefaultRun["END_HOUR"], DateTypes.END);
                    DataView LoadRecs = Category.GetCheckParameter("Load_Records").ValueAsDataView();
                    DataView LoadRecsFound;
                    LoadRecsFound = FindActiveRows(LoadRecs, BeginDate, BeginHour, EndDate, EndHour);
                    if (LoadRecsFound.Count == 1)
                    {
                        decimal LowOpBound = cDBConvert.ToDecimal(LoadRecsFound[0]["LOW_OP_BOUNDARY"]);
                        decimal UpOpBound = cDBConvert.ToDecimal(LoadRecsFound[0]["UP_OP_BOUNDARY"]);
                        if (LowOpBound > 0 && UpOpBound > LowOpBound)
                        {
                            switch (OpLvlCd)
                            {
                                case "L":
                                    if (GrossUnitLoad < LowOpBound || GrossUnitLoad > (LowOpBound + ((UpOpBound - LowOpBound) * .3m)))
                                        Category.CheckCatalogResult = "A";
                                    break;
                                case "M":
                                    if (GrossUnitLoad <= (LowOpBound + ((UpOpBound - LowOpBound) * .3m)) || GrossUnitLoad > (LowOpBound + ((UpOpBound - LowOpBound) * .6m)))
                                        Category.CheckCatalogResult = "B";
                                    break;
                                case "H":
                                    if (GrossUnitLoad <= (LowOpBound + ((UpOpBound - LowOpBound) * .6m)) || GrossUnitLoad > UpOpBound)
                                        Category.CheckCatalogResult = "C";
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                            Category.CheckCatalogResult = "D";
                    }
                    else
                        Category.CheckCatalogResult = "D";
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HGLME27");
            }

            return ReturnVal;
        }

        public static string HGLME28(cCategory Category, ref bool Log)
        //Check for Valid Run Sequence
        {
            string ReturnVal = "";

            try
            {
                DataRowView HGLMEDefaultRun = Category.GetCheckParameter("Current_Hg_LME_Default_Run").ValueAsDataRowView();
                int DefRunCt = Category.GetCheckParameter("Hg_LME_Default_Run_Count").ValueAsInt() + 1;
                Category.SetCheckParameter("Hg_LME_Default_Run_Count", DefRunCt, eParameterDataType.Integer);
                int RunNum = cDBConvert.ToInteger(HGLMEDefaultRun["RUN_NUM"]);
                if (RunNum > 0 && RunNum != DefRunCt)
                    Category.SetCheckParameter("Hg_LME_Default_Runs_Sequential", false, eParameterDataType.Boolean);
                else
                    if (Category.GetCheckParameter("Hg_LME_Default_Run_Dates_Consistent").ValueAsBool())
                    {
                        DateTime BeginDate = cDBConvert.ToDate(HGLMEDefaultRun["BEGIN_DATE"], DateTypes.START);
                        int BeginHour = cDBConvert.ToHour(HGLMEDefaultRun["BEGIN_HOUR"], DateTypes.START);
                        int BeginMin = cDBConvert.ToInteger(HGLMEDefaultRun["BEGIN_MIN"]);
                        DateTime LastEndDate = Category.GetCheckParameter("Hg_LME_Default_Last_End_Date").ValueAsDateTime(DateTypes.END);
                        int LastEndHour = Category.GetCheckParameter("Hg_LME_Default_Last_End_Hour").ValueAsInt();
                        int LastEndMin = Category.GetCheckParameter("Hg_LME_Default_Last_End_Minute").ValueAsInt();
                        if (LastEndDate != DateTime.MinValue && (BeginDate < LastEndDate || (BeginDate == LastEndDate && (BeginHour < LastEndHour || (BeginHour == LastEndHour && BeginMin < LastEndMin)))))
                            Category.SetCheckParameter("Hg_LME_Default_Runs_Sequential", false, eParameterDataType.Boolean);
                        else
                        {
                            Category.SetCheckParameter("Hg_LME_Default_Last_End_Date", cDBConvert.ToDate(HGLMEDefaultRun["END_DATE"], DateTypes.END), eParameterDataType.Date);
                            Category.SetCheckParameter("Hg_LME_Default_Last_End_Hour", cDBConvert.ToHour(HGLMEDefaultRun["END_HOUR"], DateTypes.END), eParameterDataType.Integer);
                            Category.SetCheckParameter("Hg_LME_Default_Last_End_Minute", cDBConvert.ToInteger(HGLMEDefaultRun["END_MIN"]), eParameterDataType.Integer);
                        }
                    }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HGLME28");
            }

            return ReturnVal;
        }

        public static string HGLME29(cCategory Category, ref bool Log)
        //Hg LME Default Test Runs Missing or Out of Sequence
        {
            string ReturnVal = "";

            try
            {
                if (!Category.GetCheckParameter("Hg_LME_Default_Runs_Sequential").ValueAsBool())
                    Category.CheckCatalogResult = "A";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HGLME29");
            }

            return ReturnVal;
        }

        public static string HGLME30(cCategory Category, ref bool Log)
        //Insufficient Number of Runs
        {
            string ReturnVal = "";

            try
            {
                if (Category.GetCheckParameter("Hg_LME_Default_Run_Count").ValueAsInt() < 3)
                {
                    Category.SetCheckParameter("Calculate_Hg_LME_Max_Run_Value", false, eParameterDataType.Boolean);
                    Category.CheckCatalogResult = "A";
                }
                else
                    if (Category.GetCheckParameter("Hg_LME_Default_Run_Count").ValueAsInt() <= 0)
                        Category.SetCheckParameter("Calculate_Hg_LME_Max_Run_Value", false, eParameterDataType.Boolean);
                    else
                        Category.SetCheckParameter("Calculate_Hg_LME_Max_Run_Value", true, eParameterDataType.Boolean);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HGLME30");
            }

            return ReturnVal;
        }

        public static string HGLME31(cCategory Category, ref bool Log)
        //Max Run Value Consistent with Reported Value
        {
            string ReturnVal = "";

            try
            {
                if (Category.GetCheckParameter("Calculate_Hg_LME_Max_Run_Value").ValueAsBool())
                {
                    decimal CalcMaxRunVal = Category.GetCheckParameter("Hg_LME_Default_Calc_Max_Run_Value").ValueAsDecimal();
                    decimal CalcMaxConc = Category.GetCheckParameter("Hg_LME_Default_Calc_Maximum_Concentration").ValueAsDecimal();
                    DataRowView HGLMEDefaultLevel = Category.GetCheckParameter("Current_Hg_LME_Default_Test_Level").ValueAsDataRowView();
                    decimal MaxRunVal = cDBConvert.ToDecimal(HGLMEDefaultLevel["MAX_RUN_VALUE"]);

                    if (CalcMaxConc != -1)
                    {
                        if (CalcMaxConc < CalcMaxRunVal)
                            Category.SetCheckParameter("Hg_LME_Default_Calc_Maximum_Concentration", CalcMaxRunVal, eParameterDataType.Decimal);
                    }
                    if (MaxRunVal > 0 && CalcMaxRunVal != MaxRunVal)
                        Category.CheckCatalogResult = "A";
                }
                else
                    Category.SetCheckParameter("Hg_LME_Default_Calc_Maximum_Concentration", -1m, eParameterDataType.Decimal);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HGLME31");
            }

            return ReturnVal;
        }

        public static string HGLME32(cCategory Category, ref bool Log)
        //Hg LME Default Test Consistent with Method
        {
            string ReturnVal = "";

            try
            {
                if (Category.GetCheckParameter("Test_End_Date_Valid").ValueAsBool() && Category.GetCheckParameter("Test_End_Hour_Valid").ValueAsBool())
                {
                    DataRowView CurrentHGLMETest = Category.GetCheckParameter("Current_Hg_LME_Default_Test").ValueAsDataRowView();
                    DateTime EndDate = cDBConvert.ToDate(CurrentHGLMETest["END_DATE"], DateTypes.END);
                    int EndHour = cDBConvert.ToHour(CurrentHGLMETest["END_HOUR"], DateTypes.END);
                    Category.SetCheckParameter("Hg_LME_Default_Effective_Date", EndDate, eParameterDataType.Date);
                    DataView MethodRecords = Category.GetCheckParameter("Method_Records").ValueAsDataView();
                    sFilterPair[] Filter = new sFilterPair[2];
                    Filter[0].Set("PARAMETER_CD", "HGM");
                    Filter[1].Set("METHOD_CD", "LME");
                    DataView MethRecsFound = FindRows(MethodRecords, Filter);
                    MethRecsFound.Sort = "END_DATE, END_HOUR";
                    DataRowView MethRec = null;
                    DateTime MethEndDate;
                    int MethEndHour;
                    foreach (DataRowView drv in MethRecsFound)
                    {
                        MethEndDate = cDBConvert.ToDate(drv["END_DATE"], DateTypes.END);
                        MethEndHour = cDBConvert.ToHour(drv["END_HOUR"], DateTypes.END);
                        if (MethEndDate == DateTime.MaxValue || (MethEndDate > EndDate || (MethEndDate == EndDate && MethEndHour > EndHour)))
                        {
                            MethRec = drv;
                            break;
                        }
                    }
                    if (MethRec == null)
                        Category.CheckCatalogResult = "A";
                    else
                    {
                        DateTime MethRecBeginDate = cDBConvert.ToDate(MethRec["BEGIN_DATE"], DateTypes.START);
                        if (MethRecBeginDate > EndDate)
                            Category.SetCheckParameter("Hg_LME_Default_Effective_Date", MethRecBeginDate, eParameterDataType.Date);
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HGLME32");
            }

            return ReturnVal;
        }

        public static string HGLME33(cCategory Category, ref bool Log)
        //Common Stack Test Code Valid
        {
            string ReturnVal = "";

            try
            {
                decimal CalcMaxConc = Category.GetCheckParameter("Hg_LME_Default_Calc_Maximum_Concentration").ValueAsDecimal();
                bool CalcDefault = Category.GetCheckParameter("Calculate_Hg_LME_Default").ValueAsBool();
                DataRowView CurrentHGLMETest = Category.GetCheckParameter("Current_Hg_LME_Default_Test").ValueAsDataRowView();
                string CSTestCd = cDBConvert.ToString(CurrentHGLMETest["COMMON_STACK_TEST_CD"]);
                if (CalcMaxConc <= 0)
                    CalcDefault = false;
                else
                    CalcDefault = true;
                if (cDBConvert.ToString(CurrentHGLMETest["LOCATION_IDENTIFIER"]).StartsWith("CS"))
                {
                    if (CurrentHGLMETest["COMMON_STACK_TEST_CD"] == DBNull.Value)
                    {
                        CalcDefault = false;
                        Category.CheckCatalogResult = "A";
                    }
                    else
                    {
                        DataView CSTestCdLookup = Category.GetCheckParameter("Common_Stack_Test_Code_Lookup_Table").ValueAsDataView();
                        sFilterPair[] Filter = new sFilterPair[1];
                        Filter[0].Set("COMMON_STACK_TEST_CD", CSTestCd);
                        if (CountRows(CSTestCdLookup, Filter) == 0)
                        {
                            CalcDefault = false;
                            Category.CheckCatalogResult = "B";
                        }
                        else
                            if (cDBConvert.ToInteger(CurrentHGLMETest["CS_UNIT_TEST_IND"]) == 1)
                                if (CSTestCd.InList("AU,IU"))
                                {
                                    DateTime EndDate = cDBConvert.ToDate(CurrentHGLMETest["END_DATE"], DateTypes.END);
                                    DataView UnitStackConfigRecs = (DataView)Category.GetCheckParameter("Unit_Stack_Configuration_Records").ParameterValue;
                                    sFilterPair[] FilterUSConfigRecs = new sFilterPair[3];

                                    FilterUSConfigRecs[0].Set("BEGIN_DATE", EndDate, eFilterDataType.DateEnded, eFilterPairRelativeCompare.LessThanOrEqual);
                                    FilterUSConfigRecs[1].Set("END_DATE", EndDate, eFilterDataType.DateEnded, eFilterPairRelativeCompare.GreaterThanOrEqual);
                                    FilterUSConfigRecs[2].Set("STACK_PIPE_MON_LOC_ID", cDBConvert.ToString(CurrentHGLMETest["MON_LOC_ID"]));
                                    DataView UnitStackConfigRecsFound = FindRows(UnitStackConfigRecs, FilterUSConfigRecs);
                                    if (UnitStackConfigRecsFound.Count == 0)
                                    {
                                        FilterUSConfigRecs = new sFilterPair[2];
                                        FilterUSConfigRecs[0].Set("END_DATE", EndDate, eFilterDataType.DateEnded, eFilterPairRelativeCompare.GreaterThanOrEqual);
                                        FilterUSConfigRecs[1].Set("STACK_PIPE_MON_LOC_ID", cDBConvert.ToString(CurrentHGLMETest["MON_LOC_ID"]));
                                        UnitStackConfigRecsFound = FindRows(UnitStackConfigRecs, FilterUSConfigRecs);
                                    }

                                    int tempUnitCt = 0;
                                    int tempTestCt = 0;
                                    string UnitsTested = Category.GetCheckParameter("Hg_LME_Default_Units_Tested").ValueAsString();
                                    foreach (DataRowView drv in UnitStackConfigRecsFound)
                                    {
                                        tempUnitCt++;                                        
                                        if (cDBConvert.ToString(drv["UNITID"]).InList(UnitsTested))
                                            tempTestCt++;
                                    }
                                    if (CSTestCd == "AU")
                                    {
                                        if (tempUnitCt != tempTestCt)
                                        {
                                            CalcDefault = false;
                                            Category.CheckCatalogResult = "C";
                                        }
                                    }
                                    else
                                    {
                                        if (tempUnitCt < 4)
                                        {
                                            if (tempTestCt < 1)
                                            {
                                                CalcDefault = false;
                                                Category.CheckCatalogResult = "D";
                                            }
                                        }
                                        if (String.IsNullOrEmpty(Category.CheckCatalogResult) && tempUnitCt < 7)
                                        {
                                            if (tempTestCt < 2)
                                            {
                                                CalcDefault = false;
                                                Category.CheckCatalogResult = "D";
                                            }
                                        }
                                        if (String.IsNullOrEmpty(Category.CheckCatalogResult) && tempUnitCt < 11)
                                        {
                                            if (tempTestCt < 3)
                                            {
                                                CalcDefault = false;
                                                Category.CheckCatalogResult = "D";
                                            }
                                        }
                                        else
                                        {
                                            decimal tempVal = Math.Round((tempUnitCt / 3m), MidpointRounding.AwayFromZero);
                                            if (tempTestCt < tempVal)
                                            {
                                                CalcDefault = false;
                                                Category.CheckCatalogResult = "D";
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    CalcDefault = false;
                                    Category.CheckCatalogResult = "B";
                                }
                            else
                                if (CSTestCd == "IU")
                                {
                                    CalcDefault = false;
                                    Category.CheckCatalogResult = "B";
                                }
                                else
                                    if (CSTestCd.InList("NAUS,NAUD"))
                                        if (cDBConvert.ToString(CurrentHGLMETest["TEST_REASON_CD"]) == "INITIAL")
                                        {
                                            CalcDefault = false;
                                            Category.CheckCatalogResult = "E";
                                        }
                                        else
                                        {
                                            DateTime EndDate = cDBConvert.ToDate(CurrentHGLMETest["END_DATE"], DateTypes.END);
                                            DataView QASuppRecs = (DataView)Category.GetCheckParameter("QA_Supplemental_Data_Records").ParameterValue;
                                            sFilterPair[] FilterSuppRecs = new sFilterPair[2];
                                            FilterSuppRecs[0].Set("TEST_TYPE_CD", "HGLME");
                                            FilterSuppRecs[1].Set("END_DATE", EndDate, eFilterDataType.DateEnded, eFilterPairRelativeCompare.LessThan);
                                            if (CountRows(QASuppRecs, FilterSuppRecs) == 0)
                                            {
                                                CalcDefault = false;
                                                Category.CheckCatalogResult = "F";
                                            }
                                        }
                    }
                }
                else
                    if (CurrentHGLMETest["COMMON_STACK_TEST_CD"] != DBNull.Value)
                        Category.CheckCatalogResult = "G";
                Category.SetCheckParameter("Calculate_Hg_LME_Default", CalcDefault, eParameterDataType.Boolean);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HGLME33");
            }

            return ReturnVal;
        }

        public static string HGLME34(cCategory Category, ref bool Log)
        //Determine Default Hg Concentration
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("Hg_LME_Default_Calc_Default_Value", null, eParameterDataType.Decimal);
                decimal DefVal = decimal.MinValue;
                DataRowView CurrentHGLMETest = Category.GetCheckParameter("Current_Hg_LME_Default_Test").ValueAsDataRowView();
                string GroupID = cDBConvert.ToString(CurrentHGLMETest["GROUP_ID"]);
                decimal DefHGConc = cDBConvert.ToDecimal(CurrentHGLMETest["DEFAULT_HG_CONCENTRATION"]);
                decimal CalcMaxConc = Category.GetCheckParameter("Hg_LME_Default_Calc_Maximum_Concentration").ValueAsDecimal();
                bool CalcDefault = Category.GetCheckParameter("Calculate_Hg_LME_Default").ValueAsBool();
                DateTime EndDate = cDBConvert.ToDate(CurrentHGLMETest["END_DATE"], DateTypes.END);

                if (GroupID != "")
                {
                    if (DefHGConc == decimal.MinValue)
                        Category.CheckCatalogResult = "A";
                    else
                        if (DefHGConc <= 0.5m)
                            Category.CheckCatalogResult = "B";
                        else
                            if (CalcDefault)
                            {
                                if (DefHGConc < CalcMaxConc)
                                    Category.CheckCatalogResult = "C";
                                else
                                {
                                    DefVal = DefHGConc;
                                    Category.SetCheckParameter("Hg_LME_Default_Calc_Default_Value", DefVal, eParameterDataType.Decimal);
                                    if (Category.GetCheckParameter("Test_End_Date_Valid").ValueAsBool() &&
                                        Category.GetCheckParameter("Test_End_Hour_Valid").ValueAsBool())
                                    {
                                        int EndHour = cDBConvert.ToHour(CurrentHGLMETest["END_HOUR"], DateTypes.END);
                                        DataView DefRecs = (DataView)Category.GetCheckParameter("Default_Records").ParameterValue;
                                        sFilterPair[] FilterDef = new sFilterPair[4];
                                        FilterDef[0].Set("DEFAULT_PURPOSE_CD", "LM");
                                        FilterDef[1].Set("PARAMETER_CD", "HGC");
                                        FilterDef[2].Set("DEFAULT_UOM_CD", "UGSCM");
                                        FilterDef[3].Set("GROUP_ID", GroupID);
                                        DataView DefRecsFound = FindActiveRows(DefRecs, EndDate, EndHour, DateTime.MaxValue, int.MaxValue, true, FilterDef);
                                        if (DefRecsFound.Count == 0)
                                            Category.CheckCatalogResult = "D";
                                        else
                                            if (cDBConvert.ToDecimal(DefRecsFound[0]["DEFAULT_VALUE"]) < DefVal)
                                                Category.CheckCatalogResult = "E";
                                    }
                                }
                            }
                }
                else
                {
                    if (CalcDefault)
                        if (cDBConvert.ToString(CurrentHGLMETest["COMMON_STACK_TEST_CD"]) == "NAUD")
                        {
                            DataView QASuppRecs = Category.GetCheckParameter("QA_Supplemental_Data_Records").ValueAsDataView();
                            sFilterPair[] FilterQASupp = new sFilterPair[2];
                            FilterQASupp[0].Set("END_DATE", EndDate, eFilterDataType.DateEnded, eFilterPairRelativeCompare.LessThan);
                            FilterQASupp[1].Set("TEST_TYPE_CD", "HGLME");
                            DataView QASuppRecsFound = FindRows(QASuppRecs, FilterQASupp);
                            if (QASuppRecsFound.Count == 0)
                                Category.CheckCatalogResult = "F";
                            else
                            {
                                QASuppRecsFound.Sort = "BEGIN_DATE DESC, BEGIN_HOUR DESC, BEGIN_MIN DESC";
                                DataView QASuppAttRecs = (DataView)Category.GetCheckParameter("QA_Supplemental_Attribute_Records").ParameterValue;
                                FilterQASupp[0].Set("QA_SUPP_DATA_ID", cDBConvert.ToString(QASuppRecsFound[0]["QA_SUPP_DATA_ID"]));
                                FilterQASupp[1].Set("ATTRIBUTE_NAME", "DEFAULT_HG_CONCENTRATION");
                                DataView QASuppAttRecsFound = FindRows(QASuppAttRecs, FilterQASupp);
                                if (QASuppAttRecsFound.Count == 0)
                                    Category.CheckCatalogResult = "F";
                                else
                                {
                                    decimal QASuppAttDefVal = cDBConvert.ToDecimal(QASuppAttRecsFound[0]["ATTRIBUTE_VALUE"]);
                                    if (QASuppAttDefVal > CalcMaxConc)
                                        DefVal = QASuppAttDefVal;
                                    else
                                        DefVal = CalcMaxConc;
                                }
                            }
                        }
                        else
                            if (CalcMaxConc < 0.5m)
                                DefVal = 0.5m;
                            else
                                DefVal = CalcMaxConc;
                    if (DefVal != decimal.MinValue)//now set the parameter if there is a value
                        Category.SetCheckParameter("Hg_LME_Default_Calc_Default_Value", DefVal, eParameterDataType.Decimal);
                    if (DefHGConc == decimal.MinValue)
                        Category.CheckCatalogResult = "A";
                    else
                        if (DefHGConc <= 0.5m)
                            Category.CheckCatalogResult = "B";
                        else
                            if (DefVal != decimal.MinValue && DefVal != DefHGConc)
                                Category.CheckCatalogResult = "G";
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HGLME34");
            }

            return ReturnVal;
        }

        public static string HGLME35(cCategory Category, ref bool Log)
        //Determine Hg LME Max Potential Hg Mass
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("Hg_LME_Default_Calc_Max_Potential_Hg_Mass", null, eParameterDataType.Decimal);
                decimal CalcDefVal = Category.GetCheckParameter("Hg_LME_Default_Calc_Default_Value").ValueAsDecimal();
                if (CalcDefVal == decimal.MinValue)
                    Category.CheckCatalogResult = "A";
                else
                {
                    DataRowView CurrentHGLMETest = Category.GetCheckParameter("Current_Hg_LME_Default_Test").ValueAsDataRowView();
                    int MaxOpHrs = cDBConvert.ToInteger(CurrentHGLMETest["MAX_OPERATING_HOURS"]);
                    if (Category.GetCheckParameter("Test_End_Date_Valid").ValueAsBool() && Category.GetCheckParameter("Test_End_Hour_Valid").ValueAsBool() &&
                        (0 < MaxOpHrs || MaxOpHrs <= 8760))
                    {
                        DateTime EndDate = cDBConvert.ToDate(CurrentHGLMETest["END_DATE"], DateTypes.END);
                        int EndHour = cDBConvert.ToHour(CurrentHGLMETest["END_HOUR"], DateTypes.END);
                        DateTime BeginDate = cDBConvert.ToDate(CurrentHGLMETest["BEGIN_DATE"], DateTypes.START);
                        int BeginHour = cDBConvert.ToHour(CurrentHGLMETest["BEGIN_HOUR"], DateTypes.START);
                        sFilterPair[] Filter = new sFilterPair[1];
                        Filter[0].Set("COMPONENT_TYPE_CD", "FLOW");

                        DataView SpanRecs = (DataView)Category.GetCheckParameter("Span_Records").ParameterValue;
                        DataView SpanRecsFound = FindActiveRows(SpanRecs, BeginDate, BeginHour, EndDate, EndHour, Filter);
                        decimal MPFVal = SpanRecsFound.Count == 0 ? decimal.MinValue : cDBConvert.ToDecimal(SpanRecsFound[0]["MPF_VALUE"]);
                        if (SpanRecsFound.Count > 0 && MPFVal > 0)
                        {
                            decimal MaxPotMass = Math.Round(MaxOpHrs * 0.0000000009978m * MPFVal * CalcDefVal, MidpointRounding.AwayFromZero);
                            decimal RecordMaxPotMass = cDBConvert.ToDecimal(CurrentHGLMETest["MAX_POTENTIAL_HG_MASS"]);
                            Category.SetCheckParameter("Hg_LME_Default_Calc_Max_Potential_Hg_Mass", MaxPotMass, eParameterDataType.Decimal);
                            if (RecordMaxPotMass > 0 && RecordMaxPotMass != MaxPotMass)
                                Category.CheckCatalogResult = "B";
                        }
                        else
                            Category.CheckCatalogResult = "A";
                    }
                    else
                        Category.CheckCatalogResult = "A";
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HGLME35");
            }

            return ReturnVal;
        }

        public static string HGLME36(cCategory Category, ref bool Log)
        //Determine Test Frequency 
        {
            string ReturnVal = "";

            try
            {
                string DefCalcFreq = "";
                decimal CalcMaxPotHGMass = Category.GetCheckParameter("Hg_LME_Default_Calc_Max_Potential_Hg_Mass").ValueAsDecimal();
                DateTime EffDate = Category.GetCheckParameter("Hg_LME_Default_Effective_Date").ValueAsDateTime(DateTypes.END);
                DataRowView CurrentHGLMETest = Category.GetCheckParameter("Current_Hg_LME_Default_Test").ValueAsDataRowView();
                DateTime EndDate = cDBConvert.ToDate(CurrentHGLMETest["END_DATE"], DateTypes.END);
                string MonLocId = cDBConvert.ToString(CurrentHGLMETest["MON_LOC_ID"]);
                string TestFreqCd = cDBConvert.ToString(CurrentHGLMETest["TEST_FREQUENCY_CD"]);
                if (CalcMaxPotHGMass != decimal.MinValue && EffDate != DateTime.MaxValue)
                {
                    int MonPlanBegRptYear;
                    int MonPlanBegRptMonth;
                    int MonPlanEndRptYear;
                    int MonPlanEndRptMonth;
                    DateTime MonPlanBeginRptPerFirstDate;                    
                    DataView MonPlanRecs = Category.GetCheckParameter("Facility_Monitor_Plan_Location_Records").ValueAsDataView();
                    DataRowView MonPlanRecFound = null;
                    foreach (DataRowView drv1 in MonPlanRecs)
                    {
                        cDateFunctions.GetYearAndQuarter(cDBConvert.ToInteger(drv1["BEGIN_RPT_PERIOD_ID"]), out MonPlanBegRptYear, out MonPlanBegRptMonth);
                        cDateFunctions.GetYearAndQuarter(cDBConvert.ToInteger(drv1["END_RPT_PERIOD_ID"]), out MonPlanEndRptYear, out MonPlanEndRptMonth);

                        MonPlanBeginRptPerFirstDate = cDateFunctions.StartDateThisQuarter(cDBConvert.ToInteger(drv1["BEGIN_RPT_PERIOD_ID"]));                        
                        if (cDBConvert.ToString(drv1["MON_LOC_ID"]) == MonLocId && MonPlanBeginRptPerFirstDate < EffDate &&
                            (drv1["END_RPT_PERIOD_ID"] == DBNull.Value || cDateFunctions.LastDateThisQuarter(MonPlanEndRptYear, MonPlanEndRptMonth) > EffDate))
                        {
                            MonPlanRecFound = drv1;
                            break;
                        }
                    }
                    if (MonPlanRecFound != null)
                    {
                        decimal TotalHg = CalcMaxPotHGMass;
                        int UnitCt;
                        string LocID = cDBConvert.ToString(CurrentHGLMETest["LOCATION_IDENTIFIER"]);
                        if (!LocID.StartsWith("MS") && !LocID.StartsWith("CS"))
                            UnitCt = 1;
                        else
                            UnitCt = 0;

                        string FoundMonPlanId = cDBConvert.ToString(MonPlanRecFound["MON_PLAN_ID"]);
                        DataView QASuppRecs = Category.GetCheckParameter("QA_Supplemental_Data_Records").ValueAsDataView();
                        DataView QASuppRecsFound;
                        sFilterPair[] FilterQASupp;
                        DataView MethRecs = Category.GetCheckParameter("Facility_Method_Records").ValueAsDataView();
                        DataView MethRecsFound;
                        DataRowView MethRec = null;
                        sFilterPair[] FilterMeth = new sFilterPair[3];

                        string thisLocID;
                        foreach (DataRowView drv2 in MonPlanRecs)
                        {
                            if (cDBConvert.ToString(drv2["MON_PLAN_ID"]) == FoundMonPlanId && cDBConvert.ToString(drv2["MON_LOC_ID"]) != MonLocId)
                            {
                                thisLocID = cDBConvert.ToString(drv2["STACK_NAME"]);
                                if (!thisLocID.StartsWith("MP") && !thisLocID.StartsWith("CP"))
                                {
                                    if (!thisLocID.StartsWith("MS") && !thisLocID.StartsWith("CS"))
                                        UnitCt++;
                                    FilterMeth[0].Set("PARAMETER_CD", "HGM");
                                    FilterMeth[1].Set("METHOD_CD", "LME");
                                    FilterMeth[2].Set("BEGIN_DATE", EffDate, eFilterDataType.DateBegan, eFilterPairRelativeCompare.LessThanOrEqual);
                                    MethRecsFound = FindRows(MethRecs, FilterMeth);
                                    foreach (DataRowView drv3 in MethRecsFound)
                                        if (drv3["END_DATE"] == DBNull.Value || cDBConvert.ToDate(drv3["END_DATE"], DateTypes.END) > EffDate)
                                        {
                                            MethRec = drv3;
                                            break;
                                        }
                                    if (MethRec != null)
                                    {
                                        FilterQASupp = new sFilterPair[2];
                                        FilterQASupp[0].Set("TEST_TYPE_CD", "HGLME");
                                        FilterQASupp[1].Set("MON_LOC_ID", cDBConvert.ToString(MethRec["MON_LOC_ID"]));
                                        QASuppRecsFound = FindActiveRows(QASuppRecs, EndDate.AddDays(-30), EndDate.AddDays(30), "END_DATE", "END_DATE", true, FilterQASupp);
                                        if (QASuppRecsFound.Count > 0 && cDBConvert.ToString(QASuppRecsFound[0]["CAN_SUBMIT"]) == "N")
                                        {
                                            DataView QASuppAttRecs = (DataView)Category.GetCheckParameter("QA_Supplemental_Attribute_Records").ParameterValue;
                                            FilterQASupp[0].Set("QA_SUPP_DATA_ID", cDBConvert.ToString(QASuppRecsFound[0]["QA_SUPP_DATA_ID"]));
                                            FilterQASupp[1].Set("ATTRIBUTE_NAME", "MAX_POTENTIAL_HG_MASS");
                                            DataView QASuppAttRecsFound = FindRows(QASuppAttRecs, FilterQASupp);
                                            if (QASuppAttRecsFound.Count > 0)
                                                TotalHg += cDBConvert.ToDecimal(QASuppAttRecsFound[0]["ATTRIBUTE_VALUE"]);
                                            else
                                                Category.CheckCatalogResult = "A";
                                        }
                                        else
                                        {
                                            DataView HGLMETests = Category.GetCheckParameter("Facility_Hg_LME_Default_Tests").ValueAsDataView();
                                            sFilterPair[] FilterHGLMETests = new sFilterPair[1];                                            
                                            FilterHGLMETests[0].Set("MON_LOC_ID", cDBConvert.ToString(MethRec["MON_LOC_ID"]));
                                            DataView HGLMETestsFound = FindActiveRows(HGLMETests, EndDate.AddDays(-30), EndDate.AddDays(30), "END_DATE", "END_DATE", true, FilterHGLMETests);
                                            if (HGLMETestsFound.Count > 0)
                                            {
                                                decimal MaxPotMass = cDBConvert.ToDecimal(HGLMETestsFound[0]["MAX_POTENTIAL_HG_MASS"]);
                                                if (MaxPotMass > 0)
                                                    TotalHg += MaxPotMass;
                                                else
                                                    Category.CheckCatalogResult = "A";
                                            }
                                            else
                                            {
                                                if (TestFreqCd == "")
                                                    Category.CheckCatalogResult = "B";
                                                else
                                                    Category.CheckCatalogResult = "C";
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (TotalHg > UnitCt * 464)
                        {
                            if (cDBConvert.ToString(CurrentHGLMETest["TEST_REASON_CD"]) == "INITIAL")
                                Category.CheckCatalogResult = "D";
                            else
                            {
                                FilterQASupp = new sFilterPair[3];
                                FilterQASupp[0].Set("END_DATE", cDBConvert.ToDate(CurrentHGLMETest["END_DATE"], DateTypes.END), eFilterDataType.DateEnded, eFilterPairRelativeCompare.LessThan);
                                FilterQASupp[1].Set("TEST_TYPE_CD", "HGLME");
                                FilterQASupp[2].Set("MON_LOC_ID", MonLocId);
                                if (CountRows(QASuppRecs, FilterQASupp) == 0)
                                    Category.CheckCatalogResult = "E";
                            }
                        }
                        if (TotalHg > UnitCt * 144)
                            DefCalcFreq = "2QTRS";
                        else
                            DefCalcFreq = "4QTRS";

                        if (TestFreqCd == "4QTRS")
                        {
                            if (DefCalcFreq == "2QTRS")
                                Category.CheckCatalogResult = "F";
                        }
                        else
                            if (TestFreqCd == "2QTRS")
                            {
                                if (DefCalcFreq == "4QTRS")
                                    Category.CheckCatalogResult = "G";
                            }
                            else
                                Category.CheckCatalogResult = "H";
                    }
                }
                if (DefCalcFreq != "")
                    Category.SetCheckParameter("Hg_LME_Default_Calc_Test_Frequency", DefCalcFreq, eParameterDataType.String);
                else
                    Category.SetCheckParameter("Hg_LME_Default_Calc_Test_Frequency", null, eParameterDataType.String);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HGLME36");
            }

            return ReturnVal;
        }

        public static string HGLME37(cCategory Category, ref bool Log)
        //Hg LME Default Hg Concentration Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentHGLMETest = Category.GetCheckParameter("Current_Hg_LME_Default_Test").ValueAsDataRowView();
                decimal HgConc = cDBConvert.ToDecimal(CurrentHGLMETest["DEFAULT_HG_CONCENTRATION"]);
                if (HgConc == decimal.MinValue)
                    Category.CheckCatalogResult = "A";
                else
                    if (HgConc < 0.5m)
                        Category.CheckCatalogResult = "B";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HGLME37");
            }

            return ReturnVal;
        }

        public static string HGLME38(cCategory Category, ref bool Log)
        //Duplicate Hg LME Default Test
        {
            string ReturnVal = "";

            try
            {
              if (Category.GetCheckParameter("Test_Number_Valid").ValueAsBool())
              {
                DataRowView CurrentHGLMETest = Category.GetCheckParameter("Current_Hg_LME_Default_Test").ValueAsDataRowView();
                string TestNum = cDBConvert.ToString(CurrentHGLMETest["TEST_NUM"]);
                DataView LocTestRecs = Category.GetCheckParameter("Location_Test_Records").ValueAsDataView();
                sFilterPair[] TestRecsFilter = new sFilterPair[2];
                TestRecsFilter[0].Set("TEST_NUM", TestNum);
                TestRecsFilter[1].Set("TEST_TYPE_CD", "HGLME");
                DataView LocTestRecsFound = FindRows(LocTestRecs, TestRecsFilter);
                if ((LocTestRecsFound.Count > 0 && CurrentHGLMETest["TEST_SUM_ID"] == DBNull.Value) ||
                    (LocTestRecsFound.Count > 1 && CurrentHGLMETest["TEST_SUM_ID"] != DBNull.Value) ||
                    (LocTestRecsFound.Count == 1 && CurrentHGLMETest["TEST_SUM_ID"] != DBNull.Value && CurrentHGLMETest["TEST_SUM_ID"].ToString() != LocTestRecsFound[0]["TEST_SUM_ID"].ToString()))
                  Category.CheckCatalogResult = "A";
                else
                {
                  string TestSumID = cDBConvert.ToString(CurrentHGLMETest["TEST_SUM_ID"]);
                  if (TestSumID != "")
                  {
                    DataView QASuppRecords = (DataView)Category.GetCheckParameter("QA_Supplemental_Data_Records").ParameterValue;
                    string OldFilter2 = QASuppRecords.RowFilter;
                    QASuppRecords.RowFilter = AddToDataViewFilter(OldFilter2, "TEST_NUM = '" + cDBConvert.ToString(CurrentHGLMETest["TEST_NUM"]) + "' AND TEST_TYPE_CD = 'HGLME'");
                    if (QASuppRecords.Count > 0 && cDBConvert.ToString(QASuppRecords[0]["TEST_SUM_ID"]) != TestSumID)
                    {
                      Category.CheckCatalogResult = "B";
                    }
                    QASuppRecords.RowFilter = OldFilter2;
                  }
                }
              }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HGLME38");
            }

            return ReturnVal;
        }

        public static string HGLME39(cCategory Category, ref bool Log)
        //Reported Operating Level Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView HGLMEDefaultLevel = Category.GetCheckParameter("Current_Hg_LME_Default_Test_Level").ValueAsDataRowView();
                string OpLvlCd = cDBConvert.ToString(HGLMEDefaultLevel["HG_OP_LEVEL_CD"]);
                if (OpLvlCd == "")
                    Category.CheckCatalogResult = "A";
                else
                {
                    DataView OpLvlLookup = Category.GetCheckParameter("Hg_LME_Operating_Level_Code_Lookup_Table").ValueAsDataView();
                    sFilterPair[] Filter = new sFilterPair[1];
                    Filter[0].Set("OP_LEVEL_CD", OpLvlCd);
                    if (CountRows(OpLvlLookup, Filter) == 0)
                        Category.CheckCatalogResult = "B";
                    else
                        if (cDBConvert.ToInteger(Category.GetCheckParameter("Current_Hg_LME_Default_Test").ValueAsDataRowView()["CS_UNIT_TEST_IND"]) == 1)
                        {
                            if (OpLvlCd != "T")
                                Category.CheckCatalogResult = "C";
                        }
                        else
                            if (OpLvlCd == "T")
                                Category.CheckCatalogResult = "B";
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HGLME39");
            }

            return ReturnVal;
        }

        public static string HGLME40(cCategory Category, ref bool Log)
        //Duplicate Hg LME Default Test Level
        {
            string ReturnVal = "";

            try
            {
                DataRowView HGLMEDefaultLevel = Category.GetCheckParameter("Current_Hg_LME_Default_Test_Level").ValueAsDataRowView();
                string TestLocID = cDBConvert.ToString(HGLMEDefaultLevel["TEST_LOCATION_ID"]);
                if (TestLocID != "")
                {
                    DataView HGLMELevelRecs = Category.GetCheckParameter("Hg_LME_Default_Test_Level_Records").ValueAsDataView();
                    sFilterPair[] LevelRecsFilter = new sFilterPair[1];
                    LevelRecsFilter[0].Set("TEST_LOCATION_ID", TestLocID);
                    DataView LevelRecsFound = FindRows(HGLMELevelRecs, LevelRecsFilter);
                    if ((LevelRecsFound.Count > 0 && HGLMEDefaultLevel["HG_LME_DEFAULT_TEST_DATA_ID"] == DBNull.Value) ||
                        (LevelRecsFound.Count > 1 && HGLMEDefaultLevel["HG_LME_DEFAULT_TEST_DATA_ID"] != DBNull.Value) ||
                        (LevelRecsFound.Count == 1 && HGLMEDefaultLevel["HG_LME_DEFAULT_TEST_DATA_ID"] != DBNull.Value && HGLMEDefaultLevel["HG_LME_DEFAULT_TEST_DATA_ID"].ToString() != LevelRecsFound[0]["HG_LME_DEFAULT_TEST_DATA_ID"].ToString()))
                        Category.CheckCatalogResult = "A";
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HGLME40");
            }

            return ReturnVal;
        }

        public static string HGLME41(cCategory Category, ref bool Log)
        //Duplicate Hg LME Default Test Run
        {
            string ReturnVal = "";

            try
            {
                DataRowView HGLMEDefaultRun = Category.GetCheckParameter("Current_Hg_LME_Default_Run").ValueAsDataRowView();
                int RunNum = cDBConvert.ToInteger(HGLMEDefaultRun["RUN_NUM"]);
                if (RunNum != int.MinValue)
                {
                    string TestLocID = cDBConvert.ToString(HGLMEDefaultRun["TEST_LOCATION_ID"]);
                    DataView HGLMERunRecs = Category.GetCheckParameter("Hg_LME_Default_Run_Records").ValueAsDataView();
                    sFilterPair[] RunRecsFilter = new sFilterPair[2];
                    RunRecsFilter[0].Set("TEST_LOCATION_ID", TestLocID);
                    RunRecsFilter[1].Set("RUN_NUM", RunNum, eFilterDataType.Integer);
                    DataView RunRecsFound = FindRows(HGLMERunRecs, RunRecsFilter);
                    if ((RunRecsFound.Count > 0 && HGLMEDefaultRun["HG_LME_DEFAULT_TEST_RUN_ID"] == DBNull.Value) ||
                        (RunRecsFound.Count > 1 && HGLMEDefaultRun["HG_LME_DEFAULT_TEST_RUN_ID"] != DBNull.Value) ||
                        (RunRecsFound.Count == 1 && HGLMEDefaultRun["HG_LME_DEFAULT_TEST_RUN_ID"] != DBNull.Value && HGLMEDefaultRun["HG_LME_DEFAULT_TEST_RUN_ID"].ToString() != RunRecsFound[0]["HG_LME_DEFAULT_TEST_RUN_ID"].ToString()))
                        Category.CheckCatalogResult = "A";
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HGLME41");
            }

            return ReturnVal;
        }

        public static string HGLME42(cCategory Category, ref bool Log)
        //Run APS Indicator Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView HGLMEDefaultLevel = Category.GetCheckParameter("Current_Hg_LME_Default_Test_Level").ValueAsDataRowView();   
                DataRowView HGLMEDefaultRun = Category.GetCheckParameter("Current_Hg_LME_Default_Run").ValueAsDataRowView();
                if (cDBConvert.ToString(HGLMEDefaultLevel["REF_METHOD_CD"]) == "30A")
                    if (cDBConvert.ToInteger(HGLMEDefaultRun["APS_IND"]) == 1)
                        Category.CheckCatalogResult = "A";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HGLME42");
            }

            return ReturnVal;
        }

        public static string HGLME43(cCategory Category, ref bool Log)
        //Run APS Indicator Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentHGLMETest = Category.GetCheckParameter("Current_Hg_LME_Default_Test").ValueAsDataRowView();
                DataRowView MonitorLocationRec = Category.GetCheckParameter("Current_Monitor_Location").ValueAsDataRowView();
                string LocId = cDBConvert.ToString(MonitorLocationRec["LOCATION_IDENTIFIER"]);
                string CSTestCd = cDBConvert.ToString(CurrentHGLMETest["COMMON_STACK_TEST_CD"]);
                if (LocId.StartsWith("CS"))
                {
                    if (CSTestCd == "")
                        Category.CheckCatalogResult = "A";
                    else
                    {
                        DataView CSTestCdLookup = Category.GetCheckParameter("Common_Stack_Test_Code_Lookup_Table").ValueAsDataView();
                        sFilterPair[] Filter = new sFilterPair[1];
                        Filter[0].Set("COMMON_STACK_TEST_CD", CSTestCd);
                        if (CountRows(CSTestCdLookup, Filter) == 0)
                            Category.CheckCatalogResult = "B";
                        else
                            if (cDBConvert.ToInteger(CurrentHGLMETest["CS_UNIT_TEST_IND"]) == 1)
                            {
                                if (!CSTestCd.InList("AU,IU"))
                                    Category.CheckCatalogResult = "B";
                            }
                            else
                                if (CSTestCd == "IU")
                                    Category.CheckCatalogResult = "B";
                                else
                                    if (CSTestCd.InList("NAUS,NAUD"))
                                        if (cDBConvert.ToString(CurrentHGLMETest["TEST_REASON_CD"]) == "INITIAL")
                                            Category.CheckCatalogResult = "C";
                    }
                }
                else
                    if (CSTestCd != "")
                        Category.CheckCatalogResult = "D";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HGLME43");
            }

            return ReturnVal;
        }

        #endregion
    }
}
