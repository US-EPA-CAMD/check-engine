using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.Qa.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Extensions;


namespace ECMPS.Checks.CertEventChecks
{
  public class cCertEventChecks : cChecks
  {
    #region Constructors

    public cCertEventChecks()
    {
      CheckProcedures = new dCheckProcedure[17];
      CheckProcedures[1] = new dCheckProcedure(QACERT1);
      CheckProcedures[2] = new dCheckProcedure(QACERT2);
      CheckProcedures[3] = new dCheckProcedure(QACERT3);
      CheckProcedures[4] = new dCheckProcedure(QACERT4);
      CheckProcedures[5] = new dCheckProcedure(QACERT5);
      CheckProcedures[6] = new dCheckProcedure(QACERT6);
      CheckProcedures[7] = new dCheckProcedure(QACERT7);
      CheckProcedures[8] = new dCheckProcedure(QACERT8);
      CheckProcedures[9] = new dCheckProcedure(QACERT9);
      CheckProcedures[10] = new dCheckProcedure(QACERT10);
      CheckProcedures[11] = new dCheckProcedure(QACERT11);
      CheckProcedures[12] = new dCheckProcedure(QACERT12);
      CheckProcedures[13] = new dCheckProcedure(QACERT13);
      CheckProcedures[14] = new dCheckProcedure(QACERT14);
      CheckProcedures[15] = new dCheckProcedure(QACERT15);
      CheckProcedures[16] = new dCheckProcedure(QACERT16);
    }


    #endregion


    #region Cert Event Checks

    #region Check 1-10
    public static string QACERT1(cCategory Category, ref bool Log)
    //QA Cert Event Code  Valid
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentCertEvent = (DataRowView)Category.GetCheckParameter("Current_QA_Cert_Event").ParameterValue;
        if (CurrentCertEvent["QA_CERT_EVENT_CD"] == DBNull.Value)
            Category.CheckCatalogResult = "A";
        else {
            int EventCd = cDBConvert.ToInteger(CurrentCertEvent["QA_CERT_EVENT_CD"]);
            if (EventCd == 99)
                Category.CheckCatalogResult = "B";
        }
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "QACERT1");
      }

      return ReturnVal;
    }

    public static string QACERT2(cCategory Category, ref bool Log)
    //QA Cert Event Date Valid
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentCertEvent = (DataRowView)Category.GetCheckParameter("Current_QA_Cert_Event").ParameterValue;
        DateTime Date = cDBConvert.ToDate(CurrentCertEvent["QA_CERT_EVENT_DATE"], DateTypes.END);
        Category.SetCheckParameter("Event_Date_Valid", false, eParameterDataType.Boolean);
        if (Date == DateTime.MaxValue)
          Category.CheckCatalogResult = "A";
        else
          if (Date < new DateTime(1993, 1, 1) || Date > DateTime.Now)
            Category.CheckCatalogResult = "B";
          else
            Category.SetCheckParameter("Event_Date_Valid", true, eParameterDataType.Boolean);

      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "QACERT2");
      }

      return ReturnVal;
    }

    public static string QACERT3(cCategory Category, ref bool Log)
    //QA Cert Event Hour Valid
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentCertEvent = (DataRowView)Category.GetCheckParameter("Current_QA_Cert_Event").ParameterValue;
        int Hour = cDBConvert.ToInteger(CurrentCertEvent["QA_CERT_EVENT_HOUR"]);
        if (Hour == int.MinValue)
          Category.CheckCatalogResult = "A";
        else
          if (Hour < 0 || 23 < Hour)
            Category.CheckCatalogResult = "B";
          else
            if (cDBConvert.ToString(CurrentCertEvent["QA_CERT_EVENT_CD"]) == "800")
            {
              DateTime CertEventDate = cDBConvert.ToDate(CurrentCertEvent["QA_CERT_EVENT_DATE"], DateTypes.START);
              if ((CertEventDate.Month == 5 && CertEventDate.Day == 1) || (CertEventDate.Month == 7 && CertEventDate.Day == 31))
                if (cDBConvert.ToInteger(CurrentCertEvent["QA_CERT_EVENT_HOUR"]) != 0)
                  Category.CheckCatalogResult = "C";
            }
        if (string.IsNullOrEmpty(Category.CheckCatalogResult))
          Category.SetCheckParameter("Event_Hour_Valid", true, eParameterDataType.Boolean);
        else
          Category.SetCheckParameter("Event_Hour_Valid", false, eParameterDataType.Boolean);
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "QACERT3");
      }

      return ReturnVal;
    }

    public static string QACERT4(cCategory Category, ref bool Log)
    //QA Cert Event System Valid
    {
      string ReturnVal = "";

      try
      {
        Category.SetCheckParameter("QA_Cert_Event_System_Type", null, eParameterDataType.String);
        DataRowView CurrentCertEvent = (DataRowView)Category.GetCheckParameter("Current_QA_Cert_Event").ParameterValue;
        string MonSysId = cDBConvert.ToString(CurrentCertEvent["MON_SYS_ID"]);
        string CertEventCd = cDBConvert.ToString(CurrentCertEvent["QA_CERT_EVENT_CD"]);
        if (MonSysId == "" && CurrentCertEvent["COMPONENT_ID"] == DBNull.Value)
        {
          if (CertEventCd != "700" && CertEventCd != "950")
            Category.CheckCatalogResult = "A";
        }
        else
          if (MonSysId == "")
          {
            if (Convert.ToString(Category.GetCheckParameter("QA_Cert_Event_Required_ID_Code").ParameterValue).InList("S,B"))
              Category.CheckCatalogResult = "B";
          }
          else
          {
            DataView MonSysRecs = (DataView)Category.GetCheckParameter("Monitor_System_Records").ParameterValue;
            string OldFilter = MonSysRecs.RowFilter;
            MonSysRecs.RowFilter = AddToDataViewFilter(OldFilter, "MON_SYS_ID = '" + MonSysId + "'");
            Category.SetCheckParameter("QA_Cert_Event_System_Type", cDBConvert.ToString(MonSysRecs[0]["SYS_TYPE_CD"]), eParameterDataType.String);
            if (Convert.ToBoolean(Category.GetCheckParameter("Event_Date_Valid").ParameterValue) &&
                Convert.ToBoolean(Category.GetCheckParameter("Event_Hour_Valid").ParameterValue) &&
                !CertEventCd.InList("20,25,30,35,40,51,100,101,125,250,255,300,305,400,600,605"))
            {
              DateTime BeginDate = cDBConvert.ToDate(MonSysRecs[0]["BEGIN_DATE"], DateTypes.START);
              int BeginHour = cDBConvert.ToInteger(MonSysRecs[0]["BEGIN_HOUR"]);
              DateTime EndDate = cDBConvert.ToDate(MonSysRecs[0]["END_DATE"], DateTypes.END);
              int EndHour = cDBConvert.ToInteger(MonSysRecs[0]["END_HOUR"]);
              DateTime CertDate = cDBConvert.ToDate(CurrentCertEvent["QA_CERT_EVENT_DATE"], DateTypes.END);
              int CertHour = cDBConvert.ToInteger(CurrentCertEvent["QA_CERT_EVENT_HOUR"]);
              if (BeginDate > CertDate || (BeginDate == CertDate && BeginHour > CertHour) ||
                  (EndDate != DateTime.MaxValue && EndHour != int.MinValue && (EndDate < CertDate || (EndDate == CertDate && EndHour < CertHour))))
                Category.CheckCatalogResult = "C";
            }
            MonSysRecs.RowFilter = OldFilter;
          }
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "QACERT4");
      }

      return ReturnVal;
    }

    public static string QACERT5(cCategory Category, ref bool Log)
    //QA Cert Event Component Valid
    {
      string ReturnVal = "";

      try
      {
        Category.SetCheckParameter("QA_Cert_Event_Component_Type", null, eParameterDataType.String);
        DataRowView CurrentCertEvent = (DataRowView)Category.GetCheckParameter("Current_QA_Cert_Event").ParameterValue;
        string CompId = cDBConvert.ToString(CurrentCertEvent["COMPONENT_ID"]);
        if (CompId == "")
        {
          if (Convert.ToString(Category.GetCheckParameter("QA_Cert_Event_Required_ID_Code").ParameterValue).InList("C,B"))
            Category.CheckCatalogResult = "A";
        }
        else
        {
          DataView CompRecs = (DataView)Category.GetCheckParameter("Component_Records").ParameterValue;
          string OldFilter1 = CompRecs.RowFilter;
          CompRecs.RowFilter = AddToDataViewFilter(OldFilter1, "COMPONENT_ID = '" + CompId + "'");
          Category.SetCheckParameter("QA_Cert_Event_Component_Type", cDBConvert.ToString(CompRecs[0]["COMPONENT_TYPE_CD"]), eParameterDataType.String);
          CompRecs.RowFilter = OldFilter1;
          string MonSysId = cDBConvert.ToString(CurrentCertEvent["MON_SYS_ID"]);
          DataView SystemComponentRecords = (DataView)Category.GetCheckParameter("Location_System_Component_Records").ParameterValue;
          string OldFilter2 = SystemComponentRecords.RowFilter;

          DateTime CertDate = cDBConvert.ToDate(CurrentCertEvent["QA_CERT_EVENT_DATE"], DateTypes.END);
          int CertHour = cDBConvert.ToInteger(CurrentCertEvent["QA_CERT_EVENT_HOUR"]);

          if (Convert.ToBoolean(Category.GetCheckParameter("Event_Date_Valid").ParameterValue) &&
              Convert.ToBoolean(Category.GetCheckParameter("Event_Hour_Valid").ParameterValue) &&
              !cDBConvert.ToString(CurrentCertEvent["QA_CERT_EVENT_CD"]).InList("20,25,30,35,40,51,100,101,125,250,255,300,305,400,600"))
          {
            if (MonSysId == "")
            {
              SystemComponentRecords.RowFilter = AddToDataViewFilter(OldFilter2, "COMPONENT_ID = '" + CompId + "'" +
                  " AND (BEGIN_DATE < '" + CertDate.ToShortDateString() + "'" + " OR (BEGIN_DATE = '" + CertDate + "'" +
                  " AND BEGIN_HOUR <= " + CertHour + ")) AND (END_DATE IS NULL OR (END_DATE > '" + CertDate.ToShortDateString() + "'" +
                  " OR (END_DATE = '" + CertDate + "'" + " AND END_HOUR >= " + CertHour + ")))");
              if (SystemComponentRecords.Count == 0)
                Category.CheckCatalogResult = "B";
            }
            else
            {
              SystemComponentRecords.RowFilter = AddToDataViewFilter(OldFilter2, "MON_SYS_ID = '" + MonSysId + "'" + " AND COMPONENT_ID = '" + CompId + "'" +
                  " AND (BEGIN_DATE < '" + CertDate.ToShortDateString() + "'" + " OR (BEGIN_DATE = '" + CertDate + "'" +
                  " AND BEGIN_HOUR <= " + CertHour + ")) AND (END_DATE IS NULL OR (END_DATE > '" + CertDate.ToShortDateString() + "'" +
                  " OR (END_DATE = '" + CertDate + "'" + " AND END_HOUR >= " + CertHour + ")))");
              if (SystemComponentRecords.Count == 0)
                Category.CheckCatalogResult = "C";
            }
          }
          SystemComponentRecords.RowFilter = OldFilter2;
        }
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "QACERT5");
      }

      return ReturnVal;
    }

    public static string QACERT6(cCategory Category, ref bool Log)
    //QA Cert Event Conditional Begin Date and Hour Valid
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentCertEvent = (DataRowView)Category.GetCheckParameter("Current_QA_Cert_Event").ParameterValue;
        int BeginHour = cDBConvert.ToInteger(CurrentCertEvent["CONDITIONAL_DATA_BEGIN_HOUR"]);

        if (CurrentCertEvent["CONDITIONAL_DATA_BEGIN_DATE"] == DBNull.Value)
        {
          if (cDBConvert.ToString(CurrentCertEvent["QA_CERT_EVENT_CD"]) == "800")
            Category.CheckCatalogResult = "D";
          else
            if (BeginHour != int.MinValue)
              Category.CheckCatalogResult = "A";
        }
        else
        {
          if (BeginHour == int.MinValue)
            Category.CheckCatalogResult = "B";
          else
            if (BeginHour < 0 || 23 < BeginHour)
              Category.CheckCatalogResult = "C";
        }
        if (string.IsNullOrEmpty(Category.CheckCatalogResult))
          Category.SetCheckParameter("Conditional_Begin_Date_and_Hour_Valid", true, eParameterDataType.Boolean);
        else
          Category.SetCheckParameter("Conditional_Begin_Date_and_Hour_Valid", false, eParameterDataType.Boolean);
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "QACERT6");
      }

      return ReturnVal;
    }

    //QA Cert Event Completion Test Date and Hour Valid
    public static string QACERT7(cCategory Category, ref bool Log)
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentCertEvent = (DataRowView)Category.GetCheckParameter("Current_QA_Cert_Event").ParameterValue;
        int CompletedHour = cDBConvert.ToInteger(CurrentCertEvent["LAST_TEST_COMPLETED_HOUR"]);
        Category.SetCheckParameter("Completion_Test_Date_And_Hour_Valid", true, eParameterDataType.Boolean);
        if (CurrentCertEvent["LAST_TEST_COMPLETED_DATE"] == DBNull.Value)
        {
          if (CompletedHour != int.MinValue)
          {
            Category.SetCheckParameter("Completion_Test_Date_And_Hour_Valid", false, eParameterDataType.Boolean);
            Category.CheckCatalogResult = "A";
          }
          else
          {   // Bugzilla 10445
            Category.SetCheckParameter("Completion_Test_Date_And_Hour_Valid", false, eParameterDataType.Boolean);
            Category.CheckCatalogResult = "D";
          }
        }
        else
        {
          if (CompletedHour == int.MinValue)
          {
            Category.SetCheckParameter("Completion_Test_Date_And_Hour_Valid", false, eParameterDataType.Boolean);
            Category.CheckCatalogResult = "B";
          }
          else
            if (CompletedHour < 0 || 23 < CompletedHour)
            {
              Category.SetCheckParameter("Completion_Test_Date_And_Hour_Valid", false, eParameterDataType.Boolean);
              Category.CheckCatalogResult = "C";
            }
        }
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "QACERT7");
      }

      return ReturnVal;
    }

    public static string QACERT8(cCategory Category, ref bool Log)
    //QA Cert Event Required Test Code Valid
    {
      string ReturnVal = "";

      try
      {
        Category.SetCheckParameter("QA_Cert_Event_Valid_System_Or_Component", null, eParameterDataType.String);
        Category.SetCheckParameter("QA_Cert_Event_Required_ID_Code", null, eParameterDataType.String);
        DataRowView CurrentCertEvent = (DataRowView)Category.GetCheckParameter("Current_QA_Cert_Event").ParameterValue;
        string ReqTestCd = cDBConvert.ToString(CurrentCertEvent["REQUIRED_TEST_CD"]);
        if (ReqTestCd == "")
          Category.CheckCatalogResult = "A";
        else
        {
          DataView CodeToIDOrSystemOrComponentTable = (DataView)Category.GetCheckParameter("Required_Test_Code_to_Required_ID_and_System_or_Component_Type_Cross_Check_Table").ParameterValue;
          string OldFilter = CodeToIDOrSystemOrComponentTable.RowFilter;
          CodeToIDOrSystemOrComponentTable.RowFilter = AddToDataViewFilter(OldFilter, "RequiredTestCode = '" + ReqTestCd + "'");
          if (CodeToIDOrSystemOrComponentTable.Count > 0)
          {
            Category.SetCheckParameter("QA_Cert_Event_Valid_System_Or_Component", cDBConvert.ToString(CodeToIDOrSystemOrComponentTable[0]["SystemOrComponentType"]), eParameterDataType.String);
            Category.SetCheckParameter("QA_Cert_Event_Required_ID_Code", cDBConvert.ToString(CodeToIDOrSystemOrComponentTable[0]["RequiredIDCode"]), eParameterDataType.String);
          }
          CodeToIDOrSystemOrComponentTable.RowFilter = OldFilter;
        }
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "QACERT8");
      }

      return ReturnVal;
    }

    public static string QACERT9(cCategory Category, ref bool Log)
    //QA Cert Event Conditional Begin Hour Consistent with Event Hour
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentCertEvent = (DataRowView)Category.GetCheckParameter("Current_QA_Cert_Event").ParameterValue;
        DateTime CondDate = cDBConvert.ToDate(CurrentCertEvent["CONDITIONAL_DATA_BEGIN_DATE"], DateTypes.START);
        int CondHour = cDBConvert.ToInteger(CurrentCertEvent["CONDITIONAL_DATA_BEGIN_HOUR"]);

        if (Convert.ToBoolean(Category.GetCheckParameter("Conditional_Begin_Date_and_Hour_Valid").ParameterValue) &&
            CondDate != DateTime.MinValue && CondHour != int.MinValue &&
            Convert.ToBoolean(Category.GetCheckParameter("Event_Date_Valid").ParameterValue) &&
            Convert.ToBoolean(Category.GetCheckParameter("Event_Hour_Valid").ParameterValue))
        {
          DateTime CertDate = cDBConvert.ToDate(CurrentCertEvent["QA_CERT_EVENT_DATE"], DateTypes.END);
          int CertHour = cDBConvert.ToInteger(CurrentCertEvent["QA_CERT_EVENT_HOUR"]);
          if (CertDate > CondDate || (CertDate == CondDate && CertHour > CondHour))
            Category.CheckCatalogResult = "A";
          else
            if (cDBConvert.ToString(CurrentCertEvent["QA_CERT_EVENT_CD"]) == "800")
            {
              DateTime CertEventDate = cDBConvert.ToDate(CurrentCertEvent["QA_CERT_EVENT_DATE"], DateTypes.START);
              if (!(CertEventDate.Month == 5 && CertEventDate.Day == 1) && !(CertEventDate.Month == 7 && CertEventDate.Day == 31))
                if ((CondDate != CertEventDate || (CondDate == CertEventDate && CondHour != cDBConvert.ToInteger(CurrentCertEvent["QA_CERT_EVENT_HOUR"]))) ||
                    (CertEventDate.Month != 4 && CertEventDate.Month != 7))
                  Category.CheckCatalogResult = "B";
            }
        }
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "QACERT9");
      }

      return ReturnVal;
    }

    public static string QACERT10(cCategory Category, ref bool Log)
    //QA Cert Event Completion Test Hour Consistent with Event and Conditional Hour
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentCertEvent = (DataRowView)Category.GetCheckParameter("Current_QA_Cert_Event").ParameterValue;
        DateTime CompletedDate = cDBConvert.ToDate(CurrentCertEvent["LAST_TEST_COMPLETED_DATE"], DateTypes.END);
        int CompletedHour = cDBConvert.ToInteger(CurrentCertEvent["LAST_TEST_COMPLETED_HOUR"]);
        if (Convert.ToBoolean(Category.GetCheckParameter("Completion_Test_Date_And_Hour_Valid").ParameterValue) &&
            CompletedDate != DateTime.MaxValue && CompletedHour != int.MinValue &&
            Convert.ToBoolean(Category.GetCheckParameter("Event_Date_Valid").ParameterValue) &&
            Convert.ToBoolean(Category.GetCheckParameter("Event_Hour_Valid").ParameterValue))
        {
          DateTime BeginDate = cDBConvert.ToDate(CurrentCertEvent["CONDITIONAL_DATA_BEGIN_DATE"], DateTypes.START);
          int BeginHour = cDBConvert.ToInteger(CurrentCertEvent["CONDITIONAL_DATA_BEGIN_HOUR"]);

          if (Convert.ToBoolean(Category.GetCheckParameter("Conditional_Begin_Date_and_Hour_Valid").ParameterValue) &&
              BeginDate != DateTime.MinValue && BeginHour != int.MinValue)
          {
            if (BeginDate > CompletedDate || (BeginDate == CompletedDate && BeginHour > CompletedHour))
              Category.CheckCatalogResult = "A";
          }
          else
          {
            DateTime CertDate = cDBConvert.ToDate(CurrentCertEvent["QA_CERT_EVENT_DATE"], DateTypes.END);
            int CertHour = cDBConvert.ToInteger(CurrentCertEvent["QA_CERT_EVENT_HOUR"]);
            if (CertDate > CompletedDate || (CertDate == CompletedDate && CertHour > CompletedHour))
              Category.CheckCatalogResult = "B";
          }
        }
        else
          Log = false;
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "QACERT10");
      }

      return ReturnVal;
    }

    #endregion

    #region 11-20
    public static string QACERT11(cCategory Category, ref bool Log)
    //Duplicate QA Cert Event
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentCertEvent = (DataRowView)Category.GetCheckParameter("Current_QA_Cert_Event").ParameterValue;
        string CertEventCd = cDBConvert.ToString(CurrentCertEvent["QA_CERT_EVENT_CD"]);
        DateTime CertDate = cDBConvert.ToDate(CurrentCertEvent["QA_CERT_EVENT_DATE"], DateTypes.END);
        int CertHour = cDBConvert.ToInteger(CurrentCertEvent["QA_CERT_EVENT_HOUR"]);
        if (CertEventCd != "" && CertDate != DateTime.MaxValue && CertHour != int.MinValue)
        {
          string MonSysId = cDBConvert.ToString(CurrentCertEvent["MON_SYS_ID"]);
          string CompId = cDBConvert.ToString(CurrentCertEvent["COMPONENT_ID"]);
          DataView CertEventTests = (DataView)Category.GetCheckParameter("Qa_Certification_Event_Records").ParameterValue;
          string OldFilter = CertEventTests.RowFilter;
          if (MonSysId != "" && CompId != "")
            CertEventTests.RowFilter = AddToDataViewFilter(OldFilter, "QA_CERT_EVENT_CD = '" + CertEventCd + "'" +
                " AND QA_CERT_EVENT_DATE = '" + CertDate.ToShortDateString() + "'" + " AND QA_CERT_EVENT_HOUR = " + CertHour +
                " AND MON_SYS_ID = '" + MonSysId + "'" + " AND COMPONENT_ID = '" + CompId + "'");
          else
            if (MonSysId == "" && CompId != "")
              CertEventTests.RowFilter = AddToDataViewFilter(OldFilter, "QA_CERT_EVENT_CD = '" + CertEventCd + "'" +
                  " AND QA_CERT_EVENT_DATE = '" + CertDate.ToShortDateString() + "'" + " AND QA_CERT_EVENT_HOUR = " + CertHour +
                  " AND MON_SYS_ID IS NULL AND COMPONENT_ID = '" + CompId + "'");
            else
              if (MonSysId != "" && CompId == "")
                CertEventTests.RowFilter = AddToDataViewFilter(OldFilter, "QA_CERT_EVENT_CD = '" + CertEventCd + "'" +
                    " AND QA_CERT_EVENT_DATE = '" + CertDate.ToShortDateString() + "'" + " AND QA_CERT_EVENT_HOUR = " + CertHour +
                    " AND MON_SYS_ID = '" + MonSysId + "'" + " AND COMPONENT_ID IS NULL");
              else
                if (MonSysId == "" && CompId == "")
                  CertEventTests.RowFilter = AddToDataViewFilter(OldFilter, "QA_CERT_EVENT_CD = '" + CertEventCd + "'" +
                      " AND QA_CERT_EVENT_DATE = '" + CertDate.ToShortDateString() + "'" + " AND QA_CERT_EVENT_HOUR = " + CertHour +
                      " AND MON_SYS_ID IS NULL AND COMPONENT_ID IS NULL");
          if ((CertEventTests.Count > 0 && CurrentCertEvent["QA_CERT_EVENT_ID"] == DBNull.Value) ||
              (CertEventTests.Count > 1 && CurrentCertEvent["QA_CERT_EVENT_ID"] != DBNull.Value) ||
              (CertEventTests.Count == 1 && CurrentCertEvent["QA_CERT_EVENT_ID"] != DBNull.Value && CurrentCertEvent["QA_CERT_EVENT_ID"].ToString() != CertEventTests[0]["QA_CERT_EVENT_ID"].ToString()))
            Category.CheckCatalogResult = "A";
          CertEventTests.RowFilter = OldFilter;
        }
        else
          Log = false;
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "QACERT11");
      }

      return ReturnVal;
    }

    public static string QACERT12(cCategory Category, ref bool Log)
    //QA Cert Event System/Component Type Consistent with Event Code
    {
      string ReturnVal = "";

      try
      {
          // EC-1568  MJ  01/05/2016  Added HCL,HF
          if (QaParameters.QaCertEventSystemType.NotInList("HG,ST,HCL,HF") && QaParameters.QaCertEventComponentType.NotInList("HG,STRAIN,HCL,HF"))
        {
          DataRowView CurrentCertEvent = (DataRowView)Category.GetCheckParameter("Current_QA_Cert_Event").ParameterValue;
          int EventCd;
          int.TryParse(CurrentCertEvent["QA_CERT_EVENT_CD"].ToString(), out EventCd);
          Category.SetCheckParameter("QA_Cert_Event_and_Type_Consistent", false, eParameterDataType.Boolean);
          if (EventCd == 700 || EventCd == 950)
          {
            if (CurrentCertEvent["MON_SYS_ID"] != DBNull.Value || CurrentCertEvent["COMPONENT_ID"] != DBNull.Value)
              Category.CheckCatalogResult = "A";
          }
          else
          {
            DataView EventCdToSysCompTypeTable = (DataView)Category.GetCheckParameter("Event_Code_to_System_or_Component_Type_Cross_Check_Table").ParameterValue;
            string OldFilter = EventCdToSysCompTypeTable.RowFilter;
            EventCdToSysCompTypeTable.RowFilter = AddToDataViewFilter(OldFilter, "(EventCode1 = " + EventCd + " AND EventCode2 IS NULL) OR (EventCode1 <= " + EventCd + " AND EventCode2 >= " + EventCd + ")");
            if (EventCdToSysCompTypeTable.Count > 0)
            {
              string TblSysOrCompType = cDBConvert.ToString(EventCdToSysCompTypeTable[0]["SystemOrComponentType"]);
              string SysType = Convert.ToString(Category.GetCheckParameter("QA_Cert_Event_System_Type").ParameterValue);
              string CompType = Convert.ToString(Category.GetCheckParameter("QA_Cert_Event_Component_Type").ParameterValue);
              string ReqId = Convert.ToString(Category.GetCheckParameter("QA_Cert_Event_Required_ID_Code").ParameterValue);
              string ReqTestCd = cDBConvert.ToString(CurrentCertEvent["REQUIRED_TEST_CD"]);
              if (TblSysOrCompType != "CONC")
                if (SysType == "NOXP")
                  if (!SysType.Contains("NOXP"))
                    Category.CheckCatalogResult = "B";
              if (string.IsNullOrEmpty(Category.CheckCatalogResult))
                switch (TblSysOrCompType)
                {
                  case "CONC":
                    {
                      if (SysType != "" && ReqId.InList("B,S"))
                      {
                        if (!SysType.InList("SO2,SO2R,NOXC,NOX,CO2,O2,H2O,H2OM"))
                          Category.CheckCatalogResult = "B";
                      }
                      if (string.IsNullOrEmpty(Category.CheckCatalogResult))
                        if (CompType != "" && ReqId.InList("B,C"))
                          if (!CompType.InList("SO2,NOX,CO2,O2"))
                            Category.CheckCatalogResult = "B";
                      break;
                    }
                  case "CEM":
                    {
                      if (SysType != "" && ReqId.InList("B,S"))
                      {
                        if (!SysType.InList("SO2,SO2R,NOXC,NOX,CO2,O2,FLOW,H2O,H2OM,NOXP"))
                          Category.CheckCatalogResult = "B";
                        else
                          if (SysType == "NOXP" && ReqTestCd != "5")
                            Category.CheckCatalogResult = "B";
                      }
                      if (string.IsNullOrEmpty(Category.CheckCatalogResult))
                        if (CompType != "" && ReqId.InList("B,C"))
                          if (!CompType.InList("SO2,NOX,CO2,O2,FLOW"))
                            Category.CheckCatalogResult = "B";
                      break;
                    }
                  case "FFM":
                    {
                      if (SysType != "" && ReqId.InList("B,S"))
                      {
                        if (!SysType.InList("OILV,OILM,GAS,LTOL,LTGS"))
                          Category.CheckCatalogResult = "B";
                      }
                      if (string.IsNullOrEmpty(Category.CheckCatalogResult))
                        if (CompType != "" && ReqId.InList("B,C"))
                          if (!CompType.InList("OFFM,GFFM"))
                            Category.CheckCatalogResult = "B";
                      break;
                    }
                  case "FLOW":
                    {
                      if (SysType != "" && ReqId.InList("B,S"))
                      {
                        if (SysType != "FLOW")
                          Category.CheckCatalogResult = "B";
                      }
                      if (string.IsNullOrEmpty(Category.CheckCatalogResult))
                        if (CompType != "" && ReqId.InList("B,C"))
                          if (CompType != "FLOW")
                            Category.CheckCatalogResult = "B";
                      break;
                    }
                  case "NOX":
                    {
                      if (SysType != "" && ReqId.InList("B,S"))
                      {
                        if (!SysType.InList("NOX,NOXC"))
                          Category.CheckCatalogResult = "B";
                      }
                      if (string.IsNullOrEmpty(Category.CheckCatalogResult))
                        if (CompType != "" && ReqId.InList("B,C"))
                          if (CompType != "NOX")
                            Category.CheckCatalogResult = "B";
                      break;
                    }
                  case "SO2":
                    {
                      if (SysType != "" && ReqId.InList("B,S"))
                      {
                        if (!SysType.InList("SO2,SO2R"))
                          Category.CheckCatalogResult = "B";
                      }
                      if (string.IsNullOrEmpty(Category.CheckCatalogResult))
                        if (CompType != "" && ReqId.InList("B,C"))
                          if (CompType != "SO2")
                            Category.CheckCatalogResult = "B";
                      break;
                    }
                  case "NOXE":
                    {
                      if (SysType != "" && ReqId.InList("B,S"))
                        if (SysType != "NOXE")
                          Category.CheckCatalogResult = "B";
                      break;
                    }
                  case "OP":
                    {
                      if (SysType != "" && ReqId.InList("B,S"))
                        if (!SysType.InList("OP,PM"))
                          Category.CheckCatalogResult = "B";
                      break;
                    }
                  case "H2O":
                  case "DAHS":
                    {
                      if (CompType != "" && ReqId.InList("B,C"))
                        if (CompType != TblSysOrCompType)
                          Category.CheckCatalogResult = "B";
                      break;
                    }
                  default:
                    break;
                }
            }
            EventCdToSysCompTypeTable.RowFilter = OldFilter;
            if (string.IsNullOrEmpty(Category.CheckCatalogResult))
              Category.SetCheckParameter("QA_Cert_Event_and_Type_Consistent", true, eParameterDataType.Boolean);
          }
        }
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "QACERT12");
      }

      return ReturnVal;
    }

    public static string QACERT13(cCategory Category, ref bool Log)
    //QA Cert Event Required Test Code Consistent with System and Component
    {
      string ReturnVal = "";

      try
      {
        //  EC-1568  MJ  01/05/2016  Added HCL,HF
        if (QaParameters.QaCertEventSystemType.NotInList("HG,ST,HCL,HF") && QaParameters.QaCertEventComponentType.NotInList("HG,STRAIN,HCL,HF"))
        {
          DataRowView CurrentCertEvent = (DataRowView)Category.GetCheckParameter("Current_QA_Cert_Event").ParameterValue;
          string RequCd = cDBConvert.ToString(CurrentCertEvent["REQUIRED_TEST_CD"]);
          if (RequCd == "76" || RequCd == "77")
          {
            if (CurrentCertEvent["MON_SYS_ID"] != DBNull.Value || CurrentCertEvent["COMPONENT_ID"] != DBNull.Value)
              Category.CheckCatalogResult = "A";
          }
          else
          {
            string ValidSysOrComp = Convert.ToString(Category.GetCheckParameter("QA_Cert_Event_Valid_System_Or_Component").ParameterValue);
            string SysType = Convert.ToString(Category.GetCheckParameter("QA_Cert_Event_System_Type").ParameterValue);
            string CompType = Convert.ToString(Category.GetCheckParameter("QA_Cert_Event_Component_Type").ParameterValue);
            string ReqId = Convert.ToString(Category.GetCheckParameter("QA_Cert_Event_Required_ID_Code").ParameterValue);
            string ReqTestCd = cDBConvert.ToString(CurrentCertEvent["REQUIRED_TEST_CD"]);
            switch (ValidSysOrComp)
            {
              case "CONC":
                {
                  if (SysType != "" && ReqId.InList("B,S"))
                  {
                    if (!SysType.InList("SO2,SO2R,NOXC,NOX,CO2,O2,H2O,H2OM"))
                      Category.CheckCatalogResult = "B";
                  }
                  if (string.IsNullOrEmpty(Category.CheckCatalogResult))
                    if (CompType != "" && ReqId.InList("B,C"))
                      if (!CompType.InList("SO2,NOX,CO2,O2"))
                        Category.CheckCatalogResult = "B";
                  break;
                }
              case "CEM":
                {
                  if (SysType != "" && ReqId.InList("B,S"))
                  {
                    if (!SysType.InList("SO2,SO2R,NOXC,NOX,CO2,O2,FLOW,H2O,H2OM,NOXP"))
                      Category.CheckCatalogResult = "B";
                    else
                      if (SysType == "NOXP" && ReqTestCd != "5")
                        Category.CheckCatalogResult = "B";
                  }
                  if (string.IsNullOrEmpty(Category.CheckCatalogResult))
                    if (CompType != "" && ReqId.InList("B,C"))
                      if (!CompType.InList("SO2,NOX,CO2,O2,FLOW"))
                        Category.CheckCatalogResult = "B";
                  break;
                }
              case "RATA":
                {   // EC-1568  MJ  01/05/2016  Removed HG,ST
                  if (SysType != "" && ReqId.InList("B,S"))
                    if (!SysType.InList("SO2,SO2R,NOXC,NOX,CO2,O2,FLOW,H2O,H2OM"))
                      Category.CheckCatalogResult = "B";
                  break;
                }
              case "FFM":
                {
                  if (SysType != "" && ReqId.InList("B,S"))
                  {
                    if (!SysType.InList("OILV,OILM,GAS,LTOL,LTGS"))
                      Category.CheckCatalogResult = "B";
                  }
                  if (string.IsNullOrEmpty(Category.CheckCatalogResult))
                    if (CompType != "" && ReqId.InList("B,C"))
                      if (!CompType.InList("OFFM,GFFM"))
                        Category.CheckCatalogResult = "B";
                  break;
                }
              case "FLOW":
                {
                  if (SysType != "" && ReqId.InList("B,S"))
                  {
                    if (SysType != "FLOW")
                      Category.CheckCatalogResult = "B";
                  }
                  if (string.IsNullOrEmpty(Category.CheckCatalogResult))
                    if (CompType != "" && ReqId.InList("B,C"))
                      if (CompType != "FLOW")
                        Category.CheckCatalogResult = "B";
                  break;
                }
              case "NOXP":
              case "NOXE":
                {
                  if (SysType != "" && ReqId.InList("B,S"))
                    if (SysType != ValidSysOrComp)
                      Category.CheckCatalogResult = "B";
                  break;
                }
              case "OP":
                {
                  if (SysType != "" && ReqId.InList("B,S"))
                    if (!SysType.InList("OP,PM"))
                      Category.CheckCatalogResult = "B";
                  break;
                }
              case "DAHS":
                {
                  if (CompType != "" && ReqId.InList("B,C"))
                    if (CompType != "DAHS")
                      Category.CheckCatalogResult = "B";
                  break;
                }
              default:
                break;
            }
          }
        }
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "QACERT13");
      }

      return ReturnVal;
    }

    public static string QACERT14(cCategory Category, ref bool Log)
    //QA Cert Event Date Consistent with Other Data
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentCertEvent = (DataRowView)Category.GetCheckParameter("Current_QA_Cert_Event").ParameterValue;
        if (Convert.ToBoolean(Category.GetCheckParameter("Event_Date_Valid").ParameterValue) &&
            Convert.ToBoolean(Category.GetCheckParameter("Event_Hour_Valid").ParameterValue))
        {
          string EventCd = cDBConvert.ToString(CurrentCertEvent["QA_CERT_EVENT_CD"]);
          DateTime CertDate = cDBConvert.ToDate(CurrentCertEvent["QA_CERT_EVENT_DATE"], DateTypes.START);
          int CertHour = cDBConvert.ToHour(CurrentCertEvent["QA_CERT_EVENT_HOUR"], DateTypes.START);
          string CompId;

          DataView ControlRecs;
          DataView AnalyzerRangeRecs;
          DataView OpStatusRecs;

          DataView FilteredAnalyzerRangeRecs;
          DataView FilteredOpStatusRecs;

          sFilterPair[] ControlFilter;
          sFilterPair[] AnalyzerFilter;
          sFilterPair[] OpStatusFilter;
          sFilterPair[] SpanFilter;

          if (EventCd == "20")
          {

            ControlRecs = (DataView)Category.GetCheckParameter("Location_Control_Records").ParameterValue;
            ControlFilter = new sFilterPair[2];
            ControlFilter[0].Set("CE_PARAM", "SO2");
            ControlFilter[1].Set("INSTALL_DATE", CertDate, eFilterDataType.DateBegan, eFilterPairRelativeCompare.LessThanOrEqual);
            if (CountRows(ControlRecs, ControlFilter) == 0)
              Category.CheckCatalogResult = "A";
          }
          else
            if (EventCd.InList("25,26"))
            {
              ControlRecs = (DataView)Category.GetCheckParameter("Location_Control_Records").ParameterValue;
              ControlFilter = new sFilterPair[2];
              ControlFilter[0].Set("CE_PARAM", "NOX");
              ControlFilter[1].Set("INSTALL_DATE", CertDate, eFilterDataType.DateBegan, eFilterPairRelativeCompare.LessThanOrEqual);
              if (CountRows(ControlRecs, ControlFilter) == 0)
                Category.CheckCatalogResult = "B";
            }
            else
            {
              CompId = cDBConvert.ToString(CurrentCertEvent["COMPONENT_ID"]);
              if (EventCd == "30" && CompId != "")
              {
                AnalyzerRangeRecs = (DataView)Category.GetCheckParameter("Analyzer_Range_Records").ParameterValue;
                AnalyzerFilter = new sFilterPair[3];
                AnalyzerFilter[0].Set("ANALYZER_RANGE_CD", "A,L", eFilterPairStringCompare.InList);
                AnalyzerFilter[1].Set("BEGIN_DATE", CertDate, eFilterDataType.DateBegan);
                AnalyzerFilter[2].Set("BEGIN_HOUR", CertHour, eFilterDataType.Integer);
                if (CountRows(AnalyzerRangeRecs, AnalyzerFilter) == 0)
                  Category.CheckCatalogResult = "C";
              }
              else
                if (EventCd == "35" && CompId != "")
                {
                  AnalyzerRangeRecs = (DataView)Category.GetCheckParameter("Analyzer_Range_Records").ParameterValue;
                  AnalyzerFilter = new sFilterPair[3];
                  AnalyzerFilter[0].Set("ANALYZER_RANGE_CD", "A,H", eFilterPairStringCompare.InList);
                  AnalyzerFilter[1].Set("BEGIN_DATE", CertDate, eFilterDataType.DateBegan);
                  AnalyzerFilter[2].Set("BEGIN_HOUR", CertHour, eFilterDataType.Integer);
                  if (CountRows(AnalyzerRangeRecs, AnalyzerFilter) == 0)
                    Category.CheckCatalogResult = "D";
                }
                else
                  if (EventCd.InList("50,51"))
                  {
                    OpStatusRecs = (DataView)Category.GetCheckParameter("Location_Operating_Status_Records").ParameterValue;
                    OpStatusFilter = new sFilterPair[1];
                    OpStatusFilter[0].Set("OP_STATUS_CD", "LTCS");
                    FilteredOpStatusRecs = FindActiveRows(OpStatusRecs, DateTime.MinValue, CertDate, "BEGIN_DATE", "BEGIN_DATE", true, true, OpStatusFilter);
                    if (FilteredOpStatusRecs.Count == 0)
                      Category.CheckCatalogResult = "E";
                    else
                    {
                      DateTime FoundRecDate = cDBConvert.ToDate(FilteredOpStatusRecs[0]["BEGIN_DATE"], DateTypes.START);
                      OpStatusFilter[0].Set("OP_STATUS_CD", "OPR");
                      FilteredOpStatusRecs = FindActiveRows(OpStatusRecs, FoundRecDate, CertDate, "BEGIN_DATE", "BEGIN_DATE", false, true, OpStatusFilter);
                      if (FilteredOpStatusRecs.Count == 0)
                        Category.CheckCatalogResult = "E";
                    }
                  }
                  else
                  {
                    string CompType = cDBConvert.ToString(Category.GetCheckParameter("QA_Cert_Event_Component_Type").ParameterValue);
                    if (EventCd.InList("170,171,172") && CompType.InList("SO2,NOX,CO2,O2"))
                    {
                      AnalyzerRangeRecs = (DataView)Category.GetCheckParameter("Analyzer_Range_Records").ParameterValue;
                      FilteredAnalyzerRangeRecs = FindActiveRows(AnalyzerRangeRecs, CertDate, CertHour, CertDate, CertHour, "BEGIN_DATE", "BEGIN_HOUR", "END_DATE", "END_HOUR");
                      if (FilteredAnalyzerRangeRecs.Count > 0)
                      {
                        DataView SpanRecs = (DataView)Category.GetCheckParameter("Span_Records").ParameterValue;
                        string AnalyzerRangeCd = cDBConvert.ToString(FilteredAnalyzerRangeRecs[0]["ANALYZER_RANGE_CD"]);
                        if (AnalyzerRangeCd == "A")
                        {
                          SpanFilter = new sFilterPair[3];
                          SpanFilter[0].Set("COMPONENT_TYPE_CD", CompType);
                          SpanFilter[1].Set("BEGIN_DATE", CertDate, eFilterDataType.DateBegan);
                          SpanFilter[2].Set("BEGIN_HOUR", CertHour, eFilterDataType.Integer);
                          if (CountRows(SpanRecs, SpanFilter) == 0)
                            Category.CheckCatalogResult = "F";
                        }
                        else
                        {
                          SpanFilter = new sFilterPair[4];
                          SpanFilter[0].Set("COMPONENT_TYPE_CD", CompType);
                          SpanFilter[1].Set("BEGIN_DATE", CertDate, eFilterDataType.DateBegan);
                          SpanFilter[2].Set("BEGIN_HOUR", CertHour, eFilterDataType.Integer);
                          SpanFilter[3].Set("SPAN_SCALE_CD", AnalyzerRangeCd);
                          if (CountRows(SpanRecs, SpanFilter) == 0)
                            Category.CheckCatalogResult = "F";
                        }
                      }
                    }
                    else
                      if (EventCd == "800")
                      {
                        int CertQtr = cDateFunctions.ThisQuarter(CertDate);
                        int CertYear = CertDate.Year;
                        DataView RptPeriodLookup = (DataView)Category.GetCheckParameter("Reporting_Period_Lookup_Table").ParameterValue;
                        sFilterPair[] RptPerFilter = new sFilterPair[2];
                        RptPerFilter[0].Set("CALENDAR_YEAR", CertYear, eFilterDataType.Integer);
                        RptPerFilter[1].Set("QUARTER", CertQtr, eFilterDataType.Integer);
                        DataView RptPeriodLookupFiltered = FindRows(RptPeriodLookup, RptPerFilter);
                        int CertRptPerId = cDBConvert.ToInteger(RptPeriodLookupFiltered[0]["RPT_PERIOD_ID"]);

                        DataView FrequRecs = (DataView)Category.GetCheckParameter("Location_Reporting_Frequency_Records").ParameterValue;
                        sFilterPair[] FrequFilter = new sFilterPair[3];
                        FrequFilter[0].Set("REPORT_FREQ_CD", "Q");
                        FrequFilter[1].Set("BEGIN_RPT_PERIOD_ID", CertRptPerId, eFilterDataType.Integer, eFilterPairRelativeCompare.LessThanOrEqual);
                        FrequFilter[2].Set("END_RPT_PERIOD_ID", DBNull.Value, eFilterDataType.Integer);
                        DataView FrequRecsFiltered = FindRows(FrequRecs, FrequFilter);
                        if (FrequRecsFiltered.Count == 0)
                        {
                          FrequFilter[2].Set("END_RPT_PERIOD_ID", CertRptPerId, eFilterDataType.Integer, eFilterPairRelativeCompare.GreaterThanOrEqual);
                          FrequRecsFiltered = FindRows(FrequRecs, FrequFilter);
                        }
                        if (FrequRecsFiltered.Count > 0)
                          Category.CheckCatalogResult = "G";
                      }
                  }
            }
        }
        else
          Log = false;
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "QACERT14");
      }

      return ReturnVal;
    }

    public static string QACERT15(cCategory Category, ref bool Log)
    //QA Cert Event Code Consistent with Required Test Code
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentCertEvent = (DataRowView)Category.GetCheckParameter("Current_QA_Cert_Event").ParameterValue;
        string ReqTestCd = cDBConvert.ToString(CurrentCertEvent["REQUIRED_TEST_CD"]);
        DateTime EventDate = cDBConvert.ToDate(CurrentCertEvent["QA_CERT_EVENT_DATE"], DateTypes.START);

        if (Category.GetCheckParameter("QA_Cert_Event_and_Type_Consistent").ValueAsBool() && ReqTestCd != "" &&
            QaParameters.QaCertEventSystemType.NotInList("HG,ST") && QaParameters.QaCertEventComponentType.NotInList("HG,STRAIN"))
        {
          string MissingTestTypes = "";
          string CertEvSysType = Category.GetCheckParameter("QA_Cert_Event_System_Type").ValueAsString();
          int EventCd = cDBConvert.ToInteger(CurrentCertEvent["QA_CERT_EVENT_CD"]);
          DataView CrossCheckRowsFoundReq;
          DataView ReqCodeTypeCrossCheck;
          sFilterPair[] FilterCrossCheck;
          if (CertEvSysType == "NOXP" && 40 <= EventCd && EventCd <= 51)
          {
            ReqCodeTypeCrossCheck = Category.GetCheckParameter("Test_Type_to_Required_Test_Code_Cross_Check_Table").ValueAsDataView();
            FilterCrossCheck = new sFilterPair[2];
            FilterCrossCheck[0].Set("RequiredTestCode", ReqTestCd);
            FilterCrossCheck[1].Set("TestTypeCode", "RATA");
            CrossCheckRowsFoundReq = FindRows(ReqCodeTypeCrossCheck, FilterCrossCheck);
            if (CrossCheckRowsFoundReq.Count == 0)
              MissingTestTypes = MissingTestTypes.ListAdd("RATA");
          }

          else if (EventCd == 312)
          {
            DataRowView rataToRequiredRow = cRowFilter.FindRow(
                                                                Category.GetCheckParameter("Test_Type_to_Required_Test_Code_Cross_Check_Table").ValueAsDataView(),
                                                                new cFilterCondition[]
                                                                    {
                                                                      new cFilterCondition("TestTypeCode", "RATA", eFilterConditionStringCompare.BeginsWith),
                                                                      new cFilterCondition("RequiredTestCode", ReqTestCd)
                                                                    }
                                                              );

            DataRowView abbreviatedF2lToRequiredRow = cRowFilter.FindRow(
                                                                          Category.GetCheckParameter("Test_Type_to_Required_Test_Code_Cross_Check_Table").ValueAsDataView(),
                                                                          new cFilterCondition[]
                                                                              {
                                                                                new cFilterCondition("TestTypeCode", "AF2LCHK"),
                                                                                new cFilterCondition("RequiredTestCode", ReqTestCd)
                                                                              }
                                                                        );

            if ((rataToRequiredRow == null) && (abbreviatedF2lToRequiredRow == null))
              MissingTestTypes = "AF2LCHK or RATA";
          }

          else if (EventCd < 20 || 26 < EventCd || (EventCd == 20 && CertEvSysType.StartsWith("SO2")) ||
                   ((EventCd == 25 || EventCd == 26) && CertEvSysType.StartsWith("NOX")))
          {
            DataView EvCodeTypeCrossCheck = Category.GetCheckParameter("Event_Code_to_Test_Type_Codes_Cross_Check_Table").ValueAsDataView();
            string TheseTestTypeCds = "";
            foreach (DataRowView drv in EvCodeTypeCrossCheck)
            {
              int EvCd1 = cDBConvert.ToInteger(drv["EventCode1"]);
              int EvCd2 = cDBConvert.ToInteger(drv["EventCode2"]);
              if ((EvCd1 == EventCd && EvCd2 == int.MinValue) || (EvCd1 <= EventCd && EvCd2 >= EventCd))
                TheseTestTypeCds = TheseTestTypeCds.ListAdd(cDBConvert.ToString(drv["TestTypeCode"]));
            }
            if (TheseTestTypeCds != "")
            {
              string ThisTestType;
              DataView QualRecords = Category.GetCheckParameter("Facility_Qualification_Records").ValueAsDataView();
              sFilterPair[] QualFilter = new sFilterPair[2];
              QualFilter[0].Set("QUAL_TYPE_CD", "PK,SK", eFilterPairStringCompare.InList);
              DataView FilteredQualRecs;
              ReqCodeTypeCrossCheck = Category.GetCheckParameter("Test_Type_to_Required_Test_Code_Cross_Check_Table").ValueAsDataView();
              FilterCrossCheck = new sFilterPair[2];
              FilterCrossCheck[0].Set("RequiredTestCode", ReqTestCd);
              for (int i = 0; i < TheseTestTypeCds.ListCount(); i++)
              {
                ThisTestType = TheseTestTypeCds.ListItem(i);

                if (ThisTestType == "LEAK")
                {
                  if ((CertEvSysType == "FLOW") && (CurrentCertEvent["ACQ_CD"].AsString() == "DP"))
                  {
                    FilterCrossCheck[1].Set("TestTypeCode", "LEAK");

                    if (CountRows(ReqCodeTypeCrossCheck, FilterCrossCheck) == 0)
                      MissingTestTypes = MissingTestTypes.ListAdd("LEAK");
                  }
                }

                else if ((CertEvSysType != "FLOW" || ThisTestType != "LINE") &&
                         (CertEvSysType != "H2OM" || ThisTestType == "RATA"))
                {
                  FilterCrossCheck[1].Set("TestTypeCode", ThisTestType, eFilterPairStringCompare.BeginsWith);
                  if (CountRows(ReqCodeTypeCrossCheck, FilterCrossCheck) == 0)
                    if (ThisTestType == "7DAY")
                    {
                      bool spanExemption = false;
                      {
                        if ((EventCd == 125) && CurrentCertEvent["COMPONENT_TYPE_CD"].AsString().InList("NOX,SO2"))
                        {
                          DataView analyzerRangeView = cRowFilter.FindActiveRows(
                                                                                  Category.GetCheckParameter("Analyzer_Range_Records").AsDataView(),
                                                                                  CurrentCertEvent["QA_CERT_EVENT_DATE"].AsDateTime(DateTime.MinValue),
                                                                                  CurrentCertEvent["QA_CERT_EVENT_HOUR"].AsInteger(0),
                                                                                  new cFilterCondition[]
                                                                                  {
                                                                                    new cFilterCondition("COMPONENT_ID", CurrentCertEvent["COMPONENT_ID"].AsString())
                                                                                  }
                                                                                );

                          if ((analyzerRangeView.Count > 0) && (analyzerRangeView[0]["DUAL_RANGE_IND"].AsInteger(0) == 0))
                          {
                            DataView spanView = cRowFilter.FindActiveRows(
                                                                           Category.GetCheckParameter("Span_Records").AsDataView(),
                                                                           CurrentCertEvent["QA_CERT_EVENT_DATE"].AsDateTime(DateTime.MinValue),
                                                                           CurrentCertEvent["QA_CERT_EVENT_HOUR"].AsInteger(0),
                                                                           new cFilterCondition[]
                                                                           {
                                                                             new cFilterCondition("COMPONENT_TYPE_CD", CurrentCertEvent["COMPONENT_TYPE_CD"].AsString()),
                                                                             new cFilterCondition("SPAN_SCALE_CD", analyzerRangeView[0]["ANALYZER_RANGE_CD"].AsString())
                                                                           }
                                                                         );

                            if ((spanView.Count > 0) && (spanView[0]["SPAN_VALUE"].AsDecimal(Decimal.MaxValue) <= 50))
                            {
                              spanExemption = true;
                            }
                          }
                        }
                      }

                      if (!spanExemption)
                      {
                        if (cDBConvert.ToString(CurrentCertEvent["LOCATION_IDENTIFIER"]).StartsWith("MS") ||
                            cDBConvert.ToString(CurrentCertEvent["LOCATION_IDENTIFIER"]).StartsWith("CS"))
                        {
                          DataView USCRecords = Category.GetCheckParameter("Unit_Stack_Configuration_Records").ValueAsDataView();
                          foreach (DataRowView drv in USCRecords)
                          {
                            QualFilter[1].Set("MON_LOC_ID", cDBConvert.ToString(drv["MON_LOC_ID"]));
                            FilteredQualRecs = FindActiveRows(QualRecords, EventDate, EventDate, "BEGIN_DATE", "END_DATE", true, true, QualFilter);
                            if (FilteredQualRecs.Count == 0)
                            {
                              MissingTestTypes = MissingTestTypes.ListAdd(ThisTestType);
                              break;
                            }
                          }
                        }
                        else
                        {
                          QualFilter[1].Set("MON_LOC_ID", cDBConvert.ToString(CurrentCertEvent["MON_LOC_ID"]));
                          FilteredQualRecs = FindActiveRows(QualRecords, EventDate, EventDate, "BEGIN_DATE", "END_DATE", true, true, QualFilter);
                          if (FilteredQualRecs.Count == 0)
                            MissingTestTypes = MissingTestTypes.ListAdd(ThisTestType);
                        }
                      }
                    }
                    else
                      MissingTestTypes = MissingTestTypes.ListAdd(ThisTestType);
                  else
                  {
                    if (EventCd == 800)
                    {
                      MissingTestTypes = "";
                      break;
                    }
                  }
                }
              }
            }
          }
          if (MissingTestTypes != "")
          {
            Category.CheckCatalogResult = "A";
            Category.SetCheckParameter("QA_Cert_Event_Missing_Test_Types", MissingTestTypes, eParameterDataType.String);
          }
          else
            Category.SetCheckParameter("QA_Cert_Event_Missing_Test_Types", null, eParameterDataType.String);
        }
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "QACERT15");
      }

      return ReturnVal;
    }

    public static string QACERT16(cCategory Category, ref bool Log)
    //MP Evaluation Check
    {
      string ReturnVal = "";

      try
      {

        DataView MPLocationRecords = (DataView)Category.GetCheckParameter("Monitoring_Plan_Location_Records_for_QA").ParameterValue;
        DataRowView CurrentCertEvent = (DataRowView)Category.GetCheckParameter("Current_QA_Cert_Event").ParameterValue;
        DateTime Date = cDBConvert.ToDate(CurrentCertEvent["QA_CERT_EVENT_DATE"], DateTypes.END);
        int CertQtr = cDateFunctions.ThisQuarter(Date);
        int CertYr = Date.Year;
        string OldFilter1 = MPLocationRecords.RowFilter;
        MPLocationRecords.RowFilter = AddToDataViewFilter(OldFilter1, "(SEVERITY_CD = 'CRIT1' OR SEVERITY_CD = 'FATAL') AND (END_YEAR IS NULL OR END_QUARTER IS NULL OR END_YEAR > " + CertYr +
            " OR (END_YEAR = " + CertYr + " AND END_QUARTER >= " + CertQtr + "))");
        if (MPLocationRecords.Count > 0)
          Category.CheckCatalogResult = "A";
        else
        {
          MPLocationRecords.RowFilter = AddToDataViewFilter(OldFilter1, "(NEEDS_EVAL_FLG = 'Y' AND MUST_SUBMIT = 'Y') AND (END_YEAR IS NULL OR END_QUARTER IS NULL OR END_YEAR > " + CertYr +
              " OR (END_YEAR = " + CertYr + " AND END_QUARTER >= " + CertQtr + "))");
          if (MPLocationRecords.Count > 0)
            Category.CheckCatalogResult = "B";
        }
        MPLocationRecords.RowFilter = OldFilter1;
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "QACERT16");
      }

      return ReturnVal;
    }
    #endregion

    #endregion
  }
}
