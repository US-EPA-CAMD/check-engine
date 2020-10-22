using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

namespace ECMPS.Checks.FlowLoadCheckChecks
{
  public class cFlowLoadCheckChecks : cChecks
  {
    #region Constructors

    public cFlowLoadCheckChecks()
    {
        CheckProcedures = new dCheckProcedure[20];
        CheckProcedures[1] = new dCheckProcedure(F2LCHK1);
        CheckProcedures[2] = new dCheckProcedure(F2LCHK2);        
        CheckProcedures[3] = new dCheckProcedure(F2LCHK3);        
        CheckProcedures[4] = new dCheckProcedure(F2LCHK4);        
        CheckProcedures[5] = new dCheckProcedure(F2LCHK5);
        CheckProcedures[6] = new dCheckProcedure(F2LCHK6);
        CheckProcedures[7] = new dCheckProcedure(F2LCHK7);
        CheckProcedures[8] = new dCheckProcedure(F2LCHK8);
        CheckProcedures[9] = new dCheckProcedure(F2LCHK9);
        CheckProcedures[10] = new dCheckProcedure(F2LCHK10);
        CheckProcedures[11] = new dCheckProcedure(F2LCHK11);
        CheckProcedures[12] = new dCheckProcedure(F2LCHK12);
        CheckProcedures[13] = new dCheckProcedure(F2LCHK13);
        CheckProcedures[14] = new dCheckProcedure(F2LCHK14);
        CheckProcedures[15] = new dCheckProcedure(F2LCHK15);
        CheckProcedures[16] = new dCheckProcedure(F2LCHK16);
        CheckProcedures[17] = new dCheckProcedure(F2LCHK17);
        CheckProcedures[18] = new dCheckProcedure(F2LCHK18);
        CheckProcedures[19] = new dCheckProcedure(F2LCHK19);
        
    }


    #endregion


    #region Flow to Load Check Checks
      public static string F2LCHK1(cCategory Category, ref bool Log)
      //System Type Valid
      {
          string ReturnVal = "";

          try
          {
              DataRowView CurrentFlowToLoad = (DataRowView)Category.GetCheckParameter("Current_Flow_to_Load_Check").ParameterValue;
              if (CurrentFlowToLoad["MON_SYS_ID"] == DBNull.Value)
              {
                  Category.SetCheckParameter("Flow_to_Load_Check_System_Valid", false, eParameterDataType.Boolean);
                  Category.CheckCatalogResult = "A";
              }
              else
                  if (cDBConvert.ToString(CurrentFlowToLoad["SYS_TYPE_CD"]) == "FLOW")
                      Category.SetCheckParameter("Flow_to_Load_Check_System_Valid", true, eParameterDataType.Boolean);
                  else
                  {
                      Category.SetCheckParameter("Flow_to_Load_Check_System_Valid", false, eParameterDataType.Boolean);
                      Category.CheckCatalogResult = "B";
                  }
          }

          catch (Exception ex)
          {
              ReturnVal = Category.CheckEngine.FormatError(ex, "F2LCHK1");
          }

          return ReturnVal;
      }

      public static string F2LCHK2(cCategory Category, ref bool Log)
      //Flow to Load Check Test Reason Code Valid
      {
          string ReturnVal = "";

          try
          {
              DataRowView CurrentFlowToLoad = (DataRowView)Category.GetCheckParameter("Current_Flow_to_Load_Check").ParameterValue;
              string TestReasCd = cDBConvert.ToString(CurrentFlowToLoad["TEST_REASON_CD"]);
              if (TestReasCd == "")
              {
                  DateTime MPDate = Category.GetCheckParameter("ECMPS_MP_Begin_Date").ValueAsDateTime(DateTypes.START);
                  if (Convert.ToDateTime(Category.GetCheckParameter("Test_Reporting_Period_Begin_Date").ParameterValue) >= MPDate)
                      Category.CheckCatalogResult = "A";
                  else
                      Category.CheckCatalogResult = "B";
              }
              else
              {
                  if (TestReasCd != "QA")
                  {
                      DataView TestReasLookup = (DataView)Category.GetCheckParameter("Test_Reason_Code_Lookup_Table").ParameterValue;
                      if (!LookupCodeExists(TestReasCd, TestReasLookup))                      
                          Category.CheckCatalogResult = "C";
                      else
                          Category.CheckCatalogResult = "D";                     
                  }
              }
          }
          catch (Exception ex)
          {
              ReturnVal = Category.CheckEngine.FormatError(ex, "F2LCHK2");
          }

          return ReturnVal;
      }

      public static string F2LCHK3(cCategory Category, ref bool Log)
      //Flow to Load Check Operating Level Valid
      {
          string ReturnVal = "";

          try
          {
              DataRowView CurrentFlowToLoad = (DataRowView)Category.GetCheckParameter("Current_Flow_to_Load_Check").ParameterValue;
              if (CurrentFlowToLoad["OP_LEVEL_CD"] == DBNull.Value)
              {
                  Category.SetCheckParameter("Flow_to_Load_Check_Operating_Level_Code_Valid", false, eParameterDataType.Boolean);
                  Category.CheckCatalogResult = "A";
              }
              else
              {
                  string CurrentOpLvlCd = cDBConvert.ToString(CurrentFlowToLoad["OP_LEVEL_CD"]);
                  DataView OpCdLookupTbl = (DataView)Category.GetCheckParameter("Operating_Level_Code_Lookup_Table").ParameterValue;
                  string OldFilter = OpCdLookupTbl.RowFilter;
                  OpCdLookupTbl.RowFilter = AddToDataViewFilter(OldFilter, "OP_LEVEL_CD = '" + CurrentOpLvlCd + "'");
                  if (OpCdLookupTbl.Count == 0)
                  {
                      Category.SetCheckParameter("Flow_to_Load_Check_Operating_Level_Code_Valid", false, eParameterDataType.Boolean);
                      Category.CheckCatalogResult = "B";
                  }
                  Category.SetCheckParameter("Flow_to_Load_Check_Operating_Level_Code_Valid", true, eParameterDataType.Boolean);
                  OpCdLookupTbl.RowFilter = OldFilter;
              }
          }
          catch (Exception ex)
          {
              ReturnVal = Category.CheckEngine.FormatError(ex, "F2LCHK3");
          }

          return ReturnVal;
      }

      public static string F2LCHK4(cCategory Category, ref bool Log)
      //Flow to Load Check Data Test Basis Valid
      {
          string ReturnVal = "";

          try
          {
              DataRowView CurrentFlowToLoad = (DataRowView)Category.GetCheckParameter("Current_Flow_to_Load_Check").ParameterValue;
              if (CurrentFlowToLoad["TEST_BASIS_CD"] == DBNull.Value)
              {
                  string TestResultCD = cDBConvert.ToString(CurrentFlowToLoad["TEST_RESULT_CD"]);
                  if (TestResultCD == "PASSED" || TestResultCD == "FAILED")
                      Category.CheckCatalogResult = "A";
              }
              else
              {
                  DataView TestBasisTable = (DataView)Category.GetCheckParameter("Test_Basis_Code_Lookup_Table").ParameterValue;
                  string OldFilter = TestBasisTable.RowFilter;
                  string TestBasisCD = cDBConvert.ToString(CurrentFlowToLoad["TEST_BASIS_CD"]);
                  TestBasisTable.RowFilter = AddToDataViewFilter(OldFilter, "TEST_BASIS_CD = '" + TestBasisCD + "'");
                  if (TestBasisTable.Count == 0)
                      Category.CheckCatalogResult = "B";
              }
          }
          catch (Exception ex)
          {
              ReturnVal = Category.CheckEngine.FormatError(ex, "F2LCHK4");
          }

          return ReturnVal;
      }

      public static string F2LCHK5(cCategory Category, ref bool Log)
      //Flow to Load Check Data Bias Adjusted Indicator Valid
      {
          string ReturnVal = "";

          try
          {
              DataRowView CurrentFlowToLoad = (DataRowView)Category.GetCheckParameter("Current_Flow_to_Load_Check").ParameterValue;
              if (CurrentFlowToLoad["BIAS_ADJUSTED_IND"] == DBNull.Value)
              {
                  string TestResultCD = cDBConvert.ToString(CurrentFlowToLoad["TEST_RESULT_CD"]);
                  if (TestResultCD == "PASSED" || TestResultCD == "FAILED")
                      Category.CheckCatalogResult = "A";
              }
          }
          catch (Exception ex)
          {
              ReturnVal = Category.CheckEngine.FormatError(ex, "F2LCHK5");
          }

          return ReturnVal;
      }

      public static string F2LCHK6(cCategory Category, ref bool Log)
      //Flow to Load Check Data Number of Hours Valid
      {
          string ReturnVal = "";

          try
          {
              DataRowView CurrentFlowToLoad = (DataRowView)Category.GetCheckParameter("Current_Flow_to_Load_Check").ParameterValue;
              string TestResultCD = cDBConvert.ToString(CurrentFlowToLoad["TEST_RESULT_CD"]);
              int NumHrs = cDBConvert.ToInteger(CurrentFlowToLoad["NUM_HRS"]);
              if (TestResultCD == "PASSED" || TestResultCD == "FAILED")
              {
                  if (NumHrs == int.MinValue)
                      Category.CheckCatalogResult = "A";
                  else
                      if (NumHrs < 168)
                          Category.CheckCatalogResult = "B";
              }
              else
                  if (TestResultCD == "FEW168H" || TestResultCD == "EXC168H")
                      if (NumHrs >= 168)
                          Category.CheckCatalogResult = "C";
                      else
                          if (NumHrs != int.MinValue)
                              Category.CheckCatalogResult = "D";
          }
          catch (Exception ex)
          {
              ReturnVal = Category.CheckEngine.FormatError(ex, "F2LCHK6");
          }

          return ReturnVal;
      }

      public static string F2LCHK7(cCategory Category, ref bool Log)
      //Flow to Load Check Data Number of Hours Excluded for Fuel Valid
      {
          string ReturnVal = "";

          try
          {
              DataRowView CurrentFlowToLoad = (DataRowView)Category.GetCheckParameter("Current_Flow_to_Load_Check").ParameterValue;
              int NHEFuel = cDBConvert.ToInteger(CurrentFlowToLoad["NHE_FUEL"]);
              if (NHEFuel != int.MinValue && NHEFuel < 0)
                  Category.CheckCatalogResult = "A";
          }
          catch (Exception ex)
          {
              ReturnVal = Category.CheckEngine.FormatError(ex, "F2LCHK7");
          }

          return ReturnVal;
      }
      
      public static string F2LCHK8(cCategory Category, ref bool Log)
      //Flow to Load Check Data Number of Hours Excluded for Ramping Valid
      {
          string ReturnVal = "";

          try
          {
              DataRowView CurrentFlowToLoad = (DataRowView)Category.GetCheckParameter("Current_Flow_to_Load_Check").ParameterValue;
              int NHERamping = cDBConvert.ToInteger(CurrentFlowToLoad["NHE_RAMPING"]);
              if (NHERamping != int.MinValue && NHERamping < 0)
                  Category.CheckCatalogResult = "A";
          }
          catch (Exception ex)
          {
              ReturnVal = Category.CheckEngine.FormatError(ex, "F2LCHK8");
          }

          return ReturnVal;
      }

      public static string F2LCHK9(cCategory Category, ref bool Log)
      //Flow to Load Check Data Number of Hours Excluded for Bypass Valid
      {
          string ReturnVal = "";

          try
          {
              DataRowView CurrentFlowToLoad = (DataRowView)Category.GetCheckParameter("Current_Flow_to_Load_Check").ParameterValue;
              int NHEBypass = cDBConvert.ToInteger(CurrentFlowToLoad["NHE_BYPASS"]);
              if (NHEBypass != int.MinValue && NHEBypass < 0)
                  Category.CheckCatalogResult = "A";
          }
          catch (Exception ex)
          {
              ReturnVal = Category.CheckEngine.FormatError(ex, "F2LCHK9");
          }

          return ReturnVal;
      }

      public static string F2LCHK10(cCategory Category, ref bool Log)
      //Flow to Load Check Data Number of Hours Excluded Pre RATA Valid
      {
          string ReturnVal = "";

          try
          {
              DataRowView CurrentFlowToLoad = (DataRowView)Category.GetCheckParameter("Current_Flow_to_Load_Check").ParameterValue;
              int NHEPreRATA = cDBConvert.ToInteger(CurrentFlowToLoad["NHE_PRE_RATA"]);
              if (NHEPreRATA != int.MinValue && NHEPreRATA < 0)
                  Category.CheckCatalogResult = "A";
          }
          catch (Exception ex)
          {
              ReturnVal = Category.CheckEngine.FormatError(ex, "F2LCHK10");
          }

          return ReturnVal;
      }

      public static string F2LCHK11(cCategory Category, ref bool Log)
      //Flow to Load Check Data Number of Hours Excluded Test Valid
      {
          string ReturnVal = "";

          try
          {
              DataRowView CurrentFlowToLoad = (DataRowView)Category.GetCheckParameter("Current_Flow_to_Load_Check").ParameterValue;
              int NHETest = cDBConvert.ToInteger(CurrentFlowToLoad["NHE_TEST"]);
              if (NHETest != int.MinValue && NHETest < 0)
                  Category.CheckCatalogResult = "A";
          }
          catch (Exception ex)
          {
              ReturnVal = Category.CheckEngine.FormatError(ex, "F2LCHK11");
          }

          return ReturnVal;
      }

      public static string F2LCHK12(cCategory Category, ref bool Log)
      //Flow to Load Check Data Number of Hours Excluded for Main and Bypass Valid
      {
          string ReturnVal = "";

          try
          {
              DataRowView CurrentFlowToLoad = (DataRowView)Category.GetCheckParameter("Current_Flow_to_Load_Check").ParameterValue;
              int NHEMainBypass = cDBConvert.ToInteger(CurrentFlowToLoad["NHE_MAIN_BYPASS"]);
              if (NHEMainBypass != int.MinValue && NHEMainBypass < 0)
                  Category.CheckCatalogResult = "A";
          }
          catch (Exception ex)
          {
              ReturnVal = Category.CheckEngine.FormatError(ex, "F2LCHK12");
          }

          return ReturnVal;
      }

      public static string F2LCHK13(cCategory Category, ref bool Log)
      //Flow to Load Check Total Hours Valid
      {
          string ReturnVal = "";

          try
          {
              DataRowView CurrentFlowToLoad = (DataRowView)Category.GetCheckParameter("Current_Flow_to_Load_Check").ParameterValue;
              int[] HoursArray = { cDBConvert.ToInteger(CurrentFlowToLoad["NUM_HRS"]), cDBConvert.ToInteger(CurrentFlowToLoad["NHE_FUEL"]),
                  cDBConvert.ToInteger(CurrentFlowToLoad["NHE_RAMPING"]), cDBConvert.ToInteger(CurrentFlowToLoad["NHE_BYPASS"]), 
                  cDBConvert.ToInteger(CurrentFlowToLoad["NHE_PRE_RATA"]), cDBConvert.ToInteger(CurrentFlowToLoad["NHE_TEST"]),
                  cDBConvert.ToInteger(CurrentFlowToLoad["NHE_MAIN_BYPASS"]) };
              int sum = 0;
              for (int i = 0; i < HoursArray.Length; i++)
                  sum += Math.Max(0, HoursArray[i]);
              if (sum > 2209)
                  Category.CheckCatalogResult = "A";
          }
          catch (Exception ex)
          {
              ReturnVal = Category.CheckEngine.FormatError(ex, "F2LCHK13");
          }

          return ReturnVal;
      }

      public static string F2LCHK14(cCategory Category, ref bool Log)
      //Identification of Previously Reported Test or Test Number for Flow to Load Check
      {
          string ReturnVal = "";

          try
          {
              DataRowView CurrentFlowToLoad = (DataRowView)Category.GetCheckParameter("Current_Flow_to_Load_Check").ParameterValue;
              string MonSysID = cDBConvert.ToString(CurrentFlowToLoad["MON_SYS_ID"]);
              if (Convert.ToBoolean(Category.GetCheckParameter("Flow_to_Load_Check_Operating_Level_Code_Valid").ParameterValue) &&
                  Convert.ToBoolean(Category.GetCheckParameter("Test_Reporting_Period_Valid").ParameterValue) && MonSysID != "")
              {
                  Category.SetCheckParameter("Flow_to_Load_Check_Supp_Data_ID", null, eParameterDataType.String);                  
                  DataView F2LChecks = (DataView)Category.GetCheckParameter("Flow_to_Load_Check_Records").ParameterValue;
                  string OldFilter = F2LChecks.RowFilter;
                  string OpLvlCD = cDBConvert.ToString(CurrentFlowToLoad["OP_LEVEL_CD"]);
                  string TestNum = cDBConvert.ToString(CurrentFlowToLoad["TEST_NUM"]);
                  int RptPeriodID = cDBConvert.ToInteger(CurrentFlowToLoad["RPT_PERIOD_ID"]);                  
                  F2LChecks.RowFilter = AddToDataViewFilter(OldFilter, "OP_LEVEL_CD = '" + OpLvlCD + "'" + " AND RPT_PERIOD_ID = " + RptPeriodID +
                      " AND TEST_NUM <> '" + TestNum + "'");
                  if ((F2LChecks.Count > 0 && CurrentFlowToLoad["TEST_SUM_ID"] == DBNull.Value) ||
                      (F2LChecks.Count > 1 && CurrentFlowToLoad["TEST_SUM_ID"] != DBNull.Value) ||
                      (F2LChecks.Count == 1 && CurrentFlowToLoad["TEST_SUM_ID"] != DBNull.Value && CurrentFlowToLoad["TEST_SUM_ID"].ToString() != F2LChecks[0]["TEST_SUM_ID"].ToString()))
                      Category.CheckCatalogResult = "A";
                  else
                  {
                      DataView QASuppRecs = (DataView)Category.GetCheckParameter("QA_Supplemental_Data_Records").ParameterValue;
                      string OldFilter2 = QASuppRecs.RowFilter;                      
                      QASuppRecs.RowFilter = AddToDataViewFilter(OldFilter2, "TEST_TYPE_CD = 'F2LCHK' AND MON_SYS_ID = '" + MonSysID + "'" + " AND OP_LEVEL_CD = '" + 
                          OpLvlCD + "'" + " AND RPT_PERIOD_ID = " + RptPeriodID + " AND TEST_NUM <> '" + TestNum + "'");
                      if ((QASuppRecs.Count > 0 && CurrentFlowToLoad["TEST_SUM_ID"] == DBNull.Value) ||
                          (QASuppRecs.Count > 1 && CurrentFlowToLoad["TEST_SUM_ID"] != DBNull.Value) ||
                          (QASuppRecs.Count == 1 && CurrentFlowToLoad["TEST_SUM_ID"] != DBNull.Value && CurrentFlowToLoad["TEST_SUM_ID"].ToString() != QASuppRecs[0]["TEST_SUM_ID"].ToString()))
                          Category.CheckCatalogResult = "A";
                      else
                      {
                          QASuppRecs.RowFilter = AddToDataViewFilter(OldFilter2, "TEST_TYPE_CD = 'F2LCHK' AND TEST_NUM = '" + TestNum + "'");
                          if (QASuppRecs.Count > 0)
                          {
                              Category.SetCheckParameter("Flow_to_Load_Check_Supp_Data_ID", cDBConvert.ToString(((DataRowView)QASuppRecs[0])["QA_SUPP_DATA_ID"]), eParameterDataType.String);
                              if (cDBConvert.ToString(((DataRowView)QASuppRecs[0])["CAN_SUBMIT"]) == "N")
                              {                                  
                                  QASuppRecs.RowFilter = AddToDataViewFilter(QASuppRecs.RowFilter, "MON_SYS_ID <> '" + MonSysID + "'" + " OR OP_LEVEL_CD <> '" + 
                                      OpLvlCD + "'" + " OR RPT_PERIOD_ID <> " + RptPeriodID);
                                  if ((QASuppRecs.Count > 0 && CurrentFlowToLoad["TEST_SUM_ID"] == DBNull.Value) ||
                                      (QASuppRecs.Count > 1 && CurrentFlowToLoad["TEST_SUM_ID"] != DBNull.Value) ||
                                      (QASuppRecs.Count == 1 && CurrentFlowToLoad["TEST_SUM_ID"] != DBNull.Value && CurrentFlowToLoad["TEST_SUM_ID"].ToString() != QASuppRecs[0]["TEST_SUM_ID"].ToString()))
                                      Category.CheckCatalogResult = "B";
                                  else
                                      Category.CheckCatalogResult = "C";
                              }
                          }
                      }
                      QASuppRecs.RowFilter = OldFilter2;
                  }
                  F2LChecks.RowFilter = OldFilter;
              }
          }
          catch (Exception ex)
          {
              ReturnVal = Category.CheckEngine.FormatError(ex, "F2LCHK14");
          }

          return ReturnVal;
      }


      public static string F2LCHK15(cCategory Category, ref bool Log)
      //Required Flow to Load Reference Data for Flow to Load Check
      {
          string ReturnVal = "";

          try
          {
              DataRowView CurrentFlowToLoad = (DataRowView)Category.GetCheckParameter("Current_Flow_to_Load_Check").ParameterValue;
              DataView QASuppAttRecs = (DataView)Category.GetCheckParameter("QA_Supplemental_Attribute_Records").ParameterValue;
              string OldFilter4 = QASuppAttRecs.RowFilter;
              string MonLocID = cDBConvert.ToString(CurrentFlowToLoad["MON_LOC_ID"]);
              string MonSysID = cDBConvert.ToString(CurrentFlowToLoad["MON_SYS_ID"]);
              string OpLvlCd = cDBConvert.ToString(CurrentFlowToLoad["OP_LEVEL_CD"]);
              DataView LoadRecs = (DataView)Category.GetCheckParameter("Load_Records").ParameterValue;
              string OldFilter2 = LoadRecs.RowFilter;

              if (Convert.ToBoolean(Category.GetCheckParameter("Flow_to_Load_Check_Operating_Level_Code_Valid").ParameterValue) &&
                  Convert.ToBoolean(Category.GetCheckParameter("Test_Reporting_Period_Valid").ParameterValue) &&
                  ((cDBConvert.ToString(CurrentFlowToLoad["TEST_RESULT_CD"])) == "PASSED" || (cDBConvert.ToString(CurrentFlowToLoad["TEST_RESULT_CD"])) == "FAILED"))
              {
                  Category.SetCheckParameter("Flow_to_Load_Check_Average_Gross_Unit_Load", null, eParameterDataType.Integer);
                  Category.SetCheckParameter("Flow_to_Load_Check_Load_Units_of_Measure", null, eParameterDataType.String);
                  DataView QASuppRecs = (DataView)Category.GetCheckParameter("QA_Supplemental_Data_Records").ParameterValue;
                  string OldFilter1 = QASuppRecs.RowFilter;

                  string TestBasis = cDBConvert.ToString(CurrentFlowToLoad["TEST_BASIS_CD"]);

                  DateTime LastPeriodDay = cDateFunctions.LastDateThisQuarter(cDBConvert.ToInteger(CurrentFlowToLoad["CALENDAR_YEAR"]), cDBConvert.ToInteger(CurrentFlowToLoad["QUARTER"]));
                  if (TestBasis == "Q")
                  {
                      QASuppAttRecs.RowFilter = AddToDataViewFilter(OldFilter4, "TEST_TYPE_CD = 'F2LREF' AND CAN_SUBMIT = 'N'" + " AND MON_SYS_ID = '" +
                          MonSysID + "'" + " AND OP_LEVEL_CD = '" + OpLvlCd + "'" + " AND END_DATE < '" + LastPeriodDay.AddDays(1).ToShortDateString() +
                          "' AND ATTRIBUTE_NAME = 'REF_FLOW_LOAD_RATIO'");
                  }
                  else
                      if (TestBasis == "H")
                      {
                          QASuppAttRecs.RowFilter = AddToDataViewFilter(OldFilter4, "TEST_TYPE_CD = 'F2LREF' AND CAN_SUBMIT = 'N'" + " AND MON_SYS_ID = '" +
                              MonSysID + "'" + " AND OP_LEVEL_CD = '" + OpLvlCd + "'" + " AND END_DATE < '" + LastPeriodDay.AddDays(1).ToShortDateString() +
                              "' AND ATTRIBUTE_NAME = 'REF_GHR'");
                      }
                  QASuppAttRecs.Sort = "END_DATE DESC, END_HOUR DESC, END_MIN DESC, BEGIN_DATE DESC, BEGIN_HOUR DESC, BEGIN_MIN DESC";
                  if (QASuppAttRecs.Count == 0 || (((DataRowView)QASuppAttRecs[0])["ATTRIBUTE_VALUE"] == DBNull.Value))
                  {
                      DataView F2LRefData = (DataView)Category.GetCheckParameter("Flow_to_Load_Reference_Records").ParameterValue;
                      string OldFilter3 = F2LRefData.RowFilter;
                      if (TestBasis == "Q")
                      {
                          F2LRefData.RowFilter = AddToDataViewFilter(OldFilter3, "OP_LEVEL_CD = '" + OpLvlCd + "'" + " AND END_DATE < '" + LastPeriodDay.AddDays(1).ToShortDateString() + "' AND REF_FLOW_LOAD_RATIO IS NOT NULL");

                      }
                      else
                          if (TestBasis == "H")
                          {
                              F2LRefData.RowFilter = AddToDataViewFilter(OldFilter3, "OP_LEVEL_CD = '" + OpLvlCd + "'" + " AND END_DATE < '" + LastPeriodDay.AddDays(1).ToShortDateString() + "' AND REF_GHR IS NOT NULL");
                          }
                      if (F2LRefData.Count == 0)
                          Category.CheckCatalogResult = "A";
                      else
                      {
                          F2LRefData.Sort = "END_DATE DESC, END_HOUR DESC, END_MIN DESC, BEGIN_DATE DESC, BEGIN_HOUR DESC, BEGIN_MIN DESC";
                          decimal AvgAbsPctDiff = cDBConvert.ToDecimal(CurrentFlowToLoad["AVG_ABS_PCT_DIFF"]);
                          if (AvgAbsPctDiff > 10 && AvgAbsPctDiff <= 20)
                          {
                              int AvgGrossUnitLoad = cDBConvert.ToInteger(((DataRowView)F2LRefData[0])["AVG_GROSS_UNIT_LOAD"]);
                              if (AvgGrossUnitLoad > 0)
                                  Category.SetCheckParameter("Flow_to_Load_Check_Average_Gross_Unit_Load", AvgGrossUnitLoad, eParameterDataType.Integer);
                              else
                                  Category.CheckCatalogResult = "C";
                              if (string.IsNullOrEmpty(Category.CheckCatalogResult))
                              {
                                  DateTime EndDateF2LREF = cDBConvert.ToDate(((DataRowView)F2LRefData[0])["END_DATE"], DateTypes.END);
                                  LoadRecs.RowFilter = AddToDataViewFilter(OldFilter2, "MON_LOC_ID = '" + MonLocID + "'" + " AND BEGIN_DATE < '" +
                                      EndDateF2LREF.AddDays(1).ToShortDateString() + "'" + " AND (END_DATE IS NULL OR END_DATE > '" + EndDateF2LREF.AddDays(-1).ToShortDateString() + "')");
                                  if (LoadRecs.Count > 0)
                                  {
                                      string MaxLoadUOMCd = cDBConvert.ToString(((DataRowView)LoadRecs[0])["MAX_LOAD_UOM_CD"]);
                                      if (MaxLoadUOMCd == "MW" || MaxLoadUOMCd == "KLBHR" || MaxLoadUOMCd == "MMBTUHR")
                                          Category.SetCheckParameter("Flow_to_Load_Check_Load_Units_of_Measure", MaxLoadUOMCd, eParameterDataType.String);
                                      else
                                          Category.CheckCatalogResult = "D";
                                  }
                                  else
                                      Category.CheckCatalogResult = "D";
                              }
                          }
                      }
                  }
                  else
                  {
                      string RetrievedQASuppDataID = cDBConvert.ToString(((DataRowView)QASuppAttRecs[0])["QA_SUPP_DATA_ID"]);
                      decimal AvgAbsPctDiff = cDBConvert.ToDecimal(CurrentFlowToLoad["AVG_ABS_PCT_DIFF"]);
                      if (AvgAbsPctDiff > 10 && AvgAbsPctDiff <= 20 && string.IsNullOrEmpty(Category.CheckCatalogResult))
                      {
                          QASuppAttRecs.RowFilter = AddToDataViewFilter(OldFilter4, "QA_SUPP_DATA_ID = '" + RetrievedQASuppDataID + "'" + " AND ATTRIBUTE_NAME = 'AVG_GROSS_UNIT_LOAD'");
                          if (QASuppAttRecs.Count > 0)
                          {
                              int AttributeVal = cDBConvert.ToInteger(((DataRowView)QASuppAttRecs[0])["ATTRIBUTE_VALUE"]);
                              if (AttributeVal > 0)
                                  Category.SetCheckParameter("Flow_to_Load_Check_Average_Gross_Unit_Load", AttributeVal, eParameterDataType.String);
                          }
                          else
                              Category.CheckCatalogResult = "C";
                          if (string.IsNullOrEmpty(Category.CheckCatalogResult))
                          {
                              sFilterPair[] Filter = new sFilterPair[1];
                              Filter[0].Set("QA_SUPP_DATA_ID", RetrievedQASuppDataID);
                              DataRowView QASuppRec;
                              if (FindRow(QASuppRecs, Filter, out QASuppRec))
                              {
                                  DateTime EndDateQASuppRec = cDBConvert.ToDate(QASuppRec["END_DATE"], DateTypes.END);
                                  LoadRecs.RowFilter = AddToDataViewFilter(OldFilter2, "MON_LOC_ID = '" + MonLocID + "'" + " AND BEGIN_DATE < '" +
                                      EndDateQASuppRec.AddDays(1).ToShortDateString() + "'" + " AND (END_DATE IS NULL OR END_DATE > '" + EndDateQASuppRec.AddDays(-1).ToShortDateString() + "')");
                                  if (LoadRecs.Count > 0)
                                  {
                                      string MaxLoadUOMCd = cDBConvert.ToString(((DataRowView)LoadRecs[0])["MAX_LOAD_UOM_CD"]);
                                      if (MaxLoadUOMCd == "MW" || MaxLoadUOMCd == "KLBHR" || MaxLoadUOMCd == "MMBTUHR")
                                          Category.SetCheckParameter("Flow_to_Load_Check_Load_Units_of_Measure", MaxLoadUOMCd, eParameterDataType.String);
                                      else
                                          Category.CheckCatalogResult = "D";
                                  }
                                  else
                                      Category.CheckCatalogResult = "D";
                              }
                          }
                      }
                  }
              }
          }
          catch (Exception ex)
          {
              ReturnVal = Category.CheckEngine.FormatError(ex, "F2LCHK15");
          }

          return ReturnVal;
      }

      public static string F2LCHK16(cCategory Category, ref bool Log)
      //Flow to Load Check Data Average Absolute Percentage Difference Valid
      {
          string ReturnVal = "";

          try
          {
              Category.SetCheckParameter("Flow_to_Load_Check_Calc_Test_Result", null, eParameterDataType.String);
              DataRowView CurrentFlowToLoad = (DataRowView)Category.GetCheckParameter("Current_Flow_to_Load_Check").ParameterValue;
              decimal AvgAbsPctDiff = cDBConvert.ToDecimal(CurrentFlowToLoad["AVG_ABS_PCT_DIFF"]);
              string TestResultCD = cDBConvert.ToString(CurrentFlowToLoad["TEST_RESULT_CD"]);
              if (TestResultCD == "PASSED" || TestResultCD == "FAILED")
              {
                if (AvgAbsPctDiff == decimal.MinValue)
                  Category.CheckCatalogResult = "A";
                else
                  if (AvgAbsPctDiff < 0)
                    Category.CheckCatalogResult = "B";
                  else
                  {
                    int BiasAdjInd = cDBConvert.ToInteger(CurrentFlowToLoad["BIAS_ADJUSTED_IND"]);
                    switch (BiasAdjInd)
                    {
                      case 1:
                        if (AvgAbsPctDiff > 15)
                          Category.SetCheckParameter("Flow_to_Load_Check_Calc_Test_Result", "FAILED", eParameterDataType.String);
                        else
                          if (AvgAbsPctDiff <= 10)
                            Category.SetCheckParameter("Flow_to_Load_Check_Calc_Test_Result", "PASSED", eParameterDataType.String);
                        break;
                      case 0:
                        if (AvgAbsPctDiff > 20)
                          Category.SetCheckParameter("Flow_to_Load_Check_Calc_Test_Result", "FAILED", eParameterDataType.String);
                        else
                          if (AvgAbsPctDiff <= 15)
                            Category.SetCheckParameter("Flow_to_Load_Check_Calc_Test_Result", "PASSED", eParameterDataType.String);
                        break;
                      case int.MinValue:
                        if (AvgAbsPctDiff > 20)
                          Category.SetCheckParameter("Flow_to_Load_Check_Calc_Test_Result", "FAILED", eParameterDataType.String);
                        else
                          if (AvgAbsPctDiff <= 10)
                            Category.SetCheckParameter("Flow_to_Load_Check_Calc_Test_Result", "PASSED", eParameterDataType.String);
                        break;
                    }
                    if (!(BiasAdjInd == int.MinValue && AvgAbsPctDiff <= 10))
                    {
                      if (Category.GetCheckParameter("Flow_to_Load_Check_Average_Gross_Unit_Load").ParameterValue != null &&
                          Category.GetCheckParameter("Flow_to_Load_Check_Load_Units_of_Measure").ParameterValue != null &&
                          BiasAdjInd != int.MinValue)
                      {
                        int AvgGrossUnitLoad = Convert.ToInt32(Category.GetCheckParameter("Flow_to_Load_Check_Average_Gross_Unit_Load").ParameterValue);
                        string UnitsOfMeasure = Convert.ToString(Category.GetCheckParameter("Flow_to_Load_Check_Load_Units_of_Measure").ParameterValue);
                        if ((UnitsOfMeasure == "MW" && AvgGrossUnitLoad < 60) || (UnitsOfMeasure == "KLBHR" && AvgGrossUnitLoad < 500) ||
                            (UnitsOfMeasure == "MMBTUHR" && AvgGrossUnitLoad < 600))
                          if (BiasAdjInd == 1)
                          {
                            if (AvgAbsPctDiff > 15)
                              Category.SetCheckParameter("Flow_to_Load_Check_Calc_Test_Result", "FAILED", eParameterDataType.String);
                            else
                              Category.SetCheckParameter("Flow_to_Load_Check_Calc_Test_Result", "PASSED", eParameterDataType.String);
                          }
                          else
                            if (AvgAbsPctDiff > 20)
                              Category.SetCheckParameter("Flow_to_Load_Check_Calc_Test_Result", "FAILED", eParameterDataType.String);
                            else
                              Category.SetCheckParameter("Flow_to_Load_Check_Calc_Test_Result", "PASSED", eParameterDataType.String);
                        else
                          if (BiasAdjInd == 1)
                          {
                            if (AvgAbsPctDiff > 10)
                              Category.SetCheckParameter("Flow_to_Load_Check_Calc_Test_Result", "FAILED", eParameterDataType.String);
                            else
                              Category.SetCheckParameter("Flow_to_Load_Check_Calc_Test_Result", "PASSED", eParameterDataType.String);
                          }
                          else
                            if (AvgAbsPctDiff > 15)
                              Category.SetCheckParameter("Flow_to_Load_Check_Calc_Test_Result", "FAILED", eParameterDataType.String);
                            else
                              Category.SetCheckParameter("Flow_to_Load_Check_Calc_Test_Result", "PASSED", eParameterDataType.String);
                      }
                    }
                  }
              }
              else
              {
                Category.SetCheckParameter("Flow_to_Load_Check_Calc_Test_Result", TestResultCD, eParameterDataType.String);

                if ((TestResultCD == "FEW168H" || TestResultCD == "EXC168H") && AvgAbsPctDiff != decimal.MinValue)
                  Category.CheckCatalogResult = "C";
              }
          }
          catch (Exception ex)
          {
              ReturnVal = Category.CheckEngine.FormatError(ex, "F2LCHK16");
          }

          return ReturnVal;
      }


      public static string F2LCHK17(cCategory Category, ref bool Log)
      //Flow to Load Check Test Result Code Valid
      {
          string ReturnVal = "";

          try
          {
              DataRowView CurrentFlowToLoad = (DataRowView)Category.GetCheckParameter("Current_Flow_to_Load_Check").ParameterValue;
              string TestResultCD = cDBConvert.ToString(CurrentFlowToLoad["TEST_RESULT_CD"]);
              if (TestResultCD == "")
                  Category.CheckCatalogResult = "A";
              else
                  if (!(TestResultCD == "PASSED" || TestResultCD == "FAILED" || TestResultCD == "EXC168H" || TestResultCD == "FEW168H"))
                  {
                      DataView TestResultLookup = (DataView)Category.GetCheckParameter("Test_Result_Code_Lookup_Table").ParameterValue;
                      string OldFilter = TestResultLookup.RowFilter;
                      TestResultLookup.RowFilter = AddToDataViewFilter(OldFilter, "TEST_RESULT_CD = '" + TestResultCD + "'");
                      if (TestResultLookup.Count == 0)
                          Category.CheckCatalogResult = "B";
                      else
                          Category.CheckCatalogResult = "C";
                      TestResultLookup.RowFilter = OldFilter;
                  }
                  else
                      if (TestResultCD == "EXC168H")
                      {
                          int[] HoursArray = { cDBConvert.ToInteger(CurrentFlowToLoad["NHE_FUEL"]), cDBConvert.ToInteger(CurrentFlowToLoad["NHE_RAMPING"]), 
                              cDBConvert.ToInteger(CurrentFlowToLoad["NHE_BYPASS"]), cDBConvert.ToInteger(CurrentFlowToLoad["NHE_PRE_RATA"]), 
                              cDBConvert.ToInteger(CurrentFlowToLoad["NHE_TEST"]), cDBConvert.ToInteger(CurrentFlowToLoad["NHE_MAIN_BYPASS"]) };
                          bool AllNullOrZero = true;
                          int i = 0;
                          do
                          {
                              AllNullOrZero = (HoursArray[i] == int.MinValue || HoursArray[i] == 0);
                              i++;
                          } while (i < HoursArray.Length && AllNullOrZero);
                          if (AllNullOrZero)
                              Category.CheckCatalogResult = "D";
                      }
              string CheckCalcTestResult = Convert.ToString(Category.GetCheckParameter("Flow_to_Load_Check_Calc_Test_Result").ParameterValue);
              if (string.IsNullOrEmpty(Category.CheckCatalogResult) && CheckCalcTestResult != "")
                  if (TestResultCD == "PASSED" && CheckCalcTestResult == "FAILED")
                      Category.CheckCatalogResult = "E";
                  else
                      if (TestResultCD == "FAILED" && CheckCalcTestResult == "PASSED")
                          Category.CheckCatalogResult = "F";
          }
          catch (Exception ex)
          {
              ReturnVal = Category.CheckEngine.FormatError(ex, "F2LCHK17");
          }

          return ReturnVal;
      }

      public static string F2LCHK18(cCategory Category, ref bool Log)
      //Duplicate Flow to Load Check
      {
          string ReturnVal = "";

          try
          {
              if (Convert.ToBoolean(Category.GetCheckParameter("Test_Number_Valid").ParameterValue))
              {
                  DataRowView CurrentFlowToLoad = (DataRowView)Category.GetCheckParameter("Current_Flow_to_Load_Check").ParameterValue;
                  DataView TestRecs = (DataView)Category.GetCheckParameter("Location_Test_Records").ParameterValue;
                  string OldFilter = TestRecs.RowFilter;
                  string TestNum = cDBConvert.ToString(CurrentFlowToLoad["TEST_NUM"]);
                  TestRecs.RowFilter = AddToDataViewFilter(OldFilter, "TEST_TYPE_CD = 'F2LCHK' AND TEST_NUM = '" + TestNum + "'");
                  if ((TestRecs.Count > 0 && CurrentFlowToLoad["TEST_SUM_ID"] == DBNull.Value) ||
                      (TestRecs.Count > 1 && CurrentFlowToLoad["TEST_SUM_ID"] != DBNull.Value) ||
                      (TestRecs.Count == 1 && CurrentFlowToLoad["TEST_SUM_ID"] != DBNull.Value && CurrentFlowToLoad["TEST_SUM_ID"].ToString() != TestRecs[0]["TEST_SUM_ID"].ToString()))
                  {
                      Category.SetCheckParameter("Duplicate_Flow_To_Load_Check", true, eParameterDataType.Boolean);
                      Category.CheckCatalogResult = "A";
                  }
                  else
                  {
                    string TestSumID = cDBConvert.ToString(CurrentFlowToLoad["TEST_SUM_ID"]);
                    if (TestSumID != "")
                    {
                      DataView QASuppRecords = (DataView)Category.GetCheckParameter("QA_Supplemental_Data_Records").ParameterValue;
                      string OldFilter2 = QASuppRecords.RowFilter;
                      QASuppRecords.RowFilter = AddToDataViewFilter(OldFilter2, "TEST_NUM = '" + TestNum + "' AND TEST_TYPE_CD = 'F2LCHK'");
                      if (QASuppRecords.Count > 0 && cDBConvert.ToString(QASuppRecords[0]["TEST_SUM_ID"]) != TestSumID)
                      {
                        Category.SetCheckParameter("Duplicate_Flow_To_Load_Check", true, eParameterDataType.Boolean);
                        Category.CheckCatalogResult = "B";
                      }
                      else
                        Category.SetCheckParameter("Duplicate_Flow_To_Load_Check", false, eParameterDataType.Boolean);
                      QASuppRecords.RowFilter = OldFilter2;
                    }
                    else
                      Category.SetCheckParameter("Duplicate_Flow_To_Load_Check", false, eParameterDataType.Boolean);
                  }
                  TestRecs.RowFilter = OldFilter;
              }
          }
          catch (Exception ex)
          {
              ReturnVal = Category.CheckEngine.FormatError(ex, "F2LCHK18");
          }

          return ReturnVal;
      }

      public static string F2LCHK19(cCategory Category, ref bool Log)
      //System ID Valid
      {
          string ReturnVal = "";

          try
          {
              DataRowView CurrentFlowToLoad = (DataRowView)Category.GetCheckParameter("Current_Flow_to_Load_Check").ParameterValue;
              if (CurrentFlowToLoad["MON_SYS_ID"] == DBNull.Value)
                  Category.CheckCatalogResult = "A";
          }
          catch (Exception ex)
          {
              ReturnVal = Category.CheckEngine.FormatError(ex, "F2LCHK19");
          }

          return ReturnVal;
      }
     
    #endregion
  }

}
