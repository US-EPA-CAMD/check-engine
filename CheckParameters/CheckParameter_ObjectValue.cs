using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ECMPS.Checks.Parameters
{

  /// <summary>
  /// The string value parameter object used by the check engine.
  /// </summary>
  public class cCheckParameterObjectValue : cCheckParameterAbstractValue<object, object>
  {
  
    #region Public Constructors

    /// <summary>
    /// Create and instance of a string value parameter object setting the Parameter name and data type.
    /// </summary>
    /// <param name="AParameterKey">The key of the parameter.</param>
    /// <param name="AParameterName">The name of the parameter.</param>
    /// <param name="ACheckParameters">The parent check parameters object.</param>
    public cCheckParameterObjectValue(int? AParameterKey, string AParameterName,
                                      cCheckParameters ACheckParameters)
      : base(AParameterKey, AParameterName, eParameterDataType.Object, ACheckParameters)
    {
      FIsArray = false;
    }

    #endregion


    #region Public Properties with Fields

    #region Property Fields

    private string FAggregateDelimiter = ",";
    private string FDefaultForNull = "";

    #endregion


    /// <summary>
    /// The deliniter used to separate aggregated (concatenated) strings.
    /// </summary>
    public string AggregateDelimiter
    {
      get { return FAggregateDelimiter; }
    }

    /// <summary>
    /// The default int value when the value is null or dbnull.
    /// </summary>
    public string DefaultForNull
    {
      get { return FDefaultForNull; }
    }

    #endregion


    #region Public Methods: Value As

    #region Boolean and Boolean Array

    /// <summary>
    /// Returns the value as a boolean defaulting the return value if the parameter value
    /// is null, dbnull, or is not a boolean value.
    /// </summary>
    /// <param name="ADefault">The default value.</param>
    /// <returns>The boolean value of the parameter.</returns>
    public bool AsBool(bool ADefault)
    {
      if ((Value == null) || (Value == DBNull.Value) ||
          (Value.GetType() != Type.GetType("System.Boolean")))
        return ADefault;
      else
        return Convert.ToBoolean(Value);
    }

    /// <summary>
    /// Returns the value as a boolean defaulting the return value to false if the parameter value
    /// is null, dbnull, or is not a boolean value.
    /// </summary>
    /// <returns>The boolean value of the parameter.</returns>
    public bool AsBool()
    {
      return AsBool(false);
    }

    /// <summary>
    /// Returns the value as a boolean array defaulting the return value to null if the 
    /// parameter value is null, dbnull, or is not a convertable type.
    /// </summary>
    /// <returns>The boolean array of the parameter.</returns>
    public bool[] AsBoolArray()
    {
      try
      {
        if ((Value == null) || (Value == DBNull.Value) ||
            !(Value is bool[]))
          return null;
        else
          return (bool[])Value;
      }
      catch
      {
        return null;
      }
    }

    #endregion

    #region DataRowView and DataView

    /// <summary>
    /// Returns the value as a DataRowView defaulting the return value to null if the 
    /// parameter value is null, dbnull, or is not a convertable type.
    /// </summary>
    /// <returns>The DataRowView value of the parameter.</returns>
    public DataRowView AsDataRowView()
    {
      if ((Value == null) || (Value == DBNull.Value) ||
          !(Value is DataRowView))
        return null;
      else
        return (DataRowView)Value;
    }

    /// <summary>
    /// Returns the value as a DataView defaulting the return value to null if the 
    /// parameter value is null, dbnull, or is not a convertable type.
    /// </summary>
    /// <returns>The DataView value of the parameter.</returns>
    public DataView AsDataView()
    {
      if ((Value == null) || (Value == DBNull.Value) ||
          !(Value is DataView))
        return null;
      else
        return (DataView)Value;
    }

    #endregion

    #region DateTime

    /// <summary>
    /// Returns the value as a DateTime defaulting the return value to null if the 
    /// parameter value is null, dbnull, or is not a convertable type.
    /// </summary>
    /// <param name="ADateBorderType">Whether the date is a began or ended date.</param>
    /// <returns>The DateTime value of the parameter.</returns>
    public DateTime AsDateTime(eDateBorderType ADateBorderType)
    {
      if ((Value == null) || (Value == DBNull.Value) ||
          !(Value is DateTime))
      {
        if (ADateBorderType == eDateBorderType.Began)
          return DateTime.MinValue;
        else
          return DateTime.MaxValue;
      }
      else
        return Convert.ToDateTime(Value);
    }

    #endregion

    #region Decimal and Decimal Array

    /// <summary>
    /// Returns the value as a Decimal defaulting the return value to the default value if the 
    /// parameter value is null, dbnull, or is not a convertable type.
    /// </summary>
    /// <param name="ADefault">The default value.</param>
    /// <returns>The Decimal value of the parameter.</returns>
    public decimal AsDecimal(decimal ADefault)
    {
      if ((Value == null) || (Value == DBNull.Value) ||
          (!(Value is Decimal) && !(Value is Int16) &&
           !(Value is Int32) && !(Value is Int64)))
        return ADefault;
      else
        return Convert.ToDecimal(Value);
    }

    /// <summary>
    /// Returns the value as a Decimal defaulting the return value to the minumum value if the 
    /// parameter value is null, dbnull, or is not a convertable type.
    /// </summary>
    /// <returns>The Decimal value of the parameter.</returns>
    public decimal AsDecimal()
    {
      return AsDecimal(decimal.MinValue);
    }

    /// <summary>
    /// Returns the value as a Decimal array defaulting the return value to the minumum value if the 
    /// parameter value is null, dbnull, or is not a convertable type.
    /// </summary>
    /// <returns>The Decimal array of the parameter.</returns>
    public decimal[] AsDecimalArray()
    {
      try
      {
        if ((Value == null) || (Value == DBNull.Value) ||
            !(Value is decimal[]))
          return null;
        else
          return (decimal[])Value;
      }
      catch
      {
        return null;
      }
    }

    /// <summary>
    /// Returns the value as a Decimal? defaulting the return value to the minumum value if the 
    /// parameter value is dbnull, or is not a convertable type.
    /// </summary>
    /// <returns>The Decimal? value of the parameter.</returns>
    public decimal? AsDecimalNullable()
    {
      if ((Value == null) || (Value == DBNull.Value) ||
          ((Value.GetType() != typeof(decimal?)) &&
           (Value.GetType() != typeof(Int16?)) &&
           (Value.GetType() != typeof(Int32?)) &&
           (Value.GetType() != typeof(Int64?))))
        return (decimal?)null;
      else
        return Convert.ToDecimal(Value);
    }

    #endregion

    #region Integer and Integer Array

    /// <summary>
    /// Returns the value as a int defaulting the return value to the default value if the 
    /// parameter value is null, dbnull, or is not a convertable type.
    /// </summary>
    /// <param name="ADefault">The default value.</param>
    /// <returns>The int value of the parameter.</returns>
    public int AsInt(int ADefault)
    {
      if ((Value == null) || (Value == DBNull.Value) ||
          (!(Value is Int16) && !(Value is Int32)))
        return ADefault;
      else
        return Convert.ToInt32(Value);
    }

    /// <summary>
    /// Returns the value as a int defaulting the return value to the minimum value if the 
    /// parameter value is null, dbnull, or is not a convertable type.
    /// </summary>
    /// <returns>The int value of the parameter.</returns>
    public int AsInt()
    {
      return AsInt(int.MinValue);
    }

    /// <summary>
    /// Returns the value as a int array defaulting the return value to the minumum value if the 
    /// parameter value is null, dbnull, or is not a convertable type.
    /// </summary>
    /// <returns>The int array of the parameter.</returns>
    public int[] AsIntArray()
    {
      try
      {
        if ((Value == null) || (Value == DBNull.Value) ||
            !(Value is int[]))
          return null;
        else
          return (int[])Value;
      }
      catch
      {
        return null;
      }
    }

    /// <summary>
    /// Returns the value as a int? defaulting the return value to the minumum value if the 
    /// parameter value is dbnull, or is not a convertable type.
    /// </summary>
    /// <returns>The int? value of the parameter.</returns>
    public int? AsIntNullable()
    {
      if ((Value == null) || (Value == DBNull.Value) ||
          ((Value.GetType() != typeof(Int16?)) &&
           (Value.GetType() != typeof(Int32?))))
        return (int?)null;
      else
        return Convert.ToInt32(Value);
    }

    #endregion

    #region String and String Array

    /// <summary>
    /// Returns the value as a string defaulting the return value to the default value if the 
    /// parameter value is null, dbnull, or is not a convertable type.
    /// </summary>
    /// <returns>The int value of the parameter.</returns>
    public string AsString()
    {
      if ((Value == null) || (Value == DBNull.Value))
        return "";
      if (Value.GetType() == Type.GetType("System.DateTime"))
      {
        DateTime ParameterValueDate = (DateTime)Value;
        return ParameterValueDate.ToShortDateString();
      }
      else
        return Value.ToString();
    }

    /// <summary>
    /// Returns the value as a string array defaulting the return value to the minumum value if the 
    /// parameter value is null, dbnull, or is not a convertable type.
    /// </summary>
    /// <returns>The string array of the parameter.</returns>
    public string[] AsStringArray()
    {
      try
      {
        if ((Value == null) || (Value == DBNull.Value) ||
            !(Value is string[]))
          return null;
        else
          return (string[])Value;
      }
      catch
      {
        return null;
      }
    }

    #endregion

    #endregion


    #region Public Override Methods

    /// <summary>
    /// Returns this parameter's default string value for nulls.
    /// </summary>
    /// <returns>Returns the default string value for nulls.</returns>
    public override object GetDefaultForNull()
    {
      return null;
    }

    /// <summary>
    /// Retursn this parameter's default string value for nulls as an object.
    /// </summary>
    /// <returns>Returns the default string value for nulls as an object.</returns>
    public override object GetDefaultObjectForNull()
    {
      return GetDefaultForNull();
    }

    /// <summary>
    /// Returns null typed as a string.
    /// </summary>
    /// <returns>Returns null as a string.</returns>
    public override object GetNull()
    {
      return null;
    }

    /// <summary>
    /// Returns the passed value if it is not null, otherwise it returns the default string for nulls.
    /// </summary>
    /// <param name="ANullableValue">The value to convert.</param>
    /// <returns>The converted value.</returns>
    public override object GetTypeValue(object ANullableValue)
    {
      return (ANullableValue ?? GetDefaultForNull());
    }

    #endregion


    #region Protected Override Methods

    /// <summary>
    /// Converts the input object to a string.
    /// </summary>
    /// <param name="AInputValue">The object to convert.</param>
    /// <param name="AOutputValue">The converted value.</param>
    /// <param name="AErrorMessage">The error message returned if the conversion fails.</param>
    /// <returns>Returns false if the conversion failed.</returns>
    protected override bool ConvertObjectType(object AInputValue, ref object AOutputValue, ref string AErrorMessage)
    {
      try
      {
        AOutputValue = AInputValue;
        return true;
      }
      catch (Exception ex)
      {
        AErrorMessage = "[" + this.Name + ".ConvertObjectType]: " + ex.Message;
        return false;
      }
    }

    /// <summary>
    /// Sets the value property field to null, typed as a string.
    /// </summary>
    protected override void ResetValue()
    {
      FValue = (string)null;
    }

    #endregion

  }

}
