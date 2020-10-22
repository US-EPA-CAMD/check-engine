using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Extensions;


namespace ECMPS.Checks.CheckEngine.SpecialParameterClasses
{

    /// <summary>
    /// This class is used to capture QA certification event operating day and hour counts. 
    /// 
    /// 1. The class will capture counts during the evaluation of an emission report whenever the QA Certification Event Date
    ///    or Conditional Data Begin Date fall within the quarter for the emission report an the QA Certification Event is for
    ///    a location in the emission report.
    /// 2. The class will capture the number of operating days in the quarter of the event date that occur after the event date
    ///    when the event date occurs in the quarter of the emission report.
    /// 3. The class will capture the number of operating hours in the quarter of the conditional data begin date that occur 
    ///    after the conditional data begin hour.
    /// </summary>
    public class QaCertificationSupplementalData
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rptPeriodId">The primary key of the REPORTING_PERIOD table and indicating the quarter of the emission report.</param>
        /// <param name="monLocId">The primary key of the MONITOR_LOCATION.</param>
        /// <param name="qaCertEventId">The primary key of the QA_CERT_EVENT.</param>
        /// <param name="targetDatehourCode">Indicates whether the count is based on the QA Cert Event Datehour or Conditiona Data Begin Datehour.</param>
        /// <param name="targetDatehourValue">The actual value of the datehour on which the count will be based.</param>
        public QaCertificationSupplementalData(int rptPeriodId, string monLocId, string qaCertEventId, 
                                               eQaCertificationSupplementalDataTargetDateHour targetDatehourCode, DateTime targetDatehourValue)
        {
            if (qaCertEventId == null)
            {
                throw new System.ArgumentException("Parameter cannot be null", "qaCertEventId");
            }

            if (monLocId == null)
            {
                throw new System.ArgumentException("Parameter cannot be null", "monLocId");
            }

            int quarter, year;
            {
                cDateFunctions.GetYearAndQuarter(rptPeriodId, out year, out quarter);
            }

            this.qaCertEventId = qaCertEventId;
            this.monLocId = monLocId;
            this.rptPeriodId = rptPeriodId;
            this.targetDatehourCode = targetDatehourCode;
            this.targetDatehourValue = targetDatehourValue;

            this.timeType = (targetDatehourCode == eQaCertificationSupplementalDataTargetDateHour.QaCertEventDate)
                          ? eQaCertificationSupplementalDataTimeType.Day 
                          : eQaCertificationSupplementalDataTimeType.Hour;

            // Initialize the QA Cert Event Date Count structures.
            QuarterlyOperatingCounts = new QaCertificationSupplementalDataGroup(this, eQaCertificationSupplementalDataTimePeriod.Quarterly, eQaCertificationSupplementalDataCountType.Operating, year, quarter);
            QuarterlySystemOperatingCounts = new QaCertificationSupplementalDataGroup(this, eQaCertificationSupplementalDataTimePeriod.Quarterly, eQaCertificationSupplementalDataCountType.SystemOperating, year, quarter);
            QuarterlyComponentOperatingCounts = new QaCertificationSupplementalDataGroup(this, eQaCertificationSupplementalDataTimePeriod.Quarterly, eQaCertificationSupplementalDataCountType.ComponentOperating, year, quarter);
            QuarterlySystemQualityAssuredCounts = new QaCertificationSupplementalDataGroup(this, eQaCertificationSupplementalDataTimePeriod.Quarterly, eQaCertificationSupplementalDataCountType.SystemQualityAssured, year, quarter);
            QuarterlyComponentQualityAssuredCounts = new QaCertificationSupplementalDataGroup(this, eQaCertificationSupplementalDataTimePeriod.Quarterly, eQaCertificationSupplementalDataCountType.ComponentQualityAssured, year, quarter);

            if (quarter == 2)
            {
                MayAndJuneOperatingCounts = new QaCertificationSupplementalDataGroup(this, eQaCertificationSupplementalDataTimePeriod.MayAndJune, eQaCertificationSupplementalDataCountType.Operating, year, quarter);
                MayAndJuneSystemOperatingCounts = new QaCertificationSupplementalDataGroup(this, eQaCertificationSupplementalDataTimePeriod.MayAndJune, eQaCertificationSupplementalDataCountType.SystemOperating, year, quarter);
                MayAndJuneComponentOperatingCounts = new QaCertificationSupplementalDataGroup(this, eQaCertificationSupplementalDataTimePeriod.MayAndJune, eQaCertificationSupplementalDataCountType.ComponentOperating, year, quarter);
                MayAndJuneSystemQualityAssuredCounts = new QaCertificationSupplementalDataGroup(this, eQaCertificationSupplementalDataTimePeriod.MayAndJune, eQaCertificationSupplementalDataCountType.SystemQualityAssured, year, quarter);
                MayAndJuneComponentQualityAssuredCounts = new QaCertificationSupplementalDataGroup(this, eQaCertificationSupplementalDataTimePeriod.MayAndJune, eQaCertificationSupplementalDataCountType.ComponentQualityAssured, year, quarter);
            }
            else
            {
                MayAndJuneOperatingCounts = null;
                MayAndJuneSystemOperatingCounts = null;
                MayAndJuneComponentOperatingCounts = null;
                MayAndJuneSystemQualityAssuredCounts = null;
                MayAndJuneComponentQualityAssuredCounts = null;
            }

        }


        #region Public Properties and Supporting Fields

        /// <summary>
        /// The primary key of the QA_CERT_EVENT being tracked.
        /// </summary>
        public string QaCertEventId { get { return qaCertEventId; } }
        private readonly string qaCertEventId;

        /// <summary>
        /// The primary key of the QA_CERT_EVENT being tracked.
        /// </summary>
        public string MonLocId { get { return monLocId; } }
        private readonly string monLocId;

        /// <summary>
        /// The primary key of the REPORTING_PERIOD table and indicating the quarter of the emission report.
        /// </summary>
        public int RptPeriodId { get { return rptPeriodId; } }
        private readonly int rptPeriodId;

        /// <summary>
        /// Indicates whether the count is based on the QA Cert Event Datehour or Conditiona Data Begin Datehour.
        /// </summary>
        public eQaCertificationSupplementalDataTargetDateHour TargetDatehourCode { get { return targetDatehourCode; } }
        private readonly eQaCertificationSupplementalDataTargetDateHour targetDatehourCode;

        /// <summary>
        /// The actual value of the datehour on which the count will be based.
        /// </summary>
        public DateTime TargetDatehourValue { get { return targetDatehourValue; } }
        private readonly DateTime targetDatehourValue;

        /// <summary>
        /// Indicates whether the count is of days or hours.
        /// </summary>
        public eQaCertificationSupplementalDataTimeType TimeType { get { return timeType; } }
        private readonly eQaCertificationSupplementalDataTimeType timeType;

        /// <summary>
        /// Contains quarterly operating day and hours counts.
        /// </summary>
        public QaCertificationSupplementalDataGroup QuarterlyOperatingCounts { get; private set; }

        /// <summary>
        /// Contains quarterly system operating day and hours counts.
        /// </summary>
        public QaCertificationSupplementalDataGroup QuarterlySystemOperatingCounts { get; private set; }

        /// <summary>
        /// Contains quarterly component operating day and hours counts.
        /// </summary>
        public QaCertificationSupplementalDataGroup QuarterlyComponentOperatingCounts { get; private set; }

        /// <summary>
        /// Contains quarterly system quality assured operating day and hours counts.
        /// </summary>
        public QaCertificationSupplementalDataGroup QuarterlySystemQualityAssuredCounts { get; private set; }

        /// <summary>
        /// Contains quarterly component quality assured operating day and hours counts.
        /// </summary>
        public QaCertificationSupplementalDataGroup QuarterlyComponentQualityAssuredCounts { get; private set; }

        /// <summary>
        /// Contains May and June operating day and hours counts for ozone season reporters.
        /// </summary>
        public QaCertificationSupplementalDataGroup MayAndJuneOperatingCounts { get; private set; }

        /// <summary>
        /// Contains May and June system operating day and hours counts for ozone season reporters.
        /// </summary>
        public QaCertificationSupplementalDataGroup MayAndJuneSystemOperatingCounts { get; private set; }

        /// <summary>
        /// Contains May and June component operating day and hours counts for ozone season reporters.
        /// </summary>
        public QaCertificationSupplementalDataGroup MayAndJuneComponentOperatingCounts { get; private set; }

        /// <summary>
        /// Contains May and June system quality assured operating day and hours counts for ozone season reporters.
        /// </summary>
        public QaCertificationSupplementalDataGroup MayAndJuneSystemQualityAssuredCounts { get; private set; }

        /// <summary>
        /// Contains May and June component quality assured operating day and hours counts for ozone season reporters.
        /// </summary>
        public QaCertificationSupplementalDataGroup MayAndJuneComponentQualityAssuredCounts { get; private set; }

        #endregion


        #region Public Methods

        /// <summary>
        /// Increaments QuarterlyComponentOperatingCounts, QuarterlySystemQualityAssuredCounts, MayAndJuneComponentOperatingCounts and 
        /// MayAndJuneSystemQualityAssuredCounts.
        /// 
        /// QuarterlyComponentQualityAssuredCounts values are increamented when the date of the current hour is on or after the event date, 
        /// the MODC indicates a quality assured hour, and an increament has not occurred for the date of the current hour.
        /// 
        /// MayAndJuneComponentQualityAssuredCounts values are increamented when the date of the current hour is on or after the event date, 
        /// the MODC indicates a quality assured hour, the current hour is in May or June, and an increament has not occurred for the 
        /// date of the current hour.
        /// </summary>
        /// <param name="currentOperatingHour">The current operating hour for which the instance will update counts.</param>
        /// <param name="modcCd">The MODC code for the hourly record.</param>
        public void IncreamentComponentCounts(DateTime currentOperatingHour, string modcCd)
        {
            QuarterlyComponentOperatingCounts.IncreamentForCurrentHour(currentOperatingHour);

            if ((MayAndJuneComponentOperatingCounts != null) && ((currentOperatingHour.Month == 5) || (currentOperatingHour.Month == 6)))
            {
                MayAndJuneComponentOperatingCounts.IncreamentForCurrentHour(currentOperatingHour);
            }

            if (modcCd.InList(QualityAssuredModcList))
            {
                QuarterlyComponentQualityAssuredCounts.IncreamentForCurrentHour(currentOperatingHour);

                if ((MayAndJuneComponentQualityAssuredCounts != null) && ((currentOperatingHour.Month == 5) || (currentOperatingHour.Month == 6)))
                {
                    MayAndJuneComponentQualityAssuredCounts.IncreamentForCurrentHour(currentOperatingHour);
                }
            }
        }

        /// <summary>
        /// Increaments QaCertEventQuarterOpDays, ConditionalDataBeginQuarterOpHours and ConditionalDataBeginQuarterOsHours.
        /// 
        /// QaCertEventQuarterOpDays is increamented when the date of the current hour is after the event date, and an increament
        /// has not occurred for the date of current hour.
        /// 
        /// ConditionalDataBeginQuarterOpHours is increamented when current hour is after the conditional data peeriod begin hour.
        /// 
        /// ConditionalDataBeginQuarterOsHours is increamented when current hour is after the conditional data peeriod begin hour
        /// and it falls in an ozone season.
        /// </summary>
        /// <param name="currentOperatingHour">The current operating hour for which the instance will update counts.</param>
        public void IncreamentOperatingCounts(DateTime currentOperatingHour)
        {
            QuarterlyOperatingCounts.IncreamentForCurrentHour(currentOperatingHour);

            if ( (MayAndJuneOperatingCounts != null) && ( (currentOperatingHour.Month == 5) || (currentOperatingHour.Month == 6) ) )
            {
                MayAndJuneOperatingCounts.IncreamentForCurrentHour(currentOperatingHour);
            }
        }

        /// <summary>
        /// Increaments QuarterlySystemOperatingCounts, QuarterlySystemQualityAssuredCounts, MayAndJuneSystemOperatingCounts and 
        /// MayAndJuneSystemQualityAssuredCounts.
        /// 
        /// QuarterlySystemQualityAssuredCounts values are increamented when the date of the current hour is on or after the event date, 
        /// the MODC indicates a quality assured hour, and an increament has not occurred for the date of the current hour.
        /// 
        /// MayAndJuneSystemQualityAssuredCounts values are increamented when the date of the current hour is on or after the event date, 
        /// the MODC indicates a quality assured hour, the current hour is in May or June, and an increament has not occurred for the 
        /// date of the current hour.
        /// </summary>
        /// <param name="currentOperatingHour">The current operating hour for which the instance will update counts.</param>
        /// <param name="modcCd">The MODC code for the hourly record.</param>
        public void IncreamentSystemCounts(DateTime currentOperatingHour, string modcCd)
        {
            QuarterlySystemOperatingCounts.IncreamentForCurrentHour(currentOperatingHour);

            if ((MayAndJuneSystemOperatingCounts != null) && ((currentOperatingHour.Month == 5) || (currentOperatingHour.Month == 6)))
            {
                MayAndJuneSystemOperatingCounts.IncreamentForCurrentHour(currentOperatingHour);
            }

            if (modcCd.InList(QualityAssuredModcList))
            {
                QuarterlySystemQualityAssuredCounts.IncreamentForCurrentHour(currentOperatingHour);

                if ((MayAndJuneSystemQualityAssuredCounts != null) && ((currentOperatingHour.Month == 5) || (currentOperatingHour.Month == 6)))
                {
                    MayAndJuneSystemQualityAssuredCounts.IncreamentForCurrentHour(currentOperatingHour);
                }
            }
        }

        #endregion


        #region Static Properties and Methods

        /// <summary>
        /// Returns the formated logical key based on the passed values.
        /// </summary>
        /// <param name="qaCertEventId"></param>
        /// <param name="targetDatehourCode"></param>
        /// <returns>The formatted locical key based on the logical key values for the supplemental data.</returns>
        public static string FormatKey(string qaCertEventId, eQaCertificationSupplementalDataTargetDateHour targetDatehourCode)
        {
            return qaCertEventId + "|" + targetDatehourCode.ToString();
        }

        /// <summary>
        /// The list of MODC for quality assured hours.
        /// </summary>
        public static string QualityAssuredModcList {  get { return "01,02,03,04,14,16,17,19,20,21,22,32,33,41,42,43,44,47,53,54"; } }

        /// <summary>
        /// Contains the update data table object for supplemental data.
        /// </summary>
        public static DataTable SupplementalDataUpdateDataTable { get; set; }

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
        public static string SupplementalDataUpdateTableName { get { return "CE_QaCertEventSuppData"; } }

        /// <summary>
        /// Contains the name of the update table for sampling trian supplemental data.
        /// </summary>
        public static string SupplementalDataUpdateTablePath { get { return SupplementalDataUpdateDatabaseName + "." + SupplementalDataUpdateSchemaName + "." + SupplementalDataUpdateTableName; } }


        /// <summary>
        /// Creates a new instance of SupplementalDataUpdateDataTable and populates it with data from the passed dictionary array.  
        /// </summary>
        public static void LoadSupplementalDataUpdateDataTable(Dictionary<string, QaCertificationSupplementalData>[] supplementalDataDictionaryArray, decimal workspaceSessionId, SqlConnection connection)
        {
            SupplementalDataUpdateDataTable = cDataFunctions.CreateDataTable(SupplementalDataUpdateDatabaseName, SupplementalDataUpdateSchemaName, SupplementalDataUpdateTableName, connection);

            if (SupplementalDataUpdateDataTable != null)
            {
                string suppDateCd;

                foreach (Dictionary<string, QaCertificationSupplementalData> supplementalDataDictionary in supplementalDataDictionaryArray)
                {
                    foreach (QaCertificationSupplementalData supplementalData in supplementalDataDictionary.Values)
                    {
                        if ((supplementalData.TargetDatehourCode == eQaCertificationSupplementalDataTargetDateHour.QaCertEventDate))
                            suppDateCd = "QCEDATE";
                        else if ((supplementalData.TargetDatehourCode == eQaCertificationSupplementalDataTargetDateHour.ConditionalDataBeginHour))
                            suppDateCd = "CDBHOUR";
                        else
                            suppDateCd = null;

                        if (suppDateCd != null)
                        {
                            if (supplementalData.QuarterlyOperatingCounts != null)
                                LoadSupplementalDataUpdateDataRow("OP", suppDateCd, supplementalData.QuarterlyOperatingCounts, workspaceSessionId);

                            if (supplementalData.QuarterlySystemOperatingCounts != null)
                                LoadSupplementalDataUpdateDataRow("SYSOP", suppDateCd, supplementalData.QuarterlySystemOperatingCounts, workspaceSessionId);

                            if (supplementalData.QuarterlyComponentOperatingCounts != null)
                                LoadSupplementalDataUpdateDataRow("CMPOP", suppDateCd, supplementalData.QuarterlyComponentOperatingCounts, workspaceSessionId);

                            if (supplementalData.QuarterlySystemQualityAssuredCounts != null)
                                LoadSupplementalDataUpdateDataRow("SYSQA", suppDateCd, supplementalData.QuarterlySystemQualityAssuredCounts, workspaceSessionId);

                            if (supplementalData.QuarterlyComponentQualityAssuredCounts != null)
                                LoadSupplementalDataUpdateDataRow("CMPQA", suppDateCd, supplementalData.QuarterlyComponentQualityAssuredCounts, workspaceSessionId);

                            if (supplementalData.MayAndJuneOperatingCounts != null)
                                LoadSupplementalDataUpdateDataRow("OPMJ", suppDateCd, supplementalData.MayAndJuneOperatingCounts, workspaceSessionId);

                            if (supplementalData.MayAndJuneSystemOperatingCounts != null)
                                LoadSupplementalDataUpdateDataRow("SYSOPMJ", suppDateCd, supplementalData.MayAndJuneSystemOperatingCounts, workspaceSessionId);

                            if (supplementalData.MayAndJuneComponentOperatingCounts != null)
                                LoadSupplementalDataUpdateDataRow("CMPOPMJ", suppDateCd, supplementalData.MayAndJuneComponentOperatingCounts, workspaceSessionId);

                            if (supplementalData.MayAndJuneSystemQualityAssuredCounts != null)
                                LoadSupplementalDataUpdateDataRow("SYSQAMJ", suppDateCd, supplementalData.MayAndJuneSystemQualityAssuredCounts, workspaceSessionId);

                            if (supplementalData.MayAndJuneComponentQualityAssuredCounts != null)
                                LoadSupplementalDataUpdateDataRow("CMPQAMJ", suppDateCd, supplementalData.MayAndJuneComponentQualityAssuredCounts, workspaceSessionId);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Adds a row to SupplementalDataUpdateDataTable populated with the values suppDataCd and fromsupplementalDataGroup.
        /// </summary>
        /// <param name="suppDataCd">The QA Certification Event supplemental data code for the supplementalDataGroup data.</param>
        /// <param name="suppDateCd">Indicates whether the counts for for QA Cert Event days or Conditional Data Begin hours.</param>
        /// <param name="supplementalDataGroup">The count, date and hour data from which to populate the row.</param>
        /// <param name="workspaceSessionId">The workspace session id for check session.</param>
        private static void LoadSupplementalDataUpdateDataRow(string suppDataCd, string suppDateCd, QaCertificationSupplementalDataGroup supplementalDataGroup, decimal workspaceSessionId)
        {
            DataRow dataRow = SupplementalDataUpdateDataTable.NewRow();

            dataRow["SESSION_ID"] = workspaceSessionId;
            dataRow["QA_CERT_EVENT_ID"] = supplementalDataGroup.QaCertEventId;
            dataRow["QA_CERT_EVENT_SUPP_DATA_CD"] = suppDataCd;
            dataRow["QA_CERT_EVENT_SUPP_DATE_CD"] = suppDateCd;
            dataRow["COUNT_FROM_DATEHOUR"] = supplementalDataGroup.TargetDatehourValue;
            dataRow["COUNT"] = supplementalDataGroup.Count;
            dataRow["COUNT_FROM_INCLUDED_IND"] = supplementalDataGroup.TargetIsInCount;
            dataRow["MON_LOC_ID"] = supplementalDataGroup.MonLocId;
            dataRow["RPT_PERIOD_ID"] = supplementalDataGroup.RptPeriodId;

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

            return this.Equals(that as QaCertificationSupplementalData);
        }

        /// <summary>
        /// Type specific Equals implementation that uses QaCertEventId, MonLocId, RptPeriodId, TargetDatehourCode and TargetDatehourValue.
        /// </summary>
        /// <param name="that">The QaCertficationSupplementalDataClass instance to compare against this instance.</param>
        /// <returns>Returns true if the QaCertEventId, MonLocId, RptPeriodId, TargetDatehourCode and TargetDatehourValue values are the same for the two instances.</returns>
        public bool Equals(QaCertificationSupplementalData that)
        {
            return (that != null) &&
                   this.QaCertEventId.Equals(that.QaCertEventId) &&
                   this.MonLocId.Equals(that.MonLocId) &&
                   this.RptPeriodId.Equals(that.RptPeriodId) &&
                   this.TargetDatehourCode.Equals(that.TargetDatehourCode) &&
                   this.TargetDatehourValue.Equals(that.TargetDatehourValue);
        }

        /// <summary>
        /// GetHashCode override that uses QaCertEventId, MonLocId, RptPeriodId, TargetDatehourCode and TargetDatehourValue.
        /// </summary>
        /// <returns>Returns the hash based on QaCertEventId, MonLocId, RptPeriodId, TargetDatehourCode and TargetDatehourValue.</returns>
        public override int GetHashCode()
        {
            return this.QaCertEventId.GetHashCode() ^ this.MonLocId.GetHashCode() ^ this.RptPeriodId.GetHashCode() ^ this.TargetDatehourCode.GetHashCode() ^ this.TargetDatehourValue.GetHashCode();
        }

        /// <summary>
        /// Returns the QA Cert Event Id
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return QaCertEventId + " | " + MonLocId + ": QCE Hour " + this.TargetDatehourCode.ToString() + "; CDB Hour " + this.TargetDatehourValue.ToString();
        }

        #endregion

    }


    #region Public Types

    /// <summary>
    /// Indicates on which date/hour column in a QA Certification Event row the count is based.
    /// </summary>
    public enum eQaCertificationSupplementalDataTargetDateHour
    {
        /// <summary>
        /// The Conditional Data Begin Hour of the QA Certification Event.
        /// </summary>
        ConditionalDataBeginHour,

        /// <summary>
        /// The Certification Event Date of the QA Certification Event.
        /// </summary>
        QaCertEventDate
    }

    /// <summary>
    /// Indicates on which date/hour column in a QA Certification Event row the count is based
    /// and wether the counts is of days or hours.
    /// </summary>
    public enum eQaCertificationSupplementalDataTimeType
    {
        /// <summary>
        /// Count hours.
        /// </summary>
        Hour,

        /// <summary>
        /// Count days.
        /// </summary>
        Day
    }

    #endregion

}
