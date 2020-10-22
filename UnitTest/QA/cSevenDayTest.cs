using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ECMPS.Checks.CalibrationChecks;
using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Data.Ecmps.Dbo.View;
using ECMPS.Checks.Data.EcmpsAux.CrossCheck.Virtual;
using ECMPS.Checks.Qa.Parameters;


namespace UnitTest.QA
{
    [TestClass]
    public class cSevenDayTest
    {

        /// <summary>
        /// 
        /// 
        /// Calibration Test Begin : 2017-06-17 22:12
        /// Calibration Test End   : 2017-06-17 22:59
        /// Zero Injection Time    : 2017-06-17 22:12
        /// Upscale Injection Time : 2017-06-17 22:59
        /// 
        /// Calculate Zero Calibration Injection : true
        /// Calibration Injection Times Valid    : true
        /// Span Value                           : 1.1
        /// 
        /// Test Tolerances:
        /// 
        /// | TestType | FieldDescription | Tolerance |
        /// | 7DAY     | Other            | 1.0       |
        /// | 7DAY     | DifferencePCT    | 0.1       |
        /// | OTHER    | DifferencePCT    | 1.0       |
        /// 
        /// 
        /// | ## | CompT | CalcR   | InjCount | TMeas | TRef  | TError || CalcAps | CalcResult | InjCount | CalcValue || Note
        /// |  0 | CO2   | PASSED  | 12       | 5.255 | 4.706 | 0.5    || 0       | PASSED     | 13       | 0.5       || Calculated Error Passed
        /// |  1 | CO2   | PASSED  | 12       | 5.255 | 4.606 | 0.5    || 0       | PASSED     | 13       | 0.6       || Calculated Error Failed but withing tolerance of Reported Error, which passed.
        /// |  2 | CO2   | PASSED  | 12       | 5.255 | 4.605 | 0.5    || 0       | FAILED     | 13       | 0.7       || Calculated Error Failed and outside of tolerance.
        /// |  3 | CO2   | PASSED  | 12       | 5.255 | 4.605 | 0.6    || 0       | FAILED     | 13       | 0.7       || Calculated Error Failed and Reported Error failed event though Calculated and Reported are within tolerance.
        /// |  4 | CO2   | PASSED  | 12       | 4.605 | 5.255 | 0.6    || 0       | FAILED     | 13       | 0.7       || Calculated Error Failed and Reported Error failed event though Calculated and Reported are within tolerance.
        /// |  5 | CO2   | FAILED  | 12       | 5.255 | 4.706 | 0.5    || 0       | FAILED     | 13       | 0.5       || Calculated Test Calc Result already set to FAILED.
        /// |  6 | CO2   | INVALID | 12       | 5.255 | 4.706 | 0.5    || 0       | INVALID    | 13       | 0.5       || Calculated Test Calc Result already set to INVALID.
        /// |  7 | O2    | PASSED  | 12       | 5.255 | 4.706 | 0.5    || 0       | PASSED     | 13       | 0.5       || Calculated Error Passed
        /// |  8 | O2    | PASSED  | 12       | 5.255 | 4.606 | 0.5    || 0       | PASSED     | 13       | 0.6       || Calculated Error Failed but withing tolerance of Reported Error, which passed.
        /// |  9 | O2    | PASSED  | 12       | 5.255 | 4.605 | 0.5    || 0       | FAILED     | 13       | 0.7       || Calculated Error Failed and outside of tolerance.
        /// | 10 | O2    | PASSED  | 12       | 5.255 | 4.605 | 0.6    || 0       | FAILED     | 13       | 0.7       || Calculated Error Failed and Reported Error failed event though Calculated and Reported are within tolerance.
        /// | 11 | O2    | PASSED  | 12       | 4.605 | 5.255 | 0.6    || 0       | FAILED     | 13       | 0.7       || Calculated Error Failed and Reported Error failed event though Calculated and Reported are within tolerance.
        /// | 12 | O2    | FAILED  | 12       | 5.255 | 4.706 | 0.5    || 0       | FAILED     | 13       | 0.5       || Calculated Test Calc Result already set to FAILED.
        /// | 13 | O2    | INVALID | 12       | 5.255 | 4.706 | 0.5    || 0       | INVALID    | 13       | 0.5       || Calculated Test Calc Result already set to INVALID.
        /// 
        /// </summary>
        [TestMethod]
        public void SevenDay15_Co2AndO2()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            QaParameters.Init(category.Process);
            QaParameters.Category = category;


            /* Input Parameter Values */
            DateTime calibrationTestBegin = new DateTime(2017, 06, 17, 22, 12, 0);
            DateTime calibrationTestEnd = new DateTime(2017, 06, 17, 22, 59, 0);
            DateTime zeroInjectionTime = new DateTime(2017, 06, 17, 22, 12, 0);
            DateTime upscaleInjectionTime = new DateTime(2017, 06, 17, 22, 59, 0);

            string[] componentTypeList = { "CO2", "CO2", "CO2", "CO2", "CO2", "CO2", "CO2",
                                           "O2", "O2", "O2", "O2", "O2", "O2", "O2" };
            string[] calculatedResultList = { "PASSED", "PASSED", "PASSED", "PASSED", "FAILED", "FAILED", "INVALID",
                                              "PASSED", "PASSED", "PASSED", "PASSED", "FAILED", "FAILED", "INVALID" };
            int?[] injectionCountList = { 12, 12, 12, 12, 12, 12, 12,
                                          12, 12, 12, 12, 12, 12, 12};
            decimal?[] testMeasuredValueList = { 5.255m, 5.255m, 5.255m, 5.255m, 4.605m, 5.255m, 5.255m,
                                                 5.255m, 5.255m, 5.255m, 5.255m, 4.605m, 5.255m, 5.255m };
            decimal?[] testReferenceValueList = { 4.706m, 4.606m, 4.605m, 4.605m, 5.255m, 4.706m, 4.706m,
                                                  4.706m, 4.606m, 4.605m, 4.605m, 5.255m, 4.706m, 4.706m };
            decimal?[] testErrorValueList = { 0.5m, 0.5m, 0.5m, 0.6m, 0.6m, 0.5m, 0.5m,
                                              0.5m, 0.5m, 0.5m, 0.6m, 0.6m, 0.5m, 0.5m };

            /* Expected Values */
            int?[] expApsIndList = { 0, 0, 0, 0, 0, 0, 0,
                                     0, 0, 0, 0, 0, 0, 0 };
            string[] expCalculatedResultList = { "PASSED", "PASSED", "FAILED", "FAILED", "FAILED", "FAILED", "INVALID",
                                                 "PASSED", "PASSED", "FAILED", "FAILED", "FAILED", "FAILED", "INVALID" };
            int?[] expInjectionCountList = { 13, 13, 13, 13, 13, 13, 13,
                                             13, 13, 13, 13, 13, 13, 13 };
            decimal?[] expCalcValueList = { 0.5m, 0.6m, 0.7m, 0.7m, 0.7m, 0.5m, 0.5m,
                                            0.5m, 0.6m, 0.7m, 0.7m, 0.7m, 0.5m, 0.5m };


            /* Test Case Count */
            int caseCount = 14;

            /* Check array lengths */
            Assert.AreEqual(caseCount, componentTypeList.Length, "componentTypeList length");
            Assert.AreEqual(caseCount, calculatedResultList.Length, "calculatedResultList length");
            Assert.AreEqual(caseCount, injectionCountList.Length, "injectionCountList length");
            Assert.AreEqual(caseCount, testMeasuredValueList.Length, "testMeasuredValueList length");
            Assert.AreEqual(caseCount, testReferenceValueList.Length, "testReferenceValueList length");
            Assert.AreEqual(caseCount, testErrorValueList.Length, "testErrorValueList length");
            Assert.AreEqual(caseCount, expApsIndList.Length, "expResultList length");
            Assert.AreEqual(caseCount, expCalculatedResultList.Length, "expCalculatedResultList length");
            Assert.AreEqual(caseCount, expInjectionCountList.Length, "expInjectionCountList length");


            /* Run Cases */
            for (int caseDex = 0; caseDex < caseCount; caseDex++)
            {
                /*  Initialize Input Parameters*/
                QaParameters.CalculateZeroCalibrationInjection = true;
                QaParameters.CalibrationInjectionTimesValid = true;
                QaParameters.CalibrationTestBeginDate = calibrationTestBegin.Date;
                QaParameters.CalibrationTestBeginHour = calibrationTestBegin.Hour;
                QaParameters.CalibrationTestBeginMinute = calibrationTestBegin.Minute;
                QaParameters.CalibrationTestEndDate = calibrationTestEnd.Date;
                QaParameters.CalibrationTestEndHour = calibrationTestEnd.Hour;
                QaParameters.CalibrationTestEndMinute = calibrationTestEnd.Minute;
                QaParameters.CurrentCalibrationInjection 
                    = new VwQaCalibrationInjectionRow(componentTypeCd: componentTypeList[caseDex],
                                                      zeroMeasuredValue: testMeasuredValueList[caseDex], 
                                                      zeroRefValue: testReferenceValueList[caseDex], 
                                                      zeroCalError: testErrorValueList[caseDex]);
                QaParameters.TestSpanValue = 1.1m;
                QaParameters.TestTolerancesCrossCheckTable = new CheckDataView<TestTolerancesRow>
                    (
                        new TestTolerancesRow("7DAY", "Other", "1.0"),
                        new TestTolerancesRow("7DAY", "DifferencePCT", "0.1"),
                        new TestTolerancesRow("OTHER", "DifferencePCT", "1.0")
                    );


                /*  Initialize Input/Output Parameters*/
                QaParameters.CalibrationInjectionCount = injectionCountList[caseDex];
                QaParameters.CalibrationTestResult = calculatedResultList[caseDex];


                /*  Initialize Output Parameters*/
                QaParameters.CalibrationZeroInjectionCalcApsIndicator = int.MinValue;
                QaParameters.CalibrationZeroInjectionCalcResult = decimal.MinValue;


                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Initialize variables needed to run the check. */
                bool log = false;
                string actual;

                /* Run Check */
                actual = cCalibrationChecks.SEVNDAY15(category, ref log);

                /* Check Results */
                Assert.AreEqual(string.Empty, actual, string.Format("actual {0}", caseDex));
                Assert.AreEqual(false, log, string.Format("log {0}", caseDex));

                Assert.AreEqual(null, category.CheckCatalogResult, string.Format("category.CheckCatalogResult {0}", caseDex));

                Assert.AreEqual(expInjectionCountList[caseDex], QaParameters.CalibrationInjectionCount, string.Format("CalibrationInjectionCount {0}", caseDex));
                Assert.AreEqual(expCalculatedResultList[caseDex], QaParameters.CalibrationTestResult, string.Format("CalibrationTestResult {0}", caseDex));
                Assert.AreEqual(expApsIndList[caseDex], QaParameters.CalibrationZeroInjectionCalcApsIndicator, string.Format("CalibrationZeroInjectionCalcApsIndicator {0}", caseDex));
                Assert.AreEqual(expCalcValueList[caseDex], QaParameters.CalibrationZeroInjectionCalcResult, string.Format("CalibrationZeroInjectionCalcResult {0}", caseDex));
            }
        }


        /// <summary>
        /// 
        /// 
        /// Calculate Upscale Calibration Injection : true
        /// Span Value                           : 1.1
        /// 
        /// Test Tolerances:
        /// 
        /// | TestType | FieldDescription | Tolerance |
        /// | 7DAY     | Other            | 1.0       |
        /// | 7DAY     | DifferencePCT    | 0.1       |
        /// | OTHER    | DifferencePCT    | 1.0       |
        /// 
        /// 
        /// | ## | CompT | CalcR   | TMeas | TRef  | TError || CalcAps | CalcResult | CalcValue || Note
        /// |  0 | CO2   | PASSED  | 5.255 | 4.706 | 0.5    || 0       | PASSED     | 0.5       || Calculated Error Passed
        /// |  1 | CO2   | PASSED  | 5.255 | 4.606 | 0.5    || 0       | PASSED     | 0.6       || Calculated Error Failed but withing tolerance of Reported Error, which passed.
        /// |  2 | CO2   | PASSED  | 5.255 | 4.605 | 0.5    || 0       | FAILED     | 0.7       || Calculated Error Failed and outside of tolerance.
        /// |  3 | CO2   | PASSED  | 5.255 | 4.605 | 0.6    || 0       | FAILED     | 0.7       || Calculated Error Failed and Reported Error failed event though Calculated and Reported are within tolerance.
        /// |  4 | CO2   | PASSED  | 4.605 | 5.255 | 0.6    || 0       | FAILED     | 0.7       || Calculated Error Failed and Reported Error failed event though Calculated and Reported are within tolerance.
        /// |  5 | CO2   | FAILED  | 5.255 | 4.706 | 0.5    || 0       | FAILED     | 0.5       || Calculated Test Calc Result already set to FAILED.
        /// |  6 | CO2   | INVALID | 5.255 | 4.706 | 0.5    || 0       | INVALID    | 0.5       || Calculated Test Calc Result already set to INVALID.
        /// |  7 | O2    | PASSED  | 5.255 | 4.706 | 0.5    || 0       | PASSED     | 0.5       || Calculated Error Passed
        /// |  8 | O2    | PASSED  | 5.255 | 4.606 | 0.5    || 0       | PASSED     | 0.6       || Calculated Error Failed but withing tolerance of Reported Error, which passed.
        /// |  9 | O2    | PASSED  | 5.255 | 4.605 | 0.5    || 0       | FAILED     | 0.7       || Calculated Error Failed and outside of tolerance.
        /// | 10 | O2    | PASSED  | 5.255 | 4.605 | 0.6    || 0       | FAILED     | 0.7       || Calculated Error Failed and Reported Error failed event though Calculated and Reported are within tolerance.
        /// | 11 | O2    | PASSED  | 4.605 | 5.255 | 0.6    || 0       | FAILED     | 0.7       || Calculated Error Failed and Reported Error failed event though Calculated and Reported are within tolerance.
        /// | 12 | O2    | FAILED  | 5.255 | 4.706 | 0.5    || 0       | FAILED     | 0.5       || Calculated Test Calc Result already set to FAILED.
        /// | 13 | O2    | INVALID | 5.255 | 4.706 | 0.5    || 0       | INVALID    | 0.5       || Calculated Test Calc Result already set to INVALID.
        /// 
        /// </summary>
        [TestMethod]
        public void SevenDay16_Co2AndO2()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            QaParameters.Init(category.Process);
            QaParameters.Category = category;


            /* Input Parameter Values */
            string[] componentTypeList = { "CO2", "CO2", "CO2", "CO2", "CO2", "CO2", "CO2",
                                           "O2", "O2", "O2", "O2", "O2", "O2", "O2" };
            string[] calculatedResultList = { "PASSED", "PASSED", "PASSED", "PASSED", "FAILED", "FAILED", "INVALID",
                                              "PASSED", "PASSED", "PASSED", "PASSED", "FAILED", "FAILED", "INVALID" };
            decimal?[] testMeasuredValueList = { 5.255m, 5.255m, 5.255m, 5.255m, 4.605m, 5.255m, 5.255m,
                                                 5.255m, 5.255m, 5.255m, 5.255m, 4.605m, 5.255m, 5.255m };
            decimal?[] testReferenceValueList = { 4.706m, 4.606m, 4.605m, 4.605m, 5.255m, 4.706m, 4.706m,
                                                  4.706m, 4.606m, 4.605m, 4.605m, 5.255m, 4.706m, 4.706m };
            decimal?[] testErrorValueList = { 0.5m, 0.5m, 0.5m, 0.6m, 0.6m, 0.5m, 0.5m,
                                              0.5m, 0.5m, 0.5m, 0.6m, 0.6m, 0.5m, 0.5m };

            /* Expected Values */
            int?[] expApsIndList = { 0, 0, 0, 0, 0, 0, 0,
                                     0, 0, 0, 0, 0, 0, 0 };
            string[] expCalculatedResultList = { "PASSED", "PASSED", "FAILED", "FAILED", "FAILED", "FAILED", "INVALID",
                                                 "PASSED", "PASSED", "FAILED", "FAILED", "FAILED", "FAILED", "INVALID" };
            decimal?[] expCalcValueList = { 0.5m, 0.6m, 0.7m, 0.7m, 0.7m, 0.5m, 0.5m,
                                            0.5m, 0.6m, 0.7m, 0.7m, 0.7m, 0.5m, 0.5m };


            /* Test Case Count */
            int caseCount = 14;

            /* Check array lengths */
            Assert.AreEqual(caseCount, componentTypeList.Length, "componentTypeList length");
            Assert.AreEqual(caseCount, calculatedResultList.Length, "calculatedResultList length");
            Assert.AreEqual(caseCount, testMeasuredValueList.Length, "testMeasuredValueList length");
            Assert.AreEqual(caseCount, testReferenceValueList.Length, "testReferenceValueList length");
            Assert.AreEqual(caseCount, testErrorValueList.Length, "testErrorValueList length");
            Assert.AreEqual(caseCount, expApsIndList.Length, "expResultList length");
            Assert.AreEqual(caseCount, expCalculatedResultList.Length, "expCalculatedResultList length");


            /* Run Cases */
            for (int caseDex = 0; caseDex < caseCount; caseDex++)
            {
                /*  Initialize Input Parameters*/
                QaParameters.CalculateUpscaleCalibrationInjection = true;
                QaParameters.CalibrationInjectionTimesValid = true;
                QaParameters.CurrentCalibrationInjection
                    = new VwQaCalibrationInjectionRow(componentTypeCd: componentTypeList[caseDex],
                                                      upscaleMeasuredValue: testMeasuredValueList[caseDex],
                                                      upscaleRefValue: testReferenceValueList[caseDex],
                                                      upscaleCalError: testErrorValueList[caseDex]);
                QaParameters.TestSpanValue = 1.1m;
                QaParameters.TestTolerancesCrossCheckTable = new CheckDataView<TestTolerancesRow>
                    (
                        new TestTolerancesRow("7DAY", "Other", "1.0"),
                        new TestTolerancesRow("7DAY", "DifferencePCT", "0.1"),
                        new TestTolerancesRow("OTHER", "DifferencePCT", "1.0")
                    );


                /*  Initialize Input/Output Parameters*/
                QaParameters.CalibrationTestResult = calculatedResultList[caseDex];


                /*  Initialize Output Parameters*/
                QaParameters.CalibrationUpscaleInjectionCalcApsIndicator = int.MinValue;
                QaParameters.CalibrationUpscaleInjectionCalcResult = decimal.MinValue;


                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Initialize variables needed to run the check. */
                bool log = false;
                string actual;

                /* Run Check */
                actual = cCalibrationChecks.SEVNDAY16(category, ref log);

                /* Check Results */
                Assert.AreEqual(string.Empty, actual, string.Format("actual {0}", caseDex));
                Assert.AreEqual(false, log, string.Format("log {0}", caseDex));

                Assert.AreEqual(null, category.CheckCatalogResult, string.Format("category.CheckCatalogResult {0}", caseDex));
                
                Assert.AreEqual(expCalculatedResultList[caseDex], QaParameters.CalibrationTestResult, string.Format("CalibrationTestResult {0}", caseDex));
                Assert.AreEqual(expApsIndList[caseDex], QaParameters.CalibrationUpscaleInjectionCalcApsIndicator, string.Format("CalibrationUpscaleInjectionCalcApsIndicator {0}", caseDex));
                Assert.AreEqual(expCalcValueList[caseDex], QaParameters.CalibrationUpscaleInjectionCalcResult, string.Format("CalibrationUpscaleInjectionCalcResult {0}", caseDex));
            }
        }

    }
}
