using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using ECMPS.Checks.DatabaseAccess;
using ECMPS.Checks.TypeUtilities;


namespace ECMPS.Checks.CheckEngine.SpecialParameterClasses
{

    /// <summary>
    /// Object used to track the last Daily Interference Check for components.
    /// </summary>
    public class cLastDailyInterferenceCheck
    {

        #region Public Constructors

        /// <summary>
        /// Creates an object used to track and return the last Daily Interference Check record
        /// that matches particular conditions.
        /// </summary>
        public cLastDailyInterferenceCheck()
        {
            ComponentDictionary = new Dictionary<string, cLastDailyInterferenceCheckForComponent>();
            LocationComponentList = new Dictionary<string, List<string>>();
        }

        #endregion


        #region Public Methods

        /// <summary>
        /// Adds the current row based on component id and span scale,
        /// limited by the condition set on instantiation.
        /// </summary>
        /// <param name="dailyInterferenceCheckRow">The row to add.</param>
        /// <param name="calculatedTestResultCd">The check engine calculated test result.</param>
        /// <param name="currentOpHour">The current operating hour.</param>
        /// <param name="resultMessage">Reason for failure.</param>
        /// <returns>True if addition was successful.</returns>
        public bool Add(DataRowView dailyInterferenceCheckRow, string calculatedTestResultCd, DateTime currentOpHour, ref string resultMessage)
        {
            bool result;

            bool fromPreviousQuarter = false;

            cLastDailyInterferenceCheckTest latestTest;

            if (cLastDailyInterferenceCheckTest.CreateInstance(dailyInterferenceCheckRow, calculatedTestResultCd, fromPreviousQuarter, out latestTest, out resultMessage))
            {
                cLastDailyInterferenceCheckForComponent component = GetOrCreateComponent(latestTest);
                {
                    component.Add(latestTest, currentOpHour, ref resultMessage);
                }

                result = true;
            }
            else
                result = false;

            return result;
        }


        /// <summary>
        /// Returns the last Daily Interference Check row for the component id 
        /// </summary>
        /// <param name="componentId">The component id for which to find the last daily interference.</param>
        /// <param name="online">Find online test if true and offline if false.</param>
        /// <param name="latestTest">The daily interference check object found.</param>
        /// <returns>Returns true if the last daily interference check was found.</returns>
        public bool Get(string componentId, bool online, out cLastDailyInterferenceCheckTest latestTest)
        {
            bool result;

            if (ComponentDictionary.ContainsKey(componentId))
            {
                latestTest = ComponentDictionary[componentId].Get(online);
                result = (latestTest != null);
            }
            else
            {
                latestTest = null;
                result = false;
            }

            return result;
        }


        /// <summary>
        /// Returns the last Daily Interference Check row for the component id
        /// </summary>
        /// <param name="componentId">The component id for which to find the last daily interference.</param>
        /// <param name="online">Find online test if true and offline if false.</param>
        /// <returns>Retrn the daily interference check row found.</returns>
        public DataRowView GetTestRow(string componentId, bool online)
        {
            DataRowView result;

            cLastDailyInterferenceCheckTest interferenceCheckTest;

            Get(componentId, online, out interferenceCheckTest);

            result = (interferenceCheckTest != null) ? interferenceCheckTest.DailyInterferenceCheckRow : null;

            return result;
        }


        /// <summary>
        /// Returns the last Daily Interference Check object for the component id 
        /// </summary>
        /// <param name="componentId">The component id for which to find the last daily interference.</param>
        /// <param name="online">Find online test if true and offline if false.</param>
        /// <returns>Returns the test object if the last daily interference check was found, otherwise return null..</returns>
        public cLastDailyInterferenceCheckTest Get(string componentId, bool online)
        {
            cLastDailyInterferenceCheckTest result;

            if (ComponentDictionary.ContainsKey(componentId))
            {
                result = ComponentDictionary[componentId].Get(online);
            }
            else
            {
                result = null;
            }

            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="monPlanId"></param>
        /// <param name="rptPeriodId"></param>
        /// <param name="connection"></param>
        /// <param name="resultMessage"></param>
        /// <returns></returns>
        public bool InitializeFromPreviousQuarter(string monPlanId, int rptPeriodId, SqlConnection connection, ref string resultMessage)
        {
            if (connection == null)
            {
                resultMessage = "Connection does not exists while initializing with Daily Interference Supplemental Data from the previous quarter.";
                return false;
            }
            else if (connection.State != ConnectionState.Open)
            {
                resultMessage = "Connection was not open while initializing with Daily Interference Supplemental Data from the previous quarter.";
                return false;
            }

            SqlCommand command = connection.CreateCommand();

            try
            {
                command.CommandText = $"select * from ECMPS.CheckEm.DailyInterferenceSuppDataPreviousQuarter( '{monPlanId}', {rptPeriodId})";
                command.CommandType = CommandType.Text;

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dailyInterferenceSuppData = new DataTable();
                adapter.Fill(dailyInterferenceSuppData);

                bool result = (dailyInterferenceSuppData != null);

                if (result)
                {
                    string missingColumns;

                    if (cLastDailyInterferenceCheckTest.IsDailyTestTable(dailyInterferenceSuppData, out missingColumns))
                    {
                        if (dailyInterferenceSuppData.Rows.Count > 0)
                        {
                            bool fromPreviousQuarter = true;

                            cLastDailyInterferenceCheckTest latestTest;
                            cLastDailyInterferenceCheckForComponent component;

                            foreach (DataRowView dailyInterferenceSuppDataRow in dailyInterferenceSuppData.DefaultView)
                            {
                                try
                                {
                                    if (cLastDailyInterferenceCheckTest.CreateInstance(dailyInterferenceSuppDataRow, null, fromPreviousQuarter, out latestTest, out resultMessage))
                                    {
                                        component = GetOrCreateComponent(latestTest);
                                        component.InitializeFromPreviousQuarter(latestTest, ref resultMessage);

                                        result = true;
                                    }
                                    else
                                        result = false;
                                }
                                catch (Exception ex)
                                {
                                    resultMessage = ex.Message;
                                    result = false;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            resultMessage = "No Daily (Interference) Test Supplemental Data exists for the previous quarter.";
                            result = false;
                        }
                    }
                    else
                    {
                        resultMessage = $"The source supplemental data table is missing the following columns: {missingColumns}";
                        result = false;
                    }

                }
                else
                {
                    resultMessage = (command.Parameters["@V_ERROR_MSG"].Value != DBNull.Value) ? command.Parameters["@V_ERROR_MSG"].Value.ToString() : "Table Function did not return an error message.";
                }

                return result;
            }
            catch (SqlException sqlEx)
            {
                resultMessage = string.Format("Procedure: {0} - {1}", sqlEx.Procedure, sqlEx.Message);
                return false;
            }
            catch (Exception ex)
            {
                resultMessage = ex.Message;
                return false;
            }
            finally
            {
                command.Dispose();
                command = null;
            }

        }


        /// <summary>
        /// Calls UpdateOperatingInformation on ComponentData item for each Component Id associated with the passed monLocId.
        /// </summary>
        /// <param name="monLocId">MON_LOC_ID for the location being updated.</param>
        /// <param name="opHour">The operating hour for which the update is occuring.</param>
        /// <param name="opTime">The operating time for the hour.</param>
        public void UpdateOperatingInformation(string monLocId, DateTime opHour, decimal opTime)
        {
            if (LocationComponentList.ContainsKey(monLocId))
                foreach (string componentId in LocationComponentList[monLocId])
                    if (ComponentDictionary.ContainsKey(componentId))
                        ComponentDictionary[componentId].UpdateOperatingInformation(monLocId, opHour, opTime);
        }


        #region Helper Methods

        /// <summary>
        /// Returns the component for the test if it exists, otherwise creates a component and returns
        /// the new comonent.  Also adds any created components to the list of components for the 
        /// associated location.
        /// </summary>
        /// <param name="latestTest">The test for which a component object will be returned.</param>
        /// <returns>The component for the test.</returns>
        private cLastDailyInterferenceCheckForComponent GetOrCreateComponent(cLastDailyInterferenceCheckTest latestTest)
        {
            cLastDailyInterferenceCheckForComponent result;

            if (ComponentDictionary.ContainsKey(latestTest.ComponentId))
            {
                result = ComponentDictionary[latestTest.ComponentId];
            }
            else
            {
                result = new cLastDailyInterferenceCheckForComponent(latestTest.ComponentId, latestTest.MonLocId, this);
                ComponentDictionary.Add(result.ComponentId, result);

                // Add component to the list of components for the component's location
                if (LocationComponentList.ContainsKey(result.MonLocId))
                    LocationComponentList[result.MonLocId].Add(result.ComponentId);
                else
                    LocationComponentList.Add(result.MonLocId, new List<String> { result.ComponentId });
            }

            return result;
        }

        #endregion

        #endregion


        #region Private Properties

        /// <summary>
        /// Dictionary with a Component Id lookup to cLastDailyInterferenceCheckForComponent object.
        /// </summary>
        private Dictionary<string, cLastDailyInterferenceCheckForComponent> ComponentDictionary { get; set; }

        /// <summary>
        /// This tracks the COMPONENT_ID values that belong to a particular MON_LOC_ID.
        /// </summary>
        private Dictionary<string, List<string>> LocationComponentList { get; set; }

        #endregion


        #region Static Properties

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
        public static string SupplementalDataUpdateTableName { get { return "CE_DailyTestSuppData"; } }

        /// <summary>
        /// Contains the name of the update table for sampling trian supplemental data.
        /// </summary>
        public static string SupplementalDataUpdateTablePath { get { return SupplementalDataUpdateDatabaseName + "." + SupplementalDataUpdateSchemaName + "." + SupplementalDataUpdateTableName; } }


        /// <summary>
        /// Gathers the supplemental data from a cLastDailyInterferenceCheck object and loads them inteo the ECMPS_WS table for the data.
        /// </summary>
        /// <param name="lastDailyInterferenceCheckList">The object containing the supplemental data to load.</param>
        /// <param name="rptPeriodId">The report period for the emissions being evaluated.</param>
        /// <param name="workspaceSessionId">The workspace session id for the current evaluation.</param>
        /// <param name="ecmpsWsDatabase">The object for the ECMPS_WS database.</param>
        /// <param name="errorMessage">The error message returned if the bulk load fails.</param>
        /// <returns>Returns true if the bulk load is successful.</returns>
        public static bool SaveSupplementalData(cLastDailyInterferenceCheck lastDailyInterferenceCheckList, int rptPeriodId, decimal workspaceSessionId, cDatabase ecmpsWsDatabase,
                                                ref string errorMessage)
        {
            DataTable supplementalDataUpdateTable = cDataFunctions.CreateDataTable(SupplementalDataUpdateDatabaseName, SupplementalDataUpdateSchemaName, SupplementalDataUpdateTableName, ecmpsWsDatabase.SQLConnection);

            if (supplementalDataUpdateTable != null)
            {
                foreach (string componentId in lastDailyInterferenceCheckList.ComponentDictionary.Keys)
                {
                    lastDailyInterferenceCheckList.ComponentDictionary[componentId].LoadIntoSupplementalDataTables(supplementalDataUpdateTable, rptPeriodId, workspaceSessionId);
                }
            }

            bool result = ecmpsWsDatabase.BulkLoad(supplementalDataUpdateTable, SupplementalDataUpdateTablePath, ref errorMessage);

            return result;
        }

        #endregion

    }

}
