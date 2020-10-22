using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.MonitorPlan;
using ECMPS.Checks.Mp.Parameters;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;
using ECMPS.Definitions.Extensions;


namespace ECMPS.Checks.LoadChecks
{
    public class cLoadChecks : cChecks
    {
        public cLoadChecks()
        {
            CheckProcedures = new dCheckProcedure[27];

            CheckProcedures[1] = new dCheckProcedure(LOAD1);
            CheckProcedures[2] = new dCheckProcedure(LOAD2);
            CheckProcedures[3] = new dCheckProcedure(LOAD3);
            CheckProcedures[4] = new dCheckProcedure(LOAD4);
            CheckProcedures[5] = new dCheckProcedure(LOAD5);
            CheckProcedures[6] = new dCheckProcedure(LOAD6);
            CheckProcedures[7] = new dCheckProcedure(LOAD7);
            CheckProcedures[8] = new dCheckProcedure(LOAD8);
            CheckProcedures[9] = new dCheckProcedure(LOAD9);
            CheckProcedures[10] = new dCheckProcedure(LOAD10);
            CheckProcedures[11] = new dCheckProcedure(LOAD11);
            CheckProcedures[12] = new dCheckProcedure(LOAD12);
            CheckProcedures[13] = new dCheckProcedure(LOAD13);

            CheckProcedures[17] = new dCheckProcedure(LOAD17);
            CheckProcedures[19] = new dCheckProcedure(LOAD19);
            CheckProcedures[20] = new dCheckProcedure(LOAD20);
            CheckProcedures[21] = new dCheckProcedure(LOAD21);
            CheckProcedures[22] = new dCheckProcedure(LOAD22);
            CheckProcedures[23] = new dCheckProcedure(LOAD23);
            CheckProcedures[24] = new dCheckProcedure(LOAD24);
            CheckProcedures[25] = new dCheckProcedure(LOAD25);
            CheckProcedures[26] = new dCheckProcedure(LOAD26);
			//CheckProcedures[27] = new dCheckProcedure(LOAD27);
        }

        public static string LOAD1(cCategory Category, ref bool Log) //Load Analysis Date Valid
        {
            string ReturnVal = "";
            try
            {
                DataRowView CurrentLoad = (DataRowView)Category.GetCheckParameter("Current_Load").ParameterValue;
                bool LoadLevelsRequired = (bool)Category.GetCheckParameter("Load_Levels_Required").ParameterValue;
                if (!LoadLevelsRequired)
                {
                    if (CurrentLoad["load_analysis_date"] != DBNull.Value)
                        Category.CheckCatalogResult = "A";
                }
                else
                {
                    if (CurrentLoad["load_analysis_date"] == DBNull.Value)
                    {
                        DataRowView CurrentLocation = (DataRowView)Category.GetCheckParameter("Current_Location").ParameterValue;
                        DateTime ComrOpDate = cDBConvert.ToDate(CurrentLocation["COMR_OP_DATE"], DateTypes.END);

                        DateTime fourthQuarterAfterDate;
                        {
                            if (ComrOpDate != DateTime.MaxValue)
                            {
                                int fourthQuarterYear = ComrOpDate.AddYears(1).Year;
                                int fourthQuarterQtr = (ComrOpDate.AddYears(1).Month - 1) / 3 + 1;
                                fourthQuarterAfterDate = new DateTime(fourthQuarterYear, 3 * (fourthQuarterQtr - 1) + 1, 1);
                            }
                            else
                            {
                                fourthQuarterAfterDate = DateTime.MaxValue;
                            }
                        }

                        if (ComrOpDate != DateTime.MaxValue && fourthQuarterAfterDate < DateTime.Now)
                            Category.CheckCatalogResult = "B";
                        else
                            Category.CheckCatalogResult = "E";
                    }
                    else
                    {
                        DateTime LoadAnalysisDate = cDBConvert.ToDate(CurrentLoad["load_analysis_date"], DateTypes.START);

                        if (LoadAnalysisDate < new DateTime(1993, 1, 1))
                            Category.CheckCatalogResult = "C";
                        else
                        {
                            DateTime LoadBeginDate = cDBConvert.ToDate(CurrentLoad["begin_date"], DateTypes.START);
                            if (LoadAnalysisDate > LoadBeginDate)
                            {
                                if (LoadBeginDate >= new DateTime(2001, 1, 1))
                                    Category.CheckCatalogResult = "D";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "LOAD1");
            }
            return ReturnVal;
        }
        public static string LOAD2(cCategory Category, ref bool Log) //Load Begin Date Valid
        {
            string ReturnVal = "";
            try
            {
                DataRowView CurrentLoad = (DataRowView)Category.GetCheckParameter("Current_Load").ParameterValue;
                Category.SetCheckParameter("Load_Start_Date_Valid", true, eParameterDataType.Boolean);

                if (CurrentLoad["begin_date"] == DBNull.Value)
                {
                    Category.CheckCatalogResult = "A";
                    Category.SetCheckParameter("Load_Start_Date_Valid", false, eParameterDataType.Boolean);
                }
                else
                {
                    DateTime BeginDate = cDBConvert.ToDate(CurrentLoad["begin_date"], DateTypes.START);
                    if (BeginDate < new DateTime(1993, 1, 1) || BeginDate > Category.Process.MaximumFutureDate)
                    {
                        Category.CheckCatalogResult = "B";
                        Category.SetCheckParameter("Load_Start_Date_Valid", false, eParameterDataType.Boolean);
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "LOAD2");
            }
            return ReturnVal;
        }
        public static string LOAD3(cCategory Category, ref bool Log) //Load Begin Hour Valid
        {
            string ReturnVal = "";
            try
            {
                DataRowView CurrentLoad = (DataRowView)Category.GetCheckParameter("Current_Load").ParameterValue;
                Category.SetCheckParameter("Load_Start_Hour_Valid", true, eParameterDataType.Boolean);

                if (CurrentLoad["begin_hour"] == DBNull.Value)
                {
                    Category.CheckCatalogResult = "A";
                    Category.SetCheckParameter("Load_Start_Hour_Valid", false, eParameterDataType.Boolean);
                }
                else
                {
                    int BeginHour = cDBConvert.ToInteger(CurrentLoad["begin_hour"]);
                    if (BeginHour < 0 || BeginHour > 23)
                    {
                        Category.CheckCatalogResult = "B";
                        Category.SetCheckParameter("Load_Start_Hour_Valid", false, eParameterDataType.Boolean);
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "LOAD3");
            }
            return ReturnVal;
        }
        public static string LOAD4(cCategory Category, ref bool Log) //Load End Date Valid
        {
            string ReturnVal = "";
            try
            {
                DataRowView CurrentLoad = (DataRowView)Category.GetCheckParameter("Current_Load").ParameterValue;
                Category.SetCheckParameter("Load_End_Date_Valid", true, eParameterDataType.Boolean);

                if (CurrentLoad["end_date"] != DBNull.Value)
                {
                    DateTime EndDate = cDBConvert.ToDate(CurrentLoad["end_date"], DateTypes.END);
                    if (EndDate < new DateTime(1993, 1, 1) || EndDate > Category.Process.MaximumFutureDate)
                    {
                        Category.CheckCatalogResult = "A";
                        Category.SetCheckParameter("Load_End_Date_Valid", false, eParameterDataType.Boolean);
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "LOAD4");
            }
            return ReturnVal;
        }
        public static string LOAD5(cCategory Category, ref bool Log) //Load End Hour Valid
        {
            string ReturnVal = "";
            try
            {
                DataRowView CurrentLoad = (DataRowView)Category.GetCheckParameter("Current_Load").ParameterValue;
                Category.SetCheckParameter("Load_End_Hour_Valid", true, eParameterDataType.Boolean);
                if (CurrentLoad["end_hour"] != DBNull.Value)
                {
                    int EndHour = cDBConvert.ToInteger(CurrentLoad["end_hour"]);
                    if (EndHour < 0 || EndHour > 23)
                    {
                        Category.CheckCatalogResult = "A";
                        Category.SetCheckParameter("Load_End_Hour_Valid", false, eParameterDataType.Boolean);
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "LOAD5");
            }
            return ReturnVal;
        }
        public static string LOAD6(cCategory Category, ref bool Log) //Load Dates and Hours Consistent
        {
            string ReturnVal = "";
            try
            {
                ReturnVal = Check_ConsistentHourRange(Category, "Load_Dates_And_Hours_Consistent",
                                                                "Current_Load",
                                                                "Load_Start_Date_Valid",
                                                                "Load_Start_Hour_Valid",
                                                                "Load_End_Date_Valid",
                                                                "Load_End_Hour_Valid");

            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "LOAD6");
            }
            return ReturnVal;
        }
        public static string LOAD7(cCategory Category, ref bool Log) //Load Active Status
        {
            string ReturnVal = "";
            try
            {
                bool LoadDatesAndHoursConsistent = (bool)Category.GetCheckParameter("Load_Dates_And_Hours_Consistent").ParameterValue;

                if (LoadDatesAndHoursConsistent)
                    ReturnVal = Check_ActiveHourRange(Category, "Load_Active_Status",
                                                                "Current_Load",
                                                                "Load_Evaluation_Start_Date",
                                                                "Load_Evaluation_Start_Hour",
                                                                "Load_Evaluation_End_Date",
                                                                "Load_Evaluation_End_Hour");
                else
                    Log = false;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "LOAD7");
            }
            return ReturnVal;
        }
        public static string LOAD8(cCategory Category, ref bool Log) //Maximum Load Value Valid
        {
            string ReturnVal = "";
            try
            {
                int Indicator = (int)Category.GetCheckParameter("Non_Load_Based_Indicator").ParameterValue;
                DataRowView CurrentLoad = (DataRowView)Category.GetCheckParameter("Current_Load").ParameterValue;
                if (Indicator == 1)
                {
                    if (CurrentLoad["max_load_value"] != DBNull.Value)
                        Category.CheckCatalogResult = "A";
                }
                else
                {
                    int maxLoadVal = cDBConvert.ToInteger(CurrentLoad["max_load_value"]);
                    if (CurrentLoad["max_load_value"] == DBNull.Value)
                        Category.CheckCatalogResult = "B";
                    else
                    {
                        if (maxLoadVal <= 0)
                            Category.CheckCatalogResult = "C";
                    }
                }

            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "LOAD8");
            }
            return ReturnVal;
        }
        public static string LOAD9(cCategory Category, ref bool Log) //Load Upper Operation Boundary Valid
        {
            string ReturnVal = "";
            try
            {
                DataRowView CurrentLoad = (DataRowView)Category.GetCheckParameter("Current_Load").ParameterValue;
                bool RangeOfOperationRequired = (bool)Category.GetCheckParameter("Range_of_Operation_Required").ParameterValue;

                if (RangeOfOperationRequired)
                {
                    if (CurrentLoad["up_op_boundary"] == DBNull.Value)
                        Category.CheckCatalogResult = "A";
                    else
                    {
                        int UpOpBoundary = cDBConvert.ToInteger(CurrentLoad["up_op_boundary"]);
                        int MaxLoadVal = cDBConvert.ToInteger(CurrentLoad["max_load_value"]);
                        if (!(UpOpBoundary > 0))
                            Category.CheckCatalogResult = "B";
                        else if ((!(UpOpBoundary <= MaxLoadVal)) && MaxLoadVal > 0)
                            Category.CheckCatalogResult = "C";
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "LOAD9");
            }
            return ReturnVal;
        }
        public static string LOAD10(cCategory Category, ref bool Log) //Load Lower Operation Boundary Valid
        {
            string ReturnVal = "";
            try
            {
                DataRowView CurrentLoad = (DataRowView)Category.GetCheckParameter("Current_Load").ParameterValue;
                bool RangeOfOperationRequired = (bool)Category.GetCheckParameter("Range_of_Operation_Required").ParameterValue;

                if (RangeOfOperationRequired)
                {
                    if (CurrentLoad["low_op_boundary"] == DBNull.Value)
                        Category.CheckCatalogResult = "A";
                    else
                    {
                        int LowOpBoundary = cDBConvert.ToInteger(CurrentLoad["low_op_boundary"]);
                        int UpOpBoundary = cDBConvert.ToInteger(CurrentLoad["up_op_boundary"]);
                        if (LowOpBoundary < 0)
                            Category.CheckCatalogResult = "B";
                        else if (LowOpBoundary >= UpOpBoundary && UpOpBoundary > 0)
                            Category.CheckCatalogResult = "C";
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "LOAD10");
            }
            return ReturnVal;
        }
        public static string LOAD11(cCategory Category, ref bool Log) //Load Normal Level Code Valid
        {
            string ReturnVal = "";
            try
            {
                DataRowView CurrentLoad = (DataRowView)Category.GetCheckParameter("Current_Load").ParameterValue;
                bool LoadLevelsRequired = (bool)Category.GetCheckParameter("Load_Levels_Required").ParameterValue;

                if (!LoadLevelsRequired)
                {
                    if (CurrentLoad["normal_level_cd"] != DBNull.Value)
                        Category.CheckCatalogResult = "A";
                }
                else
                {
                    if (CurrentLoad["normal_level_cd"] == DBNull.Value)
                    {
                        DataRowView CurrentLocation = (DataRowView)Category.GetCheckParameter("Current_Location").ParameterValue;
                        DateTime ComrOpDate = cDBConvert.ToDate(CurrentLocation["COMR_OP_DATE"], DateTypes.END);
                        if (ComrOpDate != DateTime.MaxValue && ComrOpDate.AddDays(180) < DateTime.Now)
                            Category.CheckCatalogResult = "B";
                        else
                            Category.CheckCatalogResult = "D";
                    }
                    else
                    {
                        string NormalLevelCd = cDBConvert.ToString(CurrentLoad["normal_level_cd"]);
                        if (!NormalLevelCd.InList("H,L,M"))
                            Category.CheckCatalogResult = "C";
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "LOAD11");
            }
            return ReturnVal;
        }
        public static string LOAD12(cCategory Category, ref bool Log) //Load Second Level Code Valid
        {
            string ReturnVal = "";
            try
            {
                DataRowView CurrentLoad = (DataRowView)Category.GetCheckParameter("Current_Load").ParameterValue;
                bool LoadLevelsRequired = (bool)Category.GetCheckParameter("Load_Levels_Required").ParameterValue;

                if (!LoadLevelsRequired)
                {
                    if (CurrentLoad["second_level_cd"] != DBNull.Value)
                        Category.CheckCatalogResult = "A";
                }
                else
                {
                    if (CurrentLoad["second_level_cd"] == DBNull.Value)
                    {
                        if (CurrentLoad["normal_level_cd"] != DBNull.Value)
                            Category.CheckCatalogResult = "B";
                    }
                    else
                    {
                        string SecondLevelCd = cDBConvert.ToString(CurrentLoad["second_level_cd"]);
                        if (!SecondLevelCd.InList("H,L,M"))
                            Category.CheckCatalogResult = "C";
                        else
                        {
                            if (SecondLevelCd == cDBConvert.ToString(CurrentLoad["normal_level_cd"]))
                                Category.CheckCatalogResult = "D";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "LOAD12");
            }
            return ReturnVal;
        }
        public static string LOAD13(cCategory Category, ref bool Log) //Maximum Load Units of Measure Valid
        {
            string ReturnVal = "";
            try
            {
                DataRowView CurrentLoad = (DataRowView)Category.GetCheckParameter("Current_Load").ParameterValue;
                Category.SetCheckParameter("Maximum_Load_Units_Of_Measure_Valid", true, eParameterDataType.Boolean);
                int NonLoadBasedBasedInd = (int)Category.GetCheckParameter("Non_Load_Based_Indicator").ParameterValue;
                if (NonLoadBasedBasedInd == 1)
                {
                    if (CurrentLoad["max_load_uom_cd"] != DBNull.Value)
                    {
                        Category.SetCheckParameter("Maximum_Load_Units_Of_Measure_Valid", false, eParameterDataType.Boolean);
                        Category.CheckCatalogResult = "A";
                    }
                }
                else
                {
                    if (CurrentLoad["max_load_uom_cd"] == DBNull.Value)
                    {
                        Category.SetCheckParameter("Maximum_Load_Units_Of_Measure_Valid", false, eParameterDataType.Boolean);
                        Category.CheckCatalogResult = "B";
                    }
                    else
                    {
                        DataView ParamUOM = (DataView)Category.GetCheckParameter("Parameter_Units_Of_Measure_Lookup_Table").ParameterValue;
                        string MaxLoadUOMCd = cDBConvert.ToString(CurrentLoad["max_load_uom_cd"]);

                        string OldFilter = ParamUOM.RowFilter;
                        ParamUOM.RowFilter = AddToDataViewFilter(OldFilter, "PARAMETER_CD = 'LOAD' and UOM_CD = '" + MaxLoadUOMCd + "'");
                        if (ParamUOM.Count == 0)
                        {
                            Category.SetCheckParameter("Maximum_Load_Units_Of_Measure_Valid", false, eParameterDataType.Boolean);
                            Category.CheckCatalogResult = "C";
                        }
                        ParamUOM.RowFilter = OldFilter;
                    }
                }

            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "LOAD13");
            }
            return ReturnVal;
        }
        public static string LOAD17(cCategory Category, ref bool Log) //Overlapping Loads
        {
            string ReturnVal = "";
            try
            {
                DataRowView CurrentLoad = (DataRowView)Category.GetCheckParameter("Current_Load").ParameterValue;
                bool LoadDatesAndHoursConsistent = (bool)Category.GetCheckParameter("Load_Dates_And_Hours_Consistent").ParameterValue;
                if (LoadDatesAndHoursConsistent)
                {
                    DataView LoadRecords = (DataView)Category.GetCheckParameter("Load_Records").ParameterValue;
                    DateTime LoadEvalBeginDate = (DateTime)Category.GetCheckParameter("Load_Evaluation_Start_Date").ParameterValue;
                    DateTime LoadEvalEndDate = (DateTime)Category.GetCheckParameter("Load_Evaluation_End_Date").ParameterValue;
                    int LoadEvalBeginHour = (int)Category.GetCheckParameter("Load_Evaluation_Start_Hour").ParameterValue;
                    int LoadEvalEndHour = (int)Category.GetCheckParameter("Load_Evaluation_End_Hour").ParameterValue;

                    DateTime CurrentBeginDate = cDBConvert.ToDate(CurrentLoad["begin_date"], DateTypes.START);
                    int CurrentBeginHour = cDBConvert.ToInteger(CurrentLoad["begin_hour"]);

                    string OldFilter = LoadRecords.RowFilter;
                    LoadRecords.RowFilter = AddToDataViewFilter(OldFilter, "begin_date > '" + CurrentBeginDate.ToShortDateString() +
                                                                "'or( begin_date = '" + CurrentBeginDate.ToShortDateString() +
                                                                "' and begin_hour >= " + CurrentBeginHour.ToString() + ")");
                    LoadRecords.RowFilter = AddEvaluationDateHourRangeToDataViewFilter(OldFilter, LoadEvalBeginDate, LoadEvalEndDate,
                                                                                        LoadEvalBeginHour, LoadEvalEndHour, false, true);
                    if (LoadRecords.Count > 1 ||
                        (LoadRecords.Count == 1 && cDBConvert.ToString(LoadRecords[0]["load_id"]) != cDBConvert.ToString(CurrentLoad["load_id"])))
                    {
                        Category.CheckCatalogResult = "A";
                    }

                    LoadRecords.RowFilter = OldFilter;
                }
                else
                    Log = false;

            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "LOAD17");
            }
            return ReturnVal;
        }
        public static string LOAD19(cCategory Category, ref bool Log) //Load Units of Measure Consistent Across Linked Locations
        {
            string ReturnVal = "";
            try
            {
                DataRowView CurrentLoad = (DataRowView)Category.GetCheckParameter("Current_Load").ParameterValue;
                bool MaxLoadUOMValid = (bool)Category.GetCheckParameter("Maximum_Load_Units_Of_Measure_Valid").ParameterValue;

                if (MaxLoadUOMValid)
                {
                    string LocationType = (string)Category.GetCheckParameter("Location_Type").ParameterValue;
                    int NonLoadBasedInd = (int)Category.GetCheckParameter("Non_Load_Based_Indicator").ParameterValue;
                    if ((!LocationType.StartsWith("U", true, null) && NonLoadBasedInd != 1))
                    {
                        DateTime LoadEvalStartDate = (DateTime)Category.GetCheckParameter("Load_Evaluation_Start_Date").ParameterValue;
                        DateTime LoadEvalEndDate = (DateTime)Category.GetCheckParameter("Load_Evaluation_End_Date").ParameterValue;
                        int LoadEvalStartHour = (int)Category.GetCheckParameter("Load_Evaluation_Start_Hour").ParameterValue;
                        int LoadEvalEndHour = (int)Category.GetCheckParameter("Load_Evaluation_End_Hour").ParameterValue;
                        DataView UnitStackRecs = (DataView)Category.GetCheckParameter("MP_Unit_Stack_Configuration_Records").ParameterValue;
                        DataView FacilityLoadRecords = (DataView)Category.GetCheckParameter("Facility_Load_Records").ParameterValue;

                        string USOldFilter = UnitStackRecs.RowFilter;
                        UnitStackRecs.RowFilter = AddEvaluationDateRangeToDataViewFilter(USOldFilter, LoadEvalStartDate, LoadEvalEndDate, false, true, false);
                        UnitStackRecs.RowFilter = AddToDataViewFilter(UnitStackRecs.RowFilter, "stack_pipe_mon_loc_id = '" +
                                                                        cDBConvert.ToString(CurrentLoad["mon_loc_id"]) + "'");
                        bool Consistent = true;
                        foreach (DataRowView UnitStackRec in UnitStackRecs)
                        {
                            string LoadOldFilter = FacilityLoadRecords.RowFilter;
                            FacilityLoadRecords.RowFilter = AddEvaluationDateHourRangeToDataViewFilter(LoadOldFilter, LoadEvalStartDate, LoadEvalEndDate,
                                                                                                LoadEvalStartHour, LoadEvalEndHour, false, true);
                            FacilityLoadRecords.RowFilter = AddToDataViewFilter(FacilityLoadRecords.RowFilter, "mon_loc_id = '" +
                                                                                cDBConvert.ToString(UnitStackRec["mon_loc_id"]) + "'");
                            foreach (DataRowView LoadRec in FacilityLoadRecords)
                            {
                                if (LoadRec["max_load_uom_cd"] != DBNull.Value
                                    && (cDBConvert.ToString(LoadRec["max_load_uom_cd"]) != cDBConvert.ToString(CurrentLoad["max_load_uom_cd"])))
                                {
                                    Consistent = false;
                                    break;
                                }

                            }
                            FacilityLoadRecords.RowFilter = LoadOldFilter;
                            if (!Consistent)
                                break;
                        }
                        UnitStackRecs.RowFilter = USOldFilter;
                        if (!Consistent)
                            Category.CheckCatalogResult = "A";
                    }
                }
                else
                    Log = false;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "LOAD19");
            }
            return ReturnVal;
        }
        public static string LOAD20(cCategory Category, ref bool Log) //Determine Load Requirement
        {
            string ReturnVal = "";
            try
            {
                DataRowView CurrentLoad = (DataRowView)Category.GetCheckParameter("Current_Load").ParameterValue;
                string theLocation = cDBConvert.ToString(CurrentLoad["mon_loc_id"]);

                Category.SetCheckParameter("Range_of_Operation_Required", false, eParameterDataType.Boolean);
                Category.SetCheckParameter("Load_Levels_Required", false, eParameterDataType.Boolean);

                DataView SystemRecords = (DataView)Category.GetCheckParameter("Monitor_System_Records").ParameterValue;
                DateTime LoadEvalStartDate = (DateTime)Category.GetCheckParameter("Load_Evaluation_Start_Date").ParameterValue;
                DateTime LoadEvalEndDate = (DateTime)Category.GetCheckParameter("Load_Evaluation_End_Date").ParameterValue;
                int LoadEvalStartHour = (int)Category.GetCheckParameter("Load_Evaluation_Start_Hour").ParameterValue;
                int LoadEvalEndHour = (int)Category.GetCheckParameter("Load_Evaluation_End_Hour").ParameterValue;

                string MSOldFilter = SystemRecords.RowFilter;
                SystemRecords.RowFilter = AddToDataViewFilter(MSOldFilter, "sys_type_cd in ('SO2','SO2R','NOX','NOXC','CO2','O2','FLOW','HG','HCL','HF')");
                SystemRecords.RowFilter = AddEvaluationDateHourRangeToDataViewFilter(SystemRecords.RowFilter, LoadEvalStartDate, LoadEvalEndDate,
                                                                                        LoadEvalStartHour, LoadEvalEndHour, false, true);
                if (SystemRecords.Count > 0)
                {
                    Category.SetCheckParameter("Range_of_Operation_Required", true, eParameterDataType.Boolean);

                    string LocationType = (string)Category.GetCheckParameter("Location_Type").ParameterValue;
                    DataView FacilityQualRecs = (DataView)Category.GetCheckParameter("Facility_Qualification_Records").ParameterValue;
                    string QualOldFilter = FacilityQualRecs.RowFilter;

                    if (LocationType.StartsWith("U", true, null))
                    {
                        FacilityQualRecs.RowFilter = AddToDataViewFilter(QualOldFilter, "mon_loc_id = '" + theLocation +
                                                                        "' and(qual_type_cd = 'PK' or qual_type_cd = 'SK')");
                        //FacilityQualRecs.RowFilter = AddEvaluationDateRangeToDataViewFilter(FacilityQualRecs.RowFilter, LoadEvalStartDate,
                        //                                                                    LoadEvalEndDate, false, true, false);
                        if (!CheckForDateRangeCovered(Category, FacilityQualRecs, LoadEvalStartDate, LoadEvalEndDate, false))
                            Category.SetCheckParameter("Load_Levels_Required", true, eParameterDataType.Boolean);
                    }
                    else
                    {
                        DataView UnitStackConfigRecs = (DataView)Category.GetCheckParameter("Unit_Stack_Configuration_Records").ParameterValue;
                        string USOldFilter = UnitStackConfigRecs.RowFilter;
                        UnitStackConfigRecs.RowFilter = AddToDataViewFilter(USOldFilter, "stack_pipe_mon_loc_id = '" + theLocation + "'");
                        UnitStackConfigRecs.RowFilter = AddEvaluationDateRangeToDataViewFilter(UnitStackConfigRecs.RowFilter, LoadEvalStartDate,
                                                                                                LoadEvalEndDate, false, true, false);
                        foreach (DataRowView UnitStackConfigRec in UnitStackConfigRecs)
                        {
                            FacilityQualRecs.RowFilter = AddToDataViewFilter(QualOldFilter, "mon_loc_id = '" + cDBConvert.ToString(UnitStackConfigRec["mon_loc_id"]) +
                                                                                            "' and (qual_type_cd = 'PK' or qual_type_cd = 'SK')");
                            if (!CheckForDateRangeCovered(Category, FacilityQualRecs, LoadEvalStartDate, LoadEvalEndDate))
                            {
                                Category.SetCheckParameter("Load_Levels_Required", true, eParameterDataType.Boolean);
                                break;
                            }
                        }
                        UnitStackConfigRecs.RowFilter = USOldFilter;
                    }
                    FacilityQualRecs.RowFilter = QualOldFilter;
                }
                else
                {
                    DataView QASuppRecs = (DataView)Category.GetCheckParameter("QA_Supplemental_Data_Records").ParameterValue;
                    string QASuppOldFilter = QASuppRecs.RowFilter;
                    QASuppRecs.RowFilter = AddToDataViewFilter(QASuppOldFilter, "mon_loc_id = '" + cDBConvert.ToString(CurrentLoad["mon_loc_id"]) +
                                                                "' and test_type_cd = 'FF2LTST'");
                    //int LoadEvalStartQuarter = LoadEvalStartDate.Month % 4;
                    //int LoadEvalEndQuarter = LoadEvalEndDate.Month % 4;

                    //string NewFilter = "calendar_year < '" + LoadEvalStartDate.Year.ToString() +
                    //                    "' or calendar_year = '" + LoadEvalStartDate.Year.ToString() + 
                    //                    "' and quarter < '" + LoadEvalStartQuarter.ToString() + 
                    //                    "' or quarter = '" + LoadEvalStartQuarter.ToString() + 
                    //                    "' and " ;
                    //QASuppRecs.RowFilter = AddToDataViewFilter(QASuppRecs.RowFilter,NewFilter);
                    //DateTime LastDayQuarterYear;
                    //DateTime FirstDayQuarterYear;

                    //int LastYear = cDBConvert.ToInteger(QASuppRecs["Calendar"]);
                    //int FirstYear;
                    //int LastMonth;
                    //int FirstMonth;
                    //int LastDay;
                    //int FirstDay = 1;
                    bool RecFound = false;
                    foreach (DataRowView QASuppRec in QASuppRecs)
                    {
                        bool CheckFirst = false;
                        int thisQuarter = cDBConvert.ToInteger(QASuppRec["Quarter"]);
                        int Year = cDBConvert.ToInteger(QASuppRec["calendar_year"]);
                        int LastMonth = thisQuarter * 3;
                        int FirstMonth = (thisQuarter * 3) - 2;
                        int LastDay = thisQuarter == 1 || thisQuarter == 4 ? 31 : 30;
                        int FirstDay = 1;
                        int FirstHour = 0, LastHour = 23;

                        if (Year < LoadEvalEndDate.Year)
                            CheckFirst = true;
                        else if (Year == LoadEvalEndDate.Year)
                        {
                            if (LastMonth < LoadEvalEndDate.Month)
                                CheckFirst = true;
                            else if (LastMonth == LoadEvalEndDate.Month)
                            {
                                if (LastDay < LoadEvalEndDate.Day)
                                    CheckFirst = true;
                                else if (LastDay == LoadEvalEndDate.Day)
                                {
                                    if (LastHour <= LoadEvalEndHour)
                                        CheckFirst = true;
                                }
                            }
                        }
                        if (CheckFirst)
                        {
                            if (Year > LoadEvalStartDate.Year)
                            {
                                RecFound = true;
                                break;
                            }
                            else if (Year == LoadEvalStartDate.Year)
                            {
                                if (FirstMonth > LoadEvalStartDate.Month)
                                {
                                    RecFound = true;
                                    break;
                                }
                                else if (FirstMonth == LoadEvalEndDate.Month)
                                {
                                    if (FirstDay > LoadEvalStartDate.Day)
                                    {
                                        RecFound = true;
                                        break;
                                    }
                                    else if (FirstDay == LoadEvalStartDate.Day)
                                    {
                                        if (FirstHour >= LoadEvalStartHour)
                                        {
                                            RecFound = true;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    Category.SetCheckParameter("Range_of_Operation_Required", RecFound, eParameterDataType.Boolean);
                    QASuppRecs.RowFilter = QASuppOldFilter;
                }
                SystemRecords.RowFilter = MSOldFilter;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "LOAD20");
            }
            return ReturnVal;
        }
        public static string LOAD21(cCategory Category, ref bool Log) //Second Normal Indicator Valid
        {
            string ReturnVal = "";
            try
            {
                DataRowView CurrentLoad = (DataRowView)Category.GetCheckParameter("Current_Load").ParameterValue;
                bool LoadLevelsRequired = (bool)Category.GetCheckParameter("Load_Levels_Required").ParameterValue;
                if (!LoadLevelsRequired)
                {
                    if (CurrentLoad["second_normal_ind"] != DBNull.Value)
                        Category.CheckCatalogResult = "A";
                }
                else
                {
                    if (CurrentLoad["second_normal_ind"] == DBNull.Value)
                    {
                        if (CurrentLoad["second_level_cd"] != DBNull.Value)
                            Category.CheckCatalogResult = "B";
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "LOAD21");
            }
            return ReturnVal;
        }
        public static string LOAD22(cCategory Category, ref bool Log) //Duplicate Load Records
        {
            string ReturnVal = "";
            try
            {
                DataRowView CurrentLoad = (DataRowView)Category.GetCheckParameter("Current_Load").ParameterValue;
                DataView LoadRecords = (DataView)Category.GetCheckParameter("Load_Records").ParameterValue;
                DateTime CurrentBeginDate = cDBConvert.ToDate(CurrentLoad["begin_date"], DateTypes.START);
                int CurrentBeginHour = cDBConvert.ToInteger(CurrentLoad["begin_hour"]);

                string OldFilter = LoadRecords.RowFilter;
                LoadRecords.RowFilter = AddToDataViewFilter(OldFilter, "begin_date = '" + CurrentBeginDate.ToShortDateString() +
                                                            "' and begin_hour = '" + CurrentBeginHour.ToString() + "'");
                if (LoadRecords.Count > 1 ||
                       (LoadRecords.Count == 1 && cDBConvert.ToString(LoadRecords[0]["load_id"]) != cDBConvert.ToString(CurrentLoad["load_id"])))
                {
                    Category.CheckCatalogResult = "A";
                }
                else
                {
                    if (CurrentLoad["end_date"] != DBNull.Value)
                    {
                        DateTime EndDate = cDBConvert.ToDate(CurrentLoad["end_date"], DateTypes.END);
                        int EndHour = cDBConvert.ToInteger(CurrentLoad["end_hour"]);

                        LoadRecords.RowFilter = AddToDataViewFilter(OldFilter, "end_date = '" + EndDate.ToShortDateString() +
                                                                    "' and end_hour = '" + EndHour.ToString() + "'");
                        if (LoadRecords.Count > 1 ||
                                (LoadRecords.Count == 1 && cDBConvert.ToString(LoadRecords[0]["load_id"]) != cDBConvert.ToString(CurrentLoad["load_id"])))
                        {
                            Category.CheckCatalogResult = "A";
                        }

                    }
                }
                LoadRecords.RowFilter = OldFilter;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "LOAD22");
            }
            return ReturnVal;
        }
        public static string LOAD23(cCategory Category, ref bool Log) //Load Analysis Date Valid
        {
            string ReturnVal = "";
            try
            {
                DataRowView CurrentLoad = (DataRowView)Category.GetCheckParameter("Current_Load").ParameterValue;
                DateTime LoadAnalysisDate = cDBConvert.ToDate(CurrentLoad["load_analysis_date"], DateTypes.START);
                DateTime LoadBeginDate = cDBConvert.ToDate(CurrentLoad["begin_date"], DateTypes.START);
                if (CurrentLoad["load_analysis_date"] != DBNull.Value)
                {
                    if (LoadAnalysisDate < new DateTime(1993, 1, 1))
                        Category.CheckCatalogResult = "A";
                    else if (CurrentLoad["begin_date"] != DBNull.Value && (LoadAnalysisDate > LoadBeginDate))
                        Category.CheckCatalogResult = "B";
                }
                else
                    Log = false;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "LOAD23");
            }
            return ReturnVal;
        }
        public static string LOAD24(cCategory Category, ref bool Log) //Load Upper Operation Boundary Valid
        {
            string ReturnVal = "";
            try
            {
                DataRowView CurrentLoad = (DataRowView)Category.GetCheckParameter("Current_Load").ParameterValue;
                if (CurrentLoad["up_op_boundary"] != DBNull.Value)
                {
                    int UpOpBoundary = cDBConvert.ToInteger(CurrentLoad["up_op_boundary"]);
                    int MaxLoadVal = cDBConvert.ToInteger(CurrentLoad["max_load_value"]);

                    if (!(UpOpBoundary > 0))
                        Category.CheckCatalogResult = "A";
                    else if (UpOpBoundary > MaxLoadVal && MaxLoadVal > 0)
                        Category.CheckCatalogResult = "B";
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "LOAD24");
            }
            return ReturnVal;
        }
        public static string LOAD25(cCategory Category, ref bool Log) //Load Lower Operation Boundary Valid
        {
            string ReturnVal = "";
            try
            {
                DataRowView CurrentLoad = (DataRowView)Category.GetCheckParameter("Current_Load").ParameterValue;
                if (CurrentLoad["low_op_boundary"] != DBNull.Value)
                {
                    int LowOpBoundary = cDBConvert.ToInteger(CurrentLoad["low_op_boundary"]);
                    int UpOpBoundary = cDBConvert.ToInteger(CurrentLoad["up_op_boundary"]);

                    if (LowOpBoundary < 0)
                        Category.CheckCatalogResult = "A";
                    else if (LowOpBoundary >= UpOpBoundary && UpOpBoundary > 0)
                        Category.CheckCatalogResult = "B";
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "LOAD25");
            }
            return ReturnVal;
        }
        public static string LOAD26(cCategory Category, ref bool Log) //Load Level Codes Valid
        {
            string ReturnVal = "";
            try
            {
                DataRowView CurrentLoad = (DataRowView)Category.GetCheckParameter("Current_Load").ParameterValue;
                if (CurrentLoad["normal_level_cd"] != DBNull.Value || CurrentLoad["second_level_cd"] != DBNull.Value)
                {
                    if (CurrentLoad["normal_level_cd"] != DBNull.Value && CurrentLoad["second_level_cd"] != DBNull.Value &&
                       (cDBConvert.ToString(CurrentLoad["normal_level_cd"]) == cDBConvert.ToString(CurrentLoad["second_level_cd"])))
                        Category.CheckCatalogResult = "A";
                    else if (CurrentLoad["second_normal_ind"] == DBNull.Value)
                        Category.CheckCatalogResult = "B";
                }
                else
                    Log = false;

            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "LOAD26");
            }
            return ReturnVal;
        }

    }
}
