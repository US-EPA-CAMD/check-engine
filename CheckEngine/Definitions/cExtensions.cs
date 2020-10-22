using System;
using System.Data;
using System.Diagnostics;
using System.Reflection;

using ECMPS.Checks.DatabaseAccess;
using ECMPS.Checks.TypeUtilities;
using ECMPS.Common;
using ECMPS.Definitions.SeverityCode;
using ECMPS.ErrorSuppression;


namespace ECMPS.Checks.CheckEngine.Definitions
{

    /// <summary>
    /// Extensions used by/in check engine (new for .net 3.5)
    /// </summary>
    public static class cExtensions
    {

        #region Severity Code

        private static DataView _dvSeverityCode = null;

        /// <summary>
        /// Returns whether the passed Severity Code will block submissions
        /// </summary>
        /// <param name="ASeverityCd">The severity code</param>
        /// <returns>true if the severity code blocks submission, otherwise false</returns>
        public static bool BlocksSubmission(this eSeverityCd ASeverityCd)
        {
            if ((_dvSeverityCode != null) || LoadSeverityCode())
            {
                DataRowView SeverityRow = GetSeverityRow(ASeverityCd.ToStringValue());

                if ((SeverityRow != null) && (SeverityRow["Blocks_Submission"] != DBNull.Value))
                    return Convert.ToInt32(SeverityRow["Blocks_Submission"]) == 1 ? true : false;
            }
            return false;
        }

        /// <summary>
        /// Returns the Severity Description associated with the passed Severity Code.
        /// </summary>
        /// <param name="ASeverityCd">The severity code for which a discription is returned.</param>
        /// <param name="ADefault">The default description to use if the passed severity code is not known.</param>
        /// <returns>The description associated with the passed severity code.</returns>        
        public static string GetSeverityDescription(this eSeverityCd ASeverityCd, string ADefault)
        {
            string Result = ADefault;

            if ((_dvSeverityCode != null) || LoadSeverityCode())
            {
                DataRowView SeverityRow = GetSeverityRow(ASeverityCd.ToStringValue());

                if ((SeverityRow != null) && (SeverityRow["Severity_Cd_Description"] != DBNull.Value))
                    Result = Convert.ToString(SeverityRow["Severity_Cd_Description"]);
            }

            return Result;
        }

        /// <summary>
        /// Get the severity code's description
        /// </summary>
        /// <param name="SeverityCd">The severity code for which a discription is returned.</param>
        /// <returns>The description associated with the passed severity code.</returns>
        public static string GetSeverityDescription(this eSeverityCd SeverityCd)
        {
            return GetSeverityDescription(SeverityCd, "Unknown Severity");
        }

        /// <summary>
        /// Returns the Severity level associated with the passed SEVERITY_CD.
        /// </summary>
        /// <param name="ASeverityCd">The severity code for which a level is returned.</param>
        /// <param name="ADefault">The default level to use if the passed severity code is not known.</param>
        /// <returns>The level associated with the passed severity code.</returns>
        public static int GetSeverityLevel(this eSeverityCd ASeverityCd, int ADefault)
        {
            int Result = ADefault;

            if ((_dvSeverityCode != null) || LoadSeverityCode())
            {
                DataRowView SeverityRow = GetSeverityRow(ASeverityCd.ToStringValue());

                if ((SeverityRow != null) && (SeverityRow["Severity_Level"] != DBNull.Value))
                    Result = Convert.ToInt32(SeverityRow["Severity_Level"]);
            }

            return Result;
        }
        /// <summary>
        /// Returns the Severity level associated with the passed SEVERITY_CD.
        /// </summary>
        /// <param name="SeverityCd">The severity code for which a level is returned.</param>
        /// <returns>The level associated with the passed severity code.</returns>
        public static int GetSeverityLevel(this eSeverityCd SeverityCd)
        {
            return GetSeverityLevel(SeverityCd, int.MaxValue);
        }

        /// <summary>
        /// Get the row in the severity code table for the severity code passed in
        /// </summary>
        /// <param name="ASeverityCd">the SEVERITY_CD in question</param>
        /// <returns>the DataRowView for the SEVERITY_CD</returns>
        private static DataRowView GetSeverityRow(string ASeverityCd)
        {
            DataRowView Result = null;

            if (!string.IsNullOrEmpty(ASeverityCd))
            {
                int nRowID = _dvSeverityCode.Find(ASeverityCd);
                if (nRowID != -1)
                    Result = _dvSeverityCode[nRowID];
            }

            return Result;
        }

        /// <summary>
        /// Initicalized the cDatabase connection string needed for Severity info if it is null.
        /// </summary>
        /// <param name="dataConnectionString">The ECMPS database connection string.</param>
        /// <param name="auxConnectionString">The ECMPS_AUX database connection string.</param>
        /// <param name="workspaceConnectionString">The ECMPS_WS database connection string.</param>
        public static void CheckSeverityDatabase(string dataConnectionString,
                                                 string auxConnectionString,
                                                 string workspaceConnectionString)
        {
            if (string.IsNullOrEmpty(cDatabase.AuxConnectionString))
                cDatabase.AuxConnectionString = auxConnectionString;
        }

        /// <summary>
        /// Load the severity code table, _dvSeverityCode, if successful
        /// </summary>
        /// <returns>true if successful, false if an error</returns>
        private static bool LoadSeverityCode()
        {
            bool bRetVal = false;
            string Sql = "select Severity_Cd, Severity_Cd_Description, Severity_Level, 0 as Blocks_Submission from Severity_Code";
            cDatabase AuxConn = cDatabase.GetConnection(cDatabase.eCatalog.AUX, "LoadSeverityCode");

            try
            {
                DataTable dtSeverityCode = AuxConn.GetDataTable(Sql);
                _dvSeverityCode = new DataView(dtSeverityCode, null, "Severity_Cd", DataViewRowState.CurrentRows);
                foreach (DataRowView drSeverityCode in _dvSeverityCode)
                {
                    string sSeverityCd = cDBConvert.ToString(drSeverityCode["severity_cd"]);
                    if (sSeverityCd == eSeverityCd.FATAL.ToStringValue() ||
                        sSeverityCd == eSeverityCd.CRIT1.ToStringValue() ||
                        sSeverityCd == eSeverityCd.EXCEPTION.ToStringValue())
                        drSeverityCode["Blocks_Submission"] = 1;
                }
                bRetVal = true;
            }
            catch (Exception ex)
            {
                _dvSeverityCode = null;
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                Logging.LogErrorMessage("Loading of SEVERITY_CODE view failed");
                bRetVal = false;
            }

            AuxConn.Close();
            return bRetVal;
        }

        /// <summary>
        /// Converts the SeverityCode string value to a SEVERITY_CD enumeration value
        /// </summary>
        /// <param name="SeverityCode">The severity code string value to covert</param>
        /// <returns>The SEVERITY_CD enumeration value for the string passed in</returns>
        public static eSeverityCd ToSeverityCd(this string SeverityCode)
        {
            eSeverityCd SeverityCd = eSeverityCd.NONE;
            if ((_dvSeverityCode != null) || LoadSeverityCode())
            {
                DataRowView SeverityRow = GetSeverityRow(SeverityCode);

                if ((SeverityRow != null) && (SeverityRow["Severity_Cd"] != DBNull.Value))
                {
                    switch (SeverityCode)
                    {
                        case "CRIT1":
                            SeverityCd = eSeverityCd.CRIT1;
                            break;
                        case "CRIT2":
                            SeverityCd = eSeverityCd.CRIT2;
                            break;
                        case "CRIT3":
                            SeverityCd = eSeverityCd.CRIT3;
                            break;
                        case "EXCEPTION":
                            SeverityCd = eSeverityCd.EXCEPTION;
                            break;
                        case "FATAL":
                            SeverityCd = eSeverityCd.FATAL;
                            break;
                        case "ADMNOVR":
                            SeverityCd = eSeverityCd.ADMNOVR;
                            break;
                        case "FORGIVE":
                            SeverityCd = eSeverityCd.ADMNOVR;
                            break;
                        case "INFORM":
                            SeverityCd = eSeverityCd.INFORM;
                            break;
                        case "NONCRIT":
                            SeverityCd = eSeverityCd.NONCRIT;
                            break;
                        case "":
                        case "NONE":
                            SeverityCd = eSeverityCd.NONE;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException("SeverityCd", string.Format("Severity Code of {0} is invalid", SeverityCode));

                    }
                }
            }
            return SeverityCd;
        }

        #endregion


        #region Error Format

        /// <summary>
        /// Format an exception nicely
        /// </summary>
        /// <param name="ex">the exception in question</param>
        /// <returns>the nicely formated error message</returns>
        public static string FormatError(this Exception ex)
        {
            string format = "[{0}]:" + Environment.NewLine + "{1}" + Environment.NewLine + "{2}";

            string result;

            result = string.Format(format, GetCallingMethod(), ex.Message, ex.StackTrace);

            return result;
        }

        /// <summary>
        /// Format an exception nicely
        /// </summary>
        /// <param name="errorMessage">the error message</param>
        /// <returns>the nicely formated error message</returns>
        public static string FormatError(this string errorMessage)
        {
            string format = "[{0}]:" + Environment.NewLine + "{1}";

            string result;

            result = string.Format(format, GetCallingMethod(), errorMessage);

            return result;
        }


        #region Helper Methods

        /// <summary>
        /// Format the StackFrame information for a method.
        /// </summary>
        /// <param name="stackFrame">The stack frame for a method.</param>
        /// <returns>The formated stack frame.</returns>
        private static string FormatMethodFrameInfo(StackFrame stackFrame)
        {
            string result;

            MethodBase methodBase = stackFrame.GetMethod();

            result = string.Format("{0}.{1}()", methodBase.DeclaringType.Name, methodBase.Name);

            return result;
        }

        /// <summary>
        /// Returns the calling method class and name.
        /// </summary>
        /// <param name="depth">The depth from the parent.  1 for direct caller</param>
        /// <returns>Returns the class name and method name of the indicated caller.</returns>
        private static string GetCallingMethod(int depth)
        {
            string result;

            StackFrame stackFrame = (new StackTrace()).GetFrame(depth + 1);

            result = FormatMethodFrameInfo(stackFrame);

            return result;
        }

        /// <summary>
        /// Returns the calling method class and name.
        /// </summary>
        /// <returns>Returns the class name and method name calling method.</returns>
        private static string GetCallingMethod()
        {
            string result;

            // depth of two to get the caller of this method's caller.
            result = GetCallingMethod(2);

            return result;
        }

        /// <summary>
        /// Returns the stack trace to the calling method.
        /// </summary>
        /// <param name="depth">The depth from the parent.  1 for direct caller</param>
        /// <returns>Returns the class name and method name of the indicated caller.</returns>
        private static string GetCallingTrace(int depth)
        {
            string result = "";

            StackTrace stackTrace = new StackTrace();
            string delim = "";

            for (int level = depth + 1; level < stackTrace.FrameCount; level++)
            {
                StackFrame stackFrame = stackTrace.GetFrame(level);

                result += delim + FormatMethodFrameInfo(stackFrame);

                delim = Environment.NewLine;
            }

            return result;
        }

        /// <summary>
        /// Returns the stack trace to the calling method.
        /// </summary>
        /// <returns>Returns the class name and method name calling method.</returns>
        private static string GetCallingTrace()
        {
            string result;

            // depth of two to get the caller of this method's caller.
            result = GetCallingTrace(2);

            return result;
        }

        #endregion

        #endregion
    }

}
