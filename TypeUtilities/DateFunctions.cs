using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using ECMPS.Checks.DatabaseAccess;
using ECMPS.Definitions.Extensions;



namespace ECMPS.Checks.TypeUtilities
{
    /// <summary>
    /// Class containing some nice date/hour functions
    /// </summary>
    public class cDateFunctions
    {

        /// <summary>
        /// Default Constructor
        /// </summary>
        public cDateFunctions()
        {
        }

        private static DataTable FReportingPeriodTable = null;

        /// <summary>
        /// Returns the current Reporting Period Table
        /// </summary>
        public static DataTable ReportingPeriodTable
        {
            get
            {
                if (FReportingPeriodTable == null)
                {
                    try
                    {
                        cDatabase Database = cDatabase.GetConnection(cDatabase.eCatalog.DATA, "cDateFunctions");

                        FReportingPeriodTable = Database.GetDataTable("select * from Reporting_Period order by Calendar_Year, Quarter");
                    }
                    catch
                    {
                        FReportingPeriodTable = null;
                    }
                }

                return FReportingPeriodTable;
            }

            set
            {
                FReportingPeriodTable = value;
            }
        }


        /// <summary>
        /// Uses the passed row and columns names to return a DateTime?
        /// containing hours and minutes.
        /// </summary>
        /// <param name="sourceRow">The row contains the date and time information.</param>
        /// <param name="dateColumnName">The name of the date column.</param>
        /// <param name="hourColumnName">The name of the hour column.</param>
        /// <param name="minuteColumnName">The name of the minute column.</param>
        /// <returns>The date value with hour and minute.</returns>
        static public DateTime? CombineToHour(DataRowView sourceRow, string dateColumnName, string hourColumnName, string minuteColumnName)
        {
            DateTime? result;

            DataColumnCollection columns = sourceRow.Row.Table.Columns;

            if ((sourceRow != null) && (dateColumnName != null) &&
                columns.Contains(dateColumnName) && (sourceRow[dateColumnName] != DBNull.Value))
            {
                result = sourceRow[dateColumnName].AsDateTime();

                if ((hourColumnName != null) &&
                    columns.Contains(hourColumnName) && (sourceRow[hourColumnName] != DBNull.Value))
                {
                    result = result.Value.AddHours(sourceRow[hourColumnName].AsInteger(0));

                    if ((minuteColumnName != null) &&
                        columns.Contains(minuteColumnName) && (sourceRow[minuteColumnName] != DBNull.Value))
                    {
                        result = result.Value.AddMinutes(sourceRow[minuteColumnName].AsInteger(0));
                    }
                }
            }
            else
            {
                result = null;
            }

            return result;
        }


        /// <summary>
        /// Uses the passed row and columns names to return a DateTime?
        /// containing hours.
        /// </summary>
        /// <param name="sourceRow">The row contains the date and time information.</param>
        /// <param name="dateColumnName">The name of the date column.</param>
        /// <param name="hourColumnName">The name of the hour column.</param>
        /// <returns>The date value with hour.</returns>
        static public DateTime? CombineToHour(DataRowView sourceRow, string dateColumnName, string hourColumnName)
        {
            DateTime? result;

            result = CombineToHour(sourceRow, dateColumnName, hourColumnName, null);

            return result;
        }


        /// <summary>
        /// Get the start date of the quarter relative to the passed in reporting period
        /// </summary>
        /// <param name="ARptPeriodId">The reference reporting period</param>
        /// <param name="AYear">The year for the the RPT_PERIOD_ID in question</param>
        /// <param name="AQuarter">The quarter for the the RPT_PERIOD_ID in question</param>
        /// <returns>System.DateTime containing the start date of this quarter</returns>
        public static bool GetYearAndQuarter(int ARptPeriodId, out int AYear, out int AQuarter)
        {
            bool Result = false;

            AYear = AQuarter = int.MinValue;

            if (ReportingPeriodTable != null)
            {
                foreach (DataRow ReportingPeriodRow in ReportingPeriodTable.Rows)
                {
                    int RptPeriodId = cDBConvert.ToInteger(ReportingPeriodRow["RPT_PERIOD_ID"]);

                    if (RptPeriodId == ARptPeriodId)
                    {
                        AYear = cDBConvert.ToInteger(ReportingPeriodRow["CALENDAR_YEAR"]);
                        AQuarter = cDBConvert.ToInteger(ReportingPeriodRow["QUARTER"]);

                        Result = true;

                        break;
                    }
                }
            }

            return Result;
        }

        /// <summary>
        /// Gets the number of the quarter of the input date: 1, 2, 3, or 4
        /// </summary>
        /// <param name="ThisDate">The reference date</param>
        /// <returns>int indicating the present quarter</returns>
        public static int ThisQuarter(DateTime ThisDate)
        {
            int TheMonth = ThisDate.Month;
            int TheQuarter = 4;
            if (TheMonth <= 3)
                TheQuarter = 1;
            else
              if (TheMonth <= 6)
                TheQuarter = 2;
            else
                if (TheMonth <= 9)
                TheQuarter = 3;
            return TheQuarter;
        }


        /// <summary>
        /// Gets the reporting period of the input date.  
        /// If non is found or a failure occurs then the return value is int.MinValue.
        /// </summary>
        /// <param name="ThisDate">The reference date</param>
        /// <returns>int indicating the present reporting period</returns>
        public static int ThisReportingPeriod(DateTime ThisDate)
        {
            int Result = int.MinValue;

            if (ReportingPeriodTable != null)
            {
                foreach (DataRow ReportingPeriodRow in ReportingPeriodTable.Rows)
                {
                    int RptYear = cDBConvert.ToInteger(ReportingPeriodRow["CALENDAR_YEAR"]);

                    if (ThisDate.Year == RptYear)
                    {
                        int RptQuarter = cDBConvert.ToInteger(ReportingPeriodRow["QUARTER"]);

                        int RptMonthBegan = 3 * (RptQuarter - 1) + 1;
                        int RptMonthEnded = 3 * RptQuarter;

                        if ((RptMonthBegan <= ThisDate.Month) && (ThisDate.Month <= RptMonthEnded))
                        {
                            Result = cDBConvert.ToInteger(ReportingPeriodRow["RPT_PERIOD_ID"]);
                            break;
                        }
                    }
                }
            }

            return Result;
        }

        /// <summary>
        /// Get the start date of the quarter relative to the passed in reporting period
        /// </summary>
        /// <param name="ARptPeriodId">The reference reporting period</param>
        /// <returns>System.DateTime containing the start date of this quarter</returns>
        public static DateTime StartDateThisQuarter(int ARptPeriodId)
        {
            DateTime Result = DateTime.MaxValue;

            if (ReportingPeriodTable != null)
            {
                foreach (DataRow ReportingPeriodRow in ReportingPeriodTable.Rows)
                {
                    int RptPeriodId = cDBConvert.ToInteger(ReportingPeriodRow["RPT_PERIOD_ID"]);

                    if (RptPeriodId == ARptPeriodId)
                    {
                        int RptYear = cDBConvert.ToInteger(ReportingPeriodRow["CALENDAR_YEAR"]);
                        int RptQuarter = cDBConvert.ToInteger(ReportingPeriodRow["QUARTER"]);
                        int RptMonthBegan = 3 * (RptQuarter - 1) + 1;

                        Result = new DateTime(RptYear, RptMonthBegan, 1);

                        break;
                    }
                }
            }

            return Result;
        }


        /// <summary>
        /// Get the start date of the next quarter relative to the passed in date
        /// </summary>
        /// <param name="ThisDate">The reference date</param>
        /// <returns>System.DateTime containing the start date of the next quarter</returns>
        public static DateTime StartDateNextQuarter(DateTime ThisDate)
        {
            int Month = ThisDate.Month;
            int Year = ThisDate.Year;
            if (Month <= 3)
                return new DateTime(Year, 4, 1);
            else
            {
                if (Month <= 6)
                    return new DateTime(Year, 7, 1);
                else
                {
                    if (Month <= 9)
                        return new DateTime(Year, 10, 1);
                    else
                        return new DateTime(Year + 1, 1, 1);
                }
            }
        }

        /// <summary>
        /// Get the start date of the quarter relative to the passed in date
        /// </summary>
        /// <param name="ThisDate">The reference date</param>
        /// <returns>System.DateTime containing the start date of this quarter</returns>
        public static DateTime StartDateThisQuarter(DateTime ThisDate)
        {
            int Month = ThisDate.Month;
            int Year = ThisDate.Year;
            if (Month <= 3)
                return new DateTime(Year, 1, 1);
            else
            {
                if (Month <= 6)
                    return new DateTime(Year, 4, 1);
                else
                {
                    if (Month <= 9)
                        return new DateTime(Year, 7, 1);
                    else
                        return new DateTime(Year, 10, 1);
                }
            }
        }

        /// <summary>
        /// Get the start date of the quarter passed in
        /// </summary>
        /// <param name="Year">The reference year</param>
        /// <param name="ThisQuarter">The reference quarter</param>
        /// <returns>System.DateTime containing the start date of the quarter</returns>
        public static DateTime StartDateThisQuarter(int Year, int ThisQuarter)
        {
            if (ThisQuarter == 1)
                return new DateTime(Year, 1, 1);
            else
            {
                if (ThisQuarter == 2)
                    return new DateTime(Year, 4, 1);
                else
                {
                    if (ThisQuarter == 3)
                        return new DateTime(Year, 7, 1);
                    else
                        return new DateTime(Year, 10, 1);
                }
            }
        }


        /// <summary>
        /// Get the end date of the quarter relative to the passed in reporting period
        /// </summary>
        /// <param name="rptPeriodId">The reference reporting period</param>
        /// <returns>System.DateTime containing the start date of this quarter</returns>
        public static DateTime LastDateThisQuarter(int rptPeriodId)
        {
            DateTime result = DateTime.MaxValue;

            if (ReportingPeriodTable != null)
            {
                foreach (DataRow ReportingPeriodRow in ReportingPeriodTable.Rows)
                {
                    int rowRptPeriodId = ReportingPeriodRow["RPT_PERIOD_ID"].AsInteger().Value;

                    if (rowRptPeriodId == rptPeriodId)
                    {
                        int year = ReportingPeriodRow["CALENDAR_YEAR"].AsInteger().Value;
                        int quarter = ReportingPeriodRow["QUARTER"].AsInteger().Value;

                        result = (new DateTime(year, 3 * quarter, 1)).AddMonths(1).AddDays(-1);

                        break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Get the end date of the next quarter relative to the passed in date
        /// </summary>
        /// <param name="ThisDate">The reference date</param>
        /// <returns>System.DateTime containing the start date of the next quarter</returns>
        public static DateTime LastDateNextQuarter(DateTime ThisDate)
        {
            int Month = ThisDate.Month;
            int Year = ThisDate.Year;
            if (Month <= 3)
                return new DateTime(Year, 6, 30);
            else
            {
                if (Month <= 6)
                    return new DateTime(Year, 9, 30);
                else
                {
                    if (Month <= 9)
                        return new DateTime(Year, 12, 31);
                    else
                        return new DateTime(Year + 1, 3, 31);
                }
            }
        }

        /// <summary>
        /// Get the last date for the quarter prior to the passed in date
        /// </summary>
        /// <param name="ThisDate">The reference date</param>
        /// <returns>System.DateTime containing the end date of the previous quarter</returns>
        public static DateTime LastDatePriorQuarter(DateTime ThisDate)
        {
            int Month = ThisDate.Month;
            int Year = ThisDate.Year;
            if (Month <= 3)
                return new DateTime(Year - 1, 12, 31);
            else
            {
                if (Month <= 6)
                    return new DateTime(Year, 3, 31);
                else
                {
                    if (Month <= 9)
                        return new DateTime(Year, 6, 30);
                    else
                        return new DateTime(Year, 9, 30);
                }
            }
        }

        /// <summary>
        /// Get the last date in the current quarter relative  to the passed in date
        /// </summary>
        /// <param name="ThisDate">The reference date</param>
        /// <returns>System.DateTime containing the end date of this quarter</returns>
        public static DateTime LastDateThisQuarter(DateTime ThisDate)
        {
            int Month = ThisDate.Month;
            int Year = ThisDate.Year;
            if (Month <= 3)
                return new DateTime(Year, 3, 31);
            else
            {
                if (Month <= 6)
                    return new DateTime(Year, 6, 30);
                else
                {
                    if (Month <= 9)
                        return new DateTime(Year, 9, 30);
                    else
                        return new DateTime(Year, 12, 31);
                }
            }
        }

        /// <summary>
        /// Get the last date in the quarter passed in
        /// </summary>
        /// <param name="Year">The reference year</param>
        /// <param name="ThisQuarter">The reference quarter</param>
        /// <returns>System.DateTime containing the end date of the quarter</returns>
        public static DateTime LastDateThisQuarter(int Year, int ThisQuarter)
        {
            if (ThisQuarter == 1)
                return new DateTime(Year, 3, 31);
            else
            {
                if (ThisQuarter == 2)
                    return new DateTime(Year, 6, 30);
                else
                {
                    if (ThisQuarter == 3)
                        return new DateTime(Year, 9, 30);
                    else
                        return new DateTime(Year, 12, 31);
                }
            }
        }


        /// <summary>
        /// Returns the whole day difference from the reference date to the test date.  The result will
        /// be negative when the test date is before the reference date.
        /// </summary>
        /// <param name="AReferenceDate">The date from which to measure the difference.</param>
        /// <param name="ATestDate">The date to which to measure the difference.</param>
        /// <returns>The difference between the test date and the reference date.</returns>
        public static int DateDifference(DateTime AReferenceDate, DateTime ATestDate)
        {
            TimeSpan Span = ATestDate.Subtract(AReferenceDate);

            return Span.Days;
        }

        /// <summary>
        /// Returns the hour difference from the reference hour to the test hour.  The result will
        /// be negative when the test hour is before the reference hour.
        /// </summary>
        /// <param name="AReferenceDate">The date from which to measure the difference.</param>
        /// <param name="AReferenceHour">The hour from which to measure the difference.</param>
        /// <param name="ATestDate">The date to which to measure the difference.</param>
        /// <param name="ATestHour">The hour to which to measure the difference.</param>
        /// <returns>The difference between the test hour and the reference hour.</returns>
        public static int HourDifference(DateTime AReferenceDate, int AReferenceHour,
                                         DateTime ATestDate, int ATestHour)
        {
            int DayDelta = DateDifference(AReferenceDate, ATestDate);

            return (24 * DayDelta + (ATestHour - AReferenceHour));
        }

        /// <summary>
        /// Returns thehourly difference between the hours in two DateTime objects.
        /// </summary>
        /// <param name="referenceHour">The starting date in the comparison.</param>
        /// <param name="testHour">The ending date in the comparison.</param>
        /// <returns>The AddHour value used on the reference DateTime to arrive at the test DateTime's Date and Hour values.</returns>
        public static int HourDifference (DateTime referenceHour, DateTime testHour)
        {
            return HourDifference(referenceHour.Date, referenceHour.Hour, testHour.Date, testHour.Hour);
        }

        /// <summary>
        /// Return latest date time in the date time list, excluding nulls.
        /// </summary>
        /// <param name="dateTimeList">The date time list to check.</param>
        /// <returns>The latest date time.</returns>
        public static DateTime? LatestDate(params DateTime?[] dateTimeList)
        {
            DateTime? result = null;

            foreach (DateTime? dateTime in dateTimeList)
            {
                if (dateTime.HasValue && (dateTime.Default(DateTime.MinValue) > result.Default(DateTime.MinValue)))
                    result = dateTime;
            }

            return result;
        }

        /// <summary>
        /// This function returns the earliest datetime in a list of nullible date times.
        /// 
        /// The function ignores any nulls in the list, but will return a null if no non
        /// null dates exist in the list.
        /// </summary>
        /// <param name="datetimeList"></param>
        /// <returns>Returns the earliest date in the list.  Returns a null if no non null dates exist in the list.</returns>
        public static DateTime? Earliest(params DateTime?[] datetimeList)
        {
            return Earliest(false, datetimeList);
        }

        /// <summary>
        /// This function returns the earliest datetime in a list of nullible date times.  
        /// 
        /// Additionally, the function allows the caller to indicate whether a null in the
        /// list should result in the function returning a null, essentially making a null
        /// the earliest date.
        /// </summary>
        /// <param name="nullIsEarliest">Indicates whether to return null if a null is in the list of dates.</param>
        /// <param name="datetimeList">The list of nullible dates from which to return the earliest date.</param>
        /// <returns>Returns the earliest date in the list.</returns>
        public static DateTime? Earliest(bool nullIsEarliest, params DateTime?[] datetimeList)
        {
            DateTime? result = null;

            foreach (DateTime? item in datetimeList)
            {
                /* Set result to null and exit if a null is encountered and a null is the 'earliest' datetime */
                if ((item == null) && nullIsEarliest)
                {
                    result = null;
                    break;
                }

                /* Use the item value if it is not null and if result is null or is greater than the item values */
                if (item.HasValue && (!result.HasValue || (item.Value < result.Value)))
                {
                    result = item;
                }
            }

            return result;
        }
    }
}
