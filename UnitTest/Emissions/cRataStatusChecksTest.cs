using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.EmissionsChecks;
using ECMPS.Checks.EmissionsReport;
using ECMPS.Definitions.Extensions;

using ECMPS.Checks.Data.Ecmps.Dbo.Table;
using ECMPS.Checks.Data.Ecmps.Dbo.View;
using ECMPS.Checks.Data.EcmpsWs.Dbo.View;
using ECMPS.Checks.Data.EcmpsAux.CrossCheck.Virtual;
using ECMPS.Checks.Data.Ecmps.CrossCheck.Table;
using ECMPS.Checks.Em.Parameters;

using UnitTest.UtilityClasses;

namespace UnitTest.Emissions
{
	[TestClass()]
	public class cRataStatusChecksTest
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

		/// <summary>
		///A test for Q3 OOC - Conditional Period Expired
		///</summary>()
		[TestMethod()]
		public void RATSTAT5_Q3()
		{
			//instantiated checks setup
			cRATAStatusChecks target = new cRATAStatusChecks(new cEmissionsCheckParameters());
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			EmParameters.Init(category.Process);
			EmParameters.Category = category;

			// Variables
			bool log = false;
			string actual;

			// Init Input
			EmParameters.AnnualReportingRequirement = false;
			EmParameters.CurrentMhvParameter = "FLOW";
			EmParameters.EarliestLocationReportDate = null;
			EmParameters.EvaluateMultiLevelRata = true;
			EmParameters.HourlyOperatingDataRecordsForLocation = new CheckDataView<VwMpHrlyOpDataRow>();
			EmParameters.LocationProgramRecordsByHourLocation = new CheckDataView<VwMpLocationProgramRow>();
			EmParameters.MaxLevelCount = 0;
			EmParameters.OperatingSuppDataRecordsByLocation = new CheckDataView<VwMpOpSuppDataRow>();
			EmParameters.OverrideRataBaf = null;
			EmParameters.InvalidRataRecord = new VwQaSuppDataHourlyStatusRow();
			EmParameters.ReportingFrequencyByLocationQuarter = null;

			// CurrentDateHour in 3rd quarter
			{
				EmParameters.CurrentRataStatus = null; 
				EmParameters.PriorRataEventRecord = new VwQaCertEventRow(conditionalDataBeginDate: new DateTime(2014, 6, 30), conditionalDataBeginHour: 0, conditionalDataBeginDatehour: new DateTime(2014, 6, 30), monSysId: "SYS1", systemIdentifier: "1");
				EmParameters.RataTestRecordsByLocationForQaStatus = new CheckDataView<VwQaSuppDataHourlyStatusRow>();
				EmParameters.SubsequentRataRecord = null;
				EmParameters.CurrentDateHour = new DateTime(2014, 7, 1); //3rd quarter

				// Init Output
				category.CheckCatalogResult = null;

				// Run Checks
				actual = target.RATSTAT5(category, ref log);

				// Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual("OOC-Conditional Period Expired", EmParameters.CurrentRataStatus, "CurrentRataStatus");
				//Assert.AreEqual(1.0, EmParameters.OverrideRataBaf, "OverrideRataBaf"); //this doesn't get written back to the parameter until RATSTAT8
			}

			//October 30
			{
				EmParameters.CurrentRataStatus = null;
				EmParameters.PriorRataEventRecord = new VwQaCertEventRow(conditionalDataBeginDate: new DateTime(2014, 6, 30), conditionalDataBeginHour: 0, conditionalDataBeginDatehour: new DateTime(2014, 6, 30), monSysId: "SYS1", systemIdentifier: "1");
				EmParameters.RataTestRecordsByLocationForQaStatus = new CheckDataView<VwQaSuppDataHourlyStatusRow>(
						new VwQaSuppDataHourlyStatusRow(endDate: new DateTime(2014, 10, 31), endHour: 0, endMin: 0, monSysId: "SYS1", systemIdentifier: "1", testResultCd: "PASSED"));
				EmParameters.SubsequentRataRecord = null; //gets reset from RataTestRecords
				EmParameters.CurrentDateHour = new DateTime(2014, 7, 1);

				// Init Output
				category.CheckCatalogResult = null;

				// Run Checks
				actual = target.RATSTAT5(category, ref log);

				// Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual("OOC-Conditional Period Expired", EmParameters.CurrentRataStatus, "CurrentRataStatus");
				//Assert.AreEqual(1.0, EmParameters.OverrideRataBaf, "OverrideRataBaf"); //this doesn't get written back to the parameter until RATSTAT8
			}
		}


	}
}
