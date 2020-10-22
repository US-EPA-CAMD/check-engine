using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Extensions;


namespace ECMPS.Checks.TEEChecks
{
  public class cTEEChecks : cChecks
  {
    #region Constructors

    public cTEEChecks()
    {
      CheckProcedures = new dCheckProcedure[10];
      CheckProcedures[1] = new dCheckProcedure(EXTEXEM1);
      CheckProcedures[2] = new dCheckProcedure(EXTEXEM2);
      CheckProcedures[3] = new dCheckProcedure(EXTEXEM3);
      CheckProcedures[4] = new dCheckProcedure(EXTEXEM4);
      CheckProcedures[5] = new dCheckProcedure(EXTEXEM5);
      CheckProcedures[6] = new dCheckProcedure(EXTEXEM6);
      CheckProcedures[7] = new dCheckProcedure(EXTEXEM7);
      CheckProcedures[8] = new dCheckProcedure(EXTEXEM8);
      CheckProcedures[9] = new dCheckProcedure(EXTEXEM9);
    }


    #endregion


    #region Cert Event Checks

    public static string EXTEXEM1(cCategory Category, ref bool Log)
    //Test Extension/Exemption Year and Quarter Valid
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentTEE = (DataRowView)Category.GetCheckParameter("Current_Test_Extension_Exemption").ParameterValue;
        int RptPerId = cDBConvert.ToInteger(CurrentTEE["RPT_PERIOD_ID"]);
        if (RptPerId == int.MinValue)
          Category.CheckCatalogResult = "A";
        else
        {
          DataView RptPerTable = (DataView)Category.GetCheckParameter("Reporting_Period_Lookup_Table").ParameterValue;
          string OldFilter = RptPerTable.RowFilter;
          RptPerTable.RowFilter = AddToDataViewFilter(OldFilter, "RPT_PERIOD_ID = " + RptPerId);
          int Year = cDBConvert.ToInteger(RptPerTable[0]["CALENDAR_YEAR"]);
          int Qtr = cDBConvert.ToInteger(RptPerTable[0]["QUARTER"]);
          RptPerTable.RowFilter = OldFilter;
          DateTime QuarterLastDate = cDateFunctions.LastDateThisQuarter(Year, Qtr);
          DateTime QuarterFirstDate = cDateFunctions.StartDateThisQuarter(QuarterLastDate);
          Category.SetCheckParameter("Test_Extension_Exemption_Begin_Date", QuarterFirstDate, eParameterDataType.Date);
          Category.SetCheckParameter("Test_Extension_Exemption_End_Date", QuarterLastDate, eParameterDataType.Date);
          if (Year > DateTime.Now.Year || (Year == DateTime.Now.Year && Qtr > cDateFunctions.ThisQuarter(DateTime.Now)))
            Category.CheckCatalogResult = "B";
          if (cDBConvert.ToString(CurrentTEE["EXTENS_EXEMPT_CD"]) == "NONQAOS" && Qtr == 3)
            Category.CheckCatalogResult = "C";
        }
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "EXTEXEM1");
      }

      return ReturnVal;
    }

    public static string EXTEXEM2(cCategory Category, ref bool Log)
    //Test Extension/Exemption Extension or Exemption Code Valid
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentTEE = (DataRowView)Category.GetCheckParameter("Current_Test_Extension_Exemption").ParameterValue;
        string ExtExemCd = cDBConvert.ToString(CurrentTEE["EXTENS_EXEMPT_CD"]);
        if (ExtExemCd == "")
          Category.CheckCatalogResult = "A";
        else
        {
          DateTime TEEBeginDate = Convert.ToDateTime(Category.GetCheckParameter("Test_Extension_Exemption_Begin_Date").ParameterValue);
          if (ExtExemCd == "NONQAOS" && TEEBeginDate != DateTime.MinValue)
          {
            int TEERptPerId = cDBConvert.ToInteger(CurrentTEE["RPT_PERIOD_ID"]);
            DataView RepFreqRecs = (DataView)Category.GetCheckParameter("Location_Reporting_Frequency_Records").ParameterValue;
            string OldFilter = RepFreqRecs.RowFilter;
            RepFreqRecs.RowFilter = AddToDataViewFilter(OldFilter, "REPORT_FREQ_CD = 'Q' AND BEGIN_RPT_PERIOD_ID <= " + TEERptPerId + " AND (END_RPT_PERIOD_ID IS NULL OR END_RPT_PERIOD_ID >= " + TEERptPerId + ")");
            if (RepFreqRecs.Count > 0)
              Category.CheckCatalogResult = "B";
            RepFreqRecs.RowFilter = OldFilter;
          }
        }
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "EXTEXEM2");
      }

      return ReturnVal;
    }

    public static string EXTEXEM3(cCategory Category, ref bool Log)
    //Test Extension/Exemption System Valid
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentTEE = (DataRowView)Category.GetCheckParameter("Current_Test_Extension_Exemption").ParameterValue;
        string MonSysId = cDBConvert.ToString(CurrentTEE["MON_SYS_ID"]);
        string ExtExemCd = cDBConvert.ToString(CurrentTEE["EXTENS_EXEMPT_CD"]);
        if (MonSysId == "")
        {
          if (ExtExemCd.InList("LOWSYTD,LOWSQTR,NRB720,NONQAPB,GRACEPB,FLOWEXP,F2LEXP"))
            Category.CheckCatalogResult = "A";
        }
        else
          if (ExtExemCd.InList("LOWSYTD,LOWSQTR,NRB720,NONQAPB,GRACEPB,FLOWEXP,F2LEXP"))
          {
            DataView MonSysRecs = (DataView)Category.GetCheckParameter("Monitor_System_Records").ParameterValue;
            string OldFilter = MonSysRecs.RowFilter;
            MonSysRecs.RowFilter = AddToDataViewFilter(OldFilter, "MON_SYS_ID = '" + cDBConvert.ToString(CurrentTEE["MON_SYS_ID"]) + "'");

            string SysDesCd = cDBConvert.ToString(MonSysRecs[0]["SYS_DESIGNATION_CD"]);
            string sysTypeCd = MonSysRecs[0]["SYS_TYPE_CD"].AsString();

            if (ExtExemCd.InList("LOWSYTD,LOWSQTR") && !sysTypeCd.InList("SO2,SO2R"))
              Category.CheckCatalogResult = "B";
            else if (ExtExemCd == "NRB720" && SysDesCd != "B")
                Category.CheckCatalogResult = "C";
            else if (ExtExemCd.InList("FLOWEXP,F2LEXP") && sysTypeCd != "FLOW")
              Category.CheckCatalogResult = "G";
            else
            {
              DateTime TEEBeginDate = Convert.ToDateTime(Category.GetCheckParameter("Test_Extension_Exemption_Begin_Date").ParameterValue);
              if (TEEBeginDate != DateTime.MinValue)
              {
                DateTime TEEEndDate = Convert.ToDateTime(Category.GetCheckParameter("Test_Extension_Exemption_End_Date").ParameterValue);
                DateTime MonSysRecEndDate = cDBConvert.ToDate(MonSysRecs[0]["END_DATE"], DateTypes.END);
                if (cDBConvert.ToDate(MonSysRecs[0]["BEGIN_DATE"], DateTypes.START) > TEEEndDate || (MonSysRecEndDate != DateTime.MaxValue && MonSysRecEndDate < TEEBeginDate))
                  Category.CheckCatalogResult = "D";
              }
            }
            
            MonSysRecs.RowFilter = OldFilter;
          }
          else
            Category.CheckCatalogResult = "E";
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "EXTEXEM3");
      }

      return ReturnVal;
    }

    public static string EXTEXEM4(cCategory Category, ref bool Log)
    //Test Extension/Exemption Component Valid
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentTEE = (DataRowView)Category.GetCheckParameter("Current_Test_Extension_Exemption").ParameterValue;
        string CompId = cDBConvert.ToString(CurrentTEE["COMPONENT_ID"]);
        string ExtExemCd = cDBConvert.ToString(CurrentTEE["EXTENS_EXEMPT_CD"]);
        if (CompId == "")
        {
          if (ExtExemCd.InList("RANGENU,NONQAPB,NONQADB,FLOWEXP"))
            Category.CheckCatalogResult = "A";
        }
        else
        {
          DataView CompRecs = (DataView)Category.GetCheckParameter("Component_Records").ParameterValue;
          string OldFilter2 = CompRecs.RowFilter;
          if (ExtExemCd.InList("RANGENU,NONQAPB"))
          {
            DateTime TEEBeginDate = Convert.ToDateTime(Category.GetCheckParameter("Test_Extension_Exemption_Begin_Date").ParameterValue);
            if (TEEBeginDate != DateTime.MinValue)
            {
              DateTime TEEEndDate = Convert.ToDateTime(Category.GetCheckParameter("Test_Extension_Exemption_End_Date").ParameterValue);
              DataView AnalyzerRangeRecs = (DataView)Category.GetCheckParameter("Location_Analyzer_Range_Records").ParameterValue;
              string OldFilter1 = AnalyzerRangeRecs.RowFilter;
              AnalyzerRangeRecs.RowFilter = AddToDataViewFilter(OldFilter1, "COMPONENT_ID = '" + CompId + "'" + " AND BEGIN_DATE <= '" +
                  TEEEndDate.ToShortDateString() + "'" + " AND (END_DATE IS NULL OR END_DATE >= '" + TEEBeginDate.ToShortDateString() + "')");
              if (AnalyzerRangeRecs.Count == 0)
              {
                CompRecs.RowFilter = AddToDataViewFilter(OldFilter2, "COMPONENT_ID = '" + CompId + "'");
                if (ExtExemCd == "RANGENU")
                  if (CompRecs.Count == 0 || !cDBConvert.ToString(CompRecs[0]["COMPONENT_TYPE_CD"]).InList("NOX,SO2,CO2,O2"))
                    Category.CheckCatalogResult = "B";
                  else
                    Category.CheckCatalogResult = "C";
                else
                  if (CompRecs.Count == 0 || cDBConvert.ToString(CompRecs[0]["COMPONENT_TYPE_CD"]) != "NOX")
                    Category.CheckCatalogResult = "F";
                  else
                    Category.CheckCatalogResult = "C";
              }
              else
                if (ExtExemCd == "RANGENU" && cDBConvert.ToInteger(AnalyzerRangeRecs[0]["DUAL_RANGE_IND"]) != 1)
                  Category.CheckCatalogResult = "D";
              AnalyzerRangeRecs.RowFilter = OldFilter1;
            }
          }
          else if (ExtExemCd == "NONQADB")
          {
            CompRecs.RowFilter = AddToDataViewFilter(OldFilter2, "COMPONENT_ID = '" + CompId + "'");
            if (CompRecs.Count == 0 || !cDBConvert.ToString(CompRecs[0]["COMPONENT_TYPE_CD"]).InList("OFFM,GFFM"))
              Category.CheckCatalogResult = "G";
          }
          else if (ExtExemCd == "FLOWEXP")
          {
            CompRecs.RowFilter = AddToDataViewFilter(OldFilter2, "COMPONENT_ID = '" + CompId + "'");

            if (CompRecs.Count == 0 || (CompRecs[0]["COMPONENT_TYPE_CD"].AsString() != "FLOW"))
              Category.CheckCatalogResult = "H";
            else
            {
              DataView locationSystemComponentRecords = Category.GetCheckParameter("Location_System_Component_Records").AsDataView();

              string teeMonSysId = CurrentTEE["MON_SYS_ID"].AsString();
              int teeRptPeriodId = CurrentTEE["RPT_PERIOD_ID"].AsInteger(0);

              cReportingPeriod reportingPeriod = cReportingPeriod.GetReportingPeriod(teeRptPeriodId);

              DataView componentSystemComponentRecords 
                = cRowFilter.FindRows(locationSystemComponentRecords,
                                      new cFilterCondition[] { new cFilterCondition("COMPONENT_ID", CompId),
                                                               new cFilterCondition("BEGIN_DATE", reportingPeriod.BeganDate, eFilterDataType.DateBegan, eFilterConditionRelativeCompare.LessThanOrEqual),
                                                               new cFilterCondition("END_DATE", reportingPeriod.EndedDate, eFilterDataType.DateEnded, eFilterConditionRelativeCompare.GreaterThanOrEqual),
                                                               new cFilterCondition("MON_SYS_ID", teeMonSysId, true)});

              if (componentSystemComponentRecords.Count == 0)
                Category.CheckCatalogResult = "I";
            }
          }
          else
            Category.CheckCatalogResult = "E";

          // Reset Component View Filter
          CompRecs.RowFilter = OldFilter2;
        }
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "EXTEXEM4");
      }

      return ReturnVal;
    }

    public static string EXTEXEM5(cCategory Category, ref bool Log)
    //Test Extension/Exemption Span Scale Valid
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentTEE = (DataRowView)Category.GetCheckParameter("Current_Test_Extension_Exemption").ParameterValue;
        string SpanScaleCd = cDBConvert.ToString(CurrentTEE["SPAN_SCALE_CD"]);
        bool EECodeIsRANGENU = cDBConvert.ToString(CurrentTEE["EXTENS_EXEMPT_CD"]) == "RANGENU";
        if (CurrentTEE["SPAN_SCALE_CD"] == DBNull.Value)
        {
          if (EECodeIsRANGENU)
            Category.CheckCatalogResult = "A";
        }
        else
          if (!EECodeIsRANGENU)
            Category.CheckCatalogResult = "B";
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "EXTEXEM5");
      }

      return ReturnVal;
    }

    public static string EXTEXEM6(cCategory Category, ref bool Log)
    //Test Extension/Exemption Fuel Code Valid
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentTEE = (DataRowView)Category.GetCheckParameter("Current_Test_Extension_Exemption").ParameterValue;
        string FuelCd = cDBConvert.ToString(CurrentTEE["FUEL_CD"]);
        string ExtExemCd = cDBConvert.ToString(CurrentTEE["EXTENS_EXEMPT_CD"]);
        if (FuelCd == "")
        {
          if (ExtExemCd == "NONQAOS")
            Category.CheckCatalogResult = "A";
        }
        else
          if (ExtExemCd == "NONQAOS")
          {
            DataView MonSysRecs = (DataView)Category.GetCheckParameter("Monitor_System_Records").ParameterValue;
            string OldFilter = MonSysRecs.RowFilter;
            MonSysRecs.RowFilter = AddToDataViewFilter(OldFilter, "SYS_TYPE_CD IN ('OILV', 'OILM', 'GAS', 'LTOL', 'LTGS') AND FUEL_CD = '" + FuelCd + "'");
            if (MonSysRecs.Count == 0)
              Category.CheckCatalogResult = "B";
            else
            {
              DateTime TEEBeginDate = Convert.ToDateTime(Category.GetCheckParameter("Test_Extension_Exemption_Begin_Date").ParameterValue);
              if (TEEBeginDate != DateTime.MinValue)
              {
                DateTime TEEEndDate = Convert.ToDateTime(Category.GetCheckParameter("Test_Extension_Exemption_End_Date").ParameterValue);
                DateTime MonSysRecEndDate = cDBConvert.ToDate(MonSysRecs[0]["END_DATE"], DateTypes.END);
                if (cDBConvert.ToDate(MonSysRecs[0]["BEGIN_DATE"], DateTypes.START) > TEEEndDate || (MonSysRecEndDate != DateTime.MaxValue && MonSysRecEndDate < TEEBeginDate))
                  Category.CheckCatalogResult = "C";
              }
            }
            MonSysRecs.RowFilter = OldFilter;
          }
          else
            Category.CheckCatalogResult = "D";
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "EXTEXEM6");
      }

      return ReturnVal;
    }

    public static string EXTEXEM7(cCategory Category, ref bool Log)
    //Test Extension/Exemption Hours Used Valid
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentTEE = (DataRowView)Category.GetCheckParameter("Current_Test_Extension_Exemption").ParameterValue;
        int HrsUsed = cDBConvert.ToInteger(CurrentTEE["HOURS_USED"]);
        if (HrsUsed != int.MinValue && HrsUsed < 0)
          Category.CheckCatalogResult = "A";
        else
        {
          string ExtExemCd = cDBConvert.ToString(CurrentTEE["EXTENS_EXEMPT_CD"]);

          if (ExtExemCd.InList("RANGENU,LOWSQTR"))
          {
            if (HrsUsed > 0)
              Category.CheckCatalogResult = "B";
          }
          else if (ExtExemCd.NotInList("FLOWEXP,F2LEXP"))
          {
            if (HrsUsed == int.MinValue)
              if (ExtExemCd == "NONQAOS" && Convert.ToDateTime(Category.GetCheckParameter("Test_Extension_Exemption_Begin_Date").ParameterValue) < Category.GetCheckParameter("ECMPS_MP_Begin_Date").ValueAsDateTime(DateTypes.START))
                Category.CheckCatalogResult = "C";
              else
                Category.CheckCatalogResult = "D";
            if (string.IsNullOrEmpty(Category.CheckCatalogResult))
            {
              switch (ExtExemCd)
              {
                case "LOWSYTD":
                  if (HrsUsed > 480)
                    Category.CheckCatalogResult = "E";
                  break;
                case "NRB720":
                  if (HrsUsed > 720)
                    Category.CheckCatalogResult = "E";
                  break;
                case "NONQADB":
                case "NONQAPB":
                case "NONQAOS":
                  if (HrsUsed > 168)
                    Category.CheckCatalogResult = "E";
                  break;
                default:
                  break;
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "EXTEXEM7");
      }

      return ReturnVal;
    }

    public static string EXTEXEM8(cCategory Category, ref bool Log)
    //Duplicate Test Extension/Exemption
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentTEE = (DataRowView)Category.GetCheckParameter("Current_Test_Extension_Exemption").ParameterValue;
        string ExtExemCd = cDBConvert.ToString(CurrentTEE["EXTENS_EXEMPT_CD"]);
        int RptPerId = cDBConvert.ToInteger(CurrentTEE["RPT_PERIOD_ID"]);
        if (ExtExemCd != "" && RptPerId != int.MinValue)
        {
          string MonSysId = cDBConvert.ToString(CurrentTEE["MON_SYS_ID"]);
          string FuelCd = cDBConvert.ToString(CurrentTEE["FUEL_CD"]);
          string CompId = cDBConvert.ToString(CurrentTEE["COMPONENT_ID"]);
          DataView TEETests = (DataView)Category.GetCheckParameter("Test_Extension_Exemption_Records").ParameterValue;
          string OldFilter = TEETests.RowFilter;
          if (FuelCd != "")
          {
            if (MonSysId != "" && CompId != "")
              TEETests.RowFilter = AddToDataViewFilter(OldFilter, "EXTENS_EXEMPT_CD = '" + ExtExemCd + "'" + " AND RPT_PERIOD_ID = " + RptPerId +
                  " AND MON_SYS_ID = '" + MonSysId + "'" + " AND COMPONENT_ID = '" + CompId + "'" + " AND FUEL_CD = '" + FuelCd + "'");
            else
              if (MonSysId == "" && CompId != "")
                TEETests.RowFilter = AddToDataViewFilter(OldFilter, "EXTENS_EXEMPT_CD = '" + ExtExemCd + "'" + " AND RPT_PERIOD_ID = " + RptPerId +
                    " AND MON_SYS_ID IS NULL AND COMPONENT_ID = '" + CompId + "'" + " AND FUEL_CD = '" + FuelCd + "'");
              else
                if (MonSysId != "" && CompId == "")
                  TEETests.RowFilter = AddToDataViewFilter(OldFilter, "EXTENS_EXEMPT_CD = '" + ExtExemCd + "'" + " AND RPT_PERIOD_ID = " + RptPerId +
                      " AND MON_SYS_ID = '" + MonSysId + "'" + " AND COMPONENT_ID IS NULL" + " AND FUEL_CD = '" + FuelCd + "'");
                else
                  if (MonSysId == "" && CompId == "")
                    TEETests.RowFilter = AddToDataViewFilter(OldFilter, "EXTENS_EXEMPT_CD = '" + ExtExemCd + "'" + " AND RPT_PERIOD_ID = " + RptPerId +
                        " AND MON_SYS_ID IS NULL AND COMPONENT_ID IS NULL" + " AND FUEL_CD = '" + FuelCd + "'");
          }
          else
          {
            if (MonSysId != "" && CompId != "")
              TEETests.RowFilter = AddToDataViewFilter(OldFilter, "EXTENS_EXEMPT_CD = '" + ExtExemCd + "'" + " AND RPT_PERIOD_ID = " + RptPerId +
                  " AND MON_SYS_ID = '" + MonSysId + "'" + " AND COMPONENT_ID = '" + CompId + "'" + " AND FUEL_CD IS NULL");
            else
              if (MonSysId == "" && CompId != "")
                TEETests.RowFilter = AddToDataViewFilter(OldFilter, "EXTENS_EXEMPT_CD = '" + ExtExemCd + "'" + " AND RPT_PERIOD_ID = " + RptPerId +
                    " AND MON_SYS_ID IS NULL AND COMPONENT_ID = '" + CompId + "'" + " AND FUEL_CD IS NULL");
              else
                if (MonSysId != "" && CompId == "")
                  TEETests.RowFilter = AddToDataViewFilter(OldFilter, "EXTENS_EXEMPT_CD = '" + ExtExemCd + "'" + " AND RPT_PERIOD_ID = " + RptPerId +
                      " AND MON_SYS_ID = '" + MonSysId + "'" + " AND COMPONENT_ID IS NULL" + " AND FUEL_CD IS NULL");
                else
                  if (MonSysId == "" && CompId == "")
                    TEETests.RowFilter = AddToDataViewFilter(OldFilter, "EXTENS_EXEMPT_CD = '" + ExtExemCd + "'" + " AND RPT_PERIOD_ID = " + RptPerId +
                        " AND MON_SYS_ID IS NULL AND COMPONENT_ID IS NULL" + " AND FUEL_CD IS NULL");
          }


          if ((TEETests.Count > 0 && CurrentTEE["TEST_EXTENSION_EXEMPTION_ID"] == DBNull.Value) ||
              (TEETests.Count > 1 && CurrentTEE["TEST_EXTENSION_EXEMPTION_ID"] != DBNull.Value) ||
              (TEETests.Count == 1 && CurrentTEE["TEST_EXTENSION_EXEMPTION_ID"] != DBNull.Value && CurrentTEE["TEST_EXTENSION_EXEMPTION_ID"].ToString() != TEETests[0]["TEST_EXTENSION_EXEMPTION_ID"].ToString()))
            Category.CheckCatalogResult = "A";
          TEETests.RowFilter = OldFilter;
        }
        else
          Log = false;
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "EXTEXEM8");
      }

      return ReturnVal;
    }

    public static string EXTEXEM9(cCategory Category, ref bool Log)
    //MP Evaluation Check
    {
      string ReturnVal = "";

      try
      {
        DataView MPLocationRecords = (DataView)Category.GetCheckParameter("Monitoring_Plan_Location_Records_for_QA").ParameterValue;
        DataRowView CurrentTEE = (DataRowView)Category.GetCheckParameter("Current_Test_Extension_Exemption").ParameterValue;
        int TEEYear = cDBConvert.ToInteger(CurrentTEE["CALENDAR_YEAR"]);
        int TEEQuarer = cDBConvert.ToInteger(CurrentTEE["QUARTER"]);
        string OldFilter = MPLocationRecords.RowFilter;
        MPLocationRecords.RowFilter = AddToDataViewFilter(OldFilter, "(SEVERITY_CD = 'CRIT1' OR SEVERITY_CD = 'FATAL') AND (END_YEAR IS NULL OR END_QUARTER IS NULL OR END_YEAR > " + TEEYear +
            " OR (END_YEAR = " + TEEYear + " AND END_QUARTER >= " + TEEQuarer + "))");
        if (MPLocationRecords.Count > 0)
          Category.CheckCatalogResult = "A";
        else
        {
          MPLocationRecords.RowFilter = AddToDataViewFilter(OldFilter, "(NEEDS_EVAL_FLG = 'Y' AND MUST_SUBMIT = 'Y') AND (END_YEAR IS NULL OR END_QUARTER IS NULL OR END_YEAR > " +
              TEEYear + " OR (END_YEAR = " + TEEYear + " AND END_QUARTER >= " + TEEQuarer + "))");
          if (MPLocationRecords.Count > 0)
            Category.CheckCatalogResult = "B";
        }
        MPLocationRecords.RowFilter = OldFilter;
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "EXTEXEM9");
      }

      return ReturnVal;
    }

    #endregion
  }
}
