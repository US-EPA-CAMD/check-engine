using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.LinearityChecks;

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
	public class cLinearityChecksTest
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

		#region LINEARITY-2

		/// <summary>
		///A test for LINEARITY2_HG
		///</summary>()
		[TestMethod()]
		public void LINEARITY2_HG()
		{
			//static check setup
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			QaParameters.Init(category.Process);
			QaParameters.Category = category;

			// Variables
			bool log = false;
			string actual;
			string[] testTestTypeList = { "HGSI", "HGLINE", "HGSI3" };
			string[] testComponentTypeList = { "HG", "NOTHG" };
            int?[] testHgConverterInd = { null, 0, 1 };

            foreach (string testTestTypeCode in testTestTypeList)
                foreach (string testComponentTypeCode in testComponentTypeList)
                    foreach (int? hgConverterInd in testHgConverterInd)
                    {
                        // Init Input
                        QaParameters.CurrentLinearityTest = new VwQaTestSummaryLineRow(testTypeCd: testTestTypeCode, componentTypeCd: testComponentTypeCode, hgConverterInd: hgConverterInd, componentId: "COMPONENT1");
                        QaParameters.LinearityTestType = "";

                        // Init Output
                        category.CheckCatalogResult = null;
                        QaParameters.LinearityComponentValid = false;

                        // Run Checks
                        actual = cLinearityChecks.LINEAR2(category, ref log);

                        // Check Results
                        Assert.AreEqual(string.Empty, actual);
                        Assert.AreEqual(false, log);

                        if (testTestTypeCode == "HGLINE")
                        {
                            if (testComponentTypeCode == "HG")
                            {
                                Assert.AreEqual("Hg linearity check", QaParameters.LinearityTestType, "LinearityTestType");
                                Assert.AreEqual(true, QaParameters.LinearityComponentValid, "LinearityComponentValid");
                                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                            }
                            else
                            {
                                Assert.AreEqual(string.Empty, QaParameters.LinearityTestType, "LinearityTestType");
                                Assert.AreEqual(false, QaParameters.LinearityComponentValid, "LinearityComponentValid");
                                Assert.AreEqual("B", category.CheckCatalogResult, "Result");
                            }
                        }
                        else if (testTestTypeCode == "HGSI3")
                        {
                            if (testComponentTypeCode == "HG")
                            {
                                Assert.AreEqual("three-point system integrity check", QaParameters.LinearityTestType, "LinearityTestType");
                                Assert.AreEqual(true, QaParameters.LinearityComponentValid, "LinearityComponentValid");
                            }
                            else
                            {
                                Assert.AreEqual(string.Empty, QaParameters.LinearityTestType, "LinearityTestType");
                                Assert.AreEqual(false, QaParameters.LinearityComponentValid, "LinearityComponentValid");
                                Assert.AreEqual("C", category.CheckCatalogResult, "Result");
                            }
                        }
                        else
                        {
                            Assert.AreEqual(string.Empty, QaParameters.LinearityTestType, "LinearityTestType");
                            Assert.AreEqual(false, QaParameters.LinearityComponentValid, "LinearityComponentValid");
                            Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                        }
                    }
		}
		#endregion

		#region LINEARITY-22

		/// <summary>
		///A test for LINEARITY22_TestTypeCode
		///</summary>()
		[TestMethod()]
		public void LINEARITY22_TestTypeCode()
		{
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			QaParameters.Init(category.Process);
			QaParameters.Category = category;

			// Variables
			bool log = false;
			string actual;

			// Init Input
			QaParameters.CurrentLinearitySummary = new ECMPS.Checks.Data.Ecmps.CheckQa.Function.LinearitySummary(testTypeCd: "TESTGOOD");
			QaParameters.LinearityInjectionReferenceValueValid = true;
			QaParameters.LinearityReferenceValueConsistentWithSpan = "true";
			QaParameters.TestSpanValue = (decimal)1;
			QaParameters.TestTolerancesCrossCheckTable = new CheckDataView<TestTolerancesRow>
				(new TestTolerancesRow(testTypeCode: "TESTGOOD", fieldDescription: "GasPercentOfSpan", tolerance: "0.1"));

			//Result A (LOW)
			{
				QaParameters.CurrentLinearityInjection = new ECMPS.Checks.Data.Ecmps.CheckQa.Function.LinearityInjection(gasLevelCd: "LOW", refValue: (decimal)19.9);

				// Init Output
				category.CheckCatalogResult = null;

				// Run Checks
				actual = cLinearityChecks.LINEAR22(category, ref log);

				// Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual("A", category.CheckCatalogResult, "Result");
				Assert.AreEqual("CRITICAL", QaParameters.LinearityReferenceValueConsistentWithSpan, "LinearityReferenceValueConsistentWithSpan");
			}

			//Result C (MID)
			{
				QaParameters.CurrentLinearityInjection = new ECMPS.Checks.Data.Ecmps.CheckQa.Function.LinearityInjection(gasLevelCd: "MID", refValue: (decimal)49.9);

				// Init Output
				category.CheckCatalogResult = null;

				// Run Checks
				actual = cLinearityChecks.LINEAR22(category, ref log);

				// Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual("C", category.CheckCatalogResult, "Result");
				Assert.AreEqual("CRITICAL", QaParameters.LinearityReferenceValueConsistentWithSpan, "LinearityReferenceValueConsistentWithSpan");
			}

			//Result E (HIGH)
			{
				QaParameters.CurrentLinearityInjection = new ECMPS.Checks.Data.Ecmps.CheckQa.Function.LinearityInjection(gasLevelCd: "HIGH", refValue: (decimal)100.1);

				// Init Output
				category.CheckCatalogResult = null;

				// Run Checks
				actual = cLinearityChecks.LINEAR22(category, ref log);

				// Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual("E", category.CheckCatalogResult, "Result");
				Assert.AreEqual("CRITICAL", QaParameters.LinearityReferenceValueConsistentWithSpan, "LinearityReferenceValueConsistentWithSpan");
			}
		}
		#endregion

		#region LINEARITY-26

		/// <summary>
		///A test for LINEARITY26_HG
		///</summary>()
		[TestMethod()]
		public void LINEARITY26_HG()
		{
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			QaParameters.Init(category.Process);
			QaParameters.Category = category;

			// Variables
			bool log = false;
			string actual;

			// Init Input
			QaParameters.CalculateLinearityLevel = true;

			//HG Test 1 - PASSAPS
			{
				QaParameters.LinearityMeasuredValueTotal = (decimal)5.100;
				QaParameters.LinearityReferenceValueTotal = (decimal)7.100;
				QaParameters.LinearityTestResult = string.Empty;

				QaParameters.CurrentLinearitySummary = new ECMPS.Checks.Data.Ecmps.CheckQa.Function.LinearitySummary(componentTypeCd: "HG", apsInd: 1, percentError: (decimal)0.6, testTypeCd: "HGLINE");
				QaParameters.TestTolerancesCrossCheckTable = new CheckDataView<TestTolerancesRow>
					(new TestTolerancesRow(fieldDescription:"MeanDifferenceUGSCM", testTypeCode:"HGLINE", tolerance:"10"));

				decimal M = (decimal)QaParameters.LinearityMeasuredValueTotal/3;
				decimal R = (decimal)QaParameters.LinearityReferenceValueTotal / 3;
				decimal D = Math.Abs((decimal)QaParameters.LinearityMeasuredValueTotal / 3 - (decimal)QaParameters.LinearityReferenceValueTotal / 3);
				decimal PE = D / R;

				// Init Output
				category.CheckCatalogResult = null;

				// Run Checks
				actual = cLinearityChecks.LINEAR26(category, ref log);

				// Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual(null, category.CheckCatalogResult, "Result");
				Assert.AreEqual("PASSAPS", QaParameters.LinearityTestResult, "LinearityTestResult");
			}

			 //HG Test 2 - PASSED
			 //Impossible to get to 2nd HG block of code
			{
				QaParameters.LinearityMeasuredValueTotal = (decimal)4.261;
				QaParameters.LinearityReferenceValueTotal = (decimal)7.100;
				QaParameters.LinearityTestResult = string.Empty;

				QaParameters.CurrentLinearitySummary = new ECMPS.Checks.Data.Ecmps.CheckQa.Function.LinearitySummary(componentTypeCd: "HG", apsInd: 0, percentError: (decimal)0.6, testTypeCd: "HGLINE");
				QaParameters.TestTolerancesCrossCheckTable = new CheckDataView<TestTolerancesRow>
					(new TestTolerancesRow(fieldDescription: "PercentError", testTypeCode: "HGLINE", tolerance: "10"));

			    // Init Output
			    category.CheckCatalogResult = null;

			    // Run Checks
			    actual = cLinearityChecks.LINEAR26(category, ref log);

			    // Check Results
			    Assert.AreEqual(string.Empty, actual);
			    Assert.AreEqual(false, log);
			    Assert.AreEqual(null, category.CheckCatalogResult, "Result");
			    Assert.AreEqual("PASSED", QaParameters.LinearityTestResult, "LinearityTestResult");
			}
		}
		#endregion


		#region LINEARITY-27

		/// <summary>
		///A test for LINEARITY27_ResultA
		///</summary>()
		[TestMethod()]
		public void LINEARITY27_ResultA()
		{
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			QaParameters.Init(category.Process);
			QaParameters.Category = category;

			// Variables
			bool log = false;
			string actual;

			// Init Input
			QaParameters.CalculateLinearityLevel = true;
			QaParameters.LinearitySummaryApsIndicator = 1;
			QaParameters.LinearitySummaryMeanDifference = (decimal)0.1;
			QaParameters.LinearitySummaryPercentError = (decimal)0.1;
			QaParameters.LinearitySummaryPercentErrorValid = true;
			QaParameters.TestTolerancesCrossCheckTable = new CheckDataView<TestTolerancesRow>();
			QaParameters.CurrentLinearitySummary = new ECMPS.Checks.Data.Ecmps.CheckQa.Function.LinearitySummary(testTypeCd: "TESTGOOD", apsInd: 0);

			// Init Output
			category.CheckCatalogResult = null;

			// Run Checks
			actual = cLinearityChecks.LINEAR27(category, ref log);

			// Check Results
			Assert.AreEqual(string.Empty, actual);
			Assert.AreEqual(false, log);
			Assert.AreEqual("A", category.CheckCatalogResult, "Result");
			Assert.AreEqual(null, QaParameters.LinearityIntermediateValues, "LinearityIntermediateValues,");
		}
		/// <summary>
		///A test for LINEARITY27_ResultB
		///</summary>()
		[TestMethod()]
		public void LINEARITY27_ResultB()
		{
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			QaParameters.Init(category.Process);
			QaParameters.Category = category;

			// Variables
			bool log = false;
			string actual;

			// Init Input
			QaParameters.CalculateLinearityLevel = true;
			QaParameters.LinearitySummaryApsIndicator = 0;
			QaParameters.LinearitySummaryMeanDifference = (decimal)0.1;
			QaParameters.LinearitySummaryPercentError = (decimal)0.1;
			QaParameters.LinearitySummaryPercentErrorValid = true;

			//Result B - test 1: APSInd=0
			{
				string[] testTestTypeList = { "LINE", "HGLINE", "HGSI3" };
				foreach (string testTestTypeCode in testTestTypeList)
				{
					QaParameters.TestTolerancesCrossCheckTable = new CheckDataView<TestTolerancesRow>
						(new TestTolerancesRow(testTypeCode: testTestTypeCode, fieldDescription: "PercentError", tolerance: "0.1"),
						new TestTolerancesRow(testTypeCode: testTestTypeCode, fieldDescription: "MeanDifferencePCT", tolerance: "0.1"));
					QaParameters.CurrentLinearitySummary = new ECMPS.Checks.Data.Ecmps.CheckQa.Function.LinearitySummary(testTypeCd: testTestTypeCode, apsInd: 0, percentError: (decimal)0.3);

					// Init Output
					category.CheckCatalogResult = null;

					// Run Checks
					actual = cLinearityChecks.LINEAR27(category, ref log);

					// Check Results
					Assert.AreEqual(string.Empty, actual);
					Assert.AreEqual(false, log);
					Assert.AreEqual("B", category.CheckCatalogResult, "Result");
					Assert.AreEqual(null, QaParameters.LinearityIntermediateValues, "LinearityIntermediateValues,");
				}
			}
			//Result B - test 2: SO2/NOX
			{
				string[] testComponentTypeList = { "SO2", "NOX" };
				foreach (string testComponentTypeCode in testComponentTypeList)
				{
					QaParameters.TestTolerancesCrossCheckTable = new CheckDataView<TestTolerancesRow>
						(new TestTolerancesRow(testTypeCode: "LINE", fieldDescription: "PercentError", tolerance: "0.1"),
						new TestTolerancesRow(testTypeCode: "LINE", fieldDescription: "MeanDifferencePCT", tolerance: "0.1"),
						new TestTolerancesRow(testTypeCode: "LINE", fieldDescription: "MeanDifferencePPM", tolerance: "0.1"));
					QaParameters.CurrentLinearitySummary = new ECMPS.Checks.Data.Ecmps.CheckQa.Function.LinearitySummary(testTypeCd: "LINE", componentTypeCd: testComponentTypeCode, apsInd: 1, percentError: (decimal)0.3);

					// Init Output
					category.CheckCatalogResult = null;

					// Run Checks
					actual = cLinearityChecks.LINEAR27(category, ref log);

					// Check Results
					Assert.AreEqual(string.Empty, actual);
					Assert.AreEqual(false, log);
					Assert.AreEqual("B", category.CheckCatalogResult, "Result");
					Assert.AreEqual(null, QaParameters.LinearityIntermediateValues, "LinearityIntermediateValues,");
				}
			}
			//Result B - test 3: HG
			{
				QaParameters.TestTolerancesCrossCheckTable = new CheckDataView<TestTolerancesRow>
					(new TestTolerancesRow(testTypeCode: "LINE", fieldDescription: "PercentError", tolerance: "0.1"),
					new TestTolerancesRow(testTypeCode: "LINE", fieldDescription: "MeanDifferencePCT", tolerance: "0.1"),
					new TestTolerancesRow(testTypeCode: "LINE", fieldDescription: "MeanDifferenceUGSCM", tolerance: "0.1"));
				QaParameters.CurrentLinearitySummary = new ECMPS.Checks.Data.Ecmps.CheckQa.Function.LinearitySummary(testTypeCd: "LINE", componentTypeCd: "HG", apsInd: 1, percentError: (decimal)0.3);

				// Init Output
				category.CheckCatalogResult = null;

				// Run Checks
				actual = cLinearityChecks.LINEAR27(category, ref log);

				// Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual("B", category.CheckCatalogResult, "Result");
				Assert.AreEqual(null, QaParameters.LinearityIntermediateValues, "LinearityIntermediateValues,");
			}

			//Result B - test 4: other ComponentTypeCode
			{
				QaParameters.TestTolerancesCrossCheckTable = new CheckDataView<TestTolerancesRow>
					(new TestTolerancesRow(testTypeCode: "LINE", fieldDescription: "PercentError", tolerance: "0.1"),
					new TestTolerancesRow(testTypeCode: "LINE", fieldDescription: "MeanDifferencePCT", tolerance: "0.1"));
				QaParameters.CurrentLinearitySummary = new ECMPS.Checks.Data.Ecmps.CheckQa.Function.LinearitySummary(testTypeCd: "LINE", componentTypeCd: "COMPONENTGOOD", apsInd: 1, percentError: (decimal)0.3);

				// Init Output
				category.CheckCatalogResult = null;

				// Run Checks
				actual = cLinearityChecks.LINEAR27(category, ref log);

				// Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual("B", category.CheckCatalogResult, "Result");
				Assert.AreEqual(null, QaParameters.LinearityIntermediateValues, "LinearityIntermediateValues,");
			}
		}
		/// <summary>
		///A test for LINEARITY27_ResultC
		///</summary>()
		[TestMethod()]
		public void LINEARITY27_ResultC()
		{
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			QaParameters.Init(category.Process);
			QaParameters.Category = category;

			// Variables
			bool log = false;
			string actual;

			// Init Input
			QaParameters.CalculateLinearityLevel = true;
			QaParameters.LinearitySummaryApsIndicator = 0;
			QaParameters.LinearitySummaryMeanDifference = (decimal)0.1;
			QaParameters.LinearitySummaryPercentError = (decimal)0.1;
			QaParameters.LinearitySummaryPercentErrorValid = true;


			//Result C - test 2: SO2/NOX
			{
				string[] testComponentTypeList = { "SO2", "NOX" };
				foreach (string testComponentTypeCode in testComponentTypeList)
				{
					QaParameters.TestTolerancesCrossCheckTable = new CheckDataView<TestTolerancesRow>
						(new TestTolerancesRow(testTypeCode: "LINE", fieldDescription: "PercentError", tolerance: "0.1"),
						new TestTolerancesRow(testTypeCode: "LINE", fieldDescription: "MeanDifferencePCT", tolerance: "0.1"),
						new TestTolerancesRow(testTypeCode: "LINE", fieldDescription: "MeanDifferencePPM", tolerance: "1.0"));
					QaParameters.CurrentLinearitySummary = new ECMPS.Checks.Data.Ecmps.CheckQa.Function.LinearitySummary(testTypeCd: "LINE", componentTypeCd: testComponentTypeCode, apsInd: 1, percentError: (decimal)0.3, meanRefValue: (decimal)1.2);

					// Init Output
					category.CheckCatalogResult = null;

					// Run Checks
					actual = cLinearityChecks.LINEAR27(category, ref log);

					// Check Results
					Assert.AreEqual(string.Empty, actual);
					Assert.AreEqual(false, log);
					Assert.AreEqual("C", category.CheckCatalogResult, "Result");
					Assert.AreEqual("MeanReferenceValue", QaParameters.LinearityIntermediateValues, "LinearityIntermediateValues,");
				}
			}
				//Result C - test 3: HG
				{
					QaParameters.TestTolerancesCrossCheckTable = new CheckDataView<TestTolerancesRow>
						(new TestTolerancesRow(testTypeCode: "LINE", fieldDescription: "PercentError", tolerance: "0.1"),
						new TestTolerancesRow(testTypeCode: "LINE", fieldDescription: "MeanDifferencePCT", tolerance: "0.1"),
						new TestTolerancesRow(testTypeCode: "LINE", fieldDescription: "MeanDifferenceUGSCM", tolerance: "1"));
					QaParameters.CurrentLinearitySummary = new ECMPS.Checks.Data.Ecmps.CheckQa.Function.LinearitySummary(testTypeCd: "LINE", componentTypeCd: "HG", apsInd: 1, percentError: (decimal)0.3, meanMeasuredValue: (decimal)1.2);

					// Init Output
					category.CheckCatalogResult = null;

					// Run Checks
					actual = cLinearityChecks.LINEAR27(category, ref log);

					// Check Results
					Assert.AreEqual(string.Empty, actual);
					Assert.AreEqual(false, log);
					Assert.AreEqual("C", category.CheckCatalogResult, "Result");
					Assert.AreEqual("MeanMeasuredValue", QaParameters.LinearityIntermediateValues, "LinearityIntermediateValues,");
				}

			}
		
		#endregion

		//#region LINEARITY-[N]

		///// <summary>
		/////A test for LINEARITY[N]
		/////</summary>()
		//[TestMethod()]
		//public void LINEARITY()
		//{
		//    cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

		//    QaParameters.Init(category.Process);
		//    QaParameters.Category = category;

		//    // Variables
		//    bool log = false;
		//    string actual;

		//    // Init Input
		//    QaParameters.CurrentLinearityTest = new VwQaTestSummaryLineRow();

		//    // Init Output
		//    category.CheckCatalogResult = null;

		//    // Run Checks
		//    actual = cLinearityChecks.LINEAR1(category, ref log);

		//    // Check Results
		//    Assert.AreEqual(string.Empty, actual);
		//    Assert.AreEqual(false, log);
		//    Assert.AreEqual(null, category.CheckCatalogResult, "Result");

		//}
		//#endregion

	}
}
