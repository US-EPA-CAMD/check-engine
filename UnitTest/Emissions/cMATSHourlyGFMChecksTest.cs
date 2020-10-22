using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.CheckEngine.SpecialParameterClasses;
using ECMPS.Checks.Data.Ecmps.CheckEm.Function;
using ECMPS.Checks.Data.Ecmps.Dbo.View;
using ECMPS.Checks.Em.Parameters;
using ECMPS.Checks.EmissionsChecks;
using ECMPS.Checks.EmissionsReport;
using ECMPS.Definitions.Extensions;

using UnitTest.UtilityClasses;


namespace UnitTest.Emissions
{
    [TestClass()]
    public class cMATSHourlyGFMChecksTest
    {
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            //populates Reporting Period Table for checks without making db call
            UnitTest.UtilityClasses.UnitTestExtensions.SetReportingPeriodTable();
        }

        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        #region Test Cases

        /// <summary>
        /// 
        /// Train Ids:
        /// Train1 - UNI
        /// Train2 - DUO
        /// Train3 - TRE
        /// Border - BOR
        /// 
        /// CurrHr: 2016-06-17 22
        /// 
        ///               |         Sampling Train 1         |         Sampling Train 2         |         Sampling Train 3         |          Boarder Train           ||
        /// | ## | CompId | CompId | BeginHr    | EndHr      | CompId | BeginHr    | EndHr      | CompId | BeginHr    | EndHr      | Border | BeginHr    | EndHr      || Result | Valid | Count | TrainPos || Notes
        /// |  0 | null   | GoodId | CurrHr - 1 | CurrHr + 1 |        |            |            |        |            |            |        |            |            || A      | false | null  | null     || GFM Component Id is null, which prevents further checking and therefore returns result A.
        /// |  1 | GoodId | GoodId | CurrHr - 1 | CurrHr + 1 |        |            |            |        |            |            |        |            |            || null   | true  | 1     | [0]      || One matching sampling train exists and is selected as the GFM train.
        /// |  2 | GoodId | BadId  | CurrHr - 1 | CurrHr + 1 |        |            |            |        |            |            |        |            |            || B      | false | 0     | null     || Sampling train does not match because of different component id, which returns result B.
        /// |  3 | GoodId | GoodId | CurrHr - 2 | CurrHr     |        |            |            |        |            |            |        |            |            || null   | true  | 1     | [0]      || One matching sampling train exists and is selected as the GFM train.
        /// |  4 | GoodId | GoodId | CurrHr     | CurrHr + 2 |        |            |            |        |            |            |        |            |            || null   | true  | 1     | [0]      || One matching sampling train exists and is selected as the GFM train.
        /// |  5 | GoodId | GoodId | CurrHr - 2 | CurrHr - 1 |        |            |            |        |            |            |        |            |            || B      | false | 0     | null     || Sampling train does not match because of end date, which returns result B.
        /// |  6 | GoodId | GoodId | CurrHr + 1 | CurrHr + 2 |        |            |            |        |            |            |        |            |            || B      | false | 0     | null     || Sampling train does not match because of begin date, which returns result B.
        /// |  7 | GoodId |        |            |            |        |            |            |        |            |            | true   | CurrHr - 1 | CurrHr + 1 || B      | false | 0     | null     || Sampling train does not exist so result B is returned event though a border trap exists.
        /// |  8 | GoodId |        |            |            |        |            |            |        |            |            | false  | CurrHr - 1 | CurrHr + 1 || B      | false | 0     | null     || Sampling train does not exist, but border trap existing trap is not a border trap so result B is returned.
        /// |  9 | GoodId |        |            |            |        |            |            |        |            |            | true   | CurrHr - 2 | CurrHr     || B      | false | 0     | null     || Sampling train does not exist so result B is returned event though a border trap exists.
        /// | 10 | GoodId |        |            |            |        |            |            |        |            |            | true   | CurrHr - 2 | CurrHr - 1 || B      | false | 0     | null     || Sampling train does not exist, but border trap does not cover current hour, so result B is returned.
        /// | 11 | GoodId |        |            |            |        |            |            |        |            |            | true   | CurrHr     | CurrHr + 2 || B      | false | 0     | null     || Sampling train does not exist so result B is returned event though a border trap exists.
        /// | 12 | GoodId |        |            |            |        |            |            |        |            |            | true   | CurrHr - 2 | CurrHr - 1 || B      | false | 0     | null     || Sampling train does not exist, but border trap does not cover current hour, so result B is returned.
        /// | 13 | GoodId | GoodId | CurrHr - 2 | CurrHr - 1 | GoodId | CurrHr     | CurrHr     | GoodId | CurrHr + 1 | CurrHr + 2 |        |            |            || null   | true  | 1     | [1]      || One matching sampling train exists and is selected as the GFM train.
        /// | 14 | GoodId | GoodId | CurrHr - 2 | CurrHr     | GoodId | CurrHr     | CurrHr     | GoodId | CurrHr     | CurrHr + 2 |        |            |            || null   | true  | 3     | [0,1,2]  || One matching sampling train exists and is selected as the GFM train.
        /// </summary>
        [TestMethod()]
        public void MatsGfm1()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            /* Input Parameter Values */
            string goodId = "GoodId";
            string badId = "BadId";

            DateTime currHr = new DateTime(2016, 6, 17, 22, 0, 0);
            DateTime currM2 = currHr.AddHours(-2);
            DateTime currM1 = currHr.AddHours(-1);
            DateTime currP1 = currHr.AddHours(1);
            DateTime currP2 = currHr.AddHours(2);

            string[] gfmCompIdList = { null, goodId, goodId, goodId, goodId, goodId, goodId, goodId, goodId, goodId, goodId, goodId, goodId, goodId, goodId };
            string[] uniCompIdList = { goodId, goodId, badId, goodId, goodId, goodId, goodId, null, null, null, null, null, null, goodId, goodId };
            DateTime?[] uniBeginHrList = { currM1, currM1, currM1, currM2, currHr, currM2, currP1, null, null, null, null, null, null, currM2, currM2 };
            DateTime?[] uniEndHrList = { currP1, currP1, currP1, currHr, currP2, currM1, currP2, null, null, null, null, null, null, currM1, currHr };
            string[] duoCompIdList = { null, null, null, null, null, null, null, null, null, null, null, null, null, goodId, goodId };
            DateTime?[] duoBeginHrList = { null, null, null, null, null, null, null, null, null, null, null, null, null, currHr, currHr };
            DateTime?[] duoEndHrList = { null, null, null, null, null, null, null, null, null, null, null, null, null, currHr, currHr };
            string[] treCompIdList = { null, null, null, null, null, null, null, null, null, null, null, null, null, goodId, goodId };
            DateTime?[] treBeginHrList = { null, null, null, null, null, null, null, null, null, null, null, null, null, currP1, currHr };
            DateTime?[] treEndHrList = { null, null, null, null, null, null, null, null, null, null, null, null, null, currP2, currP2 };
            bool?[] borIsBorderList = { null, null, null, null, null, null, null, true, false, true, true, true, true, null, null };
            DateTime?[] borBeginHrList = { null, null, null, null, null, null, null, currM1, currM1, currM2, currM2, currHr, currM2, null, null };
            DateTime?[] borEndHrList = { null, null, null, null, null, null, null, currP1, currP1, currHr, currM1, currP2, currM1, null, null };

            /* Expected Values */
            string[] expResultList = { "A", null, "B", null, null, "B", "B", "B", "B", "B", "B", "B", "B", null, null };
            bool?[] expValidList = { false, true, false, true, true, false, false, false, false, false, false, false, false, true, true };
            int?[] expCountList = { null, 1, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 3 };
            int[][] expTrainPositions = { null, new int[] { 0 }, null, new int[] { 0 }, new int[] { 0 }, null, null, null, null, null, null, null, null, new int[] { 1 }, new int[] { 0, 1, 2 } };

            /* Test Case Count */
            int caseCount = 15;

            /* Check array lengths */
            Assert.AreEqual(caseCount, gfmCompIdList.Length, "gfmCompIdList length");
            Assert.AreEqual(caseCount, uniCompIdList.Length, "uniCompIdList length");
            Assert.AreEqual(caseCount, uniBeginHrList.Length, "uniBeginHrList length");
            Assert.AreEqual(caseCount, uniEndHrList.Length, "uniEndHrList length");
            Assert.AreEqual(caseCount, duoCompIdList.Length, "duoCompIdList length");
            Assert.AreEqual(caseCount, duoBeginHrList.Length, "duoBeginHrList length");
            Assert.AreEqual(caseCount, duoEndHrList.Length, "duoEndHrList length");
            Assert.AreEqual(caseCount, treCompIdList.Length, "treCompIdList length");
            Assert.AreEqual(caseCount, treBeginHrList.Length, "treBeginHrList length");
            Assert.AreEqual(caseCount, treEndHrList.Length, "treEndHrList length");
            Assert.AreEqual(caseCount, borIsBorderList.Length, "borIsBorderList length");
            Assert.AreEqual(caseCount, borBeginHrList.Length, "borBeginHrList length");
            Assert.AreEqual(caseCount, borEndHrList.Length, "borEndHrList length");
            Assert.AreEqual(caseCount, expResultList.Length, "expResultList length");
            Assert.AreEqual(caseCount, expValidList.Length, "expValidList length");
            Assert.AreEqual(caseCount, expCountList.Length, "expCountList length");
            Assert.AreEqual(caseCount, expTrainPositions.Length, "expTrainPositions length");


            /* Run Cases */
            for (int caseDex = 0; caseDex < caseCount; caseDex++)
            {
                /* Initialize Input Parameters */
                EmParameters.CurrentDateHour = currHr;
                EmParameters.CurrentMonitorPlanLocationPostion = 1;
                EmParameters.MatsHourlyGfmRecord = new MatsHourlyGfmRecord(componentId: gfmCompIdList[caseDex]);
                EmParameters.MatsSamplingTrainRecords = new CheckDataView<MatsSamplingTrainRecord>();
                {
                    if (uniCompIdList[caseDex].HasValue())
                        EmParameters.MatsSamplingTrainRecords.Add(new MatsSamplingTrainRecord(trapTrainId: "UNI", trainQaStatusCd: "ONE", componentId: uniCompIdList[caseDex], beginDatehour: uniBeginHrList[caseDex], endDatehour: uniEndHrList[caseDex]));
                    if (duoCompIdList[caseDex].HasValue())
                        EmParameters.MatsSamplingTrainRecords.Add(new MatsSamplingTrainRecord(trapTrainId: "DUO", trainQaStatusCd: "TWO", componentId: duoCompIdList[caseDex], beginDatehour: duoBeginHrList[caseDex], endDatehour: duoEndHrList[caseDex]));
                    if (treCompIdList[caseDex].HasValue())
                        EmParameters.MatsSamplingTrainRecords.Add(new MatsSamplingTrainRecord(trapTrainId: "TRE", trainQaStatusCd: "THREE", componentId: treCompIdList[caseDex], beginDatehour: treBeginHrList[caseDex], endDatehour: treEndHrList[caseDex]));
                }
                EmParameters.MatsSorbentTrapListByLocationArray = new List<SorbentTrapEvalInformation>[] { new List<SorbentTrapEvalInformation>(), new List<SorbentTrapEvalInformation>(), new List<SorbentTrapEvalInformation>() };
                {
                    if (borIsBorderList[caseDex].HasValue || borBeginHrList[caseDex].HasValue || borEndHrList[caseDex].HasValue)
                    {
                        SorbentTrapEvalInformation info = new SorbentTrapEvalInformation()
                        {
                            IsBorderTrap = borIsBorderList[caseDex],
                            SorbentTrapBeginDateHour = borBeginHrList[caseDex],
                            SorbentTrapEndDateHour = borEndHrList[caseDex],
                        };

                        EmParameters.MatsSorbentTrapListByLocationArray[1].Add(info);
                    }
                }

                /* Initialize Output Parameters */
                EmParameters.MatsGfmSamplingTrainRecords = null;
                EmParameters.MatsHourlyGfmComponentIdValid = null;
                EmParameters.MatsSamplingTrainRecord = new MatsSamplingTrainRecord(trapTrainId: "INI");
                EmParameters.MatsSamplingTrainQaStatus = "INI";
                EmParameters.MatsSorbentTrapBeginDatehour = DateTime.MaxValue;
                EmParameters.MatsSorbentTrapEndDatehour = DateTime.MinValue;
                EmParameters.MatsSamplingTrainCount = -1;


                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Initialize variables needed to run the check. */
                bool log = false;
                string actual;

                /* Run Check */
                actual = cMATSHourlyGFMChecks.MatsGfm1(category, ref log);


                /* Check Results */
                Assert.AreEqual(string.Empty, actual, string.Format("actual[case {0}]", caseDex));
                Assert.AreEqual(false, log, string.Format("log [case {0}]", caseDex));
                Assert.AreEqual(expResultList[caseDex], category.CheckCatalogResult, string.Format("category.CheckCatalogResult [case {0}]", caseDex));

                Assert.AreEqual(expCountList[caseDex], EmParameters.MatsGfmSamplingTrainRecords == null ? (int?)null : EmParameters.MatsGfmSamplingTrainRecords.Count, string.Format("MatsGfmSamplingTrainRecords  [case {0}]", caseDex));
                Assert.AreEqual(expValidList[caseDex], EmParameters.MatsHourlyGfmComponentIdValid, string.Format("MatsHourlyGfmComponentIdValid [case {0}]", caseDex));
                Assert.AreEqual(expTrainPositions[caseDex] == null ? null : EmParameters.MatsSamplingTrainRecords[expTrainPositions[caseDex][0]].TrapTrainId, EmParameters.MatsSamplingTrainRecord == null ? null : EmParameters.MatsSamplingTrainRecord.TrapTrainId, string.Format("MatsSamplingTrainRecord  [case {0}]", caseDex));
                Assert.AreEqual(expTrainPositions[caseDex] == null ? null : EmParameters.MatsSamplingTrainRecords[expTrainPositions[caseDex][0]].TrainQaStatusCd, EmParameters.MatsSamplingTrainQaStatus, string.Format("MatsSamplingTrainQaStatus [case {0}]", caseDex));
                Assert.AreEqual(expTrainPositions[caseDex] == null ? null : EmParameters.MatsSamplingTrainRecords[expTrainPositions[caseDex][0]].BeginDatehour, EmParameters.MatsSorbentTrapBeginDatehour, string.Format("MatsSorbentTrapBeginDatehour [case {0}]", caseDex));
                Assert.AreEqual(expTrainPositions[caseDex] == null ? null : EmParameters.MatsSamplingTrainRecords[expTrainPositions[caseDex][0]].EndDatehour, EmParameters.MatsSorbentTrapEndDatehour, string.Format("MatsSorbentTrapEndDatehour [case {0}]", caseDex));
                Assert.AreEqual(expCountList[caseDex], EmParameters.MatsSamplingTrainCount, string.Format("MatsSamplingTrainCount  [case {0}]", caseDex));

                if (expTrainPositions[caseDex] != null)
                {
                    if (expTrainPositions[caseDex].Length >= 1)
                        Assert.AreEqual(EmParameters.MatsSamplingTrainRecords[expTrainPositions[caseDex][0]].TrapTrainId, EmParameters.MatsGfmSamplingTrainRecords[0].TrapTrainId, string.Format("MatsGfmSamplingTrainRecords[0]  [case {0}]", caseDex));

                    if (expTrainPositions[caseDex].Length >= 2)
                        Assert.AreEqual(EmParameters.MatsSamplingTrainRecords[expTrainPositions[caseDex][1]].TrapTrainId, EmParameters.MatsGfmSamplingTrainRecords[1].TrapTrainId, string.Format("MatsGfmSamplingTrainRecords[1]  [case {0}]", caseDex));

                    if (expTrainPositions[caseDex].Length >= 3)
                        Assert.AreEqual(EmParameters.MatsSamplingTrainRecords[expTrainPositions[caseDex][2]].TrapTrainId, EmParameters.MatsGfmSamplingTrainRecords[2].TrapTrainId, string.Format("MatsGfmSamplingTrainRecords[2]  [case {0}]", caseDex));
                }
            }
        }

        /// <summary>
        ///A test for MATSGFM-2
        ///</summary>()
        [TestMethod()]
        public void MatsGfm2()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            /* Initialize variables needed to run the check. */
            bool log = false;
            string actual;

            /* Initialize General Variables */
            DateTime beginDateHour = new DateTime(2014, 11, 12, 13, 0, 0);
            DateTime beginDate = beginDateHour.Date;
            int beginHour = beginDateHour.Hour;
            DateTime endDateHour = beginDateHour.AddHours(4);
            DateTime endDate = endDateHour.Date;
            int endHour = endDateHour.Hour;

            string[] beginEndHourFlagList = { null, "F", "I", "T" };
            bool?[] validList = new bool?[] { null, false, true };
            int[] samplingTrainCountList = new int[] { 0, 1, 2, 3 };

            /* Initialize Unchanging Parameters */
            EmParameters.MatsSorbentTrapBeginDatehour = beginDateHour;
            EmParameters.MatsSorbentTrapEndDatehour = endDateHour;

            /* Cases */
            foreach (bool? valid in validList)
                for (DateTime currentDateHour = beginDateHour; currentDateHour <= endDateHour; currentDateHour = currentDateHour.AddHours(1))
                    foreach (string beginEndHourFlag in beginEndHourFlagList)
                        foreach (int samplintTrainCount in samplingTrainCountList)
                        {
                            /* Initialize MatsSamplingTrainDictionary */
                            EmParameters.MatsSamplingTrainDictionary = new Dictionary<string, SamplingTrainEvalInformation>();
                            EmParameters.MatsSamplingTrainDictionary["ComponentId2"] = new SamplingTrainEvalInformation("ComponentId2");
                            EmParameters.MatsSamplingTrainDictionary["ComponentId2"].TrainQAStatusCode = "TESTING";

                            /*  Initialize Input Parameters*/
                            EmParameters.CurrentDateHour = currentDateHour;
                            EmParameters.MatsHourlyGfmComponentIdValid = valid;
                            EmParameters.MatsHourlyGfmRecord = new MatsHourlyGfmRecord(beginEndHourFlg: beginEndHourFlag);
                            EmParameters.MatsSamplingTrainCount = samplintTrainCount;

                            /* Expected Results */
                            string result = null;
                            {
                                if (valid.Default(false))
                                {
                                    switch (beginEndHourFlag)
                                    {
                                        case "I":
                                            {
                                                if (currentDateHour > beginDateHour.AddHours(1))
                                                    result = "A";
                                            }
                                            break;

                                        case "F":
                                            {
                                                if (currentDateHour < endDateHour.AddHours(-1))
                                                    result = "B";
                                            }
                                            break;

                                        case "T":
                                            {
                                                if (samplintTrainCount <= 1)
                                                    result = "E";
                                                else if (currentDateHour != endDateHour)
                                                    result = "F";
                                            }
                                            break;

                                        default:
                                            {
                                                if (currentDateHour == beginDateHour)
                                                    result = "C";
                                                else if (currentDateHour == endDateHour)
                                                    result = "D";
                                            }
                                            break;
                                    }
                                }
                            }


                            /* Init Cateogry Result */
                            category.CheckCatalogResult = null;

                            // Run Checks
                            actual = cMATSHourlyGFMChecks.MatsGfm2(category, ref log);

                            // Check Results
                            Assert.AreEqual(string.Empty, actual);
                            Assert.AreEqual(false, log);
                            Assert.AreEqual(result, category.CheckCatalogResult, string.Format("[valid: {0}, currentDateHour: {1}, beginEndHourFlag: {2}].Result", valid, currentDateHour, beginEndHourFlag));
                        }
        }

        /// <summary>
        ///A test for MATSGFM-3
        ///</summary>()
        [TestMethod()]
        public void MatsGfm3()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            /* Initialize variables needed to run the check. */
            bool log = false;
            string actual;

            /* Initialize General Variables */
            string[] trainQaStatusList = new string[] { "EXPIRED", "FAILED", "INC", "LOST", "PASSED", "UNCERTAIN" };
            bool?[] validList = new bool?[] { null, false, true };
            decimal?[] valueList = { null, 1.234m, 1.23m };
            string[] beginEndHourFlagList = { null, "F", "I", "N", "T" };

            /* Cases */
            foreach (bool? valid in validList)
                foreach (decimal? value in valueList)
                    foreach (string trainQaStatus in trainQaStatusList)
                        foreach (string beginEndHourFlag in beginEndHourFlagList)
                        {
                            /*  Initialize Input Parameters*/
                            EmParameters.MatsHourlyGfmComponentIdValid = valid;
                            EmParameters.MatsHourlyGfmRecord = new MatsHourlyGfmRecord(gfmReading: value, beginEndHourFlg: beginEndHourFlag);
                            EmParameters.MatsSamplingTrainQaStatus = trainQaStatus;

                            /* Expected Results */
                            string result = null;
                            {
                                if (valid.Default(false))
                                {
                                    if (value == null)
                                    {
                                        if (trainQaStatus.NotInList("INC,EXPIRED,LOST") && (beginEndHourFlag != "N"))
                                            result = "A";
                                    }
                                    else
                                    {
                                        if (beginEndHourFlag == "N")
                                            result = "D";
                                        else if (trainQaStatus.NotInList("PASSED,FAILED,UNCERTAIN"))
                                            result = "B";
                                        else if (value == 1.234m)
                                            result = "C";
                                    }
                                }
                            }

                            /* Init Cateogry Result */
                            category.CheckCatalogResult = null;

                            // Run Checks
                            actual = cMATSHourlyGFMChecks.MatsGfm3(category, ref log);

                            // Check Results
                            Assert.AreEqual(string.Empty, actual);
                            Assert.AreEqual(false, log);
                            Assert.AreEqual(result, category.CheckCatalogResult, string.Format("[valid: {0}, value: {1}, qaStatus: {2}].Result", valid, value, trainQaStatus));
                        }
        }

        /// <summary>
        ///A test for MATSGFM-4
        ///</summary>()
        [TestMethod()]
        public void MatsGfm4()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            /* Initialize variables needed to run the check. */
            bool log = false;
            string actual;

            /* Initialize General Variables */
            string[] trainQaStatusList = new string[] { "EXPIRED", "FAILED", "INC", "LOST", "PASSED", "UNCERTAIN" };
            bool?[] validList = new bool?[] { null, false, true };
            decimal?[] valueList = { null, 1.234m, 1.23m };
            string[] beginEndHourFlagList = { null, "F", "I", "N", "T" };

            /* Cases */
            foreach (bool? valid in validList)
                foreach (decimal? value in valueList)
                    foreach (string trainQaStatus in trainQaStatusList)
                        foreach (string beginEndHourFlag in beginEndHourFlagList)
                        {
                            /*  Initialize Input Parameters*/
                            EmParameters.MatsHourlyGfmComponentIdValid = valid;
                            EmParameters.MatsHourlyGfmRecord = new MatsHourlyGfmRecord(avgSamplingRate: value, beginEndHourFlg: beginEndHourFlag);
                            EmParameters.MatsSamplingTrainQaStatus = trainQaStatus;

                            /* Expected Results */
                            string result = null;
                            {
                                if (valid.Default(false))
                                {
                                    if (value == null)
                                    {
                                        if (trainQaStatus.NotInList("INC,EXPIRED,LOST") && (beginEndHourFlag != "N"))
                                            result = "A";
                                    }
                                    else
                                    {
                                        if (beginEndHourFlag == "N")
                                            result = "D";
                                        else if (trainQaStatus.NotInList("PASSED,FAILED,UNCERTAIN"))
                                            result = "B";
                                        else if (value == 1.234m)
                                            result = "C";
                                    }
                                }
                            }

                            /* Init Cateogry Result */
                            category.CheckCatalogResult = null;

                            // Run Checks
                            actual = cMATSHourlyGFMChecks.MatsGfm4(category, ref log);

                            // Check Results
                            Assert.AreEqual(string.Empty, actual);
                            Assert.AreEqual(false, log);
                            Assert.AreEqual(result, category.CheckCatalogResult, string.Format("[valid: {0}, value: {1}, qaStatus: {2}].Result", valid, value, trainQaStatus));
                        }
        }

        /// <summary>
        ///A test for MATSGFM-5
        ///</summary>()
        [TestMethod()]
        public void MatsGfm5()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            /* Initialize variables needed to run the check. */
            bool log = false;
            string actual;

            /* Initialize General Variables */
            string[] trainQaStatusList = new string[] { "EXPIRED", "FAILED", "INC", "LOST", "PASSED", "UNCERTAIN" };
            bool?[] validList = new bool?[] { null, false, true };
            string[] valueList = { null, "BAD", "CCMIN", "DSCMMIN", "LMIN", "DSCMHR", "LHR" };
            string[] beginEndHourFlagList = { null, "F", "I", "N", "T" };

            /* Cases */
            foreach (bool? valid in validList)
                foreach (string value in valueList)
                    foreach (string trainQaStatus in trainQaStatusList)
                        foreach (string beginEndHourFlag in beginEndHourFlagList)
                        {
                            /*  Initialize Input Parameters*/
                            EmParameters.MatsHourlyGfmComponentIdValid = valid;
                            EmParameters.MatsHourlyGfmRecord = new MatsHourlyGfmRecord(samplingRateUom: value, beginEndHourFlg: beginEndHourFlag);
                            EmParameters.MatsSamplingTrainQaStatus = trainQaStatus;

                            /* Expected Results */
                            string result = null;
                            {
                                if (valid.Default(false))
                                {
                                    if (value == null)
                                    {
                                        if (trainQaStatus.NotInList("INC,EXPIRED,LOST") && (beginEndHourFlag != "N"))
                                            result = "A";
                                    }
                                    else
                                    {
                                        if (beginEndHourFlag == "N")
                                            result = "D";
                                        else if (trainQaStatus.NotInList("PASSED,FAILED,UNCERTAIN"))
                                            result = "B";
                                        else if (value == "BAD")
                                            result = "C";
                                    }
                                }
                            }

                            /* Init Cateogry Result */
                            category.CheckCatalogResult = null;

                            // Run Checks
                            actual = cMATSHourlyGFMChecks.MatsGfm5(category, ref log);

                            // Check Results
                            Assert.AreEqual(string.Empty, actual);
                            Assert.AreEqual(false, log);
                            Assert.AreEqual(result, category.CheckCatalogResult, string.Format("[valid: {0}, value: {1}, qaStatus: {2}].Result", valid, value, trainQaStatus));
                        }
        }


        #region MatsGfm-6

        /*
         * The QaStatus, Modc and BeginEndHourFlag tests check the conditions for results A, B, E, F and G including cobminations for result A.
         * The HourlySFSRRatio test checks the conditions for result C and D, and the conditions for checking and counting the GFM total and deviated rows.
         * All together, the four test cover the whole check.
         */

        /// <summary>
        /// MatsGfm-6
        /// 
        /// Tests HourlySFSRRatio (flowToSamplingRatio) values for results C and D, and checks total and deviated count updates.
        /// 
        /// Case Notes:
        /// 
        /// * MatsHourlyGFMRecord.BeginEndHourFlag is alwaus null.
        /// * MatsSamplingTrainQaStatus is always "PASSED".
        /// * CurrentStackFlowHourlyRecord.ModcCode is always "01".
        /// 
        /// * Ratio is the MatsHourlyGFMRecord.HourlySFSRRatio (flowToSamplingRatio) value.  It is never null for this test.
        /// * CompValid contains MatsHourlyGFMComponentIdValid values.
        /// * Mats Sampling Train Dictionary group contains MatsSamplingTrainDictionary values.
        /// 
        /// |    |                   |    - Mats Sampling Train Dict -    ||        |  - Train Dict -  || 
        /// | ## | Ratio | CompValid | TrainId | Ratio | Total | Deviated || Result | Total | Deviated || Note
        /// |  0 |  1.01 | true      | GoodId  |   1.0 |    15 |       13 || C      |    15 |       13 || HourlySFSRRatio (flowToSamplingRatio) not rounded to one decimal place.
        /// |  1 |   0.9 | true      | GoodId  |   1.0 |    15 |       13 || D      |    15 |       13 || HourlySFSRRatio (flowToSamplingRatio) is less than 1.0.
        /// |  2 |   1.0 | true      | GoodId  |   1.0 |    15 |       13 || null   |    16 |       13 || HourlySFSRRatio (flowToSamplingRatio) is less than 1.0.
        /// |  3 | 100.0 | true      | GoodId  | 100.0 |    15 |       13 || null   |    16 |       13 || HourlySFSRRatio (flowToSamplingRatio) is greater than 100.0.
        /// |  4 | 100.1 | true      | GoodId  | 100.0 |    15 |       13 || D      |    15 |       13 || HourlySFSRRatio (flowToSamplingRatio) is greater than 100.0.
        /// |  5 |  74.6 | true      | GoodId  | 100.0 |    15 |       13 || null   |    16 |       13 || Deviation is less than or equal to 25% after rounding.
        /// |  6 |  74.5 | true      | GoodId  | 100.0 |    15 |       13 || null   |    16 |       14 || Deviation is greater than 25% after rounding.
        /// |  7 |  74.5 | false     | GoodId  | 100.0 |    15 |       13 || null   |    15 |       13 || MatsHourlyGFMComponentIdValid is false and therefore counts are not updated.
        /// |  8 |  74.5 | true      | BadId   | 100.0 |    15 |       13 || null   |    15 |       13 || No dictionary key equal to "Good"Id" and therefore counts are not updated.
        /// |  9 |  74.5 | true      | GoodId  | null  |    15 |       13 || null   |    15 |       13 || Dictionary ReferenceSFSRRatio is null and therefore counts are not updated.
        /// | 10 |  74.5 | true      | GoodId  |   0.0 |    15 |       13 || null   |    15 |       13 || Dictionary ReferenceSFSRRatio is 0 and therefore counts are not updated.
        /// | 11 |   1.0 | true      | GoodId  |   0.1 |    15 |       13 || null   |    16 |       14 || Dictionary ReferenceSFSRRatio is 0.1 and HourlySFSRRatio (flowToSamplingRatio) is 1.0, therefore both counts are updated.
        /// 
        /// </summary>
        [TestMethod()]
        public void MatsGfm6_HourlySFSRRatio()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            /* Input Parameter Values */
            decimal?[] hourlySfsrRatioList = { 1.01m, 0.9m, 1.0m, 100.0m, 100.1m, 74.6m, 74.5m, 74.5m, 74.5m, 74.5m, 74.5m, 1.0m };
            bool?[] componentIdValidList = { true, true, true, true, true, true, true, false, true, true, true, true };
            string[] dictTrainIdList = { "GoodId", "GoodId", "GoodId", "GoodId", "GoodId", "GoodId", "GoodId", "GoodId", "BadId", "GoodId", "GoodId", "GoodId" };
            decimal?[] dictRefRatioList = { 1.0m, 1.0m, 1.0m, 100.0m, 100.0m, 100.0m, 100.0m, 100.0m, 100.0m, null, 0.0m, 0.1m };

            /* Expected Values */
            string[] resultList = { "C", "D", null, null, "D", null, null, null, null, null, null, null };
            decimal?[] expTotalCountList = { 15, 15, 16, 16, 15, 16, 16, 15, 15, 15, 15, 16 };
            decimal?[] expDeviatedCountList = { 13, 13, 13, 13, 13, 13, 14, 13, 13, 13, 13, 14 };

            int dictTotalCount = 15;
            int dictDeviatedCount = 13;

            /* Test Case Count */
            int caseCount = 12;

            /* Check array lengths */
            Assert.AreEqual(caseCount, hourlySfsrRatioList.Length, "hourlySfsrRatioList length");
            Assert.AreEqual(caseCount, componentIdValidList.Length, "componentIdValidList length");
            Assert.AreEqual(caseCount, dictTrainIdList.Length, "dictTrainIdList length");
            Assert.AreEqual(caseCount, dictRefRatioList.Length, "dictRefRatioList length");
            Assert.AreEqual(caseCount, resultList.Length, "resultList length");
            Assert.AreEqual(caseCount, expTotalCountList.Length, "expTotalCountList length");
            Assert.AreEqual(caseCount, expDeviatedCountList.Length, "expDeviatedCountList length");

            /* Run Cases */
            for (int caseDex = 0; caseDex < caseCount; caseDex++)
            {
                /*  Initialize Input Parameters*/
                EmParameters.CurrentFlowMonitorHourlyRecord = new VwMpMonitorHrlyValueFlowRow(modcCd: "01");
                EmParameters.MatsHourlyGfmComponentIdValid = componentIdValidList[caseDex];
                EmParameters.MatsHourlyGfmRecord = new MatsHourlyGfmRecord(flowToSamplingRatio: hourlySfsrRatioList[caseDex]);
                EmParameters.MatsSamplingTrainDictionary = new Dictionary<string, SamplingTrainEvalInformation>();
                {
                    EmParameters.MatsSamplingTrainDictionary[dictTrainIdList[caseDex]] = new SamplingTrainEvalInformation("TrainId");
                    EmParameters.MatsSamplingTrainDictionary[dictTrainIdList[caseDex]].TotalSFSRRatioCount = dictTotalCount;
                    EmParameters.MatsSamplingTrainDictionary[dictTrainIdList[caseDex]].DeviatedSFSRRatioCount = dictDeviatedCount;
                    EmParameters.MatsSamplingTrainDictionary[dictTrainIdList[caseDex]].ReferenceSFSRRatio = dictRefRatioList[caseDex];
                }
                EmParameters.MatsSamplingTrainQaStatus = "PASSED";
                EmParameters.MatsSamplingTrainRecord = new MatsSamplingTrainRecord(trapTrainId: "GoodId");

                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Initialize variables needed to run the check. */
                bool log = false;
                string actual;

                if (caseDex >= 0)
                {
                    /* Run Check */
                    actual = cMATSHourlyGFMChecks.MatsGfm6(category, ref log);

                    /* Check results */
                    Assert.AreEqual(string.Empty, actual);
                    Assert.AreEqual(false, log);

                    Assert.AreEqual(resultList[caseDex], category.CheckCatalogResult, String.Format("Result [case {0}]", caseDex));
                    Assert.AreEqual(expTotalCountList[caseDex], EmParameters.MatsSamplingTrainDictionary[dictTrainIdList[caseDex]].TotalSFSRRatioCount, String.Format("TotalSFSRRatioCount [case {0}]", caseDex));
                    Assert.AreEqual(expDeviatedCountList[caseDex], EmParameters.MatsSamplingTrainDictionary[dictTrainIdList[caseDex]].DeviatedSFSRRatioCount, String.Format("DeviatedSFSRRatioCount [case {0}]", caseDex));
                }
            }
        }

        /// <summary>
        ///  MatsGfm-6: Tests various QA Status values for results A and B.
        /// </summary>
        [TestMethod()]
        public void MatsGfm6_QaStatus()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            /* Initialize variables needed to run the check. */
            bool log = false;
            string actual;

            /* Initialize General Variables */
            decimal?[] flowToSamplingRatioList = { null, 1.0m };
            string[] trainQaStatusList = new string[] { "EXPIRED", "FAILED", "INC", "LOST", "PASSED", "UNCERTAIN" };

            string beginEndHourFlag = null;
            string modcCd = "01";

            /* Cases */
            foreach (string trainQaStatus in trainQaStatusList)
                foreach (decimal? flowToSamplingRatio in flowToSamplingRatioList)
                {
                    /*  Initialize Input Parameters*/
                    EmParameters.MatsHourlyGfmComponentIdValid = true;  //TODO: Current the check does not run at all if MatsHourlyGfmComponentIdValid is not true.  This does not match the spec.
                    EmParameters.MatsHourlyGfmRecord = new MatsHourlyGfmRecord(componentId: "ComponentId2", flowToSamplingRatio: flowToSamplingRatio, beginEndHourFlg: beginEndHourFlag);
                    EmParameters.MatsSamplingTrainDictionary = new Dictionary<string, SamplingTrainEvalInformation>();
                    EmParameters.MatsSamplingTrainRecord = new MatsSamplingTrainRecord(trapTrainId: "TargetTrain");
                    EmParameters.MatsSamplingTrainQaStatus = trainQaStatus;
                    EmParameters.CurrentFlowMonitorHourlyRecord = new VwMpMonitorHrlyValueFlowRow(modcCd: modcCd);

                    /* Expected Results */
                    string result;
                    {
                        if (flowToSamplingRatio == null)
                            result = trainQaStatus.NotInList("INC,EXPIRED,LOST,FAILED") ? "A" : null;
                        else
                            result = trainQaStatus.NotInList("PASSED,FAILED,UNCERTAIN") ? "B" : null; ;
                    }

                    /* Init Cateogry Result */
                    category.CheckCatalogResult = null;

                    // Run Checks
                    actual = cMATSHourlyGFMChecks.MatsGfm6(category, ref log);

                    // Check Results
                    Assert.AreEqual(string.Empty, actual, string.Format("[result: qaStatus: {0}, flowToSamplingRatio: {1}].Result", trainQaStatus, flowToSamplingRatio));
                    Assert.AreEqual(false, log);
                    Assert.AreEqual(result, category.CheckCatalogResult, string.Format("[result: qaStatus: {0}, flowToSamplingRatio: {1}].Result", trainQaStatus, flowToSamplingRatio));

                    Assert.AreEqual(false, EmParameters.MatsSamplingTrainDictionary.ContainsKey("TargetTrain"), string.Format("[result: qaStatus: {0}, flowToSamplingRatio: {1}].MatsSamplingTrainDictionary.Contains", trainQaStatus, flowToSamplingRatio));
                }
        }

        /// <summary>
        ///  MatsGfm-6: Tests CurrentStackFlowHourlyRecord exists and various MODC values for results A, E and F.
        /// </summary>
        [TestMethod()]
        public void MatsGfm6_Modc()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            /* Initialize variables needed to run the check. */
            bool log = false;
            string actual;

            /* Initialize General Variables */
            decimal?[] flowToSamplingRatioList = { null, 1.0m };

            string beginEndHourFlag = null;
            string trainQaStatus = "PASSED";

            /* Cases */
            foreach (bool stackFlowExists in UnitTestStandardLists.BooleanList)
                foreach (string modcCd in stackFlowExists ? UnitTestStandardLists.ModcCodeList : new string[] { null })
                    foreach (decimal? flowToSamplingRatio in flowToSamplingRatioList)
                    {
                        /*  Initialize Input Parameters*/
                        EmParameters.MatsHourlyGfmComponentIdValid = true;  //TODO: Current the check does not run at all if MatsHourlyGfmComponentIdValid is not true.  This does not match the spec.
                        EmParameters.MatsHourlyGfmRecord = new MatsHourlyGfmRecord(componentId: "ComponentId2", flowToSamplingRatio: flowToSamplingRatio, beginEndHourFlg: beginEndHourFlag);
                        EmParameters.MatsSamplingTrainDictionary = new Dictionary<string, SamplingTrainEvalInformation>();
                        EmParameters.MatsSamplingTrainRecord = new MatsSamplingTrainRecord(trapTrainId: "TargetTrain");
                        EmParameters.MatsSamplingTrainQaStatus = trainQaStatus;
                        EmParameters.CurrentFlowMonitorHourlyRecord = (modcCd != null) ? new VwMpMonitorHrlyValueFlowRow(modcCd: modcCd) : null;

                        /* Expected Results */
                        string result;
                        {
                            if (flowToSamplingRatio == null)
                                result = modcCd.InList("01,02,03,04,20,53,54") ? "A" : null;
                            else if (!stackFlowExists)
                                result = "F";
                            else
                                result = modcCd.NotInList("01,02,03,04,20,53,54") ? "E" : null; ;
                        }

                        /* Init Cateogry Result */
                        category.CheckCatalogResult = null;

                        // Run Checks
                        actual = cMATSHourlyGFMChecks.MatsGfm6(category, ref log);

                        // Check Results
                        Assert.AreEqual(string.Empty, actual, string.Format("[result: modcCd: {0}, flowToSamplingRatio: {1}].Result", modcCd, flowToSamplingRatio));
                        Assert.AreEqual(false, log);
                        Assert.AreEqual(result, category.CheckCatalogResult, string.Format("[result: modcCd: {0}, flowToSamplingRatio: {1}].Result", modcCd, flowToSamplingRatio));

                        Assert.AreEqual(false, EmParameters.MatsSamplingTrainDictionary.ContainsKey("TargetTrain"), string.Format("[result: modcCd: {0}, flowToSamplingRatio: {1}].MatsSamplingTrainDictionary.Contains", modcCd, flowToSamplingRatio));
                    }
        }

        /// <summary>
        ///  MatsGfm-6: Tests various Begin End Hour Flag values for results A and G.
        /// </summary>
        [TestMethod()]
        public void MatsGfm6_BeginEndHourFlag()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            /* Initialize variables needed to run the check. */
            bool log = false;
            string actual;

            /* Initialize General Variables */
            decimal?[] flowToSamplingRatioList = { null, 1.0m };
            string[] beginEndHourFlagList = { null, "F", "I", "N", "T" };

            string modcCd = "01";
            string trainQaStatus = "PASSED";

            /* Cases */
            foreach (string beginEndHourFlag in beginEndHourFlagList)
                foreach (decimal? flowToSamplingRatio in flowToSamplingRatioList)
                {
                    /*  Initialize Input Parameters*/
                    EmParameters.MatsHourlyGfmComponentIdValid = true;  //TODO: Current the check does not run at all if MatsHourlyGfmComponentIdValid is not true.  This does not match the spec.
                    EmParameters.MatsHourlyGfmRecord = new MatsHourlyGfmRecord(componentId: "ComponentId2", flowToSamplingRatio: flowToSamplingRatio, beginEndHourFlg: beginEndHourFlag);
                    EmParameters.MatsSamplingTrainDictionary = new Dictionary<string, SamplingTrainEvalInformation>();
                    EmParameters.MatsSamplingTrainRecord = new MatsSamplingTrainRecord(trapTrainId: "TargetTrain");
                    EmParameters.MatsSamplingTrainQaStatus = trainQaStatus;
                    EmParameters.CurrentFlowMonitorHourlyRecord = new VwMpMonitorHrlyValueFlowRow(modcCd: modcCd);

                    /* Expected Results */
                    string result;
                    {
                        if (flowToSamplingRatio == null)
                            result = (beginEndHourFlag != "N") ? "A" : null;
                        else
                            result = (beginEndHourFlag == "N") ? "G" : null; ;
                    }

                    /* Init Cateogry Result */
                    category.CheckCatalogResult = null;

                    // Run Checks
                    actual = cMATSHourlyGFMChecks.MatsGfm6(category, ref log);

                    // Check Results
                    Assert.AreEqual(string.Empty, actual, string.Format("[result: beginEndHourFlag: {0}, flowToSamplingRatio: {1}].Result", beginEndHourFlag, flowToSamplingRatio));
                    Assert.AreEqual(false, log);
                    Assert.AreEqual(result, category.CheckCatalogResult, string.Format("[result: beginEndHourFlag: {0}, flowToSamplingRatio: {1}].Result", beginEndHourFlag, flowToSamplingRatio));

                    Assert.AreEqual(false, EmParameters.MatsSamplingTrainDictionary.ContainsKey("TargetTrain"), string.Format("[result: beginEndHourFlag: {0}, flowToSamplingRatio: {1}].MatsSamplingTrainDictionary.Contains", beginEndHourFlag, flowToSamplingRatio));
                }
        }

        #endregion


        /// <summary>
        /// 
        /// </summary>
        [TestMethod()]
        public void MatsGfm7()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;


            /* Initialize Input Parameters */
            string goodId = "GoodId";

            DateTime currHr = new DateTime(2016, 6, 17, 22, 0, 0);
            DateTime currM2 = currHr.AddHours(-2);
            DateTime currM1 = currHr.AddHours(-1);
            DateTime currP1 = currHr.AddHours(1);
            DateTime currP2 = currHr.AddHours(2);

            string failedTrainId = "FID";
            string passedTrainId = "PID";
            string uncertainTrainId = "UID";

            string failedNullRataTrainId = "FNRID";
            string passedNullRataTrainId = "PNRID";
            string uncertainNullRataTrainId = "UNRID";

            string expiredTrainId = "EID";
            string incompleteTrainId = "IID";
            string lostTrainId = "LID";

            string failedRataTrainId = "FRID";
            string passedRataTrainId = "PRID";
            string uncertainRataTrainId = "URID";

            string missingTrainId = "MISSID";

            EmParameters.MatsSamplingTrainDictionary = new Dictionary<string, SamplingTrainEvalInformation>();
            {
                EmParameters.MatsSamplingTrainDictionary[failedTrainId] = new SamplingTrainEvalInformation(failedTrainId);
                EmParameters.MatsSamplingTrainDictionary[passedTrainId] = new SamplingTrainEvalInformation(passedTrainId);
                EmParameters.MatsSamplingTrainDictionary[uncertainTrainId] = new SamplingTrainEvalInformation(uncertainTrainId);
                EmParameters.MatsSamplingTrainDictionary[failedNullRataTrainId] = new SamplingTrainEvalInformation(failedNullRataTrainId);
                EmParameters.MatsSamplingTrainDictionary[passedNullRataTrainId] = new SamplingTrainEvalInformation(passedNullRataTrainId);
                EmParameters.MatsSamplingTrainDictionary[uncertainNullRataTrainId] = new SamplingTrainEvalInformation(uncertainNullRataTrainId);
                EmParameters.MatsSamplingTrainDictionary[expiredTrainId] = new SamplingTrainEvalInformation(expiredTrainId);
                EmParameters.MatsSamplingTrainDictionary[incompleteTrainId] = new SamplingTrainEvalInformation(incompleteTrainId);
                EmParameters.MatsSamplingTrainDictionary[lostTrainId] = new SamplingTrainEvalInformation(lostTrainId);
                EmParameters.MatsSamplingTrainDictionary[failedRataTrainId] = new SamplingTrainEvalInformation(failedRataTrainId);
                EmParameters.MatsSamplingTrainDictionary[passedRataTrainId] = new SamplingTrainEvalInformation(passedRataTrainId);
                EmParameters.MatsSamplingTrainDictionary[uncertainRataTrainId] = new SamplingTrainEvalInformation(uncertainRataTrainId);
            }

            EmParameters.MatsGfmSamplingTrainRecords = new CheckDataView<MatsSamplingTrainRecord>();
            {
                /* Non RATA */
                EmParameters.MatsGfmSamplingTrainRecords.Add(new MatsSamplingTrainRecord(trapTrainId: failedTrainId, trainQaStatusCd: "FAILED", rataInd: 0, componentId: goodId, beginDatehour: currM1, endDatehour: currP1));
                EmParameters.MatsGfmSamplingTrainRecords.Add(new MatsSamplingTrainRecord(trapTrainId: passedTrainId, trainQaStatusCd: "PASSED", rataInd: 0, componentId: goodId, beginDatehour: currM1, endDatehour: currP1));
                EmParameters.MatsGfmSamplingTrainRecords.Add(new MatsSamplingTrainRecord(trapTrainId: uncertainTrainId, trainQaStatusCd: "UNCERTAIN", rataInd: 0, componentId: goodId, beginDatehour: currM1, endDatehour: currP1));

                /* NULL RATA */
                EmParameters.MatsGfmSamplingTrainRecords.Add(new MatsSamplingTrainRecord(trapTrainId: failedNullRataTrainId, trainQaStatusCd: "FAILED", rataInd: null, componentId: goodId, beginDatehour: currM1, endDatehour: currP1));
                EmParameters.MatsGfmSamplingTrainRecords.Add(new MatsSamplingTrainRecord(trapTrainId: passedNullRataTrainId, trainQaStatusCd: "PASSED", rataInd: null, componentId: goodId, beginDatehour: currM1, endDatehour: currP1));
                EmParameters.MatsGfmSamplingTrainRecords.Add(new MatsSamplingTrainRecord(trapTrainId: uncertainNullRataTrainId, trainQaStatusCd: "UNCERTAIN", rataInd: null, componentId: goodId, beginDatehour: currM1, endDatehour: currP1));

                /* Non RATA but not counted */
                EmParameters.MatsGfmSamplingTrainRecords.Add(new MatsSamplingTrainRecord(trapTrainId: expiredTrainId, trainQaStatusCd: "EXPIRED", rataInd: 0, componentId: goodId, beginDatehour: currM1, endDatehour: currP1));
                EmParameters.MatsGfmSamplingTrainRecords.Add(new MatsSamplingTrainRecord(trapTrainId: incompleteTrainId, trainQaStatusCd: "INC", rataInd: 0, componentId: goodId, beginDatehour: currM1, endDatehour: currP1));
                EmParameters.MatsGfmSamplingTrainRecords.Add(new MatsSamplingTrainRecord(trapTrainId: lostTrainId, trainQaStatusCd: "LOST", rataInd: 0, componentId: goodId, beginDatehour: currM1, endDatehour: currP1));

                /* RATA */
                EmParameters.MatsGfmSamplingTrainRecords.Add(new MatsSamplingTrainRecord(trapTrainId: failedRataTrainId, trainQaStatusCd: "FAILED", rataInd: 1, componentId: goodId, beginDatehour: currM1, endDatehour: currP1));
                EmParameters.MatsGfmSamplingTrainRecords.Add(new MatsSamplingTrainRecord(trapTrainId: passedRataTrainId, trainQaStatusCd: "PASSED", rataInd: 1, componentId: goodId, beginDatehour: currM1, endDatehour: currP1));
                EmParameters.MatsGfmSamplingTrainRecords.Add(new MatsSamplingTrainRecord(trapTrainId: uncertainRataTrainId, trainQaStatusCd: "UNCERTAIN", rataInd: 1, componentId: goodId, beginDatehour: currM1, endDatehour: currP1));

                /* Missing */
                EmParameters.MatsGfmSamplingTrainRecords.Add(new MatsSamplingTrainRecord(trapTrainId: missingTrainId, trainQaStatusCd: "PASSED", rataInd: 0, componentId: goodId, beginDatehour: currM1, endDatehour: currP1));
            }


            /* Case Lists */
            string[] flagList = { "N", null, "F", "I", "T", "N" };
            int[] countList = { 1, 1, 1, 1, 1, 2 };
            int[] totalList = { 1, 2, 3, 4, 5, 6 };

            /* Cases */
            for (int caseDex = 0; caseDex < 6; caseDex++)
            {
                /*  Initialize Input Parameters*/
                EmParameters.MatsHourlyGfmRecord = new MatsHourlyGfmRecord(beginEndHourFlg: flagList[caseDex]);


                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Initialize variables needed to run the check. */
                bool log = false;
                string actual;

                /* Run Check */
                actual = cMATSHourlyGFMChecks.MatsGfm7(category, ref log);


                /* Check Results */
                Assert.AreEqual(string.Empty, actual, string.Format("actual[case {0}]", caseDex));
                Assert.AreEqual(false, log, string.Format("log [case {0}]", caseDex));
                Assert.AreEqual(null, category.CheckCatalogResult, string.Format("category.CheckCatalogResult [case {0}]", caseDex));



                Assert.AreEqual(countList[caseDex], EmParameters.MatsSamplingTrainDictionary[failedTrainId].NotAvailablelGfmCount, string.Format("failedTrainId.NotAvailablelGfmCount [case {0}]", caseDex));
                Assert.AreEqual(totalList[caseDex], EmParameters.MatsSamplingTrainDictionary[failedTrainId].TotalGfmCount, string.Format("failedTrainId.TotalGfmCount [case {0}]", caseDex));
                Assert.AreEqual(countList[caseDex], EmParameters.MatsSamplingTrainDictionary[passedTrainId].NotAvailablelGfmCount, string.Format("passedTrainId.NotAvailablelGfmCount [case {0}]", caseDex));
                Assert.AreEqual(totalList[caseDex], EmParameters.MatsSamplingTrainDictionary[passedTrainId].TotalGfmCount, string.Format("passedTrainId.TotalGfmCount [case {0}]", caseDex));
                Assert.AreEqual(countList[caseDex], EmParameters.MatsSamplingTrainDictionary[uncertainTrainId].NotAvailablelGfmCount, string.Format("uncertainTrainId.NotAvailablelGfmCount [case {0}]", caseDex));
                Assert.AreEqual(totalList[caseDex], EmParameters.MatsSamplingTrainDictionary[uncertainTrainId].TotalGfmCount, string.Format("uncertainTrainId.TotalGfmCount [case {0}]", caseDex));

                Assert.AreEqual(countList[caseDex], EmParameters.MatsSamplingTrainDictionary[failedNullRataTrainId].NotAvailablelGfmCount, string.Format("failedNullRataTrainId.NotAvailablelGfmCount [case {0}]", caseDex));
                Assert.AreEqual(totalList[caseDex], EmParameters.MatsSamplingTrainDictionary[failedNullRataTrainId].TotalGfmCount, string.Format("failedNullRataTrainId.TotalGfmCount [case {0}]", caseDex));
                Assert.AreEqual(countList[caseDex], EmParameters.MatsSamplingTrainDictionary[passedNullRataTrainId].NotAvailablelGfmCount, string.Format("passedNullRataTrainId.NotAvailablelGfmCount [case {0}]", caseDex));
                Assert.AreEqual(totalList[caseDex], EmParameters.MatsSamplingTrainDictionary[passedNullRataTrainId].TotalGfmCount, string.Format("passedNullRataTrainId.TotalGfmCount [case {0}]", caseDex));
                Assert.AreEqual(countList[caseDex], EmParameters.MatsSamplingTrainDictionary[uncertainNullRataTrainId].NotAvailablelGfmCount, string.Format("uncertainNullRataTrainId.NotAvailablelGfmCount [case {0}]", caseDex));
                Assert.AreEqual(totalList[caseDex], EmParameters.MatsSamplingTrainDictionary[uncertainNullRataTrainId].TotalGfmCount, string.Format("uncertainNullRataTrainId.TotalGfmCount [case {0}]", caseDex));

                Assert.AreEqual(0, EmParameters.MatsSamplingTrainDictionary[expiredTrainId].NotAvailablelGfmCount, string.Format("expiredTrainId.NotAvailablelGfmCount [case {0}]", caseDex));
                Assert.AreEqual(0, EmParameters.MatsSamplingTrainDictionary[expiredTrainId].TotalGfmCount, string.Format("expiredTrainId.TotalGfmCount [case {0}]", caseDex));
                Assert.AreEqual(0, EmParameters.MatsSamplingTrainDictionary[incompleteTrainId].NotAvailablelGfmCount, string.Format("incompleteTrainId.NotAvailablelGfmCount [case {0}]", caseDex));
                Assert.AreEqual(0, EmParameters.MatsSamplingTrainDictionary[incompleteTrainId].TotalGfmCount, string.Format("incompleteTrainId.TotalGfmCount [case {0}]", caseDex));
                Assert.AreEqual(0, EmParameters.MatsSamplingTrainDictionary[lostTrainId].NotAvailablelGfmCount, string.Format("lostTrainId.NotAvailablelGfmCount [case {0}]", caseDex));
                Assert.AreEqual(0, EmParameters.MatsSamplingTrainDictionary[lostTrainId].TotalGfmCount, string.Format("lostTrainId.TotalGfmCount [case {0}]", caseDex));

                Assert.AreEqual(0, EmParameters.MatsSamplingTrainDictionary[failedRataTrainId].NotAvailablelGfmCount, string.Format("failedRataTrainId.NotAvailablelGfmCount [case {0}]", caseDex));
                Assert.AreEqual(0, EmParameters.MatsSamplingTrainDictionary[failedRataTrainId].TotalGfmCount, string.Format("failedRataTrainId.TotalGfmCount [case {0}]", caseDex));
                Assert.AreEqual(0, EmParameters.MatsSamplingTrainDictionary[passedRataTrainId].NotAvailablelGfmCount, string.Format("passedRataTrainId.NotAvailablelGfmCount [case {0}]", caseDex));
                Assert.AreEqual(0, EmParameters.MatsSamplingTrainDictionary[passedRataTrainId].TotalGfmCount, string.Format("passedRataTrainId.TotalGfmCount [case {0}]", caseDex));
                Assert.AreEqual(0, EmParameters.MatsSamplingTrainDictionary[uncertainRataTrainId].NotAvailablelGfmCount, string.Format("uncertainRataTrainId.NotAvailablelGfmCount [case {0}]", caseDex));
                Assert.AreEqual(0, EmParameters.MatsSamplingTrainDictionary[uncertainRataTrainId].TotalGfmCount, string.Format("uncertainRataTrainId.TotalGfmCount [case {0}]", caseDex));

                Assert.AreEqual(false, EmParameters.MatsSamplingTrainDictionary.ContainsKey(missingTrainId), string.Format("missingTrainId [case {0}]", caseDex));
            }

        }

        #endregion

    }
}