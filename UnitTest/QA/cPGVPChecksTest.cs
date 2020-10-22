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
    [TestClass]
    public class cPGVPChecksTest
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
        /// PGVP-2 Cylinder Id Format
        /// 
        /// 1) Start with a seed Cylinder Id that contains each number and capital letter.
        /// 2) Select a position in the seed id, and successively replace the character at that position with each ASCII character.
        /// 3) AETB-11 should return a result of A if the replacement character is not a number or a capital letter.
        /// </summary>
        [TestMethod()]
        public void Pgvp2_CylinderIdFormat()
        {
            /* Initialize objects generally needed for testing checks. */
            cQaCheckParameters qaCheckParameters = UnitTestCheckParameters.InstantiateQaParameters();
            cPgvpChecks target = new cPgvpChecks(qaCheckParameters);
            cCategory category = QaParameters.Category;

            /* General Values */
            string seed1 = "ABC9DE8FG7HI6JK5LM";
            string seed2 = "NO4PQ3RS2TU1VW0XYZ";
            string cylinderId;
            string expResult;
            char testChar;

            for (int ascii = 0; ascii <= 255; ascii++)
            {
                /* Setup Variables */
                testChar = (char)ascii;
                {
                    cylinderId = seed1 + testChar + seed2;

                    switch (testChar)
                    {
                        case 'A': case 'B': case 'C': case 'D': case 'E': case 'F': case 'G': case 'H': case 'I': case 'J': case 'K': case 'L': case 'M':
                        case 'N': case 'O': case 'P': case 'Q': case 'R': case 'S': case 'T': case 'U': case 'V': case 'W': case 'X': case 'Y': case 'Z':
                        case '1': case '2': case '3': case '4': case '5': case '6': case '7': case '8': case '9': case '0': case '-': case '&': case '.':
                            {
                                expResult = null;
                            }
                            break;
                        default:
                            {
                                expResult = "C";
                            }
                            break;
                    }                    
                }


                /* Initialize Input Parameters */
                QaParameters.CurrentProtocolGasRecord = new ProtocolGasRow(cylinderId: cylinderId, gasTypeCd: "GASGOOD");
                QaParameters.ProtocolGasParameter = "PGP";


                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Initialize variables needed to run the check. */
                bool log = false;
                string actual;

                /* Run Check */
                actual = target.PGVP2(category, ref log);


                /* Check Results */
                Assert.AreEqual(expResult, category.CheckCatalogResult, string.Format("category.CheckCatalogResult [{0} => {1}]", ascii, cylinderId));
            }
        }


        #region PGVP-3

        /// <summary>
        ///A test for PGVP3
        ///</summary>()
        [TestMethod()]
		public void PGVP3_Extend_Deadline()
		{
			//instantiated checks setup
			cQaCheckParameters qaCheckParameters = UnitTestCheckParameters.InstantiateQaParameters();
			cPgvpChecks target = new cPgvpChecks(qaCheckParameters);

			// Variables
			bool log = false;
			string actual;

			//Result E
			{
				// Init Input
				QaParameters.CurrentProtocolGasRecord = new ProtocolGasRow(vendorId:"V01", gasTypeCd: "GASGOOD");
				QaParameters.ProtocolGasParameter = "NOTNULL";
				QaParameters.CurrentTest = new VwQaTestSummaryRow(beginDate: new DateTime(2020, 1, 1));
				QaParameters.ProtocolGasVendorLookupTable = new CheckDataView<ProtocolGasVendorRow>(
						new ProtocolGasVendorRow(vendorId: "V01", deactivationDate: new DateTime(2011, 12, 31)));
				QaParameters.SystemParameterLookupTable = new CheckDataView<VwSystemParameterRow>();

				// Init Output
				QaParameters.Category.CheckCatalogResult = null;

				// Run Checks
				actual = target.PGVP3(QaParameters.Category, ref log);

				//// Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual("E", QaParameters.Category.CheckCatalogResult, "Result");
			}

			//Result Not E
			{
				// Init Input
				QaParameters.CurrentProtocolGasRecord = new ProtocolGasRow(vendorId: "V01", gasTypeCd: "GASGOOD");
				QaParameters.ProtocolGasParameter = "NOTNULL";
				QaParameters.CurrentTest = new VwQaTestSummaryRow(beginDate: DateTime.Today.AddDays(-1));
				QaParameters.ProtocolGasVendorLookupTable = new CheckDataView<ProtocolGasVendorRow>(
						new ProtocolGasVendorRow(vendorId: "V01", deactivationDate: DateTime.Today.AddYears(-8).AddDays(-1)));
				QaParameters.SystemParameterLookupTable = new CheckDataView<VwSystemParameterRow>();

				// Init Output
				QaParameters.Category.CheckCatalogResult = null;

				// Run Checks
				actual = target.PGVP3(QaParameters.Category, ref log);

				//// Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual(null, QaParameters.Category.CheckCatalogResult, "Result");
			}

			//Result D
			{
				// Init Input
				QaParameters.CurrentProtocolGasRecord = new ProtocolGasRow(vendorId: "NONPGVP", gasTypeCd: "GASGOOD");
				QaParameters.ProtocolGasParameter = "NOTNULL";
				QaParameters.CurrentTest = new VwQaTestSummaryRow(beginDate: DateTime.Today);
				QaParameters.ProtocolGasVendorLookupTable = new CheckDataView<ProtocolGasVendorRow>(
					new ProtocolGasVendorRow(vendorId: "NONPGVP"));
				QaParameters.SystemParameterLookupTable = new CheckDataView<VwSystemParameterRow>(
					new VwSystemParameterRow(sysParamName: "PGVP_AETB_RULE_DATE", paramValue1: (DateTime.Today.AddYears(-8).AddDays(-60)).ToString()));

				// Init Output
				QaParameters.Category.CheckCatalogResult = null;

				// Run Checks
				actual = target.PGVP3(QaParameters.Category, ref log);

				//// Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual("D", QaParameters.Category.CheckCatalogResult, "Result");
			}

			//Result NOT D
			{
				// Init Input
				QaParameters.CurrentProtocolGasRecord = new ProtocolGasRow(vendorId: "NONPGVP", gasTypeCd: "GASGOOD");
				QaParameters.ProtocolGasParameter = "NOTNULL";
				QaParameters.CurrentTest = new VwQaTestSummaryRow(beginDate: DateTime.Today.AddDays(-1));
				QaParameters.ProtocolGasVendorLookupTable = new CheckDataView<ProtocolGasVendorRow>(
					new ProtocolGasVendorRow(vendorId: "NONPGVP"));
				QaParameters.SystemParameterLookupTable = new CheckDataView<VwSystemParameterRow>(
					new VwSystemParameterRow(sysParamName: "PGVP_AETB_RULE_DATE", paramValue1: (DateTime.Today.AddYears(-8).AddDays(-60)).ToString()));

				// Init Output
				QaParameters.Category.CheckCatalogResult = null;

				// Run Checks
				actual = target.PGVP3(QaParameters.Category, ref log);

				//// Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual(null, QaParameters.Category.CheckCatalogResult, "Result");
			}

            //Result F
            {
                // Init Input
                QaParameters.CurrentProtocolGasRecord = new ProtocolGasRow(vendorId: "NONPGVP", gasTypeCd: "GASGOOD");
                QaParameters.ProtocolGasParameter = "NOTNULL";
                QaParameters.CurrentTest = new VwQaTestSummaryRow(beginDate: DateTime.Today.AddYears(-8).AddDays(-1));
                QaParameters.ProtocolGasVendorLookupTable = new CheckDataView<ProtocolGasVendorRow>(
                        new ProtocolGasVendorRow(vendorId: "NONPGVP", activationDate: DateTime.Today.AddDays(-1)));
                QaParameters.SystemParameterLookupTable = new CheckDataView<VwSystemParameterRow>(
                    new VwSystemParameterRow(sysParamName: "PGVP_AETB_RULE_DATE", paramValue1: (DateTime.Today.AddYears(-8).AddDays(-60)).ToString()));

                // Init Output
                QaParameters.Category.CheckCatalogResult = null;

                // Run Checks
                actual = target.PGVP3(QaParameters.Category, ref log);

                //// Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual("F", QaParameters.Category.CheckCatalogResult, "Result");
            }

            //Result F
            {
                // Init Input
                QaParameters.CurrentProtocolGasRecord = new ProtocolGasRow(vendorId: "V01", gasTypeCd: "GASGOOD");
                QaParameters.ProtocolGasParameter = "NOTNULL";
                QaParameters.CurrentTest = new VwQaTestSummaryRow(beginDate: DateTime.Today.AddYears(-8).AddDays(-1));
                QaParameters.ProtocolGasVendorLookupTable = new CheckDataView<ProtocolGasVendorRow>(
                        new ProtocolGasVendorRow(vendorId: "V01", activationDate: DateTime.Today.AddDays(-1)));
                QaParameters.SystemParameterLookupTable = new CheckDataView<VwSystemParameterRow>();

                // Init Output
                QaParameters.Category.CheckCatalogResult = null;

                // Run Checks
                actual = target.PGVP3(QaParameters.Category, ref log);

                //// Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual("F", QaParameters.Category.CheckCatalogResult, "Result");
            }

            //Result Not F
            {
                // Init Input
                QaParameters.CurrentProtocolGasRecord = new ProtocolGasRow(vendorId: "NONPGVP", gasTypeCd: "GASGOOD");
                QaParameters.ProtocolGasParameter = "NOTNULL";
                QaParameters.CurrentTest = new VwQaTestSummaryRow(beginDate: DateTime.Today);
                QaParameters.ProtocolGasVendorLookupTable = new CheckDataView<ProtocolGasVendorRow>(
                    new ProtocolGasVendorRow(vendorId: "NONPGVP"));
                QaParameters.SystemParameterLookupTable = new CheckDataView<VwSystemParameterRow>(
                    new VwSystemParameterRow(sysParamName: "PGVP_AETB_RULE_DATE", paramValue1: (DateTime.Today.AddYears(-8).AddDays(-60)).ToString()));

                // Init Output
                QaParameters.Category.CheckCatalogResult = null;

                // Run Checks
                actual = target.PGVP3(QaParameters.Category, ref log);

                //// Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual("D", QaParameters.Category.CheckCatalogResult, "Result");
            }
        }

			#endregion

    }
}
