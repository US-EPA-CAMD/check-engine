using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Data.Ecmps.CrossCheck.Table;
using ECMPS.Checks.Data.Ecmps.Dbo.Table;
using ECMPS.Checks.Data.Ecmps.Dbo.View;
using ECMPS.Checks.Data.EcmpsAux.CrossCheck.Virtual;
using ECMPS.Checks.MonitorPlan;
using ECMPS.Checks.Mp.Parameters;
using ECMPS.Checks.QualificationLEEChecks;
using ECMPS.Definitions.Extensions;

using UnitTest.UtilityClasses;


namespace UnitTest.MonitorPlan
{
	/// <summary>
	///This is a test class for cQualificationLEEChecksTest and is intended
	///to contain all cQualificationLEEChecksTest Unit Tests
	/// </summary>
	[TestClass]
	public class cQualificationLEEChecksTest
	{
		public cQualificationLEEChecksTest()
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

		// <summary>
		//A test for QUALLEE1
		//</summary>()
		[TestMethod()]
		public void QUALLEE1()
		{
			//static check setup
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			MpParameters.Init(category.Process);
			MpParameters.Category = category;

			//     Variables
			bool log = false;
			string actual;
			string[] testQLEEList ={"QLEEGOOD", "QLEEBAD"};
			//    bool testTrue = false;

			//     Init Input
			MpParameters.QualificationLeeTestTypeCodeLookupTable = new CheckDataView<VwQualLeeTestTypeCdRow>(
				new VwQualLeeTestTypeCdRow(qualLeeTestTypeCd: "QLEEGOOD"));

			foreach (string testQLEECode in testQLEEList)
			{
				//     Init Input
				MpParameters.CurrentQualificationLee = new ECMPS.Checks.Data.Ecmps.CheckMp.Function.MonitorQualificationLEEParameter(qualLeeTestTypeCd: testQLEECode);

				//     Init Output
				category.CheckCatalogResult = null;

				//     Run Checks
				actual = cQualificationLEEChecks.QUALLEE1(category, ref log);

				//     Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				if (testQLEECode == "QLEEGOOD")
				{
					Assert.AreEqual(null, category.CheckCatalogResult, "Result");
				}
				else
				{
					Assert.AreEqual("A", category.CheckCatalogResult, "Result");
				}

			}
		}

		// <summary>
		//A test for QUALLEE2
		//</summary>()
		[TestMethod()]
		public void QUALLEE2()
		{
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			MpParameters.Init(category.Process);
			MpParameters.Category = category;

			//     Variables
			bool log = false;
			string actual;

			//Result A
			{
				//     Init Input
				MpParameters.CurrentQualificationLee = new ECMPS.Checks.Data.Ecmps.CheckMp.Function.MonitorQualificationLEEParameter
					(monQualId: "ID1", qualTestDate: DateTime.Today.AddDays(-3), qualLeeTestTypeCd: "INITIAL");
				MpParameters.QualificationleeRecords = new CheckDataView<ECMPS.Checks.Data.Ecmps.CheckMp.Function.MonitorQualificationLEEParameter>
				(new ECMPS.Checks.Data.Ecmps.CheckMp.Function.MonitorQualificationLEEParameter
					(monQualId:"ID1", qualTestDate: DateTime.Today.AddDays(-3)),
				new ECMPS.Checks.Data.Ecmps.CheckMp.Function.MonitorQualificationLEEParameter
					(monQualId:"ID1", qualTestDate: DateTime.Today.AddDays(-4)));
				

				//     Init Output
				category.CheckCatalogResult = null;

				//     Run Checks
				actual = cQualificationLEEChecks.QUALLEE2(category, ref log);

				//     Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual("A", category.CheckCatalogResult, "Result");
			}

			//Result B
			{
				//     Init Input
				MpParameters.CurrentQualificationLee = new ECMPS.Checks.Data.Ecmps.CheckMp.Function.MonitorQualificationLEEParameter
					(monQualId: "ID1", qualTestDate: DateTime.Today, qualLeeTestTypeCd: "RETEST");
				MpParameters.QualificationleeRecords = new CheckDataView<ECMPS.Checks.Data.Ecmps.CheckMp.Function.MonitorQualificationLEEParameter>
				(new ECMPS.Checks.Data.Ecmps.CheckMp.Function.MonitorQualificationLEEParameter
					(monQualId: "ID1", qualTestDate: DateTime.Today.AddYears(-3)),
					new ECMPS.Checks.Data.Ecmps.CheckMp.Function.MonitorQualificationLEEParameter
					(monQualId: "ID1", qualTestDate: DateTime.Today.AddYears(-1).AddDays(-1)));

				//     Init Output
				category.CheckCatalogResult = null;

				//     Run Checks
				actual = cQualificationLEEChecks.QUALLEE2(category, ref log);

				//     Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual("B", category.CheckCatalogResult, "Result");
			}

			//Result C
			{
				//     Init Input
				MpParameters.CurrentQualificationLee = new ECMPS.Checks.Data.Ecmps.CheckMp.Function.MonitorQualificationLEEParameter
					(monQualId: "ID1", qualTestDate: DateTime.Today, qualLeeTestTypeCd: "RETEST");
				MpParameters.QualificationleeRecords = new CheckDataView<ECMPS.Checks.Data.Ecmps.CheckMp.Function.MonitorQualificationLEEParameter>
				(new ECMPS.Checks.Data.Ecmps.CheckMp.Function.MonitorQualificationLEEParameter
					(monQualId: "ID1", qualTestDate: DateTime.Today.AddDays(1)));

				//     Init Output
				category.CheckCatalogResult = null;

				//     Run Checks
				actual = cQualificationLEEChecks.QUALLEE2(category, ref log);

				//     Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual("C", category.CheckCatalogResult, "Result");
			}

			//no errors Initial
			{
				//     Init Input
				MpParameters.CurrentQualificationLee = new ECMPS.Checks.Data.Ecmps.CheckMp.Function.MonitorQualificationLEEParameter
					(monQualId: "ID1", qualTestDate: DateTime.Today, qualLeeTestTypeCd: "INITIAL");
				MpParameters.QualificationleeRecords = new CheckDataView<ECMPS.Checks.Data.Ecmps.CheckMp.Function.MonitorQualificationLEEParameter>();

				//     Init Output
				category.CheckCatalogResult = null;

				//     Run Checks
				actual = cQualificationLEEChecks.QUALLEE2(category, ref log);

				//     Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual(null, category.CheckCatalogResult, "Result");
			}

			//no errors Retest
			{
				//     Init Input
				MpParameters.CurrentQualificationLee = new ECMPS.Checks.Data.Ecmps.CheckMp.Function.MonitorQualificationLEEParameter
					(monQualId: "ID1", qualTestDate: DateTime.Today, qualLeeTestTypeCd: "RETEST");
				MpParameters.QualificationleeRecords = new CheckDataView<ECMPS.Checks.Data.Ecmps.CheckMp.Function.MonitorQualificationLEEParameter>
				(new ECMPS.Checks.Data.Ecmps.CheckMp.Function.MonitorQualificationLEEParameter
					(monQualId: "ID1", qualTestDate: DateTime.Today.AddDays(-1)));

				//     Init Output
				category.CheckCatalogResult = null;

				//     Run Checks
				actual = cQualificationLEEChecks.QUALLEE2(category, ref log);

				//     Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual(null, category.CheckCatalogResult, "Result");
			}
		}

		// <summary>
		//A test for QUALLEE3_ResultA
		//</summary>()
		[TestMethod()]
		public void QUALLEE3_ResultA()
		{
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			MpParameters.Init(category.Process);
			MpParameters.Category = category;

			//     Variables
			bool log = false;
			string actual;
			//    bool testTrue = false;

				//     Init Input
				MpParameters.CurrentQualificationLee = new ECMPS.Checks.Data.Ecmps.CheckMp.Function.MonitorQualificationLEEParameter(qualLeeTestTypeCd:"QLEEGOOD");

				//     Init Output
				category.CheckCatalogResult = null;

				//     Run Checks
				actual = cQualificationLEEChecks.QUALLEE3(category, ref log);

				//     Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual("A", category.CheckCatalogResult, "Result");
		}

		// <summary>
		//A test for QUALLEE3_ResultB
		//</summary>()
		[TestMethod()]
		public void QUALLEE3_ResultB()
		{
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			MpParameters.Init(category.Process);
			MpParameters.Category = category;

			//     Variables
			bool log = false;
			string actual;
			//    bool testTrue = false;

			//ApplicableEmissions not null
			{
				//     Init Input
				MpParameters.CurrentQualificationLee = new ECMPS.Checks.Data.Ecmps.CheckMp.Function.MonitorQualificationLEEParameter(qualLeeTestTypeCd: "QLEEGOOD", potentialAnnualEmissions: 1, applicableEmissionStandard: 1);

				//     Init Output
				category.CheckCatalogResult = null;

				//     Run Checks
				actual = cQualificationLEEChecks.QUALLEE3(category, ref log);

				//     Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual("B", category.CheckCatalogResult, "Result");
			}

			//emissionStandardPct not null
			{
				//     Init Input
				MpParameters.CurrentQualificationLee = new ECMPS.Checks.Data.Ecmps.CheckMp.Function.MonitorQualificationLEEParameter(qualLeeTestTypeCd: "QLEEGOOD", potentialAnnualEmissions: 1, emissionStandardPct: 1);

				//     Init Output
				category.CheckCatalogResult = null;

				//     Run Checks
				actual = cQualificationLEEChecks.QUALLEE3(category, ref log);

				//     Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual("B", category.CheckCatalogResult, "Result");
			}

			//emissionStandardUom not null
			{
				//     Init Input
				MpParameters.CurrentQualificationLee = new ECMPS.Checks.Data.Ecmps.CheckMp.Function.MonitorQualificationLEEParameter(qualLeeTestTypeCd: "QLEEGOOD", potentialAnnualEmissions: 1, emissionStandardUom: "NOTNULL");

				//     Init Output
				category.CheckCatalogResult = null;

				//     Run Checks
				actual = cQualificationLEEChecks.QUALLEE3(category, ref log);

				//     Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual("B", category.CheckCatalogResult, "Result");
			}

			//combo
			{
				//     Init Input
				MpParameters.CurrentQualificationLee = new ECMPS.Checks.Data.Ecmps.CheckMp.Function.MonitorQualificationLEEParameter(qualLeeTestTypeCd: "QLEEGOOD", potentialAnnualEmissions: 1, emissionStandardPct:1, emissionStandardUom: "NOTNULL");

				//     Init Output
				category.CheckCatalogResult = null;

				//     Run Checks
				actual = cQualificationLEEChecks.QUALLEE3(category, ref log);

				//     Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual("B", category.CheckCatalogResult, "Result");
			}

			//standards all null
			{
				//     Init Input
				MpParameters.CurrentQualificationLee = new ECMPS.Checks.Data.Ecmps.CheckMp.Function.MonitorQualificationLEEParameter(qualLeeTestTypeCd: "QLEEGOOD", potentialAnnualEmissions: 1);

				//     Init Output
				category.CheckCatalogResult = null;

				//     Run Checks
				actual = cQualificationLEEChecks.QUALLEE3(category, ref log);

				//     Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual(null, category.CheckCatalogResult, "Result");
			}

		}

				// <summary>
		//A test for QUALLEE3_ResultC
		//</summary>()
		[TestMethod()]
		public void QUALLEE3_ResultC()
		{
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			MpParameters.Init(category.Process);
			MpParameters.Category = category;

			//     Variables
			bool log = false;
			string actual;
			//    bool testTrue = false;

			//ApplicableEmissions is null
			{
				//     Init Input
				MpParameters.CurrentQualificationLee = new ECMPS.Checks.Data.Ecmps.CheckMp.Function.MonitorQualificationLEEParameter
					(qualLeeTestTypeCd: "QLEEGOOD", potentialAnnualEmissions: null, applicableEmissionStandard: null, emissionStandardPct: 1, emissionStandardUom: "NOTNULL");

				//     Init Output
				category.CheckCatalogResult = null;

				//     Run Checks
				actual = cQualificationLEEChecks.QUALLEE3(category, ref log);

				//     Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual("C", category.CheckCatalogResult, "Result");
			}

			//emissionStandardPct is null
			{
				//     Init Input
				MpParameters.CurrentQualificationLee = new ECMPS.Checks.Data.Ecmps.CheckMp.Function.MonitorQualificationLEEParameter
					(qualLeeTestTypeCd: "QLEEGOOD", potentialAnnualEmissions: null, applicableEmissionStandard: 1, emissionStandardPct: null, emissionStandardUom: "NOTNULL");

				//     Init Output
				category.CheckCatalogResult = null;

				//     Run Checks
				actual = cQualificationLEEChecks.QUALLEE3(category, ref log);

				//     Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual("C", category.CheckCatalogResult, "Result");
			}

			//emissionStandardUom is null
			{
				//     Init Input
				MpParameters.CurrentQualificationLee = new ECMPS.Checks.Data.Ecmps.CheckMp.Function.MonitorQualificationLEEParameter
					(qualLeeTestTypeCd: "QLEEGOOD", potentialAnnualEmissions: null, applicableEmissionStandard: 1, emissionStandardPct: 1, emissionStandardUom: null);

				//     Init Output
				category.CheckCatalogResult = null;

				//     Run Checks
				actual = cQualificationLEEChecks.QUALLEE3(category, ref log);

				//     Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual("C", category.CheckCatalogResult, "Result");
			}

			//combo
			{
				//     Init Input
				MpParameters.CurrentQualificationLee = new ECMPS.Checks.Data.Ecmps.CheckMp.Function.MonitorQualificationLEEParameter
					(qualLeeTestTypeCd: "QLEEGOOD", potentialAnnualEmissions: null, applicableEmissionStandard: 1, emissionStandardPct: null, emissionStandardUom: null);

				//     Init Output
				category.CheckCatalogResult = null;

				//     Run Checks
				actual = cQualificationLEEChecks.QUALLEE3(category, ref log);

				//     Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual("C", category.CheckCatalogResult, "Result");
			}

			//Standards not null
			{
				//     Init Input
				MpParameters.CurrentQualificationLee = new ECMPS.Checks.Data.Ecmps.CheckMp.Function.MonitorQualificationLEEParameter
					(qualLeeTestTypeCd: "QLEEGOOD", potentialAnnualEmissions: null, applicableEmissionStandard: 1, emissionStandardPct: 1, emissionStandardUom: "NOTNULL");

				//     Init Output
				category.CheckCatalogResult = null;

				//     Run Checks
				actual = cQualificationLEEChecks.QUALLEE3(category, ref log);

				//     Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual(null, category.CheckCatalogResult, "Result");
			}

		}


	}
}
