using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Data.Ecmps.Dbo.View;
using ECMPS.Checks.Em.Parameters;
using ECMPS.Checks.EmissionsReport;

using ECMPS.Definitions.Extensions;

namespace UnitTest.Emissions
{
    [TestClass]
    public class StatusDeterminationTests
    {

        /// <summary>
        /// 
        /// 
        /// CurrHr: 2016-06-17 22
        /// CurrDt: 2016-06-17
        /// 
        /// | ## | CompDt     | PrgCd | UmcdDate | UmcbDate   | ErbDate    | PrgCd | UmcdDate | UmcBDate   | ErbDate    || Status         || Note
        /// |  0 | null       | null  | null     | null       | null       | null  | null     | null       | null       || EXPIRED        || No component begin date exists so defaulted as expired.
        /// |  1 | CurrDt-180 | null  | null     | null       | null       | null  | null     | null       | null       || MISSINGPROGRAM || MATS program does not exist.
        /// |  2 | CurrDt-180 | NONM  | CurrDt   | CurrDt-180 | CurrHr-179 | null  | null     | null       | null       || MISSINGPROGRAM || Only a non=MATS program exists.
        /// |  3 | CurrDt-180 | MATS  | CurrDt   | CurrDt-179 | CurrHr-178 | null  | null     | null       | null       || MISSINGPROGRAM || MATS program exists, but UMCB and ERB are after the Component Begin Date.
        /// |  4 | CurrDt-180 | MATS  | CurrDt   | CurrDt-180 | CurrHr-179 | null  | null     | null       | null       || EXPIRED        || MATS program exists because of the UMCB, but status is expired because of UMCD.
        /// |  5 | CurrDt-180 | MATS  | CurrDt   | CurrDt-179 | CurrHr-180 | null  | null     | null       | null       || EXPIRED        || MATS program exists because of the ERB, but status is expired because of UMCD.
        /// |  6 | CurrDt-170 | MATS  | null     | CurrDt-180 | CurrHr-169 | null  | null     | null       | null       || EXPIRED        || MATS program exists because of the UMCB, but status is expired because of UMCB + 180.
        /// |  7 | CurrDt-190 | MATS  | null     | CurrDt-180 | CurrHr-190 | null  | null     | null       | null       || EXPIRED        || MATS program exists because of the ERB, but status is expired because of UMCB + 180.
        /// |  8 | CurrDt-180 | MATS  | CurrDt+1 | CurrDt-180 | CurrHr-179 | null  | null     | null       | null       || VALID          || MATS program exists because of the UMCB, and status is valid because of UMCD.
        /// |  9 | CurrDt-180 | MATS  | CurrDt+1 | CurrDt-179 | CurrHr-180 | null  | null     | null       | null       || VALID          || MATS program exists because of the ERB, and status is valid because of UMCD.
        /// | 10 | CurrDt-170 | MATS  | null     | CurrDt-179 | CurrHr-169 | null  | null     | null       | null       || VALID          || MATS program exists because of the UMCB, and status is valid because of UMCB + 180.
        /// | 11 | CurrDt-190 | MATS  | null     | CurrDt-179 | CurrHr-190 | null  | null     | null       | null       || VALID          || MATS program exists because of the ERB, and status is valid because of UMCB + 180.
        /// | 12 | CurrDt-180 | MATS  | CurrDt   | CurrDt-181 | CurrHr-179 | MATS  | CurrDt+1 | CurrDt-180 | CurrHr-179 || VALID          || Two MATS programs exist, the program with later UMCB is used and status is valid because of UMCD.
        /// | 13 | CurrDt-180 | MATS  | CurrDt   | CurrDt-180 | CurrHr-179 | MATS  | CurrDt+1 | CurrDt-181 | CurrHr-179 || EXPIRED        || Two MATS programs exist and program with later UMCB is used, but status is expired because of UMCD.
        /// | 14 | CurrDt-180 | MATS  | CurrDt   | CurrDt-179 | CurrHr-180 | MATS  | CurrDt+1 | CurrDt-178 | CurrHr-180 || VALID          || Two MATS programs exist based on ERB, the program with later UMCB is used and status is valid because of UMCD.
        /// | 15 | CurrDt-180 | MATS  | CurrDt   | CurrDt-178 | CurrHr-180 | MATS  | CurrDt+1 | CurrDt-179 | CurrHr-180 || EXPIRED        || Two MATS programs exist based on ERB and program with later UMCB is used, but status is expired because of UMCD.
        /// | 16 | CurrDt-180 | MATS  | CurrDt   | CurrDt-178 | CurrHr-179 | MATS  | CurrDt+1 | CurrDt-179 | CurrHr-180 || VALID          || Two MATS programs exist, but the program with earlier UMCB used because program with later UMCB was not selected based on UMCB and ERB.  That program had a valid UMCD.
        /// | 17 | CurrDt-180 | MATS  | CurrDt   | CurrDt-179 | CurrHr-180 | MATS  | CurrDt+1 | CurrDt-178 | CurrHr-179 || EXPIRED        || Two MATS programs exist, but the program with earlier UMCB used because program with later UMCB was not selected based on UMCB and ERB.  That program had an invalid UMCD.
        [TestMethod]
        public void ConditionalDataEvent125ForMatsTests()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            /* Input Parameter Values */
            DateTime currHr = new DateTime(2016, 6, 17, 22, 0, 0);
            DateTime currDt = currHr.Date;

            DateTime?[] compBeginDtList = { null, currDt.AddDays(-180), currDt.AddDays(-180), currDt.AddDays(-180), currDt.AddDays(-180), currDt.AddDays(-180), currDt.AddDays(-170), currDt.AddDays(-190), currDt.AddDays(-180), currDt.AddDays(-180), currDt.AddDays(-170), currDt.AddDays(-190), currDt.AddDays(-180), currDt.AddDays(-180), currDt.AddDays(-180), currDt.AddDays(-180), currDt.AddDays(-180), currDt.AddDays(-180) };
            string[] prg1CodeList = { null, null, "NOXM", "MATS", "MATS", "MATS", "MATS", "MATS", "MATS", "MATS", "MATS", "MATS", "MATS", "MATS", "MATS", "MATS", "MATS", "MATS" };
            DateTime?[] prg1UmcdList = { null, null, currDt, currDt, currDt, currDt, null, null, currDt.AddDays(1), currDt.AddDays(1), null, null, currDt, currDt, currDt, currDt, currDt, currDt };
            DateTime?[] prg1UmcbList = { null, null, currDt.AddDays(-180), currDt.AddDays(-179), currDt.AddDays(-180), currDt.AddDays(-179), currDt.AddDays(-180), currDt.AddDays(-180), currDt.AddDays(-180), currDt.AddDays(-179), currDt.AddDays(-179), currDt.AddDays(-179), currDt.AddDays(-181), currDt.AddDays(-180), currDt.AddDays(-179), currDt.AddDays(-178), currDt.AddDays(-178), currDt.AddDays(-179) };
            DateTime?[] prg1ErbList = { null, null, currDt.AddDays(-179), currDt.AddDays(-178), currDt.AddDays(-179), currDt.AddDays(-180), currDt.AddDays(-169), currDt.AddDays(-190), currDt.AddDays(-179), currDt.AddDays(-180), currDt.AddDays(-169), currDt.AddDays(-190), currDt.AddDays(-179), currDt.AddDays(-179), currDt.AddDays(-180), currDt.AddDays(-180), currDt.AddDays(-179), currDt.AddDays(-180) };
            string[] prg2CodeList = { null, null, null, null, null, null, null, null, null, null, null, null, "MATS", "MATS", "MATS", "MATS", "MATS", "MATS" };
            DateTime?[] prg2UmcdList = { null, null, null, null, null, null, null, null, null, null, null, null, currDt.AddDays(1), currDt.AddDays(1), currDt.AddDays(1), currDt.AddDays(1), currDt.AddDays(1), currDt.AddDays(1) };
            DateTime?[] prg2UmcbList = { null, null, null, null, null, null, null, null, null, null, null, null, currDt.AddDays(-180), currDt.AddDays(-181), currDt.AddDays(-178), currDt.AddDays(-179), currDt.AddDays(-179), currDt.AddDays(-178) };
            DateTime?[] prg2ErbList = { null, null, null, null, null, null, null, null, null, null, null, null, currDt.AddDays(-179), currDt.AddDays(-179), currDt.AddDays(-180), currDt.AddDays(-180), currDt.AddDays(-180), currDt.AddDays(-179) };

            /* Expected Values */
            StatusDetermination.eConditionalDataStatus[] statusList =
                {
                    StatusDetermination.eConditionalDataStatus.EXPIRED,
                    StatusDetermination.eConditionalDataStatus.MISSINGPROGRAM,
                    StatusDetermination.eConditionalDataStatus.MISSINGPROGRAM,
                    StatusDetermination.eConditionalDataStatus.MISSINGPROGRAM,
                    StatusDetermination.eConditionalDataStatus.EXPIRED,
                    StatusDetermination.eConditionalDataStatus.EXPIRED,
                    StatusDetermination.eConditionalDataStatus.EXPIRED,
                    StatusDetermination.eConditionalDataStatus.EXPIRED,
                    StatusDetermination.eConditionalDataStatus.VALID,
                    StatusDetermination.eConditionalDataStatus.VALID,
                    StatusDetermination.eConditionalDataStatus.VALID,
                    StatusDetermination.eConditionalDataStatus.VALID,
                    StatusDetermination.eConditionalDataStatus.VALID,
                    StatusDetermination.eConditionalDataStatus.EXPIRED,
                    StatusDetermination.eConditionalDataStatus.VALID,
                    StatusDetermination.eConditionalDataStatus.EXPIRED,
                    StatusDetermination.eConditionalDataStatus.VALID,
                    StatusDetermination.eConditionalDataStatus.EXPIRED
                };

            /* Test Case Count */
            int caseCount = 18;

            /* Check array lengths */
            Assert.AreEqual(caseCount, compBeginDtList.Length, "compBeginDtList length");
            Assert.AreEqual(caseCount, prg1CodeList.Length, "prg1CodeList length");
            Assert.AreEqual(caseCount, prg1UmcdList.Length, "prg1UmcdList length");
            Assert.AreEqual(caseCount, prg1UmcbList.Length, "prg1UmcbList length");
            Assert.AreEqual(caseCount, prg1ErbList.Length, "prg1ErbList length");
            Assert.AreEqual(caseCount, prg2CodeList.Length, "prg2CodeList length");
            Assert.AreEqual(caseCount, prg2UmcdList.Length, "prg2UmcdList length");
            Assert.AreEqual(caseCount, prg2UmcbList.Length, "prg2UmcbList length");
            Assert.AreEqual(caseCount, prg2ErbList.Length, "prg2ErbList length");
            Assert.AreEqual(caseCount, statusList.Length, "statusList length");

            /* Run Cases */
            for (int caseDex = 0; caseDex < caseCount; caseDex++)
            {
                /*  Initialize Input Parameters*/
                EmParameters.CurrentDateHour = currHr;
                EmParameters.LocationProgramRecordsByHourLocation = new CheckDataView<VwMpLocationProgramRow>();
                {
                    EmParameters.LocationProgramRecordsByHourLocation.Add(new VwMpLocationProgramRow(prgCd: "ARP", unitMonitorCertDeadline: currDt, unitMonitorCertBeginDate: currDt.AddDays(-190), emissionsRecordingBeginDate: currDt.AddDays(-190)));
                    if (prg1CodeList[caseDex] != null) EmParameters.LocationProgramRecordsByHourLocation.Add(new VwMpLocationProgramRow(prgCd: prg1CodeList[caseDex], unitMonitorCertDeadline: prg1UmcdList[caseDex], unitMonitorCertBeginDate: prg1UmcbList[caseDex], emissionsRecordingBeginDate: prg1ErbList[caseDex]));
                    if (prg2CodeList[caseDex] != null) EmParameters.LocationProgramRecordsByHourLocation.Add(new VwMpLocationProgramRow(prgCd: prg2CodeList[caseDex], unitMonitorCertDeadline: prg2UmcdList[caseDex], unitMonitorCertBeginDate: prg2UmcbList[caseDex], emissionsRecordingBeginDate: prg2ErbList[caseDex]));
                    EmParameters.LocationProgramRecordsByHourLocation.Add(new VwMpLocationProgramRow(prgCd: "TRNOX", unitMonitorCertDeadline: currDt, unitMonitorCertBeginDate: currDt.AddDays(-190), emissionsRecordingBeginDate: currDt.AddDays(-190)));
                }
                EmParameters.QaStatusComponentBeginDate = compBeginDtList[caseDex];

                /* Run Method */
                StatusDetermination.eConditionalDataStatus status = StatusDetermination.ConditionalDataEvent125ForMats();

                /* Check Results */
                Assert.AreEqual(statusList[caseDex], status, string.Format("status {0}", caseDex));
            }
        }

        /// <summary>
        /// 
        /// 
        /// Case Notes:
        /// 
        /// Quarters
        ///   After : 2016 Q3
        ///   Curr  : 2016 Q2
        ///   Int-3 : 2016 Q1
        ///   Int-2 : 2015 Q4
        ///   Int-1 : 2015 Q3
        ///   Event : 2015 Q2
        ///   Prior : 2015 Q1
        ///   
        /// CurrHr  : 2016-06-17 22
        /// CondBQ  : 2015-04-01 00
        /// CondEQ  : 2015-06-30 23
        /// 
        /// Current Quarter Total : 169
        /// 
        /// | ## | CondHr    | Prior | Event | Int-1 | Int-2 | Int-3 |  Curr | EventBad | Int2Bad | SuppInd | SuppCnt || Status         || Min | Max || Note
        /// |  0 | null      |     0 |     0 |     0 |     0 |     0 |     0 | null     | null    | 0       |    null || EXPIRED        ||     |     || Conditional data begin date is null.
        /// |  1 | CondBQ    |     0 |     0 |     0 |     0 |     0 |   169 | null     | null    | 0       |    null || EXPIRED        ||     |     || Count of hours in current quarter and on or before the current hour is greater than 168.
        /// |  2 | CondBQ    |     0 |     0 |     0 |     0 |     1 |   168 | null     | null    | 0       |    null || EXPIRED        ||     |     || Count of hours on or before the current hour but after the event quarter is greater than 168.
        /// |  3 | CondBQ    |     0 |     0 |     0 |     1 |     0 |   168 | null     | null    | 0       |    null || EXPIRED        ||     |     || Count of hours on or before the current hour but the event quarter is greater than 168.
        /// |  4 | CondBQ    |     0 |     0 |     1 |     0 |     0 |   168 | null     | null    | 0       |    null || EXPIRED        ||     |     || Count of hours on or before the current hour but hours after the event quarter is greater than 168.
        /// |  5 | CondBQ    |     0 |     0 |     1 |     1 |     1 |   166 | null     | null    | 0       |    null || EXPIRED        ||     |     || Count of hours on or before the current hour but after the event quarter is greater than 168.
        /// |  6 | CondBQ    |     0 |     0 |    56 |    56 |    57 |    -1 | null     | null    | 0       |    null || EXPIRED        ||     |     || Problem in current quarter count, but count of hours in intervening quarters is greater than 168.
        /// |  7 | CondBQ    |     0 |     0 |    56 |  null |    57 |    56 | null     | null    | 0       |    null || EXPIRED        ||     |     || Problem with op supp data for a quarter, but count of hours after the event quarter (excluding the proglem quarter) is greater than 168.
        /// |  8 | CondBQ    |     0 |     0 |    56 |    56 |    56 |    -1 | null     | null    | 0       |    null || MISSINGACCUM   ||     |     || Problem in current quarter count, and the problem affects checking for expired.
        /// |  9 | CondBQ    |     0 |     0 |    56 |  null |    56 |    56 | null     | null    | 0       |    null || MISSINGOPSUPP  ||     |     || Problem with op supp data for a quarter, and the problem affects checking for expired.
        /// | 10 | CondBQ    |     0 |  null |     0 |     0 |     0 |     0 | null     | null    | 0       |    null || MISSINGOPSUPP  ||     |     || Problem with op supp data for event quarter, and the problem affects checking for expired.
        /// | 11 | CondEQ    |     0 |    33 |    34 |    34 |    34 |    33 | null     | null    | 0       |    null || VALID          || 135 | 136 || Total operating hours for involved quarters is less than or equal to 168, regardless of whether they fall after the condition data period begins.
        /// | 12 | CondBQ+31 |     0 |    33 |    42 |    42 |    42 |    41 | null     | null    | 0       |    null || EXPIRED        || 169 | 200 || Minimum possible opearting hours is greater than to 168.
        /// | 13 | CondBQ+32 |     0 |    33 |    42 |    42 |    42 |    41 | null     | null    | 0       |    null || UNDETERMINED   || 168 | 200 || Minimum possible opearting hours is less than or equal to 168 (and maximum possible will be greater than 168).
        /// | 14 | CondEQ    |     0 |    33 |    42 |    42 |    42 |    41 | null     | null    | 0       |    null || VALID          || 167 | 168 || Maximum possible opearting hours is less than or equal to 168.
        /// | 15 | CondEQ    |     0 |    33 |    42 |    42 |    42 |    40 | null     | null    | 0       |    null || VALID          || 166 | 167 || Maximum possible opearting hours is less than or equal to 168.
        /// | 16 | CondEQ-1  |     0 |    33 |    42 |    42 |    42 |    41 | null     | null    | 0       |    null || UNDETERMINED   || 167 | 169 || Maximum possible opearting hours is greater than to 168 (and miminum possible is less than or equal to 168).
        /// | 17 | CondEQ    |   169 |    33 |    34 |    34 |    34 |    33 | null     | null    | 0       |    null || VALID          || 135 | 136 || Valid condition not affected by value from extraneous quarter. (Total less than or equal to 168).
        /// | 18 | CondEQ    |   169 |    33 |    42 |    42 |    42 |    41 | null     | null    | 0       |    null || VALID          || 167 | 168 || Valid condition not affected by value from extraneous quarter. (Maximum possible less than or equal to 168).
        /// | 19 | CondEQ    |  null |    33 |    34 |    34 |    34 |    33 | null     | null    | 0       |    null || VALID          || 135 | 136 || Valid condition not affected by missing extraneous quarter. (Total less than or equal to 168).
        /// | 20 | CondEQ    |  null |    33 |    42 |    42 |    42 |    41 | null     | null    | 0       |    null || VALID          || 167 | 168 || Valid condition not affected by missing extraneous quarter. (Maximum possible less than or equal to 168).
        /// | 21 | CondBQ+32 |     0 |    33 |    42 |    42 |    42 |    41 | null     | null    | 1       |    null || UNDETERMINED   || 168 | 200 || Supplemental count is null and minimum possible opearting hours is less than or equal to 168 (and maximum possible will be greater than 168).
        /// | 22 | CondBQ+32 |     0 |    33 |    42 |    42 |    42 |    41 | null     | null    | 1       |       1 || VALID          || 168 | 200 || Supplemental count makes total 168 (VALID) and the existence of the supplemental count eliminates the undetermined state.
        /// | 23 | CondBQ+32 |     0 |    33 |    42 |    42 |    42 |    41 | null     | null    | 1       |       2 || EXPIRED        || 168 | 200 || Supplemental count makes total 169 (EXPIRED) and the existence of the supplemental count eliminates the undetermined state.
        /// </summary>
        [TestMethod()]
        public void ConditionalDataEvent120ForMats_DifferentQuarters()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            category.Process.ProcessParameters.RegisterParameter(1996, "Rpt_Period_Op_Hours_Accumulator_Array"); // Currently cannot access arrays using the new check parameter access.

            /* Input Parameter Values */
            DateTime currHr = new DateTime(2016, 6, 17, 22, 0, 0);
            DateTime condBQ = new DateTime(2015, 4, 1, 0, 0, 0);
            DateTime condEQ = new DateTime(2015, 6, 30, 23, 0, 0);
            DateTime priorBQ = condBQ.AddMonths(-3);
            

            DateTime?[] condHrList = { null, condBQ, condBQ, condBQ, condBQ, condBQ, condBQ, condBQ, condBQ, condBQ, condBQ, condEQ, condBQ.AddHours(31), condBQ.AddHours(32), condEQ, condEQ, condEQ.AddHours(-1), condEQ, condEQ, condEQ, condEQ, condBQ.AddHours(32), condBQ.AddHours(32), condBQ.AddHours(32) };
            int?[] qtrPriorList = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 169, 169, null, null, 0, 0, 0 };
            int?[] qtrEventList = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, null, 33, 33, 33, 33, 33, 33, 33, 33, 33, 33, 33, 33, 33 };
            int?[] qtrInt1List = { 0, 0, 0, 0, 1, 1, 56, 56, 56, 56, 0, 34, 42, 42, 42, 42, 42, 34, 42, 34, 42, 42, 42, 42 };
            int?[] qtrInt2List = { 0, 0, 0, 1, 0, 1, 56, null, 56, null, 0, 34, 42, 42, 42, 42, 42, 34, 42, 34, 42, 42, 42, 42 };
            int?[] qtrInt3List = { 0, 0, 1, 0, 0, 1, 57, 57, 56, 56, 0, 34, 42, 42, 42, 42, 42, 34, 42, 34, 42, 42, 42, 42 };
            int?[] qtrCurrList = { 0, 169, 168, 168, 168, 166, -1, 56, -1, 56, 0, 33, 41, 41, 41, 40, 41, 33, 41, 33, 41, 41, 41, 41 };
            string[] eventBadList = { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null };
            string[] int2BadList = { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null };
            int?[] suppInd = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1 };
            int?[] suppCnt = { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, 1, 2 };

            /* Expected Values */
            StatusDetermination.eConditionalDataStatus[] statusList =
                {
                    StatusDetermination.eConditionalDataStatus.EXPIRED,
                    StatusDetermination.eConditionalDataStatus.EXPIRED,
                    StatusDetermination.eConditionalDataStatus.EXPIRED,
                    StatusDetermination.eConditionalDataStatus.EXPIRED,
                    StatusDetermination.eConditionalDataStatus.EXPIRED,
                    StatusDetermination.eConditionalDataStatus.EXPIRED,
                    StatusDetermination.eConditionalDataStatus.EXPIRED,
                    StatusDetermination.eConditionalDataStatus.EXPIRED,
                    StatusDetermination.eConditionalDataStatus.MISSINGACCUM,
                    StatusDetermination.eConditionalDataStatus.MISSINGOPSUPP,
                    StatusDetermination.eConditionalDataStatus.MISSINGOPSUPP,
                    StatusDetermination.eConditionalDataStatus.VALID,
                    StatusDetermination.eConditionalDataStatus.EXPIRED,
                    StatusDetermination.eConditionalDataStatus.UNDETERMINED,
                    StatusDetermination.eConditionalDataStatus.VALID,
                    StatusDetermination.eConditionalDataStatus.VALID,
                    StatusDetermination.eConditionalDataStatus.UNDETERMINED,
                    StatusDetermination.eConditionalDataStatus.VALID,
                    StatusDetermination.eConditionalDataStatus.VALID,
                    StatusDetermination.eConditionalDataStatus.VALID,
                    StatusDetermination.eConditionalDataStatus.VALID,
                    StatusDetermination.eConditionalDataStatus.UNDETERMINED,
                    StatusDetermination.eConditionalDataStatus.VALID,
                    StatusDetermination.eConditionalDataStatus.EXPIRED
                };

            /* Test Case Count */
            int caseCount = 24;

            /* Check array lengths */
            Assert.AreEqual(caseCount, condHrList.Length, "condHrList length");
            Assert.AreEqual(caseCount, qtrPriorList.Length, "qtrPriorList length");
            Assert.AreEqual(caseCount, qtrEventList.Length, "qtrEventList length");
            Assert.AreEqual(caseCount, qtrInt1List.Length, "qtrInt1List length");
            Assert.AreEqual(caseCount, qtrInt2List.Length, "qtrInt2List length");
            Assert.AreEqual(caseCount, qtrInt3List.Length, "qtrInt3List length");
            Assert.AreEqual(caseCount, qtrCurrList.Length, "qtrCurrList length");
            Assert.AreEqual(caseCount, statusList.Length, "statusList length");
            Assert.AreEqual(caseCount, eventBadList.Length, "eventBadList length");
            Assert.AreEqual(caseCount, int2BadList.Length, "intr2BadList length");
            Assert.AreEqual(caseCount, suppInd.Length, "suppInd length");
            Assert.AreEqual(caseCount, suppCnt.Length, "suppCnt length");


            /* Run Cases */
            for (int caseDex = 0; caseDex < caseCount; caseDex++)
            {
                /*  Initialize Input Parameters*/
                EmParameters.CurrentDateHour = currHr;
                EmParameters.CurrentMonitorPlanLocationPostion = 1;
                EmParameters.OperatingSuppDataRecordsByLocation = new CheckDataView<VwMpOpSuppDataRow>();
                {
                    DateTime qtrBeginDate = priorBQ;

                    if (qtrPriorList[caseDex] != null) EmParameters.OperatingSuppDataRecordsByLocation.Add(new VwMpOpSuppDataRow(opValue: qtrPriorList[caseDex], opTypeCd: "OPHOURS", quarterBeginDate: qtrBeginDate, quarterEndDate: qtrBeginDate.AddMonths(3).AddDays(-1), quarterOrd: qtrBeginDate.QuarterOrd()));
                    qtrBeginDate = qtrBeginDate.AddMonths(3);
                    if (qtrEventList[caseDex] != null) EmParameters.OperatingSuppDataRecordsByLocation.Add(new VwMpOpSuppDataRow(opValue: qtrEventList[caseDex], opTypeCd: (eventBadList[caseDex] == null ? "OPHOURS" : "OPDAYS"), quarterBeginDate: qtrBeginDate, quarterEndDate: qtrBeginDate.AddMonths(3).AddDays(-1), quarterOrd: qtrBeginDate.QuarterOrd()));
                    qtrBeginDate = qtrBeginDate.AddMonths(3);
                    if (qtrInt1List[caseDex] != null) EmParameters.OperatingSuppDataRecordsByLocation.Add(new VwMpOpSuppDataRow(opValue: qtrInt1List[caseDex], opTypeCd: "OPHOURS", quarterBeginDate: qtrBeginDate, quarterEndDate: qtrBeginDate.AddMonths(3).AddDays(-1), quarterOrd: qtrBeginDate.QuarterOrd()));
                    qtrBeginDate = qtrBeginDate.AddMonths(3);
                    if (qtrInt2List[caseDex] != null) EmParameters.OperatingSuppDataRecordsByLocation.Add(new VwMpOpSuppDataRow(opValue: qtrInt2List[caseDex], opTypeCd: (int2BadList[caseDex] == null ? "OPHOURS" : "OPDAYS"), quarterBeginDate: qtrBeginDate, quarterEndDate: qtrBeginDate.AddMonths(3).AddDays(-1), quarterOrd: qtrBeginDate.QuarterOrd()));
                    qtrBeginDate = qtrBeginDate.AddMonths(3);
                    if (qtrInt3List[caseDex] != null) EmParameters.OperatingSuppDataRecordsByLocation.Add(new VwMpOpSuppDataRow(opValue: qtrInt3List[caseDex], opTypeCd: "OPHOURS", quarterBeginDate: qtrBeginDate, quarterEndDate: qtrBeginDate.AddMonths(3).AddDays(-1), quarterOrd: qtrBeginDate.QuarterOrd()));

                    /* Add 169 hours for the whole current quarter.  Added because the use of the current quarter was originally missed in prior test, but found in QA testing. */
                    qtrBeginDate = qtrBeginDate.AddMonths(3);
                    EmParameters.OperatingSuppDataRecordsByLocation.Add(new VwMpOpSuppDataRow(opValue: 169, opTypeCd: "OPHOURS", quarterBeginDate: qtrBeginDate, quarterEndDate: qtrBeginDate.AddMonths(3).AddDays(-1), quarterOrd: qtrBeginDate.QuarterOrd()));
                }
                EmParameters.SetCheckParameter("Rpt_Period_Op_Hours_Accumulator_Array", new int[] { 169, qtrCurrList[caseDex].Value, 169 }); // Currently cannot access arrays using the new check parameter access.

                /* Run Method */
                StatusDetermination.eConditionalDataStatus status = StatusDetermination.ConditionalDataEvent120ForMats(condHrList[caseDex], suppInd[caseDex], suppCnt[caseDex]);

                /* Check Results */
                Assert.AreEqual(statusList[caseDex], status, string.Format("status {0}", caseDex));
            }
        }

        /// <summary>
        /// 
        /// 
        /// Case Notes:
        /// 
        /// * Null OpHours will result in all hours being operating hours.  If CondHr is null, EdgeHr is used to begin the sequence.
        /// 
        /// CurrHr: 2016-06-17 22
        /// EdgeHr: CurrHr-167 { The first hour that results in a VALID if the hours on or between it and CurrHr are operating. }
        /// QtrBHr: 2016-04-01 00
        /// NonOp1: Indicates whether to make the hour after CondHr a non op hour. (Just avoiding all op hours between CondHr and CurrHr)
        /// NonOp2: Indicates whether to make the hour before CurrHr a non op hour. (Just avoiding all op hours between CondHr and CurrHr)
        /// 
        /// | ## | CondHr   | OpHours | NonOp1 | NonOp2 || Status         || Note
        /// |  0 | null     |    null | false  | false  || EXPIRED        || 168 hours on or between ConditionalBeginDateHour and CurrentDateHour.
        /// |  1 | EdgeHr   |    null | false  | false  || VALID          || 168 hours on or between ConditionalBeginDateHour and CurrentDateHour.
        /// |  2 | EdgeHr+1 |    null | false  | false  || VALID          || 167 hours on or between ConditionalBeginDateHour and CurrentDateHour.
        /// |  3 | EdgeHr-1 |    null | false  | false  || EXPIRED        || 169 hours on or between ConditionalBeginDateHour and CurrentDateHour, all operating.
        /// |  4 | EdgeHr-3 |     168 | true   | true   || VALID          || 168 opearting on or between ConditionalBeginDateHour and CurrentDateHour.
        /// |  5 | EdgeHr-3 |     167 | true   | true   || VALID          || 167 opearting on or between ConditionalBeginDateHour and CurrentDateHour.
        /// |  6 | EdgeHr-3 |     169 | true   | true   || EXPIRED        || 169 opearting on or between ConditionalBeginDateHour and CurrentDateHour.
        /// </summary>
        [TestMethod()]
        public void ConditionalDataEvent120ForMatsForMats_SameQuarter()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            /* Input Parameter Values */
            DateTime currHr = new DateTime(2016, 6, 17, 22, 0, 0);
            DateTime edgeHr = currHr.AddHours(-167);
            DateTime qtrBHr = new DateTime(2016, 4, 1, 0, 0, 0);
            DateTime qtrEHr = new DateTime(2016, 6, 30, 23, 0, 0);

            DateTime?[] condHrList = { null, edgeHr, edgeHr.AddHours(1), edgeHr.AddHours(-1), edgeHr.AddHours(-3), edgeHr.AddHours(-3), edgeHr.AddHours(-3) };
            int?[] OpHoursList = { null, null, null, null, 168, 167, 169 };
            bool[] nonOp1List = { false, false, false, false, true, true, true };
            bool[] nonOp2List = { false, false, false, false, true, true, true };

            /* Expected Values */
            StatusDetermination.eConditionalDataStatus[] statusList =
                {
                    StatusDetermination.eConditionalDataStatus.EXPIRED,
                    StatusDetermination.eConditionalDataStatus.VALID,
                    StatusDetermination.eConditionalDataStatus.VALID,
                    StatusDetermination.eConditionalDataStatus.EXPIRED,
                    StatusDetermination.eConditionalDataStatus.VALID,
                    StatusDetermination.eConditionalDataStatus.VALID,
                    StatusDetermination.eConditionalDataStatus.EXPIRED
                };

            /* Test Case Count */
            int caseCount = 7;

            /* Check array lengths */
            Assert.AreEqual(caseCount, condHrList.Length, "condHrList length");
            Assert.AreEqual(caseCount, OpHoursList.Length, "OpHoursList length");
            Assert.AreEqual(caseCount, nonOp1List.Length, "nonOp1List length");
            Assert.AreEqual(caseCount, nonOp2List.Length, "nonOp2List length");
            Assert.AreEqual(caseCount, statusList.Length, "statusList length");


            /* Run Cases */
            for (int caseDex = 0; caseDex < caseCount; caseDex++)
            {
                /*  Initialize Input Parameters*/
                EmParameters.CurrentDateHour = currHr;
                EmParameters.HourlyOperatingDataRecordsForLocation = new CheckDataView<VwMpHrlyOpDataRow>();
                {
                    int opHrCount = 2; // Start with two hours for conditional begin and current.  opHrCount is only used if conditional begin is not null.

                    for (DateTime hr = qtrBHr; hr <= qtrEHr; hr = hr.AddHours(1))
                    {
                        if (condHrList[caseDex] == null)
                        {
                            EmParameters.HourlyOperatingDataRecordsForLocation.Add(new VwMpHrlyOpDataRow(opTime: 0.1m, beginDatehour: hr, beginDate: hr.Date, beginHour: hr.Hour));
                        }
                        else if (hr < condHrList[caseDex])
                        {
                            EmParameters.HourlyOperatingDataRecordsForLocation.Add(new VwMpHrlyOpDataRow(opTime: 0.1m, beginDatehour: hr, beginDate: hr.Date, beginHour: hr.Hour));
                        }
                        else if (hr == condHrList[caseDex].Value)
                        {
                            EmParameters.HourlyOperatingDataRecordsForLocation.Add(new VwMpHrlyOpDataRow(opTime: 0.1m, beginDatehour: hr, beginDate: hr.Date, beginHour: hr.Hour));
                        }
                        else if ((hr > condHrList[caseDex].Value) && (hr < currHr))
                        {
                            if (nonOp1List[caseDex] && (hr == condHrList[caseDex].Value.AddHours(1)))
                            {
                                EmParameters.HourlyOperatingDataRecordsForLocation.Add(new VwMpHrlyOpDataRow(opTime: 0.0m, beginDatehour: hr, beginDate: hr.Date, beginHour: hr.Hour));
                            }
                            else if (nonOp2List[caseDex] && (hr == currHr.AddHours(-1)))
                            {
                                EmParameters.HourlyOperatingDataRecordsForLocation.Add(new VwMpHrlyOpDataRow(opTime: 0.0m, beginDatehour: hr, beginDate: hr.Date, beginHour: hr.Hour));
                            }
                            else if ((OpHoursList[caseDex] == null) || (opHrCount < OpHoursList[caseDex]))
                            {
                                EmParameters.HourlyOperatingDataRecordsForLocation.Add(new VwMpHrlyOpDataRow(opTime: 0.1m, beginDatehour: hr, beginDate: hr.Date, beginHour: hr.Hour));
                                opHrCount++;
                            }
                            else
                            {
                                EmParameters.HourlyOperatingDataRecordsForLocation.Add(new VwMpHrlyOpDataRow(opTime: 0.0m, beginDatehour: hr, beginDate: hr.Date, beginHour: hr.Hour));
                            }
                        }
                        else if (hr == currHr)
                        {
                            EmParameters.HourlyOperatingDataRecordsForLocation.Add(new VwMpHrlyOpDataRow(opTime: 0.1m, beginDatehour: hr, beginDate: hr.Date, beginHour: hr.Hour));
                        }
                        else // (hr > currHr)
                        {
                            EmParameters.HourlyOperatingDataRecordsForLocation.Add(new VwMpHrlyOpDataRow(opTime: 0.1m, beginDatehour: hr, beginDate: hr.Date, beginHour: hr.Hour));
                        }
                    }
                }

                /* Run Method */
                StatusDetermination.eConditionalDataStatus status = StatusDetermination.ConditionalDataEvent120ForMats(condHrList[caseDex], null, null);

                /* Check Results */
                Assert.AreEqual(statusList[caseDex], status, string.Format("status {0}", caseDex));
            }
        }
    }
}
