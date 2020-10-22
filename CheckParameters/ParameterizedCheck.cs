using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using ECMPS.Checks.DatabaseAccess;
using ECMPS.Checks.TypeUtilities;
using ECMPS.Definitions.Extensions;

namespace ECMPS.Checks.Parameters
{
  /// <summary>
  /// Represents a check including the rule check id, check type code and check number,
  /// and required, optional and condition parameters individually and as input parameter,
  /// and output parameters.
  /// </summary>
  public class cParameterizedCheck
  {

    #region Public Constructors

    /// <summary>
    /// Creates an instance of cParameterizedCheck and sets the Rule Check Id, Check Type Code
    /// and Check Number parameters.
    /// </summary>
    /// <param name="ARuleCheckId">The rule check id of the associated check</param>
    /// <param name="ACheckTypeCd">The check type id of the associated check</param>
    /// <param name="ACheckNumber">The check number of the assocaited check</param>
    public cParameterizedCheck(int ARuleCheckId, string ACheckTypeCd, int ACheckNumber)
    {
      FRuleCheckId = ARuleCheckId;
      FCheckTypeCd = ACheckTypeCd;
      FCheckNumber = ACheckNumber;
    }

    #endregion


    #region Public Properties

    #region Property Fields

    private cCheckCondition[][] FCheckConditions = null;
    private int FCheckNumber;
    private string FCheckTypeCd;
    private cCheckParameter[] FCheckParametersInput = null;
    private cCheckParameter[] FCheckParametersInputCondition = null;
    private cCheckParameter[] FCheckParametersInputOptional = null;
    private cCheckParameter[] FCheckParametersInputRequired = null;
    private cCheckParameter[] FCheckParametersOutput = null;
    private int FRuleCheckId;

    #endregion


    /// <summary>
    /// An array of arrays of check conditins in which the results of evaluating the conditions in
    /// the inner arrays are anded together to gets results that are ored together for the outer array.
    /// </summary>
    public cCheckCondition[][] CheckConditions { get { return FCheckConditions; } }

    /// <summary>
    /// The check type code and check number associated with the Parameterized Check
    /// </summary>
    public string CheckName { get { return string.Format("{0}-{1}", FCheckTypeCd, FCheckNumber); } }

    /// <summary>
    /// The check number associated with the Parameterized Check
    /// </summary>
    public int CheckNumber { get { return FCheckNumber; } }

    /// <summary>
    /// The check type code associated with the Parameterized Check
    /// </summary>
    public string CheckTypeCd { get { return FCheckTypeCd; } }

    /// <summary>
    /// The input (required, optional and condition) parameters associated with the Parameterized Check
    /// </summary>
    public cCheckParameter[] CheckParametersInput { get { return FCheckParametersInput; } }

    /// <summary>
    /// The condition parameters associated with the Parameterized Check
    /// </summary>
    public cCheckParameter[] CheckParametersInputCondition { get { return FCheckParametersInputCondition; } }

    /// <summary>
    /// The optional parameters associated with the Parameterized Check
    /// </summary>
    public cCheckParameter[] CheckParametersInputOptional { get { return FCheckParametersInputOptional; } }

    /// <summary>
    /// The required parameters associated with the Parameterized Check
    /// </summary>
    public cCheckParameter[] CheckParametersInputRequired { get { return FCheckParametersInputRequired; } }

    /// <summary>
    /// The output parameters associated with the Parameterized Check
    /// </summary>
    public cCheckParameter[] CheckParametersOutput { get { return FCheckParametersOutput; } }

    /// <summary>
    /// The rule check id associated with the Parameterized Check
    /// </summary>
    public int RuleCheckId { get { return FRuleCheckId; } }

    #endregion


    #region Public Methods

    /// <summary>
    /// Evaluates the check conditions for the check and returns the results.
    /// </summary>
    /// <returns>The result of evaluating this checks check conditions.</returns>
    public bool EvaluateCheckConditions()
    {
      bool Result = true;

      foreach (cCheckCondition[] AndConditions in FCheckConditions)
      {
        Result = true;

        foreach (cCheckCondition Condition in AndConditions)
        {
          if (!Condition.Evaluate())
          {
            Result = false;
            break;
          }
        }

        if (Result) break;
      }

      return Result;
    }

    /// <summary>
    /// Returns true if the passed parameterized check is for the same check as this 
    /// parameterized check.
    /// </summary>
    /// <param name="AOtherParameterizedCheck">The rule check to check.</param>
    /// <returns>True if the this and the passed parameterized check are for the same check</returns>
    public bool IsSameCheck(cParameterizedCheck AOtherParameterizedCheck)
    {
      return ((AOtherParameterizedCheck != null) &&
              (FCheckTypeCd == AOtherParameterizedCheck.CheckTypeCd) &&
              (FCheckNumber == AOtherParameterizedCheck.CheckNumber));
    }

    /// <summary>
    /// Populates the parameters and conditions for the rule checks.
    /// </summary>
    /// <param name="AAuxDatabase">The AUX schema database.</param>
    /// <param name="ACheckParameters">The check parameters for the associates process.</param>
    /// <param name="AErrorMessage">The error message indicating why the population failed.</param>
    /// <returns>Returns true if the process succeeded.</returns>
    public bool Populate(cDatabase AAuxDatabase, cCheckParameters ACheckParameters, ref string AErrorMessage)
    {
      bool Result;

      if (GetCategoryCheckParameters(eParameterUsageType.Required, AAuxDatabase, ACheckParameters,
                                     out FCheckParametersInputRequired, ref AErrorMessage) &&
          GetCategoryCheckParameters(eParameterUsageType.Optional, AAuxDatabase, ACheckParameters,
                                     out FCheckParametersInputOptional, ref AErrorMessage) &&
          GetCategoryCheckParameters(eParameterUsageType.Condition, AAuxDatabase, ACheckParameters,
                                     out FCheckParametersInputCondition, ref AErrorMessage) &&
          GetCategoryCheckParameters(eParameterUsageType.Output, AAuxDatabase, ACheckParameters,
                                     out FCheckParametersOutput, ref AErrorMessage) &&
          GetCategoryCheckConditions(AAuxDatabase, ACheckParameters,
                                     out FCheckConditions, ref AErrorMessage))
      {
        cCheckParameter[] CheckParametersInputTemp = new cCheckParameter[FCheckParametersInputRequired.Length +
                                                                         FCheckParametersInputOptional.Length +
                                                                         FCheckParametersInputCondition.Length];
        int CheckParametersInputCnt = 0;

        cCheckParameter.MergeCheckParameterList(FCheckParametersInputRequired, ref CheckParametersInputTemp, ref CheckParametersInputCnt);
        cCheckParameter.MergeCheckParameterList(FCheckParametersInputOptional, ref CheckParametersInputTemp, ref CheckParametersInputCnt);
        cCheckParameter.MergeCheckParameterList(FCheckParametersInputCondition, ref CheckParametersInputTemp, ref CheckParametersInputCnt);

        FCheckParametersInput = cCheckParameter.NormalizeParameterList(CheckParametersInputTemp);

        Result = true;
      }
      else
        Result = false;

      return Result;
    }

    /// <summary>
    /// Show the check type and number the check object represents.
    /// </summary>
    /// <returns>Returns the check type and number the check object represents</returns>
    public override string ToString()
    {
      string result;

      result = string.Format("{0}-{1}", CheckTypeCd, CheckNumber);

      return result;
    }

    #endregion


    #region Public Static Methods

    /// <summary>
    /// Checks to determine whether the checks underlying two parameterized checks are the same check.
    /// </summary>
    /// <param name="AParameterizedCheckOne">One of the parameterized checks to check.</param>
    /// <param name="AParameterizedCheckTwo">One of the parameterized checks to check.</param>
    /// <returns>Returns true if the underlygin checks are the same or both parameterized checks are null.</returns>
    public static bool AreSameCheck(cParameterizedCheck AParameterizedCheckOne, 
                                    cParameterizedCheck AParameterizedCheckTwo)
    {
      bool Result;

      if ((AParameterizedCheckOne != null) && (AParameterizedCheckTwo != null))
      {
        Result = ((AParameterizedCheckOne.CheckTypeCd == AParameterizedCheckTwo.CheckTypeCd) &&
                  (AParameterizedCheckOne.CheckNumber == AParameterizedCheckTwo.CheckNumber));
      }
      else if ((AParameterizedCheckOne == null) && (AParameterizedCheckTwo == null))
      {
        Result = true;
      }
      else
      {
        Result = false;
      }

      return Result;
    }

    #endregion


    #region Private Methods: SQL Execution

    private bool GetCategoryCheckConditions(cDatabase AAuxDatabase,
                                            out DataTable AResultTable, ref string AErrorMessage)
    {
      string Sql = "SELECT DISTINCT AND_GROUP_NO, CHECK_PARAM_ID_NAME, CHECK_CONDITION, CHECK_OPERATOR_CD, NEGATION_IND"
                 + "  FROM VW_RULE_CHECK_CONDITION"
                 + "  WHERE RULE_CHECK_ID = {0}"
                 + "  ORDER BY AND_GROUP_NO";

      Sql = string.Format(Sql, FRuleCheckId);

      return cUtilities.Database_GetDataTable(Sql, AAuxDatabase, out AResultTable, ref AErrorMessage);
    }

    private bool GetCategoryCheckConditionsByAnd(cDatabase AAuxDatabase,
                                                 out DataTable AResultTable, ref string AErrorMessage)
    {
      string Sql = "SELECT AND_GROUP_NO, COUNT(RULE_CHECK_ID) CONDITION_CNT"
                 + "  FROM VW_RULE_CHECK_CONDITION"
                 + "  WHERE RULE_CHECK_ID = {0}"
                 + "  GROUP BY AND_GROUP_NO"
                 + "  ORDER BY AND_GROUP_NO";

      Sql = string.Format(Sql, FRuleCheckId);

      return cUtilities.Database_GetDataTable(Sql, AAuxDatabase, out AResultTable, ref AErrorMessage);
    }

    private bool GetCategoryCheckConditions(cDatabase AAuxDatabase, cCheckParameters ACheckParameters,
                                            out cCheckCondition[][] ACheckConditions, ref string AErrorMessage)
    {
      bool Result;

      DataTable AndCountTable;
      DataTable ConditionTable;

      if (GetCategoryCheckConditionsByAnd(AAuxDatabase, out AndCountTable, ref AErrorMessage) &&
          GetCategoryCheckConditions(AAuxDatabase, out ConditionTable, ref AErrorMessage))
      {
        ACheckConditions = new cCheckCondition[AndCountTable.Rows.Count][];

        if (AndCountTable.Rows.Count > 0)
        {
          for (int AndDex = 0; AndDex < AndCountTable.Rows.Count; AndDex++)
          {
            ACheckConditions[AndDex] = new cCheckCondition[cDBConvert.ToInteger(AndCountTable.Rows[AndDex]["CONDITION_CNT"])];
            AndDex += 1;
          }

          int BreakAndGroupNo = cDBConvert.ToInteger(ConditionTable.Rows[0]["AND_GROUP_NO"]);
          int AndGroupDex = 0;
          int ConditionDex = 0;

          foreach (DataRow ConditionRow in ConditionTable.Rows)
          {
            int AndGroupNo = cDBConvert.ToInteger(ConditionRow["AND_GROUP_NO"]);

            if (AndGroupNo != BreakAndGroupNo)
            {
              AndGroupDex += 1;
              ConditionDex = 0;
            }

            string ParameterName = cDBConvert.ToString(ConditionRow["CHECK_PARAM_ID_NAME"]);
            cCheckParameter CheckParameter = ACheckParameters.GetCheckParameter(ParameterName);
            object CompareValue = ConditionRow["CHECK_CONDITION"];
            string Operator = cDBConvert.ToString(ConditionRow["CHECK_OPERATOR_CD"]);
            eConditionOperator ConditionOperator = GetConditionOperator(Operator);
            bool Negate = (cDBConvert.ToInteger(ConditionRow["NEGATION_IND"]) > 0);

            ACheckConditions[AndGroupDex][ConditionDex] = new cCheckCondition(AndGroupNo, CheckParameter, CompareValue,
                                                                              ConditionOperator, Negate);

            ConditionDex += 1;
          }
        }

        Result = true;
      }
      else
      {
        ACheckConditions = null;
        Result = false;
      }

      return Result;
    }

    private bool GetCategoryCheckParameters(eParameterUsageType AParameterUsageType, cDatabase AAuxDatabase,
                                            out DataTable AResultTable, ref string AErrorMessage)
    {
      string Sql;
      string UsageCd;

      if (AParameterUsageType == eParameterUsageType.Condition)
      {
        Sql = "SELECT DISTINCT CHECK_PARAM_ID_NAME FROM VW_RULE_CHECK_CONDITION WHERE RULE_CHECK_ID = '{0}'";

        UsageCd = null;
      }
      else
      {
        Sql = "SELECT DISTINCT CHECK_PARAM_ID_NAME FROM VW_RULE_CHECK_PARAMETER WHERE RULE_CHECK_ID = '{0}' AND CHECK_PARAM_USAGE_CD = '{1}'";

        switch (AParameterUsageType)
        {
          case eParameterUsageType.Required: UsageCd = "REQ"; break;
          case eParameterUsageType.Optional: UsageCd = "OPT"; break;
          case eParameterUsageType.Output: UsageCd = "OUT"; break;
          default: UsageCd = ""; break;
        }
      }

      Sql = string.Format(Sql, FRuleCheckId, UsageCd);

      return cUtilities.Database_GetDataTable(Sql, AAuxDatabase, out AResultTable, ref AErrorMessage);
    }

    private bool GetCategoryCheckParameters(eParameterUsageType AParameterUsageType,
                                            cDatabase AAuxDatabase, cCheckParameters ACheckParameters,
                                            out cCheckParameter[] AParameterList, ref string AErrorMessage)
    {
      DataTable ParameterTable;

      if (GetCategoryCheckParameters(AParameterUsageType, AAuxDatabase,
                                     out ParameterTable, ref AErrorMessage))
      {
        bool Result = true;

        cCheckParameter[] TempParameterList = new cCheckParameter[ParameterTable.Rows.Count];
        int TempParameterCnt = 0;

        foreach (DataRow ParameterRow in ParameterTable.Rows)
        {
          string CheckParameterName = cDBConvert.ToString(ParameterRow["CHECK_PARAM_ID_NAME"]);

          cCheckParameter CheckParameter = ACheckParameters.GetCheckParameter(CheckParameterName);

          if (CheckParameter != null)
          {
            cCheckParameter.FindOrAddCheckParameter(CheckParameter, ref TempParameterList, ref TempParameterCnt);
          }
          else
          {
            AErrorMessage = AErrorMessage.ListAdd("[" + CheckParameterName + "] does not exist", Environment.NewLine, false);
            Result = false;
          }
        }

        AParameterList = cCheckParameter.NormalizeParameterList(TempParameterList);

        return Result;
      }
      else
      {
        AParameterList = null;
        return false;
      }
    }

    private int GetConditionAndCount(cDatabase AAuxDatabase, ref string AErrorMessage)
    {
      string Sql = "SELECT COUNT(RULE_CHECK_ID) FROM VW_RULE_CHECK_CONDITION WHERE RULE_CHECK_ID = {0} GROUP BY AND_GROUP_NO";

      Sql = string.Format(Sql, FRuleCheckId);


      int Result;

      if (cUtilities.Database_ExecuteScalar(Sql, AAuxDatabase, int.MinValue, out Result, ref AErrorMessage))
        return Result;
      else
        return int.MinValue;
    }

    #endregion


    #region Private Method

    private eConditionOperator GetConditionOperator(string AOperator)
    {
      eConditionOperator Result;

      switch (AOperator)
      {
        case "BW": Result = eConditionOperator.BeginsWith; break;
        case "CT": Result = eConditionOperator.Contains; break;
        case "EQ": Result = eConditionOperator.Equals; break;
        case "EW": Result = eConditionOperator.EndsWith; break; 
        case "GE": Result = eConditionOperator.GreaterThanOrEqual; break; 
        case "GT": Result = eConditionOperator.GreaterThan; break; 
        case "IN": Result = eConditionOperator.InList; break; 
        case "LE": Result = eConditionOperator.LessThanOrEqual; break; 
        case "LT": Result = eConditionOperator.LessThan; break; 
        case "NE": Result = eConditionOperator.NotEqual; break;
        default: Result = eConditionOperator.Equals; break;
      }

      return Result;
    }

    #endregion

  }
}
