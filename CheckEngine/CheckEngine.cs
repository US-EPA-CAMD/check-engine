using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Runtime.Remoting;

using ECMPS.Checks.CheckEngine.Definitions;
using ECMPS.Checks.DatabaseAccess;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Common;
using ECMPS.Definitions.Extensions;
using ECMPS.Definitions.SeverityCode;
using ECMPS.ErrorSuppression;

namespace ECMPS.Checks.CheckEngine
{

    /// <summary>
    /// The ECMPS Check Engine 
    /// </summary>
    public class cCheckEngine
    {

        #region Public Constructors

        /// <summary>
        /// Creates a cCheckEngine object and initialized the UserId, DataConnectionString,
        /// AuxConnectionString, WsConnectionString, DllPath and SystemStateDumpPath properties.
        /// </summary>
        /// <param name="userId">The current user id.</param>
        /// <param name="dataConnectionString">The ECMPS database connection string.</param>
        /// <param name="auxConnectionString">The ECMPS_AUX database conneciton string.</param>
        /// <param name="workspaceConnectionString">The ECMPS_WS database connection string.</param>
        /// <param name="checksDllPath">The path of the checks DLLs.</param>
        /// <param name="systemStateDumpFilePath">The path in which to dump system state information.</param>
        /// <param name="commandTimeout">The timeout to use on SQL commands.</param>
        public cCheckEngine(string userId,
                            string dataConnectionString,
                            string auxConnectionString,
                            string workspaceConnectionString,
                            string checksDllPath,
                            string systemStateDumpFilePath,
                            int? commandTimeout)
        {
            cDecimalPrecision.Initialize();

            UserId = userId;

            DataConnectionString = dataConnectionString;
            AuxConnectionString = auxConnectionString;
            WorkspaceConnectionString = workspaceConnectionString;

            ChecksDllPath = checksDllPath;

            CommandTimeout = commandTimeout.Default(300);
        }

        #endregion


        #region Protected Constructors

        /// <summary>
        /// Instantiates a cCheckEngine object primarily for unit testing purposes.
        /// </summary>
        protected cCheckEngine()
        {
        }

        #endregion


        #region Public Methods: Run Checks

        #region EM

        /// <summary>
        /// Runs the evaluation for an 'EMGEN' process.
        /// </summary>
        /// <param name="monPlanId">The MON_PLAN_ID of the emissions report.</param>
        /// <param name="rptPeriodId">The RPT_PERIOD_ID of the emissions report.</param>
        /// <param name="runMode">The type of run to perform.</param>
        /// <returns>True if the evaluation completed without processing errors.</returns>
        public bool RunChecks_EmGenerate(string monPlanId, int rptPeriodId, eCheckEngineRunMode runMode)
        {
            bool result = false;

            RunChecks_PropertiesClear();
            MonPlanId = monPlanId;
            RptPeriodId = rptPeriodId;

            result = RunChecks_Process("EMGEN", null,
                                       "ECMPS.Checks.Emissions.dll",
                                       "ECMPS.Checks.NonOperatingEmissionGeneration",
                                       "cGenerationProcess",
                                       new object[] { this },
                                       runMode);

            return result;
        }

        /// <summary>
        /// Runs the evaluation for an 'EMIMPRT' process.
        /// </summary>
        /// <param name="sORISCode">The ORIS_CODE for the import</param>
        /// <param name="nSessionID">The SESSION_ID for the import</param>
        /// <param name="runMode">The type of run to perform.</param>
        /// <returns>True if the evaluation completed without processing errors.</returns>
        public bool RunChecks_EmImport(string sORISCode, decimal nSessionID, eCheckEngineRunMode runMode)
        {
            bool result;

            RunChecks_PropertiesClear();
            ORISCode = sORISCode;

            result = RunChecks_Process("EMIMPRT", null,
                                       "ECMPS.Checks.Import.dll",
                                       "ECMPS.Checks.HourlyEmissionsImport",
                                       "cEmissionsImportProcess",
                                       new object[] { this, nSessionID },
                                       runMode);

            return result;
        }

        /// <summary>
        /// Runs the evaluation for an 'HOURLY' process.
        /// </summary>
        /// <param name="monPlanId">The MON_PLAN_ID of the emissions report.</param>
        /// <param name="rptPeriodId">The RPT_PERIOD_ID of the emissions report.</param>
        /// <param name="runMode">The type of run to perform.</param>
        /// <returns>True if the evaluation completed without processing errors.</returns>
        public bool RunChecks_EmReport(string monPlanId, int rptPeriodId, eCheckEngineRunMode runMode)
        {
            bool result;

            RunChecks_PropertiesClear();
            MonPlanId = monPlanId;
            RptPeriodId = rptPeriodId;

            result = RunChecks_Process("HOURLY", null,
                                       "ECMPS.Checks.Emissions.dll",
                                       "ECMPS.Checks.EmissionsReport",
                                       "cEmissionsReportProcess",
                                       new object[] { this },
                                       runMode);

            return result;
        }

        #endregion

        #region LME

        /// <summary>
        /// Runs the evaluation for an 'LMEGEN' process.
        /// </summary>
        /// <param name="monPlanId">The MON_PLAN_ID of the emissions report.</param>
        /// <param name="rptPeriodId">The RPT_PERIOD_ID of the emissions report.</param>
        /// <param name="runMode">The type of run to perform.</param>
        /// <returns>True if the evaluation completed without processing errors.</returns>
        public bool RunChecks_LmeGenerate(string monPlanId, int rptPeriodId, eCheckEngineRunMode runMode)
        {
            bool result;

            RunChecks_PropertiesClear();
            MonPlanId = monPlanId;
            RptPeriodId = rptPeriodId;

            result = RunChecks_Process("LMEGEN", null,
                                       "ECMPS.Checks.LME.dll",
                                       "ECMPS.Checks.LME",
                                       "cLMEGenerationProcess",
                                       new object[] { this },
                                       runMode);

            return result;
        }

        /// <summary>
        /// Runs the evaluation for an 'LMEIMPT' process.
        /// </summary>
        /// <param name="sORISCode">The ORIS_CODE for the import</param>
        /// <param name="runMode">The type of run to perform.</param>
        /// <returns>True if the evaluation completed without processing errors.</returns>
        public bool RunChecks_LmeImport(string sORISCode, eCheckEngineRunMode runMode)
        {
            bool result;

            RunChecks_PropertiesClear();
            ORISCode = sORISCode;

            result = RunChecks_Process("LMEIMPT", null,
                                       "ECMPS.Checks.LME.dll",
                                       "ECMPS.Checks.LME",
                                       "cLMEImportProcess",
                                       new object[] { this },
                                       runMode);

            return result;
        }

        /// <summary>
        /// Runs the evaluation for an 'LMESCRN' process.
        /// </summary>
        /// <param name="categoryCd">The category code for which to evaluate.</param>
        /// <param name="thisTable">The table of data to evaluate.</param>
        /// <param name="monPlanId">The MON_PLAN_ID to evaluate.</param>
        /// <param name="rptPeriodId">The RPT_PERIOD_ID of the emissions report.</param>
        /// <param name="monLocId">The MON_LOC_ID to evaluate.</param>
        /// <param name="runMode">The type of run to perform.</param>
        /// <returns>True if the evaluation completed without processing errors.</returns>
        public bool RunChecks_LmeScreen(string categoryCd, DataTable thisTable, string monPlanId, int rptPeriodId, string monLocId, eCheckEngineRunMode runMode)
        {
            bool result;

            RunChecks_PropertiesClear();
            ThisTable = thisTable;
            MonPlanId = monPlanId;
            RptPeriodId = rptPeriodId;
            MonLocId = monLocId;

            result = RunChecks_Process("LMESCRN", categoryCd,
                                       "ECMPS.Checks.LME.dll",
                                       "ECMPS.Checks.LME",
                                       "cLMEScreenProcess",
                                       new object[] { this, categoryCd },
                                       runMode);

            return result;
        }

        #endregion

        #region MP

        /// <summary>
        /// Runs the evaluation for an 'MPIMPRT' process.
        /// </summary>
        /// <param name="sORISCode">The ORIS_CODE for the import</param>
        /// <param name="nSessionID">The SESSION_ID for the import</param>
        /// <param name="runMode">The type of run to perform.</param>
        /// <returns>True if the evaluation completed without processing errors.</returns>
        public bool RunChecks_MpImport(string sORISCode, decimal nSessionID, eCheckEngineRunMode runMode)
        {
            bool result;

            RunChecks_PropertiesClear();
            ORISCode = sORISCode;

            result = RunChecks_Process("MPIMPRT", null,
                                       "ECMPS.Checks.Import.dll",
                                       "ECMPS.Checks.MonitorPlanImport",
                                       "cMonitorPlanImportProcess",
                                       new object[] { this, nSessionID },
                                       runMode);

            return result;
        }

        /// <summary>
        /// Runs the evaluation for an 'MP' process.
        /// </summary>
        /// <param name="monPlanId">The MON_PLAN_ID to evaluate.</param>
        /// <param name="evaluationBeganDate">The began date of the evaluation period.</param>
        /// <param name="evaluationEndedDate">The ended date of the evaluation period.</param>
        /// <param name="runMode">The type of run to perform.</param>
        /// <returns>True if the evaluation completed without processing errors.</returns>
        public bool RunChecks_MpReport(string monPlanId, DateTime evaluationBeganDate, DateTime evaluationEndedDate, eCheckEngineRunMode runMode)
        {
            bool result;

            RunChecks_PropertiesClear();
            MonPlanId = monPlanId;
            EvaluationBeganDate = evaluationBeganDate;
            EvaluationEndedDate = evaluationEndedDate;

            result = RunChecks_Process("MP", null,
                                       "ECMPS.Checks.MonitorPlan.dll",
                                       "ECMPS.Checks.MonitorPlanEvaluation",
                                       "cMonitorPlan",
                                       new object[] { this },
                                       runMode,
                                       RunChecks_MpReport_AdditionalInitialization);

            return result;
        }

        /// <summary>
        /// Runs the evaluation for an 'MPSCRN' process.
        /// </summary>
        /// <param name="categoryCd">The category code for which to evaluate.</param>
        /// <param name="thisTable">The table of data to evaluate.</param>
        /// <param name="monLocId">The MON_LOC_ID to evaluate.</param>
        /// <param name="runMode">The type of run to perform.</param>
        /// <returns>True if the evaluation completed without processing errors.</returns>
        public bool RunChecks_MpScreen(string categoryCd, DataTable thisTable, string monLocId, eCheckEngineRunMode runMode)
        {
            bool result;

            RunChecks_PropertiesClear();

            ThisTable = thisTable;
            MonLocId = monLocId;
            MaximumFutureDate = DateTime.MaxValue;

            result = RunChecks_Process("MPSCRN", categoryCd,
                                       "ECMPS.Checks.MonitorPlan.dll",
                                       "ECMPS.Checks.MPScreenEvaluation",
                                       "cMPScreenMain",
                                       new object[] { this, categoryCd },
                                       runMode);

            return result;
        }


        #region Helper Methods

        private bool RunChecks_MpReport_AdditionalInitialization()
        {
            bool result;

            DataDBDataContext dataDb = DbDataConnection.GetDataDBContext();

            DateTime? evaluationEndDate = null;
            DateTime? maximumFutureDate = null;
            char? resultChar = null;
            string resultMessage = null;

            dataDb.GetInitialValues(MonPlanId, EvaluationEndedDate, ref evaluationEndDate, ref maximumFutureDate, ref resultChar, ref resultMessage);

            if (resultChar == 'T')
            {
                EvaluationEndedDate = evaluationEndDate.Value;
                MaximumFutureDate = maximumFutureDate.Value;
                result = true;
            }
            else
            {
                HandleProcessingError(string.Format("MP Report Additional Initialization Failed: {0}", resultMessage));
                result = false;
            }

            return result;
        }

        #endregion

        #endregion

        #region QA

        /// <summary>
        /// Runs the evaluation for an 'QAIMPRT' process.
        /// </summary>
        /// <param name="sORISCode">The ORIS_CODE for the import</param>
        /// <param name="nSessionID">The SESSION_ID for the import</param>
        /// <param name="runMode">The type of run to perform.</param>
        /// <returns>True if the evaluation completed without processing errors.</returns>
        public bool RunChecks_QaImport(string sORISCode, decimal nSessionID, eCheckEngineRunMode runMode)
        {
            bool result;

            RunChecks_PropertiesClear();
            ORISCode = sORISCode;

            result = RunChecks_Process("QAIMPRT", null,
                                       "ECMPS.Checks.Import.dll",
                                       "ECMPS.Checks.QAImport",
                                       "cQAImportProcess",
                                       new object[] { this, nSessionID },
                                       runMode);

            return result;
        }

        /// <summary>
        /// Runs the evaluation for an 'OTHERQA' process and 'EVENT' category.
        /// </summary>
        /// <param name="qaCertEventId">The QA_CERT_EVENT_ID to evaluate.</param>
        /// <param name="monPlanId">The MON_PLAN_ID to evaluate.</param>
        /// <param name="runMode">The type of run to perform.</param>
        /// <returns>True if the evaluation completed without processing errors.</returns>
        public bool RunChecks_QaReport_Qce(string qaCertEventId, string monPlanId, eCheckEngineRunMode runMode)
        {
            bool result;

            RunChecks_PropertiesClear();

            QaCertEventId = qaCertEventId;
            MonPlanId = monPlanId;

            result = RunChecks_Process("OTHERQA", "EVENT",
                                       "ECMPS.Checks.QA.dll",
                                       "ECMPS.Checks.OtherQAEvaluation",
                                       "cOtherQAMain",
                                       new object[] { this },
                                       runMode);

            return result;
        }

        /// <summary>
        /// Runs the evaluation for an 'OTHERQA' process and 'TEE' category.
        /// </summary>
        /// <param name="testExtensionExemptionId">The TEST_EXTENSION_EXEMPTION_ID to evaluate.</param>
        /// <param name="monPlanId">The MON_PLAN_ID to evaluate.</param>
        /// <param name="runMode">The type of run to perform.</param>
        /// <returns>True if the evaluation completed without processing errors.</returns>
        public bool RunChecks_QaReport_Tee(string testExtensionExemptionId, string monPlanId, eCheckEngineRunMode runMode)
        {
            bool result;

            RunChecks_PropertiesClear();

            TestExtensionExemptionId = testExtensionExemptionId;
            MonPlanId = monPlanId;

            result = RunChecks_Process("OTHERQA", "TEE",
                                       "ECMPS.Checks.QA.dll",
                                       "ECMPS.Checks.OtherQAEvaluation",
                                       "cOtherQAMain",
                                       new object[] { this },
                                       runMode);

            return result;
        }

        /// <summary>
        /// Runs the evaluation for an 'TEST' process.
        /// </summary>
        /// <param name="monPlanId">The MON_PLAN_ID to evaluate.</param>
        /// <param name="testSumId">The TEST_SUM_ID to evaluate.</param>
        /// <param name="runMode">The type of run to perform.</param>
        /// <returns>True if the evaluation completed without processing errors.</returns>
        public bool RunChecks_QaReport_Test(string testSumId, string monPlanId, eCheckEngineRunMode runMode)
        {
            bool result;

            RunChecks_PropertiesClear();

            TestSumId = testSumId;
            MonPlanId = monPlanId;

            result = RunChecks_Process("TEST", null,
                                       "ECMPS.Checks.QA.dll",
                                       "ECMPS.Checks.QAEvaluation",
                                       "cQAMain",
                                       new object[] { this },
                                       runMode);

            return result;
        }

        /// <summary>
        /// Runs the evaluation for an 'QASCRN' process and 'SCREVNT' category.
        /// </summary>
        /// <param name="thisTable">The table of data to evaluate.</param>
        /// <param name="qaCertEventId">The QA_CERT_EVENT_ID to evaluate.</param>
        /// <param name="monPlanId">The MON_PLAN_ID to evaluate.</param>
        /// <param name="monLocId">The MON_LOC_ID to evaluate.</param>
        /// <param name="runMode">The type of run to perform.</param>
        /// <returns>True if the evaluation completed without processing errors.</returns>
        public bool RunChecks_QaScreen_Qce(DataTable thisTable, string qaCertEventId, string monPlanId, string monLocId, eCheckEngineRunMode runMode)
        {
            bool result;

            RunChecks_PropertiesClear();
            ThisTable = thisTable;
            QaCertEventId = qaCertEventId;
            MonPlanId = monPlanId;
            MonLocId = monLocId;

            result = RunChecks_Process("QASCRN", "SCREVNT",
                                       "ECMPS.Checks.QA.dll",
                                       "ECMPS.Checks.QAScreenEvaluation",
                                       "cQAScreenMain",
                                       new object[] { this, "SCREVNT" },
                                       runMode);

            return result;
        }

        /// <summary>
        /// Runs the evaluation for an 'QASCRN' process and 'SCRTEE' category.
        /// </summary>
        /// <param name="thisTable">The table of data to evaluate.</param>
        /// <param name="testExtensionExemptionId">The TEST_EXTENSION_EXEMPTION_ID to evaluate.</param>
        /// <param name="monPlanId">The MON_PLAN_ID to evaluate.</param>
        /// <param name="monLocId">The MON_LOC_ID to evaluate.</param>
        /// <param name="runMode">The type of run to perform.</param>
        /// <returns>True if the evaluation completed without processing errors.</returns>
        public bool RunChecks_QaScreen_Tee(DataTable thisTable, string testExtensionExemptionId, string monPlanId, string monLocId, eCheckEngineRunMode runMode)
        {
            bool result;

            RunChecks_PropertiesClear();

            ThisTable = thisTable;
            TestExtensionExemptionId = testExtensionExemptionId;
            MonPlanId = monPlanId;
            MonLocId = monLocId;

            result = RunChecks_Process("QASCRN", "SCRTEE",
                                       "ECMPS.Checks.QA.dll",
                                       "ECMPS.Checks.QAScreenEvaluation",
                                       "cQAScreenMain",
                                       new object[] { this, "SCRTEE" },
                                       runMode);

            return result;
        }

        /// <summary>
        /// Runs the evaluation for an 'QASCRN' process.
        /// </summary>
        /// <param name="categoryCd">The category code for which to evaluate.</param>
        /// <param name="thisTable">The table of data to evaluate.</param>
        /// <param name="testSumId">The TEST_SUM_ID to evaluate.</param>
        /// <param name="monPlanId">The MON_PLAN_ID to evaluate.</param>
        /// <param name="monLocId">The MON_LOC_ID to evaluate.</param>
        /// <param name="runMode">The type of run to perform.</param>
        /// <returns>True if the evaluation completed without processing errors.</returns>
        public bool RunChecks_QaScreen_Test(string categoryCd, DataTable thisTable, string testSumId, string monPlanId, string monLocId, eCheckEngineRunMode runMode)
        {
            bool result;

            RunChecks_PropertiesClear();
            ThisTable = thisTable;
            TestSumId = testSumId;
            MonPlanId = monPlanId;
            MonLocId = monLocId;

            result = RunChecks_Process("QASCRN", categoryCd,
                                       "ECMPS.Checks.QA.dll",
                                       "ECMPS.Checks.QAScreenEvaluation",
                                       "cQAScreenMain",
                                       new object[] { this, categoryCd },
                                       runMode);

            return result;
        }

        #endregion

        #region Comment

        /// <summary>
        /// Runs the evaluation for an 'MPSCRN' process.
        /// </summary>
        /// <param name="categoryCd">The category code for which to evaluate.</param>
        /// <param name="thisTable">The table of data to evaluate.</param>
        /// <param name="monPlanId">The MON_PLAN_ID to evaluate.</param>
        /// <param name="monLocId">The MON_LOC_ID to evaluate.</param>
        /// <param name="runMode">The type of run to perform.</param>
        /// <returns>True if the evaluation completed without processing errors.</returns>
        public bool RunChecks_Comment(string categoryCd, DataTable thisTable, string monPlanId, string monLocId, eCheckEngineRunMode runMode)
        {
            bool result;

            RunChecks_PropertiesClear();

            ThisTable = thisTable;
            MonPlanId = monPlanId;
            MonLocId = monLocId;

            string processCd, processDllName, processNameSpace, processClassName;
            {
                if (categoryCd == "EMCOMM")
                {
                    processCd = "LMESCRN";
                    processDllName = "ECMPS.Checks.LME.dll";
                    processNameSpace = "ECMPS.Checks.LME";
                    processClassName = "cLMEScreenProcess";
                }
                else
                {
                    processCd = "MPSCRN";
                    processDllName = "ECMPS.Checks.MonitorPlan.dll";
                    processNameSpace = "ECMPS.Checks.MPScreenEvaluation";
                    processClassName = "cMPScreenMain";
                }
            }

            result = RunChecks_Process(processCd, categoryCd,
                                       processDllName,
                                       processNameSpace,
                                       processClassName,
                                       new object[] { this, categoryCd },
                                       runMode);

            return result;
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Handles the actual running of the checks.
        /// </summary>
        /// <param name="processCd">The process code for which to process.</param>
        /// <param name="categoryCd">The category code for which to process.</param>
        /// <param name="processDllName">The name of the process DLL.</param>
        /// <param name="processNameSpace">The name space of the process.</param>
        /// <param name="processClassName">The class name of the process.</param>
        /// <param name="processConstructorArgements">The constructor arguments of the process.</param>
        /// <param name="runMode">The type of run to perform.</param>
        /// <param name="additionalInitialization">Delegate for additional initialization that should occur before running checks.</param>
        /// <returns>Return true if processing is successful.</returns>
        private bool RunChecks_Process(string processCd, string categoryCd,
                                       string processDllName,
                                       string processNameSpace,
                                       string processClassName,
                                       object[] processConstructorArgements,
                                       eCheckEngineRunMode runMode,
                                       dAdditionalInitialization additionalInitialization)
        {
            bool result;

            RunMode = runMode; // Sets CheckTestInit and DebugMode

            string errorMessage = null;

            try
            {
                if (!CheckTestMode)
                {
                    try
                    {
                        if (RunChecks_ProcessInit(processCd, categoryCd, additionalInitialization))
                        {
                            //This step may actually be completely unnecessary (includes should handle finding dlls)
                            Process = (cProcess)Activator.CreateInstanceFrom(ChecksDllPath + processDllName,
                                                                              processNameSpace + "." + processClassName,
                                                                              true, 0, null,
                                                                              processConstructorArgements,
                                                                              null, null).Unwrap();

                            if (WorkspaceSessionInit() && CheckSessionInit())
                            {
                                if (Process.ExecuteChecks(ChecksDllPath, ref errorMessage))
                                {
                                    result = CheckSessionCompleted(Process.SeverityCd);
                                }
                                else
                                {
                                    CheckSessionFailed(errorMessage);

                                    HandleProcessingError(string.Format("Check execution failed: {0}", errorMessage));
                                    result = false;
                                }
                            }
                            else
                                result = false;
                        }
                        else
                        {
                            result = false;
                        }
                    }
                    finally
                    {
                        RunChecks_ProcessFini();
                    }
                }

                // For Direct Check Test do not run checks, close connections or null result catalog.
                else if (RunChecks_ProcessInit(processCd, categoryCd, additionalInitialization))
                {
                    Process = (cProcess)Activator.CreateInstanceFrom(ChecksDllPath + processDllName,
                                                                      processNameSpace + "." + processClassName,
                                                                      true, 0, null, processConstructorArgements, null, null).Unwrap();

                    if (Process.CheckProcedureInit(ChecksDllPath, ref errorMessage))
                    {
                        result = true;
                    }
                    else
                    {
                        HandleProcessingError(string.Format("Check procedure init failed: {0}", errorMessage));
                        result = false;
                    }
                }
                else
                {
                    result = false;
                }
            }
            catch (Exception ex)
            {
                HandleProcessingError(string.Format("Check execution failed: {0}  {1}", ex.Message, ex.StackTrace));
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Handles the actual running of the checks.
        /// </summary>
        /// <param name="processCd">The process code for which to process.</param>
        /// <param name="categoryCd">The category code for which to process.</param>
        /// <param name="processDllName">The name of the process DLL.</param>
        /// <param name="processNameSpace">The name space of the process.</param>
        /// <param name="processClassName">The class name of the process.</param>
        /// <param name="processConstructorArgements">The constructor arguments of the process.</param>
        /// <param name="runMode">The type of run to perform.</param>
        /// <returns>Return true if processing is successful.</returns>
        private bool RunChecks_Process(string processCd, string categoryCd,
                                       string processDllName,
                                       string processNameSpace,
                                       string processClassName,
                                       object[] processConstructorArgements,
                                       eCheckEngineRunMode runMode)
        {
            bool result;

            result = RunChecks_Process(processCd, categoryCd,
                                       processDllName, processNameSpace, processClassName, processConstructorArgements,
                                       runMode, null);

            return result;
        }


        #region Check and Workspace Sessions

        /// <summary>
        /// Completes a Check Session
        /// </summary>
        /// <param name="severityCd">The severity code resulting from processing the check session</param>
        /// <returns>Returns true if the session was successfully updated.</returns>
        private bool CheckSessionCompleted(eSeverityCd severityCd)
        {
            bool result;

            char? resultChar = null;
            string errorMessage = null;

            DbAuxContext.CheckSessionCompleted(ChkSessionId,
                                               severityCd.ToStringValue(),
                                               ref resultChar,
                                               ref errorMessage);

            if (resultChar == 'T')
            {
                result = true;
            }
            else
            {
                HandleProcessingError(string.Format("Reporting Period load failed: {0}", errorMessage));
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Fails a Check Session
        /// </summary>
        /// <returns>Returns true if the session was successfully updates.</returns>
        private bool CheckSessionFailed(string errorComment)
        {
            bool result;

            char? resultChar = null;
            string errorMessage = null;

            DbAuxContext.CheckSessionFailed(ChkSessionId,
                                            errorComment,
                                            ref resultChar,
                                            ref errorMessage);

            if (resultChar == 'T')
            {
                result = true;
            }
            else
            {
                HandleProcessingError(string.Format("Marking completed check session failed: {0}", errorMessage));
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Initializes a Check Session
        /// </summary>
        /// <returns></returns>
        private bool CheckSessionInit()
        {
            bool result;

            char? resultChar = null;
            string errorMessage = null;
            string chkSessionId = null;

            DbAuxContext.CheckSessionInit(ProcessCd,
                                          CategoryCd,
                                          MonPlanId,
                                          RptPeriodId,
                                          TestSumId,
                                          QaCertEventId,
                                          TestExtensionExemptionId,
                                          EvaluationBeganDate,
                                          EvaluationEndedDate,
                                          UserId,
                                          ref chkSessionId,
                                          ref resultChar,
                                          ref errorMessage);

            if (resultChar == 'T')
            {
                ChkSessionId = chkSessionId;
                result = true;
            }
            else
            {
                ChkSessionId = null;
                HandleProcessingError(string.Format("Reporting Period load failed: {0}", errorMessage));
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Sets WorkspaceSessionId
        /// </summary>
        /// <returns>Returns true if the Workspace Session was initialized.</returns>
        private bool WorkspaceSessionInit()
        {
            bool result;

            decimal? TempId = DbAuxConnection.GetNextSessionID();

            if (TempId.HasValue)
            {
                WorkspaceSessionId = TempId.Value;
                result = true;
            }
            else
            {
                WorkspaceSessionId = 0;
                HandleProcessingError("Unable to retrieve next worksapce session id.");
                result = false;
            }

            return result;
        }

        #endregion

        #region Process Init Methods

        /// <summary>
        /// Initializes the running of checks for the given process.
        /// </summary>
        /// <param name="processCd">The process code for which to initialize processing.</param>
        /// <param name="categoryCd">The category code for which to initialize processing.</param>
        /// <param name="additionalInitialization">Delegate for additional initialization that should occur before running checks.</param>
        /// <returns>Return true if initialization is successful.</returns>
        private bool RunChecks_ProcessInit(string processCd, string categoryCd,
                                           dAdditionalInitialization additionalInitialization)
        {
            bool result;

            ProcessCd = processCd;
            CategoryCd = categoryCd;

            result = RunChecks_ProcessInit_Connections(processCd, categoryCd) &&
                     RunChecks_ProcessInit_ResultCatalog() &&
                     RunChecks_ProcessInit_FacilityInfo() &&
                     RunChecks_ProcessInit_ReportingPeriod() &&
                     ((additionalInitialization == null) || additionalInitialization());

            return result;
        }

        /// <summary>
        /// Opens the ECMPS, ECMPS_AUX and ECMPS_WS database connections and data contexts.
        /// </summary>
        /// <param name="processCd">The process code of the checks to run.</param>
        /// <param name="categoryCd">The category code of the checks to run.</param>
        /// <returns>Returns true it the connections where opened.</returns>
        private bool RunChecks_ProcessInit_Connections(string processCd, string categoryCd)
        {
            bool result;

            string module;
            {
                if (categoryCd.IsEmpty())
                    module = string.Format("CheckEngine: {0}", processCd);
                else
                    module = string.Format("CheckEngine: {0}.{1}", processCd, categoryCd);
            }

            DbDataConnection = cDatabase.GetConnection(cDatabase.eCatalog.DATA, CommandTimeout, module);
            DbAuxConnection = cDatabase.GetConnection(cDatabase.eCatalog.AUX, CommandTimeout, module);
            DbWsConnection = cDatabase.GetConnection(cDatabase.eCatalog.WORKSPACE, CommandTimeout, module);

            result = true;

            if (DbDataConnection != null)
            {
                try
                {
                    DbDataContext = new DataDBDataContext(DbDataConnection);
                    DbDataContext.CommandTimeout = CommandTimeout;
                }
                catch (Exception ex)
                {
                    HandleProcessingError(string.Format("ECMPS data context creation failed: {0}", ex.Message));
                    result = false;
                }
            }
            else
            {
                HandleProcessingError(string.Format("ECMPS connection failed: {0}", DbDataConnection.LastError));
                result = false;
            }

            if (DbAuxConnection != null)
            {
                try
                {
                    DbAuxContext = new AuxDBDataContext(DbAuxConnection);
                    DbAuxContext.CommandTimeout = CommandTimeout;
                }
                catch (Exception ex)
                {
                    HandleProcessingError(string.Format("ECMPS_AUX data context creation failed: {0}", ex.Message));
                    result = false;
                }
            }
            else
            {
                HandleProcessingError(string.Format("ECMPS connection failed: {0}", DbAuxConnection.LastError));
                result = false;
            }

            if (DbWsConnection == null)
            {
                HandleProcessingError(string.Format("ECMPS connection failed: {0}", DbWsConnection.LastError));
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Sets Facility and First ECMPS Reporting Period information
        /// </summary>
        /// <returns>Return true if processing is successful.</returns>
        private bool RunChecks_ProcessInit_FacilityInfo()
        {
            bool result;

            if (!CheckTestMode)
            {
                if (!MonPlanId.IsEmpty())
                {
                    result = RunChecks_ProcessInit_GetFacilityInfo(eFacilityLookupType.MP, MonPlanId);
                }
                else if (!MonLocId.IsEmpty())
                    result = RunChecks_ProcessInit_GetFacilityInfo(eFacilityLookupType.ML, MonLocId);
                else if (!TestSumId.IsEmpty())
                {
                    result = RunChecks_ProcessInit_GetFacilityInfo(eFacilityLookupType.TEST, TestSumId);
                }
                else if (!QaCertEventId.IsEmpty())
                {
                    result = RunChecks_ProcessInit_GetFacilityInfo(eFacilityLookupType.QCE, QaCertEventId);
                }
                else if (!TestExtensionExemptionId.IsEmpty())
                {
                    result = RunChecks_ProcessInit_GetFacilityInfo(eFacilityLookupType.TEE, TestExtensionExemptionId);
                }
                else
                {
                    FacilityID = long.MinValue;
                    FirstEcmpsReportingPeriodId = null;
                    FirstEcmpsReportingPeriod = null;

                    result = true;
                }
            }
            else
            {
                FacilityID = long.MinValue;
                FirstEcmpsReportingPeriodId = null;
                FirstEcmpsReportingPeriod = null;

                result = true;
            }

            return result;
        }

        /// <summary>
        /// Sets Facility and First ECMPS Reporting Period information
        /// </summary>
        /// <param name="facilityLookupType">The type of the facility lookup.</param>
        /// <param name="facilityLookupId">The id to use for the facility lookup.</param>
        /// <param name="facilityId">The FAC_ID of the facility.</param>
        /// <param name="firstEcmpsReportingPeriodId">The first reporting period id of the facility.</param>
        /// <param name="firstEcmpsReportingPeriod">The first reporting period object for the facility.</param>
        /// <returns>Return true if processing is successful.</returns>
        private bool RunChecks_ProcessInit_GetFacilityInfo(eFacilityLookupType facilityLookupType,
                                                           string facilityLookupId,
                                                           out int? facilityId,
                                                           out int? firstEcmpsReportingPeriodId,
                                                           out cReportingPeriod firstEcmpsReportingPeriod)
        {
            bool result;

            string errorMessage = null;

            facilityId = null;
            firstEcmpsReportingPeriodId = null;

            if (DbDataConnection.GetFacilityInfo(facilityLookupType,
                                                 facilityLookupId,
                                                 ref facilityId,
                                                 ref firstEcmpsReportingPeriodId,
                                                 ref errorMessage))
            {
                if (facilityId.HasValue)
                {
                    firstEcmpsReportingPeriod = (firstEcmpsReportingPeriodId.HasValue)
                                              ? new cReportingPeriod(firstEcmpsReportingPeriodId.Value)
                                              : null;

                    result = true;
                }
                else
                {
                    firstEcmpsReportingPeriodId = null;
                    firstEcmpsReportingPeriod = null;

                    string label;
                    {
                        switch (facilityLookupType)
                        {
                            case eFacilityLookupType.MP: label = "MON_PLAN_ID"; break;
                            case eFacilityLookupType.ORIS: label = "ORIS_CODE"; break;
                            case eFacilityLookupType.QCE: label = "QA_CERT_EVENT.ID"; break;
                            case eFacilityLookupType.TEE: label = "TEST_EXTENSION_EXEMPTION_ID"; break;
                            case eFacilityLookupType.TEST: label = "TEST_SUM_ID"; break;
                            default: label = "Unknown Id"; break;
                        }
                    }

                    HandleProcessingError(string.Format("No FAC_ID returned by database for {0} {1}.",
                                                        label, facilityLookupId));
                    result = false;
                }
            }
            else
            {
                facilityId = null;
                firstEcmpsReportingPeriodId = null;
                firstEcmpsReportingPeriod = null;

                HandleProcessingError(errorMessage);
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Sets Facility and First ECMPS Reporting Period information
        /// </summary>
        /// <param name="facilityLookupType">The type of the facility lookup.</param>
        /// <param name="facilityLookupId">The id to use for the facility lookup.</param>
        /// <returns>Return true if processing is successful.</returns>
        private bool RunChecks_ProcessInit_GetFacilityInfo(eFacilityLookupType facilityLookupType,
                                                           string facilityLookupId)
        {
            bool result;

            int? facilityId;
            int? firstEcmpsReportingPeriodId;
            cReportingPeriod firstEcmpsReportingPeriod;

            if (RunChecks_ProcessInit_GetFacilityInfo(facilityLookupType, facilityLookupId,
                                                      out facilityId,
                                                      out firstEcmpsReportingPeriodId,
                                                      out firstEcmpsReportingPeriod))
            {
                FacilityID = (facilityId.HasValue ? facilityId.Value : long.MinValue);
                FirstEcmpsReportingPeriodId = firstEcmpsReportingPeriodId;
                FirstEcmpsReportingPeriod = firstEcmpsReportingPeriod;

                result = true;
            }
            else
            {
                FacilityID = long.MinValue;
                FirstEcmpsReportingPeriodId = null;
                FirstEcmpsReportingPeriod = null;

                result = false;
            }

            return result;
        }

        /// <summary>
        /// Sets Reporting Period information
        /// 
        /// Also resets the evaluation date range if a Reporting Period was found.
        /// </summary>
        /// <returns>Return true if processing is successful.</returns>
        private bool RunChecks_ProcessInit_ReportingPeriod()
        {
            bool result;

            if (RptPeriodId.HasValue)
            {
                char? resultChar = null;
                string errorMessage = null;

                int? year = null;
                int? quarter = null;
                string periodDescription = null;
                string periodAbbreviation = null;
                DateTime? beginDate = null;
                DateTime? endDate = null;

                DbDataContext.GetReportPeriodInfo(RptPeriodId,
                                                  ref year, ref quarter,
                                                  ref periodDescription, ref periodAbbreviation,
                                                  ref beginDate, ref endDate,
                                                  ref resultChar, ref errorMessage);

                if (resultChar == 'Y')
                {
                    try
                    {
                        ReportingPeriod = new cReportingPeriod(RptPeriodId,
                                                               year, quarter,
                                                               periodDescription, periodAbbreviation,
                                                               beginDate, endDate);

                        EvaluationBeganDate = ReportingPeriod.BeganDate;
                        EvaluationEndedDate = ReportingPeriod.EndedDate;

                        result = true;
                    }
                    catch (Exception ex)
                    {
                        ReportingPeriod = null;
                        HandleProcessingError(string.Format("Reporting Period load failed: {0}", ex.Message));
                        result = false;
                    }
                }
                else
                {
                    ReportingPeriod = null;
                    HandleProcessingError(string.Format("Reporting Period load failed: {0}", errorMessage));
                    result = false;
                }
            }
            else
                result = true;

            return result;
        }

        /// <summary>
        /// Loads the Result Catalog object's data.
        /// </summary>
        /// <returns></returns>
        private bool RunChecks_ProcessInit_ResultCatalog()
        {
            bool result;

            try
            {
                ResultCatalog = new cResultCatalog(DbAuxConnection);
                result = true;
            }
            catch (Exception ex)
            {
                HandleProcessingError(string.Format("Result Catalog load failed: {0}", ex.Message));
                result = false;
            }

            return result;
        }

        #endregion

        #region Process Fini Methods

        /// <summary>
        /// Uninitializes processing.
        /// </summary>
        private void RunChecks_ProcessFini()
        {
            ResultCatalog = null;
            RunChecks_ProcessFini_Connections();
        }

        /// <summary>
        /// Uninitializes connections.
        /// </summary>
        private void RunChecks_ProcessFini_Connections()
        {
            if ((DbDataConnection != null) && (DbDataConnection.State == ConnectionState.Open)) DbDataConnection.Close();
            DbDataConnection = null;

            if ((DbAuxConnection != null) && (DbAuxConnection.State == ConnectionState.Open)) DbAuxConnection.Close();
            DbAuxConnection = null;

            if ((DbWsConnection != null) && (DbWsConnection.State == ConnectionState.Open)) DbWsConnection.Close();
            DbWsConnection = null;
        }

        #endregion

        #region Miscellaneous Methods

        /// <summary>
        /// Clears all properties associated with particular types of runs.
        /// </summary>
        private void RunChecks_PropertiesClear()
        {
            ProcessCd = null;
            CategoryCd = null;

            EvaluationBeganDate = null;
            EvaluationEndedDate = null;

            MaximumFutureDate = DateTime.Now.AddYears(1);

            MonLocId = null;
            MonPlanId = null;
            RptPeriodId = null;
            ORISCode = null;

            TestSumId = null;
            QaCertEventId = null;
            TestExtensionExemptionId = null;

            ThisTable = null;

            FacilityID = long.MinValue;
            FirstEcmpsReportingPeriodId = null;
            FirstEcmpsReportingPeriod = null;

            ReportingPeriod = null;

            WorkspaceSessionId = 0;
        }

        #endregion

        #endregion

        #endregion

        #region Public Methods: Miscellaneous

        /// <summary>
        /// Handles processing errors generated during a check session.
        /// </summary>
        /// <param name="message">The message to handle.</param>
        public void HandleProcessingError(string message)
        {
            if (message.HasValue())
            {
                if (CheckEngineErrors.IsEmpty())
                    CheckEngineErrors = message;
                else
                    CheckEngineErrors += Environment.NewLine + message;
            }
        }

        #endregion

        #region Public Properties

        #region Constructor Initialized

        /// <summary>
        /// The ECMPS_AUX database connection string.
        /// </summary>
        public string AuxConnectionString
        {
            get { return cDatabase.AuxConnectionString; }
            set { cDatabase.AuxConnectionString = value; }
        }

        /// <summary>
        /// The path of the checks DLLs.
        /// </summary>
        public string ChecksDllPath { get; set; }

        /// <summary>
        /// The command timeout used by CheckEngine SQL commands.
        /// </summary>
        public int CommandTimeout { get; set; }

        /// <summary>
        /// The ECMPS database connection string.
        /// </summary>
        public string DataConnectionString
        {
            get { return cDatabase.DataConnectionString; }
            set { cDatabase.DataConnectionString = value; }
        }

        /// <summary>
        /// The current user id.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// The ECMPS_WS database connection string.
        /// </summary>
        public string WorkspaceConnectionString
        {
            get { return cDatabase.WorkspaceConnectionString; }
            set { cDatabase.WorkspaceConnectionString = value; }
        }

        #endregion


        #region Specific Run Initialized

        /// <summary>
        /// The current category
        /// </summary>
        public string CategoryCd { get; private set; }

        /// <summary>
        /// the evaluation BEGIN_DATE
        /// </summary>
        public DateTime? EvaluationBeganDate { get; protected set; }

        /// <summary>
        /// the evaluation END_DATE
        /// </summary>
        public DateTime? EvaluationEndedDate { get; protected set; }

        /// <summary>
        /// Used to limit this distance of dates in the future.
        /// </summary>
        public DateTime MaximumFutureDate { get; protected set; }

        /// <summary>
        /// The current MON_LOC_ID
        /// </summary>
        public string MonLocId { get; private set; }

        /// <summary>
        /// The MON_PLAN_ID
        /// </summary>
        public string MonPlanId { get; private set; }

        /// <summary>
        /// The QA_CERT_EVENT_ID
        /// </summary>
        public string QaCertEventId { get; private set; }

        /// <summary>
        /// The current process
        /// </summary>
        public string ProcessCd { get; private set; }

        /// <summary>
        /// The current RPT_PERIOD_ID
        /// </summary>
        public int? RptPeriodId { get; private set; }

        /// <summary>
        /// TEST_EXTENSION_EXEMPTION_ID
        /// </summary>
        public string TestExtensionExemptionId { get; private set; }

        /// <summary>
        /// The TEST_SUM_ID
        /// </summary>
        public string TestSumId { get; private set; }

        /// <summary>
        /// The ORIS_CODE (for Import Processes)
        /// </summary>
        public string ORISCode { get; private set; }

        /// <summary>
        /// ThisTable?
        /// </summary>
        public DataTable ThisTable { get; private set; }

        #endregion


        #region Run Initialized

        /// <summary>
        /// The message indicating why a run failed.
        /// </summary>
        public string CheckEngineErrors { get; private set; }

        /// <summary>
        /// The CHK_SESSION_ID for last evaluation
        /// </summary>
        public string ChkSessionId { get; private set; }

        /// <summary>
        /// The FAC_ID
        /// </summary>
        public long FacilityID { get; private set; }

        /// <summary>
        /// The First ECMPS Reporting Period for the facility.
        /// Null if one does not exist.
        /// </summary>
        public cReportingPeriod FirstEcmpsReportingPeriod { get; protected set; }

        /// <summary>
        /// The First ECMPS Reporting Period Id for the facility.
        /// Null if one does not exist.
        /// </summary>
        public int? FirstEcmpsReportingPeriodId { get; protected set; }

        /// <summary>
        /// The cProcess object for a Check Engine run.
        /// </summary>
        public cProcess Process { get; private set; }

        /// <summary>
        /// Reporting Period for an evaluation run.
        /// </summary>
        public cReportingPeriod ReportingPeriod { get; protected set; }

        /// <summary>
        /// The cResultCatalog object.
        /// </summary>
        public cResultCatalog ResultCatalog { get; private set; }

        /// <summary>
        /// Indicates the type of check run to perform
        /// </summary>
        public eCheckEngineRunMode RunMode { get; private set; }

        /// <summary>
        /// The session id used in workspace rows populated by an evaluation.
        /// </summary>
        public decimal WorkspaceSessionId { get; private set; }

        #endregion


        #region By Process Reference

        /// <summary>
        /// The SEVERITY_CD as determined by the checks
        /// </summary>
        public eSeverityCd SeverityCd { get { return ((Process != null) ? Process.SeverityCd : eSeverityCd.EXCEPTION); } }

        #endregion


        #region By General Reference

        /// <summary>
        /// Determines whether run is for a Direct Check Test
        /// </summary>
        public bool CheckTestMode { get { return (RunMode == eCheckEngineRunMode.CheckTestInit); } }

        /// <summary>
        /// Is Check Engine running in debug mode?
        /// </summary>
        public bool DebugMode { get { return (RunMode == eCheckEngineRunMode.Debug); } }

        /// <summary>
        /// The defaulted evaluation BEGIN_DATE
        /// </summary>
        public DateTime EvalDefaultedBeganDate { get { return EvaluationBeganDate.Default(DateTime.MinValue); } }

        /// <summary>
        /// The defaulted evaluation END_DATE
        /// </summary>
        public DateTime EvalDefaultedEndedDate { get { return EvaluationEndedDate.Default(DateTime.MaxValue); } }

        /// <summary>
        /// The report period quarter.
        /// </summary>
        public int? RptPeriodQuarter { get { return (ReportingPeriod.HasValue() ? ReportingPeriod.Quarter.AsInteger() : (int?)null); } }

        /// <summary>
        /// The report period year.
        /// </summary>
        public int? RptPeriodYear { get { return (ReportingPeriod.HasValue() ? ReportingPeriod.Year : (int?)null); } }

        #endregion


        #region Data Connectivity

        /// <summary>
        /// Connection to the ECMPS_AUX database
        /// </summary>
        public cDatabase DbAuxConnection { get; private set; }

        /// <summary>
        /// Data context for the ECMPS_AUX database
        /// </summary>
        public AuxDBDataContext DbAuxContext { get; private set; }

        /// <summary>
        /// Connection to ECMPS database
        /// </summary>
        public cDatabase DbDataConnection { get; private set; }

        /// <summary>
        /// Data context for the ECMPS database
        /// </summary>
        public DataDBDataContext DbDataContext { get; private set; }

        /// <summary>
        /// Connection to the ECMPS_WS database
        /// </summary>
        public cDatabase DbWsConnection { get; private set; }

        #endregion


        #region Testing Based Properties w/ Supporting Methods and Proparties

        /// <summary>
        /// Returns the current date, which testing may override.
        /// </summary>
        public DateTime NowDate { get { return NowDateTime.Date; } }

        /// <summary>
        /// Returns the current time, which testing may override.
        /// </summary>
        public virtual DateTime NowDateTime { get { return DateTime.Now.AddTicks(NowOffset); } }


        #region Supporting Methods

        private long NowOffset = 0;

        /// <summary>
        /// Sets the NowOffset ticks based on the difference between the passed date and the current date.
        /// </summary>
        /// <param name="newNowDateTime">The base new date/time to use as a reference in generating NowDateTime</param>
        public void SetNowOffset(DateTime newNowDateTime)
        {
            NowOffset = newNowDateTime.Ticks - DateTime.Now.Ticks;
        }

        #endregion

        #endregion

        #endregion

        #region Public Methods: Used Too Often to Move

        /// <summary>
        /// Format an exception nicely
        /// </summary>
        /// <param name="ex">the exception in question</param>
        /// <param name="CheckProcedure">the check procedure that threw/caught the exception</param>
        /// <returns>the nicely formated error message</returns>
        public string FormatError(Exception ex, string CheckProcedure)
        {
            return string.Format("Failed executing method '{0}'.\n{1}\n{2}", CheckProcedure, ex.Message, ex.StackTrace);
        }


        /// <summary>
        /// Format an exception nicely
        /// </summary>
        /// <param name="ex">the exception in question</param>
        /// <returns>the nicely formated error message</returns>
        public string FormatError(Exception ex)
        {
            return string.Format("Failed executing method '{0}'.\n{1}\n{2}", ex.TargetSite.Name, ex.Message, ex.StackTrace);
        }

        #endregion

    }
}