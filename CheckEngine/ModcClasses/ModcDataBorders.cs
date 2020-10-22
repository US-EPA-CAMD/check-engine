using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

using ECMPS.Checks.TypeUtilities;
using ECMPS.Definitions.Extensions;


namespace ECMPS.Checks.CheckEngine
{
    /// <summary>
    /// Modc Data borders class
    /// </summary>
    public class cModcDataBorders
    {

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="AHourlyTable"></param>
        /// <param name="AValueColumnName"></param>
        /// <param name="AModcList"></param>
        /// <param name="ABorderModc"></param>
        /// <param name="locationView"></param>
        public cModcDataBorders(DataTable AHourlyTable, string AValueColumnName, int[] AModcList, bool ABorderModc, DataView locationView)
        {
            FHourlyTable = AHourlyTable;
            ValueColumnName = AValueColumnName;
            FBorderModc = ABorderModc;

            for (int Dex = 0; Dex <= 55; Dex++) { FModcList[Dex] = false; }
            foreach (int ModcCd in AModcList) { FModcList[ModcCd] = true; }

            if ((locationView != null) && (locationView.Count > 0))
            {
                ModcDataBorderLocations = new cModcDataBorderItem[locationView.Count];

                for (int RowPosition = 0; RowPosition < locationView.Count; RowPosition++)
                {
                    ModcDataBorderLocations[RowPosition] = new cModcDataBorderItem(this, cDBConvert.ToString(locationView[RowPosition]["Mon_Loc_Id"]));
                }
            }
            else
                ModcDataBorderLocations = null;

            ModcDataBorderLocationSystems = null;
        }

        /// <summary>
        /// Creates MODC Data Borders object for a check category with the given MODC.  If an array of MON_SYS_ID lists are provided, 
        /// the object will track borders for locations and systesm.
        /// </summary>
        /// <param name="AHourlyTable">The primary table of hourly data for the category.</param>
        /// <param name="AValueColumnName">The value column in the hourly data table.</param>
        /// <param name="AModcList">The list of MODC to use.</param>
        /// <param name="ABorderModc">Indicates whether the MODC list are border MODC.</param>
        /// <param name="locationView">View containing the locations for the emission report's monitoring plan.</param>
        /// <param name="hourlyTypeCd">The Hourly Type Code associated with the MODC Data Borders Dictionary.</param>
        /// <param name="parameterCd">The Parameter Code associated with the MODC Data Borders Dictionary.</param>
        /// <param name="moistureBasis">The Moisture Basis associated with the MODC Data Borders Dictionary.  Null when a Moisture Basis is not applicable.</param>
        /// <param name="locationMonSysIdList">Array of MON_SYS_ID lists.  Null if systems are not being tracked, otheriwse contains an array element for each location in the same position as in locationView.  Each element contains a List containing MON_SYS_ID to track.</param>
        public cModcDataBorders(DataTable AHourlyTable, string AValueColumnName, int[] AModcList, bool ABorderModc, DataView locationView,
                                string hourlyTypeCd, string parameterCd, string moistureBasis, 
                                List<string>[] locationMonSysIdList = null)
        {
            FHourlyTable = AHourlyTable;
            ValueColumnName = AValueColumnName;
            FBorderModc = ABorderModc;

            for (int Dex = 0; Dex <= 55; Dex++) { FModcList[Dex] = false; }
            foreach (int ModcCd in AModcList) { FModcList[ModcCd] = true; }

            if ((locationView != null) && (locationView.Count > 0))
            {
                ModcDataBorderLocations = new cModcDataBorderItem[locationView.Count];

                ModcDataBorderLocationSystems = ((locationMonSysIdList != null) && (locationMonSysIdList.Length == locationView.Count))
                                              ? new Dictionary<string, cModcDataBorderItem>[locationView.Count]
                                              : null;

                for (int RowPosition = 0; RowPosition < locationView.Count; RowPosition++)
                {
                    string monLocId = cDBConvert.ToString(locationView[RowPosition]["Mon_Loc_Id"]);

                    ModcDataBorderLocations[RowPosition] = new cModcDataBorderItem(this, monLocId);

                    if (ModcDataBorderLocationSystems != null)
                    {
                        ModcDataBorderLocationSystems[RowPosition] = new Dictionary<string, cModcDataBorderItem>();

                        foreach (string monSysId in locationMonSysIdList[RowPosition])
                            ModcDataBorderLocationSystems[RowPosition].Add(monSysId, new cModcDataBorderItem(this, monLocId, monSysId));
                    }
                }

                AddModcDataBordersFromCategory(hourlyTypeCd, parameterCd, moistureBasis, this);
            }
            else
            {
                ModcDataBorderLocations = null;
                ModcDataBorderLocationSystems = null;
            }
        }

        #endregion


        #region Static MODC Data Borders Dictionary Methods and Properties

        /// <summary>
        /// Contains a dictionary containing the parameter and moisture-basis key and the cModcDataBorders item.
        /// Values are loaded into the dictionary by hourly check category objects.
        /// </summary>
        private static Dictionary<string, cModcDataBorders> ModcDataBordersDictionary { get; set; }


        /// <summary>
        /// Adds the MODC Data Borders object for the Parameter Code, and Moisture Basis when applicable, to the MODC Data Borders Dictionary.
        /// </summary>
        /// <param name="hourlyTypeCd">The Hourly Type Code associated with the MODC Data Borders Dictionary.</param>
        /// <param name="parameterCd">The Parameter Code associated with the MODC Data Borders Dictionary.</param>
        /// <param name="moistureBasis">The Moisture Basis associated with the MODC Data Borders Dictionary.  Null when a Moisture Basis is not applicable.</param>
        /// <param name="modcDataBorders"></param>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when locationPos is less than zero or greater than the position indexes in ModcDataBordersDictionaryList.</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when hourlyTypeCd, parameterCd or modcDataBorders parameters are null.</exception>
        /// <exception cref="System.ArgumentException">Thrown when hourlyTypeCd, parameterCd or moistureBasis parameters have empty string or whitespace values, 
        /// or when they already contain a dictionary entry.</exception>
        public static void AddModcDataBordersFromCategory(string hourlyTypeCd, string parameterCd, string moistureBasis, cModcDataBorders modcDataBorders)
        {
            // Throw exception if an input value has a value that is not unallowed.
            if (hourlyTypeCd == null) throw new ArgumentNullException("hourlyTypeCd");
            if (parameterCd == null) throw new ArgumentNullException("parameterCd");
            if (modcDataBorders == null) throw new ArgumentNullException("modcDataBorders");
            if (string.IsNullOrWhiteSpace(hourlyTypeCd)) throw new ArgumentException("Argument empty string and whitespace values are not allowed.", "hourlyTypeCd");
            if (string.IsNullOrWhiteSpace(parameterCd)) throw new ArgumentException("Argument empty string and whitespace values are not allowed.", "parameterCd");
            if (string.IsNullOrWhiteSpace(moistureBasis) && (moistureBasis != null)) throw new ArgumentException("Argument empty string and whitespace values are not allowed.", "moistureBasis");

            // Initialize the dictionary if it has not been initialized
            if (ModcDataBordersDictionary == null) ModcDataBordersDictionary = new Dictionary<string, cModcDataBorders>();

            // Formation Dictionary Key.
            string dictionaryKey = GetParameterMoistureBasisKey(hourlyTypeCd, parameterCd, moistureBasis);

            // Throw exception if the supplied parameterCd and moistureBasis combination already exists in the dictionary.
            if (ModcDataBordersDictionary.ContainsKey(dictionaryKey)) throw new ArgumentException($"MODC Data Borders Dictionary already contains an entry for {dictionaryKey}.");

            // Add modcDataBorders to the dictionary for hourlyTypeCd, parameterCd and moistureBasis. 
            ModcDataBordersDictionary.Add(dictionaryKey, modcDataBorders);
        }


        /// <summary>
        /// Initializes the MODC Data Borders Dictionary.
        /// </summary>
        public static bool InitializeModcDataBordersDictionary()
        {
            ModcDataBordersDictionary = new Dictionary<string, cModcDataBorders>();
            return true;
        }


        /// <summary>
        /// Loads Hour Before Supplemental Data from the previous quarter.
        /// </summary>
        /// <param name="monPlanId">The MON_PLAN_ID for the emission report monitoring plan.</param>
        /// <param name="rptPeriodId">The RPT_PERIOD_ID for the emission report quarter.</param>
        /// <param name="connection">The current "data" connection used by the check engine.</param>
        /// <param name="monitorLocationView">Contains the table with the locations used by the check engine.</param>
        /// <param name="currentYear">The year of the emission report quarter.</param>
        /// <param name="currentQuarter">The quarter of the emission report quarter.</param>
        /// <param name="errorMessage">The error message reference which is updated if an error occurs during the initialization.</param>
        /// <returns>Indicates whether the initialization was successful.</returns>
        public static bool InitializeFromPreviousQuarter(string monPlanId, int rptPeriodId, SqlConnection connection, DataView monitorLocationView,
                                                         int currentYear, int currentQuarter, ref string errorMessage)
        {
            string missingColumns;

            if (connection == null)
            {
                errorMessage = "Connection does not exists while initializing with Hour Before Supplemental Data from the previous quarter.";
                return false;
            }
            else if (connection.State != ConnectionState.Open)
            {
                errorMessage = "Connection was not open while initializing with Hour Before Supplemental Data from the previous quarter.";
                return false;
            }
            else if (monitorLocationView == null)
            {
                errorMessage = "monitorLocationTable is null and a table is required.";
                return false;
            }
            else if (!IsMonitorLocationTable(monitorLocationView.Table, out missingColumns))
            {
                errorMessage = "monitorLocationTable is not monitor location table.";
                return false;
            }

            SqlCommand command = connection.CreateCommand();

            try
            {
                command.CommandText = "ECMPS.CheckEm.HourBeforeSuppDataPreviousQuarter";
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

                if (!result)
                {
                    errorMessage = (command.Parameters["@V_ERROR_MSG"].Value != DBNull.Value) ? command.Parameters["@V_ERROR_MSG"].Value.ToString() : "Stored Procedure did not return an error message.";
                }
                else
                {
                    DataTable hourBeforeLocationSuppData = dataSet.Tables[0];
                    DataTable hourBeforeSystemSuppData = dataSet.Tables[1];

                    if (!IsSuppDataLocationTable(hourBeforeLocationSuppData, out missingColumns))
                    {
                        errorMessage = (missingColumns != null)
                                     ? $"The following columns are missing from the location level HBHA Supplemental Data: {missingColumns}"
                                     : "The location level HBHA Supplemental Data does not contain all of the expected data columns.";

                        result = false;
                    }
                    else if (!IsSuppDataSystemTable(hourBeforeSystemSuppData, out missingColumns))
                    {
                        errorMessage = (missingColumns != null)
                                     ? $"The following columns are missing from the system level HBHA Supplemental Data: {missingColumns}"
                                     : "The system level HBHA Supplemental Data does not contain all of the expected data columns.";

                        result = false;
                    }
                    else if (hourBeforeLocationSuppData.Rows.Count > 0)
                    {
                        for (int locationPos = 0; locationPos < monitorLocationView.Count; locationPos++)
                        {
                            string monLocId = monitorLocationView[locationPos]["MON_LOC_ID"].AsString();

                            DataView hourBeforeLocationSuppDataView = new DataView(hourBeforeLocationSuppData, $"MON_LOC_ID = '{monLocId}'", "MON_LOC_ID", DataViewRowState.CurrentRows);

                            foreach (DataRowView locationSuppDataRow in hourBeforeLocationSuppDataView)
                            {
                                string hourlyTypeCd = locationSuppDataRow["HOURLY_TYPE_CD"].AsString();
                                string parameterCd = locationSuppDataRow["PARAMETER_CD"].AsString();
                                string moistureBasis = locationSuppDataRow["MOISTURE_BASIS"].AsString();

                                try
                                {
                                    InitializeFromPreviousQuarterDo(hourlyTypeCd, parameterCd, moistureBasis, locationPos,
                                                                    locationSuppDataRow["OP_DATEHOUR"].AsBeginDateTime(), 
                                                                    locationSuppDataRow["UNADJUSTED_HRLY_VALUE"].AsDecimal(),
                                                                    locationSuppDataRow["ADJUSTED_HRLY_VALUE"].AsDecimal(),
                                                                    locationSuppDataRow["HRLY_VALUE"].AsDecimal(),
                                                                    currentYear, currentQuarter);

                                    if (hourBeforeSystemSuppData.Rows.Count > 0)
                                    {
                                        string filterFormatString = (moistureBasis == null)
                                                                  ? "MON_LOC_ID = '{0}' and HOURLY_TYPE_CD = '{1}' and PARAMETER_CD = '{2}' and MOISTURE_BASIS is null"
                                                                  : "MON_LOC_ID = '{0}' and HOURLY_TYPE_CD = '{1}' and PARAMETER_CD = '{2}' and MOISTURE_BASIS = '{3}'";

                                        DataView hourBeforeSystemSuppDataView = new DataView(hourBeforeSystemSuppData,
                                                                                             string.Format(filterFormatString, monLocId, hourlyTypeCd, parameterCd, moistureBasis),
                                                                                             "",
                                                                                             DataViewRowState.CurrentRows);

                                        foreach (DataRowView systemSuppDataRow in hourBeforeSystemSuppDataView)
                                        {
                                            InitializeFromPreviousQuarterDo(hourlyTypeCd, parameterCd, moistureBasis, locationPos,
                                                                            systemSuppDataRow["MON_SYS_ID"].AsString(),
                                                                            systemSuppDataRow["OP_DATEHOUR"].AsBeginDateTime(),
                                                                            systemSuppDataRow["UNADJUSTED_HRLY_VALUE"].AsDecimal(),
                                                                            systemSuppDataRow["ADJUSTED_HRLY_VALUE"].AsDecimal(),
                                                                            systemSuppDataRow["HRLY_VALUE"].AsDecimal(),
                                                                            currentYear, currentQuarter);
                                        }
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
                    }
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


        #region Helper Methods

        private static string[] MonitorLocationColumns = { "LOCATION_NAME", "MON_LOC_ID" };

        private static string[] SuppDataLocationColumns = { "ADJUSTED_HRLY_VALUE", "HOURLY_TYPE_CD", "HRLY_VALUE", "LAST_QA_VALUE_SUPP_DATA_ID",
                                                            "MOISTURE_BASIS", "MON_LOC_ID", "MON_PLAN_ID", "OP_DATEHOUR", "PARAMETER_CD",
                                                            "PRIMARY_BYPASS_IND", "RPT_PERIOD_ID", "UNADJUSTED_HRLY_VALUE" };

        private static string[] SuppDatSystemColumns = { "ADJUSTED_HRLY_VALUE", "HOURLY_TYPE_CD", "HRLY_VALUE", "LAST_QA_VALUE_SUPP_DATA_ID",
                                                         "MOISTURE_BASIS", "MON_LOC_ID", "MON_PLAN_ID", "MON_SYS_ID", "OP_DATEHOUR",
                                                         "PARAMETER_CD", "RPT_PERIOD_ID", "UNADJUSTED_HRLY_VALUE" };


        /// <summary>
        /// Determines whether a column collection contains a list of columns.
        /// </summary>
        /// <param name="columnCollection">The columns to check.</param>
        /// <param name="columnNames">The list of columns the table must contain.  The table can contain additional columns.</param>
        /// <param name="missingColumns">Contains a comma delimited list of columns that are not contained in the column collection.</param>
        /// <returns> Returns true if the column collection is not null and the columns in the array are all columns in the column collection.  Otherwise returns false.</returns>
        private static bool ContainsAllColumns(DataColumnCollection columnCollection, string[] columnNames, out string missingColumns)
        {
            bool result;

            if (columnCollection != null)
            {
                result = true;

                missingColumns = "";

                foreach (string columnName in columnNames)
                {
                    if (!string.IsNullOrWhiteSpace(columnName))
                    {
                        if ((columnCollection == null) || !columnCollection.Contains(columnName))
                        {
                            missingColumns.ListAdd(columnName);
                            result = false;
                        }
                    }
                }

                missingColumns = result ? null : ReadFormatCommaDelimitedList(missingColumns);
            }
            else
            {
                missingColumns = "All";
                result = false;
            }

            return result;
        }


        /// <summary>
        /// Determines whether the column collection for a data row view contains a list of columns.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="columnNames">The list of columns the table must contain.  The table can contain additional columns.</param>
        /// <param name="missingColumns">Contains a comma delimited list of columns that are not contained in the table.</param>
        /// <returns> Returns true if the column collection is not null and the columns in the array are all columns in the table.  Otherwise returns false.</returns>
        private static bool ContainsAllColumns(DataRowView row, string[] columnNames, out string missingColumns)
        {
            bool result;

            if ((row != null) &&
                (row.Row != null) &&
                (row.Row.Table != null))
            {
                result = ContainsAllColumns(row.Row.Table.Columns, columnNames, out missingColumns);
            }
            else
            {
                missingColumns = "All";
                result = false;
            }

            return result;
        }


        /// <summary>
        /// Returns a normalized hourly type, parameter and mositure basis key based on the provided values.
        /// </summary>
        /// <param name="hourlyTypeCd">The hourly type part of the key.</param>
        /// <param name="parameterCd">The parameter part of the key.</param>
        /// <param name="moistureBasis">The moisture basis of the key.</param>
        /// <returns>Returns just the hourly type and parameter if moisture basis is null or whitespace, otherwise returns hourly type, parameter and moisture basis separated by a hypen.  
        /// The hourly type, parameter and moisture basis are trimmed and uppercased.</returns>
        private static string GetParameterMoistureBasisKey(string hourlyTypeCd, string parameterCd, string moistureBasis)
        {
            string result;

            if (string.IsNullOrWhiteSpace(moistureBasis))
                result = $"{hourlyTypeCd.Trim().ToUpper()}-{parameterCd.Trim().ToUpper()}";
            else
                result = $"{hourlyTypeCd.Trim().ToUpper()}-{parameterCd.Trim().ToUpper()}-{moistureBasis.Trim().ToUpper()}";

            return result;
        }


        /// <summary>
        /// Adds the last QA operating values to the location level MODC Data Borders Dictionary for the given location position.
        /// Skips the add if the dictionary does not exist.
        /// </summary>
        /// <param name="hourlyTypeCd">The Hourly Type Code associated with the MODC Data Borders Dictionary.</param>
        /// <param name="parameterCd">The Parameter Code associated with the MODC Data Borders Dictionary.</param>
        /// <param name="moistureBasis">The Moisture Basis associated with the MODC Data Borders Dictionary.  Null when a Moisture Basis is not applicable.</param>
        /// <param name="locationPos">The position emissions checks position for the affected locaiton.</param>
        /// <param name="lastOpDateHour">The last QA operating hour from supplemental data.</param>
        /// <param name="lastUnadjustedValue">The last QA unadjusted value from supplemental data.</param>
        /// <param name="lastAdjustedValue">The last QA adjusted value from supplemental data.</param>
        /// <param name="lastValue">The last QA adjusted/unadjusted value from supplemental data.</param>
        /// <param name="currentYear">The year of the emission report quarter.</param>
        /// <param name="currentQuarter">The quarter of the emission report quarter.</param>
        public static void InitializeFromPreviousQuarterDo(string hourlyTypeCd, string parameterCd, string moistureBasis, int locationPos,
                                                           DateTime lastOpDateHour, decimal? lastUnadjustedValue, decimal? lastAdjustedValue, decimal? lastValue, 
                                                           int currentYear, int currentQuarter)
        {
            // Throw exception if an input value has a value that is not unallowed.
            if (locationPos < 0) throw new ArgumentOutOfRangeException("locationPos", $"The value of {locationPos} is less than zero, the minimum value.");
            if (hourlyTypeCd == null) throw new ArgumentNullException("hourlyTypeCd");
            if (parameterCd == null) throw new ArgumentNullException("parameterCd");
            if (string.IsNullOrWhiteSpace(hourlyTypeCd)) throw new ArgumentException("Argument empty string and whitespace values are not allowed.", "hourlyTypeCd");
            if (string.IsNullOrWhiteSpace(parameterCd)) throw new ArgumentException("Argument empty string and whitespace values are not allowed.", "parameterCd");
            if (string.IsNullOrWhiteSpace(moistureBasis) && (moistureBasis != null)) throw new ArgumentException("Argument empty string and whitespace values are not allowed.", "moistureBasis");

            // Formation Dictionary Key.
            string dictionaryKey = GetParameterMoistureBasisKey(hourlyTypeCd, parameterCd, moistureBasis);

            // Update the dictionary Item if it exists.
            if ((ModcDataBordersDictionary != null) && ModcDataBordersDictionary.ContainsKey(dictionaryKey) 
                                                    && (ModcDataBordersDictionary[dictionaryKey] != null)
                                                    && (ModcDataBordersDictionary[dictionaryKey].ModcDataBorderLocations.Length > locationPos)
                                                    && (ModcDataBordersDictionary[dictionaryKey].ModcDataBorderLocations[locationPos] != null))
            {
                // Updated the MODC Data Border Item
                ModcDataBordersDictionary[dictionaryKey].ModcDataBorderLocations[locationPos].HandleSuppDataHour(lastOpDateHour, lastUnadjustedValue, lastAdjustedValue, lastValue,
                                                                                                                 currentYear, currentQuarter);
            }
        }


        /// <summary>
        /// Adds the last QA operating values to the location level MODC Data Borders Dictionary for the given location position.
        /// Skips the add if the dictionary does not exist.
        /// </summary>
        /// <param name="hourlyTypeCd">The Hourly Type Code associated with the MODC Data Borders Dictionary.</param>
        /// <param name="parameterCd">The Parameter Code associated with the MODC Data Borders Dictionary.</param>
        /// <param name="moistureBasis">The Moisture Basis associated with the MODC Data Borders Dictionary.  Null when a Moisture Basis is not applicable.</param>
        /// <param name="monSysId">The MON_SYS_ID of the system for which supplemental data is being added..</param>
        /// <param name="locationPos">The position emissions checks position for the affected locaiton.</param>
        /// <param name="lastOpDateHour">The last QA operating hour from supplemental data.</param>
        /// <param name="lastUnadjustedValue">The last QA unadjusted value from supplemental data.</param>
        /// <param name="lastAdjustedValue">The last QA adjusted value from supplemental data.</param>
        /// <param name="lastValue">The last QA adjusted/unadjusted value from supplemental data.</param>
        /// <param name="currentYear">The year of the emission report quarter.</param>
        /// <param name="currentQuarter">The quarter of the emission report quarter.</param>
        public static void InitializeFromPreviousQuarterDo(string hourlyTypeCd, string parameterCd, string moistureBasis, int locationPos, string monSysId,
                                                           DateTime lastOpDateHour, decimal? lastUnadjustedValue, decimal? lastAdjustedValue, decimal? lastValue, 
                                                           int currentYear, int currentQuarter)
        {
            // Throw exception if an input value has a value that is not unallowed.
            if (locationPos < 0) throw new ArgumentOutOfRangeException("locationPos", $"The value of {locationPos} is less than zero, the minimum value.");
            if (hourlyTypeCd == null) throw new ArgumentNullException("hourlyTypeCd");
            if (parameterCd == null) throw new ArgumentNullException("parameterCd");
            if (string.IsNullOrWhiteSpace(hourlyTypeCd)) throw new ArgumentException("Argument empty string and whitespace values are not allowed.", "hourlyTypeCd");
            if (string.IsNullOrWhiteSpace(parameterCd)) throw new ArgumentException("Argument empty string and whitespace values are not allowed.", "parameterCd");
            if (string.IsNullOrWhiteSpace(moistureBasis) && (moistureBasis != null)) throw new ArgumentException("Argument empty string and whitespace values are not allowed.", "moistureBasis");

            // Formation Dictionary Key.
            string dictionaryKey = GetParameterMoistureBasisKey(hourlyTypeCd, parameterCd, moistureBasis);

            // Update the dictionary Item if it exists.
            if ((ModcDataBordersDictionary != null) && ModcDataBordersDictionary.ContainsKey(dictionaryKey) 
                                                    && (ModcDataBordersDictionary[dictionaryKey] != null)
                                                    && (ModcDataBordersDictionary[dictionaryKey].ModcDataBorderLocationSystems.Length > locationPos)
                                                    && (ModcDataBordersDictionary[dictionaryKey].ModcDataBorderLocationSystems[locationPos] != null)
                                                    && ModcDataBordersDictionary[dictionaryKey].ModcDataBorderLocationSystems[locationPos].ContainsKey(monSysId)
                                                    && (ModcDataBordersDictionary[dictionaryKey].ModcDataBorderLocationSystems[locationPos][monSysId] != null))
            {
                // Updated the MODC Data Border Item
                ModcDataBordersDictionary[dictionaryKey].ModcDataBorderLocationSystems[locationPos][monSysId].HandleSuppDataHour(lastOpDateHour, lastUnadjustedValue, lastAdjustedValue, lastValue, 
                                                                                                                                 currentYear, currentQuarter);
            }
        }


        /// <summary>
        /// Returns true if the table is a Location Supplemental Data row.
        /// </summary>
        /// <param name="monitorLocationTable">The row to test.</param>
        /// <param name="missingColumns">Contains a comma delimited list of columns that are not contained in the table.</param>
        /// <returns>Returns true if the row is Daily Interference Check row.</returns>
        public static bool IsMonitorLocationTable(DataTable monitorLocationTable, out string missingColumns)
        {
            bool result;

            result = ContainsAllColumns(monitorLocationTable.Columns, MonitorLocationColumns, out missingColumns);

            return result;
        }


        /// <summary>
        /// Returns true if the table is a Location Supplemental Data row.
        /// </summary>
        /// <param name="suppDataLocationTable">The row to test.</param>
        /// <param name="missingColumns">Contains a comma delimited list of columns that are not contained in the table.</param>
        /// <returns>Returns true if the row is Daily Interference Check row.</returns>
        public static bool IsSuppDataLocationTable(DataTable suppDataLocationTable, out string missingColumns)
        {
            bool result;

            result = ContainsAllColumns(suppDataLocationTable.Columns, SuppDataLocationColumns, out missingColumns);

            return result;
        }


        /// <summary>
        /// Returns true if the table is a System Supplemental Data row.
        /// </summary>
        /// <param name="suppDataSystemTable">The row to test.</param>
        /// <param name="missingColumns">Contains a comma delimited list of columns that are not contained in the table.</param>
        /// <returns>Returns true if the row is Daily Interference Check row.</returns>
        public static bool IsSuppDataSystemTable(DataTable suppDataSystemTable, out string missingColumns)
        {
            bool result;

            result = ContainsAllColumns(suppDataSystemTable.Columns, SuppDatSystemColumns, out missingColumns);

            return result;
        }


        /// <summary>
        /// Formats a common delimited list, stripping trailing commas and replacing the last comma with an 'and'.
        /// </summary>
        /// <param name="list">The list to format.</param>
        /// <returns>The formatted list.</returns>
        private static string ReadFormatCommaDelimitedList(string list)
        {
            string result;

            if (string.IsNullOrWhiteSpace(list))
            {
                result = null;
            }
            else
            {
                result = list.Trim();

                if (result[result.Length - 1] == ',')
                    result = result.Substring(0, list.Length - 1);

                if (result.Contains(","))
                {
                    // Replace last comma with an 'and'.
                    int lastCommaPos = result.LastIndexOf(',');
                    if (lastCommaPos != -1) result = result.Remove(lastCommaPos, 1).Insert(lastCommaPos, " and");
                }
            }

            return result;
        }

        #endregion

        #endregion


        #region Public Types

        /// <summary>
        /// border status enumeration
        /// </summary>
        public enum eBorderStatus
        {
            /// <summary>
            /// None
            /// </summary>
            None,

            /// <summary>
            /// Found
            /// </summary>
            Found,

            /// <summary>
            /// Missing
            /// </summary>
            Missing
        }

        #endregion


        #region cModcDataBorderItem Class

        private class cModcDataBorderItem
        {

            #region cModcDataBorderItem Constructors

            /// <summary>
            /// Creates an object to track last and next measured data borders and the missing data count for a location or system.
            /// A system is tracked if a MON_SYS_ID is supplied in the constructor.
            /// </summary>
            /// <param name="AParent">The parent cModcDataBorders object.</param>
            /// <param name="AMonLocId">The MON_LOC_ID to track.</param>
            /// <param name="AMonSysId">The MON_SYS_ID to track, null if tracking the location.</param>
            public cModcDataBorderItem(cModcDataBorders AParent, string AMonLocId, string AMonSysId = null)
            {
                FParent = AParent;
                FMonLocId = AMonLocId;
                MonSysId = AMonSysId;
            }

            #endregion


            #region cModcDataBorderItem Public Type

            #endregion


            #region cModcDataBorderItem Private Field

            private cModcDataBorders FParent;

            #endregion


            #region cModcDataBorderItem Public Properties w/ Fields

            private string FMonLocId;
            private int FCurrentQuarter = int.MinValue;
            private int FCurrentYear = int.MinValue;
            private bool FLastFound = false;
            private DateTime FLastBorderDate = DateTime.MinValue;
            private int FLastBorderHour = 0;
            private decimal FLastBorderAdjustedValue = decimal.MinValue;
            private decimal FLastBorderUnadjustedValue = decimal.MinValue;
            private bool FLastIsSuppData = false;
            private int FLastSkipHourCount = 0;
            private int FLastRangeCount = 0;
            private eBorderStatus FNextBorderStatus = eBorderStatus.None;
            private DateTime FNextBorderDate = DateTime.MaxValue;
            private int FNextBorderHour = 0;
            private decimal FNextBorderAdjustedValue = decimal.MinValue;
            private decimal FNextBorderUnadjustedValue = decimal.MinValue;
            private int FNextRangeCount = 0;

            public string MonLocId
            {
                get
                {
                    return FMonLocId;
                }
            }

            /// <summary>
            /// The MON_SYS_ID to track.  Null if tracking for a location.
            /// </summary>
            public string MonSysId { get; private set; }

            public bool[] ModcList
            {
                get
                {
                    return FParent.ModcList;
                }
            }
            public bool BorderModc
            {
                get { return FParent.BorderModc; }
            }

            public int CurrentQuarter
            {
                get { return FCurrentQuarter; }
            }
            public int CurrentYear
            {
                get { return FCurrentYear; }
            }

            public bool LastFound
            {
                get
                {
                    return FLastFound;
                }
            }
            public DateTime LastBorderDate
            {
                get
                {
                    return FLastBorderDate;
                }
            }
            public int LastBorderHour
            {
                get
                {
                    return FLastBorderHour;
                }
            }
            public decimal LastBorderValue { get; private set; }
            public decimal LastBorderAdjustedValue
            {
                get
                {
                    return FLastBorderAdjustedValue;
                }
            }
            public decimal LastBorderUnadjustedValue
            {
                get
                {
                    return FLastBorderUnadjustedValue;
                }
            }
            public bool LastIsSuppData { get { return FLastIsSuppData; } set { FLastIsSuppData = value; } }
            public int LastSkipHourCount
            {
                get
                {
                    return FLastSkipHourCount;
                }
            }
            public int LastRangeCount
            {
                get
                {
                    return FLastRangeCount;
                }
            }
            public eBorderStatus NextBorderStatus
            {
                get
                {
                    return FNextBorderStatus;
                }
            }
            public DateTime NextBorderDate
            {
                get
                {
                    return FNextBorderDate;
                }
            }
            public int NextBorderHour
            {
                get
                {
                    return FNextBorderHour;
                }
            }
            public decimal NextBorderValue { get; private set; }
            public decimal NextBorderAdjustedValue
            {
                get
                {
                    return FNextBorderAdjustedValue;
                }
            }
            public decimal NextBorderUnadjustedValue
            {
                get
                {
                    return FNextBorderUnadjustedValue;
                }
            }
            public int NextRangeCount
            {
                get
                {
                    return FNextRangeCount;
                }
            }

            #endregion


            #region cModcDataBorderItem Public Methods

            /// <summary>
            /// AOpDate and AOpHour represent the actual hour being processed while OpDate and OpHour represent the last successfully filtered hour.
            /// These should be the same unless data does not exist for the current hour.
            /// </summary>
            /// <param name="AOpDate">The current operating date.</param>
            /// <param name="AOpHour">The current operating hour.</param>
            /// <param name="AOperating">Indicates whether the location operated during the current hour.</param>
            /// <param name="AHourlyView">The view containing the hourly data.</param>
            /// <param name="AHourlyViewPosition">The current position in the hourly data.</param>
            public void HandleNewHour(DateTime AOpDate, int AOpHour, bool AOperating, DataView AHourlyView, int AHourlyViewPosition)
            {
                int OpYear = AOpDate.Year;
                int OpQuarter = ((AOpDate.Month - 1) / 3) + 1;

                // Resets the Last and Next data if the quarter of the current op date changes from the last.
                if ((OpYear != CurrentYear) || (OpQuarter != CurrentQuarter))
                {
                    if ((LastIsSuppData == false) && (FLastFound == true))
                    {
                        FLastFound = false;
                        FLastBorderDate = DateTime.MinValue;
                        FLastBorderHour = 0;
                        LastBorderValue = decimal.MinValue;
                        FLastBorderAdjustedValue = decimal.MinValue;
                        FLastBorderUnadjustedValue = decimal.MinValue;
                        FLastSkipHourCount = 0;
                        FLastRangeCount = 0;
                    }

                    FNextBorderStatus = eBorderStatus.None;
                    FNextBorderDate = DateTime.MaxValue;
                    FNextBorderHour = 0;
                    NextBorderValue = decimal.MinValue;
                    FNextBorderAdjustedValue = decimal.MinValue;
                    FNextBorderUnadjustedValue = decimal.MinValue;
                    FNextRangeCount = 0;

                    FCurrentYear = OpYear;
                    FCurrentQuarter = OpQuarter;
                }

                if (AHourlyViewPosition < AHourlyView.Count)
                {
                    DataRowView HourlyRow = AHourlyView[AHourlyViewPosition];
                    string HourlyMonLocId = cDBConvert.ToString(HourlyRow["Mon_Loc_Id"]);
                    string HourlyMonSysId = HourlyRow.Row.Table.Columns.Contains("Mon_Sys_Id")
                                          ? cDBConvert.ToString(HourlyRow["Mon_Sys_Id"])
                                          : null;

                    if ((HourlyMonLocId == FMonLocId) && ((MonSysId == null) || (HourlyMonSysId == MonSysId)))
                    {
                        DateTime OpDate = cDBConvert.ToDate(HourlyRow["Begin_Date"], DateTypes.START);
                        int OpHour = cDBConvert.ToHour(HourlyRow["Begin_Hour"], DateTypes.START);

                        if ((OpDate != FLastBorderDate) || (OpHour != FLastBorderHour))
                        {
                            if ((OpDate < AOpDate) || ((OpDate == AOpDate) && (OpHour <= AOpHour)))
                            {
                                if (AOperating)
                                {
                                    if (((HourlyRow["Modc_Cd"] == DBNull.Value) && BorderModc) ||
                                        ((HourlyRow["Modc_Cd"] != DBNull.Value) &&
                                         (ModcList[cDBConvert.ToInteger(HourlyRow["Modc_Cd"])] == BorderModc)))
                                    {
                                        FLastFound = true;
                                        FLastBorderDate = OpDate;
                                        FLastBorderHour = OpHour;
                                        LastBorderValue = HourlyRow[FParent.ValueColumnName].AsDecimal().Default();
                                        FLastBorderAdjustedValue = cDBConvert.ToDecimal(HourlyRow["Adjusted_Hrly_Value"]);
                                        FLastBorderUnadjustedValue = cDBConvert.ToDecimal(HourlyRow["Unadjusted_Hrly_Value"]);
                                        FLastRangeCount = 0;
                                    }
                                    else
                                    {
                                        FLastRangeCount += 1;
                                        FNextRangeCount -= 1;
                                    }
                                }
                            }
                        }

                        if ((FNextBorderStatus == eBorderStatus.None) ||
                            ((FNextBorderStatus == eBorderStatus.Found) &&
                             ((OpDate > FNextBorderDate) || ((OpDate == FNextBorderDate) && (OpHour >= FNextBorderHour)))))
                        {
                            FNextBorderStatus = eBorderStatus.Missing;

                            for (int RowPosition = AHourlyViewPosition + 1; RowPosition < AHourlyView.Count; RowPosition++)
                            {
                                HourlyRow = AHourlyView[RowPosition];
                                HourlyMonLocId = cDBConvert.ToString(HourlyRow["Mon_Loc_Id"]);
                                HourlyMonSysId = HourlyRow.Row.Table.Columns.Contains("Mon_Sys_Id")
                                          ? cDBConvert.ToString(HourlyRow["Mon_Sys_Id"])
                                          : null;

                                if ((HourlyMonLocId == FMonLocId) && ((MonSysId == null) || (HourlyMonSysId == MonSysId)))
                                {
                                    OpDate = cDBConvert.ToDate(HourlyRow["Begin_Date"], DateTypes.START);
                                    OpHour = cDBConvert.ToHour(HourlyRow["Begin_Hour"], DateTypes.START);

                                    if ((OpDate.Year != FCurrentYear) || ((((OpDate.Month - 1) / 3) + 1) != FCurrentQuarter))
                                    {
                                        break;
                                    }
                                    else if (((HourlyRow["Modc_Cd"] == DBNull.Value) && BorderModc) ||
                                             ((HourlyRow["Modc_Cd"] != DBNull.Value) &&
                                              (ModcList[cDBConvert.ToInteger(HourlyRow["Modc_Cd"])] == BorderModc)))
                                    {
                                        FNextBorderStatus = eBorderStatus.Found;
                                        FNextBorderDate = OpDate;
                                        FNextBorderHour = OpHour;
                                        NextBorderValue = HourlyRow[FParent.ValueColumnName].AsDecimal().Default();
                                        FNextBorderAdjustedValue = cDBConvert.ToDecimal(HourlyRow["Adjusted_Hrly_Value"]);
                                        FNextBorderUnadjustedValue = cDBConvert.ToDecimal(HourlyRow["Unadjusted_Hrly_Value"]);
                                        break;
                                    }
                                    else
                                    {
                                        FNextRangeCount += 1;
                                    }
                                }
                            }
                        }
                    }
                }
            }


            /// <summary>
            /// Updates the item for supplemental data if the quarter of the supplemental data preceeds the current quarter and LastFound is false.
            /// </summary>
            /// <param name="lastOpDateHour">The hour of the last op hour from supplemental data.</param>
            /// <param name="lastUnadjustedValue">The last unadjusted value from supplemental data.</param>
            /// <param name="lastAdjustedValue">The last adjusted value from supplemental data.</param>
            /// <param name="lastValue">The last adjusted or unadjusted value from supplemental data, usually used when parameter can be derived or monitored.</param>
            /// <param name="currentYear">The year of the emission report quarter.</param>
            /// <param name="currentQuarter">The quarter of the emission report quarter.</param>
            public void HandleSuppDataHour(DateTime lastOpDateHour, decimal? lastUnadjustedValue, decimal? lastAdjustedValue, decimal? lastValue, int currentYear, int currentQuarter)
            {
                if (!FLastFound && ((lastOpDateHour.Year < currentYear) || ((lastOpDateHour.Year == currentYear) && (lastOpDateHour.Quarter() < currentQuarter))))
                {
                    FLastBorderDate = lastOpDateHour.Date;
                    FLastBorderHour = lastOpDateHour.Hour;
                    LastBorderValue = cDBConvert.ToDecimal(lastValue);
                    FLastBorderAdjustedValue = cDBConvert.ToDecimal(lastAdjustedValue);
                    FLastBorderUnadjustedValue = cDBConvert.ToDecimal(lastUnadjustedValue);
                    FLastRangeCount = int.MinValue;
                    FLastIsSuppData = true;
                    FLastFound = true;
                }
            }

            #endregion


            #region cModcDataBorderItem Public Methods: For Programmer Testing

            public void TestCaseSet(bool ALastFound, eBorderStatus ANextBorderStatus, int ALastSkipHourCount,
                                    DateTime ALastBorderDate, int ALastBorderHour, decimal ALastBorderAdjustedValue, decimal ALastBorderUnadjustedValue,
                                    DateTime ANextBorderDate, int ANextBorderHour, decimal ANextBorderAdjustedValue, decimal ANextBorderUnadjustedValue)
            {
                FLastFound = ALastFound;
                FNextBorderStatus = ANextBorderStatus;
                FLastSkipHourCount = ALastSkipHourCount;
                FLastBorderDate = ALastBorderDate;
                FLastBorderHour = ALastBorderHour;
                FLastBorderAdjustedValue = ALastBorderAdjustedValue;
                FLastBorderUnadjustedValue = ALastBorderUnadjustedValue;
                FNextBorderDate = ANextBorderDate;
                FNextBorderHour = ANextBorderHour;
                FNextBorderAdjustedValue = ANextBorderAdjustedValue;
                FNextBorderUnadjustedValue = ANextBorderUnadjustedValue;
            }

            #endregion

        }

        #endregion


        #region Private Fields

        private cModcDataBorderItem[] ModcDataBorderLocations;
        private Dictionary<string, cModcDataBorderItem>[] ModcDataBorderLocationSystems;
        private DataTable FHourlyTable;
        private bool FSkipNewHour = false;

        #endregion


        #region Public Properties w/ Fields

        private bool FBorderModc;
        private bool[] FModcList = new bool[56];

        /// <summary>
        /// Border MODC
        /// </summary>
        public bool BorderModc
        {
            get { return FBorderModc; }
        }

        /// <summary>
        /// MODC List
        /// </summary>
        public bool[] ModcList
        {
            get { return FModcList; }
        }

        /// <summary>
        /// The name of the value column to use, usually ADJUSTED_HRLY_VALUE and UNDADJUSTED_HRLY_VALUE.
        /// </summary>
        public string ValueColumnName { get; private set; }

        #endregion


        #region Public Methods

        /// <summary>
        /// Handles the update for the location and a new hour.  If systems are being tracked, performs the update for each system being tracked for the location.
        /// </summary>
        /// <param name="ALocationPos">The position of the location used througout emission evaluations.</param>
        /// <param name="AOpDate">The current operating date.</param>
        /// <param name="AOpHour">The current operating hour.</param>
        /// <param name="AOperating">Whether the location is operating for the current operating hour</param>
        /// <param name="AHourlyViewPosition">The current position in the hourly data.</param>
        public void HandleNewHour(int ALocationPos, DateTime AOpDate, int AOpHour, bool AOperating, int AHourlyViewPosition)
        {
            if (!FSkipNewHour)
            {
                ModcDataBorderLocations[ALocationPos].HandleNewHour(AOpDate, AOpHour, AOperating, FHourlyTable.DefaultView, AHourlyViewPosition);

                if (ModcDataBorderLocationSystems != null)
                {
                    foreach (cModcDataBorderItem systemModcDataBorders in ModcDataBorderLocationSystems[ALocationPos].Values)
                        systemModcDataBorders.HandleNewHour(AOpDate, AOpHour, AOperating, FHourlyTable.DefaultView, AHourlyViewPosition);
                }
            }
        }


        /// <summary>
        /// AverageLastAndNextAdjusted
        /// </summary>
        /// <param name="locationPos"></param>
        /// <param name="decimalPlaces"></param>
        /// <param name="value"></param>
        /// <param name="monSysId"></param>
        /// <returns></returns>
        public bool AverageLastAndNext(int locationPos, int decimalPlaces, out decimal value, string monSysId = null)
        {
            bool result;

            if (monSysId == null)
            {
                result = AverageLastAndNextDo(ModcDataBorderLocations[locationPos], locationPos, decimalPlaces, out value);
            }
            else if ((ModcDataBorderLocationSystems != null) && ModcDataBorderLocationSystems[locationPos].ContainsKey(monSysId))
            {
                result = AverageLastAndNextDo(ModcDataBorderLocationSystems[locationPos][monSysId], locationPos, decimalPlaces, out value);
            }
            else
            {
                value = Decimal.MinValue;
                result = false;
            }

            return result;
        }

        /// <summary>
        /// AverageLastAndNextAdjusted
        /// </summary>
        /// <param name="modcDataBorderItem"></param>
        /// <param name="locationPos"></param>
        /// <param name="decimalPlaces"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool AverageLastAndNextDo(cModcDataBorderItem modcDataBorderItem, int locationPos, int decimalPlaces, out decimal value)
        {
            bool result;
            
            if ((modcDataBorderItem.LastFound) &&
                (modcDataBorderItem.NextBorderStatus == eBorderStatus.Found))
            {
                if ((modcDataBorderItem.LastBorderValue >= 0) &&
                    (modcDataBorderItem.NextBorderValue >= 0))
                {
                    value = (modcDataBorderItem.LastBorderValue + modcDataBorderItem.NextBorderValue) / 2;
                    value = Math.Round(value, decimalPlaces, MidpointRounding.AwayFromZero);
                    result = true;
                }
                else
                {
                    value = decimal.MinValue;
                    result = true;
                }
            }
            else
            {
                value = decimal.MinValue;
                result = false;
            }

            return result;
        }


        /// <summary>
        /// AverageLastAndNextAdjusted
        /// </summary>
        /// <param name="locationPos"></param>
        /// <param name="decimalPlaces"></param>
        /// <param name="value"></param>
        /// <param name="monSysId"></param>
        /// <returns></returns>
        public bool AverageLastAndNextAdjusted(int locationPos, int decimalPlaces, out decimal value, string monSysId = null)
        {
            bool result;

            if (monSysId == null)
            {
                result = AverageLastAndNextAdjustedDo(ModcDataBorderLocations[locationPos], locationPos, decimalPlaces, out value);
            }
            else if ((ModcDataBorderLocationSystems != null) && ModcDataBorderLocationSystems[locationPos].ContainsKey(monSysId))
            {
                result = AverageLastAndNextAdjustedDo(ModcDataBorderLocationSystems[locationPos][monSysId], locationPos, decimalPlaces, out value);
            }
            else
            {
                value = Decimal.MinValue;
                result = false;
            }

            return result;
        }

        /// <summary>
        /// AverageLastAndNextAdjusted
        /// </summary>
        /// <param name="modcDataBorderItem"></param>
        /// <param name="locationPos"></param>
        /// <param name="decimalPlaces"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool AverageLastAndNextAdjustedDo(cModcDataBorderItem modcDataBorderItem, int locationPos, int decimalPlaces, out decimal value)
        {
            bool result;

            if ((modcDataBorderItem.LastFound) &&
                (modcDataBorderItem.NextBorderStatus == eBorderStatus.Found))
            {
                if ((modcDataBorderItem.LastBorderAdjustedValue >= 0) &&
                    (modcDataBorderItem.NextBorderAdjustedValue >= 0))
                {
                    value = (modcDataBorderItem.LastBorderAdjustedValue + modcDataBorderItem.NextBorderAdjustedValue) / 2;
                    value = Math.Round(value, decimalPlaces, MidpointRounding.AwayFromZero);
                    result = true;
                }
                else
                {
                    value = decimal.MinValue;
                    result = true;
                }
            }
            else
            {
                value = decimal.MinValue;
                result = false;
            }

            return result;
        }


        /// <summary>
        /// AverageLastAndNextUnadjusted
        /// </summary>
        /// <param name="locationPos"></param>
        /// <param name="decimalPlaces"></param>
        /// <param name="value"></param>
        /// <param name="monSysId"></param>
        /// <returns></returns>
        public bool AverageLastAndNextUnadjusted(int locationPos, int decimalPlaces, out decimal value, string monSysId = null)
        {
            bool result;

            if (monSysId == null)
            {
                result = AverageLastAndNextUnadjustedDo(ModcDataBorderLocations[locationPos], locationPos, decimalPlaces, out value);
            }
            else if ((ModcDataBorderLocationSystems != null) && ModcDataBorderLocationSystems[locationPos].ContainsKey(monSysId))
            {
                result = AverageLastAndNextUnadjustedDo(ModcDataBorderLocationSystems[locationPos][monSysId], locationPos, decimalPlaces, out value);
            }
            else
            {
                value = Decimal.MinValue;
                result = false;
            }

            return result;
        }

        /// <summary>
        /// AverageLastAndNextUnadjusted
        /// </summary>
        /// <param name="modcDataBorderItem"></param>
        /// <param name="locationPos"></param>
        /// <param name="decimalPlaces"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool AverageLastAndNextUnadjustedDo(cModcDataBorderItem modcDataBorderItem, int locationPos, int decimalPlaces, out decimal value)
        {
            bool Result;

            if ((modcDataBorderItem.LastFound) &&
                (modcDataBorderItem.NextBorderStatus == eBorderStatus.Found))
            {
                if ((modcDataBorderItem.LastBorderUnadjustedValue >= 0) &&
                    (modcDataBorderItem.NextBorderUnadjustedValue >= 0))
                {
                    value = (modcDataBorderItem.LastBorderUnadjustedValue + modcDataBorderItem.NextBorderUnadjustedValue) / 2;
                    value = Math.Round(value, 1, MidpointRounding.AwayFromZero);
                    Result = true;
                }
                else
                {
                    value = decimal.MinValue;
                    Result = true;
                }
            }
            else
            {
                value = decimal.MinValue;
                Result = false;
            }

            return Result;
        }


        /// <summary>
        /// MissingCount
        /// </summary>
        /// <param name="ALocationPos"></param>
        /// <returns></returns>
        public int MissingCount(int ALocationPos)
        {
            if (ModcDataBorderLocations[ALocationPos].LastFound && !ModcDataBorderLocations[ALocationPos].LastIsSuppData)
                return ModcDataBorderLocations[ALocationPos].LastRangeCount + ModcDataBorderLocations[ALocationPos].NextRangeCount;
            else
                return int.MinValue;
        }

        #endregion


        #region Programmer Testing Constructor and Public Methods

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ALastFound"></param>
        /// <param name="ANextBorderStatus"></param>
        /// <param name="ALastSkipHourCount"></param>
        /// <param name="ALastBorderDate"></param>
        /// <param name="ALastBorderHour"></param>
        /// <param name="ALastBorderAdjustedValue"></param>
        /// <param name="ALastBorderUnadjustedValue"></param>
        /// <param name="ANextBorderDate"></param>
        /// <param name="ANextBorderHour"></param>
        /// <param name="ANextBorderAdjustedValue"></param>
        /// <param name="ANextBorderUnadjustedValue"></param>
        /// <param name="AEvaluationPeriodBeganDate"></param>
        /// <param name="AEvaluationPeriodEndedDate"></param>
        public cModcDataBorders(bool ALastFound, eBorderStatus ANextBorderStatus, int ALastSkipHourCount,
                                DateTime ALastBorderDate, int ALastBorderHour, decimal ALastBorderAdjustedValue, decimal ALastBorderUnadjustedValue,
                                DateTime ANextBorderDate, int ANextBorderHour, decimal ANextBorderAdjustedValue, decimal ANextBorderUnadjustedValue,
                                DateTime AEvaluationPeriodBeganDate, DateTime AEvaluationPeriodEndedDate)
        {
            ModcDataBorderLocations = new cModcDataBorderItem[1];
            ModcDataBorderLocations[0] = new cModcDataBorderItem(this, "Mon_Loc_Id");
            ModcDataBorderLocations[0].TestCaseSet(ALastFound, ANextBorderStatus, ALastSkipHourCount,
                                      ALastBorderDate, ALastBorderHour, ALastBorderAdjustedValue, ALastBorderUnadjustedValue,
                                      ANextBorderDate, ANextBorderHour, ANextBorderAdjustedValue, ANextBorderUnadjustedValue);

            FSkipNewHour = true;
        }

        /// <summary>
        /// GetTestCaseResults
        /// </summary>
        /// <param name="ALastFound"></param>
        /// <param name="ANextBorderStatus"></param>
        /// <param name="ALastSkipHourCount"></param>
        /// <param name="ALastBorderDate"></param>
        /// <param name="ALastBorderHour"></param>
        /// <param name="ALastBorderAdjustedValue"></param>
        /// <param name="ALastBorderUnadjustedValue"></param>
        /// <param name="ANextBorderDate"></param>
        /// <param name="ANextBorderHour"></param>
        /// <param name="ANextBorderAdjustedValue"></param>
        /// <param name="ANextBorderUnadjustedValue"></param>
        /// <param name="AMeasuredAverageValue"></param>
        /// <param name="AMeasuredAverageDone"></param>
        /// <param name="AMissingCount"></param>
        /// <returns></returns>
        public bool GetTestCaseResults(out bool ALastFound, out eBorderStatus ANextBorderStatus, out int ALastSkipHourCount,
                                       out DateTime ALastBorderDate, out int ALastBorderHour, out decimal ALastBorderAdjustedValue, out decimal ALastBorderUnadjustedValue,
                                       out DateTime ANextBorderDate, out int ANextBorderHour, out decimal ANextBorderAdjustedValue, out decimal ANextBorderUnadjustedValue,
                                       out decimal AMeasuredAverageValue, out bool AMeasuredAverageDone,
                                       out decimal AMissingCount)
        {
            if ((ModcDataBorderLocations != null) && (ModcDataBorderLocations.Length > 0) && (ModcDataBorderLocations[0] != null))
            {
                ALastFound = ModcDataBorderLocations[0].LastFound;
                ANextBorderStatus = ModcDataBorderLocations[0].NextBorderStatus;
                ALastSkipHourCount = ModcDataBorderLocations[0].LastSkipHourCount;
                ALastBorderDate = ModcDataBorderLocations[0].LastBorderDate;
                ALastBorderHour = ModcDataBorderLocations[0].LastBorderHour;
                ALastBorderAdjustedValue = ModcDataBorderLocations[0].LastBorderAdjustedValue;
                ALastBorderUnadjustedValue = ModcDataBorderLocations[0].LastBorderUnadjustedValue;
                ANextBorderDate = ModcDataBorderLocations[0].NextBorderDate;
                ANextBorderHour = ModcDataBorderLocations[0].NextBorderHour;
                ANextBorderAdjustedValue = ModcDataBorderLocations[0].NextBorderAdjustedValue;
                ANextBorderUnadjustedValue = ModcDataBorderLocations[0].NextBorderUnadjustedValue;

                AMeasuredAverageDone = AverageLastAndNextAdjusted(0, 3, out AMeasuredAverageValue);
                AMissingCount = MissingCount(0);

                return true;
            }
            else
            {
                ALastFound = false;
                ANextBorderStatus = eBorderStatus.None;
                ALastSkipHourCount = int.MinValue;
                ALastBorderDate = DateTime.MinValue;
                ALastBorderHour = int.MinValue;
                ALastBorderAdjustedValue = decimal.MinValue;
                ALastBorderUnadjustedValue = decimal.MinValue;
                ANextBorderDate = DateTime.MinValue;
                ANextBorderHour = int.MinValue;
                ANextBorderAdjustedValue = decimal.MinValue;
                ANextBorderUnadjustedValue = decimal.MinValue;

                AMeasuredAverageValue = decimal.MinValue;
                AMeasuredAverageDone = false;
                AMissingCount = int.MinValue;

                return false;
            }
        }

        #endregion

    }
}
