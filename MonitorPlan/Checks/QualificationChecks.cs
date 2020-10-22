using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;
using ECMPS.Definitions.Enumerations;
using ECMPS.Definitions.Extensions;

using ECMPS.Checks.Mp.Parameters;

namespace ECMPS.Checks.QualificationChecks
{
    public class cQualificationChecks : cChecks
    {
        public cQualificationChecks()
        {
            CheckProcedures = new dCheckProcedure[48];

            CheckProcedures[1] = new dCheckProcedure(QUAL1);
            CheckProcedures[3] = new dCheckProcedure(QUAL3);
            CheckProcedures[5] = new dCheckProcedure(QUAL5);

            CheckProcedures[7] = new dCheckProcedure(QUAL7);
            CheckProcedures[8] = new dCheckProcedure(QUAL8);
            CheckProcedures[11] = new dCheckProcedure(QUAL11);

            CheckProcedures[12] = new dCheckProcedure(QUAL12);
            CheckProcedures[14] = new dCheckProcedure(QUAL14);
            CheckProcedures[16] = new dCheckProcedure(QUAL16);
            CheckProcedures[18] = new dCheckProcedure(QUAL18);
            CheckProcedures[19] = new dCheckProcedure(QUAL19);
            CheckProcedures[20] = new dCheckProcedure(QUAL20);

            CheckProcedures[22] = new dCheckProcedure(QUAL22);
            CheckProcedures[23] = new dCheckProcedure(QUAL23);
            CheckProcedures[24] = new dCheckProcedure(QUAL24);
            CheckProcedures[25] = new dCheckProcedure(QUAL25);

            CheckProcedures[27] = new dCheckProcedure(QUAL27);
            CheckProcedures[28] = new dCheckProcedure(QUAL28);
            CheckProcedures[29] = new dCheckProcedure(QUAL29);

            CheckProcedures[34] = new dCheckProcedure(QUAL34);
            CheckProcedures[35] = new dCheckProcedure(QUAL35);
            CheckProcedures[36] = new dCheckProcedure(QUAL36);

            CheckProcedures[37] = new dCheckProcedure(QUAL37);

            CheckProcedures[38] = new dCheckProcedure(QUAL38);
            CheckProcedures[39] = new dCheckProcedure(QUAL39);

            CheckProcedures[40] = new dCheckProcedure(QUAL40);
            CheckProcedures[41] = new dCheckProcedure(QUAL41);
            CheckProcedures[42] = new dCheckProcedure(QUAL42);
            CheckProcedures[43] = new dCheckProcedure(QUAL43);
            CheckProcedures[44] = new dCheckProcedure(QUAL44);
            CheckProcedures[45] = new dCheckProcedure(QUAL45);
            CheckProcedures[46] = new dCheckProcedure(QUAL46);
            CheckProcedures[47] = new dCheckProcedure(QUAL47);
        }

        #region qualification

        public static string QUAL1(cCategory Category, ref bool Log) //Monitoring Qualification Type Consistent with Non Load Based Indicator
        {
            string ReturnVal = "";

            try
            {
                bool QualTypeCdValid = (bool)Category.GetCheckParameter("Qualification_Type_Code_Valid").ParameterValue;
                DataRowView CurrentQual = (DataRowView)Category.GetCheckParameter("Current_Qualification").ParameterValue;
                string qualTypeCd = cDBConvert.ToString(CurrentQual["qual_type_cd"]);
                Category.SetCheckParameter("Qualification_Consistent_with_Non_Load_Based_Indicator", true, eParameterDataType.Boolean);
                if (QualTypeCdValid && qualTypeCd.InList("LMEA,LMES,COMPLEX"))
                {
                    int Indicator = (int)Category.GetCheckParameter("Non_Load_Based_Indicator").ParameterValue;
                    if (Indicator == 1)
                    {
                        Category.CheckCatalogResult = "A";
                        Category.SetCheckParameter("Qualification_Consistent_with_Non_Load_Based_Indicator", false, eParameterDataType.Boolean);
                    }
                }
                else
                    Log = false;
            }
            catch (Exception ex)
            { ReturnVal = Category.CheckEngine.FormatError(ex, "QUAL1"); }

            return ReturnVal;
        }
        public static string QUAL3(cCategory Category, ref bool Log) //Monitoring Qualification Type Consistent with Fuel
        {
            string ReturnVal = "";

            try
            {
                bool QualTypeCdValid = (bool)Category.GetCheckParameter("Qualification_Type_Code_Valid").ParameterValue;
                DataRowView CurrentQual = (DataRowView)Category.GetCheckParameter("Current_Qualification").ParameterValue;
                string qualTypeCd = cDBConvert.ToString(CurrentQual["qual_type_cd"]);

                Category.SetCheckParameter("Qualification_Consistent_with_Fuel", true, eParameterDataType.Boolean);

                if (QualTypeCdValid && qualTypeCd.InList("GF,LMEA,LMES"))
                {

                    DateTime EvalStartDate = (DateTime)Category.GetCheckParameter("Qualification_Evaluation_Start_Date").ParameterValue;
                    DateTime EvalEndDate = (DateTime)Category.GetCheckParameter("Qualification_Evaluation_End_Date").ParameterValue;
                    DataView FuelRecords = (DataView)Category.GetCheckParameter("Location_Fuel_Records").ParameterValue;
                    string OldFilter = FuelRecords.RowFilter;
                    FuelRecords.RowFilter = AddToDataViewFilter(OldFilter, "fuel_group_cd <> 'GAS'");
                    FuelRecords.RowFilter = AddToDataViewFilter(FuelRecords.RowFilter, "fuel_group_cd <> 'OIL'");
                    FuelRecords.RowFilter = AddEvaluationDateRangeToDataViewFilter(FuelRecords.RowFilter, EvalStartDate, EvalEndDate,
                                                                                    false, true, false);
                    if (FuelRecords.Count > 0)
                    {
                        Category.CheckCatalogResult = "A";
                        Category.SetCheckParameter("Qualification_Consistent_with_Fuel", false, eParameterDataType.Boolean);
                    }

                    FuelRecords.RowFilter = OldFilter;
                }
                else
                    Log = false;
            }
            catch (Exception ex)
            { ReturnVal = Category.CheckEngine.FormatError(ex, "QUAL3"); }

            return ReturnVal;
        }
        public static string QUAL5(cCategory Category, ref bool Log) //Monitoring Qualification Type Consistent with Program and Reporting Frequency
        {
            string ReturnVal = "";

            try
            {
                bool QualTypeCdValid = (bool)Category.GetCheckParameter("Qualification_Type_Code_Valid").ParameterValue;
                DataRowView CurrentQual = (DataRowView)Category.GetCheckParameter("Current_Qualification").ParameterValue;
                string qualTypeCd = cDBConvert.ToString(CurrentQual["qual_type_cd"]);

                Category.SetCheckParameter("Qualification_Consistent_with_Program_and_Reporting_Frequency", true, eParameterDataType.Boolean);

                if (QualTypeCdValid && qualTypeCd.InList("LMEA,LMES,SK,PK"))
                {
                    DateTime EvalStartDate = (DateTime)Category.GetCheckParameter("Qualification_Evaluation_Start_Date").ParameterValue;
                    DateTime EvalEndDate = (DateTime)Category.GetCheckParameter("Qualification_Evaluation_End_Date").ParameterValue;

                    int startDateRptPrd = cDateFunctions.ThisReportingPeriod(EvalStartDate);
                    int endDateRptPrd = cDateFunctions.ThisReportingPeriod(EvalEndDate);

                    if (qualTypeCd == "SK")
                    {
                        DataView RptingFreqRecs = (DataView)Category.GetCheckParameter("Location_Reporting_Frequency_Records").ParameterValue;
                        string OldFilter = RptingFreqRecs.RowFilter;
                        RptingFreqRecs.RowFilter = AddToDataViewFilter(OldFilter, "report_freq_cd = 'Q'");
                        //RptingFreqRecs.RowFilter = AddEvaluationDateRangeToDataViewFilter(RptingFreqRecs.RowFilter, EvalStartDate, EvalEndDate,
                        //                                                                    false, true, false);

                        //DataView RptingFreqs = FindRows(RptingFreqRecs, EvalStartDate, EvalEndDate, "begin_date", "end_date");
                        RptingFreqRecs.RowFilter = AddToDataViewFilter(RptingFreqRecs.RowFilter, "begin_rpt_period_id <= " + endDateRptPrd +
                            " and (end_rpt_period_id is null or end_rpt_period_id >= " + startDateRptPrd + ")");

                        if (RptingFreqRecs.Count > 0)
                        {
                            Category.CheckCatalogResult = "A";
                            Category.SetCheckParameter("Qualification_Consistent_with_Program_and_Reporting_Frequency", false, eParameterDataType.Boolean);
                        }
                        RptingFreqRecs.RowFilter = OldFilter;
                    }
                    else if (qualTypeCd == "PK" || qualTypeCd == "LMEA")
                    {
                        DataView RptingFreqRecs = (DataView)Category.GetCheckParameter("Location_Reporting_Frequency_Records").ParameterValue;
                        string OldFilter = RptingFreqRecs.RowFilter;
                        RptingFreqRecs.RowFilter = AddToDataViewFilter(OldFilter, "report_freq_cd = 'Q'");
                        //RptingFreqRecs.RowFilter = AddEvaluationDateRangeToDataViewFilter(RptingFreqRecs.RowFilter, EvalStartDate, EvalEndDate,
                        //                                                                    false, true, false);

                        //DataView RptingFreqs = FindRows(RptingFreqRecs, EvalStartDate, EvalEndDate, "begin_date", "end_date");
                        RptingFreqRecs.RowFilter = AddToDataViewFilter(RptingFreqRecs.RowFilter, "begin_rpt_period_id <= " + endDateRptPrd +
                            " and (end_rpt_period_id is null or end_rpt_period_id >= " + startDateRptPrd + ")");

                        if (RptingFreqRecs.Count == 0)
                        {
                            Category.CheckCatalogResult = "B";
                            Category.SetCheckParameter("Qualification_Consistent_with_Program_and_Reporting_Frequency", false, eParameterDataType.Boolean);
                        }
                        RptingFreqRecs.RowFilter = OldFilter;
                    }

                    else if (qualTypeCd == "LMES")
                    {
                        DataView locationProgramParameterView
                          = cRowFilter.FindRows(Category.GetCheckParameter("Location_Program_Parameter_Records").AsDataView(),
                                                new cFilterCondition[]
                                      {
                                        new cFilterCondition("PARAMETER_CD", "NOX"),
                                        new cFilterCondition("PRG_CD", MpParameters.ProgramIsOzoneSeasonList, eFilterConditionStringCompare.InList),
                                        new cFilterCondition("CLASS", "A,B", eFilterConditionStringCompare.InList),
                                        //UMCB Date check for less than Evaluation End and not null
                                        new cFilterCondition("UMCB_DATE", EvalEndDate, eFilterDataType.DateEnded, eFilterConditionRelativeCompare.LessThanOrEqual),
                                        //Program End Date check for greater than Evaluation Begin or is null
                                        new cFilterCondition("PRG_END_DATE", EvalStartDate, eFilterDataType.DateEnded, eFilterConditionRelativeCompare.GreaterThanOrEqual)
                                      });

                        if (locationProgramParameterView.Count == 0)
                        {
                            Category.CheckCatalogResult = "C";
                            Category.SetCheckParameter("Qualification_Consistent_with_Program_and_Reporting_Frequency", false, eParameterDataType.Boolean);
                        }
                    }
                }
                else
                    Log = false;
            }
            catch (Exception ex)
            { ReturnVal = Category.CheckEngine.FormatError(ex, "QUAL5"); }

            return ReturnVal;
        }
        public static string QUAL12(cCategory Category, ref bool Log) //Required LME Qualification for Program and Reporting Frequency
        {
            string ReturnVal = "";

            try
            {
                bool QualTypeCdValid = (bool)Category.GetCheckParameter("Qualification_Type_Code_Valid").ParameterValue;
                DataRowView CurrentQual = (DataRowView)Category.GetCheckParameter("Current_Qualification").ParameterValue;
                string qualTypeCd = cDBConvert.ToString(CurrentQual["qual_type_cd"]);

                if (QualTypeCdValid && qualTypeCd.StartsWith("LME"))
                {

                    DateTime EvalStartDate = (DateTime)Category.GetCheckParameter("Qualification_Evaluation_Start_Date").ParameterValue;
                    DateTime EvalEndDate = (DateTime)Category.GetCheckParameter("Qualification_Evaluation_End_Date").ParameterValue;

                    int startDateRptPrd = cDateFunctions.ThisReportingPeriod(EvalStartDate);
                    int endDateRptPrd = cDateFunctions.ThisReportingPeriod(EvalEndDate);

                    DateTime CurrentBeginDate = cDBConvert.ToDate(CurrentQual["begin_date"], DateTypes.START);
                    if (qualTypeCd == "LMES")
                    {
                        Category.SetCheckParameter("NOX_LME_Unit", true, eParameterDataType.Boolean);
                        Category.SetCheckParameter("SO2_LME_Unit", false, eParameterDataType.Boolean);

                        DataView RptingFreqRecs = (DataView)Category.GetCheckParameter("Location_Reporting_Frequency_Records").ParameterValue;
                        string OldFilter = RptingFreqRecs.RowFilter;
                        RptingFreqRecs.RowFilter = AddToDataViewFilter(OldFilter, "report_freq_cd = 'Q'");
                        //RptingFreqRecs.RowFilter = AddEvaluationDateRangeToDataViewFilter(RptingFreqRecs.RowFilter, EvalStartDate, EvalEndDate,
                        //                                                                    false, true, false);
                        //DataView RptingFreqs = FindRows(RptingFreqRecs, EvalStartDate, EvalEndDate, "begin_date", "end_date");
                        RptingFreqRecs.RowFilter = AddToDataViewFilter(RptingFreqRecs.RowFilter, "begin_rpt_period_id <= " + endDateRptPrd +
                            " and (end_rpt_period_id is null or end_rpt_period_id >= " + startDateRptPrd + ")");

                        if (RptingFreqRecs.Count > 0)
                        {
                            DateTime BeginDt = cDateFunctions.StartDateThisQuarter(cDBConvert.ToInteger(RptingFreqRecs[0]["begin_rpt_period_id"]));
                            if (CurrentBeginDate > BeginDt)
                                BeginDt = CurrentBeginDate;

                            DataView QualRecs = (DataView)Category.GetCheckParameter("Qualification_Records").ParameterValue;
                            string QualFilter = QualRecs.RowFilter;
                            //QualRecs.RowFilter = AddToDataViewFilter(QualFilter, "qual_type_cd = 'LMEA' and begin_date like '%" + BeginYear + "%'");
                            QualRecs.RowFilter = AddToDataViewFilter(QualFilter, "qual_type_cd = 'LMEA' and begin_date <= '" + BeginDt + "'");

                            if (QualRecs.Count == 0)
                                Category.CheckCatalogResult = "A";
                            QualRecs.RowFilter = QualFilter;
                        }
                        RptingFreqRecs.RowFilter = OldFilter;
                    }

                    else if (qualTypeCd == "LMEA")
                    {
                        Category.SetCheckParameter("NOX_LME_Unit", false, eParameterDataType.Boolean);
                        Category.SetCheckParameter("SO2_LME_Unit", false, eParameterDataType.Boolean);


                        DataView methodRecords = Category.GetCheckParameter("Method_Records").AsDataView();

                        { // Check for NOX LME Method
                            DataView noxmMethodView
                              = cRowFilter.FindRows(methodRecords,
                                                    new cFilterCondition[]
                                      {
                                        new cFilterCondition("METHOD_CD", "LME"),
                                        new cFilterCondition("PARAMETER_CD", "NOXM"),
                                        //UMCB Date check for less than Evaluation End and not null
                                        new cFilterCondition("BEGIN_DATE", EvalEndDate, eFilterDataType.DateEnded, eFilterConditionRelativeCompare.LessThanOrEqual),
                                        //Program End Date check for greater than Evaluation Begin or is null
                                        new cFilterCondition("END_DATE", EvalStartDate, eFilterDataType.DateEnded, eFilterConditionRelativeCompare.GreaterThanOrEqual)
                                      });

                            if (noxmMethodView.Count > 0)
                                Category.SetCheckParameter("NOX_LME_Unit", true, eParameterDataType.Boolean);
                        }

                        { // Check for SO2 LME Method
                            DataView so2mMethodView
                              = cRowFilter.FindRows(methodRecords,
                                                    new cFilterCondition[]
                                      {
                                        new cFilterCondition("METHOD_CD", "LME"),
                                        new cFilterCondition("PARAMETER_CD", "SO2M"),
                                        //UMCB Date check for less than Evaluation End and not null
                                        new cFilterCondition("BEGIN_DATE", EvalEndDate, eFilterDataType.DateEnded, eFilterConditionRelativeCompare.LessThanOrEqual),
                                        //Program End Date check for greater than Evaluation Begin or is null
                                        new cFilterCondition("END_DATE", EvalStartDate, eFilterDataType.DateEnded, eFilterConditionRelativeCompare.GreaterThanOrEqual)
                                      });

                            if (so2mMethodView.Count > 0)
                                Category.SetCheckParameter("SO2_LME_Unit", true, eParameterDataType.Boolean);
                        }

                        { // Check for RGGI if SO2M and NOXM LME methods do not exist
                            DataView ProgramRecords = (DataView)Category.GetCheckParameter("Location_Program_Records").ParameterValue;
                            string OldFilter = ProgramRecords.RowFilter;

                            if ((Category.GetCheckParameter("NOX_LME_Unit").AsBoolean().Default() == false) &&
                                (Category.GetCheckParameter("SO2_LME_Unit").AsBoolean().Default() == false))
                            {
                                ProgramRecords.RowFilter = AddToDataViewFilter(OldFilter, "prg_cd = 'RGGI' and class <> 'N'");
                                ProgramRecords.RowFilter = AddEvaluationDateRangeToDataViewFilter(ProgramRecords.RowFilter, "unit_monitor_cert_begin_date", "end_date",
                                                                                                  EvalStartDate, EvalEndDate, false, true, false);
                                if (ProgramRecords.Count > 0)
                                {
                                    Category.SetCheckParameter("SO2_LME_Unit", true, eParameterDataType.Boolean);
                                    Category.SetCheckParameter("NOX_LME_Unit", true, eParameterDataType.Boolean);
                                }
                            }

                            ProgramRecords.RowFilter = OldFilter;
                        }


                        { // Check for LMES needed in conjunction with LMEA
                            DataView locationProgramParameterView
                              = cRowFilter.FindRows(Category.GetCheckParameter("Location_Program_Parameter_Records").AsDataView(),
                                                    new cFilterCondition[]
                                      {
                                        new cFilterCondition("PARAMETER_CD", "NOX"),
                                        new cFilterCondition("PRG_CD", MpParameters.ProgramIsOzoneSeasonList, eFilterConditionStringCompare.InList),
                                        new cFilterCondition("REQUIRED_IND", 1, eFilterDataType.Integer),
                                        new cFilterCondition("CLASS", "A,B", eFilterConditionStringCompare.InList),
                                        //UMCB Date check for less than Evaluation End and not null
                                        new cFilterCondition("UMCB_DATE", EvalEndDate, eFilterDataType.DateEnded, eFilterConditionRelativeCompare.LessThanOrEqual),
                                        //Program End Date check for greater than Evaluation Begin or is null
                                        new cFilterCondition("PRG_END_DATE", EvalStartDate, eFilterDataType.DateEnded, eFilterConditionRelativeCompare.GreaterThanOrEqual)
                                      });


                            if (locationProgramParameterView.Count > 0)
                            {
                                locationProgramParameterView.Sort = "UMCB_DATE";

                                // Determine the begin date to use for filtering Qualifications
                                DateTime FilterBeginDate = CurrentBeginDate;
                                {
                                    DateTime TempDate;

                                    TempDate = new DateTime(CurrentBeginDate.Year, 5, 1);
                                    if (TempDate > FilterBeginDate) FilterBeginDate = TempDate;

                                    TempDate = cDBConvert.ToDate(locationProgramParameterView[0]["UMCB_DATE"], DateTypes.START);
                                    if (TempDate > FilterBeginDate) FilterBeginDate = TempDate;
                                }

                                DataView QualRecs = (DataView)Category.GetCheckParameter("Qualification_Records").ParameterValue;
                                string QualFilter = QualRecs.RowFilter;

                                QualRecs.RowFilter = AddToDataViewFilter(QualFilter, string.Format("qual_type_cd = 'LMES' and (begin_date <= '{0}' or begin_date is null)", FilterBeginDate));

                                if (QualRecs.Count == 0)
                                    Category.CheckCatalogResult = "B";

                                QualRecs.RowFilter = QualFilter;
                            }
                        }
                    }
                }
                else
                    Log = false;
            }
            catch (Exception ex)
            { ReturnVal = Category.CheckEngine.FormatError(ex, "QUAL12"); }

            return ReturnVal;
        }
        public static string QUAL14(cCategory Category, ref bool Log) //Required Monitoring System for Qualification
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentQual = (DataRowView)Category.GetCheckParameter("Current_Qualification").ParameterValue;
                string qualTypeCd = cDBConvert.ToString(CurrentQual["qual_type_cd"]);
                string QualSysType = "";
                if (qualTypeCd.InList("PRATA1,PRATA2,COMPLEX,LOWSULF"))
                {
                    if (qualTypeCd == "LOWSULF")
                        QualSysType = "SO2";
                    else
                        QualSysType = "FLOW";
                    Category.SetCheckParameter("Qualification_System_Type", QualSysType, eParameterDataType.String);

                    DataView MonSysRec = (DataView)Category.GetCheckParameter("Monitor_System_Records").ParameterValue;
                    DateTime QualEvalStartDt = (DateTime)Category.GetCheckParameter("Qualification_Evaluation_Start_Date").ParameterValue;
                    DateTime QualEvalEndDt = (DateTime)Category.GetCheckParameter("Qualification_Evaluation_End_Date").ParameterValue;
                    string OldFilter = MonSysRec.RowFilter;
                    MonSysRec.RowFilter = AddToDataViewFilter(OldFilter, "sys_type_cd like '" + QualSysType + "%'");
                    MonSysRec.RowFilter = AddEvaluationDateRangeToDataViewFilter(MonSysRec.RowFilter, QualEvalStartDt, QualEvalEndDt, false, true, false);
                    if (MonSysRec.Count == 0)
                        Category.CheckCatalogResult = "A";
                    MonSysRec.RowFilter = OldFilter;
                }
                else
                    Log = false;
            }
            catch (Exception ex)
            { ReturnVal = Category.CheckEngine.FormatError(ex, "QUAL14"); }

            return ReturnVal;
        }
        public static string QUAL16(cCategory Category, ref bool Log) //Monitoring Qualification Qualification Type Code Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentQual = (DataRowView)Category.GetCheckParameter("Current_Qualification").ParameterValue;
                Category.SetCheckParameter("Qualification_Type_Code_Valid", true, eParameterDataType.Boolean);
                string LcnType = (string)Category.GetCheckParameter("Location_Type").ParameterValue;
                if (CurrentQual["qual_type_cd"] == DBNull.Value)
                {
                    Category.CheckCatalogResult = "A";
                    Category.SetCheckParameter("Qualification_Type_Code_Valid", false, eParameterDataType.Boolean);
                }
                else
                {
                    string qualTypeCd = cDBConvert.ToString(CurrentQual["qual_type_cd"]);
                    if (qualTypeCd.InList("LMEA,LMES,PK,SK,GF,HGAVG"))
                    {
                        if (!LcnType.StartsWith("U"))
                        {
                            Category.CheckCatalogResult = "C";
                            Category.SetCheckParameter("Qualification_Type_Code_Valid", false, eParameterDataType.Boolean);
                        }
                    }
                    else if (qualTypeCd == "LEE")
                    {
                        /* Locate program records for MATS that are active during the qualification record. 
                         * The active check should use the earlier of the UMCB and ERB. 
                         * The ERB could be null and the could accounts for this by defaulting a null ERB to the max date.
                         */
                        if ((MpParameters.LocationProgramRecords.FindRows(
                                                                            new cFilterCondition("PRG_CD", "MATS"),
                                                                            new cFilterCondition("UNIT_MONITOR_CERT_BEGIN_DATE", eFilterConditionRelativeCompare.LessThanOrEqual, MpParameters.CurrentQualification.EndDate.Default(DateTime.MaxValue)),
                                                                            new cFilterCondition("END_DATE", eFilterConditionRelativeCompare.GreaterThanOrEqual, MpParameters.CurrentQualification.BeginDate.Value, eNullDateDefault.Max)
                                                                         ).Count > 0) ||
                            (MpParameters.LocationProgramRecords.FindRows(
                                                                            new cFilterCondition("PRG_CD", "MATS"),
                                                                            new cFilterCondition("EMISSIONS_RECORDING_BEGIN_DATE", eFilterConditionRelativeCompare.LessThanOrEqual, MpParameters.CurrentQualification.EndDate.Default(DateTime.MaxValue), eNullDateDefault.Max),
                                                                            new cFilterCondition("END_DATE", eFilterConditionRelativeCompare.GreaterThanOrEqual, MpParameters.CurrentQualification.BeginDate.Value, eNullDateDefault.Max)
                                                                         ).Count > 0))
                        {
                            if (LcnType.InList("MS,CP,MP"))
                            {
                                Category.CheckCatalogResult = "D";
                                Category.SetCheckParameter("Qualification_Type_Code_Valid", false, eParameterDataType.Boolean);
                            }
                        }
                        else
                        {
                            Category.CheckCatalogResult = "E";
                            Category.SetCheckParameter("Qualification_Type_Code_Valid", false, eParameterDataType.Boolean);
                        }

                    }
                    else
                    {
                        DataView QualTypeCds = (DataView)Category.GetCheckParameter("Qualification_Type_Code_Lookup_Table").ParameterValue;
                        if (!LookupCodeExists(qualTypeCd, "qual_type_cd", QualTypeCds))
                        {
                            Category.CheckCatalogResult = "B";
                            Category.SetCheckParameter("Qualification_Type_Code_Valid", false, eParameterDataType.Boolean);
                        }
                    }
                }
            }
            catch (Exception ex)
            { ReturnVal = Category.CheckEngine.FormatError(ex, "QUAL16"); }

            return ReturnVal;
        }
        public static string QUAL18(cCategory Category, ref bool Log) //Monitoring Qualification Begin Date Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentQual = (DataRowView)Category.GetCheckParameter("Current_Qualification").ParameterValue;

                Category.SetCheckParameter("Monitor_Qualification_Begin_Date_Valid", true, eParameterDataType.Boolean);

                if (CurrentQual["begin_date"] == DBNull.Value)
                {
                    Category.CheckCatalogResult = "A";
                    Category.SetCheckParameter("Monitor_Qualification_Begin_Date_Valid", false, eParameterDataType.Boolean);
                }
                else
                {
                    string qualTypeCd = cDBConvert.ToString(CurrentQual["qual_type_cd"]);
                    DateTime EarliestBeginDate = new DateTime(1993, 1, 1);
                    if (qualTypeCd == "LMEA" || qualTypeCd == "LMES")
                        EarliestBeginDate = new DateTime(2000, 1, 1);
                    else if (qualTypeCd.InList("PK,SK,GF"))
                        EarliestBeginDate = new DateTime(1996, 1, 1);

                    DateTime BeginDate = cDBConvert.ToDate(CurrentQual["begin_date"], DateTypes.START);
                    if (BeginDate < EarliestBeginDate || BeginDate > Category.Process.MaximumFutureDate)
                    {
                        Category.CheckCatalogResult = "B";
                        Category.SetCheckParameter("Monitor_Qualification_Begin_Date_Valid", false, eParameterDataType.Boolean);
                    }
                    else if (qualTypeCd.InList("SK,LMES") && (BeginDate.Month < 5 || BeginDate.Month > 9))
                    {
                        Category.CheckCatalogResult = "B";
                        Category.SetCheckParameter("Monitor_Qualification_Begin_Date_Valid", false, eParameterDataType.Boolean);
                    }
                }
            }
            catch (Exception ex)
            { ReturnVal = Category.CheckEngine.FormatError(ex, "QUAL18"); }

            return ReturnVal;
        }
        public static string QUAL19(cCategory Category, ref bool Log) //Monitoring Qualification End Date Valid
        {
            string ReturnVal = "";

            try
            {
                ReturnVal = Check_ValidEndDate(Category, "Monitor_Qualification_End_Date_Valid", "Current_Qualification");
            }
            catch (Exception ex)
            { ReturnVal = Category.CheckEngine.FormatError(ex, "QUAL19"); }

            return ReturnVal;
        }
        public static string QUAL20(cCategory Category, ref bool Log) //Monitoring Qualification Dates Consistent
        {
            string ReturnVal = "";

            try
            {
                ReturnVal = Check_ConsistentDateRange(Category, "Monitor_Qualification_Dates_Consistent", "Current_Qualification",
                                                        "Monitor_Qualification_Begin_Date_Valid", "Monitor_Qualification_End_Date_Valid");
            }
            catch (Exception ex)
            { ReturnVal = Category.CheckEngine.FormatError(ex, "QUAL20"); }

            return ReturnVal;
        }
        public static string QUAL34(cCategory Category, ref bool Log) //Monitoring Qualification Data Active
        {
            string ReturnVal = "";

            try
            {
                if ((bool)Category.GetCheckParameter("Monitor_Qualification_Dates_Consistent").ParameterValue)
                {
                    ReturnVal = Check_ActiveDateRange(Category, "Current_Qualification_Active", "Current_Qualification",
                                                        "Qualification_Evaluation_Start_Date", "Qualification_Evaluation_End_Date");
                }
                else
                    Log = false;
            }
            catch (Exception ex)
            { ReturnVal = Category.CheckEngine.FormatError(ex, "QUAL34"); }

            return ReturnVal;
        }
        public static string QUAL35(cCategory Category, ref bool Log) //Duplicate Monitoring Qualification Records
        {
            string ReturnVal = "";

            try
            {
                DataView QualRecs = (DataView)Category.GetCheckParameter("Qualification_Records").ParameterValue;
                DataRowView CurrentQual = (DataRowView)Category.GetCheckParameter("Current_Qualification").ParameterValue;
                string qualTypeCd = cDBConvert.ToString(CurrentQual["qual_type_cd"]);
                DateTime beginDt = cDBConvert.ToDate(CurrentQual["begin_date"], DateTypes.START);

                string OldFilter = QualRecs.RowFilter;
                QualRecs.RowFilter = AddToDataViewFilter(OldFilter, "qual_type_cd = '" + qualTypeCd + "' and begin_date = '" + beginDt.ToShortDateString() + "'");

                if (QualRecs.Count > 1 || (QualRecs.Count == 1 &&
                    cDBConvert.ToString(CurrentQual["mon_qual_id"]) != cDBConvert.ToString(QualRecs[0]["mon_qual_id"])))
                    Category.CheckCatalogResult = "A";
                else
                {
                    DateTime endDt = cDBConvert.ToDate(CurrentQual["end_date"], DateTypes.END);
                    if (CurrentQual["end_date"] != DBNull.Value)
                    {
                        QualRecs.RowFilter = AddToDataViewFilter(OldFilter, "qual_type_cd = '" + qualTypeCd + "' and end_date = '" + endDt.ToShortDateString() + "'");

                        if (QualRecs.Count > 1 || (QualRecs.Count == 1 &&
                            cDBConvert.ToString(CurrentQual["mon_qual_id"]) != cDBConvert.ToString(QualRecs[0]["mon_qual_id"])))
                            Category.CheckCatalogResult = "A";
                    }
                }

                QualRecs.RowFilter = OldFilter;
            }
            catch (Exception ex)
            { ReturnVal = Category.CheckEngine.FormatError(ex, "QUAL35"); }

            return ReturnVal;
        }
        public static string QUAL38(cCategory Category, ref bool Log) //Monitoring Qualification Child Records Valid
        {
            string ReturnVal = "";

            try
            {
                bool ValidQualification = (bool)Category.GetCheckParameter("Monitor_Qualification_Valid").ParameterValue;
                bool DatesConsistent = (bool)Category.GetCheckParameter("Monitor_Qualification_Dates_Consistent").ParameterValue;

                DataRowView CurrentQual = (DataRowView)Category.GetCheckParameter("Current_Qualification").ParameterValue;
                string qualTypeCd = cDBConvert.ToString(CurrentQual["qual_type_cd"]);

                if (ValidQualification && DatesConsistent && qualTypeCd.InList("PK,SK,GF,LMEA,LMES,LEE"))
                {
                    DataView QualPctRecs = (DataView)Category.GetCheckParameter("Qualification_Percent_Records").ParameterValue;
                    string QualPctFilter = QualPctRecs.RowFilter;

                    DateTime EvalStartDate = (DateTime)Category.GetCheckParameter("Qualification_Evaluation_Start_Date").ParameterValue;
                    DateTime EvalEndDate = (DateTime)Category.GetCheckParameter("Qualification_Evaluation_End_Date").ParameterValue;
                    int startDateRptPrd = cDateFunctions.ThisReportingPeriod(EvalStartDate);
                    int endDateRptPrd = cDateFunctions.ThisReportingPeriod(EvalEndDate);

                    int PreviousDataYear = int.MinValue;

                    if (qualTypeCd.InList("PK,SK,GF"))
                    {
                        DataView UnitProgReportFreqRecs = (DataView)Category.GetCheckParameter("Location_Reporting_Frequency_Records").ParameterValue;
                        string UnitProgFilter = UnitProgReportFreqRecs.RowFilter;
                        int BeginYear = (cDBConvert.ToDate(CurrentQual["begin_date"], DateTypes.START)).Year;
                        int CurrentYear = DateTime.Now.Year;
                        PreviousDataYear = BeginYear;
                        int EndYear;
                        if (CurrentQual["end_date"] == DBNull.Value)
                            EndYear = CurrentYear;
                        else
                            EndYear = (cDBConvert.ToDate(CurrentQual["end_date"], DateTypes.END)).Year;
                        for (int qual_year = BeginYear; qual_year <= EndYear; qual_year++)
                        {
                            QualPctRecs.RowFilter = AddToDataViewFilter(QualPctFilter, "mon_qual_id = '" + cDBConvert.ToString(CurrentQual["mon_qual_id"]) +
                                                                        "' and qual_year = '" + qual_year + "'");
                            if (QualPctRecs.Count > 0)
                            {
                                int Yr1QualDataYr = cDBConvert.ToInteger(QualPctRecs[0]["yr1_qual_data_year"]);

                                if (Yr1QualDataYr > BeginYear)
                                {
                                    if (Yr1QualDataYr != PreviousDataYear + 1)
                                    {
                                        Category.CheckCatalogResult = "A";
                                        break;
                                    }
                                    else
                                        PreviousDataYear = Yr1QualDataYr;
                                }
                                else
                                {   //qual_year-2 <= Yr1QualDataYr <= qual_year
                                    if (Yr1QualDataYr == qual_year || Yr1QualDataYr + 1 == qual_year || Yr1QualDataYr + 2 == qual_year)
                                    {
                                        if ((bool)Category.GetCheckParameter("Initial_Qualification").ParameterValue == false)
                                        {
                                            Category.CheckCatalogResult = "B";
                                            break;
                                        }
                                    }
                                }

                            }
                            else //if not found
                            {
                                int March = 3, June = 6;
                                int CurrentMonth = DateTime.Now.Month;
                                if (qual_year < CurrentYear || (qual_year == CurrentYear && CurrentMonth > June))
                                {
                                    Category.CheckCatalogResult = "C";
                                    break;
                                }
                                else if (qual_year == CurrentYear && CurrentMonth > March)
                                {
                                    UnitProgReportFreqRecs.RowFilter = AddToDataViewFilter(UnitProgFilter, "report_freq_cd = 'Q'");
                                    //UnitProgReportFreqRecs.RowFilter = AddEvaluationDateRangeToDataViewFilter(UnitProgReportFreqRecs.RowFilter, EvalStartDate, EvalEndDate,
                                    //                                                                            false, true, false);
                                    //DataView UPReportFreqRecs = FindRows(UnitProgReportFreqRecs, EvalStartDate, EvalEndDate, "begin_date", "end_date");
                                    UnitProgReportFreqRecs.RowFilter = AddToDataViewFilter(UnitProgReportFreqRecs.RowFilter, "begin_rpt_period_id <= " + endDateRptPrd +
                                        " and (end_rpt_period_id is null or end_rpt_period_id >= " + startDateRptPrd + ")");

                                    if (UnitProgReportFreqRecs.Count > 0)
                                    {
                                        Category.CheckCatalogResult = "C";
                                        break;
                                    }

                                }
                            }
                        }
                        UnitProgReportFreqRecs.RowFilter = UnitProgFilter;
                    }
                    else if (qualTypeCd.StartsWith("LME"))
                    {
                        DataView QualLMERecs = (DataView)Category.GetCheckParameter("QualificationLME_Records").ParameterValue;
                        string QualLMEFilter = QualLMERecs.RowFilter;
                        QualLMERecs.RowFilter = AddToDataViewFilter(QualLMEFilter, "mon_qual_id = '" + cDBConvert.ToString(CurrentQual["mon_qual_id"]) + "'");

                        if (QualLMERecs.Count != 3)
                            Category.CheckCatalogResult = "D";
                        else
                        {
                            QualLMERecs.Sort = "qual_data_year";
                            int BeginYear = (cDBConvert.ToDate(CurrentQual["begin_date"], DateTypes.START)).Year;

                            foreach (DataRowView QualLMERec in QualLMERecs)
                            {
                                int qualDataYear = cDBConvert.ToInteger(QualLMERec["qual_data_year"]);
                                if (QualLMERec == QualLMERecs[0]) //if first rec
                                {
                                    if (qualDataYear > BeginYear || qualDataYear + 3 < BeginYear)
                                    {
                                        Category.CheckCatalogResult = "E";
                                        break;
                                    }
                                    else
                                        PreviousDataYear = qualDataYear;
                                }
                                else
                                {
                                    if (qualDataYear != PreviousDataYear + 1)
                                    {
                                        Category.CheckCatalogResult = "E";
                                        break;
                                    }
                                    else
                                        PreviousDataYear = qualDataYear;
                                }
                            }
                        }
                        QualLMERecs.RowFilter = QualLMEFilter;
                    }
                    else if (qualTypeCd == "LEE")
                    {
                        if ((bool)MpParameters.QualificationTypeCodeValid)
                        {
                            if (MpParameters.QualificationleeRecords.SourceView.Count == 0)
                            {
                                Category.CheckCatalogResult = "F";
                            }
                        }
                    }
                    QualPctRecs.RowFilter = QualPctFilter;

                }
                else
                    Log = false;

            }
            catch (Exception ex)
            { ReturnVal = Category.CheckEngine.FormatError(ex, "QUAL38"); }

            return ReturnVal;
        }
        public static string QUAL39(cCategory Category, ref bool Log) //Overlapping Monitoring Qualification Record
        {
            string ReturnVal = "";

            try
            {
                bool qualTypeValid = (bool)Category.GetCheckParameter("Qualification_Type_Code_Valid").ParameterValue;
                bool qualConsNonLoadBasedInd = (bool)Category.GetCheckParameter("Qualification_Consistent_with_Non_Load_Based_Indicator").ParameterValue;
                bool qualConsFuel = (bool)Category.GetCheckParameter("Qualification_Consistent_with_Fuel").ParameterValue;
                bool qualConsProgRprtFreq = (bool)Category.GetCheckParameter("Qualification_Consistent_with_Program_and_Reporting_Frequency").ParameterValue;
                DataRowView CurrentQual = (DataRowView)Category.GetCheckParameter("Current_Qualification").ParameterValue;
                string qualTypeCd = cDBConvert.ToString(CurrentQual["qual_type_cd"]);

                if (!qualTypeValid || !qualConsNonLoadBasedInd || !qualConsFuel || !qualConsProgRprtFreq)
                    Category.SetCheckParameter("Monitor_Qualification_Valid", false, eParameterDataType.Boolean);
                else
                {
                    Category.SetCheckParameter("Monitor_Qualification_Valid", true, eParameterDataType.Boolean);

                    DataView QualRecs = (DataView)Category.GetCheckParameter("Qualification_Records").ParameterValue;
                    string CurrentMonQualId = cDBConvert.ToString(CurrentQual["mon_qual_id"]);
                    string OldFilter = QualRecs.RowFilter;
                    QualRecs.RowFilter = AddToDataViewFilter(OldFilter, "mon_qual_id <> '" + CurrentMonQualId + "' and qual_type_cd = '" + qualTypeCd + "' and begin_date < '" +
                                                             (cDBConvert.ToDate(CurrentQual["begin_date"], DateTypes.START)).ToShortDateString() + "'");
                    if (QualRecs.Count > 0)
                    {
                        DateTime CurrentBeginDate = cDBConvert.ToDate(CurrentQual["begin_date"], DateTypes.START);
                        Category.SetCheckParameter("Initial_Qualification", false, eParameterDataType.Boolean);

                        foreach (DataRowView drQual in QualRecs)
                        {
                            DateTime RtrvdEndDate = cDBConvert.ToDate(drQual["end_date"], DateTypes.END);

                            if (drQual["end_date"] == DBNull.Value || RtrvdEndDate >= CurrentBeginDate)
                            {
                                Category.SetCheckParameter("Monitor_Qualification_Valid", false, eParameterDataType.Boolean);
                                Category.CheckCatalogResult = "A";
                                break;
                            }
                        }
                    }
                    else
                        Category.SetCheckParameter("Initial_Qualification", true, eParameterDataType.Boolean);

                    QualRecs.RowFilter = OldFilter;
                }

            }
            catch (Exception ex)
            { ReturnVal = Category.CheckEngine.FormatError(ex, "QUAL39"); }

            return ReturnVal;
        }

        #endregion

        #region qualification percent

        public static string QUAL7(cCategory Category, ref bool Log) //Monitoring Qualification Percent Qualification Year Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentQualPct = (DataRowView)Category.GetCheckParameter("Current_Qualification_Percent").ParameterValue;
                DataRowView CurrentQual = (DataRowView)Category.GetCheckParameter("Current_Qualification").ParameterValue;
                bool DatesConsistent = (bool)Category.GetCheckParameter("Monitor_Qualification_Dates_Consistent").ParameterValue;
                int qualYear = cDBConvert.ToInteger(CurrentQualPct["qual_year"]);
                DateTime EndDate = cDBConvert.ToDate(CurrentQual["end_date"], DateTypes.END);

                bool qualYrValid = true;

                if (CurrentQualPct["qual_year"] == DBNull.Value)
                {
                    Category.CheckCatalogResult = "A";
                    qualYrValid = false;
                }
                else if (DatesConsistent && qualYear < (cDBConvert.ToDate(CurrentQual["begin_date"], DateTypes.START)).Year)
                {
                    Category.CheckCatalogResult = "B";
                    qualYrValid = false;
                }
                else if (CurrentQual["end_date"] == DBNull.Value)
                {
                    if ((qualYear > DateTime.Now.Year) && (CurrentQualPct["YR1_QUAL_DATA_TYPE_CD"].AsString() != "P"))
                    {
                        Category.CheckCatalogResult = "B";
                        qualYrValid = false;
                    }
                }
                else
                {
                    if (DatesConsistent && qualYear > ((cDBConvert.ToDate(CurrentQual["end_date"], DateTypes.END)).Year + 1))
                    {
                        Category.CheckCatalogResult = "B";
                        qualYrValid = false;
                    }
                }
                Category.SetCheckParameter("Monitor_Qualification_Percent_Qualification_Year_Valid", qualYrValid, eParameterDataType.Boolean);
            }
            catch (Exception ex)
            { ReturnVal = Category.CheckEngine.FormatError(ex, "QUAL7"); }

            return ReturnVal;
        }
        public static string QUAL8(cCategory Category, ref bool Log) //Monitoring Qualification Percent Average Percent Value Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentQualPct = (DataRowView)Category.GetCheckParameter("Current_Qualification_Percent").ParameterValue;
                decimal avgPctVal = cDBConvert.ToDecimal(CurrentQualPct["avg_pct_value"]);

                Category.SetCheckParameter("Calculated_Average_Percent_Value", null, eParameterDataType.Decimal);

                if (CurrentQualPct["avg_pct_value"] == DBNull.Value)
                    Category.CheckCatalogResult = "A";
                else if (avgPctVal < 0 || avgPctVal > 100)
                    Category.CheckCatalogResult = "B";
                else
                {
                    decimal Yr1PctVal = cDBConvert.ToDecimal(CurrentQualPct["yr1_pct_value"]);
                    decimal Yr2PctVal = cDBConvert.ToDecimal(CurrentQualPct["yr2_pct_value"]);
                    decimal Yr3PctVal = cDBConvert.ToDecimal(CurrentQualPct["yr3_pct_value"]);

                    if ((Yr1PctVal >= 0 && Yr1PctVal <= 100) && (Yr2PctVal >= 0 && Yr2PctVal <= 100) && (Yr3PctVal >= 0 && Yr3PctVal <= 100))
                    {
                        decimal calcAvgPctVal = (Yr1PctVal + Yr2PctVal + Yr3PctVal) / 3;
                        calcAvgPctVal = Math.Round(calcAvgPctVal, 1, MidpointRounding.AwayFromZero);
                        Category.SetCheckParameter("Calculated_Average_Percent_Value", calcAvgPctVal, eParameterDataType.Decimal);
                        if (avgPctVal != calcAvgPctVal)
                            Category.CheckCatalogResult = "C";
                    }
                }
            }
            catch (Exception ex)
            { ReturnVal = Category.CheckEngine.FormatError(ex, "QUAL8"); }

            return ReturnVal;
        }
        public static string QUAL11(cCategory Category, ref bool Log) //Qualification Eligibility Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentQualPct = (DataRowView)Category.GetCheckParameter("Current_Qualification_Percent").ParameterValue;
                DataRowView CurrentQual = (DataRowView)Category.GetCheckParameter("Current_Qualification").ParameterValue;
                bool endDtValid = (bool)Category.GetCheckParameter("Monitor_Qualification_End_Date_Valid").ParameterValue;
                bool qualYrValid = (bool)Category.GetCheckParameter("Monitor_Qualification_Percent_Qualification_Year_Valid").ParameterValue;
                decimal CalcAvgPctVal = cDBConvert.ToDecimal(Category.GetCheckParameter("Calculated_Average_Percent_Value").ParameterValue);

                DateTime EndDate = cDBConvert.ToDate(CurrentQual["end_date"], DateTypes.END);
                int QualYear = cDBConvert.ToInteger(CurrentQualPct["qual_year"]);
                string QualTypeCd = cDBConvert.ToString(CurrentQual["qual_type_cd"]);
                decimal Yr1PctVal = cDBConvert.ToDecimal(CurrentQualPct["yr1_pct_value"]);
                decimal Yr2PctVal = cDBConvert.ToDecimal(CurrentQualPct["yr2_pct_value"]);
                decimal Yr3PctVal = cDBConvert.ToDecimal(CurrentQualPct["yr3_pct_value"]);
                if (CurrentQual["end_date"] == DBNull.Value || (qualYrValid && endDtValid && QualYear < EndDate.Year))
                {
                    if (QualTypeCd == "GF")
                    {
                        if ((Yr1PctVal >= 0 && Yr1PctVal < 85.0m) || (Yr2PctVal >= 0 && Yr2PctVal < 85.0m) || (Yr3PctVal >= 0 && Yr3PctVal < 85.0m))
                            Category.CheckCatalogResult = "A";
                        else if (CalcAvgPctVal < 90.0m && CalcAvgPctVal != decimal.MinValue)
                            Category.CheckCatalogResult = "B";
                    }
                    else
                    {
                        if ((Yr1PctVal > 20.0m && Yr1PctVal <= 100.0m) || (Yr2PctVal > 20.0m && Yr2PctVal <= 100.0m) || (Yr3PctVal > 20.0m && Yr3PctVal <= 100.0m))
                            Category.CheckCatalogResult = "C";
                        else if (CalcAvgPctVal > 10.0m)
                            Category.CheckCatalogResult = "D";
                    }
                }
                else if (CurrentQual["end_date"] != DBNull.Value && (qualYrValid && endDtValid && QualYear == (EndDate.Year + 1)))
                {
                    if (QualTypeCd == "GF")
                    {
                        if ((Yr1PctVal >= 85.0m && Yr1PctVal <= 100.0m) && (Yr2PctVal >= 85.0m && Yr2PctVal <= 100.0m) &&
                            (Yr3PctVal >= 85.0m && Yr3PctVal <= 100.0m) && CalcAvgPctVal >= 90.0m)
                            Category.CheckCatalogResult = "E";
                    }
                    else
                    {
                        if ((Yr1PctVal >= 0 && Yr1PctVal <= 20.0m) && (Yr2PctVal >= 0 && Yr2PctVal <= 20.0m) &&
                            (Yr3PctVal >= 0 && Yr3PctVal <= 20.0m) && (CalcAvgPctVal <= 10.0m && CalcAvgPctVal != decimal.MinValue))
                            Category.CheckCatalogResult = "F";
                    }
                }
            }
            catch (Exception ex)
            { ReturnVal = Category.CheckEngine.FormatError(ex, "QUAL11"); }

            return ReturnVal;
        }
        public static string QUAL27(cCategory Category, ref bool Log) //Monitoring Qualification Percent Year1 Data Type Code Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentQualPct = (DataRowView)Category.GetCheckParameter("Current_Qualification_Percent").ParameterValue;
                bool QualPctQualYrValid = (bool)Category.GetCheckParameter("Monitor_Qualification_Percent_Qualification_Year_Valid").ParameterValue;
                bool QualPctYr1QualYrValid = (bool)Category.GetCheckParameter("Monitor_Qualification_Percent_Yr1_Qualification_Data_Year_Valid").ParameterValue;

                string Yr1QualDataTypeCd = cDBConvert.ToString(CurrentQualPct["yr1_qual_data_type_cd"]);
                int QualYear = cDBConvert.ToInteger(CurrentQualPct["qual_year"]);
                int Yr1QualDataYr = cDBConvert.ToInteger(CurrentQualPct["yr1_qual_data_year"]);

                if (CurrentQualPct["yr1_qual_data_type_cd"] == DBNull.Value)
                    Category.CheckCatalogResult = "A";
                else if (Yr1QualDataTypeCd == "P")
                {
                    if (QualPctYr1QualYrValid && QualPctQualYrValid && Yr1QualDataYr < QualYear)
                        Category.CheckCatalogResult = "B";
                }
                else if (Yr1QualDataTypeCd == "A")
                {
                    if (QualPctYr1QualYrValid && QualPctQualYrValid && Yr1QualDataYr >= QualYear)
                        Category.CheckCatalogResult = "C";
                }
                else if (Yr1QualDataTypeCd == "D")
                {
                    DataRowView CurrentQual = (DataRowView)Category.GetCheckParameter("Current_Qualification").ParameterValue;
                    //if associated qual type cd ! = GF return D
                    if (cDBConvert.ToString(CurrentQual["qual_type_cd"]) != "GF")
                        Category.CheckCatalogResult = "D";

                    else if (QualPctYr1QualYrValid && QualPctQualYrValid && Yr1QualDataYr != QualYear)
                        Category.CheckCatalogResult = "E";
                }
                else
                    Category.CheckCatalogResult = "D";

            }
            catch (Exception ex)
            { ReturnVal = Category.CheckEngine.FormatError(ex, "QUAL27"); }

            return ReturnVal;
        }
        public static string QUAL28(cCategory Category, ref bool Log) //Monitoring Qualification Percent Year1 Data Year Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentQualPct = (DataRowView)Category.GetCheckParameter("Current_Qualification_Percent").ParameterValue;
                bool QualPctQualYrValid = (bool)Category.GetCheckParameter("Monitor_Qualification_Percent_Qualification_Year_Valid").ParameterValue;
                int Yr1QualDataYr = cDBConvert.ToInteger(CurrentQualPct["yr1_qual_data_year"]);
                int QualYear = cDBConvert.ToInteger(CurrentQualPct["qual_year"]);
                Category.SetCheckParameter("Monitor_Qualification_Percent_Yr1_Qualification_Data_Year_Valid", true, eParameterDataType.Boolean);

                if (CurrentQualPct["yr1_qual_data_year"] == DBNull.Value)
                {
                    Category.CheckCatalogResult = "A";
                    Category.SetCheckParameter("Monitor_Qualification_Percent_Yr1_Qualification_Data_Year_Valid", false, eParameterDataType.Boolean);
                }
                else if (Yr1QualDataYr < 1990)
                {
                    Category.CheckCatalogResult = "B";
                    Category.SetCheckParameter("Monitor_Qualification_Percent_Yr1_Qualification_Data_Year_Valid", false, eParameterDataType.Boolean);
                }
                else if (QualPctQualYrValid && (Yr1QualDataYr > QualYear || Yr1QualDataYr + 3 < QualYear))
                {
                    Category.CheckCatalogResult = "B";
                    Category.SetCheckParameter("Monitor_Qualification_Percent_Yr1_Qualification_Data_Year_Valid", false, eParameterDataType.Boolean);
                }
            }
            catch (Exception ex)
            { ReturnVal = Category.CheckEngine.FormatError(ex, "QUAL28"); }

            return ReturnVal;
        }
        public static string QUAL29(cCategory Category, ref bool Log) //Monitoring Qualification Percent Year1 Percentage Value Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentQualPct = (DataRowView)Category.GetCheckParameter("Current_Qualification_Percent").ParameterValue;

                if (CurrentQualPct["yr1_pct_value"] == DBNull.Value)
                    Category.CheckCatalogResult = "A";
                else
                {
                    decimal yr1PctVal = cDBConvert.ToDecimal(CurrentQualPct["yr1_pct_value"]);
                    if (yr1PctVal < 0 || yr1PctVal > 100)
                        Category.CheckCatalogResult = "B";
                }
            }
            catch (Exception ex)
            { ReturnVal = Category.CheckEngine.FormatError(ex, "QUAL29"); }

            return ReturnVal;
        }
        public static string QUAL36(cCategory Category, ref bool Log) //Duplicate Monitoring Qualification Percent Records
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentQualPct = (DataRowView)Category.GetCheckParameter("Current_Qualification_Percent").ParameterValue;
                DataView QualPctRecs = (DataView)Category.GetCheckParameter("Qualification_Percent_Records").ParameterValue;

                //string OldFilter = QualPctRecs.RowFilter;
                //QualPctRecs.RowFilter = AddToDataViewFilter(OldFilter, "qual_year = '" + cDBConvert.ToInteger(CurrentQualPct["qual_year"]) + "'");
                //if (QualPctRecs.Count > 1 || QualPctRecs.Count == 1 && CurrentQualPct["mon_pct_id"] != QualPctRecs[0]["mon_pct_id"])
                //    Category.CheckCatalogResult = "A";
                //QualPctRecs.RowFilter = OldFilter;

                sFilterPair[] Criteria = new sFilterPair[2];
                Criteria[0].Field = "qual_year";
                Criteria[0].Value = cDBConvert.ToInteger(CurrentQualPct["qual_year"]);
                Criteria[1].Field = "mon_qual_id";
                Criteria[1].Value = cDBConvert.ToString(CurrentQualPct["mon_qual_id"]);

                if (DuplicateRecordCheck(CurrentQualPct, QualPctRecs, "mon_pct_id", Criteria))
                    Category.CheckCatalogResult = "A";
            }
            catch (Exception ex)
            { ReturnVal = Category.CheckEngine.FormatError(ex, "QUAL36"); }

            return ReturnVal;
        }
        public static string QUAL40(cCategory Category, ref bool Log) //Monitoring Qualification Percent Year2 Data Type Code Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentQualPct = (DataRowView)Category.GetCheckParameter("Current_Qualification_Percent").ParameterValue;
                bool QualPctQualYrValid = (bool)Category.GetCheckParameter("Monitor_Qualification_Percent_Qualification_Year_Valid").ParameterValue;
                bool QualPctYr2QualYrValid = (bool)Category.GetCheckParameter("Monitor_Qualification_Percent_Yr2_Qualification_Data_Year_Valid").ParameterValue;

                string Yr2QualDataTypeCd = cDBConvert.ToString(CurrentQualPct["yr2_qual_data_type_cd"]);
                int QualYear = cDBConvert.ToInteger(CurrentQualPct["qual_year"]);
                int Yr2QualDataYr = cDBConvert.ToInteger(CurrentQualPct["yr2_qual_data_year"]);

                if (CurrentQualPct["yr2_qual_data_type_cd"] == DBNull.Value)
                    Category.CheckCatalogResult = "A";
                else if (Yr2QualDataTypeCd == "P")
                {
                    if (QualPctYr2QualYrValid && QualPctQualYrValid && Yr2QualDataYr < QualYear)
                        Category.CheckCatalogResult = "B";
                }
                else if (Yr2QualDataTypeCd == "A")
                {
                    if (QualPctYr2QualYrValid && QualPctQualYrValid && Yr2QualDataYr >= QualYear)
                        Category.CheckCatalogResult = "C";
                }
                else
                    Category.CheckCatalogResult = "D";
            }
            catch (Exception ex)
            { ReturnVal = Category.CheckEngine.FormatError(ex, "QUAL40"); }

            return ReturnVal;
        }
        public static string QUAL41(cCategory Category, ref bool Log) //Monitoring Qualification Percent Year2 Percentage Value Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentQualPct = (DataRowView)Category.GetCheckParameter("Current_Qualification_Percent").ParameterValue;
                if (CurrentQualPct["yr2_pct_value"] == DBNull.Value)
                    Category.CheckCatalogResult = "A";
                else
                {
                    decimal yr2PctVal = cDBConvert.ToDecimal(CurrentQualPct["yr2_pct_value"]);
                    if (yr2PctVal < 0 || yr2PctVal > 100)
                        Category.CheckCatalogResult = "B";
                }
            }
            catch (Exception ex)
            { ReturnVal = Category.CheckEngine.FormatError(ex, "QUAL41"); }

            return ReturnVal;
        }
        public static string QUAL42(cCategory Category, ref bool Log) //Monitoring Qualification Percent Year2 Data Year Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentQualPct = (DataRowView)Category.GetCheckParameter("Current_Qualification_Percent").ParameterValue;
                bool QualPctYr1QualYrValid = (bool)Category.GetCheckParameter("Monitor_Qualification_Percent_Yr1_Qualification_Data_Year_Valid").ParameterValue;
                int Yr2QualDataYr = cDBConvert.ToInteger(CurrentQualPct["yr2_qual_data_year"]);
                int Yr1QualDataYr = cDBConvert.ToInteger(CurrentQualPct["yr1_qual_data_year"]);
                int QualYear = cDBConvert.ToInteger(CurrentQualPct["qual_year"]);
                Category.SetCheckParameter("Monitor_Qualification_Percent_Yr2_Qualification_Data_Year_Valid", true, eParameterDataType.Boolean);

                if (CurrentQualPct["yr2_qual_data_year"] == DBNull.Value)
                {
                    Category.CheckCatalogResult = "A";
                    Category.SetCheckParameter("Monitor_Qualification_Percent_Yr2_Qualification_Data_Year_Valid", false, eParameterDataType.Boolean);
                }
                else if (Yr2QualDataYr < 1990)
                {
                    Category.CheckCatalogResult = "B";
                    Category.SetCheckParameter("Monitor_Qualification_Percent_Yr2_Qualification_Data_Year_Valid", false, eParameterDataType.Boolean);
                }
                else if (QualPctYr1QualYrValid && !(Yr1QualDataYr + 1 == Yr2QualDataYr))
                {
                    Category.CheckCatalogResult = "B";
                    Category.SetCheckParameter("Monitor_Qualification_Percent_Yr2_Qualification_Data_Year_Valid", false, eParameterDataType.Boolean);
                }
            }
            catch (Exception ex)
            { ReturnVal = Category.CheckEngine.FormatError(ex, "QUAL42"); }

            return ReturnVal;
        }
        public static string QUAL43(cCategory Category, ref bool Log) //Monitoring Qualification Percent Year3 Data Type Code Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentQualPct = (DataRowView)Category.GetCheckParameter("Current_Qualification_Percent").ParameterValue;
                bool QualPctQualYrValid = (bool)Category.GetCheckParameter("Monitor_Qualification_Percent_Qualification_Year_Valid").ParameterValue;
                bool QualPctYr3QualYrValid = (bool)Category.GetCheckParameter("Monitor_Qualification_Percent_Yr3_Qualification_Data_Year_Valid").ParameterValue;

                string Yr3QualDataTypeCd = cDBConvert.ToString(CurrentQualPct["yr3_qual_data_type_cd"]);
                int QualYear = cDBConvert.ToInteger(CurrentQualPct["qual_year"]);
                int Yr3QualDataYr = cDBConvert.ToInteger(CurrentQualPct["yr3_qual_data_year"]);

                if (CurrentQualPct["yr3_qual_data_type_cd"] == DBNull.Value)
                    Category.CheckCatalogResult = "A";
                else if (Yr3QualDataTypeCd == "P")
                {
                    if (QualPctYr3QualYrValid && QualPctQualYrValid && Yr3QualDataYr < QualYear)
                        Category.CheckCatalogResult = "B";
                }
                else if (Yr3QualDataTypeCd == "A")
                {
                    if (QualPctYr3QualYrValid && QualPctQualYrValid && Yr3QualDataYr >= QualYear)
                        Category.CheckCatalogResult = "C";
                }
                else
                    Category.CheckCatalogResult = "D";
            }
            catch (Exception ex)
            { ReturnVal = Category.CheckEngine.FormatError(ex, "QUAL43"); }

            return ReturnVal;
        }
        public static string QUAL44(cCategory Category, ref bool Log) //Monitoring Qualification Percent Year3 Percentage Value Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentQualPct = (DataRowView)Category.GetCheckParameter("Current_Qualification_Percent").ParameterValue;
                if (CurrentQualPct["yr3_pct_value"] == DBNull.Value)
                    Category.CheckCatalogResult = "A";
                else
                {
                    decimal yr3PctVal = cDBConvert.ToDecimal(CurrentQualPct["yr3_pct_value"]);
                    if (yr3PctVal < 0 || yr3PctVal > 100)
                        Category.CheckCatalogResult = "B";
                }
            }
            catch (Exception ex)
            { ReturnVal = Category.CheckEngine.FormatError(ex, "QUAL44"); }

            return ReturnVal;
        }
        public static string QUAL45(cCategory Category, ref bool Log) //Monitoring Qualification Percent Year3 Data Year Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentQualPct = (DataRowView)Category.GetCheckParameter("Current_Qualification_Percent").ParameterValue;
                bool QualPctYr2QualYrValid = (bool)Category.GetCheckParameter("Monitor_Qualification_Percent_Yr2_Qualification_Data_Year_Valid").ParameterValue;
                int Yr3QualDataYr = cDBConvert.ToInteger(CurrentQualPct["yr3_qual_data_year"]);
                int Yr2QualDataYr = cDBConvert.ToInteger(CurrentQualPct["yr2_qual_data_year"]);
                int QualYear = cDBConvert.ToInteger(CurrentQualPct["qual_year"]);
                Category.SetCheckParameter("Monitor_Qualification_Percent_Yr3_Qualification_Data_Year_Valid", true, eParameterDataType.Boolean);

                if (CurrentQualPct["yr3_qual_data_year"] == DBNull.Value)
                {
                    Category.CheckCatalogResult = "A";
                    Category.SetCheckParameter("Monitor_Qualification_Percent_Yr3_Qualification_Data_Year_Valid", false, eParameterDataType.Boolean);
                }
                else if (Yr3QualDataYr < 1990)
                {
                    Category.CheckCatalogResult = "B";
                    Category.SetCheckParameter("Monitor_Qualification_Percent_Yr3_Qualification_Data_Year_Valid", false, eParameterDataType.Boolean);
                }
                else if (QualPctYr2QualYrValid && !(Yr2QualDataYr + 1 == Yr3QualDataYr))
                {
                    Category.CheckCatalogResult = "B";
                    Category.SetCheckParameter("Monitor_Qualification_Percent_Yr3_Qualification_Data_Year_Valid", false, eParameterDataType.Boolean);
                }
            }
            catch (Exception ex)
            { ReturnVal = Category.CheckEngine.FormatError(ex, "QUAL45"); }

            return ReturnVal;
        }

        #endregion

        #region qualification lme

        public static string QUAL22(cCategory Category, ref bool Log) //Monitoring Qualification LME Data Qualification Year Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView QualLMERec = Category.GetCheckParameter("Current_Qualification_LME").ValueAsDataRowView();
                DataRowView QualRec = Category.GetCheckParameter("Current_Qualification").ValueAsDataRowView();

                int qualDataYr = cDBConvert.ToInteger(QualLMERec["qual_data_year"]);
                int currBgnYr = cDBConvert.ToDate(QualRec["begin_date"], DateTypes.START).Year;

                if (qualDataYr == int.MinValue)
                    Category.CheckCatalogResult = "A";
                else if (qualDataYr > currBgnYr + 2 || qualDataYr < currBgnYr - 3)
                    Category.CheckCatalogResult = "B";
            }
            catch (Exception ex)
            { ReturnVal = Category.CheckEngine.FormatError(ex, "QUAL22"); }

            return ReturnVal;
        }

        public static string QUAL23(cCategory Category, ref bool Log) //Monitoring Qualification LME Data SO2 Tons Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView QualLMERec = Category.GetCheckParameter("Current_Qualification_LME").ValueAsDataRowView();
                bool SO2LMEUnit = Category.GetCheckParameter("SO2_LME_Unit").ValueAsBool();

                if (SO2LMEUnit)
                {
                    decimal SO2Tons = cDBConvert.ToDecimal(QualLMERec["SO2_Tons"]);
                    if (QualLMERec["SO2_Tons"] == DBNull.Value)
                        Category.CheckCatalogResult = "A";
                    else if (SO2Tons < 0)
                        Category.CheckCatalogResult = "B";
                    else if (SO2Tons > 25)
                        Category.CheckCatalogResult = "C";
                }
                else
                {
                    if (QualLMERec["SO2_Tons"] != DBNull.Value)
                        Category.CheckCatalogResult = "D";
                }

            }
            catch (Exception ex)
            { ReturnVal = Category.CheckEngine.FormatError(ex, "QUAL23"); }

            return ReturnVal;
        }

        public static string QUAL24(cCategory Category, ref bool Log) //Monitoring Qualification LME Data NOx Tons Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView QualLMERec = Category.GetCheckParameter("Current_Qualification_LME").ValueAsDataRowView();
                DataRowView QualRec = Category.GetCheckParameter("Current_Qualification").ValueAsDataRowView();
                bool NOXLMEUnit = Category.GetCheckParameter("NOX_LME_Unit").ValueAsBool();

                if (NOXLMEUnit)
                {
                    decimal NoxTons = cDBConvert.ToDecimal(QualLMERec["NOX_TONS"]);
                    string QualType = cDBConvert.ToString(QualRec["qual_type_cd"]);

                    int beginYr = cDBConvert.ToDate(QualRec["begin_date"], DateTypes.START).Year;

                    if (QualLMERec["NOX_TONS"] == DBNull.Value)
                        Category.CheckCatalogResult = "A";
                    else if (NoxTons < 0)
                        Category.CheckCatalogResult = "B";
                    else if (QualType == "LMES")
                    {
                        if (beginYr < 2002)
                        {
                            if (NoxTons > 25)
                                Category.CheckCatalogResult = "C";
                        }
                        else
                        {
                            if (NoxTons > 50)
                                Category.CheckCatalogResult = "C";
                        }
                    }
                    else
                    {
                        if (beginYr < 2002)
                        {
                            if (NoxTons > 50)
                                Category.CheckCatalogResult = "C";
                        }
                        else
                        {
                            if (NoxTons > 100)
                                Category.CheckCatalogResult = "C";
                        }
                    }

                }
                else
                {
                    if (QualLMERec["NOX_TONS"] != DBNull.Value)
                        Category.CheckCatalogResult = "D";
                }

            }
            catch (Exception ex)
            { ReturnVal = Category.CheckEngine.FormatError(ex, "QUAL24"); }

            return ReturnVal;
        }

        public static string QUAL25(cCategory Category, ref bool Log) //Monitoring Qualification LME Data Operating Hours Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView QualLMERec = Category.GetCheckParameter("Current_Qualification_LME").ValueAsDataRowView();
                int OpHrs = cDBConvert.ToInteger(QualLMERec["op_hours"]);

                if (OpHrs == int.MinValue)
                    Category.CheckCatalogResult = "A";
                else if (OpHrs < 0 || OpHrs > 8784)
                    Category.CheckCatalogResult = "B";
            }
            catch (Exception ex)
            { ReturnVal = Category.CheckEngine.FormatError(ex, "QUAL25"); }

            return ReturnVal;
        }


        public static string QUAL37(cCategory Category, ref bool Log) //Duplicate Monitoring Qualification Percent Records
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentQualLME = (DataRowView)Category.GetCheckParameter("Current_Qualification_LME").ParameterValue;
                DataView QualLMERecs = (DataView)Category.GetCheckParameter("QualificationLME_Records").ParameterValue;

                //string OldFilter = QualLMERecs.RowFilter;
                //QualLMERecs.RowFilter = AddToDataViewFilter(OldFilter, "qual_data_year = '" + cDBConvert.ToInteger(CurrentQualLME["qual_data_year"]) + "'");
                //if (QualLMERecs.Count > 1 || QualLMERecs.Count == 1 && CurrentQualLME["mon_lme_id"] != QualLMERecs[0]["mon_lme_id"])
                //    Category.CheckCatalogResult = "A";
                //QualLMERecs.RowFilter = OldFilter;

                sFilterPair[] Criteria = new sFilterPair[2];
                Criteria[0].Field = "qual_data_year";
                Criteria[0].Value = cDBConvert.ToInteger(CurrentQualLME["qual_data_year"]);
                Criteria[1].Field = "mon_qual_id";
                Criteria[1].Value = cDBConvert.ToString(CurrentQualLME["mon_qual_id"]);

                if (DuplicateRecordCheck(CurrentQualLME, QualLMERecs, "mon_lme_id", Criteria))
                    Category.CheckCatalogResult = "A";
            }
            catch (Exception ex)
            { ReturnVal = Category.CheckEngine.FormatError(ex, "QUAL37"); }

            return ReturnVal;
        }

        public static string QUAL46(cCategory Category, ref bool Log) //Monitoring Qualification LME Data NOx Tons Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView QualLMERec = Category.GetCheckParameter("Current_Qualification_LME").ValueAsDataRowView();
                DataRowView QualRec = Category.GetCheckParameter("Current_Qualification").ValueAsDataRowView();

                decimal NoxTons = cDBConvert.ToDecimal(QualLMERec["NOX_TONS"]);
                string QualType = cDBConvert.ToString(QualRec["qual_type_cd"]);

                int beginYr = cDBConvert.ToDate(QualRec["begin_date"], DateTypes.START).Year;

                if (QualLMERec["NOX_TONS"] != DBNull.Value)
                {
                    if (NoxTons < 0)
                        Category.CheckCatalogResult = "A";
                    else if (QualType == "LMES")
                    {
                        if (beginYr < 2002)
                        {
                            if (NoxTons > 25)
                                Category.CheckCatalogResult = "B";
                        }
                        else
                        {
                            if (NoxTons > 50)
                                Category.CheckCatalogResult = "B";
                        }
                    }
                    else
                    {
                        if (beginYr < 2002)
                        {
                            if (NoxTons > 50)
                                Category.CheckCatalogResult = "B";
                        }
                        else
                        {
                            if (NoxTons > 100)
                                Category.CheckCatalogResult = "B";
                        }
                    }
                }

            }
            catch (Exception ex)
            { ReturnVal = Category.CheckEngine.FormatError(ex, "QUAL46"); }

            return ReturnVal;
        }

        public static string QUAL47(cCategory Category, ref bool Log) //Monitoring Qualification LME Data SO2 Tons Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView QualLMERec = Category.GetCheckParameter("Current_Qualification_LME").ValueAsDataRowView();
                DataRowView QualRec = Category.GetCheckParameter("Current_Qualification").ValueAsDataRowView();

                string QualType = cDBConvert.ToString(QualRec["qual_type_cd"]);

                decimal SO2Tons = cDBConvert.ToDecimal(QualLMERec["SO2_Tons"]);
                if (QualLMERec["SO2_Tons"] != DBNull.Value)
                {
                    if (QualType == "LMES")
                        Category.CheckCatalogResult = "C";
                    else if (SO2Tons < 0)
                        Category.CheckCatalogResult = "A";
                    else if (SO2Tons > 25)
                        Category.CheckCatalogResult = "B";
                }


            }
            catch (Exception ex)
            { ReturnVal = Category.CheckEngine.FormatError(ex, "QUAL47"); }

            return ReturnVal;
        }

        #endregion

    }
}
