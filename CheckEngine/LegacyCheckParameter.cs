using System;
using System.Data;

using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Extensions;


namespace ECMPS.Checks.CheckEngine
{

  /// <summary>
  /// Legacy CheckParameter class
  /// </summary>
  public class cLegacyCheckParameter
  {
    /// <summary>
    /// parameter name
    /// </summary>
    public string ParameterName;

    /// <summary>
    /// parameter value
    /// </summary>
    public object ParameterValue;

    /// <summary>
    /// is parameter missing
    /// </summary>
    public bool ParameterMissing;

    /// <summary>
    /// parameter type
    /// </summary>
    public eParameterDataType ParameterType;

    /// <summary>
    /// InitRuleCheck
    /// </summary>
    public cParameterizedCheck InitRuleCheck;

    /// <summary>
    /// is an accumulator
    /// </summary>
    public bool IsAccumulator;

    /// <summary>
    /// is an array
    /// </summary>
    public bool IsArray;

    /// <summary>
    /// Constructor
    /// </summary>
    public cLegacyCheckParameter()
    {
      ParameterName = "";
      ParameterValue = null;
      ParameterMissing = true;
      ParameterType = eParameterDataType.String;
      InitRuleCheck = null;
      IsAccumulator = false;
      IsArray = false;
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="ThisParameterName"></param>
    /// <param name="ThisParameterValue"></param>
    /// <param name="ThisParameterType"></param>
    public cLegacyCheckParameter(string ThisParameterName, object ThisParameterValue, eParameterDataType ThisParameterType)
    {
      ParameterName = ThisParameterName;
      ParameterValue = ThisParameterValue;
      ParameterMissing = false;
      ParameterType = ThisParameterType;
      InitRuleCheck = null;
      IsAccumulator = false;
      IsArray = false;
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="ThisParameterName"></param>
    /// <param name="ThisParameterValue"></param>
    /// <param name="ThisParameterType"></param>
    /// <param name="ThisIsAccumulator"></param>
    /// <param name="ThisIsArray"></param>
    public cLegacyCheckParameter(string ThisParameterName, object ThisParameterValue, eParameterDataType ThisParameterType,
                                 bool ThisIsAccumulator, bool ThisIsArray)
    {
      ParameterName = ThisParameterName;
      ParameterValue = ThisParameterValue;
      ParameterMissing = false;
      ParameterType = ThisParameterType;
      InitRuleCheck = null;
      IsAccumulator = ThisIsAccumulator;
      IsArray = ThisIsArray;
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="AInitRuleCheck"></param>
    public cLegacyCheckParameter(cParameterizedCheck AInitRuleCheck)
    {
      ParameterName = "";
      ParameterValue = null;
      ParameterMissing = true;
      ParameterType = eParameterDataType.String;
      InitRuleCheck = AInitRuleCheck;
      IsAccumulator = false;
      IsArray = false;
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="ThisParameterName"></param>
    /// <param name="ThisParameterValue"></param>
    /// <param name="ThisParameterType"></param>
    /// <param name="AInitRuleCheck"></param>
    public cLegacyCheckParameter(string ThisParameterName, object ThisParameterValue, eParameterDataType ThisParameterType,
                                 cParameterizedCheck AInitRuleCheck)
    {
      ParameterName = ThisParameterName;
      ParameterValue = ThisParameterValue;
      ParameterMissing = false;
      ParameterType = ThisParameterType;
      InitRuleCheck = AInitRuleCheck;
      IsAccumulator = false;
      IsArray = false;
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="ThisParameterName"></param>
    /// <param name="ThisParameterValue"></param>
    /// <param name="ThisParameterType"></param>
    /// <param name="AInitRuleCheck"></param>
    /// <param name="ThisIsAccumulator"></param>
    /// <param name="ThisIsArray"></param>
    public cLegacyCheckParameter(string ThisParameterName, object ThisParameterValue, eParameterDataType ThisParameterType,
                                 cParameterizedCheck AInitRuleCheck,
                                 bool ThisIsAccumulator, bool ThisIsArray)
    {
      ParameterName = ThisParameterName;
      ParameterValue = ThisParameterValue;
      ParameterMissing = false;
      ParameterType = ThisParameterType;
      InitRuleCheck = AInitRuleCheck;
      IsAccumulator = ThisIsAccumulator;
      IsArray = ThisIsArray;
    }

    /// <summary>
    /// ValueAsBool
    /// </summary>
    /// <param name="ADefault"></param>
    /// <returns></returns>
    public bool ValueAsBool(bool ADefault)
    {
      if ((ParameterValue == null) || (ParameterValue == DBNull.Value) ||
          (ParameterValue.GetType() != Type.GetType("System.Boolean")))
        return ADefault;
      else
        return Convert.ToBoolean(ParameterValue);
    }

    /// <summary>
    /// ValueAsBool
    /// </summary>
    /// <returns></returns>
    public bool ValueAsBool()
    {
      return ValueAsBool(false);
    }

    /// <summary>
    /// ValueAsBoolArray
    /// </summary>
    /// <returns></returns>
    public bool[] ValueAsBoolArray()
    {
      try
      {
        if (ParameterValue == null)
          return null;
        else
          return (bool[])ParameterValue;
      }
      catch
      {
        return null;
      }
    }

    /// <summary>
    /// ValueAsDataRowView
    /// </summary>
    /// <returns></returns>
    public DataRowView ValueAsDataRowView()
    {
      if ((ParameterValue == null) || !(ParameterValue is DataRowView))
        return null;
      else
        return (DataRowView)ParameterValue;
    }

    /// <summary>
    /// ValueAsDataView
    /// </summary>
    /// <returns></returns>
    public DataView ValueAsDataView()
    {
      if ((ParameterValue == null) || !(ParameterValue is DataView))
        return null;
      else
        return (DataView)ParameterValue;
    }

    /// <summary>
    /// ValueAsDateTime
    /// </summary>
    /// <param name="ADateType"></param>
    /// <returns></returns>
    public DateTime ValueAsDateTime(DateTypes ADateType)
    {
      if ((ParameterValue == null) || (ParameterValue == DBNull.Value) ||
          (ParameterValue.GetType() != Type.GetType("System.DateTime")))
      {
        if (ADateType == DateTypes.START)
          return DateTime.MinValue;
        else
          return DateTime.MaxValue;
      }
      else
        return Convert.ToDateTime(ParameterValue);
    }

    /// <summary>
    /// ValueAsDecimal
    /// </summary>
    /// <param name="ADefault"></param>
    /// <returns></returns>
    public decimal ValueAsDecimal(decimal ADefault)
    {
      if ((ParameterValue == null) || (ParameterValue == DBNull.Value) ||
          ((ParameterValue.GetType() != Type.GetType("System.Decimal")) &&
           (ParameterValue.GetType() != Type.GetType("System.Int16")) &&
           (ParameterValue.GetType() != Type.GetType("System.Int32")) &&
           (ParameterValue.GetType() != Type.GetType("System.Int64"))))
        return ADefault;
      else
        return Convert.ToDecimal(ParameterValue);
    }

    /// <summary>
    /// ValueAsDecimal
    /// </summary>
    /// <returns></returns>
    public decimal ValueAsDecimal()
    {
      return ValueAsDecimal(decimal.MinValue);
    }

    /// <summary>
    /// ValueAsDecimalArray
    /// </summary>
    /// <returns></returns>
    public decimal[] ValueAsDecimalArray()
    {
      try
      {
        if (ParameterValue == null)
          return null;
        else
          return (decimal[])ParameterValue;
      }
      catch
      {
        return null;
      }
    }

    /// <summary>
    /// ValueAsInt
    /// </summary>
    /// <param name="ADefault"></param>
    /// <returns></returns>
    public int ValueAsInt(int ADefault)
    {
      if ((ParameterValue == null) || (ParameterValue == DBNull.Value) ||
          ((ParameterValue.GetType() != Type.GetType("System.Int32")) &&
           (ParameterValue.GetType() != Type.GetType("System.Int16"))))
        return ADefault;
      else
        return Convert.ToInt32(ParameterValue);
    }

    /// <summary>
    /// ValueAsInt
    /// </summary>
    /// <returns></returns>
    public int ValueAsInt()
    {
      return ValueAsInt(int.MinValue);
    }

    /// <summary>
    /// ValueAsIntArray
    /// </summary>
    /// <returns></returns>
    public int[] ValueAsIntArray()
    {
      try
      {
        if (ParameterValue == null)
          return null;
        else
          return (int[])ParameterValue;
      }
      catch
      {
        return null;
      }
    }

    /// <summary>
    /// ValueAsNullOrDecimal
    /// </summary>
    /// <returns></returns>
    public decimal? ValueAsNullOrDecimal()
    {
      if ((ParameterValue == null) || (ParameterValue == DBNull.Value) ||
          (ParameterValue.GetType() != Type.GetType("System.Decimal")))
        return (decimal?)null;
      else
        return Convert.ToDecimal(ParameterValue);
    }

    /// <summary>
    /// ValueAsNullOrInt
    /// </summary>
    /// <returns></returns>
    public int? ValueAsNullOrInt()
    {
      if ((ParameterValue == null) || (ParameterValue == DBNull.Value) ||
          ((ParameterValue.GetType() != Type.GetType("System.Int32")) &&
           (ParameterValue.GetType() != Type.GetType("System.Int16"))))
        return (int?)null;
      else
        return Convert.ToInt32(ParameterValue);
    }

    /// <summary>
    /// ValueAsString
    /// </summary>
    /// <returns></returns>
    public string ValueAsString()
    {
      if (ParameterValue == null)
        return "";
      if (ParameterValue.GetType() == Type.GetType("System.DateTime"))
      {
        DateTime ParameterValueDate = (DateTime)ParameterValue;
        return ParameterValueDate.ToShortDateString();
      }
      else
        return ParameterValue.ToString();
    }

    /// <summary>
    /// ValueAsString
    /// </summary>
    /// <param name="FieldName"></param>
    /// <returns></returns>
    public string ValueAsString(string FieldName)
    {
      if (((DataRowView)ParameterValue)[FieldName].GetType() == Type.GetType("System.DateTime"))
      {
        DateTime ParameterValueDate = (DateTime)((DataRowView)ParameterValue)[FieldName];
        return ParameterValueDate.ToShortDateString();
      }
      else
      {
        return ((DataRowView)ParameterValue)[FieldName].ToString();
      }
    }

    /// <summary>
    /// ValueAsStringArray
    /// </summary>
    /// <returns></returns>
    public string[] ValueAsStringArray()
    {
      try
      {
        if (ParameterValue == null)
          return null;
        else
          return (string[])ParameterValue;
      }
      catch
      {
        return null;
      }
    }

    /// <summary>
    /// SameUnderlyingInitCheck
    /// </summary>
    /// <param name="ARuleCheck"></param>
    /// <returns></returns>
    public bool SameUnderlyingInitCheck(cParameterizedCheck ARuleCheck)
    {
      return ((InitRuleCheck != null) && InitRuleCheck.IsSameCheck(ARuleCheck));
    }
  }

  /// <summary>
  /// Extensions needed for Legacy Check Parameters
  /// </summary>
  public static class cLegacyCheckParameterExtensions
  {

    /// <summary>
    /// Convert a cLegacyCheckParameter value to a nullable type
    /// </summary>
    /// <param name="parameter">The cLegacyCheckParameter parameter</param>
    /// <returns>Nullabe type representation of the cLegacyCheckParameter value</returns>
    public static DateTime AsBeginDateTime(this cLegacyCheckParameter parameter)
    {
      DateTime result;

      result = parameter.ParameterValue.AsDateTime(DateTime.MinValue);

      return result;
    }

    /// <summary>
    /// Convert a cLegacyCheckParameter value to a nullable type
    /// </summary>
    /// <param name="parameter">The cLegacyCheckParameter parameter</param>
    /// <returns>Nullabe type representation of the cLegacyCheckParameter value</returns>
    public static bool? AsBoolean(this cLegacyCheckParameter parameter)
    {
      bool? result;

      result = parameter.ParameterValue.AsBoolean();

      return result;
    }

    /// <summary>
    /// Convert a cLegacyCheckParameter value to a nullable type
    /// </summary>
    /// <param name="parameter">The cLegacyCheckParameter parameter</param>
    /// <param name="defaultValue">The default value for null and unconvertable values.</param>
    /// <returns>The value converted to the type</returns>
    public static bool AsBoolean(this cLegacyCheckParameter parameter, bool defaultValue)
    {
      bool result;

      result = parameter.ParameterValue.AsBoolean(defaultValue);

      return result;
    }

    /// <summary>
    /// Convert an object to a Data Row View.
    /// </summary>
    /// <param name="parameter">The cLegacyCheckParameter parameter</param>
    /// <returns>The data row view or null if the value connot be converted.</returns>
    public static DataRowView AsDataRowView(this cLegacyCheckParameter parameter)
    {
      DataRowView result;

      if ((parameter == null) || (parameter.ParameterValue == null) ||
          !(parameter.ParameterValue is DataRowView))
        result = (DataRowView)null;
      else
        result = (DataRowView)parameter.ParameterValue;

      return result;
    }

    /// <summary>
    /// Convert an object to a Data View.
    /// </summary>
    /// <param name="parameter">The cLegacyCheckParameter parameter</param>
    /// <returns>The data view or null if the value connot be converted.</returns>
    public static DataView AsDataView(this cLegacyCheckParameter parameter)
    {
      DataView result;

      if ((parameter == null) || (parameter.ParameterValue == null) ||
          !(parameter.ParameterValue is DataView))
        result = (DataView)null;
      else
        result = (DataView)parameter.ParameterValue;

      return result;
    }

    /// <summary>
    /// Convert a cLegacyCheckParameter value to a nullable type
    /// </summary>
    /// <param name="parameter">The cLegacyCheckParameter parameter</param>
    /// <returns>Nullabe type representation of the cLegacyCheckParameter value</returns>
    public static DateTime? AsDateTime(this cLegacyCheckParameter parameter)
    {
      DateTime? result;

      result = parameter.ParameterValue.AsDateTime();

      return result;
    }

    /// <summary>
    /// Convert a cLegacyCheckParameter value to a nullable type
    /// </summary>
    /// <param name="parameter">The cLegacyCheckParameter parameter</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>Nullabe type representation of the cLegacyCheckParameter value</returns>
    public static DateTime AsDateTime(this cLegacyCheckParameter parameter, DateTime defaultValue)
    {
      DateTime result;

      result = parameter.ParameterValue.AsDateTime(defaultValue);

      return result;
    }

    /// <summary>
    /// Convert a cLegacyCheckParameter value to a nullable type
    /// </summary>
    /// <param name="parameter">The cLegacyCheckParameter parameter</param>
    /// <returns>Nullabe type representation of the cLegacyCheckParameter value</returns>
    public static decimal? AsDecimal(this cLegacyCheckParameter parameter)
    {
      decimal? result;

      result = parameter.ParameterValue.AsDecimal();

      return result;
    }

    /// <summary>
    /// Convert a cLegacyCheckParameter value to a nullable type
    /// </summary>
    /// <param name="parameter">The cLegacyCheckParameter parameter</param>
    /// <param name="defaultValue">The default value for null and unconvertable values.</param>
    /// <returns>The value converted to the type</returns>
    public static decimal AsDecimal(this cLegacyCheckParameter parameter, decimal defaultValue)
    {
      decimal result;

      result = parameter.ParameterValue.AsDecimal(defaultValue);

      return result;
    }

    /// <summary>
    /// Convert a cLegacyCheckParameter value to a nullable type
    /// </summary>
    /// <param name="parameter">The cLegacyCheckParameter parameter</param>
    /// <returns>Nullible type representation of the cLegacyCheckParameter value</returns>
    public static DateTime AsEndDateTime(this cLegacyCheckParameter parameter)
    {
      DateTime result;

      result = parameter.ParameterValue.AsDateTime(DateTime.MaxValue);

      return result;
    }

    /// <summary>
    /// Convert the database date value to a nullable System.DateTime
    /// </summary>
    /// <param name="parameter">The cLegacyCheckParameter parameter</param>
    /// <returns>Nullable System.DateTime representation of the value</returns>
    public static eHour? AsHour(this cLegacyCheckParameter parameter)
    {
      eHour? result;

      result = parameter.ParameterValue.AsHour();

      return result;
    }

    /// <summary>
    /// Convert a cLegacyCheckParameter value to a nullable type
    /// </summary>
    /// <param name="parameter">The cLegacyCheckParameter parameter</param>
    /// <returns>Nullabe type representation of the cLegacyCheckParameter value</returns>
    public static int? AsInteger(this cLegacyCheckParameter parameter)
    {
      int? result;

      result = parameter.ParameterValue.AsInteger();

      return result;
    }

    /// <summary>
    /// Convert a cLegacyCheckParameter value to a nullable type
    /// </summary>
    /// <param name="parameter">The cLegacyCheckParameter parameter</param>
    /// <param name="defaultValue">The default value for null and unconvertable values.</param>
    /// <returns>The value converted to the type</returns>
    public static int AsInteger(this cLegacyCheckParameter parameter, int defaultValue)
    {
      int result;

      result = parameter.ParameterValue.AsInteger(defaultValue);

      return result;
    }

    /// <summary>
    /// Convert a cLegacyCheckParameter value to a nullable type
    /// </summary>
    /// <param name="parameter">The cLegacyCheckParameter parameter</param>
    /// <returns>Nullabe type representation of the cLegacyCheckParameter value</returns>
    public static long? AsLong(this cLegacyCheckParameter parameter)
    {
      long? result;

      result = parameter.ParameterValue.AsLong();

      return result;
    }

    /// <summary>
    /// Convert a cLegacyCheckParameter value to a nullable type
    /// </summary>
    /// <param name="parameter">The cLegacyCheckParameter parameter</param>
    /// <param name="defaultValue">The default value for null and unconvertable values.</param>
    /// <returns>The value converted to the type</returns>
    public static long AsLong(this cLegacyCheckParameter parameter, long defaultValue)
    {
      long result;

      result = parameter.ParameterValue.AsLong(defaultValue);

      return result;
    }

    /// <summary>
    /// Convert a cLegacyCheckParameter value to a nullable type
    /// </summary>
    /// <param name="parameter">The cLegacyCheckParameter parameter</param>
    /// <returns>Nullabe type representation of the cLegacyCheckParameter value</returns>
    public static string AsString(this cLegacyCheckParameter parameter)
    {
      string result;

      result = parameter.ParameterValue.AsString();

      return result;
    }

  }

}
