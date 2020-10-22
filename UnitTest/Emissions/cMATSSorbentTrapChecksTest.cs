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
    public class cMATSSorbentTrapChecksTest
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
        ///A test for MatsTrp-1
        ///</summary>()
        [TestMethod()]
        public void MatsTrp1()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            /* Initialize General Variables */
            DateTime beginDateHour = new DateTime(2014, 10, 31, 10, 0, 0);

            /* Initialize variables needed to run the check. */
            bool log = false;
            string actual;

            /* Test for null date */
            {
                /*  Initialize Input Parameters*/
                EmParameters.MatsSorbentTrapRecord = new MatsSorbentTrapRecord(beginDate: null);

                /*  Initialize Updatable Parameters*/
                EmParameters.MatsSorbentTrapEvaluationNeeded = true;

                /* Initialize Output Parameters */
                EmParameters.MatsSorbentTrapBeginDateValid = null;

                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Run Check */
                actual = cMatsSorbentTrapChecks.MatsTrp1(category, ref log);

                /* Check results */
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);

                Assert.AreEqual("A", category.CheckCatalogResult, "NullDate.Result");
                Assert.AreEqual(false, EmParameters.MatsSorbentTrapEvaluationNeeded, "NullDate.MatsSorbentTrapEvaluationNeeded");
                Assert.AreEqual(false, EmParameters.MatsSorbentTrapBeginDateValid, "NullDate.MatsSorbentTrapBeginDateValid");
            }

            /* Test for existing date */
            {
                /*  Initialize Input Parameters*/
                EmParameters.MatsSorbentTrapRecord = new MatsSorbentTrapRecord(beginDate: new DateTime(2014, 10, 13));

                /*  Initialize Updatable Parameters*/
                EmParameters.MatsSorbentTrapEvaluationNeeded = null;

                /* Initialize Output Parameters */
                EmParameters.MatsSorbentTrapBeginDateValid = null;

                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Run Check */
                actual = cMatsSorbentTrapChecks.MatsTrp1(category, ref log);

                /* Check results */
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);

                Assert.AreEqual(null, category.CheckCatalogResult, "DateWithNeededTrue.Result");
                Assert.AreEqual(null, EmParameters.MatsSorbentTrapEvaluationNeeded, "DateWithNeededTrue.MatsSorbentTrapEvaluationNeeded");
                Assert.AreEqual(true, EmParameters.MatsSorbentTrapBeginDateValid, "DateWithNeededTrue.MatsSorbentTrapBeginDateValid");
            }
        }

        /// <summary>
        ///A test for MatsTrp-2
        ///</summary>()
        [TestMethod()]
        public void MatsTrp2()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            /* Initialize General Variables */
            bool?[] booleanList = { null, false, true };
            int?[] hourList = { null, -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 };

            /* Initialize variables needed to run the check. */
            bool log = false;
            string actual;


            foreach (bool? dateValid in booleanList)
                foreach (int? hour in hourList)
                {
                    /*  Initialize Input Parameters*/
                    EmParameters.MatsSorbentTrapBeginDateValid = dateValid;
                    EmParameters.MatsSorbentTrapRecord = new MatsSorbentTrapRecord(beginHour: hour);

                    /*  Initialize Updatable Parameters*/
                    EmParameters.MatsSorbentTrapEvaluationNeeded = null;

                    /* Initialize Output Parameters */
                    EmParameters.MatsSorbentTrapBeginDateHourValid = null;

                    /* Init Cateogry Result */
                    category.CheckCatalogResult = null;

                    /* Run Check */
                    actual = cMatsSorbentTrapChecks.MatsTrp2(category, ref log);

                    /* Check Result Labels */
                    string resultPrefix = (dateValid == null ? "DateValidNull" : (dateValid.Value ? "DateValid" : "DateInvalid")) + "." + (hour == null ? "NullHour" : "Hour" + hour.ToString().PadLeft(2, '0')) + ".";

                    /* Check Results */
                    Assert.AreEqual(string.Empty, actual);
                    Assert.AreEqual(false, log);

                    if (dateValid.Default(false))
                    {
                        Assert.AreEqual((hour == null) ? "A" : (((hour < 0) || (23 < hour)) ? "B" : null), category.CheckCatalogResult, resultPrefix + "Result");
                        Assert.AreEqual(((hour == null) || (hour < 0) || (23 < hour)) ? false : (bool?)null, EmParameters.MatsSorbentTrapEvaluationNeeded, resultPrefix + "MatsSorbentTrapEvaluationNeeded");
                        Assert.AreEqual(((hour != null) && ((0 <= hour) && (hour <= 23))), EmParameters.MatsSorbentTrapBeginDateHourValid, resultPrefix + "MatsSorbentTrapBeginDateHourValid");
                    }
                    else
                    {
                        Assert.AreEqual(null, category.CheckCatalogResult, resultPrefix + "Result");
                        Assert.AreEqual(null, EmParameters.MatsSorbentTrapEvaluationNeeded, resultPrefix + "MatsSorbentTrapEvaluationNeeded");
                        Assert.AreEqual(false, EmParameters.MatsSorbentTrapBeginDateHourValid, resultPrefix + "MatsSorbentTrapBeginDateHourValid");
                    }
                }
        }

        /// <summary>
        ///A test for MatsTrp-3
        ///</summary>()
        [TestMethod()]
        public void MatsTrp3()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            /* Initialize General Variables */
            DateTime endDateHour = new DateTime(2014, 10, 31, 10, 0, 0);

            /* Initialize variables needed to run the check. */
            bool log = false;
            string actual;

            /* Test for null date */
            {
                /*  Initialize Input Parameters*/
                EmParameters.MatsSorbentTrapRecord = new MatsSorbentTrapRecord(endDate: null);

                /*  Initialize Updatable Parameters*/
                EmParameters.MatsSorbentTrapEvaluationNeeded = true;

                /* Initialize Output Parameters */
                EmParameters.MatsSorbentTrapEndDateValid = null;

                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Run Check */
                actual = cMatsSorbentTrapChecks.MatsTrp3(category, ref log);

                /* Check results */
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);

                Assert.AreEqual("A", category.CheckCatalogResult, "NullDate.Result");
                Assert.AreEqual(false, EmParameters.MatsSorbentTrapEvaluationNeeded, "NullDate.MatsSorbentTrapEvaluationNeeded");
                Assert.AreEqual(false, EmParameters.MatsSorbentTrapEndDateValid, "NullDate.MatsSorbentTrapEndDateValid");
            }

            /* Test for existing date */
            {
                /*  Initialize Input Parameters*/
                EmParameters.MatsSorbentTrapRecord = new MatsSorbentTrapRecord(endDate: new DateTime(2014, 10, 13));

                /*  Initialize Updatable Parameters*/
                EmParameters.MatsSorbentTrapEvaluationNeeded = null;

                /* Initialize Output Parameters */
                EmParameters.MatsSorbentTrapEndDateValid = null;

                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Run Check */
                actual = cMatsSorbentTrapChecks.MatsTrp3(category, ref log);

                /* Check results */
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);

                Assert.AreEqual(null, category.CheckCatalogResult, "DateWithNeededTrue.Result");
                Assert.AreEqual(null, EmParameters.MatsSorbentTrapEvaluationNeeded, "DateWithNeededTrue.MatsSorbentTrapEvaluationNeeded");
                Assert.AreEqual(true, EmParameters.MatsSorbentTrapEndDateValid, "DateWithNeededTrue.MatsSorbentTrapEndDateValid");
            }
        }

        /// <summary>
        ///A test for MatsTrp-4
        ///</summary>()
        [TestMethod()]
        public void MatsTrp4()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            /* Initialize General Variables */
            bool?[] booleanList = { null, false, true };
            int?[] hourList = { null, -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 };

            /* Initialize variables needed to run the check. */
            bool log = false;
            string actual;


            foreach (bool? dateValid in booleanList)
                foreach (int? hour in hourList)
                {
                    /*  Initialize Input Parameters*/
                    EmParameters.MatsSorbentTrapEndDateValid = dateValid;
                    EmParameters.MatsSorbentTrapRecord = new MatsSorbentTrapRecord(endHour: hour);

                    /*  Initialize Updatable Parameters*/
                    EmParameters.MatsSorbentTrapEvaluationNeeded = null;

                    /* Initialize Output Parameters */
                    EmParameters.MatsSorbentTrapEndDateHourValid = null;

                    /* Init Cateogry Result */
                    category.CheckCatalogResult = null;

                    /* Run Check */
                    actual = cMatsSorbentTrapChecks.MatsTrp4(category, ref log);

                    /* Check Result Labels */
                    string resultPrefix = (dateValid == null ? "DateValidNull" : (dateValid.Value ? "DateValid" : "DateInvalid")) + "." + (hour == null ? "NullHour" : "Hour" + hour.ToString().PadLeft(2, '0')) + ".";

                    /* Check Results */
                    Assert.AreEqual(string.Empty, actual);
                    Assert.AreEqual(false, log);

                    if (dateValid.Default(false))
                    {
                        Assert.AreEqual((hour == null) ? "A" : (((hour < 0) || (23 < hour)) ? "B" : null), category.CheckCatalogResult, resultPrefix + "Result");
                        Assert.AreEqual(((hour == null) || (hour < 0) || (23 < hour)) ? false : (bool?)null, EmParameters.MatsSorbentTrapEvaluationNeeded, resultPrefix + "MatsSorbentTrapEvaluationNeeded");
                        Assert.AreEqual(((hour != null) && ((0 <= hour) && (hour <= 23))), EmParameters.MatsSorbentTrapEndDateHourValid, resultPrefix + "MatsSorbentTrapEndDateHourValid");
                    }
                    else
                    {
                        Assert.AreEqual(null, category.CheckCatalogResult, resultPrefix + "Result");
                        Assert.AreEqual(null, EmParameters.MatsSorbentTrapEvaluationNeeded, resultPrefix + "MatsSorbentTrapEvaluationNeeded");
                        Assert.AreEqual(false, EmParameters.MatsSorbentTrapEndDateHourValid, resultPrefix + "MatsSorbentTrapEndDateHourValid");
                    }
                }
        }

        /// <summary>
        ///A test for MatsTrp-5
        ///</summary>()
        [TestMethod()]
        public void MatsTrp5()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            /* Initialize General Variables */
            DateTime beginDateHour = new DateTime(2014, 10, 31, 10, 0, 0);
            bool?[] booleanList = { null, false, true };
            int[] endDateHourOffsetList = { -1, 0, 1 };

            /* Initialize variables needed to run the check. */
            bool log = false;
            string actual;

            /* Run test cases */
            foreach (bool? beginDateHourValid in booleanList)
                foreach (bool? endDateHourValid in booleanList)
                    foreach (int endDateHourOffset in endDateHourOffsetList)
                    {
                        /*  Initialize Input Parameters*/
                        EmParameters.MatsSorbentTrapBeginDateHourValid = beginDateHourValid;
                        EmParameters.MatsSorbentTrapEndDateHourValid = endDateHourValid;
                        EmParameters.MatsSorbentTrapRecord = new MatsSorbentTrapRecord(beginDatehour: beginDateHour, endDatehour: beginDateHour.AddHours(endDateHourOffset));

                        /*  Initialize Updatable Parameters*/
                        EmParameters.MatsSorbentTrapEvaluationNeeded = null;

                        /* Initialize Output Parameters */
                        EmParameters.MatsSorbentTrapDatesAndHoursConsistent = null;

                        /* Init Cateogry Result */
                        category.CheckCatalogResult = null;

                        /* Run Check */
                        actual = cMatsSorbentTrapChecks.MatsTrp5(category, ref log);

                        /* Check Result Labels */
                        string resultPrefix = "inputs["
                                            + "beginValid: " + (beginDateHourValid == null ? "null" : beginDateHourValid.Value.ToString())
                                            + ", "
                                            + "endValid: " + (endDateHourValid == null ? "null" : endDateHourValid.Value.ToString())
                                            + ", "
                                            + "endHourOffset: " + endDateHourOffset.ToString()
                                            + "].";

                        /* Check results */
                        Assert.AreEqual(string.Empty, actual);
                        Assert.AreEqual(false, log);

                        if (beginDateHourValid.Default(false) && endDateHourValid.Default(false))
                        {
                            Assert.AreEqual((endDateHourOffset >= 0) ? null : "A", category.CheckCatalogResult, resultPrefix + "Result");
                            Assert.AreEqual((endDateHourOffset >= 0) ? (bool?)null : false, EmParameters.MatsSorbentTrapEvaluationNeeded, resultPrefix + "MatsSorbentTrapEvaluationNeeded");
                            Assert.AreEqual((endDateHourOffset >= 0), EmParameters.MatsSorbentTrapDatesAndHoursConsistent, resultPrefix + "MatsSorbentTrapDatesAndHoursConsistent");
                        }
                        else
                        {
                            Assert.AreEqual(null, category.CheckCatalogResult, resultPrefix + "Result");
                            Assert.AreEqual(null, EmParameters.MatsSorbentTrapEvaluationNeeded, resultPrefix + "MatsSorbentTrapEvaluationNeeded");
                            Assert.AreEqual(false, EmParameters.MatsSorbentTrapDatesAndHoursConsistent, resultPrefix + "MatsSorbentTrapDatesAndHoursConsistent");
                        }
                    }
        }

        /// <summary>
        ///A test for MatsTrp-6
        ///</summary>()
        [TestMethod()]
        public void MatsTrp6()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            /* Initialize General Variables */
            DateTime afterEnd = new DateTime(2014, 12, 31, 23, 0, 0);
            DateTime beforeBegin = new DateTime(2014, 10, 1, 0, 0, 0);
            DateTime currentBegin = new DateTime(2014, 11, 8, 22, 0, 0);
            DateTime currentEnd = new DateTime(2014, 11, 21, 10, 0, 0);
            string[] testCaseList = { "AfterOverlap", "AfterTransition", "AfterNoOverlap", "BeforeNoOverlap", "BeforeTransition", "BeforeOverlap", "Contained", "Contains", "Same" };

            /* Initialize variables needed to run the check. */
            bool log = false;
            string actual;

            /* Run test cases */
            foreach (string testCase in testCaseList)
            {
                /* Initialize Variables Used to Set Input Variables */
                DateTime afterBegin, beforeEnd, testBegin, testEnd;

                switch (testCase)
                {
                    case "AfterNoOverlap": { testBegin = currentEnd.AddHours(1); testEnd = currentEnd.AddDays(13); beforeEnd = currentBegin.AddHours(-1); afterBegin = testEnd.AddHours(1); }; break;
                    case "AfterTransition": { testBegin = currentEnd; testEnd = currentEnd.AddDays(13); beforeEnd = currentBegin.AddHours(-1); afterBegin = testEnd.AddHours(1); }; break;
                    case "AfterOverlap": { testBegin = currentEnd.AddHours(-1); testEnd = currentEnd.AddDays(13); beforeEnd = currentBegin.AddHours(-1); afterBegin = testEnd.AddHours(1); }; break;
                    case "BeforeNoOverlap": { testBegin = currentBegin.AddDays(-13); testEnd = currentBegin.AddHours(-1); beforeEnd = testBegin.AddHours(-1); afterBegin = currentEnd.AddHours(1); }; break;
                    case "BeforeTransition": { testBegin = currentBegin.AddDays(-13); testEnd = currentBegin; beforeEnd = testBegin.AddHours(-1); afterBegin = currentEnd.AddHours(1); }; break;
                    case "BeforeOverlap": { testBegin = currentBegin.AddDays(-13); testEnd = currentBegin.AddHours(1); beforeEnd = testBegin.AddHours(-1); afterBegin = currentEnd.AddHours(1); }; break;
                    case "Contained": { testBegin = currentBegin.AddHours(1); testEnd = currentEnd.AddHours(-1); beforeEnd = currentBegin.AddHours(-1); afterBegin = currentEnd.AddHours(1); }; break;
                    case "Contains": { testBegin = currentBegin.AddHours(-1); testEnd = currentEnd.AddHours(1); beforeEnd = testBegin.AddHours(-1); afterBegin = testEnd.AddHours(1); }; break;
                    case "Same":
                    default: { testBegin = currentBegin; testEnd = currentEnd; beforeEnd = currentBegin.AddHours(-1); afterBegin = currentEnd.AddHours(1); }; break;
                }

                /* Initialize MatsSorbentTrapRecord */
                EmParameters.MatsSorbentTrapRecord
                  = new MatsSorbentTrapRecord
                      (
                        trapId: "3",
                        monSysId: "SystemId2",
                        beginDatehour: currentBegin, beginDate: currentBegin.Date, beginHour: currentBegin.Hour,
                        endDatehour: currentEnd, endDate: currentEnd.Date, endHour: currentEnd.Hour
                      );

                /* Initialize MatsSorbentTrapRecords and include MatsSorbentTrapRecord */
                EmParameters.MatsSorbentTrapRecords
                  = new CheckDataView<MatsSorbentTrapRecord>
                  (
                    new MatsSorbentTrapRecord
                      (
                        trapId: "1",
                        monSysId: "SystemId1",
                        beginDatehour: beforeBegin, beginDate: beforeBegin.Date, beginHour: beforeBegin.Hour,
                        endDatehour: afterEnd, endDate: afterEnd.Date, endHour: afterEnd.Hour
                      ),
                    new MatsSorbentTrapRecord
                      (
                        trapId: "2",
                        monSysId: "SystemId2",
                        beginDatehour: beforeBegin, beginDate: beforeBegin.Date, beginHour: beforeBegin.Hour,
                        endDatehour: beforeEnd, endDate: beforeEnd.Date, endHour: beforeEnd.Hour
                      ),
                    EmParameters.MatsSorbentTrapRecord,
                    new MatsSorbentTrapRecord
                      (
                        trapId: "4",
                        monSysId: "SystemId2",
                        beginDatehour: testBegin, beginDate: testBegin.Date, beginHour: testBegin.Hour,
                        endDatehour: testEnd, endDate: testEnd.Date, endHour: testEnd.Hour
                      ),
                    new MatsSorbentTrapRecord
                      (
                        trapId: "5",
                        monSysId: "SystemId2",
                        beginDatehour: afterBegin, beginDate: afterBegin.Date, beginHour: afterBegin.Hour,
                        endDatehour: afterEnd, endDate: afterEnd.Date, endHour: afterEnd.Hour
                      ),
                    new MatsSorbentTrapRecord
                      (
                        trapId: "6",
                        monSysId: "SystemId3",
                        beginDatehour: beforeBegin, beginDate: beforeBegin.Date, beginHour: beforeBegin.Hour,
                        endDatehour: afterEnd, endDate: afterEnd.Date, endHour: afterEnd.Hour
                      )
                  );

                /*  Initialize Updatable Parameters*/
                EmParameters.MatsSorbentTrapEvaluationNeeded = null;

                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Run Check */
                actual = cMatsSorbentTrapChecks.MatsTrp6(category, ref log);

                /* Check Result Labels */
                string resultPrefix = testCase;

                /* Check results */
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);

                if (testCase.InList("BeforeNoOverlap,BeforeTransition,AfterNoOverlap,AfterTransition"))
                {
                    Assert.AreEqual(null, category.CheckCatalogResult, resultPrefix + ".Result");
                    Assert.AreEqual(null, EmParameters.MatsSorbentTrapEvaluationNeeded, resultPrefix + ".MatsSorbentTrapEvaluationNeeded");
                }
                else
                {
                    Assert.AreEqual("A", category.CheckCatalogResult, resultPrefix + ".Result");
                    Assert.AreEqual(false, EmParameters.MatsSorbentTrapEvaluationNeeded, resultPrefix + ".MatsSorbentTrapEvaluationNeeded");
                }
            }
        }


        [TestMethod()]
        public void MatsTrp7()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;


            /* Set Test Values */
            const string modcCd = "13";
            const string trapId = "GoodId";

            string[] caseList = { "EndsInQuarter", "CompletelyInQuarter", "BeginsInQuarter" };
            DateTime currentDateHour = new DateTime(2017, 6, 17, 22, 0, 0);
            DateTime quarterBegin = new DateTime(2017, 4, 1);
            DateTime quarterEnd = new DateTime(2017, 6, 30);


            /* Run Test Cases */
            foreach (string caseType in caseList)
            {
                /* Set Case Values */
                int? borderTrapInd, suppDataInd;
                DateTime? beginDateHour, endDateHour;
                {
                    switch (caseType)
                    {
                        case "EndsInQuarter": { borderTrapInd = 1; suppDataInd = 1; beginDateHour = quarterBegin.AddHours(-1); endDateHour = quarterBegin.AddHours(1); ; } break;
                        case "BeginsInQuarter": { borderTrapInd = 1; suppDataInd = 0; beginDateHour = quarterEnd.AddHours(-1); endDateHour = quarterEnd.AddHours(1); ; } break;
                        default: { borderTrapInd = 0; suppDataInd = 0; beginDateHour = quarterBegin.AddHours(1); endDateHour = quarterEnd.AddHours(-1); ; } break;
                    }
                }

                /* Initialize Input Parameters */
                EmParameters.CurrentMonitorPlanLocationPostion = 1;
                EmParameters.MatsSorbentTrapDictionary = new Dictionary<string, SorbentTrapEvalInformation>();
                EmParameters.MatsSorbentTrapRecord = SetValues.DateHour(new MatsSorbentTrapRecord(trapId: trapId, modcCd: modcCd, borderTrapInd: borderTrapInd, suppDataInd: suppDataInd), beginDateHour, endDateHour);

                /* Initialize Updatable Parameters */
                EmParameters.MatsSorbentTrapListByLocationArray = new List<SorbentTrapEvalInformation>[] { new List<SorbentTrapEvalInformation>(), new List<SorbentTrapEvalInformation>(), new List<SorbentTrapEvalInformation>() }; ;

                /* Initialize Output Parameters */
                EmParameters.MatsSamplingTrainProblemComponentExists = null;
                EmParameters.MatsSamplingTrainDictionary = null;
                EmParameters.MatsSorbentTrapValidExists = null;


                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Initialize variables needed to run the check. */
                bool log = false;
                string actual;

                /* Run Check */
                actual = cMatsSorbentTrapChecks.MatsTrp7(category, ref log);


                /* Check results */
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, string.Format("Result [{0}]", caseType));

                Assert.AreEqual(false, EmParameters.MatsSamplingTrainProblemComponentExists, string.Format("MatsSamplingTrainProblemComponentExists [{0}]", caseType));
                Assert.AreNotEqual(null, EmParameters.MatsSorbentTrapSamplingTrainList, string.Format("MatsSorbentTrapValidExists exists [{0}]", caseType));
                Assert.AreEqual(0, EmParameters.MatsSorbentTrapSamplingTrainList.Count, string.Format("MatsSorbentTrapSamplingTrainList count [{0}]", caseType));
                Assert.AreEqual(true, EmParameters.MatsSorbentTrapValidExists, string.Format("MatsSorbentTrapValidExists [{0}]", caseType));

                Assert.AreEqual(1, EmParameters.MatsSorbentTrapDictionary.Count, string.Format("MatsSorbentTrapDictionary.Count [{0}]", caseType));
                Assert.AreEqual(true, EmParameters.MatsSorbentTrapDictionary.ContainsKey(trapId), string.Format("MatsSorbentTrapDictionary.ContainsKey [{0}]", caseType));
                Assert.AreEqual(true, EmParameters.MatsSorbentTrapDictionary[trapId].SorbentTrapValidExists, string.Format("MatsSorbentTrapDictionary.SorbentTrapValidExists [{0}]", caseType));
                Assert.AreEqual((borderTrapInd == 1), EmParameters.MatsSorbentTrapDictionary[trapId].IsBorderTrap, string.Format("MatsSorbentTrapDictionary.IsBorderTrap [{0}]", caseType));
                Assert.AreEqual((suppDataInd == 1), EmParameters.MatsSorbentTrapDictionary[trapId].IsSupplementalData, string.Format("MatsSorbentTrapDictionary.IsSupplementalData [{0}]", caseType));
                Assert.AreEqual(trapId, EmParameters.MatsSorbentTrapDictionary[trapId].SorbentTrapId, string.Format("MatsSorbentTrapDictionary.SorbentTrapId [{0}]", caseType));
                Assert.AreEqual(beginDateHour, EmParameters.MatsSorbentTrapDictionary[trapId].SorbentTrapBeginDateHour, string.Format("MatsSorbentTrapDictionary.SorbentTrapBeginDateHour [{0}]", caseType));
                Assert.AreEqual(endDateHour, EmParameters.MatsSorbentTrapDictionary[trapId].SorbentTrapEndDateHour, string.Format("MatsSorbentTrapDictionary.SorbentTrapEndDateHour [{0}]", caseType));
                Assert.AreEqual(modcCd, EmParameters.MatsSorbentTrapDictionary[trapId].SorbentTrapModcCd, string.Format("MatsSorbentTrapDictionary.SorbentTrapModcCd [{0}]", caseType));
                Assert.AreEqual(false, EmParameters.MatsSorbentTrapDictionary[trapId].SamplingTrainProblemComponentExists, string.Format("MatsSorbentTrapDictionary.SamplingTrainProblemComponentExists [{0}]", caseType));
                Assert.AreNotEqual(null, EmParameters.MatsSorbentTrapDictionary[trapId].SamplingTrainList, string.Format("MatsSorbentTrapDictionary.SamplingTrainList exists [{0}]", caseType));
                Assert.AreEqual(0, EmParameters.MatsSorbentTrapDictionary[trapId].SamplingTrainList.Count, string.Format("MatsSorbentTrapDictionary.SamplingTrainList count [{0}]", caseType));
                Assert.AreNotEqual(null, EmParameters.MatsSorbentTrapDictionary[trapId].OperatingDateList, string.Format("MatsSorbentTrapDictionary.OperatingDateList exists [{0}]", caseType));
                Assert.AreEqual(0, EmParameters.MatsSorbentTrapDictionary[trapId].OperatingDateList.Count, string.Format("MatsSorbentTrapDictionary.OperatingDateList count [{0}]", caseType));

                Assert.AreEqual(3, EmParameters.MatsSorbentTrapListByLocationArray.Length, string.Format("MatsSorbentTrapListByLocationArray.Length [{0}]", caseType));
                Assert.AreNotEqual(null, EmParameters.MatsSorbentTrapListByLocationArray[0], string.Format("MatsSorbentTrapListByLocationArray exists [{0}]", caseType));
                Assert.AreEqual(0, EmParameters.MatsSorbentTrapListByLocationArray[0].Count, string.Format("MatsSorbentTrapListByLocationArray count [{0}]", caseType));
                Assert.AreNotEqual(null, EmParameters.MatsSorbentTrapListByLocationArray[1], string.Format("MatsSorbentTrapListByLocationArray exists [{0}]", caseType));
                Assert.AreEqual(1, EmParameters.MatsSorbentTrapListByLocationArray[1].Count, string.Format("MatsSorbentTrapListByLocationArray count [{0}]", caseType));
                Assert.AreEqual(trapId, EmParameters.MatsSorbentTrapListByLocationArray[1][0].SorbentTrapId, string.Format("MatsSorbentTrapListByLocationArray.SorbentTrapId [{0}]", caseType));
                Assert.AreNotEqual(null, EmParameters.MatsSorbentTrapListByLocationArray[2], string.Format("MatsSorbentTrapListByLocationArray exists [{0}]", caseType));
                Assert.AreEqual(0, EmParameters.MatsSorbentTrapListByLocationArray[2].Count, string.Format("MatsSorbentTrapListByLocationArray count [{0}]", caseType));
            }
        }


        /// <summary>
        /// 
        ///  MatsTrp8()
        /// 
        /// trapBeg: 2016-06-04 08
        /// trapEnd: 2016-06-17 22
        /// 
        /// | ## | sysId | sysType | sysBeg    | sysEnd    | validExists || result | validExists || Note
        /// |  0 | null  | null    | null      | null      | true        || A      | false       || System Id is missing from sorbent trap.
        /// |  1 | HERE  | null    | trapBeg   | trapEnd   | true        || B      | false       || System Type of the system for the sorbent trap is null.
        /// |  2 | HERE  | HG      | trapBeg   | trapEnd   | true        || B      | false       || System Type of the system for the sorbent trap is not ST.
        /// |  3 | HERE  | ST      | null      | trapEnd   | true        || null   | true        || System Begin Date-Hour of the system for the sorbent trap is null.
        /// |  4 | HERE  | ST      | null      | trapEnd   | false       || null   | false       || System Begin Date-Hour of the system for the sorbent trap is null, but ValidExists was false.
        /// |  5 | HERE  | ST      | trapBeg   | null      | true        || null   | true        || System End Date-Hour of the system for the sorbent trap is null.
        /// |  6 | HERE  | ST      | trapBeg   | null      | false       || null   | false       || System End Date-Hour of the system for the sorbent trap is null, but ValidExists was false.
        /// |  7 | HERE  | ST      | trapBeg   | trapEnd   | true        || null   | true        || System covers date-hours of the sorbent trap.
        /// |  8 | HERE  | ST      | trapBeg   | trapEnd   | false       || null   | false       || System covers date-hours of the sorbent trap, but ValidExists was false.
        /// |  9 | HERE  | ST      | trapBeg+1 | trapEnd   | true        || C      | false       || System begin after the sorbent trap begins.
        /// | 10 | HERE  | ST      | trapBeg   | trapEnd-1 | true        || C      | false       || System ends before the sorbent trap ends.
        /// </summary>
        [TestMethod()]
        public void MatsTrp8()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;


            /* Input Parameter Values */
            DateTime trapBeg = new DateTime(2016, 6, 4, 8, 0, 0);
            DateTime trapEnd = new DateTime(2016, 6, 17, 22, 0, 0);

            string[] sysIdList = { null, "HERE", "HERE", "HERE", "HERE", "HERE", "HERE", "HERE", "HERE", "HERE", "HERE" };
            string[] sysTypeList = { null, null, "HG", "ST", "ST", "ST", "ST", "ST", "ST", "ST", "ST" };
            DateTime?[] sysBegList = { null, trapBeg, trapBeg, null, null, trapBeg, trapBeg, trapBeg, trapBeg, trapBeg.AddHours(1), trapBeg};
            DateTime?[] sysEndList = { null, trapEnd, trapEnd, trapEnd, trapEnd, null, null, trapEnd, trapEnd, trapEnd, trapEnd.AddHours(-1) };
            bool?[] validExistsList = { true, true, true, true, false, true, false, true, false, true, true};

            /* Expected Values */
            string[] resultList = { "A", "B", "B", null, null, null, null, null, null, "C", "C" };
            bool?[] expValidExistsList = { false, false, false, true, false, true, false, true, false, false, false };

            /* Case Count */
            int caseCount = 11;

            /* Check array lengths */
            Assert.AreEqual(caseCount, sysIdList.Length, "sysIdList length");
            Assert.AreEqual(caseCount, sysTypeList.Length, "sysTypeList length");
            Assert.AreEqual(caseCount, sysBegList.Length, "sysBegList length");
            Assert.AreEqual(caseCount, sysEndList.Length, "sysEndList length");
            Assert.AreEqual(caseCount, validExistsList.Length, "validExistsList length");
            Assert.AreEqual(caseCount, resultList.Length, "resultList length");
            Assert.AreEqual(caseCount, expValidExistsList.Length, "expValidExistsList length");

            for (int caseDex = 0; caseDex < caseCount; caseDex++)
            {
                /* Initialize Input Parameters */
                EmParameters.MatsSorbentTrapRecord = SetValues.DateHour(new MatsSorbentTrapRecord(monSysId: sysIdList[caseDex], sysTypeCd: sysTypeList[caseDex],  
                                                                                                  systemBeginDatehour: sysBegList[caseDex], 
                                                                                                  systemEndDatehour: sysEndList[caseDex]),
                                                                        trapBeg, trapEnd);

                /*  Initialize Updatable Parameters*/
                EmParameters.MatsSorbentTrapValidExists = validExistsList[caseDex];

                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Initialize variables needed to run the check. */
                bool log = false;
                string actual;

                /* Run Check */
                actual = cMatsSorbentTrapChecks.MatsTrp8(category, ref log);

                /* Check results */
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);

                Assert.AreEqual(resultList[caseDex], category.CheckCatalogResult, string.Format("Result {0}", caseDex));
                Assert.AreEqual(expValidExistsList[caseDex], EmParameters.MatsSorbentTrapValidExists, string.Format("MatsSorbentTrapValidExists {0}", caseDex));
            }


        }


        /// <summary>
        ///A test for MatsTrp-9
        ///</summary>()
        [TestMethod()]
        public void MatsTrp9()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            /* Initialize General Variables */
            bool?[] booleanList = { null, false, true };
            bool[][] dictionaryValidValueList = { new bool[] { }, new bool[] { true }, new bool[] { false, false }, new bool[] { false, true }, new bool[] { true, false }, new bool[] { true, true }, new bool[] { true, true, true } };

            /* Initialize variables needed to run the check. */
            bool log = false;
            string actual;

            /* Test Cases */
            foreach (bool? problemComponentExists in booleanList)
                foreach (bool[] validValueList in dictionaryValidValueList)
                {
                    /* Initialize MatsSamplingTrainDictionary */
                    EmParameters.MatsSorbentTrapSamplingTrainList = new List<SamplingTrainEvalInformation>();

                    int valueCount = 0;
                    int validCount = 0;

                    foreach (bool validValue in validValueList)
                    {
                        valueCount++;

                        if (validValue) validCount++;

                        SamplingTrainEvalInformation samplingTrainEvalInformation = new SamplingTrainEvalInformation("SystemId" + valueCount.ToString());
                        samplingTrainEvalInformation.SamplingTrainValid = validValue;

                        EmParameters.MatsSorbentTrapSamplingTrainList.Add(samplingTrainEvalInformation);
                    }

                    /*  Initialize Input Parameters*/
                    EmParameters.MatsSamplingTrainProblemComponentExists = problemComponentExists;

                    /* Initialize Updatable Parameters*/
                    EmParameters.MatsSorbentTrapValidExists = null;

                    /* Initialize Output Parameters */
                    EmParameters.MatsSamplingTrainsValid = null;

                    /* Init Cateogry Result */
                    category.CheckCatalogResult = null;

                    /* Run Check */
                    actual = cMatsSorbentTrapChecks.MatsTrp9(category, ref log);

                    /* Check results */
                    Assert.AreEqual(string.Empty, actual);
                    Assert.AreEqual(false, log);

                    if (problemComponentExists.Default(true) == false)
                    {
                        Assert.AreEqual(valueCount == 2 ? null : "A", category.CheckCatalogResult, "NoComponentProblem.Result");
                        Assert.AreEqual((valueCount == 2 && validCount == 2) ? (bool?)null : false, EmParameters.MatsSorbentTrapValidExists, "NoComponentProblem.MatsSorbentTrapValidExists");
                        Assert.AreEqual((valueCount == 2 && validCount == 2) ? true : false, EmParameters.MatsSamplingTrainsValid, "NoComponentProblem.MatsSamplingTrainsValid");
                    }
                    else
                    {
                        Assert.AreEqual(null, category.CheckCatalogResult, "ComponentProblem.Result");
                        Assert.AreEqual(null, EmParameters.MatsSorbentTrapValidExists, "ComponentProblem.MatsSorbentTrapValidExists");
                        Assert.AreEqual(false, EmParameters.MatsSamplingTrainsValid, "ComponentProblem.MatsSamplingTrainsValid");
                    }
                }
        }

        /// <summary>
        ///A test for MatsTrp-10
        ///</summary>()
        [TestMethod()]
        public void MatsTrp10()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            /* Initialize General Variables */
            string[] statusList = { "BAD", "EXPIRED", "FAILED", "INC", "LOST", "PASSED", "UNCERTAIN" };

            /* Initialize variables needed to run the check. */
            bool log = false;
            string actual;

            /* Test Cases */
            for (int modcNo = 1; modcNo <= 55; modcNo++)
            {
                /* Initialize MatsSorbentTrapRecord */
                EmParameters.MatsSorbentTrapRecord = new MatsSorbentTrapRecord(modcCd: modcNo.ToString().PadLeft(2, '0'));

                foreach (string statusCd1 in statusList)
                    foreach (string statusCd2 in statusList)
                    {
                        /* Initialize MatsSorbentTrapSamplingTrainList */
                        EmParameters.MatsSorbentTrapSamplingTrainList = new List<SamplingTrainEvalInformation>();
                        {
                            EmParameters.MatsSorbentTrapSamplingTrainList.Add(new SamplingTrainEvalInformation("SystemId1"));
                            EmParameters.MatsSorbentTrapSamplingTrainList[0].TrainQAStatusCode = statusCd1;
                            EmParameters.MatsSorbentTrapSamplingTrainList.Add(new SamplingTrainEvalInformation("SystemId2"));
                            EmParameters.MatsSorbentTrapSamplingTrainList[1].TrainQAStatusCode = statusCd2;
                        }

                        /* Initialize General Input Parameters */
                        EmParameters.MatsSamplingTrainsValid = ((statusCd1 != "BAD") && (statusCd2 != "BAD"));

                        /* Initialize Updatable Parameters*/
                        EmParameters.MatsSorbentTrapValidExists = null;

                        /* Initialize Output Parameters */
                        EmParameters.MatsSorbentTrapModcCodeValid = null;

                        /* Init Cateogry Result */
                        category.CheckCatalogResult = null;

                        /* Run Check */
                        actual = cMatsSorbentTrapChecks.MatsTrp10(category, ref log);

                        /* Expected Results */
                        string result;
                        bool? modcCodeValid, validExists;

                        if ((statusCd1 != "BAD") && (statusCd2 != "BAD"))
                        {
                            switch (modcNo)
                            {
                                case 1: result = ((statusCd1 == "PASSED") && (statusCd2 == "PASSED")) ? null : "B"; break;
                                case 2: result = ((statusCd1 == "PASSED") && (statusCd2 == "PASSED")) ? null : "B"; break;
                                case 32: result = (((statusCd1 == "PASSED") && (statusCd2.InList("FAILED,LOST"))) || ((statusCd1.InList("FAILED,LOST")) && (statusCd2 == "PASSED"))) ? null : "C"; break;
                                case 33: result = ((statusCd1 == "UNCERTAIN") && (statusCd2 == "UNCERTAIN")) ? null : "D"; break;
                                case 34:
                                    {
                                        if ((statusCd1 == "FAILED") && (statusCd2 == "FAILED"))
                                            result = null;
                                        else if ((statusCd1 == "UNCERTAIN") && (statusCd2 == "UNCERTAIN"))
                                            result = null;
                                        else if (statusCd1.InList("LOST,EXPIRED,INC") || statusCd2.InList("LOST,EXPIRED,INC"))
                                            result = null;
                                        else
                                            result = "E";
                                    }
                                    break;
                                case 35: result = null; break;
                                case 43: result = ((statusCd1 == "PASSED") && (statusCd2 == "PASSED")) ? null : "B"; break;
                                case 44: result = (((statusCd1 == "PASSED") && (statusCd2.InList("FAILED,LOST"))) || ((statusCd1.InList("FAILED,LOST")) && (statusCd2 == "PASSED"))) ? null : "C"; break;
                                default: result = "A"; break;
                            }

                            validExists = (result == null ? (bool?)null : false);
                            modcCodeValid = (result == null ? true : false);
                        }
                        else // MatsSamplingTrainsValid is false
                        {
                            result = (modcNo.ToString().InList("1,2,32,33,34,35,43,44") ? null : "A");
                            validExists = (modcNo.ToString().InList("1,2,32,33,34,35,43,44") ? (bool?)null : false);
                            modcCodeValid = false;
                        }

                        /* Check Result Label */
                        string resultPrefix = "[modcCd: " + modcNo.ToString().PadLeft(2, '0') + ", status1: " + statusCd1 + ", status2: " + statusCd2 + "]";

                        /* Check results */
                        Assert.AreEqual(string.Empty, actual);
                        Assert.AreEqual(false, log);

                        Assert.AreEqual(result, category.CheckCatalogResult, resultPrefix + ".Result");
                        Assert.AreEqual(validExists, EmParameters.MatsSorbentTrapValidExists, resultPrefix + ".MatsSorbentTrapValidExists");
                        Assert.AreEqual(modcCodeValid, EmParameters.MatsSorbentTrapModcCodeValid, resultPrefix + ".MatsSorbentTrapModcCodeValid");
                    }
            }
        }

        /// <summary>
        /// MatsTrp11
        /// 
        /// | ## | Valid | Agreement | TrapHg | MODC | AbsInd | TrainHg0 | TrainHg1 | Exists || Result | Valid | AbsDiff | PctDiff | Exists || Note
        /// |  0 | null  | null      | 0.00E0 | 01   | null   | 0        | 0        | true   || null   | false | null    | null    | true   || MatsSorbentTrapMODCCodeValid is null resulting in no real checking
        /// |  1 | false | null      | 0.00E0 | 01   | null   | 0        | 0        | true   || null   | false | null    | null    | true   || MatsSorbentTrapMODCCodeValid is false resulting in no real checking
        /// |  2 | true  | null      | 0.00E0 | 01   | null   | 0        | 0        | true   || A      | false | null    | null    | false  || Null PTA and measured MODC, producing result A.
        /// |  3 | true  | null      | 0.00E0 | 02   | null   | 0        | 0        | true   || A      | false | null    | null    | false  || Null PTA and measured MODC, producing result A.
        /// |  4 | true  | null      | 0.00E0 | 33   | null   | 0        | 0        | true   || A      | false | null    | null    | false  || Null PTA and measured MODC, producing result A.
        /// |  5 | true  | null      | 0.00E0 | 32   | 0      | 0        | 0        | true   || B      | false | null    | null    | false  || Null PTA, non measured MODC, and non null AbsoluteDifferenceIndicator, producing result B.
        /// |  6 | true  | null      | 0.00E0 | 32   | 1      | 0        | 0        | true   || B      | false | null    | null    | false  || Null PTA, non measured MODC, and non null AbsoluteDifferenceIndicator, producing result B.
        /// |  7 | true  | null      | 0.00E0 | 34   | 1      | 0        | 0        | true   || B      | false | null    | null    | false  || Null PTA, non measured MODC, and non null AbsoluteDifferenceIndicator, producing result B.
        /// |  8 | true  | null      | 0.00E0 | 35   | 1      | 0        | 0        | true   || B      | false | null    | null    | false  || Null PTA, non measured MODC, and non null AbsoluteDifferenceIndicator, producing result B.
        /// |  9 | true  | null      | 0.00E0 | 32   | null   | 0        | 0        | true   || null   | true  | null    | null    | true   || Null PTA, non measured MODC, and null AbsoluteDifferenceIndicator, producing no result.
        /// | 10 | true  | null      | 0.00E0 | 34   | null   | 0        | 0        | true   || null   | true  | null    | null    | true   || Null PTA, non measured MODC, and null AbsoluteDifferenceIndicator, producing no result.
        /// | 11 | true  | null      | 0.00E0 | 35   | null   | 0        | 0        | true   || null   | true  | null    | null    | true   || Null PTA, non measured MODC, and null AbsoluteDifferenceIndicator, producing no result.
        /// | 12 | true  | null      | 0.00E0 | 35   | null   | 0        | 0        | false  || null   | true  | null    | null    | false  || Null PTA, non measured MODC, and null AbsoluteDifferenceIndicator, producing no result, but MatsSorbentTrapValidExists remains false.
        /// | 13 | true  | 4.001     | 0.00E0 | 32   | null   | 0        | 0        | true   || C      | false | null    | null    | false  || PTA exists with non measured MODC, producing result C.
        /// | 14 | true  | 4.001     | 0.00E0 | 34   | null   | 0        | 0        | true   || C      | false | null    | null    | false  || PTA exists with non measured MODC, producing result C.
        /// | 15 | true  | 4.001     | 0.00E0 | 35   | null   | 0        | 0        | true   || C      | false | null    | null    | false  || PTA exists with non measured MODC, producing result C.
        /// | 16 | true  | 4.001     | 0.00E0 | 01   | null   | 0        | 0        | true   || D      | false | null    | null    | false  || PTA exists with non measured MODC, but PTA is not round and therefore produces result D.
        /// | 17 | true  | 4.001     | 0.00E0 | 02   | null   | 0        | 0        | true   || D      | false | null    | null    | false  || PTA exists with non measured MODC, but PTA is not round and therefore produces result D.
        /// | 18 | true  | 4.001     | 0.00E0 | 33   | null   | 0        | 0        | true   || D      | false | null    | null    | false  || PTA exists with non measured MODC, but PTA is not round and therefore produces result D.
        /// | 19 | true  | 4.01      | 0.00E0 | 01   | null   | 0        | 0        | true   || K      | false | null    | null    | false  || PTA exists with non measured MODC, but AbsoluteDifferenceIndicator is null and therefore produces result K.
        /// | 20 | true  | 4.01      | 0.00E0 | 02   | null   | 0        | 0        | true   || K      | false | null    | null    | false  || PTA exists with non measured MODC, but AbsoluteDifferenceIndicator is null and therefore produces result K.
        /// | 21 | true  | 4.01      | 0.00E0 | 33   | null   | 0        | 0        | true   || K      | false | null    | null    | false  || PTA exists with non measured MODC, but AbsoluteDifferenceIndicator is null and therefore produces result K.
        /// | 22 | true  | 9.99      | 1.10E0 | 01   | 0      | 1.1      | 0.9      | true   || G      | false | 0.20    | 10.00   | false  || PTA exists and ABS Ind is 0, but does not agree with calculated value which produces result G.
        /// | 23 | true  | 10.01     | 1.10E0 | 01   | 0      | 1.1      | 0.9      | true   || G      | false | 0.20    | 10.00   | false  || PTA exists and ABS Ind is 0, but does not agree with calculated value which produces result G.
        /// | 24 | true  | 10.01     | 1.10E0 | 02   | 0      | 1.1      | 0.9      | true   || G      | false | 0.20    | 10.00   | false  || PTA exists and ABS Ind is 0, but does not agree with calculated value which produces result G.
        /// | 25 | true  | 10.01     | 1.10E0 | 33   | 0      | 1.1      | 0.9      | true   || G      | false | 0.20    | 10.00   | false  || PTA exists and ABS Ind is 0, but does not agree with calculated value which produces result G.
        /// | 26 | true  | 10.00     | 1.10E0 | 01   | 0      | 1.1      | 0.9      | false  || null   | true  | 0.20    | 10.00   | false  || ABS Ind is 0, PTA is less than or equal to 10% and MODC is 01, producing no result, but MatsSorbentTrapValidExists remains false.
        /// | 27 | true  | 10.00     | 1.10E0 | 01   | 0      | 1.1      | 0.9      | true   || null   | true  | 0.20    | 10.00   | true   || ABS Ind is 0, PTA is less than or equal to 10% and MODC is 01, producing no result.
        /// | 28 | true  | 10.00     | 1.10E0 | 02   | 0      | 1.1      | 0.9      | true   || null   | true  | 0.20    | 10.00   | true   || ABS Ind is 0, PTA is less than or equal to 10% and MODC is 02, producing no result.
        /// | 29 | true  | 10.00     | 1.10E0 | 33   | 0      | 1.1      | 0.9      | true   || H      | false | 0.20    | 10.00   | false  || ABS Ind is 0 and PTA is less than or equal to 10%, but MODC 33 which producs result H.
        /// | 30 | true  | 10.01     | 1.10E0 | 01   | 0      | 1.1001   | 0.8999   | true   || J      | false | 0.20    | 10.01   | false  || ABS Ind is 0, PTA is greater than 10.0, system Hg is greater than 1.0 and MODC is 01, which produces result J.
        /// | 31 | true  | 10.01     | 1.10E0 | 02   | 0      | 1.1001   | 0.8999   | true   || J      | false | 0.20    | 10.01   | false  || ABS Ind is 0, PTA is greater than 10.0, system Hg is greater than 1.0 and MODC is 02, which produces result J.
        /// | 32 | true  | 10.01     | 1.10E0 | 33   | 0      | 1.1001   | 0.8999   | true   || null   | true  | 0.20    | 10.01   | true   || ABS Ind is 0, PTA is greater than 10.0, system Hg is greater than 1.0 and MODC is 33, producing no result.
        /// | 33 | true  | 10.01     | 1.10E0 | 33   | 0      | 1.1001   | 0.8999   | false  || null   | true  | 0.20    | 10.01   | false  || ABS Ind is 0, PTA is greater than 10.0, system Hg is greater than 1.0 and MODC is 33, producing no result, but MatsSorbentTrapValidExists remains false.
        /// | 34 | true  | 10.01     | 1.00E0 | 01   | 0      | 1.1001   | 0.8999   | false  || null   | true  | 0.20    | 10.01   | false  || ABS Ind is 0, PTA is greater than 10.0, system Hg is less than or equal to 1.0 and MODC is 01, producing no result, but MatsSorbentTrapValidExists remains false.
        /// | 35 | true  | 10.01     | 1.00E0 | 01   | 0      | 1.1001   | 0.8999   | true   || null   | true  | 0.20    | 10.01   | true   || ABS Ind is 0, PTA is greater than 10.0, system Hg is less than or equal to 1.0 and MODC is 01, producing no result.
        /// | 36 | true  | 10.01     | 1.00E0 | 02   | 0      | 1.1001   | 0.8999   | true   || null   | true  | 0.20    | 10.01   | true   || ABS Ind is 0, PTA is greater than 10.0, system Hg is less than or equal to 1.0 and MODC is 02, producing no result.
        /// | 37 | true  | 10.01     | 1.00E0 | 33   | 0      | 1.1001   | 0.8999   | true   || I      | false | 0.20    | 10.01   | false  || ABS Ind is 0, PTA is greater than 10.0, system Hg is less than or equal to 1.0 and MODC is 33, which produces result I.
        /// | 38 | true  | 20.00     | 1.10E0 | 01   | 0      | 1.2      | 0.8      | true   || J      | false | 0.40    | 20.00   | false  || ABS Ind is 0, PTA is equal to 20.0, system Hg is greater than 1.0 and MODC is 01, which produces result J.
        /// | 39 | true  | 20.00     | 1.10E0 | 02   | 0      | 1.2      | 0.8      | true   || J      | false | 0.40    | 20.00   | false  || ABS Ind is 0, PTA is equal to 20.0, system Hg is greater than 1.0 and MODC is 02, which produces result J.
        /// | 40 | true  | 20.00     | 1.10E0 | 33   | 0      | 1.2      | 0.8      | true   || null   | true  | 0.40    | 20.00   | true   || ABS Ind is 0, PTA is equal to 20.0, system Hg is greater than 1.0 and MODC is 33, producing no result.
        /// | 41 | true  | 20.00     | 1.00E0 | 01   | 0      | 1.2      | 0.8      | true   || null   | true  | 0.40    | 20.00   | true   || ABS Ind is 0, PTA is equal to 20.0, system Hg is less than or equal to 1.0 and MODC is 01, producing no result.
        /// | 42 | true  | 20.00     | 1.00E0 | 02   | 0      | 1.2      | 0.8      | true   || null   | true  | 0.40    | 20.00   | true   || ABS Ind is 0, PTA is equal to 20.0, system Hg is less than or equal to 1.0 and MODC is 02, producing no result.
        /// | 43 | true  | 20.00     | 1.00E0 | 33   | 0      | 1.2      | 0.8      | true   || I      | false | 0.40    | 20.00   | false  || ABS Ind is 0, PTA is equal to 20.0, system Hg is less than or equal to 1.0 and MODC is 33, which produces result I.
        /// | 44 | true  | 20.01     | 1.10E0 | 01   | 0      | 1.2001   | 0.7999   | true   || J      | false | 0.40    | 20.01   | false  || ABS Ind is 0, PTA is greater than 20.0 and MODC is 01, which produces result J.
        /// | 45 | true  | 20.01     | 1.10E0 | 02   | 0      | 1.2001   | 0.7999   | true   || J      | false | 0.40    | 20.01   | false  || ABS Ind is 0, PTA is greater than 20.0 and MODC is 02, which produces result J.
        /// | 46 | true  | 20.01     | 1.10E0 | 33   | 0      | 1.2001   | 0.7999   | true   || null   | true  | 0.40    | 20.01   | true   || ABS Ind is 0, PTA is greater than 20.0 and MODC is 33, producing no result.
        /// | 47 | true  | 0.04      | 1.10E0 | 01   | 1      | 0.06     | 0.02     | true   || F      | false | 0.04    | 50.00   | false  || ABS Ind is 1 and PTA is greater than 0.3, which produces result F.
        /// | 48 | true  | 0.04      | 1.10E0 | 02   | 1      | 0.06     | 0.02     | true   || F      | false | 0.04    | 50.00   | false  || ABS Ind is 1 and PTA is greater than 0.3, which produces result F.
        /// | 49 | true  | 0.04      | 1.10E0 | 33   | 1      | 0.06     | 0.02     | true   || F      | false | 0.04    | 50.00   | false  || ABS Ind is 1 and PTA is greater than 0.3, which produces result F.
        /// | 50 | true  | 0.03      | 1.10E0 | 01   | 1      | 0.05     | 0.02     | true   || null   | true  | 0.03    | 42.86   | true   || ABS Ind is 1 and PTA is equal to 0.3, producing no result.
        /// | 51 | true  | 0.03      | 1.10E0 | 02   | 1      | 0.05     | 0.02     | true   || null   | true  | 0.03    | 42.86   | true   || ABS Ind is 1 and PTA is equal to 0.3, producing no result.
        /// | 52 | true  | 0.03      | 1.10E0 | 33   | 1      | 0.05     | 0.02     | true   || null   | true  | 0.03    | 42.86   | true   || ABS Ind is 1 and PTA is equal to 0.3, producing no result.
        /// | 53 | true  | 0.03      | 1.10E0 | 33   | 1      | 0.05     | 0.02     | false  || null   | true  | 0.03    | 42.86   | false  || ABS Ind is 1 and PTA is equal to 0.3, producing no result, but MatsSorbentTrapValidExists remains false.
        /// | 54 | true  | 0.03      | 1.10E0 | 01   | 1      | 0.05     | 0.03     | true   || E      | false | 0.02    | 25.00   | false  || ABS Ind is 1 and PTA is equal to 0.3, but does not equal calculated value, which produces result E.
        /// | 55 | true  | 0.03      | 1.10E0 | 02   | 1      | 0.05     | 0.03     | true   || E      | false | 0.02    | 25.00   | false  || ABS Ind is 1 and PTA is equal to 0.3, but does not equal calculated value, which produces result E.
        /// | 56 | true  | 0.03      | 1.10E0 | 33   | 1      | 0.05     | 0.03     | true   || E      | false | 0.02    | 25.00   | false  || ABS Ind is 1 and PTA is equal to 0.3, but does not equal calculated value, which produces result E.
        /// | 57 | true  | 0.00      | 1.10E0 | 01   | 0      | 0.00     | 0.00     | true   || null   | true  | 0.00    | 0.00    | true   || Divide by zero test when producing MatsCalcTrapPercentDifference.
        /// | 58 | true  | null      | 0.00E0 | 43   | null   | 0        | 0        | true   || A      | false | null    | null    | false  || Null PTA and measured MODC, producing result A.
        /// | 59 | true  | 4.001     | 0.00E0 | 43   | null   | 0        | 0        | true   || D      | false | null    | null    | false  || PTA exists with non measured MODC, but PTA is not round and therefore produces result D.
        /// | 60 | true  | 4.01      | 0.00E0 | 43   | null   | 0        | 0        | true   || K      | false | null    | null    | false  || PTA exists with non measured MODC, but AbsoluteDifferenceIndicator is null and therefore produces result K.
        /// | 61 | true  | 10.01     | 1.10E0 | 43   | 0      | 1.1      | 0.9      | true   || G      | false | 0.20    | 10.00   | false  || PTA exists and ABS Ind is 0, but does not agree with calculated value which produces result G.
        /// | 62 | true  | 10.00     | 1.10E0 | 43   | 0      | 1.1      | 0.9      | true   || null   | true  | 0.20    | 10.00   | false  || ABS Ind is 0 and PTA is less than or equal to 10%, but MODC 33 which producs result H.
        /// | 63 | true  | 10.01     | 1.10E0 | 43   | 0      | 1.1001   | 0.8999   | true   || J      | false | 0.20    | 10.01   | true   || ABS Ind is 0, PTA is greater than 10.0, system Hg is greater than 1.0 and MODC is 33, producing no result.
        /// | 64 | true  | 10.01     | 1.10E0 | 43   | 0      | 1.1001   | 0.8999   | false  || J      | false | 0.20    | 10.01   | false  || ABS Ind is 0, PTA is greater than 10.0, system Hg is greater than 1.0 and MODC is 33, producing no result,
        /// | 65 | true  | 10.01     | 1.00E0 | 43   | 0      | 1.1001   | 0.8999   | true   || null   | false | 0.20    | 10.01   | false  || ABS Ind is 0, PTA is greater than 10.0, system Hg is less than or equal to 1.0 and MODC is 33, which produces result I.
        /// | 66 | true  | 20.00     | 1.10E0 | 43   | 0      | 1.2      | 0.8      | true   || J      | false | 0.40    | 20.00   | true   || ABS Ind is 0, PTA is equal to 20.0, system Hg is greater than 1.0 and MODC is 33, producing no result.
        /// | 67 | true  | 20.00     | 1.00E0 | 43   | 0      | 1.2      | 0.8      | true   || null   | true  | 0.40    | 20.00   | false  || ABS Ind is 0, PTA is equal to 20.0, system Hg is less than or equal to 1.0 and MODC is 33, which produces result I.
        /// | 68 | true  | 20.01     | 1.10E0 | 43   | 0      | 1.2001   | 0.7999   | true   || J      | false | 0.40    | 20.01   | true   || ABS Ind is 0, PTA is greater than 20.0 and MODC is 33, producing no result.
        /// | 69 | true  | 0.04      | 1.10E0 | 43   | 1      | 0.06     | 0.02     | true   || F      | false | 0.04    | 50.00   | false  || ABS Ind is 1 and PTA is greater than 0.3, which produces result F.
        /// | 70 | true  | 0.03      | 1.10E0 | 43   | 1      | 0.05     | 0.02     | true   || null   | true  | 0.03    | 42.86   | true   || ABS Ind is 1 and PTA is equal to 0.3, producing no result.
        /// | 71 | true  | 0.03      | 1.10E0 | 43   | 1      | 0.05     | 0.02     | false  || null   | true  | 0.03    | 42.86   | false  || ABS Ind is 1 and PTA is equal to 0.3, producing no result,
        /// | 72 | true  | 0.03      | 1.10E0 | 43   | 1      | 0.05     | 0.03     | true   || E      | false | 0.02    | 25.00   | false  || ABS Ind is 1 and PTA is equal to 0.3, but does not equal calculated value, which produces result E.
        /// | 73 | true  | null      | 0.00E0 | 44   | 1      | 0        | 0        | true   || B      | false | null    | null    | false  || Null PTA, non measured MODC, and non null AbsoluteDifferenceIndicator, producing result B.
        /// | 74 | true  | null      | 0.00E0 | 44   | null   | 0        | 0        | true   || null   | true  | null    | null    | true   || Null PTA, non measured MODC, and null AbsoluteDifferenceIndicator, producing no result.
        /// | 75 | true  | null      | 0.00E0 | 44   | null   | 0        | 0        | false  || null   | true  | null    | null    | false  || Null PTA, non measured MODC, and null AbsoluteDifferenceIndicator, producing no result, but MatsSorbentTrapValidExists remains false.
        /// | 76 | true  | 4.001     | 0.00E0 | 44   | null   | 0        | 0        | true   || C      | false | null    | null    | false  || PTA exists with non measured MODC, producing result C.
        /// </summary>
        [TestMethod()]
        public void MatsTrp11()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;


            /* Input Parameter Values */
            bool?[] validList = { null, false, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true,
                                  true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true,
                                  true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true,
                                  true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true };
            decimal?[] agreementList = { null, null, null, null, null, null, null, null, null, null, null, null, null, 4.001m, 4.001m, 4.001m, 4.001m, 4.001m, 4.001m, 4.01m,
                                         4.01m, 4.01m, 9.99m, 10.01m, 10.01m, 10.01m, 10.00m, 10.00m, 10.00m, 10.00m, 10.01m, 10.01m, 10.01m, 10.01m, 10.01m, 10.01m, 10.01m, 10.01m, 20.00m, 20.00m,
                                         20.00m, 20.00m, 20.00m, 20.00m, 20.01m, 20.01m, 20.01m, 0.04m, 0.04m, 0.04m, 0.03m, 0.03m, 0.03m, 0.03m, 0.03m, 0.03m, 0.03m, 0.00m, null, 4.001m, 4.01m,
                                         10.01m, 10.00m, 10.01m, 10.01m, 10.01m, 20.00m, 20.00m, 20.01m, 0.04m, 0.03m, 0.03m, 0.03m, null, null, null, 4.001m };
            string[] trapHgList = { "0.00E0", "0.00E0", "0.00E0", "0.00E0", "0.00E0", "0.00E0", "0.00E0", "0.00E0", "0.00E0", "0.00E0", "0.00E0", "0.00E0", "0.00E0", "0.00E0", "0.00E0", "0.00E0", "0.00E0", "0.00E0", "0.00E0", "0.00E0",
                                    "0.00E0", "0.00E0", "1.10E0", "1.10E0", "1.10E0", "1.10E0", "1.10E0", "1.10E0", "1.10E0", "1.10E0", "1.10E0", "1.10E0", "1.10E0", "1.10E0", "1.00E0", "1.00E0", "1.00E0", "1.00E0", "1.10E0", "1.10E0",
                                    "1.10E0", "1.00E0", "1.00E0", "1.00E0", "1.10E0", "1.10E0", "1.10E0", "1.10E0", "1.10E0", "1.10E0", "1.10E0", "1.10E0", "1.10E0", "1.10E0", "1.10E0", "1.10E0", "1.10E0", "1.10E0", "0.00E0", "0.00E0",
                                    "0.00E0", "1.10E0", "1.10E0", "1.10E0", "1.10E0", "1.00E0", "1.10E0", "1.00E0", "1.10E0", "1.10E0", "1.10E0", "1.10E0", "1.10E0", "0.00E0", "0.00E0", "0.00E0", "0.00E0"};
            string[] modcList = { "01", "01", "01", "02", "33", "32", "32", "34", "35", "32", "34", "35", "35", "32", "34", "35", "01", "02", "33", "01",
                                  "02", "33", "01", "01", "02", "33", "01", "01", "02", "33", "01", "02", "33", "33", "01", "01", "02", "33", "01", "02",
                                  "33", "01", "02", "33", "01", "02", "33", "01", "02", "33", "01", "02", "33", "33", "01", "02", "33", "01", "43", "43",
                                  "43", "43", "43", "43", "43", "43", "43", "43", "43", "43", "43", "43", "43", "44", "44", "44", "44" };
            int?[] absIndList = { null, null, null, null, null, 0, 1, 1, 1, null, null, null, null, null, null, null, null, null, null, null,
                                  null, null, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                  0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, null, null, null, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 
                                  null, null, null };
            decimal?[] trainHg0List = { 0m, 0m, 0m, 0m, 0m, 0m, 0m, 0m, 0m, 0m, 0m, 0m, 0m, 0m, 0m, 0m, 0m, 0m, 0m, 0m,
                                        0m, 0m, 1.1m, 1.1m, 1.1m, 1.1m, 1.1m, 1.1m, 1.1m, 1.1m, 1.1001m, 1.1001m, 1.1001m, 1.1001m, 1.1001m, 1.1001m, 1.1001m, 1.1001m, 1.2m, 1.2m,
                                        1.2m, 1.2m, 1.2m, 1.2m, 1.2001m, 1.2001m, 1.2001m, 0.06m, 0.06m, 0.06m, 0.05m, 0.05m, 0.05m, 0.05m, 0.05m, 0.05m, 0.05m, 0m, 0m, 0m, 0m,
                                        1.1m, 1.1m, 1.1001m, 1.1001m, 1.1001m, 1.2m, 1.2m, 1.2001m, 0.06m, 0.05m, 0.05m, 0.05m, 0m, 0m, 0m, 0m };
            decimal?[] trainHg1List = { 0m, 0m, 0m, 0m, 0m, 0m, 0m, 0m, 0m, 0m, 0m, 0m, 0m, 0m, 0m, 0m, 0m, 0m, 0m, 0m,
                                        0m, 0m, 0.9m, 0.9m, 0.9m, 0.9m, 0.9m, 0.9m, 0.9m, 0.9m, 0.8999m, 0.8999m, 0.8999m, 0.8999m, 0.8999m, 0.8999m, 0.8999m, 0.8999m, 0.8m, 0.8m,
                                        0.8m, 0.8m, 0.8m, 0.8m, 0.7999m, 0.7999m, 0.7999m, 0.02m, 0.02m, 0.02m, 0.02m, 0.02m, 0.02m, 0.02m, 0.03m, 0.03m, 0.03m, 0m, 0m, 0m, 0m,
                                        0.9m, 0.9m, 0.8999m, 0.8999m, 0.8999m, 0.8m, 0.8m, 0.7999m, 0.02m, 0.02m, 0.02m, 0.03m, 0m, 0m, 0m, 0m };
            bool?[] existsList = { true, true, true, true, true, true, true, true, true, true, true, true, true, true, false, true, true, true, true, true,
                                   true, true, true, true, true, true, true, true, false, true, true, true, true, true, true, false, false, true, true, true,
                                   true, true, true, true, true, true, true, true, true, true, true, true, true, false, true, true, true, true, true, true,
                                   true, true, true, true, false, true, true, true, true, true, true, false, true, true, true, false, true };

            /* Expected Values */
            string[] expResultList = { null, null, "A", "A", "A", "B", "B", "B", "B", null, null, null, null, "C", "C", "C", "D", "D", "D", "K",
                                       "K", "K", "G", "G", "G", "G", null, null, null, "H", "J", "J", null, null, null, null, null, "I", "J", "J",
                                       null, null, null, "I", "J", "J", null, "F", "F", "F", null, null, null, null, "E", "E", "E", null, "A", "D",
                                       "K", "G", null, "J",  "J", null, "J", null, "J", "F", null, null, "E", "B", null, null, "C" };
            bool?[] expValidList = { false, false, false, false, false, false, false, false, false, true, true, true, true, false, false, false, false, false, false, false,
                                     false, false, false, false, false, false, true, true, true, false, false, false, true, true, true, true, true, false, false, false,
                                     true, true, true, false, false, false, true, false, false, false, true, true, true, true, false, false, false, true, false, false,
                                     false, false, true, false, false, true, false, true, false, false, true, true, false, false, true, true, false };
            decimal?[] expAbsDiffList = { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null,
                                          null, null, 0.20m, 0.20m, 0.20m, 0.20m, 0.20m, 0.20m, 0.20m, 0.20m, 0.20m, 0.20m, 0.20m, 0.20m, 0.20m, 0.20m, 0.20m, 0.20m, 0.40m, 0.40m,
                                          0.40m, 0.40m, 0.40m, 0.40m, 0.40m, 0.40m, 0.40m, 0.04m, 0.04m, 0.04m, 0.03m, 0.03m, 0.03m, 0.03m, 0.02m, 0.02m, 0.02m, 0.00m, null, null,
                                          null, 0.20m, 0.20m, 0.20m, 0.20m, 0.20m, 0.40m, 0.40m, 0.40m, 0.04m, 0.03m, 0.03m, 0.02m, null, null, null, null };
            decimal?[] expPctDiffList = { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null,
                                          null, null, 10m, 10m, 10m, 10m, 10m, 10m, 10m, 10m, 10.01m, 10.01m, 10.01m, 10.01m, 10.01m, 10.01m, 10.01m, 10.01m, 20m, 20m,
                                          20m, 20m, 20m, 20m, 20.01m, 20.01m, 20.01m, 50m, 50m, 50m, 42.86m, 42.86m, 42.86m, 42.86m, 25m, 25m, 25m, 0.00m, null, null,
                                          null, 10.00m, 10.00m, 10.01m, 10.01m, 10.01m, 20.00m, 20.00m, 20.01m, 50.00m, 42.86m, 42.86m, 25.00m, null, null, null, null };

            /* Test Case Count */
            int caseCount = 77;

            /* Check array lengths */
            Assert.AreEqual(caseCount, validList.Length, "validList length");
            Assert.AreEqual(caseCount, agreementList.Length, "agreementList length");
            Assert.AreEqual(caseCount, trapHgList.Length, "trapHgList length");
            Assert.AreEqual(caseCount, modcList.Length, "modcList length");
            Assert.AreEqual(caseCount, absIndList.Length, "absIndList length");
            Assert.AreEqual(caseCount, trainHg0List.Length, "trainHg0List length");
            Assert.AreEqual(caseCount, trainHg1List.Length, "trainHg1List length");
            Assert.AreEqual(caseCount, existsList.Length, "existsList length");
            Assert.AreEqual(caseCount, expResultList.Length, "expResultList length");
            Assert.AreEqual(caseCount, expValidList.Length, "expValidList length");
            Assert.AreEqual(caseCount, expAbsDiffList.Length, "expAbsDiffList length");
            Assert.AreEqual(caseCount, expPctDiffList.Length, "expPctDiffList length");


            /* Run Cases */
            for (int caseDex = 0; caseDex < caseCount; caseDex++)
            {
                /* Initialize Input Parameters */
                EmParameters.MatsSorbentTrapModcCodeValid = validList[caseDex];
                EmParameters.MatsSorbentTrapRecord = new MatsSorbentTrapRecord( modcCd: modcList[caseDex], absoluteDifferenceInd: absIndList[caseDex], pairedTrapAgreement: agreementList[caseDex], hgConcentration: trapHgList[caseDex] );
                EmParameters.MatsSorbentTrapSamplingTrainList = new List<SamplingTrainEvalInformation>();
                {
                    EmParameters.MatsSorbentTrapSamplingTrainList.Add(new SamplingTrainEvalInformation("TrainHg0"));
                    EmParameters.MatsSorbentTrapSamplingTrainList[0].HgConcentration = trainHg0List[caseDex];
                    EmParameters.MatsSorbentTrapSamplingTrainList.Add(new SamplingTrainEvalInformation("TrainHg1"));
                    EmParameters.MatsSorbentTrapSamplingTrainList[1].HgConcentration = trainHg1List[caseDex];
                }

                /* Initialize Updatable Parameters*/
                EmParameters.MatsSorbentTrapValidExists = existsList[caseDex];

                /* Initialize Output Parameters */
                EmParameters.MatsSorbentTrapPairedTrapAgreementValid = null;
                EmParameters.MatsCalcTrapAbsoluteDifference = null;
                EmParameters.MatsCalcTrapPercentDifference = null;


                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Initialize variables needed to run the check. */
                bool log = false;
                string actual;

                /* Run Check */
                actual = cMatsSorbentTrapChecks.MatsTrp11(category, ref log);


                /* Check Results */
                Assert.AreEqual(string.Empty, actual, string.Format("actual[case {0}]", caseDex));
                Assert.AreEqual(false, log, string.Format("log [case {0}]", caseDex));
                Assert.AreEqual(expResultList[caseDex], category.CheckCatalogResult, string.Format("category.CheckCatalogResult [case {0}]", caseDex));

                Assert.AreEqual(expValidList[caseDex], EmParameters.MatsSorbentTrapPairedTrapAgreementValid, string.Format("expValidList [case {0}]", caseDex));
                Assert.AreEqual(expAbsDiffList[caseDex], EmParameters.MatsCalcTrapAbsoluteDifference, string.Format("MatsCalcTrapAbsoluteDifference [case {0}]", caseDex));
                Assert.AreEqual(expPctDiffList[caseDex], EmParameters.MatsCalcTrapPercentDifference, string.Format("MatsCalcTrapPercentDifference [case {0}]", caseDex));
            }
        }

        /// <summary>
        ///A test for MatsTrp-12
        ///</summary>()
        [TestMethod()]
        public void MatsTrp12()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            /* Initialize General Variables */
            string[] modcList = { null, "00", "01", "02", "32", "33", "34", "35", "43", "44" };
            int[] discrepancyList = { -1, 0, 0 };
            string[] hgConcnetrationTypeList = { "Null", "NotRounded", "Rounded", "Zero", "ZeroTrains", "1stZeroTrain", "2ndZeroTrain" };
            bool?[] validList = { null, false, true };

            /* Initialize variables needed to run the check. */
            bool log = false;
            string actual;

            /* Test Cases */
            foreach (string modcCd in modcList)
            {
                bool? pairedTrapAgreementValid = (modcCd == null ? (bool?)null : modcCd.InList("01,02,32,33,34,35,43,44"));

                /* Initialize MatsSorbentTrapSamplingTrainList */
                EmParameters.MatsSorbentTrapSamplingTrainList = new List<SamplingTrainEvalInformation>();
                {
                    EmParameters.MatsSorbentTrapSamplingTrainList.Add(new SamplingTrainEvalInformation("TrainHg0"));
                    EmParameters.MatsSorbentTrapSamplingTrainList[0].HgConcentration = 0.120m;
                    EmParameters.MatsSorbentTrapSamplingTrainList[0].TrainQAStatusCode = "FAILED";
                    EmParameters.MatsSorbentTrapSamplingTrainList.Add(new SamplingTrainEvalInformation("TrainHg1"));
                    EmParameters.MatsSorbentTrapSamplingTrainList[1].HgConcentration = 0.080m;
                    EmParameters.MatsSorbentTrapSamplingTrainList[1].TrainQAStatusCode = "FAILED";
                }

                decimal? seedHgSystemConcentration = null;
                {
                    if (modcCd.InList("01,02,43"))
                    {
                        seedHgSystemConcentration = 0.100m;
                        EmParameters.MatsSorbentTrapSamplingTrainList[0].TrainQAStatusCode = "PASSED";
                        EmParameters.MatsSorbentTrapSamplingTrainList[1].TrainQAStatusCode = "PASSED";
                    }
                    else if (modcCd.InList("32,44"))
                    {
                        seedHgSystemConcentration = 0.0889m;
                        EmParameters.MatsSorbentTrapSamplingTrainList[0].TrainQAStatusCode = "FAILED";
                        EmParameters.MatsSorbentTrapSamplingTrainList[1].TrainQAStatusCode = "PASSED";
                    }
                    else if (modcCd == "33")
                    {
                        seedHgSystemConcentration = 0.120m;
                        EmParameters.MatsSorbentTrapSamplingTrainList[0].TrainQAStatusCode = "PASSED";
                        EmParameters.MatsSorbentTrapSamplingTrainList[1].TrainQAStatusCode = "PASSED";
                    }
                }

                foreach (string hgConcnetrationType in hgConcnetrationTypeList)
                {
                    // Produce cases in which calculations occur
                    if (pairedTrapAgreementValid.Default(false) && modcCd.InList("01,02,32,33,43,44") && (hgConcnetrationType == "Rounded"))
                    {
                        foreach (int discrepancy in discrepancyList)
                        {
                            /* Initialize MatsSorbentTrapRecord */
                            EmParameters.MatsSorbentTrapRecord
                              = new MatsSorbentTrapRecord
                                (
                                  modcCd: modcCd,
                                  hgConcentration: (seedHgSystemConcentration.Value + 0.001m * discrepancy).DecimaltoScientificNotation(3)
                                );

                            /* Initialize General Input Parameters */
                            EmParameters.MatsSorbentTrapPairedTrapAgreementValid = pairedTrapAgreementValid;

                            /* Initialize Updatable Parameters*/
                            EmParameters.MatsSorbentTrapValidExists = null;

                            /* Initialize Output Parameters */
                            EmParameters.MatsCalcHgSystemConcentration = null;

                            /* Init Cateogry Result */
                            category.CheckCatalogResult = null;

                            /* Run Check */
                            actual = cMatsSorbentTrapChecks.MatsTrp12(category, ref log);

                            /* Expected Results */
                            string result = (discrepancy != 0) ? "D" : null;
                            bool? validExists = (discrepancy != 0) ? false : (bool?)null;
                            string expectedHgSystemConcentration = seedHgSystemConcentration.Value.DecimaltoScientificNotation(3);

                            /* Check Result Label */
                            string resultPrefix = string.Format("[modcCd: {0}, hgcType: {1}, delta: {2}]", modcCd, hgConcnetrationType, discrepancy);

                            /* Check Results */
                            Assert.AreEqual(string.Empty, actual);
                            Assert.AreEqual(false, log);

                            Assert.AreEqual(result, category.CheckCatalogResult, resultPrefix + ".Result");
                            Assert.AreEqual(validExists, EmParameters.MatsSorbentTrapValidExists, resultPrefix + ".MatsSorbentTrapValidExists");
                            Assert.AreEqual(expectedHgSystemConcentration, EmParameters.MatsCalcHgSystemConcentration, resultPrefix + ".MatsCalcHgSystemConcentration");
                        }
                    }

                    // Produce non calculation cases 
                    else
                    {
                        /* Initialize Variables */
                        string hgConcentration;
                        {
                            switch (hgConcnetrationType)
                            {
                                case "Rounded": hgConcentration = 0.100m.DecimaltoScientificNotation(3); break;
                                case "NotRounded": hgConcentration = hgConcentration = 0.1001m.DecimaltoScientificNotation(4); break;
                                case "Zero": hgConcentration = hgConcentration = 0m.DecimaltoScientificNotation(3); break;
                                case "ZeroTrains":
                                    hgConcentration = 0.100m.DecimaltoScientificNotation(3);
                                    EmParameters.MatsSorbentTrapSamplingTrainList[0].HgConcentration = 0m;
                                    EmParameters.MatsSorbentTrapSamplingTrainList[1].HgConcentration = 0m;
                                    break;
                                case "1stZeroTrain":
                                    hgConcentration = 0.100m.DecimaltoScientificNotation(3);
                                    EmParameters.MatsSorbentTrapSamplingTrainList[0].HgConcentration = 0m;
                                    EmParameters.MatsSorbentTrapSamplingTrainList[1].HgConcentration = 0.100m;
                                    break;
                                case "2ndZeroTrain":
                                    hgConcentration = 0.100m.DecimaltoScientificNotation(3);
                                    EmParameters.MatsSorbentTrapSamplingTrainList[0].HgConcentration = 0.100m;
                                    EmParameters.MatsSorbentTrapSamplingTrainList[1].HgConcentration = 0m;
                                    break;
                                default: hgConcentration = null; break;
                            }
                        }

                        /* Initialize MatsSorbentTrapRecord */
                        EmParameters.MatsSorbentTrapRecord = new MatsSorbentTrapRecord(modcCd: modcCd, hgConcentration: hgConcentration);

                        /* Initialize General Input Parameters */
                        EmParameters.MatsSorbentTrapPairedTrapAgreementValid = pairedTrapAgreementValid;

                        /* Initialize Updatable Parameters*/
                        EmParameters.MatsSorbentTrapValidExists = null;

                        /* Initialize Output Parameters */
                        EmParameters.MatsCalcHgSystemConcentration = null;

                        /* Init Cateogry Result */
                        category.CheckCatalogResult = null;

                        /* Run Check */
                        actual = cMatsSorbentTrapChecks.MatsTrp12(category, ref log);

                        /* Expected Results */
                        string result = null;
                        bool? validExists = null;
                        decimal? expectedHgSystemConcentration = null;

                        if (pairedTrapAgreementValid.Default(false))
                        {

                            if (hgConcnetrationType == "Null")
                            {
                                if (modcCd.NotInList("34,35"))
                                    result = "A";
                            }
                            else if (hgConcnetrationType == "Zero")
                            {
                                if (modcCd.NotInList("01,02,32,33,43,44"))
                                    result = "B";
                                else
                                    result = "E";
                            }
                            else if (hgConcnetrationType.InList("ZeroTrains,1stZeroTrain,2ndZeroTrain"))
                            {
                                if (modcCd.NotInList("01,02,32,33,43,44"))
                                    result = "B";
                                else
                                    result = "F";
                            }
                            else
                            {
                                if (modcCd.NotInList("01,02,32,33,43,44"))
                                    result = "B";
                                else // hgConcnetrationType == "NotRounded"
                                    result = "C";
                            }

                            if (result != null)
                                validExists = false;
                        }

                        /* Check Result Label */
                        string resultPrefix = string.Format("[modcCd: {0}, hgcType: {1}]", modcCd, hgConcnetrationType);

                        /* Check Results */
                        Assert.AreEqual(string.Empty, actual);
                        Assert.AreEqual(false, log);

                        Assert.AreEqual(result, category.CheckCatalogResult, resultPrefix + ".Result");
                        Assert.AreEqual(validExists, EmParameters.MatsSorbentTrapValidExists, resultPrefix + ".MatsSorbentTrapValidExists");
                        Assert.AreEqual(expectedHgSystemConcentration, EmParameters.MatsCalcHgSystemConcentration, resultPrefix + ".MatsCalcHgSystemConcentration");
                    }
                }
            }
        }

        /// <summary>
        ///A test for MatsTrp-13
        ///</summary>()
        [TestMethod()]
        public void MatsTrp13()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            /* Initialize General Variables */
            bool[] booleanList = { false, true };

            /* Initialize variables needed to run the check. */
            bool log = false;
            string actual;

            /* Test Cases */
            foreach (bool validExists in booleanList)
                foreach (bool problemComponentExists in booleanList)
                {
                    /* Initialize MatsSorbentTrapDictionary */
                    EmParameters.MatsSorbentTrapDictionary = new Dictionary<string, SorbentTrapEvalInformation>();
                    EmParameters.MatsSorbentTrapDictionary["TrapId1"] = new SorbentTrapEvalInformation();
                    EmParameters.MatsSorbentTrapDictionary["TrapId1"].SorbentTrapValidExists = null;
                    EmParameters.MatsSorbentTrapDictionary["TrapId1"].SamplingTrainProblemComponentExists = null;
                    EmParameters.MatsSorbentTrapDictionary["TrapId2"] = new SorbentTrapEvalInformation();
                    EmParameters.MatsSorbentTrapDictionary["TrapId2"].SorbentTrapValidExists = null;
                    EmParameters.MatsSorbentTrapDictionary["TrapId2"].SamplingTrainProblemComponentExists = null;
                    EmParameters.MatsSorbentTrapDictionary["TrapId3"] = new SorbentTrapEvalInformation();
                    EmParameters.MatsSorbentTrapDictionary["TrapId3"].SorbentTrapValidExists = null;
                    EmParameters.MatsSorbentTrapDictionary["TrapId3"].SamplingTrainProblemComponentExists = null;

                    /* Initialize General Input Parameters */
                    EmParameters.MatsSorbentTrapRecord = new MatsSorbentTrapRecord(trapId: "TrapId2");
                    EmParameters.MatsSorbentTrapValidExists = validExists;
                    EmParameters.MatsSamplingTrainProblemComponentExists = problemComponentExists;

                    /* Init Cateogry Result */
                    category.CheckCatalogResult = null;

                    /* Run Check */
                    actual = cMatsSorbentTrapChecks.MatsTrp13(category, ref log);

                    /* Check Result Label */
                    string resultPrefix = string.Format("[valid: {0}, problemComponent: {1}]", validExists, problemComponentExists);

                    /* Check Results */
                    Assert.AreEqual(string.Empty, actual);
                    Assert.AreEqual(false, log);

                    Assert.AreEqual(null, category.CheckCatalogResult, resultPrefix + ".Result");
                    Assert.AreEqual(null, EmParameters.MatsSorbentTrapDictionary["TrapId1"].SorbentTrapValidExists, resultPrefix + ".LocationId1.SorbentTrapValidExists");
                    Assert.AreEqual(null, EmParameters.MatsSorbentTrapDictionary["TrapId1"].SamplingTrainProblemComponentExists, resultPrefix + ".LocationId1.SamplingTrainProblemComponentExists");
                    Assert.AreEqual(validExists, EmParameters.MatsSorbentTrapDictionary["TrapId2"].SorbentTrapValidExists, resultPrefix + ".LocationId1.SorbentTrapValidExists");
                    Assert.AreEqual(problemComponentExists, EmParameters.MatsSorbentTrapDictionary["TrapId2"].SamplingTrainProblemComponentExists, resultPrefix + ".LocationId1.SamplingTrainProblemComponentExists");
                    Assert.AreEqual(null, EmParameters.MatsSorbentTrapDictionary["TrapId3"].SorbentTrapValidExists, resultPrefix + ".LocationId1.SorbentTrapValidExists");
                    Assert.AreEqual(null, EmParameters.MatsSorbentTrapDictionary["TrapId3"].SamplingTrainProblemComponentExists, resultPrefix + ".LocationId1.SamplingTrainProblemComponentExists");
                }
        }

        /// <summary>
        /// MatsTrp-14
        /// 
        /// MatsSorbentTrapRecord.TrapId equals "GOOD"
        /// 
        /// | ## | DictId | MODC | Days || Result || Note
        /// |  0 | BAD    | 01   |   16 || null   || No result because of trap id.
        /// |  1 | GOOD   | 01   |   16 || A      || Result a because of MODC and number of days.
        /// |  2 | GOOD   | 04   |   16 || A      || Result a because of MODC and number of days.
        /// |  3 | GOOD   | 14   |   16 || A      || Result a because of MODC and number of days.
        /// |  4 | GOOD   | 24   |   16 || A      || Result a because of MODC and number of days.
        /// |  5 | GOOD   | 33   |   16 || A      || Result a because of MODC and number of days.
        /// |  6 | GOOD   | 34   |   16 || null   || No result because of MODC 34.
        /// |  7 | GOOD   | 35   |   16 || A      || Result a because of MODC and number of days.
        /// |  8 | GOOD   | 54   |   16 || A      || Result a because of MODC and number of days.
        /// |  9 | GOOD   | 01   |   15 || null   || No result because of number of days.
        /// | 10 | GOOD   | 04   |   15 || null   || No result because of number of days.
        /// | 11 | GOOD   | 14   |   15 || null   || No result because of number of days.
        /// | 12 | GOOD   | 24   |   15 || null   || No result because of number of days.
        /// | 13 | GOOD   | 33   |   15 || null   || No result because of number of days.
        /// | 14 | GOOD   | 34   |   15 || null   || No result because of MODC 34 and number of days.
        /// | 15 | GOOD   | 35   |   15 || null   || No result because of number of days.
        /// | 16 | GOOD   | 54   |   15 || null   || No result because of number of days.
        /// </summary>()
        [TestMethod()]
        public void MatsTrp14()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;


            /* Test Case Count */
            int caseCount = 17;

            /* Input Parameter Values */
            string goodTrapId = "GOOD";
            string badTrapId = "BAD";

            string[] dictIdList = { "BAD", "GOOD", "GOOD", "GOOD", "GOOD", "GOOD", "GOOD", "GOOD", "GOOD", "GOOD", "GOOD", "GOOD", "GOOD", "GOOD", "GOOD", "GOOD", "GOOD" };
            string[] modcList = { "01", "01", "04", "14", "24", "33", "34", "35", "54", "01", "04", "14", "24", "33", "34", "35", "54" };
            int[] daysList = { 16, 16, 16, 16, 16, 16, 16, 16, 16, 15, 15, 15, 15, 15, 15, 15, 15 };

            /* Expected Values */
            string[] resultList = { null, "A", "A", "A", "A", "A", null, "A", "A", null, null, null, null, null, null, null, null };

            /* Check array lengths */
            Assert.AreEqual(caseCount, dictIdList.Length, "dictIdList length");
            Assert.AreEqual(caseCount, modcList.Length, "modcList length");
            Assert.AreEqual(caseCount, daysList.Length, "daysList length");
            Assert.AreEqual(caseCount, resultList.Length, "resultList length");


            /* Run Cases */
            for (int caseDex = 0; caseDex < caseCount; caseDex++)
            {
                /* Initialize Input Parameters */
                EmParameters.MatsSorbentTrapRecord = new MatsSorbentTrapRecord(trapId: goodTrapId, modcCd: modcList[caseDex]);

                EmParameters.MatsSorbentTrapDictionary = new Dictionary<string, SorbentTrapEvalInformation>();

                EmParameters.MatsSorbentTrapDictionary[badTrapId] = new SorbentTrapEvalInformation();
                EmParameters.MatsSorbentTrapDictionary[goodTrapId] = new SorbentTrapEvalInformation();
                {
                    for (int day = 0; day < daysList[caseDex]; day++)
                        EmParameters.MatsSorbentTrapDictionary[dictIdList[caseDex]].OperatingDateList.Add(DateTime.Now.AddDays(-1 * day).Date);
                }

                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Initialize variables needed to run the check. */
                bool log = false;
                string actual;

                /* Run Check */
                actual = cMatsSorbentTrapChecks.MatsTrp14(category, ref log);

                /* Check Results */
                Assert.AreEqual(string.Empty, actual, string.Format("actual {0}", caseDex));
                Assert.AreEqual(false, log, string.Format("log {0}", caseDex));
                Assert.AreEqual(resultList[caseDex], category.CheckCatalogResult, string.Format("category.CheckCatalogResult {0}", caseDex));
            }
        }

        /// <summary>
        ///A test for MatsTrp-8
        ///</summary>()
        [TestMethod()]
        public void MatsTrp8_old()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            /* Initialize General Variables */
            string[] systemIdList = { null, "systemId" };
            string[] systemTypeList = { null, "CO2", "FLOW", "GAS", "H2O", "H2OM", "H2OT", "HCL", "HF", "HG", "LTGS", "LTOL", "NOX", "NOXC", "NOXE", "NOXP", "O2", "OILM", "OILV", "OP", "PM", "SO2", "ST" };

            /* Initialize variables needed to run the check. */
            bool log = false;
            string actual;

            /* Test Cases */
            foreach (string monSysId in systemIdList)
                foreach (string systemTypeCd in systemTypeList)
                {
                    /*  Initialize Input Parameters*/
                    EmParameters.MatsSorbentTrapRecord = new MatsSorbentTrapRecord(monSysId: monSysId, sysTypeCd: systemTypeCd);

                    /*  Initialize Updatable Parameters*/
                    EmParameters.MatsSorbentTrapValidExists = null;

                    /* Init Cateogry Result */
                    category.CheckCatalogResult = null;

                    /* Run Check */
                    actual = cMatsSorbentTrapChecks.MatsTrp8(category, ref log);

                    /* Check results */
                    Assert.AreEqual(string.Empty, actual);
                    Assert.AreEqual(false, log);

                    Assert.AreEqual((monSysId == null ? "A" : (systemTypeCd == "ST" ? null : "B")), category.CheckCatalogResult, "DateWithNeededTrue.Result");
                    Assert.AreEqual(((monSysId == null || systemTypeCd != "ST") ? false : (bool?)null), EmParameters.MatsSorbentTrapValidExists, "DateWithNeededTrue.MatsSorbentTrapValidExists");
                }
        }



        /// <summary>
        /// 
        /// MatsTrp15()
        /// 
        /// trapBeg: 2016-06-04 08
        /// trapEnd: 2016-06-17 22
        /// trapMid: 2016-06-10 15
        /// 
        /// | ## | meth1Cd | meth1Beg  | meth1End  | meth2Cd | meth2Beg  | meth2End  || result || Note
        /// |  0 | ST      | trapBeg   | trapMid   | CEMST   | trapMid+1 | null      || null   || A ST and a CEMST method cover the active period for the trap.
        /// |  1 | ST      | trapBeg   | trapMid   | CEMST   | trapMid+1 | trapEnd   || null   || A ST and a CEMST method cover the active period for the trap.
        /// |  2 | CEM     | trapBeg   | trapMid   | CEM     | trapMid+1 | trapEnd   || A      || The active period for the trap is not covered by a ST or CEMST method.
        /// |  3 | ST      | trapBeg   | trapMid-1 | CEMST   | trapMid+1 | trapEnd   || B      || A gap in method coverage exists in the middle of the active period for the trap.
        /// |  4 | ST      | trapBeg+1 | trapMid   | CEMST   | trapMid+1 | trapEnd   || B      || The beginning of the active periof for the trap is not covered by a method.
        /// |  5 | ST      | trapBeg   | trapMid   | CEMST   | trapMid+1 | trapEnd-1 || B      || The end of the active periof for the trap is not covered by a method.
        /// </summary>
        [TestMethod()]
        public void MatsTrp15()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;


            /* Input Parameter Values */
            DateTime trapBeg = new DateTime(2016, 6, 4, 8, 0, 0);
            DateTime trapEnd = new DateTime(2016, 6, 17, 22, 0, 0);
            DateTime trapMid = new DateTime(2016, 6, 10, 15, 0, 0);

            string[] meth1CdList = { "ST", "ST", "CEM", "ST", "ST", "ST" };
            DateTime?[] meth1BegList = { trapBeg, trapBeg, trapBeg, trapBeg, trapBeg.AddHours(1), trapBeg };
            DateTime?[] meth1EndList = { trapMid, trapMid, trapMid, trapMid.AddHours(-1), trapMid, trapMid };
            string[] meth2CdList = { "CEMST", "CEMST", "CEM", "CEMST", "CEMST", "CEMST" };
            DateTime?[] meth2BegList = { trapMid.AddHours(1), trapMid.AddHours(1), trapMid.AddHours(1), trapMid.AddHours(1), trapMid.AddHours(1), trapMid.AddHours(1) };
            DateTime?[] meth2EndList = { null, trapEnd, trapEnd, trapEnd, trapEnd, trapEnd.AddHours(-1) };

            /* Expected Values */
            string[] resultList = { null, null, "A", "B", "B", "B" };

            /* Case Count */
            int caseCount = 6;

            /* Check array lengths */
            Assert.AreEqual(caseCount, meth1CdList.Length, "meth1 CdList length");
            Assert.AreEqual(caseCount, meth1BegList.Length, "meth1 BegList length");
            Assert.AreEqual(caseCount, meth1EndList.Length, "meth1 EndList length");
            Assert.AreEqual(caseCount, meth2CdList.Length, "meth2 CdList length");
            Assert.AreEqual(caseCount, meth2BegList.Length, "meth2 BegList length");
            Assert.AreEqual(caseCount, meth2EndList.Length, "meth2 EndList length");
            Assert.AreEqual(caseCount, resultList.Length, "resultList length");

            for (int caseDex = 0; caseDex < caseCount; caseDex++)
            {
                /* Initialize Input Parameters */
                EmParameters.MatsSorbentTrapRecord = SetValues.DateHour(new MatsSorbentTrapRecord(monSysId: "TestId"), trapBeg, trapEnd);
                EmParameters.MethodRecords = new CheckDataView<VwMonitorMethodRow>();
                {
                    EmParameters.MethodRecords.Add(SetValues.DateHour(new VwMonitorMethodRow(methodCd: meth1CdList[caseDex]), meth1BegList[caseDex], meth1EndList[caseDex]));
                    EmParameters.MethodRecords.Add(SetValues.DateHour(new VwMonitorMethodRow(methodCd: meth2CdList[caseDex]), meth2BegList[caseDex], meth2EndList[caseDex]));
                }

                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Initialize variables needed to run the check. */
                bool log = false;
                string actual;

                /* Run Check */
                actual = cMatsSorbentTrapChecks.MatsTrp15(category, ref log);

                /* Check results */
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);

                Assert.AreEqual(resultList[caseDex], category.CheckCatalogResult, string.Format("Result {0}", caseDex));
            }


        }

        #endregion

    }
}