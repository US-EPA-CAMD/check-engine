using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.MonitorPlan;
using ECMPS.Checks.SystemChecks;

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
    ///This is a test class for cSystemChecksTest and is intended
    ///to contain all cSystemChecksTest Unit Tests
    /// </summary>
    [TestClass]
    public class cSystemChecksTest
    {
        public cSystemChecksTest()
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

        #region SYSTEM-9

        /// <summary>
        ///A test for SYSTEM9_RemoveHG
        ///</summary>()
        [TestMethod()]
        public void SYSTEM9_RemoveHG()
        {
			//static check setup
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            MpParameters.Init(category.Process);
            MpParameters.Category = category;

            // Variables
            bool log = false;
            string actual;
            //bool testTrue = false;

            // Init Input
            MpParameters.SystemDesignationCodeLookupTable = new CheckDataView<SystemDesignationCodeRow>
                (new SystemDesignationCodeRow(sysDesignationCd: "CI"),
                new SystemDesignationCodeRow(sysDesignationCd: "RM"),
                new SystemDesignationCodeRow(sysDesignationCd: "PB"));
            MpParameters.SystemTypeCodeValid = true;

            //Result B
            string[] testSystemDesignationList = { "CI", "RM" };
            string[] testSystemTypeList = { "SO2", "NOX", "NOXC", "SO2R", "GAS", "OILM", "OILV", "OP", "NOXP", "NOXE", "HGK", "HG", "HCL", "HF", "ST" };
            bool testTrue = false;

            foreach (string testSystemDesignationCode in testSystemDesignationList)
            {
                foreach (string testSystemTypeCode in testSystemTypeList)
                {
                    MpParameters.CurrentSystem = new VwMonitorSystemRow(sysDesignationCd: testSystemDesignationCode, sysTypeCd: testSystemTypeCode);
                    if (MpParameters.SystemTypeCodeValid == true &&
                       ((testSystemDesignationCode == "CI" && !testSystemTypeCode.InList("SO2,NOX,NOXC,SO2R"))
                            || (testSystemDesignationCode == "RM" && testSystemTypeCode.InList("GAS,OILM,OILV,OP,NOXP,NOXE,HG,HCL,HF,ST"))
                            || (testSystemDesignationCode == "PB" && !testSystemTypeCode.InList("NOX,NOXC"))
                            )
                        )
                    {
                        testTrue = true;
                    }
                    else
                    {
                        testTrue = false;
                    }

                    // Init Output
                    category.CheckCatalogResult = null;

                    // Run Checks
                    actual = cSystemChecks.SYSTEM9(category, ref log);

                    // Check Results
                    Assert.AreEqual(string.Empty, actual);
                    Assert.AreEqual(false, log);

                    if (testTrue)
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
        #endregion

        #region SYSTEM-13

        /// <summary>
        ///A test for SYSTEM13_MATS
        ///</summary>()
        [TestMethod()]
        public void SYSTEM13_MATS()
        {
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            MpParameters.Init(category.Process);
            MpParameters.Category = category;

            // Variables
            bool log = false;
            string actual;
            bool testTrue = false;

            // Init Input
            MpParameters.SystemDatesAndHoursConsistent = true;
            MpParameters.SystemEvaluationBeginDate = DateTime.Today.AddDays(-10);
            MpParameters.SystemEvaluationBeginHour = 0;
            MpParameters.SystemEvaluationEndDate = DateTime.Today;
            MpParameters.SystemEvaluationEndHour = 23;
            MpParameters.SystemTypeCodeValid = true;

            string[] testSystemTypeList = { "SO2", "SO2R", "NOX", "NOXC", "CO2", "O2", "HG", "HF", "HCL", "ST", "SYSBAD" };

            foreach (string testSystemTypeCode in testSystemTypeList)
            {
                MpParameters.CurrentSystem = new VwMonitorSystemRow(sysTypeCd: testSystemTypeCode);
                MpParameters.SystemTypeToComponentTypeCrossCheckTable = new CheckDataView<SystemTypeToComponentTypeRow>
                    (new SystemTypeToComponentTypeRow(systemTypeCode: testSystemTypeCode, componentTypeCode: "COMPON1"));
                //note - there is a componentid and componentIdentifier, which are not the same thing, but the spec refers to as ComponentID
                MpParameters.SystemSystemComponentRecords = new CheckDataView<VwMonitorSystemComponentRow>
                    (new VwMonitorSystemComponentRow(componentTypeCd: "COMPON1", componentId: "ID1", componentIdentifier: "ID1", beginDate: DateTime.Today.AddDays(-10), beginHour: 0, endDate: DateTime.Today.AddDays(-3), endHour: 23),
                    new VwMonitorSystemComponentRow(componentTypeCd: "COMPON1", componentId: "ID2", componentIdentifier: "ID2", beginDate: DateTime.Today.AddDays(-7), beginHour: 0, endDate: DateTime.Today, endHour: 23)
                    );
                //note - there is a componentid and componentIdentifier, which are not the same thing, but the spec refers to as ComponentID
                MpParameters.SystemAnalyzerRangeRecords = new CheckDataView<VwAnalyzerRangeRow>
                    (new VwAnalyzerRangeRow(componentTypeCd: "COMPON1", analyzerRangeCd: "H", componentId: "ID1", componentIdentifier: "ID1", beginDate: DateTime.Today.AddDays(-10), beginHour: 0, endDate: DateTime.Today.AddDays(-3), endHour: 23),
                    new VwAnalyzerRangeRow(componentTypeCd: "COMPON1", analyzerRangeCd: "H", componentId: "ID2", componentIdentifier: "ID2", beginDate: DateTime.Today.AddDays(-7), beginHour: 0, endDate: DateTime.Today, endHour: 23)
                    );

                if (testSystemTypeCode.InList("SO2,SO2R,NOX,NOXC,CO2,O2,HG,HF,HCL,ST"))
                {
                    testTrue = true;
                }
                else
                {
                    testTrue = false;
                }

                // Init Output
                category.CheckCatalogResult = null;

                // Run Checks
                actual = cSystemChecks.SYSTEM13(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                if (testTrue)
                {
                    Assert.AreEqual("B", category.CheckCatalogResult, "Result");
                }
                else
                {
                    Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                }
            }
        }

        #endregion

        #region SYSTEM-19

        /// <summary>
        ///A test for SYSTEM19_RemoveHGK
        /// Also tests addition of HCL and HF
        ///</summary>()
        [TestMethod()]
        public void SYSTEM19_RemoveHGK()
        {
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            MpParameters.Init(category.Process);
            MpParameters.Category = category;

            // Variables
            bool log = false;
            string actual;
            bool testTrue = false;

            // Init Input

            MpParameters.SystemDatesAndHoursConsistent = true;
            MpParameters.SystemEvaluationBeginDate = DateTime.Today.AddDays(-10);
            MpParameters.SystemEvaluationBeginHour = 0;
            MpParameters.SystemEvaluationEndDate = DateTime.Today;
            MpParameters.SystemEvaluationEndHour = 23;
            MpParameters.CurrentSystem = new VwMonitorSystemRow(sysTypeCd: "HGK");
            string[] testComponentTypeList = { "SO2", "NOX", "CO2", "O2", "H2O", "HCL", "HF", "HG", "COMPONBAD" };
            string[] testAcqList = { "DIL", "DOU", "DIN", "EXT", "WXT", "ACQBAD" };

            // test substitution of ST for HGK (SystemType != H20 or ST)
            // Also tests addition of HCL and HF
            foreach (string testComponentTypeCode in testComponentTypeList)
            {
                foreach (string testAcqCode in testAcqList)
                {
                    MpParameters.SystemSystemComponentRecords = new CheckDataView<VwMonitorSystemComponentRow>
                        (new VwMonitorSystemComponentRow(componentTypeCd: testComponentTypeCode, componentId: "ID1", componentIdentifier: "ID1", acqCd: testAcqCode, beginDate: DateTime.Today.AddDays(-10), beginHour: 0, endDate: DateTime.Today, endHour: 23));
                    MpParameters.SystemTypeCodeValid = true;
                    MpParameters.SystemTypeToComponentTypeCrossCheckTable = new CheckDataView<SystemTypeToComponentTypeRow>
                        (new SystemTypeToComponentTypeRow(systemTypeCode: "HGK", componentTypeCode: testComponentTypeCode, mandatory: "Yes"));

                    if (testComponentTypeCode.InList("SO2,NOX,CO2,O2,H2O,HCL,HF,HG") && testAcqCode.InList("DIL,DOU,DIN,EXT,WXT"))
                    {
                        testTrue = true;
                    }
                    else
                    {
                        testTrue = false;
                    }

                    // Init Output
                    category.CheckCatalogResult = null;

                    // Run Checks
                    actual = cSystemChecks.SYSTEM19(category, ref log);

                    // Check Results
                    Assert.AreEqual(string.Empty, actual);
                    Assert.AreEqual(false, log);
                    if (testTrue)
                    {
                        Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                        Assert.AreEqual(true, MpParameters.RequiredProbe, "RequiredProbe");
                    }
                    else
                    {
                        Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                        Assert.AreEqual(false, MpParameters.RequiredProbe, "RequiredProbe");
                    }
                }
            }
        }

        /// <summary>
        ///A test for SYSTEM19_AddHCL_HF
        ///</summary>()
        [TestMethod()]
        public void SYSTEM19_AddHCL_HF()
        {
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            MpParameters.Init(category.Process);
            MpParameters.Category = category;

            // Variables
            bool log = false;
            string actual;
            bool testTrue = false;

            // Init Input

            MpParameters.SystemDatesAndHoursConsistent = true;
            MpParameters.SystemEvaluationBeginDate = DateTime.Today.AddDays(-10);
            MpParameters.SystemEvaluationBeginHour = 0;
            MpParameters.SystemEvaluationEndDate = DateTime.Today;
            MpParameters.SystemEvaluationEndHour = 23;
            MpParameters.CurrentSystem = new VwMonitorSystemRow(sysTypeCd: "SYSTEMGOOD");
            string[] testComponentTypeList = { "SO2", "NOX", "CO2", "O2", "H2O", "HCL", "HF", "HG", "COMPONBAD" };
            string[] testAcqList = { "DIL", "DOU", "DIN", "EXT", "WXT", "ACQBAD" };

            //test Mandatory is null block
            foreach (string testComponentTypeCode in testComponentTypeList)
            {
                foreach (string testAcqCode in testAcqList)
                {
                    MpParameters.SystemSystemComponentRecords = new CheckDataView<VwMonitorSystemComponentRow>
                        (new VwMonitorSystemComponentRow(componentTypeCd: testComponentTypeCode, componentId: "ID1", componentIdentifier: "ID1", acqCd: testAcqCode, beginDate: DateTime.Today.AddDays(-10), beginHour: 0, endDate: DateTime.Today, endHour: 23));
                    MpParameters.SystemTypeCodeValid = true;
                    MpParameters.SystemTypeToComponentTypeCrossCheckTable = new CheckDataView<SystemTypeToComponentTypeRow>
                        (new SystemTypeToComponentTypeRow(systemTypeCode: "SYSTEMGOOD", componentTypeCode: testComponentTypeCode, mandatory: null));

                    if (testComponentTypeCode.InList("SO2,NOX,CO2,O2,H2O,HCL,HF,HG") && testAcqCode.InList("DIL,DOU,DIN,EXT,WXT"))
                    {
                        testTrue = true;
                    }
                    else
                    {
                        testTrue = false;
                    }

                    // Init Output
                    category.CheckCatalogResult = null;

                    // Run Checks
                    actual = cSystemChecks.SYSTEM19(category, ref log);

                    // Check Results
                    Assert.AreEqual(string.Empty, actual);
                    Assert.AreEqual(false, log);
                    if (testTrue)
                    {
                        Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                        Assert.AreEqual(true, MpParameters.RequiredProbe, "RequiredProbe");
                    }
                    else
                    {
                        Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                        Assert.AreEqual(false, MpParameters.RequiredProbe, "RequiredProbe");
                    }
                }
            }
        }

        /// <summary>
        ///A test for SYSTEM19_AddST_STRAIN
        ///</summary>()
        [TestMethod()]
        public void SYSTEM19_AddST_STRAIN()
        {
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            MpParameters.Init(category.Process);
            MpParameters.Category = category;

            // Variables
            bool log = false;
            string actual;

            // Init Input
            MpParameters.SystemDatesAndHoursConsistent = true;
            MpParameters.SystemEvaluationBeginDate = DateTime.Today.AddDays(-10);
            MpParameters.SystemEvaluationBeginHour = 0;
            MpParameters.SystemEvaluationEndDate = DateTime.Today;
            MpParameters.SystemEvaluationEndHour = 23;
            MpParameters.CurrentSystem = new VwMonitorSystemRow(sysTypeCd: "ST");
            string[] testComponentTypeList = { "STRAIN", "TRAP", "GFM", "COMPONBAD" };

            //test SystemType = ST and ComponentTypeCode = STRAIN, <2 System Component records
            foreach (string testComponentTypeCode in testComponentTypeList)
            {
                MpParameters.SystemSystemComponentRecords = new CheckDataView<VwMonitorSystemComponentRow>
                    (new VwMonitorSystemComponentRow(componentTypeCd: testComponentTypeCode, componentId: "ID1", componentIdentifier: "ID1", beginDate: DateTime.Today.AddDays(-10), beginHour: 0, endDate: DateTime.Today, endHour: 23));
                MpParameters.SystemTypeCodeValid = true;
                MpParameters.SystemTypeToComponentTypeCrossCheckTable = new CheckDataView<SystemTypeToComponentTypeRow>();
                //               (new SystemTypeToComponentTypeRow(systemTypeCode: "ST", componentTypeCode: testComponentTypeCode, mandatory: null));

                // Init Output
                category.CheckCatalogResult = null;

                // Run Checks
                actual = cSystemChecks.SYSTEM19(category, ref log);

                // Check Results
                //same result for 0 or 1 record found
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual("F", category.CheckCatalogResult, "Result");
                Assert.AreEqual(false, MpParameters.RequiredNonDahsComponentsReportedForSystem, "RequiredNonDahsComponentsReportedForSystem");
                Assert.AreEqual("STRAIN", MpParameters.MissingComponentsForSystem, "MissingComponentsForSystem");
            }

            //test SystemType = ST and ComponentTypeCode = STRAIN, >=2 System Component records
            foreach (string testComponentTypeCode in testComponentTypeList)
            {
                MpParameters.SystemSystemComponentRecords = new CheckDataView<VwMonitorSystemComponentRow>
                    (new VwMonitorSystemComponentRow(componentTypeCd: testComponentTypeCode, componentIdentifier: "ID1", beginDate: DateTime.Today.AddDays(-10), beginHour: 0, endDate: DateTime.Today, endHour: 23),
                    new VwMonitorSystemComponentRow(componentTypeCd: testComponentTypeCode, componentIdentifier: "ID2", beginDate: DateTime.Today.AddDays(-2), beginHour: 0, endDate: DateTime.Today, endHour: 23));
                MpParameters.SystemTypeCodeValid = true;
                MpParameters.SystemTypeToComponentTypeCrossCheckTable = new CheckDataView<SystemTypeToComponentTypeRow>();
                //               (new SystemTypeToComponentTypeRow(systemTypeCode: "ST", componentTypeCode: testComponentTypeCode, mandatory: null));

                // Init Output
                category.CheckCatalogResult = null;

                // Run Checks
                actual = cSystemChecks.SYSTEM19(category, ref log);

                // Check Results
                if (testComponentTypeCode == "STRAIN")
                {
                    Assert.AreEqual(string.Empty, actual);
                    Assert.AreEqual(false, log);
                    Assert.AreEqual("F", category.CheckCatalogResult, "Result");
                    Assert.AreEqual(true, MpParameters.RequiredNonDahsComponentsReportedForSystem, "RequiredNonDahsComponentsReportedForSystem");
                    Assert.AreEqual("STRAIN", MpParameters.MissingComponentsForSystem, "MissingComponentsForSystem");
                }
                else
                {
                    Assert.AreEqual(string.Empty, actual);
                    Assert.AreEqual(false, log);
                    Assert.AreEqual("F", category.CheckCatalogResult, "Result");
                    Assert.AreEqual(false, MpParameters.RequiredNonDahsComponentsReportedForSystem, "RequiredNonDahsComponentsReportedForSystem");
                    Assert.AreEqual("STRAIN", MpParameters.MissingComponentsForSystem, "MissingComponentsForSystem");
                }

            }
        }

        /// <summary>
        ///A test for SYSTEM19_RemoveGFM
        ///</summary>()
        [TestMethod()]
        public void SYSTEM20()
        {
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            MpParameters.Init(category.Process);
            MpParameters.Category = category;

            // Variables
            bool log = false;
            string actual;
            //bool testTrue = false;

            // Init Input
            MpParameters.SystemDatesAndHoursConsistent = true;
            MpParameters.SystemEvaluationBeginDate = DateTime.Today.AddDays(-10);
            MpParameters.SystemEvaluationBeginHour = 0;
            MpParameters.SystemEvaluationEndDate = DateTime.Today;
            MpParameters.SystemEvaluationEndHour = 23;
            MpParameters.FormulaRecords = new CheckDataView<VwMonitorFormulaRow>();
            MpParameters.SystemSystemComponentRecords = new CheckDataView<VwMonitorSystemComponentRow>();
            MpParameters.SystemTypeCodeValid = true;

            string[] testSystemTypeList = { "HGK", "H2O" };

            foreach (string testSystemTypeCode in testSystemTypeList)
            {
                MpParameters.CurrentSystem = new VwMonitorSystemRow(sysTypeCd: testSystemTypeCode);
                
                // Init Output
                category.CheckCatalogResult = null;

                // Run Checks
                actual = cSystemChecks.SYSTEM20(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                if (testSystemTypeCode == "H2O")
                {
                    Assert.AreEqual("C", category.CheckCatalogResult, "Result");
                }
                else
                {
                    Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                }
            }
        }

        #endregion

        //#region SYSTEM-[N]

        ///// <summary>
        /////A test for SYSTEM[N]
        /////</summary>()
        //[TestMethod()]
        //public void SYSTEM[N]()
        //{
        //    cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

        //    MpParameters.Init(category.Process);
        //    MpParameters.Category = category;

        //    // Variables
        //    bool log = false;
        //    string actual;
        //    //bool testTrue = false;

        //    // Init Input
        //    MpParameters.SystemTypeCodeValid = true;
        //    MpParameters.CurrentSystem = new VwMonitorSystemRow();

        //    // Init Output
        //    category.CheckCatalogResult = null;

        //    // Run Checks
        //    actual = cSystemChecks.SYSTEM[N](category, ref log);

        //    // Check Results
        //    Assert.AreEqual(string.Empty, actual);
        //    Assert.AreEqual(false, log);
        //    Assert.AreEqual(null, category.CheckCatalogResult, "Result");

        //}
        //#endregion
    }
}
