using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ECMPS.Checks.TypeUtilities
{
  /// <summary>
  /// This utility class provides methods that facilitate formating filters for DataViews.
  /// </summary>
  public static class cFilterFormat
  {

    #region Equals

    /// <summary>
    /// Returns an equal expresion for a filter
    /// </summary>
    /// <param name="columnName">The column name.</param>
    /// <param name="columnValue">The column values.</param>
    /// <returns>The equals expression.</returns>
    public static string AsEquals(string columnName, DateTime columnValue)
    {
      string format = "{0} = #{1}#";

      string result;

      result = string.Format(format, columnName, columnValue.ToShortDateString());

      return result;
    }

    /// <summary>
    /// Returns an equal expresion for a filter
    /// </summary>
    /// <param name="columnName">The column name.</param>
    /// <param name="columnValue">The column values.</param>
    /// <returns>The equals expression.</returns>
    public static string AsEquals(string columnName, string columnValue)
    {
      string format = "{0} = '{1}'";

      string result;

      result = string.Format(format, columnName, columnValue);

      return result;
    }

    #endregion


    #region Boolean Operations

    /// <summary>
    /// Ands the two filter expresions together.
    /// </summary>
    /// <param name="operandOne">The first filter expression.</param>
    /// <param name="operandTwo">The second filter expression.</param>
    /// <returns></returns>
    public static string AndFilters(string operandOne, string operandTwo)
    {
      string format = "{0} and {1}";

      string result;

      result = string.Format(format, operandOne, operandTwo);

      return result;
    }

    /// <summary>
    /// Ors the two filter expresions together.
    /// </summary>
    /// <param name="operandOne">The first filter expression.</param>
    /// <param name="operandTwo">The second filter expression.</param>
    /// <returns></returns>
    public static string OrFilters(string operandOne, string operandTwo)
    {
      string format = "({0} or {1})";

      string result;

      result = string.Format(format, operandOne, operandTwo);

      return result;
    }

    #endregion


    #region Active Rows

    /// <summary>
    /// Returns a filter string that will produce active rows given a specific hour
    /// for the specified began and ended columns.
    /// </summary>
    /// <param name="opDate">The date to test.</param>
    /// <param name="opHour">The hour to test.</param>
    /// <param name="beganDateColumnName">The began date column of the data.</param>
    /// <param name="beganHourColumnName">The began hour column of the data.</param>
    /// <param name="endedDateColumnName">The ended date column of the data.</param>
    /// <param name="endedHourColumnName">The ended hour column of the data.</param>
    /// <returns>The filter string.</returns>
    public static string ActiveRows(DateTime opDate, int opHour, 
                                    string beganDateColumnName, string beganHourColumnName,
                                    string endedDateColumnName, string endedHourColumnName)
    {
      string format = "(({2} is null) or ({2} < '{0}') or (({2} = '{0}') and ({3} <= {1})))"
                    + " and "
                    + "(({4} is null) or ({4} > '{0}') or (({4} = '{0}') and ({5} >= {1})))";

      string result;

      result = string.Format(format, 
                             opDate.ToShortDateString(), opHour, 
                             beganDateColumnName, beganHourColumnName, 
                             endedDateColumnName, endedHourColumnName);

      return result;
    }

    /// <summary>
    /// Returns a filter string that will produce active rows given a specific hour
    /// for the BEGIN_DATE, BEGIN_HOUR, END_DATE and END_HOUR columns.
    /// </summary>
    /// <param name="opDate">The date to test.</param>
    /// <param name="opHour">The hour to test.</param>
    /// <returns>The filter string.</returns>
    public static string ActiveRows(DateTime opDate, int opHour)
    {
      string result;

      result = ActiveRows(opDate, opHour, "BEGIN_DATE", "BEGIN_HOUR", "END_DATE", "END_HOUR");

      return result;
    }

    /// <summary>
    /// Returns a filter string that will produce active rows given a specific hour
    /// for the specified began and ended columns.
    /// </summary>
    /// <param name="opDate">The date to test.</param>
    /// <param name="beganDateColumnName">The began date column of the data.</param>
    /// <param name="endedDateColumnName">The ended date column of the data.</param>
    /// <returns>The filter string.</returns>
    public static string ActiveRows(DateTime opDate, string beganDateColumnName, string endedDateColumnName)
    {
      string format = "(({1} is null) or ({1} < '{0}'))"
                    + " and "
                    + "(({2} is null) or ({2} > '{0}'))";

      string result;

      result = string.Format(format,
                             opDate.ToShortDateString(),
                             beganDateColumnName,
                             endedDateColumnName);

      return result;
    }

    /// <summary>
    /// Returns a filter string that will produce active rows given a specific hour
    /// for the BEGIN_DATE and END_DATE columns.
    /// </summary>
    /// <param name="opDate">The date to test.</param>
    /// <returns>The filter string.</returns>
    public static string ActiveRows(DateTime opDate)
    {
      string result;

      result = ActiveRows(opDate, "BEGIN_DATE", "END_DATE");

      return result;
    }

    #endregion


    #region Active Rows For Location

    /// <summary>
    /// Returns a filter string that will produce active rows given a specific hour
    /// for the specified began and ended columns.
    /// </summary>
    /// <param name="monLocId">The MON_LOC_ID to test.</param>
    /// <param name="opDate">The date to test.</param>
    /// <param name="opHour">The hour to test.</param>
    /// <param name="beganDateColumnName">The began date column of the data.</param>
    /// <param name="beganHourColumnName">The began hour column of the data.</param>
    /// <param name="endedDateColumnName">The ended date column of the data.</param>
    /// <param name="endedHourColumnName">The ended hour column of the data.</param>
    /// <returns>The filter string.</returns>
    public static string ActiveRowsForLocation(string monLocId, DateTime opDate, int opHour,
                                               string beganDateColumnName, string beganHourColumnName,
                                               string endedDateColumnName, string endedHourColumnName)
    {
      string result;

      result = AndFilters(AsEquals("MON_LOC_ID", monLocId),
                          ActiveRows(opDate, opHour,
                                     beganDateColumnName, beganHourColumnName,
                                     endedDateColumnName, endedHourColumnName));

      return result;
    }

    /// <summary>
    /// Returns a filter string that will produce active rows given a specific hour
    /// for the BEGIN_DATE, BEGIN_HOUR, END_DATE and END_HOUR columns.
    /// </summary>
    /// <param name="monLocId">The MON_LOC_ID to test.</param>
    /// <param name="opDate">The date to test.</param>
    /// <param name="opHour">The hour to test.</param>
    /// <returns>The filter string.</returns>
    public static string ActiveRowsForLocation(string monLocId, DateTime opDate, int opHour)
    {
      string result;

      result = ActiveRowsForLocation(monLocId, opDate, opHour, "BEGIN_DATE", "BEGIN_HOUR", "END_DATE", "END_HOUR");

      return result;
    }

    /// <summary>
    /// Returns a filter string that will produce active rows given a specific hour
    /// for the specified began and ended columns.
    /// </summary>
    /// <param name="monLocId">The MON_LOC_ID to test.</param>
    /// <param name="opDate">The date to test.</param>
    /// <param name="beganDateColumnName">The began date column of the data.</param>
    /// <param name="endedDateColumnName">The ended date column of the data.</param>
    /// <returns>The filter string.</returns>
    public static string ActiveRowsForLocation(string monLocId, DateTime opDate, string beganDateColumnName, string endedDateColumnName)
    {
      string result;

      result = AndFilters(AsEquals("MON_LOC_ID", monLocId),
                          ActiveRows(opDate,
                                     beganDateColumnName,
                                     endedDateColumnName));

      return result;
    }

    /// <summary>
    /// Returns a filter string that will produce active rows given a specific hour
    /// for the BEGIN_DATE and END_DATE columns.
    /// </summary>
    /// <param name="monLocId">The MON_LOC_ID to test.</param>
    /// <param name="opDate">The date to test.</param>
    /// <returns>The filter string.</returns>
    public static string ActiveRowsForLocation(string monLocId, DateTime opDate)
    {
      string result;

      result = ActiveRowsForLocation(monLocId, opDate, "BEGIN_DATE", "END_DATE");

      return result;
    }

    #endregion
  }
}
