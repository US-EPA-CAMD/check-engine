using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

using UnitTest.UtilityClasses;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.EmissionsChecks;
using ECMPS.Definitions.Extensions;

using ECMPS.Checks.Data.Ecmps.CheckEm.Function;
using ECMPS.Checks.Data.Ecmps.Dbo.View;
using ECMPS.Checks.Em.Parameters;

namespace UnitTest.Emissions
{
    [TestClass()]
    public class cMATSMonitorHourlyValueChecksTest
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

        #region Checks 1-10

        /// <summary>
        /// MatsMhv-1
        /// 
        /// Curr: 2016-06-17 22
        ///
        /// | ## | MethodCd | SysId | SysType | St1Sys | St1Begin | St1End | St2Sys || Parameter | RecordSystemType | CurrentSystemType | ComponentType | Measured          | Unavailable | StCount || Note
        /// |  0 | CEM      | GOOD  | HG      | GOOD   | Curr     | Curr   | OTHER  || HGC       | HG               | HG                | HG            | 01,02,17,21       | 34,35       | null    || HG CEMS Method
        /// |  1 | CEM      | GOOD  | ST      | GOOD   | Curr     | Curr   | OTHER  || HGC       | ST               | HG                | HG            | 01,02,17,21       | 34,35       | null    || HG CEMS Method, Bad System
        /// |  2 | ST       | GOOD  | ST      | GOOD   | Curr     | Curr   | OTHER  || HGC       | ST               | ST                | STRAIN        | 01,02,32,33,41,42 | 34,35       | 1       || HG CEMS Method
        /// |  3 | ST       | GOOD  | HG      | GOOD   | Curr     | Curr   | OTHER  || HGC       | HG               | ST                | STRAIN        | 01,02,32,33,41,42 | 34,35       | 1       || HG CEMS Method, Bad System
        /// |  4 | CEMST    | GOOD  | HG      | GOOD   | Curr     | Curr   | OTHER  || HGC       | HG               | HG                | HG            | 01,02,17,21       | 34,35       | null    || HG CEMS Method
        /// |  5 | CEMST    | GOOD  | ST      | GOOD   | Curr     | Curr   | OTHER  || HGC       | ST               | ST                | STRAIN        | 01,02,32,33,41,42 | 34,35       | 1       || HG ST Method with one active sorbent trap
        /// |  6 | ST       | GOOD  | ST      | GOOD   | Curr     | Curr   | GOOD   || HGC       | ST               | ST                | STRAIN        | 01,02,32,33,41,42 | 34,35       | 2       || HG ST Method with two active sorbent traps
        /// |  7 | ST       | GOOD  | ST      | OTHER1 | Curr     | Curr   | OTHER2 || HGC       | ST               | ST                | STRAIN        | 01,02,32,33,41,42 | 34,35       | 0       || HG ST Method, but no sorbent trap with matching system
        /// |  8 | ST       | GOOD  | ST      | GOOD   | Curr-2   | Curr+2 | OTHER  || HGC       | ST               | ST                | STRAIN        | 01,02,32,33,41,42 | 34,35       | 1       || HG ST Method and active sorbent trap exists
        /// |  9 | ST       | GOOD  | ST      | GOOD   | Curr-2   | Curr-1 | OTHER  || HGC       | ST               | ST                | STRAIN        | 01,02,32,33,41,42 | 34,35       | 0       || HG ST Method, but sorbent trap before current hour
        /// | 10 | ST       | GOOD  | ST      | GOOD   | Curr+1   | Curr+2 | OTHER  || HGC       | ST               | ST                | STRAIN        | 01,02,32,33,41,42 | 34,35       | 0       || HG ST Method, but sorbent trap after current hour
        ///</summary>()
        [TestMethod()]
        public void MATSMHV1()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            /* Input Parameter Values */
            DateTime curr = new DateTime(2016, 6, 17, 22, 0, 0);

            string[] inputMethodCdList = { "CEM", "CEM", "ST", "ST", "CEMST", "CEMST", "ST", "ST", "ST", "ST", "ST" };
            string[] inputSystemIdList = { "GOOD", "GOOD", "GOOD", "GOOD", "GOOD", "GOOD", "GOOD", "GOOD", "GOOD", "GOOD", "GOOD" };
            string[] inputSystemTypeCdList = { "HG", "ST", "ST", "HG", "HG", "ST", "ST", "ST", "ST", "ST", "ST" };
            string[] inputSt1SystemIdList = { "GOOD", "GOOD", "GOOD", "GOOD", "GOOD", "GOOD", "GOOD", "OTHER1", "GOOD", "GOOD", "GOOD" };
            DateTime?[] inputSt1BeginList = { curr, curr, curr, curr, curr, curr, curr, curr, curr.AddHours(-2), curr.AddHours(-2), curr.AddHours(1) };
            DateTime?[] inputSt1EndList = { curr, curr, curr, curr, curr, curr, curr, curr, curr.AddHours(2), curr.AddHours(-1), curr.AddHours(2) };
            string[] inputSt2SystemIdList = { "OTHER", "OTHER", "OTHER", "OTHER", "OTHER", "OTHER", "GOOD", "OTHER2", "OTHER", "OTHER", "OTHER" };

            /* Expected Values */
            string expectedParameterCd = "HGC";
            string[] expectedRecordSystemTypeCdList = { "HG", "ST", "ST", "HG", "HG", "ST", "ST", "ST", "ST", "ST", "ST" };
            string[] expectedCurrentSystemTypeCdList = { "HG", "HG", "ST", "ST", "HG", "ST", "ST", "ST", "ST", "ST", "ST" };
            string[] expectedCurrentComponentTypeCdList = { "HG", "HG", "STRAIN", "STRAIN", "HG", "STRAIN", "STRAIN", "STRAIN", "STRAIN", "STRAIN", "STRAIN" };
            string[] expectedMeasuredModcList = { "01,02,17,21", "01,02,17,21", "01,02,32,33,41,42,43,44", "01,02,32,33,41,42,43,44", "01,02,17,21", "01,02,32,33,41,42,43,44",
                                                  "01,02,32,33,41,42,43,44", "01,02,32,33,41,42,43,44", "01,02,32,33,41,42,43,44", "01,02,32,33,41,42,43,44", "01,02,32,33,41,42,43,44" };
            string expectedUnavailableModc = "34,35";
            int?[] expectedStCount = { null, null, 1, 1, null, 1, 2, 0, 1, 0, 0 };

            /* Case Count */
            int caseCount = 11;

            /* Check array lengths */
            Assert.AreEqual(caseCount, inputMethodCdList.Length, "inputMethodCdList length");
            Assert.AreEqual(caseCount, inputSystemIdList.Length, "inputSystemIdList length");
            Assert.AreEqual(caseCount, inputSystemTypeCdList.Length, "inputSystemTypeCdList length");
            Assert.AreEqual(caseCount, inputSt1SystemIdList.Length, "inputSt1SystemIdList length");
            Assert.AreEqual(caseCount, inputSt1BeginList.Length, "inputSt1BeginList length");
            Assert.AreEqual(caseCount, inputSt1EndList.Length, "inputSt1EndList length");
            Assert.AreEqual(caseCount, inputSt2SystemIdList.Length, "inputSt2SystemIdList length");
            Assert.AreEqual(caseCount, expectedRecordSystemTypeCdList.Length, "expectedRecordSystemTypeCdList length");
            Assert.AreEqual(caseCount, expectedCurrentSystemTypeCdList.Length, "expectedCurrentSystemTypeCdList length");
            Assert.AreEqual(caseCount, expectedCurrentComponentTypeCdList.Length, "expectedCurrentComponentTypeCdList length");
            Assert.AreEqual(caseCount, expectedMeasuredModcList.Length, "expectedMeasuredModcList length");
            Assert.AreEqual(caseCount, expectedStCount.Length, "expectedStCount length");

            for (int caseDex = 0; caseDex < caseCount; caseDex++)
            {
                /* Initialize Input Parameters */
                EmParameters.CurrentDateHour = curr;
                EmParameters.MatsHgMethodRecord = new VwMpMonitorMethodRow(methodCd: inputMethodCdList[caseDex]);
                EmParameters.MatsHgcMhvRecord = new MATSMonitorHourlyValueData(sysTypeCd: inputSystemTypeCdList[caseDex], monSysId: inputSystemIdList[caseDex]);
                EmParameters.MatsSorbentTrapRecords = new CheckDataView<MatsSorbentTrapRecord>();
                {
                    EmParameters.MatsSorbentTrapRecords.Add(SetValues.DateHour(new MatsSorbentTrapRecord(monSysId: inputSt1SystemIdList[caseDex]), inputSt1BeginList[caseDex], inputSt1EndList[caseDex]));
                    EmParameters.MatsSorbentTrapRecords.Add(SetValues.DateHour(new MatsSorbentTrapRecord(monSysId: inputSt2SystemIdList[caseDex]), curr, curr));
                }

                /* Initialize Output Parameters */
                EmParameters.CurrentMhvParameter = null;
                EmParameters.MatsMhvRecord = null;
                EmParameters.MatsMhvSorbentTraps = (expectedStCount[caseDex] == null) ? new CheckDataView<MatsSorbentTrapRecord>() : null;

                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Initialize variables needed to run the check. */
                bool log = false;
                string actual;

                /* Run Check */
                actual = cMATSMonitorHourlyValueChecks.MATSMHV1(category, ref log);

                /* Check results */
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);

                Assert.AreEqual(null, category.CheckCatalogResult, string.Format("Result {0}", caseDex));

                Assert.AreEqual(expectedCurrentComponentTypeCdList[caseDex], EmParameters.CurrentMhvComponentType, string.Format("CurrentMhvComponentType {0}", caseDex));
                Assert.AreEqual(expectedParameterCd, EmParameters.CurrentMhvParameter, string.Format("CurrentMhvParameter {0}", caseDex));
                Assert.AreEqual(expectedCurrentSystemTypeCdList[caseDex], EmParameters.CurrentMhvSystemType, string.Format("CurrentMhvSystemType {0}", caseDex));
                Assert.AreEqual(expectedMeasuredModcList[caseDex], EmParameters.MatsMhvMeasuredModcList, string.Format("MatsMhvMeasuredModcList {0}", caseDex));
                Assert.AreEqual(expectedRecordSystemTypeCdList[caseDex], EmParameters.MatsMhvRecord.SysTypeCd, string.Format("MatsMhvRecord {0}", caseDex));
                Assert.AreEqual(expectedUnavailableModc, EmParameters.MatsMhvUnavailableModcList, string.Format("MatsMhvUnavailableModcList {0}", caseDex));
                Assert.AreEqual(expectedStCount[caseDex], ((EmParameters.MatsMhvSorbentTraps == null) ? (int?)null : EmParameters.MatsMhvSorbentTraps.Count), string.Format("MatsMhvSorbentTraps.Count {0}", caseDex));
            }
        }

        #region MATSMHV-2

        /// <summary>
        ///A test for MATSMHV-2
        ///</summary>()
        [TestMethod()]
        public void MATSMHV2()
        {
            //static check setup
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            // Variables
            bool log = false;
            string actual;

            // Init Input
            EmParameters.MatsHclcMhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData
                (componentTypeCd: "COMPGOOD", sysTypeCd: "SYSGOOD");

            // Init Output
            category.CheckCatalogResult = null;
            EmParameters.CurrentMhvParameter = null;
            EmParameters.MatsMhvRecord = null;
            EmParameters.CurrentMhvComponentType = null;
            EmParameters.CurrentMhvSystemType = null;
            EmParameters.MatsMhvMeasuredModcList = null;
            EmParameters.MatsMhvUnavailableModcList = null;

            // Run Checks
            actual = cMATSMonitorHourlyValueChecks.MATSMHV2(category, ref log);

            // Check Results
            Assert.AreEqual(false, log);
            Assert.AreEqual(null, category.CheckCatalogResult, "Result");
            Assert.AreEqual("HCLC", EmParameters.CurrentMhvParameter, "CurrentMhvParameter");
            Assert.AreEqual("COMPGOOD", EmParameters.MatsMhvRecord.ComponentTypeCd, "MatsMhvRecord");
            Assert.AreEqual("HCL", EmParameters.CurrentMhvComponentType, "CurrentMhvComponentType");
            Assert.AreEqual("HCL", EmParameters.CurrentMhvSystemType, "CurrentMhvSystemType");
            Assert.AreEqual("01,02,17,21", EmParameters.MatsMhvMeasuredModcList, "MatsMhvMeasuredModcList");
            Assert.AreEqual("34,35", EmParameters.MatsMhvUnavailableModcList, "MatsMhvUnavailableModcList");
        }
        #endregion

        #region MATSMHV-3

        /// <summary>
        ///A test for MATSMHV-3
        ///</summary>()
        [TestMethod()]
        public void MATSMHV3()
        {
            //static check setup
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            // Variables
            bool log = false;
            string actual;

            // Init Input
            EmParameters.MatsHfcMhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData
                (componentTypeCd: "COMPGOOD", sysTypeCd: "SYSGOOD");

            // Init Output
            category.CheckCatalogResult = null;
            EmParameters.CurrentMhvParameter = null;
            EmParameters.MatsMhvRecord = null;
            EmParameters.CurrentMhvComponentType = null;
            EmParameters.CurrentMhvSystemType = null;
            EmParameters.MatsMhvMeasuredModcList = null;
            EmParameters.MatsMhvUnavailableModcList = null;

            // Run Checks
            actual = cMATSMonitorHourlyValueChecks.MATSMHV3(category, ref log);

            // Check Results
            Assert.AreEqual(false, log);
            Assert.AreEqual(null, category.CheckCatalogResult, "Result");
            Assert.AreEqual("HFC", EmParameters.CurrentMhvParameter, "CurrentMhvParameter");
            Assert.AreEqual("COMPGOOD", EmParameters.MatsMhvRecord.ComponentTypeCd, "MatsMhvRecord");
            Assert.AreEqual("HF", EmParameters.CurrentMhvComponentType, "CurrentMhvComponentType");
            Assert.AreEqual("HF", EmParameters.CurrentMhvSystemType, "CurrentMhvSystemType");
            Assert.AreEqual("01,02,17,21", EmParameters.MatsMhvMeasuredModcList, "MatsMhvMeasuredModcList");
            Assert.AreEqual("34,35", EmParameters.MatsMhvUnavailableModcList, "MatsMhvUnavailableModcList");
        }
        #endregion


        /// <summary>
        /// MatsMhv-4
        /// 
        /// Note: The underlying table for Sorbent Trap is sorted by location name, system identifier, begin date and begin hour.
        /// 
        /// 
        /// Measured MODC    : 01, 02, 41, 42  [Put in MatsMhvMeasuredModcList ]
        /// Unavailable MODC : 34, 35 [Put in MatsMhvUnavailableModcList]
        /// Illegal MODC     : 32, 33 [Usually measured, but not for this test]
        /// 
        /// Note that MODC 34, 35, 41 and 42 are specifically referenced in the check spec.
        /// 
        /// 
        /// CurrentDate : Curr
        /// 
        /// Trap0 Dates : curr - 3 to curr - 2 (filtered out)
        /// Trap1 Dates : curr to curr + 1     (sorts 3rd)
        /// Trap2 Dates : curr - 1 to curr     (sorts 1st)
        /// Trap3 Dates : curr to curr         (sorts 2nd)
        /// 
        /// 
        /// | ## | Modc | Value  | SysType | Sys0   | Modc0 | Value0 | Sys1   | Modc1 | Value1 | Sys2   | Modc2 | Value2 || Result | Status || Note
        /// |  0 | null | 1.23E0 | ST      | GoodId | 02    | 1.23E0 | GoodId | 02    | 1.23E0 | GoodId | 02    | 1.23E0 || A      | false  || Null MHV MODC, which produces result A.
        /// |  1 | 01   | 1.23E0 | HG      | GoodId | 02    | 1.23E0 | GoodId | 02    | 1.23E0 | GoodId | 02    | 1.23E0 || null   | true   || Valid MHV MODC, but system type is not ST so no sorbent trap checking. 
        /// |  2 | 02   | 1.23E0 | HG      | GoodId | 01    | 1.23E0 | GoodId | 01    | 1.23E0 | GoodId | 01    | 1.23E0 || null   | true   || Valid MHV MODC, but system type is not ST so no sorbent trap checking. 
        /// |  3 | 32   | 1.23E0 | ST      | GoodId | 02    | 1.23E0 | GoodId | 02    | 1.23E0 | GoodId | 02    | 1.23E0 || B      | false  || Invalid MHV MODC, which produces result B. 
        /// |  4 | 33   | 1.23E0 | ST      | GoodId | 02    | 1.23E0 | GoodId | 02    | 1.23E0 | GoodId | 02    | 1.23E0 || B      | false  || Invalid MHV MODC, which produces result B. 
        /// |  5 | 34   | 1.23E0 | HG      | GoodId | 02    | 1.23E0 | GoodId | 02    | 1.23E0 | GoodId | 02    | 1.23E0 || null   | true   || Valid MHV MODC, but system type is not ST so no sorbent trap checking. 
        /// |  6 | 35   | 1.23E0 | HG      | GoodId | 02    | 1.23E0 | GoodId | 02    | 1.23E0 | GoodId | 02    | 1.23E0 || null   | true   || Valid MHV MODC, but system type is not ST so no sorbent trap checking. 
        /// |  7 | 41   | 1.23E0 | ST      | GoodId | 02    | 1.23E0 | GoodId | 02    | 1.23E0 | GoodId | 02    | 1.23E0 || null   | true   || Valid MHV MODC, but the MODC is 41 so no sorbent trap checking. 
        /// |  8 | 42   | 1.23E0 | ST      | GoodId | 02    | 1.23E0 | GoodId | 02    | 1.23E0 | GoodId | 02    | 1.23E0 || null   | true   || Valid MHV MODC, but the MODC is 42 so no sorbent trap checking. 
        /// |  9 | 01   | 1.23E0 | ST      | GoodId | 01    | 1.23E0 | GoodId | 02    | 1.23E0 | GoodId | 02    | 1.23E0 || null   | true   || MODC of first trap matches the MHV MODC. 
        /// | 10 | 02   | 1.23E0 | ST      | GoodId | 02    | 1.23E0 | GoodId | 01    | 1.23E0 | GoodId | 01    | 1.23E0 || null   | true   || MODC of first trap matches the MHV MODC.
        /// | 11 | 34   | 1.23E0 | ST      | GoodId | 02    | 1.23E0 | GoodId | 34    | 1.23E0 | GoodId | 02    | 1.23E0 || null   | true   || Unavailable MODC of second trap matches the MHV MODC.
        /// | 12 | 35   | 1.23E0 | ST      | GoodId | 02    | 1.23E0 | GoodId | 35    | 1.23E0 | GoodId | 02    | 1.23E0 || null   | true   || Unavailable MODC of second trap matches the MHV MODC.
        /// | 13 | 01   | 1.23E0 | ST      | GoodId | 02    | 1.23E0 | GoodId | 01    | 1.23E0 | GoodId | 02    | 1.23E0 || null   | true   || Measured MODC of second trap matches the MHV MODC.
        /// | 14 | 02   | 1.23E0 | ST      | GoodId | 01    | 1.23E0 | GoodId | 02    | 1.23E0 | GoodId | 01    | 1.23E0 || null   | true   || Measured MODC of second trap matches the MHV MODC.
        /// | 15 | 34   | 1.23E0 | ST      | GoodId | 35    | 1.23E0 | GoodId | 01    | 1.23E0 | GoodId | 02    | 1.23E0 || D      | false  || No trap MODC match the unavailabe MHV MODC.
        /// | 16 | 35   | 1.23E0 | ST      | GoodId | 32    | 1.23E0 | GoodId | 33    | 1.23E0 | GoodId | 34    | 1.23E0 || D      | false  || No trap MODC match the unavailabe MHV MODC.
        /// | 17 | 01   | 1.23E0 | ST      | GoodId | 34    | 1.23E0 | GoodId | 01    | 1.23E0 | GoodId | 02    | 1.23E0 || null   | true   || Measured MODC of second trap matches the MHV MODC.
        /// | 18 | 01   | 1.23E0 | ST      | GoodId | 35    | 1.23E0 | GoodId | 01    | 1.23E0 | GoodId | 02    | 1.23E0 || null   | true   || Measured MODC of second trap matches the MHV MODC.
        /// | 19 | 01   | 1.23E0 | ST      | GoodId | 34    | 1.23E0 | GoodId | 02    | 1.23E0 | GoodId | 35    | 1.23E0 || D      | false  || No trap MODC match the measure MHV MODC.
        /// | 20 | 01   | 1.23E0 | ST      | GoodId | 35    | 1.23E0 | GoodId | 02    | 1.23E0 | GoodId | 34    | 1.23E0 || D      | false  || No trap MODC match the measure MHV MODC.
        /// | 21 | 01   | 1.23E0 | ST      | null   | null  | null   | GoodId | 02    | 1.23E0 | GoodId | 02    | 1.23E0 || D      | false  || MODC of first trap does not match the MHV MODC, because the of bad id on usual first trap. 
        /// | 22 | 01   | 1.23E0 | ST      | GoodId | 34    | 1.23E0 | null   | null  | null   | GoodId | 01    | 1.23E0 || null   | true   || Unavailable MODC of first trap does not match the MHV MODC, the measured MODC of the second trap is skiped because of bad id, so the matched measure MODC of the third trap is used. 
        /// | 23 | 01   | 1.23E0 | ST      | GoodId | 35    | 1.23E0 | null   | null  | null   | GoodId | 01    | 1.23E0 || null   | true   || Unavailable MODC of first trap does not match the MHV MODC, the measured MODC of the second trap is skiped because of bad id, so the matched measure MODC of the third trap is used. 
        /// | 24 | 34   | 1.23E0 | ST      | GoodId | 34    | 1.22E0 | GoodId | 01    | 1.23E0 | GoodId | 02    | 1.23E0 || D      | false  || No trap MODC and HgConcentration match the MHV.
        /// | 25 | 35   | 1.23E0 | ST      | GoodId | 35    | 1.24E0 | GoodId | 33    | 1.23E0 | GoodId | 34    | 1.23E0 || D      | false  || No trap MODC and HgConcentration match the MHV.
        /// | 26 | 01   | 1.23E0 | ST      | GoodId | 34    | 1.23E0 | GoodId | 01    | 1.22E0 | GoodId | 35    | 1.23E0 || D      | false  || No trap MODC and HgConcentration match the MHV.
        /// | 27 | 01   | 1.23E0 | ST      | GoodId | 35    | 1.23E0 | GoodId | 01    | 1.24E0 | GoodId | 34    | 1.23E0 || D      | false  || No trap MODC and HgConcentration match the MHV.
        /// | 28 | 35   | null   | ST      | GoodId | 01    | 1.23E0 | GoodId | 01    | 1.23E0 | GoodId | 34    | 1.23E0 || null   | true   || 35 MHV MODC with trap MODC of 01.
        /// | 29 | 35   | null   | ST      | GoodId | 02    | 1.23E0 | GoodId | 01    | 1.23E0 | GoodId | 34    | 1.23E0 || null   | true   || 35 MHV MODC with trap MODC of 02.
        /// </summary>
        [TestMethod()]
        public void MatsMhv4()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            /* Input Parameter Values */
            DateTime currentHour = new DateTime(2016, 6, 17, 22, 0, 0);
            string goodId = "GoodId";
            string badId = "BadId";
            string trap2SysId = goodId;

            string[] modcList = { null, "01", "02", "32", "33", "34", "35", "41", "42", "01", "02", "34", "35", "01", "02", "34", "35", "01", "01", "01", "01", "01", "01", "01", "34", "35", "01", "01", "35", "35" };
            string[] valueList = { "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0" };
            string[] sysTypeList = { "ST", "HG", "HG", "ST", "ST", "HG", "HG", "ST", "ST", "ST", "ST", "ST", "ST", "ST", "ST", "ST", "ST", "ST", "ST", "ST", "ST", "ST", "ST", "ST", "ST", "ST", "ST", "ST", "ST", "ST" };
            string[] trap0SysIdList = { goodId, goodId, goodId, goodId, goodId, goodId, goodId, goodId, goodId, goodId, goodId, goodId, goodId, goodId, goodId, goodId, goodId, goodId, goodId, goodId, goodId, badId, goodId, goodId, goodId, goodId, goodId, goodId, goodId, goodId };
            string[] trap0ModcList = { "02", "02", "01", "02", "02", "02", "02", "02", "02", "01", "02", "02", "02", "02", "01", "32", "34", "34", "35", "34", "35", "01", "34", "35", "34", "35", "34", "35", "01", "02" };
            string[] trap0ValueList = { "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.22E0", "1.24E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0" };
            string[] trap1SysIdList = { goodId, goodId, goodId, goodId, goodId, goodId, goodId, goodId, goodId, goodId, goodId, goodId, goodId, goodId, goodId, goodId, goodId, goodId, goodId, goodId, goodId, goodId, badId, badId, goodId, goodId, goodId, goodId, goodId, goodId };
            string[] trap1ModcList = { "02", "02", "01", "02", "02", "02", "02", "02", "02", "02", "01", "34", "35", "01", "02", "01", "33", "01", "01", "02", "02", "02", "02", "02", "01", "33", "01", "01", "33", "33" };
            string[] trap1ValueList = { "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.22E0", "1.24E0", "1.23E0", "1.23E0" };
            string[] trap2ModcList = { "02", "02", "01", "02", "02", "02", "02", "02", "02", "02", "01", "02", "02", "02", "01", "02", "34", "02", "02", "35", "34", "02", "01", "01", "02", "34", "35", "34" , "34", "34"};
            string[] trap2ValueList = { "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0", "1.23E0" };

            /* Expected Values */
            string[] expResultList = { "A", null, null, "B", "B", null, null, null, null, null, null, null, null, null, null, "D", "D", null, null, "D", "D", "D", null, null, "D", "D", "D", "D", null, null };
            bool?[] expStatusList = { false, true, true, false, false, true, true, true, true, true, true, true, true, true, true, false, false, true, true, false, false, false, true, true, false, false, false, false, true, true };

            /* Test Case Count */
            int caseCount = 30;

            /* Check array lengths */
            Assert.AreEqual(caseCount, modcList.Length, "modcList length");
            Assert.AreEqual(caseCount, valueList.Length, "valueList length");
            Assert.AreEqual(caseCount, sysTypeList.Length, "sysTypeList length");
            Assert.AreEqual(caseCount, trap0SysIdList.Length, "trap0SysIdList length");
            Assert.AreEqual(caseCount, trap0ModcList.Length, "trap0ModcList length");
            Assert.AreEqual(caseCount, trap0ValueList.Length, "trap0ValueList length");
            Assert.AreEqual(caseCount, trap1SysIdList.Length, "trap1SysIdList length");
            Assert.AreEqual(caseCount, trap1ModcList.Length, "trap1ModcList length");
            Assert.AreEqual(caseCount, trap1ValueList.Length, "trap1ValueList length");
            Assert.AreEqual(caseCount, trap2ModcList.Length, "trap2ModcList length");
            Assert.AreEqual(caseCount, trap2ValueList.Length, "trap2ValueList length");
            Assert.AreEqual(caseCount, expResultList.Length, "expResultList length");
            Assert.AreEqual(caseCount, expStatusList.Length, "expStatusList length");


            /* Run Cases */
            for (int caseDex = 0; caseDex < caseCount; caseDex++)
            {
                /* Initialize Input Parameters */
                EmParameters.CurrentDateHour = currentHour;
                EmParameters.CurrentMhvSystemType = sysTypeList[caseDex];
                EmParameters.MatsMhvMeasuredModcList = "01,02,41,42";
                EmParameters.MatsMhvRecord = new MATSMonitorHourlyValueData(modcCd: modcList[caseDex], unadjustedHrlyValue: valueList[caseDex], monSysId: goodId);
                EmParameters.MatsMhvSorbentTraps = new CheckDataView<MatsSorbentTrapRecord>();
                {
                    if (trap0SysIdList[caseDex] == goodId)
                        EmParameters.MatsMhvSorbentTraps.Add(SetValues.DateHour(new MatsSorbentTrapRecord(modcCd: trap0ModcList[caseDex], hgConcentration: trap0ValueList[caseDex], monSysId: trap0SysIdList[caseDex]), currentHour.AddHours(-1), currentHour));
                    if (trap1SysIdList[caseDex] == goodId)
                        EmParameters.MatsMhvSorbentTraps.Add(SetValues.DateHour(new MatsSorbentTrapRecord(modcCd: trap1ModcList[caseDex], hgConcentration: trap1ValueList[caseDex], monSysId: trap1SysIdList[caseDex]), currentHour, currentHour));
                    EmParameters.MatsMhvSorbentTraps.Add(SetValues.DateHour(new MatsSorbentTrapRecord(modcCd: trap2ModcList[caseDex], hgConcentration: trap2ValueList[caseDex], monSysId: trap2SysId), currentHour, currentHour.AddHours(1)));
                }
                EmParameters.MatsMhvUnavailableModcList = "34,35";

                /* Initialize Output Parameters */
                EmParameters.MonitorHourlyModcStatus = null;


                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Initialize variables needed to run the check. */
                bool log = false;
                string actual;

                /* Run Check */
                actual = cMATSMonitorHourlyValueChecks.MATSMHV4(category, ref log);


                /* Check Results */
                Assert.AreEqual(string.Empty, actual, string.Format("actual[case {0}]", caseDex));
                Assert.AreEqual(false, log, string.Format("log [case {0}]", caseDex));
                Assert.AreEqual(expResultList[caseDex], category.CheckCatalogResult, string.Format("category.CheckCatalogResult [case {0}]", caseDex));

                Assert.AreEqual(expStatusList[caseDex], EmParameters.MonitorHourlyModcStatus, string.Format("MonitorHourlyModcStatus [case {0}]", caseDex));
            }
        }

        #region MATSMHV-5

        /// <summary>
        ///A test for MATSMHV-5
        ///</summary>()
        [TestMethod()]
        public void MATSMHV5()
        {
            //static check setup
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            // Variables
            bool log = false;
            string actual;

            //Result A
            {
                // Init Input
                EmParameters.MonitorHourlyModcStatus = true;
                EmParameters.MatsMhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData(pctAvailable: null);

                // Init Output
                category.CheckCatalogResult = null;

                // Run Checks
                actual = cMATSMonitorHourlyValueChecks.MATSMHV5(category, ref log);

                // Check Results
                Assert.AreEqual(false, log);
                Assert.AreEqual("A", category.CheckCatalogResult, "Result");
                Assert.AreEqual(false, EmParameters.MonitorHourlyPmaStatus, "MonitorHourlyPmaStatus");
            }


            //Result B
            {
                // Init Input
                EmParameters.MonitorHourlyModcStatus = true;
                decimal[] testPctAvailList = { (decimal)-0.1, (decimal)100.1 };
                foreach (decimal testPctAvail in testPctAvailList)
                {
                    EmParameters.MatsMhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData(pctAvailable: testPctAvail);

                    // Init Output
                    category.CheckCatalogResult = null;

                    // Run Checks
                    actual = cMATSMonitorHourlyValueChecks.MATSMHV5(category, ref log);

                    // Check Results
                    Assert.AreEqual(false, log);
                    Assert.AreEqual("B", category.CheckCatalogResult, "Result");
                    Assert.AreEqual(false, EmParameters.MonitorHourlyPmaStatus, "MonitorHourlyPmaStatus");
                }
            }

            //Pass - PctAvailable valid
            {
                // Init Input
                EmParameters.MonitorHourlyModcStatus = true;
                EmParameters.MatsMhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData(pctAvailable: (decimal)0.1);

                // Init Output
                category.CheckCatalogResult = null;

                // Run Checks
                actual = cMATSMonitorHourlyValueChecks.MATSMHV5(category, ref log);

                // Check Results
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual(true, EmParameters.MonitorHourlyPmaStatus, "MonitorHourlyPmaStatus");
            }

            //Pass - ModcStatus false
            {
                // Init Input
                EmParameters.MonitorHourlyModcStatus = false;
                EmParameters.MatsMhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData(pctAvailable: null);

                // Init Output
                category.CheckCatalogResult = null;

                // Run Checks
                actual = cMATSMonitorHourlyValueChecks.MATSMHV5(category, ref log);

                // Check Results
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual(false, EmParameters.MonitorHourlyPmaStatus, "MonitorHourlyPmaStatus");
            }

        }
        #endregion


        /// <summary>
        /// MatsMhv-6
        /// 
        /// MatsMhvMeasuredModcList: 01, 02
        /// 
        ///                                            |     SysType     |
        /// | ## | Status | MODC | SysId | SysIdent    | Current | Mhv   | StCnt || Result | Status || Note
        /// |  0 | false  | 00   | null  | null        | CEM     | null  | 0     || null   | false  || MODC status is false.
        /// |  1 | true   | 33   | null  | null        | CEM     | null  | 0     || F      | false  || Unavalable MODC and MHV MonSysId is null.
        /// |  2 | true   | 33   | GOOD  | Good System | CEM     | CEM   | 0     || null   | true   || Unavalable MODC and MHV MonSysId is not null.
        /// |  3 | true   | 01   | null  | null        | CEM     | null  | 0     || A      | false  || MHV MonSysId and System Identifier are null.
        /// |  4 | true   | 01   | GOOD  | null        | CEM     | CEM   | 0     || B      | false  || MHV System Identifier is null.
        /// |  5 | true   | 01   | GOOD  | Good System | CEM     | ST    | 0     || C      | false  || MHV system type does not match the current system type.
        /// |  6 | true   | 01   | GOOD  | Good System | ST      | CEM   | 0     || C      | false  || MHV system type does not match the current system type.
        /// |  7 | true   | 01   | GOOD  | Good System | CEM     | CEM   | 0     || null   | true   || CEM system with measured MODC and system id and identifier.
        /// |  8 | true   | 01   | GOOD  | Good System | ST      | ST    | 0     || E      | false  || ST system without an active sorbent trap.
        /// |  9 | true   | 01   | GOOD  | Good System | ST      | ST    | 1     || null   | true   || ST system with one active sorbent trap.
        /// | 10 | true   | 01   | GOOD  | Good System | ST      | ST    | 2     || null   | true   || ST system with two active sorbent traps.
        /// | 11 | true   | 33   | GOOD  | Good System | CEM     | ST    | 0     || C      | false  || Unavalable MODC and MHV system type does not match the current system type.
        /// | 12 | true   | 33   | GOOD  | Good System | ST      | CEM   | 0     || C      | false  || Unavalable MODC and MHV system type does not match the current system type.
        /// | 13 | true   | 33   | GOOD  | Good System | CEM     | CEM   | 0     || null   | true   || Unavalable MODC and CEM system with measured MODC and system id and identifier.
        /// | 14 | true   | 01   | GOOD  | Good System | ST      | ST    | 0     || E      | false  || Measured MODC and ST system without an active sorbent trap.
        /// | 15 | true   | 01   | GOOD  | Good System | ST      | ST    | 1     || null   | true   || Measured MODC and ST system with one active sorbent trap.
        /// | 16 | true   | 01   | GOOD  | Good System | ST      | ST    | 2     || null   | true   || Measured MODC and ST system with two active sorbent traps.
        /// | 17 | true   | 33   | GOOD  | Good System | ST      | ST    | 0     || null   | false  || Unavalable MODC and ST system without an active sorbent trap.
        /// </summary>
        [TestMethod()]
        public void MatsMhv6()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            /* Input Parameter Values */
            bool?[] modcStatusList = { false, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true };
            string[] modcList = { "00", "33", "33", "01", "01", "01", "01", "01", "01", "01", "01", "33", "33", "33", "01", "01", "01", "33" };
            string[] sysIdList = { null, null, "GOOD", null, "GOOD", "GOOD", "GOOD", "GOOD", "GOOD", "GOOD", "GOOD", "GOOD", "GOOD", "GOOD", "GOOD", "GOOD", "GOOD", "GOOD" };
            string[] sysIdentifierList = { null, null, "Good System", null, null, "Good System", "Good System", "Good System", "Good System", "Good System", "Good System", "Good System", "Good System", "Good System", "Good System", "Good System", "Good System", "Good System" };
            string[] currentSysTypeList = { "CEM", "CEM", "CEM", "CEM", "CEM", "CEM", "ST", "CEM", "ST", "ST", "ST", "CEM", "ST", "CEM", "ST", "ST", "ST", "ST" };
            string[] mhvSysTypeList = { null, null, "CEM", null, "CEM", "ST", "CEM", "CEM", "ST", "ST", "ST", "ST", "CEM", "CEM", "ST", "ST", "ST", "ST" };
            int?[] stCountList = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 2, 0, 0, 0, 0, 1, 2, 0 };

            /* Expected Values */
            string[] resultList = { null, "F", null, "A", "B", "C", "C", null, "E", null, null, "C", "C", null, "E", null, null, null };
            bool?[] expStatusList = { false, false, true, false, false, false, false, true, false, true, true, false, false, true, false, true, true, false };

            /* Case Count */
            int caseCount = 18;

            /* Check array lengths */
            Assert.AreEqual(caseCount, modcStatusList.Length, "modcStatusList length");
            Assert.AreEqual(caseCount, modcList.Length, "modcList length");
            Assert.AreEqual(caseCount, sysIdList.Length, "sysIdList length");
            Assert.AreEqual(caseCount, sysIdentifierList.Length, "sysIdentifierList length");
            Assert.AreEqual(caseCount, currentSysTypeList.Length, "currentSysTypeList length");
            Assert.AreEqual(caseCount, mhvSysTypeList.Length, "mhvSysTypeList length");
            Assert.AreEqual(caseCount, stCountList.Length, "stCountList length");
            Assert.AreEqual(caseCount, resultList.Length, "resultList length");
            Assert.AreEqual(caseCount, expStatusList.Length, "expStatusList length");

            for (int caseDex = 0; caseDex < caseCount; caseDex++)
            {
                /* Initialize Input Parameters */
                EmParameters.CurrentMhvSystemType = currentSysTypeList[caseDex];
                EmParameters.MatsMhvMeasuredModcList = "01,02";
                EmParameters.MatsMhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData
                    (
                        modcCd: modcList[caseDex], 
                        monSysId: sysIdList[caseDex], 
                        systemIdentifier: sysIdentifierList[caseDex], 
                        sysTypeCd: mhvSysTypeList[caseDex]
                    );
                EmParameters.MatsMhvSorbentTraps = new CheckDataView<MatsSorbentTrapRecord>();
                {
                    for (int dex = 0; dex < stCountList[caseDex]; dex++)
                        EmParameters.MatsMhvSorbentTraps.Add(new MatsSorbentTrapRecord(monSysId: sysIdList[caseDex]));
                }
                EmParameters.MonitorHourlyModcStatus = modcStatusList[caseDex];

                /* Initialize Output Parameters */
                EmParameters.MonitorHourlySystemStatus = null;

                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Initialize variables needed to run the check. */
                bool log = false;
                string actual;

                /* Run Check */
                actual = cMATSMonitorHourlyValueChecks.MATSMHV6(category, ref log);

                /* Check results */
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);

                Assert.AreEqual(resultList[caseDex], category.CheckCatalogResult, string.Format("Result {0}", caseDex));
                Assert.AreEqual(expStatusList[caseDex], EmParameters.MonitorHourlySystemStatus, string.Format("MonitorHourlySystemStatus {0}", caseDex));
            }

        }

        #region MATSMHV-7

        /// <summary>
        ///A test for MATSMHV-7
        ///</summary>()
        [TestMethod()]
        public void MATSMHV7()
        {
            //static check setup
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            // Variables
            bool log = false;
            string actual;
            string[] testModcList = { "01", "17", "02", "99" };
            string[] testSysDesignationList = { "B", "RB", "BAD" };


            //Modc codes
            {
                // Init Input
                EmParameters.MonitorHourlyModcStatus = true;
                EmParameters.MonitorHourlySystemStatus = true;

                foreach (string testModcCode in testModcList)
                {
                    foreach (string testSysDesignationCode in testSysDesignationList)
                    {
                        EmParameters.MatsMhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData
                            (systemIdentifier: "NOTNULL", modcCd: testModcCode, sysDesignationCd: testSysDesignationCode);
                        // Init Output
                        category.CheckCatalogResult = null;


                        // Run Checks
                        actual = cMATSMonitorHourlyValueChecks.MATSMHV7(category, ref log);

                        // Check Results
                        Assert.AreEqual(false, log);
                        if (testModcCode.InList("01,17"))
                        {
                            Assert.AreEqual("A", category.CheckCatalogResult, "Result");
                        }
                        else if ((testModcCode.InList("02")) && testSysDesignationCode.NotInList("B,RB"))
                        {
                            Assert.AreEqual("B", category.CheckCatalogResult, "Result");
                        }
                        else
                        {
                            Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                        }
                    }
                }
            }

            //Pass - modc status false
            {
                // Init Input
                EmParameters.MonitorHourlyModcStatus = false;
                EmParameters.MonitorHourlySystemStatus = true;
                EmParameters.MatsMhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData(systemIdentifier: "NOTNULL");

                // Init Output
                category.CheckCatalogResult = null;

                // Run Checks
                actual = cMATSMonitorHourlyValueChecks.MATSMHV7(category, ref log);

                // Check Results
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
            }

            //Pass - system status false
            {
                // Init Input
                EmParameters.MonitorHourlyModcStatus = true;
                EmParameters.MonitorHourlySystemStatus = false;
                EmParameters.MatsMhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData(systemIdentifier: "NOTNULL");

                // Init Output
                category.CheckCatalogResult = null;

                // Run Checks
                actual = cMATSMonitorHourlyValueChecks.MATSMHV7(category, ref log);

                // Check Results
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
            }

            //Pass - sysID null
            {
                // Init Input
                EmParameters.MonitorHourlyModcStatus = true;
                EmParameters.MonitorHourlySystemStatus = true;
                EmParameters.MatsMhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData(systemIdentifier: null);

                // Init Output
                category.CheckCatalogResult = null;

                // Run Checks
                actual = cMATSMonitorHourlyValueChecks.MATSMHV7(category, ref log);

                // Check Results
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
            }

        }
        #endregion

        #region MATSMHV-8

        /// <summary>
        ///A test for MATSMHV-8
        ///</summary>()
        [TestMethod()]
        public void MATSMHV8()
        {
            Assert.AreEqual(null, null); //TODO: Right test for check implementation using ModcHourCounts object for category.

            ////static check setup
            //cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            //EmParameters.Init(category.Process);
            //EmParameters.Category = category;

            //// Variables
            //bool log = false;
            //string actual;
            //DateTime testDate = DateTime.Today.AddDays(-90);

            //// Init Input
            //EmParameters.MonitorHourlyModcStatus = true;
            //EmParameters.MonitorHourlySystemStatus = true;
            //EmParameters.CurrentMhvParameter = "GOOD";
            //EmParameters.CurrentOperatingDatehour = DateTime.Today.AddDays(-30);

            ////Result A (no RATA test found)
            //{
            //  EmParameters.MatsMhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData(modcCd: "17", monSysId: "SYS1");
            //  EmParameters.RataTestRecordsByLocationForQaStatus = new CheckDataView<VwQaSuppDataHourlyStatusRow>();
            //  EmParameters.MatsMhvRecordsByHourLocation = new CheckDataView<ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData>();
            //  for (int i = 1; i <= 720; i++)
            //  {
            //    // Init Input
            //    DataRowView MHVrow = EmParameters.MatsMhvRecordsByHourLocation.SourceView.AddNew();
            //    MHVrow["PARAMETER_CD"] = "GOOD";
            //    MHVrow["MODC_CD"] = "17";
            //    MHVrow["BEGIN_DATEHOUR"] = testDate.AddHours(i);
            //  }
            //  // Init Output
            //  category.CheckCatalogResult = null;

            //  // Run Checks
            //  actual = cMATSMonitorHourlyValueChecks.MATSMHV8(category, ref log);

            //  // Check Results
            //  Assert.AreEqual(false, log);
            //  Assert.AreEqual("A", category.CheckCatalogResult, "Result");
            //}

            ////Pass - MODC <> 17
            //{
            //  EmParameters.MatsMhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData(modcCd: "BAD", monSysId: "SYS1");
            //  EmParameters.RataTestRecordsByLocationForQaStatus = new CheckDataView<VwQaSuppDataHourlyStatusRow>();
            //  EmParameters.MatsMhvRecordsByHourLocation = new CheckDataView<ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData>();

            //  // Init Output
            //  category.CheckCatalogResult = null;

            //  // Run Checks
            //  actual = cMATSMonitorHourlyValueChecks.MATSMHV8(category, ref log);

            //  // Check Results
            //  Assert.AreEqual(false, log);
            //  Assert.AreEqual(null, category.CheckCatalogResult, "Result");
            //}

            ////Pass < 720 hours
            //{
            //  EmParameters.MatsMhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData(modcCd: "BAD", monSysId: "SYS1");
            //  EmParameters.RataTestRecordsByLocationForQaStatus = new CheckDataView<VwQaSuppDataHourlyStatusRow>();
            //  EmParameters.MatsMhvRecordsByHourLocation = new CheckDataView<ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData>();

            //  for (int i = 1; i <= 719; i++)
            //  {
            //    // Init Input
            //    DataRowView MHVrow = EmParameters.MatsMhvRecordsByHourLocation.SourceView.AddNew();
            //    MHVrow["PARAMETER_CD"] = "GOOD";
            //    MHVrow["MODC_CD"] = "17";
            //    MHVrow["BEGIN_DATEHOUR"] = testDate.AddHours(i);
            //  }
            //  // Init Output
            //  category.CheckCatalogResult = null;

            //  // Run Checks
            //  actual = cMATSMonitorHourlyValueChecks.MATSMHV8(category, ref log);

            //  // Check Results
            //  Assert.AreEqual(false, log);
            //  Assert.AreEqual(null, category.CheckCatalogResult, "Result");
            //}

            ////Pass - found RATA
            //{
            //  EmParameters.MatsMhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData(modcCd: "17", monSysId: "SYS1");
            //  EmParameters.RataTestRecordsByLocationForQaStatus = new CheckDataView<VwQaSuppDataHourlyStatusRow>
            //    (new VwQaSuppDataHourlyStatusRow(monSysId: "SYS1", testResultCd: "PASS", endDatehour: DateTime.Today.AddDays(-60)));
            //  EmParameters.MatsMhvRecordsByHourLocation = new CheckDataView<ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData>();
            //  for (int i = 1; i <= 720; i++)
            //  {
            //    // Init Input
            //    DataRowView MHVrow = EmParameters.MatsMhvRecordsByHourLocation.SourceView.AddNew();
            //    MHVrow["PARAMETER_CD"] = "GOOD";
            //    MHVrow["MODC_CD"] = "17";
            //    MHVrow["BEGIN_DATEHOUR"] = testDate.AddHours(i);
            //  }
            //  // Init Output
            //  category.CheckCatalogResult = null;

            //  // Run Checks
            //  actual = cMATSMonitorHourlyValueChecks.MATSMHV8(category, ref log);

            //  // Check Results
            //  Assert.AreEqual(false, log);
            //  Assert.AreEqual(null, category.CheckCatalogResult, "Result");
            //}
        }
        #endregion

        #region MATSMHV-9

        /// <summary>
        ///A test for MATSMHV-9
        ///</summary>()
        [TestMethod()]
        public void MATSMHV9()
        {
            //static check setup
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            // Variables
            bool log = false;
            string actual;

            {
                // Init Input
                EmParameters.MonitorHourlyModcStatus = true;
                EmParameters.MatsMhvMeasuredModcList = "01,02,17";
                EmParameters.CurrentMhvComponentType = "GOOD";
                EmParameters.CurrentMhvSystemType = "CEM";

                //Result A
                {
                    EmParameters.MatsMhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData
                        (modcCd: "01", componentId: null);

                    // Init Output
                    category.CheckCatalogResult = null;

                    // Run Checks
                    actual = cMATSMonitorHourlyValueChecks.MATSMHV9(category, ref log);

                    // Check Results
                    Assert.AreEqual(false, log);
                    Assert.AreEqual("A", category.CheckCatalogResult, "Result");
                    Assert.AreEqual(false, EmParameters.MonitorHourlyComponentStatus, "MonitorHourlyComponentStatus");
                }

                //Result G
                {
                    EmParameters.MatsMhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData
                        (modcCd: "33", componentId: null);

                    // Init Output
                    category.CheckCatalogResult = null;

                    // Run Checks
                    actual = cMATSMonitorHourlyValueChecks.MATSMHV9(category, ref log);

                    // Check Results
                    Assert.AreEqual(false, log);
                    Assert.AreEqual("G", category.CheckCatalogResult, "Result");
                    Assert.AreEqual(false, EmParameters.MonitorHourlyComponentStatus, "MonitorHourlyComponentStatus");
                }

                //Result B Measure MODC
                {
                    EmParameters.MatsMhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData
                        (modcCd: "01", componentId: "NOTNULL", componentIdentifier: null);

                    // Init Output
                    category.CheckCatalogResult = null;

                    // Run Checks
                    actual = cMATSMonitorHourlyValueChecks.MATSMHV9(category, ref log);

                    // Check Results
                    Assert.AreEqual(false, log);
                    Assert.AreEqual("B", category.CheckCatalogResult, "Result");
                    Assert.AreEqual(false, EmParameters.MonitorHourlyComponentStatus, "MonitorHourlyComponentStatus");
                }

                //Result B Unavailable MODC
                {
                    EmParameters.MatsMhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData
                        (modcCd: "33", componentId: "NOTNULL", componentIdentifier: null);

                    // Init Output
                    category.CheckCatalogResult = null;

                    // Run Checks
                    actual = cMATSMonitorHourlyValueChecks.MATSMHV9(category, ref log);

                    // Check Results
                    Assert.AreEqual(false, log);
                    Assert.AreEqual("B", category.CheckCatalogResult, "Result");
                    Assert.AreEqual(false, EmParameters.MonitorHourlyComponentStatus, "MonitorHourlyComponentStatus");
                }

                //Result C Measured MODC
                {
                    EmParameters.MatsMhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData
                        (modcCd: "01", componentId: "NOTNULL", componentIdentifier: "NOTNULL", componentTypeCd: "BAD");

                    // Init Output
                    category.CheckCatalogResult = null;

                    // Run Checks
                    actual = cMATSMonitorHourlyValueChecks.MATSMHV9(category, ref log);

                    // Check Results
                    Assert.AreEqual(false, log);
                    Assert.AreEqual("C", category.CheckCatalogResult, "Result");
                    Assert.AreEqual(false, EmParameters.MonitorHourlyComponentStatus, "MonitorHourlyComponentStatus");
                }

                //Result C Unavailable MODC
                {
                    EmParameters.MatsMhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData
                        (modcCd: "33", componentId: "NOTNULL", componentIdentifier: "NOTNULL", componentTypeCd: "BAD");

                    // Init Output
                    category.CheckCatalogResult = null;

                    // Run Checks
                    actual = cMATSMonitorHourlyValueChecks.MATSMHV9(category, ref log);

                    // Check Results
                    Assert.AreEqual(false, log);
                    Assert.AreEqual("C", category.CheckCatalogResult, "Result");
                    Assert.AreEqual(false, EmParameters.MonitorHourlyComponentStatus, "MonitorHourlyComponentStatus");
                }

                //Result D
                {
                    EmParameters.MatsMhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData
                        (modcCd: "17", componentId: "NOTNULL", componentIdentifier: "NOTNULL", componentTypeCd: "GOOD");

                    // Init Output
                    category.CheckCatalogResult = null;

                    // Run Checks
                    actual = cMATSMonitorHourlyValueChecks.MATSMHV9(category, ref log);

                    // Check Results
                    Assert.AreEqual(false, log);
                    Assert.AreEqual("D", category.CheckCatalogResult, "Result");
                    Assert.AreEqual(false, EmParameters.MonitorHourlyComponentStatus, "MonitorHourlyComponentStatus");
                }

                //Result H
                {
                    EmParameters.MatsMhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData
                        (modcCd: "01", componentId: "NOTNULL", componentIdentifier: "LKA", componentTypeCd: "GOOD");

                    // Init Output
                    category.CheckCatalogResult = null;

                    // Run Checks
                    actual = cMATSMonitorHourlyValueChecks.MATSMHV9(category, ref log);

                    // Check Results
                    Assert.AreEqual(false, log);
                    Assert.AreEqual("H", category.CheckCatalogResult, "Result");
                    Assert.AreEqual(false, EmParameters.MonitorHourlyComponentStatus, "MonitorHourlyComponentStatus");
                }

                //Pass - Modc in list
                {
                    EmParameters.MatsMhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData
                        (modcCd: "17", componentId: "NOTNULL", componentIdentifier: "LK", componentTypeCd: "GOOD");

                    // Init Output
                    category.CheckCatalogResult = null;

                    // Run Checks
                    actual = cMATSMonitorHourlyValueChecks.MATSMHV9(category, ref log);

                    // Check Results
                    Assert.AreEqual(false, log);
                    Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                    Assert.AreEqual(true, EmParameters.MonitorHourlyComponentStatus, "MonitorHourlyComponentStatus");
                }

                //Pass - Modc not in list
                {
                    EmParameters.MatsMhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData
                        (modcCd: "99", componentId: null);

                    // Init Output
                    category.CheckCatalogResult = null;

                    // Run Checks
                    actual = cMATSMonitorHourlyValueChecks.MATSMHV9(category, ref log);

                    // Check Results
                    Assert.AreEqual(false, log);
                    Assert.AreEqual("G", category.CheckCatalogResult, "Result");
                    Assert.AreEqual(false, EmParameters.MonitorHourlyComponentStatus, "MonitorHourlyComponentStatus");
                }

                //Pass - Modc status false
                {
                    EmParameters.MonitorHourlyModcStatus = false;
                    EmParameters.MatsMhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData
                        ();

                    // Init Output
                    category.CheckCatalogResult = null;

                    // Run Checks
                    actual = cMATSMonitorHourlyValueChecks.MATSMHV9(category, ref log);

                    // Check Results
                    Assert.AreEqual(false, log);
                    Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                    Assert.AreEqual(false, EmParameters.MonitorHourlyComponentStatus, "MonitorHourlyComponentStatus");
                }
            }

            {
                EmParameters.MonitorHourlyModcStatus = true;
                EmParameters.MatsMhvMeasuredModcList = "01,02,17";
                EmParameters.CurrentMhvComponentType = "GOOD";
                EmParameters.CurrentMhvSystemType = "ST";

                //Result A
                {
                    EmParameters.MatsMhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData(modcCd: "01", componentId: "HAV");

                    // Init Output
                    category.CheckCatalogResult = null;

                    // Run Checks
                    actual = cMATSMonitorHourlyValueChecks.MATSMHV9(category, ref log);

                    // Check Results
                    Assert.AreEqual(false, log);
                    Assert.AreEqual("F", category.CheckCatalogResult, "Result");
                    Assert.AreEqual(false, EmParameters.MonitorHourlyComponentStatus, "MonitorHourlyComponentStatus");
                }

                //Result F
                {
                    EmParameters.MatsMhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData(modcCd: "01", componentId: null);

                    // Init Output
                    category.CheckCatalogResult = null;

                    // Run Checks
                    actual = cMATSMonitorHourlyValueChecks.MATSMHV9(category, ref log);

                    // Check Results
                    Assert.AreEqual(false, log);
                    Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                    Assert.AreEqual(true, EmParameters.MonitorHourlyComponentStatus, "MonitorHourlyComponentStatus");
                }
            }
        }
        #endregion

        #region MATSMHV-10

        /// <summary>
        ///A test for MATSMHV-10
        ///</summary>()
        [TestMethod()]
        public void MATSMHV10()
        {
            //static check setup
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            // Variables
            bool log = false;
            string actual;

            //Result A
            {
                // Init Input
                EmParameters.MonitorHourlySystemStatus = true;
                EmParameters.MonitorHourlyComponentStatus = true;
                EmParameters.MatsMhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData
                    (monSysId: "SYS1", componentId: "COMP1");
                EmParameters.MonitorSystemComponentRecordsByHourLocation = new CheckDataView<VwMpMonitorSystemComponentRow>();

                // Init Output
                category.CheckCatalogResult = null;

                // Run Checks
                actual = cMATSMonitorHourlyValueChecks.MATSMHV10(category, ref log);

                // Check Results
                Assert.AreEqual(false, log);
                Assert.AreEqual("A", category.CheckCatalogResult, "Result");
            }

            //Pass - records found
            {
                // Init Input
                EmParameters.MonitorHourlySystemStatus = true;
                EmParameters.MonitorHourlyComponentStatus = true;
                EmParameters.MatsMhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData
                    (monSysId: "SYS1", componentId: "COMP1");
                EmParameters.MonitorSystemComponentRecordsByHourLocation = new CheckDataView<VwMpMonitorSystemComponentRow>(new VwMpMonitorSystemComponentRow
                    (monSysId: "SYS1", componentId: "COMP1"));

                // Init Output
                category.CheckCatalogResult = null;

                // Run Checks
                actual = cMATSMonitorHourlyValueChecks.MATSMHV10(category, ref log);

                // Check Results
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
            }

            //Pass - system status false
            {
                // Init Input
                EmParameters.MonitorHourlySystemStatus = false;
                EmParameters.MonitorHourlyComponentStatus = true;
                EmParameters.MatsMhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData
                    (monSysId: "SYS1", componentId: "COMP1");
                EmParameters.MonitorSystemComponentRecordsByHourLocation = new CheckDataView<VwMpMonitorSystemComponentRow>(new VwMpMonitorSystemComponentRow
                    (monSysId: "SYS1", componentId: "COMP1"));

                // Init Output
                category.CheckCatalogResult = null;

                // Run Checks
                actual = cMATSMonitorHourlyValueChecks.MATSMHV10(category, ref log);

                // Check Results
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
            }

            //Pass - component status false
            {
                // Init Input
                EmParameters.MonitorHourlySystemStatus = true;
                EmParameters.MonitorHourlyComponentStatus = false;
                EmParameters.MatsMhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData
                    (monSysId: "SYS1", componentId: "COMP1");
                EmParameters.MonitorSystemComponentRecordsByHourLocation = new CheckDataView<VwMpMonitorSystemComponentRow>(new VwMpMonitorSystemComponentRow
                    (monSysId: "SYS1", componentId: "COMP1"));

                // Init Output
                category.CheckCatalogResult = null;

                // Run Checks
                actual = cMATSMonitorHourlyValueChecks.MATSMHV10(category, ref log);

                // Check Results
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
            }

            //Pass - systemID null
            {
                // Init Input
                EmParameters.MonitorHourlySystemStatus = true;
                EmParameters.MonitorHourlyComponentStatus = true;
                EmParameters.MatsMhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData
                    (monSysId: null, componentId: "COMP1");
                EmParameters.MonitorSystemComponentRecordsByHourLocation = new CheckDataView<VwMpMonitorSystemComponentRow>(new VwMpMonitorSystemComponentRow
                    (monSysId: "SYS1", componentId: "COMP1"));

                // Init Output
                category.CheckCatalogResult = null;

                // Run Checks
                actual = cMATSMonitorHourlyValueChecks.MATSMHV10(category, ref log);

                // Check Results
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
            }

            //Pass - componentID null
            {
                // Init Input
                EmParameters.MonitorHourlySystemStatus = true;
                EmParameters.MonitorHourlyComponentStatus = true;
                EmParameters.MatsMhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData
                    (monSysId: "SYS1", componentId: null);
                EmParameters.MonitorSystemComponentRecordsByHourLocation = new CheckDataView<VwMpMonitorSystemComponentRow>(new VwMpMonitorSystemComponentRow
                    (monSysId: "SYS1", componentId: "COMP1"));

                // Init Output
                category.CheckCatalogResult = null;

                // Run Checks
                actual = cMATSMonitorHourlyValueChecks.MATSMHV10(category, ref log);

                // Check Results
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
            }

        }
        #endregion
        #endregion

        #region Checks 11-20
        #region MATSMHV-11

        /// <summary>
        ///A test for MATSMHV-11
        ///</summary>()
        [TestMethod()]
        public void MATSMHV11()
        {
            //static check setup
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            // Variables
            bool log = false;
            string actual;

            // Init Input
            EmParameters.MonitorHourlyModcStatus = true;
            EmParameters.MatsMhvMeasuredModcList = "01,02";
            EmParameters.CurrentMhvComponentType = "HG";

            //Result A
            {
                EmParameters.MatsMhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData
                    (modcCd: "01");
                EmParameters.MonitorSpanRecordsByHourLocation = new CheckDataView<VwMpMonitorSpanRow>
                    (new VwMpMonitorSpanRow(componentTypeCd: "HG", spanScaleCd: "H"),
                    new VwMpMonitorSpanRow(componentTypeCd: "HG", spanScaleCd: "H"));

                // Init Output
                category.CheckCatalogResult = null;

                // Run Checks
                actual = cMATSMonitorHourlyValueChecks.MATSMHV11(category, ref log);

                // Check Results
                Assert.AreEqual(false, log);
                Assert.AreEqual("A", category.CheckCatalogResult, "Result");
                Assert.AreEqual(null, EmParameters.CurrentMhvMaxMinValue, "CurrentMhvMaxMinValue");
            }

            //Result B
            {
                EmParameters.MatsMhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData
                    (modcCd: "01");
                EmParameters.MonitorSpanRecordsByHourLocation = new CheckDataView<VwMpMonitorSpanRow>();

                // Init Output
                category.CheckCatalogResult = null;

                // Run Checks
                actual = cMATSMonitorHourlyValueChecks.MATSMHV11(category, ref log);

                // Check Results
                Assert.AreEqual(false, log);
                Assert.AreEqual("B", category.CheckCatalogResult, "Result");
                Assert.AreEqual(null, EmParameters.CurrentMhvMaxMinValue, "CurrentMhvMaxMinValue");
            }

            //Result C
            {
                EmParameters.MatsMhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData
                    (modcCd: "01");
                EmParameters.MonitorSpanRecordsByHourLocation = new CheckDataView<VwMpMonitorSpanRow>
                    (new VwMpMonitorSpanRow(componentTypeCd: "HG", spanScaleCd: "H", mpcValue: 0));

                // Init Output
                category.CheckCatalogResult = null;

                // Run Checks
                actual = cMATSMonitorHourlyValueChecks.MATSMHV11(category, ref log);

                // Check Results
                Assert.AreEqual(false, log);
                Assert.AreEqual("C", category.CheckCatalogResult, "Result");
                Assert.AreEqual(null, EmParameters.CurrentMhvMaxMinValue, "CurrentMhvMaxMinValue");
            }

            //Pass - set MhvMaxMin
            {
                EmParameters.MatsMhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData
                    (modcCd: "01");
                EmParameters.MonitorSpanRecordsByHourLocation = new CheckDataView<VwMpMonitorSpanRow>
                    (new VwMpMonitorSpanRow(componentTypeCd: "HG", spanScaleCd: "H", mpcValue: (decimal)1.1));

                // Init Output
                category.CheckCatalogResult = null;

                // Run Checks
                actual = cMATSMonitorHourlyValueChecks.MATSMHV11(category, ref log);

                // Check Results
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual((decimal)1.1, EmParameters.CurrentMhvMaxMinValue, "CurrentMhvMaxMinValue");
            }

            //Modc status false
            {
                EmParameters.MatsMhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData
                    (modcCd: "01");
                EmParameters.MonitorHourlyModcStatus = false;

                // Init Output
                category.CheckCatalogResult = null;

                // Run Checks
                actual = cMATSMonitorHourlyValueChecks.MATSMHV11(category, ref log);

                // Check Results
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual(null, EmParameters.CurrentMhvMaxMinValue, "CurrentMhvMaxMinValue");
            }

            //Modc code not in list
            {
                EmParameters.MatsMhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData
                    (modcCd: "99");
                EmParameters.MonitorHourlyModcStatus = false;
                EmParameters.MonitorSpanRecordsByHourLocation = new CheckDataView<VwMpMonitorSpanRow>();

                // Init Output
                category.CheckCatalogResult = null;

                // Run Checks
                actual = cMATSMonitorHourlyValueChecks.MATSMHV11(category, ref log);

                // Check Results
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual(null, EmParameters.CurrentMhvMaxMinValue, "CurrentMhvMaxMinValue");
            }
        }
        #endregion

        #region MATSMHV-12

        /// <summary>
        ///A test for MATSMHV-12
        ///</summary>()
        [TestMethod()]
        public void MATSMHV12()
        {
            //static check setup
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            // Variables
            bool log = false;
            string actual;

            // Init Input
            EmParameters.MonitorHourlyModcStatus = true;
            EmParameters.MatsMhvMeasuredModcList = "01,02";

            //MODC 21 Pass
            {
                EmParameters.MatsMhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData
                    (modcCd: "21", unadjustedHrlyValue: "0.00E+0");

                // Init Output
                category.CheckCatalogResult = null;

                // Run Checks
                actual = cMATSMonitorHourlyValueChecks.MATSMHV12(category, ref log);

                // Check Results
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual(true, EmParameters.MonitorHourlyUnadjustedValueStatus, "MonitorHourlyUnadjustedValueStatus");
                Assert.AreEqual("0.00E0", EmParameters.MatsMhvCalculatedValue, "MatsMhvCalculatedValue");
            }

            //MODC 21 Result A
            {
                EmParameters.MatsMhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData
                    (modcCd: "21", unadjustedHrlyValue: "1.01E+0");

                // Init Output
                category.CheckCatalogResult = null;

                // Run Checks
                actual = cMATSMonitorHourlyValueChecks.MATSMHV12(category, ref log);

                // Check Results
                Assert.AreEqual(false, log);
                Assert.AreEqual("A", category.CheckCatalogResult, "Result");
                Assert.AreEqual(false, EmParameters.MonitorHourlyUnadjustedValueStatus, "MonitorHourlyUnadjustedValueStatus");
                Assert.AreEqual("0.00E0", EmParameters.MatsMhvCalculatedValue, "MatsMhvCalculatedValue");
            }

            //Result B
            {
                EmParameters.MatsMhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData
                    (modcCd: "01", unadjustedHrlyValue: null);

                // Init Output
                category.CheckCatalogResult = null;

                // Run Checks
                actual = cMATSMonitorHourlyValueChecks.MATSMHV12(category, ref log);

                // Check Results
                Assert.AreEqual(false, log);
                Assert.AreEqual("B", category.CheckCatalogResult, "Result");
                Assert.AreEqual(false, EmParameters.MonitorHourlyUnadjustedValueStatus, "MonitorHourlyUnadjustedValueStatus");
                Assert.AreEqual(null, EmParameters.MatsMhvCalculatedValue, "MatsMhvCalculatedValue");
            }

            //Result C
            {
                EmParameters.MatsMhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData
                    (modcCd: "01", unadjustedHrlyValue: "1.234E0");

                // Init Output
                category.CheckCatalogResult = null;

                // Run Checks
                actual = cMATSMonitorHourlyValueChecks.MATSMHV12(category, ref log);

                // Check Results
                Assert.AreEqual(false, log);
                Assert.AreEqual("C", category.CheckCatalogResult, "Result");
                Assert.AreEqual(false, EmParameters.MonitorHourlyUnadjustedValueStatus, "MonitorHourlyUnadjustedValueStatus");
                Assert.AreEqual(null, EmParameters.MatsMhvCalculatedValue, "MatsMhvCalculatedValue");
            }

            //Result D
            {
                EmParameters.MatsMhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData
                    (modcCd: "01", unadjustedHrlyValue: "-1.01E-1");

                // Init Output
                category.CheckCatalogResult = null;

                // Run Checks
                actual = cMATSMonitorHourlyValueChecks.MATSMHV12(category, ref log);

                // Check Results
                Assert.AreEqual(false, log);
                Assert.AreEqual("D", category.CheckCatalogResult, "Result");
                Assert.AreEqual(false, EmParameters.MonitorHourlyUnadjustedValueStatus, "MonitorHourlyUnadjustedValueStatus");
                Assert.AreEqual(null, EmParameters.MatsMhvCalculatedValue, "MatsMhvCalculatedValue");
            }

            //Result E
            {
                EmParameters.MatsMhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData
                    (modcCd: "01", unadjustedHrlyValue: "1.11E-1");
                EmParameters.CurrentMhvMaxMinValue = (decimal)0.1;

                // Init Output
                category.CheckCatalogResult = null;

                // Run Checks
                actual = cMATSMonitorHourlyValueChecks.MATSMHV12(category, ref log);

                // Check Results
                Assert.AreEqual(false, log);
                Assert.AreEqual("E", category.CheckCatalogResult, "Result");
                Assert.AreEqual(true, EmParameters.MonitorHourlyUnadjustedValueStatus, "MonitorHourlyUnadjustedValueStatus");
                Assert.AreEqual("1.11E-1", EmParameters.MatsMhvCalculatedValue, "MatsMhvCalculatedValue");
            }

            //Result F
            {
                EmParameters.MatsMhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData
                    (modcCd: "99", unadjustedHrlyValue: "1.00E-1");

                // Init Output
                category.CheckCatalogResult = null;

                // Run Checks
                actual = cMATSMonitorHourlyValueChecks.MATSMHV12(category, ref log);

                // Check Results
                Assert.AreEqual(false, log);
                Assert.AreEqual("F", category.CheckCatalogResult, "Result");
                Assert.AreEqual(false, EmParameters.MonitorHourlyUnadjustedValueStatus, "MonitorHourlyUnadjustedValueStatus");
                Assert.AreEqual(null, EmParameters.MatsMhvCalculatedValue, "MatsMhvCalculatedValue");
            }

            //Pass
            {
                EmParameters.MatsMhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData
                    (modcCd: "01", unadjustedHrlyValue: "1.00E-1");
                EmParameters.CurrentMhvMaxMinValue = (decimal)0.1;

                // Init Output
                category.CheckCatalogResult = null;

                // Run Checks
                actual = cMATSMonitorHourlyValueChecks.MATSMHV12(category, ref log);

                // Check Results
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual(true, EmParameters.MonitorHourlyUnadjustedValueStatus, "MonitorHourlyUnadjustedValueStatus");
                Assert.AreEqual("1.00E-1", EmParameters.MatsMhvCalculatedValue, "MatsMhvCalculatedValue");
            }

        }
        #endregion

        #region MatsMhv-13

        /// <summary>
        /// MatsMhv13
        /// 
        /// 
        /// Static Values:
        /// 
        /// MatsMhvMeasuredModcList             : '86,88,89'
        /// MatsMhvRecord.ComponentBeginDate    : 2016-06-17
        /// MatsMhvRecord.SystemDesignationCode : 'P'
        /// 
        /// 
        ///                              |                              - MatsMhvRecord -                                ||       |       |       |       |       ||
        ///      |     - Statuses -      |                           |     - Component -        |      - System -        ||       |       |       |       |       ||
        /// | ## | Modc  | Comp  | Sys   | MODC | Unadjusted | Param | Id     | Ident  | Type   | Id     | Ident  | Type || DC    | Line  | QGA   | RATA  | WSI   || Note
        /// |  0 | false | true  | true  |  88  |    1.34E-1 | HGC   | GOODC  | GCI    | HG     | GOODS  | GSI    | N/A  || false | false | false | false | false || No status checks due to bad MODC Status
        /// |  1 | true  | true  | true  |  80  |    1.34E-1 | HGC   | GOODC  | GCI    | HG     | GOODS  | GSI    | N/A  || false | false | false | false | false || No status checks due to non measured MODC
        /// |  2 | true  | true  | true  |  88  |       null | HGC   | GOODC  | GCI    | HG     | GOODS  | GSI    | N/A  || false | false | false | false | false || No status checks due to no unadjusted value
        /// |  3 | true  | true  | true  |  88  |    1.34E-1 | HGC   | GOODC  | GCI    | HG     | GOODS  | GSI    | N/A  || true  | true  | false | true  | true  || Status checks for HGC CEMS.
        /// |  4 | true  | false | true  |  88  |    1.34E-1 | HGC   | GOODC  | GCI    | HG     | GOODS  | GSI    | N/A  || false | false | false | true  | false || No HGC component based status checks due to bad component status.
        /// |  5 | true  | true  | true  |  88  |    1.34E-1 | HGC   | null   | null   | null   | GOODS  | GSI    | N/A  || false | false | false | true  | false || No HGC component based status checks due to null component id.
        /// |  6 | true  | true  | true  |  88  |    1.34E-1 | HGC   | GOODC  | GCI    | STRAIN | GOODS  | GSI    | N/A  || false | false | false | true  | false || No HGC component based status checks due to component type not for CEMS.
        /// |  7 | true  | true  | false |  88  |    1.34E-1 | HGC   | GOODC  | GCI    | HG     | GOODS  | GSI    | N/A  || true  | true  | false | false | true  || No HGC system based status checks due to bad system status.
        /// |  8 | true  | true  | true  |  88  |    1.34E-1 | HGC   | GOODC  | GCI    | HG     | null   | null   | null || true  | true  | false | false | true  || No HGC system based status checks due to null system id.
        /// |  9 | true  | true  | true  |  88  |    1.34E-1 | HGC   | GOODC  | GCI    | N/A    | GOODS  | GSI    | N/A  || false | false | false | true  | false || No HGC component based status checks due to component type not for CEMS.
        /// | 10 | true  | true  | true  |  88  |    1.34E-1 | HCLC  | GOODC  | GCI    | N/A    | GOODS  | GSI    | N/A  || false | false | true  | true  | false || Status checks for HCLC CEMS.
        /// | 11 | true  | true  | true  |  88  |    1.34E-1 | HFC   | GOODC  | GCI    | N/A    | GOODS  | GSI    | N/A  || false | false | true  | true  | false || Status checks for HFC CEMS.
        /// | 12 | true  | true  | false |  88  |    1.34E-1 | HCLC  | GOODC  | GCI    | N/A    | GOODS  | GSI    | N/A  || false | false | true  | false | false || No HCLC system based status checks due to bad system status.
        /// | 13 | true  | true  | false |  88  |    1.34E-1 | HFC   | GOODC  | GCI    | N/A    | GOODS  | GSI    | N/A  || false | false | true  | false | false || No HFC system based status checks due to bad system status.
        /// | 14 | true  | true  | true  |  88  |    1.34E-1 | HCLC  | GOODC  | GCI    | N/A    | null   | null   | null || false | false | true  | false | false || No HCLC system based status checks due to null system id.
        /// | 15 | true  | true  | true  |  88  |    1.34E-1 | HFC   | GOODC  | GCI    | N/A    | null   | null   | null || false | false | true  | false | false || No HFC system based status checks due to null system id.
        /// | 16 | true  | false | true  |  88  |    1.34E-1 | HCLC  | GOODC  | GCI    | N/A    | GOODS  | GSI    | N/A  || false | false | false | true  | false || No HCLC component based status checks due to bad component status.
        /// | 17 | true  | false | true  |  88  |    1.34E-1 | HFC   | GOODC  | GCI    | N/A    | GOODS  | GSI    | N/A  || false | false | false | true  | false || No HFC component based status checks due to bad component status.
        /// | 18 | true  | true  | true  |  88  |    1.34E-1 | HCLC  | null   | null   | null   | GOODS  | GSI    | N/A  || false | false | false | true  | false || No HCLC component based status checks due to null component id.
        /// | 19 | true  | true  | true  |  88  |    1.34E-1 | HFC   | null   | null   | null   | GOODS  | GSI    | N/A  || false | false | false | true  | false || No HFC component based status checks due to null component id.
        /// </summary>
        [TestMethod()]
        public void MatsMhv13()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            /* Test Case Count */
            int caseCount = 20;

            /* Input Parameter Values */
            bool?[] modcStatusList = { false, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true };
            bool?[] compStatusList = { true, true, true, true, false, true, true, true, true, true, true, true, true, true, true, true, false, false, true, true };
            bool?[] sysStatusList = { true, true, true, true, true, true, true, false, true, true, true, true, false, false, true, true, true, true, true, true };
            string[] modcList = { "88", "80", "88", "88", "88", "88", "88", "88", "88", "88", "88", "88", "88", "88", "88", "88", "88", "88", "88", "88" };
            string[] unadjustedList = { "1.34E-1", "1.34E-1", null, "1.34E-1", "1.34E-1", "1.34E-1", "1.34E-1", "1.34E-1", "1.34E-1", "1.34E-1", "1.34E-1", "1.34E-1", "1.34E-1", "1.34E-1", "1.34E-1", "1.34E-1", "1.34E-1", "1.34E-1", "1.34E-1", "1.34E-1" };
            string[] paramList = { "HGC", "HGC", "HGC", "HGC", "HGC", "HGC", "HGC", "HGC", "HGC", "HGC", "HCLC", "HFC", "HCLC", "HFC", "HCLC", "HFC", "HCLC", "HFC", "HCLC", "HFC" };
            string[] compIdList = { "GOODC", "GOODC", "GOODC", "GOODC", "GOODC", null, "GOODC", "GOODC", "GOODC", "GOODC", "GOODC", "GOODC", "GOODC", "GOODC", "GOODC", "GOODC", "GOODC", "GOODC", null, null };
            string[] compIdentList = { "GCI", "GCI", "GCI", "GCI", "GCI", null, "GCI", "GCI", "GCI", "GCI", "GCI", "GCI", "GCI", "GCI", "GCI", "GCI", "GCI", "GCI", null, null };
            string[] compTypeList = { "HG", "HG", "HG", "HG", "HG", null, "STRAIN", "HG", "HG", "N/A", "N/A", "N/A", "N/A", "N/A", "N/A", "N/A", "N/A", "N/A", null, null };
            string[] sysIdList = { "GOODS", "GOODS", "GOODS", "GOODS", "GOODS", "GOODS", "GOODS", "GOODS", null, "GOODS", "GOODS", "GOODS", "GOODS", "GOODS", null, null, "GOODS", "GOODS", "GOODS", "GOODS" };
            string[] sysIdentList = { "GSI", "GSI", "GSI", "GSI", "GSI", "GSI", "GSI", "GSI", null, "GSI", "GSI", "GSI", "GSI", "GSI", null, null, "GSI", "GSI", "GSI", "GSI" };
            string[] sysTypeList = { "N/A", "N/A", "N/A", "N/A", "N/A", "N/A", "N/A", "N/A", null, "N/A", "N/A", "N/A", "N/A", "N/A", null, null, "N/A", "N/A", "N/A", "N/A" };

            DateTime compBeginDatehour = new DateTime(2016, 6, 17, 22, 0, 0);
            DateTime currentDatehour = compBeginDatehour.AddHours(1);
            string matsMhvMeasuredModcList = "86,88,89";
            string sysDesignationCd = "P";

            /* Expected Values */
            bool?[] dcReqList = { false, false, false, true, false, false, false, true, true, false, false, false, false, false, false, false, false, false, false, false };
            bool?[] lineReqList = { false, false, false, true, false, false, false, true, true, false, false, false, false, false, false, false, false, false, false, false };
            bool?[] qgaReqList = { false, false, false, false, false, false, false, false, false, false, true, true, true, true, true, true, false, false, false, false };
            bool?[] rataReqList = { false, false, false, true, true, true, true, false, false, true, true, true, false, false, false, false, true, true, true, true };
            bool?[] wsiReqList = { false, false, false, true, false, false, false, true, true, false, false, false, false, false, false, false, false, false, false, false };

            /* Check array lengths */
            Assert.AreEqual(caseCount, modcStatusList.Length, "modcStatusList length");
            Assert.AreEqual(caseCount, compStatusList.Length, "compStatusList length");
            Assert.AreEqual(caseCount, sysStatusList.Length, "sysStatusList length");
            Assert.AreEqual(caseCount, modcList.Length, "modcList length");
            Assert.AreEqual(caseCount, unadjustedList.Length, "unadjustedList length");
            Assert.AreEqual(caseCount, paramList.Length, "paramList length");
            Assert.AreEqual(caseCount, compIdList.Length, "compIdList length");
            Assert.AreEqual(caseCount, compIdentList.Length, "compIdentList length");
            Assert.AreEqual(caseCount, compTypeList.Length, "compTypeList length");
            Assert.AreEqual(caseCount, sysIdList.Length, "sysIdList length");
            Assert.AreEqual(caseCount, sysIdentList.Length, "sysIdentList length");
            Assert.AreEqual(caseCount, sysTypeList.Length, "sysTypeList length");
            Assert.AreEqual(caseCount, dcReqList.Length, "dcReqList length");
            Assert.AreEqual(caseCount, lineReqList.Length, "lineReqList length");
            Assert.AreEqual(caseCount, qgaReqList.Length, "qgaReqList length");
            Assert.AreEqual(caseCount, rataReqList.Length, "rataReqList length");
            Assert.AreEqual(caseCount, wsiReqList.Length, "wsiReqList length");

            /* Run Cases */
            for (int caseDex = 0; caseDex < caseCount; caseDex++)
            {
                /* Initialize Input Parameters */
                EmParameters.CurrentDateHour = currentDatehour;
                EmParameters.EmLocationProgramRecords = new CheckDataView<VwMpLocationProgramRow>();
                EmParameters.MatsMhvMeasuredModcList = matsMhvMeasuredModcList;
                EmParameters.MatsMhvRecord
                  = new MATSMonitorHourlyValueData
                  (
                      modcCd: modcList[caseDex], unadjustedHrlyValue: unadjustedList[caseDex], parameterCd: paramList[caseDex],
                      componentId: compIdList[caseDex], componentIdentifier: compIdentList[caseDex], componentTypeCd: compTypeList[caseDex], 
                      componentBeginDatehour: compBeginDatehour, componentBeginDate: compBeginDatehour.Date,
                      monSysId: sysIdList[caseDex], systemIdentifier: sysIdentList[caseDex], sysTypeCd: sysTypeList[caseDex], sysDesignationCd: sysDesignationCd
                  );
                EmParameters.MonitorHourlyModcStatus = modcStatusList[caseDex];
                EmParameters.MonitorHourlyComponentStatus = compStatusList[caseDex];
                EmParameters.MonitorHourlySystemStatus = sysStatusList[caseDex];

                /* Initialize Output Parameters */
                EmParameters.QaStatusComponentBeginDate = null;
                EmParameters.QaStatusComponentId = null;
                EmParameters.QaStatusComponentIdentifier = null;
                EmParameters.QaStatusComponentTypeCode = null;
                EmParameters.QaStatusMatsErbDate = null;
                EmParameters.QaStatusSystemDesignationCode = null;
                EmParameters.QaStatusSystemId = null;
                EmParameters.QaStatusSystemIdentifier = null;
                EmParameters.QaStatusSystemTypeCode = null;

                EmParameters.DailyCalStatusRequired = null;
                EmParameters.LinearityStatusRequired = null;
                EmParameters.QuarterlyGasAuditStatus = null;
                EmParameters.RataStatusRequired = null;
                EmParameters.WsiStatusRequired = null;

                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Initialize variables needed to run the check. */
                bool log = false;
                string actual;

                /* Run Check */
                actual = cMATSMonitorHourlyValueChecks.MATSMHV13(category, ref log);

                /* Check Results */
                Assert.AreEqual(string.Empty, actual, string.Format("actual {0}", caseDex));
                Assert.AreEqual(false, log, string.Format("log {0}", caseDex));
                Assert.AreEqual(null, category.CheckCatalogResult, string.Format("category.CheckCatalogResult {0}", caseDex));

                Assert.AreEqual(compBeginDatehour.Date, EmParameters.QaStatusComponentBeginDate, string.Format("QaStatusComponentBeginDate {0}", caseDex));
                Assert.AreEqual(compBeginDatehour, EmParameters.QaStatusComponentBeginDatehour, string.Format("QaStatusComponentBeginDatehour {0}", caseDex));
                Assert.AreEqual(compIdList[caseDex], EmParameters.QaStatusComponentId, string.Format("QaStatusComponentId {0}", caseDex));
                Assert.AreEqual(compIdentList[caseDex], EmParameters.QaStatusComponentIdentifier, string.Format("QaStatusComponentIdentifier {0}", caseDex));
                Assert.AreEqual(compTypeList[caseDex], EmParameters.QaStatusComponentTypeCode, string.Format("QaStatusComponentTypeCode {0}", caseDex));
                Assert.AreEqual(null, EmParameters.QaStatusMatsErbDate, string.Format("QaStatusMatsErbDate {0}", caseDex));
                Assert.AreEqual(sysDesignationCd, EmParameters.QaStatusSystemDesignationCode, string.Format("QaStatusSystemDesignationCode {0}", caseDex));
                Assert.AreEqual(sysIdList[caseDex], EmParameters.QaStatusSystemId, string.Format("QaStatusSystemId {0}", caseDex));
                Assert.AreEqual(sysIdentList[caseDex], EmParameters.QaStatusSystemIdentifier, string.Format("QaStatusSystemIdentifier {0}", caseDex));
                Assert.AreEqual(sysTypeList[caseDex], EmParameters.QaStatusSystemTypeCode, string.Format("QaStatusSystemTypeCode {0}", caseDex));

                Assert.AreEqual(dcReqList[caseDex], EmParameters.DailyCalStatusRequired, string.Format("DailyCalStatusRequired {0}", caseDex));
                Assert.AreEqual(lineReqList[caseDex], EmParameters.LinearityStatusRequired, string.Format("LinearityStatusRequired {0}", caseDex));
                Assert.AreEqual(qgaReqList[caseDex], EmParameters.QuarterlyGasAuditStatus, string.Format("QuarterlyGasAuditStatus {0}", caseDex));
                Assert.AreEqual(rataReqList[caseDex], EmParameters.RataStatusRequired, string.Format("RataStatusRequired {0}", caseDex));
                Assert.AreEqual(wsiReqList[caseDex], EmParameters.WsiStatusRequired, string.Format("WsiStatusRequired {0}", caseDex));
            }
        }

        /// <summary>
        /// 
        /// 
        /// Test Notes:
        /// 
        /// * Ensure that MonitorHourlyModcStatus is false.  The status required values are tested elsewhere.
        /// 
        /// 
        /// CurrDt : 2016-06-17
        /// CurrHr : 2016-06-17 22
        /// 
        /// |            |           - Program Record 1 -           |           - Program Record 2 -           ||            || 
        /// | ## | Param | PrgCd | UmcbDt    | ErbDt     | PrgEnd   | PrgCd | UmcbDt    | ErbDt     | PrgEnd   || ErbDate    || Note
        /// |  0 | HGC   | MATS  | CurrDt-2  | CurrDt-1  | null     | null  | null      | null      | null     || CurrDt-1   || HGC system with active MATS program and an ERBD before the current date.
        /// |  1 | HCLC  | MATS  | CurrDt-2  | CurrDt-1  | null     | null  | null      | null      | null     || CurrDt-1   || HCLC system with active MATS program and an ERBD before the current date.
        /// |  2 | HFC   | MATS  | CurrDt-2  | CurrDt-1  | null     | null  | null      | null      | null     || CurrDt-1   || HFC system with active MATS program and an ERBD before the current date.
        /// |  3 | SO2C  | MATS  | CurrDt-2  | CurrDt-1  | null     | null  | null      | null      | null     || null       || SO2C is not directly a MATS system, so ERBD is not loaded.
        /// |  4 | HGC   | MATS  | CurrDt-2  | CurrDt    | null     | null  | null      | null      | null     || CurrDt     || MATS program exists with ERBD on current date.
        /// |  5 | HGC   | MATS  | CurrDt-2  | CurrDt-1  | CurrDt   | null  | null      | null      | null     || CurrDt-1   || MATS program exists with End Date on current date.
        /// |  6 | HGC   | MATS  | CurrDt-2  | CurrDt+1  | null     | null  | null      | null      | null     || null       || MATS program exists, but ERBD is after current date.
        /// |  7 | HGC   | MATS  | CurrDt-2  | CurrDt-2  | CurrDt-1 | null  | null      | null      | null     || null       || MATS program exists, but end date occurs before current date.
        /// |  8 | HGC   | NOTM  | CurrDt-2  | CurrDt-1  | null     | null  | null      | null      | null     || null       || MATS program does not exist.
        /// |  9 | HGC   | MATS  | CurrDt-2  | CurrDt-1  | CurrDt+1 | null  | null      | null      | null     || CurrDt-1   || MATS program exists with ERBD before current date and end date after current date.
        /// | 10 | HGC   | MATS  | CurrDt-1  | CurrDt-2  | CurrDt+1 | null  | null      | null      | null     || CurrDt-2   || Ensure that UMCB is not used.
        /// | 11 | HGC   | MATS  | CurrDt-3  | CurrDt-2  | null     | MATS  | CurrDt-4  | CurrDt-1  | null     || CurrDt-2   || Ensure that earlier ERB is used if more than one active MATS program exists.
        /// </summary>
        [TestMethod()]
        public void MatsMhv13_ErbDate()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            /* Test Case Count */
            int caseCount = 12;

            /* Input Parameter Values */
            DateTime currHr = new DateTime(2016, 6, 17, 22, 0, 0);
            DateTime currDt = currHr.Date;
            Dictionary<int, DateTime> dt = new Dictionary<int, DateTime>();
            {
                dt[-4] = currDt.AddDays(-4);
                dt[-3] = currDt.AddDays(-3);
                dt[-2] = currDt.AddDays(-2);
                dt[-1] = currDt.AddDays(-1);
                dt[1] = currDt.AddDays(1);
                dt[2] = currDt.AddDays(2);
            }
            string matsMhvMeasuredModcList = "86,88,89";

            string[] paramList = { "HGC", "HCLC", "HFC", "SO2C", "HGC", "HGC", "HGC", "HGC", "HGC", "HGC", "HGC", "HGC" };
            string[] prg1CodeList = {"MATS", "MATS", "MATS", "MATS", "MATS", "MATS", "MATS", "MATS", "ARP", "MATS", "MATS", "MATS" };
            DateTime?[] prg1UmcbList = { dt[-2], dt[-2], dt[-2], dt[-2], dt[-2], dt[-2], dt[-2], dt[-2], dt[-2], dt[-2], dt[-1], dt[-3] };
            DateTime?[] prg1ErbList = { dt[-1], dt[-1], dt[-1], dt[-1], currDt, dt[-1], dt[1], dt[-2], dt[-1], dt[-1], dt[-2], dt[-2] };
            DateTime?[] prg1EndList = {null, null, null, null, null, currDt, null, dt[-1], null, dt[1], dt[1], null };
            string[] prg2CodeList = { null, null, null, null, null, null, null, null, null, null, null, "MATS" };
            DateTime?[] prg2UmcbList = { null, null, null, null, null, null, null, null, null, null, null, dt[-4] };
            DateTime?[] prg2ErbList = { null, null, null, null, null, null, null, null, null, null, null, dt[-1] };
            DateTime?[] prg2EndList = { null, null, null, null, null, null, null, null, null, null, null, null };

            /* Expected Values */
            DateTime?[] expErbList = { dt[-1], dt[-1], dt[-1], null, currDt, dt[-1], null, null, null, dt[-1], dt[-2], dt[-2] };

            /* Check array lengths */
            Assert.AreEqual(caseCount, paramList.Length, "paramList length");
            Assert.AreEqual(caseCount, prg1CodeList.Length, "prg1CodeList length");
            Assert.AreEqual(caseCount, prg1UmcbList.Length, "prg1UmcbList length");
            Assert.AreEqual(caseCount, prg1ErbList.Length, "prg1ErbList length");
            Assert.AreEqual(caseCount, prg1EndList.Length, "prg1EndList length");
            Assert.AreEqual(caseCount, prg2CodeList.Length, "prg2CodeList length");
            Assert.AreEqual(caseCount, prg2UmcbList.Length, "prg2UmcbList length");
            Assert.AreEqual(caseCount, prg2ErbList.Length, "prg2ErbList length");
            Assert.AreEqual(caseCount, prg2EndList.Length, "prg2EndList length");
            Assert.AreEqual(caseCount, expErbList.Length, "expErbList length");

            /* Run Cases */
            for (int caseDex = 0; caseDex < caseCount; caseDex++)
            {
                /* Initialize Input Parameters */
                EmParameters.CurrentDateHour = currHr;
                EmParameters.EmLocationProgramRecords = new CheckDataView<VwMpLocationProgramRow>();
                {
                    EmParameters.EmLocationProgramRecords.Add(new VwMpLocationProgramRow(prgCd: "ARP", emissionsRecordingBeginDate: dt[-4]));

                    if (prg1CodeList[caseDex] != null)
                        EmParameters.EmLocationProgramRecords.Add(new VwMpLocationProgramRow(prgCd: prg1CodeList[caseDex], unitMonitorCertBeginDate: prg1UmcbList[caseDex], emissionsRecordingBeginDate: prg1ErbList[caseDex], endDate: prg1EndList[caseDex]));

                    if (prg2CodeList[caseDex] != null)
                        EmParameters.EmLocationProgramRecords.Add(new VwMpLocationProgramRow(prgCd: prg2CodeList[caseDex], unitMonitorCertBeginDate: prg2UmcbList[caseDex], emissionsRecordingBeginDate: prg2ErbList[caseDex], endDate: prg2EndList[caseDex]));

                    EmParameters.EmLocationProgramRecords.Add(new VwMpLocationProgramRow(prgCd: "TRNOX", emissionsRecordingBeginDate: dt[-4]));
                }
                EmParameters.MatsMhvMeasuredModcList = matsMhvMeasuredModcList;
                EmParameters.MatsMhvRecord = new MATSMonitorHourlyValueData(parameterCd: paramList[caseDex]);
                EmParameters.MonitorHourlyModcStatus = false;
                EmParameters.MonitorHourlyComponentStatus = false;
                EmParameters.MonitorHourlySystemStatus = false;

                /* Initialize Output Parameters */
                EmParameters.QaStatusComponentBeginDate = null;
                EmParameters.QaStatusComponentId = null;
                EmParameters.QaStatusComponentIdentifier = null;
                EmParameters.QaStatusComponentTypeCode = null;
                EmParameters.QaStatusMatsErbDate = null;
                EmParameters.QaStatusSystemDesignationCode = null;
                EmParameters.QaStatusSystemId = null;
                EmParameters.QaStatusSystemIdentifier = null;
                EmParameters.QaStatusSystemTypeCode = null;

                EmParameters.DailyCalStatusRequired = null;
                EmParameters.LinearityStatusRequired = null;
                EmParameters.QuarterlyGasAuditStatus = null;
                EmParameters.RataStatusRequired = null;
                EmParameters.WsiStatusRequired = null;

                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Initialize variables needed to run the check. */
                bool log = false;
                string actual;

                /* Run Check */
                actual = cMATSMonitorHourlyValueChecks.MATSMHV13(category, ref log);

                /* Check Results */
                Assert.AreEqual(string.Empty, actual, string.Format("actual {0}", caseDex));
                Assert.AreEqual(false, log, string.Format("log {0}", caseDex));
                Assert.AreEqual(null, category.CheckCatalogResult, string.Format("category.CheckCatalogResult {0}", caseDex));

                Assert.AreEqual(expErbList[caseDex], EmParameters.QaStatusMatsErbDate, string.Format("QaStatusMatsErbDate {0}", caseDex));

                /* Not Testing Here */
                Assert.AreEqual(null, EmParameters.QaStatusComponentBeginDate, string.Format("QaStatusComponentBeginDate {0}", caseDex));
                Assert.AreEqual(null, EmParameters.QaStatusComponentBeginDatehour, string.Format("QaStatusComponentBeginDatehour {0}", caseDex));
                Assert.AreEqual(null, EmParameters.QaStatusComponentId, string.Format("QaStatusComponentId {0}", caseDex));
                Assert.AreEqual(null, EmParameters.QaStatusComponentIdentifier, string.Format("QaStatusComponentIdentifier {0}", caseDex));
                Assert.AreEqual(null, EmParameters.QaStatusComponentTypeCode, string.Format("QaStatusComponentTypeCode {0}", caseDex));
                Assert.AreEqual(null, EmParameters.QaStatusSystemDesignationCode, string.Format("QaStatusSystemDesignationCode {0}", caseDex));
                Assert.AreEqual(null, EmParameters.QaStatusSystemId, string.Format("QaStatusSystemId {0}", caseDex));
                Assert.AreEqual(null, EmParameters.QaStatusSystemIdentifier, string.Format("QaStatusSystemIdentifier {0}", caseDex));
                Assert.AreEqual(null, EmParameters.QaStatusSystemTypeCode, string.Format("QaStatusSystemTypeCode {0}", caseDex));

                /* Not Testing Here */
                Assert.AreEqual(false, EmParameters.DailyCalStatusRequired, string.Format("DailyCalStatusRequired {0}", caseDex));
                Assert.AreEqual(false, EmParameters.LinearityStatusRequired, string.Format("LinearityStatusRequired {0}", caseDex));
                Assert.AreEqual(false, EmParameters.QuarterlyGasAuditStatus, string.Format("QuarterlyGasAuditStatus {0}", caseDex));
                Assert.AreEqual(false, EmParameters.RataStatusRequired, string.Format("RataStatusRequired {0}", caseDex));
                Assert.AreEqual(false, EmParameters.WsiStatusRequired, string.Format("WsiStatusRequired {0}", caseDex));
            }
        }

        #endregion

        #region MATSMHV-14

        /// <summary>
        ///A test for MATSMHV-14
        ///</summary>()
        [TestMethod()]
        public void MATSMHV14()
        {
            //static check setup
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            // Variables
            bool log = false;
            string actual;

            // Init Input
            EmParameters.MatsMhvCalculatedValue = "0.00E0";

            // Init Output
            category.CheckCatalogResult = null;

            // Run Checks
            actual = cMATSMonitorHourlyValueChecks.MATSMHV14(category, ref log);

            // Check Results
            Assert.AreEqual(false, log);
            Assert.AreEqual(null, category.CheckCatalogResult, "Result");
            Assert.AreEqual("0.00E0", EmParameters.MatsMhvCalculatedHgcValue, "MatsMhvCalculatedHgcValue");
        }
        #endregion

        #region MATSMHV-15

        /// <summary>
        ///A test for MATSMHV-15
        ///</summary>()
        [TestMethod()]
        public void MATSMHV15()
        {
            //static check setup
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            // Variables
            bool log = false;
            string actual;

            // Init Input
            EmParameters.MatsMhvCalculatedValue = "0.00E0";

            // Init Output
            category.CheckCatalogResult = null;

            // Run Checks
            actual = cMATSMonitorHourlyValueChecks.MATSMHV15(category, ref log);

            // Check Results
            Assert.AreEqual(false, log);
            Assert.AreEqual(null, category.CheckCatalogResult, "Result");
            Assert.AreEqual("0.00E0", EmParameters.MatsMhvCalculatedHclcValue, "MatsMhvCalculatedHclcValue");
        }
        #endregion

        #region MATSMHV-16

        /// <summary>
        ///A test for MATSMHV-16
        ///</summary>()
        [TestMethod()]
        public void MATSMHV16()
        {
            //static check setup
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            // Variables
            bool log = false;
            string actual;

            // Init Input
            EmParameters.MatsMhvCalculatedValue = "0.00E0";

            // Init Output
            category.CheckCatalogResult = null;

            // Run Checks
            actual = cMATSMonitorHourlyValueChecks.MATSMHV16(category, ref log);

            // Check Results
            Assert.AreEqual(false, log);
            Assert.AreEqual(null, category.CheckCatalogResult, "Result");
            Assert.AreEqual("0.00E0", EmParameters.MatsMhvCalculatedHfcValue, "MatsMhvCalculatedHfcValue");

        }
        #endregion


        #region MatsMhv 19 and 20

        private enum eMatsHgLinearityLikeCertificationChecks {  MatsMhv19, MatsMhv20 };

        [TestMethod()]
        public void MatsMhv19()
        {
            MatsHgLinearityLikeCertificationCases(eMatsHgLinearityLikeCertificationChecks.MatsMhv19);
        }

        [TestMethod()]
        public void MatsMhv20()
        {
            MatsHgLinearityLikeCertificationCases(eMatsHgLinearityLikeCertificationChecks.MatsMhv20);
        }


        /// <summary>
        /// A test for MATSMHV-19
        ///
        /// Note that a test system integrity row is only created if one of the two si values is not null, 
        /// and a test QA supp data is only created if one of the three ce values is not null.
        /// 
        /// Case Notes:
        /// 
        /// * Cases assume the the StatusDeterminination ConditionalDataEvent120ForMats and ConditionalDataEvent125ForMats methods
        ///   a fully tested seperately.
        /// 
        /// currHr    : 2015-06-17 22
        /// compBegin : 2015-04-01 00
        /// umcb      : 2015-03-31
        /// umcd      : 2015-04-01
        /// erb       : 2015-04-01
        /// 
        /// testNum | wsiStatReq | mmCompIdent  | siCompId  | siEndHour    | siEndMin | siResult | siNeedsEval | ceCompId  | ceCode | ceHour       | ceCondHour   || expResult | expEvent || Note  
        ///       0 | false      | "NONLK"      | null      | null         | null     | null     | null        | null      | null   | null         | null         || null      | false    || WSI Status is not required therefore check body does not run.
        ///       1 | true       | "LKCMP"      | "GoodId"  | null         | null     | PASSED   | "N"         | null      | null   | null         | null         || null      | false    || Like-kind component, therefore check body does not run.
        ///       2 | true       | "NONLK"      | null      | null         | null     | null     | null        | null      | null   | null         | null         || "C"       | false    || No WSI nor Event.
        ///       3 | true       | "NONLK"      | "OtherId" | currHr       | 00       | PASSED   | "N"         | null      | null   | null         | null         || "C"       | false    || No Event and existing WSI for another component.
        ///       4 | true       | "NONLK"      | "GoodId"  | currHr + 1   | 00       | PASSED   | "N"         | null      | null   | null         | null         || "C"       | false    || No Event and existing WSI occured after the current hour.
        ///       5 | true       | "NONLK"      | "GoodId"  | currHr       | 00       | PASSED   | "N"         | null      | null   | null         | null         || null      | false    || WSI exists, but no Event exists.
        ///       6 | true       | "NONLK"      | null      | null         | null     | null     | null        | "OtherId" | "120"  | currHr - 169 | currHr - 168 || "C"       | false    || No WSI and existing Event for another component.
        ///       7 | true       | "NONLK"      | null      | null         | null     | null     | null        | "GoodId"  | "123"  | currHr - 169 | currHr - 168 || "C"       | false    || No WSI and existing Event not for 125 or 120.
        ///       8 | true       | "NONLK"      | null      | null         | null     | null     | null        | "GoodId"  | "125"  | currHr       | null         || "C"       | false    || No WSI and existing Event occured on or after the current hour.
        ///       9 | true       | "NONLK"      | "GoodId"  | currHr - 170 | 00       | PASSED   | "N"         | "OtherId" | "120"  | currHr - 169 | currHr - 168 || null      | false    || WSI exists, but existing Event for another component.
        ///      10 | true       | "NONLK"      | "GoodId"  | currHr - 170 | 00       | PASSED   | "N"         | "GoodId"  | "123"  | currHr - 169 | currHr - 168 || null      | false    || WSI exists, but existing Event not for 125 or 120.
        ///      11 | true       | "NONLK"      | "GoodId"  | currHr - 170 | 00       | PASSED   | "N"         | "GoodId"  | "125"  | currHr       | null         || null      | false    || WSI exists, but existing Event occured on or after the current hour.
        ///      12 | true       | "NONLK"      | "GoodId"  | currHr - 170 | 00       | PASSED   | "N"         | "GoodId"  | "125"  | currHr - 170 | currHr - 168 || null      | false    || WSI exists, but event date is not greater than WSI end date.
        ///      13 | true       | "NONLK"      | null      | null         | null     | null     | null        | "GoodId"  | "120"  | currHr - 169 | currHr - 168 || "A"       | true     || No WSI, but existing 120 event with more than 168 operating hours.
        ///      14 | true       | "NONLK"      | "GoodId"  | currHr - 170 | 00       | PASSED   | "N"         | "GoodId"  | "120"  | currHr - 169 | currHr - 168 || "B"       | true     || Existing WSI and 120 event with more than 168 operating hours.
        ///      15 | true       | "NONLK"      | null      | null         | null     | null     | null        | "GoodId"  | "125"  | currHr - 169 | currHr - 1   || "A"       | true     || No WSI, but expired 125 event exists because UMCD is before current date.
        ///      16 | true       | "NONLK"      | "GoodId"  | currHr - 170 | 00       | PASSED   | "N"         | "GoodId"  | "125"  | currHr - 169 | currHr - 1   || "B"       | true     || Existing WSI and expired 125 event exists because UMCD is before current date.
        ///      17 | true       | "NONLK"      | "GoodId"  | currHr       | 44       | PASSED   | "N"         | null      | null   | null         | null         || null      | false    || No Event and existing WSI occured in the current hour and before the 45 minute.
        ///      18 | true       | "NONLK"      | "GoodId"  | currHr       | 44       | PASSAPS  | "N"         | null      | null   | null         | null         || null      | false    || No Event and existing WSI occured in the current hour and before the 45 minute with PASSAPS.
        ///      19 | true       | "NONLK"      | "GoodId"  | currHr       | 44       | FAILED   | "N"         | null      | null   | null         | null         || "C"       | false    || No Event and existing WSI occured in the current hour and before the 45 minute, but with FAILED.
        ///      20 | true       | "NONLK"      | "GoodId"  | currHr       | 45       | PASSED   | "N"         | null      | null   | null         | null         || "C"       | false    || No Event and existing WSI occured in the current hour and on the 45 minute.
        ///      21 | true       | "NONLK"      | "GoodId"  | currHr       | 46       | PASSED   | "N"         | null      | null   | null         | null         || "C"       | false    || No Event and existing WSI occured in the current hour and after the 45 minute.
        ///      22 | true       | "NONLK"      | "GoodId"  | currHr       | 00       | PASSAPS  | "N"         | null      | null   | null         | null         || null      | false    || WSI exists, but no Event exists.
        ///      23 | true       | "NONLK"      | "GoodId"  | currHr       | 00       | FAILED   | "N"         | null      | null   | null         | null         || "C"       | false    || WSI exists but failed and no Event exists.
        ///      24 | true       | "NONLK"      | "GoodId"  | currHr - 170 | 00       | PASSED   | "N"         | "GoodId"  | "120"  | currHr - 169 | null         || "J"       | true     || Existing WSI, but conditional period begin hour for 120 event is null.
        ///      25 | true       | "NONLK"      | "GoodId"  | currHr - 170 | 00       | PASSED   | "N"         | "GoodId"  | "120"  | currHr - 169 | currHr + 1   || "J"       | true     || Existing WSI, but conditional period begin hour for 120 event began after current hour.
        ///      26 | true       | "NONLK"      | "GoodId"  | currHr - 170 | 00       | PASSED   | "N"         | "GoodId"  | "125"  | currHr - 169 | null         || "J"       | true     || Existing WSI, but conditional period begin hour for 125 event is null.
        ///      27 | true       | "NONLK"      | "GoodId"  | currHr - 170 | 00       | PASSED   | "N"         | "GoodId"  | "125"  | currHr - 169 | currHr + 1   || "J"       | true     || Existing WSI, but conditional period begin hour for 125 event began after current hour.
        ///      28 | true       | "NONLK"      | null      | null         | null     | null     | null        | "GoodId"  | "120"  | currHr       | currHr + 1   || "C"       | false    || No WSI and existing 120 Event began on the current hour, but condition period began after the current hour.
        ///      29 | true       | "NONLK"      | null      | null         | null     | null     | null        | "GoodId"  | "125"  | currHr       | currHr + 1   || "C"       | false    || No WSI and existing 125 Event began on the current hour, but condition period began after the current hour.
        ///      30 | true       | "NONLK"      | null      | null         | null     | null     | null        | "GoodId"  | "120"  | currHr       | currHr       || null      | true     || No WSI and existing 120 Event and its conditional data period began on the current hour.
        ///      31 | true       | "NONLK"      | null      | null         | null     | null     | null        | "GoodId"  | "125"  | currHr       | currHr       || "A"       | true     || No WSI and existing 125 Event and its conditional data period began on the current hour, but conditional data period has expired because UMCD is before current date..
        ///      32 | true       | "NONLK"      | "GoodId"  | currHr       | 00       | PASSED   | "Y"         | null      | null   | null         | null         || "K"       | false    || WSI exists with no Event, but the test needs to be evaluated.
        ///      33 | true       | "NONLK"      | null      | null         | null     | null     | null        | "GoodId"  | "100"  | currHr - 169 | currHr - 168 || "A"       | true     || No WSI, but existing 120 event with more than 168 operating hours.
        ///      34 | true       | "NONLK"      | "GoodId"  | currHr - 170 | 00       | PASSED   | "N"         | "GoodId"  | "100"  | currHr - 169 | currHr - 168 || "B"       | true     || Existing WSI and 120 event with more than 168 operating hours.
        ///      35 | true       | "NONLK"      | null      | null         | null     | null     | null        | "OtherId" | "100"  | currHr - 169 | currHr - 168 || "C"       | false    || No WSI and existing Event for another component.
        ///      36 | true       | "NONLK"      | "GoodId"  | currHr - 170 | 00       | PASSED   | "N"         | "OtherId" | "100"  | currHr - 169 | currHr - 168 || null      | false    || WSI exists, but existing Event for another component.
        ///      37 | true       | "NONLK"      | "GoodId"  | currHr - 170 | 00       | PASSED   | "N"         | "GoodId"  | "100"  | currHr - 169 | null         || "J"       | true     || Existing WSI, but conditional period begin hour for 120 event is null.
        ///      38 | true       | "NONLK"      | "GoodId"  | currHr - 170 | 00       | PASSED   | "N"         | "GoodId"  | "100"  | currHr - 169 | currHr + 1   || "J"       | true     || Existing WSI, but conditional period begin hour for 120 event began after current hour.
        ///      39 | true       | "NONLK"      | null      | null         | null     | null     | null        | "GoodId"  | "100"  | currHr       | currHr + 1   || "C"       | false    || No WSI and existing 120 Event began on the current hour, but condition period began after the current hour.
        ///      40 | true       | "NONLK"      | null      | null         | null     | null     | null        | "GoodId"  | "100"  | currHr       | currHr       || null      | true     || No WSI and existing 120 Event and its conditional data period began on the current hour.
        ///      41 | true       | "NONLK"      | null      | null         | null     | null     | null        | "GoodId"  | "101"  | currHr - 169 | currHr - 168 || "A"       | true     || No WSI, but existing 120 event with more than 168 operating hours.
        ///      42 | true       | "NONLK"      | "GoodId"  | currHr - 170 | 00       | PASSED   | "N"         | "GoodId"  | "101"  | currHr - 169 | currHr - 168 || "B"       | true     || Existing WSI and 120 event with more than 168 operating hours.
        ///      43 | true       | "NONLK"      | null      | null         | null     | null     | null        | "OtherId" | "101"  | currHr - 169 | currHr - 168 || "C"       | false    || No WSI and existing Event for another component.
        ///      44 | true       | "NONLK"      | "GoodId"  | currHr - 170 | 00       | PASSED   | "N"         | "OtherId" | "101"  | currHr - 169 | currHr - 168 || null      | false    || WSI exists, but existing Event for another component.
        ///      45 | true       | "NONLK"      | "GoodId"  | currHr - 170 | 00       | PASSED   | "N"         | "GoodId"  | "101"  | currHr - 169 | null         || "J"       | true     || Existing WSI, but conditional period begin hour for 120 event is null.
        ///      46 | true       | "NONLK"      | "GoodId"  | currHr - 170 | 00       | PASSED   | "N"         | "GoodId"  | "101"  | currHr - 169 | currHr + 1   || "J"       | true     || Existing WSI, but conditional period begin hour for 120 event began after current hour.
        ///      47 | true       | "NONLK"      | null      | null         | null     | null     | null        | "GoodId"  | "101"  | currHr       | currHr + 1   || "C"       | false    || No WSI and existing 120 Event began on the current hour, but condition period began after the current hour.
        ///      48 | true       | "NONLK"      | null      | null         | null     | null     | null        | "GoodId"  | "101"  | currHr       | currHr       || null      | true     || No WSI and existing 120 Event and its conditional data period began on the current hour.
        /// 
        ///</summary>()
        private void MatsHgLinearityLikeCertificationCases(eMatsHgLinearityLikeCertificationChecks check)
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            /* Initialize General Variables */
            string compGoodId = "GoodId";
            string compOtherId = "OtherId";
            DateTime compBegin = new DateTime(2015, 3, 31, 23, 0, 0);
            DateTime currentDatehour = new DateTime(2015, 6, 17, 22, 0, 0);
            DateTime currentDate = currentDatehour.Date;
            int currentHour = currentDatehour.Hour;
            DateTime afterEndDatehour = currentDatehour.AddHours(1);

            DateTime umcbDate = new DateTime(2015, 3, 31);
            DateTime umcdDate = new DateTime(2015, 4, 1);
            DateTime erbDate = new DateTime(2015, 4, 1);

            /* Input Parameter Values */
            DateTime qtrBHr = new DateTime(2015, 4, 1, 0, 0, 0);
            DateTime qtrEHr = new DateTime(2015, 6, 30, 23, 0, 0);

            bool[] wsiStatReqList = { false, true, true, true, true, true, true, true, true, true
                                    , true , true, true, true, true, true, true, true, true, true
                                    , true , true, true, true, true, true, true, true, true, true
                                    , true , true, true, true, true, true, true, true, true, true
                                    , true , true, true, true, true, true, true, true, true };
            string[] mmComponentIdent = { "NONLK", "LKCMP", "NONLK", "NONLK", "NONLK", "NONLK", "NONLK", "NONLK", "NONLK", "NONLK"
                                        , "NONLK", "NONLK", "NONLK", "NONLK", "NONLK", "NONLK", "NONLK", "NONLK", "NONLK", "NONLK"
                                        , "NONLK", "NONLK", "NONLK", "NONLK", "NONLK", "NONLK", "NONLK", "NONLK", "NONLK", "NONLK"
                                        , "NONLK", "NONLK", "NONLK", "NONLK", "NONLK", "NONLK", "NONLK", "NONLK", "NONLK", "NONLK"
                                        , "NONLK", "NONLK", "NONLK", "NONLK", "NONLK", "NONLK", "NONLK", "NONLK", "NONLK" };
            string[] siComponentIdList = { null, null, null, compOtherId, compGoodId, compGoodId, null, null, null, compGoodId
                                         , compGoodId, compGoodId, compGoodId, null, compGoodId, null, compGoodId, compGoodId, compGoodId, compGoodId
                                         , compGoodId, compGoodId, compGoodId, compGoodId, compGoodId, compGoodId, compGoodId, compGoodId, null, null
                                         , null, null, compGoodId, null, compGoodId, null, compGoodId, compGoodId, compGoodId, null
                                         , null, null, compGoodId, null, compGoodId, compGoodId, compGoodId, null, null };

            DateTime?[] siEndDatetimeList = { null, null, null, currentDatehour, currentDatehour.AddHours(1), currentDatehour, null, null, null, currentDatehour.AddHours(-170)
                                            , currentDatehour.AddHours(-170), currentDatehour.AddHours(-170), currentDatehour.AddHours(-170), null, currentDatehour.AddHours(-170), null, currentDatehour.AddHours(-170), currentDatehour.AddMinutes(44), currentDatehour.AddMinutes(44), currentDatehour.AddMinutes(44)
                                            , currentDatehour.AddMinutes(45), currentDatehour.AddMinutes(46), currentDatehour, currentDatehour, currentDatehour.AddHours(-170), currentDatehour.AddHours(-170), currentDatehour.AddHours(-170), currentDatehour.AddHours(-170), null, null
                                            , null, null, currentDatehour, null, currentDatehour.AddHours(-170), null, currentDatehour.AddHours(-170), currentDatehour.AddHours(-170), currentDatehour.AddHours(-170), null
                                            , null, null, currentDatehour.AddHours(-170), null, currentDatehour.AddHours(-170), currentDatehour.AddHours(-170), currentDatehour.AddHours(-170), null, null };

            string[] siResultList = { null, "PASSED", null, "PASSED", "PASSED", "PASSED", null, null, null, "PASSED"
                                    , "PASSED", "PASSED", "PASSED", null, "PASSED", null, "PASSED", "PASSED", "PASSAPS", "FAILED"
                                    , "PASSED", "PASSED", "PASSAPS", "FAILED", "PASSED", "PASSED", "PASSED", "PASSED", null, null
                                    , null, null, "PASSED", null, "PASSED", null, "PASSED", "PASSED", "PASSED", null
                                    , null, null, "PASSED", null, "PASSED", "PASSED", "PASSED", null, null };

            string[] siNeedsEvalList = { null, "N", null, "N", "N", "N", null, null, null, "N"
                                       , "N", "N", "N", null, "N", null, "N", "N", "N", "N"
                                       , "N", "N", "N", "N", "N", "N", "N", "N", null, null
                                       , null, null, "Y", null, "N", null, "N", "N", "N", null
                                       , null, null, "N", null, "N", "N", "N", null, null };

            string[] ceComponentIdList = { null, null, null, null, null, null, compOtherId, compGoodId, compGoodId, compOtherId
                                         , compGoodId, compGoodId, compGoodId, compGoodId, compGoodId, compGoodId, compGoodId, null, null, null
                                         , null, null, null, null, compGoodId, compGoodId, compGoodId, compGoodId, compGoodId, compGoodId
                                         , compGoodId, compGoodId, null, compGoodId, compGoodId, compOtherId, compOtherId, compGoodId, compGoodId, compGoodId
                                         , compGoodId, compGoodId, compGoodId, compOtherId, compOtherId, compGoodId, compGoodId, compGoodId, compGoodId };

            string[] ceCodeList = { null, null, null, null, null, null, "120", "123", "125", "120"
                                  , "123", "125", "125", "120", "120", "125", "125", null, null, null
                                  , null, null, null, null, "120", "120", "125", "125", "120", "125"
                                  , "120", "125", null, "100", "100", "100", "100", "100", "100", "100"
                                  , "100", "101", "101", "101", "101", "101", "101", "101", "101" };

            DateTime?[] ceDatehourList = { null, null, null, null, null, null, currentDatehour.AddHours(-169), currentDatehour.AddHours(-169), currentDatehour, currentDatehour.AddHours(-169)
                                         , currentDatehour.AddHours(-169), currentDatehour, currentDatehour.AddHours(-170), currentDatehour.AddHours(-169), currentDatehour.AddHours(-169), currentDatehour.AddHours(-169), currentDatehour.AddHours(-169), null, null, null
                                         , null, null, null, null, currentDatehour.AddHours(-169), currentDatehour.AddHours(-169), currentDatehour.AddHours(-169), currentDatehour.AddHours(-169), currentDatehour, currentDatehour
                                         , currentDatehour, currentDatehour, null, currentDatehour.AddHours(-169), currentDatehour.AddHours(-169), currentDatehour.AddHours(-169), currentDatehour.AddHours(-169), currentDatehour.AddHours(-169), currentDatehour.AddHours(-169), currentDatehour
                                         , currentDatehour, currentDatehour.AddHours(-169), currentDatehour.AddHours(-169), currentDatehour.AddHours(-169), currentDatehour.AddHours(-169), currentDatehour.AddHours(-169), currentDatehour.AddHours(-169), currentDatehour, currentDatehour};

            DateTime?[] ceCondHourList = { null, null, null, null, null, null, currentDatehour.AddHours(-168), currentDatehour.AddHours(-168), null, currentDatehour.AddHours(-168)
                                         , currentDatehour.AddHours(-168), null, currentDatehour.AddHours(-168), currentDatehour.AddHours(-168), currentDatehour.AddHours(-168), currentDatehour.AddHours(-1), currentDatehour.AddHours(-1), null, null, null
                                         , null, null, null, null, null, currentDatehour.AddHours(1), null, currentDatehour.AddHours(1), currentDatehour.AddHours(1), currentDatehour.AddHours(1)
                                         , currentDatehour, currentDatehour, null, currentDatehour.AddHours(-168), currentDatehour.AddHours(-168), currentDatehour.AddHours(-168), currentDatehour.AddHours(-168), null, currentDatehour.AddHours(1), currentDatehour.AddHours(1)
                                         , currentDatehour, currentDatehour.AddHours(-168), currentDatehour.AddHours(-168), currentDatehour.AddHours(-168), currentDatehour.AddHours(-168), null, currentDatehour.AddHours(1), currentDatehour.AddHours(1), currentDatehour };

            /* Expected Values */
            string[] expResult = { null, null, "C", "C", "C", null, "C", "C", "C", null
                                 , null, null, null, "A", "B", "A", "B", null, null, "C"
                                 , "C", "C", null, "C", "J", "J", "J", "J", "C", "C"
                                 , null, "A", "K", "A", "B", "C", null, "J", "J", "C"
                                 , null, "A", "B", "C", null, "J", "J", "C", null};

            bool[] expEventList = { false, false, false, false, false, false, false, false, false, false
                                  , false, false, false, true, true, true, true, false, false, false
                                  , false, false, false, false, true, true, true, true, false, false
                                  , true, true, false, true, true, false, false, true, true, false
                                  , true, true, true, false, false, true, true, false, true};

            /* Test Case Count */
            int caseCount = 49; //was 33

            /* Check array lengths */
            Assert.AreEqual(caseCount, wsiStatReqList.Length, "wsiStatReqList length");
            Assert.AreEqual(caseCount, mmComponentIdent.Length, "mmComponentIdent length");
            Assert.AreEqual(caseCount, siComponentIdList.Length, "siComponentIdList length");
            Assert.AreEqual(caseCount, siEndDatetimeList.Length, "siEndDatehourList length");
            Assert.AreEqual(caseCount, siResultList.Length, "siResultList length");
            Assert.AreEqual(caseCount, siNeedsEvalList.Length, "siNeedsEvalList length");
            Assert.AreEqual(caseCount, ceComponentIdList.Length, "ceComponentIdList length");
            Assert.AreEqual(caseCount, ceCodeList.Length, "ceCodeList length");
            Assert.AreEqual(caseCount, ceDatehourList.Length, "ceDatehourList length");
            Assert.AreEqual(caseCount, ceCondHourList.Length, "ceCondHourList length");
            Assert.AreEqual(caseCount, expResult.Length, "expResult length");
            Assert.AreEqual(caseCount, expEventList.Length, "expEventList length");


            for (int caseDex = 0; caseDex < caseCount; caseDex++)
            {
                /* Case Specific Variables */
                DateTime? ceDatehour = ceDatehourList[caseDex];
                DateTime? ceDate = (ceDatehour.HasValue ? ceDatehour.Value.Date : (DateTime?)null);
                int? ceHour = (ceDatehour.HasValue ? ceDatehour.Value.Hour : (int?)null);

                DateTime? ceCondDateHour = ceCondHourList[caseDex];
                DateTime? ceCondDate = (ceCondDateHour.HasValue ? ceCondDateHour.Value.Date : (DateTime?)null);
                int? ceCondHour = (ceCondDateHour.HasValue ? ceCondDateHour.Value.Hour : (int?)null);

                DateTime? siDatetime = siEndDatetimeList[caseDex];
                DateTime? siDate = (siDatetime.HasValue ? siDatetime.Value.Date : (DateTime?)null);
                int? siHour = (siDatetime.HasValue ? siDatetime.Value.Hour : (int?)null);
                int? siMinute = (siDatetime.HasValue ? siDatetime.Value.Minute : (int?)null);
                DateTime? siDatehour = (siDatetime.HasValue ? siDatetime.Value.Date.AddHours(siDatetime.Value.Hour) : (DateTime?)null);

                /*  Initialize Input Parameters*/
                EmParameters.CurrentDateHour = currentDatehour;
                EmParameters.LocationProgramRecordsByHourLocation = new CheckDataView<VwMpLocationProgramRow>();
                {
                    EmParameters.LocationProgramRecordsByHourLocation.Add(new VwMpLocationProgramRow(prgCd: "MATS", unitMonitorCertDeadline: umcdDate, unitMonitorCertBeginDate: umcbDate, emissionsRecordingBeginDate: erbDate));
                }
                EmParameters.HourlyOperatingDataRecordsForLocation = new CheckDataView<VwMpHrlyOpDataRow>();
                {
                    for (DateTime hr = qtrBHr; hr <= qtrEHr; hr = hr.AddHours(1))
                    {
                        EmParameters.HourlyOperatingDataRecordsForLocation.Add(new VwMpHrlyOpDataRow(opTime: 0.1m, beginDatehour: hr, beginDate: hr.Date, beginHour: hr.Hour));
                    }
                }
                EmParameters.QaStatusComponentBeginDate = compBegin.Date;
                EmParameters.QaStatusComponentId = compGoodId;
                EmParameters.QaStatusComponentIdentifier = mmComponentIdent[caseDex];
                EmParameters.WsiStatusRequired = wsiStatReqList[caseDex];

                if ((ceComponentIdList[caseDex] != null) || (ceCodeList[caseDex] != null) || (ceDatehourList[caseDex] != null))
                    EmParameters.QaCertificationEventRecords = new CheckDataView<VwQaCertEventRow>
                    (
                      new VwQaCertEventRow(componentId: compGoodId, qaCertEventCd: "120", qaCertEventDatehour: currentDatehour, qaCertEventDate: currentDate, qaCertEventHour: currentHour),
                      new VwQaCertEventRow(componentId: ceComponentIdList[caseDex], qaCertEventCd: ceCodeList[caseDex], 
                                           qaCertEventDatehour: ceDatehour, qaCertEventDate: ceDate, qaCertEventHour: ceHour,
                                           conditionalDataBeginDatehour: ceCondDateHour, conditionalDataBeginDate: ceCondDate, conditionalDataBeginHour: ceCondHour),
                      new VwQaCertEventRow(componentId: compGoodId, qaCertEventCd: "125", qaCertEventDatehour: currentDatehour, qaCertEventDate: currentDate, qaCertEventHour: currentHour)
                    );
                else
                    EmParameters.QaCertificationEventRecords = new CheckDataView<VwQaCertEventRow>
                    (
                      new VwQaCertEventRow(componentId: compGoodId, qaCertEventCd: "120", qaCertEventDatehour: currentDatehour, qaCertEventDate: currentDate, qaCertEventHour: currentHour),
                      new VwQaCertEventRow(componentId: compGoodId, qaCertEventCd: "125", qaCertEventDatehour: currentDatehour, qaCertEventDate: currentDate, qaCertEventHour: currentHour)
                    );

                // Assign correct MATS Test Records for QA Status parameter
                {
                    CheckDataView<VwQaSuppDataHourlyStatusRow> matsTestRecordsForQaStatus;

                    if ((siComponentIdList[caseDex] != null) || (siEndDatetimeList[caseDex] != null))
                        matsTestRecordsForQaStatus = new CheckDataView<VwQaSuppDataHourlyStatusRow>
                        (
                          new VwQaSuppDataHourlyStatusRow(componentId: compGoodId, calcTestResultCd: "PASSED", endDatehour: afterEndDatehour, endDatetime: afterEndDatehour, endDate: afterEndDatehour.Date, endHour: afterEndDatehour.Hour, endMin: afterEndDatehour.Minute),
                          new VwQaSuppDataHourlyStatusRow(componentId: siComponentIdList[caseDex], testResultCd: siResultList[caseDex], qaNeedsEvalFlg: siNeedsEvalList[caseDex], endDatehour: siDatehour, endDatetime: siDatetime, endDate: siDate, endHour: siHour, endMin: siMinute),
                          new VwQaSuppDataHourlyStatusRow(componentId: compGoodId, calcTestResultCd: "PASSAPS", endDatehour: afterEndDatehour, endDatetime: afterEndDatehour, endDate: afterEndDatehour.Date, endHour: afterEndDatehour.Hour, endMin: afterEndDatehour.Minute)
                        );
                    else
                        matsTestRecordsForQaStatus = new CheckDataView<VwQaSuppDataHourlyStatusRow>
                        (
                          new VwQaSuppDataHourlyStatusRow(componentId: compGoodId, calcTestResultCd: "PASSED", endDatehour: afterEndDatehour, endDatetime: afterEndDatehour, endDate: afterEndDatehour.Date, endHour: afterEndDatehour.Hour, endMin: afterEndDatehour.Minute),
                          new VwQaSuppDataHourlyStatusRow(componentId: compGoodId, calcTestResultCd: "PASSAPS", endDatehour: afterEndDatehour, endDatetime: afterEndDatehour, endDate: afterEndDatehour.Date, endHour: afterEndDatehour.Hour, endMin: afterEndDatehour.Minute)
                        );

                    switch (check)
                    {
                        case eMatsHgLinearityLikeCertificationChecks.MatsMhv19: { EmParameters.Mats3LevelSystemIntegrityRecordsForQaStatus = matsTestRecordsForQaStatus; }; break;
                        case eMatsHgLinearityLikeCertificationChecks.MatsMhv20: { EmParameters.MatsHgLinearityRecordsForQaStatus = matsTestRecordsForQaStatus; }; break;
                    }
                }

                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Initialize variables needed to run the check. */
                bool log = false;
                string actual = null;

                /* Run Check */
                switch (check)
                {
                    case eMatsHgLinearityLikeCertificationChecks.MatsMhv19: { actual = cMATSMonitorHourlyValueChecks.MATSMHV19(category, ref log); }; break;
                    case eMatsHgLinearityLikeCertificationChecks.MatsMhv20: { actual = cMATSMonitorHourlyValueChecks.MATSMHV20(category, ref log); }; break;
                }

                /* Check results */
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);

                Assert.AreEqual(expResult[caseDex], category.CheckCatalogResult, string.Format("Result {0}", caseDex));

                switch (check)
                {
                    case eMatsHgLinearityLikeCertificationChecks.MatsMhv19:
                        {
                            Assert.AreEqual(expEventList[caseDex], EmParameters.MatsHg3LevelSiEventRecord != null, string.Format("MatsHg3LevelSiEventRecord {0}", caseDex));
                            Assert.IsNull(EmParameters.MatsHgLinearityEventRecord, string.Format("MatsHgLinearityEventRecord {0}", caseDex));
                        };
                        break;

                    case eMatsHgLinearityLikeCertificationChecks.MatsMhv20:
                        {
                            Assert.AreEqual(expEventList[caseDex], EmParameters.MatsHgLinearityEventRecord != null, string.Format("MatsHgLinearityEventRecord {0}", caseDex));
                            Assert.IsNull(EmParameters.MatsHg3LevelSiEventRecord, string.Format("MatsHg3LevelSiEventRecord {0}", caseDex));
                        };
                        break;
                }
            }
        }

        #endregion

        #endregion

    }
}