using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using ECMPS.Definitions.Extensions;


namespace ECMPS.Checks.TypeUtilities
{
  /// <summary>
  /// Extensions needed by type utilities
  /// </summary>
  public static class cExtensions
  {

    #region Typing To and Inspection Of

    #region DateTime and DateTime?

    /// <summary>
    /// Returns the value when not null, otherwise returns the default value.
    /// </summary>
    /// <param name="value">The value to default.</param>
    /// <param name="dateType">Indcates whether the default is the DateTime Min or Max.</param>
    /// <returns>Resturns the value or the default if value is null.</returns>
    public static DateTime Default(this DateTime? value, DateTypes dateType)
    {
      DateTime result;

      if (value.HasValue)
        result = value.Value;
      else
      {
        if (dateType == DateTypes.START)
          result = DateTime.MinValue;
        else
          result = DateTime.MaxValue;
      }

      return result;
    }

    #endregion

    #region eHourMeasureParameter

    /// <summary>
    /// Returns the eHourMeasureParameter value for the passed hour measure code.
    /// </summary>
    /// <param name="hourMeasureCd">The hour measured code.</param>
    /// <returns>The eHourMeasureParameter value or null if none match.</returns>
    public static eHourMeasureParameter? AsHourMeasureParameter(this string hourMeasureCd)
    {
      eHourMeasureParameter? result;

      switch (hourMeasureCd)
      {
        case "CO2C": result = eHourMeasureParameter.Co2c; break;
        case "DENSITY": result = eHourMeasureParameter.Density; break;
        case "FF": result = eHourMeasureParameter.Ff; break;
        case "FLOW": result = eHourMeasureParameter.Flow; break;
        case "GCV": result = eHourMeasureParameter.Gcv; break;
        case "H2O": result = eHourMeasureParameter.H2o; break;
        case "NOXC": result = eHourMeasureParameter.Noxc; break;
        case "NOXR": result = eHourMeasureParameter.Noxr; break;
        case "O2D": result = eHourMeasureParameter.O2d; break;
        case "O2W": result = eHourMeasureParameter.O2w; break;
        case "SO2C": result = eHourMeasureParameter.So2c; break;
        case "SULFER": result = eHourMeasureParameter.Sulfer; break;
        default: result = null; break;
      }

      return result;
    }

    #endregion

    #region eHour, eHour? and cOperatingHour

    /// <summary>
    /// Convert the database date value to a nullable System.DateTime
    /// </summary>
    /// <param name="value">The database date to convert</param>
    /// <returns>Nullable System.DateTime representation of the value</returns>
    public static DateTime? AsDateTime(this cOperatingHour value)
    {
      if ((value == null) || (value.Date == null))
      {
        return null;
      }
      else
      {
        return value.Date;
      }
    }

    /// <summary>
    /// Convert a database value to a nullable hour
    /// </summary>
    /// <param name="value">The database integer to convert</param>
    /// <returns>Nullabe integer representation of the value</returns>
    public static eHour? AsHour(this object value)
    {
      eHour? result;

      if ((value == DBNull.Value) || (value == null))
        result = null;
      else
      {
        int hour;

        if (int.TryParse(value.ToString(), out hour))
        {
          if (hour < 0) hour = 0;
          else if (hour > 23) hour = 23;

          result = (eHour)hour;
        }
        else
          result = null;
      }

      return result;
    }

    /// <summary>
    /// Convert the database date value to a nullable System.DateTime
    /// </summary>
    /// <param name="value">The database date to convert</param>
    /// <returns>Nullable System.DateTime representation of the value</returns>
    public static eHour? AsHour(this cOperatingHour value)
    {
      if ((value == null) || (value.Date == null))
      {
        return null;
      }
      else
      {
        return value.Hour;
      }
    }

    /// <summary>
    /// Convert a database eHour value to a nullable integer
    /// </summary>
    /// <param name="value">The database integer to convert</param>
    /// <returns>Nullabe integer representation of the value</returns>
    public static int AsInteger(this eHour value)
    {
      return (int)value;
    }

    /// <summary>
    /// Convert a database eHour value to a nullable integer
    /// </summary>
    /// <param name="value">The database integer to convert</param>
    /// <returns>Nullabe integer representation of the value</returns>
    public static int? AsInteger(this eHour? value)
    {
      if (value == null)
      {
        return null;
      }
      else
      {
        return (int)value;
      }
    }

    /// <summary>
    /// Returns the DB value for the date part of an operating hour.
    /// </summary>
    /// <param name="operatingHour">The operating hour</param>
    /// <returns>The DB value for the date part of an operating hour.</returns>
    public static object DbDate(this cOperatingHour operatingHour)
    {
      object result;

      if (operatingHour == null)
        result = DBNull.Value;
      else
        result = operatingHour.Date;

      return result;
    }

    /// <summary>
    /// Returns the DB value for the hour part of an operating hour.
    /// </summary>
    /// <param name="operatingHour">The operating hour</param>
    /// <returns>The DB value for the hour part of an operating hour.</returns>
    public static object DbHour(this cOperatingHour operatingHour)
    {
      object result;

      if (operatingHour == null)
        result = DBNull.Value;
      else
        result = operatingHour.Hour;

      return result;
    }

    /// <summary>
    /// Convert the passed value to it DB representation.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>The DB representation.</returns>
    public static object DbValue(this eHour value)
    {
      object result;

      result = value.AsInteger();

      return result;
    }

    /// <summary>
    /// Convert the passed value to it DB representation.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>The DB representation.</returns>
    public static object DbValue(this eHour? value)
    {
      object result;

      if (value == null)
        result = DBNull.Value;
      else
        result = value.Value.AsInteger();

      return result;
    }

    /// <summary>
    /// Returns the value when not null, otherwise returns the default value.
    /// </summary>
    /// <param name="value">The value to default.</param>
    /// <param name="dateType">Indicates whether the default is for a begin or end date.</param>
    /// <returns>Resturns the value or the default if value is null.</returns>
    public static eHour Default(this eHour? value, DateTypes dateType)
    {
      eHour result;

      if (value.HasValue)
        result = value.Value;
      else
      {
        if (dateType == DateTypes.START)
          result = eHour.h00;
        else
          result = eHour.h23;
      }

      return result;
    }

    #endregion

    #region Integer and Integer?

    /// <summary>
    /// Returns the begin hour value when the quarter for a specific year is stored as 
    /// an integer value calculated as (4 * year) + (quarter - 1).
    /// 
    /// If the combined value is null then the quarter is null.
    /// If the combined value is less than zero then the quarter value is -1.
    /// </summary>
    /// <param name="value">The year/quarter value as a single integer value.</param>
    /// <returns>The quarter part of the year/quarter integer value.</returns>
    public static DateTime IntToBeginHour(this int value)
    {
      DateTime result;

      result = new DateTime(value.IntToYear(), 3 * (value.IntToQuarter() - 1) + 1, 1);

      return result;
    }


    /// <summary>
    /// Returns the begin hour value when the quarter for a specific year is stored as 
    /// an integer value calculated as (4 * year) + (quarter - 1).
    /// 
    /// If the combined value is null then the quarter is null.
    /// If the combined value is less than zero then the quarter value is -1.
    /// </summary>
    /// <param name="value">The year/quarter value as a single integer value.</param>
    /// <returns>The quarter part of the year/quarter integer value.</returns>
    public static DateTime? IntToBeginHour(this int? value)
    {
      DateTime? result;

      if (!value.HasValue)
        result = null;
      else
        result = value.Value.IntToBeginHour();

      return result;
    }


    /// <summary>
    /// Returns the end hour value when the quarter for a specific year is stored as 
    /// an integer value calculated as (4 * year) + (quarter - 1).
    /// 
    /// If the combined value is null then the quarter is null.
    /// If the combined value is less than zero then the quarter value is -1.
    /// </summary>
    /// <param name="value">The year/quarter value as a single integer value.</param>
    /// <returns>The quarter part of the year/quarter integer value.</returns>
    public static DateTime IntToEndHour(this int value)
    {
      DateTime result;

      result = (new DateTime(value.IntToYear(), 3 * value.IntToQuarter() + 1, 1)).AddHours(-1);

      return result;
    }


    /// <summary>
    /// Returns the end hour value when the quarter for a specific year is stored as 
    /// an integer value calculated as (4 * year) + (quarter - 1).
    /// 
    /// If the combined value is null then the quarter is null.
    /// If the combined value is less than zero then the quarter value is -1.
    /// </summary>
    /// <param name="value">The year/quarter value as a single integer value.</param>
    /// <returns>The quarter part of the year/quarter integer value.</returns>
    public static DateTime? IntToEndHour(this int? value)
    {
      DateTime? result;

      if (!value.HasValue)
        result = null;
      else
        result = value.Value.IntToEndHour();

      return result;
    }


    /// <summary>
    /// Returns the quarter value when the quarter for a specific year is stored as 
    /// an integer value calculated as (4 * year) + (quarter - 1).
    /// 
    /// If the combined value is null then the quarter is null.
    /// If the combined value is less than zero then the quarter value is -1.
    /// </summary>
    /// <param name="value">The year/quarter value as a single integer value.</param>
    /// <returns>The quarter part of the year/quarter integer value.</returns>
    public static int IntToQuarter(this int value)
    {
      int result;

      if (value < 0)
        result = -1;
      else
        result = (value % 4) + 1;

      return result;
    }


    /// <summary>
    /// Returns the quarter value when the quarter for a specific year is stored as 
    /// an integer value calculated as (4 * year) + (quarter - 1).
    /// 
    /// If the combined value is null then the quarter is null.
    /// If the combined value is less than zero then the quarter value is -1.
    /// </summary>
    /// <param name="value">The year/quarter value as a single integer value.</param>
    /// <returns>The quarter part of the year/quarter integer value.</returns>
    public static int? IntToQuarter(this int? value)
    {
      int? result;

      if (!value.HasValue)
        result = null;
      else
        result = value.Value.IntToQuarter();

      return result;
    }


    /// <summary>
    /// Returns the year value when the quarter for a specific year is stored as 
    /// an integer value calculated as (4 * year) + (quarter - 1).
    /// 
    /// If the combined value is null then the year is null.
    /// If the combined value is less than zero then the year value is -1.
    /// </summary>
    /// <param name="value">The year/quarter value as a single integer value.</param>
    /// <returns>The quarter part of the year/quarter integer value.</returns>
    public static int IntToYear(this int value)
    {
      int result;

      if (value < 0)
        result = -1;
      else
        result = value / 4;

      return result;
    }


    /// <summary>
    /// Returns the year value when the quarter for a specific year is stored as 
    /// an integer value calculated as (4 * year) + (quarter - 1).
    /// 
    /// If the combined value is null then the year is null.
    /// If the combined value is less than zero then the year value is -1.
    /// </summary>
    /// <param name="value">The year/quarter value as a single integer value.</param>
    /// <returns>The quarter part of the year/quarter integer value.</returns>
    public static int? IntToYear(this int? value)
    {
      int? result;

      if (!value.HasValue)
        result = null;
      else
        result = value.Value.IntToYear();

      return result;
    }

    #endregion

    #region eQuarter and eQuarter?

    /// <summary>
    /// Convert a database eHour value to a nullable integer
    /// </summary>
    /// <param name="value">The eQuarter to convert</param>
    /// <returns>Nullabe integer representation of the value</returns>
    public static int AsInteger(this eQuarter value)
    {
      return (int)value;
    }

    /// <summary>
    /// Convert a database eHour value to a nullable integer
    /// </summary>
    /// <param name="value">The eQuarter to convert</param>
    /// <returns>Nullabe integer representation of the value</returns>
    public static int? AsInteger(this eQuarter? value)
    {
      if (value == null)
      {
        return null;
      }
      else
      {
        return (int)value;
      }
    }

    /// <summary>
    /// Convert a database value to a nullable hour
    /// </summary>
    /// <param name="value">The Date to convert</param>
    /// <returns>Nullabe integer representation of the value</returns>
    public static eQuarter? AsQuarter(this DateTime? value)
    {
      eQuarter? result;

      if (value == null)
        result = null;
      else
      {
        int quarter = ((value.Value.Month - 1) / 3) + 1;
        result = (eQuarter)quarter;
      }

      return result;
    }

    /// <summary>
    /// Convert a database value to a nullable hour
    /// </summary>
    /// <param name="value">The database integer to convert</param>
    /// <returns>Nullabe integer representation of the value</returns>
    public static eQuarter? AsQuarter(this object value)
    {
      eQuarter? result;

      if ((value == DBNull.Value) || (value == null))
        result = null;
      else
      {
        int quarter;

        if (int.TryParse(value.ToString(), out quarter))
        {
          if (quarter < 1) quarter = 1;
          else if (quarter > 4) quarter = 4;

          result = (eQuarter)quarter;
        }
        else
          result = null;
      }

      return result;
    }

    #endregion

    #region cReportingPeriod

    /// <summary>
    /// Convert object to a cReportingPeriod.
    /// </summary>
    /// <param name="value">The object to convert.</param>
    /// <returns>Returns cReportingPeriod, or null if conversion fails.</returns>
    public static cReportingPeriod AsReportingPeriod(this object value)
    {
      cReportingPeriod result;

      try
      {
        result = (cReportingPeriod)value;
      }
      catch
      {
        result = null;
      }

      return result;
    }

    #endregion

    #endregion


    #region DataView Filter to Row

    /// <summary>
    /// Returns the first row with the earliest date in the view.
    /// </summary>
    /// <param name="view">The view to search.</param>
    /// <param name="dateColumn">The date column to compare.</param>
    /// <returns>The earliest row.</returns>
    public static DataRowView Earliest(this DataView view, string dateColumn)
    {
      DataRowView result;

      if ((view != null) && (view.Table != null) && (view.Count > 0)
          && view.Table.Columns.Contains(dateColumn)
          && (view.Table.Columns[dateColumn].DataType == typeof(DateTime)))
      {

        DataRowView earliestRow = view[0];
        DateTime? earliestDate = earliestRow[dateColumn].AsDateTime();

        foreach (DataRowView row in view)
        {
          DateTime? date = row[dateColumn].AsDateTime();

          if (date.Default(DateTypes.START) < earliestDate.Default(DateTypes.START))
          {
            earliestRow = row;
            earliestDate = date;
          }
        }

        return earliestRow;
      }
      else
        result = null;

      return result;
    }

    /// <summary>
    /// Returns the last row with the latest date in the view.
    /// </summary>
    /// <param name="view">The view to search.</param>
    /// <param name="dateColumn">The date column to compare.</param>
    /// <returns>The latest row.</returns>
    public static DataRowView Latest(this DataView view, string dateColumn)
    {
      DataRowView result;

      if ((view != null) && (view.Table != null) && (view.Count > 0)
          && view.Table.Columns.Contains(dateColumn) 
          && (view.Table.Columns[dateColumn].DataType == typeof(DateTime)))
      {

        DataRowView latestRow = view[0];
        DateTime? latestDate = latestRow[dateColumn].AsDateTime();

        foreach (DataRowView row in view)
        {
          DateTime? date = row[dateColumn].AsDateTime() ;

          if (date.Default(DateTypes.END) >= latestDate.Default(DateTypes.END))
          {
            latestRow = row;
            latestDate = date;
          }
        }

        return latestRow;
      }
      else
        result = null;

      return result;
    }

    #endregion


    #region RowFilter and FilterCondition

    /// <summary>
    /// Returns the filter conditions for the passed field.
    /// </summary>
    /// <param name="rowFilter">The filter conditions to check.</param>
    /// <param name="fieldName">The field name to find.</param>
    /// <returns>The filter conditions for the passed field, zero length if none are found.</returns>
    public static cFilterCondition[] Find(this cFilterCondition[] rowFilter, string fieldName)
    {
      int count = 0;

      foreach (cFilterCondition filtercondition in rowFilter)
      {
        if (filtercondition.Field.ToUpper() == fieldName.ToUpper())
          count += 1;
      }

      cFilterCondition[] result = new cFilterCondition[count];

      int position = 0;

      foreach (cFilterCondition filtercondition in rowFilter)
      {
        if (filtercondition.Field.ToUpper() == fieldName.ToUpper())
        {
          result[position] = filtercondition;
          position += 1;
        }
      }

      return result;
    }

    #endregion

  }
}
