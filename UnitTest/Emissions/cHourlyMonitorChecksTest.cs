using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Data.Ecmps.CheckEm.Function;
using ECMPS.Checks.EmissionsChecks;
using ECMPS.Checks.EmissionsReport;
using ECMPS.Definitions.Extensions;

using ECMPS.Checks.Data.Ecmps.Dbo.View;
using ECMPS.Checks.Em.Parameters;

using UnitTest.UtilityClasses;

namespace UnitTest.Emissions
{
    [TestClass()]
    public class cHourlyMonitorValueChecksTest
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

        #region HOURMHV-8

        /// <summary>
        ///A test for HOURMHV-8_MODCParams.  Currently only includes H2O, which need a test method like the methods for CO2C, NOXC, O2D and O2W.
        ///</summary>()
        [TestMethod()]
        public void HOURMHV8_MODCParams()
        {
            //instantiated checks setup
            cHourlyMonitorValueChecks target = new cHourlyMonitorValueChecks(new cEmissionsCheckParameters());
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            // Variables
            bool log = false;
            string actual;


            //H2O Pass
            {
                // Init Input
                EmParameters.CurrentMhvParameter = "H2O";
                EmParameters.CurrentMhvRecord = new VwMpMonitorHrlyValueRow(modcCd: "55");
                EmParameters.CurrentH2oMonitorHourlyRecord = new VwMpMonitorHrlyValueH2oRow(modcCd: "55");

                // Init Output
                category.CheckCatalogResult = null;

                // Run Checks
                actual = target.HOURMHV8(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result [H2O 55 55]");
                Assert.AreEqual("55", EmParameters.H2oMhvModc, "H2oMhvModc");
                Assert.AreEqual(true, EmParameters.MonitorHourlyModcStatus, "MonitorHourlyModcStatus,");
            }
        }
        #endregion


        /// <summary>
        /// 
        /// Required Parameter Values:
        /// 
        ///     CurrentMhvParameter: CO2C
        /// 
        /// Optional Parameter Values:
        /// 
        ///     Co2ConcChecksNeededForCo2MassCalc: [Co2m]
        ///     Co2ConcChecksNeededForHeatInput: [Hi]
        ///     Co2DiluentChecksNeededForNoxRateCalc: [Noxr]
        ///     Co2DiluentNeededForMats: [Mats]
        ///     Co2FuelSpecificMissingData: [Co2Fsmd]
        ///     CurrentCo2ConcMonitorHourlyRecord.ModcCd: {All}
        ///     CurrentCo2ConcMonitorHourlyRecord.MoistureBasis: null
        ///     HeatInputFuelSpecificMissingData: [HiFsmd]
        ///     
        /// Output Parameter Values:
        /// 
        ///     CompleteMhvRecordNeeded: [Complete]
        ///     CurrentMhvComponentType: CO2
        ///     CurrentMhvDefaultParameter: CO2X
        ///     CurrentMhvFuelSpecificHour: [FsHr]
        ///     CurrentMhvParameterDescription: CO2C
        ///     CurrentMhvRecord: {CurrentCo2ConcMonitorHourly}
        ///     CurrentMhvSystemType: CO2
        ///     MonitorHourlyModcStatus: {Result is null or E}
        ///     Co2cMhvModc: {CurrentCo2ConcMonitorHourly.ModcCd}
        ///     
        /// Result Actions ([Modc], [Noxr], [Mats]):
        /// 
        ///     Co2cOrHi        : Result is 'A' if MODC is not in {01, 02, 03, 04, 06, 07, 08, 09, 10, 12, 17, 18, 20, 21, 23, 53, 54, 55}, 'E' if ([Noxr] or [MATS]) and MODC not in {01, 02,  03, 04, 17, 20, 21, 53, 54}), and null otherwise.
        ///     NotCo2cHiOrNoxr : Result is null if MODC is in {01, 02, 03, 04, 17, 18, 20, 21, 53, 54}, 'F' is MODC is 46, and 'C' otherwise.
        ///     NoxrOnly        : Result is null if MODC is in {01, 02, 03, 04, 17, 18, 20, 21, 46, 53, 54} and 'C' otherwise.
        ///     
        /// 
        /// | ## | Co2m  | HI    | Noxr  | Mats  | Co2Fsmd | HiFsmd | MdExists || Result             | Complete | FsHr  || Note
        /// |  0 | null  | null  | null  | null  | null    | null   | true     || [NotCo2mHiOrNoxr]  | false    | false || Not needed. 
        /// |  1 | false | false | false | false | false   | false  | true     || [NotCo2mHiOrNoxr]  | false    | false || Not needed. 
        /// |  2 | false | false | false | false | true    | true   | true     || [NotCo2mHiOrNoxr]  | false    | false || Not needed. 
        /// |  3 | false | false | false | true  | false   | false  | true     || [NotCo2mHiOrNoxr]  | false    | false || Needed for MATS.
        /// |  4 | false | false | false | true  | true    | true   | true     || [NotCo2mHiOrNoxr]  | false    | false || Needed for MATS.
        /// |  5 | false | false | true  | false | false   | false  | true     || [NoxrOnly]         | false    | false || Needed for NOXR.
        /// |  6 | false | false | true  | false | true    | true   | true     || [NoxrOnly]         | false    | false || Needed for NOXR.
        /// |  7 | false | false | true  | true  | false   | false  | true     || [NotCo2mHiOrNoxr]  | false    | false || Needed for NOXR and MATS.
        /// |  8 | false | false | true  | true  | true    | true   | true     || [NotCo2mHiOrNoxr]  | false    | false || Needed for NOXR and MATS.
        /// |  9 | false | true  | false | false | false   | false  | true     || [Co2mOrHi]         | true     | false || Needed for HI.
        /// | 10 | false | true  | false | false | false   | true   | true     || [Co2mOrHi]         | true     | true  || Needed for HI.
        /// | 11 | false | true  | false | false | true    | false  | true     || [Co2mOrHi]         | true     | false || Needed for HI.
        /// | 12 | false | true  | false | false | true    | true   | true     || [Co2mOrHi]         | true     | true  || Needed for HI.
        /// | 13 | false | true  | false | true  | false   | false  | true     || [Co2mOrHi]         | true     | false || Needed for HI and MATS.
        /// | 14 | false | true  | false | true  | true    | true   | true     || [Co2mOrHi]         | true     | true  || Needed for HI and MATS.
        /// | 15 | false | true  | true  | false | false   | false  | true     || [Co2mOrHi]         | true     | false || Needed for HI and NOXR.
        /// | 16 | false | true  | true  | false | true    | true   | true     || [Co2mOrHi]         | true     | true  || Needed for HI and NOXR.
        /// | 17 | false | true  | true  | true  | false   | false  | true     || [Co2mOrHi]         | true     | false || Needed for HI, NOXR and MATS.
        /// | 18 | false | true  | true  | true  | true    | true   | true     || [Co2mOrHi]         | true     | true  || Needed for HI, NOXR and MATS.
        /// | 19 | true  | false | false | false | false   | false  | true     || [Co2mOrHi]         | true     | false || Needed for CO2C.
        /// | 20 | true  | false | false | false | false   | true   | true     || [Co2mOrHi]         | true     | false || Needed for CO2C.
        /// | 21 | true  | false | false | false | true    | false  | true     || [Co2mOrHi]         | true     | true  || Needed for CO2C.
        /// | 22 | true  | false | false | false | true    | true   | true     || [Co2mOrHi]         | true     | true  || Needed for CO2C.
        /// | 23 | true  | false | false | true  | false   | false  | true     || [Co2mOrHi]         | true     | false || Needed for CO2C and MATS.
        /// | 24 | true  | false | false | true  | true    | true   | true     || [Co2mOrHi]         | true     | true  || Needed for CO2C and MATS.
        /// | 25 | true  | false | true  | false | false   | false  | true     || [Co2mOrHi]         | true     | false || Needed for CO2C and NOXR.
        /// | 26 | true  | false | true  | false | true    | true   | true     || [Co2mOrHi]         | true     | true  || Needed for CO2C and NOXR.
        /// | 27 | true  | false | true  | true  | false   | false  | true     || [Co2mOrHi]         | true     | false || Needed for CO2C, NOXR and MATS.
        /// | 28 | true  | false | true  | true  | true    | true   | true     || [Co2mOrHi]         | true     | true  || Needed for CO2C, NOXR and MATS.
        /// | 29 | true  | true  | false | false | false   | false  | true     || [Co2mOrHi]         | true     | false || Needed for CO2M and HI.
        /// | 30 | true  | true  | false | false | false   | true   | true     || [Co2mOrHi]         | true     | true  || Needed for CO2M and HI.
        /// | 31 | true  | true  | false | false | true    | false  | true     || [Co2mOrHi]         | true     | true  || Needed for CO2M and HI.
        /// | 32 | true  | true  | false | false | true    | true   | true     || [Co2mOrHi]         | true     | true  || Needed for CO2M and HI.
        /// | 33 | true  | true  | false | true  | false   | false  | true     || [Co2mOrHi]         | true     | false || Needed for CO2M, HI and MATS.
        /// | 34 | true  | true  | false | true  | true    | true   | true     || [Co2mOrHi]         | true     | true  || Needed for CO2M, HI and MATS.
        /// | 35 | true  | true  | true  | false | false   | false  | true     || [Co2mOrHi]         | true     | false || Needed for CO2M, HI and NOXR.
        /// | 36 | true  | true  | true  | false | true    | true   | true     || [Co2mOrHi]         | true     | true  || Needed for CO2M, HI and NOXR.
        /// | 37 | true  | true  | true  | true  | false   | false  | true     || [Co2mOrHi]         | true     | false || Needed for CO2M, HI, NOXR and MATS.
        /// | 38 | true  | true  | true  | true  | true    | true   | true     || [Co2mOrHi]         | true     | true  || Needed for CO2M, HI, NOXR and MATS.
        /// | 39 | false | true  | false | false | false   | false  | false    || [Co2mOrHi]         | true     | false || Needed for HI, but no missing data record.
        /// | 40 | false | true  | false | true  | false   | false  | false    || [Co2mOrHi]         | true     | false || Needed for HI and MATS, but no missing data record.
        /// | 41 | false | true  | true  | false | false   | false  | false    || [Co2mOrHi]         | true     | false || Needed for HI and NOXR, but no missing data record.
        /// | 42 | false | true  | true  | true  | false   | false  | false    || [Co2mOrHi]         | true     | false || Needed for HI, NOXR and MATS, but no missing data record.
        /// | 43 | true  | false | false | false | false   | false  | false    || [Co2mOrHi]         | true     | false || Needed for CO2C, but no missing data record.
        /// | 44 | true  | false | false | true  | false   | false  | false    || [Co2mOrHi]         | true     | false || Needed for CO2C and MATS, but no missing data record.
        /// | 45 | true  | false | true  | false | false   | false  | false    || [Co2mOrHi]         | true     | false || Needed for CO2C and NOXR, but no missing data record.
        /// | 46 | true  | false | true  | true  | false   | false  | false    || [Co2mOrHi]         | true     | false || Needed for CO2C, NOXR and MATS, but no missing data record.
        /// | 47 | true  | true  | false | false | false   | false  | false    || [Co2mOrHi]         | true     | false || Needed for CO2M and HI, but no missing data record.
        /// | 48 | true  | true  | false | true  | false   | false  | false    || [Co2mOrHi]         | true     | false || Needed for CO2M, HI and MATS, but no missing data record.
        /// | 49 | true  | true  | true  | false | false   | false  | false    || [Co2mOrHi]         | true     | false || Needed for CO2M, HI and NOXR, but no missing data record.
        /// | 50 | true  | true  | true  | true  | false   | false  | false    || [Co2mOrHi]         | true     | false || Needed for CO2M, HI, NOXR and MATS, but no missing data record.
        /// 
        /// </summary>
        [TestMethod()]
        public void HourMhv8_Co2c()
        {
            /* Initialize objects generally needed for testing checks. */
            cHourlyMonitorValueChecks target = new cHourlyMonitorValueChecks(new cEmissionsCheckParameters());
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;


            /* MODC Lists */
            string[] modcAll = UnitTestStandardLists.ModcCodeList;

            /* Result Actions */
            Func<string, bool?, bool?, bool, string> co2cOrHi = (modc, noxr, mats, md) => modc.NotInList("01,02,03,04,06,07,08,09,10,12,17,20,21,23,53,54,55") 
                                                                                          ? "A"
                                                                                          : md && (noxr.Default(false) || mats.Default(false)) && modc.NotInList("01,02,03,04,17,20,21,53,54")
                                                                                            ? "E"
                                                                                            : null;
            Func<string, bool?, bool?, bool, string> notCo2mHiOrNoxr = (modc, noxr, mats, md) => modc.InList("01,02,03,04,17,20,21,53,54") ? null : modc == "46" ? null : "C";
            Func<string, bool?, bool?, bool, string> noxrOnly = (modc, noxr, mats, md) => modc.InList("01,02,03,04,17,20,21,46,53,54") ? null : "C";

            /* Input Parameter Values */
            bool?[] co2mList = { null, false, false, false, false, false, false, false, false, false,
                                 false, false, false, false, false, false, false, false, false, true,
                                 true, true, true, true, true, true, true, true, true, true,
                                 true, true, true, true, true, true, true, true, true, false,
                                 false, false, false, true, true, true, true, true, true, true,
                                 true};
            bool?[] hiList = { null, false, false, false, false, false, false, false, false, true,
                               true, true, true, true, true, true, true, true, true, false,
                               false, false, false, false, false, false, false, false, false, true,
                               true, true, true, true, true, true, true, true, true, true,
                               true, true, true, false, false, false, false, false, false, false, 
                               false };
            bool?[] NoxrList = { null, false, false, false, false, true, true, true, true, false,
                                 false, false, false, false, false, true, true, true, true, false,
                                 false, false, false, false, false, true, true, true, true, false,
                                 false, false, false, false, false, true, true, true, true, false, 
                                 false, true, true, false, false, true, true, false, false, true, 
                                 true };
            bool?[] matsList = { null, false, false, true, true, false, false, true, true, false,
                                 false, false, false, true, true, false, false, true, true, false,
                                 false, false, false, true, true, false, false, true, true, false,
                                 false, false, false, true, true, false, false, true, true, false, 
                                 true, false, true, false, true, false, true, false, true, false, 
                                 true  };
            bool?[] co2FsmdList = { null, false, true, false, true, false, true, false, true, false,
                                    false, true, true, false, true, false, true, false, true, false,
                                    false, true, true, false, true, false, true, false, true, false,
                                    false, true, true, false, true, false, true, false, true, false,
                                    false, false, false, false, false, false, false, false, false, false,
                                    false };
            bool?[] hiFsmdList = { null, false, true, false, true, false, true, false, true, false,
                                   true, false, true, false, true, false, true, false, true, false,
                                   true, false, true, false, true, false, true, false, true, false,
                                   true, false, true, false, true, false, true, false, true, false,
                                   false, false, false, false, false, false, false, false, false, false,
                                   false  };
            bool[] mdExists = { true, true, true, true, true, true, true, true, true, true,
                                true, true, true, true, true, true, true, true, true, true,
                                true, true, true, true, true, true, true, true, true, true,
                                true, true, true, true, true, true, true, true, true, false,
                                false, false, false, false, false, false, false, false, false, false,
                                false  };

            /* Expected Values */
            Func<string, bool?, bool?, bool, string>[] expResultActionList = { notCo2mHiOrNoxr, notCo2mHiOrNoxr, notCo2mHiOrNoxr, notCo2mHiOrNoxr, notCo2mHiOrNoxr,
                                                                               noxrOnly, noxrOnly, notCo2mHiOrNoxr, notCo2mHiOrNoxr, co2cOrHi,
                                                                               co2cOrHi, co2cOrHi, co2cOrHi, co2cOrHi, co2cOrHi,
                                                                               co2cOrHi, co2cOrHi, co2cOrHi, co2cOrHi, co2cOrHi,
                                                                               co2cOrHi, co2cOrHi, co2cOrHi, co2cOrHi, co2cOrHi,
                                                                               co2cOrHi, co2cOrHi, co2cOrHi, co2cOrHi, co2cOrHi,
                                                                               co2cOrHi, co2cOrHi, co2cOrHi, co2cOrHi, co2cOrHi,
                                                                               co2cOrHi, co2cOrHi, co2cOrHi, co2cOrHi, co2cOrHi,
                                                                               co2cOrHi, co2cOrHi, co2cOrHi, co2cOrHi, co2cOrHi,
                                                                               co2cOrHi, co2cOrHi, co2cOrHi, co2cOrHi, co2cOrHi,
                                                                               co2cOrHi};
            bool?[] expCompleteList = { false, false, false, false, false, false, false, false, false, true,
                                        true, true, true, true, true, true, true, true, true, true,
                                        true, true, true, true, true, true, true, true, true, true,
                                        true, true, true, true, true, true, true, true, true, true,
                                        true, true, true, true, true, true, true, true, true, true,
                                        true };
            bool?[] expFsHrList = { false, false, false, false, false, false, false, false, false, false,
                                    true, false, true, false, true, false, true, false, true, false,
                                    false, true, true, false, true, false, true, false, true, false,
                                    true, true, true, false, true, false, true, false, true, false,
                                    false, false, false, false, false, false, false, false, false, false,
                                    false};

            /* Test Case Count */
            int caseCount = 51;

            /* Check array lengths */
            Assert.AreEqual(caseCount, co2mList.Length, "co2mList length");
            Assert.AreEqual(caseCount, hiList.Length, "hiList length");
            Assert.AreEqual(caseCount, NoxrList.Length, "NoxrList length");
            Assert.AreEqual(caseCount, matsList.Length, "matsList length");
            Assert.AreEqual(caseCount, co2FsmdList.Length, "co2FsmdList length");
            Assert.AreEqual(caseCount, hiFsmdList.Length, "hiFsmdList length");
            Assert.AreEqual(caseCount, mdExists.Length, "mdExists length");
            Assert.AreEqual(caseCount, expResultActionList.Length, "expResultActionList length");
            Assert.AreEqual(caseCount, expCompleteList.Length, "expCompleteList length");
            Assert.AreEqual(caseCount, expFsHrList.Length, "expFsHrList length");


            /* Run Cases */
            for (int caseDex = 0; caseDex < caseCount; caseDex++)
                foreach (string modcCd in modcAll)
                {
                    /* Initialize Input Parameters */
                    EmParameters.CurrentMhvParameter = "CO2C";

                    /* Initialize Optional Parameters */
                    EmParameters.Co2ConcChecksNeededForCo2MassCalc = co2mList[caseDex];
                    EmParameters.Co2ConcChecksNeededForHeatInput = hiList[caseDex];
                    EmParameters.Co2DiluentChecksNeededForNoxRateCalc = NoxrList[caseDex];
                    EmParameters.Co2DiluentNeededForMats = matsList[caseDex];
                    EmParameters.Co2FuelSpecificMissingData = co2FsmdList[caseDex];
                    EmParameters.CurrentCo2ConcMissingDataMonitorHourlyRecord = (mdExists[caseDex] ? new VwMpMonitorHrlyValueCo2cRow(parameterCd: EmParameters.CurrentMhvParameter) : null );
                    EmParameters.CurrentCo2ConcMonitorHourlyRecord = new VwMpMonitorHrlyValueCo2cRow(modcCd: modcCd, moistureBasis: null);
                    EmParameters.HeatInputFuelSpecificMissingData = hiFsmdList[caseDex];

                    /* Initialize Output Parameters */
                    EmParameters.CompleteMhvRecordNeeded = null;
                    EmParameters.CurrentMhvComponentType = "BAD";
                    EmParameters.CurrentMhvDefaultParameter = "BAD";
                    EmParameters.CurrentMhvFuelSpecificHour = null;
                    EmParameters.CurrentMhvParameterDescription = null;
                    EmParameters.CurrentMhvRecord = new VwMpMonitorHrlyValueRow(modcCd: "BAD");
                    EmParameters.CurrentMhvSystemType = "BAD";
                    EmParameters.MonitorHourlyModcStatus = null;
                    EmParameters.NoxConcModc = "BAD";


                    /* Init Cateogry Result */
                    category.CheckCatalogResult = null;

                    /* Initialize variables needed to run the check. */
                    bool log = false;

                    /* Run Check */
                    string actual = target.HOURMHV8(category, ref log);
                    string result = expResultActionList[caseDex](modcCd, NoxrList[caseDex], matsList[caseDex], mdExists[caseDex]);

                    /* Check Results */
                    Assert.AreEqual(string.Empty, actual, string.Format("actual [case {0}, modcCd {1}, CO2M {2}, HI {3}, NOXR {4}, MATS {5}, CO2 FSMD {6}, HI FSMD {7}]", caseDex, modcCd, co2mList[caseDex], hiList[caseDex], NoxrList[caseDex], matsList[caseDex], co2FsmdList[caseDex], hiFsmdList[caseDex]));
                    Assert.AreEqual(false, log, string.Format("log [case {0}, modcCd {1}, CO2M {2}, HI {3}, NOXR {4}, MATS {5}, CO2 FSMD {6}, HI FSMD {7}]", caseDex, modcCd, co2mList[caseDex], hiList[caseDex], NoxrList[caseDex], matsList[caseDex], co2FsmdList[caseDex], hiFsmdList[caseDex]));

                    Assert.AreEqual(result, category.CheckCatalogResult, string.Format("CheckCatalogResult [case {0}, modcCd {1}, CO2M {2}, HI {3}, NOXR {4}, MATS {5}, CO2 FSMD {6}, HI FSMD {7}]", caseDex, modcCd, co2mList[caseDex], hiList[caseDex], NoxrList[caseDex], matsList[caseDex], co2FsmdList[caseDex], hiFsmdList[caseDex]));

                    Assert.AreEqual(expCompleteList[caseDex], EmParameters.CompleteMhvRecordNeeded, string.Format("CompleteMhvRecordNeeded [case {0}, modcCd {1}, CO2M {2}, HI {3}, NOXR {4}, MATS {5}, CO2 FSMD {6}, HI FSMD {7}]", caseDex, modcCd, co2mList[caseDex], hiList[caseDex], NoxrList[caseDex], matsList[caseDex], co2FsmdList[caseDex], hiFsmdList[caseDex]));
                    Assert.AreEqual("CO2", EmParameters.CurrentMhvComponentType, string.Format("CurrentMhvComponentType [case {0}, modcCd {1}, CO2M {2}, HI {3}, NOXR {4}, MATS {5}, CO2 FSMD {6}, HI FSMD {7}]", caseDex, modcCd, co2mList[caseDex], hiList[caseDex], NoxrList[caseDex], matsList[caseDex], co2FsmdList[caseDex], hiFsmdList[caseDex]));
                    Assert.AreEqual("CO2X", EmParameters.CurrentMhvDefaultParameter, string.Format("CurrentMhvDefaultParameter [case {0}, modcCd {1}, CO2M {2}, HI {3}, NOXR {4}, MATS {5}, CO2 FSMD {6}, HI FSMD {7}]", caseDex, modcCd, co2mList[caseDex], hiList[caseDex], NoxrList[caseDex], matsList[caseDex], co2FsmdList[caseDex], hiFsmdList[caseDex]));
                    Assert.AreEqual(expFsHrList[caseDex], EmParameters.CurrentMhvFuelSpecificHour, string.Format("CurrentMhvFuelSpecificHour [case {0}, modcCd {1}, CO2M {2}, HI {3}, NOXR {4}, MATS {5}, CO2 FSMD {6}, HI FSMD {7}]", caseDex, modcCd, co2mList[caseDex], hiList[caseDex], NoxrList[caseDex], matsList[caseDex], co2FsmdList[caseDex], hiFsmdList[caseDex]));
                    Assert.AreEqual("CO2C", EmParameters.CurrentMhvParameterDescription, string.Format("CurrentMhvParameterDescription [case {0}, modcCd {1}, CO2M {2}, HI {3}, NOXR {4}, MATS {5}, CO2 FSMD {6}, HI FSMD {7}]", caseDex, modcCd, co2mList[caseDex], hiList[caseDex], NoxrList[caseDex], matsList[caseDex], co2FsmdList[caseDex], hiFsmdList[caseDex]));
                    Assert.AreEqual(EmParameters.CurrentCo2ConcMonitorHourlyRecord.ModcCd, EmParameters.CurrentMhvRecord.ModcCd, string.Format("CurrentMhvRecord [case {0}, modcCd {1}, CO2M {2}, HI {3}, NOXR {4}, MATS {5}, CO2 FSMD {6}, HI FSMD {7}]", caseDex, modcCd, co2mList[caseDex], hiList[caseDex], NoxrList[caseDex], matsList[caseDex], co2FsmdList[caseDex], hiFsmdList[caseDex]));
                    Assert.AreEqual("CO2", EmParameters.CurrentMhvSystemType, string.Format("CurrentMhvSystemType [case {0}, modcCd {1}, CO2M {2}, HI {3}, NOXR {4}, MATS {5}, CO2 FSMD {6}, HI FSMD {7}]", caseDex, modcCd, co2mList[caseDex], hiList[caseDex], NoxrList[caseDex], matsList[caseDex], co2FsmdList[caseDex], hiFsmdList[caseDex]));
                    Assert.AreEqual((result == null || result == "E"), EmParameters.MonitorHourlyModcStatus, string.Format("MonitorHourlyModcStatus [case {0}, modcCd {1}, CO2M {2}, HI {3}, NOXR {4}, MATS {5}, CO2 FSMD {6}, HI FSMD {7}]", caseDex, modcCd, co2mList[caseDex], hiList[caseDex], NoxrList[caseDex], matsList[caseDex], co2FsmdList[caseDex], hiFsmdList[caseDex]));

                    Assert.AreEqual(EmParameters.CurrentCo2ConcMonitorHourlyRecord.ModcCd, EmParameters.Co2cMhvModc, string.Format("NoxConcModc [case {0}, modcCd {1}, CO2M {2}, HI {3}, NOXR {4}, MATS {5}, CO2 FSMD {6}, HI FSMD {7}]", caseDex, modcCd, co2mList[caseDex], hiList[caseDex], NoxrList[caseDex], matsList[caseDex], co2FsmdList[caseDex], hiFsmdList[caseDex]));
                }
        }


        /// <summary>
        /// 
        /// Required Parameter Values:
        /// 
        ///     CurrentMhvParameter: NOXC
        /// 
        /// Optional Parameter Values:
        /// 
        ///     CurrentNoxConcMonitorHourlyRecord.ModcCd: {All}
        ///     CurrentNoxConcMonitorHourlyRecord.MoistureBasis: null
        ///     NoxConcNeededForNoxMassCalc: [Noxm]
        ///     NoxConcNeededForNoxRateCalc: [Noxr]
        ///     NoxMassByassCode: [ByPass]
        ///     NoxMassFuelSpecificMissingData: [Fsmd]
        ///     
        /// Output Parameter Values:
        /// 
        ///     CompleteMhvRecordNeeded: [Complete]
        ///     CurrentMhvComponentType: NOX
        ///     CurrentMhvDefaultParameter: NOCX
        ///     CurrentMhvFuelSpecificHour: {FshrAction}
        ///     CurrentMhvParameterDescription: CO2C
        ///     CurrentMhvRecord: {CurrentNoxConcMonitorHourlyRecord}
        ///     CurrentMhvSystemType: NOXC
        ///     MonitorHourlyModcStatus: {Result is null}
        ///     NoxConcModc: {CurrentNoxConcMonitorHourlyRecord.ModcCd}
        ///     
        /// Result Actions ([Modc], [Noxr]):
        /// 
        ///     ForNoxm       : Result is 'A' if MODC is not in {01, 02, 03, 04, 05, 06, 07, 08, 09, 10, 11, 12, 13, 15, 17, 18, 19, 20, 21, 22, 23, 24, 53, 54, 55} and null otherwise.
        ///     NotNoxmOrNoxr : Result is null if MODC is in {01, 02, 03, 04, 17, 18, 19, 20, 21, 22, 53, 54}, 'F' is MODC is 46, and 'B' otherwise.
        ///     NoxrOnly      : Result is null if MODC is in {01, 02, 03, 04, 17, 18, 19, 20, 21, 22, 46, 53, 54} and 'B' otherwise.
        /// 
        /// Expected MODC Actions ([Modc]):
        /// 
        ///     ModcNull  : null
        ///     ModcValue : MODC when in {01, 02, 03, 04, 17, 18, 19, 20, 21, 22, 53, 54}, otherwise null.
        /// 
        /// Expected FSHR Actions ([Modc], [Fsmd], [ByPass]):
        /// 
        ///     FshrAction  : When MODC is 23 or 24 then ByPass == "BYMAXFS" else FSMD.
        ///     
        /// 
        /// | ## | Noxm  | Noxr  | fsmd   | Bypass  || Result          | Complete | Modc        || Note
        /// |  0 | null  | null  | null   | null    || [NotNoxmOrNoxr] | false    | [ModcValue] || Not needed. 
        /// |  1 | false | false | false  | null    || [NotNoxmOrNoxr] | false    | [ModcValue] || Not needed. 
        /// |  2 | false | false | true   | null    || [NotNoxmOrNoxr] | false    | [ModcValue] || Not needed. 
        /// |  3 | false | true  | false  | null    || [NoxrOnly]      | false    | [ModcValue] || Needed for NOXR.
        /// |  4 | false | true  | false  | BYMAX   || [NoxrOnly]      | false    | [ModcValue] || Needed for NOXR.
        /// |  5 | false | true  | false  | BYMAXFS || [NoxrOnly]      | false    | [ModcValue] || Needed for NOXR.
        /// |  6 | false | true  | true   | null    || [NoxrOnly]      | false    | [ModcValue] || Needed for NOXR.
        /// |  7 | false | true  | true   | BYMAX   || [NoxrOnly]      | false    | [ModcValue] || Needed for NOXR.
        /// |  8 | false | true  | true   | BYMAXFS || [NoxrOnly]      | false    | [ModcValue] || Needed for NOXR.
        /// |  9 | true  | false | false  | null    || [ForNoxm]       | true     | [ModcNull]  || Needed for NOXM.
        /// | 10 | true  | false | false  | BYMAX   || [ForNoxm]       | true     | [ModcNull]  || Needed for NOXM.
        /// | 11 | true  | false | false  | BYMAXFS || [ForNoxm]       | true     | [ModcNull]  || Needed for NOXM.
        /// | 12 | true  | false | true   | null    || [ForNoxm]       | true     | [ModcNull]  || Needed for NOXM.
        /// | 13 | true  | false | true   | BYMAX   || [ForNoxm]       | true     | [ModcNull]  || Needed for NOXM.
        /// | 14 | true  | false | true   | BYMAXFS || [ForNoxm]       | true     | [ModcNull]  || Needed for NOXM.
        /// | 15 | true  | true  | false  | null    || [ForNoxm]       | true     | [ModcNull]  || Needed for Both.
        /// | 16 | true  | true  | true   | null    || [ForNoxm]       | true     | [ModcNull]  || Needed for Both.
        /// 
        /// </summary>
        [TestMethod()]
        public void HourMhv8_Noxc()
        {
            /* Initialize objects generally needed for testing checks. */
            cHourlyMonitorValueChecks target = new cHourlyMonitorValueChecks(new cEmissionsCheckParameters());
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;


            /* MODC Lists */
            string[] modcAll = UnitTestStandardLists.ModcCodeList;

            /* Result Actions */
            Func<string, bool?, string> forNoxm = (modc, noxr) => modc.NotInList("01,02,03,04,05,06,07,08,09,10,11,12,13,15,17,18,19,20,21,22,23,24,53,54,55") ? "A" : null;
            Func<string, bool?, string> notNoxmOrNoxr = (modc, noxr) => modc.InList("01,02,03,04,17,18,19,20,21,22,53,54") ? null : modc == "46" ? null : "B";
            Func<string, bool?, string> noxrOnly = (modc, noxr) => modc.InList("01,02,03,04,17,18,19,20,21,22,46,53,54") ? null : "B";

            /* Expected MODC Actions */
            Func<string, string> modcNull = (modc) => null;
            Func<string, string> modcValue = (modc) => modc.InList("01,02,03,04,17,18,19,20,21,22,53,54") ? modc : null;

            /* Expected MODC Actions */
            Func<string, bool?, string, bool?> fshrAction = (modc, fsmd, byPass) => modc.InList("23,24") ? (byPass == "BYMAXFS") : fsmd.Default(false);


            /* Input Parameter Values */
            bool?[] noxmList = { null, false, false, false, false, false, false, false, false, true, true, true, true, true, true, true, true };
            bool?[] noxrList = { null, false, false, true, true, true, true, true, true, false, false, false, false, false, false, true, true };
            bool?[] fsmdList = { null, false, true, false, false, false, true, true, true, false, false, false, true, true, true, false, true };
            string[] bypassList = { null, null, null, null, "BYMAX", "BYMAXFS", null, "BYMAX", "BYMAXFS", null, "BYMAX", "BYMAXFS", null, "BYMAX", "BYMAXFS", null, null };

            /* Expected Values */
            Func<string, bool?, string>[] expResultActionList = { notNoxmOrNoxr, notNoxmOrNoxr, notNoxmOrNoxr,
                                                                  noxrOnly, noxrOnly, noxrOnly, noxrOnly, noxrOnly, noxrOnly,
                                                                  forNoxm, forNoxm, forNoxm, forNoxm, forNoxm, forNoxm, forNoxm, forNoxm };
            bool?[] expCompleteList = { false, false, false, false, false, false, false, false, false, true, true, true, true, true, true, true, true };
            Func<string, string>[] expModcActionList = { modcValue, modcValue, modcValue, modcValue, modcValue, modcValue, modcValue, modcValue, modcValue,
                                                         modcNull, modcNull, modcNull, modcNull, modcNull, modcNull, modcNull, modcNull };

            /* Test Case Count */
            int caseCount = 17;

            /* Check array lengths */
            Assert.AreEqual(caseCount, noxmList.Length, "co2mList length");
            Assert.AreEqual(caseCount, noxrList.Length, "noxrList length");
            Assert.AreEqual(caseCount, fsmdList.Length, "hiFsmdList length");
            Assert.AreEqual(caseCount, bypassList.Length, "bypassList length");
            Assert.AreEqual(caseCount, expResultActionList.Length, "expResultActionList length");
            Assert.AreEqual(caseCount, expCompleteList.Length, "expCompleteList length");
            Assert.AreEqual(caseCount, expModcActionList.Length, "expModcActionList length");


            /* Run Cases */
            for (int caseDex = 0; caseDex < caseCount; caseDex++)
                foreach (string modcCd in modcAll)
                {
                    /* Initialize Input Parameters */
                    EmParameters.CurrentMhvParameter = "NOXC";

                    /* Initialize Optional Parameters */
                    EmParameters.CurrentNoxConcMonitorHourlyRecord = new VwMpMonitorHrlyValueRow(modcCd: modcCd, moistureBasis: null);
                    EmParameters.NoxConcNeededForNoxMassCalc = noxmList[caseDex];
                    EmParameters.NoxConcNeededForNoxRateCalc = noxrList[caseDex];
                    EmParameters.NoxMassBypassCode = bypassList[caseDex];
                    EmParameters.NoxMassFuelSpecificMissingData = fsmdList[caseDex];

                    /* Initialize Output Parameters */
                    EmParameters.CompleteMhvRecordNeeded = null;
                    EmParameters.CurrentMhvComponentType = "BAD";
                    EmParameters.CurrentMhvDefaultParameter = "BAD";
                    EmParameters.CurrentMhvFuelSpecificHour = null;
                    EmParameters.CurrentMhvParameterDescription = null;
                    EmParameters.CurrentMhvRecord = new VwMpMonitorHrlyValueRow(modcCd: "BAD");
                    EmParameters.CurrentMhvSystemType = "BAD";
                    EmParameters.MonitorHourlyModcStatus = null;
                    EmParameters.NoxConcModc = "BAD";


                    /* Init Cateogry Result */
                    category.CheckCatalogResult = null;

                    /* Initialize variables needed to run the check. */
                    bool log = false;

                    /* Run Check */
                    string actual = target.HOURMHV8(category, ref log);
                    string result = expResultActionList[caseDex](modcCd, noxrList[caseDex]);
                    string expModc = expModcActionList[caseDex](modcCd);
                    bool? expFsHr = fshrAction(modcCd, fsmdList[caseDex], bypassList[caseDex]);

                    /* Check Results */
                    Assert.AreEqual(string.Empty, actual, string.Format("actual [case {0}, modcCd {1}, NOXM {2}, NOXR {3}, FSMD {4}]", caseDex, modcCd, noxmList[caseDex], noxrList[caseDex], fsmdList[caseDex]));
                    Assert.AreEqual(false, log, string.Format("log [case {0}, modcCd {1}, NOXM {2}, NOXR {3}, FSMD {4}]", caseDex, modcCd, noxmList[caseDex], noxrList[caseDex], fsmdList[caseDex]));

                    Assert.AreEqual(result, category.CheckCatalogResult, string.Format("CheckCatalogResult [case {0}, modcCd {1}, NOXM {2}, NOXR {3}, FSMD {4}]", caseDex, modcCd, noxmList[caseDex], noxrList[caseDex], fsmdList[caseDex]));

                    Assert.AreEqual(expCompleteList[caseDex], EmParameters.CompleteMhvRecordNeeded, string.Format("CompleteMhvRecordNeeded [case {0}, modcCd {1}, NOXM {2}, NOXR {3}, FSMD {4}]", caseDex, modcCd, noxmList[caseDex], noxrList[caseDex], fsmdList[caseDex]));
                    Assert.AreEqual("NOX", EmParameters.CurrentMhvComponentType, string.Format("CurrentMhvComponentType [case {0}, modcCd {1}, NOXM {2}, NOXR {3}, FSMD {4}]", caseDex, modcCd, noxmList[caseDex], noxrList[caseDex], fsmdList[caseDex]));
                    Assert.AreEqual("NOCX", EmParameters.CurrentMhvDefaultParameter, string.Format("CurrentMhvDefaultParameter [case {0}, modcCd {1}, NOXM {2}, NOXR {3}, FSMD {4}]", caseDex, modcCd, noxmList[caseDex], noxrList[caseDex], fsmdList[caseDex]));
                    Assert.AreEqual(expFsHr, EmParameters.CurrentMhvFuelSpecificHour, string.Format("CurrentMhvFuelSpecificHour [case {0}, modcCd {1}, NOXM {2}, NOXR {3}, FSMD {4}]", caseDex, modcCd, noxmList[caseDex], noxrList[caseDex], fsmdList[caseDex]));
                    Assert.AreEqual("NOXC", EmParameters.CurrentMhvParameterDescription, string.Format("CurrentMhvParameterDescription [case {0}, modcCd {1}, NOXM {2}, NOXR {3}, FSMD {4}]", caseDex, modcCd, noxmList[caseDex], noxrList[caseDex], fsmdList[caseDex]));
                    Assert.AreEqual(EmParameters.CurrentNoxConcMonitorHourlyRecord.ModcCd, EmParameters.CurrentMhvRecord.ModcCd, string.Format("CurrentMhvRecord [case {0}, modcCd {1}, NOXM {2}, NOXR {3}, FSMD {4}]", caseDex, modcCd, noxmList[caseDex], noxrList[caseDex], fsmdList[caseDex]));
                    Assert.AreEqual("NOXC", EmParameters.CurrentMhvSystemType, string.Format("CurrentMhvSystemType [case {0}, modcCd {1}, NOXM {2}, NOXR {3}, FSMD {4}]", caseDex, modcCd, noxmList[caseDex], noxrList[caseDex], fsmdList[caseDex]));
                    Assert.AreEqual((result == null), EmParameters.MonitorHourlyModcStatus, string.Format("MonitorHourlyModcStatus [case {0}, modcCd {1}, NOXM {2}, NOXR {3}, FSMD {4}]", caseDex, modcCd, noxmList[caseDex], noxrList[caseDex], fsmdList[caseDex]));

                    Assert.AreEqual(expModc, EmParameters.NoxConcModc, string.Format("NoxConcModc [case {0}, modcCd {1}, NOXM {2}, NOXR {3}, FSMD {4}]", caseDex, modcCd, noxmList[caseDex], noxrList[caseDex], fsmdList[caseDex]));
                }
        }


        /// <summary>
        /// 
        /// Required Parameter Values:
        /// 
        ///     CurrentMhvParameter: O2D
        /// 
        /// Optional Parameter Values:
        /// 
        ///     CurrentO2DryMonitorHourlyRecord.ModcCd: {All}
        ///     CurrentO2DryMonitorHourlyRecord.MoistureBasis: {All}
        ///     HeatInputFuelSpecificMissingData: [HiFsmd]
        ///     O2DryChecksNeededForH2o: [H2o]
        ///     O2DryChecksNeededForHeatInput: [Hi]
        ///     O2DryChecksNeededForNoxRateCalc: [Noxr]
        ///     O2DryChecksNeededToSupportCo2Calculation: [Co2c]
        ///     O2DryNeededForMats: [Mats]
        ///     
        /// Output Parameter Values:
        /// 
        ///     CompleteMhvRecordNeeded: [Complete]
        ///     CurrentMhvComponentType: O2
        ///     CurrentMhvDefaultParameter: O2N
        ///     CurrentMhvFuelSpecificHour: [FsHr]
        ///     CurrentMhvParameterDescription: {O2C if moisture basis in null otherwise "O2C with a MoistureBasis of Dry"}
        ///     CurrentMhvRecord: {CurrentO2DryMonitorHourlyRecord}
        ///     CurrentMhvSystemType: null
        ///     MonitorHourlyModcStatus: {Result is null or E}
        ///     O2DryModc: {CurrentO2DryMonitorHourlyRecord.ModcCd}
        ///     
        /// Result Actions ([Modc], [H2o], [Hi], [Noxr], [Co2c], [Mats]):
        /// 
        ///     HiNeeded                : Result is 'A' if MODC is not in {01, 02, 03, 04, 06, 07, 08, 09, 10, 12, 17, 20, 53, 54, 55}, 'E' if ([Noxr] or [MATS]) and MODC not in {01, 02, 03, 04, 17, 20, 53, 54}), and null otherwise.
        ///     NotNeededOrNotHiButMats : Result is null if MODC is in {01, 02, 03, 04, 17, 20, 53, 54}, 'G' if MODC is 46, and 'D' otherwise.
        ///     Co2cH2oAndNoxrOnly      : Result is null if MODC is in {01, 02, 03, 04, 17, 20, 46, 53, 54} and 'D' otherwise.
        ///     
        /// 
        /// | ## | H2o   | Hi    | Noxr  | Co2c  | Mats    | HiFsmd | MdExists || Result                    | Complete | FsHr  || Note
        /// |  0 | null  | null  | null  | null  | null    | null   | true     || [NotNeededOrNotHiButMats] | false    | false || Not needed. 
        /// |  1 | false | false | false | false | false   | false  | true     || [NotNeededOrNotHiButMats] | false    | false || Not needed. 
        /// |  2 | false | false | false | false | false   | true   | true     || [NotNeededOrNotHiButMats] | false    | false || Not needed. 
        /// |  3 | false | false | false | false | true    | false  | true     || [NotNeededOrNotHiButMats] | false    | false || MATS needed. 
        /// |  4 | false | false | false | false | true    | true   | true     || [NotNeededOrNotHiButMats] | false    | false || MATS needed. 
        /// |  5 | false | false | false | true  | false   | false  | true     || [Co2cH2oAndNoxrOnly]      | false    | false || CO2C, H2O or NOXR needed. 
        /// |  6 | false | false | false | true  | false   | true   | true     || [Co2cH2oAndNoxrOnly]      | false    | false || CO2C, H2O or NOXR needed.
        /// |  7 | false | false | false | true  | true    | false  | true     || [NotNeededOrNotHiButMats] | false    | false || CO2C, H2O or NOXR needed and MATS needed.
        /// |  8 | false | false | true  | false | false   | false  | true     || [Co2cH2oAndNoxrOnly]      | false    | false || CO2C, H2O or NOXR needed.
        /// |  9 | false | false | true  | false | false   | true   | true     || [Co2cH2oAndNoxrOnly]      | false    | false || CO2C, H2O or NOXR needed.
        /// | 10 | false | false | true  | false | true    | false  | true     || [NotNeededOrNotHiButMats] | false    | false || CO2C, H2O or NOXR needed and MATS needed.
        /// | 11 | false | false | true  | true  | false   | false  | true     || [Co2cH2oAndNoxrOnly]      | false    | false || CO2C, H2O or NOXR needed.
        /// | 12 | false | false | true  | true  | true    | false  | true     || [NotNeededOrNotHiButMats] | false    | false || CO2C, H2O or NOXR needed and MATS needed.
        /// | 13 | false | true  | false | false | false   | false  | true     || [HiNeeded]                | true     | false || HI needed.
        /// | 14 | false | true  | false | false | false   | true   | true     || [HiNeeded]                | true     | true  || HI needed.
        /// | 15 | false | true  | false | false | true    | false  | true     || [HiNeeded]                | true     | false || HI needed and MATS needed. 
        /// | 16 | false | true  | false | true  | false   | false  | true     || [HiNeeded]                | true     | false || HI needed and CO2C, H2O or NOXR needed. 
        /// | 17 | false | true  | false | true  | true    | false  | true     || [HiNeeded]                | true     | false || HI needed, CO2C, H2O or NOXR needed, and MATS needed.
        /// | 18 | false | true  | true  | false | false   | false  | true     || [HiNeeded]                | true     | false || HI needed and CO2C, H2O or NOXR needed. 
        /// | 19 | false | true  | true  | false | true    | false  | true     || [HiNeeded]                | true     | false || HI needed, CO2C, H2O or NOXR needed, and MATS needed.
        /// | 20 | false | true  | true  | true  | false   | false  | true     || [HiNeeded]                | true     | false || HI needed and CO2C, H2O or NOXR needed. 
        /// | 21 | false | true  | true  | true  | true    | false  | true     || [HiNeeded]                | true     | false || HI needed, CO2C, H2O or NOXR needed, and MATS needed.
        /// | 22 | true  | false | false | false | false   | false  | true     || [Co2cH2oAndNoxrOnly]      | false    | false || CO2C, H2O or NOXR needed.
        /// | 23 | true  | false | false | false | false   | true   | true     || [Co2cH2oAndNoxrOnly]      | false    | false || CO2C, H2O or NOXR needed.
        /// | 24 | true  | false | false | false | true    | false  | true     || [NotNeededOrNotHiButMats] | false    | false || CO2C, H2O or NOXR needed and MATS needed.
        /// | 25 | true  | false | false | true  | false   | false  | true     || [Co2cH2oAndNoxrOnly]      | false    | false || CO2C, H2O or NOXR needed.
        /// | 26 | true  | false | false | true  | true    | false  | true     || [NotNeededOrNotHiButMats] | false    | false || CO2C, H2O or NOXR needed and MATS needed.
        /// | 27 | true  | false | true  | false | false   | false  | true     || [Co2cH2oAndNoxrOnly]      | false    | false || CO2C, H2O or NOXR needed.
        /// | 28 | true  | false | true  | false | true    | false  | true     || [NotNeededOrNotHiButMats] | false    | false || CO2C, H2O or NOXR needed and MATS needed.
        /// | 29 | true  | false | true  | true  | false   | false  | true     || [Co2cH2oAndNoxrOnly]      | false    | false || CO2C, H2O or NOXR needed.
        /// | 30 | true  | false | true  | true  | true    | false  | true     || [NotNeededOrNotHiButMats] | false    | false || CO2C, H2O or NOXR needed and MATS needed.
        /// | 31 | true  | true  | false | false | false   | false  | true     || [HiNeeded]                | true     | false || HI needed and CO2C, H2O or NOXR needed. 
        /// | 32 | true  | true  | false | false | true    | false  | true     || [HiNeeded]                | true     | false || HI needed, CO2C, H2O or NOXR needed, and MATS needed.
        /// | 33 | true  | true  | false | true  | false   | false  | true     || [HiNeeded]                | true     | false || HI needed and CO2C, H2O or NOXR needed. 
        /// | 34 | true  | true  | false | true  | true    | false  | true     || [HiNeeded]                | true     | false || HI needed, CO2C, H2O or NOXR needed, and MATS needed.
        /// | 35 | true  | true  | true  | false | false   | false  | true     || [HiNeeded]                | true     | false || HI needed and CO2C, H2O or NOXR needed. 
        /// | 36 | true  | true  | true  | false | true    | false  | true     || [HiNeeded]                | true     | false || HI needed, CO2C, H2O or NOXR needed, and MATS needed.
        /// | 37 | true  | true  | true  | true  | false   | false  | true     || [HiNeeded]                | true     | false || HI needed and CO2C, H2O or NOXR needed. 
        /// | 38 | true  | true  | true  | true  | true    | false  | true     || [HiNeeded]                | true     | false || HI needed, CO2C, H2O or NOXR needed, and MATS needed.
        /// | 39 | false | true  | false | false | false   | false  | false    || [HiNeeded]                | true     | false || HI needed, but no missing data record.
        /// | 40 | false | true  | false | false | true    | false  | false    || [HiNeeded]                | true     | false || HI needed and MATS needed, but no missing data record. 
        /// | 41 | false | true  | true  | false | false   | false  | false    || [HiNeeded]                | true     | false || HI needed and CO2C, H2O or NOXR needed, but no missing data record. 
        /// | 42 | false | true  | true  | false | true    | false  | false    || [HiNeeded]                | true     | false || HI needed, CO2C, H2O or NOXR needed, and MATS needed, but no missing data record.
        /// 
        /// </summary>
        [TestMethod()]
        public void HourMhv8_O2cDry()
        {
            /* Initialize objects generally needed for testing checks. */
            cHourlyMonitorValueChecks target = new cHourlyMonitorValueChecks(new cEmissionsCheckParameters());
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;


            /* Code Lists */
            string[] modcCodeList = UnitTestStandardLists.ModcCodeList;
            string[] moistureBasisList = { null, "D" };

            /* Result Actions */
            Func<string, bool?, bool?, bool?, bool?, bool?, bool, string> hiNeeded = (modc, h2o, hi, noxr, co2c, mats, md) => modc.NotInList("01,02,03,04,06,07,08,09,10,12,17,20,53,54,55") ? "A" : md && (noxr.Default(false) || mats.Default(false)) && modc.NotInList("01,02,03,04,17,20,53,54") ? "E" : null;
            Func<string, bool?, bool?, bool?, bool?, bool?, bool, string> notNeededOrNotHiButMats = (modc, h2o, hi, noxr, co2c, mats, md) => modc.InList("01,02,03,04,17,20,53,54") ? null : modc == "46" ? null : "D";
            Func<string, bool?, bool?, bool?, bool?, bool?, bool, string> co2cH2oAndNoxrOnly = (modc, h2o, hi, noxr, co2c, mats, md) => modc.InList("01,02,03,04,17,20,46,53,54") ? null : "D";

            /* Input Parameter Values */
            bool?[] h2oList = { null, false, false, false, false, false, false, false, false, false,
                                false, false, false, false, false, false, false, false, false, false,
                                false, false, true, true, true, true, true, true, true, true,
                                true, true, true, true, true, true, true, true, true, false, 
                                false, false, false };
            bool?[] hiList = { null, false, false, false, false, false, false, false, false, false,
                               false, false, false, true, true, true, true, true, true, true,
                               true, true, false, false, false, false, false, false, false, false,
                               false, true, true, true, true, true, true, true, true, true, 
                               true, true, true };
            bool?[] noxrList = { null, false, false, false, false, false, false, false, true, true,
                                 true, true, true, false, false, false, false, false, true, true,
                                 true, true, false, false, false, false, false, true, true, true,
                                 true, false, false, false, false, true, true, true, true, false,
                                 false, true, true };
            bool?[] co2cList = { null, false, false, false, false, true, true, true, false, false,
                                 false, true, true, false, false, false, true, true, false, false,
                                 true, true, false, false, false, true, true, false, false, true,
                                 true, false, false, true, true, false, false, true, true, false, 
                                 false, false, false };
            bool?[] matsList = { null, false, false, true, true, false, false, true, false, false,
                                 true, false, true, false, false, true, false, true, false, true,
                                 false, true, false, false, true, false, true, false, true, false,
                                 true, false, true, false, true, false, true, false, true, false, 
                                 true, false, true };
            bool?[] hiFsmdList = { null, false, true, false, true, false, true, false, false, true,
                                   false, false, false, false, true, false, false, false, false, false,
                                   false, false, false, true, false, false, false, false, false, false,
                                   false, false, false, false, false, false, false, false, false, false, 
                                   false, false, false };
            bool[] mdExists = { true, true, true, true, true, true, true, true, true, true,
                                true, true, true, true, true, true, true, true, true, true,
                                true, true, true, true, true, true, true, true, true, true,
                                true, true, true, true, true, true, true, true, true, false, 
                                false, false, false };

            /* Expected Values */
            Func<string, bool?, bool?, bool?, bool?, bool?, bool, string>[] expResultActionList = { notNeededOrNotHiButMats, notNeededOrNotHiButMats, notNeededOrNotHiButMats, notNeededOrNotHiButMats, notNeededOrNotHiButMats,
                                                                                                    co2cH2oAndNoxrOnly, co2cH2oAndNoxrOnly, notNeededOrNotHiButMats, co2cH2oAndNoxrOnly, co2cH2oAndNoxrOnly,
                                                                                                    notNeededOrNotHiButMats, co2cH2oAndNoxrOnly, notNeededOrNotHiButMats, hiNeeded, hiNeeded,
                                                                                                    hiNeeded, hiNeeded, hiNeeded, hiNeeded, hiNeeded,
                                                                                                    hiNeeded, hiNeeded, co2cH2oAndNoxrOnly, co2cH2oAndNoxrOnly, notNeededOrNotHiButMats,
                                                                                                    co2cH2oAndNoxrOnly, notNeededOrNotHiButMats, co2cH2oAndNoxrOnly, notNeededOrNotHiButMats, co2cH2oAndNoxrOnly,
                                                                                                    notNeededOrNotHiButMats, hiNeeded, hiNeeded, hiNeeded, hiNeeded,
                                                                                                    hiNeeded, hiNeeded, hiNeeded, hiNeeded, hiNeeded, 
                                                                                                    hiNeeded, hiNeeded, hiNeeded};
            bool?[] expCompleteList = { false, false, false, false, false, false, false, false, false, false,
                                        false, false, false, true, true, true, true, true, true, true,
                                        true, true, false, false, false, false, false, false, false, false,
                                        false, true, true, true, true, true, true, true, true, true, 
                                        true, true, true };
            bool?[] expFsHrList = { false, false, false, false, false, false, false, false, false, false,
                                    false, false, false, false, true, false, false, false, false, false,
                                    false, false, false, false, false, false, false, false, false, false,
                                    false, false, false, false, false, false, false, false, false, false,
                                    false, false, false };

            /* Test Case Count */
            int caseCount = 43;

            /* Check array lengths */
            Assert.AreEqual(caseCount, h2oList.Length, "h2oList length");
            Assert.AreEqual(caseCount, hiList.Length, "hiList length");
            Assert.AreEqual(caseCount, noxrList.Length, "NoxrList length");
            Assert.AreEqual(caseCount, co2cList.Length, "co2cList length");
            Assert.AreEqual(caseCount, matsList.Length, "matsList length");
            Assert.AreEqual(caseCount, hiFsmdList.Length, "hiFsmdList length");
            Assert.AreEqual(caseCount, mdExists.Length, "mdExists length");
            Assert.AreEqual(caseCount, expResultActionList.Length, "expResultActionList length");
            Assert.AreEqual(caseCount, expCompleteList.Length, "expCompleteList length");
            Assert.AreEqual(caseCount, expFsHrList.Length, "expFsHrList length");


            /* Run Cases */
            for (int caseDex = 0; caseDex < caseCount; caseDex++)
                foreach (string modcCd in modcCodeList)
                    foreach (string moistureBasis in moistureBasisList)
                    {
                        /* Initialize Input Parameters */
                        EmParameters.CurrentMhvParameter = "O2D";

                        /* Initialize Optional Parameters */
                        EmParameters.CurrentO2DryMissingDataMonitorHourlyRecord = (mdExists[caseDex] ? new VwMpMonitorHrlyValueRow(parameterCd: "O2C", moistureBasis: "D") : null);
                        EmParameters.CurrentO2DryMonitorHourlyRecord = new VwMpMonitorHrlyValueO2DryRow(modcCd: modcCd, moistureBasis: moistureBasis);
                        EmParameters.HeatInputFuelSpecificMissingData = hiFsmdList[caseDex];
                        EmParameters.O2DryChecksNeededForH2o = h2oList[caseDex];
                        EmParameters.O2DryChecksNeededForHeatInput = hiList[caseDex];
                        EmParameters.O2DryChecksNeededForNoxRateCalc = noxrList[caseDex];
                        EmParameters.O2DryNeededToSupportCo2Calculation = co2cList[caseDex];
                        EmParameters.O2DryNeededForMats = matsList[caseDex];

                        /* Initialize Output Parameters */
                        EmParameters.CompleteMhvRecordNeeded = null;
                        EmParameters.CurrentMhvComponentType = "BAD";
                        EmParameters.CurrentMhvDefaultParameter = "BAD";
                        EmParameters.CurrentMhvFuelSpecificHour = null;
                        EmParameters.CurrentMhvParameterDescription = null;
                        EmParameters.CurrentMhvRecord = new VwMpMonitorHrlyValueRow(modcCd: "BAD");
                        EmParameters.CurrentMhvSystemType = "BAD";
                        EmParameters.MonitorHourlyModcStatus = null;
                        EmParameters.NoxConcModc = "BAD";


                        /* Init Cateogry Result */
                        category.CheckCatalogResult = null;

                        /* Initialize variables needed to run the check. */
                        bool log = false;

                        /* Run Check */
                        string actual = target.HOURMHV8(category, ref log);
                        string result = expResultActionList[caseDex](modcCd, h2oList[caseDex], hiList[caseDex], noxrList[caseDex], co2cList[caseDex], matsList[caseDex], mdExists[caseDex]);

                        /* Check Results */
                        Assert.AreEqual(string.Empty, actual, string.Format("actual [case {0}, modcCd {1}, H2O {2}, HI {3}, NOXR {4}, CO2C {5}, MATS {6}, HI FSMD {7}]", caseDex, modcCd, h2oList[caseDex], hiList[caseDex], noxrList[caseDex], co2cList[caseDex], matsList[caseDex], hiFsmdList[caseDex]));
                        Assert.AreEqual(false, log, string.Format("log  [case {0}, modcCd {1}, H2O {2}, HI {3}, NOXR {4}, CO2C {5}, MATS {6}, HI FSMD {7}]", caseDex, modcCd, h2oList[caseDex], hiList[caseDex], noxrList[caseDex], co2cList[caseDex], matsList[caseDex], hiFsmdList[caseDex]));

                        Assert.AreEqual(result, category.CheckCatalogResult, string.Format("CheckCatalogResult  [case {0}, modcCd {1}, H2O {2}, HI {3}, NOXR {4}, CO2C {5}, MATS {6}, HI FSMD {7}]", caseDex, modcCd, h2oList[caseDex], hiList[caseDex], noxrList[caseDex], co2cList[caseDex], matsList[caseDex], hiFsmdList[caseDex]));

                        Assert.AreEqual(expCompleteList[caseDex], EmParameters.CompleteMhvRecordNeeded, string.Format("CompleteMhvRecordNeeded  [case {0}, modcCd {1}, H2O {2}, HI {3}, NOXR {4}, CO2C {5}, MATS {6}, HI FSMD {7}]", caseDex, modcCd, h2oList[caseDex], hiList[caseDex], noxrList[caseDex], co2cList[caseDex], matsList[caseDex], hiFsmdList[caseDex]));
                        Assert.AreEqual("O2", EmParameters.CurrentMhvComponentType, string.Format("CurrentMhvComponentType  [case {0}, modcCd {1}, H2O {2}, HI {3}, NOXR {4}, CO2C {5}, MATS {6}, HI FSMD {7}]", caseDex, modcCd, h2oList[caseDex], hiList[caseDex], noxrList[caseDex], co2cList[caseDex], matsList[caseDex], hiFsmdList[caseDex]));
                        Assert.AreEqual("O2N", EmParameters.CurrentMhvDefaultParameter, string.Format("CurrentMhvDefaultParameter  [case {0}, modcCd {1}, H2O {2}, HI {3}, NOXR {4}, CO2C {5}, MATS {6}, HI FSMD {7}]", caseDex, modcCd, h2oList[caseDex], hiList[caseDex], noxrList[caseDex], co2cList[caseDex], matsList[caseDex], hiFsmdList[caseDex]));
                        Assert.AreEqual(expFsHrList[caseDex], EmParameters.CurrentMhvFuelSpecificHour, string.Format("CurrentMhvFuelSpecificHour  [case {0}, modcCd {1}, H2O {2}, HI {3}, NOXR {4}, CO2C {5}, MATS {6}, HI FSMD {7}]", caseDex, modcCd, h2oList[caseDex], hiList[caseDex], noxrList[caseDex], co2cList[caseDex], matsList[caseDex], hiFsmdList[caseDex]));
                        Assert.AreEqual(moistureBasis == null ? "O2C" : "O2C with a MoistureBasis of D", EmParameters.CurrentMhvParameterDescription, string.Format("CurrentMhvParameterDescription  [case {0}, modcCd {1}, H2O {2}, HI {3}, NOXR {4}, CO2C {5}, MATS {6}, HI FSMD {7}]", caseDex, modcCd, h2oList[caseDex], hiList[caseDex], noxrList[caseDex], co2cList[caseDex], matsList[caseDex], hiFsmdList[caseDex]));
                        Assert.AreEqual(EmParameters.CurrentO2DryMonitorHourlyRecord.ModcCd, EmParameters.CurrentMhvRecord.ModcCd, string.Format("CurrentMhvRecord  [case {0}, modcCd {1}, H2O {2}, HI {3}, NOXR {4}, CO2C {5}, MATS {6}, HI FSMD {7}]", caseDex, modcCd, h2oList[caseDex], hiList[caseDex], noxrList[caseDex], co2cList[caseDex], matsList[caseDex], hiFsmdList[caseDex]));
                        Assert.AreEqual(null, EmParameters.CurrentMhvSystemType, string.Format("CurrentMhvSystemType  [case {0}, modcCd {1}, H2O {2}, HI {3}, NOXR {4}, CO2C {5}, MATS {6}, HI FSMD {7}]", caseDex, modcCd, h2oList[caseDex], hiList[caseDex], noxrList[caseDex], co2cList[caseDex], matsList[caseDex], hiFsmdList[caseDex]));
                        Assert.AreEqual((result == null || result == "E"), EmParameters.MonitorHourlyModcStatus, string.Format("MonitorHourlyModcStatus  [case {0}, modcCd {1}, H2O {2}, HI {3}, NOXR {4}, CO2C {5}, MATS {6}, HI FSMD {7}]", caseDex, modcCd, h2oList[caseDex], hiList[caseDex], noxrList[caseDex], co2cList[caseDex], matsList[caseDex], hiFsmdList[caseDex]));

                        Assert.AreEqual(EmParameters.CurrentO2DryMonitorHourlyRecord.ModcCd, EmParameters.O2DryModc, string.Format("O2DryModc  [case {0}, modcCd {1}, H2O {2}, HI {3}, NOXR {4}, CO2C {5}, MATS {6}, HI FSMD {7}]", caseDex, modcCd, h2oList[caseDex], hiList[caseDex], noxrList[caseDex], co2cList[caseDex], matsList[caseDex], hiFsmdList[caseDex]));
                    }
        }


        /// <summary>
        /// 
        /// Required Parameter Values:
        /// 
        ///     CurrentMhvParameter: O2D
        /// 
        /// Optional Parameter Values:
        /// 
        ///     CurrentO2WetMonitorHourlyRecord.ModcCd: {All}
        ///     CurrentO2WetMonitorHourlyRecord.MoistureBasis: {All}
        ///     HeatInputFuelSpecificMissingData: [HiFsmd]
        ///     O2WetChecksNeededForH2o: [H2o]
        ///     O2WetChecksNeededForHeatInput: [Hi]
        ///     O2WetChecksNeededForNoxRateCalc: [Noxr]
        ///     O2WetChecksNeededToSupportCo2Calculation: [Co2c]
        ///     O2WetNeededForMats: [Mats]
        ///     
        /// Output Parameter Values:
        /// 
        ///     CompleteMhvRecordNeeded: [Complete]
        ///     CurrentMhvComponentType: O2
        ///     CurrentMhvDefaultParameter: O2N
        ///     CurrentMhvFuelSpecificHour: [FsHr]
        ///     CurrentMhvParameterDescription: {O2C if moisture basis in null otherwise "O2C with a MoistureBasis of Wet"}
        ///     CurrentMhvRecord: {CurrentO2WetMonitorHourlyRecord}
        ///     CurrentMhvSystemType: null
        ///     MonitorHourlyModcStatus: {Result is null or E}
        ///     O2WetModc: {CurrentO2WetMonitorHourlyRecord.ModcCd}
        ///     
        /// Result Actions ([Modc], [H2o], [Hi], [Noxr], [Co2c], [Mats]):
        /// 
        ///     HiNeeded                : Result is 'A' if MODC is not in {01, 02, 03, 04, 06, 07, 08, 09, 10, 12, 17, 20, 53, 54, 55}, 'E' if ([Noxr] or [MATS]) and MODC not in {01, 02, 03, 04, 17, 20, 53, 54}), and null otherwise.
        ///     NotNeededOrNotHiButMats : Result is null if MODC is in {01, 02, 03, 04, 17, 20, 53, 54}, 'G' if MODC is 46, and 'D' otherwise.
        ///     Co2cH2oAndNoxrOnly      : Result is null if MODC is in {01, 02, 03, 04, 17, 20, 46, 53, 54} and 'D' otherwise.
        ///     
        /// 
        /// | ## | H2o   | Hi    | Noxr  | Co2c  | Mats    | HiFsmd | MdExists || Result                    | Complete | FsHr  || Note
        /// |  0 | null  | null  | null  | null  | null    | null   | true     || [NotNeededOrNotHiButMats] | false    | false || Not needed. 
        /// |  1 | false | false | false | false | false   | false  | true     || [NotNeededOrNotHiButMats] | false    | false || Not needed. 
        /// |  2 | false | false | false | false | false   | true   | true     || [NotNeededOrNotHiButMats] | false    | false || Not needed. 
        /// |  3 | false | false | false | false | true    | false  | true     || [NotNeededOrNotHiButMats] | false    | false || MATS needed. 
        /// |  4 | false | false | false | false | true    | true   | true     || [NotNeededOrNotHiButMats] | false    | false || MATS needed. 
        /// |  5 | false | false | false | true  | false   | false  | true     || [Co2cH2oAndNoxrOnly]      | false    | false || CO2C, H2O or NOXR needed. 
        /// |  6 | false | false | false | true  | false   | true   | true     || [Co2cH2oAndNoxrOnly]      | false    | false || CO2C, H2O or NOXR needed.
        /// |  7 | false | false | false | true  | true    | false  | true     || [NotNeededOrNotHiButMats] | false    | false || CO2C, H2O or NOXR needed and MATS needed.
        /// |  8 | false | false | true  | false | false   | false  | true     || [Co2cH2oAndNoxrOnly]      | false    | false || CO2C, H2O or NOXR needed.
        /// |  9 | false | false | true  | false | false   | true   | true     || [Co2cH2oAndNoxrOnly]      | false    | false || CO2C, H2O or NOXR needed.
        /// | 10 | false | false | true  | false | true    | false  | true     || [NotNeededOrNotHiButMats] | false    | false || CO2C, H2O or NOXR needed and MATS needed.
        /// | 11 | false | false | true  | true  | false   | false  | true     || [Co2cH2oAndNoxrOnly]      | false    | false || CO2C, H2O or NOXR needed.
        /// | 12 | false | false | true  | true  | true    | false  | true     || [NotNeededOrNotHiButMats] | false    | false || CO2C, H2O or NOXR needed and MATS needed.
        /// | 13 | false | true  | false | false | false   | false  | true     || [HiNeeded]                | true     | false || HI needed.
        /// | 14 | false | true  | false | false | false   | true   | true     || [HiNeeded]                | true     | true  || HI needed.
        /// | 15 | false | true  | false | false | true    | false  | true     || [HiNeeded]                | true     | false || HI needed and MATS needed. 
        /// | 16 | false | true  | false | true  | false   | false  | true     || [HiNeeded]                | true     | false || HI needed and CO2C, H2O or NOXR needed. 
        /// | 17 | false | true  | false | true  | true    | false  | true     || [HiNeeded]                | true     | false || HI needed, CO2C, H2O or NOXR needed, and MATS needed.
        /// | 18 | false | true  | true  | false | false   | false  | true     || [HiNeeded]                | true     | false || HI needed and CO2C, H2O or NOXR needed. 
        /// | 19 | false | true  | true  | false | true    | false  | true     || [HiNeeded]                | true     | false || HI needed, CO2C, H2O or NOXR needed, and MATS needed.
        /// | 20 | false | true  | true  | true  | false   | false  | true     || [HiNeeded]                | true     | false || HI needed and CO2C, H2O or NOXR needed. 
        /// | 21 | false | true  | true  | true  | true    | false  | true     || [HiNeeded]                | true     | false || HI needed, CO2C, H2O or NOXR needed, and MATS needed.
        /// | 22 | true  | false | false | false | false   | false  | true     || [Co2cH2oAndNoxrOnly]      | false    | false || CO2C, H2O or NOXR needed.
        /// | 23 | true  | false | false | false | false   | true   | true     || [Co2cH2oAndNoxrOnly]      | false    | false || CO2C, H2O or NOXR needed.
        /// | 24 | true  | false | false | false | true    | false  | true     || [NotNeededOrNotHiButMats] | false    | false || CO2C, H2O or NOXR needed and MATS needed.
        /// | 25 | true  | false | false | true  | false   | false  | true     || [Co2cH2oAndNoxrOnly]      | false    | false || CO2C, H2O or NOXR needed.
        /// | 26 | true  | false | false | true  | true    | false  | true     || [NotNeededOrNotHiButMats] | false    | false || CO2C, H2O or NOXR needed and MATS needed.
        /// | 27 | true  | false | true  | false | false   | false  | true     || [Co2cH2oAndNoxrOnly]      | false    | false || CO2C, H2O or NOXR needed.
        /// | 28 | true  | false | true  | false | true    | false  | true     || [NotNeededOrNotHiButMats] | false    | false || CO2C, H2O or NOXR needed and MATS needed.
        /// | 29 | true  | false | true  | true  | false   | false  | true     || [Co2cH2oAndNoxrOnly]      | false    | false || CO2C, H2O or NOXR needed.
        /// | 30 | true  | false | true  | true  | true    | false  | true     || [NotNeededOrNotHiButMats] | false    | false || CO2C, H2O or NOXR needed and MATS needed.
        /// | 31 | true  | true  | false | false | false   | false  | true     || [HiNeeded]                | true     | false || HI needed and CO2C, H2O or NOXR needed. 
        /// | 32 | true  | true  | false | false | true    | false  | true     || [HiNeeded]                | true     | false || HI needed, CO2C, H2O or NOXR needed, and MATS needed.
        /// | 33 | true  | true  | false | true  | false   | false  | true     || [HiNeeded]                | true     | false || HI needed and CO2C, H2O or NOXR needed. 
        /// | 34 | true  | true  | false | true  | true    | false  | true     || [HiNeeded]                | true     | false || HI needed, CO2C, H2O or NOXR needed, and MATS needed.
        /// | 35 | true  | true  | true  | false | false   | false  | true     || [HiNeeded]                | true     | false || HI needed and CO2C, H2O or NOXR needed. 
        /// | 36 | true  | true  | true  | false | true    | false  | true     || [HiNeeded]                | true     | false || HI needed, CO2C, H2O or NOXR needed, and MATS needed.
        /// | 37 | true  | true  | true  | true  | false   | false  | true     || [HiNeeded]                | true     | false || HI needed and CO2C, H2O or NOXR needed. 
        /// | 38 | true  | true  | true  | true  | true    | false  | true     || [HiNeeded]                | true     | false || HI needed, CO2C, H2O or NOXR needed, and MATS needed.
        /// 
        /// | 39 | false | true  | false | false | false   | false  | false    || [HiNeeded]                | true     | false || HI needed, but no missing data record.
        /// | 40 | false | true  | false | false | true    | false  | false    || [HiNeeded]                | true     | false || HI needed and MATS needed, but no missing data record. 
        /// | 41 | false | true  | true  | false | false   | false  | false    || [HiNeeded]                | true     | false || HI needed and CO2C, H2O or NOXR needed, but no missing data record. 
        /// | 42 | false | true  | true  | false | true    | false  | false    || [HiNeeded]                | true     | false || HI needed, CO2C, H2O or NOXR needed, and MATS needed, but no missing data record.
        /// 
        /// </summary>
        [TestMethod()]
        public void HourMhv8_O2cWet()
        {
            /* Initialize objects generally needed for testing checks. */
            cHourlyMonitorValueChecks target = new cHourlyMonitorValueChecks(new cEmissionsCheckParameters());
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;


            /* Code Lists */
            string[] modcCodeList = UnitTestStandardLists.ModcCodeList;
            string[] moistureBasisList = { null, "W" };

            /* Result Actions */
            Func<string, bool?, bool?, bool?, bool?, bool?, bool, string> hiNeeded = (modc, h2o, hi, noxr, co2c, mats, md) => modc.NotInList("01,02,03,04,06,07,08,09,10,12,17,20,53,54,55") ? "A" : md && (noxr.Default(false) || mats.Default(false)) && modc.NotInList("01,02,03,04,17,20,53,54") ? "E" : null;
            Func<string, bool?, bool?, bool?, bool?, bool?, bool, string> notNeededOrNotHiButMats = (modc, h2o, hi, noxr, co2c, mats, md) => modc.InList("01,02,03,04,17,20,53,54") ? null : modc == "46" ? null : "D";
            Func<string, bool?, bool?, bool?, bool?, bool?, bool, string> co2cH2oAndNoxrOnly = (modc, h2o, hi, noxr, co2c, mats, md) => modc.InList("01,02,03,04,17,20,46,53,54") ? null : "D";

            /* Input Parameter Values */
            bool?[] h2oList = { null, false, false, false, false, false, false, false, false, false,
                                false, false, false, false, false, false, false, false, false, false,
                                false, false, true, true, true, true, true, true, true, true,
                                true, true, true, true, true, true, true, true, true, false,
                                false, false, false };
            bool?[] hiList = { null, false, false, false, false, false, false, false, false, false,
                               false, false, false, true, true, true, true, true, true, true,
                               true, true, false, false, false, false, false, false, false, false,
                               false, true, true, true, true, true, true, true, true, true,
                               true, true, true };
            bool?[] noxrList = { null, false, false, false, false, false, false, false, true, true,
                                 true, true, true, false, false, false, false, false, true, true,
                                 true, true, false, false, false, false, false, true, true, true,
                                 true, false, false, false, false, true, true, true, true, false,
                                 false, true, true };
            bool?[] co2cList = { null, false, false, false, false, true, true, true, false, false,
                                 false, true, true, false, false, false, true, true, false, false,
                                 true, true, false, false, false, true, true, false, false, true,
                                 true, false, false, true, true, false, false, true, true, false,
                                 false, false, false };
            bool?[] matsList = { null, false, false, true, true, false, false, true, false, false,
                                 true, false, true, false, false, true, false, true, false, true,
                                 false, true, false, false, true, false, true, false, true, false,
                                 true, false, true, false, true, false, true, false, true, false, 
                                 true, false, true };
            bool?[] hiFsmdList = { null, false, true, false, true, false, true, false, false, true,
                                   false, false, false, false, true, false, false, false, false, false,
                                   false, false, false, true, false, false, false, false, false, false,
                                   false, false, false, false, false, false, false, false, false, false,
                                   false, false, false };
            bool[] mdExists = { true, true, true, true, true, true, true, true, true, true,
                                true, true, true, true, true, true, true, true, true, true,
                                true, true, true, true, true, true, true, true, true, true,
                                true, true, true, true, true, true, true, true, true, false,
                                false, false, false };

            /* Expected Values */
            Func<string, bool?, bool?, bool?, bool?, bool?, bool, string>[] expResultActionList = { notNeededOrNotHiButMats, notNeededOrNotHiButMats, notNeededOrNotHiButMats, notNeededOrNotHiButMats, notNeededOrNotHiButMats,
                                                                                                    co2cH2oAndNoxrOnly, co2cH2oAndNoxrOnly, notNeededOrNotHiButMats, co2cH2oAndNoxrOnly, co2cH2oAndNoxrOnly,
                                                                                                    notNeededOrNotHiButMats, co2cH2oAndNoxrOnly, notNeededOrNotHiButMats, hiNeeded, hiNeeded,
                                                                                                    hiNeeded, hiNeeded, hiNeeded, hiNeeded, hiNeeded,
                                                                                                    hiNeeded, hiNeeded, co2cH2oAndNoxrOnly, co2cH2oAndNoxrOnly, notNeededOrNotHiButMats,
                                                                                                    co2cH2oAndNoxrOnly, notNeededOrNotHiButMats, co2cH2oAndNoxrOnly, notNeededOrNotHiButMats, co2cH2oAndNoxrOnly,
                                                                                                    notNeededOrNotHiButMats, hiNeeded, hiNeeded, hiNeeded, hiNeeded,
                                                                                                    hiNeeded, hiNeeded, hiNeeded, hiNeeded, hiNeeded, 
                                                                                                    hiNeeded, hiNeeded, hiNeeded};
            bool?[] expCompleteList = { false, false, false, false, false, false, false, false, false, false,
                                        false, false, false, true, true, true, true, true, true, true,
                                        true, true, false, false, false, false, false, false, false, false,
                                        false, true, true, true, true, true, true, true, true, true,
                                        true, true, true };
            bool?[] expFsHrList = { false, false, false, false, false, false, false, false, false, false,
                                    false, false, false, false, true, false, false, false, false, false,
                                    false, false, false, false, false, false, false, false, false, false,
                                    false, false, false, false, false, false, false, false, false, false,
                                    false, false, false };

            /* Test Case Count */
            int caseCount = 43;

            /* Check array lengths */
            Assert.AreEqual(caseCount, h2oList.Length, "h2oList length");
            Assert.AreEqual(caseCount, hiList.Length, "hiList length");
            Assert.AreEqual(caseCount, noxrList.Length, "NoxrList length");
            Assert.AreEqual(caseCount, co2cList.Length, "co2cList length");
            Assert.AreEqual(caseCount, matsList.Length, "matsList length");
            Assert.AreEqual(caseCount, hiFsmdList.Length, "hiFsmdList length");
            Assert.AreEqual(caseCount, expResultActionList.Length, "expResultActionList length");
            Assert.AreEqual(caseCount, expCompleteList.Length, "expCompleteList length");
            Assert.AreEqual(caseCount, expFsHrList.Length, "expFsHrList length");


            /* Run Cases */
            for (int caseDex = 0; caseDex < caseCount; caseDex++)
                foreach (string modcCd in modcCodeList)
                    foreach (string moistureBasis in moistureBasisList)
                    {
                        /* Initialize Input Parameters */
                        EmParameters.CurrentMhvParameter = "O2W";

                        /* Initialize Optional Parameters */
                        EmParameters.CurrentO2WetMissingDataMonitorHourlyRecord = (mdExists[caseDex] ? new VwMpMonitorHrlyValueRow(parameterCd: "O2C", moistureBasis: "W") : null);
                        EmParameters.CurrentO2WetMonitorHourlyRecord = new VwMpMonitorHrlyValueO2WetRow(modcCd: modcCd, moistureBasis: moistureBasis);
                        EmParameters.HeatInputFuelSpecificMissingData = hiFsmdList[caseDex];
                        EmParameters.O2WetChecksNeededForH2o = h2oList[caseDex];
                        EmParameters.O2WetChecksNeededForHeatInput = hiList[caseDex];
                        EmParameters.O2WetChecksNeededForNoxRateCalc = noxrList[caseDex];
                        EmParameters.O2WetNeededToSupportCo2Calculation = co2cList[caseDex];
                        EmParameters.O2WetNeededForMats = matsList[caseDex];

                        /* Initialize Output Parameters */
                        EmParameters.CompleteMhvRecordNeeded = null;
                        EmParameters.CurrentMhvComponentType = "BAD";
                        EmParameters.CurrentMhvDefaultParameter = "BAD";
                        EmParameters.CurrentMhvFuelSpecificHour = null;
                        EmParameters.CurrentMhvParameterDescription = null;
                        EmParameters.CurrentMhvRecord = new VwMpMonitorHrlyValueRow(modcCd: "BAD");
                        EmParameters.CurrentMhvSystemType = "BAD";
                        EmParameters.MonitorHourlyModcStatus = null;
                        EmParameters.NoxConcModc = "BAD";


                        /* Init Cateogry Result */
                        category.CheckCatalogResult = null;

                        /* Initialize variables needed to run the check. */
                        bool log = false;

                        /* Run Check */
                        string actual = target.HOURMHV8(category, ref log);
                        string result = expResultActionList[caseDex](modcCd, h2oList[caseDex], hiList[caseDex], noxrList[caseDex], co2cList[caseDex], matsList[caseDex], mdExists[caseDex]);

                        /* Check Results */
                        Assert.AreEqual(string.Empty, actual, string.Format("actual [case {0}, modcCd {1}, H2O {2}, HI {3}, NOXR {4}, CO2C {5}, MATS {6}, HI FSMD {7}]", caseDex, modcCd, h2oList[caseDex], hiList[caseDex], noxrList[caseDex], co2cList[caseDex], matsList[caseDex], hiFsmdList[caseDex]));
                        Assert.AreEqual(false, log, string.Format("log  [case {0}, modcCd {1}, H2O {2}, HI {3}, NOXR {4}, CO2C {5}, MATS {6}, HI FSMD {7}]", caseDex, modcCd, h2oList[caseDex], hiList[caseDex], noxrList[caseDex], co2cList[caseDex], matsList[caseDex], hiFsmdList[caseDex]));

                        Assert.AreEqual(result, category.CheckCatalogResult, string.Format("CheckCatalogResult  [case {0}, modcCd {1}, H2O {2}, HI {3}, NOXR {4}, CO2C {5}, MATS {6}, HI FSMD {7}]", caseDex, modcCd, h2oList[caseDex], hiList[caseDex], noxrList[caseDex], co2cList[caseDex], matsList[caseDex], hiFsmdList[caseDex]));

                        Assert.AreEqual(expCompleteList[caseDex], EmParameters.CompleteMhvRecordNeeded, string.Format("CompleteMhvRecordNeeded  [case {0}, modcCd {1}, H2O {2}, HI {3}, NOXR {4}, CO2C {5}, MATS {6}, HI FSMD {7}]", caseDex, modcCd, h2oList[caseDex], hiList[caseDex], noxrList[caseDex], co2cList[caseDex], matsList[caseDex], hiFsmdList[caseDex]));
                        Assert.AreEqual("O2", EmParameters.CurrentMhvComponentType, string.Format("CurrentMhvComponentType  [case {0}, modcCd {1}, H2O {2}, HI {3}, NOXR {4}, CO2C {5}, MATS {6}, HI FSMD {7}]", caseDex, modcCd, h2oList[caseDex], hiList[caseDex], noxrList[caseDex], co2cList[caseDex], matsList[caseDex], hiFsmdList[caseDex]));
                        Assert.AreEqual("O2N", EmParameters.CurrentMhvDefaultParameter, string.Format("CurrentMhvDefaultParameter  [case {0}, modcCd {1}, H2O {2}, HI {3}, NOXR {4}, CO2C {5}, MATS {6}, HI FSMD {7}]", caseDex, modcCd, h2oList[caseDex], hiList[caseDex], noxrList[caseDex], co2cList[caseDex], matsList[caseDex], hiFsmdList[caseDex]));
                        Assert.AreEqual(expFsHrList[caseDex], EmParameters.CurrentMhvFuelSpecificHour, string.Format("CurrentMhvFuelSpecificHour  [case {0}, modcCd {1}, H2O {2}, HI {3}, NOXR {4}, CO2C {5}, MATS {6}, HI FSMD {7}]", caseDex, modcCd, h2oList[caseDex], hiList[caseDex], noxrList[caseDex], co2cList[caseDex], matsList[caseDex], hiFsmdList[caseDex]));
                        Assert.AreEqual(moistureBasis == null ? "O2C" : "O2C with a MoistureBasis of W", EmParameters.CurrentMhvParameterDescription, string.Format("CurrentMhvParameterDescription  [case {0}, modcCd {1}, H2O {2}, HI {3}, NOXR {4}, CO2C {5}, MATS {6}, HI FSMD {7}]", caseDex, modcCd, h2oList[caseDex], hiList[caseDex], noxrList[caseDex], co2cList[caseDex], matsList[caseDex], hiFsmdList[caseDex]));
                        Assert.AreEqual(EmParameters.CurrentO2WetMonitorHourlyRecord.ModcCd, EmParameters.CurrentMhvRecord.ModcCd, string.Format("CurrentMhvRecord  [case {0}, modcCd {1}, H2O {2}, HI {3}, NOXR {4}, CO2C {5}, MATS {6}, HI FSMD {7}]", caseDex, modcCd, h2oList[caseDex], hiList[caseDex], noxrList[caseDex], co2cList[caseDex], matsList[caseDex], hiFsmdList[caseDex]));
                        Assert.AreEqual(null, EmParameters.CurrentMhvSystemType, string.Format("CurrentMhvSystemType  [case {0}, modcCd {1}, H2O {2}, HI {3}, NOXR {4}, CO2C {5}, MATS {6}, HI FSMD {7}]", caseDex, modcCd, h2oList[caseDex], hiList[caseDex], noxrList[caseDex], co2cList[caseDex], matsList[caseDex], hiFsmdList[caseDex]));
                        Assert.AreEqual((result == null || result == "E"), EmParameters.MonitorHourlyModcStatus, string.Format("MonitorHourlyModcStatus  [case {0}, modcCd {1}, H2O {2}, HI {3}, NOXR {4}, CO2C {5}, MATS {6}, HI FSMD {7}]", caseDex, modcCd, h2oList[caseDex], hiList[caseDex], noxrList[caseDex], co2cList[caseDex], matsList[caseDex], hiFsmdList[caseDex]));

                        Assert.AreEqual(EmParameters.CurrentO2WetMonitorHourlyRecord.ModcCd, EmParameters.O2WetModc, string.Format("O2WetModc  [case {0}, modcCd {1}, H2O {2}, HI {3}, NOXR {4}, CO2C {5}, MATS {6}, HI FSMD {7}]", caseDex, modcCd, h2oList[caseDex], hiList[caseDex], noxrList[caseDex], co2cList[caseDex], matsList[caseDex], hiFsmdList[caseDex]));
                    }
        }


        /// <summary>
        /// 
        /// CO2 Conc Checks Needed for CO2 Mass Calc = true
        /// NOx Conc Checks Needed for Heat input = true
        /// NOx Conc Checks Needed for NOx Mass = true
        /// O2 Dry Checks Needed for Heat Input = true
        /// O2 Dry Checks Needed to SUpport CO2 Calculation = true
        /// O2 Wet Checks Needed for Heat Input = true
        /// O2 Wet Checks Needed to SUpport CO2 Calculation = true
        /// 
        /// MODC Sets: 
        /// CO2C   : 01, 02, 03, 04, 17, 18, 20, 21, 53
        /// FLOW   : 01, 02, 03, 04, 20, 53
        /// H2O    : 01, 02, 03, 04, 21, 53
        /// NOXC   : 01, 02, 03, 04, 17, 18, 19, 20, 21, 22, 53
        /// O2D    : 01, 02, 03, 04, 17, 18, 20, 21, 53
        /// O2W    : 01, 02, 03, 04, 17, 18, 20, 21, 53
        /// SO2C   : 01, 02, 03, 04, 16, 17, 18, 19, 20, 21, 22, 53
        /// Others : {empty set}
        /// 
        /// Monitor System Records: Have ids ONE, GOOD and THREE and other where GOOD system type equals case system type and ONE and THREE system types equal OTHER. 
        /// 
        /// 
        /// Test Cases:
        ///      |  MODC  |
        /// | ## | Status | Param  | System Id | MODC  | Sys Type | Param Type | Legacy || Result | Status    | Sys Rec || Note
        /// |  0 | null   | CO2C   | GOOD      | 01    | CO2      | CO2        | false  || null   | true      | null    || null MODC status
        /// |  1 | false  | CO2C   | GOOD      | 01    | CO2      | CO2        | false  || null   | true      | null    || false MODC status
        /// |  2 | false  | CO2C   | GOOD      | 01    | CO2      | CO2        | false  || null   | true      | null    || false MODC status (missing data)
        /// |  3 | false  | CO2CSD | GOOD      | 06    | CO2      | CO2        | false  || null   | true      | null    || false MODC status
        /// |  4 | false  | FLOW   | GOOD      | 01    | FLOW     | FLOW       | false  || null   | true      | null    || false MODC status
        /// |  5 | false  | H2O    | GOOD      | 01    | H2O      | H2O        | false  || null   | true      | null    || false MODC status
        /// |  6 | false  | NOXC   | GOOD      | 01    | NOX      | NOX        | false  || null   | true      | null    || false MODC status
        /// |  7 | false  | O2D    | GOOD      | 01    | O2       | O2         | false  || null   | true      | null    || false MODC status
        /// |  8 | false  | O2W    | GOOD      | 01    | O2       | O2         | false  || null   | true      | null    || false MODC status
        /// |  9 | false  | O2CSD  | GOOD      | 06    | O2       | O2         | false  || null   | true      | null    || false MODC status
        /// | 10 | false  | SO2C   | GOOD      | 01    | SO2      | SO2        | false  || null   | true      | null    || false MODC status
        /// | 11 | true   | CO2C   | null      | [all] | CO2      | CO2        | false  || [note] | {!Result} | null    || MON_SYS_ID is null.  Result is C if MODC is in MODC set for the parameter, otherwise it is H.
        /// | 12 | true   | CO2CSD | null      | [all] | CO2      | CO2        | false  || [note] | {!Result} | null    || MON_SYS_ID is null.  Result is C if MODC is in MODC set for the parameter, otherwise it is H.
        /// | 13 | true   | FLOW   | null      | [all] | FLOW     | FLOW       | false  || [note] | {!Result} | null    || MON_SYS_ID is null.  Result is C if MODC is in MODC set for the parameter, otherwise it is H.
        /// | 14 | true   | H2O    | null      | [all] | H2O      | H2O        | false  || [note] | {!Result} | null    || MON_SYS_ID is null.  Result is C if MODC is in MODC set for the parameter, otherwise it is H.
        /// | 15 | true   | NOXC   | null      | [all] | NOX      | NOX        | false  || [note] | {!Result} | null    || MON_SYS_ID is null.  Result is C if MODC is in MODC set for the parameter, otherwise it is H.
        /// | 16 | true   | O2D    | null      | [all] | O2       | O2         | false  || [note] | {!Result} | null    || MON_SYS_ID is null.  Result is C if MODC is in MODC set for the parameter, otherwise it is H.
        /// | 17 | true   | O2W    | null      | [all] | O2       | O2         | false  || [note] | {!Result} | null    || MON_SYS_ID is null.  Result is C if MODC is in MODC set for the parameter, otherwise it is H.
        /// | 18 | true   | O2CSD  | null      | [all] | O2       | O2         | false  || [note] | {!Result} | null    || MON_SYS_ID is null.  Result is C if MODC is in MODC set for the parameter, otherwise it is H.
        /// | 19 | true   | SO2C   | null      | [all] | SO2      | SO2        | false  || [note] | {!Result} | null    || MON_SYS_ID is null.  Result is C if MODC is in MODC set for the parameter, otherwise it is H.
        /// | 20 | true   | CO2C   | BAD       | 01    | CO2      | CO2        | false  || D      | false     | null    || MON_SYS_ID does not match an existing system.
        /// | 21 | true   | CO2CSD | BAD       | 06    | CO2      | CO2        | false  || D      | false     | null    || MON_SYS_ID does not match an existing system.
        /// | 22 | true   | FLOW   | BAD       | 01,06 | FLOW     | FLOW       | false  || D      | false     | null    || MON_SYS_ID does not match an existing system.
        /// | 23 | true   | H2O    | BAD       | 01,06 | H2O      | H2O        | false  || D      | false     | null    || MON_SYS_ID does not match an existing system.
        /// | 24 | true   | NOXC   | BAD       | 01,06 | NOX      | NOX        | false  || D      | false     | null    || MON_SYS_ID does not match an existing system.
        /// | 25 | true   | O2D    | BAD       | 01    | O2       | O2         | false  || D      | false     | null    || MON_SYS_ID does not match an existing system.
        /// | 26 | true   | O2W    | BAD       | 01    | O2       | O2         | false  || D      | false     | null    || MON_SYS_ID does not match an existing system.
        /// | 27 | true   | O2CSD  | BAD       | 06    | O2       | O2         | false  || D      | false     | null    || MON_SYS_ID does not match an existing system.
        /// | 28 | true   | SO2C   | BAD       | 01,06 | SO2      | SO2        | false  || D      | false     | null    || MON_SYS_ID does not match an existing system.
        /// | 29 | true   | CO2C   | GOOD      | 01    | [all]    | CO2        | false  || [note] | {!Result} | null    || System Type check: Result E if MHV system type does not match parameter MHV System type.
        /// | 30 | true   | CO2C   | GOOD      | 01    | [all]    | CO2        | true   || [note] | {!Result} | null    || System Type check for legacy: Result E if MHV system type does not match parameter MHV System type and is not "NOX".
        /// | 31 | true   | CO2CSD | GOOD      | 06    | [all]    | CO2        | false  || [note] | {!Result} | null    || System Type check: Result E if MHV system type does not match parameter MHV System type.
        /// | 32 | true   | CO2CSD | GOOD      | 06    | [all]    | CO2        | true   || [note] | {!Result} | null    || System Type check for legacy: Result E if MHV system type does not match parameter MHV System type and is not "NOX".
        /// | 33 | true   | FLOW   | GOOD      | 01,06 | [all]    | FLOW       | false  || [note] | {!Result} | null    || System Type check: Result E if MHV system type does not match parameter MHV System type.
        /// | 34 | true   | FLOW   | GOOD      | 01,06 | [all]    | FLOW       | true   || [note] | {!Result} | null    || System Type check for legacy: Result E if MHV system type does not match parameter MHV System type.
        /// | 35 | true   | H2O    | GOOD      | 01,06 | [all]    | null       | false  || [note] | {!Result} | null    || System Type check: Result E if MHV system type is not "H2OM" or "M2OT".
        /// | 36 | true   | H2O    | GOOD      | 01,06 | [all]    | null       | true   || [note] | {!Result} | null    || System Type check for legacy: Result E if MHV system type is not "H2OM" or "M2OT".
        /// | 37 | true   | NOXC   | GOOD      | 01,06 | [all]    | NOX        | false  || [note] | {!Result} | null    || System Type check: Result E if MHV system type does not match parameter MHV System type.
        /// | 38 | true   | NOXC   | GOOD      | 01,06 | [all]    | NOX        | true   || [note] | {!Result} | null    || System Type check for legacy: Result E if MHV system type does not match parameter MHV System type.
        /// | 39 | true   | O2D    | GOOD      | 01    | [all]    | null       | false  || [note] | {!Result} | null    || System Type check: Result E if MHV system type is not "CO2" or "O2".
        /// | 40 | true   | O2D    | GOOD      | 01    | [all]    | null       | true   || [note] | {!Result} | null    || System Type check for legacy: Result E if MHV system type is not in set {"CO2", "H2O", "NOX", "NOXC", "O2"}.
        /// | 41 | true   | O2W    | GOOD      | 01    | [all]    | null       | false  || [note] | {!Result} | null    || System Type check: Result E if MHV system type is not "CO2" or "O2".
        /// | 42 | true   | O2W    | GOOD      | 01    | [all]    | null       | true   || [note] | {!Result} | null    || System Type check for legacy: Result E if MHV system type is not in set {"CO2", "H2O", "NOX", "NOXC", "O2"}.
        /// | 43 | true   | O2CSD  | GOOD      | 06    | [all]    | null       | false  || [note] | {!Result} | null    || System Type check: Result E if MHV system type is not "CO2" or "O2".
        /// | 44 | true   | O2CSD  | GOOD      | 06    | [all]    | null       | true   || [note] | {!Result} | null    || System Type check for legacy: Result E if MHV system type is not in set {"CO2", "H2O", "NOX", "NOXC", "O2"}.
        /// | 45 | true   | SO2C   | GOOD      | 01,06 | [all]    | SO2        | false  || [note] | {!Result} | null    || System Type check: Result E if MHV system type does not match parameter MHV System type.
        /// | 46 | true   | SO2C   | GOOD      | 01,06 | [all]    | SO2        | true   || [note] | {!Result} | null    || System Type check for legacy: Result E if MHV system type does not match parameter MHV System type.
        /// 
        /// </summary>
        [TestMethod()]
        public void HourMhv13_Core()
        {
            /* Initialize objects generally needed for testing checks. */
            cHourlyMonitorValueChecks target = new cHourlyMonitorValueChecks(new cEmissionsCheckParameters());
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;


            /* MODC Lists */
            string[] modcAll = UnitTestStandardLists.ModcCodeList;
            string[] modcBoth = { "01", "06" };
            string[] modc01 = { "01" };
            string[] modc06 = { "06" };

            /* Measured MODC Dictionaries */
            Dictionary<string, List<string>> measuredModcDictionary = new Dictionary<string, List<string>>();
            {
                measuredModcDictionary["CO2C"] = new List<string> { "01", "02", "03", "04", "17", "18", "20", "21" };
                measuredModcDictionary["CO2CSD"] = new List<string> { };
                measuredModcDictionary["FLOW"] = new List<string> { "01", "02", "03", "04", "20" };
                measuredModcDictionary["H2O"] = new List<string> { "01", "02", "03", "04", "21" };
                measuredModcDictionary["NOXC"] = new List<string> { "01", "02", "03", "04", "17", "18", "19", "20", "21", "22" };
                measuredModcDictionary["O2D"] = new List<string> { "01", "02", "03", "04", "17", "18", "20", "21" };
                measuredModcDictionary["O2W"] = new List<string> { "01", "02", "03", "04", "17", "18", "20", "21" };
                measuredModcDictionary["O2CSD"] = new List<string> { };
                measuredModcDictionary["SO2C"] = new List<string> { "01", "02", "03", "04", "16", "17", "18", "19", "20", "21", "22" };
            }

            /* System Type Code Lists*/
            string[] sysTypeAll = UnitTestStandardLists.SystemTypeCodeList;
            string[] sysTypeCo2 = { "CO2" };
            string[] sysTypeFlow = { "FLOW" };
            string[] sysTypeH2o = { "H2O" };
            string[] sysTypeNox = { "NOX" };
            string[] sysTypeO2 = { "O2" };
            string[] sysTypeSo2 = { "SO2" };

            /* Result Actions */
            Func<string, string, string, string, bool?, string> resultActionNull = (mp, mm, ms, ps, ld) => null;
            Func<string, string, string, string, bool?, string> resultActionD = (mp, mm, ms, ps, ld) => "D";
            Func<string, string, string, string, bool?, string> resultActionNoSys = (mp, mm, ms, ps, ld) => measuredModcDictionary.ContainsKey(mp) && measuredModcDictionary[mp].Contains(mm) ? "C" : "H";
            Func<string, string, string, string, bool?, string> resultActionSysMatch = (mp, mm, ms, ps, ld) => ms != ps ? "E" : null;
            Func<string, string, string, string, bool?, string> resultActionCo2cMatch = (mp, mm, ms, ps, ld) => ms != ps && (!ld.Default(false) || ms != "NOX") ? "E" : null;
            Func<string, string, string, string, bool?, string> resultActionH2oMatch = (mp, mm, ms, ps, ld) => ms.NotInList("H2OM,H2OT") ? "E" : null;
            Func<string, string, string, string, bool?, string> resultActionO2Match = (mp, mm, ms, ps, ld) => ms.NotInList(ld.Default(false) ? "CO2,H2O,NOX,NOXC,O2" : "CO2,O2") ? "E" : null;


            /* Input Parameter Values */
            bool?[] modcStatusList = { null, false, false, false, false, false, false, false, false, false,
                                       false, true, true, true, true, true, true, true, true, true,
                                       true, true, true, true, true, true, true, true, true, true,
                                       true, true, true, true, true, true, true, true, true, true ,
                                       true, true, true, true, true, true, true  };
            string[] parameterList = { "CO2C", "CO2C", "CO2C", "CO2CSD", "FLOW", "H2O", "NOXC", "O2D", "O2W", "O2CSD",
                                       "SO2C", "CO2C", "CO2CSD", "FLOW", "H2O", "NOXC", "O2D", "O2W", "O2CSD", "SO2C",
                                       "CO2C", "CO2CSD", "FLOW", "H2O", "NOXC", "O2D", "O2W", "O2CSD", "SO2C", "CO2C",
                                       "CO2C", "CO2CSD", "CO2CSD", "FLOW", "FLOW", "H2O", "H2O", "NOXC", "NOXC", "O2D",
                                       "O2D", "O2W", "O2W", "O2CSD", "O2CSD", "SO2C", "SO2C" };
            string[] systemIdList = { "GOOD", "GOOD", "GOOD", "GOOD", "GOOD", "GOOD", "GOOD", "GOOD", "GOOD", "GOOD",
                                      "GOOD", null, null, null, null, null, null, null, null, null,
                                      "BAD", "BAD", "BAD", "BAD", "BAD", "BAD", "BAD", "BAD", "BAD", "GOOD",
                                      "GOOD", "GOOD", "GOOD", "GOOD", "GOOD", "GOOD", "GOOD", "GOOD", "GOOD", "GOOD",
                                      "GOOD", "GOOD", "GOOD", "GOOD", "GOOD", "GOOD", "GOOD" };
            string[][] modcSetList = { modc01, modc01, modc01, modc06, modc01, modc01, modc01, modc01, modc01, modc06,
                                       modc01, modcAll, modcAll, modcAll, modcAll, modcAll, modcAll, modcAll, modcAll, modcAll,
                                       modc01, modc06, modcBoth, modcBoth, modcBoth, modc01, modc01, modc06, modcBoth, modc01,
                                       modc01, modc06, modc06, modcBoth, modcBoth, modcBoth, modcBoth, modcBoth, modcBoth, modc01,
                                       modc01, modc01, modc01, modc06, modc06, modcBoth, modcBoth};
            string[][] sysTypeSetList = { sysTypeCo2, sysTypeCo2, sysTypeCo2, sysTypeCo2, sysTypeFlow, sysTypeH2o, sysTypeNox, sysTypeO2, sysTypeO2, sysTypeO2,
                                          sysTypeSo2, sysTypeCo2, sysTypeCo2, sysTypeFlow, sysTypeH2o, sysTypeNox, sysTypeO2, sysTypeO2, sysTypeO2, sysTypeSo2,
                                          sysTypeCo2, sysTypeCo2, sysTypeFlow, sysTypeH2o, sysTypeNox, sysTypeO2, sysTypeO2, sysTypeO2, sysTypeSo2, sysTypeAll,
                                          sysTypeAll, sysTypeAll, sysTypeAll, sysTypeAll, sysTypeAll, sysTypeAll, sysTypeAll, sysTypeAll, sysTypeAll, sysTypeAll,
                                          sysTypeAll, sysTypeAll, sysTypeAll, sysTypeAll, sysTypeAll, sysTypeAll, sysTypeAll};
            string[] sysTypeParamList = { "CO2", "CO2", "CO2", "CO2", "FLOW", "H2O", "NOX", "O2", "O2", "O2",
                                          "SO2", "CO2", "CO2", "FLOW", "H2O", "NOX", "O2", "O2", "O2", "SO2",
                                          "CO2", "CO2", "FLOW", "H2O", "NOX", "O2", "O2", "O2", "SO2", "CO2",
                                          "CO2", "CO2", "CO2", "FLOW", "FLOW", null, null, "NOX", "NOX", null,
                                          null, null, null, null, null, "SO2", "SO2" };
            bool?[] legacyList = { false, false, false, false, false, false, false, false, false, false,
                                   false, false, false, false, false, false, false, false, false, false,
                                   false, false, false, false, false, false, false, false, false, false,
                                   true, false, true, false, true, false, true, false, true, false,
                                   true, false, true, false, true, false, true  };

            /* Expected Values */
            Func<string, string, string, string, bool?, string>[] expResultActionList = { resultActionNull, resultActionNull, resultActionNull, resultActionNull, resultActionNull,
                                                                                          resultActionNull, resultActionNull, resultActionNull, resultActionNull, resultActionNull,
                                                                                          resultActionNull, resultActionNoSys, resultActionNoSys, resultActionNoSys, resultActionNoSys,
                                                                                          resultActionNoSys, resultActionNoSys, resultActionNoSys, resultActionNoSys, resultActionNoSys,
                                                                                          resultActionD, resultActionD, resultActionD, resultActionD, resultActionD,
                                                                                          resultActionD, resultActionD, resultActionD, resultActionD, resultActionCo2cMatch,
                                                                                          resultActionCo2cMatch, resultActionCo2cMatch, resultActionCo2cMatch, resultActionSysMatch, resultActionSysMatch,
                                                                                          resultActionH2oMatch, resultActionH2oMatch, resultActionSysMatch, resultActionSysMatch, resultActionO2Match,
                                                                                          resultActionO2Match, resultActionO2Match, resultActionO2Match, resultActionO2Match, resultActionO2Match,
                                                                                          resultActionSysMatch, resultActionSysMatch};
            string[] expSystemIdList = { null, null, null, null, null, null, null, null, null, null,
                                         null, null, null, null, null, null, null, null, null, null,
                                         null, null, null, null, null, null, null, null, null, "GOOD",
                                         "GOOD", "GOOD", "GOOD", "GOOD", "GOOD", "GOOD", "GOOD", "GOOD", "GOOD", "GOOD",
                                         "GOOD", "GOOD", "GOOD", "GOOD", "GOOD", "GOOD", "GOOD" };

            /* Test Case Count */
            int caseCount = 47;

            /* Check array lengths */
            Assert.AreEqual(caseCount, modcStatusList.Length, "modcStatusList length");
            Assert.AreEqual(caseCount, parameterList.Length, "parameterList length");
            Assert.AreEqual(caseCount, systemIdList.Length, "systemIdList length");
            Assert.AreEqual(caseCount, modcSetList.Length, "modcSetList length");
            Assert.AreEqual(caseCount, sysTypeSetList.Length, "sysTypeSetList length");
            Assert.AreEqual(caseCount, sysTypeParamList.Length, "sysTypeParamList length");
            Assert.AreEqual(caseCount, legacyList.Length, "legacyList length");
            Assert.AreEqual(caseCount, expResultActionList.Length, "expResultActionList length");
            Assert.AreEqual(caseCount, expSystemIdList.Length, "expSystemIdList length");


            /* Run Cases */
            for (int caseDex = 0; caseDex < caseCount; caseDex++)
                foreach (string sysTypeCd in sysTypeSetList[caseDex])
                    foreach (string modcCd in modcSetList[caseDex])
                    {
                        /* Initialize Input Parameters */
                        EmParameters.CurrentMhvParameter = parameterList[caseDex];
                        EmParameters.CurrentMhvParameterDescription = parameterList[caseDex] + " Description";
                        EmParameters.CurrentMhvRecord = new VwMpMonitorHrlyValueRow(monSysId: systemIdList[caseDex], modcCd: modcCd);
                        EmParameters.CurrentMhvSystemType = sysTypeParamList[caseDex];
                        EmParameters.MonitorHourlyModcStatus = modcStatusList[caseDex];
                        EmParameters.MonitorSystemRecordsByHourLocation = new CheckDataView<VwMpMonitorSystemRow>
                            (
                               new VwMpMonitorSystemRow(monSysId: "ONE", sysTypeCd: "OTHER1"),
                               new VwMpMonitorSystemRow(monSysId: "GOOD", sysTypeCd: sysTypeCd),
                               new VwMpMonitorSystemRow(monSysId: "THREE", sysTypeCd: "OTHER3")
                            );

                        /* Initialize Optional Parameters */
                        EmParameters.Co2ConcChecksNeededForCo2MassCalc = true;
                        EmParameters.Co2ConcChecksNeededForHeatInput = true;
                        EmParameters.LegacyDataEvaluation = legacyList[caseDex];
                        EmParameters.NoxConcNeededForNoxMassCalc = true;
                        EmParameters.O2DryChecksNeededForHeatInput = true;
                        EmParameters.O2DryNeededToSupportCo2Calculation = true;
                        EmParameters.O2WetChecksNeededForHeatInput = true;
                        EmParameters.O2WetNeededToSupportCo2Calculation = true;

                        /* Initialize Output Parameters */
                        EmParameters.CurrentMhvMonSysRecord = new VwMpMonitorSystemRow(monSysId: "BAD", sysTypeCd: "BAD");
                        EmParameters.MonitorHourlySystemStatus = null;


                        /* Init Cateogry Result */
                        category.CheckCatalogResult = null;

                        /* Initialize variables needed to run the check. */
                        bool log = false;

                        /* Run Check */
                        string actual = target.HOURMHV13(category, ref log);
                        string result = expResultActionList[caseDex](parameterList[caseDex], modcCd, sysTypeCd, sysTypeParamList[caseDex], legacyList[caseDex]);

                        /* Check Results */
                        Assert.AreEqual(string.Empty, actual, string.Format("actual[case {0}, param {1}, type {2}, modc {3}]", caseDex, parameterList[caseDex], sysTypeCd, modcCd));
                        Assert.AreEqual(false, log, string.Format("log [case {0}, param {1}, type {2}, modc {3}]", caseDex, parameterList[caseDex], sysTypeCd, modcCd));

                        Assert.AreEqual(result, category.CheckCatalogResult, string.Format("CheckCatalogResult [case {0}, param {1}, type {2}, modc {3}]", caseDex, parameterList[caseDex], sysTypeCd, modcCd));
                        Assert.AreEqual(result.IsEmpty(), EmParameters.MonitorHourlySystemStatus, string.Format("MonitorHourlySystemStatus [case {0}, param {1}, type {2}, modc {3}]", caseDex, parameterList[caseDex], sysTypeCd, modcCd));
                        Assert.AreEqual(expSystemIdList[caseDex], EmParameters.CurrentMhvMonSysRecord == null ? null : EmParameters.CurrentMhvMonSysRecord.MonSysId, string.Format("MonitorHourlySystemStatus [case {0}, param {1}, type {2}, modc {3}]", caseDex, parameterList[caseDex], sysTypeCd, modcCd));
                    }
        }


        /// <summary>
        /// 
        /// MODC Sets: 
        /// CO2C   : 01, 02, 03, 04, 17, 18, 20, 21, 53
        /// FLOW   : 01, 02, 03, 04, 20, 53
        /// H2O    : 01, 02, 03, 04, 21, 53
        /// NOXC   : 01, 02, 03, 04, 17, 18, 19, 20, 21, 22, 53
        /// O2D    : 01, 02, 03, 04, 17, 18, 20, 21, 53
        /// O2W    : 01, 02, 03, 04, 17, 18, 20, 21, 53
        /// SO2C   : 01, 02, 03, 04, 16, 17, 18, 19, 20, 21, 22, 53
        /// Others : {empty set}
        /// 
        /// Monitor System Records: Have ids ONE, GOOD and THREE and other where GOOD system type equals case system type and ONE and THREE system types equal OTHER. 
        /// | CompId | SysId  | CompType | CompIdentifier |
        /// | Other  | SGood  | OTHER    | OTH            |
        /// | COnly  | [null] | [case]   | ONL            |
        /// | CGood  | SGood  | [case]   | [case]         |
        /// | CGood  | SBad   | [case]   | [case]         |
        /// | CFlw1  | SFlow  | FLOW     | FL1            |
        /// | CFlw2  | SFlow  | FLOW     | FL2            |
        /// | [null] | SOnly  | [null]   | [null]         |
        /// 
        /// Test Cases:
        /// 
        /// | ## | ModcStatus | Param  | SysId | SysStatus | CompId | CompType | CompIdent | Modc  | ParamCompType || Result | Status    || Note
        /// |  0 | null       | CO2C   | SGood | true      | CGood  | CO2      | GOO       | 01    | CO2           || null   | false     || MODC Status is null
        /// |  1 | false      | CO2C   | SGood | true      | CGood  | CO2      | GOO       | 01    | CO2           || null   | false     || MODC Status is false
        /// |  2 | false      | CO2CSD | SGood | true      | CGood  | CO2      | GOO       | 06    | CO2           || null   | false     || MODC Status is false
        /// |  3 | false      | FLOW   | SGood | true      | CGood  | FLOW     | GOO       | 01    | FLOW          || null   | false     || MODC Status is false
        /// |  4 | false      | H2O    | SGood | true      | CGood  | H2O      | GOO       | 01    | H2O           || null   | false     || MODC Status is false
        /// |  5 | false      | NOXC   | SGood | true      | CGood  | NOX      | GOO       | 01    | NOX           || null   | false     || MODC Status is false
        /// |  6 | false      | O2D    | SGood | true      | CGood  | O2       | GOO       | 01    | O2            || null   | false     || MODC Status is false
        /// |  7 | false      | O2W    | SGood | true      | CGood  | O2       | GOO       | 01    | O2            || null   | false     || MODC Status is false
        /// |  8 | false      | O2CSD  | SGood | true      | CGood  | O2       | GOO       | 06    | O2            || null   | false     || MODC Status is false
        /// |  9 | false      | SO2C   | SGood | true      | CGood  | SO2      | GOO       | 01    | SO2           || null   | false     || MODC Status is false
        /// | 10 | true       | CO2C   | SGood | true      | CGood  | BAD      | GOO       | 01    | CO2           || B      | false     || Component type does not match
        /// | 11 | true       | CO2CSD | SGood | true      | CGood  | BAD      | GOO       | 06    | CO2           || B      | false     || Component type does not match
        /// | 12 | true       | FLOW   | SGood | true      | CGood  | BAD      | GOO       | 01    | FLOW          || B      | false     || Component type does not match
        /// | 13 | true       | H2O    | SGood | true      | CGood  | BAD      | GOO       | 01    | H2O           || B      | false     || Component type does not match
        /// | 14 | true       | NOXC   | SGood | true      | CGood  | BAD      | GOO       | 01    | NOX           || B      | false     || Component type does not match
        /// | 15 | true       | O2D    | SGood | true      | CGood  | BAD      | GOO       | 01    | O2            || B      | false     || Component type does not match
        /// | 16 | true       | O2W    | SGood | true      | CGood  | BAD      | GOO       | 01    | O2            || B      | false     || Component type does not match
        /// | 17 | true       | O2CSD  | SGood | true      | CGood  | BAD      | GOO       | 06    | O2            || B      | false     || Component type does not match
        /// | 18 | true       | SO2C   | SGood | true      | CGood  | BAD      | GOO       | 01    | SO2           || B      | false     || Component type does not match
        /// | 19 | true       | CO2C   | SGood | true      | CGood  | CO2      | GOO       | 17    | CO2           || C      | false     || Like kind without proper component identifier
        /// | 20 | true       | NOXC   | SGood | true      | CGood  | NOX      | GOO       | 17    | NOX           || C      | false     || Like kind without proper component identifier
        /// | 21 | true       | O2D    | SGood | true      | CGood  | O2       | GOO       | 17    | O2            || C      | false     || Like kind without proper component identifier
        /// | 22 | true       | O2W    | SGood | true      | CGood  | O2       | GOO       | 17    | O2            || C      | false     || Like kind without proper component identifier
        /// | 23 | true       | SO2C   | SGood | true      | CGood  | SO2      | GOO       | 17    | SO2           || C      | false     || Like kind without proper component identifier
        /// | 24 | true       | CO2C   | SGood | true      | CGood  | CO2      | LKA       | 17    | CO2           || null   | true      || Like kind with proper component identifier
        /// | 25 | true       | NOXC   | SGood | true      | CGood  | NOX      | LKA       | 17    | NOX           || null   | true      || Like kind with proper component identifier
        /// | 26 | true       | O2D    | SGood | true      | CGood  | O2       | LKA       | 17    | O2            || null   | true      || Like kind with proper component identifier
        /// | 27 | true       | O2W    | SGood | true      | CGood  | O2       | LKA       | 17    | O2            || null   | true      || Like kind with proper component identifier
        /// | 28 | true       | SO2C   | SGood | true      | CGood  | SO2      | LKA       | 17    | SO2           || null   | true      || Like kind with proper component identifier
        /// | 29 | true       | CO2C   | SOnly | true      | CGood  | CO2      | GOO       | 01    | CO2           || D      | false     || Monitor System Component does not exist (system)
        /// | 30 | true       | CO2CSD | SOnly | true      | CGood  | CO2      | GOO       | 06    | CO2           || D      | false     || Monitor System Component does not exist (system)
        /// | 31 | true       | FLOW   | SOnly | true      | CGood  | FLOW     | GOO       | 01    | FLOW          || D      | false     || Monitor System Component does not exist (system)
        /// | 32 | true       | H2O    | SOnly | true      | CGood  | H2O      | GOO       | 01    | H2O           || D      | false     || Monitor System Component does not exist (system)
        /// | 33 | true       | NOXC   | SOnly | true      | CGood  | NOX      | GOO       | 01    | NOX           || D      | false     || Monitor System Component does not exist (system)
        /// | 34 | true       | O2D    | SOnly | true      | CGood  | O2       | GOO       | 01    | O2            || D      | false     || Monitor System Component does not exist (system)
        /// | 35 | true       | O2W    | SOnly | true      | CGood  | O2       | GOO       | 01    | O2            || D      | false     || Monitor System Component does not exist (system)
        /// | 36 | true       | O2CSD  | SOnly | true      | CGood  | O2       | GOO       | 06    | O2            || D      | false     || Monitor System Component does not exist (system)
        /// | 37 | true       | SO2C   | SOnly | true      | CGood  | SO2      | GOO       | 01    | SO2           || D      | false     || Monitor System Component does not exist (system)
        /// | 38 | true       | CO2C   | SGood | true      | COnly  | CO2      | ONL       | 01    | CO2           || D      | false     || Monitor System Component does not exist (component)
        /// | 39 | true       | CO2CSD | SGood | true      | COnly  | CO2      | ONL       | 06    | CO2           || D      | false     || Monitor System Component does not exist (component)
        /// | 40 | true       | FLOW   | SGood | true      | COnly  | FLOW     | ONL       | 01    | FLOW          || D      | false     || Monitor System Component does not exist (component)
        /// | 41 | true       | H2O    | SGood | true      | COnly  | H2O      | ONL       | 01    | H2O           || D      | false     || Monitor System Component does not exist (component)
        /// | 42 | true       | NOXC   | SGood | true      | COnly  | NOX      | ONL       | 01    | NOX           || D      | false     || Monitor System Component does not exist (component)
        /// | 43 | true       | O2D    | SGood | true      | COnly  | O2       | ONL       | 01    | O2            || D      | false     || Monitor System Component does not exist (component)
        /// | 44 | true       | O2W    | SGood | true      | COnly  | O2       | ONL       | 01    | O2            || D      | false     || Monitor System Component does not exist (component)
        /// | 45 | true       | O2CSD  | SGood | true      | COnly  | O2       | ONL       | 06    | O2            || D      | false     || Monitor System Component does not exist (component)
        /// | 46 | true       | SO2C   | SGood | true      | COnly  | SO2      | ONL       | 01    | SO2           || D      | false     || Monitor System Component does not exist (component)
        /// | 47 | true       | CO2C   | SGood | true      | CGood  | CO2      | GOO       | 01    | CO2           || null   | true      || Monitor System Component exists
        /// | 48 | true       | CO2CSD | SGood | true      | CGood  | CO2      | GOO       | 06    | CO2           || null   | true      || Monitor System Component exists
        /// | 49 | true       | FLOW   | SGood | true      | CGood  | FLOW     | GOO       | 01    | FLOW          || null   | true      || Monitor System Component exists
        /// | 50 | true       | H2O    | SGood | true      | CGood  | H2O      | GOO       | 01    | H2O           || null   | true      || Monitor System Component exists
        /// | 51 | true       | NOXC   | SGood | true      | CGood  | NOX      | GOO       | 01    | NOX           || null   | true      || Monitor System Component exists
        /// | 52 | true       | O2D    | SGood | true      | CGood  | O2       | GOO       | 01    | O2            || null   | true      || Monitor System Component exists
        /// | 53 | true       | O2W    | SGood | true      | CGood  | O2       | GOO       | 01    | O2            || null   | true      || Monitor System Component exists
        /// | 54 | true       | O2CSD  | SGood | true      | CGood  | O2       | GOO       | 06    | O2            || null   | true      || Monitor System Component exists
        /// | 55 | true       | SO2C   | SGood | true      | CGood  | SO2      | GOO       | 01    | SO2           || null   | true      || Monitor System Component exists
        /// | 56 | true       | CO2C   | SGood | true      | null   | CO2      | GOO       | [all] | CO2           || [note] | {!Result} || COMPONENT_ID is null.  Result is A if MODC is in MODC set for the parameter, otherwise it is F.
        /// | 57 | true       | CO2CSD | SGood | true      | null   | CO2      | GOO       | [all] | CO2           || [note] | {!Result} || COMPONENT_ID is null.  Result is A if MODC is in MODC set for the parameter, otherwise it is F.
        /// | 58 | true       | FLOW   | SGood | true      | null   | FLOW     | GOO       | [all] | FLOW          || [note] | {!Result} || COMPONENT_ID is null.  Result is A if MODC is in MODC set for the parameter, otherwise it is F.
        /// | 59 | true       | FLOW   | SFlow | true      | null   | FLOW     | GOO       | [all] | FLOW          || null   | true      || COMPONENT_ID is null for a FLOW MHV where the MON_SYS_ID is connected to two flow components.
        /// | 60 | true       | H2O    | SGood | true      | null   | H2O      | GOO       | [all] | H2O           || [note] | {!Result} || COMPONENT_ID is null.  Result is A if MODC is in MODC set for the parameter, otherwise it is F.
        /// | 61 | true       | NOXC   | SGood | true      | null   | NOX      | GOO       | [all] | NOX           || [note] | {!Result} || COMPONENT_ID is null.  Result is A if MODC is in MODC set for the parameter, otherwise it is F.
        /// | 62 | true       | O2D    | SGood | true      | null   | O2       | GOO       | [all] | O2            || [note] | {!Result} || COMPONENT_ID is null.  Result is A if MODC is in MODC set for the parameter, otherwise it is F.
        /// | 63 | true       | O2W    | SGood | true      | null   | O2       | GOO       | [all] | O2            || [note] | {!Result} || COMPONENT_ID is null.  Result is A if MODC is in MODC set for the parameter, otherwise it is F.
        /// | 64 | true       | O2CSD  | SGood | true      | null   | O2       | GOO       | [all] | O2            || [note] | {!Result} || COMPONENT_ID is null.  Result is A if MODC is in MODC set for the parameter, otherwise it is F.
        /// | 65 | true       | SO2C   | SGood | true      | null   | SO2      | GOO       | [all] | SO2           || [note] | {!Result} || COMPONENT_ID is null.  Result is A if MODC is in MODC set for the parameter, otherwise it is F.
        /// | 66 | true       | CO2C   | null  | true      | CGood  | CO2      | GOO       | 01    | CO2           || null   | true      || Current MHV Mon Sys Record is null.
        /// | 67 | true       | CO2C   | SGood | false     | CGood  | CO2      | GOO       | 01    | CO2           || null   | true      || Monitor Hourly System Status is false.
        /// | 68 | true       | CO2C   | SGood | true      | CGood  | CO2      | LKA       | [all] | CO2           || null   | true      || Like kind component identifier.  Result is G if MODC is not 17, otherwise it is null.
        /// | 69 | true       | NOXC   | SGood | true      | CGood  | NOX      | LKA       | [all] | NOX           || null   | true      || Like kind component identifier.  Result is G if MODC is not 17, otherwise it is null
        /// | 70 | true       | O2D    | SGood | true      | CGood  | O2       | LKA       | [all] | O2            || null   | true      || Like kind component identifier.  Result is G if MODC is not 17, otherwise it is null
        /// | 71 | true       | O2W    | SGood | true      | CGood  | O2       | LKA       | [all] | O2            || null   | true      || Like kind component identifier.  Result is G if MODC is not 17, otherwise it is null
        /// | 72 | true       | SO2C   | SGood | true      | CGood  | SO2      | LKA       | [all] | SO2           || null   | true      || Like kind component identifier.  Result is G if MODC is not 17, otherwise it is null
        /// 
        /// </summary>
        [TestMethod()]
        public void HourMhv15()
        {
            /* Initialize objects generally needed for testing checks. */
            cHourlyMonitorValueChecks target = new cHourlyMonitorValueChecks(new cEmissionsCheckParameters());
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;


            /* MODC Lists */
            string[] modcAll = UnitTestStandardLists.ModcCodeList;
            string[] modc01 = { "01" };
            string[] modc06 = { "06" };
            string[] modc17 = { "17" };

            /* Measured MODC Dictionaries */
            Dictionary<string, List<string>> measuredModcDictionary = new Dictionary<string, List<string>>();
            {
                measuredModcDictionary["CO2C"] = new List<string> { "01", "02", "03", "04", "17", "18", "20", "21", "53" };
                measuredModcDictionary["CO2CSD"] = new List<string> { };
                measuredModcDictionary["FLOW"] = new List<string> { "01", "02", "03", "04", "20", "53" };
                measuredModcDictionary["H2O"] = new List<string> { "01", "02", "03", "04", "21", "53" };
                measuredModcDictionary["NOXC"] = new List<string> { "01", "02", "03", "04", "17", "18", "19", "20", "21", "22", "53" };
                measuredModcDictionary["O2D"] = new List<string> { "01", "02", "03", "04", "17", "18", "20", "21", "53" };
                measuredModcDictionary["O2W"] = new List<string> { "01", "02", "03", "04", "17", "18", "20", "21", "53" };
                measuredModcDictionary["O2CSD"] = new List<string> { };
                measuredModcDictionary["SO2C"] = new List<string> { "01", "02", "03", "04", "16", "17", "18", "19", "20", "21", "22", "53" };
            }

            /* System Type Code Lists*/
            string[] sysTypeAll = UnitTestStandardLists.SystemTypeCodeList;
            string[] sysTypeCo2 = { "CO2" };
            string[] sysTypeFlow = { "FLOW" };
            string[] sysTypeH2o = { "H2O" };
            string[] sysTypeNox = { "NOX" };
            string[] sysTypeO2 = { "O2" };
            string[] sysTypeSo2 = { "SO2" };

            /* Result Actions */
            Func<string, string, string> resultActionNull = (mp, mm) => null;
            Func<string, string, string> resultActionAandF = (mp, mm) => measuredModcDictionary.ContainsKey(mp) && measuredModcDictionary[mp].Contains(mm) ? "A" : "F";
            Func<string, string, string> resultActionB = (mp, mm) => "B";
            Func<string, string, string> resultActionC = (mp, mm) => "C";
            Func<string, string, string> resultActionD = (mp, mm) => "D";
            Func<string, string, string> resultActionLkComp = (mp, mm) => (mm == "17") ? null : "G";

            /* Status Actions */
            Func<string, bool> statusActionFalse = (rs) => false;
            Func<string, bool> statusActionResult = (rs) => rs.IsEmpty();


            /* Input Parameter Values */
            bool?[] modcStatusList = { null, false, false, false, false, false, false, false, false, false,
                                       true, true, true, true, true, true, true, true, true, true,
                                       true, true, true, true, true, true, true, true, true, true,
                                       true, true, true, true, true, true, true, true, true, true ,
                                       true, true, true, true, true, true, true, true, true, true ,
                                       true, true, true, true, true, true, true, true, true, true ,
                                       true, true, true, true, true, true, true, true, true, true ,
                                       true, true, true  };
            string[] parameterList = { "CO2C", "CO2C", "CO2CSD", "FLOW", "H2O", "NOXC", "O2D", "O2W", "O2CSD", "SO2C",
                                       "CO2C", "CO2CSD", "FLOW", "H2O", "NOXC", "O2D", "O2W", "O2CSD", "SO2C", "CO2C",
                                       "NOXC", "O2D", "O2W", "SO2C", "CO2C", "NOXC", "O2D", "O2W", "SO2C", "CO2C",
                                       "CO2CSD", "FLOW", "H2O", "NOXC", "O2D", "O2W", "O2CSD", "SO2C", "CO2C", "CO2CSD",
                                       "FLOW", "H2O", "NOXC", "O2D", "O2W", "O2CSD", "SO2C", "CO2C", "CO2CSD", "FLOW",
                                       "H2O", "NOXC", "O2D", "O2W", "O2CSD", "SO2C", "CO2C", "CO2CSD", "FLOW", "FLOW",
                                       "H2O", "NOXC", "O2D", "O2W", "O2CSD", "SO2C", "CO2C", "CO2C", "CO2C", "NOXC",
                                       "O2D", "O2W", "SO2C" };
            string[] systemIdList = { "SGood", "SGood", "SGood", "SGood", "SGood", "SGood", "SGood", "SGood", "SGood", "SGood",
                                      "SGood", "SGood", "SGood", "SGood", "SGood", "SGood", "SGood", "SGood", "SGood", "SGood",
                                      "SGood", "SGood", "SGood", "SGood", "SGood", "SGood", "SGood", "SGood", "SGood", "SOnly",
                                      "SOnly", "SOnly", "SOnly", "SOnly", "SOnly", "SOnly", "SOnly", "SOnly", "SGood", "SGood",
                                      "SGood", "SGood", "SGood", "SGood", "SGood", "SGood", "SGood", "SGood", "SGood", "SGood",
                                      "SGood", "SGood", "SGood", "SGood", "SGood", "SGood", "SGood", "SGood", "SGood", "SFlow",
                                      "SGood", "SGood", "SGood", "SGood", "SGood", "SGood", null, "SGood", "SGood", "SGood",
                                      "SGood", "SGood", "SGood" };
            bool?[] sysStatusList = { true, true, true, true, true, true, true, true, true, true,
                                      true, true, true, true, true, true, true, true, true, true,
                                      true, true, true, true, true, true, true, true, true, true,
                                      true, true, true, true, true, true, true, true, true, true,
                                      true, true, true, true, true, true, true, true, true, true,
                                      true, true, true, true, true, true, true, true, true, true,
                                      true, true, true, true, true, true, true, false, true, true,
                                      true, true, true  };
            string[] componentIdList = { "CGood", "CGood", "CGood", "CGood", "CGood", "CGood", "CGood", "CGood", "CGood", "CGood",
                                         "CGood", "CGood", "CGood", "CGood", "CGood", "CGood", "CGood", "CGood", "CGood", "CGood",
                                         "CGood", "CGood", "CGood", "CGood", "CGood", "CGood", "CGood", "CGood", "CGood", "CGood",
                                         "CGood", "CGood", "CGood", "CGood", "CGood", "CGood", "CGood", "CGood", "COnly", "COnly",
                                         "COnly", "COnly", "COnly", "COnly", "COnly", "COnly", "COnly", "CGood", "CGood", "CGood",
                                         "CGood", "CGood", "CGood", "CGood", "CGood", "CGood", null, null, null, null,
                                         null, null, null, null, null, null, "CGood", "CGood", "CGood", "CGood",
                                         "CGood", "CGood", "CGood" };
            string[] componentTypeList = { "CO2", "CO2", "CO2", "FLOW", "H2O", "NOX", "O2", "O2", "O2", "SO2",
                                           "BAD", "BAD", "BAD", "BAD", "BAD", "BAD", "BAD", "BAD", "BAD", "CO2",
                                           "NOX", "O2", "O2", "SO2", "CO2", "NOX", "O2", "O2", "SO2", "CO2",
                                           "CO2", "FLOW", "H2O", "NOX", "O2", "O2", "O2", "SO2", "CO2", "CO2",
                                           "FLOW", "H2O", "NOX", "O2", "O2", "O2", "SO2", "CO2", "CO2", "FLOW",
                                           "H2O", "NOX", "O2", "O2", "O2", "SO2", "CO2", "CO2", "FLOW", "FLOW",
                                           "H2O", "NOX", "O2", "O2", "O2", "SO2", "CO2", "CO2", "CO2", "NOX",
                                           "O2", "O2", "SO2" };
            string[] componentIdentList = { "GOO", "GOO", "GOO", "GOO", "GOO", "GOO", "GOO", "GOO", "GOO", "GOO",
                                            "GOO", "GOO", "GOO", "GOO", "GOO", "GOO", "GOO", "GOO", "GOO", "GOO",
                                            "GOO", "GOO", "GOO", "GOO", "LKA", "LKA", "LKA", "LKA", "LKA", "GOO",
                                            "GOO", "GOO", "GOO", "GOO", "GOO", "GOO", "GOO", "GOO", "ONL", "ONL",
                                            "ONL", "ONL", "ONL", "ONL", "ONL", "ONL", "ONL", "GOO", "GOO", "GOO",
                                            "GOO", "GOO", "GOO", "GOO", "GOO", "GOO", "GOO", "GOO", "GOO", "GOO",
                                            "GOO", "GOO", "GOO", "GOO", "GOO", "GOO", "GOO", "GOO", "LKA", "LKA",
                                            "LKA", "LKA", "LKA" };
            string[][] modcSetList = { modc01, modc01, modc06, modc01, modc01, modc01, modc01, modc01, modc06, modc01,
                                       modc01, modc06, modc01, modc01, modc01, modc01, modc01, modc06, modc01, modc17,
                                       modc17, modc17, modc17, modc17, modc17, modc17, modc17, modc17, modc17, modc01,
                                       modc06, modc01, modc01, modc01, modc01, modc01, modc06, modc01, modc01, modc06,
                                       modc01, modc01, modc01, modc01, modc01, modc06, modc01, modc01, modc06, modc01,
                                       modc01, modc01, modc01, modc01, modc06, modc01, modcAll, modcAll, modcAll, modcAll,
                                       modcAll, modcAll, modcAll, modcAll, modcAll, modcAll, modc01, modc01, modcAll, modcAll,
                                       modcAll, modcAll, modcAll};
            string[] paramCompTypeList = { "CO2", "CO2", "CO2", "FLOW", "H2O", "NOX", "O2", "O2", "O2", "SO2",
                                           "CO2", "CO2", "FLOW", "H2O", "NOX", "O2", "O2", "O2", "SO2", "CO2",
                                           "NOX", "O2", "O2", "SO2", "CO2", "NOX", "O2", "O2", "SO2", "CO2",
                                           "CO2", "FLOW", "H2O", "NOX", "O2", "O2", "O2", "SO2", "CO2", "CO2",
                                           "FLOW", "H2O", "NOX", "O2", "O2", "O2", "SO2", "CO2", "CO2", "FLOW",
                                           "H2O", "NOX", "O2", "O2", "O2", "SO2", "CO2", "CO2", "FLOW", "FLOW",
                                           "H2O", "NOX", "O2", "O2", "O2", "SO2", "CO2", "CO2", "CO2", "NOX",
                                           "O2", "O2", "SO2" };

            /* Expected Values */
            Func<string, string, string>[] expResultActionList = { resultActionNull, resultActionNull, resultActionNull, resultActionNull, resultActionNull,
                                                                   resultActionNull, resultActionNull, resultActionNull, resultActionNull, resultActionNull,
                                                                   resultActionB, resultActionB, resultActionB, resultActionB, resultActionB,
                                                                   resultActionB, resultActionB, resultActionB, resultActionB, resultActionC,
                                                                   resultActionC, resultActionC, resultActionC, resultActionC, resultActionNull,
                                                                   resultActionNull, resultActionNull, resultActionNull, resultActionNull, resultActionD,
                                                                   resultActionD, resultActionD, resultActionD, resultActionD, resultActionD,
                                                                   resultActionD, resultActionD, resultActionD, resultActionD, resultActionD,
                                                                   resultActionD, resultActionD, resultActionD, resultActionD, resultActionD,
                                                                   resultActionD, resultActionD, resultActionNull, resultActionNull, resultActionNull,
                                                                   resultActionNull, resultActionNull, resultActionNull, resultActionNull, resultActionNull,
                                                                   resultActionNull, resultActionAandF, resultActionAandF, resultActionAandF, resultActionNull,
                                                                   resultActionAandF, resultActionAandF, resultActionAandF, resultActionAandF, resultActionAandF,
                                                                   resultActionAandF, resultActionNull, resultActionNull, resultActionLkComp, resultActionLkComp,
                                                                   resultActionLkComp, resultActionLkComp, resultActionLkComp};
            Func<string, bool>[] expStatusActionList = { statusActionFalse, statusActionFalse, statusActionFalse, statusActionFalse, statusActionFalse,
                                                         statusActionFalse, statusActionFalse, statusActionFalse, statusActionFalse, statusActionFalse,
                                                         statusActionResult, statusActionResult, statusActionResult, statusActionResult, statusActionResult,
                                                         statusActionResult, statusActionResult, statusActionResult, statusActionResult, statusActionResult,
                                                         statusActionResult, statusActionResult, statusActionResult, statusActionResult, statusActionResult,
                                                         statusActionResult, statusActionResult, statusActionResult, statusActionResult, statusActionResult,
                                                         statusActionResult, statusActionResult, statusActionResult, statusActionResult, statusActionResult,
                                                         statusActionResult, statusActionResult, statusActionResult, statusActionResult, statusActionResult,
                                                         statusActionResult, statusActionResult, statusActionResult, statusActionResult, statusActionResult,
                                                         statusActionResult, statusActionResult, statusActionResult, statusActionResult, statusActionResult,
                                                         statusActionResult, statusActionResult, statusActionResult, statusActionResult, statusActionResult,
                                                         statusActionResult, statusActionResult, statusActionResult, statusActionResult, statusActionResult,
                                                         statusActionResult, statusActionResult, statusActionResult, statusActionResult, statusActionResult,
                                                         statusActionResult, statusActionResult, statusActionResult, statusActionResult, statusActionResult,
                                                         statusActionResult, statusActionResult, statusActionResult};

            /* Test Case Count */
            int caseCount = 73;

            /* Check array lengths */
            Assert.AreEqual(caseCount, modcStatusList.Length, "modcStatusList length");
            Assert.AreEqual(caseCount, parameterList.Length, "parameterList length");
            Assert.AreEqual(caseCount, systemIdList.Length, "systemIdList length");
            Assert.AreEqual(caseCount, sysStatusList.Length, "sysStatusList length");
            Assert.AreEqual(caseCount, componentIdList.Length, "componentIdList length");
            Assert.AreEqual(caseCount, componentTypeList.Length, "componentTypeList length");
            Assert.AreEqual(caseCount, componentIdentList.Length, "componentIdentList length");
            Assert.AreEqual(caseCount, modcSetList.Length, "modcSetList length");
            Assert.AreEqual(caseCount, paramCompTypeList.Length, "paramCompTypeList length");
            Assert.AreEqual(caseCount, expResultActionList.Length, "expResultActionList length");
            Assert.AreEqual(caseCount, expStatusActionList.Length, "expSystemIdList length");


            /* Run Cases */
            for (int caseDex = 0; caseDex < caseCount; caseDex++)
                foreach (string modcCd in modcSetList[caseDex])
                {
                    /* Initialize Input Parameters */
                    EmParameters.ComponentRecordsByLocation = new CheckDataView<VwMpComponentRow>
                        (
                           new VwMpComponentRow(componentId: "Other", componentTypeCd: "OTHER", componentIdentifier: "OTH"),
                           new VwMpComponentRow(componentId: "COnly", componentTypeCd: componentTypeList[caseDex], componentIdentifier: "ONL"),
                           new VwMpComponentRow(componentId: "CGood", componentTypeCd: componentTypeList[caseDex], componentIdentifier: componentIdentList[caseDex]),
                           new VwMpComponentRow(componentId: "CFlw1", componentTypeCd: "FLOW", componentIdentifier: "FL1"),
                           new VwMpComponentRow(componentId: "CFlw2", componentTypeCd: "FLOW", componentIdentifier: "FL2")
                        );
                    EmParameters.CurrentMhvComponentType = paramCompTypeList[caseDex];
                    EmParameters.CurrentMhvMonSysRecord = systemIdList[caseDex] != null ? new VwMpMonitorSystemRow(monSysId: systemIdList[caseDex]) : null;
                    EmParameters.CurrentMhvParameter = parameterList[caseDex];
                    EmParameters.CurrentMhvParameterDescription = parameterList[caseDex] + " Description";
                    EmParameters.CurrentMhvRecord = new VwMpMonitorHrlyValueRow(componentId: componentIdList[caseDex], monSysId: systemIdList[caseDex], modcCd: modcCd, componentIdentifier: componentIdentList[caseDex]);
                    EmParameters.MonitorHourlyModcStatus = modcStatusList[caseDex];
                    EmParameters.MonitorHourlySystemStatus = sysStatusList[caseDex];
                    EmParameters.MonitorSystemComponentRecordsByHourLocation = new CheckDataView<VwMpMonitorSystemComponentRow>
                        (
                           new VwMpMonitorSystemComponentRow(componentId: "Other", componentTypeCd: "OTHER", monSysId: "SGood"),
                           new VwMpMonitorSystemComponentRow(componentId: "CGood", componentTypeCd: componentTypeList[caseDex], monSysId: "SGood"),
                           new VwMpMonitorSystemComponentRow(componentId: "CGood", componentTypeCd: componentTypeList[caseDex], monSysId: "SBad"),
                           new VwMpMonitorSystemComponentRow(componentId: "CFlw1", componentTypeCd: "FLOW", monSysId: "SFlow"),
                           new VwMpMonitorSystemComponentRow(componentId: "CFlw2", componentTypeCd: "FLOW", monSysId: "SFlow")
                        );

                    /* Initialize Output Parameters */
                    EmParameters.MonitorHourlyComponentStatus = null;


                    /* Init Cateogry Result */
                    category.CheckCatalogResult = null;

                    /* Initialize variables needed to run the check. */
                    bool log = false;

                    /* Run Check */
                    string actual = target.HOURMHV15(category, ref log);
                    string result = expResultActionList[caseDex](parameterList[caseDex], modcCd);
                    bool status = expStatusActionList[caseDex](result);

                    /* Check Results */
                    Assert.AreEqual(string.Empty, actual, string.Format("actual[case {0}, param {1}, modc {2}]", caseDex, parameterList[caseDex], modcCd));
                    Assert.AreEqual(false, log, string.Format("log [case {0}, param {1}, modc {2}]", caseDex, parameterList[caseDex], modcCd));

                    Assert.AreEqual(result, category.CheckCatalogResult, string.Format("CheckCatalogResult [case {0}, param {1}, modc {2}]", caseDex, parameterList[caseDex], modcCd));
                    Assert.AreEqual(status, EmParameters.MonitorHourlyComponentStatus, string.Format("MonitorHourlyComponentStatus [case {0}, param {1}, modc {2}]", caseDex, parameterList[caseDex], modcCd));
                }
        }


        /// <summary>
        /// 
        /// </summary>
        [TestMethod()]
        public void HOURHMV16()
        {
            //instantiated checks setup
            cHourlyMonitorValueChecks target = new cHourlyMonitorValueChecks(new cEmissionsCheckParameters());
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            // Variables
            bool log = false;
            string actual;

            //SO2C Result G - MODC 16 Unadjusted equal to 2.0
            {
                // Init Input
                EmParameters.CurrentMhvParameter = "SO2C";
                EmParameters.Co2ConcChecksNeededForHeatInput = false;
                EmParameters.Co2ConcChecksNeededForCo2MassCalc = true;
                EmParameters.CurrentMhvRecord = new VwMpMonitorHrlyValueRow(modcCd: "16", unadjustedHrlyValue: 2, adjustedHrlyValue: 2);

                // Init Output
                category.CheckCatalogResult = null;

                // Run Checks
                actual = target.HOURMHV16(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual("G", category.CheckCatalogResult, "Result [SO2C MODC = 16 Unadjusted = 2]");
            }

            //SO2C Result B - MODC 16 Unadjusted greater than 2.0
            {
                // Init Input
                EmParameters.CurrentMhvParameter = "SO2C";
                EmParameters.Co2ConcChecksNeededForHeatInput = false;
                EmParameters.Co2ConcChecksNeededForCo2MassCalc = true;
                EmParameters.CurrentMhvRecord = new VwMpMonitorHrlyValueRow(modcCd: "16", unadjustedHrlyValue: (decimal?)2.5, adjustedHrlyValue: 2);

                // Init Output
                category.CheckCatalogResult = null;

                // Run Checks
                actual = target.HOURMHV16(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual("B", category.CheckCatalogResult, "Result [SO2C MODC = 16 Unadjusted > 2]");
            }

            //SO2C Result Null - MODC 16 Unadjusted less than 2.0
            {
                // Init Input
                EmParameters.CurrentMhvParameter = "SO2C";
                EmParameters.Co2ConcChecksNeededForHeatInput = false;
                EmParameters.Co2ConcChecksNeededForCo2MassCalc = true;
                EmParameters.CurrentMhvRecord = new VwMpMonitorHrlyValueRow(modcCd: "16", unadjustedHrlyValue: (decimal?)1.5, adjustedHrlyValue: 2);

                // Init Output
                category.CheckCatalogResult = null;

                // Run Checks
                actual = target.HOURMHV16(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result [SO2C MODC = 16 Unadjusted < 2]");
            }
        }

        #region HOURMHV-26
        /// <summary>
        ///A test for HOURMHV-26_QAStatusParams
        ///</summary>()
        [TestMethod()]
        public void HOURMHV26_QAStatusParams()
        {
            //instantiated checks setup
            cHourlyMonitorValueChecks target = new cHourlyMonitorValueChecks(new cEmissionsCheckParameters());
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            // Variables
            bool log = false;
            string actual;

            //CO2C Result E - MATS Diluent true
            {
                // Init Input
                EmParameters.CurrentMhvRecord = new VwMpMonitorHrlyValueRow(componentId: "COMPONENTID", componentTypeCd: "HG",
                    sysDesignationCd: "SYSDESIGCODE", monSysId: "MONSYSID", sysTypeCd: "SYSTYPECD", modcCd: "01");
                EmParameters.MonitorSystemComponentRecordsByHourLocation = new CheckDataView<VwMpMonitorSystemComponentRow>();
                EmParameters.MonitorHourlyModcStatus = false;
                EmParameters.RataStatusRequired = true;
                EmParameters.LinearityStatusRequired = false;
                EmParameters.DailyCalStatusRequired = false;


                // Init Output
                category.CheckCatalogResult = null;

                // Run Checks
                actual = target.HOURMHV26(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual("COMPONENTID", EmParameters.QaStatusComponentId, "QaStatusComponentId");
                Assert.AreEqual("HG", EmParameters.QaStatusComponentTypeCode, "QaStatusComponentTypeCode");
                Assert.AreEqual("SYSDESIGCODE", EmParameters.QaStatusSystemDesignationCode, "QaStatusSystemDesignationCode");
                Assert.AreEqual("MONSYSID", EmParameters.QaStatusSystemId, "QaStatusSystemId");
                Assert.AreEqual("SYSTYPECD", EmParameters.QaStatusSystemTypeCode, "QaStatusSystemTypeCode");

            }
        }
        #endregion


        /// <summary>
        /// 
        /// MODC: 01
        /// 
        /// | ## | Parameter | Value || Result || Note
        /// |  0 | H2O       | 100.0 || null   || Parameter not covered by the check.
        /// |  1 | O2C       | 100.0 || null   || Not an actual O2 concentration parameter.
        /// |  2 | CO2C      |  16.1 || A      || CO2C threshold exceeded.
        /// |  3 | CO2CSD    |  16.1 || A      || CO2C threshold exceeded.
        /// |  4 | O2D       |  22.1 || B      || O2C threshold exceeded.
        /// |  5 | O2W       |  22.1 || B      || O2C threshold exceeded.
        /// |  6 | O2CSD     |  22.1 || B      || O2C threshold exceeded.
        /// |  7 | CO2C      |  16.0 || null   || CO2C threshold not exceeded.
        /// |  8 | CO2CSD    |  16.0 || null   || CO2C threshold not exceeded.
        /// |  9 | O2D       |  22.0 || null   || O2C threshold not exceeded.
        /// | 10 | O2W       |  22.0 || null   || O2C threshold not exceeded.
        /// | 11 | O2CSD     |  22.0 || null   || O2C threshold not exceeded.
        /// </summary>
        [TestMethod()]
        public void HourMhv28()
        {
            /* Initialize objects generally needed for testing checks. */
            cHourlyMonitorValueChecks target = new cHourlyMonitorValueChecks(new cEmissionsCheckParameters());
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;


            /* Input Parameter Values */
            string[] parameterList = { "H2O", "O2C", "CO2C", "CO2CSD", "O2D", "O2W", "O2CSD", "CO2C", "CO2CSD", "O2D", "O2W", "O2CSD" };
            decimal?[] valueList = { 100.0m, 100.0m, 16.1m, 16.1m, 22.1m, 22.1m, 22.1m, 16.0m, 16.0m, 22.0m, 22.0m, 22.0m };

            /* Expected Values */
            string[] expResultList = { null, null, "A", "A", "B", "B", "B", null, null, null, null, null };

            /* Test Case Count */
            int caseCount = 12;

            /* Check array lengths */
            Assert.AreEqual(caseCount, parameterList.Length, "parameterList length");
            Assert.AreEqual(caseCount, valueList.Length, "valueList length");
            Assert.AreEqual(caseCount, expResultList.Length, "expResultList length");


            /* Run Cases */
            for (int caseDex = 0; caseDex < caseCount; caseDex++)
            {
                /* Initialize Input Parameters */
                EmParameters.CurrentMhvParameter = parameterList[caseDex];
                EmParameters.CurrentMhvRecord = new VwMpMonitorHrlyValueRow(unadjustedHrlyValue: valueList[caseDex], modcCd: "01");


                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Initialize variables needed to run the check. */
                bool log = false;
                string actual;

                /* Run Check */
                actual = target.HOURMHV28(category, ref log);


                /* Check Results */
                Assert.AreEqual(string.Empty, actual, string.Format("actual[case {0}]", caseDex));
                Assert.AreEqual(false, log, string.Format("log [case {0}]", caseDex));
                Assert.AreEqual(expResultList[caseDex], category.CheckCatalogResult, string.Format("category.CheckCatalogResult [case {0}]", caseDex));
            }
        }


        /// <summary>
        /// 
        /// MODC: 01
        /// </summary>
        [TestMethod()]
        public void HourMhv28_MODC()
        {
            /* Initialize objects generally needed for testing checks. */
            cHourlyMonitorValueChecks target = new cHourlyMonitorValueChecks(new cEmissionsCheckParameters());
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;


            string[] parameterList = { "CO2C", "CO2CSD", "O2CSD", "O2D", "O2W" };
            string[] modcList = { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "32", "33", "34", "35", "36", "37", "38", "39", "40", "41", "42", "45", "53", "54", "55" };

            foreach (string parameterCd in parameterList)
                foreach (string modcCd in modcList)
                {
                    decimal value = parameterCd.StartsWith("CO2C") ? 16.1m : 22.1m;

                    /* Initialize Input Parameters */
                    EmParameters.CurrentMhvParameter = parameterCd;
                    EmParameters.CurrentMhvRecord = new VwMpMonitorHrlyValueRow(unadjustedHrlyValue: value, modcCd: modcCd);


                    /* Init Cateogry Result */
                    category.CheckCatalogResult = null;

                    /* Initialize variables needed to run the check. */
                    bool log = false;
                    string actual;

                    /* Run Check */
                    actual = target.HOURMHV28(category, ref log);


                    /* Check Results */
                    Assert.AreEqual(string.Empty, actual, "actual");
                    Assert.AreEqual(false, log, "log");

                    if (parameterCd.StartsWith("CO2C") && (modcCd == "01" || modcCd == "02"))
                        Assert.AreEqual("A", category.CheckCatalogResult, string.Format("category.CheckCatalogResult [{0}/{1}]", parameterCd, modcCd));
                    else if (parameterCd.StartsWith("O2") && (modcCd == "01" || modcCd == "02"))
                        Assert.AreEqual("B", category.CheckCatalogResult, string.Format("category.CheckCatalogResult [{0}/{1}]", parameterCd, modcCd));
                    else
                        Assert.AreEqual(null, category.CheckCatalogResult, string.Format("category.CheckCatalogResult [{0}/{1}]", parameterCd, modcCd));
                }
        }


        /// <summary>
        /// 
        /// Required Parameters:
        /// 
        ///     CurrentNoxrPrimaryOrPrimaryBypassMhvRecord.MhvKey      : MhvId
        ///     CurrentNoxrPrimaryOrPrimaryBypassMhvRecord.ParameterCd : [Param]
        /// 
        /// Optional Parameters:
        /// 
        ///     Co2DiluentChecksNeededForNoxRateCalc                    : [Co2Needed]
        ///     O2DryChecksNeededForNoxrRateCalc                        : [O2dNeeded]
        ///     O2WetChecksNeededForNoxrRateCalc                        : [O2wNeeded]
        /// 
        /// Output Parameters:
        /// 
        ///     CompleteMhvRecordNeeded                                 : false
        ///     CurrentMhvComponentType                                 : [CType]
        ///     CurrentMhvDefaultParameter                              : [DParam]
        ///     CurrentMhvFuelSpecificHour                              : false
        ///     CurrentMhvHbhaValue                                     : null
        ///     CurrentMhvParameter                                     : [CParam]
        ///     CurrentMhvParameteDescrption                            : [CParam]
        ///     CurrentMhvParameterStatus                               : [Status]
        ///     CurrentMhvRecord                                        : { Key: MhvId, ParameterCd: [Param] }
        ///     MonitorHourlyModcStatus                                 : true
        /// 
        /// 
        /// | ## | Param | Co2Needed | O2dNeeded | O2wNeeded || Result | Status | CParam | Desc   | CType | DParam || Note
        /// |  0 | null  | false     | false     | false     || A      | false  | null   | null   | null  | null   || Unexpected parameter code.
        /// |  1 | CO2C  | false     | false     | false     || B      | false  | null   | null   | null  | null   || Expected parameter code without needed flag.
        /// |  2 | FLOW  | false     | false     | false     || A      | false  | null   | null   | null  | null   || Unexpected parameter code.
        /// |  3 | H2O   | false     | false     | false     || A      | false  | null   | null   | null  | null   || Unexpected parameter code.
        /// |  4 | NOXC  | false     | false     | false     || null   | true   | NOXC   | NOXC   | NOX   | NOCX   || Expected parameter code.
        /// |  5 | O2C   | false     | false     | false     || C      | false  | null   | null   | null  | null   || Expected parameter code without needed flag.
        /// |  6 | O2D   | false     | false     | false     || A      | false  | null   | null   | null  | null   || Unexpected parameter code.
        /// |  7 | O2W   | false     | false     | false     || A      | false  | null   | null   | null  | null   || Unexpected parameter code.
        /// |  8 | SO2C  | false     | false     | false     || A      | false  | null   | null   | null  | null   || Unexpected parameter code.
        /// |  9 | null  | true      | true      | true      || A      | false  | null   | null   | null  | null   || Unexpected parameter code.
        /// | 10 | CO2C  | true      | true      | true      || null   | true   | CO2C   | CO2C   | CO2   | CO2X   || Expected parameter code with needed flag.
        /// | 11 | FLOW  | true      | true      | true      || A      | false  | null   | null   | null  | null   || Unexpected parameter code.
        /// | 12 | H2O   | true      | true      | true      || A      | false  | H2O    | null   | null  | null   || Unexpected parameter code.
        /// | 13 | NOXC  | true      | true      | true      || null   | true   | NOXC   | NOXC   | NOX   | NOCX   || Expected parameter code.
        /// | 14 | O2C   | true      | true      | true      || C      | false  | null   | null   | null  | null   || Expected parameter code with two needed flags.
        /// | 15 | O2D   | true      | true      | true      || A      | false  | null   | null   | null  | CO2X   || Unexpected parameter code.
        /// | 16 | O2W   | true      | true      | true      || A      | false  | null   | null   | null  | CO2X   || Unexpected parameter code.
        /// | 17 | SO2C  | true      | true      | true      || A      | false  | null   | null   | null  | null   || Unexpected parameter code.
        /// | 18 | CO2C  | false     | true      | true      || B      | false  | null   | null   | null  | null   || Expected parameter code without needed flag.
        /// | 19 | O2C   | true      | true      | false     || null   | true   | O2D    | O2 Dry | O2    | O2N    || Expected parameter code with one needed flag.
        /// | 20 | O2C   | true      | false     | true      || null   | true   | O2W    | O2 Wet | O2    | O2N    || Expected parameter code with one needed flag.
        /// </summary>
        [TestMethod()]
        public void HourMhv29()
        {
            /* Initialize objects generally needed for testing checks. */
            cHourlyMonitorValueChecks target = new cHourlyMonitorValueChecks(new cEmissionsCheckParameters());
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;


            /* Input Parameter Values */
            string[] paramList = { null, "CO2C", "FLOW", "H2O", "NOXC", "O2C", "O2D", "O2W", "SO2C", null,
                                   "CO2C", "FLOW", "H2O", "NOXC", "O2C", "O2D", "O2W", "SO2C", "CO2C", "O2C",
                                   "O2C" };
            bool?[] co2NeededList = { false, false, false, false, false, false, false, false, false, true,
                                      true, true, true, true, true, true, true, true, false, true,
                                      true  };
            bool?[] o2dNeededList = { false, false, false, false, false, false, false, false, false, true,
                                      true, true, true, true, true, true, true, true, true, true,
                                      false  };
            bool?[] o2wNeededList = { false, false, false, false, false, false, false, false, false, true,
                                      true, true, true, true, true, true, true, true, true, false,
                                      true  };
            /* Expected Values */
            string[] expResultList = { "A", "B", "A", "A", null, "C", "A", "A", "A", "A",
                                       null, "A", "A", null, "C", "A", "A", "A", "B", null,
                                       null };
            bool?[] expStatusList = { false, false, false, false, true, false, false, false, false, false,
                                      true, false, false, true, false, false, false, false, false, true,
                                      true  };
            string[] expParamList = { null, null, null, null, "NOXC", null, null, null, null, null,
                                      "CO2C", null, null, "NOXC", null, null, null, null, null, "O2D",
                                      "O2W" };
            string[] expDescList = { null, null, null, null, "NOXC", null, null, null, null, null,
                                     "CO2C", null, null, "NOXC", null, null, null, null, null, "O2 Dry",
                                     "O2 Wet" };
            string[] expTypeList = { null, null, null, null, "NOX", null, null, null, null, null,
                                     "CO2", null, null, "NOX", null, null, null, null, null, "O2",
                                     "O2" };
            string[] expDefaultList = { null, null, null, null, "NOCX", null, null, null, null, null,
                                        "CO2X", null, null, "NOCX", null, null, null, null, null, "O2N",
                                        "O2N" };

            /* Test Case Count */
            int caseCount = 21;

            /* Check array lengths */
            Assert.AreEqual(caseCount, paramList.Length, "paramList length");
            Assert.AreEqual(caseCount, co2NeededList.Length, "co2NeededList length");
            Assert.AreEqual(caseCount, o2dNeededList.Length, "o2dNeededList length");
            Assert.AreEqual(caseCount, o2wNeededList.Length, "o2wNeededList length");
            Assert.AreEqual(caseCount, expResultList.Length, "expResultList length");
            Assert.AreEqual(caseCount, expStatusList.Length, "expStatusList length");
            Assert.AreEqual(caseCount, expParamList.Length, "expParamList length");
            Assert.AreEqual(caseCount, expDescList.Length, "expDescList length");
            Assert.AreEqual(caseCount, expTypeList.Length, "expTypeList length");
            Assert.AreEqual(caseCount, expDefaultList.Length, "expDefaultList length");


            /* Run Cases */
            for (int caseDex = 0; caseDex < caseCount; caseDex++)
            {
                /* Initialize Required Parameters */
                EmParameters.CurrentNoxrPrimaryOrPrimaryBypassMhvRecord = new NoxrPrimaryAndPrimaryBypassMhv(monitorHrlyValId: "MhvId", parameterCd: paramList[caseDex]);

                /* Initialize Optional Parameters */
                EmParameters.Co2DiluentChecksNeededForNoxRateCalc = co2NeededList[caseDex];
                EmParameters.O2DryChecksNeededForNoxRateCalc = o2dNeededList[caseDex];
                EmParameters.O2WetChecksNeededForNoxRateCalc = o2wNeededList[caseDex];

                /* Initialize Output Parameters */
                EmParameters.CompleteMhvRecordNeeded = null;
                EmParameters.CurrentMhvComponentType = "BAD";
                EmParameters.CurrentMhvDefaultParameter = "BAD";
                EmParameters.CurrentMhvFuelSpecificHour = null;
                EmParameters.CurrentMhvHbhaValue = -13m;
                EmParameters.CurrentMhvParameter = "BAD";
                EmParameters.CurrentMhvParameterDescription = "BAD";
                EmParameters.CurrentMhvParameterStatus = null;
                EmParameters.CurrentMhvRecord = new VwMpMonitorHrlyValueRow(monitorHrlyValId: "BAD", parameterCd: "BAD");
                EmParameters.MonitorHourlyModcStatus = null;


                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Initialize variables needed to run the check. */
                bool log = false;

                /* Run Check */
                string actual = target.HOURMHV29(category, ref log);

                /* Check Results */
                Assert.AreEqual(string.Empty, actual, string.Format("actual [case {0}, parameterCd {1}, co2c {2}, o2d {3}, o2w {4}]", caseDex, paramList[caseDex], co2NeededList[caseDex], o2dNeededList[caseDex], o2wNeededList[caseDex]));
                Assert.AreEqual(false, log, string.Format("log  [case {0}, parameterCd {1}, co2c {2}, o2d {3}, o2w {4}]", caseDex, paramList[caseDex], co2NeededList[caseDex], o2dNeededList[caseDex], o2wNeededList[caseDex]));

                Assert.AreEqual(expResultList[caseDex], category.CheckCatalogResult, string.Format("CheckCatalogResult  [case {0}, parameterCd {1}, co2c {2}, o2d {3}, o2w {4}]", caseDex, paramList[caseDex], co2NeededList[caseDex], o2dNeededList[caseDex], o2wNeededList[caseDex]));

                Assert.AreEqual(false, EmParameters.CompleteMhvRecordNeeded, string.Format("CompleteMhvRecordNeeded  [case {0}, parameterCd {1}, co2c {2}, o2d {3}, o2w {4}]", caseDex, paramList[caseDex], co2NeededList[caseDex], o2dNeededList[caseDex], o2wNeededList[caseDex]));
                Assert.AreEqual(expTypeList[caseDex], EmParameters.CurrentMhvComponentType, string.Format("CurrentMhvComponentType  [case {0}, parameterCd {1}, co2c {2}, o2d {3}, o2w {4}]", caseDex, paramList[caseDex], co2NeededList[caseDex], o2dNeededList[caseDex], o2wNeededList[caseDex]));
                Assert.AreEqual(expDefaultList[caseDex], EmParameters.CurrentMhvDefaultParameter, string.Format("CurrentMhvDefaultParameter  [case {0}, parameterCd {1}, co2c {2}, o2d {3}, o2w {4}]", caseDex, paramList[caseDex], co2NeededList[caseDex], o2dNeededList[caseDex], o2wNeededList[caseDex]));
                Assert.AreEqual(false, EmParameters.CurrentMhvFuelSpecificHour, string.Format("CurrentMhvFuelSpecificHour  [case {0}, parameterCd {1}, co2c {2}, o2d {3}, o2w {4}]", caseDex, paramList[caseDex], co2NeededList[caseDex], o2dNeededList[caseDex], o2wNeededList[caseDex]));
                Assert.AreEqual(null, EmParameters.CurrentMhvHbhaValue, string.Format("CurrentMhvHbhaValue  [case {0}, parameterCd {1}, co2c {2}, o2d {3}, o2w {4}]", caseDex, paramList[caseDex], co2NeededList[caseDex], o2dNeededList[caseDex], o2wNeededList[caseDex]));
                Assert.AreEqual(expParamList[caseDex], EmParameters.CurrentMhvParameter, string.Format("CurrentMhvParameter  [case {0}, parameterCd {1}, co2c {2}, o2d {3}, o2w {4}]", caseDex, paramList[caseDex], co2NeededList[caseDex], o2dNeededList[caseDex], o2wNeededList[caseDex]));
                Assert.AreEqual(expDescList[caseDex], EmParameters.CurrentMhvParameterDescription, string.Format("CurrentMhvParameterDescription  [case {0}, parameterCd {1}, co2c {2}, o2d {3}, o2w {4}]", caseDex, paramList[caseDex], co2NeededList[caseDex], o2dNeededList[caseDex], o2wNeededList[caseDex]));
                Assert.AreEqual(expStatusList[caseDex], EmParameters.CurrentMhvParameterStatus, string.Format("CurrentMhvParameterStatus  [case {0}, parameterCd {1}, co2c {2}, o2d {3}, o2w {4}]", caseDex, paramList[caseDex], co2NeededList[caseDex], o2dNeededList[caseDex], o2wNeededList[caseDex]));
                Assert.AreEqual(EmParameters.CurrentNoxrPrimaryOrPrimaryBypassMhvRecord.MonitorHrlyValId, EmParameters.CurrentMhvRecord.MonitorHrlyValId, string.Format("CurrentMhvRecord  [case {0}, parameterCd {1}, co2c {2}, o2d {3}, o2w {4}]", caseDex, paramList[caseDex], co2NeededList[caseDex], o2dNeededList[caseDex], o2wNeededList[caseDex]));
                Assert.AreEqual(true, EmParameters.MonitorHourlyModcStatus, string.Format("MonitorHourlyModcStatus  [case {0}, parameterCd {1}, co2c {2}, o2d {3}, o2w {4}]", caseDex, paramList[caseDex], co2NeededList[caseDex], o2dNeededList[caseDex], o2wNeededList[caseDex]));
            }
        }


        /// <summary>
        /// 
        /// Required Parameters:
        /// 
        ///     CurrentMvComponentType                                          : GOOD
        ///     CurrentMhvParameterStatus                                       : [status]
        ///     CurrentNoxrPrimaryOrPrimaryBypassMhvRecord.ComponentId          : [cmpId]
        ///     CurrentNoxrPrimaryOrPrimaryBypassMhvRecord.ComponentTypeCd      : [cmpType]
        ///     CurrentNoxrPrimaryOrPrimaryBypassMhvRecord.SystemDesignationInd : [desigInd]
        ///     
        /// Output Parameters:
        /// 
        ///     MonitorHHourlyComponentStatus                                   : [xStatus]
        ///     
        /// 
        /// | ## | status | cmpId | cmpType | desigInd || result | sStatus | sStatus || Note
        /// |  0 | null   | CMPID | GOOD    | 1        || null   | false   | false   || status is null.
        /// |  1 | false  | CMPID | GOOD    | 1        || null   | false   | false   || status is false.
        /// |  2 | true   | CMPID | GOOD    | 1        || null   | true    | true    || All Good.
        /// |  3 | true   | CMPID | GOOD    | null     || C      | true    | false   || Problem System Designation Indicator.
        /// |  4 | true   | CMPID | GOOD    | 0        || C      | true    | false   || Problem System Designation Indicator.
        /// |  5 | true   | CMPID | BAD     | 1        || B      | false   | false   || Problem Component Type.
        /// |  6 | true   | CMPID | BAD     | null     || B      | false   | false   || Problem Component Type and System Designation Indicator.
        /// |  7 | true   | CMPID | BAD     | 0        || B      | false   | false   || Problem Component Type and System Designation Indicator.
        /// |  8 | true   | null  | GOOD    | 1        || A      | false   | false   || Problem Component Id.
        /// |  9 | true   | null  | GOOD    | null     || A      | false   | false   || Problem Component Id and System Designation Indicator.
        /// | 10 | true   | null  | GOOD    | 0        || A      | false   | false   || Problem Component Id and System Designation Indicator.
        /// | 11 | true   | null  | BAD     | 1        || A      | false   | false   || Problem Component Id and Component Type.
        /// | 12 | true   | null  | BAD     | null     || A      | false   | false   || Problem Component Id, Component Type and System Designation Indicator.
        /// | 13 | true   | null  | BAD     | 0        || A      | false   | false   || Problem Component Id, Component Type and System Designation Indicator.
        /// 
        /// </summary>
        [TestMethod()]
        public void HourMhv30()
        {
            /* Initialize objects generally needed for testing checks. */
            cHourlyMonitorValueChecks target = new cHourlyMonitorValueChecks(new cEmissionsCheckParameters());
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;


            /* Input Parameter Values */
            bool?[] statusList = { null, false, true, true, true, true, true, true, true, true, true, true, true, true };
            string[] cmpIdList = { "CMPID", "CMPID", "CMPID", "CMPID", "CMPID", "CMPID", "CMPID", "CMPID", null, null, null, null, null, null };
            string[] cmpTypeList = { "GOOD", "GOOD", "GOOD", "GOOD", "GOOD", "BAD", "BAD", "BAD", "GOOD", "GOOD", "GOOD", "BAD", "BAD", "BAD" };
            int?[] sysCountList = { 1, 1, 1, null, 0, 1, null, 0, 1, null, 0, 1, null, 0 };

            /* Expected Values */
            string[] expResultList = { null, null, null, "C", "C", "B", "B", "B", "A", "A", "A", "A", "A", "A" };
            bool?[] expCmpStatusList = { false, false, true, true, true, false, false, false, false, false, false, false, false, false };
            bool?[] expSysStatusList = { false, false, true, false, false, false, false, false, false, false, false, false, false, false };

            /* Test Case Count */
            int caseCount = 14;

            /* Check array lengths */
            Assert.AreEqual(caseCount, statusList.Length, "statusList length");
            Assert.AreEqual(caseCount, cmpIdList.Length, "cmpIdList length");
            Assert.AreEqual(caseCount, cmpTypeList.Length, "cmpTypeList length");
            Assert.AreEqual(caseCount, sysCountList.Length, "sysCountList length");
            Assert.AreEqual(caseCount, expResultList.Length, "expResultList length");
            Assert.AreEqual(caseCount, expSysStatusList.Length, "expStatusList length");


            /* Run Cases */
            for (int caseDex = 0; caseDex < caseCount; caseDex++)
            {
                /* Initialize Required Parameters */
                EmParameters.CurrentMhvComponentType = "GOOD";
                EmParameters.CurrentMhvParameterStatus = statusList[caseDex];
                EmParameters.CurrentNoxrPrimaryOrPrimaryBypassMhvRecord = new NoxrPrimaryAndPrimaryBypassMhv(componentId: cmpIdList[caseDex], componentTypeCd: cmpTypeList[caseDex], notReportedNoxrSystemCount: sysCountList[caseDex]);

                /* Initialize Output Parameters */
                EmParameters.MonitorHourlyComponentStatus = null;
                EmParameters.MonitorHourlySystemStatus = null;


                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Initialize variables needed to run the check. */
                bool log = false;

                /* Run Check */
                string actual = target.HOURMHV30(category, ref log);

                /* Check Results */
                Assert.AreEqual(string.Empty, actual, string.Format("actual [case {0}, status {1}, id {2}, type {3}, desig {4}]", caseDex, statusList[caseDex], cmpIdList[caseDex], cmpTypeList[caseDex], sysCountList[caseDex]));
                Assert.AreEqual(false, log, string.Format("log [case {0}, status {1}, id {2}, type {3}, desig {4}]", caseDex, statusList[caseDex], cmpIdList[caseDex], cmpTypeList[caseDex], sysCountList[caseDex]));

                Assert.AreEqual(expResultList[caseDex], category.CheckCatalogResult, string.Format("CheckCatalogResult [case {0}, status {1}, id {2}, type {3}, desig {4}]", caseDex, statusList[caseDex], cmpIdList[caseDex], cmpTypeList[caseDex], sysCountList[caseDex]));

                Assert.AreEqual(expCmpStatusList[caseDex], EmParameters.MonitorHourlyComponentStatus, string.Format("MonitorHourlyComponentStatus [case {0}, status {1}, id {2}, type {3}, desig {4}]", caseDex, statusList[caseDex], cmpIdList[caseDex], cmpTypeList[caseDex], sysCountList[caseDex]));
                Assert.AreEqual(expSysStatusList[caseDex], EmParameters.MonitorHourlySystemStatus, string.Format("MonitorHourlySystemStatus [case {0}, status {1}, id {2}, type {3}, desig {4}]", caseDex, statusList[caseDex], cmpIdList[caseDex], cmpTypeList[caseDex], sysCountList[caseDex]));
            }
        }


        /// <summary>
        /// 
        /// Require Parameters:
        /// 
        ///     CurrentMhvParameter                                                 : [Param]
        ///     CurrentMhvParameterStatus                                           : 
        ///     CurrentNoxrPrimaryOrPrimaryBypassMhvRecord.ModcCd                   : [modc]
        ///     CurrentNoxrPrimaryOrPrimaryBypassMhvRecord.HighSpanCount            : [HCnt]
        ///     CurrentNoxrPrimaryOrPrimaryBypassMhvRecord.HighSpanDefaultHighRange : [HDHR]
        ///     CurrentNoxrPrimaryOrPrimaryBypassMhvRecord.HighSpanFullScaleRange   : [HFSR]
        ///     CurrentNoxrPrimaryOrPrimaryBypassMhvRecord.LowSpanCount             : [LCnt]
        ///     CurrentNoxrPrimaryOrPrimaryBypassMhvRecord.LowSpanFullScaleRange    : [LFSR]
        ///     CurrentNoxrPrimaryOrPrimaryBypassMhvRecord.MaxDefaultCount          : [MCnt]
        ///     CurrentNoxrPrimaryOrPrimaryBypassMhvRecord.MaxDefaultValue          : [MVal]
        ///     
        /// Ouput Parameters:
        /// 
        ///     CurrentMhvMaxMinValue                                   : [MaxVal]
        ///     CurrentNoxrPrimaryOrPrimaryBypassMhvMaxValueDescription : [Desc]
        /// 
        /// 
        /// | ## | Status | Modc | Param | HCnt | HDHR  | HFSR  | LCnt | LFSR  | MCnt | MVal  || Result | MaxVal | Desc                         || Note
        /// |  0 | null   | 47   | CO2C  | 0    | null  | null  | 0    | null  | 0    | null  || null   | null   | null                         || CO2C with null Status.
        /// |  1 | false  | 47   | CO2C  | 0    | null  | null  | 0    | null  | 0    | null  || null   | null   | null                         || CO2C with false Status.
        /// |  2 | true   | 48   | CO2C  | 0    | null  | null  | 0    | null  | 0    | null  || null   | null   | null                         || CO2C with MODC 48.
        /// |  3 | true   | 47   | CO2C  | 0    | null  | null  | 0    | null  | 0    | null  || A      | null   | CO2 Span High Range          || CO2C with false Status.
        /// |  4 | true   | 47   | CO2C  | 0    | null  | null  | 1    | 1000  | 0    | null  || A      | null   | CO2 Span High Range          || CO2C with low span full scale range.
        /// |  5 | true   | 47   | CO2C  | 0    | null  | null  | 0    | null  | 1    | 1000  || A      | null   | CO2 Span High Range          || CO2C with max default value.
        /// |  6 | true   | 47   | CO2C  | 2    | 1000  | 500   | 0    | null  | 0    | null  || B      | null   | CO2 Span High Range          || CO2C with max default value.
        /// |  7 | true   | 47   | CO2C  | 1    | null  | 500   | 0    | null  | 0    | null  || null   | 1000   | CO2 Span High Range          || CO2C with high span full scale range.
        /// |  8 | true   | 47   | CO2C  | 1    | 1000  | null  | 0    | null  | 0    | null  || null   | 1000   | CO2 Span High Range          || CO2C with high span default high range.
        /// |  9 | true   | 47   | CO2C  | 1    | 999   | 500   | 0    | null  | 0    | null  || null   | 1000   | CO2 Span High Range          || CO2C with high span default high range and full scale range.
        /// | 10 | true   | 47   | CO2C  | 1    | 1001  | 500   | 0    | null  | 0    | null  || null   | 1001   | CO2 Span High Range          || CO2C with high span default high range and full scale range.
        /// | 11 | true   | 47   | CO2C  | 1    | null  | null  | 0    | null  | 0    | null  || C      | null   | CO2 Span High Range          || CO2C with null high span default high range and full scale range.
        /// | 12 | true   | 47   | CO2C  | 1    | null  | -1    | 0    | null  | 0    | null  || C      | null   | CO2 Span High Range          || CO2C with negative high span full scale range.
        /// | 13 | true   | 47   | CO2C  | 1    | -1    | null  | 0    | null  | 0    | null  || C      | null   | CO2 Span High Range          || CO2C with negative high span default high range.
        /// | 14 | true   | 47   | NOXC  | 0    | null  | null  | 0    | null  | 1    | 1000  || D      | null   | NOX Span                     || NOXC with max default value.
        /// | 15 | true   | 47   | NOXC  | 2    | null  | null  | 0    | null  | 0    | null  || E      | null   | NOX Span High Range          || NOXC with more than 1 high span.
        /// | 16 | true   | 47   | NOXC  | 0    | null  | null  | 2    | null  | 0    | null  || E      | null   | NOX Span Low Range           || NOXC with more than 1 low span.
        /// | 17 | true   | 47   | NOXC  | 2    | null  | null  | 2    | null  | 0    | null  || E      | null   | NOX Span                     || NOXC with more than 1 high span and more than 1 low span.
        /// | 18 | true   | 47   | NOXC  | 1    | 1001  | null  | 1    | 500   | 0    | null  || null   | 1001   | NOX Span High Range          || NOXC where High span default high range exists, low span full scale range exists, but high DHR is higher than 2 * low FSR.
        /// | 19 | true   | 47   | NOXC  | 1    | 1001  | null  | 1    | 501   | 0    | null  || null   | 1002   | NOX Span Low Range           || NOXC where High span default high range exists, low span full scale range exists, but high DHR is lower than 2 * low FSR.
        /// | 20 | true   | 47   | NOXC  | 1    | null  | 500   | 0    | null  | 0    | null  || null   | 1000   | NOX Span High Range          || NOXC where High span default high range does not exists, but the full scale range does exist.
        /// | 21 | true   | 47   | NOXC  | 1    | null  | null  | 0    | null  | 0    | null  || F      | null   | NOX Span High Range          || NOXC where High span default high range and full scale range do not exist.
        /// | 22 | true   | 47   | NOXC  | 1    | null  | -1    | 0    | null  | 0    | null  || F      | null   | NOX Span High Range          || NOXC where High span default high range does not exist and full scale range is negative.
        /// | 23 | true   | 47   | O2D   | 1    | 1000  | null  | 1    | 500   | 0    | null  || G      | null   | O2C NFS Missing Data Default || O2 Dry with high and low span.
        /// | 24 | true   | 47   | O2D   | 0    | null  | null  | 0    | null  | 2    | null  || H      | null   | O2C NFS Missing Data Default || O2 Dry with multiple default rows.
        /// | 25 | true   | 47   | O2D   | 0    | null  | null  | 0    | null  | 1    | null  || I      | null   | O2C NFS Missing Data Default || O2 Dry with null default value.
        /// | 26 | true   | 47   | O2D   | 0    | null  | null  | 0    | null  | 1    | -0.1  || I      | null   | O2C NFS Missing Data Default || O2 Dry with negative default value.
        /// | 27 | true   | 47   | O2D   | 0    | null  | null  | 0    | null  | 1    | 0.1   || null   | 0.1    | O2C NFS Missing Data Default || O2 Dry with positive default value.
        /// | 28 | true   | 47   | O2W   | 1    | 1000  | null  | 1    | 500   | 0    | null  || G      | null   | O2C NFS Missing Data Default || O2 Wet with high and low span.
        /// | 29 | true   | 47   | O2W   | 0    | null  | null  | 0    | null  | 2    | null  || H      | null   | O2C NFS Missing Data Default || O2 Wet with multiple default rows.
        /// | 28 | true   | 47   | O2W   | 0    | null  | null  | 0    | null  | 1    | 0.1   || null   | 0.1    | O2C NFS Missing Data Default || O2 Wet with positive default value.
        /// 
        /// </summary>
        [TestMethod()]
        public void NourMhv31()
        {
            /* Initialize objects generally needed for testing checks. */
            cHourlyMonitorValueChecks target = new cHourlyMonitorValueChecks(new cEmissionsCheckParameters());
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;


            /* Input Parameter Values */
            bool?[] statusList = { null, false, true, true, true, true, true, true, true, true,
                                   true, true, true, true, true, true, true, true, true, true,
                                   true, true, true, true, true, true, true, true, true };
            string[] modcList = { "47", "47", "48", "47", "47", "47", "47", "47", "47", "47",
                                  "47", "47", "47", "47", "47", "47", "47", "47", "47", "47",
                                  "47", "47", "47", "47", "47", "47", "47", "47", "47" };
            string[] paramList = { "CO2C", "CO2C", "CO2C", "CO2C", "CO2C", "CO2C", "CO2C", "CO2C", "CO2C", "CO2C",
                                   "CO2C", "CO2C", "CO2C", "CO2C", "NOXC", "NOXC", "NOXC", "NOXC", "NOXC", "NOXC",
                                   "NOXC", "NOXC", "NOXC", "O2D", "O2D", "O2D", "O2W", "O2W", "O2W" };
            int?[] highSpanCountList = { 0, 0, 0, 0, 0, 0, 2, 1, 1, 1,
                                         1, 1, 1, 1, 0, 2, 0, 2, 1, 1,
                                         1, 1, 1, 1, 0, 0, 1, 0, 0 };
            decimal?[] highSpanDhrList = { null, null, null, null, null, null, 1000m, null, 1000m, 999m,
                                           1001m, null, null, -1, null, null, null, null, 1001m, 1001m,
                                           null, null, null, 1000m, null, null, 1000m, null, null };
            decimal?[] highSpanFsrList = { null, null, null, null, null, null, 500m, 500m, null, 500m,
                                           500m, null, -1m, null, null, null, null, null, null, null,
                                           500m, null, -1m, null, null, null, null, null, null };
            int?[] lowSpanCountList = { 0, 0, 0, 0, 1, 0, 0, 0, 0, 0,
                                        0, 0, 0, 0, 0, 0, 2, 2, 1, 1,
                                        0, 0, 0, 1, 0, 0, 1, 0, 0 };
            decimal?[] lowSpanFsrList = { null, null, null, null, 1000m, null, null, null, null, null,
                                          null, null, null, null, null, null, null, null, 500m, 501m,
                                          null, null, null, 500m, null, null, 500m, null, null };
            int?[] maxDefaultCountList = { 0, 0, 0, 0, 0, 1, 0, 0, 0, 0,
                                           0, 0, 0, 0, 1, 0, 0, 0, 0, 0,
                                           0, 0, 0, 0, 2, 1, 0, 2, 1 };
            decimal?[] maxDefaultValueList = { null, null, null, null, null, 1000m, null, null, null, null,
                                               null, null, null, null, 1000m, null, null, null, null, null,
                                               null, null, null, null, null, 0.1m, null, null, 0.1m };

            /* Expected Values */
            string[] expResultList = { null, null, null, "A", "A", "A", "B", null, null, null,
                                       null, "C", "C", "C", "D", "E", "E", "E", null, null,
                                       null, "F", "F", "G", "H", null, "G", "H", null };
            decimal?[] expValueList = { null, null, null, null, null, null, null, 1000m, 1000m, 1000m,
                                        1001m, null, null, null, null, null, null, null, 1001m, 1002m,
                                        1000m, null, null, null, null, 0.1m, null, null, 0.1m };
            string[] expDescList = { null, null, null, "CO2 Span High Range", "CO2 Span High Range",
                                     "CO2 Span High Range", "CO2 Span High Range", "CO2 Span High Range", "CO2 Span High Range", "CO2 Span High Range",
                                     "CO2 Span High Range", "CO2 Span High Range", "CO2 Span High Range", "CO2 Span High Range", "NOX Span",
                                     "NOX Span High Range", "NOX Span Low Range", "NOX Span", "NOX Span High Range", "NOX Span Low Range",
                                     "NOX Span High Range", "NOX Span High Range", "NOX Span High Range", "O2C NFS Missing Data Default", "O2C NFS Missing Data Default",
                                     "O2C NFS Missing Data Default", "O2C NFS Missing Data Default", "O2C NFS Missing Data Default", "O2C NFS Missing Data Default" };


            /* Test Case Count */
            int caseCount = 29;

            /* Check array lengths */
            Assert.AreEqual(caseCount, statusList.Length, "statusList length");
            Assert.AreEqual(caseCount, modcList.Length, "modcList length");
            Assert.AreEqual(caseCount, paramList.Length, "paramList length");
            Assert.AreEqual(caseCount, highSpanCountList.Length, "highSpanCountList length");
            Assert.AreEqual(caseCount, highSpanDhrList.Length, "highSpanDhrList length");
            Assert.AreEqual(caseCount, highSpanFsrList.Length, "highSpanFsrList length");
            Assert.AreEqual(caseCount, lowSpanCountList.Length, "lowSpanCountList length");
            Assert.AreEqual(caseCount, lowSpanFsrList.Length, "lowSpanFsrList length");
            Assert.AreEqual(caseCount, maxDefaultCountList.Length, "maxDefaultCountList length");
            Assert.AreEqual(caseCount, maxDefaultValueList.Length, "maxDefaultValueList length");
            Assert.AreEqual(caseCount, expResultList.Length, "expResultList length");
            Assert.AreEqual(caseCount, expValueList.Length, "expValueList length");
            Assert.AreEqual(caseCount, expDescList.Length, "expDescList length");


            /* Run Cases */
            for (int caseDex = 0; caseDex < caseCount; caseDex++)
            {
                /* Initialize Required Parameters */
                EmParameters.CurrentMhvParameter = paramList[caseDex];
                EmParameters.CurrentMhvParameterStatus = statusList[caseDex];
                EmParameters.CurrentNoxrPrimaryOrPrimaryBypassMhvRecord = new NoxrPrimaryAndPrimaryBypassMhv
                                                                              (
                                                                                  modcCd: modcList[caseDex],
                                                                                  highSpanCount: highSpanCountList[caseDex], highSpanDefaultHighRange: highSpanDhrList[caseDex], highSpanFullScaleRange: highSpanFsrList[caseDex],
                                                                                  lowSpanCount: lowSpanCountList[caseDex], lowSpanFullScaleRange: lowSpanFsrList[caseDex],
                                                                                  maxDefaultCount: maxDefaultCountList[caseDex], maxDefaultValue: maxDefaultValueList[caseDex]
                                                                              );

                /* Initialize Output Parameters */
                EmParameters.CurrentMhvMaxMinValue = decimal.MinValue;
                EmParameters.CurrentNoxrPrimaryOrPrimaryBypassMhvMaxValueDescription = "BAD";


                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Initialize variables needed to run the check. */
                bool log = false;

                /* Run Check */
                string actual = target.HOURMHV31(category, ref log);

                /* Check Results */
                Assert.AreEqual(string.Empty, actual, string.Format("actual [case {0}]", caseDex));
                Assert.AreEqual(false, log, string.Format("log [case {0}]", caseDex));

                Assert.AreEqual(expResultList[caseDex], category.CheckCatalogResult, string.Format("CheckCatalogResult [case {0}]", caseDex));

                Assert.AreEqual(expValueList[caseDex], EmParameters.CurrentMhvMaxMinValue, string.Format("CurrentMhvMaxMinValue [case {0}]", caseDex));
                Assert.AreEqual(expDescList[caseDex], EmParameters.CurrentNoxrPrimaryOrPrimaryBypassMhvMaxValueDescription, string.Format("CurrentNoxrPrimaryOrPrimaryBypassMhvMaxValueDescription [case {0}]", caseDex));
            }
        }


        /// <summary>
        /// 
        /// Required Parameters:
        /// 
        ///     CurrentNoxrPrimaryOrPrimaryBypassMhvRecord.AdjustedHrlyValue    : [Adjusted]
        ///     CurrentNoxrPrimaryOrPrimaryBypassMhvRecord.MoistureBasis        : [Basis]
        ///     CurrentNoxrPrimaryOrPrimaryBypassMhvRecord.MonSysId             : [SysId]
        ///     CurrentNoxrPrimaryOrPrimaryBypassMhvRecord.PctAvailable         : [Pma]
        /// 
        /// Output Parameters:
        /// 
        ///     CurrentMhvExtraneousFields                                      : [Fields]
        /// 
        /// 
        /// | ## | Adjusted | Basis | SysId | Pma  || Result | Fields                                                                     || Notes
        /// |  0 | null     | null  | null  | null || null   | ""                                                                         || All extrateous fields are null.
        /// |  1 | 50m      | null  | null  | null || A      | "AdjustedHourlyValue"                                                      || AdjustedHourlyValue is NOT null.
        /// |  2 | null     | D     | null  | null || A      | "MositureBasis"                                                            || MositureBasis is NOT null.
        /// |  3 | null     | null  | SYSID | null || A      | "MonitorSystemID"                                                          || MonitorSystemID is NOT null.
        /// |  4 | null     | null  | null  | 80m  || A      | "PercentAvailable "                                                        || PercentAvailable is NOT null.
        /// |  5 | 50m      | D     | SYSID | 80m  || A      | "AdjustedHourlyValue, MositureBasis, MonitorSystemID and PercentAvailable" || All extrateous fields are NOT null.
        /// </summary>
        [TestMethod()]
        public void HourMhv32()
        {
            /* Initialize objects generally needed for testing checks. */
            cHourlyMonitorValueChecks target = new cHourlyMonitorValueChecks(new cEmissionsCheckParameters());
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;


            /* Input Parameter Values */
            decimal?[] adjustedList = { null, 50m, null, null, null, 50m };
            string[] basisList = { null, null, "D", null, null, "D" };
            string[] sysIdList = { null, null, null, "SYSID", null, "SYSID" };
            decimal?[] pmaList = { null, null, null, null, 80m, 80m };

            /* Expected Values */
            string[] expResultList = { null, "A", "A", "A", "A", "A" };
            string[] expFiledList = { "", "AdjustedHourlyValue", "MoistureBasis", "MonitorSystemID", "PercentAvailable", "AdjustedHourlyValue, MoistureBasis, MonitorSystemID, and PercentAvailable" };

            /* Test Case Count */
            int caseCount = 6;

            /* Check array lengths */
            Assert.AreEqual(caseCount, adjustedList.Length, "adjustedList length");
            Assert.AreEqual(caseCount, basisList.Length, "basisList length");
            Assert.AreEqual(caseCount, sysIdList.Length, "sysIdList length");
            Assert.AreEqual(caseCount, pmaList.Length, "pmaList length");
            Assert.AreEqual(caseCount, expResultList.Length, "expResultList length");
            Assert.AreEqual(caseCount, expFiledList.Length, "expFiledList length");


            /* Run Cases */
            for (int caseDex = 0; caseDex < caseCount; caseDex++)
            {
                /* Initialize Required Parameters */
                EmParameters.CurrentNoxrPrimaryOrPrimaryBypassMhvRecord = new NoxrPrimaryAndPrimaryBypassMhv
                                                                              (
                                                                                  adjustedHrlyValue: adjustedList[caseDex], monSysId: sysIdList[caseDex], pctAvailable: pmaList[caseDex], moistureBasis: basisList[caseDex],
                                                                                  parameterCd: "PARAM", unadjustedHrlyValue: 13.0m, modcCd: "13", componentId: "CmpId"
                                                                              );

                /* Initialize Output Parameters */
                EmParameters.CurrentMhvExtraneousFields = "BAD";


                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Initialize variables needed to run the check. */
                bool log = false;

                /* Run Check */
                string actual = target.HOURMHV32(category, ref log);

                /* Check Results */
                Assert.AreEqual(string.Empty, actual, string.Format("actual [case {0}, adjusted {1}, basis {2}, system {3}, pma {4}]", caseDex, adjustedList[caseDex], basisList[caseDex], sysIdList[caseDex], pmaList[caseDex]));
                Assert.AreEqual(false, log, string.Format("log [case {0}]", caseDex));

                Assert.AreEqual(expResultList[caseDex], category.CheckCatalogResult, string.Format("CheckCatalogResult [case {0}, adjusted {1}, basis {2}, system {3}, pma {4}]", caseDex, adjustedList[caseDex], basisList[caseDex], sysIdList[caseDex], pmaList[caseDex]));

                Assert.AreEqual(expFiledList[caseDex], EmParameters.CurrentMhvExtraneousFields, string.Format("CurrentMhvExtraneousFields [case {0}, adjusted {1}, basis {2}, system {3}, pma {4}]", caseDex, adjustedList[caseDex], basisList[caseDex], sysIdList[caseDex], pmaList[caseDex]));
            }
        }


        /// <summary>
        /// 
        /// Required Parameters:
        /// 
        ///     CurrentHourlyOpRecord                       : [LoadRange]
        ///     CurrentMhvMaxMinValue                       : [MaxVal]
        ///     CurrentMhvParameter                         : [Param]
        ///     CurrentNoxrPrimaryOrPrimaryBypassMhvRecord  : { unadjustedHrlyValue: [Value], modcCd: [ModcCd]}
        /// 
        /// 
        /// | ## | Value   | ModcCd | Param | MaxVal | LoadRange || Result || Note
        /// |  0 | null    | 47     | NOXC  | 100.0m | 1         || A      || Null unadjusted with MODC 47.
        /// |  1 | null    | 48     | NOXC  | 100.0m | 1         || null   || Null unadjusted with MODC 48.
        /// |  2 | 100.0m  | 48     | NOXC  | 100.0m | 1         || B      || Populated unadjusted with MODC 48.
        /// |  3 | 100.0m  | 47     | NOXC  | 100.0m | 1         || null   || Valid unadjusted.
        /// |  4 | -0.1m   | 47     | NOXC  | 100.0m | 1         || C      || Negative unadjusted.
        /// |  5 | 1.01m   | 47     | NOXC  | 100.0m | 1         || D      || Unadjusted not rounded to one decimal place.
        /// |  6 | 101.0m  | 47     | NOXC  | 100.0m | 1         || H      || Unadjusted greater than max value.
        /// |  7 | 16.0m   | 47     | CO2C  | 100.0m | 1         || null   || CO2C less than or equal to 16.0%.
        /// |  8 | 16.1m   | 47     | CO2C  | 100.0m | 1         || E      || CO2C greater than 16.0%.
        /// |  9 | 22.0m   | 47     | O2D   | 100.0m | 1         || null   || O2D less than or equal to 22.0%.
        /// | 10 | 22.1m   | 47     | O2D   | 100.0m | 1         || F      || O2D greater than 22.0%.
        /// | 11 | 22.0m   | 47     | O2W   | 100.0m | 1         || null   || O2D less than or equal to 22.0%.
        /// | 12 | 22.1m   | 47     | O2W   | 100.0m | 1         || F      || O2D greater than 22.0%.
        /// | 13 | 0.0m    | 47     | CO2C  | 100.0m | 1         || null   || CO2C is 0% but load range is 1.
        /// | 14 | 0.0m    | 47     | CO2C  | 100.0m | 2         || G      || CO2C is 0% but load range greater than 1.
        /// 
        /// </summary>
        [TestMethod()]
        public void HourMhv33()
        {
            /* Initialize objects generally needed for testing checks. */
            cHourlyMonitorValueChecks target = new cHourlyMonitorValueChecks(new cEmissionsCheckParameters());
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;


            /* Input Parameter Values */
            decimal?[] valueList = { null, null, 100.0m, 100.0m, -0.1m, 1.01m, 101.0m, 16.0m, 16.1m, 22.0m, 22.1m, 22.0m, 22.1m, 0.0m, 0.0m };
            string[] modcList = { "47", "48", "48", "47", "47", "47", "47", "47", "47", "47", "47", "47", "47", "47", "47" };
            string[] paramList = { "NOXC", "NOXC", "NOXC", "NOXC", "NOXC", "NOXC", "NOXC", "CO2C", "CO2C", "O2D", "O2D", "O2W", "O2W", "CO2C", "CO2C" };
            decimal?[] maxValList = { 100.0m, 100.0m, 100.0m, 100.0m, 100.0m, 100.0m, 100.0m, 100.0m, 100.0m, 100.0m, 100.0m, 100.0m, 100.0m, 100.0m, 100.0m };
            int?[] loadRangeList = { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2 };

            /* Expected Values */
            string[] expResultList = { "A", null, "B", null, "C", "D", "H", null, "E", null, "F", null, "F", null, "G" };

            /* Test Case Count */
            int caseCount = 15;

            /* Check array lengths */
            Assert.AreEqual(caseCount, valueList.Length, "valueList length");
            Assert.AreEqual(caseCount, modcList.Length, "modcList length");
            Assert.AreEqual(caseCount, paramList.Length, "paramList length");
            Assert.AreEqual(caseCount, maxValList.Length, "maxValList length");
            Assert.AreEqual(caseCount, loadRangeList.Length, "lpadRangeList length");
            Assert.AreEqual(caseCount, expResultList.Length, "expResultList length");


            /* Run Cases */
            for (int caseDex = 0; caseDex < caseCount; caseDex++)
            {
                /* Initialize Required Parameters */
                EmParameters.CurrentHourlyOpRecord = new VwMpHrlyOpDataRow(loadRange: loadRangeList[caseDex]);
                EmParameters.CurrentMhvMaxMinValue = maxValList[caseDex];
                EmParameters.CurrentMhvParameter = paramList[caseDex];
                EmParameters.CurrentNoxrPrimaryOrPrimaryBypassMhvRecord = new NoxrPrimaryAndPrimaryBypassMhv(unadjustedHrlyValue: valueList[caseDex], modcCd: modcList[caseDex]);


                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Initialize variables needed to run the check. */
                bool log = false;

                /* Run Check */
                string actual = target.HOURMHV33(category, ref log);

                /* Check Results */
                Assert.AreEqual(string.Empty, actual, string.Format("actual [case {0}, value {1}, modc {2}, param {3}, maxVal {4}, loadRange {5}]", caseDex, valueList[caseDex], modcList[caseDex], paramList[caseDex], maxValList[caseDex], loadRangeList[caseDex]));
                Assert.AreEqual(false, log, string.Format("log[case {0}, value {1}, modc {2}, param {3}, maxVal {4}, loadRange {5}]", caseDex, valueList[caseDex], modcList[caseDex], paramList[caseDex], maxValList[caseDex], loadRangeList[caseDex]));

                Assert.AreEqual(expResultList[caseDex], category.CheckCatalogResult, string.Format("CheckCatalogResult [case {0}, value {1}, modc {2}, param {3}, maxVal {4}, loadRange {5}]", caseDex, valueList[caseDex], modcList[caseDex], paramList[caseDex], maxValList[caseDex], loadRangeList[caseDex]));
            }
        }


        /// <summary>
        /// 
        /// Required Parameters:
        /// 
        ///     CurrentNoxrPrimaryOrPrimaryBypassMhvRecord  : { PrimaryBypassExistsIndicator: [Ind]}
        /// 
        /// 
        /// | ## | Ind  || Result || Note
        /// |  0 | null || A      || PrimaryBypassExistsIndicator is null.
        /// |  1 | 0    || A      || PrimaryBypassExistsIndicator is 0.
        /// |  2 | 1    || null   || PrimaryBypassExistsIndicator is 1.
        /// 
        /// </summary>
        [TestMethod()]
        public void HourMhv34()
        {
            /* Initialize objects generally needed for testing checks. */
            cHourlyMonitorValueChecks target = new cHourlyMonitorValueChecks(new cEmissionsCheckParameters());
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;


            /* Input Parameter Values */
            int?[] indList = { null, 0, 1 };

            /* Expected Values */
            string[] expResultList = { "A", "A", null };

            /* Test Case Count */
            int caseCount = 3;

            /* Check array lengths */
            Assert.AreEqual(caseCount, indList.Length, "valueList length");
            Assert.AreEqual(caseCount, expResultList.Length, "expResultList length");


            /* Run Cases */
            for (int caseDex = 0; caseDex < caseCount; caseDex++)
            {
                /* Initialize Required Parameters */
                EmParameters.CurrentNoxrPrimaryOrPrimaryBypassMhvRecord = new NoxrPrimaryAndPrimaryBypassMhv(primaryBypassExistInd: indList[caseDex]);


                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Initialize variables needed to run the check. */
                bool log = false;

                /* Run Check */
                string actual = target.HOURMHV34(category, ref log);

                /* Check Results */
                Assert.AreEqual(string.Empty, actual, string.Format("actual [case {0}, indicator {1}]", caseDex, indList[caseDex]));
                Assert.AreEqual(false, log, string.Format("log [case {0}, indicator {1}]", caseDex, indList[caseDex]));

                Assert.AreEqual(expResultList[caseDex], category.CheckCatalogResult, string.Format("CheckCatalogResult [case {0}, indicator {1}]", caseDex, indList[caseDex]));
            }
        }


        /// <summary>
        /// 
        /// Required Parameters:
        /// 
        ///     CurrentNoxrPrimaryOrPrimaryBypassMhvRecord.PrimaryBypassExistsIndicator : [Bypass]
        ///     CurrentNoxrPrimaryOrPrimaryBypassMhvRecord.UsedNoxcCount                : [UsedNoxc]
        ///     CurrentNoxrPrimaryOrPrimaryBypassMhvRecord.UsedDiluentCount             : [UsedDiluent]
        ///     CurrentNoxrPrimaryOrPrimaryBypassMhvRecord.UnusedNoxcCount              : [UnusedNoxc]
        ///     CurrentNoxrPrimaryOrPrimaryBypassMhvRecord.UnusedDiluentCount           : [UnusedDiluent]
        /// 
        /// Output Parameters:
        /// 
        ///     CurrentMhvMissing                                                       : [Fields]
        /// 
        /// 
        /// | ## | Bypass | UsedNoxc | UsedDiluent | UnusedNoxc | UnusedDiluent  || Result | Fields                                                      || Notes
        /// |  0 | 1      | 1        | 1           | 1          | 1              || null   | ""                                                          || All counts are 1.
        /// |  1 | 1      | 0        | 1           | 1          | 1              || A      | "Used NOXC"                                                 || UsedNoxc count is zero.
        /// |  2 | 1      | 1        | 0           | 1          | 1              || A      | "Used CO2C/O2C"                                             || UsedDiluent count is zero.
        /// |  3 | 1      | 1        | 1           | 0          | 1              || A      | "Unused NOXC"                                               || UnusedNoxc count is zero.
        /// |  4 | 1      | 1        | 1           | 1          | 0              || A      | "Unused CO2C/O2C"                                           || UnusedDiluent count is zero.
        /// |  5 | 1      | 0        | 0           | 0          | 0              || A      | "Used NOXC, Used CO2C/O2C, Unused NOXC and Unused CO2C/O2C" || All counts are zero.
        /// |  6 | 0      | 0        | 0           | 0          | 0              || null   | ""                                                          || Bypass indicator is 0.
        /// |  7 | null   | 0        | 0           | 0          | 0              || null   | ""                                                          || Bypass indicator is null..
        /// </summary>
        [TestMethod()]
        public void HourMhv35()
        {
            /* Initialize objects generally needed for testing checks. */
            cHourlyMonitorValueChecks target = new cHourlyMonitorValueChecks(new cEmissionsCheckParameters());
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;


            /* Input Parameter Values */
            int?[] bypassIndList = { 1, 1, 1, 1, 1, 1, 0, null };
            int?[] usedNoxcList = { 1, 0, 1, 1, 1, 0, 0, 0 };
            int?[] usedDiluentList = { 1, 1, 0, 1, 1, 0, 0, 0 };
            int?[] unusedNoxcList = { 1, 1, 1, 0, 1, 0, 0, 0 };
            int?[] unusedDiluentList = { 1, 1, 1, 1, 0, 0, 0, 0 };

            /* Expected Values */
            string[] expResultList = { null, "A", "A", "A", "A", "A", null, null };
            string[] exMhvRowsList = { "", "Used NOXC", "Used CO2C/O2C", "Unused NOXC", "Unused CO2C/O2C", "Used NOXC, Used CO2C/O2C, Unused NOXC, and Unused CO2C/O2C", "", "" };

            /* Test Case Count */
            int caseCount = 8;

            /* Check array lengths */
            Assert.AreEqual(caseCount, bypassIndList.Length, "bypassIndList length");
            Assert.AreEqual(caseCount, usedNoxcList.Length, "usedNoxcList length");
            Assert.AreEqual(caseCount, usedDiluentList.Length, "usedDiluentList length");
            Assert.AreEqual(caseCount, unusedNoxcList.Length, "unusedNoxcList length");
            Assert.AreEqual(caseCount, unusedDiluentList.Length, "unusedDiluentList length");
            Assert.AreEqual(caseCount, expResultList.Length, "expResultList length");
            Assert.AreEqual(caseCount, exMhvRowsList.Length, "exMhvRowsList length");


            /* Run Cases */
            for (int caseDex = 0; caseDex < caseCount; caseDex++)
            {
                /* Initialize Required Parameters */
                EmParameters.CurrentNoxrPrimaryOrPrimaryBypassMhvRecord = new NoxrPrimaryAndPrimaryBypassMhv
                                                                              (
                                                                                  primaryBypassExistInd: bypassIndList[caseDex],
                                                                                  usedNoxcCount: usedNoxcList[caseDex], usedDiluentCount: usedDiluentList[caseDex],
                                                                                  unusedNoxcCount: unusedNoxcList[caseDex], unusedDiluentCount: unusedDiluentList[caseDex]
                                                                              );

                /* Initialize Output Parameters */
                EmParameters.CurrentMhvMissing = "BAD";


                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Initialize variables needed to run the check. */
                bool log = false;

                /* Run Check */
                string actual = target.HOURMHV35(category, ref log);

                /* Check Results */
                Assert.AreEqual(string.Empty, actual, string.Format("actual [case {0}, bypass {1}, usedNox {2}, usedDil {3}, unusedNox {4}, unusedDil {5}]", caseDex, bypassIndList[caseDex], usedNoxcList[caseDex], usedDiluentList[caseDex], unusedNoxcList[caseDex], unusedDiluentList[caseDex]));
                Assert.AreEqual(false, log, string.Format("log [case {0}, bypass {1}, usedNox {2}, usedDil {3}, unusedNox {4}, unusedDil {5}]", caseDex, bypassIndList[caseDex], usedNoxcList[caseDex], usedDiluentList[caseDex], unusedNoxcList[caseDex], unusedDiluentList[caseDex]));

                Assert.AreEqual(expResultList[caseDex], category.CheckCatalogResult, string.Format("CheckCatalogResult [case {0}, bypass {1}, usedNox {2}, usedDil {3}, unusedNox {4}, unusedDil {5}]", caseDex, bypassIndList[caseDex], usedNoxcList[caseDex], usedDiluentList[caseDex], unusedNoxcList[caseDex], unusedDiluentList[caseDex]));

                Assert.AreEqual(exMhvRowsList[caseDex], EmParameters.CurrentMhvMissing, string.Format("CurrentMhvMissing [case {0}, bypass {1}, usedNox {2}, usedDil {3}, unusedNox {4}, unusedDil {5}]", caseDex, bypassIndList[caseDex], usedNoxcList[caseDex], usedDiluentList[caseDex], unusedNoxcList[caseDex], unusedDiluentList[caseDex]));
            }
        }


        /// <summary>
        /// 
        /// Required Parameters:
        /// 
        ///     CurrentNoxrPrimaryOrPrimaryBypassMhvRecord.PrimaryBypassExistsIndicator : [Bypass]
        ///     CurrentNoxrPrimaryOrPrimaryBypassMhvRecord.UsedNoxcCount                : [UsedNoxc]
        ///     CurrentNoxrPrimaryOrPrimaryBypassMhvRecord.UsedDiluentCount             : [UsedDiluent]
        ///     CurrentNoxrPrimaryOrPrimaryBypassMhvRecord.UnusedNoxcCount              : [UnusedNoxc]
        ///     CurrentNoxrPrimaryOrPrimaryBypassMhvRecord.UnusedDiluentCount           : [UnusedDiluent]
        /// 
        /// Output Parameters:
        /// 
        ///     CurrentMhvMissing                                                       : [Fields]
        /// 
        /// 
        /// | ## | Bypass | UsedNoxc | UsedDiluent | UnusedNoxc | UnusedDiluent  || Result | Fields                                                      || Notes
        /// |  0 | 1      | 1        | 1           | 1          | 1              || null   | ""                                                          || All counts are 1.
        /// |  1 | 1      | 2        | 1           | 1          | 1              || A      | "Used NOXC"                                                 || UsedNoxc count is greater than 1.
        /// |  2 | 1      | 1        | 2           | 1          | 1              || A      | "Used CO2C/O2C"                                             || UsedDiluent count is greater than 1.
        /// |  3 | 1      | 1        | 1           | 2          | 1              || A      | "Unused NOXC"                                               || UnusedNoxc count is greater than 1.
        /// |  4 | 1      | 1        | 1           | 1          | 2              || A      | "Unused CO2C/O2C"                                           || UnusedDiluent count is greater than 1.
        /// |  5 | 1      | 2        | 2           | 2          | 2              || A      | "Used NOXC, Used CO2C/O2C, Unused NOXC and Unused CO2C/O2C" || All counts are greater than 1.
        /// |  6 | 0      | 2        | 2           | 2          | 2              || null   | ""                                                          || Bypass indicator is 0.
        /// |  7 | null   | 2        | 2           | 2          | 2              || null   | ""                                                          || Bypass indicator is null..
        /// </summary>
        [TestMethod()]
        public void HourMhv36()
        {
            /* Initialize objects generally needed for testing checks. */
            cHourlyMonitorValueChecks target = new cHourlyMonitorValueChecks(new cEmissionsCheckParameters());
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;


            /* Input Parameter Values */
            int?[] bypassIndList = { 1, 1, 1, 1, 1, 1, 0, null };
            int?[] usedNoxcList = { 1, 2, 1, 1, 1, 2, 2, 2 };
            int?[] usedDiluentList = { 1, 1, 2, 1, 1, 2, 2, 2 };
            int?[] unusedNoxcList = { 1, 1, 1, 2, 1, 2, 2, 2 };
            int?[] unusedDiluentList = { 1, 1, 1, 1, 2, 2, 2, 2 };

            /* Expected Values */
            string[] expResultList = { null, "A", "A", "A", "A", "A", null, null };
            string[] exMhvRowsList = { "", "Used NOXC", "Used CO2C/O2C", "Unused NOXC", "Unused CO2C/O2C", "Used NOXC, Used CO2C/O2C, Unused NOXC, and Unused CO2C/O2C", "", "" };

            /* Test Case Count */
            int caseCount = 8;

            /* Check array lengths */
            Assert.AreEqual(caseCount, bypassIndList.Length, "bypassIndList length");
            Assert.AreEqual(caseCount, usedNoxcList.Length, "usedNoxcList length");
            Assert.AreEqual(caseCount, usedDiluentList.Length, "usedDiluentList length");
            Assert.AreEqual(caseCount, unusedNoxcList.Length, "unusedNoxcList length");
            Assert.AreEqual(caseCount, unusedDiluentList.Length, "unusedDiluentList length");
            Assert.AreEqual(caseCount, expResultList.Length, "expResultList length");
            Assert.AreEqual(caseCount, exMhvRowsList.Length, "exMhvRowsList length");


            /* Run Cases */
            for (int caseDex = 0; caseDex < caseCount; caseDex++)
            {
                /* Initialize Required Parameters */
                EmParameters.CurrentNoxrPrimaryOrPrimaryBypassMhvRecord = new NoxrPrimaryAndPrimaryBypassMhv
                                                                              (
                                                                                  primaryBypassExistInd: bypassIndList[caseDex],
                                                                                  usedNoxcCount: usedNoxcList[caseDex], usedDiluentCount: usedDiluentList[caseDex],
                                                                                  unusedNoxcCount: unusedNoxcList[caseDex], unusedDiluentCount: unusedDiluentList[caseDex]
                                                                              );

                /* Initialize Output Parameters */
                EmParameters.CurrentMhvDuplicate = "BAD";


                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Initialize variables needed to run the check. */
                bool log = false;

                /* Run Check */
                string actual = target.HOURMHV36(category, ref log);

                /* Check Results */
                Assert.AreEqual(string.Empty, actual, string.Format("actual [case {0}, bypass {1}, usedNox {2}, usedDil {3}, unusedNox {4}, unusedDil {5}]", caseDex, bypassIndList[caseDex], usedNoxcList[caseDex], usedDiluentList[caseDex], unusedNoxcList[caseDex], unusedDiluentList[caseDex]));
                Assert.AreEqual(false, log, string.Format("log [case {0}, bypass {1}, usedNox {2}, usedDil {3}, unusedNox {4}, unusedDil {5}]", caseDex, bypassIndList[caseDex], usedNoxcList[caseDex], usedDiluentList[caseDex], unusedNoxcList[caseDex], unusedDiluentList[caseDex]));

                Assert.AreEqual(expResultList[caseDex], category.CheckCatalogResult, string.Format("CheckCatalogResult [case {0}, bypass {1}, usedNox {2}, usedDil {3}, unusedNox {4}, unusedDil {5}]", caseDex, bypassIndList[caseDex], usedNoxcList[caseDex], usedDiluentList[caseDex], unusedNoxcList[caseDex], unusedDiluentList[caseDex]));

                Assert.AreEqual(exMhvRowsList[caseDex], EmParameters.CurrentMhvDuplicate, string.Format("CurrentMhvDuplicate [case {0}, bypass {1}, usedNox {2}, usedDil {3}, unusedNox {4}, unusedDil {5}]", caseDex, bypassIndList[caseDex], usedNoxcList[caseDex], usedDiluentList[caseDex], unusedNoxcList[caseDex], unusedDiluentList[caseDex]));
            }
        }


        /// <summary>
        /// 
        /// Required Parameters:
        /// 
        ///     CurrentNoxrPrimaryOrPrimaryBypassMhvRecord.PrimaryBypassExistsIndicator : [Bypass]
        ///     CurrentNoxrPrimaryOrPrimaryBypassMhvRecord.UsedNoxcCount                : [UsedNoxc]
        ///     CurrentNoxrPrimaryOrPrimaryBypassMhvRecord.UsedDiluentCount             : [UsedDiluent]
        ///     CurrentNoxrPrimaryOrPrimaryBypassMhvRecord.UnusedNoxcCount              : [UnusedNoxc]
        ///     CurrentNoxrPrimaryOrPrimaryBypassMhvRecord.UnusedDiluentCount           : [UnusedDiluent]
        /// 
        /// Output Parameters:
        /// 
        ///     CurrentMhvMissing                                                       : [Fields]
        /// 
        ///               |  UnusedNoxc  | UnusedDiluent ||
        /// | ## | Bypass | Count | Modc | Count | Modc  || Result || Notes
        /// |  0 | null   | 1     | 47   | 1     | 48    || null   || Bypass indicator 1 null.
        /// |  1 | 0      | 1     | 47   | 1     | 48    || null   || Bypass indicator 1 0.
        /// |  2 | 1      | 1     | 47   | 1     | 48    || A      || MODC are different.
        /// |  3 | 1      | 1     | 48   | 1     | 47    || A      || MODC are different.
        /// |  4 | 1      | 0     | 47   | 1     | 48    || null   || NOXC count is zero.
        /// |  5 | 1      | null  | 47   | 1     | 48    || null   || NOXC count is null.
        /// |  6 | 1      | 1     | 47   | 0     | 48    || null   || Diluent count is zero.
        /// |  7 | 1      | 1     | 47   | null  | 48    || null   || Diluent count is null.
        /// |  8 | 1      | 1     | 47   | 1     | 47    || null   || MODC are the same.
        /// |  9 | 1      | 1     | 48   | 1     | 48    || null   || MODC are the same.
        /// </summary>
        [TestMethod()]
        public void HourMhv37()
        {
            /* Initialize objects generally needed for testing checks. */
            cHourlyMonitorValueChecks target = new cHourlyMonitorValueChecks(new cEmissionsCheckParameters());
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;


            /* Input Parameter Values */
            int?[] bypassIndList = { null, 0, 1, 1, 1, 1, 1, 1, 1, 1 };
            int?[] noxcCountList = { 1, 1, 1, 1, 0, null, 1, 1, 1, 1 };
            string[] noxcModcList = { "47", "47", "47", "48", "47", "47", "47", "47", "47", "48" };
            int?[] diluentCountList = { 1, 1, 1, 1, 1, 1, 0, null, 1, 1 };
            string[] diluentModcList = { "48", "48", "48", "47", "48", "48", "48", "48", "47", "48" };

            /* Expected Values */
            string[] expResultList = { null, null, "A", "A", null, null, null, null, null, null };

            /* Test Case Count */
            int caseCount = 10;

            /* Check array lengths */
            Assert.AreEqual(caseCount, bypassIndList.Length, "bypassIndList length");
            Assert.AreEqual(caseCount, noxcCountList.Length, "noxcCountList length");
            Assert.AreEqual(caseCount, noxcModcList.Length, "noxcModcList length");
            Assert.AreEqual(caseCount, diluentCountList.Length, "diluentCountList length");
            Assert.AreEqual(caseCount, diluentModcList.Length, "diluentModcList length");
            Assert.AreEqual(caseCount, expResultList.Length, "expResultList length");


            /* Run Cases */
            for (int caseDex = 0; caseDex < caseCount; caseDex++)
            {
                /* Initialize Required Parameters */
                EmParameters.CurrentNoxrPrimaryOrPrimaryBypassMhvRecord = new NoxrPrimaryAndPrimaryBypassMhv
                                                                              (
                                                                                  primaryBypassExistInd: bypassIndList[caseDex],
                                                                                  unusedNoxcCount: noxcCountList[caseDex], unusedNoxcModcCd: noxcModcList[caseDex],
                                                                                  unusedDiluentCount: diluentCountList[caseDex], unusedDiluentModcCd: diluentModcList[caseDex]
                                                                              );


                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Initialize variables needed to run the check. */
                bool log = false;

                /* Run Check */
                string actual = target.HOURMHV37(category, ref log);

                /* Check Results */
                Assert.AreEqual(string.Empty, actual, string.Format("actual [case {0}, bypass {1}, noxCount {2}, noxModc {3}, dilCount {4}, dilModc {5}]", caseDex, bypassIndList[caseDex], noxcCountList[caseDex], noxcModcList[caseDex], diluentCountList[caseDex], diluentModcList[caseDex]));
                Assert.AreEqual(false, log, string.Format("log [case {0}, bypass {1}, noxCount {2}, noxModc {3}, dilCount {4}, dilModc {5}]", caseDex, bypassIndList[caseDex], noxcCountList[caseDex], noxcModcList[caseDex], diluentCountList[caseDex], diluentModcList[caseDex]));

                Assert.AreEqual(expResultList[caseDex], category.CheckCatalogResult, string.Format("CheckCatalogResult [case {0}, bypass {1}, noxCount {2}, noxModc {3}, dilCount {4}, dilModc {5}]", caseDex, bypassIndList[caseDex], noxcCountList[caseDex], noxcModcList[caseDex], diluentCountList[caseDex], diluentModcList[caseDex]));
            }
        }

    }

}