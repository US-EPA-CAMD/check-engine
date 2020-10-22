using System;
using System.Data;

using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Extensions;


namespace ECMPS.Checks.CheckEngine.SpecialParameterClasses
{

    /// <summary>
    /// This class is used to capture system operating day and hour counts. 
    /// </summary>
    public class OperatingSupplementalData
    {

        /// <summary>
        /// This class is used to capture system operating day and hour counts. 
        /// </summary>
        /// <param name="rptPeriodId">The primary key of the REPORTING_PERIOD table and indicating the quarter of the emission report.</param>
        /// <param name="id">The id under which the supplemental data is being tracked.</param>
        /// <param name="monLocId">The primary key to the MONITOR_LOCATION table for locations in the current emission report.</param>
        /// <param name="skipSavingModcCounts">Indicates whether to skip counting Quality Assured and Monitor Available hours.  Defaults to false.</param>
        protected OperatingSupplementalData(int rptPeriodId, string id, string monLocId, bool skipSavingModcCounts = false)
        {
            if (id == null)
            {
                throw new System.ArgumentException("Parameter cannot be null", "id");
            }

            if (monLocId == null)
            {
                throw new System.ArgumentException("Parameter cannot be null", "monLocId");
            }

            this.rptPeriodId = rptPeriodId;
            this.id = id;
            this.monLocId = monLocId;
            this.skipSavingModcCounts = skipSavingModcCounts;

            int quarter, year;
            {
                cDateFunctions.GetYearAndQuarter(rptPeriodId, out year, out quarter);
            }

            // Initialize the count structures.
            QuarterlyOperatingCounts = new OperatingSupplementalDataGroup(this, eOperatingSupplementalDataTimePeriod.Quarterly, eOperatingSupplementalDataCountType.Operating);
            QuarterlyQualityAssuredCounts = new OperatingSupplementalDataGroup(this, eOperatingSupplementalDataTimePeriod.Quarterly, eOperatingSupplementalDataCountType.QualityAssured);
            QuarterlyMonitorAvailableCounts = new OperatingSupplementalDataGroup(this, eOperatingSupplementalDataTimePeriod.Quarterly, eOperatingSupplementalDataCountType.MonitorAvailable);

            // Pnnly initialize May and June count structures if the quarter is Q2.
            if (quarter == 2)
            {
                MayAndJuneOperatingCounts = new OperatingSupplementalDataGroup(this, eOperatingSupplementalDataTimePeriod.MayAndJune, eOperatingSupplementalDataCountType.Operating);
                MayAndJuneQualityAssuredCounts = new OperatingSupplementalDataGroup(this, eOperatingSupplementalDataTimePeriod.MayAndJune, eOperatingSupplementalDataCountType.QualityAssured);
                MayAndJuneMonitorAvailableCounts = new OperatingSupplementalDataGroup(this, eOperatingSupplementalDataTimePeriod.MayAndJune, eOperatingSupplementalDataCountType.MonitorAvailable);
            }
        }


        #region Public Properties and Supporting Fields

        /// <summary>
        /// Returns the logical key for the data contained in the instance.
        /// </summary>
        public string Key {  get { return FormatKey(Id, RptPeriodId);  } }

        /// <summary>
        /// The id under which the supplemental data is being tracked.
        /// </summary>
        public string Id { get { return id; } }
        private readonly string id;

        /// <summary>
        /// The primary key to the MONITOR_LOCATION table.
        /// </summary>
        public string MonLocId { get { return monLocId; } }
        private readonly string monLocId;

        /// <summary>
        /// The primary key of the REPORTING_PERIOD table and indicating the quarter of the emission report.
        /// </summary>
        public int RptPeriodId { get { return rptPeriodId; } }
        private readonly int rptPeriodId;

        /// <summary>
        /// Indicates whether to skip counting Quality Assured and Monitor Available hours.
        /// </summary>
        public bool SkipSavingModcCounts { get { return skipSavingModcCounts; } }
        private readonly bool skipSavingModcCounts;

        /// <summary>
        /// Contains quarterly operating day and hours counts.
        /// </summary>
        public OperatingSupplementalDataGroup QuarterlyOperatingCounts { get; private set; }

        /// <summary>
        /// Contains quarterly quality assured operating day and hours counts.
        /// </summary>
        public OperatingSupplementalDataGroup QuarterlyQualityAssuredCounts { get; private set; }

        /// <summary>
        /// Contains monitor available operating day and hours counts.
        /// </summary>
        public OperatingSupplementalDataGroup QuarterlyMonitorAvailableCounts { get; private set; }

        /// <summary>
        /// Contains May and June operating day and hours counts for ozone season reporters.
        /// </summary>
        public OperatingSupplementalDataGroup MayAndJuneOperatingCounts { get; private set; }

        /// <summary>
        /// Contains May and June quality assured operating day and hours counts for ozone season reporters.
        /// </summary>
        public OperatingSupplementalDataGroup MayAndJuneQualityAssuredCounts { get; private set; }

        /// <summary>
        /// Contains May and June monitor available operating day and hours counts for ozone season reporters.
        /// </summary>
        public OperatingSupplementalDataGroup MayAndJuneMonitorAvailableCounts { get; private set; }

        #endregion


        #region Public Methods

        /// <summary>
        /// Increaments QuarterlyOperatingCounts, QuarterlyQualityAssuredCounts, QuarterlyMonitorAvailableCounts, MayAndJuneOperatingCounts, MayAndJuneQualityAssuredCounts and MayAndJuneMonitorAvailableCounts.
        /// 
        /// 
        /// QuarterlyOperatingCounts is increamented when an increament has not occured for date for the day count or hour for the hour count.
        /// 
        /// QuarterlyQualityAssuredCounts is increamented when an increament has not occured for date for the day count or hour for the hour count, and the hour is a quality assured hour.
        /// 
        /// QuarterlyMonitorAvailableCounts is increamented when an increament has not occured for date for the day count or hour for the hour count, and the hour is a monitor available hour.
        /// 
        /// 
        /// The following are only increameanted when the operating hour occurred in May or June:
        /// 
        ///     MayAndJuneOperatingCounts is increamented when an increament has not occured for date for the day count or hour for the hour count.
        /// 
        ///     MayAndJuneQualityAssuredCounts is increamented when an increament has not occured for date for the day count or hour for the hour count, and the hour is a quality assured hour.
        /// 
        ///     MayAndJuneMonitorAvailableCounts is increamented when an increament has not occured for date for the day count or hour for the hour count, and the hour is a monitor available hour.
        /// </summary>
        /// <param name="currentOperatingHour">The current operating hour for which the instance will update counts.</param>
        /// <param name="modcCd">The MODC for which qaulity assured and monitor availabe counts will be updated.</param>
        public void IncreamentForCurrentHour(DateTime currentOperatingHour, string modcCd)
        {
            bool qualityAssured = (modcCd != null) && modcCd.InList(QualityAssuredModcList);
            bool monitorAvailable = (modcCd != null) && modcCd.InList(MonitorAvailableModcList);


            QuarterlyOperatingCounts.IncreamentForCurrentHour(currentOperatingHour);

            if (qualityAssured)
                QuarterlyQualityAssuredCounts.IncreamentForCurrentHour(currentOperatingHour);

            if (monitorAvailable)
                QuarterlyMonitorAvailableCounts.IncreamentForCurrentHour(currentOperatingHour);


            // Only increameant for May and June.
            if ((currentOperatingHour.Month == 5) || (currentOperatingHour.Month == 6))
            {
                if (MayAndJuneOperatingCounts != null)
                    MayAndJuneOperatingCounts.IncreamentForCurrentHour(currentOperatingHour);

                if ((MayAndJuneQualityAssuredCounts != null) && qualityAssured)
                    MayAndJuneQualityAssuredCounts.IncreamentForCurrentHour(currentOperatingHour);

                if ((MayAndJuneMonitorAvailableCounts != null) && monitorAvailable)
                    MayAndJuneMonitorAvailableCounts.IncreamentForCurrentHour(currentOperatingHour);
            }
        }


        /// <summary>
        /// TESTING ONLY: Initializes count values.
        /// </summary>
        /// <param name="days">The day count for testing.</param>
        /// <param name="hours">The hour count for testing.</param>
        /// <param name="timePeriod">The time period of the day and hour counts to initialize.</param>
        /// <param name="countType">The count type of the day and hour counts to initialize.</param>
        public void ResetValuesForTesting(ushort days, ushort hours, eOperatingSupplementalDataTimePeriod? timePeriod = null, eOperatingSupplementalDataCountType? countType = null)
        {
            if ((timePeriod == eOperatingSupplementalDataTimePeriod.Quarterly || timePeriod == null) &&
                (countType == eOperatingSupplementalDataCountType.Operating || countType == null))
            {
                QuarterlyOperatingCounts = new OperatingSupplementalDataGroup(this, eOperatingSupplementalDataTimePeriod.Quarterly, 
                                                                                    eOperatingSupplementalDataCountType.Operating, 
                                                                                    days, hours);
            }

            if ((timePeriod == eOperatingSupplementalDataTimePeriod.Quarterly || timePeriod == null) &&
                (countType == eOperatingSupplementalDataCountType.QualityAssured || countType == null))
            {
                QuarterlyQualityAssuredCounts = new OperatingSupplementalDataGroup(this, eOperatingSupplementalDataTimePeriod.Quarterly,
                                                                                         eOperatingSupplementalDataCountType.QualityAssured,
                                                                                         days, hours);
            }

            if ((timePeriod == eOperatingSupplementalDataTimePeriod.Quarterly || timePeriod == null) &&
                (countType == eOperatingSupplementalDataCountType.MonitorAvailable || countType == null))
            {
                QuarterlyMonitorAvailableCounts = new OperatingSupplementalDataGroup(this, eOperatingSupplementalDataTimePeriod.Quarterly,
                                                                                           eOperatingSupplementalDataCountType.MonitorAvailable,
                                                                                           days, hours);
            }

            if ((timePeriod == eOperatingSupplementalDataTimePeriod.MayAndJune || timePeriod == null) &&
                (countType == eOperatingSupplementalDataCountType.Operating || countType == null))
            {
                MayAndJuneOperatingCounts = new OperatingSupplementalDataGroup(this, eOperatingSupplementalDataTimePeriod.MayAndJune,
                                                                                     eOperatingSupplementalDataCountType.Operating,
                                                                                     days, hours);
            }

            if ((timePeriod == eOperatingSupplementalDataTimePeriod.MayAndJune || timePeriod == null) &&
                (countType == eOperatingSupplementalDataCountType.QualityAssured || countType == null))
            {
                MayAndJuneQualityAssuredCounts = new OperatingSupplementalDataGroup(this, eOperatingSupplementalDataTimePeriod.MayAndJune,
                                                                                          eOperatingSupplementalDataCountType.QualityAssured,
                                                                                          days, hours);
            }

            if ((timePeriod == eOperatingSupplementalDataTimePeriod.MayAndJune || timePeriod == null) &&
                (countType == eOperatingSupplementalDataCountType.MonitorAvailable || countType == null))
            {
                MayAndJuneMonitorAvailableCounts = new OperatingSupplementalDataGroup(this, eOperatingSupplementalDataTimePeriod.MayAndJune,
                                                                                            eOperatingSupplementalDataCountType.MonitorAvailable,
                                                                                            days, hours);
            }
        }

        #endregion


        #region Static Properties and Methods

        /// <summary>
        /// Returns the formated logical key based on the passed values.
        /// </summary>
        /// <param name="id">The key for the supplemental data.</param>
        /// <param name="rptPeriodId">The reporting period key for the supplemental data.</param>
        /// <returns>The formatted locical key based on the system and reporting period keys.</returns>
        public static string FormatKey(string id, int rptPeriodId)
        {
            return id + "|" + rptPeriodId.ToString();
        }

        /// <summary>
        /// The list of MODC for monitor availabe hours.
        /// </summary>
        public static string MonitorAvailableModcList { get { return "01,02,04,14,16,17,19,20,21,22,32,33,41,42,43,44,53"; } }

        /// <summary>
        /// The list of MODC for quality assured hours.
        /// </summary>
        public static string QualityAssuredModcList { get { return "01,02,03,04,14,16,17,19,20,21,22,32,33,41,42,43,44,47,53,54"; } }

        /// <summary>
        /// Creates a new instance of SupplementalDataUpdateDataTable and populates it with data from the passed dictionary array.  
        /// </summary>
        /// <param name="supplementalData">The data used to update the table.</param>
        /// <param name="idColumnName">The name of the id column used as the supplemental data key.</param>
        /// <param name="SupplementalDataUpdateDataTable">Table to update with data.</param>
        /// <param name="workspaceSessionId">The workspace session id for check session.</param>
        protected static void LoadSupplementalDataUpdateDataDoRow(OperatingSupplementalData supplementalData, string idColumnName, DataTable SupplementalDataUpdateDataTable, decimal workspaceSessionId)
        {
            if (SupplementalDataUpdateDataTable != null)
            {
                if (supplementalData.QuarterlyOperatingCounts != null)
                    LoadSupplementalDataUpdateDataDoRow("OP", idColumnName, SupplementalDataUpdateDataTable, supplementalData.QuarterlyOperatingCounts, workspaceSessionId);

                if ((supplementalData.QuarterlyQualityAssuredCounts != null) && !supplementalData.SkipSavingModcCounts)
                    LoadSupplementalDataUpdateDataDoRow("QA", idColumnName, SupplementalDataUpdateDataTable, supplementalData.QuarterlyQualityAssuredCounts, workspaceSessionId);

                if ((supplementalData.QuarterlyMonitorAvailableCounts != null) && !supplementalData.SkipSavingModcCounts)
                    LoadSupplementalDataUpdateDataDoRow("MA", idColumnName, SupplementalDataUpdateDataTable, supplementalData.QuarterlyMonitorAvailableCounts, workspaceSessionId);

                if (supplementalData.MayAndJuneOperatingCounts != null)
                    LoadSupplementalDataUpdateDataDoRow("OPMJ", idColumnName, SupplementalDataUpdateDataTable, supplementalData.MayAndJuneOperatingCounts, workspaceSessionId);

                if ((supplementalData.MayAndJuneQualityAssuredCounts != null) && !supplementalData.SkipSavingModcCounts)
                    LoadSupplementalDataUpdateDataDoRow("QAMJ", idColumnName, SupplementalDataUpdateDataTable, supplementalData.MayAndJuneQualityAssuredCounts, workspaceSessionId);

                if ((supplementalData.MayAndJuneMonitorAvailableCounts != null) && !supplementalData.SkipSavingModcCounts)
                    LoadSupplementalDataUpdateDataDoRow("MAMJ", idColumnName, SupplementalDataUpdateDataTable, supplementalData.MayAndJuneMonitorAvailableCounts, workspaceSessionId);
            }
        }

        /// <summary>
        /// Adds a row to SupplementalDataUpdateDataTable populated with the values suppDataCd and fromsupplementalDataGroup.
        /// </summary>
        /// <param name="suppDataCd">The operating supplemental data code for the supplementalDataGroup data.</param>
        /// <param name="idColumnName">The name of the id column used as the supplemental data key.</param>
        /// <param name="SupplementalDataUpdateDataTable">Table to update with data.</param>
        /// <param name="supplementalDataGroup">The counts from which to populate the row.</param>
        /// <param name="workspaceSessionId">The workspace session id for check session.</param>
        private static void LoadSupplementalDataUpdateDataDoRow(string suppDataCd, string idColumnName, 
                                                                DataTable SupplementalDataUpdateDataTable, OperatingSupplementalDataGroup supplementalDataGroup, 
                                                                decimal workspaceSessionId)
        {
            DataRow dataRow = SupplementalDataUpdateDataTable.NewRow();

            dataRow["SESSION_ID"] = workspaceSessionId;
            dataRow[idColumnName] = supplementalDataGroup.Id;
            dataRow["MON_LOC_ID"] = supplementalDataGroup.MonLocId;
            dataRow["RPT_PERIOD_ID"] = supplementalDataGroup.RptPeriodId;
            dataRow["OP_SUPP_DATA_TYPE_CD"] = suppDataCd;
            dataRow["DAYS"] = supplementalDataGroup.Days;
            dataRow["HOURS"] = supplementalDataGroup.Hours;

            SupplementalDataUpdateDataTable.Rows.Add(dataRow);
        }

        #endregion

        #region Equality, Hash and ToSting implementations and overrides

        /// <summary>
        /// Base object Equals override that checks for comparison to null, the same object, different types,
        /// and finally uses the type specific Equals.
        /// </summary>
        /// <param name="that">The object instance to compare against this instance.</param>
        /// <returns>Returns true of the objects are of the same type and have the same key contents.</returns>
        public override bool Equals(object that)
        {
            // If that is null then this and that are not equal since this is never null.
            if (object.ReferenceEquals(that, null))
                return false;

            // If this and that are the same object then they are equal
            if (object.ReferenceEquals(this, that))
                return true;

            // If this and that have different types, they are not equal
            if (this.GetType() != that.GetType())
                return false;

            return this.Equals(that as SystemOperatingSupplementalData);
        }

        /// <summary>
        /// Type specific Equals implementation that uses MonSysId and RptPeriodId.
        /// </summary>
        /// <param name="that">The SystemOperatingSupplementalData instance to compare against this instance.</param>
        /// <returns>Returns true if the MonSysId and RptPeriodId values are the same for the two instances.</returns>
        public bool Equals(OperatingSupplementalData that)
        {
            return (that != null) &&
                   this.Id.Equals(that.Id) &&
                   this.MonLocId.Equals(that.MonLocId) &&
                   this.RptPeriodId.Equals(that.RptPeriodId);
        }

        /// <summary>
        /// GetHashCode override that uses MonSysId and RptPeriodId.
        /// </summary>
        /// <returns>Returns the hash based on MonSysId and RptPeriodId.</returns>
        public override int GetHashCode()
        {
            return this.Id.GetHashCode() ^ this.MonLocId.GetHashCode() ^ this.RptPeriodId.GetHashCode();
        }

        /// <summary>
        /// Returns the Monitor System and Reporting Period Ids
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Id + " " + MonLocId + " " + RptPeriodId.ToString();
        }

        #endregion

    }

}
