using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using ECMPS.Checks.TypeUtilities;
using ECMPS.Definitions.Extensions;
using ECMPS.Definitions.SeverityCode;


namespace ECMPS.Checks.CheckEngine.SpecialParameterClasses
{
    /// <summary>
    /// 
    /// </summary>
    public class cDailyCalibrationData
    {

        #region Public Constructors

        /// <summary>
        /// Instantiates a cDailyCalibrationRunningData object.
        /// </summary>
        /// <param name="dailyCalibrationSeverityCd">The daily alibration category object's severity code.</param>
        public cDailyCalibrationData(eSeverityCd dailyCalibrationSeverityCd)
        {
            DailyCalibrationSeverityCd = dailyCalibrationSeverityCd;

            ComponentData = new Dictionary<string, cDailyCalibrationComponentData>();
            LocationComponentList = new Dictionary<string, List<string>>();
        }

        #endregion


        #region Public Properties

        /// <summary>
        /// The Daily Calibration Category object.
        /// </summary>
        public eSeverityCd DailyCalibrationSeverityCd { get; private set; }

        /// <summary>
        /// Dictionary with a Component Id lookup to cDailyCalibrationComponentData object.
        /// </summary>
        public Dictionary<string, cDailyCalibrationComponentData> ComponentData { get; private set; }

        /// <summary>
        /// This tracks the COMPONENT_ID values that belong to a particular MON_LOC_ID.
        /// </summary>
        public Dictionary<string, List<string>> LocationComponentList { get; private set; }

        #endregion


        #region Public Methods

        /// <summary>
        /// Get the most recent test for a component matching the passed validity, span scale
        /// and online/offline indication.
        /// </summary>
        /// <param name="componentId">The component id of the test.</param>
        /// <param name="valid">Indicates whether the test should be valid.</param>
        /// <param name="spanScaleCd">Indicates the span scale of the test.</param>
        /// <param name="online">Indicates whether the test was online (true) or offline (false).</param>
        /// <param name="testData">The test data object.</param>
        /// <returns>Returns true if a row was found.</returns>
        public bool GetMostRecent(string componentId, bool valid, string spanScaleCd, bool online,
                                  out cDailyCalibrationTestData testData)
        {
            bool result;

            eSpanScale spanScale;
            {
                switch (spanScaleCd)
                {
                    case "H": spanScale = eSpanScale.High; break;
                    case "L": spanScale = eSpanScale.Low; break;
                    default: spanScale = eSpanScale.None; break;
                }
            }

            if (ComponentData.ContainsKey(componentId))
            {
                testData = ComponentData[componentId].GetTestData(valid, spanScale, online);
                result = (testData != null);
            }
            else
            {
                testData = null;
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Get the most recent test for a component matching the passed validity, span scale
        /// and online/offline indication.
        /// </summary>
        /// <param name="componentId">The component id of the test.</param>
        /// <param name="valid">Indicates whether the test should be valid.</param>
        /// <param name="spanScaleCd">Indicates the span scale of the test.</param>
        /// <param name="online">Indicates whether the test was online (true) or offline (false).</param>
        /// <param name="dailyCalibrationRow">The returned daily calibration row.  Null if none found.</param>
        /// <param name="calculatedTestResultCd">The calculated test result code for the row.</param>
        /// <returns>Returns true if a row was found.</returns>
        public bool GetMostRecent(string componentId, bool valid, string spanScaleCd, bool online,
                                  out DataRowView dailyCalibrationRow,
                                  out string calculatedTestResultCd)
        {
            bool result;

            cDailyCalibrationTestData testData;

            if (GetMostRecent(componentId, valid, spanScaleCd, online, out testData))
            {
                dailyCalibrationRow = testData.DailyCalibrationRow;
                calculatedTestResultCd = testData.CalculatedTestResultCd;
                result = true;
            }
            else
            {
                dailyCalibrationRow = null;
                calculatedTestResultCd = null;
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Get the most recent test for a component matching the passed validity and span scale
        /// regardless of the online/offline indication.
        /// </summary>
        /// <param name="componentId">The component id of the test.</param>
        /// <param name="valid">Indicates whether the test should be valid.</param>
        /// <param name="spanScaleCd">Indicates the span scale of the test.</param>
        /// <param name="testData">The test data object.</param>
        /// <returns>Returns true if a row was found.</returns>
        public bool GetMostRecent(string componentId, bool valid, string spanScaleCd,
                                  out cDailyCalibrationTestData testData)
        {
            bool result;

            eSpanScale spanScale;
            {
                switch (spanScaleCd)
                {
                    case "H": spanScale = eSpanScale.High; break;
                    case "L": spanScale = eSpanScale.Low; break;
                    default: spanScale = eSpanScale.None; break;
                }
            }

            if (ComponentData.ContainsKey(componentId))
            {
                testData = ComponentData[componentId].GetTestData(valid, spanScale);
                result = (testData != null);
            }
            else
            {
                testData = null;
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Get the most recent test for a component matching the passed validity and span scale
        /// regardless of the online/offline indication.
        /// </summary>
        /// <param name="componentId">The component id of the test.</param>
        /// <param name="valid">Indicates whether the test should be valid.</param>
        /// <param name="spanScaleCd">Indicates the span scale of the test.</param>
        /// <param name="dailyCalibrationRow">The returned daily calibration row.  Null if none found.</param>
        /// <param name="calculatedTestResultCd">The calculated test result code for the row.</param>
        /// <returns>Returns true if a row was found.</returns>
        public bool GetMostRecent(string componentId, bool valid, string spanScaleCd,
                                  out DataRowView dailyCalibrationRow,
                                  out string calculatedTestResultCd)
        {
            bool result;

            cDailyCalibrationTestData testData;

            if (GetMostRecent(componentId, valid, spanScaleCd, out testData))
            {
                dailyCalibrationRow = testData.DailyCalibrationRow;
                calculatedTestResultCd = testData.CalculatedTestResultCd;
                result = true;
            }
            else
            {
                dailyCalibrationRow = null;
                calculatedTestResultCd = null;
                result = false;
            }

            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="monPlanId"></param>
        /// <param name="rptPeriodId"></param>
        /// <param name="connection"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public bool InitializeFromPreviousQuarter(string monPlanId, int rptPeriodId, SqlConnection connection, ref string errorMessage)
        {
            if (connection == null)
            {
                errorMessage = "Connection does not exists while initializing with Daily Test Supplemental Data from the previous quarter.";
                return false;
            }
            else if (connection.State != ConnectionState.Open)
            {
                errorMessage = "Connection was not open while initializing with Daily Test Supplemental Data from the previous quarter.";
                return false;
            }

            SqlCommand command = connection.CreateCommand();

            try
            {
                command.CommandText = "ECMPS.CheckEm.DailyCalibrationSuppDataPreviousQuarter";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@V_MON_PLAN_ID", SqlDbType.VarChar);
                command.Parameters.Add("@V_RPT_PERIOD_ID", SqlDbType.Int); ;
                command.Parameters.Add("@V_RESULT", SqlDbType.VarChar, 1);
                command.Parameters.Add("@V_ERROR_MSG", SqlDbType.VarChar, 200);

                command.Parameters["@V_MON_PLAN_ID"].Value = monPlanId;
                command.Parameters["@V_RPT_PERIOD_ID"].Value = rptPeriodId;
                command.Parameters["@V_RESULT"].Direction = ParameterDirection.Output;
                command.Parameters["@V_ERROR_MSG"].Direction = ParameterDirection.Output;

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);

                bool result = (command.Parameters["@V_RESULT"].Value != DBNull.Value) ? (command.Parameters["@V_RESULT"].Value.ToString() == "T") : false;

                if (result)
                {
                    DataTable dailyTestLocationSuppData = dataSet.Tables[0];
                    DataTable dailyTestSystemSuppData = dataSet.Tables[1];

                    if (isDailyTestSupplementalDataTable(dailyTestLocationSuppData, out errorMessage) && 
                        isDailyTestSystemSupplementalDataTable(dailyTestSystemSuppData, out errorMessage))
                    {
                        if (dailyTestLocationSuppData.Rows.Count > 0)
                        {
                            foreach (DataRowView locationSuppDataRow in dailyTestLocationSuppData.DefaultView)
                            {
                                DataView systemSuppDataRows = new DataView(dailyTestSystemSuppData, string.Format("DAILY_TEST_SUPP_DATA_ID = '{0}'", locationSuppDataRow["DAILY_TEST_SUPP_DATA_ID"]), "", DataViewRowState.CurrentRows);

                                try
                                {
                                    cDailyCalibrationTestData testData = new cDailyCalibrationTestData(locationSuppDataRow, systemSuppDataRows);

                                    cDailyCalibrationComponentData componentData;
                                    {
                                        if (ComponentData.ContainsKey(testData.ComponentId))
                                            componentData = ComponentData[testData.ComponentId];
                                        else
                                        {
                                            componentData = new cDailyCalibrationComponentData(testData.ComponentId, testData.ComponentIdentifier);
                                            ComponentData.Add(testData.ComponentId, componentData);

                                            // Add component to the list of components for the component's location
                                            if (LocationComponentList.ContainsKey(testData.MonLocId))
                                                LocationComponentList[testData.MonLocId].Add(testData.ComponentId);
                                            else
                                                LocationComponentList.Add(testData.MonLocId, new List<String> { testData.ComponentId });
                                        }
                                    }

                                    if (!componentData.Update(testData, out errorMessage))
                                    {
                                        result = false;
                                        break;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    errorMessage = ex.Message;
                                    result = false;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            errorMessage = "No Daily (Calibration) Test Supplemental Data exists for the previous quarter.";
                            result = false;
                        }
                    }
                    else
                    {
                        result = false;
                    }

                }
                else
                {
                    errorMessage = (command.Parameters["@V_ERROR_MSG"].Value != DBNull.Value) ? command.Parameters["@V_ERROR_MSG"].Value.ToString() : "Stored Procedure did not return an error message.";
                }

                return result;
            }
            catch (SqlException sqlEx)
            {
                errorMessage = string.Format("Procedure: {0} - {1}", sqlEx.Procedure, sqlEx.Message);
                return false;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
            finally
            {
                command.Dispose();
                command = null;
            }

        }

        /// <summary>
        /// Updates the supplemental data load tables for each component.
        /// </summary>
        /// <param name="rptPeriodId"></param>
        /// <param name="workspaceSessionId"></param>
        /// <param name="connection"></param>
        public void LoadIntoSupplementalDataTables(int rptPeriodId, decimal workspaceSessionId, SqlConnection connection)
        {
            SupplementalDataUpdateLocationDataTable = cDataFunctions.CreateDataTable(SupplementalDataUpdateDatabaseName, SupplementalDataUpdateSchemaName, SupplementalDataUpdateLocationTableName, connection);
            SupplementalDataUpdateSystemDataTable = cDataFunctions.CreateDataTable(SupplementalDataUpdateDatabaseName, SupplementalDataUpdateSchemaName, SupplementalDataUpdateSystemTableName, connection);

            if ((SupplementalDataUpdateLocationDataTable != null) && (SupplementalDataUpdateSystemDataTable != null))
            {
                foreach (string componentId in ComponentData.Keys)
                {
                    ComponentData[componentId].LoadIntoSupplementalDataTables(SupplementalDataUpdateLocationDataTable, SupplementalDataUpdateSystemDataTable, rptPeriodId, workspaceSessionId);
                }
            }
        }

        /// <summary>
        /// Updates the most resent calibration information.
        /// </summary>
        /// <param name="errorMessage">The error message for any error produced</param>
        /// <returns>True if the update was successful.</returns>
        public bool Update(out string errorMessage)
        {
            bool result;

            try
            {
                cDailyCalibrationTestData testData = new cDailyCalibrationTestData(DailyCalibrationSeverityCd);

                cDailyCalibrationComponentData componentData;
                {
                    if (ComponentData.ContainsKey(testData.ComponentId))
                        componentData = ComponentData[testData.ComponentId];
                    else
                    {
                        componentData = new cDailyCalibrationComponentData(testData.ComponentId, testData.ComponentIdentifier);
                        ComponentData.Add(testData.ComponentId, componentData);

                        // Add component to the list of components for the component's location
                        if (LocationComponentList.ContainsKey(testData.MonLocId))
                            LocationComponentList[testData.MonLocId].Add(testData.ComponentId);
                        else
                            LocationComponentList.Add(testData.MonLocId, new List<String> { testData.ComponentId });
                    }
                }

                result = componentData.Update(testData, out errorMessage);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Updates the most resent calibration information.
        /// 
        /// Reports any errors in the debug output log.
        /// </summary>
        public void Update()
        {
            string errorMessage;

            if (!Update(out errorMessage))
            {
                System.Diagnostics.Debug.WriteLine("");
                System.Diagnostics.Debug.WriteLine(string.Format("[cDailyCalibrationData.Update]: {0}", errorMessage));
                System.Diagnostics.Debug.WriteLine("");
            }
        }

        /// <summary>
        /// Calls UpdateOperatingInformation on ComponentData item for each Component Id associated with the passed monLocId.
        /// </summary>
        /// <param name="monLocId">MON_LOC_ID for the location being updated.</param>
        /// <param name="opHour">The operating hour for which the update is occuring.</param>
        /// <param name="opTime">The operating time for the hour.</param>
        /// <param name="systemOpTimeDictionary">Contains the MON_SYS_ID and operating times for the primary and primary bypass systems When a primary bypass is active.</param>
        public void UpdateOperatingInformation(string monLocId, DateTime opHour, decimal opTime, Dictionary<string, decimal> systemOpTimeDictionary)
        {
            if (LocationComponentList.ContainsKey(monLocId))
                foreach (string componentId in LocationComponentList[monLocId])
                    if (ComponentData.ContainsKey(componentId))
                        ComponentData[componentId].UpdateOperatingInformation(monLocId, opHour, opTime, systemOpTimeDictionary);
        }

        #endregion


        #region Static Properties

        /// <summary>
        /// Contains the update data table object for supplemental data.
        /// </summary>
        public static DataTable SupplementalDataUpdateLocationDataTable { get; set; }

        /// <summary>
        /// Contains the update data table object for supplemental data.
        /// </summary>
        public static DataTable SupplementalDataUpdateSystemDataTable { get; set; }

        /// <summary>
        /// Contains the name of the update database for the supplemental data.
        /// </summary>
        public static string SupplementalDataUpdateDatabaseName { get { return "ECMPS_WS"; } }

        /// <summary>
        /// Contains the name of the update schema for the supplemental data.
        /// </summary>
        public static string SupplementalDataUpdateSchemaName { get { return "Supp"; } }

        /// <summary>
        /// Contains the name of the update table for sampling trian supplemental data.
        /// </summary>
        public static string SupplementalDataUpdateLocationTableName { get { return "CE_DailyTestSuppData"; } }

        /// <summary>
        /// Contains the name of the update table for sampling trian supplemental data.
        /// </summary>
        public static string SupplementalDataUpdateLocationTablePath { get { return SupplementalDataUpdateDatabaseName + "." + SupplementalDataUpdateSchemaName + "." + SupplementalDataUpdateLocationTableName; } }

        /// <summary>
        /// Contains the name of the update table for sampling trian supplemental data.
        /// </summary>
        public static string SupplementalDataUpdateSystemTableName { get { return "CE_DailyTestSystemSuppData"; } }

        /// <summary>
        /// Contains the name of the update table for sampling trian supplemental data.
        /// </summary>
        public static string SupplementalDataUpdateSystemTablePath { get { return SupplementalDataUpdateDatabaseName + "." + SupplementalDataUpdateSchemaName + "." + SupplementalDataUpdateSystemTableName; } }

        #endregion


        #region Private Methods

        /// <summary>
        /// Determines whether the passed table meets the minimum column requirement to be a Daily Test (Location) Supplemental Data table.
        /// </summary>
        /// <param name="table">The table to check.</param>
        /// <param name="errorMessage">The error message with missing columns.</param>
        /// <returns></returns>
        private bool isDailyTestSupplementalDataTable(DataTable table, out string errorMessage)
        {
            bool result = true;

            string[] columnNames = { "CALC_TEST_RESULT_CD", "COMPONENT_ID", "COMPONENT_IDENTIFIER", "DAILY_TEST_DATE", "DAILY_TEST_DATEHOURMIN", "DAILY_TEST_HOUR", "DAILY_TEST_MIN",
                                     "DAILY_TEST_SUM_ID", "DAILY_TEST_SUPP_DATA_ID", "FIRST_OP_AFTER_NONOP_DATEHOUR", "KEY_ONLINE_IND", "KEY_VALID_IND", "LAST_COVERED_NONOP_DATEHOUR",
                                     "MON_LOC_ID", "ONLINE_OFFLINE_IND", "OP_HOUR_CNT", "RPT_PERIOD_ID", "SORT_DAILY_TEST_DATEHOURMIN", "SPAN_SCALE_CD", "TEST_RESULT_CD", "TEST_TYPE_CD" };

            string missingColumns = null;

            if (CheckTableColumns(table, columnNames, out missingColumns))
            {
                errorMessage = null;
            }
            else
            {
                errorMessage = string.Format("Daily Test (Location) Supplemental Data table is missing columns: {0}", missingColumns);
            }

            return result;
        }

        /// <summary>
        /// Determines whether the passed table meets the minimum column requirement to be a Daily Test System Supplemental Data table.
        /// </summary>
        /// <param name="table">The table to check.</param>
        /// <param name="errorMessage">The error message with missing columns.</param>
        /// <returns></returns>
        private bool isDailyTestSystemSupplementalDataTable(DataTable table, out string errorMessage)
        {
            bool result = true;

            string[] columnNames = {"DAILY_TEST_SUPP_DATA_ID", "FIRST_OP_AFTER_NONOP_DATEHOUR", "LAST_COVERED_NONOP_DATEHOUR", "MON_LOC_ID", "MON_SYS_ID", "OP_HOUR_CNT", "RPT_PERIOD_ID" };

            string missingColumns = null;

            if (CheckTableColumns(table, columnNames, out missingColumns))
            {
                errorMessage = null;
            }
            else
            {
                errorMessage = string.Format("Daily Test System Supplemental Data table is missing columns: {0}", missingColumns);
            }

            return result;
        }

        #region Helper Methods

        /// <summary>
        /// Determines whether a table contains a minimum list of columns.
        /// </summary>
        /// <param name="table">The table to check.</param>
        /// <param name="columnNames">The list of columns the table must contain.  The table can contain additional columns.</param>
        /// <param name="missingColumns">Contains a comma delimited list of columns that are not contained in the table.</param>
        /// <returns> Returns true if the table is defined and the columns in the array are all columns in the table.  Otherwise returns false.</returns>
        private bool CheckTableColumns(DataTable table, string[] columnNames, out string missingColumns)
        {
            bool result = true;

            missingColumns = null;

            foreach (string columnName in columnNames)
            {
                if (!string.IsNullOrWhiteSpace(columnName))
                {
                    if ((table == null) || !table.Columns.Contains(columnName))
                    {
                        missingColumns.ListAdd(columnName);
                        result = false;
                    }
                }
            }

            return result;
        }

        #endregion

        #endregion
    }
}
