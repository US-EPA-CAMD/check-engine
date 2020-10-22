using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.EmissionsChecks;
using ECMPS.Checks.EmissionsReport;

using ECMPS.Checks.Data.Ecmps.Dbo.View;
using ECMPS.Checks.Em.Parameters;
using UnitTest.UtilityClasses;


namespace UnitTest.Emissions
{
    [TestClass]
    public class DailyCalibrationStatusTest
    {

        #region DcStat-2

        /// <summary>
        /// 
        /// Input Parameters:
        /// 
        ///   ApplicableComponentId                    : "GoodCmp"
        ///   CurrentAnalyzerRangeUsed                 : "H"
        ///   DualRangeStatus                          : false
        ///   CurrentDateHour                          : 2016-06-17 22
        ///   CurrentReportingPeriod                   : [2016 Q2 (94)]
        ///   First Day/Hour of Operation              : CurrentDateHour - [First] - [op] + 1 and if [NonOp] is true, - 1. (Plus one makes [First] the number of hours between Firt and Current hour)
        ///   HighRangeComponentID                     : [ApplicableComponentId]
        ///   HourlyNonOperatingDataRecordsForLocation : Fill with non operating hours based on CurrentDateHour, [First], [NonOp] and [Op].
        ///   HourlyOperatingDataRecordsForLocation    : Fill with operating hours based on CurrentDateHour, [First], [NonOp] and [Op].
        ///   LowRangeComponentID                      : [ApplicableComponentId]
        ///   MatsDailyCalRequiredDate                 : [Req Date]
        ///   PriorDailyCalRecord                      : null
        ///   QACertificationEventRecords              : [empty]
        ///   QaStatusComponentBeginDateHour           : CurrentDateHour - [Component] + 1 (Plus one makes [Component] the number of hours between Firt and Current hour)
        ///   QaStatusSystemTypeCOde                   : [Sys]
        /// 
        /// Input/Ouput Parameters:
        /// 
        ///   CurrentDailyCalStatus                    : null / [Status]
        ///   InvalidDailyCalRecord                    : null / null
        /// 
        /// Output Parameters:
        /// 
        ///   PriorDailyCalEventRecord                 : null
        /// 
        /// Note:
        ///   Op1 - Includes the first operating hour.
        ///   Op2 - Does not include the current hour.
        /// 
        /// Cases:
        /// 
        /// |    |     |            - Hours -          |             ||
        /// | ## | Sys | Op1 | NonOp | Op2 | Component | Req Date    || Status            || Note
        /// |  0 | HG  |  26 |     0 |   0 |        26 | current     || OOC-No Prior Test || OOC-No Prior Test because no non operating hours exist after first operating hour.
        /// |  1 | HCL |  26 |     0 |   0 |        26 | current     || OOC-No Prior Test || OOC-No Prior Test because no non operating hours exist after first operating hour.
        /// |  2 | HF  |  26 |     0 |   0 |        26 | current     || OOC-No Prior Test || OOC-No Prior Test because no non operating hours exist after first operating hour.
        /// |  3 | SO2 |  26 |     0 |   0 |        26 | current     || OOC-No Prior Test || OOC-No Prior Test because no non operating hours exist after first operating hour.
        /// |  4 | HG  |  25 |     0 |   0 |        26 | current     || IC-Undetermined   || IC-Undetermined based on the number of clock hours after first operating hour.
        /// |  5 | HCL |  25 |     0 |   0 |        26 | current     || IC-Undetermined   || IC-Undetermined based on the number of clock hours after first operating hour.
        /// |  6 | HF  |  25 |     0 |   0 |        26 | current     || IC-Undetermined   || IC-Undetermined based on the number of clock hours after first operating hour.
        /// |  7 | SO2 |  25 |     0 |   0 |        26 | current     || IC-Undetermined   || IC-Undetermined based on the number of clock hours after first operating hour.
        /// |  8 | HG  |  26 |     0 |   0 |        25 | current     || IC-Undetermined   || IC-Undetermined based on HG/HCL condition and the number of clock hours after the component begin hour.
        /// |  9 | HCL |  26 |     0 |   0 |        25 | current     || IC-Undetermined   || IC-Undetermined based on HG/HCL condition and the number of clock hours after the component begin hour.
        /// | 10 | HF  |  26 |     0 |   0 |        25 | current     || OOC-No Prior Test || OOC-No Prior Test based failing First Hour condition and HG/HCL condition, but having non operating hours within 24 hours and no subsequent operating hours before eight hours.
        /// | 11 | SO2 |  26 |     0 |   0 |        25 | current     || OOC-No Prior Test || OOC-No Prior Test based failing First Hour condition and HG/HCL condition, but having non operating hours within 24 hours and no subsequent operating hours before eight hours.
        /// | 12 | HG  |  26 |     0 |   0 |        26 | current + 1 || IC-Undetermined   || IC-Undetermined based on HG/HCL condition and the Daily Cal Required date.
        /// | 13 | HCL |  26 |     0 |   0 |        26 | current + 1 || IC-Undetermined   || IC-Undetermined based on HG/HCL condition and the Daily Cal Required date.
        /// | 14 | HF  |  26 |     0 |   0 |        26 | current + 1 || OOC-No Prior Test || OOC-No Prior Test based failing First Hour condition and HG/HCL condition, but having non operating hours within 24 hours and no subsequent operating hours before eight hours.
        /// | 15 | SO2 |  26 |     0 |   0 |        26 | current + 1 || OOC-No Prior Test || OOC-No Prior Test based failing First Hour condition and HG/HCL condition, but having non operating hours within 24 hours and no subsequent operating hours before eight hours.
        /// | 16 | HG  |  25 |     1 |   7 |        26 | current     || OOC-No Prior Test || OOC-No Prior Test because non op occurs after 25th hour following first op hour.  Would have been IC-Undetermined otherwise.
        /// | 17 | HCL |  25 |     1 |   7 |        26 | current     || OOC-No Prior Test || OOC-No Prior Test because non op occurs after 25th hour following first op hour.  Would have been IC-Undetermined otherwise.
        /// | 18 | HF  |  25 |     1 |   7 |        26 | current     || OOC-No Prior Test || OOC-No Prior Test because non op occurs after 25th hour following first op hour.  Would have been IC-Undetermined otherwise.
        /// | 19 | SO2 |  25 |     1 |   7 |        26 | current     || OOC-No Prior Test || OOC-No Prior Test because non op occurs after 25th hour following first op hour.  Would have been IC-Undetermined otherwise.
        /// | 20 | HG  |  24 |     1 |   8 |        26 | current     || OOC-No Prior Test || OOC-No Prior Test because eight hours occur between the first op hour after the non op hour, and the current hour.
        /// | 21 | HCL |  24 |     1 |   8 |        26 | current     || OOC-No Prior Test || OOC-No Prior Test because eight hours occur between the first op hour after the non op hour, and the current hour.
        /// | 22 | HF  |  24 |     1 |   8 |        26 | current     || OOC-No Prior Test || OOC-No Prior Test because eight hours occur between the first op hour after the non op hour, and the current hour.
        /// | 23 | SO2 |  24 |     1 |   8 |        26 | current     || OOC-No Prior Test || OOC-No Prior Test because eight hours occur between the first op hour after the non op hour, and the current hour.
        /// | 24 | HG  |  24 |     1 |   8 |        26 | null        || OOC-No Prior Test || OOC-No Prior Test because eight hours occur between the first op hour after the non op hour, and the current hour.  Preceeding MatsDailyCalRequiredDate condition failed because it is null.
        /// | 25 | HCL |  24 |     1 |   8 |        26 | null        || OOC-No Prior Test || OOC-No Prior Test because eight hours occur between the first op hour after the non op hour, and the current hour.  Preceeding MatsDailyCalRequiredDate condition failed because it is null.
        /// | 26 | HF  |  24 |     1 |   8 |        26 | null        || OOC-No Prior Test || OOC-No Prior Test because eight hours occur between the first op hour after the non op hour, and the current hour.  Preceeding MatsDailyCalRequiredDate condition failed because it is null.
        /// | 27 | SO2 |  24 |     1 |   8 |        26 | null        || OOC-No Prior Test || OOC-No Prior Test because eight hours occur between the first op hour after the non op hour, and the current hour.  Preceeding MatsDailyCalRequiredDate condition failed because it is null.
        /// | 28 | HG  |  24 |     1 |   7 |        26 | current     || IC-Undetermined   || IC-Undetermined because less than 8 hours exist between the first op hour after the non op hour, and the current hour.
        /// | 29 | HCL |  24 |     1 |   7 |        26 | current     || IC-Undetermined   || IC-Undetermined because less than 8 hours exist between the first op hour after the non op hour, and the current hour.
        /// | 30 | HF  |  24 |     1 |   7 |        26 | current     || IC-Undetermined   || IC-Undetermined because less than 8 hours exist between the first op hour after the non op hour, and the current hour.
        /// | 31 | SO2 |  24 |     1 |   7 |        26 | current     || IC-Undetermined   || IC-Undetermined because less than 8 hours exist between the first op hour after the non op hour, and the current hour.
        /// | 32 | HG  |  24 |     2 |   0 |        26 | current     || IC-Undetermined   || IC-Undetermined because no operating hours exist between the first non op hour and the current hour.
        /// | 33 | HCL |  24 |     2 |   0 |        26 | current     || IC-Undetermined   || IC-Undetermined because no operating hours exist between the first non op hour and the current hour.
        /// | 34 | HF  |  24 |     2 |   0 |        26 | current     || IC-Undetermined   || IC-Undetermined because no operating hours exist between the first non op hour and the current hour.
        /// | 35 | SO2 |  24 |     2 |   0 |        26 | current     || IC-Undetermined   || IC-Undetermined because no operating hours exist between the first non op hour and the current hour.
        /// | 36 | HG  |  24 |     0 |   7 |        26 | current     || OOC-No Prior Test || OOC-No Prior Test because no non operating hours exist after first operating hour.
        /// | 37 | HCL |  24 |     0 |   7 |        26 | current     || OOC-No Prior Test || OOC-No Prior Test because no non operating hours exist after first operating hour.
        /// | 38 | HF  |  24 |     0 |   7 |        26 | current     || OOC-No Prior Test || OOC-No Prior Test because no non operating hours exist after first operating hour.
        /// | 39 | SO2 |  24 |     0 |   7 |        26 | current     || OOC-No Prior Test || OOC-No Prior Test because no non operating hours exist after first operating hour.
        /// 
        /// </summary>
        [TestMethod]
        public void DcStat2_NoTestOrEvent()
        {
            /* Initialize objects generally needed for testing checks. */
            cDailyCalibrationStatusChecks target = new cDailyCalibrationStatusChecks(new cEmissionsCheckParameters());
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            UnitTestExtensions.SetReportingPeriodTable();

            /* Test Case Count */
            int caseCount = 40;

            /* Input Parameter Values */
            string[] sysTypeList = { "HG", "HCL", "HF", "SO2", "HG", "HCL", "HF", "SO2", "HG", "HCL", "HF", "SO2", "HG", "HCL", "HF", "SO2", "HG", "HCL", "HF", "SO2", "HG", "HCL", "HF", "SO2", "HG", "HCL", "HF", "SO2", "HG", "HCL", "HF", "SO2", "HG", "HCL", "HF", "SO2", "HG", "HCL", "HF", "SO2" };
            int[] op1HourOffsetList = { 26, 26, 26, 26, 25, 25, 25, 25, 26, 26, 26, 26, 26, 26, 26, 26, 25, 25, 25, 25, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24 };
            int[] nonOpHourOffsetList = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 0, 0, 0, 0 };
            int[] op2HourOffsetList = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7, 7, 7, 7, 8, 8, 8, 8, 8, 8, 8, 8, 7, 7, 7, 7, 0, 0, 0, 0, 7, 7, 7, 7 }; 
            int[] componentHourOffsetList = { 26, 26, 26, 26, 26, 26, 26, 26, 25, 25, 25, 25, 26, 26, 26, 26, 26, 26, 26, 26, 26, 26, 26, 26, 26, 26, 26, 26, 26, 26, 26, 26, 26, 26, 26, 26, 26, 26, 26, 26 };
            int?[] requiredDateOffsetList = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, null, null, null, null, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            DateTime currentDatehour = new DateTime(2016, 6, 17, 22, 0, 0);

            /* Expected Values */
            string[] statusList = { "OOC-No Prior Test", "OOC-No Prior Test", "OOC-No Prior Test", "OOC-No Prior Test", "IC-Undetermined", "IC-Undetermined", "IC-Undetermined", "IC-Undetermined", "IC-Undetermined", "IC-Undetermined", "OOC-No Prior Test", "OOC-No Prior Test", "IC-Undetermined", "IC-Undetermined", "OOC-No Prior Test", "OOC-No Prior Test", "OOC-No Prior Test", "OOC-No Prior Test", "OOC-No Prior Test", "OOC-No Prior Test", "OOC-No Prior Test", "OOC-No Prior Test", "OOC-No Prior Test", "OOC-No Prior Test", "OOC-No Prior Test", "OOC-No Prior Test", "OOC-No Prior Test", "OOC-No Prior Test", "IC-Undetermined", "IC-Undetermined", "IC-Undetermined", "IC-Undetermined", "IC-Undetermined", "IC-Undetermined", "IC-Undetermined", "IC-Undetermined", "OOC-No Prior Test", "OOC-No Prior Test", "OOC-No Prior Test", "OOC-No Prior Test" };

            /* Check array lengths */
            Assert.AreEqual(caseCount, sysTypeList.Length, "sysTypeList length");
            Assert.AreEqual(caseCount, op1HourOffsetList.Length, "op1HourOffsetList length");
            Assert.AreEqual(caseCount, componentHourOffsetList.Length, "componentHourOffsetList length");
            Assert.AreEqual(caseCount, nonOpHourOffsetList.Length, "nonOpHourOffsetList length");
            Assert.AreEqual(caseCount, op2HourOffsetList.Length, "op2HourOffsetList length");
            Assert.AreEqual(caseCount, requiredDateOffsetList.Length, "requiredDateOffsetList length");
            Assert.AreEqual(caseCount, statusList.Length, "statusList length");

            /* Run Cases */
            for (int caseDex = 0; caseDex < caseCount; caseDex++)
            {
                /*  General Variables */
                DateTime firstOperatingBeginHour = currentDatehour.AddHours(-1 * (op1HourOffsetList[caseDex] + nonOpHourOffsetList[caseDex] + op2HourOffsetList[caseDex]));
                DateTime firstOperatingEndHour = firstOperatingBeginHour.AddHours(op1HourOffsetList[caseDex] - 1);
                DateTime? nonOperatingBeginHour = (nonOpHourOffsetList[caseDex] > 0 ? firstOperatingEndHour.AddHours(1) : (DateTime?)null);
                DateTime? nonOperatingEndHour = (nonOpHourOffsetList[caseDex] > 0 ? nonOperatingBeginHour.Value.AddHours(nonOpHourOffsetList[caseDex] - 1) : (DateTime?)null);
                DateTime? nextOperatingBeginHour = (op2HourOffsetList[caseDex] > 0 ? firstOperatingEndHour.AddHours(nonOpHourOffsetList[caseDex] + 1) : (DateTime ?)null);
                DateTime? nextOperatingEndHour = (op2HourOffsetList[caseDex] > 0 ? nextOperatingBeginHour.Value.AddHours(op2HourOffsetList[caseDex] - 1) : (DateTime?)null);

                /* Initialize Input Parameters */
                EmParameters.ApplicableComponentId = "GoodCmp";
                EmParameters.CurrentAnalyzerRangeUsed = "H";
                EmParameters.DualRangeStatus = false;
                EmParameters.CurrentDateHour = currentDatehour;
                EmParameters.CurrentReportingPeriod = 94;
                EmParameters.FirstDayOfOperation = firstOperatingBeginHour.Date;
                EmParameters.FirstHourOfOperation = firstOperatingBeginHour.Hour;
                EmParameters.HighRangeComponentId = EmParameters.ApplicableComponentId;
                EmParameters.HourlyNonOperatingDataRecordsForLocation = new CheckDataView<VwMpHrlyOpDataRow>();
                {
                    /* Add Initial non-operating hours */
                    for (DateTime hour = new DateTime(2016, 6, 16); hour < firstOperatingBeginHour; hour = hour.AddHours(1))
                        EmParameters.HourlyNonOperatingDataRecordsForLocation.Add(new VwMpHrlyOpDataRow(beginDatehour: hour, beginDate: hour.Date, beginHour: hour.Hour));

                    /* Add Itermitten non-operating hour */
                    if (nonOperatingBeginHour.HasValue)
                        for (DateTime hour = nonOperatingBeginHour.Value; hour <= nonOperatingEndHour.Value; hour = hour.AddHours(1))
                            EmParameters.HourlyNonOperatingDataRecordsForLocation.Add(new VwMpHrlyOpDataRow(beginDatehour: hour, beginDate: hour.Date, beginHour: hour.Hour));

                    /* Add Ending non-operating hours */
                    for (DateTime hour = currentDatehour.AddHours(1); hour < new DateTime(2016, 6, 19); hour = hour.AddHours(1))
                        EmParameters.HourlyNonOperatingDataRecordsForLocation.Add(new VwMpHrlyOpDataRow(beginDatehour: hour, beginDate: hour.Date, beginHour: hour.Hour));
                }
                EmParameters.HourlyOperatingDataRecordsForLocation = new CheckDataView<VwMpHrlyOpDataRow>();
                {
                    /* Add First operating hours */
                    for (DateTime hour = firstOperatingBeginHour; hour <= firstOperatingEndHour; hour = hour.AddHours(1))
                        EmParameters.HourlyOperatingDataRecordsForLocation.Add(new VwMpHrlyOpDataRow(beginDatehour: hour, beginDate: hour.Date, beginHour: hour.Hour));

                    /* Add Additional operating */
                    if (nextOperatingBeginHour.HasValue)
                        for (DateTime hour = nextOperatingBeginHour.Value; hour <= nextOperatingEndHour; hour = hour.AddHours(1))
                            EmParameters.HourlyOperatingDataRecordsForLocation.Add(new VwMpHrlyOpDataRow(beginDatehour: hour, beginDate: hour.Date, beginHour: hour.Hour));

                    /* Add ccurrent hour */
                    EmParameters.HourlyOperatingDataRecordsForLocation.Add(new VwMpHrlyOpDataRow(beginDatehour: currentDatehour, beginDate: currentDatehour.Date, beginHour: currentDatehour.Hour));
                }
                EmParameters.LowRangeComponentId = EmParameters.ApplicableComponentId;
                EmParameters.MatsDailyCalRequiredDatehour = (requiredDateOffsetList[caseDex] == null ? (DateTime?)null : currentDatehour.AddHours(requiredDateOffsetList[caseDex].Value));
                EmParameters.PriorDailyCalRecord = null;
                EmParameters.QaCertificationEventRecords = new CheckDataView<VwQaCertEventRow>();
                EmParameters.QaStatusComponentBeginDatehour = currentDatehour.AddHours(-1 * componentHourOffsetList[caseDex]);
                EmParameters.QaStatusSystemTypeCode = sysTypeList[caseDex];

                /* Initialize Input/Ouput Parameters */
                EmParameters.CurrentDailyCalStatus = null;
                EmParameters.InvalidDailyCalRecord = null;

                /* Initialize Output Parameters */
                EmParameters.PriorDailyCalEventRecord = null;


                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Initialize variables needed to run the check. */
                bool log = false;
                string actual;

                if (caseDex >= 0)
                {

                    /* Run Check */
                    actual = target.DCSTAT2(category, ref log);

                    /* Check Results */
                    Assert.AreEqual(string.Empty, actual, string.Format("actual {0}", caseDex));
                    Assert.AreEqual(false, log, string.Format("log {0}", caseDex));
                    Assert.AreEqual(null, category.CheckCatalogResult, string.Format("category.CheckCatalogResult {0}", caseDex));

                    Assert.AreEqual(statusList[caseDex], EmParameters.CurrentDailyCalStatus, string.Format("CurrentDailyCalStatus {0}", caseDex));
                    Assert.AreEqual(null, EmParameters.InvalidDailyCalRecord, string.Format("InvalidDailyCalRecord {0}", caseDex));
                    Assert.AreEqual(null, EmParameters.PriorDailyCalEventRecord, string.Format("PriorDailyCalEventRecord {0}", caseDex));
                }
            }
        }

        /// <summary>
        /// DcStat-2 coverting MATS ERB conditions.
        /// 
        /// Notes:
        /// 
        /// * Make the number of days between the first operating hour and the MATS ERB Date, hour 0, more than 25.  (At least FirstHour + 26).
        /// * Make QaStatusComponentBeginDatehour on or before the first operating hour.
        /// * Make MatsDailyCalRequiredDate null.
        /// 
        /// Ref : 2016-06-17 (no hour/hour 0)
        /// 
        /// * +/- hours for Current Hr column.
        /// 
        /// | ## | Sys | ERBD | Current Hr || Status            || Note
        /// |  0 | HG  | Ref  | Ref + 25   || IC-Undetermined   || IC-Undetermined because less than 25 hours exist between hour 0 of the ERBD and the current hour.
        /// |  1 | HCL | Ref  | Ref + 25   || IC-Undetermined   || IC-Undetermined because less than 25 hours exist between hour 0 of the ERBD and the current hour.
        /// |  2 | HF  | Ref  | Ref + 25   || OOC-No Prior Test || OOC-No Prior Test based failing HG/HCL condition for ERBD.
        /// |  3 | SO2 | Ref  | Ref + 25   || OOC-No Prior Test || OOC-No Prior Test based failing HG/HCL condition for ERBD.
        /// |  4 | HG  | Ref  | Ref + 26   || OOC-No Prior Test || OOC-No Prior Test based failing 25 hours between condition for ERBD.
        /// |  5 | HCL | Ref  | Ref + 26   || OOC-No Prior Test || OOC-No Prior Test based failing 25 hours between condition for ERBD.
        /// |  6 | HF  | Ref  | Ref + 26   || OOC-No Prior Test || OOC-No Prior Test based failing HG/HCL and 25 hours between conditions for ERBD.
        /// |  7 | SO2 | Ref  | Ref + 26   || OOC-No Prior Test || OOC-No Prior Test based failing HG/HCL and 25 hours between conditions for ERBD.
        /// |  8 | HG  | null | Ref + 25   || OOC-No Prior Test || OOC-No Prior Test based failing QaStatusMatsErbDate exists condition for ERBD.
        /// |  9 | HCL | null | Ref + 25   || OOC-No Prior Test || OOC-No Prior Test based failing QaStatusMatsErbDate exists condition for ERBD.
        /// | 10 | HF  | null | Ref + 25   || OOC-No Prior Test || OOC-No Prior Test based failing HG/HCL and QaStatusMatsErbDate exists conditions for ERBD.
        /// | 11 | SO2 | null | Ref + 25   || OOC-No Prior Test || OOC-No Prior Test based failing HG/HCL and QaStatusMatsErbDate exists conditions for ERBD.
        /// </summary>
        [TestMethod]
        public void DcStat2_NoTestOrEvent_ErbDate()
        {
            /* Initialize objects generally needed for testing checks. */
            cDailyCalibrationStatusChecks target = new cDailyCalibrationStatusChecks(new cEmissionsCheckParameters());
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            UnitTestExtensions.SetReportingPeriodTable();

            /* Test Case Count */
            int caseCount = 12;

            /* Input Parameter Values */
            DateTime refDate = new DateTime(2016, 6, 17);

            string[] sysTypeList = { "HG", "HCL", "HF", "SO2", "HG", "HCL", "HF", "SO2", "HG", "HCL", "HF", "SO2" };
            DateTime?[] erbDateList = { refDate, refDate, refDate, refDate, refDate, refDate, refDate, refDate, null, null, null, null };
            DateTime[] currentHourList = { refDate.AddHours(25), refDate.AddHours(25), refDate.AddHours(25), refDate.AddHours(25), refDate.AddHours(26), refDate.AddHours(26), refDate.AddHours(26), refDate.AddHours(26), refDate.AddHours(25), refDate.AddHours(25), refDate.AddHours(25), refDate.AddHours(25) };

            /* Expected Values */
            string[] statusList = { "IC-Undetermined", "IC-Undetermined", "OOC-No Prior Test", "OOC-No Prior Test", "OOC-No Prior Test", "OOC-No Prior Test", "OOC-No Prior Test", "OOC-No Prior Test", "OOC-No Prior Test", "OOC-No Prior Test", "OOC-No Prior Test", "OOC-No Prior Test" };

            /* Check array lengths */
            Assert.AreEqual(caseCount, sysTypeList.Length, "sysTypeList length");
            Assert.AreEqual(caseCount, currentHourList.Length, "currentHourList length");
            Assert.AreEqual(caseCount, statusList.Length, "statusList length");

            /* Run Cases */
            for (int caseDex = 0; caseDex < caseCount; caseDex++)
            {
                /*  General Variables */
                DateTime currentDatehour = currentHourList[caseDex];
                DateTime firstOperatingBeginHour = refDate.AddHours(-26);
                DateTime firstOperatingEndHour = currentDatehour.AddHours(-1);
                DateTime componentBegin = firstOperatingBeginHour.AddDays(-13);

                /* Initialize Input Parameters */
                EmParameters.ApplicableComponentId = "GoodCmp";
                EmParameters.CurrentAnalyzerRangeUsed = "H";
                EmParameters.DualRangeStatus = false;
                EmParameters.CurrentDateHour = currentDatehour;
                EmParameters.CurrentReportingPeriod = 94;
                EmParameters.FirstDayOfOperation = firstOperatingBeginHour.Date;
                EmParameters.FirstHourOfOperation = firstOperatingBeginHour.Hour;
                EmParameters.HighRangeComponentId = EmParameters.ApplicableComponentId;
                EmParameters.HourlyNonOperatingDataRecordsForLocation = new CheckDataView<VwMpHrlyOpDataRow>();
                {
                    /* Add Initial non-operating hours */
                    for (DateTime hour = new DateTime(2016, 6, 16); hour < firstOperatingBeginHour; hour = hour.AddHours(1))
                        EmParameters.HourlyNonOperatingDataRecordsForLocation.Add(new VwMpHrlyOpDataRow(beginDatehour: hour, beginDate: hour.Date, beginHour: hour.Hour));

                    /* Add Ending non-operating hours */
                    for (DateTime hour = currentDatehour.AddHours(1); hour < new DateTime(2016, 6, 19); hour = hour.AddHours(1))
                        EmParameters.HourlyNonOperatingDataRecordsForLocation.Add(new VwMpHrlyOpDataRow(beginDatehour: hour, beginDate: hour.Date, beginHour: hour.Hour));
                }
                EmParameters.HourlyOperatingDataRecordsForLocation = new CheckDataView<VwMpHrlyOpDataRow>();
                {
                    /* Add First operating hours */
                    for (DateTime hour = firstOperatingBeginHour; hour <= firstOperatingEndHour; hour = hour.AddHours(1))
                        EmParameters.HourlyOperatingDataRecordsForLocation.Add(new VwMpHrlyOpDataRow(beginDatehour: hour, beginDate: hour.Date, beginHour: hour.Hour));

                    /* Add ccurrent hour */
                    EmParameters.HourlyOperatingDataRecordsForLocation.Add(new VwMpHrlyOpDataRow(beginDatehour: currentDatehour, beginDate: currentDatehour.Date, beginHour: currentDatehour.Hour));
                }
                EmParameters.LowRangeComponentId = EmParameters.ApplicableComponentId;
                EmParameters.MatsDailyCalRequiredDatehour = null;
                EmParameters.QaStatusMatsErbDate = erbDateList[caseDex];
                EmParameters.PriorDailyCalRecord = null;
                EmParameters.QaCertificationEventRecords = new CheckDataView<VwQaCertEventRow>();
                EmParameters.QaStatusComponentBeginDatehour = componentBegin;
                EmParameters.QaStatusSystemTypeCode = sysTypeList[caseDex];

                /* Initialize Input/Ouput Parameters */
                EmParameters.CurrentDailyCalStatus = null;
                EmParameters.InvalidDailyCalRecord = null;

                /* Initialize Output Parameters */
                EmParameters.PriorDailyCalEventRecord = null;


                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Initialize variables needed to run the check. */
                bool log = false;
                string actual;

                if (caseDex >= 0)
                {

                    /* Run Check */
                    actual = target.DCSTAT2(category, ref log);

                    /* Check Results */
                    Assert.AreEqual(string.Empty, actual, string.Format("actual {0}", caseDex));
                    Assert.AreEqual(false, log, string.Format("log {0}", caseDex));
                    Assert.AreEqual(null, category.CheckCatalogResult, string.Format("category.CheckCatalogResult {0}", caseDex));

                    Assert.AreEqual(statusList[caseDex], EmParameters.CurrentDailyCalStatus, string.Format("CurrentDailyCalStatus {0}", caseDex));
                    Assert.AreEqual(null, EmParameters.InvalidDailyCalRecord, string.Format("InvalidDailyCalRecord {0}", caseDex));
                    Assert.AreEqual(null, EmParameters.PriorDailyCalEventRecord, string.Format("PriorDailyCalEventRecord {0}", caseDex));
                }
            }
        }

        #endregion

    }
}
