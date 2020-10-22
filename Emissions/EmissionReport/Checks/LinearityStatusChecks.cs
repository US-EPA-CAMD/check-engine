using System;
using System.Data;
using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.CheckEngine.SpecialParameterClasses;
using ECMPS.Checks.Data.Ecmps.CheckEm.Function;
using ECMPS.Checks.Data.Ecmps.Dbo.View;
using ECMPS.Checks.EmissionsReport;
using ECMPS.Checks.Em.Parameters;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Extensions;


namespace ECMPS.Checks.EmissionsChecks
{
    public class cLinearityStatusChecks : cEmissionsChecks
    {
        #region Constructors

        public cLinearityStatusChecks(cEmissionsReportProcess emissionReportProcess)
            : base(emissionReportProcess)
        {
            CheckProcedures = new dCheckProcedure[8];

            CheckProcedures[1] = new dCheckProcedure(LINSTAT1);
            CheckProcedures[2] = new dCheckProcedure(LINSTAT2);
            CheckProcedures[3] = new dCheckProcedure(LINSTAT3);
            CheckProcedures[4] = new dCheckProcedure(LINSTAT4);
            CheckProcedures[5] = new dCheckProcedure(LINSTAT5);
            CheckProcedures[6] = new dCheckProcedure(LINSTAT6);
            CheckProcedures[7] = new dCheckProcedure(LINSTAT7);
        }

        //        /// <summary>
        //    /// Constructor used for testing.
        //    /// </summary>
        //    /// <param name="mpManualParameters"></param>
        public cLinearityStatusChecks(cEmissionsCheckParameters emManualParameters)
        {
            EmManualParameters = emManualParameters;
        }

        #endregion

        #region Public Static Methods: Checks

        public static string LINSTAT1(cCategory Category, ref bool Log)
        //Check Analyzer Range Exemption For Linearity Status       
        {
            string ReturnVal = "";

            try
            {
                string LinStat = null;
                if (Convert.ToString(Category.GetCheckParameter("Current_MHV_Parameter").ParameterValue).InList("SO2C,NOXC"))
                {
                    DataView MonitorSpanRecsFound;
                    sFilterPair[] SpanFilter = new sFilterPair[2];
                    SpanFilter[0].Set("COMPONENT_TYPE_CD", EmParameters.QaStatusComponentTypeCode);
                    SpanFilter[1].Set("SPAN_SCALE_CD", EmParameters.CurrentAnalyzerRangeUsed);
                    MonitorSpanRecsFound = FindRows(EmParameters.MonitorSpanRecordsByHourLocation.SourceView, SpanFilter);
                    if (MonitorSpanRecsFound.Count != 1 || cDBConvert.ToDecimal(MonitorSpanRecsFound[0]["SPAN_VALUE"]) <= 0)
                        LinStat = "Invalid Monitor Span";
                    else
                    {
                        if (cDBConvert.ToDecimal(MonitorSpanRecsFound[0]["SPAN_VALUE"]) <= 30)
                            LinStat = "IC-Exempt";
                    }
                }
                Category.SetCheckParameter("Current_Linearity_Status", LinStat, eParameterDataType.String);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "LINSTAT1");
            }

            return ReturnVal;
        }

        public static string LINSTAT2(cCategory Category, ref bool Log)
        //Locate Most Recent Prior Linearity Test       
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("Prior_Linearity_Record", null, eParameterDataType.DataRowView);
                Category.SetCheckParameter("Invalid_Linearity_Record", null, eParameterDataType.DataRowView);
                if (Category.GetCheckParameter("Current_Linearity_Status").ParameterValue == null)
                {
                    string ApplCompId = Convert.ToString(Category.GetCheckParameter("Applicable_Component_ID").ParameterValue);
                    string AnalyzerRangeUsed = Convert.ToString(Category.GetCheckParameter("Current_Analyzer_Range_Used").ParameterValue);
                    DataView LinTestRecs = Category.GetCheckParameter("Linearity_Test_Records_By_Location_For_QA_Status").ValueAsDataView();
                    DataView PriorLinTestRecsFound;
                    DateTime CurrentDate = EmParameters.CurrentDateHour.AsStartDate();
                    int CurrentHour = EmParameters.CurrentDateHour.AsStartHour();

                    sFilterPair[] Filter = new sFilterPair[3];
                    Filter[0].Set("COMPONENT_ID", ApplCompId);
                    Filter[1].Set("SPAN_SCALE_CD", AnalyzerRangeUsed);
                    Filter[2].Set("TEST_RESULT_CD", "INVALID", true);

                    if (CurrentHour != 0)
                        PriorLinTestRecsFound = FindActiveRows(LinTestRecs, DateTime.MinValue, 0, CurrentDate, CurrentHour - 1, "END_DATE", "END_HOUR", "END_DATE", "END_HOUR", Filter);
                    else
                        PriorLinTestRecsFound = FindActiveRows(LinTestRecs, DateTime.MinValue, 0, CurrentDate.AddDays(-1), 23, "END_DATE", "END_HOUR", "END_DATE", "END_HOUR", Filter);
                    DataRowView FirstFoundRec = null;
                    DataRowView SecondFoundRec = null;
                    DataRowView MostRecentRec = null;
                    if (PriorLinTestRecsFound.Count != 0)
                    {
                        PriorLinTestRecsFound.Sort = "END_DATE DESC, END_HOUR DESC, END_MIN DESC";
                        FirstFoundRec = PriorLinTestRecsFound[0];
                    }

                    sFilterPair[] Filter2 = new sFilterPair[6];
                    Filter2[0].Set("COMPONENT_ID", ApplCompId);
                    Filter2[1].Set("SPAN_SCALE_CD", AnalyzerRangeUsed);
                    Filter2[2].Set("TEST_RESULT_CD", "PASSED,PASSAPS", eFilterPairStringCompare.InList);
                    Filter2[3].Set("END_DATE", CurrentDate, eFilterDataType.DateEnded);
                    Filter2[4].Set("END_HOUR", CurrentHour, eFilterDataType.Integer);
                    Filter2[5].Set("END_MIN", 45, eFilterDataType.Integer, eFilterPairRelativeCompare.LessThan);
                    PriorLinTestRecsFound = FindRows(LinTestRecs, Filter2);
                    if (PriorLinTestRecsFound.Count != 0)
                    {
                        PriorLinTestRecsFound.Sort = "END_DATE DESC, END_HOUR DESC, END_MIN DESC";
                        SecondFoundRec = PriorLinTestRecsFound[0];
                    }

                    if (FirstFoundRec != null && SecondFoundRec != null)
                    {
                        DateTime FirstBegDate = cDBConvert.ToDate(FirstFoundRec["BEGIN_DATE"], DateTypes.START);
                        DateTime SecondBegDate = cDBConvert.ToDate(SecondFoundRec["BEGIN_DATE"], DateTypes.START);
                        int FirstBegHour = cDBConvert.ToHour(FirstFoundRec["BEGIN_HOUR"], DateTypes.START);
                        int SecondBegHour = cDBConvert.ToHour(SecondFoundRec["BEGIN_HOUR"], DateTypes.START);

                        if (FirstBegDate > SecondBegDate ||
                            (FirstBegDate == SecondBegDate && (FirstBegHour > SecondBegHour || (FirstBegHour == SecondBegHour && cDBConvert.ToInteger(FirstFoundRec["BEGIN_MIN"]) > cDBConvert.ToInteger(SecondFoundRec["BEGIN_MIN"])))))
                            MostRecentRec = FirstFoundRec;
                        else
                            MostRecentRec = SecondFoundRec;
                    }
                    else
                        if (FirstFoundRec != null)
                        MostRecentRec = FirstFoundRec;
                    else
                            if (SecondFoundRec != null)
                        MostRecentRec = SecondFoundRec;

                    DataView InvalidLinTestRecsFound;
                    Filter[2].Set("TEST_RESULT_CD", "INVALID");
                    if (MostRecentRec != null)
                    {
                        Category.SetCheckParameter("Prior_Linearity_Record", MostRecentRec, eParameterDataType.DataRowView);
                        DateTime PriorDate = cDBConvert.ToDate(MostRecentRec["END_DATE"], DateTypes.END);
                        int PriorHour = cDBConvert.ToHour(MostRecentRec["END_HOUR"], DateTypes.END);
                        if (CurrentHour != 0)
                            if (PriorHour != 23)
                                InvalidLinTestRecsFound = FindActiveRows(LinTestRecs, PriorDate, PriorHour + 1, CurrentDate, CurrentHour - 1, "END_DATE", "END_HOUR", "END_DATE", "END_HOUR", Filter);
                            else
                                InvalidLinTestRecsFound = FindActiveRows(LinTestRecs, PriorDate.AddDays(1), 0, CurrentDate, CurrentHour - 1, "END_DATE", "END_HOUR", "END_DATE", "END_HOUR", Filter);
                        else
                            if (PriorHour != 23)
                            InvalidLinTestRecsFound = FindActiveRows(LinTestRecs, PriorDate, PriorHour + 1, CurrentDate.AddDays(-1), 23, "END_DATE", "END_HOUR", "END_DATE", "END_HOUR", Filter);
                        else
                            InvalidLinTestRecsFound = FindActiveRows(LinTestRecs, PriorDate.AddDays(1), 0, CurrentDate.AddDays(-1), 23, "END_DATE", "END_HOUR", "END_DATE", "END_HOUR", Filter);
                        if (InvalidLinTestRecsFound.Count > 0)
                        {
                            InvalidLinTestRecsFound.Sort = "END_DATE DESC, END_HOUR DESC, END_MIN DESC";
                            Category.SetCheckParameter("Invalid_Linearity_Record", InvalidLinTestRecsFound[0], eParameterDataType.DataRowView);
                        }
                    }
                    else
                    {
                        if (CurrentHour != 0)
                            InvalidLinTestRecsFound = FindActiveRows(LinTestRecs, DateTime.MinValue, 0, CurrentDate, CurrentHour - 1, "END_DATE", "END_HOUR", "END_DATE", "END_HOUR", Filter);
                        else
                            InvalidLinTestRecsFound = FindActiveRows(LinTestRecs, DateTime.MinValue, 0, CurrentDate.AddDays(-1), 23, "END_DATE", "END_HOUR", "END_DATE", "END_HOUR", Filter);
                        if (InvalidLinTestRecsFound.Count > 0)
                        {
                            InvalidLinTestRecsFound.Sort = "END_DATE DESC, END_HOUR DESC, END_MIN DESC";
                            Category.SetCheckParameter("Invalid_Linearity_Record", InvalidLinTestRecsFound[0], eParameterDataType.DataRowView);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "LINSTAT2");
            }

            return ReturnVal;
        }

        public static string LINSTAT3(cCategory Category, ref bool Log)
        //Locate Most Recent Prior Event       
        {
            string ReturnVal = "";

            try
            {
                DataRowView PriorLinEventRec = null;
                if (Category.GetCheckParameter("Current_Linearity_Status").ParameterValue == null)
                {
                    //all QACE
                    DataView QACertEventRecs = Category.GetCheckParameter("QA_Certification_Event_Records").ValueAsDataView();
                    DataTable QACertEventRecsTable = QACertEventRecs.Table.Copy();
                    string ApplCompId = Convert.ToString(Category.GetCheckParameter("Applicable_Component_ID").ParameterValue);
                    string ApplSysIds = Convert.ToString(Category.GetCheckParameter("Applicable_System_IDs").ParameterValue);
                    string AnalyzerRangeUsed = Convert.ToString(Category.GetCheckParameter("Current_Analyzer_Range_Used").ParameterValue);
                    //filter QACE - helper method has dates
                    DataView QACertRecsFound = FindCertEventRecs3(QACertEventRecsTable, "Prior_Linearity_Record", ApplCompId, AnalyzerRangeUsed, Category);
                    DateTime PriorLinRecDate = DateTime.MinValue;
                    int PriorLinRecHour = int.MinValue;

                    if (QACertRecsFound != null && QACertRecsFound.Count > 0)
                    {
                        QACertRecsFound.Sort = "QA_CERT_EVENT_DATE DESC, QA_CERT_EVENT_HOUR DESC";
                        DataView MonSpanRecs = Category.GetCheckParameter("Monitor_Span_Records_By_Hour_Location").ValueAsDataView();
                        DataView MonSpanRecsFound;
                        sFilterPair[] MonSpanFilter = new sFilterPair[2];
                        string CompTypeCd = EmParameters.QaStatusComponentTypeCode;

                        foreach (DataRowView drv in QACertRecsFound)
                        {
                            PriorLinEventRec = drv;
                            if (cDBConvert.ToString(PriorLinEventRec["QA_CERT_EVENT_CD"]) == "170" &&
                                Convert.ToBoolean(Category.GetCheckParameter("Dual_Range_Status").ValueAsBool()))
                            {
                                PriorLinRecDate = cDBConvert.ToDate(PriorLinEventRec["QA_CERT_EVENT_DATE"], DateTypes.START);
                                PriorLinRecHour = cDBConvert.ToHour(PriorLinEventRec["QA_CERT_EVENT_HOUR"], DateTypes.START);
                                MonSpanFilter[0].Set("COMPONENT_TYPE_CD", CompTypeCd);
                                MonSpanFilter[1].Set("SPAN_SCALE_CD", AnalyzerRangeUsed);
                                MonSpanRecsFound = FindActiveRows(MonSpanRecs, PriorLinRecDate, PriorLinRecHour, PriorLinRecDate, PriorLinRecHour, "BEGIN_DATE", "BEGIN_HOUR", "BEGIN_DATE", "BEGIN_HOUR", MonSpanFilter);
                                if (MonSpanRecsFound.Count > 0)
                                    break;
                                else
                                    PriorLinEventRec = null;
                            }
                            else
                                break;
                        }
                    }
                    if (PriorLinEventRec == null)
                    {
                        if (Category.GetCheckParameter("Prior_Linearity_Record").ParameterValue == null)
                            Category.SetCheckParameter("Current_Linearity_Status", "OOC-No Prior Test or Event", eParameterDataType.String);
                    }
                    else
                    {
                        DataRowView InvLinRec = Category.GetCheckParameter("Invalid_Linearity_Record").ValueAsDataRowView();
                        if (InvLinRec != null)
                        {
                            DateTime InvLinRecDate = cDBConvert.ToDate(InvLinRec["END_DATE"], DateTypes.START);
                            int InvLinRecHour = cDBConvert.ToHour(InvLinRec["END_HOUR"], DateTypes.START);
                            if (PriorLinRecDate > InvLinRecDate || (PriorLinRecDate == InvLinRecDate || PriorLinRecHour > InvLinRecHour))
                            {
                                DataView LinTestRecs = Category.GetCheckParameter("Linearity_Test_Records_By_Location_For_QA_Status").ValueAsDataView();
                                LinTestRecs.Sort = "END_DATE, END_HOUR, END_MIN";
                                DataView PriorLinTestRecsFound;
                                sFilterPair[] LinTestFilter = new sFilterPair[3];
                                LinTestFilter[0].Set("COMPONENT_ID", ApplCompId);
                                LinTestFilter[1].Set("SPAN_SCALE_CD", AnalyzerRangeUsed);
                                LinTestFilter[2].Set("TEST_RESULT_CD", "INVALID");
                                PriorLinTestRecsFound = FindActiveRows(LinTestRecs, PriorLinRecDate, PriorLinRecHour, DateTime.Now, DateTime.Now.Hour, "END_DATE", "END_HOUR", "END_DATE", "END_HOUR", LinTestFilter);
                                if (PriorLinTestRecsFound.Count > 0)
                                {
                                    PriorLinTestRecsFound.Sort = "END_DATE, END_HOUR, END_MIN";
                                    Category.SetCheckParameter("Invalid_Linearity_Record", PriorLinTestRecsFound[0], eParameterDataType.DataRowView);
                                }
                                else
                                    Category.SetCheckParameter("Invalid_Linearity_Record", null, eParameterDataType.DataRowView);
                            }
                        }
                    }
                }
                Category.SetCheckParameter("Prior_Linearity_Event_Record", PriorLinEventRec, eParameterDataType.DataRowView);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "LINSTAT3");
            }

            return ReturnVal;
        }

        public static string LINSTAT4(cCategory Category, ref bool Log)
        //Determine Expiration Dates For Most Recent Prior Linearity Test       
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("Linearity_Missing_Op_Data_Info", null, eParameterDataType.String);
                string LinStatus = cDBConvert.ToString(Category.GetCheckParameter("Current_Linearity_Status").ParameterValue);
                DataRowView PriorLinEventRec = Category.GetCheckParameter("Prior_Linearity_Event_Record").ValueAsDataRowView();
                DataRowView PriorLinRec = Category.GetCheckParameter("Prior_Linearity_Record").ValueAsDataRowView();
                if (LinStatus == "" && PriorLinRec != null && PriorLinEventRec == null)
                {
                    Category.SetCheckParameter("Check_For_Ignored_Linearity", true, eParameterDataType.Boolean);
                    DateTime PriorTestExpDate;
                    DateTime PriorTestExpDateWExt;
                    bool MissingOpData = false;

                    DataView CertEventRecs = Category.GetCheckParameter("QA_Certification_Event_Records").ValueAsDataView();
                    DataView CertRecsFound;
                    DataRowView CertRec;
                    DateTime CertRecLastTestComplDate;
                    sFilterPair[] CertEventFilter;

                    DateTime PriorTestEndDate = cDBConvert.ToDate(PriorLinRec["END_DATE"], DateTypes.END);
                    int PriorTestEndHour = cDBConvert.ToHour(PriorLinRec["END_HOUR"], DateTypes.END);
                    DateTime PriorTestBegDate = cDBConvert.ToDate(PriorLinRec["BEGIN_DATE"], DateTypes.START);
                    int PriorTestBegHour = cDBConvert.ToHour(PriorLinRec["BEGIN_HOUR"], DateTypes.START);

                    if (cDBConvert.ToString(PriorLinRec["QA_NEEDS_EVAL_FLG"]) == "Y")
                        LinStatus = "Prior Test Not Yet Evaluated";
                    else
                    {
                        string TestResCd = cDBConvert.ToString(PriorLinRec["TEST_RESULT_CD"]);
                        if (TestResCd == "" || TestResCd.InList("FAILED,ABORTED"))
                        {
                            CertEventRecs = Category.GetCheckParameter("QA_Certification_Event_Records").ValueAsDataView();
                            DataTable QACertEventRecsTable = CertEventRecs.Table.Copy();
                            string ApplCompId = Convert.ToString(Category.GetCheckParameter("Applicable_Component_ID").ParameterValue);
                            string ApplSysIds = Convert.ToString(Category.GetCheckParameter("Applicable_System_IDs").ParameterValue);
                            string AnalyzerRangeUsed = Convert.ToString(Category.GetCheckParameter("Current_Analyzer_Range_Used").ParameterValue);
                            CertRecsFound = FindCertEventRecs2(QACertEventRecsTable, "Prior_Linearity_Record", ApplCompId, AnalyzerRangeUsed, Category);

                            if (CertRecsFound != null && CertRecsFound.Count > 0)
                            {
                                CertRecsFound.Sort = "QA_CERT_EVENT_DATE DESC, QA_CERT_EVENT_HOUR DESC";
                                PriorLinEventRec = CertRecsFound[0];
                                Category.SetCheckParameter("Prior_Linearity_Event_Record", PriorLinEventRec, eParameterDataType.DataRowView);
                            }
                            else
                                if (TestResCd == "")
                                LinStatus = "OOC-Test Has Critical Errors";
                            else
                                    if (TestResCd == "FAILED")
                                LinStatus = "OOC-Test Failed";
                            else
                                        if (TestResCd == "ABORTED")
                                LinStatus = "OOC-Test Aborted";
                        }
                        else
                        {
                            PriorTestExpDate = cDBConvert.ToDate(PriorLinRec["TEST_EXP_DATE"], DateTypes.START);
                            PriorTestExpDateWExt = cDBConvert.ToDate(PriorLinRec["TEST_EXP_DATE_WITH_EXT"], DateTypes.START);
                            DataRowView CurrentMHVRecord = (DataRowView)Category.GetCheckParameter("Current_MHV_Record").ParameterValue;
                            bool AnnRptReq = Category.GetCheckParameter("Annual_Reporting_Requirement").ValueAsBool();
                            if (PriorTestExpDate == DateTime.MinValue)
                            {
                                if (!AnnRptReq)
                                {
                                    int EndQtr = cDBConvert.ToInteger(PriorLinRec["QUARTER"]);
                                    DateTime PriorLinRecEndDate = cDBConvert.ToDate(PriorLinRec["BEGIN_DATE"], DateTypes.START);
                                    int PriorTestExpDateYear = PriorLinRecEndDate.Year;
                                    if (EndQtr == 2)
                                    {
                                        if (PriorLinRecEndDate.Month > 7)
                                            PriorTestExpDateYear++;
                                        PriorTestExpDate = new DateTime(PriorTestExpDateYear, 7, 30);
                                    }
                                    else
                                    {
                                        if (PriorLinRecEndDate.Month > 4)
                                            PriorTestExpDateYear++;
                                        PriorTestExpDate = new DateTime(PriorTestExpDateYear, 4, 30);
                                    }
                                }
                                else
                                {
                                    //Added 11/11/2014 RAB
                                    DateTime? AlternateTestDate = null;
                                    if (EmParameters.PriorLinearityRecord.ComponentTypeCd == "HG")
                                    {
                                        cFilterCondition[] ProgramFilter = new cFilterCondition[] { new cFilterCondition("PRG_CD", "MATS") };

                                        DataRowView LatestProgramRec = cRowFilter.FindMostRecentRow(EmParameters.LocationProgramRecordsByHourLocation.SourceView,
                                          EmParameters.QaStatusComponentBeginDate.Default(DateTypes.START),
                                          "EMISSIONS_RECORDING_BEGIN_DATE",
                                          ProgramFilter, eFilterConditionRelativeCompare.LessThanOrEqual);

                                        if (LatestProgramRec != null && LatestProgramRec["EMISSIONS_RECORDING_BEGIN_DATE"] != null)
                                            AlternateTestDate = LatestProgramRec["EMISSIONS_RECORDING_BEGIN_DATE"].AsDateTime().GetValueOrDefault();
                                    }

                                    CertEventRecs = (DataView)Category.GetCheckParameter("Qa_Certification_Event_Records").ParameterValue;
                                    CertEventFilter = new sFilterPair[2];

                                    DateTime PriorRecBeginDate = cDBConvert.ToDate(PriorLinRec["BEGIN_DATE"], DateTypes.START);
                                    int PriorRecBeginHour = cDBConvert.ToHour(PriorLinRec["BEGIN_HOUR"], DateTypes.START);
                                    CertEventFilter[0].Set("COMPONENT_ID", EmParameters.ApplicableComponentId);
                                    CertEventFilter[1].Set("LINEARITY_REQUIRED", "Y");
                                    CertRecsFound = FindActiveRows(CertEventRecs, DateTime.MinValue, 0, PriorRecBeginDate, PriorRecBeginHour, "QA_CERT_EVENT_DATE", "QA_CERT_EVENT_HOUR", "QA_CERT_EVENT_DATE", "QA_CERT_EVENT_HOUR", CertEventFilter);

                                    /* 
									 * Set CertRec to most recent QaCertificationEventRecord where:
									 * 
									 * 1) LinearityRequired is equal to 'Y'
									 * 2) BeginDate is prior to PriorLinearityRecord.BeginDateHour
									 * 3) LinearityCertEvent is equal to 'Y'
									 * 4) ConditionalDataBeginDate is null
									 * 5) CompletionTestDateHour is after PriorLinearityRecord.EndDateHour
									 * 
									 */
                                    {
                                        if (CertRecsFound.Count > 0)
                                        {
                                            CertRecsFound.Sort = "QA_CERT_EVENT_DATE DESC, QA_CERT_EVENT_HOUR DESC";
                                            CertRec = CertRecsFound[0];
                                            CertRecLastTestComplDate = cDBConvert.ToDate(CertRec["LAST_TEST_COMPLETED_DATE"], DateTypes.START);
                                        }
                                        else
                                        {
                                            CertRec = null;
                                            CertRecLastTestComplDate = DateTime.MaxValue;
                                        }
                                    }

                                    if ((CertRec != null) &&
                                        ((cDBConvert.ToString(CertRec["LINEARITY_CERT_EVENT"]) == "Y") &&
                                        (CertRec["CONDITIONAL_DATA_BEGIN_DATE"] == DBNull.Value) &&
                                        (CertRecLastTestComplDate > PriorTestEndDate || (CertRecLastTestComplDate == PriorTestEndDate &&
                                         cDBConvert.ToHour(CertRec["LAST_TEST_COMPLETED_HOUR"], DateTypes.START) > PriorTestEndHour))))
                                    {
                                        if (AlternateTestDate == null)
                                        {
                                            PriorTestExpDate = cDateFunctions.LastDateThisQuarter(cDateFunctions.StartDateNextQuarter(CertRecLastTestComplDate));
                                        }
                                        else
                                        {
                                            DateTime?[] DateList = { CertRecLastTestComplDate, AlternateTestDate };
                                            DateTime LaterDate = cDateFunctions.LatestDate(DateList).GetValueOrDefault();
                                            PriorTestExpDate = cDateFunctions.LastDateThisQuarter(cDateFunctions.StartDateNextQuarter(LaterDate));
                                        }
                                    }
                                    else if (cDBConvert.ToInteger(PriorLinRec["GP_IND"]) == 1)
                                    {
                                        if (AlternateTestDate == null)
                                        {
                                            PriorTestExpDate = cDateFunctions.LastDateThisQuarter(PriorTestEndDate);
                                        }
                                        else
                                        {
                                            DateTime?[] DateList = { PriorTestEndDate, AlternateTestDate };
                                            DateTime LaterDate = cDateFunctions.LatestDate(DateList).GetValueOrDefault();
                                            PriorTestExpDate = cDateFunctions.LastDateThisQuarter(LaterDate);
                                        }
                                    }
                                    else
                                    {
                                        if (AlternateTestDate == null)
                                        {
                                            PriorTestExpDate = cDateFunctions.LastDateThisQuarter(cDateFunctions.StartDateNextQuarter(PriorTestEndDate));
                                        }
                                        else
                                        {
                                            DateTime?[] DateList = { PriorTestEndDate, AlternateTestDate };
                                            DateTime LaterDate = cDateFunctions.LatestDate(DateList).GetValueOrDefault();
                                            PriorTestExpDate = cDateFunctions.LastDateThisQuarter(cDateFunctions.StartDateNextQuarter(LaterDate));

                                        }
                                    }
                                }
                                PriorLinRec["TEST_EXP_DATE"] = PriorTestExpDate;
                            }
                            DateTime CurrentDate = EmParameters.CurrentDateHour.AsStartDate();
                            int CurrentHour = EmParameters.CurrentDateHour.AsStartHour();

                            if (CurrentDate <= PriorTestExpDate)
                                LinStatus = "IC";
                            else
                                if (!AnnRptReq)
                                LinStatus = "OOC-Expired";
                            else
                            {
                                int YearPriorToCurrentQtr = CurrentDate.Year;
                                int QtrPriorToCurrentQtr = cDateFunctions.ThisQuarter(CurrentDate);
                                if (QtrPriorToCurrentQtr == 1)
                                {
                                    QtrPriorToCurrentQtr = 4;
                                    YearPriorToCurrentQtr--;
                                }
                                else
                                    QtrPriorToCurrentQtr--;
                                DataView OpSuppDataRecs = Category.GetCheckParameter("Operating_Supp_Data_Records_by_Location").ValueAsDataView();
                                DataView OpSuppDataRecsFound;
                                sFilterPair[] OpSuppFilter;
                                DateTime EarliestLocRptDate = Category.GetCheckParameter("Earliest_Location_Report_Date").ValueAsDateTime(DateTypes.START);
                                if (PriorTestExpDateWExt == DateTime.MinValue)
                                {
                                    int NumExtQtrs = 0;
                                    int PriorTestYear = PriorTestExpDate.Year;
                                    int PriorTestQtr = cDateFunctions.ThisQuarter(PriorTestExpDate);

                                    OpSuppFilter = new sFilterPair[3];
                                    OpSuppFilter[0].Set("OP_TYPE_CD", "OPHOURS");
                                    DataView TEERecs = Category.GetCheckParameter("Test_Extension_Exemption_Records").ValueAsDataView();
                                    DataView TEERecsFound;
                                    sFilterPair[] TEEFilterRange = new sFilterPair[5];
                                    sFilterPair[] TEEFilterPB = new sFilterPair[4];
                                    TEEFilterRange[0].Set("COMPONENT_ID", Category.GetCheckParameter("Applicable_Component_ID").ValueAsString());
                                    TEEFilterRange[1].Set("EXTENS_EXEMPT_CD", "RANGENU");
                                    TEEFilterRange[2].Set("SPAN_SCALE_CD", Category.GetCheckParameter("Current_Analyzer_Range_Used").ValueAsString());
                                    TEEFilterPB[0].Set("COMPONENT_ID", Category.GetCheckParameter("Applicable_Component_ID").ValueAsString());
                                    TEEFilterPB[1].Set("EXTENS_EXEMPT_CD", "NONQAPB");
                                    DataView LocRptFreqRecords = Category.GetCheckParameter("Location_Reporting_Frequency_Records").ValueAsDataView();
                                    sFilterPair[] LocRptFilter = new sFilterPair[1];
                                    LocRptFilter[0].Set("REPORT_FREQ_CD", "OS");
                                    DataView LocRptFreqRecsFound = FindRows(LocRptFreqRecords, LocRptFilter);
                                    string thisEndYrQtr, thisBeginYrQtr;
                                    int thisEndYr, thisBeginYr, thisEndQtr, thisBeginQtr;
                                    bool FoundLocRptFrqRec = false;
                                    bool StopLooking = false;

                                    int? operatingHoursCount;

                                    int j;
                                    DateTime LastDateThisQtr;
                                    for (int i = PriorTestYear; i <= YearPriorToCurrentQtr; i++)
                                    {
                                        j = 1;
                                        if (i == PriorTestYear)
                                            j = PriorTestQtr;
                                        do
                                        {
                                            if (NumExtQtrs == 3)
                                                StopLooking = true;
                                            else
                                            {
                                                LastDateThisQtr = cDateFunctions.LastDateThisQuarter(i, j);
                                                if (EarliestLocRptDate > LastDateThisQtr)
                                                    NumExtQtrs++;
                                                else
                                                {
                                                    /* Selects operating hour counts based on system if Primary/Primary-Bypass systems (stacks) are invovled. */
                                                    if ((EmParameters.PrimaryBypassActiveForHour == true) && EmParameters.QaStatusComponentTypeCode.InList("CO2,NOX,O2"))
                                                    {
                                                        CheckDataView<SystemOpSuppData>  systemOpSuppData 
                                                            = EmParameters.SystemOperatingSuppDataRecordsByLocation.FindRows(new cFilterCondition("MON_SYS_ID", EmParameters.QaStatusPrimaryOrPrimaryBypassSystemId),
                                                                                                                             new cFilterCondition("CALENDAR_YEAR", i),
                                                                                                                             new cFilterCondition("QUARTER", j),
                                                                                                                             new cFilterCondition("OP_SUPP_DATA_TYPE_CD", "OP"));

                                                        if (systemOpSuppData.Count > 0)
                                                            operatingHoursCount = systemOpSuppData[0].Hours;
                                                        else
                                                            operatingHoursCount = null;
                                                    }
                                                    else
                                                    {
                                                        OpSuppFilter[1].Set("CALENDAR_YEAR", i, eFilterDataType.Integer);
                                                        OpSuppFilter[2].Set("QUARTER", j, eFilterDataType.Integer);
                                                        OpSuppDataRecsFound = FindRows(OpSuppDataRecs, OpSuppFilter);

                                                        if (OpSuppDataRecsFound.Count > 0)
                                                            operatingHoursCount = cDBConvert.ToInteger(OpSuppDataRecsFound[0]["OP_VALUE"]);
                                                        else
                                                            operatingHoursCount = null;
                                                    }

                                                    if ((operatingHoursCount != null) && (operatingHoursCount.Value < 168))
                                                        NumExtQtrs++;
                                                    else if (EmParameters.PriorLinearityRecord.ComponentTypeCd != "HG")
                                                    {
                                                        TEEFilterRange[3].Set("CALENDAR_YEAR", i, eFilterDataType.Integer);
                                                        TEEFilterRange[4].Set("QUARTER", j, eFilterDataType.Integer);
                                                        TEERecsFound = FindRows(TEERecs, TEEFilterRange);
                                                        if (TEERecsFound.Count > 0)
                                                            NumExtQtrs++;
                                                        else
                                                        {
                                                            TEEFilterPB[2].Set("CALENDAR_YEAR", i, eFilterDataType.Integer);
                                                            TEEFilterPB[3].Set("QUARTER", j, eFilterDataType.Integer);
                                                            TEERecsFound = FindRows(TEERecs, TEEFilterPB);
                                                            if (TEERecsFound.Count > 0)
                                                                NumExtQtrs++;
                                                            else
                                                                if (operatingHoursCount == null)
                                                            {
                                                                if (j == 1 || j == 4)
                                                                {
                                                                    foreach (DataRowView drv in LocRptFreqRecsFound)
                                                                    {
                                                                        thisBeginYrQtr = cDBConvert.ToString(drv["BEGIN_QUARTER"]);
                                                                        thisBeginYr = 0;
                                                                        thisBeginQtr = 0;
                                                                        if (thisBeginYrQtr.Length > 9)
                                                                        {
                                                                            thisBeginYr = int.Parse(thisBeginYrQtr.Substring(0, 4));
                                                                            thisBeginQtr = int.Parse(thisBeginYrQtr.Substring(9, 1));
                                                                        }
                                                                        thisEndYrQtr = cDBConvert.ToString(drv["END_QUARTER"]);
                                                                        thisEndYr = 0;
                                                                        thisEndQtr = 0;
                                                                        if (thisEndYrQtr.Length > 9)
                                                                        {
                                                                            thisEndYr = int.Parse(thisEndYrQtr.Substring(0, 4));
                                                                            thisEndQtr = int.Parse(thisEndYrQtr.Substring(9, 1));
                                                                        }
                                                                        if ((thisBeginYr < i || (thisBeginYr == i && thisBeginQtr <= j)) &&
                                                                           (thisEndYr == 0 || thisEndYr > i || (thisEndYr == i && thisEndQtr >= j)))
                                                                        {
                                                                            if (AnnRptReq && j == 4 && thisEndYrQtr != "" && thisEndYr == i)
                                                                                NumExtQtrs++;
                                                                            FoundLocRptFrqRec = true;
                                                                            break;
                                                                        }
                                                                    }
                                                                    if (!FoundLocRptFrqRec)
                                                                    {
                                                                        Category.SetCheckParameter("Linearity_Missing_Op_Data_Info", i + "Q" + j, eParameterDataType.String);
                                                                        MissingOpData = true;
                                                                        StopLooking = true;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    Category.SetCheckParameter("Linearity_Missing_Op_Data_Info", i + "Q" + j, eParameterDataType.String);
                                                                    MissingOpData = true;
                                                                    StopLooking = true;
                                                                }
                                                            }
                                                            else
                                                                StopLooking = true;
                                                        }
                                                    }
                                                }
                                            }
                                            j++;
                                        } while (!StopLooking && j <= 4 && !(i == YearPriorToCurrentQtr && j > QtrPriorToCurrentQtr));
                                        if (StopLooking)
                                            break;
                                    }

                                    if (EmParameters.PriorLinearityRecord.ComponentTypeCd != "HG")
                                    {
                                        DateTime start = new DateTime(PriorTestYear, 3 * (PriorTestQtr - 1) + 1, 1);
                                        DateTime newStart = start.AddMonths(3 * NumExtQtrs);
                                        DateTime newEnd = (new DateTime(YearPriorToCurrentQtr, 3 * (QtrPriorToCurrentQtr - 1) + 1, 1)).AddMonths(3).AddDays(-1);

                                        for (DateTime date = newStart; date <= newEnd; date = date.AddMonths(3))
                                        {
                                            int quarter = cDateFunctions.ThisQuarter(date);

                                            TEEFilterPB[1].Set("EXTENS_EXEMPT_CD", "NONQAPB");
                                            TEEFilterPB[2].Set("CALENDAR_YEAR", date.Year, eFilterDataType.Integer);
                                            TEEFilterPB[3].Set("QUARTER", quarter, eFilterDataType.Integer);
                                            TEERecsFound = FindRows(TEERecs, TEEFilterPB);
                                            if (TEERecsFound.Count > 0)
                                                NumExtQtrs++;

                                        }
                                    }

                                    PriorTestExpDateWExt = PriorTestExpDate;
                                    for (int i = 0; i < NumExtQtrs; i++)
                                        PriorTestExpDateWExt = cDateFunctions.LastDateThisQuarter(cDateFunctions.StartDateNextQuarter(PriorTestExpDateWExt));
                                    PriorLinRec["TEST_EXP_DATE_WITH_EXT"] = PriorTestExpDateWExt;
                                }
                                else
                                    PriorLinRec["TEST_EXP_DATE_WITH_EXT"] = PriorTestExpDate;

                                if (CurrentDate <= PriorTestExpDateWExt)
                                    LinStatus = "IC-Extension";
                                else
                                    if (MissingOpData)
                                {
                                    LinStatus = "Missing Op Data";
                                    PriorLinRec["TEST_EXP_DATE_WITH_EXT"] = DBNull.Value;
                                }
                                else
                                {
                                    int currentOpHours;

                                    /* Selects operating hour counts based on system if Primary/Primary-Bypass systems (stacks) are invovled. */
                                    if ((EmParameters.PrimaryBypassActiveForHour == true) && EmParameters.QaStatusComponentTypeCode.InList("CO2,NOX,O2"))
                                    {
                                        currentOpHours = EmParameters.SystemOperatingSuppDataDictionaryArray[Category.CurrentMonLocPos][EmParameters.QaStatusPrimaryOrPrimaryBypassSystemId].QuarterlyOperatingCounts.Hours;
                                    }
                                    else
                                    {
                                        int CurrentPosition = Category.GetCheckParameter("Current_Monitor_Plan_Location_Postion").ValueAsInt();
                                        int[] OpHrsAccumArray = Category.GetCheckParameter("Rpt_Period_Op_Hours_Accumulator_Array").ValueAsIntArray();

                                        currentOpHours = OpHrsAccumArray[CurrentPosition];
                                    }

                                    if (currentOpHours == -1)
                                        LinStatus = "Invalid Op Data";
                                    else
                                    {
                                        int GraceOpHrs = currentOpHours;

                                        if (GraceOpHrs > 168)
                                            LinStatus = "OOC-Expired";
                                        else
                                        {
                                            int FirstQtr = cDateFunctions.ThisQuarter(PriorTestExpDateWExt) + 1;
                                            int FirstYear = PriorTestExpDateWExt.Year;
                                            int EarlyLocRptDateQtr = cDateFunctions.ThisQuarter(EarliestLocRptDate);
                                            if ((PriorTestExpDateWExt >= EarliestLocRptDate && ((FirstYear > YearPriorToCurrentQtr || (FirstYear == YearPriorToCurrentQtr && FirstQtr > QtrPriorToCurrentQtr)))) ||
                                                (PriorTestExpDateWExt < EarliestLocRptDate && ((EarliestLocRptDate.Year > YearPriorToCurrentQtr || (EarliestLocRptDate.Year == YearPriorToCurrentQtr && EarlyLocRptDateQtr > QtrPriorToCurrentQtr)))))
                                                LinStatus = "IC-Grace";
                                            else
                                            {
                                                bool Quit = false;

                                                int? operatingHoursCount;

                                                int j;

                                                OpSuppFilter = new sFilterPair[4];
                                                OpSuppFilter[0].Set("OP_TYPE_CD", "OPHOURS");
                                                OpSuppFilter[1].Set("FUEL_CD", DBNull.Value, eFilterDataType.String);

                                                for (int i = FirstYear; i <= YearPriorToCurrentQtr; i++)
                                                {
                                                    j = 1;
                                                    if (i == FirstYear)
                                                        j = FirstQtr;
                                                    while (j <= 4 && !Quit && !(i == YearPriorToCurrentQtr && j > QtrPriorToCurrentQtr))
                                                    {
                                                        if (EarliestLocRptDate <= cDateFunctions.LastDateThisQuarter(i, j))
                                                        {
                                                            /* Selects operating hour counts based on system if Primary/Primary-Bypass systems (stacks) are invovled. */
                                                            if ((EmParameters.PrimaryBypassActiveForHour == true) && EmParameters.QaStatusComponentTypeCode.InList("CO2,NOX,O2"))
                                                            {
                                                                CheckDataView<SystemOpSuppData> systemOpSuppData
                                                                    = EmParameters.SystemOperatingSuppDataRecordsByLocation.FindRows(new cFilterCondition("MON_SYS_ID", EmParameters.QaStatusPrimaryOrPrimaryBypassSystemId),
                                                                                                                                     new cFilterCondition("CALENDAR_YEAR", i),
                                                                                                                                     new cFilterCondition("QUARTER", j),
                                                                                                                                     new cFilterCondition("OP_SUPP_DATA_TYPE_CD", "OP"));

                                                                if (systemOpSuppData.Count > 0)
                                                                    operatingHoursCount = systemOpSuppData[0].Hours;
                                                                else
                                                                    operatingHoursCount = null;
                                                            }
                                                            else
                                                            {
                                                                OpSuppFilter[2].Set("CALENDAR_YEAR", i, eFilterDataType.Integer);
                                                                OpSuppFilter[3].Set("QUARTER", j, eFilterDataType.Integer);
                                                                OpSuppDataRecsFound = FindRows(OpSuppDataRecs, OpSuppFilter);

                                                                if (OpSuppDataRecsFound.Count > 0)
                                                                    operatingHoursCount = cDBConvert.ToInteger(OpSuppDataRecsFound[0]["OP_VALUE"]);
                                                                else
                                                                    operatingHoursCount = null;
                                                            }

                                                            if (operatingHoursCount.HasValue)
                                                            {
                                                                GraceOpHrs += operatingHoursCount.Value;
                                                                if (GraceOpHrs > 168)
                                                                {
                                                                    LinStatus = "OOC-Expired";
                                                                    Quit = true;
                                                                    break;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                Category.SetCheckParameter("Linearity_Missing_Op_Data_Info", i + "Q" + j, eParameterDataType.String);
                                                                LinStatus = "Missing Op Data";
                                                                Quit = true;
                                                                break;
                                                            }
                                                        }
                                                        j++;
                                                    }
                                                    if (Quit)
                                                        break;
                                                }
                                                if (LinStatus == "")
                                                    LinStatus = "IC-Grace";
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (LinStatus != "")
                    Category.SetCheckParameter("Current_Linearity_Status", LinStatus, eParameterDataType.String);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "LINSTAT4");
            }

            return ReturnVal;
        }

        public static string LINSTAT5(cCategory Category, ref bool Log)
        //Determine Event Conditional Status       
        {
            string ReturnVal = "";

            try
            {
                string LinStatus = Category.GetCheckParameter("Current_Linearity_Status").ValueAsString();
                DataRowView PriorLinEventRec = Category.GetCheckParameter("Prior_Linearity_Event_Record").ValueAsDataRowView();
                DataRowView SubsqLinRec = null;

                if (LinStatus == "" && PriorLinEventRec != null)
                {
                    DateTime CondBeginDate = cDBConvert.ToDate(PriorLinEventRec["CONDITIONAL_DATA_BEGIN_DATE"], DateTypes.START);
                    int CondBeginHour = cDBConvert.ToHour(PriorLinEventRec["CONDITIONAL_DATA_BEGIN_HOUR"], DateTypes.START);
                    DateTime CurrentDate = EmParameters.CurrentDateHour.AsStartDate();
                    int CurrentHour = EmParameters.CurrentDateHour.AsStartHour();
                    int CurrentYear = CurrentDate.Year;
                    int CurrentQtr = cDateFunctions.ThisQuarter(CurrentDate);
                    if (CondBeginDate == DateTime.MinValue || PriorLinEventRec["CONDITIONAL_DATA_BEGIN_HOUR"] == DBNull.Value ||
                        (CurrentDate < CondBeginDate || (CurrentDate == CondBeginDate && CurrentHour < CondBeginHour)))
                        LinStatus = "OOC-Event";
                    else
                    {
                        string ApplCompId = Convert.ToString(Category.GetCheckParameter("Applicable_Component_ID").ParameterValue);
                        string AnalyzerRangeUsed = Category.GetCheckParameter("Current_Analyzer_Range_Used").ValueAsString(); ;
                        DataView LinTestRecs = Category.GetCheckParameter("Linearity_Test_Records_By_Location_For_QA_Status").ValueAsDataView();
                        DataView LinTestRecsFound;

                        sFilterPair[] LinTestRecsFilter = new sFilterPair[3];
                        LinTestRecsFilter[0].Set("COMPONENT_ID", ApplCompId);
                        LinTestRecsFilter[1].Set("SPAN_SCALE_CD", AnalyzerRangeUsed);
                        LinTestRecsFilter[2].Set("TEST_RESULT_CD", "INVALID", true);
                        LinTestRecsFound = FindActiveRows(LinTestRecs, CondBeginDate, CondBeginHour, DateTime.MaxValue, 23, "END_DATE", "END_HOUR", "END_DATE", "END_HOUR", LinTestRecsFilter);
                        if (LinTestRecsFound.Count > 0)
                        {
                            LinTestRecsFound.Sort = "END_DATE ASC, END_HOUR ASC, END_MIN ASC";
                            SubsqLinRec = LinTestRecsFound[0];

                            if (cDBConvert.ToString(SubsqLinRec["QA_NEEDS_EVAL_FLG"]) == "Y")
                                LinStatus = "Recertification Test Not Yet Evaluated";
                            else
                            {
                                string TestResCd = cDBConvert.ToString(SubsqLinRec["TEST_RESULT_CD"]);
                                if (TestResCd == "")
                                    LinStatus = "OOC-Recertification Test Has Critical Errors";
                                else
                                    if (TestResCd == "FAILED")
                                    LinStatus = "OOC-Recertification Test Failed";
                                else
                                        if (TestResCd == "ABORTED")
                                    LinStatus = "OOC-Recertification Test Aborted";
                            }
                            if (Category.GetCheckParameter("Invalid_Linearity_Record").ValueAsDataRowView() == null)
                            {
                                LinTestRecsFilter[2].Set("TEST_RESULT_CD", "INVALID");
                                LinTestRecsFound = FindActiveRows(LinTestRecs, CondBeginDate, CondBeginHour, cDBConvert.ToDate(SubsqLinRec["END_DATE"], DateTypes.END), cDBConvert.ToHour(SubsqLinRec["END_HOUR"], DateTypes.END), "END_DATE", "END_HOUR", "END_DATE", "END_HOUR", LinTestRecsFilter);
                                if (LinTestRecsFound.Count > 0)
                                {
                                    DataRowView InvLinRec = LinTestRecsFound[0];
                                    Category.SetCheckParameter("Invalid_Linearity_Record", InvLinRec, eParameterDataType.DataRowView);
                                }
                            }
                        }

                        if ((PriorLinEventRec["SYS_TYPE_CD"].AsString() == "HG") && PriorLinEventRec["QA_CERT_EVENT_CD"].AsString().InList("100,101,120,125"))
                        {
                            // Conditional data checking for HG (non sorbent trap) systems (components) for events 100, 101, 120 and 125
                            // occurs in MATSMHV 19 and 20 because both HGSI3 and HGLINE test must occur for these events.  The linearity
                            // status checks will except either test.
                            LinStatus = "IC-Skip Duplicate Checking for Hg Conditional Data";
                        }

                        bool AnnRptReq = Category.GetCheckParameter("Annual_Reporting_Requirement").ValueAsBool();
                        if (LinStatus == "" && !AnnRptReq)
                            if (SubsqLinRec != null)
                            {
                                DateTime LinTestEndDate = cDBConvert.ToDate(SubsqLinRec["END_DATE"], DateTypes.END);
                                int LinTestEndHour = cDBConvert.ToHour(SubsqLinRec["END_HOUR"], DateTypes.END);
                                DateTime OctDate = new DateTime(CurrentYear, 10, 30);
                                if (LinTestEndDate > OctDate)
                                    LinStatus = "OOC-Conditional Period Expired";
                            }
                            else
                                if (cDateFunctions.ThisQuarter(CurrentDate) == 3)
                                LinStatus = "OOC-Conditional Period Expired";
                        if (LinStatus == "")
                        {
                            string EventId = cDBConvert.ToString(PriorLinEventRec["QA_CERT_EVENT_ID"]);
                            string EventCd = cDBConvert.ToString(PriorLinEventRec["QA_CERT_EVENT_CD"]);
                            DateTime CertEventDate = cDBConvert.ToDate(PriorLinEventRec["QA_CERT_EVENT_DATE"], DateTypes.START);
                            DataView OpSuppDataRecs = Category.GetCheckParameter("Operating_Supp_Data_Records_by_Location").ValueAsDataView();
                            DataView OpSuppDataRecsFound;
                            sFilterPair[] OpSuppFilter;

                            //Time values needed in several places below                            
                            int YearPriorToCurrentQtr = CurrentYear;//temp, adjusted below
                            int QtrPriorToCurrentQtr = cDateFunctions.ThisQuarter(CurrentDate);
                            int CertEventYear = CertEventDate.Year;
                            int CertEventQtr = cDateFunctions.ThisQuarter(CertEventDate);
                            if (QtrPriorToCurrentQtr == 1)
                            {
                                QtrPriorToCurrentQtr = 4;
                                YearPriorToCurrentQtr--;
                            }
                            else
                                QtrPriorToCurrentQtr--;

                            int[] OpHrsAccumArray = Category.GetCheckParameter("Rpt_Period_Op_Hours_Accumulator_Array").ValueAsIntArray();
                            int[] OpDaysAccumArray = Category.GetCheckParameter("Rpt_Period_Op_Days_Accumulator_Array").ValueAsIntArray();
                            int CurrentPosition = Category.GetCheckParameter("Current_Monitor_Plan_Location_Postion").ValueAsInt();
                            DateTime EarliestLocRptDate = Category.GetCheckParameter("Earliest_Location_Report_Date").ValueAsDateTime(DateTypes.START);
                            if (cDBConvert.ToString(PriorLinEventRec["LINEARITY_CERT_EVENT"]) == "Y" && EmParameters.PriorLinearityEventRecord.SysTypeCd.NotInList("ST"))
                            {
                                if (EventCd == "125")
                                {
                                    if (PriorLinEventRec["MON_SYS_ID"] == DBNull.Value)
                                        LinStatus = "Invalid Certification Event";
                                    else
                                    {
                                        DateTime SysBeginDate = cDBConvert.ToDate(PriorLinEventRec["SYS_BEGIN_DATE"], DateTypes.START);
                                        if (SysBeginDate == DateTime.MinValue)
                                            LinStatus = "Invalid Monitor System";
                                        else
                                        {
                                            string SysTypeCd = cDBConvert.ToString(PriorLinEventRec["SYS_TYPE_CD"]);
                                            DataView LocProgramRecs = Category.GetCheckParameter("Location_Program_Records_By_Hour_Location").ValueAsDataView();
                                            DataView LocProgramRecsFound;
                                            sFilterPair[] LocProgFilter;
                                            if (SysTypeCd == "SO2")
                                            {
                                                LocProgFilter = new sFilterPair[1];
                                                LocProgFilter[0].Set("PRG_CD", EmParameters.ProgramRequiresSo2SystemCertificationList, eFilterPairStringCompare.InList);
                                                LocProgramRecsFound = FindActiveRows(LocProgramRecs, DateTime.MinValue, SysBeginDate, "UNIT_MONITOR_CERT_BEGIN_DATE", "UNIT_MONITOR_CERT_BEGIN_DATE", true, true, LocProgFilter);
                                                if (LocProgramRecsFound.Count == 0)
                                                {
                                                    LocProgFilter = new sFilterPair[2];
                                                    LocProgFilter[0].Set("PRG_CD", EmParameters.ProgramRequiresSo2SystemCertificationList, eFilterPairStringCompare.InList);
                                                    LocProgFilter[1].Set("EMISSIONS_RECORDING_BEGIN_DATE", DateTime.MinValue, eFilterDataType.DateBegan, true);
                                                    LocProgramRecsFound = FindActiveRows(LocProgramRecs, DateTime.MinValue, SysBeginDate, "EMISSIONS_RECORDING_BEGIN_DATE", "EMISSIONS_RECORDING_BEGIN_DATE", true, true, LocProgFilter);
                                                }
                                            }
                                            else
                                                if (SysTypeCd == "NOX")
                                            {
                                                LocProgFilter = new sFilterPair[1];
                                                LocProgFilter[0].Set("PRG_CD", EmParameters.ProgramRequiresNoxSystemCertificationList, eFilterPairStringCompare.InList);
                                                LocProgramRecsFound = FindActiveRows(LocProgramRecs, DateTime.MinValue, SysBeginDate, "UNIT_MONITOR_CERT_BEGIN_DATE", "UNIT_MONITOR_CERT_BEGIN_DATE", true, true, LocProgFilter);
                                                if (LocProgramRecsFound.Count == 0)
                                                {
                                                    LocProgFilter = new sFilterPair[2];
                                                    LocProgFilter[0].Set("PRG_CD", EmParameters.ProgramRequiresNoxSystemCertificationList, eFilterPairStringCompare.InList);
                                                    LocProgFilter[1].Set("EMISSIONS_RECORDING_BEGIN_DATE", DateTime.MinValue, eFilterDataType.DateBegan, true);
                                                    LocProgramRecsFound = FindActiveRows(LocProgramRecs, DateTime.MinValue, SysBeginDate, "EMISSIONS_RECORDING_BEGIN_DATE", "EMISSIONS_RECORDING_BEGIN_DATE", true, true, LocProgFilter);
                                                }
                                            }
                                            else
                                                    if (SysTypeCd == "NOXC")
                                            {
                                                LocProgFilter = new sFilterPair[1];
                                                LocProgFilter[0].Set("PRG_CD", EmParameters.ProgramRequiresNoxcSystemCertificationList, eFilterPairStringCompare.InList);
                                                LocProgramRecsFound = FindActiveRows(LocProgramRecs, DateTime.MinValue, SysBeginDate, "UNIT_MONITOR_CERT_BEGIN_DATE", "UNIT_MONITOR_CERT_BEGIN_DATE", true, true, LocProgFilter);
                                                if (LocProgramRecsFound.Count == 0)
                                                {
                                                    LocProgFilter = new sFilterPair[2];
                                                    LocProgFilter[0].Set("PRG_CD", EmParameters.ProgramRequiresNoxcSystemCertificationList, eFilterPairStringCompare.InList);
                                                    LocProgFilter[1].Set("EMISSIONS_RECORDING_BEGIN_DATE", DateTime.MinValue, eFilterDataType.DateBegan, true);
                                                    LocProgramRecsFound = FindActiveRows(LocProgramRecs, DateTime.MinValue, SysBeginDate, "EMISSIONS_RECORDING_BEGIN_DATE", "EMISSIONS_RECORDING_BEGIN_DATE", true, true, LocProgFilter);
                                                }
                                            }
                                            else
                                                        if (SysTypeCd == "HG")
                                            {
                                                LocProgFilter = new sFilterPair[1];
                                                LocProgFilter[0].Set("PRG_CD", "MATS", eFilterPairStringCompare.InList);
                                                LocProgramRecsFound = FindActiveRows(LocProgramRecs, DateTime.MinValue, SysBeginDate, "UNIT_MONITOR_CERT_BEGIN_DATE", "UNIT_MONITOR_CERT_BEGIN_DATE", true, true, LocProgFilter);
                                                if (LocProgramRecsFound.Count == 0)
                                                {
                                                    LocProgFilter = new sFilterPair[2];
                                                    LocProgFilter[0].Set("PRG_CD", "MATS", eFilterPairStringCompare.InList);
                                                    LocProgFilter[1].Set("EMISSIONS_RECORDING_BEGIN_DATE", DateTime.MinValue, eFilterDataType.DateBegan, true);
                                                    LocProgramRecsFound = FindActiveRows(LocProgramRecs, DateTime.MinValue, SysBeginDate, "EMISSIONS_RECORDING_BEGIN_DATE", "EMISSIONS_RECORDING_BEGIN_DATE", true, true, LocProgFilter);
                                                }
                                            }

                                            else
                                            {
                                                LocProgramRecsFound = FindActiveRows(LocProgramRecs, DateTime.MinValue, SysBeginDate, "UNIT_MONITOR_CERT_BEGIN_DATE", "UNIT_MONITOR_CERT_BEGIN_DATE", true, true);
                                                if (LocProgramRecsFound.Count == 0)
                                                {
                                                    LocProgFilter = new sFilterPair[1];
                                                    LocProgFilter[0].Set("EMISSIONS_RECORDING_BEGIN_DATE", DateTime.MinValue, eFilterDataType.DateBegan, true);
                                                    LocProgramRecsFound = FindActiveRows(LocProgramRecs, DateTime.MinValue, SysBeginDate, "EMISSIONS_RECORDING_BEGIN_DATE", "EMISSIONS_RECORDING_BEGIN_DATE", true, true, LocProgFilter);
                                                }
                                            }
                                            if (LocProgramRecsFound.Count == 0)
                                                LinStatus = "Missing Program";
                                            else
                                            {
                                                DateTime UnitMonCertDeadline = cDBConvert.ToDate(LocProgramRecsFound[0]["UNIT_MONITOR_CERT_DEADLINE"], DateTypes.START);
                                                if (UnitMonCertDeadline != DateTime.MinValue)
                                                    if (CurrentDate < UnitMonCertDeadline)
                                                        LinStatus = "IC-Conditional";
                                                    else
                                                        LinStatus = "OOC-Conditional Period Expired";
                                                else
                                                {
                                                    DateTime UnitMonCertBeginDate = cDBConvert.ToDate(LocProgramRecsFound[0]["UNIT_MONITOR_CERT_BEGIN_DATE"], DateTypes.START);

                                                    if (CurrentDate < UnitMonCertBeginDate.AddDays(180))
                                                        LinStatus = "IC-Conditional";
                                                    else
                                                        LinStatus = "OOC-Conditional Period Expired";
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    int DateDiff = cDateFunctions.DateDifference(CertEventDate, CurrentDate);
                                    if (DateDiff + 1 > 180)// add one for inclusivity
                                        LinStatus = "OOC-Conditional Period Expired";
                                    else
                                    {
                                        if (CertEventDate.Year == CurrentYear && cDateFunctions.ThisQuarter(CertEventDate) == CurrentQtr)
                                            if (DateDiff + 1 > 90)// add one for inclusivity
                                            {
                                                /* Selects operating hour counts based on system if Primary/Primary-Bypass systems (stacks) are invovled. */
                                                if ((EmParameters.PrimaryBypassActiveForHour == true) && EmParameters.QaStatusComponentTypeCode.InList("CO2,NOX,O2"))
                                                {
                                                    int? dayCount;

                                                    if (((dayCount = LinStat5QaCertEventDays(Category.CurrentMonLocPos, EventId)) == null) || (dayCount > 90))
                                                        LinStatus = "OOC-Conditional Period Expired";
                                                    else
                                                        LinStatus = "IC-Conditional";
                                                }
                                                else
                                                {
                                                    if (OpHrsAccumArray[CurrentPosition] == -1)
                                                        LinStatus = "Invalid Op Data";
                                                    else
                                                        if (DateDiff + 1 == OpDaysAccumArray[CurrentPosition])
                                                        LinStatus = "OOC-Conditional Period Expired";
                                                    else
                                                        LinStatus = "IC-Conditional";
                                                }
                                            }
                                            else
                                                LinStatus = "IC-Conditional";
                                        else
                                            if (PriorLinEventRec["MIN_OP_DAYS_PRIOR_QTR"] == DBNull.Value)
                                        {
                                            PriorLinEventRec["MIN_OP_DAYS_PRIOR_QTR"] = 0;
                                            PriorLinEventRec["MAX_OP_DAYS_PRIOR_QTR"] = 0;
                                            OpSuppFilter = new sFilterPair[3];
                                            OpSuppFilter[0].Set("OP_TYPE_CD", "OPDAYS");

                                            bool StopLooking = false;
                                            int j;
                                            int DaySpread1;
                                            int DaySpread2;
                                            int? operatingDaysCount;

                                            for (int i = CertEventYear; i <= YearPriorToCurrentQtr; i++)
                                            {
                                                j = 1;
                                                if (i == CertEventYear)
                                                    j = CertEventQtr;
                                                do
                                                {
                                                    if (EarliestLocRptDate <= cDateFunctions.LastDateThisQuarter(i, j))
                                                    {
                                                        /* Selects operating hour counts based on system if Primary/Primary-Bypass systems (stacks) are invovled. */
                                                        if ((EmParameters.PrimaryBypassActiveForHour == true) && EmParameters.QaStatusComponentTypeCode.InList("CO2,NOX,O2"))
                                                        {
                                                            CheckDataView<SystemOpSuppData> systemOpSuppData
                                                                = EmParameters.SystemOperatingSuppDataRecordsByLocation.FindRows(new cFilterCondition("MON_SYS_ID", EmParameters.QaStatusPrimaryOrPrimaryBypassSystemId),
                                                                                                                                 new cFilterCondition("CALENDAR_YEAR", i),
                                                                                                                                 new cFilterCondition("QUARTER", j),
                                                                                                                                 new cFilterCondition("OP_SUPP_DATA_TYPE_CD", "OP"));

                                                            if (systemOpSuppData.Count > 0)
                                                                operatingDaysCount = systemOpSuppData[0].Days;
                                                            else
                                                                operatingDaysCount = null;
                                                        }
                                                        else
                                                        {
                                                            OpSuppFilter[1].Set("CALENDAR_YEAR", i, eFilterDataType.Integer);
                                                            OpSuppFilter[2].Set("QUARTER", j, eFilterDataType.Integer);
                                                            OpSuppDataRecsFound = FindRows(OpSuppDataRecs, OpSuppFilter);

                                                            if (OpSuppDataRecsFound.Count > 0)
                                                                operatingDaysCount = cDBConvert.ToInteger(OpSuppDataRecsFound[0]["OP_VALUE"]);
                                                            else
                                                                operatingDaysCount = null;
                                                        }

                                                        if (operatingDaysCount == null)
                                                        {
                                                            Category.SetCheckParameter("Linearity_Missing_Op_Data_Info", i + "Q" + j, eParameterDataType.String);
                                                            PriorLinEventRec["MIN_OP_DAYS_PRIOR_QTR"] = -1;
                                                            StopLooking = true;
                                                        }
                                                        else
                                                        {
                                                            if (j == CertEventQtr && i == CertEventYear)
                                                            {
                                                                // Determine whether supplemental count exists, try system specific for Primary-Bypass situation first.
                                                                int? supplementalCount = null;
                                                                {
                                                                    if ((EmParameters.PrimaryBypassActiveForHour == true) && EmParameters.QaStatusComponentTypeCode.InList("CO2,NOX,O2") &&
                                                                        (PriorLinEventRec["QA_CERT_EVENT_DATE_SYSTEM_SUPP_DATA_EXISTS_IND"].AsInteger() == 1))
                                                                    {
                                                                        supplementalCount = PriorLinEventRec["QA_CERT_EVENT_SYSTEM_OP_DAY_COUNT"].AsShort(0);
                                                                    }

                                                                    if (!supplementalCount.HasValue && (PriorLinEventRec["QA_CERT_EVENT_DATE_SUPP_DATA_EXISTS_IND"].AsInteger() == 1))
                                                                    {
                                                                        supplementalCount = PriorLinEventRec["QA_CERT_EVENT_OP_DAY_COUNT"].AsShort(0);
                                                                    }
                                                                }

                                                                if (supplementalCount.HasValue)
                                                                {
                                                                    PriorLinEventRec["MIN_OP_DAYS_PRIOR_QTR"] = (int)PriorLinEventRec["MIN_OP_DAYS_PRIOR_QTR"] + supplementalCount.Value;
                                                                    PriorLinEventRec["MAX_OP_DAYS_PRIOR_QTR"] = (int)PriorLinEventRec["MAX_OP_DAYS_PRIOR_QTR"] + supplementalCount.Value;
                                                                }
                                                                else
                                                                {
                                                                    DaySpread1 = operatingDaysCount.Value - cDateFunctions.DateDifference(cDateFunctions.StartDateThisQuarter(CertEventDate), CertEventDate);

                                                                    if (DaySpread1 > 0)
                                                                        PriorLinEventRec["MIN_OP_DAYS_PRIOR_QTR"] = DaySpread1;

                                                                    DaySpread2 = cDateFunctions.DateDifference(CertEventDate.AddDays(-1), cDateFunctions.LastDateThisQuarter(CertEventDate));

                                                                    if (operatingDaysCount.Value < DaySpread2)
                                                                        PriorLinEventRec["MAX_OP_DAYS_PRIOR_QTR"] = operatingDaysCount.Value;
                                                                    else
                                                                        PriorLinEventRec["MAX_OP_DAYS_PRIOR_QTR"] = DaySpread2;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                PriorLinEventRec["MIN_OP_DAYS_PRIOR_QTR"] = (int)PriorLinEventRec["MIN_OP_DAYS_PRIOR_QTR"] + operatingDaysCount.Value;
                                                                PriorLinEventRec["MAX_OP_DAYS_PRIOR_QTR"] = (int)PriorLinEventRec["MAX_OP_DAYS_PRIOR_QTR"] + operatingDaysCount.Value;
                                                            }
                                                        }
                                                    }
                                                    j++;
                                                } while (!StopLooking && j <= 4 && !(i == YearPriorToCurrentQtr && j > QtrPriorToCurrentQtr));
                                                if (StopLooking)
                                                    break;
                                            }

                                            int currentOpDays;

                                            /* Selects operating hour counts based on system if Primary/Primary-Bypass systems (stacks) are invovled. */
                                            if ((EmParameters.PrimaryBypassActiveForHour == true) && EmParameters.QaStatusComponentTypeCode.InList("CO2,NOX,O2"))
                                            {
                                                currentOpDays = EmParameters.SystemOperatingSuppDataDictionaryArray[Category.CurrentMonLocPos][EmParameters.QaStatusPrimaryOrPrimaryBypassSystemId].QuarterlyOperatingCounts.Days;
                                            }
                                            else
                                            {
                                                currentOpDays = OpDaysAccumArray[CurrentPosition];
                                            }

                                            if ((int)PriorLinEventRec["MIN_OP_DAYS_PRIOR_QTR"] == -1)
                                                LinStatus = "Missing Op Data";
                                            else if ((int)PriorLinEventRec["MIN_OP_DAYS_PRIOR_QTR"] + currentOpDays > 90)
                                                LinStatus = "OOC-Conditional Period Expired";
                                            else if ((int)PriorLinEventRec["MIN_OP_DAYS_PRIOR_QTR"] == (int)PriorLinEventRec["MAX_OP_DAYS_PRIOR_QTR"])
                                                LinStatus = "IC-Conditional"; // If the max and min or the same then supplemental data was used.
                                            else if ((int)PriorLinEventRec["MAX_OP_DAYS_PRIOR_QTR"] + currentOpDays > 90)
                                                LinStatus = "Undetermined-Conditional Data";
                                            else
                                                LinStatus = "IC-Conditional";
                                        }
                                        else
                                            LinStatus = "IC-Conditional";
                                    }
                                }
                            }
                            else
                            {
                                int CondBeginYear = CondBeginDate.Year;
                                int CondBeginQtr = cDateFunctions.ThisQuarter(CondBeginDate);
                                if (CondBeginDate.Year == CurrentYear && CondBeginQtr == CurrentQtr)
                                {
                                    /* Selects operating hour counts based on system if Primary/Primary-Bypass systems (stacks) are invovled. */
                                    if ((EmParameters.PrimaryBypassActiveForHour == true) && EmParameters.QaStatusComponentTypeCode.InList("CO2,NOX,O2"))
                                    {
                                        int? hourCount;

                                        if (((hourCount = LinStat5ConditionalDataBeginHours(Category.CurrentMonLocPos, EventId)) == null) || (hourCount > 168))
                                            LinStatus = "OOC-Conditional Period Expired";
                                        else
                                            LinStatus = "IC-Conditional";
                                    }
                                    else
                                    {
                                        DataView HrOpRecs = Category.GetCheckParameter("Hourly_Operating_Data_Records_for_Location").ValueAsDataView();
                                        sFilterPair[] HrOpFilter = new sFilterPair[1];
                                        HrOpFilter[0].Set("OP_TIME", 0, eFilterDataType.Decimal, eFilterPairRelativeCompare.GreaterThan);
                                        DataView HrOpRecsFound;
                                        HrOpRecsFound = FindActiveRows(HrOpRecs, CondBeginDate, CondBeginHour, CurrentDate, CurrentHour, "BEGIN_DATE", "BEGIN_HOUR", "BEGIN_DATE", "BEGIN_HOUR", HrOpFilter);
                                        if (HrOpRecsFound.Count > 168)
                                            LinStatus = "OOC-Conditional Period Expired";
                                        else
                                            LinStatus = "IC-Conditional";
                                    }
                                }
                                else
                                {
                                    if (PriorLinEventRec["MIN_OP_HOURS_PRIOR_QTR"] == DBNull.Value)
                                    {
                                        PriorLinEventRec["MIN_OP_HOURS_PRIOR_QTR"] = 0;
                                        PriorLinEventRec["MAX_OP_HOURS_PRIOR_QTR"] = 0;
                                        bool StopLooking = false;
                                        int j;
                                        int HourSpread1;
                                        int HourSpread2;
                                        int? operatingHoursCount;

                                        for (int i = CondBeginYear; i <= YearPriorToCurrentQtr; i++)
                                        {
                                            j = 1;
                                            if (i == CondBeginYear)
                                                j = CondBeginQtr;
                                            do
                                            {
                                                if (EarliestLocRptDate <= cDateFunctions.LastDateThisQuarter(i, j))
                                                {
                                                    /* Selects operating hour counts based on system if Primary/Primary-Bypass systems (stacks) are invovled. */
                                                    if ((EmParameters.PrimaryBypassActiveForHour == true) && EmParameters.QaStatusComponentTypeCode.InList("CO2,NOX,O2"))
                                                    {
                                                        string opSuppDataTypeCd = (EmParameters.AnnualReportingRequirement == true) || (j != 2) ? "OP" : "OPMJ";

                                                        CheckDataView<SystemOpSuppData> systemOpSuppData
                                                            = EmParameters.SystemOperatingSuppDataRecordsByLocation.FindRows(new cFilterCondition("MON_SYS_ID", EmParameters.QaStatusPrimaryOrPrimaryBypassSystemId),
                                                                                                                             new cFilterCondition("CALENDAR_YEAR", i),
                                                                                                                             new cFilterCondition("QUARTER", j),
                                                                                                                             new cFilterCondition("OP_SUPP_DATA_TYPE_CD", opSuppDataTypeCd));

                                                        if (systemOpSuppData.Count > 0)
                                                            operatingHoursCount = systemOpSuppData[0].Hours;
                                                        else
                                                            operatingHoursCount = null;
                                                    }
                                                    else
                                                    {
                                                        if (!AnnRptReq && j == 2)
                                                        {
                                                            OpSuppFilter = new sFilterPair[3];
                                                            OpSuppFilter[0].Set("OP_TYPE_CD", "OSHOURS");
                                                            OpSuppFilter[1].Set("CALENDAR_YEAR", i, eFilterDataType.Integer);
                                                            OpSuppFilter[2].Set("QUARTER", j, eFilterDataType.Integer);
                                                        }
                                                        else
                                                        {
                                                            OpSuppFilter = new sFilterPair[4];
                                                            OpSuppFilter[0].Set("OP_TYPE_CD", "OPHOURS");
                                                            OpSuppFilter[1].Set("FUEL_CD", DBNull.Value, eFilterDataType.String);
                                                            OpSuppFilter[2].Set("CALENDAR_YEAR", i, eFilterDataType.Integer);
                                                            OpSuppFilter[3].Set("QUARTER", j, eFilterDataType.Integer);
                                                        }

                                                        OpSuppDataRecsFound = FindRows(OpSuppDataRecs, OpSuppFilter);

                                                        if (OpSuppDataRecsFound.Count > 0)
                                                            operatingHoursCount = cDBConvert.ToInteger(OpSuppDataRecsFound[0]["OP_VALUE"]);
                                                        else
                                                            operatingHoursCount = null;
                                                    }

                                                    if (operatingHoursCount == null)
                                                    {
                                                        Category.SetCheckParameter("Linearity_Missing_Op_Data_Info", i + "Q" + j, eParameterDataType.String);
                                                        PriorLinEventRec["MIN_OP_HOURS_PRIOR_QTR"] = -1;
                                                        StopLooking = true;
                                                    }
                                                    else
                                                    {
                                                        if (j == CondBeginQtr && i == CondBeginYear)
                                                        {
                                                            // Determine whether supplemental count exists, try system specific for Primary-Bypass situation first.
                                                            int? supplementalCount = null;
                                                            {
                                                                if ((EmParameters.PrimaryBypassActiveForHour == true) && EmParameters.QaStatusComponentTypeCode.InList("CO2,NOX,O2") &&
                                                                    (PriorLinEventRec["CONDITIONAL_BEGIN_HOUR_SYSTEM_SUPP_DATA_EXISTS_IND"].AsInteger() == 1))
                                                                {
                                                                    supplementalCount = PriorLinEventRec["CONDITIONAL_BEGIN_SYSTEM_OP_HOUR_COUNT"].AsShort(0);
                                                                }

                                                                if (!supplementalCount.HasValue && (PriorLinEventRec["CONDITIONAL_BEGIN_HOUR_SUPP_DATA_EXISTS_IND"].AsInteger() == 1))
                                                                {
                                                                    supplementalCount = PriorLinEventRec["CONDITIONAL_BEGIN_OP_HOUR_COUNT"].AsShort(0);
                                                                }
                                                            }

                                                            if (supplementalCount.HasValue)
                                                            {
                                                                PriorLinEventRec["MIN_OP_HOURS_PRIOR_QTR"] = (int)PriorLinEventRec["MIN_OP_HOURS_PRIOR_QTR"] + supplementalCount.Value;
                                                                PriorLinEventRec["MAX_OP_HOURS_PRIOR_QTR"] = (int)PriorLinEventRec["MAX_OP_HOURS_PRIOR_QTR"] + supplementalCount.Value;
                                                            }
                                                            else
                                                            {
                                                                HourSpread1 = operatingHoursCount.Value - (24 * cDateFunctions.DateDifference(cDateFunctions.StartDateThisQuarter(CondBeginDate), CondBeginDate) + CondBeginHour);
                                                                if (HourSpread1 > 0)
                                                                    PriorLinEventRec["MIN_OP_HOURS_PRIOR_QTR"] = HourSpread1;
                                                                HourSpread2 = 24 * cDateFunctions.DateDifference(CondBeginDate.AddDays(-1), cDateFunctions.LastDateThisQuarter(CondBeginDate)) + CondBeginHour;
                                                                if (operatingHoursCount.Value < HourSpread2)
                                                                    PriorLinEventRec["MAX_OP_HOURS_PRIOR_QTR"] = operatingHoursCount.Value;
                                                                else
                                                                    PriorLinEventRec["MAX_OP_HOURS_PRIOR_QTR"] = HourSpread2;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            PriorLinEventRec["MIN_OP_HOURS_PRIOR_QTR"] = (int)PriorLinEventRec["MIN_OP_HOURS_PRIOR_QTR"] + operatingHoursCount.Value;
                                                            PriorLinEventRec["MAX_OP_HOURS_PRIOR_QTR"] = (int)PriorLinEventRec["MAX_OP_HOURS_PRIOR_QTR"] + operatingHoursCount.Value;
                                                        }
                                                    }
                                                }
                                                j++;
                                            } while (!StopLooking && j <= 4 && !(i == YearPriorToCurrentQtr && j > QtrPriorToCurrentQtr));
                                            if (StopLooking)
                                                break;
                                        }
                                    }

                                    int currentOpHours;

                                    /* Selects operating hour counts based on system if Primary/Primary-Bypass systems (stacks) are invovled. */
                                    if ((EmParameters.PrimaryBypassActiveForHour == true) && EmParameters.QaStatusComponentTypeCode.InList("CO2,NOX,O2"))
                                    {
                                        currentOpHours = EmParameters.SystemOperatingSuppDataDictionaryArray[Category.CurrentMonLocPos][EmParameters.QaStatusPrimaryOrPrimaryBypassSystemId].QuarterlyOperatingCounts.Hours;
                                    }
                                    else
                                    {
                                        currentOpHours = OpHrsAccumArray[CurrentPosition];
                                    }

                                    if ((int)PriorLinEventRec["MIN_OP_HOURS_PRIOR_QTR"] == -1)
                                        LinStatus = "Missing Op Data";
                                    else if (currentOpHours == -1)
                                    {
                                        if ((int)PriorLinEventRec["MIN_OP_HOURS_PRIOR_QTR"] > 168)
                                            LinStatus = "OOC-Conditional Period Expired";
                                        else
                                            LinStatus = "Invalid Op Data";
                                    }
                                    else
                                    {
                                        if ((int)PriorLinEventRec["MIN_OP_HOURS_PRIOR_QTR"] + currentOpHours > 168)
                                            LinStatus = "OOC-Conditional Period Expired";
                                        else if ((int)PriorLinEventRec["MIN_OP_HOURS_PRIOR_QTR"] == (int)PriorLinEventRec["MAX_OP_HOURS_PRIOR_QTR"])
                                            LinStatus = "IC-Conditional"; // If the max and min or the same then supplemental data was used.
                                        else if ((int)PriorLinEventRec["MAX_OP_HOURS_PRIOR_QTR"] + currentOpHours > 168)
                                            LinStatus = "Undetermined-Conditional Data";
                                        else
                                            LinStatus = "IC-Conditional";
                                    }
                                }
                            }
                        }
                    }
                }
                Category.SetCheckParameter("Subsequent_Linearity_Record", SubsqLinRec, eParameterDataType.DataRowView);
                if (LinStatus != "")
                    Category.SetCheckParameter("Current_Linearity_Status", LinStatus, eParameterDataType.String);
                else
                    Category.SetCheckParameter("Current_Linearity_Status", null, eParameterDataType.String);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "LINSTAT5");
            }

            return ReturnVal;
        }


        #region LINSTAT-5 Helpers

        /// <summary>
        /// Retrieves the QA cert event day count for the qa cert event associated with the given id.
        /// </summary>
        /// <param name="locationPosition">The position of the location to which the qa cert event belongs.</param>
        /// <param name="eventId">The QA_CERT_EVENT_ID of the qa cert event.</param>
        /// <returns>The count of operating days on or after the QA cert event date in the quarter of the date.</returns>
        public static int? LinStat5QaCertEventDays(int locationPosition, string eventId)
        {
            int? result;

            string dictionaryKey = QaCertificationSupplementalData.FormatKey(eventId, eQaCertificationSupplementalDataTargetDateHour.QaCertEventDate);

            if (EmParameters.QaCertEventSuppDataDictionaryArray[locationPosition].ContainsKey(dictionaryKey))
            {
                result = EmParameters.QaCertEventSuppDataDictionaryArray[locationPosition][dictionaryKey].QuarterlySystemOperatingCounts.Count;
            }
            else
            {
                result = null;
            }

            return result;
        }


        /// <summary>
        /// Retrieves the conditional data begin hour count for the qa cert event's associated with the given id.
        /// </summary>
        /// <param name="locationPosition">The position of the location to which the qa cert event belongs.</param>
        /// <param name="eventId">The QA_CERT_EVENT_ID of the qa cert event.</param>
        /// <returns>The count of operating hours on or after the conditional data begin hour in the quarter of the hour.</returns>
        public static int? LinStat5ConditionalDataBeginHours(int locationPosition, string eventId)
        {
            int? result;

            string dictionaryKey = QaCertificationSupplementalData.FormatKey(eventId, eQaCertificationSupplementalDataTargetDateHour.ConditionalDataBeginHour);

            if (EmParameters.QaCertEventSuppDataDictionaryArray[locationPosition].ContainsKey(dictionaryKey))
            {
                result = EmParameters.QaCertEventSuppDataDictionaryArray[locationPosition][dictionaryKey].QuarterlySystemOperatingCounts.Count;
            }
            else
            {
                result = null;
            }

            return result;
        }

        #endregion


        public static string LINSTAT6(cCategory Category, ref bool Log)
        //Determine Final Linearity Status       
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("Alternate_Linearity_Record", null, eParameterDataType.DataRowView);
                DataRowView AltInvalidLinRec = null;
                string LinStatus = Category.GetCheckParameter("Current_Linearity_Status").ValueAsString();

                if (LinStatus.StartsWith("OOC"))
                {
                    if (Category.GetCheckParameter("Invalid_Linearity_Record").ValueAsDataRowView() != null)
                        LinStatus += "*";
                    Category.CheckCatalogResult = LinStatus;
                }
                else
                {
                    string AnalyzerRangeUsed = Category.GetCheckParameter("Current_Analyzer_Range_Used").ValueAsString();
                    if (LinStatus == "Invalid Monitor Span")
                    {
                        if (AnalyzerRangeUsed == "H")
                            LinStatus += " (High Scale)";
                        else
                            LinStatus += " (Low Scale)";
                        Category.CheckCatalogResult = LinStatus;
                    }
                    else
                        if (Category.GetCheckParameter("Dual_Range_Status").ValueAsBool() && (LinStatus.StartsWith("IC") || LinStatus.StartsWith("Undetermined")))
                    {
                        string AltAnalyzerRange;
                        string AltComponentID;
                        string AltSysIds = "";
                        if (AnalyzerRangeUsed == "H")
                        {
                            AltAnalyzerRange = "L";
                            AltComponentID = Category.GetCheckParameter("Low_Range_Component_ID").ValueAsString();
                        }
                        else
                        {
                            AltAnalyzerRange = "H";
                            AltComponentID = Category.GetCheckParameter("High_Range_Component_ID").ValueAsString();
                        }
                        DataView MonSysCompRecs = Category.GetCheckParameter("Monitor_System_Component_Records_By_Hour_Location").ValueAsDataView();
                        foreach (DataRowView drv in MonSysCompRecs)
                            if (cDBConvert.ToString(drv["COMPONENT_ID"]) == AltComponentID)
                                AltSysIds = AltSysIds.ListAdd(cDBConvert.ToString(drv["MON_SYS_ID"]));
                        if (AltSysIds == "")//tantamount to no such MonSysCompRecs found
                        {
                            LinStatus = "Invalid Monitor System Component";
                            Category.CheckCatalogResult = LinStatus;
                        }
                        else
                        {
                            DataRowView CurrentMHVRecord = (DataRowView)Category.GetCheckParameter("Current_MHV_Record").ParameterValue;
                            if (Category.GetCheckParameter("Current_MHV_Parameter").ValueAsString().InList("SO2C,NOXC"))
                            {
                                DataView MonSpanRecs = Category.GetCheckParameter("Monitor_Span_Records_By_Hour_Location").ValueAsDataView();
                                sFilterPair[] MonSpanFilter = new sFilterPair[2];
                                MonSpanFilter[0].Set("COMPONENT_TYPE_CD", EmParameters.QaStatusComponentTypeCode);
                                MonSpanFilter[1].Set("SPAN_SCALE_CD", AltAnalyzerRange);
                                DataView MonSpanRecsFound = FindRows(MonSpanRecs, MonSpanFilter);
                                if (MonSpanRecsFound.Count == 0 || MonSpanRecsFound.Count > 1 || cDBConvert.ToDecimal(MonSpanRecsFound[0]["SPAN_VALUE"]) <= 0)
                                {
                                    LinStatus = "Invalid Monitor Span";
                                    if (AltAnalyzerRange == "H")
                                        LinStatus += " (High Scale)";
                                    else
                                        LinStatus += " (Low Scale)";
                                    Category.CheckCatalogResult = LinStatus;
                                }
                                else
                                    if (cDBConvert.ToDecimal(MonSpanRecsFound[0]["SPAN_VALUE"]) <= 30)
                                    if (!LinStatus.StartsWith("IC"))
                                        Category.CheckCatalogResult = LinStatus;
                                    else
                                        return ReturnVal;
                            }
                            if (string.IsNullOrEmpty(Category.CheckCatalogResult))
                            {
                                DateTime CurrentDate = EmParameters.CurrentDateHour.AsStartDate();
                                int CurrentHour = EmParameters.CurrentDateHour.AsStartHour();

                                DataView LinTestRecs = Category.GetCheckParameter("Linearity_Test_Records_By_Location_For_QA_Status").ValueAsDataView();

                                DataView PriorLinTestRecsFound;
                                sFilterPair[] LinTestRecsFilter = new sFilterPair[3];

                                DataView priorLinearityTestRecords
                                  = cRowFilter.FindRows(LinTestRecs,
                                                        new cFilterCondition[]
                                                        {
                                            new cFilterCondition("COMPONENT_ID", AltComponentID),
                                            new cFilterCondition("SPAN_SCALE_CD", AltAnalyzerRange),
                                            new cFilterCondition("TEST_RESULT_CD", "INVALID", true),
                                            new cFilterCondition("END_DATEHOUR", EmParameters.CurrentDateHour.AsStartDateTime(), eFilterDataType.DateEnded, eFilterConditionRelativeCompare.LessThan)
                                                        },
                                                        new cFilterCondition[]
                                                        {
                                            new cFilterCondition("COMPONENT_ID", AltComponentID),
                                            new cFilterCondition("SPAN_SCALE_CD", AltAnalyzerRange),
                                            new cFilterCondition("TEST_RESULT_CD", "PASSED,PASSAPS", eFilterConditionStringCompare.InList),
                                            new cFilterCondition("END_DATEHOUR", EmParameters.CurrentDateHour.AsStartDateTime(), eFilterDataType.DateEnded),
                                            new cFilterCondition("END_MIN", 45, eFilterDataType.Integer, eFilterConditionRelativeCompare.LessThan)
                                                        }
                                                       );

                                DataRowView MostRecentRec = null;

                                if (priorLinearityTestRecords.Count > 0)
                                {
                                    priorLinearityTestRecords.Sort = "END_DATE DESC, END_HOUR DESC, END_MIN DESC";
                                    MostRecentRec = priorLinearityTestRecords[0];
                                    Category.SetCheckParameter("Alternate_Linearity_Record", MostRecentRec, eParameterDataType.DataRowView);
                                }

                                DataView QACertEventRecs = Category.GetCheckParameter("QA_Certification_Event_Records").ValueAsDataView();
                                DataTable QACertEventRecsTable = QACertEventRecs.Table.Copy();
                                DataView QACertRecsFound = FindCertEventRecs(QACertEventRecsTable, "Alternate_Linearity_Record", AltComponentID, AltAnalyzerRange, Category);
                                if (QACertRecsFound != null && QACertRecsFound.Count > 0)
                                {
                                    DateTime CondBeginDate = cDBConvert.ToDate(QACertRecsFound[0]["CONDITIONAL_DATA_BEGIN_DATE"], DateTypes.START);
                                    int CondBeginHour = cDBConvert.ToHour(QACertRecsFound[0]["CONDITIONAL_DATA_BEGIN_HOUR"], DateTypes.START);
                                    if (CondBeginDate == DateTime.MinValue || CondBeginHour == int.MinValue || (CurrentDate < CondBeginDate || (CurrentDate == CondBeginDate &&
                                        CurrentHour < CondBeginHour)))
                                    {
                                        if (!LinStatus.StartsWith("IC"))
                                            Category.CheckCatalogResult = LinStatus;
                                    }
                                    else
                                    {
                                        LinTestRecsFilter = new sFilterPair[3];
                                        LinTestRecsFilter[0].Set("COMPONENT_ID", AltComponentID);
                                        LinTestRecsFilter[1].Set("SPAN_SCALE_CD", AltAnalyzerRange);
                                        LinTestRecsFilter[2].Set("TEST_RESULT_CD", "INVALID");
                                        PriorLinTestRecsFound = FindActiveRows(LinTestRecs, CondBeginDate, CondBeginHour, CurrentDate, CurrentHour, "END_DATE", "END_HOUR", "END_DATE", "END_HOUR", LinTestRecsFilter);
                                        if (PriorLinTestRecsFound.Count > 0)
                                        {
                                            PriorLinTestRecsFound.Sort = "END_DATE DESC, END_HOUR DESC, END_MIN DESC";
                                            AltInvalidLinRec = PriorLinTestRecsFound[0];
                                        }
                                        else
                                            AltInvalidLinRec = null;
                                        LinTestRecsFilter = new sFilterPair[3];
                                        LinTestRecsFilter[0].Set("COMPONENT_ID", AltComponentID);
                                        LinTestRecsFilter[1].Set("TEST_RESULT_CD", "INVALID", true);
                                        LinTestRecsFilter[2].Set("SPAN_SCALE_CD", AltAnalyzerRange);
                                        PriorLinTestRecsFound = FindActiveRows(LinTestRecs, CondBeginDate, CondBeginHour, DateTime.MaxValue, 23, "END_DATE", "END_HOUR", "END_DATE", "END_HOUR", LinTestRecsFilter);
                                        if (PriorLinTestRecsFound.Count > 0)
                                        {
                                            PriorLinTestRecsFound.Sort = "END_DATE ASC, END_HOUR ASC, END_MIN ASC";
                                            Category.SetCheckParameter("Alternate_Linearity_Record", PriorLinTestRecsFound[0], eParameterDataType.DataRowView);
                                            if (cDBConvert.ToString(PriorLinTestRecsFound[0]["QA_NEEDS_EVAL_FLG"]) == "Y")
                                                LinStatus = "Alternate Range Recertification Test Not Yet Evaluated";
                                            else
                                            {
                                                string TestResCd = cDBConvert.ToString(PriorLinTestRecsFound[0]["TEST_RESULT_CD"]);
                                                if (TestResCd == "" || TestResCd.InList("FAILED,ABORTED"))
                                                {
                                                    if (AltInvalidLinRec == null)
                                                    {
                                                        LinTestRecsFilter = new sFilterPair[3];
                                                        LinTestRecsFilter[0].Set("COMPONENT_ID", AltComponentID);
                                                        LinTestRecsFilter[1].Set("SPAN_SCALE_CD", AltAnalyzerRange);
                                                        LinTestRecsFilter[2].Set("TEST_RESULT_CD", "INVALID");
                                                        PriorLinTestRecsFound = FindActiveRows(LinTestRecs, cDBConvert.ToDate(QACertRecsFound[0]["QA_CERT_EVENT_DATE"], DateTypes.START), cDBConvert.ToHour(QACertRecsFound[0]["QA_CERT_EVENT_HOUR"], DateTypes.START), cDBConvert.ToDate(PriorLinTestRecsFound[0]["END_DATE"], DateTypes.END), cDBConvert.ToHour(PriorLinTestRecsFound[0]["END_HOUR"], DateTypes.END), "END_DATE", "END_HOUR", "END_DATE", "END_HOUR", LinTestRecsFilter);
                                                        if (PriorLinTestRecsFound.Count > 0)
                                                            AltInvalidLinRec = PriorLinTestRecsFound[0];
                                                    }
                                                    if (TestResCd == "")
                                                    {
                                                        LinStatus = "OOC-Alternate Range Recertification Test Has Critical Errors";
                                                        if (AltInvalidLinRec != null)
                                                            LinStatus += "*";
                                                    }
                                                    else
                                                        if (TestResCd == "FAILED")
                                                    {
                                                        LinStatus = "OOC-Alternate Range Recertification Test Failed";
                                                        if (AltInvalidLinRec != null)
                                                            LinStatus += "*";
                                                    }
                                                    else
                                                            if (TestResCd == "ABORTED")
                                                    {
                                                        LinStatus = "OOC-Alternate Range Recertification Test Aborted";
                                                        if (AltInvalidLinRec != null)
                                                            LinStatus += "*";
                                                    }
                                                }
                                            }
                                        }
                                        if (!LinStatus.StartsWith("IC"))
                                            Category.CheckCatalogResult = LinStatus;
                                    }
                                }
                                else
                                {
                                    DataRowView AltLinRec = Category.GetCheckParameter("Alternate_Linearity_Record").ValueAsDataRowView();
                                    if (AltLinRec != null)
                                        if (cDBConvert.ToString(AltLinRec["QA_NEEDS_EVAL_FLG"]) == "Y")
                                            LinStatus = "Alternate Range Test Not Yet Evaluated";
                                        else
                                        {
                                            string AltTestResCd = cDBConvert.ToString(AltLinRec["TEST_RESULT_CD"]);
                                            if (AltTestResCd == "" || AltTestResCd.InList("ABORTED,FAILED"))
                                            {
                                                LinTestRecsFilter = new sFilterPair[3];
                                                LinTestRecsFilter[0].Set("COMPONENT_ID", AltComponentID);
                                                LinTestRecsFilter[1].Set("SPAN_SCALE_CD", AltAnalyzerRange);
                                                LinTestRecsFilter[2].Set("TEST_RESULT_CD", "INVALID");
                                                PriorLinTestRecsFound = FindActiveRows(LinTestRecs, cDBConvert.ToDate(AltLinRec["END_DATE"], DateTypes.END), cDBConvert.ToHour(AltLinRec["END_HOUR"], DateTypes.END), CurrentDate, CurrentHour, "END_DATE", "END_HOUR", "END_DATE", "END_HOUR", LinTestRecsFilter);
                                                if (PriorLinTestRecsFound.Count > 0)
                                                    AltInvalidLinRec = PriorLinTestRecsFound[0];
                                                string TestResCd = cDBConvert.ToString(AltLinRec["TEST_RESULT_CD"]);
                                                if (TestResCd == "")
                                                {
                                                    LinStatus = "OOC-Alternate Range Test Has Critical Errors";
                                                    if (AltInvalidLinRec != null)
                                                        LinStatus += "*";
                                                }
                                                else
                                                    if (TestResCd == "FAILED")
                                                {
                                                    LinStatus = "OOC-Alternate Range Test Failed";
                                                    if (AltInvalidLinRec != null)
                                                        LinStatus += "*";
                                                }
                                                else
                                                        if (TestResCd == "ABORTED")
                                                {
                                                    LinStatus = "OOC-Alternate Range Test Aborted";
                                                    if (AltInvalidLinRec != null)
                                                        LinStatus += "*";
                                                }
                                            }
                                        }
                                    else
                                        LinStatus = "OOC-No Prior Alternate Range Test or Event";
                                    if (!LinStatus.StartsWith("IC"))
                                        Category.CheckCatalogResult = LinStatus;
                                }
                            }
                        }
                    }
                    else
                            if (!LinStatus.StartsWith("IC"))
                        Category.CheckCatalogResult = LinStatus;
                }
                Category.SetCheckParameter("Alternate_Invalid_Linearity_Record", AltInvalidLinRec, eParameterDataType.DataRowView);
                if (LinStatus != "")
                    Category.SetCheckParameter("Current_Linearity_Status", LinStatus, eParameterDataType.String);
                else
                    Category.SetCheckParameter("Current_Linearity_Status", null, eParameterDataType.String);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "LINSTAT6");
            }

            return ReturnVal;
        }

        /// <summary>
        /// LinStat-7
        /// 
        /// For a Hg CEMS component, when LINSTAT-4 has assigned a status of IC, IC-Extension or IC-Grace, perform the following:
        /// 
        /// 1) Locate the most recent 3-Level SI.
        /// 2) If one does not exist, return a result.
        /// 3) Otherwise, locate an intervening certification event (120, 125) for the component.
        /// 4) If an event was located, do nothing.
        /// 5) Otherwise, return "OOC- No Priior HGSI3 Test or Event".
        /// 
        /// Note: This check should only run in the Hg Concentration Linearity Status category.
        /// </summary>
        /// <param name="category">The category object for the category in which the check is runnig.</param>
        /// <param name="log">obsolete</param>
        /// <returns></returns>
        public static string LINSTAT7(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                EmParameters.MatsCheckForHgsi3Ran = false;

                if ((EmParameters.CurrentLinearityStatus == "IC") ||
                    (EmParameters.CurrentLinearityStatus == "IC-Extension") || 
                    (EmParameters.CurrentLinearityStatus == "IC-Grace"))
                {
                    if (EmParameters.PriorLinearityRecord.ComponentTypeCd == "HG")
                    {
                        VwQaSuppDataHourlyStatusRow linearityTestRecord 
                            = EmParameters.LinearityTestRecordsByLocationForQaStatus.FindMostRecentRow(
                                                                                                            EmParameters.CurrentDateHour.Value,
                                                                                                            "END_DATEHOUR",
                                                                                                            new cFilterCondition[]
                                                                                                            {
                                                                                                                new cFilterCondition("COMPONENT_ID", EmParameters.PriorLinearityRecord.ComponentId),
                                                                                                                new cFilterCondition("TEST_TYPE_CD", "HGSI3"),
                                                                                                                new cFilterCondition("TEST_RESULT_CD", "PASSED,PASSAPS", eFilterConditionStringCompare.InList)
                                                                                                            }
                                                                                                      );
                        if (linearityTestRecord == null)
                        {
                            EmParameters.CurrentLinearityStatus = "OOC-No Prior 3-Point SI or Event";
                        }
                        else
                        {
                            int certOrRecertEventCount
                                = EmParameters.QaCertificationEventRecords.CountRows(
                                                                                        new cFilterCondition[]
                                                                                        {
                                                                                            new cFilterCondition("COMPONENT_ID", EmParameters.PriorLinearityRecord.ComponentId),
                                                                                            new cFilterCondition("QA_CERT_EVENT_CD", "120,125", eFilterConditionStringCompare.InList),
                                                                                            new cFilterCondition("QA_CERT_EVENT_DATEHOUR", eFilterConditionRelativeCompare.LessThan, EmParameters.CurrentDateHour.Value),
                                                                                            new cFilterCondition("QA_CERT_EVENT_DATEHOUR", eFilterConditionRelativeCompare.GreaterThan, linearityTestRecord.EndDatehour.Value)
                                                                                        }
                                                                                    );

                            if (certOrRecertEventCount > 0)
                            {
                                EmParameters.CurrentLinearityStatus = "OOC-No Prior 3-Point SI or Event";
                            }
                        }

                        EmParameters.MatsCheckForHgsi3Ran = true;
                    }
                }
            }
            catch (Exception ex)
            {
                returnVal = category.CheckEngine.FormatError(ex);
            }

            return returnVal;
        }

        #endregion

        #region Private Methods: Utilities

        private static DataView FindCertEventRecs(DataTable SourceTable, string LinearityRecordParameterName, string ApplCompId, string AnalyzerRangeUsed, cCategory ACategory)
        {
            DataView ReturnView = null;//method returns null if no records found

            //make a like results table and clear it
            DataTable FoundRecordTable = SourceTable.Clone();
            FoundRecordTable.Rows.Clear();

            DataRow FilterRow;
            string CompId;
            string MonSysId;
            DateTime CurrentDate = EmParameters.CurrentDateHour.AsStartDate();
            int CurrentHour = EmParameters.CurrentDateHour.AsStartHour();

            DateTime CondBeginDate;
            int CondBeginHour;
            DateTime CertDate;
            int CertHour;
            DateTime LastCompletedDate;
            int LastCompletedHour;
            string CertEventCd;
            DataRowView OtherLinRec = (DataRowView)ACategory.GetCheckParameter(LinearityRecordParameterName).ParameterValue;
            bool DualRangeStatus = ACategory.GetCheckParameter("Dual_Range_Status").ValueAsBool();
            string HighCompId = ACategory.GetCheckParameter("High_Range_Component_ID").ValueAsString();
            string LowCompId = ACategory.GetCheckParameter("Low_Range_Component_ID").ValueAsString();

            //Fill FoundRecordTable with appropriate rows from SourceTable
            foreach (DataRow SourceRow in SourceTable.Rows)
            {
                CompId = cDBConvert.ToString(SourceRow["COMPONENT_ID"]);
                MonSysId = cDBConvert.ToString(SourceRow["MON_SYS_ID"]);
                CertEventCd = cDBConvert.ToString(SourceRow["QA_CERT_EVENT_CD"]);
                if (CompId == ApplCompId)
                    if (cDBConvert.ToString(SourceRow["LINEARITY_REQUIRED"]) == "Y")
                    {
                        CondBeginDate = cDBConvert.ToDate(SourceRow["CONDITIONAL_DATA_BEGIN_DATE"], DateTypes.START);
                        CondBeginHour = cDBConvert.ToInteger(SourceRow["CONDITIONAL_DATA_BEGIN_HOUR"]);
                        CertDate = cDBConvert.ToDate(SourceRow["QA_CERT_EVENT_DATE"], DateTypes.START);
                        CertHour = cDBConvert.ToInteger(SourceRow["QA_CERT_EVENT_HOUR"]);
                        LastCompletedDate = cDBConvert.ToDate(SourceRow["LAST_TEST_COMPLETED_DATE"], DateTypes.END);
                        LastCompletedHour = cDBConvert.ToInteger(SourceRow["LAST_TEST_COMPLETED_HOUR"]);

                        if (CertDate < CurrentDate || (CertDate == CurrentDate && CertHour < CurrentHour) || (CertDate == CurrentDate && CertHour == CurrentHour && CertDate == CondBeginDate && CertHour == CondBeginHour))
                            if (OtherLinRec == null
                                || CertDate > cDBConvert.ToDate(OtherLinRec["END_DATE"], DateTypes.END) || (CertDate == cDBConvert.ToDate(OtherLinRec["END_DATE"], DateTypes.END) && CertHour >= cDBConvert.ToHour(OtherLinRec["END_HOUR"], DateTypes.END))
                                || (CertDate == cDBConvert.ToDate(OtherLinRec["END_DATE"], DateTypes.END) && CertHour == cDBConvert.ToHour(OtherLinRec["END_HOUR"], DateTypes.END) && (LastCompletedDate == DateTime.MaxValue || LastCompletedDate > cDBConvert.ToDate(OtherLinRec["END_DATE"], DateTypes.END) || (LastCompletedDate == cDBConvert.ToDate(OtherLinRec["END_DATE"], DateTypes.END) && LastCompletedHour >= cDBConvert.ToHour(OtherLinRec["END_HOUR"], DateTypes.END)))))
                                if (!DualRangeStatus || HighCompId != LowCompId ||
                                    (!CertEventCd.InList("27,30,172") && AnalyzerRangeUsed == "H") ||
                                    (!CertEventCd.InList("35,171") && AnalyzerRangeUsed == "L"))
                                {
                                    FilterRow = FoundRecordTable.NewRow();

                                    foreach (DataColumn Column in FoundRecordTable.Columns)
                                        FilterRow[Column.ColumnName] = SourceRow[Column.ColumnName];

                                    FoundRecordTable.Rows.Add(FilterRow);
                                    ReturnView = FoundRecordTable.DefaultView;
                                }
                    }
            }
            return ReturnView;
        }

        private static DataView FindCertEventRecs2(DataTable SourceTable, string LinearityRecordParameterName, string ApplCompId, string AnalyzerRangeUsed, cCategory ACategory)
        {
            DataView ReturnView = null;//method returns null if no records found

            //make a like results table and clear it
            DataTable FoundRecordTable = SourceTable.Clone();
            FoundRecordTable.Rows.Clear();

            DataRow FilterRow;
            string CompId;
            string MonSysId;
            DateTime CurrentDate = EmParameters.CurrentDateHour.AsStartDate();
            int CurrentHour = EmParameters.CurrentDateHour.AsStartHour();

            DateTime CondBeginDate;
            int CondBeginHour;
            string CertEventCd;
            DataRowView OtherLinRec = (DataRowView)ACategory.GetCheckParameter(LinearityRecordParameterName).ParameterValue;
            DateTime OtherLinRecEndDate;
            int OtherLinRecEndHour;
            bool DualRangeStatus = ACategory.GetCheckParameter("Dual_Range_Status").ValueAsBool();
            string HighCompId = ACategory.GetCheckParameter("High_Range_Component_ID").ValueAsString();
            string LowCompId = ACategory.GetCheckParameter("Low_Range_Component_ID").ValueAsString();

            //Fill FoundRecordTable with appropriate rows from SourceTable
            foreach (DataRow SourceRow in SourceTable.Rows)
            {
                CompId = cDBConvert.ToString(SourceRow["COMPONENT_ID"]);
                MonSysId = cDBConvert.ToString(SourceRow["MON_SYS_ID"]);
                CertEventCd = cDBConvert.ToString(SourceRow["QA_CERT_EVENT_CD"]);
                if (CompId == ApplCompId)
                    if (cDBConvert.ToString(SourceRow["LINEARITY_REQUIRED"]) == "Y")
                    {
                        CondBeginDate = cDBConvert.ToDate(SourceRow["CONDITIONAL_DATA_BEGIN_DATE"], DateTypes.START);
                        CondBeginHour = cDBConvert.ToInteger(SourceRow["CONDITIONAL_DATA_BEGIN_HOUR"]);

                        if (CondBeginDate < CurrentDate || (CondBeginDate == CurrentDate && CondBeginHour <= CurrentHour))
                        {
                            OtherLinRecEndDate = cDBConvert.ToDate(OtherLinRec["END_DATE"], DateTypes.END);
                            OtherLinRecEndHour = cDBConvert.ToHour(OtherLinRec["END_HOUR"], DateTypes.END);
                            if (CondBeginDate > OtherLinRecEndDate || (CondBeginDate == OtherLinRecEndDate && CondBeginHour >= cDBConvert.ToHour(OtherLinRec["END_HOUR"], DateTypes.END)))
                                if (!DualRangeStatus || HighCompId != LowCompId ||
                                    (!CertEventCd.InList("27,30,172") && AnalyzerRangeUsed == "H") ||
                                    (!CertEventCd.InList("35,171") && AnalyzerRangeUsed == "L"))
                                    if (ACategory.GetCheckParameter("Annual_Reporting_Requirement").ValueAsBool() || cDBConvert.ToDate(SourceRow["QA_CERT_EVENT_DATE"], DateTypes.START) >= new DateTime(CurrentDate.Year, 4, 1))
                                    {
                                        FilterRow = FoundRecordTable.NewRow();

                                        foreach (DataColumn Column in FoundRecordTable.Columns)
                                            FilterRow[Column.ColumnName] = SourceRow[Column.ColumnName];

                                        FoundRecordTable.Rows.Add(FilterRow);
                                        ReturnView = FoundRecordTable.DefaultView;
                                    }
                        }
                    }
            }

            return ReturnView;
        }

        private static DataView FindCertEventRecs3(DataTable SourceTable, string LinearityRecordParameterName, string ApplCompId, string AnalyzerRangeUsed, cCategory ACategory)
        {
            DataView ReturnView = null;//method returns null if no records found

            //make a like results table and clear it
            DataTable FoundRecordTable = SourceTable.Clone();
            FoundRecordTable.Rows.Clear();

            DataRow FilterRow;
            string CompId;
            string MonSysId;
            DateTime CurrentDate = EmParameters.CurrentDateHour.AsStartDate();
            int CurrentHour = EmParameters.CurrentDateHour.AsStartHour();

            DateTime CondBeginDate;
            int CondBeginHour;
            DateTime CertDate;
            int CertHour;
            DateTime LastCompletedDate;
            int LastCompletedHour;
            string CertEventCd;
            DataRowView OtherLinRec = (DataRowView)ACategory.GetCheckParameter(LinearityRecordParameterName).ParameterValue;
            bool DualRangeStatus = ACategory.GetCheckParameter("Dual_Range_Status").ValueAsBool();
            string HighCompId = ACategory.GetCheckParameter("High_Range_Component_ID").ValueAsString();
            string LowCompId = ACategory.GetCheckParameter("Low_Range_Component_ID").ValueAsString();

            //Fill FoundRecordTable with appropriate rows from SourceTable
            foreach (DataRow SourceRow in SourceTable.Rows)
            {
                CompId = cDBConvert.ToString(SourceRow["COMPONENT_ID"]);
                MonSysId = cDBConvert.ToString(SourceRow["MON_SYS_ID"]);
                CertEventCd = cDBConvert.ToString(SourceRow["QA_CERT_EVENT_CD"]);
                if (CompId == ApplCompId)
                    if (cDBConvert.ToString(SourceRow["LINEARITY_REQUIRED"]) == "Y")
                    {
                        CondBeginDate = cDBConvert.ToDate(SourceRow["CONDITIONAL_DATA_BEGIN_DATE"], DateTypes.START);
                        CondBeginHour = cDBConvert.ToInteger(SourceRow["CONDITIONAL_DATA_BEGIN_HOUR"]);
                        CertDate = cDBConvert.ToDate(SourceRow["QA_CERT_EVENT_DATE"], DateTypes.START);
                        CertHour = cDBConvert.ToInteger(SourceRow["QA_CERT_EVENT_HOUR"]);
                        LastCompletedDate = cDBConvert.ToDate(SourceRow["LAST_TEST_COMPLETED_DATE"], DateTypes.END);
                        LastCompletedHour = cDBConvert.ToInteger(SourceRow["LAST_TEST_COMPLETED_HOUR"]);

                        if (CertDate < CurrentDate || (CertDate == CurrentDate && CertHour < CurrentHour) || (CertDate == CurrentDate && CertHour == CurrentHour && CertDate == CondBeginDate && CertHour == CondBeginHour))
                            if (OtherLinRec == null
                                || CertDate > cDBConvert.ToDate(OtherLinRec["END_DATE"], DateTypes.END) || (CertDate == cDBConvert.ToDate(OtherLinRec["END_DATE"], DateTypes.END) && CertHour >= cDBConvert.ToHour(OtherLinRec["END_HOUR"], DateTypes.END))
                                || (CertDate == cDBConvert.ToDate(OtherLinRec["END_DATE"], DateTypes.END) && CertHour == cDBConvert.ToHour(OtherLinRec["END_HOUR"], DateTypes.END) && (LastCompletedDate == DateTime.MaxValue || LastCompletedDate > cDBConvert.ToDate(OtherLinRec["END_DATE"], DateTypes.END) || (LastCompletedDate == cDBConvert.ToDate(OtherLinRec["END_DATE"], DateTypes.END) && LastCompletedHour >= cDBConvert.ToHour(OtherLinRec["END_HOUR"], DateTypes.END)))))
                                if (!DualRangeStatus || HighCompId != LowCompId ||
                                    (!CertEventCd.InList("27,30,172") && AnalyzerRangeUsed == "H") ||
                                    (!CertEventCd.InList("35,171") && AnalyzerRangeUsed == "L"))
                                    if (ACategory.GetCheckParameter("Annual_Reporting_Requirement").ValueAsBool() || cDBConvert.ToDate(SourceRow["QA_CERT_EVENT_DATE"], DateTypes.START) >= new DateTime(CurrentDate.Year, 4, 1))
                                    {
                                        FilterRow = FoundRecordTable.NewRow();

                                        foreach (DataColumn Column in FoundRecordTable.Columns)
                                            FilterRow[Column.ColumnName] = SourceRow[Column.ColumnName];

                                        FoundRecordTable.Rows.Add(FilterRow);
                                        ReturnView = FoundRecordTable.DefaultView;
                                    }
                    }
            }
            return ReturnView;
        }

        #endregion
    }
}
