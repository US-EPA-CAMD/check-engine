using System;
using System.Collections.Generic;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Data.Ecmps.Dbo.View;
using ECMPS.Checks.Em.Parameters;
using ECMPS.Checks.TypeUtilities;
using ECMPS.Definitions.Extensions;
using ECMPS.Definitions.SeverityCode;


namespace ECMPS.Checks.CheckEngine.SpecialParameterClasses
{
    /// <summary>
    /// 
    /// </summary>
    public class cDailyCalibrationTestData
    {

        #region Public Constructor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dailyCalibrationSeverityCd">The daily alibration category object's severity code.</param>
        public cDailyCalibrationTestData(eSeverityCd dailyCalibrationSeverityCd)
        {
            string errorMessage;

            SeverityCd = dailyCalibrationSeverityCd;

            FromPreviousQuarter = false;

            LocationSupplementalValues = new DailyCalibrationSupplementalValues(this);
            SystemSupplementalValuesDictionary = new Dictionary<string, DailyCalibrationSupplementalValues>();



            DailyCalibrationRow = EmParameters.CurrentDailyCalibrationTest.SourceRow;
            Online = (EmParameters.DailyCalCalcOnlineInd.Default(1) == 1);
            CalculatedTestResultCd = EmParameters.DailyCalCalcResult;

            if (CalculatedTestResultCd.InList("INC,IGNORED"))
                Valid = null;
            else
                Valid = (CalculatedTestResultCd != "INVALID");

            if (isDailyCalibrationRow(DailyCalibrationRow, out errorMessage))
            {
                MonLocId = DailyCalibrationRow["MON_LOC_ID"].AsString();
                ComponentId = DailyCalibrationRow["COMPONENT_ID"].AsString();
                ComponentIdentifier = DailyCalibrationRow["COMPONENT_IDENTIFIER"].AsString();


                switch (CalculatedTestResultCd)
                {
                    case "FAILED":
                        {
                            DailyTestDate = EmParameters.DailyCalFailDate.Default(DateTypes.START);
                            DailyTestHour = EmParameters.DailyCalFailHour.Default(0);
                            DailyTestMinute = 0;//arbitrary value
                        }
                        break;

                    case "ABORTED":
                        {
                            DailyTestDate = DailyCalibrationRow["DAILY_TEST_DATE"].AsDateTime().Default(DateTypes.START);
                            DailyTestHour = DailyCalibrationRow["DAILY_TEST_HOUR"].AsInteger().Default();
                            DailyTestMinute = 0;//arbitrary value
                        }
                        break;

                    default:
                        {
                            DailyTestDate = DailyCalibrationRow["DAILY_TEST_DATE"].AsDateTime().Default(DateTypes.START);
                            DailyTestHour = DailyCalibrationRow["DAILY_TEST_HOUR"].AsInteger().Default();
                            DailyTestMinute = DailyCalibrationRow["DAILY_TEST_MIN"].AsInteger().Default(0);
                        }
                        break;
                }

                string spanScaleCd = DailyCalibrationRow["SPAN_SCALE_CD"].AsString();
                {
                    switch (spanScaleCd.Default())
                    {
                        case "H": SpanScaleCd = eSpanScale.High; break;
                        case "L": SpanScaleCd = eSpanScale.Low; break;
                        case "": SpanScaleCd = eSpanScale.None; break;

                        default:
                            {
                                Exception exception = new Exception(string.Format("Span Scale invalid: {0}", spanScaleCd));
                                throw exception;
                            }
                    }
                }


                if (EmParameters.PrimaryBypassActiveForHour.Default(false)) // Component type should only include CO2, NOX or O2.
                {
                    CheckDataView<VwMpMonitorSystemComponentRow> monitorSystemComponentRecords
                        = EmParameters.MonitorSystemComponentRecordsByHourLocation.FindRows(new cFilterCondition("COMPONENT_ID", ComponentId),
                                                                                            new cFilterCondition("SYS_TYPE_CD", "NOX"),
                                                                                            new cFilterCondition("SYS_DESIGNATION_CD", "P,PB", eFilterConditionStringCompare.InList));

                    foreach (VwMpMonitorSystemComponentRow monitorSystemComponentRecord in monitorSystemComponentRecords)
                    {
                        SystemSupplementalValuesDictionary.Add(monitorSystemComponentRecord.MonSysId, new DailyCalibrationSupplementalValues(this));
                    }
                }

            }
            else
            {
                Exception exception = new Exception(errorMessage);
                throw exception;
            }
        }

        /// <summary>
        /// Creates Daily Calibration Test Data object from a Daily Test (Location) Supplemental Data row and a list
        /// of Daily Test System Supplemental Data rows.
        /// </summary>
        /// <param name="dailyTestLocationSuppDataRow">The Daily Test (Location) Supplemental Data row</param>
        /// <param name="dailyTestSystemSuppDataRows">The Daily Test System Supplemental Data rows.</param>
        public cDailyCalibrationTestData(DataRowView dailyTestLocationSuppDataRow, DataView dailyTestSystemSuppDataRows)
        {
            // TODO: Ensure that this is not really needed.
            //SeverityCd = dailyCalibrationSeverityCd;

            FromPreviousQuarter = true;

            DailyCalibrationRow = dailyTestLocationSuppDataRow;

            CalculatedTestResultCd = dailyTestLocationSuppDataRow["CALC_TEST_RESULT_CD"].AsString();
            ComponentId = dailyTestLocationSuppDataRow["COMPONENT_ID"].AsString();
            ComponentIdentifier = dailyTestLocationSuppDataRow["COMPONENT_IDENTIFIER"].AsString();
            DailyTestDate = dailyTestLocationSuppDataRow["SORT_DAILY_TEST_DATEHOURMIN"].AsDateTime().Default(DateTypes.START).Date;
            DailyTestHour = dailyTestLocationSuppDataRow["SORT_DAILY_TEST_DATEHOURMIN"].AsDateTime().Default(DateTypes.START).Hour;
            DailyTestMinute = dailyTestLocationSuppDataRow["SORT_DAILY_TEST_DATEHOURMIN"].AsDateTime().Default(DateTypes.START).Minute;
            MonLocId = dailyTestLocationSuppDataRow["MON_LOC_ID"].AsString();
            Online = (dailyTestLocationSuppDataRow["KEY_ONLINE_IND"].AsInteger() == 1);
            SpanScaleCd = dailyTestLocationSuppDataRow["SPAN_SCALE_CD"].AsString() == "H" ? eSpanScale.High : dailyTestLocationSuppDataRow["SPAN_SCALE_CD"].AsString() == "L" ? eSpanScale.Low : eSpanScale.None;
            Valid = (dailyTestLocationSuppDataRow["KEY_VALID_IND"].AsInteger() == 1);

            LocationSupplementalValues = new DailyCalibrationSupplementalValues(this, dailyTestLocationSuppDataRow["FIRST_OP_AFTER_NONOP_DATEHOUR"].AsDateTime(),
                                                                                      dailyTestLocationSuppDataRow["LAST_COVERED_NONOP_DATEHOUR"].AsDateTime(),
                                                                                      null, // Value is not necessary since last hour was in emissions report for previous quarter
                                                                                      dailyTestLocationSuppDataRow["OP_HOUR_CNT"].AsInteger(0));

            SystemSupplementalValuesDictionary = new Dictionary<string, DailyCalibrationSupplementalValues>();
            {
                foreach (DataRowView dailyTestSystemSuppDataRow in dailyTestSystemSuppDataRows)
                {
                    if (dailyTestSystemSuppDataRow["DAILY_TEST_SUPP_DATA_ID"].AsString() == dailyTestLocationSuppDataRow["DAILY_TEST_SUPP_DATA_ID"].AsString())
                    {
                        string monSysId = dailyTestSystemSuppDataRow["MON_SYS_ID"].AsString();

                        if (!SystemSupplementalValuesDictionary.ContainsKey(monSysId)) // MON_SYS_ID should not occur twice for the same DAILY_TEST_SUPP_DATA_ID
                        {
                            SystemSupplementalValuesDictionary.Add(monSysId, new DailyCalibrationSupplementalValues(this, dailyTestSystemSuppDataRow["FIRST_OP_AFTER_NONOP_DATEHOUR"].AsDateTime(),
                                                                                                                          dailyTestSystemSuppDataRow["LAST_COVERED_NONOP_DATEHOUR"].AsDateTime(),
                                                                                                                          null, // Value is not necessary since last hour was in emissions report for previous quarter
                                                                                                                          dailyTestSystemSuppDataRow["OP_HOUR_CNT"].AsInteger(0)));
                        }
                    }
                }
            }
        }

        #endregion


        #region Public Properties General

        /// <summary>
        /// The Component Id for this set of daily calibration data.
        /// </summary>
        public string ComponentId { get; private set; }

        /// <summary>
        /// The Component Identifier for this set of daily calibration data.
        /// </summary>
        public string ComponentIdentifier { get; private set; }

        /// <summary>
        /// The daily calibration row used to derive row related properties.
        /// </summary>
        public DataRowView DailyCalibrationRow { get; private set; }

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
        /// The Online Offline Indicator for this set of daily calibration data.
        /// </summary>
        public bool Online { get; private set; }

        /// <summary>
        /// The Severity Code for the Daily Calibration Test
        /// </summary>
        public eSeverityCd SeverityCd { get; private set; }

        /// <summary>
        /// The Span Scale Code for this set of daily calibration data.
        /// </summary>
        public eSpanScale SpanScaleCd { get; private set; }

        /// <summary>
        /// The Calculated Test Result Code for this set of daily calibration data.
        /// </summary>
        public string CalculatedTestResultCd { get; private set; }

        /// <summary>
        /// Indicates whether the test was valid (true), invalid (false) or incomplete (null).
        /// </summary>
        public bool? Valid { get; private set; }

        #endregion


        #region Public Properties Supplemental Data Tracking

        /// <summary>
        /// Indicates whether this data from the previous quarter.
        /// </summary>
        private bool FromPreviousQuarter { get; set; }

        /// <summary>
        /// Contains the values used to populate Daily Test Supplemental Data
        /// </summary>
        public DailyCalibrationSupplementalValues LocationSupplementalValues { get; private set; }


        /// <summary>
        /// The MON_LOC_ID for the test.  Used when updating operating information.
        /// </summary>
        public string MonLocId { get; private set; }

        /// <summary>
        /// Contains a dictionary with a MON_SYS_ID key and Daily Calibration Supplemental Data values.
        /// 
        /// This is only needed when primary and primary bypass stacks are reported as systems.
        /// This will allow operating information updates for specific systems (stacks).
        /// </summary>
        public Dictionary<string, DailyCalibrationSupplementalValues> SystemSupplementalValuesDictionary { get; set; }

        #endregion


        #region Public Methods

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
        /// <param name="systemOpTimeDictionary">Contains the MON_SYS_ID and operating times for the primary and primary bypass systems When a primary bypass is active.</param>
        public void UpdateOperatingInformation(string monLocId, DateTime opHour, decimal opTime, Dictionary<string, decimal> systemOpTimeDictionary)
        {
            // Ensure the test is for the current location and operating hour is greater than or equal to the daily test hour
            if ((monLocId == MonLocId) && (opHour >= DailyTestDateHour))
            {
                // Update the location level values.
                LocationSupplementalValues.Update(opHour, opTime);

                // If a system was passed, update the system level values.
                if ((systemOpTimeDictionary != null) && (systemOpTimeDictionary.Count > 0))
                {
                    foreach (string monSysId in systemOpTimeDictionary.Keys)
                    {
                        // Update the dictionary entryfor the system if it exists
                        if (SystemSupplementalValuesDictionary.ContainsKey(monSysId))
                        {
                            SystemSupplementalValuesDictionary[monSysId].Update(opHour, systemOpTimeDictionary[monSysId]);
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Adds a row to SupplementalDataUpdateDataTable populated with the values suppDataCd and fromsupplementalDataGroup.
        /// </summary>
        /// <param name="SupplementalDataUpdateLocationDataTable"></param>
        /// <param name="SupplementalDataUpdateSystemDataTable"></param>
        /// <param name="rptPeriodId"></param>
        /// <param name="workspaceSessionId"></param>
        public void LoadIntoSupplementalDataTables(DataTable SupplementalDataUpdateLocationDataTable, DataTable SupplementalDataUpdateSystemDataTable, int rptPeriodId, decimal workspaceSessionId)
        {
            // Only store values for tests that are not incomplete or ignored (Valid is null), and are online tests
            if (Valid.HasValue && !FromPreviousQuarter)
            {
                DataRow dataRow;

                dataRow = SupplementalDataUpdateLocationDataTable.NewRow();

                dataRow["SESSION_ID"] = workspaceSessionId;
                dataRow["DAILY_TEST_SUM_ID"] = DailyCalibrationRow["DAILY_TEST_SUM_ID"].AsString();
                dataRow["RPT_PERIOD_ID"] = rptPeriodId;
                dataRow["KEY_ONLINE_IND"] = (Online ? 1 : 0);
                dataRow["KEY_VALID_IND"] = (Valid.Value ? 1 : 0);
                dataRow["OP_HOUR_CNT"] = LocationSupplementalValues.OperatingHourCount;
                dataRow["LAST_COVERED_NONOP_DATEHOUR"] = LocationSupplementalValues.LastCoveredNonOpHour.DbValue();
                dataRow["FIRST_OP_AFTER_NONOP_DATEHOUR"] = LocationSupplementalValues.FirstOpHourAfterLastCoveredNonOpHour.DbValue();
                dataRow["SORT_DAILY_TEST_DATEHOURMIN"] = DailyTestDate.AddHours(DailyTestHour).AddMinutes(DailyTestMinute);
                dataRow["CALC_TEST_RESULT_CD"] = CalculatedTestResultCd;

                SupplementalDataUpdateLocationDataTable.Rows.Add(dataRow);

                foreach (string monSysId in SystemSupplementalValuesDictionary.Keys)
                {
                    DailyCalibrationSupplementalValues systemSupplementalValues = SystemSupplementalValuesDictionary[monSysId];

                    dataRow = SupplementalDataUpdateSystemDataTable.NewRow();

                    dataRow["SESSION_ID"] = workspaceSessionId;
                    dataRow["DAILY_TEST_SUM_ID"] = DailyCalibrationRow["DAILY_TEST_SUM_ID"].AsString();
                    dataRow["RPT_PERIOD_ID"] = rptPeriodId;
                    dataRow["KEY_ONLINE_IND"] = (Online ? 1 : 0);
                    dataRow["KEY_VALID_IND"] = (Valid.Value ? 1 : 0);
                    dataRow["MON_SYS_ID"] = monSysId;
                    dataRow["OP_HOUR_CNT"] = systemSupplementalValues.OperatingHourCount;
                    dataRow["LAST_COVERED_NONOP_DATEHOUR"] = systemSupplementalValues.LastCoveredNonOpHour.DbValue();
                    dataRow["FIRST_OP_AFTER_NONOP_DATEHOUR"] = systemSupplementalValues.FirstOpHourAfterLastCoveredNonOpHour.DbValue();

                    SupplementalDataUpdateSystemDataTable.Rows.Add(dataRow);
                }
            }
        }
        
        #endregion


        #region Private Methods

        private bool isDailyCalibrationRow(DataRowView dailyCalibrationRow, out string errorMessage)
        {
            bool result = true;

            errorMessage = null;

            string missingColumns = null;

            DataColumnCollection columns = dailyCalibrationRow.Row.Table.Columns;

            if (!columns.Contains("MON_LOC_ID"))
            {
                missingColumns.ListAdd("MON_LOC_ID");
                result = false;
            }

            if (!columns.Contains("TEST_RESULT_CD"))
            {
                missingColumns.ListAdd("TEST_RESULT_CD");
                result = false;
            }

            if (!columns.Contains("COMPONENT_ID"))
            {
                missingColumns.ListAdd("COMPONENT_ID");
                result = false;
            }

            if (!columns.Contains("COMPONENT_IDENTIFIER"))
            {
                missingColumns.ListAdd("COMPONENT_IDENTIFIER");
                result = false;
            }

            if (!columns.Contains("SPAN_SCALE_CD"))
            {
                missingColumns.ListAdd("SPAN_SCALE_CD");
                result = false;
            }

            if (!columns.Contains("ONLINE_OFFLINE_IND"))
            {
                missingColumns.ListAdd("ONLINE_OFFLINE_IND");
                result = false;
            }

            if (!columns.Contains("DAILY_TEST_DATE"))
            {
                missingColumns.ListAdd("DAILY_TEST_DATE");
                result = false;
            }

            if (!columns.Contains("DAILY_TEST_HOUR"))
            {
                missingColumns.ListAdd("DAILY_TEST_HOUR");
                result = false;
            }

            if (!columns.Contains("DAILY_TEST_MIN"))
            {
                missingColumns.ListAdd("DAILY_TEST_MIN");
                result = false;
            }

            if (!result)
                errorMessage = string.Format("Daily Calibration row missing columns: {0}", missingColumns);

            return result;
        }

        #endregion

    }
}
