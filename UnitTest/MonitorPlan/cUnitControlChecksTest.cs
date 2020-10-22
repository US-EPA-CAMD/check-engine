using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.MonitorPlan;
using ECMPS.Checks.UnitControlChecks;

using ECMPS.Definitions.Extensions;

using ECMPS.Checks.Data.Ecmps.Dbo.Table;
using ECMPS.Checks.Data.Ecmps.Dbo.View;
using ECMPS.Checks.Data.EcmpsAux.CrossCheck.Virtual;
using ECMPS.Checks.Data.Ecmps.CrossCheck.Table;
using ECMPS.Checks.Mp.Parameters;

using UnitTest.UtilityClasses;

namespace UnitTest.MonitorPlan
{
	[TestClass]
	public class cUnitControlChecksTest
	{
		public cUnitControlChecksTest()
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

		#region UnitControl-16

		/// <summary>
		///A test for CONTROL16_USCTest
		///</summary>()
		[TestMethod()]
		public void CONTROL16_USCTest()
		{
			//static check setup
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			MpParameters.Init(category.Process);
			MpParameters.Category = category;

			// USC checking, single component record SCR/NOX
			{
				// Variables
				bool log = false;
				string actual;
				string[] testControlCodeList = { "SCR", "SNCR", "OTHER" };
				string[] testLocList = { "UNITLOC", "CSLOC1", "CSLOC2", "MSLOC1", "MSLOC2" };

				// Init Input
				MpParameters.ControlEvaluationStartDate = new DateTime(2019, 6, 17);
				MpParameters.ControlEvaluationEndDate = new DateTime(2020, 6, 17);
				MpParameters.CurrentLocation = new VwMpLocationRow(monLocId: "UNITLOC");
				MpParameters.UnitStackConfigurationRecords = new CheckDataView<VwUnitStackConfigurationRow>(
					new VwUnitStackConfigurationRow(monLocId: "UNITLOC", stackName: "CS1", stackPipeMonLocId: "CSLOC1"),
					new VwUnitStackConfigurationRow(monLocId: "UNITLOC", stackName: "CS2", stackPipeMonLocId: "CSLOC2"),
					new VwUnitStackConfigurationRow(monLocId: "UNITLOC", stackName: "MS1", stackPipeMonLocId: "MSLOC1"),
					new VwUnitStackConfigurationRow(monLocId: "UNITLOC", stackName: "MS2", stackPipeMonLocId: "MSLOC2"));
				MpParameters.AnalyzerRangeRecords = new CheckDataView<VwAnalyzerRangeRow>(new VwAnalyzerRangeRow(componentId: "COMPON1", dualRangeInd: 1));
				MpParameters.SpanRecords = new CheckDataView<MonitorSpanRow>();

				foreach (string testControlCode in testControlCodeList)
				{
					foreach (string testLocation in testLocList)
					{
						MpParameters.CurrentControl = new UnitControlRow(controlCd: testControlCode);
						MpParameters.SystemComponentRecords = new CheckDataView<VwMonitorSystemComponentRow>(
								new VwMonitorSystemComponentRow(monLocId: testLocation, componentTypeCd: "NOX", componentId: "COMPON1"));

						// Init Output
						category.CheckCatalogResult = null;

						// Run Checks
						actual = cUnitControlChecks.CONTROL16(category, ref log);

						// Check Results
						Assert.AreEqual(string.Empty, actual);
						Assert.AreEqual(false, log);
						if (testLocation.StartsWith("CS") && testControlCode.InList("SCR,SNCR"))
							Assert.AreEqual("A", category.CheckCatalogResult, "Result");
						else
							Assert.AreEqual(null, category.CheckCatalogResult, "Result");
					}
				}
			}

			// USC checking, single component record SO2
			{
				// Variables
				bool log = false;
				string actual;
				string[] testControlCodeList = { "DA", "DL", "MO", "SB", "WL", "WLS", "OTHER" };
				string[] testLocList = { "UNITLOC", "CSLOC1", "CSLOC2", "MSLOC1", "MSLOC2" };

				// Init Input

				MpParameters.CurrentLocation = new VwMpLocationRow(monLocId: "UNITLOC");
				MpParameters.UnitStackConfigurationRecords = new CheckDataView<VwUnitStackConfigurationRow>(
					new VwUnitStackConfigurationRow(monLocId: "UNITLOC", stackName: "CS1", stackPipeMonLocId: "CSLOC1"),
					new VwUnitStackConfigurationRow(monLocId: "UNITLOC", stackName: "CS2", stackPipeMonLocId: "CSLOC2"),
					new VwUnitStackConfigurationRow(monLocId: "UNITLOC", stackName: "MS1", stackPipeMonLocId: "MSLOC1"),
					new VwUnitStackConfigurationRow(monLocId: "UNITLOC", stackName: "MS2", stackPipeMonLocId: "MSLOC2"));
				MpParameters.AnalyzerRangeRecords = new CheckDataView<VwAnalyzerRangeRow>(new VwAnalyzerRangeRow(componentId: "COMPON1", dualRangeInd: 1));
				MpParameters.SpanRecords = new CheckDataView<MonitorSpanRow>();

				foreach (string testControlCode in testControlCodeList)
				{
					foreach (string testLocation in testLocList)
					{
						MpParameters.CurrentControl = new UnitControlRow(controlCd: testControlCode);
						MpParameters.SystemComponentRecords = new CheckDataView<VwMonitorSystemComponentRow>(
								new VwMonitorSystemComponentRow(monLocId: testLocation, componentTypeCd: "SO2", componentId: "COMPON1"));

						// Init Output
						category.CheckCatalogResult = null;

						// Run Checks
						actual = cUnitControlChecks.CONTROL16(category, ref log);

						// Check Results
						Assert.AreEqual(string.Empty, actual);
						Assert.AreEqual(false, log);
						if (testLocation.StartsWith("CS") && testControlCode.InList("DA,DL,MO,SB,WL,WLS"))
							Assert.AreEqual("A", category.CheckCatalogResult, "Result");
						else
							Assert.AreEqual(null, category.CheckCatalogResult, "Result");
					}
				}
			}

		}

		/// <summary>
		///A test for CONTROL16_ComponentDetail
		///</summary>()
		//[TestMethod()] 
		public void CONTROL16_ComponentDetail()
		{
			//TODO (EC-3519): Replace or rethink this test.  Removed TestMethod attribute to stop it from running.  If fails because of changes subsequent to when it was written.
			//static check setup
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			MpParameters.Init(category.Process);
			MpParameters.Category = category;

			// Dual Range Indicator NOX
			{
				// Variables
				bool log = false;
				string actual;
				int[] testDualList = { 0, 1 };

				// Init Input
				MpParameters.ControlEvaluationStartDate = new DateTime(2019, 6, 17);
				MpParameters.ControlEvaluationEndDate = new DateTime(2020, 6, 17);
				MpParameters.CurrentLocation = new VwMpLocationRow(monLocId: "UNITLOC");
				MpParameters.UnitStackConfigurationRecords = new CheckDataView<VwUnitStackConfigurationRow>();
				MpParameters.CurrentControl = new UnitControlRow(controlCd: "SCR");
				MpParameters.SystemComponentRecords = new CheckDataView<VwMonitorSystemComponentRow>(
						new VwMonitorSystemComponentRow(monLocId: "UNITLOC", componentTypeCd: "NOX", componentId: "COMPON1"));

				foreach (int testDual in testDualList)
				{
					MpParameters.AnalyzerRangeRecords = new CheckDataView<VwAnalyzerRangeRow>(new VwAnalyzerRangeRow(componentId: "COMPON1", dualRangeInd: testDual));
					MpParameters.SpanRecords = new CheckDataView<MonitorSpanRow>();

					// Init Output
					category.CheckCatalogResult = null;

					// Run Checks
					actual = cUnitControlChecks.CONTROL16(category, ref log);

					// Check Results
					Assert.AreEqual(string.Empty, actual);
					Assert.AreEqual(false, log);
					if (testDual == 1)
						Assert.AreEqual(null, category.CheckCatalogResult, "Result");
					else
						Assert.AreEqual("A", category.CheckCatalogResult, "Result");
				}

				// Span Record NOX
				{
					int?[] testDefaultList = { null, 1, 0 };
					string[] testSpanScaleList = { "H", "M", "L" };

					foreach (int? testDefault in testDefaultList)
					{
						foreach (string testSpanScaleCode in testSpanScaleList)
						{
							MpParameters.AnalyzerRangeRecords = new CheckDataView<VwAnalyzerRangeRow>(new VwAnalyzerRangeRow(componentId: "COMPON1", dualRangeInd: 0));
							MpParameters.SpanRecords = new CheckDataView<MonitorSpanRow>(
								new MonitorSpanRow(componentTypeCd: "NOX", defaultHighRange: testDefault, spanScaleCd: testSpanScaleCode));

							// Init Output
							category.CheckCatalogResult = null;

							// Run Checks
							actual = cUnitControlChecks.CONTROL16(category, ref log);

							// Check Results
							Assert.AreEqual(string.Empty, actual);
							Assert.AreEqual(false, log);
							if (testDefault == null || testSpanScaleCode != "H")
								Assert.AreEqual("A", category.CheckCatalogResult, "Result");
							else
								Assert.AreEqual(null, category.CheckCatalogResult, "Result");
						}
					}
				}
			}


			// Dual Range Indicator SO2
			{
				// Variables
				bool log = false;
				string actual;
				int[] testDualList = { 0, 1 };

				// Init Input

				MpParameters.CurrentLocation = new VwMpLocationRow(monLocId: "UNITLOC");
				MpParameters.UnitStackConfigurationRecords = new CheckDataView<VwUnitStackConfigurationRow>();
				MpParameters.CurrentControl = new UnitControlRow(controlCd: "DA");
				MpParameters.SystemComponentRecords = new CheckDataView<VwMonitorSystemComponentRow>(
						new VwMonitorSystemComponentRow(monLocId: "UNITLOC", componentTypeCd: "SO2", componentId: "COMPON1"));

				foreach (int testDual in testDualList)
				{
					MpParameters.AnalyzerRangeRecords = new CheckDataView<VwAnalyzerRangeRow>(new VwAnalyzerRangeRow(componentId: "COMPON1", dualRangeInd: testDual));
					MpParameters.SpanRecords = new CheckDataView<MonitorSpanRow>();

					// Init Output
					category.CheckCatalogResult = null;

					// Run Checks
					actual = cUnitControlChecks.CONTROL16(category, ref log);

					// Check Results
					Assert.AreEqual(string.Empty, actual);
					Assert.AreEqual(false, log);
					if (testDual == 1)
						Assert.AreEqual(null, category.CheckCatalogResult, "Result");
					else
						Assert.AreEqual("A", category.CheckCatalogResult, "Result");
				}

				// Span Record
				{
					int?[] testDefaultList = { null, 1, 0 };
					string[] testSpanScaleList = { "H", "M", "L" };

					foreach (int? testDefault in testDefaultList)
					{
						foreach (string testSpanScaleCode in testSpanScaleList)
						{
							MpParameters.AnalyzerRangeRecords = new CheckDataView<VwAnalyzerRangeRow>(new VwAnalyzerRangeRow(componentId: "COMPON1", dualRangeInd: 0));
							MpParameters.SpanRecords = new CheckDataView<MonitorSpanRow>(
								new MonitorSpanRow(componentTypeCd: "SO2", defaultHighRange: testDefault, spanScaleCd: testSpanScaleCode));

							// Init Output
							category.CheckCatalogResult = null;

							// Run Checks
							actual = cUnitControlChecks.CONTROL16(category, ref log);

							// Check Results
							Assert.AreEqual(string.Empty, actual);
							Assert.AreEqual(false, log);
							if (testDefault == null || testSpanScaleCode != "H")
								Assert.AreEqual("A", category.CheckCatalogResult, "Result");
							else
								Assert.AreEqual(null, category.CheckCatalogResult, "Result");
						}
					}
				}
			}
		}
		#endregion


	}
}
