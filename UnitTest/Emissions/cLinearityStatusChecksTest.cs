using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.EmissionsChecks;
using ECMPS.Checks.EmissionsReport;
using ECMPS.Definitions.Extensions;

using ECMPS.Checks.Data.Ecmps.Dbo.Table;
using ECMPS.Checks.Data.Ecmps.Dbo.View;
using ECMPS.Checks.Data.EcmpsWs.Dbo.View;
using ECMPS.Checks.Data.EcmpsAux.CrossCheck.Virtual;
using ECMPS.Checks.Data.Ecmps.CrossCheck.Table;
using ECMPS.Checks.Em.Parameters;

using UnitTest.UtilityClasses;

namespace UnitTest.Emissions
{
    [TestClass()]
    public class cLinearityStatusChecksTest
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

        #region LinStat4

        /// <summary>
        ///A test for LINSTAT4_FixFilterError
        ///</summary>()
        [TestMethod()]
        public void LINSTAT4_FixFilterError()
        {
            //can't fully test this check until Arrays added to parameters, but verified that filter problem has been resolved

            //static check setup
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            // Variables
            bool log = false;
            string actual;

            // Init Input
            EmParameters.ApplicableSystemIds = null;
            EmParameters.CurrentAnalyzerRangeUsed = "";
            EmParameters.CurrentLinearityStatus = "";
            EmParameters.DualRangeStatus = false;
            EmParameters.EarliestLocationReportDate = DateTime.Today;
            EmParameters.HighRangeComponentId = "";
            EmParameters.LocationReportingFrequencyRecords = new CheckDataView<VwLocationReportingFrequencyRow>();
            EmParameters.LowRangeComponentId = "";
            EmParameters.PriorLinearityEventRecord = new VwQaCertEventRow();
            EmParameters.OperatingSuppDataRecordsByLocation = new CheckDataView<VwMpOpSuppDataRow>();
            EmParameters.QaCertificationEventRecords = new CheckDataView<VwQaCertEventRow>();

            EmParameters.PriorLinearityRecord = new VwQaSuppDataHourlyStatusRow(qaNeedsEvalFlg: "N", testResultCd: "PASSED", testExpDate: DateTime.Today.AddMonths(-36),
                testExpDateWithExt: null);
            EmParameters.CurrentMhvRecord = new VwMpMonitorHrlyValueRow(beginDate: DateTime.Today, beginHour: 0);
            EmParameters.AnnualReportingRequirement = true;
            EmParameters.ApplicableComponentId = "COMPONENTGOOD";
            EmParameters.TestExtensionExemptionRecords = new CheckDataView<VwQaTestExtensionExemptionRow>(
              new VwQaTestExtensionExemptionRow(componentId: "COMPONENTGOOD", extensExemptCd: "NONQAPB", calendarYear: 2014, quarter: 1));

            // Init Output
            category.CheckCatalogResult = null;

            // Run Checks
            actual = cLinearityStatusChecks.LINSTAT4(category, ref log);

            // Check Results
            Assert.AreEqual(string.Empty, actual);
            Assert.AreEqual(false, log);
        }

        #endregion

        /// <summary>
        ///LinStat-7
        ///
        ///                       |- Prior Linearity -|-    Linearity Test Records By Location     -|-    QA Cert Event  Records    -||
        /// | ## |     Status     | CompType | CompId | CompId |  Type  | Result  |   EndDateHour   | CompId | Code |  QceDateHour   || Status                           | CheckRan || Notes
        /// |  0 | IC             | HG       | GOOD   |                                             |                                || OOC-No Prior 3-Point SI or Event | true     || No HGSI3 for target IC and component type. 
        /// |  1 | IC-Extension   | HG       | GOOD   |                                             |                                || OOC-No Prior 3-Point SI or Event | true     || No HGSI3 for target IC and component type. 
        /// |  2 | IC-Grace       | HG       | GOOD   |                                             |                                || OOC-No Prior 3-Point SI or Event | true     || No HGSI3 for target IC and component type. 
        /// |  3 | IC-Conditional | HG       | GOOD   |                                             |                                || IC-Conditional                   | false    || No HGSI3 but result not reset because of non-target IC of IC-Conditional.
        /// |  4 | IC-Exempt      | HG       | GOOD   |                                             |                                || IC-Exempt                        | false    || No HGSI3 but result not reset because of non-target IC of IC-Exempt.
        /// |  5 | OOC-           | HG       | GOOD   |                                             |                                || OOC-                             | false    || No HGSI3 but result not reset because already OOC. 
        /// |  6 | IC             | ST       | GOOD   |                                             |                                || IC                               | false    || No HGSI3 but result not reset because system type is not for Hg CEMS. 
        /// |  7 | IC             | HG       | GOOD   | BAD    | HGSI3  | PASSED  | [current] - 1h  |                                || OOC-No Prior 3-Point SI or Event | true     || Existing HGSI3 test for a different component.
        /// |  8 | IC             | HG       | GOOD   | GOOD   | HGLINE | PASSED  | [current] - 1h  |                                || OOC-No Prior 3-Point SI or Event | true     || Existing test is HGLINE not HGSI3.
        /// |  9 | IC             | HG       | GOOD   | GOOD   | HGSI3  | FAILED  | [current] - 1h  |                                || OOC-No Prior 3-Point SI or Event | true     || Existing HGSI3 test failed.
        /// | 10 | IC             | HG       | GOOD   | GOOD   | HGSI3  | PASSED  | [current]       |                                || OOC-No Prior 3-Point SI or Event | true     || Existing HGSI3 test ended during the current hour.
        /// | 11 | IC             | HG       | GOOD   | GOOD   | HGSI3  | PASSED  | [current] - 1h  |                                || IC                               | true     || Existing HGSI3 test matches so result was not reset.
        /// | 12 | IC             | HG       | GOOD   | GOOD   | HGSI3  | PASSAPS | [current] - 1h  |                                || IC                               | true     || Existing HGSI3 test matches so result was not reset.
        /// | 13 | IC-Extension   | HG       | GOOD   | GOOD   | HGSI3  | PASSED  | [current] - 1h  |                                || IC-Extension                     | true     || Existing HGSI3 test matches so result was not reset.
        /// | 14 | IC-Grace       | HG       | GOOD   | GOOD   | HGSI3  | PASSED  | [current] - 1h  |                                || IC-Grace                         | true     || Existing HGSI3 test matches so result was not reset.
        /// | 15 | IC             | HG       | GOOD   | GOOD   | HGSI3  | PASSED  | [current] - 2h  | GOOD   | 120  | [current] - 1h || OOC-No Prior 3-Point SI or Event | true     || Existing event between matching test and current hour.
        /// | 16 | IC             | HG       | GOOD   | GOOD   | HGSI3  | PASSED  | [current] - 2h  | GOOD   | 125  | [current] - 1h || OOC-No Prior 3-Point SI or Event | true     || Existing event between matching test and current hour.
        /// | 17 | IC-Extension   | HG       | GOOD   | GOOD   | HGSI3  | PASSED  | [current] - 2h  | GOOD   | 120  | [current] - 1h || OOC-No Prior 3-Point SI or Event | true     || Existing event between matching test and current hour.
        /// | 18 | IC-Grace       | HG       | GOOD   | GOOD   | HGSI3  | PASSED  | [current] - 2h  | GOOD   | 120  | [current] - 1h || OOC-No Prior 3-Point SI or Event | true     || Existing event between matching test and current hour.
        /// | 19 | IC-Conditional | HG       | GOOD   | GOOD   | HGSI3  | PASSED  | [current] - 2h  | GOOD   | 120  | [current] - 1h || IC-Conditional                   | false    || Existing event but result not reset because of non-target IC of IC-Conditional.
        /// | 20 | IC-Exempt      | HG       | GOOD   | GOOD   | HGSI3  | PASSED  | [current] - 2h  | GOOD   | 120  | [current] - 1h || IC-Exempt                        | false    || Existing event but result not reset because of non-target IC of IC-Exempt.
        /// | 21 | OOC-           | HG       | GOOD   | GOOD   | HGSI3  | PASSED  | [current] - 2h  | GOOD   | 120  | [current] - 1h || OOC-                             | false    || Existing event but result not reset because already OOC. 
        /// | 22 | IC             | ST       | GOOD   | GOOD   | HGSI3  | PASSED  | [current] - 2h  | GOOD   | 120  | [current] - 1h || IC                               | false    || Existing event but result not reset because system type is not for Hg CEMS. 
        /// | 23 | IC             | HG       | GOOD   | GOOD   | HGSI3  | PASSED  | [current] - 2h  | BAD    | 120  | [current] - 1h || IC                               | true     || Existing event for a different component.
        /// | 24 | IC             | HG       | GOOD   | GOOD   | HGSI3  | PASSED  | [current] - 2h  | GOOD   | 123  | [current] - 1h || IC                               | true     || Existing event for a non cert/recert event code.
        /// | 25 | IC             | HG       | GOOD   | GOOD   | HGSI3  | PASSED  | [current] - 2h  | GOOD   | 120  | [current] - 2h || IC                               | true     || Existing event occured during end hour of prior test.
        /// | 26 | IC             | HG       | GOOD   | GOOD   | HGSI3  | PASSED  | [current] - 2h  | GOOD   | 120  | [current]      || IC                               | true     || Existing event occurred during the current hour.
        /// 
        ///</summary>()
        [TestMethod()]
        public void LinStat7()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            /* Case Count */
            int caseCount = 27;

            /* Input Parameter Values */
            string componentId = "GOOD";
            DateTime currentHour = new DateTime(2013, 6, 17, 22, 0, 0);
            DateTime currentHourMinus1 = currentHour.AddHours(-1);
            DateTime currentHourMinus2 = currentHour.AddHours(-2);
            DateTime currentTime = currentHour.AddMinutes(13);
            DateTime currentTimeMinus1 = currentHourMinus1.AddMinutes(26);
            DateTime currentTimeMinus2 = currentHourMinus2.AddMinutes(39);

            string[] statusList = { "IC", "IC-Extension", "IC-Grace", "IC-Conditional", "IC-Exempt", "OOC-", "IC", "IC", "IC", "IC", "IC", "IC", "IC", "IC-Extension", "IC-Grace", "IC", "IC", "IC-Extension", "IC-Grace", "IC-Conditional", "IC-Exempt", "OOC-", "IC", "IC", "IC", "IC", "IC" };
            string[] componentTypeList = { "HG", "HG", "HG", "HG", "HG", "HG", "ST", "HG", "HG", "HG", "HG", "HG", "HG", "HG", "HG", "HG", "HG", "HG", "HG", "HG", "HG", "HG", "ST", "HG", "HG", "HG", "HG" };
            string[] linearityComponentList = { null, null, null, null, null, null, null, "BAD", "GOOD", "GOOD", "GOOD", "GOOD", "GOOD", "GOOD", "GOOD", "GOOD", "GOOD", "GOOD", "GOOD", "GOOD", "GOOD", "GOOD", "GOOD", "GOOD", "GOOD", "GOOD", "GOOD" };
            string[] linearityTypeList = { null, null, null, null, null, null, null, "HGSI3", "HGLINE", "HGSI3", "HGSI3", "HGSI3", "HGSI3", "HGSI3", "HGSI3", "HGSI3", "HGSI3", "HGSI3", "HGSI3", "HGSI3", "HGSI3", "HGSI3", "HGSI3", "HGSI3", "HGSI3", "HGSI3", "HGSI3" };
            string[] linearityResultList = { null, null, null, null, null, null, null, "PASSED", "PASSED", "FAILED", "PASSED", "PASSED", "PASSAPS", "PASSED", "PASSED", "PASSED", "PASSED", "PASSED", "PASSED", "PASSED", "PASSED", "PASSED", "PASSED", "PASSED", "PASSED", "PASSED", "PASSED" };
            DateTime?[] linearityTimeList = { null, null, null, null, null, null, null, currentTimeMinus1, currentTimeMinus1, currentTimeMinus1, currentTime, currentTimeMinus1, currentTimeMinus1, currentTimeMinus1, currentTimeMinus1, currentTimeMinus2, currentTimeMinus2, currentTimeMinus2, currentTimeMinus2, currentTimeMinus2, currentTimeMinus2, currentTimeMinus2, currentTimeMinus2, currentTimeMinus2, currentTimeMinus2, currentTimeMinus2, currentTimeMinus2 };
            string[] qceComponentList = { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, "GOOD", "GOOD", "GOOD", "GOOD", "GOOD", "GOOD", "GOOD", "GOOD", "BAD", "GOOD", "GOOD", "GOOD" };
            string[] qceCodeList = { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, "120", "125", "120", "120", "120", "120", "120", "120", "120", "123", "120", "120" };
            DateTime?[] qceHourList = { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, currentHourMinus1, currentHourMinus1, currentHourMinus1, currentHourMinus1, currentHourMinus1, currentHourMinus1, currentHourMinus1, currentHourMinus1, currentHourMinus1, currentHourMinus1, currentHourMinus2, currentHour };

            /* Expected Values */
            string[] resultList = { "OOC-No Prior 3-Point SI or Event", "OOC-No Prior 3-Point SI or Event", "OOC-No Prior 3-Point SI or Event", "IC-Conditional", "IC-Exempt", "OOC-", "IC", "OOC-No Prior 3-Point SI or Event", "OOC-No Prior 3-Point SI or Event", "OOC-No Prior 3-Point SI or Event", "OOC-No Prior 3-Point SI or Event", "IC", "IC", "IC-Extension", "IC-Grace", "OOC-No Prior 3-Point SI or Event", "OOC-No Prior 3-Point SI or Event", "OOC-No Prior 3-Point SI or Event", "OOC-No Prior 3-Point SI or Event", "IC-Conditional", "IC-Exempt", "OOC-", "IC", "IC", "IC", "IC", "IC" };
            bool[] checkRan = { true, true, true, false, false, false, false, true, true, true, true, true, true, true, true, true, true, true, true, false, false, false, false, true, true, true, true };

            /* Check array lengths */
            Assert.AreEqual(caseCount, statusList.Length, "statusList length");
            Assert.AreEqual(caseCount, componentTypeList.Length, "componentTypeList length");
            Assert.AreEqual(caseCount, linearityComponentList.Length, "linearityComponentList length");
            Assert.AreEqual(caseCount, linearityTypeList.Length, "linearityTypeList length");
            Assert.AreEqual(caseCount, linearityResultList.Length, "linearityResultList length");
            Assert.AreEqual(caseCount, linearityTimeList.Length, "linearityTimeList length");
            Assert.AreEqual(caseCount, qceComponentList.Length, "qceComponentList length");
            Assert.AreEqual(caseCount, qceCodeList.Length, "qceCodeList length");
            Assert.AreEqual(caseCount, qceHourList.Length, "qceHourList length");
            Assert.AreEqual(caseCount, resultList.Length, "resultList length");
            Assert.AreEqual(caseCount, checkRan.Length, "checkRan length");

            for (int caseDex = 0; caseDex < caseCount; caseDex++)
            {
                /* Initialize Input Parameters */
                EmParameters.CurrentDateHour = currentHour;
                EmParameters.PriorLinearityRecord = new VwQaSuppDataHourlyStatusRow(componentId: componentId, componentTypeCd: componentTypeList[caseDex]);

                EmParameters.LinearityTestRecordsByLocationForQaStatus = new CheckDataView<VwQaSuppDataHourlyStatusRow>();
                {
                    if (linearityComponentList[caseDex] != null)
                        EmParameters.LinearityTestRecordsByLocationForQaStatus.Add
                            (
                                new VwQaSuppDataHourlyStatusRow(componentId: "OTHER1", testTypeCd: "HGSI3", testResultCd: "PASSED",
                                                                endDate: currentTimeMinus1.Date, endHour: currentTimeMinus1.Hour, endMin: currentTimeMinus1.Minute,
                                                                endDatehour: currentTimeMinus1.Date.AddHours(currentTimeMinus1.Hour)),
                                new VwQaSuppDataHourlyStatusRow(componentId: linearityComponentList[caseDex], testTypeCd: linearityTypeList[caseDex], testResultCd: linearityResultList[caseDex],
                                                                endDate: linearityTimeList[caseDex].Value.Date, endHour: linearityTimeList[caseDex].Value.Hour, endMin: linearityTimeList[caseDex].Value.Minute,
                                                                endDatehour: linearityTimeList[caseDex].Value.Date.AddHours(linearityTimeList[caseDex].Value.Hour)),
                                new VwQaSuppDataHourlyStatusRow(componentId: "OTHER2", testTypeCd: "HGSI3", testResultCd: "PASSAPS",
                                                                endDate: currentTimeMinus1.Date, endHour: currentTimeMinus1.Hour, endMin: currentTimeMinus1.Minute,
                                                                endDatehour: currentTimeMinus1.Date.AddHours(currentTimeMinus1.Hour))
                            );
                }

                EmParameters.QaCertificationEventRecords = new CheckDataView<VwQaCertEventRow>();
                {
                    if (qceComponentList[caseDex] != null)
                        EmParameters.QaCertificationEventRecords.Add
                        (
                            new VwQaCertEventRow(componentId: "OTHER1", qaCertEventCd: "125",
                                                 qaCertEventDate: currentHourMinus1.Date, qaCertEventHour: currentHourMinus1.Hour, qaCertEventDatehour: currentHourMinus1),
                            new VwQaCertEventRow(componentId: qceComponentList[caseDex], qaCertEventCd: qceCodeList[caseDex],
                                                 qaCertEventDate: qceHourList[caseDex].Value.Date, qaCertEventHour: qceHourList[caseDex].Value.Hour, qaCertEventDatehour: qceHourList[caseDex].Value),
                            new VwQaCertEventRow(componentId: "OTHER2", qaCertEventCd: "120",
                                                 qaCertEventDate: currentHourMinus1.Date, qaCertEventHour: currentHourMinus1.Hour, qaCertEventDatehour: currentHourMinus1)
                        );
                }

                /* Initialize Input/Output Parameters */
                EmParameters.CurrentLinearityStatus = statusList[caseDex];

                /* Initialize Output Parameters */
                EmParameters.MatsCheckForHgsi3Ran = null;

                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Initialize variables needed to run the check. */
                bool log = false;
                string actual;

                if (caseDex == 23)
                {
                    // Run Checks
                    actual = cLinearityStatusChecks.LINSTAT7(category, ref log);

                    // Check Results
                    Assert.AreEqual(string.Empty, actual);
                    Assert.AreEqual(false, log);
                    Assert.AreEqual(resultList[caseDex], EmParameters.CurrentLinearityStatus, string.Format("Result {0}", caseDex));
                    Assert.AreEqual(checkRan[caseDex], EmParameters.MatsCheckForHgsi3Ran, string.Format("MatsCheckForHgsi3Ran {0}", caseDex));
                }
            }
        }

    }
}
