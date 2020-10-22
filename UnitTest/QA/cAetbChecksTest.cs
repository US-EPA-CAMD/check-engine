using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.TestChecks;
using ECMPS.Checks.QA;
using ECMPS.Definitions.Extensions;

using ECMPS.Checks;
using ECMPS.Checks.Data.Ecmps.Dbo.Table;
using ECMPS.Checks.Data.Ecmps.Dbo.View;
using ECMPS.Checks.Data.EcmpsAux.CrossCheck.Virtual;
using ECMPS.Checks.Data.Ecmps.CrossCheck.Table;
using ECMPS.Checks.Qa.Parameters;

using UnitTest.UtilityClasses;

namespace UnitTest.QA
{
	/// <summary>
	/// Summary description for cAetbChecksTest
	/// </summary>
	[TestClass]
	public class cAetbChecksTest
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


		#region AETB-10

		/// <summary>
		///A test for AETB-10
		///</summary>()
		[TestMethod()]
		public void AETB10()
		{
			//instantiated checks setup
			cQaCheckParameters qaCheckParameters = UnitTestCheckParameters.InstantiateQaParameters();
			cAetbChecks target = new cAetbChecks(qaCheckParameters);

			// Variables
			bool log = false;
			string actual;


			//no AETB record
			{
				string[] sysTypeList = { "HG","HCL","HF","ST","NOTMATS" };

				foreach (string sysTypeCode in sysTypeList)
				{
					// Init Input
					QaParameters.CurrentTest = new VwQaTestSummaryRow(sysTypeCd: sysTypeCode, beginDate: DateTime.Today.AddDays(1));
					QaParameters.SystemParameterLookupTable = new CheckDataView<VwSystemParameterRow>(
						new VwSystemParameterRow(sysParamName: "PGVP_AETB_RULE_DATE", paramValue1: DateTime.Today.AddYears(-1).ToString()));
					QaParameters.AirEmissionTestingRecords = new CheckDataView<ECMPS.Checks.Data.EcmpsWs.Dbo.View.VwQaAiremissiontestingdataRow>();

					// Init Output
					QaParameters.Category.CheckCatalogResult = null;

					// Run Checks
					actual = target.AETB10(QaParameters.Category, ref log);

					// Check Results
					Assert.AreEqual(string.Empty, actual);
					Assert.AreEqual(false, log);
					if (sysTypeCode.InList("HG,HCL,HF,ST"))
					{
						Assert.AreEqual(null, QaParameters.Category.CheckCatalogResult, "Result");
					}
					else
					{
						Assert.AreEqual("A", QaParameters.Category.CheckCatalogResult, "Result");
					}
				}
			}

			//with AETB record
			{
				string[] sysTypeList = { "HG", "HCL", "HF", "ST", "NOTMATS" };

				foreach (string sysTypeCode in sysTypeList)
				{
					// Init Input
					QaParameters.CurrentTest = new VwQaTestSummaryRow(sysTypeCd: sysTypeCode, beginDate: DateTime.Today.AddDays(1));
					QaParameters.SystemParameterLookupTable = new CheckDataView<VwSystemParameterRow>(
						new VwSystemParameterRow(sysParamName: "PGVP_AETB_RULE_DATE", paramValue1: DateTime.Today.AddYears(-1).ToString()));
					QaParameters.AirEmissionTestingRecords = new CheckDataView<ECMPS.Checks.Data.EcmpsWs.Dbo.View.VwQaAiremissiontestingdataRow>(
						new ECMPS.Checks.Data.EcmpsWs.Dbo.View.VwQaAiremissiontestingdataRow(airEmissionTestId: "1", testSumId: "1")
						);

					// Init Output
					QaParameters.Category.CheckCatalogResult = null;

					// Run Checks
					actual = target.AETB10(QaParameters.Category, ref log);

					// Check Results
					Assert.AreEqual(string.Empty, actual);
					Assert.AreEqual(false, log);
					if (sysTypeCode.InList("HG,HCL,HF,ST"))
					{
						Assert.AreEqual("B", QaParameters.Category.CheckCatalogResult, "Result");
					}
					else
					{
						Assert.AreEqual(null, QaParameters.Category.CheckCatalogResult, "Result");
					}
				}
			}
		}
		#endregion

	}
}
