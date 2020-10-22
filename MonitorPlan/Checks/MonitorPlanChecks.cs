using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Data.Ecmps.CheckMp.Function;
using ECMPS.Checks.Data.Ecmps.Lookup.Table;
using ECMPS.Checks.Data.Ecmps.Dbo.View;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Extensions;

using ECMPS.Checks.Mp.Parameters;


namespace ECMPS.Checks.MonitorPlanChecks
{
    public class cMonitorPlanChecks : cChecks
    {
        public cMonitorPlanChecks()
        {
            CheckProcedures = new dCheckProcedure[14];

            CheckProcedures[1] = new dCheckProcedure(MONPLAN1);
            CheckProcedures[3] = new dCheckProcedure(MONPLAN3);
            CheckProcedures[4] = new dCheckProcedure(MONPLAN4);
            CheckProcedures[5] = new dCheckProcedure(MONPLAN5);
            CheckProcedures[6] = new dCheckProcedure(MONPLAN6);
            CheckProcedures[7] = new dCheckProcedure(MONPLAN7);
            CheckProcedures[8] = new dCheckProcedure(MONPLAN8);
            CheckProcedures[9] = new dCheckProcedure(MONPLAN9);
            CheckProcedures[10] = new dCheckProcedure(MONPLAN10);
            CheckProcedures[11] = new dCheckProcedure(MONPLAN11);
            CheckProcedures[12] = new dCheckProcedure(MONPLAN12);
            CheckProcedures[13] = new dCheckProcedure(MONPLAN13);
        }


        #region Monitor Plan Checks

        public static string MONPLAN1(cCategory Category, ref bool Log)
        // Monitoring Plan Has Affected Unit
        {
            string ReturnVal = "";

            try
            {
                DataView MonitoringPlanProgramRecords = (DataView)Category.GetCheckParameter("Monitoring_Plan_Program_List").ParameterValue;

                bool ProgramFound = false;

                foreach (DataRowView MonitoringPlanProgramRow in MonitoringPlanProgramRecords)
                {
                    if (cDBConvert.ToDate(MonitoringPlanProgramRow["END_DATE"], DateTypes.END) >= Category.CheckEngine.EvalDefaultedBeganDate)
                    {
                        ProgramFound = true;
                        break;
                    }
                }

                if (ProgramFound)
                    Category.SetCheckParameter("evaluate_monitoring_plan", true, eParameterDataType.Boolean);
                else
                {
                    Category.SetCheckParameter("evaluate_monitoring_plan", false, eParameterDataType.Boolean);
                    Category.CheckCatalogResult = "A";
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "MONPLAN1");
            }

            return ReturnVal;
        }

        public static string MONPLAN3(cCategory Category, ref bool Log)
        // Duplicate Monitoring Plan Comment Records
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentMonitoringPlanComment = (DataRowView)Category.GetCheckParameter("Current_Monitoring_Plan_Comment").ParameterValue;
                DataView MonitoringPlanCommentRecords = (DataView)Category.GetCheckParameter("Monitoring_Plan_Comment_Records").ParameterValue;

                string MonPlanComment = cDBConvert.ToString(CurrentMonitoringPlanComment["Mon_Plan_Comment"]);
                if (cDBConvert.ToString(CurrentMonitoringPlanComment["Submission_Availability_Cd"]) != "UPDATED")
                {
                    sFilterPair[] BeganDateFilter = new sFilterPair[2];
                    BeganDateFilter[0].Set("Mon_Plan_Comment", MonPlanComment);
                    BeganDateFilter[1].Set("Begin_Date", CurrentMonitoringPlanComment["Begin_Date"], eFilterDataType.DateBegan);

                    if (CurrentMonitoringPlanComment["End_Date"] == DBNull.Value)
                    {
                        if (DuplicateRecordCheck(CurrentMonitoringPlanComment, MonitoringPlanCommentRecords,
                                                 "Mon_Plan_Comment_Id", BeganDateFilter))
                            Category.CheckCatalogResult = "A";
                    }
                    else
                    {
                        sFilterPair[] EndedDateFilter = new sFilterPair[2];
                        EndedDateFilter[0].Set("Mon_Plan_Comment", MonPlanComment);
                        EndedDateFilter[1].Set("End_Date", CurrentMonitoringPlanComment["End_Date"], eFilterDataType.DateEnded);

                        if (DuplicateRecordCheck(CurrentMonitoringPlanComment, MonitoringPlanCommentRecords,
                                                 "Mon_Plan_Comment_Id", BeganDateFilter, EndedDateFilter))
                            Category.CheckCatalogResult = "A";
                    }
                }
                else
                    Category.CheckCatalogResult = "B";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "MONPLAN3");
            }

            return ReturnVal;
        }

        public static string MONPLAN4(cCategory Category, ref bool Log)
        // Monitor Plan Comment Begin Date Valid
        {
            string ReturnVal = "";

            try
            {
                ReturnVal = Check_ValidStartDate(Category, "Monitor_Plan_Comment_Begin_Date_Valid",
                                                           "Current_Monitoring_Plan_Comment");
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "MONPLAN4");
            }

            return ReturnVal;
        }

        public static string MONPLAN5(cCategory Category, ref bool Log)
        // Monitor Plan Comment End Date Valid
        {
            string ReturnVal = "";

            try
            {
                ReturnVal = Check_ValidEndDate(Category, "Monitor_Plan_Comment_End_Date_Valid",
                                                         "Current_Monitoring_Plan_Comment");
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "MONPLAN5");
            }

            return ReturnVal;
        }

        public static string MONPLAN6(cCategory Category, ref bool Log)
        // Monitor Plan Comment Dates Consistent
        {
            string ReturnVal = "";

            try
            {
                ReturnVal = Check_ConsistentDateRange(Category, "Span_Dates_and_Hours_Consistent",
                                                                "Current_Monitoring_Plan_Comment",
                                                                "Monitor_Plan_Comment_Begin_Date_Valid",
                                                                "Monitor_Plan_Comment_End_Date_Valid");
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "MONPLAN6");
            }

            return ReturnVal;
        }

        public static string MONPLAN7(cCategory Category, ref bool Log)
        // Monitor Plan Comment Valid 
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentMonitoringPlanComment = (DataRowView)Category.GetCheckParameter("Current_Monitoring_Plan_Comment").ParameterValue;

                if (CurrentMonitoringPlanComment["Mon_Plan_Comment"] == DBNull.Value)
                    Category.CheckCatalogResult = "A";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "MONPLAN7");
            }

            return ReturnVal;
        }

        public static string MONPLAN8(cCategory Category, ref bool Log)
        //Initialize Variables
        {
            string ReturnVal = "";

            try
            {
                int? FirstEcmpsRptPeriodId = Category.GetCheckParameter("First_ECMPS_Reporting_Period").ValueAsNullOrInt();

                if (FirstEcmpsRptPeriodId == null)
                    Category.SetCheckParameter("ECMPS_MP_Begin_Date", new DateTime(2009, 1, 1), eParameterDataType.Date);
                else
                    Category.SetCheckParameter("ECMPS_MP_Begin_Date", cDateFunctions.StartDateThisQuarter(FirstEcmpsRptPeriodId.Value), eParameterDataType.Date);

                Category.SetCheckParameter("Protocol_Gases", null, eParameterDataType.String);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "MONPLAN8");
            }

            return ReturnVal;
        }

        public static string MONPLAN9(cCategory category, ref bool log)
        // Monitoring Plan Has Actively Reporting Units
        {
            string returnVal = "";

            try
            {
                DataRowView currentMonitoringPlanConfiguration = category.GetCheckParameter("Current_Monitoring_Plan_Configuration").AsDataRowView();
                DataView monitoringPlanOpStatusRecords = category.GetCheckParameter("Monitoring_Plan_Op_Status_Records").AsDataView();
                DataView mpLocations = category.GetCheckParameter("MP_Locations").AsDataView();

                int? beginRptPeriodId = currentMonitoringPlanConfiguration["BEGIN_RPT_PERIOD_ID"].AsInteger();
                int? endRptPeriodId = currentMonitoringPlanConfiguration["END_RPT_PERIOD_ID"].AsInteger();

                if (endRptPeriodId == null)
                {
                    DataView monitoringPlanOpStatusView
                      = cRowFilter.FindRows(monitoringPlanOpStatusRecords,
                                            new cFilterCondition[] { new cFilterCondition("OP_STATUS_CD", "RET,LTCS", eFilterConditionStringCompare.InList),
                                                           new cFilterCondition("END_DATE", DateTime.MaxValue, eFilterDataType.DateEnded)});

                    DataView mpCommonLocations
                      = cRowFilter.FindRows(mpLocations, new cFilterCondition[] { new cFilterCondition("LOCATION_TYPE", "CS,CP", eFilterConditionStringCompare.InList) });

                    if ((monitoringPlanOpStatusView.Count > 0) && (mpCommonLocations.Count > 0))
                    {
                        monitoringPlanOpStatusView
                          = cRowFilter.FindRows(monitoringPlanOpStatusRecords,
                                                new cFilterCondition[] { new cFilterCondition("OP_STATUS_CD", "OPR"),
                                                             new cFilterCondition("END_DATE", DateTime.MaxValue, eFilterDataType.DateEnded)});

                        if (monitoringPlanOpStatusView.Count > 0)
                            category.CheckCatalogResult = "A";
                    }
                }
                else
                {
                    if (endRptPeriodId.Value < beginRptPeriodId.Value)
                    {
                        category.CheckCatalogResult = "B";
                    }
                    else
                    {
                        DataView emissionsFileRecords = category.GetCheckParameter("Emissions_File_Records").AsDataView();
                        DataView emissionsFileView = emissionsFileRecords.ToTable().DefaultView;
                        cReportingPeriod endReportingPeriod = cReportingPeriod.GetReportingPeriod(endRptPeriodId.Value);

                        emissionsFileView.RowFilter = string.Format("CALENDAR_YEAR > {0} or CALENDAR_YEAR = {0} and QUARTER > {1}",
                                                                    endReportingPeriod.Year,
                                                                    endReportingPeriod.Quarter.AsInteger());

                        if (emissionsFileView.Count > 0)
                        {
                            category.CheckCatalogResult = "C";
                        }
                        else
                        {
                            DateTime beganRangeBegan = new DateTime(endReportingPeriod.Year - 1, 1, 1);
                            DateTime beganRangeEnded = new DateTime(endReportingPeriod.Year - 1, 12, 31);
                            DateTime endedLimit = endReportingPeriod.EndedDate;

                            DataView monitoringPlanOpStatusView
                              = cRowFilter.FindRows(monitoringPlanOpStatusRecords,
                                                    new cFilterCondition[] { new cFilterCondition("OP_STATUS_CD", "RET,LTCS", eFilterConditionStringCompare.InList),
                                                               new cFilterCondition("BEGIN_DATE", beganRangeBegan, eFilterDataType.DateBegan, eFilterConditionRelativeCompare.GreaterThanOrEqual),
                                                               new cFilterCondition("BEGIN_DATE", beganRangeEnded, eFilterDataType.DateBegan, eFilterConditionRelativeCompare.LessThanOrEqual),
                                                               new cFilterCondition("END_DATE", endedLimit, eFilterDataType.DateEnded, eFilterConditionRelativeCompare.GreaterThan)});

                            DataView mpCommonLocations
                              = cRowFilter.FindRows(mpLocations, new cFilterCondition[] { new cFilterCondition("LOCATION_TYPE", "CS,CP", eFilterConditionStringCompare.InList) });

                            if ((monitoringPlanOpStatusView.Count > 0) && (mpCommonLocations.Count > 0))
                                category.CheckCatalogResult = "A";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                returnVal = category.CheckEngine.FormatError(ex, "MONPLAN9");
            }

            return returnVal;
        }

        public static string MONPLAN10(cCategory Category, ref bool Log)
        //Determine If Monitor Plan Can Be Submitted
        {
            string ReturnVal = "";

            try
            {
                DataRowView currentMonitoringPlanConfiguration = Category.GetCheckParameter("Current_Monitoring_Plan_Configuration").AsDataRowView();
                int? beginRptPeriodId = currentMonitoringPlanConfiguration["BEGIN_RPT_PERIOD_ID"].AsInteger();
                cReportingPeriod beginReportingPeriod = cReportingPeriod.GetReportingPeriod(beginRptPeriodId.Value);
                if (beginReportingPeriod.BeganDate > DateTime.Now.AddDays(60))
                    Category.CheckCatalogResult = "A";

            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "MONPLAN10");
            }

            return ReturnVal;
        }

        /// <summary>
        /// Monitoring Plan Has Valid MATS Methods
        /// </summary>
        /// <param name="category"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public static string MONPLAN11(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                MpParameters.MatsRequiredCheck = false;
                MpParameters.MatsEvaluationBeginDate = null;

                DateTime? monitorMethodEarliestDate = null;
                {
                    VwMpMonitorMethodRow mpMonitorMethodRow
                      = MpParameters.MpMethodRecords.FindEarliestRow
                        (
                          new cFilterCondition("PARAMETER_CD", "HGRE,HGRH,HCLRE,HCLRH,HFRE,HFRH,SO2RE,SO2RH", eFilterConditionStringCompare.InList)
                        );

                    if (mpMonitorMethodRow != null)
                    {
                        monitorMethodEarliestDate = mpMonitorMethodRow.BeginDate;
                        MpParameters.MatsRequiredCheck = true;
                    }
                }

                DateTime? supplementalMethodEarliestDate = null;
                {
                    MATSMethodDataParameter matsMpSupplementalComplianceMethodRow
                      = MpParameters.MatsMpSupplementalComplianceMethodRecords.FindEarliestRow();

                    if (matsMpSupplementalComplianceMethodRow != null)
                    {
                        supplementalMethodEarliestDate = matsMpSupplementalComplianceMethodRow.BeginDate;
                    }
                }

                DateTime? matsRuleComplianceDate = null;
                {
                    VwSystemParameterRow systemParameterRow
                      = (MpParameters.SystemParameterLookupTable != null) ? MpParameters.SystemParameterLookupTable.FindRow(new cFilterCondition("SYS_PARAM_NAME", "MATS_RULE")) : null;

                    if (systemParameterRow != null)
                    {
                        matsRuleComplianceDate = systemParameterRow.ParamValue1.AsDateTime(); // ParamValue1 is the MATS Rule Compliance Date
                    }
                }


                // Set MatsEvaluationBeginDate
                if ((supplementalMethodEarliestDate != null) && (monitorMethodEarliestDate != null))
                {
                    MpParameters.MatsEvaluationBeginDate = (supplementalMethodEarliestDate < monitorMethodEarliestDate) ? supplementalMethodEarliestDate : monitorMethodEarliestDate;
                }
                else if (supplementalMethodEarliestDate != null)
                {
                    MpParameters.MatsEvaluationBeginDate = supplementalMethodEarliestDate;
                }
                else if (monitorMethodEarliestDate != null)
                {
                    MpParameters.MatsEvaluationBeginDate = monitorMethodEarliestDate;
                }


                // Override MatsEvaluationBeginDate if it is after the MATS compliance date.
                if ((matsRuleComplianceDate != null) && ((MpParameters.MatsEvaluationBeginDate == null) || (MpParameters.MatsEvaluationBeginDate > matsRuleComplianceDate)))
                {
                    MpParameters.MatsEvaluationBeginDate = matsRuleComplianceDate;
                }


                // Override MatsEvaluationBeginDate if it exists and is prior to the evaluation begin date.
                if ((MpParameters.MatsEvaluationBeginDate != null) && (MpParameters.MatsEvaluationBeginDate < MpParameters.EvaluationBeginDate))
                {
                    MpParameters.MatsEvaluationBeginDate = MpParameters.EvaluationBeginDate;
                }
            }
            catch (Exception ex)
            {
                returnVal = category.CheckEngine.FormatError(ex);
            }

            return returnVal;
        }

        /// <summary>
        /// MONPLAN-12: Initialize Check Engine Values
        /// </summary>
        /// <param name="category">The category object running the check.</param>
        /// <param name="log">obsolete</param>
        /// <returns>The error string if an error occurs.</returns>
        public static string MONPLAN12(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                MpParameters.EvaluationBeginDate = category.CheckEngine.EvalDefaultedBeganDate;
                MpParameters.EvaluationEndDate = category.CheckEngine.EvalDefaultedEndedDate;
                MpParameters.MaximumFutureDate = category.CheckEngine.MaximumFutureDate;
            }
            catch (Exception ex)
            {
                returnVal = category.CheckEngine.FormatError(ex);
            }

            return returnVal;
        }

        /// <summary>
        /// Initialize Program Lists
        /// 
        /// Initializes the program code list for programs that are ozone season programs
        /// </summary>
        /// <param name="category">The category object running the check.</param>
        /// <param name="log">obsolete</param>
        /// <returns></returns>
        public static string MONPLAN13(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
               MpParameters.ProgramIsOzoneSeasonList = "";

                foreach (ProgramCodeRow programCodeRow in MpParameters.ProgramCodeTable)
                {
                    if (programCodeRow.OsInd == 1)
                    {
                        MpParameters.ProgramIsOzoneSeasonList = MpParameters.ProgramIsOzoneSeasonList.ListAdd(programCodeRow.PrgCd);
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

    }
}
