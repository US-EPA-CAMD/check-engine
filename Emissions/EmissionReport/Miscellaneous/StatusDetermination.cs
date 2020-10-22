using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Data.Ecmps.Dbo.View;
using ECMPS.Checks.Em.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Enumerations;
using ECMPS.Definitions.Extensions;


namespace ECMPS.Checks.EmissionsReport
{
    static public class StatusDetermination
    {

        /// <summary>
        /// Values representing the various results of a conditional data status determination.
        /// </summary>
        public enum eConditionalDataStatus
        {
            /// <summary>
            /// Conditional data period has expired
            /// </summary>
            EXPIRED,
            /// <summary>
            /// Accumulator array indicates that an error occurred that prevented an accurate count.
            /// </summary>
            MISSINGACCUM,
            /// <summary>
            /// An operating supplemental data rocord needed in a count is missing.
            /// </summary>
            MISSINGOPSUPP,
            /// <summary>
            /// An expected program record is missing.
            /// </summary>
            MISSINGPROGRAM,
            /// <summary>
            /// Indicates that a required check parameter was missing.
            /// </summary>
            MISSINGVALUE,
            /// <summary>
            /// The combination of operating hours in the quarter of the event, and the begin hour of the conditonal data period prevented a absolute determiniation of the status.
            /// </summary>
            UNDETERMINED,
            /// <summary>
            /// The conditional data period is currently valid.
            /// </summary>
            VALID
        }

        /// <summary>
        /// Based on the passed conditional data period begin date and the LocationProgramRecordsByHourLocation and CurrentDateHour,
        /// this function returns the conditional data status for an initial certification (Event 125).
        /// 
        /// The check harnesses the setting of program dates instead of counting operating days.  The row filtering ensures that the
        /// logic only uses program rows where the Unit Monitor Cert Begin (UMCB) Date is not null.
        /// 
        /// Logic:
        /// 
        /// When CertEventRecord.QaCertEventCode is equal to “125”:
        /// 
        ///     1) If CertEventRecord.ConditionalBeginDateHour is null, set ConditionalDataStatus to EXPIRED.
        ///     2) Else locate a record in LocationProgramRecordsByHourLocation with the latest UnitMonitorCertBeginDate where ProgramCode equals “MATS” and UnitMonitorCertBeginDate is on or before QaStatusComponentBeginDate.
        ///     3) If not found, locate a record in LocationProgramRecordsByHourLocation with the latest UnitMonitorCertBeginDate where ProgramCode equals “MATS” and EmissionsRecordingBeginDate on or before QaStatusComponentBeginDate.
        ///     4) If a LocationProgramRecordsByHourLocation was not located, set ConditionalDataStatus to MISSINGPROGRAM.
        ///     5) Else if UnitMonitorCertDeadline of the located record is NOT null, AND is on or prior to the date of CurrentDateHour, set ConditionalDataStatus to EXPIRED.
        ///     6) Else if UnitMonitorCertDeadline of the located record is null, AND UnitMonitorCertBeginDate + 180 is on or prior to the date of CurrentDateHour, set ConditionalDataStatus to EXPIRED.
        ///     7) Otherwise set ConditionalDataStatus to VALID.
        /// 
        /// </summary>
        /// <returns>Returns VALID if the conditional data period is in affect, EXPIRED if it has expired, and MISSINGPROGRAM if a MATS program was not active during conditional data period.</returns>
        static public eConditionalDataStatus ConditionalDataEvent125ForMats()
        {
            eConditionalDataStatus result;

            if ((EmParameters.CurrentDateHour == null) || (EmParameters.LocationProgramRecordsByHourLocation == null))
            {
                result = eConditionalDataStatus.MISSINGVALUE; // This should never happen in a legitimate call.
            }
            else if (EmParameters.QaStatusComponentBeginDate == null)
            {
                result = eConditionalDataStatus.EXPIRED;
            }
            else
            {
                VwMpLocationProgramRow locationProgramRow;
                {
                    locationProgramRow
                        = EmParameters.LocationProgramRecordsByHourLocation.FindMostRecentRow
                        (
                            EmParameters.QaStatusComponentBeginDate.Value.Date.AddDays(1),
                            "UNIT_MONITOR_CERT_BEGIN_DATE",
                            new cFilterCondition[] { new cFilterCondition("PRG_CD", "MATS") }
                        );

                    if (locationProgramRow == null)
                    {
                        CheckDataView<VwMpLocationProgramRow> locationProgramView
                            = EmParameters.LocationProgramRecordsByHourLocation.FindRows
                            (
                                new cFilterCondition("PRG_CD", "MATS"),
                                new cFilterCondition("EMISSIONS_RECORDING_BEGIN_DATE", eFilterConditionRelativeCompare.LessThanOrEqual, EmParameters.QaStatusComponentBeginDate.Value, eNullDateDefault.Max)
                            );

                        if (locationProgramView.Count > 0)
                        {
                            locationProgramRow = locationProgramView[0];

                            for (int dex = 1; dex < locationProgramView.Count; dex++)
                                if (locationProgramView[dex].UnitMonitorCertBeginDate > locationProgramRow.UnitMonitorCertBeginDate)
                                    locationProgramRow = locationProgramView[dex];
                        }
                    }
                }

                if (locationProgramRow == null)
                    result = eConditionalDataStatus.MISSINGPROGRAM;
                else if ((locationProgramRow.UnitMonitorCertDeadline != null) && (locationProgramRow.UnitMonitorCertDeadline <= EmParameters.CurrentDateHour.Value.Date))
                    result = eConditionalDataStatus.EXPIRED;
                else if ((locationProgramRow.UnitMonitorCertDeadline == null) && (locationProgramRow.UnitMonitorCertBeginDate.Value.AddDays(180) <= EmParameters.CurrentDateHour.Value.Date))
                    result = eConditionalDataStatus.EXPIRED;
                else
                    result = eConditionalDataStatus.VALID;
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="conditionalDataPeriodBeginDateHour"></param>
        /// <param name="supplementalDataInd"></param>
        /// <param name="supplementalDataOpHourCount"></param>
        /// <returns></returns>
        static public eConditionalDataStatus ConditionalDataEvent120ForMats(DateTime? conditionalDataPeriodBeginDateHour, int? supplementalDataInd, int? supplementalDataOpHourCount)
        {
            eConditionalDataStatus result;

            if (EmParameters.CurrentDateHour == null)
            {
                result = eConditionalDataStatus.MISSINGVALUE; // This should never happen in a legitimate call.
            }
            else if (conditionalDataPeriodBeginDateHour == null)
            {
                result = eConditionalDataStatus.EXPIRED;
            }
            else if ((cDateFunctions.HourDifference(conditionalDataPeriodBeginDateHour.Value, EmParameters.CurrentDateHour.Value) + 1) <= 168) //Include compare dates
            {
                result = eConditionalDataStatus.VALID;
            }
            else if (conditionalDataPeriodBeginDateHour.SameQuarter(EmParameters.CurrentDateHour))
            {
                if (EmParameters.HourlyOperatingDataRecordsForLocation != null)
                {
                    int count
                        = EmParameters.HourlyOperatingDataRecordsForLocation.CountRows
                        (
                            new cFilterCondition[]
                            {
                                new cFilterCondition("OP_TIME", 0, eFilterDataType.Decimal, eFilterConditionRelativeCompare.GreaterThan),
                                new cFilterCondition("BEGIN_DATEHOUR", eFilterConditionRelativeCompare.GreaterThanOrEqual, conditionalDataPeriodBeginDateHour.Value),
                                new cFilterCondition("BEGIN_DATEHOUR", eFilterConditionRelativeCompare.LessThanOrEqual, EmParameters.CurrentDateHour.Value)
                            }
                        );

                    if (count > 168)
                        result = eConditionalDataStatus.EXPIRED;
                    else
                        result = eConditionalDataStatus.VALID;
                }
                else
                    result = eConditionalDataStatus.MISSINGVALUE; // This should never happen in a legitimate call.
            }
            else
            {
                /* Grab the operating hours for the current quarter on or before the current hour */
                int opeartingHoursCurrentQuarter = 0;
                bool opeartingHoursCurrentQuarterValid = false;
                {
                    int[] rptPeriodOpHoursAccumulatorArray = (int[])EmParameters.GetCheckParameter("Rpt_Period_Op_Hours_Accumulator_Array");

                    if ((EmParameters.CurrentMonitorPlanLocationPostion != null) && (rptPeriodOpHoursAccumulatorArray != null) &&
                        (rptPeriodOpHoursAccumulatorArray[EmParameters.CurrentMonitorPlanLocationPostion.Value] != -1))
                    {
                        opeartingHoursCurrentQuarter = rptPeriodOpHoursAccumulatorArray[EmParameters.CurrentMonitorPlanLocationPostion.Value];
                        opeartingHoursCurrentQuarterValid = true;
                    }
                }

                if (opeartingHoursCurrentQuarter > 168)
                {
                    result = eConditionalDataStatus.EXPIRED;
                }
                else
                {
                    /* Find sum of Op Hours for supplemental record between the quarter of the event quarter and the current quarter */
                    int operatingHoursBetweenQuarters = 0;
                    bool operatingHoursBetweenQuartersValid = true;
                    {
                        int quartersBetweenCount = Math.Max(EmParameters.CurrentDateHour.Value.QuarterOrd() - conditionalDataPeriodBeginDateHour.Value.QuarterOrd() - 1, 0);

                        if (EmParameters.OperatingSuppDataRecordsByLocation != null)
                        {
                            CheckDataView<VwMpOpSuppDataRow> opSuppDataView
                                = EmParameters.OperatingSuppDataRecordsByLocation.FindRows
                                (
                                    new cFilterCondition("OP_TYPE_CD", "OPHOURS"),
                                    new cFilterCondition("FUEL_CD", null),
                                    new cFilterCondition("QUARTER_ORD", eFilterConditionRelativeCompare.GreaterThan, conditionalDataPeriodBeginDateHour.Value.QuarterOrd()),
                                    new cFilterCondition("QUARTER_ORD", eFilterConditionRelativeCompare.LessThan, EmParameters.CurrentDateHour.Value.QuarterOrd())
                                );

                            if (opSuppDataView.Count != quartersBetweenCount)
                                operatingHoursBetweenQuartersValid = false;

                            foreach (VwMpOpSuppDataRow opSuppDataRow in opSuppDataView)
                            {
                                if (opSuppDataRow.OpValue.HasValue)
                                    try
                                    {
                                        operatingHoursBetweenQuarters += (int)opSuppDataRow.OpValue.Value;
                                    }
                                    catch
                                    {
                                        operatingHoursBetweenQuartersValid = false;
                                    }
                                else
                                    operatingHoursBetweenQuartersValid = false;
                            }
                        }
                        else
                            operatingHoursBetweenQuartersValid = false;
                    }

                    /* Determine whether the operating hours for the current and ‘between’ quarters exceed the allowed */
                    if ((opeartingHoursCurrentQuarter + operatingHoursBetweenQuarters) > 168)
                    {
                        result = eConditionalDataStatus.EXPIRED;
                    }
                    /* Stop checking if subsequent checks are affected by missing data */
                    else if (!opeartingHoursCurrentQuarterValid)
                    {
                        result = eConditionalDataStatus.MISSINGACCUM;
                    }
                    else if (!operatingHoursBetweenQuartersValid)
                    {
                        result = eConditionalDataStatus.MISSINGOPSUPP;
                    }
                    /* Use QA Cert Event Supplemental Data for Conditional Begin Date if it exists */
                    else if ((supplementalDataInd == 1) && supplementalDataOpHourCount.HasValue)
                    {
                        if ((opeartingHoursCurrentQuarter + operatingHoursBetweenQuarters + supplementalDataOpHourCount) <= 168)
                        {
                            result = eConditionalDataStatus.VALID;
                        }
                        else
                        {
                            result = eConditionalDataStatus.EXPIRED;
                        }
                    }
                    else
                    {
                        /* Find Op Hours supplemental record for the quarter of the event */
                        int operatingHoursEventQuarter;
                        bool operatingHoursEventQuarterValid;
                        DateTime? eventQuarterBeginDate;
                        DateTime? eventQuarterEndDate;
                        {
                            if (EmParameters.OperatingSuppDataRecordsByLocation != null)
                            {
                                VwMpOpSuppDataRow opSuppDataRow
                                = EmParameters.OperatingSuppDataRecordsByLocation.FindRow
                                (
                                    new cFilterCondition("OP_TYPE_CD", "OPHOURS"),
                                    new cFilterCondition("FUEL_CD", null),
                                    new cFilterCondition("QUARTER_ORD", eFilterConditionRelativeCompare.Equals, conditionalDataPeriodBeginDateHour.Value.QuarterOrd())
                                );

                                if (opSuppDataRow != null)
                                    try
                                    {
                                        operatingHoursEventQuarter = (int)opSuppDataRow.OpValue.Value;
                                        operatingHoursEventQuarterValid = true;
                                        eventQuarterBeginDate = opSuppDataRow.QuarterBeginDate;
                                        eventQuarterEndDate = opSuppDataRow.QuarterEndDate;
                                    }
                                    catch
                                    {
                                        operatingHoursEventQuarter = 0;
                                        operatingHoursEventQuarterValid = false;
                                        eventQuarterBeginDate = null;
                                        eventQuarterEndDate = null;
                                    }
                                else
                                {
                                    operatingHoursEventQuarter = 0;
                                    operatingHoursEventQuarterValid = false;
                                    eventQuarterBeginDate = null;
                                    eventQuarterEndDate = null;
                                }
                            }
                            else
                            {
                                operatingHoursEventQuarter = 0;
                                operatingHoursEventQuarterValid = false;
                                eventQuarterBeginDate = null;
                                eventQuarterEndDate = null;
                            }
                        }

                        /* Stop checking if subsequent checks are affected by missing data */
                        if (!operatingHoursEventQuarterValid)
                        {
                            result = eConditionalDataStatus.MISSINGOPSUPP;
                        }
                        /* Check whether assuming that every hour in the event quarter is operating would not exceed allowed */
                        else if ((opeartingHoursCurrentQuarter + operatingHoursBetweenQuarters + operatingHoursEventQuarter) <= 168)
                        {
                            result = eConditionalDataStatus.VALID;
                        }
                        else
                        {
                            int hoursInEventQuarterPriorToConditionalBeginHour = cDateFunctions.HourDifference(eventQuarterBeginDate.Value, conditionalDataPeriodBeginDateHour.Value);
                            int hoursInEventQuarterOnOrAfterConditionalBeginHour = cDateFunctions.HourDifference(conditionalDataPeriodBeginDateHour.Value, eventQuarterEndDate.Value.AddHours(23)) + 1;

                            /* Check whether assuming the minimum number of operating hours in the event quarter would exceeding allowed */
                            if ((operatingHoursEventQuarter > hoursInEventQuarterPriorToConditionalBeginHour) &&
                                ((opeartingHoursCurrentQuarter + operatingHoursBetweenQuarters + (operatingHoursEventQuarter - hoursInEventQuarterPriorToConditionalBeginHour)) > 168))
                            {
                                result = eConditionalDataStatus.EXPIRED;
                            }
                            /* Check that treating every calendar hour on or after the conditional data begin hour as an operating hour does not exceed allowed */
                            else if ((operatingHoursEventQuarter > hoursInEventQuarterOnOrAfterConditionalBeginHour) &&
                                     ((opeartingHoursCurrentQuarter + operatingHoursBetweenQuarters + hoursInEventQuarterOnOrAfterConditionalBeginHour) <= 168))
                            {
                                result = eConditionalDataStatus.VALID;
                            }
                            /* Cannot determine whether allowed operating hours were exceeded because of uncertainty about operating hours in event quarter */
                            else
                            {
                                result = eConditionalDataStatus.UNDETERMINED;
                            }
                        }
                    }
                }
            }


            return result;
        }
    }
}
