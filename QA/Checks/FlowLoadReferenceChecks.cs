using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Extensions;


namespace ECMPS.Checks.FlowLoadReferenceChecks
{
    public class cFlowLoadReferenceChecks : cChecks
    {
        #region Constructors
        public cFlowLoadReferenceChecks()
        {
            CheckProcedures = new dCheckProcedure[17];
            CheckProcedures[1] = new dCheckProcedure(F2LREF1);
            CheckProcedures[2] = new dCheckProcedure(F2LREF2);
            CheckProcedures[3] = new dCheckProcedure(F2LREF3);
            CheckProcedures[4] = new dCheckProcedure(F2LREF4);
            CheckProcedures[5] = new dCheckProcedure(F2LREF5);
            CheckProcedures[6] = new dCheckProcedure(F2LREF6);
            CheckProcedures[7] = new dCheckProcedure(F2LREF7);
            CheckProcedures[8] = new dCheckProcedure(F2LREF8);
            CheckProcedures[9] = new dCheckProcedure(F2LREF9);
            CheckProcedures[10] = new dCheckProcedure(F2LREF10);
            CheckProcedures[11] = new dCheckProcedure(F2LREF11);
            CheckProcedures[12] = new dCheckProcedure(F2LREF12);
            CheckProcedures[13] = new dCheckProcedure(F2LREF13);
            CheckProcedures[14] = new dCheckProcedure(F2LREF14);
            CheckProcedures[15] = new dCheckProcedure(F2LREF15);
            CheckProcedures[16] = new dCheckProcedure(F2LREF16);
        }

        #endregion


        #region Flow to Load Reference Checks
        public static string F2LREF1(cCategory Category, ref bool Log)
        //System Type Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentFlowToLoadReference = (DataRowView)Category.GetCheckParameter("Current_Flow_to_Load_Reference").ParameterValue;
                if (CurrentFlowToLoadReference["MON_SYS_ID"] == DBNull.Value)
                {
                    Category.SetCheckParameter("Flow_to_Load_Reference_System_Valid", false, eParameterDataType.Boolean);
                    Category.CheckCatalogResult = "A";
                }
                else
                    if (cDBConvert.ToString(CurrentFlowToLoadReference["SYS_TYPE_CD"]) == "FLOW")
                        Category.SetCheckParameter("Flow_to_Load_Reference_System_Valid", true, eParameterDataType.Boolean);
                    else
                    {
                        Category.SetCheckParameter("Flow_to_Load_Reference_System_Valid", false, eParameterDataType.Boolean);
                        Category.CheckCatalogResult = "B";
                    }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "F2LREF1");
            }

            return ReturnVal;
        }

        public static string F2LREF2(cCategory Category, ref bool Log)
        //Identification of Previously Reported Test or Test Number for Flow to Load Reference Data
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentFlowToLoadReference = (DataRowView)Category.GetCheckParameter("Current_Flow_to_Load_Reference").ParameterValue;
                string MonSysID = cDBConvert.ToString(CurrentFlowToLoadReference["MON_SYS_ID"]);

                if (Convert.ToBoolean(Category.GetCheckParameter("Test_End_Date_Valid").ParameterValue) &&
                    Convert.ToBoolean(Category.GetCheckParameter("Test_End_Hour_Valid").ParameterValue) &&
                    Convert.ToBoolean(Category.GetCheckParameter("Test_End_Minute_Valid").ParameterValue) &&
                    MonSysID != "")
                {
                    DateTime EndDate = cDBConvert.ToDate(CurrentFlowToLoadReference["END_DATE"], DateTypes.END);
                    int EndHour = cDBConvert.ToHour(CurrentFlowToLoadReference["END_HOUR"], DateTypes.END);
                    int EndMinute = cDBConvert.ToInteger(CurrentFlowToLoadReference["END_MIN"]);
                    string TestNumber = cDBConvert.ToString(CurrentFlowToLoadReference["TEST_NUM"]);
                    Category.SetCheckParameter("Flow_to_Load_Reference_Supp_Data_ID", null, eParameterDataType.String);
                    decimal RefFlowLoadRatio = cDBConvert.ToDecimal(CurrentFlowToLoadReference["REF_FLOW_LOAD_RATIO"]);
                    DataView FlowToLoadRefRecords = (DataView)Category.GetCheckParameter("Flow_to_Load_Reference_Records").ParameterValue;
                    string OldFlowToLoadRefFilter = FlowToLoadRefRecords.RowFilter;
                    DataView QASuppRecs = (DataView)Category.GetCheckParameter("QA_Supplemental_Data_Records").ParameterValue;
                    DataView QASuppAttRecs = (DataView)Category.GetCheckParameter("QA_Supplemental_Attribute_Records").ParameterValue;
                    string OldQASuppRecsFilter = QASuppRecs.RowFilter;
                    string OldQASuppAttRecFilter = QASuppAttRecs.RowFilter;

                    if (RefFlowLoadRatio != decimal.MinValue)
                    {
                        FlowToLoadRefRecords.RowFilter = AddToDataViewFilter(OldFlowToLoadRefFilter, "END_DATE = '" + EndDate.ToShortDateString() + "' " +
                            "AND END_HOUR = " + EndHour + " AND END_MIN = " + EndMinute + " AND REF_FLOW_LOAD_RATIO IS NOT NULL");
                        if ((FlowToLoadRefRecords.Count > 0 && CurrentFlowToLoadReference["TEST_SUM_ID"] == DBNull.Value) ||
                            (FlowToLoadRefRecords.Count > 1 && CurrentFlowToLoadReference["TEST_SUM_ID"] != DBNull.Value) ||
                            (FlowToLoadRefRecords.Count == 1 && CurrentFlowToLoadReference["TEST_SUM_ID"] != DBNull.Value && CurrentFlowToLoadReference["TEST_SUM_ID"].ToString() != FlowToLoadRefRecords[0]["TEST_SUM_ID"].ToString()))
                            Category.CheckCatalogResult = "A";
                        else
                        {
                            QASuppAttRecs.RowFilter = AddToDataViewFilter(OldQASuppAttRecFilter, "TEST_TYPE_CD = 'F2LREF'" + " AND MON_SYS_ID = '" + MonSysID +
                                "'" + " AND END_DATE = '" + EndDate.ToShortDateString() + "' " + " AND END_HOUR = " + EndHour + " AND END_MIN = " +
                                EndMinute + " AND TEST_NUM <> '" + TestNumber + "' AND ATTRIBUTE_NAME = 'REF_FLOW_LOAD_RATIO'");
                            if ((QASuppAttRecs.Count > 0 && CurrentFlowToLoadReference["TEST_SUM_ID"] == DBNull.Value) ||
                                (QASuppAttRecs.Count > 1 && CurrentFlowToLoadReference["TEST_SUM_ID"] != DBNull.Value) ||
                                (QASuppAttRecs.Count == 1 && CurrentFlowToLoadReference["TEST_SUM_ID"] != DBNull.Value && CurrentFlowToLoadReference["TEST_SUM_ID"].ToString() != QASuppAttRecs[0]["TEST_SUM_ID"].ToString()))
                            {   
                                string FoundQASuppDataId = cDBConvert.ToString(QASuppAttRecs[0]["QA_SUPP_DATA_ID"]);
                                sFilterPair[] Filter = new sFilterPair[2];
                                Filter[0].Set("QA_SUPP_DATA_ID", FoundQASuppDataId);
                                Filter[1].Set("ATTRIBUTE_NAME", "REF_GHR");
                                QASuppAttRecs.RowFilter = OldQASuppAttRecFilter;
                                DataView FoundRecs = FindRows(QASuppAttRecs, Filter);
                                if (FoundRecs.Count == 0)
                                    Category.CheckCatalogResult = "A";
                            }
                            if (string.IsNullOrEmpty(Category.CheckCatalogResult))
                            {
                                QASuppRecs.RowFilter = AddToDataViewFilter(OldQASuppRecsFilter, "TEST_TYPE_CD = 'F2LREF'" + " AND TEST_NUM = '" +
                                    TestNumber + "'");
                                if (QASuppRecs.Count > 0)
                                {
                                    string DataIDRetrieved = cDBConvert.ToString(((DataRowView)QASuppRecs[0])["QA_SUPP_DATA_ID"]);
                                    Category.SetCheckParameter("Flow_to_Load_Reference_Supp_Data_ID", DataIDRetrieved, eParameterDataType.String);
                                    if (cDBConvert.ToString(((DataRowView)QASuppRecs[0])["CAN_SUBMIT"]) == "N")
                                    {
                                        QASuppRecs.RowFilter = AddToDataViewFilter(QASuppRecs.RowFilter, "MON_SYS_ID <> '" + MonSysID + "'" + " OR END_DATE <> '" + EndDate.ToShortDateString() + "' " +
                                            " OR END_HOUR <> " + EndHour + " OR END_MIN <> " + EndMinute);
                                        if ((QASuppRecs.Count > 0 && CurrentFlowToLoadReference["TEST_SUM_ID"] == DBNull.Value) ||
                                            (QASuppRecs.Count > 1 && CurrentFlowToLoadReference["TEST_SUM_ID"] != DBNull.Value) ||
                                            (QASuppRecs.Count == 1 && CurrentFlowToLoadReference["TEST_SUM_ID"] != DBNull.Value && CurrentFlowToLoadReference["TEST_SUM_ID"].ToString() != QASuppRecs[0]["TEST_SUM_ID"].ToString()))
                                            Category.CheckCatalogResult = "B";
                                        else
                                        {
                                            QASuppAttRecs.RowFilter = OldQASuppAttRecFilter;
                                            QASuppAttRecs.RowFilter = AddToDataViewFilter(OldQASuppAttRecFilter, "QA_SUPP_DATA_ID = '" + DataIDRetrieved + "' AND ATTRIBUTE_NAME = 'REF_FLOW_LOAD_RATIO'");
                                            if (QASuppAttRecs.Count > 0)
                                                Category.CheckCatalogResult = "C";
                                            else
                                                Category.CheckCatalogResult = "D";
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                        if (CurrentFlowToLoadReference["REF_GHR"] != DBNull.Value)
                        {
                            FlowToLoadRefRecords.RowFilter = AddToDataViewFilter(OldFlowToLoadRefFilter, "END_DATE = '" + EndDate.ToShortDateString() + "' " +
                                "AND END_HOUR = " + EndHour + " AND END_MIN = " + EndMinute + " AND REF_GHR IS NOT NULL");
                            if ((FlowToLoadRefRecords.Count > 0 && CurrentFlowToLoadReference["TEST_SUM_ID"] == DBNull.Value) ||
                                (FlowToLoadRefRecords.Count > 1 && CurrentFlowToLoadReference["TEST_SUM_ID"] != DBNull.Value) ||
                                (FlowToLoadRefRecords.Count == 1 && CurrentFlowToLoadReference["TEST_SUM_ID"] != DBNull.Value && CurrentFlowToLoadReference["TEST_SUM_ID"].ToString() != FlowToLoadRefRecords[0]["TEST_SUM_ID"].ToString()))
                                Category.CheckCatalogResult = "A";
                            else
                            {
                                QASuppAttRecs.RowFilter = AddToDataViewFilter(OldQASuppAttRecFilter, "TEST_TYPE_CD = 'F2LREF'" + " AND MON_SYS_ID = '" + MonSysID +
                                    "'" + " AND END_DATE = '" + EndDate.ToShortDateString() + "' " + " AND END_HOUR = " + EndHour + " AND END_MIN = " +
                                    EndMinute + " AND TEST_NUM <> '" + TestNumber + "' AND ATTRIBUTE_NAME = 'REF_GHR'");
                                if ((QASuppAttRecs.Count > 0 && CurrentFlowToLoadReference["TEST_SUM_ID"] == DBNull.Value) ||
                                    (QASuppAttRecs.Count > 1 && CurrentFlowToLoadReference["TEST_SUM_ID"] != DBNull.Value) ||
                                    (QASuppAttRecs.Count == 1 && CurrentFlowToLoadReference["TEST_SUM_ID"] != DBNull.Value && CurrentFlowToLoadReference["TEST_SUM_ID"].ToString() != QASuppAttRecs[0]["TEST_SUM_ID"].ToString()))
                                    Category.CheckCatalogResult = "A";
                                else
                                {
                                    QASuppRecs.RowFilter = AddToDataViewFilter(OldQASuppRecsFilter, "TEST_TYPE_CD = 'F2LREF'" + " AND TEST_NUM = '" + TestNumber + "'");
                                    if (QASuppRecs.Count > 0)
                                    {
                                        string DataIDRetrieved = cDBConvert.ToString(((DataRowView)QASuppRecs[0])["QA_SUPP_DATA_ID"]);
                                        Category.SetCheckParameter("Flow_to_Load_Reference_Supp_Data_ID", DataIDRetrieved, eParameterDataType.String);
                                        if (cDBConvert.ToString(((DataRowView)QASuppRecs[0])["CAN_SUBMIT"]) == "N")
                                        {
                                            QASuppRecs.RowFilter = AddToDataViewFilter(QASuppRecs.RowFilter, "MON_SYS_ID <> '" + MonSysID + "'" + " OR END_DATE <> '" + EndDate.ToShortDateString() + "' " +
                                                " OR END_HOUR <> " + EndHour + " OR END_MIN <> " + EndMinute);
                                            if ((QASuppRecs.Count > 0 && CurrentFlowToLoadReference["TEST_SUM_ID"] == DBNull.Value) ||
                                                (QASuppRecs.Count > 1 && CurrentFlowToLoadReference["TEST_SUM_ID"] != DBNull.Value) ||
                                                (QASuppRecs.Count == 1 && CurrentFlowToLoadReference["TEST_SUM_ID"] != DBNull.Value && CurrentFlowToLoadReference["TEST_SUM_ID"].ToString() != QASuppRecs[0]["TEST_SUM_ID"].ToString()))
                                                Category.CheckCatalogResult = "B";
                                            else
                                            {
                                                QASuppAttRecs.RowFilter = OldQASuppAttRecFilter;
                                                QASuppAttRecs.RowFilter = AddToDataViewFilter(OldQASuppAttRecFilter, "QA_SUPP_DATA_ID = '" + DataIDRetrieved + "' AND ATTRIBUTE_NAME = 'REF_GHR'");
                                                if (QASuppAttRecs.Count > 0)
                                                    Category.CheckCatalogResult = "C";
                                                else
                                                    Category.CheckCatalogResult = "D";
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    QASuppAttRecs.RowFilter = OldQASuppAttRecFilter;
                    QASuppRecs.RowFilter = OldQASuppRecsFilter;
                    FlowToLoadRefRecords.RowFilter = OldFlowToLoadRefFilter;
                }
                else
                    Log = false;
            }

            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "F2LREF2");
            }

            return ReturnVal;
        }

        public static string F2LREF3(cCategory Category, ref bool Log)
        //Flow to Load Reference Data RATA Test Number Valid            
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentFlowToLoadReference = (DataRowView)Category.GetCheckParameter("Current_Flow_to_Load_Reference").ParameterValue;
                if (CurrentFlowToLoadReference["RATA_TEST_NUM"] == DBNull.Value)
                {
                    Category.SetCheckParameter("Flow_to_Load_Reference_RATA_Test_Number_Valid", false, eParameterDataType.Boolean);
                    Category.CheckCatalogResult = "A";
                }
                else
                    Category.SetCheckParameter("Flow_to_Load_Reference_RATA_Test_Number_Valid", true, eParameterDataType.Boolean);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "F2LREF3");
            }

            return ReturnVal;
        }

        public static string F2LREF4(cCategory Category, ref bool Log)
        //Flow to Load Reference Data Operating Level Code Valid
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("Flow_to_Load_Reference_Operating_Level_Code_Valid", false, eParameterDataType.Boolean);
                DataRowView CurrentFlowToLoadReference = (DataRowView)Category.GetCheckParameter("Current_Flow_to_Load_Reference").ParameterValue;
                string CurrentOpLvlCd = cDBConvert.ToString(CurrentFlowToLoadReference["OP_LEVEL_CD"]);
                if (CurrentOpLvlCd == "")
                    Category.CheckCatalogResult = "A";
                else
                {
                    DataView OpCdLookupTbl = (DataView)Category.GetCheckParameter("Operating_Level_Code_Lookup_Table").ParameterValue;
                    string OldFilter1 = OpCdLookupTbl.RowFilter;
                    OpCdLookupTbl.RowFilter = AddToDataViewFilter(OldFilter1, "OP_LEVEL_CD = '" + CurrentOpLvlCd + "'");
                    if (OpCdLookupTbl.Count == 0)
                        Category.CheckCatalogResult = "B";
                    else
                    {
                        if (CurrentOpLvlCd != "N" && Convert.ToBoolean(Category.GetCheckParameter("Test_End_Date_Valid").ParameterValue) &&
                            Convert.ToBoolean(Category.GetCheckParameter("Test_End_Hour_Valid").ParameterValue))
                        {
                            DataView LoadRecs = (DataView)Category.GetCheckParameter("Load_Records").ParameterValue;
                            string OldFilter2 = LoadRecs.RowFilter;
                            DateTime CurrentEndDate = cDBConvert.ToDate(CurrentFlowToLoadReference["END_DATE"], DateTypes.END);
                            int CurrentEndHour = cDBConvert.ToHour(CurrentFlowToLoadReference["END_HOUR"], DateTypes.END);
                            LoadRecs.RowFilter = AddToDataViewFilter(OldFilter2, "(BEGIN_DATE < '" + CurrentEndDate.ToShortDateString() + "'" +
                                " OR (BEGIN_DATE = '" + CurrentEndDate.ToShortDateString() + "'" + " AND BEGIN_HOUR < " + (CurrentEndHour + 1) + "))" +
                                " AND (END_DATE IS NULL OR (END_DATE > '" + CurrentEndDate.ToShortDateString() + "'" + " OR (END_DATE = '" +
                                CurrentEndDate.ToShortDateString() + "'" + " AND END_HOUR > " + (CurrentEndHour - 1) + ")))");
                            if (LoadRecs.Count == 0)
                                Category.CheckCatalogResult = "C";
                            else
                            {
                                string OldSort = LoadRecs.Sort;
                                LoadRecs.Sort = "BEGIN_DATE DESC";
                                DataRowView LatestLoadRec = (DataRowView)LoadRecs[0];
                                string LatestNormLvlCd = cDBConvert.ToString(LatestLoadRec["NORMAL_LEVEL_CD"]);
                                string LatestSecLvlCd = cDBConvert.ToString(LatestLoadRec["SECOND_LEVEL_CD"]);
                                int CurrentSecNormInd = cDBConvert.ToInteger(LatestLoadRec["SECOND_NORMAL_IND"]);
                                if (LatestNormLvlCd != CurrentOpLvlCd && (LatestSecLvlCd != CurrentOpLvlCd || CurrentSecNormInd != 1))
                                    Category.CheckCatalogResult = "D";
                                else
                                    Category.SetCheckParameter("Flow_to_Load_Reference_Operating_Level_Code_Valid", true, eParameterDataType.Boolean);
                                LoadRecs.Sort = OldSort;
                            }
                            LoadRecs.RowFilter = OldFilter2;
                        }
                    }
                    OpCdLookupTbl.RowFilter = OldFilter1;
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "F2LREF4");
            }

            return ReturnVal;
        }

        public static string F2LREF5(cCategory Category, ref bool Log)
        //Flow to Load Reference Data Methodology Valid
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("Flow_to_Load_Reference_Methodology_Valid", true, eParameterDataType.Boolean);
                DataRowView CurrentFlowToLoadReference = (DataRowView)Category.GetCheckParameter("Current_Flow_to_Load_Reference").ParameterValue;
                if (CurrentFlowToLoadReference["REF_FLOW_LOAD_RATIO"] != DBNull.Value)
                {
                    if (CurrentFlowToLoadReference["REF_GHR"] != DBNull.Value)
                    {
                        Category.SetCheckParameter("Flow_to_Load_Reference_Methodology_Valid", false, eParameterDataType.Boolean);
                        Category.CheckCatalogResult = "A";
                    }
                }
                else
                    if (CurrentFlowToLoadReference["REF_GHR"] == DBNull.Value)
                    {
                        Category.SetCheckParameter("Flow_to_Load_Reference_Methodology_Valid", false, eParameterDataType.Boolean);
                        Category.CheckCatalogResult = "B";
                    }
                if (string.IsNullOrEmpty(Category.CheckCatalogResult) && CurrentFlowToLoadReference["REF_GHR"] != DBNull.Value &&
                    CurrentFlowToLoadReference["AVG_HRLY_HI_RATE"] == DBNull.Value)
                {
                    Category.SetCheckParameter("Flow_to_Load_Reference_Methodology_Valid", false, eParameterDataType.Boolean);
                    Category.CheckCatalogResult = "C";
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "F2LREF5");
            }

            return ReturnVal;
        }

        public static string F2LREF6(cCategory Category, ref bool Log)
        //Flow to Load Reference Data Calc Separate Reference Indicator Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentFlowToLoadReference = (DataRowView)Category.GetCheckParameter("Current_Flow_to_Load_Reference").ParameterValue;
                DataRowView CurrentLocation = (DataRowView)Category.GetCheckParameter("Current_Location").ParameterValue;
                int CalcSepRefInd = cDBConvert.ToInteger(CurrentFlowToLoadReference["CALC_SEP_REF_IND"]);
                string LocId = cDBConvert.ToString(CurrentLocation["LOCATION_IDENTIFIER"]);
                if (LocId.StartsWith("MS"))
                {
                    if (CalcSepRefInd == int.MinValue)
                        Category.CheckCatalogResult = "A";
                }
                else
                    if (CalcSepRefInd == 1)
                        Category.CheckCatalogResult = "B";

            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "F2LREF6");
            }

            return ReturnVal;
        }

        public static string F2LREF7(cCategory Category, ref bool Log)
        //Flow to Load Reference Data Average Hourly Heat Input Rate Valid
        {
            string ReturnVal = "";

            try
            {
                if (Convert.ToBoolean(Category.GetCheckParameter("Flow_to_Load_Reference_Methodology_Valid").ParameterValue))
                {
                    DataRowView CurrentFlowToLoadReference = (DataRowView)Category.GetCheckParameter("Current_Flow_to_Load_Reference").ParameterValue;
                    if (CurrentFlowToLoadReference["AVG_HRLY_HI_RATE"] != DBNull.Value)
                    {
                        if (CurrentFlowToLoadReference["REF_GHR"] == DBNull.Value)
                            Category.CheckCatalogResult = "A";
                        else
                            if (cDBConvert.ToDecimal(CurrentFlowToLoadReference["AVG_HRLY_HI_RATE"]) <= 0)
                                Category.CheckCatalogResult = "B";
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "F2LREF7");
            }

            return ReturnVal;
        }

        public static string F2LREF8(cCategory Category, ref bool Log)
        //Flow to Load Reference Data Reference RATA Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentFlowToLoadReference = (DataRowView)Category.GetCheckParameter("Current_Flow_to_Load_Reference").ParameterValue;
                Category.SetCheckParameter("Flow_to_Load_Reference_RATA_Summary_ID", null, eParameterDataType.String);
                Category.SetCheckParameter("Flow_to_Load_Reference_RATA_Supp_ID", null, eParameterDataType.String);
                if (Convert.ToBoolean(Category.GetCheckParameter("Flow_to_Load_Reference_RATA_Test_Number_Valid").ParameterValue) &&
                    Convert.ToBoolean(Category.GetCheckParameter("Flow_to_Load_Reference_Operating_Level_Code_Valid").ParameterValue))
                {
                    DataView QASuppRecords = (DataView)Category.GetCheckParameter("QA_Supplemental_Data_Records").ParameterValue;
                    string OldFilter1 = QASuppRecords.RowFilter;
                    DataView RATASummRecords = (DataView)Category.GetCheckParameter("System_RATA_Summary_Records").ParameterValue;
                    string OldFilter2 = RATASummRecords.RowFilter;
                    string MonSysID = cDBConvert.ToString(CurrentFlowToLoadReference["MON_SYS_ID"]);
                    string OpLvlCd = cDBConvert.ToString(CurrentFlowToLoadReference["OP_LEVEL_CD"]);
                    string RATATestNumber = cDBConvert.ToString(CurrentFlowToLoadReference["RATA_TEST_NUM"]);
                    DateTime CurrentEndDate = cDBConvert.ToDate(CurrentFlowToLoadReference["END_DATE"], DateTypes.END);
                    int CurrentEndHour = cDBConvert.ToHour(CurrentFlowToLoadReference["END_HOUR"], DateTypes.END);
                    int CurrentEndMin = cDBConvert.ToInteger(CurrentFlowToLoadReference["END_MIN"]);
                    QASuppRecords.RowFilter = AddToDataViewFilter(OldFilter1, "TEST_TYPE_CD = 'RATA'" + " AND MON_SYS_ID = '" +
                        MonSysID + "'" + " AND TEST_NUM = '" + RATATestNumber + "'");
                    if (QASuppRecords.Count == 0)
                    {
                        RATASummRecords.RowFilter = AddToDataViewFilter(OldFilter2, "TEST_NUM = '" + RATATestNumber + "'");
                        if (RATASummRecords.Count == 0)
                            Category.CheckCatalogResult = "A";
                        else
                        {
                            RATASummRecords.RowFilter = AddToDataViewFilter(RATASummRecords.RowFilter, "OP_LEVEL_CD = '" + OpLvlCd + "'");
                            if (RATASummRecords.Count > 0)
                                Category.CheckCatalogResult = "D";
                            else
                                Category.CheckCatalogResult = "C";
                        }
                    }
                    else
                    {
                        if (cDBConvert.ToString(QASuppRecords[0]["CAN_SUBMIT"]) == "Y")
                        {
                            RATASummRecords.RowFilter = AddToDataViewFilter(OldFilter2, "TEST_NUM = '" + RATATestNumber + "' AND NEEDS_EVAL_FLG = 'Y'");
                            if (RATASummRecords.Count > 0)
                                Category.CheckCatalogResult = "D";
                        }
                        if (String.IsNullOrEmpty(Category.CheckCatalogResult))
                        {
                          if  (cDBConvert.ToString(((DataRowView)QASuppRecords[0])["TEST_RESULT_CD"]) != "")
                          {

                            DataView QASuppAttRecords = (DataView)Category.GetCheckParameter("QA_Supplemental_Attribute_Records").ParameterValue;
                            string OldFilter3 = QASuppAttRecords.RowFilter;
                            string RetrievedQASuppDataID = cDBConvert.ToString(((DataRowView)QASuppRecords[0])["QA_SUPP_DATA_ID"]);
                            QASuppAttRecords.RowFilter = AddToDataViewFilter(OldFilter3, "MON_SYS_ID = '" + MonSysID + "'" + " AND QA_SUPP_DATA_ID = '" +
                                RetrievedQASuppDataID + "'" + " AND ATTRIBUTE_NAME = 'OP_LEVEL_CD_LIST'");
                            if (!OpLvlCd.InList(cDBConvert.ToString(((DataRowView)QASuppAttRecords[0])["ATTRIBUTE_VALUE"])))
                              Category.CheckCatalogResult = "C";
                            else
                              if (Convert.ToBoolean(Category.GetCheckParameter("Test_End_Date_Valid").ParameterValue) &&
                                  Convert.ToBoolean(Category.GetCheckParameter("Test_End_Hour_Valid").ParameterValue) &&
                                  Convert.ToBoolean(Category.GetCheckParameter("Test_End_Minute_Valid").ParameterValue))
                              {
                                DateTime QASuppRecBegDate = cDBConvert.ToDate(((DataRowView)QASuppRecords[0])["BEGIN_DATE"], DateTypes.START);
                                int QASuppRecBegHour = cDBConvert.ToHour(((DataRowView)QASuppRecords[0])["BEGIN_HOUR"], DateTypes.START);
                                int QASuppRecBegMin = cDBConvert.ToInteger(((DataRowView)QASuppRecords[0])["BEGIN_MIN"]);
                                DateTime QASuppRecEndDate = cDBConvert.ToDate(((DataRowView)QASuppRecords[0])["END_DATE"], DateTypes.END);
                                int QASuppRecEndHour = cDBConvert.ToHour(((DataRowView)QASuppRecords[0])["END_HOUR"], DateTypes.END);
                                int QASuppRecEndMin = cDBConvert.ToInteger(((DataRowView)QASuppRecords[0])["END_MIN"]);

                                if ((CurrentEndDate > QASuppRecBegDate || (CurrentEndDate == QASuppRecBegDate &&
                                    (CurrentEndHour > QASuppRecBegHour || (CurrentEndHour == QASuppRecBegHour && CurrentEndMin > QASuppRecBegMin)))) &&
                                    (CurrentEndDate < QASuppRecEndDate || (CurrentEndDate == QASuppRecEndDate &&
                                    (CurrentEndHour < QASuppRecEndHour || (CurrentEndHour == QASuppRecEndHour && CurrentEndMin <= QASuppRecEndMin)))))
                                  Category.SetCheckParameter("Flow_to_Load_Reference_RATA_Supp_ID", cDBConvert.ToString(((DataRowView)QASuppRecords[0])["QA_SUPP_DATA_ID"]), eParameterDataType.String);
                                else
                                  Category.CheckCatalogResult = "B";
                              }
                            QASuppAttRecords.RowFilter = OldFilter3;
                          }
                          else
                            Category.CheckCatalogResult = "D";
                        }
                    }
                    QASuppRecords.RowFilter = OldFilter1;
                    RATASummRecords.RowFilter = OldFilter2;
                }
            }

            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "F2LREF8");
            }

            return ReturnVal;
        }
        public static string F2LREF9(cCategory Category, ref bool Log)
        //Calculate Flow to Load Reference Data Values
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("Flow_to_Load_Reference_Calc_Average_Gross_Unit_Load", null, eParameterDataType.Decimal);
                Category.SetCheckParameter("Flow_to_Load_Reference_Calc_Average_Reference_Method_Flow", null, eParameterDataType.Decimal);
                string F2LRefRATASuppID = Convert.ToString(Category.GetCheckParameter("Flow_to_Load_Reference_RATA_Supp_ID").ParameterValue);
                string F2LRefRATASumID = Convert.ToString(Category.GetCheckParameter("Flow_to_Load_Reference_RATA_Summary_ID").ParameterValue);
                if (F2LRefRATASuppID != "" || F2LRefRATASumID != "")
                {
                    DataRowView CurrentFlowToLoadReference = (DataRowView)Category.GetCheckParameter("Current_Flow_to_Load_Reference").ParameterValue;
                    int RunUsedCount = 0, SumGrossUnitLoad = 0;
                    decimal SumReferenceMethodFlow = 0;
                    string OpLvlCd = cDBConvert.ToString(CurrentFlowToLoadReference["OP_LEVEL_CD"]);
                    string UnitStackPipeID = cDBConvert.ToString(CurrentFlowToLoadReference["LOCATION_IDENTIFIER"]);
                    int CalcSepRefInd = cDBConvert.ToInteger(CurrentFlowToLoadReference["CALC_SEP_REF_IND"]);
                    DataView FacRATARunRecords = (DataView)Category.GetCheckParameter("Facility_RATA_Run_Records").ParameterValue;
                    string OldFilter2 = FacRATARunRecords.RowFilter;
                    int GrossUnitLoad;
                    decimal RATARefVal;

                    if (F2LRefRATASuppID != "")
                    {
                        DataView FacQASuppAttRecords = (DataView)Category.GetCheckParameter("Facility_QA_Supplemental_Attribute_Records").ParameterValue;
                        string OldFilter1 = FacQASuppAttRecords.RowFilter;

                        if (OpLvlCd == "H" || OpLvlCd == "N")
                        {
                            FacQASuppAttRecords.RowFilter = AddToDataViewFilter(OldFilter1, "QA_SUPP_DATA_ID = '" + F2LRefRATASuppID + "'" +
                                " AND ATTRIBUTE_NAME = 'HIGH_RUN_USED_COUNT'");
                            if (FacQASuppAttRecords.Count > 0)
                            {
                                RunUsedCount = cDBConvert.ToInteger(((DataRowView)FacQASuppAttRecords[0])["ATTRIBUTE_VALUE"]);
                                FacQASuppAttRecords.RowFilter = AddToDataViewFilter(OldFilter1, "QA_SUPP_DATA_ID = '" + F2LRefRATASuppID + "'" +
                                    " AND ATTRIBUTE_NAME = 'HIGH_SUM_GROSS_UNIT_LOAD'");
                                if (FacQASuppAttRecords.Count > 0)
                                {
                                    SumGrossUnitLoad = cDBConvert.ToInteger(((DataRowView)FacQASuppAttRecords[0])["ATTRIBUTE_VALUE"]);
                                    FacQASuppAttRecords.RowFilter = AddToDataViewFilter(OldFilter1, "QA_SUPP_DATA_ID = '" + F2LRefRATASuppID + "'" +
                                        " AND ATTRIBUTE_NAME = 'HIGH_SUM_RATA_REF_VALUE'");
                                    if (FacQASuppAttRecords.Count > 0)
                                        SumReferenceMethodFlow = cDBConvert.ToDecimal(((DataRowView)FacQASuppAttRecords[0])["ATTRIBUTE_VALUE"]);
                                    else
                                        Category.CheckCatalogResult = "A";
                                }
                                else
                                    Category.CheckCatalogResult = "A";
                            }
                            else
                                Category.CheckCatalogResult = "A";
                        }
                        else
                            if (OpLvlCd == "M")
                            {
                                FacQASuppAttRecords.RowFilter = AddToDataViewFilter(OldFilter1, "QA_SUPP_DATA_ID = '" + F2LRefRATASuppID + "'" +
                                    " AND ATTRIBUTE_NAME = 'MID_RUN_USED_COUNT'");
                                if (FacQASuppAttRecords.Count > 0)
                                {
                                    RunUsedCount = cDBConvert.ToInteger(((DataRowView)FacQASuppAttRecords[0])["ATTRIBUTE_VALUE"]);
                                    FacQASuppAttRecords.RowFilter = AddToDataViewFilter(OldFilter1, "QA_SUPP_DATA_ID = '" + F2LRefRATASuppID + "'" +
                                        " AND ATTRIBUTE_NAME = 'MID_SUM_GROSS_UNIT_LOAD'");
                                    if (FacQASuppAttRecords.Count > 0)
                                    {
                                        SumGrossUnitLoad = cDBConvert.ToInteger(((DataRowView)FacQASuppAttRecords[0])["ATTRIBUTE_VALUE"]);
                                        FacQASuppAttRecords.RowFilter = AddToDataViewFilter(OldFilter1, "QA_SUPP_DATA_ID = '" + F2LRefRATASuppID + "'" +
                                            " AND ATTRIBUTE_NAME = 'MID_SUM_RATA_REF_VALUE'");
                                        if (FacQASuppAttRecords.Count > 0)
                                            SumReferenceMethodFlow = cDBConvert.ToDecimal(((DataRowView)FacQASuppAttRecords[0])["ATTRIBUTE_VALUE"]);
                                        else
                                            Category.CheckCatalogResult = "A";
                                    }
                                    else
                                        Category.CheckCatalogResult = "A";
                                }
                                else
                                    Category.CheckCatalogResult = "A";
                            }
                            else
                                if (OpLvlCd == "L")
                                {
                                    FacQASuppAttRecords.RowFilter = AddToDataViewFilter(OldFilter1, "QA_SUPP_DATA_ID = '" + F2LRefRATASuppID + "'" +
                                        " AND ATTRIBUTE_NAME = 'LOW_RUN_USED_COUNT'");
                                    if (FacQASuppAttRecords.Count > 0)
                                    {
                                        RunUsedCount = cDBConvert.ToInteger(((DataRowView)FacQASuppAttRecords[0])["ATTRIBUTE_VALUE"]);
                                        FacQASuppAttRecords.RowFilter = AddToDataViewFilter(OldFilter1, "QA_SUPP_DATA_ID = '" + F2LRefRATASuppID + "'" +
                                            " AND ATTRIBUTE_NAME = 'LOW_SUM_GROSS_UNIT_LOAD'");
                                        if (FacQASuppAttRecords.Count > 0)
                                        {
                                            SumGrossUnitLoad = cDBConvert.ToInteger(((DataRowView)FacQASuppAttRecords[0])["ATTRIBUTE_VALUE"]);
                                            FacQASuppAttRecords.RowFilter = AddToDataViewFilter(OldFilter1, "QA_SUPP_DATA_ID = '" + F2LRefRATASuppID + "'" +
                                                " AND ATTRIBUTE_NAME = 'LOW_SUM_RATA_REF_VALUE'");
                                            if (FacQASuppAttRecords.Count > 0)
                                                SumReferenceMethodFlow = cDBConvert.ToDecimal(((DataRowView)FacQASuppAttRecords[0])["ATTRIBUTE_VALUE"]);
                                            else
                                                Category.CheckCatalogResult = "A";
                                        }
                                        else
                                            Category.CheckCatalogResult = "A";
                                    }
                                    else
                                        Category.CheckCatalogResult = "A";
                                }
                        FacQASuppAttRecords.RowFilter = OldFilter1;
                    }
                    else
                        if (F2LRefRATASumID != "")
                        {
                            FacRATARunRecords.RowFilter = AddToDataViewFilter(OldFilter2, "RATA_SUM_ID = '" + F2LRefRATASumID + "'" + " AND RUN_STATUS_CD = 'RUNUSED'");
                            foreach (DataRowView drv in FacRATARunRecords)
                            {
                                GrossUnitLoad = cDBConvert.ToInteger(drv["GROSS_UNIT_LOAD"]);
                                RATARefVal = cDBConvert.ToDecimal(drv["CALC_RATA_REF_VALUE"]);
                                if (RATARefVal == Decimal.MinValue)
                                    RATARefVal = cDBConvert.ToDecimal(drv["RATA_REF_VALUE"]);

                                if (GrossUnitLoad > 0 && RATARefVal > 0)
                                {
                                    RunUsedCount++;
                                    SumGrossUnitLoad += cDBConvert.ToInteger(drv["GROSS_UNIT_LOAD"]);
                                    SumReferenceMethodFlow += RATARefVal;
                                }
                                else
                                {
                                    Category.CheckCatalogResult = "A";
                                    break;
                                }
                            }
                            if (string.IsNullOrEmpty(Category.CheckCatalogResult) && RunUsedCount < 9)
                                Category.CheckCatalogResult = "A";
                        }
                    if (string.IsNullOrEmpty(Category.CheckCatalogResult))
                    {
                        SumReferenceMethodFlow /= RunUsedCount;

                        if (UnitStackPipeID.StartsWith("MS") && CalcSepRefInd != 1)
                        {
                            DataView UnitStackConfigs = (DataView)Category.GetCheckParameter("Facility_Unit_Stack_Configuration_Records").ParameterValue;
                            string OldFilter3 = UnitStackConfigs.RowFilter;
                            string UnitStackPipeID2 = cDBConvert.ToString(CurrentFlowToLoadReference["MON_LOC_ID"]);
                            UnitStackConfigs.RowFilter = AddToDataViewFilter(OldFilter3, "STACK_PIPE_MON_LOC_ID = '" + UnitStackPipeID2 + "'");
                            int UnitID = cDBConvert.ToInteger(((DataRowView)UnitStackConfigs[0])["UNIT_ID"]);
                            UnitStackConfigs.RowFilter = AddToDataViewFilter(OldFilter3, "UNIT_ID = " + UnitID + " AND STACK_NAME LIKE 'MS%' AND STACK_NAME <> '" + UnitStackPipeID + "'");
                            DataView FacQASuppDataRecords = (DataView)Category.GetCheckParameter("Facility_QA_Supplemental_Data_Records").ParameterValue;
                            string OldFilter4 = FacQASuppDataRecords.RowFilter;
                            string StackName;
                            DateTime EndDate = cDBConvert.ToDate(CurrentFlowToLoadReference["END_DATE"], DateTypes.END);
                            string EndDatePlus30 = EndDate.AddDays(30).ToShortDateString();
                            string EndDateMinus30 = EndDate.AddDays(-30).ToShortDateString();

                            foreach (DataRowView drv in UnitStackConfigs)
                            {
                                int OtherRunUsedCount = 0;
                                decimal OtherSumReferenceMethodFlow = 0;
                                StackName = cDBConvert.ToString(drv["STACK_NAME"]);
                                FacQASuppDataRecords.RowFilter = AddToDataViewFilter(OldFilter4, "LOCATION_IDENTIFIER = '" + StackName + "'" +
                                    " AND TEST_TYPE_CD = 'RATA' AND CAN_SUBMIT = 'N' AND SYS_TYPE_CD = 'FLOW'" + " AND END_DATE >= '" +
                                    EndDateMinus30 + "' AND END_DATE <= '" + EndDatePlus30 + "' AND TEST_RESULT_CD LIKE 'PASS%'");
                                if (FacQASuppDataRecords.Count > 1)
                                    Category.CheckCatalogResult = "B";
                                else
                                    if (FacQASuppDataRecords.Count == 1)
                                    {
                                        string FacQASuppDataID = cDBConvert.ToString(((DataRowView)FacQASuppDataRecords[0])["QA_SUPP_DATA_ID"]);
                                        DataView FacQASuppAttRecords = (DataView)Category.GetCheckParameter("Facility_QA_Supplemental_Attribute_Records").ParameterValue;
                                        string OldFilter5 = FacQASuppAttRecords.RowFilter;
                                        FacQASuppAttRecords.RowFilter = AddToDataViewFilter(OldFilter5, "QA_SUPP_DATA_ID = '" + FacQASuppDataID + "'" +
                                            " AND ATTRIBUTE_NAME = 'OP_LEVEL_CD_LIST'" + " AND ATTRIBUTE_VALUE LIKE '%" + OpLvlCd + "%'");
                                        if (FacQASuppAttRecords.Count == 0)
                                            Category.CheckCatalogResult = "B";
                                        else
                                            if (OpLvlCd == "H" || OpLvlCd == "N")
                                            {
                                                FacQASuppAttRecords.RowFilter = AddToDataViewFilter(OldFilter5, "QA_SUPP_DATA_ID = '" + FacQASuppDataID + "'" +
                                                    " AND ATTRIBUTE_NAME = 'HIGH_RUN_USED_COUNT'");
                                                if (FacQASuppAttRecords.Count > 0)
                                                {
                                                    OtherRunUsedCount += cDBConvert.ToInteger(((DataRowView)FacQASuppAttRecords[0])["ATTRIBUTE_VALUE"]);
                                                    FacQASuppAttRecords.RowFilter = AddToDataViewFilter(OldFilter5, "QA_SUPP_DATA_ID = '" +
                                                        FacQASuppDataID + "'" + " AND ATTRIBUTE_NAME = 'HIGH_SUM_GROSS_UNIT_LOAD'");
                                                    if (FacQASuppAttRecords.Count > 0)
                                                    {
                                                        SumGrossUnitLoad += cDBConvert.ToInteger(((DataRowView)FacQASuppAttRecords[0])["ATTRIBUTE_VALUE"]);
                                                        FacQASuppAttRecords.RowFilter = AddToDataViewFilter(OldFilter5, "QA_SUPP_DATA_ID = '" + FacQASuppDataID +
                                                            "'" + " AND ATTRIBUTE_NAME = 'HIGH_SUM_RATA_REF_VALUE'");
                                                        if (FacQASuppAttRecords.Count > 0)
                                                            OtherSumReferenceMethodFlow += cDBConvert.ToDecimal(((DataRowView)FacQASuppAttRecords[0])["ATTRIBUTE_VALUE"]);
                                                        else
                                                            Category.CheckCatalogResult = "A";
                                                    }
                                                    else
                                                        Category.CheckCatalogResult = "A";
                                                }
                                                else
                                                    Category.CheckCatalogResult = "A";
                                            }
                                            else
                                                if (OpLvlCd == "M")
                                                {
                                                    FacQASuppAttRecords.RowFilter = AddToDataViewFilter(OldFilter5, "QA_SUPP_DATA_ID = '" + FacQASuppDataID + "'" +
                                                        " AND ATTRIBUTE_NAME = 'MID_RUN_USED_COUNT'");
                                                    if (FacQASuppAttRecords.Count > 0)
                                                    {
                                                        OtherRunUsedCount += cDBConvert.ToInteger(((DataRowView)FacQASuppAttRecords[0])["ATTRIBUTE_VALUE"]);
                                                        FacQASuppAttRecords.RowFilter = AddToDataViewFilter(OldFilter5, "QA_SUPP_DATA_ID = '" +
                                                            FacQASuppDataID + "'" + " AND ATTRIBUTE_NAME = 'MID_SUM_GROSS_UNIT_LOAD'");
                                                        if (FacQASuppAttRecords.Count > 0)
                                                        {
                                                            SumGrossUnitLoad += cDBConvert.ToInteger(((DataRowView)FacQASuppAttRecords[0])["ATTRIBUTE_VALUE"]);
                                                            FacQASuppAttRecords.RowFilter = AddToDataViewFilter(OldFilter5, "QA_SUPP_DATA_ID = '" + FacQASuppDataID +
                                                                "'" + " AND ATTRIBUTE_NAME = 'MID_SUM_RATA_REF_VALUE'");
                                                            if (FacQASuppAttRecords.Count > 0)
                                                                OtherSumReferenceMethodFlow += cDBConvert.ToDecimal(((DataRowView)FacQASuppAttRecords[0])["ATTRIBUTE_VALUE"]);
                                                            else
                                                                Category.CheckCatalogResult = "A";
                                                        }
                                                        else
                                                            Category.CheckCatalogResult = "A";
                                                    }
                                                    else
                                                        Category.CheckCatalogResult = "A";
                                                }
                                                else
                                                    if (OpLvlCd == "L")
                                                    {
                                                        FacQASuppAttRecords.RowFilter = AddToDataViewFilter(OldFilter5, "QA_SUPP_DATA_ID = '" + FacQASuppDataID + "'" +
                                                            " AND ATTRIBUTE_NAME = 'LOW_RUN_USED_COUNT'");
                                                        if (FacQASuppAttRecords.Count > 0)
                                                        {
                                                            OtherRunUsedCount += cDBConvert.ToInteger(((DataRowView)FacQASuppAttRecords[0])["ATTRIBUTE_VALUE"]);
                                                            FacQASuppAttRecords.RowFilter = AddToDataViewFilter(OldFilter5, "QA_SUPP_DATA_ID = '" +
                                                                FacQASuppDataID + "'" + " AND ATTRIBUTE_NAME = 'LOW_SUM_GROSS_UNIT_LOAD'");
                                                            if (FacQASuppAttRecords.Count > 0)
                                                            {
                                                                SumGrossUnitLoad += cDBConvert.ToInteger(((DataRowView)FacQASuppAttRecords[0])["ATTRIBUTE_VALUE"]);
                                                                FacQASuppAttRecords.RowFilter = AddToDataViewFilter(OldFilter5, "QA_SUPP_DATA_ID = '" + FacQASuppDataID +
                                                                    "'" + " AND ATTRIBUTE_NAME = 'LOW_SUM_RATA_REF_VALUE'");
                                                                if (FacQASuppAttRecords.Count > 0)
                                                                    OtherSumReferenceMethodFlow += cDBConvert.ToDecimal(((DataRowView)FacQASuppAttRecords[0])["ATTRIBUTE_VALUE"]);
                                                                else
                                                                    Category.CheckCatalogResult = "A";
                                                            }
                                                            else
                                                                Category.CheckCatalogResult = "A";
                                                        }
                                                        else
                                                            Category.CheckCatalogResult = "A";
                                                    }
                                        FacQASuppAttRecords.RowFilter = OldFilter5;
                                    }
                                    else//i.e., FacQASuppDataRecords.Count == 0
                                    {
                                        DataView FacRATASumRecords = (DataView)Category.GetCheckParameter("Facility_RATA_Summary_Records").ParameterValue;
                                        string OldFilter6 = FacRATASumRecords.RowFilter;
                                        string UnitStackConfigRecStackPipeMonLocID = cDBConvert.ToString(drv["STACK_PIPE_MON_LOC_ID"]);
                                        FacRATASumRecords.RowFilter = AddToDataViewFilter(OldFilter6, "MON_LOC_ID = '" + UnitStackConfigRecStackPipeMonLocID + "'" +
                                            " AND SYS_TYPE_CD = 'FLOW'" + " AND END_DATE >= '" + EndDateMinus30 + "' AND END_DATE <= '" +
                                            EndDatePlus30 + "'" + " AND TEST_RESULT_CD LIKE 'PASS%' AND OP_LEVEL_CD = '" + OpLvlCd + "'");
                                        if (FacRATASumRecords.Count == 1)
                                        {
                                            string RetrievedRATASumID = cDBConvert.ToString(((DataRowView)FacRATASumRecords[0])["RATA_SUM_ID"]);
                                            FacRATARunRecords.RowFilter = AddToDataViewFilter(OldFilter2, "RATA_SUM_ID = '" + RetrievedRATASumID + "'" +
                                                " AND RUN_STATUS_CD = 'RUNUSED'");
                                            foreach (DataRowView drv3 in FacRATARunRecords)
                                            {
                                                GrossUnitLoad = cDBConvert.ToInteger(drv3["GROSS_UNIT_LOAD"]);
                                                RATARefVal = cDBConvert.ToDecimal(drv3["CALC_RATA_REF_VALUE"]);
                                                if (RATARefVal == Decimal.MinValue)
                                                    RATARefVal = cDBConvert.ToDecimal(drv3["RATA_REF_VALUE"]);
                                                if (GrossUnitLoad > 0 && RATARefVal > 0)
                                                {
                                                    OtherRunUsedCount++;
                                                    SumGrossUnitLoad += GrossUnitLoad;
                                                    OtherSumReferenceMethodFlow += RATARefVal;
                                                }
                                                else
                                                {
                                                    Category.CheckCatalogResult = "A";
                                                    break;
                                                }
                                            }
                                            if (OtherRunUsedCount < 9)
                                                Category.CheckCatalogResult = "A";
                                        }
                                        else
                                            Category.CheckCatalogResult = "B";
                                        FacRATASumRecords.RowFilter = OldFilter6;
                                    }
                                if (!string.IsNullOrEmpty(Category.CheckCatalogResult))
                                    break;
                                RunUsedCount += OtherRunUsedCount;
                                SumReferenceMethodFlow += (OtherSumReferenceMethodFlow / OtherRunUsedCount);
                            }
                            UnitStackConfigs.RowFilter = OldFilter3;
                            FacQASuppDataRecords.RowFilter = OldFilter4;
                        }
                    }
                    FacRATARunRecords.RowFilter = OldFilter2;

                    if (string.IsNullOrEmpty(Category.CheckCatalogResult))
                    {
                        Category.SetCheckParameter("Flow_to_Load_Reference_Calc_Average_Gross_Unit_Load", Math.Round((decimal)(SumGrossUnitLoad / RunUsedCount), MidpointRounding.AwayFromZero), eParameterDataType.Decimal);
                        Category.SetCheckParameter("Flow_to_Load_Reference_Calc_Average_Reference_Method_Flow", Math.Round(SumReferenceMethodFlow, MidpointRounding.AwayFromZero), eParameterDataType.Decimal);
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "F2LREF9");
            }

            return ReturnVal;
        }

        public static string F2LREF10(cCategory Category, ref bool Log)
        //Flow to Load Reference Data Average Reference Method Flow Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentFlowToLoadReference = (DataRowView)Category.GetCheckParameter("Current_Flow_to_Load_Reference").ParameterValue;
                if (CurrentFlowToLoadReference["AVG_REF_METHOD_FLOW"] == DBNull.Value)
                    Category.CheckCatalogResult = "A";
                else
                {
                    int AvgRefMethodFlow = cDBConvert.ToInteger(CurrentFlowToLoadReference["AVG_REF_METHOD_FLOW"]);
                    if (AvgRefMethodFlow <= 0)
                        Category.CheckCatalogResult = "B";
                    else
                    {
                        if (Category.GetCheckParameter("Flow_to_Load_Reference_Calc_Average_Reference_Method_Flow").ParameterValue != null)
                        {
                            decimal F2LRefCalcAvgRefMethodFlow = Convert.ToDecimal(Category.GetCheckParameter("Flow_to_Load_Reference_Calc_Average_Reference_Method_Flow").ParameterValue);
                            DataView TestToleranceRecords = (DataView)Category.GetCheckParameter("Test_Tolerances_Cross_Check_Table").ParameterValue;
                            string OldFilter = TestToleranceRecords.RowFilter;
                            TestToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, "TestTypeCode = 'F2LREF' AND FieldDescription = 'AverageReferenceMethodFlow'");
                            if (Math.Abs(F2LRefCalcAvgRefMethodFlow - AvgRefMethodFlow) > cDBConvert.ToDecimal(((DataRowView)TestToleranceRecords[0])["Tolerance"]))
                                Category.CheckCatalogResult = "C";
                            TestToleranceRecords.RowFilter = OldFilter;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "F2LREF10");
            }

            return ReturnVal;
        }

        public static string F2LREF11(cCategory Category, ref bool Log)
        //Flow to Load Reference Data Average Gross Unit Load Valid
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("Flow_to_Load_Reference_Calc_GHR", null, eParameterDataType.Decimal);
                Category.SetCheckParameter("Flow_to_Load_Reference_Calc_Flow_to_Load_Ratio", null, eParameterDataType.Decimal);
                DataRowView CurrentFlowToLoadReference = (DataRowView)Category.GetCheckParameter("Current_Flow_to_Load_Reference").ParameterValue;
                if (CurrentFlowToLoadReference["AVG_GROSS_UNIT_LOAD"] == DBNull.Value)
                    Category.CheckCatalogResult = "A";
                else
                {
                    int AvgGrossUnitLoad = cDBConvert.ToInteger(CurrentFlowToLoadReference["AVG_GROSS_UNIT_LOAD"]);
                    if (AvgGrossUnitLoad <= 0)
                        Category.CheckCatalogResult = "B";
                    else
                    {
                        if (Convert.ToBoolean(Category.GetCheckParameter("Flow_to_Load_Reference_Methodology_Valid").ParameterValue))
                        {
                            decimal AvgHrlyHIRate = cDBConvert.ToDecimal(CurrentFlowToLoadReference["AVG_HRLY_HI_RATE"]);
                            if (AvgHrlyHIRate > 0)
                                Category.SetCheckParameter("Flow_to_Load_Reference_Calc_GHR", Math.Round(((AvgHrlyHIRate / AvgGrossUnitLoad) * 1000), 0, MidpointRounding.AwayFromZero), eParameterDataType.Decimal);
                            int AvgRefMethodFlow = cDBConvert.ToInteger(CurrentFlowToLoadReference["AVG_REF_METHOD_FLOW"]);
                            if (AvgRefMethodFlow > 0)
                                Category.SetCheckParameter("Flow_to_Load_Reference_Calc_Flow_to_Load_Ratio", Math.Round(((decimal)(AvgRefMethodFlow / AvgGrossUnitLoad)) / 100000, 2, MidpointRounding.AwayFromZero), eParameterDataType.Decimal);
                        }
                        if (Category.GetCheckParameter("Flow_to_Load_Reference_Calc_Average_Gross_Unit_Load").ParameterValue != null)
                        {
                            DataView TestToleranceRecords = (DataView)Category.GetCheckParameter("Test_Tolerances_Cross_Check_Table").ParameterValue;
                            string OldFilter = TestToleranceRecords.RowFilter;
                            TestToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, "TestTypeCode = 'F2LREF' AND FieldDescription = 'AverageGrossUnitLoad'");
                            if (TestToleranceRecords.Count > 0)
                            {
                                decimal F2LRefCalcAvgGrossUnitLoad = Convert.ToDecimal(Category.GetCheckParameter("Flow_to_Load_Reference_Calc_Average_Gross_Unit_Load").ParameterValue);
                                if (Math.Abs(F2LRefCalcAvgGrossUnitLoad - AvgGrossUnitLoad) > Convert.ToDecimal(((DataRowView)TestToleranceRecords[0])["Tolerance"]))
                                    Category.CheckCatalogResult = "C";
                            }
                            TestToleranceRecords.RowFilter = OldFilter;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "F2LREF11");
            }

            return ReturnVal;
        }

        public static string F2LREF12(cCategory Category, ref bool Log)
        //Flow to Load Reference Data Reference Flow to Load Ratio Valid
        {
            string ReturnVal = "";

            try
            {
                if (Convert.ToBoolean(Category.GetCheckParameter("Flow_to_Load_Reference_Methodology_Valid").ParameterValue))
                {
                    DataRowView CurrentFlowToLoadReference = (DataRowView)Category.GetCheckParameter("Current_Flow_to_Load_Reference").ParameterValue;
                    if (CurrentFlowToLoadReference["REF_FLOW_LOAD_RATIO"] != DBNull.Value)
                    {
                        decimal RefFlowLoadRatio = cDBConvert.ToDecimal(CurrentFlowToLoadReference["REF_FLOW_LOAD_RATIO"]);
                        if (RefFlowLoadRatio <= 0)
                            Category.CheckCatalogResult = "A";
                        else
                        {
                            if (Category.GetCheckParameter("Flow_To_Load_Reference_Calc_Flow_To_Load_Ratio").ParameterValue != null)
                            {
                                DataView TestToleranceRecords = (DataView)Category.GetCheckParameter("Test_Tolerances_Cross_Check_Table").ParameterValue;
                                string OldFilter = TestToleranceRecords.RowFilter;
                                TestToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, "TestTypeCode = 'F2LREF' AND FieldDescription = 'ReferenceFlowLoadRatio'");
                                decimal F2LRefCalcFlowLoadRatio = Convert.ToDecimal(Category.GetCheckParameter("Flow_to_Load_Reference_Calc_Flow_to_Load_Ratio").ParameterValue);
                                if (Math.Abs(F2LRefCalcFlowLoadRatio - RefFlowLoadRatio) > cDBConvert.ToDecimal(((DataRowView)TestToleranceRecords[0])["Tolerance"]))
                                    Category.CheckCatalogResult = "B";
                                TestToleranceRecords.RowFilter = OldFilter;
                            }
                        }
                    }
                }
                else
                    Log = false;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "F2LREF12");
            }

            return ReturnVal;
        }

        public static string F2LREF13(cCategory Category, ref bool Log)
        //Flow to Load Reference Data Reference Gross Heat Rate Valid
        {
            string ReturnVal = "";

            try
            {
                if (Convert.ToBoolean(Category.GetCheckParameter("Flow_to_Load_Reference_Methodology_Valid").ParameterValue))
                {
                    DataRowView CurrentFlowToLoadReference = (DataRowView)Category.GetCheckParameter("Current_Flow_to_Load_Reference").ParameterValue;
                    if (CurrentFlowToLoadReference["REF_GHR"] != DBNull.Value)
                    {
                        int RefGHR = cDBConvert.ToInteger(CurrentFlowToLoadReference["REF_GHR"]);
                        if (RefGHR <= 0)
                            Category.CheckCatalogResult = "A";
                        else
                        {
                            if (Category.GetCheckParameter("Flow_To_Load_Reference_Calc_GHR").ParameterValue != null)
                            {
                                DataView TestToleranceRecords = (DataView)Category.GetCheckParameter("Test_Tolerances_Cross_Check_Table").ParameterValue;
                                string OldFilter = TestToleranceRecords.RowFilter;
                                TestToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, "TestTypeCode = 'F2LREF' AND FieldDescription = 'ReferenceGHR'");
                                decimal F2LRefCalcGHR = Convert.ToDecimal(Category.GetCheckParameter("Flow_to_Load_Reference_Calc_GHR").ParameterValue);
                                if (Math.Abs(F2LRefCalcGHR - RefGHR) > cDBConvert.ToDecimal(((DataRowView)TestToleranceRecords[0])["Tolerance"]))
                                    Category.CheckCatalogResult = "B";
                                TestToleranceRecords.RowFilter = OldFilter;
                            }
                        }
                    }
                }
                else
                    Log = false;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "F2LREF13");
            }

            return ReturnVal;
        }

        public static string F2LREF14(cCategory Category, ref bool Log)
        //Duplicate Flow to Load Reference Data
        {
            string ReturnVal = "";

            try
            {
                if (Convert.ToBoolean(Category.GetCheckParameter("Test_Number_Valid").ParameterValue))
                {
                    DataView TestRecs = (DataView)Category.GetCheckParameter("Location_Test_Records").ParameterValue;
                    string OldFilter = TestRecs.RowFilter;
                    DataRowView CurrentFlowToLoadReference = (DataRowView)Category.GetCheckParameter("Current_Flow_to_Load_Reference").ParameterValue;
                    string TestNum = cDBConvert.ToString(CurrentFlowToLoadReference["TEST_NUM"]);
                    TestRecs.RowFilter = AddToDataViewFilter(OldFilter, "TEST_TYPE_CD = 'F2LREF' AND TEST_NUM = '" + TestNum + "'");
                    if ((TestRecs.Count > 0 && CurrentFlowToLoadReference["TEST_SUM_ID"] == DBNull.Value) ||
                        (TestRecs.Count > 1 && CurrentFlowToLoadReference["TEST_SUM_ID"] != DBNull.Value) ||
                        (TestRecs.Count == 1 && CurrentFlowToLoadReference["TEST_SUM_ID"] != DBNull.Value && CurrentFlowToLoadReference["TEST_SUM_ID"].ToString() != TestRecs[0]["TEST_SUM_ID"].ToString()))
                    {
                        Category.SetCheckParameter("Duplicate_Flow_To_Load_Reference", true, eParameterDataType.Boolean);
                        Category.CheckCatalogResult = "A";
                    }
                    else
                    {
                      string TestSumID = cDBConvert.ToString(CurrentFlowToLoadReference["TEST_SUM_ID"]);
                      if (TestSumID != "")
                      {
                        DataView QASuppRecords = (DataView)Category.GetCheckParameter("QA_Supplemental_Data_Records").ParameterValue;
                        string OldFilter2 = QASuppRecords.RowFilter;
                        QASuppRecords.RowFilter = AddToDataViewFilter(OldFilter2, "TEST_NUM = '" + TestNum + "' AND TEST_TYPE_CD = 'F2LREF'");
                        if (QASuppRecords.Count > 0 && cDBConvert.ToString(QASuppRecords[0]["TEST_SUM_ID"]) != TestSumID)
                        {
                          Category.SetCheckParameter("Duplicate_Flow_To_Load_Reference", true, eParameterDataType.Boolean);
                          Category.CheckCatalogResult = "B";
                        }
                        else
                          Category.SetCheckParameter("Duplicate_Flow_To_Load_Reference", false, eParameterDataType.Boolean);
                        QASuppRecords.RowFilter = OldFilter2;
                      }
                      else
                        Category.SetCheckParameter("Duplicate_Flow_To_Load_Reference", false, eParameterDataType.Boolean);
                    }
                    TestRecs.RowFilter = OldFilter;
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "F2LREF14");
            }

            return ReturnVal;
        }

        public static string F2LREF15(cCategory Category, ref bool Log)
        //System ID Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentFlowToLoadReference = (DataRowView)Category.GetCheckParameter("Current_Flow_to_Load_Reference").ParameterValue;
                if (CurrentFlowToLoadReference["MON_SYS_ID"] == DBNull.Value)
                    Category.CheckCatalogResult = "A";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "F2LREF15");
            }

            return ReturnVal;
        }

        public static string F2LREF16(cCategory Category, ref bool Log)
        //Calculate Flow-to-Load Reference Data
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentFlowToLoadReference = (DataRowView)Category.GetCheckParameter("Current_Flow_to_Load_Reference").ParameterValue;
                Category.SetCheckParameter("F2L_Calc_Flow", null, eParameterDataType.Decimal);
                Category.SetCheckParameter("F2L_Calc_GHR", null, eParameterDataType.Decimal);
                Category.SetCheckParameter("F2L_Calc_GUL", null, eParameterDataType.Decimal);
                Category.SetCheckParameter("F2L_Calc_Ratio", null, eParameterDataType.Decimal);

                DateTime F2LEndDate = cDBConvert.ToDate(CurrentFlowToLoadReference["END_DATE"], DateTypes.END);
                int F2LEndHour = cDBConvert.ToInteger(CurrentFlowToLoadReference["END_HOUR"]);
                int F2LEndMin = cDBConvert.ToInteger(CurrentFlowToLoadReference["END_MIN"]);
                string RATATestNum = cDBConvert.ToString(CurrentFlowToLoadReference["RATA_TEST_NUM"]);
                string OpLvlCd = cDBConvert.ToString(CurrentFlowToLoadReference["OP_LEVEL_CD"]);

                if( CurrentFlowToLoadReference["MON_SYS_ID"] == DBNull.Value || RATATestNum == "" || !OpLvlCd.InList( "H,L,M" ) )
                    Category.CheckCatalogResult = "A";
                else
                    if( F2LEndDate == DateTime.MaxValue || F2LEndDate < new DateTime( 1993, 1, 1 ) || F2LEndDate > DateTime.Now || F2LEndHour < 0 || 23 < F2LEndHour || F2LEndMin < 0 || 59 < F2LEndMin )
                        Category.CheckCatalogResult = "A";
                    else
                        if ((CurrentFlowToLoadReference["REF_FLOW_LOAD_RATIO"] == DBNull.Value && CurrentFlowToLoadReference["REF_GHR"] == DBNull.Value) ||
                            (CurrentFlowToLoadReference["REF_FLOW_LOAD_RATIO"] != DBNull.Value && CurrentFlowToLoadReference["REF_GHR"] != DBNull.Value))
                            Category.CheckCatalogResult = "A";
                        else
                        {
                            decimal HIRate = cDBConvert.ToDecimal(CurrentFlowToLoadReference["AVG_HRLY_HI_RATE"]);
                            if (CurrentFlowToLoadReference["REF_GHR"] != DBNull.Value && HIRate <= 0)
                                Category.CheckCatalogResult = "A";
                        }
                if (string.IsNullOrEmpty(Category.CheckCatalogResult))
                {
                    DataView RATASumRecs = (DataView)Category.GetCheckParameter("Facility_RATA_Summary_Records").ParameterValue;
                    string OldFilter1 = RATASumRecs.RowFilter;
                    RATASumRecs.RowFilter = AddToDataViewFilter(OldFilter1, "MON_SYS_ID = '" + cDBConvert.ToString(CurrentFlowToLoadReference["MON_SYS_ID"]) + "' AND OP_LEVEL_CD = '" + OpLvlCd + "' AND TEST_NUM = '" + RATATestNum + "'");
                    if (RATASumRecs.Count == 0)
                        Category.CheckCatalogResult = "B";
                    else
                    {
                        DateTime SumRecBegDate = cDBConvert.ToDate(RATASumRecs[0]["BEGIN_DATE"], DateTypes.START);
                        int SumRecBegHour = cDBConvert.ToHour(RATASumRecs[0]["BEGIN_HOUR"], DateTypes.START);
                        int SumRecBegMin = cDBConvert.ToInteger(RATASumRecs[0]["BEGIN_MIN"]);
                        DateTime SumRecEndDate = cDBConvert.ToDate(RATASumRecs[0]["END_DATE"], DateTypes.END);
                        int SumRecEndHour = cDBConvert.ToHour(RATASumRecs[0]["END_HOUR"], DateTypes.END);
                        int SumRecEndMin = cDBConvert.ToInteger(RATASumRecs[0]["END_MIN"]);
                        if (F2LEndDate < SumRecBegDate || (F2LEndDate == SumRecBegDate && (F2LEndHour < SumRecBegHour || (F2LEndHour == SumRecBegHour && F2LEndMin < SumRecBegMin))) || F2LEndDate > SumRecEndDate ||
                            (F2LEndDate == SumRecEndDate && (F2LEndHour > SumRecEndHour || (F2LEndHour == SumRecEndHour && F2LEndMin > SumRecEndMin))))
                            Category.CheckCatalogResult = "E";
                        else
                        {
                            int RunUsedCt = 0;
                            decimal SumGrossUnitLoad = 0;
                            decimal SumRefMethFlow = 0;
                            DataView RATARunRecs = (DataView)Category.GetCheckParameter("Facility_RATA_Run_Records").ParameterValue;
                            string OldFilter2 = RATARunRecs.RowFilter;
                            RATARunRecs.RowFilter = AddToDataViewFilter(OldFilter2, "RATA_SUM_ID = '" + cDBConvert.ToString(RATASumRecs[0]["RATA_SUM_ID"]) +
                                "' AND RUN_STATUS_CD = 'RUNUSED'");
                            decimal GrossUnitLd;
                            decimal RATARefVal;
                            decimal CalcRATARefVal;
                            foreach (DataRowView drvRun1 in RATARunRecs)
                            {
                                GrossUnitLd = cDBConvert.ToDecimal(drvRun1["GROSS_UNIT_LOAD"]);
                                RATARefVal = cDBConvert.ToDecimal(drvRun1["RATA_REF_VALUE"]);
                                CalcRATARefVal = cDBConvert.ToDecimal(drvRun1["CALC_RATA_REF_VALUE"]);
                                if (GrossUnitLd > 0 && RATARefVal > 0)
                                {
                                    RunUsedCt++;
                                    SumGrossUnitLoad += GrossUnitLd;
                                    SumRefMethFlow += RATARefVal;
                                }
                                else
                                {
                                    Category.CheckCatalogResult = "C";
                                    break;
                                }
                            }
                            if (string.IsNullOrEmpty(Category.CheckCatalogResult))
                            {
                                RATARunRecs.RowFilter = OldFilter2;
                                if (RunUsedCt < 9)
                                    Category.CheckCatalogResult = "C";
                                else
                                {
                                    SumRefMethFlow = SumRefMethFlow / RunUsedCt;
                                    DataRowView MonitorLocationRec = (DataRowView)Category.GetCheckParameter("Current_Monitor_Location").ParameterValue;
                                    string UnitStackPipeId = cDBConvert.ToString(MonitorLocationRec["STACK_NAME"]);
                                    int CalcSepRefInd = cDBConvert.ToInteger(CurrentFlowToLoadReference["CALC_SEP_REF_IND"]);
                                    if (UnitStackPipeId.StartsWith("MS") && CalcSepRefInd != 1)
                                    {
                                        DataView UnStConfigRecs = (DataView)Category.GetCheckParameter("Unit_Stack_Configuration_Records").ParameterValue;
                                        string OldFilter3 = UnStConfigRecs.RowFilter;
                                        UnStConfigRecs.RowFilter = AddToDataViewFilter(OldFilter3, "STACK_PIPE_MON_LOC_ID = '" + cDBConvert.ToString(CurrentFlowToLoadReference["MON_LOC_ID"]) + "'");
                                        string LocUnStConfigRecMonLocId = cDBConvert.ToString(UnStConfigRecs[0]["MON_LOC_ID"]);
                                        UnStConfigRecs.RowFilter = OldFilter3;
                                        UnStConfigRecs.RowFilter = AddToDataViewFilter(OldFilter3, "MON_LOC_ID = '" + LocUnStConfigRecMonLocId +
                                            "' AND STACK_NAME LIKE 'MS%' AND STACK_NAME <> '" + UnitStackPipeId + "'");
                                        string UnStRecStPpLoc;
                                        foreach (DataRowView drvUSConfig in UnStConfigRecs)
                                        {
                                            RATASumRecs.RowFilter = OldFilter1;
                                            decimal OtherSumRefMethFlow = 0;
                                            int OtherRunUsedCt = 0;
                                            UnStRecStPpLoc = cDBConvert.ToString(drvUSConfig["STACK_PIPE_MON_LOC_ID"]);
                                            RATASumRecs.RowFilter = AddToDataViewFilter(OldFilter1, "MON_LOC_ID = '" + UnStRecStPpLoc + "' AND SYS_TYPE_CD = 'FLOW' AND END_DATE <= '" +
                                                F2LEndDate.AddDays(30).ToShortDateString() + "' AND END_DATE >= '" + F2LEndDate.AddDays(-30).ToShortDateString() +
                                                "' AND TEST_RESULT_CD LIKE 'PASS%' AND OP_LEVEL_CD = '" + OpLvlCd + "'");
                                            if (RATASumRecs.Count == 1)
                                            {
                                                RATARunRecs.RowFilter = AddToDataViewFilter(OldFilter2, "RATA_SUM_ID = '" + cDBConvert.ToString(RATASumRecs[0]["RATA_SUM_ID"]) +
                                                    "' AND RUN_STATUS_CD = 'RUNUSED'");
                                                foreach (DataRowView drvRun2 in RATARunRecs)
                                                {
                                                    GrossUnitLd = cDBConvert.ToDecimal(drvRun2["GROSS_UNIT_LOAD"]);
                                                    RATARefVal = cDBConvert.ToDecimal(drvRun2["RATA_REF_VALUE"]);
                                                    CalcRATARefVal = cDBConvert.ToDecimal(drvRun2["CALC_RATA_REF_VALUE"]);
                                                    if (GrossUnitLd > 0 && RATARefVal > 0)
                                                    {
                                                        OtherRunUsedCt++;
                                                        SumGrossUnitLoad += GrossUnitLd;
                                                        OtherSumRefMethFlow += RATARefVal;
                                                    }
                                                    else
                                                    {
                                                        Category.CheckCatalogResult = "C";
                                                        break;
                                                    }
                                                }
                                                if (string.IsNullOrEmpty(Category.CheckCatalogResult))
                                                    if (OtherRunUsedCt < 9)
                                                    {
                                                        Category.CheckCatalogResult = "C";
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        RunUsedCt += OtherRunUsedCt;
                                                        SumRefMethFlow += OtherSumRefMethFlow / OtherRunUsedCt;
                                                    }
                                            }
                                            else
                                            {
                                                Category.CheckCatalogResult = "D";
                                                break;
                                            }
                                        }
                                    }
                                    if (string.IsNullOrEmpty(Category.CheckCatalogResult))
                                    {
                                        decimal F2LCalcGUL = Math.Round(SumGrossUnitLoad / RunUsedCt, MidpointRounding.AwayFromZero);
                                        Category.SetCheckParameter("F2L_Calc_GUL", F2LCalcGUL, eParameterDataType.Decimal);
                                        decimal CalcFlow = Math.Round(SumRefMethFlow, MidpointRounding.AwayFromZero);
                                        Category.SetCheckParameter("F2L_Calc_Flow", CalcFlow, eParameterDataType.Decimal);
                                        if (CurrentFlowToLoadReference["REF_GHR"] != DBNull.Value)
                                            Category.SetCheckParameter("F2L_Calc_GHR", Math.Round(cDBConvert.ToDecimal(CurrentFlowToLoadReference["AVG_HRLY_HI_RATE"]) / F2LCalcGUL * 1000, MidpointRounding.AwayFromZero), eParameterDataType.Decimal);
                                        else
                                        {
                                            decimal CalcRatio = Math.Round(CalcFlow / F2LCalcGUL / 100000, 2, MidpointRounding.AwayFromZero);
                                            Category.SetCheckParameter("F2L_Calc_Ratio", CalcRatio, eParameterDataType.Decimal);
                                        }
                                    }
                                }
                            }
                            RATARunRecs.RowFilter = OldFilter2;
                        }
                    }
                    RATASumRecs.RowFilter = OldFilter1;
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "F2LREF16");
            }

            return ReturnVal;
        }

        #endregion
    }
}
