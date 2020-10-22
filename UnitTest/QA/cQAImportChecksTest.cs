using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks;
using ECMPS.Checks.ImportChecks;
using ECMPS.Checks.QaImport;
using ECMPS.Checks.QaImport.Parameters;

using ECMPS.Definitions.Extensions;

using ECMPS.Checks.Data.Ecmps.CrossCheck.Table;
using ECMPS.Checks.Data.Ecmps.Dbo.Table;
using ECMPS.Checks.Data.Ecmps.Dbo.View;
using ECMPS.Checks.Data.EcmpsAux.CrossCheck.Virtual;
using ECMPS.Checks.Data.EcmpsWs.Dbo.View;
using ECMPS.Checks.Data.EcmpsWs.Dbo.Table;
using ECMPS.Checks.Qa.Parameters;

using UnitTest.UtilityClasses;

namespace UnitTest.ImportChecks
{
	/// <summary>
	///This is a test class for cQAImportChecksTest and is intended
	///to contain all cQAImportChecksTest Unit Tests
	/// </summary>
	[TestClass]
	public class cQAImportChecksTest
	{
		public cQAImportChecksTest()
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
		
		#region IMPORT-16

		/// <summary>
		///A test for IMPORT16
		///</summary>()
		[TestMethod()]
		public void IMPORT16()
		{
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			QaImportParameters.Init(category.Process);
			QaImportParameters.Category = category;

			// Variables
			bool log = false;
			string actual;
			string[] testTestTypeCodeList = {"LINE", "HGLINE", "HGSI3"};


			//LinearitySummary Records
			{
				foreach (string testTestTypeCode in testTestTypeCodeList)
				{
					// Init Input
					QaImportParameters.CurrentWorkspaceTestSummary =
						new ECMPS.Checks.Data.EcmpsWs.Dbo.View.VwCheckQaTestsummaryRow(testTypeCd: testTestTypeCode, tsPk: 1);
					QaImportParameters.WorkspaceLinearitySummaryRecords = new CheckDataView<VwCheckQaLinearitysummaryRow>(
						new VwCheckQaLinearitysummaryRow(tsFk: 1, gasLevelCd: "H"));

					QaImportParameters.WorkspaceAiremissiontestingRecords = new CheckDataView<VwCheckQaAiremissiontestingRow>();
					QaImportParameters.WorkspaceAeCorrSummaryRecords = new CheckDataView<QaAecorrtestsummaryRow>();
					QaImportParameters.WorkspaceCalibrationInjectionRecords = new CheckDataView<QaCalibrationinjectionRow>();
					QaImportParameters.WorkspaceCycleTimeSummaryRecords = new CheckDataView<QaCycletimesummaryRow>();
					QaImportParameters.WorkspaceFlowToLoadCheckRecords = new CheckDataView<QaFlowtoloadcheckRow>();
					QaImportParameters.WorkspaceFlowToLoadReferenceRecords = new CheckDataView<QaFlowtoloadreferenceRow>();
					QaImportParameters.WorkspaceFuelFlowmeterAccuracyRecords = new  CheckDataView<QaFuelflowmeteraccuracyRow>();
					QaImportParameters.WorkspaceFuelflowToLoadBaselineRecords = new  CheckDataView<QaFuelflowtoloadbaselineRow>();
					QaImportParameters.WorkspaceFuelflowToLoadTestRecords = new CheckDataView<QaFuelflowtoloadtestRow>();
					QaImportParameters.WorkspaceHgSummaryRecords = new CheckDataView<QaHgtestsummaryRow>();
					QaImportParameters.WorkspaceOnlineOfflineCalibrationRecords = new CheckDataView<QaOnoffcalibrationRow>();
					QaImportParameters.WorkspaceProtocolgasRecords = new CheckDataView<VwCheckQaProtocolgasRow>();
					QaImportParameters.WorkspaceRataRecords = new CheckDataView<QaRataRow>();
					QaImportParameters.WorkspaceTestQualificationRecords = new CheckDataView<QaTestqualificationRow>();
					QaImportParameters.WorkspaceTransmitterTransducerRecords = new CheckDataView<QaTransaccuracyRow>();
					QaImportParameters.WorkspaceUnitDefaultTestRecords = new CheckDataView<QaUnitdefaulttestRow>();

					// Init Output
					QaImportParameters.InappropriateQaChildren = null;
					category.CheckCatalogResult = null;

					// Run Checks
					actual = cImportChecks.IMPORT16(category, ref log);

					// Check Results
					Assert.AreEqual(string.Empty, actual);
					Assert.AreEqual(false, log);
					if (testTestTypeCode.InList("LINE"))
					{
						Assert.AreEqual(null, category.CheckCatalogResult, "Result");
						Assert.IsNull(QaImportParameters.InappropriateQaChildren, "InappropriateQaChildren");
					}
					else
					{
						Assert.AreEqual("A", category.CheckCatalogResult, "Result");
						Assert.IsNotNull(QaImportParameters.InappropriateQaChildren, "InappropriateQaChildren");
					}
				}
			}

			//HgSummary Records
			{
				foreach (string testTestTypeCode in testTestTypeCodeList)
				{
					// Init Input
					QaImportParameters.CurrentWorkspaceTestSummary =
						new ECMPS.Checks.Data.EcmpsWs.Dbo.View.VwCheckQaTestsummaryRow(testTypeCd: testTestTypeCode, tsPk: 1);
					QaImportParameters.WorkspaceHgSummaryRecords = new CheckDataView<QaHgtestsummaryRow>(
						new QaHgtestsummaryRow(tsFk: 1, gasLevelCd: "H"));
					QaImportParameters.WorkspaceLinearitySummaryRecords = new  CheckDataView<VwCheckQaLinearitysummaryRow>();

					QaImportParameters.WorkspaceAiremissiontestingRecords = new CheckDataView<VwCheckQaAiremissiontestingRow>();
					QaImportParameters.WorkspaceAeCorrSummaryRecords = new CheckDataView<QaAecorrtestsummaryRow>();
					QaImportParameters.WorkspaceCalibrationInjectionRecords = new CheckDataView<QaCalibrationinjectionRow>();
					QaImportParameters.WorkspaceCycleTimeSummaryRecords = new CheckDataView<QaCycletimesummaryRow>();
					QaImportParameters.WorkspaceFlowToLoadCheckRecords = new CheckDataView<QaFlowtoloadcheckRow>();
					QaImportParameters.WorkspaceFlowToLoadReferenceRecords = new CheckDataView<QaFlowtoloadreferenceRow>();
					QaImportParameters.WorkspaceFuelFlowmeterAccuracyRecords = new CheckDataView<QaFuelflowmeteraccuracyRow>();
					QaImportParameters.WorkspaceFuelflowToLoadBaselineRecords = new CheckDataView<QaFuelflowtoloadbaselineRow>();
					QaImportParameters.WorkspaceFuelflowToLoadTestRecords = new CheckDataView<QaFuelflowtoloadtestRow>();
					QaImportParameters.WorkspaceOnlineOfflineCalibrationRecords = new CheckDataView<QaOnoffcalibrationRow>();
					QaImportParameters.WorkspaceProtocolgasRecords = new CheckDataView<VwCheckQaProtocolgasRow>();
					QaImportParameters.WorkspaceRataRecords = new CheckDataView<QaRataRow>();
					QaImportParameters.WorkspaceTestQualificationRecords = new CheckDataView<QaTestqualificationRow>();
					QaImportParameters.WorkspaceTransmitterTransducerRecords = new CheckDataView<QaTransaccuracyRow>();
					QaImportParameters.WorkspaceUnitDefaultTestRecords = new CheckDataView<QaUnitdefaulttestRow>();

					// Init Output
					QaImportParameters.InappropriateQaChildren = null;
					category.CheckCatalogResult = null;

					// Run Checks
					actual = cImportChecks.IMPORT16(category, ref log);

					// Check Results
					Assert.AreEqual(string.Empty, actual);
					Assert.AreEqual(false, log);
					if (testTestTypeCode.InList("HGLINE,HGSI3"))
					{
						Assert.AreEqual(null, category.CheckCatalogResult, "Result");
						Assert.IsNull(QaImportParameters.InappropriateQaChildren, "InappropriateQaChildren");
					}
					else
					{
						Assert.AreEqual("A", category.CheckCatalogResult, "Result");
						Assert.IsNotNull(QaImportParameters.InappropriateQaChildren, "InappropriateQaChildren");
					}
				}
			}
		}
		#endregion
		#region IMPORT-17

		/// <summary>
		///A test for IMPORT17
		///</summary>()
		[TestMethod()]
		public void IMPORT17()
		{
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			QaImportParameters.Init(category.Process);
			QaImportParameters.Category = category;

			// Variables
			bool log = false;
			string actual;
			string[] testTestTypeCodeList = {"OTHER", "FF2LBAS", "F2LREF", "APPE", "UNITDEF", "7DAY", "LINE", "CYCLE", "ONOFF", "HGLINE", "HGSI3",
												"RATA", "LEAK", "FF2LTST", "F2LCHK", "GFMCAL", "HGSI", "HGLME"};

			//TestDescription
			{
				foreach (string testTestTypeCode in testTestTypeCodeList)
				{
					// Init Input
					QaImportParameters.CurrentWorkspaceTestSummary =
						new ECMPS.Checks.Data.EcmpsWs.Dbo.View.VwCheckQaTestsummaryRow(testTypeCd: testTestTypeCode, testDescription: "NOTNULL");

					// Init Output
					QaImportParameters.ExtraneousTestSummaryFields = null;
					category.CheckCatalogResult = null;

					// Run Checks
					actual = cImportChecks.IMPORT17(category, ref log);

					// Check Results
					Assert.AreEqual(string.Empty, actual);
					Assert.AreEqual(false, log);
					if (testTestTypeCode.NotInList("OTHER"))
					{
						Assert.AreEqual("A", category.CheckCatalogResult, "Result");
						Assert.AreEqual("TestDescription", QaImportParameters.ExtraneousTestSummaryFields, "ExtraneousTestSummaryFields");
					}
					else
					{
						Assert.AreEqual(null, category.CheckCatalogResult, "Result");
						Assert.AreEqual(null, QaImportParameters.ExtraneousTestSummaryFields, "ExtraneousTestSummaryFields");
					}
				}
			}

			//TestResultCode
			{
				foreach (string testTestTypeCode in testTestTypeCodeList)
				{
					// Init Input
					QaImportParameters.CurrentWorkspaceTestSummary =
						new ECMPS.Checks.Data.EcmpsWs.Dbo.View.VwCheckQaTestsummaryRow(testTypeCd: testTestTypeCode, testResultCd: "NOTNULL");

					// Init Output
					QaImportParameters.ExtraneousTestSummaryFields = null;
					category.CheckCatalogResult = null;

					// Run Checks
					actual = cImportChecks.IMPORT17(category, ref log);

					// Check Results
					Assert.AreEqual(string.Empty, actual);
					Assert.AreEqual(false, log);
					if (testTestTypeCode.InList("FF2LBAS,F2LREF,APPE,UNITDEF"))
					{
						Assert.AreEqual("A", category.CheckCatalogResult, "Result");
						Assert.AreEqual("TestResultCode", QaImportParameters.ExtraneousTestSummaryFields, "ExtraneousTestSummaryFields");
					}
					else
					{
						Assert.AreEqual(null, category.CheckCatalogResult, "Result");
						Assert.AreEqual(null, QaImportParameters.ExtraneousTestSummaryFields, "ExtraneousTestSummaryFields");
					}
				}
			}

			//SpanScaleCode
			{
				foreach (string testTestTypeCode in testTestTypeCodeList)
				{
					// Init Input
					QaImportParameters.CurrentWorkspaceTestSummary =
						new ECMPS.Checks.Data.EcmpsWs.Dbo.View.VwCheckQaTestsummaryRow(testTypeCd: testTestTypeCode, spanScaleCd: "NOTNULL");

					// Init Output
					QaImportParameters.ExtraneousTestSummaryFields = null;
					category.CheckCatalogResult = null;

					// Run Checks
					actual = cImportChecks.IMPORT17(category, ref log);

					// Check Results
					Assert.AreEqual(string.Empty, actual);
					Assert.AreEqual(false, log);
					if (testTestTypeCode.NotInList("7DAY,LINE,CYCLE,ONOFF,HGLINE,HGSI3"))
					{
						Assert.AreEqual("A", category.CheckCatalogResult, "Result");
						Assert.AreEqual("SpanScaleCode", QaImportParameters.ExtraneousTestSummaryFields, "ExtraneousTestSummaryFields");
					}
					else
					{
						Assert.AreEqual(null, category.CheckCatalogResult, "Result");
						Assert.AreEqual(null, QaImportParameters.ExtraneousTestSummaryFields, "ExtraneousTestSummaryFields");
					}
				}
			}

			//TestReasonCode
			{
				foreach (string testTestTypeCode in testTestTypeCodeList)
				{
					// Init Input
					QaImportParameters.CurrentWorkspaceTestSummary =
						new ECMPS.Checks.Data.EcmpsWs.Dbo.View.VwCheckQaTestsummaryRow(testTypeCd: testTestTypeCode, testReasonCd: "NOTNULL");

					// Init Output
					QaImportParameters.ExtraneousTestSummaryFields = null;
					category.CheckCatalogResult = null;

					// Run Checks
					actual = cImportChecks.IMPORT17(category, ref log);

					// Check Results
					Assert.AreEqual(string.Empty, actual);
					Assert.AreEqual(false, log);
					if (testTestTypeCode.InList("FF2LBAS,F2LREF"))
					{
						Assert.AreEqual("A", category.CheckCatalogResult, "Result");
						Assert.AreEqual("TestReasonCode", QaImportParameters.ExtraneousTestSummaryFields, "ExtraneousTestSummaryFields");
					}
					else
					{
						Assert.AreEqual(null, category.CheckCatalogResult, "Result");
						Assert.AreEqual(null, QaImportParameters.ExtraneousTestSummaryFields, "ExtraneousTestSummaryFields");
					}
				}
			}

			//GracePeriodIndicator
			{
				foreach (string testTestTypeCode in testTestTypeCodeList)
				{
					// Init Input
					QaImportParameters.CurrentWorkspaceTestSummary =
						new ECMPS.Checks.Data.EcmpsWs.Dbo.View.VwCheckQaTestsummaryRow(testTypeCd: testTestTypeCode, gpInd: "1");

					// Init Output
					QaImportParameters.ExtraneousTestSummaryFields = null;
					category.CheckCatalogResult = null;

					// Run Checks
					actual = cImportChecks.IMPORT17(category, ref log);

					// Check Results
					Assert.AreEqual(string.Empty, actual);
					Assert.AreEqual(false, log);
					if (testTestTypeCode.NotInList("RATA,LINE,LEAK,HGLINE,HGSI3"))
					{
						Assert.AreEqual("A", category.CheckCatalogResult, "Result");
						Assert.AreEqual("GracePeriodIndicator", QaImportParameters.ExtraneousTestSummaryFields, "ExtraneousTestSummaryFields");
					}
					else
					{
						Assert.AreEqual(null, category.CheckCatalogResult, "Result");
						Assert.AreEqual(null, QaImportParameters.ExtraneousTestSummaryFields, "ExtraneousTestSummaryFields");
					}
				}
			}

			//TestBeginDate/Hour/Minute
			{
				foreach (string testTestTypeCode in testTestTypeCodeList)
				{
					// Init Input
					QaImportParameters.CurrentWorkspaceTestSummary =
						new ECMPS.Checks.Data.EcmpsWs.Dbo.View.VwCheckQaTestsummaryRow(testTypeCd: testTestTypeCode, beginDate: "BEGINDATE", beginHour: "BEGINHOUR", beginMin: "BEGINMIN");

					// Init Output
					QaImportParameters.ExtraneousTestSummaryFields = null;
					category.CheckCatalogResult = null;

					// Run Checks
					actual = cImportChecks.IMPORT17(category, ref log);

					// Check Results
					Assert.AreEqual(string.Empty, actual);
					Assert.AreEqual(false, log);
					if (testTestTypeCode.NotInList("RATA,7DAY,LINE,CYCLE,ONOFF,FF2LBAS,APPE,UNITDEF,HGSI3,HGLINE"))
					{
						Assert.AreEqual("A", category.CheckCatalogResult, "Result");
						Assert.AreEqual("BeginDate,BeginHour,BeginMinute", QaImportParameters.ExtraneousTestSummaryFields, "ExtraneousTestSummaryFields");
					}
					else if (testTestTypeCode.InList("ONOFF,FF2LBAS"))
					{
						Assert.AreEqual("A", category.CheckCatalogResult, "Result");
						Assert.AreEqual("BeginMinute", QaImportParameters.ExtraneousTestSummaryFields, "ExtraneousTestSummaryFields");
					}
					else
					{
						Assert.AreEqual(null, category.CheckCatalogResult, "Result");
						Assert.AreEqual(null, QaImportParameters.ExtraneousTestSummaryFields, "ExtraneousTestSummaryFields");
					}
				}
			}

			//TestEndDate/Hour/Minute
			{
				foreach (string testTestTypeCode in testTestTypeCodeList)
				{
					// Init Input
					QaImportParameters.CurrentWorkspaceTestSummary =
						new ECMPS.Checks.Data.EcmpsWs.Dbo.View.VwCheckQaTestsummaryRow(testTypeCd: testTestTypeCode, endDate: "NOTNULL", endHour: "1", endMin: "1");

					// Init Output
					QaImportParameters.ExtraneousTestSummaryFields = null;
					category.CheckCatalogResult = null;

					// Run Checks
					actual = cImportChecks.IMPORT17(category, ref log);

					// Check Results
					Assert.AreEqual(string.Empty, actual);
					Assert.AreEqual(false, log);
					if (testTestTypeCode.InList("FF2LTST,F2LCHK"))
					{
						Assert.AreEqual("A", category.CheckCatalogResult, "Result");
						Assert.AreEqual("EndDate,EndHour,EndMinute", QaImportParameters.ExtraneousTestSummaryFields, "ExtraneousTestSummaryFields");
					}
					else if (testTestTypeCode.InList("ONOFF,FF2LBAS"))
					{
						Assert.AreEqual("A", category.CheckCatalogResult, "Result");
						Assert.AreEqual("EndMinute", QaImportParameters.ExtraneousTestSummaryFields, "ExtraneousTestSummaryFields");
					}
					else
					{
						Assert.AreEqual(null, category.CheckCatalogResult, "Result");
						Assert.AreEqual(null, QaImportParameters.ExtraneousTestSummaryFields, "ExtraneousTestSummaryFields");
					}
				}
			}

			//Year/Quarter
			{
				foreach (string testTestTypeCode in testTestTypeCodeList)
				{
					// Init Input
					QaImportParameters.CurrentWorkspaceTestSummary =
						new ECMPS.Checks.Data.EcmpsWs.Dbo.View.VwCheckQaTestsummaryRow(testTypeCd: testTestTypeCode, calendarYear: "NOTNULL", quarter: "NOTNULL");

					// Init Output
					QaImportParameters.ExtraneousTestSummaryFields = null;
					category.CheckCatalogResult = null;

					// Run Checks
					actual = cImportChecks.IMPORT17(category, ref log);

					// Check Results
					Assert.AreEqual(string.Empty, actual);
					Assert.AreEqual(false, log);
					if (testTestTypeCode.NotInList("FF2LTST,F2LCHK"))
					{
						Assert.AreEqual("A", category.CheckCatalogResult, "Result");
						Assert.AreEqual("Year,Quarter", QaImportParameters.ExtraneousTestSummaryFields, "ExtraneousTestSummaryFields");
					}
					else
					{
						Assert.AreEqual(null, category.CheckCatalogResult, "Result");
						Assert.AreEqual(null, QaImportParameters.ExtraneousTestSummaryFields, "ExtraneousTestSummaryFields");
					}
				}
			}
		}
		#endregion

		#region IMPORT-18

		/// <summary>
		///A test for IMPORT18
		///</summary>()
		[TestMethod()]
		public void IMPORT18()
		{
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			QaImportParameters.Init(category.Process);
			QaImportParameters.Category = category;

			// Variables
			bool log = false;
			string actual;

			//Result A - System ID not null
			{
				string[] testTestTypeCodeList = { "7DAY", "LINE", "CYCLE", "ONOFF", "FFACC", "FFACCTT", "HGSI3", "HGLINE" };

				foreach (string testTestTypeCode in testTestTypeCodeList)
				{
					// Init Input
					QaImportParameters.CurrentWorkspaceTestSummary =
						new ECMPS.Checks.Data.EcmpsWs.Dbo.View.VwCheckQaTestsummaryRow(testTypeCd: testTestTypeCode, systemIdentifier: "NOTNULL", componentIdentifier: "NOTNULL");
					QaImportParameters.ProductionComponentRecords = new CheckDataView<ECMPS.Checks.Data.Ecmps.CheckImp.Function.Components>();
					QaImportParameters.ProductionMonitorMethodRecords = new CheckDataView<ECMPS.Checks.Data.Ecmps.CheckImp.Function.Methods>();
					QaImportParameters.ProductionMonitorSystemRecords = new CheckDataView<ECMPS.Checks.Data.Ecmps.CheckImp.Function.Systems>();

					// Init Output
					category.CheckCatalogResult = null;

					// Run Checks
					actual = cImportChecks.IMPORT18(category, ref log);

					// Check Results
					Assert.AreEqual(string.Empty, actual);
					Assert.AreEqual(false, log);
					Assert.AreEqual("A", category.CheckCatalogResult, "Result");
				}
			}
			//Result A - Component ID null
			{
				string[] testTestTypeCodeList = { "7DAY", "LINE", "CYCLE", "ONOFF", "FFACC", "FFACCTT", "HGSI3", "HGLINE" };

				foreach (string testTestTypeCode in testTestTypeCodeList)
				{
					// Init Input
					QaImportParameters.CurrentWorkspaceTestSummary =
						new ECMPS.Checks.Data.EcmpsWs.Dbo.View.VwCheckQaTestsummaryRow(testTypeCd: testTestTypeCode, systemIdentifier: null, componentIdentifier: null);
					QaImportParameters.ProductionComponentRecords = new CheckDataView<ECMPS.Checks.Data.Ecmps.CheckImp.Function.Components>();
					QaImportParameters.ProductionMonitorMethodRecords = new CheckDataView<ECMPS.Checks.Data.Ecmps.CheckImp.Function.Methods>();
					QaImportParameters.ProductionMonitorSystemRecords = new CheckDataView<ECMPS.Checks.Data.Ecmps.CheckImp.Function.Systems>();

					// Init Output
					category.CheckCatalogResult = null;

					// Run Checks
					actual = cImportChecks.IMPORT18(category, ref log);

					// Check Results
					Assert.AreEqual(string.Empty, actual);
					Assert.AreEqual(false, log);
					Assert.AreEqual("A", category.CheckCatalogResult, "Result");
				}
			}

			//Result B 
			{
				string[] testTestTypeCodeList = { "7DAY", "LINE", "CYCLE", "ONOFF", "FFACC", "FFACCTT", "HGSI3", "HGLINE" };
				string[] testComponentTypeCodeList = { "SO2", "CO2", "NOX", "O2", "FLOW", "HG", "OFFM", "GFFM", "BAD" };

				foreach (string testTestTypeCode in testTestTypeCodeList)
				{
					foreach (string testComponentTypeCode in testComponentTypeCodeList)
					{
						// Init Input
						QaImportParameters.CurrentWorkspaceTestSummary =
							new ECMPS.Checks.Data.EcmpsWs.Dbo.View.VwCheckQaTestsummaryRow(testTypeCd: testTestTypeCode, systemIdentifier: null, componentIdentifier: "COMPON1", locationId: "111", orisCode: 111);
						QaImportParameters.ProductionComponentRecords = new CheckDataView<ECMPS.Checks.Data.Ecmps.CheckImp.Function.Components>(new ECMPS.Checks.Data.Ecmps.CheckImp.Function.Components(componentTypeCd: testComponentTypeCode, componentIdentifier: "COMPON1", orisCode: 111, unitOrStack: "111"));
						QaImportParameters.ProductionMonitorMethodRecords = new CheckDataView<ECMPS.Checks.Data.Ecmps.CheckImp.Function.Methods>();
						QaImportParameters.ProductionMonitorSystemRecords = new CheckDataView<ECMPS.Checks.Data.Ecmps.CheckImp.Function.Systems>();

						// Init Output
						category.CheckCatalogResult = null;

						// Run Checks
						actual = cImportChecks.IMPORT18(category, ref log);

						// Check Results
						Assert.AreEqual(string.Empty, actual);
						Assert.AreEqual(false, log);
						if (testTestTypeCode.InList("7DAY,ONOFF") && testComponentTypeCode.NotInList("SO2,CO2,NOX,O2,FLOW,HG")
							|| testTestTypeCode.InList("CYCLE") && testComponentTypeCode.NotInList("SO2,CO2,NOX,O2,HG")
							|| testTestTypeCode.InList("LINE") && testComponentTypeCode.NotInList("SO2,CO2,NOX,O2")
							|| testTestTypeCode.InList("HGSI3,HGLINE") && testComponentTypeCode.NotInList("HG")
							|| testTestTypeCode.InList("FFACC,FFACCTT") && testComponentTypeCode.NotInList("OFFM,GFFM"))
						{
							Assert.AreEqual("B", category.CheckCatalogResult, "Result");
						}
						else
						{
							Assert.AreEqual(null, category.CheckCatalogResult, "Result");
						}

					}
				}
			}

			//Result C - System ID is null
			{
				string[] testTestTypeCodeList = { "RATA", "F2LCHK", "F2LREF", "FF2LBAS", "FF2LTST", "APPE" };

				foreach (string testTestTypeCode in testTestTypeCodeList)
				{
					// Init Input
					QaImportParameters.CurrentWorkspaceTestSummary =
						new ECMPS.Checks.Data.EcmpsWs.Dbo.View.VwCheckQaTestsummaryRow(testTypeCd: testTestTypeCode, systemIdentifier: null, componentIdentifier: null);
					QaImportParameters.ProductionComponentRecords = new CheckDataView<ECMPS.Checks.Data.Ecmps.CheckImp.Function.Components>();
					QaImportParameters.ProductionMonitorMethodRecords = new CheckDataView<ECMPS.Checks.Data.Ecmps.CheckImp.Function.Methods>();
					QaImportParameters.ProductionMonitorSystemRecords = new CheckDataView<ECMPS.Checks.Data.Ecmps.CheckImp.Function.Systems>();

					// Init Output
					category.CheckCatalogResult = null;

					// Run Checks
					actual = cImportChecks.IMPORT18(category, ref log);

					// Check Results
					Assert.AreEqual(string.Empty, actual);
					Assert.AreEqual(false, log);
					Assert.AreEqual("C", category.CheckCatalogResult, "Result");
				}
			}

			//Result C - Component ID not null
			{
				string[] testTestTypeCodeList = { "RATA", "F2LCHK", "F2LREF", "FF2LBAS", "FF2LTST", "APPE" };

				foreach (string testTestTypeCode in testTestTypeCodeList)
				{
					// Init Input
					QaImportParameters.CurrentWorkspaceTestSummary =
						new ECMPS.Checks.Data.EcmpsWs.Dbo.View.VwCheckQaTestsummaryRow(testTypeCd: testTestTypeCode, systemIdentifier: "NOTNULL", componentIdentifier: "NOTNULL");
					QaImportParameters.ProductionComponentRecords = new CheckDataView<ECMPS.Checks.Data.Ecmps.CheckImp.Function.Components>();
					QaImportParameters.ProductionMonitorMethodRecords = new CheckDataView<ECMPS.Checks.Data.Ecmps.CheckImp.Function.Methods>();
					QaImportParameters.ProductionMonitorSystemRecords = new CheckDataView<ECMPS.Checks.Data.Ecmps.CheckImp.Function.Systems>();

					// Init Output
					category.CheckCatalogResult = null;

					// Run Checks
					actual = cImportChecks.IMPORT18(category, ref log);

					// Check Results
					Assert.AreEqual(string.Empty, actual);
					Assert.AreEqual(false, log);
					Assert.AreEqual("C", category.CheckCatalogResult, "Result");
				}
			}

			//Result D
			{
				string[] testTestTypeCodeList = { "RATA", "F2LCHK", "F2LREF", "FF2LBAS", "FF2LTST", "APPE" };
				string[] testSystemTypeCodeList = { "SO2", "CO2", "NOX", "NOXC", "O2", "FLOW", "H2O", "H2OM", "NOXP", "SO2R", "HG", "HCL", "HF", "BAD" };

				foreach (string testTestTypeCode in testTestTypeCodeList)
				{
					foreach (string testSystemTypeCode in testSystemTypeCodeList)
					{
						// Init Input
						QaImportParameters.CurrentWorkspaceTestSummary =
							new ECMPS.Checks.Data.EcmpsWs.Dbo.View.VwCheckQaTestsummaryRow(testTypeCd: testTestTypeCode, systemIdentifier: "GOOD", componentIdentifier: null, locationId: "111", orisCode: 111);
						QaImportParameters.ProductionComponentRecords = new CheckDataView<ECMPS.Checks.Data.Ecmps.CheckImp.Function.Components>();
						QaImportParameters.ProductionMonitorMethodRecords = new CheckDataView<ECMPS.Checks.Data.Ecmps.CheckImp.Function.Methods>();
						QaImportParameters.ProductionMonitorSystemRecords = new CheckDataView<ECMPS.Checks.Data.Ecmps.CheckImp.Function.Systems>(new ECMPS.Checks.Data.Ecmps.CheckImp.Function.Systems(systemIdentifier: "GOOD", sysTypeCd: testSystemTypeCode, unitOrStack: "111", orisCode: 111));

						// Init Output
						category.CheckCatalogResult = null;

						// Run Checks
						actual = cImportChecks.IMPORT18(category, ref log);

						// Check Results
						Assert.AreEqual(string.Empty, actual);
						Assert.AreEqual(false, log);
						if (testTestTypeCode.InList("RATA") && testSystemTypeCode.NotInList("SO2,CO2,NOX,NOXC,O2,FLOW,H2O,H2OM,NOXP,SO2R,HG,HCL,HF")
							|| testTestTypeCode.InList("APPE") && testSystemTypeCode.NotInList("NOXE")
							|| testTestTypeCode.InList("F2LCHK,F2LREF") && testSystemTypeCode.NotInList("FLOW")
							|| testTestTypeCode.InList("FF2LBAS,FF2LTST") && testSystemTypeCode.NotInList("OILV,OILM,GAS,LTOL,LTGS"))
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

			//Result E - systemIdentifier not null
			{
				// Init Input
				QaImportParameters.CurrentWorkspaceTestSummary =
					new ECMPS.Checks.Data.EcmpsWs.Dbo.View.VwCheckQaTestsummaryRow(testTypeCd: "UNITDEF", systemIdentifier: "NOTNULL", componentIdentifier: null);
				QaImportParameters.ProductionComponentRecords = new CheckDataView<ECMPS.Checks.Data.Ecmps.CheckImp.Function.Components>();
				QaImportParameters.ProductionMonitorMethodRecords = new CheckDataView<ECMPS.Checks.Data.Ecmps.CheckImp.Function.Methods>();
				QaImportParameters.ProductionMonitorSystemRecords = new CheckDataView<ECMPS.Checks.Data.Ecmps.CheckImp.Function.Systems>();

				// Init Output
				category.CheckCatalogResult = null;

				// Run Checks
				actual = cImportChecks.IMPORT18(category, ref log);

				// Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual("E", category.CheckCatalogResult, "Result");
			}

			//Result E - ComponentIdentifier not null
			{
				// Init Input
				QaImportParameters.CurrentWorkspaceTestSummary =
					new ECMPS.Checks.Data.EcmpsWs.Dbo.View.VwCheckQaTestsummaryRow(testTypeCd: "UNITDEF", systemIdentifier: null, componentIdentifier: "NOTNULL");
				QaImportParameters.ProductionComponentRecords = new CheckDataView<ECMPS.Checks.Data.Ecmps.CheckImp.Function.Components>();
				QaImportParameters.ProductionMonitorMethodRecords = new CheckDataView<ECMPS.Checks.Data.Ecmps.CheckImp.Function.Methods>();
				QaImportParameters.ProductionMonitorSystemRecords = new CheckDataView<ECMPS.Checks.Data.Ecmps.CheckImp.Function.Systems>();

				// Init Output
				category.CheckCatalogResult = null;

				// Run Checks
				actual = cImportChecks.IMPORT18(category, ref log);

				// Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual("E", category.CheckCatalogResult, "Result");
			}

			//Result F 
			{
				// Init Input
				QaImportParameters.CurrentWorkspaceTestSummary =
					new ECMPS.Checks.Data.EcmpsWs.Dbo.View.VwCheckQaTestsummaryRow(testTypeCd: "UNITDEF", systemIdentifier: null, componentIdentifier: null, locationId: "111", orisCode: 111);
				QaImportParameters.ProductionComponentRecords = new CheckDataView<ECMPS.Checks.Data.Ecmps.CheckImp.Function.Components>();
				QaImportParameters.ProductionMonitorMethodRecords = new CheckDataView<ECMPS.Checks.Data.Ecmps.CheckImp.Function.Methods>();
				QaImportParameters.ProductionMonitorSystemRecords = new CheckDataView<ECMPS.Checks.Data.Ecmps.CheckImp.Function.Systems>();

				// Init Output
				category.CheckCatalogResult = null;

				// Run Checks
				actual = cImportChecks.IMPORT18(category, ref log);

				// Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual("F", category.CheckCatalogResult, "Result");
			}

			//not F (pass)
			{
				// Init Input
				QaImportParameters.CurrentWorkspaceTestSummary =
					new ECMPS.Checks.Data.EcmpsWs.Dbo.View.VwCheckQaTestsummaryRow(testTypeCd: "UNITDEF", systemIdentifier: null, componentIdentifier: null, locationId: "111", orisCode: 111);
				QaImportParameters.ProductionComponentRecords = new CheckDataView<ECMPS.Checks.Data.Ecmps.CheckImp.Function.Components>();
				QaImportParameters.ProductionMonitorMethodRecords = new CheckDataView<ECMPS.Checks.Data.Ecmps.CheckImp.Function.Methods>(new ECMPS.Checks.Data.Ecmps.CheckImp.Function.Methods(parameterCd: "NOXM", methodCd: "LME", unitOrStack: "111", orisCode: 111));
				QaImportParameters.ProductionMonitorSystemRecords = new CheckDataView<ECMPS.Checks.Data.Ecmps.CheckImp.Function.Systems>();

				// Init Output
				category.CheckCatalogResult = null;

				// Run Checks
				actual = cImportChecks.IMPORT18(category, ref log);

				// Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual(null, category.CheckCatalogResult, "Result");
			}

		}
		#endregion

		#region IMPORT-33

		/// <summary>
		///A test for IMPORT33
		///</summary>()
		[TestMethod()]
		public void IMPORT33()
		{
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			QaImportParameters.Init(category.Process);
			QaImportParameters.Category = category;

			// Variables
			bool log = false;
			string actual;

			string[] testUnitStackPipeIDList = { "CSstart", "MSstart", "CPstart", "MPstart" };
			string[] testTestTypeCodeList = {  "FFACC", "FFACCTT", "FF2LTST", "FF2LBAS", "APPE", "UNITDEF", "PEI", "PEMSACC", "RATA", "7DAY", "LINE", "CYCLE", "ONOFF", 
												"LEAK", "APPE", "HGSI3", "HGLINE", "OTHER" };

			foreach (string testUnitStackPipeID in testUnitStackPipeIDList)
			{
				foreach (string testTestTypeCode in testTestTypeCodeList)
				{
					// Init Input
					QaImportParameters.CurrentWorkspaceTestSummary =
						new ECMPS.Checks.Data.EcmpsWs.Dbo.View.VwCheckQaTestsummaryRow(testTypeCd: testTestTypeCode, locationId: testUnitStackPipeID);

					// Init Output
					QaImportParameters.TestLocationType = null;
					category.CheckCatalogResult = null;

					// Run Checks
					actual = cImportChecks.IMPORT33(category, ref log);

					// Check Results
					Assert.AreEqual(string.Empty, actual);
					Assert.AreEqual(false, log);
					if ((testUnitStackPipeID.StartsWith("CS") || testUnitStackPipeID.StartsWith("MS")) && testTestTypeCode.InList("FFACC,FFACCTT,FF2LTST,FF2LBAS,APPE,UNITDEF,PEI,PEMSACC"))
					{
						Assert.AreEqual("A", category.CheckCatalogResult, "Result");
						Assert.AreEqual("stack", QaImportParameters.TestLocationType, "Test Location Type");
					}
					else if (testUnitStackPipeID.StartsWith("CP") && testTestTypeCode.InList("RATA,LINE,7DAY,ONOFF,CYCLE,LEAK,APPE,UNITDEF,PEMSACC,HGLINE,HGSI3"))
					{
						Assert.AreEqual("A", category.CheckCatalogResult, "Result");
						Assert.AreEqual("common pipe", QaImportParameters.TestLocationType, "Test Location Type");
					}
					else if (testUnitStackPipeID.StartsWith("MP") && testTestTypeCode.InList("RATA,LINE,7DAY,ONOFF,CYCLE,LEAK,UNITDEF,PEMSACC,HGLINE,HGSI3"))
					{
						Assert.AreEqual("A", category.CheckCatalogResult, "Result");
						Assert.AreEqual("multiple pipe", QaImportParameters.TestLocationType, "Test Location Type");
					}
					else
					{
						Assert.AreEqual(null, category.CheckCatalogResult, "Result");
						Assert.AreEqual(null, QaImportParameters.TestLocationType, "Test Location Type");
					}

				}
			}
		}
		#endregion
	}
}
