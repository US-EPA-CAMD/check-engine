using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.MonitorPlan;
using ECMPS.Checks.QualificationChecks;

using ECMPS.Definitions.Extensions;

using ECMPS.Checks.Data.Ecmps.Dbo.Table;
using ECMPS.Checks.Data.Ecmps.Dbo.View;
using ECMPS.Checks.Data.EcmpsAux.CrossCheck.Virtual;
using ECMPS.Checks.Data.Ecmps.CrossCheck.Table;
using ECMPS.Checks.Mp.Parameters;

using UnitTest.UtilityClasses;

namespace UnitTest.MonitorPlan
{
    /// <summary>
    ///This is a test class for cQualificationChecksTest and is intended
    ///to contain all cQualificationChecksTest Unit Tests
    /// </summary>
    [TestClass]
    public class cQualificationChecksTest
    {
        public cQualificationChecksTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

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


        /// <summary>
        /// 
        /// |    |         - Qualification -         |         |                   - Program -                  ||        |       || 
        /// | ## | Type    | Begin Date | End Date   | LocType | Program | UmcbDate   | ErbDate    | End Date   || Result | Valid || Note
        /// |  0 | null    | 2016-06-17 | null       | MP      | MATS    | 2016-06-17 | 2016-06-17 | null       || A      | false || Result A due to null qualificaiton type.
        /// |  1 | BAD     | 2016-06-17 | null       | MP      | MATS    | 2016-06-17 | 2016-06-17 | null       || B      | false || Result B due to unknown qualificaiton type.
        /// |  2 | COMPLEX | 2016-06-17 | null       | MP      | MATS    | 2016-06-17 | 2016-06-17 | null       || null   | true  || No result because qual type is not location dependent.
        /// |  3 | LOWSULF | 2016-06-17 | null       | MP      | MATS    | 2016-06-17 | 2016-06-17 | null       || null   | true  || No result because qual type is not location dependent.
        /// |  4 | PRATA1  | 2016-06-17 | null       | MP      | MATS    | 2016-06-17 | 2016-06-17 | null       || null   | true  || No result because qual type is not location dependent.
        /// |  5 | PRATA2  | 2016-06-17 | null       | MP      | MATS    | 2016-06-17 | 2016-06-17 | null       || null   | true  || No result because qual type is not location dependent.
        /// |  6 | GF      | 2016-06-17 | null       | UB      | MATS    | 2016-06-17 | 2016-06-17 | null       || null   | true  || No result because qual type is unit specific and location type begins with 'U'.
        /// |  7 | GF      | 2016-06-17 | null       | UP      | MATS    | 2016-06-17 | 2016-06-17 | null       || null   | true  || No result because qual type is unit specific and location type begins with 'U'.
        /// |  8 | GF      | 2016-06-17 | null       | US      | MATS    | 2016-06-17 | 2016-06-17 | null       || null   | true  || No result because qual type is unit specific and location type begins with 'U'.
        /// |  9 | GF      | 2016-06-17 | null       | U       | MATS    | 2016-06-17 | 2016-06-17 | null       || null   | true  || No result because qual type is unit specific and location type begins with 'U'.
        /// | 10 | HGAVG   | 2016-06-17 | null       | U       | MATS    | 2016-06-17 | 2016-06-17 | null       || null   | true  || No result because qual type is unit specific and location type begins with 'U'.
        /// | 11 | LMEA    | 2016-06-17 | null       | U       | MATS    | 2016-06-17 | 2016-06-17 | null       || null   | true  || No result because qual type is unit specific and location type begins with 'U'.
        /// | 12 | LMES    | 2016-06-17 | null       | U       | MATS    | 2016-06-17 | 2016-06-17 | null       || null   | true  || No result because qual type is unit specific and location type begins with 'U'.
        /// | 13 | PK      | 2016-06-17 | null       | U       | MATS    | 2016-06-17 | 2016-06-17 | null       || null   | true  || No result because qual type is unit specific and location type begins with 'U'.
        /// | 14 | SK      | 2016-06-17 | null       | U       | MATS    | 2016-06-17 | 2016-06-17 | null       || null   | true  || No result because qual type is unit specific and location type begins with 'U'.
        /// | 15 | GF      | 2016-06-17 | null       | MP      | MATS    | 2016-06-17 | 2016-06-17 | null       || C      | false || Result C due to unit qualification type for a non-unit location.
        /// | 16 | GF      | 2016-06-17 | null       | MS      | MATS    | 2016-06-17 | 2016-06-17 | null       || C      | false || Result C due to unit qualification type for a non-unit location.
        /// | 17 | GF      | 2016-06-17 | null       | CP      | MATS    | 2016-06-17 | 2016-06-17 | null       || C      | false || Result C due to unit qualification type for a non-unit location.
        /// | 18 | GF      | 2016-06-17 | null       | CS      | MATS    | 2016-06-17 | 2016-06-17 | null       || C      | false || Result C due to unit qualification type for a non-unit location.
        /// | 19 | HGAVG   | 2016-06-17 | null       | CS      | MATS    | 2016-06-17 | 2016-06-17 | null       || C      | false || Result C due to unit qualification type for a non-unit location.
        /// | 20 | LMEA    | 2016-06-17 | null       | CS      | MATS    | 2016-06-17 | 2016-06-17 | null       || C      | false || Result C due to unit qualification type for a non-unit location.
        /// | 21 | LMES    | 2016-06-17 | null       | CS      | MATS    | 2016-06-17 | 2016-06-17 | null       || C      | false || Result C due to unit qualification type for a non-unit location.
        /// | 22 | PK      | 2016-06-17 | null       | CS      | MATS    | 2016-06-17 | 2016-06-17 | null       || C      | false || Result C due to unit qualification type for a non-unit location.
        /// | 23 | SK      | 2016-06-17 | null       | CS      | MATS    | 2016-06-17 | 2016-06-17 | null       || C      | false || Result C due to unit qualification type for a non-unit location.
        /// | 24 | LEE     | 2016-06-17 | null       | UB      | MATS    | 2016-06-17 | 2016-06-17 | null       || null   | true  || No result because LEE can exist at a unit with active program.
        /// | 25 | LEE     | 2016-06-17 | null       | UP      | MATS    | 2016-06-17 | 2016-06-17 | null       || null   | true  || No result because LEE can exist at a unit with active program.
        /// | 26 | LEE     | 2016-06-17 | null       | US      | MATS    | 2016-06-17 | 2016-06-17 | null       || null   | true  || No result because LEE can exist at a unit with active program.
        /// | 27 | LEE     | 2016-06-17 | null       | U       | MATS    | 2016-06-17 | 2016-06-17 | null       || null   | true  || No result because LEE can exist at a unit with active program.
        /// | 28 | LEE     | 2016-06-17 | null       | MP      | MATS    | 2016-06-17 | 2016-06-17 | null       || D      | false || Result D because LEE cannot exist at a MS, CP or MP.
        /// | 29 | LEE     | 2016-06-17 | null       | MS      | MATS    | 2016-06-17 | 2016-06-17 | null       || D      | false || Result D because LEE cannot exist at a MS, CP or MP.
        /// | 30 | LEE     | 2016-06-17 | null       | CP      | MATS    | 2016-06-17 | 2016-06-17 | null       || D      | false || Result D because LEE cannot exist at a MS, CP or MP.
        /// | 31 | LEE     | 2016-06-17 | null       | CS      | MATS    | 2016-06-17 | 2016-06-17 | null       || null   | true  || No result because LEE can exist at a CS with active associated program.
        /// | 32 | LEE     | 2015-06-17 | 2016-06-16 | UB      | MATS    | 2016-06-17 | 2016-06-17 | null       || E      | false || Result E because no active program exists.
        /// | 33 | LEE     | 2015-06-17 | 2016-06-16 | UP      | MATS    | 2016-06-17 | 2016-06-17 | null       || E      | false || Result E because no active program exists.
        /// | 34 | LEE     | 2015-06-17 | 2016-06-16 | US      | MATS    | 2016-06-17 | 2016-06-17 | null       || E      | false || Result E because no active program exists.
        /// | 35 | LEE     | 2015-06-17 | 2016-06-16 | U       | MATS    | 2016-06-17 | 2016-06-17 | null       || E      | false || Result E because no active program exists.
        /// | 36 | LEE     | 2015-06-17 | 2016-06-16 | MP      | MATS    | 2016-06-17 | 2016-06-17 | null       || E      | false || Result E because no active associated program exists.
        /// | 37 | LEE     | 2015-06-17 | 2016-06-16 | MS      | MATS    | 2016-06-17 | 2016-06-17 | null       || E      | false || Result E because no active associated program exists.
        /// | 38 | LEE     | 2015-06-17 | 2016-06-16 | CP      | MATS    | 2016-06-17 | 2016-06-17 | null       || E      | false || Result E because no active associated program exists.
        /// | 39 | LEE     | 2015-06-17 | 2016-06-16 | CS      | MATS    | 2016-06-17 | 2016-06-17 | null       || E      | false || Result E because no active associated program exists.
        /// | 40 | LEE     | 2015-06-17 | 2016-06-16 | U       | MATS    | 2016-06-16 | 2016-06-17 | null       || null   | true  || No result because an active program exists due to UMCB.
        /// | 41 | LEE     | 2015-06-17 | 2016-06-16 | U       | MATS    | 2016-06-17 | 2016-06-16 | null       || null   | true  || No result because an active program exists due to ERB.
        /// | 42 | LEE     | 2015-06-17 | 2016-06-16 | U       | MATS    | 2014-06-17 | 2014-06-17 | 2015-06-17 || null   | true  || No result because an active program exists due to Program End Date.
        /// | 43 | LEE     | 2015-06-17 | 2016-06-16 | U       | MATS    | 2014-06-17 | 2014-06-17 | 2015-06-16 || E      | false || Result E because no active associated program exists.
        /// | 44 | LEE     | 2016-06-17 | null       | U       | ARP     | 2016-06-17 | 2016-06-17 | null       || E      | false || Result E because no MATS program exists.
        /// | 45 | LEE     | 2015-06-17 | 2016-06-16 | U       | MATS    | 2016-06-17 | null       | null       || E      | false || Result E because no active program exists.
        /// </summary>
        [TestMethod()]
        public void Qual16()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            MpParameters.Init(category.Process);
            MpParameters.Category = category;

            /* Test Case Values */
            int caseCount = 46;
            DateTime d20160617 = new DateTime(2016, 6, 17);
            DateTime d20150617 = new DateTime(2015, 6, 17);
            DateTime d20160616 = new DateTime(2016, 6, 16);
            DateTime d20140617 = new DateTime(2014, 6, 17);
            DateTime d20150616 = new DateTime(2015, 6, 16);

            /* Input Parameter Values */
            string[] qualTypeList = { null, "BAD", "COMPLEX", "LOWSULF", "PRATA1", "PRATA2", "GF", "GF", "GF", "GF", "HGAVG", "LMEA", "LMES", "PK", "SK", "GF", "GF", "GF", "GF", "HGAVG", "LMEA", "LMES", "PK", "SK", "LEE", "LEE", "LEE", "LEE", "LEE", "LEE", "LEE", "LEE", "LEE", "LEE", "LEE", "LEE", "LEE", "LEE", "LEE", "LEE", "LEE", "LEE", "LEE", "LEE", "LEE", "LEE" };
            DateTime?[] qualBeginList = { d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20150617, d20150617, d20150617, d20150617, d20150617, d20150617, d20150617, d20150617, d20150617, d20150617, d20150617, d20150617, d20160617, d20150617 };
            DateTime?[] qualEndList = { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, d20160616, d20160616, d20160616, d20160616, d20160616, d20160616, d20160616, d20160616, d20160616, d20160616, d20160616, d20160616, null, d20160616 };
            string[] locTypeList = { "MP", "MP", "MP", "MP", "MP", "MP", "UB", "UP", "US", "U", "U", "U", "U", "U", "U", "MP", "MS", "CP", "CS", "CS", "CS", "CS", "CS", "CS", "UB", "UP", "US", "U", "MP", "MS", "CP", "CS", "UB", "UP", "US", "U", "MP", "MS", "CP", "CS", "U", "U", "U", "U", "U", "U" };
            string[] prgCodeList = { "MATS", "MATS", "MATS", "MATS", "MATS", "MATS", "MATS", "MATS", "MATS", "MATS", "MATS", "MATS", "MATS", "MATS", "MATS", "MATS", "MATS", "MATS", "MATS", "MATS", "MATS", "MATS", "MATS", "MATS", "MATS", "MATS", "MATS", "MATS", "MATS", "MATS", "MATS", "MATS", "MATS", "MATS", "MATS", "MATS", "MATS", "MATS", "MATS", "MATS", "MATS", "MATS", "MATS", "MATS", "ARP", "MATS" };
            DateTime?[] prgUmcbList = { d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160616, d20160617, d20140617, d20140617, d20160617, d20160617 };
            DateTime?[] prgErbList = { d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160617, d20160616, d20140617, d20140617, d20160617, null };
            DateTime?[] prgEndList = { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, d20150617, d20150616, null, null };

            /* Expected Values */
            string[] resultList = { "A", "B", null, null, null, null, null, null, null, null, null, null, null, null, null, "C", "C", "C", "C", "C", "C", "C", "C", "C", null, null, null, null, "D", "D", "D", null, "E", "E", "E", "E", "E", "E", "E", "E", null, null, null, "E", "E", "E" };
            bool?[] validList = { false, false, true, true, true, true, true, true, true, true, true, true, true, true, true, false, false, false, false, false, false, false, false, false, true, true, true, true, false, false, false, true, false, false, false, false, false, false, false, false, true, true, true, false, false, false };

            /* Check array lengths */
            Assert.AreEqual(caseCount, qualTypeList.Length, "qualTypeList length");
            Assert.AreEqual(caseCount, qualBeginList.Length, "qualBeginList length");
            Assert.AreEqual(caseCount, qualEndList.Length, "qualEndList length");
            Assert.AreEqual(caseCount, locTypeList.Length, "locTypeList length");
            Assert.AreEqual(caseCount, prgCodeList.Length, "prgCodeList length");
            Assert.AreEqual(caseCount, prgUmcbList.Length, "prgUmcbList length");
            Assert.AreEqual(caseCount, prgErbList.Length, "prgErbList length");
            Assert.AreEqual(caseCount, prgEndList.Length, "prgEndList length");
            Assert.AreEqual(caseCount, resultList.Length, "resultList length");
            Assert.AreEqual(caseCount, validList.Length, "validList length");

            /* Run Cases */
            for (int caseDex = 0; caseDex < caseCount; caseDex++)
            {
                /* Initialize Input Parameters */
                MpParameters.CurrentQualification = new VwMpQualificationRow(qualTypeCd: qualTypeList[caseDex], beginDate: qualBeginList[caseDex], endDate: qualEndList[caseDex]);
                MpParameters.LocationProgramRecords
                    = new CheckDataView<VwLocationProgramRow>
                    (
                        new VwLocationProgramRow(prgCd: "ARP", unitMonitorCertBeginDate: d20140617, emissionsRecordingBeginDate: d20140617, endDate: null),
                        new VwLocationProgramRow(prgCd: prgCodeList[caseDex], unitMonitorCertBeginDate: prgUmcbList[caseDex], emissionsRecordingBeginDate: prgErbList[caseDex], endDate: prgEndList[caseDex]),
                        new VwLocationProgramRow(prgCd: "TRNOX", unitMonitorCertBeginDate: d20140617, emissionsRecordingBeginDate: d20140617, endDate: null)
                    );
                MpParameters.LocationType = locTypeList[caseDex];
                MpParameters.QualificationTypeCodeLookupTable
                    = new CheckDataView<QualTypeCodeRow>
                    (
                        new QualTypeCodeRow(qualTypeCd: "COMPLEX", qualTypeCdDescription: "Flow-to-Load Test Exemption due to Complex Stack Configuration (Petition Approved)"),
                        new QualTypeCodeRow(qualTypeCd: "GF", qualTypeCdDescription: "Gas-Fired Unit"),
                        new QualTypeCodeRow(qualTypeCd: "HGAVG", qualTypeCdDescription: "MATS Hg Averaging Group"),
                        new QualTypeCodeRow(qualTypeCd: "LEE", qualTypeCdDescription: "LEE qualification"),
                        new QualTypeCodeRow(qualTypeCd: "LMEA", qualTypeCdDescription: "Annual LME Unit"),
                        new QualTypeCodeRow(qualTypeCd: "LMES", qualTypeCdDescription: "Ozone-Season LME Unit"),
                        new QualTypeCodeRow(qualTypeCd: "LOWSULF", qualTypeCdDescription: "RATA Exemption for Using Only Very Low Sulfur Fuel"),
                        new QualTypeCodeRow(qualTypeCd: "PK", qualTypeCdDescription: "Year-Round Peaking Unit"),
                        new QualTypeCodeRow(qualTypeCd: "PRATA1", qualTypeCdDescription: "Single-Level RATA (Petition Approved)"),
                        new QualTypeCodeRow(qualTypeCd: "PRATA2", qualTypeCdDescription: "Two-Level RATA (Petition Approved)"),
                        new QualTypeCodeRow(qualTypeCd: "SK", qualTypeCdDescription: "Ozone-Season Peaking Unit")
                    );

                /* Initialize Output Parameters */
                MpParameters.QualificationTypeCodeValid = null;

                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Initialize variables needed to run the check. */
                bool log = false;
                string actual;

                /* Run Check */
                actual = cQualificationChecks.QUAL16(category, ref log);

                /* Check Results */
                Assert.AreEqual(string.Empty, actual, string.Format("actual {0}", caseDex));
                Assert.AreEqual(false, log, string.Format("log {0}", caseDex));
                Assert.AreEqual(resultList[caseDex], category.CheckCatalogResult, string.Format("category.CheckCatalogResult {0}", caseDex));

                Assert.AreEqual(validList[caseDex], MpParameters.QualificationTypeCodeValid, string.Format("QualificationTypeCodeValid {0}", caseDex));
            }
        }


        #region QUAL16

            /// <summary>
            ///A test for QUAL16_LEE
            ///</summary>()
        [TestMethod()]
        public void QUAL16_LEE()
        {
            //static check setup
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            MpParameters.Init(category.Process);
            MpParameters.Category = category;

            // Variables
            bool log = false;
            string actual;

            // Init Input
            MpParameters.CurrentQualification = new VwMpQualificationRow(qualTypeCd: "LEE", beginDate: new DateTime(2019, 6, 17));

            {
                //MATS
                MpParameters.LocationProgramRecords = new CheckDataView<VwLocationProgramRow>(new VwLocationProgramRow(prgCd: "MATS", unitMonitoringBeginDate: new DateTime(2018, 6, 17)));
                string[] testLocationTypeList = { "MS", "CP", "MP", "U" };

                foreach (string testLocationType in testLocationTypeList)
                {
                    MpParameters.LocationType = testLocationType;

                    // Init Output
                    category.CheckCatalogResult = null;
                    MpParameters.QualificationTypeCodeValid = true;

                    // Run Checks
                    actual = cQualificationChecks.QUAL16(category, ref log);

                    // Check Results
                    Assert.AreEqual(string.Empty, actual);
                    Assert.AreEqual(false, log);
                    if (testLocationType.InList("MS,CP,MP"))
                    {
                        Assert.AreEqual("D", category.CheckCatalogResult, "Result");
                        Assert.AreEqual(false, MpParameters.QualificationTypeCodeValid, "Valid");
                    }
                    else
                    {
                        Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                        Assert.AreEqual(true, MpParameters.QualificationTypeCodeValid, "Valid");
                    }

                }
            }
            {
                // Not MATS
                MpParameters.LocationProgramRecords = new CheckDataView<VwLocationProgramRow>(new VwLocationProgramRow(prgCd: "NOTMATS"));
                string[] testLocationTypeList = { "MS", "CP", "MP", "U" };

                foreach (string testLocationType in testLocationTypeList)
                {
                    MpParameters.LocationType = testLocationType;

                    // Init Output
                    category.CheckCatalogResult = null;
                    MpParameters.QualificationTypeCodeValid = true;

                    // Run Checks
                    actual = cQualificationChecks.QUAL16(category, ref log);

                    // Check Results
                    Assert.AreEqual(string.Empty, actual);
                    Assert.AreEqual(false, log);
                    Assert.AreEqual("E", category.CheckCatalogResult, "Result");
                    Assert.AreEqual(false, MpParameters.QualificationTypeCodeValid, "Valid");

                }
            }
        }

        /// <summary>
        ///A test for QUAL16_NOTLEE
        ///</summary>()
        [TestMethod()]
        public void QUAL16_NOTLEE()
        {
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            MpParameters.Init(category.Process);
            MpParameters.Category = category;

            // Variables
            bool log = false;
            string actual;
            //bool testTrue = false;

            // Init Input
            MpParameters.CurrentQualification = new VwMpQualificationRow(qualTypeCd: "NOTLEE");
            MpParameters.LocationType = "U";
            MpParameters.QualificationTypeCodeLookupTable = new CheckDataView<QualTypeCodeRow>();

            // Init Output
            category.CheckCatalogResult = null;
            MpParameters.QualificationTypeCodeValid = true;

            // Run Checks
            actual = cQualificationChecks.QUAL16(category, ref log);

            // Check Results
            Assert.AreEqual(string.Empty, actual);
            Assert.AreEqual(false, log);
            Assert.AreEqual("B", category.CheckCatalogResult, "Result");
            Assert.AreEqual(false, MpParameters.QualificationTypeCodeValid, "Valid");
        }
        #endregion

        #region QUAL38
        // <summary>
        //A test for QUAL38_LEE
        //</summary>()
        [TestMethod()]
        public void QUAL38_LEE()
        {
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            MpParameters.Init(category.Process);
            MpParameters.Category = category;

            //Variables
            bool log = false;
            string actual;
            //bool testTrue = false;

            //Init Input
            MpParameters.MonitorQualificationValid = true;
            MpParameters.MonitorQualificationDatesConsistent = true;
            MpParameters.QualificationPercentRecords = new CheckDataView<VwMonitorQualificationPctRow>();
            MpParameters.QualificationEvaluationEndDate = DateTime.Today;
            MpParameters.QualificationEvaluationStartDate = DateTime.Today.AddDays(-10);
            MpParameters.QualificationTypeCodeValid = true;
            MpParameters.CurrentQualification = new VwMpQualificationRow(qualTypeCd: "LEE");

            //missing QualLEE records
            {
                MpParameters.QualificationleeRecords = new CheckDataView<ECMPS.Checks.Data.Ecmps.CheckMp.Function.MonitorQualificationLEEParameter>();

                //Init Output
                category.CheckCatalogResult = null;

                //Run Checks
                actual = cQualificationChecks.QUAL38(category, ref log);

                //Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual("F", category.CheckCatalogResult, "Result");
            }

            //has QualLEE records
            {
                MpParameters.QualificationleeRecords = new CheckDataView<ECMPS.Checks.Data.Ecmps.CheckMp.Function.MonitorQualificationLEEParameter>(
                    new ECMPS.Checks.Data.Ecmps.CheckMp.Function.MonitorQualificationLEEParameter(monLocId: "NOTNULL"));

                //Init Output
                category.CheckCatalogResult = null;

                //Run Checks
                actual = cQualificationChecks.QUAL38(category, ref log);

                //Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
            }

        }
        #endregion

        //// <summary>
        ////A test for QUAL[N]
        ////</summary>()
        //[TestMethod()]
        //public void QUAL[N]()
        //{
        //    cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

        //    MpParameters.Init(category.Process);
        //    MpParameters.Category = category;

        ////     Variables
        //    bool log = false;
        //    string actual;
        ////    bool testTrue = false;

        ////     Init Input
        //    MpParameters.QualificationTypeCodeValid = true;
        //    MpParameters.CurrentQualification = new CheckDataView<VwMpQualificationRow>();

        ////     Init Output
        //    category.CheckCatalogResult = null;

        ////     Run Checks
        //    actual = cQualificationChecks.QUAL[N](category, ref log);

        ////     Check Results
        //    Assert.AreEqual(string.Empty, actual);
        //    Assert.AreEqual(false, log);
        //    Assert.AreEqual(null, category.CheckCatalogResult, "Result");

        //}

    }
}
