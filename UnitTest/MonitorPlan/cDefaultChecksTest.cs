using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.DefaultChecks;
using ECMPS.Checks.MonitorPlan;

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
    ///This is a test class for cDefaultChecksTest and is intended
    ///to contain all cDefaultChecksTest Unit Tests
    /// </summary>
    [TestClass]
    public class cDefaultChecksTest
    {
        public cDefaultChecksTest()
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

        #region DEFAULT-49

        /// <summary>
        ///A test for DEFAULT49_RemoveAKSF_HGX_HGC
        ///</summary>()
        [TestMethod()]
        public void DEFAULT49_RemoveAKSF_HGX_HGC()
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
            MpParameters.DefaultEvaluationBeginDate = DateTime.Today.AddDays(-10);
            MpParameters.DefaultEvaluationEndDate = DateTime.Today;
            MpParameters.DefaultParameterCodeValid = true;
            MpParameters.FuelCodeToMinimumAndMaximumMoistureDefaultCrossCheckTable = new CheckDataView<FuelCodeToMinimumAndMaximumMoistureDefaultValueRow>();
            MpParameters.LocationUnitTypeRecords = new CheckDataView<VwLocationUnitTypeRow>();
            MpParameters.MethodRecords = new CheckDataView<VwMonitorMethodRow>();
            MpParameters.ParameterUnitsOfMeasureLookupTable = new CheckDataView<ParameterUomRow>();
            string[] testParameterList = { "H2OX", "AKSF", "HGX", "HGC" };

            foreach (string testParameterCode in testParameterList)
            {
                MpParameters.CurrentDefault = new VwMonitorDefaultRow(parameterCd: testParameterCode, defaultSourceCd: "DEF", defaultValue: 14);
                //If DefaultPurposeCode and the DefaultSourceCode combination is not equal to "LM" and "DEF

                // Init Output
                category.CheckCatalogResult = null;

                // Run Checks
                actual = cDefaultChecks.DEFAULT49(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                if (testParameterCode == "H2OX")
                {
                    Assert.AreEqual("F", category.CheckCatalogResult, "Result");
                }
                else
                {
                    Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                }
            }
        }
        #endregion

        #region DEFAULT-50

        /// <summary>
        ///A test for DEFAULT50_UOMNull
        ///</summary>()
        [TestMethod()]
        public void DEFAULT50_UOMNull()
        {
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            MpParameters.Init(category.Process);
            MpParameters.Category = category;

            // Variables
            bool log = false;
            string actual;
            //bool testTrue = false;

            // Init Input
            MpParameters.CurrentDefault = new VwMonitorDefaultRow(parameterCd: "AKSF", defaultUomCd: null);
            MpParameters.DefaultParameterCodeValid = true;
            MpParameters.ParameterUnitsOfMeasureLookupTable = new CheckDataView<ParameterUomRow>();
            MpParameters.UnitsOfMeasureCodeLookupTable = new CheckDataView<UnitsOfMeasureCodeRow>();

            // Init Output
            category.CheckCatalogResult = null;

            // Run Checks
            actual = cDefaultChecks.DEFAULT50(category, ref log);

            // Check Results
            Assert.AreEqual(string.Empty, actual);
            Assert.AreEqual(false, log);
            Assert.AreEqual("A", category.CheckCatalogResult, "Result");
        }


        /// <summary>
        ///A test for DEFAULT50_RemoveAKSF
        ///</summary>()
        [TestMethod()]
        public void DEFAULT50_RemoveAKSF()
        {
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            MpParameters.Init(category.Process);
            MpParameters.Category = category;

            // Variables
            bool log = false;
            string actual;
            //bool testTrue = false;

            // Init Input
            MpParameters.CurrentDefault = new VwMonitorDefaultRow(parameterCd: "AKSF", defaultUomCd: "UOMGOOD");
            MpParameters.DefaultParameterCodeValid = true;
            MpParameters.ParameterUnitsOfMeasureLookupTable = new CheckDataView<ParameterUomRow>();
            MpParameters.UnitsOfMeasureCodeLookupTable = new CheckDataView<UnitsOfMeasureCodeRow>();

            // Init Output
            category.CheckCatalogResult = null;

            // Run Checks
            actual = cDefaultChecks.DEFAULT50(category, ref log);

            // Check Results
            Assert.AreEqual(string.Empty, actual);
            Assert.AreEqual(false, log);
            Assert.AreEqual("B", category.CheckCatalogResult, "Result");
        }
        #endregion

        #region DEFAULT-53

        /// <summary>
        ///A test for DEFAULT53_RemoveAKSF
        ///</summary>()
        [TestMethod()]
        public void DEFAULT53_RemoveAKSF()
        {
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            MpParameters.Init(category.Process);
            MpParameters.Category = category;

            // Variables
            bool log = false;
            string actual;
            bool testTrue = false;

            // Init Input
            MpParameters.DefaultParameterCodeValid = true;
            MpParameters.FuelCodeLookupTable = new CheckDataView<FuelCodeRow>(new FuelCodeRow(fuelCd: "NOTNSF"));

            string[] testParameterList = { "AKSF", "O2", "CO2N", "CO2X", "H2ON", "H2OX", "" };

            foreach (string testParameterCode in testParameterList)
            {
                MpParameters.CurrentDefault = new VwMonitorDefaultRow(parameterCd: testParameterCode, fuelCd: "NOTNSF");
                if (testParameterCode.InList("CO2N,CO2X,H2ON,H2OX") || testParameterCode.StartsWith("O2"))
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
                actual = cDefaultChecks.DEFAULT53(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                if (testTrue == true)
                {
                    Assert.AreEqual("C", category.CheckCatalogResult, "Result");
                    Assert.AreEqual(false, MpParameters.DefaultFuelCodeValid, "DefaultFuelCodeValid");
                }
                else
                {
                    Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                    Assert.AreEqual(true, MpParameters.DefaultFuelCodeValid, "DefaultFuelCodeValid");
                }
            }
        }
        #endregion

        #region DEFAULT-54

        /// <summary>
        ///A test for DEFAULT54_RemoveHGC
        ///</summary>()
        [TestMethod()]
        public void DEFAULT54_RemoveHGC()
        {
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            MpParameters.Init(category.Process);
            MpParameters.Category = category;

            // Variables
            bool log = false;
            string actual;
            //bool testTrue = false;

            // Init Input
            MpParameters.CurrentDefault = new VwMonitorDefaultRow(parameterCd: "HGC", defaultSourceCd: "DEF", fuelCd: "NOTNFS", defaultPurposeCd: "LM");
            MpParameters.DefaultFuelCodeValid = true;
            MpParameters.DefaultParameterCodeValid = true;
            MpParameters.DefaultPurposeCodeValid = true;
            MpParameters.DefaultEvaluationBeginDate = DateTime.Today.AddDays(-10);
            MpParameters.DefaultEvaluationEndDate = DateTime.Today;
            MpParameters.DefaultParameterBoilerTypeAndFuelTypeToDefaultValueCrossCheckTable = new CheckDataView<DefaultParameterBoilerTypeAndFuelTypeToDefaultValueRow>();
            MpParameters.DefaultRecords = new CheckDataView<VwMonitorDefaultRow>();
            MpParameters.LocationUnitTypeRecords = new CheckDataView<VwLocationUnitTypeRow>();

            // Init Output
            category.CheckCatalogResult = null;

            // Run Checks
            actual = cDefaultChecks.DEFAULT54(category, ref log);

            // Check Results
            Assert.AreEqual(string.Empty, actual);
            Assert.AreEqual(false, log);
            Assert.AreEqual("A", category.CheckCatalogResult, "Result");

        }
        #endregion

        #region DEFAULT-98

        /// <summary>
        ///A test for DEFAULT98
        ///</summary>()
        [TestMethod()]
        public void DEFAULT98()
        {
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            MpParameters.Init(category.Process);
            MpParameters.Category = category;

            // Variables
            bool log = false;
            string actual;
            bool testTrue = false;

            // Init Input
            MpParameters.DefaultParameterCodeValid = true;

            //GroupID is null
            string[] testParameterList = { "HGC", "NOXR" };
            string[] testPurposeList = { "LM", "NOTLM" };

            foreach (string testParameterCode in testParameterList)
            {
                foreach (string testPurposeCode in testPurposeList)
                {
                    MpParameters.CurrentDefault = new VwMonitorDefaultRow(parameterCd: testParameterCode, defaultPurposeCd: testPurposeCode, groupId: null);

                    // Init Output
                    category.CheckCatalogResult = null;

                    // Run Checks
                    actual = cDefaultChecks.DEFAULT98(category, ref log);

                    // Check Results
                    Assert.AreEqual(string.Empty, actual);
                    Assert.AreEqual(false, log);
                    Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                }
            }


            //If GroupID is not null,
            //If ParameterCode is not equal to "NOXR", or DefaultPurposeCode is not equal to "LM",
            //return result B.


            //GroupID is not null
            foreach (string testParameterCode in testParameterList)
            {
                foreach (string testPurposeCode in testPurposeList)
                {
                    MpParameters.CurrentDefault = new VwMonitorDefaultRow(parameterCd: testParameterCode, defaultPurposeCd: testPurposeCode, groupId: "NOTNULL");

                    if (testParameterCode != "NOXR" || testPurposeCode != "LM")
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
                    actual = cDefaultChecks.DEFAULT98(category, ref log);

                    // Check Results
                    Assert.AreEqual(string.Empty, actual);
                    Assert.AreEqual(false, log);
                    if (testTrue == true)
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

        //#region DEFAULT-[N]

        ///// <summary>
        /////A test for DEFAULT[N]
        /////</summary>()
        //[TestMethod()]
        //public void DEFAULT[N]()
        //{
        //    cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

        //    MpParameters.Init(category.Process);
        //    MpParameters.Category = category;

        //    // Variables
        //    bool log = false;
        //    string actual;
        //    //bool testTrue = false;

        //    // Init Input
        //    MpParameters.EcmpsMpBeginDate = null;

        //    // Init Output
        //    category.CheckCatalogResult = null;

        //    // Run Checks
        //    actual = cDefaultChecks.DEFAULT[N](category, ref log);

        //    // Check Results
        //    Assert.AreEqual(string.Empty, actual);
        //    Assert.AreEqual(false, log);
        //    Assert.AreEqual(null, category.CheckCatalogResult, "Result");

        //}
        //#endregion
    }
}
