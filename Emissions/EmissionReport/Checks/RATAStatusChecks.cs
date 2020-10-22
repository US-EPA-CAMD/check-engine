using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.CheckEngine.SpecialParameterClasses;
using ECMPS.Checks.Data.Ecmps.CheckEm.Function;
using ECMPS.Checks.Em.Parameters;
using ECMPS.Checks.EmissionsReport;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Extensions;


namespace ECMPS.Checks.EmissionsChecks
{
    public class cRATAStatusChecks : cEmissionsChecks
    {
        #region Constructors

        public cRATAStatusChecks(cEmissionsReportProcess emissionReportProcess)
            : base(emissionReportProcess)
        {
            CheckProcedures = new dCheckProcedure[9];

            CheckProcedures[1] = new dCheckProcedure(RATSTAT1);
            CheckProcedures[2] = new dCheckProcedure(RATSTAT2);
            CheckProcedures[3] = new dCheckProcedure(RATSTAT3);
            CheckProcedures[4] = new dCheckProcedure(RATSTAT4);
            CheckProcedures[5] = new dCheckProcedure(RATSTAT5);
            CheckProcedures[6] = new dCheckProcedure(RATSTAT6);
            CheckProcedures[7] = new dCheckProcedure(RATSTAT7);
            CheckProcedures[8] = new dCheckProcedure(RATSTAT8);
        }

        /// <summary>
        /// Constructor used for testing.
        /// </summary>
        /// <param name="emissionParameters"></param>
        public cRATAStatusChecks(cEmissionsCheckParameters emissionParameters)
        {
            EmManualParameters = emissionParameters;
        }

        #endregion

        #region Public Static Methods: Checks

        public string RATSTAT1(cCategory Category, ref bool Log)
        // Check Low Sulfur and FLOW Exemptions     
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("Current_RATA_Status", null, eParameterDataType.String);
                OverrideRATABAF.SetValue(null, Category);
                Category.SetCheckParameter("Max_Level_Count", null, eParameterDataType.Integer);
                Category.SetCheckParameter("Flow_RATA_Exemption", false, eParameterDataType.Boolean);

                string SysTypeCd = EmParameters.QaStatusSystemTypeCode;
                string MonSysID = EmParameters.QaStatusSystemId;
                string MonLocID = EmParameters.CurrentMonitorLocationId;

                int RptPeriodID = Category.GetCheckParameter("Current_Reporting_Period").AsInteger(0);
                DataView TestExtensionExemptionRecords = Category.GetCheckParameter("Test_Extension_Exemption_Records").ValueAsDataView();

                DataView MonQualRecs = Category.GetCheckParameter("Monitor_Qualification_Records_By_Hour").ValueAsDataView();
                DataView MonQualRecsFound;
                sFilterPair[] MonQualFilter;

                if (SysTypeCd != null && SysTypeCd.StartsWith("SO2"))
                {
                    DataView TEERecsFound;
                    sFilterPair[] TEEFilter = new sFilterPair[3];
                    TEEFilter[0].Set("MON_SYS_ID", MonSysID);
                    TEEFilter[1].Set("RPT_PERIOD_ID", EmParameters.CurrentReportingPeriod, eFilterDataType.Integer);
                    TEEFilter[2].Set("EXTENS_EXEMPT_CD", "LOWSYTD");
                    TEERecsFound = FindRows(TestExtensionExemptionRecords, TEEFilter);
                    if (TEERecsFound.Count > 0)
                        Category.SetCheckParameter("Current_RATA_Status", "IC-Exempt", eParameterDataType.String);
                    else
                    {
                        MonQualFilter = new sFilterPair[2];
                        MonQualFilter[0].Set("MON_LOC_ID", MonLocID);
                        MonQualFilter[1].Set("QUAL_TYPE_CD", "LOWSULF");
                        MonQualRecsFound = FindRows(MonQualRecs, MonQualFilter);
                        if (MonQualRecsFound.Count > 0)
                            Category.SetCheckParameter("Current_RATA_Status", "IC-Exempt", eParameterDataType.String);
                    }
                }
                else if (SysTypeCd == "FLOW")
                {
                    bool PeakingBypass = false;
                    if (Category.GetCheckParameter("Current_Entity_Type").ValueAsString().InList("CS,MS"))
                    {
                        DataView AttributeRecords = Category.GetCheckParameter("Location_Attribute_Records_By_Hour_Location").ValueAsDataView();
                        PeakingBypass = true;
                        if (cDBConvert.ToInteger(AttributeRecords[0]["BYPASS_IND"]) != 1)
                        {
                            DataView UnitStackConfigRecs = (DataView)Category.GetCheckParameter("Unit_Stack_Configuration_Records_By_Hour_Location").ParameterValue;
                            MonQualFilter = new sFilterPair[2];
                            foreach (DataRowView drv in UnitStackConfigRecs)
                            {
                                MonQualFilter[0].Set("MON_LOC_ID", cDBConvert.ToString(drv["MON_LOC_ID"]));
                                MonQualFilter[1].Set("QUAL_TYPE_CD", "PK,SK", eFilterPairStringCompare.InList);
                                MonQualRecsFound = FindRows(MonQualRecs, MonQualFilter);
                                if (MonQualRecsFound.Count == 0)
                                {
                                    PeakingBypass = false;
                                    break;
                                }
                            }
                        }
                    }
                    else
                        if (Category.GetCheckParameter("Current_Unit_is_Peaking").ValueAsBool())
                        PeakingBypass = true;
                    if (PeakingBypass)
                        Category.SetCheckParameter("Max_Level_Count", 1, eParameterDataType.Integer);
                    else
                    {
                        MonQualFilter = new sFilterPair[2];
                        MonQualFilter[0].Set("MON_LOC_ID", MonLocID);
                        MonQualFilter[1].Set("QUAL_TYPE_CD", "PRATA1");
                        MonQualRecsFound = FindRows(MonQualRecs, MonQualFilter);
                        if (MonQualRecsFound.Count > 0)
                            Category.SetCheckParameter("Max_Level_Count", 1, eParameterDataType.Integer);
                        else
                        {
                            MonQualFilter[1].Set("QUAL_TYPE_CD", "PRATA2");
                            MonQualRecsFound = FindRows(MonQualRecs, MonQualFilter);
                            if (MonQualRecsFound.Count > 0)
                                Category.SetCheckParameter("Max_Level_Count", 2, eParameterDataType.Integer);
                            else
                            {
                                Category.SetCheckParameter("Max_Level_Count", 3, eParameterDataType.Integer);
                                FlowSystemIdArray.AggregateValue(MonSysID, LocationPos.Value.Value, Category);
                            }
                        }
                    }

                    // Set FlowRATAExemption
                    {
                        DataView TestExtensionExemptionView
                            = cRowFilter.FindRows(TestExtensionExemptionRecords,
                                                                        new cFilterCondition[]
                                    { new cFilterCondition("MON_SYS_ID", MonSysID),
                                      new cFilterCondition("COMPONENT_ID", EmParameters.QaStatusComponentId),
                                      new cFilterCondition("RPT_PERIOD_ID", RptPeriodID, eFilterDataType.Integer),
                                      new cFilterCondition("EXTENS_EXEMPT_CD", "FLOWEXP")});

                        if (TestExtensionExemptionView.Count > 0)
                            Category.SetCheckParameter("Flow_RATA_Exemption", true, eParameterDataType.Boolean);
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATSTAT1");
            }

            return ReturnVal;
        }

        public string RATSTAT2(cCategory Category, ref bool Log)
        //Locate Most Recent Prior RATA Test     
        {
            string ReturnVal = "";

            try
            {
                // Parameter Variables
                string ApplicableSystemIdList = null;

                // Initialize Parameters
                Category.SetCheckParameter("Prior_RATA_Record", null, eParameterDataType.DataRowView);
                Category.SetCheckParameter("Invalid_RATA_Record", null, eParameterDataType.DataRowView);
                Category.SetCheckParameter("Applicable_System_ID_List", ApplicableSystemIdList, eParameterDataType.String);

                // Get Current Hourly Data
                string MonSysID = EmParameters.QaStatusSystemId;
                DateTime CurrentDate = EmParameters.CurrentDateHour.AsStartDate();
                int CurrentHour = EmParameters.CurrentDateHour.AsStartHour();

                // Determine and Reset Applicable_System_ID_List
                {
                    if (Category.GetCheckParameter("Flow_RATA_Exemption").AsBoolean(false))
                    {
                        string ComponentID = EmParameters.QaStatusComponentId;

                        DataView MonitorSystemComponentRecords = Category.GetCheckParameter("Monitor_System_Component_Records_By_Hour_Location").AsDataView();

                        DataView MonitorSystemComponentView
                            = cRowFilter.FindRows(MonitorSystemComponentRecords,
                                                                        new cFilterCondition[] { new cFilterCondition("COMPONENT_ID", ComponentID) });

                        foreach (DataRowView MonitorSystemComponentRow in MonitorSystemComponentView)
                            ApplicableSystemIdList = ApplicableSystemIdList.ListAdd(MonitorSystemComponentRow["MON_SYS_ID"].AsString());
                    }
                    else
                        ApplicableSystemIdList = ApplicableSystemIdList.ListAdd(MonSysID);

                    Category.SetCheckParameter("Applicable_System_ID_List", ApplicableSystemIdList, eParameterDataType.String);
                }


                DataView RATATestRecs = Category.GetCheckParameter("RATA_Test_Records_By_Location_For_QA_Status").ValueAsDataView();
                DataView PriorRATATestRecs;
                sFilterPair[] Filter = new sFilterPair[2];
                Filter[0].Set("MON_SYS_ID", ApplicableSystemIdList, eFilterPairStringCompare.InList);
                Filter[1].Set("TEST_RESULT_CD", "INVALID", true);
                if (CurrentHour != 0)
                    PriorRATATestRecs = FindActiveRows(RATATestRecs, DateTime.MinValue, 0, CurrentDate, CurrentHour - 1, "END_DATE", "END_HOUR", "END_DATE", "END_HOUR", Filter);
                else
                    PriorRATATestRecs = FindActiveRows(RATATestRecs, DateTime.MinValue, 0, CurrentDate.AddDays(-1), 23, "END_DATE", "END_HOUR", "END_DATE", "END_HOUR", Filter);
                Filter[1].Set("TEST_RESULT_CD", "INVALID");
                if (PriorRATATestRecs.Count > 0)
                {
                    PriorRATATestRecs.Sort = "END_DATE DESC, END_HOUR DESC, END_MIN DESC";
                    Category.SetCheckParameter("Prior_RATA_Record", PriorRATATestRecs[0], eParameterDataType.DataRowView);
                    if (Category.GetCheckParameter("Current_RATA_Status").ParameterValue == null)
                    {
                        PriorRATATestRecs = FindActiveRows(RATATestRecs, cDBConvert.ToDate(PriorRATATestRecs[0]["END_DATE"], DateTypes.END), cDBConvert.ToHour(PriorRATATestRecs[0]["END_MIN"], DateTypes.END), CurrentDate, CurrentHour, "END_DATE", "END_HOUR", "END_DATE", "END_HOUR", Filter);
                        if (PriorRATATestRecs.Count > 0)
                        {
                            PriorRATATestRecs.Sort = "END_DATE DESC, END_HOUR DESC, END_MIN DESC";
                            Category.SetCheckParameter("Invalid_RATA_Record", PriorRATATestRecs[0], eParameterDataType.DataRowView);
                        }
                    }
                }
                else
                {
                    if (CurrentHour != 0)
                        PriorRATATestRecs = FindActiveRows(RATATestRecs, DateTime.MinValue, 0, CurrentDate, CurrentHour - 1, "END_DATE", "END_HOUR", "END_DATE", "END_HOUR", Filter);
                    else
                        PriorRATATestRecs = FindActiveRows(RATATestRecs, DateTime.MinValue, 0, CurrentDate.AddDays(-1), 23, "END_DATE", "END_HOUR", "END_DATE", "END_HOUR", Filter);
                    if (PriorRATATestRecs.Count > 0)
                    {
                        PriorRATATestRecs.Sort = "END_DATE DESC, END_HOUR DESC, END_MIN DESC";
                        Category.SetCheckParameter("Invalid_RATA_Record", PriorRATATestRecs[0], eParameterDataType.DataRowView);
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATSTAT2");
            }

            return ReturnVal;
        }

        public string RATSTAT3(cCategory Category, ref bool Log)
        //Locate Most Recent Prior Event     
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("Prior_RATA_Event_Record", null, eParameterDataType.DataRowView);
                if (Category.GetCheckParameter("Current_RATA_Status").ParameterValue == null)
                {
                    DataView QACertEventRecs = Category.GetCheckParameter("QA_Certification_Event_Records").ValueAsDataView();
                    QACertEventRecs.Sort = "QA_CERT_EVENT_DATE DESC, QA_CERT_EVENT_HOUR DESC";
                    string MonSysID = EmParameters.QaStatusSystemId;

                    DateTime CurrentDate = EmParameters.CurrentDateHour.AsStartDate();
                    int CurrentHour = EmParameters.CurrentDateHour.AsStartHour();
                    DataRowView PriorRATAEventRecFound = null;
                    DateTime CondBeginDate;
                    int CondBeginHour;
                    DateTime CertDate = DateTime.MinValue;//only because compiler requires initialization
                    int CertHour = int.MinValue;//only because compiler requires initialization
                    DateTime LastCompletedDate;
                    int LastCompletedHour;
                    DataRowView PriorRATARec = Category.GetCheckParameter("Prior_RATA_Record").ValueAsDataRowView();

                    string applicableSystemIdList = Category.GetCheckParameter("Applicable_System_ID_List").AsString();

                    foreach (DataRowView qaCertEventRec in QACertEventRecs)
                    {
                        if (qaCertEventRec["MON_SYS_ID"].AsString().InList(applicableSystemIdList) &&
                                (cDBConvert.ToString(qaCertEventRec["RATA_REQUIRED"]) == "Y"))
                        {
                            CondBeginDate = cDBConvert.ToDate(qaCertEventRec["CONDITIONAL_DATA_BEGIN_DATE"], DateTypes.START);
                            CondBeginHour = cDBConvert.ToInteger(qaCertEventRec["CONDITIONAL_DATA_BEGIN_HOUR"]);
                            CertDate = cDBConvert.ToDate(qaCertEventRec["QA_CERT_EVENT_DATE"], DateTypes.START);
                            CertHour = cDBConvert.ToInteger(qaCertEventRec["QA_CERT_EVENT_HOUR"]);
                            if ((CertDate < CurrentDate || (CertDate == CurrentDate && CertHour < CurrentHour)) || (CertDate == CurrentDate && CertHour == CurrentHour && CertDate == CondBeginDate && CertHour == CondBeginHour))
                            {
                                LastCompletedDate = cDBConvert.ToDate(qaCertEventRec["LAST_TEST_COMPLETED_DATE"], DateTypes.END);
                                LastCompletedHour = cDBConvert.ToInteger(qaCertEventRec["LAST_TEST_COMPLETED_HOUR"]);

                                if (PriorRATARec == null
                                    || CertDate > cDBConvert.ToDate(PriorRATARec["END_DATE"], DateTypes.END)
                                    || (CertDate == cDBConvert.ToDate(PriorRATARec["END_DATE"], DateTypes.END) && CertHour > cDBConvert.ToHour(PriorRATARec["END_HOUR"], DateTypes.END))
                                    || (CertDate == cDBConvert.ToDate(PriorRATARec["END_DATE"], DateTypes.END) && CertHour == cDBConvert.ToHour(PriorRATARec["END_HOUR"], DateTypes.END)
                                        && (LastCompletedDate == DateTime.MaxValue || LastCompletedDate > cDBConvert.ToDate(PriorRATARec["END_DATE"], DateTypes.END)
                                        || (LastCompletedDate == cDBConvert.ToDate(PriorRATARec["END_DATE"], DateTypes.END) && LastCompletedHour > cDBConvert.ToHour(PriorRATARec["END_HOUR"], DateTypes.END))))
                                    && (Category.GetCheckParameter("Annual_Reporting_Requirement").ValueAsBool()
                                        || CertDate >= new DateTime(CurrentDate.Year, 4, 1))
                                    && (EmParameters.QaStatusSystemTypeCode.NotInList("HCL,HF,HG,ST")
                                        || qaCertEventRec["QA_CERT_EVENT_CD"].AsString().InList("101,110,125,130"))
                                    )
                                {
                                    PriorRATAEventRecFound = qaCertEventRec;
                                    Category.SetCheckParameter("Prior_RATA_Event_Record", PriorRATAEventRecFound, eParameterDataType.DataRowView);
                                    break;
                                }
                            }
                        }
                    }
                    if (PriorRATAEventRecFound == null)
                    {
                        if (PriorRATARec == null)
                        {
                            Category.SetCheckParameter("Current_RATA_Status", "OOC-No Prior Test or Event", eParameterDataType.String);

                            if (EmParameters.CurrentMhvParameter == "FLOW")
                                OverrideRATABAF.SetValue(1.0m, Category);
                        }
                    }
                    else
                    {
                        DataRowView InvalRATARec = Category.GetCheckParameter("Invalid_RATA_Record").ValueAsDataRowView();
                        if (InvalRATARec != null && (CertDate > cDBConvert.ToDate(InvalRATARec["END_DATE"], DateTypes.END) || (CertDate == cDBConvert.ToDate(InvalRATARec["END_DATE"], DateTypes.END) && CertHour > cDBConvert.ToHour(InvalRATARec["END_HOUR"], DateTypes.END))))
                        {
                            DataView RATARecs = Category.GetCheckParameter("RATA_Test_Records_By_Location_For_QA_Status").ValueAsDataView();
                            DataView RATARecsFound;
                            sFilterPair[] Filter = new sFilterPair[2];
                            Filter[0].Set("MON_SYS_ID", MonSysID);
                            Filter[1].Set("TEST_RESULT_CD", "INVALID");
                            RATARecsFound = FindActiveRows(RATARecs, CertDate, CertHour, CurrentDate, CurrentHour, "END_DATE", "END_HOUR", "END_DATE", "END_HOUR", Filter);
                            if (RATARecsFound.Count > 0)
                            {
                                RATARecsFound.Sort = "END_DATE ASC, END_HOUR ASC";
                                Category.SetCheckParameter("Invalid_RATA_Record", RATARecsFound[0], eParameterDataType.DataRowView);
                            }
                            else
                                Category.SetCheckParameter("Invalid_RATA_Record", null, eParameterDataType.DataRowView);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATSTAT3");
            }

            return ReturnVal;
        }

        public string RATSTAT4(cCategory Category, ref bool Log)
        //Check RATA Result     
        {
            string ReturnVal = "";

            try
            {
                string Status = Category.GetCheckParameter("Current_RATA_Status").ValueAsString();
                DataRowView PriorRATARec = Category.GetCheckParameter("Prior_RATA_Record").ValueAsDataRowView();
                DataRowView PriorRATAEventRec = Category.GetCheckParameter("Prior_RATA_Event_Record").ValueAsDataRowView();
                Category.SetCheckParameter("Evaluate_Multi_Level_RATA", false, eParameterDataType.Boolean);//I made this part of the spec up

                if (Status == "" && PriorRATARec != null && PriorRATAEventRec == null)
                {
                    Category.SetCheckParameter("Evaluate_Multi_Level_RATA", true, eParameterDataType.Boolean);
                    if (cDBConvert.ToString(PriorRATARec["QA_NEEDS_EVAL_FLG"]) == "Y")
                        Status = "Prior Test Not Yet Evaluated";
                    else
                    {
                        string TestResCd = cDBConvert.ToString(PriorRATARec["TEST_RESULT_CD"]);
                        if (TestResCd == "" || TestResCd.InList("FAILED,ABORTED"))
                        {
                            DataView QACertEventRecs = Category.GetCheckParameter("QA_Certification_Event_Records").ValueAsDataView();

                            string applicableSystemIdList = Category.GetCheckParameter("Applicable_System_ID_List").AsString();
                            sFilterPair[] Filter = new sFilterPair[2];
                            Filter[0].Set("MON_SYS_ID", applicableSystemIdList, eFilterPairStringCompare.InList);
                            Filter[1].Set("RATA_REQUIRED", "Y");

                            DataView QACertEventRecsFound = FindRows(QACertEventRecs, Filter);
                            QACertEventRecsFound.Sort = "QA_CERT_EVENT_DATE DESC, QA_CERT_EVENT_HOUR DESC";

                            DateTime CondBeginDate;
                            int CondBeginHour;
                            DateTime CurrentDate = EmParameters.CurrentDateHour.AsStartDate();
                            int CurrentHour = EmParameters.CurrentDateHour.AsStartHour();
                            DateTime PriorRATAEndDate = cDBConvert.ToDate(PriorRATARec["END_DATE"], DateTypes.END);
                            int PriorRATAEndHour = cDBConvert.ToHour(PriorRATARec["END_HOUR"], DateTypes.END);

                            foreach (DataRowView drv in QACertEventRecsFound)
                            {
                                CondBeginDate = cDBConvert.ToDate(drv["CONDITIONAL_DATA_BEGIN_DATE"], DateTypes.START);
                                CondBeginHour = cDBConvert.ToInteger(drv["CONDITIONAL_DATA_BEGIN_HOUR"]);
                                if (CondBeginDate < CurrentDate || (CondBeginDate == CurrentDate && CondBeginHour <= CurrentHour))
                                    if (CondBeginDate > PriorRATAEndDate || (CondBeginDate == PriorRATAEndDate && CondBeginHour >= PriorRATAEndHour))
                                        if (Category.GetCheckParameter("Annual_Reporting_Requirement").ValueAsBool() || cDBConvert.ToDate(drv["QA_CERT_EVENT_DATE"], DateTypes.START) >= new DateTime(CurrentDate.Year, 4, 1))
                                        {
                                            PriorRATAEventRec = drv;
                                            break;
                                        }
                            }
                            if (PriorRATAEventRec != null)
                                Category.SetCheckParameter("Prior_RATA_Event_Record", PriorRATAEventRec, eParameterDataType.DataRowView);
                            else
                                if (TestResCd == "")
                            {
                                Status = "OOC-Prior Test Has Critical Errors";

                                if (EmParameters.CurrentMhvParameter == "FLOW")
                                    OverrideRATABAF.SetValue(PriorRATARec["OVERALL_BIAS_ADJ_FACTOR"].AsDecimal(), Category);
                            }
                            else if (TestResCd == "FAILED")
                            {
                                Status = "OOC-Prior Test Failed";

                                if (EmParameters.CurrentMhvParameter == "FLOW")
                                    OverrideRATABAF.SetValue(PriorRATARec["OVERALL_BIAS_ADJ_FACTOR"].AsDecimal(), Category);
                            }
                            else if (TestResCd == "ABORTED")
                            {
                                Status = "OOC-Prior Test Aborted";

                                if (EmParameters.CurrentMhvParameter == "FLOW")
                                    OverrideRATABAF.SetValue(PriorRATARec["OVERALL_BIAS_ADJ_FACTOR"].AsDecimal(), Category);
                            }
                        }
                    }
                }
                if (Status != "")
                    Category.SetCheckParameter("Current_RATA_Status", Status, eParameterDataType.String);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATSTAT4");
            }

            return ReturnVal;
        }

        public string RATSTAT5(cCategory Category, ref bool Log)
        //Determine Event Conditional Status
        {
            string ReturnVal = "";

            try
            {
                DataRowView SubsqRATARec = null;
                Category.SetCheckParameter("RATA_Missing_Op_Data_Info", null, eParameterDataType.String);
                string Status = Category.GetCheckParameter("Current_RATA_Status").ValueAsString();
                DataRowView PriorRATAEventRec = Category.GetCheckParameter("Prior_RATA_Event_Record").ValueAsDataRowView();
                Category.SetCheckParameter("RATA_Event_Operating_Level_Count", null, eParameterDataType.Integer);
                if (Status == "" && PriorRATAEventRec != null)
                {
                    DateTime CondBeginDate = cDBConvert.ToDate(PriorRATAEventRec["CONDITIONAL_DATA_BEGIN_DATE"], DateTypes.START);
                    int CondBeginHour = cDBConvert.ToHour(PriorRATAEventRec["CONDITIONAL_DATA_BEGIN_HOUR"], DateTypes.START);
                    DateTime CurrentDate = EmParameters.CurrentDateHour.AsStartDate();
                    int CurrentHour = EmParameters.CurrentDateHour.AsStartHour();
                    int CurrentYear = CurrentDate.Year;
                    int CurrentQtr = cDateFunctions.ThisQuarter(CurrentDate);
                    if (CondBeginDate == DateTime.MinValue || (CurrentDate < CondBeginDate || (CurrentDate == CondBeginDate && CurrentHour < CondBeginHour)))
                    {
                        Status = "OOC-Event";

                        if (EmParameters.CurrentMhvParameter == "FLOW")
                            OverrideRATABAF.SetValue(1.0m, Category);
                    }
                    else
                    {
                        DataView RATARecs = Category.GetCheckParameter("RATA_Test_Records_By_Location_For_QA_Status").ValueAsDataView();

                        sFilterPair[] RATATestRecsFilter = new sFilterPair[2];
                        RATATestRecsFilter[0].Set("MON_SYS_ID", cDBConvert.ToString(PriorRATAEventRec["MON_SYS_ID"]));
                        RATATestRecsFilter[1].Set("TEST_RESULT_CD", "INVALID", true);

                        DataView RATARecsFound = FindActiveRows(RATARecs, CondBeginDate, CondBeginHour, DateTime.MaxValue, 23, "END_DATE", "END_HOUR", "END_DATE", "END_HOUR", RATATestRecsFilter);

                        if (RATARecsFound.Count > 0)
                        {
                            RATARecsFound.Sort = "END_DATE ASC, END_HOUR ASC";
                            SubsqRATARec = RATARecsFound[0];
                            if (cDBConvert.ToString(SubsqRATARec["QA_NEEDS_EVAL_FLG"]) == "Y")
                                Status = "Recertification Test Not Yet Evaluated";
                            else
                            {
                                string TestResCd = cDBConvert.ToString(SubsqRATARec["TEST_RESULT_CD"]);
                                if (TestResCd == "")
                                {
                                    Status = "OOC-Recertification Test Has Critical Errors";

                                    if (EmParameters.CurrentMhvParameter == "FLOW")
                                        OverrideRATABAF.SetValue(SubsqRATARec["OVERALL_BIAS_ADJ_FACTOR"].AsDecimal(), Category);
                                }
                                else if (TestResCd == "FAILED")
                                {
                                    Status = "OOC-Recertification Test Failed";

                                    if (EmParameters.CurrentMhvParameter == "FLOW")
                                        OverrideRATABAF.SetValue(SubsqRATARec["OVERALL_BIAS_ADJ_FACTOR"].AsDecimal(), Category);
                                }
                                else if (TestResCd == "ABORTED")
                                {
                                    Status = "OOC-Recertification Test Aborted";

                                    if (EmParameters.CurrentMhvParameter == "FLOW")
                                        OverrideRATABAF.SetValue(SubsqRATARec["OVERALL_BIAS_ADJ_FACTOR"].AsDecimal(), Category);
                                }
                                else
                                {
                                    int ReqLevelCount = 1;
                                    if (Category.GetCheckParameter("Prior_RATA_Record").ParameterValue == null)
                                        ReqLevelCount = Category.GetCheckParameter("Max_Level_Count").ValueAsInt();
                                    else
                                        if (cDBConvert.ToString(PriorRATAEventRec["RATA3_REQUIRED"]) == "Y")
                                        ReqLevelCount = 3;
                                    else
                                            if (cDBConvert.ToString(PriorRATAEventRec["RATA2_REQUIRED"]) == "Y")
                                        ReqLevelCount = 2;

                                    if (cDBConvert.ToString(SubsqRATARec["OP_LEVEL_CD_LIST"]).ListCount() < ReqLevelCount)
                                    {
                                        Status = "OOC-Incomplete Recertification";

                                        if (EmParameters.CurrentMhvParameter == "FLOW")
                                            OverrideRATABAF.SetValue(SubsqRATARec["OVERALL_BIAS_ADJ_FACTOR"].AsDecimal(), Category);
                                    }
                                    else
                                        Category.SetCheckParameter("RATA_Event_Operating_Level_Count", ReqLevelCount, eParameterDataType.Integer);
                                }
                            }
                            if (Category.GetCheckParameter("Invalid_RATA_Record").ValueAsDataRowView() == null)
                            {
                                RATATestRecsFilter[1].Set("TEST_RESULT_CD", "INVALID");
                                RATARecsFound = FindActiveRows(RATARecs, CondBeginDate, CondBeginHour, cDBConvert.ToDate(SubsqRATARec["END_DATE"], DateTypes.END), cDBConvert.ToHour(SubsqRATARec["END_HOUR"], DateTypes.END), "END_DATE", "END_HOUR", "END_DATE", "END_HOUR", RATATestRecsFilter);
                                if (RATARecsFound.Count > 0)
                                    Category.SetCheckParameter("Invalid_RATA_Record", RATARecsFound[0], eParameterDataType.DataRowView);
                            }
                        }
                        else
                        {
                            int ReqLevelCount = 1;
                            if (cDBConvert.ToString(PriorRATAEventRec["RATA3_REQUIRED"]) == "Y")
                                ReqLevelCount = 3;
                            else
                                if (cDBConvert.ToString(PriorRATAEventRec["RATA2_REQUIRED"]) == "Y")
                                ReqLevelCount = 2;
                            Category.SetCheckParameter("RATA_Event_Operating_Level_Count", ReqLevelCount, eParameterDataType.Integer);
                        }
                        bool AnnRptReq = Category.GetCheckParameter("Annual_Reporting_Requirement").ValueAsBool();
                        if (Status == "" && !AnnRptReq)
                            if (SubsqRATARec != null)
                            {
                                DateTime SubsqEndDate = cDBConvert.ToDate(SubsqRATARec["END_DATE"], DateTypes.END);
                                int SubsqEndHour = cDBConvert.ToHour(SubsqRATARec["END_HOUR"], DateTypes.END);
                                DateTime OctDate = new DateTime(CurrentYear, 10, 30);
                                if (SubsqEndDate > OctDate)
                                {
                                    Status = "OOC-Conditional Period Expired";

                                    if (EmParameters.CurrentMhvParameter == "FLOW")
                                        OverrideRATABAF.SetValue(1.0m, Category);
                                }
                            }
                            else if (SubsqRATARec == null && CurrentQtr == 3)
                            {
                                Status = "OOC-Conditional Period Expired";

                                if (EmParameters.CurrentMhvParameter == "FLOW")
                                    OverrideRATABAF.SetValue(1.0m, Category);
                            }

                        if (Status == "")
                        {
                            string EventId = cDBConvert.ToString(PriorRATAEventRec["QA_CERT_EVENT_ID"]);
                            string EventCd = cDBConvert.ToString(PriorRATAEventRec["QA_CERT_EVENT_CD"]);
                            DateTime CertEventDate = cDBConvert.ToDate(PriorRATAEventRec["QA_CERT_EVENT_DATE"], DateTypes.START);
                            DataView OpSuppDataRecs = Category.GetCheckParameter("Operating_Supp_Data_Records_by_Location").ValueAsDataView();
                            DataView OpSuppDataRecsFound;
                            sFilterPair[] OpSuppFilter;
                            DataView ReportingFrequencyByLocationQuarterRecs = Category.GetCheckParameter("Reporting_Frequency_by_Location_Quarter").ValueAsDataView();

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
                            DateTime EarliestLocRptDate = Category.GetCheckParameter("Earliest_Location_Report_Date").ValueAsDateTime(DateTypes.START);
                            int[] OpHrsAccumArray = Category.GetCheckParameter("Rpt_Period_Op_Hours_Accumulator_Array").ValueAsIntArray();
                            int[] OpDaysAccumArray = Category.GetCheckParameter("Rpt_Period_Op_Days_Accumulator_Array").ValueAsIntArray();
                            int CurrentPosition = Category.GetCheckParameter("Current_Monitor_Plan_Location_Postion").ValueAsInt();

                            if (cDBConvert.ToString(PriorRATAEventRec["RATA_CERT_EVENT"]) == "Y" && (PriorRATAEventRec["SYS_TYPE_CD"].AsString() != "HF"))
                            {
                                string ParameterCd = EmParameters.CurrentMhvParameter;
                                if ((ParameterCd != "FLOW" && EventCd == "125") || (ParameterCd == "FLOW" && EventCd == "305"))
                                {
                                    DateTime SysBeginDate = cDBConvert.ToDate(PriorRATAEventRec["SYS_BEGIN_DATE"], DateTypes.START);
                                    if (SysBeginDate == DateTime.MinValue)
                                        Status = "Invalid Monitor System";
                                    else
                                    {
                                        string EventRecSysTypeCd = cDBConvert.ToString(PriorRATAEventRec["SYS_TYPE_CD"]);
                                        DataView LocProgramRecs = Category.GetCheckParameter("Location_Program_Records_By_Hour_Location").ValueAsDataView();
                                        DataView LocProgramRecsFound;
                                        sFilterPair[] LocProgFilter;

                                        if (EventRecSysTypeCd == "SO2")
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
                                        else if (EventRecSysTypeCd == "NOX")
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
                                        else if (EventRecSysTypeCd == "NOXC")
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
                                        else if (EventRecSysTypeCd.InList("HCL,HG,ST"))
                                        {
                                            LocProgFilter = new sFilterPair[1];
                                            LocProgFilter[0].Set("PRG_CD", "MATS");
                                            LocProgramRecsFound = FindActiveRows(LocProgramRecs, DateTime.MinValue, SysBeginDate, "UNIT_MONITOR_CERT_BEGIN_DATE", "UNIT_MONITOR_CERT_BEGIN_DATE", true, true, LocProgFilter);

                                            if (LocProgramRecsFound.Count == 0)
                                            {
                                                LocProgFilter = new sFilterPair[2];
                                                LocProgFilter[0].Set("PRG_CD", "MATS");
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
                                            Status = "Missing Program";
                                        else
                                        {
                                            DateTime UnitMonCertDeadline = cDBConvert.ToDate(LocProgramRecsFound[0]["UNIT_MONITOR_CERT_DEADLINE"], DateTypes.START);
                                            if (UnitMonCertDeadline != DateTime.MinValue)
                                                if (CurrentDate < UnitMonCertDeadline)
                                                    Status = "IC-Conditional";
                                                else
                                                {
                                                    Status = "OOC-Conditional Period Expired";

                                                    if (EmParameters.CurrentMhvParameter == "FLOW")
                                                        OverrideRATABAF.SetValue(1.0m, Category);
                                                }
                                            else
                                            {
                                                DateTime UnitMonCertBeginDate = cDBConvert.ToDate(LocProgramRecsFound[0]["UNIT_MONITOR_CERT_BEGIN_DATE"], DateTypes.START);
                                                if (CurrentDate < UnitMonCertBeginDate.AddDays(180))
                                                    Status = "IC-Conditional";
                                                else
                                                {
                                                    Status = "OOC-Conditional Period Expired";

                                                    if (EmParameters.CurrentMhvParameter == "FLOW")
                                                        OverrideRATABAF.SetValue(1.0m, Category);
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    int DateDiff = cDateFunctions.DateDifference(CertEventDate, CurrentDate);
                                    if (DateDiff + 1 > 180)// add one for inclusivity
                                    {
                                        Status = "OOC-Conditional Period Expired";

                                        if (EmParameters.CurrentMhvParameter == "FLOW")
                                            OverrideRATABAF.SetValue(1.0m, Category);
                                    }
                                    else
                                    {
                                        if (CertEventDate.Year == CurrentYear && cDateFunctions.ThisQuarter(CertEventDate) == CurrentQtr)
                                            if (DateDiff + 1 > 90)// add one for inclusivity
                                            {
                                                /* Selects operating hour counts based on system if Primary/Primary-Bypass systems (stacks) are invovled. */
                                                if ((EmParameters.PrimaryBypassActiveForHour == true) && (EmParameters.QaStatusSystemTypeCode == "NOX"))
                                                {
                                                    int? dayCount;

                                                    if (((dayCount = cLinearityStatusChecks.LinStat5QaCertEventDays(Category.CurrentMonLocPos, EventId)) == null) || (dayCount > 90))
                                                        Status = "OOC-Conditional Period Expired";
                                                    else
                                                        Status = "IC-Conditional";
                                                }
                                                else
                                                {
                                                    if (OpHrsAccumArray[CurrentPosition] == -1)
                                                        Status = "Invalid Op Data";
                                                    else
                                                    if (DateDiff + 1 == OpDaysAccumArray[CurrentPosition])
                                                    {
                                                        Status = "OOC-Conditional Period Expired";

                                                        if (EmParameters.CurrentMhvParameter == "FLOW")
                                                            OverrideRATABAF.SetValue(1.0m, Category);
                                                    }
                                                }
                                            }
                                            else
                                                Status = "IC-Conditional";
                                        else
                                            if (PriorRATAEventRec["MIN_OP_DAYS_PRIOR_QTR"] == DBNull.Value)
                                        {
                                            PriorRATAEventRec["MIN_OP_DAYS_PRIOR_QTR"] = 0;
                                            PriorRATAEventRec["MAX_OP_DAYS_PRIOR_QTR"] = 0;
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
                                                        if ((EmParameters.PrimaryBypassActiveForHour == true) && (EmParameters.QaStatusSystemTypeCode == "NOX"))
                                                        {
                                                            CheckDataView<SystemOpSuppData> systemOpSuppData
                                                                = EmParameters.SystemOperatingSuppDataRecordsByLocation.FindRows(new cFilterCondition("MON_SYS_ID", EmParameters.QaStatusSystemId),
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
                                                            ReportingFrequencyByLocationQuarterRecs.RowFilter = string.Format("Calendar_Year={0} and Quarter={1}", i, j);
                                                            sFilterPair[] quarterlyFilter = new sFilterPair[1];
                                                            quarterlyFilter[0].Set("Report_Freq_Cd", "Q");
                                                            DataView quarterlyFreqency = FindRows(ReportingFrequencyByLocationQuarterRecs, quarterlyFilter);
                                                            if ((ReportingFrequencyByLocationQuarterRecs.Count > 0)
                                                                && (j.InRange(2, 3) || (quarterlyFreqency.Count > 0)))
                                                            {
                                                                Category.SetCheckParameter("RATA_Missing_Op_Data_Info", i + "Q" + j, eParameterDataType.String);
                                                                PriorRATAEventRec["MIN_OP_DAYS_PRIOR_QTR"] = -1;
                                                                StopLooking = true;
                                                                break;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (j == CertEventQtr && i == CertEventYear)
                                                            {
                                                                // Determine whether supplemental count exists, try system specific for Primary-Bypass situation first.
                                                                int? supplementalCount = null;
                                                                {
                                                                    if ((EmParameters.PrimaryBypassActiveForHour == true) && (EmParameters.QaStatusSystemTypeCode == "NOX") &&
                                                                        (PriorRATAEventRec["QA_CERT_EVENT_DATE_SYSTEM_SUPP_DATA_EXISTS_IND"].AsInteger() == 1))
                                                                    {
                                                                        supplementalCount = PriorRATAEventRec["QA_CERT_EVENT_SYSTEM_OP_DAY_COUNT"].AsShort(0);
                                                                    }

                                                                    if (!supplementalCount.HasValue && (PriorRATAEventRec["QA_CERT_EVENT_DATE_SUPP_DATA_EXISTS_IND"].AsInteger() == 1))
                                                                    {
                                                                        supplementalCount = PriorRATAEventRec["QA_CERT_EVENT_OP_DAY_COUNT"].AsShort(0);
                                                                    }
                                                                }

                                                                if (supplementalCount.HasValue)
                                                                {
                                                                    PriorRATAEventRec["MIN_OP_DAYS_PRIOR_QTR"] = (int)PriorRATAEventRec["MIN_OP_DAYS_PRIOR_QTR"] + supplementalCount.Value;
                                                                    PriorRATAEventRec["MAX_OP_DAYS_PRIOR_QTR"] = (int)PriorRATAEventRec["MAX_OP_DAYS_PRIOR_QTR"] + supplementalCount.Value;
                                                                }
                                                                else
                                                                {
                                                                    DaySpread1 = operatingDaysCount.Value - cDateFunctions.DateDifference(cDateFunctions.StartDateThisQuarter(CertEventDate), CertEventDate);

                                                                    if (DaySpread1 > 0)
                                                                        PriorRATAEventRec["MIN_OP_DAYS_PRIOR_QTR"] = DaySpread1;

                                                                    DaySpread2 = cDateFunctions.DateDifference(CertEventDate.AddDays(-1), cDateFunctions.LastDateThisQuarter(CertEventDate));

                                                                    if (operatingDaysCount.Value < DaySpread2)
                                                                        PriorRATAEventRec["MAX_OP_DAYS_PRIOR_QTR"] = operatingDaysCount.Value;
                                                                    else
                                                                        PriorRATAEventRec["MAX_OP_DAYS_PRIOR_QTR"] = DaySpread2;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                PriorRATAEventRec["MIN_OP_DAYS_PRIOR_QTR"] = (int)PriorRATAEventRec["MIN_OP_DAYS_PRIOR_QTR"] + operatingDaysCount.Value;
                                                                PriorRATAEventRec["MAX_OP_DAYS_PRIOR_QTR"] = (int)PriorRATAEventRec["MAX_OP_DAYS_PRIOR_QTR"] + operatingDaysCount.Value;
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
                                            if ((EmParameters.PrimaryBypassActiveForHour == true) && (EmParameters.QaStatusSystemTypeCode == "NOX"))
                                            {
                                                currentOpDays = EmParameters.SystemOperatingSuppDataDictionaryArray[Category.CurrentMonLocPos][EmParameters.QaStatusPrimaryOrPrimaryBypassSystemId].QuarterlyOperatingCounts.Days;
                                            }
                                            else
                                            {
                                                currentOpDays = OpDaysAccumArray[CurrentPosition];
                                            }

                                            if ((int)PriorRATAEventRec["MIN_OP_DAYS_PRIOR_QTR"] == -1)
                                                Status = "Missing Op Data";
                                            else if ((int)PriorRATAEventRec["MIN_OP_DAYS_PRIOR_QTR"] + currentOpDays > 90)
                                            {
                                                Status = "OOC-Conditional Period Expired";

                                                if (EmParameters.CurrentMhvParameter == "FLOW")
                                                    OverrideRATABAF.SetValue(1.0m, Category);
                                            }
                                            else if ((int)PriorRATAEventRec["MIN_OP_DAYS_PRIOR_QTR"] == (int)PriorRATAEventRec["MAX_OP_DAYS_PRIOR_QTR"])
                                                Status = "IC-Conditional"; // If the max and min or the same then supplemental data was used.
                                            else if ((int)PriorRATAEventRec["MAX_OP_DAYS_PRIOR_QTR"] + currentOpDays > 90)
                                                Status = "Undetermined-Conditional Data";
                                            else
                                                Status = "IC-Conditional";
                                        }
                                        else
                                            Status = "IC-Conditional";
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
                                    if ((EmParameters.PrimaryBypassActiveForHour == true) && (EmParameters.QaStatusSystemTypeCode == "NOX"))
                                    {
                                        int? hourCount;

                                        if (((hourCount = cLinearityStatusChecks.LinStat5ConditionalDataBeginHours(Category.CurrentMonLocPos, EventId)) == null) || (hourCount > 720))
                                            Status = "OOC-Conditional Period Expired";
                                        else
                                            Status = "IC-Conditional";
                                    }
                                    else
                                    {
                                        DataView HrOpRecs = Category.GetCheckParameter("Hourly_Operating_Data_Records_for_Location").ValueAsDataView();
                                        sFilterPair[] HrOpFilter = new sFilterPair[1];
                                        HrOpFilter[0].Set("OP_TIME", 0, eFilterDataType.Decimal, eFilterPairRelativeCompare.GreaterThan);
                                        DataView HrOpRecsFound;
                                        HrOpRecsFound = FindActiveRows(HrOpRecs, CondBeginDate, CondBeginHour, CurrentDate, CurrentHour, "BEGIN_DATE", "BEGIN_HOUR", "BEGIN_DATE", "BEGIN_HOUR", HrOpFilter);
                                        if (HrOpRecsFound.Count > 720)
                                        {
                                            Status = "OOC-Conditional Period Expired";

                                            if (EmParameters.CurrentMhvParameter == "FLOW")
                                                OverrideRATABAF.SetValue(1.0m, Category);
                                        }
                                        else
                                            Status = "IC-Conditional";
                                    }
                                }
                                else
                                {
                                    if (PriorRATAEventRec["MIN_OP_HOURS_PRIOR_QTR"] == DBNull.Value)
                                    {
                                        PriorRATAEventRec["MIN_OP_HOURS_PRIOR_QTR"] = 0;
                                        PriorRATAEventRec["MAX_OP_HOURS_PRIOR_QTR"] = 0;
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
                                                    if ((EmParameters.PrimaryBypassActiveForHour == true) && (EmParameters.QaStatusSystemTypeCode == "NOX"))
                                                    {
                                                        string opSuppDataTypeCd = (EmParameters.AnnualReportingRequirement == true) || (j != 2) ? "OP" : "OPMJ";

                                                        CheckDataView<SystemOpSuppData> systemOpSuppData
                                                            = EmParameters.SystemOperatingSuppDataRecordsByLocation.FindRows(new cFilterCondition("MON_SYS_ID", EmParameters.QaStatusSystemId),
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
                                                        ReportingFrequencyByLocationQuarterRecs.RowFilter = string.Format("Calendar_Year={0} and Quarter={1}", i, j);
                                                        sFilterPair[] quarterlyFilter = new sFilterPair[1];
                                                        quarterlyFilter[0].Set("Report_Freq_Cd", "Q");
                                                        DataView quarterlyFreqency = FindRows(ReportingFrequencyByLocationQuarterRecs, quarterlyFilter);
                                                        if ((ReportingFrequencyByLocationQuarterRecs.Count > 0)
                                                            && (j.InRange(2, 3) || (quarterlyFreqency.Count > 0)))
                                                        {
                                                            Category.SetCheckParameter("RATA_Missing_Op_Data_Info", i + "Q" + j, eParameterDataType.String);
                                                            PriorRATAEventRec["MIN_OP_HOURS_PRIOR_QTR"] = -1;
                                                            StopLooking = true;
                                                            break;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (j == CondBeginQtr && i == CondBeginYear)
                                                        {
                                                            // Determine whether supplemental count exists, try system specific for Primary-Bypass situation first.
                                                            int? supplementalCount = null;
                                                            {
                                                                if ((EmParameters.PrimaryBypassActiveForHour == true) && (EmParameters.QaStatusSystemTypeCode == "NOX") &&
                                                                    (PriorRATAEventRec["CONDITIONAL_BEGIN_HOUR_SYSTEM_SUPP_DATA_EXISTS_IND"].AsInteger() == 1))
                                                                {
                                                                    supplementalCount = PriorRATAEventRec["CONDITIONAL_BEGIN_SYSTEM_OP_HOUR_COUNT"].AsShort(0);
                                                                }

                                                                if (!supplementalCount.HasValue && (PriorRATAEventRec["CONDITIONAL_BEGIN_HOUR_SUPP_DATA_EXISTS_IND"].AsInteger() == 1))
                                                                {
                                                                    supplementalCount = PriorRATAEventRec["CONDITIONAL_BEGIN_OP_HOUR_COUNT"].AsShort(0);
                                                                }
                                                            }

                                                            if (supplementalCount.HasValue)
                                                            {
                                                                PriorRATAEventRec["MIN_OP_HOURS_PRIOR_QTR"] = (int)PriorRATAEventRec["MIN_OP_HOURS_PRIOR_QTR"] + supplementalCount.Value;
                                                                PriorRATAEventRec["MAX_OP_HOURS_PRIOR_QTR"] = (int)PriorRATAEventRec["MAX_OP_HOURS_PRIOR_QTR"] + supplementalCount.Value;
                                                            }
                                                            else
                                                            {
                                                                HourSpread1 = operatingHoursCount.Value - (24 * cDateFunctions.DateDifference(cDateFunctions.StartDateThisQuarter(CondBeginDate), CondBeginDate) + CondBeginHour);
                                                                if (HourSpread1 > 0)
                                                                    PriorRATAEventRec["MIN_OP_HOURS_PRIOR_QTR"] = HourSpread1;
                                                                HourSpread2 = 24 * cDateFunctions.DateDifference(CondBeginDate.AddDays(-1), cDateFunctions.LastDateThisQuarter(CondBeginDate)) + CondBeginHour;
                                                                if (operatingHoursCount.Value < HourSpread2)
                                                                    PriorRATAEventRec["MAX_OP_HOURS_PRIOR_QTR"] = operatingHoursCount.Value;
                                                                else
                                                                    PriorRATAEventRec["MAX_OP_HOURS_PRIOR_QTR"] = HourSpread2;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            PriorRATAEventRec["MIN_OP_HOURS_PRIOR_QTR"] = (int)PriorRATAEventRec["MIN_OP_HOURS_PRIOR_QTR"] + operatingHoursCount.Value;
                                                            PriorRATAEventRec["MAX_OP_HOURS_PRIOR_QTR"] = (int)PriorRATAEventRec["MAX_OP_HOURS_PRIOR_QTR"] + operatingHoursCount.Value;
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
                                    if ((EmParameters.PrimaryBypassActiveForHour == true) && (EmParameters.QaStatusSystemTypeCode == "NOX"))
                                    {
                                        currentOpHours = EmParameters.SystemOperatingSuppDataDictionaryArray[Category.CurrentMonLocPos][EmParameters.QaStatusSystemId].QuarterlyOperatingCounts.Hours;
                                    }
                                    else
                                    {
                                        currentOpHours = OpHrsAccumArray[CurrentPosition];
                                    }

                                    if ((int)PriorRATAEventRec["MIN_OP_HOURS_PRIOR_QTR"] == -1)
                                        Status = "Missing Op Data";
                                    else if (currentOpHours == -1)
                                    {
                                        if ((int)PriorRATAEventRec["MIN_OP_HOURS_PRIOR_QTR"] > 720)
                                        {
                                            Status = "OOC-Conditional Period Expired";

                                            if (EmParameters.CurrentMhvParameter == "FLOW")
                                                OverrideRATABAF.SetValue(1.0m, Category);
                                        }
                                        else
                                            Status = "Invalid Op Data";
                                    }
                                    else
                                    {
                                        if ((int)PriorRATAEventRec["MIN_OP_HOURS_PRIOR_QTR"] + currentOpHours > 720)
                                        {
                                            Status = " OOC-Conditional Period Expired";

                                            if (EmParameters.CurrentMhvParameter == "FLOW")
                                                OverrideRATABAF.SetValue(1.0m, Category);
                                        }
                                        else if ((int)PriorRATAEventRec["MIN_OP_HOURS_PRIOR_QTR"] == (int)PriorRATAEventRec["MAX_OP_HOURS_PRIOR_QTR"])
                                            Status = "IC-Conditional"; // If the max and min or the same then supplemental data was used.
                                        else if ((int)PriorRATAEventRec["MAX_OP_HOURS_PRIOR_QTR"] + currentOpHours > 720)
                                            Status = "Undetermined-Conditional Data";
                                        else
                                            Status = "IC-Conditional";
                                    }
                                }
                            }
                        }
                    }
                }
                Category.SetCheckParameter("Subsequent_RATA_Record", SubsqRATARec, eParameterDataType.DataRowView);
                if (Status != "")
                    Category.SetCheckParameter("Current_RATA_Status", Status, eParameterDataType.String);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATSTAT5");
            }

            return ReturnVal;
        }

        public string RATSTAT6(cCategory Category, ref bool Log)
        //Evaluate Prior Multi-Level RATA    
        {
            string ReturnVal = "";

            try
            {
                string ParameterCd = EmParameters.CurrentMhvParameter;
                string Status = Category.GetCheckParameter("Current_RATA_Status").ValueAsString();
                Category.SetCheckParameter("Prior_Rata_Is_Alternate_Single_Level_RATA", false, eParameterDataType.DataRowView);
                DataRowView PriorRATARec = Category.GetCheckParameter("Prior_RATA_Record").ValueAsDataRowView();
                int EventLvlCount = Category.GetCheckParameter("RATA_Event_Operating_Level_Count").ValueAsInt();

                if (ParameterCd == "FLOW" && PriorRATARec != null
                        && (Status == "" || Status.StartsWith("IC") || Status.StartsWith("Undetermined")))
                {
                    Category.SetCheckParameter("Prior_Rata_Is_Alternate_Single_Level_RATA", true, eParameterDataType.DataRowView);
                    DataRowView PriorMultiLvlRec = null;//variable is needed
                    Category.SetCheckParameter("Prior_Multi_Level_RATA_Record", null, eParameterDataType.DataRowView);//parameter set is needed
                    Category.SetCheckParameter("Invalid_Multi_Level_RATA_Record", null, eParameterDataType.DataRowView);
                    DataRowView PriorMaxLvlRec = null;//variable is needed
                    Category.SetCheckParameter("Prior_Max_Level_RATA_Record", null, eParameterDataType.DataRowView);
                    bool AnnRptReq = Category.GetCheckParameter("Annual_Reporting_Requirement").ValueAsBool();
                    string OpLvlCdList = cDBConvert.ToString(PriorRATARec["OP_LEVEL_CD_LIST"]);
                    int OpLvlCount = OpLvlCdList.ListCount();
                    int MaxLvlCount = Category.GetCheckParameter("Max_Level_Count").ValueAsInt();

                    if (OpLvlCount >= MaxLvlCount)
                    {
                        Category.SetCheckParameter("Prior_Rata_Is_Alternate_Single_Level_RATA", false, eParameterDataType.DataRowView);
                        return ReturnVal;
                    }
                    else
                        if (OpLvlCount == 2)
                    {
                        Category.SetCheckParameter("Prior_Rata_Is_Alternate_Single_Level_RATA", false, eParameterDataType.DataRowView);
                        if (MaxLvlCount != 3)
                            return ReturnVal;
                    }
                    else
                            if (AnnRptReq)
                        if (OpLvlCount == 1 && cDBConvert.ToString(PriorRATARec["TEST_CLAIM_CD"]) == "SLC")
                            Category.SetCheckParameter("Prior_Rata_Is_Alternate_Single_Level_RATA", false, eParameterDataType.DataRowView);

                    // Monitor System Ids for Current and Prior RATA
                    string CurrentMonSysId = EmParameters.QaStatusSystemId;
                    string PriorMonSysId = PriorRATARec["MON_SYS_ID"].AsString();

                    DataView QACertEventRecs = Category.GetCheckParameter("QA_Certification_Event_Records").ValueAsDataView(); ;
                    DataView QACertEventRecsFound;
                    DateTime CondBeginDate;
                    int CondBeginHour;
                    DateTime CertDate;
                    int CertHour;
                    DateTime LastCompletedDate;
                    int LastCompletedHour;
                    DateTime CurrentDate = EmParameters.CurrentDateHour.AsStartDate();
                    int CurrentHour = EmParameters.CurrentDateHour.AsStartHour();

                    //DateTime PriorMultiEndDate;
                    //int PriorMultiEndHour;
                    bool FoundAnEvent;
                    DataView RATARecs = Category.GetCheckParameter("RATA_Test_Records_By_Location_For_QA_Status").ValueAsDataView();
                    DataView RATARecsFound;
                    DateTime PriorRATAEndDate = cDBConvert.ToDate(PriorRATARec["END_DATE"], DateTypes.END);
                    int PriorRATAEndHour = cDBConvert.ToHour(PriorRATARec["END_HOUR"], DateTypes.END);
                    DataRowView SubsequentRATARec = Category.GetCheckParameter("Subsequent_RATA_Record").ValueAsDataRowView();

                    if (Category.GetCheckParameter("Prior_Rata_Is_Alternate_Single_Level_RATA").ValueAsBool() && EventLvlCount < 2)
                    {
                        PriorRATAEndDate = cDBConvert.ToDate(PriorRATARec["END_DATE"], DateTypes.END);
                        PriorRATAEndHour = cDBConvert.ToHour(PriorRATARec["END_HOUR"], DateTypes.END);

                        // Get Filtered RATA Stat Records By Location For QA Status
                        {
                            sFilterPair[] rowFilter = new sFilterPair[2];
                            rowFilter[0].Set("MON_SYS_ID", PriorMonSysId);
                            rowFilter[1].Set("TEST_RESULT_CD", "INVALID", true);
                            RATARecsFound = FindActiveRows(RATARecs,
                                                                                         DateTime.MinValue, 0, PriorRATAEndDate, PriorRATAEndHour,
                                                                                         "END_DATE", "END_HOUR", "END_DATE", "END_HOUR",
                                                                                         rowFilter);
                        }

                        if (RATARecsFound.Count > 0)
                        {
                            RATARecsFound.Sort = "END_DATE DESC, END_HOUR DESC, END_MIN DESC";
                            foreach (DataRowView drv1 in RATARecsFound)
                                if (cDBConvert.ToString(drv1["OP_LEVEL_CD_LIST"]).ListCount() >= 2 || cDBConvert.ToString(drv1["TEST_CLAIM_CD"]) == "SLC")
                                {
                                    PriorMultiLvlRec = drv1;
                                    break;
                                }

                            if (PriorMultiLvlRec != null) // RATA Test Records By Location For QA Status Found
                            {
                                // Get Filtered RATA Stat Records By Location For QA Status
                                {
                                    sFilterPair[] rowFilter = new sFilterPair[2];
                                    rowFilter[0].Set("MON_SYS_ID", PriorMonSysId);
                                    rowFilter[1].Set("TEST_RESULT_CD", "INVALID");
                                    RATARecsFound = FindActiveRows(RATARecs,
                                                                                                 cDBConvert.ToDate(PriorMultiLvlRec["END_DATE"], DateTypes.END),
                                                                                                 cDBConvert.ToHour(PriorMultiLvlRec["END_HOUR"], DateTypes.END),
                                                                                                 PriorRATAEndDate, PriorRATAEndHour, "END_DATE", "END_HOUR", "END_DATE", "END_HOUR",
                                                                                                 rowFilter);
                                }

                                if (RATARecsFound.Count > 0)
                                {
                                    RATARecsFound.Sort = "END_DATE DESC, END_HOUR DESC, END_MIN DESC";
                                    foreach (DataRowView drv2 in RATARecsFound)
                                        if (cDBConvert.ToString(drv2["OP_LEVEL_CD_LIST"]).ListCount() >= 2 || cDBConvert.ToString(drv2["TEST_CLAIM_CD"]) == "SLC")
                                        {
                                            Category.SetCheckParameter("Invalid_Multi_Level_RATA_Record", drv2, eParameterDataType.DataRowView);
                                            break;
                                        }
                                }
                            }
                        }
                        if (PriorMultiLvlRec == null)//"else" in spec
                        {
                            // Get Filtered RATA Stat Records By Location For QA Status
                            {
                                sFilterPair[] rowFilter = new sFilterPair[2];
                                rowFilter[0].Set("MON_SYS_ID", PriorMonSysId);
                                rowFilter[1].Set("TEST_RESULT_CD", "INVALID");
                                RATARecsFound = FindActiveRows(RATARecs,
                                                                                             DateTime.MinValue, 0, PriorRATAEndDate, PriorRATAEndHour,
                                                                                             "END_DATE", "END_HOUR", "END_DATE", "END_HOUR",
                                                                                             rowFilter);
                            }

                            if (RATARecsFound.Count > 0)
                            {
                                RATARecsFound.Sort = "END_DATE DESC, END_HOUR DESC, END_MIN DESC";
                                foreach (DataRowView drv3 in RATARecsFound)
                                    if (cDBConvert.ToString(drv3["OP_LEVEL_CD_LIST"]).ListCount() >= 2 || cDBConvert.ToString(drv3["TEST_CLAIM_CD"]) == "SLC")
                                    {
                                        Category.SetCheckParameter("Invalid_Multi_Level_RATA_Record", drv3, eParameterDataType.DataRowView);
                                        break;
                                    }
                            }
                        }
                        if (PriorMultiLvlRec != null)
                        {
                            Category.SetCheckParameter("Prior_Multi_Level_RATA_Record", PriorMultiLvlRec, eParameterDataType.DataRowView);

                            // Get Filtered QA Certtification
                            {
                                sFilterPair[] rowFilter = new sFilterPair[2];
                                rowFilter[0].Set("MON_SYS_ID", PriorMultiLvlRec["MON_SYS_ID"].AsString());
                                rowFilter[1].Set("RATA2_REQUIRED", "Y");
                                QACertEventRecsFound = FindRows(QACertEventRecs, rowFilter);
                            }

                            QACertEventRecsFound.Sort = "QA_CERT_EVENT_DATE DESC, QA_CERT_EVENT_HOUR DESC";

                            DateTime PriorMultiEndDate = cDBConvert.ToDate(PriorMultiLvlRec["END_DATE"], DateTypes.END); ;
                            int PriorMultiEndHour = cDBConvert.ToHour(PriorMultiLvlRec["END_HOUR"], DateTypes.END); ;
                            FoundAnEvent = false;
                            foreach (DataRowView drv4 in QACertEventRecsFound)
                            {
                                CondBeginDate = cDBConvert.ToDate(drv4["CONDITIONAL_DATA_BEGIN_DATE"], DateTypes.START);
                                CondBeginHour = cDBConvert.ToInteger(drv4["CONDITIONAL_DATA_BEGIN_HOUR"]);
                                CertDate = cDBConvert.ToDate(drv4["QA_CERT_EVENT_DATE"], DateTypes.START);
                                CertHour = cDBConvert.ToInteger(drv4["QA_CERT_EVENT_HOUR"]);
                                if ((CertDate < CurrentDate || (CertDate == CurrentDate && CertHour < CurrentHour)) || (CertDate == CurrentDate && CertHour == CurrentHour && CertDate == CondBeginDate && CertHour == CondBeginHour))
                                {
                                    LastCompletedDate = cDBConvert.ToDate(drv4["LAST_TEST_COMPLETED_DATE"], DateTypes.END);
                                    LastCompletedHour = cDBConvert.ToInteger(drv4["LAST_TEST_COMPLETED_HOUR"]);
                                    if (CertDate > PriorMultiEndDate || (CertDate == PriorMultiEndDate && CertHour >= PriorMultiEndHour) ||
                                            (CertDate == PriorMultiEndDate && CertHour == PriorMultiEndHour && (LastCompletedDate == DateTime.MaxValue || LastCompletedDate > PriorMultiEndDate || (LastCompletedDate == PriorMultiEndDate && LastCompletedHour > PriorMultiEndHour))))
                                        if (Category.GetCheckParameter("Annual_Reporting_Requirement").ValueAsBool() || CertDate >= new DateTime(CurrentDate.Year, 4, 1))
                                        {
                                            FoundAnEvent = true;
                                            break;
                                        }
                                }
                            }
                            if (FoundAnEvent)
                            {
                                Category.SetCheckParameter("Subsequent_RATA_Record", PriorMultiLvlRec, eParameterDataType.DataRowView);
                                Status = "OOC-Incomplete Recertification";
                                OverrideRATABAF.SetValue(1.0m, Category);
                            }
                            else if (cDBConvert.ToString(PriorMultiLvlRec["QA_NEEDS_EVAL_FLG"]) == "Y")
                                Status = "Prior Multi-Level RATA Not Yet Evaluated";
                            else
                            {
                                string testResultCd = PriorMultiLvlRec["TEST_RESULT_CD"].AsString();

                                if (testResultCd == null || testResultCd.InList("FAILED,ABORTED"))
                                {
                                    Status = "OOC-Incomplete QA RATA";
                                    OverrideRATABAF.SetValue(PriorRATARec["OVERALL_BIAS_ADJ_FACTOR"].AsDecimal(), Category);
                                }
                                else if (cDBConvert.ToString(PriorMultiLvlRec["OP_LEVEL_CD_LIST"]).ListCount() >= MaxLvlCount)
                                    return ReturnVal;
                            }
                        }
                        else
                        {
                            Status = "OOC-Incomplete QA RATA";
                            OverrideRATABAF.SetValue(PriorRATARec["OVERALL_BIAS_ADJ_FACTOR"].AsDecimal(), Category);
                        }
                    }

                    if ((Status == "" || Status.StartsWith("IC") || Status.StartsWith("Undetermined"))
                            && EventLvlCount < MaxLvlCount)
                    {
                        Category.SetCheckParameter("Invalid_Multi_Level_RATA_Record", null, eParameterDataType.DataRowView);
                        PriorRATAEndDate = cDBConvert.ToDate(PriorRATARec["END_DATE"], DateTypes.END);
                        PriorRATAEndHour = cDBConvert.ToHour(PriorRATARec["END_HOUR"], DateTypes.END);

                        // Get Filtered RATA Stat Records By Location For QA Status
                        {
                            sFilterPair[] rowFilter = new sFilterPair[2];
                            rowFilter[0].Set("MON_SYS_ID", CurrentMonSysId);
                            rowFilter[1].Set("TEST_RESULT_CD", "INVALID", true);
                            RATARecsFound = FindActiveRows(RATARecs,
                                            DateTime.MinValue, 0, PriorRATAEndDate, PriorRATAEndHour,
                                            "END_DATE", "END_HOUR", "END_DATE", "END_HOUR",
                                            rowFilter);
                        }

                        if (RATARecsFound.Count > 0)
                        {
                            RATARecsFound.Sort = "END_DATE DESC, END_HOUR DESC, END_MIN DESC";
                            foreach (DataRowView drv1 in RATARecsFound)
                                if (cDBConvert.ToString(drv1["OP_LEVEL_CD_LIST"]).ListCount() == MaxLvlCount)
                                {
                                    PriorMaxLvlRec = drv1;
                                    Category.SetCheckParameter("Prior_Max_Level_RATA_Record", PriorMaxLvlRec, eParameterDataType.DataRowView);
                                    break;
                                }
                            if (PriorMaxLvlRec != null)
                            {
                                // Get Filtered RATA Stat Records By Location For QA Status
                                {
                                    sFilterPair[] rowFilter = new sFilterPair[2];
                                    rowFilter[0].Set("MON_SYS_ID", CurrentMonSysId);
                                    rowFilter[1].Set("TEST_RESULT_CD", "INVALID");
                                    RATARecsFound = FindActiveRows(RATARecs,
                                                    cDBConvert.ToDate(PriorMaxLvlRec["END_DATE"], DateTypes.END),
                                                    cDBConvert.ToHour(PriorMaxLvlRec["END_HOUR"], DateTypes.END),
                                                    PriorRATAEndDate, PriorRATAEndHour,
                                                    "END_DATE", "END_HOUR", "END_DATE", "END_HOUR",
                                                    rowFilter);
                                }

                                if (RATARecsFound.Count > 0)
                                {
                                    RATARecsFound.Sort = "END_DATE DESC, END_HOUR DESC, END_MIN DESC";
                                    foreach (DataRowView drv2 in RATARecsFound)
                                        if (cDBConvert.ToString(drv2["OP_LEVEL_CD_LIST"]).ListCount() == MaxLvlCount)
                                        {
                                            Category.SetCheckParameter("Invalid_Multi_Level_RATA_Record", drv2, eParameterDataType.DataRowView);
                                            break;
                                        }
                                }
                            }
                        }
                        if (PriorMaxLvlRec == null)//"else" in spec
                        {
                            // Get Filtered RATA Stat Records By Location For QA Status
                            {
                                sFilterPair[] rowFilter = new sFilterPair[2];
                                rowFilter[0].Set("MON_SYS_ID", CurrentMonSysId);
                                rowFilter[1].Set("TEST_RESULT_CD", "INVALID");
                                RATARecsFound = FindActiveRows(RATARecs,
                                            DateTime.MinValue, 0, PriorRATAEndDate, PriorRATAEndHour,
                                            "END_DATE", "END_HOUR", "END_DATE", "END_HOUR",
                                            rowFilter);
                            }

                            if (RATARecsFound.Count > 0)
                            {
                                RATARecsFound.Sort = "END_DATE DESC, END_HOUR DESC, END_MIN DESC";
                                foreach (DataRowView drv3 in RATARecsFound)
                                    if (cDBConvert.ToString(drv3["OP_LEVEL_CD_LIST"]).ListCount() == MaxLvlCount)
                                    {
                                        Category.SetCheckParameter("Invalid_Multi_Level_RATA_Record", drv3, eParameterDataType.DataRowView);
                                        break;
                                    }
                            }
                        }
                        if (PriorMaxLvlRec == null)
                        {
                            Status = "OOC-No Prior Maximum Level RATA";
                            OverrideRATABAF.SetValue(1.0m, Category);
                        }
                        else
                        {
                            // Get Filtered QA Certtification
                            {
                                sFilterPair[] rowFilter = new sFilterPair[1];
                                rowFilter[0].Set("MON_SYS_ID", CurrentMonSysId);
                                QACertEventRecsFound = FindRows(QACertEventRecs, rowFilter);
                            }

                            QACertEventRecsFound.Sort = "QA_CERT_EVENT_DATE DESC, QA_CERT_EVENT_HOUR DESC";

                            DateTime PriorMaxEndDate = cDBConvert.ToDate(PriorMaxLvlRec["END_DATE"], DateTypes.END);
                            int PriorMaxEndHour = cDBConvert.ToHour(PriorMaxLvlRec["END_HOUR"], DateTypes.END);
                            FoundAnEvent = false;
                            foreach (DataRowView drv4 in QACertEventRecsFound)
                            {
                                CondBeginDate = cDBConvert.ToDate(drv4["CONDITIONAL_DATA_BEGIN_DATE"], DateTypes.START);
                                CondBeginHour = cDBConvert.ToInteger(drv4["CONDITIONAL_DATA_BEGIN_HOUR"]);
                                CertDate = cDBConvert.ToDate(drv4["QA_CERT_EVENT_DATE"], DateTypes.START);
                                CertHour = cDBConvert.ToInteger(drv4["QA_CERT_EVENT_HOUR"]);
                                if ((CertDate < CurrentDate || (CertDate == CurrentDate && CertHour < CurrentHour)) || (CertDate == CurrentDate && CertHour == CurrentHour && CertDate == CondBeginDate && CertHour == CondBeginHour))
                                {
                                    LastCompletedDate = cDBConvert.ToDate(drv4["LAST_TEST_COMPLETED_DATE"], DateTypes.END);
                                    LastCompletedHour = cDBConvert.ToInteger(drv4["LAST_TEST_COMPLETED_HOUR"]);
                                    if (CertDate > PriorMaxEndDate || (CertDate == PriorMaxEndDate && CertHour > PriorMaxEndHour) ||
                                            (CertDate == PriorMaxEndDate && CertHour == PriorMaxEndHour && (LastCompletedDate == DateTime.MaxValue || LastCompletedDate > PriorMaxEndDate || (LastCompletedDate == PriorMaxEndDate && LastCompletedHour > PriorMaxEndHour))))
                                        if ((MaxLvlCount == 2 && (cDBConvert.ToString(drv4["RATA2_REQUIRED"]) == "Y" || cDBConvert.ToString(drv4["RATA3_REQUIRED"]) == "Y")) ||
                                                (MaxLvlCount == 3 && cDBConvert.ToString(drv4["RATA3_REQUIRED"]) == "Y"))
                                        {
                                            FoundAnEvent = true;
                                            break;
                                        }
                                }
                            }
                            if (FoundAnEvent)
                            {
                                Category.SetCheckParameter("Subsequent_RATA_Record", PriorMaxLvlRec, eParameterDataType.DataRowView);
                                Status = "OOC-Incomplete Recertification";
                                OverrideRATABAF.SetValue(1.0m, Category);
                            }
                            else if (cDBConvert.ToString(PriorMaxLvlRec["QA_NEEDS_EVAL_FLG"]) == "Y")
                                Status = "Prior Maximum Level RATA Not Yet Evaluated";
                            else
                            {
                                string PriorMaxLvlRecResCd = cDBConvert.ToString(PriorMaxLvlRec["TEST_RESULT_CD"]);
                                if (PriorMaxLvlRecResCd == "")
                                {
                                    Status = "OOC-Prior Maximum Level RATA Has Critical Errors";
                                    OverrideRATABAF.SetValue(PriorRATARec["OVERALL_BIAS_ADJ_FACTOR"].AsDecimal(), Category);
                                }
                                else if (PriorMaxLvlRecResCd == "FAILED")
                                {
                                    Status = "OOC-Prior Maximum Level RATA Failed";
                                    OverrideRATABAF.SetValue(PriorRATARec["OVERALL_BIAS_ADJ_FACTOR"].AsDecimal(), Category);
                                }
                                else if (PriorMaxLvlRecResCd == "ABORTED")
                                {
                                    Status = "OOC-Prior Maximum Level RATA Aborted";
                                    OverrideRATABAF.SetValue(PriorRATARec["OVERALL_BIAS_ADJ_FACTOR"].AsDecimal(), Category);
                                }
                                else
                                {
                                    DateTime TestExpDate;
                                    {
                                        DateTime effectiveCertificationDate;

                                        if ((PriorMaxLvlRec["TEST_REASON_CD"].AsString() == "INITIAL"))
                                        {
                                            DateTime currentHour = CurrentHourlyRecordforRataStatus.Value["BEGIN_DATE"].AsDateTime(DateTime.MinValue).AddHours(CurrentHourlyRecordforRataStatus.Value["BEGIN_HOUR"].AsInteger(0));

                                            DataRowView initialQaCertificationEventRow = cRowFilter.FindMostRecentRow(
                                                                QaCertificationEventRecords.Value,
                                                                currentHour.AddHours(-1).Date,
                                                                currentHour.AddHours(-1).Hour,
                                                                "QA_CERT_EVENT_DATE", "QA_CERT_EVENT_HOUR",
                                                                new cFilterCondition[]
                                                                {
                                                                new cFilterCondition("MON_SYS_ID", CurrentMonSysId, eFilterConditionStringCompare.InList),
                                                                new cFilterCondition("QA_CERT_EVENT_CD", "305", eFilterConditionStringCompare.InList)
                                                                }
                                                                                                                    );

                                            if ((initialQaCertificationEventRow != null) &&
                                                    (initialQaCertificationEventRow["LAST_TEST_COMPLETED_DATE"].AsDateTime(DateTime.MinValue) > PriorMaxLvlRec["END_DATE"].AsDateTime(DateTime.MaxValue)))
                                            {
                                                effectiveCertificationDate = initialQaCertificationEventRow["LAST_TEST_COMPLETED_DATE"].AsDateTime(DateTime.MinValue);
                                            }
                                            else
                                            {
                                                effectiveCertificationDate = PriorMaxLvlRec["END_DATE"].AsDateTime(DateTime.MaxValue);
                                            }
                                        }
                                        else
                                        {
                                            effectiveCertificationDate = PriorMaxLvlRec["END_DATE"].AsDateTime(DateTime.MaxValue);
                                        }

                                        // Add 20 quarters to the later of the RATA End Date and the Last Test Completed Date.
                                        TestExpDate = cDateFunctions.LastDateThisQuarter(effectiveCertificationDate.AddYears(5));
                                    }

                                    PriorMaxLvlRec["TEST_EXP_DATE"] = TestExpDate;

                                    if (CurrentDate > TestExpDate)
                                        if (!AnnRptReq)
                                        {
                                            Status = "OOC-Prior Maximum Level RATA Expired";
                                            OverrideRATABAF.SetValue(PriorRATARec["OVERALL_BIAS_ADJ_FACTOR"].AsDecimal(), Category);
                                        }
                                        else
                                        {
                                            int[] OpHrsAccumArray = Category.GetCheckParameter("Rpt_Period_Op_Hours_Accumulator_Array").ValueAsIntArray();
                                            int CurrentPosition = Category.GetCheckParameter("Current_Monitor_Plan_Location_Postion").ValueAsInt();
                                            int GraceOpHrs = OpHrsAccumArray[CurrentPosition];
                                            if (GraceOpHrs < 0)
                                                Status = "Invalid Op Data";
                                            else if (GraceOpHrs > 720)
                                            {
                                                Status = "OOC-Prior Maximum Level RATA Expired";
                                                OverrideRATABAF.SetValue(PriorRATARec["OVERALL_BIAS_ADJ_FACTOR"].AsDecimal(), Category);
                                            }
                                            else
                                            {
                                                int FirstQtr = cDateFunctions.ThisQuarter(TestExpDate.AddMonths(3));
                                                int FirstYear = TestExpDate.AddMonths(3).Year;
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
                                                DateTime EarliestLocRptDate = Category.GetCheckParameter("Earliest_Location_Report_Date").ValueAsDateTime(DateTypes.START);
                                                sFilterPair[] OpSuppFilter = new sFilterPair[4];
                                                OpSuppFilter[0].Set("OP_TYPE_CD", "OPHOURS");
                                                OpSuppFilter[1].Set("FUEL_CD", DBNull.Value, eFilterDataType.String);
                                                bool Quit = false;
                                                int j;
                                                for (int i = FirstYear; i <= YearPriorToCurrentQtr; i++)
                                                {
                                                    j = 1;
                                                    if (i == FirstYear)
                                                        j = FirstQtr;
                                                    while (j <= 4 && !Quit && !(i == YearPriorToCurrentQtr && j > QtrPriorToCurrentQtr))
                                                    {
                                                        if (EarliestLocRptDate <= cDateFunctions.LastDateThisQuarter(i, j))
                                                        {
                                                            OpSuppFilter[2].Set("CALENDAR_YEAR", i, eFilterDataType.Integer);
                                                            OpSuppFilter[3].Set("QUARTER", j, eFilterDataType.Integer);
                                                            OpSuppDataRecsFound = FindRows(OpSuppDataRecs, OpSuppFilter);
                                                            if (OpSuppDataRecsFound.Count > 0)
                                                            {
                                                                GraceOpHrs += cDBConvert.ToInteger(OpSuppDataRecsFound[0]["OP_VALUE"]);
                                                                if (GraceOpHrs > 720)
                                                                {
                                                                    Status = "OOC-Prior Maximum Level RATA Expired";
                                                                    OverrideRATABAF.SetValue(PriorRATARec["OVERALL_BIAS_ADJ_FACTOR"].AsDecimal(), Category);

                                                                    Quit = true;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                DataView ReportingFrequencyByLocationQuarterRecs = Category.GetCheckParameter("Reporting_Frequency_by_Location_Quarter").ValueAsDataView();
                                                                ReportingFrequencyByLocationQuarterRecs.RowFilter = string.Format("Calendar_Year={0} and Quarter={1}", i, j);
                                                                sFilterPair[] quarterlyFilter = new sFilterPair[1];
                                                                quarterlyFilter[0].Set("Report_Freq_Cd", "Q");
                                                                DataView quarterlyFreqency = FindRows(ReportingFrequencyByLocationQuarterRecs, quarterlyFilter);
                                                                if ((ReportingFrequencyByLocationQuarterRecs.Count > 0)
                                                                    && (j.InRange(2, 3) || (quarterlyFreqency.Count > 0)))
                                                                {
                                                                    Category.SetCheckParameter("RATA_Missing_Op_Data_Info", i + "Q" + j, eParameterDataType.String);
                                                                    Status = "Missing Op Data";
                                                                    Quit = true;
                                                                }
                                                            }
                                                        }
                                                        j++;
                                                    }
                                                    if (Quit)
                                                        break;
                                                }
                                            }
                                        }
                                }
                            }
                        }
                    }
                }
                if (Status != "")
                    Category.SetCheckParameter("Current_RATA_Status", Status, eParameterDataType.String);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATSTAT6");
            }

            return ReturnVal;
        }

        public string RATSTAT7(cCategory Category, ref bool Log)
        //Determine Expiration Dates for Most Recent Prior RATA Test    
        {
            string ReturnVal = "";

            try
            {
                string Status = Category.GetCheckParameter("Current_RATA_Status").ValueAsString();
                DataRowView PriorRataRec = Category.GetCheckParameter("Prior_RATA_Record").ValueAsDataRowView();
                DataRowView PriorRataEventRec = Category.GetCheckParameter("Prior_RATA_Event_Record").ValueAsDataRowView();
                DataRowView PriorMultiLvlRATARec = Category.GetCheckParameter("Prior_Multi_Level_RATA_Record").ValueAsDataRowView();

                if (Status == "" && PriorRataRec != null && PriorRataEventRec == null)
                {
                    bool PriorRATAIsAltSingle = Category.GetCheckParameter("Prior_Rata_Is_Alternate_Single_Level_RATA").ValueAsBool();
                    DateTime PriorTestExpDate = cDBConvert.ToDate(PriorRataRec["TEST_EXP_DATE"], DateTypes.START);
                    DateTime PriorTestExpDateWExt = cDBConvert.ToDate(PriorRataRec["TEST_EXP_DATE_WITH_EXT"], DateTypes.START);
                    bool MissingOpData = false;
                    int NumExtQtrs = 0;
                    bool PriorTestIgnoreGraceForExtensions;

                    bool AnnRptReq = Category.GetCheckParameter("Annual_Reporting_Requirement").ValueAsBool();
                    DateTime PriorTestEndDate = cDBConvert.ToDate(PriorRataRec["END_DATE"], DateTypes.END);
                    int PriorTestEndHour = cDBConvert.ToHour(PriorRataRec["END_HOUR"], DateTypes.END);
                    DateTime PriorTestBegDate = cDBConvert.ToDate(PriorRataRec["BEGIN_DATE"], DateTypes.START);
                    int PriorTestBegHour = cDBConvert.ToHour(PriorRataRec["BEGIN_HOUR"], DateTypes.START);
                    DateTime CurrentDate = EmParameters.CurrentDateHour.AsStartDate();
                    int CurrentHour = EmParameters.CurrentDateHour.AsStartHour();

                    DataView QACertEventRecs = Category.GetCheckParameter("QA_Certification_Event_Records").ValueAsDataView();
                    DataView QACertEventRecsFound;
                    DataRowView CertRec;
                    DateTime CertRecLastTestComplDate;
                    sFilterPair[] FilterCertRecs;

                    DateTime EarliestLocRptDate = Category.GetCheckParameter("Earliest_Location_Report_Date").ValueAsDateTime(DateTypes.START);

                    DataView TEERecs = Category.GetCheckParameter("Test_Extension_Exemption_Records").ValueAsDataView();
                    DataView TEERecsFound;
                    sFilterPair[] TEEFilter = new sFilterPair[3];
                    DataView ReportingFrequencyByLocationQuarterRecs = Category.GetCheckParameter("Reporting_Frequency_by_Location_Quarter").ValueAsDataView();

                    if (EmParameters.PriorRataRecord.IgnoreGraceForExtensions == 1)
                    {
                        PriorTestIgnoreGraceForExtensions = true;
                    }
                    else
                    {
                        PriorTestIgnoreGraceForExtensions = false;
                    }

                    if (PriorTestExpDate == DateTime.MinValue)
                    {
                        if (!AnnRptReq)
                        {
                            if (new DateTime(2007, 10, 1) <= PriorTestEndDate && PriorTestEndDate <= new DateTime(2007, 12, 31))
                                PriorTestExpDate = new DateTime(2008, 9, 30);
                            else
                                PriorTestExpDate = new DateTime(PriorTestEndDate.Year, 9, 30);
                        }
                        else
                            if (EmParameters.QaStatusSystemDesignationCode == "B")
                        {
                            TEEFilter[0].Set("MON_SYS_ID", EmParameters.QaStatusSystemId);
                            TEEFilter[1].Set("RPT_PERIOD_ID", EmParameters.CurrentReportingPeriod, eFilterDataType.Integer);
                            TEEFilter[2].Set("EXTENS_EXEMPT_CD", "NRB720");
                            TEERecsFound = FindRows(TEERecs, TEEFilter);
                            if (TEERecsFound.Count > 0)
                            {
                                PriorTestExpDate = cDateFunctions.LastDateThisQuarter(PriorTestEndDate.AddYears(2));
                            }
                        }
                        if (PriorTestExpDate == DateTime.MinValue)
                        {
                            if (PriorRATAIsAltSingle)
                            {
                                PriorTestExpDate = cDateFunctions.LastDateThisQuarter(cDBConvert.ToDate(PriorMultiLvlRATARec["END_DATE"], DateTypes.END).AddYears(1));
                                if (cDBConvert.ToInteger(PriorMultiLvlRATARec["GP_IND"]) == 1)
                                    PriorTestExpDate = cDateFunctions.LastDateThisQuarter(PriorTestExpDate.AddMonths(-3));
                            }
                            else
                            {
                                {
                                    /*
									 * Locate most resent QA Certification Event for the current system with:
									 * 
									 * 1) RATA Required equal to "Y", and
									 * 2) BeginDateHour prior toe PriorRataRecord.BeginDateHour
									 * 
									 */

                                    FilterCertRecs = new sFilterPair[2];
                                    FilterCertRecs[0].Set("MON_SYS_ID", cDBConvert.ToString(PriorRataRec["MON_SYS_ID"]));
                                    FilterCertRecs[1].Set("RATA_REQUIRED", "Y");

                                    QACertEventRecsFound = FindActiveRows(QACertEventRecs, DateTime.MinValue, 0, PriorTestBegDate, PriorTestBegHour,
                                                                          "QA_CERT_EVENT_DATE", "QA_CERT_EVENT_HOUR", "QA_CERT_EVENT_DATE", "QA_CERT_EVENT_HOUR", FilterCertRecs);

                                    if (QACertEventRecsFound.Count > 0)
                                    {
                                        QACertEventRecsFound.Sort = "QA_CERT_EVENT_DATE DESC, QA_CERT_EVENT_HOUR DESC";
                                        CertRec = QACertEventRecsFound[0];
                                        CertRecLastTestComplDate = cDBConvert.ToDate(CertRec["LAST_TEST_COMPLETED_DATE"], DateTypes.START);
                                    }
                                    else
                                    {
                                        CertRec = null;
                                        CertRecLastTestComplDate = DateTime.MaxValue;
                                    }
                                    Category.SetCheckParameter("Prior_Rata_Event_Record", CertRec, eParameterDataType.DataRowView);
                                }

                                //4QTRS,8QTRS
                                if (cDBConvert.ToString(PriorRataRec["RATA_FREQUENCY_CD"]).InList("4QTRS,8QTRS"))
                                {

                                    if ((CertRec != null) &&
                                        (cDBConvert.ToString(CertRec["RATA_CERT_EVENT"]) == "Y") &&
                                        (CertRec["CONDITIONAL_DATA_BEGIN_DATE"] == DBNull.Value) &&
                                        ((CertRecLastTestComplDate > PriorTestEndDate) ||
                                         ((CertRecLastTestComplDate == PriorTestEndDate) &&
                                          (cDBConvert.ToHour(CertRec["LAST_TEST_COMPLETED_HOUR"], DateTypes.START) > PriorTestEndHour))))
                                    {
                                        if (EmParameters.PriorRataRecord.SysTypeCd.InList("HCL,HF,HG,ST"))
                                        {
                                            //find Latest MATS Program Record
                                            cFilterCondition[] ProgramFilter = new cFilterCondition[] { new cFilterCondition("PRG_CD", "MATS") };
                                            DataRowView LatestMATSProgramRec = cRowFilter.FindMostRecentRow(EmParameters.LocationProgramRecordsByHourLocation.SourceView,
                                                  EmParameters.PriorRataEventRecord.SysBeginDate.GetValueOrDefault(),
                                                  "EMISSIONS_RECORDING_BEGIN_DATE",
                                                  ProgramFilter, eFilterConditionRelativeCompare.GreaterThanOrEqual);

                                            if (LatestMATSProgramRec != null && LatestMATSProgramRec["EMISSIONS_RECORDING_BEGIN_DATE"].AsDateTime() > CertRecLastTestComplDate)
                                            {
                                                PriorTestExpDate = cDateFunctions.LastDateThisQuarter(((DateTime)LatestMATSProgramRec["EMISSIONS_RECORDING_BEGIN_DATE"]).AddYears(1));
                                            }
                                            else
                                            {
                                                PriorTestExpDate = cDateFunctions.LastDateThisQuarter(CertRecLastTestComplDate.AddYears(1));
                                            }

                                            if (EmParameters.PriorRataRecord.GpInd == 1)
                                            {
                                                PriorTestIgnoreGraceForExtensions = true;
                                            }
                                        }
                                        else //not MATS
                                        {
                                            PriorTestExpDate = cDateFunctions.LastDateThisQuarter(CertRecLastTestComplDate.AddYears(1));

                                            if (EmParameters.PriorRataRecord.GpInd == 1)
                                            {
                                                PriorTestExpDate = cDateFunctions.LastDateThisQuarter(PriorTestExpDate.AddMonths(-3));
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (EmParameters.PriorRataRecord.SysTypeCd.InList("HCL,HF,HG,ST"))
                                        {
                                            //find Latest MATS Program Record
                                            cFilterCondition[] ProgramFilter = new cFilterCondition[] { new cFilterCondition("PRG_CD", "MATS") };
                                            DataRowView LatestMATSProgramRec = null;
                                            //prevent explosions if no cert event record
                                            if (EmParameters.PriorRataEventRecord != null)
                                            {
                                                LatestMATSProgramRec = cRowFilter.FindMostRecentRow(EmParameters.LocationProgramRecordsByHourLocation.SourceView,
                                                EmParameters.PriorRataEventRecord.SysBeginDate.GetValueOrDefault(),
                                                "EMISSIONS_RECORDING_BEGIN_DATE",
                                                ProgramFilter, eFilterConditionRelativeCompare.GreaterThanOrEqual);
                                            }

                                            if (LatestMATSProgramRec != null && LatestMATSProgramRec["EMISSIONS_RECORDING_BEGIN_DATE"].AsDateTime() > PriorTestEndDate)
                                            {
                                                PriorTestExpDate = cDateFunctions.LastDateThisQuarter(((DateTime)LatestMATSProgramRec["EMISSIONS_RECORDING_BEGIN_DATE"]).AddYears(1));
                                            }
                                            else
                                            {
                                                PriorTestExpDate = cDateFunctions.LastDateThisQuarter(PriorTestEndDate.AddYears(1));
                                            }
                                        }
                                        else //not MATS
                                        {
                                            PriorTestExpDate = cDateFunctions.LastDateThisQuarter(PriorTestEndDate.AddYears(1));
                                        }

                                        if (EmParameters.PriorRataRecord.GpInd == 1)
                                        {
                                            PriorTestExpDate = cDateFunctions.LastDateThisQuarter(PriorTestExpDate.AddMonths(-3));
                                        }
                                    }
                                }
                                else // not 4QTRS,8QTRS
                                {
                                    if (CertRec != null)
                                    {
                                        if (cDBConvert.ToString(CertRec["RATA_CERT_EVENT"]) == "Y" && CertRec["CONDITIONAL_DATA_BEGIN_DATE"] == DBNull.Value &&
                                            (CertRecLastTestComplDate > PriorTestEndDate || (CertRecLastTestComplDate == PriorTestEndDate &&
                                            cDBConvert.ToHour(CertRec["LAST_TEST_COMPLETED_HOUR"], DateTypes.START) > PriorTestEndHour)))
                                            PriorTestExpDate = cDateFunctions.LastDateThisQuarter(CertRecLastTestComplDate.AddMonths(6));
                                        else
                                            PriorTestExpDate = cDateFunctions.LastDateThisQuarter(PriorTestEndDate.AddMonths(6));
                                    }
                                    else
                                        PriorTestExpDate = cDateFunctions.LastDateThisQuarter(PriorTestEndDate.AddMonths(6));
                                }
                            }
                        }
                        //Save final expDate back to the parameter
                        PriorRataRec["TEST_EXP_DATE"] = PriorTestExpDate;

                        if (PriorTestIgnoreGraceForExtensions == true)
                        {
                            EmParameters.PriorRataRecord.IgnoreGraceForExtensions = 1;
                        }
                        else
                        {
                            EmParameters.PriorRataRecord.IgnoreGraceForExtensions = 0;
                        }
                    }
                    if (CurrentDate <= PriorTestExpDate)
                        Status = "IC";
                    else
                        if (!AnnRptReq)
                    {
                        Status = "OOC-Expired";

                        if (EmParameters.CurrentMhvParameter == "FLOW")
                            OverrideRATABAF.SetValue(PriorRataRec["OVERALL_BIAS_ADJ_FACTOR"].AsDecimal(), Category);
                    }
                    else
                    {
                        //setting up
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

                        //logic start
                        if (PriorTestExpDateWExt == DateTime.MinValue)
                        {
                            int StartQtr;
                            int StartYear;
                            int EndQtr;
                            int EndYear;
                            if (EmParameters.CurrentMhvParameter == "FLOW" && PriorRATAIsAltSingle)
                            {
                                StartQtr = cDateFunctions.ThisQuarter(cDBConvert.ToDate(PriorMultiLvlRATARec["END_DATE"], DateTypes.END));
                                StartYear = cDBConvert.ToDate(PriorMultiLvlRATARec["END_DATE"], DateTypes.END).Year;
                                EndQtr = StartQtr;
                                EndYear = StartYear + 2;

                                if (cDBConvert.ToInteger(PriorMultiLvlRATARec["GP_IND"]) != 1) //reversed from spec
                                    if (StartQtr != 4)
                                        StartQtr++;
                                    else //GP_Ind == 1
                                    {
                                        StartQtr = 1;
                                        StartYear++;
                                    }
                            }
                            else // not (Flow and PriorRATAIsAltSingle)
                            {
                                StartQtr = cDateFunctions.ThisQuarter(PriorTestEndDate);
                                StartYear = PriorTestEndDate.Year;
                                EndQtr = StartQtr;
                                EndYear = StartYear + 2;
                                if (cDBConvert.ToInteger(PriorRataRec["GP_IND"]) == 1 && PriorTestIgnoreGraceForExtensions == false)
                                {
                                    StartQtr = cDateFunctions.ThisQuarter(PriorTestEndDate);
                                }
                                else
                                {
                                    if (StartQtr != 4)
                                        StartQtr++;
                                    else
                                    {
                                        StartQtr = 1;
                                        StartYear++;
                                    }
                                }
                            }

                            DateTime MaximumExtensionDate = cDateFunctions.LastDateThisQuarter(EndYear, EndQtr);

                            //Loop requires that we set EndQtr/Year to earlier of the EndQtr/Year and the Qtr/Year prior to current
                            if (EndYear != YearPriorToCurrentQtr)
                            {
                                if (EndYear > YearPriorToCurrentQtr)
                                {
                                    EndYear = YearPriorToCurrentQtr;
                                    EndQtr = QtrPriorToCurrentQtr;
                                }
                            }
                            else
                                if (EndQtr > QtrPriorToCurrentQtr)
                                EndQtr = QtrPriorToCurrentQtr;

                            DateTime StartNonQaPrimaryBypassQuarterDate = cDateFunctions.StartDateThisQuarter(StartYear, StartQtr);
                            int j;
                            bool Quit = false;
                            TEEFilter = new sFilterPair[4];
                            TEEFilter[0].Set("MON_SYS_ID", EmParameters.QaStatusSystemId);
                            OpSuppFilter = new sFilterPair[3];
                            OpSuppFilter[0].Set("OP_TYPE_CD", "OPHOURS");

                            int? operatingHoursCount;

                            //For each quarter
                            for (int i = StartYear; i <= EndYear; i++)
                            {
                                j = 1;
                                if (i == StartYear)
                                    j = StartQtr;
                                while (j <= 4 && !(i == EndYear && j > EndQtr))
                                {
                                    // Prevent extensions beyond the maximum expiration date
                                    if (PriorTestExpDate.AddMonths(3 * (NumExtQtrs + 1)) > MaximumExtensionDate)
                                    {
                                        Quit = true;
                                        break;
                                    }

                                    if (EarliestLocRptDate > cDateFunctions.LastDateThisQuarter(i, j))
                                    {
                                        NumExtQtrs++;
                                        StartNonQaPrimaryBypassQuarterDate = cDateFunctions.StartDateThisQuarter(i, j).AddMonths(3);
                                    }
                                    else
                                    {
                                        /* Selects operating hour counts based on system if Primary/Primary-Bypass systems (stacks) are invovled. */
                                        if ((EmParameters.PrimaryBypassActiveForHour == true) && EmParameters.QaStatusSystemTypeCode == "NOX")
                                        {
                                            CheckDataView<SystemOpSuppData> systemOpSuppData
                                                = EmParameters.SystemOperatingSuppDataRecordsByLocation.FindRows(new cFilterCondition("MON_SYS_ID", EmParameters.QaStatusSystemId),
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
                                        {
                                            NumExtQtrs++;
                                            StartNonQaPrimaryBypassQuarterDate = cDateFunctions.StartDateThisQuarter(i, j).AddMonths(3);
                                        }

                                        else
                                            if (EmParameters.QaStatusSystemTypeCode.StartsWith("SO2"))
                                        {
                                            TEEFilter[1].Set("CALENDAR_YEAR", i, eFilterDataType.Integer);
                                            TEEFilter[2].Set("QUARTER", j, eFilterDataType.Integer);
                                            TEEFilter[3].Set("EXTENS_EXEMPT_CD", "LOWSQTR");
                                            TEERecsFound = FindRows(TEERecs, TEEFilter);
                                            if (TEERecsFound.Count > 0)
                                            {
                                                NumExtQtrs++;
                                                StartNonQaPrimaryBypassQuarterDate = cDateFunctions.StartDateThisQuarter(i, j).AddMonths(3);
                                            }
                                            else if (operatingHoursCount == null)
                                            {
                                                ReportingFrequencyByLocationQuarterRecs.RowFilter = string.Format("Calendar_Year={0} and Quarter={1}", i, j);
                                                sFilterPair[] quarterlyFilter = new sFilterPair[1];
                                                quarterlyFilter[0].Set("Report_Freq_Cd", "Q");
                                                DataView quarterlyFreqency = FindRows(ReportingFrequencyByLocationQuarterRecs, quarterlyFilter);
                                                if ((ReportingFrequencyByLocationQuarterRecs.Count > 0)
                                                    && (j.InRange(2, 3) || (quarterlyFreqency.Count > 0)))
                                                {
                                                    MissingOpData = true;
                                                    Category.SetCheckParameter("RATA_Missing_Op_Data_Info", i + "Q" + j, eParameterDataType.String);
                                                    //Quit = true;
                                                }
                                            }
                                            //else
                                            //Quit = true;
                                        }
                                        else if (EmParameters.QaStatusSystemDesignationCode == "PB")
                                        {
                                            TEEFilter[1].Set("CALENDAR_YEAR", i, eFilterDataType.Integer);
                                            TEEFilter[2].Set("QUARTER", j, eFilterDataType.Integer);
                                            TEEFilter[3].Set("EXTENS_EXEMPT_CD", "NONQAPB,GRACEPB", eFilterPairStringCompare.InList);
                                            TEERecsFound = FindRows(TEERecs, TEEFilter);

                                            if (TEERecsFound.Count > 0)
                                            {
                                                NumExtQtrs++;
                                                StartNonQaPrimaryBypassQuarterDate = cDateFunctions.StartDateThisQuarter(i, j).AddMonths(3);
                                            }
                                            else if (operatingHoursCount == null)
                                            {
                                                ReportingFrequencyByLocationQuarterRecs.RowFilter = string.Format("Calendar_Year={0} and Quarter={1}", i, j);
                                                sFilterPair[] quarterlyFilter = new sFilterPair[1];
                                                quarterlyFilter[0].Set("Report_Freq_Cd", "Q");
                                                DataView quarterlyFreqency = FindRows(ReportingFrequencyByLocationQuarterRecs, quarterlyFilter);
                                                if ((ReportingFrequencyByLocationQuarterRecs.Count > 0)
                                                    && (j.InRange(2, 3) || (quarterlyFreqency.Count > 0)))
                                                {
                                                    MissingOpData = true;
                                                    Category.SetCheckParameter("RATA_Missing_Op_Data_Info", i + "Q" + j, eParameterDataType.String);
                                                    //Quit = true;
                                                }
                                            }
                                            //else
                                            //Quit = true;
                                        }
                                        else if (operatingHoursCount == null)
                                        {
                                            ReportingFrequencyByLocationQuarterRecs.RowFilter = string.Format("Calendar_Year={0} and Quarter={1}", i, j);
                                            sFilterPair[] quarterlyFilter = new sFilterPair[1];
                                            quarterlyFilter[0].Set("Report_Freq_Cd", "Q");
                                            DataView quarterlyFreqency = FindRows(ReportingFrequencyByLocationQuarterRecs, quarterlyFilter);
                                            if ((ReportingFrequencyByLocationQuarterRecs.Count > 0)
                                                && (j.InRange(2, 3) || (quarterlyFreqency.Count > 0)))
                                            {
                                                MissingOpData = true;
                                                Category.SetCheckParameter("RATA_Missing_Op_Data_Info", i + "Q" + j, eParameterDataType.String);
                                                //Quit = true;
                                            }
                                        }
                                    }
                                    j++;
                                }
                                if (Quit)
                                    break;
                            }

                            //
                            DateTime newStart = StartNonQaPrimaryBypassQuarterDate;
                            DateTime newEnd = cDateFunctions.LastDatePriorQuarter(CurrentDate);

                            for (DateTime date = newStart; date <= newEnd; date = date.AddMonths(3))
                            {
                                int quarter = cDateFunctions.ThisQuarter(date);

                                if (EmParameters.QaStatusSystemDesignationCode == "PB")
                                {
                                    // Allow additional extensions for non QA Primary Bypass exemptions.

                                    TEEFilter[1].Set("CALENDAR_YEAR", date.Year, eFilterDataType.Integer);
                                    TEEFilter[2].Set("QUARTER", quarter, eFilterDataType.Integer);
                                    TEEFilter[3].Set("EXTENS_EXEMPT_CD", "NONQAPB,GRACEPB", eFilterPairStringCompare.InList);
                                    TEERecsFound = FindRows(TEERecs, TEEFilter);
                                    if (TEERecsFound.Count > 0)
                                        NumExtQtrs++;
                                    else
                                        break;
                                }
                            }


                            PriorTestExpDateWExt = PriorTestExpDate;
                            PriorTestExpDateWExt = cDateFunctions.LastDateThisQuarter(PriorTestExpDateWExt.AddMonths(3 * NumExtQtrs));
                            PriorRataRec["TEST_EXP_DATE_WITH_EXT"] = PriorTestExpDateWExt;

                        }
                        if (CurrentDate <= PriorTestExpDateWExt)
                            Status = "IC-Extension";
                        else if (MissingOpData)
                        {
                            Status = "Missing Op Data";
                            PriorRataRec["TEST_EXP_DATE_WITH_EXT"] = DBNull.Value;
                        }
                        else
                        {
                            int currentOpHours;

                            /* Selects operating hour counts based on system if Primary/Primary-Bypass systems (stacks) are invovled. */
                            if ((EmParameters.PrimaryBypassActiveForHour == true) && EmParameters.QaStatusSystemTypeCode == "NOX")
                            {
                                currentOpHours = EmParameters.SystemOperatingSuppDataDictionaryArray[Category.CurrentMonLocPos][EmParameters.QaStatusSystemId].QuarterlyOperatingCounts.Hours;
                            }
                            else
                            {
                                int[] OpHrsAccumArray = Category.GetCheckParameter("Rpt_Period_Op_Hours_Accumulator_Array").ValueAsIntArray();
                                int CurrentPosition = Category.GetCheckParameter("Current_Monitor_Plan_Location_Postion").ValueAsInt();

                                currentOpHours = OpHrsAccumArray[CurrentPosition];
                            }

                            if (currentOpHours == -1)
                                Status = "Invalid Op Data";
                            else
                            {
                                int GraceOpHrs = currentOpHours;
                                if (GraceOpHrs > 720)
                                {
                                    Status = "OOC-Expired";

                                    if (EmParameters.CurrentMhvParameter == "FLOW")
                                        OverrideRATABAF.SetValue(PriorRataRec["OVERALL_BIAS_ADJ_FACTOR"].AsDecimal(), Category);
                                }
                                else
                                {
                                    int FirstQtr = cDateFunctions.ThisQuarter(PriorTestExpDateWExt.AddMonths(3));
                                    int FirstYear = PriorTestExpDateWExt.AddMonths(3).Year;
                                    int EarlyLocRptDateQtr = cDateFunctions.ThisQuarter(EarliestLocRptDate);

                                    if ((PriorTestExpDateWExt >= EarliestLocRptDate && ((FirstYear > YearPriorToCurrentQtr || (FirstYear == YearPriorToCurrentQtr && FirstQtr > QtrPriorToCurrentQtr)))) ||
                                            (PriorTestExpDateWExt < EarliestLocRptDate && ((EarliestLocRptDate.Year > YearPriorToCurrentQtr || (EarliestLocRptDate.Year == YearPriorToCurrentQtr && EarlyLocRptDateQtr > QtrPriorToCurrentQtr)))))
                                        Status = "IC-Grace";
                                    else
                                    {
                                        bool Quit = false;

                                        int? operatingHoursCount;

                                        int j;
                                        OpSuppFilter = new sFilterPair[4];
                                        OpSuppFilter[0].Set("OP_TYPE_CD", "OPHOURS");
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
                                                    if ((EmParameters.PrimaryBypassActiveForHour == true) && EmParameters.QaStatusSystemTypeCode == "NOX")
                                                    {
                                                        CheckDataView<SystemOpSuppData> systemOpSuppData
                                                            = EmParameters.SystemOperatingSuppDataRecordsByLocation.FindRows(new cFilterCondition("MON_SYS_ID", EmParameters.QaStatusSystemId),
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
                                                        OpSuppFilter[1].Set("FUEL_CD", DBNull.Value, eFilterDataType.String);
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

                                                        if (GraceOpHrs > 720)
                                                        {
                                                            Status = "OOC-Expired";

                                                            if (EmParameters.CurrentMhvParameter == "FLOW")
                                                                OverrideRATABAF.SetValue(PriorRataRec["OVERALL_BIAS_ADJ_FACTOR"].AsDecimal(), Category);

                                                            Quit = true;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        ReportingFrequencyByLocationQuarterRecs.RowFilter = string.Format("Calendar_Year={0} and Quarter={1}", i, j);
                                                        sFilterPair[] quarterlyFilter = new sFilterPair[1];
                                                        quarterlyFilter[0].Set("Report_Freq_Cd", "Q");
                                                        DataView quarterlyFreqency = FindRows(ReportingFrequencyByLocationQuarterRecs, quarterlyFilter);
                                                        if ((ReportingFrequencyByLocationQuarterRecs.Count > 0)
                                                            && (j.InRange(2, 3) || (quarterlyFreqency.Count > 0)))
                                                        {
                                                            Category.SetCheckParameter("RATA_Missing_Op_Data_Info", i + "Q" + j, eParameterDataType.String);
                                                            Status = "Missing Op Data";
                                                            Quit = true;
                                                        }
                                                    }
                                                }
                                                j++;
                                            }
                                            if (Quit)
                                                break;
                                        }
                                        if (Status == "")
                                            Status = "IC-Grace";
                                    }
                                }
                            }
                        }
                    }
                    if (PriorRATAIsAltSingle && Status == "OOC-Expired")
                    {
                        Status = "OOC-Incomplete QA RATA";

                        if (EmParameters.CurrentMhvParameter == "FLOW")
                            OverrideRATABAF.SetValue(PriorRataRec["OVERALL_BIAS_ADJ_FACTOR"].AsDecimal(), Category);
                    }
                    Category.SetCheckParameter("Current_RATA_Status", Status, eParameterDataType.String);
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATSTAT7");
            }

            return ReturnVal;
        }

        public string RATSTAT8(cCategory Category, ref bool Log)
        //Determine Final RATA Status    
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("Alternate_RATA_Record", null, eParameterDataType.DataRowView);
                DateTime CurrentDate = EmParameters.CurrentDateHour.AsStartDate();
                int CurrentHour = EmParameters.CurrentDateHour.AsStartHour();

                string MonSysId = EmParameters.QaStatusSystemId;
                string Status = Category.GetCheckParameter("Current_RATA_Status").ValueAsString();

                if (Status.StartsWith("OOC"))
                {
                    Category.SetCheckParameter("Invalid_RATA_Test_Number", null, eParameterDataType.String);

                    DataRowView InvMultiRec = Category.GetCheckParameter("Invalid_Multi_Level_RATA_Record").ValueAsDataRowView();
                    DataRowView InvRATARec = Category.GetCheckParameter("Invalid_RATA_Record").ValueAsDataRowView();

                    if (InvMultiRec != null)
                    {
                        Category.SetCheckParameter("Invalid_RATA_Test_Number", cDBConvert.ToString(InvMultiRec["TEST_NUM"]), eParameterDataType.String);
                        Status += "*";

                        if (EmParameters.CurrentMhvParameter == "FLOW")
                            Category.SetCheckParameter("RATA_Status_BAF", InvMultiRec["OVERALL_BIAS_ADJ_FACTOR"].AsDecimal(), eParameterDataType.Decimal);
                    }
                    else if (InvRATARec != null)
                    {
                        Category.SetCheckParameter("Invalid_RATA_Test_Number", cDBConvert.ToString(InvRATARec["TEST_NUM"]), eParameterDataType.String);
                        Status += "*";

                        if (EmParameters.CurrentMhvParameter == "FLOW")
                            Category.SetCheckParameter("RATA_Status_BAF", InvRATARec["OVERALL_BIAS_ADJ_FACTOR"].AsDecimal(), eParameterDataType.Decimal);
                    }
                    else if (OverrideRATABAF.Value.HasValue)
                    {
                        if (EmParameters.CurrentMhvParameter == "FLOW")
                            Category.SetCheckParameter("RATA_Status_BAF", OverrideRATABAF.Value, eParameterDataType.Decimal);
                    }
                }

                else if (Status.StartsWith("IC") || Status.StartsWith("Undetermined"))
                {
                    DataRowView PriorRataRec = Category.GetCheckParameter("Prior_RATA_Record").ValueAsDataRowView();
                    DataRowView PriorRataEventRec = Category.GetCheckParameter("Prior_RATA_Event_Record").ValueAsDataRowView();
                    if (EmParameters.QaStatusSystemTypeCode == "NOX")
                    {
                        string ComponentIDList = "";
                        string AlternateSystemIDList = "";
                        DataView MonSysCompRecs = Category.GetCheckParameter("Monitor_System_Component_Records_By_Hour_Location").ValueAsDataView();
                        string ThisMonSysId;
                        foreach (DataRowView drv in MonSysCompRecs)
                        {
                            ThisMonSysId = cDBConvert.ToString(drv["MON_SYS_ID"]);
                            if (cDBConvert.ToString(drv["COMPONENT_TYPE_CD"]).InList("CO2,NOX,O2") && ThisMonSysId == MonSysId)
                                ComponentIDList = ComponentIDList.ListAdd(cDBConvert.ToString(drv["COMPONENT_ID"]));
                        }
                        if (ComponentIDList != "")
                            foreach (DataRowView drv in MonSysCompRecs)
                            {
                                ThisMonSysId = cDBConvert.ToString(drv["MON_SYS_ID"]);
                                if (cDBConvert.ToString(drv["COMPONENT_ID"]).InList(ComponentIDList) &&
                                        cDBConvert.ToString(drv["SYS_TYPE_CD"]).InList("CO2,NOXC,O2"))
                                    AlternateSystemIDList = AlternateSystemIDList.ListAdd(ThisMonSysId);
                            }
                        DataRowView AltRATARec = null;
                        string AltRATARecTestResCd;
                        if (AlternateSystemIDList != "")
                        {
                            DataView RATATestRecs = Category.GetCheckParameter("RATA_Test_Records_By_Location_For_QA_Status").ValueAsDataView();
                            DataView RATATestRecsFound1;
                            DataView RATATestRecsFoundPass;
                            DataView RATATestRecsFoundFail;
                            sFilterPair[] Filter = new sFilterPair[1];
                            if (PriorRataEventRec != null)
                            {
                                //Find records matching PriorRATAEventRecord
                                DateTime CondBeginDate = cDBConvert.ToDate(PriorRataEventRec["CONDITIONAL_DATA_BEGIN_DATE"], DateTypes.START);
                                int CondBeginHour = cDBConvert.ToHour(PriorRataEventRec["CONDITIONAL_DATA_BEGIN_HOUR"], DateTypes.START);
                                if (CondBeginDate != DateTime.MinValue)
                                {
                                    Filter[0].Set("MON_SYS_ID", AlternateSystemIDList, eFilterPairStringCompare.InList);
                                    if (CurrentHour != 0)
                                        RATATestRecsFound1 = FindActiveRows(RATATestRecs, CondBeginDate, CondBeginHour, CurrentDate, CurrentHour - 1, "END_DATE", "END_HOUR", "END_DATE", "END_HOUR", Filter);
                                    else
                                        RATATestRecsFound1 = FindActiveRows(RATATestRecs, CondBeginDate, CondBeginHour, CurrentDate.AddDays(-1), 23, "END_DATE", "END_HOUR", "END_DATE", "END_HOUR", Filter);

                                    //find Passing records
                                    Filter[0].Set("TEST_RESULT_CD", "PASSED,PASSAPS", eFilterPairStringCompare.InList);
                                    RATATestRecsFoundPass = FindRows(RATATestRecsFound1, Filter);
                                    RATATestRecsFoundPass.Sort = "END_DATE DESC, END_HOUR DESC, END_MIN DESC";

                                    //find failed, null or inappropriate code records
                                    Filter[0].Set("TEST_RESULT_CD", "PASSED,PASSAPS", eFilterPairStringCompare.InList, true);
                                    RATATestRecsFoundFail = FindRows(RATATestRecsFound1, Filter);
                                    RATATestRecsFoundFail.Sort = "END_DATE DESC, END_HOUR DESC, END_MIN DESC, TEST_RESULT_CD DESC";

                                    //Find the most recent record, preferring PASSED/PASSAPS, then FAILED/ABORTED, then null
                                    if (RATATestRecsFoundFail.Count == 0 && RATATestRecsFoundPass.Count > 0)
                                        AltRATARec = RATATestRecsFoundPass[0];
                                    else
                                        if (RATATestRecsFoundFail.Count > 0 && RATATestRecsFoundPass.Count == 0)
                                        AltRATARec = RATATestRecsFoundFail[0];
                                    else
                                            if (RATATestRecsFoundFail.Count > 0 && RATATestRecsFoundPass.Count > 0)
                                    {
                                        if (cDBConvert.ToDate(RATATestRecsFoundPass[0]["BEGIN_DATE"], DateTypes.START) > cDBConvert.ToDate(RATATestRecsFoundFail[0]["BEGIN_DATE"], DateTypes.START) ||
                                                (cDBConvert.ToDate(RATATestRecsFoundPass[0]["BEGIN_DATE"], DateTypes.START) == cDBConvert.ToDate(RATATestRecsFoundFail[0]["BEGIN_DATE"], DateTypes.START) &&
                                                cDBConvert.ToHour(RATATestRecsFoundPass[0]["BEGIN_HOUR"], DateTypes.START) > cDBConvert.ToHour(RATATestRecsFoundFail[0]["BEGIN_HOUR"], DateTypes.START)))
                                            AltRATARec = RATATestRecsFoundPass[0];
                                        else
                                            AltRATARec = RATATestRecsFoundFail[0];
                                    }

                                    if (AltRATARec != null)
                                    {
                                        Category.SetCheckParameter("Alternate_RATA_Record", AltRATARec, eParameterDataType.DataRowView);
                                        AltRATARecTestResCd = cDBConvert.ToString(AltRATARec["TEST_RESULT_CD"]);
                                        //no code or inappropriate code
                                        if (AltRATARecTestResCd == "" || AltRATARecTestResCd.NotInList("PASSED,PASSAPS,FAILED,ABORTED"))
                                            Status = "OOC-Prior Alternate System RATA Has Critical Errors";
                                        else
                                            if (AltRATARecTestResCd == "FAILED")
                                            Status = "OOC-Prior Alternate System RATA Failed";
                                        else
                                                if (AltRATARecTestResCd == "ABORTED")
                                            Status = "OOC-Prior Alternate System RATA Aborted";
                                    }
                                    else
                                    {
                                        //refilter for QA_NEEDS_EVAL_FLG
                                        Filter[0].Set("QA_NEEDS_EVAL_FLG", "Y", eFilterDataType.String);
                                        RATATestRecsFoundFail = FindRows(RATATestRecsFound1, Filter);
                                        if (RATATestRecsFoundFail.Count > 0)
                                        {
                                            RATATestRecsFoundFail.Sort = "END_DATE DESC, END_HOUR DESC, END_MIN DESC, TEST_RESULT_CD DESC";
                                            AltRATARec = RATATestRecsFoundFail[0];
                                            Category.SetCheckParameter("Alternate_RATA_Record", AltRATARec, eParameterDataType.DataRowView);
                                            Status = "Prior Alternate System RATA Not Yet Evaluated";
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (PriorRataRec != null)
                                {
                                    //Find records matching PriorRATARecord
                                    DateTime PriorRecEndDate = cDBConvert.ToDate(PriorRataRec["END_DATE"], DateTypes.END);
                                    int PriorRecEndHour = cDBConvert.ToHour(PriorRataRec["END_HOUR"], DateTypes.END);
                                    Filter[0].Set("MON_SYS_ID", AlternateSystemIDList, eFilterPairStringCompare.InList);
                                    if (CurrentHour != 0)
                                        RATATestRecsFound1 = FindActiveRows(RATATestRecs, PriorRecEndDate, PriorRecEndHour, CurrentDate, CurrentHour - 1, "END_DATE", "END_HOUR", "END_DATE", "END_HOUR", Filter);
                                    else
                                        RATATestRecsFound1 = FindActiveRows(RATATestRecs, PriorRecEndDate, PriorRecEndHour, CurrentDate.AddDays(-1), 23, "END_DATE", "END_HOUR", "END_DATE", "END_HOUR", Filter);

                                    //find Passing records
                                    Filter[0].Set("TEST_RESULT_CD", "PASSED,PASSAPS", eFilterPairStringCompare.InList);
                                    RATATestRecsFoundPass = FindRows(RATATestRecsFound1, Filter);
                                    RATATestRecsFoundPass.Sort = "END_DATE DESC, END_HOUR DESC, END_MIN DESC";

                                    //find failed, null or inappropriate code records
                                    Filter[0].Set("TEST_RESULT_CD", "PASSED,PASSAPS", eFilterPairStringCompare.InList, true);
                                    RATATestRecsFoundFail = FindRows(RATATestRecsFound1, Filter);
                                    RATATestRecsFoundFail.Sort = "END_DATE DESC, END_HOUR DESC, END_MIN DESC, TEST_RESULT_CD DESC";

                                    //Find the most recent record, preferring PASSED/PASSAPS, then FAILED/ABORTED, then null
                                    if (RATATestRecsFoundFail.Count == 0 && RATATestRecsFoundPass.Count > 0)
                                        AltRATARec = RATATestRecsFoundPass[0];
                                    else
                                        if (RATATestRecsFoundFail.Count > 0 && RATATestRecsFoundPass.Count == 0)
                                        AltRATARec = RATATestRecsFoundFail[0];
                                    else
                                            if (RATATestRecsFoundFail.Count > 0 && RATATestRecsFoundPass.Count > 0)
                                    {
                                        if (cDBConvert.ToDate(RATATestRecsFoundPass[0]["BEGIN_DATE"], DateTypes.START) > cDBConvert.ToDate(RATATestRecsFoundFail[0]["BEGIN_DATE"], DateTypes.START) ||
                                                (cDBConvert.ToDate(RATATestRecsFoundPass[0]["BEGIN_DATE"], DateTypes.START) == cDBConvert.ToDate(RATATestRecsFoundFail[0]["BEGIN_DATE"], DateTypes.START) &&
                                                cDBConvert.ToHour(RATATestRecsFoundPass[0]["BEGIN_HOUR"], DateTypes.START) > cDBConvert.ToHour(RATATestRecsFoundFail[0]["BEGIN_HOUR"], DateTypes.START)))
                                            AltRATARec = RATATestRecsFoundPass[0];
                                        else
                                            AltRATARec = RATATestRecsFoundFail[0];
                                    }

                                    if (AltRATARec != null)
                                    {
                                        Category.SetCheckParameter("Alternate_RATA_Record", AltRATARec, eParameterDataType.DataRowView);
                                        AltRATARecTestResCd = cDBConvert.ToString(AltRATARec["TEST_RESULT_CD"]);
                                        if (AltRATARecTestResCd == "" || AltRATARecTestResCd.NotInList("PASSED,PASSAPS,FAILED,ABORTED"))
                                            Status = "OOC-Prior Alternate System RATA Has Critical Errors";
                                        else
                                            if (AltRATARecTestResCd == "FAILED")
                                            Status = "OOC-Prior Alternate System RATA Failed";
                                        else
                                                if (AltRATARecTestResCd == "ABORTED")
                                            Status = "OOC-Prior Alternate System RATA Aborted";
                                    }
                                    else
                                    {
                                        //refilter for QA_NEEDS_EVAL_FLG
                                        Filter[0].Set("QA_NEEDS_EVAL_FLG", "Y", eFilterDataType.String);
                                        RATATestRecsFoundFail = FindRows(RATATestRecsFound1, Filter);
                                        if (RATATestRecsFoundFail.Count > 0)
                                        {
                                            RATATestRecsFoundFail.Sort = "END_DATE DESC, END_HOUR DESC, END_MIN DESC, TEST_RESULT_CD DESC";
                                            AltRATARec = RATATestRecsFoundFail[0];
                                            Category.SetCheckParameter("Alternate_RATA_Record", AltRATARec, eParameterDataType.DataRowView);
                                            Status = "Prior Alternate System RATA Not Yet Evaluated";
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (Status.StartsWith("IC") || Status.StartsWith("Undetermined"))
                    {
                        if (PriorRataRec == null)
                        {
                            Category.SetCheckParameter("RATA_Status_BAF", 1, eParameterDataType.Integer);
                        }
                        else if ((Status.StartsWith("Undetermined-Cond") || Status.StartsWith("IC-Cond") || Status.StartsWith("PendingOOC-Cond")) &&
                                         ((PriorRataEventRec["QA_CERT_EVENT_CD"].AsString().InList("40,50,51,100,101,120,125,151,250,255,300,305")) ||
                                            (!PriorRataRec["TEST_RESULT_CD"].AsString().StartsWith("PASS"))))
                        {
                            Category.SetCheckParameter("RATA_Status_BAF", 1, eParameterDataType.Integer);
                        }
                        else if (MonSysId != PriorRataRec["MON_SYS_ID"].AsString())
                        {
                            DataView RataTestRecords = Category.GetCheckParameter("RATA_Test_Records_By_Location_For_QA_Status").ValueAsDataView();

                            DateTime RangeEndDate; int RangeEndHour;
                            {
                                if (CurrentHour <= 0)
                                {
                                    RangeEndDate = CurrentDate.AddDays(-1);
                                    RangeEndHour = 23;
                                }
                                else
                                {
                                    RangeEndDate = CurrentDate;
                                    RangeEndHour = CurrentHour - 1;
                                }
                            }

                            // Using the method in this manner simulates checking for a specific row date/hour in a range
                            // and particularly in this case a row date/hour before a particular date/hour.
                            DataView RataTestView
                                 = cRowFilter.FindActiveRows(RataTestRecords,
                                                DateTime.MinValue, 0, RangeEndDate, RangeEndHour,
                                                "END_DATE", "END_HOUR", "END_DATE", "END_HOUR",
                                                new cFilterCondition[] { new cFilterCondition("MON_SYS_ID", MonSysId) });

                            DataView RataTestSpecificView;

                            RataTestSpecificView
                                = cRowFilter.FindRows(RataTestView, new cFilterCondition[] { new cFilterCondition("TEST_RESULT_CD", "INVALID", true) });

                            if (RataTestSpecificView.Count > 0)
                            {
                                RataTestSpecificView.Sort = "END_DATE desc, END_HOUR desc, END_MIN desc";
                                Category.SetCheckParameter("RATA_Status_BAF", RataTestSpecificView[0]["OVERALL_BIAS_ADJ_FACTOR"].AsDecimal(decimal.MinValue), eParameterDataType.Decimal);
                            }
                            else
                            {
                                Status = "OOC-No Prior Test or Event";

                                RataTestSpecificView
                                    = cRowFilter.FindRows(RataTestView, new cFilterCondition[] { new cFilterCondition("TEST_RESULT_CD", "INVALID") });

                                if (RataTestSpecificView.Count > 0)
                                {
                                    RataTestSpecificView.Sort = "END_DATE desc, END_HOUR desc, END_MIN desc";
                                    Category.SetCheckParameter("Invalid_RATA_Test_Number", RataTestSpecificView[0]["TEST_NUM"].AsString(""), eParameterDataType.String);
                                    Status += "*";
                                }
                            }
                        }
                        else
                        {
                            Category.SetCheckParameter("RATA_Status_BAF", cDBConvert.ToDecimal(PriorRataRec["OVERALL_BIAS_ADJ_FACTOR"]), eParameterDataType.Decimal);
                        }
                    }
                }

                if (!Status.StartsWith("IC"))
                    Category.CheckCatalogResult = Status;

                Category.SetCheckParameter("Current_RATA_Status", Status, eParameterDataType.String);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "RATSTAT8");
            }

            return ReturnVal;
        }

        #endregion

    }
}
