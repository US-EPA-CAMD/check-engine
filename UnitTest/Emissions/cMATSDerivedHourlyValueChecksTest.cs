using Microsoft.VisualStudio.TestTools.UnitTesting;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.EmissionsChecks;
using ECMPS.Checks.EmissionsReport;
using ECMPS.Definitions.Extensions;

using ECMPS.Checks.Data.Ecmps.CheckEm.Function;
using ECMPS.Checks.Data.Ecmps.Dbo.View;
using ECMPS.Checks.Em.Parameters;

using UnitTest.UtilityClasses;

namespace UnitTest.Emissions
{
    [TestClass()]
    public class cMATSDerivedHourlyValueChecksTest
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

		#region MATSDHV-1

		/// <summary>
		///A test for MATSDHV-1
        ///</summary>()
        [TestMethod()]
		public void MATSDHV1()
        {
			//static check setup
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
			EmParameters.Category = category;

            // Variables
            bool log = false;
            string actual;
            
            // Init Input
			EmParameters.MatsHgDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(locationName: "LOC1");
            
                // Init Output
                category.CheckCatalogResult = null;

                // Run Checks
				actual = cMATSDerivedHourlyValueChecks.MATSDHV1(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
				Assert.AreEqual("HGRE", EmParameters.CurrentDhvParameter, "CurrentDhvParameter");
				Assert.AreEqual("LOC1", EmParameters.MatsDhvRecord.LocationName, "MatsDhvRecord");
				Assert.AreEqual("A-3", EmParameters.MatsEquationCodeWithH2o, "MatsEquationCodeWithH2o");
				Assert.AreEqual("A-2", EmParameters.MatsEquationCodeWithoutH2o, "MatsEquationCodeWithoutH2o");
        }
        #endregion

		#region MATSDHV-2

		/// <summary>
		///A test for MATSDHV-2
		///</summary>()
		[TestMethod()]
		public void MATSDHV2()
		{
			//static check setup
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			EmParameters.Init(category.Process);
			EmParameters.Category = category;

			// Variables
			bool log = false;
			string actual;

			// Init Input
			EmParameters.MatsHgDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(locationName: "LOC1");

			// Init Output
			category.CheckCatalogResult = null;

			// Run Checks
			actual = cMATSDerivedHourlyValueChecks.MATSDHV2(category, ref log);

			// Check Results
			Assert.AreEqual(string.Empty, actual);
			Assert.AreEqual(false, log);
			Assert.AreEqual(null, category.CheckCatalogResult, "Result");
			Assert.AreEqual("HGRH", EmParameters.CurrentDhvParameter, "CurrentDhvParameter");
			Assert.AreEqual("LOC1", EmParameters.MatsDhvRecord.LocationName, "MatsDhvRecord");
		}
		#endregion

		#region MATSDHV-3

		/// <summary>
		///A test for MATSDHV-3
		///</summary>()
		[TestMethod()]
		public void MATSDHV3()
		{
			//static check setup
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			EmParameters.Init(category.Process);
			EmParameters.Category = category;

			// Variables
			bool log = false;
			string actual;

			// Init Input
			EmParameters.MatsHclDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(locationName: "LOC1");

			// Init Output
			category.CheckCatalogResult = null;

			// Run Checks
			actual = cMATSDerivedHourlyValueChecks.MATSDHV3(category, ref log);

			// Check Results
			Assert.AreEqual(string.Empty, actual);
			Assert.AreEqual(false, log);
			Assert.AreEqual(null, category.CheckCatalogResult, "Result");
			Assert.AreEqual("HCLRE", EmParameters.CurrentDhvParameter, "CurrentDhvParameter");
			Assert.AreEqual("LOC1", EmParameters.MatsDhvRecord.LocationName, "MatsDhvRecord");
			Assert.AreEqual("HC-3", EmParameters.MatsEquationCodeWithH2o, "MatsEquationCodeWithH2o");
			Assert.AreEqual("HC-2", EmParameters.MatsEquationCodeWithoutH2o, "MatsEquationCodeWithoutH2o");
		}
		#endregion

		#region MATSDHV-4

		/// <summary>
		///A test for MATSDHV-4
		///</summary>()
		[TestMethod()]
		public void MATSDHV4()
		{
			//static check setup
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			EmParameters.Init(category.Process);
			EmParameters.Category = category;

			// Variables
			bool log = false;
			string actual;

			// Init Input
			EmParameters.MatsHclDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(locationName: "LOC1");

			// Init Output
			category.CheckCatalogResult = null;

			// Run Checks
			actual = cMATSDerivedHourlyValueChecks.MATSDHV4(category, ref log);

			// Check Results
			Assert.AreEqual(string.Empty, actual);
			Assert.AreEqual(false, log);
			Assert.AreEqual(null, category.CheckCatalogResult, "Result");
			Assert.AreEqual("HCLRH", EmParameters.CurrentDhvParameter, "CurrentDhvParameter");
			Assert.AreEqual("LOC1", EmParameters.MatsDhvRecord.LocationName, "MatsDhvRecord");
		}
		#endregion

		#region MATSDHV-5

		/// <summary>
		///A test for MATSDHV-5
		///</summary>()
		[TestMethod()]
		public void MATSDHV5()
		{
			//static check setup
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			EmParameters.Init(category.Process);
			EmParameters.Category = category;

			// Variables
			bool log = false;
			string actual;

			// Init Input
			EmParameters.MatsHfDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(locationName: "LOC1");

			// Init Output
			category.CheckCatalogResult = null;

			// Run Checks
			actual = cMATSDerivedHourlyValueChecks.MATSDHV5(category, ref log);

			// Check Results
			Assert.AreEqual(string.Empty, actual);
			Assert.AreEqual(false, log);
			Assert.AreEqual(null, category.CheckCatalogResult, "Result");
			Assert.AreEqual("HFRE", EmParameters.CurrentDhvParameter, "CurrentDhvParameter");
			Assert.AreEqual("LOC1", EmParameters.MatsDhvRecord.LocationName, "MatsDhvRecord");
			Assert.AreEqual("HF-3", EmParameters.MatsEquationCodeWithH2o, "MatsEquationCodeWithH2o");
			Assert.AreEqual("HF-2", EmParameters.MatsEquationCodeWithoutH2o, "MatsEquationCodeWithoutH2o");
		}
		#endregion

		#region MATSDHV-6

		/// <summary>
		///A test for MATSDHV-6
		///</summary>()
		[TestMethod()]
		public void MATSDHV6()
		{
			//static check setup
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			EmParameters.Init(category.Process);
			EmParameters.Category = category;

			// Variables
			bool log = false;
			string actual;

			// Init Input
			EmParameters.MatsHfDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(locationName: "LOC1");

			// Init Output
			category.CheckCatalogResult = null;

			// Run Checks
			actual = cMATSDerivedHourlyValueChecks.MATSDHV6(category, ref log);

			// Check Results
			Assert.AreEqual(string.Empty, actual);
			Assert.AreEqual(false, log);
			Assert.AreEqual(null, category.CheckCatalogResult, "Result");
			Assert.AreEqual("HFRH", EmParameters.CurrentDhvParameter, "CurrentDhvParameter");
			Assert.AreEqual("LOC1", EmParameters.MatsDhvRecord.LocationName, "MatsDhvRecord");
		}
		#endregion

		#region MATSDHV-7

		/// <summary>
		///A test for MATSDHV-7
		///</summary>()
		[TestMethod()]
		public void MATSDHV7()
		{
			//static check setup
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			EmParameters.Init(category.Process);
			EmParameters.Category = category;

			// Variables
			bool log = false;
			string actual;

			// Init Input
			EmParameters.MatsSo2DhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(locationName: "LOC1");

			// Init Output
			category.CheckCatalogResult = null;

			// Run Checks
			actual = cMATSDerivedHourlyValueChecks.MATSDHV7(category, ref log);

			// Check Results
			Assert.AreEqual(string.Empty, actual);
			Assert.AreEqual(false, log);
			Assert.AreEqual(null, category.CheckCatalogResult, "Result");
			Assert.AreEqual("SO2RE", EmParameters.CurrentDhvParameter, "CurrentDhvParameter");
			Assert.AreEqual("LOC1", EmParameters.MatsDhvRecord.LocationName, "MatsDhvRecord");
			Assert.AreEqual("S-3", EmParameters.MatsEquationCodeWithH2o, "MatsEquationCodeWithH2o");
			Assert.AreEqual("S-2", EmParameters.MatsEquationCodeWithoutH2o, "MatsEquationCodeWithoutH2o");
		}
		#endregion

		#region MATSDHV-8

		/// <summary>
		///A test for MATSDHV-8
		///</summary>()
		[TestMethod()]
		public void MATSDHV8()
		{
			//static check setup
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			EmParameters.Init(category.Process);
			EmParameters.Category = category;

			// Variables
			bool log = false;
			string actual;

			// Init Input
			EmParameters.MatsSo2DhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(locationName: "LOC1");

			// Init Output
			category.CheckCatalogResult = null;

			// Run Checks
			actual = cMATSDerivedHourlyValueChecks.MATSDHV8(category, ref log);

			// Check Results
			Assert.AreEqual(string.Empty, actual);
			Assert.AreEqual(false, log);
			Assert.AreEqual(null, category.CheckCatalogResult, "Result");
			Assert.AreEqual("SO2RH", EmParameters.CurrentDhvParameter, "CurrentDhvParameter");
			Assert.AreEqual("LOC1", EmParameters.MatsDhvRecord.LocationName, "MatsDhvRecord");
		}
        #endregion


        /// <summary>
        /// MatsDhv-9
        /// 
        /// | ## | MODC | Parameter | StartShut || Result | Status || Note
        /// |  0 | 35   | null      | null      || A      | false  || Bad MODC code
        /// |  1 | 36   | null      | null      || null   | true   || Good MODC for which parameter and startup/shutdown are not checked
        /// |  2 | 37   | null      | null      || B      | false  || Good MODC for which parameter and startup/shutdown are checked ans are bad.
        /// |  3 | 38   | null      | null      || null   | true   || Good MODC for which parameter and startup/shutdown are not checked
        /// |  4 | 39   | null      | null      || C      | false  || Good MODC for which parameter and startup/shutdown are checked ans are bad.
        /// |  5 | 40   | null      | null      || A      | false  || Bad MODC code
        /// |  6 | 37   | HGRH      | "U"       || null   | true   || Good MODC, parameter and startup/shutdown.
        /// |  7 | 37   | HCLRH     | "D"       || null   | true   || Good MODC, parameter and startup/shutdown.
        /// |  8 | 37   | HFRH      | "U"       || null   | true   || Good MODC, parameter and startup/shutdown.
        /// |  9 | 37   | SO2RH     | "D"       || null   | true   || Good MODC, parameter and startup/shutdown.
        /// | 10 | 37   | HGRE      | "U"       || B      | false  || Good MODC and startup/shutdown, but bad parameter.
        /// | 11 | 37   | HCLRE     | "D"       || B      | false  || Good MODC and startup/shutdown, but bad parameter.
        /// | 12 | 37   | HFRE      | "U"       || B      | false  || Good MODC and startup/shutdown, but bad parameter.
        /// | 13 | 37   | SO2RE     | "D"       || B      | false  || Good MODC and startup/shutdown, but bad parameter.
        /// | 14 | 37   | HGRH      | null      || B      | false  || Good MODC and parameter, but bad startup/shutdown.
        /// | 15 | 39   | HGRE      | "D"       || null   | true   || Good MODC, parameter and startup/shutdown.
        /// | 16 | 39   | HCLRE     | "U"       || null   | true   || Good MODC, parameter and startup/shutdown.
        /// | 17 | 39   | HFRE      | "D"       || null   | true   || Good MODC, parameter and startup/shutdown.
        /// | 18 | 39   | SO2RE     | "U"       || null   | true   || Good MODC, parameter and startup/shutdown.
        /// | 19 | 39   | HGRH      | "D"       || C      | false  || Good MODC and startup/shutdown, but bad parameter.
        /// | 20 | 39   | HCLRH     | "U"       || C      | false  || Good MODC and startup/shutdown, but bad parameter.
        /// | 21 | 39   | HFRH      | "D"       || C      | false  || Good MODC and startup/shutdown, but bad parameter.
        /// | 22 | 39   | SO2RH     | "U"       || C      | false  || Good MODC and startup/shutdown, but bad parameter.
        /// | 23 | 39   | HGRE      | null      || C      | false  || Good MODC and parameter, but bad startup/shutdown.
        /// </summary>
        [TestMethod()]
        public void MatsDhv9()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;


            /* Input Parameter Values */
            string[] modcList = { "35", "36", "37", "38", "39", "40", "37", "37", "37", "37", "37", "37", "37", "37", "37", "39", "39", "39", "39", "39", "39", "39", "39", "39" };
            string[] parameterList = { null, null, null, null, null, null, "HGRH", "HCLRH", "HFRH", "SO2RH", "HGRE", "HCLRE", "HFRE", "SO2RE", "HGRH", "HGRE", "HCLRE", "HFRE", "SO2RE", "HGRH", "HCLRH", "HFRH", "SO2RH", "HGRE" };
            string[] upDownList = { null, null, null, null, null, null, "U", "D", "U", "D", "U", "D", "U", "D", null, "D", "U", "D", "U", "D", "U", "D", "U", null };

            /* Expected Values */
            string[] expResultList = { "A", null, "B", null, "C", "A", null, null, null, null, "B", "B", "B", "B", "B", null, null, null, null, "C", "C", "C", "C", "C" };
            bool[] expStatusList = { false, true, false, true, false, false, true, true, true, true, false, false, false, false, false, true, true, true, true, false, false, false, false, false };

            /* Test Case Count */
            int caseCount = 24;

            /* Check array lengths */
            Assert.AreEqual(caseCount, modcList.Length, "modcList length");
            Assert.AreEqual(caseCount, parameterList.Length, "parameterList length");
            Assert.AreEqual(caseCount, upDownList.Length, "upDownList length");
            Assert.AreEqual(caseCount, expResultList.Length, "expResultList length");
            Assert.AreEqual(caseCount, expStatusList.Length, "expStatusList length");


            /* Run Cases */
            for (int caseDex = 0; caseDex < caseCount; caseDex++)
            {
                /* Initialize Input Parameters */
                EmParameters.CurrentHourlyOpRecord = new VwMpHrlyOpDataRow(matsStartupShutdownFlg: upDownList[caseDex]);
                EmParameters.MatsDhvRecord = new MATSDerivedHourlyValueData(modcCd: modcList[caseDex], parameterCd: parameterList[caseDex]);

                /* Initialize Ouput Parameters */
                EmParameters.DerivedHourlyModcStatus = null;


                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Initialize variables needed to run the check. */
                bool log = false;
                string actual;

                /* Run Check */
                actual = cMATSDerivedHourlyValueChecks.MATSDHV9(category, ref log);


                /* Check Results */
                Assert.AreEqual(string.Empty, actual, string.Format("actual[case {0}]", caseDex));
                Assert.AreEqual(false, log, string.Format("log [case {0}]", caseDex));
                Assert.AreEqual(expResultList[caseDex], category.CheckCatalogResult, string.Format("category.CheckCatalogResult [case {0}]", caseDex));

                Assert.AreEqual(expStatusList[caseDex], EmParameters.DerivedHourlyModcStatus, string.Format("DerivedHourlyModcStatus [case {0}]", caseDex));
            }
        }


        /// <summary>
        /// 
        /// CUrrentDhvParameter     : [HCLRE, HCLRH, HFRE, HFRH, HGRE, HGRH, SO2RE, SO2RH]
        /// DerivedHourlyModcStatus : { null, false, true}
        /// MatsDhvRecord           : { { FormulaId: [null, GoodId], FormulaActiveInd: [null, 0, 1], ModcCode: [36, 37, 38, 39], FormulaParameter: [HCLRE, HCLRH, HFRE, HFRH, HGRE, HGRH, SO2RE, SO2RH] }
        /// 
        /// | ## | Status | FormId | Active | Modc | FParam | CParam || Result | Status || Note
        /// |  0 | null   | null   | false  | 36   | HGRE   | HGRE   || null   | false  || Null DerivedHourlyModcStatus
        /// |  1 | false  | null   | false  | 36   | HGRE   | HGRE   || null   | false  || False DerivedHourlyModcStatus
        /// |  2 | true   | null   | false  | 36   | HGRE   | HGRE   || A      | false  || null formula id with MODC 36
        /// |  3 | true   | null   | false  | 37   | HGRE   | HGRE   || A      | false  || null formula id with MODC 37
        /// |  4 | true   | null   | false  | 38   | HGRE   | HGRE   || G      | false  || null formula id with MODC 38
        /// |  5 | true   | null   | false  | 39   | HGRE   | HGRE   || A      | false  || null formula id with MODC 39
        /// |  6 | true   | GoodId | null   | 36   | HGRE   | HGRE   || B      | false  || Formula is not active.
        /// |  7 | true   | GoodId | false  | 36   | HGRE   | HGRE   || B      | false  || Formula is not active.
        /// |  8 | true   | GoodId | true   | 36   | HGRE   | HGRH   || C      | false  || Parameters do not match.
        /// |  9 | true   | GoodId | true   | 36   | HGRE   | HCLRE  || C      | false  || Parameters do not match.
        /// | 10 | true   | GoodId | true   | 36   | HGRE   | HFRE   || C      | false  || Parameters do not match.
        /// | 11 | true   | GoodId | true   | 36   | HGRE   | SO2RE  || C      | false  || Parameters do not match.
        /// | 12 | true   | GoodId | true   | 36   | HGRH   | HGRE   || C      | false  || Parameters do not match.
        /// | 13 | true   | GoodId | true   | 36   | HGRH   | HCLRH  || C      | false  || Parameters do not match.
        /// | 14 | true   | GoodId | true   | 36   | HGRH   | HFRH   || C      | false  || Parameters do not match.
        /// | 15 | true   | GoodId | true   | 36   | HGRH   | SO2RH  || C      | false  || Parameters do not match.
        /// | 16 | true   | GoodId | true   | 36   | HGRE   | HGRE   || null   | true   || MODC 36 and HGRE.
        /// | 17 | true   | GoodId | true   | 36   | HCLRE  | HCLRE  || null   | true   || MODC 36 and HCLRE.
        /// | 18 | true   | GoodId | true   | 36   | HFRE   | HFRE   || null   | true   || MODC 36 and HFRE.
        /// | 19 | true   | GoodId | true   | 36   | SO2RE  | SO2RE  || null   | true   || MODC 36 and SO2RE.
        /// | 20 | true   | GoodId | true   | 36   | HGRH   | HGRH   || null   | true   || MODC 36 and HGRH.
        /// | 21 | true   | GoodId | true   | 36   | HCLRH  | HCLRH  || null   | true   || MODC 36 and HCLRH.
        /// | 22 | true   | GoodId | true   | 36   | HFRH   | HFRH   || null   | true   || MODC 36 and HFRH.
        /// | 23 | true   | GoodId | true   | 36   | SO2RH  | SO2RH  || null   | true   || MODC 36 and SO2RH.
        /// | 24 | true   | GoodId | true   | 37   | HGRE   | HGRE   || D      | false  || MODC 37 and HGRE.
        /// | 25 | true   | GoodId | true   | 37   | HCLRE  | HCLRE  || D      | false  || MODC 37 and HCLRE.
        /// | 26 | true   | GoodId | true   | 37   | HFRE   | HFRE   || D      | false  || MODC 37 and HFRE.
        /// | 27 | true   | GoodId | true   | 37   | SO2RE  | SO2RE  || D      | false  || MODC 37 and SO2RE.
        /// | 28 | true   | GoodId | true   | 37   | HGRH   | HGRH   || null   | true   || MODC 37 and HGRH.
        /// | 29 | true   | GoodId | true   | 37   | HCLRH  | HCLRH  || null   | true   || MODC 37 and HCLRH.
        /// | 30 | true   | GoodId | true   | 37   | HFRH   | HFRH   || null   | true   || MODC 37 and HFRH.
        /// | 31 | true   | GoodId | true   | 37   | SO2RH  | SO2RH  || null   | true   || MODC 37 and SO2RH.
        /// | 32 | true   | GoodId | true   | 38   | HGRE   | HGRE   || null   | true   || MODC 38 and HGRE.
        /// | 33 | true   | GoodId | true   | 38   | HCLRE  | HCLRE  || null   | true   || MODC 38 and HCLRE.
        /// | 34 | true   | GoodId | true   | 38   | HFRE   | HFRE   || null   | true   || MODC 38 and HFRE.
        /// | 35 | true   | GoodId | true   | 38   | SO2RE  | SO2RE  || null   | true   || MODC 38 and SO2RE.
        /// | 36 | true   | GoodId | true   | 38   | HGRH   | HGRH   || null   | true   || MODC 38 and HGRH.
        /// | 37 | true   | GoodId | true   | 38   | HCLRH  | HCLRH  || null   | true   || MODC 38 and HCLRH.
        /// | 38 | true   | GoodId | true   | 38   | HFRH   | HFRH   || null   | true   || MODC 38 and HFRH.
        /// | 39 | true   | GoodId | true   | 38   | SO2RH  | SO2RH  || null   | true   || MODC 38 and SO2RH.
        /// | 40 | true   | GoodId | true   | 39   | HGRE   | HGRE   || null   | true   || MODC 39 and HGRE.
        /// | 41 | true   | GoodId | true   | 39   | HCLRE  | HCLRE  || null   | true   || MODC 39 and HCLRE.
        /// | 42 | true   | GoodId | true   | 39   | HFRE   | HFRE   || null   | true   || MODC 39 and HFRE.
        /// | 43 | true   | GoodId | true   | 39   | SO2RE  | SO2RE  || null   | true   || MODC 39 and SO2RE.
        /// | 44 | true   | GoodId | true   | 39   | HGRH   | HGRH   || E      | false  || MODC 39 and HGRH.
        /// | 45 | true   | GoodId | true   | 39   | HCLRH  | HCLRH  || E      | false  || MODC 39 and HCLRH.
        /// | 46 | true   | GoodId | true   | 39   | HFRH   | HFRH   || E      | false  || MODC 39 and HFRH.
        /// | 47 | true   | GoodId | true   | 39   | SO2RH  | SO2RH  || E      | false  || MODC 39 and SO2RH.
        /// 
        /// </summary>
        [TestMethod()]
        public void MatsDhv10()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;


            /* Input Parameter Values */
            bool?[] statusList = { null, false, true, true, true, true, true, true, true, true,
                                   true, true, true, true, true, true, true, true, true, true,
                                   true, true, true, true, true, true, true, true, true, true,
                                   true, true, true, true, true, true, true, true, true, true,
                                   true, true, true, true, true, true, true, true };
            string[] formIdList = { null, null, null, null, null, null, "GoodId", "GoodId", "GoodId", "GoodId",
                                    "GoodId", "GoodId", "GoodId", "GoodId", "GoodId", "GoodId", "GoodId", "GoodId", "GoodId", "GoodId",
                                    "GoodId", "GoodId", "GoodId", "GoodId", "GoodId", "GoodId", "GoodId", "GoodId", "GoodId", "GoodId",
                                    "GoodId", "GoodId", "GoodId", "GoodId", "GoodId", "GoodId", "GoodId", "GoodId", "GoodId", "GoodId",
                                    "GoodId", "GoodId", "GoodId", "GoodId", "GoodId", "GoodId", "GoodId", "GoodId" };
            int?[] activeList = { 0, 0, 0, 0, 0, 0, null, 0, 1, 1,
                                  1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
                                  1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
                                  1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
                                  1, 1, 1, 1, 1, 1, 1, 1 };
            string[] modcList = { "36", "36", "36", "37", "38", "39", "36", "36", "36", "36",
                                  "36", "36", "36", "36", "36", "36", "36", "36", "36", "36",
                                  "36", "36", "36", "36", "37", "37", "37", "37", "37", "37",
                                  "37", "37", "38", "38", "38", "38", "38", "38", "38", "38",
                                  "39", "39", "39", "39", "39", "39", "39", "39" };
            string[] dhvParamList = { "HGRE", "HGRE", "HGRE", "HGRE", "HGRE", "HGRE", "HGRE", "HGRE", "HGRE", "HGRE",
                                      "HGRE", "HGRE", "HGRH", "HGRH", "HGRH", "HGRH", "HGRE", "HCLRE", "HFRE", "SO2RE",
                                      "HGRH", "HCLRH", "HFRH", "SO2RH", "HGRE", "HCLRE", "HFRE", "SO2RE", "HGRH", "HCLRH",
                                      "HFRH", "SO2RH", "HGRE", "HCLRE", "HFRE", "SO2RE", "HGRE", "HGRE", "HGRE", "HGRE",
                                      "HGRE", "HCLRE", "HFRE", "SO2RE", "HGRH", "HCLRH", "HFRH", "SO2RH" };
            string[] chkParamList = { "HGRE", "HGRE", "HGRE", "HGRE", "HGRE", "HGRE", "HGRE", "HGRE", "HGRH", "HCLRE",
                                      "HFRE", "SO2RE", "HGRE", "HCLRH", "HFRH", "SO2RH", "HGRE", "HCLRE", "HFRE", "SO2RE",
                                      "HGRH", "HCLRH", "HFRH", "SO2RH", "HGRE", "HCLRE", "HFRE", "SO2RE", "HGRH", "HCLRH",
                                      "HFRH", "SO2RH", "HGRE", "HCLRE", "HFRE", "SO2RE", "HGRE", "HGRE", "HGRE", "HGRE",
                                      "HGRE", "HCLRE", "HFRE", "SO2RE", "HGRH", "HCLRH", "HFRH", "SO2RH" };

            /* Expected Values */
            string[] expResultList = { null, null, "A", "A", "G", "A", "B", "B", "C", "C",
                                       "C", "C", "C", "C", "C", "C", null, null, null, null,
                                       null, null, null, null, "D", "D", "D", "D", null, null,
                                       null, null, null, null, null, null, null, null, null, null,
                                       null, null, null, null, "E", "E", "E", "E" };
            bool?[] expStatusList = { false, false, false, false, false, false, false, false, false, false,
                                      false, false, false, false, false, false, true, true, true, true,
                                      true, true, true, true, false, false, false, false, true, true,
                                      true, true, true, true, true, true, true, true, true, true,
                                      true, true, true, true, false, false, false, false };

            /* Test Case Count */
            int caseCount = 48;

            /* Check array lengths */
            Assert.AreEqual(caseCount, statusList.Length, "statusList length");
            Assert.AreEqual(caseCount, formIdList.Length, "formIdList length");
            Assert.AreEqual(caseCount, activeList.Length, "activeList length");
            Assert.AreEqual(caseCount, modcList.Length, "modcList length");
            Assert.AreEqual(caseCount, dhvParamList.Length, "dhvParamList length");
            Assert.AreEqual(caseCount, chkParamList.Length, "chkParamList length");
            Assert.AreEqual(caseCount, expResultList.Length, "expResultList length");
            Assert.AreEqual(caseCount, expStatusList.Length, "expStatusList length");


            /* Run Cases */
            for (int caseDex = 0; caseDex < caseCount; caseDex++)
            {
                /* Initialize Input Parameters */
                EmParameters.CurrentDhvParameter = chkParamList[caseDex];
                EmParameters.DerivedHourlyModcStatus = statusList[caseDex];
                EmParameters.MatsDhvRecord = new MATSDerivedHourlyValueData(monFormId: formIdList[caseDex], formulaActiveInd: activeList[caseDex], modcCd: modcList[caseDex], formulaParameterCd: dhvParamList[caseDex]);

                /* Initialize Ouput Parameters */
                EmParameters.DerivedHourlyFormulaStatus = null;


                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Initialize variables needed to run the check. */
                bool log = false;
                string actual;

                /* Run Check */
                actual = cMATSDerivedHourlyValueChecks.MATSDHV10(category, ref log);


                /* Check Results */
                Assert.AreEqual(string.Empty, actual, string.Format("actual[case {0}]", caseDex));
                Assert.AreEqual(false, log, string.Format("log [case {0}]", caseDex));
                Assert.AreEqual(expResultList[caseDex], category.CheckCatalogResult, string.Format("category.CheckCatalogResult [case {0}]", caseDex));

                Assert.AreEqual(expStatusList[caseDex], EmParameters.DerivedHourlyFormulaStatus, string.Format("DerivedHourlyFormulaStatus [case {0}]", caseDex));
            }
        }

		#endregion


		#region Checks 11-20

		#region MATSDHV-11

		/// <summary>
		///A test for MATSDHV-11
		///</summary>()
		[TestMethod()]
		public void MATSDHV11()
		{
			//static check setup
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			EmParameters.Init(category.Process);
			EmParameters.Category = category;

			// Variables
			bool log = false;
			string actual;

			//NOH2O
			{
				// Init Input
				EmParameters.DerivedHourlyFormulaStatus = true;
				EmParameters.MatsEquationCodeWithoutH2o = "NOH2O";
				EmParameters.MatsEquationCodeWithH2o = "H2O";
				EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData
					(equationCd: "NOH2O");

				// Init Output
				category.CheckCatalogResult = null;
				EmParameters.FlowMonitorHourlyChecksNeeded = false;
				EmParameters.MoistureNeeded = false;
				EmParameters.H2oMissingDataApproach = "";

				// Run Checks
				actual = cMATSDerivedHourlyValueChecks.MATSDHV11(category, ref log);

				// Check Results
				Assert.AreEqual(false, log);
				Assert.AreEqual(null, category.CheckCatalogResult, "Result");
				Assert.AreEqual(true, EmParameters.DerivedHourlyEquationStatus, "DerivedHourlyEquationStatus");
				Assert.AreEqual(true, EmParameters.FlowMonitorHourlyChecksNeeded, "FlowMonitorHourlyChecksNeeded");
				Assert.AreEqual(false, EmParameters.MoistureNeeded, "MoistureNeeded");
				Assert.AreEqual("", EmParameters.H2oMissingDataApproach, "MissingDataApproach");
			}

			// H2O
			{
				// Init Input
				EmParameters.DerivedHourlyFormulaStatus = true;
				EmParameters.MatsEquationCodeWithoutH2o = "NOH2O";
				EmParameters.MatsEquationCodeWithH2o = "H2O";
				EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData
					(equationCd: "H2O");

				// Init Output
				category.CheckCatalogResult = null;
				EmParameters.FlowMonitorHourlyChecksNeeded = false;
				EmParameters.MoistureNeeded = false;
				EmParameters.H2oMissingDataApproach = "";

				// Run Checks
				actual = cMATSDerivedHourlyValueChecks.MATSDHV11(category, ref log);

				// Check Results
				Assert.AreEqual(false, log);
				Assert.AreEqual(null, category.CheckCatalogResult, "Result");
				Assert.AreEqual(true, EmParameters.DerivedHourlyEquationStatus, "DerivedHourlyEquationStatus");
				Assert.AreEqual(true, EmParameters.FlowMonitorHourlyChecksNeeded, "FlowMonitorHourlyChecksNeeded");
				Assert.AreEqual(true, EmParameters.MoistureNeeded, "MoistureNeeded");
				Assert.AreEqual("MIN", EmParameters.H2oMissingDataApproach, "MissingDataApproach");
			}

			//Result A
			{
				// Init Input
				EmParameters.DerivedHourlyFormulaStatus = true;
				EmParameters.MatsEquationCodeWithoutH2o = "NOH2O";
				EmParameters.MatsEquationCodeWithH2o = "H2O";
				EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData
					(equationCd: "BAD");

				// Init Output
				category.CheckCatalogResult = null;
				EmParameters.FlowMonitorHourlyChecksNeeded = false;
				EmParameters.MoistureNeeded = false;
				EmParameters.H2oMissingDataApproach = "";

				// Run Checks
				actual = cMATSDerivedHourlyValueChecks.MATSDHV11(category, ref log);

				// Check Results
				Assert.AreEqual(false, log);
				Assert.AreEqual("A", category.CheckCatalogResult, "Result");
				Assert.AreEqual(false, EmParameters.DerivedHourlyEquationStatus, "DerivedHourlyEquationStatus");
				Assert.AreEqual(false, EmParameters.FlowMonitorHourlyChecksNeeded, "FlowMonitorHourlyChecksNeeded");
				Assert.AreEqual(false, EmParameters.MoistureNeeded, "MoistureNeeded");
				Assert.AreEqual("", EmParameters.H2oMissingDataApproach, "MissingDataApproach");
			}
			
			//Null Equation Code
			{
				// Init Input
				EmParameters.DerivedHourlyFormulaStatus = true;
				EmParameters.MatsEquationCodeWithoutH2o = "NOH2O";
				EmParameters.MatsEquationCodeWithH2o = "H2O";
				EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData
					(equationCd: null);

				// Init Output
				category.CheckCatalogResult = null;
				EmParameters.FlowMonitorHourlyChecksNeeded = false;
				EmParameters.MoistureNeeded = false;
				EmParameters.H2oMissingDataApproach = "";

				// Run Checks
				actual = cMATSDerivedHourlyValueChecks.MATSDHV11(category, ref log);

				// Check Results
				Assert.AreEqual(false, log);
				Assert.AreEqual(null, category.CheckCatalogResult, "Result");
				Assert.AreEqual(true, EmParameters.DerivedHourlyEquationStatus, "DerivedHourlyEquationStatus");
				Assert.AreEqual(false, EmParameters.FlowMonitorHourlyChecksNeeded, "FlowMonitorHourlyChecksNeeded");
				Assert.AreEqual(false, EmParameters.MoistureNeeded, "MoistureNeeded");
				Assert.AreEqual("", EmParameters.H2oMissingDataApproach, "MissingDataApproach");
			}
		}
		#endregion

		#region MATSDHV-12

		/// <summary>
		///A test for MATSDHV-12
		///</summary>()
		[TestMethod()]
		public void MATSDHV12()
		{
			//static check setup
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			EmParameters.Init(category.Process);
			EmParameters.Category = category;

			// Variables
			bool log = false;
			string actual;
			string[] testEquationList = { "19-1", "19-2", "19-3", "19-3D", "19-4", "19-5", "19-5D", "19-6", "19-7", "19-8", "19-9", "BAD", null };

			foreach (string testEquationCode in testEquationList)
			{
				// Init Input
				EmParameters.DerivedHourlyFormulaStatus = true;
				EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData
					(equationCd: testEquationCode);

				// Init Output
				category.CheckCatalogResult = null;
				EmParameters.O2DryNeededForMats = false;
				EmParameters.O2WetNeededForMats = false;
				EmParameters.Co2DiluentNeededForMats = false;
				EmParameters.FdFactorNeeded = false;
				EmParameters.FwFactorNeeded = false;
				EmParameters.FcFactorNeeded = false;
				EmParameters.MoistureNeeded = false;

				// Run Checks
				actual = cMATSDerivedHourlyValueChecks.MATSDHV12(category, ref log);

				// Check Results
				Assert.AreEqual(false, log);
				if (testEquationCode.InList("19-1,19-4"))
				{
					Assert.AreEqual(null, category.CheckCatalogResult, "Result");
					Assert.AreEqual(true, EmParameters.DerivedHourlyEquationStatus, "DerivedHourlyEquationStatus");
					Assert.AreEqual(true, EmParameters.O2DryNeededForMats, "O2DryNeededForMats");
					Assert.AreEqual(false, EmParameters.O2WetNeededForMats, "O2WetNeededForMats");
					Assert.AreEqual(false, EmParameters.Co2DiluentNeededForMats, "Co2DiluentNeededForMats");
					Assert.AreEqual(true, EmParameters.FdFactorNeeded, "FdFactorNeeded");
					Assert.AreEqual(false, EmParameters.FwFactorNeeded, "FwFactorNeeded");
					Assert.AreEqual(false, EmParameters.FcFactorNeeded, "FcFactorNeeded");
				}
				else if (testEquationCode.InList("19-3,19-3D,19-5,19-5D"))
				{
					Assert.AreEqual(null, category.CheckCatalogResult, "Result");
					Assert.AreEqual(true, EmParameters.DerivedHourlyEquationStatus, "DerivedHourlyEquationStatus");
					Assert.AreEqual(false, EmParameters.O2DryNeededForMats, "O2DryNeededForMats");
					Assert.AreEqual(true, EmParameters.O2WetNeededForMats, "O2WetNeededForMats");
					Assert.AreEqual(false, EmParameters.Co2DiluentNeededForMats, "Co2DiluentNeededForMats");
					Assert.AreEqual(true, EmParameters.FdFactorNeeded, "FdFactorNeeded");
					Assert.AreEqual(false, EmParameters.FwFactorNeeded, "FwFactorNeeded");
					Assert.AreEqual(false, EmParameters.FcFactorNeeded, "FcFactorNeeded");
				}
				else if (testEquationCode.InList("19-2"))
				{
					Assert.AreEqual(null, category.CheckCatalogResult, "Result");
					Assert.AreEqual(true, EmParameters.DerivedHourlyEquationStatus, "DerivedHourlyEquationStatus");
					Assert.AreEqual(false, EmParameters.O2DryNeededForMats, "O2DryNeededForMats");
					Assert.AreEqual(true, EmParameters.O2WetNeededForMats, "O2WetNeededForMats");
					Assert.AreEqual(false, EmParameters.Co2DiluentNeededForMats, "Co2DiluentNeededForMats");
					Assert.AreEqual(false, EmParameters.FdFactorNeeded, "FdFactorNeeded");
					Assert.AreEqual(true, EmParameters.FwFactorNeeded, "FwFactorNeeded");
					Assert.AreEqual(false, EmParameters.FcFactorNeeded, "FcFactorNeeded");
				}
				else if (testEquationCode.InList("19-6,19-7,19-8,19-9"))
				{
					Assert.AreEqual(null, category.CheckCatalogResult, "Result");
					Assert.AreEqual(true, EmParameters.DerivedHourlyEquationStatus, "DerivedHourlyEquationStatus");
					Assert.AreEqual(false, EmParameters.O2DryNeededForMats, "O2DryNeededForMats");
					Assert.AreEqual(false, EmParameters.O2WetNeededForMats, "O2WetNeededForMats");
					Assert.AreEqual(true, EmParameters.Co2DiluentNeededForMats, "Co2DiluentNeededForMats");
					Assert.AreEqual(false, EmParameters.FdFactorNeeded, "FdFactorNeeded");
					Assert.AreEqual(false, EmParameters.FwFactorNeeded, "FwFactorNeeded");
					Assert.AreEqual(true, EmParameters.FcFactorNeeded, "FcFactorNeeded");
				}
				else if (testEquationCode != null)
				{
					Assert.AreEqual("A", category.CheckCatalogResult, "Result");
					Assert.AreEqual(false, EmParameters.DerivedHourlyEquationStatus, "DerivedHourlyEquationStatus");
					Assert.AreEqual(false, EmParameters.O2DryNeededForMats, "O2DryNeededForMats");
					Assert.AreEqual(false, EmParameters.O2WetNeededForMats, "O2WetNeededForMats");
					Assert.AreEqual(false, EmParameters.Co2DiluentNeededForMats, "Co2DiluentNeededForMats");
					Assert.AreEqual(false, EmParameters.FdFactorNeeded, "FdFactorNeeded");
					Assert.AreEqual(false, EmParameters.FwFactorNeeded, "FwFactorNeeded");
					Assert.AreEqual(false, EmParameters.FcFactorNeeded, "FcFactorNeeded");
				}
				else if (testEquationCode == null)
				{
					Assert.AreEqual(null, category.CheckCatalogResult, "Result");
					Assert.AreEqual(true, EmParameters.DerivedHourlyEquationStatus, "DerivedHourlyEquationStatus");
					Assert.AreEqual(false, EmParameters.O2DryNeededForMats, "O2DryNeededForMats");
					Assert.AreEqual(false, EmParameters.O2WetNeededForMats, "O2WetNeededForMats");
					Assert.AreEqual(false, EmParameters.Co2DiluentNeededForMats, "Co2DiluentNeededForMats");
					Assert.AreEqual(false, EmParameters.FdFactorNeeded, "FdFactorNeeded");
					Assert.AreEqual(false, EmParameters.FwFactorNeeded, "FwFactorNeeded");
					Assert.AreEqual(false, EmParameters.FcFactorNeeded, "FcFactorNeeded");
				}

				if (testEquationCode.InList("19-3,19-3D,19-4,19-5,19-8,19-9"))
				{
					Assert.AreEqual(true, EmParameters.MoistureNeeded, "MoistureNeeded");
				}
				else
				{
					Assert.AreEqual(false, EmParameters.MoistureNeeded, "MoistureNeeded");
				}
			}		
		}
		#endregion

		#region MATSDHV-13

		/// <summary>
		///A test for MATSDHV-13
		///</summary>()
		[TestMethod()]
		public void MATSDHV13()
		{
			//static check setup
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			EmParameters.Init(category.Process);
			EmParameters.Category = category;

			// Variables
			bool log = false;
			string actual;

			// Init Input
			EmParameters.CurrentDhvParameter = "GOOD";
			EmParameters.DerivedHourlyEquationStatus = true;

			// Init Output
			category.CheckCatalogResult = null;
			EmParameters.MatsHgDhvParameter = null;
			EmParameters.MatsHgDhvValid = false;

			// Run Checks
			actual = cMATSDerivedHourlyValueChecks.MATSDHV13(category, ref log);

			// Check Results
			Assert.AreEqual(false, log);
			Assert.AreEqual(null, category.CheckCatalogResult, "Result");
			Assert.AreEqual("GOOD", EmParameters.MatsHgDhvParameter, "MatsHgDhvParameter");
			Assert.AreEqual(true, EmParameters.MatsHgDhvValid, "MatsHgDhvValid");
		}
		#endregion

		#region MATSDHV-14

		/// <summary>
		///A test for MATSDHV-14
		///</summary>()
		[TestMethod()]
		public void MATSDHV14()
		{
			//static check setup
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			EmParameters.Init(category.Process);
			EmParameters.Category = category;

			// Variables
			bool log = false;
			string actual;

			// Init Input
			EmParameters.CurrentDhvParameter = "GOOD";
			EmParameters.DerivedHourlyEquationStatus = true;

			// Init Output
			category.CheckCatalogResult = null;
			EmParameters.MatsHclDhvParameter = null;
			EmParameters.MatsHclDhvValid = false;

			// Run Checks
			actual = cMATSDerivedHourlyValueChecks.MATSDHV14(category, ref log);

			// Check Results
			Assert.AreEqual(false, log);
			Assert.AreEqual(null, category.CheckCatalogResult, "Result");
			Assert.AreEqual("GOOD", EmParameters.MatsHclDhvParameter, "MatsHclDhvParameter");
			Assert.AreEqual(true, EmParameters.MatsHclDhvValid, "MatsHclDhvValid");
		}
		#endregion

		#region MATSDHV-15

		/// <summary>
		///A test for MATSDHV-15
		///</summary>()
		[TestMethod()]
		public void MATSDHV15()
		{
			//static check setup
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			EmParameters.Init(category.Process);
			EmParameters.Category = category;

			// Variables
			bool log = false;
			string actual;

			// Init Input
			EmParameters.CurrentDhvParameter = "GOOD";
			EmParameters.DerivedHourlyEquationStatus = true;

			// Init Output
			category.CheckCatalogResult = null;
			EmParameters.MatsHfDhvParameter = null;
			EmParameters.MatsHfDhvValid = false;

			// Run Checks
			actual = cMATSDerivedHourlyValueChecks.MATSDHV15(category, ref log);

			// Check Results
			Assert.AreEqual(false, log);
			Assert.AreEqual(null, category.CheckCatalogResult, "Result");
			Assert.AreEqual("GOOD", EmParameters.MatsHfDhvParameter, "MatsHfDhvParameter");
			Assert.AreEqual(true, EmParameters.MatsHfDhvValid, "MatsHfDhvValid");
		}
		#endregion

		#region MATSDHV-16

		/// <summary>
		///A test for MATSDHV-16
		///</summary>()
		[TestMethod()]
		public void MATSDHV16()
		{
			//static check setup
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			EmParameters.Init(category.Process);
			EmParameters.Category = category;

			// Variables
			bool log = false;
			string actual;

			// Init Input
			EmParameters.CurrentDhvParameter = "GOOD";
			EmParameters.DerivedHourlyEquationStatus = true;

			// Init Output
			category.CheckCatalogResult = null;
			EmParameters.MatsSo2DhvParameter = null;
			EmParameters.MatsSo2DhvValid = false;

			// Run Checks
			actual = cMATSDerivedHourlyValueChecks.MATSDHV16(category, ref log);

			// Check Results
			Assert.AreEqual(false, log);
			Assert.AreEqual(null, category.CheckCatalogResult, "Result");
			Assert.AreEqual("GOOD", EmParameters.MatsSo2DhvParameter, "MatsSo2DhvParameter");
			Assert.AreEqual(true, EmParameters.MatsSo2DhvValid, "MatsSo2DhvValid");
		}
        #endregion


        /// <summary>
        /// 
        /// Required Parameters
        /// 
        ///     CO2DiluentNeededForMats            : {Co2}
        ///     DerivedHourlyEquaitonStatus        : {EStatus}
        ///     DerivedHourlyModcStatus            : {MStatus}
        ///     O2DryNeededForMats                 : {O2 Dry}
        ///     O2WetNeededForMats                 : {O2 Wet}
        /// 
        /// Optional Parameters:
        /// 
        ///     CO2DiluentNeededForMatsCalculation : {Calc}
        ///     O2DryNeededForMatsCalculation      : {Calc}
        ///     O2WetNeededForMatsCalculation      : {Calc}
        /// 
        /// 
        /// | ## | EStatus | MStatus | Modc | Co2   | O2 Dry | O2 Wet | Calc  || Co2   | O2 Dry | O2 Wet || Note
        /// |  0 | null    | null    | 37   | true  | true   | true   | false || false | false  | false  || Derived Hourly Equation and MODC Status is null.
        /// |  1 | false   | false   | 37   | true  | true   | true   | false || false | false  | false  || Derived Hourly Equation and MODC Status is false.
        /// |  2 | false   | false   | 37   | true  | true   | true   | true  || true  | true   | true   || Derived Hourly Equation and MODC Status is false, initially true.
        /// |  3 | false   | false   | 39   | true  | true   | true   | false || false | false  | false  || Derived Hourly Equation and MODC Status is false.
        /// |  4 | false   | true    | 39   | true  | true   | true   | false || false | false  | false  || Derived Hourly Equation Status is false and Derived Hourly MODC Status is true.
        /// |  5 | true    | false   | 39   | true  | true   | true   | false || false | false  | false  || Derived Hourly Equation Status is true and Derived Hourly MODC Status is false.
        /// |  6 | true    | true    | 36   | true  | true   | true   | false || true  | true   | true   || MODC 36.
        /// |  7 | true    | true    | 37   | true  | true   | true   | false || true  | true   | true   || MODC 37.
        /// |  8 | true    | true    | 38   | true  | true   | true   | false || false | false  | false  || MODC 38.
        /// |  9 | true    | true    | 39   | true  | true   | true   | false || true  | true   | true   || MODC 39.
        /// | 10 | true    | true    | 36   | true  | false  | false  | false || true  | false  | false  || CO2 Diluent only.
        /// | 11 | true    | true    | 36   | false | true   | false  | false || false | true   | false  || O2 Dry only.
        /// | 12 | true    | true    | 36   | false | false  | true   | false || false | false  | true   || O2 Wet only.
        /// | 13 | true    | true    | 36   | true  | false  | false  | true  || true  | true   | true   || CO2 Diluent only, initially true.
        /// | 14 | true    | true    | 36   | false | true   | false  | true  || true  | true   | true   || O2 Dry only, initially true.
        /// | 15 | true    | true    | 36   | false | false  | true   | true  || true  | true   | true   || O2 Wet only, initially true.
        /// </summary>
        [TestMethod()]
        public void MatsDhv18()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            /* Input Parameter Values */
            bool?[] eStatusList = { null, false, false, false, false, true, true, true, true, true, true, true, true, true, true, true };
            bool?[] mStatusList = { null, false, false, false, true, false, true, true, true, true, true, true, true, true, true, true };
            string[] modcList = { "37", "37", "37", "39", "39", "39", "36", "37", "38", "39", "36", "36", "36", "36", "36", "36" };
            bool?[] co2NeededList = { true, true, true, true, true, true, true, true, true, true, true, false, false, true, false, false };
            bool?[] o2dNeededList = { true, true, true, true, true, true, true, true, true, true, false, true, false, false, true, false };
            bool?[] o2wNeededList = { true, true, true, true, true, true, true, true, true, true, false, false, true, false, false, true };
            bool?[] calcNeededList = { false, false, true, false, false, false, false, false, false, false, false, false, false, true, true, true };

            /* Expected Values */
            bool?[] expCo2NeededList = { false, false, true, false, false, false, true, true, false, true, true, false, false, true, true, true };
            bool?[] expO2dNeededList = { false, false, true, false, false, false, true, true, false, true, false, true, false, true, true, true };
            bool?[] expO2wNeededList = { false, false, true, false, false, false, true, true, false, true, false, false, true, true, true, true };

            /* Test Case Count */
            int caseCount = 16;

            /* Check array lengths */
            Assert.AreEqual(caseCount, eStatusList.Length, "eStatusList length");
            Assert.AreEqual(caseCount, mStatusList.Length, "mStatusList length");
            Assert.AreEqual(caseCount, modcList.Length, "modcList length");
            Assert.AreEqual(caseCount, co2NeededList.Length, "co2NeededList length");
            Assert.AreEqual(caseCount, o2dNeededList.Length, "o2dNeededList length");
            Assert.AreEqual(caseCount, o2wNeededList.Length, "o2wNeededList length");
            Assert.AreEqual(caseCount, calcNeededList.Length, "calcNeededList length");
            Assert.AreEqual(caseCount, expCo2NeededList.Length, "expCo2NeededList length");
            Assert.AreEqual(caseCount, expO2dNeededList.Length, "expO2dNeededList length");
            Assert.AreEqual(caseCount, expO2wNeededList.Length, "expO2wNeededList length");

            /* Run Cases */
            for (int caseDex = 0; caseDex < caseCount; caseDex++)
            {
                /* Initialize Input Parameters */
                EmParameters.Co2DiluentNeededForMats = co2NeededList[caseDex];
                EmParameters.DerivedHourlyEquationStatus = eStatusList[caseDex];
                EmParameters.DerivedHourlyModcStatus = mStatusList[caseDex];
                EmParameters.MatsDhvRecord = new MATSDerivedHourlyValueData(modcCd: modcList[caseDex]);
                EmParameters.O2DryNeededForMats = o2dNeededList[caseDex];
                EmParameters.O2WetNeededForMats = o2wNeededList[caseDex];

                /* Initialize Input/Output Parameters */
                EmParameters.Co2DiluentNeededForMatsCalculation = calcNeededList[caseDex];
                EmParameters.O2DryNeededForMatsCalculation = calcNeededList[caseDex];
                EmParameters.O2WetNeededForMatsCalculation = calcNeededList[caseDex];

                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Initialize variables needed to run the check. */
                bool log = false;
                string actual;

                /* Run Check */
                actual = cMATSDerivedHourlyValueChecks.MATSDHV18(category, ref log);

                /* Check Results */
                Assert.AreEqual(string.Empty, actual, string.Format("actual[case {0}]", caseDex));
                Assert.AreEqual(false, log, string.Format("log [case {0}]", caseDex));
                Assert.AreEqual(null, category.CheckCatalogResult, string.Format("category.CheckCatalogResult [case {0}]", caseDex));

                Assert.AreEqual(expCo2NeededList[caseDex], EmParameters.Co2DiluentNeededForMatsCalculation, string.Format("Co2DiluentNeededForMatsCalculation [case {0}]", caseDex));
                Assert.AreEqual(expO2dNeededList[caseDex], EmParameters.O2DryNeededForMatsCalculation, string.Format("O2DryNeededForMatsCalculation [case {0}]", caseDex));
                Assert.AreEqual(expO2wNeededList[caseDex], EmParameters.O2WetNeededForMatsCalculation, string.Format("O2WetNeededForMatsCalculation [case {0}]", caseDex));
            }
        }

        #endregion

    }
}