using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.CheckEngine.Definitions;
using ECMPS.Checks.Data.Ecmps.Lookup.Table;
using ECMPS.Checks.EmGeneration.Parameters;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Extensions;


namespace ECMPS.Checks.NonOperatingEmissionGeneration
{
    public class cGenerationChecks : cChecks
    {

        #region Public Constructors

        /// <summary>
        /// Public Constructors
        /// </summary>
        /// <param name="generationProcess">The parent generation process object for the generation checks.</param>
        public cGenerationChecks(cGenerationProcess generationProcess)
          : base()
        {
            GenerationProcess = generationProcess;

            CheckProcedures = new dCheckProcedure[12];
            {
                CheckProcedures[1] = new dCheckProcedure(EMGEN1);
                CheckProcedures[2] = new dCheckProcedure(EMGEN2);
                CheckProcedures[3] = new dCheckProcedure(EMGEN3);
                CheckProcedures[4] = new dCheckProcedure(EMGEN4);
                CheckProcedures[5] = new dCheckProcedure(EMGEN5);
                CheckProcedures[6] = new dCheckProcedure(EMGEN6);
                CheckProcedures[7] = new dCheckProcedure(EMGEN7);
                CheckProcedures[8] = new dCheckProcedure(EMGEN8);
                CheckProcedures[9] = new dCheckProcedure(EMGEN9);
                CheckProcedures[10] = new dCheckProcedure(EMGEN10);
                CheckProcedures[11] = new dCheckProcedure(EMGEN11);
            }
        }

        #endregion


        #region Public Properties: General

        /// <summary>
        /// The Check Engine object of the parent check process.
        /// </summary>
        public cCheckEngine CheckEngine { get { return GenerationProcess.CheckEngine; } }

        /// <summary>
        /// The Generation Check Parameters for this category.
        /// </summary>
        public cGenerationParameters GenerationParameters { get { return GenerationProcess.GenerationParameters; } }

        /// <summary>
        /// THe Generation Process object for the category
        /// </summary>
        public cGenerationProcess GenerationProcess { get; private set; }

        /// <summary>
        /// The table containing the generated Hrly Op Data rows
        /// </summary>
        public DataTable HrlyOpDataTable { get { return GenerationProcess.HrlyOpDataTable; } }

        /// <summary>
        /// The Check Engine object of the parent check process.
        /// </summary>
        public cReportingPeriod ReportingPeriod { get { return CheckEngine.ReportingPeriod; } }

        /// <summary>
        /// The table containing the generated Summary Value rows
        /// </summary>
        public DataTable SummaryValueTable { get { return GenerationProcess.SummaryValueTable; } }

        #endregion


        #region Private Properties: Check Parameters

        private cCheckParameterDataRowViewValue GenBco2SummaryValueRecord { get { return GenerationParameters.GenBco2SummaryValueRecord; } }
        private cCheckParameterDateValue GenBeginDate { get { return GenerationParameters.GenBeginDate; } }
        private cCheckParameterIntegerValue GenBeginHour { get { return GenerationParameters.GenBeginHour; } }
        private cCheckParameterDataRowViewValue GenCo2mSummaryValueRecord { get { return GenerationParameters.GenCo2mSummaryValueRecord; } }
        private cCheckParameterDataRowViewValue GenHitSummaryValueRecord { get { return GenerationParameters.GenHitSummaryValueRecord; } }
        private cCheckParameterDataRowViewValue GenHourlyOpDataRecord { get { return GenerationParameters.GenHourlyOpDataRecord; } }
        private cCheckParameterDataRowViewValue GenNoxmSummaryValueRecord { get { return GenerationParameters.GenNoxmSummaryValueRecord; } }
        private cCheckParameterDataRowViewValue GenNoxrSummaryValueRecord { get { return GenerationParameters.GenNoxrSummaryValueRecord; } }
        private cCheckParameterDataRowViewValue GenOpHoursSummaryValueRecord { get { return GenerationParameters.GenOpHoursSummaryValueRecord; } }
        private cCheckParameterDataRowViewValue GenOpTimeSummaryValueRecord { get { return GenerationParameters.GenOpTimeSummaryValueRecord; } }
        private cCheckParameterBooleanValue GenOsReportingRequirement { get { return GenerationParameters.GenOsReportingRequirement; } }
        private cCheckParameterStringValue GenReportingFrequency { get { return GenerationParameters.GenReportingFrequency; } }
        private cCheckParameterDataRowViewValue GenSo2mSummaryValueRecord { get { return GenerationParameters.GenSo2mSummaryValueRecord; } }
        private cCheckParameterDataViewLegacy LocationProgramRecords { get { return GenerationParameters.LocationProgramRecords; } }
        private cCheckParameterDataViewLegacy LocationReportingFrequencyRecords { get { return GenerationParameters.LocationReportingFrequencyRecords; } }
        private cCheckParameterDataViewLegacy MpMethodRecords { get { return GenerationParameters.MpMethodRecords; } }
        private cCheckParameterDataViewLegacy OperatingSuppDataRecordsByLocation { get { return GenerationParameters.OperatingSuppDataRecordsByLocation; } }
        private cCheckParameterDataViewLegacy UnitStackConfigurationRecords { get { return GenerationParameters.UnitStackConfigurationRecords; } }

        #endregion


        #region Public Methods: Checks

        public string EMGEN1(cCategory category, ref bool log)
        // Initialize Emissions Generation for Location 
        {
            string resultValue = "";

            try
            {
                // Get Location Reporting Frequency for Reporting Period
                {
                    DataView reportingFrequencyView
                      = cRowFilter.FindRows(LocationReportingFrequencyRecords.Value,
                                            new cFilterCondition[] { new cFilterCondition("BEGIN_DATE",
                                                                                ReportingPeriod.BeganDate,
                                                                                eFilterDataType.DateBegan,
                                                                                eFilterConditionRelativeCompare.LessThanOrEqual),
                                                           new cFilterCondition("END_DATE",
                                                                                ReportingPeriod.EndedDate,
                                                                                eFilterDataType.DateEnded,
                                                                                eFilterConditionRelativeCompare.GreaterThanOrEqual) });

                    if ((reportingFrequencyView.Count == 1) && !reportingFrequencyView[0]["REPORT_FREQ_CD"].IsDbNull())
                    {
                        GenReportingFrequency.SetValue(reportingFrequencyView[0]["REPORT_FREQ_CD"].AsString(), category);
                    }
                    else
                    {
                        category.CheckCatalogResult = "A";
                        return resultValue;
                    }
                }

                // Determine OS Reporting Requirement
                {
                    DataView locationProgramView
                      = cRowFilter.FindRows(LocationProgramRecords.Value,
                                            new cFilterCondition[] { new cFilterCondition("PRG_CD",
                                                                                EmGenerationParameters.ProgramIsOzoneSeasonList,
                                                                                eFilterConditionStringCompare.InList),
                                                           new cFilterCondition("UNIT_MONITOR_CERT_BEGIN_DATE",
                                                                                new DateTime(ReportingPeriod.Year, 12, 31),
                                                                                eFilterDataType.DateBegan,
                                                                                eFilterConditionRelativeCompare.LessThanOrEqual),
                                                           new cFilterCondition("END_DATE",
                                                                                new DateTime(ReportingPeriod.Year, 1, 1),
                                                                                eFilterDataType.DateEnded,
                                                                                eFilterConditionRelativeCompare.GreaterThanOrEqual) });

                    if (locationProgramView.Count > 0)
                    {
                        GenOsReportingRequirement.SetValue(true, category);
                    }
                    else if (GenReportingFrequency.Value == "OS")
                    {
                        category.CheckCatalogResult = "B";
                        return resultValue;
                    }
                    else
                    {
                        GenOsReportingRequirement.SetValue(false, category);
                    }
                }

                // Set Gen Begin Date and Hour
                {
                    DataRowView earliestMonitorMethodRow
                      = cRowFilter.FindEarliestRow(MpMethodRecords.Value,
                                                   new cFilterCondition[] { new cFilterCondition("MON_LOC_ID", category.CurrentMonLocId) });

                    if (earliestMonitorMethodRow == null)
                    {
                        category.CheckCatalogResult = "C";
                        return resultValue;
                    }
                    else
                    {
                        DateTime earliestMethodBeginDate = earliestMonitorMethodRow["BEGIN_DATE"].AsDateTime(DateTime.MaxValue);
                        int earliestMethodBeginHour = earliestMonitorMethodRow["BEGIN_DATE"].AsInteger(0);

                        if (earliestMethodBeginDate > ReportingPeriod.EndedDate)
                        {
                            category.CheckCatalogResult = "C";
                            return resultValue;
                        }
                        else if ((GenReportingFrequency.Value == "OS") &&
                                 ((ReportingPeriod.Quarter == eQuarter.q1) ||
                                  (ReportingPeriod.Quarter == eQuarter.q4)))
                        {
                            category.CheckCatalogResult = "D";
                            return resultValue;
                        }
                        else if ((GenReportingFrequency.Value == "OS") &&
                                 (ReportingPeriod.Quarter == eQuarter.q2))
                        {
                            if (earliestMethodBeginDate < new DateTime(ReportingPeriod.Year, 5, 1))
                            {
                                GenBeginDate.SetValue(new DateTime(ReportingPeriod.Year, 5, 1), category);
                                GenBeginHour.SetValue(0, category);
                            }
                            else
                            {
                                GenBeginDate.SetValue(earliestMethodBeginDate, category);
                                GenBeginHour.SetValue(earliestMethodBeginHour, category);
                            }
                        }
                        else
                        {
                            if (earliestMethodBeginDate < ReportingPeriod.BeganDate)
                            {
                                GenBeginDate.SetValue(ReportingPeriod.BeganDate, category);
                                GenBeginHour.SetValue(0, category);
                            }
                            else
                            {
                                GenBeginDate.SetValue(earliestMethodBeginDate, category);
                                GenBeginHour.SetValue(earliestMethodBeginHour, category);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                resultValue = ex.FormatError();
            }

            return resultValue;
        }

        public string EMGEN2(cCategory category, ref bool log)
        // Generate Hourly Op Data Record 
        {
            string resultValue = "";

            try
            {
                GenHourlyOpDataRecord.SetValue(null, category);

                if ((category.CurrentOpDate > GenBeginDate.Value) ||
                    ((category.CurrentOpDate == GenBeginDate.Value) &&
                     (category.CurrentOpHour >= GenBeginHour.Value)))
                {
                    if (category.CurrentMonLocName.StartsWith("CP"))
                    {
                        DateTime opDateHour = category.CurrentOpDate.Date.AddHours(category.CurrentOpHour);

                        DataView monitorMethodView
                          = cRowFilter.FindRows(MpMethodRecords.Value,
                                                new cFilterCondition[] { new cFilterCondition("MON_LOC_ID",
                                                                                  category.CurrentMonLocId),
                                                             new cFilterCondition("PARAMETER_CD",
                                                                                  "HIT"),
                                                             new cFilterCondition("METHOD_CD",
                                                                                  "LTFF"),
                                                             new cFilterCondition("BEGIN_DATEHOUR",
                                                                                  opDateHour,
                                                                                  eFilterDataType.DateBegan,
                                                                                  eFilterConditionRelativeCompare.LessThanOrEqual),
                                                             new cFilterCondition("END_DATEHOUR",
                                                                                  opDateHour,
                                                                                  eFilterDataType.DateEnded,
                                                                                  eFilterConditionRelativeCompare.GreaterThanOrEqual) });



                        // Filter applied to MPMethodRecords also handles the LTFF check.
                        if ((monitorMethodView.Count > 0))
                            return resultValue; //Unfortunately the spec calls for an exit here instead falling out
                    }

                    DataRowView genHourlyOpDataRecord;
                    {
                        genHourlyOpDataRecord = HrlyOpDataTable.Clone().DefaultView.AddNew();
                        genHourlyOpDataRecord["MON_LOC_ID"] = category.CurrentMonLocId;
                        genHourlyOpDataRecord["RPT_PERIOD_ID"] = ReportingPeriod.RptPeriodId;
                        genHourlyOpDataRecord["BEGIN_DATE"] = category.CurrentOpDate;
                        genHourlyOpDataRecord["BEGIN_HOUR"] = category.CurrentOpHour;
                        genHourlyOpDataRecord["OP_TIME"] = 0;
                    }
                    GenHourlyOpDataRecord.SetValue(genHourlyOpDataRecord, category);
                }
            }
            catch (Exception ex)
            {
                resultValue = ex.FormatError();
            }

            return resultValue;
        }

        public string EMGEN3(cCategory category, ref bool log)
        // Generate CO2 Mass YTD Values 
        {
            string resultValue = "";

            try
            {
                GenCo2mSummaryValueRecord.SetValue(null, category);
                decimal ytdValue = 0;

                DataView monitorMethodView
                  = cRowFilter.FindRows(MpMethodRecords.Value,
                                        new cFilterCondition[] { new cFilterCondition("MON_LOC_ID",
                                                                              category.CurrentMonLocId),
                                                         new cFilterCondition("PARAMETER_CD",
                                                                              "CO2,CO2M",
                                                                              eFilterConditionStringCompare.InList),
                                                         new cFilterCondition("BEGIN_DATE",
                                                                              ReportingPeriod.EndedDate,
                                                                              eFilterDataType.DateBegan,
                                                                              eFilterConditionRelativeCompare.LessThanOrEqual),
                                                         new cFilterCondition("END_DATE",
                                                                              new DateTime(ReportingPeriod.Year, 1, 1),
                                                                              eFilterDataType.DateEnded,
                                                                              eFilterConditionRelativeCompare.GreaterThanOrEqual) });

                if (monitorMethodView.Count > 0)
                {
                    DateTime methodBeganDate = monitorMethodView[0]["BEGIN_DATE"].AsDateTime(DateTime.MinValue);

                    if (ReportingPeriod.Quarter > eQuarter.q1)
                    {
                        bool missingOpSuppData = false;

                        for (int checkQuarter = ReportingPeriod.Quarter.AsInteger() - 1; checkQuarter >= 1; checkQuarter--)
                        {
                            cReportingPeriod checkReportingPeriod = cReportingPeriod.GetReportingPeriod(ReportingPeriod.Year, (eQuarter)checkQuarter);

                            DataView operatingSuppDataView
                              = cRowFilter.FindRows(OperatingSuppDataRecordsByLocation.Value,
                                                    new cFilterCondition[] { new cFilterCondition("OP_TYPE_CD", "CO2M"),
                                                               new cFilterCondition("RPT_PERIOD_ID",
                                                                                    checkReportingPeriod.RptPeriodId,
                                                                                    eFilterDataType.Integer) });

                            if (operatingSuppDataView.Count == 0)
                            {
                                if (methodBeganDate <= checkReportingPeriod.EndedDate)
                                {
                                    category.CheckCatalogResult = "A";
                                    return resultValue;
                                }
                                else
                                    missingOpSuppData = true;
                            }
                            else
                            {
                                if (missingOpSuppData)
                                {
                                    category.CheckCatalogResult = "A";
                                    return resultValue;
                                }
                                else
                                    ytdValue += operatingSuppDataView[0]["OP_VALUE"].AsDecimal(0);
                            }
                        }
                    }

                    DataRowView genSummaryValueRecord;
                    {
                        genSummaryValueRecord = SummaryValueTable.Clone().DefaultView.AddNew();
                        genSummaryValueRecord["MON_LOC_ID"] = category.CurrentMonLocId;
                        genSummaryValueRecord["RPT_PERIOD_ID"] = ReportingPeriod.RptPeriodId;
                        genSummaryValueRecord["PARAMETER_CD"] = "CO2M";

                        DateTime? methodEndedDate = monitorMethodView[0]["END_DATE"].AsDateTime();

                        if (!methodEndedDate.HasValue || (methodEndedDate.Value >= ReportingPeriod.BeganDate))
                            genSummaryValueRecord["CURRENT_RPT_PERIOD_TOTAL"] = 0m;

                        genSummaryValueRecord["YEAR_TOTAL"] = ytdValue;
                    }
                    GenCo2mSummaryValueRecord.SetValue(genSummaryValueRecord, category);
                }
                else
                {
                    bool foundOpSuppData = false;

                    if (ReportingPeriod.Quarter > eQuarter.q1)
                    {
                        for (int checkQuarter = ReportingPeriod.Quarter.AsInteger() - 1; checkQuarter >= 1; checkQuarter--)
                        {
                            cReportingPeriod checkReportingPeriod = cReportingPeriod.GetReportingPeriod(ReportingPeriod.Year, (eQuarter)checkQuarter);

                            DataView operatingSuppDataView
                              = cRowFilter.FindRows(OperatingSuppDataRecordsByLocation.Value,
                                                    new cFilterCondition[] { new cFilterCondition("OP_TYPE_CD", "CO2M"),
                                                               new cFilterCondition("RPT_PERIOD_ID",
                                                                                    checkReportingPeriod.RptPeriodId,
                                                                                    eFilterDataType.Integer) });

                            if (operatingSuppDataView.Count == 0)
                            {
                                if (foundOpSuppData)
                                {
                                    category.CheckCatalogResult = "A";
                                    return resultValue;
                                }
                            }
                            else
                            {
                                foundOpSuppData = true;
                                ytdValue += operatingSuppDataView[0]["OP_VALUE"].AsDecimal(0);
                            }
                        }
                    }

                    if (foundOpSuppData)
                    {
                        DataRowView genSummaryValueRecord;
                        {
                            genSummaryValueRecord = SummaryValueTable.Clone().DefaultView.AddNew();
                            genSummaryValueRecord["MON_LOC_ID"] = category.CurrentMonLocId;
                            genSummaryValueRecord["RPT_PERIOD_ID"] = ReportingPeriod.RptPeriodId;
                            genSummaryValueRecord["PARAMETER_CD"] = "CO2M";
                            genSummaryValueRecord["CURRENT_RPT_PERIOD_TOTAL"] = 0m;
                            genSummaryValueRecord["YEAR_TOTAL"] = ytdValue;
                        }
                        GenCo2mSummaryValueRecord.SetValue(genSummaryValueRecord, category);
                    }
                }
            }
            catch (Exception ex)
            {
                resultValue = ex.FormatError();
            }

            return resultValue;
        }

        public string EMGEN4(cCategory category, ref bool log)
        // Generate SO2 Mass YTD Values 
        {
            string resultValue = "";

            try
            {
                GenSo2mSummaryValueRecord.SetValue(null, category);
                decimal ytdValue = 0;

                DataView monitorMethodView
                  = cRowFilter.FindRows(MpMethodRecords.Value,
                                        new cFilterCondition[] { new cFilterCondition("MON_LOC_ID",
                                                                              category.CurrentMonLocId),
                                                         new cFilterCondition("PARAMETER_CD",
                                                                              "SO2,SO2M",
                                                                              eFilterConditionStringCompare.InList),
                                                         new cFilterCondition("BEGIN_DATE",
                                                                              ReportingPeriod.EndedDate,
                                                                              eFilterDataType.DateBegan,
                                                                              eFilterConditionRelativeCompare.LessThanOrEqual),
                                                         new cFilterCondition("END_DATE",
                                                                              new DateTime(ReportingPeriod.Year, 1, 1),
                                                                              eFilterDataType.DateEnded,
                                                                              eFilterConditionRelativeCompare.GreaterThanOrEqual) });

                if (monitorMethodView.Count > 0)
                {
                    DateTime methodBeganDate = monitorMethodView[0]["BEGIN_DATE"].AsDateTime(DateTime.MinValue);

                    if (ReportingPeriod.Quarter > eQuarter.q1)
                    {
                        bool missingOpSuppData = false;

                        for (int checkQuarter = ReportingPeriod.Quarter.AsInteger() - 1; checkQuarter >= 1; checkQuarter--)
                        {
                            cReportingPeriod checkReportingPeriod = cReportingPeriod.GetReportingPeriod(ReportingPeriod.Year, (eQuarter)checkQuarter);

                            DataView operatingSuppDataView
                              = cRowFilter.FindRows(OperatingSuppDataRecordsByLocation.Value,
                                                    new cFilterCondition[] { new cFilterCondition("OP_TYPE_CD", "SO2M"),
                                                               new cFilterCondition("RPT_PERIOD_ID",
                                                                                    checkReportingPeriod.RptPeriodId,
                                                                                    eFilterDataType.Integer) });

                            if (operatingSuppDataView.Count == 0)
                            {
                                if (methodBeganDate <= checkReportingPeriod.EndedDate)
                                {
                                    category.CheckCatalogResult = "A";
                                    return resultValue;
                                }
                                else
                                    missingOpSuppData = true;
                            }
                            else
                            {
                                if (missingOpSuppData)
                                {
                                    category.CheckCatalogResult = "A";
                                    return resultValue;
                                }
                                else
                                    ytdValue += operatingSuppDataView[0]["OP_VALUE"].AsDecimal(0);
                            }
                        }
                    }

                    DataRowView genSummaryValueRecord;
                    {
                        genSummaryValueRecord = SummaryValueTable.Clone().DefaultView.AddNew();
                        genSummaryValueRecord["MON_LOC_ID"] = category.CurrentMonLocId;
                        genSummaryValueRecord["RPT_PERIOD_ID"] = ReportingPeriod.RptPeriodId;
                        genSummaryValueRecord["PARAMETER_CD"] = "SO2M";

                        DateTime? methodEndedDate = monitorMethodView[0]["END_DATE"].AsDateTime();

                        if (!methodEndedDate.HasValue || (methodEndedDate.Value >= ReportingPeriod.BeganDate))
                            genSummaryValueRecord["CURRENT_RPT_PERIOD_TOTAL"] = 0m;

                        genSummaryValueRecord["YEAR_TOTAL"] = ytdValue;
                    }
                    GenSo2mSummaryValueRecord.SetValue(genSummaryValueRecord, category);
                }
                else
                {
                    bool foundOpSuppData = false;

                    if (ReportingPeriod.Quarter > eQuarter.q1)
                    {
                        for (int checkQuarter = ReportingPeriod.Quarter.AsInteger() - 1; checkQuarter >= 1; checkQuarter--)
                        {
                            cReportingPeriod checkReportingPeriod = cReportingPeriod.GetReportingPeriod(ReportingPeriod.Year, (eQuarter)checkQuarter);

                            DataView operatingSuppDataView
                              = cRowFilter.FindRows(OperatingSuppDataRecordsByLocation.Value,
                                                    new cFilterCondition[] { new cFilterCondition("OP_TYPE_CD", "SO2M"),
                                                               new cFilterCondition("RPT_PERIOD_ID",
                                                                                    checkReportingPeriod.RptPeriodId,
                                                                                    eFilterDataType.Integer) });

                            if (operatingSuppDataView.Count == 0)
                            {
                                if (foundOpSuppData)
                                {
                                    category.CheckCatalogResult = "A";
                                    return resultValue;
                                }
                            }
                            else
                            {
                                foundOpSuppData = true;
                                ytdValue += operatingSuppDataView[0]["OP_VALUE"].AsDecimal(0);
                            }
                        }
                    }

                    if (foundOpSuppData)
                    {
                        DataRowView genSummaryValueRecord;
                        {
                            genSummaryValueRecord = SummaryValueTable.Clone().DefaultView.AddNew();
                            genSummaryValueRecord["MON_LOC_ID"] = category.CurrentMonLocId;
                            genSummaryValueRecord["RPT_PERIOD_ID"] = ReportingPeriod.RptPeriodId;
                            genSummaryValueRecord["PARAMETER_CD"] = "SO2M";
                            genSummaryValueRecord["CURRENT_RPT_PERIOD_TOTAL"] = 0m;
                            genSummaryValueRecord["YEAR_TOTAL"] = ytdValue;
                        }
                        GenSo2mSummaryValueRecord.SetValue(genSummaryValueRecord, category);
                    }
                }
            }
            catch (Exception ex)
            {
                resultValue = ex.FormatError();
            }

            return resultValue;
        }

        public string EMGEN5(cCategory category, ref bool log)
        // Generate NOx Mass YTD and OS Values 
        {
            string resultValue = "";

            try
            {
                GenNoxmSummaryValueRecord.SetValue(null, category);
                decimal ytdValue = 0;
                decimal osValue = 0;

                eQuarter startQuarter;
                {
                    if (GenReportingFrequency.Value == "Q")
                        startQuarter = eQuarter.q1;
                    else
                        startQuarter = eQuarter.q2;
                }

                DataView monitorMethodView
                  = cRowFilter.FindRows(MpMethodRecords.Value,
                                        new cFilterCondition[] { new cFilterCondition("MON_LOC_ID",
                                                                              category.CurrentMonLocId),
                                                         new cFilterCondition("PARAMETER_CD",
                                                                              "NOX,NOXM",
                                                                              eFilterConditionStringCompare.InList),
                                                         new cFilterCondition("BEGIN_DATE",
                                                                              ReportingPeriod.EndedDate,
                                                                              eFilterDataType.DateBegan,
                                                                              eFilterConditionRelativeCompare.LessThanOrEqual),
                                                         new cFilterCondition("END_DATE",
                                                                              new DateTime(ReportingPeriod.Year, 3 * (startQuarter.AsInteger() - 1) + 1, 1),
                                                                              eFilterDataType.DateEnded,
                                                                              eFilterConditionRelativeCompare.GreaterThanOrEqual) });

                if (monitorMethodView.Count > 0)
                {
                    DateTime methodBeganDate = monitorMethodView[0]["BEGIN_DATE"].AsDateTime(DateTime.MinValue);

                    if (ReportingPeriod.Quarter > startQuarter)
                    {
                        bool missingOpSuppData = false;

                        for (int checkQuarter = ReportingPeriod.Quarter.AsInteger() - 1; checkQuarter >= startQuarter.AsInteger(); checkQuarter--)
                        {
                            cReportingPeriod checkReportingPeriod = cReportingPeriod.GetReportingPeriod(ReportingPeriod.Year, (eQuarter)checkQuarter);

                            if ((GenReportingFrequency.Value == "Q") || (checkReportingPeriod.Quarter == eQuarter.q3))
                            {
                                DataView operatingSuppDataView
                                  = cRowFilter.FindRows(OperatingSuppDataRecordsByLocation.Value,
                                                        new cFilterCondition[] { new cFilterCondition("OP_TYPE_CD", "NOXM"),
                                                                 new cFilterCondition("RPT_PERIOD_ID",
                                                                                      checkReportingPeriod.RptPeriodId,
                                                                                      eFilterDataType.Integer) });

                                if (operatingSuppDataView.Count == 0)
                                {
                                    if (methodBeganDate <= checkReportingPeriod.EndedDate)
                                    {
                                        category.CheckCatalogResult = "A";
                                        return resultValue;
                                    }
                                    else
                                        missingOpSuppData = true;
                                }
                                else
                                {
                                    if (missingOpSuppData)
                                    {
                                        category.CheckCatalogResult = "A";
                                        return resultValue;
                                    }
                                    else
                                    {
                                        if (GenReportingFrequency.Value == "Q")
                                            ytdValue += operatingSuppDataView[0]["OP_VALUE"].AsDecimal(0);

                                        if (GenOsReportingRequirement.Value.Default(false) && (checkReportingPeriod.Quarter == eQuarter.q3))
                                            osValue += operatingSuppDataView[0]["OP_VALUE"].AsDecimal(0);
                                    }
                                }
                            }

                            if (GenOsReportingRequirement.Value.Default(false) && (checkReportingPeriod.Quarter == eQuarter.q2))
                            {
                                DataView operatingSuppDataView
                                  = cRowFilter.FindRows(OperatingSuppDataRecordsByLocation.Value,
                                                        new cFilterCondition[] { new cFilterCondition("OP_TYPE_CD", "NOXMOS"),
                                                                 new cFilterCondition("RPT_PERIOD_ID",
                                                                                      checkReportingPeriod.RptPeriodId, //Should be the Q2 Id
                                                                                      eFilterDataType.Integer) });

                                if (operatingSuppDataView.Count == 0)
                                {
                                    if (methodBeganDate <= checkReportingPeriod.EndedDate)
                                    {
                                        category.CheckCatalogResult = "A";
                                        return resultValue;
                                    }
                                    else
                                        missingOpSuppData = true;
                                }
                                else
                                {
                                    if (missingOpSuppData)
                                    {
                                        category.CheckCatalogResult = "A";
                                        return resultValue;
                                    }
                                    else
                                    {
                                        osValue += operatingSuppDataView[0]["OP_VALUE"].AsDecimal(0);
                                    }
                                }
                            }
                        }
                    }

                    DataRowView genSummaryValueRecord;
                    {
                        genSummaryValueRecord = SummaryValueTable.Clone().DefaultView.AddNew();
                        genSummaryValueRecord["MON_LOC_ID"] = category.CurrentMonLocId;
                        genSummaryValueRecord["RPT_PERIOD_ID"] = ReportingPeriod.RptPeriodId;
                        genSummaryValueRecord["PARAMETER_CD"] = "NOXM";

                        DateTime? methodEndedDate = monitorMethodView[0]["END_DATE"].AsDateTime();

                        if (!methodEndedDate.HasValue || (methodEndedDate.Value >= ReportingPeriod.BeganDate))
                            genSummaryValueRecord["CURRENT_RPT_PERIOD_TOTAL"] = 0m;

                        if (GenReportingFrequency.Value == "Q")
                            genSummaryValueRecord["YEAR_TOTAL"] = ytdValue;

                        if (GenOsReportingRequirement.Value.Default(false))
                            genSummaryValueRecord["OS_TOTAL"] = osValue;
                    }
                    GenNoxmSummaryValueRecord.SetValue(genSummaryValueRecord, category);
                }
                else
                {
                    bool foundOpSuppData = false;

                    if (ReportingPeriod.Quarter > startQuarter)
                    {
                        for (int checkQuarter = ReportingPeriod.Quarter.AsInteger() - 1; checkQuarter >= startQuarter.AsInteger(); checkQuarter--)
                        {
                            cReportingPeriod checkReportingPeriod = cReportingPeriod.GetReportingPeriod(ReportingPeriod.Year, (eQuarter)checkQuarter);

                            if ((GenReportingFrequency.Value == "Q") || (checkReportingPeriod.Quarter == eQuarter.q3))
                            {
                                DataView operatingSuppDataView
                                  = cRowFilter.FindRows(OperatingSuppDataRecordsByLocation.Value,
                                                        new cFilterCondition[] { new cFilterCondition("OP_TYPE_CD", "NOXM"),
                                                                 new cFilterCondition("RPT_PERIOD_ID",
                                                                                      checkReportingPeriod.RptPeriodId,
                                                                                      eFilterDataType.Integer) });

                                if (operatingSuppDataView.Count == 0)
                                {
                                    if (foundOpSuppData)
                                    {
                                        category.CheckCatalogResult = "A";
                                        return resultValue;
                                    }
                                }
                                else
                                {
                                    foundOpSuppData = true;

                                    if (GenReportingFrequency.Value == "Q")
                                        ytdValue += operatingSuppDataView[0]["OP_VALUE"].AsDecimal(0);

                                    if (GenOsReportingRequirement.Value.Default(false) && (checkReportingPeriod.Quarter == eQuarter.q3))
                                        osValue += operatingSuppDataView[0]["OP_VALUE"].AsDecimal(0);
                                }
                            }

                            if (GenOsReportingRequirement.Value.Default(false) && (checkReportingPeriod.Quarter == eQuarter.q2))
                            {
                                DataView operatingSuppDataView
                                  = cRowFilter.FindRows(OperatingSuppDataRecordsByLocation.Value,
                                                        new cFilterCondition[] { new cFilterCondition("OP_TYPE_CD", "NOXMOS"),
                                                                 new cFilterCondition("RPT_PERIOD_ID",
                                                                                      checkReportingPeriod.RptPeriodId, //Should be the Q2 Id
                                                                                      eFilterDataType.Integer) });

                                if (operatingSuppDataView.Count == 0)
                                {
                                    if (foundOpSuppData)
                                    {
                                        category.CheckCatalogResult = "A";
                                        return resultValue;
                                    }
                                }
                                else
                                {
                                    foundOpSuppData = true;
                                    osValue += operatingSuppDataView[0]["OP_VALUE"].AsDecimal(0);
                                }
                            }
                        }
                    }

                    if (foundOpSuppData)
                    {
                        DataRowView genSummaryValueRecord;
                        {
                            genSummaryValueRecord = SummaryValueTable.Clone().DefaultView.AddNew();
                            genSummaryValueRecord["MON_LOC_ID"] = category.CurrentMonLocId;
                            genSummaryValueRecord["RPT_PERIOD_ID"] = ReportingPeriod.RptPeriodId;
                            genSummaryValueRecord["PARAMETER_CD"] = "NOXM";
                            genSummaryValueRecord["CURRENT_RPT_PERIOD_TOTAL"] = 0m;

                            if (GenReportingFrequency.Value == "Q")
                                genSummaryValueRecord["YEAR_TOTAL"] = ytdValue;

                            if (GenOsReportingRequirement.Value.Default(false))
                                genSummaryValueRecord["OS_TOTAL"] = osValue;
                        }
                        GenNoxmSummaryValueRecord.SetValue(genSummaryValueRecord, category);
                    }
                }
            }
            catch (Exception ex)
            {
                resultValue = ex.FormatError();
            }

            return resultValue;
        }

        public string EMGEN6(cCategory category, ref bool log)
        // Generate NOx Rate YTD Values 
        {
            string resultValue = "";

            try
            {
                GenNoxrSummaryValueRecord.SetValue(null, category);
                decimal ytdValue = 0;
                decimal totalOpHrs = 0;

                bool arpProgramFound = false;
                {
                    DataView locationProgramView
                      = cRowFilter.FindRows(LocationProgramRecords.Value,
                                            new cFilterCondition[] { new cFilterCondition("PRG_CD",
                                                                                "ARP"),
                                                           new cFilterCondition("CLASS",
                                                                                "NA",
                                                                                true),
                                                           new cFilterCondition("END_DATE",
                                                                                new DateTime(ReportingPeriod.Year, 1, 1),
                                                                                eFilterDataType.DateEnded,
                                                                                eFilterConditionRelativeCompare.GreaterThanOrEqual) });

                    foreach (DataRowView locationProgramRow in locationProgramView)
                    {
                        DateTime? erbDate = locationProgramRow["EMISSIONS_RECORDING_BEGIN_DATE"].AsDateTime();
                        DateTime? umcbDate = locationProgramRow["UNIT_MONITOR_CERT_BEGIN_DATE"].AsDateTime();

                        if ((erbDate.HasValue && (erbDate.Value <= ReportingPeriod.EndedDate)) ||
                            (!erbDate.HasValue && umcbDate.HasValue && (umcbDate.Value <= ReportingPeriod.EndedDate)))
                        {
                            arpProgramFound = true;
                            break;
                        }
                    }
                }

                if (arpProgramFound)
                {
                    DataView monitorMethodView;
                    {
                        monitorMethodView
                          = cRowFilter.FindRows(MpMethodRecords.Value,
                                                new cFilterCondition[] { new cFilterCondition("MON_LOC_ID",
                                                                                  category.CurrentMonLocId),
                                                             new cFilterCondition("PARAMETER_CD",
                                                                                  "NOXR"),
                                                             new cFilterCondition("BEGIN_DATE",
                                                                                  ReportingPeriod.EndedDate,
                                                                                  eFilterDataType.DateBegan,
                                                                                  eFilterConditionRelativeCompare.LessThanOrEqual),
                                                             new cFilterCondition("END_DATE",
                                                                                  new DateTime(ReportingPeriod.Year, 1, 1),
                                                                                  eFilterDataType.DateEnded,
                                                                                  eFilterConditionRelativeCompare.GreaterThanOrEqual) });

                        if ((monitorMethodView.Count == 0) && !category.CurrentMonLocName.PadRight(2).Substring(0, 2).InList("CS,CP,MS,MP"))
                        {
                            monitorMethodView
                              = cRowFilter.FindRows(MpMethodRecords.Value,
                                                    new cFilterCondition[] { new cFilterCondition("MON_LOC_ID",
                                                                                  category.CurrentMonLocId),
                                                               new cFilterCondition("PARAMETER_CD",
                                                                                    "NOXM"),
                                                               new cFilterCondition("METHOD_CD",
                                                                                    "LME"),
                                                               new cFilterCondition("BEGIN_DATE",
                                                                                    ReportingPeriod.EndedDate,
                                                                                    eFilterDataType.DateBegan,
                                                                                    eFilterConditionRelativeCompare.LessThanOrEqual),
                                                               new cFilterCondition("END_DATE",
                                                                                    new DateTime(ReportingPeriod.Year, 1, 1),
                                                                                    eFilterDataType.DateEnded,
                                                                                    eFilterConditionRelativeCompare.GreaterThanOrEqual) });

                            if (monitorMethodView.Count == 0)
                            {
                                /* 
                                 * NOXR summary is required at a unit for ARP when NOXR is reported at a MS in a MS configuration.
                                 * The current confiuration is a MS configuration if the current unit only connects to MS locations.
                                 */
                                int nonMsUnitStackConfigurationCount
                                  = cRowFilter.CountRows(UnitStackConfigurationRecords.Value,
                                                        new cFilterCondition[] { new cFilterCondition("MON_LOC_ID", category.CurrentMonLocId),
                                                               new cFilterCondition("STACK_NAME", "MS", eFilterConditionStringCompare.BeginsWith, true) });

                                if (nonMsUnitStackConfigurationCount == 0)
                                {

                                    DataView msUnitStackConfigurationView
                                      = cRowFilter.FindRows(UnitStackConfigurationRecords.Value,
                                                            new cFilterCondition[] { new cFilterCondition("MON_LOC_ID",
                                                                                    category.CurrentMonLocId),
                                                               new cFilterCondition("STACK_NAME",
                                                                                    "MS",
                                                                                    eFilterConditionStringCompare.BeginsWith) });

                                    if (msUnitStackConfigurationView.Count > 0)
                                    {
                                        string msMonLocIdList = msUnitStackConfigurationView.DistinctValues("STACK_PIPE_MON_LOC_ID").DelimitedList();

                                        monitorMethodView
                                          = cRowFilter.FindRows(MpMethodRecords.Value,
                                                                new cFilterCondition[] { new cFilterCondition("MON_LOC_ID",
                                                                                      msMonLocIdList,
                                                                                      eFilterConditionStringCompare.InList),
                                                                 new cFilterCondition("PARAMETER_CD",
                                                                                      "NOXR"),
                                                                 new cFilterCondition("BEGIN_DATE",
                                                                                      ReportingPeriod.EndedDate,
                                                                                      eFilterDataType.DateBegan,
                                                                                      eFilterConditionRelativeCompare.LessThanOrEqual),
                                                                 new cFilterCondition("END_DATE",
                                                                                      new DateTime(ReportingPeriod.Year, 1, 1),
                                                                                      eFilterDataType.DateEnded,
                                                                                      eFilterConditionRelativeCompare.GreaterThanOrEqual) });
                                    }
                                }
                            }
                        }
                    }

                    if (monitorMethodView.Count > 0)
                    {
                        DateTime methodBeganDate = monitorMethodView[0]["BEGIN_DATE"].AsDateTime(DateTime.MinValue);

                        if (ReportingPeriod.Quarter == eQuarter.q1)
                        {
                            DataRowView genSummaryValueRecord;
                            {
                                genSummaryValueRecord = SummaryValueTable.Clone().DefaultView.AddNew();
                                genSummaryValueRecord["MON_LOC_ID"] = category.CurrentMonLocId;
                                genSummaryValueRecord["RPT_PERIOD_ID"] = ReportingPeriod.RptPeriodId;
                                genSummaryValueRecord["PARAMETER_CD"] = "NOXR";
                                genSummaryValueRecord["CURRENT_RPT_PERIOD_TOTAL"] = 0m;
                                genSummaryValueRecord["YEAR_TOTAL"] = 0m;
                            }
                            GenNoxrSummaryValueRecord.SetValue(genSummaryValueRecord, category);
                        }
                        else
                        {
                            cReportingPeriod priorReportingPeriod = cReportingPeriod.GetReportingPeriod(ReportingPeriod.Year,
                                                                                                        (eQuarter)(ReportingPeriod.Quarter.AsInteger() - 1));

                            DataView priorOperatingSuppDataView
                              = cRowFilter.FindRows(OperatingSuppDataRecordsByLocation.Value,
                                                    new cFilterCondition[] { new cFilterCondition("OP_TYPE_CD", "NOXRYTD"),
                                                               new cFilterCondition("RPT_PERIOD_ID",
                                                                                    priorReportingPeriod.RptPeriodId,
                                                                                    eFilterDataType.Integer) });


                            if (priorOperatingSuppDataView.Count > 0)
                            {
                                DataRowView genSummaryValueRecord;
                                {
                                    genSummaryValueRecord = SummaryValueTable.Clone().DefaultView.AddNew();
                                    genSummaryValueRecord["MON_LOC_ID"] = category.CurrentMonLocId;
                                    genSummaryValueRecord["RPT_PERIOD_ID"] = ReportingPeriod.RptPeriodId;
                                    genSummaryValueRecord["PARAMETER_CD"] = "NOXR";

                                    DateTime? methodEndedDate = monitorMethodView[0]["END_DATE"].AsDateTime();

                                    if (!methodEndedDate.HasValue || (methodEndedDate.Value >= ReportingPeriod.BeganDate))
                                        genSummaryValueRecord["CURRENT_RPT_PERIOD_TOTAL"] = 0m;

                                    genSummaryValueRecord["YEAR_TOTAL"] = priorOperatingSuppDataView[0]["OP_VALUE"].AsDecimal(0);
                                }
                                GenNoxrSummaryValueRecord.SetValue(genSummaryValueRecord, category);
                            }
                            else
                            {
                                bool missingOpSuppData = false;

                                for (int checkQuarter = ReportingPeriod.Quarter.AsInteger() - 1; checkQuarter >= 1; checkQuarter--)
                                {
                                    cReportingPeriod checkReportingPeriod = cReportingPeriod.GetReportingPeriod(ReportingPeriod.Year, (eQuarter)checkQuarter);

                                    DataView checkNoxrOperatingSuppDataView
                                      = cRowFilter.FindRows(OperatingSuppDataRecordsByLocation.Value,
                                                            new cFilterCondition[] { new cFilterCondition("OP_TYPE_CD", "NOXR"),
                                                                   new cFilterCondition("RPT_PERIOD_ID",
                                                                                        checkReportingPeriod.RptPeriodId,
                                                                                        eFilterDataType.Integer) });

                                    if (checkNoxrOperatingSuppDataView.Count == 0)
                                    {
                                        if (methodBeganDate <= checkReportingPeriod.EndedDate)
                                        {
                                            category.CheckCatalogResult = "A";
                                            return resultValue;
                                        }
                                        else
                                            missingOpSuppData = true;
                                    }
                                    else
                                    {
                                        if (missingOpSuppData)
                                        {
                                            category.CheckCatalogResult = "A";
                                            return resultValue;
                                        }
                                        else
                                        {
                                            decimal noxValue = checkNoxrOperatingSuppDataView[0]["OP_VALUE"].AsDecimal(0);

                                            DataView checkOpHoursOperatingSuppDataView
                                              = cRowFilter.FindRows(OperatingSuppDataRecordsByLocation.Value,
                                                                    new cFilterCondition[] { new cFilterCondition("OP_TYPE_CD", "OPHOURS"),
                                                                       new cFilterCondition("RPT_PERIOD_ID",
                                                                                            checkReportingPeriod.RptPeriodId,
                                                                                            eFilterDataType.Integer) });

                                            if (checkOpHoursOperatingSuppDataView.Count == 0)
                                            {
                                                category.CheckCatalogResult = "B";
                                                return resultValue;
                                            }
                                            else
                                            {
                                                decimal opHours = checkOpHoursOperatingSuppDataView[0]["OP_VALUE"].AsDecimal(0);
                                                totalOpHrs += opHours;
                                                ytdValue += noxValue * opHours;
                                            }
                                        }
                                    }
                                }


                                GenNoxrSummaryValueRecord.SetValue(SummaryValueTable.Clone().DefaultView.AddNew(), category);
                                {
                                    GenNoxrSummaryValueRecord.Value["MON_LOC_ID"] = category.CurrentMonLocId;
                                    GenNoxrSummaryValueRecord.Value["RPT_PERIOD_ID"] = ReportingPeriod.RptPeriodId;
                                    GenNoxrSummaryValueRecord.Value["PARAMETER_CD"] = "NOXR";

                                    DateTime? methodEndedDate = monitorMethodView[0]["END_DATE"].AsDateTime();

                                    if (!methodEndedDate.HasValue || (methodEndedDate.Value >= ReportingPeriod.BeganDate))
                                        GenNoxrSummaryValueRecord.Value["CURRENT_RPT_PERIOD_TOTAL"] = 0m;

                                    if (totalOpHrs == 0)
                                        GenNoxrSummaryValueRecord.Value["YEAR_TOTAL"] = 0m;
                                    else
                                    {
                                        GenNoxrSummaryValueRecord.Value["YEAR_TOTAL"] = Math.Round(ytdValue / totalOpHrs, 3, MidpointRounding.AwayFromZero);
                                        category.CheckCatalogResult = "C";
                                        return resultValue;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (ReportingPeriod.Quarter > eQuarter.q1)
                        {
                            cReportingPeriod priorReportingPeriod = cReportingPeriod.GetReportingPeriod(ReportingPeriod.Year,
                                                                                                        (eQuarter)(ReportingPeriod.Quarter.AsInteger() - 1));

                            DataView priorOperatingSuppDataView
                              = cRowFilter.FindRows(OperatingSuppDataRecordsByLocation.Value,
                                                    new cFilterCondition[] { new cFilterCondition("OP_TYPE_CD", "NOXRYTD"),
                                                                 new cFilterCondition("RPT_PERIOD_ID",
                                                                                      priorReportingPeriod.RptPeriodId,
                                                                                      eFilterDataType.Integer) });


                            if (priorOperatingSuppDataView.Count > 0)
                            {
                                DataRowView genSummaryValueRecord;
                                {
                                    genSummaryValueRecord = SummaryValueTable.Clone().DefaultView.AddNew();
                                    genSummaryValueRecord["MON_LOC_ID"] = category.CurrentMonLocId;
                                    genSummaryValueRecord["RPT_PERIOD_ID"] = ReportingPeriod.RptPeriodId;
                                    genSummaryValueRecord["PARAMETER_CD"] = "NOXR";
                                    genSummaryValueRecord["CURRENT_RPT_PERIOD_TOTAL"] = 0m;
                                    genSummaryValueRecord["YEAR_TOTAL"] = priorOperatingSuppDataView[0]["OP_VALUE"].AsDecimal().DbValue();
                                }
                                GenNoxrSummaryValueRecord.SetValue(genSummaryValueRecord, category);
                            }
                            else
                            {
                                bool foundOpSuppData = false;

                                for (int checkQuarter = ReportingPeriod.Quarter.AsInteger() - 1; checkQuarter >= 1; checkQuarter--)
                                {
                                    cReportingPeriod checkReportingPeriod = cReportingPeriod.GetReportingPeriod(ReportingPeriod.Year, (eQuarter)checkQuarter);

                                    DataView checkNoxrOperatingSuppDataView
                                      = cRowFilter.FindRows(OperatingSuppDataRecordsByLocation.Value,
                                                            new cFilterCondition[] { new cFilterCondition("OP_TYPE_CD", "NOXR"),
                                                                   new cFilterCondition("RPT_PERIOD_ID",
                                                                                        checkReportingPeriod.RptPeriodId,
                                                                                        eFilterDataType.Integer) });

                                    if (checkNoxrOperatingSuppDataView.Count == 0)
                                    {
                                        if (foundOpSuppData)
                                        {
                                            category.CheckCatalogResult = "A";
                                            return resultValue;
                                        }
                                    }
                                    else
                                    {
                                        foundOpSuppData = true;

                                        decimal noxValue = checkNoxrOperatingSuppDataView[0]["OP_VALUE"].AsDecimal(0);

                                        DataView checkOpHoursOperatingSuppDataView
                                          = cRowFilter.FindRows(OperatingSuppDataRecordsByLocation.Value,
                                                                new cFilterCondition[] { new cFilterCondition("OP_TYPE_CD", "OPHOURS"),
                                                                       new cFilterCondition("RPT_PERIOD_ID",
                                                                                            checkReportingPeriod.RptPeriodId,
                                                                                            eFilterDataType.Integer) });

                                        if (checkOpHoursOperatingSuppDataView.Count == 0)
                                        {
                                            category.CheckCatalogResult = "B";
                                            return resultValue;
                                        }
                                        else
                                        {
                                            decimal opHours = checkOpHoursOperatingSuppDataView[0]["OP_VALUE"].AsDecimal(0);
                                            totalOpHrs += opHours;
                                            ytdValue += noxValue * opHours;
                                        }
                                    }

                                    if (foundOpSuppData)
                                    {
                                        GenNoxrSummaryValueRecord.SetValue(SummaryValueTable.Clone().DefaultView.AddNew(), category);
                                        {
                                            GenNoxrSummaryValueRecord.Value["MON_LOC_ID"] = category.CurrentMonLocId;
                                            GenNoxrSummaryValueRecord.Value["RPT_PERIOD_ID"] = ReportingPeriod.RptPeriodId;
                                            GenNoxrSummaryValueRecord.Value["PARAMETER_CD"] = "NOXR";
                                            GenNoxrSummaryValueRecord.Value["CURRENT_RPT_PERIOD_TOTAL"] = 0m;

                                            if (totalOpHrs == 0)
                                                GenNoxrSummaryValueRecord.Value["YEAR_TOTAL"] = 0m;
                                            else
                                            {
                                                GenNoxrSummaryValueRecord.Value["YEAR_TOTAL"] = Math.Round(ytdValue / totalOpHrs, 3, MidpointRounding.AwayFromZero);
                                                category.CheckCatalogResult = "C";
                                                return resultValue;
                                            }
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
                resultValue = ex.FormatError();
            }

            return resultValue;
        }

        public string EMGEN7(cCategory category, ref bool log)
        // Generate Total Heat Input YTD and OS Values 
        {
            string resultValue = "";

            try
            {
                GenHitSummaryValueRecord.SetValue(null, category);
                decimal ytdValue = 0;
                decimal osValue = 0;

                eQuarter startQuarter;
                {
                    if (GenReportingFrequency.Value == "Q")
                        startQuarter = eQuarter.q1;
                    else
                        startQuarter = eQuarter.q2;
                }

                DataView monitorMethodView
                  = cRowFilter.FindRows(MpMethodRecords.Value,
                                        new cFilterCondition[] { new cFilterCondition("MON_LOC_ID",
                                                                              category.CurrentMonLocId),
                                                         new cFilterCondition("PARAMETER_CD",
                                                                              "HI,HIT",
                                                                              eFilterConditionStringCompare.InList),
                                                         new cFilterCondition("METHOD_CD",
                                                                              "EXP",
                                                                              true),
                                                         new cFilterCondition("BEGIN_DATE",
                                                                              ReportingPeriod.EndedDate,
                                                                              eFilterDataType.DateBegan,
                                                                              eFilterConditionRelativeCompare.LessThanOrEqual),
                                                         new cFilterCondition("END_DATE",
                                                                              new DateTime(ReportingPeriod.Year, 3 * (startQuarter.AsInteger() - 1) + 1, 1),
                                                                              eFilterDataType.DateEnded,
                                                                              eFilterConditionRelativeCompare.GreaterThanOrEqual) });

                if (monitorMethodView.Count > 0)
                {
                    DateTime methodBeganDate = monitorMethodView[0]["BEGIN_DATE"].AsDateTime(DateTime.MinValue);

                    if (ReportingPeriod.Quarter > startQuarter)
                    {
                        bool missingOpSuppData = false;

                        for (int checkQuarter = ReportingPeriod.Quarter.AsInteger() - 1; checkQuarter >= startQuarter.AsInteger(); checkQuarter--)
                        {
                            cReportingPeriod checkReportingPeriod = cReportingPeriod.GetReportingPeriod(ReportingPeriod.Year, (eQuarter)checkQuarter);

                            if ((GenReportingFrequency.Value == "Q") || (checkReportingPeriod.Quarter == eQuarter.q3))
                            {
                                DataView operatingSuppDataView
                                  = cRowFilter.FindRows(OperatingSuppDataRecordsByLocation.Value,
                                                        new cFilterCondition[] { new cFilterCondition("OP_TYPE_CD", "HIT"),
                                                                 new cFilterCondition("RPT_PERIOD_ID",
                                                                                      checkReportingPeriod.RptPeriodId,
                                                                                      eFilterDataType.Integer) });

                                if (operatingSuppDataView.Count == 0)
                                {
                                    if (methodBeganDate <= checkReportingPeriod.EndedDate)
                                    {
                                        category.CheckCatalogResult = "A";
                                        return resultValue;
                                    }
                                    else
                                        missingOpSuppData = true;
                                }
                                else
                                {
                                    if (missingOpSuppData)
                                    {
                                        category.CheckCatalogResult = "A";
                                        return resultValue;
                                    }
                                    else
                                    {
                                        if (GenReportingFrequency.Value == "Q")
                                            ytdValue += operatingSuppDataView[0]["OP_VALUE"].AsDecimal(0);

                                        if (GenOsReportingRequirement.Value.Default(false) && (checkReportingPeriod.Quarter == eQuarter.q3))
                                            osValue += operatingSuppDataView[0]["OP_VALUE"].AsDecimal(0);
                                    }
                                }
                            }

                            if (GenOsReportingRequirement.Value.Default(false) && (checkReportingPeriod.Quarter == eQuarter.q2))
                            {
                                DataView operatingSuppDataView
                                  = cRowFilter.FindRows(OperatingSuppDataRecordsByLocation.Value,
                                                        new cFilterCondition[] { new cFilterCondition("OP_TYPE_CD", "HITOS"),
                                                                 new cFilterCondition("RPT_PERIOD_ID",
                                                                                      checkReportingPeriod.RptPeriodId, //Should be the Q2 Id
                                                                                      eFilterDataType.Integer) });

                                if (operatingSuppDataView.Count == 0)
                                {
                                    if (methodBeganDate <= checkReportingPeriod.EndedDate)
                                    {
                                        category.CheckCatalogResult = "A";
                                        return resultValue;
                                    }
                                    else
                                        missingOpSuppData = true;
                                }
                                else
                                {
                                    if (missingOpSuppData)
                                    {
                                        category.CheckCatalogResult = "A";
                                        return resultValue;
                                    }
                                    else
                                    {
                                        osValue += operatingSuppDataView[0]["OP_VALUE"].AsDecimal(0);
                                    }
                                }
                            }
                        }
                    }

                    DataRowView genSummaryValueRecord;
                    {
                        genSummaryValueRecord = SummaryValueTable.Clone().DefaultView.AddNew();
                        genSummaryValueRecord["MON_LOC_ID"] = category.CurrentMonLocId;
                        genSummaryValueRecord["RPT_PERIOD_ID"] = ReportingPeriod.RptPeriodId;
                        genSummaryValueRecord["PARAMETER_CD"] = "HIT";

                        DateTime? methodEndedDate = monitorMethodView[0]["END_DATE"].AsDateTime();

                        if (!methodEndedDate.HasValue || (methodEndedDate.Value >= ReportingPeriod.BeganDate))
                            genSummaryValueRecord["CURRENT_RPT_PERIOD_TOTAL"] = 0m;

                        if (GenReportingFrequency.Value == "Q")
                            genSummaryValueRecord["YEAR_TOTAL"] = ytdValue;

                        if (GenOsReportingRequirement.Value.Default(false))
                            genSummaryValueRecord["OS_TOTAL"] = osValue;
                    }
                    GenHitSummaryValueRecord.SetValue(genSummaryValueRecord, category);
                }
                else
                {
                    bool foundOpSuppData = false;

                    if (ReportingPeriod.Quarter > startQuarter)
                    {
                        for (int checkQuarter = ReportingPeriod.Quarter.AsInteger() - 1; checkQuarter >= startQuarter.AsInteger(); checkQuarter--)
                        {
                            cReportingPeriod checkReportingPeriod = cReportingPeriod.GetReportingPeriod(ReportingPeriod.Year, (eQuarter)checkQuarter);

                            if ((GenReportingFrequency.Value == "Q") || (checkReportingPeriod.Quarter == eQuarter.q3))
                            {
                                DataView operatingSuppDataView
                                  = cRowFilter.FindRows(OperatingSuppDataRecordsByLocation.Value,
                                                        new cFilterCondition[] { new cFilterCondition("OP_TYPE_CD", "HIT"),
                                                                 new cFilterCondition("RPT_PERIOD_ID",
                                                                                      checkReportingPeriod.RptPeriodId,
                                                                                      eFilterDataType.Integer) });

                                if (operatingSuppDataView.Count == 0)
                                {
                                    if (foundOpSuppData)
                                    {
                                        category.CheckCatalogResult = "A";
                                        return resultValue;
                                    }
                                }
                                else
                                {
                                    foundOpSuppData = true;

                                    if (GenReportingFrequency.Value == "Q")
                                        ytdValue += operatingSuppDataView[0]["OP_VALUE"].AsDecimal(0);

                                    if (GenOsReportingRequirement.Value.Default(false) && (checkReportingPeriod.Quarter == eQuarter.q3))
                                        osValue += operatingSuppDataView[0]["OP_VALUE"].AsDecimal(0);
                                }
                            }

                            if (GenOsReportingRequirement.Value.Default(false) && (checkReportingPeriod.Quarter == eQuarter.q2))
                            {
                                DataView operatingSuppDataView
                                  = cRowFilter.FindRows(OperatingSuppDataRecordsByLocation.Value,
                                                        new cFilterCondition[] { new cFilterCondition("OP_TYPE_CD", "HITOS"),
                                                                 new cFilterCondition("RPT_PERIOD_ID",
                                                                                      checkReportingPeriod.RptPeriodId, //Should be the Q2 Id
                                                                                      eFilterDataType.Integer) });

                                if (operatingSuppDataView.Count == 0)
                                {
                                    if (foundOpSuppData)
                                    {
                                        category.CheckCatalogResult = "A";
                                        return resultValue;
                                    }
                                }
                                else
                                {
                                    foundOpSuppData = true;
                                    osValue += operatingSuppDataView[0]["OP_VALUE"].AsDecimal(0);
                                }
                            }
                        }
                    }

                    if (foundOpSuppData)
                    {
                        DataRowView genSummaryValueRecord;
                        {
                            genSummaryValueRecord = SummaryValueTable.Clone().DefaultView.AddNew();
                            genSummaryValueRecord["MON_LOC_ID"] = category.CurrentMonLocId;
                            genSummaryValueRecord["RPT_PERIOD_ID"] = ReportingPeriod.RptPeriodId;
                            genSummaryValueRecord["PARAMETER_CD"] = "HIT";
                            genSummaryValueRecord["CURRENT_RPT_PERIOD_TOTAL"] = 0m;

                            if (GenReportingFrequency.Value == "Q")
                                genSummaryValueRecord["YEAR_TOTAL"] = ytdValue;

                            if (GenOsReportingRequirement.Value.Default(false))
                                genSummaryValueRecord["OS_TOTAL"] = osValue;
                        }
                        GenHitSummaryValueRecord.SetValue(genSummaryValueRecord, category);
                    }
                }
            }
            catch (Exception ex)
            {
                resultValue = ex.FormatError();
            }

            return resultValue;
        }

        public string EMGEN8(cCategory category, ref bool log)
        // Generate Operating Time YTD and OS Values
        {
            string resultValue = "";

            try
            {
                GenOpTimeSummaryValueRecord.SetValue(null, category);
                decimal ytdValue = 0;
                decimal osValue = 0;

                eQuarter startQuarter;
                {
                    if (GenReportingFrequency.Value == "Q")
                        startQuarter = eQuarter.q1;
                    else
                        startQuarter = eQuarter.q2;
                }

                DataView monitorMethodView
                  = cRowFilter.FindRows(MpMethodRecords.Value,
                                        new cFilterCondition[] { new cFilterCondition("MON_LOC_ID",
                                                                              category.CurrentMonLocId),
                                                         new cFilterCondition("BEGIN_DATE",
                                                                              ReportingPeriod.EndedDate,
                                                                              eFilterDataType.DateBegan,
                                                                              eFilterConditionRelativeCompare.LessThanOrEqual),
                                                         new cFilterCondition("END_DATE",
                                                                              new DateTime(ReportingPeriod.Year, 3 * (startQuarter.AsInteger() - 1) + 1, 1),
                                                                              eFilterDataType.DateEnded,
                                                                              eFilterConditionRelativeCompare.GreaterThanOrEqual) });

                if (monitorMethodView.Count > 0)
                {
                    DateTime methodBeganDate = monitorMethodView[0]["BEGIN_DATE"].AsDateTime(DateTime.MinValue);

                    if (ReportingPeriod.Quarter > startQuarter)
                    {
                        bool missingOpSuppData = false;

                        for (int checkQuarter = ReportingPeriod.Quarter.AsInteger() - 1; checkQuarter >= startQuarter.AsInteger(); checkQuarter--)
                        {
                            cReportingPeriod checkReportingPeriod = cReportingPeriod.GetReportingPeriod(ReportingPeriod.Year, (eQuarter)checkQuarter);

                            if ((GenReportingFrequency.Value == "Q") || (checkReportingPeriod.Quarter == eQuarter.q3))
                            {
                                DataView operatingSuppDataView
                                  = cRowFilter.FindRows(OperatingSuppDataRecordsByLocation.Value,
                                                        new cFilterCondition[] { new cFilterCondition("OP_TYPE_CD", "OPTIME"),
                                                                 new cFilterCondition("RPT_PERIOD_ID",
                                                                                      checkReportingPeriod.RptPeriodId,
                                                                                      eFilterDataType.Integer) });

                                if (operatingSuppDataView.Count == 0)
                                {
                                    if (methodBeganDate <= checkReportingPeriod.EndedDate)
                                    {
                                        category.CheckCatalogResult = "A";
                                        return resultValue;
                                    }
                                    else
                                        missingOpSuppData = true;
                                }
                                else
                                {
                                    if (missingOpSuppData)
                                    {
                                        category.CheckCatalogResult = "A";
                                        return resultValue;
                                    }
                                    else
                                    {
                                        if (GenReportingFrequency.Value == "Q")
                                            ytdValue += operatingSuppDataView[0]["OP_VALUE"].AsDecimal(0);

                                        if (GenOsReportingRequirement.Value.Default(false) && (checkReportingPeriod.Quarter == eQuarter.q3))
                                            osValue += operatingSuppDataView[0]["OP_VALUE"].AsDecimal(0);
                                    }
                                }
                            }

                            if (GenOsReportingRequirement.Value.Default(false) && (checkReportingPeriod.Quarter == eQuarter.q2))
                            {
                                DataView operatingSuppDataView
                                  = cRowFilter.FindRows(OperatingSuppDataRecordsByLocation.Value,
                                                        new cFilterCondition[] { new cFilterCondition("OP_TYPE_CD", "OSTIME"),
                                                                 new cFilterCondition("RPT_PERIOD_ID",
                                                                                      checkReportingPeriod.RptPeriodId, //Should be the Q2 Id
                                                                                      eFilterDataType.Integer) });

                                if (operatingSuppDataView.Count == 0)
                                {
                                    if (methodBeganDate <= checkReportingPeriod.EndedDate)
                                    {
                                        category.CheckCatalogResult = "A";
                                        return resultValue;
                                    }
                                    else
                                        missingOpSuppData = true;
                                }
                                else
                                {
                                    if (missingOpSuppData)
                                    {
                                        category.CheckCatalogResult = "A";
                                        return resultValue;
                                    }
                                    else
                                    {
                                        osValue += operatingSuppDataView[0]["OP_VALUE"].AsDecimal(0);
                                    }
                                }
                            }
                        }
                    }

                    DataRowView genSummaryValueRecord;
                    {
                        genSummaryValueRecord = SummaryValueTable.Clone().DefaultView.AddNew();
                        genSummaryValueRecord["MON_LOC_ID"] = category.CurrentMonLocId;
                        genSummaryValueRecord["RPT_PERIOD_ID"] = ReportingPeriod.RptPeriodId;
                        genSummaryValueRecord["PARAMETER_CD"] = "OPTIME";

                        DateTime? methodEndedDate = monitorMethodView[0]["END_DATE"].AsDateTime();

                        if (!methodEndedDate.HasValue || (methodEndedDate.Value >= ReportingPeriod.BeganDate))
                            genSummaryValueRecord["CURRENT_RPT_PERIOD_TOTAL"] = 0m;

                        if (GenReportingFrequency.Value == "Q")
                            genSummaryValueRecord["YEAR_TOTAL"] = ytdValue;

                        if (GenOsReportingRequirement.Value.Default(false))
                            genSummaryValueRecord["OS_TOTAL"] = osValue;
                    }
                    GenOpTimeSummaryValueRecord.SetValue(genSummaryValueRecord, category);
                }
                else
                {
                    bool foundOpSuppData = false;

                    if (ReportingPeriod.Quarter > startQuarter)
                    {
                        for (int checkQuarter = ReportingPeriod.Quarter.AsInteger() - 1; checkQuarter >= startQuarter.AsInteger(); checkQuarter--)
                        {
                            cReportingPeriod checkReportingPeriod = cReportingPeriod.GetReportingPeriod(ReportingPeriod.Year, (eQuarter)checkQuarter);

                            if ((GenReportingFrequency.Value == "Q") || (checkReportingPeriod.Quarter == eQuarter.q3))
                            {
                                DataView operatingSuppDataView
                                  = cRowFilter.FindRows(OperatingSuppDataRecordsByLocation.Value,
                                                        new cFilterCondition[] { new cFilterCondition("OP_TYPE_CD", "OPTIME"),
                                                                 new cFilterCondition("RPT_PERIOD_ID",
                                                                                      checkReportingPeriod.RptPeriodId,
                                                                                      eFilterDataType.Integer) });

                                if (operatingSuppDataView.Count == 0)
                                {
                                    if (foundOpSuppData)
                                    {
                                        category.CheckCatalogResult = "A";
                                        return resultValue;
                                    }
                                }
                                else
                                {
                                    foundOpSuppData = true;

                                    if (GenReportingFrequency.Value == "Q")
                                        ytdValue += operatingSuppDataView[0]["OP_VALUE"].AsDecimal(0);

                                    if (GenOsReportingRequirement.Value.Default(false) && (checkReportingPeriod.Quarter == eQuarter.q3))
                                        osValue += operatingSuppDataView[0]["OP_VALUE"].AsDecimal(0);
                                }
                            }

                            if (GenOsReportingRequirement.Value.Default(false) && (checkReportingPeriod.Quarter == eQuarter.q2))
                            {
                                DataView operatingSuppDataView
                                  = cRowFilter.FindRows(OperatingSuppDataRecordsByLocation.Value,
                                                        new cFilterCondition[] { new cFilterCondition("OP_TYPE_CD", "OSTIME"),
                                                                 new cFilterCondition("RPT_PERIOD_ID",
                                                                                      checkReportingPeriod.RptPeriodId, //Should be the Q2 Id
                                                                                      eFilterDataType.Integer) });

                                if (operatingSuppDataView.Count == 0)
                                {
                                    if (foundOpSuppData)
                                    {
                                        category.CheckCatalogResult = "A";
                                        return resultValue;
                                    }
                                }
                                else
                                {
                                    foundOpSuppData = true;
                                    osValue += operatingSuppDataView[0]["OP_VALUE"].AsDecimal(0);
                                }
                            }
                        }
                    }

                    if (foundOpSuppData)
                    {
                        DataRowView genSummaryValueRecord;
                        {
                            genSummaryValueRecord = SummaryValueTable.Clone().DefaultView.AddNew();
                            genSummaryValueRecord["MON_LOC_ID"] = category.CurrentMonLocId;
                            genSummaryValueRecord["RPT_PERIOD_ID"] = ReportingPeriod.RptPeriodId;
                            genSummaryValueRecord["PARAMETER_CD"] = "OPTIME";
                            genSummaryValueRecord["CURRENT_RPT_PERIOD_TOTAL"] = 0m;

                            if (GenReportingFrequency.Value == "Q")
                                genSummaryValueRecord["YEAR_TOTAL"] = ytdValue;

                            if (GenOsReportingRequirement.Value.Default(false))
                                genSummaryValueRecord["OS_TOTAL"] = osValue;
                        }
                        GenOpTimeSummaryValueRecord.SetValue(genSummaryValueRecord, category);
                    }
                }
            }
            catch (Exception ex)
            {
                resultValue = ex.FormatError();
            }

            return resultValue;
        }

        public string EMGEN9(cCategory category, ref bool log)
        // Generate Operating Hours YTD and OS Values
        {
            string resultValue = "";

            try
            {
                GenOpHoursSummaryValueRecord.SetValue(null, category);
                decimal ytdValue = 0;
                decimal osValue = 0;

                eQuarter startQuarter;
                {
                    if (GenReportingFrequency.Value == "Q")
                        startQuarter = eQuarter.q1;
                    else
                        startQuarter = eQuarter.q2;
                }

                DataView monitorMethodView
                  = cRowFilter.FindRows(MpMethodRecords.Value,
                                        new cFilterCondition[] { new cFilterCondition("MON_LOC_ID",
                                                                              category.CurrentMonLocId),
                                                         new cFilterCondition("BEGIN_DATE",
                                                                              ReportingPeriod.EndedDate,
                                                                              eFilterDataType.DateBegan,
                                                                              eFilterConditionRelativeCompare.LessThanOrEqual),
                                                         new cFilterCondition("END_DATE",
                                                                              new DateTime(ReportingPeriod.Year, 3 * (startQuarter.AsInteger() - 1) + 1, 1),
                                                                              eFilterDataType.DateEnded,
                                                                              eFilterConditionRelativeCompare.GreaterThanOrEqual) });

                if (monitorMethodView.Count > 0)
                {
                    DateTime methodBeganDate = monitorMethodView[0]["BEGIN_DATE"].AsDateTime(DateTime.MinValue);

                    if (ReportingPeriod.Quarter > startQuarter)
                    {
                        bool missingOpSuppData = false;

                        for (int checkQuarter = ReportingPeriod.Quarter.AsInteger() - 1; checkQuarter >= startQuarter.AsInteger(); checkQuarter--)
                        {
                            cReportingPeriod checkReportingPeriod = cReportingPeriod.GetReportingPeriod(ReportingPeriod.Year, (eQuarter)checkQuarter);

                            if ((GenReportingFrequency.Value == "Q") || (checkReportingPeriod.Quarter == eQuarter.q3))
                            {
                                DataView operatingSuppDataView
                                  = cRowFilter.FindRows(OperatingSuppDataRecordsByLocation.Value,
                                                        new cFilterCondition[] { new cFilterCondition("OP_TYPE_CD", "OPHOURS"),
                                                                 new cFilterCondition("FUEL_CD", ""),
                                                                 new cFilterCondition("RPT_PERIOD_ID",
                                                                                      checkReportingPeriod.RptPeriodId,
                                                                                      eFilterDataType.Integer) });

                                if (operatingSuppDataView.Count == 0)
                                {
                                    if (methodBeganDate <= checkReportingPeriod.EndedDate)
                                    {
                                        category.CheckCatalogResult = "A";
                                        return resultValue;
                                    }
                                    else
                                        missingOpSuppData = true;
                                }
                                else
                                {
                                    if (missingOpSuppData)
                                    {
                                        category.CheckCatalogResult = "A";
                                        return resultValue;
                                    }
                                    else
                                    {
                                        if (GenReportingFrequency.Value == "Q")
                                            ytdValue += operatingSuppDataView[0]["OP_VALUE"].AsDecimal(0);

                                        if (GenOsReportingRequirement.Value.Default(false) && (checkReportingPeriod.Quarter == eQuarter.q3))
                                            osValue += operatingSuppDataView[0]["OP_VALUE"].AsDecimal(0);
                                    }
                                }
                            }

                            if (GenOsReportingRequirement.Value.Default(false) && (checkReportingPeriod.Quarter == eQuarter.q2))
                            {
                                DataView operatingSuppDataView
                                  = cRowFilter.FindRows(OperatingSuppDataRecordsByLocation.Value,
                                                        new cFilterCondition[] { new cFilterCondition("OP_TYPE_CD", "OSHOURS"),
                                                                 new cFilterCondition("FUEL_CD", ""),
                                                                 new cFilterCondition("RPT_PERIOD_ID",
                                                                                      checkReportingPeriod.RptPeriodId, //Should be the Q2 Id
                                                                                      eFilterDataType.Integer) });

                                if (operatingSuppDataView.Count == 0)
                                {
                                    if (methodBeganDate <= checkReportingPeriod.EndedDate)
                                    {
                                        category.CheckCatalogResult = "A";
                                        return resultValue;
                                    }
                                    else
                                        missingOpSuppData = true;
                                }
                                else
                                {
                                    if (missingOpSuppData)
                                    {
                                        category.CheckCatalogResult = "A";
                                        return resultValue;
                                    }
                                    else
                                    {
                                        osValue += operatingSuppDataView[0]["OP_VALUE"].AsDecimal(0);
                                    }
                                }
                            }
                        }
                    }

                    DataRowView genSummaryValueRecord;
                    {
                        genSummaryValueRecord = SummaryValueTable.Clone().DefaultView.AddNew();
                        genSummaryValueRecord["MON_LOC_ID"] = category.CurrentMonLocId;
                        genSummaryValueRecord["RPT_PERIOD_ID"] = ReportingPeriod.RptPeriodId;
                        genSummaryValueRecord["PARAMETER_CD"] = "OPHOURS";

                        DateTime? methodEndedDate = monitorMethodView[0]["END_DATE"].AsDateTime();

                        if (!methodEndedDate.HasValue || (methodEndedDate.Value >= ReportingPeriod.BeganDate))
                            genSummaryValueRecord["CURRENT_RPT_PERIOD_TOTAL"] = 0m;

                        if (GenReportingFrequency.Value == "Q")
                            genSummaryValueRecord["YEAR_TOTAL"] = ytdValue;

                        if (GenOsReportingRequirement.Value.Default(false))
                            genSummaryValueRecord["OS_TOTAL"] = osValue;
                    }
                    GenOpHoursSummaryValueRecord.SetValue(genSummaryValueRecord, category);
                }
                else
                {
                    bool foundOpSuppData = false;

                    if (ReportingPeriod.Quarter > startQuarter)
                    {
                        for (int checkQuarter = ReportingPeriod.Quarter.AsInteger() - 1; checkQuarter >= startQuarter.AsInteger(); checkQuarter--)
                        {
                            cReportingPeriod checkReportingPeriod = cReportingPeriod.GetReportingPeriod(ReportingPeriod.Year, (eQuarter)checkQuarter);

                            if ((GenReportingFrequency.Value == "Q") || (checkReportingPeriod.Quarter == eQuarter.q3))
                            {
                                DataView operatingSuppDataView
                                  = cRowFilter.FindRows(OperatingSuppDataRecordsByLocation.Value,
                                                        new cFilterCondition[] { new cFilterCondition("OP_TYPE_CD", "OPHOURS"),
                                                                 new cFilterCondition("RPT_PERIOD_ID",
                                                                                      checkReportingPeriod.RptPeriodId,
                                                                                      eFilterDataType.Integer) });

                                if (operatingSuppDataView.Count == 0)
                                {
                                    if (foundOpSuppData)
                                    {
                                        category.CheckCatalogResult = "A";
                                        return resultValue;
                                    }
                                }
                                else
                                {
                                    foundOpSuppData = true;

                                    if (GenReportingFrequency.Value == "Q")
                                        ytdValue += operatingSuppDataView[0]["OP_VALUE"].AsDecimal(0);

                                    if (GenOsReportingRequirement.Value.Default(false) && (checkReportingPeriod.Quarter == eQuarter.q3))
                                        osValue += operatingSuppDataView[0]["OP_VALUE"].AsDecimal(0);
                                }
                            }

                            if (GenOsReportingRequirement.Value.Default(false) && (checkReportingPeriod.Quarter == eQuarter.q2))
                            {
                                DataView operatingSuppDataView
                                  = cRowFilter.FindRows(OperatingSuppDataRecordsByLocation.Value,
                                                        new cFilterCondition[] { new cFilterCondition("OP_TYPE_CD", "OSHOURS"),
                                                                 new cFilterCondition("RPT_PERIOD_ID",
                                                                                      checkReportingPeriod.RptPeriodId, //Should be the Q2 Id
                                                                                      eFilterDataType.Integer) });

                                if (operatingSuppDataView.Count == 0)
                                {
                                    if (foundOpSuppData)
                                    {
                                        category.CheckCatalogResult = "A";
                                        return resultValue;
                                    }
                                }
                                else
                                {
                                    foundOpSuppData = true;
                                    osValue += operatingSuppDataView[0]["OP_VALUE"].AsDecimal(0);
                                }
                            }
                        }
                    }

                    if (foundOpSuppData)
                    {
                        DataRowView genSummaryValueRecord;
                        {
                            genSummaryValueRecord = SummaryValueTable.Clone().DefaultView.AddNew();
                            genSummaryValueRecord["MON_LOC_ID"] = category.CurrentMonLocId;
                            genSummaryValueRecord["RPT_PERIOD_ID"] = ReportingPeriod.RptPeriodId;
                            genSummaryValueRecord["PARAMETER_CD"] = "OPHOURS";
                            genSummaryValueRecord["CURRENT_RPT_PERIOD_TOTAL"] = 0m;

                            if (GenReportingFrequency.Value == "Q")
                                genSummaryValueRecord["YEAR_TOTAL"] = ytdValue;

                            if (GenOsReportingRequirement.Value.Default(false))
                                genSummaryValueRecord["OS_TOTAL"] = osValue;
                        }
                        GenOpHoursSummaryValueRecord.SetValue(genSummaryValueRecord, category);
                    }
                }
            }
            catch (Exception ex)
            {
                resultValue = ex.FormatError();
            }

            return resultValue;
        }

        public string EMGEN10(cCategory category, ref bool log)
        // Generate BCO2 Mass YTD Values 
        {
            string resultValue = "";

            try
            {
                GenBco2SummaryValueRecord.SetValue(null, category);
                decimal ytdValue = 0;

                if (ReportingPeriod.Quarter > eQuarter.q1)
                {
                    bool foundOpSuppData = false;

                    for (int checkQuarter = ReportingPeriod.Quarter.AsInteger() - 1; checkQuarter >= 1; checkQuarter--)
                    {
                        cReportingPeriod checkReportingPeriod = cReportingPeriod.GetReportingPeriod(ReportingPeriod.Year, (eQuarter)checkQuarter);

                        DataView checkOperatingSuppDataView
                          = cRowFilter.FindRows(OperatingSuppDataRecordsByLocation.Value,
                                                new cFilterCondition[] { new cFilterCondition("OP_TYPE_CD", "BCO2"),
                                                               new cFilterCondition("RPT_PERIOD_ID",
                                                                                    checkReportingPeriod.RptPeriodId,
                                                                                    eFilterDataType.Integer) });

                        if (checkOperatingSuppDataView.Count == 0)
                        {
                            if (foundOpSuppData)
                            {
                                category.CheckCatalogResult = "A";
                                return resultValue;
                            }
                        }
                        else
                        {
                            if ((foundOpSuppData == false) && (checkQuarter != (ReportingPeriod.Quarter.AsInteger() - 1)))
                            {
                                category.CheckCatalogResult = "A";
                                return resultValue;
                            }
                            else
                            {
                                foundOpSuppData = true;
                                ytdValue += checkOperatingSuppDataView[0]["OP_VALUE"].AsDecimal(0);
                            }
                        }
                    }

                    if (foundOpSuppData)
                    {
                        DataRowView genSummaryValueRecord;
                        {
                            genSummaryValueRecord = SummaryValueTable.Clone().DefaultView.AddNew();
                            genSummaryValueRecord["MON_LOC_ID"] = category.CurrentMonLocId;
                            genSummaryValueRecord["RPT_PERIOD_ID"] = ReportingPeriod.RptPeriodId;
                            genSummaryValueRecord["PARAMETER_CD"] = "BCO2";
                            genSummaryValueRecord["CURRENT_RPT_PERIOD_TOTAL"] = 0m;
                            genSummaryValueRecord["YEAR_TOTAL"] = ytdValue;
                        }
                        GenBco2SummaryValueRecord.SetValue(genSummaryValueRecord, category);
                    }
                }
            }
            catch (Exception ex)
            {
                resultValue = ex.FormatError();
            }

            return resultValue;
        }

        /// <summary>
        /// EMGEN-11
        /// 
        /// Initializes program code lists that contain programs that:
        /// 
        /// 1) Are ozone season programs
        /// </summary>
        /// <param name="category"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public string EMGEN11(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                EmGenerationParameters.ProgramIsOzoneSeasonList = "";

                foreach (ProgramCodeRow programCodeRow in EmGenerationParameters.ProgramCodeTable)
                {
                    if (programCodeRow.OsInd == 1)
                        EmGenerationParameters.ProgramIsOzoneSeasonList = EmGenerationParameters.ProgramIsOzoneSeasonList.ListAdd(programCodeRow.PrgCd);
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
