using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.CheckEngine.Definitions;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.QA;
using ECMPS.Checks.Qa.Parameters;
using ECMPS.Checks.TestChecks;
using ECMPS.Checks.TypeUtilities;
using ECMPS.Checks.DatabaseAccess;

using ECMPS.Common;
using ECMPS.Definitions.SeverityCode;
using ECMPS.Definitions.Extensions;

namespace ECMPS.Checks.QAEvaluation
{
    public class cQAMain : cQaProcess
    {

        #region Constructors

        public cQAMain(cCheckEngine checkEngine)
            : base(checkEngine)
        {
        }


        #endregion


        #region Public Fields

        DataTable mCalculatedLinearitySummary;
        DataTable mCalculatedHGTestSummary;
        DataTable mCalculatedTestSummary;
        DataTable mCalculatedRATA;
        DataTable mCalculatedRATASummary;
        DataTable mCalculatedCalibrationInjection;
        DataTable mCalculatedCycleTimeSummary;
        DataTable mCalculatedCycleTimeInjection;
        DataTable mCalculatedFlowToLoadReference;
        DataTable mCalculatedGFMCalibration;
        DataTable mCalculatedRATARun;
        DataTable mCalculatedFlowRATARun;
        DataTable mCalculatedRATATraverse;
        DataTable mCalculatedOOC;
        DataTable mCalculatedAPPESummary;
        DataTable mCalculatedAPPERun;
        DataTable mCalculatedAPPEHIOil;
        DataTable mCalculatedAPPEHIGas;
        DataTable mQASupp;
        DataTable mQASuppAttribute;
        DataTable mCalculatedUnitDefault;
        DataTable mCalculatedHGLMESummary;
        DataTable mCalculatedHGLME;
        String TestIDs;

        #endregion


        #region Abstract Overrides

        /// <summary>
        /// Loads the Check Procedure delegates needed for a process code.
        /// </summary>
        /// <param name="checksDllPath">The path of the checks DLLs.</param>
        /// <param name="errorMessage">The message returned if the initialization fails.</param>
        /// <returns>True if the initialization succeeds.</returns>
        public override bool CheckProcedureInit(string checksDllPath, ref string errorMessage)
        {
            bool result;

            try
            {
                Checks[21] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.QA.dll",
                                                                   "ECMPS.Checks.LinearityChecks.cLinearityChecks").Unwrap();
                Checks[22] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.QA.dll",
                                                                   "ECMPS.Checks.RATAChecks.cRATAChecks").Unwrap();
                Checks[23] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.QA.dll",
                                                                   "ECMPS.Checks.CalibrationChecks.cCalibrationChecks").Unwrap();
                Checks[24] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.QA.dll",
                                                                   "ECMPS.Checks.FlowLoadReferenceChecks.cFlowLoadReferenceChecks").Unwrap();
                Checks[25] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.QA.dll",
                                                                   "ECMPS.Checks.FlowLoadCheckChecks.cFlowLoadCheckChecks").Unwrap();
                Checks[26] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.QA.dll",
                                                                   "ECMPS.Checks.CycleTimeChecks.cCycleTimeChecks").Unwrap();
                Checks[27] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.QA.dll",
                                                                   "ECMPS.Checks.TestChecks.cTestChecks").Unwrap();
                Checks[29] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.QA.dll",
                                                                   "ECMPS.Checks.OOCChecks.cOOCChecks").Unwrap();
                Checks[30] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.QA.dll",
                                                                   "ECMPS.Checks.FFACC_Checks.cFFACC_Checks").Unwrap();
                Checks[31] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.QA.dll",
                                                                   "ECMPS.Checks.FFACCTTChecks.cFFACCTTChecks").Unwrap();
                Checks[32] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.QA.dll",
                                                                   "ECMPS.Checks.FF2LBASChecks.cFF2LBASChecks").Unwrap();
                Checks[33] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.QA.dll",
                                                                   "ECMPS.Checks.FF2LTSTChecks.cFF2LTSTChecks").Unwrap();
                Checks[34] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.QA.dll",
                                                                   "ECMPS.Checks.AppendixEChecks.cAppendixEChecks").Unwrap();
                Checks[41] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.QA.dll",
                                                                   "ECMPS.Checks.UnitDefaultChecks.cUnitDefaultChecks").Unwrap();
                Checks[6] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.MonitorPlan.dll",
                                                                  "ECMPS.Checks.MonitorPlanChecks.cMonitorPlanChecks").Unwrap();
                Checks[48] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.QA.dll",
                                                                   "ECMPS.Checks.GFMCalibrationChecks.cGFMCalibrationChecks").Unwrap();
                Checks[50] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.QA.dll",
                                                                   "ECMPS.Checks.HGLMEDefaultChecks.cHGLMEDefaultChecks").Unwrap();

                // Process Dependent
                {
                    object[] constructorArgements = new object[] { this };

                    Checks[53] = (cAetbChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.QA.dll",
                                                                           "ECMPS.Checks.TestChecks.cAetbChecks",
                                                                           true, 0, null,
                                                                           constructorArgements,
                                                                           null, null).Unwrap();

                    Checks[54] = (cPgvpChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.QA.dll",
                                                                           "ECMPS.Checks.TestChecks.cPgvpChecks",
                                                                           true, 0, null,
                                                                           constructorArgements,
                                                                           null, null).Unwrap();
                }

                result = true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.FormatError();
                result = false;
            }

            return result;
        }

        protected override string ExecuteChecksWork()
        {
            bool RunResult;
            string Result = "";
            DataRow NewRow;

            // Initialize category object; checks object
            // run checks for each record in each category
            // update database (logs and calculate values)


            //QA
            string TestTypeCd, TestSummaryId;
            cOtherQA OtherQA;
            cInvalidQA InvalidQA;
            cCheckParameterBands OtherChecks = GetCheckBands("MISC");
            cCheckParameterBands InvalidChecks = GetCheckBands("CRITLV2");

            SetCheckParameter("First_ECMPS_Reporting_Period", CheckEngine.FirstEcmpsReportingPeriodId);

            foreach (DataRow drTest in mSourceData.Tables["QATestSummary"].Rows)
            {

                TestSummaryId = (string)drTest["test_sum_id"];
                TestTypeCd = (string)drTest["test_type_cd"];
                TestIDs = TestIDs.ListAdd(TestSummaryId);

                switch (TestTypeCd)
                {
                    case "HGSI3":
                    case "HGLINE":
                    case "LINE":
                        RunResult = EvaluateLinearity(drTest, mCheckEngine, this);
                        break;
                    case "RATA":
                        RunResult = EvaluateRATA(drTest, mCheckEngine, this);
                        break;
                    case "7DAY":
                        RunResult = Evaluate7Day(drTest, mCheckEngine, this);
                        break;
                    case "CYCLE":
                        RunResult = EvaluateCycle(drTest, mCheckEngine, this);
                        break;
                    case "F2LREF":
                        RunResult = EvaluateFlowLoadReference(drTest, mCheckEngine, this);
                        break;
                    case "F2LCHK":
                        RunResult = EvaluateFlowLoadCheck(drTest, mCheckEngine, this);
                        break;
                    case "ONOFF":
                        RunResult = EvaluateOOC(drTest, mCheckEngine, this);
                        break;
                    case "FFACC":
                        RunResult = EvaluateFFACC(drTest, mCheckEngine, this);
                        break;
                    case "FFACCTT":
                        RunResult = EvaluateFFACCTT(drTest, mCheckEngine, this);
                        break;
                    case "FF2LBAS":
                        RunResult = EvaluateFF2LBAS(drTest, mCheckEngine, this);
                        break;
                    case "FF2LTST":
                        RunResult = EvaluateFF2LTST(drTest, mCheckEngine, this);
                        break;
                    case "APPE":
                        RunResult = EvaluateAppendixE(drTest, mCheckEngine, this);
                        break;
                    case "UNITDEF":
                        RunResult = EvaluateUnitDefault(drTest, mCheckEngine, this);
                        break;
                    default:
                        OtherQA = new cOtherQA(mCheckEngine, this, (string)drTest["mon_loc_id"], TestSummaryId);
                        OtherQA.SetCheckBands(OtherChecks);
                        RunResult = OtherQA.ProcessChecks();

                        if (mCheckEngine.SeverityCd != eSeverityCd.FATAL && Convert.ToBoolean(GetCheckParameter("Miscellaneous_Test_Type_Valid").ParameterValue))
                        {
                            NewRow = mQASupp.NewRow();
                            NewRow["TEST_SUM_ID"] = TestSummaryId;
                            NewRow["TEST_TYPE_CD"] = TestTypeCd;
                            NewRow["MON_LOC_ID"] = (string)drTest["mon_loc_id"];
                            if (drTest["mon_sys_id"] != DBNull.Value)
                                NewRow["MON_SYS_ID"] = (string)drTest["mon_sys_id"];
                            if (drTest["component_id"] != DBNull.Value)
                                NewRow["COMPONENT_ID"] = (string)drTest["component_id"];
                            if (drTest["test_reason_cd"] != DBNull.Value)
                                NewRow["TEST_REASON_CD"] = (string)drTest["test_reason_cd"];
                            NewRow["TEST_NUM"] = (string)drTest["test_num"];
                            if (drTest["end_date"] != DBNull.Value)
                                NewRow["END_DATE"] = cDBConvert.ToString((DateTime)drTest["end_date"]);
                            if (drTest["end_hour"] != DBNull.Value)
                                NewRow["END_HOUR"] = cDBConvert.ToString((Decimal)drTest["end_hour"]);
                            if (drTest["end_min"] != DBNull.Value)
                                NewRow["END_MIN"] = cDBConvert.ToString((Decimal)drTest["end_min"]);
                            if (cDBConvert.ToInteger(drTest["gp_ind"]) == 1)
                                NewRow["GP_IND"] = "1";
                            string CalcTestResCdParameter = cDBConvert.ToString(drTest["test_result_cd"]);
                            if (CalcTestResCdParameter == "FAILED" || CalcTestResCdParameter == "ABORTED")
                                NewRow["TEST_RESULT_CD"] = CalcTestResCdParameter;
                            else
                                if (mCheckEngine.SeverityCd == eSeverityCd.CRIT1)
                                NewRow["TEST_RESULT_CD"] = null;
                            else
                                    if (mCheckEngine.SeverityCd == eSeverityCd.CRIT2)
                                NewRow["TEST_RESULT_CD"] = "INVALID";
                            else
                                NewRow["TEST_RESULT_CD"] = CalcTestResCdParameter;
                            NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;
                            mQASupp.Rows.Add(NewRow);
                        }

                        OtherQA.EraseParameters();
                        break;
                }

                if (mCheckEngine.SeverityCd == eSeverityCd.CRIT2)
                {
                    InvalidQA = new cInvalidQA(mCheckEngine, this, (string)drTest["mon_loc_id"], TestSummaryId);
                    InvalidQA.SetCheckBands(InvalidChecks);
                    RunResult = InvalidQA.ProcessChecks();
                    InvalidQA.EraseParameters();
                }
            }

            DbUpdate(ref Result);

            return Result;
        }

        protected override void InitCalculatedData()
        {
            string ErrorMsg = "";
            mCalculatedLinearitySummary = CloneTable("ECMPS_WS", "QA_LINEARITYSUMMARY", mCheckEngine.DbDataConnection.SQLConnection, ref ErrorMsg);
            if (ErrorMsg == "")
                mCalculatedHGTestSummary = CloneTable("ECMPS_WS", "QA_HGTESTSUMMARY", mCheckEngine.DbDataConnection.SQLConnection, ref ErrorMsg);
            if (ErrorMsg == "")
                mCalculatedTestSummary = CloneTable("ECMPS_WS", "QA_TESTSUMMARY", mCheckEngine.DbDataConnection.SQLConnection, ref ErrorMsg);
            if (ErrorMsg == "")
                mCalculatedRATA = CloneTable("ECMPS_WS", "QA_RATA", mCheckEngine.DbDataConnection.SQLConnection, ref ErrorMsg);
            if (ErrorMsg == "")
                mCalculatedRATASummary = CloneTable("ECMPS_WS", "QA_RATASUMMARY", mCheckEngine.DbDataConnection.SQLConnection, ref ErrorMsg);
            if (ErrorMsg == "")
                mCalculatedRATARun = CloneTable("ECMPS_WS", "QA_RATARUN", mCheckEngine.DbDataConnection.SQLConnection, ref ErrorMsg);
            if (ErrorMsg == "")
                mCalculatedFlowRATARun = CloneTable("ECMPS_WS", "QA_FLOWRATARUN", mCheckEngine.DbDataConnection.SQLConnection, ref ErrorMsg);
            if (ErrorMsg == "")
                mCalculatedRATATraverse = CloneTable("ECMPS_WS", "QA_RATATRAVERSE", mCheckEngine.DbDataConnection.SQLConnection, ref ErrorMsg);
            if (ErrorMsg == "")
                mCalculatedCycleTimeSummary = CloneTable("ECMPS_WS", "QA_CYCLETIMESUMMARY", mCheckEngine.DbDataConnection.SQLConnection, ref ErrorMsg);
            if (ErrorMsg == "")
                mCalculatedCycleTimeInjection = CloneTable("ECMPS_WS", "QA_CYCLETIMEINJECTION", mCheckEngine.DbDataConnection.SQLConnection, ref ErrorMsg);
            if (ErrorMsg == "")
                mCalculatedCalibrationInjection = CloneTable("ECMPS_WS", "QA_CALIBRATIONINJECTION", mCheckEngine.DbDataConnection.SQLConnection, ref ErrorMsg);
            if (ErrorMsg == "")
                mCalculatedFlowToLoadReference = CloneTable("ECMPS_WS", "QA_FLOWTOLOADREFERENCE", mCheckEngine.DbDataConnection.SQLConnection, ref ErrorMsg);
            if (ErrorMsg == "")
                mCalculatedOOC = CloneTable("ECMPS_WS", "QA_ONOFFCALIBRATION", mCheckEngine.DbDataConnection.SQLConnection, ref ErrorMsg);
            if (ErrorMsg == "")
                mCalculatedAPPESummary = CloneTable("ECMPS_WS", "QA_AECorrTestSummary", mCheckEngine.DbDataConnection.SQLConnection, ref ErrorMsg);
            if (ErrorMsg == "")
                mCalculatedAPPERun = CloneTable("ECMPS_WS", "QA_AECorrTestRun", mCheckEngine.DbDataConnection.SQLConnection, ref ErrorMsg);
            if (ErrorMsg == "")
                mCalculatedAPPEHIOil = CloneTable("ECMPS_WS", "QA_AEHeatInputFromOil", mCheckEngine.DbDataConnection.SQLConnection, ref ErrorMsg);
            if (ErrorMsg == "")
                mCalculatedAPPEHIGas = CloneTable("ECMPS_WS", "QA_AEHeatInputFromGas", mCheckEngine.DbDataConnection.SQLConnection, ref ErrorMsg);
            if (ErrorMsg == "")
                mQASupp = CloneTable("ECMPS_WS", "QA_QASUPPLEMENTALDATA", mCheckEngine.DbDataConnection.SQLConnection, ref ErrorMsg);
            if (ErrorMsg == "")
                mQASuppAttribute = CloneTable("ECMPS_WS", "QA_QASUPPLEMENTALATTRIBUTE", mCheckEngine.DbDataConnection.SQLConnection, ref ErrorMsg);
            if (ErrorMsg == "")
                mCalculatedUnitDefault = CloneTable("ECMPS_WS", "QA_UnitDefaultTest", mCheckEngine.DbDataConnection.SQLConnection, ref ErrorMsg);
            if (ErrorMsg == "")
                mCalculatedGFMCalibration = CloneTable("ECMPS_WS", "QA_GFMCALIBRATION", mCheckEngine.DbDataConnection.SQLConnection, ref ErrorMsg);
            if (ErrorMsg == "")
                mCalculatedHGLME = CloneTable("ECMPS_WS", "QA_HgLMEDefaultTestSummary", mCheckEngine.DbDataConnection.SQLConnection, ref ErrorMsg);
            if (ErrorMsg == "")
                mCalculatedHGLMESummary = CloneTable("ECMPS_WS", "QA_HgLMEDefaultTestData", mCheckEngine.DbDataConnection.SQLConnection, ref ErrorMsg);
        }

        public bool EvaluateLinearity(DataRow drTest, cCheckEngine mCheckEngine, cQAMain QA)
        {

            string LinearitySummaryId, LinearityInjectionId, MonitorLocationId, TestSummaryId;
            Boolean SkipCategory, RunResult = true;

            MonitorLocationId = (string)drTest["mon_loc_id"];
            TestSummaryId = (string)drTest["test_sum_id"];

            SkipCategory = false;

            DataRow NewRow;

            cLinearity Linearity;
            cCheckParameterBands LinearityChecks = this.GetCheckBands("LINTEST");

            Linearity = new cLinearity(mCheckEngine, QA, MonitorLocationId, TestSummaryId);
            Linearity.SetCheckBands(LinearityChecks);

            RunResult = Linearity.ProcessChecks();

            if (RunResult == true)
            {

                if ((Boolean)QA.GetCheckParameter("Linearity_Component_Valid").ParameterValue == false)
                    SkipCategory = true;
                else
                {
                    if ((Boolean)QA.GetCheckParameter("Linearity_Test_Aborted").ParameterValue == true)
                        SkipCategory = true;
                }

                if (SkipCategory)
                {

                    QA.mSourceData.Tables["QALinearitySummary"].DefaultView.RowFilter = "test_sum_id = '" + TestSummaryId + "'";

                    foreach (DataRowView drLinearitySummary in QA.mSourceData.Tables["QALinearitySummary"].DefaultView)
                    {
                        //figure out which table to save to with correct column names
                        if (drTest["TEST_TYPE_CD"].ToString().InList("HGLINE,HGSI3"))
                        {
                            NewRow = QA.mCalculatedHGTestSummary.NewRow();
                            NewRow["HG_TEST_SUM_ID"] = Convert.ToString(drLinearitySummary["lin_sum_id"]);
                        }
                        else
                        {
                            NewRow = QA.mCalculatedLinearitySummary.NewRow();
                            NewRow["LIN_SUM_ID"] = Convert.ToString(drLinearitySummary["lin_sum_id"]);
                        }
                        NewRow["CALC_MEAN_REF_VALUE"] = null;
                        NewRow["CALC_MEAN_MEASURED_VALUE"] = null;
                        NewRow["CALC_APS_IND"] = null;
                        NewRow["CALC_PERCENT_ERROR"] = null;
                        NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;

                        //save to the correct table
                        if (drTest["TEST_TYPE_CD"].ToString().InList("HGLINE,HGSI3"))
                        {
                            QA.mCalculatedHGTestSummary.Rows.Add(NewRow);
                        }
                        else
                        {
                            QA.mCalculatedLinearitySummary.Rows.Add(NewRow);
                        }
                    }
                }
                else
                {

                    cLinearitySummary LinearitySummary;
                    cCheckParameterBands LinearitySummaryChecks = GetCheckBands("LINSUM");
                    cCheckParameterBands LinearitySummaryPass2Checks = GetCheckBands("CLCLSUM");

                    QA.mSourceData.Tables["QALinearitySummary"].DefaultView.RowFilter = "test_sum_id = '" + TestSummaryId + "'";
                    QA.mSourceData.Tables["QALinearitySummary"].DefaultView.Sort = "gas_level_cd";

                    foreach (DataRowView drLinearitySummary in QA.mSourceData.Tables["QALinearitySummary"].DefaultView)
                    {

                        SkipCategory = false;

                        LinearitySummaryId = Convert.ToString(drLinearitySummary["lin_sum_id"]);

                        LinearitySummary = new cLinearitySummary(mCheckEngine, QA, MonitorLocationId, LinearitySummaryId, Linearity);
                        LinearitySummary.SetCheckBands(LinearitySummaryChecks);

                        RunResult = LinearitySummary.ProcessChecks();

                        if (RunResult == true)
                        {

                            if ((Boolean)QA.GetCheckParameter("Linearity_Level_Valid").ParameterValue == false)
                                SkipCategory = true;

                            if (SkipCategory)
                            {

                                //figure out which table to save to with correct column names
                                if (drTest["TEST_TYPE_CD"].ToString().InList("HGLINE,HGSI3"))
                                {
                                    NewRow = QA.mCalculatedHGTestSummary.NewRow();
                                    NewRow["HG_TEST_SUM_ID"] = LinearitySummaryId;
                                }
                                else
                                {
                                    NewRow = QA.mCalculatedLinearitySummary.NewRow();
                                    NewRow["LIN_SUM_ID"] = LinearitySummaryId;
                                }
                                NewRow["CALC_MEAN_REF_VALUE"] = null;
                                NewRow["CALC_MEAN_MEASURED_VALUE"] = null;
                                NewRow["CALC_APS_IND"] = null;
                                NewRow["CALC_PERCENT_ERROR"] = null;
                                NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;

                                //save to the correct table
                                if (drTest["TEST_TYPE_CD"].ToString().InList("HGLINE,HGSI3"))
                                {
                                    QA.mCalculatedHGTestSummary.Rows.Add(NewRow);
                                }
                                else
                                {
                                    QA.mCalculatedLinearitySummary.Rows.Add(NewRow);
                                }
                            }

                            else
                            {
                                cLinearityInjection LinearityInjection;
                                cCheckParameterBands LinearityInjectionChecks = GetCheckBands("LININJ");

                                QA.mSourceData.Tables["QALinearityInjection"].DefaultView.RowFilter = "lin_sum_id = '" + LinearitySummaryId + "'";
                                QA.mSourceData.Tables["QALinearityInjection"].DefaultView.Sort = "injection_date desc, injection_hour desc, injection_min desc";

                                foreach (DataRowView drLinearityInjection in QA.mSourceData.Tables["QALinearityInjection"].DefaultView)
                                {
                                    LinearityInjectionId = cDBConvert.ToString(drLinearityInjection["lin_inj_id"]);

                                    LinearityInjection = new cLinearityInjection(mCheckEngine, QA, MonitorLocationId, LinearityInjectionId, LinearitySummary);
                                    LinearityInjection.SetCheckBands(LinearityInjectionChecks);

                                    RunResult = LinearityInjection.ProcessChecks();

                                    LinearityInjection.EraseParameters();

                                }
                                if (RunResult == true)
                                {
                                    LinearitySummary.SetCheckBands(LinearitySummaryPass2Checks);

                                    RunResult = LinearitySummary.ProcessChecks();

                                    if (RunResult == true)
                                    {
                                        //figure out which table to save to with correct column names
                                        if (drTest["TEST_TYPE_CD"].ToString().InList("HGLINE,HGSI3"))
                                        {
                                            NewRow = QA.mCalculatedHGTestSummary.NewRow();
                                            NewRow["HG_TEST_SUM_ID"] = LinearitySummaryId;
                                        }
                                        else
                                        {
                                            NewRow = QA.mCalculatedLinearitySummary.NewRow();
                                            NewRow["LIN_SUM_ID"] = LinearitySummaryId;
                                        }
                                        if (QA.GetCheckParameter("Linearity_Summary_Mean_Reference_Value").ParameterValue != null)
                                            NewRow["CALC_MEAN_REF_VALUE"] = cDBConvert.ToString((Decimal)QA.GetCheckParameter("Linearity_Summary_Mean_Reference_Value").ParameterValue);
                                        if (QA.GetCheckParameter("Linearity_Summary_Mean_Measured_Value").ParameterValue != null)
                                            NewRow["CALC_MEAN_MEASURED_VALUE"] = cDBConvert.ToString((Decimal)QA.GetCheckParameter("Linearity_Summary_Mean_Measured_Value").ParameterValue);
                                        if (QA.GetCheckParameter("Linearity_Summary_APS_Indicator").ParameterValue != null)
                                            NewRow["CALC_APS_IND"] = cDBConvert.ToString((int)QA.GetCheckParameter("Linearity_Summary_APS_Indicator").ParameterValue);
                                        if (QA.GetCheckParameter("Linearity_Summary_Percent_Error").ParameterValue != null)
                                            NewRow["CALC_PERCENT_ERROR"] = cDBConvert.ToString((Decimal)QA.GetCheckParameter("Linearity_Summary_Percent_Error").ParameterValue);
                                        NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;

                                        //save to the correct table
                                        if (drTest["TEST_TYPE_CD"].ToString().InList("HGLINE,HGSI3"))
                                        {
                                            QA.mCalculatedHGTestSummary.Rows.Add(NewRow);
                                        }
                                        else
                                        {
                                            QA.mCalculatedLinearitySummary.Rows.Add(NewRow);
                                        }
                                    }
                                }
                            }
                        }

                        LinearitySummary.EraseParameters();

                    }

                    // LINPGVP: Linearity Protocol Gas Data Category
                    {
                        RunResult = RunResult && RunPgvpChecks("LINPGVP", Linearity);
                    }

                }

                if (RunResult == true)
                {
                    cCheckParameterBands LinearityPass2Checks = GetCheckBands("CLCLINE");

                    Linearity.SetCheckBands(LinearityPass2Checks);

                    RunResult = Linearity.ProcessChecks();

                    if (RunResult == true)
                    {

                        NewRow = QA.mCalculatedTestSummary.NewRow();
                        NewRow["TEST_SUM_ID"] = TestSummaryId;

                        string CalcTestResCdParameter = (string)QA.GetCheckParameter("Linearity_Test_Result").ParameterValue;
                        if (CalcTestResCdParameter == "FAILED" || CalcTestResCdParameter == "ABORTED")
                            NewRow["CALC_TEST_RESULT_CD"] = CalcTestResCdParameter;
                        else
                            if (QA.CheckEngine.SeverityCd == eSeverityCd.FATAL || QA.CheckEngine.SeverityCd == eSeverityCd.CRIT1)
                            NewRow["CALC_TEST_RESULT_CD"] = null;
                        else
                                if (QA.CheckEngine.SeverityCd == eSeverityCd.CRIT2)
                            NewRow["CALC_TEST_RESULT_CD"] = "INVALID";
                        else
                            NewRow["CALC_TEST_RESULT_CD"] = CalcTestResCdParameter;

                        if (QA.GetCheckParameter("Test_Span_Value").ParameterValue != null)
                            NewRow["CALC_SPAN_VALUE"] = cDBConvert.ToString((Decimal)QA.GetCheckParameter("Test_Span_Value").ParameterValue);
                        NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;
                        QA.mCalculatedTestSummary.Rows.Add(NewRow);

                        if (QA.CheckEngine.SeverityCd != eSeverityCd.FATAL)
                        {
                            NewRow = QA.mQASupp.NewRow();
                            if (CalcTestResCdParameter == "FAILED" || CalcTestResCdParameter == "ABORTED")
                                NewRow["TEST_RESULT_CD"] = CalcTestResCdParameter;
                            else
                                if (QA.CheckEngine.SeverityCd == eSeverityCd.CRIT1)
                                NewRow["TEST_RESULT_CD"] = null;
                            else
                                    if (QA.CheckEngine.SeverityCd == eSeverityCd.CRIT2)
                                NewRow["TEST_RESULT_CD"] = "INVALID";
                            else
                                NewRow["TEST_RESULT_CD"] = CalcTestResCdParameter;

                            NewRow["TEST_SUM_ID"] = TestSummaryId;
                            NewRow["TEST_TYPE_CD"] = cDBConvert.ToString(drTest["TEST_TYPE_CD"]);//"LINE" or "HGSI"
                            NewRow["MON_LOC_ID"] = (string)drTest["mon_loc_id"];
                            NewRow["COMPONENT_ID"] = (string)drTest["component_id"];
                            if (drTest["test_reason_cd"] != DBNull.Value)
                                NewRow["TEST_REASON_CD"] = (string)drTest["test_reason_cd"];
                            NewRow["TEST_NUM"] = (string)drTest["test_num"];
                            if (drTest["span_scale_cd"] != DBNull.Value)
                                NewRow["SPAN_SCALE"] = (string)drTest["span_scale_cd"];
                            if (drTest["begin_date"] != DBNull.Value)
                                NewRow["BEGIN_DATE"] = cDBConvert.ToString((DateTime)drTest["begin_date"]);
                            if (drTest["begin_hour"] != DBNull.Value)
                                NewRow["BEGIN_HOUR"] = cDBConvert.ToString((Decimal)drTest["begin_hour"]);
                            if (drTest["begin_min"] != DBNull.Value)
                                NewRow["BEGIN_MIN"] = cDBConvert.ToString((Decimal)drTest["begin_min"]);
                            if (drTest["end_date"] != DBNull.Value)
                                NewRow["END_DATE"] = cDBConvert.ToString((DateTime)drTest["end_date"]);
                            if (drTest["end_hour"] != DBNull.Value)
                                NewRow["END_HOUR"] = cDBConvert.ToString((Decimal)drTest["end_hour"]);
                            if (drTest["end_min"] != DBNull.Value)
                                NewRow["END_MIN"] = cDBConvert.ToString((Decimal)drTest["end_min"]);
                            if (cDBConvert.ToInteger(drTest["gp_ind"]) == 1)
                                NewRow["GP_IND"] = "1";

                            NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;
                            QA.mQASupp.Rows.Add(NewRow);
                        }
                    }
                }
            }

            Linearity.EraseParameters();
            return RunResult;

        }

        public bool EvaluateRATA(DataRow drTest, cCheckEngine mCheckEngine, cQAMain QA)
        {

            string RATASummaryId, RATARunId, MonitorLocationId, TestSummaryId, RATAClaimId, RATAId;
            string FlowRATARunId, RATATraverseId, ReferenceMethodCode;

            Boolean SkipCategory, RunResult = true;

            MonitorLocationId = (string)drTest["mon_loc_id"];
            TestSummaryId = (string)drTest["test_sum_id"];

            QA.mSourceData.Tables["QARATA"].DefaultView.RowFilter = "test_sum_id = '" + TestSummaryId + "'";
            DataRowView drRATA = (DataRowView)QA.mSourceData.Tables["QARATA"].DefaultView[0];
            RATAId = Convert.ToString(drRATA["rata_id"]);
            decimal OverallBAF = cDBConvert.ToDecimal(drRATA["overall_bias_adj_factor"]);

            QA.mSourceData.Tables["QARATA"].DefaultView.RowFilter = "";

            SkipCategory = false;

            DataRow NewRow;

            cRATA RATA;
            cCheckParameterBands RATAChecks = GetCheckBands("RATA");

            RATA = new cRATA(mCheckEngine, QA, MonitorLocationId, TestSummaryId);
            RATA.SetCheckBands(RATAChecks);

            RunResult = RATA.ProcessChecks();

            if (RunResult == true)
            {

                if ((Boolean)QA.GetCheckParameter("RATA_System_Valid").ParameterValue == false)
                    SkipCategory = true;
                else
                {
                    if ((Boolean)QA.GetCheckParameter("RATA_Aborted").ParameterValue == true)
                        SkipCategory = true;
                }

                if (SkipCategory)
                {

                    QA.mSourceData.Tables["QARATASummary"].DefaultView.RowFilter = "test_sum_id = '" + TestSummaryId + "'";

                    foreach (DataRowView drRATASummary in QA.mSourceData.Tables["QARATASummary"].DefaultView)
                    {
                        NewRow = QA.mCalculatedRATASummary.NewRow();
                        NewRow["RATA_SUM_ID"] = Convert.ToString(drRATASummary["rata_sum_id"]);
                        NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;
                        QA.mCalculatedRATASummary.Rows.Add(NewRow);
                    }

                    QA.mSourceData.Tables["QARATARun"].DefaultView.RowFilter = "test_sum_id = '" + TestSummaryId + "'";

                    foreach (DataRowView drRATARun in QA.mSourceData.Tables["QARATARun"].DefaultView)
                    {
                        NewRow = QA.mCalculatedRATARun.NewRow();
                        NewRow["RATA_RUN_ID"] = Convert.ToString(drRATARun["rata_run_id"]);
                        NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;
                        QA.mCalculatedRATARun.Rows.Add(NewRow);
                    }

                    QA.mSourceData.Tables["QAFlowRATARun"].DefaultView.RowFilter = "test_sum_id = '" + TestSummaryId + "'";

                    foreach (DataRowView drFlowRATARun in QA.mSourceData.Tables["QAFlowRATARun"].DefaultView)
                    {
                        NewRow = QA.mCalculatedFlowRATARun.NewRow();
                        NewRow["FLOW_RATA_RUN_ID"] = Convert.ToString(drFlowRATARun["flow_rata_run_id"]);
                        NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;
                        QA.mCalculatedFlowRATARun.Rows.Add(NewRow);
                    }

                    QA.mSourceData.Tables["QARATATraverse"].DefaultView.RowFilter = "test_sum_id = '" + TestSummaryId + "'";

                    foreach (DataRowView drRATATraverse in QA.mSourceData.Tables["QARATATraverse"].DefaultView)
                    {
                        NewRow = QA.mCalculatedRATATraverse.NewRow();
                        NewRow["RATA_TRAVERSE_ID"] = cDBConvert.ToString(drRATATraverse["rata_traverse_id"]);
                        NewRow["CALC_CALC_VEL"] = null;
                        NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;
                        QA.mCalculatedRATATraverse.Rows.Add(NewRow);
                    }
                }

                else
                {

                    cRATAClaim RATAClaim;
                    cCheckParameterBands RATAClaimChecks = GetCheckBands("RATACLM");

                    QA.mSourceData.Tables["QATestClaim"].DefaultView.RowFilter = "test_sum_id = '" + TestSummaryId + "'";

                    foreach (DataRowView drRATAClaim in QA.mSourceData.Tables["QATestClaim"].DefaultView)
                    {

                        RATAClaimId = cDBConvert.ToString(drRATAClaim["test_qualification_id"]);

                        RATAClaim = new cRATAClaim(mCheckEngine, QA, MonitorLocationId, RATAClaimId, RATA);
                        RATAClaim.SetCheckBands(RATAClaimChecks);

                        RunResult = RATAClaim.ProcessChecks();

                        RATAClaim.EraseParameters();
                    }

                    if (RunResult == true)
                    {

                        cRATASummary RATASummary;
                        cCheckParameterBands RATASummaryChecks = GetCheckBands("RATASUM");
                        cCheckParameterBands RATASummaryPass2Checks = GetCheckBands("CLCRSUM");
                        cCheckParameterBands RATASummary2HChecks = GetCheckBands("CLCFLO2");
                        cCheckParameterBands RATARunChecks = GetCheckBands("RATARUN");
                        cCheckParameterBands FlowRATARunChecks = GetCheckBands("FLOWRUN");
                        cCheckParameterBands FlowRATARunPass2Checks = GetCheckBands("CLCFLOW");
                        cCheckParameterBands FlowRATARun2HChecks = GetCheckBands("CLCFLO3");
                        cCheckParameterBands RATATraverseChecks = GetCheckBands("TRAVPT");

                        QA.mSourceData.Tables["QARATASummary"].DefaultView.RowFilter = "test_sum_id = '" + TestSummaryId + "'";
                        QA.mSourceData.Tables["QARATASummary"].DefaultView.Sort = "op_level_cd";

                        foreach (DataRowView drRATASummary in QA.mSourceData.Tables["QARATASummary"].DefaultView)
                        {

                            SkipCategory = false;

                            RATASummaryId = Convert.ToString(drRATASummary["rata_sum_id"]);
                            ReferenceMethodCode = cDBConvert.ToString(drRATASummary["ref_method_cd"]);


                            RATASummary = new cRATASummary(mCheckEngine, QA, MonitorLocationId, RATASummaryId, RATA);
                            RATASummary.SetCheckBands(RATASummaryChecks);

                            RunResult = RATASummary.ProcessChecks();

                            if (RunResult == true)
                            {

                                cRATARun RATARun;

                                QA.mSourceData.Tables["QARATARun"].DefaultView.RowFilter = "rata_sum_id = '" + RATASummaryId + "'";
                                QA.mSourceData.Tables["QARATARun"].DefaultView.Sort = "run_num asc";

                                foreach (DataRowView drRATARun in QA.mSourceData.Tables["QARATARun"].DefaultView)
                                {
                                    RATARunId = Convert.ToString(drRATARun["rata_run_id"]);

                                    RATARun = new cRATARun(mCheckEngine, QA, MonitorLocationId, RATARunId, RATASummary);
                                    RATARun.SetCheckBands(RATARunChecks);

                                    RunResult = RATARun.ProcessChecks();

                                    if (RunResult == true)
                                    {
                                        if ((Boolean)QA.GetCheckParameter("Flow_RATA_Run_Valid").ParameterValue == true)
                                        {

                                            cFlowRATARun FlowRATARun;

                                            QA.mSourceData.Tables["QAFlowRATARun"].DefaultView.RowFilter = "rata_run_id = '" + RATARunId + "'";

                                            foreach (DataRowView drFlowRATARun in QA.mSourceData.Tables["QAFlowRATARun"].DefaultView)
                                            {
                                                FlowRATARunId = Convert.ToString(drFlowRATARun["flow_rata_run_id"]);

                                                FlowRATARun = new cFlowRATARun(mCheckEngine, QA, MonitorLocationId, FlowRATARunId, RATARun);
                                                FlowRATARun.SetCheckBands(FlowRATARunChecks);

                                                RunResult = FlowRATARun.ProcessChecks();

                                                if (RunResult == true)
                                                {

                                                    cRATATraverse RATATraverse;

                                                    QA.mSourceData.Tables["QARATATraverse"].DefaultView.RowFilter = "flow_rata_run_id = '" + FlowRATARunId + "'";
                                                    QA.mSourceData.Tables["QARATATraverse"].DefaultView.Sort = "method_traverse_point_id asc";

                                                    foreach (DataRowView drRATATraverse in QA.mSourceData.Tables["QARATATraverse"].DefaultView)
                                                    {
                                                        RATATraverseId = cDBConvert.ToString(drRATATraverse["rata_traverse_id"]);

                                                        RATATraverse = new cRATATraverse(mCheckEngine, QA, MonitorLocationId, RATATraverseId, FlowRATARun);
                                                        RATATraverse.SetCheckBands(RATATraverseChecks);

                                                        RunResult = RATATraverse.ProcessChecks();

                                                        if (RunResult == true)
                                                        {
                                                            NewRow = QA.mCalculatedRATATraverse.NewRow();
                                                            NewRow["RATA_TRAVERSE_ID"] = cDBConvert.ToString(drRATATraverse["rata_traverse_id"]);
                                                            if (QA.GetCheckParameter("RATA_Traverse_Calc_Velocity").ParameterValue != null)
                                                                NewRow["CALC_CALC_VEL"] = (decimal)QA.GetCheckParameter("RATA_Traverse_Calc_Velocity").ParameterValue;
                                                            NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;
                                                            QA.mCalculatedRATATraverse.Rows.Add(NewRow);
                                                        }

                                                        RATATraverse.EraseParameters();
                                                    }
                                                }

                                                if (RunResult == true)
                                                {

                                                    FlowRATARun.SetCheckBands(FlowRATARunPass2Checks);

                                                    RunResult = FlowRATARun.ProcessChecks();

                                                    if (RunResult == true)
                                                    {
                                                        NewRow = QA.mCalculatedFlowRATARun.NewRow();
                                                        NewRow["FLOW_RATA_RUN_ID"] = Convert.ToString(drFlowRATARun["flow_rata_run_id"]);
                                                        if (QA.GetCheckParameter("RATA_Calc_Average_Adjusted_Velocity").ParameterValue != null)
                                                            NewRow["CALC_AVG_VEL_W_WALL"] = (decimal)QA.GetCheckParameter("RATA_Calc_Average_Adjusted_Velocity").ParameterValue;
                                                        if (QA.GetCheckParameter("RATA_Calc_Average_Velocity").ParameterValue != null)
                                                            NewRow["CALC_AVG_VEL_WO_WALL"] = (decimal)QA.GetCheckParameter("RATA_Calc_Average_Velocity").ParameterValue;
                                                        if (QA.GetCheckParameter("RATA_Calc_Calculated_WAF").ParameterValue != null)
                                                            NewRow["CALC_CALC_WAF"] = (decimal)QA.GetCheckParameter("RATA_Calc_Calculated_WAF").ParameterValue;
                                                        if (QA.GetCheckParameter("RATA_Calc_Dry_Molecular_Weight").ParameterValue != null)
                                                            NewRow["CALC_DRY_MOLECULAR_WEIGHT"] = (decimal)QA.GetCheckParameter("RATA_Calc_Dry_Molecular_Weight").ParameterValue;
                                                        if (QA.GetCheckParameter("RATA_Calc_Wet_Molecular_Weight").ParameterValue != null)
                                                            NewRow["CALC_WET_MOLECULAR_WEIGHT"] = (decimal)QA.GetCheckParameter("RATA_Calc_Wet_Molecular_Weight").ParameterValue;
                                                        NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;
                                                        QA.mCalculatedFlowRATARun.Rows.Add(NewRow);

                                                        if (!ReferenceMethodCode.InList("M2H,D2H,2FH,2GH"))
                                                        {
                                                            NewRow = QA.mCalculatedRATARun.NewRow();
                                                            NewRow["RATA_RUN_ID"] = Convert.ToString(drFlowRATARun["rata_run_id"]);
                                                            if (QA.GetCheckParameter("RATA_Calc_Average_Stack_Flow").ParameterValue != null)
                                                                NewRow["CALC_RATA_REF_VALUE"] = (decimal)QA.GetCheckParameter("RATA_Calc_Average_Stack_Flow").ParameterValue;
                                                            NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;
                                                            QA.mCalculatedRATARun.Rows.Add(NewRow);
                                                        }
                                                    }
                                                }

                                                FlowRATARun.EraseParameters();

                                            }
                                        }
                                        else
                                        {
                                            QA.mSourceData.Tables["QAFlowRATARun"].DefaultView.RowFilter = "rata_run_id = '" + RATARunId + "'";

                                            foreach (DataRowView drFlowRATARun in QA.mSourceData.Tables["QAFlowRATARun"].DefaultView)
                                            {
                                                NewRow = QA.mCalculatedFlowRATARun.NewRow();
                                                NewRow["FLOW_RATA_RUN_ID"] = Convert.ToString(drFlowRATARun["flow_rata_run_id"]);
                                                NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;
                                                QA.mCalculatedFlowRATARun.Rows.Add(NewRow);
                                            }

                                            QA.mSourceData.Tables["QARATATraverse"].DefaultView.RowFilter = "rata_run_id = '" + RATARunId + "'";

                                            foreach (DataRowView drRATATraverse in QA.mSourceData.Tables["QARATATraverse"].DefaultView)
                                            {
                                                NewRow = QA.mCalculatedRATATraverse.NewRow();
                                                NewRow["RATA_TRAVERSE_ID"] = cDBConvert.ToString(drRATATraverse["rata_traverse_id"]);
                                                NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;
                                                QA.mCalculatedRATATraverse.Rows.Add(NewRow);
                                            }
                                        }
                                    }
                                    RATARun.EraseParameters();
                                }

                                if (RunResult == true)
                                {

                                    if (ReferenceMethodCode.InList("M2H,D2H,2FH,2GH"))
                                    {
                                        RATASummary.SetCheckBands(RATASummary2HChecks);

                                        RunResult = RATASummary.ProcessChecks();

                                        if (RunResult == true)
                                        {
                                            QA.mSourceData.Tables["QARATARun"].DefaultView.RowFilter = "rata_sum_id = '" + RATASummaryId + "'";

                                            foreach (DataRowView drRATARun in QA.mSourceData.Tables["QARATARun"].DefaultView)
                                            {
                                                RATARunId = cDBConvert.ToString(drRATARun["rata_run_id"]);

                                                RATARun = new cRATARun(mCheckEngine, QA, MonitorLocationId, RATARunId, RATASummary);

                                                RATARun.SetCheckBands(FlowRATARun2HChecks);

                                                RunResult = RATARun.ProcessChecks();

                                                if (RunResult == true)
                                                {
                                                    NewRow = QA.mCalculatedRATARun.NewRow();
                                                    NewRow["RATA_RUN_ID"] = Convert.ToString(drRATARun["rata_run_id"]);
                                                    if (QA.GetCheckParameter("RATA_Calc_Average_Stack_Flow_2H").ParameterValue != null)
                                                        NewRow["CALC_RATA_REF_VALUE"] = cDBConvert.ToString((Decimal)QA.GetCheckParameter("RATA_Calc_Average_Stack_Flow_2H").ParameterValue);
                                                    NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;
                                                    QA.mCalculatedRATARun.Rows.Add(NewRow);
                                                }
                                                RATARun.EraseParameters();
                                            }
                                        }
                                    }
                                }

                                if (RunResult == true)
                                {

                                    RATASummary.SetCheckBands(RATASummaryPass2Checks);

                                    RunResult = RATASummary.ProcessChecks();

                                    if (RunResult == true)
                                    {

                                        NewRow = QA.mCalculatedRATASummary.NewRow();
                                        NewRow["RATA_SUM_ID"] = RATASummaryId;
                                        if (QA.GetCheckParameter("RATA_Summary_APS_Indicator").ParameterValue != null)
                                            NewRow["CALC_APS_IND"] = cDBConvert.ToString((int)QA.GetCheckParameter("RATA_Summary_APS_Indicator").ParameterValue);
                                        if (QA.GetCheckParameter("RATA_Summary_Average_Gross_Unit_Load").ParameterValue != null)
                                            NewRow["CALC_AVG_GROSS_UNIT_LOAD"] = cDBConvert.ToString((Decimal)QA.GetCheckParameter("RATA_Summary_Average_Gross_Unit_Load").ParameterValue);
                                        if (QA.GetCheckParameter("RATA_Summary_BAF").ParameterValue != null)
                                            NewRow["CALC_BIAS_ADJ_FACTOR"] = cDBConvert.ToString((Decimal)QA.GetCheckParameter("RATA_Summary_BAF").ParameterValue);
                                        if (QA.GetCheckParameter("RATA_Calc_Level_WAF").ParameterValue != null)
                                            NewRow["CALC_CALC_WAF"] = cDBConvert.ToString((Decimal)QA.GetCheckParameter("RATA_Calc_Level_WAF").ParameterValue);
                                        if (QA.GetCheckParameter("RATA_Summary_Confidence_Coefficient").ParameterValue != null)
                                            NewRow["CALC_CONFIDENCE_COEF"] = cDBConvert.ToString((Decimal)QA.GetCheckParameter("RATA_Summary_Confidence_Coefficient").ParameterValue);
                                        if (QA.GetCheckParameter("RATA_Summary_Mean_CEM_Value").ParameterValue != null)
                                            NewRow["CALC_MEAN_CEM_VALUE"] = cDBConvert.ToString((Decimal)QA.GetCheckParameter("RATA_Summary_Mean_CEM_Value").ParameterValue);
                                        if (QA.GetCheckParameter("RATA_Summary_Mean_Difference").ParameterValue != null)
                                            NewRow["CALC_MEAN_DIFF"] = cDBConvert.ToString((Decimal)QA.GetCheckParameter("RATA_Summary_Mean_Difference").ParameterValue);
                                        if (QA.GetCheckParameter("RATA_Summary_Mean_Reference_Value").ParameterValue != null)
                                            NewRow["CALC_MEAN_RATA_REF_VALUE"] = cDBConvert.ToString((Decimal)QA.GetCheckParameter("RATA_Summary_Mean_Reference_Value").ParameterValue);
                                        if (QA.GetCheckParameter("RATA_Summary_Relative_Accuracy").ParameterValue != null)
                                            NewRow["CALC_RELATIVE_ACCURACY"] = cDBConvert.ToString((Decimal)QA.GetCheckParameter("RATA_Summary_Relative_Accuracy").ParameterValue);
                                        if (QA.GetCheckParameter("RATA_Calc_Stack_Area").ParameterValue != null)
                                            NewRow["CALC_STACK_AREA"] = cDBConvert.ToString((Decimal)QA.GetCheckParameter("RATA_Calc_Stack_Area").ParameterValue);
                                        if (QA.GetCheckParameter("RATA_Summary_Standard_Deviation").ParameterValue != null)
                                            NewRow["CALC_STND_DEV_DIFF"] = cDBConvert.ToString((Decimal)QA.GetCheckParameter("RATA_Summary_Standard_Deviation").ParameterValue);
                                        if (QA.GetCheckParameter("RATA_Summary_TValue").ParameterValue != null)
                                            NewRow["CALC_T_VALUE"] = cDBConvert.ToString((Decimal)QA.GetCheckParameter("RATA_Summary_TValue").ParameterValue);
                                        NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;
                                        QA.mCalculatedRATASummary.Rows.Add(NewRow);
                                    }
                                }
                            }

                            RATASummary.EraseParameters();

                        }
                    }

                    // RATPGVP: RATA Protocol Gas Data Category
                    {
                        RunResult = RunResult && RunPgvpChecks("RATPGVP", RATA);
                    }

                    // RATAETB: RATA Air Emission Testing Data
                    {
                        RunResult = RunResult && RunAetbChecks("RATAETB", RATA);
                    }

                }

                if (RunResult == true)
                {
                    cCheckParameterBands RATAPass2Checks = GetCheckBands("CLCRATA");

                    RATA.SetCheckBands(RATAPass2Checks);

                    RunResult = RATA.ProcessChecks();

                    if (RunResult == true)
                    {

                        NewRow = QA.mCalculatedTestSummary.NewRow();
                        NewRow["TEST_SUM_ID"] = TestSummaryId;

                        string CalcTestResCdParameter = (string)QA.GetCheckParameter("RATA_Result").ParameterValue;

                        if (CalcTestResCdParameter == "FAILED" || CalcTestResCdParameter == "ABORTED")
                            NewRow["CALC_TEST_RESULT_CD"] = CalcTestResCdParameter;
                        else if (QA.CheckEngine.SeverityCd == eSeverityCd.FATAL || QA.CheckEngine.SeverityCd == eSeverityCd.CRIT1)
                            NewRow["CALC_TEST_RESULT_CD"] = null;
                        else if (QA.CheckEngine.SeverityCd == eSeverityCd.CRIT2)
                            NewRow["CALC_TEST_RESULT_CD"] = "INVALID";
                        else
                            NewRow["CALC_TEST_RESULT_CD"] = CalcTestResCdParameter;

                        NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;
                        QA.mCalculatedTestSummary.Rows.Add(NewRow);

                        NewRow = QA.mCalculatedRATA.NewRow();
                        NewRow["RATA_ID"] = RATAId;
                        if (QA.GetCheckParameter("Overall_Relative_Accuracy").ParameterValue != null)
                            NewRow["CALC_RELATIVE_ACCURACY"] = cDBConvert.ToString((Decimal)QA.GetCheckParameter("Overall_Relative_Accuracy").ParameterValue);
                        NewRow["CALC_RATA_FREQUENCY_CD"] = (string)QA.GetCheckParameter("RATA_Frequency").ParameterValue;
                        if (QA.GetCheckParameter("Overall_BAF").ParameterValue != null)
                            NewRow["CALC_OVERALL_BIAS_ADJ_FACTOR"] = cDBConvert.ToString((Decimal)QA.GetCheckParameter("Overall_BAF").ParameterValue);
                        if (QA.GetCheckParameter("RATA_NUMBER_OF_LOAD_LEVELS").ParameterValue != null)
                            NewRow["CALC_NUM_LOAD_LEVEL"] = cDBConvert.ToString((int)QA.GetCheckParameter("RATA_Number_Of_Load_Levels").ParameterValue);
                        NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;
                        QA.mCalculatedRATA.Rows.Add(NewRow);

                        if (QA.CheckEngine.SeverityCd != eSeverityCd.FATAL)
                        {
                            NewRow = QA.mQASupp.NewRow();

                            if (CalcTestResCdParameter == "FAILED" || CalcTestResCdParameter == "ABORTED")
                                NewRow["TEST_RESULT_CD"] = CalcTestResCdParameter;
                            else if (QA.CheckEngine.SeverityCd == eSeverityCd.CRIT1)
                                NewRow["TEST_RESULT_CD"] = null;
                            else if (QA.CheckEngine.SeverityCd == eSeverityCd.CRIT2)
                                NewRow["TEST_RESULT_CD"] = "INVALID";
                            /* 
                             * If CalcTestResCdParameter equals null and QA.CheckEngine.SeverityCd is not CRIT1, 
                             * supressions of all CRIT1 errors must have occurred.  So use the report test 
                             * result as the calculated. 
                             */
                            else if (CalcTestResCdParameter == null)
                                NewRow["TEST_RESULT_CD"] = drTest["TEST_RESULT_CD"];
                            else
                                NewRow["TEST_RESULT_CD"] = CalcTestResCdParameter;

                           NewRow["TEST_SUM_ID"] = TestSummaryId;
                            NewRow["TEST_TYPE_CD"] = "RATA";
                            NewRow["MON_LOC_ID"] = (string)drTest["mon_loc_id"];
                            NewRow["MON_SYS_ID"] = (string)drTest["mon_sys_id"];
                            if (drTest["test_reason_cd"] != DBNull.Value)
                                NewRow["TEST_REASON_CD"] = (string)drTest["test_reason_cd"];
                            NewRow["TEST_NUM"] = (string)drTest["test_num"];
                            if (drTest["begin_date"] != DBNull.Value)
                                NewRow["BEGIN_DATE"] = cDBConvert.ToString((DateTime)drTest["begin_date"]);
                            if (drTest["begin_hour"] != DBNull.Value)
                                NewRow["BEGIN_HOUR"] = cDBConvert.ToString((Decimal)drTest["begin_hour"]);
                            if (drTest["begin_min"] != DBNull.Value)
                                NewRow["BEGIN_MIN"] = cDBConvert.ToString((Decimal)drTest["begin_min"]);
                            if (drTest["end_date"] != DBNull.Value)
                                NewRow["END_DATE"] = cDBConvert.ToString((DateTime)drTest["end_date"]);
                            if (drTest["end_hour"] != DBNull.Value)
                                NewRow["END_HOUR"] = cDBConvert.ToString((Decimal)drTest["end_hour"]);
                            if (drTest["end_min"] != DBNull.Value)
                                NewRow["END_MIN"] = cDBConvert.ToString((Decimal)drTest["end_min"]);
                            if (cDBConvert.ToInteger(drTest["gp_ind"]) == 1)
                                NewRow["GP_IND"] = "1";

                            NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;
                            QA.mQASupp.Rows.Add(NewRow);

                            if (QA.GetCheckParameter("RATA_Level_List").ParameterValue != null)
                            {
                                NewRow = QA.mQASuppAttribute.NewRow();
                                NewRow["TEST_SUM_ID"] = TestSummaryId;
                                NewRow["ATTRIBUTE_NAME"] = "OP_LEVEL_CD_LIST";
                                NewRow["ATTRIBUTE_VALUE"] = (string)QA.GetCheckParameter("RATA_Level_List").ParameterValue;
                                NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;
                                QA.mQASuppAttribute.Rows.Add(NewRow);
                            }

                            if (QA.GetCheckParameter("RATA_Claim_Code").ParameterValue != null)
                            {
                                NewRow = QA.mQASuppAttribute.NewRow();
                                NewRow["TEST_SUM_ID"] = TestSummaryId;
                                NewRow["ATTRIBUTE_NAME"] = "TEST_CLAIM_CD";
                                NewRow["ATTRIBUTE_VALUE"] = (string)QA.GetCheckParameter("RATA_Claim_Code").ParameterValue;
                                NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;
                                QA.mQASuppAttribute.Rows.Add(NewRow);
                            }

                            if (QA.CheckEngine.SeverityCd != eSeverityCd.CRIT1)
                            {
                                if (QA.GetCheckParameter("RATA_Frequency").ParameterValue != null && QA.CheckEngine.SeverityCd != eSeverityCd.CRIT1)
                                {
                                    NewRow = QA.mQASuppAttribute.NewRow();
                                    NewRow["TEST_SUM_ID"] = TestSummaryId;
                                    NewRow["ATTRIBUTE_NAME"] = "RATA_FREQUENCY_CD";
                                    NewRow["ATTRIBUTE_VALUE"] = (string)QA.GetCheckParameter("RATA_Frequency").ParameterValue;
                                    NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;
                                    QA.mQASuppAttribute.Rows.Add(NewRow);
                                }

                                if (OverallBAF >= 1)
                                {
                                    NewRow = QA.mQASuppAttribute.NewRow();
                                    NewRow["TEST_SUM_ID"] = TestSummaryId;
                                    NewRow["ATTRIBUTE_NAME"] = "OVERALL_BIAS_ADJ_FACTOR";
                                    NewRow["ATTRIBUTE_VALUE"] = OverallBAF.ToString("0.000");
                                    NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;
                                    QA.mQASuppAttribute.Rows.Add(NewRow);
                                }

                                if ((string)drTest["sys_type_cd"] == "FLOW")
                                {
                                    if (QA.GetCheckParameter("High_BAF").ParameterValue != null)
                                    {
                                        NewRow = QA.mQASuppAttribute.NewRow();
                                        NewRow["TEST_SUM_ID"] = TestSummaryId;
                                        NewRow["ATTRIBUTE_NAME"] = "HIGH_BAF";
                                        NewRow["ATTRIBUTE_VALUE"] = cDBConvert.ToString((decimal)QA.GetCheckParameter("High_BAF").ParameterValue);
                                        NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;
                                        QA.mQASuppAttribute.Rows.Add(NewRow);
                                    }

                                    if (QA.GetCheckParameter("Mid_BAF").ParameterValue != null)
                                    {
                                        NewRow = QA.mQASuppAttribute.NewRow();
                                        NewRow["TEST_SUM_ID"] = TestSummaryId;
                                        NewRow["ATTRIBUTE_NAME"] = "MID_BAF";
                                        NewRow["ATTRIBUTE_VALUE"] = cDBConvert.ToString((decimal)QA.GetCheckParameter("Mid_BAF").ParameterValue);
                                        NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;
                                        QA.mQASuppAttribute.Rows.Add(NewRow);
                                    }

                                    if (QA.GetCheckParameter("Low_BAF").ParameterValue != null)
                                    {
                                        NewRow = QA.mQASuppAttribute.NewRow();
                                        NewRow["TEST_SUM_ID"] = TestSummaryId;
                                        NewRow["ATTRIBUTE_NAME"] = "LOW_BAF";
                                        NewRow["ATTRIBUTE_VALUE"] = cDBConvert.ToString((decimal)QA.GetCheckParameter("Low_BAF").ParameterValue);
                                        NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;
                                        QA.mQASuppAttribute.Rows.Add(NewRow);
                                    }

                                    if (QA.GetCheckParameter("High_Run_Count").ParameterValue != null)
                                    {
                                        NewRow = QA.mQASuppAttribute.NewRow();
                                        NewRow["TEST_SUM_ID"] = TestSummaryId;
                                        NewRow["ATTRIBUTE_NAME"] = "HIGH_RUN_USED_COUNT";
                                        NewRow["ATTRIBUTE_VALUE"] = cDBConvert.ToString((decimal)QA.GetCheckParameter("High_Run_Count").ParameterValue);
                                        NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;
                                        QA.mQASuppAttribute.Rows.Add(NewRow);
                                    }

                                    if (QA.GetCheckParameter("Mid_Run_Count").ParameterValue != null)
                                    {
                                        NewRow = QA.mQASuppAttribute.NewRow();
                                        NewRow["TEST_SUM_ID"] = TestSummaryId;
                                        NewRow["ATTRIBUTE_NAME"] = "MID_RUN_USED_COUNT";
                                        NewRow["ATTRIBUTE_VALUE"] = cDBConvert.ToString((decimal)QA.GetCheckParameter("Mid_Run_Count").ParameterValue);
                                        NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;
                                        QA.mQASuppAttribute.Rows.Add(NewRow);
                                    }

                                    if (QA.GetCheckParameter("Low_Run_Count").ParameterValue != null)
                                    {
                                        NewRow = QA.mQASuppAttribute.NewRow();
                                        NewRow["TEST_SUM_ID"] = TestSummaryId;
                                        NewRow["ATTRIBUTE_NAME"] = "LOW_RUN_USED_COUNT";
                                        NewRow["ATTRIBUTE_VALUE"] = cDBConvert.ToString((decimal)QA.GetCheckParameter("Low_Run_Count").ParameterValue);
                                        NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;
                                        QA.mQASuppAttribute.Rows.Add(NewRow);
                                    }

                                    if (QA.GetCheckParameter("High_Sum_Reference_Value").ParameterValue != null)
                                    {
                                        NewRow = QA.mQASuppAttribute.NewRow();
                                        NewRow["TEST_SUM_ID"] = TestSummaryId;
                                        NewRow["ATTRIBUTE_NAME"] = "HIGH_SUM_RATA_REF_VALUE";
                                        NewRow["ATTRIBUTE_VALUE"] = cDBConvert.ToString((decimal)QA.GetCheckParameter("High_Sum_Reference_Value").ParameterValue);
                                        NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;
                                        QA.mQASuppAttribute.Rows.Add(NewRow);
                                    }

                                    if (QA.GetCheckParameter("Mid_Sum_Reference_Value").ParameterValue != null)
                                    {
                                        NewRow = QA.mQASuppAttribute.NewRow();
                                        NewRow["TEST_SUM_ID"] = TestSummaryId;
                                        NewRow["ATTRIBUTE_NAME"] = "MID_SUM_RATA_REF_VALUE";
                                        NewRow["ATTRIBUTE_VALUE"] = cDBConvert.ToString((decimal)QA.GetCheckParameter("Mid_Sum_Reference_Value").ParameterValue);
                                        NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;
                                        QA.mQASuppAttribute.Rows.Add(NewRow);
                                    }

                                    if (QA.GetCheckParameter("Low_Sum_Reference_Value").ParameterValue != null)
                                    {
                                        NewRow = QA.mQASuppAttribute.NewRow();
                                        NewRow["TEST_SUM_ID"] = TestSummaryId;
                                        NewRow["ATTRIBUTE_NAME"] = "LOW_SUM_RATA_REF_VALUE";
                                        NewRow["ATTRIBUTE_VALUE"] = cDBConvert.ToString((decimal)QA.GetCheckParameter("Low_Sum_Reference_Value").ParameterValue);
                                        NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;
                                        QA.mQASuppAttribute.Rows.Add(NewRow);
                                    }

                                    if (QA.GetCheckParameter("High_Sum_Gross_Unit_Load").ParameterValue != null)
                                    {
                                        NewRow = QA.mQASuppAttribute.NewRow();
                                        NewRow["TEST_SUM_ID"] = TestSummaryId;
                                        NewRow["ATTRIBUTE_NAME"] = "HIGH_SUM_GROSS_UNIT_LOAD";
                                        NewRow["ATTRIBUTE_VALUE"] = cDBConvert.ToString((decimal)QA.GetCheckParameter("High_Sum_Gross_Unit_Load").ParameterValue);
                                        NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;
                                        QA.mQASuppAttribute.Rows.Add(NewRow);
                                    }

                                    if (QA.GetCheckParameter("Mid_Sum_Gross_Unit_Load").ParameterValue != null)
                                    {
                                        NewRow = QA.mQASuppAttribute.NewRow();
                                        NewRow["TEST_SUM_ID"] = TestSummaryId;
                                        NewRow["ATTRIBUTE_NAME"] = "MID_SUM_GROSS_UNIT_LOAD";
                                        NewRow["ATTRIBUTE_VALUE"] = cDBConvert.ToString((decimal)QA.GetCheckParameter("Mid_Sum_Gross_Unit_Load").ParameterValue);
                                        NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;
                                        QA.mQASuppAttribute.Rows.Add(NewRow);
                                    }

                                    if (QA.GetCheckParameter("Low_Sum_Gross_Unit_Load").ParameterValue != null)
                                    {
                                        NewRow = QA.mQASuppAttribute.NewRow();
                                        NewRow["TEST_SUM_ID"] = TestSummaryId;
                                        NewRow["ATTRIBUTE_NAME"] = "LOW_SUM_GROSS_UNIT_LOAD";
                                        NewRow["ATTRIBUTE_VALUE"] = cDBConvert.ToString((decimal)QA.GetCheckParameter("Low_Sum_Gross_Unit_Load").ParameterValue);
                                        NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;
                                        QA.mQASuppAttribute.Rows.Add(NewRow);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            RATA.EraseParameters();
            return RunResult;

        }

        public bool EvaluateFlowLoadCheck(DataRow drTest, cCheckEngine mCheckEngine, cQAMain QA)
        {

            string MonitorLocationId, TestSummaryId;
            Boolean RunResult = true;

            MonitorLocationId = (string)drTest["mon_loc_id"];
            TestSummaryId = (string)drTest["test_sum_id"];

            QA.mSourceData.Tables["QAFlowLoadCheck"].DefaultView.RowFilter = "test_sum_id = '" + TestSummaryId + "'";
            DataRowView drFlowLoadCheck = (DataRowView)QA.mSourceData.Tables["QAFlowLoadCheck"].DefaultView[0];
            string OpLevelCd = cDBConvert.ToString(drFlowLoadCheck["op_level_cd"]);
            QA.mSourceData.Tables["QAFlowLoadCheck"].DefaultView.RowFilter = "";

            DataRow NewRow;

            cFlowLoadCheck FlowLoadCheck;
            cCheckParameterBands FlowLoadCheckChecks = GetCheckBands("F2LCHK");

            FlowLoadCheck = new cFlowLoadCheck(mCheckEngine, QA, MonitorLocationId, TestSummaryId);
            FlowLoadCheck.SetCheckBands(FlowLoadCheckChecks);

            RunResult = FlowLoadCheck.ProcessChecks();

            if (RunResult == true)
            {

                NewRow = QA.mCalculatedTestSummary.NewRow();
                NewRow["TEST_SUM_ID"] = TestSummaryId;

                string CalcTestResCdParameter = (string)QA.GetCheckParameter("Flow_to_Load_Check_Calc_Test_Result").ParameterValue;
                if (CalcTestResCdParameter == "FAILED" || CalcTestResCdParameter == "ABORTED")
                    NewRow["CALC_TEST_RESULT_CD"] = CalcTestResCdParameter;
                else
                    if (QA.CheckEngine.SeverityCd == eSeverityCd.FATAL || QA.CheckEngine.SeverityCd == eSeverityCd.CRIT1)
                    NewRow["CALC_TEST_RESULT_CD"] = null;
                else
                        if (QA.CheckEngine.SeverityCd == eSeverityCd.CRIT2)
                    NewRow["CALC_TEST_RESULT_CD"] = "INVALID";
                else
                    NewRow["CALC_TEST_RESULT_CD"] = CalcTestResCdParameter;

                NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;
                QA.mCalculatedTestSummary.Rows.Add(NewRow);


                if (QA.CheckEngine.SeverityCd != eSeverityCd.FATAL)
                {
                    NewRow = QA.mQASupp.NewRow();

                    if (CalcTestResCdParameter == "FAILED" || CalcTestResCdParameter == "ABORTED")
                        NewRow["TEST_RESULT_CD"] = CalcTestResCdParameter;
                    else
                        if (QA.CheckEngine.SeverityCd == eSeverityCd.CRIT1)
                        NewRow["TEST_RESULT_CD"] = null;
                    else
                            if (QA.CheckEngine.SeverityCd == eSeverityCd.CRIT2)
                        NewRow["TEST_RESULT_CD"] = "INVALID";
                    else
                        NewRow["TEST_RESULT_CD"] = CalcTestResCdParameter;

                    NewRow["TEST_SUM_ID"] = TestSummaryId;
                    NewRow["TEST_TYPE_CD"] = "F2LCHK";
                    NewRow["MON_LOC_ID"] = (string)drTest["mon_loc_id"];
                    NewRow["MON_SYS_ID"] = (string)drTest["mon_sys_id"];
                    NewRow["TEST_NUM"] = (string)drTest["test_num"];
                    if (drTest["test_reason_cd"] != DBNull.Value)
                        NewRow["TEST_REASON_CD"] = (string)drTest["test_reason_cd"];
                    if (drTest["rpt_period_id"] != DBNull.Value)
                        NewRow["RPT_PERIOD_ID"] = (int)drTest["rpt_period_id"];
                    if (OpLevelCd != "")
                        NewRow["OP_LEVEL_CD"] = OpLevelCd;

                    NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;
                    QA.mQASupp.Rows.Add(NewRow);
                }
            }

            FlowLoadCheck.EraseParameters();

            return RunResult;

        }

        public bool EvaluateOOC(DataRow drTest, cCheckEngine mCheckEngine, cQAMain QA)
        {

            string MonitorLocationId, TestSummaryId, OOCId;
            Boolean RunResult = true;

            MonitorLocationId = (string)drTest["mon_loc_id"];
            TestSummaryId = (string)drTest["test_sum_id"];
            QA.mSourceData.Tables["QAOOCTest"].DefaultView.RowFilter = "test_sum_id = '" + TestSummaryId + "'";
            DataRowView drOOC = (DataRowView)QA.mSourceData.Tables["QAOOCTest"].DefaultView[0];
            OOCId = cDBConvert.ToString(drOOC["on_off_cal_id"]);
            QA.mSourceData.Tables["QAOOCTest"].DefaultView.RowFilter = "";


            DataRow NewRow;

            cOOC OOC;
            cCheckParameterBands OOCChecks = GetCheckBands("ONOFF");

            OOC = new cOOC(mCheckEngine, QA, MonitorLocationId, TestSummaryId);
            OOC.SetCheckBands(OOCChecks);

            RunResult = OOC.ProcessChecks();

            if (RunResult == true)
            {

                NewRow = QA.mCalculatedTestSummary.NewRow();
                NewRow["TEST_SUM_ID"] = TestSummaryId;

                string CalcTestResCdParameter = (string)QA.GetCheckParameter("OOC_Test_Calc_Result").ParameterValue;
                if (CalcTestResCdParameter == "FAILED" || CalcTestResCdParameter == "ABORTED")
                    NewRow["CALC_TEST_RESULT_CD"] = CalcTestResCdParameter;
                else
                    if (QA.CheckEngine.SeverityCd == eSeverityCd.FATAL || QA.CheckEngine.SeverityCd == eSeverityCd.CRIT1)
                    NewRow["CALC_TEST_RESULT_CD"] = null;
                else
                        if (QA.CheckEngine.SeverityCd == eSeverityCd.CRIT2)
                    NewRow["CALC_TEST_RESULT_CD"] = "INVALID";
                else
                    NewRow["CALC_TEST_RESULT_CD"] = CalcTestResCdParameter;

                NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;
                QA.mCalculatedTestSummary.Rows.Add(NewRow);

                NewRow = QA.mCalculatedOOC.NewRow();
                NewRow["ON_OFF_CAL_ID"] = OOCId;
                if (QA.GetCheckParameter("OOC_Online_Zero_Injection_Calc_APS_Indicator").ParameterValue != null)
                    NewRow["CALC_ONLINE_ZERO_APS_IND"] = cDBConvert.ToString((int)QA.GetCheckParameter("OOC_Online_Zero_Injection_Calc_APS_Indicator").ParameterValue);
                if (QA.GetCheckParameter("OOC_Online_Zero_Injection_Calc_Result").ParameterValue != null)
                    NewRow["CALC_ONLINE_ZERO_CAL_ERROR"] = cDBConvert.ToString((Decimal)QA.GetCheckParameter("OOC_Online_Zero_Injection_Calc_Result").ParameterValue);
                if (QA.GetCheckParameter("OOC_Online_Upscale_Injection_Calc_APS_Indicator").ParameterValue != null)
                    NewRow["CALC_ONLINE_UPSCALE_APS_IND"] = cDBConvert.ToString((int)QA.GetCheckParameter("OOC_Online_Upscale_Injection_Calc_APS_Indicator").ParameterValue);
                if (QA.GetCheckParameter("OOC_Online_Upscale_Injection_Calc_Result").ParameterValue != null)
                    NewRow["CALC_ONLINE_UPSCALE_CAL_ERROR"] = cDBConvert.ToString((Decimal)QA.GetCheckParameter("OOC_Online_Upscale_Injection_Calc_Result").ParameterValue);
                if (QA.GetCheckParameter("OOC_Offline_Zero_Injection_Calc_APS_Indicator").ParameterValue != null)
                    NewRow["CALC_OFFLINE_ZERO_APS_IND"] = cDBConvert.ToString((int)QA.GetCheckParameter("OOC_Offline_Zero_Injection_Calc_APS_Indicator").ParameterValue);
                if (QA.GetCheckParameter("OOC_Offline_Zero_Injection_Calc_Result").ParameterValue != null)
                    NewRow["CALC_OFFLINE_ZERO_CAL_ERROR"] = cDBConvert.ToString((Decimal)QA.GetCheckParameter("OOC_Offline_Zero_Injection_Calc_Result").ParameterValue);
                if (QA.GetCheckParameter("OOC_Offline_Upscale_Injection_Calc_APS_Indicator").ParameterValue != null)
                    NewRow["CALC_OFFLINE_UPSCALE_APS_IND"] = cDBConvert.ToString((int)QA.GetCheckParameter("OOC_Offline_Upscale_Injection_Calc_APS_Indicator").ParameterValue);
                if (QA.GetCheckParameter("OOC_Offline_Upscale_Injection_Calc_Result").ParameterValue != null)
                    NewRow["CALC_OFFLINE_UPSCALE_CAL_ERROR"] = cDBConvert.ToString((Decimal)QA.GetCheckParameter("OOC_Offline_Upscale_Injection_Calc_Result").ParameterValue);
                NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;
                QA.mCalculatedOOC.Rows.Add(NewRow);

                if (QA.CheckEngine.SeverityCd != eSeverityCd.FATAL)
                {
                    NewRow = QA.mQASupp.NewRow();

                    if (CalcTestResCdParameter == "FAILED" || CalcTestResCdParameter == "ABORTED")
                        NewRow["TEST_RESULT_CD"] = CalcTestResCdParameter;
                    else
                        if (QA.CheckEngine.SeverityCd == eSeverityCd.CRIT1)
                        NewRow["TEST_RESULT_CD"] = null;
                    else
                            if (QA.CheckEngine.SeverityCd == eSeverityCd.CRIT2)
                        NewRow["TEST_RESULT_CD"] = "INVALID";
                    else
                        NewRow["TEST_RESULT_CD"] = CalcTestResCdParameter;

                    NewRow["TEST_SUM_ID"] = TestSummaryId;
                    NewRow["TEST_TYPE_CD"] = "ONOFF";
                    NewRow["MON_LOC_ID"] = (string)drTest["mon_loc_id"];
                    NewRow["COMPONENT_ID"] = (string)drTest["component_id"];
                    if (drTest["test_reason_cd"] != DBNull.Value)
                        NewRow["TEST_REASON_CD"] = (string)drTest["test_reason_cd"];
                    NewRow["TEST_NUM"] = (string)drTest["test_num"];
                    if (drTest["span_scale_cd"] != DBNull.Value)
                        NewRow["SPAN_SCALE"] = (string)drTest["span_scale_cd"];
                    if (drTest["begin_date"] != DBNull.Value)
                        NewRow["BEGIN_DATE"] = cDBConvert.ToString((DateTime)drTest["begin_date"]);
                    if (drTest["begin_hour"] != DBNull.Value)
                        NewRow["BEGIN_HOUR"] = cDBConvert.ToString((Decimal)drTest["begin_hour"]);
                    if (drTest["begin_min"] != DBNull.Value)
                        NewRow["BEGIN_MIN"] = cDBConvert.ToString((Decimal)drTest["begin_min"]);
                    if (drTest["end_date"] != DBNull.Value)
                        NewRow["END_DATE"] = cDBConvert.ToString((DateTime)drTest["end_date"]);
                    if (drTest["end_hour"] != DBNull.Value)
                        NewRow["END_HOUR"] = cDBConvert.ToString((Decimal)drTest["end_hour"]);
                    if (drTest["end_min"] != DBNull.Value)
                        NewRow["END_MIN"] = cDBConvert.ToString((Decimal)drTest["end_min"]);
                    if (cDBConvert.ToInteger(drTest["gp_ind"]) == 1)
                        NewRow["GP_IND"] = "1";

                    NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;
                    QA.mQASupp.Rows.Add(NewRow);
                }
            }

            OOC.EraseParameters();

            return RunResult;

        }

        public bool EvaluateFlowLoadReference(DataRow drTest, cCheckEngine mCheckEngine, cQAMain QA)
        {

            string MonitorLocationId, TestSummaryId, FlowLoadReferenceId;
            Boolean RunResult = true;

            MonitorLocationId = (string)drTest["mon_loc_id"];
            TestSummaryId = (string)drTest["test_sum_id"];
            QA.mSourceData.Tables["QAFlowLoadReference"].DefaultView.RowFilter = "test_sum_id = '" + TestSummaryId + "'";
            DataRowView drFlowLoadReference = (DataRowView)QA.mSourceData.Tables["QAFlowLoadReference"].DefaultView[0];
            FlowLoadReferenceId = Convert.ToString(drFlowLoadReference["flow_load_ref_id"]);
            string OpLevelCd = cDBConvert.ToString(drFlowLoadReference["op_level_cd"]);
            QA.mSourceData.Tables["QAFlowLoadReference"].DefaultView.RowFilter = "";


            DataRow NewRow;

            cFlowLoadReference FlowLoadReference;
            cCheckParameterBands FlowLoadReferenceChecks = GetCheckBands("F2LREF");

            FlowLoadReference = new cFlowLoadReference(mCheckEngine, QA, MonitorLocationId, TestSummaryId);
            FlowLoadReference.SetCheckBands(FlowLoadReferenceChecks);

            RunResult = FlowLoadReference.ProcessChecks();

            if (RunResult == true)
            {

                NewRow = QA.mCalculatedTestSummary.NewRow();
                NewRow["TEST_SUM_ID"] = TestSummaryId;
                NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;
                QA.mCalculatedTestSummary.Rows.Add(NewRow);

                NewRow = QA.mCalculatedFlowToLoadReference.NewRow();
                NewRow["FLOW_LOAD_REF_ID"] = FlowLoadReferenceId;
                if (QA.GetCheckParameter("Flow_to_Load_Reference_Calc_Average_Gross_Unit_Load").ParameterValue != null)
                    NewRow["CALC_AVG_GROSS_UNIT_LOAD"] = cDBConvert.ToString((Decimal)QA.GetCheckParameter("Flow_to_Load_Reference_Calc_Average_Gross_Unit_Load").ParameterValue);
                if (QA.GetCheckParameter("Flow_to_Load_Reference_Calc_Average_Reference_Method_Flow").ParameterValue != null)
                    NewRow["CALC_AVG_REF_METHOD_FLOW"] = cDBConvert.ToString((Decimal)QA.GetCheckParameter("Flow_to_Load_Reference_Calc_Average_Reference_Method_Flow").ParameterValue);
                if (QA.GetCheckParameter("Flow_to_Load_Reference_Calc_Flow_To_Load_Ratio").ParameterValue != null)
                    NewRow["CALC_REF_FLOW_LOAD_RATIO"] = cDBConvert.ToString((Decimal)QA.GetCheckParameter("Flow_to_Load_Reference_Calc_Flow_To_Load_Ratio").ParameterValue);
                if (QA.GetCheckParameter("Flow_to_Load_Reference_Calc_GHR").ParameterValue != null)
                    NewRow["CALC_REF_GHR"] = cDBConvert.ToString((Decimal)QA.GetCheckParameter("Flow_to_Load_Reference_Calc_GHR").ParameterValue);
                NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;
                QA.mCalculatedFlowToLoadReference.Rows.Add(NewRow);

                if (QA.CheckEngine.SeverityCd != eSeverityCd.FATAL)
                {
                    NewRow = QA.mQASupp.NewRow();

                    NewRow["TEST_SUM_ID"] = TestSummaryId;
                    NewRow["TEST_TYPE_CD"] = "F2LREF";
                    NewRow["MON_LOC_ID"] = (string)drTest["mon_loc_id"];
                    NewRow["MON_SYS_ID"] = (string)drTest["mon_sys_id"];
                    NewRow["TEST_NUM"] = (string)drTest["test_num"];
                    if (drTest["end_date"] != DBNull.Value)
                        NewRow["END_DATE"] = cDBConvert.ToString((DateTime)drTest["end_date"]);
                    if (drTest["end_hour"] != DBNull.Value)
                        NewRow["END_HOUR"] = cDBConvert.ToString((Decimal)drTest["end_hour"]);
                    if (drTest["end_min"] != DBNull.Value)
                        NewRow["END_MIN"] = cDBConvert.ToString((Decimal)drTest["end_min"]);
                    if (OpLevelCd != "")
                        NewRow["OP_LEVEL_CD"] = OpLevelCd;
                    NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;
                    QA.mQASupp.Rows.Add(NewRow);

                    if (QA.GetCheckParameter("Flow_to_Load_Reference_Calc_Average_Gross_Unit_Load").ParameterValue != null)
                    {
                        NewRow = QA.mQASuppAttribute.NewRow();
                        NewRow["TEST_SUM_ID"] = TestSummaryId;
                        NewRow["ATTRIBUTE_NAME"] = "AVG_GROSS_UNIT_LOAD";
                        NewRow["ATTRIBUTE_VALUE"] = cDBConvert.ToString((decimal)QA.GetCheckParameter("Flow_to_Load_Reference_Calc_Average_Gross_Unit_Load").ParameterValue);
                        NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;
                        QA.mQASuppAttribute.Rows.Add(NewRow);
                    }

                    if (QA.GetCheckParameter("Flow_to_Load_Reference_Calc_Flow_To_Load_Ratio").ParameterValue != null)
                    {
                        NewRow = QA.mQASuppAttribute.NewRow();
                        NewRow["TEST_SUM_ID"] = TestSummaryId;
                        NewRow["ATTRIBUTE_NAME"] = "REF_FLOW_LOAD_RATIO";
                        NewRow["ATTRIBUTE_VALUE"] = cDBConvert.ToString((decimal)QA.GetCheckParameter("Flow_to_Load_Reference_Calc_Flow_To_Load_Ratio").ParameterValue);
                        NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;
                        QA.mQASuppAttribute.Rows.Add(NewRow);
                    }

                    if (QA.GetCheckParameter("Flow_to_Load_Reference_Calc_GHR").ParameterValue != null)
                    {
                        NewRow = QA.mQASuppAttribute.NewRow();
                        NewRow["TEST_SUM_ID"] = TestSummaryId;
                        NewRow["ATTRIBUTE_NAME"] = "REF_GHR";
                        NewRow["ATTRIBUTE_VALUE"] = cDBConvert.ToString((decimal)QA.GetCheckParameter("Flow_to_Load_Reference_Calc_GHR").ParameterValue);
                        NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;
                        QA.mQASuppAttribute.Rows.Add(NewRow);
                    }
                }
            }

            FlowLoadReference.EraseParameters();

            return RunResult;

        }

        public bool Evaluate7Day(DataRow drTest, cCheckEngine mCheckEngine, cQAMain QA)
        {

            string CalibrationInjectionId, MonitorLocationId, TestSummaryId;
            Boolean SkipCategory, RunResult = true;

            MonitorLocationId = (string)drTest["mon_loc_id"];
            TestSummaryId = (string)drTest["test_sum_id"];
            SkipCategory = false;

            DataRow NewRow;

            cCalibration Calibration;
            cCheckParameterBands CalibrationChecks = GetCheckBands("7DAY");

            Calibration = new cCalibration(mCheckEngine, QA, MonitorLocationId, TestSummaryId);
            Calibration.SetCheckBands(CalibrationChecks);

            RunResult = Calibration.ProcessChecks();

            if (RunResult == true)
            {

                if ((Boolean)QA.GetCheckParameter("Calibration_Test_Component_Type_Valid").ParameterValue == false)
                    SkipCategory = true;
                else
                {
                    if ((Boolean)QA.GetCheckParameter("Calibration_Test_Aborted").ParameterValue == true)
                        SkipCategory = true;
                }

                if (SkipCategory)
                {

                    QA.mSourceData.Tables["QACalibrationInjection"].DefaultView.RowFilter = "test_sum_id = '" + TestSummaryId + "'";

                    foreach (DataRowView drCalibrationInjection in QA.mSourceData.Tables["QACalibrationInjection"].DefaultView)
                    {
                        NewRow = QA.mCalculatedCalibrationInjection.NewRow();
                        NewRow["CAL_INJ_ID"] = cDBConvert.ToString(drCalibrationInjection["cal_inj_id"]);
                        NewRow["CALC_ZERO_APS_IND"] = null;
                        NewRow["CALC_ZERO_CAL_ERROR"] = null;
                        NewRow["CALC_UPSCALE_APS_IND"] = null;
                        NewRow["CALC_UPSCALE_CAL_ERROR"] = null;
                        NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;
                        QA.mCalculatedCalibrationInjection.Rows.Add(NewRow);
                    }
                }
                else
                {

                    cCalibrationInjection CalibrationInjection;
                    cCheckParameterBands CalibrationInjectionChecks = GetCheckBands("7DAYINJ");

                    QA.mSourceData.Tables["QACalibrationInjection"].DefaultView.RowFilter = "test_sum_id = '" + TestSummaryId + "'";
                    QA.mSourceData.Tables["QACalibrationInjection"].DefaultView.Sort = "zero_injection_date";

                    foreach (DataRowView drCalibrationInjection in QA.mSourceData.Tables["QACalibrationInjection"].DefaultView)
                    {

                        SkipCategory = false;

                        CalibrationInjectionId = cDBConvert.ToString(drCalibrationInjection["cal_inj_id"]);

                        CalibrationInjection = new cCalibrationInjection(mCheckEngine, QA, MonitorLocationId, CalibrationInjectionId, Calibration);
                        CalibrationInjection.SetCheckBands(CalibrationInjectionChecks);

                        RunResult = CalibrationInjection.ProcessChecks();

                        if (RunResult == true)
                        {

                            NewRow = QA.mCalculatedCalibrationInjection.NewRow();
                            NewRow["CAL_INJ_ID"] = CalibrationInjectionId;
                            if (QA.GetCheckParameter("Calibration_Zero_Injection_Calc_APS_Indicator").ParameterValue != null)
                                NewRow["CALC_ZERO_APS_IND"] = cDBConvert.ToString((int)QA.GetCheckParameter("Calibration_Zero_Injection_Calc_APS_Indicator").ParameterValue);
                            if (QA.GetCheckParameter("Calibration_Zero_Injection_Calc_Result").ParameterValue != null)
                                NewRow["CALC_ZERO_CAL_ERROR"] = cDBConvert.ToString((Decimal)QA.GetCheckParameter("Calibration_Zero_Injection_Calc_Result").ParameterValue);
                            if (QA.GetCheckParameter("Calibration_Upscale_Injection_Calc_APS_Indicator").ParameterValue != null)
                                NewRow["CALC_UPSCALE_APS_IND"] = cDBConvert.ToString((int)QA.GetCheckParameter("Calibration_Upscale_Injection_Calc_APS_Indicator").ParameterValue);
                            if (QA.GetCheckParameter("Calibration_Upscale_Injection_Calc_Result").ParameterValue != null)
                                NewRow["CALC_UPSCALE_CAL_ERROR"] = cDBConvert.ToString((Decimal)QA.GetCheckParameter("Calibration_Upscale_Injection_Calc_Result").ParameterValue);
                            NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;
                            QA.mCalculatedCalibrationInjection.Rows.Add(NewRow);
                        }

                        CalibrationInjection.EraseParameters();

                    }
                }

                if (RunResult == true)
                {
                    cCheckParameterBands CalibrationPass2Checks = GetCheckBands("CLC7DAY");

                    Calibration.SetCheckBands(CalibrationPass2Checks);

                    RunResult = Calibration.ProcessChecks();

                    if (RunResult == true)
                    {

                        NewRow = QA.mCalculatedTestSummary.NewRow();
                        NewRow["TEST_SUM_ID"] = TestSummaryId;

                        string CalcTestResCdParameter = (string)QA.GetCheckParameter("Calibration_Test_Result").ParameterValue;
                        if (CalcTestResCdParameter == "FAILED" || CalcTestResCdParameter == "ABORTED")
                            NewRow["CALC_TEST_RESULT_CD"] = CalcTestResCdParameter;
                        else
                            if (QA.CheckEngine.SeverityCd == eSeverityCd.FATAL || QA.CheckEngine.SeverityCd == eSeverityCd.CRIT1)
                            NewRow["CALC_TEST_RESULT_CD"] = null;
                        else
                                if (QA.CheckEngine.SeverityCd == eSeverityCd.CRIT2)
                            NewRow["CALC_TEST_RESULT_CD"] = "INVALID";
                        else
                            NewRow["CALC_TEST_RESULT_CD"] = CalcTestResCdParameter;

                        if (QA.GetCheckParameter("Test_Span_Value").ParameterValue != null)
                            NewRow["CALC_SPAN_VALUE"] = cDBConvert.ToString((Decimal)QA.GetCheckParameter("Test_Span_Value").ParameterValue);
                        NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;
                        QA.mCalculatedTestSummary.Rows.Add(NewRow);

                        if (QA.CheckEngine.SeverityCd != eSeverityCd.FATAL)
                        {
                            NewRow = QA.mQASupp.NewRow();

                            if (CalcTestResCdParameter == "FAILED" || CalcTestResCdParameter == "ABORTED")
                                NewRow["TEST_RESULT_CD"] = CalcTestResCdParameter;
                            else
                                if (QA.CheckEngine.SeverityCd == eSeverityCd.CRIT1)
                                NewRow["TEST_RESULT_CD"] = null;
                            else
                                    if (QA.CheckEngine.SeverityCd == eSeverityCd.CRIT2)
                                NewRow["TEST_RESULT_CD"] = "INVALID";
                            else
                                NewRow["TEST_RESULT_CD"] = CalcTestResCdParameter;

                            NewRow["TEST_SUM_ID"] = TestSummaryId;
                            NewRow["TEST_TYPE_CD"] = "7DAY";
                            NewRow["MON_LOC_ID"] = (string)drTest["mon_loc_id"];
                            NewRow["COMPONENT_ID"] = (string)drTest["component_id"];
                            if (drTest["test_reason_cd"] != DBNull.Value)
                                NewRow["TEST_REASON_CD"] = (string)drTest["test_reason_cd"];
                            NewRow["TEST_NUM"] = (string)drTest["test_num"];
                            if (drTest["span_scale_cd"] != DBNull.Value)
                                NewRow["SPAN_SCALE"] = (string)drTest["span_scale_cd"];
                            if (drTest["begin_date"] != DBNull.Value)
                                NewRow["BEGIN_DATE"] = cDBConvert.ToString((DateTime)drTest["begin_date"]);
                            if (drTest["begin_hour"] != DBNull.Value)
                                NewRow["BEGIN_HOUR"] = cDBConvert.ToString((Decimal)drTest["begin_hour"]);
                            if (drTest["begin_min"] != DBNull.Value)
                                NewRow["BEGIN_MIN"] = cDBConvert.ToString((Decimal)drTest["begin_min"]);
                            if (drTest["end_date"] != DBNull.Value)
                                NewRow["END_DATE"] = cDBConvert.ToString((DateTime)drTest["end_date"]);
                            if (drTest["end_hour"] != DBNull.Value)
                                NewRow["END_HOUR"] = cDBConvert.ToString((Decimal)drTest["end_hour"]);
                            if (drTest["end_min"] != DBNull.Value)
                                NewRow["END_MIN"] = cDBConvert.ToString((Decimal)drTest["end_min"]);
                            if (cDBConvert.ToInteger(drTest["gp_ind"]) == 1)
                                NewRow["GP_IND"] = "1";

                            NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;
                            QA.mQASupp.Rows.Add(NewRow);
                        }
                    }
                }
            }

            Calibration.EraseParameters();
            return RunResult;

        }

        public bool EvaluateCycle(DataRow drTest, cCheckEngine mCheckEngine, cQAMain QA)
        {

            string CycleTimeInjectionId, MonitorLocationId, TestSummaryId, CycleTimeSummaryId;
            Boolean SkipCategory, RunResult = true;

            MonitorLocationId = (string)drTest["mon_loc_id"];
            TestSummaryId = (string)drTest["test_sum_id"];
            QA.mSourceData.Tables["QACycle"].DefaultView.RowFilter = "test_sum_id = '" + TestSummaryId + "'";
            DataRowView drCycle = (DataRowView)QA.mSourceData.Tables["QACycle"].DefaultView[0];
            CycleTimeSummaryId = Convert.ToString(drCycle["cycle_time_sum_id"]);
            SkipCategory = false;

            DataRow NewRow;

            cCycleTime CycleTime;
            cCheckParameterBands CycleTimeChecks = GetCheckBands("CYCLE");

            CycleTime = new cCycleTime(mCheckEngine, QA, MonitorLocationId, TestSummaryId);
            CycleTime.SetCheckBands(CycleTimeChecks);

            RunResult = CycleTime.ProcessChecks();

            if (RunResult == true)
            {
                if ((Boolean)QA.GetCheckParameter("Cycle_Time_Test_Component_Type_Valid").ParameterValue == false)
                    SkipCategory = true;
                else
                    if ((Boolean)QA.GetCheckParameter("Cycle_Time_Test_Aborted").ParameterValue == true)
                    SkipCategory = true;
                if (SkipCategory)
                {

                    QA.mSourceData.Tables["QACycleTimeInjection"].DefaultView.RowFilter = "test_sum_id = '" + TestSummaryId + "'";

                    foreach (DataRowView drCycleTimeInjection in QA.mSourceData.Tables["QACycleTimeInjection"].DefaultView)
                    {
                        NewRow = QA.mCalculatedCycleTimeInjection.NewRow();
                        NewRow["CYCLE_TIME_INJ_ID"] = cDBConvert.ToString(drCycleTimeInjection["cycle_time_inj_id"]);
                        NewRow["CALC_INJECTION_CYCLE_TIME"] = null;
                        NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;
                        QA.mCalculatedCycleTimeInjection.Rows.Add(NewRow);
                    }
                }
                else
                {
                    cCycleTimeInjection CycleTimeInjection;
                    cCheckParameterBands CycleTimeInjectionChecks = GetCheckBands("CYCLINJ");

                    QA.mSourceData.Tables["QACycleTimeInjection"].DefaultView.RowFilter = "test_sum_id = '" + TestSummaryId + "'";

                    foreach (DataRowView drCycleTimeInjection in QA.mSourceData.Tables["QACycleTimeInjection"].DefaultView)
                    {

                        SkipCategory = false;

                        CycleTimeInjectionId = cDBConvert.ToString(drCycleTimeInjection["cycle_time_inj_id"]);

                        CycleTimeInjection = new cCycleTimeInjection(mCheckEngine, QA, MonitorLocationId, CycleTimeInjectionId, CycleTime);
                        CycleTimeInjection.SetCheckBands(CycleTimeInjectionChecks);

                        RunResult = CycleTimeInjection.ProcessChecks();

                        if (RunResult == true)
                        {
                            NewRow = QA.mCalculatedCycleTimeInjection.NewRow();
                            NewRow["CYCLE_TIME_INJ_ID"] = CycleTimeInjectionId;
                            if (QA.GetCheckParameter("Cycle_Time_Calc_Injection_Cycle_Time").ParameterValue != null)
                                NewRow["CALC_INJECTION_CYCLE_TIME"] = cDBConvert.ToString((int)QA.GetCheckParameter("Cycle_Time_Calc_Injection_Cycle_Time").ParameterValue);
                            NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;
                            QA.mCalculatedCycleTimeInjection.Rows.Add(NewRow);
                        }

                        CycleTimeInjection.EraseParameters();

                    }
                }

                if (RunResult == true)
                {
                    cCheckParameterBands CycleTimePass2Checks = GetCheckBands("CLCCYCL");

                    CycleTime.SetCheckBands(CycleTimePass2Checks);

                    RunResult = CycleTime.ProcessChecks();

                    if (RunResult == true)
                    {

                        NewRow = QA.mCalculatedTestSummary.NewRow();
                        NewRow["TEST_SUM_ID"] = TestSummaryId;

                        string CalcTestResCdParameter = (string)QA.GetCheckParameter("Cycle_Time_Calc_Test_Result").ParameterValue;
                        if (CalcTestResCdParameter == "FAILED" || CalcTestResCdParameter == "ABORTED")
                            NewRow["CALC_TEST_RESULT_CD"] = CalcTestResCdParameter;
                        else
                            if (QA.CheckEngine.SeverityCd == eSeverityCd.FATAL || QA.CheckEngine.SeverityCd == eSeverityCd.CRIT1)
                            NewRow["CALC_TEST_RESULT_CD"] = null;
                        else
                                if (QA.CheckEngine.SeverityCd == eSeverityCd.CRIT2)
                            NewRow["CALC_TEST_RESULT_CD"] = "INVALID";
                        else
                            NewRow["CALC_TEST_RESULT_CD"] = CalcTestResCdParameter;

                        if (QA.GetCheckParameter("Test_Span_Value").ParameterValue != null)
                            NewRow["CALC_SPAN_VALUE"] = cDBConvert.ToString((Decimal)QA.GetCheckParameter("Test_Span_Value").ParameterValue);
                        NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;
                        QA.mCalculatedTestSummary.Rows.Add(NewRow);

                        NewRow = QA.mCalculatedCycleTimeSummary.NewRow();
                        NewRow["CYCLE_TIME_SUM_ID"] = CycleTimeSummaryId;
                        if (QA.GetCheckParameter("Cycle_Time_Calc_Total_Cycle_Time").ParameterValue != null)
                            NewRow["CALC_TOTAL_TIME"] = cDBConvert.ToString((int)QA.GetCheckParameter("Cycle_Time_Calc_Total_Cycle_Time").ParameterValue);
                        NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;
                        QA.mCalculatedCycleTimeSummary.Rows.Add(NewRow);

                        if (QA.CheckEngine.SeverityCd != eSeverityCd.FATAL)
                        {
                            NewRow = QA.mQASupp.NewRow();

                            if (CalcTestResCdParameter == "FAILED" || CalcTestResCdParameter == "ABORTED")
                                NewRow["TEST_RESULT_CD"] = CalcTestResCdParameter;
                            else
                                if (QA.CheckEngine.SeverityCd == eSeverityCd.CRIT1)
                                NewRow["TEST_RESULT_CD"] = null;
                            else
                                    if (QA.CheckEngine.SeverityCd == eSeverityCd.CRIT2)
                                NewRow["TEST_RESULT_CD"] = "INVALID";
                            else
                                NewRow["TEST_RESULT_CD"] = CalcTestResCdParameter;

                            NewRow["TEST_SUM_ID"] = TestSummaryId;
                            NewRow["TEST_TYPE_CD"] = "CYCLE";
                            NewRow["MON_LOC_ID"] = (string)drTest["mon_loc_id"];
                            NewRow["COMPONENT_ID"] = (string)drTest["component_id"];
                            if (drTest["test_reason_cd"] != DBNull.Value)
                                NewRow["TEST_REASON_CD"] = (string)drTest["test_reason_cd"];
                            NewRow["TEST_NUM"] = (string)drTest["test_num"];
                            if (drTest["span_scale_cd"] != DBNull.Value)
                                NewRow["SPAN_SCALE"] = (string)drTest["span_scale_cd"];
                            if (drTest["begin_date"] != DBNull.Value)
                                NewRow["BEGIN_DATE"] = cDBConvert.ToString((DateTime)drTest["begin_date"]);
                            if (drTest["begin_hour"] != DBNull.Value)
                                NewRow["BEGIN_HOUR"] = cDBConvert.ToString((Decimal)drTest["begin_hour"]);
                            if (drTest["begin_min"] != DBNull.Value)
                                NewRow["BEGIN_MIN"] = cDBConvert.ToString((Decimal)drTest["begin_min"]);
                            if (drTest["end_date"] != DBNull.Value)
                                NewRow["END_DATE"] = cDBConvert.ToString((DateTime)drTest["end_date"]);
                            if (drTest["end_hour"] != DBNull.Value)
                                NewRow["END_HOUR"] = cDBConvert.ToString((Decimal)drTest["end_hour"]);
                            if (drTest["end_min"] != DBNull.Value)
                                NewRow["END_MIN"] = cDBConvert.ToString((Decimal)drTest["end_min"]);
                            if (cDBConvert.ToInteger(drTest["gp_ind"]) == 1)
                                NewRow["GP_IND"] = "1";

                            NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;
                            QA.mQASupp.Rows.Add(NewRow);
                        }
                    }
                }
            }

            CycleTime.EraseParameters();
            return RunResult;

        }

        private bool EvaluateFFACC(DataRow drTest, cCheckEngine mCheckEngine, cQAMain QA)
        {
            string MonitorLocationId, TestSummaryId, FFACCId;
            Boolean RunResult = true;

            MonitorLocationId = (string)drTest["mon_loc_id"];
            TestSummaryId = (string)drTest["test_sum_id"];
            QA.mSourceData.Tables["QAAccuracyTest"].DefaultView.RowFilter = "test_sum_id = '" + TestSummaryId + "'";
            DataRowView drFFACC = (DataRowView)QA.mSourceData.Tables["QAAccuracyTest"].DefaultView[0];
            FFACCId = Convert.ToString(drFFACC["fuel_flow_acc_id"]);
            QA.mSourceData.Tables["QAAccuracyTest"].DefaultView.RowFilter = "";


            DataRow NewRow;

            cFFACC FFACC;
            cCheckParameterBands FFACC_Checks = GetCheckBands("FFACC");

            FFACC = new cFFACC(mCheckEngine, QA, MonitorLocationId, TestSummaryId);
            FFACC.SetCheckBands(FFACC_Checks);

            RunResult = FFACC.ProcessChecks();

            if (RunResult == true)
            {

                NewRow = QA.mCalculatedTestSummary.NewRow();
                NewRow["TEST_SUM_ID"] = TestSummaryId;

                string CalcTestResCdParameter = (string)QA.GetCheckParameter("Accuracy_Test_Calc_Result").ParameterValue;
                if (CalcTestResCdParameter == "FAILED" || CalcTestResCdParameter == "ABORTED")
                    NewRow["CALC_TEST_RESULT_CD"] = CalcTestResCdParameter;
                else
                    if (QA.CheckEngine.SeverityCd == eSeverityCd.FATAL || QA.CheckEngine.SeverityCd == eSeverityCd.CRIT1)
                    NewRow["CALC_TEST_RESULT_CD"] = null;
                else
                        if (QA.CheckEngine.SeverityCd == eSeverityCd.CRIT2)
                    NewRow["CALC_TEST_RESULT_CD"] = "INVALID";
                else
                    NewRow["CALC_TEST_RESULT_CD"] = CalcTestResCdParameter;

                if (QA.GetCheckParameter("Test_Span_Value").ParameterValue != null)
                    NewRow["CALC_SPAN_VALUE"] = cDBConvert.ToString((Decimal)QA.GetCheckParameter("Test_Span_Value").ParameterValue);
                NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;
                QA.mCalculatedTestSummary.Rows.Add(NewRow);

                if (QA.CheckEngine.SeverityCd != eSeverityCd.FATAL)
                {
                    NewRow = QA.mQASupp.NewRow();

                    if (CalcTestResCdParameter == "FAILED" || CalcTestResCdParameter == "ABORTED")
                        NewRow["TEST_RESULT_CD"] = CalcTestResCdParameter;
                    else
                        if (QA.CheckEngine.SeverityCd == eSeverityCd.CRIT1)
                        NewRow["TEST_RESULT_CD"] = null;
                    else
                            if (QA.CheckEngine.SeverityCd == eSeverityCd.CRIT2)
                        NewRow["TEST_RESULT_CD"] = "INVALID";
                    else
                        NewRow["TEST_RESULT_CD"] = CalcTestResCdParameter;

                    NewRow["TEST_SUM_ID"] = TestSummaryId;
                    NewRow["TEST_TYPE_CD"] = "FFACC";
                    NewRow["MON_LOC_ID"] = (string)drTest["mon_loc_id"];
                    if (drTest["component_id"] != DBNull.Value)
                        NewRow["COMPONENT_ID"] = (string)drTest["component_id"];
                    if (drTest["test_reason_cd"] != DBNull.Value)
                        NewRow["TEST_REASON_CD"] = (string)drTest["test_reason_cd"];
                    if (drTest["test_num"] != DBNull.Value)
                        NewRow["TEST_NUM"] = (string)drTest["test_num"];
                    if (drTest["end_date"] != DBNull.Value)
                        NewRow["END_DATE"] = cDBConvert.ToString((DateTime)drTest["end_date"]);
                    if (drTest["end_hour"] != DBNull.Value)
                        NewRow["END_HOUR"] = cDBConvert.ToString((Decimal)drTest["end_hour"]);
                    if (drTest["end_min"] != DBNull.Value)
                        NewRow["END_MIN"] = cDBConvert.ToString((Decimal)drTest["end_min"]);
                    if (drTest["reinstall_date"] != DBNull.Value)
                        NewRow["REINSTALLATION_DATE"] = cDBConvert.ToString((DateTime)drTest["reinstall_date"]);
                    if (drTest["reinstall_hour"] != DBNull.Value)
                        NewRow["REINSTALLATION_HOUR"] = cDBConvert.ToString((Decimal)drTest["reinstall_hour"]);

                    NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;
                    QA.mQASupp.Rows.Add(NewRow);
                }
            }

            FFACC.EraseParameters();

            return RunResult;

        }

        private bool EvaluateFFACCTT(DataRow drTest, cCheckEngine mCheckEngine, cQAMain QA)
        {
            string MonitorLocationId, TestSummaryId, FFACCTTId;
            Boolean RunResult = true;

            MonitorLocationId = (string)drTest["mon_loc_id"];
            TestSummaryId = (string)drTest["test_sum_id"];
            QA.mSourceData.Tables["QATransmitterTransducerTest"].DefaultView.RowFilter = "test_sum_id = '" + TestSummaryId + "'";
            DataRowView drFFACCTT = (DataRowView)QA.mSourceData.Tables["QATransmitterTransducerTest"].DefaultView[0];
            FFACCTTId = cDBConvert.ToString(drFFACCTT["trans_ac_id"]);
            QA.mSourceData.Tables["QATransmitterTransducerTest"].DefaultView.RowFilter = "";


            DataRow NewRow;

            cFFACCTT FFACCTT;
            cCheckParameterBands FFACCTTChecks = GetCheckBands("FFACCTT");

            FFACCTT = new cFFACCTT(mCheckEngine, QA, MonitorLocationId, TestSummaryId);
            FFACCTT.SetCheckBands(FFACCTTChecks);

            RunResult = FFACCTT.ProcessChecks();

            if (RunResult == true)
            {

                NewRow = QA.mCalculatedTestSummary.NewRow();
                NewRow["TEST_SUM_ID"] = TestSummaryId;

                string CalcTestResCdParameter = (string)QA.GetCheckParameter("Transmitter_Transducer_Test_Calc_Result").ParameterValue;
                if (CalcTestResCdParameter == "FAILED" || CalcTestResCdParameter == "ABORTED")
                    NewRow["CALC_TEST_RESULT_CD"] = CalcTestResCdParameter;
                else
                    if (QA.CheckEngine.SeverityCd == eSeverityCd.FATAL || QA.CheckEngine.SeverityCd == eSeverityCd.CRIT1)
                    NewRow["CALC_TEST_RESULT_CD"] = null;
                else
                        if (QA.CheckEngine.SeverityCd == eSeverityCd.CRIT2)
                    NewRow["CALC_TEST_RESULT_CD"] = "INVALID";
                else
                    NewRow["CALC_TEST_RESULT_CD"] = CalcTestResCdParameter;

                NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;
                QA.mCalculatedTestSummary.Rows.Add(NewRow);

                if (QA.CheckEngine.SeverityCd != eSeverityCd.FATAL)
                {
                    NewRow = QA.mQASupp.NewRow();

                    if (CalcTestResCdParameter == "FAILED" || CalcTestResCdParameter == "ABORTED")
                        NewRow["TEST_RESULT_CD"] = CalcTestResCdParameter;
                    else
                        if (QA.CheckEngine.SeverityCd == eSeverityCd.CRIT1)
                        NewRow["TEST_RESULT_CD"] = null;
                    else
                            if (QA.CheckEngine.SeverityCd == eSeverityCd.CRIT2)
                        NewRow["TEST_RESULT_CD"] = "INVALID";
                    else
                        NewRow["TEST_RESULT_CD"] = CalcTestResCdParameter;

                    NewRow["TEST_SUM_ID"] = TestSummaryId;
                    NewRow["TEST_TYPE_CD"] = "FFACCTT";
                    NewRow["MON_LOC_ID"] = (string)drTest["mon_loc_id"];
                    if (drTest["component_id"] != DBNull.Value)
                        NewRow["COMPONENT_ID"] = (string)drTest["component_id"];
                    if (drTest["test_reason_cd"] != DBNull.Value)
                        NewRow["TEST_REASON_CD"] = (string)drTest["test_reason_cd"];
                    if (drTest["test_num"] != DBNull.Value)
                        NewRow["TEST_NUM"] = (string)drTest["test_num"];
                    if (drTest["end_date"] != DBNull.Value)
                        NewRow["END_DATE"] = cDBConvert.ToString((DateTime)drTest["end_date"]);
                    if (drTest["end_hour"] != DBNull.Value)
                        NewRow["END_HOUR"] = cDBConvert.ToString((Decimal)drTest["end_hour"]);
                    if (drTest["end_min"] != DBNull.Value)
                        NewRow["END_MIN"] = cDBConvert.ToString((Decimal)drTest["end_min"]);

                    NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;
                    QA.mQASupp.Rows.Add(NewRow);
                }
            }

            FFACCTT.EraseParameters();

            return RunResult;

        }

        private bool EvaluateFF2LBAS(DataRow drTest, cCheckEngine mCheckEngine, cQAMain QA)
        {
            string MonitorLocationId, TestSummaryId, FF2LBASId;
            Boolean RunResult = true;

            MonitorLocationId = (string)drTest["mon_loc_id"];
            TestSummaryId = (string)drTest["test_sum_id"];
            QA.mSourceData.Tables["QAFuelFlowToLoadBaselineTest"].DefaultView.RowFilter = "test_sum_id = '" + TestSummaryId + "'";
            DataRowView drFF2LBAS = (DataRowView)QA.mSourceData.Tables["QAFuelFlowToLoadBaselineTest"].DefaultView[0];
            FF2LBASId = cDBConvert.ToString(drFF2LBAS["FUEL_FLOW_BASELINE_ID"]);
            QA.mSourceData.Tables["QAFuelFlowToLoadBaselineTest"].DefaultView.RowFilter = "";

            DataRow NewRow;

            cFF2LBAS FF2LBAS;
            cCheckParameterBands FF2LBASChecks = GetCheckBands("FF2LBAS");

            FF2LBAS = new cFF2LBAS(mCheckEngine, QA, MonitorLocationId, TestSummaryId);
            FF2LBAS.SetCheckBands(FF2LBASChecks);

            RunResult = FF2LBAS.ProcessChecks();

            if (RunResult == true)
            {

                NewRow = QA.mCalculatedTestSummary.NewRow();
                NewRow["TEST_SUM_ID"] = TestSummaryId;
                NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;
                QA.mCalculatedTestSummary.Rows.Add(NewRow);

                if (QA.CheckEngine.SeverityCd != eSeverityCd.FATAL)
                {
                    NewRow = QA.mQASupp.NewRow();

                    NewRow["TEST_SUM_ID"] = TestSummaryId;
                    NewRow["TEST_TYPE_CD"] = "FF2LBAS";
                    NewRow["MON_LOC_ID"] = (string)drTest["mon_loc_id"];
                    if (drTest["mon_sys_id"] != DBNull.Value)
                        NewRow["MON_SYS_ID"] = (string)drTest["mon_sys_id"];
                    if (drTest["test_num"] != DBNull.Value)
                        NewRow["TEST_NUM"] = (string)drTest["test_num"];
                    if (drTest["begin_date"] != DBNull.Value)
                        NewRow["BEGIN_DATE"] = cDBConvert.ToString((DateTime)drTest["begin_date"]);
                    if (drTest["begin_hour"] != DBNull.Value)
                        NewRow["BEGIN_HOUR"] = cDBConvert.ToString((Decimal)drTest["begin_hour"]);
                    if (drTest["end_date"] != DBNull.Value)
                        NewRow["END_DATE"] = cDBConvert.ToString((DateTime)drTest["end_date"]);
                    if (drTest["end_hour"] != DBNull.Value)
                        NewRow["END_HOUR"] = cDBConvert.ToString((Decimal)drTest["end_hour"]);
                    NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;
                    QA.mQASupp.Rows.Add(NewRow);

                    if (QA.GetCheckParameter("FF2LBAS_Test_Basis").ParameterValue != null)
                    {
                        NewRow = QA.mQASuppAttribute.NewRow();
                        NewRow["TEST_SUM_ID"] = TestSummaryId;
                        NewRow["ATTRIBUTE_NAME"] = "TEST_BASIS_CD";
                        NewRow["ATTRIBUTE_VALUE"] = (string)QA.GetCheckParameter("FF2LBAS_Test_Basis").ParameterValue;
                        NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;
                        QA.mQASuppAttribute.Rows.Add(NewRow);
                    }

                }
            }

            FF2LBAS.EraseParameters();

            return RunResult;
        }

        private bool EvaluateFF2LTST(DataRow drTest, cCheckEngine mCheckEngine, cQAMain QA)
        {
            string MonitorLocationId, TestSummaryId, FF2LTSTId;
            Boolean RunResult = true;

            MonitorLocationId = (string)drTest["mon_loc_id"];
            TestSummaryId = (string)drTest["test_sum_id"];
            QA.mSourceData.Tables["QAFuelFlowToLoadTest"].DefaultView.RowFilter = "test_sum_id = '" + TestSummaryId + "'";
            DataRowView drFF2LTST = (DataRowView)QA.mSourceData.Tables["QAFuelFlowToLoadTest"].DefaultView[0];
            FF2LTSTId = cDBConvert.ToString(drFF2LTST["FUEL_FLOW_LOAD_ID"]);
            QA.mSourceData.Tables["QAFuelFlowToLoadTest"].DefaultView.RowFilter = "";

            DataRow NewRow;

            cFF2LTST FF2LTST;
            cCheckParameterBands FF2LTSTChecks = GetCheckBands("FF2LTST");

            FF2LTST = new cFF2LTST(mCheckEngine, QA, MonitorLocationId, TestSummaryId);
            FF2LTST.SetCheckBands(FF2LTSTChecks);

            RunResult = FF2LTST.ProcessChecks();

            if (RunResult == true)
            {

                NewRow = QA.mCalculatedTestSummary.NewRow();
                NewRow["TEST_SUM_ID"] = TestSummaryId;
                NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;
                QA.mCalculatedTestSummary.Rows.Add(NewRow);

                if (QA.CheckEngine.SeverityCd != eSeverityCd.FATAL)
                {
                    NewRow = QA.mQASupp.NewRow();

                    NewRow["TEST_SUM_ID"] = TestSummaryId;
                    NewRow["TEST_TYPE_CD"] = "FF2LTST";

                    string CalcTestResCdParameter = cDBConvert.ToString(drTest["test_result_cd"]);
                    if (CalcTestResCdParameter == "FAILED" || CalcTestResCdParameter == "ABORTED")
                        NewRow["TEST_RESULT_CD"] = CalcTestResCdParameter;
                    else
                        if (QA.CheckEngine.SeverityCd == eSeverityCd.CRIT1)
                        NewRow["TEST_RESULT_CD"] = null;
                    else
                            if (QA.CheckEngine.SeverityCd == eSeverityCd.CRIT2)
                        NewRow["TEST_RESULT_CD"] = "INVALID";
                    else
                        NewRow["TEST_RESULT_CD"] = CalcTestResCdParameter;

                    NewRow["MON_LOC_ID"] = (string)drTest["mon_loc_id"];
                    if (drTest["mon_sys_id"] != DBNull.Value)
                        NewRow["MON_SYS_ID"] = (string)drTest["mon_sys_id"];
                    if (drTest["test_num"] != DBNull.Value)
                        NewRow["TEST_NUM"] = (string)drTest["test_num"];
                    if (drTest["begin_date"] != DBNull.Value)
                        NewRow["BEGIN_DATE"] = cDBConvert.ToString((DateTime)drTest["begin_date"]);
                    if (drTest["begin_hour"] != DBNull.Value)
                        NewRow["BEGIN_HOUR"] = cDBConvert.ToString((Decimal)drTest["begin_hour"]);
                    if (drTest["end_date"] != DBNull.Value)
                        NewRow["END_DATE"] = cDBConvert.ToString((DateTime)drTest["end_date"]);
                    if (drTest["end_hour"] != DBNull.Value)
                        NewRow["END_HOUR"] = cDBConvert.ToString((Decimal)drTest["end_hour"]);
                    if (drTest["rpt_period_id"] != DBNull.Value)
                        NewRow["RPT_PERIOD_ID"] = (int)drTest["rpt_period_id"];
                    NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;
                    QA.mQASupp.Rows.Add(NewRow);
                }
            }

            FF2LTST.EraseParameters();

            return RunResult;
        }

        public bool EvaluateAppendixE(DataRow drTest, cCheckEngine mCheckEngine, cQAMain QA)
        {
            string APPESummaryId, APPERunId, MonitorLocationId, TestSummaryId;
            string APPEHIGasId, APPEHIOilId;

            Boolean SkipCategory, RunResult = true;

            MonitorLocationId = (string)drTest["mon_loc_id"];
            TestSummaryId = (string)drTest["test_sum_id"];

            QA.mSourceData.Tables["QAAppendixE"].DefaultView.RowFilter = "test_sum_id = '" + TestSummaryId + "'";
            DataRow drAppendixE = (DataRow)QA.mSourceData.Tables["QAAppendixE"].Rows[0];

            QA.mSourceData.Tables["QAAppendixE"].DefaultView.RowFilter = "";

            SkipCategory = false;

            DataRow NewRow;

            cAppendixE AppendixE;
            cCheckParameterBands AppendixEChecks = GetCheckBands("APPE");

            AppendixE = new cAppendixE(mCheckEngine, QA, MonitorLocationId, TestSummaryId);
            AppendixE.SetCheckBands(AppendixEChecks);

            RunResult = AppendixE.ProcessChecks();

            if (RunResult == true)
            {

                if ((Boolean)QA.GetCheckParameter("APPE_System_Valid").ParameterValue == false)
                    SkipCategory = true;

                if (SkipCategory)
                {

                    QA.mSourceData.Tables["QAAppendixESummary"].DefaultView.RowFilter = "TEST_SUM_ID = '" + TestSummaryId + "'";

                    foreach (DataRowView drAPPESummary in QA.mSourceData.Tables["QAAppendixESummary"].DefaultView)
                    {
                        NewRow = QA.mCalculatedAPPESummary.NewRow();
                        NewRow["AE_CORR_TEST_SUM_ID"] = Convert.ToString(drAPPESummary["AE_CORR_TEST_SUM_ID"]);
                        NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;
                        QA.mCalculatedAPPESummary.Rows.Add(NewRow);
                    }

                    QA.mSourceData.Tables["QAAppendixERun"].DefaultView.RowFilter = "test_sum_id = '" + TestSummaryId + "'";

                    foreach (DataRowView drAPPERun in QA.mSourceData.Tables["QAAppendixERun"].DefaultView)
                    {
                        NewRow = QA.mCalculatedAPPERun.NewRow();
                        NewRow["AE_CORR_TEST_RUN_ID"] = Convert.ToString(drAPPERun["AE_CORR_TEST_RUN_ID"]);
                        NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;
                        QA.mCalculatedAPPERun.Rows.Add(NewRow);
                    }


                    QA.mSourceData.Tables["QAAppendixEOil"].DefaultView.RowFilter = "TEST_SUM_ID = '" + TestSummaryId + "'";

                    foreach (DataRowView drAPPEHIOil in QA.mSourceData.Tables["QAAppendixEOil"].DefaultView)
                    {
                        NewRow = QA.mCalculatedAPPEHIOil.NewRow();
                        NewRow["AE_HI_OIL_ID"] = cDBConvert.ToString(drAPPEHIOil["AE_HI_OIL_ID"]);
                        NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;
                        QA.mCalculatedAPPEHIOil.Rows.Add(NewRow);
                    }
                    QA.mSourceData.Tables["QAAppendixEGas"].DefaultView.RowFilter = "TEST_SUM_ID = '" + TestSummaryId + "'";

                    foreach (DataRowView drAPPEHIGas in QA.mSourceData.Tables["QAAppendixEGas"].DefaultView)
                    {
                        NewRow = QA.mCalculatedAPPEHIGas.NewRow();
                        NewRow["AE_HI_GAS_ID"] = cDBConvert.ToString(drAPPEHIGas["AE_HI_GAS_ID"]);
                        NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;
                        QA.mCalculatedAPPEHIGas.Rows.Add(NewRow);
                    }
                }

                else
                {
                    cAppendixESummary AppendixESummary;
                    cCheckParameterBands APPESummaryChecks = GetCheckBands("APPESUM");
                    cCheckParameterBands APPESummaryPass2Checks = GetCheckBands("CLCAESM");
                    cCheckParameterBands APPERunChecks = GetCheckBands("APPERUN");
                    cCheckParameterBands APPERunPass2Checks = GetCheckBands("CLCAERN");
                    cCheckParameterBands APPEOilChecks = GetCheckBands("APPEOIL");
                    cCheckParameterBands APPEGasChecks = GetCheckBands("APPEGAS");

                    QA.mSourceData.Tables["QAAppendixESummary"].DefaultView.RowFilter = "test_sum_id = '" + TestSummaryId + "'";
                    QA.mSourceData.Tables["QAAppendixESummary"].DefaultView.Sort = "op_level_num";

                    foreach (DataRowView drAPPESummary in QA.mSourceData.Tables["QAAppendixESummary"].DefaultView)
                    {

                        SkipCategory = false;

                        APPESummaryId = Convert.ToString(drAPPESummary["ae_corr_test_sum_id"]);
                        AppendixESummary = new cAppendixESummary(mCheckEngine, QA, MonitorLocationId, APPESummaryId, AppendixE);
                        AppendixESummary.SetCheckBands(APPESummaryChecks);

                        RunResult = AppendixESummary.ProcessChecks();

                        if (RunResult == true)
                        {

                            cAppendixERun AppendixERun;

                            QA.mSourceData.Tables["QAAppendixERun"].DefaultView.RowFilter = "ae_corr_test_sum_id = '" + APPESummaryId + "'";
                            QA.mSourceData.Tables["QAAppendixERun"].DefaultView.Sort = "run_num";

                            foreach (DataRowView drAPPERun in QA.mSourceData.Tables["QAAppendixERun"].DefaultView)
                            {
                                APPERunId = Convert.ToString(drAPPERun["AE_CORR_TEST_RUN_ID"]);
                                AppendixERun = new cAppendixERun(mCheckEngine, QA, MonitorLocationId, APPERunId, AppendixESummary);
                                AppendixERun.SetCheckBands(APPERunChecks);
                                RunResult = AppendixERun.ProcessChecks();

                                if (RunResult == true)
                                {

                                    cAppendixEOil AppendixEOil;

                                    QA.mSourceData.Tables["QAAppendixEOil"].DefaultView.RowFilter = "AE_CORR_TEST_RUN_ID = '" + APPERunId + "'";

                                    foreach (DataRowView drAPPEHIOil in QA.mSourceData.Tables["QAAppendixEOil"].DefaultView)
                                    {
                                        APPEHIOilId = cDBConvert.ToString(drAPPEHIOil["AE_HI_OIL_ID"]);
                                        AppendixEOil = new cAppendixEOil(mCheckEngine, QA, MonitorLocationId, APPEHIOilId, AppendixERun);
                                        AppendixEOil.SetCheckBands(APPEOilChecks);
                                        RunResult = AppendixEOil.ProcessChecks();

                                        if (RunResult == true)
                                        {
                                            NewRow = QA.mCalculatedAPPEHIOil.NewRow();
                                            NewRow["AE_HI_OIL_ID"] = cDBConvert.ToString(drAPPEHIOil["AE_HI_OIL_ID"]);
                                            if (QA.GetCheckParameter("APPE_Calc_Oil_Heat_Input").ParameterValue != null)
                                                NewRow["CALC_OIL_HI"] = Convert.ToDecimal(QA.GetCheckParameter("APPE_Calc_Oil_Heat_Input").ParameterValue);
                                            if (QA.GetCheckParameter("APPE_Calc_Oil_Mass").ParameterValue != null)
                                                NewRow["CALC_OIL_MASS"] = Convert.ToDecimal(QA.GetCheckParameter("APPE_Calc_Oil_Mass").ParameterValue);
                                            NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;
                                            QA.mCalculatedAPPEHIOil.Rows.Add(NewRow);
                                        }

                                        AppendixEOil.EraseParameters();
                                    }
                                }

                                if (RunResult == true)
                                {
                                    cAppendixEGas AppendixEGas;

                                    QA.mSourceData.Tables["QAAppendixEGas"].DefaultView.RowFilter = "AE_CORR_TEST_RUN_ID = '" + APPERunId + "'";

                                    foreach (DataRowView drAPPEHIGas in QA.mSourceData.Tables["QAAppendixEGas"].DefaultView)
                                    {
                                        APPEHIGasId = cDBConvert.ToString(drAPPEHIGas["AE_HI_GAS_ID"]);
                                        AppendixEGas = new cAppendixEGas(mCheckEngine, QA, MonitorLocationId, APPEHIGasId, AppendixERun);
                                        AppendixEGas.SetCheckBands(APPEGasChecks);
                                        RunResult = AppendixEGas.ProcessChecks();

                                        if (RunResult == true)
                                        {
                                            NewRow = QA.mCalculatedAPPEHIGas.NewRow();
                                            NewRow["AE_HI_GAS_ID"] = cDBConvert.ToString(drAPPEHIGas["AE_HI_GAS_ID"]);
                                            if (QA.GetCheckParameter("APPE_Calc_Gas_Heat_Input").ParameterValue != null)
                                                NewRow["CALC_GAS_HI"] = Convert.ToDecimal(QA.GetCheckParameter("APPE_Calc_Gas_Heat_Input").ParameterValue);
                                            NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;
                                            QA.mCalculatedAPPEHIGas.Rows.Add(NewRow);
                                        }

                                        AppendixEGas.EraseParameters();
                                    }
                                }

                                if (RunResult == true)
                                {

                                    AppendixERun.SetCheckBands(APPERunPass2Checks);

                                    RunResult = AppendixERun.ProcessChecks();

                                    if (RunResult == true)
                                    {
                                        NewRow = QA.mCalculatedAPPERun.NewRow();
                                        NewRow["AE_CORR_TEST_RUN_ID"] = Convert.ToString(drAPPERun["AE_CORR_TEST_RUN_ID"]);
                                        if (QA.GetCheckParameter("APPE_Calc_Run_Total_HI").ParameterValue != null)
                                            NewRow["CALC_TOTAL_HI"] = Convert.ToDecimal(QA.GetCheckParameter("APPE_Calc_Run_Total_HI").ParameterValue);
                                        if (QA.GetCheckParameter("APPE_Calc_Run_HI_Rate").ParameterValue != null)
                                            NewRow["CALC_HOURLY_HI_RATE"] = Convert.ToDecimal(QA.GetCheckParameter("APPE_Calc_Run_HI_Rate").ParameterValue);
                                        NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;
                                        QA.mCalculatedAPPERun.Rows.Add(NewRow);
                                    }
                                }

                                AppendixERun.EraseParameters();

                            }


                        }

                        if (RunResult == true)
                        {

                            AppendixESummary.SetCheckBands(APPESummaryPass2Checks);
                            RunResult = AppendixESummary.ProcessChecks();

                            if (RunResult == true)
                            {
                                NewRow = QA.mCalculatedAPPESummary.NewRow();
                                NewRow["AE_CORR_TEST_SUM_ID"] = Convert.ToString(drAPPESummary["AE_CORR_TEST_SUM_ID"]);
                                if (QA.GetCheckParameter("APPE_Calc_Level_Mean_Reference_Value").ParameterValue != null)
                                    NewRow["CALC_MEAN_REF_VALUE"] = Convert.ToDecimal(QA.GetCheckParameter("APPE_Calc_Level_Mean_Reference_Value").ParameterValue);
                                if (QA.GetCheckParameter("APPE_Calc_Level_Average_HI_Rate").ParameterValue != null)
                                    NewRow["CALC_AVG_HRLY_HI_RATE"] = Convert.ToDecimal(QA.GetCheckParameter("APPE_Calc_Level_Average_HI_Rate").ParameterValue);
                                NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;
                                QA.mCalculatedAPPESummary.Rows.Add(NewRow);
                            }
                        }

                        AppendixESummary.EraseParameters();

                    }

                    // APEPGVP: Appendix E Protocol Gas Data
                    {
                        RunResult = RunResult && RunPgvpChecks("APEPGVP", AppendixE);
                    }

                    // AEAETB: Appendix E Air Emission Testing Data
                    {
                        RunResult = RunResult && RunAetbChecks("AEAETB", AppendixE);
                    }
                }

                if (RunResult == true)
                {
                    cCheckParameterBands APPEPass2Checks = GetCheckBands("CLCAPPE");
                    AppendixE.SetCheckBands(APPEPass2Checks);
                    RunResult = AppendixE.ProcessChecks();

                    if (RunResult == true)
                    {

                        NewRow = QA.mCalculatedTestSummary.NewRow();
                        NewRow["TEST_SUM_ID"] = TestSummaryId;
                        if (QA.CheckEngine.SeverityCd == eSeverityCd.FATAL || QA.CheckEngine.SeverityCd == eSeverityCd.CRIT1)
                            NewRow["CALC_TEST_RESULT_CD"] = null;
                        else
                            if (QA.CheckEngine.SeverityCd == eSeverityCd.CRIT2)
                            NewRow["CALC_TEST_RESULT_CD"] = "INVALID";
                        else
                            NewRow["CALC_TEST_RESULT_CD"] = "PASSED";

                        NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;
                        QA.mCalculatedTestSummary.Rows.Add(NewRow);

                        if (QA.CheckEngine.SeverityCd != eSeverityCd.FATAL)
                        {
                            NewRow = QA.mQASupp.NewRow();

                            NewRow["TEST_SUM_ID"] = TestSummaryId;
                            NewRow["TEST_TYPE_CD"] = "APPE";
                            NewRow["MON_LOC_ID"] = (string)drTest["mon_loc_id"];
                            if (drTest["mon_sys_id"] != DBNull.Value)
                                NewRow["MON_SYS_ID"] = (string)drTest["mon_sys_id"];
                            if (drTest["test_reason_cd"] != DBNull.Value)
                                NewRow["TEST_REASON_CD"] = (string)drTest["test_reason_cd"];
                            NewRow["TEST_NUM"] = (string)drTest["test_num"];
                            if (drTest["begin_date"] != DBNull.Value)
                                NewRow["BEGIN_DATE"] = cDBConvert.ToString((DateTime)drTest["begin_date"]);
                            if (drTest["begin_hour"] != DBNull.Value)
                                NewRow["BEGIN_HOUR"] = cDBConvert.ToString((Decimal)drTest["begin_hour"]);
                            if (drTest["begin_min"] != DBNull.Value)
                                NewRow["BEGIN_MIN"] = cDBConvert.ToString((Decimal)drTest["begin_min"]);
                            if (drTest["end_date"] != DBNull.Value)
                                NewRow["END_DATE"] = cDBConvert.ToString((DateTime)drTest["end_date"]);
                            if (drTest["end_hour"] != DBNull.Value)
                                NewRow["END_HOUR"] = cDBConvert.ToString((Decimal)drTest["end_hour"]);
                            if (drTest["end_min"] != DBNull.Value)
                                NewRow["END_MIN"] = cDBConvert.ToString((Decimal)drTest["end_min"]);

                            if (QA.CheckEngine.SeverityCd == eSeverityCd.FATAL || QA.CheckEngine.SeverityCd == eSeverityCd.CRIT1)
                                NewRow["TEST_RESULT_CD"] = null;
                            else
                                if (QA.CheckEngine.SeverityCd == eSeverityCd.CRIT2)
                                NewRow["TEST_RESULT_CD"] = "INVALID";
                            else
                                NewRow["TEST_RESULT_CD"] = "PASSED";

                            NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;
                            QA.mQASupp.Rows.Add(NewRow);

                            //add many qa supp attributes record hi and nox rate segment data
                            int SegmentCount = Convert.ToInt16(QA.GetCheckParameter("APPE_Level_Count").ParameterValue);

                            if (SegmentCount != int.MinValue && QA.CheckEngine.SeverityCd != eSeverityCd.CRIT1 && QA.CheckEngine.SeverityCd != eSeverityCd.CRIT2)
                            {

                                NewRow = QA.mQASuppAttribute.NewRow();
                                NewRow["TEST_SUM_ID"] = TestSummaryId;
                                NewRow["ATTRIBUTE_NAME"] = "SEGMENT_COUNT";
                                NewRow["ATTRIBUTE_VALUE"] = Convert.ToString(SegmentCount);
                                NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;
                                QA.mQASuppAttribute.Rows.Add(NewRow);
                                decimal[] NOxRateArray = (decimal[])QA.GetCheckParameter("APPE_NOx_Rate_Array").ParameterValue;
                                decimal[] HIRateArray = (decimal[])QA.GetCheckParameter("APPE_Heat_Input_Rate_Array").ParameterValue;
                                for (int i = 1; i <= SegmentCount; i++)
                                {
                                    NewRow = QA.mQASuppAttribute.NewRow();
                                    NewRow["TEST_SUM_ID"] = TestSummaryId;
                                    NewRow["ATTRIBUTE_NAME"] = "NOX_RATE_" + i;
                                    NewRow["ATTRIBUTE_VALUE"] = Convert.ToString(NOxRateArray[i]);
                                    NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;
                                    QA.mQASuppAttribute.Rows.Add(NewRow);

                                    NewRow = QA.mQASuppAttribute.NewRow();
                                    NewRow["TEST_SUM_ID"] = TestSummaryId;
                                    NewRow["ATTRIBUTE_NAME"] = "HI_RATE_" + i;
                                    NewRow["ATTRIBUTE_VALUE"] = Convert.ToString(HIRateArray[i]);
                                    NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;
                                    QA.mQASuppAttribute.Rows.Add(NewRow);
                                }
                            }
                        }
                    }
                }
            }

            AppendixE.EraseParameters();

            return RunResult;

        }

        public bool EvaluateUnitDefault(DataRow drTest, cCheckEngine mCheckEngine, cQAMain QA)
        {
            string UnitDefaultRunId, MonitorLocationId, TestSummaryId, UnitDefaultSummaryId;
            Boolean SkipCategory, RunResult = true;

            MonitorLocationId = (string)drTest["mon_loc_id"];
            TestSummaryId = (string)drTest["test_sum_id"];
            QA.mSourceData.Tables["QAUnitDefaultTest"].DefaultView.RowFilter = "test_sum_id = '" + TestSummaryId + "'";
            DataRowView drUnitDef = (DataRowView)QA.mSourceData.Tables["QAUnitDefaultTest"].DefaultView[0];
            UnitDefaultSummaryId = Convert.ToString(drUnitDef["UNIT_DEFAULT_TEST_SUM_ID"]);
            SkipCategory = false;

            DataRow NewRow;

            cUnitDefault UnitDefault;
            cCheckParameterBands UnitDefaultChecks = GetCheckBands("UNITDEF");

            UnitDefault = new cUnitDefault(mCheckEngine, QA, MonitorLocationId, TestSummaryId);
            UnitDefault.SetCheckBands(UnitDefaultChecks);

            RunResult = UnitDefault.ProcessChecks();

            if (RunResult == true)
            {
                if ((Boolean)QA.GetCheckParameter("Unit_Default_Fuel_Valid").ParameterValue == false)
                    SkipCategory = true;
                if (!SkipCategory)
                {
                    cUnitDefaultRun UnitDefaultRun;
                    cCheckParameterBands UnitDefaultRunChecks = GetCheckBands("UDEFRUN");

                    QA.mSourceData.Tables["QAUnitDefaultRun"].DefaultView.RowFilter = "test_sum_id = '" + TestSummaryId + "'";
                    QA.mSourceData.Tables["QAUnitDefaultRun"].DefaultView.Sort = "OP_LEVEL_NUM, RUN_NUM";

                    foreach (DataRowView drUnitDefaultRun in QA.mSourceData.Tables["QAUnitDefaultRun"].DefaultView)
                    {
                        SkipCategory = false;
                        UnitDefaultRunId = cDBConvert.ToString(drUnitDefaultRun["UNIT_DEFAULT_TEST_RUN_ID"]);
                        UnitDefaultRun = new cUnitDefaultRun(mCheckEngine, QA, MonitorLocationId, UnitDefaultRunId, UnitDefault);
                        UnitDefaultRun.SetCheckBands(UnitDefaultRunChecks);
                        RunResult = UnitDefaultRun.ProcessChecks();
                        UnitDefaultRun.EraseParameters();
                    }

                    // UDFPGVP: Unit Default Protocol Gas Data
                    {
                        RunResult = RunResult && RunPgvpChecks("UDFPGVP", UnitDefault);
                    }

                    // UDFAETB: Unit Default Air Emission Testing Data
                    {
                        RunResult = RunResult && RunAetbChecks("UDFAETB", UnitDefault);
                    }
                }

                if (RunResult == true)
                {
                    cCheckParameterBands UnitDefaultPass2Checks = GetCheckBands("CLCUDEF");

                    UnitDefault.SetCheckBands(UnitDefaultPass2Checks);

                    RunResult = UnitDefault.ProcessChecks();

                    if (RunResult == true)
                    {
                        NewRow = QA.mCalculatedTestSummary.NewRow();
                        NewRow["TEST_SUM_ID"] = TestSummaryId;
                        if (QA.CheckEngine.SeverityCd == eSeverityCd.FATAL || QA.CheckEngine.SeverityCd == eSeverityCd.CRIT1)
                            NewRow["CALC_TEST_RESULT_CD"] = null;
                        else
                            if (QA.CheckEngine.SeverityCd == eSeverityCd.CRIT2)
                            NewRow["CALC_TEST_RESULT_CD"] = "INVALID";
                        else
                            NewRow["CALC_TEST_RESULT_CD"] = "PASSED";

                        NewRow["TEST_SUM_ID"] = TestSummaryId;

                        NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;
                        QA.mCalculatedTestSummary.Rows.Add(NewRow);

                        NewRow = QA.mCalculatedUnitDefault.NewRow();
                        NewRow["UNIT_DEFAULT_TEST_SUM_ID"] = UnitDefaultSummaryId;
                        if (QA.GetCheckParameter("Unit_Default_Maximum_NOx_Rate").ParameterValue != null)
                            NewRow["CALC_NOX_DEFAULT_RATE"] = cDBConvert.ToString(Convert.ToDecimal(QA.GetCheckParameter("Unit_Default_Maximum_NOx_Rate").ParameterValue));
                        NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;
                        QA.mCalculatedUnitDefault.Rows.Add(NewRow);

                        if (QA.CheckEngine.SeverityCd != eSeverityCd.FATAL)
                        {
                            NewRow = QA.mQASupp.NewRow();

                            NewRow["TEST_SUM_ID"] = TestSummaryId;
                            NewRow["TEST_TYPE_CD"] = "UNITDEF";
                            NewRow["MON_LOC_ID"] = (string)drTest["mon_loc_id"];

                            if (drTest["test_reason_cd"] != DBNull.Value)
                                NewRow["TEST_REASON_CD"] = (string)drTest["test_reason_cd"];
                            NewRow["TEST_NUM"] = (string)drTest["test_num"];
                            if (drTest["begin_date"] != DBNull.Value)
                                NewRow["BEGIN_DATE"] = cDBConvert.ToString((DateTime)drTest["begin_date"]);
                            if (drTest["begin_hour"] != DBNull.Value)
                                NewRow["BEGIN_HOUR"] = cDBConvert.ToString((Decimal)drTest["begin_hour"]);
                            if (drTest["begin_min"] != DBNull.Value)
                                NewRow["BEGIN_MIN"] = cDBConvert.ToString((Decimal)drTest["begin_min"]);
                            if (drTest["end_date"] != DBNull.Value)
                                NewRow["END_DATE"] = cDBConvert.ToString((DateTime)drTest["end_date"]);
                            if (drTest["end_hour"] != DBNull.Value)
                                NewRow["END_HOUR"] = cDBConvert.ToString((Decimal)drTest["end_hour"]);
                            if (drTest["end_min"] != DBNull.Value)
                                NewRow["END_MIN"] = cDBConvert.ToString((Decimal)drTest["end_min"]);
                            if (QA.CheckEngine.SeverityCd == eSeverityCd.FATAL || QA.CheckEngine.SeverityCd == eSeverityCd.CRIT1)
                                NewRow["TEST_RESULT_CD"] = null;
                            else if (QA.CheckEngine.SeverityCd == eSeverityCd.CRIT2)
                                NewRow["TEST_RESULT_CD"] = "INVALID";
                            else
                                NewRow["TEST_RESULT_CD"] = "PASSED";

                            NewRow["OPERATING_CONDITION_CD"] = drUnitDef["OPERATING_CONDITION_CD"];
                            NewRow["FUEL_CD"] = drUnitDef["FUEL_CD"];

                            NewRow["SUBMISSION_ID"] = mCheckEngine.WorkspaceSessionId;

                            QA.mQASupp.Rows.Add(NewRow);
                        }
                    }
                }
            }

            UnitDefault.EraseParameters();
            return RunResult;
        }

        protected override void InitSourceData()
        {
            try
            {
                // Initialize all data tables in process

                mSourceData = new DataSet();
                mFacilityID = GetFacilityID();

                //SqlDataAdapter SourceDataAdapter;
                SqlDataAdapter SourceDataAdapter;
                DataTable SourceDataTable;

                // ElapsedTimes.TimingBegin("InitSourceData", this);

                //get test summary record for this test sum ID
                SourceDataTable = new DataTable("QATestSummary");
                SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_TEST_SUMMARY " +
                  "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "'", mCheckEngine.DbDataConnection.SQLConnection);
                // this defaults to 30 seconds if we don't override it
                if (SourceDataAdapter.SelectCommand != null)
                    SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                SourceDataAdapter.Fill(SourceDataTable);
                mSourceData.Tables.Add(SourceDataTable);

                DataRow drTest = (DataRow)mSourceData.Tables["QATestSummary"].Rows[0];
                string TestTypeCd = (string)drTest["test_type_cd"];

                //get qa supp records for this location ID
                SourceDataTable = new DataTable("QASuppData");
                SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_SUPP_DATA " +
                  "WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM TEST_SUMMARY " +
                  "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
                // this defaults to 30 seconds if we don't override it
                if (SourceDataAdapter.SelectCommand != null)
                    SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                SourceDataAdapter.Fill(SourceDataTable);
                mSourceData.Tables.Add(SourceDataTable);

                //get test result codes
                SourceDataTable = new DataTable("TestResultCode");
                SourceDataAdapter = new SqlDataAdapter("SELECT * FROM test_result_code", mCheckEngine.DbDataConnection.SQLConnection);
                // this defaults to 30 seconds if we don't override it
                if (SourceDataAdapter.SelectCommand != null)
                    SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                SourceDataAdapter.Fill(SourceDataTable);
                mSourceData.Tables.Add(SourceDataTable);

                //get test reason codes
                SourceDataTable = new DataTable("TestReasonCode");
                SourceDataAdapter = new SqlDataAdapter("SELECT * FROM test_reason_code", mCheckEngine.DbDataConnection.SQLConnection);
                // this defaults to 30 seconds if we don't override it
                if (SourceDataAdapter.SelectCommand != null)
                    SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                SourceDataAdapter.Fill(SourceDataTable);
                mSourceData.Tables.Add(SourceDataTable);

                //monitor plan location
                SourceDataTable = new DataTable("MonitorPlanLocation");
                SourceDataAdapter = new SqlDataAdapter("SELECT * FROM vw_monitor_plan_location " +
                        "WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM TEST_SUMMARY " +
                        "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
                // this defaults to 30 seconds if we don't override it
                if (SourceDataAdapter.SelectCommand != null)
                    SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                SourceDataAdapter.Fill(SourceDataTable);
                mSourceData.Tables.Add(SourceDataTable);

                switch (TestTypeCd)
                {
                    case "HGSI3":
                    case "HGLINE":
                    case "LINE":
                        {
                            //get linearity tests for this location ID
                            SourceDataTable = new DataTable("QALinearityTest");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_TEST_SUMMARY_LINE " +
                              "WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM TEST_SUMMARY " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get linearity summary records for this test sum ID
                            SourceDataTable = new DataTable("QALinearitySummary");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM CheckQa.LinearitySummary " +
                              "('" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get linearity injection records for this test sum ID
                            SourceDataTable = new DataTable("QALinearityInjection");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM CheckQa.LinearityInjection " +
                              "('" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get span records for this location ID
                            SourceDataTable = new DataTable("QASpan");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_MONITOR_SPAN " +
                              "WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM TEST_SUMMARY " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get analyzer range records for this component ID
                            SourceDataTable = new DataTable("QAAnalyzerRange");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_ANALYZER_RANGE " +
                              "WHERE COMPONENT_ID IN (SELECT COMPONENT_ID FROM TEST_SUMMARY " +
                              "WHERE COMPONENT_ID IS NOT NULL AND TEST_SUM_ID = '" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get System Component records for this location
                            SourceDataTable = new DataTable("QAComponent");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_COMPONENT " +
                                "WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM TEST_SUMMARY " +
                                "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get linearity injection records for this component ID
                            SourceDataTable = new DataTable("ComponentLinearityInjection");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM CheckQa.LinearityInjectionByComponent " +
                              "('" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get System Component records for this component
                            SourceDataTable = new DataTable("QASystemComponent");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_MONITOR_SYSTEM_COMPONENT " +
                              "WHERE COMPONENT_ID IN (SELECT COMPONENT_ID FROM TEST_SUMMARY " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "' AND COMPONENT_ID IS NOT NULL)", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            // PGVP Source Data Tables
                            {
                                AddSourceData("GasComponentCode", "SELECT * FROM Lookup.GAS_COMPONENT_CODE");
                                AddSourceData("GasTypeCode", "SELECT * FROM GAS_TYPE_CODE");
                                AddSourceData("ProtocolGas", string.Format("SELECT * FROM PROTOCOL_GAS WHERE TEST_SUM_ID = '{0}'", mCheckEngine.TestSumId));
                                AddSourceData("ProtocolGasVendor", "SELECT * FROM PROTOCOL_GAS_VENDOR");
                                AddSourceData("SystemParameter", "SELECT * FROM SYSTEM_PARAMETER");
                            }

                            break;
                        }

                    case "HGLME":
                        {
                            //get monitor location record for the location
                            SourceDataTable = new DataTable("MonitorLocation");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_MONITOR_LOCATION " +
                              "WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM TEST_SUMMARY " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get HgLME Default tests for this location ID
                            SourceDataTable = new DataTable("QAHgLMEDefaultTest");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_TEST_SUMMARY_HGLME " +
                              "WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM TEST_SUMMARY " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get HgLME Default summary records for this test sum ID
                            SourceDataTable = new DataTable("QAHgLMEDefaultLevel");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_HG_LME_DEFAULT_TEST_DATA " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "'", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get HgLME Default run records for this test sum IDCSTestCode
                            SourceDataTable = new DataTable("QAHgLMEDefaultRun");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_HG_LME_DEFAULT_TEST_RUN " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "'", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get CS test codes
                            SourceDataTable = new DataTable("CSTestCode");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM COMMON_STACK_TEST_CODE", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //facility monitor plan location
                            SourceDataTable = new DataTable("FacilityMonitorPlanLocation");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_MONITOR_PLAN_LOCATION " +
                                    "WHERE FAC_ID IN (SELECT FAC_ID FROM VW_MONITOR_LOCATION " +
                                    "WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM TEST_SUMMARY " +
                                    "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "'))", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get qa supp attribute records for this location ID
                            SourceDataTable = new DataTable("QASuppAttribute");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_SUPP_ATTRIBUTE " +
                              "WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM TEST_SUMMARY " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get qa supp attribute records for this location ID
                            SourceDataTable = new DataTable("FacilityHGLMEDefaultTests");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_TEST_SUMMARY_HGLME " +
                                    "WHERE FAC_ID IN (SELECT FAC_ID FROM VW_MONITOR_LOCATION " +
                                    "WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM TEST_SUMMARY " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "'))", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get fac qa supp records for this location ID
                            SourceDataTable = new DataTable("FacilityQASuppData");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_SUPP_DATA " +
                                    "WHERE FAC_ID IN (SELECT FAC_ID FROM VW_MONITOR_LOCATION " +
                                    "WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM TEST_SUMMARY " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "'))", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            SourceDataTable = new DataTable("QAQualification");
                            SourceDataAdapter = new SqlDataAdapter("select * from vw_mp_monitor_qualification " +
                                "where mon_loc_id in (SELECT MON_LOC_ID FROM TEST_SUMMARY " +
                                "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get fac qa supp attribute records for this location ID
                            SourceDataTable = new DataTable("FacilityQASuppAttribute");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_SUPP_ATTRIBUTE " +
                                    "WHERE FAC_ID IN (SELECT FAC_ID FROM VW_MONITOR_LOCATION " +
                                    "WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM TEST_SUMMARY " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "'))", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get default records for this location ID
                            SourceDataTable = new DataTable("QADefault");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_MONITOR_DEFAULT " +
                              "WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM TEST_SUMMARY " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get span records for this location ID
                            SourceDataTable = new DataTable("QASpan");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_MONITOR_SPAN " +
                              "WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM TEST_SUMMARY " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get method records for this location ID
                            SourceDataTable = new DataTable("QAMethod");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_MONITOR_METHOD " +
                              "WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM TEST_SUMMARY " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get method records for this location ID
                            SourceDataTable = new DataTable("QAFacilityMethod");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_MP_MONITOR_METHOD " +
                                    "WHERE FAC_ID IN (SELECT FAC_ID FROM VW_MONITOR_LOCATION " +
                                    "WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM TEST_SUMMARY " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "'))", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get operating level codes
                            SourceDataTable = new DataTable("OperatingLevelCode");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM operating_level_code", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get Unit Stack Configuration records for this stack
                            SourceDataTable = new DataTable("QAUnitStackConfiguration");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_UNIT_STACK_CONFIGURATION " +
                              "WHERE STACK_PIPE_MON_LOC_ID IN (SELECT MON_LOC_ID FROM TEST_SUMMARY " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get reference method codes
                            SourceDataTable = new DataTable("RefMethodCode");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM ref_method_code", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get monitor location records for this facility
                            SourceDataTable = new DataTable("QAFacilityLocation");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_MONITOR_LOCATION " +
                                    "WHERE FAC_ID IN (SELECT FAC_ID FROM VW_MONITOR_LOCATION " +
                                    "WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM TEST_SUMMARY " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get loadrecords for this location ID
                            SourceDataTable = new DataTable("QALoad");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_MONITOR_LOAD " +
                              "WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM TEST_SUMMARY " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            break;
                        }
                    case "RATA":
                        {
                            //get RATAs for this location ID
                            SourceDataTable = new DataTable("QARATA");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_TEST_SUMMARY_RATA " +
                              "WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM TEST_SUMMARY " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get span records for this location ID
                            SourceDataTable = new DataTable("QASpan");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_MONITOR_SPAN " +
                              "WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM TEST_SUMMARY " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get test claim record for this test sum ID
                            SourceDataTable = new DataTable("QATestClaim");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_TEST_CLAIM " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "'", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get RATA summary records for this test sum ID
                            SourceDataTable = new DataTable("QARATASummary");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_RATA_SUMMARY " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "'", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get RATA Run records for this test sum ID
                            SourceDataTable = new DataTable("QARATARun");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_RATA_RUN " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "'", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get Flow RATA Run records for this test sum ID
                            SourceDataTable = new DataTable("QAFlowRATARun");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_FLOW_RATA_RUN " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "'", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get RATA Traverse records for this test sum ID
                            SourceDataTable = new DataTable("QARATATraverse");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_RATA_TRAVERSE " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "'", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get qa supp attribute records for this location ID
                            SourceDataTable = new DataTable("QASuppAttribute");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_SUPP_ATTRIBUTE " +
                              "WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM TEST_SUMMARY " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get default records for this location ID
                            SourceDataTable = new DataTable("QADefault");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_MONITOR_DEFAULT " +
                              "WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM TEST_SUMMARY " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get rectangular duct waf records for this location ID
                            SourceDataTable = new DataTable("QARectDuctWAF");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_RECT_DUCT_WAF " +
                              "WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM TEST_SUMMARY " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get location attribute records for this location ID
                            SourceDataTable = new DataTable("QALocationAttribute");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_LOCATION_ATTRIBUTE " +
                              "WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM TEST_SUMMARY " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get loadrecords for this location ID
                            SourceDataTable = new DataTable("QALoad");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_MONITOR_LOAD " +
                              "WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM TEST_SUMMARY " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get reporting frequency records for this location ID
                            SourceDataTable = new DataTable("QAReportingFrequency");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_LOCATION_REPORTING_FREQUENCY " +
                              "WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM TEST_SUMMARY " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get qualification records for this facility
                            SourceDataTable = new DataTable("QAMonitorQualification");
                            SourceDataAdapter = new SqlDataAdapter("select * from vw_monitor_qualification " +
                                "WHERE FAC_ID IN (SELECT FAC_ID FROM VW_MONITOR_LOCATION " +
                                "WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM TEST_SUMMARY " +
                                "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "'))", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get Unit Stack Configuration records for this stack
                            SourceDataTable = new DataTable("QAUnitStackConfiguration");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_UNIT_STACK_CONFIGURATION " +
                              "WHERE STACK_PIPE_MON_LOC_ID IN (SELECT MON_LOC_ID FROM TEST_SUMMARY " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get operating level codes
                            SourceDataTable = new DataTable("OperatingLevelCode");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM operating_level_code WHERE OP_LEVEL_CD IN ('H', 'M', 'L', 'N')", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get reference method codes
                            SourceDataTable = new DataTable("ReferenceMethodCode");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM ref_method_code", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get rata frequency codes
                            SourceDataTable = new DataTable("RATAFrequencyCode");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM rata_frequency_code", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get pressure measure codes
                            SourceDataTable = new DataTable("PressureMeasureCode");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM pressure_measure_code", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            // AETB and PGVP Source Data Tables
                            {
                                AddSourceData("AirEmissionTesting", string.Format("SELECT * FROM AIR_EMISSION_TESTING WHERE TEST_SUM_ID = '{0}'", mCheckEngine.TestSumId));
                                AddSourceData("GasComponentCode", "SELECT * FROM Lookup.GAS_COMPONENT_CODE");
                                AddSourceData("GasTypeCode", "SELECT * FROM GAS_TYPE_CODE");
                                AddSourceData("ProtocolGas", string.Format("SELECT * FROM PROTOCOL_GAS WHERE TEST_SUM_ID = '{0}'", mCheckEngine.TestSumId));
                                AddSourceData("ProtocolGasVendor", "SELECT * FROM PROTOCOL_GAS_VENDOR");
                                AddSourceData("SystemParameter", "SELECT * FROM SYSTEM_PARAMETER");
                            }

                            AddSourceData("ApsCode", "SELECT * FROM Lookup.APS_CODE");

                            break;

                        }
                    case "F2LCHK":
                        {
                            //get Flow To Load Checks for this location ID
                            SourceDataTable = new DataTable("QAFlowLoadCheck");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_TEST_SUMMARY_F2LCHK " +
                              "WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM TEST_SUMMARY " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get Flow to Load Reference for this location ID
                            SourceDataTable = new DataTable("QAFlowLoadReference");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_TEST_SUMMARY_F2LREF " +
                              "WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM TEST_SUMMARY " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get loadrecords for this location ID
                            SourceDataTable = new DataTable("QALoad");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_MONITOR_LOAD " +
                              "WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM TEST_SUMMARY " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get qa supp attribute records for this location ID
                            SourceDataTable = new DataTable("QASuppAttribute");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_SUPP_ATTRIBUTE " +
                              "WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM TEST_SUMMARY " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get test basis codes
                            SourceDataTable = new DataTable("TestBasisCode");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM test_basis_code", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get operating level codes
                            SourceDataTable = new DataTable("OperatingLevelCode");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM operating_level_code WHERE OP_LEVEL_CD IN ('H', 'M', 'L', 'N')", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get reporting period lookup table
                            SourceDataTable = new DataTable("ReportingPeriodLookup");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM REPORTING_PERIOD", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            break;
                        }
                    case "F2LREF":
                        {
                            //get test summary records for this location ID
                            SourceDataTable = new DataTable("QAFlowLoadReference");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_TEST_SUMMARY_F2LREF " +
                              "WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM TEST_SUMMARY " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get loadrecords for this location ID
                            SourceDataTable = new DataTable("QALoad");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_MONITOR_LOAD " +
                              "WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM TEST_SUMMARY " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get qa supp attribute records for this location ID
                            SourceDataTable = new DataTable("QASuppAttribute");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM QA_SUPP_ATTRIBUTE " +
                              "LEFT OUTER JOIN QA_SUPP_DATA ON QA_SUPP_ATTRIBUTE.QA_SUPP_DATA_ID = QA_SUPP_DATA.QA_SUPP_DATA_ID " +
                              "WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM TEST_SUMMARY " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get Unit Stack Configuration records for this facility
                            SourceDataTable = new DataTable("FacilityUnitStackConfiguration");
                            SourceDataAdapter
                              = new SqlDataAdapter(string.Format("SELECT * FROM VW_MP_UNIT_STACK_CONFIGURATION where fac_id = {0}", CheckEngine.FacilityID),
                                                   mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get qa supp records for this facility
                            SourceDataTable = new DataTable("FacilityQASuppData");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_SUPP_DATA " +
                              "where mon_loc_id in (SELECT MON_LOC_ID FROM TEST_SUMMARY " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get qa supp attribute records for this facility
                            SourceDataTable = new DataTable("FacilityQASuppAttribute");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM QA_SUPP_ATTRIBUTE " +
                              "LEFT OUTER JOIN VW_QA_SUPP_DATA ON QA_SUPP_ATTRIBUTE.QA_SUPP_DATA_ID = VW_QA_SUPP_DATA.QA_SUPP_DATA_ID " +
                              "where mon_loc_id in (SELECT MON_LOC_ID FROM TEST_SUMMARY " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get RATA summary records for this facility
                            SourceDataTable = new DataTable("FacilityRATASummary");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_RATA_SUMMARY " +
                              "where mon_loc_id in (SELECT MON_LOC_ID FROM TEST_SUMMARY " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get RATA Run records for this facility
                            SourceDataTable = new DataTable("FacilityRATARun");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_RATA_RUN " +
                              "where mon_loc_id in (SELECT MON_LOC_ID FROM TEST_SUMMARY " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get operating level codes
                            SourceDataTable = new DataTable("OperatingLevelCode");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM operating_level_code WHERE OP_LEVEL_CD IN ('H', 'M', 'L', 'N')", mCheckEngine.DbDataConnection.SQLConnection);
                            SourceDataAdapter.Fill(SourceDataTable);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            mSourceData.Tables.Add(SourceDataTable);

                            //get monitor location records for this facility
                            SourceDataTable = new DataTable("QALocation");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_MONITOR_LOCATION " +
                              "WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM TEST_SUMMARY " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);
                            break;
                        }
                    case "7DAY":
                        {
                            //get test summary records for this location ID
                            SourceDataTable = new DataTable("QACalibrationTest");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_TEST_SUMMARY_7DAY " +
                              "WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM TEST_SUMMARY " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get calibration injection records for this test sum ID
                            SourceDataTable = new DataTable("QACalibrationInjection");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_CALIBRATION_INJECTION " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "'", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get span records for this location ID
                            SourceDataTable = new DataTable("QASpan");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_MONITOR_SPAN " +
                              "WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM TEST_SUMMARY " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get analyzer range records for this component ID
                            SourceDataTable = new DataTable("QAAnalyzerRange");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_ANALYZER_RANGE " +
                              "WHERE COMPONENT_ID IN (SELECT COMPONENT_ID FROM TEST_SUMMARY " +
                              "WHERE COMPONENT_ID IS NOT NULL AND TEST_SUM_ID = '" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get System Component records for this location
                            SourceDataTable = new DataTable("QAComponent");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_COMPONENT " +
                                "WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM TEST_SUMMARY " +
                                "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get System Component records for this component
                            SourceDataTable = new DataTable("QASystemComponent");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_MONITOR_SYSTEM_COMPONENT " +
                              "WHERE COMPONENT_ID IN (SELECT COMPONENT_ID FROM TEST_SUMMARY " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "' AND COMPONENT_ID IS NOT NULL)", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            break;
                        }
                    case "ONOFF":
                        {
                            //get test summary records for this location ID
                            SourceDataTable = new DataTable("QAOOCTest");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_TEST_SUMMARY_ONOFF " +
                              "WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM TEST_SUMMARY " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get span records for this location ID
                            SourceDataTable = new DataTable("QASpan");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_MONITOR_SPAN " +
                              "WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM TEST_SUMMARY " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get analyzer range records for this component ID
                            SourceDataTable = new DataTable("QAAnalyzerRange");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_ANALYZER_RANGE " +
                              "WHERE COMPONENT_ID IN (SELECT COMPONENT_ID FROM TEST_SUMMARY " +
                              "WHERE COMPONENT_ID IS NOT NULL AND TEST_SUM_ID = '" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get System Component records for this component
                            SourceDataTable = new DataTable("QASystemComponent");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_MONITOR_SYSTEM_COMPONENT " +
                              "WHERE COMPONENT_ID IN (SELECT COMPONENT_ID FROM TEST_SUMMARY " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "' AND COMPONENT_ID IS NOT NULL)", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get System Component records for this location
                            SourceDataTable = new DataTable("QAComponent");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_COMPONENT " +
                                "WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM TEST_SUMMARY " +
                                "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            break;
                        }
                    case "CYCLE":
                        {
                            //get test summary records for this location ID
                            SourceDataTable = new DataTable("QACycle");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_TEST_SUMMARY_CYCLE " +
                              "WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM TEST_SUMMARY " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get cycle time injection records for this test sum ID
                            SourceDataTable = new DataTable("QACycleTimeInjection");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_CYCLE_TIME_INJECTION " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "'", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get span records for this location ID
                            SourceDataTable = new DataTable("QASpan");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_MONITOR_SPAN " +
                              "WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM TEST_SUMMARY " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get analyzer range records for this component ID
                            SourceDataTable = new DataTable("QAAnalyzerRange");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_ANALYZER_RANGE " +
                              "WHERE COMPONENT_ID IN (SELECT COMPONENT_ID FROM TEST_SUMMARY " +
                              "WHERE COMPONENT_ID IS NOT NULL AND TEST_SUM_ID = '" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get System Component records for this component
                            SourceDataTable = new DataTable("QASystemComponent");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_MONITOR_SYSTEM_COMPONENT " +
                              "WHERE COMPONENT_ID IN (SELECT COMPONENT_ID FROM TEST_SUMMARY " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "' AND COMPONENT_ID IS NOT NULL)", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get System Component records for this location
                            SourceDataTable = new DataTable("QAComponent");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_COMPONENT " +
                                "WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM TEST_SUMMARY " +
                                "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            break;
                        }

                    case "FFACC":
                        {
                            //get test summary records for this location ID
                            SourceDataTable = new DataTable("QAAccuracyTest");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_TEST_SUMMARY_FFACC " +
                              "WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM TEST_SUMMARY " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get Accuracy Test Method code lookup
                            SourceDataTable = new DataTable("AccuracyTestMethodCode");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_ACCURACY_TEST_METHOD_CODE", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get System Component records for this component
                            SourceDataTable = new DataTable("QASystemComponent");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_MONITOR_SYSTEM_COMPONENT " +
                              "WHERE COMPONENT_ID IN (SELECT COMPONENT_ID FROM TEST_SUMMARY " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "' AND COMPONENT_ID IS NOT NULL)", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);
                            break;
                        }
                    case "FFACCTT":
                        {
                            //get test summary records for this location ID
                            SourceDataTable = new DataTable("QATransmitterTransducerTest");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_TEST_SUMMARY_FFACCTT " +
                              "WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM TEST_SUMMARY " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get Accuracy Spec Code lookup
                            SourceDataTable = new DataTable("AccuracySpecificationCode");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_ACCURACY_SPEC_CODE", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get System Component records for this component
                            SourceDataTable = new DataTable("QASystemComponent");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_MONITOR_SYSTEM_COMPONENT " +
                              "WHERE COMPONENT_ID IN (SELECT COMPONENT_ID FROM TEST_SUMMARY " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "' AND COMPONENT_ID IS NOT NULL)", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);
                            break;
                        }
                    case "FF2LBAS":
                        {
                            //get test summary records for this location ID
                            SourceDataTable = new DataTable("QAFuelFlowToLoadBaselineTest");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_TEST_SUMMARY_FF2LBAS " +
                              "WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM TEST_SUMMARY " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get System Component records for this component
                            SourceDataTable = new DataTable("QASystemComponent");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_MONITOR_SYSTEM_COMPONENT " +
                              "WHERE MON_SYS_ID IN (SELECT MON_SYS_ID FROM TEST_SUMMARY " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "' AND MON_SYS_ID IS NOT NULL)", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get test summary records for this location ID
                            SourceDataTable = new DataTable("TestSummary");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_TEST_SUMMARY " +
                              "WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM TEST_SUMMARY " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get loadrecords for this location ID
                            SourceDataTable = new DataTable("QALoad");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_MONITOR_LOAD " +
                              "WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM TEST_SUMMARY " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get Unit Stack Configuration records for this stack
                            SourceDataTable = new DataTable("QAUnitStackConfiguration");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_UNIT_STACK_CONFIGURATION " +
                              "WHERE STACK_PIPE_MON_LOC_ID IN (SELECT MON_LOC_ID FROM TEST_SUMMARY " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get Unit Capacity records 
                            SourceDataTable = new DataTable("QAUnitCapacity");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_LOCATION_CAPACITY " +
                                "WHERE FAC_ID IN (SELECT FAC_ID FROM VW_MONITOR_LOCATION " +
                                "WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM TEST_SUMMARY " +
                                "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "'))", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get UOM lookup table
                            SourceDataTable = new DataTable("UnitsOfMeasureLookup");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_PARAMETER_UOM", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);
                            break;
                        }
                    case "FF2LTST":
                        {
                            //get test summary records for this location ID
                            SourceDataTable = new DataTable("QAFuelFlowToLoadTest");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_TEST_SUMMARY_FF2LTST " +
                              "WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM TEST_SUMMARY " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get test summary records for this location ID
                            SourceDataTable = new DataTable("QAFuelFlowToLoadBaselineTest");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_TEST_SUMMARY_FF2LBAS " +
                              "WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM TEST_SUMMARY " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get qa supp attribute records for this location ID
                            SourceDataTable = new DataTable("QASuppAttribute");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_SUPP_ATTRIBUTE " +
                              "WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM TEST_SUMMARY " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get test basis codes
                            SourceDataTable = new DataTable("TestBasisCode");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM test_basis_code", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get reporting period lookup table
                            SourceDataTable = new DataTable("ReportingPeriodLookup");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM REPORTING_PERIOD", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get System Component records for this component
                            SourceDataTable = new DataTable("QASystemSystemComponent");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_MONITOR_SYSTEM_COMPONENT " +
                              "WHERE MON_SYS_ID IN (SELECT MON_SYS_ID FROM TEST_SUMMARY " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);
                            break;
                        }
                    case "APPE":
                        {
                            //get AppendixE tests for this location ID
                            SourceDataTable = new DataTable("QAAppendixE");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_TEST_SUMMARY_APPE " +
                              "WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM TEST_SUMMARY " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get Gas records APPE_Gas_Records
                            SourceDataTable = new DataTable("QAAppendixEGas");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_AE_HI_GAS " +
                                "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "'", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get Oil records APPE_Oil_Records
                            SourceDataTable = new DataTable("QAAppendixEOil");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_AE_HI_OIL " +
                               "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "'", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get AppendixE Run records for this test sum ID
                            SourceDataTable = new DataTable("QAAppendixERun");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_AE_CORRELATION_TEST_RUN " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "'", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get AppendixE summary records for this test sum ID
                            SourceDataTable = new DataTable("QAAppendixESummary");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_AE_CORRELATION_TEST_SUM " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "'", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get UOM lookup table
                            SourceDataTable = new DataTable("UnitsOfMeasureLookup");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_PARAMETER_UOM", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get Monitor System records for this component
                            SourceDataTable = new DataTable("MonitorSystemRecords");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_MONITOR_SYSTEM " +
                                "WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM TEST_SUMMARY " +
                                "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get default records for this location ID
                            SourceDataTable = new DataTable("QADefault");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_MONITOR_DEFAULT " +
                              "WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM TEST_SUMMARY " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            // AETB and PGVP Source Data Tables
                            {
                                AddSourceData("AirEmissionTesting", string.Format("SELECT * FROM AIR_EMISSION_TESTING WHERE TEST_SUM_ID = '{0}'", mCheckEngine.TestSumId));
                                AddSourceData("GasComponentCode", "SELECT * FROM Lookup.GAS_COMPONENT_CODE");
                                AddSourceData("GasTypeCode", "SELECT * FROM GAS_TYPE_CODE");
                                AddSourceData("ProtocolGas", string.Format("SELECT * FROM PROTOCOL_GAS WHERE TEST_SUM_ID = '{0}'", mCheckEngine.TestSumId));
                                AddSourceData("ProtocolGasVendor", "SELECT * FROM PROTOCOL_GAS_VENDOR");
                                AddSourceData("SystemParameter", "SELECT * FROM SYSTEM_PARAMETER");
                            }

                            break;
                        }
                    case "UNITDEF":
                        {
                            //get test summary records for this location ID
                            SourceDataTable = new DataTable("QAUnitDefaultTest");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_TEST_SUMMARY_UNITDEF " +
                              "WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM TEST_SUMMARY " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get unit def run records for this test sum ID
                            SourceDataTable = new DataTable("QAUnitDefaultRun");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_UNIT_DEFAULT_TEST_RUN " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "'", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get method records for this location ID
                            SourceDataTable = new DataTable("QAMethod");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_MONITOR_METHOD " +
                              "WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM TEST_SUMMARY " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get fuel code lookup table
                            SourceDataTable = new DataTable("FuelCodeLookup");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM FUEL_CODE", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get default records for this location ID
                            SourceDataTable = new DataTable("QADefault");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_MONITOR_DEFAULT " +
                              "WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM TEST_SUMMARY " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get default records for this location ID
                            SourceDataTable = new DataTable("QAUnitType");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_MP_LOCATION_UNIT_TYPE " +
                              "WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM TEST_SUMMARY " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            // AETB and PGVP Source Data Tables
                            {
                                AddSourceData("AirEmissionTesting", string.Format("SELECT * FROM AIR_EMISSION_TESTING WHERE TEST_SUM_ID = '{0}'", mCheckEngine.TestSumId));
                                AddSourceData("GasComponentCode", "SELECT * FROM Lookup.GAS_COMPONENT_CODE");
                                AddSourceData("GasTypeCode", "SELECT * FROM GAS_TYPE_CODE");
                                AddSourceData("ProtocolGas", string.Format("SELECT * FROM PROTOCOL_GAS WHERE TEST_SUM_ID = '{0}'", mCheckEngine.TestSumId));
                                AddSourceData("ProtocolGasVendor", "SELECT * FROM PROTOCOL_GAS_VENDOR");
                                AddSourceData("SystemParameter", "SELECT * FROM SYSTEM_PARAMETER");
                            }

                            break;
                        }

                    case "GFMCAL":
                        {
                            //get test summary records for this location ID
                            SourceDataTable = new DataTable("QAGFM");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_TEST_SUMMARY_GFMCAL " +
                              "WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM TEST_SUMMARY " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get cycle time injection records for this test sum ID
                            SourceDataTable = new DataTable("QAGFMData");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_GFM_CALIBRATION_DATA " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "'", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get test summary records for this location ID
                            SourceDataTable = new DataTable("TestSummary");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_TEST_SUMMARY " +
                              "WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM TEST_SUMMARY " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get qa supp attribute records for this location ID
                            SourceDataTable = new DataTable("QASuppAttribute");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_QA_SUPP_ATTRIBUTE " +
                              "WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM TEST_SUMMARY " +
                              "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);
                            break;
                        }
                    default:
                        //"MISC" codes
                        //}
                        //if (TestTypeCd == "PEI" || TestTypeCd == "LEAK" || TestTypeCd == "DAHS" || TestTypeCd == "PEMSACC" || TestTypeCd == "OTHER")
                        {
                            //get Monitor System records for this component
                            SourceDataTable = new DataTable("MonitorSystemRecords");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_MONITOR_SYSTEM " +
                                "WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM TEST_SUMMARY " +
                                "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get System Component records for this location
                            SourceDataTable = new DataTable("QAComponent");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM VW_COMPONENT " +
                                "WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM TEST_SUMMARY " +
                                "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            //get test summary records for this location
                            SourceDataTable = new DataTable("TestSummary");
                            SourceDataAdapter = new SqlDataAdapter("SELECT * FROM TEST_SUMMARY " +
                                "WHERE MON_LOC_ID IN (SELECT MON_LOC_ID FROM TEST_SUMMARY " +
                                "WHERE TEST_SUM_ID = '" + mCheckEngine.TestSumId + "')", mCheckEngine.DbDataConnection.SQLConnection);
                            // this defaults to 30 seconds if we don't override it
                            if (SourceDataAdapter.SelectCommand != null)
                                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
                            SourceDataAdapter.Fill(SourceDataTable);
                            mSourceData.Tables.Add(SourceDataTable);

                            break;
                        }
                }
                // ElapsedTimes.TimingEnd("InitSourceData", this);

                LoadCrossChecks();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("cQAMain.InitSourceData failed: " + ex.Message);
            }
        }

        /// <summary>
        /// This method initializes the class containing static properties enabling strongly typed access to the parameters used by the process.
        /// </summary>
        protected override void InitStaticParameterClass()
        {
            QaParameters.Init(this);
        }

        /// <summary>
        /// Allows the setting of the current category for which parameters will be set.
        /// </summary>
        /// <param name="category"></param>
        public override void SetStaticParameterCategory(cCategory category)
        {
            QaParameters.Category = category;
        }

        #endregion


        #region Virtual Overrides: DB Update

        /// <summary>
        /// Loads ECMPS_WS tables for the process with calculated values.
        /// </summary>
        /// <param name="sqlTransaction">The transaction to use with any commands.  Use null for no transaction.</param>
        /// <param name="errorMessage">The error message returned on failure.</param>
        /// <returns>Returns true if the update succeeds.</returns>
        protected override bool DbUpdate_CalcWsLoad(SqlTransaction sqlTransaction, ref string errorMessage)
        {
            bool result;

            if (DbWsConnection.ClearScratchSession(eWorkspaceDataType.QA, mCheckEngine.WorkspaceSessionId))
            {
                if (DbWsConnection.BulkLoad(mCalculatedLinearitySummary, "QA_LINEARITYSUMMARY", ref errorMessage) &&
                    DbWsConnection.BulkLoad(mCalculatedHGTestSummary, "QA_HGTESTSUMMARY", ref errorMessage) &&
                    DbWsConnection.BulkLoad(mCalculatedRATA, "QA_RATA", ref errorMessage) &&
                    DbWsConnection.BulkLoad(mCalculatedRATASummary, "QA_RATASUMMARY", ref errorMessage) &&
                    DbWsConnection.BulkLoad(mCalculatedRATARun, "QA_RATARUN", ref errorMessage) &&
                    DbWsConnection.BulkLoad(mCalculatedFlowRATARun, "QA_FLOWRATARUN", ref errorMessage) &&
                    DbWsConnection.BulkLoad(mCalculatedRATATraverse, "QA_RATATRAVERSE", ref errorMessage) &&
                    DbWsConnection.BulkLoad(mCalculatedCycleTimeSummary, "QA_CYCLETIMESUMMARY", ref errorMessage) &&
                    DbWsConnection.BulkLoad(mCalculatedCycleTimeInjection, "QA_CYCLETIMEINJECTION", ref errorMessage) &&
                    DbWsConnection.BulkLoad(mCalculatedCalibrationInjection, "QA_CALIBRATIONINJECTION", ref errorMessage) &&
                    DbWsConnection.BulkLoad(mCalculatedFlowToLoadReference, "QA_FLOWTOLOADREFERENCE", ref errorMessage) &&
                    DbWsConnection.BulkLoad(mCalculatedOOC, "QA_ONOFFCALIBRATION", ref errorMessage) &&
                    DbWsConnection.BulkLoad(mCalculatedAPPESummary, "QA_AECORRTESTSUMMARY", ref errorMessage) &&
                    DbWsConnection.BulkLoad(mCalculatedAPPERun, "QA_AECORRTESTRUN", ref errorMessage) &&
                    DbWsConnection.BulkLoad(mCalculatedAPPEHIGas, "QA_AEHEATINPUTFROMGAS", ref errorMessage) &&
                    DbWsConnection.BulkLoad(mCalculatedAPPEHIOil, "QA_AEHEATINPUTFROMOIL", ref errorMessage) &&
                    DbWsConnection.BulkLoad(mCalculatedUnitDefault, "QA_UnitDefaultTest", ref errorMessage) &&
                    DbWsConnection.BulkLoad(mCalculatedTestSummary, "QA_TESTSUMMARY", ref errorMessage) &&
                    DbWsConnection.BulkLoad(mQASuppAttribute, "QA_QASUPPLEMENTALATTRIBUTE", ref errorMessage) &&
                    DbWsConnection.BulkLoad(mCalculatedGFMCalibration, "QA_GFMCALIBRATION", ref errorMessage) &&
                    DbWsConnection.BulkLoad(mCalculatedHGLME, "QA_HgLMEDefaultTestSummary", ref errorMessage) &&
                    DbWsConnection.BulkLoad(mCalculatedHGLMESummary, "QA_HgLMEDefaultTestData", ref errorMessage) &&
                    DbWsConnection.BulkLoad(mQASupp, "QA_QASUPPLEMENTALDATA", ref errorMessage))
                    result = true;
                else
                    result = false;
            }
            else
            {
                errorMessage = mCheckEngine.DbWsConnection.LastError;
                result = false;
            }

            return result;
        }

        /// <summary>
        /// The Update ECMPS Status process identifier.
        /// </summary>
        protected override string DbUpdate_EcmpsStatusProcess { get { return "QA Test Evaluation"; } }

        /// <summary>
        /// The Update ECMPS Status id key or list for the item(s) for which the update will occur.
        /// </summary>
        protected override string DbUpdate_EcmpsStatusIdKeyOrList { get { return mCheckEngine.TestSumId; } }

        /// <summary>
        /// The Update ECMPS Status Additional value for the items(s) for which the update will occur..
        /// </summary>
        protected override string DbUpdate_EcmpsStatusOtherField { get { return mCheckEngine.ChkSessionId; } }

        /// <summary>
        /// Returns the WS data type for the process.
        /// </summary>
        /// <returns>The workspace data type for the process, or null for none.</returns>
        protected override eWorkspaceDataType? DbUpdate_WorkspaceDataType { get { return eWorkspaceDataType.QA; } }

        #endregion


        #region Private Methods

        private void LoadCrossChecks()
        {
            DataTable Catalog = mCheckEngine.DbAuxConnection.GetDataTable("SELECT * FROM vw_Cross_Check_Catalog");
            DataTable Value = mCheckEngine.DbAuxConnection.GetDataTable("SELECT * FROM vw_Cross_Check_Catalog_Value");
            DataTable CrossCheck;
            DataRow CrossCheckRow;
            string CrossCheckName;
            string Column1Name;
            string Column2Name;
            string Column3Name;

            foreach (DataRow CatalogRow in Catalog.Rows)
            {
                CrossCheckName = (string)CatalogRow["Cross_Chk_Catalog_Name"];
                CrossCheckName = CrossCheckName.Replace(" ", "");

                CrossCheck = new DataTable("CrossCheck_" + CrossCheckName);

                Column1Name = (string)CatalogRow["Description1"];
                Column2Name = (string)CatalogRow["Description2"];

                CrossCheck.Columns.Add(Column1Name);
                CrossCheck.Columns.Add(Column2Name);

                if (CatalogRow["Description3"] != DBNull.Value)
                {
                    Column3Name = (string)CatalogRow["Description3"];
                    CrossCheck.Columns.Add(Column3Name);
                }
                else Column3Name = "";

                Column1Name.Replace(" ", "");
                Column2Name.Replace(" ", "");
                Column3Name.Replace(" ", "");

                Value.DefaultView.RowFilter = "Cross_Chk_Catalog_Id = " + cDBConvert.ToString(CatalogRow["Cross_Chk_Catalog_Id"]);

                foreach (DataRowView ValueRow in Value.DefaultView)
                {
                    CrossCheckRow = CrossCheck.NewRow();

                    CrossCheckRow[Column1Name] = ValueRow["Value1"];
                    CrossCheckRow[Column2Name] = ValueRow["Value2"];

                    if (CatalogRow["Description3"] != DBNull.Value)
                        CrossCheckRow[Column3Name] = ValueRow["Value3"];

                    CrossCheck.Rows.Add(CrossCheckRow);
                }

                mSourceData.Tables.Add(CrossCheck);
            }
        }

        private bool RunAetbChecks(string categoryCd, cCategory parentCategory)
        {
            bool result = true;

            cAetbCategory aetbCategory;
            cCheckParameterBands aetbChecks = GetCheckBands(categoryCd);

            foreach (DataRowView airEmissionTestingRow in this.mSourceData.Tables["AirEmissionTesting"].DefaultView)
            {
                aetbCategory = new cAetbCategory(parentCategory, categoryCd, airEmissionTestingRow);
                aetbCategory.SetCheckBands(aetbChecks);

                result = aetbCategory.ProcessChecks() && result;

                aetbCategory.EraseParameters();
            }

            return result;
        }

        private bool RunPgvpChecks(string categoryCd, cCategory parentCategory)
        {
            bool result = true;

            cPgvpCategory pgvpCategory;
            cCheckParameterBands pgvpChecks = GetCheckBands(categoryCd);

            foreach (DataRowView protocolGasRow in this.mSourceData.Tables["ProtocolGas"].DefaultView)
            {
                pgvpCategory = new cPgvpCategory(parentCategory, categoryCd, protocolGasRow);
                pgvpCategory.SetCheckBands(pgvpChecks);

                result = pgvpCategory.ProcessChecks() && result;

                pgvpCategory.EraseParameters();
            }

            return result;
        }

        #endregion

    }

}