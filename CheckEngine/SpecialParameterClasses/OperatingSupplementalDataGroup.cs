using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using ECMPS.Checks.TypeUtilities;


namespace ECMPS.Checks.CheckEngine.SpecialParameterClasses
{

    /// <summary>
    /// This class is used to capture system operating day and hour counts. 
    /// </summary>
    public class OperatingSupplementalDataGroup
    {

        /// <summary>
        /// This class is used to capture system operating day and hour counts. 
        /// </summary>
        /// <param name="parent">The parent  OperatingSupplementalData object.</param>
        /// <param name="timePeriod">Indicates what time period the object represents.</param>
        /// <param name="countType">Indicates what type of count the object represents.</param>
        public OperatingSupplementalDataGroup(OperatingSupplementalData parent, eOperatingSupplementalDataTimePeriod timePeriod, eOperatingSupplementalDataCountType countType)
        {
            this.parent = parent;
            this.timePeriod = timePeriod;
            this.countType = countType;

            Days = 0;
            Hours = 0;

            lastHandledOperatingHour = null;
        }


        /// <summary>
        /// This constructor is used to initialize day and hour count for testing. 
        /// </summary>
        /// <param name="parent">The parent  OperatingSupplementalData object.</param>
        /// <param name="timePeriod">Indicates what time period the object represents.</param>
        /// <param name="countType">Indicates what type of count the object represents.</param>
        /// <param name="days">The initial day count.</param>
        /// <param name="hours">The initial hour count.</param>
        public OperatingSupplementalDataGroup(OperatingSupplementalData parent, eOperatingSupplementalDataTimePeriod timePeriod, eOperatingSupplementalDataCountType countType,
                                                 ushort days, ushort hours)
        {
            this.parent = parent;
            this.timePeriod = timePeriod;
            this.countType = countType;

            Days = days;
            Hours = hours;

            lastHandledOperatingHour = null;
        }


        #region Public Properties and Supporting Fields

        /// <summary>
        /// The parent OperatingSupplementalData object.
        /// </summary>
        public OperatingSupplementalData Parent { get { return parent; } }
        private readonly OperatingSupplementalData parent;

        /// <summary>
        /// Indicates the time period represented by OperatingSupplementalDataGroup values. 
        /// </summary>
        public eOperatingSupplementalDataTimePeriod TimePeriod { get { return timePeriod; } }
        private readonly eOperatingSupplementalDataTimePeriod timePeriod;

        /// <summary>
        /// Indicates type of count represented by OperatingSupplementalDataGroup values. 
        /// </summary>
        public eOperatingSupplementalDataCountType CountType { get { return countType; } }
        private readonly eOperatingSupplementalDataCountType countType;


        /// <summary>
        /// The id under which the supplemental data is being tracked.
        /// </summary>
        public string Id { get { return parent.Id; } }

        /// <summary>
        /// The primary key to the MONITOR_LOCATION table.
        /// </summary>
        public string MonLocId { get { return parent.MonLocId; } }

        /// <summary>
        /// The primary key of the REPORTING_PERIOD table and indicating the quarter of the emission report.
        /// </summary>
        public int RptPeriodId { get { return parent.RptPeriodId; } }

        /// <summary>
        /// The number of operating days in the current quarter for which the system was used to report a value.
        /// </summary>
        public ushort Days { get; private set; }

        /// <summary>
        /// The number of operating hours in the current quarter for which the system was used to report a value.
        /// </summary>
        public ushort Hours { get; private set; }

        /// <summary>
        /// When populated, indicates the last operating hour for which an increament was handled.
        /// </summary>
        private DateTime? lastHandledOperatingHour { get; set; }

        #endregion


        #region Public Methods

        /// <summary>
        /// Increaments Days and Hours.
        /// 
        /// Days is increamented when an increament has not occurred for the date of the current hour.
        /// Hours is increamented when an increament has not occurred for the current hour.
        /// </summary>
        /// <param name="currentOperatingHour">The current operating hour for which the instance will update counts.</param>
        public void IncreamentForCurrentHour(DateTime currentOperatingHour)
        {
            // Only update if lastHandledOperatingHour is null or current hour is after the last lastHandledOperatingHour
            if ((lastHandledOperatingHour == null) || (currentOperatingHour > lastHandledOperatingHour.Value))
            {
                // Only update the day counts if lastHandledOperatingHour is null or current date is after the the date of the last lastHandledOperatingHour
                if ((lastHandledOperatingHour == null) || (currentOperatingHour.Date > lastHandledOperatingHour.Value.Date))
                {
                    Days += 1;
                }

                Hours += 1;

                // Record current hour as the last handled operating hour.
                lastHandledOperatingHour = currentOperatingHour;
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

            return this.Equals(that as SystemOperatingSupplementalData);
        }

        /// <summary>
        /// Type specific Equals implementation that uses Parent, TimePeriod and CountType.
        /// </summary>
        /// <param name="that">The OperatingSupplementalDataGroup instance to compare against this instance.</param>
        /// <returns>Returns true if the Parent, TimePeriod and CountType values are the same for the two instances.</returns>
        public bool Equals(OperatingSupplementalDataGroup that)
        {
            return (that != null) &&
                   this.Parent.Equals(that.Parent) &&
                   this.TimePeriod.Equals(that.TimePeriod) &&
                   this.CountType.Equals(that.CountType);
        }

        /// <summary>
        /// GetHashCode override that uses Parent, TimePeriod and CountType.
        /// </summary>
        /// <returns>Returns the hash based on Parent, TimePeriod and CountType.</returns>
        public override int GetHashCode()
        {
            return this.Parent.GetHashCode() ^ this.TimePeriod.GetHashCode() ^ this.CountType.GetHashCode();
        }

        /// <summary>
        /// Returns the Parent, TimePeriod and CountType.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Parent.ToString() + " ( " + TimePeriod.ToString() + ", " + CountType.ToString() + ") ";
        }

        #endregion

    }


    #region Public Types

    /// <summary>
    /// Indicates the type of count represented by QaCertificationSupplementalDataGroup values.
    /// </summary>
    public enum eOperatingSupplementalDataCountType
    {
        /// <summary>
        /// Indicates that the data group reporesents monitor available counts.
        /// </summary>
        MonitorAvailable,

        /// <summary>
        /// Indicates that the data group represents operating hour counts.
        /// </summary>
        Operating,

        /// <summary>
        /// Indicates that the data group reporesents quality assured hour counts.
        /// </summary>
        QualityAssured
    }

    /// <summary>
    /// Indicates the time period represented by QaCertificationSupplementalDataGroup values.
    /// </summary>
    public enum eOperatingSupplementalDataTimePeriod
    {
        /// <summary>
        /// Indicates that the data group represents values for the whole quarter.
        /// </summary>
        Quarterly,

        /// <summary>
        /// Indicates that the data group reporesents values for May and June for non annual ozone season reporters
        /// </summary>
        MayAndJune
    }

    #endregion

}
