using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.MonitorPlan;
using ECMPS.Checks.LoadChecks;

using ECMPS.Definitions.Extensions;

using ECMPS.Checks;
using ECMPS.Checks.Data.Ecmps.Dbo.Table;
using ECMPS.Checks.Data.Ecmps.Dbo.View;
using ECMPS.Checks.Data.EcmpsAux.CrossCheck.Virtual;
using ECMPS.Checks.Data.Ecmps.CrossCheck.Table;
using ECMPS.Checks.Mp.Parameters;

using UnitTest.UtilityClasses;

namespace UnitTest.MonitorPlan
{
    /// <summary>
    /// Summary description for cLoadChecksTest
    /// </summary>
    [TestClass]
    public class cLoadChecksTest
    {
        public cLoadChecksTest()
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

        #region LOAD-20

        /// <summary>
        ///A test for LOAD20_HG
        ///</summary>()
        [TestMethod()]
        public void LOAD20_HG()
        {
			//static check setup
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            MpParameters.Init(category.Process);
            MpParameters.Category = category;

            // Variables
            bool log = false;
            string actual;
            bool testTrue = false;
            string[] testSystemTypeList = { "SYSBAD", "SO2", "SO2R", "NOX", "NOXC", "CO2", "O2", "FLOW", "HG", "HCL", "HF" };

            // Init Input
            MpParameters.CurrentLoad = new VwMonitorLoadRow(monLocId: "LOC1");
            MpParameters.FacilityQualificationRecords = new CheckDataView<MonitorQualificationRow>();
            MpParameters.LoadEvaluationEndDate = DateTime.Today;
            MpParameters.LoadEvaluationEndHour = 0;
            MpParameters.LoadEvaluationStartDate = DateTime.Today.AddDays(-1);
            MpParameters.LoadEvaluationStartHour = 0;
            MpParameters.LocationType = "U";
            MpParameters.QaSupplementalDataRecords = new CheckDataView<VwQaSuppDataRow>();
            MpParameters.UnitStackConfigurationRecords = new CheckDataView<VwUnitStackConfigurationRow>();

            foreach (string testSystemTypeCode in testSystemTypeList)
            {
                // Init Input
                MpParameters.MonitorSystemRecords = new CheckDataView<VwMonitorSystemRow>
                    (new VwMonitorSystemRow(monLocId: "LOC1", sysTypeCd: testSystemTypeCode, beginDate: DateTime.Today.AddDays(-1), beginHour: 0, beginDatehour: DateTime.Today.AddDays(-1)));
                if (testSystemTypeCode.InList("SO2,SO2R,NOX,NOXC,CO2,O2,FLOW,HG,HCL,HF"))
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
                actual = cLoadChecks.LOAD20(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                if (testTrue)
                {
                    Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                    Assert.AreEqual(true, MpParameters.RangeOfOperationRequired, "RangeOfOperationRequired");
                    Assert.AreEqual(true, MpParameters.LoadLevelsRequired, "LoadLevelsRequired");
                }
                else
                {
                    Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                    Assert.AreEqual(false, MpParameters.RangeOfOperationRequired, "RangeOfOperationRequired");
                    Assert.AreEqual(false, MpParameters.LoadLevelsRequired, "LoadLevelsRequired");
                }
            }
        }
        #endregion

    }
}