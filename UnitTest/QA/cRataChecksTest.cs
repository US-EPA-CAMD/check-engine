using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Data.Ecmps.Dbo.Table;
using ECMPS.Checks.Data.Ecmps.Dbo.View;
using ECMPS.Checks.Data.Ecmps.Lookup.Table;
using ECMPS.Checks.Data.EcmpsAux.CrossCheck.Virtual;
using ECMPS.Checks.Qa.Parameters;
using ECMPS.Checks.RATAChecks;
using ECMPS.Definitions.Extensions;

using UnitTest.UtilityClasses;



namespace UnitTest.QA
{
	[TestClass]
	public class cRataChecksTest
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

		#region RATA-2

		/// <summary>
		///A test for RATA2
		///</summary>()
		[TestMethod()]
		public void RATA2()
		{
			//static check setup
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			QaParameters.Init(category.Process);
			QaParameters.Category = category;

			// Variables
			bool log = false;
			string actual;

			// Result A
			{
				// Init Input
				QaParameters.CurrentRata = new VwQaTestSummaryRataRow(monSysId: null);

				// Init Output
				category.CheckCatalogResult = null;
				QaParameters.RataSystemValid = false;

				// Run Checks
				actual = cRATAChecks.RATA2(category, ref log);

				// Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual("A", category.CheckCatalogResult, "Result");
				Assert.AreEqual(false, QaParameters.RataSystemValid, "RataSystemValid");
			}
			// Result B
			{
				bool testTrue = false;
				string[] testSystemTypeList = { "SYSTEMBAD", "SO2", "NOX", "CO2", "FLOW", "SO2R", "O2", "H2O", "H2OM", "NOXC", "NOXP", "HG", "HCL", "HF", "ST" };

				foreach (string testSystemTypeCode in testSystemTypeList)
				{
					// Init Input
					QaParameters.CurrentRata = new VwQaTestSummaryRataRow(monSysId: "NOTNULL", sysTypeCd: testSystemTypeCode);

					if (testSystemTypeCode.InList("SO2,NOX,CO2,FLOW,SO2R,O2,H2O,H2OM,NOXC,NOXP,HG,HCL,HF,ST"))
					{
						testTrue = true;
					}
					// Init Output
					category.CheckCatalogResult = null;

					// Run Checks
					actual = cRATAChecks.RATA2(category, ref log);

					// Check Results
					Assert.AreEqual(string.Empty, actual);
					Assert.AreEqual(false, log);
					if (testTrue)
					{
						Assert.AreEqual(null, category.CheckCatalogResult, "Result");
						Assert.AreEqual(true, QaParameters.RataSystemValid, "RataSystemValid");
					}
					else
					{
						Assert.AreEqual("B", category.CheckCatalogResult, "Result");
						Assert.AreEqual(false, QaParameters.RataSystemValid, "RataSystemValid");
					}
				}
			}
		}
		#endregion

		#region RATA-6

		/// <summary>
		///A test for RATA6_HG
		///</summary>()
		[TestMethod()]
		public void RATA6_HG()
		{
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			QaParameters.Init(category.Process);
			QaParameters.Category = category;

			// Variables
			bool log = false;
			string actual;

			// Init Input
			QaParameters.TestDatesConsistent = true;
			QaParameters.HighestRataCemValue = 2;
			QaParameters.DefaultRecords = new CheckDataView<VwMonitorDefaultRow>();

			//Result D
			{
				// Init Input
				QaParameters.CurrentRata = new VwQaTestSummaryRataRow(sysTypeCd: "HG",
					beginDate: DateTime.Today.AddDays(-10), beginHour: 0,
					systemBeginDate: DateTime.Today.AddDays(-10), systemBeginHour: 0,
					endDate: DateTime.Today, endHour: 23);
				QaParameters.SpanRecords = new CheckDataView<MonitorSpanRow>
					(new MonitorSpanRow(componentTypeCd: "HG", spanScaleCd: "H", mpcValue: 1, beginDate: DateTime.Today.AddDays(-10), beginHour: 0));

				// Init Output
				category.CheckCatalogResult = null;

				// Run Checks
				actual = cRATAChecks.RATA6(category, ref log);

				// Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual("D", category.CheckCatalogResult, "Result");
			}

			//Result E
			{
				// Init Input
				QaParameters.CurrentRata = new VwQaTestSummaryRataRow(sysTypeCd: "HG",
					beginDate: DateTime.Today.AddDays(-10), beginHour: 0,
					systemBeginDate: DateTime.Today.AddDays(-10), systemBeginHour: 0,
					endDate: DateTime.Today, endHour: 23);
				QaParameters.SpanRecords = new CheckDataView<MonitorSpanRow>();

				// Init Output
				category.CheckCatalogResult = null;

				// Run Checks
				actual = cRATAChecks.RATA6(category, ref log);

				// Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual("E", category.CheckCatalogResult, "Result");
			}

			//Result F
			{
				// Init Input
				QaParameters.CurrentRata = new VwQaTestSummaryRataRow(sysTypeCd: "HG",
					beginDate: DateTime.Today.AddDays(-10), beginHour: 0,
					systemBeginDate: DateTime.Today.AddDays(-10), systemBeginHour: 0,
					endDate: DateTime.Today, endHour: 23);
				QaParameters.SpanRecords = new CheckDataView<MonitorSpanRow>
					(new MonitorSpanRow(componentTypeCd: "HG", spanScaleCd: "H", mpcValue: 1, beginDate: DateTime.Today.AddDays(-10), beginHour: 0),
					new MonitorSpanRow(componentTypeCd: "HG", spanScaleCd: "H", mpcValue: 1, beginDate: DateTime.Today.AddDays(-10), beginHour: 0));

				// Init Output
				category.CheckCatalogResult = null;

				// Run Checks
				actual = cRATAChecks.RATA6(category, ref log);

				// Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual("F", category.CheckCatalogResult, "Result");
			}
		}

		/// <summary>
		///A test for RATA6_HCL_HF
		///</summary>()
		[TestMethod()]
		public void RATA6_HCL_HF()
		{
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			QaParameters.Init(category.Process);
			QaParameters.Category = category;

			// Variables
			bool log = false;
			string actual;

			// Init Input
			QaParameters.TestDatesConsistent = true;
			QaParameters.HighestRataCemValue = 2;
			QaParameters.DefaultRecords = new CheckDataView<VwMonitorDefaultRow>();
			string[] testSystemTypeList = { "HCL", "HF" };

			// Init Input
			QaParameters.HighestRataCemValue = 2;

			foreach (string testSystemTypeCode in testSystemTypeList)
			{
				// Init Input
				QaParameters.CurrentRata = new VwQaTestSummaryRataRow(sysTypeCd: testSystemTypeCode,
					beginDate: DateTime.Today.AddDays(-10), beginHour: 0,
					systemBeginDate: DateTime.Today.AddDays(-10), systemBeginHour: 0,
					endDate: DateTime.Today, endHour: 23);
				QaParameters.SpanRecords = new CheckDataView<MonitorSpanRow>();

				// Init Output
				category.CheckCatalogResult = null;

				// Run Checks
				actual = cRATAChecks.RATA6(category, ref log);

				// Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual(null, category.CheckCatalogResult, "Result");
			}
		}

		#endregion

		#region RATA-16

		/// <summary>
		///A test for RATA16InList
		///</summary>()
		[TestMethod()]
		public void RATA16InList()
		{
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			QaParameters.Init(category.Process);
			QaParameters.Category = category;

			// Variables
			bool log = false;
			string actual;
			string[] refMethodCodeList = { "20", "320", "20,3A" };

			// Init Input
			QaParameters.RataReferenceMethodValid = true;
			QaParameters.CurrentRata = new VwQaTestSummaryRataRow(sysTypeCd: "NOTFLOW");
			QaParameters.ReferenceMethodCodeLookupTable = new CheckDataView<RefMethodCodeRow>(
				new RefMethodCodeRow(refMethodCd: "20"),
				new RefMethodCodeRow(refMethodCd: "320"),
				new RefMethodCodeRow(refMethodCd: "20,3A")
				);
			QaParameters.EcmpsMpBeginDate = DateTime.Today.AddDays(-10);
			foreach (string refMethodCode in refMethodCodeList)
			{
				// Init Input
				QaParameters.CurrentRataSummary = new VwQaRataSummaryRow(refMethodCd: refMethodCode, endDate: DateTime.Today, endHour: 0);

				// Init Output
				category.CheckCatalogResult = null;

				// Run Checks
				actual = cRATAChecks.RATA16(category, ref log);

				// Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				if (("20").InList(refMethodCode))
				{
					Assert.AreEqual("E", category.CheckCatalogResult, "Result");
				}
				else
				{
					Assert.AreEqual("C", category.CheckCatalogResult, "Result");
				}
			}
		}
        #endregion


        /// <summary>
        /// 
        /// RATA Level Valid: This parameter should have been initialized to true, but was not.  It appears that it is
        ///                   currently not used so the lack of initialization does not currently matter.
        ///                   
        /// 
        /// | ## | RunStat | SysType | Used | Unused || Result | Valid | Used | Unused || Note
        /// |  0 | null    | FLOW    |  100 |     10 || A      | false |  100 |     10 || Null Run Status.
        /// |  1 | ""      | FLOW    |  100 |     10 || A      | false |  100 |     10 || Null Run Status.
        /// |  2 | " "     | FLOW    |  100 |     10 || A      | false |  100 |     10 || Null Run Status.
        /// |  3 | BAD     | FLOW    |  100 |     10 || B      | false |  100 |     10 || Non existent Run Status.
        /// |  4 | RUNUSED | FLOW    |  100 |     10 || null   | null  |  101 |     10 || RUNUSED is a valid Run Status.
        /// |  5 | RUNUSED | CO2     |  100 |     10 || null   | null  |  101 |     10 || RUNUSED is a valid Run Status.
        /// |  6 | RUNUSED | GAS     |  100 |     10 || null   | null  |  101 |     10 || RUNUSED is a valid Run Status.
        /// |  7 | RUNUSED | NOX     |  100 |     10 || null   | null  |  101 |     10 || RUNUSED is a valid Run Status.
        /// |  8 | RUNUSED | SO2     |  100 |     10 || null   | null  |  101 |     10 || RUNUSED is a valid Run Status.
        /// |  9 | RUNUSED | HG      |  100 |     10 || null   | null  |  101 |     10 || RUNUSED is a valid Run Status.
        /// | 10 | RUNUSED | ST      |  100 |     10 || null   | null  |  101 |     10 || RUNUSED is a valid Run Status.
        /// | 11 | RUNUSED | HCL     |  100 |     10 || null   | null  |  101 |     10 || RUNUSED is a valid Run Status.
        /// | 12 | NOTUSED | FLOW    |  100 |     10 || null   | null  |  100 |     11 || NOTUSED is a valid Run Status.
        /// | 13 | NOTUSED | CO2     |  100 |     10 || null   | null  |  100 |     11 || NOTUSED is a valid Run Status.
        /// | 14 | NOTUSED | GAS     |  100 |     10 || null   | null  |  100 |     11 || NOTUSED is a valid Run Status.
        /// | 15 | NOTUSED | NOX     |  100 |     10 || null   | null  |  100 |     11 || NOTUSED is a valid Run Status.
        /// | 16 | NOTUSED | SO2     |  100 |     10 || null   | null  |  100 |     11 || NOTUSED is a valid Run Status.
        /// | 17 | NOTUSED | HG      |  100 |     10 || null   | null  |  100 |     11 || NOTUSED is a valid Run Status.
        /// | 18 | NOTUSED | ST      |  100 |     10 || null   | null  |  100 |     11 || NOTUSED is a valid Run Status.
        /// | 19 | NOTUSED | HCL     |  100 |     10 || null   | null  |  100 |     11 || NOTUSED is a valid Run Status.
        /// | 20 | IGNORED | FLOW    |  100 |     10 || C      | false |  100 |     10 || IGNORED is a not valid Run Status for FLOW systems.
        /// | 21 | IGNORED | CO2     |  100 |     10 || C      | false |  100 |     10 || IGNORED is a not valid Run Status for CO2 systems.
        /// | 22 | IGNORED | GAS     |  100 |     10 || C      | false |  100 |     10 || IGNORED is a not valid Run Status for GAS systems.
        /// | 23 | IGNORED | NOX     |  100 |     10 || C      | false |  100 |     10 || IGNORED is a not valid Run Status for NOX systems.
        /// | 24 | IGNORED | SO2     |  100 |     10 || C      | false |  100 |     10 || IGNORED is a not valid Run Status for SO2 systems.
        /// | 25 | IGNORED | HG      |  100 |     10 || C      | false |  100 |     10 || IGNORED is a not valid Run Status for HG systems.
        /// | 26 | IGNORED | ST      |  100 |     10 || null   | null  |  100 |     10 || IGNORED is a valid Run Status for ST systems.
        /// | 27 | IGNORED | HCL     |  100 |     10 || C      | false |  100 |     10 || IGNORED is a not valid Run Status for HCL systems.
        /// </summary>
        [TestMethod]
        public void Rata29()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            QaParameters.Init(category.Process);
            QaParameters.Category = category;


            /* Input Parameter Values */
            string[] runStatusList = { null, "", " ", "BAD",
                                       "RUNUSED", "RUNUSED", "RUNUSED", "RUNUSED", "RUNUSED", "RUNUSED", "RUNUSED", "RUNUSED",
                                       "NOTUSED", "NOTUSED", "NOTUSED", "NOTUSED", "NOTUSED", "NOTUSED", "NOTUSED", "NOTUSED",
                                       "IGNORED", "IGNORED", "IGNORED", "IGNORED", "IGNORED", "IGNORED", "IGNORED", "IGNORED" };
            string[] sysTypeList = { "FLOW", "FLOW", "FLOW", "FLOW",
                                     "FLOW", "CO2", "GAS", "NOX", "SO2", "HG", "ST", "HCL",
                                     "FLOW", "CO2", "GAS", "NOX", "SO2", "HG", "ST", "HCL",
                                     "FLOW", "CO2", "GAS", "NOX", "SO2", "HG", "ST", "HCL" };
            int?[] usedCntList = { 100, 100, 100, 100,
                                   100, 100, 100, 100, 100, 100, 100, 100,
                                   100, 100, 100, 100, 100, 100, 100, 100,
                                   100, 100, 100, 100, 100, 100, 100, 100 };
            int?[] unusedCntList = { 10, 10, 10, 10,
                                     10, 10, 10, 10, 10, 10, 10, 10,
                                     10, 10, 10, 10, 10, 10, 10, 10,
                                     10, 10, 10, 10, 10, 10, 10, 10 };

            /* Expected Values */
            string[] expResultList = { "A", "A", "A", "B",
                                       null, null, null, null, null, null, null, null,
                                       null, null, null, null, null, null, null, null,
                                       "C", "C", "C", "C", "C", "C", null, "C" };
            bool?[] expValidList = { false, false, false, false,
                                     null, null, null, null, null, null, null, null,
                                     null, null, null, null, null, null, null, null,
                                     false, false, false, false, false, false, null, false};
            int?[] expUsedCntList = { 100, 100, 100, 100,
                                      101, 101, 101, 101, 101, 101, 101, 101,
                                      100, 100, 100, 100, 100, 100, 100, 100,
                                      100, 100, 100, 100, 100, 100, 100, 100 };
            int?[] expUnusedCntList = { 10, 10, 10, 10,
                                        10, 10, 10, 10, 10, 10, 10, 10,
                                        11, 11, 11, 11, 11, 11, 11, 11,
                                        10, 10, 10, 10, 10, 10, 10, 10 };

            /* Case Count */
            int caseCount = 28;

            /* Check array lengths */
            Assert.AreEqual(caseCount, runStatusList.Length, "runStatusList length");
            Assert.AreEqual(caseCount, sysTypeList.Length, "sysTypeList length");
            Assert.AreEqual(caseCount, usedCntList.Length, "usedCntList length");
            Assert.AreEqual(caseCount, unusedCntList.Length, "unusedCntList length");
            Assert.AreEqual(caseCount, expResultList.Length, "expResultList length");
            Assert.AreEqual(caseCount, expValidList.Length, "expValidList length");
            Assert.AreEqual(caseCount, expUsedCntList.Length, "expUsedCntList length");
            Assert.AreEqual(caseCount, expUnusedCntList.Length, "expUnusedCntList length");


            for (int caseDex = 0; caseDex < caseCount; caseDex++)
            {
                /* Initialize Input Parameters */
                QaParameters.CurrentRata = new VwQaTestSummaryRataRow(sysTypeCd: sysTypeList[caseDex]);
                QaParameters.CurrentRataRun = new VwQaRataRunRow(runStatusCd: runStatusList[caseDex]);

                /* Initialize Input-Output Parameters */
                QaParameters.RataRunCount = usedCntList[caseDex];
                QaParameters.RataUnusedRunCount = unusedCntList[caseDex];

                /* Initialize Output Parameters */
                QaParameters.RataLevelValid = null;


                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Initialize variables needed to run the check. */
                bool log = false;
                string actual;

                /* Run Check */
                actual = cRATAChecks.RATA29(category, ref log);


                /* Check results */
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(expResultList[caseDex], category.CheckCatalogResult, string.Format("Result {0}", caseDex));

                Assert.AreEqual(expValidList[caseDex], QaParameters.RataLevelValid, string.Format("RataLevelValid {0}", caseDex));
                Assert.AreEqual(expUsedCntList[caseDex], QaParameters.RataRunCount, string.Format("RataRunCount {0}", caseDex));
                Assert.AreEqual(expUnusedCntList[caseDex], QaParameters.RataUnusedRunCount, string.Format("RataUnusedRunCount {0}", caseDex));
            }
        }


        #region RATA-37

        /// <summary>
        ///A test for RATA37_Hg
        ///</summary>()
        [TestMethod()]
        public void RATA37_Hg()
        {
            RATA37_HG_OR_ST("HG");
        }

        /// <summary>
        ///A test for RATA37_ST
        ///</summary>()
        [TestMethod()]
        public void RATA37_ST()
        {
            RATA37_HG_OR_ST("ST");
        }

		/// <summary>
		/// 
		///             |     Rel. Acc.     |  Mean Ref   |    Mean Diff    |     Con Coef    |
		/// | ## | Aps  | Calc    | Rpt     | Calc  | Rpt | Calc   | Rpt    | Calc   | Rpt    | Result  | Freq  || Result | Aps  | Rata    | Freq  || Note
		/// |  0 | 0    | 20.0490 | 20.0500 | 2.550 | 0.0 | 0.275  | 0.275  | 0.275  | 0.275  | PASSED  | 4QTRS || null   | 0    | PASSED  | 4QTRS || Passed based on calculated value.
		/// |  1 | 0    | 20.0490 | 20.0500 | 2.550 | 0.0 | 0.275  | 0.275  | 0.275  | 0.275  | PASSAPS | 4QTRS || null   | 0    | PASSAPS | 4QTRS || Passed based on calculated value, previous result is PASSAPS.
		/// |  2 | 0    | 20.0490 | 20.0500 | 2.550 | 0.0 | 0.275  | 0.275  | 0.275  | 0.275  | FAILED  | 4QTRS || null   | 0    | FAILED  | null  || Passed based on calculated value, previous result is FAILED.
		/// |  3 | 0    | 20.0490 | 20.0500 | 2.550 | 0.0 | 0.275  | 0.275  | 0.275  | 0.275  | PASSED  | 2QTRS || null   | 0    | PASSED  | 2QTRS || Passed based on calculated value, previous frequency is 2QTRS.
		/// |  4 | 1    | 20.0500 | 20.0500 | 2.549 | 0.0 | 0.274  | 0.275  | 0.275  | 0.275  | PASSED  | 4QTRS || null   | 1    | PASSAPS | 4QTRS || Passed APS based on calculated values.
		/// |  5 | 1    | 20.0500 | 20.0500 | 2.549 | 0.0 | 0.274  | 0.275  | 0.275  | 0.275  | FAILED  | 4QTRS || null   | 1    | FAILED  | null  || Passed APS based on calculated values, previous result is FAILED.
		/// |  6 | null | 20.0500 | 20.0500 | 2.549 | 0.0 | 0.274  | 0.275  | 0.275  | 0.275  | PASSED  | 4QTRS || A      | 1    | PASSAPS | 4QTRS || Passed APS based on calculated values, but reported APS is null.
		/// |  7 | 0    | 20.0500 | 20.0500 | 2.549 | 0.0 | 0.274  | 0.275  | 0.275  | 0.275  | PASSED  | 4QTRS || B      | 1    | PASSAPS | 4QTRS || Passed APS based on calculated values, but reported APS is 0.
		/// |  8 | 0    | 20.0500 | 20.0500 | 2.550 | 0.0 | 0.275  | 0.275  | 0.275  | 0.275  | PASSED  | 4QTRS || null   | null | FAILED  | null  || Failed based on calculated and reported value.
		/// | *9 | 0    | 20.0500 | 20.0490 | 2.550 | 0.0 | 0.275  | 0.275  | 0.275  | 0.275  | PASSED  | 4QTRS || null   | 0    | PASSED  | 4QTRS || Passed based on reported value.
		/// |*10 | 0    | 20.0500 | 20.0350 | 2.550 | 0.0 | 0.275  | 0.275  | 0.275  | 0.275  | PASSED  | 4QTRS || null   | 0    | PASSED  | 4QTRS || Passed based on reported value, edge case.
		/// |*11 | 0    | 20.0500 | 20.0349 | 2.550 | 0.0 | 0.275  | 0.275  | 0.275  | 0.275  | PASSED  | 4QTRS || null   | null | FAILED  | null  || Passed based on reported value, but reported and calculated are not within tolerance.
		/// | 12 | 1    | 20.0500 | 20.0500 | 2.549 | 0.0 | 0.275  | 0.274  | 0.275  | 0.275  | PASSED  | 4QTRS || null   | 1    | PASSAPS | 4QTRS || Passed APS based on reported values.
		/// | 13 | 1    | 20.0500 | 20.0500 | 2.549 | 0.0 | 0.275  | 0.275  | 0.275  | 0.274  | PASSED  | 4QTRS || null   | 1    | PASSAPS | 4QTRS || Passed APS based on reported values.
		/// | 14 | 1    | 20.0500 | 20.0500 | 2.549 | 0.0 | 0.275  | 0.125  | 0.275  | 0.275  | PASSED  | 4QTRS || D      | null | FAILED  | null  || Passed APS based on reported values, but reported and calculated mean difference are not within tolerance.
		/// | 15 | 1    | 20.0500 | 20.0500 | 2.549 | 0.0 | 0.275  | 0.275  | 0.275  | 0.125  | PASSED  | 4QTRS || D      | null | FAILED  | null  || Passed APS based on reported values, but reported and calculated confidence coefficient are not within tolerance.
		/// | 16 | 1    | 20.0500 | 20.0500 | 2.549 | 0.0 | 0.275  | 0.126  | 0.275  | 0.276  | PASSED  | 4QTRS || null   | 1    | PASSAPS | 4QTRS || Passed APS based on reported values, and reported and calculated mean difference are barely within tolerance.
		/// | 17 | 1    | 20.0500 | 20.0500 | 2.549 | 0.0 | 0.275  | 0.275  | 0.275  | 0.126  | PASSED  | 4QTRS || null   | 1    | PASSAPS | 4QTRS || Passed APS based on reported values, and reported and calculated confidence coefficient are barely within tolerance.
		/// | 18 | 1    | 20.0500 | 20.0500 | 2.550 | 0.0 | -0.275 | -0.275 | 0.275  | 0.275  | PASSED  | 4QTRS || D      | null | FAILED  | null  || Failed based on calculated and reported value.  Tests Absolute Values.
		/// | 19 | 1    | 20.0500 | 20.0500 | 2.550 | 0.0 | 0.275  | 0.275  | -0.275 | -0.275 | PASSED  | 4QTRS || D      | null | FAILED  | null  || Failed based on calculated and reported value.  Tests Absolute Values.
		/// | 20 | 1    | 20.0500 | 20.0500 | 2.550 | 0.0 | -0.275 | -0.125 | 0.275  | 0.275  | PASSED  | 4QTRS || D      | null | FAILED  | null  || Passed APS based on reported values, but reported and calculated mean difference are not within tolerance..  Tests Absolute Values.
		/// | 21 | 1    | 20.0500 | 20.0500 | 2.550 | 0.0 | 0.275  | 0.275  | -0.275 | -0.125 | PASSED  | 4QTRS || D      | null | FAILED  | null  || Passed APS based on reported values, but reported and calculated confidence coefficient are not within tolerance.  Tests Absolute Values.
		/// 
		/// * Skipping case because it is not complete and not possible to implement.
		/// 
		/// </summary>
		/// <param name="systemTypeCd"></param>
		public void RATA37_HG_OR_ST(string systemTypeCd)
        {
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            QaParameters.Init(category.Process);
            QaParameters.Category = category;


            /* Input Parameter Values */
            int?[] rptApsList = { 0, 0, 0, 0, 1,
                                  1, null, 0, 0, 0,
                                  0, 0, 1, 1, 1,
                                  1, 1, 1, 1, 1,
                                  1, 1 };
            decimal?[] calcRelAccuracyList = { 20.0490m, 20.0490m, 20.0490m, 20.0490m, 20.0500m,
                                               20.0500m, 20.0500m, 20.0500m, 20.0500m, 20.0500m,
                                               20.0500m, 20.0500m, 20.0500m, 20.0500m, 20.0500m,
                                               20.0500m, 20.0500m, 20.0500m, 20.0500m, 20.0500m,
                                               20.0500m, 20.0500m };
            decimal?[] rptRelAccuracyList = { 20.0500m, 20.0500m, 20.0500m, 20.0500m, 20.0500m,
                                              20.0500m, 20.0500m, 20.0500m, 20.0500m, 20.0490m,
                                              20.0350m, 20.0349m, 20.0500m, 20.0500m, 20.0500m,
                                              20.0500m, 20.0500m, 20.0500m, 20.0500m, 20.0500m,
                                              20.0500m, 20.0500m };
            decimal?[] calcMeanRefList = { 2.550m, 2.550m, 2.550m, 2.550m, 2.549m,
                                           2.549m, 2.549m, 2.549m, 2.550m, 2.550m,
                                           2.550m, 2.550m, 2.549m, 2.549m, 2.549m,
                                           2.549m, 2.549m, 2.549m, 2.550m, 2.550m,
                                           2.550m, 2.550m };
            decimal?[] rptMeanRefList = { 0.0000m, 0.0000m, 0.0000m, 0.0000m, 0.0000m,
                                          0.0000m, 0.0000m, 0.0000m, 0.0000m, 0.0000m,
                                          0.0000m, 0.0000m, 0.0000m, 0.0000m, 0.0000m,
                                          0.0000m, 0.0000m, 0.0000m, 0.0000m, 0.0000m,
                                          0.0000m, 0.0000m };
            decimal?[] calcMeanDiffList = { 0.275m, 0.275m, 0.275m, 0.275m, 0.274m,
                                            0.274m, 0.274m, 0.274m, 0.275m, 0.275m,
                                            0.275m, 0.275m, 0.275m, 0.275m, 0.275m,
                                            0.275m, 0.275m, 0.275m, -0.275m, 0.275m,
                                            -0.275m, 0.275m };
            decimal?[] rptMeanDiffList = { 0.275m, 0.275m, 0.275m, 0.275m, 0.275m,
                                           0.275m, 0.275m, 0.275m, 0.275m, 0.275m,
                                           0.275m, 0.275m, 0.274m, 0.275m, 0.125m,
                                           0.275m, 0.126m, 0.275m, -0.275m, 0.275m,
                                           -0.125m, 0.275m };
            decimal?[] calcConfCoefList = { 0.275m, 0.275m, 0.275m, 0.275m, 0.275m,
                                            0.275m, 0.275m, 0.275m, 0.275m, 0.275m,
                                            0.275m, 0.275m, 0.275m, 0.275m, 0.275m,
                                            0.275m, 0.275m, 0.275m, 0.275m, -0.275m,
                                            0.275m, -0.275m };
            decimal?[]rptConfCoefList = { 0.275m, 0.275m, 0.275m, 0.275m, 0.275m,
                                          0.275m, 0.275m, 0.275m, 0.275m, 0.275m,
                                          0.275m, 0.275m, 0.275m, 0.274m, 0.275m,
                                          0.125m, 0.275m, 0.126m, 0.275m, -0.275m,
                                          0.275m, -0.125m };
            string[] rataResultList = { "PASSED", "PASSAPS", "FAILED", "PASSED", "PASSED",
                                        "FAILED", "PASSED", "PASSED", "PASSED", "PASSED",
                                        "PASSED", "PASSED", "PASSED", "PASSED", "PASSED",
                                        "PASSED", "PASSED", "PASSED", "PASSED", "PASSED",
                                        "PASSED", "PASSED" };
            string[] rataFreqList = { "4QTRS", "4QTRS", "4QTRS", "2QTRS", "4QTRS",
                                      "4QTRS", "4QTRS", "4QTRS", "4QTRS", "4QTRS",
                                      "4QTRS", "4QTRS", "4QTRS", "4QTRS", "4QTRS",
                                      "4QTRS", "4QTRS", "4QTRS", "4QTRS", "4QTRS",
                                      "4QTRS", "4QTRS" };

            /* Expected Values */
            string[] expResultList = { null, null, null, null, null,
                                       null, "A", "B", null, null,
                                       null, null, null, null, "D",
									   "D", null, null, "D", "D",
									   "D", "D" };
            int?[] expApsList = { 0, 0, 0, 0, 1,
                                  1, 1, 1, null, 0,
                                  0, null, 1, 1, null,
                                  null, 1, 1, null, null,
                                  null, null };
            string[] expRataResultList = { "PASSED", "PASSAPS", "FAILED", "PASSED", "PASSAPS",
                                           "FAILED", "PASSAPS", "PASSAPS", "FAILED", "PASSED",
                                           "PASSED", "FAILED", "PASSAPS", "PASSAPS", "FAILED",
                                           "FAILED", "PASSAPS", "PASSAPS", "FAILED", "FAILED",
                                           "FAILED", "FAILED"};
            string[] expRataFreqList = { "4QTRS", "4QTRS", null, "2QTRS", "4QTRS",
                                         null, "4QTRS", "4QTRS", null, "4QTRS",
                                         "4QTRS", null, "4QTRS", "4QTRS", null,
                                         null, "4QTRS", "4QTRS", null, null,
                                         null, null };

            /* Case Count */
            int caseCount = 22;

            /* Check array lengths */
            Assert.AreEqual(caseCount, rptApsList.Length, "rptApsList length");
            Assert.AreEqual(caseCount, calcRelAccuracyList.Length, "calcRelAccuracyList length");
            Assert.AreEqual(caseCount, rptRelAccuracyList.Length, "rptRelAccuracyList length");
            Assert.AreEqual(caseCount, calcMeanRefList.Length, "calcMeanRefList length");
            Assert.AreEqual(caseCount, rptMeanRefList.Length, "rptMeanRefList length");
            Assert.AreEqual(caseCount, calcMeanDiffList.Length, "calcMeanDiffList length");
            Assert.AreEqual(caseCount, rptMeanDiffList.Length, "rptMeanDiffList length");
            Assert.AreEqual(caseCount, calcConfCoefList.Length, "calcConfCoefList length");
            Assert.AreEqual(caseCount, rptConfCoefList.Length, "rptConfCoefList length");
            Assert.AreEqual(caseCount, rataResultList.Length, "rataResultList length");
            Assert.AreEqual(caseCount, rataFreqList.Length, "rataFreqList length");
            Assert.AreEqual(caseCount, expResultList.Length, "expResultList length");
            Assert.AreEqual(caseCount, expApsList.Length, "expApsList length");
            Assert.AreEqual(caseCount, expRataResultList.Length, "expRataResultList length");
            Assert.AreEqual(caseCount, expRataFreqList.Length, "expRataFreqList length");



            for (int caseDex = 0; caseDex < caseCount; caseDex++)
            {
                /* Initialize Input Parameters */
                QaParameters.CalculateRataLevel = true;
                QaParameters.CurrentRataSummary = new VwQaRataSummaryRow(sysTypeCd: systemTypeCd, 
                                                                         apsInd: rptApsList[caseDex],
                                                                         relativeAccuracy: rptRelAccuracyList[caseDex],
                                                                         meanRataRefValue: rptMeanRefList[caseDex],
                                                                         meanDiff: rptMeanDiffList[caseDex],
                                                                         confidenceCoef: rptConfCoefList[caseDex]);
                QaParameters.RataSummaryConfidenceCoefficient = calcConfCoefList[caseDex];
                QaParameters.RataSummaryMeanDifference = calcMeanDiffList[caseDex];
                QaParameters.RataSummaryMeanReferenceValue = calcMeanRefList[caseDex];
                QaParameters.RataSummaryRelativeAccuracy = calcRelAccuracyList[caseDex];
                QaParameters.TestTolerancesCrossCheckTable = new CheckDataView<TestTolerancesRow>                      
                (
                    new TestTolerancesRow(testTypeCode: "RATA", fieldDescription: "ConfidenceCoefficientUGSCM", tolerance: "0.1"),
                    new TestTolerancesRow(testTypeCode: "RATA", fieldDescription: "MeanDifferenceUGSCM", tolerance: "0.1"),                        
                    new TestTolerancesRow(testTypeCode: "RATA", fieldDescription: "RelativeAccuracy", tolerance: "0.01")
                );

                /* Initialize Input/Output Parameters */
                QaParameters.RataFrequency = rataFreqList[caseDex];
                QaParameters.RataResult = rataResultList[caseDex];

                /* Initialize Output Parameters */
                QaParameters.RataSummaryApsIndicator = null;


                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Initialize variables needed to run the check. */
                bool log = false;
                string actual;

                /* Run Check */
                if (caseDex == 9) continue; // Skipping case because it is not complete and not possible to implement.
                if (caseDex == 10) continue; // Skipping case because it is not complete and not possible to implement.
                if (caseDex == 11) continue; // Skipping case because it is not complete and not possible to implement.
                actual = cRATAChecks.RATA37(category, ref log);


                /* Check results */
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(expResultList[caseDex], category.CheckCatalogResult, string.Format("Result {0}", caseDex));

                Assert.AreEqual(expRataResultList[caseDex], QaParameters.RataResult, string.Format("RATAResult {0}", caseDex));
                Assert.AreEqual(expRataFreqList[caseDex], QaParameters.RataFrequency, string.Format("RataFrequency {0}", caseDex));
                Assert.AreEqual(expApsList[caseDex], QaParameters.RataSummaryApsIndicator, string.Format("RataSummaryApsIndicator {0}", caseDex));
            }
        }


        /// <summary>
        /// 
        /// Case Notes:
        /// 
        /// * Result B is not possible for HCL since currently the check will only set RataSummaryApsIndicator to 1 if the ApsInd in the record is 1.
        /// * Receiving a FAILED because of the tolerance check is impossible because:
        ///   * Tolerance checking is skipped if RataSummaryRelativeAccuracy rounded to one decimal place is less than or equal to 20.
        ///   * Tolerance checking is skipped if the RA in the summary record is greater than 20 (or less than 0).
        ///   * Tolerance is checked after rounding the RA in the summary record and RataSummaryRelativeAccuracy to two decimal places.
        ///   It is impossible to have values that get past both skips and will pass the tolerance check.  The lowest RataSummaryRelativeAccuracy
        ///   value to avoid the skip would be 20.04 and the highest summary record RA to avoid the skip would be 20.00, and those two values fail
        ///   the tolerance check.
        /// 
        /// Calc - CalculateRataLevel
        /// Summ = RataSummaryRelativeAccuracy
        /// 
        /// | ## | Calc  | RataRes | RataFreq | SysType |    RA | ApsInd | ApsCd | Summ  || Result | RataRes | RataFreq | RataAps || Note
        /// |  0 | null  | null    | null     | HCL     | 20.00 |      0 | null  | 20.04 || null   | INVALID | null     |    null || CalculateRataLevel is null.
        /// |  1 | false | null    | null     | HCL     | 20.00 |      0 | null  | 20.04 || null   | INVALID | null     |    null || CalculateRataLevel is false.
        /// |  2 | false | PASSED  | 4QTRS    | HCL     | 20.00 |      0 | null  | 20.04 || null   | INVALID | null     |    null || CalculateRataLevel is false and RataResult is initially "PASSED".
        /// |  3 | true  | null    | null     | HCL     | 20.00 |      0 | null  | 20.04 || null   | PASSED  | 4QTRS    |       0 || RataSummaryRelativeAccuracy is LE 20, resulting in PASSED and 4QTRS.
        /// |  4 | true  | null    | null     | HCL     | 20.01 |      0 | null  | 20.04 || null   | PASSED  | 4QTRS    |       0 || RataSummaryRelativeAccuracy is LE 20, resulting in PASSED and 4QTRS.
        /// |  5 | true  | PASSED  | 4QTRS    | HCL     | 20.01 |      0 | null  | 20.04 || null   | PASSED  | 4QTRS    |       0 || RataSummaryRelativeAccuracy is LE 20, and PASSED with existing PASSED results in PASSED and 4QTRS.
        /// |  6 | true  | PASSAPS | 4QTRS    | HCL     | 20.01 |      0 | null  | 20.04 || null   | PASSAPS | 4QTRS    |       0 || RataSummaryRelativeAccuracy is LE 20, and PASSED with existing PASSAPS results in PASSAPS and 4QTRS.
        /// |  7 | true  | FAILED  | 4QTRS    | HCL     | 20.01 |      0 | null  | 20.04 || null   | FAILED  | null     |       0 || RataSummaryRelativeAccuracy is LE 20, and PASSED with existing FAILED results in FAILED and null frequency.
        /// |  8 | true  | INVALID | 4QTRS    | HCL     | 20.01 |      0 | null  | 20.04 || null   | INVALID | null     |       0 || RataSummaryRelativeAccuracy is LE 20, and PASSED with existing INVALID results in INVALID and null frequency.
        /// |  9 | true  | null    | null     | HCL     | -0.01 |      0 | null  | 20.05 || null   | FAILED  | null     |    null || Record RA is less than 0, resulting in a FAILED.
        /// | 10 | true  | null    | null     | HCL     |  0.00 |      0 | null  | 20.05 || null   | FAILED  | null     |    null || Record RA is 0, but fails tolerance, resulting in a FAILED
        /// | 11 | true  | null    | null     | HCL     | 20.01 |      0 | null  | 20.05 || null   | FAILED  | null     |    null || Record RA is GT 20, resulting in a FAILED.
        /// | 12 | true  | PASSED  | 4QTRS    | HCL     | 20.01 |      0 | null  | 20.05 || null   | FAILED  | null     |    null || Record RA is GT 20, resulting in a FAILED, and FAILED with existing PASSED results in FAILED and null frequency.
        /// | 13 | true  | PASSAPS | 4QTRS    | HCL     | 20.01 |      0 | null  | 20.05 || null   | FAILED  | null     |    null || Record RA is GT 20, resulting in a FAILED, and FAILED with existing PASSAPS results in FAILED and null frequency.
        /// | 14 | true  | FAILED  | 4QTRS    | HCL     | 20.01 |      0 | null  | 20.05 || null   | FAILED  | null     |    null || Record RA is GT 20, resulting in a FAILED, and FAILED with existing FAILED results in FAILED and null frequency.
        /// | 15 | true  | INVALID | 4QTRS    | HCL     | 20.01 |      0 | null  | 20.05 || null   | INVALID | null     |    null || Record RA is GT 20, resulting in a FAILED, and FAILED with existing INVALID results in INVALID and null frequency.
        /// | 16 | true  | null    | null     | HCL     | 20.01 |      1 | PS18  | 20.05 || null   | PASSAPS | 4QTRS    |       1 || Record APS Indicator is 1, resulting in a PASSAPS and 4QTRS.
        /// | 17 | true  | null    | null     | HCL     | 20.01 |      1 | PS15  | 20.05 || null   | PASSAPS | 4QTRS    |       1 || Record APS Indicator is 1, resulting in a PASSAPS and 4QTRS.
        /// | 18 | true  | PASSED  | 4QTRS    | HCL     | 20.01 |      1 | PS15  | 20.05 || null   | PASSAPS | 4QTRS    |       1 || Record APS Indicator is 1, resulting in a PASSAPS, and PASSAPS with existing PASSED results in PASSAPS and 4QTRS.
        /// | 19 | true  | PASSAPS | 4QTRS    | HCL     | 20.01 |      1 | PS15  | 20.05 || null   | PASSAPS | 4QTRS    |       1 || Record APS Indicator is 1, resulting in a PASSAPS, and PASSAPS with existing PASSAPS results in PASSAPS and 4QTRS.
        /// | 20 | true  | FAILED  | 4QTRS    | HCL     | 20.01 |      1 | PS15  | 20.05 || null   | FAILED  | null     |       1 || Record APS Indicator is 1, resulting in a PASSAPS, and PASSAPS with existing FAILED results in FAILED and null frequency.
        /// | 21 | true  | INVALID | 4QTRS    | HCL     | 20.01 |      1 | PS15  | 20.05 || null   | INVALID | null     |       1 || Record APS Indicator is 1, resulting in a PASSAPS, and PASSAPS with existing INVALID results in INVALID and null frequency.
        /// | 22 | true  | null    | null     | HCL     | 20.01 |   null | null  | 20.04 || A      | PASSED  | 4QTRS    |       0 || Record APS is null, which results in an A, but RataSummaryRelativeAccuracy is LE 20, resulting in PASSED and 4QTRS.
        /// | 23 | true  | PASSED  | 2QTRS    | HCL     | 20.01 |      0 | null  | 20.04 || null   | PASSED  | 2QTRS    |       0 || RataSummaryRelativeAccuracy is LE 20, and PASSED with existing PASSED and 2QTRS results in PASSED and 2QTRS.
        /// | 24 | true  | PASSAPS | 2QTRS    | HCL     | 20.01 |      1 | PS15  | 20.05 || null   | PASSAPS | 2QTRS    |       1 || ecord APS Indicator is 1, and PASSAPS with existing PASSAPS and 2QTRS results in PASSAPS and 2QTRS.
        /// </summary>
        [TestMethod()]
        public void Rata37_HCL()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            QaParameters.Init(category.Process);
            QaParameters.Category = category;

            /* Input Parameter Values */
            string systemTypeCd = "HCL";

            bool?[] calcList = { null, false, false, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true };
            string[] rataResultList = { null, null, "PASSED", null, null, "PASSED", "PASSAPS", "FAILED", "INVALID", null, null, null, "PASSED", "PASSAPS", "FAILED", "INVALID", null, null, "PASSED", "PASSAPS", "FAILED", "INVALID", null, "PASSED", "PASSAPS" };
            string[] rataFreqList = { null, null, "4QTRS", null, null, "4QTRS", "4QTRS", "4QTRS", "4QTRS", null, null, null, "4QTRS", "4QTRS", "4QTRS", "4QTRS", null, null, "4QTRS", "4QTRS", "4QTRS", "4QTRS", null, "2QTRS", "2QTRS" };
            decimal?[] relAccList = { 20.00m, 20.00m, 20.00m, 20.00m, 20.01m, 20.01m, 20.01m, 20.01m, 20.01m, -0.01m, 0.00m, 20.01m, 20.01m, 20.01m, 20.01m, 20.01m, 20.01m, 20.01m, 20.01m, 20.01m, 20.01m, 20.01m, 20.01m, 20.01m, 20.01m };
            int?[] apsIndList = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, null, 0, 1 };
            string[] apsCodeList = { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, "PS18", "PS15", "PS15", "PS15", "PS15", "PS15", null, null, null };
            decimal?[] summRaList = { 20.04m, 20.04m, 20.04m, 20.04m, 20.04m, 20.04m, 20.04m, 20.04m, 20.04m, 20.05m, 20.05m, 20.05m, 20.05m, 20.05m, 20.05m, 20.05m, 20.05m, 20.05m, 20.05m, 20.05m, 20.05m, 20.05m, 20.04m, 20.04m, 20.05m };

            /* Expected Values */
            string[] resultList = { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, "A", null, null };
            string[] expRataResultList = { "INVALID", "INVALID", "INVALID", "PASSED", "PASSED", "PASSED", "PASSAPS", "FAILED", "INVALID", "FAILED", "FAILED", "FAILED", "FAILED", "FAILED", "FAILED", "INVALID", "PASSAPS", "PASSAPS", "PASSAPS", "PASSAPS", "FAILED", "INVALID", "PASSED", "PASSED", "PASSAPS" };
            string[] expRataFreqList = { null, null, null, "4QTRS", "4QTRS", "4QTRS", "4QTRS", null, null, null, null, null, null, null, null, null, "4QTRS", "4QTRS", "4QTRS", "4QTRS", null, null, "4QTRS", "2QTRS", "2QTRS" };
            int?[] expApsIndList = { null, null, null, 0, 0, 0, 0, 0, 0, null, null, null, null, null, null, null, 1, 1, 1, 1, 1, 1, 0, 0, 1 };

            /* Test Case Count */
            int caseCount = 25;

            /* Check array lengths */
            Assert.AreEqual(caseCount, calcList.Length, "calcList length");
            Assert.AreEqual(caseCount, rataResultList.Length, "rataResultList length");
            Assert.AreEqual(caseCount, rataFreqList.Length, "rataFreqList length");
            Assert.AreEqual(caseCount, relAccList.Length, "relAccList length");
            Assert.AreEqual(caseCount, apsIndList.Length, "apsIndList length");
            Assert.AreEqual(caseCount, apsCodeList.Length, "apsCodeList length");
            Assert.AreEqual(caseCount, summRaList.Length, "summRaList length");
            Assert.AreEqual(caseCount, resultList.Length, "resultList length");
            Assert.AreEqual(caseCount, expRataResultList.Length, "expRataResultList length");
            Assert.AreEqual(caseCount, expRataFreqList.Length, "expRataFreqList length");
            Assert.AreEqual(caseCount, expApsIndList.Length, "expApsIndList length");


            for (int caseDex = 0; caseDex < caseCount; caseDex++)
            {
                /*  Initialize Input Parameters*/
                QaParameters.CalculateRataLevel = calcList[caseDex];
                QaParameters.CurrentRataSummary = new VwQaRataSummaryRow(sysTypeCd: systemTypeCd, relativeAccuracy: relAccList[caseDex], apsInd: apsIndList[caseDex], apsCd: apsCodeList[caseDex]);
                QaParameters.RataFrequency = rataFreqList[caseDex];
                QaParameters.RataResult = rataResultList[caseDex];
                QaParameters.RataSummaryRelativeAccuracy = summRaList[caseDex];
                QaParameters.TestTolerancesCrossCheckTable = new CheckDataView<TestTolerancesRow>();
                {
                    QaParameters.TestTolerancesCrossCheckTable.Add(new TestTolerancesRow(testTypeCode: "RATA", fieldDescription: "RelativeInaccuracy", tolerance: "0.1"));
                    QaParameters.TestTolerancesCrossCheckTable.Add(new TestTolerancesRow(testTypeCode: "RATA", fieldDescription: "RelativeAccuracy", tolerance: "0.01"));
                    QaParameters.TestTolerancesCrossCheckTable.Add(new TestTolerancesRow(testTypeCode: "RADA", fieldDescription: "RelativeAccuracy", tolerance: "0.01"));
                }

                /*  Initialize Output Parameters*/
                QaParameters.RataSummaryApsIndicator = int.MinValue;

                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Initialize variables needed to run the check. */
                bool log = false;
                string actual = null;

                /* Run Check */
                actual = cRATAChecks.RATA37(category, ref log);

                /* Check results */
                Assert.AreEqual(string.Empty, actual, string.Format("Return {0}", caseDex));
                Assert.AreEqual(false, log, string.Format("Log {0}", caseDex));
                Assert.AreEqual(resultList[caseDex], category.CheckCatalogResult, string.Format("Result {0}", caseDex));

                Assert.AreEqual(expRataResultList[caseDex], QaParameters.RataResult, string.Format("RataResult {0}", caseDex));
                Assert.AreEqual(expRataFreqList[caseDex], QaParameters.RataFrequency, string.Format("RataFrequency {0}", caseDex));
                Assert.AreEqual(expApsIndList[caseDex], QaParameters.RataSummaryApsIndicator, string.Format("RataSummaryApsIndicator {0}", caseDex));
            }
        }

        #endregion

        #region RATA-38

        ///// <summary>
        /////A test for RATA38_ClaimCode
        /////</summary>()
        [TestMethod()]
		public void RATA38_ClaimCode()
		{
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			QaParameters.Init(category.Process);
			QaParameters.Category = category;

			// Variables
			bool log = false;
			string actual;
			string[] testClaimCodeList = { "ORE", "NLE", "BAD" };
			bool[] testBoolList = { true, false };

			// Init Input
			QaParameters.CurrentRataSummary = new VwQaRataSummaryRow(opLevelCd: "H");
			QaParameters.LoadLowerBoundary = null;
			QaParameters.LoadRecords = new CheckDataView<VwMonitorLoadRow>();
			QaParameters.LoadUpperBoundary = null;
			QaParameters.RataSummaryAverageGrossUnitLoad = null;

			//check ClaimCode
			{
				QaParameters.CalculateRataLevel = true;
				QaParameters.TestDatesConsistent = true;

				foreach (string testClaimCode in testClaimCodeList)
				{
					QaParameters.RataClaimCode = testClaimCode;

					// Init Output
					category.CheckCatalogResult = null;

					// Run Checks
					actual = cRATAChecks.RATA38(category, ref log);

					// Check Results
					Assert.AreEqual(string.Empty, actual);
					Assert.AreEqual(false, log);
					if (QaParameters.CalculateRataLevel == true && QaParameters.TestDatesConsistent == true && testClaimCode.NotInList("ORE,NLE"))
					{
						Assert.AreEqual("D", category.CheckCatalogResult, "Result");
					}
					else
					{
						Assert.AreEqual(null, category.CheckCatalogResult, "Result");
					}
				}
			}

			// check Bools			
			foreach (bool testBool in testBoolList)
			{
				QaParameters.CalculateRataLevel = testBool;
				QaParameters.TestDatesConsistent = !testBool;

				foreach (string testClaimCode in testClaimCodeList)
				{
					QaParameters.RataClaimCode = testClaimCode;

					// Init Output
					category.CheckCatalogResult = null;

					// Run Checks
					actual = cRATAChecks.RATA38(category, ref log);

					// Check Results
					Assert.AreEqual(string.Empty, actual);
					Assert.AreEqual(false, log);
					if (QaParameters.CalculateRataLevel == true && QaParameters.TestDatesConsistent == true && testClaimCode.NotInList("ORE,NLE"))
					{
						Assert.AreEqual("D", category.CheckCatalogResult, "Result");
					}
					else
					{
						Assert.AreEqual(null, category.CheckCatalogResult, "Result");
					}
				}
			}
		}
		#endregion
		
		#region RATA-39

		/// <summary>
		///A test for RATA39_HG
		///</summary>()
		[TestMethod()]
		public void RATA39_HG()
		{
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			QaParameters.Init(category.Process);
			QaParameters.Category = category;

			// Variables
			bool log = false;
			string actual;
			string[] testSystemTypeList = { "HGBAD", "HG", "HCL", "HF", "ST" };

			// Init Input
			QaParameters.HighBaf = (decimal)0;
			QaParameters.LowBaf = (decimal)0;
			QaParameters.MidBaf = (decimal)0;
			QaParameters.RataSummaryApsIndicator = 0;
			QaParameters.RataSummaryConfidenceCoefficient = 1;
			QaParameters.RataSummaryMeanCemValue = 4;
			QaParameters.RataSummaryMeanDifference = 2;
			QaParameters.RataSummaryMeanReferenceValue = 0;
			QaParameters.TestTolerancesCrossCheckTable = new CheckDataView<TestTolerancesRow>
				(new TestTolerancesRow(tolerance: "1", fieldDescription: "BAF"));

			{
				foreach (string testSystemTypeCode in testSystemTypeList)
				{
					// Init Input
					QaParameters.CurrentRataSummary = new VwQaRataSummaryRow(sysTypeCd: testSystemTypeCode, opLevelCd: "H", biasAdjFactor: (decimal)1);

					// Init Output
					category.CheckCatalogResult = null;
					QaParameters.RataSummaryBaf = null;

					// Run Checks
					actual = cRATAChecks.RATA39(category, ref log);

					// Check Results
					Assert.AreEqual(string.Empty, actual);
					Assert.AreEqual(false, log);
					Assert.AreEqual(null, category.CheckCatalogResult, "Result");

					if (testSystemTypeCode.InList("HG,HCL,HF,ST"))
					{
						Assert.AreEqual((decimal)1, QaParameters.RataSummaryBaf, "RataSummaryBaf");
					}
					else
					{
						Assert.AreEqual((decimal)1.5, QaParameters.RataSummaryBaf, "RataSummaryBaf");
					}
				}
			}
			{//Result C

				foreach (string testSystemTypeCode in testSystemTypeList)
				{
					// Init Input
					QaParameters.CurrentRataSummary = new VwQaRataSummaryRow(sysTypeCd: testSystemTypeCode, opLevelCd: "H", biasAdjFactor: (decimal)1.5);

					// Init Output
					category.CheckCatalogResult = null;
					QaParameters.RataSummaryBaf = null;

					// Run Checks
					actual = cRATAChecks.RATA39(category, ref log);

					// Check Results
					Assert.AreEqual(string.Empty, actual);
					Assert.AreEqual(false, log);

					if (testSystemTypeCode.InList("HG,HCL,HF,ST"))
					{
						Assert.AreEqual("C", category.CheckCatalogResult, "Result");
					}
					else
					{
						Assert.AreEqual(null, category.CheckCatalogResult, "Result");
					}
				}
			}

		}

		#endregion

		#region RATA-46

		/// <summary>
		///A test for RATA46_HG
		///</summary>()
		[TestMethod()]
		public void RATA46_HG()
		{
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			QaParameters.Init(category.Process);
			QaParameters.Category = category;

			// Variables
			bool log = false;
			string actual;
			string[] testSystemTypeList = { "HGBAD", "HG", "ST" };

			// Init Input
			QaParameters.RataNumberOfLoadLevels = 1;
			QaParameters.RataSummaryRecords = new CheckDataView<VwQaRataSummaryRow>
				(new VwQaRataSummaryRow(refMethodCd: "29"));
			QaParameters.TestDatesConsistent = true;

			foreach (string testSystemTypeCode in testSystemTypeList)
			{
				QaParameters.CurrentRata = new VwQaTestSummaryRataRow(sysTypeCd: testSystemTypeCode,
					beginDate: DateTime.Today.AddDays(-15), beginHour: 0,
					endDate: DateTime.Today, endHour: 23);

				// Init Output
				category.CheckCatalogResult = null;

				// Run Checks
				actual = cRATAChecks.RATA46(category, ref log);

				// Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				if (testSystemTypeCode.InList("HG,ST"))
				{
					Assert.AreEqual("C", category.CheckCatalogResult, "Result");
				}
				else
				{
					Assert.AreEqual("A", category.CheckCatalogResult, "Result");
				}
			}
		}
        #endregion

        #region RATA-47

        /// <summary>
        /// 
        /// Notes:
        /// 
        /// * Currently does not check for multiple load rows.
        /// * Currently only checks for result E. 
        /// 
        /// 
        /// Parameters:
        /// 
        /// Levels : RATA Level List 
        /// Claim  : RATA Claim Code 
        /// Count  : RATA Number of Load Levels 
        /// RATA   : Current_RATA
        /// Load1  : Load_Records
        /// Load2  : Load_Records
        /// 
        /// Normal : Normal RATA Operating Levels 
        /// Used   : RATA Frequently Used Levels 
        /// 
        /// 
        /// |                             |            RATA           |                  Load 1                 |                  Load 1                 ||                        || 
        /// | ## | Levels | Claim | Count | Begin  | End    | Type    | Begin  | End    | Normal | Second | Ind | Begin  | End    | Normal | Second | Ind || Result | Normal | Used || Note
        /// |  0 | H,M    | SLC   | 2     | Beg    | End    | HG      | Beg    | null   | H      | M      | 1   |                                         || null   | null   | null || Non Flow, normal level exists.
        /// |  1 | H,M    | SLC   | 2     | Beg    | End    | HG      | Beg    | null   | H      | M      | 1   |                                         || null   | null   | null || Non Flow, normal level does not exist, but second level exists.
        /// |  2 | M      | SLC   | 2     | Beg    | End    | CO2     | Beg    | null   | H      | null   | 1   |                                         || B      | null   | null || CO2, normal level does not exist, but second level is null.
        /// |  3 | M      | SLC   | 2     | Beg    | End    | HCL     | Beg    | null   | H      | null   | 1   |                                         || B      | null   | null || HCL, normal level does not exist, but second level is null.
        /// |  4 | M      | SLC   | 2     | Beg    | End    | HF      | Beg    | null   | H      | null   | 1   |                                         || B      | null   | null || HF, normal level does not exist, but second level is null.
        /// |  5 | M      | SLC   | 2     | Beg    | End    | HG      | Beg    | null   | H      | null   | 1   |                                         || B      | null   | null || HG, normal level does not exist, but second level is null.
        /// |  6 | M      | SLC   | 2     | Beg    | End    | NOX     | Beg    | null   | H      | null   | 1   |                                         || B      | null   | null || NOX, normal level does not exist, but second level is null.
        /// |  7 | M      | SLC   | 2     | Beg    | End    | SO2     | Beg    | null   | H      | null   | 1   |                                         || B      | null   | null || SO2, normal level does not exist, but second level is null.
        /// |  8 | M      | SLC   | 2     | Beg    | End    | ST      | Beg    | null   | H      | null   | 1   |                                         || B      | null   | null || ST, normal level does not exist, but second level is null.
        /// |  9 | M      | SLC   | 2     | Beg    | End    | CO2     | Beg    | null   | H      | L      | 1   |                                         || E      | null   | null || CO2, normal and second levels do not exist.
        /// | 10 | M      | SLC   | 2     | Beg    | End    | HCL     | Beg    | null   | H      | L      | 1   |                                         || null   | null   | null || HCL, normal and second levels do not exist.
        /// | 11 | M      | SLC   | 2     | Beg    | End    | HF      | Beg    | null   | H      | L      | 1   |                                         || null   | null   | null || HF, normal and second levels do not exist.
        /// | 12 | M      | SLC   | 2     | Beg    | End    | HG      | Beg    | null   | H      | L      | 1   |                                         || null   | null   | null || HG, normal and second levels do not exist.
        /// | 13 | M      | SLC   | 2     | Beg    | End    | NOX     | Beg    | null   | H      | L      | 1   |                                         || E      | null   | null || NOX, normal and second levels do not exist.
        /// | 14 | M      | SLC   | 2     | Beg    | End    | SO2     | Beg    | null   | H      | L      | 1   |                                         || E      | null   | null || SO2, normal and second levels do not exist.
        /// | 15 | M      | SLC   | 2     | Beg    | End    | ST      | Beg    | null   | H      | L      | 1   |                                         || null   | null   | null || ST, normal and second levels do not exist.
        /// | 16 | M      | SLC   | 2     | Beg    | End    | CO2     | Beg    | null   | H      | M      | 0   |                                         || E      | null   | null || CO2, normal level does not exist, second level exists, but second level indicator is 0.
        /// | 17 | M      | SLC   | 2     | Beg    | End    | HCL     | Beg    | null   | H      | M      | 0   |                                         || null   | null   | null || HCL, normal level does not exist, second level exists, but second level indicator is 0.
        /// | 18 | M      | SLC   | 2     | Beg    | End    | HF      | Beg    | null   | H      | M      | 0   |                                         || null   | null   | null || HF, normal level does not exist, second level exists, but second level indicator is 0.
        /// | 19 | M      | SLC   | 2     | Beg    | End    | HG      | Beg    | null   | H      | M      | 0   |                                         || null   | null   | null || HG, normal level does not exist, second level exists, but second level indicator is 0.
        /// | 20 | M      | SLC   | 2     | Beg    | End    | NOX     | Beg    | null   | H      | M      | 0   |                                         || E      | null   | null || NOX, normal level does not exist, second level exists, but second level indicator is 0.
        /// | 21 | M      | SLC   | 2     | Beg    | End    | SO2     | Beg    | null   | H      | M      | 0   |                                         || E      | null   | null || SO2, normal level does not exist, second level exists, but second level indicator is 0.
        /// | 22 | M      | SLC   | 2     | Beg    | End    | ST      | Beg    | null   | H      | M      | 0   |                                         || null   | null   | null || ST, normal level does not exist, second level exists, but second level indicator is 0.
        /// </summary>
        [TestMethod()]
        public void Rata47_NonFlowMultipleLevel()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            QaParameters.Init(category.Process);
            QaParameters.Category = category;

            /* Input Parameter Values */
            DateTime beg = new DateTime(2017, 06, 17, 22, 08, 0);
            DateTime end = new DateTime(2017, 06, 18, 0, 30, 0);

            string[] levelsList = { "H,M", "H,M", "M", "M", "M", "M", "M", "M", "M", "M", "M", "M", "M", "M", "M", "M", "M", "M", "M", "M", "M", "M", "M" };
            string[] claimList = { "SLC", "SLC", "SLC", "SLC", "SLC", "SLC", "SLC", "SLC", "SLC", "SLC", "SLC", "SLC", "SLC", "SLC", "SLC", "SLC", "SLC", "SLC", "SLC", "SLC", "SLC", "SLC", "SLC" };
            int?[] countList = { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 };
            DateTime?[] rataBeginList = { beg, beg, beg, beg, beg, beg, beg, beg, beg, beg, beg, beg, beg, beg, beg, beg, beg, beg, beg, beg, beg, beg, beg };
            DateTime?[] rataEndList = { end, end, end, end, end, end, end, end, end, end, end, end, end, end, end, end, end, end, end, end, end, end, end };
            string[] rataSysTypeList = { "HG", "HG", "CO2", "HCL", "HF", "HG", "NOX", "SO2", "ST", "CO2", "HCL", "HF", "HG", "NOX", "SO2", "ST", "CO2", "HCL", "HF", "HG", "NOX", "SO2", "ST" };
            DateTime?[] load1BeginList = { beg, beg, beg, beg, beg, beg, beg, beg, beg, beg, beg, beg, beg, beg, beg, beg, beg, beg, beg, beg, beg, beg, beg };
            DateTime?[] load1EndList = { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null };
            string[] load1NormalList = { "H", "H", "H", "H", "H", "H", "H", "H", "H", "H", "H", "H", "H", "H", "H", "H", "H", "H", "H", "H", "H", "H", "H" };
            string[] load1SecondList = { "M", "M", null, null, null, null, null, null, null, "L", "L", "L", "L", "L", "L", "L", "M", "M", "M", "M", "M", "M", "M" };
            int?[] load1IndicatorList = { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0 };
            DateTime?[] load2BeginList = { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null };
            DateTime?[] load2EndList = { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null };
            string[] load2NormalList = { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null };
            string[] load2SecondList = { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null };
            int?[] load2IndicatorList = { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null };

            /* Expected Values */
            string[] resultList = { null, null, "B", "B", "B", "B", "B", "B", "B", "E", null, null, null, "E", "E", null, "E", null, null, null, "E", "E", null };
            string[] expNormalList = { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null };
            string[] expUsedList = { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null };

            /* Test Case Count */
            int caseCount = 23;

            /* Check array lengths */
            Assert.AreEqual(caseCount, levelsList.Length, "levelsList length");
            Assert.AreEqual(caseCount, claimList.Length, "claimList length");
            Assert.AreEqual(caseCount, countList.Length, "countList length");
            Assert.AreEqual(caseCount, rataBeginList.Length, "rataBeginList length");
            Assert.AreEqual(caseCount, rataEndList.Length, "rataEndList length");
            Assert.AreEqual(caseCount, rataSysTypeList.Length, "rataSysTypeList length");
            Assert.AreEqual(caseCount, load1BeginList.Length, "load1BeginList length");
            Assert.AreEqual(caseCount, load1EndList.Length, "load1EndList length");
            Assert.AreEqual(caseCount, load1NormalList.Length, "load1NormalList length");
            Assert.AreEqual(caseCount, load1SecondList.Length, "load1SecondList length");
            Assert.AreEqual(caseCount, load1IndicatorList.Length, "load1IndicatorList length");
            Assert.AreEqual(caseCount, load2BeginList.Length, "load2BeginList length");
            Assert.AreEqual(caseCount, load2EndList.Length, "load2EndList length");
            Assert.AreEqual(caseCount, load2NormalList.Length, "load2NormalList length");
            Assert.AreEqual(caseCount, load2SecondList.Length, "load2SecondList length");
            Assert.AreEqual(caseCount, load2IndicatorList.Length, "load2IndicatorList length");
            Assert.AreEqual(caseCount, resultList.Length, "resultList length");
            Assert.AreEqual(caseCount, expNormalList.Length, "expNormalList length");
            Assert.AreEqual(caseCount, expUsedList.Length, "expUsedList length");


            for (int caseDex = 0; caseDex < caseCount; caseDex++)
            {
                /*  Initialize Input Parameters*/
                QaParameters.CurrentRata = SetValues.DateHour(new VwQaTestSummaryRataRow(sysTypeCd: rataSysTypeList[caseDex], beginDate: rataBeginList[caseDex].Value.Date), rataBeginList[caseDex], rataEndList[caseDex]);
                QaParameters.LoadRecords = new CheckDataView<VwMonitorLoadRow>();
                {
                    QaParameters.LoadRecords.Add(SetValues.DateHour(new VwMonitorLoadRow(normalLevelCd: load1NormalList[caseDex], secondLevelCd: load1SecondList[caseDex], secondNormalInd: load1IndicatorList[caseDex]), load1BeginList[caseDex], load1EndList[caseDex]));

                    if (load2BeginList[caseDex] != null)
                        QaParameters.LoadRecords.Add(SetValues.DateHour(new VwMonitorLoadRow(normalLevelCd: load2NormalList[caseDex], secondLevelCd: load2SecondList[caseDex], secondNormalInd: load2IndicatorList[caseDex]), load2BeginList[caseDex], load2EndList[caseDex]));
                }
                QaParameters.RataClaimCode = claimList[caseDex];
                QaParameters.RataLevelList = levelsList[caseDex];
                QaParameters.RataNumberOfLoadLevels = countList[caseDex];

                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Initialize variables needed to run the check. */
                bool log = false;
                string actual = null;

                /* Run Check */
                actual = cRATAChecks.RATA47(category, ref log);

                /* Check results */
                Assert.AreEqual(string.Empty, actual, string.Format("Return {0}", caseDex));
                Assert.AreEqual(false, log, string.Format("Log {0}", caseDex));

                Assert.AreEqual(resultList[caseDex], category.CheckCatalogResult, string.Format("Result {0}", caseDex));
                Assert.AreEqual(expNormalList[caseDex], QaParameters.NormalRataOperatingLevels, string.Format("NormalRataOperatingLevels {0}", caseDex));
                Assert.AreEqual(expUsedList[caseDex], QaParameters.RataFrequentlyUsedLevels, string.Format("RataFrequentlyUsedLevels {0}", caseDex));
            }
        }

        #endregion

        #region RATA-52

        /// <summary>
        ///A test for RATA52_HG
        ///</summary>()
        [TestMethod()]
		public void RATA52_HG()
		{
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			QaParameters.Init(category.Process);
			QaParameters.Category = category;

			// Variables
			bool log = false;
			string actual;
			string[] testSystemTypeList = { "HGBAD", "HG", "ST" };

			// Init Input
			QaParameters.EcmpsMpBeginDate = null;
			QaParameters.FacilityQualificationRecords = new CheckDataView<MonitorQualificationRow>();
			QaParameters.LocationAttributeRecords = new CheckDataView<MonitorLocationAttributeRow>();
			QaParameters.LocationReportingFrequencyRecords = new CheckDataView<VwLocationReportingFrequencyRow>();
			QaParameters.OverallBaf = (decimal)0;
			QaParameters.RataClaimCode = "";
			QaParameters.RataFrequencyCodeLookupTable = new CheckDataView<RataFrequencyCodeRow>
				(new RataFrequencyCodeRow(rataFrequencyCd: "NOTOS"),
				new RataFrequencyCodeRow(rataFrequencyCd: "8QTRS"));
			QaParameters.RataNumberOfLoadLevels = 0;
			QaParameters.RataResult = "PASSED";

			foreach (string testSystemTypeCode in testSystemTypeList)
			{
				QaParameters.RataFrequency = "NOTOS";
				QaParameters.CurrentRata = new VwQaTestSummaryRataRow(sysTypeCd: testSystemTypeCode, rataFrequencyCd: "8QTRS", sysDesignationCd: "B",
					beginDate: DateTime.Today.AddDays(-10), beginHour: 0, endDate: DateTime.Today, endHour: 23);

				// Init Output
				QaParameters.RataFrequencyDetermined = null;
				category.CheckCatalogResult = null;

				// Run Checks
				actual = cRATAChecks.RATA52(category, ref log);

				// Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				if (testSystemTypeCode.InList("HG,ST"))
				{
					Assert.AreEqual(true, QaParameters.RataFrequencyDetermined, "RataFrequencyDetermined");
					Assert.AreEqual("NOTOS", QaParameters.RataFrequency, "RataFrequency");
					Assert.AreEqual("D", category.CheckCatalogResult, "Result");
				}
				else
				{
					Assert.AreEqual(true, QaParameters.RataFrequencyDetermined, "RataFrequencyDetermined");
					Assert.AreEqual("8QTRS", QaParameters.RataFrequency, "RataFrequency");
					Assert.AreEqual(null, category.CheckCatalogResult, "Result");
				}

			}
		}
		#endregion

		#region RATA-125

		/// <summary>
		///A test for RATA125_HGST
		///</summary>()
		[TestMethod()]
		public void RATA125_HGST()
		{
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			QaParameters.Init(category.Process);
			QaParameters.Category = category;

			// Variables
			bool log = false;
			string actual;
			string[] testSystemTypeList = { "SYSBAD", "HG", "ST" };

			// Init Input
			QaParameters.LocationAttributeRecords = new CheckDataView<MonitorLocationAttributeRow>();
			QaParameters.RataRunRecords = new CheckDataView<VwQaRataRunRow>
				(new VwQaRataRunRow(runNum: 1, runStatusCd: "RUNUSED", opLevelCd: "H", cemValue: 1, grossUnitLoad: 1, rataRefValue: (decimal)2.01, beginDate: DateTime.Today.AddDays(-10), beginHour: 0, beginMin: 0, endDate: DateTime.Today.AddDays(-10), endHour: 23, endMin: 59),
				new VwQaRataRunRow(runNum: 2, runStatusCd: "RUNUSED", opLevelCd: "H", cemValue: 1, grossUnitLoad: 1, rataRefValue: (decimal)2.01, beginDate: DateTime.Today.AddDays(-9), beginHour: 0, beginMin: 0, endDate: DateTime.Today.AddDays(-9), endHour: 23, endMin: 59),
				new VwQaRataRunRow(runNum: 3, runStatusCd: "RUNUSED", opLevelCd: "H", cemValue: 1, grossUnitLoad: 1, rataRefValue: (decimal)2.01, beginDate: DateTime.Today.AddDays(-8), beginHour: 0, beginMin: 0, endDate: DateTime.Today.AddDays(-8), endHour: 23, endMin: 59),
				new VwQaRataRunRow(runNum: 4, runStatusCd: "RUNUSED", opLevelCd: "H", cemValue: 1, grossUnitLoad: 1, rataRefValue: (decimal)2, beginDate: DateTime.Today.AddDays(-7), beginHour: 0, beginMin: 0, endDate: DateTime.Today.AddDays(-7), endHour: 23, endMin: 59),
				new VwQaRataRunRow(runNum: 5, runStatusCd: "RUNUSED", opLevelCd: "H", cemValue: 1, grossUnitLoad: 1, rataRefValue: (decimal)2, beginDate: DateTime.Today.AddDays(-6), beginHour: 0, beginMin: 0, endDate: DateTime.Today.AddDays(-6), endHour: 23, endMin: 59),
				new VwQaRataRunRow(runNum: 6, runStatusCd: "RUNUSED", opLevelCd: "H", cemValue: 1, grossUnitLoad: 1, rataRefValue: (decimal)2, beginDate: DateTime.Today.AddDays(-5), beginHour: 0, beginMin: 0, endDate: DateTime.Today.AddDays(-5), endHour: 23, endMin: 59),
				new VwQaRataRunRow(runNum: 7, runStatusCd: "RUNUSED", opLevelCd: "H", cemValue: 1, grossUnitLoad: 1, rataRefValue: (decimal)2, beginDate: DateTime.Today.AddDays(-4), beginHour: 0, beginMin: 0, endDate: DateTime.Today.AddDays(-4), endHour: 23, endMin: 59),
				new VwQaRataRunRow(runNum: 8, runStatusCd: "RUNUSED", opLevelCd: "H", cemValue: 1, grossUnitLoad: 1, rataRefValue: (decimal)2, beginDate: DateTime.Today.AddDays(-3), beginHour: 0, beginMin: 0, endDate: DateTime.Today.AddDays(-3), endHour: 23, endMin: 59),
				new VwQaRataRunRow(runNum: 9, runStatusCd: "RUNUSED", opLevelCd: "H", cemValue: 1, grossUnitLoad: 1, rataRefValue: (decimal)2, beginDate: DateTime.Today.AddDays(-2), beginHour: 0, beginMin: 0, endDate: DateTime.Today.AddDays(-2), endHour: 23, endMin: 59)
				);
			QaParameters.ReferenceMethodCodeLookupTable = new CheckDataView<RefMethodCodeRow>
				(new RefMethodCodeRow(refMethodCd: "REFGOOD", parameterCd: "HG"));
			QaParameters.TvaluesCrossCheckTable = new CheckDataView<TValuesRow>(new TValuesRow(numberOfItems: "8", tValue: "1"));

			foreach (string testSytemTypeCode in testSystemTypeList)
			{
				QaParameters.CurrentRata = new VwQaTestSummaryRataRow(sysTypeCd: testSytemTypeCode);
				QaParameters.CurrentRataSummary = new VwQaRataSummaryRow(opLevelCd: "H", sysTypeCd: testSytemTypeCode, refMethodCd: "REFGOOD");
				// Init Output
				category.CheckCatalogResult = null;

				// Run Checks
				actual = cRATAChecks.RATA125(category, ref log);

				// Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual(null, category.CheckCatalogResult, "Result");
				if (testSytemTypeCode.InList("HG,ST"))
				{
					Assert.AreEqual(1, QaParameters.RataCalcAps, "RataCalcAPS");
					Assert.AreEqual(1, QaParameters.RataCalcBaf, "RataCalcBAF");
				}
				else
				{
					Assert.AreNotEqual(1, QaParameters.RataCalcAps, "RataCalcAPS");
					Assert.AreNotEqual(1, QaParameters.RataCalcBaf, "RataCalcBAF");
				}

			}
		}
		#endregion

		#region RATA-92

		/// <summary>
		///A test for RATA92
		///</summary>()
		[TestMethod()]
		public void RATA92()
		{
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			QaParameters.Init(category.Process);
			QaParameters.Category = category;

			// Variables
			bool log = false;
			string actual;

			//result A
			{
				// Init Input
				QaParameters.CurrentFlowRataRun = new VwQaFlowRataRunRow(refMethodCd: "BAD", calcWaf: 1);
				QaParameters.RataCalcCalculatedWaf = 1;
				QaParameters.RataSumWaf = (decimal)0.1234567;
				QaParameters.RataReplacementPointCount = 1;
				QaParameters.TestTolerancesCrossCheckTable = new CheckDataView<TestTolerancesRow>();

				// Init Output
				category.CheckCatalogResult = null;

				// Run Checks
				actual = cRATAChecks.RATA92(category, ref log);

				// Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual("A", category.CheckCatalogResult, "Result");
				Assert.AreEqual((decimal)1, category.GetCheckParameter("RATA_Calc_Calculated_WAF").ParameterValue, "CalcCalcWAF");
			}

			//result B
			{
				// Init Input
				QaParameters.CurrentFlowRataRun = new VwQaFlowRataRunRow(refMethodCd: "2FH", calcWaf: null);
				QaParameters.RataCalcCalculatedWaf = 1;
				QaParameters.RataSumWaf = (decimal)0.1234567;
				QaParameters.RataReplacementPointCount = 1;
				QaParameters.TestTolerancesCrossCheckTable = new CheckDataView<TestTolerancesRow>();

				// Init Output
				category.CheckCatalogResult = null;

				// Run Checks
				actual = cRATAChecks.RATA92(category, ref log);

				// Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual("B", category.CheckCatalogResult, "Result");
				Assert.AreEqual((decimal)0.1235, category.GetCheckParameter("RATA_Calc_Calculated_WAF").ParameterValue, "CalcCalcWAF");
			}

			//result C
			{
				// Init Input
				QaParameters.CurrentFlowRataRun = new VwQaFlowRataRunRow(refMethodCd: "2FH", calcWaf: 1);
				QaParameters.RataCalcCalculatedWaf = 1;
				QaParameters.RataSumWaf = (decimal)0.1234567;
				QaParameters.RataReplacementPointCount = 0;
				QaParameters.TestTolerancesCrossCheckTable = new CheckDataView<TestTolerancesRow>();

				// Init Output
				category.CheckCatalogResult = null;

				// Run Checks
				actual = cRATAChecks.RATA92(category, ref log);

				// Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual("C", category.CheckCatalogResult, "Result");
				Assert.AreEqual((decimal)0.1235, category.GetCheckParameter("RATA_Calc_Calculated_WAF").ParameterValue, "CalcCalcWAF");
			}

			//result D
			{
				// Init Input
				QaParameters.CurrentFlowRataRun = new VwQaFlowRataRunRow(refMethodCd: "2FH", calcWaf: 1);
				QaParameters.RataCalcCalculatedWaf = 1;
				QaParameters.RataSumWaf = (decimal)0.1234567;
				QaParameters.RataReplacementPointCount = 1;
				QaParameters.TestTolerancesCrossCheckTable = new CheckDataView<TestTolerancesRow>(
					new TestTolerancesRow(testTypeCode: "RATA", fieldDescription: "WAF", tolerance: "0.01"));

				// Init Output
				category.CheckCatalogResult = null;

				// Run Checks
				actual = cRATAChecks.RATA92(category, ref log);

				// Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual("D", category.CheckCatalogResult, "Result");
				Assert.AreEqual((decimal)0.1235, category.GetCheckParameter("RATA_Calc_Calculated_WAF").ParameterValue, "CalcCalcWAF");
			}
		}
		#endregion

		#region RATA-130

		/// <summary>
		///A test for RATA130
		///</summary>()
		[TestMethod()]
		public void RATA130()
		{
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			QaParameters.Init(category.Process);
			QaParameters.Category = category;

			// Variables
			bool log = false;
			string actual;

			//Result A
			{
				// Init Input
				QaParameters.RataRunBeginTimeValid = true;
				QaParameters.RataRunEndTimeValid = true;
				QaParameters.CurrentRata = new VwQaTestSummaryRataRow(sysTypeCd: "FLOW");
				QaParameters.CurrentRataRun = new VwQaRataRunRow(runStatusCd: "RUNUSED", beginDate: DateTime.Today, beginHour: 1, beginMin: 0,
					endDate: DateTime.Today, endHour: 1, endMin: 3);

				// Init Output
				category.CheckCatalogResult = null;

				// Run Checks
				actual = cRATAChecks.RATA130(category, ref log);

				// Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual("A", category.CheckCatalogResult, "Result");
			}
			//Result B
			{
				// Init Input
				QaParameters.RataRunBeginTimeValid = true;
				QaParameters.RataRunEndTimeValid = true;
				QaParameters.CurrentRata = new VwQaTestSummaryRataRow(sysTypeCd: "NOTHG");
				QaParameters.CurrentRataRun = new VwQaRataRunRow(runStatusCd: "RUNUSED", beginDate: DateTime.Today, beginHour: 1, beginMin: 0,
					endDate: DateTime.Today, endHour: 1, endMin: 4);

				// Init Output
				category.CheckCatalogResult = null;

				// Run Checks
				actual = cRATAChecks.RATA130(category, ref log);

				// Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual("B", category.CheckCatalogResult, "Result");
			}
			//Pass FLOW
			{
				// Init Input
				QaParameters.RataRunBeginTimeValid = true;
				QaParameters.RataRunEndTimeValid = true;
				QaParameters.CurrentRata = new VwQaTestSummaryRataRow(sysTypeCd: "FLOW");
				QaParameters.CurrentRataRun = new VwQaRataRunRow(runStatusCd: "RUNUSED", beginDate: DateTime.Today, beginHour: 1, beginMin: 0,
					endDate: DateTime.Today, endHour: 1, endMin: 4);

				// Init Output
				category.CheckCatalogResult = null;

				// Run Checks
				actual = cRATAChecks.RATA130(category, ref log);

				// Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual(null, category.CheckCatalogResult, "Result");
			}
			//Pass HG
			{
				// Init Input
				QaParameters.RataRunBeginTimeValid = true;
				QaParameters.RataRunEndTimeValid = true;
				QaParameters.CurrentRata = new VwQaTestSummaryRataRow(sysTypeCd: "HGSTART");
				QaParameters.CurrentRataRun = new VwQaRataRunRow(runStatusCd: "RUNUSED", beginDate: DateTime.Today, beginHour: 1, beginMin: 0,
					endDate: DateTime.Today, endHour: 1, endMin: 20);

				// Init Output
				category.CheckCatalogResult = null;

				// Run Checks
				actual = cRATAChecks.RATA130(category, ref log);

				// Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual(null, category.CheckCatalogResult, "Result");
			}
			//Pass statuscode
			{
				// Init Input
				QaParameters.RataRunBeginTimeValid = true;
				QaParameters.RataRunEndTimeValid = true;
				QaParameters.CurrentRata = new VwQaTestSummaryRataRow(sysTypeCd: "FLOW");
				QaParameters.CurrentRataRun = new VwQaRataRunRow(runStatusCd: "BAD", beginDate: DateTime.Today, beginHour: 1, beginMin: 0,
					endDate: DateTime.Today, endHour: 1, endMin: 3);

				// Init Output
				category.CheckCatalogResult = null;

				// Run Checks
				actual = cRATAChecks.RATA130(category, ref log);

				// Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual(null, category.CheckCatalogResult, "Result");
			}

		}
        #endregion


        /// <summary>
        /// 
        /// 
        /// | ## | SysTypeCd | ApsInd | ApsCode || Result || Note
        /// |  0 | HG        | 1      | PS15    || A      || APS Code exists, but System Type is not the required value of "HCL".
        /// |  1 | ST        | 1      | PS15    || A      || APS Code exists, but System Type is not the required value of "HCL".
        /// |  2 | HF        | 1      | PS15    || A      || APS Code exists, but System Type is not the required value of "HCL".
        /// |  3 | SO2       | 1      | PS15    || A      || APS Code exists, but System Type is not the required value of "HCL".
        /// |  4 | HCL       | 0      | PS15    || B      || APS Code exists, but APS Indicator is not the required value of 1.
        /// |  5 | HCL       | null   | PS15    || B      || APS Code exists, but APS Indicator is not the required value of 1.
        /// |  6 | HCL       | 1      | PS13    || C      || APS Code exists, but does not have the required value of "PS15" or "PS18".
        /// |  7 | HCL       | 1      | PS15    || null   || APS Code exists, has the required value of "PS15", is "HCL", and APS Indicator is 1, which makes the APS Code valid.
        /// |  8 | HCL       | 1      | PS18    || null   || APS Code exists, has the required value of "PS18", is "HCL", and APS Indicator is 1, which makes the APS Code valid.
        /// |  9 | HCL       | 1      | null    || null   || Null APS Code is allowed for all System Type and APS Indicator values, including HCL.
        /// | 10 | HG        | 1      | null    || null   || Null APS Code is allowed for all System Type and APS Indicator values, including HCL.
        /// | 11 | ST        | 1      | null    || null   || Null APS Code is allowed for all System Type and APS Indicator values, including HCL.
        /// | 12 | HF        | 1      | null    || null   || Null APS Code is allowed for all System Type and APS Indicator values, including HCL.
        /// | 13 | SO2       | 1      | null    || null   || Null APS Code is allowed for all System Type and APS Indicator values, including HCL.
        /// </summary>
        [TestMethod()]
        public void Rata131()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            QaParameters.Init(category.Process);
            QaParameters.Category = category;

            /* Input Parameter Values */
            string[] sysTypeList = { "HG", "ST", "HF", "SO2", "HCL", "HCL", "HCL", "HCL", "HCL", "HCL", "HG", "ST", "HF", "SO2" };
            int?[] apsIndList = { 1, 1, 1, 1, 0, null, 1, 1, 1, 1, 1, 1, 1, 1 };
            string[] apsCodeList = { "PS15", "PS15", "PS15", "PS15", "PS15", "PS15", "PS13", "PS15", "PS18", null, null, null, null, null };

            /* Expected Values */
            string[] resultList = { "A", "A", "A", "A", "B", "B", "C", null, null, null, null, null, null, null };

            /* Test Case Count */
            int caseCount = 14;

            /* Check array lengths */
            Assert.AreEqual(caseCount, sysTypeList.Length, "sysTypeList length");
            Assert.AreEqual(caseCount, apsIndList.Length, "apsIndList length");
            Assert.AreEqual(caseCount, apsCodeList.Length, "apsCodeList length");
            Assert.AreEqual(caseCount, resultList.Length, "resultList length");


            for (int caseDex = 0; caseDex < caseCount; caseDex++)
            {
                /*  Initialize Input Parameters*/
                QaParameters.ApsCodeLookupTable = new CheckDataView<ApsCodeRow>(new ApsCodeRow(apsCd: "PS15"), new ApsCodeRow(apsCd: "PS18"));
                QaParameters.CurrentRataSummary = new VwQaRataSummaryRow(sysTypeCd: sysTypeList[caseDex], apsInd: apsIndList[caseDex], apsCd: apsCodeList[caseDex]);

                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Initialize variables needed to run the check. */
                bool log = false;
                string actual = null;

                /* Run Check */
                actual = cRATAChecks.RATA131(category, ref log);

                /* Check results */
                Assert.AreEqual(string.Empty, actual, string.Format("Return {0}", caseDex));
                Assert.AreEqual(false, log, string.Format("Log {0}", caseDex));
                Assert.AreEqual(resultList[caseDex], category.CheckCatalogResult, string.Format("Result {0}", caseDex));
            }
        }

    }
}
