using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.TestChecks;

using ECMPS.Definitions.Extensions;

using ECMPS.Checks;
using ECMPS.Checks.Data.Ecmps.Dbo.Table;
using ECMPS.Checks.Data.Ecmps.Dbo.View;
using ECMPS.Checks.Data.EcmpsAux.CrossCheck.Virtual;
using ECMPS.Checks.Data.Ecmps.CrossCheck.Table;
using ECMPS.Checks.Qa.Parameters;
using ECMPS.Checks.Mp.Parameters;

using UnitTest.UtilityClasses;

namespace UnitTest.QA
{
	/// <summary>
	/// Summary description for cTestChecksTest
	/// </summary>
	[TestClass]
	public class cTestChecksTest
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

		#region TEST-12

		/// <summary>
		///A test for TEST-12
		///</summary>()
		[TestMethod()]
		public void TEST12()
		{
			//static check setup
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			QaParameters.Init(category.Process);
			QaParameters.Category = category;

			// Variables
			bool log = false;
			string actual;
			string[] testSystemTypeList = { null, "SYSBAD", "LEAK", "PEI", "DAHS", "PEMSACC", "OTHER", "DGFMCAL", "MFMCAL", "TSCAL", "BCAL", "QGA" };

			foreach (string testSystemTypeCode in testSystemTypeList)
			{
				// Init Input
				QaParameters.CurrentTest = new VwQaTestSummaryRow(testTypeCd: testSystemTypeCode);

				// Init Output
				category.CheckCatalogResult = null;

				// Run Checks
				actual = cTestChecks.TEST12(category, ref log);

				// Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				if (testSystemTypeCode == null)
				{
					Assert.AreEqual("A", category.CheckCatalogResult, "Result");
					Assert.AreEqual(true, QaParameters.MiscellaneousTestTypeValid, "MiscellaneousTestTypeValid");
				}

				else if (testSystemTypeCode.InList("LEAK,PEI,DAHS,PEMSACC,OTHER,DGFMCAL,MFMCAL,TSCAL,BCAL,QGA"))
				{
					Assert.AreEqual(null, category.CheckCatalogResult, "Result");
					Assert.AreEqual(true, QaParameters.MiscellaneousTestTypeValid, "MiscellaneousTestTypeValid");
				}
				else
				{
					Assert.AreEqual("B", category.CheckCatalogResult, "Result");
					Assert.AreEqual(false, QaParameters.MiscellaneousTestTypeValid, "MiscellaneousTestTypeValid");
				}
			}
		}
		#endregion

		#region TEST-16

		/// <summary>
		///A test for TEST16_MATS
		///</summary>()
		[TestMethod()]
		public void TEST16_MATS()
		{
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			QaParameters.Init(category.Process);
			QaParameters.Category = category;

			// Variables
			bool log = false;
			string actual;
			string[] testTestTypeList = { "GFMTCAL", "GFMBCAL", "DGFMTCAL", "MFMCAL", "TSCAL", "BCAL", "QGA" };
			string[] testComponentTypeList = { "STRAIN", "GFM", "HCL", "HF" };

			// Init Input
			QaParameters.MonitorSystemRecords = new CheckDataView<VwMonitorSystemRow>();

			foreach (string testTestTypeCode in testTestTypeList)
			{
				foreach (string testComponentTypeCode in testComponentTypeList)
				{
					QaParameters.CurrentTest = new VwQaTestSummaryRow(testTypeCd: testTestTypeCode, componentId: "COMPONENT1");
					QaParameters.ComponentRecords = new CheckDataView<VwComponentRow>(new VwComponentRow(componentId: "COMPONENT1", componentTypeCd: testComponentTypeCode));

					// Init Output
					category.CheckCatalogResult = null;
					QaParameters.MiscellaneousTestIdFieldname = null;

					// Run Checks
					actual = cTestChecks.TEST16(category, ref log);

					// Check Results
					Assert.AreEqual(string.Empty, actual);
					Assert.AreEqual(false, log);
					if ((testTestTypeCode.InList("DGFMTCAL,MFMCAL,TSCAL,BCAL") && testComponentTypeCode != "STRAIN")
						|| (testTestTypeCode == "QGA" && testComponentTypeCode.NotInList("HCL,HF")))
					{
						Assert.AreEqual("C", category.CheckCatalogResult, "Result");
						Assert.AreEqual("component", QaParameters.MiscellaneousTestIdFieldname, "MiscellaneousTestIdFieldname");
					}
					else
					{
						Assert.AreEqual(null, category.CheckCatalogResult, "Result");
						Assert.AreEqual(null, QaParameters.MiscellaneousTestIdFieldname, "MiscellaneousTestIdFieldname");
					}
				}
			}
		}

		/// <summary>
		///A test for TEST16_EMPTY
		///</summary>()
		[TestMethod()]
		public void TEST16_EMPTY()
		{
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			QaParameters.Init(category.Process);
			QaParameters.Category = category;

			// Variables
			bool log = false;
			string actual;

			// Init Input
			QaParameters.MonitorSystemRecords = new CheckDataView<VwMonitorSystemRow>();
			QaParameters.CurrentTest = new VwQaTestSummaryRow(testTypeCd: "GOOD", componentId: "", monSysId: null);

			// Init Output
			category.CheckCatalogResult = null;
			QaParameters.MiscellaneousTestIdFieldname = null;

			// Run Checks
			actual = cTestChecks.TEST16(category, ref log);

			// Check Results
			Assert.AreEqual(string.Empty, actual);
			Assert.AreEqual(false, log);
			Assert.AreEqual("E", category.CheckCatalogResult, "Result");
		}
		#endregion
		#region TEST-23

		/// <summary>
		///A test for TEST23
		///</summary>()
		[TestMethod()]
		public void TEST23()
		{
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			QaParameters.Init(category.Process);
			QaParameters.Category = category;

			// Variables
			bool log = false;
			string actual;

			// Result A
			{
				string[] testTestTypeList = { "7DAY", "CYCLE" };
				// Init Input
				foreach (string testTestTypeCode in testTestTypeList)
				{
					QaParameters.CurrentTest = new VwQaTestSummaryRow(testTypeCd: testTestTypeCode, componentId:"GOOD", injectionProtocolCd: null);
					QaParameters.ComponentRecords = new CheckDataView<VwComponentRow>
						(new VwComponentRow(componentId: "GOOD", componentTypeCd: "HG"));

					// Init Output
					category.CheckCatalogResult = null;

					// Run Checks
					actual = cTestChecks.TEST23(category, ref log);

					// Check Results
					Assert.AreEqual(string.Empty, actual);
					Assert.AreEqual(false, log);
					Assert.AreEqual("A", category.CheckCatalogResult, "Result");
				}
			}

			// Result B
			{
				string[] testTestTypeList = { "7DAY", "CYCLE" };
				// Init Input
				foreach (string testTestTypeCode in testTestTypeList)
				{
					QaParameters.CurrentTest = new VwQaTestSummaryRow(testTypeCd: testTestTypeCode, componentId: "GOOD", injectionProtocolCd: "BAD");
					QaParameters.ComponentRecords = new CheckDataView<VwComponentRow>
						(new VwComponentRow(componentId: "GOOD", componentTypeCd: "HG"));

					// Init Output
					category.CheckCatalogResult = null;

					// Run Checks
					actual = cTestChecks.TEST23(category, ref log);

					// Check Results
					Assert.AreEqual(string.Empty, actual);
					Assert.AreEqual(false, log);
					Assert.AreEqual("B", category.CheckCatalogResult, "Result");
				}
			}

			// Result C
			{
				string[] testTestTypeList = { "7DAY", "CYCLE" };
				// Init Input
				foreach (string testTestTypeCode in testTestTypeList)
				{
					QaParameters.CurrentTest = new VwQaTestSummaryRow(testTypeCd: testTestTypeCode, componentId: "GOOD", injectionProtocolCd: "NOTNULL");
					QaParameters.ComponentRecords = new CheckDataView<VwComponentRow>
						(new VwComponentRow(componentId: "GOOD", componentTypeCd: "NOTHG"));

					// Init Output
					category.CheckCatalogResult = null;

					// Run Checks
					actual = cTestChecks.TEST23(category, ref log);

					// Check Results
					Assert.AreEqual(string.Empty, actual);
					Assert.AreEqual(false, log);
					Assert.AreEqual("C", category.CheckCatalogResult, "Result");
				}
			}

			// Result D
			{
				// Init Input
					QaParameters.CurrentTest = new VwQaTestSummaryRow(testTypeCd: "BAD", componentId: "GOOD", injectionProtocolCd: "NOTNULL");
					QaParameters.ComponentRecords = new CheckDataView<VwComponentRow>
						(new VwComponentRow(componentId: "GOOD", componentTypeCd: "HG"));

					// Init Output
					category.CheckCatalogResult = null;

					// Run Checks
					actual = cTestChecks.TEST23(category, ref log);

					// Check Results
					Assert.AreEqual(string.Empty, actual);
					Assert.AreEqual(false, log);
					Assert.AreEqual("D", category.CheckCatalogResult, "Result");
				}

			// Pass
			{
				string[] testTestTypeList = { "7DAY", "CYCLE" };
				string[] testInjectionList = { "HGE", "HGO" };

				// Init Input
				foreach (string testTestTypeCode in testTestTypeList)
				{
					foreach (string testInjectionCode in testInjectionList)
					{
						QaParameters.CurrentTest = new VwQaTestSummaryRow(testTypeCd: testTestTypeCode, componentId: "GOOD", injectionProtocolCd: testInjectionCode);
						QaParameters.ComponentRecords = new CheckDataView<VwComponentRow>
							(new VwComponentRow(componentId: "GOOD", componentTypeCd: "HG"));

						// Init Output
						category.CheckCatalogResult = null;

						// Run Checks
						actual = cTestChecks.TEST23(category, ref log);

						// Check Results
						Assert.AreEqual(string.Empty, actual);
						Assert.AreEqual(false, log);
						Assert.AreEqual(null, category.CheckCatalogResult, "Result");
					}
				}
			}

		}
		#endregion

		//#region TEST-[N]

		///// <summary>
		/////A test for TEST[N]
		/////</summary>()
		//[TestMethod()]
		//public void TESTN()
		//{
		//    cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

		//    QaParameters.Init(category.Process);
		//    QaParameters.Category = category;

		//    // Variables
		//    bool log = false;
		//    string actual;

		//    // Init Input
		//    QaParameters.CurrentTest = new VwQaTestSummaryRow();

		//    // Init Output
		//    category.CheckCatalogResult = null;

		//    // Run Checks
		//    actual = cTestChecks.TEST[N](category, ref log);

		//    // Check Results
		//    Assert.AreEqual(string.Empty, actual);
		//    Assert.AreEqual(false, log);
		//    Assert.AreEqual(null, category.CheckCatalogResult, "Result");

		//}
		//#endregion

	}
}
