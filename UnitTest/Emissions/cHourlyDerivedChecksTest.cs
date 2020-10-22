using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.EmissionsChecks;
using ECMPS.Checks.EmissionsReport;
using ECMPS.Definitions.Extensions;

using ECMPS.Checks.Data.Ecmps.Dbo.Table;
using ECMPS.Checks.Data.Ecmps.Dbo.View;
using ECMPS.Checks.Data.EcmpsAux.CrossCheck.Virtual;
using ECMPS.Checks.Data.Ecmps.CrossCheck.Table;
using ECMPS.Checks.Em.Parameters;

using UnitTest.UtilityClasses;

namespace UnitTest.Emissions
{
	[TestClass()]
	public class cHourlyDerivedValueChecksTest
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


		#region HOURDHV-13
		/// <summary>
		///A test for HOURDHV-13_AddMATSParam
		///</summary>()
		[TestMethod()]
		public void HOURDHV13_AddMATSParam()
		{
			//instantiated checks setup
            cHourlyDerivedValueChecks target = new cHourlyDerivedValueChecks(new cEmissionsCheckParameters());
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

			// Variables
			bool log = false;
			string actual;

			// Init Input
			EmParameters.CurrentDhvMethod = "MDF";
			EmParameters.CurrentDhvParameter = "H2O";
			EmParameters.CurrentDhvRecord = new VwMpDerivedHrlyValueRow(modcCd: "01");

			// Init Output
			category.CheckCatalogResult = null;

			// Run Checks
			actual = target.HOURDHV13(category, ref log);

			// Check Results
			Assert.AreEqual(string.Empty, actual);
			Assert.AreEqual(false, log);
			Assert.AreEqual("A", category.CheckCatalogResult, "Result");
			Assert.AreEqual("01", EmParameters.H2oDhvModc, "H2oDhvModc");
		}
        #endregion


        #region Checks 21-30

        /// <summary>
        /// 
        /// Constant Required Parameters: 
        /// 
        /// DerivedHourlyModcStatus = true
        /// LegacyDataEvaluation = false
        /// 
        /// MonitorFormulaRecordByHourLocation: 
        /// 
        ///     | FormualId | ParameterCd | EquationCd |
        ///     | OneId     | Param1      | F-12       |
        ///     | GoodId    | [Parameter] | F-13       |
        ///     | ThreeId   | Param3      | F-14       |
        /// 
        /// 
        /// Constant Optional Parameters: 
        /// 
        /// AppEConstantFuelMix = false
        /// HourlyFuelFlowCountForGas = 0
        /// HourlyFuelFlowCountForOil = 0
        /// LmeHiMethod = null
        /// 
        /// 
        /// Constant Output Parameters:
        /// 
        /// CurrentDhvFormulaRecord Id = (Result == null and FormulaId == GoodId ? GoodId : null)
        /// CurrentDhvMultipleFuelEquationCode = null
        /// DerivedHourlyEquationStatus = false
        /// DerivedHourlyFormulaStatus = (Result == null)
        /// 
        /// 
        /// 
        /// 
        /// | ## | FormulaId | Parameter | Method | ModcCd | NoxrMeth || Result || Note
        /// |  0 | null      | CO2C      | null   | [all]  | null     || [note] || Null formula: Result is null if MODC is 40, MODC in 01, 02, 03, 04, 05, 14, 21, 22, 53, 54 then C, otherwise K.
        /// |  1 | null      | H2O       | MDF    | [all]  | null     || [note] || Null formula: Result is null if MODC is 40, MODC in 01, 02, 03, 04, 05, 14, 21, 22, 53, 54 then C, otherwise C.
        /// |  2 | null      | H2O       | MMD    | [all]  | null     || [note] || Null formula: Result is null if MODC is 40, MODC in 01, 02, 03, 04, 05, 14, 21, 22, 53, 54 then C, otherwise C.
        /// |  3 | null      | H2O       | MTB    | [all]  | null     || [note] || Null formula: Result is null if MODC is 40, MODC in 01, 02, 03, 04, 05, 14, 21, 22, 53, 54 then C, otherwise C.
        /// |  4 | null      | H2O       | MWD    | [all]  | null     || [note] || Null formula: Result is null if MODC is 40, MODC in 01, 02, 03, 04, 05, 14, 21, 22, 53, 54 then C, otherwise C.
        /// |  5 | null      | NOXR      | CEM    | [all]  | CEM      || [note] || Null formula: Result is null if MODC is 40, MODC in 01, 02, 03, 04, 05, 14, 21, 22, 53, 54 then C, otherwise C.
        /// |  6 | BadId     | CO2C      | null   | [all]  | null     || E      || Formula Id without Record
        /// |  7 | BadId     | H2O       | MDF    | [all]  | null     || [note] || Formula Id without Record: Result is D if MODC is 40, otherwise E.
        /// |  8 | BadId     | H2O       | MMD    | [all]  | null     || [note] || Formula Id without Record: Result is D if MODC is 40, otherwise E.
        /// |  9 | BadId     | H2O       | MTB    | [all]  | null     || [note] || Formula Id without Record: Result is D if MODC is 40, otherwise E.
        /// | 10 | BadId     | H2O       | MWD    | [all]  | null     || [note] || Formula Id without Record: Result is D if MODC is 40, otherwise E.
        /// | 11 | BadId     | NOXR      | CEM    | [all]  | CEM      || E      || Formula Id without Record
        /// | 12 | GoodId    | CO2C      | null   | [all]  | null     || null   || Formual Id with Record
        /// | 13 | GoodId    | H2O       | MDF    | [all]  | null     || [note] || Formual Id with Record: Result is D if MODC is 40, otherwise null.
        /// | 14 | GoodId    | H2O       | MMD    | [all]  | null     || [note] || Formual Id with Record: Result is D if MODC is 40, otherwise null.
        /// | 15 | GoodId    | H2O       | MTB    | [all]  | null     || [note] || Formual Id with Record: Result is D if MODC is 40, otherwise null.
        /// | 16 | GoodId    | H2O       | MWD    | [all]  | null     || [note] || Formual Id with Record: Result is D if MODC is 40, otherwise null.
        /// | 17 | GoodId    | NOXR      | CEM    | [all]  | CEM      || null   || Formual Id with Record
        /// </summary>
        [TestMethod()]
        public void HourDhv24_Co2cOrH2oOrNoxrCem()
        {
            /* Initialize objects generally needed for testing checks. */
            cHourlyDerivedValueChecks target = new cHourlyDerivedValueChecks(new cEmissionsCheckParameters());
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;


            /* MODC Lists */
            string[] modcAll = UnitTestStandardLists.ModcCodeList;

            /* Result Actions */
            Func<string, string> resultActionCOrKOrNull = (modc) => modc == "40" ? null : (modc.InList("01,02,03,04,05,14,21,22,53,54") ? "C" : "K");
            Func<string, string> resultActionDOrE = (modc) => modc == "40" ? "D" : "E";
            Func<string, string> resultActionDOrNull = (modc) => modc == "40" ? "D" : null;
            Func<string, string> resultActionE = (modc) => "E";
            Func<string, string> resultActionNull = (modc) => null;


            /* Input Parameter Values */
            string[] formulaIdList = { null, null, null, null, null, null, "BadId", "BadId", "BadId", "BadId",
                                       "BadId", "BadId", "GoodId", "GoodId", "GoodId", "GoodId", "GoodId", "GoodId" };
            string[] parameterList = { "CO2C", "H2O", "H2O", "H2O", "H2O", "NOXR", "CO2C", "H2O", "H2O", "H2O",
                                       "H2O", "NOXR", "CO2C", "H2O", "H2O", "H2O", "H2O", "NOXR" };
            string[] methodList = { null, "MDF", "MMD", "MTB", "MWD", "CEM", null, "MDF", "MMD", "MTB",
                                    "MWD", "CEM", null, "MDF", "MMD", "MTB", "MWD", "CEM" };
            string[] noxrMethList = { null, null, null, null, null, "CEM", null, null, null, null,
                                      null, "CEM", null, null, null, null, null, "CEM" };

            /* Expected Values */
            Func<string, string>[] expResultActionList = { resultActionCOrKOrNull, resultActionCOrKOrNull, resultActionCOrKOrNull, resultActionCOrKOrNull, resultActionCOrKOrNull,
                                                           resultActionCOrKOrNull, resultActionE, resultActionDOrE, resultActionDOrE, resultActionDOrE,
                                                           resultActionDOrE, resultActionE, resultActionNull, resultActionDOrNull, resultActionDOrNull,
                                                           resultActionDOrNull, resultActionDOrNull, resultActionNull };

            /* Test Case Count */
            int caseCount = 18;

            /* Check array lengths */
            Assert.AreEqual(caseCount, formulaIdList.Length, "formulaIdList length");
            Assert.AreEqual(caseCount, methodList.Length, "methodList length");
            Assert.AreEqual(caseCount, parameterList.Length, "parameterList length");
            Assert.AreEqual(caseCount, noxrMethList.Length, "noxrMethList length");
            Assert.AreEqual(caseCount, expResultActionList.Length, "expResultActionList length");


            /* Run Cases */
            for (int caseDex = 0; caseDex < caseCount; caseDex++)
                foreach (string modcCd in modcAll)
                {
                    /* Initialize Input Parameters */
                    EmParameters.CurrentDhvMethod = methodList[caseDex];
                    EmParameters.CurrentDhvParameter = parameterList[caseDex];
                    EmParameters.CurrentDhvRecord = new VwMpDerivedHrlyValueRow(modcCd: modcCd, monFormId: formulaIdList[caseDex]);
                    EmParameters.DerivedHourlyModcStatus = true;
                    EmParameters.LegacyDataEvaluation = false;
                    EmParameters.MonitorFormulaRecordsByHourLocation = new CheckDataView<VwMpMonitorFormulaRow>
                        (
                           new VwMpMonitorFormulaRow(monFormId: "OneId", parameterCd: "Param1", equationCd: "F-12"),
                           new VwMpMonitorFormulaRow(monFormId: "GoodId", parameterCd: parameterList[caseDex], equationCd: "F-13"),
                           new VwMpMonitorFormulaRow(monFormId: "ThreeId", parameterCd: "Param3", equationCd: "F-14")
                        );

                    /* Initialize Optional Parameters */
                    EmParameters.AppEConstantFuelMix = false;
                    EmParameters.CurrentNoxrMethodCode = noxrMethList[caseDex];
                    EmParameters.HourlyFuelFlowCountForGas = 0;
                    EmParameters.HourlyFuelFlowCountForOil = 0;
                    EmParameters.LmeHiMethod = null;

                    /* Initialize Output Parameters */
                    EmParameters.CurrentDhvFormulaRecord = new VwMpMonitorFormulaRow(monFormId: "BadId", parameterCd: "BadPar", equationCd: "BadEq");
                    EmParameters.CurrentDhvMultipleFuelEquationCode = "BAD";
                    EmParameters.DerivedHourlyEquationStatus = null;
                    EmParameters.DerivedHourlyFormulaStatus = null;


                    /* Init Cateogry Result */
                    category.CheckCatalogResult = null;

                    /* Initialize variables needed to run the check. */
                    bool log = false;

                    /* Run Check */
                    string actual = target.HOURDHV24(category, ref log);
                    string result = expResultActionList[caseDex](modcCd);

                    /* Check Results */
                    Assert.AreEqual(string.Empty, actual, string.Format("actual [case {0}, param {1}, meth {2}, modc {3}, formId {4}]", caseDex, parameterList[caseDex], methodList[caseDex], modcCd, formulaIdList[caseDex]));
                    Assert.AreEqual(false, log, string.Format("log [case {0}, param {1}, meth {2}, modc {3}, formId {4}]", caseDex, parameterList[caseDex], methodList[caseDex], modcCd, formulaIdList[caseDex]));

                    Assert.AreEqual(result, category.CheckCatalogResult, string.Format("CheckCatalogResult [case {0}, param {1}, meth {2}, modc {3}, formId {4}]", caseDex, parameterList[caseDex], methodList[caseDex], modcCd, formulaIdList[caseDex]));

                    Assert.AreEqual(result.IsEmpty() && formulaIdList[caseDex] == "GoodId" ? "GoodId" : null, EmParameters.CurrentDhvFormulaRecord != null ? EmParameters.CurrentDhvFormulaRecord.MonFormId : null, string.Format("CurrentDhvFormulaRecord [case {0}, param {1}, meth {2}, modc {3}, formId {4}]", caseDex, parameterList[caseDex], methodList[caseDex], modcCd, formulaIdList[caseDex]));
                    Assert.AreEqual(null, EmParameters.CurrentDhvMultipleFuelEquationCode, string.Format("CurrentDhvMultipleFuelEquationCode [case {0}, param {1}, meth {2}, modc {3}, formId {4}]", caseDex, parameterList[caseDex], methodList[caseDex], modcCd, formulaIdList[caseDex]));
                    Assert.AreEqual(false, EmParameters.DerivedHourlyEquationStatus, string.Format("DerivedHourlyEquationStatus [case {0}, param {1}, meth {2}, modc {3}, formId {4}]", caseDex, parameterList[caseDex], methodList[caseDex], modcCd, formulaIdList[caseDex]));
                    Assert.AreEqual((result == null || result == "K"), EmParameters.DerivedHourlyFormulaStatus, string.Format("DerivedHourlyFormulaStatus [case {0}, param {1}, meth {2}, modc {3}, formId {4}]", caseDex, parameterList[caseDex], methodList[caseDex], modcCd, formulaIdList[caseDex]));
                }
        }


        /// <summary>
        /// 
        /// Input Parameters:
        /// 
        ///     Current DHV Method                          : AE, AMS, CMS, PEM
        ///     Current DHV Record                          : { {ModcCode: [ModcCodeList] } }
        ///     Current NOx Rate Method Code                : [Current DHV Method]
        ///     DerivedHourlyFormulaRecord                  : { null, { EquationCode: [EquationCodeList] } }
        ///     Derived Hourly Formula Status               : true, false
        ///     FC Factor Needed                            : false
        ///     FD Factor Needed                            : false
        ///     FW Factor Needed                            : false
        ///     H2O Missing Data Approach                   : TEST
        ///     Moisture Needed                             : false
        /// 
        /// Output Parameters:
        /// 
        ///     CO2 Diluent Checks Needed for NOx Rate Calc : Current DHV Method = "CEM" and Equation Code in set { 19-6, 19-7, 19-8, 19-9, F-6 }
        ///     Derived Hourly Equation Status              : Current DHV Method = "CEM" and Equation Code in set { 19-1, 19-2, 19-3, 19-3D, 19-4, 19-5, 19-5D, 19-6, 19-7, 19-8, 19-9, F-5, F-6 } or
        ///                                                   Current DHV Method = "AE" and Equation Code equals E-2, or 
        ///                                                   Current DHV Method = "AMS" or "PEM".
        ///     FC Factor Needed                            : Current DHV Method = "CEM", MODC in {01, 02, 03, 04, 05, 14, 21, 22, 53, 54} and Equation Code in set { 19-6, 19-7, 19-8, 19-9, F-6 }
        ///     FD Factor Needed                            : Current DHV Method = "CEM", MODC in {01, 02, 03, 04, 05, 14, 21, 22, 53, 54} and Equation Code in set { 19-1, 19-3, 19-3D, 19-4, 19-5, 19-5D, F-5 }
        ///     FW Factor Needed                            : Current DHV Method = "CEM", MODC in {01, 02, 03, 04, 05, 14, 21, 22, 53, 54} and Equation Code in set { 19-2 }
        ///     H2O Missing Data Approach                   : If Equation Code in set { 19-3, 19-3D, 19-4, 19-8 } then TESTMAX, If Equation Code in set { 19-5, 19-9 } then TESTMIN, otherwise TEST.
        ///     NOx Rate Equation Code                      : [DerivedHourlyFormulaRecord.EquationCode]
        ///     O2 Dry Checks Needed for NOx Rate Calc      : Current DHV Method = "CEM" and Equation Code in set { 19-1, 19-4, F-5 }
        ///     O2 Wet Checks Needed for NOx Rate Calc      : Current DHV Method = "CEM" and Equation Code in set { 19-2, 19-3, 19-5 }
        ///     Moisture Needed                             : Current DHV Method = "CEM" and Equation Code in set { 19-3, 19-3D, 19-4, 19-5, 19-8, 19-9 }
        /// 
        /// </summary>
        [TestMethod()]
        public void HourDhv27()
        {
            /* Initialize objects generally needed for testing checks. */
            cHourlyDerivedValueChecks target = new cHourlyDerivedValueChecks(new cEmissionsCheckParameters());
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;


            foreach (string methodCd in new string[] { "AE", "AMS", "CEM", "PEM" })
                foreach (string modcCd in UnitTestStandardLists.ModcCodeList)
                    foreach (string equationCd in new string[] { null, "19-1", "19-2", "19-3", "19-3D", "19-4", "19-5", "19-5D", "19-6", "19-7", "19-8", "19-9", "E-2", "F-5", "F-6", "NS-1", "NS-2" })
                    {
                        bool? status = true;

                        /* Initialize Required Parameters */
                        EmParameters.Co2DiluentChecksNeededForNoxRateCalc = null;
                        EmParameters.CurrentDhvFormulaRecord = new VwMpMonitorFormulaRow(equationCd: equationCd);
                        EmParameters.CurrentDhvMethod = methodCd;
                        EmParameters.CurrentDhvParameter = "NOXR";
                        EmParameters.CurrentDhvRecord = new VwMpDerivedHrlyValueRow(modcCd: modcCd);
                        EmParameters.DerivedHourlyEquationStatus = false;
                        EmParameters.DerivedHourlyFormulaStatus = true;
                        EmParameters.O2DryChecksNeededForNoxRateCalc = false;
                        EmParameters.O2WetChecksNeededForNoxRateCalc = false;

                        /* Initialize Optional Parameters */
                        EmParameters.FcFactorNeeded = false;
                        EmParameters.FdFactorNeeded = false;
                        EmParameters.FwFactorNeeded = false;
                        EmParameters.H2oMissingDataApproach = "TEST";
                        EmParameters.MoistureNeeded = false;
                        EmParameters.NoxcMonitorHourlyCount = 0;  // This does not seem to be used.  May be used for check leveling.

                        /* Initialize Output Parameters */
                        EmParameters.HourlyFuelFlowNeededForNoxRateCalc = null;
                        EmParameters.NoxRateEquationCode = "BAD";


                        /* Init Cateogry Result */
                        category.CheckCatalogResult = null;

                        /* Initialize variables needed to run the check. */
                        bool log = false;

                        /* Run Check */
                        string actual = target.HOURDHV27(category, ref log);

                        bool? co2dNeeded = (methodCd == "CEM" && modcCd != "23" && equationCd.InList("19-6,19-7,19-8,19-9,F-6"));
                        bool? equationStatus = (methodCd == "CEM" && equationCd.InList("19-1,19-2,19-3,19-3D,19-4,19-5,19-5D,19-6,19-7,19-8,19-9,F-5,F-6")) ||
                                               (methodCd == "AE" && equationCd == "E-2") ||
                                               methodCd.InList("AMS,PEM");
                        bool? fcFactorNeeded = (methodCd == "CEM" && modcCd.InList("01,02,03,04,05,14,21,22,53,54") && equationCd.InList("19-6,19-7,19-8,19-9,F-6"));
                        bool? fdFactorNeeded = (methodCd == "CEM" && modcCd.InList("01,02,03,04,05,14,21,22,53,54") && equationCd.InList("19-1,19-3,19-3D,19-4,19-5,19-5D,F-5"));
                        bool? fwFactorNeeded = (methodCd == "CEM" && modcCd.InList("01,02,03,04,05,14,21,22,53,54") && equationCd.InList("19-2"));
                        string h2oMdApproach = methodCd != "CEM" ? "TEST" :  modcCd != "23" && equationCd.InList("19-3,19-3D,19-4,19-8") ? "TEST,MAX" : modcCd != "23" && equationCd.InList("19-5,19-9") ? "TEST,MIN" : "TEST";
                        bool? h2odNeeded = (methodCd == "CEM" && modcCd != "23" && equationCd.InList("19-3,19-3D,19-4,19-5,19-8,19-9"));
                        bool? hourlyFFNeeded = (methodCd == "AE");
                        bool? o2dNeeded = (methodCd == "CEM" && modcCd != "23" && equationCd.InList("19-1,19-4,F-5"));
                        bool? o2wNeeded = (methodCd == "CEM" && modcCd != "23" && equationCd.InList("19-2,19-3,19-5"));

                        string result = equationStatus == true ? null : equationCd == null ? "A" : methodCd == "CEM" ? "B" : "C";

                        /* Check Results */
                        Assert.AreEqual(string.Empty, actual, string.Format("actual [methodCd {0}, modcCd {1}, equationCd {2}, status {3}]", methodCd, modcCd, equationCd, status));
                        Assert.AreEqual(false, log, string.Format("log  [methodCd {0}, modcCd {1}, equationCd {2}, status {3}]", methodCd, modcCd, equationCd, status));

                        Assert.AreEqual(result, category.CheckCatalogResult, string.Format("CheckCatalogResult  [methodCd {0}, modcCd {1}, equationCd {2}, status {3}]", methodCd, modcCd, equationCd, status));

                        Assert.AreEqual(co2dNeeded, EmParameters.Co2DiluentChecksNeededForNoxRateCalc, string.Format("Co2DiluentChecksNeededForNoxRateCalc  [methodCd {0}, modcCd {1}, equationCd {2}, status {3}]", methodCd, modcCd, equationCd, status));
                        Assert.AreEqual(equationStatus, EmParameters.DerivedHourlyEquationStatus, string.Format("DerivedHourlyEquationStatus  [methodCd {0}, modcCd {1}, equationCd {2}, status {3}]", methodCd, modcCd, equationCd, status));
                        Assert.AreEqual(fcFactorNeeded, EmParameters.FcFactorNeeded, string.Format("FcFactorNeeded  [methodCd {0}, modcCd {1}, equationCd {2}, status {3}]", methodCd, modcCd, equationCd, status));
                        Assert.AreEqual(fdFactorNeeded, EmParameters.FdFactorNeeded, string.Format("FdFactorNeeded  [methodCd {0}, modcCd {1}, equationCd {2}, status {3}]", methodCd, modcCd, equationCd, status));
                        Assert.AreEqual(fwFactorNeeded, EmParameters.FwFactorNeeded, string.Format("FwFactorNeeded  [methodCd {0}, modcCd {1}, equationCd {2}, status {3}]", methodCd, modcCd, equationCd, status));
                        Assert.AreEqual(h2oMdApproach, EmParameters.H2oMissingDataApproach, string.Format("H2oMissingDataApproach  [methodCd {0}, modcCd {1}, equationCd {2}, status {3}]", methodCd, modcCd, equationCd, status));
                        Assert.AreEqual(hourlyFFNeeded, EmParameters.HourlyFuelFlowNeededForNoxRateCalc, string.Format("HourlyFuelFlowNeededForNoxRateCalc  [methodCd {0}, modcCd {1}, equationCd {2}, status {3}]", methodCd, modcCd, equationCd, status));
                        Assert.AreEqual(h2odNeeded, EmParameters.MoistureNeeded, string.Format("MoistureNeeded  [methodCd {0}, modcCd {1}, equationCd {2}, status {3}]", methodCd, modcCd, equationCd, status));
                        Assert.AreEqual(equationCd != null ? equationCd : "", EmParameters.NoxRateEquationCode, string.Format("NoxRateEquationCode  [methodCd {0}, modcCd {1}, equationCd {2}, status {3}]", methodCd, modcCd, equationCd, status));
                        Assert.AreEqual(o2dNeeded, EmParameters.O2DryChecksNeededForNoxRateCalc, string.Format("O2DryChecksNeededForNoxRateCalc  [methodCd {0}, modcCd {1}, equationCd {2}, status {3}]", methodCd, modcCd, equationCd, status));
                        Assert.AreEqual(o2wNeeded, EmParameters.O2WetChecksNeededForNoxRateCalc, string.Format("O2WetChecksNeededForNoxRateCalc  [methodCd {0}, modcCd {1}, equationCd {2}, status {3}]", methodCd, modcCd, equationCd, status));
                    }
        }

        #endregion


        #region Checks 31-40

        /// <summary>
        /// 
        /// 
        /// Required Parameters:
        /// 
        ///     CurrentDhvMethod            : NOX: AMS, CEM, CEMNOXR, NOXR;
        ///                                   NOXR: AE, AMS, CEM, PEM
        ///     CurrentDhvParameter         : NOX, NOXR (based on check categories)
        ///     CurrentDhvRecord            : null (not used in check)
        ///     DerivedHourlyEquationStatus : null, false, true
        ///     DerivedHourlyModcStatus     : null, false, true
        ///     
        /// Optional Parameters:
        /// 
        ///     NoxConcMonitorHourlyCount   : 0, 1
        ///     NoxMassEquationCode         : F-24A, F-26A, F-26B, SS-2A, SS-2B, SS-2C
        ///     
        /// Output Parameters:
        /// 
        ///     NoxConcNeededForNoxMassCalc : If Parameter is NOX and Method in {CEM, CEMNOXR}, then equals EquationStatus is true and EquationCode begins with "F-26" and HourlyCount is not zero.
        ///                                   Otherwise equals null.
        ///     NOxConcNeededForNoxRateCalc : If Parameter is NOXR and Metod in {CEM, CEMNOXR}, then equals ModcStatus is true and HourlyCount is not zero.
        ///                                   Otherwise equals null.
        ///     
        /// Result:
        /// 
        ///     NoxConcNeededForNoxMassCalc : If Method not in {CEM, CEMNOXR} then return null.
        ///                                   Otherwise if Parameter is NOX, EquationStatus is true, EquationCode begins with "F-26" and HourlyCount is zero then return "A".
        ///                                   Otherwise if Parameter is NOXR, ModcStatus is true and HourlyCount is zero then return "A".
        ///                                   Otherwise return null.
        ///     
        /// </summary>
        [TestMethod()]
        public void HourDhv32()
        {
            /* Initialize objects generally needed for testing checks. */
            cHourlyDerivedValueChecks target = new cHourlyDerivedValueChecks(new cEmissionsCheckParameters());
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;


            foreach (string parameterCd in new string[] { "NOX", "NOXR" })
                foreach (string methodCd in (parameterCd == "NOX" ? new string[] { "AMS", "CEM", "CEMNOXR", "NOXR" } : new string[] { "AE", "AMS", "CEM", "PEM" }))
                    foreach (bool? equationStatus in UnitTestStandardLists.ValidList)
                        foreach (bool? modcStatus in UnitTestStandardLists.ValidList)
                            foreach (int hourlyCount in new int[] { 0, 1 })
                                foreach (string equationCd in (equationStatus == true ? new string[] { "F-24A", "F-26A", "F-26B", "SS-2A", "SS-2B", "SS-2C" } : new string[] { null }))
                                {
                                    string[] modcList = (modcStatus == true) ? UnitTestStandardLists.ModcCodeList : new string[] { "00" };

                                    foreach (string modcCd in modcList)
                                    {

                                        /* Initialize Required Parameters */
                                        EmParameters.CurrentDhvMethod = methodCd;
                                        EmParameters.CurrentDhvParameter = parameterCd;
                                        EmParameters.CurrentDhvRecord = new VwMpDerivedHrlyValueRow(modcCd: modcCd);
                                        EmParameters.DerivedHourlyEquationStatus = equationStatus;
                                        EmParameters.DerivedHourlyModcStatus = modcStatus;

                                        /* Initialize Optional Parameters */
                                        EmParameters.NoxcMonitorHourlyCount = hourlyCount;
                                        EmParameters.NoxMassEquationCode = equationCd;

                                        /* Initialize Output Parameters */
                                        EmParameters.NoxConcNeededForNoxMassCalc = null;
                                        EmParameters.NoxConcNeededForNoxRateCalc = null;


                                        /* Init Cateogry Result */
                                        category.CheckCatalogResult = null;

                                        /* Initialize variables needed to run the check. */
                                        bool log = false;

                                        /* Run Check */
                                        string actual = target.HOURDHV32(category, ref log);

                                        bool? expForNoxm = (parameterCd == "NOX") && methodCd.InList("CEM,CEMNOXR") ? (equationStatus == true) && equationCd.StartsWith("F-26") && (hourlyCount != 0) : (bool?)null;
                                        bool? expForNoxr = (parameterCd == "NOXR") && methodCd.InList("CEM,CEMNOXR") ? (modcStatus == true) && (hourlyCount != 0) : (bool?)null;

                                        string result = methodCd.NotInList("CEM,CEMNOXR") ? null
                                                      : (parameterCd == "NOX") && (equationStatus == true) && equationCd.StartsWith("F-26") && (hourlyCount == 0) ? "A"
                                                      : (parameterCd == "NOXR") && (modcStatus == true) && (hourlyCount == 0) ? (modcCd.InList("01,02,03,04,14,21,22,53,54") ? "A" :(modcCd != "23" ? "C" : null))
                                                      : null;

                                        /* Check Results */
                                        Assert.AreEqual(string.Empty, actual, string.Format("actual [parameterCd {0}, methodCd {1}, equationStatus {2}, modcStatus {3}, modcCd {6}, hourlyCount {4}, equationCd {5}]", parameterCd, methodCd, equationStatus, modcStatus, hourlyCount, equationCd, modcCd));
                                        Assert.AreEqual(false, log, string.Format("log  [parameterCd {0}, methodCd {1}, equationStatus {2}, modcStatus {3}, modcCd {6}, hourlyCount {4}, equationCd {5}]", parameterCd, methodCd, equationStatus, modcStatus, hourlyCount, equationCd, modcCd));

                                        Assert.AreEqual(result, category.CheckCatalogResult, string.Format("CheckCatalogResult [parameterCd {0}, methodCd {1}, equationStatus {2}, modcStatus {3}, modcCd {6}, hourlyCount {4}, equationCd {5}]", parameterCd, methodCd, equationStatus, modcStatus, hourlyCount, equationCd, modcCd));

                                        Assert.AreEqual(expForNoxm, EmParameters.NoxConcNeededForNoxMassCalc, string.Format("NoxConcNeededForNoxMassCalc [parameterCd {0}, methodCd {1}, equationStatus {2}, modcStatus {3}, modcCd {6}, hourlyCount {4}, equationCd {5}]", parameterCd, methodCd, equationStatus, modcStatus, hourlyCount, equationCd, modcCd));
                                        Assert.AreEqual(expForNoxr, EmParameters.NoxConcNeededForNoxRateCalc, string.Format("NoxConcNeededForNoxRateCalc [parameterCd {0}, methodCd {1}, equationStatus {2}, modcStatus {3}, modcCd {6}, hourlyCount {4}, equationCd {5}]", parameterCd, methodCd, equationStatus, modcStatus, hourlyCount, equationCd, modcCd));
                                    }
                                }
        }

        #endregion

    }
}