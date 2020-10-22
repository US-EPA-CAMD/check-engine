using System;
using System.Collections.Generic;
using System.Text;

namespace ECMPS.Checks.DatabaseAccess
{

    /// <summary>
    /// Replaces AuxDBDataContext DBML, especially the method calls for stored procedures/functions
    /// </summary>
    public class AuxDBDataContext
    {

        /// <summary>
        /// Creates a DataDBDataContext instance using the cDatabase instance and command timeout defaulted to 30 seconds.
        /// </summary>
        /// <param name="database">The cDatabase instance for the ECMPS (data) database.</param>
        /// <param name="commandTimeout">The default timeout to use for the database.</param>
        public AuxDBDataContext(cDatabase database, int commandTimeout = 30)
        {
            Database = database;
            CommandTimeout = commandTimeout;
        }

        /// <summary>
        /// The command timeout to use for database commands.
        /// </summary>
        public int CommandTimeout { get; set; }

        /// <summary>
        /// The cDatabase instance to use for datbase connections.
        /// </summary>
        public cDatabase Database { get; private set; }


        /// <summary>
        /// Calls ECMPS_AUX.Check.CheckSessionCompleted stored procedure.
        /// </summary>
        /// <param name="chkSessionId">Check Session Id for the current check session.</param>
        /// <param name="severityCd">The resulting severity code for the check session.</param>
        /// <param name="result">'T' if the update was successful and 'F' if it was not.</param>
        /// <param name="errorMessage">The error message produced when the update was not successful.</param>
        /// <returns>0</returns>
        public int CheckSessionCompleted(string chkSessionId, string severityCd, ref System.Nullable<char> result, ref string errorMessage)
        {
            //TODO (EC-3519): Testing Needed
            string resultString;

            Database.CreateStoredProcedureCommand("Check.CheckSessionCompleted");

            Database.AddInputParameter("@V_CHK_SESSION_ID", chkSessionId);
            Database.AddInputParameter("@V_SEVERITY_CD", severityCd);
            Database.AddOutputParameterString("@V_RESULT", 1);
            Database.AddOutputParameterString("@V_ERROR_MSG", 1000);

            Database.ExecuteNonQuery();

            resultString = Database.GetParameterString("@V_RESULT");
            errorMessage = Database.GetParameterString("@V_ERROR_MSG");

            result = !string.IsNullOrWhiteSpace(resultString) ? resultString[0] : (char?)null;

            return 0;
        }


        /// <summary>
        /// Calls ECMPS_AUX.Check.CheckSessionFailed stored procedure.
        /// </summary>
        /// <param name="chkSessionId">Check Session Id for the current check session.</param>
        /// <param name="errorComment">Error information produced by the Check Engine.</param>
        /// <param name="result">'T' if the update was successful and 'F' if it was not.</param>
        /// <param name="errorMessage">The error message produced when the update was not successful.</param>
        /// <returns>0</returns>
        public int CheckSessionFailed(string chkSessionId, string errorComment, ref System.Nullable<char> result, ref string errorMessage)
        {
            //TODO (EC-3519): Testing Needed
            string resultString;

            Database.CreateStoredProcedureCommand("Check.CheckSessionFailed");

            Database.AddInputParameter("@V_CHK_SESSION_ID", chkSessionId);
            Database.AddInputParameter("@V_ERROR_COMMENT", errorComment);
            Database.AddOutputParameterString("@V_RESULT", 1);
            Database.AddOutputParameterString("@V_ERROR_MSG", 1000);

            Database.ExecuteNonQuery();

            resultString = Database.GetParameterString("@V_RESULT");
            errorMessage = Database.GetParameterString("@V_ERROR_MSG");

            result = !string.IsNullOrWhiteSpace(resultString) ? resultString[0] : (char?)null;

            return 0;
        }


        /// <summary>
        /// Calls ECMPS_AUX.Check.CheckSessionInit stored procedure.
        /// </summary>
        /// <param name="processCd">Check Process Code for the evaluation.</param>
        /// <param name="categoryCd">Check Category Code for the evaluation.</param>
        /// <param name="monPlanId">MON_PLAN_ID for the evaluation.</param>
        /// <param name="rptPeriodId">RPT_PERIOD_ID for the evaluation, if applicable.</param>
        /// <param name="testSumId">TEST_SUM_ID for the evaluation, if applicable.</param>
        /// <param name="qaCertEventId">QA_CERT_EVENT_ID for the evaluation, if applicable.</param>
        /// <param name="testExtensionExemptionId">TEST_EXTENSION_EXEMPTION_ID for the evaluation, if applicable.</param>
        /// <param name="evaluationBeginDate">The evaluation date range begin date.</param>
        /// <param name="evaluationEndDate">The evaluation date range end date.</param>
        /// <param name="userId">The user id for the user performing the evaluation.</param>
        /// <param name="chkSessionId">The Check Session Id for the evaluation.</param>
        /// <param name="result">'T' if the update was successful and 'F' if it was not.</param>
        /// <param name="errorMessage">The error message produced when the update was not successful.</param>
        /// <returns>0</returns>
        public int CheckSessionInit(string processCd, string categoryCd, string monPlanId, System.Nullable<int> rptPeriodId, 
                                    string testSumId, string qaCertEventId, string testExtensionExemptionId, 
                                    System.Nullable<System.DateTime> evaluationBeginDate, System.Nullable<System.DateTime> evaluationEndDate, 
                                    string userId, ref string chkSessionId, 
                                    ref System.Nullable<char> result, ref string errorMessage)
        {
            //TODO (EC-3519): Testing Needed
            string resultString;

            Database.CreateStoredProcedureCommand("Check.CheckSessionInit");

            Database.AddInputParameter("@V_PROCESS_CD", processCd);
            Database.AddInputParameter("@V_CATEGORY_CD", categoryCd);
            Database.AddInputParameter("@V_MON_PLAN_ID", monPlanId);
            Database.AddInputParameter("@V_RPT_PERIOD_ID", rptPeriodId);
            Database.AddInputParameter("@V_TEST_SUM_ID", testSumId);
            Database.AddInputParameter("@V_QA_CERT_EVENT_ID", qaCertEventId);
            Database.AddInputParameter("@V_TEST_EXTENSION_EXEMPTION_ID", testExtensionExemptionId);
            Database.AddInputParameter("@V_EVAL_BEGIN_DATE", evaluationBeginDate);
            Database.AddInputParameter("@V_EVAL_END_DATE", evaluationEndDate);
            Database.AddInputParameter("@V_USERID", userId);
            Database.AddOutputParameterString("@V_CHK_SESSION_ID", 45);
            Database.AddOutputParameterString("@V_RESULT", 1);
            Database.AddOutputParameterString("@V_ERROR_MSG", 1000);

            Database.ExecuteNonQuery();

            chkSessionId = Database.GetParameterString("@V_CHK_SESSION_ID");
            resultString = Database.GetParameterString("@V_RESULT");
            errorMessage = Database.GetParameterString("@V_ERROR_MSG");

            result = !string.IsNullOrWhiteSpace(resultString) ? resultString[0] : (char?)null;

            return 0;
        }


        /// <summary>
        /// Calls ECMPS_AUX.dbo.sp_GetNextSessionID stored procedure.
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="result"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
		public int sp_GetNextSessionID(ref System.Nullable<decimal> sessionId, ref System.Nullable<char> result, ref string errorMessage)
        {
            //TODO (EC-3519): Testing Needed

            string resultString;

            Database.CreateStoredProcedureCommand("dbo.sp_GetNextSessionID");

            Database.AddOutputParameterDecimal("@V_SessionID");
            Database.AddOutputParameterString("@V_RESULT", 1);
            Database.AddOutputParameterString("@V_ERROR_MSG", 1000);

            Database.ExecuteNonQuery();

            sessionId = Database.GetParameterDecimal("@V_SessionID");
            resultString = Database.GetParameterString("@V_RESULT");
            errorMessage = Database.GetParameterString("@V_ERROR_MSG");

            result = !string.IsNullOrWhiteSpace(resultString) ? resultString[0] : (char?)null;

            return 0;
        }

    }

}
