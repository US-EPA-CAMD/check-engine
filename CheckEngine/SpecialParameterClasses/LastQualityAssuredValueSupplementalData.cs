using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Extensions;


namespace ECMPS.Checks.CheckEngine.SpecialParameterClasses
{

    /// <summary>
    /// This class is used to capture last quality assured values for specific derived/monitored parameters and systems.
    /// </summary>
    public class LastQualityAssuredValueSupplementalData
    {

        /// <summary>
        /// This class is used to capture system operating day and hour counts. 
        /// </summary>
        /// <param name="rptPeriodId">The primary key of the REPORTING_PERIOD table and indicating the quarter of the emission report.</param>
        /// <param name="monLocId">The primary key of the MONITOR_LOCATION table.</param>
        /// <param name="parameterCd">The parameter of the hourly data.</param>
        /// <param name="moistureBasis">The moisture basis for O2 concentration parameters.</param>
        /// <param name="hourlyType">Indicates the type of hourly data, derived or monitored.</param>
        /// <param name="monSysId">The primary key of the MONITOR_SYSTEM table.</param>
        /// <param name="componentId">The primary key of the COMPONENT table.</param>
        public LastQualityAssuredValueSupplementalData(int rptPeriodId, string monLocId, string parameterCd, string moistureBasis, eHourlyType hourlyType, string monSysId, string componentId)
        {
            if (monLocId == null)
            {
                throw new System.ArgumentException("Parameter cannot be null", "monLocId");
            }

            if (parameterCd == null)
            {
                throw new System.ArgumentException("Parameter cannot be null", "parameterCd");
            }

            this.rptPeriodId = rptPeriodId;
            this.monLocId = monLocId;
            this.parameterCd = parameterCd;
            this.moistureBasis = moistureBasis;
            this.hourlyType = hourlyType;
            this.monSysId = monSysId;
            this.componentId = componentId;

            OpDateHour = null;
            UnadjustedHourlyValue = null;
            AdjustedHourlyValue = null;
        }


        #region Public Properties and Supporting Fields

        /// <summary>
        /// The primary key of the REPORTING_PERIOD table and indicating the quarter of the emission report.
        /// </summary>
        public int RptPeriodId { get { return rptPeriodId; } }
        private readonly int rptPeriodId;

        /// <summary>
        /// The primary key of the MONITOR_LOCATION being tracked.
        /// </summary>
        public string MonLocId { get { return monLocId; } }
        private readonly string monLocId;

        /// <summary>
        /// The primary key of the PARAMETER_CODE being tracked.
        /// </summary>
        public string ParameterCd { get { return parameterCd; } }
        private readonly string parameterCd;

        /// <summary>
        /// The moisture bases for O2 concentrations.
        /// </summary>
        public string MoistureBasis { get { return moistureBasis; } }
        private readonly string moistureBasis;

        /// <summary>
        /// The primary key of the QA_CERT_EVENT being tracked.
        /// </summary>
        public eHourlyType HourlyType { get { return hourlyType; } }
        private readonly eHourlyType hourlyType;

        /// <summary>
        /// The primary key of the MONITOR_SYSTEM being tracked.
        /// </summary>
        public string MonSysId { get { return monSysId; } }
        private readonly string monSysId;

        /// <summary>
        /// The primary key of the COMPONENT being tracked.
        /// </summary>
        public string ComponentId { get { return componentId; } }
        private readonly string componentId;

        /// <summary>
        /// The hour of the last quality assured hour.
        /// </summary>
        public DateTime? OpDateHour { get; private set; }

        /// <summary>
        /// The last quality assured unadjusted hourly value.
        /// </summary>
        public decimal? UnadjustedHourlyValue { get; private set; }

        /// <summary>
        /// The last quality assured adjusted hourly value.
        /// </summary>
        public decimal? AdjustedHourlyValue { get; private set; }

        /// <summary>
        /// When populated, indicates the last operating hour for which an increament was handled.
        /// </summary>
        private DateTime? lastHandledOperatingHour { get; set; }

        #endregion


        #region Public Methods

        /// <summary>
        /// Updates the last quality assured hour if the current operating hour is after the last handled operating hour
        /// and the MODC indicates a quality assured hour.
        /// </summary>
        /// <param name="currentOperatingHour">The current operating hour for which the instance will update counts.</param>
        /// <param name="modcCd">The MODC code for the hourly record.</param>
        /// <param name="unadjustedHourlyValue">The unadjusted hourly value for the current hour and parameters (and system).</param>
        /// <param name="adjustedHourlyValue">The adjusted hourly value for the current hour and parameters (and system).</param>
        public void UpdateLastQualityAssuredValue(DateTime currentOperatingHour, string modcCd, decimal? unadjustedHourlyValue, decimal? adjustedHourlyValue)
        {
            if (modcCd.InList(QualityAssuredModcList))
            {
                // Only update if lastHandledOperatingHour is null or current hour is after the last lastHandledOperatingHour
                if ((lastHandledOperatingHour == null) || (currentOperatingHour > lastHandledOperatingHour.Value))
                {
                    OpDateHour = currentOperatingHour;
                    UnadjustedHourlyValue = unadjustedHourlyValue;
                    AdjustedHourlyValue = adjustedHourlyValue;

                    // Record current hour as the last handled operating hour.
                    lastHandledOperatingHour = currentOperatingHour;
                }
            }
        }

        #endregion


        #region Static Properties and Methods

        /// <summary>
        /// Returns the formated logical key based on the passed values.
        /// </summary>
        /// <param name="rptPeriodId">The reporting period key for the supplemental data.</param>
        /// <param name="monLocId">The monitor location key for the supplemental data.</param>
        /// <param name="parameterCd">The parameter code for the supplemental data.</param>
        /// <param name="moistureBasis">The moisture basis, D for dry and W for wet, for O2 concentraion supplemental data.</param>
        /// <param name="hourlyType">Determine whether the hourly source is monitored or derived.</param>
        /// <param name="monSysId">The monitor system key for the supplemental data.</param>
        /// <param name="componentId">The component key for the supplemental data.</param>
        /// <returns>The formatted locical key based on the logical key values for the supplemental data.</returns>
        public static string FormatKey(int rptPeriodId, string monLocId, string parameterCd, string moistureBasis, eHourlyType hourlyType, string monSysId, string componentId)
        {
            return monLocId + "|" + rptPeriodId.ToString() + "|" + parameterCd + "|" + moistureBasis + "|" + hourlyType.ToString() + "|" + monSysId + "|" + componentId;
        }

        /// <summary>
        /// The list of MODC for quality assured hours.
        /// </summary>
        public static string QualityAssuredModcList { get { return "01,02,03,04,14,16,17,19,20,21,22,47,53,54"; } }


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
        public static string SupplementalDataUpdateTableName { get { return "CE_LastQaValueSuppData"; } }

        /// <summary>
        /// Contains the name of the update table for sampling trian supplemental data.
        /// </summary>
        public static string SupplementalDataUpdateTablePath { get { return SupplementalDataUpdateDatabaseName + "." + SupplementalDataUpdateSchemaName + "." + SupplementalDataUpdateTableName; } }


        /// <summary>
        /// Creates a new instance of SupplementalDataUpdateDataTable and populates it with data from the passed dictionary array.  
        /// </summary>
        /// <param name="supplementalDataDictionaryArray">Dictionary containing data used to update the table.</param>
        /// <param name="workspaceSessionId">The workspace session id for check session.</param>
        /// <param name="connection">Database connection to use when creating the internal supplemental data table.</param>
        public static void LoadSupplementalDataUpdateDataTable(Dictionary<string, LastQualityAssuredValueSupplementalData>[] supplementalDataDictionaryArray, decimal workspaceSessionId, SqlConnection connection)
        {
            SupplementalDataUpdateDataTable = cDataFunctions.CreateDataTable(SupplementalDataUpdateDatabaseName, SupplementalDataUpdateSchemaName, SupplementalDataUpdateTableName, connection);

            if (SupplementalDataUpdateDataTable != null)
            {
                foreach (Dictionary<string, LastQualityAssuredValueSupplementalData> supplementalDataDictionary in supplementalDataDictionaryArray)
                {
                    foreach (LastQualityAssuredValueSupplementalData supplementalData in supplementalDataDictionary.Values)
                    {
                        DataRow dataRow = SupplementalDataUpdateDataTable.NewRow();

                        dataRow["SESSION_ID"] = workspaceSessionId;
                        dataRow["MON_LOC_ID"] = supplementalData.MonLocId;
                        dataRow["RPT_PERIOD_ID"] = supplementalData.RptPeriodId;
                        dataRow["PARAMETER_CD"] = supplementalData.ParameterCd;
                        dataRow["MOISTURE_BASIS"] = supplementalData.MoistureBasis;
                        dataRow["HOURLY_TYPE_CD"] = supplementalData.HourlyType.ToString().ToUpper();
                        dataRow["MON_SYS_ID"] = supplementalData.MonSysId;
                        dataRow["COMPONENT_ID"] = supplementalData.ComponentId;
                        dataRow["OP_DATEHOUR"] = supplementalData.OpDateHour;

                        if (supplementalData.UnadjustedHourlyValue.HasValue)
                            dataRow["UNADJUSTED_HRLY_VALUE"] = supplementalData.UnadjustedHourlyValue.Value;
                        else
                            dataRow["UNADJUSTED_HRLY_VALUE"] = DBNull.Value;

                        if (supplementalData.AdjustedHourlyValue.HasValue)
                            dataRow["ADJUSTED_HRLY_VALUE"] = supplementalData.AdjustedHourlyValue.Value;
                        else
                            dataRow["ADJUSTED_HRLY_VALUE"] = DBNull.Value;

                        SupplementalDataUpdateDataTable.Rows.Add(dataRow);
                    }
                }
            }
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

            return this.Equals(that as LastQualityAssuredValueSupplementalData);
        }

        /// <summary>
        /// Type specific Equals implementation that uses RptPeriodId, HourlyType, ParameterCd and MonSysId.
        /// </summary>
        /// <param name="that">The LastQualityAssuredValueSupplementalData instance to compare against this instance.</param>
        /// <returns>Returns true if the RptPeriodId, HourlyType, ParameterCd and MonSysId values are the same for the two instances.</returns>
        public bool Equals(LastQualityAssuredValueSupplementalData that)
        {
            return (that != null) &&
                   (this.MonLocId == that.MonLocId) &&
                   (this.RptPeriodId == that.RptPeriodId) &&
                   (this.ParameterCd == that.ParameterCd) &&
                   (this.MoistureBasis == that.MoistureBasis) &&
                   (this.HourlyType == that.HourlyType) &&
                   (this.MonSysId == that.MonSysId) &&
                   (this.ComponentId == that.ComponentId);
        }

        /// <summary>
        /// GetHashCode override that uses RptPeriodId, HourlyType, ParameterCd and MonSysId.
        /// </summary>
        /// <returns>Returns the hash based on RptPeriodId, HourlyType, ParameterCd and MonSysId.</returns>
        public override int GetHashCode()
        {
            return (this.MonLocId == null ? "" : this.MonLocId).GetHashCode() ^
                   this.RptPeriodId.GetHashCode() ^ this.ParameterCd.GetHashCode() ^
                   (this.ParameterCd == null ? "" : this.ParameterCd).GetHashCode() ^
                   (this.MoistureBasis == null ? "" : this.MoistureBasis).GetHashCode() ^
                   this.HourlyType.GetHashCode() ^
                   (this.MonSysId == null ? "" : this.MonSysId).GetHashCode() ^
                   (this.ComponentId == null ? "" : this.ComponentId).GetHashCode();
        }

        /// <summary>
        /// Returns the Hourly Type, Parameter, MON_SYS_ID, RPT_PERIOD_ID and Value.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.HourlyType.ToString() + " " + this.ParameterCd + " '" + this.MonSysId + " " + this.RptPeriodId.ToString() + " - Unadjusted: " + this.UnadjustedHourlyValue.ToString() + " Adjusted: " + this.AdjustedHourlyValue.ToString();
        }

        #endregion

    }



    #region Public Types

    /// <summary>
    /// Indicates the type of hourly data, derived or monitored.
    /// </summary>
    public enum eHourlyType
    {
        /// <summary>
        /// Derived Hourly Value
        /// </summary>
        Derived,

        /// <summary>
        /// Monitored Hourly Value
        /// </summary>
        Monitor
    }

    #endregion

}
