using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using ECMPS.Checks.TypeUtilities;


namespace ECMPS.Checks.CheckEngine.SpecialParameterClasses
{

    /// <summary>
    /// This class is used to capture component operating day and hour counts. 
    /// </summary>
    public class ComponentOperatingSupplementalData : OperatingSupplementalData
    {

        /// <summary>
        /// This class is used to capture component operating day and hour counts. 
        /// </summary>
        /// <param name="rptPeriodId">The primary key of the REPORTING_PERIOD table and indicating the quarter of the emission report.</param>
        /// <param name="componentId">The primary key of the COMPONENT table.</param>
        /// <param name="monLocId">The primary key of the MONITOR_LOCATION table.</param>
        public ComponentOperatingSupplementalData(int rptPeriodId, string componentId, string monLocId)
            : base(rptPeriodId, componentId, monLocId)
        {
        }


        #region Public Properties and Supporting Fields

        /// <summary>
        /// The primary key of the COMPONENT being tracked.
        /// </summary>
        public string ComponentId { get { return Id; } } 

        #endregion


        #region Static Properties and Methods

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
        public static string SupplementalDataUpdateTableName { get { return "CE_ComponentOpSuppData"; } }

        /// <summary>
        /// Contains the name of the update table for sampling trian supplemental data.
        /// </summary>
        public static string SupplementalDataUpdateTablePath { get { return SupplementalDataUpdateDatabaseName + "." + SupplementalDataUpdateSchemaName + "." + SupplementalDataUpdateTableName; } }


        /// <summary>
        /// Creates a new instance of SupplementalDataUpdateDataTable and populates it with data from the passed dictionary array.  
        /// </summary>
        public static void LoadSupplementalDataUpdateDataTable(Dictionary<string, ComponentOperatingSupplementalData>[] supplementalDataDictionaryArray, decimal workspaceSessionId, SqlConnection connection)
        {
            SupplementalDataUpdateDataTable = cDataFunctions.CreateDataTable(SupplementalDataUpdateDatabaseName, SupplementalDataUpdateSchemaName, SupplementalDataUpdateTableName, connection);

            if (SupplementalDataUpdateDataTable != null)
            {
                foreach (Dictionary<string, ComponentOperatingSupplementalData> supplementalDataDictionary in supplementalDataDictionaryArray)
                {
                    foreach (OperatingSupplementalData supplementalData in supplementalDataDictionary.Values)
                    {
                        LoadSupplementalDataUpdateDataDoRow(supplementalData, "COMPONENT_ID", SupplementalDataUpdateDataTable, workspaceSessionId);
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

            return this.Equals(that as ComponentOperatingSupplementalData);
        }

        /// <summary>
        /// Type specific Equals implementation that uses ComponentId and RptPeriodId.
        /// </summary>
        /// <param name="that">The ComponentOperatingSupplementalData instance to compare against this instance.</param>
        /// <returns>Returns true if the ComponentId and RptPeriodId values are the same for the two instances.</returns>
        public bool Equals(ComponentOperatingSupplementalData that)
        {
            return (that != null) &&
                   this.ComponentId.Equals(that.ComponentId) &&
                   this.RptPeriodId.Equals(that.RptPeriodId);
        }

        /// <summary>
        /// GetHashCode override that uses ComponentId and RptPeriodId.
        /// </summary>
        /// <returns>Returns the hash based on ComponentId and RptPeriodId.</returns>
        public override int GetHashCode()
        {
            return this.ComponentId.GetHashCode() ^ this.RptPeriodId.GetHashCode();
        }

        /// <summary>
        /// Returns the Component and Reporting Period Ids
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ComponentId + " " + RptPeriodId.ToString();
        }

        #endregion

    }

}
