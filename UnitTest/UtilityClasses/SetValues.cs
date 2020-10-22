using System;

using ECMPS.Checks.Data.Ecmps.CheckEm.Function;
using ECMPS.Checks.Data.Ecmps.CheckMp.Function;
using ECMPS.Checks.Data.Ecmps.Dbo.Table;
using ECMPS.Checks.Data.Ecmps.Dbo.View;

namespace UnitTest.UtilityClasses
{
    public static class SetValues
    {
        /// <summary>
        /// Sets Begin and end information for CombinedMethods.
        /// </summary>
        /// <param name="record">The record to update.</param>
        /// <param name="beginDateHour">The begin value used to set the begin information.</param>
        /// <param name="endDateHour">The end value used to set the end information.</param>
        /// <returns></returns>
        public static CombinedMethods DateHour(CombinedMethods record, DateTime? beginDateHour, DateTime? endDateHour)
        {
            if (record != null)
            {
                if (beginDateHour.HasValue)
                {
                    record.BeginDatehour = beginDateHour;
                    record.BeginDate = beginDateHour.Value.Date;
                    record.BeginHour = beginDateHour.Value.Hour;
                }

                if (endDateHour.HasValue)
                {
                    record.EndDatehour = endDateHour;
                    record.EndDate = endDateHour.Value.Date;
                    record.EndHour = endDateHour.Value.Hour;
                }
            }

            return record;
        }

        /// <summary>
        /// Sets Begin and end information for MatsSorbentTrapRecord.
        /// </summary>
        /// <param name="record">The record to update.</param>
        /// <param name="beginDateHour">The begin value used to set the begin information.</param>
        /// <param name="endDateHour">The end value used to set the end information.</param>
        /// <returns></returns>
        public static MatsSorbentTrapRecord DateHour(MatsSorbentTrapRecord record, DateTime? beginDateHour, DateTime? endDateHour)
        {
            if (record != null)
            {
                if (beginDateHour.HasValue)
                {
                    record.BeginDatehour = beginDateHour;
                    record.BeginDate = beginDateHour.Value.Date;
                    record.BeginHour = beginDateHour.Value.Hour;
                }

                if (endDateHour.HasValue)
                {
                    record.EndDatehour = endDateHour;
                    record.EndDate = endDateHour.Value.Date;
                    record.EndHour = endDateHour.Value.Hour;
                }
            }

            return record;
        }

        /// <summary>
        /// Sets Begin and end information for MatsSorbentTrapRecord.
        /// </summary>
        /// <param name="record">The record to update.</param>
        /// <param name="beginDateHour">The begin value used to set the begin information.</param>
        /// <param name="endDateHour">The end value used to set the end information.</param>
        /// <returns></returns>
        public static MatsSorbentTrapSupplementalDataRecord DateHour(MatsSorbentTrapSupplementalDataRecord record, DateTime? beginDateHour, DateTime? endDateHour)
        {
            if (record != null)
            {
                if (beginDateHour.HasValue)
                {
                    record.BeginDatehour = beginDateHour;
                    record.BeginDate = beginDateHour.Value.Date;
                    record.BeginHour = beginDateHour.Value.Hour;
                }

                if (endDateHour.HasValue)
                {
                    record.EndDatehour = endDateHour;
                    record.EndDate = endDateHour.Value.Date;
                    record.EndHour = endDateHour.Value.Hour;
                }
            }

            return record;
        }

        /// <summary>
        /// Sets Begin and end information for MatsSorbentTrapRecord.
        /// </summary>
        /// <param name="record">The record to update.</param>
        /// <param name="beginDateHour">The begin value used to set the begin information.</param>
        /// <param name="endDateHour">The end value used to set the end information.</param>
        /// <returns></returns>
        public static VwMonitorLoadRow DateHour(VwMonitorLoadRow record, DateTime? beginDateHour, DateTime? endDateHour)
        {
            if (record != null)
            {
                if (beginDateHour.HasValue)
                {
                    record.BeginDate = beginDateHour.Value.Date;
                    record.BeginHour = beginDateHour.Value.Hour;
                }

                if (endDateHour.HasValue)
                {
                    record.EndDate = endDateHour.Value.Date;
                    record.EndHour = endDateHour.Value.Hour;
                }
            }

            return record;
        }

        /// <summary>
        /// Sets Begin and end information for VwMonitorMethodRow.
        /// </summary>
        /// <param name="record">The record to update.</param>
        /// <param name="beginDateHour">The begin value used to set the begin information.</param>
        /// <param name="endDateHour">The end value used to set the end information.</param>
        /// <returns></returns>
        public static VwMonitorMethodRow DateHour(VwMonitorMethodRow record, DateTime? beginDateHour, DateTime? endDateHour)
        {
            if (record != null)
            {
                if (beginDateHour.HasValue)
                {
                    record.BeginDatehour = beginDateHour;
                    record.BeginDate = beginDateHour.Value.Date;
                    record.BeginHour = beginDateHour.Value.Hour;
                }

                if (endDateHour.HasValue)
                {
                    record.EndDatehour = endDateHour;
                    record.EndDate = endDateHour.Value.Date;
                    record.EndHour = endDateHour.Value.Hour;
                }
            }

            return record;
        }

        /// <summary>
        /// Sets Begin and end information for VwMpMonitorDefaultRow.
        /// </summary>
        /// <param name="record">The record to update.</param>
        /// <param name="beginDateHour">The begin value used to set the begin information.</param>
        /// <param name="endDateHour">The end value used to set the end information.</param>
        /// <returns></returns>
        public static VwMonitorDefaultRow DateHour(VwMonitorDefaultRow record, DateTime? beginDateHour, DateTime? endDateHour)
        {
            if (record != null)
            {
                if (beginDateHour.HasValue)
                {
                    record.BeginDatehour = beginDateHour;
                    record.BeginDate = beginDateHour.Value.Date;
                    record.BeginHour = beginDateHour.Value.Hour;
                }

                if (endDateHour.HasValue)
                {
                    record.EndDatehour = endDateHour;
                    record.EndDate = endDateHour.Value.Date;
                    record.EndHour = endDateHour.Value.Hour;
                }
            }

            return record;
        }

        /// <summary>
        /// Sets Begin and end information for VwQaRataSummaryRow.
        /// </summary>
        /// <param name="record">The record to update.</param>
        /// <param name="beginDateHour">The begin value used to set the begin information.</param>
        /// <param name="endDateHour">The end value used to set the end information.</param>
        /// <returns></returns>
        public static VwQaTestSummaryRataRow DateHour(VwQaTestSummaryRataRow record, DateTime? beginDateHour, DateTime? endDateHour)
        {
            if (record != null)
            {
                if (beginDateHour.HasValue)
                {
                    record.BeginDate = beginDateHour.Value.Date;
                    record.BeginHour = beginDateHour.Value.Hour;
                }

                if (endDateHour.HasValue)
                {
                    record.EndDate = endDateHour.Value.Date;
                    record.EndHour = endDateHour.Value.Hour;
                }
            }

            return record;
        }

    }
}
