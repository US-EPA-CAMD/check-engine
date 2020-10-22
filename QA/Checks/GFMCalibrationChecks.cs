using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Extensions;


namespace ECMPS.Checks.GFMCalibrationChecks
{
    public class cGFMCalibrationChecks : cChecks
    {
        #region Constructors

        public cGFMCalibrationChecks()
        {
            CheckProcedures = new dCheckProcedure[21];

            CheckProcedures[1] = new dCheckProcedure(GFMCAL1);
            CheckProcedures[2] = new dCheckProcedure(GFMCAL2);
            CheckProcedures[3] = new dCheckProcedure(GFMCAL3);
            CheckProcedures[4] = new dCheckProcedure(GFMCAL4);
            CheckProcedures[5] = new dCheckProcedure(GFMCAL5);
            CheckProcedures[6] = new dCheckProcedure(GFMCAL6);
            CheckProcedures[7] = new dCheckProcedure(GFMCAL7);
            CheckProcedures[8] = new dCheckProcedure(GFMCAL8);
            CheckProcedures[9] = new dCheckProcedure(GFMCAL9);
            CheckProcedures[10] = new dCheckProcedure(GFMCAL10);
            CheckProcedures[11] = new dCheckProcedure(GFMCAL11);
            CheckProcedures[12] = new dCheckProcedure(GFMCAL12);
            CheckProcedures[13] = new dCheckProcedure(GFMCAL13);
            CheckProcedures[14] = new dCheckProcedure(GFMCAL14);
            CheckProcedures[15] = new dCheckProcedure(GFMCAL15);
            CheckProcedures[16] = new dCheckProcedure(GFMCAL16);
            CheckProcedures[17] = new dCheckProcedure(GFMCAL17);
            CheckProcedures[18] = new dCheckProcedure(GFMCAL18);
            CheckProcedures[19] = new dCheckProcedure(GFMCAL19);
            CheckProcedures[20] = new dCheckProcedure(GFMCAL20);
        }


        #endregion

        #region GFMCalibrationChecks Checks

        public static string GFMCAL1(cCategory Category, ref bool Log)
        //Initialize GFM Calibration Test Variables
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("GFM_Cal_Calc_Test_Result", null, eParameterDataType.String);
                Category.SetCheckParameter("GFM_Cal_Level_Count", 0, eParameterDataType.Integer);
                Category.SetCheckParameter("GFM_Cal_Levels_Valid", true, eParameterDataType.Boolean);
                Category.SetCheckParameter("GFM_Cal_Sum_Cal_Factor", 0m, eParameterDataType.Decimal);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "GFMCAL1");
            }

            return ReturnVal;
        }

        public static string GFMCAL2(cCategory Category, ref bool Log)
        //GFM Cal Test Component Type Valid  
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentGFMTest = Category.GetCheckParameter("Current_GFM_Calibration_Test").ValueAsDataRowView();
                if (CurrentGFMTest["COMPONENT_ID"] == DBNull.Value)
                {
                    Category.SetCheckParameter("GFM_Cal_Component_Valid", false, eParameterDataType.Boolean);
                    Category.CheckCatalogResult = "A";
                }
                else
                    if (cDBConvert.ToString(CurrentGFMTest["COMPONENT_TYPE_CD"]) == "GFM")
                        Category.SetCheckParameter("GFM_Cal_Component_Valid", true, eParameterDataType.Boolean);
                    else
                    {
                        Category.SetCheckParameter("GFM_Cal_Component_Valid", false, eParameterDataType.Boolean);
                        Category.CheckCatalogResult = "B";
                    }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "GFMCAL2");
            }

            return ReturnVal;
        }

        public static string GFMCAL3(cCategory Category, ref bool Log)
        //Identification of Previously Reported Test or Test Number for GFM Cal Test
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentGFMTest = Category.GetCheckParameter("Current_GFM_Calibration_Test").ValueAsDataRowView();
                string CompId = cDBConvert.ToString(CurrentGFMTest["COMPONENT_ID"]);
                if (CompId != "" && Category.GetCheckParameter("Test_End_Date_Valid").ValueAsBool() &&
                    Category.GetCheckParameter("Test_End_Hour_Valid").ValueAsBool() && Category.GetCheckParameter("Test_End_Minute_Valid").ValueAsBool())
                {
                    Category.SetCheckParameter("Extra_GFM_Cal_Test", false, eParameterDataType.Boolean);

                    DateTime EndDate = cDBConvert.ToDate(CurrentGFMTest["END_DATE"], DateTypes.END);
                    int EndHour = cDBConvert.ToHour(CurrentGFMTest["END_HOUR"], DateTypes.END);
                    int EndMin = cDBConvert.ToInteger(CurrentGFMTest["END_MIN"]);
                    DataView GFMTestRecs = Category.GetCheckParameter("GFM_Cal_Test_Records").ValueAsDataView();
                    DataView GFMTestRecsFound;
                    sFilterPair[] Filter = new sFilterPair[3];
                    Filter[0].Set("END_DATE", EndDate, eFilterDataType.DateEnded);
                    Filter[1].Set("END_HOUR", EndHour, eFilterDataType.Integer);
                    Filter[2].Set("END_MIN", EndMin, eFilterDataType.Integer);
                    GFMTestRecsFound = FindRows(GFMTestRecs, Filter);                    
                    if ((GFMTestRecsFound.Count > 0 && CurrentGFMTest["TEST_SUM_ID"] == DBNull.Value) ||
                        (GFMTestRecsFound.Count > 1 && CurrentGFMTest["TEST_SUM_ID"] != DBNull.Value) ||
                        (GFMTestRecsFound.Count == 1 && CurrentGFMTest["TEST_SUM_ID"] != DBNull.Value && cDBConvert.ToString(CurrentGFMTest["TEST_SUM_ID"]) != cDBConvert.ToString(GFMTestRecsFound[0]["TEST_SUM_ID"])))
                    {
                        Category.SetCheckParameter("Extra_GFM_Cal_Test", true, eParameterDataType.Boolean);
                        Category.CheckCatalogResult = "A";
                    }
                    else
                    {
                        DataView QASuppRecs = Category.GetCheckParameter("QA_Supplemental_Data_Records").ValueAsDataView();
                        string TestNum = cDBConvert.ToString(CurrentGFMTest["TEST_NUM"]);
                        Filter = new sFilterPair[6];
                        Filter[0].Set("END_DATE", EndDate, eFilterDataType.DateEnded);
                        Filter[1].Set("END_HOUR", EndHour, eFilterDataType.Integer);
                        Filter[2].Set("END_MIN", EndMin, eFilterDataType.Integer);
                        Filter[3].Set("TEST_TYPE_CD", "GFMCAL");
                        Filter[4].Set("COMPONENT_ID", CompId);
                        Filter[5].Set("TEST_NUM", TestNum, true);
                        DataView QASuppRecsFound = FindRows(QASuppRecs, Filter);
                        if ((QASuppRecsFound.Count > 0 && CurrentGFMTest["TEST_SUM_ID"] == DBNull.Value) ||
                            (QASuppRecsFound.Count > 1 && CurrentGFMTest["TEST_SUM_ID"] != DBNull.Value) ||
                            (QASuppRecsFound.Count == 1 && CurrentGFMTest["TEST_SUM_ID"] != DBNull.Value && CurrentGFMTest["TEST_SUM_ID"].ToString() != QASuppRecsFound[0]["TEST_SUM_ID"].ToString()))
                        {
                            Category.SetCheckParameter("Extra_GFM_Cal_Test", true, eParameterDataType.Boolean);
                            Category.CheckCatalogResult = "A";
                        }
                        else
                        {
                            Filter = new sFilterPair[2];
                            Filter[0].Set("TEST_TYPE_CD", "GFMCAL");
                            Filter[1].Set("TEST_NUM", TestNum);
                            QASuppRecsFound = FindRows(QASuppRecs, Filter);
                            if ((QASuppRecsFound.Count > 0 && CurrentGFMTest["TEST_SUM_ID"] == DBNull.Value) ||
                                (QASuppRecsFound.Count > 1 && CurrentGFMTest["TEST_SUM_ID"] != DBNull.Value) ||
                                (QASuppRecsFound.Count == 1 && CurrentGFMTest["TEST_SUM_ID"] != DBNull.Value && CurrentGFMTest["TEST_SUM_ID"].ToString() != QASuppRecsFound[0]["TEST_SUM_ID"].ToString()))
                                if (cDBConvert.ToString(QASuppRecsFound[0]["CAN_SUBMIT"]) == "N")
                                {
                                    Filter = new sFilterPair[4];
                                    Filter[0].Set("END_DATE", EndDate, eFilterDataType.DateEnded, true);
                                    Filter[1].Set("END_HOUR", EndHour, eFilterDataType.Integer, true);
                                    Filter[2].Set("END_MIN", EndMin, eFilterDataType.Integer, true);
                                    Filter[3].Set("COMPONENT_ID", CompId, true);
                                    QASuppRecsFound = FindRows(QASuppRecs, Filter);
                                    if ((QASuppRecsFound.Count > 0 && CurrentGFMTest["TEST_SUM_ID"] == DBNull.Value) ||
                                        (QASuppRecsFound.Count > 1 && CurrentGFMTest["TEST_SUM_ID"] != DBNull.Value) ||
                                        (QASuppRecsFound.Count == 1 && CurrentGFMTest["TEST_SUM_ID"] != DBNull.Value && CurrentGFMTest["TEST_SUM_ID"].ToString() != QASuppRecsFound[0]["TEST_SUM_ID"].ToString()))
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
                ReturnVal = Category.CheckEngine.FormatError(ex, "GFMCAL3");
            }

            return ReturnVal;
        }

        public static string GFMCAL4(cCategory Category, ref bool Log)
        //GFM Cal Test Reason Code Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentGFMTest = Category.GetCheckParameter("Current_GFM_Calibration_Test").ValueAsDataRowView();
                string TestReasCd = cDBConvert.ToString(CurrentGFMTest["TEST_REASON_CD"]);
                DataView TestReasonCodes = (DataView)Category.GetCheckParameter("Test_Reason_Code_Lookup_Table").ParameterValue;
                sFilterPair[] Filter = new sFilterPair[1];
                Filter[0].Set("TEST_REASON_CD", TestReasCd);
                if (CountRows(TestReasonCodes, Filter) == 0)
                    Category.CheckCatalogResult = "A";
                else
                {
                    int NumLevels = cDBConvert.ToInteger(CurrentGFMTest["NUM_LEVELS"]);
                    if (NumLevels == 1 && TestReasCd != "QA")
                        Category.CheckCatalogResult = "B";
                    else
                        if (NumLevels == 3 && !TestReasCd.InList("INITIAL,RECERT"))
                            Category.CheckCatalogResult = "C";
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "GFMCAL4");
            }

            return ReturnVal;
        }

        public static string GFMCAL5(cCategory Category, ref bool Log)
        //Reference Calibration Factor Y Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentGFMTest = Category.GetCheckParameter("Current_GFM_Calibration_Test").ValueAsDataRowView();
                int NumLevels = cDBConvert.ToInteger(CurrentGFMTest["NUM_LEVELS"]);
                decimal RefCalFacY = cDBConvert.ToDecimal(CurrentGFMTest["REF_CALIBRATION_Y"]);
                if (NumLevels == 1)
                {
                    if (RefCalFacY == decimal.MinValue)
                        Category.CheckCatalogResult = "A";
                    else
                        if (RefCalFacY < 0)
                            Category.CheckCatalogResult = "B";
                }
                else
                {
                    if (NumLevels == 3)
                        if (RefCalFacY != decimal.MinValue)
                            Category.CheckCatalogResult = "C";
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "GFMCAL5");
            }

            return ReturnVal;
        }

        public static string GFMCAL6(cCategory Category, ref bool Log)
        //Calibration Factor Y Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentGFMTest = Category.GetCheckParameter("Current_GFM_Calibration_Test").ValueAsDataRowView();                
                decimal CalFacY = cDBConvert.ToDecimal(CurrentGFMTest["CALIBRATION_Y"]);
                if (CalFacY == decimal.MinValue)
                    Category.CheckCatalogResult = "A";
                else
                    if (CalFacY < 0)
                        Category.CheckCatalogResult = "B";                
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "GFMCAL6");
            }

            return ReturnVal;
        }

        public static string GFMCAL7(cCategory Category, ref bool Log)
        //Percent Calibration Change Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentGFMTest = Category.GetCheckParameter("Current_GFM_Calibration_Test").ValueAsDataRowView();
                int NumLevels = cDBConvert.ToInteger(CurrentGFMTest["NUM_LEVELS"]);
                decimal PercCalChng = cDBConvert.ToDecimal(CurrentGFMTest["PERCENT_CAL_CHANGE"]);
                if (NumLevels == 1)
                {
                    if (PercCalChng == decimal.MinValue)
                        Category.CheckCatalogResult = "A";
                    else
                        if (PercCalChng < 0)
                            Category.CheckCatalogResult = "B";
                }
                else
                {
                    if (NumLevels == 3)
                        if (PercCalChng != decimal.MinValue)
                            Category.CheckCatalogResult = "C";
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "GFMCAL7");
            }

            return ReturnVal;
        }

        public static string GFMCAL8(cCategory Category, ref bool Log)
        //Concurrent GFM Cal Tests
        {
            string ReturnVal = "";

            try
            {
                if (Category.GetCheckParameter("Test_Dates_Consistent").ValueAsBool())
                {
                    DataRowView CurrentGFMTest = Category.GetCheckParameter("Current_GFM_Calibration_Test").ValueAsDataRowView();
                    DateTime BeginDate = cDBConvert.ToDate(CurrentGFMTest["BEGIN_DATE"], DateTypes.START);
                    int BeginHour = cDBConvert.ToHour(CurrentGFMTest["BEGIN_HOUR"], DateTypes.START);
                    int BeginMin = cDBConvert.ToInteger(CurrentGFMTest["BEGIN_MIN"]);
                    DateTime EndDate = cDBConvert.ToDate(CurrentGFMTest["END_DATE"], DateTypes.END);
                    int EndHour = cDBConvert.ToHour(CurrentGFMTest["END_HOUR"], DateTypes.END);
                    int EndMin = cDBConvert.ToInteger(CurrentGFMTest["END_MIN"]);
                    DataView GFMTestRecs = Category.GetCheckParameter("GFM_Cal_Test_Records").ValueAsDataView();
                    string OldFilterTestRecs = GFMTestRecs.RowFilter;                    
                    GFMTestRecs.RowFilter = AddToDataViewFilter(OldFilterTestRecs, "(BEGIN_DATE < '" + EndDate + "' OR (BEGIN_DATE = '" + EndDate + "' AND BEGIN_HOUR < " +
                        EndHour + " OR (BEGIN_HOUR = " + EndHour + " AND BEGIN_MIN < " + EndMin + "))) AND (END_DATE > '" + BeginDate +
                        "' OR (END_DATE = '" + BeginDate + "' AND END_HOUR > " + BeginHour + " OR (END_HOUR = " + BeginHour +
                        " AND END_MIN > " + BeginMin + ")))");
                    if (GFMTestRecs.Count > 0)
                        Category.CheckCatalogResult = "A";
                    else
                    {
                        DataView QASuppRecs = Category.GetCheckParameter("QA_Supplemental_Data_Records").ValueAsDataView();                        
                        string TestNum = cDBConvert.ToString(CurrentGFMTest["TEST_NUM"]);
                        string CompID = cDBConvert.ToString(CurrentGFMTest["COMPONENT_ID"]);
                        string OldFilterQASupp = QASuppRecs.RowFilter;
                        QASuppRecs.RowFilter = AddToDataViewFilter(OldFilterQASupp, "(BEGIN_DATE < '" + EndDate + "' OR (BEGIN_DATE = '" + EndDate + "' AND BEGIN_HOUR < " + 
                            EndHour + " OR (BEGIN_HOUR = " + EndHour + " AND BEGIN_MIN < " + EndMin + "))) AND (END_DATE > '" + BeginDate + 
                            "' OR (END_DATE = '" + BeginDate + "' AND END_HOUR > " + BeginHour + " OR (END_HOUR = " + BeginHour +
                            " AND END_MIN > " + BeginMin + "))) AND COMPONENT_ID = '" + CompID + "' AND test_sum_id <> '" + cDBConvert.ToString(CurrentGFMTest["TEST_SUM_ID"]) + "' and TEST_TYPE_CD = 'GFMCAL' AND TEST_NUM <> '" + TestNum + "'");                            
                        QASuppRecs.RowFilter = OldFilterQASupp;
                        if (QASuppRecs.Count > 0)
                            Category.CheckCatalogResult = "A";
                        QASuppRecs.RowFilter = OldFilterQASupp;
                    }
                    GFMTestRecs.RowFilter = OldFilterTestRecs;
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "GFMCAL8");
            }

            return ReturnVal;
        }

        public static string GFMCAL9(cCategory Category, ref bool Log)
        //Level Code Valid
        {
            string ReturnVal = "";

            try
            {
                int CalLvlCt = Category.GetCheckParameter("GFM_Cal_Level_Count").ValueAsInt();
                CalLvlCt++;
                Category.SetCheckParameter("GFM_Cal_Level_Count", CalLvlCt, eParameterDataType.Integer);
                Category.SetCheckParameter("GFM_Cal_Level_Code_Valid", true, eParameterDataType.Boolean);
                DataRowView CurrentGFMData = Category.GetCheckParameter("Current_GFM_Calibration_Data").ValueAsDataRowView();
                string LvlCd = cDBConvert.ToString(CurrentGFMData["GAS_LEVEL_CD"]);
                if (LvlCd != "" && !LvlCd.InList("HIGH,MID,LOW"))
                {
                    Category.SetCheckParameter("GFM_Cal_Level_Code_Valid", true, eParameterDataType.Boolean);
                    Category.CheckCatalogResult = "A";
                }
                else
                {
                    DataRowView CurrentGFMTest = Category.GetCheckParameter("Current_GFM_Calibration_Test").ValueAsDataRowView();
                    int NumLevels = cDBConvert.ToInteger(CurrentGFMTest["NUM_LEVELS"]);
                    if (NumLevels == 3)
                    {
                        if (LvlCd == "")
                        {
                            Category.SetCheckParameter("GFM_Cal_Level_Code_Valid", false, eParameterDataType.Boolean);
                            Category.CheckCatalogResult = "A";
                        }
                        else
                            if (CalLvlCt == 1 && LvlCd != "L")
                                Category.SetCheckParameter("GFM_Cal_Level_Code_Valid", false, eParameterDataType.Boolean);
                            else
                                if (CalLvlCt == 2 && LvlCd != "M")
                                    Category.SetCheckParameter("GFM_Cal_Level_Code_Valid", false, eParameterDataType.Boolean);
                                else
                                    if (CalLvlCt == 3 && LvlCd != "H")
                                        Category.SetCheckParameter("GFM_Cal_Level_Code_Valid", false, eParameterDataType.Boolean);
                    }
                    else
                        if (NumLevels == 1)
                            if (LvlCd != "")
                                Category.CheckCatalogResult = "C";
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "GFMCAL9");
            }

            return ReturnVal;
        }

        public static string GFMCAL10(cCategory Category, ref bool Log)
        //Sample Rate Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentGFMData = Category.GetCheckParameter("Current_GFM_Calibration_Data").ValueAsDataRowView();
                decimal SampleRate = cDBConvert.ToDecimal(CurrentGFMData["SAMPLE_RATE"]);
                if (SampleRate == decimal.MinValue)
                    Category.CheckCatalogResult = "A";
                else
                    if (SampleRate <= 0)
                        Category.CheckCatalogResult = "B";     
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "GFMCAL10");
            }

            return ReturnVal;
        }

        public static string GFMCAL11(cCategory Category, ref bool Log)
        //Calibration Factor for Level Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentGFMData = Category.GetCheckParameter("Current_GFM_Calibration_Data").ValueAsDataRowView();
                decimal CalFac = cDBConvert.ToDecimal(CurrentGFMData["CALIBRATION_FACTOR"]);
                if (CalFac == decimal.MinValue)
                {
                    Category.SetCheckParameter("GFM_Cal_Sum_Cal_Factor", -1m, eParameterDataType.Decimal);
                    Category.CheckCatalogResult = "A";
                }
                else
                    if (CalFac < 0)
                    {
                        Category.SetCheckParameter("GFM_Cal_Sum_Cal_Factor", -1m, eParameterDataType.Decimal);
                        Category.CheckCatalogResult = "B";
                    }
                    else
                    {
                        decimal GFMCalSumCalFac = Category.GetCheckParameter("GFM_Cal_Sum_Cal_Factor").ValueAsDecimal();
                        if (GFMCalSumCalFac >= 0)
                        {
                            GFMCalSumCalFac += CalFac;
                            Category.SetCheckParameter("GFM_Cal_Sum_Cal_Factor", GFMCalSumCalFac, eParameterDataType.Decimal);
                            if (GFMCalSumCalFac < (decimal)0.9 || (decimal)1.1 < GFMCalSumCalFac)
                                Category.CheckCatalogResult = "C";
                        }
                    }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "GFMCAL11");
            }

            return ReturnVal;
        }

        public static string GFMCAL12(cCategory Category, ref bool Log)
        //Number Of Levels Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentGFMTest = Category.GetCheckParameter("Current_GFM_Calibration_Test").ValueAsDataRowView();
                int NumLevels = cDBConvert.ToInteger(CurrentGFMTest["NUM_LEVELS"]);

                if (NumLevels == int.MinValue)
                    Category.CheckCatalogResult = "A";
                else
                    if (NumLevels != 1 && NumLevels != 3)
                        Category.CheckCatalogResult = "B";
                    else
                    {
                        int CalLvlCt = Category.GetCheckParameter("GFM_Cal_Level_Count").ValueAsInt();
                        if ((CalLvlCt == 1 || CalLvlCt == 3) && NumLevels != CalLvlCt)
                            Category.CheckCatalogResult = "C";
                    }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "GFMCAL12");
            }

            return ReturnVal;
        }

        public static string GFMCAL13(cCategory Category, ref bool Log)
        //GFM Cal Test Load Levels Valid
        {
            string ReturnVal = "";

            try
            {
                int CalLvlCt = Category.GetCheckParameter("GFM_Cal_Level_Count").ValueAsInt();
                if (CalLvlCt != 1 && CalLvlCt != 3)
                    Category.CheckCatalogResult = "A";
                else
                    if (!Category.GetCheckParameter("GFM_Cal_Levels_Valid").ValueAsBool())
                        Category.CheckCatalogResult = "B";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "GFMCAL13");
            }

            return ReturnVal;
        }

        public static string GFMCAL14(cCategory Category, ref bool Log)
        //Determine Reference Calibration Factor Y
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("GFM_Cal_Reference_Cal_Factor", null, eParameterDataType.Decimal);
                if (Category.GetCheckParameter("GFM_Cal_Level_Count").ValueAsInt() == 1 && Category.GetCheckParameter("Test_End_Date_Valid").ValueAsBool() &&
                    Category.GetCheckParameter("Test_End_Hour_Valid").ValueAsBool() && Category.GetCheckParameter("Test_End_Minute_Valid").ValueAsBool())
                {
                    DataRowView CurrentGFMTest = Category.GetCheckParameter("Current_GFM_Calibration_Test").ValueAsDataRowView();
                    DataView QASuppAttRecs = Category.GetCheckParameter("QA_Supplemental_Attribute_Records").ValueAsDataView();
                    DataView QASuppAttRecsFound;
                    sFilterPair[] Filter = new sFilterPair[4];
                    Filter[0].Set("TEST_TYPE_CD", "GFMCAL");
                    Filter[1].Set("COMPONENT_ID", cDBConvert.ToString(CurrentGFMTest["COMPONENT_ID"]));
                    Filter[2].Set("ATTRIBUTE_NAME", "NUMBER_OF_LEVELS");
                    Filter[3].Set("ATTRIBUTE_VALUE", "3");
                    QASuppAttRecsFound = FindRows(QASuppAttRecs, Filter);
                    if (QASuppAttRecsFound.Count == 0)
                        Category.CheckCatalogResult = "A";
                    else
                    {
                        string OldFilter = QASuppAttRecsFound.RowFilter;
                        DateTime EndDate = cDBConvert.ToDate(CurrentGFMTest["END_DATE"], DateTypes.END);
                        int EndHour = cDBConvert.ToHour(CurrentGFMTest["END_HOUR"], DateTypes.END);
                        int EndMin = cDBConvert.ToInteger(CurrentGFMTest["END_MIN"]);
                        QASuppAttRecsFound.RowFilter = AddToDataViewFilter(OldFilter, "END_DATE < '" + EndDate +
                            "' OR (END_DATE = '" + EndDate + "' AND END_HOUR < " + EndHour + " OR (END_HOUR = " + EndHour +
                            " AND END_MIN < " + EndMin + "))");
                        if (QASuppAttRecsFound.Count == 0)
                            Category.CheckCatalogResult = "A";
                        else
                        {
                            QASuppAttRecsFound.Sort = "BEGIN_DATE DESC, BEGIN_HOUR DESC, BEGIN_MIN DESC";
                            if (cDBConvert.ToString(QASuppAttRecsFound[0]["TEST_RESULT_CD"]) != "PASSED")
                                Category.CheckCatalogResult = "B";
                            else
                            {
                                QASuppAttRecsFound.RowFilter = OldFilter;
                                Filter = new sFilterPair[2];
                                Filter[0].Set("QA_SUPP_DATA_ID", cDBConvert.ToString(QASuppAttRecsFound[0]["QA_SUPP_DATA_ID"]));
                                Filter[1].Set("ATTRIBUTE_NAME", "CALIBRATION_FACTOR_Y");
                                QASuppAttRecsFound = FindRows(QASuppAttRecs, Filter);
                                if (QASuppAttRecsFound.Count > 0)
                                    Category.SetCheckParameter("GFM_Cal_Reference_Cal_Factor", cDBConvert.ToDecimal(QASuppAttRecsFound[0]["ATTRIBUTE_VALUE"]), eParameterDataType.Decimal);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "GFMCAL14");
            }

            return ReturnVal;
        }

        public static string GFMCAL15(cCategory Category, ref bool Log)
        //Calibration Factor Y Consistent with Reported Value
        {
            string ReturnVal = "";

            try
            {
                int LvlCt = Category.GetCheckParameter("GFM_Cal_Level_Count").ValueAsInt();
                decimal CalSumCalFactorY = Category.GetCheckParameter("GFM_Cal_Sum_Cal_Factor").ValueAsDecimal();
                decimal CalCalcFactorY;
                if ((LvlCt == 1 || LvlCt == 3) && CalSumCalFactorY >= 0)
                {
                    CalCalcFactorY = Math.Round(CalSumCalFactorY / LvlCt, 3, MidpointRounding.AwayFromZero);
                    Category.SetCheckParameter("GFM_Cal_Calc_Cal_Factor_Y", CalCalcFactorY, eParameterDataType.Decimal);
                }
                else
                {
                    CalCalcFactorY = decimal.MinValue;//"null"
                    Category.SetCheckParameter("GFM_Cal_Calc_Cal_Factor_Y", null, eParameterDataType.Decimal);
                }
                DataRowView CurrentGFMTest = Category.GetCheckParameter("Current_GFM_Calibration_Test").ValueAsDataRowView();
                decimal RefCalFacY = cDBConvert.ToDecimal(CurrentGFMTest["REF_CALIBRATION_Y"]);
                if (CalCalcFactorY != decimal.MinValue && RefCalFacY >= 0)
                    if (LvlCt == 1)
                    {
                        if (cDBConvert.ToInteger(CurrentGFMTest["NUM_LEVELS"]) == 1 && CalCalcFactorY != RefCalFacY)
                            Category.CheckCatalogResult = "A";
                    }
                    else
                    {
                        DataView TestToleranceRecords = (DataView)Category.GetCheckParameter("Test_Tolerances_Cross_Check_Table").ValueAsDataView();
                        string OldFilter = TestToleranceRecords.RowFilter;
                        TestToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, "TestTypeCode = 'GFMCAL' and FieldDescription = 'CalibrationFactorY'");
                        decimal Tolerance = Convert.ToDecimal(TestToleranceRecords[0]["Tolerance"]);
                        TestToleranceRecords.RowFilter = OldFilter;
                        if (Math.Abs(CalCalcFactorY - RefCalFacY) > Tolerance)
                            Category.CheckCatalogResult = "B";
                    }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "GFMCAL15");
            }

            return ReturnVal;
        }

        public static string GFMCAL16(cCategory Category, ref bool Log)
        //Percent Calibration Change Consistent with Reported Value
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("GFM_Cal_Calc_Percent_Change", null, eParameterDataType.Decimal);
                int CalLvlCt = Category.GetCheckParameter("GFM_Cal_Level_Count").ValueAsInt();
                decimal CalCalcCalFactorY = Category.GetCheckParameter("GFM_Cal_Calc_Cal_Factor_Y").ValueAsDecimal();
                if (CalLvlCt == 1)
                {
                    DataRowView CurrentGFMTest = Category.GetCheckParameter("Current_GFM_Calibration_Test").ValueAsDataRowView();
                    string TestResCd = cDBConvert.ToString(CurrentGFMTest["TEST_RESULT_CD"]);
                    decimal CalRefCalFactorY = Category.GetCheckParameter("GFM_Cal_Reference_Cal_Factor").ValueAsDecimal();
                    decimal PercCalChg = cDBConvert.ToDecimal(CurrentGFMTest["PERCENT_CAL_CHANGE"]);
                    if (CalCalcCalFactorY != decimal.MinValue && CalRefCalFactorY != decimal.MinValue)
                    {
                        decimal CalCalcPercChg = Math.Abs(CalCalcCalFactorY - CalRefCalFactorY) / CalRefCalFactorY * 100;
                        CalCalcPercChg = Math.Round(CalCalcPercChg, 1, MidpointRounding.AwayFromZero);
                        Category.SetCheckParameter("GFM_Cal_Calc_Percent_Change", CalCalcPercChg, eParameterDataType.Decimal);
                        if (CalCalcPercChg <= 5)
                            Category.SetCheckParameter("GFM_Cal_Calc_Test_Result", "PASSED", eParameterDataType.String);
                        else
                        {
                            Category.SetCheckParameter("GFM_Cal_Calc_Test_Result", "FAILED", eParameterDataType.String);
                            if (PercCalChg >= 0 && PercCalChg != CalCalcPercChg)
                            {
                                DataView TestToleranceRecords = (DataView)Category.GetCheckParameter("Test_Tolerances_Cross_Check_Table").ValueAsDataView();
                                string OldFilter = TestToleranceRecords.RowFilter;
                                TestToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, "TestTypeCode = 'GFMCAL' and FieldDescription = 'PercentCalibrationChange'");
                                decimal Tolerance = Convert.ToDecimal(TestToleranceRecords[0]["Tolerance"]);
                                TestToleranceRecords.RowFilter = OldFilter;
                                if (Math.Abs(CalCalcPercChg - PercCalChg) > Tolerance)
                                    Category.CheckCatalogResult = "A";
                                else
                                    if (PercCalChg <= 5)
                                        Category.SetCheckParameter("GFM_Cal_Calc_Test_Result", "PASSED", eParameterDataType.String);
                            }
                        }
                    }
                }
                else
                    if (CalLvlCt == 3)
                        if (CalCalcCalFactorY != decimal.MinValue)
                            Category.SetCheckParameter("GFM_Cal_Calc_Test_Result", "PASSED", eParameterDataType.String);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "GFMCAL16");
            }

            return ReturnVal;
        }

        public static string GFMCAL17(cCategory Category, ref bool Log)
        //Determine GFM Calibration Test Result
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentGFMTest = Category.GetCheckParameter("Current_GFM_Calibration_Test").ValueAsDataRowView();
                int NumLevels = cDBConvert.ToInteger(CurrentGFMTest["NUM_LEVELS"]);
                string TestResCd = cDBConvert.ToString(CurrentGFMTest["TEST_RESULT_CD"]);
                if (NumLevels == 1)
                {
                    if (TestResCd == "")
                        Category.CheckCatalogResult = "A";
                    else
                        if (!TestResCd.InList("PASSED,FAILED"))
                        {
                            DataView TestResCodes = (DataView)Category.GetCheckParameter("Test_Result_Code_Lookup_Table").ValueAsDataView();
                            sFilterPair[] Filter = new sFilterPair[1];
                            Filter[0].Set("TEST_RESULT_CD", TestResCd);
                            if (CountRows(TestResCodes, Filter) == 0)
                                Category.CheckCatalogResult = "B";
                            else
                                Category.CheckCatalogResult = "C";
                        }
                        else
                        {
                            string CalCalcTestResult = Category.GetCheckParameter("GFM_Cal_Calc_Test_Result").ValueAsString();
                            if (CalCalcTestResult == "FAILED" && TestResCd == "PASSED")
                                Category.CheckCatalogResult = "D";
                            else
                                if (CalCalcTestResult == "PASSED" && TestResCd == "FAILED")
                                    Category.CheckCatalogResult = "E";
                        }
                }
                else
                    if (NumLevels == 3)
                        if (TestResCd != "")
                            Category.CheckCatalogResult = "F";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "GFMCAL17");
            }

            return ReturnVal;
        }

        public static string GFMCAL18(cCategory Category, ref bool Log)
        //Component ID Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentGFMTest = Category.GetCheckParameter("Current_GFM_Calibration_Test").ValueAsDataRowView();
                if (CurrentGFMTest["COMPONENT_ID"] == DBNull.Value)
                    Category.CheckCatalogResult = "A";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "GFMCAL18");
            }

            return ReturnVal;
        }

        public static string GFMCAL19(cCategory Category, ref bool Log)
        //Duplicate GFM Cal Test
        {
            string ReturnVal = "";

            try
            {
                if (Category.GetCheckParameter("Test_Number_Valid").ValueAsBool())
                {
                    DataRowView CurrentGFMTest = Category.GetCheckParameter("Current_GFM_Calibration_Test").ValueAsDataRowView();                    
                    DataView LocTestRecs = (DataView)Category.GetCheckParameter("Location_Test_Records").ParameterValue;
                    DataView LocTestRecsFound;
                    sFilterPair[] Filter = new sFilterPair[2];
                    Filter[0].Set("TEST_NUM", cDBConvert.ToString(CurrentGFMTest["TEST_NUM"]));
                    Filter[1].Set("TEST_TYPE_CD", "GFMCAL");
                    LocTestRecsFound = FindRows(LocTestRecs, Filter);
                    if ((LocTestRecsFound.Count > 0 && CurrentGFMTest["TEST_SUM_ID"] == DBNull.Value) ||
                        (LocTestRecsFound.Count > 1 && CurrentGFMTest["TEST_SUM_ID"] != DBNull.Value) ||
                        (LocTestRecsFound.Count == 1 && CurrentGFMTest["TEST_SUM_ID"] != DBNull.Value && CurrentGFMTest["TEST_SUM_ID"].ToString() != LocTestRecsFound[0]["TEST_SUM_ID"].ToString()))                    
                        Category.CheckCatalogResult = "A";
                    else
                    {
                      string TestSumID = cDBConvert.ToString(CurrentGFMTest["TEST_SUM_ID"]);
                      if (TestSumID != "")
                      {
                        DataView QASuppRecords = (DataView)Category.GetCheckParameter("QA_Supplemental_Data_Records").ParameterValue;
                        string OldFilter2 = QASuppRecords.RowFilter;
                        QASuppRecords.RowFilter = AddToDataViewFilter(OldFilter2, "TEST_NUM = '" + cDBConvert.ToString(CurrentGFMTest["TEST_NUM"]) + "' AND TEST_TYPE_CD = 'GFMCAL'");
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
                ReturnVal = Category.CheckEngine.FormatError(ex, "GFMCAL19");
            }

            return ReturnVal;
        }

        public static string GFMCAL20(cCategory Category, ref bool Log)
        //Duplicate GFM Cal Test Run
        {
            string ReturnVal = "";

            try
            {
                if (Category.GetCheckParameter("GFM_Cal_Level_Code_Valid").ValueAsBool())
                {
                    DataRowView CurrentGFMData = Category.GetCheckParameter("Current_GFM_Calibration_Data").ValueAsDataRowView();
                    string LvlCd = cDBConvert.ToString(CurrentGFMData["GAS_LEVEL_CD"]);
                    DataView GFMCalDataRecs = Category.GetCheckParameter("GFM_Cal_Data_Records").ValueAsDataView();
                    DataView GFMCalDataRecsFound;
                    sFilterPair[] Filter = new sFilterPair[1];
                    Filter[0].Set("GAS_LEVEL_CD", LvlCd);
                    GFMCalDataRecsFound = FindRows(GFMCalDataRecs, Filter);
                    if ((GFMCalDataRecsFound.Count > 0 && CurrentGFMData["GFM_CALIBRATION_DATA_ID"] == DBNull.Value) ||
                        (GFMCalDataRecsFound.Count > 1 && CurrentGFMData["GFM_CALIBRATION_DATA_ID"] != DBNull.Value) ||
                        (GFMCalDataRecsFound.Count == 1 && CurrentGFMData["GFM_CALIBRATION_DATA_ID"] != DBNull.Value && cDBConvert.ToString(CurrentGFMData["GFM_CALIBRATION_DATA_ID"]) != cDBConvert.ToString(GFMCalDataRecsFound[0]["GFM_CALIBRATION_DATA_ID"])))                        
                        Category.CheckCatalogResult = "A";
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "GFMCAL20");
            }

            return ReturnVal;
        }

        #endregion
    }
}
