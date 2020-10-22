using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;
using ECMPS.Definitions.Extensions;

namespace ECMPS.Checks.FF2LBASChecks
{
    public class cFF2LBASChecks : cChecks
    {
        #region Constructors

        public cFF2LBASChecks()
        {
            CheckProcedures = new dCheckProcedure[21];

            CheckProcedures[1] = new dCheckProcedure(FF2LBAS1);
            CheckProcedures[2] = new dCheckProcedure(FF2LBAS2);
            CheckProcedures[3] = new dCheckProcedure(FF2LBAS3);
            CheckProcedures[4] = new dCheckProcedure(FF2LBAS4);
            CheckProcedures[5] = new dCheckProcedure(FF2LBAS5);
            CheckProcedures[6] = new dCheckProcedure(FF2LBAS6);
            CheckProcedures[7] = new dCheckProcedure(FF2LBAS7);
            CheckProcedures[8] = new dCheckProcedure(FF2LBAS8);
            CheckProcedures[9] = new dCheckProcedure(FF2LBAS9);
            CheckProcedures[10] = new dCheckProcedure(FF2LBAS10);
            CheckProcedures[11] = new dCheckProcedure(FF2LBAS11);
            CheckProcedures[12] = new dCheckProcedure(FF2LBAS12);
            CheckProcedures[13] = new dCheckProcedure(FF2LBAS13);
            CheckProcedures[14] = new dCheckProcedure(FF2LBAS14);
            CheckProcedures[15] = new dCheckProcedure(FF2LBAS15);
            CheckProcedures[16] = new dCheckProcedure(FF2LBAS16);
            CheckProcedures[17] = new dCheckProcedure(FF2LBAS17);
            CheckProcedures[18] = new dCheckProcedure(FF2LBAS18);
            CheckProcedures[19] = new dCheckProcedure(FF2LBAS19);
            CheckProcedures[20] = new dCheckProcedure(FF2LBAS20);            
        }

        #endregion


        #region FF2LBASChecks Checks

        public static string FF2LBAS1(cCategory Category, ref bool Log)
        //Fuel Flow to Load Baseline System Type Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentFuelFlowLoadBaseline = (DataRowView)Category.GetCheckParameter("Current_Fuel_Flow_To_Load_Baseline").ParameterValue;
                if (CurrentFuelFlowLoadBaseline["MON_SYS_ID"] == DBNull.Value)
                {
                    Category.SetCheckParameter("FF2LBAS_System_Valid", false, eParameterDataType.Boolean);
                    Category.CheckCatalogResult = "A";
                }
                else
                    if( cDBConvert.ToString( CurrentFuelFlowLoadBaseline["SYS_TYPE_CD"] ).InList( "OILV,OILM,GAS,LTOL,LTGS" ) )
                        Category.SetCheckParameter("FF2LBAS_System_Valid", true, eParameterDataType.Boolean);
                    else
                    {
                        Category.SetCheckParameter("FF2LBAS_System_Valid", false, eParameterDataType.Boolean);
                        Category.CheckCatalogResult = "B";
                    }
            }

            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "FF2LBAS1");
            }

            return ReturnVal;
        }

        public static string FF2LBAS2(cCategory Category, ref bool Log)
        //Identification of Previously Reported Test or Test Number for Fuel Flow to Load Baseline Data
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentFuelFlowLoadBaseline = (DataRowView)Category.GetCheckParameter("Current_Fuel_Flow_To_Load_Baseline").ParameterValue;
                string MonSysID = cDBConvert.ToString(CurrentFuelFlowLoadBaseline["MON_SYS_ID"]);
                if (Convert.ToBoolean(Category.GetCheckParameter("Test_End_Date_Valid").ParameterValue) &&
                    Convert.ToBoolean(Category.GetCheckParameter("Test_End_Hour_Valid").ParameterValue) && MonSysID != "")
                {
                    DateTime EndDate = cDBConvert.ToDate(CurrentFuelFlowLoadBaseline["END_DATE"], DateTypes.END);
                    int EndHour = cDBConvert.ToHour(CurrentFuelFlowLoadBaseline["END_HOUR"], DateTypes.END);
                    string TestNumber = cDBConvert.ToString(CurrentFuelFlowLoadBaseline["TEST_NUM"]);
                    DataView FF2LBASRecords = (DataView)Category.GetCheckParameter("Fuel_Flow_To_Load_Baseline_Records").ParameterValue;
                    string OldFilter1 = FF2LBASRecords.RowFilter;
                    FF2LBASRecords.RowFilter = AddToDataViewFilter(OldFilter1, "END_DATE = '" + EndDate.ToShortDateString() + "'" +
                        " AND END_HOUR = " + EndHour);
                    if ((FF2LBASRecords.Count > 0 && CurrentFuelFlowLoadBaseline["TEST_SUM_ID"] == DBNull.Value) ||
                        (FF2LBASRecords.Count > 1 && CurrentFuelFlowLoadBaseline["TEST_SUM_ID"] != DBNull.Value) ||
                        (FF2LBASRecords.Count == 1 && CurrentFuelFlowLoadBaseline["TEST_SUM_ID"] != DBNull.Value && CurrentFuelFlowLoadBaseline["TEST_SUM_ID"].ToString() != FF2LBASRecords[0]["TEST_SUM_ID"].ToString()))
                        Category.CheckCatalogResult = "A";
                    else
                    {
                        DataView QASuppRecs = (DataView)Category.GetCheckParameter("QA_Supplemental_Data_Records").ParameterValue;
                        string OldFilter2 = QASuppRecs.RowFilter;
                        QASuppRecs.RowFilter = AddToDataViewFilter(OldFilter2, "TEST_TYPE_CD = 'FF2LBAS'" + " AND MON_SYS_ID = '" + MonSysID +
                            "'" + " AND END_DATE = '" + EndDate.ToShortDateString() + "'" + " AND END_HOUR = " + EndHour + " AND TEST_NUM <> '" + TestNumber + "'");
                        if ((QASuppRecs.Count > 0 && CurrentFuelFlowLoadBaseline["TEST_SUM_ID"] == DBNull.Value) ||
                            (QASuppRecs.Count > 1 && CurrentFuelFlowLoadBaseline["TEST_SUM_ID"] != DBNull.Value) ||
                            (QASuppRecs.Count == 1 && CurrentFuelFlowLoadBaseline["TEST_SUM_ID"] != DBNull.Value && CurrentFuelFlowLoadBaseline["TEST_SUM_ID"].ToString() != QASuppRecs[0]["TEST_SUM_ID"].ToString()))
                            Category.CheckCatalogResult = "A";
                        else
                        {
                            QASuppRecs.RowFilter = AddToDataViewFilter(OldFilter2, "TEST_TYPE_CD = 'FF2LBAS'" + " AND TEST_NUM = '" + TestNumber + "'");
                            if (QASuppRecs.Count > 0)
                            {
                                if (cDBConvert.ToString(((DataRowView)QASuppRecs[0])["CAN_SUBMIT"]) == "N")
                                {
                                    QASuppRecs.RowFilter = AddToDataViewFilter(QASuppRecs.RowFilter, "MON_SYS_ID <> '" + MonSysID + "'" + " OR END_DATE <> '" + EndDate.ToShortDateString() + "' " +
                                        " OR END_HOUR <> " + EndHour);
                                    if ((QASuppRecs.Count > 0 && CurrentFuelFlowLoadBaseline["TEST_SUM_ID"] == DBNull.Value) ||
                                        (QASuppRecs.Count > 1 && CurrentFuelFlowLoadBaseline["TEST_SUM_ID"] != DBNull.Value) ||
                                        (QASuppRecs.Count == 1 && CurrentFuelFlowLoadBaseline["TEST_SUM_ID"] != DBNull.Value && CurrentFuelFlowLoadBaseline["TEST_SUM_ID"].ToString() != QASuppRecs[0]["TEST_SUM_ID"].ToString()))
                                        Category.CheckCatalogResult = "B";
                                    else
                                        Category.CheckCatalogResult = "C";
                                }
                            }
                        }
                        QASuppRecs.RowFilter = OldFilter2;
                    }
                    FF2LBASRecords.RowFilter = OldFilter1;
                }
                else
                    Log = false;
            }

            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "FF2LBAS2");
            }

            return ReturnVal;
        }

        public static string FF2LBAS3(cCategory Category, ref bool Log)
        //Fuel Flow to Load Baseline Accuracy Test Number Valid
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("FF2LBAS_Accuracy_Test_Baseline_Start_Date", null, eParameterDataType.Date);
                Category.SetCheckParameter("FF2LBAS_Accuracy_Test_Baseline_Start_Hour", null, eParameterDataType.Integer);
                Category.SetCheckParameter("FF2LBAS_PEI_Required", false, eParameterDataType.Boolean);
                DataRowView CurrentFuelFlowLoadBaseline = (DataRowView)Category.GetCheckParameter("Current_Fuel_Flow_To_Load_Baseline").ParameterValue;
                string AccTestNum = cDBConvert.ToString(CurrentFuelFlowLoadBaseline["ACCURACY_TEST_NUMBER"]);
                if (AccTestNum == "")
                    Category.CheckCatalogResult = "A";
                else
                {
                    DataView QASuppRecs = (DataView)Category.GetCheckParameter("QA_Supplemental_Data_Records").ParameterValue;
                    string OldFilter1 = QASuppRecs.RowFilter;
                    DataView LocTestRecs = (DataView)Category.GetCheckParameter("Location_Test_Records").ParameterValue;
                    string OldFilter2 = LocTestRecs.RowFilter;                    
                    DataRowView FoundRec = null;
                    QASuppRecs.RowFilter = AddToDataViewFilter(OldFilter1, "(TEST_TYPE_CD = 'FFACC' OR TEST_TYPE_CD = 'FFACCTT')" +
                        " AND TEST_NUM = '" + AccTestNum + "'" + " AND CAN_SUBMIT = 'N'");
                    if (QASuppRecs.Count == 0)
                    {
                        LocTestRecs.RowFilter = AddToDataViewFilter(OldFilter2, "(TEST_TYPE_CD = 'FFACC' OR TEST_TYPE_CD = 'FFACCTT')" +
                            " AND TEST_NUM = '" + AccTestNum + "'");
                        if (LocTestRecs.Count == 0)
                            Category.CheckCatalogResult = "B";
                        else
                            FoundRec = (DataRowView)LocTestRecs[0];                        
                    }
                    else
                        FoundRec = (DataRowView)QASuppRecs[0];                    

                    if (FoundRec != null)//Found a QA Supp Record or Location Test Record
                    {
                        DataView SysSysCompRecs = (DataView)Category.GetCheckParameter("System_System_Component_Records").ParameterValue;
                        string OldFilter3 = SysSysCompRecs.RowFilter;
                        SysSysCompRecs.RowFilter = AddToDataViewFilter(OldFilter3, "COMPONENT_ID = '" + cDBConvert.ToString(FoundRec["COMPONENT_ID"]) + "'");
                        if (SysSysCompRecs.Count == 0)
                            Category.CheckCatalogResult = "C";
                        else
                        {
                            if (cDBConvert.ToString(FoundRec["TEST_TYPE_CD"]) == "FFACCTT" || cDBConvert.ToString(SysSysCompRecs[0]["ACQ_CD"]).InList("NOZ,ORF,VEN"))
                                Category.SetCheckParameter("FF2LBAS_PEI_Required", true, eParameterDataType.Boolean);
                            DateTime ReinstallDate = cDBConvert.ToDate(FoundRec["REINSTALL_DATE"], DateTypes.START);
                            if (ReinstallDate == DateTime.MinValue)
                            {
                                DateTime EndDate = cDBConvert.ToDate(FoundRec["END_DATE"], DateTypes.END);
                                int EndHour = cDBConvert.ToHour(FoundRec["END_HOUR"], DateTypes.END);
                                Category.SetCheckParameter("FF2LBAS_Accuracy_Test_Baseline_Start_Date", EndDate, eParameterDataType.Date);
                                Category.SetCheckParameter("FF2LBAS_Accuracy_Test_Baseline_Start_Hour", EndHour, eParameterDataType.Integer);
                            }
                            else
                            {
                                int ReinstallHour = cDBConvert.ToHour(FoundRec["REINSTALL_HOUR"], DateTypes.START);
                                Category.SetCheckParameter("FF2LBAS_Accuracy_Test_Baseline_Start_Date", ReinstallDate, eParameterDataType.Date);
                                Category.SetCheckParameter("FF2LBAS_Accuracy_Test_Baseline_Start_Hour", ReinstallHour, eParameterDataType.Integer);
                            }
                        }
                        SysSysCompRecs.RowFilter = OldFilter3;
                    }
                    LocTestRecs.RowFilter = OldFilter2;
                    QASuppRecs.RowFilter = OldFilter1;
                }
            }

            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "FF2LBAS3");
            }

            return ReturnVal;
        }

        public static string FF2LBAS4(cCategory Category, ref bool Log)
        //Fuel Flow to Load Baseline Method Valid
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("FF2LBAS_Method_Valid", true, eParameterDataType.Boolean);
                DataRowView CurrentFuelFlowLoadBaseline = (DataRowView)Category.GetCheckParameter("Current_Fuel_Flow_To_Load_Baseline").ParameterValue;
                if (CurrentFuelFlowLoadBaseline["BASELINE_FUEL_FLOW_LOAD_RATIO"] != DBNull.Value)
                {
                    if (CurrentFuelFlowLoadBaseline["BASELINE_GHR"] != DBNull.Value)
                    {
                        Category.SetCheckParameter("FF2LBAS_Method_Valid", false, eParameterDataType.Boolean);
                        Category.CheckCatalogResult = "A";
                    }
                }
                else
                    if (CurrentFuelFlowLoadBaseline["BASELINE_GHR"] == DBNull.Value)
                    {
                        Category.SetCheckParameter("FF2LBAS_Method_Valid", false, eParameterDataType.Boolean);
                        Category.CheckCatalogResult = "B";
                    }
            }

            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "FF2LBAS4");
            }

            return ReturnVal;
        }

        public static string FF2LBAS5(cCategory Category, ref bool Log)
        //Fuel Flow to Load Baseline Duration Valid
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("FF2LBAS_Collection_Period_Hours", null, eParameterDataType.Decimal);
                if (Convert.ToBoolean(Category.GetCheckParameter("Test_Dates_Consistent").ParameterValue))
                {
                    DataRowView CurrentFuelFlowLoadBaseline = (DataRowView)Category.GetCheckParameter("Current_Fuel_Flow_To_Load_Baseline").ParameterValue;
                    DateTime BeginDate = cDBConvert.ToDate(CurrentFuelFlowLoadBaseline["BEGIN_DATE"], DateTypes.END);
                    DateTime EndDate = cDBConvert.ToDate(CurrentFuelFlowLoadBaseline["END_DATE"], DateTypes.END);
                    if (Convert.ToBoolean(Category.GetCheckParameter("Test_Dates_Consistent").ParameterValue))
                    {
                        int BeginQuarter = (BeginDate.Month-1)/3 + 1;
                        int EndQuarter = (EndDate.Month-1)/3 + 1;                        
                        
                        if (((EndDate.Year - BeginDate.Year) * 4 + (EndQuarter - BeginQuarter)) > 4)
                            Category.CheckCatalogResult = "A";
                        else
                        {
                            int BeginHour = cDBConvert.ToHour(CurrentFuelFlowLoadBaseline["BEGIN_HOUR"], DateTypes.START);
                            int EndHour = cDBConvert.ToHour(CurrentFuelFlowLoadBaseline["END_HOUR"], DateTypes.END);
                            DateTime PreciseBeginDate = DateTime.Parse(string.Format("{0} {1}:00", BeginDate.ToShortDateString(), BeginHour));
                            DateTime PreciseEndDate = DateTime.Parse(string.Format("{0} {1}:00", EndDate.ToShortDateString(), EndHour));
                            TimeSpan Span = PreciseEndDate.Subtract(PreciseBeginDate);
                            int Hours = (int)Span.TotalHours + 1;//add 1 for inclusivity
                            Category.SetCheckParameter("FF2LBAS_Collection_Period_Hours", Hours, eParameterDataType.Decimal);
                        }
                    }
                }
                else
                    Log = false;
            }

            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "FF2LBAS5");
            }

            return ReturnVal;
        }

        public static string FF2LBAS6(cCategory Category, ref bool Log)
        //Fuel Flow to Load Baseline PEI Test Number Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentFuelFlowLoadBaseline = (DataRowView)Category.GetCheckParameter("Current_Fuel_Flow_To_Load_Baseline").ParameterValue;
                if (CurrentFuelFlowLoadBaseline["PEI_TEST_NUMBER"] == DBNull.Value)
                {
                    DateTime BaseDate = Convert.ToDateTime(Category.GetCheckParameter("FF2LBAS_Accuracy_Test_Baseline_Start_Date").ParameterValue);
                    int BaseHour = Convert.ToInt32(Category.GetCheckParameter("FF2LBAS_Accuracy_Test_Baseline_Start_Hour").ParameterValue);
                    if (BaseDate == DateTime.MinValue)
                        Category.SetCheckParameter("FF2LBAS_Baseline_Start_Date", null, eParameterDataType.Date);
                    else
                        Category.SetCheckParameter("FF2LBAS_Baseline_Start_Date", BaseDate, eParameterDataType.Date);
                    if (Category.GetCheckParameter("FF2LBAS_Accuracy_Test_Baseline_Start_Hour").ParameterValue == null)
                        Category.SetCheckParameter("FF2LBAS_Baseline_Start_Hour", null, eParameterDataType.Integer);
                    else
                        Category.SetCheckParameter("FF2LBAS_Baseline_Start_Hour", Convert.ToInt32(Category.GetCheckParameter("FF2LBAS_Accuracy_Test_Baseline_Start_Hour").ParameterValue), eParameterDataType.Integer);
                    if (Convert.ToBoolean(Category.GetCheckParameter("FF2LBAS_PEI_Required").ParameterValue))
                        Category.CheckCatalogResult = "A";
                }
                else
                {
                    Category.SetCheckParameter("FF2LBAS_Baseline_Start_Date", null, eParameterDataType.Date);
                    Category.SetCheckParameter("FF2LBAS_Baseline_Start_Hour", null, eParameterDataType.Integer);
                    DataView QASuppRecs = (DataView)Category.GetCheckParameter("QA_Supplemental_Data_Records").ParameterValue;
                    string OldFilter1 = QASuppRecs.RowFilter;
                    DataView LocTestRecs = (DataView)Category.GetCheckParameter("Location_Test_Records").ParameterValue;
                    string OldFilter2 = LocTestRecs.RowFilter;                    
                    string PEITestNum = cDBConvert.ToString(CurrentFuelFlowLoadBaseline["PEI_TEST_NUMBER"]);
                    DataRowView FoundRec = null;
                    QASuppRecs.RowFilter = AddToDataViewFilter(OldFilter1, "TEST_TYPE_CD = 'PEI'"  + " AND TEST_NUM = '" + PEITestNum + "'" + " AND CAN_SUBMIT = 'N'");
                    if (QASuppRecs.Count == 0)
                    {
                        LocTestRecs.RowFilter = AddToDataViewFilter(OldFilter2, "TEST_TYPE_CD = 'PEI'" + " AND TEST_NUM = '" + PEITestNum + "'");
                        if (LocTestRecs.Count == 0)
                            Category.CheckCatalogResult = "B";
                        else
                            FoundRec = (DataRowView)LocTestRecs[0];
                    }
                    else
                        FoundRec = (DataRowView)QASuppRecs[0];
                    if (FoundRec != null)//Found a QASupp or Location Test Record
                    {
                        DataView SysSysCompRecs = (DataView)Category.GetCheckParameter("System_System_Component_Records").ParameterValue;
                        string OldFilter3 = SysSysCompRecs.RowFilter;
                        SysSysCompRecs.RowFilter = AddToDataViewFilter(OldFilter3, "COMPONENT_ID = '" + cDBConvert.ToString(FoundRec["COMPONENT_ID"]) + "'");
                        if (SysSysCompRecs.Count == 0)
                            Category.CheckCatalogResult = "C";
                        else
                        {
                            DateTime EndDate = cDBConvert.ToDate(FoundRec["END_DATE"], DateTypes.END);
                            int EndHour = cDBConvert.ToHour(FoundRec["END_HOUR"], DateTypes.END);
                            DateTime AccTestStartDate = Convert.ToDateTime(Category.GetCheckParameter("FF2LBAS_Accuracy_Test_Baseline_Start_Date").ParameterValue);
                            int AccTestStartHour = Convert.ToInt32(Category.GetCheckParameter("FF2LBAS_Accuracy_Test_Baseline_Start_Hour").ParameterValue);
                            if (AccTestStartDate > EndDate || (AccTestStartDate == EndDate && AccTestStartHour > EndHour))
                            {
                                Category.SetCheckParameter("FF2LBAS_Baseline_Start_Date", AccTestStartDate, eParameterDataType.Date);
                                Category.SetCheckParameter("FF2LBAS_Baseline_Start_Hour", AccTestStartHour, eParameterDataType.Integer);
                            }
                            else
                            {
                                Category.SetCheckParameter("FF2LBAS_Baseline_Start_Date", cDBConvert.ToDate(FoundRec["END_DATE"], DateTypes.END), eParameterDataType.Date);
                                Category.SetCheckParameter("FF2LBAS_Baseline_Start_Hour", cDBConvert.ToHour(FoundRec["END_HOUR"], DateTypes.END), eParameterDataType.Integer);
                            }
                            DateTime SysRecBeginDate = cDBConvert.ToDate(SysSysCompRecs[0]["BEGIN_DATE"], DateTypes.START);
                            int SysRecBeginHour = cDBConvert.ToHour(SysSysCompRecs[0]["BEGIN_HOUR"], DateTypes.START);
                            DateTime BaselineStartDate = Convert.ToDateTime(Category.GetCheckParameter("FF2LBAS_Accuracy_Test_Baseline_Start_Date").ParameterValue);
                            int BaselineStartHour = Convert.ToInt32(Category.GetCheckParameter("FF2LBAS_Accuracy_Test_Baseline_Start_Hour").ParameterValue);
                            if (SysRecBeginDate > BaselineStartDate || (SysRecBeginDate == BaselineStartDate && SysRecBeginHour > BaselineStartHour))
                                Category.CheckCatalogResult = "D";
                        }
                        SysSysCompRecs.RowFilter = OldFilter3;
                    }
                    QASuppRecs.RowFilter = OldFilter1;
                    LocTestRecs.RowFilter = OldFilter2;
                }
            }

            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "FF2LBAS6");
            }

            return ReturnVal;
        }

        public static string FF2LBAS7(cCategory Category, ref bool Log)
        //Fuel Flow to Load Baseline Average Load Valid
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("FF2LBAS_Load_UOM_Code", null, eParameterDataType.String);
                DataRowView CurrentFuelFlowLoadBaseline = (DataRowView)Category.GetCheckParameter("Current_Fuel_Flow_To_Load_Baseline").ParameterValue;
                if (CurrentFuelFlowLoadBaseline["AVG_LOAD"] == DBNull.Value)
                    Category.CheckCatalogResult = "A";
                else
                {
                    int AvgLoad = cDBConvert.ToInteger(CurrentFuelFlowLoadBaseline["AVG_LOAD"]);
                    if (AvgLoad <= 0)
                        Category.CheckCatalogResult = "B";
                    else
                    {
                        if (Convert.ToBoolean(Category.GetCheckParameter("Test_Dates_Consistent").ParameterValue))
                        {
                            DataView LoadRecs = (DataView)Category.GetCheckParameter("Load_Records").ParameterValue;
                            string OldFilter = LoadRecs.RowFilter;
                            DateTime CurrentEndDate = cDBConvert.ToDate(CurrentFuelFlowLoadBaseline["END_DATE"], DateTypes.END);
                            int CurrentEndHour = cDBConvert.ToHour(CurrentFuelFlowLoadBaseline["END_HOUR"], DateTypes.END);
                            LoadRecs.RowFilter = AddToDataViewFilter(OldFilter, "(BEGIN_DATE < '" + CurrentEndDate.ToShortDateString() + "'" +
                                " OR (BEGIN_DATE = '" + CurrentEndDate.ToShortDateString() + "'" + " AND BEGIN_HOUR < " + (CurrentEndHour + 1) + "))" +
                                " AND (END_DATE IS NULL OR (END_DATE > '" + CurrentEndDate.ToShortDateString() + "'" + " OR (END_DATE = '" +
                                CurrentEndDate.ToShortDateString() + "'" + " AND END_HOUR > " + (CurrentEndHour - 1) + ")))");
                            if (LoadRecs.Count == 0)
                                Category.CheckCatalogResult = "C";
                            else
                            {
                                string OldSort = LoadRecs.Sort;
                                LoadRecs.Sort = "MAX_LOAD_VALUE DESC";
                                DataRowView LatestLoadRec = (DataRowView)LoadRecs[0];
                                Category.SetCheckParameter("FF2LBAS_Load_UOM_Code", cDBConvert.ToString(LatestLoadRec["MAX_LOAD_UOM_CD"]), eParameterDataType.String);
                                if (AvgLoad > cDBConvert.ToInteger(LatestLoadRec["MAX_LOAD_VALUE"]))
                                    Category.CheckCatalogResult = "D";
                                LoadRecs.Sort = OldSort;
                            }
                            LoadRecs.RowFilter = OldFilter;
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "FF2LBAS7");
            }

            return ReturnVal;
        }

        public static string FF2LBAS8(cCategory Category, ref bool Log)
        //Fuel Flow to Load Baseline Avg Hourly Heat Input Rate Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentFuelFlowLoadBaseline = (DataRowView)Category.GetCheckParameter("Current_Fuel_Flow_To_Load_Baseline").ParameterValue;
                decimal AvgHHIR = cDBConvert.ToDecimal(CurrentFuelFlowLoadBaseline["AVG_HRLY_HI_RATE"]);

                if (Convert.ToBoolean(Category.GetCheckParameter("FF2LBAS_Method_Valid").ParameterValue) && AvgHHIR != decimal.MinValue)
                {
                    if (AvgHHIR <= 0)
                        Category.CheckCatalogResult = "A";
                }
                else
                    Log = false;
            }

            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "FF2LBAS8");
            }

            return ReturnVal;
        }

        public static string FF2LBAS9(cCategory Category, ref bool Log)
        //Fuel Flow to Load Baseline Heat Input Rate Consistent with Maximum Heat Input
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentFuelFlowLoadBaseline = (DataRowView)Category.GetCheckParameter("Current_Fuel_Flow_To_Load_Baseline").ParameterValue;
                decimal AvgHHIR = cDBConvert.ToDecimal(CurrentFuelFlowLoadBaseline["AVG_HRLY_HI_RATE"]);
                if (Convert.ToBoolean(Category.GetCheckParameter("FF2LBAS_Method_Valid").ParameterValue) &&
                    Convert.ToBoolean(Category.GetCheckParameter("Test_Dates_Consistent").ParameterValue) &&
                    AvgHHIR > 0)
                {
                    DateTime BeginDate = cDBConvert.ToDate(CurrentFuelFlowLoadBaseline["BEGIN_DATE"], DateTypes.END);
                    DateTime EndDate = cDBConvert.ToDate(CurrentFuelFlowLoadBaseline["END_DATE"], DateTypes.END);
                    DataView UnitStackRecs = (DataView)Category.GetCheckParameter("Unit_Stack_Configuration_Records").ParameterValue;
                    string OldFilter1 = UnitStackRecs.RowFilter;
                    UnitStackRecs.RowFilter = AddToDataViewFilter(OldFilter1, "BEGIN_DATE < '" + EndDate.AddDays(1).ToShortDateString() + "'" + 
                        " AND (END_DATE IS NULL OR END_DATE > '" + BeginDate.AddDays(-1).ToShortDateString() + "')");
                    DataView UnitCapRecs = (DataView)Category.GetCheckParameter("Facility_Unit_Capacity_Records").ParameterValue;
                    string OldFilter2 = UnitCapRecs.RowFilter;
                    string OldSort = UnitCapRecs.Sort;
                    string LocId = cDBConvert.ToString(CurrentFuelFlowLoadBaseline["LOCATION_IDENTIFIER"]);
                    string UnitCapLoc = "";                    

                    if (LocId.StartsWith("MP"))
                    {
                        if (UnitStackRecs.Count > 0)//Delete this condition if assured that not needed
                            UnitCapLoc = cDBConvert.ToString(UnitStackRecs[0]["MON_LOC_ID"]);
                        UnitCapRecs.RowFilter = AddToDataViewFilter(OldFilter2, "MON_LOC_ID = '" + UnitCapLoc + "'" + " AND BEGIN_DATE < '" + EndDate.AddDays(1).ToShortDateString() + "'" +
                            " AND (END_DATE IS NULL OR END_DATE < '" + BeginDate.AddDays(1).ToShortDateString() + "')");
                        if (UnitCapRecs.Count == 0)
                            Category.CheckCatalogResult = "A";
                        else
                        {
                            UnitCapRecs.Sort = "MAX_HI_CAPACITY DESC";
                            if (AvgHHIR > cDBConvert.ToDecimal(UnitCapRecs[0]["MAX_HI_CAPACITY"]))
                                Category.CheckCatalogResult = "B";
                        }
                    }
                    else
                        if (LocId.StartsWith("CP"))
                        {
                            decimal TotalHI = 0;
                            int i = 0;                            
                            foreach (DataRowView drv in UnitStackRecs)
                            {
                                UnitCapLoc = cDBConvert.ToString(UnitStackRecs[i++]["MON_LOC_ID"]);//As above, this will throw runtime error unless UnitStackRecs.Count > 0                            
                                UnitCapRecs.RowFilter = AddToDataViewFilter(OldFilter2, "MON_LOC_ID = '" + UnitCapLoc + "'" + " AND BEGIN_DATE < '" + EndDate.AddDays(1).ToShortDateString() + "'" +
                                    " AND (END_DATE IS NULL OR END_DATE < '" + BeginDate.AddDays(1).ToShortDateString() + "')");
                                if (UnitCapRecs.Count == 0)
                                {
                                    Category.CheckCatalogResult = "A";
                                    break;
                                }
                                else
                                {
                                    UnitCapRecs.Sort = "MAX_HI_CAPACITY DESC";
                                    TotalHI += cDBConvert.ToDecimal(UnitCapRecs[0]["MAX_HI_CAPACITY"]);
                                }
                            }
                            if (AvgHHIR > TotalHI && string.IsNullOrEmpty(Category.CheckCatalogResult))
                                Category.CheckCatalogResult = "B";
                        }
                        else
                        {
                            UnitCapRecs.RowFilter = AddToDataViewFilter(OldFilter2, "MON_LOC_ID = '" + cDBConvert.ToString(CurrentFuelFlowLoadBaseline["MON_LOC_ID"]) + "'" +
                                " AND BEGIN_DATE < '" + EndDate.AddDays(1).ToShortDateString() + "'" +
                                " AND (END_DATE IS NULL OR END_DATE < '" + BeginDate.AddDays(1).ToShortDateString() + "')");
                            if (UnitCapRecs.Count == 0)
                                Category.CheckCatalogResult = "A";
                            else
                            {
                                UnitCapRecs.Sort = "MAX_HI_CAPACITY DESC";
                                if (AvgHHIR > cDBConvert.ToDecimal(UnitCapRecs[0]["MAX_HI_CAPACITY"]))
                                    Category.CheckCatalogResult = "B";
                            }
                        }
                    UnitCapRecs.RowFilter = OldFilter2;
                    UnitStackRecs.RowFilter = OldFilter1;
                }
                else
                    Log = false;
            }

            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "FF2LBAS9");
            }

            return ReturnVal;
        }

        public static string FF2LBAS10(cCategory Category, ref bool Log)
        //Fuel Flow to Load Baseline Average Fuel Flow Rate Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentFuelFlowLoadBaseline = (DataRowView)Category.GetCheckParameter("Current_Fuel_Flow_To_Load_Baseline").ParameterValue;
                decimal AvgFFRate = cDBConvert.ToDecimal(CurrentFuelFlowLoadBaseline["AVG_FUEL_FLOW_RATE"]);
                if (Convert.ToBoolean(Category.GetCheckParameter("FF2LBAS_Method_Valid").ParameterValue) && AvgFFRate != decimal.MinValue)
                {
                    if (AvgFFRate <= 0)
                        Category.CheckCatalogResult = "A";
                }
                else
                    Log = false;
            }

            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "FF2LBAS10");
            }

            return ReturnVal;
        }

        public static string FF2LBAS11(cCategory Category, ref bool Log)
        //Fuel Flow to Load Baseline Begin Time Valid
        {
            string ReturnVal = "";

            try
            {
                if (Convert.ToBoolean(Category.GetCheckParameter("Test_Begin_Date_Valid").ParameterValue) &&
                    Convert.ToBoolean(Category.GetCheckParameter("Test_Begin_Hour_Valid").ParameterValue))
                {
                    DateTime BaselineStartDate = Category.GetCheckParameter("FF2LBAS_Baseline_Start_Date").ValueAsDateTime(DateTypes.START);
                    if (BaselineStartDate != DateTime.MinValue)
                    {
                        DataRowView CurrentFuelFlowLoadBaseline = (DataRowView)Category.GetCheckParameter("Current_Fuel_Flow_To_Load_Baseline").ParameterValue;
                        DateTime BeginDate = cDBConvert.ToDate(CurrentFuelFlowLoadBaseline["BEGIN_DATE"], DateTypes.END);
                        int BeginHour = cDBConvert.ToHour(CurrentFuelFlowLoadBaseline["BEGIN_HOUR"], DateTypes.START);
                        
                        if (BeginDate < BaselineStartDate || (BeginDate == BaselineStartDate && 
                            BeginHour < Category.GetCheckParameter("FF2LBAS_Baseline_Start_Hour").ValueAsInt()))
                            Category.CheckCatalogResult = "A";
                    }
                    else
                        Log = false;
                }
                else
                    Log = false;
            }

            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "FF2LBAS11");
            }

            return ReturnVal;
        }

        public static string FF2LBAS12(cCategory Category, ref bool Log)
        //Fuel Flow to Load Baseline Number of Hours Excluded Cofiring Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentFuelFlowLoadBaseline = (DataRowView)Category.GetCheckParameter("Current_Fuel_Flow_To_Load_Baseline").ParameterValue;
                int NHECofiring = cDBConvert.ToInteger(CurrentFuelFlowLoadBaseline["NHE_COFIRING"]);
                if (NHECofiring != int.MinValue && NHECofiring < 0)
                    Category.CheckCatalogResult = "A";
            }

            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "FF2LBAS12");
            }

            return ReturnVal;
        }

        public static string FF2LBAS13(cCategory Category, ref bool Log)
        //Fuel Flow to Load Baseline Number of Hours Excluded Ramping Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentFuelFlowLoadBaseline = (DataRowView)Category.GetCheckParameter("Current_Fuel_Flow_To_Load_Baseline").ParameterValue;
                int NHERamping = cDBConvert.ToInteger(CurrentFuelFlowLoadBaseline["NHE_RAMPING"]);
                if (NHERamping != int.MinValue && NHERamping < 0)
                    Category.CheckCatalogResult = "A";
            }

            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "FF2LBAS13");
            }

            return ReturnVal;
        }

        public static string FF2LBAS14(cCategory Category, ref bool Log)
        //Fuel Flow to Load Baseline Number of Hours Excluded Low Range Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentFuelFlowLoadBaseline = (DataRowView)Category.GetCheckParameter("Current_Fuel_Flow_To_Load_Baseline").ParameterValue;
                int NHELowRange = cDBConvert.ToInteger(CurrentFuelFlowLoadBaseline["NHE_LOW_RANGE"]);
                if (NHELowRange != int.MinValue && NHELowRange < 0)
                    Category.CheckCatalogResult = "A";
                else
                {
                    int CollPerHrs = Convert.ToInt32(Category.GetCheckParameter("FF2LBAS_Collection_Period_Hours").ParameterValue);
                    if (CollPerHrs != int.MinValue)
                    {
                        int TempVal = 168;
                        int NHERamping = cDBConvert.ToInteger(CurrentFuelFlowLoadBaseline["NHE_RAMPING"]);
                        int NHECofiring = cDBConvert.ToInteger(CurrentFuelFlowLoadBaseline["NHE_COFIRING"]);
                        if (NHECofiring > 0)
                            TempVal += NHECofiring;
                        if (NHERamping > 0)
                            TempVal += NHERamping;
                        if (NHELowRange > 0)
                            TempVal += NHELowRange;
                        if (TempVal > CollPerHrs)
                            Category.CheckCatalogResult = "B";
                    }
                }
            }

            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "FF2LBAS14");
            }

            return ReturnVal;
        }

        public static string FF2LBAS15(cCategory Category, ref bool Log)
        //Fuel Flow to Load Baseline Base Fuel Flow to Load UOM Valid
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("FF2LBAS_Fuel_Flow_To_Load_Ratio_UOM_Code_Valid", false, eParameterDataType.Boolean);
                DataRowView CurrentFuelFlowLoadBaseline = (DataRowView)Category.GetCheckParameter("Current_Fuel_Flow_To_Load_Baseline").ParameterValue;                
                if (Convert.ToBoolean(Category.GetCheckParameter("FF2LBAS_Method_Valid").ParameterValue) &&
                    CurrentFuelFlowLoadBaseline["AVG_FUEL_FLOW_RATE"] != DBNull.Value)
                {
                    Category.SetCheckParameter("FF2LBAS_Fuel_Flow_To_Load_Ratio_UOM_Code_Valid", true, eParameterDataType.Boolean);
                    if (CurrentFuelFlowLoadBaseline["FUEL_FLOW_LOAD_UOM_CD"] == DBNull.Value)
                    {
                        Category.SetCheckParameter("FF2LBAS_Fuel_Flow_To_Load_Ratio_UOM_Code_Valid", false, eParameterDataType.Boolean);
                        Category.CheckCatalogResult = "A";
                    }
                    else
                    {
                        DataView ParamUnitsOfMeasure = (DataView)Category.GetCheckParameter("Parameter_Units_Of_Measure_Lookup_Table").ParameterValue;
                        string OldFilter1 = ParamUnitsOfMeasure.RowFilter;
                        string FF2LUOMCd = cDBConvert.ToString(CurrentFuelFlowLoadBaseline["FUEL_FLOW_LOAD_UOM_CD"]);
                        ParamUnitsOfMeasure.RowFilter = AddToDataViewFilter(OldFilter1, "PARAMETER_CD = 'FF2L' AND UOM_CD = '" + FF2LUOMCd + "'");
                        if (ParamUnitsOfMeasure.Count == 0)
                        {
                            Category.SetCheckParameter("FF2LBAS_Fuel_Flow_To_Load_Ratio_UOM_Code_Valid", false, eParameterDataType.Boolean);
                            Category.CheckCatalogResult = "B";
                        }
                        else
                        {
                            if (Category.GetCheckParameter("FF2LBAS_Load_UOM_Code").ParameterValue != null && CurrentFuelFlowLoadBaseline["MON_SYS_ID"] != DBNull.Value)
                            {
                                string LoadUOMCd = Convert.ToString(Category.GetCheckParameter("FF2LBAS_Load_UOM_Code").ParameterValue);
                                string SysTypeCd = cDBConvert.ToString(((DataRowView)Category.GetCheckParameter("Associated_System").ParameterValue)["SYS_TYPE_CD"]);
                                DataView SysTypeCrossCheckTbl = (DataView)Category.GetCheckParameter("Fuel_Flow_To_Load_Baseline_Uom_To_Load_Uom_And_Systemtype_Cross_Check_Table").ParameterValue;
                                string OldFilter2 = SysTypeCrossCheckTbl.RowFilter;
                                SysTypeCrossCheckTbl.RowFilter = AddToDataViewFilter(OldFilter2, "BaselineUOM = '" + FF2LUOMCd + "' AND LoadUOM = '" + LoadUOMCd + "' AND (SystemTypeList = '" + 
                                    SysTypeCd + "' OR SystemTypeList LIKE '" + SysTypeCd + ",%' OR SystemTypeList LIKE '%," + SysTypeCd + ",%' OR SystemTypeList LIKE '%," + SysTypeCd + "')");                                
                                if (SysTypeCrossCheckTbl.Count == 0)
                                {
                                    Category.SetCheckParameter("FF2LBAS_Fuel_Flow_To_Load_Ratio_UOM_Code_Valid", false, eParameterDataType.Boolean);
                                    Category.CheckCatalogResult = "C";
                                }
                                SysTypeCrossCheckTbl.RowFilter = OldFilter2;
                            }
                        }
                        ParamUnitsOfMeasure.RowFilter = OldFilter1;
                    }
                }
                else
                    Log = false;
            }

            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "FF2LBAS15");
            }

            return ReturnVal;
        }

        public static string FF2LBAS16(cCategory Category, ref bool Log)
        //Fuel Flow to Load Baseline Base GHR UOM Valid
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("FF2LBAS_GHR_UOM_Code_Valid", false, eParameterDataType.Boolean);
                DataRowView CurrentFuelFlowLoadBaseline = (DataRowView)Category.GetCheckParameter("Current_Fuel_Flow_To_Load_Baseline").ParameterValue;
                if (Convert.ToBoolean(Category.GetCheckParameter("FF2LBAS_Method_Valid").ParameterValue) && CurrentFuelFlowLoadBaseline["AVG_HRLY_HI_RATE"] != DBNull.Value)
                {
                    Category.SetCheckParameter("FF2LBAS_GHR_UOM_Code_Valid", true, eParameterDataType.Boolean);
                    if (CurrentFuelFlowLoadBaseline["GHR_UOM_CD"] == DBNull.Value)
                    {
                        Category.SetCheckParameter("FF2LBAS_GHR_UOM_Code_Valid", false, eParameterDataType.Boolean);
                        Category.CheckCatalogResult = "A";
                    }
                    else
                    {
                        DataView ParamUnitsOfMeasure = (DataView)Category.GetCheckParameter("Parameter_Units_Of_Measure_Lookup_Table").ParameterValue;
                        string OldFilter1 = ParamUnitsOfMeasure.RowFilter;
                        string GHRUOMCd = cDBConvert.ToString(CurrentFuelFlowLoadBaseline["GHR_UOM_CD"]);
                        ParamUnitsOfMeasure.RowFilter = AddToDataViewFilter(OldFilter1, "PARAMETER_CD = 'GHR' AND UOM_CD = '" + GHRUOMCd + "'");
                        if (ParamUnitsOfMeasure.Count == 0)
                        {
                            Category.SetCheckParameter("FF2LBAS_GHR_UOM_Code_Valid", false, eParameterDataType.Boolean);
                            Category.CheckCatalogResult = "B";
                        }
                        else
                        {
                            if (Category.GetCheckParameter("FF2LBAS_Load_UOM_Code").ParameterValue != null)
                            {
                                string LoadUOMCd = Convert.ToString(Category.GetCheckParameter("FF2LBAS_Load_UOM_Code").ParameterValue);
                                string SysTypeCd = cDBConvert.ToString(((DataRowView)Category.GetCheckParameter("Associated_System").ParameterValue)["SYS_TYPE_CD"]);
                                DataView SysTypeCrossCheckTbl = (DataView)Category.GetCheckParameter("Fuel_Flow_To_Load_Baseline_Uom_To_Load_Uom_And_Systemtype_Cross_Check_Table").ParameterValue;
                                string OldFilter2 = SysTypeCrossCheckTbl.RowFilter;
                                SysTypeCrossCheckTbl.RowFilter = AddToDataViewFilter(OldFilter2, "BaselineUOM = '" + GHRUOMCd + "' AND LoadUOM = '" + LoadUOMCd + "'");
                                if (SysTypeCrossCheckTbl.Count == 0)
                                {
                                    Category.SetCheckParameter("FF2LBAS_GHR_UOM_Code_Valid", false, eParameterDataType.Boolean);
                                    Category.CheckCatalogResult = "C";
                                }
                                SysTypeCrossCheckTbl.RowFilter = OldFilter2;
                            }
                        }
                        ParamUnitsOfMeasure.RowFilter = OldFilter1;
                    }
                }
                else
                    Log = false;
            }

            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "FF2LBAS16");
            }

            return ReturnVal;
        }

        public static string FF2LBAS17(cCategory Category, ref bool Log)
        //Fuel Flow to Load Baseline Fuel Flow to Load Ratio Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentFuelFlowLoadBaseline = (DataRowView)Category.GetCheckParameter("Current_Fuel_Flow_To_Load_Baseline").ParameterValue;
                decimal AvgFFRate = cDBConvert.ToDecimal(CurrentFuelFlowLoadBaseline["AVG_FUEL_FLOW_RATE"]);
                if (Convert.ToBoolean(Category.GetCheckParameter("FF2LBAS_Method_Valid").ParameterValue) && AvgFFRate != decimal.MinValue)
                {
                    decimal BaseFF2LRatio = cDBConvert.ToDecimal(CurrentFuelFlowLoadBaseline["BASELINE_FUEL_FLOW_LOAD_RATIO"]);
                    if (BaseFF2LRatio == decimal.MinValue)
                        Category.CheckCatalogResult = "A";
                    else
                        if (BaseFF2LRatio <= 0)
                            Category.CheckCatalogResult = "B";

                    if (string.IsNullOrEmpty(Category.CheckCatalogResult))
                    {
                        int AvgLoad = cDBConvert.ToInteger(CurrentFuelFlowLoadBaseline["AVG_LOAD"]);
                        if (Convert.ToBoolean(Category.GetCheckParameter("FF2LBAS_GHR_UOM_Code_Valid").ParameterValue) && AvgLoad > 0 && AvgFFRate > 0)//!
                        {
                            decimal CalcFFLR = Math.Round(AvgFFRate / AvgLoad, 2, MidpointRounding.AwayFromZero);
                            if (BaseFF2LRatio != CalcFFLR)
                                Category.CheckCatalogResult = "C";
                        }
                    }
                }
                else
                    Log = false;
            }

            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "FF2LBAS17");
            }

            return ReturnVal;
        }

        public static string FF2LBAS18(cCategory Category, ref bool Log)
        //Fuel Flow to Load Baseline GHR Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentFuelFlowLoadBaseline = (DataRowView)Category.GetCheckParameter("Current_Fuel_Flow_To_Load_Baseline").ParameterValue;
                int BaseGHR = cDBConvert.ToInteger(CurrentFuelFlowLoadBaseline["BASELINE_GHR"]);
                decimal BaseFF2LRatio = cDBConvert.ToDecimal(CurrentFuelFlowLoadBaseline["BASELINE_FUEL_FLOW_LOAD_RATIO"]);
                if (BaseGHR == int.MinValue && BaseFF2LRatio != decimal.MinValue)
                    Category.SetCheckParameter("FF2LBAS_Test_Basis", "Q", eParameterDataType.String);
                else
                    if (BaseGHR != int.MinValue && BaseFF2LRatio == decimal.MinValue)
                        Category.SetCheckParameter("FF2LBAS_Test_Basis", "H", eParameterDataType.String);
                    else
                        Category.SetCheckParameter("FF2LBAS_Test_Basis", null, eParameterDataType.String);
                decimal AvgHHIRate = cDBConvert.ToDecimal(CurrentFuelFlowLoadBaseline["AVG_HRLY_HI_RATE"]);
                if (Convert.ToBoolean(Category.GetCheckParameter("FF2LBAS_Method_Valid").ParameterValue) && AvgHHIRate != decimal.MinValue)
                {
                    if (BaseGHR == int.MinValue)
                        Category.CheckCatalogResult = "A";
                    else
                        if (BaseGHR <= 0)
                            Category.CheckCatalogResult = "B";

                    if (string.IsNullOrEmpty(Category.CheckCatalogResult))
                    {
                        int AvgLoad = cDBConvert.ToInteger(CurrentFuelFlowLoadBaseline["AVG_LOAD"]);
                        if (Convert.ToBoolean(Category.GetCheckParameter("FF2LBAS_GHR_UOM_Code_Valid").ParameterValue) && AvgLoad > 0 && AvgHHIRate > 0)
                        {
                            decimal CalcGHR = Math.Round(1000*AvgHHIRate / AvgLoad, MidpointRounding.AwayFromZero);
                            if (BaseGHR != CalcGHR)
                                Category.CheckCatalogResult = "C";
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "FF2LBAS18");
            }

            return ReturnVal;
        }

        public static string FF2LBAS19(cCategory Category, ref bool Log)
        //System ID Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentFuelFlowLoadBaseline = (DataRowView)Category.GetCheckParameter("Current_Fuel_Flow_To_Load_Baseline").ParameterValue;
                if (CurrentFuelFlowLoadBaseline["MON_SYS_ID"] == DBNull.Value)
                    Category.CheckCatalogResult = "A";
            }

            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "FF2LBAS19");
            }

            return ReturnVal;
        }

        public static string FF2LBAS20(cCategory Category, ref bool Log)
        //Duplicate Fuel Flow to Load Baseline Data
        {
            string ReturnVal = "";

            try
            {
                if (Convert.ToBoolean(Category.GetCheckParameter("Test_Number_Valid").ParameterValue))
                {
                    DataRowView CurrentFuelFlowLoadBaseline = (DataRowView)Category.GetCheckParameter("Current_Fuel_Flow_To_Load_Baseline").ParameterValue;
                    string TestNum = cDBConvert.ToString(CurrentFuelFlowLoadBaseline["TEST_NUM"]);
                    DataView TestRecs = (DataView)Category.GetCheckParameter("Location_Test_Records").ParameterValue;
                    string OldFilter = TestRecs.RowFilter;
                    TestRecs.RowFilter = AddToDataViewFilter(OldFilter, "TEST_TYPE_CD = 'FF2LBAS' AND TEST_NUM = '" + TestNum + "'");
                    if ((TestRecs.Count > 0 && CurrentFuelFlowLoadBaseline["TEST_SUM_ID"] == DBNull.Value) ||
                        (TestRecs.Count > 1 && CurrentFuelFlowLoadBaseline["TEST_SUM_ID"] != DBNull.Value) ||
                        (TestRecs.Count == 1 && CurrentFuelFlowLoadBaseline["TEST_SUM_ID"] != DBNull.Value && CurrentFuelFlowLoadBaseline["TEST_SUM_ID"].ToString() != TestRecs[0]["TEST_SUM_ID"].ToString()))
                    {
                        Category.SetCheckParameter("Duplicate_Fuel_Flow_To_Load_Baseline", true, eParameterDataType.Boolean);
                        Category.CheckCatalogResult = "A";
                    }
                    else
                    {
                      string TestSumID = cDBConvert.ToString(CurrentFuelFlowLoadBaseline["TEST_SUM_ID"]);
                      if (TestSumID != "")
                      {
                        DataView QASuppRecords = (DataView)Category.GetCheckParameter("QA_Supplemental_Data_Records").ParameterValue;
                        string OldFilter2 = QASuppRecords.RowFilter;
                        QASuppRecords.RowFilter = AddToDataViewFilter(OldFilter2, "TEST_NUM = '" + TestNum + "' AND TEST_TYPE_CD = 'FF2LBAS'");
                        if (QASuppRecords.Count > 0 && cDBConvert.ToString(QASuppRecords[0]["TEST_SUM_ID"]) != TestSumID)
                        {
                          Category.SetCheckParameter("Duplicate_Fuel_Flow_To_Load_Baseline", true, eParameterDataType.Boolean);
                          Category.CheckCatalogResult = "B";
                        }
                        else
                          Category.SetCheckParameter("Duplicate_Fuel_Flow_To_Load_Baseline", false, eParameterDataType.Boolean);
                        QASuppRecords.RowFilter = OldFilter2;
                      }
                      else
                        Category.SetCheckParameter("Duplicate_Fuel_Flow_To_Load_Baseline", false, eParameterDataType.Boolean);
                    }
                    TestRecs.RowFilter = OldFilter;
                }
                else
                    Log = false;
            }

            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "FF2LBAS20");
            }

            return ReturnVal;
        }
        
        /*
        public static string FF2LBAS-(cCategory Category, ref bool Log)
        {
            string ReturnVal = "";

            try
            {
                
            }

            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "FF2LBAS-");
            }

            return ReturnVal;
        }
        */
        #endregion
    }
}
