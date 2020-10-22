using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.CalibrationChecks;

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
	/// Summary description for cCalibrationChecksTest
	/// </summary>
	[TestClass]
	public class cCalibrationChecksTest
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

		#region SEVNDAY-24

		/// <summary>
		///A test for SEVNDAY24_HG
		///</summary>()
		[TestMethod()]
		public void SEVNDAY24_HG()
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
				QaParameters.Current7DayCalibrationTest = new VwQaTestSummaryRow(componentTypeCd: "NOTHG");
				QaParameters.CalibrationMaximumZeroReferenceValue = 21m;
				QaParameters.CalibrationMinimumZeroReferenceValue = 0m;
				QaParameters.TestSpanValue = 100m;
				QaParameters.TestTolerancesCrossCheckTable = new CheckDataView<TestTolerancesRow>
					(new TestTolerancesRow(testTypeCode: "7DAY", fieldDescription: "GasPercentOfSpan", tolerance: "0.1"));

				// Init Output
				category.CheckCatalogResult = null;

				// Run Checks
				actual = cCalibrationChecks.SEVNDAY24(category, ref log);

				// Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual("A", category.CheckCatalogResult, "Result");
			}

			//Pass - Do not check HG
			{
				// Init Input
				QaParameters.Current7DayCalibrationTest = new VwQaTestSummaryRow(componentTypeCd: "HG");
				QaParameters.CalibrationMaximumZeroReferenceValue = 21m;
				QaParameters.CalibrationMinimumZeroReferenceValue = 0m;
				QaParameters.TestSpanValue = 100m;
				QaParameters.TestTolerancesCrossCheckTable = new CheckDataView<TestTolerancesRow>
					(new TestTolerancesRow(testTypeCode: "7DAY", fieldDescription: "GasPercentOfSpan", tolerance: "0.1"));

				// Init Output
				category.CheckCatalogResult = null;

				// Run Checks
				actual = cCalibrationChecks.SEVNDAY24(category, ref log);

				// Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual(null, category.CheckCatalogResult, "Result");
			}

		}
		#endregion

	}
}
