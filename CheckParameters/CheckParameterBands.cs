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
  /// This class contains the lists of checks leveled into bands based on
  /// the output check parameter dependencies between the checks.  The class
  /// also provides the method for determining and loading the bands.
  /// </summary>
  public class cCheckParameterBands
  {

    #region Public Constructors

    /// <summary>
    /// Instantiates a CCheckParameterBands object and save the passed category codes.
    /// </summary>
    /// <param name="ACategoryCd"></param>
    public cCheckParameterBands(string ACategoryCd)
    {
      FCategoryCd = ACategoryCd;
    }

    #endregion


    #region Private Fields

    cParameterizedCheck[][] FBands = null;

    #endregion


    #region Private Fields

    private int[] FCheckCountList;

    #endregion


    #region Public Properties

    #region Property Fields

    private int FBandCount = 0;
    private string FCategoryCd;
    private int FTotalChecks = 0;

    #endregion

    /// <summary>
    /// Indicates the number of check bands
    /// </summary>
    public int BandCount { get { return FBandCount; } }

    /// <summary>
    /// The category code associated with the checks for the check parameter bands
    /// </summary>
    public string CategoryCd { get { return FCategoryCd; } }

    /// <summary>
    /// Indicates the total number of checks in all check bands
    /// </summary>
    public int TotalChecks { get { return FTotalChecks; } }

    #endregion


    #region Public Methods: Populate with Helper Methods

    /// <summary>
    /// Populates the check bands for the process and category codes of the oject's instance.
    /// </summary>
    /// <param name="AAuxDatabase">Database connection to an ECMPS_AUX schema</param>
    /// <param name="ACheckParameters">The check parameters object containing the parameter implementations</param>
    /// <param name="AForRun">When set to true, only checks with a run flag of 'Y' are populated.</param>
    /// <param name="AErrorMessage">Error message set when result is false.</param>
    /// <returns>True if successful</returns>
    public bool Populate(cDatabase AAuxDatabase, cCheckParameters ACheckParameters, bool AForRun, 
                         ref string AErrorMessage)
    {
      cParameterizedCheck[,] TempBands;
      int[] CheckCounts;
      int BandCnt = 0;

      if (LoadChecks(AAuxDatabase, ACheckParameters, AForRun,  
                     out TempBands, out CheckCounts, out BandCnt,
                     ref AErrorMessage))
      {
        BandCnt = 1;

        // Push checks to proper band
        for (int BandDex = 0; (BandDex < BandCnt) && (BandDex < CheckCounts.Length); BandDex++)
        {
          for (int CheckDex = (CheckCounts[BandDex] - 1); CheckDex >= 0; CheckDex--)
          {
            cParameterizedCheck Check = TempBands[BandDex, CheckDex];

            if (FindParent(BandDex, TempBands[BandDex, CheckDex], ref TempBands, ref CheckCounts) ||
                FindParent(BandDex + 1, TempBands[BandDex, CheckDex], ref TempBands, ref CheckCounts))
            {
              AppendCheckToLevel(BandDex + 1, TempBands[BandDex, CheckDex], ref TempBands, ref CheckCounts);
              RemoveCheckFromLevel(BandDex, CheckDex, ref TempBands, ref CheckCounts);

              if ((BandDex + 1) >= BandCnt) BandCnt = (BandDex + 1) + 1;
            }
          }

          SortCheckBand(BandDex, CheckCounts, ref TempBands);
        }

        // Set object fields
        FBands = new cParameterizedCheck[BandCnt][];
        FCheckCountList = new int[BandCnt];
        FTotalChecks = 0;

        for (int BandDex = 0; BandDex < BandCnt; BandDex++)
        {
          FCheckCountList[BandDex] = CheckCounts[BandDex];
          FTotalChecks += CheckCounts[BandDex];
          FBands[BandDex] = new cParameterizedCheck[CheckCounts[BandDex]];

          for (int CheckDex = 0; CheckDex < CheckCounts[BandDex]; CheckDex++)
          {
            FBands[BandDex][CheckDex] = TempBands[BandDex, CheckDex];
          }
        }

        FBandCount = BandCnt;

        return true;
      }
      else
        return false;
    }

    /// <summary>
    /// Populates the check bands for the process and category codes of the oject's instance.
    /// Only checks with a run flag of 'Y' are populated.
    /// </summary>
    /// <param name="AAuxDatabase">Database connection to an ECMPS_AUX schema</param>
    /// <param name="ACheckParameters">The check parameters object containing the parameter implementations</param>
    /// <param name="AErrorMessage">Error message set when result is false.</param>
    /// <returns>True if successful</returns>
    public bool Populate(cDatabase AAuxDatabase, cCheckParameters ACheckParameters,
                         ref string AErrorMessage)
    {
      return Populate(AAuxDatabase, ACheckParameters, true, ref AErrorMessage);
    }

    #region Private Helper Methods

    private void AppendCheckToLevel(int ACheckBandPos,
                                    cParameterizedCheck AParameterizedCheck,
                                    ref cParameterizedCheck[,] ACheckBands,
                                    ref int[] ACheckCountList)
    {
      ACheckBands[ACheckBandPos, ACheckCountList[ACheckBandPos]] = AParameterizedCheck;
      ACheckCountList[ACheckBandPos] += 1;
    }

    private bool FindParent(int ACheckBandPos,
                            cParameterizedCheck AParameterizedCheck,
                            ref cParameterizedCheck[,] ACheckBands,
                            ref int[] ACheckCountList)
    {
      if ((AParameterizedCheck.CheckParametersInput != null) &&
          (ACheckBandPos < ACheckCountList.Length))
      {
        bool Result = false;

        foreach (cCheckParameter InputParameter in AParameterizedCheck.CheckParametersInput)
        {
          for (int SearchDex = 0; SearchDex < ACheckCountList[ACheckBandPos]; SearchDex++)
          {
            cParameterizedCheck SearchCheck = ACheckBands[ACheckBandPos, SearchDex];

            if (SearchCheck.RuleCheckId != AParameterizedCheck.RuleCheckId)
            {
              foreach (cCheckParameter OutputParameter in SearchCheck.CheckParametersOutput)
              {
                if (OutputParameter.Name.ToUpper().Trim() == InputParameter.Name.ToUpper().Trim())
                {
                  Result = true;
                  break;
                }
              }
            }

            if (Result) break;
          }

          if (Result) break;
        }

        return Result;
      }
      else
        return false;
    }

    private bool LoadChecks(cDatabase AAuxDatabase, cCheckParameters ACheckParameters, bool AForRun,
                            out cParameterizedCheck[,] ACheckBands,
                            out int[] ACheckCountList,
                            out int ACheckBandCnt,
                            ref string AErrorMessage)
    {
      string ErrorMessage = "";
      int BandsMax = GetOutParameterCount(FCategoryCd, AAuxDatabase, AForRun, ref ErrorMessage) + 1;
      int ChecksPerBandMax = GetCheckCount(FCategoryCd, AAuxDatabase, AForRun, ref ErrorMessage);

      ACheckBands = new cParameterizedCheck[BandsMax, ChecksPerBandMax];
      ACheckCountList = new int[BandsMax];
      ACheckBandCnt = 0;

      for (int CheckDex = 0; CheckDex < ACheckCountList.Length; CheckDex++)
        ACheckCountList[CheckDex] = 0;

      DataTable CheckTable;

      if (GetCategoryChecks(FCategoryCd, AAuxDatabase, AForRun, out CheckTable, ref AErrorMessage))
      {
        bool Result = true;

        foreach (DataRow CheckRow in CheckTable.Rows)
        {
          int RuleCheckId = cDBConvert.ToInteger(CheckRow["RULE_CHECK_ID"]);
          string CheckTypeCd = cDBConvert.ToString(CheckRow["CHECK_TYPE_CD"]);
          int CheckNumber = cDBConvert.ToInteger(CheckRow["CHECK_NUMBER"]);

          cParameterizedCheck ParameterizedCheck = new cParameterizedCheck(RuleCheckId, CheckTypeCd, CheckNumber);

          if (ParameterizedCheck.Populate(AAuxDatabase, ACheckParameters, ref ErrorMessage))
          {
            AppendCheckToLevel(ACheckBandCnt, ParameterizedCheck, ref ACheckBands, ref ACheckCountList);
          }
          else
          {
            AErrorMessage = AErrorMessage.ListAdd(ErrorMessage, Environment.NewLine, false);
            Result = false;
          }
        }

        ACheckBandCnt = 1;

        return Result;
      }
      else
        return false;
    }

    private void RemoveCheckFromLevel(int ACheckBandPos, int ACheckPos,
                                      ref cParameterizedCheck[,] ACheckBands,
                                      ref int[] ACheckCountList)
    {
      if (ACheckPos < ACheckCountList[ACheckBandPos])
      {
        int CheckDex = ACheckPos;

        // The last item moved should be a null even when not at the end of the second dimension
        while (CheckDex < ACheckCountList[ACheckBandPos])
        {
          if ((CheckDex + 1) < ACheckBands.GetLength(1))
            ACheckBands[ACheckBandPos, CheckDex] = ACheckBands[ACheckBandPos, CheckDex + 1];
          else
            ACheckBands[ACheckBandPos, CheckDex] = null;

          CheckDex += 1;
        }

        ACheckCountList[ACheckBandPos] -= 1;
      }
    }

    private void SortCheckBand(int ACheckBandPos, int[] ACheckCountList, 
                               ref cParameterizedCheck[,] ACheckBands)
    {
      int CheckCnt = ACheckCountList[ACheckBandPos];

      int TargetPos = 1;

      while (TargetPos < CheckCnt)
      {
        cParameterizedCheck TargetCheck = ACheckBands[ACheckBandPos, TargetPos];

        int SearchPos = TargetPos - 1;

        while ((SearchPos >= 0) &&
               ((string.Compare(ACheckBands[ACheckBandPos, SearchPos].CheckTypeCd, TargetCheck.CheckTypeCd, true) > 0) ||
                ((string.Compare(ACheckBands[ACheckBandPos, SearchPos].CheckTypeCd, TargetCheck.CheckTypeCd, true) == 0) &&
                 (ACheckBands[ACheckBandPos, SearchPos].CheckNumber > TargetCheck.CheckNumber))))
        {
          SearchPos -= 1;
        }

        SearchPos += 1;  // Shift to actual insert position

        for (int UpdateDex = TargetPos; UpdateDex > SearchPos; UpdateDex--)
        {
          ACheckBands[ACheckBandPos, UpdateDex] = ACheckBands[ACheckBandPos, UpdateDex - 1];
        }

        ACheckBands[ACheckBandPos, SearchPos] = TargetCheck;

        TargetPos += 1;
      }
    }

    #endregion

    #endregion


    #region Public Methods: Gets

    /// <summary>
    /// Returns the check corresponding to the passed band and check position.
    /// </summary>
    /// <param name="ABandPos">The position of the band in which the check resides</param>
    /// <param name="ACheckPos">The position of the check in the band in which it resides</param>
    /// <returns>The check corresponding to the passed band and check position</returns>
    public cParameterizedCheck GetCheck(int ABandPos, int ACheckPos)
    {
      if ((FCheckCountList != null) && (ABandPos < FBands.Length) &&
          (FBands[ABandPos] != null) && (ACheckPos < FBands[ABandPos].Length))
        return FBands[ABandPos][ACheckPos];
      else
        return null;
    }

    /// <summary>
    /// Returns the count of checks in the check band corresponding to the passed band position
    /// </summary>
    /// <param name="ABandPos">The position of the check band</param>
    /// <returns>The count of checks in the check band corresponding to the passed band position</returns>
    public int GetCheckCount(int ABandPos)
    {
      if ((FCheckCountList != null) && (ABandPos < FCheckCountList.Length))
        return FCheckCountList[ABandPos];
      else
        return 0;
    }

    #endregion


    #region Private Methods: SQL Execution

    private bool GetCategoryCheckParameters(int ARuleCheckId, eParameterUsageType AParameterUsageType, cDatabase ADatabase,
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

      Sql = string.Format(Sql, ARuleCheckId, UsageCd);

      return cUtilities.Database_GetDataTable(Sql, ADatabase, out AResultTable, ref AErrorMessage);
    }

    private bool GetCategoryCheckParameters(int ARuleCheckId, eParameterUsageType AParameterUsageType,
                                            cDatabase AAuxDatabase, cCheckParameters ACheckParameters,
                                            out cCheckParameter[] AParameterList, ref string AErrorMessage)
    {
      DataTable ParameterTable;

      if (GetCategoryCheckParameters(ARuleCheckId, AParameterUsageType, AAuxDatabase,
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

    private bool GetCategoryChecks(string ACategoryCd, cDatabase AAuxDatabase, bool AForRun, 
                                   out DataTable AResultTable, ref string AErrorMessage)
    {
      string WhereClause = string.Format("(CATEGORY_CD = '{0}')", ACategoryCd)
                      + (AForRun ? " AND RUN_CHECK_FLG = 'Y'" : "");

      string Sql = string.Format("SELECT RULE_CHECK_ID, CHECK_TYPE_CD, CHECK_NUMBER " + 
                                 "  FROM VW_RULE_CHECK " +
                                 "  WHERE {0}", 
                                 WhereClause);

      return cUtilities.Database_GetDataTable(Sql, AAuxDatabase, out AResultTable, ref AErrorMessage);
    }

    private int GetCheckCount(string ACategoryCd, cDatabase AAuxDatabase, bool AForRun, 
                              ref string AErrorMessage)
    {
      string WhereClause = string.Format("(CATEGORY_CD = '{0}')", ACategoryCd)
                      + (AForRun ? " AND RUN_CHECK_FLG = 'Y'" : "");

      string Sql = string.Format("SELECT COUNT(CHECK_CATALOG_ID) " +
                                 "  FROM (SELECT CHECK_CATALOG_ID " +
                                 "          FROM VW_RULE_CHECK " +
                                 "          WHERE {0} " +
                                 "          GROUP BY CHECK_CATALOG_ID) as out",
                                 WhereClause);

      int Result;

      if (cUtilities.Database_ExecuteScalar(Sql, AAuxDatabase, int.MinValue, out Result, ref AErrorMessage))
        return Result;
      else
        return int.MinValue;
    }

    private int GetOutParameterCount(string ACategoryCd, cDatabase AAuxDatabase, bool AForRun, 
                                     ref string AErrorMessage)
    {
      string WhereClause = string.Format("(CATEGORY_CD = '{0}' AND CHECK_PARAM_USAGE_CD = 'OUT')", ACategoryCd) 
                         + (AForRun ? " AND RUN_CHECK_FLG = 'Y'" : "");

      string Sql = string.Format("SELECT COUNT(CHECK_CATALOG_ID) " + 
                                 "  FROM (SELECT CHECK_CATALOG_ID " + 
                                 "          FROM VW_RULE_CHECK_PARAMETER " + 
                                 "          WHERE {0} " +
                                 "          GROUP BY CHECK_CATALOG_ID) as out",
                                 WhereClause);

      int Result;

      if (cUtilities.Database_ExecuteScalar(Sql, AAuxDatabase, int.MinValue, out Result, ref AErrorMessage))
        return Result;
      else
        return int.MinValue;
    }

    #endregion

  }
}
