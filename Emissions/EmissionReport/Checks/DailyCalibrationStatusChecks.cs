using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.CheckEngine.SpecialParameterClasses;
using ECMPS.Checks.Em.Parameters;
using ECMPS.Checks.EmissionsReport;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Extensions;


namespace ECMPS.Checks.EmissionsChecks
{
    public class cDailyCalibrationStatusChecks : cEmissionsChecks
    {
        #region Constructors

        public cDailyCalibrationStatusChecks(cEmissionsReportProcess emissionReportProcess)
          : base(emissionReportProcess)
        {
            CheckProcedures = new dCheckProcedure[5];

            CheckProcedures[1] = new dCheckProcedure(DCSTAT1);
            CheckProcedures[2] = new dCheckProcedure(DCSTAT2);
            CheckProcedures[3] = new dCheckProcedure(DCSTAT3);
            CheckProcedures[4] = new dCheckProcedure(DCSTAT4);
        }

        /// <summary>
        /// Constructor used for testing.
        /// </summary>
        /// <param name="emissionParameters"></param>
        public cDailyCalibrationStatusChecks(cEmissionsCheckParameters emissionParameters)
        {
            EmManualParameters = emissionParameters;
        }

        #endregion

        #region Public Static Methods: Checks

        public string DCSTAT1(cCategory Category, ref bool Log)
        //Locate Most Recent Prior Daily Calibration Test
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("Prior_Daily_Cal_Record", null, eParameterDataType.DataRowView);
                EmParameters.PriorDailyCalLastCoveredNonOpHour = null;
                EmParameters.PriorDailyCalFirstOpHourAfterLastNonOpHour = null;
                Category.SetCheckParameter("Invalid_Daily_Cal_Record", null, eParameterDataType.DataRowView);

                string ApplCompId = Category.GetCheckParameter("Applicable_Component_ID").ValueAsString();
                string AnalyzerRangeUsed = Convert.ToString(Category.GetCheckParameter("Current_Analyzer_Range_Used").ParameterValue);

                Category.SetCheckParameter("Prior_Daily_Cal_Rec_Calc_Test_Res", null, eParameterDataType.String);
                cDailyCalibrationData DailyCalStatRecs = (cDailyCalibrationData)Category.GetCheckParameter("Most_Recent_Daily_Calibration_Test_Object").ParameterValue;

                DataRowView FoundDailyCalRec;
                string CalcTestRes = "";
                cDailyCalibrationTestData mostRecentDailyCalibrationTestObject;

                if (DailyCalStatRecs.GetMostRecent(ApplCompId, true, AnalyzerRangeUsed, out mostRecentDailyCalibrationTestObject))
                {
                    FoundDailyCalRec = mostRecentDailyCalibrationTestObject.DailyCalibrationRow;
                    CalcTestRes = mostRecentDailyCalibrationTestObject.CalculatedTestResultCd;

                    Category.SetCheckParameter("Prior_Daily_Cal_Record", FoundDailyCalRec, eParameterDataType.DataRowView);
                    Category.SetCheckParameter("Prior_Daily_Cal_Rec_Calc_Test_Res", CalcTestRes, eParameterDataType.String);


                    if (EmParameters.QaStatusPrimaryOrPrimaryBypassSystemId == null)
                    {
                        EmParameters.PriorDailyCalLastCoveredNonOpHour = mostRecentDailyCalibrationTestObject.LocationSupplementalValues.LastCoveredNonOpHour;
                        EmParameters.PriorDailyCalFirstOpHourAfterLastNonOpHour = mostRecentDailyCalibrationTestObject.LocationSupplementalValues.FirstOpHourAfterLastCoveredNonOpHour;
                    }
                    else if (mostRecentDailyCalibrationTestObject.SystemSupplementalValuesDictionary.ContainsKey(EmParameters.QaStatusPrimaryOrPrimaryBypassSystemId))
                    {
                        EmParameters.PriorDailyCalLastCoveredNonOpHour = mostRecentDailyCalibrationTestObject.SystemSupplementalValuesDictionary[EmParameters.QaStatusPrimaryOrPrimaryBypassSystemId].LastCoveredNonOpHour;
                        EmParameters.PriorDailyCalFirstOpHourAfterLastNonOpHour = mostRecentDailyCalibrationTestObject.SystemSupplementalValuesDictionary[EmParameters.QaStatusPrimaryOrPrimaryBypassSystemId].FirstOpHourAfterLastCoveredNonOpHour;
                    }


                    if (DailyCalStatRecs.GetMostRecent(ApplCompId, false, AnalyzerRangeUsed, out FoundDailyCalRec, out CalcTestRes))
                    {
                        DataRowView PriorDailyCalRec = Category.GetCheckParameter("Prior_Daily_Cal_Record").ValueAsDataRowView();
                        DateTime PriorTestDate = cDBConvert.ToDate(PriorDailyCalRec["DAILY_TEST_DATE"], DateTypes.START);
                        int PriorTestHour = cDBConvert.ToInteger(PriorDailyCalRec["DAILY_TEST_HOUR"]);
                        int PriorTestMin = cDBConvert.ToInteger(PriorDailyCalRec["DAILY_TEST_MIN"]);
                        DateTime thisDate = cDBConvert.ToDate(FoundDailyCalRec["DAILY_TEST_DATE"], DateTypes.START);
                        int thisHour = cDBConvert.ToInteger(FoundDailyCalRec["DAILY_TEST_HOUR"]);
                        int thisMin = cDBConvert.ToInteger(FoundDailyCalRec["DAILY_TEST_MIN"]);
                        if (thisDate > PriorTestDate || (thisDate == PriorTestDate && (thisHour > PriorTestHour || (thisHour == PriorTestHour && thisMin > PriorTestMin))))
                            Category.SetCheckParameter("Invalid_Daily_Cal_Record", FoundDailyCalRec, eParameterDataType.DataRowView);
                    }
                }
                else
                {
                    if (DailyCalStatRecs.GetMostRecent(ApplCompId, false, AnalyzerRangeUsed, out FoundDailyCalRec, out CalcTestRes))
                        Category.SetCheckParameter("Invalid_Daily_Cal_Record", FoundDailyCalRec, eParameterDataType.DataRowView);
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "DCSTAT1");
            }

            return ReturnVal;
        }

        public string DCSTAT2(cCategory Category, ref bool Log)
        //Locate Most Recent Prior Event
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("Prior_Daily_Cal_Event_Record", null, eParameterDataType.DataRowView);
                Category.SetCheckParameter("Current_Daily_Cal_Status", null, eParameterDataType.String);

                DataRowView PriorDailyCalEventRec = null;//for readability, same as QACertRecsFound[0]
                string Status = "";

                DataView QACertEventRecs = Category.GetCheckParameter("QA_Certification_Event_Records").ValueAsDataView();
                DataTable QACertEventRecsTable = QACertEventRecs.Table.Copy();
                string ApplCompId = Category.GetCheckParameter("Applicable_Component_ID").ValueAsString();
                string AnalyzerRangeUsed = Convert.ToString(Category.GetCheckParameter("Current_Analyzer_Range_Used").ParameterValue);
                DataView QACertRecsFound = FindCertEventRecs(QACertEventRecsTable, "Prior_Daily_Cal_Record", ApplCompId, AnalyzerRangeUsed, true, Category);

                if (QACertRecsFound != null && QACertRecsFound.Count > 0)
                {
                    QACertRecsFound.Sort = "QA_CERT_EVENT_DATE DESC, QA_CERT_EVENT_HOUR DESC";
                    PriorDailyCalEventRec = QACertRecsFound[0];
                }

                /* Check for an event record found */
                if (PriorDailyCalEventRec != null)
                {
                    Category.SetCheckParameter("Prior_Daily_Cal_Event_Record", PriorDailyCalEventRec, eParameterDataType.DataRowView);
                }

                if (PriorDailyCalEventRec == null)
                {
                    DataRowView PriorDailyCalRec = Category.GetCheckParameter("Prior_Daily_Cal_Record").ValueAsDataRowView();
                    if (PriorDailyCalRec == null)
                    {
                        DateTime MHVDate = EmParameters.CurrentDateHour.AsStartDate();
                        int MHVHour = EmParameters.CurrentDateHour.AsStartHour();
                        DateTime FirstOpDate = Category.GetCheckParameter("First_Day_of_Operation").ValueAsDateTime(DateTypes.START);
                        int FirstOpHour = Category.GetCheckParameter("First_Hour_of_Operation").ValueAsInt();

                        if ((cDateFunctions.HourDifference(FirstOpDate, FirstOpHour, MHVDate, MHVHour) - 1) /*Hours Between*/ < 25)
                            Status = "IC-Undetermined";

                        /* 
                         * Set to IC-Undetermined if the earliest System Component for the component occurred less than 25 hours 
                         * between the begin hour of the component and the current hour 
                         */
                        else if (EmParameters.QaStatusSystemTypeCode.InList("HG,HCL") &&
                                 (cDateFunctions.HourDifference(EmParameters.QaStatusComponentBeginDatehour.Value, EmParameters.CurrentDateHour.Value) - 1) < 25)
                        {
                            Status = "IC-Undetermined";
                        }

                        /* 
                         * Set to IC-Undetermined if the earliest System Component for the component occurred less than 25 hours 
                         * between the begin hour of the component and the current hour 
                         */
                        else if ((EmParameters.QaStatusSystemTypeCode == "SO2") && (EmParameters.So2cIsOnlyForMats == true) &&
                                 (cDateFunctions.HourDifference(EmParameters.QaStatusComponentBeginDatehour.Value, EmParameters.CurrentDateHour.Value) - 1) < 25)
                        {
                            Status = "IC-Undetermined";
                        }

                        /* 
                         * Set the status to IC-Undetermined for Hg and HCl systems when the Current Date and hour are before 
                         * the MATS Daily Calibration Required Date.  This prevents an OOC in the period when a location has
                         * been operating before starting to report MATS values in a quarter.
                         */
                        else if (EmParameters.QaStatusSystemTypeCode.InList("HG,HCL") &&
                                 (EmParameters.MatsDailyCalRequiredDatehour != null) && (EmParameters.CurrentDateHour < EmParameters.MatsDailyCalRequiredDatehour))
                        {
                            Status = "IC-Undetermined";
                        }

                        /*
                         * Set the status to IC-Undetermined for Hg and HCl systems when the Current Date and hour are before
                         * the QA Status MATS ERB Date.  This prevents an OOC in the period when a location has been operating
                         * before starting to report emissions for MATS.
                         */
                        else if (EmParameters.QaStatusSystemTypeCode.InList("HG,HCL") &&
                                 (EmParameters.QaStatusMatsErbDate != null) &&
                                 (cDateFunctions.HourDifference(EmParameters.QaStatusMatsErbDate.Value, EmParameters.CurrentDateHour.Value) - 1) < 25)
                        {
                            Status = "IC-Undetermined";
                        }

                        else
                        {
                            //Create date and hour = 24th hour after first op hour
                            DateTime FirstHrPlus24Date = FirstOpDate.AddDays(1);
                            int FirstHourPlus24Hour = FirstOpHour;

                            DataView HrlyNonOpRecs = Category.GetCheckParameter("Hourly_Non_Operating_Data_Records_for_Location").ValueAsDataView();
                            HrlyNonOpRecs.Sort = "BEGIN_DATE, BEGIN_HOUR";//use this sort for performance, record likely being early in quarter
                            DateTime thisDate = DateTime.MinValue;//compiler requires init
                            int thisHour = 0;//compiler requires init                                
                            DataRowView FoundHrlyOpRec = null;
                            foreach (DataRowView drv1 in HrlyNonOpRecs)
                            {
                                thisDate = cDBConvert.ToDate(drv1["BEGIN_DATE"], DateTypes.START);
                                thisHour = cDBConvert.ToInteger(drv1["BEGIN_HOUR"]);
                                if (thisDate < FirstHrPlus24Date || (thisDate == FirstHrPlus24Date && thisHour <= FirstHourPlus24Hour))
                                    FoundHrlyOpRec = drv1;
                                else
                                    break;
                            }
                            if (FoundHrlyOpRec != null)
                            {
                                DataView HrlyOpRecs = Category.GetCheckParameter("Hourly_Operating_Data_Records_for_Location").ValueAsDataView();
                                HrlyOpRecs.Sort = "BEGIN_DATE, BEGIN_HOUR";//use this sort for performance, record likely being early in quarter
                                DateTime FoundDate = cDBConvert.ToDate(FoundHrlyOpRec["BEGIN_DATE"], DateTypes.START);
                                int FoundHour = cDBConvert.ToInteger(FoundHrlyOpRec["BEGIN_HOUR"]);
                                bool Found = false;
                                foreach (DataRowView drv2 in HrlyOpRecs)
                                {
                                    thisDate = cDBConvert.ToDate(drv2["BEGIN_DATE"], DateTypes.START);
                                    thisHour = cDBConvert.ToInteger(drv2["BEGIN_HOUR"]);
                                    if ((thisDate > FoundDate || (thisDate == FoundDate && thisHour > FoundHour)) &&
                                        (thisDate < MHVDate || (thisDate == MHVDate && thisHour <= MHVHour)))
                                    {
                                        Found = true;
                                        break;
                                    }
                                }
                                if (!Found || cDateFunctions.HourDifference(thisDate, thisHour, MHVDate, MHVHour) < 8)
                                    Status = "IC-Undetermined";
                                else
                                    Status = "OOC-No Prior Test";
                            }
                            else
                                Status = "OOC-No Prior Test";
                        }
                    }
                    else
                    {
                        string TestResCd = Category.GetCheckParameter("Prior_Daily_Cal_Rec_Calc_Test_Res").ValueAsString(); //cDBConvert.ToString(PriorDailyCalRec["TEST_RESULT_CD"]);
                        if (TestResCd == "")
                            Status = "OOC-Test Has Critical Errors";
                        else
                          if (TestResCd == "FAILED")
                            Status = "OOC-Test Failed";
                        else
                            if (TestResCd == "ABORTED")
                            Status = "OOC-Test Aborted";
                    }
                }
                else
                {
                    Status = "OOC-Event";
                    DataRowView InvDailyCalRec = Category.GetCheckParameter("Invalid_Daily_Cal_Record").ValueAsDataRowView();
                    DateTime PriorCertDate = cDBConvert.ToDate(PriorDailyCalEventRec["QA_CERT_EVENT_DATE"], DateTypes.START);
                    int PriorCertHour = cDBConvert.ToInteger(PriorDailyCalEventRec["QA_CERT_EVENT_HOUR"]);
                    if (InvDailyCalRec != null && (cDBConvert.ToDate(InvDailyCalRec["DAILY_TEST_DATE"], DateTypes.START) < PriorCertDate ||
                        (cDBConvert.ToDate(InvDailyCalRec["DAILY_TEST_DATE"], DateTypes.START) == PriorCertDate && cDBConvert.ToInteger(InvDailyCalRec["DAILY_TEST_HOUR"]) < PriorCertHour)))
                        Category.SetCheckParameter("Invalid_Daily_Cal_Record", null, eParameterDataType.DataRowView);
                }
                if (Status != "")
                    Category.SetCheckParameter("Current_Daily_Cal_Status", Status, eParameterDataType.String);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "DCSTAT2");
            }

            return ReturnVal;
        }

        public string DCSTAT3(cCategory Category, ref bool Log)
        //Determine Test Expiration Date for Most Recent Prior Daily Calibration Test
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("Online_Daily_Cal_Record", null, eParameterDataType.DataRowView);

                if (Category.GetCheckParameter("Current_Daily_Cal_Status").ParameterValue == null)
                {
                    DataRowView PriorDailyCalRec = Category.GetCheckParameter("Prior_Daily_Cal_Record").ValueAsDataRowView();
                    DateTime PriorTestDate = cDBConvert.ToDate(PriorDailyCalRec["DAILY_TEST_DATE"], DateTypes.START);
                    int PriorTestHour = cDBConvert.ToHour(PriorDailyCalRec["DAILY_TEST_HOUR"], DateTypes.START);
                    DateTime MHVDate = EmParameters.CurrentDateHour.AsStartDate();
                    int MHVHour = EmParameters.CurrentDateHour.AsStartHour();
                    int HourDiff = cDateFunctions.HourDifference(PriorTestDate, PriorTestHour, MHVDate, MHVHour);//This will never be negative, so we don't need to take abs val
                    DataView HrlyOpRecs = Category.GetCheckParameter("Hourly_Operating_Data_Records_for_Location").ValueAsDataView();

                    string Status = "";

                    if (cDBConvert.ToInteger(PriorDailyCalRec["ONLINE_OFFLINE_IND"]) == 1)
                    {
                        if (HourDiff < 26)
                            Status = "IC";
                        else
                        {
                            if (EmParameters.PriorDailyCalLastCoveredNonOpHour != null)
                            {
                                if ((EmParameters.PriorDailyCalFirstOpHourAfterLastNonOpHour != null) && cDateFunctions.HourDifference(EmParameters.PriorDailyCalFirstOpHourAfterLastNonOpHour.Value.Date,
                                                                                                                                       EmParameters.PriorDailyCalFirstOpHourAfterLastNonOpHour.Value.Hour, 
                                                                                                                                       MHVDate, 
                                                                                                                                       MHVHour) >= 8)
                                    Status = "OOC-Expired";
                                else
                                    Status = "IC-Grace";
                            }
                            else
                                Status = "OOC-Expired";
                        }
                    }
                    else
                    {
                        string ApplCompId = Category.GetCheckParameter("Applicable_Component_ID").ValueAsString();
                        string AnalyzerRangeUsed = Convert.ToString(Category.GetCheckParameter("Current_Analyzer_Range_Used").ParameterValue);
                        cDailyCalibrationData DailyCalStatRecs = (cDailyCalibrationData)Category.GetCheckParameter("Most_Recent_Daily_Calibration_Test_Object").ParameterValue;

                        DataRowView OnlineDailyCalRec;
                        string CalcTestRes = "";
                        cDailyCalibrationTestData mostRecentDailyCalibrationTestObject;

                        if (DailyCalStatRecs.GetMostRecent(ApplCompId, true, AnalyzerRangeUsed, true, out mostRecentDailyCalibrationTestObject))
                        {
                            OnlineDailyCalRec = mostRecentDailyCalibrationTestObject.DailyCalibrationRow;
                            CalcTestRes = mostRecentDailyCalibrationTestObject.CalculatedTestResultCd;

                            Category.SetCheckParameter("Online_Daily_Cal_Record", OnlineDailyCalRec, eParameterDataType.DataRowView);


                            int? onlineDailyCalOpHourCount;
                            DateTime? onlineDailyCalLastCoveredNonOpHour, onlineDailyCalFirstOpHourAfterLastNonOpHour;
                            {
                                if (EmParameters.QaStatusPrimaryOrPrimaryBypassSystemId == null)
                                {
                                    onlineDailyCalOpHourCount = mostRecentDailyCalibrationTestObject.LocationSupplementalValues.OperatingHourCount;
                                    onlineDailyCalLastCoveredNonOpHour = mostRecentDailyCalibrationTestObject.LocationSupplementalValues.LastCoveredNonOpHour;
                                    onlineDailyCalFirstOpHourAfterLastNonOpHour = mostRecentDailyCalibrationTestObject.LocationSupplementalValues.FirstOpHourAfterLastCoveredNonOpHour;
                                }
                                else if (mostRecentDailyCalibrationTestObject.SystemSupplementalValuesDictionary.ContainsKey(EmParameters.QaStatusPrimaryOrPrimaryBypassSystemId))
                                {
                                    onlineDailyCalOpHourCount = mostRecentDailyCalibrationTestObject.SystemSupplementalValuesDictionary[EmParameters.QaStatusPrimaryOrPrimaryBypassSystemId].OperatingHourCount;
                                    onlineDailyCalLastCoveredNonOpHour = mostRecentDailyCalibrationTestObject.SystemSupplementalValuesDictionary[EmParameters.QaStatusPrimaryOrPrimaryBypassSystemId].LastCoveredNonOpHour;
                                    onlineDailyCalFirstOpHourAfterLastNonOpHour = mostRecentDailyCalibrationTestObject.SystemSupplementalValuesDictionary[EmParameters.QaStatusPrimaryOrPrimaryBypassSystemId].FirstOpHourAfterLastCoveredNonOpHour;
                                }
                                else
                                {
                                    onlineDailyCalOpHourCount = null;
                                    onlineDailyCalLastCoveredNonOpHour = null;
                                    onlineDailyCalFirstOpHourAfterLastNonOpHour = null;
                                }
                            }


                            DateTime OnlineDate = cDBConvert.ToDate(OnlineDailyCalRec["DAILY_TEST_DATE"], DateTypes.START);
                            int OnlineHour = cDBConvert.ToInteger(OnlineDailyCalRec["DAILY_TEST_HOUR"]);

                            if (Category.GetCheckParameter("Invalid_Daily_Cal_Record").ParameterValue == null)//not in param list
                            {
                                DataRowView FoundDailyCalRec;
                                string thisCalcTestRes;
                                if (DailyCalStatRecs.GetMostRecent(ApplCompId, false, AnalyzerRangeUsed, true, out FoundDailyCalRec, out thisCalcTestRes))
                                {
                                    DateTime FoundEndDate = cDBConvert.ToDate(FoundDailyCalRec["DAILY_TEST_DATE"], DateTypes.START);
                                    int FoundEndHour = cDBConvert.ToInteger(FoundDailyCalRec["DAILY_TEST_HOUR"]);
                                    if ((FoundEndDate > OnlineDate || (FoundEndDate == OnlineDate && FoundEndHour > OnlineHour)) &&
                                        (FoundEndDate < PriorTestDate || (FoundEndDate == PriorTestDate && FoundEndHour <= PriorTestHour)))
                                        Category.SetCheckParameter("Invalid_Daily_Cal_Record", FoundDailyCalRec, eParameterDataType.DataRowView);
                                }
                            }
                            
                            if (CalcTestRes == "" || CalcTestRes == null)
                                Status = "OOC-Prior Online Test Has Critical Errors";
                            else if (CalcTestRes == "FAILED")
                                Status = "OOC-Recertification Test Failed";
                            else if (CalcTestRes == "ABORTED")
                                Status = "OOC-Prior Online Test Aborted";
                            else if ((onlineDailyCalOpHourCount != null) && (onlineDailyCalOpHourCount.Value<= 26) &&
                                     (CountHoursBetween(PriorTestDate, PriorTestHour, MHVDate, MHVHour) < 26))
                                Status = "IC";
                            else if (CountHoursBetween(OnlineDate, OnlineHour, MHVDate, MHVHour) < 26)
                                Status = "IC";
                            else
                            {
                                if (onlineDailyCalLastCoveredNonOpHour != null)
                                {
                                    if ((onlineDailyCalFirstOpHourAfterLastNonOpHour != null) && 
                                         cDateFunctions.HourDifference(onlineDailyCalFirstOpHourAfterLastNonOpHour.Value.Date,
                                                                       onlineDailyCalFirstOpHourAfterLastNonOpHour.Value.Hour, 
                                                                       MHVDate, 
                                                                       MHVHour) >= 8)
                                        Status = "OOC-Expired";
                                    else
                                        Status = "IC-Grace";
                                }
                                else
                                    Status = "OOC-Expired";
                            }
                        }
                        else
                        {
                            int[] OpHrsAccumArray = Category.GetCheckParameter("Rpt_Period_Op_Hours_Accumulator_Array").ValueAsIntArray();
                            int CurrentPosition = Category.GetCheckParameter("Current_Monitor_Plan_Location_Postion").ValueAsInt();
                            if (OpHrsAccumArray != null && OpHrsAccumArray[CurrentPosition] < 26)
                                Status = "IC-Undetermined";
                            else
                                Status = "OOC-Expired";
                        }
                    }
                    if (Status != "")
                        Category.SetCheckParameter("Current_Daily_Cal_Status", Status, eParameterDataType.String);
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "DCSTAT3");
            }

            return ReturnVal;
        }

        public string DCSTAT4(cCategory Category, ref bool Log)
        //Determine Final Daily Calibration Status
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("Alternate_Daily_Cal_Record", null, eParameterDataType.DataRowView);
                DataRowView AltDailyCalRec = null;//we need variable, as well
                string Status = Category.GetCheckParameter("Current_Daily_Cal_Status").ValueAsString();
                if (Status.StartsWith("OOC"))
                {
                    if (Category.GetCheckParameter("Invalid_Daily_Cal_Record").ParameterValue != null)
                        Status += "*";
                    Category.CheckCatalogResult = Status;
                }
                else
                {
                    if (Category.GetCheckParameter("Dual_Range_Status").ValueAsBool() && Status.StartsWith("IC"))
                    {
                        string AltAnalyzerRange;
                        string AltComponentID;

                        string currentAnalyzerRange = Category.GetCheckParameter("Current_Analyzer_Range_Used").AsString();

                        if (currentAnalyzerRange == "H")
                        {
                            AltAnalyzerRange = "L";
                            AltComponentID = Category.GetCheckParameter("Low_Range_Component_ID").ValueAsString();
                        }
                        else
                        {
                            AltAnalyzerRange = "H";
                            AltComponentID = Category.GetCheckParameter("High_Range_Component_ID").ValueAsString();
                        }

                        cDailyCalibrationData DailyCalStatRecs = (cDailyCalibrationData)Category.GetCheckParameter("Most_Recent_Daily_Calibration_Test_Object").ParameterValue;
                        string CalcTestRes = "";
                        if (DailyCalStatRecs.GetMostRecent(AltComponentID, true, AltAnalyzerRange, out AltDailyCalRec, out CalcTestRes))
                            Category.SetCheckParameter("Alternate_Daily_Cal_Record", AltDailyCalRec, eParameterDataType.DataRowView);
                        if (AltDailyCalRec != null)
                        {
                            if (CalcTestRes == "" || CalcTestRes == null)
                                Status = "OOC-Alternate Range Test Has Critical Errors";
                            else if (CalcTestRes == "FAILED")
                                Status = "OOC-Alternate Range Test Failed";
                            else if (CalcTestRes == "ABORTED")
                                Status = "OOC-Alternate Range Test Aborted";
                            else
                            {
                                DataRowView foundDailyCal;
                                DateTime? opDateHour = Category.CurrentOpDate.AddHours(Category.CurrentOpHour);

                                cLastDailyCalibration lastFailedOrAbortedDailyCalObject = (cLastDailyCalibration)LastFailedOrAbortedDailyCalObject.Value;

                                bool lastAlternateFailedOrAborted = false;
                                {
                                    if (lastFailedOrAbortedDailyCalObject.Get(AltComponentID, AltAnalyzerRange, opDateHour, out foundDailyCal))
                                    {
                                        DataRowView priorDailyCalRecord = Category.GetCheckParameter("Prior_Daily_Cal_Record").AsDataRowView();

                                        if (priorDailyCalRecord == null)
                                        {
                                            lastAlternateFailedOrAborted = true;
                                        }
                                        else
                                        {
                                            DateTime priorDateHour = cDateFunctions.CombineToHour(priorDailyCalRecord, "DAILY_TEST_DATE", "DAILY_TEST_HOUR", "DAILY_TEST_MIN").Value;
                                            DateTime foundDateHour = cDateFunctions.CombineToHour(foundDailyCal, "DAILY_TEST_DATE", "DAILY_TEST_HOUR", "DAILY_TEST_MIN").Value;

                                            if (foundDateHour > priorDateHour)
                                            {
                                                lastAlternateFailedOrAborted = true;
                                            }
                                        }
                                    }
                                }

                                if (lastAlternateFailedOrAborted)
                                {
                                    Status = "OOC-No Passing Test After Alternate Range Failed Test";
                                }
                                else
                                {
                                    string applicableComponentId = Category.GetCheckParameter("Applicable_Component_ID").ValueAsString();

                                    if (lastFailedOrAbortedDailyCalObject.Get(applicableComponentId, currentAnalyzerRange, opDateHour, out foundDailyCal))
                                    {
                                        DateTime altDateHour = cDateFunctions.CombineToHour(AltDailyCalRec, "DAILY_TEST_DATE", "DAILY_TEST_HOUR", "DAILY_TEST_MIN").Value;
                                        DateTime foundDateHour = cDateFunctions.CombineToHour(foundDailyCal, "DAILY_TEST_DATE", "DAILY_TEST_HOUR", "DAILY_TEST_MIN").Value;

                                        if (foundDateHour > altDateHour)
                                        {
                                            Status = "OOC-No Passing Alternate Range Test After Failed Test";
                                        }
                                    }
                                }
                            }

                            if (Status.StartsWith("OOC"))
                            {
                                if (Category.GetCheckParameter("Invalid_Daily_Cal_Record").ParameterValue != null)
                                    Status += "*";
                                else
                                {
                                    DataRowView FoundDailyCalRec;
                                    if (DailyCalStatRecs.GetMostRecent(AltComponentID, false, AltAnalyzerRange, out FoundDailyCalRec, out CalcTestRes))
                                    {
                                        DateTime AltDate = cDBConvert.ToDate(AltDailyCalRec["DAILY_TEST_DATE"], DateTypes.START);
                                        int AltHour = cDBConvert.ToInteger(AltDailyCalRec["DAILY_TEST_HOUR"]);
                                        DateTime FoundEndDate = cDBConvert.ToDate(FoundDailyCalRec["DAILY_TEST_DATE"], DateTypes.START);
                                        int FoundEndHour = cDBConvert.ToInteger(FoundDailyCalRec["DAILY_TEST_HOUR"]);
                                        if (FoundEndDate > AltDate || (FoundEndDate == AltDate && FoundEndHour > AltHour))
                                        {
                                            Category.SetCheckParameter("Invalid_Daily_Cal_Record", FoundDailyCalRec, eParameterDataType.DataRowView);
                                            Status += "*";
                                        }
                                    }
                                }
                                Category.CheckCatalogResult = Status;
                            }
                        }
                    }
                    else
                      if (!Status.StartsWith("IC"))
                        Category.CheckCatalogResult = Status;
                }
                Category.SetCheckParameter("Current_Daily_Cal_Status", Status, eParameterDataType.String);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "DCSTAT4");
            }

            return ReturnVal;
        }

        #endregion

        #region Private Methods: Utilities

        private static int CountOpHoursBetween(DataView HrlyOpRecs, DateTime Date1In, int Hour1In, DateTime Date2In, int Hour2In)
        {
            DateTime Date1 = Date1In;
            int Hour1 = Hour1In;
            DateTime Date2 = Date2In;
            int Hour2;
            //if (Hour1In == 23)
            //{
            //    Hour1 = 0;
            //    Date1 = Date1In.AddDays(1);
            //}
            //else
            //    Hour1 = Hour1In + 1;
            if (Hour2In == 0)
            {
                Hour2 = 23;
                Date2 = Date2In.AddDays(-1);
            }
            else
                Hour2 = Hour2In - 1;

            sFilterPair[] Filter = new sFilterPair[1];
            Filter[0].Set("OP_TIME", 0m, eFilterDataType.Decimal, true);
            DataView HrlyOpRecsFound = FindActiveRows(HrlyOpRecs, Date1, Hour1, Date2, Hour2, "BEGIN_DATE", "BEGIN_HOUR", "BEGIN_DATE", "BEGIN_HOUR", Filter);
            return HrlyOpRecsFound.Count;
        }

        private static int CountHoursBetween(DateTime Date1, int Hour1, DateTime Date2, int Hour2)
        {
            int retVal;
            retVal = cDateFunctions.HourDifference(Date1, Hour1, Date2, Hour2);
            //if (retVal > 0)
            //    retVal--;//hours "between"
            return retVal;
        }

        private static DataView FindCertEventRecs(DataTable SourceTable, string DailyCalRecordParameterName, string ApplCompId, string AnalyzerRangeUsed, bool UseEventDate, cCategory ACategory)
        {
            DataView ReturnView = null;//method returns null if no records found

            //make a like results table and clear it
            DataTable FoundRecordTable = SourceTable.Clone();
            FoundRecordTable.Rows.Clear();

            DataRow FilterRow;
            string CompId;

            DataRowView PriorDailyCalRec = (DataRowView)ACategory.GetCheckParameter(DailyCalRecordParameterName).ParameterValue;
            DateTime CurrentDate = EmParameters.CurrentDateHour.AsStartDate();
            int CurrentHour = EmParameters.CurrentDateHour.AsStartHour();
            DateTime CheckDate;
            int CheckHour;
            string CertEventCd;
            bool DualRangeStatus = ACategory.GetCheckParameter("Dual_Range_Status").ValueAsBool();
            string HighCompId = ACategory.GetCheckParameter("High_Range_Component_ID").ValueAsString();
            string LowCompId = ACategory.GetCheckParameter("Low_Range_Component_ID").ValueAsString();
            int CurrentRptPer = ACategory.GetCheckParameter("Current_Reporting_Period").ValueAsInt();

            foreach (DataRow SourceRow in SourceTable.Rows)
            {
                CompId = cDBConvert.ToString(SourceRow["COMPONENT_ID"]);

                if (CompId == ApplCompId)
                {
                    if (UseEventDate)
                    {
                        CheckDate = cDBConvert.ToDate(SourceRow["QA_CERT_EVENT_DATE"], DateTypes.START);
                        CheckHour = cDBConvert.ToInteger(SourceRow["QA_CERT_EVENT_HOUR"]);
                    }
                    else
                    {
                        CheckDate = cDBConvert.ToDate(SourceRow["CONDITIONAL_DATA_BEGIN_DATE"], DateTypes.START);
                        CheckHour = cDBConvert.ToInteger(SourceRow["CONDITIONAL_DATA_BEGIN_HOUR"]);
                    }

                    if ((CheckDate < CurrentDate || (CheckDate == CurrentDate && CheckHour <= CurrentHour)) &&
                        ((UseEventDate && SourceRow["CONDITIONAL_DATA_BEGIN_DATE"] == DBNull.Value) || (!UseEventDate && SourceRow["CONDITIONAL_DATA_BEGIN_DATE"] != DBNull.Value)))
                        if ((PriorDailyCalRec == null && cDateFunctions.ThisReportingPeriod(CheckDate) == CurrentRptPer) ||
                            (PriorDailyCalRec != null && (CheckDate > cDBConvert.ToDate(PriorDailyCalRec["DAILY_TEST_DATE"], DateTypes.END) || (CheckDate == cDBConvert.ToDate(PriorDailyCalRec["DAILY_TEST_DATE"], DateTypes.END) && CheckHour > cDBConvert.ToHour(PriorDailyCalRec["DAILY_TEST_HOUR"], DateTypes.END)))))
                        {
                            CertEventCd = cDBConvert.ToString(SourceRow["QA_CERT_EVENT_CD"]);
                            if (!DualRangeStatus || HighCompId != LowCompId ||
                                (!CertEventCd.InList("20,25,26,30,172") && AnalyzerRangeUsed == "H") ||
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
            }
            return ReturnView;
        }


        #endregion
    }
}
