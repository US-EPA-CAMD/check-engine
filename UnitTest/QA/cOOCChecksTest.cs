using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.OOCChecks;

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
	[TestClass]
	public class cOOCChecksTest
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

		#region ONOFF-1

		/// <summary>
		///A test for ONOFF1
		///</summary>()
		[TestMethod()]
		public void ONOFF1()
		{
			//static check setup
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			QaParameters.Init(category.Process);
			QaParameters.Category = category;

			// Variables
			bool log = false;
			string actual;
			string[] testComponentTypeList = { "SO2", "NOX", "CO2", "O2", "FLOW", "HG" };

			//Result A
			// Init Input
			{
				QaParameters.CurrentOocTest = new VwQaTestSummaryOnoffRow(componentId: null, componentIdentifier: null);

				// Init Output
				category.CheckCatalogResult = null;

				// Run Checks
				actual = cOOCChecks.ONOFF1(category, ref log);

				// Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual("A", category.CheckCatalogResult, "Result");
				Assert.AreEqual(false, QaParameters.OocTestComponentTypeValid, "OocTestComponentTypeValid");
			}

			//Result B
			{
				foreach (string testComponentTypeCode in testComponentTypeList)
				{
					// Init Input
					QaParameters.CurrentOocTest = new VwQaTestSummaryOnoffRow(componentTypeCd: testComponentTypeCode, componentId: "COMPONID");

					// Init Output
					category.CheckCatalogResult = null;

					// Run Checks
					actual = cOOCChecks.ONOFF1(category, ref log);

					// Check Results
					Assert.AreEqual(string.Empty, actual);
					Assert.AreEqual(false, log);

					if (testComponentTypeCode.InList("SO2,NOX,CO2,O2,FLOW"))
					{
						Assert.AreEqual(null, category.CheckCatalogResult, "Result");
						Assert.AreEqual(true, QaParameters.OocTestComponentTypeValid, "OocTestComponentTypeValid");
					}
					else
					{
						Assert.AreEqual("B", category.CheckCatalogResult, "Result");
						Assert.AreEqual(false, QaParameters.OocTestComponentTypeValid, "OocTestComponentTypeValid");
					}
				}
			}
		}
		#endregion

		#region ONOFF-28

		/// <summary>
		///A test for ONOFF28_RemoveHG
		///</summary>()
		[TestMethod()]
		public void ONOFF28_RemoveHG()
		{
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			QaParameters.Init(category.Process);
			QaParameters.Category = category;

			// Variables
			bool log = false;
			string actual;
			string[] testComponentTypeList = { "CO2", "O2", "HG" };

			// Init Input
			QaParameters.TestSpanDetermined = true;
			QaParameters.TestSpanValue = 1;
			QaParameters.TestTolerancesCrossCheckTable = new CheckDataView<TestTolerancesRow>();
			QaParameters.UpscaleOocGasLevelValid = true;

			foreach (string testComponentTypeCode in testComponentTypeList)
			{
				QaParameters.OocTestCalcResult = "";
				QaParameters.CurrentOocTest = new VwQaTestSummaryOnoffRow(componentTypeCd: testComponentTypeCode, offlineUpscaleRefValue: 1, offlineUpscaleMeasuredValue: 1);
				// Init Output
				category.CheckCatalogResult = null;

				// Run Checks
				actual = cOOCChecks.ONOFF28(category, ref log);

				// Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				if (testComponentTypeCode.InList("CO2,O2"))
				{
					Assert.AreEqual(null, category.CheckCatalogResult, "Result");
					Assert.AreEqual("PASSED", QaParameters.OocTestCalcResult, "OocTestCalcResult");
				}
				else
				{
					Assert.AreEqual(null, category.CheckCatalogResult, "Result");
					Assert.AreEqual("", QaParameters.OocTestCalcResult, "OocTestCalcResult");
				}
			}
		}
		#endregion

		#region ONOFF-29

		/// <summary>
		///A test for ONOFF29_RemoveHG
		///</summary>()
		[TestMethod()]
		public void ONOFF29_RemoveHG()
		{
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			QaParameters.Init(category.Process);
			QaParameters.Category = category;

			// Variables
			bool log = false;
			string actual;
			string[] testComponentTypeList = { "CO2", "O2", "HG" };

			//Init Input
			QaParameters.TestSpanDetermined = true;
			QaParameters.TestSpanValue = 1;
			QaParameters.TestTolerancesCrossCheckTable = new CheckDataView<TestTolerancesRow>();

			foreach (string testComponentTypeCode in testComponentTypeList)
			{
				QaParameters.OocTestCalcResult = "";
				QaParameters.CurrentOocTest = new VwQaTestSummaryOnoffRow(componentTypeCd: testComponentTypeCode, offlineZeroRefValue: 1, offlineZeroMeasuredValue: 1);

				// Init Output
				category.CheckCatalogResult = null;

				// Run Checks
				actual = cOOCChecks.ONOFF29(category, ref log);

				// Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				if (testComponentTypeCode.InList("CO2,O2"))
				{
					Assert.AreEqual(null, category.CheckCatalogResult, "Result");
					Assert.AreEqual("PASSED", QaParameters.OocTestCalcResult, "OocTestCalcResult");
				}
				else
				{
					Assert.AreEqual(null, category.CheckCatalogResult, "Result");
					Assert.AreEqual("", QaParameters.OocTestCalcResult, "OocTestCalcResult");
				}
			}
		}
		#endregion

		#region ONOFF-30

		/// <summary>
		///A test for ONOFF30_RemoveHG
		///</summary>()
		[TestMethod()]
		public void ONOFF30_RemoveHG()
		{
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			QaParameters.Init(category.Process);
			QaParameters.Category = category;

			// Variables
			bool log = false;
			string actual;
			string[] testComponentTypeList = { "CO2", "O2", "HG" };

			//Init Input
			QaParameters.TestSpanDetermined = true;
			QaParameters.TestSpanValue = 1;
			QaParameters.TestTolerancesCrossCheckTable = new CheckDataView<TestTolerancesRow>();
			QaParameters.UpscaleOocGasLevelValid = true;

			foreach (string testComponentTypeCode in testComponentTypeList)
			{
				QaParameters.OocTestCalcResult = "";
				QaParameters.CurrentOocTest = new VwQaTestSummaryOnoffRow(componentTypeCd: testComponentTypeCode, onlineUpscaleRefValue: 1, onlineUpscaleMeasuredValue: 1);

				// Init Output
				category.CheckCatalogResult = null;

				// Run Checks
				actual = cOOCChecks.ONOFF30(category, ref log);

				// Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				if (testComponentTypeCode.InList("CO2,O2"))
				{
					Assert.AreEqual(null, category.CheckCatalogResult, "Result");
					Assert.AreEqual("PASSED", QaParameters.OocTestCalcResult, "OocTestCalcResult");
				}
				else
				{
					Assert.AreEqual(null, category.CheckCatalogResult, "Result");
					Assert.AreEqual("", QaParameters.OocTestCalcResult, "OocTestCalcResult");
				}
			}
		}
		#endregion

		#region ONOFF-31

		/// <summary>
		///A test for ONOFF31_RemoveHG
		///</summary>()
		[TestMethod()]
		public void ONOFF31_RemoveHG()
		{
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			QaParameters.Init(category.Process);
			QaParameters.Category = category;

			// Variables
			bool log = false;
			string actual;
			string[] testComponentTypeList = { "CO2", "O2", "HG" };

			//Init Input
			QaParameters.TestSpanDetermined = true;
			QaParameters.TestSpanValue = 1;
			QaParameters.TestTolerancesCrossCheckTable = new CheckDataView<TestTolerancesRow>();

			foreach (string testComponentTypeCode in testComponentTypeList)
			{
				QaParameters.OocTestCalcResult = "";
				QaParameters.CurrentOocTest = new VwQaTestSummaryOnoffRow(componentTypeCd: testComponentTypeCode, onlineZeroRefValue: 1, onlineZeroMeasuredValue: 1);

				// Init Output
				category.CheckCatalogResult = null;

				// Run Checks
				actual = cOOCChecks.ONOFF31(category, ref log);

				// Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				if (testComponentTypeCode.InList("CO2,O2"))
				{
					Assert.AreEqual(null, category.CheckCatalogResult, "Result");
					Assert.AreEqual("PASSED", QaParameters.OocTestCalcResult, "OocTestCalcResult");
				}
				else
				{
					Assert.AreEqual(null, category.CheckCatalogResult, "Result");
					Assert.AreEqual("", QaParameters.OocTestCalcResult, "OocTestCalcResult");
				}
			}
		}
		#endregion

		#region ONOFF-32

		/// <summary>
		///A test for ONOFF32_RemoveHG
		///</summary>()
		[TestMethod()]
		public void ONOFF32_RemoveHG()
		{
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			QaParameters.Init(category.Process);
			QaParameters.Category = category;

			// Variables
			bool log = false;
			string actual;
			string[] testComponentTypeList = { "FLOW", "HG" };

			//Init Input
			QaParameters.TestTolerancesCrossCheckTable = new CheckDataView<TestTolerancesRow>
				(new TestTolerancesRow(testTypeCode: "7DAY", fieldDescription: "DifferenceINH2O", tolerance: "0.1"),
				new TestTolerancesRow(testTypeCode: "7DAY", fieldDescription: "DifferenceUGSCM", tolerance: "0.1"),
				new TestTolerancesRow(testTypeCode: "7DAY", fieldDescription: "DifferencePPM", tolerance: "3"));
			QaParameters.OocOfflineUpscaleInjectionCalcApsIndicator = 1;
			QaParameters.OocOfflineUpscaleInjectionCalcResult = 1;

			foreach (string testComponentTypeCode in testComponentTypeList)
			{
				QaParameters.CurrentOocTest = new VwQaTestSummaryOnoffRow(componentTypeCd: testComponentTypeCode, offlineUpscaleApsInd: 1, offlineUpscaleCalError: 2, acqCd: "DP");

				// Init Output
				category.CheckCatalogResult = null;

				// Run Checks
				actual = cOOCChecks.ONOFF32(category, ref log);

				// Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				if (testComponentTypeCode.InList("FLOW"))
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

		#region ONOFF-33

		/// <summary>
		///A test for ONOFF33_RemoveHG
		///</summary>()
		[TestMethod()]
		public void ONOFF33_RemoveHG()
		{
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			QaParameters.Init(category.Process);
			QaParameters.Category = category;

			// Variables
			bool log = false;
			string actual;
			string[] testComponentTypeList = { "FLOW", "HG" };

			//Init Input
			QaParameters.TestTolerancesCrossCheckTable = new CheckDataView<TestTolerancesRow>
				(new TestTolerancesRow(testTypeCode: "7DAY", fieldDescription: "DifferenceINH2O", tolerance: "0.1"),
				new TestTolerancesRow(testTypeCode: "7DAY", fieldDescription: "DifferenceUGSCM", tolerance: "0.1"),
				new TestTolerancesRow(testTypeCode: "7DAY", fieldDescription: "DifferencePPM", tolerance: "3"));
			QaParameters.OocOfflineZeroInjectionCalcResult = 1;
			QaParameters.OocOfflineZeroInjectionCalcApsIndicator = 1;

			foreach (string testComponentTypeCode in testComponentTypeList)
			{
				QaParameters.CurrentOocTest = new VwQaTestSummaryOnoffRow(componentTypeCd: testComponentTypeCode, offlineZeroApsInd: 1, offlineZeroCalError: 2, acqCd: "DP");

				// Init Output
				category.CheckCatalogResult = null;

				// Run Checks
				actual = cOOCChecks.ONOFF33(category, ref log);

				// Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				if (testComponentTypeCode == "FLOW")
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

		#region ONOFF-34

		/// <summary>
		///A test for ONOFF34_RemoveHG
		///</summary>()
		[TestMethod()]
		public void ONOFF34_RemoveHG()
		{
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			QaParameters.Init(category.Process);
			QaParameters.Category = category;

			// Variables
			bool log = false;
			string actual;
			string[] testComponentTypeList = { "FLOW", "HG" };

			//Init Input
			QaParameters.TestTolerancesCrossCheckTable = new CheckDataView<TestTolerancesRow>
				(new TestTolerancesRow(testTypeCode: "7DAY", fieldDescription: "DifferenceINH2O", tolerance: "0.1"),
				new TestTolerancesRow(testTypeCode: "7DAY", fieldDescription: "DifferenceUGSCM", tolerance: "0.1"),
				new TestTolerancesRow(testTypeCode: "7DAY", fieldDescription: "DifferencePPM", tolerance: "3"));
			QaParameters.OocOnlineUpscaleInjectionCalcApsIndicator = 1;
			QaParameters.OocOnlineUpscaleInjectionCalcResult = 1;

			foreach (string testComponentTypeCode in testComponentTypeList)
			{
				QaParameters.CurrentOocTest = new VwQaTestSummaryOnoffRow(componentTypeCd: testComponentTypeCode, onlineUpscaleApsInd: 1, onlineUpscaleCalError: 2, acqCd: "DP");

				// Init Output
				category.CheckCatalogResult = null;

				// Run Checks
				actual = cOOCChecks.ONOFF34(category, ref log);

				// Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				if (testComponentTypeCode == "FLOW")
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

		#region ONOFF-35

		/// <summary>
		///A test for ONOFF35_RemoveHG
		///</summary>()
		[TestMethod()]
		public void ONOFF35_RemoveHG()
		{
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			QaParameters.Init(category.Process);
			QaParameters.Category = category;

			// Variables
			bool log = false;
			string actual;
			string[] testComponentTypeList = { "FLOW", "HG" };

			//Init Input
			QaParameters.TestTolerancesCrossCheckTable = new CheckDataView<TestTolerancesRow>
				(new TestTolerancesRow(testTypeCode: "7DAY", fieldDescription: "DifferenceINH2O", tolerance: "0.1"),
				new TestTolerancesRow(testTypeCode: "7DAY", fieldDescription: "DifferenceUGSCM", tolerance: "0.1"),
				new TestTolerancesRow(testTypeCode: "7DAY", fieldDescription: "DifferencePPM", tolerance: "3"));
			QaParameters.OocOnlineZeroInjectionCalcApsIndicator = 1;
			QaParameters.OocOnlineZeroInjectionCalcResult = 1;

			foreach (string testComponentTypeCode in testComponentTypeList)
			{
				QaParameters.CurrentOocTest = new VwQaTestSummaryOnoffRow(componentTypeCd: testComponentTypeCode, onlineZeroApsInd: 1, onlineZeroCalError: 2, acqCd: "DP");

				// Init Output
				category.CheckCatalogResult = null;

				// Run Checks
				actual = cOOCChecks.ONOFF35(category, ref log);

				// Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				if (testComponentTypeCode == "FLOW")
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

		#region ONOFF-45

		/// <summary>
		///A test for ONOFF45_RemoveHG
		///</summary>()
		[TestMethod()]
		public void ONOFF45_RemoveHG()
		{
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			QaParameters.Init(category.Process);
			QaParameters.Category = category;

			// Variables
			bool log = false;
			string actual;

			//Init Input
			QaParameters.SpanRecords = new CheckDataView<MonitorSpanRow>
				(new MonitorSpanRow(componentTypeCd: "HG", spanScaleCd: "H", spanValue: 1, beginDate: DateTime.Today.AddDays(-10), beginHour: 0));
			QaParameters.SystemComponentRecords = new CheckDataView<VwMonitorSystemComponentRow>
				(new VwMonitorSystemComponentRow(componentTypeCd: "HG", beginDate: DateTime.Today.AddDays(-10), beginHour: 0, endDate: DateTime.Today, endHour: 23));

			QaParameters.CurrentOocTest = new VwQaTestSummaryOnoffRow(componentTypeCd: "HG", upscaleGasLevelCd: "HIGH", spanScaleCd: "H",
				onlineUpscaleRefValue: 2, onlineUpscaleMeasuredValue: 1, onlineZeroRefValue: 1, onlineZeroMeasuredValue: 5,
				offlineUpscaleRefValue: 2, offlineUpscaleMeasuredValue: 1, offlineZeroRefValue: 1, offlineZeroMeasuredValue: 5,
				beginDate: DateTime.Today.AddDays(-11), beginHour: 0);

			// Init Output
			category.CheckCatalogResult = null;

			// Run Checks
			actual = cOOCChecks.ONOFF45(category, ref log);

			// Check Results
			Assert.AreEqual(string.Empty, actual);
			Assert.AreEqual(false, log);
			Assert.AreEqual(null, category.CheckCatalogResult, "Result");
			//Online tests
			Assert.AreEqual(null, QaParameters.OnlineZeroCalcResult, "OnlineZeroCalcResult");
			Assert.AreEqual(0, QaParameters.OnlineZeroCalcAps, "OnlineZeroCalcAps");
			Assert.AreEqual(null, QaParameters.OnlineUpscaleCalcResult, "OnlineUpscaleCalcResult");
			Assert.AreEqual(0, QaParameters.OnlineUpscaleCalcAps, "OnlineUpscaleCalcAps");
			//Offline tests
			Assert.AreEqual(null, QaParameters.OfflineZeroCalcResult, "OfflineZeroCalcResult");
			Assert.AreEqual(0, QaParameters.OfflineZeroCalcAps, "OfflineZeroCalcAps");
			Assert.AreEqual(null, QaParameters.OfflineUpscaleCalcResult, "OfflineUpscaleCalcResult");
			Assert.AreEqual(0, QaParameters.OfflineUpscaleCalcAps, "OfflineUpscaleCalcAps");

		}
		#endregion

		//#region ONOFF-[N]

		///// <summary>
		/////A test for ONOFF[N]
		/////</summary>()
		//[TestMethod()]
		//public void ONOFF()
		//{
		//    cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

		//    QaParameters.Init(category.Process);
		//    QaParameters.Category = category;

		//    // Variables
		//    bool log = false;
		//    string actual;

		//    // Init Input
		//    QaParameters.CurrentOocTest = new VwQaTestSummaryOnoffRow();

		//    // Init Output
		//    category.CheckCatalogResult = null;

		//    // Run Checks
		//    actual = cOOCChecks.ONOFF1(category, ref log);

		//    // Check Results
		//    Assert.AreEqual(string.Empty, actual);
		//    Assert.AreEqual(false, log);
		//    Assert.AreEqual(null, category.CheckCatalogResult, "Result");

		//}
		//#endregion



	}
}
