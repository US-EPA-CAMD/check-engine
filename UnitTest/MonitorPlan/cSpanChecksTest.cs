using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.MonitorPlan;
using ECMPS.Checks.SpanChecks;

using ECMPS.Definitions.Extensions;

using ECMPS.Checks;
using ECMPS.Checks.Data.Ecmps.Dbo.Table;
using ECMPS.Checks.Data.Ecmps.Dbo.View;
using ECMPS.Checks.Data.EcmpsAux.CrossCheck.Virtual;
using ECMPS.Checks.Data.Ecmps.CrossCheck.Table;
using ECMPS.Checks.Mp.Parameters;
using ECMPS.Checks.TypeUtilities;

using UnitTest.UtilityClasses;

namespace UnitTest.MonitorPlan
{
    /// <summary>
    ///This is a test class for cSpanChecksTest and is intended
    ///to contain all cSpanChecksTest Unit Tests
    /// </summary>
    [TestClass]
    public class cSpanChecksTest
    {
        public cSpanChecksTest()
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

        #region SPAN-1

        /// <summary>
        ///A test for SPAN1_MPCValue
        ///</summary>()
        [TestMethod()]
        public void SPAN1_MPCValue()
        {
            //static check setup
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            MpParameters.Init(category.Process);
            MpParameters.Category = category;

            // Variables
            bool log = false;
            string actual;
            bool testTrue = false;
            decimal[] testMPCList = { 1, 9, 10, 16, 100 };

            // Init Input
            MpParameters.LocationFuelRecords = new CheckDataView<VwLocationFuelRow>();
            MpParameters.LocationUnitTypeRecords = new CheckDataView<VwLocationUnitTypeRow>();
            MpParameters.NoxMpcToFuelCategoryAndUnitType = new CheckDataView<NoxMpcToFuelCategoryAndUnitTypeRow>();
            MpParameters.SpanComponentTypeCodeValid = true;
            MpParameters.SpanEvaluationBeginDate = DateTime.Today.AddDays(-10);
            MpParameters.SpanEvaluationEndDate = DateTime.Today;
            MpParameters.SpanMpcValueValid = true;

            foreach (decimal testMPCValue in testMPCList)
            {
                MpParameters.CurrentSpan = new VwMonitorSpanRow(componentTypeCd: "HG", mpcValue: testMPCValue, spanMethodCd: "TB");
                if (testMPCValue == 10 || testMPCValue == 16)
                {
                    testTrue = false;
                }
                else
                {
                    testTrue = true;
                }

                // Init Output
                category.CheckCatalogResult = null;

                // Run Checks
                actual = cSpanChecks.SPAN1(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                if (testTrue)
                {
                    Assert.AreEqual("E", category.CheckCatalogResult, "Result");
                }
                else
                {
                    Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                }
            }
        }
        #endregion

        #region SPAN-2

        /// <summary>
        ///A test for SPAN2_HG
        ///</summary>()
        [TestMethod()]
        public void SPAN2_HG()
        {
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            MpParameters.Init(category.Process);
            MpParameters.Category = category;

            // Variables
            bool log = false;
            string actual;
            bool testTrue = false;

            // Init Input
            MpParameters.LocationAttributeRecords = new CheckDataView<MonitorLocationAttributeRow>();
            MpParameters.LocationControlRecords = new CheckDataView<VwLocationControlRow>();
            MpParameters.MethodRecords = new CheckDataView<VwMonitorMethodRow>();
            MpParameters.SpanEvaluationBeginDate = DateTime.Today.AddDays(-10);
            MpParameters.SpanEvaluationBeginHour = 0;
            MpParameters.SpanEvaluationEndDate = DateTime.Today;
            MpParameters.SpanEvaluationEndHour = 23;
            MpParameters.SpanComponentTypeCodeValid = true;

            //MECValue is null
            foreach (string testComponentCode in UnitTestStandardLists.ComponentTypeCodeList)
            {
                MpParameters.CurrentSpan = new VwMonitorSpanRow(componentTypeCd: testComponentCode, mecValue: null, spanScaleCd: "L");
                if (testComponentCode.InList("SO2,NOX"))
                {
                    testTrue = true;
                }
                else
                {
                    testTrue = false;
                }

                // Init Output
                category.CheckCatalogResult = null;
                MpParameters.SpanMecValueValid = true;

                // Run Checks
                actual = cSpanChecks.SPAN2(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                if (testTrue)
                {
                    Assert.AreEqual("A", category.CheckCatalogResult, "Result");
                    Assert.AreEqual(false, MpParameters.SpanMecValueValid, "SpanMecValueValid");
                }
                else
                {
                    Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                    Assert.AreEqual(true, MpParameters.SpanMecValueValid, "SpanMecValueValid");
                }
            }

            //MECValue NOT null
            foreach (string testComponentCode in UnitTestStandardLists.ComponentTypeCodeList)
            {
                MpParameters.CurrentSpan = new VwMonitorSpanRow(componentTypeCd: testComponentCode, mecValue: 1, spanScaleCd: "L");
                if (testComponentCode.InList("FLOW,O2,HG,HCL"))
                {
                    testTrue = true;
                }
                else
                {
                    testTrue = false;
                }

                // Init Output
                category.CheckCatalogResult = null;
                MpParameters.SpanMecValueValid = true;

                // Run Checks
                actual = cSpanChecks.SPAN2(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                if (testTrue)
                {
                    Assert.AreEqual("F", category.CheckCatalogResult, "Result");
                    Assert.AreEqual(false, MpParameters.SpanMecValueValid, "SpanMecValueValid");
                }
                else
                {
                    Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                    Assert.AreEqual(true, MpParameters.SpanMecValueValid, "SpanMecValueValid");
                }
            }
        }
        #endregion

        #region SPAN-4

        /// <summary>
        ///A test for SPAN4_HG
        ///</summary>()
        [TestMethod()]
        public void SPAN4_HG()
        {
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            MpParameters.Init(category.Process);
            MpParameters.Category = category;

            // Variables
            bool log = false;
            string actual;
            string[] testComponentList = { "HG", "NOTHG" };

            // Init Input
            MpParameters.EcmpsMpBeginDate = null;
            MpParameters.LocationAnalyzerRangeRecords = new CheckDataView<AnalyzerRangeRow>();
            MpParameters.SpanComponentTypeCodeValid = true;
            MpParameters.SpanDatesAndHoursConsistent = true;
            MpParameters.SpanEvaluationBeginDate = DateTime.Today.AddDays(-10);
            MpParameters.SpanEvaluationBeginHour = 0;
            MpParameters.SpanEvaluationEndDate = DateTime.Today;
            MpParameters.SpanEvaluationEndHour = 23;
            MpParameters.SpanRecords = new CheckDataView<MonitorSpanRow>();

            foreach (string testComponentCode in testComponentList)
            {
                //"ScaleTransitionPoint" = maxLowRange
                MpParameters.CurrentSpan = new VwMonitorSpanRow(componentTypeCd: testComponentCode, spanScaleCd: "H", maxLowRange: 1, spanValue: null, defaultHighRange: 1);

                // Init Output
                category.CheckCatalogResult = null;

                // Run Checks
                actual = cSpanChecks.SPAN4(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                if (testComponentCode != "HG")
                {
                    Assert.AreEqual("A", category.CheckCatalogResult, "Result");
                }
                else
                {
                    Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                }
            }
        }
        #endregion

        #region SPAN-6

        /// <summary>
        ///A test for SPAN6_HG
        ///</summary>()
        [TestMethod()]
        public void SPAN6_HG()
        {
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            MpParameters.Init(category.Process);
            MpParameters.Category = category;

            // Variables
            bool log = false;
            string actual;
            string sResult = null;
            string MPC_MEC = null;
            string[] testSpanScaleList = { "H", "L" };
            decimal MinimumSpanValue = decimal.MinValue;

            // Init Input
            MpParameters.SpanComponentTypeCodeValid = true;
            MpParameters.SpanMpcValueValid = true;
            MpParameters.SpanMecValueValid = true;

            foreach (string ComponentTypeCode in UnitTestStandardLists.ComponentTypeCodeList)
            {
                foreach (string testSpanScaleCode in testSpanScaleList)
                {
                    MpParameters.CurrentSpan = new VwMonitorSpanRow(componentTypeCd: ComponentTypeCode, spanValue: 11, spanScaleCd: testSpanScaleCode, mpcValue: 10, mecValue: 2);

                    // Init Output
                    category.CheckCatalogResult = null;
                    MpParameters.MpcMec = null;
                    MpParameters.MinimumSpanValue = decimal.MinValue;

                    sResult = null;
                    MPC_MEC = string.Empty;
                    MinimumSpanValue = decimal.MinValue;

                    // Run Checks
                    actual = cSpanChecks.SPAN6(category, ref log);
                    if (cDBConvert.ToBoolean(MpParameters.SpanComponentTypeCodeValid))
                    {
                        if (MpParameters.CurrentSpan.SpanValue == null)
                        {
                            switch (ComponentTypeCode)
                            {
                                case "SO2":
                                case "NOX":
                                    sResult = "A";
                                    break;
                                default:
                                    if (MpParameters.CurrentSpan.SpanScaleCd != "H")
                                        sResult = "A";
                                    break;
                            }
                        }
                        else
                        {
                            switch (ComponentTypeCode)
                            {
                                case "FLOW":
                                    if (MpParameters.CurrentSpan.SpanUomCd != "SCFH")
                                        MinimumSpanValue = 0.001m;
                                    else
                                        MinimumSpanValue = 500000;
                                    break;
                                case "SO2":
                                case "NOX":
                                case "HG":
                                case "HCL":
                                    MinimumSpanValue = 1;
                                    break;
                                case "CO2":
                                case "O2":
                                    MinimumSpanValue = 0.1m;
                                    break;
                                default:
                                    //Do nothing...
                                    break;
                            }
                            if (cDBConvert.ToDecimal(MpParameters.CurrentSpan.SpanValue) < MinimumSpanValue)
                                sResult = "B"; 
                            else if ((MpParameters.MaximumSpanValue != null) &&
                                     (cDBConvert.ToDecimal(MpParameters.CurrentSpan.SpanValue) > cDBConvert.ToDecimal(MpParameters.MaximumSpanValue)))
                                sResult = "C";
                            else
                            {
                                switch (ComponentTypeCode)
                                {
                                    case "FLOW":
                                    case "HG":
                                    case "HCL":
                                        //Do Nothing
                                        break;
                                    default:
                                        switch (MpParameters.CurrentSpan.SpanScaleCd)
                                        {
                                            case "H":
                                                if (cDBConvert.ToBoolean(MpParameters.SpanMpcValueValid))
                                                {
                                                    MPC_MEC = "MPC";
                                                    if (cDBConvert.ToDecimal(MpParameters.CurrentSpan.SpanValue) < cDBConvert.ToDecimal(MpParameters.CurrentSpan.MpcValue))
                                                        sResult = "D";
                                                    else if (ComponentTypeCode.InList("SO2,NOX") &&
                                                            ((100 * Math.Ceiling(1.25m * cDBConvert.ToDecimal(MpParameters.CurrentSpan.MpcValue) / 100)))
                                                              < cDBConvert.ToDecimal(MpParameters.CurrentSpan.SpanValue))
                                                    {
                                                        sResult = "E";
                                                    }
                                                }
                                                break;
                                            case "L":
                                                if (cDBConvert.ToBoolean(MpParameters.SpanMecValueValid))
                                                {
                                                    MPC_MEC = "MEC";
                                                    if (cDBConvert.ToDecimal(MpParameters.CurrentSpan.SpanValue) < cDBConvert.ToDecimal(MpParameters.CurrentSpan.MecValue))
                                                        sResult = "D";
                                                    else if (ComponentTypeCode.InList("SO2,NOX") &&
                                                            ((100 * Math.Ceiling(1.25m * cDBConvert.ToDecimal(MpParameters.CurrentSpan.MecValue) / 100)))
                                                              < cDBConvert.ToDecimal(MpParameters.CurrentSpan.SpanValue))
                                                    {
                                                        sResult = "E";
                                                    }
                                                }
                                                break;
                                            default:
                                                //Do nothing...
                                                break;
                                        }
                                        break;
                                }
                            }
                        }
                        // Check Results
                        Assert.AreEqual(string.Empty, actual);
                        Assert.AreEqual(false, log);
                        Assert.AreEqual(MinimumSpanValue, cDBConvert.ToDecimal(MpParameters.MinimumSpanValue), "MinimumSpanValue");
                        Assert.AreEqual(MPC_MEC, cDBConvert.ToString(MpParameters.MpcMec), "MPC_MEC");
                        Assert.AreEqual(sResult, category.CheckCatalogResult, "Result");
                    }
                }
            }
        }
        #endregion

        #region SPAN-18
        /// <summary>
        ///A test for SPAN18_SpanScaleCode
        ///</summary>()
        [TestMethod()]
        public void SPAN18_SpanScaleCode()
        {
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            MpParameters.Init(category.Process);
            MpParameters.Category = category;

            // Variables
            bool log = false;
            string actual;
            string[] testSpanScaleList = { null, "H", "L", "SPANBAD" };

            // Init Input
            MpParameters.SpanComponentTypeCodeValid = true;

            //Test C: SpanScaleCode not null
            foreach (string testSpanScaleCode in testSpanScaleList)
            {
                foreach (string ComponentTypeCode in UnitTestStandardLists.ComponentTypeCodeList)
                {
                    MpParameters.CurrentSpan = new VwMonitorSpanRow(componentTypeCd: ComponentTypeCode,
                                                                    spanScaleCd: testSpanScaleCode);
                    // Init Output
                    category.CheckCatalogResult = null;

                    // Run Checks
                    actual = cSpanChecks.SPAN18(category, ref log);

                    // Check Results of Init
                    Assert.AreEqual(string.Empty, actual);
                    Assert.AreEqual(false, log);
                    if (ComponentTypeCode != "FLOW")
                    {
                        if (testSpanScaleCode == null)
                        {
                            Assert.AreEqual("A", category.CheckCatalogResult, "Result");
                            Assert.AreEqual(false, MpParameters.SpanScaleCodeValid, "SpanScaleCodeValid");
                        }
                        else if (!testSpanScaleCode.InList("H,L"))
                        {
                            Assert.AreEqual("B", category.CheckCatalogResult, "Result");
                            Assert.AreEqual(false, MpParameters.SpanScaleCodeValid, "SpanScaleCodeValid");
                        }
                        else if (ComponentTypeCode.InList("HG,HCL") &&
                                (testSpanScaleCode != "H"))
                        {
                            Assert.AreEqual("D", category.CheckCatalogResult, "Result");
                            Assert.AreEqual(false, MpParameters.SpanScaleCodeValid, "SpanScaleCodeValid");
                        }
                        else
                        { 
                            //When no failing case set Span Scale Code Valid true.
                            Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                            Assert.AreEqual(true, MpParameters.SpanScaleCodeValid, "SpanScaleCodeValid");
                        }
                    }
                    else if (testSpanScaleCode != null)
                    {
                        //FLOW should have a NULL Span Scale Code.
                        Assert.AreEqual("C", category.CheckCatalogResult, "Result");
                        Assert.AreEqual(false, MpParameters.SpanScaleCodeValid, "SpanScaleCodeValid");
                    }
                    else
                    {
                        //When no failing case set Span Scale Code Valid true.
                        Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                        Assert.AreEqual(true, MpParameters.SpanScaleCodeValid, "SpanScaleCodeValid");
                    }
                }
            }
        }        

        [TestMethod()]
        public void SPAN18_ResultC()
        {
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            MpParameters.Init(category.Process);
            MpParameters.Category = category;

            // Variables
            bool log = false;
            string actual;
            string[] testComponentTypeList = { "FLOW", "NOTFLOW" };

            // Init Input
            MpParameters.SpanComponentTypeCodeValid = true;

            //Test C: SpanScaleCode not null
            foreach (string testComponentCode in testComponentTypeList)
            {
                MpParameters.CurrentSpan = new VwMonitorSpanRow(componentTypeCd: testComponentCode, spanScaleCd: "NOTNULL");

                // Init Output
                category.CheckCatalogResult = null;

                // Run Checks
                actual = cSpanChecks.SPAN18(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                if (testComponentCode == "FLOW")
                {
                    Assert.AreEqual("C", category.CheckCatalogResult, "Result");
                    Assert.AreEqual(false, MpParameters.SpanScaleCodeValid, "SpanScaleCodeValid");
                }
                else
                {
                    Assert.AreEqual("B", category.CheckCatalogResult, "Result");
                    Assert.AreEqual(false, MpParameters.SpanScaleCodeValid, "SpanScaleCodeValid");
                }
            }
            //Test Not C: SpanScaleCode null
            MpParameters.CurrentSpan = new VwMonitorSpanRow(componentTypeCd: "FLOW", spanScaleCd: null);

            // Init Output
            category.CheckCatalogResult = null;

            // Run Checks
            actual = cSpanChecks.SPAN18(category, ref log);

            // Check Results
            Assert.AreEqual(string.Empty, actual);
            Assert.AreEqual(false, log);
            Assert.AreEqual(null, category.CheckCatalogResult, "Result");
            Assert.AreEqual(true, MpParameters.SpanScaleCodeValid, "SpanScaleCodeValid");
        }
        #endregion

        #region SPAN-48

        /// <summary>
        ///A test for SPAN48_RemoveHG
        ///</summary>()
        [TestMethod()]
        public void SPAN48_RemoveHG()
        {
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            MpParameters.Init(category.Process);
            MpParameters.Category = category;

            // Variables
            bool log = false;
            string actual;
            //bool testTrue = false;
            string[] testComponentTypeList = { "HG", "SO2" };

            // Init Input
            MpParameters.SpanEvaluationBeginDate = DateTime.Today.AddDays(-10);
            MpParameters.SpanEvaluationBeginHour = 0;
            MpParameters.SpanEvaluationEndDate = DateTime.Today;
            MpParameters.SpanEvaluationEndHour = 23;
            MpParameters.SpanRecords = new CheckDataView<MonitorSpanRow>();
            MpParameters.SpanDefaultHighRangeValueValid = true;

            foreach (string testComponentTypeCode in testComponentTypeList)
            {
                MpParameters.CurrentSpan = new VwMonitorSpanRow(spanScaleCd: "H", componentTypeCd: testComponentTypeCode, defaultHighRange: 1);

                // Init Output
                category.CheckCatalogResult = null;

                // Run Checks
                actual = cSpanChecks.SPAN48(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                if (testComponentTypeCode.InList("SO2,NOX"))
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

        #region SPAN-57

        /// <summary>
        ///A test for SPAN57_MECNull
        ///</summary>()
        [TestMethod()]
        public void SPAN57_MECNull()
        {
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            MpParameters.Init(category.Process);
            MpParameters.Category = category;

            // Variables
            bool log = false;
            string actual;
            //bool testTrue = false;
            string[] testComponentTypeList = { "FLOW", "O2", "SO2", "HG", "NOX" };
            string[] testSpanScaleList = { "L", "H" };
            string testResultCode = null;

            // Init Input
            MpParameters.SpanComponentTypeCodeValid = true;

            foreach (string testComponentTypeCode in testComponentTypeList)
            {
                foreach (string testSpanScaleCode in testSpanScaleList)
                {
                    MpParameters.CurrentSpan = new VwMonitorSpanRow(componentTypeCd: testComponentTypeCode, spanScaleCd: testSpanScaleCode, mecValue: null, defaultHighRange: 1);

                    if (testComponentTypeCode.NotInList("FLOW,O2") && testSpanScaleCode == "L")
                    {
                        testResultCode = "A";
                    }
                    else if (testComponentTypeCode.InList("SO2,HG,NOX") && testSpanScaleCode == "H")
                    {
                        testResultCode = "B";
                    }
                    else
                    {
                        testResultCode = null;
                    }

                    // Init Output
                    category.CheckCatalogResult = null;
                    MpParameters.SpanMecValueValid = true;

                    // Run Checks
                    actual = cSpanChecks.SPAN57(category, ref log);

                    // Check Results
                    Assert.AreEqual(string.Empty, actual);
                    Assert.AreEqual(false, log);
                    Assert.AreEqual(testResultCode, category.CheckCatalogResult, "Result");
                    if (testResultCode != null)
                    {
                        Assert.AreEqual(false, MpParameters.SpanMecValueValid, "SpanMecValueValid");
                    }
                    else
                    {
                        Assert.AreEqual(true, MpParameters.SpanMecValueValid, "SpanMecValueValid");
                    }
                }
            }
        }

        /// <summary>
        ///A test for SPAN57_MECNotNull
        ///</summary>()
        [TestMethod()]
        public void SPAN57_MECNotNull()
        {
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            MpParameters.Init(category.Process);
            MpParameters.Category = category;

            // Variables
            bool log = false;
            string actual;
            //bool testTrue = false;
            string[] testComponentTypeList = { "FLOW", "O2", "SO2", "HG", "NOX" };
            decimal[] testMECList = { -1, 0, 1 };
            string testResultCode = null;

            // Init Input
            MpParameters.SpanComponentTypeCodeValid = true;

            foreach (string testComponentTypeCode in testComponentTypeList)
            {
                foreach (decimal testMECValue in testMECList)
                {
                    MpParameters.CurrentSpan = new VwMonitorSpanRow(componentTypeCd: testComponentTypeCode, mecValue: testMECValue);

                    if (testComponentTypeCode.InList("FLOW,HG,O2"))
                    {
                        testResultCode = "C";
                    }
                    else if (testMECValue <= 0)
                    {
                        testResultCode = "D";
                    }
                    else
                    {
                        testResultCode = null;
                    }

                    // Init Output
                    category.CheckCatalogResult = null;
                    MpParameters.SpanMecValueValid = true;

                    // Run Checks
                    actual = cSpanChecks.SPAN57(category, ref log);

                    // Check Results
                    Assert.AreEqual(string.Empty, actual);
                    Assert.AreEqual(false, log);
                    Assert.AreEqual(testResultCode, category.CheckCatalogResult, "Result");
                    if (testResultCode != null)
                    {
                        Assert.AreEqual(false, MpParameters.SpanMecValueValid, "SpanMecValueValid");
                    }
                    else
                    {
                        Assert.AreEqual(true, MpParameters.SpanMecValueValid, "SpanMecValueValid");
                    }
                }
            }
        }
        #endregion

        #region SPAN-61

        /// <summary>
        ///A test for SPAN61
        ///</summary>()
        [TestMethod()]
        public void SPAN61()
        {
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            MpParameters.Init(category.Process);
            MpParameters.Category = category;

            // Variables
            bool log = false;
            string actual; 

            foreach (string ComponentTypeCd in UnitTestStandardLists.ComponentTypeCodeList)
            {
                if (ComponentTypeCd.InList("HG,HCL"))
                {
            //ScaleTransitionPoint (maxLowRange) is NOT null 
                    // Init Input
                    MpParameters.SpanComponentTypeCodeValid = true;
                    MpParameters.CurrentSpan = new VwMonitorSpanRow(componentTypeCd: ComponentTypeCd, maxLowRange: 1);

                    // Init Output
                    category.CheckCatalogResult = null;

                    // Run Checks
                    actual = cSpanChecks.SPAN61(category, ref log);

                    // Check Results
                    Assert.AreEqual(string.Empty, actual);
                    //Only specified to return A when Scale Transition Point is not NULL.
                    Assert.AreEqual(false, log);
                    Assert.AreEqual("A", category.CheckCatalogResult, "Result");

            //ScaleTransitionPoint (maxLowRange) is null
                    // Init Input
                    MpParameters.SpanComponentTypeCodeValid = true;
                    MpParameters.CurrentSpan = new VwMonitorSpanRow(componentTypeCd: ComponentTypeCd, maxLowRange: null);

                    // Init Output
                    category.CheckCatalogResult = null;

                    // Run Checks
                    actual = cSpanChecks.SPAN61(category, ref log);

                    // Check Results
                    Assert.AreEqual(string.Empty, actual);
                    Assert.AreEqual(false, log);
                    Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                }
                else
                {
                    // Init Input
                    MpParameters.SpanComponentTypeCodeValid = true;
                    MpParameters.CurrentSpan = new VwMonitorSpanRow(componentTypeCd: ComponentTypeCd, maxLowRange: null);

                    // Init Output
                    category.CheckCatalogResult = null;

                    // Run Checks
                    actual = cSpanChecks.SPAN61(category, ref log);

                    // Check Results
                    Assert.AreEqual(string.Empty, actual);
                    Assert.AreEqual(false, log);
                    Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                }
            }
        }        

        #endregion


        //}
        //    #region SPAN-N

        //    /// <summary>
        //    ///A test for SPAN[N]
        //    ///</summary>()
        //    [TestMethod()]
        //    public void SPAN[N]()
        //    {
        //        cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

        //        MpParameters.Init(category.Process);
        //        MpParameters.Category = category;

        //        // Variables
        //        bool log = false;
        //        string actual;
        //        //bool testTrue = false;

        //        // Init Input
        //                  MpParameters.CurrentSpan = new VwMonitorSpanRow();

        //        // Init Output
        //        category.CheckCatalogResult = null;

        //        // Run Checks
        //        actual = cSpanChecks.SPAN[N](category, ref log);

        //        // Check Results
        //        Assert.AreEqual(string.Empty, actual);
        //        Assert.AreEqual(false, log);
        //        Assert.AreEqual(null, category.CheckCatalogResult, "Result");
        //        Assert.AreEqual(false, MpParameters.MethodParameterValid, "MethodParameterValid");

        //    }
        //    #endregion
        //}
    }
}
