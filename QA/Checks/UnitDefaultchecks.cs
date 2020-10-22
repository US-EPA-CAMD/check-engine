using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;
using ECMPS.Definitions.Extensions;

namespace ECMPS.Checks.UnitDefaultChecks
{
    public class cUnitDefaultChecks : cChecks
    {
        #region Constructors

        public cUnitDefaultChecks()
        {
            CheckProcedures = new dCheckProcedure[31];

            CheckProcedures[1] = new dCheckProcedure(UNITDEF1);
            CheckProcedures[2] = new dCheckProcedure(UNITDEF2);
            CheckProcedures[3] = new dCheckProcedure(UNITDEF3);
            CheckProcedures[4] = new dCheckProcedure(UNITDEF4);
            CheckProcedures[5] = new dCheckProcedure(UNITDEF5);
            CheckProcedures[6] = new dCheckProcedure(UNITDEF6);
            CheckProcedures[7] = new dCheckProcedure(UNITDEF7);
            CheckProcedures[8] = new dCheckProcedure(UNITDEF8);
            CheckProcedures[9] = new dCheckProcedure(UNITDEF9);
            CheckProcedures[10] = new dCheckProcedure(UNITDEF10);
            CheckProcedures[11] = new dCheckProcedure(UNITDEF11);
            CheckProcedures[12] = new dCheckProcedure(UNITDEF12);
            CheckProcedures[13] = new dCheckProcedure(UNITDEF13);
            CheckProcedures[14] = new dCheckProcedure(UNITDEF14);
            CheckProcedures[15] = new dCheckProcedure(UNITDEF15);
            CheckProcedures[16] = new dCheckProcedure(UNITDEF16);
            CheckProcedures[17] = new dCheckProcedure(UNITDEF17);
            CheckProcedures[18] = new dCheckProcedure(UNITDEF18);
            CheckProcedures[19] = new dCheckProcedure(UNITDEF19);
            CheckProcedures[20] = new dCheckProcedure(UNITDEF20);
            CheckProcedures[21] = new dCheckProcedure(UNITDEF21);
            CheckProcedures[22] = new dCheckProcedure(UNITDEF22);
            CheckProcedures[23] = new dCheckProcedure(UNITDEF23);
            CheckProcedures[24] = new dCheckProcedure(UNITDEF24);
            CheckProcedures[25] = new dCheckProcedure(UNITDEF25);
            CheckProcedures[26] = new dCheckProcedure(UNITDEF26);
            CheckProcedures[27] = new dCheckProcedure(UNITDEF27);
            CheckProcedures[28] = new dCheckProcedure(UNITDEF28);
            CheckProcedures[29] = new dCheckProcedure(UNITDEF29);
            CheckProcedures[30] = new dCheckProcedure(UNITDEF30);
        }

        #endregion


        #region UnitDefault Checks

        public static string UNITDEF1(cCategory Category, ref bool Log)
        //Initialize Unit Default Test Variables
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("Unit_Default_Maximum_NOx_Rate", 0m, eParameterDataType.Decimal);
                Category.SetCheckParameter("Unit_Default_Level_Sum_Reference_Value", 0m, eParameterDataType.Decimal);
                Category.SetCheckParameter("Unit_Default_Level_Count", 0, eParameterDataType.Integer);
                Category.SetCheckParameter("Unit_Default_Flagged_NOx_Rate", 0m, eParameterDataType.Decimal);
                Category.SetCheckParameter("Unit_Default_Flagged_Level_Sum_Reference_Value", 0m, eParameterDataType.Decimal);
                Category.SetCheckParameter("Unit_Default_Level_Run_Count", 0, eParameterDataType.Integer);
                Category.SetCheckParameter("Unit_Default_Last_Run_Number", 0, eParameterDataType.Integer);
                Category.SetCheckParameter("Unit_Default_Run_Sequence_Valid", true, eParameterDataType.Boolean);
                Category.SetCheckParameter("Unit_Default_Run_Used_Indicators_Consistent", true, eParameterDataType.Boolean);
                Category.SetCheckParameter("Unit_Default_Run_Sequence_Consecutive", false, eParameterDataType.Boolean);
                Category.SetCheckParameter("Unit_Default_Run_Sequence", null, eParameterDataType.String);
                Category.SetCheckParameter("Unit_Default_Last_Op_Level", null, eParameterDataType.String);
                Category.SetCheckParameter("Unit_Default_Flagged_Op_Level", null, eParameterDataType.String);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "UNITDEF1");
            }

            return ReturnVal;
        }

        public static string UNITDEF2(cCategory Category, ref bool Log)
        //Unit Default Test Fuel Code Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentUnitDefTest = (DataRowView)Category.GetCheckParameter("Current_Unit_Default_Test").ParameterValue;
                string FuelCd = cDBConvert.ToString(CurrentUnitDefTest["FUEL_CD"]);
                if (FuelCd == "")
                {
                    Category.SetCheckParameter("Unit_Default_Fuel_Valid", false, eParameterDataType.Boolean);
                    Category.CheckCatalogResult = "A";
                }
                else
                {
                    DataView FuelCdLookup = (DataView)Category.GetCheckParameter("Fuel_Code_Lookup_Table").ParameterValue;
                    string OldFilter = FuelCdLookup.RowFilter;
                    FuelCdLookup.RowFilter = AddToDataViewFilter(OldFilter, "FUEL_CD = '" + FuelCd + "'");
                    if( FuelCdLookup.Count == 0 || !cDBConvert.ToString(FuelCdLookup[0]["FUEL_GROUP_CD"]).InList( "OIL,GAS" ) )
                    {
                        Category.SetCheckParameter("Unit_Default_Fuel_Valid", false, eParameterDataType.Boolean);
                        Category.CheckCatalogResult = "B";
                    }
                    else
                        Category.SetCheckParameter("Unit_Default_Fuel_Valid", true, eParameterDataType.Boolean);
                    FuelCdLookup.RowFilter = OldFilter;
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "UNITDEF2");
            }

            return ReturnVal;
        }

        public static string UNITDEF3(cCategory Category, ref bool Log)
        //Unit Default Test Operating Condition Code Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentUnitDefTest = (DataRowView)Category.GetCheckParameter("Current_Unit_Default_Test").ParameterValue;
                string OpCondCd = cDBConvert.ToString(CurrentUnitDefTest["OPERATING_CONDITION_CD"]);
                if (OpCondCd != "")
                    if( !OpCondCd.InList( "A,B,P" ) )
                        Category.CheckCatalogResult = "A";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "UNITDEF3");
            }

            return ReturnVal;
        }

        public static string UNITDEF4(cCategory Category, ref bool Log)
        //Unit Default Test Operating Condition Code Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentUnitDefTest = (DataRowView)Category.GetCheckParameter("Current_Unit_Default_Test").ParameterValue;
                string TestReasCd = cDBConvert.ToString(CurrentUnitDefTest["TEST_REASON_CD"]);
                if (TestReasCd == "")
                {
                    DateTime MPDate = Category.GetCheckParameter("ECMPS_MP_Begin_Date").ValueAsDateTime(DateTypes.START);
                    if (cDBConvert.ToDate(CurrentUnitDefTest["END_DATE"], DateTypes.END) >= MPDate)
                        Category.CheckCatalogResult = "A";
                    else
                        Category.CheckCatalogResult = "B";
                }
                else
                    if( !TestReasCd.InList( "INITIAL,QA,RECERT" ) )
                    {
                        DataView TestReasCdLookup = (DataView)Category.GetCheckParameter("Test_Reason_Code_Lookup_Table").ParameterValue;
                        string OldFilter = TestReasCdLookup.RowFilter;
                        TestReasCdLookup.RowFilter = AddToDataViewFilter(OldFilter, "TEST_REASON_CD = '" + TestReasCd + "'");
                        if (TestReasCdLookup.Count == 0)
                            Category.CheckCatalogResult = "C";
                        else
                            Category.CheckCatalogResult = "D";
                        TestReasCdLookup.RowFilter = OldFilter;
                    }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "UNITDEF4");
            }

            return ReturnVal;
        }

        public static string UNITDEF5(cCategory Category, ref bool Log)
        //Unit Default Test NOx Default Rate Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentUnitDefTest = (DataRowView)Category.GetCheckParameter("Current_Unit_Default_Test").ParameterValue;
                decimal NOxDefRate = cDBConvert.ToDecimal(CurrentUnitDefTest["NOX_DEFAULT_RATE"]);
                if (NOxDefRate == decimal.MinValue)
                    Category.CheckCatalogResult = "A";
                else
                    if (NOxDefRate < 0)
                        Category.CheckCatalogResult = "B";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "UNITDEF5");
            }

            return ReturnVal;
        }

        public static string UNITDEF6(cCategory Category, ref bool Log)
        //Identification of Previously Reported Test or Test Number for Unit Default Test
        {
            string ReturnVal = "";

            try
            {
                if (Convert.ToBoolean(Category.GetCheckParameter("Test_End_Date_Valid").ParameterValue) &&
                    Convert.ToBoolean(Category.GetCheckParameter("Test_End_Hour_Valid").ParameterValue) &&
                    Convert.ToBoolean(Category.GetCheckParameter("Test_End_Minute_Valid").ParameterValue))
                {
                    DataRowView CurrentUnitDefTest = (DataRowView)Category.GetCheckParameter("Current_Unit_Default_Test").ParameterValue;
                    string FuelCd = cDBConvert.ToString(CurrentUnitDefTest["FUEL_CD"]);
                    string OpCondCd = cDBConvert.ToString(CurrentUnitDefTest["OPERATING_CONDITION_CD"]);
                    string EndDate = cDBConvert.ToDate(CurrentUnitDefTest["END_DATE"], DateTypes.END).ToShortDateString();
                    int EndHour = cDBConvert.ToHour(CurrentUnitDefTest["END_HOUR"], DateTypes.END);
                    int EndMin = cDBConvert.ToInteger(CurrentUnitDefTest["END_MIN"]);
                    string TestNum = cDBConvert.ToString(CurrentUnitDefTest["TEST_NUM"]);
                    DataView UnitDefTestRecs = (DataView)Category.GetCheckParameter("Unit_Default_Test_Records").ParameterValue;
                    string OldFilter1 = UnitDefTestRecs.RowFilter;
                    if (OpCondCd != "")
                        UnitDefTestRecs.RowFilter = AddToDataViewFilter(OldFilter1, "FUEL_CD = '" + FuelCd + "' AND OPERATING_CONDITION_CD = '" +
                            OpCondCd + "' AND END_DATE = '" + EndDate + "' AND END_HOUR = " + EndHour + " AND END_MIN = " + EndMin + " AND TEST_NUM <> '" + TestNum + "'");
                    else
                        UnitDefTestRecs.RowFilter = AddToDataViewFilter(OldFilter1, "FUEL_CD = '" + FuelCd + "' AND OPERATING_CONDITION_CD IS NULL AND END_DATE = '" +
                            EndDate + "' AND END_HOUR = " + EndHour + " AND END_MIN = " + EndMin + " AND TEST_NUM <> '" + TestNum + "'");
                    if ((UnitDefTestRecs.Count > 0 && CurrentUnitDefTest["TEST_SUM_ID"] == DBNull.Value) ||
                        (UnitDefTestRecs.Count > 1 && CurrentUnitDefTest["TEST_SUM_ID"] != DBNull.Value) ||
                        (UnitDefTestRecs.Count == 1 && CurrentUnitDefTest["TEST_SUM_ID"] != DBNull.Value && CurrentUnitDefTest["TEST_SUM_ID"].ToString() != UnitDefTestRecs[0]["TEST_SUM_ID"].ToString()))
                                        
                    {
                        Category.SetCheckParameter("Extra_Unit_Default_Test", true, eParameterDataType.Boolean);
                        Category.CheckCatalogResult = "A";
                    }
                    else
                    {
                        DataView QASuppRecs = (DataView)Category.GetCheckParameter("QA_Supplemental_Data_Records").ParameterValue;
                        string OldFilter2 = QASuppRecs.RowFilter;
                        if (OpCondCd != "")
                            QASuppRecs.RowFilter = AddToDataViewFilter(OldFilter2, "TEST_TYPE_CD = 'UNITDEF' AND FUEL_CD = '" + FuelCd + "' AND OPERATING_CONDITION_CD = '" +
                                OpCondCd + "' AND END_DATE = '" + EndDate + "' AND END_HOUR = " + EndHour + " AND END_MIN = " +
                                EndMin + " AND TEST_NUM <> '" + TestNum + "'");
                        else
                            QASuppRecs.RowFilter = AddToDataViewFilter(OldFilter2, "TEST_TYPE_CD = 'UNITDEF' AND FUEL_CD = '" + FuelCd +
                                "' AND OPERATING_CONDITION_CD IS NULL AND END_DATE = '" + EndDate + "' AND END_HOUR = " + EndHour + " AND END_MIN = " +
                                EndMin + " AND TEST_NUM <> '" + TestNum + "'");
                        if ((QASuppRecs.Count > 0 && CurrentUnitDefTest["TEST_SUM_ID"] == DBNull.Value) ||
                            (QASuppRecs.Count > 1 && CurrentUnitDefTest["TEST_SUM_ID"] != DBNull.Value) ||
                            (QASuppRecs.Count == 1 && CurrentUnitDefTest["TEST_SUM_ID"] != DBNull.Value && CurrentUnitDefTest["TEST_SUM_ID"].ToString() != QASuppRecs[0]["TEST_SUM_ID"].ToString()))
                        {
                            Category.SetCheckParameter("Extra_Unit_Default_Test", true, eParameterDataType.Boolean);
                            Category.CheckCatalogResult = "A";
                        }
                        else
                        {
                            Category.SetCheckParameter("Extra_Unit_Default_Test", false, eParameterDataType.Boolean);
                            QASuppRecs.RowFilter = AddToDataViewFilter(OldFilter2, "TEST_TYPE_CD = 'UNITDEF'" + " AND TEST_NUM = '" + TestNum + "'");
                            if (QASuppRecs.Count > 0)
                                if (cDBConvert.ToString(QASuppRecs[0]["CAN_SUBMIT"]) == "N")
                                {
                                    if (OpCondCd != "")
                                        QASuppRecs.RowFilter = AddToDataViewFilter(QASuppRecs.RowFilter, "FUEL_CD = '" + FuelCd + "' AND OPERATING_CONDITION_CD <> '" +
                                            OpCondCd + "' AND END_DATE <> '" + EndDate + "' AND END_HOUR <> " + EndHour + " AND END_MIN <> " + EndMin);
                                    else
                                        QASuppRecs.RowFilter = AddToDataViewFilter(QASuppRecs.RowFilter, "FUEL_CD <> '" + FuelCd + "' AND OPERATING_CONDITION_CD IS NULL AND END_DATE <> '" +
                                            EndDate + "' AND END_HOUR <> " + EndHour + " AND END_MIN <> " + EndMin);
                                    if ((QASuppRecs.Count > 0 && CurrentUnitDefTest["TEST_SUM_ID"] == DBNull.Value) ||
                                        (QASuppRecs.Count > 1 && CurrentUnitDefTest["TEST_SUM_ID"] != DBNull.Value) ||
                                        (QASuppRecs.Count == 1 && CurrentUnitDefTest["TEST_SUM_ID"] != DBNull.Value && CurrentUnitDefTest["TEST_SUM_ID"].ToString() != QASuppRecs[0]["TEST_SUM_ID"].ToString()))
                                        Category.CheckCatalogResult = "B";
                                    else
                                        Category.CheckCatalogResult = "C";
                                }
                        }
                        QASuppRecs.RowFilter = OldFilter2;
                    }
                    UnitDefTestRecs.RowFilter = OldFilter1;
                }
                else
                    Log = false;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "UNITDEF6");
            }

            return ReturnVal;
        }

        public static string UNITDEF7(cCategory Category, ref bool Log)
        //Determine Run Sequence
        {
            string ReturnVal = "";

            try
            {
                DataView UnitDefRunRecs = (DataView)Category.GetCheckParameter("Unit_Default_Run_Records").ParameterValue;
                if (UnitDefRunRecs.Count == 0)
                    Category.SetCheckParameter("Unit_Default_Run_Times_Valid", false, eParameterDataType.Boolean);
                else
                {
                    Category.SetCheckParameter("Unit_Default_Run_Times_Valid", true, eParameterDataType.Boolean);
                    UnitDefRunRecs.Sort = "BEGIN_DATE, BEGIN_HOUR, BEGIN_MIN";
                    bool FirstRun = true;
                    DateTime BeginDate;
                    int BeginHour;
                    int BeginMin;
                    DateTime EndDate;
                    int EndHour;
                    int EndMin;
                    DateTime PrevEndDate = DateTime.MinValue;
                    int PrevEndHour = int.MinValue;
                    int PrevEndMin = int.MinValue;
                    foreach (DataRowView drv in UnitDefRunRecs)
                    {
                        BeginDate = cDBConvert.ToDate(drv["BEGIN_DATE"], DateTypes.START);
                        BeginHour = cDBConvert.ToInteger(drv["BEGIN_HOUR"]);
                        BeginMin = cDBConvert.ToInteger(drv["BEGIN_MIN"]);
                        EndDate = cDBConvert.ToDate(drv["END_DATE"], DateTypes.END);
                        EndHour = cDBConvert.ToInteger(drv["END_HOUR"]);
                        EndMin = cDBConvert.ToInteger(drv["END_MIN"]);
                        if (FirstRun)
                        {
                            Category.SetCheckParameter("Simultaneous_Unit_Default_Runs", false, eParameterDataType.Boolean);
                            if (BeginDate == DateTime.MinValue || BeginHour < 0 || 23 < BeginHour || BeginMin < 0 || 59 < BeginMin)
                                Category.SetCheckParameter("Unit_Default_Run_Times_Valid", false, eParameterDataType.Boolean);
                            else
                            {
                                Category.SetCheckParameter("Unit_Default_Test_Begin_Date", BeginDate, eParameterDataType.Date);
                                Category.SetCheckParameter("Unit_Default_Test_Begin_Hour", BeginHour, eParameterDataType.Integer);
                                Category.SetCheckParameter("Unit_Default_Test_Begin_Minute", BeginMin, eParameterDataType.Integer);
                            }
                            if (EndDate == DateTime.MaxValue || EndHour < 0 || 23 < EndHour || EndMin < 0 || 59 < EndMin)
                                Category.SetCheckParameter("Unit_Default_Run_Times_Valid", false, eParameterDataType.Boolean);
                            else
                            {
                                Category.SetCheckParameter("Unit_Default_Test_End_Date", EndDate, eParameterDataType.Date);
                                Category.SetCheckParameter("Unit_Default_Test_End_Hour", EndHour, eParameterDataType.Integer);
                                Category.SetCheckParameter("Unit_Default_Test_End_Minute", EndMin, eParameterDataType.Integer);
                            }
                            FirstRun = false;
                        }
                        else
                        {
                            if (Convert.ToBoolean(Category.GetCheckParameter("Unit_Default_Run_Times_Valid").ParameterValue) &&
                                (BeginDate != DateTime.MinValue && 0 <= BeginHour && BeginHour <= 23 && 0 <= BeginMin && BeginMin <= 59))
                                if (BeginDate < PrevEndDate || (BeginDate == PrevEndDate && (BeginHour < PrevEndHour ||
                                    (BeginHour == PrevEndHour && BeginMin < PrevEndMin))))
                                    Category.SetCheckParameter("Simultaneous_Unit_Default_Runs", true, eParameterDataType.Boolean);
                            if (EndDate == DateTime.MaxValue || EndHour < 0 || 23 < EndHour || EndMin < 0 || 59 < EndMin)
                                Category.SetCheckParameter("Unit_Default_Run_Times_Valid", false, eParameterDataType.Boolean);
                            else
                            {
                                Category.SetCheckParameter("Unit_Default_Test_End_Date", EndDate, eParameterDataType.Date);
                                Category.SetCheckParameter("Unit_Default_Test_End_Hour", EndHour, eParameterDataType.Integer);
                                Category.SetCheckParameter("Unit_Default_Test_End_Minute", EndMin, eParameterDataType.Integer);
                            }
                        }
                        PrevEndDate = EndDate;
                        PrevEndHour = EndHour;
                        PrevEndMin = EndMin;
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "UNITDEF7");
            }

            return ReturnVal;
        }

        public static string UNITDEF8(cCategory Category, ref bool Log)
        //Unit Default Test Number of Units in Group Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentUnitDefTest = (DataRowView)Category.GetCheckParameter("Current_Unit_Default_Test").ParameterValue;
                int NumUnitsInGrp = cDBConvert.ToInteger(CurrentUnitDefTest["NUM_UNITS_IN_GROUP"]);
                if (cDBConvert.ToString(CurrentUnitDefTest["GROUP_ID"]) != "")
                {
                    if (NumUnitsInGrp == int.MinValue)
                        Category.CheckCatalogResult = "A";
                    else
                        if (NumUnitsInGrp < 2)
                            Category.CheckCatalogResult = "B";
                }
                else
                    if (NumUnitsInGrp != int.MinValue)
                        Category.CheckCatalogResult = "C";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "UNITDEF8");
            }

            return ReturnVal;
        }

        public static string UNITDEF9(cCategory Category, ref bool Log)
        //Unit Default Test Number of Tests for Group Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentUnitDefTest = (DataRowView)Category.GetCheckParameter("Current_Unit_Default_Test").ParameterValue;
                string GroupId = cDBConvert.ToString(CurrentUnitDefTest["GROUP_ID"]);
                int NumTstsForGrp = cDBConvert.ToInteger(CurrentUnitDefTest["NUM_TESTS_FOR_GROUP"]);
                if (GroupId != "")
                {
                    if (NumTstsForGrp == int.MinValue)
                        Category.CheckCatalogResult = "A";
                    else
                    {
                        int NumUnitsInGrp = cDBConvert.ToInteger(CurrentUnitDefTest["NUM_UNITS_IN_GROUP"]);
                        if (NumUnitsInGrp < 3)
                        {
                            if (NumTstsForGrp < 1)
                                Category.CheckCatalogResult = "B";
                        }
                        else
                            if (NumUnitsInGrp < 7)
                            {
                                if (NumTstsForGrp < 2)
                                    Category.CheckCatalogResult = "B";
                            }
                            else
                                if (NumUnitsInGrp < 11)
                                {
                                    if (NumTstsForGrp < 3)
                                        Category.CheckCatalogResult = "B";
                                }
                                else
                                {
                                    decimal tempVal = Math.Round((decimal)NumUnitsInGrp / 3, MidpointRounding.AwayFromZero);
                                    if (NumTstsForGrp < tempVal)
                                        Category.CheckCatalogResult = "B";
                                }
                    }
                }
                else
                    if (NumTstsForGrp != int.MinValue)
                        Category.CheckCatalogResult = "C";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "UNITDEF9");
            }

            return ReturnVal;
        }

        public static string UNITDEF10(cCategory Category, ref bool Log)
        //Unit Default Test Begin Time Consistent with Run Times
        {
            string ReturnVal = "";

            try
            {
                if (Convert.ToBoolean(Category.GetCheckParameter("Unit_Default_Run_Times_Valid").ParameterValue) &&
                    Convert.ToBoolean(Category.GetCheckParameter("Test_Begin_Date_Valid").ParameterValue) &&
                    Convert.ToBoolean(Category.GetCheckParameter("Test_Begin_Hour_Valid").ParameterValue) &&
                    Convert.ToBoolean(Category.GetCheckParameter("Test_Begin_Minute_Valid").ParameterValue))
                {
                    DataRowView CurrentUnitDefTest = (DataRowView)Category.GetCheckParameter("Current_Unit_Default_Test").ParameterValue;
                    if (cDBConvert.ToDate(CurrentUnitDefTest["BEGIN_DATE"], DateTypes.START) != Convert.ToDateTime(Category.GetCheckParameter("Unit_Default_Test_Begin_Date").ParameterValue) ||
                        cDBConvert.ToInteger(CurrentUnitDefTest["BEGIN_HOUR"]) != Convert.ToInt16(Category.GetCheckParameter("Unit_Default_Test_Begin_Hour").ParameterValue) ||
                        cDBConvert.ToInteger(CurrentUnitDefTest["BEGIN_MIN"]) != Convert.ToInt16(Category.GetCheckParameter("Unit_Default_Test_Begin_Minute").ParameterValue))
                        Category.CheckCatalogResult = "A";
                }
                else
                    Log = false;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "UNITDEF10");
            }

            return ReturnVal;
        }

        public static string UNITDEF11(cCategory Category, ref bool Log)
        //Unit Default Test End Time Consistent with Run Times
        {
            string ReturnVal = "";

            try
            {
                if (Convert.ToBoolean(Category.GetCheckParameter("Unit_Default_Run_Times_Valid").ParameterValue) &&
                    Convert.ToBoolean(Category.GetCheckParameter("Test_End_Date_Valid").ParameterValue) &&
                    Convert.ToBoolean(Category.GetCheckParameter("Test_End_Hour_Valid").ParameterValue) &&
                    Convert.ToBoolean(Category.GetCheckParameter("Test_End_Minute_Valid").ParameterValue))
                {
                    DataRowView CurrentUnitDefTest = (DataRowView)Category.GetCheckParameter("Current_Unit_Default_Test").ParameterValue;
                    if (cDBConvert.ToDate(CurrentUnitDefTest["END_DATE"], DateTypes.END) != Convert.ToDateTime(Category.GetCheckParameter("Unit_Default_Test_End_Date").ParameterValue) ||
                        cDBConvert.ToInteger(CurrentUnitDefTest["END_HOUR"]) != Convert.ToInt16(Category.GetCheckParameter("Unit_Default_Test_End_Hour").ParameterValue) ||
                        cDBConvert.ToInteger(CurrentUnitDefTest["END_MIN"]) != Convert.ToInt16(Category.GetCheckParameter("Unit_Default_Test_End_Minute").ParameterValue))
                        Category.CheckCatalogResult = "A";
                }
                else
                    Log = false;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "UNITDEF11");
            }

            return ReturnVal;
        }

        public static string UNITDEF12(cCategory Category, ref bool Log)
        //Unit Default Test Consistent with Methodology
        {
            string ReturnVal = "";

            try
            {
                if (Convert.ToBoolean(Category.GetCheckParameter("Test_Dates_Consistent").ParameterValue))
                {
                    DataRowView CurrentUnitDefTest = (DataRowView)Category.GetCheckParameter("Current_Unit_Default_Test").ParameterValue;
                    string EndDate = cDBConvert.ToDate(CurrentUnitDefTest["END_DATE"], DateTypes.END).ToShortDateString();
                    int EndHour = cDBConvert.ToHour(CurrentUnitDefTest["END_HOUR"], DateTypes.END);
                    DataView MethodRecs = (DataView)Category.GetCheckParameter("Method_Records").ParameterValue;
                    string OldFilter = MethodRecs.RowFilter;
                    MethodRecs.RowFilter = AddToDataViewFilter(OldFilter, "PARAMETER_CD = 'NOXM' AND METHOD_CD = 'LME' AND (END_DATE IS NULL OR (END_DATE > '" +
                        EndDate + "' OR (END_DATE = '" + EndDate + "' AND END_HOUR > " + EndHour + ")))");
                    if (MethodRecs.Count == 0)
                        Category.CheckCatalogResult = "A";
                    MethodRecs.RowFilter = OldFilter;
                }
                else
                    Log = false;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "UNITDEF12");
            }

            return ReturnVal;
        }

        public static string UNITDEF13(cCategory Category, ref bool Log)
        //Simultaneous Runs
        {
            string ReturnVal = "";

            try
            {
                if (Convert.ToBoolean(Category.GetCheckParameter("Unit_Default_Run_Times_Valid").ParameterValue))
                {
                    if (Convert.ToBoolean(Category.GetCheckParameter("Simultaneous_Unit_Default_Runs").ParameterValue))
                        Category.CheckCatalogResult = "A";
                }
                else
                    Log = false;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "UNITDEF13");
            }

            return ReturnVal;
        }

        public static string UNITDEF14(cCategory Category, ref bool Log)
        //Concurrent Unit Default Tests
        {
            string ReturnVal = "";

            try
            {
                if (Convert.ToBoolean(Category.GetCheckParameter("Test_Dates_Consistent").ParameterValue))
                {
                    DataRowView CurrentUnitDefTest = (DataRowView)Category.GetCheckParameter("Current_Unit_Default_Test").ParameterValue;
                    string FuelCd = cDBConvert.ToString(CurrentUnitDefTest["FUEL_CD"]);
                    string OpCondCd = cDBConvert.ToString(CurrentUnitDefTest["OPERATING_CONDITION_CD"]);
                    string EndDate = cDBConvert.ToDate(CurrentUnitDefTest["END_DATE"], DateTypes.END).ToShortDateString();
                    int EndHour = cDBConvert.ToHour(CurrentUnitDefTest["END_HOUR"], DateTypes.END);
                    int EndMin = cDBConvert.ToInteger(CurrentUnitDefTest["END_MIN"]);
                    string BeginDate = cDBConvert.ToDate(CurrentUnitDefTest["BEGIN_DATE"], DateTypes.START).ToShortDateString();
                    int BeginHour = cDBConvert.ToHour(CurrentUnitDefTest["BEGIN_HOUR"], DateTypes.START);
                    int BeginMin = cDBConvert.ToInteger(CurrentUnitDefTest["BEGIN_MIN"]);
                    string TestNum = cDBConvert.ToString(CurrentUnitDefTest["TEST_NUM"]);
                    string TestSumId = cDBConvert.ToString(CurrentUnitDefTest["TEST_SUM_ID"]);
                    DataView UnitDefTestRecs = (DataView)Category.GetCheckParameter("Unit_Default_Test_Records").ParameterValue;
                    string OldFilter1 = UnitDefTestRecs.RowFilter;
                    if (OpCondCd != "")
                        UnitDefTestRecs.RowFilter = AddToDataViewFilter(OldFilter1, "FUEL_CD = '" + FuelCd + "' AND OPERATING_CONDITION_CD = '" + OpCondCd +
                            "' AND (BEGIN_DATE < '" + EndDate + "' OR (BEGIN_DATE = '" + EndDate + "' AND (BEGIN_HOUR < " + EndHour +
                            " OR (BEGIN_HOUR = " + EndHour + " AND BEGIN_MIN < " + EndMin + ")))) AND (END_DATE > '" + BeginDate + "' OR (END_DATE = '" +
                            BeginDate + "' AND (END_HOUR > " + BeginHour + " OR (END_HOUR = " + BeginHour + " AND END_MIN > " + BeginMin + ")))) AND TEST_NUM <> '" + TestNum + "'");
                    else
                        UnitDefTestRecs.RowFilter = AddToDataViewFilter(OldFilter1, "FUEL_CD = '" + FuelCd + "' AND OPERATING_CONDITION_CD IS NULL AND (BEGIN_DATE < '" +
                            EndDate + "' OR (BEGIN_DATE = '" + EndDate + "' AND (BEGIN_HOUR < " + EndHour +
                            " OR (BEGIN_HOUR = " + EndHour + " AND BEGIN_MIN < " + EndMin + ")))) AND (END_DATE > '" + BeginDate + "' OR (END_DATE = '" +
                            BeginDate + "' AND (END_HOUR > " + BeginHour + " OR (END_HOUR = " + BeginHour + " AND END_MIN > " + BeginMin + ")))) AND TEST_NUM <> '" + TestNum + "'");
                    if (UnitDefTestRecs.Count > 0)
                        Category.CheckCatalogResult = "A";
                    else
                    {
                        DataView QASuppRecs = (DataView)Category.GetCheckParameter("QA_Supplemental_Data_Records").ParameterValue;
                        string OldFilter2 = QASuppRecs.RowFilter;
                        if (OpCondCd != "")
                            QASuppRecs.RowFilter = AddToDataViewFilter(OldFilter2, "test_sum_id <> '" + TestSumId + "' and TEST_TYPE_CD = 'UNITDEF' AND FUEL_CD = '" + FuelCd +
                                "' AND OPERATING_CONDITION_CD = '" + OpCondCd + "' AND (BEGIN_DATE < '" + EndDate + "' OR (BEGIN_DATE = '" + EndDate +
                                "' AND (BEGIN_HOUR < " + EndHour + " OR (BEGIN_HOUR = " + EndHour + " AND BEGIN_MIN < " + EndMin + ")))) AND (END_DATE > '" +
                                BeginDate + "' OR (END_DATE = '" + BeginDate + "' AND (END_HOUR > " + BeginHour + " OR (END_HOUR = " + BeginHour +
                                " AND END_MIN > " + BeginMin + ")))) AND TEST_NUM <> '" + TestNum + "'");
                        else
                            QASuppRecs.RowFilter = AddToDataViewFilter(OldFilter2, "test_sum_id <> '" + TestSumId + "' and TEST_TYPE_CD = 'UNITDEF' AND FUEL_CD = '" + FuelCd +
                                "' AND OPERATING_CONDITION_CD IS NULL AND (BEGIN_DATE < '" + EndDate + "' OR (BEGIN_DATE = '" + EndDate +
                                "' AND (BEGIN_HOUR < " + EndHour + " OR (BEGIN_HOUR = " + EndHour + " AND BEGIN_MIN < " + EndMin + ")))) AND (END_DATE > '" +
                                BeginDate + "' OR (END_DATE = '" + BeginDate + "' AND (END_HOUR > " + BeginHour + " OR (END_HOUR = " + BeginHour +
                                " AND END_MIN > " + BeginMin + ")))) AND TEST_NUM <> '" + TestNum + "'");
                        if (QASuppRecs.Count > 0)
                            Category.CheckCatalogResult = "A";
                        QASuppRecs.RowFilter = OldFilter2;
                    }
                    UnitDefTestRecs.RowFilter = OldFilter1;
                }
                else
                    Log = false;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "UNITDEF14");
            }

            return ReturnVal;
        }

        public static string UNITDEF15(cCategory Category, ref bool Log)
        //Insufficient Number of Runs
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentUnitDefRun = (DataRowView)Category.GetCheckParameter("Current_Unit_Default_Run").ParameterValue;
                int RunUsedInd = cDBConvert.ToInteger(CurrentUnitDefRun["RUN_USED_IND"]);
                string FlaggedOpLvl = Convert.ToString(Category.GetCheckParameter("Unit_Default_Flagged_Op_Level").ParameterValue);
                string OpLvl = cDBConvert.ToString(CurrentUnitDefRun["OP_LEVEL_NUM"]);

                if (Convert.ToString(Category.GetCheckParameter("Unit_Default_Last_Op_Level").ParameterValue) == OpLvl)
                {
                    if (OpLvl.InList(FlaggedOpLvl))
                    {
                        if (RunUsedInd != 1)
                            Category.SetCheckParameter("Unit_Default_Run_Used_Indicators_Consistent", false, eParameterDataType.Boolean);
                    }
                    else
                        if (RunUsedInd == 1)
                        {
                            Category.SetCheckParameter("Unit_Default_Run_Used_Indicators_Consistent", false, eParameterDataType.Boolean);
                            FlaggedOpLvl = FlaggedOpLvl.ListAdd(OpLvl, true);
                            Category.SetCheckParameter("Unit_Default_Flagged_Op_Level", FlaggedOpLvl, eParameterDataType.String);
                        }
                }
                else
                {
                    int LvlCt = Convert.ToInt16(Category.GetCheckParameter("Unit_Default_Level_Count").ParameterValue);
                    Category.SetCheckParameter("Unit_Default_Level_Count", ++LvlCt, eParameterDataType.Integer);
                    if (RunUsedInd == 1)
                    {
                        FlaggedOpLvl = FlaggedOpLvl.ListAdd(OpLvl, true);
                        Category.SetCheckParameter("Unit_Default_Flagged_Op_Level", FlaggedOpLvl, eParameterDataType.Decimal);
                    }
                    if (Category.GetCheckParameter("Unit_Default_Last_Op_Level").ParameterValue != null)
                    {
                        if (Convert.ToInt16(Category.GetCheckParameter("Unit_Default_Level_Run_Count").ParameterValue) < 3)
                        {
                            Category.SetCheckParameter("Unit_Default_Incomplete_Level", Convert.ToString(Category.GetCheckParameter("Unit_Default_Last_Op_Level").ParameterValue), eParameterDataType.String);
                            Category.SetCheckParameter("Unit_Default_Level_Sum_Reference_Value", 0m, eParameterDataType.Decimal);
                            Category.SetCheckParameter("Unit_Default_Level_Run_Count", 0, eParameterDataType.Integer);
                            Category.SetCheckParameter("Unit_Default_Last_Op_Level", OpLvl, eParameterDataType.String);
                            Category.SetCheckParameter("Unit_Default_Maximum_NOx_Rate", -1, eParameterDataType.Decimal);
                            Category.CheckCatalogResult = "A";
                        }
                        else
                        {
                            decimal MaxNOxRate = Convert.ToDecimal(Category.GetCheckParameter("Unit_Default_Maximum_NOx_Rate").ParameterValue);
                            if (MaxNOxRate >= 0)
                            {
                                decimal LvlSumRefVal = Convert.ToDecimal(Category.GetCheckParameter("Unit_Default_Level_Sum_Reference_Value").ParameterValue);
                                int LvlRunCt = Convert.ToInt16(Category.GetCheckParameter("Unit_Default_Level_Run_Count").ParameterValue);
                                if (LvlSumRefVal >= 0)
                                {
                                    decimal tempRate = Math.Round(LvlSumRefVal / LvlRunCt, 3, MidpointRounding.AwayFromZero);
                                    if (tempRate > MaxNOxRate)
                                        Category.SetCheckParameter("Unit_Default_Maximum_NOx_Rate", tempRate, eParameterDataType.Decimal);
                                }
                                else
                                    Category.SetCheckParameter("Unit_Default_Maximum_NOx_Rate", -1, eParameterDataType.Decimal);
                                decimal FlaggedLvlSumRefVal = Convert.ToDecimal(Category.GetCheckParameter("Unit_Default_Flagged_Level_Sum_Reference_Value").ParameterValue);
                                if (FlaggedLvlSumRefVal >= 0)
                                    Category.SetCheckParameter("Unit_Default_Flagged_NOx_Rate", Math.Round(FlaggedLvlSumRefVal / LvlRunCt, 3, MidpointRounding.AwayFromZero), eParameterDataType.Decimal);
                            }
                        }
                    }
                    Category.SetCheckParameter("Unit_Default_Level_Sum_Reference_Value", 0m, eParameterDataType.Decimal);
                    Category.SetCheckParameter("Unit_Default_Level_Run_Count", 0, eParameterDataType.Integer);
                    Category.SetCheckParameter("Unit_Default_Last_Op_Level", OpLvl, eParameterDataType.String);
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "UNITDEF15");
            }

            return ReturnVal;
        }

        public static string UNITDEF16(cCategory Category, ref bool Log)
        //Operating Level for Run Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentUnitDefRun = (DataRowView)Category.GetCheckParameter("Current_Unit_Default_Run").ParameterValue;
                int OpLvl = cDBConvert.ToInteger(CurrentUnitDefRun["OP_LEVEL_NUM"]);
                if (OpLvl == int.MinValue)
                    Category.CheckCatalogResult = "A";
                else
                    if (OpLvl < 1 || 99 < OpLvl)
                        Category.CheckCatalogResult = "B";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "UNITDEF16");
            }

            return ReturnVal;
        }

        public static string UNITDEF17(cCategory Category, ref bool Log)
        //Run Begin Time Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentUnitDefRun = (DataRowView)Category.GetCheckParameter("Current_Unit_Default_Run").ParameterValue;
                int BeginHour = cDBConvert.ToInteger(CurrentUnitDefRun["BEGIN_HOUR"]);
                int BeginMin = cDBConvert.ToInteger(CurrentUnitDefRun["BEGIN_MIN"]);
                if (cDBConvert.ToDate(CurrentUnitDefRun["BEGIN_DATE"], DateTypes.START) == DateTime.MinValue || BeginHour < 0 ||
                    23 < BeginHour || BeginMin < 0 || 59 < BeginMin)
                    Category.CheckCatalogResult = "A";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "UNITDEF17");
            }

            return ReturnVal;
        }

        public static string UNITDEF18(cCategory Category, ref bool Log)
        //Run End Time Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentUnitDefRun = (DataRowView)Category.GetCheckParameter("Current_Unit_Default_Run").ParameterValue;
                DateTime EndDate = cDBConvert.ToDate(CurrentUnitDefRun["END_DATE"], DateTypes.END);
                int EndHour = cDBConvert.ToInteger(CurrentUnitDefRun["END_HOUR"]);
                int EndMin = cDBConvert.ToInteger(CurrentUnitDefRun["END_MIN"]);
                if (EndDate == DateTime.MaxValue || EndHour < 0 || 23 < EndHour || EndMin < 0 || 59 < EndMin)
                    Category.CheckCatalogResult = "A";
                else
                {
                    DateTime BeginDate = cDBConvert.ToDate(CurrentUnitDefRun["BEGIN_DATE"], DateTypes.START);
                    int BeginHour = cDBConvert.ToInteger(CurrentUnitDefRun["BEGIN_HOUR"]);
                    int BeginMin = cDBConvert.ToInteger(CurrentUnitDefRun["BEGIN_MIN"]);
                    if (BeginDate != DateTime.MinValue && 0 <= BeginHour && BeginHour < 23 && 0 <= BeginMin && BeginMin <= 59)
                        if (BeginDate > EndDate || (BeginDate == EndDate && (BeginHour > EndHour || (BeginHour == EndHour && BeginMin >= EndMin))))
                            Category.CheckCatalogResult = "B";
                        else
                        {
                            TimeSpan ts = EndDate - BeginDate;
                            int tempLength = (ts.Days * 24 * 60) + ((EndHour - BeginHour) * 60) + (EndMin - BeginMin);
                            if (tempLength < 8)
                                Category.CheckCatalogResult = "C";
                        }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "UNITDEF18");
            }

            return ReturnVal;
        }

        public static string UNITDEF19(cCategory Category, ref bool Log)
        //Response Time Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentUnitDefRun = (DataRowView)Category.GetCheckParameter("Current_Unit_Default_Run").ParameterValue;
                int RespTime = cDBConvert.ToInteger(CurrentUnitDefRun["RESPONSE_TIME"]);
                if (RespTime == int.MinValue)
                    Category.CheckCatalogResult = "A";
                else
                    if (RespTime < 0 || 800 < RespTime)
                        Category.CheckCatalogResult = "B";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "UNITDEF19");
            }

            return ReturnVal;
        }

        public static string UNITDEF20(cCategory Category, ref bool Log)
        //Run Number Valid
        {
            string ReturnVal = "";

            try
            {
                int UnitDefLvlRunCt = Convert.ToInt16(Category.GetCheckParameter("Unit_Default_Level_Run_Count").ParameterValue);
                Category.SetCheckParameter("Unit_Default_Level_Run_Count", ++UnitDefLvlRunCt, eParameterDataType.Integer);
                DataRowView CurrentUnitDefRun = (DataRowView)Category.GetCheckParameter("Current_Unit_Default_Run").ParameterValue;
                int RunNum = cDBConvert.ToInteger(CurrentUnitDefRun["RUN_NUM"]);
                if (RunNum == int.MinValue)
                    Category.CheckCatalogResult = "A";
                else
                {
                    if (UnitDefLvlRunCt == 1)
                    {
                        if (Convert.ToInt16(Category.GetCheckParameter("Unit_Default_Level_Count").ParameterValue) == 1 &&
                            cDBConvert.ToInteger(CurrentUnitDefRun["OP_LEVEL_NUM"]) != 1)
                            Category.SetCheckParameter("Unit_Default_Run_Sequence_Valid", false, eParameterDataType.Boolean);
                        else
                            if (RunNum != 1)
                                Category.SetCheckParameter("Unit_Default_Run_Sequence_Consecutive", true, eParameterDataType.Boolean);
                    }
                    else
                        if (RunNum - Convert.ToInt16(Category.GetCheckParameter("Unit_Default_Last_Run_Number").ParameterValue) != 1)
                            Category.SetCheckParameter("Unit_Default_Run_Sequence_Valid", false, eParameterDataType.Boolean);
                    Category.SetCheckParameter("Unit_Default_Last_Run_Number", RunNum, eParameterDataType.Integer);

                    //Append Run Number to UnitDefRunSequence in numeric order
                    string RunSequ = Convert.ToString(Category.GetCheckParameter("Unit_Default_Run_Sequence").ParameterValue);
                    int[] SequArray = new int[RunSequ.ListCount() + 1];//make array with one extra place
                    int i;
                    for (i = 0; i < SequArray.Length - 1; i++)//fill all but last element of array
                        SequArray[i] = Convert.ToInt16(RunSequ.ListItem(i));
                    SequArray[i] = RunNum;//fill last element of array
                    Array.Sort(SequArray);
                    RunSequ = "";
                    for (i = 0; i < SequArray.Length; i++)
                        RunSequ = RunSequ.ListAdd(Convert.ToString(SequArray[i]), true);
                    Category.SetCheckParameter("Unit_Default_Run_Sequence", RunSequ, eParameterDataType.String);

                    if (RunNum <= 0)
                        Category.CheckCatalogResult = "B";
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "UNITDEF20");
            }

            return ReturnVal;
        }

        public static string UNITDEF21(cCategory Category, ref bool Log)
        //Reference Value for Run Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentUnitDefRun = (DataRowView)Category.GetCheckParameter("Current_Unit_Default_Run").ParameterValue;
                decimal RefVal = cDBConvert.ToDecimal(CurrentUnitDefRun["REF_VALUE"]);
                if (RefVal == decimal.MinValue)
                {
                    Category.SetCheckParameter("Unit_Default_Maximum_NOx_Rate", -1, eParameterDataType.Decimal);
                    Category.CheckCatalogResult = "A";
                }
                else
                    if (RefVal < 0)
                    {
                        Category.SetCheckParameter("Unit_Default_Maximum_NOx_Rate", -1, eParameterDataType.Decimal);
                        Category.CheckCatalogResult = "B";
                    }
                    else
                    {
                        decimal UnitSumRefVal = Convert.ToDecimal(Category.GetCheckParameter("Unit_Default_Level_Sum_Reference_Value").ParameterValue);
                        UnitSumRefVal += RefVal;
                        Category.SetCheckParameter("Unit_Default_Level_Sum_Reference_Value", UnitSumRefVal, eParameterDataType.Decimal);
                        if (cDBConvert.ToInteger(CurrentUnitDefRun["RUN_USED_IND"]) == 1)
                        {
                            decimal FlaggedLvlSumRefVal = Convert.ToDecimal(Category.GetCheckParameter("Unit_Default_Flagged_Level_Sum_Reference_Value").ParameterValue);
                            FlaggedLvlSumRefVal += RefVal;
                            Category.SetCheckParameter("Unit_Default_Flagged_Level_Sum_Reference_Value", FlaggedLvlSumRefVal, eParameterDataType.Decimal);
                        }
                    }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "UNITDEF21");
            }

            return ReturnVal;
        }

        public static string UNITDEF22(cCategory Category, ref bool Log)
        //Out of Sequence or Missing Runs
        {
            string ReturnVal = "";

            try
            {
                if (Convert.ToBoolean(Category.GetCheckParameter("Unit_Default_Run_Times_Valid").ParameterValue))
                {
                    if (Convert.ToBoolean(Category.GetCheckParameter("Unit_Default_Run_Sequence_Valid").ParameterValue))
                    {
                        if (Convert.ToBoolean(Category.GetCheckParameter("Unit_Default_Run_Sequence_Consecutive").ParameterValue))
                        {
                            string RunSequ = Convert.ToString(Category.GetCheckParameter("Unit_Default_Run_Sequence").ParameterValue);
                            if (RunSequ.ListItem(0) != "1")
                                Category.CheckCatalogResult = "A";
                            else
                            {
                                for( int i = 1; i < RunSequ.ListCount(); i++ )
                                    if( Convert.ToInt16( RunSequ.ListItem( i ) ) - Convert.ToInt16( RunSequ.ListItem(i - 1 ) ) != 1 )
                                    {
                                        Category.CheckCatalogResult = "A";
                                        break;
                                    }
                            }
                        }
                    }
                    else
                        Category.CheckCatalogResult = "A";
                }
                else
                    Log = false;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "UNITDEF22");
            }

            return ReturnVal;
        }

        public static string UNITDEF23(cCategory Category, ref bool Log)
        //Insufficient Number of Runs for Highest Operating Level
        {
            string ReturnVal = "";

            try
            {
                if (Category.GetCheckParameter("Unit_Default_Last_Op_Level").ParameterValue == null)
                    Category.SetCheckParameter("Calculate_Unit_Default_NOx_Rate", false, eParameterDataType.Boolean);
                else
                {
                    Category.SetCheckParameter("Calculate_Unit_Default_NOx_Rate", true, eParameterDataType.Boolean);
                    int LvlRunCt = Convert.ToInt16(Category.GetCheckParameter("Unit_Default_Level_Run_Count").ParameterValue);
                    if (LvlRunCt < 3)
                    {
                        Category.SetCheckParameter("Calculate_Unit_Default_NOx_Rate", false, eParameterDataType.Boolean);
                        Category.CheckCatalogResult = "A";
                    }
                    else
                        if (Convert.ToDecimal(Category.GetCheckParameter("Unit_Default_Maximum_NOx_Rate").ParameterValue) < 0)
                            Category.SetCheckParameter("Calculate_Unit_Default_NOx_Rate", false, eParameterDataType.Boolean);
                        else
                        {
                            decimal LvlSumRefVal = Convert.ToDecimal(Category.GetCheckParameter("Unit_Default_Level_Sum_Reference_Value").ParameterValue);
                            if (LvlSumRefVal >= 0)
                            {
                                decimal tempRate = LvlSumRefVal / LvlRunCt;
                                tempRate = Math.Round(tempRate, 3, MidpointRounding.AwayFromZero);
                                if (tempRate > Convert.ToDecimal(Category.GetCheckParameter("Unit_Default_Maximum_NOx_Rate").ParameterValue))
                                    Category.SetCheckParameter("Unit_Default_Maximum_NOx_Rate", tempRate, eParameterDataType.Decimal);
                            }
                            else
                                Category.SetCheckParameter("Unit_Default_Maximum_NOx_Rate", -1, eParameterDataType.Decimal);
                            decimal FlaggedLvlSumRefVal = Convert.ToDecimal(Category.GetCheckParameter("Unit_Default_Flagged_Level_Sum_Reference_Value").ParameterValue);
                            if (FlaggedLvlSumRefVal >= 0)
                                Category.SetCheckParameter("Unit_Default_Flagged_NOx_Rate", Math.Round(FlaggedLvlSumRefVal / LvlRunCt, 3, MidpointRounding.AwayFromZero), eParameterDataType.Decimal);
                        }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "UNITDEF23");
            }

            return ReturnVal;
        }

        public static string UNITDEF24(cCategory Category, ref bool Log)
        //Unit Default Test Run Used Indicators Consistent with Default NOx Rate
        {
            string ReturnVal = "";

            try
            {
                string FlaggedOpLvl = Convert.ToString(Category.GetCheckParameter("Unit_Default_Flagged_Op_Level").ParameterValue);
                if (FlaggedOpLvl == "")
                    Category.CheckCatalogResult = "A";
                else
                {
                    if( FlaggedOpLvl.ListCount() > 1 )
                        Category.CheckCatalogResult = "B";
                    else
                        if (!Convert.ToBoolean(Category.GetCheckParameter("Unit_Default_Run_Used_Indicators_Consistent").ParameterValue))
                            Category.CheckCatalogResult = "C";
                        else
                            if (Convert.ToBoolean(Category.GetCheckParameter("Calculate_Unit_Default_NOx_Rate").ParameterValue))
                            {
                                decimal FlaggedNOxRate = Convert.ToDecimal(Category.GetCheckParameter("Unit_Default_Flagged_NOx_Rate").ParameterValue);
                                decimal MaxNOxRate = Convert.ToDecimal(Category.GetCheckParameter("Unit_Default_Maximum_NOx_Rate").ParameterValue);
                                DataView TolerancesTable = (DataView)Category.GetCheckParameter("Test_Tolerances_Cross_Check_Table").ParameterValue;
                                string OldFilter = TolerancesTable.RowFilter;
                                TolerancesTable.RowFilter = AddToDataViewFilter(OldFilter, "FieldDescription = 'MeanReferenceValue'");
                                decimal Tolerance = cDBConvert.ToDecimal(TolerancesTable[0]["Tolerance"]);
                                TolerancesTable.RowFilter = OldFilter;
                                if (Math.Abs(FlaggedNOxRate - MaxNOxRate) > Tolerance)
                                    Category.CheckCatalogResult = "D";
                            }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "UNITDEF24");
            }

            return ReturnVal;
        }

        public static string UNITDEF25(cCategory Category, ref bool Log)
        //Unit Default Test NOx Rate Consistent with Recalculated Value
        {
            string ReturnVal = "";

            try
            {
                if (Convert.ToBoolean(Category.GetCheckParameter("Calculate_Unit_Default_NOx_Rate").ParameterValue))
                {
                    DataRowView CurrentUnitDefTest = (DataRowView)Category.GetCheckParameter("Current_Unit_Default_Test").ParameterValue;
                    decimal NOxDefRate = cDBConvert.ToDecimal(CurrentUnitDefTest["NOX_DEFAULT_RATE"]);
                    decimal MaxNOxRate = Convert.ToDecimal(Category.GetCheckParameter("Unit_Default_Maximum_NOx_Rate").ParameterValue);
                    if (NOxDefRate >= 0 && NOxDefRate != MaxNOxRate)
                    {
                        DataView TolerancesTable = (DataView)Category.GetCheckParameter("Test_Tolerances_Cross_Check_Table").ParameterValue;
                        string OldFilter = TolerancesTable.RowFilter;
                        TolerancesTable.RowFilter = AddToDataViewFilter(OldFilter, "FieldDescription = 'MeanReferenceValue'");
                        decimal Tolerance = cDBConvert.ToDecimal(TolerancesTable[0]["Tolerance"]);
                        TolerancesTable.RowFilter = OldFilter;
                        if (Math.Abs(NOxDefRate - MaxNOxRate) > Tolerance)
                            Category.CheckCatalogResult = "A";
                        else
                            Category.SetCheckParameter("Unit_Default_Maximum_NOx_Rate", NOxDefRate, eParameterDataType.Decimal);
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "UNITDEF25");
            }

            return ReturnVal;
        }

        public static string UNITDEF26(cCategory Category, ref bool Log)
        //Unit Default Test NOx Rate Consistent with Default Value
        {
            string ReturnVal = "";

            try
            {
                if (Convert.ToBoolean(Category.GetCheckParameter("Test_End_Date_Valid").ParameterValue) &&
                    Convert.ToBoolean(Category.GetCheckParameter("Test_End_Hour_Valid").ParameterValue))
                {
                    if (Convert.ToBoolean(Category.GetCheckParameter("Calculate_Unit_Default_NOx_Rate").ParameterValue))
                    {
                        decimal tempVal = decimal.MinValue;
                        DataRowView CurrentUnitDefTest = (DataRowView)Category.GetCheckParameter("Current_Unit_Default_Test").ParameterValue;
                        string OpCondCd = cDBConvert.ToString(CurrentUnitDefTest["OPERATING_CONDITION_CD"]);
                        string FuelCd = cDBConvert.ToString(CurrentUnitDefTest["FUEL_CD"]);

                        DateTime EndDate = cDBConvert.ToDate(CurrentUnitDefTest["END_DATE"], DateTypes.END);
                        string CheckDate = EndDate.ToShortDateString();
                        string CheckBeginDate = EndDate.AddDays(-90).ToShortDateString();
                        string CheckEndDate = EndDate.AddDays(90).ToShortDateString();
                        int EndHour = cDBConvert.ToHour(CurrentUnitDefTest["END_HOUR"], DateTypes.END);
                        DataView MonDefRecs = (DataView)Category.GetCheckParameter("Default_Records").ParameterValue;
                        string OldFilter = MonDefRecs.RowFilter;
                        string NewFilter = "PARAMETER_CD = 'NOXR' AND DEFAULT_PURPOSE_CD = 'LM' AND DEFAULT_SOURCE_CD = 'TEST' AND FUEL_CD = '" +
                            FuelCd + "' AND BEGIN_DATE >= '" + CheckBeginDate  + "' AND BEGIN_DATE <= '" + CheckEndDate + "'";
                        MonDefRecs.Sort = "BEGIN_DATE DESC, BEGIN_HOUR DESC";

                        switch (OpCondCd)
                        {
                          case (""):
                            {
                                  MonDefRecs.RowFilter = AddToDataViewFilter(NewFilter, "OPERATING_CONDITION_CD <> 'B' AND OPERATING_CONDITION_CD <> 'P'");
                                  break;
                              }
                          case ("A"):
                              {
                                  MonDefRecs.RowFilter = AddToDataViewFilter(NewFilter, "OPERATING_CONDITION_CD = 'B'");
                                  break;
                              }
                          case ("B"):
                              {
                                  goto case "A";
                              }
                          case ("P"):
                              {
                                  MonDefRecs.RowFilter = AddToDataViewFilter(NewFilter, "OPERATING_CONDITION_CD = 'P'");
                                  break;
                              }
                          default:
                              break;
                        }
                        if (MonDefRecs.Count == 0)
                        {
                          NewFilter = "PARAMETER_CD = 'NOXR' AND DEFAULT_PURPOSE_CD = 'LM' AND DEFAULT_SOURCE_CD = 'TEST' AND FUEL_CD = '" +
                              FuelCd + "' AND BEGIN_DATE < '" + CheckDate + "'";
                          MonDefRecs.Sort = "BEGIN_DATE DESC, BEGIN_HOUR DESC";

                          switch (OpCondCd)
                          {
                            case (""):
                              {
                                MonDefRecs.RowFilter = AddToDataViewFilter(NewFilter, "OPERATING_CONDITION_CD <> 'B' AND OPERATING_CONDITION_CD <> 'P'");
                                break;
                              }
                            case ("A"):
                              {
                                MonDefRecs.RowFilter = AddToDataViewFilter(NewFilter, "OPERATING_CONDITION_CD = 'B'");
                                break;
                              }
                            case ("B"):
                              {
                                goto case "A";
                              }
                            case ("P"):
                              {
                                MonDefRecs.RowFilter = AddToDataViewFilter(NewFilter, "OPERATING_CONDITION_CD = 'P'");
                                break;
                              }
                            default:
                              break;
                          }
                        }
                        if (MonDefRecs.Count > 0)
                          tempVal = cDBConvert.ToDecimal(MonDefRecs[0]["DEFAULT_VALUE"]);
                        MonDefRecs.RowFilter = OldFilter;
                        if (tempVal == decimal.MinValue)
                            Category.CheckCatalogResult = "A";
                        else
                        {
                            decimal MaxRate = Convert.ToDecimal(Category.GetCheckParameter("Unit_Default_Maximum_NOx_Rate").ParameterValue);
                            if (CurrentUnitDefTest["GROUP_ID"] == DBNull.Value)
                            {
                                if (MaxRate > (decimal)0.15)
                                {
                                    if (tempVal != MaxRate)
                                        Category.CheckCatalogResult = "B";
                                }
                                else
                                    if (tempVal != (decimal)0.15 && tempVal != MaxRate)
                                        Category.CheckCatalogResult = "B";
                            }
                            else
                                if (tempVal < MaxRate)
                                    Category.CheckCatalogResult = "B";
                        }
                    }
                }
                else
                    Log = false;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "UNITDEF26");
            }

            return ReturnVal;
        }

        public static string UNITDEF27(cCategory Category, ref bool Log)
        //Unit Default Validation of Base and Peak Load Unit Default Tests
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentUnitDefTest = (DataRowView)Category.GetCheckParameter("Current_Unit_Default_Test").ParameterValue;
                string OpCondCd = cDBConvert.ToString(CurrentUnitDefTest["OPERATING_CONDITION_CD"]);
                if (Convert.ToBoolean(Category.GetCheckParameter("Test_End_Date_Valid").ParameterValue) &&
                    Convert.ToBoolean(Category.GetCheckParameter("Test_End_Hour_Valid").ParameterValue) &&
                    OpCondCd.InList("A,B,P"))
                {
                    DataView UnitTypeRecs = (DataView)Category.GetCheckParameter("Location_Unit_Type_Records").ParameterValue;
                    string OldFilter1 = UnitTypeRecs.RowFilter;
                    string EndDate = cDBConvert.ToDate(CurrentUnitDefTest["END_DATE"], DateTypes.END).ToShortDateString();
                    string BeginDate = cDBConvert.ToDate(CurrentUnitDefTest["BEGIN_DATE"], DateTypes.START).ToShortDateString();
                    UnitTypeRecs.RowFilter = AddToDataViewFilter(OldFilter1, "UNIT_TYPE_CD <> 'CC' AND UNIT_TYPE_CD <> 'CT'" +
                        " AND UNIT_TYPE_CD <> 'ICE'" + " AND UNIT_TYPE_CD <> 'IGC' AND UNIT_TYPE_CD <> 'OT'" +
                        " AND (BEGIN_DATE IS NULL OR BEGIN_DATE < '" + EndDate + "') AND (END_DATE IS NULL OR END_DATE > '" + BeginDate + "')");
                    if (UnitTypeRecs.Count > 0)
                        Category.CheckCatalogResult = "A";
                    else
                    {
                        if (OpCondCd == "A" && Convert.ToBoolean(Category.GetCheckParameter("Calculate_Unit_Default_NOx_Rate").ParameterValue))
                        {
                            string FuelCd = cDBConvert.ToString(CurrentUnitDefTest["FUEL_CD"]);
                            int EndHour = cDBConvert.ToHour(CurrentUnitDefTest["END_HOUR"], DateTypes.END);
                            DataView MonDefRecs = (DataView)Category.GetCheckParameter("Default_Records").ParameterValue;
                            string OldFilter2 = MonDefRecs.RowFilter;
                            MonDefRecs.RowFilter = AddToDataViewFilter(OldFilter2, "PARAMETER_CD = 'NOXR' AND DEFAULT_PURPOSE_CD = 'LM'" +
                                " AND DEFAULT_SOURCE_CD = 'TEST' AND FUEL_CD = '" + FuelCd + "' AND OPERATING_CONDITION_CD = 'P' AND (END_DATE IS NULL OR (END_DATE > '" +
                                EndDate + "' OR (END_DATE = '" + EndDate + "' AND END_HOUR >= " + EndHour + ")))");
                            if (MonDefRecs.Count == 0)
                                Category.CheckCatalogResult = "B";
                            else
                            {
                                decimal tempPeakVal = Math.Round(Convert.ToDecimal(Category.GetCheckParameter("Unit_Default_Maximum_NOx_Rate").ParameterValue) * (decimal)1.15, 3, MidpointRounding.AwayFromZero);
                                decimal DefVal = cDBConvert.ToDecimal(MonDefRecs[0]["DEFAULT_VALUE"]);
                                if (tempPeakVal != DefVal)
                                {
                                    DataView TolerancesTable = (DataView)Category.GetCheckParameter("Test_Tolerances_Cross_Check_Table").ParameterValue;
                                    string OldFilter3 = TolerancesTable.RowFilter;
                                    TolerancesTable.RowFilter = AddToDataViewFilter(OldFilter3, "FieldDescription = 'MeanReferenceValue'");
                                    decimal Tolerance = cDBConvert.ToDecimal(TolerancesTable[0]["Tolerance"]);
                                    TolerancesTable.RowFilter = OldFilter3;
                                    if (Math.Abs(tempPeakVal - DefVal) > Tolerance)
                                    {
                                        if (CurrentUnitDefTest["GROUP_ID"] == DBNull.Value)
                                        {
                                            if (tempPeakVal > (decimal)0.15)
                                                Category.CheckCatalogResult = "C";
                                            else
                                                if (DefVal != (decimal)0.15)
                                                    Category.CheckCatalogResult = "C";
                                        }
                                        else
                                            if (DefVal < tempPeakVal)
                                                Category.CheckCatalogResult = "C";
                                    }
                                }
                            }
                            MonDefRecs.RowFilter = OldFilter2;
                        }
                    }
                    UnitTypeRecs.RowFilter = OldFilter1;
                }
                else
                    Log = false;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "UNITDEF27");
            }

            return ReturnVal;
        }

        public static string UNITDEF28(cCategory Category, ref bool Log)
        //Duplicate Unit Default Test
        {
            string ReturnVal = "";

            try
            {
                if (Convert.ToBoolean(Category.GetCheckParameter("Test_Number_Valid").ParameterValue))
                {
                    DataRowView CurrentUnitDefTest = (DataRowView)Category.GetCheckParameter("Current_Unit_Default_Test").ParameterValue;
                    string TestNum = cDBConvert.ToString(CurrentUnitDefTest["TEST_NUM"]);
                    DataView TestRecs = (DataView)Category.GetCheckParameter("Location_Test_Records").ParameterValue;
                    string OldFilter = TestRecs.RowFilter;
                    TestRecs.RowFilter = AddToDataViewFilter(OldFilter, "TEST_TYPE_CD = 'UNITDEF' AND TEST_NUM = '" + TestNum + "'");
                    if ((TestRecs.Count > 0 && CurrentUnitDefTest["TEST_SUM_ID"] == DBNull.Value) ||
                        (TestRecs.Count > 1 && CurrentUnitDefTest["TEST_SUM_ID"] != DBNull.Value) ||
                        (TestRecs.Count == 1 && CurrentUnitDefTest["TEST_SUM_ID"] != DBNull.Value && CurrentUnitDefTest["TEST_SUM_ID"].ToString() != TestRecs[0]["TEST_SUM_ID"].ToString()))
                    {
                        Category.SetCheckParameter("Duplicate_Unit_Default_Test", true, eParameterDataType.Boolean);
                        Category.CheckCatalogResult = "A";
                    }
                    else
                    {
                      string TestSumID = cDBConvert.ToString(CurrentUnitDefTest["TEST_SUM_ID"]);
                      if (TestSumID != "")
                      {
                        DataView QASuppRecords = (DataView)Category.GetCheckParameter("QA_Supplemental_Data_Records").ParameterValue;
                        string OldFilter2 = QASuppRecords.RowFilter;
                        QASuppRecords.RowFilter = AddToDataViewFilter(OldFilter2, "TEST_NUM = '" + TestNum + "' AND TEST_TYPE_CD = 'UNITDEF'");
                        if (QASuppRecords.Count > 0 && cDBConvert.ToString(QASuppRecords[0]["TEST_SUM_ID"]) != TestSumID)
                        {
                          Category.SetCheckParameter("Duplicate_Unit_Default_Test", true, eParameterDataType.Boolean);
                          Category.CheckCatalogResult = "B";
                        }
                        else
                          Category.SetCheckParameter("Duplicate_Unit_Default_Test", false, eParameterDataType.Boolean);
                        QASuppRecords.RowFilter = OldFilter2;
                      }
                      else
                        Category.SetCheckParameter("Duplicate_Unit_Default_Test", false, eParameterDataType.Boolean);
                    }
                    TestRecs.RowFilter = OldFilter;
                }
                else
                    Log = false;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "UNITDEF28");
            }

            return ReturnVal;
        }

        public static string UNITDEF29(cCategory Category, ref bool Log)
        //Duplicate Unit Default Test Run
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentUnitDefRun = (DataRowView)Category.GetCheckParameter("Current_Unit_Default_Run").ParameterValue;
                int RunNum = cDBConvert.ToInteger(CurrentUnitDefRun["RUN_NUM"]);
                int OpLvl = cDBConvert.ToInteger(CurrentUnitDefRun["OP_LEVEL_NUM"]);
                if (RunNum != int.MinValue && OpLvl != int.MinValue)
                {
                    DataView RunRecs = (DataView)Category.GetCheckParameter("Unit_Default_Run_Records").ParameterValue;
                    string OldFilter = RunRecs.RowFilter;
                    RunRecs.RowFilter = AddToDataViewFilter(OldFilter, "RUN_NUM = " + RunNum + " AND OP_LEVEL_NUM = " + OpLvl);
                    if ((RunRecs.Count > 0 && CurrentUnitDefRun["UNIT_DEFAULT_TEST_RUN_ID"] == DBNull.Value) ||
                        (RunRecs.Count > 1 && CurrentUnitDefRun["UNIT_DEFAULT_TEST_RUN_ID"] != DBNull.Value) ||
                        (RunRecs.Count == 1 && CurrentUnitDefRun["UNIT_DEFAULT_TEST_RUN_ID"] != DBNull.Value && CurrentUnitDefRun["UNIT_DEFAULT_TEST_RUN_ID"].ToString() != RunRecs[0]["UNIT_DEFAULT_TEST_RUN_ID"].ToString()))
                    {
                        Category.SetCheckParameter("Duplicate_Unit_Default_Test", true, eParameterDataType.Boolean);
                        Category.CheckCatalogResult = "A";
                    }
                    else
                        Category.SetCheckParameter("Duplicate_Unit_Default_Test", false, eParameterDataType.Boolean);
                    RunRecs.RowFilter = OldFilter;
                }
                else
                    Log = false;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "UNITDEF29");
            }

            return ReturnVal;
        }


        public static string UNITDEF30(cCategory Category, ref bool Log)
        //Calculate Unit Default NOx Rate
        {
            string ReturnVal = "";

            try
            {
                decimal NOxRate = decimal.MinValue;
                int tmpLvl = int.MaxValue;
                DataView RunRecs = (DataView)Category.GetCheckParameter("Unit_Default_Run_Records").ParameterValue;
                RunRecs.Sort = "OP_LEVEL_NUM";
                int OpLvl, tmpCt = int.MinValue;
                decimal tmpRV = decimal.MinValue, RefVal, tmpRate;
                foreach (DataRowView drv in RunRecs)
                {
                    OpLvl = cDBConvert.ToInteger(drv["OP_LEVEL_NUM"]);
                    RefVal = cDBConvert.ToDecimal(drv["REF_VALUE"]);
                    if (OpLvl <= 0 || RefVal < 0)
                    {
                        Category.CheckCatalogResult = "A";
                        break;
                    }
                    else
                    {
                        if (OpLvl != tmpLvl)
                        {
                            if (tmpLvl != int.MaxValue)
                                if (tmpCt < 3)
                                {
                                    Category.CheckCatalogResult = "B";
                                    break;
                                }
                                else
                                {
                                    tmpRate = Math.Round(tmpRV / tmpCt, 3, MidpointRounding.AwayFromZero);
                                    if (tmpRate > NOxRate)
                                        NOxRate = tmpRate;
                                }
                            tmpCt = 0;
                            tmpRV = 0;
                            tmpLvl = OpLvl;
                        }
                        tmpCt++;
                        tmpRV += RefVal;
                    }
                }
                if (string.IsNullOrEmpty(Category.CheckCatalogResult))
                    if (tmpLvl == int.MaxValue || tmpCt < 3)
                        Category.CheckCatalogResult = "B";
                    else
                    {
                        tmpRate = Math.Round(tmpRV / tmpCt, 3, MidpointRounding.AwayFromZero);
                        if (tmpRate > NOxRate)
                            NOxRate = tmpRate;

                        if (NOxRate == decimal.MinValue)
                            Category.SetCheckParameter("Unit_Default_Test_NOx_Rate", null, eParameterDataType.Decimal);
                        else
                            Category.SetCheckParameter("Unit_Default_Test_NOx_Rate", NOxRate, eParameterDataType.Decimal);
                    }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "UNITDEF30");
            }

            return ReturnVal;
        }

        #endregion
    }
}
