using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.CertEventChecks;

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
	/// Summary description for cCertEventChecksTest
	/// </summary>
	[TestClass]
	public class cCertEventChecksTest
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

		#region QACERT 11-20

		/// <summary>
		///A test for QACERT12_RemoveHGK
		///</summary>()
		[TestMethod()]
		public void QACERT12_RemoveHGK()
		{
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			QaParameters.Init(category.Process);
			QaParameters.Category = category;

			// Variables
			bool log = false;
			string actual;
			string[] testSysComponentTypeList = { "HGK", "CEM" };

			// Init Input
			QaParameters.CurrentQaCertEvent = new VwQaCertEventRow(qaCertEventCd: "100");
			QaParameters.QaCertEventComponentType = "";
			QaParameters.QaCertEventRequiredIdCode = "B";
			QaParameters.QaCertEventSystemType = "HGK";

			foreach (string testSysComponentType in testSysComponentTypeList)
			{
				QaParameters.EventCodeToSystemOrComponentTypeCrossCheckTable = new CheckDataView<EventCodeToSystemOrComponentTypeRow>(
					new EventCodeToSystemOrComponentTypeRow(eventCode1: "100", eventCode2: null, systemOrComponentType: testSysComponentType));
				// Init Output
				category.CheckCatalogResult = null;

				// Run Checks
				actual = cCertEventChecks.QACERT12(category, ref log);

				// Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				if (testSysComponentType == "CEM")
					Assert.AreEqual("B", category.CheckCatalogResult, "Result");
				else
					Assert.AreEqual(null, category.CheckCatalogResult, "Result");
			}
		}


		/// <summary>
		///A test for QACERT13_RemoveHGK
		///</summary>()
		[TestMethod()]
		public void QACERT13_RemoveHGK()
		{
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			QaParameters.Init(category.Process);
			QaParameters.Category = category;

			// Variables
			bool log = false;
			string actual;
			string[] testCertSysTypeList = { "HGK", "HG", "ST" };

			// Init Input
			QaParameters.CurrentQaCertEvent = new VwQaCertEventRow(qaCertEventCd: "100", requiredTestCd: "1");
			QaParameters.QaCertEventComponentType = "";
			QaParameters.QaCertEventRequiredIdCode = "B";
			QaParameters.QaCertEventValidSystemOrComponent = "RATA";

			foreach (string testCertSysType in testCertSysTypeList)
			{
				QaParameters.QaCertEventSystemType = testCertSysType;

				// Init Output
				category.CheckCatalogResult = null;

				// Run Checks
				actual = cCertEventChecks.QACERT13(category, ref log);

				// Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				if (testCertSysType.NotInList("SO2,SO2R,NOXC,NOX,CO2,O2,FLOW,H2O,H2OM,HG,ST"))
					Assert.AreEqual("B", category.CheckCatalogResult, "Result");
				else
					Assert.AreEqual(null, category.CheckCatalogResult, "Result");
			}
		}
		
		#endregion

	}
}
