using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;
using ECMPS.Definitions.Extensions;

namespace ECMPS.Checks.AppendixEChecks
{
  public class cAppendixEChecks : cChecks
  {
    #region Constructors

    public cAppendixEChecks()
    {
      CheckProcedures = new dCheckProcedure[57];

      CheckProcedures[1] = new dCheckProcedure(AppendixE1);
      CheckProcedures[2] = new dCheckProcedure(AppendixE2);
      CheckProcedures[3] = new dCheckProcedure(AppendixE3);
      CheckProcedures[4] = new dCheckProcedure(AppendixE4);
      CheckProcedures[5] = new dCheckProcedure(AppendixE5);
      CheckProcedures[6] = new dCheckProcedure(AppendixE6);
      CheckProcedures[7] = new dCheckProcedure(AppendixE7);
      CheckProcedures[8] = new dCheckProcedure(AppendixE8);
      CheckProcedures[9] = new dCheckProcedure(AppendixE9);
      CheckProcedures[10] = new dCheckProcedure(AppendixE10);
      CheckProcedures[11] = new dCheckProcedure(AppendixE11);
      CheckProcedures[12] = new dCheckProcedure(AppendixE12);
      CheckProcedures[13] = new dCheckProcedure(AppendixE13);
      CheckProcedures[14] = new dCheckProcedure(AppendixE14);
      CheckProcedures[15] = new dCheckProcedure(AppendixE15);
      CheckProcedures[16] = new dCheckProcedure(AppendixE16);
      CheckProcedures[17] = new dCheckProcedure(AppendixE17);
      CheckProcedures[18] = new dCheckProcedure(AppendixE18);
      CheckProcedures[19] = new dCheckProcedure(AppendixE19);
      CheckProcedures[20] = new dCheckProcedure(AppendixE20);
      CheckProcedures[21] = new dCheckProcedure(AppendixE21);
      CheckProcedures[22] = new dCheckProcedure(AppendixE22);
      CheckProcedures[23] = new dCheckProcedure(AppendixE23);
      CheckProcedures[24] = new dCheckProcedure(AppendixE24);
      CheckProcedures[25] = new dCheckProcedure(AppendixE25);
      CheckProcedures[26] = new dCheckProcedure(AppendixE26);
      CheckProcedures[27] = new dCheckProcedure(AppendixE27);
      CheckProcedures[28] = new dCheckProcedure(AppendixE28);
      CheckProcedures[29] = new dCheckProcedure(AppendixE29);
      CheckProcedures[30] = new dCheckProcedure(AppendixE30);
      CheckProcedures[31] = new dCheckProcedure(AppendixE31);
      CheckProcedures[32] = new dCheckProcedure(AppendixE32);
      CheckProcedures[33] = new dCheckProcedure(AppendixE33);
      CheckProcedures[34] = new dCheckProcedure(AppendixE34);
      CheckProcedures[35] = new dCheckProcedure(AppendixE35);
      CheckProcedures[36] = new dCheckProcedure(AppendixE36);
      CheckProcedures[37] = new dCheckProcedure(AppendixE37);
      CheckProcedures[38] = new dCheckProcedure(AppendixE38);
      CheckProcedures[39] = new dCheckProcedure(AppendixE39);
      CheckProcedures[40] = new dCheckProcedure(AppendixE40);
      CheckProcedures[41] = new dCheckProcedure(AppendixE41);
      CheckProcedures[42] = new dCheckProcedure(AppendixE42);
      CheckProcedures[43] = new dCheckProcedure(AppendixE43);
      CheckProcedures[44] = new dCheckProcedure(AppendixE44);
      CheckProcedures[45] = new dCheckProcedure(AppendixE45);
      CheckProcedures[46] = new dCheckProcedure(AppendixE46);
      CheckProcedures[47] = new dCheckProcedure(AppendixE47);
      CheckProcedures[48] = new dCheckProcedure(AppendixE48);
      CheckProcedures[49] = new dCheckProcedure(AppendixE49);
      CheckProcedures[50] = new dCheckProcedure(AppendixE50);
      CheckProcedures[51] = new dCheckProcedure(AppendixE51);
      CheckProcedures[52] = new dCheckProcedure(AppendixE52);
      CheckProcedures[53] = new dCheckProcedure(AppendixE53);
      CheckProcedures[54] = new dCheckProcedure(AppendixE54);
      CheckProcedures[55] = new dCheckProcedure(AppendixE55);
      CheckProcedures[56] = new dCheckProcedure(AppendixE56);
    }

    #endregion

    #region AppendixE Checks

    public static string AppendixE1(cCategory Category, ref bool Log)
    //Initialize Appendix E Test Variables
    {
      string ReturnVal = "";

      try
      {
        Category.SetCheckParameter("Last_APPE_Maximum_HI_Rate", 0m, eParameterDataType.Decimal);
        Category.SetCheckParameter("APPE_Maximum_NOx_Rate", 0m, eParameterDataType.Decimal);
        Category.SetCheckParameter("APPE_Level_Count", 0, eParameterDataType.Integer);
        Category.SetCheckParameter("APPE_Last_Run_Number", 0, eParameterDataType.Integer);
        Category.SetCheckParameter("APPE_Heat_Input_Consistent_with_Operating_Level", true, eParameterDataType.Boolean);
        Category.SetCheckParameter("Calculate_APPE_Segments", true, eParameterDataType.Boolean);
        Category.SetCheckParameter("APPE_Run_Sequence_Valid", true, eParameterDataType.Boolean);
        Category.SetCheckParameter("APPE_Gas_and_Oil_Systems_Consistent", true, eParameterDataType.Boolean);
        Category.SetCheckParameter("APPE_Run_Sequence_Consecutive", false, eParameterDataType.Boolean);
        Category.SetCheckParameter("APPE_Run_Sequence", null, eParameterDataType.String);
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "AppendixE1");
      }

      return ReturnVal;
    }

    public static string AppendixE2(cCategory Category, ref bool Log)
    //System Type Valid
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentAppendixE = (DataRowView)Category.GetCheckParameter("Current_Appendix_E_Test").ParameterValue;
        if (CurrentAppendixE["MON_SYS_ID"] == DBNull.Value)
        {
          Category.SetCheckParameter("APPE_System_Valid", false, eParameterDataType.Boolean);
          Category.CheckCatalogResult = "A";
        }
        else
          if (cDBConvert.ToString(CurrentAppendixE["SYS_TYPE_CD"]) == "NOXE")
          {
            Category.SetCheckParameter("APPE_System_Fuel_Code", cDBConvert.ToString(CurrentAppendixE["FUEL_CD"]), eParameterDataType.String);
            Category.SetCheckParameter("APPE_System_Valid", true, eParameterDataType.Boolean);
          }
          else
          {
            Category.SetCheckParameter("APPE_System_Valid", false, eParameterDataType.Boolean);
            Category.CheckCatalogResult = "B";
          }
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "AppendixE2");
      }

      return ReturnVal;
    }

    public static string AppendixE3(cCategory Category, ref bool Log)
    //Appendix E Test Reason Code Valid
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentAppendixE = (DataRowView)Category.GetCheckParameter("Current_Appendix_E_Test").ParameterValue;
        string TestReasCd = cDBConvert.ToString(CurrentAppendixE["TEST_REASON_CD"]);
        if (TestReasCd == "")
          if (cDBConvert.ToDate(CurrentAppendixE["END_DATE"], DateTypes.END) >= Category.GetCheckParameter("ECMPS_MP_Begin_Date").ValueAsDateTime(DateTypes.START))
            Category.CheckCatalogResult = "A";
          else
            Category.CheckCatalogResult = "B";
        else
          if( !TestReasCd.InList( "INITIAL,QA,RECERT" ) )
          {
            DataView TestReasLookup = (DataView)Category.GetCheckParameter("Test_Reason_Code_Lookup_Table").ParameterValue;
            if (!LookupCodeExists(TestReasCd, TestReasLookup))
              Category.CheckCatalogResult = "C";
            else
              Category.CheckCatalogResult = "D";
          }
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "AppendixE3");
      }

      return ReturnVal;
    }
    public static string AppendixE4(cCategory Category, ref bool Log)
    //Identification of Previously Reported Test or Test Number for Appendix E Test
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentAppendixE = (DataRowView)Category.GetCheckParameter("Current_Appendix_E_Test").ParameterValue;
        string MonSysID = cDBConvert.ToString(CurrentAppendixE["MON_SYS_ID"]);
        if (Convert.ToBoolean(Category.GetCheckParameter("Test_End_Date_Valid").ParameterValue) &&
            Convert.ToBoolean(Category.GetCheckParameter("Test_End_Hour_Valid").ParameterValue) &&
            Convert.ToBoolean(Category.GetCheckParameter("Test_End_Minute_Valid").ParameterValue) && MonSysID != "")
        {
          Category.SetCheckParameter("Extra_APPE_Test", false, eParameterDataType.Boolean);
          DateTime EndDate = cDBConvert.ToDate(CurrentAppendixE["END_DATE"], DateTypes.END);
          int EndHour = cDBConvert.ToHour(CurrentAppendixE["END_HOUR"], DateTypes.END);
          int EndMin = cDBConvert.ToInteger(CurrentAppendixE["END_MIN"]);
          DataView AppendixERecords = (DataView)Category.GetCheckParameter("APPE_Test_Records").ParameterValue;
          string OldFilter1 = AppendixERecords.RowFilter;
          AppendixERecords.RowFilter = AddToDataViewFilter(OldFilter1, "END_DATE = '" + EndDate + "'" + " AND END_HOUR = " + EndHour +
              " AND END_MIN = " + EndMin);
          if ((AppendixERecords.Count > 0 && CurrentAppendixE["TEST_SUM_ID"] == DBNull.Value) ||
              (AppendixERecords.Count > 1 && CurrentAppendixE["TEST_SUM_ID"] != DBNull.Value) ||
              (AppendixERecords.Count == 1 && CurrentAppendixE["TEST_SUM_ID"] != DBNull.Value && CurrentAppendixE["TEST_SUM_ID"].ToString() != AppendixERecords[0]["TEST_SUM_ID"].ToString()))
          {
            Category.SetCheckParameter("Extra_APPE_Test", true, eParameterDataType.Boolean);
            Category.CheckCatalogResult = "A";
          }
          else
          {
            string TestNumber = cDBConvert.ToString(CurrentAppendixE["TEST_NUM"]);
            DataView QASuppRecs = (DataView)Category.GetCheckParameter("QA_Supplemental_Data_Records").ParameterValue;
            string OldFilter2 = QASuppRecs.RowFilter;
            QASuppRecs.RowFilter = AddToDataViewFilter(OldFilter2, "TEST_TYPE_CD = 'APPE'" + " AND MON_SYS_ID = '" + MonSysID +
                "'" + " AND END_DATE = '" + EndDate + "'" + " AND END_HOUR = " + EndHour + " AND END_MIN = " + EndMin + " AND TEST_NUM <> '" + TestNumber + "'");
            if ((QASuppRecs.Count > 0 && CurrentAppendixE["TEST_SUM_ID"] == DBNull.Value) ||
                (QASuppRecs.Count > 1 && CurrentAppendixE["TEST_SUM_ID"] != DBNull.Value) ||
                (QASuppRecs.Count == 1 && CurrentAppendixE["TEST_SUM_ID"] != DBNull.Value && CurrentAppendixE["TEST_SUM_ID"].ToString() != QASuppRecs[0]["TEST_SUM_ID"].ToString()))
            {
              Category.SetCheckParameter("Extra_APPE_Test", true, eParameterDataType.Boolean);
              Category.CheckCatalogResult = "A";
            }
            else
            {
              QASuppRecs.RowFilter = AddToDataViewFilter(OldFilter2, "TEST_TYPE_CD = 'APPE'" + " AND TEST_NUM = '" + TestNumber + "'");
              if (QASuppRecs.Count > 0)
              {
                if (cDBConvert.ToString(((DataRowView)QASuppRecs[0])["CAN_SUBMIT"]) == "N")
                {
                  QASuppRecs.RowFilter = AddToDataViewFilter(QASuppRecs.RowFilter, "MON_SYS_ID <> '" + MonSysID + "'" + " OR END_DATE <> '" + EndDate.ToShortDateString() + "' " +
                     " OR END_HOUR <> " + EndHour + " OR END_MIN <> " + EndMin);
                  if ((QASuppRecs.Count > 0 && CurrentAppendixE["TEST_SUM_ID"] == DBNull.Value) ||
                      (QASuppRecs.Count > 1 && CurrentAppendixE["TEST_SUM_ID"] != DBNull.Value) ||
                      (QASuppRecs.Count == 1 && CurrentAppendixE["TEST_SUM_ID"] != DBNull.Value && CurrentAppendixE["TEST_SUM_ID"].ToString() != QASuppRecs[0]["TEST_SUM_ID"].ToString()))
                    Category.CheckCatalogResult = "B";
                  else
                    Category.CheckCatalogResult = "C";
                }
              }
            }
            QASuppRecs.RowFilter = OldFilter2;
          }
          AppendixERecords.RowFilter = OldFilter1;
        }
        else
          Log = false;
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "AppendixE4");
      }

      return ReturnVal;
    }


    public static string AppendixE5(cCategory Category, ref bool Log)
    //Determine Run Sequence
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentAppendixE = (DataRowView)Category.GetCheckParameter("Current_Appendix_E_Test").ParameterValue;
        DataView RunRecords = (DataView)Category.GetCheckParameter("APPE_Run_Records").ParameterValue;
        decimal[] LevelCountArrayHI = null;
        decimal[] LevelCountArrayNOX = null;
        DateTime BeginDate = DateTime.MinValue;
        int BeginHour = int.MinValue;
        int BeginMin = int.MinValue;
        DateTime EndDate = DateTime.MaxValue, LastDate = DateTime.MinValue; ;
        int EndHour = int.MaxValue, LastHour = int.MinValue;
        int EndMin = int.MaxValue, LastMin = int.MinValue;
        bool Simultaneous = false, RunTimesValid = true;

        if (RunRecords.Count == 0)
          RunTimesValid = false;
        else
        {
          DataTable LevelColumn = RunRecords.ToTable(true, "OP_LEVEL_NUM");
          LevelCountArrayHI = new decimal[LevelColumn.Rows.Count + 1];
          LevelCountArrayNOX = new decimal[LevelColumn.Rows.Count + 1];
          string OldSort = RunRecords.Sort;
          RunRecords.Sort = "BEGIN_DATE, BEGIN_HOUR, BEGIN_MIN";
          bool First = true;

          foreach (DataRowView RunRecord in RunRecords)
          {
            BeginDate = cDBConvert.ToDate(RunRecord["BEGIN_DATE"], DateTypes.START);
            BeginHour = cDBConvert.ToInteger(RunRecord["BEGIN_HOUR"]);
            BeginMin = cDBConvert.ToInteger(RunRecord["BEGIN_MIN"]);

            if (First)
            {
              EndDate = cDBConvert.ToDate(RunRecord["END_DATE"], DateTypes.END);
              EndHour = cDBConvert.ToInteger(RunRecord["END_HOUR"]);
              EndMin = cDBConvert.ToInteger(RunRecord["END_MIN"]);
              if (BeginDate == DateTime.MinValue || BeginHour < 0 || 23 < BeginHour || BeginMin < 0 || 59 < BeginMin)
                RunTimesValid = false;
              else
              {
                Category.SetCheckParameter("APPE_Test_Begin_Date", BeginDate, eParameterDataType.Date);
                Category.SetCheckParameter("APPE_Test_Begin_Hour", BeginHour, eParameterDataType.Integer);
                Category.SetCheckParameter("APPE_Test_Begin_Minute", BeginMin, eParameterDataType.Integer);
              }

              if (EndDate == DateTime.MaxValue || EndHour < 0 || 23 < EndHour || EndMin < 0 || 59 < EndMin)
                RunTimesValid = false;
              else
              {
                LastDate = EndDate;
                LastHour = EndHour;
                LastMin = EndMin;
              }
              First = false;
            }
            else
            {
              if (RunTimesValid && BeginDate != DateTime.MinValue && 0 <= BeginHour && BeginHour <= 23 && 0 <= BeginMin && BeginMin <= 59)
                if (BeginDate < EndDate || (BeginDate == EndDate && (BeginHour < EndHour || (BeginHour == EndHour && BeginMin < EndMin))))
                  Simultaneous = true;
              EndDate = cDBConvert.ToDate(RunRecord["END_DATE"], DateTypes.END);
              EndHour = cDBConvert.ToInteger(RunRecord["END_HOUR"]);
              EndMin = cDBConvert.ToInteger(RunRecord["END_MIN"]);
              if (EndDate == DateTime.MaxValue || EndHour < 0 || 23 < EndHour || EndMin < 0 || 59 < EndMin)
                RunTimesValid = false;
              else
              {
                LastDate = EndDate;
                LastHour = EndHour;
                LastMin = EndMin;
              }
            }
          }
          RunRecords.Sort = OldSort;
        }
        Category.SetCheckParameter("APPE_Run_Times_Valid", RunTimesValid, eParameterDataType.Boolean);
        Category.SetCheckParameter("APPE_Heat_Input_Rate_Array", LevelCountArrayHI, eParameterDataType.Decimal);
        Category.SetCheckParameter("APPE_NOx_Rate_Array", LevelCountArrayNOX, eParameterDataType.Decimal);
        Category.SetCheckParameter("Simultaneous_APPE_Runs", Simultaneous, eParameterDataType.Boolean);
        Category.SetCheckParameter("APPE_Test_End_Date", LastDate, eParameterDataType.Date);
        Category.SetCheckParameter("APPE_Test_End_Hour", LastHour, eParameterDataType.Integer);
        Category.SetCheckParameter("APPE_Test_End_Minute", LastMin, eParameterDataType.Integer);
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "AppendixE5");
      }

      return ReturnVal;
    }

    public static string AppendixE6(cCategory Category, ref bool Log)
    //Fuel System Consistency Check
    {
      string ReturnVal = "";

      try
      {
        Category.SetCheckParameter("APPE_System_List", null, eParameterDataType.String);
        Category.SetCheckParameter("APPE_Systems_with_Inconsistent_UOM", null, eParameterDataType.String);
        DataView OilRecs = (DataView)Category.GetCheckParameter("APPE_Oil_Records").ParameterValue;
        string OldSort = OilRecs.Sort;
        OilRecs.Sort = "MON_SYS_ID";
        string OldFilter1 = OilRecs.RowFilter;
        string MonSysIdList = "";
        string InconsUOMMonSysIdList = null;
        string MonSysId = "";
        DataTable ThisMonSysTbl;
        foreach (DataRowView drv in OilRecs)
        {
          MonSysId = cDBConvert.ToString(drv["MON_SYS_ID"]);
          if (!MonSysId.InList(MonSysIdList))
          {
            MonSysIdList = MonSysIdList.ListAdd(MonSysId);
            OilRecs.RowFilter = AddToDataViewFilter(OldFilter1, "MON_SYS_ID = '" + MonSysId + "'");
            ThisMonSysTbl = OilRecs.ToTable(true, "OIL_VOLUME_UOM_CD");
            if (ThisMonSysTbl.Rows.Count > 1)
              InconsUOMMonSysIdList = InconsUOMMonSysIdList.ListAdd(MonSysId);
            OilRecs.RowFilter = OldFilter1;
          }
        }
        OilRecs.Sort = OldSort;
        DataView GasRecs = (DataView)Category.GetCheckParameter("APPE_Gas_Records").ParameterValue;
        OldSort = GasRecs.Sort;
        GasRecs.Sort = "MON_SYS_ID";
        DataTable GasRecTable = GasRecs.ToTable(true, "MON_SYS_ID");
        string GasMonSysIdList = ColumnToDatalist(GasRecTable.DefaultView, "MON_SYS_ID");
        if (GasMonSysIdList != "")
          MonSysIdList = MonSysIdList.ListAdd(GasMonSysIdList);
        Category.SetCheckParameter("APPE_System_List", MonSysIdList, eParameterDataType.String);
        Category.SetCheckParameter("APPE_Systems_with_Inconsistent_UOM", InconsUOMMonSysIdList, eParameterDataType.String);
        if (InconsUOMMonSysIdList != null)
          Category.CheckCatalogResult = "A";
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "AppendixE6");
      }

      return ReturnVal;
    }

    public static string AppendixE7(cCategory Category, ref bool Log)
    //Simultaneous Runs
    {
      string ReturnVal = "";

      try
      {
        if (Convert.ToBoolean(Category.GetCheckParameter("APPE_Run_Times_Valid").ParameterValue))
        {
          if (Convert.ToBoolean(Category.GetCheckParameter("Simultaneous_APPE_Runs").ParameterValue))
            Category.CheckCatalogResult = "A";
        }
        else
          Log = false;
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "AppendixE7");
      }

      return ReturnVal;
    }

    public static string AppendixE8(cCategory Category, ref bool Log)
    //Out of Sequence or Missing Runs
    {
      string ReturnVal = "";

      try
      {
        if (Convert.ToBoolean(Category.GetCheckParameter("APPE_Run_Times_Valid").ParameterValue))
        {
          if (Convert.ToBoolean(Category.GetCheckParameter("APPE_Run_Sequence_Valid").ParameterValue))
          {
            if (Convert.ToBoolean(Category.GetCheckParameter("APPE_Run_Sequence_Consecutive").ParameterValue))
            {
              string Sequence = Convert.ToString(Category.GetCheckParameter("APPE_Run_Sequence").ParameterValue);
              int Item = 0;
              if (Sequence != "")
                Item = Int16.Parse( Sequence.ListItem(0));
              int LastItem;
              if (Item == 1)
                for (int i = 1; i < Sequence.ListCount(); i++)
                {
                  LastItem = Item;
                  Item = Int16.Parse(Sequence.ListItem(i));
                  if (Item - LastItem != 1)
                  {
                    Category.CheckCatalogResult = "A";
                    break;
                  }
                }
              else
                Category.CheckCatalogResult = "A";
            }
          }
          else
            Category.CheckCatalogResult = "A";
        }
        else
          Log = false;
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "AppendixE8");
      }

      return ReturnVal;
    }

    public static string AppendixE9(cCategory Category, ref bool Log)
    //Concurrent Appendix E Tests
    {
      string ReturnVal = "";

      try
      {
        if (Convert.ToBoolean(Category.GetCheckParameter("Test_Dates_Consistent").ParameterValue))
        {
          DataView APPETestRecs = (DataView)Category.GetCheckParameter("APPE_Test_Records").ParameterValue;
          DataRowView CurrentAppendixE = (DataRowView)Category.GetCheckParameter("Current_Appendix_E_Test").ParameterValue;
          DateTime BeginDate = cDBConvert.ToDate(CurrentAppendixE["BEGIN_DATE"], DateTypes.START);
          int BeginHour = cDBConvert.ToHour(CurrentAppendixE["BEGIN_HOUR"], DateTypes.START);
          int BeginMin = cDBConvert.ToInteger(CurrentAppendixE["BEGIN_MIN"]);
          DateTime EndDate = cDBConvert.ToDate(CurrentAppendixE["END_DATE"], DateTypes.END);
          int EndHour = cDBConvert.ToHour(CurrentAppendixE["END_HOUR"], DateTypes.END);
          int EndMin = cDBConvert.ToInteger(CurrentAppendixE["END_MIN"]);
          string TestNum = cDBConvert.ToString(CurrentAppendixE["TEST_NUM"]);
          string OldFilter = APPETestRecs.RowFilter;
          APPETestRecs.RowFilter = AddToDataViewFilter(OldFilter, "TEST_NUM <> '" + TestNum + "'" + " AND (BEGIN_DATE < '" + EndDate + "'" + " OR (BEGIN_DATE =  '" + EndDate + "'" + " AND (BEGIN_HOUR < " + EndHour + " OR (BEGIN_HOUR = " + EndHour + " AND BEGIN_MIN < " + EndMin + "))))" +
              " AND (END_DATE > '" + BeginDate + "'" + " OR (END_DATE = '" + BeginDate + "'" + " AND (END_HOUR > " + BeginHour + " OR (END_HOUR = " + BeginHour + " AND END_MIN > " + BeginMin + "))))");
          if (APPETestRecs.Count > 0)
            Category.CheckCatalogResult = "A";
          else
          {
            DataView QASuppRecs = (DataView)Category.GetCheckParameter("QA_Supplemental_Data_Records").ParameterValue;
            string MonSysId = cDBConvert.ToString(CurrentAppendixE["MON_SYS_ID"]);
            string OldFilter2 = QASuppRecs.RowFilter;
            QASuppRecs.RowFilter = AddToDataViewFilter(OldFilter2, "test_sum_id <> '" + cDBConvert.ToString(CurrentAppendixE["TEST_SUM_ID"]) + "' and TEST_TYPE_CD = 'APPE' AND MON_SYS_ID = '" + MonSysId + "'" + " AND TEST_NUM <> '" + TestNum + "'" +
                " AND (BEGIN_DATE < '" + EndDate + "'" + " OR (BEGIN_DATE =  '" + EndDate + "'" + " AND (BEGIN_HOUR < " + EndHour + " OR (BEGIN_HOUR = " + EndHour + " AND BEGIN_MIN < " + EndMin + "))))" +
                " AND (END_DATE > '" + BeginDate + "'" + " OR (END_DATE = '" + BeginDate + "'" + " AND (END_HOUR > " + BeginHour + " OR (END_HOUR = " + BeginHour + " AND END_MIN > " + BeginMin + "))))");
            if (QASuppRecs.Count > 0)
              Category.CheckCatalogResult = "A";
            QASuppRecs.RowFilter = OldFilter2;
          }
          APPETestRecs.RowFilter = OldFilter;
        }
        else
          Log = false;
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "AppendixE9");
      }

      return ReturnVal;
    }

    public static string AppendixE10(cCategory Category, ref bool Log)
    //Appendix E Test Begin Time Consistent with Run Times
    {
      string ReturnVal = "";

      try
      {
        if (Convert.ToBoolean(Category.GetCheckParameter("APPE_Run_Times_Valid").ParameterValue) &&
            Convert.ToBoolean(Category.GetCheckParameter("Test_Begin_Date_Valid").ParameterValue) &&
            Convert.ToBoolean(Category.GetCheckParameter("Test_Begin_Hour_Valid").ParameterValue) &&
            Convert.ToBoolean(Category.GetCheckParameter("Test_Begin_Minute_Valid").ParameterValue))
        {
          DataRowView CurrentAppendixE = (DataRowView)Category.GetCheckParameter("Current_Appendix_E_Test").ParameterValue;
          if (cDBConvert.ToDate(CurrentAppendixE["BEGIN_DATE"], DateTypes.START) != Convert.ToDateTime(Category.GetCheckParameter("APPE_Test_Begin_Date").ParameterValue) ||
              cDBConvert.ToHour(CurrentAppendixE["BEGIN_HOUR"], DateTypes.START) != Convert.ToInt32(Category.GetCheckParameter("APPE_Test_Begin_Hour").ParameterValue) ||
              cDBConvert.ToInteger(CurrentAppendixE["BEGIN_MIN"]) != Convert.ToInt32(Category.GetCheckParameter("APPE_Test_Begin_Minute").ParameterValue))
            Category.CheckCatalogResult = "A";

        }
        else
          Log = false;
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "AppendixE10");
      }

      return ReturnVal;
    }

    public static string AppendixE11(cCategory Category, ref bool Log)
    //Appendix E Test End Time Consistent with Run Times
    {
      string ReturnVal = "";

      try
      {
        if (Convert.ToBoolean(Category.GetCheckParameter("APPE_Run_Times_Valid").ParameterValue) &&
            Convert.ToBoolean(Category.GetCheckParameter("Test_End_Date_Valid").ParameterValue) &&
            Convert.ToBoolean(Category.GetCheckParameter("Test_End_Hour_Valid").ParameterValue) &&
            Convert.ToBoolean(Category.GetCheckParameter("Test_End_Minute_Valid").ParameterValue))
        {
          DataRowView CurrentAppendixE = (DataRowView)Category.GetCheckParameter("Current_Appendix_E_Test").ParameterValue;
          if (cDBConvert.ToDate(CurrentAppendixE["END_DATE"], DateTypes.START) != Convert.ToDateTime(Category.GetCheckParameter("APPE_Test_End_Date").ParameterValue) ||
              cDBConvert.ToHour(CurrentAppendixE["END_HOUR"], DateTypes.START) != Convert.ToInt32(Category.GetCheckParameter("APPE_Test_End_Hour").ParameterValue) ||
              cDBConvert.ToInteger(CurrentAppendixE["END_MIN"]) != Convert.ToInt32(Category.GetCheckParameter("APPE_Test_End_Minute").ParameterValue))
            Category.CheckCatalogResult = "A";

        }
        else
          Log = false;
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "AppendixE11");
      }

      return ReturnVal;
    }

    public static string AppendixE12(cCategory Category, ref bool Log)
    //Initialize Variables for Operating Level
    {
      string ReturnVal = "";

      try
      {
        Category.SetCheckParameter("APPE_Level_Maximum_HI_Rate", 0m, eParameterDataType.Decimal);
        Category.SetCheckParameter("APPE_Level_Run_Count", 0, eParameterDataType.Integer);
        Category.SetCheckParameter("APPE_Level_Sum_HI_Rate", 0m, eParameterDataType.Decimal);
        Category.SetCheckParameter("APPE_Level_Sum_Reference_Value", 0m, eParameterDataType.Decimal);
        Category.SetCheckParameter("APPE_Level_Count", 1 + Convert.ToInt16(Category.GetCheckParameter("APPE_Level_Count").ParameterValue), eParameterDataType.Integer);
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "AppendixE12");
      }

      return ReturnVal;
    }

    public static string AppendixE13(cCategory Category, ref bool Log)
    //Operating Level for Run Valid
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentAppendixESummary = (DataRowView)Category.GetCheckParameter("Current_Appendix_E_Summary").ParameterValue;
        int OpLvlNum = cDBConvert.ToInteger(CurrentAppendixESummary["OP_LEVEL_NUM"]);
        if (OpLvlNum == int.MinValue)
          Category.CheckCatalogResult = "A";
        else
          if (OpLvlNum < 1 || 99 < OpLvlNum)
            Category.CheckCatalogResult = "B";
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "AppendixE13");
      }

      return ReturnVal;
    }

    public static string AppendixE14(cCategory Category, ref bool Log)
    //Mean Reference Value for Level Valid
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentAppendixESummary = (DataRowView)Category.GetCheckParameter("Current_Appendix_E_Summary").ParameterValue;
        decimal MeanRefVal = cDBConvert.ToDecimal(CurrentAppendixESummary["MEAN_REF_VALUE"]);
        if (MeanRefVal == decimal.MinValue)
        {
          Category.SetCheckParameter("APPE_Level_Sum_Reference_Value", null, eParameterDataType.Decimal);
          Category.CheckCatalogResult = "A";
        }
        else
          if (MeanRefVal < 0)
          {
            Category.SetCheckParameter("APPE_Level_Sum_Reference_Value", null, eParameterDataType.Decimal);
            Category.CheckCatalogResult = "B";
          }
          else
            if (MeanRefVal > 3)
              Category.CheckCatalogResult = "C";
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "AppendixE14");
      }

      return ReturnVal;
    }

    public static string AppendixE15(cCategory Category, ref bool Log)
    //F-Factor Valid
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentAppendixESummary = (DataRowView)Category.GetCheckParameter("Current_Appendix_E_Summary").ParameterValue;
        decimal FFactor = cDBConvert.ToDecimal(CurrentAppendixESummary["F_FACTOR"]);
        if (FFactor == decimal.MinValue)
          Category.CheckCatalogResult = "A";
        else
          if (FFactor < 1000 || 22000 < FFactor)
            Category.CheckCatalogResult = "B";
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "AppendixE15");
      }

      return ReturnVal;
    }

    public static string AppendixE16(cCategory Category, ref bool Log)
    //Average Hourly Heat Input for Level Valid
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentAppendixESummary = (DataRowView)Category.GetCheckParameter("Current_Appendix_E_Summary").ParameterValue;
        decimal AvgHIRate = cDBConvert.ToDecimal(CurrentAppendixESummary["AVG_HRLY_HI_RATE"]);
        if (AvgHIRate == decimal.MinValue)
          Category.CheckCatalogResult = "A";
        else
          if (AvgHIRate < 0)
            Category.CheckCatalogResult = "B";
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "AppendixE16");
      }

      return ReturnVal;
    }

    public static string AppendixE17(cCategory Category, ref bool Log)
    //Initialize Variables for Run
    {
      string ReturnVal = "";

      try
      {
        Category.SetCheckParameter("APPE_Calc_Run_Total_HI", 0m, eParameterDataType.Decimal);
        Category.SetCheckParameter("APPE_Run_System_Count", 0, eParameterDataType.Integer);
        int LvlRunCt = Convert.ToInt16(Category.GetCheckParameter("APPE_Level_Run_Count").ParameterValue);
        Category.SetCheckParameter("APPE_Level_Run_Count", ++LvlRunCt, eParameterDataType.Integer);
        DataRowView CurrentAppendixERun = (DataRowView)Category.GetCheckParameter("Current_Appendix_E_Run").ParameterValue;
        int RunNum = cDBConvert.ToInteger(CurrentAppendixERun["RUN_NUM"]);
        if (RunNum != int.MinValue)
        {
          int LastRunNum = Convert.ToInt16(Category.GetCheckParameter("APPE_Last_Run_Number").ParameterValue);
          int LvlCt = Convert.ToInt16(Category.GetCheckParameter("APPE_Level_Count").ParameterValue);
          if (LvlRunCt == 1)
          {
            if (LvlCt == 1 && cDBConvert.ToInteger(CurrentAppendixERun["OP_LEVEL_NUM"]) != 1)
            {
              Category.SetCheckParameter("APPE_Run_Sequence_Valid", false, eParameterDataType.Boolean);
            }
            else
              if (RunNum != 1)
                Category.SetCheckParameter("APPE_Run_Sequence_Consecutive", true, eParameterDataType.Boolean);
          }
          else
            if (RunNum - LastRunNum != 1)
              Category.SetCheckParameter("APPE_Run_Sequence_Valid", false, eParameterDataType.Boolean);
          Category.SetCheckParameter("APPE_Last_Run_Number", RunNum, eParameterDataType.Integer);
          string SequenceList = Convert.ToString(Category.GetCheckParameter("APPE_Run_Sequence").ParameterValue);

          SequenceList = SequenceList == "" ? RunNum.ToString() : SequenceList + "," + RunNum;

          int SequenceLength = SequenceList.ListCount();
          int[] SequArray = new int[SequenceLength];
          for (int i = 0; i < SequenceLength; i++)
            SequArray[i] = Int16.Parse(SequenceList.ListItem(i));
          Array.Sort(SequArray);
          SequenceList = "";
          for (int i = 0; i < SequenceLength; i++)
            SequenceList = SequenceList.ListAdd(Convert.ToString(SequArray[i]));

          Category.SetCheckParameter("APPE_Run_Sequence", SequenceList, eParameterDataType.String);
        }
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "AppendixE17");
      }

      return ReturnVal;
    }

    public static string AppendixE18(cCategory Category, ref bool Log)
    //Run Number Valid
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentAppendixERun = (DataRowView)Category.GetCheckParameter("Current_Appendix_E_Run").ParameterValue;
        int RunNum = cDBConvert.ToInteger(CurrentAppendixERun["RUN_NUM"]);
        if (RunNum == int.MinValue)
          Category.CheckCatalogResult = "A";
        else
          if (RunNum <= 0)
            Category.CheckCatalogResult = "B";
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "AppendixE18");
      }

      return ReturnVal;
    }

    public static string AppendixE19(cCategory Category, ref bool Log)
    //Run Begin Time Valid
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentAppendixERun = (DataRowView)Category.GetCheckParameter("Current_Appendix_E_Run").ParameterValue;
        int BeginHour = cDBConvert.ToInteger(CurrentAppendixERun["BEGIN_HOUR"]);
        int BeginMin = cDBConvert.ToInteger(CurrentAppendixERun["BEGIN_MIN"]);
        if (CurrentAppendixERun["BEGIN_DATE"] == DBNull.Value || BeginHour < 0 || 23 < BeginHour || BeginMin < 0 || 59 < BeginMin)
          Category.CheckCatalogResult = "A";
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "AppendixE19");
      }

      return ReturnVal;
    }

    public static string AppendixE20(cCategory Category, ref bool Log)
    //Run End Time Valid
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentAppendixERun = (DataRowView)Category.GetCheckParameter("Current_Appendix_E_Run").ParameterValue;
        Category.SetCheckParameter("APPE_Run_Length", null, eParameterDataType.Decimal);
        DateTime EndDate = cDBConvert.ToDate(CurrentAppendixERun["END_DATE"], DateTypes.END);
        int EndHour = cDBConvert.ToInteger(CurrentAppendixERun["END_HOUR"]);
        int EndMin = cDBConvert.ToInteger(CurrentAppendixERun["END_MIN"]);
        if (EndDate == DateTime.MaxValue || EndHour < 0 || 23 < EndHour || EndMin < 0 || 59 < EndMin)
          Category.CheckCatalogResult = "A";
        else
        {
          DateTime BeginDate = cDBConvert.ToDate(CurrentAppendixERun["BEGIN_DATE"], DateTypes.START);
          int BeginHour = cDBConvert.ToInteger(CurrentAppendixERun["BEGIN_HOUR"]);
          int BeginMin = cDBConvert.ToInteger(CurrentAppendixERun["BEGIN_MIN"]);
          if (!(BeginDate == DateTime.MinValue || BeginHour < 0 || 23 < BeginHour || BeginMin < 0 || 59 < BeginMin))
            if (BeginDate > EndDate || (BeginDate == EndDate && (BeginHour > EndHour || (BeginHour == EndHour && BeginMin > EndMin))))
              Category.CheckCatalogResult = "B";
            else
            {
              TimeSpan ts = EndDate - BeginDate;
              int tempLength = (ts.Days * 24 * 60) + ((EndHour - BeginHour) * 60) + (EndMin - BeginMin);
              Category.SetCheckParameter("APPE_Run_Length", tempLength, eParameterDataType.Decimal);
              if (tempLength < 8)
                Category.CheckCatalogResult = "C";
            }
        }
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "AppendixE20");
      }

      return ReturnVal;
    }

    public static string AppendixE21(cCategory Category, ref bool Log)
    //Response Time Valid
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentAppendixERun = (DataRowView)Category.GetCheckParameter("Current_Appendix_E_Run").ParameterValue;
        int RespTime = cDBConvert.ToInteger(CurrentAppendixERun["RESPONSE_TIME"]);
        if (RespTime != int.MinValue)
          if (RespTime < 0 || 800 < RespTime)
            Category.CheckCatalogResult = "A";
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "AppendixE21");
      }

      return ReturnVal;
    }

    public static string AppendixE22(cCategory Category, ref bool Log)
    //Reference Value for Run Valid
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentAppendixERun = (DataRowView)Category.GetCheckParameter("Current_Appendix_E_Run").ParameterValue;
        decimal RefVal = cDBConvert.ToDecimal(CurrentAppendixERun["REF_VALUE"]);
        if (RefVal == decimal.MinValue)
        {
          Category.SetCheckParameter("APPE_Level_Sum_Reference_Value", -1, eParameterDataType.Decimal);
          Category.CheckCatalogResult = "A";
        }
        else
          if (RefVal < 0)
          {
            Category.SetCheckParameter("APPE_Level_Sum_Reference_Value", -1, eParameterDataType.Decimal);
            Category.CheckCatalogResult = "B";
          }
          else
          {
            decimal LvlSumRefVal = Convert.ToDecimal(Category.GetCheckParameter("APPE_Level_Sum_Reference_Value").ParameterValue);
            if (LvlSumRefVal >= 0)
              Category.SetCheckParameter("APPE_Level_Sum_Reference_Value", LvlSumRefVal + RefVal, eParameterDataType.Decimal);
          }
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "AppendixE22");
      }

      return ReturnVal;
    }

    public static string AppendixE23(cCategory Category, ref bool Log)
    //Hourly Heat Input Rate for Run Valid
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentAppendixERun = (DataRowView)Category.GetCheckParameter("Current_Appendix_E_Run").ParameterValue;
        decimal HIRate = cDBConvert.ToDecimal(CurrentAppendixERun["HOURLY_HI_RATE"]);
        if (HIRate == decimal.MinValue)
          Category.CheckCatalogResult = "A";
        else
          if (HIRate <= 0)
            Category.CheckCatalogResult = "B";
          else
            if (HIRate > Convert.ToDecimal(Category.GetCheckParameter("APPE_Level_Maximum_HI_Rate").ParameterValue))
              Category.SetCheckParameter("APPE_Level_Maximum_HI_Rate", HIRate, eParameterDataType.Decimal);
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "AppendixE23");
      }

      return ReturnVal;
    }

    public static string AppendixE24(cCategory Category, ref bool Log)
    //Total Heat Input for Run Valid
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentAppendixERun = (DataRowView)Category.GetCheckParameter("Current_Appendix_E_Run").ParameterValue;
        decimal TotalHI = cDBConvert.ToDecimal(CurrentAppendixERun["TOTAL_HI"]);
        if (TotalHI == decimal.MinValue)
          Category.CheckCatalogResult = "A";
        else
          if (TotalHI <= 0)
            Category.CheckCatalogResult = "B";
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "AppendixE24");
      }

      return ReturnVal;
    }

    public static string AppendixE25(cCategory Category, ref bool Log)
    //Determine Oil System Type 
    {
      string ReturnVal = "";

      try
      {
        Category.SetCheckParameter("APPE_Oil_System_Type", null, eParameterDataType.String);
        Category.SetCheckParameter("APPE_Run_System_Count", 1 + Convert.ToInt16(Category.GetCheckParameter("APPE_Run_System_Count").ParameterValue), eParameterDataType.Integer);
        DataRowView CurrentAppendixEOil = (DataRowView)Category.GetCheckParameter("Current_Appendix_E_HI_for_Oil").ParameterValue;
        string MonSysId = cDBConvert.ToString(CurrentAppendixEOil["MON_SYS_ID"]);
        if (MonSysId == "")
          Category.CheckCatalogResult = "A";
        else
        {
          DataView SystemRecords = (DataView)Category.GetCheckParameter("Monitor_System_Records").ParameterValue;
          string OldFilter = SystemRecords.RowFilter;
          SystemRecords.RowFilter = AddToDataViewFilter(OldFilter, "MON_SYS_ID = '" + MonSysId + "'");
          Category.SetCheckParameter("APPE_Oil_System_Type", cDBConvert.ToString(SystemRecords[0]["SYS_TYPE_CD"]), eParameterDataType.String);
          SystemRecords.RowFilter = OldFilter;
        }
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "AppendixE25");
      }

      return ReturnVal;
    }

    public static string AppendixE26(cCategory Category, ref bool Log)
    //Volumetric Oil Flow Valid
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentAppendixEOil = (DataRowView)Category.GetCheckParameter("Current_Appendix_E_HI_for_Oil").ParameterValue;
        decimal OilVol = cDBConvert.ToDecimal(CurrentAppendixEOil["OIL_VOLUME"]);
        string OilSysType = Convert.ToString(Category.GetCheckParameter("APPE_Oil_System_Type").ParameterValue);
        if (OilVol == decimal.MinValue && OilSysType == "OILV")
          Category.CheckCatalogResult = "A";
        else
          if (OilSysType == "OILM" && !(OilVol == decimal.MinValue && cDBConvert.ToString(CurrentAppendixEOil["OIL_VOLUME_UOM_CD"]) == ""))
            Category.CheckCatalogResult = "B";
          else
            if (OilVol != decimal.MinValue && OilVol <= 0)
              Category.CheckCatalogResult = "C";
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "AppendixE26");
      }

      return ReturnVal;
    }

    public static string AppendixE27(cCategory Category, ref bool Log)
    //Volumetric Oil Units of Measure Valid
    {
      string ReturnVal = "";

      try
      {
        Category.SetCheckParameter("APPE_Oil_Density_UOM", null, eParameterDataType.String);
        Category.SetCheckParameter("APPE_Oil_GCV_UOM", null, eParameterDataType.String);
        DataRowView CurrentAppendixEOil = (DataRowView)Category.GetCheckParameter("Current_Appendix_E_HI_for_Oil").ParameterValue;
        string OilVolUOMCd = cDBConvert.ToString(CurrentAppendixEOil["OIL_VOLUME_UOM_CD"]);
        if (OilVolUOMCd == "")
        {
          if (Convert.ToString(Category.GetCheckParameter("APPE_Oil_System_Type").ParameterValue) == "OILV")
            Category.CheckCatalogResult = "A";
        }
        else
        {
          DataView UOMTable = (DataView)Category.GetCheckParameter("Oil_Volume_UOM_to_Density_UOM_to_GCV_UOM").ParameterValue;
          string OldFilter = UOMTable.RowFilter;
          UOMTable.RowFilter = AddToDataViewFilter(OldFilter, "OilVolumeUOM = '" + OilVolUOMCd + "'");
          if (UOMTable.Count == 0)
            Category.CheckCatalogResult = "B";
          else
          {
            Category.SetCheckParameter("APPE_Oil_Density_UOM", cDBConvert.ToString(UOMTable[0]["OilDensityUOM"]), eParameterDataType.String);
            Category.SetCheckParameter("APPE_Oil_GCV_UOM", cDBConvert.ToString(UOMTable[0]["OilGCVUOM"]), eParameterDataType.String);
          }
        }
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "AppendixE27");
      }

      return ReturnVal;
    }

    public static string AppendixE28(cCategory Category, ref bool Log)
    //Oil Density Units of Measure Valid
    {
      string ReturnVal = "";

      try
      {
        Category.SetCheckParameter("APPE_Oil_Density_Maximum_Value", null, eParameterDataType.Decimal);
        Category.SetCheckParameter("APPE_Oil_Density_Minimum_Value", null, eParameterDataType.Decimal);
        Category.SetCheckParameter("APPE_Oil_Density_UOM_Valid", false, eParameterDataType.Boolean);
        DataRowView CurrentAppendixEOil = (DataRowView)Category.GetCheckParameter("Current_Appendix_E_HI_for_Oil").ParameterValue;
        string TestUOM = cDBConvert.ToString(CurrentAppendixEOil["OIL_DENSITY_UOM_CD"]);
        if (TestUOM == "")
        {
          if (CurrentAppendixEOil["OIL_DENSITY"] != DBNull.Value && Convert.ToString(Category.GetCheckParameter("APPE_Oil_System_Type").ParameterValue) == "OILV")
            Category.CheckCatalogResult = "A";
        }
        else
        {
          DataView UOMLookup = (DataView)Category.GetCheckParameter("Parameter_Units_Of_Measure_Lookup_Table").ParameterValue;
          string OldFilter = UOMLookup.RowFilter;
          UOMLookup.RowFilter = AddToDataViewFilter(OldFilter, "PARAMETER_CD = 'DENSOIL' AND UOM_CD = '" + TestUOM + "'");
          if (UOMLookup.Count == 0)
            Category.CheckCatalogResult = "B";
          else
          {
            string APPEOilDensityUOM = Convert.ToString(Category.GetCheckParameter("APPE_Oil_Density_UOM").ParameterValue);
            if (APPEOilDensityUOM != "")
              if (TestUOM != APPEOilDensityUOM)
                Category.CheckCatalogResult = "C";
              else
              {
                Category.SetCheckParameter("APPE_Oil_Density_Maximum_Value", cDBConvert.ToDecimal(UOMLookup[0]["MAX_VALUE"]), eParameterDataType.Decimal);
                Category.SetCheckParameter("APPE_Oil_Density_Minimum_Value", cDBConvert.ToDecimal(UOMLookup[0]["MIN_VALUE"]), eParameterDataType.Decimal);
                Category.SetCheckParameter("APPE_Oil_Density_UOM_Valid", true, eParameterDataType.Boolean);
              }
          }
          UOMLookup.RowFilter = OldFilter;
        }
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "AppendixE28");
      }

      return ReturnVal;
    }

    public static string AppendixE29(cCategory Category, ref bool Log)
    //Oil Density Valid
    {
      string ReturnVal = "";

      try
      {
        string APPEOilSysType = Convert.ToString(Category.GetCheckParameter("APPE_Oil_System_Type").ParameterValue);
        DataRowView CurrentAppendixEOil = (DataRowView)Category.GetCheckParameter("Current_Appendix_E_HI_for_Oil").ParameterValue;
        decimal OilDensity = cDBConvert.ToDecimal(CurrentAppendixEOil["OIL_DENSITY"]);
        if (APPEOilSysType == "OILM" && (OilDensity != decimal.MinValue || cDBConvert.ToString(CurrentAppendixEOil["OIL_DENSITY_UOM_CD"]) != ""))
          Category.CheckCatalogResult = "A";
        else
        {
          if (OilDensity == decimal.MinValue)
          {
            if (APPEOilSysType == "OILV" && CurrentAppendixEOil["OIL_MASS"] != DBNull.Value)
              Category.CheckCatalogResult = "B";
          }
          else
          {
            if (OilDensity <= 0)
              Category.CheckCatalogResult = "C";
            if (Category.GetCheckParameter("APPE_Oil_Density_Minimum_Value").ParameterValue != null)
              if (OilDensity < Convert.ToDecimal(Category.GetCheckParameter("APPE_Oil_Density_Minimum_Value").ParameterValue))
                Category.CheckCatalogResult = "D";
            if (string.IsNullOrEmpty(Category.CheckCatalogResult))
              if (Category.GetCheckParameter("APPE_Oil_Density_Maximum_Value").ParameterValue != null)
                if (OilDensity > Convert.ToDecimal(Category.GetCheckParameter("APPE_Oil_Density_Maximum_Value").ParameterValue))
                  Category.CheckCatalogResult = "D";
          }
        }
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "AppendixE29");
      }

      return ReturnVal;
    }

    public static string AppendixE30(cCategory Category, ref bool Log)
    //Mass Oil Flow Valid
    {
      string ReturnVal = "";

      try
      {
        Category.SetCheckParameter("APPE_Calc_Oil_Mass", null, eParameterDataType.Decimal);
        DataRowView CurrentAppendixEOil = (DataRowView)Category.GetCheckParameter("Current_Appendix_E_HI_for_Oil").ParameterValue;
        decimal OilMass = cDBConvert.ToDecimal(CurrentAppendixEOil["OIL_MASS"]);
        string APPEOilSysType = Convert.ToString(Category.GetCheckParameter("APPE_Oil_System_Type").ParameterValue);
        if (OilMass == decimal.MinValue)
        {
          if (APPEOilSysType == "OILM")
            Category.CheckCatalogResult = "A";
        }
        else
          if (OilMass <= 0)
            Category.CheckCatalogResult = "B";
          else
            if (APPEOilSysType == "OILV" && Convert.ToBoolean(Category.GetCheckParameter("APPE_Oil_Density_UOM_Valid").ParameterValue))
            {
              decimal OilVol = cDBConvert.ToDecimal(CurrentAppendixEOil["OIL_VOLUME"]);
              decimal OilDensity = cDBConvert.ToDecimal(CurrentAppendixEOil["OIL_DENSITY"]);
              if (OilVol > 0 && OilDensity > 0)
              {
                decimal CalcOilMass = Math.Round(OilVol * OilDensity, 1, MidpointRounding.AwayFromZero);
                Category.SetCheckParameter("APPE_Calc_Oil_Mass", CalcOilMass, eParameterDataType.Decimal);
                if (OilMass != CalcOilMass)
                {
                  DataView TestToleranceRecords = (DataView)Category.GetCheckParameter("Test_Tolerances_Cross_Check_Table").ParameterValue;
                  string OldFilter = TestToleranceRecords.RowFilter;
                  TestToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, "FieldDescription = 'OilMass'");
                  decimal Tolerance = cDBConvert.ToDecimal(((DataRowView)TestToleranceRecords[0])["Tolerance"]);
                  TestToleranceRecords.RowFilter = OldFilter;
                  if (Math.Abs(OilMass - CalcOilMass) > Tolerance)
                    Category.CheckCatalogResult = "C";
                  else
                    Category.SetCheckParameter("APPE_Calc_Oil_Mass", OilMass, eParameterDataType.Decimal);
                }
              }
            }
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "AppendixE30");
      }

      return ReturnVal;
    }

    public static string AppendixE31(cCategory Category, ref bool Log)
    //Oil GCV Units of Measure Valid
    {
      string ReturnVal = "";

      try
      {
        Category.SetCheckParameter("APPE_Oil_GCV_Maximum_Value", null, eParameterDataType.Decimal);
        Category.SetCheckParameter("APPE_Oil_GCV_Minimum_Value", null, eParameterDataType.Decimal);
        Category.SetCheckParameter("APPE_Oil_GCV_UOM_Valid", false, eParameterDataType.Boolean);
        DataRowView CurrentAppendixEOil = (DataRowView)Category.GetCheckParameter("Current_Appendix_E_HI_for_Oil").ParameterValue;
        string TestUOM = cDBConvert.ToString(CurrentAppendixEOil["OIL_GCV_UOM_CD"]);
        if (TestUOM == "")
          Category.CheckCatalogResult = "A";
        else
        {
          DataView UOMLookup = (DataView)Category.GetCheckParameter("Parameter_Units_Of_Measure_Lookup_Table").ParameterValue;
          string OldFilter = UOMLookup.RowFilter;
          if (CurrentAppendixEOil["OIL_MASS"] != DBNull.Value)
            if (TestUOM != "BTULB")
              Category.CheckCatalogResult = "B";
            else
            {
              UOMLookup.RowFilter = AddToDataViewFilter(OldFilter, "PARAMETER_CD = 'GCV' AND UOM_CD = 'BTULB'");
              Category.SetCheckParameter("APPE_Oil_GCV_Maximum_Value", cDBConvert.ToDecimal(UOMLookup[0]["MAX_VALUE"]), eParameterDataType.Decimal);
              Category.SetCheckParameter("APPE_Oil_GCV_Minimum_Value", cDBConvert.ToDecimal(UOMLookup[0]["MIN_VALUE"]), eParameterDataType.Decimal);
              Category.SetCheckParameter("APPE_Oil_GCV_UOM_Valid", true, eParameterDataType.Boolean);
            }
          else
          {
            UOMLookup.RowFilter = AddToDataViewFilter(OldFilter, "PARAMETER_CD = 'GCV' AND UOM_CD = '" + TestUOM + "'");
            if (UOMLookup.Count == 0)
              Category.CheckCatalogResult = "B";
            else
            {
              string APPEOilGCVUOM = Convert.ToString(Category.GetCheckParameter("APPE_Oil_GCV_UOM").ParameterValue);
              if (APPEOilGCVUOM != "")
                if (TestUOM != APPEOilGCVUOM)
                  Category.CheckCatalogResult = "C";
                else
                {
                  Category.SetCheckParameter("APPE_Oil_GCV_UOM_Valid", true, eParameterDataType.Boolean);
                  Category.SetCheckParameter("APPE_Oil_GCV_Minimum_Value", cDBConvert.ToDecimal(UOMLookup[0]["MIN_VALUE"]), eParameterDataType.Decimal);
                  Category.SetCheckParameter("APPE_Oil_GCV_Maximum_Value", cDBConvert.ToDecimal(UOMLookup[0]["MAX_VALUE"]), eParameterDataType.Decimal);
                }
            }
          }
          UOMLookup.RowFilter = OldFilter;
        }
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "AppendixE31");
      }

      return ReturnVal;
    }

    public static string AppendixE32(cCategory Category, ref bool Log)
    //Oil GCV Valid
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentAppendixEOil = (DataRowView)Category.GetCheckParameter("Current_Appendix_E_HI_for_Oil").ParameterValue;
        decimal OilGCV = cDBConvert.ToDecimal(CurrentAppendixEOil["OIL_GCV"]);
        if (OilGCV == decimal.MinValue)
          Category.CheckCatalogResult = "A";
        else
        {
          if (OilGCV <= 0)
            Category.CheckCatalogResult = "B";
          else
          {
            if (Category.GetCheckParameter("APPE_Oil_GCV_Minimum_Value").ParameterValue != null)
              if (OilGCV < Convert.ToDecimal(Category.GetCheckParameter("APPE_Oil_GCV_Minimum_Value").ParameterValue))
                Category.CheckCatalogResult = "C";
            if (string.IsNullOrEmpty(Category.CheckCatalogResult))
              if (Category.GetCheckParameter("APPE_Oil_GCV_Maximum_Value").ParameterValue != null)
                if (OilGCV > Convert.ToDecimal(Category.GetCheckParameter("APPE_Oil_GCV_Maximum_Value").ParameterValue))
                  Category.CheckCatalogResult = "C";
          }
        }
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "AppendixE32");
      }

      return ReturnVal;
    }

    public static string AppendixE33(cCategory Category, ref bool Log)
    //Heat Input from Oil Valid
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentAppendixEOil = (DataRowView)Category.GetCheckParameter("Current_Appendix_E_HI_for_Oil").ParameterValue;
        Category.SetCheckParameter("APPE_Calc_Oil_Heat_Input", null, eParameterDataType.Decimal);
        decimal OilGCV = cDBConvert.ToDecimal(CurrentAppendixEOil["OIL_GCV"]);
        if (Convert.ToBoolean(Category.GetCheckParameter("APPE_Oil_GCV_UOM_Valid").ParameterValue) && OilGCV > 0)
        {
          decimal CalcOilMass = Convert.ToDecimal(Category.GetCheckParameter("APPE_Calc_Oil_Mass").ParameterValue);
          if (Category.GetCheckParameter("APPE_Calc_Oil_Mass").ParameterValue != null)
            Category.SetCheckParameter("APPE_Calc_Oil_Heat_Input", (decimal)Math.Round(CalcOilMass * OilGCV / 1000000, 1, MidpointRounding.AwayFromZero), eParameterDataType.Decimal);
          else
          {
            decimal OilMass = cDBConvert.ToDecimal(CurrentAppendixEOil["OIL_MASS"]);
            if (OilMass > 0)
              Category.SetCheckParameter("APPE_Calc_Oil_Heat_Input", (decimal)Math.Round(OilMass * OilGCV / 1000000, 1, MidpointRounding.AwayFromZero), eParameterDataType.Decimal);
            else
            {
              decimal OilVol = cDBConvert.ToDecimal(CurrentAppendixEOil["OIL_VOLUME"]);
              if (OilVol > 0)
                Category.SetCheckParameter("APPE_Calc_Oil_Heat_Input", (decimal)Math.Round(OilVol * OilGCV / 1000000, 1, MidpointRounding.AwayFromZero), eParameterDataType.Decimal);
            }
          }
        }
        decimal OilHI = cDBConvert.ToDecimal(CurrentAppendixEOil["OIL_HI"]);
        if (OilHI == decimal.MinValue)
          Category.CheckCatalogResult = "A";
        else
          if (OilHI <= 0)
            Category.CheckCatalogResult = "B";
        if (string.IsNullOrEmpty(Category.CheckCatalogResult))
          if (Category.GetCheckParameter("APPE_Calc_Oil_Heat_Input").ParameterValue == null)
            Category.SetCheckParameter("APPE_Calc_Run_Total_HI", null, eParameterDataType.Decimal);
          else
          {
            decimal CalcOilHI = Convert.ToDecimal(Category.GetCheckParameter("APPE_Calc_Oil_Heat_Input").ParameterValue);
            decimal CalcRunTotalHI = Convert.ToDecimal(Category.GetCheckParameter("APPE_Calc_Run_Total_HI").ParameterValue);
            if (CalcOilHI == OilHI)
            {
              if (Category.GetCheckParameter("APPE_Calc_Run_Total_HI").ParameterValue != null)
                Category.SetCheckParameter("APPE_Calc_Run_Total_HI", CalcRunTotalHI + OilHI, eParameterDataType.Decimal);
            }
            else
            {
              DataView TestToleranceRecords = (DataView)Category.GetCheckParameter("Test_Tolerances_Cross_Check_Table").ParameterValue;
              string OldFilter = TestToleranceRecords.RowFilter;
              TestToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, "FieldDescription = 'HeatInputRate'");
              decimal Tolerance = cDBConvert.ToDecimal(((DataRowView)TestToleranceRecords[0])["Tolerance"]);
              TestToleranceRecords.RowFilter = OldFilter;
              if (Math.Abs(OilHI - CalcOilHI) > Tolerance)
                if (Category.GetCheckParameter("APPE_Calc_Run_Total_HI").ParameterValue != null)
                {
                  Category.SetCheckParameter("APPE_Calc_Run_Total_HI", CalcRunTotalHI + CalcOilHI, eParameterDataType.Decimal);
                  Category.CheckCatalogResult = "C";
                }
                else
                  Category.CheckCatalogResult = "C";
              else
                if (Category.GetCheckParameter("APPE_Calc_Run_Total_HI").ParameterValue != null)
                  Category.SetCheckParameter("APPE_Calc_Run_Total_HI", CalcRunTotalHI + OilHI, eParameterDataType.Decimal);
            }
          }
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "AppendixE33");
      }

      return ReturnVal;
    }

    public static string AppendixE34(cCategory Category, ref bool Log)
    //Gas Volume Valid
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentAppendixEGas = (DataRowView)Category.GetCheckParameter("Current_Appendix_E_HI_for_Gas").ParameterValue;
        Category.SetCheckParameter("APPE_Run_System_Count", 1 + Convert.ToInt16(Category.GetCheckParameter("APPE_Run_System_Count").ParameterValue), eParameterDataType.Integer);
        decimal GasVol = cDBConvert.ToDecimal(CurrentAppendixEGas["GAS_VOLUME"]);
        if (GasVol == decimal.MinValue)
          Category.CheckCatalogResult = "A";
        else
          if (GasVol <= 0)
            Category.CheckCatalogResult = "B";
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "AppendixE34");
      }

      return ReturnVal;
    }

    public static string AppendixE35(cCategory Category, ref bool Log)
    //Gas GCV Valid
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentAppendixEGas = (DataRowView)Category.GetCheckParameter("Current_Appendix_E_HI_for_Gas").ParameterValue;
        decimal GasGCV = cDBConvert.ToDecimal(CurrentAppendixEGas["GAS_GCV"]);
        if (GasGCV == decimal.MinValue)
          Category.CheckCatalogResult = "A";
        else
          if (GasGCV <= 0)
            Category.CheckCatalogResult = "B";
          else
          {
            DataView UOMLookup = (DataView)Category.GetCheckParameter("Parameter_Units_Of_Measure_Lookup_Table").ParameterValue;
            string OldFilter = UOMLookup.RowFilter;
            UOMLookup.RowFilter = AddToDataViewFilter(OldFilter, "PARAMETER_CD = 'GCV' AND UOM_CD = 'BTUHSCF'");
            decimal MinVal = cDBConvert.ToDecimal(UOMLookup[0]["MIN_VALUE"]);
            decimal MaxVal = cDBConvert.ToDecimal(UOMLookup[0]["MAX_VALUE"]);
            UOMLookup.RowFilter = OldFilter;
            if (MinVal == decimal.MinValue)
              Category.SetCheckParameter("APPE_Gas_GCV_Minimum_Value", null, eParameterDataType.Decimal);
            else
              Category.SetCheckParameter("APPE_Gas_GCV_Minimum_Value", MinVal, eParameterDataType.Decimal);
            if (MaxVal == decimal.MinValue)
              Category.SetCheckParameter("APPE_Gas_GCV_Maximum_Value", null, eParameterDataType.Decimal);
            else
              Category.SetCheckParameter("APPE_Gas_GCV_Maximum_Value", MaxVal, eParameterDataType.Decimal);
            if (MinVal != decimal.MinValue)
              if (GasGCV < MinVal)
                Category.CheckCatalogResult = "C";
            if (string.IsNullOrEmpty(Category.CheckCatalogResult) && MaxVal != decimal.MaxValue)
              if (GasGCV > MaxVal)
                Category.CheckCatalogResult = "C";
          }
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "AppendixE35");
      }

      return ReturnVal;
    }

    public static string AppendixE36(cCategory Category, ref bool Log)
    //Heat Input for Gas Valid
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentAppendixEGas = (DataRowView)Category.GetCheckParameter("Current_Appendix_E_HI_for_Gas").ParameterValue;
        Category.SetCheckParameter("APPE_Calc_Gas_Heat_Input", null, eParameterDataType.Decimal);
        decimal GasVolume = cDBConvert.ToDecimal(CurrentAppendixEGas["GAS_VOLUME"]);
        decimal GasGCV = cDBConvert.ToDecimal(CurrentAppendixEGas["GAS_GCV"]);
        if (GasGCV > 0 && GasVolume > 0)
          Category.SetCheckParameter("APPE_Calc_Gas_Heat_Input", (decimal)Math.Round(GasVolume * GasGCV / 1000000, 1, MidpointRounding.AwayFromZero), eParameterDataType.Decimal);
        decimal GasHI = cDBConvert.ToDecimal(CurrentAppendixEGas["GAS_HI"]);
        if (GasHI == decimal.MinValue)
          Category.CheckCatalogResult = "A";
        else
          if (GasHI <= 0)
            Category.CheckCatalogResult = "B";
        if (string.IsNullOrEmpty(Category.CheckCatalogResult))
          if (Category.GetCheckParameter("APPE_Calc_Gas_Heat_Input").ParameterValue == null)
            Category.SetCheckParameter("APPE_Calc_Run_Total_HI", null, eParameterDataType.Decimal);
          else
          {
            decimal CalcGasHI = Convert.ToDecimal(Category.GetCheckParameter("APPE_Calc_Gas_Heat_Input").ParameterValue);
            decimal CalcTotalHI = Convert.ToDecimal(Category.GetCheckParameter("APPE_Calc_Run_Total_HI").ParameterValue);
            if (CalcGasHI == GasHI)
            {
              if (CalcTotalHI != decimal.MinValue)
                Category.SetCheckParameter("APPE_Calc_Run_Total_HI", CalcTotalHI + GasHI, eParameterDataType.Decimal);
            }
            else
            {
              DataView TestToleranceRecords = (DataView)Category.GetCheckParameter("Test_Tolerances_Cross_Check_Table").ParameterValue;
              string OldFilter = TestToleranceRecords.RowFilter;
              TestToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, "FieldDescription = 'HeatInputRate'");
              decimal Tolerance = cDBConvert.ToDecimal(((DataRowView)TestToleranceRecords[0])["Tolerance"]);
              TestToleranceRecords.RowFilter = OldFilter;
              if (Math.Abs(GasHI - CalcGasHI) > Tolerance)
                if (Category.GetCheckParameter("APPE_Calc_Run_Total_HI").ParameterValue != null)
                {
                  Category.SetCheckParameter("APPE_Calc_Run_Total_HI", CalcTotalHI + CalcGasHI, eParameterDataType.Decimal);
                  Category.CheckCatalogResult = "C";
                }
                else
                  Category.CheckCatalogResult = "C";
              else
                if (Category.GetCheckParameter("APPE_Calc_Run_Total_HI").ParameterValue != null)
                  Category.SetCheckParameter("APPE_Calc_Run_Total_HI", CalcTotalHI + GasHI, eParameterDataType.Decimal);
            }
          }
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "AppendixE36");
      }

      return ReturnVal;
    }

    public static string AppendixE37(cCategory Category, ref bool Log)
    //Reported Total HI for Run Consistent with Recalculated Value
    {
      string ReturnVal = "";

      try
      {
          if( Convert.ToString( Category.GetCheckParameter( "APPE_System_List" ).ParameterValue ).ListCount() ==
            Convert.ToInt16(Category.GetCheckParameter("APPE_Run_System_Count").ParameterValue))
        {
          Category.SetCheckParameter("APPE_Use_Calculated_Run_HI", true, eParameterDataType.Boolean);
          decimal CalcRunTotalHI = Convert.ToDecimal(Category.GetCheckParameter("APPE_Calc_Run_Total_HI").ParameterValue);
          DataRowView CurrentAppendixERun = (DataRowView)Category.GetCheckParameter("Current_Appendix_E_Run").ParameterValue;
          decimal TotalHI = cDBConvert.ToDecimal(CurrentAppendixERun["TOTAL_HI"]);
          if (Category.GetCheckParameter("APPE_Calc_Run_Total_HI").ParameterValue != null && TotalHI > 0 && CalcRunTotalHI != TotalHI)
          {
            DataView TestToleranceRecords = (DataView)Category.GetCheckParameter("Test_Tolerances_Cross_Check_Table").ParameterValue;
            string OldFilter = TestToleranceRecords.RowFilter;
            TestToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, "FieldDescription = 'HeatInputRate'");
            decimal Tolerance = cDBConvert.ToDecimal(((DataRowView)TestToleranceRecords[0])["Tolerance"]);
            TestToleranceRecords.RowFilter = OldFilter;
            if (Math.Abs(TotalHI - CalcRunTotalHI) > Tolerance)
              Category.CheckCatalogResult = "A";
            else
              Category.SetCheckParameter("APPE_Use_Calculated_Run_HI", false, eParameterDataType.Boolean);
          }
        }
        else
        {
          Category.SetCheckParameter("APPE_Use_Calculated_Run_HI", false, eParameterDataType.Boolean);
          Category.SetCheckParameter("APPE_Gas_and_Oil_Systems_Consistent", false, eParameterDataType.Boolean);
          Category.SetCheckParameter("APPE_Calc_Run_Total_HI", null, eParameterDataType.Decimal);
        }

      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "AppendixE37");
      }

      return ReturnVal;
    }

    public static string AppendixE38(cCategory Category, ref bool Log)
    //Insufficient Number of Runs
    {
      string ReturnVal = "";

      try
      {
        if (Convert.ToInt16(Category.GetCheckParameter("APPE_Level_Run_Count").ParameterValue) < 3)
          Category.CheckCatalogResult = "A";
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "AppendixE38");
      }

      return ReturnVal;
    }

    public static string AppendixE39(cCategory Category, ref bool Log)
    //Heat Input Rate for Run Consistent with Operating Level
    {
      string ReturnVal = "";

      try
      {
        if (Convert.ToBoolean(Category.GetCheckParameter("APPE_Heat_Input_Consistent_with_Operating_Level").ParameterValue))
        {
          decimal MaxRate = Convert.ToDecimal(Category.GetCheckParameter("APPE_Level_Maximum_HI_Rate").ParameterValue);

          if (Convert.ToDecimal(Category.GetCheckParameter("Last_APPE_Maximum_HI_Rate").ParameterValue) > MaxRate)
          {
            Category.CheckCatalogResult = "A";
            Category.SetCheckParameter("Calculate_APPE_Segments", false, eParameterDataType.Boolean);
            Category.SetCheckParameter("APPE_Heat_Input_Consistent_with_Operating_Level", false, eParameterDataType.Boolean);
          }
          else
            Category.SetCheckParameter("Last_APPE_Maximum_HI_Rate", MaxRate, eParameterDataType.Decimal);
        }
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "AppendixE39");
      }

      return ReturnVal;
    }

    public static string AppendixE40(cCategory Category, ref bool Log)
    //Reported Arithmetic Mean of Reference Values for Level Consistent with Recalculated Value
    {
      string ReturnVal = "";

      try
      {
        int RunCt = Convert.ToInt16(Category.GetCheckParameter("APPE_Level_Run_Count").ParameterValue);
        if (Convert.ToDecimal(Category.GetCheckParameter("APPE_Level_Sum_Reference_Value").ParameterValue) >= 0 &&
             RunCt >= 3)
        {
          decimal CalcLevelMeanRefVal = Math.Round(Convert.ToDecimal(Category.GetCheckParameter("APPE_Level_Sum_Reference_Value").ParameterValue) / RunCt, 3, MidpointRounding.AwayFromZero);
          Category.SetCheckParameter("APPE_Calc_Level_Mean_Reference_Value", CalcLevelMeanRefVal, eParameterDataType.Decimal);
          DataRowView CurrentAppendixESummary = (DataRowView)Category.GetCheckParameter("Current_Appendix_E_Summary").ParameterValue;
          decimal MeanRefVal = cDBConvert.ToDecimal(CurrentAppendixESummary["MEAN_REF_VALUE"]);

          decimal[] TempArray = (decimal[])Category.GetCheckParameter("APPE_NOx_Rate_Array").ParameterValue;
          int LvlCt = Convert.ToInt16(Category.GetCheckParameter("APPE_Level_Count").ParameterValue);

          if (MeanRefVal > 0 && MeanRefVal != CalcLevelMeanRefVal)
          {
            DataView TestToleranceRecords = (DataView)Category.GetCheckParameter("Test_Tolerances_Cross_Check_Table").ParameterValue;
            string OldFilter = TestToleranceRecords.RowFilter;
            TestToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, "FieldDescription = 'MeanReferenceValue'");
            decimal Tolerance = cDBConvert.ToDecimal(((DataRowView)TestToleranceRecords[0])["Tolerance"]);
            TestToleranceRecords.RowFilter = OldFilter;
            if (Math.Abs(MeanRefVal - CalcLevelMeanRefVal) > Tolerance)
            {
              TempArray[LvlCt] = CalcLevelMeanRefVal;
              Category.CheckCatalogResult = "A";
            }
            else
              TempArray[LvlCt] = MeanRefVal;
          }
          else
            TempArray[LvlCt] = CalcLevelMeanRefVal;
          if (Convert.ToDecimal(Category.GetCheckParameter("APPE_Maximum_NOx_Rate").ParameterValue) < TempArray[LvlCt])
            Category.SetCheckParameter("APPE_Maximum_NOx_Rate", TempArray[LvlCt], eParameterDataType.Decimal);
          Category.SetCheckParameter("APPE_NOx_Rate_Array", TempArray, eParameterDataType.Decimal);//Run this even after setting Result A
        }
        else
        {
          Category.SetCheckParameter("Calculate_APPE_Segments", false, eParameterDataType.Boolean);
          Category.SetCheckParameter("APPE_Calc_Level_Mean_Reference_Value", null, eParameterDataType.Decimal);
        }
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "AppendixE40");
      }

      return ReturnVal;
    }

    public static string AppendixE41(cCategory Category, ref bool Log)
    //Reported Average Heat Input Rate for Level Consistent with Recalculated Value
    {
      string ReturnVal = "";

      try
      {
        int RunCt = Convert.ToInt16(Category.GetCheckParameter("APPE_Level_Run_Count").ParameterValue);
        if (Category.GetCheckParameter("APPE_Level_Sum_HI_Rate").ParameterValue != null &&
             RunCt >= 3)
        {
          decimal CalcAvgHIRate = Math.Round(Convert.ToDecimal(Category.GetCheckParameter("APPE_Level_Sum_HI_Rate").ParameterValue) / RunCt, 1, MidpointRounding.AwayFromZero);
          Category.SetCheckParameter("APPE_Calc_Level_Average_HI_Rate", CalcAvgHIRate, eParameterDataType.Decimal);
          DataRowView CurrentAppendixESummary = (DataRowView)Category.GetCheckParameter("Current_Appendix_E_Summary").ParameterValue;
          decimal AvgHrlyHIRate = cDBConvert.ToDecimal(CurrentAppendixESummary["AVG_HRLY_HI_RATE"]);

          decimal[] TempArray = (decimal[])Category.GetCheckParameter("APPE_Heat_Input_Rate_Array").ParameterValue;
          int LvlCt = Convert.ToInt16(Category.GetCheckParameter("APPE_Level_Count").ParameterValue);

          if (AvgHrlyHIRate > 0 && AvgHrlyHIRate != CalcAvgHIRate)
          {
            DataView TestToleranceRecords = (DataView)Category.GetCheckParameter("Test_Tolerances_Cross_Check_Table").ParameterValue;
            string OldFilter = TestToleranceRecords.RowFilter;
            TestToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, "FieldDescription = 'HeatInputRate'");
            decimal Tolerance = cDBConvert.ToDecimal(((DataRowView)TestToleranceRecords[0])["Tolerance"]);
            TestToleranceRecords.RowFilter = OldFilter;
            if (Math.Abs(AvgHrlyHIRate - CalcAvgHIRate) > Tolerance)
            {
              TempArray[LvlCt] = CalcAvgHIRate;
              Category.CheckCatalogResult = "A";
            }
            else
              TempArray[LvlCt] = AvgHrlyHIRate;
          }
          else
            TempArray[LvlCt] = CalcAvgHIRate;
          Category.SetCheckParameter("APPE_Heat_Input_Rate_Array", TempArray, eParameterDataType.Decimal);//Run this even after setting Result A
        }
        else
        {
          Category.SetCheckParameter("Calculate_APPE_Segments", false, eParameterDataType.Boolean);
          Category.SetCheckParameter("APPE_Calc_Level_Average_HI_Rate", null, eParameterDataType.Decimal);
        }
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "AppendixE41");
      }

      return ReturnVal;
    }

    public static string AppendixE42(cCategory Category, ref bool Log)
    //Insufficient Number of Operating Levels
    {
      string ReturnVal = "";

      try
      {
        Category.SetCheckParameter("APPE_Level_Count_Validated", true, eParameterDataType.Boolean);
        if (Convert.ToInt16(Category.GetCheckParameter("APPE_Level_Count").ParameterValue) < 4)
        {
          Category.SetCheckParameter("Calculate_APPE_Segments", false, eParameterDataType.Boolean);
          Category.CheckCatalogResult = "A";
        }
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "AppendixE42");
      }

      return ReturnVal;
    }

    public static string AppendixE43(cCategory Category, ref bool Log)
    //Appendix E Oil and Gas Records Consistent
    {
      string ReturnVal = "";

      try
      {
        if (!Convert.ToBoolean(Category.GetCheckParameter("Calculate_APPE_Segments").ParameterValue))
        {
          Category.SetCheckParameter("APPE_NOx_Rate_Array", null, eParameterDataType.Decimal);
          Category.SetCheckParameter("APPE_Heat_Input_Rate_Array", null, eParameterDataType.Decimal);
        }
        if (!Convert.ToBoolean(Category.GetCheckParameter("APPE_Gas_and_Oil_Systems_Consistent").ParameterValue))
        {
          Category.SetCheckParameter("APPE_NOx_Rate_Array", null, eParameterDataType.Decimal);
          Category.SetCheckParameter("APPE_Heat_Input_Rate_Array", null, eParameterDataType.Decimal);
          Category.CheckCatalogResult = "A";
        }
        else
        {
          string FuelCd = Convert.ToString(Category.GetCheckParameter("APPE_System_Fuel_Code").ParameterValue);
          if (FuelCd == "")
          {
            Category.SetCheckParameter("APPE_NOx_Rate_Array", null, eParameterDataType.Decimal);
            Category.SetCheckParameter("APPE_Heat_Input_Rate_Array", null, eParameterDataType.Decimal);
            Category.CheckCatalogResult = "B";
          }
          else
          {
            string APPESysList = Convert.ToString(Category.GetCheckParameter("APPE_System_List").ParameterValue);
            if( APPESysList.ListCount() == 1 )
              if (FuelCd == "MIX")
              {
                Category.SetCheckParameter("APPE_NOx_Rate_Array", null, eParameterDataType.Decimal);
                Category.SetCheckParameter("APPE_Heat_Input_Rate_Array", null, eParameterDataType.Decimal);
                Category.CheckCatalogResult = "C";
              }
              else
              {
                DataView SystemRecords = (DataView)Category.GetCheckParameter("Monitor_System_Records").ParameterValue;
                string OldFilter = SystemRecords.RowFilter;
                SystemRecords.RowFilter = AddToDataViewFilter(OldFilter, "MON_SYS_ID = '" + APPESysList + "'");
                if (cDBConvert.ToString(SystemRecords[0]["FUEL_CD"]) != FuelCd)
                {
                  Category.SetCheckParameter("APPE_NOx_Rate_Array", null, eParameterDataType.Decimal);
                  Category.SetCheckParameter("APPE_Heat_Input_Rate_Array", null, eParameterDataType.Decimal);
                  Category.CheckCatalogResult = "D";
                }
                SystemRecords.RowFilter = OldFilter;
              }
            else
              if (FuelCd != "MIX")
              {
                Category.SetCheckParameter("APPE_NOx_Rate_Array", null, eParameterDataType.Decimal);
                Category.SetCheckParameter("APPE_Heat_Input_Rate_Array", null, eParameterDataType.Decimal);
                Category.CheckCatalogResult = "E";
              }
          }
        }
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "AppendixE43");
      }

      return ReturnVal;
    }

    public static string AppendixE44(cCategory Category, ref bool Log)
    //Hourly Heat Input Rate Consistent with Recalculated Value
    {
      string ReturnVal = "";

      try
      {
        if (Category.GetCheckParameter("APPE_Calc_Run_Total_HI").ParameterValue != null && Category.GetCheckParameter("APPE_Run_Length").ParameterValue != null)
        {
          decimal CalcRunHIRate;
          DataRowView CurrentAppendixERun = (DataRowView)Category.GetCheckParameter("Current_Appendix_E_Run").ParameterValue;
          decimal CalcRunTotalHI = Convert.ToDecimal(Category.GetCheckParameter("APPE_Calc_Run_Total_HI").ParameterValue);
          decimal APPERunLength = Convert.ToDecimal(Category.GetCheckParameter("APPE_Run_Length").ParameterValue);
          if (Convert.ToBoolean(Category.GetCheckParameter("APPE_Use_Calculated_Run_HI").ParameterValue))
            CalcRunHIRate = Math.Round(60 * CalcRunTotalHI / APPERunLength, 1, MidpointRounding.AwayFromZero);
          else
            CalcRunHIRate = Math.Round(60 * cDBConvert.ToDecimal(CurrentAppendixERun["TOTAL_HI"]) / APPERunLength, 1, MidpointRounding.AwayFromZero);
          Category.SetCheckParameter("APPE_Calc_Run_HI_Rate", CalcRunHIRate, eParameterDataType.Decimal);
          decimal HrlyHIRate = cDBConvert.ToDecimal(CurrentAppendixERun["HOURLY_HI_RATE"]);
          bool LvlSumHIRateIsNull = Category.GetCheckParameter("APPE_Level_Sum_HI_Rate").ParameterValue == null;
          decimal LvlSumHIRate = Convert.ToDecimal(Category.GetCheckParameter("APPE_Level_Sum_HI_Rate").ParameterValue);
          if (HrlyHIRate > 0 && HrlyHIRate != CalcRunHIRate)
          {
            decimal MinRate = Math.Round(60 * CalcRunTotalHI / (APPERunLength + 1), 1, MidpointRounding.AwayFromZero);
            decimal MaxRate;
            if (APPERunLength == 1)
              MaxRate = (decimal)99999.9;
            else
              MaxRate = Math.Round(60 * CalcRunTotalHI / (APPERunLength - 1), 1, MidpointRounding.AwayFromZero);
            DataView TestToleranceRecords = (DataView)Category.GetCheckParameter("Test_Tolerances_Cross_Check_Table").ParameterValue;
            string OldFilter = TestToleranceRecords.RowFilter;
            TestToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, "FieldDescription = 'HeatInputRate'");
            decimal Tolerance = cDBConvert.ToDecimal(((DataRowView)TestToleranceRecords[0])["Tolerance"]);
            TestToleranceRecords.RowFilter = OldFilter;
            MinRate -= Tolerance;
            MaxRate += Tolerance;
            if (HrlyHIRate < MinRate || HrlyHIRate > MaxRate)
              if (!LvlSumHIRateIsNull)
              {
                Category.SetCheckParameter("APPE_Level_Sum_HI_Rate", CalcRunHIRate + LvlSumHIRate, eParameterDataType.Decimal);
                Category.CheckCatalogResult = "A";
              }
              else
                Category.CheckCatalogResult = "A";
            else
              if (!LvlSumHIRateIsNull)
                Category.SetCheckParameter("APPE_Level_Sum_HI_Rate", HrlyHIRate + LvlSumHIRate, eParameterDataType.Decimal);
          }
          else
            if (!LvlSumHIRateIsNull)
              Category.SetCheckParameter("APPE_Level_Sum_HI_Rate", CalcRunHIRate + LvlSumHIRate, eParameterDataType.Decimal);
        }
        else
        {
          Category.SetCheckParameter("APPE_Level_Sum_HI_Rate", null, eParameterDataType.Decimal);
          Category.SetCheckParameter("APPE_Calc_Run_HI_Rate", null, eParameterDataType.Decimal);
        }
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "AppendixE44");
      }

      return ReturnVal;
    }

    public static string AppendixE45(cCategory Category, ref bool Log)
    //Gas Monitoring System ID Valid
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentAppendixEGas = (DataRowView)Category.GetCheckParameter("Current_Appendix_E_HI_for_Gas").ParameterValue;
        if (CurrentAppendixEGas["MON_SYS_ID"] == DBNull.Value)
          Category.CheckCatalogResult = "A";
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "AppendixE45");
      }

      return ReturnVal;
    }

    public static string AppendixE46(cCategory Category, ref bool Log)
    //Duplicate Appendix E Test
    {
      string ReturnVal = "";

      try
      {
        if (Convert.ToBoolean(Category.GetCheckParameter("Test_Number_Valid").ParameterValue))
        {
          DataRowView CurrentAppendixE = (DataRowView)Category.GetCheckParameter("Current_Appendix_E_Test").ParameterValue;
          DataView TestRecs = (DataView)Category.GetCheckParameter("Location_Test_Records").ParameterValue;
          string OldFilter = TestRecs.RowFilter;
          TestRecs.RowFilter = AddToDataViewFilter(OldFilter, "TEST_TYPE_CD = 'APPE' AND TEST_NUM = '" + cDBConvert.ToString(CurrentAppendixE["TEST_NUM"]) + "'");
          if ((TestRecs.Count > 0 && CurrentAppendixE["TEST_SUM_ID"] == DBNull.Value) ||
              (TestRecs.Count > 1 && CurrentAppendixE["TEST_SUM_ID"] != DBNull.Value) ||
              (TestRecs.Count == 1 && CurrentAppendixE["TEST_SUM_ID"] != DBNull.Value && CurrentAppendixE["TEST_SUM_ID"].ToString() != TestRecs[0]["TEST_SUM_ID"].ToString()))
          {
            Category.SetCheckParameter("Duplicate_Appendix_E_Test", true, eParameterDataType.Boolean);
            Category.CheckCatalogResult = "A";
          }
          else
          {
            string TestSumID = cDBConvert.ToString(CurrentAppendixE["TEST_SUM_ID"]);
            if (TestSumID != "")
            {
              DataView QASuppRecords = (DataView)Category.GetCheckParameter("QA_Supplemental_Data_Records").ParameterValue;
              string OldFilter2 = QASuppRecords.RowFilter;
              QASuppRecords.RowFilter = AddToDataViewFilter(OldFilter2, "TEST_NUM = '" + cDBConvert.ToString(CurrentAppendixE["TEST_NUM"]) + "' AND TEST_TYPE_CD = 'APPE'");
              if (QASuppRecords.Count > 0 && cDBConvert.ToString(QASuppRecords[0]["TEST_SUM_ID"]) != TestSumID)
              {
                Category.SetCheckParameter("Duplicate_Appendix_E_Test", true, eParameterDataType.Boolean);
                Category.CheckCatalogResult = "B";
              }
              else
                Category.SetCheckParameter("Duplicate_Appendix_E_Test", false, eParameterDataType.Boolean);
              QASuppRecords.RowFilter = OldFilter2;
            }
            else
              Category.SetCheckParameter("Duplicate_Appendix_E_Test", false, eParameterDataType.Boolean);
          }
          TestRecs.RowFilter = OldFilter;
        }
        else
          Log = false;
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "AppendixE46");
      }

      return ReturnVal;
    }

    public static string AppendixE47(cCategory Category, ref bool Log)
    //Appendix E Monitoring System ID Valid
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentAppendixE = (DataRowView)Category.GetCheckParameter("Current_Appendix_E_Test").ParameterValue;
        if (CurrentAppendixE["MON_SYS_ID"] == DBNull.Value)
          Category.CheckCatalogResult = "A";
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "AppendixE47");
      }

      return ReturnVal;
    }

    public static string AppendixE48(cCategory Category, ref bool Log)
    //Duplicate Appendix E Summary
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentAppendixESummary = (DataRowView)Category.GetCheckParameter("Current_Appendix_E_Summary").ParameterValue;
        int OpLvlNum = cDBConvert.ToInteger(CurrentAppendixESummary["OP_LEVEL_NUM"]);
        if (OpLvlNum != int.MinValue)
        {
          DataView TestRecs = (DataView)Category.GetCheckParameter("APPE_Summary_Records").ParameterValue;
          string OldFilter = TestRecs.RowFilter;
          TestRecs.RowFilter = AddToDataViewFilter(OldFilter, "OP_LEVEL_NUM = " + OpLvlNum);
          if ((TestRecs.Count > 0 && CurrentAppendixESummary["AE_CORR_TEST_SUM_ID"] == DBNull.Value) ||
              (TestRecs.Count > 1 && CurrentAppendixESummary["AE_CORR_TEST_SUM_ID"] != DBNull.Value) ||
              (TestRecs.Count == 1 && CurrentAppendixESummary["AE_CORR_TEST_SUM_ID"] != DBNull.Value && CurrentAppendixESummary["AE_CORR_TEST_SUM_ID"].ToString() != TestRecs[0]["AE_CORR_TEST_SUM_ID"].ToString()))
            Category.CheckCatalogResult = "A";
          TestRecs.RowFilter = OldFilter;
        }
        else
          Log = false;
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "AppendixE48");
      }

      return ReturnVal;
    }

    public static string AppendixE49(cCategory Category, ref bool Log)
    //Duplicate Appendix E Run
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentAppendixERun = (DataRowView)Category.GetCheckParameter("Current_Appendix_E_Run").ParameterValue;
        int OpLvlNum = cDBConvert.ToInteger(CurrentAppendixERun["OP_LEVEL_NUM"]);
        int RunNum = cDBConvert.ToInteger(CurrentAppendixERun["RUN_NUM"]);
        if (OpLvlNum != int.MinValue && RunNum != int.MinValue)
        {
          DataView TestRecs = (DataView)Category.GetCheckParameter("APPE_Run_Records").ParameterValue;
          string OldFilter = TestRecs.RowFilter;
          TestRecs.RowFilter = AddToDataViewFilter(OldFilter, "OP_LEVEL_NUM = " + OpLvlNum + " AND RUN_NUM = " + RunNum);
          if ((TestRecs.Count > 0 && CurrentAppendixERun["AE_CORR_TEST_RUN_ID"] == DBNull.Value) ||
              (TestRecs.Count > 1 && CurrentAppendixERun["AE_CORR_TEST_RUN_ID"] != DBNull.Value) ||
              (TestRecs.Count == 1 && CurrentAppendixERun["AE_CORR_TEST_RUN_ID"] != DBNull.Value && CurrentAppendixERun["AE_CORR_TEST_RUN_ID"].ToString() != TestRecs[0]["AE_CORR_TEST_RUN_ID"].ToString()))
            Category.CheckCatalogResult = "A";
          TestRecs.RowFilter = OldFilter;
        }
        else
          Log = false;
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "AppendixE49");
      }

      return ReturnVal;
    }

    public static string AppendixE50(cCategory Category, ref bool Log)
    //Duplicate Appendix E Oil Record
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentAppendixEOil = (DataRowView)Category.GetCheckParameter("Current_Appendix_E_HI_for_Oil").ParameterValue;
        int OpLvlNum = cDBConvert.ToInteger(CurrentAppendixEOil["OP_LEVEL_NUM"]);
        int RunNum = cDBConvert.ToInteger(CurrentAppendixEOil["RUN_NUM"]);
        string MonSysId = cDBConvert.ToString(CurrentAppendixEOil["MON_SYS_ID"]);
        if (OpLvlNum != int.MinValue && RunNum != int.MinValue && MonSysId != "")
        {
          DataView TestRecs = (DataView)Category.GetCheckParameter("APPE_Oil_Records").ParameterValue;
          string OldFilter = TestRecs.RowFilter;
          TestRecs.RowFilter = AddToDataViewFilter(OldFilter, "OP_LEVEL_NUM = " + OpLvlNum + " AND RUN_NUM = " + RunNum +
              " AND MON_SYS_ID = '" + MonSysId + "'");
          if ((TestRecs.Count > 0 && CurrentAppendixEOil["AE_HI_OIL_ID"] == DBNull.Value) ||
              (TestRecs.Count > 1 && CurrentAppendixEOil["AE_HI_OIL_ID"] != DBNull.Value) ||
              (TestRecs.Count == 1 && CurrentAppendixEOil["AE_HI_OIL_ID"] != DBNull.Value && CurrentAppendixEOil["AE_HI_OIL_ID"].ToString() != TestRecs[0]["AE_HI_OIL_ID"].ToString()))
            Category.CheckCatalogResult = "A";
          TestRecs.RowFilter = OldFilter;
        }
        else
          Log = false;
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "AppendixE50");
      }

      return ReturnVal;
    }
    public static string AppendixE51(cCategory Category, ref bool Log)
    //Duplicate Appendix E Gas Record
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentAppendixEGas = (DataRowView)Category.GetCheckParameter("Current_Appendix_E_HI_for_Gas").ParameterValue;
        int OpLvlNum = cDBConvert.ToInteger(CurrentAppendixEGas["OP_LEVEL_NUM"]);
        int RunNum = cDBConvert.ToInteger(CurrentAppendixEGas["RUN_NUM"]);
        string MonSysId = cDBConvert.ToString(CurrentAppendixEGas["MON_SYS_ID"]);
        if (OpLvlNum != int.MinValue && RunNum != int.MinValue && MonSysId != "")
        {
          DataView TestRecs = (DataView)Category.GetCheckParameter("APPE_Gas_Records").ParameterValue;
          string OldFilter = TestRecs.RowFilter;
          TestRecs.RowFilter = AddToDataViewFilter(OldFilter, "OP_LEVEL_NUM = " + OpLvlNum + " AND RUN_NUM = " + RunNum +
              " AND MON_SYS_ID = '" + MonSysId + "'");
          if ((TestRecs.Count > 0 && CurrentAppendixEGas["AE_HI_GAS_ID"] == DBNull.Value) ||
              (TestRecs.Count > 1 && CurrentAppendixEGas["AE_HI_GAS_ID"] != DBNull.Value) ||
              (TestRecs.Count == 1 && CurrentAppendixEGas["AE_HI_GAS_ID"] != DBNull.Value && CurrentAppendixEGas["AE_HI_GAS_ID"].ToString() != TestRecs[0]["AE_HI_GAS_ID"].ToString()))
            Category.CheckCatalogResult = "A";
          TestRecs.RowFilter = OldFilter;
        }
        else
          Log = false;
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "AppendixE51");
      }

      return ReturnVal;
    }

    public static string AppendixE52(cCategory Category, ref bool Log)
    //Maximum NOx Rate Consistent with NORX Default
    {
      string ReturnVal = "";

      try
      {
        decimal MaxNOxRate = Convert.ToDecimal(Category.GetCheckParameter("APPE_Maximum_NOx_Rate").ParameterValue);
        if (MaxNOxRate > 0 && Category.GetCheckParameter("APPE_System_Fuel_Code").ParameterValue != null)
        {
          DataRowView CurrentAppendixE = (DataRowView)Category.GetCheckParameter("Current_Appendix_E_Test").ParameterValue;
          string EndDate = cDBConvert.ToDate(CurrentAppendixE["END_DATE"], DateTypes.END).ToShortDateString();
          int EndHour = cDBConvert.ToHour(CurrentAppendixE["END_HOUR"], DateTypes.END);
          string SysFuelCd = Convert.ToString(Category.GetCheckParameter("APPE_System_Fuel_Code").ParameterValue);
          DataView MonitorDefaultRecs = (DataView)Category.GetCheckParameter("Default_Records").ParameterValue;
          string OldFilter = MonitorDefaultRecs.RowFilter;
          MonitorDefaultRecs.RowFilter = AddToDataViewFilter(OldFilter, "PARAMETER_CD = 'NORX' AND DEFAULT_PURPOSE_CD = 'MD' AND FUEL_CD = '" + SysFuelCd + "'" +
              " AND (BEGIN_DATE < '" + EndDate + "'" + " OR (BEGIN_DATE = '" + EndDate + "'" + " AND BEGIN_HOUR <= " + EndHour + ")) AND (END_DATE IS NULL OR END_DATE > '" + EndDate + "' OR (END_DATE = '" + EndDate + "' AND END_HOUR > " + EndHour + "))");
          if (MonitorDefaultRecs.Count == 0)
          {
            MonitorDefaultRecs.RowFilter = AddToDataViewFilter(OldFilter, "PARAMETER_CD = 'NORX' AND DEFAULT_PURPOSE_CD = 'MD' AND FUEL_CD = '" + SysFuelCd + "'" +
                " AND (END_DATE IS NULL OR END_DATE > '" + EndDate + "' OR (END_DATE = '" + EndDate + "' AND END_HOUR > " + EndHour + "))");
            MonitorDefaultRecs.Sort = "BEGIN_DATE";
            if (MonitorDefaultRecs.Count == 0)
              Category.CheckCatalogResult = "A";
            else
              if (cDBConvert.ToDecimal(MonitorDefaultRecs[0]["DEFAULT_VALUE"]) < MaxNOxRate)
                Category.CheckCatalogResult = "B";
          }
          else
          {
            foreach (DataRowView drv in MonitorDefaultRecs)
              if (cDBConvert.ToDecimal(drv["DEFAULT_VALUE"]) < MaxNOxRate)
              {
                Category.CheckCatalogResult = "B";
                break;
              }
          }
          MonitorDefaultRecs.RowFilter = OldFilter;
        }
        else
          Log = false;
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "AppendixE52");
      }

      return ReturnVal;
    }

    public static string AppendixE53(cCategory Category, ref bool Log)
    //Calculate Heat Input for Gas
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentAppendixEGas = (DataRowView)Category.GetCheckParameter("Current_Appendix_E_HI_for_Gas").ParameterValue;
        Category.SetCheckParameter("APPE_Gas_Calc_HI", null, eParameterDataType.Decimal);
        decimal GasGCV = cDBConvert.ToDecimal(CurrentAppendixEGas["GAS_GCV"]);
        decimal GasVol = cDBConvert.ToDecimal(CurrentAppendixEGas["GAS_VOLUME"]);
        if (GasGCV <= 0 || GasVol <= 0)
          Category.CheckCatalogResult = "A";
        else
        {
          decimal CalcHI = Math.Round(GasVol * GasGCV / 1000000, 1, MidpointRounding.AwayFromZero);
          Category.SetCheckParameter("APPE_Gas_Calc_HI", CalcHI, eParameterDataType.Decimal);
        }
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "AppendixE53");
      }

      return ReturnVal;
    }

    public static string AppendixE54(cCategory Category, ref bool Log)
    //Calculate Heat Input for Oil
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentAppendixEOil = (DataRowView)Category.GetCheckParameter("Current_Appendix_E_HI_for_Oil").ParameterValue;
        Category.SetCheckParameter("APPE_Oil_Calc_HI", null, eParameterDataType.Decimal);
        Category.SetCheckParameter("APPE_Oil_Calc_Mass_Oil", null, eParameterDataType.Decimal);
        decimal OilGCV = cDBConvert.ToDecimal(CurrentAppendixEOil["OIL_GCV"]);
        decimal OilVol = cDBConvert.ToDecimal(CurrentAppendixEOil["OIL_VOLUME"]);
        decimal OilMass = cDBConvert.ToDecimal(CurrentAppendixEOil["OIL_MASS"]);
        string OilGCVUOM = cDBConvert.ToString(CurrentAppendixEOil["OIL_GCV_UOM_CD"]);
        string OilDensUOM = cDBConvert.ToString(CurrentAppendixEOil["OIL_DENSITY_UOM_CD"]);
        if (OilGCVUOM == "" || OilGCV <= 0 || (OilVol != decimal.MinValue && OilVol <= 0) || (OilMass != decimal.MinValue && OilMass <= 0))
          Category.CheckCatalogResult = "A";
        else
          if (OilVol == decimal.MinValue && OilMass == decimal.MinValue)
            Category.CheckCatalogResult = "A";
          else
            if (OilVol == decimal.MinValue || OilDensUOM != "")
              if (OilGCVUOM != "BTULB")
                Category.CheckCatalogResult = "A";
        if (string.IsNullOrEmpty(Category.CheckCatalogResult))
        {
          decimal CalcHI;
          if (OilVol == decimal.MinValue)
          {
            CalcHI = Math.Round(OilMass * OilGCV / 1000000, 1, MidpointRounding.AwayFromZero);
            Category.SetCheckParameter("APPE_Oil_Calc_HI", CalcHI, eParameterDataType.Decimal);
          }
          else
          {
            string OilVolUOM = cDBConvert.ToString(CurrentAppendixEOil["OIL_VOLUME_UOM_CD"]);
            if (OilVolUOM == "")
              Category.CheckCatalogResult = "A";
            else
            {
              DataView UOMCrossCheckTbl = (DataView)Category.GetCheckParameter("Oil_Volume_UOM_to_Density_UOM_to_GCV_UOM").ParameterValue;
              string OldFilter = UOMCrossCheckTbl.RowFilter;
              UOMCrossCheckTbl.RowFilter = AddToDataViewFilter(OldFilter, "OilVolumeUOM = '" + OilVolUOM + "'");
              if (UOMCrossCheckTbl.Count == 0)
                Category.CheckCatalogResult = "A";
              else
              {
                if (OilDensUOM == "")
                {
                  if (OilGCVUOM != cDBConvert.ToString(UOMCrossCheckTbl[0]["OilGCVUOM"]))
                    Category.CheckCatalogResult = "A";
                  else
                  {
                    CalcHI = Math.Round(OilVol * OilGCV / 1000000, 1, MidpointRounding.AwayFromZero);
                    Category.SetCheckParameter("APPE_Oil_Calc_HI", CalcHI, eParameterDataType.Decimal);
                  }
                }
                else
                  if (OilDensUOM != cDBConvert.ToString(UOMCrossCheckTbl[0]["OilDensityUOM"]))
                    Category.CheckCatalogResult = "A";
                  else
                  {
                    decimal OilCalcMassOil = OilVol * cDBConvert.ToDecimal(CurrentAppendixEOil["OIL_DENSITY"]);
                    CalcHI = Math.Round(OilCalcMassOil * OilGCV / 1000000, 1, MidpointRounding.AwayFromZero);
                    Category.SetCheckParameter("APPE_Oil_Calc_HI", CalcHI, eParameterDataType.Decimal);
                    OilCalcMassOil = Math.Round(OilCalcMassOil, 1, MidpointRounding.AwayFromZero);
                    Category.SetCheckParameter("APPE_Oil_Calc_Mass_Oil", OilCalcMassOil, eParameterDataType.Decimal);
                  }
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "AppendixE54");
      }

      return ReturnVal;
    }

    public static string AppendixE55(cCategory Category, ref bool Log)
    //Calculate Appendix E Run
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentAppendixERun = (DataRowView)Category.GetCheckParameter("Current_Appendix_E_Run").ParameterValue;
        Category.SetCheckParameter("APPE_Run_Calc_HI", null, eParameterDataType.Decimal);
        Category.SetCheckParameter("APPE_Run_Calc_HI_Rate", null, eParameterDataType.Decimal);
        DateTime EndDate = cDBConvert.ToDate(CurrentAppendixERun["END_DATE"], DateTypes.END);
        int EndHour = cDBConvert.ToInteger(CurrentAppendixERun["END_HOUR"]);
        int EndMin = cDBConvert.ToInteger(CurrentAppendixERun["END_MIN"]);
        DateTime BeginDate = cDBConvert.ToDate(CurrentAppendixERun["BEGIN_DATE"], DateTypes.START);
        int BeginHour = cDBConvert.ToInteger(CurrentAppendixERun["BEGIN_HOUR"]);
        int BeginMin = cDBConvert.ToInteger(CurrentAppendixERun["BEGIN_MIN"]);
        if (EndDate == DateTime.MaxValue || EndHour < 0 || 23 < EndHour || EndMin < 0 || 59 < EndMin)
          Category.CheckCatalogResult = "A";
        else
          if (BeginDate == DateTime.MinValue || BeginHour < 0 || 23 < BeginHour || BeginMin < 0 || 59 < BeginMin)
            Category.CheckCatalogResult = "A";
          else
            if (BeginDate > EndDate || (BeginDate == EndDate && (BeginHour > EndHour || (BeginHour == EndHour && BeginMin >= EndMin))))
              Category.CheckCatalogResult = "A";
        if (string.IsNullOrEmpty(Category.CheckCatalogResult))
        {
          decimal tmpHI = 0;
          DataView OilRecs = (DataView)Category.GetCheckParameter("APPE_Oil_Records").ParameterValue;
          string OldFilterOil = OilRecs.RowFilter;
          string RunId = cDBConvert.ToString(CurrentAppendixERun["AE_CORR_TEST_RUN_ID"]);
          OilRecs.RowFilter = AddToDataViewFilter(OldFilterOil, "AE_CORR_TEST_RUN_ID = '" + RunId + "'");
          decimal OilHI;
          foreach (DataRowView drvOil in OilRecs)
          {
            OilHI = cDBConvert.ToDecimal(drvOil["OIL_HI"]);
            if (OilHI <= 0)
            {
              Category.CheckCatalogResult = "A";
              break;
            }
            else
              tmpHI += OilHI;
          }
          OilRecs.RowFilter = OldFilterOil;
          if (string.IsNullOrEmpty(Category.CheckCatalogResult))
          {
            DataView GasRecs = (DataView)Category.GetCheckParameter("APPE_Gas_Records").ParameterValue;
            string OldFilterGas = GasRecs.RowFilter;
            GasRecs.RowFilter = AddToDataViewFilter(OldFilterGas, "AE_CORR_TEST_RUN_ID = '" + RunId + "'");
            decimal GasHI;
            foreach (DataRowView drvGas in GasRecs)
            {
              GasHI = cDBConvert.ToDecimal(drvGas["GAS_HI"]);
              if (GasHI <= 0)
              {
                Category.CheckCatalogResult = "A";
                break;
              }
              else
                tmpHI += GasHI;
            }
            GasRecs.RowFilter = OldFilterGas;
            if (string.IsNullOrEmpty(Category.CheckCatalogResult))
            {
              if (tmpHI == 0)
                Category.CheckCatalogResult = "B";
              else
              {
                TimeSpan ts = EndDate - BeginDate;
                int RunLength = ((ts.Days * 24 * 60) + ((EndHour - BeginHour) * 60) + (EndMin - BeginMin));
                Category.SetCheckParameter("APPE_Run_Calc_HI", tmpHI, eParameterDataType.Decimal);
                Category.SetCheckParameter("APPE_Run_Calc_HI_Rate", Math.Round(60 * tmpHI / RunLength, 1, MidpointRounding.AwayFromZero), eParameterDataType.Decimal);
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "AppendixE55");
      }

      return ReturnVal;
    }

    public static string AppendixE56(cCategory Category, ref bool Log)
    //Calculate Appendix E Summary
    {
      string ReturnVal = "";

      try
      {
        Category.SetCheckParameter("APPE_Calc_Avg_HI_Rate", null, eParameterDataType.Decimal);
        Category.SetCheckParameter("APPE_Calc_Mean_RV", null, eParameterDataType.Decimal);
        DataRowView CurrentAppendixESummary = (DataRowView)Category.GetCheckParameter("Current_Appendix_E_Summary").ParameterValue;
        int OpLvl = cDBConvert.ToInteger(CurrentAppendixESummary["OP_LEVEL_NUM"]);
        if (OpLvl <= 0)
          Category.CheckCatalogResult = "A";
        else
        {
          decimal tmpCt = 0, tmpHI = 0, tmpRV = 0;
          DataView RunRecs = (DataView)Category.GetCheckParameter("APPE_Run_Records").ParameterValue;
          string OldFilter = RunRecs.RowFilter;
          string SumId = cDBConvert.ToString(CurrentAppendixESummary["AE_CORR_TEST_SUM_ID"]);
          RunRecs.RowFilter = AddToDataViewFilter(OldFilter, "OP_LEVEL_NUM = '" + OpLvl + "' AND AE_CORR_TEST_SUM_ID = '" + SumId + "'");
          decimal HrlyHIRate;
          decimal RefVal;
          foreach (DataRowView drv in RunRecs)
          {
            tmpCt++;
            HrlyHIRate = cDBConvert.ToDecimal(drv["HOURLY_HI_RATE"]);
            RefVal = cDBConvert.ToDecimal(drv["REF_VALUE"]);
            if (HrlyHIRate <= 0 || RefVal <= 0)
            {
              Category.CheckCatalogResult = "A";
              break;
            }
            else
            {
              tmpHI += HrlyHIRate;
              tmpRV += RefVal;
            }
          }
          RunRecs.RowFilter = OldFilter;
          if (string.IsNullOrEmpty(Category.CheckCatalogResult))
            if (tmpCt < 3)
              Category.CheckCatalogResult = "B";
            else
            {
              Category.SetCheckParameter("APPE_Calc_Avg_HI_Rate", Math.Round(tmpHI / tmpCt, 1, MidpointRounding.AwayFromZero), eParameterDataType.Decimal);
              Category.SetCheckParameter("APPE_Calc_Mean_RV", Math.Round(tmpRV / tmpCt, 3, MidpointRounding.AwayFromZero), eParameterDataType.Decimal);
            }
        }
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "AppendixE56");
      }

      return ReturnVal;
    }

    #endregion
  }
}
