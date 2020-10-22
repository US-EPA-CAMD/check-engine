using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.CheckEngine.SpecialParameterClasses;

using ECMPS.Checks.Data.Ecmps.CheckEm.Function;
using ECMPS.Checks.Data.Ecmps.Dbo.View;
using ECMPS.Checks.Data.Ecmps.Dbo.Table;

using ECMPS.Checks.Em.Parameters;
using ECMPS.Checks.EmissionsChecks;
using ECMPS.Checks.EmissionsReport;
using ECMPS.Checks.Parameters;
using ECMPS.Definitions.Extensions;

using UnitTest.UtilityClasses;
using ECMPS.Checks.TypeUtilities;


namespace UnitTest.Emissions
{
	[TestClass]
	public class DailyCalibrationTest
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


		#region Test Cases

		[TestMethod]
		public void DayCal1()
		{
			/* Initialize objects generally needed for testing checks. */
			cEmissionsCheckParameters parameters = UnitTestCheckParameters.InstantiateEmParameters();
			cDailyCalibrationChecks target = new cDailyCalibrationChecks(parameters);
			cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

			EmParameters.Init(category.Process);
			EmParameters.Category = category;

			/* Initialize General Variables */
			int?[] componentCaseList = { null, 0, 1 };

			/* Initialize variables needed to run the check. */
			bool log = false;
			string actual;

			/* Run Test Cases */
			foreach (bool hasComponentIdentifier in UnitTestStandardLists.BooleanList)
				foreach (string componentTypeCd in UnitTestStandardLists.ComponentTypeCodeList)
					foreach (int? onlineOfflineInd in UnitTestStandardLists.IndicatorList)
					{
						/*  Initialize Input Parameters*/
						EmParameters.CurrentDailyCalibrationTest = new VwMpDailyCalibrationRow(componentIdentifier: (hasComponentIdentifier ? "cmpId2" : null), componentTypeCd: componentTypeCd, onlineOfflineInd: onlineOfflineInd);

						/* Initialize Output Parameters */
						EmParameters.DailyCalComponentTypeValid = null;

						EmParameters.DailyCalCalcResult = "Wha?";
						EmParameters.DailyCalFailDate = DateTime.Now.Date;
						EmParameters.DailyCalFailHour = DateTime.Now.Hour;

						/* Init Cateogry Result */
						category.CheckCatalogResult = null;

						// Run Checks
						actual = target.DAYCAL1(category, ref log);

						/* Expected Results */
						string result = null;
						{
							if (!hasComponentIdentifier)
								result = "A";
							else if (!componentTypeCd.InList("CO2,FLOW,HCL,HG,NOX,O2,SO2"))
								result = "B";
							else if (componentTypeCd.InList("HCL,HG") && (onlineOfflineInd != 1))
								result = "C";
						}

						bool? expectedValid = (result == null);

						/* Check Result Label */
						string resultPrefix = string.Format("[hasComponentId: {0}, componentTypeCd: {1}, onlineOfflineInd: {2}]", hasComponentIdentifier, componentTypeCd, onlineOfflineInd);

						// Check Results
						Assert.AreEqual(string.Empty, actual);
						Assert.AreEqual(false, log);
						Assert.AreEqual(result, category.CheckCatalogResult, resultPrefix + ".Result");

						Assert.AreEqual(expectedValid, EmParameters.DailyCalComponentTypeValid, resultPrefix + ".DailyCalCalcResult");
						Assert.AreEqual(null, EmParameters.DailyCalCalcResult, resultPrefix + ".DailyCalCalcResult");
						Assert.AreEqual(null, EmParameters.DailyCalFailDate, resultPrefix + ".DailyCalFailDate");
						Assert.AreEqual(null, EmParameters.DailyCalFailHour, resultPrefix + ".DailyCalFailHour");
					}
		}

        [TestMethod]
        public void DayCal4ABandD()
        {
            /* Initialize objects generally needed for testing checks. */
            cEmissionsCheckParameters parameters = UnitTestCheckParameters.InstantiateEmParameters();
            cDailyCalibrationChecks target = new cDailyCalibrationChecks(parameters);
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            /* Initialize variables needed to run the check. */
            bool log = false;
            string actual;

            /* Run Test Cases */
            foreach (string ComponentTypeCode in UnitTestStandardLists.ComponentTypeCodeList)
                foreach (string SpanScaleCode in UnitTestStandardLists.SpanScaleCodeList)
                {
                    /*  Initialize Input Parameters*/
                    EmParameters.CurrentDailyCalibrationTest = new VwMpDailyCalibrationRow(componentTypeCd: ComponentTypeCode, 
                                                                                           spanScaleCd: SpanScaleCode);
                    EmParameters.DailyCalComponentTypeValid = true;

                    /*  Initialize Output Parameters*/
                    EmParameters.DailyCalSpanScaleValid = null;

                    /* Init Cateogry Result */
                    category.CheckCatalogResult = null; 

                    // Run Checks
                    actual = target.DAYCAL4(category, ref log);
                     
                    switch (ComponentTypeCode)
                    {
                        case "FLOW":
                            if (!string.IsNullOrEmpty(SpanScaleCode)){
                                Assert.AreEqual(false, cDBConvert.ToBoolean(EmParameters.DailyCalSpanScaleValid), "DailyCalSpanScaleValid");
                                Assert.AreEqual("D", category.CheckCatalogResult, "Result");
                            }
                            break;

                        case "HG":
                        case "HCL":
                            if (string.IsNullOrEmpty(SpanScaleCode)){
                                Assert.AreEqual(false, cDBConvert.ToBoolean(EmParameters.DailyCalSpanScaleValid), "DailyCalSpanScaleValid");
                                Assert.AreEqual("A", category.CheckCatalogResult, "Result");
                            }
                            else if (SpanScaleCode != "H"){
                                Assert.AreEqual(false, cDBConvert.ToBoolean(EmParameters.DailyCalSpanScaleValid), "DailyCalSpanScaleValid");
                                Assert.AreEqual("B", category.CheckCatalogResult, "Result");
                            }
                            break;
                        default:
                            if (string.IsNullOrEmpty(SpanScaleCode)){
                                Assert.AreEqual(false, cDBConvert.ToBoolean(EmParameters.DailyCalSpanScaleValid), "DailyCalSpanScaleValid");
                                Assert.AreEqual("A", category.CheckCatalogResult, "Result");
                            }
                            else if (!SpanScaleCode.InList("H,L")){
                                Assert.AreEqual(false, cDBConvert.ToBoolean(EmParameters.DailyCalSpanScaleValid), "DailyCalSpanScaleValid");
                                Assert.AreEqual("B", category.CheckCatalogResult, "Result");
                            }
                            else{
                                // Do EM Test to check for result C.
                            }
                            break;
                    } 
                    // Check Results
                    Assert.AreEqual(string.Empty, actual);
                    Assert.AreEqual(false, log);
                }
        }


        /// <summary>
        /// PGVP-2 Cylinder Id Format
        /// 
        /// 1) Start with a seed Cylinder Id that contains each number and capital letter.
        /// 2) Select a position in the seed id, and successively replace the character at that position with each ASCII character.
        /// 3) AETB-11 should return a result of A if the replacement character is not a number or a capital letter.
        /// </summary>
        [TestMethod()]
        public void DayCal27_CylinderIdFormat()
        {
            /* Initialize objects generally needed for testing checks. */
            cEmissionsCheckParameters emCheckParameters = new cEmissionsCheckParameters();
            cDailyCalibrationChecks target = new cDailyCalibrationChecks(emCheckParameters);
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory(emCheckParameters);

            EmParameters.Init(category.Process);
            EmParameters.Category = category;


            /* General Values */
            string seed1 = "ABC9DE8FG7HI6JK5LM";
            string seed2 = "NO4PQ3RS2TU1VW0XYZ";
            string cylinderId;
            string expList;
            string expResult = null;
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
                                expList = "OldId,AaaId";
                            }
                            break;
                        default:
                            {
                                expList = string.Format("OldId,AaaId,{0}", cylinderId);
                            }
                            break;
                    }
                }


                /* Initialize Input Parameters */
                EmParameters.CurrentDailyCalibrationTest = new VwMpDailyCalibrationRow(cylinderId: cylinderId, upscaleGasTypeCd: "GASGOOD");
                EmParameters.EvaluateUpscaleInjection = true;
                EmParameters.UpscaleGasTypeValid = true;

                /* Initialize Input-Output Parameters */
                EmParameters.InvalidCylinderIdList = new List<string> { "OldId", "AaaId" };


                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Initialize variables needed to run the check. */
                bool log = false;
                string actual;

                /* Run Check */
                actual = target.DAYCAL27(category, ref log);


                /* Check Results */
                Assert.AreEqual(string.Empty, actual, string.Format("actual [{0} => {1}]", ascii, cylinderId));
                Assert.AreEqual(false, log, string.Format("log [{0} => {1}]", ascii, cylinderId));
                Assert.AreEqual(expResult, category.CheckCatalogResult, string.Format("CheckCatalogResult [{0} => {1}]", ascii, cylinderId));
                Assert.AreEqual(expList, EmParameters.InvalidCylinderIdList.DelimitedList(","), string.Format("InvalidCylinderIdList [{0} => {1}]", ascii, cylinderId));
            }
        }


        /// <summary>
        ///A test for DAYCAL28 - Extend PGVP expiration to 8 years
        ///</summary>()
        [TestMethod]
		public void DAYCAL28()
		{
			//instantiated checks and old instantiated parameters setup

			cEmissionsCheckParameters emCheckParameters = UnitTestCheckParameters.InstantiateEmParameters();
			cDailyCalibrationChecks target = new cDailyCalibrationChecks(emCheckParameters);
			
			// Init Variables
			bool log = false;
			string actual;

			// Init Input
			EmParameters.EvaluateUpscaleInjection = true;
			EmParameters.ProtocolGasVendorLookupTable = new CheckDataView<ProtocolGasVendorRow>(new ProtocolGasVendorRow(vendorId: "NONPGVP"));
			EmParameters.UpscaleGasTypeValid = true;

			//Result D
			{
				EmParameters.CurrentDailyCalibrationTest = new VwMpDailyCalibrationRow(vendorId: "NONPGVP", dailyTestDate: DateTime.Today, dailyTestDatetime: DateTime.Today, dailyTestHour: 0, dailyTestMin: 0, upscaleGasTypeCd: "NOTNULL");
				EmParameters.DailyCalPgvpRuleDate = DateTime.Today.AddDays(-60).AddYears(-8);

				// Init Output
				EmParameters.Category.CheckCatalogResult = null;

				// Run Checks
				actual = target.DAYCAL28(EmParameters.Category, ref log);

				// Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual("D", EmParameters.Category.CheckCatalogResult, "Result");
			}

			//Pass
			{
				EmParameters.CurrentDailyCalibrationTest = new VwMpDailyCalibrationRow(vendorId: "NONPGVP", dailyTestDate: DateTime.Today, dailyTestDatetime: DateTime.Today, dailyTestHour: 0, dailyTestMin: 0, upscaleGasTypeCd: "NOTNULL");
				EmParameters.DailyCalPgvpRuleDate = DateTime.Today.AddDays(-59).AddYears(-8);

				// Init Output
				EmParameters.Category.CheckCatalogResult = null;

				// Run Checks
				actual = target.DAYCAL28(EmParameters.Category, ref log);

				// Check Results
				Assert.AreEqual(string.Empty, actual);
				Assert.AreEqual(false, log);
				Assert.AreEqual(null, EmParameters.Category.CheckCatalogResult, "Result");
			}

            //Result G
            {
                EmParameters.CurrentDailyCalibrationTest = new VwMpDailyCalibrationRow(vendorId: "NONPGVP", dailyTestDate: DateTime.Today.AddYears(-8).AddDays(-1), dailyTestDatetime: DateTime.Today.AddYears(-8).AddDays(-1), dailyTestHour: 0, dailyTestMin: 0, upscaleGasTypeCd: "NOTNULL");
                EmParameters.ProtocolGasVendorLookupTable = new CheckDataView<ProtocolGasVendorRow>(
                        new ProtocolGasVendorRow(vendorId: "NONPGVP", activationDate: DateTime.Today.AddDays(-1)));
                EmParameters.DailyCalPgvpRuleDate = DateTime.Today.AddDays(-60).AddYears(-8);

                // Init Output
                EmParameters.Category.CheckCatalogResult = null;

                // Run Checks
                actual = target.DAYCAL28(EmParameters.Category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual("G", EmParameters.Category.CheckCatalogResult, "Result");
            }

            //Result G
            {
                EmParameters.CurrentDailyCalibrationTest = new VwMpDailyCalibrationRow(vendorId: "V01", dailyTestDate: DateTime.Today.AddYears(-8).AddDays(-1), dailyTestDatetime: DateTime.Today.AddYears(-8).AddDays(-1), dailyTestHour: 0, dailyTestMin: 0, upscaleGasTypeCd: "NOTNULL");
                EmParameters.ProtocolGasVendorLookupTable = new CheckDataView<ProtocolGasVendorRow>(
                        new ProtocolGasVendorRow(vendorId: "V01", activationDate: DateTime.Today.AddDays(-1)));
                //EmParameters.DailyCalPgvpRuleDate = DateTime.Today.AddDays(-60).AddYears(-8);

                // Init Output
                EmParameters.Category.CheckCatalogResult = null;

                // Run Checks
                actual = target.DAYCAL28(EmParameters.Category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual("G", EmParameters.Category.CheckCatalogResult, "Result");
            }

        }

		#endregion

	}
}
