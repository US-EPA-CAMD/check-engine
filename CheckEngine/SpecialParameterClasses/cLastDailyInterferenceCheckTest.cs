using System;
using System.Data;

using ECMPS.Checks.TypeUtilities;
using ECMPS.Definitions.Extensions;


namespace ECMPS.Checks.CheckEngine.SpecialParameterClasses
{
    /// <summary>
    /// Class represents daily interference check test data and its supplemental data and update methods.
    /// </summary>
    public class cLastDailyInterferenceCheckTest
    {

        #region Public Constructor

        /// <summary>
        /// Initializes the supplemental data values and additional information.
        /// </summary>
        /// <param name="dailyInterferenceCheckRow">The Daily Interference Check or Daily Interference Supp Data Row to load.</param>
        /// <param name="calculatedTestResultCd">The check engine calculated test result.</param>
        /// <param name="fromPreviousQuarter">Indicates whether the row is from a previous quarter.  Defaults to false.  Prevents saving the row as supplemental data and determines whether the row should contain supplemental data information.</param>
        public cLastDailyInterferenceCheckTest(DataRowView dailyInterferenceCheckRow, string calculatedTestResultCd, bool fromPreviousQuarter)
        {
            string resultList;

            if (dailyInterferenceCheckRow == null)
            {
                throw new ArgumentNullException("dailyInterferenceCheckRow");
            }
            else if (!IsDailyTestRow(dailyInterferenceCheckRow, out resultList))
            {
                throw new ArgumentException("The row does not contain a Daily Interference Check or Dialy Interference Supp Data row.", "dailyInterferenceCheckRow");
            }
            else if (!fromPreviousQuarter && ContainsSuppDataColumns(dailyInterferenceCheckRow, out resultList))
            {
                throw new ArgumentException("Expected non-supplemental data row but the row contains supplemental data columns.", "dailyInterferenceCheckRow");
            }
            else if (fromPreviousQuarter && !IsSuppDataRow(dailyInterferenceCheckRow, out resultList))
            {
                throw new ArgumentException("Expected supplemental data row but the row does not contain the supplemental data columns.", "dailyInterferenceCheckRow");
            }
            else if (!HasExpectedTestType(dailyInterferenceCheckRow))
            {
                throw new ArgumentException($"The test type is '{dailyInterferenceCheckRow["TEST_TYPE_CD"].AsString()}' instead of the expected '{ExpectedTestTypeCd}' for interference checks.", "dailyInterferenceCheckRow");
            }
            else if (!HasRequiredValues(dailyInterferenceCheckRow, out resultList))
            {
                throw new ArgumentException($"Required values are missing for {resultList}.", "dailyInterferenceCheckRow");
            }

            FromPreviousQuarter = fromPreviousQuarter;

            DailyInterferenceCheckRow = dailyInterferenceCheckRow;
            {
                ComponentId = DailyInterferenceCheckRow["COMPONENT_ID"].AsString();
                ComponentIdentifier = DailyInterferenceCheckRow["COMPONENT_IDENTIFIER"].AsString();
                DailyTestDate = DailyInterferenceCheckRow["DAILY_TEST_DATE"].AsDateTime(DateTime.MinValue);
                DailyTestHour = DailyInterferenceCheckRow["DAILY_TEST_HOUR"].AsInteger(0);
                DailyTestMinute = DailyInterferenceCheckRow["DAILY_TEST_MIN"].AsInteger(0);
                DailyTestSumId = DailyInterferenceCheckRow["DAILY_TEST_SUM_ID"].AsString();
                MonLocId = DailyInterferenceCheckRow["MON_LOC_ID"].AsString();

                CalculatedTestResultCd = !fromPreviousQuarter ? calculatedTestResultCd : DailyInterferenceCheckRow["CALC_TEST_RESULT_CD"].AsString();

                if (FromPreviousQuarter)
                {
                    FirstOpHourAfterLastCoveredNonOpHour = DailyInterferenceCheckRow["FIRST_OP_AFTER_NONOP_DATEHOUR"].AsDateTime();
                    LastCoveredNonOpHour = DailyInterferenceCheckRow["LAST_COVERED_NONOP_DATEHOUR"].AsDateTime();
                    LastHandledOpHour = null; // Value is not necessary since last hour was in emissions report for previous quarter;
                    OperatingHourCount = DailyInterferenceCheckRow["OP_HOUR_CNT"].AsInteger(0);
                }
                else
                {
                    FirstOpHourAfterLastCoveredNonOpHour = null;
                    LastCoveredNonOpHour = null;
                    LastHandledOpHour = null;
                    OperatingHourCount = 0;
                }
            }
        }

        #endregion


        #region Public Static Methods: General

        /// <summary>
        /// Creates Daily Calibration Test Data object from a Daily Test (Location) Supplemental Data row and a list
        /// of Daily Test System Supplemental Data rows.
        /// </summary>
        /// <param name="dailyInterferenceCheckRow">The Daily Interference Check or Supplemental Data row</param>
        /// <param name="calculatedTestResultCd">The check engine calculated test result.</param>
        /// <param name="fromPreviousQuarter">Indicates whether the row is from a previous quarter.  Defaults to false.  Prevents saving the row as supplemental data and determines whether the row should contain supplemental data information.</param>
        /// <param name="instance">The created instacne, null if failed</param>
        /// <param name="resultMessage">Message indicating why the creation failed, null if successful.</param>
        /// <returns>Returns true if the creation was successful.</returns>
        public static bool CreateInstance(DataRowView dailyInterferenceCheckRow, string calculatedTestResultCd, bool fromPreviousQuarter, 
                                   out cLastDailyInterferenceCheckTest instance, 
                                   out string resultMessage)
        {
            bool result;

            try
            {
                instance = new cLastDailyInterferenceCheckTest(dailyInterferenceCheckRow, calculatedTestResultCd, fromPreviousQuarter);
                resultMessage = null;
                result = true;
            }
            catch (Exception ex)
            {
                instance = null;
                resultMessage = $"cLastDailyInterferenceCheckTest.CreateInstance() {ex.Message}";
                result = false;
            }

            return result;
        }

        #endregion


        #region Public Static Methods: Row Check

        private static string ExpectedTestTypeCd = "INTCHK";
        private static string[] SuppDataColumns = { "CALC_TEST_RESULT_CD", "FIRST_OP_AFTER_NONOP_DATEHOUR", "KEY_ONLINE_IND", "LAST_COVERED_NONOP_DATEHOUR", "OP_HOUR_CNT" };
        private static string[] RequiredValueColumns = { "COMPONENT_ID", "COMPONENT_IDENTIFIER", "DAILY_TEST_DATE", "DAILY_TEST_DATEHOUR", "DAILY_TEST_HOUR", "DAILY_TEST_MIN",
                                                         "DAILY_TEST_SUM_ID", "MON_LOC_ID", "TEST_RESULT_CD", "TEST_TYPE_CD" };


        /// <summary>
        /// Returns true if the row is a Supplemental Data row.
        /// </summary>
        /// <param name="suppDataRow">The row to test.</param>
        /// <param name="containedColumns">Contains a comma delimited list of columns that are contained in the column collection.</param>
        /// <returns>Returns true if the row is Daily Interference Check row.</returns>
        public static bool ContainsSuppDataColumns(DataRowView suppDataRow, out string containedColumns)
        {
            bool result;

            result = ContainsSomeColumns(suppDataRow, SuppDataColumns, out containedColumns);

            return result;
        }


        /// <summary>
        /// Ensures that the Daily Test Date/Time columns are not null.
        /// </summary>
        /// <param name="dailyInterferenceCheckRow"></param>
        /// <param name="missingValues"></param>
        /// <returns></returns>
        public static bool HasRequiredValues(DataRowView dailyInterferenceCheckRow, out string missingValues)
        {
            bool result;

            missingValues = "";

            if ((dailyInterferenceCheckRow != null) &&
                (dailyInterferenceCheckRow.Row != null) &&
                (dailyInterferenceCheckRow.Row.Table != null))
            {
                DataColumnCollection columns = dailyInterferenceCheckRow.Row.Table.Columns;

                result = true;

                foreach (string columnName in RequiredValueColumns)
                    if (columns.Contains(columnName) && (dailyInterferenceCheckRow[columnName] == DBNull.Value))
                    {
                        missingValues += $"{columnName}, ";
                        result = false;
                    }

                if (!result)
                {
                    missingValues = missingValues.Substring(0, missingValues.Length - 2); // Strip trailing comma and space.

                    if (missingValues.Contains(","))
                    {
                        int lastCommaPos = missingValues.LastIndexOf(',');
                        if (lastCommaPos != -1) missingValues = missingValues.Remove(lastCommaPos, 1).Insert(lastCommaPos, " and");
                    }
                }
            }
            else
            {
                missingValues = "All";
                result = false;
            }

            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dailyInterferenceCheckRow"></param>
        /// <returns></returns>
        public static bool HasExpectedTestType(DataRowView dailyInterferenceCheckRow)
        {
            bool result;

            result = (dailyInterferenceCheckRow != null) &&
                     (dailyInterferenceCheckRow.Row != null) &&
                     (dailyInterferenceCheckRow.Row.Table != null) &&
                     dailyInterferenceCheckRow.Row.Table.Columns.Contains("TEST_TYPE_CD") &&
                     (dailyInterferenceCheckRow["TEST_TYPE_CD"].AsString() == ExpectedTestTypeCd);

            return result;
        }


        /// <summary>
        /// Returns true if the row is a Daily Test row.
        /// </summary>
        /// <param name="dailyTestRow">The row to test.</param>
        /// <param name="missingColumns">Contains a comma delimited list of columns that are not contained in the table.</param>
        /// <returns>Returns true if the row is Daily Interference Check row.</returns>
        public static bool IsDailyTestRow(DataRowView dailyTestRow, out string missingColumns)
        {
            bool result;

            result = ContainsAllColumns(dailyTestRow, RequiredValueColumns, out missingColumns);

            return result;
        }


        /// <summary>
        /// Returns true if the row is a Daily Test row.
        /// </summary>
        /// <param name="dailyTestTable">The row to test.</param>
        /// <param name="missingColumns">Contains a comma delimited list of columns that are not contained in the table.</param>
        /// <returns>Returns true if the row is Daily Interference Check row.</returns>
        public static bool IsDailyTestTable(DataTable dailyTestTable, out string missingColumns)
        {
            bool result;

            result = ContainsAllColumns(dailyTestTable, RequiredValueColumns, out missingColumns);

            return result;
        }


        /// <summary>
        /// Returns true if the row is a Supplemental Data row.
        /// </summary>
        /// <param name="suppDataRow">The row to test.</param>
        /// <param name="missingColumns">Contains a comma delimited list of columns that are not contained in the table.</param>
        /// <returns>Returns true if the row is Daily Interference Check row.</returns>
        public static bool IsSuppDataRow(DataRowView suppDataRow, out string missingColumns)
        {
            bool result;

            result = ContainsAllColumns(suppDataRow, SuppDataColumns, out missingColumns);

            return result;
        }


        #region Helper Methods

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
        /// Determines whether the column collection for a data row view contains a list of columns.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="columnNames">The list of columns the table must contain.  The table can contain additional columns.</param>
        /// <param name="missingColumns">Contains a comma delimited list of columns that are not contained in the table.</param>
        /// <returns> Returns true if the column collection is not null and the columns in the array are all columns in the table.  Otherwise returns false.</returns>
        private static bool ContainsAllColumns(DataTable table, string[] columnNames, out string missingColumns)
        {
            bool result;

            if (table != null)
            {
                result = ContainsAllColumns(table.Columns, columnNames, out missingColumns);
            }
            else
            {
                missingColumns = "All";
                result = false;
            }

            return result;
        }


        /// <summary>
        /// Returns true if the row is a Supplemental Data row.
        /// </summary>
        /// <param name="columnCollection">The columns to check.</param>
        /// <param name="columnNames">The list of columns the table must contain.  The table can contain additional columns.</param>
        /// <param name="containedColumns">Contains a comma delimited list of columns that are contained in the column collection.</param>
        /// <returns>Returns true if the column collection contains one of the columns in columnNames.</returns>
        public static bool ContainsSomeColumns(DataColumnCollection columnCollection, string[] columnNames, out string containedColumns)
        {
            bool result;

            if (columnCollection != null)
            {
                result = false;

                containedColumns = "";

                foreach (string columnName in columnNames)
                {
                    if (columnCollection.Contains(columnName))
                    {
                        containedColumns.ListAdd(columnName);
                        result = true;
                    }
                }

                containedColumns = result ? ReadFormatCommaDelimitedList(containedColumns) : null;
            }
            else
            {
                containedColumns = null;
                result = false;
            }

            return result;
        }


        /// <summary>
        /// Determines whether the column collection for a data row view contains a list of columns.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="columnNames">The list of columns the table must contain.  The table can contain additional columns.</param>
        /// <param name="containedColumns">Contains a comma delimited list of columns that are contained in the column collection.</param>
        /// <returns> Returns true if the column collection is not null and the columns in the array are all columns in the table.  Otherwise returns false.</returns>
        private static bool ContainsSomeColumns(DataRowView row, string[] columnNames, out string containedColumns)
        {
            bool result;

            if ((row != null) &&
                (row.Row != null) &&
                (row.Row.Table != null))
            {
                result = ContainsAllColumns(row.Row.Table.Columns, columnNames, out containedColumns);
            }
            else
            {
                containedColumns = null;
                result = false;
            }

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


        #region Public Properties: Daily Interference Check Information

        /// <summary>
        /// The Calculated Test Result Code for this set of daily calibration data.
        /// </summary>
        public string CalculatedTestResultCd { get; private set; }

        /// <summary>
        /// The Component Id for this set of daily calibration data.
        /// </summary>
        public string ComponentId { get; private set; }

        /// <summary>
        /// The Component Identifier for this set of daily calibration data.
        /// </summary>
        public string ComponentIdentifier { get; private set; }

        /// <summary>
        /// The Daily Test Date-Hour for this set of daily calibration data.
        /// </summary>
        public DateTime DailyTestDateHour { get { return DailyTestDate.AddHours(DailyTestHour); } }

        /// <summary>
        /// The Daily Test Date for this set of daily calibration data.
        /// </summary>
        public DateTime DailyTestDate { get; private set; }

        /// <summary>
        /// The Daily Test Hour for this set of daily calibration data.
        /// </summary>
        public int DailyTestHour { get; private set; }

        /// <summary>
        /// The Daily Test Minute for this set of daily calibration data.
        /// </summary>
        public int DailyTestMinute { get; private set; }

        /// <summary>
        /// The latest daily interference check based on the current hour.
        /// </summary>
        public DataRowView DailyInterferenceCheckRow { get; private set; }

        /// <summary>
        /// The DAILY_TEST_SUM_ID of the daily interference test.
        /// </summary>
        public string DailyTestSumId { get; private set; }

        /// <summary>
        /// Indicates whether this data is from a previous quarter.
        /// </summary>
        public bool FromPreviousQuarter { get; private set; }

        /// <summary>
        /// Indicates whether the test was done online or offline.
        /// </summary>
        public bool IsOnlineTest { get { return CalculatedTestResultCd != "IGNORED"; } }

        /// <summary>
        /// The Monitor Location Id for this set of daily calibration data.
        /// </summary>
        public string MonLocId { get; private set; }

        #endregion


        #region Public Properties: Operating Hour Information

        /// <summary>
        /// Contains the firest operating hour that occurred after LastCoveredNonOpHour.
        /// 
        /// The value is reset to null whenever LastCoveredNonOpHour is updated.
        /// 
        /// Used in supplemental data.
        /// </summary>
        public DateTime? FirstOpHourAfterLastCoveredNonOpHour { get; private set; }

        /// <summary>
        /// Contains the last non operating hour that occurred during the hours coverred by the test.
        /// 
        /// Used in supplemental data.
        /// </summary>
        public DateTime? LastCoveredNonOpHour { get; private set; }

        /// <summary>
        /// Indicates the last hour for which potential updates to OperatingHourCount, LastCoveredNonOpHour and FirstOpHourAfterLastCoveredNonOpHour
        /// were handled.
        /// </summary>
        public DateTime? LastHandledOpHour { get; private set; }

        /// <summary>
        /// The count of operating hours on or after the the test hour.  
        /// 
        /// Used in supplemental data and should be the count from the test to the end of the quarter for any data used in supplemantal data.
        /// </summary>
        public int OperatingHourCount { get; private set; }

        #endregion


        #region Public Methods


        /// <summary>
        /// Adds a row to SupplementalDataUpdateDataTable populated with the values suppDataCd and fromsupplementalDataGroup.
        /// </summary>
        /// <param name="SupplementalDataUpdateTable"></param>
        /// <param name="rptPeriodId"></param>
        /// <param name="workspaceSessionId"></param>
        public void LoadIntoSupplementalDataTables(DataTable SupplementalDataUpdateTable, int rptPeriodId, decimal workspaceSessionId)
        {
            // Only store values for tests that are not incomplete or ignored (Valid is null), and are online tests
            if (!FromPreviousQuarter)
            {
                DataRow dataRow;

                dataRow = SupplementalDataUpdateTable.NewRow();

                dataRow["SESSION_ID"] = workspaceSessionId;
                dataRow["DAILY_TEST_SUM_ID"] = DailyTestSumId;
                dataRow["RPT_PERIOD_ID"] = rptPeriodId;
                dataRow["KEY_ONLINE_IND"] = (IsOnlineTest ? 1 : 0);
                dataRow["KEY_VALID_IND"] = 1;
                dataRow["OP_HOUR_CNT"] = OperatingHourCount;
                dataRow["LAST_COVERED_NONOP_DATEHOUR"] = LastCoveredNonOpHour.DbValue();
                dataRow["FIRST_OP_AFTER_NONOP_DATEHOUR"] = FirstOpHourAfterLastCoveredNonOpHour.DbValue();
                dataRow["SORT_DAILY_TEST_DATEHOURMIN"] = DailyTestDate.AddHours(DailyTestHour).AddMinutes(DailyTestMinute);
                dataRow["CALC_TEST_RESULT_CD"] = CalculatedTestResultCd;

                SupplementalDataUpdateTable.Rows.Add(dataRow);
            }
        }


        /// <summary>
        /// Updates OperatingHourCount, LastCoveredNonOpHour and FirstOpHourAfterLastCoveredNonOpHour for an hour.
        /// 
        ///     OperatingHourCount: Updated when the op hour is a new hour and and an operating hour.
        ///     LastCoveredNonOpHour: Update to the op hour when the hour falls within the 26 hours covered by the test starting with the test hour.
        ///     FirstOpHourAfterLastCoveredNonOpHour: When null, set to a new hour when it is operating.
        ///                                           If LastCoveredNonOpHour is updated, FirstOpHourAfterLastCoveredNonOpHour is set to null.
        /// </summary>
        /// <param name="monLocId">MON_LOC_ID for the location being updated.</param>
        /// <param name="opHour">The operating hour for which the update is occuring.</param>
        /// <param name="opTime">The operating time for the hour.</param>
        public void UpdateOperatingInformation(string monLocId, DateTime opHour, decimal opTime)
        {
            // Ensure the test is for the current location and operating hour is greater than or equal to the daily test hour
            if ((monLocId == MonLocId) && (opHour >= DailyTestDateHour))
            {
                // Only update if the op hour has not been handled.
                if ((LastHandledOpHour == null) || (opHour > LastHandledOpHour.Value))
                {
                    if (opTime > 0m)
                    {
                        OperatingHourCount += 1;

                        if (LastCoveredNonOpHour.HasValue && !FirstOpHourAfterLastCoveredNonOpHour.HasValue)
                            FirstOpHourAfterLastCoveredNonOpHour = opHour;
                    }
                    else
                    {
                        TimeSpan interval = opHour - DailyTestDateHour;
                        int hourCountIncludingTestHourAndOpHour = (int)Math.Round(interval.TotalHours) + 1; // Plus one include the hour of the test in the count.

                        // The test is affective for 26 hours including the hour of the test and the non-op hour must fall within that period or in the very next hour.
                        if ((1 <= hourCountIncludingTestHourAndOpHour) && (hourCountIncludingTestHourAndOpHour <= 27))
                        {
                            LastCoveredNonOpHour = opHour;
                            FirstOpHourAfterLastCoveredNonOpHour = null;
                        }

                        LastHandledOpHour = opHour;
                    }
                }
            }
        }

        #endregion
    }
}
