using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Em.Parameters;
using ECMPS.Checks.EmissionsReport;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Extensions;


namespace ECMPS.Checks.EmissionsChecks
{
  public class WeeklyTestSummaryChecks : cEmissionsChecks
  {

    #region Constructors

    public WeeklyTestSummaryChecks(cEmissionsReportProcess emissionReportProcess)
      : base(emissionReportProcess)
    {
      CheckProcedures = new dCheckProcedure[14];

      CheckProcedures[1] = new dCheckProcedure(EMWTS1);
      CheckProcedures[2] = new dCheckProcedure(EMWTS2);
      CheckProcedures[3] = new dCheckProcedure(EMWTS3);
      CheckProcedures[4] = new dCheckProcedure(EMWTS4);
      CheckProcedures[5] = new dCheckProcedure(EMWTS5);
      CheckProcedures[6] = new dCheckProcedure(EMWTS6);
      CheckProcedures[7] = new dCheckProcedure(EMWTS7);
      CheckProcedures[8] = new dCheckProcedure(EMWTS8);
      CheckProcedures[9] = new dCheckProcedure(EMWTS9);
    }

    #endregion


    #region Check Methods (static)

    /// <summary>
    /// Initialize Parameters
    /// </summary>
    /// <param name="category">Category Object</param>
    /// <param name="log">Indicates whether to log results.</param>
    /// <returns>Returns error message if check fails to run correctly.</returns>
    public static string EMWTS1(cCategory category, ref bool log)
    {
      string returnVal = "";

      try
      {
        EmParameters.WeeklyTestSummaryValid = true;
        EmParameters.CalculatedWeeklyTestSummaryResult = null;
      }
      catch (Exception ex)
      {
        returnVal = category.CheckEngine.FormatError(ex);
      }

      return returnVal;
    }


    /// <summary>
    /// Check Weekly Test Type
    /// </summary>
    /// <param name="category">Category Object</param>
    /// <param name="log">Indicates whether to log results.</param>
    /// <returns>Returns error message if check fails to run correctly.</returns>
    public static string EMWTS2(cCategory category, ref bool log)
    {
      string returnVal = "";

      try
      {
        if (EmParameters.CurrentWeeklyTestSummary.TestTypeCd != "HGSI1")
        {
          EmParameters.WeeklyTestSummaryValid = false;
          category.CheckCatalogResult = "A";
        }
      }
      catch (Exception ex)
      {
        returnVal = category.CheckEngine.FormatError(ex);
      }

      return returnVal;
    }


    /// <summary>
    /// Check Weekly Test System
    /// </summary>
    /// <param name="category">Category Object</param>
    /// <param name="log">Indicates whether to log results.</param>
    /// <returns>Returns error message if check fails to run correctly.</returns>
    public static string EMWTS3(cCategory category, ref bool log)
    {
      string returnVal = "";

      try
      {
        if (EmParameters.CurrentWeeklyTestSummary.TestTypeCd == "HGSI1")
        {
          if (EmParameters.CurrentWeeklyTestSummary.MonSysId != null)
          {
            EmParameters.WeeklyTestSummaryValid = false;
            category.CheckCatalogResult = "A";
          }
        }
      }
      catch (Exception ex)
      {
        returnVal = category.CheckEngine.FormatError(ex);
      }

      return returnVal;
    }


    /// <summary>
    /// Check Weekly Test Component
    /// </summary>
    /// <param name="category">Category Object</param>
    /// <param name="log">Indicates whether to log results.</param>
    /// <returns>Returns error message if check fails to run correctly.</returns>
    public static string EMWTS4(cCategory category, ref bool log)
    {
      string returnVal = "";

      try
      {
        if (EmParameters.CurrentWeeklyTestSummary.TestTypeCd == "HGSI1")
        {
          if (EmParameters.CurrentWeeklyTestSummary.ComponentId == null)
          {
            EmParameters.WeeklyTestSummaryValid = false;
            category.CheckCatalogResult = "A";
          }
          else if (EmParameters.CurrentWeeklyTestSummary.ComponentTypeCd != "HG")
          {
            EmParameters.WeeklyTestSummaryValid = false;
            category.CheckCatalogResult = "B";
          }
        }
      }
      catch (Exception ex)
      {
        returnVal = category.CheckEngine.FormatError(ex);
      }

      return returnVal;
    }


    /// <summary>
    /// Check Weekly Test Date
    /// </summary>
    /// <param name="category">Category Object</param>
    /// <param name="log">Indicates whether to log results.</param>
    /// <returns>Returns error message if check fails to run correctly.</returns>
    public static string EMWTS5(cCategory category, ref bool log)
    {
      string returnVal = "";

      try
      {
        EmParameters.TestDateValid = false;

        if (EmParameters.CurrentWeeklyTestSummary.TestDate == null)
        {
          EmParameters.WeeklyTestSummaryValid = false;
          category.CheckCatalogResult = "A";
        }
        else if ((EmParameters.CurrentWeeklyTestSummary.TestDate < (new DateTime(1993, 1, 1))) ||
                 (EmParameters.CurrentWeeklyTestSummary.TestDate > EmParameters.CurrentReportingPeriodEndHour))
        {
          EmParameters.WeeklyTestSummaryValid = false;
          category.CheckCatalogResult = "B";
        }
        else
        {
          EmParameters.TestDateValid = true;
        }
      }
      catch (Exception ex)
      {
        returnVal = category.CheckEngine.FormatError(ex);
      }

      return returnVal;
    }


    /// <summary>
    /// Check Weekly Test Hour
    /// </summary>
    /// <param name="category">Category Object</param>
    /// <param name="log">Indicates whether to log results.</param>
    /// <returns>Returns error message if check fails to run correctly.</returns>
    public static string EMWTS6(cCategory category, ref bool log)
    {
      string returnVal = "";

      try
      {
        EmParameters.TestHourValid = false;

        if (EmParameters.CurrentWeeklyTestSummary.TestHour == null)
        {
          EmParameters.WeeklyTestSummaryValid = false;
          category.CheckCatalogResult = "A";
        }
        else if ((EmParameters.CurrentWeeklyTestSummary.TestHour < 0) || (23 < EmParameters.CurrentWeeklyTestSummary.TestHour))
        {
          EmParameters.WeeklyTestSummaryValid = false;
          category.CheckCatalogResult = "B";
        }
        else
        {
          EmParameters.TestHourValid = EmParameters.TestDateValid;
        }
      }
      catch (Exception ex)
      {
        returnVal = category.CheckEngine.FormatError(ex);
      }

      return returnVal;
    }


    /// <summary>
    /// Check Weekly Test Minute
    /// </summary>
    /// <param name="category">Category Object</param>
    /// <param name="log">Indicates whether to log results.</param>
    /// <returns>Returns error message if check fails to run correctly.</returns>
    public static string EMWTS7(cCategory category, ref bool log)
    {
      string returnVal = "";

      try
      {
        EmParameters.TestDateTimeValid = false;

        if (EmParameters.CurrentWeeklyTestSummary.TestMin == null)
        {
          EmParameters.WeeklyTestSummaryValid = false;
          category.CheckCatalogResult = "A";
        }
        else if ((EmParameters.CurrentWeeklyTestSummary.TestMin < 0) || (59 < EmParameters.CurrentWeeklyTestSummary.TestMin))
        {
          EmParameters.WeeklyTestSummaryValid = false;
          category.CheckCatalogResult = "B";
        }
        else if (EmParameters.TestHourValid.Default(false))
        {
          DateTime calcTestDateTime = EmParameters.CurrentWeeklyTestSummary.TestDate.Value.AddHours(EmParameters.CurrentWeeklyTestSummary.TestHour.Value).AddMinutes(EmParameters.CurrentWeeklyTestSummary.TestMin.Value);

          if (EmParameters.CurrentWeeklyTestSummary.TestDatetime != calcTestDateTime)
          {
            EmParameters.WeeklyTestSummaryValid = false;
            category.CheckCatalogResult = "C";
          }
          else
          {
            EmParameters.TestDateTimeValid = true;
          }
        }
      }
      catch (Exception ex)
      {
        returnVal = category.CheckEngine.FormatError(ex);
      }

      return returnVal;
    }


    /// <summary>
    /// Check Weekly Test Span Scale
    /// </summary>
    /// <param name="category">Category Object</param>
    /// <param name="log">Indicates whether to log results.</param>
    /// <returns>Returns error message if check fails to run correctly.</returns>
    public static string EMWTS8(cCategory category, ref bool log)
    {
      string returnVal = "";

      try
      {
        if (EmParameters.CurrentWeeklyTestSummary.SpanScaleCd == null)
        {
          EmParameters.WeeklyTestSummaryValid = false;
          category.CheckCatalogResult = "A";
        }
        else if (EmParameters.CurrentWeeklyTestSummary.SpanScaleCd.NotInList("H,M,L"))
        {
          EmParameters.WeeklyTestSummaryValid = false;
          category.CheckCatalogResult = "B";
        }
        else if (EmParameters.CurrentWeeklyTestSummary.TestTypeCd == "HGSI1")
        {
          if (EmParameters.CurrentWeeklyTestSummary.SpanScaleCd != "H")
          {
            EmParameters.WeeklyTestSummaryValid = false;
            category.CheckCatalogResult = "C";
          }
        }
      }
      catch (Exception ex)
      {
        returnVal = category.CheckEngine.FormatError(ex);
      }

      return returnVal;
    }


    /// <summary>
    /// Check Weekly Test Result
    /// </summary>
    /// <param name="category">Category Object</param>
    /// <param name="log">Indicates whether to log results.</param>
    /// <returns>Returns error message if check fails to run correctly.</returns>
    public static string EMWTS9(cCategory category, ref bool log)
    {
      string returnVal = "";

      try
      {
        EmParameters.TestResultValid = false;

        if (EmParameters.CurrentWeeklyTestSummary.TestResultCd == null)
        {
          EmParameters.WeeklyTestSummaryValid = false;
          category.CheckCatalogResult = "A";
        }
        else if (EmParameters.CurrentWeeklyTestSummary.TestResultCd.NotInList(EmParameters.TestResultCodeList))
        {
          EmParameters.WeeklyTestSummaryValid = false;
          category.CheckCatalogResult = "B";
        }
        else if (EmParameters.CurrentWeeklyTestSummary.TestTypeCd == "HGSI1")
        {
          if (EmParameters.CurrentWeeklyTestSummary.TestResultCd.NotInList("PASSED,PASSAPS,FAILED"))
          {
            EmParameters.WeeklyTestSummaryValid = false;
            category.CheckCatalogResult = "C";
          }
          else
          {
            EmParameters.TestResultValid = true;
          }
        }
        else
        {
          EmParameters.TestResultValid = true;
        }
      }
      catch (Exception ex)
      {
        returnVal = category.CheckEngine.FormatError(ex);
      }

      return returnVal;
    }

    #endregion

  }
}
