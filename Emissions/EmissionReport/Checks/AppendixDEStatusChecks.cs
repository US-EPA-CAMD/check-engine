using System;
using System.Data;
using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.EmissionsReport;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;
using ECMPS.Definitions.Extensions;

namespace ECMPS.Checks.EmissionsChecks
{
  public class cAppendixDEStatusChecks : cEmissionsChecks
  {
    #region Constructors

    public cAppendixDEStatusChecks(cEmissionsReportProcess emissionReportProcess) 
      : base(emissionReportProcess)
    {
      CheckProcedures = new dCheckProcedure[14];

      CheckProcedures[1] = new dCheckProcedure(ADESTAT1);
      CheckProcedures[2] = new dCheckProcedure(ADESTAT2);
      CheckProcedures[3] = new dCheckProcedure(ADESTAT3);
      CheckProcedures[4] = new dCheckProcedure(ADESTAT4);
      CheckProcedures[5] = new dCheckProcedure(ADESTAT5);
      CheckProcedures[6] = new dCheckProcedure(ADESTAT6);
      CheckProcedures[7] = new dCheckProcedure(ADESTAT7);
      CheckProcedures[8] = new dCheckProcedure(ADESTAT8);
      CheckProcedures[9] = new dCheckProcedure(ADESTAT9);
      CheckProcedures[10] = new dCheckProcedure(ADESTAT10);
      CheckProcedures[11] = new dCheckProcedure(ADESTAT11);
      CheckProcedures[12] = new dCheckProcedure(ADESTAT12);
      CheckProcedures[13] = new dCheckProcedure(ADESTAT13);
    }

    #endregion

    #region Public Static Methods: Checks

    public string ADESTAT1(cCategory Category, ref bool Log)
    //Determine Appendix E Status       
    {
      string ReturnVal = "";

      try
      {
        Category.SetCheckParameter("Prior_Appendix_E_Record", null, eParameterDataType.DataRowView);
        Category.SetCheckParameter("Invalid_Appendix_E_Record", null, eParameterDataType.DataRowView);
        Category.SetCheckParameter("Current_Appendix_E_Status", null, eParameterDataType.String);
        Category.SetCheckParameter("Prior_Appendix_E_Event_Record", null, eParameterDataType.DataRowView);
        Category.SetCheckParameter("Subsequent_Appendix_E_Record", null, eParameterDataType.DataRowView);
        Category.SetCheckParameter("Appendix_E_Missing_Op_Data_Info", null, eParameterDataType.String);

        string AppEOpCd = Category.GetCheckParameter("App_E_Op_Code").ValueAsString();
        string FuelCd = Category.GetCheckParameter("App_E_Fuel_Code").ValueAsString();
        string AppENOxSysId = Category.GetCheckParameter("App_E_NOXE_System_ID").ValueAsString();
        string Status = "";
        if( AppEOpCd.InList( "N,W,X,Y,Z" ) && AppENOxSysId != "" )
        {
          NoxeSystemIdArray.AggregateValue(AppENOxSysId, LocationPos.Value.Value, Category);

          DateTime CurrentDate = Category.GetCheckParameter("Current_Operating_Date").ValueAsDateTime(DateTypes.END);
          int CurrentHour = Category.GetCheckParameter("Current_Operating_Hour").ValueAsInt();
          DataView AppERecsQAStatus = Category.GetCheckParameter("Appendix_E_Test_Records_By_Location_For_QA_Status").ValueAsDataView();
          DataView OpSuppRecs;
          DataView OpSuppRecsFiltered;
          sFilterPair[] OpSuppFilter;
          sFilterPair[] AppERecsFilter = new sFilterPair[2];
          AppERecsFilter[0].Set("MON_SYS_ID", AppENOxSysId);
          AppERecsFilter[1].Set("TEST_RESULT_CD", "INVALID", true);
          DataView AppERecsQAStatusFiltered;
          DataRowView PriorRec = null;
          DateTime PriorRecDate = DateTime.MinValue;//because compiler requires initialization
          int PriorRecHour = int.MinValue;//because compiler requires initialization
          if (CurrentHour != 0)
            AppERecsQAStatusFiltered = FindActiveRows(AppERecsQAStatus, DateTime.MinValue, 0, CurrentDate, CurrentHour - 1, "END_DATE", "END_HOUR", "END_DATE", "END_HOUR", AppERecsFilter);
          else
            AppERecsQAStatusFiltered = FindActiveRows(AppERecsQAStatus, DateTime.MinValue, 0, CurrentDate.AddDays(-1), 23, "END_DATE", "END_HOUR", "END_DATE", "END_HOUR", AppERecsFilter);
          if (AppERecsQAStatusFiltered.Count > 0)
          {
            AppERecsQAStatusFiltered.Sort = "END_DATE DESC, END_HOUR DESC, END_MIN DESC";
            PriorRec = AppERecsQAStatusFiltered[0];
            Category.SetCheckParameter("Prior_Appendix_E_Record", PriorRec, eParameterDataType.DataRowView);
            PriorRecDate = cDBConvert.ToDate(PriorRec["END_DATE"], DateTypes.END);
            PriorRecHour = cDBConvert.ToHour(PriorRec["END_HOUR"], DateTypes.END);
            AppERecsFilter[1].Set("TEST_RESULT_CD", "INVALID");
            AppERecsQAStatusFiltered = FindActiveRows(AppERecsQAStatus, PriorRecDate, PriorRecHour, CurrentDate, CurrentHour, "END_DATE", "END_HOUR", "END_DATE", "END_HOUR", AppERecsFilter);//may as well use AppERecsQAStatusFiltered as argument, for performance
            if (AppERecsQAStatusFiltered.Count > 0)
            {
              AppERecsQAStatusFiltered.Sort = "END_DATE DESC, END_HOUR DESC, END_MIN DESC";
              Category.SetCheckParameter("Invalid_Appendix_E_Record", AppERecsQAStatusFiltered[0], eParameterDataType.DataRowView);
            }
          }
          else
          {
            AppERecsFilter[1].Set("TEST_RESULT_CD", "INVALID");
            AppERecsQAStatusFiltered = FindActiveRows(AppERecsQAStatus, DateTime.MinValue, 0, CurrentDate, CurrentHour, "END_DATE", "END_HOUR", "END_DATE", "END_HOUR", AppERecsFilter);
            if (AppERecsQAStatusFiltered.Count > 0)
            {
              AppERecsQAStatusFiltered.Sort = "END_DATE DESC, END_HOUR DESC, END_MIN DESC";
              Category.SetCheckParameter("Invalid_Appendix_E_Record", AppERecsQAStatusFiltered[0], eParameterDataType.DataRowView);
            }
          }
          if (PriorRec != null)
          {
              if (cDBConvert.ToString(PriorRec["QA_NEEDS_EVAL_FLG"]) == "Y")
                  Status = "Prior Test Not Yet Evaluated";
              else
                  if (PriorRec["TEST_RESULT_CD"] == DBNull.Value)
                      Status = "OOC-Prior Test Has Critical Errors";
                  else
                  {
                      DateTime PriorTestExpDate = cDBConvert.ToDate(PriorRec["TEST_EXP_DATE"], DateTypes.END);
                      if (PriorTestExpDate == DateTime.MaxValue)
                          PriorTestExpDate = cDateFunctions.LastDateThisQuarter(PriorRecDate).AddYears(5);
                      if (CurrentDate > PriorTestExpDate)
                          Status = "OOC-Expired";
                      else
                      {
                          DataView QACertEvents = Category.GetCheckParameter("Qa_Certification_Event_Records").ValueAsDataView();
                          sFilterPair[] CertFilter = new sFilterPair[2];
                          CertFilter[0].Set("MON_SYS_ID", AppENOxSysId);
                          CertFilter[1].Set("REQUIRED_TEST_CD", 75, eFilterDataType.Integer);
                          DataView QACertEventsFiltered;
                          if (PriorRecHour != 0)
                              QACertEventsFiltered = FindActiveRows(QACertEvents, PriorRecDate, PriorRecHour - 1, CurrentDate, CurrentHour, "QA_CERT_EVENT_DATE", "QA_CERT_EVENT_HOUR", "QA_CERT_EVENT_DATE", "QA_CERT_EVENT_HOUR", CertFilter);
                          else
                              QACertEventsFiltered = FindActiveRows(QACertEvents, PriorRecDate.AddDays(-1), 23, CurrentDate, CurrentHour, "QA_CERT_EVENT_DATE", "QA_CERT_EVENT_HOUR", "QA_CERT_EVENT_DATE", "QA_CERT_EVENT_HOUR", CertFilter);
                          if (QACertEventsFiltered.Count > 0)
                          {
                              QACertEventsFiltered.Sort = "QA_CERT_EVENT_DATE DESC, QA_CERT_EVENT_HOUR DESC";
                              DataRowView CertRecFound = QACertEventsFiltered[0];
                              Category.SetCheckParameter("Prior_Appendix_E_Event_Record", CertRecFound, eParameterDataType.DataRowView);
                              DateTime PriorCertDate = cDBConvert.ToDate(CertRecFound["QA_CERT_EVENT_DATE"], DateTypes.START);
                              if (cDateFunctions.DateDifference(PriorCertDate.AddDays(-1), CurrentDate) > 180)
                                  Status = "OOC-Event";
                              else
                                  if (CertRecFound["MIN_OP_DAYS_PRIOR_QTR"] == DBNull.Value)
                                  {
                                      int MinOpPriorQtr = 0;
                                      int MaxOpPriorQtr = 0;
                                      OpSuppRecs = Category.GetCheckParameter("Operating_Supp_Data_Records_by_Location").ValueAsDataView();
                                      OpSuppFilter = new sFilterPair[3];
                                      OpSuppFilter[0].Set("OP_TYPE_CD", "OPDAYS");

                                      int PriorCertYear = PriorCertDate.Year;
                                      int PriorCertQtr = cDateFunctions.ThisQuarter(PriorCertDate);
                                      int YearPriorToCurrentQtr = CurrentDate.Year;
                                      int QtrPriorToCurrentQtr = cDateFunctions.ThisQuarter(CurrentDate);
                                      if (QtrPriorToCurrentQtr != 1)
                                          QtrPriorToCurrentQtr--;
                                      else
                                      {
                                          QtrPriorToCurrentQtr = 4;
                                          YearPriorToCurrentQtr--;
                                      }
                                      bool StopLooking = false;
                                      int j;
                                      int OpVal;
                                      DateTime EarliestLocRptDate = Category.GetCheckParameter("Earliest_Location_Report_Date").ValueAsDateTime(DateTypes.START);
                                      DateTime LastDateThisQtr;
                                      for (int i = PriorCertYear; i <= YearPriorToCurrentQtr; i++)
                                      {
                                          j = 1;
                                          if (i == PriorCertYear)
                                              j = PriorCertQtr;
                                          do
                                          {
                                              LastDateThisQtr = cDateFunctions.LastDateThisQuarter(i, j);
                                              if (EarliestLocRptDate <= LastDateThisQtr)
                                              {
                                                  OpSuppFilter[1].Set("CALENDAR_YEAR", i, eFilterDataType.Integer);
                                                  OpSuppFilter[2].Set("QUARTER", j, eFilterDataType.Integer);
                                                  OpSuppRecsFiltered = FindRows(OpSuppRecs, OpSuppFilter);
                                                  if (OpSuppRecsFiltered.Count == 0)
                                                  {
                                                      MinOpPriorQtr = -1;
                                                      Category.SetCheckParameter("Appendix_E_Missing_Op_Data_Info", i + "Q" + j, eParameterDataType.String);
                                                      StopLooking = true;
                                                      break;
                                                  }
                                                  else
                                                  {
                                                      OpVal = cDBConvert.ToInteger(OpSuppRecsFiltered[0]["OP_VALUE"]);
                                                      if (i == PriorCertYear && j == PriorCertQtr)
                                                      {
                                                          int DateDiff = cDateFunctions.DateDifference(cDateFunctions.StartDateThisQuarter(PriorCertDate), PriorCertDate);
                                                          if (OpVal - DateDiff > 0)
                                                              MinOpPriorQtr = DateDiff;
                                                          DateDiff = cDateFunctions.DateDifference(PriorCertDate.AddDays(-1), cDateFunctions.LastDateThisQuarter(PriorCertDate));
                                                          if (OpVal < DateDiff)
                                                              MaxOpPriorQtr = OpVal;
                                                          else
                                                              MaxOpPriorQtr = DateDiff;
                                                      }
                                                      else
                                                      {
                                                          MinOpPriorQtr += OpVal;
                                                          MaxOpPriorQtr += OpVal;
                                                      }
                                                  }
                                              }
                                              j++;
                                          } while (!StopLooking && j <= 4 && !(i == YearPriorToCurrentQtr && j > QtrPriorToCurrentQtr));
                                          if (StopLooking)
                                              break;
                                      }
                                      CertRecFound["MIN_OP_DAYS_PRIOR_QTR"] = MinOpPriorQtr;
                                      CertRecFound["MAX_OP_DAYS_PRIOR_QTR"] = MaxOpPriorQtr;
                                  }
                              if (!Status.StartsWith("OOC"))
                              {
                                  int CurrentPosition = Category.GetCheckParameter("Current_Monitor_Plan_Location_Postion").ValueAsInt();
                                  decimal[] OpTimeAccumArray = Category.GetCheckParameter("Rpt_Period_Op_Time_Accumulator_Array").ValueAsDecimalArray();
                                  int[] OpDaysAccumArray = Category.GetCheckParameter("Rpt_Period_Op_Days_Accumulator_Array").ValueAsIntArray();

                                  if (OpTimeAccumArray[CurrentPosition] == -1)
                                      Status = "Invalid Op Data";
                                  else
                                      if (cDBConvert.ToInteger(CertRecFound["MIN_OP_DAYS_PRIOR_QTR"]) == -1)
                                          Status = "Missing Op Data";
                                      else
                                          if (cDBConvert.ToInteger(CertRecFound["MIN_OP_DAYS_PRIOR_QTR"]) + OpDaysAccumArray[CurrentPosition] > 30)
                                              Status = "OOC-Event";
                                          else
                                              if (cDBConvert.ToInteger(CertRecFound["MAX_OP_DAYS_PRIOR_QTR"]) + OpDaysAccumArray[CurrentPosition] > 30)
                                                  Status = "Undetermined-Event";
                                              else
                                                  Status = "IC";
                              }
                          }
                          else
                              Status = "IC";
                      }
                  }
          }
          else
            if (FuelCd != "MIX")
              {
                  AppERecsFilter[1].Set("TEST_RESULT_CD", "INVALID", true);
                  AppERecsQAStatusFiltered = FindActiveRows(AppERecsQAStatus, CurrentDate, CurrentHour - 1, DateTime.MaxValue, 23, "END_DATE", "END_HOUR", "END_DATE", "END_HOUR", AppERecsFilter);//hour - 1 for inclusivity
                  if (AppERecsQAStatusFiltered.Count > 0)
                  {
                      AppERecsQAStatusFiltered.Sort = "END_DATE ASC, END_HOUR ASC, END_MIN ASC";
                      DataRowView SubsqRec = AppERecsQAStatusFiltered[0];
                      Category.SetCheckParameter("Subsequent_Appendix_E_Record", SubsqRec, eParameterDataType.DataRowView);
                      AppERecsFilter[1].Set("TEST_RESULT_CD", "INVALID");

                      if (CurrentHour != 0)
                          AppERecsQAStatusFiltered = FindActiveRows(AppERecsQAStatus, CurrentDate, CurrentHour - 1, cDBConvert.ToDate(SubsqRec["END_DATE"], DateTypes.END), cDBConvert.ToHour(SubsqRec["END_HOUR"], DateTypes.END), "END_DATE", "END_HOUR", "END_DATE", "END_HOUR", AppERecsFilter);
                      else
                          AppERecsQAStatusFiltered = FindActiveRows(AppERecsQAStatus, CurrentDate.AddDays(-1), 23, cDBConvert.ToDate(SubsqRec["END_DATE"], DateTypes.END), cDBConvert.ToHour(SubsqRec["END_HOUR"], DateTypes.END), "END_DATE", "END_HOUR", "END_DATE", "END_HOUR", AppERecsFilter);

                      if (AppERecsQAStatusFiltered.Count > 0)
                      {
                          AppERecsQAStatusFiltered.Sort = "END_DATE ASC, END_HOUR ASC, END_MIN ASC";
                          Category.SetCheckParameter("Invalid_Appendix_E_Record", AppERecsQAStatusFiltered[0], eParameterDataType.DataRowView);
                      }
                      OpSuppRecs = Category.GetCheckParameter("Operating_Supp_Data_Records_by_Location").ValueAsDataView();
                      OpSuppFilter = new sFilterPair[2];
                      OpSuppFilter[0].Set("FUEL_CD", FuelCd);
                      OpSuppFilter[1].Set("OP_TYPE_CD", "OPHOURS");
                      OpSuppRecsFiltered = FindRows(OpSuppRecs, OpSuppFilter);
                      DateTime DateFirstCombust = DateTime.Now;
                      if (OpSuppRecsFiltered.Count > 0)
                      {
                        OpSuppRecsFiltered.Sort = "CALENDAR_YEAR, QUARTER";
                        DataRowView FuelOpSuppDataRecord = OpSuppRecsFiltered[0];
                        DateFirstCombust = cDateFunctions.LastDateThisQuarter(cDBConvert.ToInteger(FuelOpSuppDataRecord["CALENDAR_YEAR"]), cDBConvert.ToInteger(FuelOpSuppDataRecord["QUARTER"]));
                        DateFirstCombust = DateFirstCombust.AddDays(-(int)(cDBConvert.ToInteger(OpSuppRecsFiltered[0]["OP_VALUE"]) - 1) / 24);
                      }

                      if (OpSuppRecsFiltered.Count > 0 && cDateFunctions.DateDifference(DateFirstCombust, CurrentDate) > 180)
                        Status = "OOC-No Prior Test";
                      else
                      {
                        DataView FuelLookupRecs = Category.GetCheckParameter("Fuel_Code_Lookup_Table").ValueAsDataView();
                        sFilterPair[] Filter3 = new sFilterPair[1];
                        Filter3[0].Set("FUEL_CD", FuelCd);
                        DataView FuelLookupRecsFiltered = FindRows(FuelLookupRecs, Filter3);
                        string UnitFuelCd = "";
                        if (FuelLookupRecsFiltered.Count > 0)
                          UnitFuelCd = cDBConvert.ToString(FuelLookupRecsFiltered[0]["UNIT_FUEL_CD"]);
                        Filter3[0].Set("FUEL_CD", UnitFuelCd);
                        DataView FuelRecs = Category.GetCheckParameter("Fuel_Records_By_Hour_Location").ValueAsDataView();
                        DataView FuelRecsFiltered = FindRows(FuelRecs, Filter3);
                        if (FuelRecsFiltered.Count != 1)
                          Status = "Invalid Location Fuel";
                        else
                          if (cDBConvert.ToString(FuelRecsFiltered[0]["INDICATOR_CD"]) == "S")
                            if (cDBConvert.ToString(SubsqRec["QA_NEEDS_EVAL_FLG"]) == "Y")
                              Status = "Subsequent Test Not Yet Evaluated";
                            else
                              if (SubsqRec["TEST_RESULT_CD"] == DBNull.Value)
                                Status = "OOC-Subsequent Test Has Critical Errors";
                              else
                              {
                                Category.SetCheckParameter("Prior_Appendix_E_Record", SubsqRec, eParameterDataType.DataRowView);
                                Status = "IC";
                              }
                          else
                            Status = "OOC-No Prior Test";
                      }
                  }
                  else
                  {
                      Status = "OOC-No Prior Test";
                      AppERecsFilter[1].Set("TEST_RESULT_CD", "INVALID");
                      AppERecsQAStatusFiltered = FindActiveRows(AppERecsQAStatus, CurrentDate, CurrentHour - 1, DateTime.MaxValue, 23, "END_DATE", "END_HOUR", "END_DATE", "END_HOUR", AppERecsFilter);//hour - 1 for inclusivity
                      if (AppERecsQAStatusFiltered.Count > 0)
                      {
                          AppERecsQAStatusFiltered.Sort = "END_DATE ASC, END_HOUR ASC, END_MIN ASC";
                          Category.SetCheckParameter("Invalid_Appendix_E_Record", AppERecsQAStatusFiltered[0], eParameterDataType.DataRowView);
                      }
                  }
              }
              else
                  Status = "OOC-No Prior Test";
          if (Status.StartsWith("OOC"))
            if (Category.GetCheckParameter("Invalid_Appendix_E_Record").ParameterValue != null)
              Status += "*";
          if (Status != "")
            Category.SetCheckParameter("Current_Appendix_E_Status", Status, eParameterDataType.String);
          else
            Category.SetCheckParameter("Current_Appendix_E_Status", null, eParameterDataType.DataRowView);
          if (!Status.StartsWith("IC"))
            Category.CheckCatalogResult = Status;
        }
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "ADESTAT1");
      }

      return ReturnVal;
    }

    public string ADESTAT2(cCategory Category, ref bool Log)
    //Locate Most Recent Prior Accuracy Test       
    {
      string ReturnVal = "";

      try
      {
        string Status = "";
        Category.SetCheckParameter("Invalid_Accuracy_Record", null, eParameterDataType.DataRowView);
        Category.SetCheckParameter("Prior_Accuracy_Record", null, eParameterDataType.DataRowView);
        Category.SetCheckParameter("Inappropriate_Transmitter_Transducer_Test", false, eParameterDataType.Boolean);

        DataView AccTestRecs = Category.GetCheckParameter("Accuracy_Test_Records_By_Location_For_QA_Status").ValueAsDataView();
        DataRowView FFCompRecToCheck = Category.GetCheckParameter("Fuel_Flow_Component_Record_to_Check").ValueAsDataRowView();
        string CompID = cDBConvert.ToString(FFCompRecToCheck["COMPONENT_ID"]);
        DateTime CurrentDate = Category.GetCheckParameter("Current_Operating_Date").ValueAsDateTime(DateTypes.END);
        int CurrentHour = Category.GetCheckParameter("Current_Operating_Hour").ValueAsInt();
        sFilterPair[] FilterAccTest;
        string AcqCd = cDBConvert.ToString(FFCompRecToCheck["ACQ_CD"]);
        FilterAccTest = new sFilterPair[2];
        FilterAccTest[0].Set("TEST_RESULT_CD", "INVALID", true);
        FilterAccTest[1].Set("COMPONENT_ID", CompID);
        DataView AccTestRecsFound;
        DataRowView PriorAccTestRec;
        if (CurrentHour > 0)
          AccTestRecsFound = FindActiveRows(AccTestRecs, DateTime.MinValue, 0, CurrentDate, CurrentHour - 1, "END_DATE", "END_HOUR", "END_DATE", "END_HOUR", FilterAccTest);
        else
          AccTestRecsFound = FindActiveRows(AccTestRecs, DateTime.MinValue, 0, CurrentDate.AddDays(-1), 23, "END_DATE", "END_HOUR", "END_DATE", "END_HOUR", FilterAccTest);
        if (AccTestRecsFound.Count > 0)      
        {
          AccTestRecsFound.Sort = "END_DATE DESC, END_HOUR DESC, END_MIN DESC";
          PriorAccTestRec = AccTestRecsFound[0];
          Category.SetCheckParameter("Prior_Accuracy_Record", PriorAccTestRec, eParameterDataType.DataRowView);

          if ((string)PriorAccTestRec["TEST_TYPE_CD"] == "FFACCTT" && !AcqCd.InList("ORF,NOZ,VEN"))
            Category.SetCheckParameter("Inappropriate_Transmitter_Transducer_Test", true, eParameterDataType.Boolean);

          FilterAccTest = new sFilterPair[2];
          FilterAccTest[0].Set("TEST_RESULT_CD", "INVALID");
          FilterAccTest[1].Set("COMPONENT_ID", CompID);
          DateTime PriorEndDate = cDBConvert.ToDate(PriorAccTestRec["END_DATE"], DateTypes.END);
          int PriorEndHour = cDBConvert.ToInteger(PriorAccTestRec["END_HOUR"]);
          if (PriorEndHour < 23)
            if (CurrentHour > 0)
              AccTestRecsFound = FindActiveRows(AccTestRecs, PriorEndDate, PriorEndHour + 1, CurrentDate, CurrentHour - 1, "END_DATE", "END_HOUR", "END_DATE", "END_HOUR", FilterAccTest);
            else
              AccTestRecsFound = FindActiveRows(AccTestRecs, PriorEndDate, PriorEndHour + 1, CurrentDate.AddDays(-1), 23, "END_DATE", "END_HOUR", "END_DATE", "END_HOUR", FilterAccTest);
          else
            if (CurrentHour > 0)
              AccTestRecsFound = FindActiveRows(AccTestRecs, PriorEndDate.AddDays(1), 0, CurrentDate, CurrentHour - 1, "END_DATE", "END_HOUR", "END_DATE", "END_HOUR", FilterAccTest);
            else
              AccTestRecsFound = FindActiveRows(AccTestRecs, PriorEndDate.AddDays(1), 0, CurrentDate.AddDays(-1), 23, "END_DATE", "END_HOUR", "END_DATE", "END_HOUR", FilterAccTest);
          if (AccTestRecsFound.Count > 0)
            Category.SetCheckParameter("Invalid_Accuracy_Record", AccTestRecsFound[0], eParameterDataType.DataRowView);
          if (cDBConvert.ToString(PriorAccTestRec["QA_NEEDS_EVAL_FLG"]) == "Y")
            Status = "Accuracy Test Not Yet Evaluated";
          else
            switch (cDBConvert.ToString(PriorAccTestRec["TEST_RESULT_CD"]))
            {
              case "":
                Status = "OOC-Accuracy Test Has Critical Errors";
                break;
              case "FAILED":
                Status = "OOC-Accuracy Test Failed";
                break;
              case "ABORTED":
                Status = "OOC-Accuracy Test Aborted";
                break;
              default:
                break;
            }
        }
        else
        {
          Status = "OOC-No Prior Accuracy Test";
          FilterAccTest = new sFilterPair[2];
          FilterAccTest[0].Set("TEST_RESULT_CD", "INVALID");
          FilterAccTest[1].Set("COMPONENT_ID", CompID);
          if (CurrentHour > 0)
            AccTestRecsFound = FindActiveRows(AccTestRecs, DateTime.MinValue, 0, CurrentDate, CurrentHour - 1, "END_DATE", "END_HOUR", "END_DATE", "END_HOUR", FilterAccTest);
          else
            AccTestRecsFound = FindActiveRows(AccTestRecs, DateTime.MinValue, 0, CurrentDate.AddDays(-1), 23, "END_DATE", "END_HOUR", "END_DATE", "END_HOUR", FilterAccTest);
          if (AccTestRecsFound.Count > 0)
            Category.SetCheckParameter("Invalid_Accuracy_Record", AccTestRecsFound[0], eParameterDataType.DataRowView);
        }
        if (Status != "")
          Category.SetCheckParameter("Current_Accuracy_Status", Status, eParameterDataType.String);
        else
          Category.SetCheckParameter("Current_Accuracy_Status", null, eParameterDataType.String);
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "ADESTAT2");
      }

      return ReturnVal;
    }

    public string ADESTAT3(cCategory Category, ref bool Log)
    //Locate Most Recent Prior Accuracy Event       
    {
      string ReturnVal = "";

      try
      {
        Category.SetCheckParameter("Prior_Accuracy_Event_Record", null, eParameterDataType.DataRowView);
        string Status = Category.GetCheckParameter("Current_Accuracy_Status").ValueAsString();
        if (Status == "")
        {
          DataView QACertRecs = Category.GetCheckParameter("Qa_Certification_Event_Records").ValueAsDataView();
          DataView QACertRecsFound;
          DateTime CurrentDate = Category.GetCheckParameter("Current_Operating_Date").ValueAsDateTime(DateTypes.END);
          int CurrentHour = Category.GetCheckParameter("Current_Operating_Hour").ValueAsInt();
          DataRowView PriorAccTestRec = Category.GetCheckParameter("Prior_Accuracy_Record").ValueAsDataRowView();
          DateTime PriorEndDate = cDBConvert.ToDate(PriorAccTestRec["END_DATE"], DateTypes.END);
          int PriorEndHour = cDBConvert.ToInteger(PriorAccTestRec["END_HOUR"]);
          DateTime ReinstallDate = cDBConvert.ToDate(PriorAccTestRec["REINSTALL_DATE"], DateTypes.START);
          int ReinstallHour = cDBConvert.ToHour(PriorAccTestRec["REINSTALL_HOUR"], DateTypes.START);
          DataRowView FFCompRecToCheck = Category.GetCheckParameter("Fuel_Flow_Component_Record_to_Check").ValueAsDataRowView();
          string CompID = cDBConvert.ToString(FFCompRecToCheck["COMPONENT_ID"]);
          sFilterPair[] CertFilter = new sFilterPair[2];
          CertFilter[0].Set("COMPONENT_ID", CompID);
          CertFilter[1].Set("FFACC_REQUIRED", "Y");
          if (PriorEndDate > ReinstallDate || (PriorEndDate == ReinstallDate && PriorEndHour > ReinstallHour))
          {
            if (PriorEndHour < 23)
              if (CurrentHour > 0)
                QACertRecsFound = FindActiveRows(QACertRecs, PriorEndDate, PriorEndHour + 1, CurrentDate, CurrentHour - 1, "QA_CERT_EVENT_DATE", "QA_CERT_EVENT_HOUR", "QA_CERT_EVENT_DATE", "QA_CERT_EVENT_HOUR", CertFilter);
              else
                QACertRecsFound = FindActiveRows(QACertRecs, PriorEndDate, PriorEndHour + 1, CurrentDate.AddDays(-1), 23, "QA_CERT_EVENT_DATE", "QA_CERT_EVENT_HOUR", "QA_CERT_EVENT_DATE", "QA_CERT_EVENT_HOUR", CertFilter);
            else
              if (CurrentHour > 0)
                QACertRecsFound = FindActiveRows(QACertRecs, PriorEndDate.AddDays(1), 0, CurrentDate, CurrentHour - 1, "QA_CERT_EVENT_DATE", "QA_CERT_EVENT_HOUR", "QA_CERT_EVENT_DATE", "QA_CERT_EVENT_HOUR", CertFilter);
              else
                QACertRecsFound = FindActiveRows(QACertRecs, PriorEndDate.AddDays(1), 0, CurrentDate.AddDays(-1), 23, "QA_CERT_EVENT_DATE", "QA_CERT_EVENT_HOUR", "QA_CERT_EVENT_DATE", "QA_CERT_EVENT_HOUR", CertFilter);
          }
          else
          {
            if (ReinstallHour < 23)
              if (CurrentHour > 0)
                QACertRecsFound = FindActiveRows(QACertRecs, ReinstallDate, ReinstallHour + 1, CurrentDate, CurrentHour - 1, "QA_CERT_EVENT_DATE", "QA_CERT_EVENT_HOUR", "QA_CERT_EVENT_DATE", "QA_CERT_EVENT_HOUR", CertFilter);
              else
                QACertRecsFound = FindActiveRows(QACertRecs, ReinstallDate, ReinstallHour + 1, CurrentDate.AddDays(-1), 23, "QA_CERT_EVENT_DATE", "QA_CERT_EVENT_HOUR", "QA_CERT_EVENT_DATE", "QA_CERT_EVENT_HOUR", CertFilter);
            else
              if (CurrentHour > 0)
                QACertRecsFound = FindActiveRows(QACertRecs, ReinstallDate.AddDays(1), 0, CurrentDate, CurrentHour - 1, "QA_CERT_EVENT_DATE", "QA_CERT_EVENT_HOUR", "QA_CERT_EVENT_DATE", "QA_CERT_EVENT_HOUR", CertFilter);
              else
                QACertRecsFound = FindActiveRows(QACertRecs, ReinstallDate.AddDays(1), 0, CurrentDate.AddDays(-1), 23, "QA_CERT_EVENT_DATE", "QA_CERT_EVENT_HOUR", "QA_CERT_EVENT_DATE", "QA_CERT_EVENT_HOUR", CertFilter);
          }
          if (QACertRecsFound.Count > 0)
          {
            QACertRecsFound.Sort = "QA_CERT_EVENT_DATE DESC, QA_CERT_EVENT_HOUR DESC";
            Category.SetCheckParameter("Prior_Accuracy_Event_Record", QACertRecsFound[0], eParameterDataType.DataRowView);
            Status = "OOC-Event";
          }
          else
          {
            DateTime PriorTestExpDate = cDateFunctions.LastDateThisQuarter(PriorEndDate).AddYears(5);
            if (CurrentDate > PriorTestExpDate)
              Status = "OOC-Accuracy Test Expired";
            else
            {
              if (PriorEndDate > ReinstallDate || (PriorEndDate == ReinstallDate && PriorEndHour > ReinstallHour))
                PriorTestExpDate = cDateFunctions.LastDateThisQuarter(PriorEndDate).AddYears(1);
              else
                PriorTestExpDate = cDateFunctions.LastDateThisQuarter(ReinstallDate).AddYears(1);
              PriorAccTestRec["TEST_EXP_DATE"] = PriorTestExpDate;
              if (CurrentDate <= PriorTestExpDate)
                Status = "IC";
            }
          }
          if (Status != "")
            Category.SetCheckParameter("Current_Accuracy_Status", Status, eParameterDataType.String);
        }
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "ADESTAT3");
      }

      return ReturnVal;
    }

    public string ADESTAT4(cCategory Category, ref bool Log)
    //Determine Eligibility for Fuel Flow to Load Testing (Accuracy)       
    {
      string ReturnVal = "";

      try
      {
        Category.SetCheckParameter("FF2L_Accuracy_Eligible", null, eParameterDataType.Boolean);
        Category.SetCheckParameter("FF2L_Accuracy_Check_Date", null, eParameterDataType.Date);

        if (Category.GetCheckParameter("Current_Accuracy_Status").ParameterValue == null)
        {
          DataView FF2LTestRecs = Category.GetCheckParameter("FF2L_Test_Records_By_Location_For_QA_Status").ValueAsDataView();
          DataRowView FFCompRecToCheck = Category.GetCheckParameter("Fuel_Flow_Component_Record_to_Check").ValueAsDataRowView();
          string MonSysID = cDBConvert.ToString(FFCompRecToCheck["MON_SYS_ID"]);

          DateTime CurrentDate = Category.GetCheckParameter("Current_Operating_Date").ValueAsDateTime(DateTypes.END);
          DataRowView PriorAccTestRec = Category.GetCheckParameter("Prior_Accuracy_Record").ValueAsDataRowView();
          DateTime PriorAccRecEndDate = cDBConvert.ToDate(PriorAccTestRec["END_DATE"], DateTypes.START);
          DateTime ReinstallDate = cDBConvert.ToDate(PriorAccTestRec["REINSTALL_DATE"], DateTypes.START);
          DateTime FirstDate = PriorAccRecEndDate;
          if (ReinstallDate > PriorAccRecEndDate)
            FirstDate = ReinstallDate;
          int FirstYear = FirstDate.Year;
          int FirstQuarter = cDateFunctions.ThisQuarter(FirstDate);
          int SecondYear = CurrentDate.Year;
          int SecondQuarter = cDateFunctions.ThisQuarter(CurrentDate);
          DateTime CheckDate = DateTime.MinValue;

          sFilterPair[] Filter = new sFilterPair[2];
          Filter[0].Set("MON_SYS_ID", MonSysID);
          Filter[1].Set("TEST_RESULT_CD", "PASSED,FAILED,EXC168H,INPROG", eFilterPairStringCompare.InList);
          DataView FF2LTestRecsFound = FindRows(FF2LTestRecs, Filter);
          int thisYear;
          int thisQuarter;
          int PriorAccRecYear = PriorAccRecEndDate.Year;
          int PriorAccRecQuarter = cDateFunctions.ThisQuarter(PriorAccRecEndDate);
          bool FoundFF2LRec = false;
          foreach (DataRowView drv in FF2LTestRecsFound)
          {
            thisYear = cDBConvert.ToInteger(drv["CALENDAR_YEAR"]);
            thisQuarter = cDBConvert.ToInteger(drv["QUARTER"]);
            if (thisYear < SecondYear || (thisYear == SecondYear && thisQuarter < SecondQuarter))
              if (thisYear > FirstYear || (thisYear == FirstYear && thisQuarter > FirstQuarter))
              {
                FoundFF2LRec = true;
                break;
              }
          }
          if (FoundFF2LRec)
          {
            bool ValidFuelFlowTestExistsForEachComponent  = true;
            DateTime CertificationCheckDate;
            CheckExistenceOfValidFuelFlowTest(Category, out ValidFuelFlowTestExistsForEachComponent , out CertificationCheckDate);

            if (ValidFuelFlowTestExistsForEachComponent  && CertificationCheckDate != null)
            {
              Category.SetCheckParameter("FF2L_Accuracy_Eligible", true, eParameterDataType.Boolean);
              Category.SetCheckParameter("FF2L_Accuracy_Check_Date", CertificationCheckDate, eParameterDataType.Date);
            }
            else
            {
              Category.SetCheckParameter("FF2L_Accuracy_Eligible", true, eParameterDataType.Boolean);
              Category.SetCheckParameter("FF2L_Accuracy_Check_Date", FirstDate, eParameterDataType.Date);
              DataView FFCompRecs = Category.GetCheckParameter("Fuel_Flow_Component_Records").ValueAsDataView();
              string RecToCheckCompID = cDBConvert.ToString(FFCompRecToCheck["COMPONENT_ID"]);
              string CompID;
              bool Found;
              DataView AccTestRecs = Category.GetCheckParameter("Accuracy_Test_Records_By_Location_For_QA_Status").ValueAsDataView();
              DataView AccTestRecsFound;
              sFilterPair[] FilterAccTest = new sFilterPair[2];
              FilterAccTest[0].Set("TEST_RESULT_CD", "PASSED");
              DataView PEITestRecs = Category.GetCheckParameter("PEI_Test_Records_By_Location_For_QA_Status").ValueAsDataView();
              DataView PEITestRecsFound;
              sFilterPair[] FilterPEITest = new sFilterPair[2];
              FilterPEITest[0].Set("TEST_RESULT_CD", "PASSED");
              foreach (DataRowView drv in FFCompRecs)
              {
                CompID = cDBConvert.ToString(drv["COMPONENT_ID"]);
                if (CompID != RecToCheckCompID)
                {
                  FilterAccTest[1].Set("COMPONENT_ID", CompID);
                  AccTestRecsFound = FindRows(AccTestRecs, FilterAccTest);
                  PriorAccRecYear = PriorAccRecEndDate.Year;
                  PriorAccRecQuarter = cDateFunctions.ThisQuarter(PriorAccRecEndDate);
                  string thisTestType = "";
                  Found = false;
                  CheckDate = DateTime.MinValue;
                  foreach (DataRowView drv2 in AccTestRecsFound)
                  {
                    thisYear = cDBConvert.ToInteger(drv2["CALENDAR_YEAR"]);
                    thisQuarter = cDBConvert.ToInteger(drv2["QUARTER"]);
                    if (cDBConvert.ToDate(drv2["REINSTALL_DATE"], DateTypes.START) > cDBConvert.ToDate(drv2["END_DATE"], DateTypes.START))
                    {
                      thisYear = cDBConvert.ToDate(drv2["REINSTALL_DATE"], DateTypes.START).Year;
                      thisQuarter = cDateFunctions.ThisQuarter(cDBConvert.ToDate(drv2["REINSTALL_DATE"], DateTypes.START));
                    }
                    if (thisYear == FirstYear && (thisQuarter == FirstQuarter || thisQuarter == FirstQuarter + 1 || thisQuarter == FirstQuarter - 1) ||
                        (thisYear == FirstYear + 1 && (thisQuarter == 1 && FirstQuarter == 4)) ||
                        (thisYear == FirstYear - 1 && (thisQuarter == 4 && FirstQuarter == 1)))
                    {
                      Found = true;
                      if (cDBConvert.ToDate(drv2["END_DATE"], DateTypes.START) > CheckDate)
                      {
                        CheckDate = cDBConvert.ToDate(drv2["END_DATE"], DateTypes.START);
                        thisTestType = cDBConvert.ToString(drv2["TEST_TYPE_CD"]);
                      }
                      if (cDBConvert.ToDate(drv2["REINSTALL_DATE"], DateTypes.START) > CheckDate)
                      {
                        CheckDate = cDBConvert.ToDate(drv2["REINSTALL_DATE"], DateTypes.START);
                        thisTestType = cDBConvert.ToString(drv2["TEST_TYPE_CD"]);
                      }
                    }
                  }
                  if (!Found)
                  {
                    Category.SetCheckParameter("FF2L_Accuracy_Eligible", false, eParameterDataType.Boolean);
                    return ReturnVal;
                  }
                  else
                  {
                    if (CheckDate > Category.GetCheckParameter("FF2L_Accuracy_Check_Date").ValueAsDateTime(DateTypes.START))
                      Category.SetCheckParameter("FF2L_Accuracy_Check_Date", CheckDate, eParameterDataType.Date);

                    if (thisTestType == "FFACCTT")
                    {
                      FilterPEITest[1].Set("COMPONENT_ID", CompID);
                      PEITestRecsFound = FindRows(PEITestRecs, FilterPEITest);
                      Found = false;
                      CheckDate = DateTime.MinValue;
                      foreach (DataRowView drv2 in PEITestRecsFound)
                      {
                        thisYear = cDBConvert.ToInteger(drv2["CALENDAR_YEAR"]);
                        thisQuarter = cDBConvert.ToInteger(drv2["QUARTER"]);
                        if (thisYear == FirstYear && (thisQuarter == FirstQuarter || thisQuarter == FirstQuarter + 1 || thisQuarter == FirstQuarter - 1) ||
                            (thisYear == FirstYear + 1 && (thisQuarter == 1 && FirstQuarter == 4)) ||
                            (thisYear == FirstYear - 1 && (thisQuarter == 4 && FirstQuarter == 1)))
                        {
                          Found = true;
                          if (cDBConvert.ToDate(drv2["END_DATE"], DateTypes.START) > CheckDate)
                            CheckDate = cDBConvert.ToDate(drv2["END_DATE"], DateTypes.START);
                        }
                      }
                      if (!Found)
                      {
                        Category.SetCheckParameter("FF2L_Accuracy_Eligible", false, eParameterDataType.Boolean);
                        return ReturnVal;
                      }
                      else
                      {
                        if (CheckDate > Category.GetCheckParameter("FF2L_Accuracy_Check_Date").ValueAsDateTime(DateTypes.START))
                          Category.SetCheckParameter("FF2L_Accuracy_Check_Date", CheckDate, eParameterDataType.Date);
                      }
                    }
                  }
                }
                else
                {
                  if (cDBConvert.ToString(PriorAccTestRec["TEST_TYPE_CD"]) == "FFACCTT")
                  {
                    FilterPEITest[1].Set("COMPONENT_ID", CompID);
                    PEITestRecsFound = FindRows(PEITestRecs, FilterPEITest);
                    Found = false;
                    CheckDate = DateTime.MinValue;
                    foreach (DataRowView drv2 in PEITestRecsFound)
                    {
                      thisYear = cDBConvert.ToInteger(drv2["CALENDAR_YEAR"]);
                      thisQuarter = cDBConvert.ToInteger(drv2["QUARTER"]);
                      if (thisYear == PriorAccRecYear && (thisQuarter == PriorAccRecQuarter || thisQuarter == PriorAccRecQuarter + 1 || thisQuarter == PriorAccRecQuarter - 1) ||
                          (thisYear == PriorAccRecYear + 1 && (thisQuarter == 1 && PriorAccRecQuarter == 4)) ||
                          (thisYear == PriorAccRecYear - 1 && (thisQuarter == 4 && PriorAccRecQuarter == 1)))
                      {
                        Found = true;
                        if (cDBConvert.ToDate(drv2["END_DATE"], DateTypes.START) > CheckDate)
                          CheckDate = cDBConvert.ToDate(drv2["END_DATE"], DateTypes.START);
                      }
                    }
                    if (!Found)
                    {
                      Category.SetCheckParameter("FF2L_Accuracy_Eligible", false, eParameterDataType.Boolean);
                      return ReturnVal;
                    }
                    else
                    {
                      if (CheckDate > Category.GetCheckParameter("FF2L_Accuracy_Check_Date").ValueAsDateTime(DateTypes.START))
                        Category.SetCheckParameter("FF2L_Accuracy_Check_Date", CheckDate, eParameterDataType.Date);
                    }
                  }
                }
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "ADESTAT4");
      }

      return ReturnVal;
    }

    public string ADESTAT5(cCategory Category, ref bool Log)
    //Evaluate Fuel Flow to Load Tests (Accuracy)       
    {
      string ReturnVal = "";

      try
      {
        Category.SetCheckParameter("FF2L_Accuracy_Begin_Year_Quarter", null, eParameterDataType.String);
        Category.SetCheckParameter("FF2L_Accuracy_End_Year_Quarter", null, eParameterDataType.String);
        Category.SetCheckParameter("Invalid_FF2L_Test_Number", null, eParameterDataType.String);
        Category.SetCheckParameter("Missing_FF2L_Year_Quarter", null, eParameterDataType.String);
        DateTime CheckDate = Category.GetCheckParameter("FF2L_Accuracy_Check_Date").ValueAsDateTime(DateTypes.END);
        if (Category.GetCheckParameter("FF2L_Accuracy_Eligible").ValueAsBool())
        {
          DataView FF2LTestRecs = Category.GetCheckParameter("FF2L_Test_Records_By_Location_For_QA_Status").ValueAsDataView();
          DataRowView FFCompRecToCheck = Category.GetCheckParameter("Fuel_Flow_Component_Record_to_Check").ValueAsDataRowView();
          string MonSysID = cDBConvert.ToString(FFCompRecToCheck["MON_SYS_ID"]);
          DateTime CurrentDate = Category.GetCheckParameter("Current_Operating_Date").ValueAsDateTime(DateTypes.END);
          DataRowView PriorAccTestRec = Category.GetCheckParameter("Prior_Accuracy_Record").ValueAsDataRowView();
          DateTime PriorAccRecEndDate = cDBConvert.ToDate(PriorAccTestRec["END_DATE"], DateTypes.START);
          DateTime ReinstallDate = cDBConvert.ToDate(PriorAccTestRec["REINSTALL_DATE"], DateTypes.START);
          DateTime FirstDate = PriorAccRecEndDate;
          if (ReinstallDate > PriorAccRecEndDate)
            FirstDate = ReinstallDate;
          int FirstYear = FirstDate.Year;
          int FirstQuarter = cDateFunctions.ThisQuarter(FirstDate);
          int SecondYear = CurrentDate.Year;
          int SecondQuarter = cDateFunctions.ThisQuarter(CurrentDate);

          sFilterPair[] FilterFF2LRecs = new sFilterPair[2];
          FilterFF2LRecs[0].Set("MON_SYS_ID", MonSysID);
          FilterFF2LRecs[1].Set("TEST_RESULT_CD", "FAILED");
          DataView FF2LTestRecsFound = FindRows(FF2LTestRecs, FilterFF2LRecs);
          int thisYear;
          int thisQuarter;
          bool FoundFF2LRec = false;
          string FoundTestNum = "";//compiler needs initialization
          foreach (DataRowView drv in FF2LTestRecsFound)
          {
            thisYear = cDBConvert.ToInteger(drv["CALENDAR_YEAR"]);
            thisQuarter = cDBConvert.ToInteger(drv["QUARTER"]);
            if (thisYear < SecondYear || (thisYear == SecondYear && thisQuarter < SecondQuarter))
              if (thisYear > FirstYear || (thisYear == FirstYear && thisQuarter > FirstQuarter))
              {
                FoundFF2LRec = true;
                FoundTestNum = cDBConvert.ToString(drv["TEST_NUM"]);
                break;
              }
          }
          if (FoundFF2LRec)
          {
            Category.SetCheckParameter("Invalid_FF2L_Test_Number", FoundTestNum, eParameterDataType.String);
            Category.SetCheckParameter("Current_Accuracy_Status", "OOC-Fuel Flow to Load Test Failed", eParameterDataType.String);
            return ReturnVal;
          }
          FilterFF2LRecs[1].Set("TEST_RESULT_CD", DBNull.Value, eFilterDataType.String);
          FF2LTestRecsFound = FindRows(FF2LTestRecs, FilterFF2LRecs);
          FoundFF2LRec = false;
          FoundTestNum = "";
          foreach (DataRowView drv in FF2LTestRecsFound)
          {
            thisYear = cDBConvert.ToInteger(drv["CALENDAR_YEAR"]);
            thisQuarter = cDBConvert.ToInteger(drv["QUARTER"]);
            if (thisYear < SecondYear || (thisYear == SecondYear && thisQuarter < SecondQuarter))
              if (thisYear > FirstYear || (thisYear == FirstYear && thisQuarter > FirstQuarter))
              {
                FoundFF2LRec = true;
                FoundTestNum = cDBConvert.ToString(drv["TEST_NUM"]);
                break;
              }
          }
          if (FoundFF2LRec)
          {
            Category.SetCheckParameter("Invalid_FF2L_Test_Number", FoundTestNum, eParameterDataType.String);
            Category.SetCheckParameter("Current_Accuracy_Status", "OOC-Fuel Flow to Load Test Has Critical Errors", eParameterDataType.String);
            return ReturnVal;
          }
          FilterFF2LRecs[1].Set("QA_NEEDS_EVAL_FLG", "Y");
          FF2LTestRecsFound = FindRows(FF2LTestRecs, FilterFF2LRecs);
          FoundFF2LRec = false;
          FoundTestNum = "";
          foreach (DataRowView drv in FF2LTestRecsFound)
          {
            thisYear = cDBConvert.ToInteger(drv["CALENDAR_YEAR"]);
            thisQuarter = cDBConvert.ToInteger(drv["QUARTER"]);
            if (thisYear < SecondYear || (thisYear == SecondYear && thisQuarter < SecondQuarter))
              if (thisYear > FirstYear || (thisYear == FirstYear && thisQuarter > FirstQuarter))
              {
                FoundFF2LRec = true;
                FoundTestNum = cDBConvert.ToString(drv["TEST_NUM"]);
                break;
              }
          }
          if (FoundFF2LRec)
          {
            Category.SetCheckParameter("Invalid_FF2L_Test_Number", FoundTestNum, eParameterDataType.String);
            Category.SetCheckParameter("Current_Accuracy_Status", "Fuel Flow to Load Test Has Not Yet Been Evaluated", eParameterDataType.String);
            return ReturnVal;
          }
          FilterFF2LRecs[1].Set("TEST_RESULT_CD", "PASSED,FEW168H,EXC168H", eFilterPairStringCompare.InList);
          FF2LTestRecsFound = FindRows(FF2LTestRecs, FilterFF2LRecs);
          DataRowView thisFF2LRecFound = null;
          FoundFF2LRec = false;
          foreach (DataRowView drv in FF2LTestRecsFound)
          {
            thisYear = cDBConvert.ToInteger(drv["CALENDAR_YEAR"]);
            thisQuarter = cDBConvert.ToInteger(drv["QUARTER"]);
            if (thisYear < SecondYear || (thisYear == SecondYear && thisQuarter < SecondQuarter))
              if (thisYear > FirstYear || (thisYear == FirstYear && thisQuarter > FirstQuarter))
              {
                if (!FoundFF2LRec)//if this is the first one found
                {
                  thisFF2LRecFound = drv;
                  FoundFF2LRec = true;
                }
                else
                  if (thisYear < cDBConvert.ToInteger(thisFF2LRecFound["CALENDAR_YEAR"]) || (thisYear == cDBConvert.ToInteger(thisFF2LRecFound["CALENDAR_YEAR"]) && thisQuarter < cDBConvert.ToInteger(thisFF2LRecFound["QUARTER"])))
                    thisFF2LRecFound = drv;//take the earlier of the two                                
              }
          }
          int BegYear = 0, BegQtr = 0;
          if (FoundFF2LRec)
          {
            BegYear = cDBConvert.ToInteger(thisFF2LRecFound["CALENDAR_YEAR"]);
            BegQtr = cDBConvert.ToInteger(thisFF2LRecFound["QUARTER"]);
            Category.SetCheckParameter("FF2L_Accuracy_Begin_Year_Quarter", BegYear.ToString() + BegQtr.ToString(), eParameterDataType.String);
          }
          FilterFF2LRecs[1].Set("TEST_RESULT_CD", "INPROG");
          FF2LTestRecsFound = FindRows(FF2LTestRecs, FilterFF2LRecs);
          thisFF2LRecFound = null;
          FoundFF2LRec = false;
          foreach (DataRowView drv in FF2LTestRecsFound)
          {
            thisYear = cDBConvert.ToInteger(drv["CALENDAR_YEAR"]);
            thisQuarter = cDBConvert.ToInteger(drv["QUARTER"]);
            if (thisYear < SecondYear || (thisYear == SecondYear && thisQuarter < SecondQuarter))
              if (thisYear > FirstYear || (thisYear == FirstYear && thisQuarter > FirstQuarter))
              {
                if (!FoundFF2LRec)//if this is the first one found
                {
                  thisFF2LRecFound = drv;
                  FoundFF2LRec = true;
                }
                else
                  if (thisYear > cDBConvert.ToInteger(thisFF2LRecFound["CALENDAR_YEAR"]) || (thisYear == cDBConvert.ToInteger(thisFF2LRecFound["CALENDAR_YEAR"]) && thisQuarter > cDBConvert.ToInteger(thisFF2LRecFound["QUARTER"])))
                    thisFF2LRecFound = drv;//take the later of the two                                
              }
          }
          if (FoundFF2LRec)
          {
            thisYear = cDBConvert.ToInteger(thisFF2LRecFound["CALENDAR_YEAR"]);
            thisQuarter = cDBConvert.ToInteger(thisFF2LRecFound["QUARTER"]);
            if (BegYear > 0 && (thisYear > BegYear || (thisYear == BegYear && thisQuarter > BegQtr)))
            {
              FoundTestNum = cDBConvert.ToString(thisFF2LRecFound["TEST_NUM"]);
              Category.SetCheckParameter("Invalid_FF2L_Test_Number", FoundTestNum, eParameterDataType.String);
              Category.SetCheckParameter("Current_Accuracy_Status", "OOC-Invalid Fuel Flow to Load Test", eParameterDataType.String);
              return ReturnVal;
            }
            else
            {
              if (thisYear > CheckDate.Year + 1 || (thisYear == CheckDate.Year + 1 && thisQuarter > cDateFunctions.ThisQuarter(CheckDate)))
              {
                FoundTestNum = cDBConvert.ToString(thisFF2LRecFound["TEST_NUM"]);
                Category.SetCheckParameter("Invalid_FF2L_Test_Number", FoundTestNum, eParameterDataType.String);
                Category.SetCheckParameter("Current_Accuracy_Status", "Undetermined-Baseline Period Expired", eParameterDataType.String);
                return ReturnVal;
              }
            }
          }
          BegYear = CheckDate.Year;
          BegQtr = cDateFunctions.ThisQuarter(CheckDate) + 1;
          if (BegQtr == 5)
          {
            BegYear = FirstYear + 1;
            BegQtr = 1;
          } 
          Category.SetCheckParameter("FF2L_Accuracy_Begin_Year_Quarter", BegYear.ToString() + BegQtr.ToString(), eParameterDataType.String);
          FilterFF2LRecs[1].Set("TEST_RESULT_CD", "PASSED,FEW168H,EXC168H,INPROG", eFilterPairStringCompare.InList);
          FF2LTestRecsFound = FindRows(FF2LTestRecs, FilterFF2LRecs);
          thisFF2LRecFound = null;
          FoundFF2LRec = false;
          foreach (DataRowView drv in FF2LTestRecsFound)
          {
            thisYear = cDBConvert.ToInteger(drv["CALENDAR_YEAR"]);
            thisQuarter = cDBConvert.ToInteger(drv["QUARTER"]);
            if (thisYear < SecondYear || (thisYear == SecondYear && thisQuarter < SecondQuarter))
              if (thisYear > FirstYear || (thisYear == FirstYear && thisQuarter > FirstQuarter))
              {
                if (!FoundFF2LRec)//if this is the first one found
                  thisFF2LRecFound = drv;
                FoundFF2LRec = true;
                if (thisYear > cDBConvert.ToInteger(thisFF2LRecFound["CALENDAR_YEAR"]) || (thisYear == cDBConvert.ToInteger(thisFF2LRecFound["CALENDAR_YEAR"]) && thisQuarter > cDBConvert.ToInteger(thisFF2LRecFound["QUARTER"])))
                  thisFF2LRecFound = drv;//take the later of the two                                
              }
          }
          int EndYear = cDBConvert.ToInteger(thisFF2LRecFound["CALENDAR_YEAR"]);
          int EndQtr = cDBConvert.ToInteger(thisFF2LRecFound["QUARTER"]);
          Category.SetCheckParameter("FF2L_Accuracy_End_Year_Quarter", EndYear.ToString() + EndQtr.ToString(), eParameterDataType.String);
          DataView OpSuppRecs = Category.GetCheckParameter("Operating_Supp_Data_Records_by_Location").ValueAsDataView();
          DataView OpSuppRecsFound;
          FilterFF2LRecs = new sFilterPair[3];
          FilterFF2LRecs[0].Set("MON_SYS_ID", MonSysID);
          sFilterPair[] FilterOpSupp = new sFilterPair[5];
          FilterOpSupp[0].Set("OP_TYPE_CD", "OPHOURS,OSHOURS", eFilterPairStringCompare.InList);
          FilterOpSupp[1].Set("FUEL_CD", cDBConvert.ToString(Category.GetCheckParameter("Current_Fuel_Flow_Record").ValueAsDataRowView()["FUEL_CD"]));
          FilterOpSupp[2].Set("OP_VALUE", 168, eFilterDataType.Integer, eFilterPairRelativeCompare.GreaterThanOrEqual);
          for (int i = BegYear; i <= EndYear; i++)
          {
            int thisYearsFirstQ = 1;
            if (i == BegYear)
              thisYearsFirstQ = BegQtr;
            for (int j = thisYearsFirstQ; j <= 4 && !(j > EndQtr && i == EndYear); j++)
            {
              FoundFF2LRec = false;
              FilterFF2LRecs[1].Set("CALENDAR_YEAR", i, eFilterDataType.Integer);
              FilterFF2LRecs[2].Set("QUARTER", j, eFilterDataType.Integer);
              FF2LTestRecsFound = FindRows(FF2LTestRecs, FilterFF2LRecs);
              if (FF2LTestRecsFound.Count > 0)
              {
                FoundFF2LRec = true;
                thisFF2LRecFound = FF2LTestRecsFound[0];
              }
              if (FoundFF2LRec)
              {
                if (cDBConvert.ToString(thisFF2LRecFound["TEST_RESULT_CD"]) == "FEW168H")
                {
                  FilterOpSupp[3].Set("CALENDAR_YEAR", i, eFilterDataType.Integer);
                  FilterOpSupp[4].Set("QUARTER", j, eFilterDataType.Integer);
                  OpSuppRecsFound = FindRows(OpSuppRecs, FilterOpSupp);
                  if (OpSuppRecsFound.Count > 0)
                  {
                    DataView FF2LBaselineRecs = Category.GetCheckParameter("FF2L_Baseline_Records_By_Location_For_QA_Status").ValueAsDataView();
                    sFilterPair[] FilterFF2LBASRecs = new sFilterPair[1];
                    FilterFF2LBASRecs[0].Set("MON_SYS_ID", MonSysID);
                    DataView FF2LBaselineRecsFound = FindRows(FF2LBaselineRecs, FilterFF2LBASRecs);
                    bool FoundFF2LBASRec = false;
                    int BaselineEndYear, BaselineEndQtr;
                    foreach (DataRowView drv in FF2LBaselineRecsFound)
                    {
                      BaselineEndYear = cDBConvert.ToDate(drv["END_DATE"], DateTypes.END).Year;
                      BaselineEndQtr = cDBConvert.ToDate(drv["END_DATE"], DateTypes.END).Quarter();
                      if (BaselineEndYear == i && BaselineEndQtr == j)
                      {
                        FoundFF2LBASRec = true;
                        break;
                      }
                    }
                    if (!FoundFF2LBASRec)
                    {
                      FoundTestNum = cDBConvert.ToString(thisFF2LRecFound["TEST_NUM"]);
                      Category.SetCheckParameter("Invalid_FF2L_Test_Number", FoundTestNum, eParameterDataType.String);
                      Category.SetCheckParameter("Current_Accuracy_Status", "OOC-Invalid Fuel Flow to Load Test", eParameterDataType.String);
                      return ReturnVal;
                    }
                  }
                }
              }
              else
              {
                FilterOpSupp[3].Set("CALENDAR_YEAR", i, eFilterDataType.Integer);
                FilterOpSupp[4].Set("QUARTER", j, eFilterDataType.Integer);
                OpSuppRecsFound = FindRows(OpSuppRecs, FilterOpSupp);
                if (OpSuppRecsFound.Count > 0)
                {
                  Category.SetCheckParameter("Missing_FF2L_Year_Quarter", i.ToString() + " Q" + j.ToString(), eParameterDataType.String);
                  Category.SetCheckParameter("Current_Accuracy_Status", "Undetermined-Missing Fuel Flow to Load Test", eParameterDataType.String);
                  return ReturnVal;
                }
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "ADESTAT5");
      }

      return ReturnVal;
    }

    public string ADESTAT6(cCategory Category, ref bool Log)
    //Determine Accuracy Test Expiration Date       
    {
        string ReturnVal = "";

        try
        {
            Category.SetCheckParameter("Accuracy_Missing_Op_Data_Info", null, eParameterDataType.String);
            string Status = Category.GetCheckParameter("Current_Accuracy_Status").ValueAsString();
            if (Status == "")
            {
                DataRowView PriorAccTestRec = Category.GetCheckParameter("Prior_Accuracy_Record").ValueAsDataRowView();
                DateTime PriorExpDate = cDBConvert.ToDate(PriorAccTestRec["TEST_EXP_DATE"], DateTypes.END);
                DateTime PriorEndDate = cDBConvert.ToDate(PriorAccTestRec["END_DATE"], DateTypes.END);
                DateTime ReinstallDate = cDBConvert.ToDate(PriorAccTestRec["REINSTALL_DATE"], DateTypes.START);
                if (ReinstallDate > PriorEndDate)
                    PriorEndDate = ReinstallDate;
                DateTime CurrentDate = Category.GetCheckParameter("Current_Operating_Date").ValueAsDateTime(DateTypes.END);
                int PriorYear = PriorEndDate.Year;
                int PriorQtr = cDateFunctions.ThisQuarter(PriorEndDate);
                int CurrentYear = CurrentDate.Year;
                int CurrentQtr = cDateFunctions.ThisQuarter(CurrentDate);
                int FirstQtr;
                bool AccEligible = Category.GetCheckParameter("FF2L_Accuracy_Eligible").ValueAsBool();

                int BegYr = 9999;
                int BegQtr = 4;
                int EndYr = 0;
                int EndQtr = 1;
                string BegYrQtr = Category.GetCheckParameter("FF2L_Accuracy_Begin_Year_Quarter").ValueAsString();
                string EndYrQtr = Category.GetCheckParameter("FF2L_Accuracy_End_Year_Quarter").ValueAsString();
                if (BegYrQtr != "" & EndYrQtr != "")
                {
                    BegYr = int.Parse(BegYrQtr.Substring(0, 4));
                    BegQtr = int.Parse(BegYrQtr.Substring(4, 1));
                    EndYr = int.Parse(EndYrQtr.Substring(0, 4));
                    EndQtr = int.Parse(EndYrQtr.Substring(4, 1));
                }

                DataRowView CurrentFFRecord = Category.GetCheckParameter("Current_Fuel_Flow_Record").ValueAsDataRowView();
                string FuelCd = cDBConvert.ToString(CurrentFFRecord["FUEL_CD"]);
                DataView OpSuppRecs = Category.GetCheckParameter("Operating_Supp_Data_Records_by_Location").ValueAsDataView();
                DataView OpSuppRecsFound;
                sFilterPair[] FilterOpSupp = new sFilterPair[4];
                FilterOpSupp[0].Set("OP_TYPE_CD", "OPHOURS");
                FilterOpSupp[1].Set("FUEL_CD", FuelCd);
                bool IsOSOReporter;
                DataView LocRptFreqRecords = Category.GetCheckParameter("Location_Reporting_Frequency_Records").ValueAsDataView();
                sFilterPair[] LocRptFilter = new sFilterPair[1];
                LocRptFilter[0].Set("REPORT_FREQ_CD", "OS");
                DataView LocRptFreqRecsFound = FindRows(LocRptFreqRecords, LocRptFilter);
                string thisBegYrQtr;
                string thisEndYrQtr;
                int thisBegYr;
                int thisEndYr;
                int thisBegQtr;
                int thisEndQtr;
                DateTime EarliestLocRptDate = Category.GetCheckParameter("Earliest_Location_Report_Date").ValueAsDateTime(DateTypes.START);
                DateTime LastDateThisQtr;


                DataRowView FFCompRecToCheck = Category.GetCheckParameter("Fuel_Flow_Component_Record_to_Check").ValueAsDataRowView();
                string CompID = FFCompRecToCheck == null ? "" : cDBConvert.ToString(FFCompRecToCheck["COMPONENT_ID"]);
                DataView TEERecs = Category.GetCheckParameter("Test_Extension_Exemption_Records").ValueAsDataView();
                
                for (int i = PriorYear; i <= CurrentYear; i++)
                {
                    if (i == PriorYear)
                        if (PriorQtr == 4)
                            continue;//begin with PriorYear+1
                        else
                            FirstQtr = PriorQtr + 1;
                    else
                        FirstQtr = 1;
                    for (int j = FirstQtr; j <= 4 && !(j == CurrentQtr && i == CurrentYear); j++)
                    {
                        IsOSOReporter = false;
                        foreach (DataRowView drv in LocRptFreqRecsFound)
                        {
                            thisBegYrQtr = cDBConvert.ToString(drv["BEGIN_QUARTER"]);
                            thisEndYrQtr = cDBConvert.ToString(drv["END_QUARTER"]);
                            thisBegYr = 0;
                            thisEndYr = 0;
                            thisBegQtr = 0;
                            thisEndQtr = 0;
                            if (thisBegYrQtr.Length > 9)
                            {
                                thisBegYr = int.Parse(thisBegYrQtr.Substring(0, 4));
                                thisBegQtr = int.Parse(thisBegYrQtr.Substring(9, 1));
                            }
                            if (thisEndYrQtr.Length > 9)
                            {
                                thisEndYr = int.Parse(thisEndYrQtr.Substring(0, 4));
                                thisEndQtr = int.Parse(thisEndYrQtr.Substring(9, 1));
                            }
                            if ((thisBegYr < i || (thisBegYr == i && thisBegQtr <= j)) && (thisEndYrQtr == "" || thisEndYr > i || (thisEndQtr >= j && thisEndYr == i)))
                            {
                                IsOSOReporter = true;
                                break;
                            }
                        }
                        if (!IsOSOReporter || j == 3)
                            if (AccEligible && (i > BegYr || (i == BegYr && j >= BegQtr)) && (i < EndYr || (i == EndYr && j <= EndQtr)))
                                PriorExpDate = cDateFunctions.LastDateThisQuarter(PriorExpDate.AddMonths(3));
                            else
                            {
                                LastDateThisQtr = cDateFunctions.LastDateThisQuarter(i, j);
                                if (EarliestLocRptDate > LastDateThisQtr)
                                    PriorExpDate = cDateFunctions.LastDateThisQuarter(PriorExpDate.AddMonths(3));
                                else
                                {
                                    FilterOpSupp[2].Set("CALENDAR_YEAR", i, eFilterDataType.Integer);
                                    FilterOpSupp[3].Set("QUARTER", j, eFilterDataType.Integer);
                                    FilterOpSupp[1].Set("FUEL_CD", FuelCd);
                                    OpSuppRecsFound = FindRows(OpSuppRecs, FilterOpSupp);
                                    if (OpSuppRecsFound.Count == 0)
                                    {
                                        FilterOpSupp[1].Set("FUEL_CD", DBNull.Value, eFilterDataType.String);
                                        OpSuppRecsFound = FindRows(OpSuppRecs, FilterOpSupp);
                                        if (OpSuppRecsFound.Count == 0)
                                            Category.SetCheckParameter("Accuracy_Missing_Op_Data_Info", i.ToString() + "Q" + j.ToString(), eParameterDataType.String);
                                        else
                                            PriorExpDate = cDateFunctions.LastDateThisQuarter(PriorExpDate.AddMonths(3));
                                    }
                                    else
                                        if (cDBConvert.ToInteger(OpSuppRecsFound[0]["OP_VALUE"]) <= 168)
                                            PriorExpDate = cDateFunctions.LastDateThisQuarter(PriorExpDate.AddMonths(3));
                                        else
                                        {
                                            sFilterPair[] FilterTEE = new sFilterPair[4];
                                            FilterTEE[0].Set("COMPONENT_ID", CompID);
                                            FilterTEE[1].Set("CALENDAR_YEAR", i, eFilterDataType.Integer);
                                            FilterTEE[2].Set("QUARTER", j, eFilterDataType.Integer);
                                            FilterTEE[3].Set("EXTENS_EXEMPT_CD", "NONQADB");
                                            if (CountRows(TEERecs, FilterTEE) > 0)
                                                PriorExpDate = cDateFunctions.LastDateThisQuarter(PriorExpDate.AddMonths(3));
                                        }
                                }
                            }
                        else
                        {                            
                            string FFRecMonSysID = cDBConvert.ToString(CurrentFFRecord["MON_SYS_ID"]);
                            sFilterPair[] FilterTEE = new sFilterPair[4];
                            FilterTEE[0].Set("CALENDAR_YEAR", i, eFilterDataType.Integer);
                            FilterTEE[1].Set("QUARTER", j, eFilterDataType.Integer);
                            FilterTEE[2].Set("EXTENS_EXEMPT_CD", "NONQAOS");
                            FilterTEE[3].Set("FUEL_CD", FuelCd);
                            if (IsOSOReporter && j == 2)
                            {
                                if (AccEligible && (i > BegYr || (i == BegYr && j >= BegQtr)) && (i < EndYr || (i == EndYr && j <= EndQtr)))
                                    PriorExpDate = cDateFunctions.LastDateThisQuarter(PriorExpDate.AddMonths(3));
                                else
                                    if (CountRows(TEERecs, FilterTEE) > 0)
                                        PriorExpDate = cDateFunctions.LastDateThisQuarter(PriorExpDate.AddMonths(3));
                                    else
                                    {
                                        FilterTEE = new sFilterPair[4];
                                        FilterTEE[0].Set("COMPONENT_ID", CompID);
                                        FilterTEE[1].Set("CALENDAR_YEAR", i, eFilterDataType.Integer);
                                        FilterTEE[2].Set("QUARTER", j, eFilterDataType.Integer);
                                        FilterTEE[3].Set("EXTENS_EXEMPT_CD", "NONQADB");
                                        if (CountRows(TEERecs, FilterTEE) > 0)
                                            PriorExpDate = cDateFunctions.LastDateThisQuarter(PriorExpDate.AddMonths(3));
                                    }
                            }
                            else
                                if (IsOSOReporter && (j == 1 || j == 4))
                                    if (AccEligible && (i > BegYr || (i == BegYr && j >= BegQtr)) && (i < EndYr || (i == EndYr && j <= EndQtr)))
                                        PriorExpDate = cDateFunctions.LastDateThisQuarter(PriorExpDate.AddMonths(3));
                                    else
                                        if (CountRows(TEERecs, FilterTEE) > 0)
                                            PriorExpDate = cDateFunctions.LastDateThisQuarter(PriorExpDate.AddMonths(3));
                                        else
                                        {
                                            FilterTEE = new sFilterPair[4];
                                            FilterTEE[0].Set("COMPONENT_ID", CompID);
                                            FilterTEE[1].Set("CALENDAR_YEAR", i, eFilterDataType.Integer);
                                            FilterTEE[2].Set("QUARTER", j, eFilterDataType.Integer);
                                            FilterTEE[3].Set("EXTENS_EXEMPT_CD", "NONQADB");
                                            if (CountRows(TEERecs, FilterTEE) > 0)
                                                PriorExpDate = cDateFunctions.LastDateThisQuarter(PriorExpDate.AddMonths(3));
                                        }
                        }
                    }
                }
                if (CurrentDate > PriorExpDate)
                    if (Category.GetCheckParameter("Accuracy_Missing_Op_Data_Info").ParameterValue != null)
                    {
                        Status = "Missing Op Data";
                        Category.CheckCatalogResult = Status;
                    }
                    else
                        if (!AccEligible && Category.GetCheckParameter("FF2L_Accuracy_Eligible").ParameterValue != null)
                        {
                            Status = "OOC-Accuracy Test Expired-Fuel Flow To Load Test Ignored";
                            Category.CheckCatalogResult = Status;
                        }
                        else
                            Status = "OOC-Accuracy Test Expired";
                else
                    Status = "IC-Extension";
            }
            if (string.IsNullOrEmpty(Category.CheckCatalogResult) && Status != "" && !Status.StartsWith("IC"))
            {
              if ((Status.StartsWith("OOC") || Status.StartsWith("Undetermined")) && Category.GetCheckParameter("Invalid_Accuracy_Record").ParameterValue != null)
                Status += "*";
              Category.CheckCatalogResult = Status;
            }
            else
            {
              if ((bool)Category.GetCheckParameter("Inappropriate_Transmitter_Transducer_Test").ParameterValue)
                Category.CheckCatalogResult = "Inappropriate Transmitter Transducer Test";
            }
            if (Status != "")
                Category.SetCheckParameter("Current_Accuracy_Status", Status, eParameterDataType.String);
        }
        catch (Exception ex)
        {
            ReturnVal = Category.CheckEngine.FormatError(ex, "ADESTAT6");
        }

        return ReturnVal;
    }

    public string ADESTAT7(cCategory Category, ref bool Log)
    //Determine if Component Requires a PEI Test       
    {
      string ReturnVal = "";

      try
      {
        DataRowView PriorAccTestRec = Category.GetCheckParameter("Prior_Accuracy_Record").ValueAsDataRowView();
        if (PriorAccTestRec != null && cDBConvert.ToString(PriorAccTestRec["TEST_TYPE_CD"]) == "FFACCTT")
          Category.SetCheckParameter("PEI_Required", true, eParameterDataType.Boolean);
        else
          Category.SetCheckParameter("PEI_Required", false, eParameterDataType.Boolean);
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "ADESTAT7");
      }

      return ReturnVal;
    }

    public string ADESTAT8(cCategory Category, ref bool Log)
    //Locate Most Recent Prior PEI Test       
    {
      string ReturnVal = "";

      try
      {
        string Status = "";
        Category.SetCheckParameter("Prior_PEI_Record", null, eParameterDataType.String);
        if (Category.GetCheckParameter("PEI_Required").ValueAsBool())
        {
          DataView PEITestRecs = Category.GetCheckParameter("PEI_Test_Records_By_Location_For_QA_Status").ValueAsDataView();
          DataRowView FFCompRecToCheck = Category.GetCheckParameter("Fuel_Flow_Component_Record_to_Check").ValueAsDataRowView();
          string CompID = cDBConvert.ToString(FFCompRecToCheck["COMPONENT_ID"]);
          DateTime CurrentDate = Category.GetCheckParameter("Current_Operating_Date").ValueAsDateTime(DateTypes.END);
          int CurrentHour = Category.GetCheckParameter("Current_Operating_Hour").ValueAsInt();
          sFilterPair[] FilterPEITest = new sFilterPair[2];
          FilterPEITest[0].Set("TEST_RESULT_CD", "INVALID", true);
          FilterPEITest[1].Set("COMPONENT_ID", CompID);
          DataView PEITestRecsFound;
          DataRowView PriorPEITestRec;
          if (CurrentHour > 0)
            PEITestRecsFound = FindActiveRows(PEITestRecs, DateTime.MinValue, 0, CurrentDate, CurrentHour - 1, "END_DATE", "END_HOUR", "END_DATE", "END_HOUR", FilterPEITest);
          else
            PEITestRecsFound = FindActiveRows(PEITestRecs, DateTime.MinValue, 0, CurrentDate.AddDays(-1), 23, "END_DATE", "END_HOUR", "END_DATE", "END_HOUR", FilterPEITest);
          if (PEITestRecsFound.Count > 0)
          {
            PEITestRecsFound.Sort = "END_DATE DESC, END_HOUR DESC, END_MIN DESC";
            PriorPEITestRec = PEITestRecsFound[0];
            Category.SetCheckParameter("Prior_PEI_Record", PriorPEITestRec, eParameterDataType.DataRowView);
            if (cDBConvert.ToString(PriorPEITestRec["QA_NEEDS_EVAL_FLG"]) == "Y")
              Status = "PEI Test Not Yet Evaluated";
            else
              switch (cDBConvert.ToString(PriorPEITestRec["TEST_RESULT_CD"]))
              {
                case "":
                  Status = "OOC-PEI Test Has Critical Errors";
                  break;
                case "FAILED":
                  Status = "OOC-PEI Test Failed";
                  break;
                case "ABORTED":
                  Status = "OOC-PEI Test Aborted";
                  break;
                default:
                  break;
              }
          }
          else
            Status = "OOC-No Prior PEI Test";
        }
        if (Status != "")
        {
          Category.SetCheckParameter("Current_PEI_Status", Status, eParameterDataType.String);
          Category.CheckCatalogResult = Status;
        }
        else
          Category.SetCheckParameter("Current_PEI_Status", null, eParameterDataType.String);
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "ADESTAT8");
      }

      return ReturnVal;
    }

    public string ADESTAT9(cCategory Category, ref bool Log)
    //Locate Most Recent Prior PEI Event       
    {
      string ReturnVal = "";

      try
      {
        Category.SetCheckParameter("Prior_PEI_Event_Record", null, eParameterDataType.DataRowView);
        string PEIStatus = Category.GetCheckParameter("Current_PEI_Status").ValueAsString();
        if (Category.GetCheckParameter("PEI_Required").ValueAsBool() && PEIStatus == "")
        {
          DataView QACertEventRecs = Category.GetCheckParameter("Qa_Certification_Event_Records").ValueAsDataView();
          DataView QACertEventRecsFound;
          DataRowView FFCompRecToCheck = Category.GetCheckParameter("Fuel_Flow_Component_Record_to_Check").ValueAsDataRowView();
          string CompID = cDBConvert.ToString(FFCompRecToCheck["COMPONENT_ID"]);
          DateTime CurrentDate = Category.GetCheckParameter("Current_Operating_Date").ValueAsDateTime(DateTypes.END);
          int CurrentHour = Category.GetCheckParameter("Current_Operating_Hour").ValueAsInt();
          DataRowView PriorPEIRec = Category.GetCheckParameter("Prior_PEI_Record").ValueAsDataRowView();
          DateTime PriorPEIEndDate = cDBConvert.ToDate(PriorPEIRec["END_DATE"], DateTypes.END);
          int PriorPEIEndHour = cDBConvert.ToHour(PriorPEIRec["END_HOUR"], DateTypes.END);
          sFilterPair[] QACertFilter = new sFilterPair[2];
          QACertFilter[0].Set("COMPONENT_ID", CompID);
          QACertFilter[1].Set("PEI_REQUIRED", "Y");
          if (CurrentHour == 0)
            if (PriorPEIEndHour == 23)
              QACertEventRecsFound = FindActiveRows(QACertEventRecs, PriorPEIEndDate.AddDays(1), 0, CurrentDate.AddDays(-1), 23, "QA_CERT_EVENT_DATE", "QA_CERT_EVENT_HOUR", "QA_CERT_EVENT_DATE", "QA_CERT_EVENT_HOUR", QACertFilter);
            else
              QACertEventRecsFound = FindActiveRows(QACertEventRecs, PriorPEIEndDate, PriorPEIEndHour + 1, CurrentDate.AddDays(-1), 23, "QA_CERT_EVENT_DATE", "QA_CERT_EVENT_HOUR", "QA_CERT_EVENT_DATE", "QA_CERT_EVENT_HOUR", QACertFilter);
          else
            if (PriorPEIEndHour == 23)
              QACertEventRecsFound = FindActiveRows(QACertEventRecs, PriorPEIEndDate.AddDays(1), 0, CurrentDate, CurrentHour - 1, "QA_CERT_EVENT_DATE", "QA_CERT_EVENT_HOUR", "QA_CERT_EVENT_DATE", "QA_CERT_EVENT_HOUR", QACertFilter);
            else
              QACertEventRecsFound = FindActiveRows(QACertEventRecs, PriorPEIEndDate, PriorPEIEndHour + 1, CurrentDate, CurrentHour - 1, "QA_CERT_EVENT_DATE", "QA_CERT_EVENT_HOUR", "QA_CERT_EVENT_DATE", "QA_CERT_EVENT_HOUR", QACertFilter);
          if (QACertEventRecsFound.Count > 0)
          {
            Category.SetCheckParameter("Prior_PEI_Event_Record", QACertEventRecsFound[0], eParameterDataType.DataRowView);
            PEIStatus = "OOC-Event";
          }
          else
          {
            DateTime PriorExpDate = cDateFunctions.LastDateThisQuarter(PriorPEIEndDate).AddYears(5);
            if (CurrentDate > PriorExpDate)
              PEIStatus = "OOC-PEI Test Expired";
            else
            {
              PriorExpDate = cDateFunctions.LastDateThisQuarter(PriorPEIEndDate).AddYears(3);
              PriorPEIRec["TEST_EXP_DATE"] = PriorExpDate;
              if (CurrentDate <= PriorExpDate)
                PEIStatus = "IC";
            }
          }
          if (PEIStatus.StartsWith("OOC"))
            Category.CheckCatalogResult = PEIStatus;
          if (PEIStatus != "")
            Category.SetCheckParameter("Current_PEI_Status", PEIStatus, eParameterDataType.String);
        }
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "ADESTAT9");
      }

      return ReturnVal;
    }

    public string ADESTAT10(cCategory Category, ref bool Log)
    //Determine Eligibility for Fuel Flow to Load Testing (PEI)       
    {
      string ReturnVal = "";

      try
      {
        Category.SetCheckParameter("FF2L_PEI_Eligible", null, eParameterDataType.Boolean);
        Category.SetCheckParameter("FF2L_PEI_Check_Date", null, eParameterDataType.Date);
        if (Category.GetCheckParameter("PEI_Required").ValueAsBool() && Category.GetCheckParameter("Current_PEI_Status").ParameterValue == null)
        {
          DataView FF2LTestRecs = Category.GetCheckParameter("FF2L_Test_Records_By_Location_For_QA_Status").ValueAsDataView();
          DataRowView FFCompRecToCheck = Category.GetCheckParameter("Fuel_Flow_Component_Record_to_Check").ValueAsDataRowView();
          string MonSysID = cDBConvert.ToString(FFCompRecToCheck["MON_SYS_ID"]);

          DateTime CurrentDate = Category.GetCheckParameter("Current_Operating_Date").ValueAsDateTime(DateTypes.END);
          DataRowView PriorPEIRec = Category.GetCheckParameter("Prior_PEI_Record").ValueAsDataRowView();
          DateTime PriorPEIEndDate = cDBConvert.ToDate(PriorPEIRec["END_DATE"], DateTypes.END);
          int PriorPEIYear = PriorPEIEndDate.Year;
          int PriorPEIQuarter = cDateFunctions.ThisQuarter(PriorPEIEndDate);
          int CurrentYear = CurrentDate.Year;
          int CurrentQuarter = cDateFunctions.ThisQuarter(CurrentDate);
          DateTime CheckDate = DateTime.MinValue;

          sFilterPair[] Filter = new sFilterPair[2];
          Filter[0].Set("MON_SYS_ID", MonSysID);
          Filter[1].Set("TEST_RESULT_CD", "PASSED,FAILED,FEW168H,EXC168H,INPROG", eFilterPairStringCompare.InList);
          DataView FF2LTestRecsFound = FindRows(FF2LTestRecs, Filter);
          int thisYear;
          int thisQuarter;
          bool FoundFF2LRec = false;
          foreach (DataRowView drv in FF2LTestRecsFound)
          {
            thisYear = cDBConvert.ToInteger(drv["CALENDAR_YEAR"]);
            thisQuarter = cDBConvert.ToInteger(drv["QUARTER"]);
            if (thisYear < CurrentYear || (thisYear == CurrentYear && thisQuarter < CurrentQuarter))
              if (thisYear > PriorPEIYear || (thisYear == PriorPEIYear && thisQuarter > PriorPEIQuarter))
              {
                FoundFF2LRec = true;
                break;
              }
          }
          if (FoundFF2LRec)
          {
            bool ValidFuelFlowTestExistsForEachComponent  = true;
            DateTime CertificationCheckDate;
            CheckExistenceOfValidFuelFlowTest(Category, out ValidFuelFlowTestExistsForEachComponent , out CertificationCheckDate);

            if (ValidFuelFlowTestExistsForEachComponent  && CertificationCheckDate != null)
            {
              Category.SetCheckParameter("FF2L_PEI_Eligible", true, eParameterDataType.Boolean);
              Category.SetCheckParameter("FF2L_PEI_Check_Date", CertificationCheckDate, eParameterDataType.Date);
            }
            else
            {
              Category.SetCheckParameter("FF2L_PEI_Eligible", true, eParameterDataType.Boolean);
              Category.SetCheckParameter("FF2L_PEI_Check_Date", PriorPEIEndDate, eParameterDataType.Date);
              DataView FFCompRecs = Category.GetCheckParameter("Fuel_Flow_Component_Records").ValueAsDataView();
              string RecToCheckCompID = cDBConvert.ToString(FFCompRecToCheck["COMPONENT_ID"]);
              string CompID = "";
              bool Found;
              DataView AccTestRecs = Category.GetCheckParameter("Accuracy_Test_Records_By_Location_For_QA_Status").ValueAsDataView();
              DataView AccTestRecsFound;
              sFilterPair[] FilterAccTest = new sFilterPair[2];
              DataView PEITestRecs = Category.GetCheckParameter("PEI_Test_Records_By_Location_For_QA_Status").ValueAsDataView();
              DataView PEITestRecsFound;
              sFilterPair[] FilterPEITest = new sFilterPair[2];
              foreach (DataRowView drv in FFCompRecs)
              {
                CompID = cDBConvert.ToString(drv["COMPONENT_ID"]);
                FilterAccTest[0].Set("TEST_RESULT_CD", "PASSED");
                FilterAccTest[1].Set("COMPONENT_ID", CompID);
                AccTestRecsFound = FindRows(AccTestRecs, FilterAccTest);
                string thisTestType = "";
                Found = false;
                CheckDate = DateTime.MinValue;
                foreach (DataRowView drv2 in AccTestRecsFound)
                {
                  thisYear = cDBConvert.ToInteger(drv2["CALENDAR_YEAR"]);
                  thisQuarter = cDBConvert.ToInteger(drv2["QUARTER"]);
                  if (cDBConvert.ToDate(drv2["REINSTALL_DATE"], DateTypes.START) > cDBConvert.ToDate(drv2["END_DATE"], DateTypes.START))
                  {
                    thisYear = cDBConvert.ToDate(drv2["REINSTALL_DATE"], DateTypes.START).Year;
                    thisQuarter = cDateFunctions.ThisQuarter(cDBConvert.ToDate(drv2["REINSTALL_DATE"], DateTypes.START));
                  }
                  if (thisYear == PriorPEIYear && (thisQuarter == PriorPEIQuarter || thisQuarter == PriorPEIQuarter + 1 || thisQuarter == PriorPEIQuarter - 1) ||
                      (thisYear == PriorPEIYear + 1 && (thisQuarter == 1 && PriorPEIQuarter == 4)) ||
                      (thisYear == PriorPEIYear - 1 && (thisQuarter == 4 && PriorPEIQuarter == 1)))
                  {
                    Found = true;
                    if (cDBConvert.ToDate(drv2["END_DATE"], DateTypes.START) > CheckDate)
                    {
                      CheckDate = cDBConvert.ToDate(drv2["END_DATE"], DateTypes.START);
                      thisTestType = cDBConvert.ToString(drv2["TEST_TYPE_CD"]);
                    }
                    if (cDBConvert.ToDate(drv2["REINSTALL_DATE"], DateTypes.START) > CheckDate)
                    {
                      CheckDate = cDBConvert.ToDate(drv2["REINSTALL_DATE"], DateTypes.START);
                      thisTestType = cDBConvert.ToString(drv2["TEST_TYPE_CD"]);
                    }
                  }
                }
                if (!Found)
                {
                  Category.SetCheckParameter("FF2L_PEI_Eligible", false, eParameterDataType.Boolean);
                  return ReturnVal;
                }
                else
                {
                  if (CheckDate > Category.GetCheckParameter("FF2L_PEI_Check_Date").ValueAsDateTime(DateTypes.START))
                    Category.SetCheckParameter("FF2L_PEI_Check_Date", CheckDate, eParameterDataType.Date);
                  if (thisTestType == "FFACCTT" && CompID != RecToCheckCompID)
                  {
                    FilterPEITest[0].Set("TEST_RESULT_CD", "PASSED");
                    FilterPEITest[1].Set("COMPONENT_ID", CompID);
                    PEITestRecsFound = FindRows(PEITestRecs, FilterPEITest);
                    Found = false;
                    foreach (DataRowView drv2 in PEITestRecsFound)
                    {
                      thisYear = cDBConvert.ToInteger(drv2["CALENDAR_YEAR"]);
                      thisQuarter = cDBConvert.ToInteger(drv2["QUARTER"]);
                      if (thisYear == PriorPEIYear && (thisQuarter == PriorPEIQuarter || thisQuarter == PriorPEIQuarter + 1 || thisQuarter == PriorPEIQuarter - 1) ||
                          (thisYear == PriorPEIYear + 1 && (thisQuarter == 1 && PriorPEIQuarter == 4)) ||
                          (thisYear == PriorPEIYear - 1 && (thisQuarter == 4 && PriorPEIQuarter == 1)))
                      {
                        Found = true;
                        break;
                      }
                    }
                    if (!Found)
                    {
                      Category.SetCheckParameter("FF2L_Accuracy_Eligible", false, eParameterDataType.Boolean);
                      return ReturnVal;
                    }
                    else
                    {
                      if (CheckDate > Category.GetCheckParameter("FF2L_PEI_Check_Date").ValueAsDateTime(DateTypes.START))
                        Category.SetCheckParameter("FF2L_PEI_Check_Date", CheckDate, eParameterDataType.Date);
                    }
                  }
                }
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "ADESTAT10");
      }

      return ReturnVal;
    }

    public string ADESTAT11(cCategory Category, ref bool Log)
    //Evaluate Fuel Flow to Load Tests (PEI)       
    {
      string ReturnVal = "";

      try
      {
        Category.SetCheckParameter("FF2L_PEI_Begin_Year_Quarter", null, eParameterDataType.String);
        Category.SetCheckParameter("FF2L_PEI_End_Year_Quarter", null, eParameterDataType.String);

        if (Category.GetCheckParameter("FF2L_PEI_Eligible").ValueAsBool())
        {
          DataView FF2LTestRecs = Category.GetCheckParameter("FF2L_Test_Records_By_Location_For_QA_Status").ValueAsDataView();

          DataRowView FFCompRecToCheck = Category.GetCheckParameter("Fuel_Flow_Component_Record_to_Check").ValueAsDataRowView();
          string MonSysID = cDBConvert.ToString(FFCompRecToCheck["MON_SYS_ID"]);
          string componentId = cDBConvert.ToString(FFCompRecToCheck["COMPONENT_ID"]);

          string Status = "";
          DateTime CurrentDate = Category.GetCheckParameter("Current_Operating_Date").ValueAsDateTime(DateTypes.END);
          DataRowView PriorPEITestRec = Category.GetCheckParameter("Prior_PEI_Record").ValueAsDataRowView();
          DateTime PriorPEIRecEndDate = cDBConvert.ToDate(PriorPEITestRec["END_DATE"], DateTypes.START);
          DateTime ReinstallDate = cDBConvert.ToDate(PriorPEITestRec["REINSTALL_DATE"], DateTypes.START);
          DateTime FirstDate = PriorPEIRecEndDate;
          if (ReinstallDate > PriorPEIRecEndDate)
            FirstDate = ReinstallDate;
          int FirstYear = FirstDate.Year;
          int FirstQuarter = cDateFunctions.ThisQuarter(FirstDate);
          int SecondYear = CurrentDate.Year;
          int SecondQuarter = cDateFunctions.ThisQuarter(CurrentDate);
          DateTime CheckDate = Category.GetCheckParameter("FF2L_PEI_Check_Date").ValueAsDateTime(DateTypes.END);

          sFilterPair[] FilterFF2LRecs = new sFilterPair[2];
          FilterFF2LRecs[0].Set("MON_SYS_ID", MonSysID);
          FilterFF2LRecs[1].Set("TEST_RESULT_CD", "FAILED");
          DataView FF2LTestRecsFound = FindRows(FF2LTestRecs, FilterFF2LRecs);

          int thisYear;
          int thisQuarter;
          bool FoundFF2LRec = false;
          string FoundTestNum = "";//compiler needs initialization
          DataRowView FuelFlowToLoadRow = null;
          {
            foreach (DataRowView drv in FF2LTestRecsFound)
            {
              thisYear = cDBConvert.ToInteger(drv["CALENDAR_YEAR"]);
              thisQuarter = cDBConvert.ToInteger(drv["QUARTER"]);
              if (thisYear < SecondYear || (thisYear == SecondYear && thisQuarter < SecondQuarter))
                if (thisYear > FirstYear || (thisYear == FirstYear && thisQuarter > FirstQuarter))
                {
                  FoundFF2LRec = true;
                  FuelFlowToLoadRow = drv;
                  FoundTestNum = cDBConvert.ToString(drv["TEST_NUM"]);
                  break;
                }
            }
          }

          if (FoundFF2LRec)
          {
            DateTime qaCertEventHourRangeBegin = FuelFlowToLoadRow["END_DATEHOUR"].AsDateTime(DateTime.MinValue);
            DateTime qaCertEventHourRangeEnd = Category.GetCheckParameter("Current_Operating_Date").AsDateTime(DateTime.MaxValue).AddHours(Category.GetCheckParameter("Current_Operating_Hour").AsInteger(23));

            DataView qaCertificationEventView = cRowFilter.FindRows(
                                                                     Category.GetCheckParameter("Qa_Certification_Event_Records").AsDataView(),
                                                                     new cFilterCondition[]
                                                                         {
                                                                           new cFilterCondition("COMPONENT_ID", componentId),
                                                                           new cFilterCondition("QA_CERT_EVENT_CD", "410"),
                                                                           new cFilterCondition("REQUIRED_TEST_CD", "53"),
                                                                           new cFilterCondition("QA_CERT_EVENT_DATEHOUR", qaCertEventHourRangeBegin, eFilterDataType.DateBegan, eFilterConditionRelativeCompare.GreaterThan),
                                                                           new cFilterCondition("QA_CERT_EVENT_DATEHOUR", qaCertEventHourRangeEnd, eFilterDataType.DateBegan, eFilterConditionRelativeCompare.LessThan)
                                                                         }
                                                                     );

            if (qaCertificationEventView.Count == 0)
            {
              Category.SetCheckParameter("Invalid_FF2L_Test_Number", FoundTestNum, eParameterDataType.String);
              Status = "OOC-Fuel Flow to Load Test Failed";
              Category.SetCheckParameter("Current_PEI_Status", Status, eParameterDataType.String);
              Category.CheckCatalogResult = Status;
              return ReturnVal;
            }
          }

          FilterFF2LRecs[1].Set("TEST_RESULT_CD", DBNull.Value, eFilterDataType.String);
          FF2LTestRecsFound = FindRows(FF2LTestRecs, FilterFF2LRecs);
          FoundFF2LRec = false;
          FoundTestNum = "";
          foreach (DataRowView drv in FF2LTestRecsFound)
          {
            thisYear = cDBConvert.ToInteger(drv["CALENDAR_YEAR"]);
            thisQuarter = cDBConvert.ToInteger(drv["QUARTER"]);
            if (thisYear < SecondYear || (thisYear == SecondYear && thisQuarter < SecondQuarter))
              if (thisYear > FirstYear || (thisYear == FirstYear && thisQuarter > FirstQuarter))
              {
                FoundFF2LRec = true;
                FoundTestNum = cDBConvert.ToString(drv["TEST_NUM"]);
                break;
              }
          }
          if (FoundFF2LRec)
          {
            Category.SetCheckParameter("Invalid_FF2L_Test_Number", FoundTestNum, eParameterDataType.String);
            Status = "OOC-Fuel Flow to Load Test Has Critical Errors";
            Category.SetCheckParameter("Current_PEI_Status", Status, eParameterDataType.String);
            Category.CheckCatalogResult = Status;
            return ReturnVal;
          }
          FilterFF2LRecs[1].Set("QA_NEEDS_EVAL_FLG", "Y");
          FF2LTestRecsFound = FindRows(FF2LTestRecs, FilterFF2LRecs);
          FoundFF2LRec = false;
          FoundTestNum = "";
          foreach (DataRowView drv in FF2LTestRecsFound)
          {
            thisYear = cDBConvert.ToInteger(drv["CALENDAR_YEAR"]);
            thisQuarter = cDBConvert.ToInteger(drv["QUARTER"]);
            if (thisYear < SecondYear || (thisYear == SecondYear && thisQuarter < SecondQuarter))
              if (thisYear > FirstYear || (thisYear == FirstYear && thisQuarter > FirstQuarter))
              {
                FoundFF2LRec = true;
                FoundTestNum = cDBConvert.ToString(drv["TEST_NUM"]);
                break;
              }
          }
          if (FoundFF2LRec)
          {
            Category.SetCheckParameter("Invalid_FF2L_Test_Number", FoundTestNum, eParameterDataType.String);
            Status = "Fuel Flow to Load Test Has Not Yet Been Evaluated";
            Category.SetCheckParameter("Current_PEI_Status", Status, eParameterDataType.String);
            Category.CheckCatalogResult = Status;
            return ReturnVal;
          }
          FilterFF2LRecs[1].Set("TEST_RESULT_CD", "PASSED,FEW168H,EXC168H", eFilterPairStringCompare.InList);
          FF2LTestRecsFound = FindRows(FF2LTestRecs, FilterFF2LRecs);
          DataRowView thisFF2LRecFound = null;
          FoundFF2LRec = false;
          foreach (DataRowView drv in FF2LTestRecsFound)
          {
            thisYear = cDBConvert.ToInteger(drv["CALENDAR_YEAR"]);
            thisQuarter = cDBConvert.ToInteger(drv["QUARTER"]);
            if (thisYear < SecondYear || (thisYear == SecondYear && thisQuarter < SecondQuarter))
              if (thisYear > FirstYear || (thisYear == FirstYear && thisQuarter > FirstQuarter))
              {
                if (!FoundFF2LRec)//if this is the first one found
                {
                  thisFF2LRecFound = drv;
                  FoundFF2LRec = true;
                }
                else
                  if (thisYear < cDBConvert.ToInteger(thisFF2LRecFound["CALENDAR_YEAR"]) || (thisYear == cDBConvert.ToInteger(thisFF2LRecFound["CALENDAR_YEAR"]) && thisQuarter < cDBConvert.ToInteger(thisFF2LRecFound["QUARTER"])))
                    thisFF2LRecFound = drv;//take the earlier of the two                                
              }
          }
          int BegYear = 0, BegQtr = 0;
          if (FoundFF2LRec)
          {
            BegYear = cDBConvert.ToInteger(thisFF2LRecFound["CALENDAR_YEAR"]);
            BegQtr = cDBConvert.ToInteger(thisFF2LRecFound["QUARTER"]);
            Category.SetCheckParameter("FF2L_PEI_Begin_Year_Quarter", BegYear.ToString() + BegQtr.ToString(), eParameterDataType.String);
          }
          FilterFF2LRecs[1].Set("TEST_RESULT_CD", "INPROG");
          FF2LTestRecsFound = FindRows(FF2LTestRecs, FilterFF2LRecs);
          thisFF2LRecFound = null;
          FoundFF2LRec = false;
          foreach (DataRowView drv in FF2LTestRecsFound)
          {
            thisYear = cDBConvert.ToInteger(drv["CALENDAR_YEAR"]);
            thisQuarter = cDBConvert.ToInteger(drv["QUARTER"]);
            if (thisYear < SecondYear || (thisYear == SecondYear && thisQuarter < SecondQuarter))
              if (thisYear > FirstYear || (thisYear == FirstYear && thisQuarter > FirstQuarter))
              {
                if (!FoundFF2LRec)//if this is the first one found
                {
                  thisFF2LRecFound = drv;
                  FoundFF2LRec = true;
                }
                else
                  if (thisYear > cDBConvert.ToInteger(thisFF2LRecFound["CALENDAR_YEAR"]) || (thisYear == cDBConvert.ToInteger(thisFF2LRecFound["CALENDAR_YEAR"]) && thisQuarter > cDBConvert.ToInteger(thisFF2LRecFound["QUARTER"])))
                    thisFF2LRecFound = drv;//take the later of the two                                
              }
          }
          if (FoundFF2LRec)
          {
            thisYear = cDBConvert.ToInteger(thisFF2LRecFound["CALENDAR_YEAR"]);
            thisQuarter = cDBConvert.ToInteger(thisFF2LRecFound["QUARTER"]);
            if (BegYear > 0 && (thisYear > BegYear || (thisYear == BegYear && thisQuarter > BegQtr)))
            {
              FoundTestNum = cDBConvert.ToString(thisFF2LRecFound["TEST_NUM"]);
              Category.SetCheckParameter("Invalid_FF2L_Test_Number", FoundTestNum, eParameterDataType.String);
              Status = "OOC-Invalid Fuel Flow to Load Test";
              Category.SetCheckParameter("Current_PEI_Status", Status, eParameterDataType.String);
              Category.CheckCatalogResult = Status;
              return ReturnVal;
            }
            else
            {
              if (thisYear > CheckDate.Year + 1 || (thisYear == CheckDate.Year + 1 && thisQuarter > cDateFunctions.ThisQuarter(CheckDate)))
              {
                FoundTestNum = cDBConvert.ToString(thisFF2LRecFound["TEST_NUM"]);
                Category.SetCheckParameter("Invalid_FF2L_Test_Number", FoundTestNum, eParameterDataType.String);
                Status = "Undetermined-Baseline Period Expired";
                Category.SetCheckParameter("Current_PEI_Status", Status, eParameterDataType.String);
                Category.CheckCatalogResult = Status;
                return ReturnVal;
              }
            }
          }
          BegYear = CheckDate.Year;
          BegQtr = cDateFunctions.ThisQuarter(CheckDate) + 1;
          if (BegQtr == 5)
          {
            BegYear = FirstYear + 1;
            BegQtr = 1;
          }
          Category.SetCheckParameter("FF2L_PEI_Begin_Year_Quarter", BegYear.ToString() + BegQtr.ToString(), eParameterDataType.String);

          FilterFF2LRecs[1].Set("TEST_RESULT_CD", "PASSED,FEW168H,EXC168H,INPROG", eFilterPairStringCompare.InList);
          FF2LTestRecsFound = FindRows(FF2LTestRecs, FilterFF2LRecs);
          thisFF2LRecFound = null;
          FoundFF2LRec = false;
          foreach (DataRowView drv in FF2LTestRecsFound)
          {
            thisYear = cDBConvert.ToInteger(drv["CALENDAR_YEAR"]);
            thisQuarter = cDBConvert.ToInteger(drv["QUARTER"]);
            if (thisYear < SecondYear || (thisYear == SecondYear && thisQuarter < SecondQuarter))
              if (thisYear > FirstYear || (thisYear == FirstYear && thisQuarter > FirstQuarter))
              {
                if (!FoundFF2LRec)//if this is the first one found
                  thisFF2LRecFound = drv;
                FoundFF2LRec = true;
                if (thisYear > cDBConvert.ToInteger(thisFF2LRecFound["CALENDAR_YEAR"]) || (thisYear == cDBConvert.ToInteger(thisFF2LRecFound["CALENDAR_YEAR"]) && thisQuarter > cDBConvert.ToInteger(thisFF2LRecFound["QUARTER"])))
                  thisFF2LRecFound = drv;//take the later of the two                                
              }
          }
          int EndYear = cDBConvert.ToInteger(thisFF2LRecFound["CALENDAR_YEAR"]);
          int EndQtr = cDBConvert.ToInteger(thisFF2LRecFound["QUARTER"]);
          Category.SetCheckParameter("FF2L_PEI_End_Year_Quarter", EndYear.ToString() + EndQtr.ToString(), eParameterDataType.String);
          DataView OpSuppRecs = Category.GetCheckParameter("Operating_Supp_Data_Records_by_Location").ValueAsDataView();
          DataView OpSuppRecsFound;
          FilterFF2LRecs = new sFilterPair[3];
          FilterFF2LRecs[0].Set("MON_SYS_ID", MonSysID);
          sFilterPair[] FilterOpSupp = new sFilterPair[5];
          FilterOpSupp[0].Set("OP_TYPE_CD", "OPHOURS,OSHOURS", eFilterPairStringCompare.InList);
          FilterOpSupp[1].Set("FUEL_CD", cDBConvert.ToString(Category.GetCheckParameter("Current_Fuel_Flow_Record").ValueAsDataRowView()["FUEL_CD"]));
          FilterOpSupp[2].Set("OP_VALUE", 168, eFilterDataType.Integer, eFilterPairRelativeCompare.GreaterThanOrEqual);
          for (int i = BegYear; i <= EndYear; i++)
          {
            int thisYearsFirstQ = 1;
            if (i == BegYear)
              thisYearsFirstQ = BegQtr;
            for (int j = thisYearsFirstQ; j <= 4 && !(j > EndQtr && i == EndYear); j++)
            {
              FoundFF2LRec = false;
              FilterFF2LRecs[1].Set("CALENDAR_YEAR", i, eFilterDataType.Integer);
              FilterFF2LRecs[2].Set("QUARTER", j, eFilterDataType.Integer);
              FF2LTestRecsFound = FindRows(FF2LTestRecs, FilterFF2LRecs);
              if (FF2LTestRecsFound.Count > 0)
              {
                FoundFF2LRec = true;
                thisFF2LRecFound = FF2LTestRecsFound[0];
              }
              if (FoundFF2LRec)
              {
                if (cDBConvert.ToString(thisFF2LRecFound["TEST_RESULT_CD"]) == "FEW168H")
                {
                  FilterOpSupp[3].Set("CALENDAR_YEAR", i, eFilterDataType.Integer);
                  FilterOpSupp[4].Set("QUARTER", j, eFilterDataType.Integer);
                  OpSuppRecsFound = FindRows(OpSuppRecs, FilterOpSupp);
                  if (OpSuppRecsFound.Count > 0)
                  {
                    DataView FF2LBaselineRecs = Category.GetCheckParameter("FF2L_Baseline_Records_By_Location_For_QA_Status").ValueAsDataView();
                    sFilterPair[] FilterFF2LBASRecs = new sFilterPair[1];
                    FilterFF2LBASRecs[0].Set("MON_SYS_ID", MonSysID);
                    DataView FF2LBaselineRecsFound = FindRows(FF2LBaselineRecs, FilterFF2LBASRecs);
                    bool FoundFF2LBASRec = false;
                    int BaselineEndYear, BaselineEndQtr;
                    foreach (DataRowView drv in FF2LBaselineRecsFound)
                    {
                      BaselineEndYear = cDBConvert.ToDate(drv["END_DATE"], DateTypes.END).Year;
                      BaselineEndQtr = cDBConvert.ToDate(drv["END_DATE"], DateTypes.END).Quarter();
                      if (BaselineEndYear == i && BaselineEndQtr == j)
                      {
                        FoundFF2LBASRec = true;
                        break;
                      }
                    }
                    if (!FoundFF2LBASRec)
                    {
                      FoundTestNum = cDBConvert.ToString(thisFF2LRecFound["TEST_NUM"]);
                      Category.SetCheckParameter("Invalid_FF2L_Test_Number", FoundTestNum, eParameterDataType.String);
                      Status = "OOC-Invalid Fuel Flow to Load Test";
                      Category.SetCheckParameter("Current_PEI_Status", Status, eParameterDataType.String);
                      Category.CheckCatalogResult = Status;
                      return ReturnVal;
                    }
                  }
                }
              }
              else
              {
                FilterOpSupp[3].Set("CALENDAR_YEAR", i, eFilterDataType.Integer);
                FilterOpSupp[4].Set("QUARTER", j, eFilterDataType.Integer);
                OpSuppRecsFound = FindRows(OpSuppRecs, FilterOpSupp);
                if (OpSuppRecsFound.Count > 0)
                {
                  Category.SetCheckParameter("Missing_FF2L_Year_Quarter", i.ToString() + " Q" + j.ToString(), eParameterDataType.String);
                  Status = "Undetermined-Missing Fuel Flow to Load Test";
                  Category.SetCheckParameter("Current_PEI_Status", Status, eParameterDataType.String);
                  Category.CheckCatalogResult = Status;
                  return ReturnVal;
                }
              }
            }
          }
          if (Status != "")
            Category.SetCheckParameter("Current_PEI_Status", Status, eParameterDataType.String);
        }
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "ADESTAT11");
      }

      return ReturnVal;
    }

    public string ADESTAT12(cCategory Category, ref bool Log)
    //Determine PEI Test Expiration Date       
    {
      string ReturnVal = "";

      try
      {
        Category.SetCheckParameter("PEI_Missing_Op_Data_Info", null, eParameterDataType.Boolean);
        string Status = Category.GetCheckParameter("Current_PEI_Status").ValueAsString();

        if (Category.GetCheckParameter("PEI_Required").ValueAsBool() && Status == "")
        {
          DataRowView PriorPEIRec = Category.GetCheckParameter("Prior_PEI_Record").ValueAsDataRowView();
          DateTime PriorExpDate = cDBConvert.ToDate(PriorPEIRec["TEST_EXP_DATE"], DateTypes.END);
          DateTime PriorEndDate = cDBConvert.ToDate(PriorPEIRec["END_DATE"], DateTypes.END);
          DateTime PriorTestExpDate = cDBConvert.ToDate(PriorPEIRec["TEST_EXP_DATE"], DateTypes.END);

          DateTime CurrentDate = Category.GetCheckParameter("Current_Operating_Date").ValueAsDateTime(DateTypes.END);
          int PriorYear = PriorEndDate.Year;
          int PriorQtr = cDateFunctions.ThisQuarter(PriorEndDate);
          int CurrentYear = CurrentDate.Year;
          int CurrentQtr = cDateFunctions.ThisQuarter(CurrentDate);
          int FirstQtr;
          bool PEIEligible = Category.GetCheckParameter("FF2L_PEI_Eligible").ValueAsBool();

          int BegYr = 9999;
          int BegQtr = 4;
          int EndYr = 0;
          int EndQtr = 1;
          string BegYrQtr = Category.GetCheckParameter("FF2L_PEI_Begin_Year_Quarter").ValueAsString();
          string EndYrQtr = Category.GetCheckParameter("FF2L_PEI_End_Year_Quarter").ValueAsString();
          if (BegYrQtr != "" & EndYrQtr != "")
          {
            BegYr = int.Parse(BegYrQtr.Substring(0, 4));
            BegQtr = int.Parse(BegYrQtr.Substring(4, 1));
            EndYr = int.Parse(EndYrQtr.Substring(0, 4));
            EndQtr = int.Parse(EndYrQtr.Substring(4, 1));
          }

          DataRowView CurrentFFRecord = Category.GetCheckParameter("Current_Fuel_Flow_Record").ValueAsDataRowView();
          string FuelCd = cDBConvert.ToString(CurrentFFRecord["FUEL_CD"]);
          bool IsOSOReporter;
          DataView LocRptFreqRecords = Category.GetCheckParameter("Location_Reporting_Frequency_Records").ValueAsDataView();
          sFilterPair[] LocRptFilter = new sFilterPair[1];
          LocRptFilter[0].Set("REPORT_FREQ_CD", "OS");
          DataView LocRptFreqRecsFound = FindRows(LocRptFreqRecords, LocRptFilter);
          string thisBegYrQtr;
          string thisEndYrQtr;
          int thisBegYr;
          int thisEndYr;
          int thisBegQtr;
          int thisEndQtr;
          for (int i = PriorYear; i <= CurrentYear; i++)
          {
            if (i == PriorYear)
              if (PriorQtr == 4)
                continue;//begin with PriorYear+1
              else
                FirstQtr = PriorQtr + 1;
            else
              FirstQtr = 1;
            for (int j = FirstQtr; j <= 4 && !(j == CurrentQtr && i == CurrentYear); j++)
            {
              IsOSOReporter = false;
              foreach (DataRowView drv in LocRptFreqRecsFound)
              {
                thisBegYrQtr = cDBConvert.ToString(drv["BEGIN_QUARTER"]);
                thisEndYrQtr = cDBConvert.ToString(drv["END_QUARTER"]);
                thisBegYr = 0;
                thisEndYr = 0;
                thisBegQtr = 0;
                thisEndQtr = 0;
                if (thisBegYrQtr.Length > 9)
                {
                  thisBegYr = int.Parse(thisBegYrQtr.Substring(0, 4));
                  thisBegQtr = int.Parse(thisBegYrQtr.Substring(9, 1));
                }
                if (thisEndYrQtr.Length > 9)
                {
                  thisEndYr = int.Parse(thisEndYrQtr.Substring(0, 4));
                  thisEndQtr = int.Parse(thisEndYrQtr.Substring(9, 1));
                }
                if ((thisBegQtr <= j && thisBegQtr != 0 && thisBegYr == i) && (thisEndYrQtr == "" || thisEndYr > i || (thisEndQtr >= j && thisEndYr == i)))
                {
                  IsOSOReporter = true;
                  break;
                }
              }
              if (!IsOSOReporter || j == 3)
              {
                if (Category.GetCheckParameter("FF2L_PEI_Eligible").ParameterValue != null && PEIEligible && (i > BegYr || (i == BegYr && j >= BegQtr)) && (i < EndYr || (i == EndYr && j <= EndQtr)))
                  PriorExpDate = cDateFunctions.LastDateThisQuarter(PriorExpDate.AddMonths(3));
              }
              else
                if (IsOSOReporter && j == 2)
                  if (Category.GetCheckParameter("FF2L_PEI_Eligible").ParameterValue != null && PEIEligible && (i > BegYr || (i == BegYr && j >= BegQtr)) && (i < EndYr || (i == EndYr && j <= EndQtr)))
                    PriorExpDate = cDateFunctions.LastDateThisQuarter(PriorExpDate.AddMonths(9));
            }
          }
          if (CurrentDate > PriorExpDate)
          {
            if (Category.GetCheckParameter("PEI_Missing_Op_Data_Info").ParameterValue != null)
              Status = "Missing Op Data";
            else
              if (Category.GetCheckParameter("FF2L_PEI_Eligible").ParameterValue != null && !PEIEligible)
                Status = "OOC-PEI Test Expired-Fuel Flow To Load Test Ignored";
              else
                Status = "OOC-PEI Test Expired";
            Category.CheckCatalogResult = Status;
          }
          else
            Status = "IC-Extension";
        }
        if (Status != "")
          Category.SetCheckParameter("Current_PEI_Status", Status, eParameterDataType.String);
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "ADESTAT12");
      }

      return ReturnVal;
    }

    public string ADESTAT13(cCategory Category, ref bool Log)
    //Determine System Appendix D Status       
    {
      string ReturnVal = "";

      try
      {
        string AppDStatus = Category.GetCheckParameter("Current_Appendix_D_Status").ValueAsString();
        string AccStatus = Category.GetCheckParameter("Current_Accuracy_Status").ValueAsString();
        string PEIStatus = Category.GetCheckParameter("Current_PEI_Status").ValueAsString();
        if (AppDStatus == "OOC-Multiple Reasons" || (AppDStatus.StartsWith("OOC") && AccStatus.StartsWith("OOC") && AppDStatus != AccStatus))
          AppDStatus = "OOC-Multiple Reasons";
        else
          if (AppDStatus.StartsWith("OOC"))
          { }//do nothing
          else
            if (AccStatus.StartsWith("OOC"))
              AppDStatus = AccStatus;
            else
              if (!AppDStatus.StartsWith("IC") && !AppDStatus.StartsWith("Undetermined") && !AppDStatus.EndsWith("Not Yet Evaluated") &&
                  !AccStatus.StartsWith("IC") && !AccStatus.StartsWith("Undetermined") && !AccStatus.EndsWith("Not Yet Evaluated") &&
                  AppDStatus != AccStatus && AppDStatus != "")
                AppDStatus = "Invalid Data";
              else
                if (!AppDStatus.StartsWith("IC") && !AppDStatus.StartsWith("Undetermined") && !AppDStatus.EndsWith("Not Yet Evaluated") && AppDStatus != "")
                { }//do nothing
                else
                  if (!AccStatus.StartsWith("IC") && !AccStatus.StartsWith("Undetermined") && !AccStatus.EndsWith("Not Yet Evaluated"))
                    AppDStatus = AccStatus;
                  else
                    if (AppDStatus.EndsWith("Not Yet Evaluated") || AccStatus.EndsWith("Not Yet Evaluated"))
                      AppDStatus = "Test Not Yet Evaluated";
                    else
                      if (AppDStatus.StartsWith("Undetermined") || AccStatus.StartsWith("Undetermined"))
                        AppDStatus = "Undetermined";
                      else
                        if (AppDStatus == "IC-Extension" || AccStatus == "IC-Extension")
                          AppDStatus = "IC-Extension";
                        else
                          AppDStatus = "IC";
        if (Category.GetCheckParameter("PEI_Required").ValueAsBool())
          if (AppDStatus == "OOC-Multiple Reasons" || (AppDStatus.StartsWith("OOC") && PEIStatus.StartsWith("OOC") && AppDStatus != PEIStatus))
            AppDStatus = "OOC-Multiple Reasons";
          else
            if (AppDStatus.StartsWith("OOC"))
            { }//do nothing
            else
              if (PEIStatus.StartsWith("OOC"))
                AppDStatus = PEIStatus;
              else
                if (!AppDStatus.StartsWith("IC") && !AppDStatus.StartsWith("Undetermined") && !AppDStatus.EndsWith("Not Yet Evaluated") &&
                    !PEIStatus.StartsWith("IC") && !PEIStatus.StartsWith("Undetermined") && !PEIStatus.EndsWith("Not Yet Evaluated") &&
                    AppDStatus != PEIStatus && AppDStatus != "")
                  AppDStatus = "Invalid Data";
                else
                  if (!AppDStatus.StartsWith("IC") && !AppDStatus.StartsWith("Undetermined") && !AppDStatus.EndsWith("Not Yet Evaluated") && AppDStatus != "")
                  { }//do nothing
                  else
                    if ((!PEIStatus.StartsWith("IC") && !PEIStatus.StartsWith("Undetermined") && !PEIStatus.EndsWith("Not Yet Evaluated")))
                      AppDStatus = PEIStatus;
                    else
                      if (AppDStatus.EndsWith("Not Yet Evaluated") || PEIStatus.EndsWith("Not Yet Evaluated"))
                        AppDStatus = "Test Not Yet Evaluated";
                      else
                        if (AppDStatus.StartsWith("Undetermined") || PEIStatus.StartsWith("Undetermined"))
                          AppDStatus = "Undetermined";
                        else
                          if (AppDStatus == "IC-Extension" || PEIStatus == "IC-Extension")
                            AppDStatus = "IC-Extension";
                          else
                            AppDStatus = "IC";
        if (AppDStatus != "")
          Category.SetCheckParameter("Current_Appendix_D_Status", AppDStatus, eParameterDataType.String);
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "ADESTAT13");
      }

      return ReturnVal;
    }

    #endregion

    #region Helper Methods

    public static void CheckExistenceOfValidFuelFlowTest(cCategory Category, out bool ValidFuelFlowTestExistsForEachComponent , out DateTime CertificationCheckDate)
    // Search for the existence of a valid fuel flow test
    {
      DataRowView PriorAccRec = Category.GetCheckParameter("PRIOR_ACCURACY_RECORD").ValueAsDataRowView();

      // initialize outputs
      ValidFuelFlowTestExistsForEachComponent  = true;
      CertificationCheckDate = cDBConvert.ToDate(PriorAccRec["END_DATE"], DateTypes.START);     
      if(cDBConvert.ToDate(PriorAccRec["REINSTALLATION_DATE"], DateTypes.START) > CertificationCheckDate)
      {
        CertificationCheckDate = cDBConvert.ToDate(PriorAccRec["REINSTALLATION_DATE"], DateTypes.START);
      }

      DataRowView CurrentFFRec = Category.GetCheckParameter("Current_Fuel_Flow_Record").ValueAsDataRowView();
      DateTime CurrentDate = Category.GetCheckParameter("Current_Operating_Date").ValueAsDateTime(DateTypes.END);
      DataView FFCompRecs = Category.GetCheckParameter("Fuel_Flow_Component_Records").ValueAsDataView();
      DataView AccTestRecs = Category.GetCheckParameter("Accuracy_Test_Records_By_Location_For_QA_Status").ValueAsDataView();
      DataView AccTestRecsFound;
      sFilterPair[] FilterAccTest = new sFilterPair[1];
      DataView PEITestRecs = Category.GetCheckParameter("PEI_Test_Records_By_Location_For_QA_Status").ValueAsDataView();
      DataView PEITestRecsFound;
      sFilterPair[] FilterPEITest = new sFilterPair[1];
      string CompID;

      // loop through the Fuel_Flow_Component_Records
      foreach (DataRowView drv in FFCompRecs)
      {
        CompID = cDBConvert.ToString(drv["COMPONENT_ID"]);
        FilterAccTest[0].Set("COMPONENT_ID", CompID);
        AccTestRecsFound = FindRows(AccTestRecs, FilterAccTest);

        // find the latest AccuracyTestRecord
        DateTime tmpLatestAccDate;
        bool AccFound = false;
        DateTime latestAccTestDate = DateTime.MinValue;
        DataRowView latestAccTestRec = null;
        foreach (DataRowView drv2 in AccTestRecsFound)
        {
          // get the latest between END_DATE and REINSTALLATION_DATE
          tmpLatestAccDate = cDBConvert.ToDate(drv2["END_DATE"], DateTypes.START);
          if (cDBConvert.ToDate(drv2["REINSTALLATION_DATE"], DateTypes.START) > cDBConvert.ToDate(drv2["END_DATE"], DateTypes.START))
          {
            tmpLatestAccDate = cDBConvert.ToDate(drv2["REINSTALLATION_DATE"], DateTypes.START);
          }

          if (tmpLatestAccDate.Year < CurrentDate.Year || 
              (tmpLatestAccDate.Year == CurrentDate.Year && tmpLatestAccDate.Quarter() < CurrentDate.Quarter()))
          {
            AccFound = true;
            if (tmpLatestAccDate > latestAccTestDate)
            {
              latestAccTestRec = drv2;
              latestAccTestDate = tmpLatestAccDate;
            }
          }
        }

        if (!AccFound || latestAccTestRec == null)
        {
          // exit early
          ValidFuelFlowTestExistsForEachComponent = false;
          break;
        }
        if (latestAccTestRec["TEST_RESULT_CD"].AsString() != "PASSED")
        {
          // exit early 
          ValidFuelFlowTestExistsForEachComponent = false;
          break;
        }
        else if (latestAccTestDate > cDBConvert.ToDate(CurrentFFRec["SYSTEM_BEGIN_DATE"], DateTypes.START))
        {
          // exit early 
          ValidFuelFlowTestExistsForEachComponent = false;
          break;
        }
        else
        {
          if (latestAccTestDate > CertificationCheckDate)
          {
            CertificationCheckDate = latestAccTestDate;
          }
        }

        if (latestAccTestRec["TEST_TYPE_CD"].AsString() == "FFACCTT")
        {
          FilterPEITest[0].Set("COMPONENT_ID", CompID);
          PEITestRecsFound = FindRows(AccTestRecs, FilterAccTest);

          // find the latest PEITestRecord
          DateTime tmpLatestPEIDate;
          bool PEIFound = false;
          DateTime latestPEIDate = DateTime.MinValue;
          DataRowView latestPEIRec = null;
          foreach (DataRowView drv2 in AccTestRecsFound)
          {
            // get the latest between END_DATE and REINSTALL_DATE
            tmpLatestPEIDate = cDBConvert.ToDate(drv2["END_DATE"], DateTypes.START);

            if (tmpLatestPEIDate.Year < CurrentDate.Year ||
                (tmpLatestPEIDate.Year == CurrentDate.Year && tmpLatestPEIDate.Quarter() < CurrentDate.Quarter()))
            {
              PEIFound = true;
              if (tmpLatestPEIDate > latestPEIDate)
              {
                latestPEIDate = tmpLatestPEIDate;
                latestPEIRec = drv2;
              }
            }
          }

          // PEI Record was found if latestPEIRecord is not null
          if (!PEIFound || latestPEIRec == null)
          {
            // exit early 
            ValidFuelFlowTestExistsForEachComponent = false;
            break;
          }
          else if (latestPEIRec["TEST_RESULT_CD"].AsString() != "PASSED")
          {
            // exit early 
            ValidFuelFlowTestExistsForEachComponent = false;
            break;
          }
          else if (latestPEIDate > cDBConvert.ToDate(CurrentFFRec["SYSTEM_BEGIN_DATE"], DateTypes.START))
          {
            // exit early 
            ValidFuelFlowTestExistsForEachComponent = false;
            break;
          }
          else
          {
            if (latestPEIDate > CertificationCheckDate)
            {
              CertificationCheckDate = latestPEIDate;
            }
          }
        }
      }
    }
    
    #endregion
  }
}