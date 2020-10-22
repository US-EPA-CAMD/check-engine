using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.MethodChecks;
using ECMPS.Checks.MonitorPlan;
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
    ///This is a test class for cMethodChecksTest and is intended
    ///to contain all cMethodChecksTest Unit Tests
    ///</summary>
    [TestClass()]
    public class cMethodChecksTest
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
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            //populates Reporting Period Table for checks without making db call
            UnitTest.UtilityClasses.UnitTestExtensions.SetReportingPeriodTable();
        }

        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        #region Method-7

        /// <summary>
        ///A test for METHOD7_ResultB
        ///</summary>()
        [TestMethod()]
        public void METHOD7_ResultB()
        {
			//instantiated checks setup
            cMethodChecks target = new cMethodChecks(new cMpCheckParameters());
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            MpParameters.Init(category.Process);
            MpParameters.Category = category;

            // Variables
            bool log = false;
            string actual;
            bool testTrue = false;
            string[] locationTestList = { "CP", "MP", "LOCBAD" };
            string[] parameterTestList = { "H2O", "OP", "NOX", "NOXR", "NOXM", "HGRE", "HGRH", "HCLRE", "HCLRH", "HFRE", "HFRH", "SO2RE", "SO2RH", "BAD" };

            // Init Input
            MpParameters.EcmpsMpBeginDate = null;

            foreach (string locationTest in locationTestList)
            {
                foreach (string parameterTest in parameterTestList)
                {
                    //Init Input
                    MpParameters.LocationType = locationTest;
                    MpParameters.CurrentMethod = new VwMonitorMethodRow(parameterCd: parameterTest, methodCd: null, endDate: null);

                    if(MpParameters.LocationType == "CP" || MpParameters.LocationType == "MP")
                    {
                        testTrue = parameterTest.InList("H2O,OP,NOX,NOXR,NOXM,HGRE,HGRH,HCLRE,HCLRH,HFRE,HFRH,SO2RE,SO2RH");
                    }

            // Init Output
            category.CheckCatalogResult = null;
            MpParameters.MethodParameterValid = null;

                    // Run Checks
                    actual = target.METHOD7(category, ref log);

            if (testTrue == true)
            {
                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual("B", category.CheckCatalogResult, "Result");
                Assert.AreEqual(false, MpParameters.MethodParameterValid, "MethodParameterValid");
            }
            else
            {
                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual("C", category.CheckCatalogResult, "Result");
                Assert.AreEqual(false, MpParameters.MethodParameterValid, "MethodParameterValid");
            }
                }
            }
        }
        #endregion

        #region Method-8

        /// <summary>
        ///A test for METHOD8_ResultA
        ///</summary>()
        [TestMethod()]
        public void METHOD8_ResultA()
        {
            cMethodChecks target = new cMethodChecks(new cMpCheckParameters());
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            MpParameters.Init(category.Process);
            MpParameters.Category = category;

            // Variables
            bool log = false;
            string actual;

            // Init Input
            MpParameters.CurrentMethod = new VwMonitorMethodRow(parameterCd: null, methodCd: null, endDate: null);
            MpParameters.EcmpsMpBeginDate = null;
            MpParameters.LocationType = "U";
            MpParameters.MethodCodeLookupTable = new CheckDataView<MethodCodeRow>();
            MpParameters.ParameterToMethodCrossCheckTable = new CheckDataView<MethodParameterToMethodToSystemTypeRow>();

            // Init Output
            category.CheckCatalogResult = null;
            MpParameters.MethodMethodCodeValid = null;
            MpParameters.MethodSubstituteDataCodeValid = null;

            // Run Checks
            actual = target.METHOD8(category, ref log);

            // Check Results
            Assert.AreEqual(string.Empty, actual);
            Assert.AreEqual(false, log);
            Assert.AreEqual("A", category.CheckCatalogResult, "Result");
            Assert.AreEqual(false, MpParameters.MethodMethodCodeValid, "MethodMethodCodeValid");
            Assert.AreEqual(true, MpParameters.MethodSubstituteDataCodeValid, "MethodSubstituteDataCodeValid");
        }


        /// <summary>
        ///A test for METHOD8_ResultB
        ///</summary>()
        [TestMethod()]
        public void METHOD8_ResultB()
        {
            cMethodChecks target = new cMethodChecks(new cMpCheckParameters());
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            MpParameters.Init(category.Process);
            MpParameters.Category = category;

            // Variables
            bool log = false;
            string actual;

            // Init Input
            MpParameters.EcmpsMpBeginDate = null;
            MpParameters.LocationType = "U";
            MpParameters.MethodCodeLookupTable = new CheckDataView<MethodCodeRow>(new MethodCodeRow(methodCd: "METHGOOD"));
            MpParameters.ParameterToMethodCrossCheckTable = new CheckDataView<MethodParameterToMethodToSystemTypeRow>();

            // Test for not result B
            {
                // Init Input
                MpParameters.CurrentMethod = new VwMonitorMethodRow(parameterCd: "PARAMGOOD", methodCd: "METHGOOD", endDate: null);

                // Init Output
                category.CheckCatalogResult = null;
                MpParameters.MethodMethodCodeValid = null;
                MpParameters.MethodSubstituteDataCodeValid = null;

                // Run Checks
                actual = target.METHOD8(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual("E", category.CheckCatalogResult, "Result - not B");
                Assert.AreEqual(false, MpParameters.MethodMethodCodeValid, "MethodMethodCodeValid - not B");
                Assert.AreEqual(false, MpParameters.MethodSubstituteDataCodeValid, "MethodSubstituteDataCodeValid - not B");
            }

            // Test for result B
            {
                // Init Input
                MpParameters.CurrentMethod = new VwMonitorMethodRow(parameterCd: "PARAMGOOD", methodCd: "METHBAD", endDate: null);

                // Init Output
                category.CheckCatalogResult = null;
                MpParameters.MethodMethodCodeValid = null;
                MpParameters.MethodSubstituteDataCodeValid = null;

                // Run Checks
                actual = target.METHOD8(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual("B", category.CheckCatalogResult, "Result - B");
                Assert.AreEqual(false, MpParameters.MethodMethodCodeValid, "MethodMethodCodeValid - B");
                Assert.AreEqual(false, MpParameters.MethodSubstituteDataCodeValid, "MethodSubstituteDataCodeValid - B");
            }
        }

        /// <summary>
        ///A test for METHOD8_ResultC1
        ///</summary>()
        [TestMethod()]
        public void METHOD8_ResultC1()
        {
            cMethodChecks target = new cMethodChecks(new cMpCheckParameters());
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            MpParameters.Init(category.Process);
            MpParameters.Category = category;

            // Variables
            bool log = false;
            string actual;

            // Init Input
            MpParameters.EcmpsMpBeginDate = null;
            MpParameters.ParameterToMethodCrossCheckTable = new CheckDataView<MethodParameterToMethodToSystemTypeRow>();


            //Test for C
            {
                // Init Input
                MpParameters.MethodCodeLookupTable = new CheckDataView<MethodCodeRow>(new MethodCodeRow(methodCd: "EXP"));
                MpParameters.CurrentMethod = new VwMonitorMethodRow(parameterCd: "HI", methodCd: "EXP", endDate: null);
                MpParameters.LocationType = "NOTU";

                // Init Output
                category.CheckCatalogResult = null;
                MpParameters.MethodMethodCodeValid = null;
                MpParameters.MethodSubstituteDataCodeValid = null;

                // Run Checks
                actual = target.METHOD8(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual("C", category.CheckCatalogResult, "Result");
                Assert.AreEqual(false, MpParameters.MethodMethodCodeValid, "MethodMethodCodeValid");
                Assert.AreEqual(false, MpParameters.MethodSubstituteDataCodeValid, "MethodSubstituteDataCodeValid");
            }
            //Tests for not C - Not EXP
            {
                // Init Input
                MpParameters.MethodCodeLookupTable = new CheckDataView<MethodCodeRow>(new MethodCodeRow(methodCd: "NOTEXP"));
                MpParameters.CurrentMethod = new VwMonitorMethodRow(parameterCd: "HI", methodCd: "NOTEXP", endDate: null);
                MpParameters.LocationType = "NOTU";

                // Init Output
                category.CheckCatalogResult = null;
                MpParameters.MethodMethodCodeValid = null;
                MpParameters.MethodSubstituteDataCodeValid = null;

                // Run Checks
                actual = target.METHOD8(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual("E", category.CheckCatalogResult, "Result");
                Assert.AreEqual(false, MpParameters.MethodMethodCodeValid, "MethodMethodCodeValid");
                Assert.AreEqual(false, MpParameters.MethodSubstituteDataCodeValid, "MethodSubstituteDataCodeValid");
            }
            //Tests for not C - Not HI
            {
                // Init Input
                MpParameters.MethodCodeLookupTable = new CheckDataView<MethodCodeRow>(new MethodCodeRow(methodCd: "EXP"));
                MpParameters.CurrentMethod = new VwMonitorMethodRow(parameterCd: "NOTHI", methodCd: "EXP", endDate: null);
                MpParameters.LocationType = "NOTU";

                // Init Output
                category.CheckCatalogResult = null;
                MpParameters.MethodMethodCodeValid = null;
                MpParameters.MethodSubstituteDataCodeValid = null;

                // Run Checks
                actual = target.METHOD8(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual("E", category.CheckCatalogResult, "Result");
                Assert.AreEqual(false, MpParameters.MethodMethodCodeValid, "MethodMethodCodeValid");
                Assert.AreEqual(false, MpParameters.MethodSubstituteDataCodeValid, "MethodSubstituteDataCodeValid");
            }
            //Tests for not C - Starts w/ U
            {
                // Init Input
                MpParameters.MethodCodeLookupTable = new CheckDataView<MethodCodeRow>(new MethodCodeRow(methodCd: "EXP"));
                MpParameters.CurrentMethod = new VwMonitorMethodRow(parameterCd: "HI", methodCd: "EXP", endDate: null);
                MpParameters.LocationType = "USTART";

                // Init Output
                category.CheckCatalogResult = null;
                MpParameters.MethodMethodCodeValid = null;
                MpParameters.MethodSubstituteDataCodeValid = null;

                // Run Checks
                actual = target.METHOD8(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual("E", category.CheckCatalogResult, "Result");
                Assert.AreEqual(false, MpParameters.MethodMethodCodeValid, "MethodMethodCodeValid");
                Assert.AreEqual(false, MpParameters.MethodSubstituteDataCodeValid, "MethodSubstituteDataCodeValid");
            }
            //Tests for not C - Just U
            {
                // Init Input
                MpParameters.MethodCodeLookupTable = new CheckDataView<MethodCodeRow>(new MethodCodeRow(methodCd: "EXP"));
                MpParameters.CurrentMethod = new VwMonitorMethodRow(parameterCd: "HI", methodCd: "EXP", endDate: null);
                MpParameters.LocationType = "U";

                // Init Output
                category.CheckCatalogResult = null;
                MpParameters.MethodMethodCodeValid = null;
                MpParameters.MethodSubstituteDataCodeValid = null;

                // Run Checks
                actual = target.METHOD8(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual("E", category.CheckCatalogResult, "Result");
                Assert.AreEqual(false, MpParameters.MethodMethodCodeValid, "MethodMethodCodeValid");
                Assert.AreEqual(false, MpParameters.MethodSubstituteDataCodeValid, "MethodSubstituteDataCodeValid");
            }
        }

        /// <summary>
        ///A test for METHOD8_ResultD
        ///</summary>()
        [TestMethod()]
        public void METHOD8_ResultD()
        {
            cMethodChecks target = new cMethodChecks(new cMpCheckParameters());
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            MpParameters.Init(category.Process);
            MpParameters.Category = category;

            // Variables
            bool log = false;
            string actual;

            // Init Input
            MpParameters.ParameterToMethodCrossCheckTable = new CheckDataView<MethodParameterToMethodToSystemTypeRow>();
            MpParameters.LocationType = "U";

            //Test 1 for D
            {
                // Init Input
                MpParameters.EcmpsMpBeginDate = DateTime.Today;
                MpParameters.MethodCodeLookupTable = new CheckDataView<MethodCodeRow>(new MethodCodeRow(methodCd: "CEMNOXR"));
                MpParameters.CurrentMethod = new VwMonitorMethodRow(parameterCd: "NOX", methodCd: "CEMNOXR", endDate: null);

                // Init Output
                category.CheckCatalogResult = null;
                MpParameters.MethodMethodCodeValid = null;
                MpParameters.MethodSubstituteDataCodeValid = null;

                // Run Checks
                actual = target.METHOD8(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual("D", category.CheckCatalogResult, "Result");
                Assert.AreEqual(true, MpParameters.MethodMethodCodeValid, "MethodMethodCodeValid");
                Assert.AreEqual(true, MpParameters.MethodSubstituteDataCodeValid, "MethodSubstituteDataCodeValid");
            }

            //Test 2 for D
            {
                // Init Input
                MpParameters.EcmpsMpBeginDate = DateTime.Today;
                MpParameters.MethodCodeLookupTable = new CheckDataView<MethodCodeRow>(new MethodCodeRow(methodCd: "CEMNOXR"));
                MpParameters.CurrentMethod = new VwMonitorMethodRow(parameterCd: "NOX", methodCd: "CEMNOXR", endDate: DateTime.Today);

                // Init Output
                category.CheckCatalogResult = null;
                MpParameters.MethodMethodCodeValid = null;
                MpParameters.MethodSubstituteDataCodeValid = null;

                // Run Checks
                actual = target.METHOD8(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual("D", category.CheckCatalogResult, "Result");
                Assert.AreEqual(true, MpParameters.MethodMethodCodeValid, "MethodMethodCodeValid");
                Assert.AreEqual(true, MpParameters.MethodSubstituteDataCodeValid, "MethodSubstituteDataCodeValid");
            }

            //Test 1 for not D
            {
                // Init Input
                MpParameters.EcmpsMpBeginDate = DateTime.Today;
                MpParameters.MethodCodeLookupTable = new CheckDataView<MethodCodeRow>(new MethodCodeRow(methodCd: "NOTCEMNOXR"));
                MpParameters.CurrentMethod = new VwMonitorMethodRow(parameterCd: "NOX", methodCd: "NOTCEMNOXR", endDate: DateTime.Today);

                // Init Output
                category.CheckCatalogResult = null;
                MpParameters.MethodMethodCodeValid = null;
                MpParameters.MethodSubstituteDataCodeValid = null;

                // Run Checks
                actual = target.METHOD8(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual("E", category.CheckCatalogResult, "Result");
                Assert.AreEqual(false, MpParameters.MethodMethodCodeValid, "MethodMethodCodeValid");
                Assert.AreEqual(false, MpParameters.MethodSubstituteDataCodeValid, "MethodSubstituteDataCodeValid");
            }

            //Test 2 for not D
            {
                // Init Input
                MpParameters.EcmpsMpBeginDate = DateTime.Today;
                MpParameters.MethodCodeLookupTable = new CheckDataView<MethodCodeRow>(new MethodCodeRow(methodCd: "CEMNOXR"));
                MpParameters.CurrentMethod = new VwMonitorMethodRow(parameterCd: "NOTNOX", methodCd: "CEMNOXR", endDate: DateTime.Today);

                // Init Output
                category.CheckCatalogResult = null;
                MpParameters.MethodMethodCodeValid = null;
                MpParameters.MethodSubstituteDataCodeValid = null;

                // Run Checks
                actual = target.METHOD8(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual("E", category.CheckCatalogResult, "Result");
                Assert.AreEqual(false, MpParameters.MethodMethodCodeValid, "MethodMethodCodeValid");
                Assert.AreEqual(false, MpParameters.MethodSubstituteDataCodeValid, "MethodSubstituteDataCodeValid");
            }

            //Test 3 for not D
            {
                // Init Input
                MpParameters.EcmpsMpBeginDate = DateTime.Today;
                MpParameters.MethodCodeLookupTable = new CheckDataView<MethodCodeRow>(new MethodCodeRow(methodCd: "CEMNOXR"));
                MpParameters.CurrentMethod = new VwMonitorMethodRow(parameterCd: "NOX", methodCd: "CEMNOXR", endDate: DateTime.Today.AddDays(-1));

                // Init Output
                category.CheckCatalogResult = null;
                MpParameters.MethodMethodCodeValid = null;
                MpParameters.MethodSubstituteDataCodeValid = null;

                // Run Checks
                actual = target.METHOD8(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual(true, MpParameters.MethodMethodCodeValid, "MethodMethodCodeValid");
                Assert.AreEqual(true, MpParameters.MethodSubstituteDataCodeValid, "MethodSubstituteDataCodeValid");
            }

        }

        /// <summary>
        ///A test for METHOD8_ResultC2
        ///</summary>()
        [TestMethod()]
        public void METHOD8_ResultC2()
        {
            cMethodChecks target = new cMethodChecks(new cMpCheckParameters());
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            MpParameters.Init(category.Process);
            MpParameters.Category = category;

            // Variables
            bool log = false;
            string actual;
            bool testTrue;
            bool methodSubstituteDataCodeValidTest;

            // Init Input
            MpParameters.EcmpsMpBeginDate = null;
            string[] locationTestList = { "CS", "MS", "CP", "MP", "U", "LOCBAD" };
            string[] methodTestList = { "ADSTART", "EXP", "AE", "FSA", "LTFF", "MHHI", "CALC", "XCALCX", "LME", "CEM", "XCEMX", "NOXR", "F23", "ST", "BAD" };

            foreach (string locTest in locationTestList)
            {
                foreach (string methodTest in methodTestList)
                {
                    //Init Input
                    MpParameters.LocationType = locTest;
                    MpParameters.CurrentMethod = new VwMonitorMethodRow(parameterCd: "PARAMGOOD", methodCd: methodTest);
                    MpParameters.ParameterToMethodCrossCheckTable = new CheckDataView<MethodParameterToMethodToSystemTypeRow>(new MethodParameterToMethodToSystemTypeRow(methodParameterCode: "PARAMGOOD", methodCode: methodTest));
                    MpParameters.MethodCodeLookupTable = new CheckDataView<MethodCodeRow>(new MethodCodeRow(methodCd: methodTest));

                    switch (MpParameters.LocationType)
                    {
                        case "CS":
                            testTrue = (methodTest.InList("ADSTART,EXP,AE,FSA,LTFF,MHHI,LME") || methodTest.StartsWith("AD"));
                            methodSubstituteDataCodeValidTest = true;
                            break;
                        case "MS":
                            testTrue = (methodTest.InList("EXP,AE,FSA,LTFF,MHHI,CALC,LME") || methodTest.StartsWith("AD"));
                            methodSubstituteDataCodeValidTest = true;
                            break;
                        case "CP":
                            testTrue = (methodTest.InList("EXP,LME,MHHI,NOXR,AE,F23,ST") || methodTest.Contains("CEM") || methodTest.Contains("CALC"));
                            methodSubstituteDataCodeValidTest = true;
                            break;
                        case "MP":
                            testTrue = (methodTest.InList("EXP,LME,MHHI,LTFF,F23,ST") || methodTest.Contains("CEM") || methodTest.Contains("CALC"));
                            methodSubstituteDataCodeValidTest = true;
                            break;
                        case "U":
                            testTrue = methodTest.Contains("CALC");
                            methodSubstituteDataCodeValidTest = !methodTest.Contains("CALC");
                            break;
                        default:
                            testTrue = false;
                            methodSubstituteDataCodeValidTest = true;
                            break;
                    }

                    //Test for C
                    {
                        // Init Output
                        category.CheckCatalogResult = null;
                        MpParameters.MethodMethodCodeValid = null;
                        MpParameters.MethodSubstituteDataCodeValid = null;

                        // Run Checks
                        actual = target.METHOD8(category, ref log);
                    }
                    if (testTrue)
                    {
                        // Check testTrue Results
                        Assert.AreEqual(string.Empty, actual);
                        Assert.AreEqual(false, log);
                        Assert.AreEqual("C", category.CheckCatalogResult, "Result");
                        Assert.AreEqual(false, MpParameters.MethodMethodCodeValid, "MethodMethodCodeValid");
                        Assert.AreEqual(methodSubstituteDataCodeValidTest, MpParameters.MethodSubstituteDataCodeValid, "MethodSubstituteDataCodeValid");
                    }
                    else
                    {
                        // Check test failed Results
                        Assert.AreEqual(string.Empty, actual);
                        Assert.AreEqual(false, log);
                        Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                        Assert.AreEqual(true, MpParameters.MethodMethodCodeValid, "MethodMethodCodeValid");
                        Assert.AreEqual(methodSubstituteDataCodeValidTest, MpParameters.MethodSubstituteDataCodeValid, "MethodSubstituteDataCodeValid");
                    }
                }
            }
        }

        /// <summary>
        ///A test for METHOD8_ResultE
        ///</summary>()
        [TestMethod()]
        public void METHOD8_ResultE()
        {
            cMethodChecks target = new cMethodChecks(new cMpCheckParameters());
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            MpParameters.Init(category.Process);
            MpParameters.Category = category;

            // Variables
            bool log = false;
            string actual;

            // Init Input
            MpParameters.ParameterToMethodCrossCheckTable = new CheckDataView<MethodParameterToMethodToSystemTypeRow>(new MethodParameterToMethodToSystemTypeRow(methodParameterCode: "PARAMGOOD", methodCode: "METHODGOOD"));
            MpParameters.LocationType = "U";
            MpParameters.EcmpsMpBeginDate = DateTime.Today;

            //Test 1 for E
            {
                MpParameters.MethodCodeLookupTable = new CheckDataView<MethodCodeRow>(new MethodCodeRow(methodCd: "METHODGOOD"));
                MpParameters.CurrentMethod = new VwMonitorMethodRow(parameterCd: "PARAMBAD", methodCd: "METHODGOOD");

                // Init Output
                category.CheckCatalogResult = null;
                MpParameters.MethodMethodCodeValid = null;
                MpParameters.MethodSubstituteDataCodeValid = null;

                // Run Checks
                actual = target.METHOD8(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual("E", category.CheckCatalogResult, "Result");
                Assert.AreEqual(false, MpParameters.MethodMethodCodeValid, "MethodMethodCodeValid");
                Assert.AreEqual(false, MpParameters.MethodSubstituteDataCodeValid, "MethodSubstituteDataCodeValid");
            }

            //Test 2 for E
            {
                MpParameters.MethodCodeLookupTable = new CheckDataView<MethodCodeRow>(new MethodCodeRow(methodCd: "METHODBAD"));
                MpParameters.CurrentMethod = new VwMonitorMethodRow(parameterCd: "PARAMGOOD", methodCd: "METHODBAD");

                // Init Output
                category.CheckCatalogResult = null;
                MpParameters.MethodMethodCodeValid = null;
                MpParameters.MethodSubstituteDataCodeValid = null;

                // Run Checks
                actual = target.METHOD8(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual("E", category.CheckCatalogResult, "Result");
                Assert.AreEqual(false, MpParameters.MethodMethodCodeValid, "MethodMethodCodeValid");
                Assert.AreEqual(false, MpParameters.MethodSubstituteDataCodeValid, "MethodSubstituteDataCodeValid");
            }

            //Test for not E
            {
                MpParameters.CurrentMethod = new VwMonitorMethodRow(parameterCd: "PARAMGOOD", methodCd: "METHODGOOD");
                MpParameters.MethodCodeLookupTable = new CheckDataView<MethodCodeRow>(new MethodCodeRow(methodCd: "METHODGOOD"));

                // Init Output
                category.CheckCatalogResult = null;
                MpParameters.MethodMethodCodeValid = null;
                MpParameters.MethodSubstituteDataCodeValid = null;

                // Run Checks
                actual = target.METHOD8(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual(true, MpParameters.MethodMethodCodeValid, "MethodMethodCodeValid");
                Assert.AreEqual(true, MpParameters.MethodSubstituteDataCodeValid, "MethodSubstituteDataCodeValid");
            }
        }


        #endregion

        #region Method-9

        /// <summary>
        ///A test for METHOD9_RemovedHGM
        ///</summary>()
        [TestMethod()]
        public void METHOD9_RemovedHGM()
        {
            cMethodChecks target = new cMethodChecks(new cMpCheckParameters());
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            MpParameters.Init(category.Process);
            MpParameters.Category = category;

            // Variables
            bool log = false;
            string actual;

            // Init Input
            MpParameters.MethodMethodCodeValid = true;
            MpParameters.MethodSubstituteDataCodeValid = true;
            MpParameters.MethodParameterValid = true; // ?? not listed as a required parameter

            //Parameter: HGM, SubstituteDataCd: null
            {
                //Init Input
                MpParameters.CurrentMethod = new VwMonitorMethodRow(parameterCd: "HGM", methodCd: "METHODGOOD");
                MpParameters.SubstituteDataCodeLookupTable = new CheckDataView<SubstituteDataCodeRow>();
                MpParameters.MethodToSubstituteDataCodeCrossCheckTable = new CheckDataView<MethodToSubstituteDataCodeRow>();

                // Init Output
                category.CheckCatalogResult = null;

                // Run Checks
                actual = target.METHOD9(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual(true, MpParameters.MethodSubstituteDataCodeValid, "MethodSubstituteDataCodeValid");
            }

            //Parameter: HGM, SubstituteDataCd: SPTS
            {
                //Init Input
                MpParameters.CurrentMethod = new VwMonitorMethodRow(parameterCd: "HGM", methodCd: "METHODGOOD", subDataCd: "NOTSPTS");
                MpParameters.SubstituteDataCodeLookupTable = new CheckDataView<SubstituteDataCodeRow>(new SubstituteDataCodeRow(subDataCd: "NOTSPTS"));
                MpParameters.MethodToSubstituteDataCodeCrossCheckTable = new CheckDataView<MethodToSubstituteDataCodeRow>
                    (new MethodToSubstituteDataCodeRow(methodCode: "METHODGOOD", substituteDataCode: "NOTSPTS"));


                // Init Output
                category.CheckCatalogResult = null;

                // Run Checks
                actual = target.METHOD9(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual(true, MpParameters.MethodSubstituteDataCodeValid, "MethodSubstituteDataCodeValid");
            }
			        }

		/// <summary>
		///A test for METHOD9_NullSubstituteCd
        ///</summary>()
		[TestMethod()]
		public void METHOD9_NullSubstituteCd()
		{
			cMethodChecks target = new cMethodChecks(new cMpCheckParameters());
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			MpParameters.Init(category.Process);
			MpParameters.Category = category;

			// Variables
			bool log = false;
			string actual;

			// Init Input
			MpParameters.MethodMethodCodeValid = true;
			MpParameters.MethodSubstituteDataCodeValid = true;
			MpParameters.MethodParameterValid = true; // ?? not listed as a required parameter
			string[] parameterTestList = { "HGRE", "HGRH", "HCLRE", "HCLRH", "HFRE", "HFRH", "SO2RE", "SO2RH" };

			//Parameter: HGRE, SubstituteDataCd: null
			{
				//Init Input
				MpParameters.SubstituteDataCodeLookupTable = new CheckDataView<SubstituteDataCodeRow>();
				MpParameters.MethodToSubstituteDataCodeCrossCheckTable = new CheckDataView<MethodToSubstituteDataCodeRow>();
				foreach (string parameterTestCode in parameterTestList)
				{
					MpParameters.CurrentMethod = new VwMonitorMethodRow(parameterCd: parameterTestCode, methodCd: "METHODGOOD");

					// Init Output
					category.CheckCatalogResult = null;

					// Run Checks
					actual = target.METHOD9(category, ref log);

					// Check Results
					Assert.AreEqual(string.Empty, actual);
					Assert.AreEqual(false, log);
					Assert.AreEqual(null, category.CheckCatalogResult, "Result");
					Assert.AreEqual(true, MpParameters.MethodSubstituteDataCodeValid, "MethodSubstituteDataCodeValid");
				}
			}
		}

		/// <summary>
		///A test for METHOD9_MATSParameters
        ///</summary>()
		[TestMethod()]
		public void METHOD9_MATSParameters()
		{
			cMethodChecks target = new cMethodChecks(new cMpCheckParameters());
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			MpParameters.Init(category.Process);
			MpParameters.Category = category;

			// Variables
			bool log = false;
			string actual;

			// Init Input
			MpParameters.MethodMethodCodeValid = true;
			MpParameters.MethodSubstituteDataCodeValid = true;
			MpParameters.MethodParameterValid = true; // ?? not listed as a required parameter
			string[] parameterTestList = { "HGRE", "HGRH", "HCLRE", "HCLRH", "HFRE", "HFRH", "SO2RE", "SO2RH" };

			//Parameter: MATS, SubstituteDataCd: notnull
			{
				//Init Input
				MpParameters.SubstituteDataCodeLookupTable = new CheckDataView<SubstituteDataCodeRow>();
				MpParameters.MethodToSubstituteDataCodeCrossCheckTable = new CheckDataView<MethodToSubstituteDataCodeRow>();

				foreach (string parameterTestCode in parameterTestList)
				{
					MpParameters.CurrentMethod = new VwMonitorMethodRow(parameterCd: parameterTestCode, methodCd: "METHODGOOD", subDataCd: "NOTNULL");

					// Init Output
					category.CheckCatalogResult = null;

					// Run Checks
					actual = target.METHOD9(category, ref log);

					// Check Results
					Assert.AreEqual(string.Empty, actual);
					Assert.AreEqual(false, log);
					Assert.AreEqual("C", category.CheckCatalogResult, "Result");
					Assert.AreEqual(false, MpParameters.MethodSubstituteDataCodeValid, "MethodSubstituteDataCodeValid");
				}
			}
		}
        #endregion

        #region Method-10

        /// <summary>
        ///A test for METHOD10
        ///</summary>()
        [TestMethod()]
        public void METHOD10()
        {
            cMethodChecks target = new cMethodChecks(new cMpCheckParameters());
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            MpParameters.Init(category.Process);
            MpParameters.Category = category;

            // Variables
            bool log = false;
            string actual;

            // Init Input
            MpParameters.BypassApproachCodeLookupTable = new CheckDataView<BypassApproachCodeRow>(new BypassApproachCodeRow(bypassApproachCd: "BAGOOD"));

            //BypassApproach null
            {
                //Init Input
                MpParameters.MethodMethodCodeValid = true;
                MpParameters.MethodParameterValid = true;
                MpParameters.CurrentMethod = new VwMonitorMethodRow(methodCd: "METHODGOOD");

                // Init Output
                category.CheckCatalogResult = null;

                // Run Checks
                actual = target.METHOD10(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual(true, MpParameters.MethodBypassApproachCodeValid, "MethodBypassApproachCodeValid");
            }

            //BypassApproach not found
            {
                //Init Input
                MpParameters.MethodMethodCodeValid = true;
                MpParameters.MethodParameterValid = true;
                MpParameters.CurrentMethod = new VwMonitorMethodRow(methodCd: "METHODGOOD", bypassApproachCd: "BABAD");

                // Init Output
                category.CheckCatalogResult = null;

                // Run Checks
                actual = target.METHOD10(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual("A", category.CheckCatalogResult, "Result");
                Assert.AreEqual(false, MpParameters.MethodBypassApproachCodeValid, "MethodBypassApproachCodeValid");
            }

            //Codes not valid
            {
                //Init Input
                MpParameters.MethodMethodCodeValid = false;
                MpParameters.MethodParameterValid = false;
                MpParameters.CurrentMethod = new VwMonitorMethodRow(methodCd: "METHODGOOD", bypassApproachCd: "BAGOOD");

                // Init Output
                category.CheckCatalogResult = null;

                // Run Checks
                actual = target.METHOD10(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual(true, MpParameters.MethodBypassApproachCodeValid, "MethodBypassApproachCodeValid");
            }

            //Method Parameter valid and in list
            {
                //Init Input
                string[] testParameters = { "SO2", "NOX", "NOXR", "PARAMBAD" };
                bool testTrue;
                MpParameters.MethodMethodCodeValid = false;
                MpParameters.MethodParameterValid = true;

                foreach (string testParam in testParameters)
                {
                    MpParameters.CurrentMethod = new VwMonitorMethodRow(methodCd: "METHODGOOD", parameterCd: testParam, bypassApproachCd: "BAGOOD");
                    if (testParam.InList("SO2,NOX,NOXR"))
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
                    actual = target.METHOD10(category, ref log);

                    // Check Results
                    if (testTrue)
                    {
                        Assert.AreEqual(string.Empty, actual);
                        Assert.AreEqual(false, log);
                        Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                        Assert.AreEqual(true, MpParameters.MethodBypassApproachCodeValid, "MethodBypassApproachCodeValid");
                    }
                    else
                    {
                        Assert.AreEqual(string.Empty, actual);
                        Assert.AreEqual(false, log);
                        Assert.AreEqual("B", category.CheckCatalogResult, "Result");
                        Assert.AreEqual(false, MpParameters.MethodBypassApproachCodeValid, "MethodBypassApproachCodeValid");
                    }
                }
            }

            //MethodMethodCode valid and in list
            {
                //Init Input
                string[] testMethodMethods = { "AMS", "NOXR", "CEM", "CEMF23", "METHODBAD" };
                bool testTrue;
                MpParameters.MethodMethodCodeValid = true;
                MpParameters.MethodParameterValid = false;

                foreach (string testMethod in testMethodMethods)
                {
                    MpParameters.CurrentMethod = new VwMonitorMethodRow(methodCd: testMethod, bypassApproachCd: "BAGOOD");
                    if (testMethod.InList("AMS,NOXR,CEM,CEMF23"))
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
                    actual = target.METHOD10(category, ref log);

                    // Check Results
                    if (testTrue)
                    {
                        Assert.AreEqual(string.Empty, actual);
                        Assert.AreEqual(false, log);
                        Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                        Assert.AreEqual(true, MpParameters.MethodBypassApproachCodeValid, "MethodBypassApproachCodeValid");
                    }
                    else
                    {
                        Assert.AreEqual(string.Empty, actual);
                        Assert.AreEqual(false, log);
                        Assert.AreEqual("B", category.CheckCatalogResult, "Result");
                        Assert.AreEqual(false, MpParameters.MethodBypassApproachCodeValid, "MethodBypassApproachCodeValid");
                    }
                }
            }

        }
        #endregion

        #region Method-23

        /// <summary>
        ///A test for METHOD23_RemoveHGM
        ///</summary>()
        [TestMethod()]
        public void METHOD23_RemoveHGM()
        {
            cMethodChecks target = new cMethodChecks(new cMpCheckParameters());
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            MpParameters.Init(category.Process);
            MpParameters.Category = category;

            // Variables
            bool log = false;
            string actual;

            // Init Input
            MpParameters.FacilityMethodRecords = new CheckDataView<VwMonitorMethodRow>
                (new VwMonitorMethodRow(monLocId: "LOC1", parameterCd: "NOTNULL", beginDate: DateTime.Today.AddDays(-1), methodCd: "AMS", beginHour: 0, endDate: DateTime.Today, endHour: 23));
            MpParameters.LocationType = "CP";
            MpParameters.MethodDatesAndHoursConsistent = true;
            MpParameters.MethodEvaluationBeginDate = DateTime.Today.AddDays(-1);
            MpParameters.MethodEvaluationBeginHour = 0;
            MpParameters.MethodEvaluationEndDate = DateTime.Today;
            MpParameters.MethodEvaluationEndHour = 0;
            MpParameters.MethodMethodCodeValid = true;
            MpParameters.UnitStackConfigurationRecords = new CheckDataView<VwUnitStackConfigurationRow>
                (new VwUnitStackConfigurationRow(beginDate: DateTime.Today.AddDays(-1), stackPipeMonLocId: "LOC1"));

            //ParameterCode HGM
            {
                //Init Input
                MpParameters.CurrentMethod = new VwMonitorMethodRow(methodCd: "LME", parameterCd: "HGM", monLocId: "LOC1");

                // Init Output
                category.CheckCatalogResult = null;

                // Run Checks
                actual = target.METHOD23(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual("A", category.CheckCatalogResult, "Result");
            }

            //ParameterCode null
            {
                //Init Input
                MpParameters.CurrentMethod = new VwMonitorMethodRow(methodCd: "LME", monLocId: "LOC1");

                // Init Output
                category.CheckCatalogResult = null;

                // Run Checks
                actual = target.METHOD23(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual("A", category.CheckCatalogResult, "Result");
            }
        }
        #endregion

        #region Method-26

        /// <summary>
        ///A test for METHOD26_AddMATS
        ///</summary>()
        [TestMethod()]
        public void METHOD26_AddMATS()
        {
            cMethodChecks target = new cMethodChecks(new cMpCheckParameters());
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            MpParameters.Init(category.Process);
            MpParameters.Category = category;

            // Variables
            bool log = false;
            string actual;

            // Init Input
            MpParameters.MethodEvaluationBeginDate = DateTime.Today.AddDays(-1);
            MpParameters.MethodEvaluationBeginHour = 0;
            MpParameters.MethodEvaluationEndDate = DateTime.Today;
            MpParameters.MethodEvaluationEndHour = 0;
            MpParameters.MethodMethodCodeValid = true;
            MpParameters.MethodParameterValid = true;
            MpParameters.MonitorSystemRecords = new CheckDataView<VwMonitorSystemRow>();
            MpParameters.MethodDatesAndHoursConsistent = true;

            //ParameterCode and MethodCode
            {
                string[] testMethods = { "METHODGOOD", "XXCEMXX", "CALC" };
                string[] testParameters = {"SO2", "HI", "CO2", "NOX",  "HGRE", "HGRH", "HCLRE", "HCLRH", "HFRE", "HFRH", "SO2RE", "SO2RH", "HGM"};
                bool testTrue;

                foreach (string testMethod in testMethods)
                {
                    foreach (string testParam in testParameters)
                    {
                        //Init Input
                        MpParameters.CurrentMethod = new VwMonitorMethodRow(methodCd: testMethod, parameterCd: testParam);

                        if ((testMethod == "XXCEMXX" && testParam.InList("SO2,HI,CO2,NOX")) || (testMethod != "CALC" && testParam.InList("HGRE,HCLRE,HFRE,SO2RE")))
                            testTrue = true;
                        else
                            testTrue = false;

                        // Init Output
                        category.CheckCatalogResult = null;

                        // Run Checks
                        actual = target.METHOD26(category, ref log);

                        if (testTrue == true)
                        {
                            // Check Results
                            Assert.AreEqual(string.Empty, actual);
                            Assert.AreEqual(false, log);
                            Assert.AreEqual("A", category.CheckCatalogResult, String.Format("Result: {0}/{1}", testParam, testMethod));
                        }
                        else
                        {
                            // Check Results
                            Assert.AreEqual(string.Empty, actual);
                            Assert.AreEqual(false, log);
                            Assert.AreEqual(null, category.CheckCatalogResult, String.Format("Result: {0}/{1}", testParam, testMethod));
                        }

                    }
                }
            }
        }
        #endregion

        #region Method-27

        /// <summary>
        ///A test for METHOD27_MethodValid
        ///</summary>()
        [TestMethod()]
        public void METHOD27_MethodValid()
        {
            cMethodChecks target = new cMethodChecks(new cMpCheckParameters());
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            MpParameters.Init(category.Process);
            MpParameters.Category = category;

            // Variables
            bool log = false;
            string actual;
            string missingFormulaForMethodTest;

            // Init Input
            MpParameters.CurrentMethod = new VwMonitorMethodRow(parameterCd: "PARAMGOOD", methodCd: "METHODGOOD");
            MpParameters.EcmpsMpBeginDate = null;
            MpParameters.FormulaRecords = new CheckDataView<VwMonitorFormulaRow>(); //null
            MpParameters.LocationType = "U";
            MpParameters.MethodEvaluationBeginDate = DateTime.Today.AddDays(-1);
            MpParameters.MethodEvaluationBeginHour = 0;
            MpParameters.MethodEvaluationEndDate = DateTime.Today;
            MpParameters.MethodEvaluationEndHour = 0;
            MpParameters.ParameterAndMethodAndLocationToFormulaCrosscheck = new CheckDataView<ParameterMethodToFormulaRow>(new ParameterMethodToFormulaRow(parameterCd: "PARAMGOOD", methodCd: "METHODGOOD", locationTypeList: null, systemTypeList: null, formulaList: null, notFoundResult: "Z"));
            MpParameters.MonitorSystemRecords = new CheckDataView<VwMonitorSystemRow>(); //null
            MpParameters.UnitStackConfigurationRecords = new CheckDataView<VwUnitStackConfigurationRow>(); //null

            missingFormulaForMethodTest = MpParameters.CurrentMethod.ParameterCd + " ";

            //Test for Valid
            {
            MpParameters.MethodParameterValid = true;
            MpParameters.MethodMethodCodeValid = true;
            MpParameters.MethodDatesAndHoursConsistent = true;

                // Init Output
                category.CheckCatalogResult = null;
                MpParameters.MissingFormulaForMethod = null;

                // Run Checks
                actual = target.METHOD27(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual("Z", category.CheckCatalogResult, "Result");
                Assert.AreEqual(missingFormulaForMethodTest, MpParameters.MissingFormulaForMethod, "MissingFormulaForMethod");
            }

            //Test 1 for Not Valid
            {
                MpParameters.MethodParameterValid = false;
                MpParameters.MethodMethodCodeValid = true;
                MpParameters.MethodDatesAndHoursConsistent = true;

                // Init Output
                category.CheckCatalogResult = null;
                MpParameters.MissingFormulaForMethod = null;

                // Run Checks
                actual = target.METHOD27(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual(null, MpParameters.MissingFormulaForMethod, "MissingFormulaForMethod");
            }
            //Test 2 for Not Valid
            {
                MpParameters.MethodParameterValid = true;
                MpParameters.MethodMethodCodeValid = false;
                MpParameters.MethodDatesAndHoursConsistent = true;

                // Init Output
                category.CheckCatalogResult = null;
                MpParameters.MissingFormulaForMethod = null;

                // Run Checks
                actual = target.METHOD27(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual(null, MpParameters.MissingFormulaForMethod, "MissingFormulaForMethod");
            }
            //Test 3 for Not Valid
            {
                MpParameters.MethodParameterValid = true;
                MpParameters.MethodMethodCodeValid = true;
                MpParameters.MethodDatesAndHoursConsistent = false;

                // Init Output
                category.CheckCatalogResult = null;
                MpParameters.MissingFormulaForMethod = null;

                // Run Checks
                actual = target.METHOD27(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual(null, MpParameters.MissingFormulaForMethod, "MissingFormulaForMethod");
            }
        }

        /// <summary>
        ///A test for METHOD27_CrossCheck
        ///</summary>()
        [TestMethod()]
        public void METHOD27_CrossCheck()
        {
            cMethodChecks target = new cMethodChecks(new cMpCheckParameters());
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            MpParameters.Init(category.Process);
            MpParameters.Category = category;

            // Variables
            bool log = false;
            string actual;
            string missingFormulaForMethodTest;

            // Init Input
            MpParameters.EcmpsMpBeginDate = null;
            MpParameters.FormulaRecords = new CheckDataView<VwMonitorFormulaRow>(); //null
            MpParameters.MethodEvaluationBeginDate = DateTime.Today.AddDays(-1);
            MpParameters.MethodEvaluationBeginHour = 0;
            MpParameters.MethodEvaluationEndDate = DateTime.Today;
            MpParameters.MethodEvaluationEndHour = 0;
            MpParameters.MethodParameterValid = true;
            MpParameters.MethodMethodCodeValid = true;
            MpParameters.MethodDatesAndHoursConsistent = true;
            MpParameters.MonitorSystemRecords = new CheckDataView<VwMonitorSystemRow>(); //null
            MpParameters.UnitStackConfigurationRecords = new CheckDataView<VwUnitStackConfigurationRow>(); //null


            //Test for Found with null LocationList
            {
                MpParameters.CurrentMethod = new VwMonitorMethodRow(parameterCd: "PARAMGOOD", methodCd: "METHODGOOD");
                MpParameters.LocationType = "LOC1";
                MpParameters.ParameterAndMethodAndLocationToFormulaCrosscheck = new CheckDataView<ParameterMethodToFormulaRow>
                    (new ParameterMethodToFormulaRow(parameterCd: "PARAMGOOD", methodCd: "METHODGOOD", locationTypeList: null, systemTypeList: null, formulaList: null, notFoundResult: "Z"));
                missingFormulaForMethodTest = MpParameters.CurrentMethod.ParameterCd + " ";

                // Init Output
                category.CheckCatalogResult = null;
                MpParameters.MissingFormulaForMethod = null;

                // Run Checks
                actual = target.METHOD27(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual("Z", category.CheckCatalogResult, "Result");
                Assert.AreEqual(missingFormulaForMethodTest, MpParameters.MissingFormulaForMethod, "MissingFormulaForMethod");
            }

            //Test for Found with valid Location
            {
                MpParameters.CurrentMethod = new VwMonitorMethodRow(parameterCd: "PARAMGOOD", methodCd: "METHODGOOD");
                MpParameters.LocationType = "LOC1";
                MpParameters.ParameterAndMethodAndLocationToFormulaCrosscheck = new CheckDataView<ParameterMethodToFormulaRow>
                    (new ParameterMethodToFormulaRow(parameterCd: "PARAMGOOD", methodCd: "METHODGOOD", locationTypeList: "LOC1,LOC2", systemTypeList: null, formulaList: null, notFoundResult: "Z"));
                missingFormulaForMethodTest = MpParameters.CurrentMethod.ParameterCd + " ";

                // Init Output
                category.CheckCatalogResult = null;
                MpParameters.MissingFormulaForMethod = null;

                // Run Checks
                actual = target.METHOD27(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual("Z", category.CheckCatalogResult, "Result");
                Assert.AreEqual(missingFormulaForMethodTest, MpParameters.MissingFormulaForMethod, "MissingFormulaForMethod");
            }

            //Test for Not Found: invalid Location
            {
                MpParameters.CurrentMethod = new VwMonitorMethodRow(parameterCd: "PARAMGOOD", methodCd: "METHODGOOD");
                MpParameters.LocationType = "LOC3";
                MpParameters.ParameterAndMethodAndLocationToFormulaCrosscheck = new CheckDataView<ParameterMethodToFormulaRow>
                    (new ParameterMethodToFormulaRow(parameterCd: "PARAMGOOD", methodCd: "METHODGOOD", locationTypeList: "LOC1,LOC2", systemTypeList: null, formulaList: null, notFoundResult: "Z"));
                missingFormulaForMethodTest = MpParameters.CurrentMethod.ParameterCd + " ";

                // Init Output
                category.CheckCatalogResult = null;
                MpParameters.MissingFormulaForMethod = null;

                // Run Checks
                actual = target.METHOD27(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual(null, MpParameters.MissingFormulaForMethod, "MissingFormulaForMethod");
            }

            //Test for Not Found: invalid ParameterCode
            {
                MpParameters.CurrentMethod = new VwMonitorMethodRow(parameterCd: "PARAMBAD", methodCd: "METHODGOOD");
                MpParameters.LocationType = "LOC1";
                MpParameters.ParameterAndMethodAndLocationToFormulaCrosscheck = new CheckDataView<ParameterMethodToFormulaRow>
                    (new ParameterMethodToFormulaRow(parameterCd: "PARAMGOOD", methodCd: "METHODGOOD", locationTypeList: "LOC1,LOC2", systemTypeList: null, formulaList: null, notFoundResult: "Z"));
                missingFormulaForMethodTest = MpParameters.CurrentMethod.ParameterCd + " ";

                // Init Output
                category.CheckCatalogResult = null;
                MpParameters.MissingFormulaForMethod = null;

                // Run Checks
                actual = target.METHOD27(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual(null, MpParameters.MissingFormulaForMethod, "MissingFormulaForMethod");
            }

            //Test for Not Found: invalid MethodCode
            {
                MpParameters.CurrentMethod = new VwMonitorMethodRow(parameterCd: "PARAMGOOD", methodCd: "METHODBAD");
                MpParameters.LocationType = "LOC1";
                MpParameters.ParameterAndMethodAndLocationToFormulaCrosscheck = new CheckDataView<ParameterMethodToFormulaRow>
                    (new ParameterMethodToFormulaRow(parameterCd: "PARAMGOOD", methodCd: "METHODGOOD", locationTypeList: "LOC1,LOC2", systemTypeList: null, formulaList: null, notFoundResult: "Z"));
                missingFormulaForMethodTest = MpParameters.CurrentMethod.ParameterCd + " ";

                // Init Output
                category.CheckCatalogResult = null;
                MpParameters.MissingFormulaForMethod = null;

                // Run Checks
                actual = target.METHOD27(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual(null, MpParameters.MissingFormulaForMethod, "MissingFormulaForMethod");
            }
        }

        /// <summary>
        ///A test for METHOD27_FindMonitorFormula
        ///</summary>()
        [TestMethod()]
        public void METHOD27_FindMonitorFormula()
        {
            cMethodChecks target = new cMethodChecks(new cMpCheckParameters());
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            MpParameters.Init(category.Process);
            MpParameters.Category = category;

            // Variables
            bool log = false;
            string actual;
            string missingFormulaForMethodTest;

            // Init Input
            MpParameters.CurrentMethod = new VwMonitorMethodRow(parameterCd: "PARAMGOOD", methodCd: "METHODGOOD");
            MpParameters.LocationType = "LOC1";
            MpParameters.ParameterAndMethodAndLocationToFormulaCrosscheck = new CheckDataView<ParameterMethodToFormulaRow>
                (new ParameterMethodToFormulaRow(parameterCd: "PARAMGOOD", methodCd: "METHODGOOD", locationTypeList: null, systemTypeList: null, formulaList: "FORM1,FORM2", notFoundResult: "Z"));
            MpParameters.EcmpsMpBeginDate = null;
            MpParameters.MethodEvaluationBeginDate = DateTime.Today.AddDays(-10);
            MpParameters.MethodEvaluationBeginHour = 0;
            MpParameters.MethodEvaluationEndDate = DateTime.Today;
            MpParameters.MethodEvaluationEndHour = 0;
            MpParameters.MethodParameterValid = true;
            MpParameters.MethodMethodCodeValid = true;
            MpParameters.MethodDatesAndHoursConsistent = true;
            MpParameters.MonitorSystemRecords = new CheckDataView<VwMonitorSystemRow>(); //null
            MpParameters.UnitStackConfigurationRecords = new CheckDataView<VwUnitStackConfigurationRow>(); //null

            //Test 1 for Found: Begin/End date equal
            {
                MpParameters.FormulaRecords = new CheckDataView<VwMonitorFormulaRow>
                    (new VwMonitorFormulaRow(parameterCd: "PARAMGOOD", equationCd: "FORM1", beginDatehour: DateTime.Today.AddDays(-10), endDatehour: DateTime.Today));
                missingFormulaForMethodTest = MpParameters.CurrentMethod.ParameterCd + " ";

                // Init Output
                category.CheckCatalogResult = null;
                MpParameters.MissingFormulaForMethod = null;

                // Run Checks
                actual = target.METHOD27(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual(null, MpParameters.MissingFormulaForMethod, "MissingFormulaForMethod");
            }

            //Test 2 for Found: Begin/End date </>
            {
                MpParameters.FormulaRecords = new CheckDataView<VwMonitorFormulaRow>
                    (new VwMonitorFormulaRow(parameterCd: "PARAMGOOD", equationCd: "FORM1", beginDatehour: DateTime.Today.AddDays(-12), endDatehour: DateTime.Today.AddDays(1)));
                missingFormulaForMethodTest = MpParameters.CurrentMethod.ParameterCd + " ";

                // Init Output
                category.CheckCatalogResult = null;
                MpParameters.MissingFormulaForMethod = null;

                // Run Checks
                actual = target.METHOD27(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual(null, MpParameters.MissingFormulaForMethod, "MissingFormulaForMethod");
            }

            //Test 2 for Found: null dates
            {
                MpParameters.FormulaRecords = new CheckDataView<VwMonitorFormulaRow>
                    (new VwMonitorFormulaRow(parameterCd: "PARAMGOOD", equationCd: "FORM1", beginDatehour: null, endDatehour: null));
                missingFormulaForMethodTest = MpParameters.CurrentMethod.ParameterCd + " ";

                // Init Output
                category.CheckCatalogResult = null;
                MpParameters.MissingFormulaForMethod = null;

                // Run Checks
                actual = target.METHOD27(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual(null, MpParameters.MissingFormulaForMethod, "MissingFormulaForMethod");
            }

            //Test 1 for Not Found: parameter code
            {
                MpParameters.FormulaRecords = new CheckDataView<VwMonitorFormulaRow>
                    (new VwMonitorFormulaRow(parameterCd: "PARAMBAD", equationCd: "FORM1", beginDatehour: null, endDatehour: null));
                missingFormulaForMethodTest = MpParameters.CurrentMethod.ParameterCd + " FORM1,FORM2";

                // Init Output
                category.CheckCatalogResult = null;
                MpParameters.MissingFormulaForMethod = null;

                // Run Checks
                actual = target.METHOD27(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual("Z", category.CheckCatalogResult, "Result");
                Assert.AreEqual(missingFormulaForMethodTest, MpParameters.MissingFormulaForMethod, "MissingFormulaForMethod");
            }

            //Test 2 for Not Found: formula code
            {
                MpParameters.FormulaRecords = new CheckDataView<VwMonitorFormulaRow>
                    (new VwMonitorFormulaRow(parameterCd: "PARAMGOOD", equationCd: "FORM3", beginDatehour: null, endDatehour: null));
                missingFormulaForMethodTest = MpParameters.CurrentMethod.ParameterCd + " FORM1,FORM2";

                // Init Output
                category.CheckCatalogResult = null;
                MpParameters.MissingFormulaForMethod = null;

                // Run Checks
                actual = target.METHOD27(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual("Z", category.CheckCatalogResult, "Result");
                Assert.AreEqual(missingFormulaForMethodTest, MpParameters.MissingFormulaForMethod, "MissingFormulaForMethod");
            }

            //Test 3 for Not Found: begin date
            {
                MpParameters.FormulaRecords = new CheckDataView<VwMonitorFormulaRow>
                    (new VwMonitorFormulaRow(parameterCd: "PARAMGOOD", equationCd: "FORM1", beginDatehour: DateTime.Today.AddDays(1), endDatehour: null));
                missingFormulaForMethodTest = MpParameters.CurrentMethod.ParameterCd + " FORM1,FORM2";

                // Init Output
                category.CheckCatalogResult = null;
                MpParameters.MissingFormulaForMethod = null;

                // Run Checks
                actual = target.METHOD27(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual("Z", category.CheckCatalogResult, "Result");
                Assert.AreEqual(missingFormulaForMethodTest, MpParameters.MissingFormulaForMethod, "MissingFormulaForMethod");
            }

            //Test 4 for Not Found: end date
            {
                MpParameters.FormulaRecords = new CheckDataView<VwMonitorFormulaRow>
                    (new VwMonitorFormulaRow(parameterCd: "PARAMGOOD", equationCd: "FORM1", beginDatehour: null, endDatehour: DateTime.Today.AddDays(-11)));
                missingFormulaForMethodTest = MpParameters.CurrentMethod.ParameterCd + " FORM1,FORM2";

                // Init Output
                category.CheckCatalogResult = null;
                MpParameters.MissingFormulaForMethod = null;

                // Run Checks
                actual = target.METHOD27(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual("Z", category.CheckCatalogResult, "Result");
                Assert.AreEqual(missingFormulaForMethodTest, MpParameters.MissingFormulaForMethod, "MissingFormulaForMethod");
            }

            //Test for Span true - span check uses date/hour separated
            {
                MpParameters.FormulaRecords = new CheckDataView<VwMonitorFormulaRow>
                    (new VwMonitorFormulaRow(parameterCd: "PARAMGOOD", equationCd: "FORM1", beginDatehour: DateTime.Today.AddDays(-10), beginDate: DateTime.Today.AddDays(-10), beginHour: 0, endDatehour: DateTime.Today.AddDays(-5), endDate: DateTime.Today.AddDays(-5), endHour: 0),
                    new VwMonitorFormulaRow(parameterCd: "PARAMGOOD", equationCd: "FORM1", beginDatehour: DateTime.Today.AddDays(-5), beginDate: DateTime.Today.AddDays(-5), beginHour: 0, endDatehour: DateTime.Today, endDate: DateTime.Today, endHour: 0));

                missingFormulaForMethodTest = MpParameters.CurrentMethod.ParameterCd + " ";

                // Init Output
                category.CheckCatalogResult = null;
                MpParameters.MissingFormulaForMethod = null;

                // Run Checks
                actual = target.METHOD27(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual(null, MpParameters.MissingFormulaForMethod, "MissingFormulaForMethod");
            }

            //Test for Span false - span check uses date/hour separated
            {
                MpParameters.FormulaRecords = new CheckDataView<VwMonitorFormulaRow>
                    (new VwMonitorFormulaRow(parameterCd: "PARAMGOOD", equationCd: "FORM1", beginDatehour: DateTime.Today.AddDays(-10), beginDate: DateTime.Today.AddDays(-10), beginHour: 0, endDatehour: DateTime.Today.AddDays(-5), endDate: DateTime.Today.AddDays(-5), endHour: 0),
                    new VwMonitorFormulaRow(parameterCd: "PARAMGOOD", equationCd: "FORM1", beginDatehour: DateTime.Today.AddDays(-3), beginDate: DateTime.Today.AddDays(-3), beginHour: 0, endDatehour: DateTime.Today, endDate: DateTime.Today, endHour: 0));

                missingFormulaForMethodTest = MpParameters.CurrentMethod.ParameterCd + " FORM1,FORM2";

                // Init Output
                category.CheckCatalogResult = null;
                MpParameters.MissingFormulaForMethod = null;

                // Run Checks
                actual = target.METHOD27(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual("B", category.CheckCatalogResult, "Result");
                Assert.AreEqual(missingFormulaForMethodTest, MpParameters.MissingFormulaForMethod, "MissingFormulaForMethod");
            }
        }

        /// <summary>
        ///A test for METHOD27_MonitorSystem
        ///</summary>()
        [TestMethod()]
        public void METHOD27_MonitorSystem()
        {
            cMethodChecks target = new cMethodChecks(new cMpCheckParameters());
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            MpParameters.Init(category.Process);
            MpParameters.Category = category;

            // Variables
            bool log = false;
            string actual;
            string missingFormulaForMethodTest;

            // Init Input
            MpParameters.CurrentMethod = new VwMonitorMethodRow(parameterCd: "PARAMGOOD", methodCd: "METHODGOOD");
            MpParameters.LocationType = "LOC1";
            MpParameters.MethodEvaluationBeginDate = DateTime.Today.AddDays(-10);
            MpParameters.MethodEvaluationBeginHour = 0;
            MpParameters.MethodEvaluationEndDate = DateTime.Today;
            MpParameters.MethodEvaluationEndHour = 0;
            MpParameters.MethodParameterValid = true;
            MpParameters.MethodMethodCodeValid = true;
            MpParameters.MethodDatesAndHoursConsistent = true;
            MpParameters.UnitStackConfigurationRecords = new CheckDataView<VwUnitStackConfigurationRow>(); //null

            //MonitorFormula Not Found (ECMPS earlier); also tests MonitorSystem found
            {
                MpParameters.EcmpsMpBeginDate = DateTime.Today.AddYears(-5);
                MpParameters.ParameterAndMethodAndLocationToFormulaCrosscheck = new CheckDataView<ParameterMethodToFormulaRow>
                    (new ParameterMethodToFormulaRow(parameterCd: "PARAMGOOD", methodCd: "METHODGOOD", locationTypeList: null,
                        systemTypeList: "SYSTEM1, SYSTEM2", ecmpsOnly: "Yes", formulaList: "FORM1,FORM2", notFoundResult: "Z"));
                MpParameters.MonitorSystemRecords = new CheckDataView<VwMonitorSystemRow>
                    (new VwMonitorSystemRow(sysTypeCd: "SYSTEM1", fuelCd: "FUEL1", beginDatehour: DateTime.Today.AddDays(-10), beginDate: DateTime.Today.AddDays(-10), beginHour: 0, endDatehour: DateTime.Today, endDate: DateTime.Today, endHour: 0),
                    new VwMonitorSystemRow(sysTypeCd: "SYSTEM1", fuelCd: "FUEL2", beginDatehour: DateTime.Today.AddDays(-10), beginDate: DateTime.Today.AddDays(-10), beginHour: 0, endDatehour: DateTime.Today, endDate: DateTime.Today, endHour: 0));
                MpParameters.FormulaRecords = new CheckDataView<VwMonitorFormulaRow>(); //null
                missingFormulaForMethodTest = MpParameters.CurrentMethod.ParameterCd + " FORM1,FORM2";

                // Init Output
                category.CheckCatalogResult = null;
                MpParameters.MissingFormulaForMethod = null;

                // Run Checks
                actual = target.METHOD27(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual("Z", category.CheckCatalogResult, "Result");
                Assert.AreEqual("PARAMGOOD FORM1,FORM2", MpParameters.MissingFormulaForMethod, "MissingFormulaForMethod");
            }

            //ECMPS Only later
            {
                MpParameters.EcmpsMpBeginDate = DateTime.Today.AddDays(-5);
                MpParameters.ParameterAndMethodAndLocationToFormulaCrosscheck = new CheckDataView<ParameterMethodToFormulaRow>
                    (new ParameterMethodToFormulaRow(parameterCd: "PARAMGOOD", methodCd: "METHODGOOD", locationTypeList: null,
                        systemTypeList: "SYSTEM1, SYSTEM2", ecmpsOnly: "Yes", formulaList: "FORM1,FORM2", notFoundResult: "Z"));
                MpParameters.MonitorSystemRecords = new CheckDataView<VwMonitorSystemRow>
                    (new VwMonitorSystemRow(sysTypeCd: "SYSTEM1", fuelCd: "FUEL1", beginDatehour: DateTime.Today.AddDays(-10), beginDate: DateTime.Today.AddDays(-10), beginHour: 0, endDatehour: DateTime.Today.AddDays(-8), endDate: DateTime.Today.AddDays(-8), endHour: 0),
                    new VwMonitorSystemRow(sysTypeCd: "SYSTEM1", fuelCd: "FUEL2", beginDatehour: DateTime.Today.AddDays(-10), beginDate: DateTime.Today.AddDays(-10), beginHour: 0, endDatehour: DateTime.Today.AddDays(-8), endDate: DateTime.Today.AddDays(-8), endHour: 0));
                MpParameters.FormulaRecords = new CheckDataView<VwMonitorFormulaRow>(); //null
                missingFormulaForMethodTest = MpParameters.CurrentMethod.ParameterCd + " FORM1,FORM2";

                // Init Output
                category.CheckCatalogResult = null;
                MpParameters.MissingFormulaForMethod = null;

                // Run Checks
                actual = target.METHOD27(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual(null, MpParameters.MissingFormulaForMethod, "MissingFormulaForMethod");
            }

            //ECMPS Only = false (null), ignore EcmpsBeginDate
            {
                MpParameters.EcmpsMpBeginDate = DateTime.Today.AddDays(-5);
                MpParameters.ParameterAndMethodAndLocationToFormulaCrosscheck = new CheckDataView<ParameterMethodToFormulaRow>
                    (new ParameterMethodToFormulaRow(parameterCd: "PARAMGOOD", methodCd: "METHODGOOD", locationTypeList: null,
                        systemTypeList: "SYSTEM1, SYSTEM2", ecmpsOnly: null, formulaList: "FORM1,FORM2", notFoundResult: "Z"));
                MpParameters.MonitorSystemRecords = new CheckDataView<VwMonitorSystemRow>
                    (new VwMonitorSystemRow(sysTypeCd: "SYSTEM1", fuelCd: "FUEL1", beginDatehour: DateTime.Today.AddDays(-10), beginDate: DateTime.Today.AddDays(-10), beginHour: 0, endDatehour: DateTime.Today.AddDays(-8), endDate: DateTime.Today.AddDays(-8), endHour: 0),
                    new VwMonitorSystemRow(sysTypeCd: "SYSTEM1", fuelCd: "FUEL2", beginDatehour: DateTime.Today.AddDays(-10), beginDate: DateTime.Today.AddDays(-10), beginHour: 0, endDatehour: DateTime.Today.AddDays(-8), endDate: DateTime.Today.AddDays(-8), endHour: 0));
                MpParameters.FormulaRecords = new CheckDataView<VwMonitorFormulaRow>(); //null
                missingFormulaForMethodTest = MpParameters.CurrentMethod.ParameterCd + " FORM1,FORM2";

                // Init Output
                category.CheckCatalogResult = null;
                MpParameters.MissingFormulaForMethod = null;

                // Run Checks
                actual = target.METHOD27(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual("Z", category.CheckCatalogResult, "Result");
                Assert.AreEqual(missingFormulaForMethodTest, MpParameters.MissingFormulaForMethod, "MissingFormulaForMethod");
            }

            //MonitorSystem Not Found: SystemTypeCode
            {
                MpParameters.EcmpsMpBeginDate = null;
                MpParameters.ParameterAndMethodAndLocationToFormulaCrosscheck = new CheckDataView<ParameterMethodToFormulaRow>
                    (new ParameterMethodToFormulaRow(parameterCd: "PARAMGOOD", methodCd: "METHODGOOD", locationTypeList: null,
                        systemTypeList: "SYSTEM1, SYSTEM2", ecmpsOnly: null, formulaList: "FORM1,FORM2", notFoundResult: "Z"));
                MpParameters.MonitorSystemRecords = new CheckDataView<VwMonitorSystemRow>
                    (new VwMonitorSystemRow(sysTypeCd: "SYSTEM3", fuelCd: "FUEL1", beginDatehour: DateTime.Today.AddDays(-10), beginDate: DateTime.Today.AddDays(-10), beginHour: 0, endDatehour: DateTime.Today.AddDays(-8), endDate: DateTime.Today.AddDays(-8), endHour: 0),
                    new VwMonitorSystemRow(sysTypeCd: "SYSTEM3", fuelCd: "FUEL2", beginDatehour: DateTime.Today.AddDays(-10), beginDate: DateTime.Today.AddDays(-10), beginHour: 0, endDatehour: DateTime.Today.AddDays(-8), endDate: DateTime.Today.AddDays(-8), endHour: 0));
                MpParameters.FormulaRecords = new CheckDataView<VwMonitorFormulaRow>(); //null
                missingFormulaForMethodTest = MpParameters.CurrentMethod.ParameterCd + " FORM1,FORM2";

                // Init Output
                category.CheckCatalogResult = null;
                MpParameters.MissingFormulaForMethod = null;

                // Run Checks
                actual = target.METHOD27(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual(null, MpParameters.MissingFormulaForMethod, "MissingFormulaForMethod");
            }

            //MonitorSystem Not Found: BeginDate
            {
                MpParameters.EcmpsMpBeginDate = null;
                MpParameters.ParameterAndMethodAndLocationToFormulaCrosscheck = new CheckDataView<ParameterMethodToFormulaRow>
                    (new ParameterMethodToFormulaRow(parameterCd: "PARAMGOOD", methodCd: "METHODGOOD", locationTypeList: null,
                        systemTypeList: "SYSTEM1, SYSTEM2", ecmpsOnly: null, formulaList: "FORM1,FORM2", notFoundResult: "Z"));
                MpParameters.MonitorSystemRecords = new CheckDataView<VwMonitorSystemRow>
                    (new VwMonitorSystemRow(sysTypeCd: "SYSTEM1", fuelCd: "FUEL1", beginDatehour: DateTime.Today.AddDays(1), beginDate: DateTime.Today.AddDays(1), beginHour: 0, endDatehour: DateTime.Today.AddDays(-8), endDate: DateTime.Today.AddDays(-8), endHour: 0),
                    new VwMonitorSystemRow(sysTypeCd: "SYSTEM1", fuelCd: "FUEL2", beginDatehour: DateTime.Today.AddDays(1), beginDate: DateTime.Today.AddDays(1), beginHour: 0, endDatehour: DateTime.Today.AddDays(-8), endDate: DateTime.Today.AddDays(-8), endHour: 0));
                MpParameters.FormulaRecords = new CheckDataView<VwMonitorFormulaRow>(); //null
                missingFormulaForMethodTest = MpParameters.CurrentMethod.ParameterCd + " FORM1,FORM2";

                // Init Output
                category.CheckCatalogResult = null;
                MpParameters.MissingFormulaForMethod = null;

                // Run Checks
                actual = target.METHOD27(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual(null, MpParameters.MissingFormulaForMethod, "MissingFormulaForMethod");
            }

            //MonitorSystem Not Found: EndDate
            {
                MpParameters.EcmpsMpBeginDate = null;
                MpParameters.ParameterAndMethodAndLocationToFormulaCrosscheck = new CheckDataView<ParameterMethodToFormulaRow>
                    (new ParameterMethodToFormulaRow(parameterCd: "PARAMGOOD", methodCd: "METHODGOOD", locationTypeList: null,
                        systemTypeList: "SYSTEM1, SYSTEM2", ecmpsOnly: null, formulaList: "FORM1,FORM2", notFoundResult: "Z"));
                MpParameters.MonitorSystemRecords = new CheckDataView<VwMonitorSystemRow>
                    (new VwMonitorSystemRow(sysTypeCd: "SYSTEM1", fuelCd: "FUEL1", beginDatehour: DateTime.Today.AddDays(-10), beginDate: DateTime.Today.AddDays(-10), beginHour: 0, endDatehour: DateTime.Today.AddDays(-11), endDate: DateTime.Today.AddDays(-11), endHour: 0),
                    new VwMonitorSystemRow(sysTypeCd: "SYSTEM1", fuelCd: "FUEL2", beginDatehour: DateTime.Today.AddDays(-10), beginDate: DateTime.Today.AddDays(-10), beginHour: 0, endDatehour: DateTime.Today.AddDays(-11), endDate: DateTime.Today.AddDays(-11), endHour: 0));
                MpParameters.FormulaRecords = new CheckDataView<VwMonitorFormulaRow>(); //null
                missingFormulaForMethodTest = MpParameters.CurrentMethod.ParameterCd + " FORM1,FORM2";

                // Init Output
                category.CheckCatalogResult = null;
                MpParameters.MissingFormulaForMethod = null;

                // Run Checks
                actual = target.METHOD27(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual(null, MpParameters.MissingFormulaForMethod, "MissingFormulaForMethod");
            }

            //MonitorSystem found: 1 FuelCode
            {
                MpParameters.EcmpsMpBeginDate = DateTime.Today.AddYears(-5);
                MpParameters.ParameterAndMethodAndLocationToFormulaCrosscheck = new CheckDataView<ParameterMethodToFormulaRow>
                    (new ParameterMethodToFormulaRow(parameterCd: "PARAMGOOD", methodCd: "METHODGOOD", locationTypeList: null,
                        systemTypeList: "SYSTEM1, SYSTEM2", ecmpsOnly: "Yes", formulaList: "FORM1,FORM2", notFoundResult: "Z"));
                MpParameters.MonitorSystemRecords = new CheckDataView<VwMonitorSystemRow>
                    (new VwMonitorSystemRow(sysTypeCd: "SYSTEM1", fuelCd: "FUEL1", beginDatehour: DateTime.Today.AddDays(-10), beginDate: DateTime.Today.AddDays(-10), beginHour: 0, endDatehour: DateTime.Today, endDate: DateTime.Today, endHour: 0),
                    new VwMonitorSystemRow(sysTypeCd: "SYSTEM1", fuelCd: "FUEL1", beginDatehour: DateTime.Today.AddDays(-10), beginDate: DateTime.Today.AddDays(-10), beginHour: 0, endDatehour: DateTime.Today, endDate: DateTime.Today, endHour: 0));
                MpParameters.FormulaRecords = new CheckDataView<VwMonitorFormulaRow>(); //null
                missingFormulaForMethodTest = MpParameters.CurrentMethod.ParameterCd + " FORM1,FORM2";

                // Init Output
                category.CheckCatalogResult = null;
                MpParameters.MissingFormulaForMethod = null;

                // Run Checks
                actual = target.METHOD27(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual(null, MpParameters.MissingFormulaForMethod, "MissingFormulaForMethod");
            }

            //MonitorSystem found: only 1 record overlaps
            {
                MpParameters.EcmpsMpBeginDate = DateTime.Today.AddYears(-5);
                MpParameters.ParameterAndMethodAndLocationToFormulaCrosscheck = new CheckDataView<ParameterMethodToFormulaRow>
                    (new ParameterMethodToFormulaRow(parameterCd: "PARAMGOOD", methodCd: "METHODGOOD", locationTypeList: null,
                        systemTypeList: "SYSTEM1, SYSTEM2", ecmpsOnly: "Yes", formulaList: "FORM1,FORM2", notFoundResult: "Z"));
                MpParameters.MonitorSystemRecords = new CheckDataView<VwMonitorSystemRow>
                    (new VwMonitorSystemRow(sysTypeCd: "SYSTEM1", fuelCd: "FUEL1", beginDatehour: DateTime.Today.AddDays(-20), beginDate: DateTime.Today.AddDays(-20), beginHour: 0, endDatehour: DateTime.Today.AddDays(-11), endDate: DateTime.Today.AddDays(-11), endHour: 0),
                    new VwMonitorSystemRow(sysTypeCd: "SYSTEM1", fuelCd: "FUEL2", beginDatehour: DateTime.Today.AddDays(-10), beginDate: DateTime.Today.AddDays(-10), beginHour: 0, endDatehour: DateTime.Today, endDate: DateTime.Today, endHour: 0));
                MpParameters.FormulaRecords = new CheckDataView<VwMonitorFormulaRow>(); //null
                missingFormulaForMethodTest = MpParameters.CurrentMethod.ParameterCd + " FORM1,FORM2";

                // Init Output
                category.CheckCatalogResult = null;
                MpParameters.MissingFormulaForMethod = null;

                // Run Checks
                actual = target.METHOD27(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual(null, MpParameters.MissingFormulaForMethod, "MissingFormulaForMethod");
            }

            //MonitorSystem found, MonitorFormula Found, null dates
            {
                MpParameters.EcmpsMpBeginDate = null;
                MpParameters.ParameterAndMethodAndLocationToFormulaCrosscheck = new CheckDataView<ParameterMethodToFormulaRow>
                    (new ParameterMethodToFormulaRow(parameterCd: "PARAMGOOD", methodCd: "METHODGOOD", locationTypeList: null,
                        systemTypeList: "SYSTEM1, SYSTEM2", ecmpsOnly: "Yes", formulaList: "FORM1,FORM2", notFoundResult: "Z"));
                MpParameters.MonitorSystemRecords = new CheckDataView<VwMonitorSystemRow>
                    (new VwMonitorSystemRow(sysTypeCd: "SYSTEM1", fuelCd: "FUEL1", beginDatehour: DateTime.Today.AddDays(-10), beginDate: DateTime.Today.AddDays(-10), beginHour: 0, endDatehour: DateTime.Today, endDate: DateTime.Today, endHour: 0),
                    new VwMonitorSystemRow(sysTypeCd: "SYSTEM1", fuelCd: "FUEL2", beginDatehour: DateTime.Today.AddDays(-10), beginDate: DateTime.Today.AddDays(-10), beginHour: 0, endDatehour: DateTime.Today, endDate: DateTime.Today, endHour: 0));
                MpParameters.FormulaRecords = new CheckDataView<VwMonitorFormulaRow>
                    (new VwMonitorFormulaRow(parameterCd: "PARAMGOOD", equationCd: "FORM1", beginDatehour: null, endDatehour: null)); //null
                missingFormulaForMethodTest = MpParameters.CurrentMethod.ParameterCd + " FORM1,FORM2";

                // Init Output
                category.CheckCatalogResult = null;
                MpParameters.MissingFormulaForMethod = null;

                // Run Checks
                actual = target.METHOD27(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual(null, MpParameters.MissingFormulaForMethod, "MissingFormulaForMethod");
            }

            //MonitorSystem found, MonitorFormula Found, equal dates
            {
                MpParameters.EcmpsMpBeginDate = null;
                MpParameters.ParameterAndMethodAndLocationToFormulaCrosscheck = new CheckDataView<ParameterMethodToFormulaRow>
                    (new ParameterMethodToFormulaRow(parameterCd: "PARAMGOOD", methodCd: "METHODGOOD", locationTypeList: null,
                        systemTypeList: "SYSTEM1, SYSTEM2", ecmpsOnly: "Yes", formulaList: "FORM1,FORM2", notFoundResult: "Z"));
                MpParameters.MonitorSystemRecords = new CheckDataView<VwMonitorSystemRow>
                    (new VwMonitorSystemRow(sysTypeCd: "SYSTEM1", fuelCd: "FUEL1", beginDatehour: DateTime.Today.AddDays(-10), beginDate: DateTime.Today.AddDays(-10), beginHour: 0, endDatehour: DateTime.Today, endDate: DateTime.Today, endHour: 0),
                    new VwMonitorSystemRow(sysTypeCd: "SYSTEM1", fuelCd: "FUEL2", beginDatehour: DateTime.Today.AddDays(-10), beginDate: DateTime.Today.AddDays(-10), beginHour: 0, endDatehour: DateTime.Today, endDate: DateTime.Today, endHour: 0));
                MpParameters.FormulaRecords = new CheckDataView<VwMonitorFormulaRow>
                    (new VwMonitorFormulaRow(parameterCd: "PARAMGOOD", equationCd: "FORM1", beginDatehour: DateTime.Today.AddDays(-10), beginDate: DateTime.Today.AddDays(-10), beginHour: 0, endDatehour: DateTime.Today, endDate: DateTime.Today, endHour: 0)); //null
                missingFormulaForMethodTest = MpParameters.CurrentMethod.ParameterCd + " FORM1,FORM2";

                // Init Output
                category.CheckCatalogResult = null;
                MpParameters.MissingFormulaForMethod = null;

                // Run Checks
                actual = target.METHOD27(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual(null, MpParameters.MissingFormulaForMethod, "MissingFormulaForMethod");
            }

            //MonitorSystem found, MonitorFormula Not Found: ParameterCode
            {
                MpParameters.EcmpsMpBeginDate = null;
                MpParameters.ParameterAndMethodAndLocationToFormulaCrosscheck = new CheckDataView<ParameterMethodToFormulaRow>
                    (new ParameterMethodToFormulaRow(parameterCd: "PARAMGOOD", methodCd: "METHODGOOD", locationTypeList: null,
                        systemTypeList: "SYSTEM1, SYSTEM2", ecmpsOnly: "Yes", formulaList: "FORM1,FORM2", notFoundResult: "Z"));
                MpParameters.MonitorSystemRecords = new CheckDataView<VwMonitorSystemRow>
                    (new VwMonitorSystemRow(sysTypeCd: "SYSTEM1", fuelCd: "FUEL1", beginDatehour: DateTime.Today.AddDays(-10), beginDate: DateTime.Today.AddDays(-10), beginHour: 0, endDatehour: DateTime.Today, endDate: DateTime.Today, endHour: 0),
                    new VwMonitorSystemRow(sysTypeCd: "SYSTEM1", fuelCd: "FUEL2", beginDatehour: DateTime.Today.AddDays(-10), beginDate: DateTime.Today.AddDays(-10), beginHour: 0, endDatehour: DateTime.Today, endDate: DateTime.Today, endHour: 0));
                MpParameters.FormulaRecords = new CheckDataView<VwMonitorFormulaRow>
                    (new VwMonitorFormulaRow(parameterCd: "PARAMBAD", equationCd: "FORM1", beginDatehour: DateTime.Today.AddDays(-10), beginDate: DateTime.Today.AddDays(-10), beginHour: 0, endDatehour: DateTime.Today, endDate: DateTime.Today, endHour: 0)); //null
                missingFormulaForMethodTest = MpParameters.CurrentMethod.ParameterCd + " FORM1,FORM2";

                // Init Output
                category.CheckCatalogResult = null;
                MpParameters.MissingFormulaForMethod = null;

                // Run Checks
                actual = target.METHOD27(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual("Z", category.CheckCatalogResult, "Result");
                Assert.AreEqual(missingFormulaForMethodTest, MpParameters.MissingFormulaForMethod, "MissingFormulaForMethod");
            }

            //MonitorSystem found, MonitorFormula Not Found: FormulaCode (equationCd)
            {
                MpParameters.EcmpsMpBeginDate = null;
                MpParameters.ParameterAndMethodAndLocationToFormulaCrosscheck = new CheckDataView<ParameterMethodToFormulaRow>
                    (new ParameterMethodToFormulaRow(parameterCd: "PARAMGOOD", methodCd: "METHODGOOD", locationTypeList: null,
                        systemTypeList: "SYSTEM1, SYSTEM2", ecmpsOnly: "Yes", formulaList: "FORM1,FORM2", notFoundResult: "Z"));
                MpParameters.MonitorSystemRecords = new CheckDataView<VwMonitorSystemRow>
                    (new VwMonitorSystemRow(sysTypeCd: "SYSTEM1", fuelCd: "FUEL1", beginDatehour: DateTime.Today.AddDays(-10), beginDate: DateTime.Today.AddDays(-10), beginHour: 0, endDatehour: DateTime.Today, endDate: DateTime.Today, endHour: 0),
                    new VwMonitorSystemRow(sysTypeCd: "SYSTEM1", fuelCd: "FUEL2", beginDatehour: DateTime.Today.AddDays(-10), beginDate: DateTime.Today.AddDays(-10), beginHour: 0, endDatehour: DateTime.Today, endDate: DateTime.Today, endHour: 0));
                MpParameters.FormulaRecords = new CheckDataView<VwMonitorFormulaRow>
                    (new VwMonitorFormulaRow(parameterCd: "PARAMGOOD", equationCd: "FORM3", beginDatehour: DateTime.Today.AddDays(-10), beginDate: DateTime.Today.AddDays(-10), beginHour: 0, endDatehour: DateTime.Today, endDate: DateTime.Today, endHour: 0)); //null
                missingFormulaForMethodTest = MpParameters.CurrentMethod.ParameterCd + " FORM1,FORM2";

                // Init Output
                category.CheckCatalogResult = null;
                MpParameters.MissingFormulaForMethod = null;

                // Run Checks
                actual = target.METHOD27(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual("Z", category.CheckCatalogResult, "Result");
                Assert.AreEqual(missingFormulaForMethodTest, MpParameters.MissingFormulaForMethod, "MissingFormulaForMethod");
            }

            //MonitorSystem found, MonitorFormula Not Found: BeginDate
            {
                MpParameters.EcmpsMpBeginDate = null;
                MpParameters.ParameterAndMethodAndLocationToFormulaCrosscheck = new CheckDataView<ParameterMethodToFormulaRow>
                    (new ParameterMethodToFormulaRow(parameterCd: "PARAMGOOD", methodCd: "METHODGOOD", locationTypeList: null,
                        systemTypeList: "SYSTEM1, SYSTEM2", ecmpsOnly: "Yes", formulaList: "FORM1,FORM2", notFoundResult: "Z"));
                MpParameters.MonitorSystemRecords = new CheckDataView<VwMonitorSystemRow>
                    (new VwMonitorSystemRow(sysTypeCd: "SYSTEM1", fuelCd: "FUEL1", beginDatehour: DateTime.Today.AddDays(-10), beginDate: DateTime.Today.AddDays(-10), beginHour: 0, endDatehour: DateTime.Today, endDate: DateTime.Today, endHour: 0),
                    new VwMonitorSystemRow(sysTypeCd: "SYSTEM1", fuelCd: "FUEL2", beginDatehour: DateTime.Today.AddDays(-10), beginDate: DateTime.Today.AddDays(-10), beginHour: 0, endDatehour: DateTime.Today, endDate: DateTime.Today, endHour: 0));
                MpParameters.FormulaRecords = new CheckDataView<VwMonitorFormulaRow>
                    (new VwMonitorFormulaRow(parameterCd: "PARAMGOOD", equationCd: "FORM1", beginDatehour: DateTime.Today.AddDays(1), beginDate: DateTime.Today.AddDays(1), beginHour: 0, endDatehour: DateTime.Today, endDate: DateTime.Today, endHour: 0)); //null
                missingFormulaForMethodTest = MpParameters.CurrentMethod.ParameterCd + " FORM1,FORM2";

                // Init Output
                category.CheckCatalogResult = null;
                MpParameters.MissingFormulaForMethod = null;

                // Run Checks
                actual = target.METHOD27(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual("Z", category.CheckCatalogResult, "Result");
                Assert.AreEqual(missingFormulaForMethodTest, MpParameters.MissingFormulaForMethod, "MissingFormulaForMethod");
            }

            //MonitorSystem found, MonitorFormula Not Found: EndDate
            {
                MpParameters.EcmpsMpBeginDate = null;
                MpParameters.ParameterAndMethodAndLocationToFormulaCrosscheck = new CheckDataView<ParameterMethodToFormulaRow>
                    (new ParameterMethodToFormulaRow(parameterCd: "PARAMGOOD", methodCd: "METHODGOOD", locationTypeList: null,
                        systemTypeList: "SYSTEM1, SYSTEM2", ecmpsOnly: "Yes", formulaList: "FORM1,FORM2", notFoundResult: "Z"));
                MpParameters.MonitorSystemRecords = new CheckDataView<VwMonitorSystemRow>
                    (new VwMonitorSystemRow(sysTypeCd: "SYSTEM1", fuelCd: "FUEL1", beginDatehour: DateTime.Today.AddDays(-10), beginDate: DateTime.Today.AddDays(-10), beginHour: 0, endDatehour: DateTime.Today, endDate: DateTime.Today, endHour: 0),
                    new VwMonitorSystemRow(sysTypeCd: "SYSTEM1", fuelCd: "FUEL2", beginDatehour: DateTime.Today.AddDays(-10), beginDate: DateTime.Today.AddDays(-10), beginHour: 0, endDatehour: DateTime.Today, endDate: DateTime.Today, endHour: 0));
                MpParameters.FormulaRecords = new CheckDataView<VwMonitorFormulaRow>
                    (new VwMonitorFormulaRow(parameterCd: "PARAMGOOD", equationCd: "FORM1", beginDatehour: DateTime.Today.AddDays(-10), beginDate: DateTime.Today.AddDays(-10), beginHour: 0, endDatehour: DateTime.Today.AddDays(-11), endDate: DateTime.Today.AddDays(-11), endHour: 0)); //null
                missingFormulaForMethodTest = MpParameters.CurrentMethod.ParameterCd + " FORM1,FORM2";

                // Init Output
                category.CheckCatalogResult = null;
                MpParameters.MissingFormulaForMethod = null;

                // Run Checks
                actual = target.METHOD27(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual("Z", category.CheckCatalogResult, "Result");
                Assert.AreEqual(missingFormulaForMethodTest, MpParameters.MissingFormulaForMethod, "MissingFormulaForMethod");
            }

            //MonitorSystem found, MonitorFormula Found, dates </>, passes span/overlap
            {
                MpParameters.EcmpsMpBeginDate = null;
                MpParameters.ParameterAndMethodAndLocationToFormulaCrosscheck = new CheckDataView<ParameterMethodToFormulaRow>
                    (new ParameterMethodToFormulaRow(parameterCd: "PARAMGOOD", methodCd: "METHODGOOD", locationTypeList: null,
                        systemTypeList: "SYSTEM1, SYSTEM2", ecmpsOnly: "Yes", formulaList: "FORM1,FORM2", notFoundResult: "Z"));
                MpParameters.MonitorSystemRecords = new CheckDataView<VwMonitorSystemRow>
                    (new VwMonitorSystemRow(sysTypeCd: "SYSTEM1", fuelCd: "FUEL1", beginDatehour: DateTime.Today.AddDays(-10), beginDate: DateTime.Today.AddDays(-11), beginHour: 0, endDatehour: DateTime.Today.AddDays(-5), endDate: DateTime.Today.AddDays(-5), endHour: 0),
                    new VwMonitorSystemRow(sysTypeCd: "SYSTEM1", fuelCd: "FUEL2", beginDatehour: DateTime.Today.AddDays(-10), beginDate: DateTime.Today.AddDays(-11), beginHour: 0, endDatehour: DateTime.Today.AddDays(-5), endDate: DateTime.Today.AddDays(-5), endHour: 0));
                MpParameters.FormulaRecords = new CheckDataView<VwMonitorFormulaRow>
                    (new VwMonitorFormulaRow(parameterCd: "PARAMGOOD", equationCd: "FORM1", beginDatehour: DateTime.Today.AddDays(-11), beginDate: DateTime.Today.AddDays(-11), beginHour: 0, endDatehour: DateTime.Today.AddDays(-5), endDate: DateTime.Today.AddDays(-5), endHour: 0)); //null
                missingFormulaForMethodTest = MpParameters.CurrentMethod.ParameterCd + " FORM1,FORM2";

                // Init Output
                category.CheckCatalogResult = null;
                MpParameters.MissingFormulaForMethod = null;

                // Run Checks
                actual = target.METHOD27(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual(null, MpParameters.MissingFormulaForMethod, "MissingFormulaForMethod");
            }

            //MonitorSystem found, MonitorFormula Found, dates </>, fails span/overlap
            {
                MpParameters.EcmpsMpBeginDate = null;
                MpParameters.ParameterAndMethodAndLocationToFormulaCrosscheck = new CheckDataView<ParameterMethodToFormulaRow>
                    (new ParameterMethodToFormulaRow(parameterCd: "PARAMGOOD", methodCd: "METHODGOOD", locationTypeList: null,
                        systemTypeList: "SYSTEM1, SYSTEM2", ecmpsOnly: "Yes", formulaList: "FORM1,FORM2", notFoundResult: "Z"));
                MpParameters.MonitorSystemRecords = new CheckDataView<VwMonitorSystemRow>
                    (new VwMonitorSystemRow(sysTypeCd: "SYSTEM1", fuelCd: "FUEL1", beginDatehour: DateTime.Today.AddDays(-10), beginDate: DateTime.Today.AddDays(-11), beginHour: 0, endDatehour: DateTime.Today.AddDays(-5), endDate: DateTime.Today.AddDays(-5), endHour: 0),
                    new VwMonitorSystemRow(sysTypeCd: "SYSTEM1", fuelCd: "FUEL2", beginDatehour: DateTime.Today.AddDays(-10), beginDate: DateTime.Today.AddDays(-11), beginHour: 0, endDatehour: DateTime.Today.AddDays(-5), endDate: DateTime.Today.AddDays(-5), endHour: 0));
                MpParameters.FormulaRecords = new CheckDataView<VwMonitorFormulaRow>
                    (new VwMonitorFormulaRow(parameterCd: "PARAMGOOD", equationCd: "FORM1", beginDatehour: DateTime.Today.AddDays(-9), beginDate: DateTime.Today.AddDays(-9), beginHour: 0, endDatehour: DateTime.Today.AddDays(-6), endDate: DateTime.Today.AddDays(-6), endHour: 0)); //null
                missingFormulaForMethodTest = MpParameters.CurrentMethod.ParameterCd + " FORM1,FORM2";

                // Init Output
                category.CheckCatalogResult = null;
                MpParameters.MissingFormulaForMethod = null;

                // Run Checks
                actual = target.METHOD27(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual("B", category.CheckCatalogResult, "Result");
                Assert.AreEqual(missingFormulaForMethodTest, MpParameters.MissingFormulaForMethod, "MissingFormulaForMethod");
            }
        }

        #endregion

        #region Method-28

        /// <summary>
        ///A test for METHOD28_RemoveHGM
        ///</summary>()
        [TestMethod()]
        public void METHOD28_RemoveHGM()
        {
            cMethodChecks target = new cMethodChecks(new cMpCheckParameters());
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            MpParameters.Init(category.Process);
            MpParameters.Category = category;

            // Variables
            bool log = false;
            string actual;

            // Init Input
            MpParameters.DefaultRecords = new CheckDataView<VwMonitorDefaultRow>();
            MpParameters.LocationFuelRecords = new CheckDataView<VwLocationFuelRow>();
            MpParameters.LocationType = "U";
            MpParameters.MethodEvaluationBeginDate = DateTime.Today.AddDays(-1);
            MpParameters.MethodEvaluationBeginHour = 0;
            MpParameters.MethodEvaluationEndDate = DateTime.Today;
            MpParameters.MethodEvaluationEndHour = 0;
            MpParameters.MethodMethodCodeValid = true;
            MpParameters.MethodParameterValid = true;
            MpParameters.MethodDatesAndHoursConsistent = true;

            //HGM
            {
                //Init Input
                MpParameters.CurrentMethod = new VwMonitorMethodRow(methodCd: null, parameterCd: "HGM");

                // Init Output
                category.CheckCatalogResult = null;

                // Run Checks
                actual = target.METHOD28(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
            }
        }
        #endregion

        #region Method-33

        /// <summary>
        ///A test for METHOD33_RemoveHGM
        ///</summary>()
        [TestMethod()]
        public void METHOD33_RemoveHGM()
        {
          /*
           * When checks use the old instantiated parameters, use the same Check Parameters object 
           * to create the Unit Test Category and the Checks instance.
           */
			cMpCheckParameters mpCheckParameters = UnitTestCheckParameters.InstantiateMpParameters();
			cMethodChecks target = new cMethodChecks(mpCheckParameters);

            // Variables
            bool log = false;
            string actual;

            // Init Input
            MpParameters.FacilityQualificationRecords = new CheckDataView<MonitorQualificationRow>
                (new MonitorQualificationRow(monLocId:"LOC1", beginDate: DateTime.Today.AddDays(-1), endDate: null, qualTypeCd:"NOTLMEA"));
            MpParameters.LocationProgramParameterRecords = new CheckDataView<ECMPS.Checks.Data.Ecmps.CheckMp.Function.LocationProgramParameter>
                (new ECMPS.Checks.Data.Ecmps.CheckMp.Function.LocationProgramParameter(monLocId: "LOC1", parameterCd: "NOX", osInd: 1, classCd: "A", umcbDate: DateTime.Today.AddDays(-1), prgEndDate: DateTime.Today));
            MpParameters.LocationReportingFrequencyRecords = new CheckDataView<VwLocationReportingFrequencyRow>
                (new VwLocationReportingFrequencyRow(monLocId: "LOC1", reportFreqCd: "Q", beginQuarter: "2014 Q1", endQuarter: null, beginRptPeriodId: 85, endRptPeriodId: 85));
            MpParameters.MethodEvaluationBeginDate = DateTime.Today.AddDays(-1);
            MpParameters.MethodEvaluationBeginHour = 0;
            MpParameters.MethodEvaluationEndDate = DateTime.Today;
            MpParameters.MethodEvaluationEndHour = 0;
            MpParameters.MethodMethodCodeValid = true;
            MpParameters.MethodDatesAndHoursConsistent = true;

            //HGM
            {
                //Init Input
                MpParameters.CurrentMethod = new VwMonitorMethodRow(methodCd: "LME", parameterCd: "HGM", monLocId: "LOC1");

                // Init Output
				MpParameters.Category.CheckCatalogResult = null;
				MpParameters.MissingQualificationForMethod = null;
				MpParameters.IncompleteQualificationForMethod = null;

                // Run Checks
				actual = target.METHOD33(MpParameters.Category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
				Assert.AreEqual("A", MpParameters.Category.CheckCatalogResult, "Result");
            }
        }

        #endregion

        /// <summary>
        /// Method35
        /// 
        /// MEBD - Method Evaluation Begin Date-Hour : 2016-06-02
        /// MEED - Method Evaluation End Date-Hour   : 2016-06-29
        /// 
        /// PRB1/PRE1 - Period 1 Begin / End Dates   : 2016-06-02 / 2016-06-11
        /// PRB2/PRE2 - Period 2 Begin / End Dates   : 2016-06-12 / 2016-06-20
        /// PRB3/PRE3 - Period 3 Begin / End Dates   : 2016-06-21 / 2016-06-29
        /// 
        /// | Case | Method | MethodLoc | Valid | Consistent | Control | InstallDt | RetiredDt | OriginalCd | ControlLoc || Result || Note 
        /// |    0 | BAD    | 'Good'    | true  | true       | null    | null      | null      | null       | 'Good'     || null   || Non moisture table method, no control
        /// |    1 | MTB    | 'Good'    | false | true       | null    | null      | null      | null       | 'Good'     || null   || Moisture table method with Method Valid set to false, no control
        /// |    2 | MTB    | 'Good'    | true  | false      | null    | null      | null      | null       | 'Good'     || null   || Inconsistent dates (MethodDatesAndHoursInconsistent), no control
        /// |    3 | MTB    | 'Good'    | true  | true       | WL      | null      | null      | 1          | 'Good'     || null   || Original control without retirement date.
        /// |    4 | MTB    | 'Good'    | true  | true       | WLS     | null      | null      | 1          | 'Good'     || null   || Original control without retirement date.
        /// |    5 | MTB    | 'Good'    | true  | true       | WS      | null      | null      | 1          | 'Good'     || null   || Original control without retirement date.
        /// |    6 | MTB    | 'Good'    | true  | true       | WL      | null      | MEED      | 1          | 'Good'     || null   || Original control with retirement date on or after Method Evaluation End Date.
        /// |    7 | MTB    | 'Good'    | true  | true       | WLS     | null      | MEED      | 1          | 'Good'     || null   || Original control with retirement date on or after Method Evaluation End Date.
        /// |    8 | MTB    | 'Good'    | true  | true       | WS      | null      | MEED      | 1          | 'Good'     || null   || Original control with retirement date on or after Method Evaluation End Date.
        /// |    9 | MTB    | 'Good'    | true  | true       | WL      | MEBD      | null      | 1          | 'Good'     || null   || Active control begins on or before Method Evaluation Begin Date, install date invalidates original code.
        /// |   10 | MTB    | 'Good'    | true  | true       | WLS     | MEBD      | null      | 1          | 'Good'     || null   || Active control begins on or before Method Evaluation Begin Date, install date invalidates original code.
        /// |   11 | MTB    | 'Good'    | true  | true       | WS      | MEBD      | null      | 1          | 'Good'     || null   || Active control begins on or before Method Evaluation Begin Date, install date invalidates original code.
        /// |   12 | MTB    | 'Good'    | true  | true       | WL      | MEBD      | MEED      | 1          | 'Good'     || null   || Control spans evaluation period, install date invalidates original code.
        /// |   13 | MTB    | 'Good'    | true  | true       | WLS     | MEBD      | MEED      | 1          | 'Good'     || null   || Control spans evaluation period, install date invalidates original code.
        /// |   14 | MTB    | 'Good'    | true  | true       | WS      | MEBD      | MEED      | 1          | 'Good'     || null   || Control spans evaluation period, install date invalidates original code.
        /// |   15 | MTB    | 'Good'    | true  | true       | WL      | MEBD      | MEED      | 0          | 'Good'     || null   || Control spans evaluation period.
        /// |   16 | MTB    | 'Good'    | true  | true       | WLS     | MEBD      | MEED      | 0          | 'Good'     || null   || Control spans evaluation period.
        /// |   17 | MTB    | 'Good'    | true  | true       | WS      | MEBD      | MEED      | 0          | 'Good'     || null   || Control spans evaluation period.
        /// |   18 | MTB    | 'Good'    | true  | true       | OTHER   | null      | null      | 1          | 'Good'     || A      || Original control without retirement date, but bad control code.
        /// |   19 | MTB    | 'Good'    | true  | true       | OTHER   | MEBD      | null      | 1          | 'Good'     || A      || Active control without retirement date, but bad control code.
        /// |   20 | MTB    | 'Good'    | true  | true       | WL      | null      | null      | 0          | 'Good'     || A      || Non-original control without install date.
        /// |   21 | MTB    | 'Good'    | true  | true       | WLS     | null      | null      | 0          | 'Good'     || A      || Non-original control without install date.
        /// |   22 | MTB    | 'Good'    | true  | true       | WS      | null      | null      | 0          | 'Good'     || A      || Non-original control without install date.
        /// |   23 | MTB    | 'Good'    | true  | true       | WL      | MEED + 1  | null      | 0          | 'Good'     || A      || Non-original control that begins after Method Evaluation End Date.
        /// |   24 | MTB    | 'Good'    | true  | true       | WLS     | MEED + 1  | null      | 0          | 'Good'     || A      || Non-original control that begins after Method Evaluation End Date.
        /// |   25 | MTB    | 'Good'    | true  | true       | WS      | MEED + 1  | null      | 0          | 'Good'     || A      || Non-original control that begins after Method Evaluation End Date.
        /// |   26 | MTB    | 'Good'    | true  | true       | WL      | MEBD - 2  | MEBD - 1  | 0          | 'Good'     || A      || Non-original control that ends before Method Evaluation Begin Date.
        /// |   27 | MTB    | 'Good'    | true  | true       | WLS     | MEBD - 2  | MEBD - 1  | 0          | 'Good'     || A      || Non-original control that ends before Method Evaluation Begin Date.
        /// |   28 | MTB    | 'Good'    | true  | true       | WS      | MEBD - 2  | MEBD - 1  | 0          | 'Good'     || A      || Non-original control that ends before Method Evaluation Begin Date.
        /// |   29 | MTB    | 'Good'    | true  | true       | WL      | PRB1      | PRE1      | 0          | 'Good'     || null   || Multiple controls spanning Method Evaluation Period.
        /// |      |        |           |       |            | WLS     | PRB2      | PRE2      | 1          | 'Good'     ||        || 
        /// |      |        |           |       |            | WS      | PRB3      | PRE3      | 0          | 'Good'     ||        || 
        /// |   30 | MTB    | 'Good'    | true  | true       | WL      | PRB1 + 1  | PRE1      | 0          | 'Good'     || B      || Multiple controls spanning Method Evaluation Period, but missing first day.
        /// |      |        |           |       |            | WLS     | PRB2      | PRE2      | 1          | 'Good'     ||        || 
        /// |      |        |           |       |            | WS      | PRB3      | PRE3      | 0          | 'Good'     ||        || 
        /// |   31 | MTB    | 'Good'    | true  | true       | WL      | PRB1      | PRE1      | 0          | 'Good'     || B      || Multiple controls spanning Method Evaluation Period, but missing last day.
        /// |      |        |           |       |            | WLS     | PRB2      | PRE2      | 1          | 'Good'     ||        || 
        /// |      |        |           |       |            | WS      | PRB3      | PRE3 - 1  | 0          | 'Good'     ||        || 
        /// |   32 | MTB    | 'Good'    | true  | true       | WL      | PRB1      | PRE1      | 0          | 'Good'     || B      || Multiple controls spanning Method Evaluation Period, but missing middle day.
        /// |      |        |           |       |            | WLS     | PRB2 + 1  | PRE2      | 1          | 'Good'     ||        || 
        /// |      |        |           |       |            | WS      | PRB3      | PRE3      | 0          | 'Good'     ||        || 
        /// |   33 | MTB    | 'Good'    | true  | true       | WL      | null      | null      | 1          | 'Other'    || A      || Original control without retirement date and mismatch location key.
        /// |   34 | MTB    | 'Good'    | true  | true       | WLS     | null      | null      | 1          | 'Other'    || A      || Original control without retirement date and mismatch location key.
        /// |   35 | MTB    | 'Good'    | true  | true       | WS      | null      | null      | 1          | 'Other'    || A      || Original control without retirement date and mismatch location key.
        /// |   36 | MTB    | 'Good'    | true  | true       | WL      | MEBD      | MEED      | 0          | 'Other'    || A      || Control spans evaluation period, but has a mismatched location key.
        /// |   37 | MTB    | 'Good'    | true  | true       | WLS     | MEBD      | MEED      | 0          | 'Other'    || A      || Control spans evaluation period, but has a mismatched location key.
        /// |   38 | MTB    | 'Good'    | true  | true       | WS      | MEBD      | MEED      | 0          | 'Other'    || A      || Control spans evaluation period, but has a mismatched location key.
        /// </summary>
        [TestMethod()]
        public void Method35()
        {
            /* Initialize objects generally needed for testing checks. */
            cMethodChecks target = new cMethodChecks(new cMpCheckParameters());
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            MpParameters.Init(category.Process);
            MpParameters.Category = category;

            /* Initialize General Variables */
            DateTime methodEvaluationBeginDatehour = new DateTime(2016, 6, 2);
            DateTime methodEvaluationEndDatehour = new DateTime(2016, 6, 29);
            DateTime mebd = methodEvaluationBeginDatehour.Date;
            DateTime meed = methodEvaluationEndDatehour.Date;
            DateTime prb1 = new DateTime(2016, 6, 2);
            DateTime pre1 = new DateTime(2016, 6, 11);
            DateTime prb2 = new DateTime(2016, 6, 12);
            DateTime pre2 = new DateTime(2016, 6, 20);
            DateTime prb3 = new DateTime(2016, 6, 21);
            DateTime pre3 = new DateTime(2016, 6, 29);

            /* Initialize General Parameters */
            MpParameters.MethodEvaluationBeginDate = methodEvaluationBeginDatehour.Date;
            MpParameters.MethodEvaluationBeginHour = methodEvaluationBeginDatehour.Hour;
            MpParameters.MethodEvaluationEndDate = methodEvaluationEndDatehour.Date;
            MpParameters.MethodEvaluationEndHour = methodEvaluationEndDatehour.Hour;

            /* Case Count */
            int caseCount = 39;

            /* Input Parameter Values */
            string methodLocId = "Good";
            string[] methodList = { "BAD", "MTB", "MTB", "MTB", "MTB", "MTB", "MTB", "MTB", "MTB", "MTB", "MTB", "MTB", "MTB", "MTB", "MTB", "MTB", "MTB", "MTB", "MTB", "MTB", "MTB", "MTB", "MTB", "MTB", "MTB", "MTB", "MTB", "MTB", "MTB", "MTB", "MTB", "MTB", "MTB", "MTB", "MTB", "MTB", "MTB", "MTB", "MTB" };
            bool?[] validList = { true, false, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true };
            bool?[] consistentList = { true, true, false, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true };
            string[][] controlCdList = { new string[] { null }, new string[] { null }, new string[] { null }, new string[] { "WL" }, new string[] { "WLS" }, new string[] { "WS" }, new string[] { "WL" }, new string[] { "WLS" }, new string[] { "WS" }, new string[] { "WL" }, new string[] { "WLS" }, new string[] { "WS" }, new string[] { "WL" }, new string[] { "WLS" }, new string[] { "WS" }, new string[] { "WL" }, new string[] { "WLS" }, new string[] { "WS" }, new string[] { "OTHER" }, new string[] { "OTHER" }, new string[] { "WL" }, new string[] { "WLS" }, new string[] { "WS" }, new string[] { "WL" }, new string[] { "WLS" }, new string[] { "WS" }, new string[] { "WL" }, new string[] { "WLS" }, new string[] { "WS" }, new string[] { "WL", "WLS", "WS" }, new string[] { "WL", "WLS", "WS" }, new string[] { "WL", "WLS", "WS" }, new string[] { "WL", "WLS", "WS" }, new string[] { "WL" }, new string[] { "WLS" }, new string[] { "WS" }, new string[] { "WL" }, new string[] { "WLS" }, new string[] { "WS" } };
            DateTime?[][] installDtList = { new DateTime?[] { null }, new DateTime?[] { null }, new DateTime?[] { null }, new DateTime?[] { null }, new DateTime?[] { null }, new DateTime?[] { null }, new DateTime?[] { null }, new DateTime?[] { null }, new DateTime?[] { null }, new DateTime?[] { mebd }, new DateTime?[] { mebd }, new DateTime?[] { mebd }, new DateTime?[] { mebd }, new DateTime?[] { mebd }, new DateTime?[] { mebd }, new DateTime?[] { mebd }, new DateTime?[] { mebd }, new DateTime?[] { mebd }, new DateTime?[] { null }, new DateTime?[] { mebd }, new DateTime?[] { null }, new DateTime?[] { null }, new DateTime?[] { null }, new DateTime?[] { meed.AddDays(1) }, new DateTime?[] { meed.AddDays(1) }, new DateTime?[] { meed.AddDays(1) }, new DateTime?[] { mebd.AddDays(-2) }, new DateTime?[] { mebd.AddDays(-2) }, new DateTime?[] { mebd.AddDays(-2) }, new DateTime?[] { prb1, prb2, prb3 }, new DateTime?[] { prb1.AddDays(1), prb2, prb3 }, new DateTime?[] { prb1, prb2, prb3 }, new DateTime?[] { prb1, prb2.AddDays(1), prb3 }, new DateTime?[] { null }, new DateTime?[] { null }, new DateTime?[] { null }, new DateTime?[] { mebd }, new DateTime?[] { mebd }, new DateTime?[] { mebd } };
            DateTime?[][] retireDtList = { new DateTime?[] { null }, new DateTime?[] { null }, new DateTime?[] { null }, new DateTime?[] { null }, new DateTime?[] { null }, new DateTime?[] { null }, new DateTime?[] { meed }, new DateTime?[] { meed }, new DateTime?[] { meed }, new DateTime?[] { null }, new DateTime?[] { null }, new DateTime?[] { null }, new DateTime?[] { meed }, new DateTime?[] { meed }, new DateTime?[] { meed }, new DateTime?[] { meed }, new DateTime?[] { meed }, new DateTime?[] { meed }, new DateTime?[] { null }, new DateTime?[] { null }, new DateTime?[] { null }, new DateTime?[] { null }, new DateTime?[] { null }, new DateTime?[] { null }, new DateTime?[] { null }, new DateTime?[] { null }, new DateTime?[] { mebd.AddDays(-1) }, new DateTime?[] { mebd.AddDays(-1) }, new DateTime?[] { mebd.AddDays(-1) }, new DateTime?[] { pre1, pre2, pre3 }, new DateTime?[] { pre1, pre2, pre3 }, new DateTime?[] { pre1, pre2, pre3.AddDays(-1) }, new DateTime?[] { pre1, pre2, pre3 }, new DateTime?[] { null }, new DateTime?[] { null }, new DateTime?[] { null }, new DateTime?[] { meed }, new DateTime?[] { meed }, new DateTime?[] { meed } };
            int?[][] originalCdList = { new int?[] { null }, new int?[] { null }, new int?[] { null }, new int?[] { 1 }, new int?[] { 1 }, new int?[] { 1 }, new int?[] { 1 }, new int?[] { 1 }, new int?[] { 1 }, new int?[] { 1 }, new int?[] { 1 }, new int?[] { 1 }, new int?[] { 1 }, new int?[] { 1 }, new int?[] { 1 }, new int?[] { 0 }, new int?[] { 0 }, new int?[] { 0 }, new int?[] { 1 }, new int?[] { 1 }, new int?[] { 0 }, new int?[] { 0 }, new int?[] { 0 }, new int?[] { 0 }, new int?[] { 0 }, new int?[] { 0 }, new int?[] { 0 }, new int?[] { 0 }, new int?[] { 0 }, new int?[] { 0, 1, 0 }, new int?[] { 0, 1, 0 }, new int?[] { 0, 1, 0 }, new int?[] { 0, 1, 0 }, new int?[] { 1 }, new int?[] { 1 }, new int?[] { 1 }, new int?[] { 0 }, new int?[] { 0 }, new int?[] { 0 } };
            string[][] controlLocIdList = { new string[] { "Good" }, new string[] { "Good" }, new string[] { "Good" }, new string[] { "Good" }, new string[] { "Good" }, new string[] { "Good" }, new string[] { "Good" }, new string[] { "Good" }, new string[] { "Good" }, new string[] { "Good" }, new string[] { "Good" }, new string[] { "Good" }, new string[] { "Good" }, new string[] { "Good" }, new string[] { "Good" }, new string[] { "Good" }, new string[] { "Good" }, new string[] { "Good" }, new string[] { "Good" }, new string[] { "Good" }, new string[] { "Good" }, new string[] { "Good" }, new string[] { "Good" }, new string[] { "Good" }, new string[] { "Good" }, new string[] { "Good" }, new string[] { "Good" }, new string[] { "Good" }, new string[] { "Good" }, new string[] { "Good", "Good", "Good" }, new string[] { "Good", "Good", "Good" }, new string[] { "Good", "Good", "Good" }, new string[] { "Good", "Good", "Good" }, new string[] { "Other" }, new string[] { "Other" }, new string[] { "Other" }, new string[] { "Other" }, new string[] { "Other" }, new string[] { "Other" } };

            /* Expected Values */
            string[] expectedResultList = { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", null, "B", "B", "B", "A", "A", "A", "A", "A", "A" };

            for (int caseDex = 0; caseDex < caseCount; caseDex++)
            {
                /* Generate rows for Location Control Records */
                VwLocationControlRow[] locationControlRows;
                {
                    string[] controlCodes = controlCdList[caseDex];
                    DateTime?[] installDates = installDtList[caseDex];
                    DateTime?[] retireDates = retireDtList[caseDex];
                    int?[] originalCodes = originalCdList[caseDex];
                    string[] locationIds = controlLocIdList[caseDex];

                    locationControlRows = new VwLocationControlRow[controlCodes.Length];

                    for (int controlDex = 0; controlDex < locationControlRows.Length; controlDex++)
                        locationControlRows[controlDex] 
                            = new VwLocationControlRow
                              (
                                controlCd: controlCodes[controlDex], 
                                installDate: installDates[controlDex],
                                retireDate: retireDates[controlDex],
                                origInd: originalCodes[controlDex],
                                monLocId: locationIds[controlDex]
                              );
                }

                /* Initialize Input Parameters */
                MpParameters.CurrentMethod = new VwMonitorMethodRow(methodCd: methodList[caseDex], monLocId: methodLocId);
                MpParameters.LocationControlRecords = new CheckDataView<VwLocationControlRow>(locationControlRows); ;
                MpParameters.MethodDatesAndHoursConsistent = consistentList[caseDex];
                MpParameters.MethodMethodCodeValid = validList[caseDex];

                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Initialize variables needed to run the check. */
                bool log = false;
                string actual;

                /* Run Check */
                actual = target.METHOD35(category, ref log);

                /* Check results */
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);

                Assert.AreEqual(expectedResultList[caseDex], category.CheckCatalogResult, string.Format("Result {0}", caseDex));
           }
        }

        /// <summary>
        /// 
        /// MULT : Multiple matches
        /// </summary>
        [TestMethod()]
        public void Method39()
        {
          /* Initialize objects generally needed for testing checks. */
          cMethodChecks target = new cMethodChecks(new cMpCheckParameters());
          cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

          MpParameters.Init(category.Process);
          MpParameters.Category = category;

          /* Initialize General Variables */
          DateTime methodEvaluationBeginDatehour = new DateTime(2015, 1, 1, 12, 0, 0);
          DateTime methodEvaluationEndDatehour = new DateTime(2015, 3, 31, 11, 0, 0);
          string[] methodParameterArray = { "PASSED", "MIXED", "NONE" };

          /* Initialize General Parameters */
          MpParameters.MethodEvaluationBeginDate = methodEvaluationBeginDatehour.Date;
          MpParameters.MethodEvaluationBeginHour = methodEvaluationBeginDatehour.Hour;
          MpParameters.MethodEvaluationEndDate = methodEvaluationEndDatehour.Date;
          MpParameters.MethodEvaluationEndHour = methodEvaluationEndDatehour.Hour;

          /* Initialize Method Records */
          MpParameters.MethodRecords
            = new CheckDataView<VwMonitorMethodRow>
              (
                new VwMonitorMethodRow
                  (
                    parameterCd: "BEFORE",
                    beginDatehour: methodEvaluationBeginDatehour.AddMonths(-1), beginDate: methodEvaluationBeginDatehour.AddMonths(-1).Date, beginHour: methodEvaluationBeginDatehour.AddMonths(-1).Hour,
                    endDatehour: methodEvaluationBeginDatehour.AddDays(-1), endDate: methodEvaluationBeginDatehour.AddDays(-1).Date, endHour: methodEvaluationBeginDatehour.AddDays(-1).Hour
                  ),

                new VwMonitorMethodRow
                  (
                    parameterCd: "MATCH1", 
                    beginDatehour: methodEvaluationBeginDatehour.AddMonths(-1), beginDate: methodEvaluationBeginDatehour.AddMonths(-1).Date,  beginHour: methodEvaluationBeginDatehour.AddMonths(-1).Hour,
                    endDatehour: methodEvaluationBeginDatehour, endDate: methodEvaluationBeginDatehour.Date,  endHour: methodEvaluationBeginDatehour.Hour
                  ),

                new VwMonitorMethodRow
                  (
                    parameterCd: "NOMATCH",
                    beginDatehour: methodEvaluationBeginDatehour, beginDate: methodEvaluationBeginDatehour.Date, beginHour: methodEvaluationBeginDatehour.Hour,
                    endDatehour: methodEvaluationEndDatehour, endDate: methodEvaluationEndDatehour.Date, endHour: methodEvaluationEndDatehour.Hour
                  ),

                new VwMonitorMethodRow
                  (
                    parameterCd: "MATCH2", 
                    beginDatehour: methodEvaluationBeginDatehour, beginDate: methodEvaluationBeginDatehour.Date,  beginHour: methodEvaluationBeginDatehour.Hour,
                    endDatehour: methodEvaluationEndDatehour, endDate: methodEvaluationEndDatehour.Date,  endHour: methodEvaluationEndDatehour.Hour
                  ),

                new VwMonitorMethodRow
                  (
                    parameterCd: "MATCH3", 
                    beginDatehour: methodEvaluationEndDatehour, beginDate: methodEvaluationEndDatehour.Date,  beginHour: methodEvaluationEndDatehour.Hour,
                    endDatehour: methodEvaluationEndDatehour.AddMonths(1), endDate: methodEvaluationEndDatehour.AddMonths(1).Date,  endHour: methodEvaluationEndDatehour.AddMonths(1).Hour
                  ),

                new VwMonitorMethodRow
                  (
                    parameterCd: "AFTER",
                    beginDatehour: methodEvaluationEndDatehour.AddDays(1), beginDate: methodEvaluationEndDatehour.AddDays(1).Date, beginHour: methodEvaluationEndDatehour.AddDays(1).Hour,
                    endDatehour: methodEvaluationEndDatehour.AddMonths(1), endDate: methodEvaluationEndDatehour.AddMonths(1).Date, endHour: methodEvaluationEndDatehour.AddMonths(1).Hour
                  ),

                new VwMonitorMethodRow
                  (
                    parameterCd: "SAMEDAY",
                    beginDatehour: methodEvaluationBeginDatehour.AddMonths(-2), beginDate: methodEvaluationBeginDatehour.AddMonths(-2).Date, beginHour: methodEvaluationBeginDatehour.AddMonths(-2).Hour,
                    endDatehour: methodEvaluationEndDatehour, endDate: methodEvaluationEndDatehour.Date, endHour: methodEvaluationEndDatehour.Hour
                  ),

                new VwMonitorMethodRow
                  (
                    parameterCd: "DAYBEFORE",
                    beginDatehour: methodEvaluationBeginDatehour.AddMonths(-2).AddDays(-1), beginDate: methodEvaluationBeginDatehour.AddMonths(-2).AddDays(-1).Date, beginHour: methodEvaluationBeginDatehour.AddMonths(-2).AddDays(-1).Hour,
                    endDatehour: methodEvaluationEndDatehour, endDate: methodEvaluationEndDatehour.Date, endHour: methodEvaluationEndDatehour.Hour
                  )
              );

          /* Initialize Method Parameter Equivalent Crosscheck */
          MpParameters.MethodParameterEquivalentCrosscheck
            = new CheckDataView<MethodParameterEquivalentCrosscheckRow>
              (
                new MethodParameterEquivalentCrosscheckRow(parameterCode: "MIXED", equivalentCode: "AFTER"),
                new MethodParameterEquivalentCrosscheckRow(parameterCode: "MIXED", equivalentCode: "BEFORE"),
                new MethodParameterEquivalentCrosscheckRow(parameterCode: "MIXED", equivalentCode: "MATCH1"),
                new MethodParameterEquivalentCrosscheckRow(parameterCode: "MIXED", equivalentCode: "MATCH2"),
                new MethodParameterEquivalentCrosscheckRow(parameterCode: "MIXED", equivalentCode: "MATCH3"),
                new MethodParameterEquivalentCrosscheckRow(parameterCode: "MIXED", equivalentCode: "SAMEDAY"), // Will also match
                new MethodParameterEquivalentCrosscheckRow(parameterCode: "MIXED", equivalentCode: "DAYBEFORE"),
                new MethodParameterEquivalentCrosscheckRow(parameterCode: "PASSED", equivalentCode: "AFTER"),
                new MethodParameterEquivalentCrosscheckRow(parameterCode: "PASSED", equivalentCode: "BEFORE"),
                new MethodParameterEquivalentCrosscheckRow(parameterCode: "PASSED", equivalentCode: "DAYBEFORE")
              );

          /* Run Cases */
          foreach (string methodParameterCd in methodParameterArray)
            foreach (bool? methodDatesAndHoursConsistent in UnitTestStandardLists.ValidList)
            {
              /*  Initialize Input Parameters*/
              MpParameters.CurrentMethod 
                = new VwMonitorMethodRow
                  (
                    parameterCd: methodParameterCd, 
                    beginDatehour: methodEvaluationBeginDatehour.AddMonths(-2), beginDate: methodEvaluationBeginDatehour.AddMonths(-2).Date, beginHour: methodEvaluationBeginDatehour.AddMonths(-2).Hour
                  );
              MpParameters.MethodDatesAndHoursConsistent = methodDatesAndHoursConsistent;

              /* Initialize Output Parameter */
              MpParameters.OverlappingParameterList = null;

              /* Expected Results */
              string result = null;
              {
                if (methodDatesAndHoursConsistent.Default(false) && (methodParameterCd == "MIXED"))
                {
                  result = "A";
                }
              }

              /* Check Result Label */
              string resultPrefix = string.Format("[parameter: {0}, consistent: {1}]", methodParameterCd, methodDatesAndHoursConsistent);

              /* Init Cateogry Result */
              category.CheckCatalogResult = null;

              /* Initialize variables needed to run the check. */
              bool log = false;

              /* Run Check */
              string error = target.METHOD39(category, ref log);

              /* Check Results */
              Assert.AreEqual(string.Empty, error, resultPrefix + ".Error");
              Assert.AreEqual(false, log, resultPrefix + ".Log");
              Assert.AreEqual(result, category.CheckCatalogResult, resultPrefix + ".Result");

              Assert.AreEqual((result != null ? "MATCH1, MATCH2, MATCH3, and SAMEDAY" : null), MpParameters.OverlappingParameterList, resultPrefix + ".OverlappingParameterList : {0}");
            }
        }


        /// <summary>
        /// 
        /// Method Hours: MethB[2016-03-15 09], MethM[2016-05-01 15], MethE[2016-06-17 22]
        /// Fuel Dates:  FuelB[2016-03-14], FuelE[2016-06-18], 
        /// Inner Range Hours: RangB[2016-03-16 23], RangE[2016-06-16 00]
        ///     
        /// 
        ///                                     |     Method      |             Unit Fuel 1            |             Unit Fuel 2            |                Default 1                   |                Default 2                   ||
        /// | ## | Consis | EvalB    | EvalE    | Param | SubData | Code | Ind | Begin Dt  | End Dt    | Code | Ind | Begin Dt  | End Dt    | Param | Fuel | Pur | Begin Hr  | End Hr    | Param | Fuel | Pur | Begin Hr  | End Hr    || Result | Missing     | Incomplete || Notes
        /// |  0 | null   | MethB    | MethE    | NOXR  | FSP75   | NAT  | P   | FuelB     | null      | DES  | S   | FuelB     | null      | NOX   | PNG  | MD  | MethB     | null      | NOX   | DSL  | MD  | MethB     | null      || null   | empty       | empty      || MethodDatesAndHoursConsistent is null and therefore the body of the check does not run.
        /// |  1 | false  | MethB    | MethE    | NOXR  | FSP75   | NAT  | P   | FuelB     | null      | DES  | S   | FuelB     | null      | NOX   | PNG  | MD  | MethB     | null      | NOX   | DSL  | MD  | MethB     | null      || null   | empty       | empty      || MethodDatesAndHoursConsistent is false and therefore the body of the check does not run.
        /// |  2 | true   | MethB    | MethE    | NOXR  | FSP75   | NAT  | P   | FuelB     | null      | DES  | S   | FuelB     | null      | NOX   | PNG  | MD  | MethB     | null      | NOX   | DSL  | MD  | MethB     | null      || B      | NAT and DES | empty      || FSP75 Method exists, but defaults are missing for primary and secondary fuels.
        /// |  3 | true   | MethB    | MethE    | NOXR  | FSP75C  | NAT  | P   | FuelB     | null      | DES  | S   | FuelB     | null      | NOX   | PNG  | MD  | MethB     | null      | NOX   | DSL  | MD  | MethB     | null      || B      | NAT and DES | empty      || FSP75C Method exists, but defaults are missing for primary and secondary fuels.
        /// |  4 | true   | MethB    | MethE    | NOXR  | FSP75   | NAT  | P   | FuelB     | null      | DES  | S   | FuelB     | null      | NORX  | PNG  | MD  | MethB     | null      | NORX  | DSL  | MD  | MethB     | null      || null   | empty       | empty      || FSP75 Method exists and defaults exists for both the primary and secondary fuels.
        /// |  5 | true   | MethB    | MethE    | NOXR  | FSP75   | C    | P   | FuelB     | null      | DES  | S   | FuelB     | null      | NORX  | ANT  | MD  | MethB     | null      | NORX  | DSL  | MD  | MethB     | null      || null   | empty       | empty      || FSP75 Method exists and defaults exists for both the primary and secondary fuels.
        /// |  6 | true   | MethB    | MethE    | NOXR  | FSP75   | C    | P   | FuelB     | null      | DES  | S   | FuelB     | null      | NORX  | ANT  | MD  | MethB     | MethM - 1 | NORX  | SUB  | MD  | MethM + 1 | null      || A      | DES         | C          || FSP75 Method exists, but defaults are incomplete for primary and missing for secondary fuels.
        /// |  7 | true   | MethB    | MethE    | NOXR  | FSP75   | C    | P   | FuelB     | null      | DES  | F   | FuelB     | null      | NORX  | ANT  | MD  | MethB - 2 | MethB - 1 | NORX  | SUB  | MD  | MethE + 1 | Meth + 2  || B      | C           | empty      || FSP75 Method exists, but defaults begin before and after the method for the primary fuel.
        /// |  8 | true   | MethB    | MethE    | NOXR  | FSP75   | C    | P   | FuelB     | null      | DES  | F   | FuelB     | null      | NORX  | ANT  | MD  | MethB - 2 | MethB     | NORX  | SUB  | MD  | MethE + 1 | Meth + 2  || C      | empty       | C          || FSP75 Method exists, but one defaults ends on method begin hour and other begins after method end hour for the primary fuel.
        /// |  9 | true   | MethB    | MethE    | NOXR  | FSP75   | C    | P   | FuelB     | null      | DES  | F   | FuelB     | null      | NORX  | ANT  | MD  | MethB - 2 | MethB - 1 | NORX  | SUB  | MD  | MethE     | Meth + 2  || C      | empty       | C          || FSP75 Method exists, but one defaults ends before the method begin hour and other begins on the method end hour for the primary fuel.
        /// | 10 | true   | MethB    | MethE    | NOXR  | FSP75   | C    | P   | FuelB     | null      | DES  | F   | FuelB     | null      | NORX  | ANT  | MD  | MethB     | MethM - 1 | NORX  | SUB  | MD  | MethM + 1 | null      || C      | empty       | C          || FSP75 Method exists, but defaults have gap for the primary fuel.
        /// | 11 | true   | MethB    | MethE    | NOXR  | FSP75   | C    | P   | FuelB     | null      | DES  | F   | FuelB     | null      | NORX  | ANT  | MD  | MethB + 1 | MethM     | NORX  | SUB  | MD  | MethM + 1 | null      || C      | empty       | C          || FSP75 Method exists, but defaults begin an hour after the method begins for the primary fuel.
        /// | 12 | true   | MethB    | MethE    | NOXR  | FSP75   | C    | P   | FuelB     | null      | DES  | F   | FuelB     | null      | NORX  | ANT  | MD  | MethB     | MethM     | NORX  | SUB  | MD  | MethM + 1 | MethE - 1 || C      | empty       | C          || FSP75 Method exists, but defaults end an hour before the method ends for the primary fuel.
        /// | 13 | true   | MethB    | MethE    | NOXR  | FSP75   | C    | P   | FuelB     | null      | DES  | I   | FuelB     | null      | NORX  | ANT  | MD  | MethB     | null      | NORX  | DSL  | MD  | MethM + 1 | null      || null   | empty       | empty      || FSP75 Method and defaults exist for the primary fuel.
        /// | 14 | true   | MethB    | MethE    | NOXR  | FSP75   | C    | P   | FuelB     | null      | DES  | I   | FuelB     | null      | NORX  | ANT  | DC  | MethB     | null      | NORX  | DSL  | MD  | MethM + 1 | null      || B      | C           | empty      || FSP75 Method, but default has the wrong purpose code.
        /// | 15 | true   | MethB    | MethE    | NOXR  | FSP75   | C    | P   | FuelB     | null      | DES  | I   | FuelB     | null      | NORX  | ANT  | DM  | MethB     | null      | NORX  | DSL  | MD  | MethM + 1 | null      || B      | C           | empty      || FSP75 Method, but default has the wrong purpose code.
        /// | 16 | true   | MethB    | MethE    | NOXR  | FSP75   | C    | P   | FuelB     | null      | DES  | I   | FuelB     | null      | NORX  | ANT  | F23 | MethB     | null      | NORX  | DSL  | MD  | MethM + 1 | null      || B      | C           | empty      || FSP75 Method, but default has the wrong purpose code.
        /// | 17 | true   | MethB    | MethE    | NOXR  | FSP75   | C    | P   | FuelB     | null      | DES  | I   | FuelB     | null      | NORX  | ANT  | LM  | MethB     | null      | NORX  | DSL  | MD  | MethM + 1 | null      || B      | C           | empty      || FSP75 Method, but default has the wrong purpose code.
        /// | 18 | true   | MethB    | MethE    | NOXR  | FSP75   | C    | P   | FuelB     | null      | DES  | I   | FuelB     | null      | NORX  | ANT  | PM  | MethB     | null      | NORX  | DSL  | MD  | MethM + 1 | null      || B      | C           | empty      || FSP75 Method, but default has the wrong purpose code.
        /// | 19 | true   | MethB    | MethE    | NOXR  | FSP75   | C    | P   | FuelB     | null      | DES  | I   | FuelB     | null      | NORX  | ANT  | DC  | MethB + 1 | null      | NORX  | DSL  | MD  | MethM + 1 | null      || C      | empty       | C          || FSP75 Method for NOXR but default begin hour after method.
        /// | 20 | true   | MethB    | MethE    | NOX   | FSP75   | C    | P   | FuelB     | null      | DES  | I   | FuelB     | null      | NOCX  | ANT  | DC  | MethB + 1 | null      | NOCX  | DSL  | MD  | MethM + 1 | null      || null   | empty       | empty      || FSP75 Method for NOX, which does not have a cross check entry with a null component type, so no result was produced.
        /// | 21 | true   | MethB    | MethE    | NOXR  | FSP75   | BAD  | P   | FuelB     | null      | DES  | F   | FuelB     | null      | NORX  | ANT  | MD  | MethB     | null      | NORX  | SUB  | MD  | MethB     | null      || B      | BAD         | empty      || FSP75 Method exists, but primary default fuel does not exist in the fuel code lookup.
        /// | 22 | true   | RangB 00 | RangE 23 | NXOR  | FSP75   | C    | P   | RangB - 2 | RangB - 1 | DES  | F   | FuelB     | null      | NORX  | PNG  | MD  | MethB     | null      | NORX  | DSL  | MD  | MethB     | null      || null   | empty       | empty      || FSP75 Method exists, but primary default occurred before the method evaluation period.
        /// | 23 | true   | RangB 00 | RangE 23 | NXOR  | FSP75   | C    | P   | RangB - 2 | RangB     | DES  | F   | FuelB     | null      | NORX  | PNG  | MD  | MethB     | null      | NORX  | DSL  | MD  | MethB     | null      || B      | C           | empty      || FSP75 Method exists and default ends the day of the method evaluation period begin hour, but no default exists.
        /// | 24 | true   | RangB 00 | RangE 23 | NXOR  | FSP75   | C    | P   | RangE + 1 | RangE + 2 | DES  | F   | FuelB     | null      | NORX  | PNG  | MD  | MethB     | null      | NORX  | DSL  | MD  | MethB     | null      || null   | empty       | empty      || FSP75 Method exists, but primary default occurred after the method evaluation period.
        /// | 25 | true   | RangB 00 | RangE 23 | NXOR  | FSP75   | C    | P   | RangE     | RangE + 2 | DES  | F   | FuelB     | null      | NORX  | PNG  | MD  | MethB     | null      | NORX  | DSL  | MD  | MethB     | null      || B      | C           | empty      || FSP75 Method exists and default begins the day of the method evaluation period end hour, but no default exists.
        /// | 26 | true   | RangB 00 | RangE 23 | NXOR  | FSP75   | C    | P   | RangB     | RangE     | DES  | F   | FuelB     | null      | NORX  | ANT  | MD  | RangB     | MethM     | NORX  | SUB  | MD  | MethM + 1 | RangE     || null   | empty       | empty      || FSP75 Method exists, location date range within evaluation range, and defaults cover the location begin date 23rd hour through the end date 0 hour.
        /// | 27 | true   | RangB 00 | RangE 23 | NXOR  | FSP75   | C    | P   | RangB     | RangE     | DES  | F   | FuelB     | null      | NORX  | ANT  | MD  | RangB + 1 | MethM     | NORX  | SUB  | MD  | MethM + 1 | RangE     || C      | empty       | C          || FSP75 Method exists and location date range within evaluation range, but defaults do not cover the first hour of the day after the location begin date.
        /// | 28 | true   | RangB 00 | RangE 23 | NXOR  | FSP75   | C    | P   | RangB     | RangE     | DES  | F   | FuelB     | null      | NORX  | ANT  | MD  | RangB     | MethM     | NORX  | SUB  | MD  | MethM + 1 | RangE - 1 || C      | empty       | C          || FSP75 Method exists and location date range within evaluation range, but defaults do not cover the last hour of the day before the location end date.
        /// | 29 | true   | RangB 00 | RangE 23 | NXOR  | FSP75   | C    | P   | RangB     | RangE     | DES  | F   | FuelB     | null      | NORX  | ANT  | MD  | RangB     | MethM - 1 | NORX  | SUB  | MD  | MethM + 1 | RangE     || C      | empty       | C          || FSP75 Method exists and location date range within evaluation range, but defaults do not cover an hour between location begin date 23rd hour through the end date 0 hour.
        /// | 30 | true   | MethB    | MethE    | SO2   | FSP75   | C    | P   | FuelB     | null      | DES  | I   | FuelB     | null      | SORX  | ANT  | MD  | MethB     | null      | NORX  | DSL  | MD  | MethM + 1 | null      || null   | empty       | empty      || FSP75 Method and default exist for the primary fuel.
        /// | 31 | true   | MethB    | MethE    | SO2   | FSP75   | C    | P   | FuelB     | null      | DES  | I   | FuelB     | null      | SO2X  | ANT  | MD  | MethB     | null      | NORX  | DSL  | MD  | MethM + 1 | null      || B      | C           | empty      || FSP75 Method exists, default does not exist for the crosscheck with a null component type.
        /// </summary>
        [TestMethod()]
        public void Method40()
        {
            /* Initialize objects generally needed for testing checks. */
            cMethodChecks target = new cMethodChecks(new cMpCheckParameters());
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            MpParameters.Init(category.Process);
            MpParameters.Category = category;

            /* Input Parameter Values */
            DateTime fuelB = new DateTime(2016, 3, 14);
            DateTime fuelE = new DateTime(2016, 6, 18);
            DateTime methB = new DateTime(2016, 3, 15, 9, 0, 0);
            DateTime methE = new DateTime(2016, 6, 17, 22, 0, 0);
            DateTime methM = new DateTime(2016, 5, 1, 15, 0, 0);
            DateTime rangB = new DateTime(2016, 3, 16, 23, 0, 0);
            DateTime rangE = new DateTime(2016, 6, 16, 0, 0, 0);

            bool?[] consistentList = { null, false, true, true, true, true, true, true, true, true,
                                       true, true, true, true, true, true, true, true, true, true,
                                       true ,true, true, true, true, true, true, true, true, true,
                                       true, true };
            DateTime?[] evalBeginList = { methB, methB, methB, methB, methB, methB, methB, methB, methB, methB,
                                          methB, methB, methB, methB, methB, methB, methB, methB, methB, methB,
                                          methB, methB, rangB.Date, rangB.Date, rangB.Date, rangB.Date, rangB.Date, rangB.Date, rangB.Date, rangB.Date,
                                          methB, methB };
            DateTime?[] evalEndList = { methE, methE, methE, methE, methE, methE, methE, methE, methE, methE,
                                        methE, methE, methE, methE, methE, methE, methE, methE, methE, methE,
                                        methE, methE, rangE.Date.AddHours(23), rangE.Date.AddHours(23), rangE.Date.AddHours(23), rangE.Date.AddHours(23), rangE.Date.AddHours(23), rangE.Date.AddHours(23), rangE.Date.AddHours(23), rangE.Date.AddHours(23),
                                        methE, methE };
            string[] methParamList = { "NOXR", "NOXR", "NOXR", "NOXR", "NOXR", "NOXR", "NOXR", "NOXR", "NOXR", "NOXR",
                                       "NOXR", "NOXR", "NOXR", "NOXR", "NOXR", "NOXR", "NOXR", "NOXR", "NOXR", "NOXR",
                                       "NOX", "NOXR", "NOXR", "NOXR", "NOXR", "NOXR", "NOXR", "NOXR", "NOXR", "NOXR",
                                       "SO2", "SO2" };
            string[] methSubDataList = { "FSP75", "FSP75", "FSP75", "FSP75C", "FSP75", "FSP75", "FSP75", "FSP75", "FSP75", "FSP75",
                                         "FSP75", "FSP75", "FSP75", "FSP75", "FSP75", "FSP75", "FSP75", "FSP75", "FSP75", "FSP75",
                                         "FSP75", "FSP75", "FSP75", "FSP75", "FSP75", "FSP75", "FSP75", "FSP75", "FSP75", "FSP75",
                                         "FSP75", "FSP75" };
            string[] fuel1CodeList = { "NAT", "NAT", "NAT", "NAT", "NAT", "C", "C", "C", "C", "C",
                                       "C", "C", "C", "C", "C", "C", "C", "C", "C", "C",
                                       "C", "BAD", "C", "C", "C", "C", "C", "C", "C", "C",
                                       "C", "C" };
            string[] fuel1IndList = { "P", "P", "P", "P", "P", "P", "P", "P", "P", "P",
                                      "P", "P", "P", "P", "P", "P", "P", "P", "P", "P",
                                      "P", "P", "P", "P", "P", "P", "P", "P", "P", "P",
                                      "P", "P" };
            DateTime?[] fuel1BeginList = { fuelB, fuelB, fuelB, fuelB, fuelB, fuelB, fuelB, fuelB, fuelB, fuelB,
                                           fuelB, fuelB, fuelB, fuelB, fuelB, fuelB, fuelB, fuelB, fuelB, fuelB,
                                           fuelB, fuelB, rangB.Date.AddDays(-2), rangB.Date.AddDays(-2), rangE.Date.AddDays(1), rangE.Date, rangB.Date, rangB.Date, rangB.Date, rangB.Date,
                                           fuelB, fuelB };
            DateTime?[] fuel1EndList = { null, null, null, null, null, null, null, null, null, null,
                                         null, null, null, null, null, null, null, null, null, null,
                                         null, null, rangB.Date.AddDays(-1), rangB.Date, rangE.Date.AddDays(2), rangE.Date.AddDays(2), rangE.Date, rangE.Date, rangE.Date, rangE.Date,
                                         null, null};
            string[] fuel2CodeList = { "DES", "DES", "DES", "DES", "DES", "DES", "DES", "DES", "DES", "DES",
                                       "DES", "DES", "DES", "DES", "DES", "DES", "DES", "DES", "DES", "DES",
                                       "DES", "DES", "DES", "DES", "DES", "DES", "DES", "DES", "DES", "DES",
                                       "DES", "DES" };
            string[] fuel2IndList = { "S", "S", "S", "S", "S", "S", "S", "F", "F", "F",
                                      "F", "F", "F", "I", "I", "I", "I", "I", "I", "I",
                                      "I", "F", "F", "F", "F", "F", "F", "F", "F", "F",
                                      "I", "I" };
            DateTime?[] fuel2BeginList = { fuelB, fuelB, fuelB, fuelB, fuelB, fuelB, fuelB, fuelB, fuelB, fuelB,
                                           fuelB, fuelB, fuelB, fuelB, fuelB, fuelB, fuelB, fuelB, fuelB, fuelB,
                                           fuelB, fuelB, fuelB, fuelB, fuelB, fuelB, fuelB, fuelB, fuelB, fuelB,
                                           fuelB, fuelB };
            DateTime?[] fuel2EndList = { null, null, null, null, null, null, null, null, null, null,
                                         null, null, null, null, null, null, null, null, null, null,
                                         null, null, null, null, null, null, null, null, null, null,
                                         null, null };
            string[] def1ParamList = { "NOX", "NOX", "NOX", "NOX", "NORX", "NORX", "NORX", "NORX", "NORX", "NORX",
                                       "NORX", "NORX", "NORX", "NORX", "NORX", "NORX", "NORX", "NORX", "NORX", "NORX",
                                       "NOCX", "NORX", "NORX", "NORX", "NORX", "NORX", "NORX", "NORX", "NORX", "NORX",
                                       "SORX", "SO2X" };
            string[] def1FuelList = { "PNG", "PNG", "PNG", "PNG", "PNG", "ANT", "ANT", "ANT", "ANT", "ANT",
                                      "ANT", "ANT", "ANT", "ANT", "ANT", "ANT", "ANT", "ANT", "ANT", "ANT",
                                      "ANT", "ANT", "PNG", "PNG", "PNG", "PNG", "ANT", "ANT", "ANT", "ANT",
                                      "ANT", "ANT" };
            string[] def1PurposeList = { "MD", "MD", "MD", "MD", "MD", "MD", "MD", "MD", "MD", "MD",
                                         "MD", "MD", "MD", "MD", "DC", "DM", "F23", "LM", "PM", "MD",
                                         "MD", "MD", "MD", "MD", "MD", "MD", "MD", "MD", "MD", "MD",
                                         "MD", "MD" };
            DateTime?[] def1BeginList = { methB, methB, methB, methB, methB, methB, methB, methB.AddHours(-2), methB.AddHours(-2), methB.AddHours(-2),
                                          methB, methB.AddHours(1), methB, methB, methB, methB, methB, methB, methB, methB.AddHours(1),
                                          methB.AddHours(1), methB, methB, methB, methB, methB, rangB, rangB.AddHours(1), rangB, rangB,
                                          methB, methB };
            DateTime?[] def1EndList = { null, null, null, null, null, null, methM.AddHours(-1), methB.AddHours(-1), methB, methB.AddHours(-1),
                                        methM.AddHours(-1), methM, methM, null, null, null, null, null, null, null,
                                        null, null, null, null, null, null, methM, methM, methM, methM.AddHours(-1),
                                        null, null  };
            string[] def2ParamList = { "NOX", "NOX", "NOX", "NOX", "NORX", "NORX", "NORX", "NORX", "NORX", "NORX",
                                       "NORX", "NORX", "NORX", "NORX", "NORX", "NORX", "NORX", "NORX", "NORX", "NORX",
                                       "NOCX", "NORX", "NORX", "NORX", "NORX", "NORX", "NORX", "NORX", "NORX", "NORX",
                                       "NORX", "NORX" };
            string[] def2FuelList = { "DSL", "DSL", "DSL", "DSL", "DSL", "DSL", "SUB", "SUB", "SUB", "SUB",
                                      "SUB", "SUB", "SUB", "DSL", "DSL", "DSL", "DSL", "DSL", "DSL", "DSL",
                                      "DSL", "SUB", "DSL", "DSL", "DSL", "DSL", "SUB", "SUB", "SUB", "SUB",
                                      "DSL", "DSL" };
            string[] def2PurposeList = { "MD", "MD", "MD", "MD", "MD", "MD", "MD", "MD", "MD", "MD",
                                         "MD", "MD", "MD", "MD", "MD", "MD", "MD", "MD", "MD", "MD",
                                         "MD", "MD", "MD", "MD", "MD", "MD", "MD", "MD", "MD", "MD",
                                         "MD", "MD" };
            DateTime?[] def2BeginList = { methB, methB, methB, methB, methB, methB, methM.AddHours(1), methE.AddHours(1), methE.AddHours(1), methE,
                                          methM.AddHours(1), methM.AddHours(1), methM.AddHours(1), methM.AddHours(1), methM.AddHours(1), methM.AddHours(1), methM.AddHours(1), methM.AddHours(1), methM.AddHours(1), methM.AddHours(1),
                                          methM.AddHours(1), methB, methB, methB, methB, methB, methM.AddHours(1), methM.AddHours(1), methM.AddHours(1), methM.AddHours(1),
                                          methM.AddHours(1), methM.AddHours(1) };
            DateTime?[] def2EndList = { null, null, null, null, null, null, null, methE.AddHours(2), methE.AddHours(2), methE.AddHours(2),
                                        null, null, methE.AddHours(-1), null, null, null, null, null, null, null,
                                        null, null, null, null, null, null, rangE, rangE, rangE.AddHours(-1), rangE,
                                        null, null  };

            /* Expected Values */
            string[] expResultList = { null, null, "B", "B", null, null, "A", "B", "C", "C",
                                       "C", "C", "C", null, "B", "B", "B", "B", "B", "C",
                                       null, "B", null, "B", null, "B", null, "C", "C", "C",
                                       null, "B" };
            string[] expMissingList = { "", "", "NAT and DES", "NAT and DES", "", "", "DES", "C", "", "",
                                        "", "", "", "", "C", "C", "C", "C", "C", "",
                                        "", "BAD", "", "C", "", "C", "", "", "", "",
                                        "", "C"};
            string[] expIncompleteList = { "", "", "", "", "", "", "C", "", "C", "C",
                                           "C", "C", "C", "", "", "", "", "", "", "C",
                                           "", "", "", "", "", "", "", "C", "C", "C",
                                           "", "" };

            /* Test Case Count */
            int caseCount = 32;

            /* Check array lengths */
            Assert.AreEqual(caseCount, consistentList.Length, "consistentList length");
            Assert.AreEqual(caseCount, evalBeginList.Length, "evalBeginList length");
            Assert.AreEqual(caseCount, evalEndList.Length, "evalEndList length");
            Assert.AreEqual(caseCount, methParamList.Length, "methParamList length");
            Assert.AreEqual(caseCount, methSubDataList.Length, "methSubDataList length");
            Assert.AreEqual(caseCount, fuel1CodeList.Length, "fuel1CodeList length");
            Assert.AreEqual(caseCount, fuel1IndList.Length, "fuel1IndList length");
            Assert.AreEqual(caseCount, fuel1BeginList.Length, "fuel1BeginList length");
            Assert.AreEqual(caseCount, fuel1EndList.Length, "fuel1EndList length");
            Assert.AreEqual(caseCount, fuel2CodeList.Length, "fuel2CodeList length");
            Assert.AreEqual(caseCount, fuel2IndList.Length, "fuel2IndList length");
            Assert.AreEqual(caseCount, fuel2BeginList.Length, "fuel2BeginList length");
            Assert.AreEqual(caseCount, fuel2EndList.Length, "fuel2EndList length");
            Assert.AreEqual(caseCount, def1ParamList.Length, "def1ParamList length");
            Assert.AreEqual(caseCount, def1FuelList.Length, "def1FuelList length");
            Assert.AreEqual(caseCount, def1PurposeList.Length, "def1PurposeList length");
            Assert.AreEqual(caseCount, def1BeginList.Length, "def1BeginList length");
            Assert.AreEqual(caseCount, def1EndList.Length, "def1EndList length");
            Assert.AreEqual(caseCount, def2ParamList.Length, "def2ParamList length");
            Assert.AreEqual(caseCount, def2FuelList.Length, "def2FuelList length");
            Assert.AreEqual(caseCount, def2PurposeList.Length, "def2PurposeList length");
            Assert.AreEqual(caseCount, def2BeginList.Length, "def2BeginList length");
            Assert.AreEqual(caseCount, def2EndList.Length, "def2EndList length");
            Assert.AreEqual(caseCount, expResultList.Length, "expResultList length");
            Assert.AreEqual(caseCount, expMissingList.Length, "expMissingList length");
            Assert.AreEqual(caseCount, expIncompleteList.Length, "expIncompleteList length");

            /* Initialize Static Parameters */
            MpParameters.FuelCodeLookupTable = new CheckDataView<FuelCodeRow>
            (
                new FuelCodeRow(unitFuelCd: "NAT", fuelCd: "PNG"),
                new FuelCodeRow(unitFuelCd: "DES", fuelCd: "DSL"),
                new FuelCodeRow(unitFuelCd: "C", fuelCd: "ANT"),
                new FuelCodeRow(unitFuelCd: "C", fuelCd: "SUB")
            );
            MpParameters.MethodParameterToMaximumDefaultParameterLookupTable = new CheckDataView<MethodParameterToMaximumDefaultParameterToComponentTypeRow>
            (
                new MethodParameterToMaximumDefaultParameterToComponentTypeRow(methodParameterCode: "NOX", defaultParameterCode: "NOCX", componentTypeCode: "NOX"),
                new MethodParameterToMaximumDefaultParameterToComponentTypeRow(methodParameterCode: "NOXR", defaultParameterCode: "NORX", componentTypeCode: null),
                new MethodParameterToMaximumDefaultParameterToComponentTypeRow(methodParameterCode: "SO2", defaultParameterCode: "FLOX", componentTypeCode: "FLOW"),
                new MethodParameterToMaximumDefaultParameterToComponentTypeRow(methodParameterCode: "SO2", defaultParameterCode: "SO2X", componentTypeCode: "SO2"),
                new MethodParameterToMaximumDefaultParameterToComponentTypeRow(methodParameterCode: "SO2", defaultParameterCode: "SORX", componentTypeCode: null)
            );


            /* Run Cases */
            for (int caseDex = 0; caseDex < caseCount; caseDex++)
            {
                /* Initialize Input Parameters */
                MpParameters.CurrentMethod = new VwMonitorMethodRow(parameterCd: methParamList[caseDex], subDataCd: methSubDataList[caseDex]);
                MpParameters.DefaultRecords = new CheckDataView<VwMonitorDefaultRow>
                (
                    SetValues.DateHour(new VwMonitorDefaultRow(parameterCd: def1ParamList[caseDex], fuelCd: def1FuelList[caseDex], defaultPurposeCd: def1PurposeList[caseDex]), def1BeginList[caseDex], def1EndList[caseDex]),
                    SetValues.DateHour(new VwMonitorDefaultRow(parameterCd: def2ParamList[caseDex], fuelCd: def2FuelList[caseDex], defaultPurposeCd: def2PurposeList[caseDex]), def2BeginList[caseDex], def2EndList[caseDex])
                );
                MpParameters.LocationFuelRecords = new CheckDataView<VwLocationFuelRow>
                (
                    new VwLocationFuelRow(fuelCd: fuel1CodeList[caseDex], indicatorCd: fuel1IndList[caseDex], beginDate: fuel1BeginList[caseDex], endDate: fuel1EndList[caseDex]),
                    new VwLocationFuelRow(fuelCd: fuel2CodeList[caseDex], indicatorCd: fuel2IndList[caseDex], beginDate: fuel2BeginList[caseDex], endDate: fuel2EndList[caseDex])
                );
                MpParameters.MethodDatesAndHoursConsistent = consistentList[caseDex];
                MpParameters.MethodEvaluationBeginDate = evalBeginList[caseDex].Value.Date;
                MpParameters.MethodEvaluationBeginHour = evalBeginList[caseDex].Value.Hour;
                MpParameters.MethodEvaluationEndDate = evalEndList[caseDex].Value.Date;
                MpParameters.MethodEvaluationEndHour = evalEndList[caseDex].Value.Hour;

                /* Initialize Output Parameters */
                MpParameters.FuelsWithIncompleteDefaults = "Init List";
                MpParameters.FuelsWithMissingDefaults = "Init List";


                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Initialize variables needed to run the check. */
                bool log = false;
                string actual;

                /* Run Check */
                actual = target.METHOD40(category, ref log);


                /* Check Results */
                Assert.AreEqual(string.Empty, actual, string.Format("actual [{0}]", caseDex));
                Assert.AreEqual(false, log, string.Format("log [{0}]", caseDex));
                Assert.AreEqual(expResultList[caseDex], category.CheckCatalogResult, string.Format("category.CheckCatalogResult [{0}]", caseDex));

                Assert.AreEqual(expIncompleteList[caseDex], MpParameters.FuelsWithIncompleteDefaults, string.Format("FuelsWithIncompleteDefaults [{0}]", caseDex));
                Assert.AreEqual(expMissingList[caseDex], MpParameters.FuelsWithMissingDefaults, string.Format("FuelsWithMissingDefaults [{0}]", caseDex));
            }

        }
    }
}
