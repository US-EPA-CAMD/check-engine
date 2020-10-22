using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.ComponentChecks;
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
	///This is a test class for cComponentChecksTest and is intended
	///to contain all cComponentChecksTest Unit Tests
	///</summary>
	[TestClass()]
	public class cComponentChecksTest
	{
		//public cComponentChecksTest()
		//{
		//    //
		//    // TODO: Add constructor logic here
		//    //
		//}

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

		#region COMPONENT-10

		/// <summary>
		///A test for COMPONENT10
		///</summary>()
		[TestMethod()]
		public void COMPONENT10()
		{
			//static check setup
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			MpParameters.Init(category.Process);
			MpParameters.Category = category;

			// Variables
			bool log = false;
			string actual;
			bool testTrue = false;
			string testSerialNumber = null;
			int passNum = 0;

			// Init Input
			MpParameters.CurrentComponent = new VwComponentRow();
			MpParameters.ComponentComponentTypeValid = false;

			string[] testComponentList = { "COMPONENTBAD", "TRAP", "BGFF", "BOFF", "TANK", "DAHS", "DL", "PLC", "FLC" };

			while (passNum++ <= 2)
			{
				if (passNum == 2)
				{
					MpParameters.ComponentComponentTypeValid = true;
				}
				if (passNum == 3)
				{
					testSerialNumber = "NOTNULL";
				}
				foreach (string testComponent in testComponentList)
				{
					MpParameters.CurrentComponent = new VwComponentRow(monLocId: "LOC1", serialNumber: testSerialNumber, componentTypeCd: testComponent);
					if (!testComponent.InList("BGFF,BOFF,TANK,DAHS,DL,PLC,FLC") & testSerialNumber == null & MpParameters.ComponentComponentTypeValid == true)
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
					actual = cComponentChecks.COMPON10(category, ref log);

					// Check Results
					Assert.AreEqual(string.Empty, actual);
					Assert.AreEqual(false, log);
					if (testTrue)
					{
						Assert.AreEqual("A", category.CheckCatalogResult, "Result");
					}
					else
					{
						Assert.AreEqual(null, category.CheckCatalogResult, "Result");
					}
				}
			}
		}
		#endregion

		#region COMPONENT-13

		/// <summary>
		///A test for COMPONENT13_HG
		///</summary>()
		[TestMethod()]
		public void COMPONENT13_HG()
		{
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			MpParameters.Init(category.Process);
			MpParameters.Category = category;

			// Variables
			bool log = false;
			string actual;
			bool testTrue = false;

			// Init Input
			MpParameters.ComponentBasisCodeValid = true;
			MpParameters.ComponentComponentTypeValid = true;
			MpParameters.ComponentTypeAndBasisToSampleAcquisitionMethodCrossCheckTable = new CheckDataView<ComponentTypeAndBasisToSampleAcquisitionMethodRow>
				(new ComponentTypeAndBasisToSampleAcquisitionMethodRow(sampleAcquisitionMethodCode: "ACQGOOD", genericComponentType: "CONC", basisCode: "BASISGOOD"));
			MpParameters.SampleAcquisitionMethodCodeLookupTable = new CheckDataView<AcquisitionMethodCodeRow>
				(new AcquisitionMethodCodeRow(acqCd: "ACQGOOD"));

			string[] testComponentList = { "COMPONENTBAD", "SO2", "NOX", "CO2", "O2", "PRB", "HG", "HCL", "HF", "PM" };

			foreach (string testComponent in testComponentList)
			{
				MpParameters.CurrentComponent = new VwComponentRow(acqCd: "ACQGOOD", monLocId: "LOC1", basisCd: "BASISGOOD", componentTypeCd: testComponent);
				if (testComponent.InList("SO2,NOX,CO2,O2,PRB,HG,HCL,HF,PM"))
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
				actual = cComponentChecks.COMPON13(category, ref log);

				// Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);

				if (testTrue)
				{
					Assert.AreEqual(null, category.CheckCatalogResult, "Result");
				}
				else
				{
					Assert.AreEqual("C", category.CheckCatalogResult, "Result");
				}

			}
		}
		#endregion

		#region COMPONENT-14

		/// <summary>
		///A test for COMPONENT14_ComponentTypeCode
		///</summary>()
		[TestMethod()]
		public void COMPONENT14_ComponentTypeCode()
		{
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			MpParameters.Init(category.Process);
			MpParameters.Category = category;

			// Variables
			bool log = false;
			string actual;
			string testResult = null;

			// Init Input
			MpParameters.ComponentComponentTypeValid = true;
			MpParameters.UsedIdentifierRecords = new CheckDataView<VwUsedIdentifierRow>();

			string[] testComponentList = { "COMPONENTBAD", "NOX", "SO2", "CO2", "O2", "FLOW", "GFM", "HG", "HCL", "HF", "STRAIN" };
			string[] testBasisList = { "W", "D", "B" };

			foreach (string testBasis in testBasisList)
			{
				foreach (string testComponent in testComponentList)
				{
					MpParameters.CurrentComponent = new VwComponentRow(basisCd: testBasis, componentTypeCd: testComponent);

					if (testComponent.InList("NOX,SO2,CO2,O2,FLOW,HG,HCL,HF,STRAIN"))
					{
						if ((testComponent == "FLOW" & testBasis != "W")
							|| (testComponent.InList("STRAIN") & testBasis != "D")
							|| (testComponent != "O2" & testBasis == "B"))
						{
							testResult = "B";
						}
						else
						{
							testResult = null;
						}
					}
					else
					{
						testResult = "D";
					}

					// Init Output
					category.CheckCatalogResult = null;

					// Run Checks
					actual = cComponentChecks.COMPON14(category, ref log);

					// Check Results
					Assert.AreEqual(string.Empty, actual);
					Assert.AreEqual(false, log);
					Assert.AreEqual(testResult, category.CheckCatalogResult, "Result");
				}
			}
		}
		#endregion

		#region COMPONENT-16

		/// <summary>
		///A test for COMPONENT16_ResultC
		///</summary>()
		[TestMethod()]
		public void COMPONENT16_ResultC()
		{
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			MpParameters.Init(category.Process);
			MpParameters.Category = category;

			// Variables
			bool log = false;
			string actual;
			string[] testAnalyzerRangeList = { "H", "NOTH" };
			string[] testComponentTypeList = { "HG", "NOTHG" };


			foreach (string testAnalyzerRangeCode in testAnalyzerRangeList)
			{
				foreach (string testComponentTypeCode in testComponentTypeList)
				{
					// Init Input
					MpParameters.CurrentAnalyzerRange = new VwAnalyzerRangeRow(analyzerRangeCd: testAnalyzerRangeCode);
					MpParameters.AnalyzerRangeCodeLookupTable = new CheckDataView<AnalyzerRangeCodeRow>
						(new AnalyzerRangeCodeRow(analyzerRangeCd: testAnalyzerRangeCode));
					MpParameters.CurrentComponent = new VwComponentRow(componentTypeCd: testComponentTypeCode);

					// Init Output
					category.CheckCatalogResult = null;

					// Run Checks
					actual = cComponentChecks.COMPON16(category, ref log);

					// Check Results
					Assert.AreEqual(string.Empty, actual);
					Assert.AreEqual(false, log);
					if (testComponentTypeCode == "HG" && testAnalyzerRangeCode != "H")
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

		#region COMPONENT-30

		/// <summary>
		///A test for COMPONENT30_ComponentTypeCode
		///</summary>()
		[TestMethod()]
		public void COMPONENT30_ComponentTypeCode()
		{
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			MpParameters.Init(category.Process);
			MpParameters.Category = category;

			// Variables
			bool log = false;
			string actual;
			string testResult = null;

			// Init Input
			MpParameters.ComponentAnalyzerRangeRecords = new CheckDataView<AnalyzerRangeRow>();
			MpParameters.ComponentDatesAndHoursConsistent = true;
			MpParameters.ComponentEvaluationBeginDate = DateTime.Today.AddDays(-1);
			MpParameters.ComponentEvaluationBeginHour = 0;
			MpParameters.ComponentEvaluationEndDate = DateTime.Today;
			MpParameters.ComponentEvaluationEndHour = 0;

			string[] testComponentList = { "SO2", "NOX", "CO2", "O2", "HG", "HCL", "HF" };

			foreach (string testComponent in testComponentList)
			{
				MpParameters.CurrentComponent = new VwComponentRow(componentTypeCd: testComponent);

				if (testComponent.InList("SO2,NOX,CO2,O2,HG"))
				{
					testResult = "A";
				}
				else
				{
					testResult = null;
				}

				// Init Output
				category.CheckCatalogResult = null;

				// Run Checks
				actual = cComponentChecks.COMPON30(category, ref log);

				// Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual(testResult, category.CheckCatalogResult, "Result");
			}
		}
		#endregion

		#region COMPONENT-44

		/// <summary>
		///A test for COMPONENT44_ReviseOptional
		///</summary>()
		[TestMethod()]
		public void COMPONENT44_ReviseOptional()
		{
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			MpParameters.Init(category.Process);
			MpParameters.Category = category;

			// Variables
			bool log = false;
			string actual;
			//            bool testTrue = false;

			// Init Input
			MpParameters.SystemComponentDatesAndHoursConsistent = true;
			MpParameters.SystemComponentEvaluationBeginDate = DateTime.Today.AddDays(-10);
			MpParameters.SystemComponentEvaluationBeginHour = 0;
			MpParameters.SystemComponentEvaluationEndDate = DateTime.Today;
			MpParameters.SystemComponentEvaluationEndHour = 23;
			MpParameters.SystemComponentRecordValid = true;
			MpParameters.SystemTypeCodeValid = true;
			MpParameters.CurrentSystem = new VwMonitorSystemRow(sysTypeCd: "NOTCO2");
			MpParameters.CurrentSystemComponent = new VwMpMonitorSystemComponentRow(sysTypeCd: "NOTCO2", componentTypeCd: "COMPONENTONLY");
      MpParameters.MethodRecords = new CheckDataView<VwMonitorMethodRow>
        (new VwMonitorMethodRow(parameterCd: "PARAM", methodCd: "METHOD", beginDate: DateTime.Today.AddDays(-10), beginHour: 0, endDate: DateTime.Today, endHour: 23));
			MpParameters.FormulaParameterAndComponentTypeAndBasisToFormulaCodeCrossCheckTable = new CheckDataView<FormulaParameterAndComponentTypeAndBasisToFormulaCodeRow>
				(new FormulaParameterAndComponentTypeAndBasisToFormulaCodeRow(parameterCode: "PARAM", componentTypeAndBasis: "COMPONENTONLY"));
			MpParameters.FormulaRecords = new CheckDataView<VwMonitorFormulaRow>(); //don't find

			// Optional valid value
			{
				MpParameters.SystemTypeToFormulaParameterCrossCheckTable = new CheckDataView<SystemTypeToFormulaParameterRow>
					(new SystemTypeToFormulaParameterRow(systemTypeCode: "NOTCO2", optional: "PARAM/METHOD", parameterCode: "PARAM"));

				// Init Output
				category.CheckCatalogResult = null;

				// Run Checks
				actual = cComponentChecks.COMPON44(category, ref log);

				// Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual("A", category.CheckCatalogResult, "Result");
			}

			// Optional not pair
			{
				MpParameters.SystemTypeToFormulaParameterCrossCheckTable = new CheckDataView<SystemTypeToFormulaParameterRow>
					(new SystemTypeToFormulaParameterRow(systemTypeCode: "NOTCO2", optional: "PARAM", parameterCode: "PARAM"));

				// Init Output
				category.CheckCatalogResult = null;

				// Run Checks
				actual = cComponentChecks.COMPON44(category, ref log);

				// Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual(null, category.CheckCatalogResult, "Result");
			}

		}
		#endregion

		#region COMPONENT-45
		/// <summary>
		///A test for COMPONENT45
		///</summary>()
		[TestMethod()]
		public void COMPONENT45()
		{
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			MpParameters.Init(category.Process);
			MpParameters.Category = category;

			// Variables
			bool log = false;
			string actual;

			// Init Input
			MpParameters.AnalyzerRangeDatesAndHoursConsistent = true;
			MpParameters.AnalyzerRangeEvaluationBeginDate = DateTime.Today.AddDays(-10);
			MpParameters.AnalyzerRangeEvaluationBeginHour = 0;
			MpParameters.AnalyzerRangeEvaluationEndDate = DateTime.Today;
			MpParameters.AnalyzerRangeEvaluationEndHour = 23;
			MpParameters.CurrentAnalyzerRange = new VwAnalyzerRangeRow(analyzerRangeCd: "H");

			string[] testComponentList = { "COMPONENTBAD", "SO2", "NOX", "CO2", "O2", "HG", "HCL", "HF" };

			//Result A or skip
			{
				foreach (string testComponent in testComponentList)
				{
					MpParameters.CurrentComponent = new VwComponentRow(componentTypeCd: testComponent);
					MpParameters.SpanRecords = new CheckDataView<MonitorSpanRow>();

					// Init Output
					category.CheckCatalogResult = null;

					// Run Checks
					actual = cComponentChecks.COMPON45(category, ref log);

					// Check Results
					Assert.AreEqual(string.Empty, actual);
					Assert.AreEqual(false, log);
					if (testComponent.InList("HCL,HF"))
					{
						Assert.AreEqual(null, category.CheckCatalogResult, "Result");
					}
					else
					{
						Assert.AreEqual("A", category.CheckCatalogResult, "Result");
					}

				}
			}

			//Result B
			{
				MpParameters.CurrentComponent = new VwComponentRow(componentTypeCd: "COMPONENTGOOD");
				MpParameters.SpanRecords = new CheckDataView<MonitorSpanRow>(
					new MonitorSpanRow(componentTypeCd: "COMPONENTGOOD", spanScaleCd: "H", beginDate: DateTime.Today.AddDays(-10), beginHour: 1)
					);

				// Init Output
				category.CheckCatalogResult = null;

				// Run Checks
				actual = cComponentChecks.COMPON45(category, ref log);

				// Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual("B", category.CheckCatalogResult, "Result");
			}

			//Pass
			{
				MpParameters.CurrentComponent = new VwComponentRow(componentTypeCd: "COMPONENTGOOD");
				MpParameters.SpanRecords = new CheckDataView<MonitorSpanRow>(
					new MonitorSpanRow(componentTypeCd: "COMPONENTGOOD", spanScaleCd: "H", beginDate: DateTime.Today.AddDays(-10), beginHour: 0)
					);

				// Init Output
				category.CheckCatalogResult = null;

				// Run Checks
				actual = cComponentChecks.COMPON45(category, ref log);

				// Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual(null, category.CheckCatalogResult, "Result");
			}

		}
		#endregion

		#region COMPONENT-47

		/// <summary>
		///A test for COMPONENT47
		///</summary>()
		[TestMethod()]
		public void COMPONENT47()
		{
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			MpParameters.Init(category.Process);
			MpParameters.Category = category;

			// Variables
			bool log = false;
			string actual;
			string testResult = null;

			// Init Input
			MpParameters.ComponentDatesAndHoursConsistent = true;
			MpParameters.ComponentEvaluationBeginDate = DateTime.Today.AddDays(-10);
			MpParameters.ComponentEvaluationBeginHour = 0;
			MpParameters.ComponentEvaluationEndDate = DateTime.Today;
			MpParameters.ComponentEvaluationEndHour = 0;

			string[] testComponentList = { "COMPONENTBAD", "SO2", "NOX", "CO2", "O2", "HG", "HCL", "HF" };

			//Dates Overlap
			{
				foreach (string testComponent in testComponentList)
				{
					MpParameters.CurrentComponent = new VwComponentRow(componentTypeCd: testComponent);
					MpParameters.ComponentAnalyzerRangeRecords = new CheckDataView<AnalyzerRangeRow>
						(new AnalyzerRangeRow(beginDate: DateTime.Today.AddDays(-10), beginHour: 0, endDate: DateTime.Today.AddDays(-4)),
						(new AnalyzerRangeRow(beginDate: DateTime.Today.AddDays(-5), beginHour: 0, endDate: DateTime.Today))
						);

					if (testComponent.InList("SO2,NOX,CO2,O2,HG,HCL,HF"))
					{
						testResult = "A";
					}
					else
					{
						testResult = null;
					}
					// Init Output
					category.CheckCatalogResult = null;

					// Run Checks
					actual = cComponentChecks.COMPON47(category, ref log);

					// Check Results
					Assert.AreEqual(string.Empty, actual);
					Assert.AreEqual(false, log);
					Assert.AreEqual(testResult, category.CheckCatalogResult, "Result");
				}
			}

			//Dates do not Overlap - null regardless of ComponentTypeCode
			{
				foreach (string testComponent in testComponentList)
				{
					MpParameters.CurrentComponent = new VwComponentRow(componentTypeCd: testComponent);
					MpParameters.ComponentAnalyzerRangeRecords = new CheckDataView<AnalyzerRangeRow>
						(new AnalyzerRangeRow(beginDate: DateTime.Today.AddDays(-10), beginHour: 0, endDate: DateTime.Today.AddDays(-6)),
						(new AnalyzerRangeRow(beginDate: DateTime.Today.AddDays(-5), beginHour: 0, endDate: DateTime.Today))
						);

					// Init Output
					category.CheckCatalogResult = null;

					// Run Checks
					actual = cComponentChecks.COMPON47(category, ref log);

					// Check Results
					Assert.AreEqual(string.Empty, actual);
					Assert.AreEqual(false, log);
					Assert.AreEqual(null, category.CheckCatalogResult, "Result");
				}
			}

		}
		#endregion

		#region COMPONENT-37

		/// <summary>
		///A test for COMPONENT37_A
		///</summary>()
		[TestMethod()]
		public void COMPONENT37_A()
		{
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			MpParameters.Init(category.Process);
			MpParameters.Category = category;

			// Variables
			bool log = false;
			string actual;


			//result A
			{
				// Init Input
				MpParameters.CurrentComponent = new VwComponentRow(componentTypeCd: "HG");
				MpParameters.CurrentAnalyzerRange = new VwAnalyzerRangeRow(dualRangeInd: null);

				// Init Output
				category.CheckCatalogResult = null;

				// Run Checks
				actual = cComponentChecks.COMPON37(category, ref log);

				// Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual("A", category.CheckCatalogResult, "Result");
			}
		}

		/// <summary>
		///A test for COMPONENT37_C
		///</summary>()
		[TestMethod()]
		public void COMPONENT37_C()
		{
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			MpParameters.Init(category.Process);
			MpParameters.Category = category;

			// Variables
			bool log = false;
			string actual;
			//            bool testTrue = false;

			//result C
			{
				// Init Input
				MpParameters.CurrentComponent = new VwComponentRow(componentTypeCd: "HG");
				MpParameters.CurrentAnalyzerRange = new VwAnalyzerRangeRow(dualRangeInd: 1, componentTypeCd: "HG");

				// Init Output
				category.CheckCatalogResult = null;

				// Run Checks
				actual = cComponentChecks.COMPON37(category, ref log);

				// Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual("C", category.CheckCatalogResult, "Result");
			}

			//result not C
			{
				// Init Input
				MpParameters.CurrentComponent = new VwComponentRow(componentTypeCd: "HG");
				MpParameters.CurrentAnalyzerRange = new VwAnalyzerRangeRow(dualRangeInd: 0);

				// Init Output
				category.CheckCatalogResult = null;

				// Run Checks
				actual = cComponentChecks.COMPON37(category, ref log);

				// Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual(null, category.CheckCatalogResult, "Result");
			}
		}

		/// <summary>
		///A test for COMPONENT37_B
		///</summary>()
		[TestMethod()]
		public void COMPONENT37_B()
		{
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			MpParameters.Init(category.Process);
			MpParameters.Category = category;

			// Variables
			bool log = false;
			string actual;

			//result B
			{
				// Init Input
				MpParameters.CurrentComponent = new VwComponentRow(componentTypeCd: "NOTHG");
				MpParameters.CurrentAnalyzerRange = new VwAnalyzerRangeRow(dualRangeInd: 0, analyzerRangeCd: "A");

				// Init Output
				category.CheckCatalogResult = null;

				// Run Checks
				actual = cComponentChecks.COMPON37(category, ref log);

				// Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual("B", category.CheckCatalogResult, "Result");
			}

			//result not B: test 1
			{
				// Init Input
				MpParameters.CurrentComponent = new VwComponentRow(componentTypeCd: "NOTHG");
				MpParameters.CurrentAnalyzerRange = new VwAnalyzerRangeRow(dualRangeInd: 1, analyzerRangeCd: "A");

				// Init Output
				category.CheckCatalogResult = null;

				// Run Checks
				actual = cComponentChecks.COMPON37(category, ref log);

				// Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual(null, category.CheckCatalogResult, "Result");
			}

			//result not B: test 2
			{
				// Init Input
				MpParameters.CurrentComponent = new VwComponentRow(componentTypeCd: "NOTHG");
				MpParameters.CurrentAnalyzerRange = new VwAnalyzerRangeRow(dualRangeInd: 0, analyzerRangeCd: "NOTA");

				// Init Output
				category.CheckCatalogResult = null;

				// Run Checks
				actual = cComponentChecks.COMPON37(category, ref log);

				// Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual(null, category.CheckCatalogResult, "Result");
			}
		}

		#endregion

		#region COMPONENT-51

		/// <summary>
		///A test for COMPONENT51_HCL_HF
		///</summary>()
		[TestMethod()]
		public void COMPONENT51_HCL_HF()
		{
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			MpParameters.Init(category.Process);
			MpParameters.Category = category;

			// Variables
			bool log = false;
			string actual;
			string[] testComponentTypeList = { "GOOD", "HCL", "HF" };

			// Init Input
			MpParameters.CurrentAnalyzerRange = new VwAnalyzerRangeRow(analyzerRangeCd: "L");
			MpParameters.AnalyzerRangeDatesAndHoursConsistent = true;
			MpParameters.SpanRecords = new CheckDataView<MonitorSpanRow>();
			MpParameters.AnalyzerRangeEvaluationBeginDate = DateTime.Today.AddDays(-10);
			MpParameters.AnalyzerRangeEvaluationBeginHour = 0;
			MpParameters.AnalyzerRangeEvaluationEndDate = DateTime.Today;
			MpParameters.AnalyzerRangeEvaluationEndHour = 23;

			foreach (string testComponentTypeCode in testComponentTypeList)
			{
				MpParameters.CurrentComponent = new VwComponentRow(componentTypeCd: testComponentTypeCode);

				// Init Output
				category.CheckCatalogResult = null;

				// Run Checks
				actual = cComponentChecks.COMPON51(category, ref log);

				// Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				if(testComponentTypeCode.InList("HCL,HF"))
				{
				Assert.AreEqual(null, category.CheckCatalogResult, $"Result - Component Type: {testComponentTypeCode}");
				}
				else
				{
				Assert.AreEqual("A", category.CheckCatalogResult, $"Result - Component Type: {testComponentTypeCode}");
				}
			
			}
		}
		#endregion

		#region COMPONENT-81

		/// <summary>
		///A test for COMPONENT81_VALID
		///</summary>()
		[TestMethod()]
		public void COMPONENT81_VALID()
		{
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			MpParameters.Init(category.Process);
			MpParameters.Category = category;

			// Variables
			bool log = false;
			string actual;

			//ComponentType not valid
			{
				// Init Input
				MpParameters.CurrentComponent = new VwComponentRow();
				MpParameters.ComponentComponentTypeValid = false;

				// Init Output
				category.CheckCatalogResult = null;

				// Run Checks
				actual = cComponentChecks.COMPON81(category, ref log);

				// Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual(null, category.CheckCatalogResult, "Result");
			}
		}

		/// <summary>
		///A test for COMPONENT81_A
		///</summary>()
		[TestMethod()]
		public void COMPONENT81_A()
		{
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			MpParameters.Init(category.Process);
			MpParameters.Category = category;

			// Variables
			bool log = false;
			string actual;

			//Result A
			{
				// Init Input
				MpParameters.CurrentComponent = new VwComponentRow(componentTypeCd: "HG", hgConverterInd: null);
				MpParameters.ComponentComponentTypeValid = true;

				// Init Output
				category.CheckCatalogResult = null;

				// Run Checks
				actual = cComponentChecks.COMPON81(category, ref log);

				// Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual("A", category.CheckCatalogResult, "Result");
			}
		}

		/// <summary>
		///A test for COMPONENT81_B
		///</summary>()
		[TestMethod()]
		public void COMPONENT81_B()
		{
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			MpParameters.Init(category.Process);
			MpParameters.Category = category;

			// Variables
			bool log = false;
			string actual;

			//Result B
			{
				int[] testHgConverterIndList = { 0, 1, 2 };
				bool testTrue = false;

				foreach (int testHgConverterInd in testHgConverterIndList)
				{
					// Init Input
					MpParameters.CurrentComponent = new VwComponentRow(componentTypeCd: "HG", hgConverterInd: testHgConverterInd);
					MpParameters.ComponentComponentTypeValid = true;

					if (testHgConverterInd.InRange(0, 1))
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
					actual = cComponentChecks.COMPON81(category, ref log);

					if (testTrue)
					{
						// Check Results
						Assert.AreEqual(string.Empty, actual);
						Assert.AreEqual(false, log);
						Assert.AreEqual("B", category.CheckCatalogResult, "Result");
					}
					else
					{
						// Check Results
						Assert.AreEqual(string.Empty, actual);
						Assert.AreEqual(false, log);
						Assert.AreEqual(null, category.CheckCatalogResult, "Result");
					}
				}
			}
		}

		/// <summary>
		///A test for COMPONENT81_C
		///</summary>()
		[TestMethod()]
		public void COMPONENT81_C()
		{
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			MpParameters.Init(category.Process);
			MpParameters.Category = category;

			// Variables
			bool log = false;
			string actual;

			//Result C
			{
				// Init Input
				MpParameters.CurrentComponent = new VwComponentRow(componentTypeCd: "NOTHG", hgConverterInd: 0);
				MpParameters.ComponentComponentTypeValid = true;

				// Init Output
				category.CheckCatalogResult = null;

				// Run Checks
				actual = cComponentChecks.COMPON81(category, ref log);

				// Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual("C", category.CheckCatalogResult, "Result");
			}

			//Result Not C
			{
				// Init Input
				MpParameters.CurrentComponent = new VwComponentRow(componentTypeCd: "NOTHG", hgConverterInd: null);
				MpParameters.ComponentComponentTypeValid = true;

				// Init Output
				category.CheckCatalogResult = null;

				// Run Checks
				actual = cComponentChecks.COMPON81(category, ref log);

				// Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual(null, category.CheckCatalogResult, "Result");
			}

		}

		#endregion

		//        #region COMPONENT-[N]

		//        /// <summary>
		//        ///A test for COMPONENT[N]
		//        ///</summary>()
		//        [TestMethod()]
		//        public void COMPONENT[N]()
		//        {
		//            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

		//            MpParameters.Init(category.Process);
		//            MpParameters.Category = category;

		//            // Variables
		//            bool log = false;
		//            string actual;
		////            bool testTrue = false;

		//            // Init Input
		//            MpParameters.CurrentComponent = new VwComponentRow();

		//                // Init Output
		//                category.CheckCatalogResult = null;

		//                // Run Checks
		//                actual = cComponentChecks.COMPON[N](category, ref log);

		//                // Check Results
		//                Assert.AreEqual(string.Empty, actual);
		//                Assert.AreEqual(false, log);
		//                Assert.AreEqual(null, category.CheckCatalogResult, "Result");

		//            }
		//        #endregion

	}
}
