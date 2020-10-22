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
    public class QaCertificationSupplementalDataGroup
    {

        /// <summary>
        /// Tracks an increaments operating and quality assured operating hours for QA Certification Events.
        /// </summary>
        /// <param name="parent">The parent QaCertificationSupplementalData object.</param>
        /// <param name="timePeriod">Indicates what time period the object represents.</param>
        /// <param name="countType">Indicates what type of count the object represents.</param>
        /// <param name="year">The primary key of the QA_CERT_EVENT.</param>
        /// <param name="quarter">The primary key of the QA_CERT_EVENT.</param>
        public QaCertificationSupplementalDataGroup(QaCertificationSupplementalData parent, 
                                                    eQaCertificationSupplementalDataTimePeriod timePeriod, 
                                                    eQaCertificationSupplementalDataCountType countType, 
                                                    int year, int quarter)
        {
            this.parent = parent;
            this.timePeriod = timePeriod;
            this.countType = countType;

            Count = 0;
            TargetIsInCount = false;

            lastHandledOperatingHour = null;
        }


        #region Public Properties and Supporting Fields

        /// <summary>
        /// The parent QaCertificationSupplementalData object.
        /// </summary>
        public QaCertificationSupplementalData Parent { get { return parent; } }
        private readonly QaCertificationSupplementalData parent;

        /// <summary>
        /// Indicates whether the count is based on the QA Cert Event Datehour or Conditiona Data Begin Datehour.
        /// </summary>
        public eQaCertificationSupplementalDataTargetDateHour TargetDatehourCode { get { return parent.TargetDatehourCode; } }

        /// <summary>
        /// The value of the target datehour.
        /// </summary>
        public DateTime TargetDatehourValue { get { return parent.TargetDatehourValue; } }

        /// <summary>
        /// Indicates whether to count days or hours. 
        /// </summary>
        public eQaCertificationSupplementalDataTimeType TimeType { get { return parent.TimeType; } }

        /// <summary>
        /// Indicates the time period represented by QaCertificationSupplementalDataGroup values. 
        /// </summary>
        public eQaCertificationSupplementalDataTimePeriod TimePeriod { get { return timePeriod; } }
        private readonly eQaCertificationSupplementalDataTimePeriod timePeriod;

        /// <summary>
        /// Indicates type of count represented by QaCertificationSupplementalDataGroup values. 
        /// </summary>
        public eQaCertificationSupplementalDataCountType CountType { get { return countType; } }
        private readonly eQaCertificationSupplementalDataCountType countType;

        /// <summary>
        /// The primary key of the QA_CERT_EVENT being tracked.
        /// </summary>
        public string QaCertEventId { get { return parent.QaCertEventId; } }

        /// <summary>
        /// The primary key of the MON_LOC_ID being tracked.
        /// </summary>
        public string MonLocId { get { return parent.MonLocId; } }

        /// <summary>
        /// The primary key of the REPORTING_PERIOD table and indicating the quarter of the emission report.
        /// </summary>
        public int RptPeriodId { get { return parent.RptPeriodId; } }

        /// <summary>
        /// Indicates whether the QA_CERT_EVENT_DATE is in the day count.
        /// </summary>
        public bool TargetIsInCount { get; private set; }

        /// <summary>
        /// The number of counted days in the quarter of the QA_CERT_EVENT_DATE and on or after the date.
        /// </summary>
        public ushort? Count { get; private set; }

        /// <summary>
        /// When populated, indicates the last operating hour for which an increament was handled.
        /// </summary>
        private DateTime? lastHandledOperatingHour { get; set; }

        #endregion


        #region Public Methods

        /// <summary>
        /// Increaments QaCertEventQuarterDays and ConditionalDataBeginQuarterHours.
        /// 
        /// QaCertEventQuarterDays is increamented when the date of the current hour is after the event date, and an increament
        /// has not occurred for the date of the current hour.
        /// 
        /// ConditionalDataBeginQuarterHours is increamented when current hour is after the conditional data peeriod begin hour.
        /// 
        /// QaCertEventDateIsInDayCount and ConditionalDataBeginDatehourIsInHourCount are set to true when their respective
        /// counts are increamented and either the day for QaCertEventDateIsInDayCount or the hour for 
        /// ConditionalDataBeginDatehourIsInHourCount is the current day or hour.
        /// </summary>
        /// <param name="currentOperatingHour">The current operating hour for which the instance will update counts.</param>
        public void IncreamentForCurrentHour(DateTime currentOperatingHour)
        {
            // Only Update if current hour is on or after the target hour.
            if (currentOperatingHour >= TargetDatehourValue)
            {
                switch (TimeType)
                {
                    case eQaCertificationSupplementalDataTimeType.Hour:
                        {
                            // Only update if lastHandledOperatingHour is null or current hour is after the last lastHandledOperatingHour
                            if ((lastHandledOperatingHour == null) || (currentOperatingHour > lastHandledOperatingHour.Value))
                            {
                                Count += 1;

                                // Indicate that the ConditionalDataBeginDatehour is included in the count.
                                if (currentOperatingHour == TargetDatehourValue)
                                    TargetIsInCount = true;

                                // Record current hour as the last handled operating hour.
                                lastHandledOperatingHour = currentOperatingHour;
                            }
                        }
                        break;

                    case eQaCertificationSupplementalDataTimeType.Day:
                        {
                            // Only update if lastHandledOperatingHour is null or current hour is after the last lastHandledOperatingHour
                            if ((lastHandledOperatingHour == null) || (currentOperatingHour.Date > lastHandledOperatingHour.Value.Date))
                            {
                                Count += 1;

                                // Indicate that the ConditionalDataBeginDatehour is included in the count.
                                if (currentOperatingHour.Date == TargetDatehourValue.Date)
                                    TargetIsInCount = true;

                                // Record current hour as the last handled operating hour.
                                lastHandledOperatingHour = currentOperatingHour;
                            }
                            // Increament the last handled hour when in an already handled day, but in a new hour.
                            else if (currentOperatingHour > lastHandledOperatingHour.Value)
                            {
                                // Record current hour as the last handled operating hour.
                                lastHandledOperatingHour = currentOperatingHour;
                            }
                        }
                        break;
                }
            }
        }

        #endregion


        #region Private Methods

        private bool IsNewCurrentOperatingingDate(DateTime currentOperatingHour)
        {
            return (currentOperatingHour.Date > lastHandledOperatingHour.Value.Date);
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

            return this.Equals(that as QaCertificationSupplementalDataGroup);
        }

        /// <summary>
        /// Type specific Equals implementation that uses Parent, TimePeriod and CountType.
        /// </summary>
        /// <param name="that">The QaCertificationSupplementalDataGroup instance to compare against this instance.</param>
        /// <returns>Returns true if the Parent, TimePeriod and CountType values are the same for the two instances.</returns>
        public bool Equals(QaCertificationSupplementalDataGroup that)
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
        /// Returns the Parent, TargetDateHour, TimeType, TimePeriod and CountType
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
    public enum eQaCertificationSupplementalDataCountType
    {
        /// <summary>
        /// Indicates that the data group represents operating hour counts.
        /// </summary>
        Operating,

        /// <summary>
        /// Indicates that the data group reporesents system operating hour counts.
        /// </summary>
        SystemOperating,

        /// <summary>
        /// Indicates that the data group reporesents component operating hour counts.
        /// </summary>
        ComponentOperating,

        /// <summary>
        /// Indicates that the data group reporesents system quality assured hour counts.
        /// </summary>
        SystemQualityAssured,

        /// <summary>
        /// Indicates that the data group reporesents component quality assured hour counts.
        /// </summary>
        ComponentQualityAssured
    }

    /// <summary>
    /// Indicates the time period represented by QaCertificationSupplementalDataGroup values.
    /// </summary>
    public enum eQaCertificationSupplementalDataTimePeriod
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
