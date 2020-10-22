using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Data.Ecmps.CrossCheck.Table;
using ECMPS.Checks.Data.Ecmps.Dbo.Table;
using ECMPS.Checks.Data.Ecmps.Dbo.View;
using ECMPS.Checks.Data.EcmpsAux.CrossCheck.Virtual;
using ECMPS.Checks.MATSSupplementalMethodChecks;
using ECMPS.Checks.MonitorPlan;
using ECMPS.Checks.Mp.Parameters;
using ECMPS.Definitions.Extensions;

using UnitTest.UtilityClasses;


namespace UnitTest.MonitorPlan
{
	/// <summary>
	///This is a test class for cMATSSupplementalMethodChecksTest and is intended
	///to contain all cMATSSupplementalMethodChecksTest Unit Tests
	/// </summary>
	[TestClass]
	public class cMATSSupplementalMethodChecksTest
	{
		public cMATSSupplementalMethodChecksTest()
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
		//A test for MATSMTH1
		//</summary>()
		[TestMethod()]
		public void MATSMTH1()
		{
			//static check setup
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			MpParameters.Init(category.Process);
			MpParameters.Category = category;

			//     Variables
			bool log = false;
			string actual;
			//    bool testTrue = false;

			//Result A
			{
				//     Init Input
				MpParameters.MatsSupplementalComplianceMethodRecord = new ECMPS.Checks.Data.Ecmps.CheckMp.Function.MATSMethodDataParameter(matsMethodCd: "METHGOOD");

				//     Init Output
				category.CheckCatalogResult = null;

				//     Run Checks
				actual = cMATSSupplementalMethodChecks.MATSMTH1(category, ref log);

				//     Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual("A", category.CheckCatalogResult, "Result");
			}

			//Result B
			{
				//     Init Input
				MpParameters.MatsSupplementalComplianceMethodRecord = new ECMPS.Checks.Data.Ecmps.CheckMp.Function.MATSMethodDataParameter(matsMethodCd: "METHGOOD", beginDate: DateTime.Today.AddDays(-1));
				MpParameters.MatsEvaluationBeginDate = DateTime.Today;

				//     Init Output
				category.CheckCatalogResult = null;

				//     Run Checks
				actual = cMATSSupplementalMethodChecks.MATSMTH1(category, ref log);

				//     Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual("B", category.CheckCatalogResult, "Result");
			}

			//Result C
			{
				//     Init Input
				MpParameters.MatsSupplementalComplianceMethodRecord = new ECMPS.Checks.Data.Ecmps.CheckMp.Function.MATSMethodDataParameter(matsMethodCd: "METHGOOD", beginDate: DateTime.Today.AddDays(1));
				MpParameters.MaximumFutureDate = DateTime.Today;

				//     Init Output
				category.CheckCatalogResult = null;

				//     Run Checks
				actual = cMATSSupplementalMethodChecks.MATSMTH1(category, ref log);

				//     Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual("C", category.CheckCatalogResult, "Result");
			}


			//No Errors
			{
				//     Init Input
				MpParameters.MatsSupplementalComplianceMethodRecord = new ECMPS.Checks.Data.Ecmps.CheckMp.Function.MATSMethodDataParameter(matsMethodCd: "METHGOOD", beginDate: DateTime.Today);
				MpParameters.MaximumFutureDate = DateTime.Today;
				MpParameters.MatsEvaluationBeginDate = DateTime.Today;

				//     Init Output
				category.CheckCatalogResult = null;

				//     Run Checks
				actual = cMATSSupplementalMethodChecks.MATSMTH1(category, ref log);

				//     Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual(null, category.CheckCatalogResult, "Result");
			}

		}

		// <summary>
		//A test for MATSMTH2
		//</summary>()
		[TestMethod()]
		public void MATSMTH2()
		{
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			MpParameters.Init(category.Process);
			MpParameters.Category = category;

			//     Variables
			bool log = false;
			string actual;
			//    bool testTrue = false;

			//Result A
			{
				//     Init Input
				MpParameters.MatsSupplementalComplianceMethodRecord = new ECMPS.Checks.Data.Ecmps.CheckMp.Function.MATSMethodDataParameter
					(matsMethodCd: "METHGOOD", beginDate: DateTime.Today);

				//     Init Output
				category.CheckCatalogResult = null;

				//     Run Checks
				actual = cMATSSupplementalMethodChecks.MATSMTH2(category, ref log);

				//     Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual("A", category.CheckCatalogResult, "Result");
			}

			//Result B < 0
			{
				//     Init Input
				MpParameters.MatsSupplementalComplianceMethodRecord = new ECMPS.Checks.Data.Ecmps.CheckMp.Function.MATSMethodDataParameter
					(matsMethodCd: "METHGOOD", beginDate: DateTime.Today, beginHour: -1);

				//     Init Output 
				category.CheckCatalogResult = null;

				//     Run Checks
				actual = cMATSSupplementalMethodChecks.MATSMTH2(category, ref log);

				//     Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual("B", category.CheckCatalogResult, "Result");
			}

			//Result B > 23
			{
				//     Init Input
				MpParameters.MatsSupplementalComplianceMethodRecord = new ECMPS.Checks.Data.Ecmps.CheckMp.Function.MATSMethodDataParameter
					(matsMethodCd: "METHGOOD", beginDate: DateTime.Today, beginHour: 24);

				//     Init Output
				category.CheckCatalogResult = null;

				//     Run Checks
				actual = cMATSSupplementalMethodChecks.MATSMTH2(category, ref log);

				//     Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual("B", category.CheckCatalogResult, "Result");
			}

			// no errors
			{
				//     Init Input
				MpParameters.MatsSupplementalComplianceMethodRecord = new ECMPS.Checks.Data.Ecmps.CheckMp.Function.MATSMethodDataParameter
					(matsMethodCd: "METHGOOD", beginDate: DateTime.Today, beginHour: 0);
				MpParameters.CurrentBeginDateValid = true;

				//     Init Output
				category.CheckCatalogResult = null;

				//     Run Checks
				actual = cMATSSupplementalMethodChecks.MATSMTH2(category, ref log);

				//     Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual(null, category.CheckCatalogResult, "Result");
				Assert.AreEqual(DateTime.Today, MpParameters.CurrentBeginDateAndHour, "CurrentBeginDateAndHour");
				Assert.AreEqual(true, MpParameters.CurrentBeginDateAndHourValid, "CurrentBeginDateAndHourValid");
			}
		}

		// <summary>
		//A test for MATSMTH3
		//</summary>()
		[TestMethod()]
		public void MATSMTH3()
		{
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			MpParameters.Init(category.Process);
			MpParameters.Category = category;

			//     Variables
			bool log = false;
			string actual;
			//    bool testTrue = false;

			//Result A
			{
				//     Init Input
				MpParameters.MatsSupplementalComplianceMethodRecord = new ECMPS.Checks.Data.Ecmps.CheckMp.Function.MATSMethodDataParameter
					(matsMethodCd: "METHGOOD", beginDate: DateTime.Today, endDate: DateTime.Today.AddDays(-1));
				MpParameters.MatsEvaluationBeginDate = DateTime.Today;

				//     Init Output
				category.CheckCatalogResult = null;

				//     Run Checks
				actual = cMATSSupplementalMethodChecks.MATSMTH3(category, ref log);

				//     Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual("A", category.CheckCatalogResult, "Result");
			}

			//Result B
			{
				//     Init Input
				MpParameters.MatsSupplementalComplianceMethodRecord = new ECMPS.Checks.Data.Ecmps.CheckMp.Function.MATSMethodDataParameter
					(matsMethodCd: "METHGOOD", beginDate: DateTime.Today, endDate: DateTime.Today.AddDays(1));
				MpParameters.MaximumFutureDate = DateTime.Today;

				//     Init Output
				category.CheckCatalogResult = null;

				//     Run Checks
				actual = cMATSSupplementalMethodChecks.MATSMTH3(category, ref log);

				//     Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual("B", category.CheckCatalogResult, "Result");
			}

			//no errors - enddate not null
			{
				//     Init Input
				MpParameters.MatsSupplementalComplianceMethodRecord = new ECMPS.Checks.Data.Ecmps.CheckMp.Function.MATSMethodDataParameter
					(matsMethodCd: "METHGOOD", beginDate: DateTime.Today, endDate: DateTime.Today.AddDays(1));
				MpParameters.MaximumFutureDate = DateTime.Today.AddDays(1);

				//     Init Output
				category.CheckCatalogResult = null;

				//     Run Checks
				actual = cMATSSupplementalMethodChecks.MATSMTH3(category, ref log);

				//     Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual(null, category.CheckCatalogResult, "Result");
			}

			//no errors - enddate is null
			{
				//     Init Input
				MpParameters.MatsSupplementalComplianceMethodRecord = new ECMPS.Checks.Data.Ecmps.CheckMp.Function.MATSMethodDataParameter
					(matsMethodCd: "METHGOOD", beginDate: DateTime.Today);
				MpParameters.MaximumFutureDate = DateTime.Today.AddDays(1);

				//     Init Output
				category.CheckCatalogResult = null;

				//     Run Checks
				actual = cMATSSupplementalMethodChecks.MATSMTH3(category, ref log);

				//     Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual(null, category.CheckCatalogResult, "Result");
			}
		}


				// <summary>
		//A test for MATSMTH4
		//</summary>()
		[TestMethod()]
		public void MATSMTH4()
		{
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			MpParameters.Init(category.Process);
			MpParameters.Category = category;

			//     Variables
			bool log = false;
			string actual;
			//    bool testTrue = false;

			//Result A
			{
				//     Init Input
				MpParameters.MatsSupplementalComplianceMethodRecord = new ECMPS.Checks.Data.Ecmps.CheckMp.Function.MATSMethodDataParameter
					(matsMethodCd: "METHGOOD", endDate: null, endHour: 0);

				//     Init Output
				category.CheckCatalogResult = null;

				//     Run Checks
				actual = cMATSSupplementalMethodChecks.MATSMTH4(category, ref log);

				//     Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual("A", category.CheckCatalogResult, "Result");
			}

			//Result B < 0
			{
				//     Init Input
				MpParameters.MatsSupplementalComplianceMethodRecord = new ECMPS.Checks.Data.Ecmps.CheckMp.Function.MATSMethodDataParameter
					(matsMethodCd: "METHGOOD", endDate: DateTime.Today, endHour: -1);

				//     Init Output
				category.CheckCatalogResult = null;

				//     Run Checks
				actual = cMATSSupplementalMethodChecks.MATSMTH4(category, ref log);

				//     Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual("B", category.CheckCatalogResult, "Result");
			}

			//Result B > 23
			{
				//     Init Input
				MpParameters.MatsSupplementalComplianceMethodRecord = new ECMPS.Checks.Data.Ecmps.CheckMp.Function.MATSMethodDataParameter
					(matsMethodCd: "METHGOOD", endDate: DateTime.Today, endHour: 24);

				//     Init Output
				category.CheckCatalogResult = null;

				//     Run Checks
				actual = cMATSSupplementalMethodChecks.MATSMTH4(category, ref log);

				//     Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual("B", category.CheckCatalogResult, "Result");
			}

			//no errors - EndHour not null
			{
				//     Init Input
				MpParameters.MatsSupplementalComplianceMethodRecord = new ECMPS.Checks.Data.Ecmps.CheckMp.Function.MATSMethodDataParameter
					(matsMethodCd: "METHGOOD", endDate: DateTime.Today, endHour: 0);
				MpParameters.CurrentEndDateValid = true;

				//     Init Output
				category.CheckCatalogResult = null;

				//     Run Checks
				actual = cMATSSupplementalMethodChecks.MATSMTH4(category, ref log);

				//     Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual(null, category.CheckCatalogResult, "Result");
				Assert.AreEqual(DateTime.Today, MpParameters.CurrentEndDateAndHour, "CurrentEndDateAndHour");
				Assert.AreEqual(true, MpParameters.CurrentEndDateAndHourValid, "CurrentEndDateAndHourValid");
			}

			//Result C
			{
				//     Init Input
				MpParameters.MatsSupplementalComplianceMethodRecord = new ECMPS.Checks.Data.Ecmps.CheckMp.Function.MATSMethodDataParameter
					(matsMethodCd: "METHGOOD", endDate: DateTime.Today, endHour: null);

				//     Init Output
				category.CheckCatalogResult = null;

				//     Run Checks
				actual = cMATSSupplementalMethodChecks.MATSMTH4(category, ref log);

				//     Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual("C", category.CheckCatalogResult, "Result");
				
			}

			//no errors - EndHour null
			{
				//     Init Input
				MpParameters.MatsSupplementalComplianceMethodRecord = new ECMPS.Checks.Data.Ecmps.CheckMp.Function.MATSMethodDataParameter
					(matsMethodCd: "METHGOOD", endDate: null, endHour: null);
				MpParameters.CurrentEndDateValid = true;

				//     Init Output
				category.CheckCatalogResult = null;

				//     Run Checks
				actual = cMATSSupplementalMethodChecks.MATSMTH4(category, ref log);

				//     Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual(null, category.CheckCatalogResult, "Result");
				Assert.AreEqual(null, MpParameters.CurrentEndDateAndHour, "CurrentEndDateAndHour");
				Assert.AreEqual(true, MpParameters.CurrentEndDateAndHourValid, "CurrentEndDateAndHourValid");

			}


		}


		// <summary>
		//A test for MATSMTH5
		//</summary>()
		[TestMethod()]
		public void MATSMTH5()
		{
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			MpParameters.Init(category.Process);
			MpParameters.Category = category;

			//     Variables
			bool log = false;
			string actual;
			//    bool testTrue = false;

			//Result A - days
			{
				//     Init Input
				MpParameters.CurrentBeginDateAndHour =  DateTime.Today;
				MpParameters.CurrentEndDateAndHour = DateTime.Today.AddDays(-1);
				MpParameters.CurrentBeginDateAndHourValid = true;
				MpParameters.CurrentEndDateAndHourValid = true;

				//     Init Output
				category.CheckCatalogResult = null;

				//     Run Checks
				actual = cMATSSupplementalMethodChecks.MATSMTH5(category, ref log);

				//     Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual("A", category.CheckCatalogResult, "Result");
			}

			//Result A - hours
			{
				//     Init Input
				MpParameters.CurrentBeginDateAndHour = DateTime.Today.AddHours(-1);
				MpParameters.CurrentEndDateAndHour = DateTime.Today.AddHours(-2);
				MpParameters.CurrentBeginDateAndHourValid = true;
				MpParameters.CurrentEndDateAndHourValid = true;

				//     Init Output
				category.CheckCatalogResult = null;

				//     Run Checks
				actual = cMATSSupplementalMethodChecks.MATSMTH5(category, ref log);

				//     Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual("A", category.CheckCatalogResult, "Result");
			}


			//no errors
			{
				//     Init Input
				MpParameters.CurrentBeginDateAndHour = DateTime.Today;
				MpParameters.CurrentEndDateAndHour = DateTime.Today.AddDays(1);
				MpParameters.CurrentBeginDateAndHourValid = true;
				MpParameters.CurrentEndDateAndHourValid = true;

				//     Init Output
				category.CheckCatalogResult = null;

				//     Run Checks
				actual = cMATSSupplementalMethodChecks.MATSMTH5(category, ref log);

				//     Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual(null, category.CheckCatalogResult, "Result");
			}
		}

		// <summary>
		//A test for MATSMTH6
		//</summary>()
		[TestMethod()]
		public void MATSMTH6()
		{
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			MpParameters.Init(category.Process);
			MpParameters.Category = category;

			//     Variables
			bool log = false;
			string actual;
			//    bool testTrue = false;

			//Parameter null
			{
				//     Init Input
				MpParameters.MatsSupplementalComplianceMethodRecord = new ECMPS.Checks.Data.Ecmps.CheckMp.Function.MATSMethodDataParameter
					(matsMethodCd: "METHGOOD", matsMethodParameterCd: null);

				//     Init Output
				category.CheckCatalogResult = null;

				//     Run Checks
				actual = cMATSSupplementalMethodChecks.MATSMTH6(category, ref log);

				//     Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual("A", category.CheckCatalogResult, "Result");
				Assert.AreEqual(false, MpParameters.CurrentParameterValid, "CurrentParameterValid");
			}

			//Parameter not in lookup
			{
				//     Init Input
				MpParameters.MatsSupplementalComplianceMethodRecord = new ECMPS.Checks.Data.Ecmps.CheckMp.Function.MATSMethodDataParameter
					(matsMethodCd: "METHGOOD", matsMethodParameterCd: "PARAMBAD");
				MpParameters.MatsMethodParameterCodeLookup = new CheckDataView<ECMPS.Checks.Data.Ecmps.Lookup.Table.MatsMethodParameterCodeRow>();

				//     Init Output
				category.CheckCatalogResult = null;

				//     Run Checks
				actual = cMATSSupplementalMethodChecks.MATSMTH6(category, ref log);

				//     Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual("B", category.CheckCatalogResult, "Result");
				Assert.AreEqual(false, MpParameters.CurrentParameterValid, "CurrentParameterValid");
			}

			//no errors
			{
				//     Init Input
				MpParameters.MatsSupplementalComplianceMethodRecord = new ECMPS.Checks.Data.Ecmps.CheckMp.Function.MATSMethodDataParameter
					(matsMethodCd: "METHGOOD", matsMethodParameterCd: "PARAMGOOD");
				MpParameters.MatsMethodParameterCodeLookup = new CheckDataView<ECMPS.Checks.Data.Ecmps.Lookup.Table.MatsMethodParameterCodeRow>
					(new ECMPS.Checks.Data.Ecmps.Lookup.Table.MatsMethodParameterCodeRow(matsMethodParameterCd: "PARAMGOOD"));

				//     Init Output
				category.CheckCatalogResult = null;

				//     Run Checks
				actual = cMATSSupplementalMethodChecks.MATSMTH6(category, ref log);

				//     Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual(null, category.CheckCatalogResult, "Result");
				Assert.AreEqual(true, MpParameters.CurrentParameterValid, "CurrentParameterValid");
			}
		}

		// <summary>
		//A test for MATSMTH7
		//</summary>()
		[TestMethod()]
		public void MATSMTH7()
		{
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			MpParameters.Init(category.Process);
			MpParameters.Category = category;

			//     Variables
			bool log = false;
			string actual;
			//    bool testTrue = false;

			//Method null
			{
				//     Init Input
				MpParameters.MatsSupplementalComplianceMethodRecord = new ECMPS.Checks.Data.Ecmps.CheckMp.Function.MATSMethodDataParameter
					(matsMethodCd: null);

				//     Init Output
				category.CheckCatalogResult = null;

				//     Run Checks
				actual = cMATSSupplementalMethodChecks.MATSMTH7(category, ref log);

				//     Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual("A", category.CheckCatalogResult, "Result");
				Assert.AreEqual(false, MpParameters.CurrentMethodValid, "CurrentMethodValid");
			}

			//Method not in lookup
			{
				//     Init Input
				MpParameters.MatsSupplementalComplianceMethodRecord = new ECMPS.Checks.Data.Ecmps.CheckMp.Function.MATSMethodDataParameter
					(matsMethodCd: "METHBAD");
				MpParameters.MatsMethodCodeLookup = new CheckDataView<ECMPS.Checks.Data.Ecmps.Lookup.Table.MatsMethodCodeRow>();

				//     Init Output
				category.CheckCatalogResult = null;

				//     Run Checks
				actual = cMATSSupplementalMethodChecks.MATSMTH7(category, ref log);

				//     Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual("B", category.CheckCatalogResult, "Result");
				Assert.AreEqual(false, MpParameters.CurrentMethodValid, "CurrentMethodValid");
			}

			//no errors
			{
				//     Init Input
				MpParameters.MatsSupplementalComplianceMethodRecord = new ECMPS.Checks.Data.Ecmps.CheckMp.Function.MATSMethodDataParameter
					(matsMethodCd: "METHGOOD");
				MpParameters.MatsMethodCodeLookup = new CheckDataView<ECMPS.Checks.Data.Ecmps.Lookup.Table.MatsMethodCodeRow>
				(new ECMPS.Checks.Data.Ecmps.Lookup.Table.MatsMethodCodeRow(matsMethodCd: "METHGOOD"));

				//     Init Output
				category.CheckCatalogResult = null;

				//     Run Checks
				actual = cMATSSupplementalMethodChecks.MATSMTH7(category, ref log);

				//     Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual(null, category.CheckCatalogResult, "Result");
				Assert.AreEqual(true, MpParameters.CurrentMethodValid, "CurrentMethodValid");
			}
		}



		// <summary>
		//A test for MATSMTH8
		//</summary>()
		[TestMethod()]
		public void MATSMTH8()
		{
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			MpParameters.Init(category.Process);
			MpParameters.Category = category;

			//     Variables
			bool log = false;
			string actual;
			//    bool testTrue = false;

			//     Init Input
			MpParameters.CurrentParameterValid = true;
			MpParameters.CurrentMethodValid = true;

			//Crosscheck not found
			{
				MpParameters.MatsSupplementalComplianceMethodRecord = new ECMPS.Checks.Data.Ecmps.CheckMp.Function.MATSMethodDataParameter
					(matsMethodCd: "METHBAD", matsMethodParameterCd: "PARAMBAD");
				MpParameters.CrosscheckMatssupplementalcomplianceparametertomethod = new CheckDataView<MatsSupplementalComplianceParameterToMethodRow>();

				//     Init Output
				category.CheckCatalogResult = null;

				//     Run Checks
				actual = cMATSSupplementalMethodChecks.MATSMTH8(category, ref log);

				//     Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual("A", category.CheckCatalogResult, "Result");
			}

			//no errors
			{
				MpParameters.MatsSupplementalComplianceMethodRecord = new ECMPS.Checks.Data.Ecmps.CheckMp.Function.MATSMethodDataParameter
					(matsMethodCd: "METHGOOD", matsMethodParameterCd: "PARAMGOOD");
				MpParameters.CrosscheckMatssupplementalcomplianceparametertomethod = new CheckDataView<MatsSupplementalComplianceParameterToMethodRow>
					(new MatsSupplementalComplianceParameterToMethodRow(methodCode: "METHGOOD", parameterCode: "PARAMGOOD"));

				//     Init Output
				category.CheckCatalogResult = null;

				//     Run Checks
				actual = cMATSSupplementalMethodChecks.MATSMTH8(category, ref log);

				//     Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual(null, category.CheckCatalogResult, "Result");
			}
		}

		// <summary>
		//A test for MATSMTH9
		//</summary>()
		[TestMethod()]
		public void MATSMTH9()
		{
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			MpParameters.Init(category.Process);
			MpParameters.Category = category;

			//     Variables
			bool log = false;
			string actual;

			//Combined records not found
			{
				//     Init Input
				MpParameters.MatsCombinedMethodRecordsByLocation = new CheckDataView<ECMPS.Checks.Data.Ecmps.CheckMp.Function.MatsCombinedMethod>
				(new ECMPS.Checks.Data.Ecmps.CheckMp.Function.MatsCombinedMethod(parameterGroup: "HG", beginDatehour: DateTime.Today.AddDays(1),
					endDatehour: DateTime.Today));
				MpParameters.EvaluationEndDate = DateTime.Today;
				MpParameters.MatsEvaluationBeginDate = DateTime.Today;

				//     Init Output
				category.CheckCatalogResult = null;

				//     Run Checks
				actual = cMATSSupplementalMethodChecks.MATSMTH9(category, ref log);

				//     Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual(null, category.CheckCatalogResult, "Result");
			}

			//Result B
			{
				//     Init Input
				MpParameters.MatsCombinedMethodRecordsByLocation = new CheckDataView<ECMPS.Checks.Data.Ecmps.CheckMp.Function.MatsCombinedMethod>
				(new ECMPS.Checks.Data.Ecmps.CheckMp.Function.MatsCombinedMethod(parameterGroup: "HG",
					beginDatehour: DateTime.Today.AddDays(-10), endDatehour: DateTime.Today.AddDays(-8)),
					new ECMPS.Checks.Data.Ecmps.CheckMp.Function.MatsCombinedMethod(parameterGroup: "HG",
					beginDatehour: DateTime.Today.AddDays(-7), endDatehour: DateTime.Today));

				MpParameters.MatsEvaluationBeginDate = DateTime.Today.AddDays(-10);
				MpParameters.EvaluationEndDate = DateTime.Today;

				//     Init Output
				category.CheckCatalogResult = null;

				//     Run Checks
				actual = cMATSSupplementalMethodChecks.MATSMTH9(category, ref log);

				//     Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual("B", category.CheckCatalogResult, "Result");
			}

			//no errors
			{
				//     Init Input
				MpParameters.MatsCombinedMethodRecordsByLocation = new CheckDataView<ECMPS.Checks.Data.Ecmps.CheckMp.Function.MatsCombinedMethod>
				(new ECMPS.Checks.Data.Ecmps.CheckMp.Function.MatsCombinedMethod(parameterGroup: "HG",
					beginDatehour: DateTime.Today.AddDays(-10), endDatehour: DateTime.Today.AddDays(-8)),
					new ECMPS.Checks.Data.Ecmps.CheckMp.Function.MatsCombinedMethod(parameterGroup: "HG",
					beginDatehour: DateTime.Today.AddDays(-8), endDatehour: DateTime.Today));
				MpParameters.MatsEvaluationBeginDate = DateTime.Today.AddDays(-10);
				MpParameters.EvaluationEndDate = DateTime.Today;

				//     Init Output
				category.CheckCatalogResult = null;

				//     Run Checks
				actual = cMATSSupplementalMethodChecks.MATSMTH9(category, ref log);

				//     Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual(null, category.CheckCatalogResult, "Result");
			}
		}

		// <summary>
		//A test for MATSMTH10
		//</summary>()
		[TestMethod()]
		public void MATSMTH10()
		{
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			MpParameters.Init(category.Process);
			MpParameters.Category = category;

			//     Variables
			bool log = false;
			string actual;

			//Combined records not found
			{
				//     Init Input
				MpParameters.MatsCombinedMethodRecordsByLocation = new CheckDataView<ECMPS.Checks.Data.Ecmps.CheckMp.Function.MatsCombinedMethod>
				(new ECMPS.Checks.Data.Ecmps.CheckMp.Function.MatsCombinedMethod(parameterGroup: "HCL", beginDatehour: DateTime.Today.AddDays(1),
					endDatehour: DateTime.Today));
				MpParameters.EvaluationEndDate = DateTime.Today;
				MpParameters.MatsEvaluationBeginDate = DateTime.Today;

				//     Init Output
				category.CheckCatalogResult = null;

				//     Run Checks
				actual = cMATSSupplementalMethodChecks.MATSMTH10(category, ref log);

				//     Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual(null, category.CheckCatalogResult, "Result");
			}

			//Result B
			{
				//     Init Input
				MpParameters.MatsCombinedMethodRecordsByLocation = new CheckDataView<ECMPS.Checks.Data.Ecmps.CheckMp.Function.MatsCombinedMethod>
				(new ECMPS.Checks.Data.Ecmps.CheckMp.Function.MatsCombinedMethod(parameterGroup: "HCL",
					beginDatehour: DateTime.Today.AddDays(-10), endDatehour: DateTime.Today.AddDays(-8)),
					new ECMPS.Checks.Data.Ecmps.CheckMp.Function.MatsCombinedMethod(parameterGroup: "HCL",
					beginDatehour: DateTime.Today.AddDays(-7), endDatehour: DateTime.Today));

				MpParameters.MatsEvaluationBeginDate = DateTime.Today.AddDays(-10);
				MpParameters.EvaluationEndDate = DateTime.Today;

				//     Init Output
				category.CheckCatalogResult = null;

				//     Run Checks
				actual = cMATSSupplementalMethodChecks.MATSMTH10(category, ref log);

				//     Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual("B", category.CheckCatalogResult, "Result");
			}

			//no errors
			{
				//     Init Input
				MpParameters.MatsCombinedMethodRecordsByLocation = new CheckDataView<ECMPS.Checks.Data.Ecmps.CheckMp.Function.MatsCombinedMethod>
				(new ECMPS.Checks.Data.Ecmps.CheckMp.Function.MatsCombinedMethod(parameterGroup: "HCL",
					beginDatehour: DateTime.Today.AddDays(-10), endDatehour: DateTime.Today.AddDays(-8)),
					new ECMPS.Checks.Data.Ecmps.CheckMp.Function.MatsCombinedMethod(parameterGroup: "HCL",
					beginDatehour: DateTime.Today.AddDays(-8), endDatehour: DateTime.Today));
				MpParameters.MatsEvaluationBeginDate = DateTime.Today.AddDays(-10);
				MpParameters.EvaluationEndDate = DateTime.Today;

				//     Init Output
				category.CheckCatalogResult = null;

				//     Run Checks
				actual = cMATSSupplementalMethodChecks.MATSMTH10(category, ref log);

				//     Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual(null, category.CheckCatalogResult, "Result");
			}
		}

		// <summary>
		//A test for MATSMTH11
		//</summary>()
		[TestMethod()]
		public void MATSMTH11()
		{
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			MpParameters.Init(category.Process);
			MpParameters.Category = category;

			//     Variables
			bool log = false;
			string actual;

			//Combined records not found
			{
				//     Init Input
				MpParameters.MatsCombinedMethodRecordsByLocation = new CheckDataView<ECMPS.Checks.Data.Ecmps.CheckMp.Function.MatsCombinedMethod>
				(new ECMPS.Checks.Data.Ecmps.CheckMp.Function.MatsCombinedMethod(parameterGroup: "HF", beginDatehour: DateTime.Today.AddDays(1),
					endDatehour: DateTime.Today));
				MpParameters.EvaluationEndDate = DateTime.Today;
				MpParameters.MatsEvaluationBeginDate = DateTime.Today;

				//     Init Output
				category.CheckCatalogResult = null;

				//     Run Checks
				actual = cMATSSupplementalMethodChecks.MATSMTH11(category, ref log);

				//     Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual(null, category.CheckCatalogResult, "Result");
			}

			//Result B
			{
				//     Init Input
				MpParameters.MatsCombinedMethodRecordsByLocation = new CheckDataView<ECMPS.Checks.Data.Ecmps.CheckMp.Function.MatsCombinedMethod>
				(new ECMPS.Checks.Data.Ecmps.CheckMp.Function.MatsCombinedMethod(parameterGroup: "HF",
					beginDatehour: DateTime.Today.AddDays(-10), endDatehour: DateTime.Today.AddDays(-8)),
					new ECMPS.Checks.Data.Ecmps.CheckMp.Function.MatsCombinedMethod(parameterGroup: "HF",
					beginDatehour: DateTime.Today.AddDays(-7), endDatehour: DateTime.Today));

				MpParameters.MatsEvaluationBeginDate = DateTime.Today.AddDays(-10);
				MpParameters.EvaluationEndDate = DateTime.Today;

				//     Init Output
				category.CheckCatalogResult = null;

				//     Run Checks
				actual = cMATSSupplementalMethodChecks.MATSMTH11(category, ref log);

				//     Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual("B", category.CheckCatalogResult, "Result");
			}

			//no errors
			{
				//     Init Input
				MpParameters.MatsCombinedMethodRecordsByLocation = new CheckDataView<ECMPS.Checks.Data.Ecmps.CheckMp.Function.MatsCombinedMethod>
				(new ECMPS.Checks.Data.Ecmps.CheckMp.Function.MatsCombinedMethod(parameterGroup: "HF",
					beginDatehour: DateTime.Today.AddDays(-10), endDatehour: DateTime.Today.AddDays(-8)),
					new ECMPS.Checks.Data.Ecmps.CheckMp.Function.MatsCombinedMethod(parameterGroup: "HF",
					beginDatehour: DateTime.Today.AddDays(-8), endDatehour: DateTime.Today));
				MpParameters.MatsEvaluationBeginDate = DateTime.Today.AddDays(-10);
				MpParameters.EvaluationEndDate = DateTime.Today;

				//     Init Output
				category.CheckCatalogResult = null;

				//     Run Checks
				actual = cMATSSupplementalMethodChecks.MATSMTH11(category, ref log);

				//     Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual(null, category.CheckCatalogResult, "Result");
			}
		}
	}
}
