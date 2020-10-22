using System;
using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;
using ECMPS.Checks.Em.Parameters;

using ECMPS.Definitions.Extensions;


namespace ECMPS.Checks.LMEChecks
{
    public class cLMEChecks : cChecks
    {
        public cLMEChecks()
        {
            CheckProcedures = new dCheckProcedure[47];

            CheckProcedures[1] = new dCheckProcedure(LME1);
            CheckProcedures[2] = new dCheckProcedure(LME2);
            //FCheckProcedures[3] = new dCheckProcedure(LME3);
            CheckProcedures[4] = new dCheckProcedure(LME4);
            CheckProcedures[5] = new dCheckProcedure(LME5);
            CheckProcedures[6] = new dCheckProcedure(LME6);
            CheckProcedures[7] = new dCheckProcedure(LME7);
            CheckProcedures[8] = new dCheckProcedure(LME8);
            CheckProcedures[9] = new dCheckProcedure(LME9);
            CheckProcedures[10] = new dCheckProcedure(LME10);
            CheckProcedures[11] = new dCheckProcedure(LME11);
            CheckProcedures[12] = new dCheckProcedure(LME12);
            CheckProcedures[13] = new dCheckProcedure(LME13);
            CheckProcedures[14] = new dCheckProcedure(LME14);
            CheckProcedures[15] = new dCheckProcedure(LME15);
            CheckProcedures[16] = new dCheckProcedure(LME16);
            CheckProcedures[17] = new dCheckProcedure(LME17);
            CheckProcedures[18] = new dCheckProcedure(LME18);
            CheckProcedures[19] = new dCheckProcedure(LME19);
            CheckProcedures[20] = new dCheckProcedure(LME20);
            CheckProcedures[21] = new dCheckProcedure(LME21);
            CheckProcedures[22] = new dCheckProcedure(LME22);
            CheckProcedures[23] = new dCheckProcedure(LME23);
            CheckProcedures[24] = new dCheckProcedure(LME24);
            CheckProcedures[25] = new dCheckProcedure(LME25);
            CheckProcedures[26] = new dCheckProcedure(LME26);
            CheckProcedures[27] = new dCheckProcedure(LME27);
            CheckProcedures[28] = new dCheckProcedure(LME28);
            CheckProcedures[29] = new dCheckProcedure(LME29);
            CheckProcedures[30] = new dCheckProcedure(LME30);
            CheckProcedures[31] = new dCheckProcedure(LME31);
            CheckProcedures[32] = new dCheckProcedure(LME32);
            CheckProcedures[33] = new dCheckProcedure(LME33);
            CheckProcedures[34] = new dCheckProcedure(LME34);
            CheckProcedures[35] = new dCheckProcedure(LME35);
            CheckProcedures[36] = new dCheckProcedure(LME36);
            CheckProcedures[37] = new dCheckProcedure(LME37);
            CheckProcedures[38] = new dCheckProcedure(LME38);
            CheckProcedures[39] = new dCheckProcedure(LME39);
            CheckProcedures[40] = new dCheckProcedure(LME40);

            CheckProcedures[41] = new dCheckProcedure(LME41);
            CheckProcedures[42] = new dCheckProcedure(LME42);
            CheckProcedures[43] = new dCheckProcedure(LME43);
            CheckProcedures[44] = new dCheckProcedure(LME44);
            CheckProcedures[45] = new dCheckProcedure(LME45);
            CheckProcedures[46] = new dCheckProcedure(LME46);

        }

        #region Checks  1 - 10

        public static string LME1(cCategory Category, ref bool Log) //LME Facility and Units Present in the Production Facility Table
        {
            string ReturnVal = "";
            try
            {
                Category.SetCheckParameter("LME_Facility_ID", null, eParameterDataType.String);
                DataView LMEImportFile = Category.GetCheckParameter("Workspace_LME_Records").ValueAsDataView();
                string sOrisCds = ColumnToDatalist(LMEImportFile, "oris_code", false);

                if (sOrisCds.ListCount(",") != 1)
                    Category.CheckCatalogResult = "A";
                else if (sOrisCds.ListCount(",") == 1)
                {
                    string sLMEOris = cDBConvert.ToString(LMEImportFile[0]["oris_code"]);
                    Category.SetCheckParameter("LME_ORIS_Code", sLMEOris, eParameterDataType.String);

                    DataView ProdFacRecs = Category.GetCheckParameter("Production_Facility_Records").ValueAsDataView();
                    string sFacilityFilter = ProdFacRecs.RowFilter;
                    ProdFacRecs.RowFilter = AddToDataViewFilter(sFacilityFilter, "oris_code = '" + sLMEOris + "'");
                    if (ProdFacRecs.Count == 0)
                        Category.CheckCatalogResult = "B";
                    else
                    {
                        DataView ProdUnitRecs = Category.GetCheckParameter("Production_Unit_Records").ValueAsDataView();
                        string sUnitFilter = ProdUnitRecs.RowFilter;
                        bool bAllFound = true;
                        foreach (DataRowView wsLMERow in LMEImportFile)
                        {
                            string sOrisCd = cDBConvert.ToString(wsLMERow["oris_code"]);
                            string sLocation = cDBConvert.ToString(wsLMERow["unit_pipe"]);
                            ProdUnitRecs.RowFilter = AddToDataViewFilter(sUnitFilter, "oris_code = '" + sOrisCd + "' and unitid = '" + sLocation + "'");
                            if (ProdUnitRecs.Count == 0)
                            {
                                bAllFound = false;
                                Category.CheckCatalogResult = "C";
                                break;
                            }
                        }
                        ProdUnitRecs.RowFilter = sUnitFilter;
                        if (bAllFound)
                        {
                            string sFacId = cDBConvert.ToString(ProdFacRecs[0]["fac_id"]);
                            Category.SetCheckParameter("LME_Facility_ID", sFacId, eParameterDataType.String);
                        }
                    }
                    ProdFacRecs.RowFilter = sFacilityFilter;
                }

            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "LME1");
            }
            return ReturnVal;
        }

        public static string LME2(cCategory Category, ref bool Log) //LME File Configuration Valid
        {
            string ReturnVal = "";
            try
            {
                DataView LMEImportFile = Category.GetCheckParameter("Workspace_LME_Records").ValueAsDataView();
                string sFacId = Category.GetCheckParameter("LME_Facility_ID").ValueAsString();
                bool beginDateValid = true;
                if (sFacId != "")
                {
                    DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
                    dtfi.ShortDatePattern = "yyyyMMdd";
                    DateTime dtBeginDate;
                    string sBeginDate;
                    foreach (DataRowView LMERow in LMEImportFile)
                    {
                        sBeginDate = cDBConvert.ToString(LMERow["begin_date"]);

                        if (!DateTime.TryParseExact(sBeginDate, "d", dtfi, DateTimeStyles.AssumeLocal, out dtBeginDate))
                        {
                            //   Category.CheckCatalogResult = "A";
                            beginDateValid = false;
                            break;
                        }
                        else if (dtBeginDate.Year < 1993 || dtBeginDate.Year > 2030)
                        {
                            beginDateValid = false;
                            break;
                        }
                    }
                    if (!beginDateValid)
                        Category.CheckCatalogResult = "A";
                    else
                    {
                        string OldSort = LMEImportFile.Sort;
                        LMEImportFile.Sort = "begin_date";

                        string sFirstBeginDt = cDBConvert.ToString(LMEImportFile[0]["begin_date"]);
                        DateTime EarliestBeginDate = DateTime.ParseExact(sFirstBeginDt, "d", dtfi);

                        string sLastBeginDt = cDBConvert.ToString(LMEImportFile[LMEImportFile.Count - 1]["begin_date"]);
                        DateTime LatestBeginDate = DateTime.ParseExact(sLastBeginDt, "d", dtfi);

                        DataView ReportingPeriodTable = Category.GetCheckParameter("Reporting_Period_Lookup_Table").ValueAsDataView();
                        string sFilter = ReportingPeriodTable.RowFilter;
                        ReportingPeriodTable.RowFilter = AddToDataViewFilter(sFilter, "begin_date <= '" + EarliestBeginDate + "' and end_date >= '" + EarliestBeginDate + "'");
                        int EarliestRptPeriod = cDBConvert.ToInteger(ReportingPeriodTable[0]["rpt_period_id"]);
                        ReportingPeriodTable.RowFilter = AddToDataViewFilter(sFilter, "begin_date <= '" + LatestBeginDate + "' and end_date >= '" + LatestBeginDate + "'");
                        int LatestRptPeriod = cDBConvert.ToInteger(ReportingPeriodTable[0]["rpt_period_id"]);
                        ReportingPeriodTable.RowFilter = sFilter;

                        if (EarliestRptPeriod != LatestRptPeriod)
                            Category.CheckCatalogResult = "B";
                        else
                        {
                            DataView MonitorMethods = Category.GetCheckParameter("Method_Records").ValueAsDataView();
                            string locns = ColumnToDatalist(LMEImportFile, "oris_code", "int", "unit_pipe", "string", false);
                            locns = "'" + locns.Replace(",", "','") + "'";

                            DataView MonPlanLcnRecs = Category.GetCheckParameter("Monitoring_Plan_Location_Records").ValueAsDataView();
                            string sMonPlanLocnFilter = MonPlanLcnRecs.RowFilter;
                            MonPlanLcnRecs.RowFilter = AddToDataViewFilter(sMonPlanLocnFilter, "oris_code+location_name in (" + locns + ")");
                            string monLocs = ColumnToDatalist(MonPlanLcnRecs, "mon_loc_id");
                            monLocs = "'" + monLocs.Replace(",", "','") + "'";

                            string sMethodFilter = MonitorMethods.RowFilter;
                            MonitorMethods.RowFilter = AddToDataViewFilter(sMethodFilter, "mon_loc_id in (" + monLocs + ") and parameter_cd in ('SO2M','NOXM','CO2M') and method_cd = 'LME'");
                            MonitorMethods.RowFilter = AddEvaluationDateRangeToDataViewFilter(MonitorMethods.RowFilter, EarliestBeginDate, LatestBeginDate, true, true, false);
                            if (MonitorMethods.Count == 0)
                                Category.CheckCatalogResult = "C";
                            else
                            {
                                Category.SetCheckParameter("LME_Reporting_Period_ID", EarliestRptPeriod, eParameterDataType.Integer);

                                DataView ProdMonPlanRecs = Category.GetCheckParameter("Monitor_Plan_Records").ValueAsDataView();

                                string sMonPlanFilter = ProdMonPlanRecs.RowFilter;

                                string MonPlanIds = ColumnToDatalist(MonPlanLcnRecs, "mon_plan_id");
                                MonPlanIds = "'" + MonPlanIds.Replace(",", "','") + "'";
                                ProdMonPlanRecs.RowFilter = AddToDataViewFilter(sMonPlanFilter, "fac_id = '" + sFacId + "' and begin_rpt_period_id <= " + EarliestRptPeriod +
                                    " and (end_rpt_period_id is null or end_rpt_period_id >= " + LatestRptPeriod + ") and mon_plan_id in (" + MonPlanIds + ")");

                                if (ProdMonPlanRecs.Count != 1)
                                    Category.CheckCatalogResult = "D";
                                else
                                {
                                    Category.SetCheckParameter("LME_MP_ID", cDBConvert.ToString(ProdMonPlanRecs[0]["mon_plan_id"]), eParameterDataType.String);
                                }
                                ProdMonPlanRecs.RowFilter = sMonPlanFilter;

                            }
                            MonPlanLcnRecs.RowFilter = sMonPlanLocnFilter;
                            MonitorMethods.RowFilter = sMethodFilter;
                        }
                        LMEImportFile.Sort = OldSort;
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "LME2");
            }
            return ReturnVal;
        }

        public static string LME4(cCategory Category, ref bool Log) //Check LME Import Begin Hour
        {
            string ReturnVal = "";
            try
            {
                DataRowView CurrentWSLME = Category.GetCheckParameter("Current_Workspace_LME_Hour").ValueAsDataRowView();

                int BeginHour = int.MinValue;
                try
                {
                    BeginHour = int.Parse(cDBConvert.ToString(CurrentWSLME["begin_hour"]));
                }
                catch (FormatException)
                {
                    Category.CheckCatalogResult = "A";
                }
                if (BeginHour < 0 || BeginHour > 23)
                    Category.CheckCatalogResult = "A";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "LME4");
            }
            return ReturnVal;
        }

        public static string LME5(cCategory Category, ref bool Log) //Check LME Import Op Time
        {
            string ReturnVal = "";
            try
            {
                DataRowView CurrentWSLME = Category.GetCheckParameter("Current_Workspace_LME_Hour").ValueAsDataRowView();

                string sOpTime = cDBConvert.ToString(CurrentWSLME["op_time"]);
                if (!string.IsNullOrEmpty(sOpTime))
                {
                    decimal OpTime;
                    if (!decimal.TryParse(sOpTime, out OpTime))
                    {
                        Category.CheckCatalogResult = "A";
                    }
                    else
                    {
                        if (OpTime < 0.00m || OpTime > 1.00m)
                            Category.CheckCatalogResult = "A";
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "LME5");
            }
            return ReturnVal;
        }

        public static string LME6(cCategory Category, ref bool Log) //Check LME Import Load Value
        {
            string ReturnVal = "";
            try
            {
                DataRowView CurrentWSLME = Category.GetCheckParameter("Current_Workspace_LME_Hour").ValueAsDataRowView();

                string sLoadVal = cDBConvert.ToString(CurrentWSLME["hr_load"]);
                if (!string.IsNullOrEmpty(sLoadVal))
                {
                    decimal LoadVal = -1;
                    if (!decimal.TryParse(sLoadVal, out LoadVal))
                    {
                        Category.CheckCatalogResult = "A";
                    }
                    else
                    {
                        if (LoadVal < 0M)
                            Category.CheckCatalogResult = "A";
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "LME6");
            }
            return ReturnVal;
        }

        public static string LME7(cCategory Category, ref bool Log) //Check LME Import Load UOM
        {
            string ReturnVal = "";
            try
            {
                DataRowView CurrentWSLME = Category.GetCheckParameter("Current_Workspace_LME_Hour").ValueAsDataRowView();
                string LoadUOMCd = cDBConvert.ToString(CurrentWSLME["LOAD_UOM_CD"]);
                if (!LoadUOMCd.InList("MW,,KLBHR,MMBTUHR"))
                    Category.CheckCatalogResult = "A";

            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "LME7");
            }
            return ReturnVal;
        }

        public static string LME8(cCategory Category, ref bool Log) //Check LME Import Fuel Code List
        {
            string ReturnVal = "";
            try
            {
                DataRowView CurrentWSLME = Category.GetCheckParameter("Current_Workspace_LME_Hour").ValueAsDataRowView();
                string FuelCdList = cDBConvert.ToString(CurrentWSLME["fuel_cd_list"]);
                if (FuelCdList != "")
                {
                    string[] FuelCds = FuelCdList.Split(';');

                    DataView FuelCdLookupTbl = Category.GetCheckParameter("Fuel_Code_Lookup_Table").ValueAsDataView();
                    string Filter = FuelCdLookupTbl.RowFilter;
                    foreach (string fuel in FuelCds)
                    {
                        FuelCdLookupTbl.RowFilter = AddToDataViewFilter(Filter, "fuel_cd = '" + fuel + "'");
                        if (FuelCdLookupTbl.Count == 0 || !cDBConvert.ToString(FuelCdLookupTbl[0]["fuel_group_cd"]).InList("OIL,GAS"))
                        {
                            Category.CheckCatalogResult = "A";
                            break;
                        }
                    }
                    FuelCdLookupTbl.RowFilter = Filter;
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "LME8");
            }
            return ReturnVal;
        }

        public static string LME9(cCategory Category, ref bool Log) //Check LME Import Operating Condition Code
        {
            string ReturnVal = "";
            try
            {
                DataRowView CurrentWSLME = Category.GetCheckParameter("Current_Workspace_LME_Hour").ValueAsDataRowView();
                string OpCondnCd = cDBConvert.ToString(CurrentWSLME["OPERATING_CONDITION_CD"]);
                if (!OpCondnCd.InList("C,,U,B,P"))
                    Category.CheckCatalogResult = "A";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "LME9");
            }
            return ReturnVal;
        }

        public static string LME10(cCategory Category, ref bool Log)
        {
            string ReturnVal = "";
            try
            {
                int LocationCount = cDBConvert.ToInteger(Category.GetCheckParameter("Current_Location_Count").ParameterValue);

                decimal[] LMEGenLTFFHIArray = new decimal[LocationCount];
                decimal[] LMEGenTotalHIArray = new decimal[LocationCount];
                decimal[] LMEGenTotalLoadArray = new decimal[LocationCount];
                decimal[] LMEGenTotalSO2MArray = new decimal[LocationCount];
                decimal[] LMEGenTotalNOXMArray = new decimal[LocationCount];
                decimal[] LMEGenTotalCO2MArray = new decimal[LocationCount];
                decimal[] LMEGenTotalOpTimeArray = new decimal[LocationCount];
                int[] LMEGenTotalOpHrsArray = new int[LocationCount];
                decimal[] LMEGenLTFFAprilHIArray = new decimal[LocationCount];
                decimal[] LMEGenAprilHIArray = new decimal[LocationCount];
                decimal[] LMEGenAprilLoadArray = new decimal[LocationCount];
                decimal[] LMEGenAprilNOXMArray = new decimal[LocationCount];
                decimal[] LMEGenAprilOpTimeArray = new decimal[LocationCount];
                int[] LMEGenAprilOpHrsArray = new int[LocationCount];
                decimal[] LMEGenLTFFAprilOpTimeArray = new decimal[LocationCount];
                decimal[] LMEGenLTFFTotalOpTimeArray = new decimal[LocationCount];

                for (int LocationIndex = 0; LocationIndex < LocationCount; LocationIndex++)
                {
                    LMEGenLTFFHIArray[LocationIndex] = 0;
                    LMEGenTotalHIArray[LocationIndex] = 0;
                    LMEGenTotalLoadArray[LocationIndex] = 0;
                    LMEGenTotalSO2MArray[LocationIndex] = 0;
                    LMEGenTotalNOXMArray[LocationIndex] = 0;
                    LMEGenTotalCO2MArray[LocationIndex] = 0;
                    LMEGenTotalOpTimeArray[LocationIndex] = 0;
                    LMEGenTotalOpHrsArray[LocationIndex] = 0;
                    LMEGenLTFFAprilHIArray[LocationIndex] = 0;
                    LMEGenAprilHIArray[LocationIndex] = 0;
                    LMEGenAprilLoadArray[LocationIndex] = 0;
                    LMEGenAprilNOXMArray[LocationIndex] = 0;
                    LMEGenAprilOpTimeArray[LocationIndex] = 0;
                    LMEGenAprilOpHrsArray[LocationIndex] = 0;
                    LMEGenLTFFTotalOpTimeArray[LocationIndex] = 0;
                    LMEGenLTFFAprilOpTimeArray[LocationIndex] = 0;
                }

                Category.SetCheckParameter("LME_Gen_LTFF_Heat_Input_Array", LMEGenLTFFHIArray, eParameterDataType.Decimal, true, true);
                Category.SetCheckParameter("LME_Gen_Total_Heat_Input_Array", LMEGenTotalHIArray, eParameterDataType.Decimal, true, true);
                Category.SetCheckParameter("LME_Gen_Total_Load_Array", LMEGenTotalLoadArray, eParameterDataType.Decimal, true, true);
                Category.SetCheckParameter("LME_Gen_Total_SO2M_Array", LMEGenTotalSO2MArray, eParameterDataType.Decimal, true, true);
                Category.SetCheckParameter("LME_Gen_Total_NOXM_Array", LMEGenTotalNOXMArray, eParameterDataType.Decimal, true, true);
                Category.SetCheckParameter("LME_Gen_Total_CO2M_Array", LMEGenTotalCO2MArray, eParameterDataType.Decimal, true, true);
                Category.SetCheckParameter("LME_Gen_Total_Op_Time_Array", LMEGenTotalOpTimeArray, eParameterDataType.Decimal, true, true);
                Category.SetCheckParameter("LME_Gen_Total_Op_Hours_Array", LMEGenTotalOpHrsArray, eParameterDataType.Integer, true, true);
                Category.SetCheckParameter("LME_Gen_LTFF_April_Heat_Input_Array", LMEGenLTFFAprilHIArray, eParameterDataType.Decimal, true, true);
                Category.SetCheckParameter("LME_Gen_April_Heat_Input_Array", LMEGenAprilHIArray, eParameterDataType.Decimal, true, true);
                Category.SetCheckParameter("LME_Gen_April_Load_Array", LMEGenAprilLoadArray, eParameterDataType.Decimal, true, true);
                Category.SetCheckParameter("LME_Gen_April_NOXM_Array", LMEGenAprilNOXMArray, eParameterDataType.Decimal, true, true);
                Category.SetCheckParameter("LME_Gen_April_Op_Time_Array", LMEGenAprilOpTimeArray, eParameterDataType.Decimal, true, true);
                Category.SetCheckParameter("LME_Gen_April_Op_Hours_Array", LMEGenAprilOpHrsArray, eParameterDataType.Integer, true, true);
                Category.SetCheckParameter("LME_Gen_LTFF_Total_Op_Time_Array", LMEGenLTFFTotalOpTimeArray, eParameterDataType.Decimal, true, true);
                Category.SetCheckParameter("LME_Gen_LTFF_April_Op_Time_Array", LMEGenLTFFAprilOpTimeArray, eParameterDataType.Decimal, true, true);

                Category.SetCheckParameter("LME_Gen_CP_Total_Heat_Input", 0.0m, eParameterDataType.Decimal);
                Category.SetCheckParameter("LME_Gen_Total_Load", 0.0m, eParameterDataType.Decimal);
                Category.SetCheckParameter("LME_Gen_Total_Optime", 0.0m, eParameterDataType.Decimal);
                Category.SetCheckParameter("LME_Gen_CP_April_Heat_Input", 0.0m, eParameterDataType.Decimal);
                Category.SetCheckParameter("LME_Gen_April_Load", 0.0m, eParameterDataType.Decimal);
                Category.SetCheckParameter("LME_Gen_April_Optime", 0.0m, eParameterDataType.Decimal);

                Category.SetCheckParameter("LME_Gen_Annual", false, eParameterDataType.Boolean);
                Category.SetCheckParameter("LME_Gen_OS", false, eParameterDataType.Boolean);

                Category.SetCheckParameter("LME_Gen_HI_Method", null, eParameterDataType.String);
                Category.SetCheckParameter("LME_Gen_HI_Substitute_Data_Code", null, eParameterDataType.String);

                int RptPeriodId = Category.GetCheckParameter("Current_Reporting_Period").ValueAsInt();
                DataView RptPeriodTable = Category.GetCheckParameter("Reporting_Period_Lookup_Table").ValueAsDataView();
                sFilterPair[] RptPeriodCriteria = new sFilterPair[1];
                RptPeriodCriteria[0].Field = "rpt_period_id";
                RptPeriodCriteria[0].Value = RptPeriodId;
                DataRowView RptPeriodRecord = FindRow(RptPeriodTable, RptPeriodCriteria);

                DateTime RptPeriodBeginDate = cDBConvert.ToDate(RptPeriodRecord["begin_date"], DateTypes.START);
                DateTime RptPeriodEndDate = cDBConvert.ToDate(RptPeriodRecord["end_date"], DateTypes.END);

                DataView MPLocationRecords = Category.GetCheckParameter("Monitoring_Plan_Location_Records").ValueAsDataView();
                string MonitorPlanMonLocList = cDataFunctions.ColumnToDatalist(MPLocationRecords, "Mon_Loc_Id");

                DataView MethodRecords = Category.GetCheckParameter("MP_Method_Records").ValueAsDataView();
                string sMethodFilter = MethodRecords.RowFilter;
                MethodRecords.RowFilter = AddToDataViewFilter(sMethodFilter, "parameter_cd = 'HIT'");
                MethodRecords.RowFilter = AddEvaluationDateRangeToDataViewFilter(MethodRecords.RowFilter, RptPeriodBeginDate, RptPeriodEndDate, true, true, false);

                string[] sMPLocns = MonitorPlanMonLocList.Split(',');

                bool bMissingLocn = false;
                string tempFilter = MethodRecords.RowFilter;
                foreach (string locn in sMPLocns)
                {
                    MethodRecords.RowFilter = AddToDataViewFilter(tempFilter, "mon_loc_id = '" + locn + "'");
                    if (MethodRecords.Count == 0)
                    {
                        Category.CheckCatalogResult = "A";
                        bMissingLocn = true;
                        break;
                    }
                }
                MethodRecords.RowFilter = tempFilter;

                if (!bMissingLocn)
                {
                    bool bLMEGenAnnual = false;
                    bool bLMEGenOS = false;

                    int rptPrdQrtr = cDBConvert.ToInteger(RptPeriodRecord["quarter"]);
                    int nLMEYearStartQtr = rptPrdQrtr;

                    string sLocnFilter = MPLocationRecords.RowFilter;
                    MPLocationRecords.RowFilter = AddToDataViewFilter(sLocnFilter, "unit_id is not null");
                    string sUnitLocns = cDataFunctions.ColumnToDatalist(MPLocationRecords, "mon_loc_id");
                    MPLocationRecords.RowFilter = sLocnFilter;
                    MPLocationRecords.Sort = "mon_loc_id";
                    string FilterUnitLocations = "('" + sUnitLocns.Replace(",", "','") + "')";

                    DataView MPQualRecords = Category.GetCheckParameter("MP_Qualification_Records").ValueAsDataView();
                    string sQualFilter = MPQualRecords.RowFilter;

                    MPQualRecords.RowFilter = AddToDataViewFilter(sQualFilter, "mon_loc_id in " + FilterUnitLocations + " and qual_type_cd in ('LMEA','LMES')");
                    MPQualRecords.RowFilter = AddEvaluationDateRangeToDataViewFilter(MPQualRecords.RowFilter, RptPeriodEndDate, new DateTime(RptPeriodEndDate.Year, 1, 1), true, true, false);

                    foreach (DataRowView QualRec in MPQualRecords)
                    {
                        if (cDBConvert.ToString(QualRec["qual_type_cd"]) == "LMEA")
                            bLMEGenAnnual = true;
                        else if (cDBConvert.ToString(QualRec["qual_type_cd"]) == "LMES")
                            bLMEGenOS = true;
                    }
                    if (!bLMEGenAnnual && !bLMEGenOS)
                        Category.CheckCatalogResult = "B";
                    else if (!bLMEGenAnnual && (rptPrdQrtr == 1 || rptPrdQrtr == 4))
                        Category.CheckCatalogResult = "C";
                    else
                    {
                        if (rptPrdQrtr > 1)
                        {
                            if (bLMEGenAnnual)
                                nLMEYearStartQtr = 1;
                            else
                                nLMEYearStartQtr = 2;
                        }

                        bool bMHHI = true;
                        bool bLTFF = true;
                        bool bsetSubs = false;
                        string MethodCd;
                        foreach (DataRowView MethodRecord in MethodRecords)
                        {
                            MethodCd = cDBConvert.ToString(MethodRecord["method_cd"]);
                            if (MethodCd != "MHHI")
                            {
                                bMHHI = false;
                                break;
                            }
                        }
                        if (bMHHI)
                        {
                            Category.SetCheckParameter("LME_Gen_HI_Method", "MHHI", eParameterDataType.String);
                            DataView LTFFRecs = Category.GetCheckParameter("LTFF_Records").ValueAsDataView();
                            string LTFFFilter = LTFFRecs.RowFilter;
                            LTFFRecs.RowFilter = AddToDataViewFilter(LTFFFilter, "rpt_period_id = " + RptPeriodId);
                            if (LTFFRecs.Count > 0)
                                Category.CheckCatalogResult = "D";
                            LTFFRecs.RowFilter = LTFFFilter;
                        }
                        else
                        {
                            foreach (DataRowView MethodRecord in MethodRecords)
                            {
                                MethodCd = cDBConvert.ToString(MethodRecord["method_cd"]);
                                if (MethodCd != "LTFF" && MethodCd != "CALC" && MethodCd != "LTFCALC")
                                {
                                    bLTFF = false;
                                    break;
                                }
                                if (cDBConvert.ToString(MethodRecord["sub_data_cd"]) == "MHHI")
                                    bsetSubs = true;
                            }
                            if (bLTFF)
                            {
                                Category.SetCheckParameter("LME_Gen_HI_Method", "LTFF", eParameterDataType.String);
                                if (bsetSubs)
                                    Category.SetCheckParameter("LME_Gen_HI_Substitute_Data_Code", "MHHI", eParameterDataType.String);

                                DataView HrlyOpRecs = Category.GetCheckParameter("Hourly_Operating_Data_Records_for_LME_Configuration").ValueAsDataView();
                                string HrlyOpSort = HrlyOpRecs.Sort;
                                HrlyOpRecs.Sort = "mon_loc_id";
                                int locnIndex = 0;

                                foreach (DataRowView HrlyOp in HrlyOpRecs)
                                {
                                    if (cDBConvert.ToDecimal(HrlyOp["hr_load"]) < 0M && HrlyOp["hr_load"] != DBNull.Value)
                                    {
                                        Category.CheckCatalogResult = "E";
                                        break;
                                    }
                                    decimal Optime = cDBConvert.ToDecimal(HrlyOp["op_time"]);
                                    if (Optime < 0.0m || Optime > 1.0m)
                                    {
                                        Category.CheckCatalogResult = "F";
                                        break;
                                    }
                                    if (Optime > 0 && HrlyOp["hr_load"] == DBNull.Value)
                                    {
                                        Category.CheckCatalogResult = "E";
                                        break;
                                    }
                                    else
                                    {
                                        if (Optime > 0 && cDBConvert.ToInteger(HrlyOp["mhhi_indicator"]) != 1)
                                        {
                                            string lcn = cDBConvert.ToString(HrlyOp["mon_loc_id"]);
                                            decimal hrLoad = cDBConvert.ToDecimal(HrlyOp["hr_load"]);
                                            decimal opTime = cDBConvert.ToDecimal(HrlyOp["op_time"]);

                                            decimal TotalLoad = hrLoad * opTime;

                                            while (locnIndex < LocationCount)
                                            {
                                                if (MPLocationRecords[locnIndex]["mon_loc_id"].ToString() == lcn)
                                                    break;
                                                locnIndex++;
                                            }
                                            Category.AccumCheckAggregate("LME_Gen_Total_Load_Array", locnIndex, TotalLoad);
                                            Category.AccumCheckAggregate("LME_Gen_Total_Load", TotalLoad);
                                            Category.AccumCheckAggregate("LME_Gen_Total_Optime", opTime);
                                            Category.AccumCheckAggregate("LME_Gen_LTFF_Total_Op_Time_Array", locnIndex, opTime);

                                            if (cDBConvert.ToDate(HrlyOp["begin_date"], DateTypes.START).Month == 4 && bLMEGenOS)
                                            {
                                                Category.AccumCheckAggregate("LME_Gen_April_Load_Array", locnIndex, TotalLoad);
                                                Category.AccumCheckAggregate("LME_Gen_April_Load", TotalLoad);
                                                Category.AccumCheckAggregate("LME_Gen_April_Optime", opTime);
                                                Category.AccumCheckAggregate("LME_Gen_LTFF_April_Op_Time_Array", locnIndex, opTime);
                                            }
                                        }
                                    }
                                } // end foreach hourly op data record

                                if (String.IsNullOrEmpty(Category.CheckCatalogResult))
                                {
                                    decimal LMEGenTotalLoad = Category.GetCheckParameter("LME_Gen_Total_Load").ValueAsDecimal();
                                    decimal LMEGenTotalOptime = Category.GetCheckParameter("LME_Gen_Total_Optime").ValueAsDecimal();

                                    DataView LTFFRecs = Category.GetCheckParameter("LTFF_Records").ValueAsDataView();
                                    string LTFFFilter = LTFFRecs.RowFilter;

                                    if (bLMEGenOS && rptPrdQrtr == 2)
                                    {
                                        decimal LMEGenAprilLoad = Category.GetCheckParameter("LME_Gen_April_Load").ValueAsDecimal();
                                        decimal LMEGenAprilOpTime = Category.GetCheckParameter("LME_Gen_April_Optime").ValueAsDecimal();

                                        LTFFRecs.RowFilter = AddToDataViewFilter(LTFFFilter, string.Format("rpt_period_id = {0} and FUEL_FLOW_PERIOD_CD='A'", RptPeriodId));
                                        if (LTFFRecs.Count > 0 && LMEGenAprilLoad == 0m && LMEGenAprilOpTime == 0m)
                                        {
                                            Category.CheckCatalogResult = "J";
                                        }
                                        else if (LTFFRecs.Count == 0 && (LMEGenAprilLoad > 0m || LMEGenAprilOpTime > 1m))
                                        {
                                            Category.CheckCatalogResult = "K";
                                        }
                                        else
                                        {
                                            LTFFRecs.RowFilter = AddToDataViewFilter(LTFFFilter, string.Format("rpt_period_id = {0} and FUEL_FLOW_PERIOD_CD='MJ'", RptPeriodId));
                                            if (LTFFRecs.Count > 0)
                                            {
                                                if (((LMEGenTotalLoad - LMEGenAprilLoad) == 0m) && ((LMEGenTotalOptime - LMEGenAprilOpTime) == 0m))
                                                    Category.CheckCatalogResult = "L";
                                            }
                                            else
                                            {
                                                if (((LMEGenTotalLoad - LMEGenAprilLoad) > 0m) || ((LMEGenTotalOptime - LMEGenAprilOpTime) > 1m))
                                                    Category.CheckCatalogResult = "M";
                                            }
                                        }
                                    }
                                    else
                                    {
                                        LTFFRecs.RowFilter = AddToDataViewFilter(LTFFFilter, "rpt_period_id = " + RptPeriodId);
                                        if (LTFFRecs.Count > 0)
                                        {
                                            if (LMEGenTotalLoad == 0.0m && LMEGenTotalOptime == 0.0m)
                                                Category.CheckCatalogResult = "G";
                                        }
                                        else
                                        {
                                            if (LMEGenTotalLoad > 0.0m || LMEGenTotalOptime > 1m)
                                                Category.CheckCatalogResult = "I";
                                        }
                                    }

                                    LTFFRecs.RowFilter = LTFFFilter;
                                }

                                HrlyOpRecs.Sort = HrlyOpSort;
                            }
                            else
                                Category.CheckCatalogResult = "H";
                        }
                    }
                    MPQualRecords.RowFilter = sQualFilter;
                    Category.SetCheckParameter("LME_Year_Start_Quarter", nLMEYearStartQtr, eParameterDataType.Integer);
                    Category.SetCheckParameter("LME_Gen_Annual", bLMEGenAnnual, eParameterDataType.Boolean);
                    Category.SetCheckParameter("LME_Gen_OS", bLMEGenOS, eParameterDataType.Boolean);
                }

                MethodRecords.RowFilter = sMethodFilter;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "LME10");
            }
            return ReturnVal;
        }

        #endregion

        #region Checks 11 - 20

        public static string LME11(cCategory Category, ref bool Log) //Check LTFF System
        {
            string ReturnVal = "";
            try
            {
                DataRowView LTFFRecord = Category.GetCheckParameter("Current_LTFF_Record").ValueAsDataRowView();

                if (LTFFRecord["mon_sys_id"] == DBNull.Value)
                    Category.CheckCatalogResult = "A";
                else
                {
                    string sysTypeCd;
                    if (LTFFRecord.Row.Table.Columns.Contains("sys_type_cd"))
                        sysTypeCd = cDBConvert.ToString(LTFFRecord["sys_type_cd"]);
                    else
                    {
                        string MonSysID = cDBConvert.ToString(LTFFRecord["mon_sys_id"]);
                        DataView SystemRecords = Category.GetCheckParameter("Monitor_System_Records").ValueAsDataView();
                        string SysFilter = SystemRecords.RowFilter;
                        SystemRecords.RowFilter = AddToDataViewFilter(SysFilter, "mon_sys_id = '" + MonSysID + "'");
                        sysTypeCd = cDBConvert.ToString(SystemRecords[0]["sys_type_cd"]);
                        SystemRecords.RowFilter = SysFilter;
                    }
                    if (sysTypeCd != "LTOL" && sysTypeCd != "LTGS")
                        Category.CheckCatalogResult = "B";
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "LME11");
            }
            return ReturnVal;
        }

        public static string LME12(cCategory Category, ref bool Log) //Check LTFF Fuel Flow Period Code
        {
            string ReturnVal = "";
            try
            {
                DataRowView LTFFRecord = Category.GetCheckParameter("Current_LTFF_Record").ValueAsDataRowView();
                //int CurrentRptPeriod = Category.GetCheckParameter("Current_Reporting_Period").ValueAsInt();
                //DataView RptPeriods = Category.GetCheckParameter("Reporting_Period_Lookup_Table").ValueAsDataView();
                //int rptPeriodQuarter = cDBConvert.ToInteger(RptPeriods[CurrentRptPeriod - 1]["quarter"]);
                if (Category.GetCheckParameter("LME_Gen_OS").ValueAsBool())
                {
                    //if (rptPeriodQuarter == 2)
                    if (cDBConvert.ToInteger(LTFFRecord["quarter"]) == 2)
                    {
                        if (LTFFRecord["fuel_flow_period_cd"] == DBNull.Value)
                            Category.CheckCatalogResult = "A";
                    }
                    else
                    {
                        if (LTFFRecord["fuel_flow_period_cd"] != DBNull.Value)
                            Category.CheckCatalogResult = "B";
                    }
                }
                else
                {
                    if (LTFFRecord["fuel_flow_period_cd"] != DBNull.Value)
                        Category.CheckCatalogResult = "C";
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "LME12");
            }
            return ReturnVal;
        }

        public static string LME13(cCategory Category, ref bool Log) //Check Long Term Fuel Flow Value
        {
            string ReturnVal = "";
            try
            {
                DataRowView LTFFRecord = Category.GetCheckParameter("Current_LTFF_Record").ValueAsDataRowView();
                decimal LTFFVal = cDBConvert.ToDecimal(LTFFRecord["long_term_fuel_flow_value"]);
                if (LTFFVal <= 0)
                    Category.CheckCatalogResult = "A";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "LME13");
            }
            return ReturnVal;
        }

        public static string LME14(cCategory Category, ref bool Log) //Check Long Term Fuel Flow UOM
        {
            string ReturnVal = "";
            try
            {
                DataRowView LTFFRecord = Category.GetCheckParameter("Current_LTFF_Record").ValueAsDataRowView();
                //DataRowView CorrespondingMonSysRec = Category.GetCheckParameter("Current_Monitor_System_Record").ValueAsDataRowView();
                if (LTFFRecord["LTFF_UOM_CD"] == DBNull.Value)
                    Category.CheckCatalogResult = "A";
                else
                {
                    string sysTypeCd;
                    if (LTFFRecord.Row.Table.Columns.Contains("sys_type_cd"))
                        sysTypeCd = cDBConvert.ToString(LTFFRecord["sys_type_cd"]);
                    else
                    {   //screen check
                        string MonSysID = cDBConvert.ToString(LTFFRecord["mon_sys_id"]);
                        DataView SystemRecords = Category.GetCheckParameter("Monitor_System_Records").ValueAsDataView();
                        string SysFilter = SystemRecords.RowFilter;
                        SystemRecords.RowFilter = AddToDataViewFilter(SysFilter, "mon_sys_id = '" + MonSysID + "'");
                        if (SystemRecords.Count == 0)
                            sysTypeCd = "";
                        else
                            sysTypeCd = cDBConvert.ToString(SystemRecords[0]["sys_type_cd"]);
                        SystemRecords.RowFilter = SysFilter;
                    }

                    string LTFFUOMCd = cDBConvert.ToString(LTFFRecord["LTFF_UOM_CD"]);

                    if (sysTypeCd == "LTOL" && (LTFFUOMCd != "LB" && LTFFUOMCd != "GAL"))
                        Category.CheckCatalogResult = "A";
                    else if (sysTypeCd == "LTGS" && LTFFUOMCd != "SCF")
                        Category.CheckCatalogResult = "A";

                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "LME14");
            }
            return ReturnVal;
        }

        public static string LME15(cCategory Category, ref bool Log) //Check LTFF GCV
        {
            string ReturnVal = "";
            try
            {
                DataRowView LTFFRecord = Category.GetCheckParameter("Current_LTFF_Record").ValueAsDataRowView();
                decimal LTFF_GCV = cDBConvert.ToDecimal(LTFFRecord["GROSS_CALORIFIC_VALUE"]);
                if (LTFF_GCV <= 0)
                    Category.CheckCatalogResult = "A";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "LME15");
            }
            return ReturnVal;
        }

        public static string LME16(cCategory Category, ref bool Log) //Check LTFF GCV UOM
        {
            string ReturnVal = "";
            try
            {
                DataRowView LTFFRecord = Category.GetCheckParameter("Current_LTFF_Record").ValueAsDataRowView();

                Category.SetCheckParameter("LME_Gen_LTFF_Heat_Input", null, eParameterDataType.Integer);
                if (LTFFRecord["GCV_UOM_CD"] == DBNull.Value)
                {
                    Category.CheckCatalogResult = "A";
                }
                else
                {
                    string LTFFUOMCd = cDBConvert.ToString(LTFFRecord["LTFF_UOM_CD"]);
                    string GCVUOMCd = cDBConvert.ToString(LTFFRecord["GCV_UOM_CD"]);
                    string FuelCd = cDBConvert.ToString(LTFFRecord["FUEL_CD"]);

                    if (LTFFUOMCd == "LB" && GCVUOMCd != "BTULB")
                        Category.CheckCatalogResult = "A";
                    else if (LTFFUOMCd == "GAL" && GCVUOMCd != "BTUGAL")
                        Category.CheckCatalogResult = "A";
                    else if (LTFFUOMCd == "SCF" && GCVUOMCd != "BTUSCF")
                        Category.CheckCatalogResult = "A";
                    else
                    {
                        decimal gcv = cDBConvert.ToDecimal(LTFFRecord["GROSS_CALORIFIC_VALUE"]);
                        decimal LTFFVal = cDBConvert.ToDecimal(LTFFRecord["long_term_fuel_flow_value"]);

                        if (gcv > 0 && LTFFVal > 0)
                        {
                            decimal LMEGenLTFFHI = (decimal)Math.Round(gcv * LTFFVal / 1000000, 0, MidpointRounding.AwayFromZero);
                            Category.SetCheckParameter("LME_Gen_LTFF_Heat_Input", LMEGenLTFFHI, eParameterDataType.Decimal);
                        }

                        // updated specs 2012Q2
                        /*
    Max Expected GCV = Lookup "Upper Value" in "Fuel Type Warning Levels for GCV Cross Check Table" where "Fuel Code - Units Of Measure" column = concatenation of (FuelCode, " - ", LongTermFuelFlowUOMCode )
    Min Expected GCV = Lookup "Lower Value" in "Fuel Type Warning Levels for GCV Cross Check Table" where "Fuel Code - Units Of Measure" column = concatenation of (FuelCode, " - ", LongTermFuelFlowUOMCode )
    Max Allowed GCV  = Lookup "Upper Value" in "Fuel Type Reality Checks for GCV Cross Check Table" where "Fuel Code - Units Of Measure" column = concatenation of (FuelCode, " - ", LongTermFuelFlowUOMCode )
    Min Allowed GCV  = Lookup "Lower Value" in "Fuel Type Reality Checks for GCV Cross Check Table" where "Fuel Code - Units Of Measure" column = concatenation of (FuelCode, " - ", LongTermFuelFlowUOMCode )

    if (Max Allowed GCV is not null AND GrossCalorificValue > Max Allowed GCV) OR (Min Allowed GCV is not null AND GrossCalorificValue < Min Allowed GCV)
        return result B
    else
        if (Min Expected GCV is not null AND GrossCalorificValue < Min Expected GCV) OR (Max Expected GCV is not null AND GrossCalorificValue > Max Expected GCV)
            return result C                     
                         */

                        string sFuelCdUOM = string.Format("{0} - {1}", FuelCd, GCVUOMCd);

                        DataView dvReality = Category.GetCheckParameter("Fuel_Type_Reality_Checks_for_GCV_Cross_Check_Table").AsDataView();
                        DataView dvWarnings = Category.GetCheckParameter("Fuel_Type_Warning_Levels_for_GCV_Cross_Check_Table").AsDataView();

                        string sFilter = string.Format("[Fuel Code - Units of Measure]='{0}'", sFuelCdUOM);
                        dvReality.RowFilter = sFilter;
                        dvWarnings.RowFilter = sFilter;

                        decimal? maxAllowedGCV = null;
                        decimal? minAllowedGCV = null;
                        decimal? maxExpectedGCV = null;
                        decimal? minExpectedGCV = null;

                        if (dvReality.Count >= 1)
                        {
                            maxAllowedGCV = Convert.ToDecimal(dvReality[0]["Upper Value"]);
                            minAllowedGCV = Convert.ToDecimal(dvReality[0]["Lower Value"]);
                        }
                        if (dvWarnings.Count >= 1)
                        {
                            maxExpectedGCV = Convert.ToDecimal(dvWarnings[0]["Upper Value"]);
                            minExpectedGCV = Convert.ToDecimal(dvWarnings[0]["Lower Value"]);
                        }

                        if ((maxAllowedGCV.HasValue && gcv > maxAllowedGCV) || (minAllowedGCV.HasValue && gcv < minAllowedGCV))
                            Category.CheckCatalogResult = "B";
                        else if ((minExpectedGCV.HasValue && gcv < minExpectedGCV) || (maxExpectedGCV.HasValue && gcv > maxExpectedGCV))
                            Category.CheckCatalogResult = "C";
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "LME16");
            }
            return ReturnVal;
        }

        public static string LME17(cCategory Category, ref bool Log) //Check LTFF Total Heat Input
        {
            string ReturnVal = "";
            try
            {
                DataRowView LTFFRecord = Category.GetCheckParameter("Current_LTFF_Record").ValueAsDataRowView();
                DataRowView CurrentLocation = Category.GetCheckParameter("Current_Monitor_Plan_Location_Record").ValueAsDataRowView();
                int LocnPosn = Category.GetCheckParameter("Current_Monitor_Plan_Location_Postion").ValueAsInt();
                decimal LMEGenLTFFHI;
                string Location = cDBConvert.ToString(CurrentLocation["location_name"]);
                if (Category.GetCheckParameter("LME_Gen_LTFF_Heat_Input").ParameterValue == null)
                    LMEGenLTFFHI = -1;
                else
                    LMEGenLTFFHI = Category.GetCheckParameter("LME_Gen_LTFF_Heat_Input").ValueAsDecimal();

                if (LMEGenLTFFHI >= 0)
                {
                    //decimal TotalHI = cDBConvert.ToDecimal(LTFFRecord["total_heat_input"]);

                    decimal[] LcnTotalHI = Category.GetCheckParameter("LME_Gen_Total_Heat_Input_Array").ValueAsDecimalArray();
                    if (LcnTotalHI[LocnPosn] >= 0)
                    {
                        Category.AccumCheckAggregate("LME_Gen_LTFF_Heat_Input_Array", LocnPosn, LMEGenLTFFHI);
                        if (Category.GetCheckParameter("LME_Gen_OS").ValueAsBool() && cDBConvert.ToString(LTFFRecord["fuel_flow_period_cd"]) == "A")
                            Category.AccumCheckAggregate("LME_Gen_LTFF_April_Heat_Input_Array", LocnPosn, LMEGenLTFFHI);
                    }
                    if (Location.PadRight(Location.Length + 1).Substring(0, 2) == "CP")
                    {
                        if (Category.GetCheckParameter("LME_Gen_CP_Total_Heat_Input").ValueAsDecimal() >= 0)
                        {
                            Category.AccumCheckAggregate("LME_Gen_CP_Total_Heat_Input", LMEGenLTFFHI);
                            if (Category.GetCheckParameter("LME_Gen_OS").ValueAsBool() && cDBConvert.ToString(LTFFRecord["fuel_flow_period_cd"]) == "A")
                                Category.AccumCheckAggregate("LME_Gen_CP_April_Heat_Input", LMEGenLTFFHI);
                        }
                    }
                }
                else
                {
                    if (Location.PadRight(Location.Length + 1).Substring(0, 2) == "CP")
                        Category.SetCheckParameter("LME_Gen_CP_Total_Heat_Input", -1.0m, eParameterDataType.Decimal);
                    Category.SetArrayParameter("LME_Gen_LTFF_Heat_Input_Array", LocnPosn, -1.0m);
                    //if (LMEGenLTFFHI == 0)
                    //  Category.CheckCatalogResult = "A";
                }

            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "LME17");
            }
            return ReturnVal;
        }

        public static string LME18(cCategory Category, ref bool Log) //Check LME Begin Hour
        {
            string ReturnVal = "";
            try
            {
                DataRowView CurrentRecord = Category.GetCheckParameter("Current_LME_Hourly_Op_Record").ValueAsDataRowView();

                int BeginHour = cDBConvert.ToInteger(CurrentRecord["begin_hour"]);
                if (CurrentRecord["begin_hour"] == DBNull.Value || (BeginHour < 0 || BeginHour > 23))
                {
                    Category.CheckCatalogResult = "A";
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "LME18");
            }
            return ReturnVal;
        }

        public static string LME19(cCategory Category, ref bool Log) //Check LME Begin Date
        {
            string ReturnVal = "";
            try
            {
                DataRowView CurrentRecord = Category.GetCheckParameter("Current_LME_Hourly_Op_Record").ValueAsDataRowView();
                DataView ReportingPeriods = Category.GetCheckParameter("Reporting_Period_Lookup_Table").ValueAsDataView();
                int CurrentRptPeriod = Category.GetCheckParameter("Current_Reporting_Period").ValueAsInt();
                sFilterPair[] Criteria = new sFilterPair[1];
                Criteria[0].Field = "rpt_period_id";
                Criteria[0].Value = CurrentRptPeriod;
                DataRowView ReportingPeriod = FindRow(ReportingPeriods, Criteria);
                DateTime PeriodBeginDate = cDBConvert.ToDate(ReportingPeriod["begin_date"], DateTypes.START);
                DateTime PeriodEndDate = cDBConvert.ToDate(ReportingPeriod["end_date"], DateTypes.END);
                DateTime BeginDate = cDBConvert.ToDate(CurrentRecord["begin_date"], DateTypes.START);


                if (CurrentRecord["begin_date"] == DBNull.Value || !(BeginDate >= PeriodBeginDate && BeginDate <= PeriodEndDate))
                    Category.CheckCatalogResult = "A";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "LME19");
            }
            return ReturnVal;
        }

        public static string LME20(cCategory Category, ref bool Log) //Duplicate LME Hourly Op Record
        {
            string ReturnVal = "";
            try
            {
                DataRowView CurrentRecord = Category.GetCheckParameter("Current_LME_Hourly_Op_Record").ValueAsDataRowView();
                DataView OtherRecords = Category.GetCheckParameter("Hourly_Operating_Data_Records_for_LME_Configuration").ValueAsDataView();
                string sRecordsFilter = OtherRecords.RowFilter;
                OtherRecords.RowFilter = AddToDataViewFilter(sRecordsFilter, "mon_loc_id = '" + cDBConvert.ToString(CurrentRecord["mon_loc_id"]) + "'");
                string sID = "hour_id";
                sFilterPair[] Criteria = new sFilterPair[2];
                Criteria[0].Field = "begin_date";
                Criteria[1].Field = "begin_hour";
                Criteria[0].Value = cDBConvert.ToDate(CurrentRecord["begin_date"], DateTypes.START);
                Criteria[1].Value = cDBConvert.ToInteger(CurrentRecord["begin_hour"]);
                Criteria[0].DataType = eFilterDataType.DateBegan;
                Criteria[1].DataType = eFilterDataType.Integer;

                if (DuplicateRecordCheck(CurrentRecord, OtherRecords, sID, Criteria))
                    Category.CheckCatalogResult = "A";
                OtherRecords.RowFilter = sRecordsFilter;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "LME20");
            }
            return ReturnVal;
        }

        #endregion

        #region Checks 21 - 30

        public static string LME21(cCategory Category, ref bool Log)
        {
            string ReturnVal = "";
            try
            {
                Category.SetCheckParameter("Current_LME_Hourly_Op_Record", null, eParameterDataType.DataRowView);
                Category.SetCheckParameter("Generate_LME", false, eParameterDataType.DataRowView);

                DataView MethodRecords = Category.GetCheckParameter("Monitor_Method_Records_By_Hour_Location").ValueAsDataView();
                string sLMEGenParameters;

                string sMethodFilter = MethodRecords.RowFilter;
                if (Category.GetCheckParameter("LME_Gen_Annual").ValueAsBool())
                    MethodRecords.RowFilter = AddToDataViewFilter(sMethodFilter, "parameter_cd in ('SO2M','NOXM','CO2M') and method_cd  = 'LME'");
                else
                    MethodRecords.RowFilter = AddToDataViewFilter(sMethodFilter, "parameter_cd = 'NOXM' and method_cd  = 'LME'");

                if (MethodRecords.Count > 0)
                    sLMEGenParameters = ColumnToDatalist(MethodRecords, "parameter_cd", false);
                else
                    sLMEGenParameters = null;
                MethodRecords.RowFilter = sMethodFilter;

                DataView HrlyOpRecs = Category.GetCheckParameter("Hourly_Operating_Data_Records_By_Hour_Location").ValueAsDataView();

                if (HrlyOpRecs.Count > 0)
                {
                    if (sLMEGenParameters == null)
                        Category.CheckCatalogResult = "A";
                    else
                    {
                        Category.SetCheckParameter("Current_LME_Hourly_Op_Record", HrlyOpRecs[0], eParameterDataType.DataRowView);
                        Category.SetCheckParameter("Generate_LME", true, eParameterDataType.Boolean);
                        int CurrentMonth = cDBConvert.ToDate(HrlyOpRecs[0]["begin_date"], DateTypes.START).Month;
                        if (!Category.GetCheckParameter("LME_Gen_Annual").ValueAsBool() && CurrentMonth == 4)
                            Category.CheckCatalogResult = "B";
                    }
                }
                else
                {
                    if (sLMEGenParameters != null &&
                        (Category.GetCheckParameter("LME_Gen_Annual").ValueAsBool() ||
                        (Category.CurrentOpDate.Month >= 5 && Category.CurrentOpDate.Month <= 9)))
                        Category.CheckCatalogResult = "C";
                }
                Category.SetCheckParameter("LME_Gen_Parameters", sLMEGenParameters, eParameterDataType.String);

            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "LME21");
            }
            return ReturnVal;
        }

        public static string LME22(cCategory Category, ref bool Log)
        {
            string ReturnVal = "";
            try
            {
                if (Category.GetCheckParameter("Current_LME_Hourly_Op_Record").ParameterValue != null)
                {
                    int locn = Category.GetCheckParameter("Current_Monitor_Plan_Location_Postion").ValueAsInt();
                    DataRowView HrlyOpRec = Category.GetCheckParameter("Current_LME_Hourly_Op_Record").ValueAsDataRowView();
                    decimal OpTime = cDBConvert.ToDecimal(HrlyOpRec["op_time"]);
                    if (OpTime < 0 || OpTime > 1)
                    {
                        Category.SetArrayParameter("LME_Gen_Total_Op_Time_Array", locn, -1.0m);
                        Category.SetCheckParameter("Generate_LME", false, eParameterDataType.Boolean);
                        Category.CheckCatalogResult = "A";
                    }
                    else
                    {
                        decimal[] TotalOpTimeforLocns = Category.GetCheckParameter("LME_Gen_Total_Op_Time_Array").ValueAsDecimalArray();
                        if (OpTime > 0.0m && TotalOpTimeforLocns[locn] >= 0.0m)
                        {
                            Category.AccumCheckAggregate("LME_Gen_Total_Op_Hours_Array", locn, 1);
                            Category.AccumCheckAggregate("LME_Gen_Total_Op_Time_Array", locn, OpTime);
                            if (Category.CurrentOpDate.Month == 4)
                            {
                                Category.AccumCheckAggregate("LME_Gen_April_Op_Hours_Array", locn, 1);
                                Category.AccumCheckAggregate("LME_Gen_April_Op_Time_Array", locn, OpTime);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "LME22");
            }
            return ReturnVal;
        }

        public static string LME23(cCategory Category, ref bool Log) //Check LME Data Entry Screen Op Time
        {
            string ReturnVal = "";
            try
            {
                DataRowView CurrentRecord = Category.GetCheckParameter("Current_LME_Hourly_Op_Record").ValueAsDataRowView();

                decimal OpTime = cDBConvert.ToDecimal(CurrentRecord["op_time"]);
                if (CurrentRecord["op_time"] == DBNull.Value || (OpTime < 0 || OpTime > 1))
                {
                    Category.CheckCatalogResult = "A";
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "LME23");
            }
            return ReturnVal;
        }

        public static string LME24(cCategory Category, ref bool Log)
        {
            string ReturnVal = "";
            try
            {
                if (Category.GetCheckParameter("Current_LME_Hourly_Op_Record").ParameterValue != null)
                {
                    DataRowView HrlyOpRecord = Category.GetCheckParameter("Current_LME_Hourly_Op_Record").ValueAsDataRowView();
                    decimal LoadValue = cDBConvert.ToDecimal(HrlyOpRecord["hr_load"]);
                    if (HrlyOpRecord["hr_load"] == DBNull.Value)
                    {
                        decimal OpTime = cDBConvert.ToDecimal(HrlyOpRecord["op_time"]);
                        if (OpTime > 0)
                        {
                            if (Category.GetCheckParameter("LME_Gen_HI_Method").ValueAsString() == "LTTF")
                            {
                                Category.SetCheckParameter("Generate_LME", false, eParameterDataType.Boolean);
                                Category.CheckCatalogResult = "B";
                            }
                            else
                                Category.CheckCatalogResult = "C";
                        }
                    }
                    else if (LoadValue < 0)
                    {
                        Category.SetCheckParameter("Generate_LME", false, eParameterDataType.Boolean);
                        Category.CheckCatalogResult = "A";
                    }
                    else
                    {
                        if (cDBConvert.ToDecimal(HrlyOpRecord["op_time"]) == 0.0m)
                            Category.CheckCatalogResult = "D";
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "LME24");
            }
            return ReturnVal;
        }

        public static string LME25(cCategory Category, ref bool Log) //Check LME Data Entry Screen Load Value
        {
            string ReturnVal = "";
            try
            {
                DataRowView CurrentRecord = Category.GetCheckParameter("Current_LME_Hourly_Op_Record").ValueAsDataRowView();
                decimal OpTime = cDBConvert.ToDecimal(CurrentRecord["op_time"]);
                if (CurrentRecord["hr_load"] == DBNull.Value)
                {
                    if (OpTime > 0)
                        Category.CheckCatalogResult = "A";
                }
                else
                {
                    decimal LoadValue = cDBConvert.ToDecimal(CurrentRecord["hr_load"]);
                    if (LoadValue < 0)
                        Category.CheckCatalogResult = "A";
                    else
                      if (OpTime == 0)
                        Category.CheckCatalogResult = "B";
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "LME25");
            }
            return ReturnVal;
        }

        public static string LME26(cCategory Category, ref bool Log) //Check LME Load UOM
        {
            string ReturnVal = "";
            try
            {
                if (Category.GetCheckParameter("Current_LME_Hourly_Op_Record").ParameterValue != null)
                {
                    DataRowView CurrentRecord = Category.GetCheckParameter("Current_LME_Hourly_Op_Record").ValueAsDataRowView();
                    if (CurrentRecord["hr_load"] == DBNull.Value)
                    {
                        if (CurrentRecord["load_uom_cd"] != DBNull.Value)
                            Category.CheckCatalogResult = "A";
                    }
                    else
                    {
                        string loadUOMCd = cDBConvert.ToString(CurrentRecord["load_uom_cd"]);
                        if (!loadUOMCd.InList("MW,KLBHR,MMBTUHR"))
                        {
                            Category.CheckCatalogResult = "A";
                            Category.SetCheckParameter("Generate_LME", false, eParameterDataType.Boolean);
                        }
                        else
                        {
                            DataView MonitorLoadRecords = Category.GetCheckParameter("Monitor_Load_Records_by_Hour_and_Location").ValueAsDataView();

                            if (MonitorLoadRecords.Count != 1 || MonitorLoadRecords[0]["max_load_uom_cd"] == DBNull.Value)
                            {
                                Category.CheckCatalogResult = "B";
                                Category.SetCheckParameter("Generate_LME", false, eParameterDataType.Boolean);
                            }
                            else
                            {
                                if (loadUOMCd != cDBConvert.ToString(MonitorLoadRecords[0]["max_load_uom_cd"]))
                                {
                                    Category.CheckCatalogResult = "C";
                                    Category.SetCheckParameter("Generate_LME", false, eParameterDataType.Boolean);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "LME26");
            }
            return ReturnVal;
        }

        public static string LME27(cCategory Category, ref bool Log) //Check LME Fuel Code List
        {
            string ReturnVal = "";
            try
            {
                if (Category.GetCheckParameter("Current_LME_Hourly_Op_Record").ParameterValue != null)
                {
                    DataRowView CurrentRecord = Category.GetCheckParameter("Current_LME_Hourly_Op_Record").ValueAsDataRowView();
                    if (cDBConvert.ToDecimal(CurrentRecord["op_time"]) > 0 && CurrentRecord["fuel_cd_list"] == DBNull.Value)
                    {
                        Category.CheckCatalogResult = "A";
                        Category.SetCheckParameter("Generate_LME", false, eParameterDataType.Boolean);
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "LME27");
            }
            return ReturnVal;
        }

        public static string LME28(cCategory Category, ref bool Log) //Calculate Heat Input for LME Unit
        {
            string ReturnVal = "";
            try
            {
                int locn = Category.GetCheckParameter("Current_Monitor_Plan_Location_Postion").ValueAsInt();
                string LMEGenParams = Category.GetCheckParameter("LME_Gen_Parameters").ValueAsString();

                Category.SetCheckParameter("LME_Calc_Heat_Input", null, eParameterDataType.Decimal);
                Category.SetCheckParameter("LME_Gen_Heat_Input_Record", null, eParameterDataType.DataRowView);
                Category.SetCheckParameter("LME_Gen_Fuel_Code", null, eParameterDataType.String);


                if (Category.GetCheckParameter("Generate_LME").ValueAsBool())
                {
                    DataRowView HrlyOpRec = Category.GetCheckParameter("Current_LME_Hourly_Op_Record").ValueAsDataRowView();
                    if (cDBConvert.ToDecimal(HrlyOpRec["op_time"]) > 0)
                    {
                        string LMEGenHIMethod = Category.GetCheckParameter("LME_Gen_HI_Method").ValueAsString();

                        if (LMEGenHIMethod == "MHHI" || cDBConvert.ToInteger(HrlyOpRec["mhhi_indicator"]) == 1)
                        {
                            DataView DefaultRecords = Category.GetCheckParameter("Monitor_Default_Records_by_Hour_Location").ValueAsDataView();
                            string defaultFilter = DefaultRecords.RowFilter;
                            DefaultRecords.RowFilter = AddToDataViewFilter(defaultFilter, "parameter_cd = 'MHHI'");
                            if (DefaultRecords.Count == 1 && cDBConvert.ToDecimal(DefaultRecords[0]["default_value"]) > 0 &&
                                cDBConvert.ToString(DefaultRecords[0]["default_uom_cd"]) == "MMBTUHR")
                            {
                                decimal LMECalcHI = cDBConvert.ToDecimal(DefaultRecords[0]["default_value"]) *
                                    cDBConvert.ToDecimal(HrlyOpRec["op_time"]);
                                LMECalcHI = Math.Round(LMECalcHI, 1, MidpointRounding.AwayFromZero);
                                Category.SetCheckParameter("LME_Calc_Heat_Input", LMECalcHI, eParameterDataType.Decimal);
                            }
                            else
                            {
                                if (LMEGenParams.Contains("SO2M"))
                                    Category.SetArrayParameter("LME_Gen_Total_SO2M_Array", locn, -1.0m);
                                else if (LMEGenParams.Contains("NOXM"))
                                    Category.SetArrayParameter("LME_Gen_Total_NOXM_Array", locn, -1.0m);
                                else if (LMEGenParams.Contains("CO2M"))
                                    Category.SetArrayParameter("LME_Gen_Total_CO2M_Array", locn, -1.0m);
                                Category.SetArrayParameter("LME_Gen_Total_Heat_Input_Array", locn, -1.0m);
                                Category.CheckCatalogResult = "A";
                            }
                            DefaultRecords.RowFilter = defaultFilter;
                        }
                        else if (LMEGenHIMethod == "LTFF")
                        {

                            decimal LMEGenTotalLoad = Category.GetCheckParameter("LME_Gen_Total_Load").ValueAsDecimal();
                            decimal LMEGenTotalOptime = Category.GetCheckParameter("LME_Gen_Total_Optime").ValueAsDecimal();
                            decimal LMEGenCPTotalHI = Category.GetCheckParameter("LME_Gen_CP_Total_Heat_Input").ValueAsDecimal();
                            decimal[] LMEGenTotalHIArray = Category.GetCheckParameter("LME_Gen_Total_Heat_Input_Array").ValueAsDecimalArray();
                            decimal hourLoad = cDBConvert.ToDecimal(HrlyOpRec["hr_load"]);
                            decimal OpTime = cDBConvert.ToDecimal(HrlyOpRec["op_time"]);

                            if (LMEGenCPTotalHI >= 0 && LMEGenTotalHIArray[locn] >= 0 && hourLoad >= 0)
                            {
                                decimal LMECalcHI = decimal.MinValue;
                                int rptPeriod = Category.GetCheckParameter("Current_Reporting_Period").ValueAsInt();
                                DataView RptPeriodInfo = Category.GetCheckParameter("Reporting_Period_Lookup_Table").ValueAsDataView();
                                string RptPrdFilter = RptPeriodInfo.RowFilter;
                                RptPeriodInfo.RowFilter = "rpt_period_id = " + rptPeriod;

                                decimal LMEGenLocnTotalLoad = Category.GetCheckParameter("LME_Gen_Total_Load_Array").ValueAsDecimalArray()[locn];
                                decimal LMEGenLTFFHI4Locn = Category.GetCheckParameter("LME_Gen_LTFF_Heat_Input_Array").ValueAsDecimalArray()[locn];
                                decimal[] LMEGenTotalOptimeArray = Category.GetCheckParameter("LME_Gen_LTFF_Total_Op_Time_Array").ValueAsDecimalArray();

                                if (Category.GetCheckParameter("LME_Gen_OS").ValueAsBool() && cDBConvert.ToInteger(RptPeriodInfo[0]["quarter"]) == 2)
                                {
                                    decimal LMEGenCPAprilHI = Category.GetCheckParameter("LME_Gen_CP_April_Heat_Input").ValueAsDecimal();
                                    decimal LMEGenAprilLoad = Category.GetCheckParameter("LME_Gen_April_Load").ValueAsDecimal();
                                    decimal LMEGenAprilOptime = Category.GetCheckParameter("LME_Gen_April_Optime").ValueAsDecimal();
                                    decimal[] LMEGenLTFFAprilHIArray = Category.GetCheckParameter("LME_Gen_LTFF_April_Heat_Input_Array").ValueAsDecimalArray();
                                    decimal[] LMEGenAprilLoadArray = Category.GetCheckParameter("LME_Gen_April_Load_Array").ValueAsDecimalArray();
                                    decimal[] LMEGenAprilOptimeArray = Category.GetCheckParameter("LME_Gen_LTFF_April_Op_Time_Array").ValueAsDecimalArray();

                                    if (Category.CurrentOpDate.Month == 4)
                                    {
                                        if (LMEGenAprilLoad > 0)
                                        {
                                            if (hourLoad == 0)
                                                LMECalcHI = 0m;
                                            else
                                                LMECalcHI = Math.Round((LMEGenCPAprilHI * hourLoad * OpTime / LMEGenAprilLoad) +
                                                  (LMEGenLTFFAprilHIArray[locn] * hourLoad * OpTime / LMEGenAprilLoadArray[locn]), 1, MidpointRounding.AwayFromZero);
                                            Category.SetCheckParameter("LME_Calc_Heat_Input", LMECalcHI, eParameterDataType.Decimal);
                                        }
                                        else if (LMEGenAprilOptime > 0)
                                        {
                                            LMECalcHI = Math.Round((LMEGenCPAprilHI * OpTime / LMEGenAprilOptime) +
                                                 (LMEGenLTFFAprilHIArray[locn] * OpTime / LMEGenAprilOptimeArray[locn]), 1, MidpointRounding.AwayFromZero);
                                            Category.SetCheckParameter("LME_Calc_Heat_Input", LMECalcHI, eParameterDataType.Decimal);
                                        }
                                    }
                                    else
                                    {
                                        if (LMEGenTotalLoad > 0)
                                        {
                                            if (hourLoad == 0)
                                                LMECalcHI = 0m;
                                            else
                                                LMECalcHI = Math.Round(((LMEGenCPTotalHI - LMEGenCPAprilHI) * hourLoad * OpTime / (LMEGenTotalLoad - LMEGenAprilLoad)) +
                                                  ((LMEGenLTFFHI4Locn - LMEGenLTFFAprilHIArray[locn]) * hourLoad * OpTime / (LMEGenLocnTotalLoad - LMEGenAprilLoadArray[locn])), 1, MidpointRounding.AwayFromZero);
                                            Category.SetCheckParameter("LME_Calc_Heat_Input", LMECalcHI, eParameterDataType.Decimal);
                                        }
                                        else if (LMEGenTotalOptime > 0)
                                        {
                                            LMECalcHI = Math.Round(((LMEGenCPTotalHI - LMEGenCPAprilHI) * OpTime / (LMEGenTotalOptime - LMEGenAprilOptime)) +
                                                ((LMEGenLTFFHI4Locn - LMEGenLTFFAprilHIArray[locn]) * OpTime / (LMEGenTotalOptimeArray[locn] - LMEGenAprilOptimeArray[locn])), 1, MidpointRounding.AwayFromZero);
                                            Category.SetCheckParameter("LME_Calc_Heat_Input", LMECalcHI, eParameterDataType.Decimal);
                                        }
                                    }
                                }
                                else
                                {
                                    if (LMEGenTotalLoad > 0)
                                    {
                                        if (hourLoad == 0)
                                            LMECalcHI = 0m;
                                        else
                                            LMECalcHI = Math.Round((LMEGenCPTotalHI * hourLoad * OpTime / LMEGenTotalLoad) +
                                              (LMEGenLTFFHI4Locn * hourLoad * OpTime / LMEGenLocnTotalLoad), 1, MidpointRounding.AwayFromZero);
                                        Category.SetCheckParameter("LME_Calc_Heat_Input", LMECalcHI, eParameterDataType.Decimal);
                                    }
                                    else if (LMEGenTotalOptime > 0)
                                    {
                                        LMECalcHI = Math.Round((LMEGenCPTotalHI * OpTime / LMEGenTotalOptime) +
                                            (LMEGenLTFFHI4Locn * OpTime / LMEGenTotalOptimeArray[locn]), 1, MidpointRounding.AwayFromZero);
                                        Category.SetCheckParameter("LME_Calc_Heat_Input", LMECalcHI, eParameterDataType.Decimal);
                                    }
                                }
                                RptPeriodInfo.RowFilter = RptPrdFilter;
                            }
                        }
                        decimal LMECalcHeatInput = Category.GetCheckParameter("LME_Calc_Heat_Input").ValueAsDecimal();
                        if (LMECalcHeatInput != decimal.MinValue)
                        {
                            if (LMECalcHeatInput > 999999.9m)
                            {
                                if (LMEGenParams.Contains("SO2M"))
                                    Category.SetArrayParameter("LME_Gen_Total_SO2M_Array", locn, -1.0m);
                                else if (LMEGenParams.Contains("NOXM"))
                                    Category.SetArrayParameter("LME_Gen_Total_NOXM_Array", locn, -1.0m);
                                else if (LMEGenParams.Contains("CO2M"))
                                    Category.SetArrayParameter("LME_Gen_Total_CO2M_Array", locn, -1.0m);
                                Category.SetCheckParameter("LME_Calc_Heat_Input", null, eParameterDataType.Decimal);
                                Category.SetArrayParameter("LME_Gen_Total_Heat_Input_Array", locn, -1.0m);
                                Category.CheckCatalogResult = "B";
                            }
                            else if (LMECalcHeatInput >= 0)
                            {
                                DataTable LMEGenHITable = new DataTable("LMEGenHI");
                                LMEGenHITable.Columns.Add("hour_id");
                                LMEGenHITable.Columns.Add("parameter_cd");
                                LMEGenHITable.Columns.Add("adjusted_hrly_value");
                                LMEGenHITable.Columns.Add("modc_cd");
                                DataRow LMEGenHIRecord = LMEGenHITable.NewRow();
                                LMEGenHIRecord["hour_id"] = HrlyOpRec["hour_id"];
                                LMEGenHIRecord["parameter_cd"] = "HIT";
                                LMEGenHIRecord["adjusted_hrly_value"] = LMECalcHeatInput;
                                if (cDBConvert.ToInteger(HrlyOpRec["mhhi_indicator"]) == 1)
                                    LMEGenHIRecord["modc_cd"] = 45;
                                //if (LMECalcHeatInput < 1)
                                //{
                                //    LMECalcHeatInput = 1;
                                //    Category.SetCheckParameter("LME_Calc_Heat_Input", LMECalcHeatInput, eParameterDataType.Decimal);
                                //    LMEGenHIRecord["modc_cd"] = 26;
                                //}
                                if (Category.GetCheckParameter("LME_Gen_Total_Heat_Input_Array").ValueAsDecimalArray()[locn] >= 0)
                                {
                                    Category.AccumCheckAggregate("LME_Gen_Total_Heat_Input_Array", locn, LMECalcHeatInput);
                                    if (Category.CurrentOpDate.Month == 4)
                                        Category.AccumCheckAggregate("LME_Gen_April_Heat_Input_Array", locn, LMECalcHeatInput);
                                }
                                LMEGenHITable.Rows.Add(LMEGenHIRecord);
                                Category.SetCheckParameter("LME_Gen_Heat_Input_Record", LMEGenHITable.DefaultView[0], eParameterDataType.DataRowView);
                            }
                        }
                    }
                }
                else
                {
                    if (LMEGenParams != "" &&
                        (Category.GetCheckParameter("LME_Gen_Annual").ValueAsBool() ||
                            (Category.CurrentOpDate.Month >= 5 && Category.CurrentOpDate.Month <= 9)))
                    {
                        if (LMEGenParams.Contains("SO2M"))
                            Category.SetArrayParameter("LME_Gen_Total_SO2M_Array", locn, -1.0m);
                        else if (LMEGenParams.Contains("NOXM"))
                            Category.SetArrayParameter("LME_Gen_Total_NOXM_Array", locn, -1.0m);
                        else if (LMEGenParams.Contains("CO2M"))
                            Category.SetArrayParameter("LME_Gen_Total_CO2M_Array", locn, -1.0m);
                        Category.SetArrayParameter("LME_Gen_Total_Heat_Input_Array", locn, -1.0m);
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "LME28");
            }
            return ReturnVal;
        }

        public static string LME29(cCategory Category, ref bool Log) //Calculate SO2 Mass for LME Unit
        {
            string ReturnVal = "";
            try
            {
                Category.SetCheckParameter("LME_Gen_SO2M_Record", null, eParameterDataType.DataRowView);
                if (Category.GetCheckParameter("LME_Gen_Parameters").ValueAsString().Contains("SO2M") &&
                    Category.GetCheckParameter("Current_LME_Hourly_Op_Record").ParameterValue != null)
                {
                    int locn = Category.GetCheckParameter("Current_Monitor_Plan_Location_Postion").ValueAsInt();
                    decimal SO2Rate = decimal.MinValue;
                    string SO2Fuel = "";
                    DataRowView HrlyOpRecord = Category.GetCheckParameter("Current_LME_Hourly_Op_Record").ValueAsDataRowView();
                    if (HrlyOpRecord["fuel_cd_list"] != DBNull.Value)
                    {
                        SO2Rate = 0;
                        SO2Fuel = null;

                        string FuelCdList = cDBConvert.ToString(HrlyOpRecord["fuel_cd_list"]);
                        string[] FuelCds = FuelCdList.Split(';');

                        DataView DefaultRecords = Category.GetCheckParameter("Monitor_Default_Records_by_Hour_Location").ValueAsDataView();
                        string defaultFilter = DefaultRecords.RowFilter;

                        foreach (string FuelCd in FuelCds)
                        {
                            DefaultRecords.RowFilter = AddToDataViewFilter(defaultFilter, "parameter_cd = 'SO2R' and default_purpose_cd = 'LM' and fuel_cd = '" + FuelCd + "'");
                            if (DefaultRecords.Count == 1 && cDBConvert.ToDecimal(DefaultRecords[0]["default_value"]) > 0
                                && cDBConvert.ToString(DefaultRecords[0]["default_uom_cd"]) == "LBMMBTU")
                            {
                                decimal defaultVal = cDBConvert.ToDecimal(DefaultRecords[0]["default_value"]);
                                if (SO2Rate < defaultVal)
                                {
                                    SO2Rate = defaultVal;
                                    SO2Fuel = FuelCd;
                                }
                            }
                            else
                            {
                                Category.SetArrayParameter("LME_Gen_Total_SO2M_Array", locn, -1.0m);
                                Category.SetCheckParameter("LME_Gen_Fuel_Code", FuelCd, eParameterDataType.String);
                                Category.CheckCatalogResult = "A";
                                DefaultRecords.RowFilter = defaultFilter;
                                break;
                            }
                            DefaultRecords.RowFilter = defaultFilter;
                        }
                    }
                    decimal LMECalcHI = Category.GetCheckParameter("LME_Calc_Heat_Input").ValueAsDecimal();
                    if (LMECalcHI >= 0 && SO2Rate > 0 && string.IsNullOrEmpty(Category.CheckCatalogResult))
                    {
                        decimal SO2Mass = Math.Round(LMECalcHI * SO2Rate, 1, MidpointRounding.AwayFromZero);
                        if (SO2Mass > 99999.9m)
                        {
                            Category.SetArrayParameter("LME_Gen_Total_SO2M_Array", locn, -1.0m);
                            Category.CheckCatalogResult = "B";
                        }
                        else
                        {
                            DataTable SO2MTable = new DataTable("LMEGenSO2M");
                            SO2MTable.Columns.Add("hour_id");
                            SO2MTable.Columns.Add("parameter_cd");
                            SO2MTable.Columns.Add("adjusted_hrly_value");
                            SO2MTable.Columns.Add("fuel_cd");

                            DataRow LMEGenSO2M = SO2MTable.NewRow();
                            LMEGenSO2M["hour_id"] = HrlyOpRecord["hour_id"];
                            LMEGenSO2M["parameter_cd"] = "SO2M";
                            LMEGenSO2M["adjusted_hrly_value"] = SO2Mass;
                            LMEGenSO2M["fuel_cd"] = SO2Fuel;
                            SO2MTable.Rows.Add(LMEGenSO2M);
                            Category.SetCheckParameter("LME_Gen_SO2M_Record", SO2MTable.DefaultView[0], eParameterDataType.DataRowView);

                            if (Category.GetCheckParameter("LME_Gen_Total_SO2M_Array").ValueAsDecimalArray()[locn] >= 0)
                                Category.AccumCheckAggregate("LME_Gen_Total_SO2M_Array", locn, SO2Mass);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "LME29");
            }
            return ReturnVal;
        }

        public static string LME30(cCategory Category, ref bool Log) //Calculate NOX Mass for LME Unit
        {
            string ReturnVal = "";
            try
            {
                Category.SetCheckParameter("LME_Gen_NOXM_Record", null, eParameterDataType.DataRowView);

                DataRowView CurrentLmeHourlyOpRecord = Category.GetCheckParameter("Current_LME_Hourly_Op_Record").ValueAsDataRowView();

                if (Category.GetCheckParameter("LME_Gen_Parameters").ValueAsString().Contains("NOXM") &&
                    (CurrentLmeHourlyOpRecord != null) &&
                    (CurrentLmeHourlyOpRecord["OP_TIME"].AsDecimal(0) > 0))
                {
                    int locn = Category.GetCheckParameter("Current_Monitor_Plan_Location_Postion").ValueAsInt();
                    decimal NOXRate = decimal.MinValue;
                    string NOXFuel = "";

                    if (CurrentLmeHourlyOpRecord["fuel_cd_list"] != DBNull.Value)
                    {
                        NOXRate = 0;
                        NOXFuel = null;

                        string FuelCdList = cDBConvert.ToString(CurrentLmeHourlyOpRecord["fuel_cd_list"]);
                        string[] FuelCds = FuelCdList.Split(';');

                        DataView DefaultRecords = Category.GetCheckParameter("Monitor_Default_Records_by_Hour_Location").ValueAsDataView();
                        string defaultFilter = DefaultRecords.RowFilter;

                        string OpCondnCd = cDBConvert.ToString(CurrentLmeHourlyOpRecord["operating_condition_cd"]);

                        foreach (string FuelCd in FuelCds)
                        {
                            if (CurrentLmeHourlyOpRecord["operating_condition_cd"] == DBNull.Value)
                                DefaultRecords.RowFilter = AddToDataViewFilter(defaultFilter, "parameter_cd = 'NOXR' and default_purpose_cd = 'LM' and operating_condition_cd = 'A' and fuel_cd = '" + FuelCd + "'");
                            else if (OpCondnCd == "U")
                                DefaultRecords.RowFilter = AddToDataViewFilter(defaultFilter, "parameter_cd = 'NORX' and default_purpose_cd = 'MD' and operating_condition_cd = 'U' and fuel_cd = '" + FuelCd + "'");
                            else
                                DefaultRecords.RowFilter = AddToDataViewFilter(defaultFilter, "parameter_cd = 'NOXR' and default_purpose_cd = 'LM' and operating_condition_cd = '" + OpCondnCd + "' and fuel_cd = '" + FuelCd + "'");

                            if (DefaultRecords.Count == 1 && cDBConvert.ToDecimal(DefaultRecords[0]["default_value"]) > 0
                                && cDBConvert.ToString(DefaultRecords[0]["default_uom_cd"]) == "LBMMBTU")
                            {
                                decimal defaultVal = cDBConvert.ToDecimal(DefaultRecords[0]["default_value"]);
                                if (NOXRate < defaultVal)
                                {
                                    NOXRate = defaultVal;
                                    NOXFuel = FuelCd;
                                }
                            }
                            else
                            {
                                Category.SetArrayParameter("LME_Gen_Total_NOXM_Array", locn, -1.0m);
                                Category.SetCheckParameter("LME_Gen_Fuel_Code", FuelCd, eParameterDataType.String);
                                if (CurrentLmeHourlyOpRecord["operating_condition_cd"] == DBNull.Value)
                                    Category.CheckCatalogResult = "A";
                                else
                                    Category.CheckCatalogResult = "B";
                                DefaultRecords.RowFilter = defaultFilter;
                                break;
                            }
                            DefaultRecords.RowFilter = defaultFilter;
                        }
                    }
                    decimal LMECalcHI = Category.GetCheckParameter("LME_Calc_Heat_Input").ValueAsDecimal();
                    if (LMECalcHI >= 0 && NOXRate > 0 && string.IsNullOrEmpty(Category.CheckCatalogResult))
                    {
                        decimal NOXMass = Math.Round(LMECalcHI * NOXRate, 1, MidpointRounding.AwayFromZero);
                        if (NOXMass > 99999.9m)
                        {
                            Category.SetArrayParameter("LME_Gen_Total_NOXM_Array", locn, -1.0m);
                            Category.CheckCatalogResult = "C";
                        }
                        else
                        {
                            DataTable NOXMTable = new DataTable("LMEGenNOXM");
                            NOXMTable.Columns.Add("hour_id");
                            NOXMTable.Columns.Add("parameter_cd");
                            NOXMTable.Columns.Add("adjusted_hrly_value");
                            NOXMTable.Columns.Add("fuel_cd");
                            NOXMTable.Columns.Add("operating_condition_cd");

                            DataRow LMEGenNOXM = NOXMTable.NewRow();
                            LMEGenNOXM["hour_id"] = CurrentLmeHourlyOpRecord["hour_id"];
                            LMEGenNOXM["parameter_cd"] = "NOXM";
                            LMEGenNOXM["adjusted_hrly_value"] = NOXMass;
                            LMEGenNOXM["fuel_cd"] = NOXFuel;
                            LMEGenNOXM["operating_condition_cd"] = CurrentLmeHourlyOpRecord["operating_condition_cd"];
                            NOXMTable.Rows.Add(LMEGenNOXM);
                            Category.SetCheckParameter("LME_Gen_NOXM_Record", NOXMTable.DefaultView[0], eParameterDataType.DataRowView);

                            if (Category.GetCheckParameter("LME_Gen_Total_NOXM_Array").ValueAsDecimalArray()[locn] >= 0)
                            {
                                Category.AccumCheckAggregate("LME_Gen_Total_NOXM_Array", locn, NOXMass);
                                if (Category.CurrentOpDate.Month == 4)
                                    Category.AccumCheckAggregate("LME_Gen_April_NOXM_Array", locn, NOXMass);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "LME30");
            }
            return ReturnVal;
        }

        #endregion

        #region Checks 31 - 40

        public static string LME31(cCategory Category, ref bool Log) //Calculate CO2 Mass for LME Unit
        {
            string ReturnVal = "";
            try
            {

                Category.SetCheckParameter("LME_Gen_CO2M_Record", null, eParameterDataType.DataRowView);
                if (Category.GetCheckParameter("LME_Gen_Parameters").ValueAsString().Contains("CO2M") &&
                    Category.GetCheckParameter("Current_LME_Hourly_Op_Record").ParameterValue != null)
                {
                    int locn = Category.GetCheckParameter("Current_Monitor_Plan_Location_Postion").ValueAsInt();
                    decimal CO2Rate = decimal.MinValue;
                    string CO2Fuel = "";
                    DataRowView HrlyOpRecord = Category.GetCheckParameter("Current_LME_Hourly_Op_Record").ValueAsDataRowView();
                    if (HrlyOpRecord["fuel_cd_list"] != DBNull.Value)
                    {
                        CO2Rate = 0;
                        CO2Fuel = null;

                        string FuelCdList = cDBConvert.ToString(HrlyOpRecord["fuel_cd_list"]);
                        string[] FuelCds = FuelCdList.Split(';');

                        DataView DefaultRecords = Category.GetCheckParameter("Monitor_Default_Records_by_Hour_Location").ValueAsDataView();
                        string defaultFilter = DefaultRecords.RowFilter;

                        foreach (string FuelCd in FuelCds)
                        {
                            DefaultRecords.RowFilter = AddToDataViewFilter(defaultFilter, "parameter_cd = 'CO2R' and default_purpose_cd = 'LM' and fuel_cd = '" + FuelCd + "'");
                            if (DefaultRecords.Count == 1 && cDBConvert.ToDecimal(DefaultRecords[0]["default_value"]) > 0
                                && cDBConvert.ToString(DefaultRecords[0]["default_uom_cd"]) == "TNMMBTU")
                            {
                                decimal defaultVal = cDBConvert.ToDecimal(DefaultRecords[0]["default_value"]);
                                if (CO2Rate < defaultVal)
                                {
                                    CO2Rate = defaultVal;
                                    CO2Fuel = FuelCd;
                                }
                            }
                            else
                            {
                                Category.SetArrayParameter("LME_Gen_Total_CO2M_Array", locn, -1.0m);
                                Category.SetCheckParameter("LME_Gen_Fuel_Code", FuelCd, eParameterDataType.String);
                                Category.CheckCatalogResult = "A";
                                DefaultRecords.RowFilter = defaultFilter;
                                break;
                            }
                            DefaultRecords.RowFilter = defaultFilter;
                        }
                    }
                    decimal LMECalcHI = Category.GetCheckParameter("LME_Calc_Heat_Input").ValueAsDecimal();
                    if (LMECalcHI >= 0 && CO2Rate > 0 && string.IsNullOrEmpty(Category.CheckCatalogResult))
                    {
                        decimal CO2Mass = Math.Round(LMECalcHI * CO2Rate, 1, MidpointRounding.AwayFromZero);
                        if (CO2Mass > 99999999.9m)
                        {
                            Category.SetArrayParameter("LME_Gen_Total_CO2M_Array", locn, -1.0m);
                            Category.CheckCatalogResult = "B";
                        }
                        else
                        {
                            DataTable CO2MTable = new DataTable("LMEGenCO2M");
                            CO2MTable.Columns.Add("hour_id");
                            CO2MTable.Columns.Add("parameter_cd");
                            CO2MTable.Columns.Add("adjusted_hrly_value");
                            CO2MTable.Columns.Add("fuel_cd");
                            DataRow LMEGenCO2M = CO2MTable.NewRow();
                            LMEGenCO2M["hour_id"] = HrlyOpRecord["hour_id"];
                            LMEGenCO2M["parameter_cd"] = "CO2M";
                            LMEGenCO2M["adjusted_hrly_value"] = CO2Mass;
                            LMEGenCO2M["fuel_cd"] = CO2Fuel;
                            CO2MTable.Rows.Add(LMEGenCO2M);
                            Category.SetCheckParameter("LME_Gen_CO2M_Record", CO2MTable.DefaultView[0], eParameterDataType.DataRowView);

                            if (Category.GetCheckParameter("LME_Gen_Total_CO2M_Array").ValueAsDecimalArray()[locn] >= 0)
                                Category.AccumCheckAggregate("LME_Gen_Total_CO2M_Array", locn, CO2Mass);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "LME31");
            }
            return ReturnVal;
        }

        public static string LME32(cCategory Category, ref bool Log)//Calculate HIT Summary Values
        {
            string ReturnVal = "";
            try
            {
                Category.SetCheckParameter("LME_Summary_Heat_Input_Record", null, eParameterDataType.DataRowView);
                DataRowView CurrentLocation = Category.GetCheckParameter("Current_Monitor_Plan_Location_Record").ValueAsDataRowView();
                string Location = cDBConvert.ToString(CurrentLocation["location_name"]);
                int theLocation = Category.GetCheckParameter("Current_Monitor_Plan_Location_Postion").ValueAsInt();
                decimal LMEGenTotalHIArrayLocn, LMEGenAprilHILocn;
                if (Location.PadRight(Location.Length + 1).Substring(0, 2) == "CP")
                {
                    LMEGenTotalHIArrayLocn = Category.GetCheckParameter("LME_Gen_LTFF_Heat_Input_Array").ValueAsDecimalArray()[theLocation];
                    LMEGenAprilHILocn = Category.GetCheckParameter("LME_Gen_LTFF_April_Heat_Input_Array").ValueAsDecimalArray()[theLocation];
                }
                else
                {
                    LMEGenTotalHIArrayLocn = Category.GetCheckParameter("LME_Gen_Total_Heat_Input_Array").ValueAsDecimalArray()[theLocation];
                    LMEGenAprilHILocn = Category.GetCheckParameter("LME_Gen_April_Heat_Input_Array").ValueAsDecimalArray()[theLocation];
                }

                if (LMEGenTotalHIArrayLocn >= 0)
                {
                    int CurrentRptPeriod = Category.GetCheckParameter("Current_Reporting_Period").ValueAsInt();
                    DataTable LMESummaryHITable = new DataTable("LMESummaryHITable");
                    LMESummaryHITable.Columns.Add("mon_loc_id");
                    LMESummaryHITable.Columns.Add("rpt_period_id");
                    LMESummaryHITable.Columns.Add("parameter_cd");
                    LMESummaryHITable.Columns.Add("year_total");
                    LMESummaryHITable.Columns.Add("current_rpt_period_total");
                    LMESummaryHITable.Columns.Add("os_total");

                    DataRow LMESummaryHIRecord = LMESummaryHITable.NewRow();

                    LMESummaryHIRecord["mon_loc_id"] = Category.CurrentMonLocId;
                    LMESummaryHIRecord["rpt_period_id"] = CurrentRptPeriod;
                    LMESummaryHIRecord["parameter_cd"] = "HIT";

                    DataView RptPeriodTable = Category.GetCheckParameter("Reporting_Period_Lookup_Table").ValueAsDataView();
                    string RptPrdFilter = RptPeriodTable.RowFilter;
                    RptPeriodTable.RowFilter = AddToDataViewFilter(RptPrdFilter, "rpt_period_id = " + CurrentRptPeriod);
                    int rptPeriodQtr = cDBConvert.ToInteger(RptPeriodTable[0]["quarter"]);
                    int rptPeriodYr = cDBConvert.ToInteger(RptPeriodTable[0]["calendar_year"]);

                    bool bLMEGenOS = Category.GetCheckParameter("LME_Gen_OS").ValueAsBool();
                    bool bLMEGenAnnual = Category.GetCheckParameter("LME_Gen_Annual").ValueAsBool();
                    int LMEYrStartQtr = Category.GetCheckParameter("LME_Year_Start_Quarter").ValueAsInt();

                    if (bLMEGenOS && !bLMEGenAnnual && rptPeriodQtr == 2)
                        LMESummaryHIRecord["current_rpt_period_total"] = Math.Round(LMEGenTotalHIArrayLocn - LMEGenAprilHILocn, 0, MidpointRounding.AwayFromZero);
                    else
                        LMESummaryHIRecord["current_rpt_period_total"] = Math.Round(LMEGenTotalHIArrayLocn, 0, MidpointRounding.AwayFromZero);

                    DataView OpSuppData = Category.GetCheckParameter("Operating_Supp_Data_Records_by_Location").ValueAsDataView();
                    string OpSuppFilter = OpSuppData.RowFilter;

                    if (bLMEGenOS)
                    {
                        if (rptPeriodQtr == 2)
                        {
                            LMESummaryHIRecord["os_total"] = Math.Round(LMEGenTotalHIArrayLocn - LMEGenAprilHILocn, 0, MidpointRounding.AwayFromZero);
                        }
                        else if (rptPeriodQtr == 3)
                        {
                            LMESummaryHIRecord["os_total"] = Math.Round(LMEGenTotalHIArrayLocn, 0, MidpointRounding.AwayFromZero);
                        }
                        else if (rptPeriodQtr == 4 && LMEYrStartQtr < 4)
                        {
                            RptPeriodTable.RowFilter = AddToDataViewFilter(RptPrdFilter, "calendar_year = " + rptPeriodYr + " and quarter = 3");
                            int opSuppRptPrd = cDBConvert.ToInteger(RptPeriodTable[0]["rpt_period_id"]);

                            OpSuppData.RowFilter = AddToDataViewFilter(OpSuppFilter, "rpt_period_id = " + opSuppRptPrd + "and op_type_cd = 'HIT'");
                            if (OpSuppData.Count > 0)
                            {
                                if (OpSuppData[0]["op_value"] != DBNull.Value)
                                    LMESummaryHIRecord["os_total"] = cDBConvert.ToDecimal(OpSuppData[0]["op_value"]);
                            }
                        }

                        if ((rptPeriodQtr == 3 || rptPeriodQtr == 4) && LMEYrStartQtr < 3)
                        {
                            RptPeriodTable.RowFilter = AddToDataViewFilter(RptPrdFilter, "calendar_year = " + rptPeriodYr + " and quarter = 2");
                            int opSuppRptPrd = cDBConvert.ToInteger(RptPeriodTable[0]["rpt_period_id"]);

                            OpSuppData.RowFilter = AddToDataViewFilter(OpSuppFilter, "rpt_period_id = " + opSuppRptPrd + "and op_type_cd = 'HITOS'");
                            if (OpSuppData.Count > 0)
                            {
                                if (OpSuppData[0]["op_value"] != DBNull.Value)
                                    LMESummaryHIRecord["os_total"] = cDBConvert.ToDecimal(LMESummaryHIRecord["os_total"]) +
                                                                    cDBConvert.ToDecimal(OpSuppData[0]["op_value"]);
                            }
                        }
                    }

                    if (bLMEGenAnnual && string.IsNullOrEmpty(Category.CheckCatalogResult))
                    {
                        LMESummaryHIRecord["year_total"]
                          = Math.Round(cDBConvert.ToDecimal(LMESummaryHIRecord["current_rpt_period_total"]),
                                       0, MidpointRounding.AwayFromZero);

                        if (rptPeriodQtr > LMEYrStartQtr)
                        {
                            RptPeriodTable.RowFilter = AddToDataViewFilter(RptPrdFilter, "calendar_year = " + rptPeriodYr);
                            //RptPeriodTable.Sort = "rpt_period_id";
                            int RptPrdOfYear = cDBConvert.ToInteger(RptPeriodTable[0]["rpt_period_id"]);
                            int quarter = 1;
                            while (RptPrdOfYear < CurrentRptPeriod)
                            {
                                if (quarter >= LMEYrStartQtr)
                                {
                                    OpSuppData.RowFilter = AddToDataViewFilter(OpSuppFilter, "rpt_period_id = " + RptPrdOfYear + "and op_type_cd = 'HIT'");
                                    if (OpSuppData.Count > 0)
                                    {
                                        if (OpSuppData[0]["op_value"] != DBNull.Value)
                                            LMESummaryHIRecord["year_total"] = cDBConvert.ToDecimal(LMESummaryHIRecord["year_total"]) +
                                                                            cDBConvert.ToDecimal(OpSuppData[0]["op_value"]);
                                    }

                                }
                                quarter++;
                                RptPrdOfYear++;
                            }
                        }
                    }
                    OpSuppData.RowFilter = OpSuppFilter;
                    RptPeriodTable.RowFilter = RptPrdFilter;
                    LMESummaryHITable.Rows.Add(LMESummaryHIRecord);
                    Category.SetCheckParameter("LME_Summary_Heat_Input_Record", LMESummaryHITable.DefaultView[0], eParameterDataType.DataRowView);

                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "LME32");
            }
            return ReturnVal;
        }

        public static string LME33(cCategory Category, ref bool Log) //Calculate OPTIME Summary Values
        {
            string ReturnVal = "";
            try
            {
                Category.SetCheckParameter("LME_Summary_Op_Time_Record", null, eParameterDataType.DataRowView);
                int theLocation = Category.GetCheckParameter("Current_Monitor_Plan_Location_Postion").ValueAsInt();
                DataRowView CurrentMPLocnRecord = Category.GetCheckParameter("Current_Monitor_Plan_Location_Record").ValueAsDataRowView();

                decimal LMEGenTotalOpTimeArrayLocn = Category.GetCheckParameter("LME_Gen_Total_Op_Time_Array").ValueAsDecimalArray()[theLocation];

                if (LMEGenTotalOpTimeArrayLocn >= 0 && CurrentMPLocnRecord["unit_id"] != DBNull.Value)
                {
                    int CurrentRptPeriod = Category.GetCheckParameter("Current_Reporting_Period").ValueAsInt();
                    DataTable LMESummaryOpTimeTable = new DataTable("LMESummaryOpTimeTable");
                    LMESummaryOpTimeTable.Columns.Add("mon_loc_id");
                    LMESummaryOpTimeTable.Columns.Add("rpt_period_id");
                    LMESummaryOpTimeTable.Columns.Add("parameter_cd");
                    LMESummaryOpTimeTable.Columns.Add("current_rpt_period_total");
                    LMESummaryOpTimeTable.Columns.Add("os_total");
                    LMESummaryOpTimeTable.Columns.Add("year_total");

                    DataRow LMESummaryOPTimeRecord = LMESummaryOpTimeTable.NewRow();
                    LMESummaryOPTimeRecord["mon_loc_id"] = cDBConvert.ToString(CurrentMPLocnRecord["mon_loc_id"]);
                    LMESummaryOPTimeRecord["rpt_period_id"] = CurrentRptPeriod;
                    LMESummaryOPTimeRecord["parameter_cd"] = "OPTIME";

                    DataView RptPeriodTable = Category.GetCheckParameter("Reporting_Period_Lookup_Table").ValueAsDataView();
                    string RptPrdFilter = RptPeriodTable.RowFilter;
                    RptPeriodTable.RowFilter = AddToDataViewFilter(RptPrdFilter, "rpt_period_id = " + CurrentRptPeriod);
                    int rptPeriodQtr = cDBConvert.ToInteger(RptPeriodTable[0]["quarter"]);
                    int rptPeriodYr = cDBConvert.ToInteger(RptPeriodTable[0]["calendar_year"]);

                    decimal LMEGenAprilOPTimeLocn = Category.GetCheckParameter("LME_Gen_April_Op_Time_Array").ValueAsDecimalArray()[theLocation];
                    bool bLMEGenOS = Category.GetCheckParameter("LME_Gen_OS").ValueAsBool();
                    bool bLMEGenAnnual = Category.GetCheckParameter("LME_Gen_Annual").ValueAsBool();
                    int LMEYrStartQtr = Category.GetCheckParameter("LME_Year_Start_Quarter").ValueAsInt();

                    if (bLMEGenOS && !bLMEGenAnnual && rptPeriodQtr == 2)
                        LMESummaryOPTimeRecord["current_rpt_period_total"] = LMEGenTotalOpTimeArrayLocn - LMEGenAprilOPTimeLocn;
                    else
                        LMESummaryOPTimeRecord["current_rpt_period_total"] = LMEGenTotalOpTimeArrayLocn;

                    cReportingPeriod ecmpsQuarter = Category.GetCheckParameter("First_ECMPS_Reporting_Period_Object").ParameterValue.AsReportingPeriod();
                    DataView OpSuppData = Category.GetCheckParameter("Operating_Supp_Data_Records_by_Location").ValueAsDataView();

                    string OpSuppFilter = OpSuppData.RowFilter;

                    if (bLMEGenOS)
                    {
                        if (rptPeriodQtr == 2)
                        {
                            LMESummaryOPTimeRecord["os_total"] = LMEGenTotalOpTimeArrayLocn - LMEGenAprilOPTimeLocn;
                        }
                        else if (rptPeriodQtr == 3)
                        {
                            LMESummaryOPTimeRecord["os_total"] = LMEGenTotalOpTimeArrayLocn;
                        }
                        else if (rptPeriodQtr == 4 && LMEYrStartQtr < 4)
                        {
                            RptPeriodTable.RowFilter = AddToDataViewFilter(RptPrdFilter, "calendar_year = " + rptPeriodYr + " and quarter = 3");
                            int opSuppRptPrd = cDBConvert.ToInteger(RptPeriodTable[0]["rpt_period_id"]);

                            OpSuppData.RowFilter = AddToDataViewFilter(OpSuppFilter, "rpt_period_id = " + opSuppRptPrd + "and op_type_cd = 'OPTIME'");
                            if (OpSuppData.Count > 0)
                            {
                                if (OpSuppData[0]["op_value"] != DBNull.Value)
                                    LMESummaryOPTimeRecord["os_total"] = cDBConvert.ToDecimal(OpSuppData[0]["op_value"]);
                            }
                        }

                        if ((rptPeriodQtr == 3 || rptPeriodQtr == 4) && LMEYrStartQtr < 3)
                        {
                            RptPeriodTable.RowFilter = AddToDataViewFilter(RptPrdFilter, "calendar_year = " + rptPeriodYr + " and quarter = 2");
                            int opSuppRptPrd = cDBConvert.ToInteger(RptPeriodTable[0]["rpt_period_id"]);

                            OpSuppData.RowFilter = AddToDataViewFilter(OpSuppFilter, "rpt_period_id = " + opSuppRptPrd + "and op_type_cd = 'OSTIME'");
                            if (OpSuppData.Count > 0)
                            {
                                if (OpSuppData[0]["op_value"] != DBNull.Value)
                                    LMESummaryOPTimeRecord["os_total"] = cDBConvert.ToDecimal(LMESummaryOPTimeRecord["os_total"]) +
                                                                        cDBConvert.ToDecimal(OpSuppData[0]["op_value"]);
                            }
                        }
                    }

                    if (bLMEGenAnnual && string.IsNullOrEmpty(Category.CheckCatalogResult))
                    {
                        LMESummaryOPTimeRecord["year_total"] = cDBConvert.ToDecimal(LMESummaryOPTimeRecord["current_rpt_period_total"]);
                        if (rptPeriodQtr > LMEYrStartQtr)
                        {
                            RptPeriodTable.RowFilter = AddToDataViewFilter(RptPrdFilter, "calendar_year = " + rptPeriodYr);
                            //RptPeriodTable.Sort = "rpt_period_id";
                            int RptPrdOfYear = cDBConvert.ToInteger(RptPeriodTable[0]["rpt_period_id"]);
                            int quarter = 1;
                            while (RptPrdOfYear < CurrentRptPeriod)
                            {
                                if (quarter >= LMEYrStartQtr)
                                {
                                    OpSuppData.RowFilter = AddToDataViewFilter(OpSuppFilter, "rpt_period_id = " + RptPrdOfYear + "and op_type_cd = 'OPTIME'");
                                    if (OpSuppData.Count > 0)
                                    {
                                        if (OpSuppData[0]["op_value"] != DBNull.Value)
                                            LMESummaryOPTimeRecord["year_total"] = cDBConvert.ToDecimal(LMESummaryOPTimeRecord["year_total"]) +
                                                                            cDBConvert.ToDecimal(OpSuppData[0]["op_value"]);
                                    }

                                }
                                quarter++;
                                RptPrdOfYear++;
                            }

                        }
                    }
                    LMESummaryOpTimeTable.Rows.Add(LMESummaryOPTimeRecord);
                    Category.SetCheckParameter("LME_Summary_Op_Time_Record", LMESummaryOpTimeTable.DefaultView[0], eParameterDataType.DataRowView);
                    OpSuppData.RowFilter = OpSuppFilter;
                    RptPeriodTable.RowFilter = RptPrdFilter;
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "LME33");
            }
            return ReturnVal;
        }

        public static string LME34(cCategory Category, ref bool Log) //Calculate OPHOURS Summary Values
        {
            string ReturnVal = "";
            try
            {
                Category.SetCheckParameter("LME_Summary_Op_Hours_Record", null, eParameterDataType.DataRowView);
                int theLocation = Category.GetCheckParameter("Current_Monitor_Plan_Location_Postion").ValueAsInt();
                DataRowView CurrentMPLocnRecord = Category.GetCheckParameter("Current_Monitor_Plan_Location_Record").ValueAsDataRowView();

                int LMEGenTotalOpHrsArrayLocn = Category.GetCheckParameter("LME_Gen_Total_Op_Hours_Array").ValueAsIntArray()[theLocation];
                decimal LMEGenTotalOpTimeArrayLocn = Category.GetCheckParameter("LME_Gen_Total_Op_Time_Array").ValueAsDecimalArray()[theLocation];

                if (LMEGenTotalOpTimeArrayLocn >= 0 && CurrentMPLocnRecord["unit_id"] != DBNull.Value)
                {
                    int CurrentRptPeriod = Category.GetCheckParameter("Current_Reporting_Period").ValueAsInt();

                    DataTable LMESummaryOpHrsTable = new DataTable("LMESummaryOpHrs");
                    LMESummaryOpHrsTable.Columns.Add("mon_loc_id");
                    LMESummaryOpHrsTable.Columns.Add("rpt_period_id");
                    LMESummaryOpHrsTable.Columns.Add("parameter_cd");
                    LMESummaryOpHrsTable.Columns.Add("current_rpt_period_total");
                    LMESummaryOpHrsTable.Columns.Add("os_total");
                    LMESummaryOpHrsTable.Columns.Add("year_total");

                    DataRow LMESummaryOpHrsRecord = LMESummaryOpHrsTable.NewRow();
                    LMESummaryOpHrsRecord["mon_loc_id"] = cDBConvert.ToString(CurrentMPLocnRecord["mon_loc_id"]);
                    LMESummaryOpHrsRecord["rpt_period_id"] = CurrentRptPeriod;
                    LMESummaryOpHrsRecord["parameter_cd"] = "OPHOURS";

                    DataView RptPeriodTable = Category.GetCheckParameter("Reporting_Period_Lookup_Table").ValueAsDataView();
                    string RptPrdFilter = RptPeriodTable.RowFilter;
                    RptPeriodTable.RowFilter = AddToDataViewFilter(RptPrdFilter, "rpt_period_id = " + CurrentRptPeriod);
                    int rptPeriodQtr = cDBConvert.ToInteger(RptPeriodTable[0]["quarter"]);
                    int rptPeriodYr = cDBConvert.ToInteger(RptPeriodTable[0]["calendar_year"]);

                    int LMEGenAprilOpHrsLocn = Category.GetCheckParameter("LME_Gen_April_Op_Hours_Array").ValueAsIntArray()[theLocation];
                    bool bLMEGenOS = Category.GetCheckParameter("LME_Gen_OS").ValueAsBool();
                    bool bLMEGenAnnual = Category.GetCheckParameter("LME_Gen_Annual").ValueAsBool();
                    int LMEYrStartQtr = Category.GetCheckParameter("LME_Year_Start_Quarter").ValueAsInt();

                    if (bLMEGenOS && !bLMEGenAnnual && rptPeriodQtr == 2)
                        LMESummaryOpHrsRecord["current_rpt_period_total"] = LMEGenTotalOpHrsArrayLocn - LMEGenAprilOpHrsLocn;
                    else
                        LMESummaryOpHrsRecord["current_rpt_period_total"] = LMEGenTotalOpHrsArrayLocn;

                    DataView OpSuppData = Category.GetCheckParameter("Operating_Supp_Data_Records_by_Location").ValueAsDataView();
                    string OpSuppFilter = OpSuppData.RowFilter;

                    if (bLMEGenOS)
                    {
                        if (rptPeriodQtr == 2)
                        {
                            LMESummaryOpHrsRecord["os_total"] = LMEGenTotalOpHrsArrayLocn - LMEGenAprilOpHrsLocn;
                        }
                        else if (rptPeriodQtr == 3)
                        {
                            LMESummaryOpHrsRecord["os_total"] = LMEGenTotalOpHrsArrayLocn;
                        }
                        else if (rptPeriodQtr == 4 && LMEYrStartQtr < 4)
                        {
                            RptPeriodTable.RowFilter = AddToDataViewFilter(RptPrdFilter, "calendar_year = " + rptPeriodYr + " and quarter = 3");
                            int opSuppRptPrd = cDBConvert.ToInteger(RptPeriodTable[0]["rpt_period_id"]);

                            OpSuppData.RowFilter = AddToDataViewFilter(OpSuppFilter, "rpt_period_id = " + opSuppRptPrd + "and op_type_cd = 'OPHOURS'");
                            if (OpSuppData.Count > 0)
                            {
                                if (OpSuppData[0]["op_value"] != DBNull.Value)
                                    LMESummaryOpHrsRecord["os_total"] = cDBConvert.ToDecimal(OpSuppData[0]["op_value"]);
                            }
                        }

                        if ((rptPeriodQtr == 3 || rptPeriodQtr == 4) && LMEYrStartQtr < 3)
                        {
                            RptPeriodTable.RowFilter = AddToDataViewFilter(RptPrdFilter, "calendar_year = " + rptPeriodYr + " and quarter = 2");
                            int opSuppRptPrd = cDBConvert.ToInteger(RptPeriodTable[0]["rpt_period_id"]);

                            OpSuppData.RowFilter = AddToDataViewFilter(OpSuppFilter, "rpt_period_id = " + opSuppRptPrd + " and op_type_cd = 'OSHOURS'");
                            if (OpSuppData.Count > 0)
                            {
                                if (OpSuppData[0]["op_value"] != DBNull.Value)
                                    LMESummaryOpHrsRecord["os_total"] = cDBConvert.ToDecimal(LMESummaryOpHrsRecord["os_total"]) +
                                                                        cDBConvert.ToDecimal(OpSuppData[0]["op_value"]);
                            }
                        }
                    }

                    if (bLMEGenAnnual && string.IsNullOrEmpty(Category.CheckCatalogResult))
                    {
                        LMESummaryOpHrsRecord["year_total"] = cDBConvert.ToDecimal(LMESummaryOpHrsRecord["current_rpt_period_total"]);
                        if (rptPeriodQtr > LMEYrStartQtr)
                        {
                            RptPeriodTable.RowFilter = AddToDataViewFilter(RptPrdFilter, "calendar_year = " + rptPeriodYr);
                            //RptPeriodTable.Sort = "rpt_period_id";
                            int RptPrdOfYear = cDBConvert.ToInteger(RptPeriodTable[0]["rpt_period_id"]);
                            int quarter = 1;
                            while (RptPrdOfYear < CurrentRptPeriod)
                            {
                                if (quarter >= LMEYrStartQtr)
                                {
                                    OpSuppData.RowFilter = AddToDataViewFilter(OpSuppFilter, "rpt_period_id = " + RptPrdOfYear + "and op_type_cd = 'OPHOURS'");
                                    if (OpSuppData.Count > 0)
                                    {
                                        if (OpSuppData[0]["op_value"] != DBNull.Value)
                                            LMESummaryOpHrsRecord["year_total"] = cDBConvert.ToDecimal(LMESummaryOpHrsRecord["year_total"]) +
                                                                            cDBConvert.ToDecimal(OpSuppData[0]["op_value"]);
                                    }

                                }
                                quarter++;
                                RptPrdOfYear++;
                            }

                        }
                    }
                    LMESummaryOpHrsTable.Rows.Add(LMESummaryOpHrsRecord);
                    Category.SetCheckParameter("LME_Summary_Op_Hours_Record", LMESummaryOpHrsTable.DefaultView[0], eParameterDataType.DataRowView);
                    OpSuppData.RowFilter = OpSuppFilter;
                    RptPeriodTable.RowFilter = RptPrdFilter;
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "LME34");
            }
            return ReturnVal;
        }

        public static string LME35(cCategory Category, ref bool Log) //Calculate SO2M Summary Values
        {
            string ReturnVal = "";
            try
            {
                Category.SetCheckParameter("LME_Summary_SO2M_Record", null, eParameterDataType.DataRowView);

                DataRowView CurrentMPLocnRecord = Category.GetCheckParameter("Current_Monitor_Plan_Location_Record").ValueAsDataRowView();
                int theLocation = Category.GetCheckParameter("Current_Monitor_Plan_Location_Postion").ValueAsInt();
                decimal LMEGenTotalSO2MArrayLocn = Category.GetCheckParameter("LME_Gen_Total_SO2M_Array").ValueAsDecimalArray()[theLocation];

                if (CurrentMPLocnRecord["unit_id"] != DBNull.Value && Category.GetCheckParameter("LME_Gen_Annual").ValueAsBool() && LMEGenTotalSO2MArrayLocn >= 0)
                {
                    int CurrentRptPeriod = Category.GetCheckParameter("Current_Reporting_Period").ValueAsInt();
                    DataView RptPeriodTable = Category.GetCheckParameter("Reporting_Period_Lookup_Table").ValueAsDataView();
                    string rptPrdFilter = RptPeriodTable.RowFilter;
                    RptPeriodTable.RowFilter = AddToDataViewFilter(rptPrdFilter, "rpt_period_id = " + CurrentRptPeriod);
                    DateTime rptPeriodBeginDate = cDBConvert.ToDate(RptPeriodTable[0]["begin_date"], DateTypes.START);
                    DateTime rptPeriodEndDate = cDBConvert.ToDate(RptPeriodTable[0]["end_date"], DateTypes.END);
                    int rptPeriodQtr = cDBConvert.ToInteger(RptPeriodTable[0]["quarter"]);
                    int rptPeriodYr = cDBConvert.ToInteger(RptPeriodTable[0]["calendar_year"]);

                    DataView MethodRecords = Category.GetCheckParameter("Method_Records").ValueAsDataView();
                    string methodFilter = MethodRecords.RowFilter;
                    MethodRecords.RowFilter = AddToDataViewFilter(methodFilter, "unit_id = " + cDBConvert.ToInteger(CurrentMPLocnRecord["unit_id"]) +
                                                                    " and parameter_cd = 'SO2M' and method_cd = 'LME'");
                    MethodRecords.RowFilter = AddEvaluationDateRangeToDataViewFilter(MethodRecords.RowFilter, rptPeriodBeginDate, rptPeriodEndDate, true, true, false);

                    if (MethodRecords.Count > 0)
                    {
                        DataTable LMESummarySO2Table = new DataTable("LMESummarySO2");
                        LMESummarySO2Table.Columns.Add("mon_loc_id");
                        LMESummarySO2Table.Columns.Add("rpt_period_id");
                        LMESummarySO2Table.Columns.Add("parameter_cd");
                        LMESummarySO2Table.Columns.Add("current_rpt_period_total");
                        LMESummarySO2Table.Columns.Add("year_total");

                        DataRow LMESummarySO2MRecord = LMESummarySO2Table.NewRow();
                        LMESummarySO2MRecord["mon_loc_id"] = cDBConvert.ToString(CurrentMPLocnRecord["mon_loc_id"]);
                        LMESummarySO2MRecord["rpt_period_id"] = CurrentRptPeriod;
                        LMESummarySO2MRecord["parameter_cd"] = "SO2M";
                        LMESummarySO2MRecord["current_rpt_period_total"] = Math.Round(LMEGenTotalSO2MArrayLocn / 2000, 1, MidpointRounding.AwayFromZero);
                        LMESummarySO2MRecord["year_total"] = cDBConvert.ToDecimal(LMESummarySO2MRecord["current_rpt_period_total"]);

                        int LMEYearStartQtr = Category.GetCheckParameter("LME_Year_Start_Quarter").ValueAsInt();
                        DataView OpSuppData = Category.GetCheckParameter("Operating_Supp_Data_Records_by_Location").ValueAsDataView();
                        string OpSuppFilter = OpSuppData.RowFilter;

                        if (rptPeriodQtr > LMEYearStartQtr)
                        {
                            RptPeriodTable.RowFilter = AddToDataViewFilter(rptPrdFilter, "calendar_year = " + rptPeriodYr);
                            //RptPeriodTable.Sort = "rpt_period_id";
                            int RptPrdOfYear = cDBConvert.ToInteger(RptPeriodTable[0]["rpt_period_id"]);
                            int quarter = 1;
                            while (RptPrdOfYear < CurrentRptPeriod)
                            {
                                if (quarter >= LMEYearStartQtr)
                                {
                                    OpSuppData.RowFilter = AddToDataViewFilter(OpSuppFilter, "rpt_period_id = " + RptPrdOfYear + "and op_type_cd = 'SO2M'");
                                    if (OpSuppData.Count > 0)
                                    {
                                        if (OpSuppData[0]["op_value"] != DBNull.Value)
                                            LMESummarySO2MRecord["year_total"] = cDBConvert.ToDecimal(LMESummarySO2MRecord["year_total"]) +
                                                                            cDBConvert.ToDecimal(OpSuppData[0]["op_value"]);
                                    }

                                }
                                quarter++;
                                RptPrdOfYear++;
                            }
                        }
                        OpSuppData.RowFilter = OpSuppFilter;
                        LMESummarySO2Table.Rows.Add(LMESummarySO2MRecord);
                        Category.SetCheckParameter("LME_Summary_SO2M_Record", LMESummarySO2Table.DefaultView[0], eParameterDataType.DataRowView);
                    }

                    MethodRecords.RowFilter = methodFilter;
                    RptPeriodTable.RowFilter = rptPrdFilter;
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "LME35");
            }
            return ReturnVal;
        }

        public static string LME36(cCategory Category, ref bool Log) //Calculate CO2M Summary Values
        {
            string ReturnVal = "";
            try
            {
                Category.SetCheckParameter("LME_Summary_CO2M_Record", null, eParameterDataType.DataRowView);

                DataRowView CurrentMPLocnRecord = Category.GetCheckParameter("Current_Monitor_Plan_Location_Record").ValueAsDataRowView();
                int theLocation = Category.GetCheckParameter("Current_Monitor_Plan_Location_Postion").ValueAsInt();
                decimal LMEGenTotalCO2MArrayLocn = Category.GetCheckParameter("LME_Gen_Total_CO2M_Array").ValueAsDecimalArray()[theLocation];

                if (CurrentMPLocnRecord["unit_id"] != DBNull.Value && Category.GetCheckParameter("LME_Gen_Annual").ValueAsBool() && LMEGenTotalCO2MArrayLocn >= 0)
                {
                    int CurrentRptPeriod = Category.GetCheckParameter("Current_Reporting_Period").ValueAsInt();
                    DataView RptPeriodTable = Category.GetCheckParameter("Reporting_Period_Lookup_Table").ValueAsDataView();
                    string rptPrdFilter = RptPeriodTable.RowFilter;
                    RptPeriodTable.RowFilter = AddToDataViewFilter(rptPrdFilter, "rpt_period_id = " + CurrentRptPeriod);
                    DateTime rptPeriodBeginDate = cDBConvert.ToDate(RptPeriodTable[0]["begin_date"], DateTypes.START);
                    DateTime rptPeriodEndDate = cDBConvert.ToDate(RptPeriodTable[0]["end_date"], DateTypes.END);
                    int rptPeriodQtr = cDBConvert.ToInteger(RptPeriodTable[0]["quarter"]);
                    int rptPeriodYr = cDBConvert.ToInteger(RptPeriodTable[0]["calendar_year"]);

                    DataView MethodRecords = Category.GetCheckParameter("Method_Records").ValueAsDataView();
                    string methodFilter = MethodRecords.RowFilter;
                    MethodRecords.RowFilter = AddToDataViewFilter(methodFilter, "unit_id = " + cDBConvert.ToInteger(CurrentMPLocnRecord["unit_id"]) +
                                                                    " and parameter_cd = 'CO2M' and method_cd = 'LME'");
                    MethodRecords.RowFilter = AddEvaluationDateRangeToDataViewFilter(MethodRecords.RowFilter, rptPeriodBeginDate, rptPeriodEndDate, true, true, false);

                    if (MethodRecords.Count > 0)
                    {
                        DataTable LMESummaryCO2Table = new DataTable("LMESummaryCO2");
                        LMESummaryCO2Table.Columns.Add("mon_loc_id");
                        LMESummaryCO2Table.Columns.Add("rpt_period_id");
                        LMESummaryCO2Table.Columns.Add("parameter_cd");
                        LMESummaryCO2Table.Columns.Add("current_rpt_period_total");
                        LMESummaryCO2Table.Columns.Add("year_total");
                        DataRow LMESummaryCO2MRecord = LMESummaryCO2Table.NewRow();
                        LMESummaryCO2MRecord["mon_loc_id"] = cDBConvert.ToString(CurrentMPLocnRecord["mon_loc_id"]);
                        LMESummaryCO2MRecord["rpt_period_id"] = CurrentRptPeriod;
                        LMESummaryCO2MRecord["parameter_cd"] = "CO2M";
                        LMESummaryCO2MRecord["current_rpt_period_total"] = LMEGenTotalCO2MArrayLocn;
                        LMESummaryCO2MRecord["year_total"] = cDBConvert.ToDecimal(LMESummaryCO2MRecord["current_rpt_period_total"]);

                        int LMEYearStartQtr = Category.GetCheckParameter("LME_Year_Start_Quarter").ValueAsInt();
                        DataView OpSuppData = Category.GetCheckParameter("Operating_Supp_Data_Records_by_Location").ValueAsDataView();
                        string OpSuppFilter = OpSuppData.RowFilter;

                        if (rptPeriodQtr > LMEYearStartQtr)
                        {
                            RptPeriodTable.RowFilter = AddToDataViewFilter(rptPrdFilter, "calendar_year = " + rptPeriodYr);
                            //RptPeriodTable.Sort = "rpt_period_id";
                            int RptPrdOfYear = cDBConvert.ToInteger(RptPeriodTable[0]["rpt_period_id"]);
                            int quarter = 1;
                            while (RptPrdOfYear < CurrentRptPeriod)
                            {
                                if (quarter >= LMEYearStartQtr)
                                {
                                    OpSuppData.RowFilter = AddToDataViewFilter(OpSuppFilter, "rpt_period_id = " + RptPrdOfYear + "and op_type_cd = 'CO2M'");
                                    if (OpSuppData.Count > 0)
                                    {
                                        if (OpSuppData[0]["op_value"] != DBNull.Value)
                                            LMESummaryCO2MRecord["year_total"] = cDBConvert.ToDecimal(LMESummaryCO2MRecord["year_total"]) +
                                                                            cDBConvert.ToDecimal(OpSuppData[0]["op_value"]);
                                    }

                                }
                                quarter++;
                                RptPrdOfYear++;
                            }
                        }
                        OpSuppData.RowFilter = OpSuppFilter;
                        LMESummaryCO2Table.Rows.Add(LMESummaryCO2MRecord);
                        Category.SetCheckParameter("LME_Summary_CO2M_Record", LMESummaryCO2Table.DefaultView[0], eParameterDataType.DataRowView);
                    }

                    MethodRecords.RowFilter = methodFilter;
                    RptPeriodTable.RowFilter = rptPrdFilter;
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "LME36");
            }
            return ReturnVal;
        }

        public static string LME37(cCategory Category, ref bool Log) //Calculate NOXM Summary Values
        {
            string ReturnVal = "";
            try
            {
                Category.SetCheckParameter("LME_Summary_NOXM_Record", null, eParameterDataType.DataRowView);
                int theLocation = Category.GetCheckParameter("Current_Monitor_Plan_Location_Postion").ValueAsInt();
                DataRowView CurrentMPLocnRecord = Category.GetCheckParameter("Current_Monitor_Plan_Location_Record").ValueAsDataRowView();

                decimal LMEGenTotalNOXMArrayLocn = Category.GetCheckParameter("LME_Gen_Total_NOXM_Array").ValueAsDecimalArray()[theLocation];

                if (LMEGenTotalNOXMArrayLocn >= 0 && CurrentMPLocnRecord["unit_id"] != DBNull.Value)
                {
                    int CurrentRptPeriod = Category.GetCheckParameter("Current_Reporting_Period").ValueAsInt();
                    DataView RptPeriodTable = Category.GetCheckParameter("Reporting_Period_Lookup_Table").ValueAsDataView();
                    string RptPrdFilter = RptPeriodTable.RowFilter;
                    RptPeriodTable.RowFilter = AddToDataViewFilter(RptPrdFilter, "rpt_period_id = " + CurrentRptPeriod);
                    DateTime rptPeriodBeginDate = cDBConvert.ToDate(RptPeriodTable[0]["begin_date"], DateTypes.START);
                    DateTime rptPeriodEndDate = cDBConvert.ToDate(RptPeriodTable[0]["end_date"], DateTypes.END);
                    int rptPeriodQtr = cDBConvert.ToInteger(RptPeriodTable[0]["quarter"]);
                    int rptPeriodYr = cDBConvert.ToInteger(RptPeriodTable[0]["calendar_year"]);
                    bool bLMEGenOS = Category.GetCheckParameter("LME_Gen_OS").ValueAsBool();

                    DataView MethodRecords = Category.GetCheckParameter("Method_Records").ValueAsDataView();
                    string methodFilter = MethodRecords.RowFilter;
                    MethodRecords.RowFilter = AddToDataViewFilter(methodFilter, "unit_id = " + cDBConvert.ToInteger(CurrentMPLocnRecord["unit_id"]) +
                                                                    " and parameter_cd = 'NOXM' and method_cd = 'LME'");

                    DateTime methodFilterBeginDate = ((rptPeriodQtr == 2) && bLMEGenOS) ? new DateTime(rptPeriodYr, 5, 1) : rptPeriodBeginDate;

                    MethodRecords.RowFilter = AddEvaluationDateRangeToDataViewFilter(MethodRecords.RowFilter, methodFilterBeginDate, rptPeriodEndDate, true, true, false);

                    if (MethodRecords.Count > 0)
                    {

                        DataTable LMESummaryNOXMTable = new DataTable("LMESummaryNOXM");
                        LMESummaryNOXMTable.Columns.Add("mon_loc_id");
                        LMESummaryNOXMTable.Columns.Add("rpt_period_id");
                        LMESummaryNOXMTable.Columns.Add("parameter_cd");
                        LMESummaryNOXMTable.Columns.Add("current_rpt_period_total");
                        LMESummaryNOXMTable.Columns.Add("os_total");
                        LMESummaryNOXMTable.Columns.Add("year_total");

                        DataRow LMESummaryNOXMRecord = LMESummaryNOXMTable.NewRow();
                        LMESummaryNOXMRecord["mon_loc_id"] = cDBConvert.ToString(CurrentMPLocnRecord["mon_loc_id"]);
                        LMESummaryNOXMRecord["rpt_period_id"] = CurrentRptPeriod;
                        LMESummaryNOXMRecord["parameter_cd"] = "NOXM";

                        decimal LMEGenAprilNOXMLocn = Category.GetCheckParameter("LME_Gen_April_NOXM_Array").ValueAsDecimalArray()[theLocation];
                        bool bLMEGenAnnual = Category.GetCheckParameter("LME_Gen_Annual").ValueAsBool();
                        int LMEYrStartQtr = Category.GetCheckParameter("LME_Year_Start_Quarter").ValueAsInt();

                        if (bLMEGenOS && !bLMEGenAnnual && rptPeriodQtr == 2)
                            LMESummaryNOXMRecord["current_rpt_period_total"] = Math.Round((LMEGenTotalNOXMArrayLocn - LMEGenAprilNOXMLocn) / 2000, 1, MidpointRounding.AwayFromZero);
                        else
                            LMESummaryNOXMRecord["current_rpt_period_total"] = Math.Round(LMEGenTotalNOXMArrayLocn / 2000, 1, MidpointRounding.AwayFromZero);

                        DataView OpSuppData = Category.GetCheckParameter("Operating_Supp_Data_Records_by_Location").ValueAsDataView();
                        string OpSuppFilter = OpSuppData.RowFilter;

                        if (bLMEGenOS)
                        {
                            if (rptPeriodQtr == 2)
                            {
                                LMESummaryNOXMRecord["os_total"] = Math.Round((LMEGenTotalNOXMArrayLocn - LMEGenAprilNOXMLocn) / 2000, 1, MidpointRounding.AwayFromZero);
                            }
                            else if (rptPeriodQtr == 3)
                            {
                                LMESummaryNOXMRecord["os_total"] = Math.Round(LMEGenTotalNOXMArrayLocn / 2000, 1, MidpointRounding.AwayFromZero);
                            }
                            else if (rptPeriodQtr == 4 && LMEYrStartQtr < 4)
                            {
                                RptPeriodTable.RowFilter = AddToDataViewFilter(RptPrdFilter, "calendar_year = " + rptPeriodYr + " and quarter = 3");
                                int opSuppRptPrd = cDBConvert.ToInteger(RptPeriodTable[0]["rpt_period_id"]);

                                OpSuppData.RowFilter = AddToDataViewFilter(OpSuppFilter, "rpt_period_id = " + opSuppRptPrd + "and op_type_cd = 'NOXM'");
                                if (OpSuppData.Count > 0)
                                {
                                    if (OpSuppData[0]["op_value"] != DBNull.Value)
                                        LMESummaryNOXMRecord["os_total"] = cDBConvert.ToDecimal(OpSuppData[0]["op_value"]);
                                }
                            }

                            if ((rptPeriodQtr == 3 || rptPeriodQtr == 4) && LMEYrStartQtr < 3)
                            {
                                RptPeriodTable.RowFilter = AddToDataViewFilter(RptPrdFilter, "calendar_year = " + rptPeriodYr + " and quarter = 2");
                                int opSuppRptPrd = cDBConvert.ToInteger(RptPeriodTable[0]["rpt_period_id"]);

                                OpSuppData.RowFilter = AddToDataViewFilter(OpSuppFilter, "rpt_period_id = " + opSuppRptPrd + " and op_type_cd = 'NOXMOS'");
                                if (OpSuppData.Count > 0)
                                {
                                    if (OpSuppData[0]["op_value"] != DBNull.Value)
                                        LMESummaryNOXMRecord["os_total"] = cDBConvert.ToDecimal(LMESummaryNOXMRecord["os_total"]) +
                                                                            cDBConvert.ToDecimal(OpSuppData[0]["op_value"]);
                                }
                            }
                        }

                        if (bLMEGenAnnual && string.IsNullOrEmpty(Category.CheckCatalogResult))
                        {
                            LMESummaryNOXMRecord["year_total"] = cDBConvert.ToDecimal(LMESummaryNOXMRecord["current_rpt_period_total"]);
                            if (rptPeriodQtr > LMEYrStartQtr)
                            {
                                RptPeriodTable.RowFilter = AddToDataViewFilter(RptPrdFilter, "calendar_year = " + rptPeriodYr);
                                //RptPeriodTable.Sort = "rpt_period_id";
                                int RptPrdOfYear = cDBConvert.ToInteger(RptPeriodTable[0]["rpt_period_id"]);
                                int quarter = 1;
                                while (RptPrdOfYear < CurrentRptPeriod)
                                {
                                    if (quarter >= LMEYrStartQtr)
                                    {
                                        OpSuppData.RowFilter = AddToDataViewFilter(OpSuppFilter, "rpt_period_id = " + RptPrdOfYear + "and op_type_cd = 'NOXM'");
                                        if (OpSuppData.Count > 0)
                                        {
                                            if (OpSuppData[0]["op_value"] != DBNull.Value)
                                                LMESummaryNOXMRecord["year_total"] = cDBConvert.ToDecimal(LMESummaryNOXMRecord["year_total"]) +
                                                                                cDBConvert.ToDecimal(OpSuppData[0]["op_value"]);
                                        }

                                    }
                                    quarter++;
                                    RptPrdOfYear++;
                                }
                            }
                        }
                        OpSuppData.RowFilter = OpSuppFilter;
                        RptPeriodTable.RowFilter = RptPrdFilter;
                        LMESummaryNOXMTable.Rows.Add(LMESummaryNOXMRecord);
                        Category.SetCheckParameter("LME_Summary_NOXM_Record", LMESummaryNOXMTable.DefaultView[0], eParameterDataType.DataRowView);
                    }
                    MethodRecords.RowFilter = methodFilter;
                    RptPeriodTable.RowFilter = RptPrdFilter;
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "LME37");
            }
            return ReturnVal;
        }

        public static string LME38(cCategory Category, ref bool Log) //Calculate NOXR Summary Values
        {
            string ReturnVal = "";
            try
            {
                Category.SetCheckParameter("LME_Summary_NOXR_Record", null, eParameterDataType.DataRowView);

                DataRowView CurrentMPLocnRecord = Category.GetCheckParameter("Current_Monitor_Plan_Location_Record").ValueAsDataRowView();
                int theLocation = Category.GetCheckParameter("Current_Monitor_Plan_Location_Postion").ValueAsInt();
                decimal LMEGenTotalNOXMArrayLocn = Category.GetCheckParameter("LME_Gen_Total_NOXM_Array").ValueAsDecimalArray()[theLocation];

                if (Category.GetCheckParameter("LME_Summary_Heat_Input_Record").ParameterValue != null && Category.GetCheckParameter("LME_Summary_NOXM_Record").ParameterValue != null)
                {
                    DataRowView LMESummaryHIRecord = Category.GetCheckParameter("LME_Summary_Heat_Input_Record").ValueAsDataRowView();
                    DataRowView LMESummaryNOXMRecord = Category.GetCheckParameter("LME_Summary_NOXM_Record").ValueAsDataRowView();

                    int CurrentRptPeriod = Category.GetCheckParameter("Current_Reporting_Period").ValueAsInt();
                    DataView RptPeriodTable = Category.GetCheckParameter("Reporting_Period_Lookup_Table").ValueAsDataView();
                    string rptPrdFilter = RptPeriodTable.RowFilter;
                    RptPeriodTable.RowFilter = AddToDataViewFilter(rptPrdFilter, "rpt_period_id = " + CurrentRptPeriod);
                    DateTime rptPeriodBeginDate = cDBConvert.ToDate(RptPeriodTable[0]["begin_date"], DateTypes.START);
                    DateTime rptPeriodEndDate = cDBConvert.ToDate(RptPeriodTable[0]["end_date"], DateTypes.END);
                    int rptPeriodYr = cDBConvert.ToInteger(RptPeriodTable[0]["calendar_year"]);
                    int rptPeriodQtr = cDBConvert.ToInteger(RptPeriodTable[0]["quarter"]);

                    DataView ProgramRecords = Category.GetCheckParameter("Location_Program_Records").ValueAsDataView();
                    string programFilter = ProgramRecords.RowFilter;
                    ProgramRecords.RowFilter = AddToDataViewFilter(programFilter, "unit_id = " + cDBConvert.ToInteger(CurrentMPLocnRecord["unit_id"]) +
                                                                    " and prg_cd = 'ARP' and class <> 'NA'");
                    ProgramRecords.RowFilter = AddToDataViewFilter(ProgramRecords.RowFilter, string.Format("unit_monitor_cert_begin_date is not null and unit_monitor_cert_begin_date <= '{0}'", rptPeriodEndDate.ToShortDateString()));
                    ProgramRecords.RowFilter = AddToDataViewFilter(ProgramRecords.RowFilter, string.Format("(end_date is null or end_date >= '{0}')", rptPeriodBeginDate.ToShortDateString()));

                    if (ProgramRecords.Count > 0)
                    {
                        DataTable LMESummaryNOXRTable = new DataTable("LMESummaryNOXR");
                        LMESummaryNOXRTable.Columns.Add("mon_loc_id");
                        LMESummaryNOXRTable.Columns.Add("rpt_period_id");
                        LMESummaryNOXRTable.Columns.Add("parameter_cd");
                        LMESummaryNOXRTable.Columns.Add("current_rpt_period_total");
                        LMESummaryNOXRTable.Columns.Add("year_total");

                        DataRow LMESummaryNOXRRecord = LMESummaryNOXRTable.NewRow();
                        LMESummaryNOXRRecord["mon_loc_id"] = cDBConvert.ToString(CurrentMPLocnRecord["mon_loc_id"]);
                        LMESummaryNOXRRecord["rpt_period_id"] = CurrentRptPeriod;
                        LMESummaryNOXRRecord["parameter_cd"] = "NOXR";

                        decimal HIrptPrdTotal = cDBConvert.ToDecimal(LMESummaryHIRecord["current_rpt_period_total"]);

                        if (LMEGenTotalNOXMArrayLocn == 0)
                            LMESummaryNOXRRecord["current_rpt_period_total"] = 0;
                        else
                            LMESummaryNOXRRecord["current_rpt_period_total"] = Math.Round(LMEGenTotalNOXMArrayLocn / HIrptPrdTotal, 3, MidpointRounding.AwayFromZero);

                        int LMEYrStartQtr = Category.GetCheckParameter("LME_Year_Start_Quarter").ValueAsInt();
                        if (rptPeriodQtr > LMEYrStartQtr)
                        {
                            if (LMESummaryHIRecord["year_total"] != DBNull.Value)
                            {
                                decimal NOxTotal = Category.GetCheckParameter("LME_Gen_Total_NOXM_Array").ValueAsDecimalArray()[theLocation];

                                DataView OpSuppData = Category.GetCheckParameter("Operating_Supp_Data_Records_by_Location").ValueAsDataView();
                                string OpSuppFilter = OpSuppData.RowFilter;

                                RptPeriodTable.RowFilter = AddToDataViewFilter(rptPrdFilter, "calendar_year = " + rptPeriodYr);

                                int RptPrdOfYear = cDBConvert.ToInteger(RptPeriodTable[0]["rpt_period_id"]);
                                int quarter = 1;
                                while (RptPrdOfYear < CurrentRptPeriod)
                                {
                                    if (quarter >= LMEYrStartQtr)
                                    {
                                        OpSuppData.RowFilter = AddToDataViewFilter(OpSuppFilter, "rpt_period_id = " + RptPrdOfYear + "and op_type_cd = 'NOXR'");
                                        if (OpSuppData.Count > 0)
                                        {
                                            decimal NOXRValue = cDBConvert.ToDecimal(OpSuppData[0]["op_value"]);
                                            OpSuppData.RowFilter = AddToDataViewFilter(OpSuppFilter, "rpt_period_id = " + RptPrdOfYear + "and op_type_cd = 'HIT'");

                                            if (OpSuppData.Count > 0)
                                            {
                                                NOxTotal += Math.Round(cDBConvert.ToDecimal(OpSuppData[0]["op_value"]) * NOXRValue, 1, MidpointRounding.AwayFromZero);
                                            }
                                        }

                                    }
                                    quarter++;
                                    RptPrdOfYear++;
                                }
                                if (NOxTotal == 0)
                                    LMESummaryNOXRRecord["year_total"] = 0;
                                else
                                    LMESummaryNOXRRecord["year_total"] = Math.Round(NOxTotal / cDBConvert.ToDecimal(LMESummaryHIRecord["year_total"]), 3, MidpointRounding.AwayFromZero);
                                OpSuppData.RowFilter = OpSuppFilter;
                                RptPeriodTable.RowFilter = rptPrdFilter;
                                //decimal NOXMyrTotal = cDBConvert.ToDecimal(LMESummaryNOXMRecord["year_total"]);
                                //decimal HIyrTotal = cDBConvert.ToDecimal(LMESummaryHIRecord["year_total"]);

                                //if (NOXMyrTotal == 0)
                                //    LMESummaryNOXRRecord["year_total"] = 0;
                                //else
                                //    LMESummaryNOXRRecord["year_total"] = Math.Round((NOXMyrTotal * 2000) / HIyrTotal, 3, MidpointRounding.AwayFromZero);
                            }
                        }
                        else
                        {
                            LMESummaryNOXRRecord["year_total"] = LMESummaryNOXRRecord["current_rpt_period_total"];
                        }
                        LMESummaryNOXRTable.Rows.Add(LMESummaryNOXRRecord);
                        Category.SetCheckParameter("LME_Summary_NOXR_Record", LMESummaryNOXRTable.DefaultView[0], eParameterDataType.DataRowView);


                    }
                    ProgramRecords.RowFilter = programFilter;
                    RptPeriodTable.RowFilter = rptPrdFilter;
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "LME38");
            }
            return ReturnVal;
        }

        public static string LME39(cCategory Category, ref bool Log) //Check LME Import MHHI Indicator
        {
            string ReturnVal = "";
            try
            {
                DataRowView CurrentWSLME = Category.GetCheckParameter("Current_Workspace_LME_Hour").ValueAsDataRowView();
                string MHHIInd = cDBConvert.ToString(CurrentWSLME["mhhi_indicator"]);
                if (MHHIInd != "" && MHHIInd != "Y")
                    Category.CheckCatalogResult = "A";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "LME39");
            }
            return ReturnVal;
        }

        public static string LME40(cCategory Category, ref bool Log) //Check LME MHHI Indicator
        {
            string ReturnVal = "";
            try
            {
                if (Category.GetCheckParameter("Current_LME_Hourly_Op_Record").ParameterValue != null)
                {
                    DataRowView HrlyOpRecord = Category.GetCheckParameter("Current_LME_Hourly_Op_Record").ValueAsDataRowView();
                    if (cDBConvert.ToInteger(HrlyOpRecord["mhhi_indicator"]) == 1)
                    {
                        if (Category.GetCheckParameter("LME_Gen_HI_Substitute_Data_Code").ValueAsString() != "MHHI")
                        {
                            Category.SetCheckParameter("Generate_LME", false, eParameterDataType.Boolean);
                            Category.CheckCatalogResult = "A";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "LME40");
            }
            return ReturnVal;
        }

        #endregion

        #region Checks 41 - 50

        public static string LME41(cCategory Category, ref bool Log) //Check LTFF Fuel Flow Period Code
        {
            string ReturnVal = "";
            try
            {
                DataRowView LTFFRecord = Category.GetCheckParameter("Current_LTFF_Record").ValueAsDataRowView();
                if (Category.GetCheckParameter("LME_OS").ValueAsBool())
                {
                    int CurrentRptPeriod = Category.GetCheckParameter("Current_Reporting_Period").ValueAsInt();

                    //int quarter = Math.Abs(Math.IEEERemainder((double)CurrentRptPeriod, 4) == 0 ? 4 : (int)Math.IEEERemainder((double)CurrentRptPeriod, 4));
                    //if(quarter == 2)
                    if (cDBConvert.ToInteger(LTFFRecord["quarter"]) == 2)
                    {
                        if (LTFFRecord["fuel_flow_period_cd"] == DBNull.Value)
                            Category.CheckCatalogResult = "A";
                    }
                    else
                    {
                        if (LTFFRecord["fuel_flow_period_cd"] != DBNull.Value)
                            Category.CheckCatalogResult = "B";
                    }
                }
                else
                {
                    if (LTFFRecord["fuel_flow_period_cd"] != DBNull.Value)
                        Category.CheckCatalogResult = "C";
                }

            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "LME41");
            }
            return ReturnVal;
        }

        public static string LME42(cCategory Category, ref bool Log) //Check LTFF Total Heat Input
        {
            string ReturnVal = "";
            try
            {
                DataRowView LTFFRecord = Category.GetCheckParameter("Current_LTFF_Record").ValueAsDataRowView();
                DataRowView CurrentLocation = Category.GetCheckParameter("Current_Monitor_Plan_Location_Record").ValueAsDataRowView();
                int LocnPosn = Category.GetCheckParameter("Current_Monitor_Plan_Location_Postion").ValueAsInt();

                //string CurrentEntity = Category.GetCheckParameter("Current_Entity_Type").ValueAsString();
                decimal LMEGenLTFFHI = Category.GetCheckParameter("LME_Gen_LTFF_Heat_Input").ValueAsDecimal();

                string Location = cDBConvert.ToString(CurrentLocation["location_name"]);

                if (Category.GetCheckParameter("LME_Gen_LTFF_Heat_Input").ParameterValue != null)
                {
                    //int TotalHI = cDBConvert.ToInteger(LTFFRecord["total_heat_input"]);                              

                    decimal[] LcnTotalHI = Category.GetCheckParameter("LME_Total_Heat_Input_Array").ValueAsDecimalArray();
                    if (LcnTotalHI[LocnPosn] >= 0)
                    {
                        Category.AccumCheckAggregate("LME_Total_Heat_Input_Array", LocnPosn, LMEGenLTFFHI);
                        if (Category.GetCheckParameter("LME_OS").ValueAsBool() && cDBConvert.ToString(LTFFRecord["fuel_flow_period_cd"]) == "A")
                            Category.AccumCheckAggregate("LME_April_Heat_Input_Array", LocnPosn, LMEGenLTFFHI);
                    }
                    if (Location.PadRight(Location.Length + 1).Substring(0, 2) == "CP")
                    {
                        if (Category.GetCheckParameter("LME_CP_Total_Heat_Input").ValueAsDecimal() >= 0)
                        {
                            Category.AccumCheckAggregate("LME_CP_Total_Heat_Input", LMEGenLTFFHI);
                            if (Category.GetCheckParameter("LME_OS").ValueAsBool() && cDBConvert.ToString(LTFFRecord["fuel_flow_period_cd"]) == "A")
                                Category.AccumCheckAggregate("LME_CP_April_Heat_Input", LMEGenLTFFHI);
                        }
                    }
                }
                else
                {
                    Category.SetArrayParameter("LME_Total_Heat_Input_Array", LocnPosn, -1.0m);
                    if (Location.PadRight(Location.Length + 1).Substring(0, 2) == "CP")
                        Category.SetCheckParameter("LME_CP_Total_Heat_Input", -1.0m, eParameterDataType.Decimal);
                }

                if (Location.PadRight(Location.Length + 1).Substring(0, 2) == "CP")
                {
                    decimal TotalHI4Lcn = Category.GetCheckParameter("LME_Total_Heat_Input_Array").ValueAsDecimalArray()[LocnPosn];
                    Category.SetArrayParameter("Rpt_Period_Hi_Calculated_Accumulator_Array", LocnPosn, TotalHI4Lcn);
                    decimal AprilHI4Lcn = Category.GetCheckParameter("LME_April_Heat_Input_Array").ValueAsDecimalArray()[LocnPosn];
                    Category.SetArrayParameter("April_HI_Calculated_Accumulator_Array", LocnPosn, AprilHI4Lcn);
                    Category.SetArrayParameter("Expected_Summary_Value_Hi_Array", LocnPosn, true);
                }
                decimal TotalHI = cDBConvert.ToDecimal(LTFFRecord["total_heat_input"]);
                if (TotalHI >= 0)
                {
                    decimal RptPrdHIRptdAccm4Lcn = Category.GetCheckParameter("Rpt_Period_Hi_Calculated_Accumulator_Array").ValueAsDecimalArray()[LocnPosn];
                    if (Location.PadRight(Location.Length + 1).Substring(0, 2) == "CP" && RptPrdHIRptdAccm4Lcn > 0)
                    {
                        if (RptPrdHIRptdAccm4Lcn != Decimal.MinValue)
                            Category.AccumCheckAggregate("Rpt_Period_Hi_Reported_Accumulator_Array", LocnPosn, TotalHI);
                        else
                            Category.SetArrayParameter("Rpt_Period_Hi_Reported_Accumulator_Array", LocnPosn, TotalHI);
                    }

                    if (Category.GetCheckParameter("LME_Gen_LTFF_Heat_Input").ParameterValue != null && LMEGenLTFFHI != TotalHI)
                    {
                        DataView HrlyEmissionsTolerances = Category.GetCheckParameter("Hourly_Emissions_Tolerances_Cross_Check_Table").ValueAsDataView();
                        string xcheckFilter = HrlyEmissionsTolerances.RowFilter;
                        HrlyEmissionsTolerances.RowFilter = AddToDataViewFilter(xcheckFilter, "Parameter = 'HI' and UOM = 'MMBTUHR'");
                        decimal HITolerance = cDBConvert.ToDecimal(HrlyEmissionsTolerances[0]["Tolerance"]);
                        if (Math.Abs(TotalHI - LMEGenLTFFHI) > HITolerance)
                            Category.CheckCatalogResult = "A";
                        HrlyEmissionsTolerances.RowFilter = xcheckFilter;
                    }
                }
                else
                {
                    if (Location.PadRight(Location.Length + 1).Substring(0, 2) == "CP")
                        Category.AccumCheckAggregate("Rpt_Period_Hi_Reported_Accumulator_Array", LocnPosn, -1.0m);
                    Category.CheckCatalogResult = "B";
                }


            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "LME42");
            }
            return ReturnVal;
        }

        public static string LME43(cCategory Category, ref bool Log) //Check LTFF Fuel Flow Period Code
        {
            string ReturnVal = "";
            try
            {
                DataRowView LTFFRecord = Category.GetCheckParameter("Current_LTFF_Record").ValueAsDataRowView();
                int CurrentRptPeriod = Category.GetCheckParameter("Current_Reporting_Period").ValueAsInt();
                //int rptPeriod = cDBConvert.ToInteger(LTFFRecord["rpt_period_id"]);
                DataView RptPeriods = Category.GetCheckParameter("Reporting_Period_Lookup_Table").ValueAsDataView();
                int rptPeriodQuarter = cDBConvert.ToInteger(RptPeriods[CurrentRptPeriod - 1]["quarter"]);
                if (rptPeriodQuarter != 2)
                //if (cDBConvert.ToInteger(LTFFRecord["quarter"]) == 2)
                {
                    if (LTFFRecord["fuel_flow_period_cd"] != DBNull.Value)
                        Category.CheckCatalogResult = "A";
                }

            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "LME43");
            }
            return ReturnVal;
        }

        public static string LME44(cCategory Category, ref bool Log) //Check Fuel Codes against LTFF Records
        {
            string ReturnVal = "";
            try
            {
                if (Category.GetCheckParameter("Current_LME_Hourly_Op_Record").ParameterValue != null &&
                    Category.GetCheckParameter("LME_Gen_HI_Method").ValueAsString() == "LTFF")
                {
                    DataRowView HrlyOpRecord = Category.GetCheckParameter("Current_LME_Hourly_Op_Record").ValueAsDataRowView();
                    if (cDBConvert.ToInteger(HrlyOpRecord["mhhi_indicator"]) != 1 && HrlyOpRecord["fuel_cd_list"] != DBNull.Value)
                    {
                        string HrOpMonLocID = cDBConvert.ToString(HrlyOpRecord["mon_loc_id"]);
                        DateTime HrOpBeginDate = cDBConvert.ToDate(HrlyOpRecord["begin_date"], DateTypes.START);
                        //int HrOpBeginHr = cDBConvert.ToInteger(HrlyOpRecord["begin_hour"]);

                        DataView UnitStackConfigRecs = Category.GetCheckParameter("Unit_Stack_Configuration_Records").ValueAsDataView();
                        string USFilter = UnitStackConfigRecs.RowFilter;
                        UnitStackConfigRecs.RowFilter = AddToDataViewFilter(USFilter, "mon_loc_id = '" + HrOpMonLocID + "' and stack_name like 'CP%'");
                        UnitStackConfigRecs.RowFilter = AddEvaluationDateRangeToDataViewFilter(UnitStackConfigRecs.RowFilter, HrOpBeginDate, HrOpBeginDate, true, true, false);
                        string Pipes = ColumnToDatalist(UnitStackConfigRecs, "stack_pipe_mon_loc_id");
                        Pipes = Pipes.Replace(",", "','");
                        UnitStackConfigRecs.RowFilter = USFilter;
                        string[] FuelCdList = cDBConvert.ToString(HrlyOpRecord["fuel_cd_list"]).Split(';');
                        DataView LTFFRecords = Category.GetCheckParameter("LTFF_Records").ValueAsDataView();
                        //DataView MPLocnRecords = Category.GetCheckParameter("Monitoring_Plan_Location_Records").ValueAsDataView();

                        //string MPLocnFilter = MPLocnRecords.RowFilter;
                        //MPLocnRecords.RowFilter = AddToDataViewFilter(MPLocnFilter,"mon_loc_id = '" + HrOpMonLocID + "'");
                        //string MonPlanIDs = ColumnToDatalist(MPLocnRecords,"mon_plan_id");
                        //MonPlanIDs = MonPlanIDs.Replace(",","','");
                        //MPLocnRecords.RowFilter = AddToDataViewFilter(MPLocnFilter,"mon_plan_id in ('" + MonPlanIDs + "')");
                        //string MonLocIDs = ColumnToDatalist(MPLocnRecords,"mon_loc_id");
                        //MonLocIDs = MonLocIDs.Replace(",","','");
                        //MPLocnRecords.RowFilter = MPLocnFilter;
                        string Filter = LTFFRecords.RowFilter;
                        foreach (string FuelCd in FuelCdList)
                        {
                            LTFFRecords.RowFilter = AddToDataViewFilter(Filter, "rpt_period_id = " + cDBConvert.ToInteger(HrlyOpRecord["rpt_period_id"]) +
                                " and (mon_loc_id in ('" + Pipes + "') or mon_loc_id = '" + HrOpMonLocID + "') and fuel_cd = '" + FuelCd + "'");
                            if (LTFFRecords.Count == 0)
                            {
                                Category.SetCheckParameter("Generate_LME", false, eParameterDataType.Boolean);
                                Category.CheckCatalogResult = "A";
                                break;
                            }
                        }
                        LTFFRecords.RowFilter = Filter;
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "LME44");
            }
            return ReturnVal;
        }

        public static string LME45(cCategory category, ref bool log)
        // Duplicate LTFF Record
        {
            string returnVal = "";

            try
            {
                DataRowView currentLtffRecord = category.GetCheckParameter("Current_LTFF_Record").ValueAsDataRowView();
                DataView ltffRecords = category.GetCheckParameter("LTFF_Records").ValueAsDataView();

                int? rptPeriodId = currentLtffRecord["RPT_PERIOD_ID"].AsInteger();
                string monSysId = currentLtffRecord["MON_SYS_ID"].AsString();
                string fuelFlowPeriodCd = currentLtffRecord["FUEL_FLOW_PERIOD_CD"].AsString();

                cFilterCondition[] rowFilter
                  = new cFilterCondition[]
                      {new cFilterCondition("RPT_PERIOD_ID", rptPeriodId.Default(), eFilterDataType.Integer),
               new cFilterCondition("MON_SYS_ID", monSysId.Default()),
               new cFilterCondition("FUEL_FLOW_PERIOD_CD", fuelFlowPeriodCd.Default())};

                if (DuplicateRecordCheck(currentLtffRecord, ltffRecords, "LTFF_ID", rowFilter))
                    category.CheckCatalogResult = "A";
            }
            catch (Exception ex)
            {
                returnVal = category.CheckEngine.FormatError(ex, "LME45");
            }
            return returnVal;
        }

        /// <summary>
        /// Duplicate LME Import Hourly Op Record
        /// </summary>
        /// <param name="category">Category containing the check.</param>
        /// <param name="log">Obsolete log parameter flag.</param>
        /// <returns></returns>
        public static string LME46(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                string lmeDuplicateHourlyOpImportList = null;
                category.SetCheckParameter("LME_Duplicate_Hourly_Op_Import_List", lmeDuplicateHourlyOpImportList, eParameterDataType.String);

                DataView lmeDuplicateHourlyOpImportRecords = category.GetCheckParameter("LME_Duplicate_Hourly_Op_Import_Records").AsDataView();

                if (lmeDuplicateHourlyOpImportRecords.Count > 0)
                {
                    foreach (DataRowView lmeDuplicateHourlyOpImportRow in lmeDuplicateHourlyOpImportRecords)
                    {
                        string beginDate = lmeDuplicateHourlyOpImportRow["BEGIN_DATE"].AsString();
                        string beginHour = lmeDuplicateHourlyOpImportRow["BEGIN_HOUR"].AsString().PadLeft(2, '0');
                        string locationName = lmeDuplicateHourlyOpImportRow["LOCATION_NAME"].AsString();
                        int duplicateCount = lmeDuplicateHourlyOpImportRow["DUPLICATE_COUNT"].AsInteger(0);

                        lmeDuplicateHourlyOpImportList = lmeDuplicateHourlyOpImportList.ListAdd(string.Format("{0}-{1} {2} has {3} rows", beginDate, beginHour, locationName, duplicateCount));
                    }

                    category.CheckCatalogResult = "A";
                }
            }
            catch (Exception ex)
            {
                returnVal = category.CheckEngine.FormatError(ex);

            }

            return returnVal;
        }

        #endregion

    }
}
