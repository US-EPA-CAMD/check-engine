using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using ECMPS.Definitions.Enumerations;
using System.Text.RegularExpressions;

namespace ECMPS.Definitions.Extensions 
{

	public static class cExtensions
	{

		#region Typing To, and Inspection and Manipulation Of

		#region Array

		/// <summary>
		/// Returns a copy of the passed array Appends and element to the array and assigns the passed value.
		/// </summary>
		/// <param name="list">The array to extend.</param>
		/// <returns>The resulting array.</returns>
		public static string[] Append(this string[] list)
		{
			if (list != null)
				Array.Resize<string>(ref list, list.Length + 1);

			return list;
		}

		/// <summary>
		/// Returns a copy of the passed array Appends and element to the array and assigns the passed value.
		/// </summary>
		/// <param name="list">The array to extend.</param>
		/// <param name="value">The value for the new element.</param>
		/// <returns>The resulting array.</returns>
		public static string[] Append(this string[] list, string value)
		{
			if (list != null)
			{
				Array.Resize<string>(ref list, list.Length + 1);
				list[list.Length - 1] = value;
			}

			return list;
		}

		/// <summary>
		/// Returns string array as a delimited list.
		/// </summary>
		/// <param name="array">The string array to convert.</param>
		/// <param name="delimeter">The delimiter of the resulting list.</param>
		/// <returns>A delimited string list.</returns>
		public static string AsList(this string[] array, char delimeter)
		{
			string result = null;

			bool first = true;

			foreach (string item in array)
			{
				if (first)
					result = item;
				else
					result += delimeter.ToString() + item;
			}

			return result;
		}

		/// <summary>
		/// Returns string array as a delimited list.
		/// </summary>
		/// <param name="array">The string array to convert.</param>
		/// <returns>A delimited string list.</returns>
		public static string AsList(this string[] array)
		{
			string result = null;

			result = array.AsList(',');

			return result;
		}

		#endregion

		#region Boolean and Boolean?

		/// <summary>
		/// Convert an object to a nullable type.
		/// </summary>
		/// <param name="value">The object to convert.</param>
		/// <returns>The value converted to the nullable type</returns>
		public static bool? AsBoolean(this object value)
		{
			bool? result;

			if ((value == DBNull.Value) || (value == null))
			{
				result = null;
			}
			else
			{
				bool tempBool;
				int tempInt;

				if (value.ToString().ToUpper().InList("T,Y,1,F,N,0"))
					result = value.ToString().ToUpper().InList("T,Y,1");
				else if (bool.TryParse(value.ToString(), out tempBool))
					result = tempBool;
				else if (int.TryParse(value.ToString(), out tempInt))
					result = (tempInt > 0);
				else
					result = null;
			}

			return result;
		}

		/// <summary>
		/// Convert an object to a type.
		/// </summary>
		/// <param name="value">The object to convert.</param>
		/// <param name="defaultValue">The default value for null and unconvertable values.</param>
		/// <returns>The value converted to the type</returns>
		public static bool AsBoolean(this object value, bool defaultValue)
		{
			bool result;

			bool? temp = value.AsBoolean();

			if (temp.HasValue())
				result = temp.Value;
			else
				result = defaultValue;

			return result;
		}

		/// <summary>
		/// Returns the value when not null, otherwise returns the default value.
		/// </summary>
		/// <param name="value">The value to default.</param>
		/// <param name="defaultValue">The default value to use if value is null.</param>
		/// <returns>Resturns the value or the default if value is null.</returns>
		public static bool Default(this bool? value, bool defaultValue)
		{
			bool result;

			if (value.HasValue)
				result = value.Value;
			else
				result = defaultValue;

			return result;
		}

		/// <summary>
		/// Returns the value when not null, otherwise returns the default value.
		/// </summary>
		/// <param name="value">The value to default.</param>
		/// <returns>Resturns the value or the default if value is null.</returns>
		public static bool Default(this bool? value)
		{
			bool result;

			result = value.Default(false);

			return result;
		}

		#endregion

		#region DataView and DataRowView
        
		/// <summary>
		/// Convert an object to a Data Row View.
		/// </summary>
		/// <param name="value">The object to convert.</param>
		/// <returns>The data row view or null if the value connot be converted.</returns>
		public static DataRowView AsDataRowView(this object value)
		{
			DataRowView result;

			if ((value == null) || !(value is DataRowView))
				result = null;
			else
				result = (DataRowView)value;

			return result;
		}

		/// <summary>
		/// Convert an object to a Data View.
		/// </summary>
		/// <param name="value">The object to convert.</param>
		/// <returns>The data view or null if the value connot be converted.</returns>
		public static DataView AsDataView(this object value)
		{
			DataView result;

			if ((value == null) || !(value is DataView))
				result = null;
			else
				result = (DataView)value;

			return result;
		}

		/// <summary>
		/// Returns a DateTime? for the row based on the Date, Hour and Minute columns specified.
		/// </summary>
		/// <param name="row">The row to act upon.</param>
		/// <param name="dateColumnName">The date column.</param>
		/// <param name="hourColumnName">The hour column or null if none.</param>
		/// <param name="minuteColumnName">The minute column or null if none.</param>
		/// <returns></returns>
		public static DateTime? AsDateTime(this DataRowView row, string dateColumnName, string hourColumnName, string minuteColumnName)
		{
			DateTime? result;

			if ((row != null) && (dateColumnName.IsNotEmpty() && row[dateColumnName].IsNotDbNull()))
			{
				result = row[dateColumnName].AsDateTime();

				if (hourColumnName.IsNotEmpty() && row[hourColumnName].IsNotDbNull())
				{
					result = result.Value.AddHours(row[hourColumnName].AsInteger(0));

					if (minuteColumnName.IsNotEmpty() && row[minuteColumnName].IsNotDbNull())
						result = result.Value.AddMinutes(row[minuteColumnName].AsInteger(0));
				}
			}
			else
				result = null;

			return result;
		}

		/// <summary>
		/// Returns a DateTime? for the row based on the Date and Hour columns specified.
		/// </summary>
		/// <param name="row">The row to act upon.</param>
		/// <param name="dateColumnName">The date column.</param>
		/// <param name="hourColumnName">The hour column or null if none.</param>
		/// <returns></returns>
		public static DateTime? AsDateTime(this DataRowView row, string dateColumnName, string hourColumnName)
		{
			DateTime? result;

			result = row.AsDateTime(dateColumnName, hourColumnName, null);

			return result;
		}

		#endregion

		#region DateTime and DateTime?

		/// <summary>
		/// Returns the date part of a DateTime? or null if null.
		/// </summary>
		/// <param name="value">The DateTime?.</param>
		/// <returns>The date without the time.</returns>
		public static DateTime? AddHours(this DateTime? value, int? hour)
		{
			DateTime? result;

			if (value.HasValue)
			{
				result = value.Value.Date;

				if (hour.HasValue)
					result.Value.AddHours(hour.Value);
			}
			else
				result = null;

			return result;
		}

		/// <summary>
		/// Convert an object to a type.
		/// </summary>
		/// <param name="value">The object to convert.</param>
		/// <returns>The value converted to the type</returns>
		public static DateTime AsBeginDateTime(this object value)
		{
			DateTime result;

			DateTime? temp = value.AsDateTime();

			if (temp.HasValue())
				result = temp.Value;
			else
				result = DateTime.MinValue;

			return result;
		}

		/// <summary>
		/// Returns the date part of a DateTime? or null if null.
		/// </summary>
		/// <param name="value">The DateTime?.</param>
		/// <returns>The date without the time.</returns>
		public static DateTime? AsDate(this DateTime? value)
		{
			DateTime? result;

			if (value.HasValue)
				result = value.Value.Date;
			else
				result = null;

			return result;
		}

		/// <summary>
		/// Returns the date part of a DateTime.
		/// </summary>
		/// <param name="value">The DateTime.</param>
		/// <returns>The date without the time.</returns>
		public static DateTime AsDate(this DateTime value)
		{
			DateTime result;

			result = value.Date;

			return result;
		}

		/// <summary>
		/// Convert an object to a nullable type.
		/// </summary>
		/// <param name="value">The object to convert.</param>
		/// <returns>The value converted to the nullable type</returns>
		public static DateTime? AsDateTime(this object value)
		{
			DateTime? result;

			if ((value == DBNull.Value) || (value == null))
			{
				result = null;
			}
			else
			{
				DateTime temp;

				if (DateTime.TryParse(value.ToString(), out temp))
					result = temp;
				else
					result = null;
			}

			return result;
		}

		/// <summary>
		/// Convert an object to a type.
		/// </summary>
		/// <param name="value">The object to convert.</param>
		/// <param name="defaultValue">The default value for null and unconvertable values.</param>
		/// <returns>The value converted to the type</returns>
		public static DateTime AsDateTime(this object value, DateTime defaultValue)
		{
			DateTime result;

			DateTime? temp = value.AsDateTime();

			if (temp.HasValue())
				result = temp.Value;
			else
				result = defaultValue;

			return result;
		}

		/// <summary>
		/// Convert an object to a type.
		/// </summary>
		/// <param name="value">The object to convert.</param>
		/// <returns>The value converted to the type</returns>
		public static DateTime AsEndDateTime(this object value)
		{
			DateTime result;

			DateTime? temp = value.AsDateTime();

			if (temp.HasValue())
				result = temp.Value;
			else
				result = DateTime.MaxValue;

			return result;
		}

		/// <summary>
		/// Returns the hour  of a DateTime? or null if null.
		/// </summary>
		/// <param name="value">The DateTime?.</param>
		/// <returns>The date without the time.</returns>
		public static int? AsHour(this DateTime? value)
		{
			int? result;

			if (value.HasValue)
				result = value.Value.Hour;
			else
				result = null;

			return result;
		}

		/// <summary>
		/// Returns the hour  of a DateTime? or null if null.
		/// </summary>
		/// <param name="value">The DateTime.</param>
		/// <returns>The date without the time.</returns>
		public static int AsHour(this DateTime value)
		{
			int result;

			result = value.Hour;

			return result;
		}

		/// <summary>
		/// Convert the passed value to a nullable DateTime, changing the result to MinDate if it is null,
		/// and returns the date portion of the result.
		/// </summary>
		/// <param name="value">The object to convert.</param>
		/// <returns>The value converted to the type</returns>
		public static DateTime AsStartDate(this object value)
		{
			DateTime result;

			result = value.AsStartDateTime().Date;

			return result;
		}

		/// <summary>
		/// Convert the passed value to a nullable DateTime, changing the result to MinDate if it is null,
		/// and returns the final result.
		/// </summary>
		/// <param name="value">The object to convert.</param>
		/// <returns>The value converted to the type</returns>
		public static DateTime AsStartDateTime(this object value)
		{
			DateTime result;

			DateTime? temp = value.AsDateTime();

			if (temp.HasValue())
				result = temp.Value;
			else
				result = DateTime.MinValue;

			return result;
		}

		/// <summary>
		/// Convert the passed value to a nullable DateTime, changing the result to MinDate if it is null,
		/// and returns the hour portion of the result.
		/// </summary>
		/// <param name="value">The object to convert.</param>
		/// <returns>The value converted to the type</returns>
		public static int AsStartHour(this object value)
		{
			int result;

			result = value.AsStartDateTime().Hour;

			return result;
		}

		/// <summary>
		/// Determines whether a date/time is between two other date/times.
		/// 
		/// If the date/time is null then false is returned, and a null at
		/// either end of the range indicates no limit.
		/// </summary>
		/// <param name="value">The date to compare.</param>
		/// <param name="beginDateTime">The begin date range.</param>
		/// <param name="endDateTime">The end date range.</param>
		/// <returns>Returns true if the date is within the range.</returns>
		public static bool Between(this DateTime? value, DateTime? beginDateTime, DateTime? endDateTime)
		{
			bool result;

			if (value.HasValue)
			{
				result = (beginDateTime.Default(value.Value) <= value.Value) && (value.Value <= endDateTime.Default(value.Value));
			}
			else
				result = false;

			return result;
		}

		/// <summary>
		/// Convert the passed value to it DB date only representation.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The DB representation.</returns>
		public static object DbDate(this DateTime value)
		{
			object result;

			if ((value == DateTime.MinValue) || (value == DateTime.MaxValue))
				result = DBNull.Value;
			else
				result = value.Date;

			return result;
		}

		/// <summary>
		/// Convert the passed value to it DB date only representation.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The DB representation.</returns>
		public static object DbDate(this DateTime? value)
		{
			object result;

			if (value == null)
				result = DBNull.Value;
			else
				result = value.Value.Date;

			return result;
		}

		/// <summary>
		/// Convert the passed value to it DB date only representation.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The DB representation.</returns>
		public static object DbHour(this DateTime value)
		{
			object result;

			if ((value == DateTime.MinValue) || (value == DateTime.MaxValue))
				result = DBNull.Value;
			else
				result = value.Hour;

			return result;
		}

		/// <summary>
		/// Convert the passed value to it DB date only representation.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The DB representation.</returns>
		public static object DbHour(this DateTime? value)
		{
			object result;

			if (value == null)
				result = DBNull.Value;
			else
				result = value.Value.Hour;

			return result;
		}

		/// <summary>
		/// Convert the passed value to it DB representation.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The DB representation.</returns>
		public static object DbValue(this DateTime value)
		{
			object result;

			if ((value == DateTime.MinValue) || (value == DateTime.MaxValue))
				result = DBNull.Value;
			else
				result = value;

			return result;
		}

		/// <summary>
		/// Convert the passed value to it DB representation.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The DB representation.</returns>
		public static object DbValue(this DateTime? value)
		{
			object result;

			if (value == null)
				result = DBNull.Value;
			else
				result = value;

			return result;
		}

		/// <summary>
		/// Returns the value when not null, otherwise returns the default value.
		/// </summary>
		/// <param name="value">The value to default.</param>
		/// <param name="defaultValue">The default value to use if value is null.</param>
		/// <returns>Resturns the value or the default if value is null.</returns>
		public static DateTime Default(this DateTime? value, DateTime defaultValue)
		{
			DateTime result;

			if (value.HasValue)
				result = value.Value;
			else
				result = defaultValue;

			return result;
		}

		/// <summary>
		/// Returns the value when not null, otherwise returns the default value.
		/// </summary>
		/// <param name="value">The value to default.</param>
		/// <returns>Resturns the value or the default if value is null.</returns>
		public static DateTime Default(this DateTime? value)
		{
			DateTime result;

			result = value.Default(DateTime.MinValue);

			return result;
		}

		/// <summary>
		/// Determines whether a date/time is not between two other date/times.
		/// 
		/// If the date/time is null then false is returned, and a null at
		/// either end of the range indicates no range limit.
		/// </summary>
		/// <param name="value">The date to compare.</param>
		/// <param name="beginDateTime">The begin date range.</param>
		/// <param name="endDateTime">The end date range.</param>
		/// <returns>Returns true if the date is within the range.</returns>
		public static bool NotBetween(this DateTime? value, DateTime? beginDateTime, DateTime? endDateTime)
		{
			bool result;

			if (value.HasValue)
			{
				result = !((beginDateTime.Default(value.Value) <= value.Value) && (value.Value <= endDateTime.Default(value.Value)));
			}
			else
				result = false;

			return result;
		}

		/// <summary>
		/// Returns the quarter of the given date.
		/// </summary>
		/// <param name="date">The date to inspect.</param>
		/// <returns>The quarter of the date.</returns>
		public static int Quarter(this DateTime date)
		{
			int result = ((date.Month - 1) / 3) + 1;

			return result;
		}

		/// <summary>
		/// Returns the quarter of the given date.
		/// </summary>
		/// <param name="date">The date to inspect.</param>
		/// <returns>The quarter of the date or null if the date is null.</returns>
		public static int? Quarter(this DateTime? date)
		{
			int? result;

			if (date.HasValue)
				result = ((date.Value.Month - 1) / 3) + 1;
			else
				result = null;

			return result;
		}

        /// <summary>
        /// Returns the 'quarter ordinal' for a date using the formula [ordinal = 4 * year + quarter].
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static int QuarterOrd(this DateTime date)
        {
            int result = 4 * date.Year + ((date.Month - 1) / 3 + 1);

            return result;
        }

        /// <summary>
        /// Returns true if the two dates are in the same quarter, or if both dates are null.
        /// </summary>
        /// <param name="thisDate">The nullable date object doing the comparison.</param>
        /// <param name="compareDate">The date to use in the comparison with the data object.</param>
        /// <returns>True if the dates are in the same quarter or or both null, otherwise false.</returns>
        public static bool SameQuarter(this DateTime? thisDate, DateTime? compareDate)
        {
            bool result;

            if (compareDate == thisDate)
                result = true;
            else if (!compareDate.HasValue || !thisDate.HasValue)
                result = false;
            else
                result = (compareDate.Value.Year == thisDate.Value.Year) && (compareDate.Quarter() == thisDate.Quarter());

            return result;
        }

		/// <summary>
		/// Returns a time span object for the from and to dates.
		/// </summary>
		/// <param name="fromDate">The date from which the time span is measured.</param>
		/// <param name="toDate">The date to which the time span is measured.</param>
		/// <returns>The time span between the two dates.</returns>
		public static TimeSpan SpanTo(this DateTime fromDate, DateTime toDate)
		{
			TimeSpan result;

			result = new TimeSpan((toDate.Ticks - fromDate.Ticks));

			return result;
		}

		#endregion

		#region Decimal and Decimal?

		/// <summary>
		/// Convert an double to a nullable decimal.
		/// </summary>
		/// <param name="value">The double to convert.</param>
		/// <returns>The value converted to the nullable type</returns>
		public static decimal? AsDecimal(this double value)
		{
			decimal? result;

			decimal temp;

			if (decimal.TryParse(value.ToString(), out temp))
				result = temp;
			else
				result = null;

			return result;
		}

		/// <summary>
		/// Convert an double to a decimal.
		/// </summary>
		/// <param name="value">The double to convert.</param>
		/// <param name="defaultValue">The default value for null and unconvertable values.</param>
		/// <returns>The value converted to the nullable type</returns>
		public static decimal AsDecimal(this double value, decimal defaultValue)
		{
			decimal result;

			decimal? temp = value.AsDecimal();

			if (temp.HasValue())
				result = temp.Value;
			else
				result = defaultValue;

			return result;
		}

		/// <summary>
		/// Convert an nullabe double to a nullable decimal.
		/// </summary>
		/// <param name="value">The nullabe double to convert.</param>
		/// <returns>The value converted to the nullable type</returns>
		public static decimal? AsDecimal(this double? value)
		{
			decimal? result;

			if (value.HasValue())
				result = value.Value.AsDecimal();
			else
				result = null;

			return result;
		}

		/// <summary>
		/// Convert an nullabe double to a nullable decimal.
		/// </summary>
		/// <param name="value">The nullabe double to convert.</param>
		/// <param name="defaultValue">The default value for null and unconvertable values.</param>
		/// <returns>The value converted to the nullable type</returns>
		public static decimal AsDecimal(this double? value, decimal defaultValue)
		{
			decimal result;

			decimal? temp = value.AsDecimal();

			if (temp.HasValue())
				result = temp.Value;
			else
				result = defaultValue;

			return result;
		}

		/// <summary>
		/// Convert an object to a nullable type.
		/// </summary>
		/// <param name="value">The object to convert.</param>
		/// <returns>The value converted to the nullable type</returns>
		public static decimal? AsDecimal(this object value)
		{
			decimal? result;

			if ((value == DBNull.Value) || (value == null))
			{
				result = null;
			}
			else
			{
				decimal temp;

				if (decimal.TryParse(value.ToString(), out temp))
					result = temp;
				else
					result = null;
			}

			return result;
		}

		/// <summary>
		/// Convert an object to a type.
		/// </summary>
		/// <param name="value">The object to convert.</param>
		/// <param name="defaultValue">The default value for null and unconvertable values.</param>
		/// <returns>The value converted to the type</returns>
		public static decimal AsDecimal(this object value, decimal defaultValue)
		{
			decimal result;

			decimal? temp = value.AsDecimal();

			if (temp.HasValue())
				result = temp.Value;
			else
				result = defaultValue;

			return result;
		}

		/// <summary>
		/// Convert the passed value to it DB representation.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The DB representation.</returns>
		public static object DbValue(this decimal value)
		{
			object result;

			if ((value == decimal.MinValue) || (value == decimal.MaxValue))
				result = DBNull.Value;
			else
				result = value;

			return result;
		}

		/// <summary>
		/// Convert the passed value to it DB representation.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The DB representation.</returns>
		public static object DbValue(this decimal? value)
		{
			object result;

			if (value == null)
				result = DBNull.Value;
			else
				result = value;

			return result;
		}

		/// <summary>
		/// Returns the value when not null, otherwise returns the default value.
		/// </summary>
		/// <param name="value">The value to default.</param>
		/// <param name="defaultValue">The default value to use if value is null.</param>
		/// <returns>Resturns the value or the default if value is null.</returns>
		public static decimal Default(this decimal? value, decimal defaultValue)
		{
			decimal result;

			if (value.HasValue)
				result = value.Value;
			else
				result = defaultValue;

			return result;
		}

		/// <summary>
		/// Returns the value when not null, otherwise returns the default value.
		/// </summary>
		/// <param name="value">The value to default.</param>
		/// <returns>Resturns the value or the default if value is null.</returns>
		public static decimal Default(this decimal? value)
		{
			decimal result;

			result = value.Default(decimal.MinValue);

			return result;
		}

		/// <summary>
		/// Convert a value to a DB assignable format.
		/// </summary>
		/// <param name="value">The value to convert</param>
		/// <returns>DB assignable representation of the value</returns>
		public static string ForDbAssign(this decimal? value)
		{
			string result;

			if (value.HasValue)
			{
				result = string.Format("{0}", value.Value);
			}
			else
			{
				result = "NULL";
			}

			return result;
		}

		/// <summary>
		/// Returns the value raised to the indicated power.
		/// </summary>
		/// <param name="value">Value to raise.</param>
		/// <param name="power">Power to by which to raise the value.</param>
		/// <returns>The value raise to the indicated power.</returns>
		public static decimal Pow(this decimal value, int power)
		{
			decimal result = 1;

			decimal factor = (power >= 0) ? value : (1 / value);

			power = Math.Abs(power);

			for (int count = 1; count <= power; count++)
				result *= factor;

			return result;
		}

		/// <summary>
		/// Rounds a decimal to the number of places specified. 
		/// Negative number rounds to the left of decimal point, positive number rounds to the right.
		/// </summary>
		/// <param name="d">The decimal to call RoundTo on</param>
		/// <param name="place">The place to round to</param>
		/// <returns>d rounded to (10^-place) place</returns>
		public static decimal RoundTo(this Decimal d, int place)
		{
			return (decimal)Math.Pow(10, -place) * Math.Round(d / (decimal)Math.Pow(10, -place), MidpointRounding.AwayFromZero);
		}

		/// <summary>
		/// Returns true if the value is equal to itself rounded to the specified number of decimals.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="decimals"></param>
		/// <returns></returns>
		public static bool IsRounded(this decimal value, int decimals = 0)
		{
			bool result;

			result = (value == Math.Round(value, decimals, MidpointRounding.AwayFromZero));

			return result;
		}

		#endregion

		#region Integer, Integer? and Short?

		/// <summary>
		/// Convert an object to a nullable type.
		/// </summary>
		/// <param name="value">The object to convert.</param>
		/// <returns>The value converted to the nullable type</returns>
		public static int? AsInteger(this object value)
		{
			int? result;

			if ((value == DBNull.Value) || (value == null))
			{
				result = null;
			}
			else
			{
				int temp;

				if (int.TryParse(value.ToString(), out temp))
					result = temp;
				else
					result = null;
			}

			return result;
		}

		/// <summary>
		/// Convert an object to a type.
		/// </summary>
		/// <param name="value">The object to convert.</param>
		/// <param name="defaultValue">The default value for null and unconvertable values.</param>
		/// <returns>The value converted to the type</returns>
		public static int AsInteger(this object value, int defaultValue)
		{
			int result;

			int? temp = value.AsInteger();

			if (temp.HasValue())
				result = temp.Value;
			else
				result = defaultValue;

			return result;
		}

		/// <summary>
		/// Convert the passed value to it DB representation.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The DB representation.</returns>
		public static object DbValue(this int value)
		{
			object result;

			if ((value == int.MinValue) || (value == int.MaxValue))
				result = DBNull.Value;
			else
				result = value;

			return result;
		}

		/// <summary>
		/// Convert the passed value to it DB representation.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The DB representation.</returns>
		public static object DbValue(this int? value)
		{
			object result;

			if (value == null)
				result = DBNull.Value;
			else
				result = value;

			return result;
		}

		/// <summary>
		/// Returns the value when not null, otherwise returns the default value.
		/// </summary>
		/// <param name="value">The value to default.</param>
		/// <param name="defaultValue">The default value to use if value is null.</param>
		/// <returns>Resturns the value or the default if value is null.</returns>
		public static int Default(this int? value, int defaultValue)
		{
			int result;

			if (value.HasValue)
				result = value.Value;
			else
				result = defaultValue;

			return result;
		}

		/// <summary>
		/// Returns the value when not null, otherwise returns the default value.
		/// </summary>
		/// <param name="value">The value to default.</param>
		/// <returns>Resturns the value or the default if value is null.</returns>
		public static int Default(this int? value)
		{
			int result;

			result = value.Default(int.MinValue);

			return result;
		}

		/// <summary>
		/// Convert a value to a DB assignable format.
		/// </summary>
		/// <param name="value">The value to convert</param>
		/// <returns>DB assignable representation of the value</returns>
		public static string ForDbAssign(this int? value)
		{
			string result;

			if (value.HasValue)
			{
				result = string.Format("{0}", value.Value);
			}
			else
			{
				result = "NULL";
			}

			return result;
		}

		/// <summary>
		/// Determines whether the value is in the given range.
		/// </summary>
		/// <param name="value">The value to test.</param>
		/// <param name="began">The beginning of the range.</param>
		/// <param name="ended">The end of the range.</param>
		/// <returns>Returns true if the value is in the range.</returns>
		public static bool InRange(this int value, int began, int ended)
		{
			bool result;

			result = (began <= value) && (value <= ended);

			return result;
		}

		/// <summary>
		/// Determines whether the value is in the given range.
		/// </summary>
		/// <param name="value">The value to test.</param>
		/// <param name="began">The beginning of the range.</param>
		/// <param name="ended">The end of the range.</param>
		/// <returns>Returns true if the value is in the range.</returns>
		public static bool InRange(this int? value, int began, int ended)
		{
			bool result;

			if (value.HasValue)
				result = value.Value.InRange(began, ended);
			else
				result = false;

			return result;
		}

        /// <summary>
        /// Convert an object to a nullable type.
        /// </summary>
        /// <param name="value">The object to convert.</param>
        /// <returns>The value converted to the nullable type</returns>
        public static short? AsShort(this object value)
        {
            short? result;

            if ((value == DBNull.Value) || (value == null))
            {
                result = null;
            }
            else
            {
                short temp;

                if (short.TryParse(value.ToString(), out temp))
                    result = temp;
                else
                    result = null;
            }

            return result;
        }

        /// <summary>
        /// Convert an object to a type.
        /// </summary>
        /// <param name="value">The object to convert.</param>
        /// <param name="defaultValue">The default value for null and unconvertable values.</param>
        /// <returns>The value converted to the type</returns>
        public static short AsShort(this object value, short defaultValue)
        {
            short result;

            short? temp = value.AsShort();

            if (temp.HasValue())
                result = temp.Value;
            else
                result = defaultValue;

            return result;
        }

        #endregion

        #region Scientific Notation

        /// <summary>
        /// Convert string in Scientific Notation format to a decimal
        /// </summary>
        /// <param name="ScientificNotation">The string to convert.</param>
        /// <returns>Returns decimal, or null if conversion fails.</returns>
        public static Decimal ScientificNotationtoDecimal(this string ScientificNotation)
		{
			decimal result;
			Decimal.TryParse(ScientificNotation, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out result);

			return result;
		}

		/// <summary>
		/// Determines whether the passed string represents a number in scientific notation with the specified number of significant digits.
		/// </summary>
		/// <param name="scientificNotation"></param>
		/// <param name="significantDigits"></param>
		/// <returns></returns>
		public static bool SignificantDigitNotationValid(this string scientificNotation, int significantDigits = 3)
		{
			bool result = false;

			/* No spaces are allowed including leading and trailing, and an E is required */
			if (!scientificNotation.Contains(' ') && scientificNotation.Contains('E') && (significantDigits > 0))
			{
				string[] parsed = scientificNotation.Split('E');

				/* Should contain decimal before and integer after one 'E' */
				if (parsed.Length == 2)
				{
					string number = parsed[0];
					{
						/* negative numbers are treated the same as their positive counter parts */
						if (!String.IsNullOrEmpty(number) && (number[0] == '-'))
							number = number.Substring(1);
					}
					string exponent = parsed[1];

					int trashInt;

					/* The exponent should be an integer */
					if (int.TryParse(exponent, out trashInt))
					{
						decimal numberVal;

						/* Insure the number is not null and is actual a number */
						if (!String.IsNullOrEmpty(number) && decimal.TryParse(number, out numberVal))
						{
							if (((significantDigits > 1) && (number.Length == (significantDigits + 1)) || ((significantDigits == 1) && (number.Length == 1))) && // Length of string value 1 is SD is 1 otherwise SD + 1
								((-10 < numberVal) && (numberVal < 10)) && // values should be greater than -10 but less than 10, excluding those end points
								(numberVal == Math.Round(numberVal, (significantDigits - 1)))) // The value should equal the value rounding to SD-1 decimal places.
							{
								result = true;
							}
						}
					}
				}
			}

			return result;
		}

		/// <summary>
		/// Converts a decimal to Scientific Notation
		/// </summary>
		/// <param name="Number">The number to convert.</param>
		/// <param name="SignificantDigits">Optional parameter for specifying significant digits. Default is 3.</param>
		/// <returns>String in Scientific Notation</returns>
		public static String DecimaltoScientificNotation(this decimal Number, int SignificantDigits = 3)
		{
			string customFormat = "0." + (new string('0', (SignificantDigits - 1))) + "E0";
			string result = Number.ToString(customFormat, System.Globalization.CultureInfo.InvariantCulture);

			return result;
		}

		/// <summary>
		/// Converts a decimal to Scientific Notation
		/// </summary>
		/// <param name="Number">The nullable number to convert.</param>
		/// <param name="SignificantDigits">Optional parameter for specifying significant digits. Default is 3.</param>
		/// <returns>String in Scientific Notation</returns>
		public static String DecimaltoScientificNotation(this decimal? Number, int SignificantDigits = 3)
		{
			string result;

			if (Number.HasValue)
				result = Number.Value.DecimaltoScientificNotation(SignificantDigits);
			else
				result = null;

			return result;
		}

		/// <summary>
		/// Validates Scientific Notation format
		/// </summary>
		/// <param name="value">The string to validate.</param>
		/// <returns>Returns boolean indicating whether format is valid.</returns>
		public static Boolean ScientificNotationValid(this string ScientificNotation)
		{
			Boolean success;
			decimal result; //not actually using result, just testing to see if it's valid
			if (ScientificNotation == "0")
				success = false;
			else
				success = Decimal.TryParse(ScientificNotation, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out result);

			return success;
		}

		/// <summary>
		/// Examines string in Scientific Notation format and returns exponent indicated
		/// </summary>
		/// <param name="value">The string to convert.</param>
		/// <returns>Returns integer, or null if invalid format.</returns>
		public static int ScientificNotationExponent(string ScientificNotation)
		{
			int exponent;
			ScientificNotation = ScientificNotation.ToUpper();

			//value of number after the "E"
			int ix = ScientificNotation.IndexOf("E") + 1;
			int.TryParse(ScientificNotation.Substring(ix, (ScientificNotation.Length - ix)), out exponent);
			return exponent;
		}

		/// <summary>
		/// Examines string in Scientific Notation format and returns number of Significant Digits reported
		/// </summary>
		/// <param name="value">The string to convert.</param>
		/// <returns>Returns integer, or null if invalid format.</returns>
		public static int? ScientificNotationSignificantDigits(this string value)
		{
			int? result;

			if (value.ScientificNotationValid())
			{
				string numberPart = ((value.ToUpper().Contains('E')) ? value.ToUpper().Substring(0, value.IndexOf("E")) : value);
				result = numberPart.Replace(".", "").Replace("-", "").Replace("+", "").Trim().Length;
			}
			else
			{
				result = null;
			}

			return result;
		}

		#endregion

		#region List

		/// <summary>
		/// Converts a string list into a delimited string.
		/// </summary>
		/// <param name="stringList">The source string list.</param>
		/// <param name="delimiter">The delimiter for the result.</param>
		/// <returns>The resulting delimited string or null if the string list is empty.</returns>
		public static string DelimitedList(this List<string> stringList, string delimiter)
		{
			string result;

			if ((stringList != null) && (stringList.Count > 0))
			{
				string resultDelimiter = "";

				result = "";

				foreach (string value in stringList)
				{
					result += resultDelimiter + value;
					resultDelimiter = delimiter;
				}
			}
			else
			{
				result = null;
			}

			return result;
		}

		/// <summary>
		/// Converts a string list into a delimited string.
		/// </summary>
		/// <param name="stringList">The source string list.</param>
		/// <returns>The resulting delimited string or null if the string list is empty.</returns>
		public static string DelimitedList(this List<string> stringList)
		{
			string result;

			result = DelimitedList(stringList, ",");

			return result;
		}

		/// <summary>
		/// Returns a list of distince values in the list of rows for the indicated column.
		/// </summary>
		/// <param name="rowList">The data row view list to check.</param>
		/// <param name="columnName">The column to check in the active range rows.</param>
		/// <returns>The list of distinct values.</returns>
		public static List<string> DistinctValues(this List<DataRowView> rowList, string columnName)
		{
			List<string> result = new List<string>();

			foreach (DataRowView row in rowList)
			{
				if (row.Row.Table.Columns.Contains(columnName))
				{
					string value = row[columnName].AsString();
					if (!result.Contains(value)) result.Add(value);
				}
			}

			return result;
		}

		/// <summary>
		/// Returns a list of distince values in the list of rows for the indicated column.
		/// </summary>
		/// <param name="view">The data view to check.</param>
		/// <param name="columnName">The column to check in the active range rows.</param>
		/// <returns>The list of distinct values.</returns>
		public static List<string> DistinctValues(this DataView view, string columnName)
		{
			List<string> result = new List<string>();

			if (view.Table.Columns.Contains(columnName))
			{
				foreach (DataRowView row in view)
				{
					string value = row[columnName].AsString();
					if (!result.Contains(value)) result.Add(value);
				}
			}

			return result;
		}

		#endregion

		#region Long and Long?

		/// <summary>
		/// Convert an object to a nullable type.
		/// </summary>
		/// <param name="value">The object to convert.</param>
		/// <returns>The value converted to the nullable type</returns>
		public static long? AsLong(this object value)
		{
			long? result;

			if ((value == DBNull.Value) || (value == null))
			{
				result = null;
			}
			else
			{
				long temp;

				if (long.TryParse(value.ToString(), out temp))
					result = temp;
				else
					result = null;
			}

			return result;
		}

		/// <summary>
		/// Convert an object to a type.
		/// </summary>
		/// <param name="value">The object to convert.</param>
		/// <param name="defaultValue">The default value for null and unconvertable values.</param>
		/// <returns>The value converted to the type</returns>
		public static long AsLong(this object value, long defaultValue)
		{
			long result;

			long? temp = value.AsLong();

			if (temp.HasValue())
				result = temp.Value;
			else
				result = defaultValue;

			return result;
		}

		/// <summary>
		/// Convert the passed value to it DB representation.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The DB representation.</returns>
		public static object DbValue(this long value)
		{
			object result;

			if ((value == long.MinValue) || (value == long.MaxValue))
				result = DBNull.Value;
			else
				result = value;

			return result;
		}

		/// <summary>
		/// Convert the passed value to it DB representation.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The DB representation.</returns>
		public static object DbValue(this long? value)
		{
			object result;

			if (value == null)
				result = DBNull.Value;
			else
				result = value;

			return result;
		}

		/// <summary>
		/// Returns the value when not null, otherwise returns the default value.
		/// </summary>
		/// <param name="value">The value to default.</param>
		/// <param name="defaultValue">The default value to use if value is null.</param>
		/// <returns>Resturns the value or the default if value is null.</returns>
		public static long Default(this long? value, long defaultValue)
		{
			long result;

			if (value.HasValue)
				result = value.Value;
			else
				result = defaultValue;

			return result;
		}

		/// <summary>
		/// Returns the value when not null, otherwise returns the default value.
		/// </summary>
		/// <param name="value">The value to default.</param>
		/// <returns>Resturns the value or the default if value is null.</returns>
		public static long Default(this long? value)
		{
			long result;

			result = value.Default(long.MinValue);

			return result;
		}

		/// <summary>
		/// Convert a value to a DB assignable format.
		/// </summary>
		/// <param name="value">The value to convert</param>
		/// <returns>DB assignable representation of the value</returns>
		public static string ForDbAssign(this long? value)
		{
			string result;

			if (value.HasValue)
			{
				result = string.Format("{0}", value.Value);
			}
			else
			{
				result = "NULL";
			}

			return result;
		}

		#endregion

		#region Object

		/// <summary>
		/// Indicates whether an object in not DbNull or null.
		/// </summary>
		/// <param name="value">The value to inspect.</param>
		/// <returns>Returns true if value is not DbNull or null.</returns>
		public static bool HasDbValue(this object value)
		{
			bool result;

			result = (value != DBNull.Value) && (value != null);

			return result;
		}

		/// <summary>
		/// Indicates whether an object in null.
		/// </summary>
		/// <param name="value">The value to inspect.</param>
		/// <returns>Returns true if value is not null, but true if it is DbNull.</returns>
		public static bool HasValue(this object value)
		{
			bool result;

			result = (value != null);

			return result;
		}

		/// <summary>
		/// Indicates whether an object is set to DbNull.
		/// </summary>
		/// <param name="value">The value to inspect.</param>
		/// <returns>Returns true if value is DbNull, but false if it is null.</returns>
		public static bool IsDbNull(this object value)
		{
			bool result;

			result = (value == DBNull.Value);

			return result;
		}

		/// <summary>
		/// Indicates whether an object is set to DbNull.
		/// </summary>
		/// <param name="value">The value to inspect.</param>
		/// <returns>Returns true if value is DbNull, but false if it is null.</returns>
		public static bool IsNotDbNull(this object value)
		{
			bool result;

			result = (value != DBNull.Value);

			return result;
		}

		/// <summary>
		/// Indicates whether an object is not null.
		/// </summary>
		/// <param name="value">The value to inspect.</param>
		/// <returns>Returns true if value is null, but false if it is set to DbNull.</returns>
		public static bool IsNotNull(this object value)
		{
			bool result;

			result = (value != null);

			return result;
		}

		/// <summary>
		/// Indicates whether an object is null.
		/// </summary>
		/// <param name="value">The value to inspect.</param>
		/// <returns>Returns true if value is null, but false if it is set to DbNull.</returns>
		public static bool IsNull(this object value)
		{
			bool result;

			result = (value == null);

			return result;
		}

		#endregion

		#region String

		/// <summary>
		/// Convert a database string value to a string
		/// </summary>
		/// <param name="value">The database string to convert</param>
		/// <returns>string representation of the value</returns>
		public static string AsString(this object value)
		{
			if ((value == DBNull.Value) || (value == null))
			{
				return null;
			}
			else
			{
				return Convert.ToString(value);
			}
		}

		/// <summary>
		/// Convert a database string value to a string
		/// </summary>
		/// <param name="value">The database string to convert</param>
		/// <returns>string representation of the value</returns>
		public static string AsString(this object value, string defaultValue)
		{
			if ((value == DBNull.Value) || (value == null))
			{
				return defaultValue;
			}
			else
			{
				return Convert.ToString(value);
			}
		}

		/// <summary>
		/// Convert the passed value to it DB representation.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>The DB representation.</returns>
		public static object DbValue(this string value)
		{
			object result;

			if (string.IsNullOrEmpty(value))
				result = DBNull.Value;
			else
				result = value;

			return result;
		}

		/// <summary>
		/// Returns the value when not null, otherwise returns the default value.
		/// </summary>
		/// <param name="value">The value to default.</param>
		/// <returns>Resturns the value or the default if value is null.</returns>
		public static string Default(this string value)
		{
			string result;

			if (!string.IsNullOrEmpty(value))
				result = value;
			else
				result = "";

			return result;
		}

		/// <summary>
		/// Convert a value to a DB assignable format.
		/// </summary>
		/// <param name="value">The value to convert</param>
		/// <returns>DB assignable representation of the value</returns>
		public static string ForDbAssign(this string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				return "NULL";
			}
			else
			{
				return string.Format("'{0}'", value);
			}
		}

		/// <summary>
		/// Determines whether the string matches the standard format.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="standardFormat"></param>
		/// <returns></returns>
		public static bool InFormat(this string value, eStandardFormat standardFormat)
		{
			bool result;

			if (value.IsEmpty())
			{
				result = false;
			}
			else
			{
				switch (standardFormat)
				{
					case eStandardFormat.Email:
						{
							if (value[0] == '.')
							{
								result = false;  //Cannot begin with a period
							}
							else
							{
								int atPosition = value.IndexOf('@');
								bool atMultiple = (value.IndexOf('@', atPosition + 1) > -1);
								result = false;

								bool foundAt = false;
								bool foundDot = false;
								bool haveDomain = false;
								bool haveLocal = false;
								bool haveTop = false;

								foreach (char character in value)
								{
									if (!foundAt && (character == '@'))
									{
										foundAt = true;
									}
									else if (foundAt && !foundDot && (character == '.'))
									{
										foundDot = true;
									}
									else if (!foundAt) // Working on local
									{
										haveLocal = true;
									}
									else if (!foundDot) // Working on local
									{
										haveDomain = true;
									}
									else // Working on top domain
									{
										haveTop = true;
										break;
									}
								}

								result = haveLocal && haveDomain && haveTop;
							}
						}
						break;

					case eStandardFormat.Phone:
						{
							const int ascii0 = (int)'0';
							const int ascii9 = (int)'9';
							const int asciiDash = (int)'-';

							if ((value != null) && (value.Length == 12))
							{
								result = true;

								for (int dex = 0; dex < value.Length; dex++)
								{
									int ascii = (int)value[dex];

									if ((dex == 3) || (dex == 7))
									{
										if (ascii != asciiDash) result = false;
									}
									else
									{
										if ((ascii < ascii0) || (ascii > ascii9)) result = false;
									}
								}
							}
							else
								result = false;
						}
						break;

					default:
						{
							result = false;
						}
						break;
				}
			}

			return result;
		}

		/// <summary>
		/// Indicates whether the item is in the associated list.
		/// </summary>
		/// <param name="item">Item to test.</param>
		/// <param name="list">List to test against.</param>
		/// <param name="delimeter">The list delimiter.</param>
		/// <returns></returns>
		public static bool InList(this string item, string list, char delimeter)
		{
			bool result = false;

			for (int dex = 0; dex < list.ListCount(delimeter); dex++)
			{
				if (ListItem(list, dex, delimeter) == item)
					result = true;
			}

			return result;
		}

		/// <summary>
		/// Indicates whether the item is in the associated list.
		/// </summary>
		/// <param name="item">Item to test.</param>
		/// <param name="list">List to test against.</param>
		/// <returns></returns>
		public static bool InList(this string item, string list)
		{
			bool result;

			result = item.InList(list, ',');

			return result;
		}

		/// <summary>
		/// Indicates whether the item is in the associated list.
		/// </summary>
		/// <param name="item">Item to test.</param>
		/// <param name="list">List to test against.</param>
		/// <param name="delimeter">The list delimiter.</param>
		/// <returns></returns>
		public static bool InList(this string item, string list, string delimeter)
		{
			bool result = false;

			for (int dex = 0; dex < list.ListCount(delimeter); dex++)
			{
				if (list.ListItem(dex, delimeter) == item)
				{
					result = true;
					break;
				}
			}

			return result;
		}

		/// <summary>
		/// Determines whether all characters in a string are alpha characters.
		/// </summary>
		/// <param name="value">The string to test.</param>
		/// <returns></returns>
		public static bool IsAlpha(this string value)
		{
			const int asciiUpperA = (int)'A';
			const int asciiUpperZ = (int)'Z';
			const int asciiLowerA = (int)'a';
			const int asciiLowerZ = (int)'z';

			bool result = true;

			foreach (char character in value)
			{
				int asciiValue = (int)character;

				if (!(((asciiValue >= asciiUpperA) && (asciiValue <= asciiUpperZ)) || ((asciiValue >= asciiLowerA) && (asciiValue <= asciiLowerZ))))
				{
					result = false;
					break;
				}
			}

			return result;
		}

		/// <summary>
		/// Determines whether all characters in a string are alpha characters.
		/// </summary>
		/// <param name="value">The string to test.</param>
		/// <param name="exceptions">Exception characters.</param>
		/// <returns></returns>
		public static bool IsAlpha(this string value, params char[] exceptions)
		{
			const int asciiUpperA = (int)'A';
			const int asciiUpperZ = (int)'Z';
			const int asciiLowerA = (int)'a';
			const int asciiLowerZ = (int)'z';

			bool result = true;

			foreach (char character in value)
			{
				int asciiValue = (int)character;

				if (!(((asciiValue >= asciiUpperA) && (asciiValue <= asciiUpperZ)) || ((asciiValue >= asciiLowerA) && (asciiValue <= asciiLowerZ))))
				{
					bool inList = false;

					foreach (char item in exceptions)
					{
						if (character == item)
						{
							inList = true;
							break;
						}
					}

					if (!inList)
					{
						result = false;
						break;
					}
				}
			}

			return result;
		}

		/// <summary>
		/// Indicates whether an string is null or an empty string.
		/// </summary>
		/// <param name="value">The value to inspect.</param>
		/// <returns>Returns true if value is null, but false if it is set to DbNull.</returns>
		public static bool IsEmpty(this string value)
		{
			bool result;

			result = string.IsNullOrEmpty(value);

			return result;
		}

		/// <summary>
		/// Indicates whether an string is not null and not an empty string.
		/// </summary>
		/// <param name="value">The value to inspect.</param>
		/// <returns>Returns true if value is null, but false if it is set to DbNull.</returns>
		public static bool IsNotEmpty(this string value)
		{
			bool result;

			result = !string.IsNullOrEmpty(value);

			return result;
		}

        /// <summary>
        /// Indicates whether an string is null or an empty string.
        /// </summary>
        /// <param name="value">The value to inspect.</param>
        /// <returns>Returns true if value is null, but false if it is set to DbNull.</returns>
        public static bool IsWhitespace(this string value)
        {
            bool result;

            result = string.IsNullOrWhiteSpace(value);

            return result;
        }

        /// <summary>
        /// Add an item to the list
        /// </summary>
        /// <param name="list">The list to add the item to (may be null)</param>
        /// <param name="item">The item to add to the list</param>
        /// <param name="delimiter">The delimeter for the list</param>
        /// <param name="allowDuplicates">Are duplicates allowed in the list?</param>
        /// <returns>The new list with the item appened to the end</returns>
        public static string ListAdd(this string list, string item, char delimiter, bool allowDuplicates)
		{
			if (string.IsNullOrEmpty(list) || list.Trim() == "")
			{
				return item;
			}
			else
			{
				if (allowDuplicates)
					return list + delimiter + item;
				else
				{
					if (!item.InList(list, delimiter))
						return list + delimiter + item;
					else
						return list;
				}
			}
		}

		/// <summary>
		/// Add an item to the list, using default of comma as the delimeter - NO DUPLICATES ALLOWED
		/// </summary>
		/// <param name="list">The list to add the item to (may be null)</param>
		/// <param name="item">The item to add to the list</param>
		/// <returns>The new list with the item appened to the end</returns>
		public static string ListAdd(this string list, string item)
		{
			return ListAdd(list, item, ',', false);
		}

		/// <summary>
		/// Add an item to the list, using default of comma as the delimeter
		/// </summary>
		/// <param name="list">The list to add the item to (may be null)</param>
		/// <param name="item">The item to add to the list</param>
		/// <param name="allowDuplicates">Are duplicates allowed in the list?</param>
		/// <returns>The new list with the item appened to the end</returns>
		public static string ListAdd(this string list, string item, bool allowDuplicates)
		{
			return ListAdd(list, item, ',', allowDuplicates);
		}

		/// <summary>
		/// Add an item to the list
		/// </summary>
		/// <param name="list">The list to add the item to (may be null)</param>
		/// <param name="item">The item to add to the list</param>
		/// <param name="delimiter">The delimeter for the list</param>
		/// <param name="allowDuplicates">Are duplicates allowed in the list?</param>
		/// <returns>The new list with the item appened to the end</returns>
		public static string ListAdd(this string list, string item, string delimiter, bool allowDuplicates)
		{
			if (string.IsNullOrEmpty(list) || list.Trim() == "")
			{
				return item;
			}
			else
			{
				if (allowDuplicates)
					return list + delimiter + item;
				else
				{
					if (!item.InList(list, delimiter))
						return list + delimiter + item;
					else
						return list;
				}
			}
		}

		/// <summary>
		/// Add an item to the list
		/// </summary>
		/// <param name="list">The list to add the item to (may be null)</param>
		/// <param name="item">The item to add to the list</param>
		/// <param name="delimiter">The delimeter for the list</param>
		/// <returns>The new list with the item appened to the end</returns>
		public static string ListAdd(this string list, string item, string delimiter)
		{
			string result;

			result = list.ListAdd(item, delimiter, false);

			return result;
		}

		/// <summary>
		/// Gets the number of items in a string list.
		/// </summary>
		/// <param name="list">The string list in which to count.</param>
		/// <param name="delimiter">The string list delimiter.</param>
		/// <returns>The number of items in the string list.</returns>
		public static int ListCount(this string list, char delimiter)
		{
			int result = 0;

			if ((list != null) && !list.Trim().IsEmpty() && (list.Length > 0))
			{
				result += 1;

				char[] characters = list.ToCharArray();

				foreach (char character in characters)
				{ if (character == delimiter) result += 1; }
			}

			return result;
		}

		/// <summary>
		/// Gets the number of items in a string list delimited by a comma.
		/// </summary>
		/// <param name="list">The string list in which to count.</param>
		/// <returns>The number of items in the string list.</returns>
		public static int ListCount(this string list)
		{
			int result;

			result = list.ListCount(',');

			return result;
		}

		/// <summary>
		/// Gets the number of items in a string list.
		/// </summary>
		/// <param name="list">The string list in which to count.</param>
		/// <param name="delimiter">The string list delimiter.</param>
		/// <returns>The number of items in the string list.</returns>
		public static int ListCount(this string list, string delimiter)
		{
			int result = 0;

			if ((list != null) && !list.Trim().IsEmpty() && (list.Length > 0))
			{
				result = 1;

				if (!delimiter.IsEmpty())
				{
					int position = 0;
					string temp = list;

					while (temp.IndexOf(temp, position) > 0)
					{
						result += 1;
						position = temp.IndexOf(temp, position) + delimiter.Length;
					}

					for (int i = 0; i <= list.Length - 1; i++)
					{
						if (list.ToCharArray()[i].ToString() == delimiter)
						{
							result++;
						}
					}
				}
			}

			return result;
		}

		/// <summary>
		/// Returns the item in a string list at the position indicated.
		/// If the position does not exist, a null is returned.
		/// </summary>
		/// <param name="list">The list to search.</param>
		/// <param name="position">The position of the item to return.</param>
		/// <param name="delimiter">The list delimiter.</param>
		/// <returns>The item at the indicated position.</returns>
		public static string ListItem(this string list, int position, char delimiter)
		{
			string result = null;

			if ((list != null) && !string.IsNullOrEmpty(list.Trim()) && (list.Length > 0))
			{
				int dex = 0;

				char[] characters = list.ToCharArray();

				result = "";

				foreach (char character in characters)
				{
					if (character == delimiter)
						dex += 1;
					else if (dex == position)
						result += character.ToString();
					else if (dex > position)
						break;
				}

				if (dex < position)
					result = null;
			}

			return result;
		}

		/// <summary>
		/// Returns the item in a comma delimited string list at the position indicated.
		/// If the position does not exist, a null is returned.
		/// </summary>
		/// <param name="list">The list to search.</param>
		/// <param name="position">The position of the item to return.</param>
		/// <returns>The item at the indicated position.</returns>
		public static string ListItem(this string list, int position)
		{
			string result = "";

			result = list.ListItem(position, ',');

			return result;
		}

		/// <summary>
		/// Returns the item in a string list at the position indicated.
		/// If the position does not exist, a null is returned.
		/// </summary>
		/// <param name="list">The list to search.</param>
		/// <param name="position">The position of the item to return.</param>
		/// <param name="delimiter">The list delimiter.</param>
		/// <returns>The item at the indicated position.</returns>
		public static string ListItem(this string list, int position, string delimiter)
		{
			string result = "";

			int delimeterCount = 0; ;
			int itemBeganPosition = -1;
			int itemEndedPosition = 0; ;

			for (int dex = 0; dex <= list.Length - 1; dex++)
			{
				if (itemBeganPosition >= 0 && delimeterCount == position)
				{
					if (list.ToCharArray()[dex].ToString() == delimiter)
					{
						itemEndedPosition = dex;
						break;
					}
				}
				else
				{
					if (dex == 0 && position == 0)
					{
						itemBeganPosition = 0;
					}
					else
					{
						if (list.ToCharArray()[dex].ToString() == delimiter)
						{
							delimeterCount += 1;
							itemBeganPosition = dex + 1;
						}
					}
				}

				if (itemEndedPosition == 0 && delimeterCount == position)
				{
					itemEndedPosition = list.Length;
				}
			}

			result = list.Substring(itemBeganPosition, itemEndedPosition - itemBeganPosition);

			return result;
		}

		/// <summary>
		/// Returns count of occurrences of item in list.
		/// </summary>
		/// <param name="item">Item to test.</param>
		/// <param name="list">List to test against.</param>
		/// <param name="delimeter">The list delimiter.</param>
		/// <returns></returns>
		public static int ListItemCount(this string list, string item, string delimeter)
		{
			string[] IdArray = list.Split(Convert.ToChar(delimeter));
			int count = 0;
			foreach (string compareitem in IdArray)
			{
				if (item == compareitem)
				{
					count++;
				}
			}
			return count;
		}

		/// <summary>
		/// Indicates whether the item is not in the associated list.
		/// </summary>
		/// <param name="item">Item to test.</param>
		/// <param name="list">List to test against.</param>
		/// <param name="delimeter">The list delimiter.</param>
		/// <returns></returns>
		public static bool NotInList(this string item, string list, char delimeter)
		{
			bool result;

			result = !item.InList(list, delimeter);

			return result;
		}

		/// <summary>
		/// Indicates whether the item is not in the associated list.
		/// </summary>
		/// <param name="item">Item to test.</param>
		/// <param name="list">List to test against.</param>
		/// <returns></returns>
		public static bool NotInList(this string item, string list)
		{
			bool result;

			result = !item.InList(list);

			return result;
		}

		/// <summary>
		/// Indicates whether the item is not in the associated list.
		/// </summary>
		/// <param name="item">Item to test.</param>
		/// <param name="list">List to test against.</param>
		/// <param name="delimeter">The list delimiter.</param>
		/// <returns></returns>
		public static bool NotInList(this string item, string list, string delimeter)
		{
			bool result;

			result = !item.InList(list, delimeter);

			return result;
		}

		/// <summary>
		/// Format a list for display
		/// <example>fred,barney,wilma becomes fred, barney and wilma</example>
		/// </summary>
		/// <param name="AList">The list in question</param>
		/// <param name="OrText">Is the last item in the list 'and' or 'or'</param>
		/// <returns>The formated list for display</returns>
		public static string FormatList(this string AList, bool OrText)
		{
			if (string.IsNullOrEmpty(AList))
				return AList;
			else
			{
				string Result = AList;

				int LastComma = Result.LastIndexOf(",");

				if (LastComma <= 0)
					return Result;
				else
				{
					string LastItem = Result.Substring(LastComma + 1);

					Result = Result.Substring(0, LastComma) + (AList.ListCount() > 2 ? "," : " ") + (OrText ? "or " : "and ") + LastItem;
					Result = Result.Replace(",", ", ");

					return Result;
				}
			}
		}

		/// <summary>
		/// Format a list for display
		/// <example>fred,barney,wilma becomes fred, barney and wilma</example>
		/// </summary>
		/// <param name="AList">The list in question</param>
		/// <returns>The formated list for display</returns>
		public static string FormatList(this string AList)
		{
			return AList.FormatList(false);
		}

		#endregion

		#endregion

	}

}
