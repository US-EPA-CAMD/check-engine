using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.EmissionsReport;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;
using ECMPS.Definitions.Extensions;

namespace ECMPS.Checks.EmissionsChecks
{
  public class cDailyEmissionTestChecks : cEmissionsChecks
  {
    #region Constructors

    public cDailyEmissionTestChecks(cEmissionsReportProcess emissionReportProcess)
      : base(emissionReportProcess)
    {
      CheckProcedures = new dCheckProcedure[7];

      CheckProcedures[1] = new dCheckProcedure(EMTEST1);
      CheckProcedures[2] = new dCheckProcedure(EMTEST2);
      CheckProcedures[3] = new dCheckProcedure(EMTEST3);
      CheckProcedures[4] = new dCheckProcedure(EMTEST4);
      CheckProcedures[5] = new dCheckProcedure(EMTEST5);
      CheckProcedures[6] = new dCheckProcedure(EMTEST6);
    }

    #endregion

    #region Public Static Methods: Checks

    public string EMTEST1(cCategory Category, ref bool Log)
    //Daily Test Date Valid
    {
      string ReturnVal = "";

      try
      {
        Category.SetCheckParameter("EM_Test_Date_Valid", true, eParameterDataType.Boolean);
        DataRowView CurrentDailyEMTest = (DataRowView)Category.GetCheckParameter("Current_Daily_Emission_Test").ParameterValue;
        DateTime EMTestDate = cDBConvert.ToDate(CurrentDailyEMTest["DAILY_TEST_DATE"], DateTypes.START);
        if (EMTestDate == DateTime.MinValue)
        {
          Category.SetCheckParameter("EM_Test_Date_Valid", false, eParameterDataType.Boolean);
          Category.CheckCatalogResult = "A";
        }
        else
          if (EMTestDate < new DateTime(1993, 1, 1) || EMTestDate > DateTime.Now)
          {
            Category.SetCheckParameter("EM_Test_Date_Valid", false, eParameterDataType.Boolean);
            Category.CheckCatalogResult = "B";
          }
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "EMTEST1");
      }

      return ReturnVal;
    }

    public string EMTEST2(cCategory Category, ref bool Log)
    //Daily Test Hour Valid
    {
      string ReturnVal = "";

      try
      {
        Category.SetCheckParameter("EM_Test_Hour_Valid", true, eParameterDataType.Boolean);
        DataRowView CurrentDailyEMTest = (DataRowView)Category.GetCheckParameter("Current_Daily_Emission_Test").ParameterValue;
        int EMTestHour = cDBConvert.ToInteger(CurrentDailyEMTest["DAILY_TEST_HOUR"]);
        if (EMTestHour == int.MinValue)
        {
          Category.SetCheckParameter("EM_Test_Hour_Valid", false, eParameterDataType.Boolean);
          Category.CheckCatalogResult = "A";
        }
        else
          if (EMTestHour < 0 || 23 < EMTestHour)
          {
            Category.SetCheckParameter("EM_Test_Hour_Valid", false, eParameterDataType.Boolean);
            Category.CheckCatalogResult = "B";
          }
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "EMTEST2");
      }

      return ReturnVal;
    }

    public string EMTEST3(cCategory Category, ref bool Log)
    //Daily Test Minute Valid
    {
      string ReturnVal = "";

      try
      {
        Category.SetCheckParameter("EM_Test_Minute_Valid", true, eParameterDataType.Boolean);
        DataRowView CurrentDailyEMTest = (DataRowView)Category.GetCheckParameter("Current_Daily_Emission_Test").ParameterValue;
        int EMTestMin = cDBConvert.ToInteger(CurrentDailyEMTest["DAILY_TEST_MIN"]);
        if (EMTestMin == int.MinValue)
          if (!Category.GetCheckParameter("Legacy_Data_Evaluation").ValueAsBool())
          {
            Category.SetCheckParameter("EM_Test_Minute_Valid", false, eParameterDataType.Boolean);
            Category.CheckCatalogResult = "A";
          }
          else
            Category.CheckCatalogResult = "B";
        else
          if (EMTestMin < 0 || 59 < EMTestMin)
          {
            Category.SetCheckParameter("EM_Test_Minute_Valid", false, eParameterDataType.Boolean);
            Category.CheckCatalogResult = "C";
          }
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "EMTEST3");
      }

      return ReturnVal;
    }

    public string EMTEST4(cCategory Category, ref bool Log)
    //Daily Test System or Component Valid
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentDailyEMTest = (DataRowView)Category.GetCheckParameter("Current_Daily_Emission_Test").ParameterValue;
        string TestTypeCd = cDBConvert.ToString(CurrentDailyEMTest["TEST_TYPE_CD"]);
        string MonSysId = cDBConvert.ToString(CurrentDailyEMTest["MON_SYS_ID"]);
        string CompId = cDBConvert.ToString(CurrentDailyEMTest["COMPONENT_ID"]);

        if (MonSysId != "" && CompId != "")
          Category.CheckCatalogResult = "A";
        else
          if (TestTypeCd == "INTCHK")
          {
            if (CompId == "")
              Category.CheckCatalogResult = "B";
            else
              if (cDBConvert.ToString(CurrentDailyEMTest["COMPONENT_TYPE_CD"]) != "FLOW")
                Category.CheckCatalogResult = "C";
          }
          else
            if (TestTypeCd == "PEMSCAL")
              if (MonSysId == "")
                Category.CheckCatalogResult = "D";
              else
                if (cDBConvert.ToString(CurrentDailyEMTest["SYS_TYPE_CD"]) != "NOXP")
                  Category.CheckCatalogResult = "E";
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "EMTEST4");
      }

      return ReturnVal;
    }

    public string EMTEST5(cCategory Category, ref bool Log)
    //Daily Test Span Scale Valid
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentDailyEMTest = (DataRowView)Category.GetCheckParameter("Current_Daily_Emission_Test").ParameterValue;
        if (CurrentDailyEMTest["SPAN_SCALE_CD"] != DBNull.Value)
          Category.CheckCatalogResult = "A";
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "EMTEST5");
      }

      return ReturnVal;
    }

    public string EMTEST6(cCategory category, ref bool log)
    //Daily Test Result Code Valid
    {
      string ReturnVal = "";

      try
      {
        EmTestCalcResult.SetValue(null, category);

        string testResultCd = CurrentDailyEmissionTest.Value["TEST_RESULT_CD"].AsString();

        if (testResultCd == null)
        {
          category.CheckCatalogResult = "A";
        }
        else if (testResultCd.NotInList("ABORTED,PASSED,FAILED"))
        {
          category.CheckCatalogResult = "B";
        }
        else
        {
          EmTestCalcResult.SetValue(testResultCd, category);

          if ((CurrentDailyEmissionTest.Value["TEST_TYPE_CD"].AsString() == "INTCHK") &&
              EmTestDateValid.Value.Default(false) && 
              EmTestHourValid.Value.Default(false) && 
              (CurrentOperatingTime.Value.Default() == 0))
          {
            IgnoredDailyInterferenceTests.SetValue(true, category);
            EmTestCalcResult.SetValue("IGNORED", category);
          }
        }
      }
      catch (Exception ex)
      {
        ReturnVal = category.CheckEngine.FormatError(ex, "EMTEST6");
      }

      return ReturnVal;
    }

    #endregion
  }
}
