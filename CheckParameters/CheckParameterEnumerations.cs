namespace ECMPS.Checks.Parameters
{

  /// <summary>
  /// Indicates the data type of the parameter value
  /// </summary>
  public enum eParameterDataType
  {
    /// <summary>
    /// Object Value
    /// </summary>
    Object,

    /// <summary>
    /// Boolean Value
    /// </summary>
    Boolean,

    /// <summary>
    /// DataRowView Value
    /// </summary>
    DataRowView,

    /// <summary>
    /// DataView Value
    /// </summary>
    DataView,

    /// <summary>
    /// Date Value
    /// </summary>
    Date,

    /// <summary>
    /// Decimal Value
    /// </summary>
    Decimal,

    /// <summary>
    /// Integer Value
    /// </summary>
    Integer,

    /// <summary>
    /// String Value
    /// </summary>
    String
  }

  ///// <summary>
  ///// Indicates the update mode of the parameter value
  ///// </summary>
  //public enum eParameterUpdateMode
  //{
  //  /// <summary>
  //  /// Parameter does not allow updates
  //  /// </summary>
  //  None,

  //  /// <summary>
  //  /// Parameter supports updates but does not support aggregation
  //  /// </summary>
  //  Update,

  //  /// <summary>
  //  /// Parameter supports updates and aggregation
  //  /// </summary>
  //  Aggregate
  //}

  /// <summary>
  /// Indicates how a check uses a particular parameter
  /// </summary>
  public enum eParameterUsageType
  {
    /// <summary>
    /// Required Input Parameter
    /// </summary> 
    Required,

    /// <summary>
    /// Optional Input Parameter
    /// </summary> 
    Optional,

    /// <summary>
    /// Condition Input Parameter
    /// </summary> 
    Condition,

    /// <summary>
    /// Output Parameter
    /// </summary> 
    Output
  }

  /// <summary>
  /// Indicates whether a dbnull date (or hour) should be treated as a begin or end date.
  /// </summary>
  public enum eDateBorderType
  {
    /// <summary>
    /// Treat DBNULL date as a begin date.
    /// </summary>
    Began,

    /// <summary>
    /// Treat DBNULL date as an end date.
    /// </summary>
    Ended
  }

  /// <summary>
  /// Indicates the comparison method for a filter condition
  /// </summary>
  public enum eFilterPairCompare
  {
    /// <summary>
    /// Determine if table value equals the reference value
    /// </summary>
    Equals,

    /// <summary>
    /// Determine if table value begins with the reference value
    /// </summary>
    BeginsWith,

    /// <summary>
    /// Determine if table value contains the reference value
    /// </summary>
    Contains,

    /// <summary>
    /// Determine if table value ends with the reference value
    /// </summary>
    EndsWith,

    /// <summary>
    /// Determine if table value is in the reference value list
    /// </summary>
    InList,

    /// <summary>
    /// Determine if table value is less than the reference value
    /// </summary>
    LessThan,

    /// <summary>
    /// Determine if table value is less than or equal to the reference value
    /// </summary>
    LessThanOrEqual,

    /// <summary>
    /// Determine if table value is greater than or equal to the reference value
    /// </summary>
    GreaterThanOrEqual,

    /// <summary>
    /// Determine if table value is greater than the reference value
    /// </summary>
    GreaterThan
  }

  /// <summary>
  /// Indicates the string comparison method for a filter condition
  /// </summary>
  public enum eFilterPairStringCompare
  {
    /// <summary>
    /// Determine if table value equals the reference value
    /// </summary>
    Equals,

    /// <summary>
    /// Determine if table value begins with the reference value
    /// </summary>
    BeginsWith,

    /// <summary>
    /// Determine if table value contains the reference value
    /// </summary>
    Contains,

    /// <summary>
    /// Determine if table value ends with the reference value
    /// </summary>
    EndsWith,

    /// <summary>
    /// Determine if table value is in the reference value list
    /// </summary>
    InList
  }

  /// <summary>
  /// Indicates the relative comparison method for a filter condition
  /// </summary>
  public enum eFilterPairRelativeCompare
  {
    /// <summary>
    /// Determine if table value equals the reference value
    /// </summary>
    Equals,

    /// <summary>
    /// Determine if table value is less than the reference value
    /// </summary>
    LessThan,

    /// <summary>
    /// Determine if table value is less than or equal to the reference value
    /// </summary>
    LessThanOrEqual,

    /// <summary>
    /// Determine if table value is greater than or equal to the reference value
    /// </summary>
    GreaterThanOrEqual,

    /// <summary>
    /// Determine if table value is greater than the reference value
    /// </summary>
    GreaterThan
  }

  /// <summary>
  /// Enumeration of check condition comparison methods.
  /// </summary>
  public enum eConditionOperator
  {
    /// <summary>Return true if field equals the value</summary>
    Equals,
    /// <summary>Return true if field does not equal the value</summary>
    NotEqual,
    /// <summary>Return true if field begins with the value</summary>
    BeginsWith,
    /// <summary>Return true if field contains the value</summary>
    Contains,
    /// <summary>Return true if field ends with the value</summary>
    EndsWith,
    /// <summary>Return true if field in the list represented in the value</summary>
    InList,
    /// <summary>Return true if field is less than the value</summary>
    LessThan,
    /// <summary>Return true if field is less than or equal to the value</summary>
    LessThanOrEqual,
    /// <summary>Return true if field is greater than or equal to the value</summary>
    GreaterThanOrEqual,
    /// <summary>Return true if field is greater than the value</summary>
    GreaterThan
  }

}
