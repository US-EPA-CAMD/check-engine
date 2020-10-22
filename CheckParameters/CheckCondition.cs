using System;
using System.Collections.Generic;
using System.Text;

using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Extensions;

namespace ECMPS.Checks.Parameters
{
  /// <summary>
  /// A check condition including the metheod to evaluate it.
  /// </summary>
  public class cCheckCondition
  {

    #region Public Constructors

    /// <summary>
    /// Creates a cCheckCondtion instance.
    /// </summary>
    /// <param name="AAndGroup">The and group of the condtion.</param>
    /// <param name="ACheckParameter">The check parameter to compare.</param>
    /// <param name="ACompareValue">The value to compare.</param>
    /// <param name="AOperator">The comparison operator.</param>
    /// <param name="ANegate">Whether to negate the comparison.</param>
    public cCheckCondition(int AAndGroup, cCheckParameter ACheckParameter, object ACompareValue,
                           eConditionOperator AOperator, bool ANegate)
    {
      FAndGroup = AAndGroup;
      FCheckParameter = ACheckParameter;
      FCompareValue = ACompareValue;
      FNegate = ANegate;
      FOperator = AOperator;

      FCheckConditionEvaluator = new dCheckConditionEvaluator(AlwaysFalse);

      switch (AOperator)
      {
        case eConditionOperator.BeginsWith:
          FCheckConditionEvaluator = new dCheckConditionEvaluator(BeginsWith);
          break;

        case eConditionOperator.Contains:
          FCheckConditionEvaluator = new dCheckConditionEvaluator(Contains);
          break;

        case eConditionOperator.EndsWith:
          FCheckConditionEvaluator = new dCheckConditionEvaluator(EndsWith);
          break;

        case eConditionOperator.Equals:
          FCheckConditionEvaluator = new dCheckConditionEvaluator(Equals);
          break;

        case eConditionOperator.GreaterThan:
          FCheckConditionEvaluator = new dCheckConditionEvaluator(GreaterThan);
          break;

        case eConditionOperator.GreaterThanOrEqual:
          FCheckConditionEvaluator = new dCheckConditionEvaluator(GreaterThanOrEqual);
          break;

        case eConditionOperator.InList:
          FCheckConditionEvaluator = new dCheckConditionEvaluator(InList);
          break;

        case eConditionOperator.LessThan:
          FCheckConditionEvaluator = new dCheckConditionEvaluator(LessThan);
          break;

        case eConditionOperator.LessThanOrEqual:
          FCheckConditionEvaluator = new dCheckConditionEvaluator(LessThanOrEqual);
          break;

        case eConditionOperator.NotEqual:
          FCheckConditionEvaluator = new dCheckConditionEvaluator(Equals);
          FNegate = !ANegate;
          break;

        default:
          FCheckConditionEvaluator = new dCheckConditionEvaluator(AlwaysFalse);
          break;
      }
    }

    #endregion


    #region Private Fields

    private dCheckConditionEvaluator FCheckConditionEvaluator;

    #endregion


    #region Public Properties

    #region Property Fields

    private int FAndGroup = int.MinValue;
    private cCheckParameter FCheckParameter = null;
    private object FCompareValue = null;
    private bool FNegate = false;
    private eConditionOperator FOperator = eConditionOperator.Equals;

    #endregion


    /// <summary>
    /// The and group for the condition.
    /// </summary>
    public int AndGroup { get { return FAndGroup; } }

    /// <summary>
    /// The check parameter to compare to the value.
    /// </summary>
    public cCheckParameter CheckParameter { get { return FCheckParameter; } }

    /// <summary>
    /// The value to compare to the check parameter.
    /// </summary>
    public object CompareValue { get { return FCompareValue; } }

    /// <summary>
    /// Whether to negate the comparison.
    /// </summary>
    public bool Negate { get { return FNegate; } }

    /// <summary>
    /// The comparison operator.
    /// </summary>
    public eConditionOperator Operator { get { return FOperator; } }

    #endregion


    #region Public Methods

    /// <summary>
    /// Evaluates the condition and returns the result.
    /// </summary>
    /// <returns>The result of the evaluation.</returns>
    public bool Evaluate()
    {
      return FCheckConditionEvaluator(FCheckParameter, FCompareValue, FNegate);
    }

    #endregion


    #region Private Methods

    /// <summary>
    /// Compares the parameter to the compare value.
    /// </summary>
    /// <param name="ACheckParameter">The check parameter to compare.</param>
    /// <param name="ACompareValue">The value to compare.</param>
    /// <param name="ANegate"></param>
    /// <returns>Always returns null.</returns>
    protected bool AlwaysFalse(cCheckParameter ACheckParameter, object ACompareValue, bool ANegate)
    {
      return false;
    }

    /// <summary>
    /// Compares the parameter to the compare value.
    /// </summary>
    /// <param name="ACheckParameter">The check parameter to compare.</param>
    /// <param name="ACompareValue">The value to compare.</param>
    /// <param name="ANegate"></param>
    /// <returns>Returns true if the parameter value begins with the compare value.</returns>
    protected bool BeginsWith(cCheckParameter ACheckParameter, object ACompareValue, bool ANegate)
    {
      bool Result;

      if ((ACheckParameter.IsSet) && (ACheckParameter.ValueAsObject() != null))
      {
        string ParameterValue = cDBConvert.ToString(ACheckParameter.ValueAsObject());

        Result = ParameterValue.StartsWith(cDBConvert.ToString(ACompareValue));
      }
      else
        Result = false;

      return (Result != ANegate);
    }

    /// <summary>
    /// Compares the parameter to the compare value.
    /// </summary>
    /// <param name="ACheckParameter">The check parameter to compare.</param>
    /// <param name="ACompareValue">The value to compare.</param>
    /// <param name="ANegate"></param>
    /// <returns>Returns true if the parameter value contains the compare value.</returns>
    protected bool Contains(cCheckParameter ACheckParameter, object ACompareValue, bool ANegate)
    {
      bool Result;

      if ((ACheckParameter.IsSet) && (ACheckParameter.ValueAsObject() != null))
      {
        string ParameterValue = cDBConvert.ToString(ACheckParameter.ValueAsObject());

        Result = ParameterValue.Contains(cDBConvert.ToString(ACompareValue));
      }
      else
        Result = false;

      return (Result != ANegate);
    }

    /// <summary>
    /// Compares the parameter to the compare value.
    /// </summary>
    /// <param name="ACheckParameter">The check parameter to compare.</param>
    /// <param name="ACompareValue">The value to compare.</param>
    /// <param name="ANegate"></param>
    /// <returns>Returns true if the parameter value equals the compare value.</returns>
    protected bool Equals(cCheckParameter ACheckParameter, object ACompareValue, bool ANegate)
    {
      bool Result;

      if ((ACheckParameter.IsSet) && (ACheckParameter.ValueAsObject() != null))
      {
        string ParameterValue = cDBConvert.ToString(ACheckParameter.ValueAsObject());

        switch (ACheckParameter.DataType)
        {
          case eParameterDataType.Boolean:
            Result = (cDBConvert.ToBoolean(ParameterValue) == cDBConvert.ToBoolean(ACompareValue));
            break;

          case eParameterDataType.Date:
            Result = (cDBConvert.ToDate(ParameterValue, DateTypes.START) == cDBConvert.ToDate(ACompareValue, DateTypes.START));
            break;

          case eParameterDataType.Decimal:
            Result = (cDBConvert.ToDecimal(ParameterValue) == cDBConvert.ToDecimal(ACompareValue));
            break;

          case eParameterDataType.Integer:
            Result = (cDBConvert.ToInteger(ParameterValue) == cDBConvert.ToInteger(ACompareValue));
            break;

          case eParameterDataType.String:
            Result = (cDBConvert.ToString(ParameterValue) == cDBConvert.ToString(ACompareValue));
            break;

          default:
            Result = false;
            break;
        }
      }
      else
        Result = false;

      return (Result != ANegate);
    }

    /// <summary>
    /// Compares the parameter to the compare value.
    /// </summary>
    /// <param name="ACheckParameter">The check parameter to compare.</param>
    /// <param name="ACompareValue">The value to compare.</param>
    /// <param name="ANegate"></param>
    /// <returns>Returns true if the parameter value ends with the compare value.</returns>
    protected bool EndsWith(cCheckParameter ACheckParameter, object ACompareValue, bool ANegate)
    {
      bool Result;

      if ((ACheckParameter.IsSet) && (ACheckParameter.ValueAsObject() != null))
      {
        string ParameterValue = cDBConvert.ToString(ACheckParameter.ValueAsObject());

        Result = ParameterValue.EndsWith(cDBConvert.ToString(ACompareValue));
      }
      else
        Result = false;

      return (Result != ANegate);
    }

    /// <summary>
    /// Compares the parameter to the compare value.
    /// </summary>
    /// <param name="ACheckParameter">The check parameter to compare.</param>
    /// <param name="ACompareValue">The value to compare.</param>
    /// <param name="ANegate"></param>
    /// <returns>Returns true if the parameter value is greater than the compare value.</returns>
    protected bool GreaterThan(cCheckParameter ACheckParameter, object ACompareValue, bool ANegate)
    {
      bool Result;

      if ((ACheckParameter.IsSet) && (ACheckParameter.ValueAsObject() != null))
      {
        string ParameterValue = cDBConvert.ToString(ACheckParameter.ValueAsObject());

        switch (ACheckParameter.DataType)
        {
          case eParameterDataType.Date:
            Result = (cDBConvert.ToDate(ParameterValue, DateTypes.START) > cDBConvert.ToDate(ACompareValue, DateTypes.END));
            break;

          case eParameterDataType.Decimal:
            Result = (cDBConvert.ToDecimal(ParameterValue, decimal.MinValue) > cDBConvert.ToDecimal(ACompareValue, decimal.MaxValue));
            break;

          case eParameterDataType.Integer:
            Result = (cDBConvert.ToInteger(ParameterValue, int.MinValue) > cDBConvert.ToInteger(ACompareValue, int.MaxValue));
            break;

          default:
            Result = false;
            break;
        }
      }
      else
        Result = false;

      return (Result != ANegate);
    }

    /// <summary>
    /// Compares the parameter to the compare value.
    /// </summary>
    /// <param name="ACheckParameter">The check parameter to compare.</param>
    /// <param name="ACompareValue">The value to compare.</param>
    /// <param name="ANegate"></param>
    /// <returns>Returns true if the parameter value is greater than or equal to the compare value.</returns>
    protected bool GreaterThanOrEqual(cCheckParameter ACheckParameter, object ACompareValue, bool ANegate)
    {
      bool Result;

      if ((ACheckParameter.IsSet) && (ACheckParameter.ValueAsObject() != null))
      {
        string ParameterValue = cDBConvert.ToString(ACheckParameter.ValueAsObject());

        switch (ACheckParameter.DataType)
        {
          case eParameterDataType.Date:
            Result = (cDBConvert.ToDate(ParameterValue, DateTypes.START) >= cDBConvert.ToDate(ACompareValue, DateTypes.END));
            break;

          case eParameterDataType.Decimal:
            Result = (cDBConvert.ToDecimal(ParameterValue, decimal.MinValue) >= cDBConvert.ToDecimal(ACompareValue, decimal.MaxValue));
            break;

          case eParameterDataType.Integer:
            Result = (cDBConvert.ToInteger(ParameterValue, int.MinValue) >= cDBConvert.ToInteger(ACompareValue, int.MaxValue));
            break;

          default:
            Result = false;
            break;
        }
      }
      else
        Result = false;

      return (Result != ANegate);
    }

    /// <summary>
    /// Compares the parameter to the compare value.
    /// </summary>
    /// <param name="ACheckParameter">The check parameter to compare.</param>
    /// <param name="ACompareValue">The value to compare.</param>
    /// <param name="ANegate"></param>
    /// <returns>Returns true if the parameter value is in the list in the compare value.</returns>
    protected bool InList(cCheckParameter ACheckParameter, object ACompareValue, bool ANegate)
    {
      bool Result;

      if ((ACheckParameter.IsSet) && (ACheckParameter.ValueAsObject() != null))
      {
        string ParameterValue = cDBConvert.ToString(ACheckParameter.ValueAsObject());

        Result = ParameterValue.InList(cDBConvert.ToString(ACompareValue));
      }
      else
        Result = false;

      return (Result != ANegate);
    }

    /// <summary>
    /// Compares the parameter to the compare value.
    /// </summary>
    /// <param name="ACheckParameter">The check parameter to compare.</param>
    /// <param name="ACompareValue">The value to compare.</param>
    /// <param name="ANegate"></param>
    /// <returns>Returns true if the parameter value is less than the compare value.</returns>
    protected bool LessThan(cCheckParameter ACheckParameter, object ACompareValue, bool ANegate)
    {
      bool Result;

      if ((ACheckParameter.IsSet) && (ACheckParameter.ValueAsObject() != null))
      {
        string ParameterValue = cDBConvert.ToString(ACheckParameter.ValueAsObject());

        switch (ACheckParameter.DataType)
        {
          case eParameterDataType.Date:
            Result = (cDBConvert.ToDate(ParameterValue, DateTypes.END) < cDBConvert.ToDate(ACompareValue, DateTypes.START));
            break;

          case eParameterDataType.Decimal:
            Result = (cDBConvert.ToDecimal(ParameterValue, decimal.MaxValue) < cDBConvert.ToDecimal(ACompareValue, decimal.MinValue));
            break;

          case eParameterDataType.Integer:
            Result = (cDBConvert.ToInteger(ParameterValue, int.MaxValue) < cDBConvert.ToInteger(ACompareValue, int.MinValue));
            break;

          default:
            Result = false;
            break;
        }
      }
      else
        Result = false;

      return (Result != ANegate);
    }

    /// <summary>
    /// Compares the parameter to the compare value.
    /// </summary>
    /// <param name="ACheckParameter">The check parameter to compare.</param>
    /// <param name="ACompareValue">The value to compare.</param>
    /// <param name="ANegate"></param>
    /// <returns>Returns true if the parameter value is less than or equal to the compare value.</returns>
    protected bool LessThanOrEqual(cCheckParameter ACheckParameter, object ACompareValue, bool ANegate)
    {
      bool Result;

      if ((ACheckParameter.IsSet) && (ACheckParameter.ValueAsObject() != null))
      {
        string ParameterValue = cDBConvert.ToString(ACheckParameter.ValueAsObject());

        switch (ACheckParameter.DataType)
        {
          case eParameterDataType.Date:
            Result = (cDBConvert.ToDate(ParameterValue, DateTypes.END) <= cDBConvert.ToDate(ACompareValue, DateTypes.START));
            break;

          case eParameterDataType.Decimal:
            Result = (cDBConvert.ToDecimal(ParameterValue, decimal.MaxValue) <= cDBConvert.ToDecimal(ACompareValue, decimal.MinValue));
            break;

          case eParameterDataType.Integer:
            Result = (cDBConvert.ToInteger(ParameterValue, int.MaxValue) <= cDBConvert.ToInteger(ACompareValue, int.MinValue));
            break;

          default:
            Result = false;
            break;
        }
      }
      else
        Result = false;

      return (Result != ANegate);
    }

    #endregion

  }
}
