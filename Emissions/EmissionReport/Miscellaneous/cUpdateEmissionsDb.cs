using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

using ECMPS.Checks.DatabaseAccess;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Extensions;

using ECMPS.DM.Definitions;
using ECMPS.DM.Miscellaneous;
using ECMPS.DM.Utilities;


namespace ECMPS.Checks.EmissionsReport
{
    public class cUpdateEmissionsDb
    {

        #region Public Constructors

        /// <summary>
        /// Creates a DB object with methods to handle DM.cUpdateEmissions calls.
        /// </summary>
        /// <param name="dbData">The ECMPS schema cDatabase object.</param>
        /// <param name="dbAux">The ECMPS Aux schema cDatabase object.</param>
        /// <param name="dbWs">The ECMPS Workspace schema cDatabase object.</param>
        public cUpdateEmissionsDb(cDatabase dbData, cDatabase dbAux, cDatabase dbWs)
        {
            CheckSessionId = null;

            DbData = dbData;
            DbAux = dbAux;
            DbWs = dbWs;

            ReportFailureCallback = null;
            DisplayLoggedErrorCallback = null;

            CheckLogTable = null;
            ResultForEveryHour = null;
            ResultForIndividualHours = null;
        }

        /// <summary>
        /// Creates a DB object with methods to handle DM.cUpdateEmissions calls.
        /// </summary>
        /// <param name="dbData">The ECMPS schema cDatabase object.</param>
        /// <param name="dbAux">The ECMPS Aux schema cDatabase object.</param>
        /// <param name="dbWs">The ECMPS Workspace schema cDatabase object.</param>
        /// <param name="reportFailureCallback">The processing failure callback used to show the process failed.</param>
        public cUpdateEmissionsDb(cDatabase dbData, cDatabase dbAux, cDatabase dbWs,
                                  dErrorCallback reportFailureCallback)
        {
            CheckSessionId = null;

            DbData = dbData;
            DbAux = dbAux;
            DbWs = dbWs;

            ReportFailureCallback = reportFailureCallback;
            DisplayLoggedErrorCallback = null;

            CheckLogTable = null;
            ResultForEveryHour = null;
            ResultForIndividualHours = null;
        }

        /// <summary>
        /// Creates a DB object with methods to handle DM.cUpdateEmissions calls.
        /// </summary>
        /// <param name="dbData">The ECMPS schema cDatabase object.</param>
        /// <param name="dbAux">The ECMPS Aux schema cDatabase object.</param>
        /// <param name="dbWs">The ECMPS Workspace schema cDatabase object.</param>
        /// <param name="reportFailureCallback">The processing failure callback used to show the process failed.</param>
        /// <param name="errorDisplayCallback">The error display callback used to display errors.</param>
        public cUpdateEmissionsDb(cDatabase dbData, cDatabase dbAux, cDatabase dbWs,
                                  dErrorCallback reportFailureCallback,
                                  dErrorCallback errorDisplayCallback)
        {
            CheckSessionId = null;

            DbData = dbData;
            DbAux = dbAux;
            DbWs = dbWs;

            ReportFailureCallback = reportFailureCallback;
            DisplayLoggedErrorCallback = errorDisplayCallback;

            CheckLogTable = null;
            ResultForEveryHour = null;
            ResultForIndividualHours = null;
        }

        #endregion


        #region Public Delegate

        public delegate void dErrorCallback(string errorMessage);

        #endregion


        #region Public Types

        /// <summary>
        /// Stores the information for a check catalog result.
        /// </summary>
        public class cCheckCatalogResult
        {

            /// <summary>
            /// Creates a check catalog result object.
            /// </summary>
            /// <param name="checkCatalogResultId">The check catalog result id of the result.</param>
            /// <param name="checkResult">The check result of the result.</param>
            /// <param name="severityCd">The severity code of the result.</param>
            public cCheckCatalogResult(int? checkCatalogResultId, string checkResult, string severityCd)
            {
                CheckCatalogResultId = checkCatalogResultId;
                CheckResult = checkResult;
                SeverityCd = severityCd;
            }

            /// <summary>
            /// The severity code of the result.
            /// </summary>
            public int? CheckCatalogResultId { get; private set; }

            /// <summary>
            /// The check result value of the result.
            /// </summary>
            public string CheckResult { get; private set; }

            /// <summary>
            /// The severity code of the result.
            /// </summary>
            public string SeverityCd { get; private set; }

        }

        #endregion


        #region Public Properties

        /// <summary>
        /// The Check Session Id used to populate logged errors.
        /// </summary>
        public string CheckSessionId { get; private set; }

        /// <summary>
        /// The table into which errors are logged.
        /// </summary>
        public DataTable CheckLogTable { get; private set; }

        /// <summary>
        /// The ECMPS schema cDatabase object.
        /// </summary>
        public cDatabase DbData { get; private set; }

        /// <summary>
        /// The ECMPS Aux schema cDatabase object.
        /// </summary>
        public cDatabase DbAux { get; private set; }

        /// <summary>
        /// The ECMPS Workspace schema cDatabase object.
        /// </summary>
        public cDatabase DbWs { get; private set; }

        /// <summary>
        /// Error display callback for testing form.
        /// </summary>
        public dErrorCallback DisplayLoggedErrorCallback { get; private set; }

        /// <summary>
        /// Used to report when processing failed to complete normally.
        /// </summary>
        public dErrorCallback ReportFailureCallback { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public cCheckCatalogResult ResultForEveryHour { get; private set; }

        public cCheckCatalogResult ResultForIndividualHours { get; private set; }

        #endregion


        #region Public Methods

        /// <summary>
        /// Loads the Monitor D&M Emissions tables, and returns the data needed to
        /// load the Unit Hour table.
        /// </summary>
        /// <param name="monPlanId">MON_PLAN_ID of the emission report.</param>
        /// <param name="rptPeriodId">RPT_PERIOD_ID of the emission report.</param>
        /// <param name="unitInfo"></param>
        /// <param name="locationInfo"></param>
        /// <param name="factorFormulaeArray"></param>
        /// <param name="errorMessage">The error message if an error occurs.</param>
        /// <returns>True if the SP executed without error.</returns>
        public bool GetFactorFormulaeArray(string monPlanId, int rptPeriodId,
                                           cUnitInfo[] unitInfo, cLocationInfo[] locationInfo,
                                           out cFactorFormulae[] factorFormulaeArray,
                                           ref string errorMessage)
        {
            bool result;

            try
            {
                DbData.CreateStoredProcedureCommand("DmEm.GetApportionmentData");

                DbData.AddInputParameter("@V_MON_PLAN_ID", monPlanId);
                DbData.AddInputParameter("@V_RPT_PERIOD_ID", rptPeriodId);
                DbData.AddOutputParameterString("@V_RESULT", 1);
                DbData.AddOutputParameterString("@V_ERROR_MSG", 200);

                DataSet dataSet = DbData.GetDataSet();

                result = (DbData.GetParameterString("@V_RESULT") == "T");
                errorMessage = DbData.GetParameterString("@V_ERROR_MSG");

                if (result)
                {
                    DataTable apportionmentTable = dataSet.Tables[0];

                    if (apportionmentTable.Rows.Count == 1)
                    {
                        DataTable apportionmentRangeTable = dataSet.Tables[1];
                        DataTable apportionmentDataTable = dataSet.Tables[2];
                        DataTable apportionmentFormulaeTable = dataSet.Tables[3];
                        DataTable apportionmentConditionTable = dataSet.Tables[4];
                        DataTable apportionmentSubtractiveTable = dataSet.Tables[5];

                        DataView apportionmentRangeView = apportionmentRangeTable.DefaultView;
                        {
                            apportionmentRangeView.Sort = "Begin_DateHour, End_DateHour";
                        }

                        factorFormulaeArray = new cFactorFormulae[apportionmentDataTable.Rows.Count];
                        int factorFormulaeArrayDex = 0;
                        {
                            for (int apportionmentRangeDex = 0; apportionmentRangeDex < apportionmentRangeView.Count; apportionmentRangeDex++)
                            {
                                DataRowView apportionmentRangeRow = apportionmentRangeView[apportionmentRangeDex];
                                int? apportRangeId = apportionmentRangeRow["Apport_Range_Id"].AsInteger();
                                DateTime beginDateHour = apportionmentRangeRow["Begin_DateHour"].AsDateTime(DateTime.MinValue);
                                DateTime endDateHour = apportionmentRangeRow["End_DateHour"].AsDateTime(DateTime.MaxValue);


                                DataView apportionmentDataView = new DataView(apportionmentDataTable,
                                                                              string.Format("Apport_Range_Id = {0}", apportRangeId),
                                                                              "Evaluation_Order",
                                                                              DataViewRowState.CurrentRows);


                                foreach (DataRowView apportionmentDataRow in apportionmentDataView)
                                {
                                    int? apportDataId = apportionmentDataRow["Apport_Data_Id"].AsInteger();

                                    // Init Factor Formula Object
                                    cFactorFormulae factorFormulae = new cFactorFormulae(unitInfo, locationInfo,
                                                                                         beginDateHour.Date,
                                                                                         beginDateHour.Hour,
                                                                                         endDateHour.Date,
                                                                                         endDateHour.Hour);

                                    // Set Operating Conditions
                                    {
                                        DataView apportionmentConditionView = new DataView(apportionmentConditionTable,
                                                                                           string.Format("Apport_Data_Id = {0}", apportDataId),
                                                                                           "Target_Tag",
                                                                                           DataViewRowState.CurrentRows);

                                        if (apportionmentConditionView.Count > 0)
                                        {
                                            foreach (DataRowView apportionmentConditionRow in apportionmentConditionView)
                                            {
                                                factorFormulae.UpdateOperatingCondition(apportionmentConditionRow["Target_Tag"].AsString(), apportionmentConditionRow["Operating_Ind"].AsBoolean());
                                            }
                                        }
                                    }


                                    // Add Formulae
                                    {
                                        DataView apportionmentFormulaView = new DataView(apportionmentFormulaeTable,
                                                                                         string.Format("Apport_Data_Id = {0}", apportDataId),
                                                                                         "Monitor_Tag, Unit_Tag",
                                                                                         DataViewRowState.CurrentRows);

                                        {
                                            foreach (DataRowView apportionmnetFormulaRow in apportionmentFormulaView)
                                            {
                                                eComplexParameter? complexParameter;
                                                {
                                                    switch (apportionmnetFormulaRow["Pollutant_Cd"].AsString())
                                                    {
                                                        case "CO2M": complexParameter = eComplexParameter.Co2m; break;
                                                        case "SO2M": complexParameter = eComplexParameter.So2m; break;
                                                        case "NOXM": complexParameter = eComplexParameter.Noxm; break;
                                                        case "HGM": complexParameter = eComplexParameter.Hgm; break;
                                                        case "HCLM": complexParameter = eComplexParameter.Hclm; break;
                                                        case "HFM": complexParameter = eComplexParameter.Hfm; break;
                                                        default: complexParameter = null; break;
                                                    }
                                                }

                                                if (complexParameter.HasValue)
                                                    factorFormulae.SetFormula(complexParameter.Value,
                                                                              apportionmnetFormulaRow["Unit_Tag"].AsString(),
                                                                              apportionmnetFormulaRow["Monitor_Tag"].AsString(),
                                                                              apportionmnetFormulaRow["Hi_Load_Formula"].AsString(),
                                                                              apportionmnetFormulaRow["Op_Time_Formula"].AsString());
                                                else
                                                    factorFormulae.SetFormula(apportionmnetFormulaRow["Unit_Tag"].AsString(),
                                                                              apportionmnetFormulaRow["Monitor_Tag"].AsString(),
                                                                              apportionmnetFormulaRow["Hi_Load_Formula"].AsString(),
                                                                              apportionmnetFormulaRow["Op_Time_Formula"].AsString());
                                            }
                                        }
                                    }

                                    // Add Reduce By 
                                    {
                                        DataView apportionmentSubtractiveView = new DataView(apportionmentSubtractiveTable,
                                                                                             string.Format("Apport_Data_Id = {0}", apportDataId),
                                                                                             "Target_Tag, Subtracting_Tag, Pollutant_Cd",
                                                                                             DataViewRowState.CurrentRows);

                                        {
                                            Dictionary<string, Dictionary<string, List<String>>> subtractiveDict = new Dictionary<string, Dictionary<string, List<String>>>(); // Subtracted Tag, Pollutant, Subtracting Tag

                                            foreach (DataRowView apportionmnetSubtractiveRow in apportionmentSubtractiveView)
                                            {
                                                string targetTag = apportionmnetSubtractiveRow["Target_Tag"].AsString();
                                                string subtractingTag = apportionmnetSubtractiveRow["Subtracting_Tag"].AsString();

                                                string pollutantCd;
                                                {
                                                    if (apportionmnetSubtractiveRow["Pollutant_Cd"] != DBNull.Value)
                                                        pollutantCd = apportionmnetSubtractiveRow["Pollutant_Cd"].AsString();
                                                    else
                                                        pollutantCd = "ALL";
                                                }

                                                if (!subtractiveDict.ContainsKey(targetTag))
                                                {
                                                    subtractiveDict.Add(targetTag, new Dictionary<string, List<String>>());
                                                }

                                                if (!subtractiveDict[targetTag].ContainsKey(pollutantCd))
                                                {
                                                    subtractiveDict[targetTag].Add(pollutantCd, new List<String>());
                                                }

                                                if (!subtractiveDict[targetTag][pollutantCd].Contains(subtractingTag))
                                                {
                                                    subtractiveDict[targetTag][pollutantCd].Add(subtractingTag);
                                                }
                                            }

                                            foreach (string targetTag in subtractiveDict.Keys)
                                                foreach (string pollutantCd in subtractiveDict[targetTag].Keys)
                                                {
                                                    List<String> reduceByTagList = subtractiveDict[targetTag][pollutantCd];
                                                    string[] reduceByTags = new string[reduceByTagList.Count];

                                                    for (int reduceByDex = 0; reduceByDex < reduceByTagList.Count; reduceByDex++)
                                                        reduceByTags[reduceByDex] = reduceByTagList[reduceByDex];

                                                    switch (pollutantCd)
                                                    {
                                                        case "ALL": factorFormulae.AddReduceBy(targetTag, reduceByTags); break;
                                                        case "CO2M": factorFormulae.AddReduceBy(eComplexParameter.Co2m, targetTag, reduceByTags); break;
                                                        case "HCLM": factorFormulae.AddReduceBy(eComplexParameter.Hclm, targetTag, reduceByTags); break;
                                                        case "HFM": factorFormulae.AddReduceBy(eComplexParameter.Hfm, targetTag, reduceByTags); break;
                                                        case "HGM": factorFormulae.AddReduceBy(eComplexParameter.Hgm, targetTag, reduceByTags); break;
                                                        case "NOXM": factorFormulae.AddReduceBy(eComplexParameter.Noxm, targetTag, reduceByTags); break;
                                                        case "SO2M": factorFormulae.AddReduceBy(eComplexParameter.So2m, targetTag, reduceByTags); break;
                                                    }
                                                }
                                        }
                                    }

                                    factorFormulaeArray[factorFormulaeArrayDex] = factorFormulae;
                                    factorFormulaeArrayDex++;
                                }
                            }
                        }
                    }
                    else
                        factorFormulaeArray = null;
                }
                else
                    factorFormulaeArray = null;
            }
            catch (Exception ex)
            {
                factorFormulaeArray = null;
                errorMessage = ex.Message;
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Creates the table to store logged error items and sets the CHECK_CATALOG_ID
        /// to use in the table.
        /// </summary>
        /// <param name="checkCatalogId">The CHECK_SESSION_ID value used to populate the logged errors.</param>
        /// <param name="errorMessage">Message returned if initialization failed.</param>
        /// <returns>True if the initialization was successful.</returns>
        public bool InitErrorLogging(string checkSessionId, ref string errorMessage)
        {
            bool result;

            CheckSessionId = checkSessionId;

            try
            {
                CheckLogTable = DbAux.GetDataTable("SELECT * FROM dbo.CHECK_LOG WHERE NULL = NULL");

                InitErrorLogging_Results();

                result = true;
            }
            catch (Exception ex)
            {
                errorMessage = string.Format("{0} (IntErrorLogging)", ex.Message);
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Delegate used for methods that log errors.
        /// </summary>
        /// <param name="message">The error message to log.</param>
        /// <param name="detail">Additional detail about the error.</param>
        /// <param name="opDate">The op date of the data that caused the error.</param>
        /// <param name="opHour">The op hour of the data that caused the error.</param>
        /// <param name="locationKey">The location key of the data that caused the error.</param>
        /// <param name="erroredMethod">The method that caused the error.</param>
        /// <returns>Returns false if the error was not logged.</returns>
        public bool LogError(string message, string detail,
                             DateTime? opDate, int? opHour, string locationKey,
                             string erroredMethod)
        {
            bool result;

            if (DisplayLoggedErrorCallback != null)
                DisplayLoggedErrorCallback(LogErrorFormat(message, detail, opDate, opHour, locationKey, erroredMethod));

            try
            {
                DataRow checkLogRow = CheckLogTable.NewRow();

                // Replace this with a default in database
                //checkLogRow["CHK_LOG_ID"] = CheckLogID;

                checkLogRow["CHK_SESSION_ID"] = CheckSessionId.DbValue();
                checkLogRow["RESULT_MESSAGE"] = message.DbValue();
                checkLogRow["CHK_LOG_COMMENT"] = LogErrorFormat(message, detail, erroredMethod).DbValue();
                checkLogRow["MON_LOC_ID"] = locationKey.DbValue();
                checkLogRow["OP_BEGIN_DATE"] = opDate.DbValue();
                checkLogRow["OP_BEGIN_HOUR"] = opHour.DbValue();
                checkLogRow["OP_END_DATE"] = opDate.DbValue();
                checkLogRow["OP_END_HOUR"] = opHour.DbValue();

                cCheckCatalogResult checkCatalogResult;
                {
                    if (opDate.HasValue || opHour.HasValue)
                        checkCatalogResult = ResultForIndividualHours;
                    else
                        checkCatalogResult = ResultForEveryHour;
                }

                checkLogRow["CHECK_CATALOG_RESULT_ID"] = checkCatalogResult.CheckCatalogResultId;
                checkLogRow["CHECK_RESULT"] = checkCatalogResult.CheckResult;
                checkLogRow["SEVERITY_CD"] = checkCatalogResult.SeverityCd;

                checkLogRow["BEGIN_DATE"] = DateTime.Now;

                CheckLogTable.Rows.Add(checkLogRow);

                result = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("{0} (LogError)", ex.Message));
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Update the check log, set apportionment type, and set emissions created to 'N'.
        /// </summary>
        /// <param name="dmEmissionsId">The DM_EMISSIONS_ID of the update.</param>
        /// <param name="apportionmentType">The apportionment type of the update.</param>
        public void UpdateFailure(string dmEmissionsId,
                                  eApportionmentType? apportionmentType)
        {
            string errorMessage = null;

            if (CheckLogUpdate(ref errorMessage))
            {
                if (dmEmissionsId != null)
                {
                    try
                    {
                        DbData.CreateStoredProcedureCommand("DmEm.UpdateFailure");

                        DbData.AddInputParameter("@V_DM_EMISSIONS_ID", dmEmissionsId);
                        DbData.AddInputParameter("@V_APPORTIONMENT_TYPE_CD", apportionmentType.DbCode());
                        DbData.AddOutputParameterString("@V_RESULT", 1);
                        DbData.AddOutputParameterString("@V_ERROR_MSG", 200);

                        DbData.ExecuteNonQuery();

                        if (DbData.GetParameterString("@V_RESULT") != "T")
                        {
                            errorMessage = DbData.GetParameterString("@V_ERROR_MSG");

                            if ((ReportFailureCallback != null) && errorMessage.HasValue())
                                ReportFailureCallback(errorMessage);
                        }
                    }
                    catch (Exception ex)
                    {
                        if (ReportFailureCallback != null)
                            ReportFailureCallback(string.Format("{0} (UpdateFailure)", ex.Message));
                    }
                }
            }
            else
            {
                if (ReportFailureCallback != null)
                    ReportFailureCallback(errorMessage);
            }
        }

        /// <summary>
        /// Loads the Monitor D&M Emissions tables, and returns the data needed to
        /// load the Unit Hour table.
        /// </summary>
        /// <param name="monPlanId">MON_PLAN_ID of the emission report.</param>
        /// <param name="rptPeriodId">RPT_PERIOD_ID of the emission report.</param>
        /// <param name="userId">The user id to store in created rows.</param>
        /// <param name="dmEmissionsId">The DM_EMISSIONS_ID to store in created rows.</param>
        /// <param name="dataSource">The data source to store in created rows.</param>
        /// <param name="isMatsEmissionReport">Indicates whether the emission report is a MATS report.</param>
        /// <param name="locationTable">Table of location information.</param>
        /// <param name="locationTypeCountTable">Table with location type count row.</param>
        /// <param name="locationLinkSpanCountTable">Table with location link type span count row.</param>
        /// <param name="locationLinkActiveTable">Table with location link type active count row.</param>
        /// <param name="specialMethodCountTable">Table with special method type count row.</param>
        /// <param name="monitorHourTable">Montitor hour data.</param>
        /// <param name="sqlTransaction"The transaction to use with the command, null for none.></param>
        /// <param name="errorMessage">The error message if an error occurs.</param>
        /// <returns>True if the SP executed without error.</returns>
        public bool UpdateInit(string monPlanId, int rptPeriodId, string userId,
                               out string dmEmissionsId,
                               out string dataSource,
                               out bool? isMatsEmissionReport,
                               out DataTable locationTable,
                               out DataTable rptPeriodInfoTable,
                               out DataTable locationTypeCountTable,
                               out DataTable locationLinkSpanCountTable,
                               out DataTable locationLinkActiveTable,
                               out DataTable specialMethodCountTable,
                               out DataTable monitorHourTable,
                               ref string errorMessage)
        {
            bool result;

            try
            {
                DbData.CreateStoredProcedureCommand("DmEm.UpdateInit");

                DbData.AddInputParameter("@V_MON_PLAN_ID", monPlanId);
                DbData.AddInputParameter("@V_RPT_PERIOD_ID", rptPeriodId);
                DbData.AddInputParameter("@V_USERID", userId);
                DbData.AddOutputParameterString("@V_DM_EMISSIONS_ID", 45);
                DbData.AddOutputParameterString("@V_DATA_SOURCE", 35);
                DbData.AddOutputParameterInt("@V_IS_MATS_EM_RPT");
                DbData.AddOutputParameterString("@V_RESULT", 1);
                DbData.AddOutputParameterString("@V_ERROR_MSG", 200);

                DataSet dataSet = DbData.GetDataSet();

                dmEmissionsId = DbData.GetParameterString("@V_DM_EMISSIONS_ID");
                dataSource = DbData.GetParameterString("@V_DATA_SOURCE");
                isMatsEmissionReport = (DbData.GetParameterInt("@V_IS_MATS_EM_RPT") == 1); //TODO (EC-3519): Ensure this works with the GetParameterInt type change.
                result = (DbData.GetParameterString("@V_RESULT") == "T");
                errorMessage = DbData.GetParameterString("@V_ERROR_MSG");

                if (result)
                {
                    locationTable = dataSet.Tables[0];
                    locationTable.TableName = "Location";
                    rptPeriodInfoTable = dataSet.Tables[1];
                    rptPeriodInfoTable.TableName = "RptPeriodInfo";
                    locationTypeCountTable = dataSet.Tables[2];
                    locationTypeCountTable.TableName = "LocationTypeCount";
                    locationLinkSpanCountTable = dataSet.Tables[3];
                    locationLinkSpanCountTable.TableName = "LocationLinkSpanCount";
                    locationLinkActiveTable = dataSet.Tables[4];
                    locationLinkActiveTable.TableName = "LocationLinkActiveCount";
                    specialMethodCountTable = dataSet.Tables[5];
                    specialMethodCountTable.TableName = "SpecialMethodCount";
                    monitorHourTable = dataSet.Tables[6];
                    monitorHourTable.TableName = "MonitorHour";
                }
                else
                {
                    locationTable = null;
                    rptPeriodInfoTable = null;
                    locationTypeCountTable = null;
                    locationLinkSpanCountTable = null;
                    locationLinkActiveTable = null;
                    specialMethodCountTable = null;
                    monitorHourTable = null;
                }
            }
            catch (Exception ex)
            {
                dmEmissionsId = null;
                dataSource = null;
                isMatsEmissionReport = null;
                locationTable = null;
                rptPeriodInfoTable = null;
                locationTypeCountTable = null;
                locationLinkSpanCountTable = null;
                locationLinkActiveTable = null;
                specialMethodCountTable = null;
                monitorHourTable = null;

                errorMessage = ex.Message;
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Deletes existing emissions and creates a DM_EMISSIONS row.
        /// </summary>
        /// <param name="monPlanId">MON_PLAN_ID of the emission report.</param>
        /// <param name="rptPeriodId">RPT_PERIOD_ID of the emission report.</param>
        /// <param name="userId">The user id to store in created rows.</param>
        /// <param name="errorMessage">The error message if an error occurs.</param>
        /// <returns>True if the SP executed without error.</returns>
        public bool UpdateInit_Setup(string monPlanId, int rptPeriodId, string userId,
                                     ref string errorMessage)
        {
            bool result;

            try
            {
                DbData.CreateStoredProcedureCommand("DmEm.UpdateInit_Setup");

                DbData.AddInputParameter("@V_MON_PLAN_ID", monPlanId);
                DbData.AddInputParameter("@V_RPT_PERIOD_ID", rptPeriodId);
                DbData.AddInputParameter("@V_USERID", userId);
                DbData.AddOutputParameterString("@V_DM_EMISSIONS_ID", 45);
                DbData.AddOutputParameterString("@V_RESULT", 1);
                DbData.AddOutputParameterString("@V_ERROR_MSG", 200);

                DbData.ExecuteNonQuery();

                errorMessage = DbData.GetParameterString("@V_ERROR_MSG");
                result = (DbData.GetParameterString("@V_RESULT") == "T");
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Deletes existing emissions and creates a DM_EMISSIONS row.
        /// </summary>
        /// <param name="monPlanId">MON_PLAN_ID of the emission report.</param>
        /// <param name="rptPeriodId">RPT_PERIOD_ID of the emission report.</param>
        /// <param name="userId">The user id to store in created rows.</param>
        /// <param name="errorMessage">The error message if an error occurs.</param>
        /// <returns>True if the SP executed without error.</returns>
        public bool UpdateReset(string monPlanId, int rptPeriodId, string userId,
                                     ref string errorMessage)
        {
            bool result;

            try
            {
                DbData.CreateStoredProcedureCommand("DmEm.UpdateReset");

                DbData.AddInputParameter("@V_MON_PLAN_ID", monPlanId);
                DbData.AddInputParameter("@V_RPT_PERIOD_ID", rptPeriodId);
                DbData.AddInputParameter("@V_USERID", userId);
                DbData.AddOutputParameterString("@V_RESULT", 1);
                DbData.AddOutputParameterString("@V_ERROR_MSG", 200);

                DbData.ExecuteNonQuery();

                errorMessage = DbData.GetParameterString("@V_ERROR_MSG");
                result = (DbData.GetParameterString("@V_RESULT") == "T");
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Delegate for the method called to update the apportionment type, unit hour data 
        /// and check log.
        /// </summary>
        /// <param name="dmEmissionsId">The DM_EMISSIONS_ID of the update.</param>
        /// <param name="apportionmentType">The apportionment type of the update.</param>
        /// <param name="isMatsEmissionReport">Indicates whether the emission report is a MATS report.</param>
        /// <param name="dmEmissionsIdArray">The DM_EMISSIONS_ID update array.</param>
        /// <param name="unitKeyArray">The UNIT_ID update array.</param>
        /// <param name="opDateArray">The op date update array.</param>
        /// <param name="opHourArray">The op hour update array.</param>
        /// <param name="opTimeArray">The op time update array.</param>
        /// <param name="gLoadArray">The g-load update array.</param>
        /// <param name="mLoadArray">The MATS load update array.</param>
        /// <param name="sLoadArray">The s-load update array.</param>
        /// <param name="tLoadArray">The t-load update array.</param>
        /// <param name="hitArray">The HIT update array.</param>
        /// <param name="hitMeasureArray">The HIT hour measure update array.</param>
        /// <param name="so2mArray">The SO2M update array.</param>
        /// <param name="so2mMeasureArray">The SO2M hour measure update array.</param>
        /// <param name="so2rArray">The SO2R update array.</param>
        /// <param name="so2rMeasureArray">The SO2R hour measure update array.</param>
        /// <param name="co2mArray">The CO2M update array.</param>
        /// <param name="co2mMeasureArray">The CO2M hour measure update array.</param>
        /// <param name="co2rArray">The CO2R update array.</param>
        /// <param name="co2rMeasureArray">The CO2R hour measure update array.</param>
        /// <param name="noxmArray">The NOXM update array.</param>
        /// <param name="noxmMeasureArray">The NOXM hour measure update array.</param>
        /// <param name="noxrArray">The NOXR update array.</param>
        /// <param name="noxrMeasureArray">The NOXR hour measure update array.</param>
        /// <param name="hgRateEoArray">The Hg Rate Electrical Output update array.</param>
        /// <param name="hgRateHiArray">The Hg Rate Heat Input update array.</param>
        /// <param name="hgMassArray">The Hg Mass update array.</param>
        /// <param name="hgMeasureArray">The Hg hour measure update array.</param>
        /// <param name="hclRateEoArray">The HCl Rate Electrical Output update array.</param>
        /// <param name="hclRateHiArray">The HCl Rate Heat Input update array.</param>
        /// <param name="hclMassArray">The HCl Mass update array.</param>
        /// <param name="hclMeasureArray">The HCl hour measure update array.</param>
        /// <param name="hfRateEoArray">The HF Rate Electrical Output update array.</param>
        /// <param name="hfRateHiArray">The HF Rate Heat Input update array.</param>
        /// <param name="hfMassArray">The HF Mass update array.</param>
        /// <param name="hfMeasureArray">The HF hour measure update array.</param>
        /// <param name="monPlanIdArray">The MON_PLAN_ID update array.</param>
        /// <param name="rptPeriodIdArray">The RPT_PERIOD_ID update array.</param>
        /// <param name="opYearArray">The op year update array.</param>
        /// <param name="dataSourceArray">The data source update array.</param>
        /// <param name="userIdArray">The user id update array.</param>
        public void UpdateSuccess(string dmEmissionsId,
                                  eApportionmentType apportionmentType,
                                  bool? isMatsEmissionReport,
                                  string[] dmEmissionsIdArray,
                                  int?[] unitKeyArray,
                                  DateTime?[] opDateArray,
                                  int?[] opHourArray,
                                  decimal?[] opTimeArray,
                                  decimal?[] gLoadArray,
                                  decimal?[] mLoadArray,
                                  decimal?[] sLoadArray,
                                  decimal?[] tLoadArray,
                                  decimal?[] hitArray,
                                  string[] hitMeasureArray,
                                  decimal?[] so2mArray,
                                  string[] so2mMeasureArray,
                                  decimal?[] so2rArray,
                                  string[] so2rMeasureArray,
                                  decimal?[] co2mArray,
                                  string[] co2mMeasureArray,
                                  decimal?[] co2rArray,
                                  string[] co2rMeasureArray,
                                  decimal?[] noxmArray,
                                  string[] noxmMeasureArray,
                                  decimal?[] noxrArray,
                                  string[] noxrMeasureArray,
                                  decimal?[] hgRateEoArray,
                                  decimal?[] hgRateHiArray,
                                  decimal?[] hgMassArray,
                                  string[] hgMeasureArray,
                                  decimal?[] hclRateEoArray,
                                  decimal?[] hclRateHiArray,
                                  decimal?[] hclMassArray,
                                  string[] hclMeasureArray,
                                  decimal?[] hfRateEoArray,
                                  decimal?[] hfRateHiArray,
                                  decimal?[] hfMassArray,
                                  string[] hfMeasureArray,
                                  string[] monPlanIdArray,
                                  int?[] rptPeriodIdArray,
                                  int?[] opYearArray,
                                  string[] dataSourceArray,
                                  string[] userIdArray)
        {
            string errorMessage = null;

            if (UpdateUnitHour(isMatsEmissionReport,
                               dmEmissionsIdArray, unitKeyArray, opDateArray, opHourArray,
                               opTimeArray, gLoadArray, mLoadArray, sLoadArray, tLoadArray,
                               hitArray, hitMeasureArray,
                               so2mArray, so2mMeasureArray, so2rArray, so2rMeasureArray,
                               co2mArray, co2mMeasureArray, co2rArray, co2rMeasureArray,
                               noxmArray, noxmMeasureArray, noxrArray, noxrMeasureArray,
                               hgRateEoArray, hgRateHiArray, hgMassArray, hgMeasureArray,
                               hclRateEoArray, hclRateHiArray, hclMassArray, hclMeasureArray,
                               hfRateEoArray, hfRateHiArray, hfMassArray, hfMeasureArray,
                               monPlanIdArray, rptPeriodIdArray, opYearArray,
                               dataSourceArray, userIdArray,
                               ref errorMessage) &&
                CheckLogUpdate(ref errorMessage))
            {
                try
                {
                    DbData.CreateStoredProcedureCommand("DmEm.UpdateSuccess");

                    DbData.AddInputParameter("@V_DM_EMISSIONS_ID", dmEmissionsId);
                    DbData.AddInputParameter("@V_APPORTIONMENT_TYPE_CD", apportionmentType.DbCode());
                    DbData.AddOutputParameterString("@V_RESULT", 1);
                    DbData.AddOutputParameterString("@V_ERROR_MSG", 200);

                    DbData.ExecuteNonQuery();

                    if (DbData.GetParameterString("@V_RESULT") != "T")
                    {
                        errorMessage = DbData.GetParameterString("@V_ERROR_MSG");

                        if ((ReportFailureCallback != null) && errorMessage.HasValue())
                            ReportFailureCallback(errorMessage);
                    }
                }
                catch (Exception ex)
                {
                    if (ReportFailureCallback != null)
                        ReportFailureCallback(string.Format("{0} (UpdateSuccess)", ex.Message));
                }
            }
            else
            {
                if (ReportFailureCallback != null)
                    ReportFailureCallback(errorMessage);
            }
        }


        #region Helper Methods

        /// <summary>
        /// Merges row from CheckLogTable with adjacent hours into a single rows.
        /// </summary>
        /// <returns></returns>
        protected DataTable CheckLogMerge()
        {
            DataTable result;

            if (CheckLogTable != null)
            {
                result = CheckLogTable.Clone();

                string originalSort = CheckLogTable.DefaultView.Sort;

                try
                {
                    CheckLogTable.DefaultView.Sort = "MON_LOC_ID, " +
                                                     "CHECK_CATALOG_RESULT_ID, " +
                                                     "RESULT_MESSAGE, " +
                                                     "CHK_LOG_COMMENT, " +
                                                     "OP_BEGIN_DATE, " +
                                                     "OP_BEGIN_HOUR, " +
                                                     "OP_END_DATE, " +
                                                     "OP_END_HOUR";

                    string breakMonLocId = null;
                    int? breakCheckCatalogResultId = null;
                    string breakCheckResult = null;
                    string breakSeverityCd = null;
                    string breakResultMessage = null;
                    string breakChkLogComment = null;

                    DateTime? checkBeginDate = null;
                    DateTime? checkOpBeganDate = null;
                    int? checkOpBeganHour = null;
                    DateTime? checkOpEndedDate = null;
                    int? checkOpEndedHour = null;

                    bool initial = true;

                    foreach (DataRowView checkLogRow in CheckLogTable.DefaultView)
                    {
                        string tableMonLocId = checkLogRow["MON_LOC_ID"].AsString();
                        int? tableCheckCatalogResultId = checkLogRow["CHECK_CATALOG_RESULT_ID"].AsInteger();
                        string tableCheckResult = checkLogRow["CHECK_RESULT"].AsString();
                        string tableSeverityCd = checkLogRow["SEVERITY_CD"].AsString();
                        string tableResultMessage = checkLogRow["RESULT_MESSAGE"].AsString();
                        string tableChkLogComment = checkLogRow["CHK_LOG_COMMENT"].AsString();
                        DateTime? tableOpBeganDate = checkLogRow["CHECK_DATE"].AsDateTime();
                        int? tableOpBeganHour = checkLogRow["CHECK_HOUR"].AsInteger();
                        DateTime? tableOpEndedDate = checkLogRow["CHECK_DATE"].AsDateTime();
                        int? tableOpEndedHour = checkLogRow["CHECK_HOUR"].AsInteger();
                        DateTime? tableBeginDate = checkLogRow["BEGIN_DATE"].AsDateTime();

                        if (!((cDBConvert.ToString(tableMonLocId) == cDBConvert.ToString(breakMonLocId)) &&
                              (cDBConvert.ToLong(tableCheckCatalogResultId) == cDBConvert.ToLong(breakCheckCatalogResultId)) &&
                              (cDBConvert.ToString(tableResultMessage) == cDBConvert.ToString(breakResultMessage)) &&
                              (cDBConvert.ToString(tableChkLogComment) == cDBConvert.ToString(breakChkLogComment)) &&
                              (tableOpBeganDate.HasValue && tableOpBeganHour.HasValue) &&
                              (checkOpBeganDate.HasValue && checkOpBeganHour.HasValue) &&
                              (checkOpEndedDate.HasValue && checkOpEndedHour.HasValue) &&
                              (((tableOpBeganDate.Value == checkOpEndedDate.Value) &&
                                ((tableOpBeganHour.Value - checkOpEndedHour.Value) <= 1)) ||
                               ((tableOpBeganDate.Value == checkOpEndedDate.Value.AddDays(+1)) &&
                                ((tableOpBeganHour.Value <= 0) && (checkOpEndedHour.Value >= 23))))))
                        {
                            if (!initial)
                            {
                                DataRow resultRow = result.NewRow();

                                resultRow["CHK_SESSION_ID"] = CheckSessionId.DbValue();

                                resultRow["MON_LOC_ID"] = breakMonLocId.DbValue();
                                resultRow["CHECK_CATALOG_RESULT_ID"] = breakCheckCatalogResultId.DbValue();
                                resultRow["CHECK_RESULT"] = breakCheckResult.DbValue();
                                resultRow["SEVERITY_CD"] = breakSeverityCd.DbValue();
                                resultRow["RESULT_MESSAGE"] = breakResultMessage.DbValue();
                                resultRow["CHK_LOG_COMMENT"] = breakChkLogComment.DbValue();
                                resultRow["BEGIN_DATE"] = checkBeginDate.DbValue();

                                resultRow["OP_BEGIN_DATE"] = checkOpBeganDate.DbValue();
                                resultRow["OP_BEGIN_HOUR"] = checkOpBeganHour.DbValue();
                                resultRow["OP_END_DATE"] = checkOpEndedDate.DbValue();
                                resultRow["OP_END_HOUR"] = checkOpEndedHour.DbValue();

                                result.Rows.Add(resultRow);
                            }

                            breakMonLocId = tableMonLocId;
                            breakCheckCatalogResultId = tableCheckCatalogResultId;
                            breakCheckResult = tableCheckResult;
                            breakSeverityCd = tableSeverityCd;
                            breakResultMessage = tableResultMessage;
                            breakChkLogComment = tableChkLogComment;

                            checkOpBeganDate = tableOpBeganDate;
                            checkOpBeganHour = tableOpBeganHour;

                            checkBeginDate = tableBeginDate;
                        }

                        checkOpEndedDate = tableOpBeganDate;
                        checkOpEndedHour = tableOpBeganHour;

                        if (tableBeginDate > checkBeginDate)
                            checkBeginDate = tableBeginDate;

                        initial = false;
                    }

                    if (!initial)
                    {
                        DataRow resultRow = result.NewRow();

                        resultRow["CHK_SESSION_ID"] = CheckSessionId.DbValue();

                        resultRow["MON_LOC_ID"] = breakMonLocId.DbValue();
                        resultRow["CHECK_CATALOG_RESULT_ID"] = breakCheckCatalogResultId.DbValue();
                        resultRow["CHECK_RESULT"] = breakCheckResult.DbValue();
                        resultRow["SEVERITY_CD"] = breakSeverityCd.DbValue();
                        resultRow["RESULT_MESSAGE"] = breakResultMessage.DbValue();
                        resultRow["CHK_LOG_COMMENT"] = breakChkLogComment.DbValue();
                        resultRow["BEGIN_DATE"] = checkBeginDate.DbValue();

                        resultRow["OP_BEGIN_DATE"] = checkOpBeganDate.DbValue();
                        resultRow["OP_BEGIN_HOUR"] = checkOpBeganHour.DbValue();
                        resultRow["OP_END_DATE"] = checkOpEndedDate.DbValue();
                        resultRow["OP_END_HOUR"] = checkOpEndedHour.DbValue();

                        result.Rows.Add(resultRow);
                    }
                }
                finally
                {
                    CheckLogTable.DefaultView.Sort = originalSort;
                }
            }
            else
                result = null;

            return result;
        }

        /// <summary>
        /// Updates CHECK_LOG with any logged errors.
        /// </summary>
        /// <param name="errorMessage">Returns the message generated if the update failed.</param>
        /// <returns></returns>
        private bool CheckLogUpdate(ref string errorMessage)
        {
            bool result;

            try
            {
                if ((CheckLogTable != null) && (CheckLogTable.Rows.Count > 0))
                {
                    DataTable checkLogMergeTable = CheckLogMerge();

                    result = DbAux.BulkLoad(checkLogMergeTable,
                                            "CHECK_LOG",
                                            new string[] { "CHK_LOG_ID" },
                                            ref errorMessage);
                }
                else
                    result = true;
            }
            catch (Exception ex)
            {
                errorMessage = string.Format("{0} (CheckLogUpdate)", ex.Message);
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Grabs the result information to use.
        /// </summary>
        private void InitErrorLogging_Results()
        {
            string sql = "select ccr.CHECK_CATALOG_RESULT_ID, "
                       + "       ccr.CHECK_RESULT, "
                       + "       ccr.SEVERITY_CD "
                       + "  from CHECK_CATALOG_RESULT ccr "
                       + "  where ccr.CHECK_CATALOG_ID "
                       + "          in (select CHECK_CATALOG_ID "
                       + "                from CHECK_CATALOG chk "
                       + "                where chk.CHECK_TYPE_CD = 'DMEM' "
                       + "                      and chk.CHECK_NUMBER = 1) ";

            try
            {
                DataTable resultTable = DbAux.GetDataTable(sql);

                foreach (DataRow resultRow in resultTable.Rows)
                {
                    string checkResult = resultRow["CHECK_RESULT"].AsString();

                    if (checkResult == "A")
                        ResultForEveryHour = new cCheckCatalogResult(resultRow["CHECK_CATALOG_RESULT_ID"].AsInteger(),
                                                                     checkResult,
                                                                     resultRow["SEVERITY_CD"].AsString());
                    else if (checkResult == "B")
                        ResultForIndividualHours = new cCheckCatalogResult(resultRow["CHECK_CATALOG_RESULT_ID"].AsInteger(),
                                                                           checkResult,
                                                                           resultRow["SEVERITY_CD"].AsString());
                }
            }
            catch
            {
            }


        }

        /// <summary>
        /// Formats the error information for display
        /// </summary>
        /// <param name="message">The error message to log.</param>
        /// <param name="detail">Additional detail about the error.</param>
        /// <param name="date">The op date of the data that caused the error.</param>
        /// <param name="hour">The op hour of the data that caused the error.</param>
        /// <param name="locationKey">The location key of the data that caused the error.</param>
        /// <param name="callingMethod">The class and name of the calling method.</param>
        /// <returns></returns>
        private static string LogErrorFormat(string message, string detail,
                                             DateTime? date, int? hour, string locationKey,
                                             string callingMethod)
        {
            string result;

            string format;
            {
                string formatAppend = "";
                {
                    string formatDelim = "";

                    if (detail != null)
                    {
                        formatAppend += formatDelim + "Detail: {1}";
                        formatDelim = "; ";
                    }

                    if (hour.HasValue())
                    {
                        formatAppend += formatDelim + "Hour: {2}-{3}";
                        formatDelim = "; ";
                    }
                    else if (date.HasValue())
                    {
                        formatAppend += formatDelim + "Date: {2}";
                        formatDelim = "; ";
                    }

                    if (locationKey.HasValue())
                    {
                        formatAppend += formatDelim + "Location: {4}";
                        formatDelim = "; ";
                    }

                    if (formatAppend != "")
                        formatAppend = " [" + formatAppend + "]";

                    if (callingMethod.HasValue())
                        formatAppend += " ({5})";
                }

                format = "{0}" + formatAppend;
            }

            result = string.Format(format, message, detail, date, hour, locationKey, callingMethod);

            return result;
        }

        /// <summary>
        /// Formats the error information for display
        /// </summary>
        /// <param name="message">The error message to log.</param>
        /// <param name="detail">Additional detail about the error.</param>
        /// <param name="callingMethod">The class and name of the calling method.</param>
        /// <returns></returns>
        private static string LogErrorFormat(string message, string detail, string callingMethod)
        {
            string result;

            string format;
            {
                string formatAppend = "";
                {
                    if (detail != null)
                        formatAppend += " [Detail: {1}]";

                    if (callingMethod.HasValue())
                        formatAppend += " ({2})";
                }

                format = "{0}" + formatAppend;
            }

            result = string.Format(format, message, detail, callingMethod);

            return result;
        }

        /// <summary>
        /// Updates UNIT_HOUR with the apportioned data.
        /// </summary>
        /// <param name="isMatsEmissionReport">Indicates whether the emission report is a MATS report.</param>
        /// <param name="dmEmissionsIdArray">The DM_EMISSIONS_ID update array.</param>
        /// <param name="unitKeyArray">The UNIT_ID update array.</param>
        /// <param name="opDateArray">The op date update array.</param>
        /// <param name="opHourArray">The op hour update array.</param>
        /// <param name="opTimeArray">The op time update array.</param>
        /// <param name="gLoadArray">The g-load update array.</param>
        /// <param name="mLoadArray">The MATS load update array.</param>
        /// <param name="sLoadArray">The s-load update array.</param>
        /// <param name="tLoadArray">The t-load update array.</param>
        /// <param name="hitArray">The HIT update array.</param>
        /// <param name="hitMeasureArray">The HIT hour measure update array.</param>
        /// <param name="so2mArray">The SO2M update array.</param>
        /// <param name="so2mMeasureArray">The SO2M hour measure update array.</param>
        /// <param name="so2rArray">The SO2R update array.</param>
        /// <param name="so2rMeasureArray">The SO2R hour measure update array.</param>
        /// <param name="co2mArray">The CO2M update array.</param>
        /// <param name="co2mMeasureArray">The CO2M hour measure update array.</param>
        /// <param name="co2rArray">The CO2R update array.</param>
        /// <param name="co2rMeasureArray">The CO2R hour measure update array.</param>
        /// <param name="noxmArray">The NOXM update array.</param>
        /// <param name="noxmMeasureArray">The NOXM hour measure update array.</param>
        /// <param name="noxrArray">The NOXR update array.</param>
        /// <param name="noxrMeasureArray">The NOXR hour measure update array.</param>
        /// <param name="hgRateEoArray">The Hg Rate Electrical Output update array.</param>
        /// <param name="hgRateHiArray">The Hg Rate Heat Input update array.</param>
        /// <param name="hgMassArray">The Hg Mass update array.</param>
        /// <param name="hgMeasureArray">The Hg hour measure update array.</param>
        /// <param name="hclRateEoArray">The HCl Rate Electrical Output update array.</param>
        /// <param name="hclRateHiArray">The HCl Rate Heat Input update array.</param>
        /// <param name="hclMassArray">The HCl Mass update array.</param>
        /// <param name="hclMeasureArray">The HCl hour measure update array.</param>
        /// <param name="hfRateEoArray">The HF Rate Electrical Output update array.</param>
        /// <param name="hfRateHiArray">The HF Rate Heat Input update array.</param>
        /// <param name="hfMassArray">The HF Mass update array.</param>
        /// <param name="hfMeasureArray">The HF hour measure update array.</param>
        /// <param name="monPlanIdArray">The MON_PLAN_ID update array.</param>
        /// <param name="rptPeriodIdArray">The RPT_PERIOD_ID update array.</param>
        /// <param name="opYearArray">The op year update array.</param>
        /// <param name="dataSourceArray">The data source update array.</param>
        /// <param name="userIdArray">The user id update array.</param>
        /// <param name="errorMessage">Error message.</param>
        /// <returns>True if successful.</returns>
        private bool UpdateUnitHour(bool? isMatsEmissionReport,
                                    string[] dmEmissionsIdArray,
                                    int?[] unitKeyArray,
                                    DateTime?[] opDateArray,
                                    int?[] opHourArray,
                                    decimal?[] opTimeArray,
                                    decimal?[] gLoadArray,
                                    decimal?[] mLoadArray,
                                    decimal?[] sLoadArray,
                                    decimal?[] tLoadArray,
                                    decimal?[] hitArray,
                                    string[] hitMeasureArray,
                                    decimal?[] so2mArray,
                                    string[] so2mMeasureArray,
                                    decimal?[] so2rArray,
                                    string[] so2rMeasureArray,
                                    decimal?[] co2mArray,
                                    string[] co2mMeasureArray,
                                    decimal?[] co2rArray,
                                    string[] co2rMeasureArray,
                                    decimal?[] noxmArray,
                                    string[] noxmMeasureArray,
                                    decimal?[] noxrArray,
                                    string[] noxrMeasureArray,
                                    decimal?[] hgRateEoArray,
                                    decimal?[] hgRateHiArray,
                                    decimal?[] hgMassArray,
                                    string[] hgMeasureArray,
                                    decimal?[] hclRateEoArray,
                                    decimal?[] hclRateHiArray,
                                    decimal?[] hclMassArray,
                                    string[] hclMeasureArray,
                                    decimal?[] hfRateEoArray,
                                    decimal?[] hfRateHiArray,
                                    decimal?[] hfMassArray,
                                    string[] hfMeasureArray,
                                    string[] monPlanIdArray,
                                    int?[] rptPeriodIdArray,
                                    int?[] opYearArray,
                                    string[] dataSourceArray,
                                    string[] userIdArray,
                                    ref string errorMessage)
        {
            bool result;

            try
            {
                if (unitKeyArray.Length > 0)
                {
                    DataTable unitHourTable = DbData.GetDataTable("SELECT * FROM DmEm.UNIT_HOUR WHERE NULL = NULL");
                    DataTable matsUnitHourTable = DbData.GetDataTable("SELECT * FROM DmEm.MATS_UNIT_HOUR WHERE NULL = NULL");

                    for (int dex = 0; dex < unitKeyArray.Length; dex++)
                    {
                        DataRow unitHourRow = unitHourTable.NewRow();
                        {
                            unitHourRow["DM_EMISSIONS_ID"] = dmEmissionsIdArray[dex].DbValue();
                            unitHourRow["UNIT_ID"] = unitKeyArray[dex].DbValue();
                            unitHourRow["OP_DATE"] = opDateArray[dex].DbValue();
                            unitHourRow["OP_HOUR"] = opHourArray[dex].DbValue();
                            unitHourRow["OP_TIME"] = opTimeArray[dex].DbValue();
                            unitHourRow["GLOAD"] = gLoadArray[dex].DbValue();
                            unitHourRow["SLOAD"] = sLoadArray[dex].DbValue();
                            unitHourRow["TLOAD"] = tLoadArray[dex].DbValue();
                            unitHourRow["HIT"] = hitArray[dex].DbValue();
                            unitHourRow["HIT_HOUR_MEASURE_CD"] = hitMeasureArray[dex].DbValue();
                            unitHourRow["SO2M"] = so2mArray[dex].DbValue();
                            unitHourRow["SO2M_HOUR_MEASURE_CD"] = so2mMeasureArray[dex].DbValue();
                            unitHourRow["SO2R"] = so2rArray[dex].DbValue();
                            unitHourRow["SO2R_HOUR_MEASURE_CD"] = so2rMeasureArray[dex].DbValue();
                            unitHourRow["CO2M"] = co2mArray[dex].DbValue();
                            unitHourRow["CO2M_HOUR_MEASURE_CD"] = co2mMeasureArray[dex].DbValue();
                            unitHourRow["CO2R"] = co2rArray[dex].DbValue();
                            unitHourRow["CO2R_HOUR_MEASURE_CD"] = co2rMeasureArray[dex].DbValue();
                            unitHourRow["NOXM"] = noxmArray[dex].DbValue();
                            unitHourRow["NOXM_HOUR_MEASURE_CD"] = noxmMeasureArray[dex].DbValue();
                            unitHourRow["NOXR"] = noxrArray[dex].DbValue();
                            unitHourRow["NOXR_HOUR_MEASURE_CD"] = noxrMeasureArray[dex].DbValue();
                            unitHourRow["MON_PLAN_ID"] = monPlanIdArray[dex].DbValue();
                            unitHourRow["RPT_PERIOD_ID"] = rptPeriodIdArray[dex].DbValue();
                            unitHourRow["OP_YEAR"] = opYearArray[dex].DbValue();
                            unitHourRow["DATA_SOURCE"] = dataSourceArray[dex].DbValue();
                            unitHourRow["USERID"] = userIdArray[dex].DbValue();
                        }
                        unitHourTable.Rows.Add(unitHourRow);

                        if (isMatsEmissionReport.Default(false))
                        {
                            DataRow matsUnitHourRow = matsUnitHourTable.NewRow();
                            {
                                matsUnitHourRow["DM_EMISSIONS_ID"] = dmEmissionsIdArray[dex].DbValue();
                                matsUnitHourRow["UNIT_ID"] = unitKeyArray[dex].DbValue();
                                matsUnitHourRow["OP_DATE"] = opDateArray[dex].DbValue();
                                matsUnitHourRow["OP_HOUR"] = opHourArray[dex].DbValue();
                                matsUnitHourRow["OP_TIME"] = opTimeArray[dex].DbValue();
                                matsUnitHourRow["GLOAD"] = mLoadArray[dex].DbValue();
                                matsUnitHourRow["SLOAD"] = sLoadArray[dex].DbValue();
                                matsUnitHourRow["TLOAD"] = tLoadArray[dex].DbValue();
                                matsUnitHourRow["HIT"] = hitArray[dex].DbValue();
                                matsUnitHourRow["HIT_HOUR_MEASURE_CD"] = hitMeasureArray[dex].DbValue();
                                matsUnitHourRow["HG_RATE_EO"] = hgRateEoArray[dex].DbValue();
                                matsUnitHourRow["HG_RATE_HI"] = hgRateHiArray[dex].DbValue();
                                matsUnitHourRow["HG_MASS"] = hgMassArray[dex].DbValue();
                                matsUnitHourRow["HG_HOUR_MEASURE_CD"] = hgMeasureArray[dex].DbValue();
                                matsUnitHourRow["HCL_RATE_EO"] = hclRateEoArray[dex].DbValue();
                                matsUnitHourRow["HCL_RATE_HI"] = hclRateHiArray[dex].DbValue();
                                matsUnitHourRow["HCL_MASS"] = hclMassArray[dex].DbValue();
                                matsUnitHourRow["HCL_HOUR_MEASURE_CD"] = hclMeasureArray[dex].DbValue();
                                matsUnitHourRow["HF_RATE_EO"] = hfRateEoArray[dex].DbValue();
                                matsUnitHourRow["HF_RATE_HI"] = hfRateHiArray[dex].DbValue();
                                matsUnitHourRow["HF_MASS"] = hfMassArray[dex].DbValue();
                                matsUnitHourRow["HF_HOUR_MEASURE_CD"] = hfMeasureArray[dex].DbValue();
                                matsUnitHourRow["MON_PLAN_ID"] = monPlanIdArray[dex].DbValue();
                                matsUnitHourRow["RPT_PERIOD_ID"] = rptPeriodIdArray[dex].DbValue();
                                matsUnitHourRow["OP_YEAR"] = opYearArray[dex].DbValue();
                                matsUnitHourRow["DATA_SOURCE"] = dataSourceArray[dex].DbValue();
                                matsUnitHourRow["USERID"] = userIdArray[dex].DbValue();
                            }
                            matsUnitHourTable.Rows.Add(matsUnitHourRow);
                        }
                    }

                    result = true;

                    result = DbData.BulkLoad(unitHourTable,
                                             "DmEm.UNIT_HOUR",
                                             new string[] { "UNIT_HOUR_ID", "ADD_DATE" },
                                             ref errorMessage) && result;

                    if (isMatsEmissionReport.Default(false))
                    {
                        result = DbData.BulkLoad(matsUnitHourTable,
                                                 "DmEm.MATS_UNIT_HOUR",
                                                 new string[] { "MATS_UNIT_HOUR_ID", "ADD_DATE" },
                                                 ref errorMessage) && result;
                    }
                }
                else
                    result = true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                result = false;
            }

            return result;
        }

        #endregion

        #endregion

    }
}
