using System;
using System.Collections.Generic;
using System.Text;

namespace ECMPS.Checks.TypeUtilities
{
  /// <summary>
  /// The types of date/hour objects
  /// </summary>
  public enum DateTypes
  {
    /// <summary>
    /// A BEGIN_DATE or BEGIN_HOUR field
    /// </summary>
    START,

    /// <summary>
    /// An END_DATE or END_HOUR field
    /// </summary>
    END
  }

  /// <summary>
  /// Class to convert database objects to their real types in a DBNull.Value aware way
  /// </summary>
  public class cDBConvert
  {

    #region Get Database Value Functions (Handle NULL values)

    /// <summary>
    /// Convert a database object to a bool type, handling NULL values properly
    /// </summary>
    /// <param name="DBBooleanValue">The database object in question</param>
    /// <param name="Default">The default if the value is NULL</param>
    /// <returns>Boolean representation of value, or default if NULL</returns>
    public static bool ToBoolean(object DBBooleanValue, bool Default)
    {
      if ((DBBooleanValue == DBNull.Value) || (DBBooleanValue == null))
      {
        return Default;
      }
      else
      {
        return Convert.ToBoolean(DBBooleanValue);
      }
    }

    /// <summary>
    /// Convert a database object to a bool type, false if value is NULL
    /// </summary>
    /// <param name="DBBooleanValue">The database object in question</param>
    /// <returns>Boolean representation of value, or false if NULL</returns>
    public static bool ToBoolean(object DBBooleanValue)
    {
      return ToBoolean(DBBooleanValue, false);
    }

    /// <summary>
    /// Convert the database date value to a System.DateTime
    /// </summary>
    /// <param name="DBDateValue">The database date object in question</param>
    /// <param name="DateType">Is it a BEGIN_DATE or an END_DATE field?</param>
    /// <returns>System.DateTime object, or defaults if NULL</returns>
    public static DateTime ToDate(object DBDateValue, DateTypes DateType)
    {
      if ((DBDateValue == DBNull.Value) || (DBDateValue == null))
      {
        if (DateType == DateTypes.START)
        {
          return DateTime.MinValue;
        }
        else
        {
          return DateTime.MaxValue;
        }
      }
      else
      {
        return Convert.ToDateTime(DBDateValue);
      }
    }

    /// <summary>
    /// Convert a database decimal value to a decimal object
    /// <remarks>returns decimal.MinValue if NULL</remarks>
    /// </summary>
    /// <param name="DBDecimalValue">The database decimal object in question</param>
    /// <param name="DefaultValue">The default value if object is NULL</param>
    /// <returns>decimal object, or decimal.MinValue if NULL</returns>
    public static decimal ToDecimal(object DBDecimalValue, decimal DefaultValue)
    {
      if ((DBDecimalValue == DBNull.Value) || (DBDecimalValue == null))
      {
        return DefaultValue;
      }
      else
      {
        return Convert.ToDecimal(DBDecimalValue);
      }
    }

    /// <summary>
    /// Convert a database decimal value to a decimal object
    /// <remarks>returns decimal.MinValue if NULL</remarks>
    /// </summary>
    /// <param name="DBDecimalValue">The database decimal object in question</param>
    /// <returns>decimal object, or decimal.MinValue if NULL</returns>
    public static decimal ToDecimal(object DBDecimalValue)
    {
      if ((DBDecimalValue == DBNull.Value) || (DBDecimalValue == null))
      {
        return decimal.MinValue;
      }
      else
      {
        return Convert.ToDecimal(DBDecimalValue);
      }
    }

    /// <summary>
    /// Convert a database "HOUR" field to a int object
    /// </summary>
    /// <param name="DBIntegerValue">The database "HOUR" object in question</param>
    /// <param name="DateType">Is it a BEGIN_HOUR or an END_HOUR</param>
    /// <returns>Valid int value for an "HOUR" field</returns>
    public static int ToHour(object DBIntegerValue, DateTypes DateType)
    {
      int nValue = 0;
      if ((DBIntegerValue == DBNull.Value) || (DBIntegerValue == null))
      {
        if (DateType == DateTypes.START)
          nValue = 0;
        else
          nValue = 23;
      }
      else
      {
        nValue = Convert.ToInt32(DBIntegerValue);
        if (nValue < 0)
          nValue = 0;
        else if (nValue > 23)
          nValue = 23;
      }

      return nValue;
    }

    /// <summary>
    /// Convert a database "MINUTE" field to a int object
    /// </summary>
    /// <param name="DBIntegerValue">The database "MINUTE" object in question</param>
    /// <param name="DateType">Is it a BEGIN_MIN or an END_MIN</param>
    /// <returns>Valid int value for an "MINUTE" field</returns>
    public static int ToMinute(object DBIntegerValue, DateTypes DateType)
    {
      int nValue = 0;
      if ((DBIntegerValue == DBNull.Value) || (DBIntegerValue == null))
      {
        if (DateType == DateTypes.START)
          nValue = 0;
        else
          nValue = 59;
      }
      else
      {
        nValue = Convert.ToInt32(DBIntegerValue);
        if (nValue < 0)
          nValue = 0;
        else if (nValue > 59)
          nValue = 59;
      }

      return nValue;
    }

    /// <summary>
    /// Convert a database integer field to an int object
    /// </summary>
    /// <param name="DBIntegerValue">The database integer object in question</param>
    /// <param name="DefaultValue">The default value if object is NULL</param>
    /// <returns>The database object converted to an int</returns>
    public static int ToInteger(object DBIntegerValue, int DefaultValue)
    {
      if ((DBIntegerValue == DBNull.Value) || (DBIntegerValue == null))
      {
        return DefaultValue;
      }
      else
      {
        return Convert.ToInt32(DBIntegerValue);
      }
    }

    /// <summary>
    /// Convert a database integer field to an int object
    /// </summary>
    /// <param name="DBIntegerValue">The database integer object in question</param>
    /// <returns>The database object converted to an int</returns>
    public static int ToInteger(object DBIntegerValue)
    {
      return ToInteger(DBIntegerValue, int.MinValue);
    }

    /// <summary>
    /// Convert a database long field to an long object
    /// </summary>
    /// <param name="DBLongValue">The database long object in question</param>
    /// <returns>The database object converted to an long</returns>
    public static long ToLong(object DBLongValue)
    {
      if ((DBLongValue == DBNull.Value) || (DBLongValue == null))
      {
        return long.MinValue;
      }
      else
      {
        return Convert.ToInt64(DBLongValue);
      }
    }

    /// <summary>
    /// Convert a database string field to a System.string object
    /// </summary>
    /// <param name="DBStringValue">The database string object in question</param>
    /// <returns>The database object converted to an string</returns>
    public static string ToString( object DBStringValue )
    {
        string sValue = "";

        if( (DBStringValue == DBNull.Value) || (DBStringValue == null) )
        {
            sValue = "";
        }
        else
        {
            if( DBStringValue is DateTime )
            {   // use the XML date format!
                DateTime dt = (DateTime)DBStringValue;
                sValue = dt.ToString( "yyyy-MM-ddTHH:mm:ss" );
            }
            else
            {
                sValue = Convert.ToString( DBStringValue ).Trim();
            }
        }

        return sValue;
    }

    #endregion

    #region Get Database Nullable Value Functions

    /// <summary>
    /// Convert a database boolean to a nullable bool type
    /// </summary>
    /// <param name="DBBooleanValue">The database boolean to convert</param>
    /// <returns>Nullable boolean representation of the value</returns>
    public static bool? ToNullableBoolean(object DBBooleanValue)
    {
      if ((DBBooleanValue == DBNull.Value) || (DBBooleanValue == null))
      {
        return null;
      }
      else
      {
        return Convert.ToBoolean(DBBooleanValue);
      }
    }

    /// <summary>
    /// Convert the database date value to a nullable System.DateTime
    /// </summary>
    /// <param name="DBDateValue">The database date to convert</param>
    /// <returns>Nullable System.DateTime representation of the value</returns>
    public static DateTime? ToNullableDateTime(object DBDateValue)
    {
      if ((DBDateValue == DBNull.Value) || (DBDateValue == null))
      {
        return null;
      }
      else
      {
        return Convert.ToDateTime(DBDateValue);
      }
    }

    /// <summary>
    /// Convert a database decimal value to a nullable decimal
    /// </summary>
    /// <param name="DBDecimalValue">The database decimal to convert</param>
    /// <returns>Nullabe decimal representation of the value</returns>
    public static decimal? ToNullableDecimal(object DBDecimalValue)
    {
      if ((DBDecimalValue == DBNull.Value) || (DBDecimalValue == null))
      {
        return null;
      }
      else
      {
        return Convert.ToDecimal(DBDecimalValue);
      }
    }

    /// <summary>
    /// Convert a database decimal value to a nullable integer
    /// </summary>
    /// <param name="DBIntegerValue">The database integer to convert</param>
    /// <returns>Nullabe integer representation of the value</returns>
    public static int? ToNullableInteger(object DBIntegerValue)
    {
      if ((DBIntegerValue == DBNull.Value) || (DBIntegerValue == null))
      {
        return null;
      }
      else
      {
        return Convert.ToInt32(DBIntegerValue);
      }
    }

    /// <summary>
    /// Convert a database decimal value to a nullable long
    /// </summary>
    /// <param name="DBLongValue">The database long to convert</param>
    /// <returns>Nullabe long representation of the value</returns>
    public static long? ToNullableLong(object DBLongValue)
    {
      if ((DBLongValue == DBNull.Value) || (DBLongValue == null))
      {
        return null;
      }
      else
      {
        return Convert.ToInt64(DBLongValue);
      }
    }

    /// <summary>
    /// Convert a database string value to a string
    /// </summary>
    /// <param name="DBStringValue">The database string to convert</param>
    /// <returns>string representation of the value</returns>
    public static string ToNullableString(object DBStringValue)
    {
        if ((DBStringValue == DBNull.Value) || (DBStringValue == null))
        {
            return null;
        }
        else
        {
            return Convert.ToString(DBStringValue);
        }
    }

    #endregion

    #region Set Database Value Function (Set DBNULL values)

    /// <summary>
    /// Convert a type specific value to a value suitable to insert into the database - NULL aware
    /// </summary>
    /// <param name="ADate">The System.DateTime object to convert</param>
    /// <param name="ANullDateType">Is it a BEGIN_DATE or END_DATE</param>
    /// <returns>object representing the value, DBNull.Value if it should be null</returns>
    public static Object ToVariant(DateTime ADate, DateTypes ANullDateType)
    {
      if ((ANullDateType == DateTypes.START) && (ADate == DateTime.MinValue))
        return DBNull.Value;
      else if ((ANullDateType == DateTypes.END) && (ADate == DateTime.MaxValue))
        return DBNull.Value;
      else
        return ADate;
    }

    /// <summary>
    /// Convert a type specific value to a value suitable to insert into the database - NULL aware
    /// </summary>
    /// <param name="ADecimal">The System.Decimal object to convert</param>
    /// <returns>object representing the value, DBNull.Value if it should be null</returns>
    public static Object ToVariant(Decimal ADecimal)
    {
      if (ADecimal == Decimal.MinValue)
        return DBNull.Value;
      else
        return ADecimal;
    }

    /// <summary>
    /// Convert a type specific value to a value suitable to insert into the database - NULL aware
    /// </summary>
    /// <param name="AInteger">The int object to convert</param>
    /// <param name="ANullValue">The value for a "null" integer</param>
    /// <returns>object representing the value, DBNull.Value if it should be null</returns>
    public static Object ToVariant(int AInteger, int ANullValue)
    {
      if (AInteger == ANullValue)
        return DBNull.Value;
      else
        return AInteger;
    }

    /// <summary>
    /// Convert a type specific value to a value suitable to insert into the database - NULL aware
    /// </summary>
    /// <param name="AInteger">The int object to convert</param>
    /// <returns>object representing the value, DBNull.Value if it should be null</returns>
    public static Object ToVariant(int AInteger)
    {
      if (AInteger == int.MinValue)
        return DBNull.Value;
      else
        return AInteger;
    }

    /// <summary>
    /// Convert a type specific value to a value suitable to insert into the database - NULL aware
    /// </summary>
    /// <param name="ALong">The long object to convert</param>
    /// <returns>object representing the value, DBNull.Value if it should be null</returns>
    public static Object ToVariant(long ALong)
    {
      if (ALong == long.MinValue)
        return DBNull.Value;
      else
        return ALong;
    }

    /// <summary>
    /// Convert a type specific value to a value suitable to insert into the database - NULL aware
    /// </summary>
    /// <param name="AString">The System.String object to convert</param>
    /// <returns>object representing the value, DBNull.Value if it should be null</returns>
    public static Object ToVariant(string AString)
    {
      if (string.IsNullOrEmpty(AString))
        return DBNull.Value;
      else
        return AString;
    }

    #endregion

  }
}
