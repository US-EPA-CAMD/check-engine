using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.CheckEngine.SpecialParameterClasses;
using ECMPS.Checks.Data.Ecmps.CheckEm.Function;
using ECMPS.Checks.Data.Ecmps.Dbo.View;
using ECMPS.Checks.Em.Parameters;
using ECMPS.Checks.EmissionsChecks;


namespace UnitTest.Emissions
{
    [TestClass]
    public class WeeklySystemIntegrityStatusTest
    {

        #region Test Cases

        [TestMethod]
        public void WsiStat1()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            /* Initialize variables needed to run the check. */
            bool log = false;
            string actual;

            /* Initialize Output Parameters */
            EmParameters.WsiStatus = "Has Value";
            EmParameters.WsiPluginEventRecord = new VwQaCertEventRow();

            /* Init Cateogry Result */
            category.CheckCatalogResult = null;

            /* Run Check */
            actual = WeeklySystemIntegrityStatusChecks.WSISTAT1(category, ref log);

            /* Check results */
            Assert.AreEqual(string.Empty, actual);
            Assert.AreEqual(false, log);

            Assert.AreEqual(null, category.CheckCatalogResult, "Result");
            Assert.AreEqual(null, EmParameters.WsiStatus, "WsiStatus");
            Assert.AreEqual(null, EmParameters.WsiPluginEventRecord, "WsiPluginEventRecord");
        }


        [TestMethod]
        public void WsiStat2()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            /* Initialize General Variables */
            string componentExistingId = "GoodId";
            string componentMissingId = "OtherId";
            string[] componentIdList = { componentExistingId, componentMissingId };

            foreach (string componentId in componentIdList)
            {
                /*  Initialize Input Parameters*/
                EmParameters.QaStatusComponentId = componentId;

                EmParameters.WsiTestDictionary = new Dictionary<string, WsiTestStatusInformation>();
                {
                    WsiTestStatusInformation wsiTestStatusInformation = new WsiTestStatusInformation();
                    wsiTestStatusInformation.MostRecentTestRecord = new WeeklySystemIntegrity(componentId: componentExistingId);
                    EmParameters.WsiTestDictionary.Add(componentExistingId, wsiTestStatusInformation);
                }

                /* Initialize Output Parameters */
                EmParameters.WsiPriorTestRecord = new WeeklySystemIntegrity(componentId: componentMissingId);

                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Expected Values */
                string componentExpectedId = (componentId == componentExistingId) ? componentExistingId : null;

                /* Initialize variables needed to run the check. */
                bool log = false;
                string actual;

                /* Run Check */
                actual = WeeklySystemIntegrityStatusChecks.WSISTAT2(category, ref log);

                /* Check results */
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);

                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual(componentExpectedId, (EmParameters.WsiPriorTestRecord) != null ? EmParameters.WsiPriorTestRecord.ComponentId : null, "WsiPriorTestRecord");
            }
        }


        [TestMethod]
        public void WsiStat3_General()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            /* Initialize General Variables */
            string componentGoodId = "GoodId";
            string componentBadId = "OtherId";
            DateTime referenceDatehour = new DateTime(2015, 6, 17, 22, 0, 0);
            DateTime currentDatehour = referenceDatehour.AddHours(1);
            DateTime testDatehour = referenceDatehour.AddHours(-1);

            int caseCount = 7;
            string[] componentIdList = { componentGoodId, componentGoodId, componentBadId, componentBadId, componentBadId, componentGoodId, componentGoodId, componentGoodId, componentGoodId };
            DateTime[] eventDatehourList = { referenceDatehour, referenceDatehour, referenceDatehour, referenceDatehour, referenceDatehour, testDatehour, currentDatehour, referenceDatehour, referenceDatehour };
            string[] eventCdList = { "110", "130", "110", "110", "110", "110", "110", "140", "141" };
            string[] testResultCdList = { "PASSED", "PASSED", "FAILED", null, "PASSED", "PASSED", "PASSED", "PASSED", "PASSED" };
            bool[] recordFoundList = { true, true, false, false, false, false, false, false, false };
            string[] wsiStatusList = { "OOC-Event", "OOC-Event", "OOC-Test Failed", "OOC-Test Has Critical Errors", "IC", "IC", "IC", "IC", "IC" };

            for (int caseDex = 0; caseDex < caseCount; caseDex++)
            {
                /*  Initialize Input Parameters*/
                EmParameters.CurrentDateHour = currentDatehour;
                EmParameters.QaStatusComponentId = componentGoodId;
                EmParameters.WsiPriorTestRecord = new WeeklySystemIntegrity(testDatehour: testDatehour, testResultCd: testResultCdList[caseDex]);

                EmParameters.WsiTestDictionary = new Dictionary<string, WsiTestStatusInformation>();
                {
                    EmParameters.WsiTestDictionary.Add(componentBadId, new WsiTestStatusInformation());
                    EmParameters.WsiTestDictionary.Add(componentGoodId, new WsiTestStatusInformation());
                }

                EmParameters.QaCertificationEventRecords = new CheckDataView<VwQaCertEventRow>
                (
                  new VwQaCertEventRow
                  (
                    componentId: componentIdList[caseDex],
                    qaCertEventDatehour: eventDatehourList[caseDex],
                    qaCertEventDate: eventDatehourList[caseDex].Date,
                    qaCertEventHour: eventDatehourList[caseDex].Hour,
                    qaCertEventCd: eventCdList[caseDex]
                  )
                );

                /* Initialize Output Parameters */
                EmParameters.WsiInterveningEventRecord = new VwQaCertEventRow(componentId: componentBadId);
                EmParameters.WsiPluginEventRecord = null;
                EmParameters.WsiStatus = null;

                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Initialize variables needed to run the check. */
                bool log = false;
                string actual;

                /* Run Check */
                actual = WeeklySystemIntegrityStatusChecks.WSISTAT3(category, ref log);

                /* Check results */
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);

                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual(recordFoundList[caseDex], (EmParameters.WsiInterveningEventRecord != null), "WsiInterveningEventRecord");
                Assert.AreEqual(wsiStatusList[caseDex], EmParameters.WsiStatus, "WsiStatus");

                if (wsiStatusList[caseDex] == "OOC-Event")
                    Assert.AreNotEqual(null, EmParameters.WsiPluginEventRecord, "WsiPluginEventRecord");
                else
                    Assert.AreEqual(null, EmParameters.WsiPluginEventRecord, "WsiPluginEventRecord");
            }
        }


        [TestMethod]
        public void WsiStat3_Expired()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            /* Initialize General Variables */
            string componentGoodId = "GoodId";
            DateTime referenceDatehour = new DateTime(2015, 6, 17, 22, 0, 0);
            DateTime currentDatehour = referenceDatehour.AddHours(1);
            DateTime testDatehour = referenceDatehour.AddHours(-8);

            int caseCount = 4;
            string[] initialStatusList = { null, null, null, "OOC-Other" };
            string[] componentIdList = { componentGoodId, componentGoodId, componentGoodId, componentGoodId };
            List<DateTime>[] operatingDateLists = new List<DateTime>[]
            {
        null,
        new List<DateTime> { referenceDatehour, referenceDatehour.AddHours(-1), referenceDatehour.AddHours(-2), referenceDatehour.AddHours(-3), referenceDatehour.AddHours(-4), referenceDatehour.AddHours(-5), referenceDatehour.AddHours(-6) },
        new List<DateTime> { referenceDatehour, referenceDatehour.AddHours(-1), referenceDatehour.AddHours(-2), referenceDatehour.AddHours(-3), referenceDatehour.AddHours(-4), referenceDatehour.AddHours(-5), referenceDatehour.AddHours(-6), referenceDatehour.AddHours(-7) },
        null
            };
            string[] wsiStatusList = { "IC", "IC", "OOC-Expired", "OOC-Other" };

            for (int caseDex = 0; caseDex < caseCount; caseDex++)
            {
                /*  Initialize Input Parameters*/
                EmParameters.CurrentDateHour = currentDatehour;
                EmParameters.QaCertificationEventRecords = new CheckDataView<VwQaCertEventRow>();
                EmParameters.QaStatusComponentId = componentGoodId;
                EmParameters.WsiPriorTestRecord = new WeeklySystemIntegrity(testDatehour: testDatehour, testResultCd: "PASSED");

                EmParameters.WsiTestDictionary = new Dictionary<string, WsiTestStatusInformation>();
                {
                    WsiTestStatusInformation wsiTestStatusInformation = new WsiTestStatusInformation();
                    wsiTestStatusInformation.OperatingDateList = operatingDateLists[caseDex];
                    EmParameters.WsiTestDictionary.Add(componentGoodId, wsiTestStatusInformation);
                }

                /* Initialize Input/Output Parameters */
                EmParameters.WsiStatus = initialStatusList[caseDex];

                /* Initialize Output Parameters */
                EmParameters.WsiInterveningEventRecord = new VwQaCertEventRow(componentId: componentGoodId);

                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Initialize variables needed to run the check. */
                bool log = false;
                string actual;

                /* Run Check */
                actual = WeeklySystemIntegrityStatusChecks.WSISTAT3(category, ref log);

                /* Check results */
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);

                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual(null, EmParameters.WsiInterveningEventRecord, "WsiInterveningEventRecord");
                Assert.AreEqual(wsiStatusList[caseDex], EmParameters.WsiStatus, "WsiStatus");
            }
        }


        [TestMethod]
        public void WsiStat4()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            /* Initialize General Variables */
            int caseCount = 7;
            string[] wsiStatusList = { "IC", "IC-Undetermined", "OOC-Event", "OOC-Expired", "OOC-No Prior Test", "OOC-Test Failed", "OOC-Test Has Critical Errors" };
            string[] checkResultList = { null, null, "OOC-Event", "OOC-Expired", "OOC-No Prior Test", "OOC-Test Failed", "OOC-Test Has Critical Errors" };

            for (int caseDex = 0; caseDex < caseCount; caseDex++)
            {
                /*  Initialize Input Parameters*/
                EmParameters.WsiStatus = wsiStatusList[caseDex];

                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Initialize variables needed to run the check. */
                bool log = false;
                string actual;

                /* Run Check */
                actual = WeeklySystemIntegrityStatusChecks.WSISTAT4(category, ref log);

                /* Check results */
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);

                Assert.AreEqual(checkResultList[caseDex], category.CheckCatalogResult, "Result");
            }
        }

        /// <summary>
        /// WsiStat-5
        /// 
        /// PrgCd : MATS
        /// 
        /// AncHr : 2014-06-17 22
        /// OocHr : 2015-06-06 00
        /// OldHr : 2015-06-01 00
        /// IncHr : 2015-06-07 23
        /// RefHr : 2015-06-17 22
        /// CurHr : 2015-06-17 23
        /// 
        /// DateList: 2015-06-06, 2015-06-07, 2015-06-08, 2015-06-10, 2015-06-11, 2015-06-12, 2015-06-14, 2015-06-15, and 2015-06-16
        /// 
        /// | ## | CompId  | EventCd | EventHr | TestHr    | CompHr | ErbDt || Status            | Found || Note
        /// |  0 | GoodId  | 140     | RefHr   | null      | OldHr  | AncHr || IC-Undetermined   | true  || Event exists and operating days after event date is less than or equal to 0.
        /// |  1 | GoodId  | 141     | RefHr   | null      | OldHr  | AncHr || IC-Undetermined   | true  || Event exists and operating days after event date is less than or equal to 0.
        /// |  2 | GoodId  | 140     | IncHr   | null      | OldHr  | AncHr || IC-Undetermined   | true  || Event exists and operating days after event date is less than or equal to 7.
        /// |  3 | GoodId  | 140     | OocHr   | null      | OldHr  | AncHr || OOC-No Prior Test | true  || Event exists and operating days after event date is less than or equal to 8.
        /// |  4 | OtherId | 140     | RefHr   | null      | RefHr  | AncHr || IC-Undetermined   | false || Event does not exist due to CompId, Test does not exist.  Count of operating days after earliest op date is 0.
        /// |  5 | OtherId | 140     | RefHr   | null      | IncHr  | AncHr || IC-Undetermined   | false || Event does not exist due to CompId, Test does not exist.  Count of operating days after earliest op date is 7.
        /// |  6 | OtherId | 140     | RefHr   | null      | OocHr  | AncHr || OOC-No Prior Test | false || Event does not exist due to CompId, Test does not exist.  Count of operating days after earliest op date is 8.
        /// |  7 | GoodId  | 140     | CurHr   | null      | RefHr  | AncHr || IC-Undetermined   | false || Event does not exist due to EventHr equal to Current Hour.  Count of operating days after earliest op date is 0.
        /// |  8 | GoodId  | 110     | RefHr   | null      | RefHr  | AncHr || IC-Undetermined   | false || Event does not exist due to EventCd equal to Current Hour.  Count of operating days after earliest op date is 0.
        /// |  9 | GoodId  | 130     | RefHr   | null      | RefHr  | AncHr || IC-Undetermined   | false || Event does not exist due to EventCd equal to Current Hour.  Count of operating days after earliest op date is 0.
        /// | 10 | GoodId  | 140     | IncHr   | IncHr - 1 | OldHr  | AncHr || IC-Undetermined   | true  || Event exists (occurred after prior test).  Count of days after event date is 7.
        /// | 11 | GoodId  | 140     | OocHr   | OocHr - 1 | OldHr  | AncHr || OOC-Event         | true  || Event exists (occurred after prior test).  Count of days after event date is 8.
        /// | 12 | GoodId  | 140     | IncHr   | IncHr     | OldHr  | AncHr || null              | false || Event does not exist due to EventHr during same hour as EndHr of the Prior Test.  No additional checking.
        /// | 13 | GoodId  | 140     | OocHr   | OocHr     | OldHr  | AncHr || null              | false || Event does not exist due to EventHr during same hour as EndHr of the Prior Test.  No additional checking.
        /// | 14 | OtherId | 140     | RefHr   | null      | OocHr  | OocHr || OOC-No Prior Test | false || Event does not exist due to CompId, Test does not exist.  Count of operating days after earliest op date is 8.
        /// | 15 | OtherId | 140     | RefHr   | null      | OocHr  | IncHr || IC-Undetermined   | false || Event does not exist due to CompId, Test does not exist.  Count of operating days after earliest op date is 7 because of ERB Date good and after Comp Begin Date.
        /// | 16 | OtherId | 140     | RefHr   | null      | IncHr  | OocHr || IC-Undetermined   | false || Event does not exist due to CompId, Test does not exist.  Count of operating days after earliest op date is 7 because of Comp Begin Date is good and after ERB.
        /// | 17 | OtherId | 140     | RefHr   | null      | OocHr  | CurHr || IC-Undetermined   | false || Event does not exist due to CompId, Test does not exist.  Count of operating days after earliest op date is 0 because ERB equals current date.
        /// | 18 | OtherId | 140     | RefHr   | null      | OocHr  | null  || OOC-No Prior Test | false || Event does not exist due to CompId, Test does not exist.  Count of operating days after earliest op date is 8 because MATS ERB does not exist and Comp Begin Date is used.
        /// | 19 | OtherId | 140     | RefHr   | null      | IncHr  | null  || IC-Undetermined   | false || Event does not exist due to CompId, Test does not exist.  Count of operating days after earliest op date is 7 because MATS ERB does not exist and Comp Begin Date is used.
        /// </summary>
        [TestMethod]
        public void WsiStat5()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            /* Initialize General Variables */
            string componentGoodId = "GoodId";
            string componentOtherId = "OtherId";
            string[] prgMats1 = new string[] { "MATS" };
            string[] prgMats2 = new string[] { "MATS", "MATS" };
            DateTime referenceDatehour = new DateTime(2015, 6, 17, 22, 0, 0);
            DateTime currentDatehour = referenceDatehour.AddHours(1);
            DateTime icDatehour = new DateTime(2015, 6, 7, 23, 0, 0);
            DateTime oocDatehour = new DateTime(2015, 6, 6, 0, 0, 0);
            DateTime oldDatehour = new DateTime(2015, 6, 1, 0, 0, 0);
            DateTime ancDatehour = new DateTime(2014, 6, 17, 22, 0, 0);
            List<DateTime> dateList = new List<DateTime>
            {
                new DateTime(2015, 6, 6),
                new DateTime(2015, 6, 7),
                new DateTime(2015, 6, 8),
                new DateTime(2015, 6, 10),
                new DateTime(2015, 6, 11),
                new DateTime(2015, 6, 12),
                new DateTime(2015, 6, 14),
                new DateTime(2015, 6, 15),
                new DateTime(2015, 6, 16)
            };
            DateTime?[] ancDtArray = new DateTime?[] { ancDatehour.Date };
            DateTime?[] curDtArray = new DateTime?[] { currentDatehour.Date };
            DateTime?[] incDtArray = new DateTime?[] { icDatehour.Date };
            DateTime?[] nullDtArray = new DateTime?[] { null };
            DateTime?[] oocDtArray = new DateTime?[] { oocDatehour.Date };

            /* Test Case Count */
            int caseCount = 20;

            /* Input Parameter Values */
            string[] componentIdList = { componentGoodId, componentGoodId, componentGoodId, componentGoodId, componentOtherId, componentOtherId, componentOtherId, componentGoodId, componentGoodId, componentGoodId, componentGoodId, componentGoodId, componentGoodId, componentGoodId, componentOtherId, componentOtherId, componentOtherId, componentOtherId, componentOtherId, componentOtherId };
            string[] eventCdList = { "140", "141", "140", "140", "140", "140", "140", "140", "110", "130", "140", "140", "140", "140", "140", "140", "140", "140", "140", "140" };
            DateTime[] eventDatehourList = { referenceDatehour, referenceDatehour, icDatehour, oocDatehour, referenceDatehour, referenceDatehour, referenceDatehour, currentDatehour, referenceDatehour, referenceDatehour, icDatehour, oocDatehour, icDatehour, oocDatehour, referenceDatehour, referenceDatehour, referenceDatehour, referenceDatehour, referenceDatehour, referenceDatehour };
            DateTime?[] wsiPriorTestDateList = { null, null, null, null, null, null, null, null, null, null, icDatehour.AddHours(-1), oocDatehour.AddHours(-1), icDatehour, oocDatehour, null, null, null, null, null, null };
            DateTime[] componentDatehourList = { oldDatehour, oldDatehour, oldDatehour, oldDatehour, referenceDatehour, icDatehour, oocDatehour, referenceDatehour, referenceDatehour, referenceDatehour, oldDatehour, oldDatehour, oldDatehour, oldDatehour, oocDatehour, oocDatehour, icDatehour, oocDatehour, oocDatehour, icDatehour };
            DateTime?[] erbDtList = { ancDatehour.Date, ancDatehour.Date, ancDatehour.Date, ancDatehour.Date, ancDatehour.Date, ancDatehour.Date, ancDatehour.Date, ancDatehour.Date, ancDatehour.Date, ancDatehour.Date, ancDatehour.Date, ancDatehour.Date, ancDatehour.Date, ancDatehour.Date, oocDatehour.Date, icDatehour.Date, oocDatehour.Date, currentDatehour, null, null };

            /* Expected Values */
            bool[] recordFoundList = { true, true, true, true, false, false, false, false, false, false, true, true, false, false, false, false, false, false, false, false };
            string[] wsiStatusList = { "IC-Undetermined", "IC-Undetermined", "IC-Undetermined", "OOC-No Prior Test", "IC-Undetermined", "IC-Undetermined", "OOC-No Prior Test", "IC-Undetermined", "IC-Undetermined", "IC-Undetermined", "IC-Undetermined", "OOC-Event", null, null, "OOC-No Prior Test", "IC-Undetermined", "IC-Undetermined", "IC-Undetermined", "OOC-No Prior Test", "IC-Undetermined" };

            /* Check array lengths */
            Assert.AreEqual(caseCount, componentIdList.Length, "componentIdList length");
            Assert.AreEqual(caseCount, eventDatehourList.Length, "eventDatehourList length");
            Assert.AreEqual(caseCount, componentDatehourList.Length, "componentDatehourList length");
            Assert.AreEqual(caseCount, eventCdList.Length, "eventCdList length");
            Assert.AreEqual(caseCount, wsiPriorTestDateList.Length, "wsiPriorTestDateList length");
            Assert.AreEqual(caseCount, erbDtList.Length, "erbDtList length");
            Assert.AreEqual(caseCount, recordFoundList.Length, "recordFoundList length");
            Assert.AreEqual(caseCount, wsiStatusList.Length, "wsiStatusList length");

            /* Run Cases */
            for (int caseDex = 0; caseDex < caseCount; caseDex++)
            {
                /*  Initialize Input Parameters*/
                EmParameters.CurrentDateHour = currentDatehour;
                EmParameters.CurrentMonitorPlanLocationPostion = 1;
                EmParameters.OperatingDateArray = new List<DateTime>[] { new List<DateTime>(), dateList, new List<DateTime>() };
                EmParameters.QaStatusComponentBeginDate = componentDatehourList[caseDex].Date;
                EmParameters.QaStatusComponentId = componentGoodId;
                EmParameters.QaStatusMatsErbDate = erbDtList[caseDex];
                EmParameters.WsiPriorTestRecord = (wsiPriorTestDateList[caseDex] != null ? new WeeklySystemIntegrity(testDatetime: wsiPriorTestDateList[caseDex].Value, testDatehour: wsiPriorTestDateList[caseDex].Value, testDate: wsiPriorTestDateList[caseDex].Value.Date) : null);
                EmParameters.QaCertificationEventRecords = new CheckDataView<VwQaCertEventRow>
                (
                  new VwQaCertEventRow
                  (
                    componentId: componentIdList[caseDex],
                    qaCertEventDatehour: eventDatehourList[caseDex],
                    qaCertEventDate: eventDatehourList[caseDex].Date,
                    qaCertEventHour: eventDatehourList[caseDex].Hour,
                    qaCertEventCd: eventCdList[caseDex]
                  )
                );

                /* Initialize Output Parameters */
                EmParameters.WsiInterveningLikeKindEventRecord = new VwQaCertEventRow(componentId: componentOtherId);
                EmParameters.WsiPluginEventRecord = null;
                EmParameters.WsiStatus = null;

                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Initialize variables needed to run the check. */
                bool log = false;
                string actual;

                if (caseDex >= 0)
                {
                    /* Run Check */
                    actual = WeeklySystemIntegrityStatusChecks.WSISTAT5(category, ref log);

                    /* Check results */
                    Assert.AreEqual(string.Empty, actual, string.Format("actual {0}", caseDex));
                    Assert.AreEqual(false, log, string.Format("log {0}", caseDex));

                    Assert.AreEqual(null, category.CheckCatalogResult, String.Format("Result [case {0}]", caseDex));
                    Assert.AreEqual(recordFoundList[caseDex], (EmParameters.WsiInterveningLikeKindEventRecord != null), String.Format("WsiInterveningLikeKindEventRecord [case {0}]", caseDex));
                    Assert.AreEqual(wsiStatusList[caseDex], EmParameters.WsiStatus, String.Format("WsiStatus [case {0}]", caseDex));

                    if (wsiStatusList[caseDex] == "OOC-Event")
                        Assert.AreNotEqual(null, EmParameters.WsiPluginEventRecord, String.Format("WsiPluginEventRecord [case {0}]", caseDex));
                    else
                        Assert.AreEqual(null, EmParameters.WsiPluginEventRecord, String.Format("WsiPluginEventRecord [case {0}]", caseDex));
                }
            }
        }

        #endregion

    }
}
