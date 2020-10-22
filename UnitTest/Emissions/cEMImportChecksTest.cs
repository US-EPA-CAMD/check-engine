using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks;
using ECMPS.Checks.ImportChecks;
using ECMPS.Checks.EmImport;
using ECMPS.Checks.EmImport.Parameters;

using ECMPS.Definitions.Extensions;


using ECMPS.Checks.Data.Ecmps.Dbo.Table;
using ECMPS.Checks.Data.Ecmps.Dbo.View;
using ECMPS.Checks.Data.EcmpsAux.CrossCheck.Virtual;
using ECMPS.Checks.Data.Ecmps.CrossCheck.Table;
using ECMPS.Checks.Em.Parameters;

using UnitTest.UtilityClasses;

namespace UnitTest.ImportChecks
{
	/// <summary>
	///This is a test class for cEMImportChecksTest and is intended
	///to contain all cEMImportChecksTest Unit Tests
	/// </summary>
	[TestClass]
	public class cEMImportChecksTest
	{
		public cEMImportChecksTest()
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


		#region IMPORT-38

		/// <summary>
		///A test for IMPORT38
		///</summary>()
		[TestMethod()]
		public void IMPORT38()
		{
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			EmImportParameters.Init(category.Process);
			EmImportParameters.Category = category;

			// Variables
			bool log = false;
			string actual;

			//Result A
			{
				//Init Input
				EmImportParameters.CurrentWorkspaceWeeklyTestSummary = new ECMPS.Checks.Data.EcmpsWs.Dbo.View.VwCheckEmWeeklytestsummaryRow(testTypeCd: "NOTHGSI1", wtsdPk: 1);
				EmImportParameters.WorkspaceWeeklySystemIntegrityRecords = new CheckDataView<ECMPS.Checks.Data.EcmpsWs.Dbo.Table.EmWeeklysystemintegrityRow>(
					new ECMPS.Checks.Data.EcmpsWs.Dbo.Table.EmWeeklysystemintegrityRow(wtsdFk: 1));

					// Init Output
					category.CheckCatalogResult = null;

					// Run Checks
					actual = cImportChecks.IMPORT38(category, ref log);

					// Check Results
					Assert.AreEqual(string.Empty, actual);
					Assert.AreEqual(false, log);
					Assert.AreEqual("A", category.CheckCatalogResult, "Result");
			}

			//PASS - HGSI
			{
				//Init Input
				EmImportParameters.CurrentWorkspaceWeeklyTestSummary = new ECMPS.Checks.Data.EcmpsWs.Dbo.View.VwCheckEmWeeklytestsummaryRow(testTypeCd: "HGSI1", wtsdPk: 1);
				EmImportParameters.WorkspaceWeeklySystemIntegrityRecords = new CheckDataView<ECMPS.Checks.Data.EcmpsWs.Dbo.Table.EmWeeklysystemintegrityRow>(
					new ECMPS.Checks.Data.EcmpsWs.Dbo.Table.EmWeeklysystemintegrityRow(wtsdFk: 1));

				// Init Output
				category.CheckCatalogResult = null;

				// Run Checks
				actual = cImportChecks.IMPORT38(category, ref log);

				// Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual(null, category.CheckCatalogResult, "Result");
			}

			//PASS - NOT HGSI, no System Integrity record
			{
				//Init Input
				EmImportParameters.CurrentWorkspaceWeeklyTestSummary = new ECMPS.Checks.Data.EcmpsWs.Dbo.View.VwCheckEmWeeklytestsummaryRow(testTypeCd: "NOTHGSI1", wtsdPk: 1);
				EmImportParameters.WorkspaceWeeklySystemIntegrityRecords = new CheckDataView<ECMPS.Checks.Data.EcmpsWs.Dbo.Table.EmWeeklysystemintegrityRow>();

				// Init Output
				category.CheckCatalogResult = null;

				// Run Checks
				actual = cImportChecks.IMPORT38(category, ref log);

				// Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual(null, category.CheckCatalogResult, "Result");
			}

		}
		#endregion
	}
}
