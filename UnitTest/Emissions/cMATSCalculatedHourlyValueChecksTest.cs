using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.EmissionsChecks;
using ECMPS.Checks.EmissionsReport;
using ECMPS.Checks.Data.Ecmps.CheckEm.Function;
using ECMPS.Checks.Data.Ecmps.CrossCheck.Table;
using ECMPS.Checks.Data.Ecmps.Dbo.Table;
using ECMPS.Checks.Data.Ecmps.Dbo.View;
using ECMPS.Checks.Data.EcmpsAux.CrossCheck.Virtual;
using ECMPS.Checks.Em.Parameters;
using ECMPS.Definitions.Extensions;


using UnitTest.UtilityClasses;

namespace UnitTest.Emissions
{
    [TestClass()]
    public class cMATSCalculatedHourlyValueChecksTest
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

        #region Checks 1-10

        /// <summary>
        ///A test for MATSCHV-1
        ///</summary>()
        [TestMethod()]
        public void MATSCHV1()
        {
            //static check setup
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            // Variables
            bool log = false;
            string actual;

            /* MATS Load is not null */
            {
                // Init Input
                EmParameters.CurrentHourlyOpRecord = new VwMpHrlyOpDataRow(matsHourLoad: 225m);
                EmParameters.MatsHgDhvParameter = "HGDHV";
                EmParameters.MatsHgDhvValid = true;
                EmParameters.MatsHgDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(parameterCd: "HG");
                EmParameters.MatsMhvCalculatedHgcValue = "1.01E-2";
                EmParameters.MatsHgcMhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData(parameterCd: "HGC");

                // Init Output
                category.CheckCatalogResult = null;


                // Run Checks
                actual = cMATSCalculatedHourlyValueChecks.MATSCHV1(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual(ECMPS.Definitions.Extensions.cExtensions.ScientificNotationtoDecimal("6.24E-11"), EmParameters.CalculationConversionFactor, "CalculationConversionFactor");
                Assert.AreEqual("HGDHV", EmParameters.CurrentDhvParameter, "CurrentDhvParameter,");
                Assert.AreEqual(true, EmParameters.CurrentDhvRecordValid, "CurrentDhvRecordValid");
                Assert.AreEqual((1000m / EmParameters.CurrentHourlyOpRecord.MatsHourLoad.Value), EmParameters.FinalConversionFactor, "FinalConversionFactor,");
                Assert.AreEqual("HG", EmParameters.MatsDhvRecord.ParameterCd, "MatsDhvRecord");
                Assert.AreEqual("1.01E-2", EmParameters.MatsMhvCalculatedValue, "MatsMhvCalculatedValue");
                Assert.AreEqual("HGC", EmParameters.MatsMhvRecord.ParameterCd, "MatsMhvRecord");
                Assert.AreEqual("A-3", EmParameters.MatsMoistureEquationList, "MatsMoistureEquationList");
                Assert.AreEqual("36,39", EmParameters.MatsDhvMeasuredModcList, "MatsDhvMeasuredModcList");
                Assert.AreEqual("38", EmParameters.MatsDhvUnavailableModcList, "MatsDhvUnavailableModcList");
            }

            /* MATS Load is null */
            {
                // Init Input
                EmParameters.CurrentHourlyOpRecord = new VwMpHrlyOpDataRow(matsHourLoad: null);
                EmParameters.MatsHgDhvParameter = "HGDHV";
                EmParameters.MatsHgDhvValid = true;
                EmParameters.MatsHgDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(parameterCd: "HG");
                EmParameters.MatsMhvCalculatedHgcValue = "1.01E-2";
                EmParameters.MatsHgcMhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData(parameterCd: "HGC");

                // Init Output
                category.CheckCatalogResult = null;


                // Run Checks
                actual = cMATSCalculatedHourlyValueChecks.MATSCHV1(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual(ECMPS.Definitions.Extensions.cExtensions.ScientificNotationtoDecimal("6.24E-11"), EmParameters.CalculationConversionFactor, "CalculationConversionFactor");
                Assert.AreEqual("HGDHV", EmParameters.CurrentDhvParameter, "CurrentDhvParameter,");
                Assert.AreEqual(true, EmParameters.CurrentDhvRecordValid, "CurrentDhvRecordValid");
                Assert.AreEqual(null, EmParameters.FinalConversionFactor, "FinalConversionFactor,");
                Assert.AreEqual("HG", EmParameters.MatsDhvRecord.ParameterCd, "MatsDhvRecord");
                Assert.AreEqual("1.01E-2", EmParameters.MatsMhvCalculatedValue, "MatsMhvCalculatedValue");
                Assert.AreEqual("HGC", EmParameters.MatsMhvRecord.ParameterCd, "MatsMhvRecord");
                Assert.AreEqual("A-3", EmParameters.MatsMoistureEquationList, "MatsMoistureEquationList");
                Assert.AreEqual("36,39", EmParameters.MatsDhvMeasuredModcList, "MatsDhvMeasuredModcList");
                Assert.AreEqual("38", EmParameters.MatsDhvUnavailableModcList, "MatsDhvUnavailableModcList");
            }

            /* MATS Load is 0 */
            {
                // Init Input
                EmParameters.CurrentHourlyOpRecord = new VwMpHrlyOpDataRow(matsHourLoad: 0m);
                EmParameters.MatsHgDhvParameter = "HGDHV";
                EmParameters.MatsHgDhvValid = true;
                EmParameters.MatsHgDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(parameterCd: "HG");
                EmParameters.MatsMhvCalculatedHgcValue = "1.01E-2";
                EmParameters.MatsHgcMhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData(parameterCd: "HGC");

                // Init Output
                category.CheckCatalogResult = null;


                // Run Checks
                actual = cMATSCalculatedHourlyValueChecks.MATSCHV1(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual(ECMPS.Definitions.Extensions.cExtensions.ScientificNotationtoDecimal("6.24E-11"), EmParameters.CalculationConversionFactor, "CalculationConversionFactor");
                Assert.AreEqual("HGDHV", EmParameters.CurrentDhvParameter, "CurrentDhvParameter,");
                Assert.AreEqual(true, EmParameters.CurrentDhvRecordValid, "CurrentDhvRecordValid");
                Assert.AreEqual(null, EmParameters.FinalConversionFactor, "FinalConversionFactor,");
                Assert.AreEqual("HG", EmParameters.MatsDhvRecord.ParameterCd, "MatsDhvRecord");
                Assert.AreEqual("1.01E-2", EmParameters.MatsMhvCalculatedValue, "MatsMhvCalculatedValue");
                Assert.AreEqual("HGC", EmParameters.MatsMhvRecord.ParameterCd, "MatsMhvRecord");
                Assert.AreEqual("A-3", EmParameters.MatsMoistureEquationList, "MatsMoistureEquationList");
                Assert.AreEqual("36,39", EmParameters.MatsDhvMeasuredModcList, "MatsDhvMeasuredModcList");
                Assert.AreEqual("38", EmParameters.MatsDhvUnavailableModcList, "MatsDhvUnavailableModcList");
            }

        }


        /// <summary>
        ///A test for MATSCHV-2
        ///</summary>()
        [TestMethod()]
        public void MATSCHV2()
        {
            //static check setup
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            // Variables
            bool log = false;
            string actual;

            /* MATS Load is not null */
            {
                // Init Input
                EmParameters.CurrentHourlyOpRecord = new VwMpHrlyOpDataRow(matsHourLoad: 225m);
                EmParameters.MatsHclDhvParameter = "HCLDHV";
                EmParameters.MatsHclDhvValid = true;
                EmParameters.MatsHclDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(parameterCd: "HCL");
                EmParameters.MatsMhvCalculatedHclcValue = "1.01E-2";
                EmParameters.MatsHclcMhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData(parameterCd: "HCLC");

                // Init Output
                category.CheckCatalogResult = null;


                // Run Checks
                actual = cMATSCalculatedHourlyValueChecks.MATSCHV2(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual(ECMPS.Definitions.Extensions.cExtensions.ScientificNotationtoDecimal("9.43E-8"), EmParameters.CalculationConversionFactor, "CalculationConversionFactor");
                Assert.AreEqual("HCLDHV", EmParameters.CurrentDhvParameter, "CurrentDhvParameter,");
                Assert.AreEqual(true, EmParameters.CurrentDhvRecordValid, "CurrentDhvRecordValid");
                Assert.AreEqual((1m / EmParameters.CurrentHourlyOpRecord.MatsHourLoad.Value), EmParameters.FinalConversionFactor, "FinalConversionFactor,");
                Assert.AreEqual("HCL", EmParameters.MatsDhvRecord.ParameterCd, "MatsDhvRecord");
                Assert.AreEqual("1.01E-2", EmParameters.MatsMhvCalculatedValue, "MatsMhvCalculatedValue");
                Assert.AreEqual("HCLC", EmParameters.MatsMhvRecord.ParameterCd, "MatsMhvRecord");
                Assert.AreEqual("HC-3", EmParameters.MatsMoistureEquationList, "MatsMoistureEquationList");
                Assert.AreEqual("36,39", EmParameters.MatsDhvMeasuredModcList, "MatsDhvMeasuredModcList");
                Assert.AreEqual("38", EmParameters.MatsDhvUnavailableModcList, "MatsDhvUnavailableModcList");
            }

            /* MATS Load is null */
            {
                // Init Input
                EmParameters.CurrentHourlyOpRecord = new VwMpHrlyOpDataRow(matsHourLoad: null);
                EmParameters.MatsHclDhvParameter = "HCLDHV";
                EmParameters.MatsHclDhvValid = true;
                EmParameters.MatsHclDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(parameterCd: "HCL");
                EmParameters.MatsMhvCalculatedHclcValue = "1.01E-2";
                EmParameters.MatsHclcMhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData(parameterCd: "HCLC");

                // Init Output
                category.CheckCatalogResult = null;


                // Run Checks
                actual = cMATSCalculatedHourlyValueChecks.MATSCHV2(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual(ECMPS.Definitions.Extensions.cExtensions.ScientificNotationtoDecimal("9.43E-8"), EmParameters.CalculationConversionFactor, "CalculationConversionFactor");
                Assert.AreEqual("HCLDHV", EmParameters.CurrentDhvParameter, "CurrentDhvParameter,");
                Assert.AreEqual(true, EmParameters.CurrentDhvRecordValid, "CurrentDhvRecordValid");
                Assert.AreEqual(null, EmParameters.FinalConversionFactor, "FinalConversionFactor,");
                Assert.AreEqual("HCL", EmParameters.MatsDhvRecord.ParameterCd, "MatsDhvRecord");
                Assert.AreEqual("1.01E-2", EmParameters.MatsMhvCalculatedValue, "MatsMhvCalculatedValue");
                Assert.AreEqual("HCLC", EmParameters.MatsMhvRecord.ParameterCd, "MatsMhvRecord");
                Assert.AreEqual("HC-3", EmParameters.MatsMoistureEquationList, "MatsMoistureEquationList");
                Assert.AreEqual("36,39", EmParameters.MatsDhvMeasuredModcList, "MatsDhvMeasuredModcList");
                Assert.AreEqual("38", EmParameters.MatsDhvUnavailableModcList, "MatsDhvUnavailableModcList");
            }

            /* MATS Load is 0 */
            {
                // Init Input
                EmParameters.CurrentHourlyOpRecord = new VwMpHrlyOpDataRow(matsHourLoad: 0m);
                EmParameters.MatsHclDhvParameter = "HCLDHV";
                EmParameters.MatsHclDhvValid = true;
                EmParameters.MatsHclDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(parameterCd: "HCL");
                EmParameters.MatsMhvCalculatedHclcValue = "1.01E-2";
                EmParameters.MatsHclcMhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData(parameterCd: "HCLC");

                // Init Output
                category.CheckCatalogResult = null;


                // Run Checks
                actual = cMATSCalculatedHourlyValueChecks.MATSCHV2(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual(ECMPS.Definitions.Extensions.cExtensions.ScientificNotationtoDecimal("9.43E-8"), EmParameters.CalculationConversionFactor, "CalculationConversionFactor");
                Assert.AreEqual("HCLDHV", EmParameters.CurrentDhvParameter, "CurrentDhvParameter,");
                Assert.AreEqual(true, EmParameters.CurrentDhvRecordValid, "CurrentDhvRecordValid");
                Assert.AreEqual(null, EmParameters.FinalConversionFactor, "FinalConversionFactor,");
                Assert.AreEqual("HCL", EmParameters.MatsDhvRecord.ParameterCd, "MatsDhvRecord");
                Assert.AreEqual("1.01E-2", EmParameters.MatsMhvCalculatedValue, "MatsMhvCalculatedValue");
                Assert.AreEqual("HCLC", EmParameters.MatsMhvRecord.ParameterCd, "MatsMhvRecord");
                Assert.AreEqual("HC-3", EmParameters.MatsMoistureEquationList, "MatsMoistureEquationList");
                Assert.AreEqual("36,39", EmParameters.MatsDhvMeasuredModcList, "MatsDhvMeasuredModcList");
                Assert.AreEqual("38", EmParameters.MatsDhvUnavailableModcList, "MatsDhvUnavailableModcList");
            }
        }


        /// <summary>
        ///A test for MATSCHV-3
        ///</summary>()
        [TestMethod()]
        public void MATSCHV3()
        {
            //static check setup
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            // Variables
            bool log = false;
            string actual;

            /* MATS Load is not null */
            {
                // Init Input
                EmParameters.CurrentHourlyOpRecord = new VwMpHrlyOpDataRow(matsHourLoad: 225m);
                EmParameters.MatsHfDhvParameter = "HFDHV";
                EmParameters.MatsHfDhvValid = true;
                EmParameters.MatsHfDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(parameterCd: "HF");
                EmParameters.MatsMhvCalculatedHfcValue = "1.01E-2";
                EmParameters.MatsHfcMhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData(parameterCd: "HFC");

                // Init Output
                category.CheckCatalogResult = null;


                // Run Checks
                actual = cMATSCalculatedHourlyValueChecks.MATSCHV3(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual(ECMPS.Definitions.Extensions.cExtensions.ScientificNotationtoDecimal("5.18E-8"), EmParameters.CalculationConversionFactor, "CalculationConversionFactor");
                Assert.AreEqual("HFDHV", EmParameters.CurrentDhvParameter, "CurrentDhvParameter,");
                Assert.AreEqual(true, EmParameters.CurrentDhvRecordValid, "CurrentDhvRecordValid");
                Assert.AreEqual((1m / EmParameters.CurrentHourlyOpRecord.MatsHourLoad.Value), EmParameters.FinalConversionFactor, "FinalConversionFactor,");
                Assert.AreEqual("HF", EmParameters.MatsDhvRecord.ParameterCd, "MatsDhvRecord");
                Assert.AreEqual("1.01E-2", EmParameters.MatsMhvCalculatedValue, "MatsMhvCalculatedValue");
                Assert.AreEqual("HFC", EmParameters.MatsMhvRecord.ParameterCd, "MatsMhvRecord");
                Assert.AreEqual("HF-3", EmParameters.MatsMoistureEquationList, "MatsMoistureEquationList");
                Assert.AreEqual("36,39", EmParameters.MatsDhvMeasuredModcList, "MatsDhvMeasuredModcList");
                Assert.AreEqual("38", EmParameters.MatsDhvUnavailableModcList, "MatsDhvUnavailableModcList");
            }

            /* MATS Load is null */
            {
                // Init Input
                EmParameters.CurrentHourlyOpRecord = new VwMpHrlyOpDataRow(matsHourLoad: null);
                EmParameters.MatsHfDhvParameter = "HFDHV";
                EmParameters.MatsHfDhvValid = true;
                EmParameters.MatsHfDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(parameterCd: "HF");
                EmParameters.MatsMhvCalculatedHfcValue = "1.01E-2";
                EmParameters.MatsHfcMhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData(parameterCd: "HFC");

                // Init Output
                category.CheckCatalogResult = null;


                // Run Checks
                actual = cMATSCalculatedHourlyValueChecks.MATSCHV3(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual(ECMPS.Definitions.Extensions.cExtensions.ScientificNotationtoDecimal("5.18E-8"), EmParameters.CalculationConversionFactor, "CalculationConversionFactor");
                Assert.AreEqual("HFDHV", EmParameters.CurrentDhvParameter, "CurrentDhvParameter,");
                Assert.AreEqual(true, EmParameters.CurrentDhvRecordValid, "CurrentDhvRecordValid");
                Assert.AreEqual(null, EmParameters.FinalConversionFactor, "FinalConversionFactor,");
                Assert.AreEqual("HF", EmParameters.MatsDhvRecord.ParameterCd, "MatsDhvRecord");
                Assert.AreEqual("1.01E-2", EmParameters.MatsMhvCalculatedValue, "MatsMhvCalculatedValue");
                Assert.AreEqual("HFC", EmParameters.MatsMhvRecord.ParameterCd, "MatsMhvRecord");
                Assert.AreEqual("HF-3", EmParameters.MatsMoistureEquationList, "MatsMoistureEquationList");
                Assert.AreEqual("36,39", EmParameters.MatsDhvMeasuredModcList, "MatsDhvMeasuredModcList");
                Assert.AreEqual("38", EmParameters.MatsDhvUnavailableModcList, "MatsDhvUnavailableModcList");
            }

            /* MATS Load is 0 */
            {
                // Init Input
                EmParameters.CurrentHourlyOpRecord = new VwMpHrlyOpDataRow(matsHourLoad: 0m);
                EmParameters.MatsHfDhvParameter = "HFDHV";
                EmParameters.MatsHfDhvValid = true;
                EmParameters.MatsHfDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(parameterCd: "HF");
                EmParameters.MatsMhvCalculatedHfcValue = "1.01E-2";
                EmParameters.MatsHfcMhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData(parameterCd: "HFC");

                // Init Output
                category.CheckCatalogResult = null;


                // Run Checks
                actual = cMATSCalculatedHourlyValueChecks.MATSCHV3(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual(ECMPS.Definitions.Extensions.cExtensions.ScientificNotationtoDecimal("5.18E-8"), EmParameters.CalculationConversionFactor, "CalculationConversionFactor");
                Assert.AreEqual("HFDHV", EmParameters.CurrentDhvParameter, "CurrentDhvParameter,");
                Assert.AreEqual(true, EmParameters.CurrentDhvRecordValid, "CurrentDhvRecordValid");
                Assert.AreEqual(null, EmParameters.FinalConversionFactor, "FinalConversionFactor,");
                Assert.AreEqual("HF", EmParameters.MatsDhvRecord.ParameterCd, "MatsDhvRecord");
                Assert.AreEqual("1.01E-2", EmParameters.MatsMhvCalculatedValue, "MatsMhvCalculatedValue");
                Assert.AreEqual("HFC", EmParameters.MatsMhvRecord.ParameterCd, "MatsMhvRecord");
                Assert.AreEqual("HF-3", EmParameters.MatsMoistureEquationList, "MatsMoistureEquationList");
                Assert.AreEqual("36,39", EmParameters.MatsDhvMeasuredModcList, "MatsDhvMeasuredModcList");
                Assert.AreEqual("38", EmParameters.MatsDhvUnavailableModcList, "MatsDhvUnavailableModcList");
            }
        }

        /// <summary>
        ///A test for MATSCHV-4
        ///</summary>()
        [TestMethod()]
        public void MATSCHV4()
        {
            //static check setup
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            // Variables
            bool log = false;
            string actual;

            /* MATS Load is not null */
            {
                // Init Input
                EmParameters.CurrentHourlyOpRecord = new VwMpHrlyOpDataRow(matsHourLoad: 225m);
                EmParameters.MatsSo2DhvParameter = "So2DHV";
                EmParameters.MatsSo2DhvValid = true;
                EmParameters.MatsSo2DhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(parameterCd: "So2");

                // Init Output
                category.CheckCatalogResult = null;


                // Run Checks
                actual = cMATSCalculatedHourlyValueChecks.MATSCHV4(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual(ECMPS.Definitions.Extensions.cExtensions.ScientificNotationtoDecimal("1.66E-7"), EmParameters.CalculationConversionFactor, "CalculationConversionFactor");
                Assert.AreEqual("So2DHV", EmParameters.CurrentDhvParameter, "CurrentDhvParameter,");
                Assert.AreEqual(true, EmParameters.CurrentDhvRecordValid, "CurrentDhvRecordValid");
                Assert.AreEqual((1m / EmParameters.CurrentHourlyOpRecord.MatsHourLoad.Value), EmParameters.FinalConversionFactor, "FinalConversionFactor,");
                Assert.AreEqual("So2", EmParameters.MatsDhvRecord.ParameterCd, "MatsDhvRecord");
                Assert.AreEqual("S-3", EmParameters.MatsMoistureEquationList, "MatsMoistureEquationList");
                Assert.AreEqual("36,39", EmParameters.MatsDhvMeasuredModcList, "MatsDhvMeasuredModcList");
                Assert.AreEqual("38", EmParameters.MatsDhvUnavailableModcList, "MatsDhvUnavailableModcList");
            }

            /* MATS Load is null */
            {
                // Init Input
                EmParameters.CurrentHourlyOpRecord = new VwMpHrlyOpDataRow(matsHourLoad: null);
                EmParameters.MatsSo2DhvParameter = "So2DHV";
                EmParameters.MatsSo2DhvValid = true;
                EmParameters.MatsSo2DhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(parameterCd: "So2");

                // Init Output
                category.CheckCatalogResult = null;


                // Run Checks
                actual = cMATSCalculatedHourlyValueChecks.MATSCHV4(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual(ECMPS.Definitions.Extensions.cExtensions.ScientificNotationtoDecimal("1.66E-7"), EmParameters.CalculationConversionFactor, "CalculationConversionFactor");
                Assert.AreEqual("So2DHV", EmParameters.CurrentDhvParameter, "CurrentDhvParameter,");
                Assert.AreEqual(true, EmParameters.CurrentDhvRecordValid, "CurrentDhvRecordValid");
                Assert.AreEqual(null, EmParameters.FinalConversionFactor, "FinalConversionFactor,");
                Assert.AreEqual("So2", EmParameters.MatsDhvRecord.ParameterCd, "MatsDhvRecord");
                Assert.AreEqual("S-3", EmParameters.MatsMoistureEquationList, "MatsMoistureEquationList");
                Assert.AreEqual("36,39", EmParameters.MatsDhvMeasuredModcList, "MatsDhvMeasuredModcList");
                Assert.AreEqual("38", EmParameters.MatsDhvUnavailableModcList, "MatsDhvUnavailableModcList");
            }

            /* MATS Load is 0 */
            {
                // Init Input
                EmParameters.CurrentHourlyOpRecord = new VwMpHrlyOpDataRow(matsHourLoad: 0m);
                EmParameters.MatsSo2DhvParameter = "So2DHV";
                EmParameters.MatsSo2DhvValid = true;
                EmParameters.MatsSo2DhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(parameterCd: "So2");

                // Init Output
                category.CheckCatalogResult = null;


                // Run Checks
                actual = cMATSCalculatedHourlyValueChecks.MATSCHV4(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual(ECMPS.Definitions.Extensions.cExtensions.ScientificNotationtoDecimal("1.66E-7"), EmParameters.CalculationConversionFactor, "CalculationConversionFactor");
                Assert.AreEqual("So2DHV", EmParameters.CurrentDhvParameter, "CurrentDhvParameter,");
                Assert.AreEqual(true, EmParameters.CurrentDhvRecordValid, "CurrentDhvRecordValid");
                Assert.AreEqual(null, EmParameters.FinalConversionFactor, "FinalConversionFactor,");
                Assert.AreEqual("So2", EmParameters.MatsDhvRecord.ParameterCd, "MatsDhvRecord");
                Assert.AreEqual("S-3", EmParameters.MatsMoistureEquationList, "MatsMoistureEquationList");
                Assert.AreEqual("36,39", EmParameters.MatsDhvMeasuredModcList, "MatsDhvMeasuredModcList");
                Assert.AreEqual("38", EmParameters.MatsDhvUnavailableModcList, "MatsDhvUnavailableModcList");
            }
        }

        /// <summary>
        ///A test for MATSCHV-5
        ///</summary>()
        [TestMethod()]
        public void MATSCHV5()
        {
            //static check setup
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            // Variables
            bool log = false;
            string actual;

            // Init Input
            EmParameters.MatsHgDhvParameter = "HGDHV";
            EmParameters.MatsHgDhvValid = true;
            EmParameters.MatsHgDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(parameterCd: "HG");
            EmParameters.MatsMhvCalculatedHgcValue = "1.01E-2";
            EmParameters.MatsHgcMhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData(parameterCd: "HGC");

            // Init Output
            category.CheckCatalogResult = null;


            // Run Checks
            actual = cMATSCalculatedHourlyValueChecks.MATSCHV5(category, ref log);

            // Check Results
            Assert.AreEqual(string.Empty, actual);
            Assert.AreEqual(false, log);
            Assert.AreEqual(null, category.CheckCatalogResult, "Result");
            Assert.AreEqual(ECMPS.Definitions.Extensions.cExtensions.ScientificNotationtoDecimal("6.24E-11"), EmParameters.CalculationConversionFactor, "CalculationConversionFactor");
            Assert.AreEqual("HGDHV", EmParameters.CurrentDhvParameter, "CurrentDhvParameter,");
            Assert.AreEqual(true, EmParameters.CurrentDhvRecordValid, "CurrentDhvRecordValid");
            Assert.AreEqual(1000000m, EmParameters.FinalConversionFactor, "FinalConversionFactor,");
            Assert.AreEqual("HG", EmParameters.MatsDhvRecord.ParameterCd, "MatsDhvRecord");
            Assert.AreEqual("1.01E-2", EmParameters.MatsMhvCalculatedValue, "MatsMhvCalculatedValue");
            Assert.AreEqual("HGC", EmParameters.MatsMhvRecord.ParameterCd, "MatsMhvRecord");
            Assert.AreEqual("19-3,19-3D,19-4,19-5,19-8,19-9", EmParameters.MatsMoistureEquationList, "MatsMoistureEquationList");
            Assert.AreEqual("36,37", EmParameters.MatsDhvMeasuredModcList, "MatsDhvMeasuredModcList");
            Assert.AreEqual("38", EmParameters.MatsDhvUnavailableModcList, "MatsDhvUnavailableModcList");
        }

        /// <summary>
        ///A test for MATSCHV-6
        ///</summary>()
        [TestMethod()]
        public void MATSCHV6()
        {
            //static check setup
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            // Variables
            bool log = false;
            string actual;

            // Init Input
            EmParameters.MatsHclDhvParameter = "HCLDHV";
            EmParameters.MatsHclDhvValid = true;
            EmParameters.MatsHclDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(parameterCd: "HCL");
            EmParameters.MatsMhvCalculatedHclcValue = "1.01E-2";
            EmParameters.MatsHclcMhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData(parameterCd: "HCLC");

            // Init Output
            category.CheckCatalogResult = null;


            // Run Checks
            actual = cMATSCalculatedHourlyValueChecks.MATSCHV6(category, ref log);

            // Check Results
            Assert.AreEqual(string.Empty, actual);
            Assert.AreEqual(false, log);
            Assert.AreEqual(null, category.CheckCatalogResult, "Result");
            Assert.AreEqual(ECMPS.Definitions.Extensions.cExtensions.ScientificNotationtoDecimal("9.43E-8"), EmParameters.CalculationConversionFactor, "CalculationConversionFactor");
            Assert.AreEqual("HCLDHV", EmParameters.CurrentDhvParameter, "CurrentDhvParameter,");
            Assert.AreEqual(true, EmParameters.CurrentDhvRecordValid, "CurrentDhvRecordValid");
            Assert.AreEqual(1m, EmParameters.FinalConversionFactor, "FinalConversionFactor,");
            Assert.AreEqual("HCL", EmParameters.MatsDhvRecord.ParameterCd, "MatsDhvRecord");
            Assert.AreEqual("1.01E-2", EmParameters.MatsMhvCalculatedValue, "MatsMhvCalculatedValue");
            Assert.AreEqual("HCLC", EmParameters.MatsMhvRecord.ParameterCd, "MatsMhvRecord");
            Assert.AreEqual("19-3,19-3D,19-4,19-5,19-8,19-9", EmParameters.MatsMoistureEquationList, "MatsMoistureEquationList");
            Assert.AreEqual("36,37", EmParameters.MatsDhvMeasuredModcList, "MatsDhvMeasuredModcList");
            Assert.AreEqual("38", EmParameters.MatsDhvUnavailableModcList, "MatsDhvUnavailableModcList");
        }

        /// <summary>
        ///A test for MATSCHV-7
        ///</summary>()
        [TestMethod()]
        public void MATSCHV7()
        {
            //static check setup
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            // Variables
            bool log = false;
            string actual;

            // Init Input
            EmParameters.MatsHfDhvParameter = "HFDHV";
            EmParameters.MatsHfDhvValid = true;
            EmParameters.MatsHfDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(parameterCd: "HF");
            EmParameters.MatsMhvCalculatedHfcValue = "1.01E-2";
            EmParameters.MatsHfcMhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData(parameterCd: "HFC");

            // Init Output
            category.CheckCatalogResult = null;


            // Run Checks
            actual = cMATSCalculatedHourlyValueChecks.MATSCHV7(category, ref log);

            // Check Results
            Assert.AreEqual(string.Empty, actual);
            Assert.AreEqual(false, log);
            Assert.AreEqual(null, category.CheckCatalogResult, "Result");
            Assert.AreEqual(ECMPS.Definitions.Extensions.cExtensions.ScientificNotationtoDecimal("5.18E-8"), EmParameters.CalculationConversionFactor, "CalculationConversionFactor");
            Assert.AreEqual("HFDHV", EmParameters.CurrentDhvParameter, "CurrentDhvParameter,");
            Assert.AreEqual(true, EmParameters.CurrentDhvRecordValid, "CurrentDhvRecordValid");
            Assert.AreEqual(1m, EmParameters.FinalConversionFactor, "FinalConversionFactor,");
            Assert.AreEqual("HF", EmParameters.MatsDhvRecord.ParameterCd, "MatsDhvRecord");
            Assert.AreEqual("1.01E-2", EmParameters.MatsMhvCalculatedValue, "MatsMhvCalculatedValue");
            Assert.AreEqual("HFC", EmParameters.MatsMhvRecord.ParameterCd, "MatsMhvRecord");
            Assert.AreEqual("19-3,19-3D,19-4,19-5,19-8,19-9", EmParameters.MatsMoistureEquationList, "MatsMoistureEquationList");
            Assert.AreEqual("36,37", EmParameters.MatsDhvMeasuredModcList, "MatsDhvMeasuredModcList");
            Assert.AreEqual("38", EmParameters.MatsDhvUnavailableModcList, "MatsDhvUnavailableModcList");
        }

        /// <summary>
        ///A test for MATSCHV-8
        ///</summary>()
        [TestMethod()]
        public void MATSCHV8()
        {
            //static check setup
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            // Variables
            bool log = false;
            string actual;

            // Init Input
            EmParameters.MatsSo2DhvParameter = "So2DHV";
            EmParameters.MatsSo2DhvValid = true;
            EmParameters.MatsSo2DhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(parameterCd: "So2");

            // Init Output
            category.CheckCatalogResult = null;


            // Run Checks
            actual = cMATSCalculatedHourlyValueChecks.MATSCHV8(category, ref log);

            // Check Results
            Assert.AreEqual(string.Empty, actual);
            Assert.AreEqual(false, log);
            Assert.AreEqual(null, category.CheckCatalogResult, "Result");
            Assert.AreEqual(ECMPS.Definitions.Extensions.cExtensions.ScientificNotationtoDecimal("1.66E-7"), EmParameters.CalculationConversionFactor, "CalculationConversionFactor");
            Assert.AreEqual("So2DHV", EmParameters.CurrentDhvParameter, "CurrentDhvParameter,");
            Assert.AreEqual(true, EmParameters.CurrentDhvRecordValid, "CurrentDhvRecordValid");
            Assert.AreEqual(1m, EmParameters.FinalConversionFactor, "FinalConversionFactor,");
            Assert.AreEqual("So2", EmParameters.MatsDhvRecord.ParameterCd, "MatsDhvRecord");
            Assert.AreEqual("19-3,19-3D,19-4,19-5,19-8,19-9", EmParameters.MatsMoistureEquationList, "MatsMoistureEquationList");
            Assert.AreEqual("36,37", EmParameters.MatsDhvMeasuredModcList, "MatsDhvMeasuredModcList");
            Assert.AreEqual("38", EmParameters.MatsDhvUnavailableModcList, "MatsDhvUnavailableModcList");
        }

        #region MATSCHV-9

        /// <summary>
        ///A test for MATSCHV-9
        ///</summary>()
        [TestMethod()]
        public void MATSCHV9()
        {
            //static check setup
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            // Variables
            bool log = false;
            string actual;

            //All conditions true, MATS MHV MODC is 34
            {
                // Init Input
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(modcCd: "01");
                EmParameters.MatsDhvMeasuredModcList = "01,02";
                EmParameters.MatsMhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData(unadjustedHrlyValue: "2.02E-2", modcCd: "34");
                EmParameters.MatsMhvCalculatedValue = "1.01E-2";

                // Init Output
                category.CheckCatalogResult = null;

                // Run Checks
                actual = cMATSCalculatedHourlyValueChecks.MATSCHV9(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "{All conditions true, MATS MHV MODC is 34} Result");
                Assert.AreEqual(ECMPS.Definitions.Extensions.cExtensions.ScientificNotationtoDecimal("1.01E-2"), EmParameters.CalculationConcentration, "{All conditions true, MATS MHV MODC is 34} CalculationConcentration");
                Assert.AreEqual(true, EmParameters.CalculationConcentrationSubstituted, "{All conditions true, MATS MHV MODC is 34} CalculationConcentrationSubstituted");
            }

            //All conditions true, MATS MHV MODC is 35
            {
                // Init Input
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(modcCd: "01");
                EmParameters.MatsDhvMeasuredModcList = "01,02";
                EmParameters.MatsMhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData(unadjustedHrlyValue: "2.02E-2", modcCd: "35");
                EmParameters.MatsMhvCalculatedValue = "1.01E-2";

                // Init Output
                category.CheckCatalogResult = null;

                // Run Checks
                actual = cMATSCalculatedHourlyValueChecks.MATSCHV9(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "{All conditions true, MATS MHV MODC is 35} Result");
                Assert.AreEqual(ECMPS.Definitions.Extensions.cExtensions.ScientificNotationtoDecimal("1.01E-2"), EmParameters.CalculationConcentration, "{All conditions true, MATS MHV MODC is 35} CalculationConcentration");
                Assert.AreEqual(true, EmParameters.CalculationConcentrationSubstituted, "{All conditions true, MATS MHV MODC is 35} CalculationConcentrationSubstituted");
            }

            //MHV MODC not 38
            {
                // Init Input
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(modcCd: "01");
                EmParameters.MatsDhvMeasuredModcList = "01,02";
                EmParameters.MatsMhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData(unadjustedHrlyValue: "2.02E-2", modcCd: "99");
                EmParameters.MatsMhvCalculatedValue = "1.01E-2";

                // Init Output
                category.CheckCatalogResult = null;


                // Run Checks
                actual = cMATSCalculatedHourlyValueChecks.MATSCHV9(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "{MHV MODC not 38} Result");
                Assert.AreEqual(ECMPS.Definitions.Extensions.cExtensions.ScientificNotationtoDecimal("1.01E-2"), EmParameters.CalculationConcentration, "{MHV MODC not 38} CalculationConcentration");
                Assert.AreEqual(false, EmParameters.CalculationConcentrationSubstituted, "{MHV MODC not 38} CalculationConcentrationSubstituted");
            }

            //MatsMhvCalculatedValue is null
            {
                // Init Input
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(modcCd: "01");
                EmParameters.MatsDhvMeasuredModcList = "01,02";
                EmParameters.MatsMhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData(modcCd: "01");
                EmParameters.MatsMhvCalculatedValue = null;

                // Init Output
                category.CheckCatalogResult = null;


                // Run Checks
                actual = cMATSCalculatedHourlyValueChecks.MATSCHV9(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "{MatsMhvCalculatedValue is null} Result");
                Assert.AreEqual(null, EmParameters.CalculationConcentration, "{MatsMhvCalculatedValue is null} CalculationConcentration");
                Assert.AreEqual(false, EmParameters.CalculationConcentrationSubstituted, "{MatsMhvCalculatedValue is null} CalculationConcentrationSubstituted");
            }

            //CurrentDHVRecordValid = false
            {
                // Init Input
                EmParameters.CurrentDhvRecordValid = false;
                EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(modcCd: "01");
                EmParameters.MatsDhvMeasuredModcList = "01,02";
                EmParameters.MatsMhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData(unadjustedHrlyValue: "2.02E-2", modcCd: "01");
                EmParameters.MatsMhvCalculatedValue = "1.01E-2";

                // Init Output
                category.CheckCatalogResult = null;


                // Run Checks
                actual = cMATSCalculatedHourlyValueChecks.MATSCHV9(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "{CurrentDHVRecordValid = false} Result");
                Assert.AreEqual(null, EmParameters.CalculationConcentration, "{CurrentDHVRecordValid = false} CalculationConcentration");
                Assert.AreEqual(false, EmParameters.CalculationConcentrationSubstituted, "{CurrentDHVRecordValid = false} CalculationConcentrationSubstituted");
            }

            //DHV MODC Code not in list
            {
                // Init Input
                EmParameters.CurrentDhvRecordValid = false;
                EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(modcCd: "99");
                EmParameters.MatsDhvMeasuredModcList = "01,02,38";
                EmParameters.MatsMhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData(unadjustedHrlyValue: "2.02E-2", modcCd: "01");
                EmParameters.MatsMhvCalculatedValue = "1.01E-2";

                // Init Output
                category.CheckCatalogResult = null;


                // Run Checks
                actual = cMATSCalculatedHourlyValueChecks.MATSCHV9(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "{DHV MODC Code not in list} Result");
                Assert.AreEqual(null, EmParameters.CalculationConcentration, "{DHV MODC Code not in list} CalculationConcentration");
                Assert.AreEqual(false, EmParameters.CalculationConcentrationSubstituted, "{DHV MODC Code not in list} CalculationConcentrationSubstituted");
            }

        }
        #endregion

        #region MATSCHV-10

        /// <summary>
        ///A test for MATSCHV-10
        ///</summary>()
        [TestMethod()]
        public void MATSCHV10()
        {
            //static check setup
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            // Variables
            bool log = false;
            string actual;

            //All conditions true
            {
                // Init Input
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(modcCd: "01");
                EmParameters.MatsDhvMeasuredModcList = "01,02";
                EmParameters.CurrentSo2MonitorHourlyRecord = new VwMpMonitorHrlyValueSo2cRow(unadjustedHrlyValue: (decimal)1.01, modcCd: "05");

                // Init Output
                category.CheckCatalogResult = null;


                // Run Checks
                actual = cMATSCalculatedHourlyValueChecks.MATSCHV10(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual((decimal)1.01, EmParameters.CalculationConcentration, "CalculationConcentration");
                Assert.AreEqual(true, EmParameters.CalculationConcentrationSubstituted, "CalculationConcentrationSubstituted");
            }

            // CurrentSo2MonitorHourlyRecord is null
            {
                // Init Input
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(modcCd: "01");
                EmParameters.MatsDhvMeasuredModcList = "01,02";
                EmParameters.CurrentSo2MonitorHourlyRecord = null;

                // Init Output
                category.CheckCatalogResult = null;


                // Run Checks
                actual = cMATSCalculatedHourlyValueChecks.MATSCHV10(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual((decimal?)null, EmParameters.CalculationConcentration, "CalculationConcentration");
                Assert.AreEqual(false, EmParameters.CalculationConcentrationSubstituted, "CalculationConcentrationSubstituted");
            }

            //MHV Modc not in list
            {
                // Init Input
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(modcCd: "01");
                EmParameters.MatsDhvMeasuredModcList = "01,02";
                EmParameters.CurrentSo2MonitorHourlyRecord = new VwMpMonitorHrlyValueSo2cRow(unadjustedHrlyValue: (decimal)1.01, modcCd: "99");

                // Init Output
                category.CheckCatalogResult = null;


                // Run Checks
                actual = cMATSCalculatedHourlyValueChecks.MATSCHV10(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual((decimal)1.01, EmParameters.CalculationConcentration, "CalculationConcentration");
                Assert.AreEqual(false, EmParameters.CalculationConcentrationSubstituted, "CalculationConcentrationSubstituted");
            }

            //CurrentDHVRecordValid = false
            {
                // Init Input
                EmParameters.CurrentDhvRecordValid = false;
                EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(modcCd: "01");
                EmParameters.MatsDhvMeasuredModcList = "01,02";
                EmParameters.CurrentSo2MonitorHourlyRecord = new VwMpMonitorHrlyValueSo2cRow(unadjustedHrlyValue: (decimal)1.01, modcCd: "05");

                // Init Output
                category.CheckCatalogResult = null;


                // Run Checks
                actual = cMATSCalculatedHourlyValueChecks.MATSCHV10(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual(null, EmParameters.CalculationConcentration, "CalculationConcentration");
                Assert.AreEqual(false, EmParameters.CalculationConcentrationSubstituted, "CalculationConcentrationSubstituted");
            }

            //DHV Modc not in list
            {
                // Init Input
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(modcCd: "99");
                EmParameters.MatsDhvMeasuredModcList = "01,02";
                EmParameters.CurrentSo2MonitorHourlyRecord = new VwMpMonitorHrlyValueSo2cRow(unadjustedHrlyValue: (decimal)1.01, modcCd: "05");

                // Init Output
                category.CheckCatalogResult = null;


                // Run Checks
                actual = cMATSCalculatedHourlyValueChecks.MATSCHV10(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual(null, EmParameters.CalculationConcentration, "CalculationConcentration");
                Assert.AreEqual(false, EmParameters.CalculationConcentrationSubstituted, "CalculationConcentrationSubstituted");
            }

        }
        #endregion
        #endregion

        #region Checks 11-20

        #region MATSCHV-11

        /// <summary>
        ///A test for MATSCHV-11
        ///</summary>()
        [TestMethod()]
        public void MATSCHV11()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            /* Initialize Static Parameers */
            decimal? flowValue = 0.01m;
            string measuredModcList = "36,39";

            /* Initialize Static Parameers */
            EmParameters.MatsDhvMeasuredModcList = measuredModcList;

            /* Run Cases */
            foreach (bool? currentDhvRecordValid in UnitTestStandardLists.ValidList)
                foreach (string modcCd in UnitTestStandardLists.ModcCodeList)
                    foreach (bool stackFlowExists in UnitTestStandardLists.BooleanList)
                    {
                        string[] flowModcList = stackFlowExists ? UnitTestStandardLists.ModcCodeList : new string[] { null };

                        foreach (string flowModcCd in flowModcList)
                        {
                            /* Result Prefix */
                            string resultPrefix = string.Format("[valid: {0}, modc: {1}, flow: {2}]", currentDhvRecordValid, modcCd, stackFlowExists);

                            /* Expected Results */
                            string result = null;
                            decimal? expectedCalculationFlow = null;
                            bool? expectedCalculationFlowSubstituted = false;
                            {
                                if (currentDhvRecordValid.Default(false) && modcCd.InList(measuredModcList))
                                {
                                    if (stackFlowExists)
                                    {
                                        expectedCalculationFlow = flowValue;

                                        if (flowModcCd.NotInList("01,02,03,04,20,53,54"))
                                            expectedCalculationFlowSubstituted = true;
                                    }
                                    else
                                    {
                                        expectedCalculationFlow = null;
                                        expectedCalculationFlowSubstituted = false;
                                    }
                                }
                            }

                            /*  Initialize Input Parameters*/
                            EmParameters.CurrentDhvRecordValid = currentDhvRecordValid;
                            EmParameters.CurrentFlowMonitorHourlyRecord = (flowModcCd != null) ? new VwMpMonitorHrlyValueFlowRow(modcCd: flowModcCd, unadjustedHrlyValue: flowValue) : null;
                            EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(modcCd: modcCd);

                            /*  Initialize Output Parameters*/
                            EmParameters.CalculationFlow = null;
                            EmParameters.CalculationFlowSubstituted = null;

                            /* Run Check */
                            bool log = false;
                            category.CheckCatalogResult = null;
                            string actual = cMATSCalculatedHourlyValueChecks.MATSCHV11(category, ref log);

                            /* Check Results */
                            Assert.AreEqual(string.Empty, actual);
                            Assert.AreEqual(false, log);

                            Assert.AreEqual(result, category.CheckCatalogResult, resultPrefix + ".Result");
                            Assert.AreEqual(expectedCalculationFlow, EmParameters.CalculationFlow, resultPrefix + ".CalculationFlow");
                            Assert.AreEqual(expectedCalculationFlowSubstituted, EmParameters.CalculationFlowSubstituted, resultPrefix + ".CalculationFlowSubstituted");
                        }
                    }

        }

        #endregion

        #region MATSCHV-12

        /// <summary>
        ///A test for MATSCHV-12
        ///</summary>()
        [TestMethod()]
        public void MATSCHV12()
        {
            //static check setup
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            // Variables
            bool log = false;
            string actual;

            //Result A - MODC=14
            {
                // Init Input

                string[] testEquationList = { "19-1", "19-2", "19-3", "19-3D", "19-4", "19-5", "19-5D" };
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvMeasuredModcList = "01,02,37";
                foreach (string testEquationCode in testEquationList)
                {
                    EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(modcCd: "37", equationCd: testEquationCode);
                    EmParameters.MonitorDefaultRecordsByHourLocation = new CheckDataView<VwMpMonitorDefaultRow>(
                        new VwMpMonitorDefaultRow(parameterCd: "O2X", defaultPurposeCd: "DC", fuelCd: "NFS"),
                        new VwMpMonitorDefaultRow(parameterCd: "O2X", defaultPurposeCd: "DC", fuelCd: "NFS"));

                    // Init Output
                    category.CheckCatalogResult = null;

                    // Run Checks
                    actual = cMATSCalculatedHourlyValueChecks.MATSCHV12(category, ref log);

                    // Check Results
                    Assert.AreEqual(string.Empty, actual);
                    Assert.AreEqual(false, log);
                    Assert.AreEqual("A", category.CheckCatalogResult, "Result A - MODC 37");
                }
            }

            //Result A - EquationCode match
            {
                // Init Input
                string[] testEquationList = { "19-1", "19-2", "19-3", "19-3D", "19-4", "19-5", "19-5D" };
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvMeasuredModcList = "01,02,37";
                EmParameters.O2DryNeededForMats = false;
                EmParameters.O2WetNeededForMats = false;
                EmParameters.Co2DiluentNeededForMats = false;

                foreach (string testEquationCode in testEquationList)
                {
                    EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(modcCd: "01", equationCd: testEquationCode);
                    EmParameters.MonitorDefaultRecordsByHourLocation = new CheckDataView<VwMpMonitorDefaultRow>(
                        new VwMpMonitorDefaultRow(parameterCd: "O2X", defaultPurposeCd: "DC", fuelCd: "NFS"),
                        new VwMpMonitorDefaultRow(parameterCd: "O2X", defaultPurposeCd: "DC", fuelCd: "NFS"));

                    // Init Output
                    category.CheckCatalogResult = null;

                    // Run Checks
                    actual = cMATSCalculatedHourlyValueChecks.MATSCHV12(category, ref log);

                    // Check Results
                    Assert.AreEqual(string.Empty, actual);
                    Assert.AreEqual(false, log);
                    if (testEquationCode.InList("19-3D,19-5D"))
                    {
                        Assert.AreEqual("A", category.CheckCatalogResult, "Result A - MODC not 37");
                    }
                    else
                    {
                        Assert.AreEqual(null, category.CheckCatalogResult, "Result A - MODC not 37");
                    }
                }
            }

            //Result B
            {
                // Init Input

                string[] testEquationList = { "19-1", "19-2", "19-3", "19-3D", "19-4", "19-5", "19-5D" };
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvMeasuredModcList = "01,02,37";
                foreach (string testEquationCode in testEquationList)
                {
                    EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(modcCd: "37", equationCd: testEquationCode);
                    EmParameters.MonitorDefaultRecordsByHourLocation = new CheckDataView<VwMpMonitorDefaultRow>();

                    // Init Output
                    category.CheckCatalogResult = null;

                    // Run Checks
                    actual = cMATSCalculatedHourlyValueChecks.MATSCHV12(category, ref log);

                    // Check Results
                    Assert.AreEqual(string.Empty, actual);
                    Assert.AreEqual(false, log);
                    Assert.AreEqual("B", category.CheckCatalogResult, "Result");
                }
            }

            //Result C and null (pass)
            {
                // Init Input

                string[] testEquationList = { "19-1", "19-2", "19-3", "19-3D", "19-4", "19-5", "19-5D" };
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvMeasuredModcList = "01,02,37";
                decimal?[] testDefaultValueList = { null, (decimal)0.1, -1, 0, (decimal)0.1, 1 };

                foreach (string testEquationCode in testEquationList)
                {
                    foreach (decimal? testDefaultValue in testDefaultValueList)
                    {
                        EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(modcCd: "37", equationCd: testEquationCode);
                        EmParameters.MonitorDefaultRecordsByHourLocation = new CheckDataView<VwMpMonitorDefaultRow>(
                            new VwMpMonitorDefaultRow(parameterCd: "O2X", defaultPurposeCd: "DC", fuelCd: "NFS", defaultValue: testDefaultValue));

                        // Init Output
                        category.CheckCatalogResult = null;

                        // Run Checks
                        actual = cMATSCalculatedHourlyValueChecks.MATSCHV12(category, ref log);

                        // Check Results
                        Assert.AreEqual(string.Empty, actual);
                        Assert.AreEqual(false, log);
                        if (testDefaultValue != null && testDefaultValue <= 0)
                        {
                            Assert.AreEqual("C", category.CheckCatalogResult, "Result");
                            Assert.AreEqual(null, EmParameters.CalculationDiluent, "CalculationDiluent");
                        }
                        else
                        {
                            Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                            Assert.AreEqual(testDefaultValue, EmParameters.CalculationDiluent, "CalculationDiluent");
                        }
                    }

                }
            }


            //Result D
            {
                // Init Input

                string[] testEquationList = { "19-6", "19-7", "19-8", "19-9" };
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvMeasuredModcList = "01,02,37";
                foreach (string testEquationCode in testEquationList)
                {
                    EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(modcCd: "37", equationCd: testEquationCode);
                    EmParameters.MonitorDefaultRecordsByHourLocation = new CheckDataView<VwMpMonitorDefaultRow>(
                        new VwMpMonitorDefaultRow(parameterCd: "CO2N", defaultPurposeCd: "DC", fuelCd: "NFS"),
                        new VwMpMonitorDefaultRow(parameterCd: "CO2N", defaultPurposeCd: "DC", fuelCd: "NFS"));

                    // Init Output
                    category.CheckCatalogResult = null;

                    // Run Checks
                    actual = cMATSCalculatedHourlyValueChecks.MATSCHV12(category, ref log);

                    // Check Results
                    Assert.AreEqual(string.Empty, actual);
                    Assert.AreEqual(false, log);
                    Assert.AreEqual("D", category.CheckCatalogResult, "Result");
                }
            }

            //Result E
            {
                // Init Input

                string[] testEquationList = { "19-6", "19-7", "19-8", "19-9" };
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvMeasuredModcList = "01,02,37";
                foreach (string testEquationCode in testEquationList)
                {
                    EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(modcCd: "37", equationCd: testEquationCode);
                    EmParameters.MonitorDefaultRecordsByHourLocation = new CheckDataView<VwMpMonitorDefaultRow>();

                    // Init Output
                    category.CheckCatalogResult = null;

                    // Run Checks
                    actual = cMATSCalculatedHourlyValueChecks.MATSCHV12(category, ref log);

                    // Check Results
                    Assert.AreEqual(string.Empty, actual);
                    Assert.AreEqual(false, log);
                    Assert.AreEqual("E", category.CheckCatalogResult, "Result");
                }
            }

            //Result F and null (pass)
            {
                // Init Input

                string[] testEquationList = { "19-6", "19-7", "19-8", "19-9" };
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvMeasuredModcList = "01,02,37";
                decimal?[] testDefaultValueList = { null, (decimal)0.1, -1, 0, (decimal)0.1, 1 };

                foreach (string testEquationCode in testEquationList)
                {
                    foreach (decimal? testDefaultValue in testDefaultValueList)
                    {
                        EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(modcCd: "37", equationCd: testEquationCode);
                        EmParameters.MonitorDefaultRecordsByHourLocation = new CheckDataView<VwMpMonitorDefaultRow>(
                            new VwMpMonitorDefaultRow(parameterCd: "CO2N", defaultPurposeCd: "DC", fuelCd: "NFS", defaultValue: testDefaultValue));

                        // Init Output
                        category.CheckCatalogResult = null;

                        // Run Checks
                        actual = cMATSCalculatedHourlyValueChecks.MATSCHV12(category, ref log);

                        // Check Results
                        Assert.AreEqual(string.Empty, actual);
                        Assert.AreEqual(false, log);
                        if (testDefaultValue != null && testDefaultValue <= 0)
                        {
                            Assert.AreEqual("F", category.CheckCatalogResult, "Result");
                            Assert.AreEqual(null, EmParameters.CalculationDiluent, "CalculationDiluent");
                        }
                        else
                        {
                            Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                            Assert.AreEqual(testDefaultValue, EmParameters.CalculationDiluent, "CalculationDiluent");
                        }
                    }
                }
            }

            //Pass - O2Dry
            {
                // Init Input

                string[] testEquationList = { "19-1", "19-4" };
                string[] testModcList = { "01", "02", "03", "04", "17", "20", "53", "54", "99" };
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvMeasuredModcList = "01,02,37";
                EmParameters.O2DryNeededForMats = true;
                EmParameters.O2DryCalculatedAdjustedValue = (decimal)0.01;

                foreach (string testEquationCode in testEquationList)
                {
                    foreach (string testModcCode in testModcList)
                    {
                        EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(modcCd: "01", equationCd: testEquationCode);
                        EmParameters.O2DryModc = testModcCode;

                        // Init Output
                        category.CheckCatalogResult = null;

                        // Run Checks
                        actual = cMATSCalculatedHourlyValueChecks.MATSCHV12(category, ref log);

                        // Check Results
                        Assert.AreEqual(string.Empty, actual);
                        Assert.AreEqual(false, log);
                        Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                        Assert.AreEqual((decimal)0.01, EmParameters.CalculationDiluent, string.Format("[Pass - O2Dry] CalculationDiluent: Equation {0}, MODC {1}", testEquationCode, testModcCode));

                        if (testModcCode.InList("01,02,03,04,17,20,53,54"))
                        {
                            Assert.AreEqual(false, EmParameters.CalculationDiluentSubstituted, string.Format("[Pass - O2Dry] CalculationDiluentSubstituted: Equation {0}, MODC {1}", testEquationCode, testModcCode));
                        }
                        else
                        {
                            Assert.AreEqual(true, EmParameters.CalculationDiluentSubstituted, string.Format("[Pass - O2Dry] CalculationDiluentSubstituted: Equation {0}, MODC {1}", testEquationCode, testModcCode));
                        }

                    }
                }
            }

            //Pass - O2Wet
            {
                // Init Input

                string[] testEquationList = { "19-2", "19-3", "19-5" };
                string[] testModcList = { "01", "02", "03", "04", "17", "20", "53", "54", "99" };
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvMeasuredModcList = "01,02,37";
                EmParameters.O2WetNeededForMats = true;
                EmParameters.O2WetCalculatedAdjustedValue = (decimal)0.01;

                foreach (string testEquationCode in testEquationList)
                {
                    foreach (string testModcCode in testModcList)
                    {
                        EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(modcCd: "01", equationCd: testEquationCode);
                        EmParameters.O2WetModc = testModcCode;

                        // Init Output
                        category.CheckCatalogResult = null;

                        // Run Checks
                        actual = cMATSCalculatedHourlyValueChecks.MATSCHV12(category, ref log);

                        // Check Results
                        Assert.AreEqual(string.Empty, actual);
                        Assert.AreEqual(false, log);
                        Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                        Assert.AreEqual((decimal)0.01, EmParameters.CalculationDiluent, "CalculationDiluent");

                        if (testModcCode.InList("01,02,03,04,17,20,53,54"))
                        {
                            Assert.AreEqual(false, EmParameters.CalculationDiluentSubstituted, "CalculationDiluentSubstituted");
                        }
                        else
                        {
                            Assert.AreEqual(true, EmParameters.CalculationDiluentSubstituted, "CalculationDiluentSubstituted");
                        }
                    }
                }
            }

            //Pass - CO2 Diluent
            {
                // Init Input

                string[] testEquationList = { "19-6", "19-7", "19-8", "19-9" };
                string[] testModcList = { "01", "02", "03", "04", "17", "20", "21", "53", "54", "99" };
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvMeasuredModcList = "01,02,37";
                EmParameters.Co2DiluentNeededForMats = true;
                EmParameters.Co2cMhvCalculatedAdjustedValue = (decimal)0.01;

                foreach (string testEquationCode in testEquationList)
                {
                    foreach (string testModcCode in testModcList)
                    {
                        EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(modcCd: "01", equationCd: testEquationCode);
                        EmParameters.Co2cMhvModc = testModcCode;

                        // Init Output
                        category.CheckCatalogResult = null;

                        // Run Checks
                        actual = cMATSCalculatedHourlyValueChecks.MATSCHV12(category, ref log);

                        // Check Results
                        Assert.AreEqual(string.Empty, actual);
                        Assert.AreEqual(false, log);
                        Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                        Assert.AreEqual((decimal)0.01, EmParameters.CalculationDiluent, "CalculationDiluent");

                        if (testModcCode.InList("01,02,03,04,17,20,21,53,54"))
                        {
                            Assert.AreEqual(false, EmParameters.CalculationDiluentSubstituted, "CalculationDiluentSubstituted");
                        }
                        else
                        {
                            Assert.AreEqual(true, EmParameters.CalculationDiluentSubstituted, "CalculationDiluentSubstituted");
                        }

                    }
                }
            }


        }
        #endregion

        #region MATSCHV-13

        /// <summary>
        ///A test for MATSCHV-13 - Passing conditions
        ///</summary>()
        [TestMethod()]
        public void MATSCHV13_Pass()
        {
            //static check setup
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            // Variables
            bool log = false;
            string actual;
            string[] testModcList = { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "12", "21", "53", "54", "55", "99" };

            // Init Input
            EmParameters.CurrentDhvRecordValid = true;
            EmParameters.MatsDhvMeasuredModcList = "01,02";
            EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(modcCd: "01", equationCd: "M01");
            EmParameters.MatsMoistureEquationList = "M01, M02";

            //MWD
            {
                EmParameters.H2oMethodCode = "MWD";
                EmParameters.H2oDerivedHourlyChecksNeeded = true;
                EmParameters.H2oDhvCalculatedAdjustedValue = (decimal)0.01;

                foreach (string testModcCode in testModcList)
                {
                    EmParameters.H2oDhvModc = testModcCode;

                    // Init Output
                    category.CheckCatalogResult = null;

                    // Run Checks
                    actual = cMATSCalculatedHourlyValueChecks.MATSCHV13(category, ref log);

                    // Check Results
                    Assert.AreEqual(string.Empty, actual);
                    Assert.AreEqual(false, log);
                    Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                    Assert.AreEqual((decimal)0.01, EmParameters.CalculationMoisture, "CalculationMoisture");
                    if (testModcCode.InList("01,02,03,04,05,06,07,08,09,10,12,21,53,54,55"))
                    {
                        Assert.AreEqual(false, EmParameters.CalculationMoistureSubstituted, "CalculationMoistureSubstituted");
                    }
                    else
                    {
                        Assert.AreEqual(true, EmParameters.CalculationMoistureSubstituted, "CalculationMoistureSubstituted");
                    }
                }
            }

            //MMS
            {
                string[] testMethodList = { "MMS", "MTB" };
                EmParameters.H2oMonitorHourlyChecksNeeded = true;
                EmParameters.H2oMhvCalculatedAdjustedValue = (decimal)0.01;

                foreach (string testMethodCode in testMethodList)
                {
                    foreach (string testModcCode in testModcList)
                    {
                        EmParameters.H2oMhvModc = testModcCode;
                        EmParameters.H2oMethodCode = testMethodCode;

                        // Init Output
                        category.CheckCatalogResult = null;

                        // Run Checks
                        actual = cMATSCalculatedHourlyValueChecks.MATSCHV13(category, ref log);

                        // Check Results
                        Assert.AreEqual(string.Empty, actual);
                        Assert.AreEqual(false, log);
                        Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                        Assert.AreEqual((decimal)0.01, EmParameters.CalculationMoisture, "CalculationMoisture");
                        if (testModcCode.InList("01,02,03,04,06,07,08,09,10,12,21,53,54,55"))
                        {
                            Assert.AreEqual(false, EmParameters.CalculationMoistureSubstituted, "CalculationMoistureSubstituted");
                        }
                        else
                        {
                            Assert.AreEqual(true, EmParameters.CalculationMoistureSubstituted, "CalculationMoistureSubstituted");
                        }
                    }
                }

                //MDF
                {
                    EmParameters.H2oMethodCode = "MDF";
                    EmParameters.H2oDerivedHourlyChecksNeeded = true;
                    EmParameters.H2oDhvCalculatedAdjustedValue = (decimal)0.01;

                    foreach (string testModcCode in testModcList)
                    {
                        EmParameters.H2oDhvModc = testModcCode;

                        // Init Output
                        category.CheckCatalogResult = null;

                        // Run Checks
                        actual = cMATSCalculatedHourlyValueChecks.MATSCHV13(category, ref log);

                        // Check Results
                        Assert.AreEqual(string.Empty, actual);
                        Assert.AreEqual(false, log);
                        Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                        Assert.AreEqual((decimal)0.01, EmParameters.CalculationMoisture, "CalculationMoisture");
                        if (testModcCode.InList("01,02,03,04,05,06,07,08,09,10,12,21,53,54,55"))
                        {
                            Assert.AreEqual(false, EmParameters.CalculationMoistureSubstituted, "CalculationMoistureSubstituted");
                        }
                        else
                        {
                            Assert.AreEqual(true, EmParameters.CalculationMoistureSubstituted, "CalculationMoistureSubstituted");
                        }
                    }
                }

                //MDF - DHV not needed
                {
                    EmParameters.H2oMethodCode = "MDF";
                    EmParameters.H2oDerivedHourlyChecksNeeded = false;
                    EmParameters.H2oDefaultValue = (decimal)0.01;

                    // Init Output
                    category.CheckCatalogResult = null;

                    // Run Checks
                    actual = cMATSCalculatedHourlyValueChecks.MATSCHV13(category, ref log);

                    // Check Results
                    Assert.AreEqual(string.Empty, actual);
                    Assert.AreEqual(false, log);
                    Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                    Assert.AreEqual((decimal)0.01, EmParameters.CalculationMoisture, "CalculationMoisture");
                    Assert.AreEqual(false, EmParameters.CalculationMoistureSubstituted, "CalculationMoistureSubstituted");
                }
            }
        }

        /// <summary>
        ///A test for MATSCHV-13 - combinations of failure conditions
        ///</summary>()
        [TestMethod()]
        public void MATSCHV13_Fail()
        {
            //static check setup
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            // Variables
            bool log = false;
            string actual;

            //CurrentDhvRecordValid= false
            {
                // Init Input
                EmParameters.CurrentDhvRecordValid = false;
                EmParameters.MatsDhvMeasuredModcList = "01,02";
                EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(modcCd: "01", equationCd: "M01");
                EmParameters.MatsMoistureEquationList = "M01, M02";
                EmParameters.MatsDhvMeasuredModcList = "01,02";

                // Init Output
                category.CheckCatalogResult = null;

                // Run Checks
                actual = cMATSCalculatedHourlyValueChecks.MATSCHV13(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual(null, EmParameters.CalculationMoisture, "CalculationMoisture");
                Assert.AreEqual(false, EmParameters.CalculationMoistureSubstituted, "CalculationMoistureSubstituted");
            }

            //Modc code not in list
            {
                // Init Input
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvMeasuredModcList = "01,02";
                EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(modcCd: "01", equationCd: "M01");
                EmParameters.MatsMoistureEquationList = "M01, M02";
                EmParameters.MatsDhvMeasuredModcList = "02";

                // Init Output
                category.CheckCatalogResult = null;

                // Run Checks
                actual = cMATSCalculatedHourlyValueChecks.MATSCHV13(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual(null, EmParameters.CalculationMoisture, "CalculationMoisture");
                Assert.AreEqual(false, EmParameters.CalculationMoistureSubstituted, "CalculationMoistureSubstituted");
            }

            //Equation code not in list
            {
                // Init Input
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvMeasuredModcList = "01,02";
                EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(modcCd: "01", equationCd: "M99");
                EmParameters.MatsMoistureEquationList = "M01, M02";
                // Init Output
                category.CheckCatalogResult = null;

                // Run Checks
                actual = cMATSCalculatedHourlyValueChecks.MATSCHV13(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual(null, EmParameters.CalculationMoisture, "CalculationMoisture");
                Assert.AreEqual(false, EmParameters.CalculationMoistureSubstituted, "CalculationMoistureSubstituted");
            }


            //Method specific loop
            {
                // Init Input
                string[] testMethodList = { "BAD", "MWD", "MMS", "MTB", "MDF" };
                bool[] testBoolList = { true, false };
                decimal?[] testDecimalList = { null, (decimal)0.01 };

                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvMeasuredModcList = "01,02";
                EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(modcCd: "01", equationCd: "M01");
                EmParameters.MatsMoistureEquationList = "M01, M02";

                foreach (string testMethodCode in testMethodList)
                {
                    foreach (bool testBool in testBoolList)
                    {
                        foreach (decimal? testDecimalValue in testDecimalList)
                        {
                            EmParameters.H2oMethodCode = testMethodCode;
                            EmParameters.H2oDerivedHourlyChecksNeeded = testBool;
                            EmParameters.H2oMonitorHourlyChecksNeeded = testBool;
                            EmParameters.H2oDhvCalculatedAdjustedValue = testDecimalValue;
                            EmParameters.H2oMhvCalculatedAdjustedValue = testDecimalValue;
                            EmParameters.H2oDefaultValue = testDecimalValue;

                            // Init Output
                            category.CheckCatalogResult = null;

                            // Run Checks
                            actual = cMATSCalculatedHourlyValueChecks.MATSCHV13(category, ref log);

                            // Check Results
                            Assert.AreEqual(string.Empty, actual);
                            Assert.AreEqual(false, log);
                            Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                            if ((testMethodCode == "MWD" && testBool == true && testDecimalValue != null)
                                || (testMethodCode.InList("MMS,MTB") && testBool == true && testDecimalValue != null)
                                || (testMethodCode == "MDF" && testBool == true && testDecimalValue != null))
                            {
                                Assert.AreEqual((decimal)0.01, EmParameters.CalculationMoisture, "CalculationMoisture");
                                Assert.AreEqual(true, EmParameters.CalculationMoistureSubstituted, "CalculationMoistureSubstituted");
                            }
                            else if (testMethodCode == "MDF" && testBool == false && testDecimalValue != null)
                            {
                                Assert.AreEqual((decimal)0.01, EmParameters.CalculationMoisture, "CalculationMoisture");
                                Assert.AreEqual(false, EmParameters.CalculationMoistureSubstituted, "CalculationMoistureSubstituted");
                            }
                            else
                            {
                                Assert.AreEqual(null, EmParameters.CalculationMoisture, "CalculationMoisture");
                                Assert.AreEqual(false, EmParameters.CalculationMoistureSubstituted, "CalculationMoistureSubstituted");
                            }
                        }
                    }

                }

            }
        }

        #endregion

        /// <summary>
        /// MATSCHV-14: Complete
        ///</summary>()
        [TestMethod()]
        public void MatsChv14()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            /* Initialize General Variables */
            string[] modcList = new string[] { "81", "82", "83", "84", "85", "86" };
            string matsDhvMeasuredModcList = "81,82,83";
            string matsDhvUnavailableModcList = "84,85,86";
            string[] equationList = new string[] { "M-1", "M-2", "M-3", "N-1", "N-2", "N-3" };
            string matsMoistureEquationList = "M-1,M-2,M-3";

            /* Initialize General Parameters */
            EmParameters.CalculationConversionFactor = 3.0m;

            /* Run Cases */
            foreach (bool? currentDhvRecordValid in UnitTestStandardLists.ValidList)
                foreach (string modcCd in modcList)
                    foreach (string equationCd in equationList)
                        foreach (bool? calculationConcentrationSubstituted in UnitTestStandardLists.ValidList)
                            foreach (bool? calculationFlowSubstituted in UnitTestStandardLists.ValidList)
                                foreach (bool? calculationMoistureSubstituted in UnitTestStandardLists.ValidList)
                                {
                                    decimal?[] calculationConcentrationList = (calculationConcentrationSubstituted != null) ? new decimal?[] { null, 1.5m } : new decimal?[] { null };
                                    decimal?[] calculationFlowList = (calculationFlowSubstituted != null) ? new decimal?[] { null, 2.0m } : new decimal?[] { null };
                                    decimal?[] calculationMoistureList = (calculationMoistureSubstituted != null) ? new decimal?[] { null, 0.5m } : new decimal?[] { null };

                                    foreach (decimal? calculationConcentration in calculationConcentrationList)
                                        foreach (decimal? calculationFlow in calculationConcentrationList)
                                            foreach (decimal? calculationMoisture in calculationConcentrationList)
                                            {
                                                decimal?[] finalConversionFactorList;
                                                {
                                                    if (calculationConcentration.HasValue && calculationFlow.HasValue && (!equationCd.InList(matsMoistureEquationList) || calculationMoisture.HasValue))
                                                        finalConversionFactorList = new decimal?[] { (1000m / 225m), (decimal?)null };
                                                    else
                                                        finalConversionFactorList = new decimal?[] { (1000m / 225m) };
                                                }

                                                foreach (decimal? finalConversionFactor in finalConversionFactorList)
                                                {
                                                    /*  Initialize Input Parameters*/
                                                    EmParameters.CalculationConcentration = calculationConcentration;
                                                    EmParameters.CalculationConcentrationSubstituted = calculationConcentrationSubstituted;
                                                    EmParameters.CalculationFlow = calculationFlow;
                                                    EmParameters.CalculationFlowSubstituted = calculationFlowSubstituted;
                                                    EmParameters.CalculationMoisture = calculationMoisture;
                                                    EmParameters.CalculationMoistureSubstituted = calculationMoistureSubstituted;
                                                    EmParameters.CurrentDhvRecordValid = currentDhvRecordValid;
                                                    EmParameters.FinalConversionFactor = finalConversionFactor;
                                                    EmParameters.MatsDhvMeasuredModcList = matsDhvMeasuredModcList;
                                                    EmParameters.MatsDhvRecord = new MATSDerivedHourlyValueData(modcCd: modcCd, equationCd: equationCd);
                                                    EmParameters.MatsDhvUnavailableModcList = matsDhvUnavailableModcList;
                                                    EmParameters.MatsMoistureEquationList = matsMoistureEquationList;

                                                    /* Initialize Output Parameter */
                                                    EmParameters.CalculatedUnadjustedValue = decimal.MinValue;

                                                    /* Expected Results */
                                                    string result = null;
                                                    decimal? calculatedUnadjustedValue = null;
                                                    {
                                                        if (currentDhvRecordValid.Default(false))
                                                        {
                                                            if (modcCd.InList(matsDhvMeasuredModcList))
                                                            {
                                                                if (equationCd != null)
                                                                {
                                                                    if (equationCd.InList(matsMoistureEquationList))
                                                                    {
                                                                        if (calculationConcentrationSubstituted.Default(false) || calculationFlowSubstituted.Default(false) || calculationMoistureSubstituted.Default(false))
                                                                            result = "A";
                                                                        else if ((calculationConcentration == null) || (calculationFlow == null) || (calculationMoisture == null))
                                                                            result = "B";
                                                                        else if (finalConversionFactor.HasValue)
                                                                            calculatedUnadjustedValue = EmParameters.CalculationConversionFactor
                                                                                                      * calculationConcentration
                                                                                                      * calculationFlow
                                                                                                      * (1 - calculationMoisture/100)
                                                                                                      * finalConversionFactor.Value;
                                                                    }
                                                                    else
                                                                    {
                                                                        if (calculationConcentrationSubstituted.Default(false) || calculationFlowSubstituted.Default(false))
                                                                            result = "C";
                                                                        else if ((calculationConcentration == null) || (calculationFlow == null))
                                                                            result = "D";
                                                                        else if (finalConversionFactor.HasValue)
                                                                            calculatedUnadjustedValue = EmParameters.CalculationConversionFactor
                                                                                                      * calculationConcentration
                                                                                                      * calculationFlow
                                                                                                      * finalConversionFactor.Value;
                                                                    }
                                                                }
                                                                else
                                                                    result = "E";
                                                            }
                                                        }
                                                    }

                                                    /* Init Cateogry Result */
                                                    category.CheckCatalogResult = null;

                                                    /* Initialize variables needed to run the check. */
                                                    bool log = false;

                                                    /* Run Check */
                                                    string error = cMATSCalculatedHourlyValueChecks.MATSCHV14(category, ref log);

                                                    /* Check Result Label */
                                                    string resultPrefix = string.Format("[modc: {0}, eq: {1}, con: {2}, flow: {3}, h2o: {4}, conS: {5}, flowS: {6}, h2oS: {7}, val: {8}]",
                                                                                        modcCd,
                                                                                        equationCd,
                                                                                        calculationConcentration,
                                                                                        calculationFlow,
                                                                                        calculationMoisture,
                                                                                        calculationConcentrationSubstituted,
                                                                                        calculationFlowSubstituted,
                                                                                        calculationMoistureSubstituted,
                                                                                        currentDhvRecordValid);

                                                    /* Check Results */
                                                    Assert.AreEqual(string.Empty, error, resultPrefix + ".Error");
                                                    Assert.AreEqual(false, log, resultPrefix + ".Log");
                                                    Assert.AreEqual(result, category.CheckCatalogResult, resultPrefix + ".Result");

                                                    Assert.AreEqual(calculatedUnadjustedValue, EmParameters.CalculatedUnadjustedValue, resultPrefix + ".CalculatedUnadjustedValue");
                                                }
                                            }
                                }
        }

        #region MATSCHV-15

        /// <summary>
        /// MATSCHV-15: Results A and B.  Any combination of values that make it to checking a particular formula will get result A.
        ///</summary>()
        [TestMethod()]
        public void MatsChv15_NonEquationSpecific()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            /* Initialize General Variables */
            string[] modcList = new string[] { "81", "82", "83", "84", "85", "86" };
            string matsDhvMeasuredModcList = "81,82,83";
            string matsDhvUnavailableModcList = "84,85,86";
            string[] equationList = new string[] { null, "19-0", "19-1", "19-2", "19-3", "19-3D", "19-4", "19-5", "19-5D", "19-6", "19-7", "19-8", "19-9" };
            string equationValidList = "19-1,19-2,19-3,19-3D,19-4,19-5,19-5D,19-6,19-7,19-8,19-9";
            string[] fFactorList = new string[] { null, "C", "D", "W" };
            string[] substitutedList = new string[] { null, "", "C", "D", "M" };

            /* Initialize General Parameters */
            EmParameters.CalculationConversionFactor = 3.0m;

            /* Run Cases */
            foreach (bool? currentDhvRecordValid in UnitTestStandardLists.ValidList)
                foreach (string modcCd in modcList)
                    foreach (string equationCd in equationList)
                    {
                        bool? calculationConcentrationSubstituted, calculationDiluentSubstituted, calculationMoistureSubstituted;
                        {
                            calculationConcentrationSubstituted = calculationDiluentSubstituted = calculationMoistureSubstituted = null;
                        }

                        bool? validFcFactorExists, validFdFactorExists, validFwFactorExists;
                        {
                            validFcFactorExists = validFdFactorExists = validFwFactorExists = null;
                        }

                        decimal? fcFactor, fdFactor, fwFactor;
                        {
                            fcFactor = fdFactor = fwFactor = (decimal?)null;
                        }

                        decimal? calculationConcentration, calculationDiluent, calculationMoisture;
                        {
                            calculationConcentration = calculationDiluent = calculationMoisture = (decimal?)null;
                        }

                        /*  Initialize Input Parameters*/
                        EmParameters.CalculationConcentration = calculationConcentration;
                        EmParameters.CalculationConcentrationSubstituted = calculationConcentrationSubstituted;
                        EmParameters.CalculationDiluent = calculationDiluent;
                        EmParameters.CalculationDiluentSubstituted = calculationDiluentSubstituted;
                        EmParameters.CalculationMoisture = calculationMoisture;
                        EmParameters.CalculationMoistureSubstituted = calculationMoistureSubstituted;
                        EmParameters.CurrentDhvRecordValid = currentDhvRecordValid;
                        EmParameters.CurrentHourlyOpRecord = new VwMpHrlyOpDataRow(fcFactor: fcFactor, fdFactor: fdFactor, fwFactor: fwFactor);
                        EmParameters.MatsDhvMeasuredModcList = matsDhvMeasuredModcList;
                        EmParameters.MatsDhvRecord = new MATSDerivedHourlyValueData(modcCd: modcCd, equationCd: equationCd);
                        EmParameters.MatsDhvUnavailableModcList = matsDhvUnavailableModcList;
                        EmParameters.MonitorDefaultRecordsByHourLocation = new CheckDataView<VwMpMonitorDefaultRow>();

                        /* Initialize Optional Parameters */
                        EmParameters.ValidFcFactorExists = validFcFactorExists;
                        EmParameters.ValidFdFactorExists = validFdFactorExists;
                        EmParameters.ValidFwFactorExists = validFwFactorExists;

                        /* Initialize Output Parameter */
                        EmParameters.CalculatedUnadjustedValue = decimal.MinValue;

                        /* Expected Results */
                        string result = null;
                        decimal? calculatedUnadjustedValue = null;
                        {
                            if (currentDhvRecordValid.Default(false))
                            {
                                if (modcCd.InList(matsDhvMeasuredModcList))
                                {
                                    if (equationCd != null)
                                    {
                                        if (equationCd.InList(equationValidList))
                                            result = "A";
                                    }
                                    else
                                        result = "B";
                                }
                            }
                        }

                        /* Check Result Label */
                        string resultPrefix = string.Format("A, B and C - [modc: {0}, eq: {1}, val: {2}]",
                                                            modcCd,
                                                            equationCd,
                                                            currentDhvRecordValid);

                        /* Init Cateogry Result */
                        category.CheckCatalogResult = null;

                        /* Initialize variables needed to run the check. */
                        bool log = false;

                        /* Run Check */
                        string error = cMATSCalculatedHourlyValueChecks.MATSCHV15(category, ref log);

                        /* Check Results */
                        Assert.AreEqual(string.Empty, error, resultPrefix + ".Error");
                        Assert.AreEqual(false, log, resultPrefix + ".Log");
                        Assert.AreEqual(result, category.CheckCatalogResult, resultPrefix + ".Result");

                        Assert.AreEqual(calculatedUnadjustedValue.HasValue ? Math.Round(calculatedUnadjustedValue.Value, 3) : (decimal?)null,
                                        EmParameters.CalculatedUnadjustedValue.HasValue ? Math.Round(EmParameters.CalculatedUnadjustedValue.Value, 3) : (decimal?)null,
                                        resultPrefix + ".CalculatedUnadjustedValue");
                    }
        }

        /// <summary>
        /// MATSCHV-15: Results A and C
        ///</summary>()
        [TestMethod()]
        public void MatsChv15_EquationSpecific_AandC()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            /* Initialize General Variables */
            string[] modcList = new string[] { "81" };
            string modcCd = "81";
            string matsDhvMeasuredModcList = "81";
            string matsDhvUnavailableModcList = "91";
            string[] equationList = new string[] { "19-1", "19-2", "19-3", "19-3D", "19-4", "19-5", "19-5D", "19-6", "19-7", "19-8", "19-9" };
            string[] fFactorList = new string[] { null, "C", "D", "W" };
            string[] substitutedList = new string[] { null, "", "C", "D", "M" };

            /* Initialize General Parameters */
            EmParameters.CalculationConversionFactor = 3.0m;
            EmParameters.CurrentDhvRecordValid = true;

            /* Run Cases */
            foreach (string equationCd in equationList)
                foreach (string substituted in substitutedList)
                    foreach (string fFactor in fFactorList)
                    {
                        bool? calculationConcentrationSubstituted, calculationDiluentSubstituted, calculationMoistureSubstituted;
                        {
                            if (substituted != null)
                            {
                                calculationConcentrationSubstituted = (substituted == "C");
                                calculationDiluentSubstituted = (substituted == "D");
                                calculationMoistureSubstituted = (substituted == "M");
                            }
                            else
                            {
                                calculationConcentrationSubstituted = calculationDiluentSubstituted = calculationMoistureSubstituted = null;
                            }
                        }

                        bool? validFcFactorExists, validFdFactorExists, validFwFactorExists;
                        {
                            if (fFactor != null)
                            {
                                validFcFactorExists = (fFactor == "C");
                                validFdFactorExists = (fFactor == "D");
                                validFwFactorExists = (fFactor == "W");
                            }
                            else
                            {
                                validFcFactorExists = validFdFactorExists = validFwFactorExists = null;
                            }
                        }

                        decimal? fcFactor = validFcFactorExists.Default(false) ? 3.0m : (decimal?)null;
                        decimal? fdFactor = validFdFactorExists.Default(false) ? 4.0m : (decimal?)null;
                        decimal? fwFactor = validFwFactorExists.Default(false) ? 5.0m : (decimal?)null;

                        decimal?[] calculationConcentrationList = !calculationConcentrationSubstituted.Default(true) ? new decimal?[] { null, 1.5m } : new decimal?[] { null };
                        decimal?[] calculationDiluentList = !calculationDiluentSubstituted.Default(true) ? new decimal?[] { null, 2.0m } : new decimal?[] { null };
                        decimal?[] calculationMoistureList = !calculationMoistureSubstituted.Default(true) ? new decimal?[] { null, 0.5m } : new decimal?[] { null };

                        foreach (decimal? calculationConcentration in calculationConcentrationList)
                            foreach (decimal? calculationDiluent in calculationDiluentList)
                                foreach (decimal? calculationMoisture in calculationMoistureList)
                                {
                                    /*  Initialize Input Parameters*/
                                    EmParameters.CalculationConcentration = calculationConcentration;
                                    EmParameters.CalculationConcentrationSubstituted = calculationConcentrationSubstituted;
                                    EmParameters.CalculationDiluent = calculationDiluent;
                                    EmParameters.CalculationDiluentSubstituted = calculationDiluentSubstituted;
                                    EmParameters.CalculationMoisture = calculationMoisture;
                                    EmParameters.CalculationMoistureSubstituted = calculationMoistureSubstituted;
                                    EmParameters.CurrentHourlyOpRecord = new VwMpHrlyOpDataRow(fcFactor: fcFactor, fdFactor: fdFactor, fwFactor: fwFactor);
                                    EmParameters.FinalConversionFactor = 1000000m;
                                    EmParameters.MatsDhvMeasuredModcList = matsDhvMeasuredModcList;
                                    EmParameters.MatsDhvRecord = new MATSDerivedHourlyValueData(modcCd: modcCd, equationCd: equationCd);
                                    EmParameters.MatsDhvUnavailableModcList = matsDhvUnavailableModcList;
                                    EmParameters.MonitorDefaultRecordsByHourLocation = new CheckDataView<VwMpMonitorDefaultRow>();

                                    /* Initialize Optional Parameters */
                                    EmParameters.ValidFcFactorExists = validFcFactorExists;
                                    EmParameters.ValidFdFactorExists = validFdFactorExists;
                                    EmParameters.ValidFwFactorExists = validFwFactorExists;

                                    /* Initialize Output Parameter */
                                    EmParameters.CalculatedUnadjustedValue = decimal.MinValue;

                                    /* Expected Results */
                                    string result = null;
                                    decimal? calculatedUnadjustedValue = null;
                                    {
                                        switch (equationCd)
                                        {
                                            case "19-1":
                                                {
                                                    if (calculationConcentrationSubstituted.Default(true) || calculationDiluentSubstituted.Default(true))
                                                        result = "A";
                                                    else if ((calculationConcentration == null) || (calculationDiluent == null) || !validFdFactorExists.Default(false))
                                                        result = "C";
                                                    else
                                                        calculatedUnadjustedValue = EmParameters.CalculationConversionFactor
                                                                                  * calculationConcentration
                                                                                  * fdFactor
                                                                                  * (20.9m / (20.9m - calculationDiluent))
                                                                                  * EmParameters.FinalConversionFactor;
                                                }
                                                break;

                                            case "19-2":
                                                {
                                                    if (calculationConcentrationSubstituted.Default(true) || calculationDiluentSubstituted.Default(true))
                                                        result = "A";
                                                    else if ((calculationConcentration == null) || (calculationDiluent == null) || !validFwFactorExists.Default(false))
                                                        result = "C";
                                                    else
                                                        calculatedUnadjustedValue = EmParameters.CalculationConversionFactor
                                                                                  * calculationConcentration
                                                                                  * fwFactor
                                                                                  * (20.9m / (20.9m * (1 - 0.027m) - calculationDiluent))
                                                                                  * EmParameters.FinalConversionFactor;
                                                }
                                                break;

                                            case "19-3":
                                                {
                                                    if (calculationConcentrationSubstituted.Default(true) || calculationDiluentSubstituted.Default(true) || calculationMoistureSubstituted.Default(true))
                                                        result = "A";
                                                    else if ((calculationConcentration == null) || (calculationDiluent == null) || !validFdFactorExists.Default(false) || (calculationMoisture == null))
                                                        result = "C";
                                                    else
                                                        calculatedUnadjustedValue = EmParameters.CalculationConversionFactor
                                                                                  * calculationConcentration
                                                                                  * fdFactor
                                                                                  * (20.9m / (20.9m * ((100 - calculationMoisture) / 100m) - calculationDiluent))
                                                                                  * EmParameters.FinalConversionFactor;
                                                }
                                                break;

                                            case "19-3D":
                                                {
                                                    if (calculationConcentrationSubstituted.Default(true) || calculationDiluentSubstituted.Default(true) || calculationMoistureSubstituted.Default(true))
                                                        result = "A";
                                                    else if ((calculationConcentration == null) || (calculationDiluent == null) || !validFdFactorExists.Default(false) || (calculationMoisture == null))
                                                        result = "C";
                                                    else
                                                        calculatedUnadjustedValue = EmParameters.CalculationConversionFactor
                                                                                  * calculationConcentration
                                                                                  * fdFactor
                                                                                  * (20.9m / ((20.9m * ((100 - calculationMoisture) / 100m)) - (calculationDiluent * ((100 - calculationMoisture) / 100m))))
                                                                                  * EmParameters.FinalConversionFactor;
                                                }
                                                break;

                                            case "19-4":
                                                {
                                                    if (calculationConcentrationSubstituted.Default(true) || calculationDiluentSubstituted.Default(true) || calculationMoistureSubstituted.Default(true))
                                                        result = "A";
                                                    else if ((calculationConcentration == null) || (calculationDiluent == null) || !validFdFactorExists.Default(false) || (calculationMoisture == null))
                                                        result = "C";
                                                    else
                                                        calculatedUnadjustedValue = EmParameters.CalculationConversionFactor
                                                                                  * calculationConcentration
                                                                                  * fdFactor
                                                                                  * (1 / ((100 - calculationMoisture) / 100m))
                                                                                  * (20.9m / (20.9m - calculationDiluent))
                                                                                  * EmParameters.FinalConversionFactor;
                                                }
                                                break;

                                            case "19-5":
                                                {
                                                    if (calculationConcentrationSubstituted.Default(true) || calculationDiluentSubstituted.Default(true) || calculationMoistureSubstituted.Default(true))
                                                        result = "A";
                                                    else if ((calculationConcentration == null) || (calculationDiluent == null) || !validFdFactorExists.Default(false) || (calculationMoisture == null))
                                                        result = "C";
                                                    else
                                                        calculatedUnadjustedValue = EmParameters.CalculationConversionFactor
                                                                                  * calculationConcentration
                                                                                  * fdFactor
                                                                                  * (20.9m / (20.9m - calculationDiluent / ((100 - calculationMoisture) / 100m)))
                                                                                  * EmParameters.FinalConversionFactor;
                                                }
                                                break;

                                            case "19-5D":
                                                {
                                                    if (calculationConcentrationSubstituted.Default(true) || calculationDiluentSubstituted.Default(true))
                                                        result = "A";
                                                    else if ((calculationConcentration == null) || (calculationDiluent == null) || !validFdFactorExists.Default(false))
                                                        result = "C";
                                                    else
                                                        calculatedUnadjustedValue = EmParameters.CalculationConversionFactor
                                                                                  * calculationConcentration
                                                                                  * fdFactor
                                                                                  * (20.9m / (20.9m - calculationDiluent))
                                                                                  * EmParameters.FinalConversionFactor;
                                                }
                                                break;

                                            case "19-6":
                                            case "19-7":
                                                {
                                                    if (calculationConcentrationSubstituted.Default(true) || calculationDiluentSubstituted.Default(true))
                                                        result = "A";
                                                    else if ((calculationConcentration == null) || (calculationDiluent == null) || !validFcFactorExists.Default(false))
                                                        result = "C";
                                                    else
                                                        calculatedUnadjustedValue = EmParameters.CalculationConversionFactor
                                                                                  * calculationConcentration
                                                                                  * fcFactor
                                                                                  * (100m / calculationDiluent)
                                                                                  * EmParameters.FinalConversionFactor;
                                                }
                                                break;

                                            case "19-8":
                                                {
                                                    if (calculationConcentrationSubstituted.Default(true) || calculationDiluentSubstituted.Default(true) || calculationMoistureSubstituted.Default(true))
                                                        result = "A";
                                                    else if ((calculationConcentration == null) || (calculationDiluent == null) || !validFcFactorExists.Default(false) || (calculationMoisture == null))
                                                        result = "C";
                                                    else
                                                        calculatedUnadjustedValue = EmParameters.CalculationConversionFactor
                                                                                  * calculationConcentration
                                                                                  * fcFactor
                                                                                  * (1 / ((100 - calculationMoisture) / 100m))
                                                                                  * (100m / calculationDiluent)
                                                                                  * EmParameters.FinalConversionFactor;
                                                }
                                                break;

                                            case "19-9":
                                                {
                                                    if (calculationConcentrationSubstituted.Default(true) || calculationDiluentSubstituted.Default(true) || calculationMoistureSubstituted.Default(true))
                                                        result = "A";
                                                    else if ((calculationConcentration == null) || (calculationDiluent == null) || !validFcFactorExists.Default(false) || (calculationMoisture == null))
                                                        result = "C";
                                                    else
                                                        calculatedUnadjustedValue = EmParameters.CalculationConversionFactor
                                                                                  * calculationConcentration
                                                                                  * fcFactor
                                                                                  * ((100 - calculationMoisture) / 100m)
                                                                                  * (100m / calculationDiluent)
                                                                                  * EmParameters.FinalConversionFactor;
                                                }
                                                break;
                                        }
                                    }

                                    /* Check Result Label */
                                    string resultPrefix = string.Format("A, B and C - [eq: {0}, sub: {1}, fF: {2}, con: {3}, dil: {4}, h2o: {5}]",
                                                                        equationCd,
                                                                        substituted,
                                                                        fFactor,
                                                                        calculationConcentration,
                                                                        calculationDiluent,
                                                                        calculationMoisture);

                                    /* Init Cateogry Result */
                                    category.CheckCatalogResult = null;

                                    /* Initialize variables needed to run the check. */
                                    bool log = false;

                                    /* Run Check */
                                    string error = cMATSCalculatedHourlyValueChecks.MATSCHV15(category, ref log);

                                    /* Check Results */
                                    Assert.AreEqual(string.Empty, error, resultPrefix + ".Error");
                                    Assert.AreEqual(false, log, resultPrefix + ".Log");
                                    Assert.AreEqual(result, category.CheckCatalogResult, resultPrefix + ".Result");

                                    Assert.AreEqual(calculatedUnadjustedValue.HasValue ? Math.Round(calculatedUnadjustedValue.Value, 3) : (decimal?)null,
                                                    EmParameters.CalculatedUnadjustedValue.HasValue ? Math.Round(EmParameters.CalculatedUnadjustedValue.Value, 3) : (decimal?)null,
                                                    resultPrefix + ".CalculatedUnadjustedValue");
                                }
                    }

        }

        /// <summary>
        ///A test for MATSCHV-15
        ///</summary>()

        [TestMethod()]
        public void MATSCHV15_Formula19_1()
        {
            //static check setup
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            // Variables
            bool log = false;
            string actual;
            decimal testValue = (decimal)0.01;

            //Result A
            {
                // Init Input
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(equationCd: "19-1", modcCd: "01");
                EmParameters.CalculationConcentrationSubstituted = false;
                EmParameters.CalculationDiluentSubstituted = false;
                EmParameters.FinalConversionFactor = 1000000m;
                EmParameters.MatsDhvMeasuredModcList = "01,02";
                EmParameters.MatsDhvUnavailableModcList = "03";

                int[] testComboList = { 1, 2 };

                foreach (int testCombo in testComboList)
                {
                    if (testCombo == 1)
                        EmParameters.CalculationConcentrationSubstituted = true;
                    else
                        EmParameters.CalculationDiluentSubstituted = true;

                    // Init Output
                    EmParameters.CalculatedUnadjustedValue = -123456789m;
                    category.CheckCatalogResult = null;


                    // Run Checks
                    actual = cMATSCalculatedHourlyValueChecks.MATSCHV15(category, ref log);

                    // Check Results
                    Assert.AreEqual(string.Empty, actual);
                    Assert.AreEqual(false, log);
                    Assert.AreEqual("A", category.CheckCatalogResult, "Result");
                    Assert.AreEqual(null, EmParameters.CalculatedUnadjustedValue, "CalculatedUnadjustedValue");
                }
            }

            //Result B
            {
                // Init Input
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(equationCd: null, modcCd: "01");
                EmParameters.CalculationConcentrationSubstituted = false;
                EmParameters.CalculationDiluentSubstituted = false;
                EmParameters.FinalConversionFactor = 1000000m;
                EmParameters.MatsDhvMeasuredModcList = "01,02";
                EmParameters.MatsDhvUnavailableModcList = "03";


                // Init Output
                EmParameters.CalculatedUnadjustedValue = -123456789m;
                category.CheckCatalogResult = null;


                // Run Checks
                actual = cMATSCalculatedHourlyValueChecks.MATSCHV15(category, ref log);

                // Check Results
                string label = "Result B";
                Assert.AreEqual(string.Empty, actual, string.Format("actual: [{0}]", label));
                Assert.AreEqual(false, log, string.Format("log: [{0}]", label));
                Assert.AreEqual("B", category.CheckCatalogResult, string.Format("result: [{0}]", label));
                Assert.AreEqual(null, EmParameters.CalculatedUnadjustedValue, "CalculatedUnadjustedValue");
            }

            //Result C
            {
                // Init Input
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(equationCd: "19-1", modcCd: "01");
                EmParameters.CalculationConcentration = testValue;
                EmParameters.CalculationDiluent = testValue;
                EmParameters.ValidFdFactorExists = true;
                EmParameters.FinalConversionFactor = 1000000m;
                EmParameters.MatsDhvMeasuredModcList = "01,02";
                EmParameters.MatsDhvUnavailableModcList = "03";

                int[] testComboList = { 1, 2, 3 };

                foreach (int testCombo in testComboList)
                {
                    if (testCombo == 1)
                        EmParameters.CalculationConcentration = null;
                    else if (testCombo == 2)
                        EmParameters.CalculationDiluent = null;
                    else
                        EmParameters.ValidFdFactorExists = false;

                    // Init Output
                    EmParameters.CalculatedUnadjustedValue = -123456789m;
                    category.CheckCatalogResult = null;

                    // Run Checks
                    actual = cMATSCalculatedHourlyValueChecks.MATSCHV15(category, ref log);

                    // Check Results
                    Assert.AreEqual(string.Empty, actual);
                    Assert.AreEqual(false, log);
                    Assert.AreEqual("C", category.CheckCatalogResult, "Result");
                    Assert.AreEqual(null, EmParameters.CalculatedUnadjustedValue, "CalculatedUnadjustedValue");
                }
            }

            //Result D
            {
                // Init Input
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(equationCd: "19-1", modcCd: "01");
                EmParameters.CalculationConcentration = testValue;
                EmParameters.CalculationDiluent = (decimal)20.9;
                EmParameters.ValidFdFactorExists = true;
                EmParameters.FinalConversionFactor = 1000000m;
                EmParameters.MatsDhvMeasuredModcList = "01,02";
                EmParameters.MatsDhvUnavailableModcList = "03";


                // Init Output
                EmParameters.CalculatedUnadjustedValue = -123456789m;
                category.CheckCatalogResult = null;


                // Run Checks
                actual = cMATSCalculatedHourlyValueChecks.MATSCHV15(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual("D", category.CheckCatalogResult, "Result");
                Assert.AreEqual(null, EmParameters.CalculatedUnadjustedValue, "CalculatedUnadjustedValue");
            }

            //Pass
            {
                // Init Input
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(equationCd: "19-1", modcCd: "01");
                EmParameters.CalculationConcentration = testValue;
                EmParameters.CalculationDiluent = testValue;
                EmParameters.CalculationConversionFactor = testValue;
                EmParameters.ValidFdFactorExists = true;
                EmParameters.FinalConversionFactor = 1000000m;
                EmParameters.MatsDhvMeasuredModcList = "01,02";
                EmParameters.MatsDhvUnavailableModcList = "03";
                EmParameters.CurrentHourlyOpRecord = new VwMpHrlyOpDataRow(fdFactor: testValue);

                decimal formula = (testValue * testValue * testValue * (20.9m / (20.9m - testValue))) * EmParameters.FinalConversionFactor.Value;

                // Init Output
                EmParameters.CalculatedUnadjustedValue = -123456789m;
                category.CheckCatalogResult = null;

                // Run Checks
                actual = cMATSCalculatedHourlyValueChecks.MATSCHV15(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual(formula, EmParameters.CalculatedUnadjustedValue, "CalculatedUnadjustedValue");
            }

            // Pass with unavailable MODC and existing equation code
            {
                // Init Input
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(equationCd: "19-1", modcCd: "03");
                EmParameters.CalculationConcentrationSubstituted = false;
                EmParameters.CalculationDiluentSubstituted = false;
                EmParameters.FinalConversionFactor = 1000000m;
                EmParameters.MatsDhvMeasuredModcList = "01,02";
                EmParameters.MatsDhvUnavailableModcList = "03";


                // Init Output
                EmParameters.CalculatedUnadjustedValue = -123456789m;
                category.CheckCatalogResult = null;


                // Run Checks
                actual = cMATSCalculatedHourlyValueChecks.MATSCHV15(category, ref log);

                // Check Results
                string label = "Unavailable MODC with Equation";
                Assert.AreEqual(string.Empty, actual, string.Format("actual: [{0}]", label));
                Assert.AreEqual(false, log, string.Format("log: [{0}]", label));
                Assert.AreEqual(null, category.CheckCatalogResult, string.Format("result: [{0}]", label));
                Assert.AreEqual(null, EmParameters.CalculatedUnadjustedValue, "CalculatedUnadjustedValue");
            }

            // Pass with null Final Conversion Factor
            {
                // Init Input
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(equationCd: "19-1", modcCd: "01");
                EmParameters.CalculationConcentration = testValue;
                EmParameters.CalculationDiluent = testValue;
                EmParameters.CalculationConversionFactor = testValue;
                EmParameters.ValidFdFactorExists = true;
                EmParameters.FinalConversionFactor = null;
                EmParameters.MatsDhvMeasuredModcList = "01,02";
                EmParameters.MatsDhvUnavailableModcList = "03";
                EmParameters.CurrentHourlyOpRecord = new VwMpHrlyOpDataRow(fdFactor: testValue);

                // Init Output
                EmParameters.CalculatedUnadjustedValue = -123456789m;
                category.CheckCatalogResult = null;

                // Run Checks
                actual = cMATSCalculatedHourlyValueChecks.MATSCHV15(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual(null, EmParameters.CalculatedUnadjustedValue, "CalculatedUnadjustedValue");
            }
        }

        [TestMethod()]
        public void MATSCHV15_Formula19_2()
        {
            //static check setup
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            // Variables
            bool log = false;
            string actual;
            decimal testValue = (decimal)0.01;

            //Result A
            {
                // Init Input
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(equationCd: "19-2", modcCd: "01");
                EmParameters.CalculationConcentrationSubstituted = false;
                EmParameters.CalculationDiluentSubstituted = false;
                EmParameters.FinalConversionFactor = 1000000m;
                EmParameters.MatsDhvMeasuredModcList = "01,02";
                EmParameters.MatsDhvUnavailableModcList = "03";

                int[] testComboList = { 1, 2 };

                foreach (int testCombo in testComboList)
                {
                    if (testCombo == 1)
                        EmParameters.CalculationConcentrationSubstituted = true;
                    else
                        EmParameters.CalculationDiluentSubstituted = true;

                    // Init Output
                    EmParameters.CalculatedUnadjustedValue = -123456789m;
                    category.CheckCatalogResult = null;


                    // Run Checks
                    actual = cMATSCalculatedHourlyValueChecks.MATSCHV15(category, ref log);

                    // Check Results
                    Assert.AreEqual(string.Empty, actual);
                    Assert.AreEqual(false, log);
                    Assert.AreEqual("A", category.CheckCatalogResult, "Result");
                    Assert.AreEqual(null, EmParameters.CalculatedUnadjustedValue, "CalculatedUnadjustedValue");
                }
            }

            //Result B
            {
                // Init Input
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(equationCd: null, modcCd: "01");
                EmParameters.CalculationConcentrationSubstituted = false;
                EmParameters.CalculationDiluentSubstituted = false;
                EmParameters.FinalConversionFactor = 1000000m;
                EmParameters.MatsDhvMeasuredModcList = "01,02";
                EmParameters.MatsDhvUnavailableModcList = "03";


                // Init Output
                EmParameters.CalculatedUnadjustedValue = -123456789m;
                category.CheckCatalogResult = null;


                // Run Checks
                actual = cMATSCalculatedHourlyValueChecks.MATSCHV15(category, ref log);

                // Check Results
                string label = "Result B";
                Assert.AreEqual(string.Empty, actual, string.Format("actual: [{0}]", label));
                Assert.AreEqual(false, log, string.Format("log: [{0}]", label));
                Assert.AreEqual("B", category.CheckCatalogResult, string.Format("result: [{0}]", label));
                Assert.AreEqual(null, EmParameters.CalculatedUnadjustedValue, "CalculatedUnadjustedValue");
            }

            //Result F
            {
                // Init Input
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(equationCd: "19-2", modcCd: "01");
                EmParameters.CalculationConcentration = testValue;
                EmParameters.CalculationDiluent = testValue;
                EmParameters.ValidFdFactorExists = true;
                EmParameters.FinalConversionFactor = 1000000m;
                EmParameters.MatsDhvMeasuredModcList = "01,02";
                EmParameters.MatsDhvUnavailableModcList = "03";

                EmParameters.MonitorDefaultRecordsByHourLocation = new CheckDataView<VwMpMonitorDefaultRow>(
                                new VwMpMonitorDefaultRow(parameterCd: "BWA", defaultValue: (decimal)0.01),
                                new VwMpMonitorDefaultRow(parameterCd: "BWA", defaultValue: (decimal)0.01));

                // Init Output
                EmParameters.CalculatedUnadjustedValue = -123456789m;
                category.CheckCatalogResult = null;

                // Run Checks
                actual = cMATSCalculatedHourlyValueChecks.MATSCHV15(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual("F", category.CheckCatalogResult, "Result");
                Assert.AreEqual(null, EmParameters.CalculatedUnadjustedValue, "CalculatedUnadjustedValue");
            }

            //Result C
            {
                int[] testComboList = { 1, 2, 3, 4 };
                // Init Input
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(equationCd: "19-2", modcCd: "02");
                EmParameters.CalculationConcentration = testValue;
                EmParameters.CalculationDiluent = testValue;
                EmParameters.ValidFdFactorExists = true;
                EmParameters.FinalConversionFactor = 1000000m;
                EmParameters.MatsDhvMeasuredModcList = "02";
                EmParameters.MatsDhvUnavailableModcList = "03";

                foreach (int testCombo in testComboList)
                {
                    if (testCombo == 1)
                    {
                        EmParameters.CalculationDiluent = null;
                        EmParameters.MonitorDefaultRecordsByHourLocation = new CheckDataView<VwMpMonitorDefaultRow>();
                    }
                    else if (testCombo == 2)
                    {
                        EmParameters.CalculationConcentration = null;
                        EmParameters.MonitorDefaultRecordsByHourLocation = new CheckDataView<VwMpMonitorDefaultRow>();
                    }
                    else if (testCombo == 3)
                    {
                        EmParameters.ValidFwFactorExists = false;
                        EmParameters.MonitorDefaultRecordsByHourLocation = new CheckDataView<VwMpMonitorDefaultRow>();
                    }
                    else //variable MoistureFraction is null b/c default value is greater than or equal to 1.
                    {
                        EmParameters.MonitorDefaultRecordsByHourLocation = new CheckDataView<VwMpMonitorDefaultRow>(new VwMpMonitorDefaultRow(parameterCd: "BWA", defaultValue: (decimal)1));
                    }

                    // Init Output
                    EmParameters.CalculatedUnadjustedValue = -123456789m;
                    category.CheckCatalogResult = null;

                    // Run Checks
                    actual = cMATSCalculatedHourlyValueChecks.MATSCHV15(category, ref log);

                    // Check Results
                    Assert.AreEqual(string.Empty, actual);
                    Assert.AreEqual(false, log);
                    Assert.AreEqual("C", category.CheckCatalogResult, "Result");
                    Assert.AreEqual(null, EmParameters.CalculatedUnadjustedValue, "CalculatedUnadjustedValue");
                }
            }

            //Result D
            {
                // Init Input
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(equationCd: "19-2", modcCd: "01");
                EmParameters.CalculationConcentrationSubstituted = false;
                EmParameters.CalculationDiluentSubstituted = false;
                EmParameters.FinalConversionFactor = 1000000m;
                EmParameters.MatsDhvMeasuredModcList = "01,02";
                EmParameters.MatsDhvUnavailableModcList = "03";
                EmParameters.MonitorDefaultRecordsByHourLocation = new CheckDataView<VwMpMonitorDefaultRow>(
                    new VwMpMonitorDefaultRow(parameterCd: "BWA", defaultValue: testValue));
                EmParameters.CalculationDiluent = (decimal)20.9 * (1 - testValue);
                EmParameters.CalculationConcentration = testValue;
                EmParameters.ValidFwFactorExists = true;

                // Init Output
                EmParameters.CalculatedUnadjustedValue = -123456789m;
                category.CheckCatalogResult = null;


                // Run Checks
                actual = cMATSCalculatedHourlyValueChecks.MATSCHV15(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual("D", category.CheckCatalogResult, "Result");
                Assert.AreEqual(null, EmParameters.CalculatedUnadjustedValue, "CalculatedUnadjustedValue");
            }

            //Pass
            {
                // Init Input
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(equationCd: "19-2", modcCd: "01");
                EmParameters.CalculationConcentrationSubstituted = false;
                EmParameters.CalculationDiluentSubstituted = false;
                EmParameters.FinalConversionFactor = 1000000m;
                EmParameters.MatsDhvMeasuredModcList = "01,02";
                EmParameters.MatsDhvUnavailableModcList = "03";
                EmParameters.MonitorDefaultRecordsByHourLocation = new CheckDataView<VwMpMonitorDefaultRow>(
                    new VwMpMonitorDefaultRow(parameterCd: "BWA", defaultValue: testValue));
                EmParameters.CalculationDiluent = testValue;
                EmParameters.CalculationConcentration = testValue;
                EmParameters.ValidFwFactorExists = true;
                EmParameters.CalculationConversionFactor = testValue;
                EmParameters.CurrentHourlyOpRecord = new VwMpHrlyOpDataRow(fwFactor: testValue);

                decimal formula = (testValue * testValue * testValue * (20.9m / (20.9m * (1 - testValue) - testValue))) * EmParameters.FinalConversionFactor.Value;

                // Init Output
                EmParameters.CalculatedUnadjustedValue = -123456789m;
                category.CheckCatalogResult = null;


                // Run Checks
                actual = cMATSCalculatedHourlyValueChecks.MATSCHV15(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual(formula, EmParameters.CalculatedUnadjustedValue, "CalculatedUnadjustedValue");
            }

            // Pass with unavailable MODC and existing equation code
            {
                // Init Input
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(equationCd: "19-2", modcCd: "03");
                EmParameters.CalculationConcentrationSubstituted = false;
                EmParameters.CalculationDiluentSubstituted = false;
                EmParameters.FinalConversionFactor = 1000000m;
                EmParameters.MatsDhvMeasuredModcList = "01,02";
                EmParameters.MatsDhvUnavailableModcList = "03";


                // Init Output
                EmParameters.CalculatedUnadjustedValue = -123456789m;
                category.CheckCatalogResult = null;


                // Run Checks
                actual = cMATSCalculatedHourlyValueChecks.MATSCHV15(category, ref log);

                // Check Results
                string label = "Unavailable MODC with Equation";
                Assert.AreEqual(string.Empty, actual, string.Format("actual: [{0}]", label));
                Assert.AreEqual(false, log, string.Format("log: [{0}]", label));
                Assert.AreEqual(null, category.CheckCatalogResult, string.Format("result: [{0}]", label));
                Assert.AreEqual(null, EmParameters.CalculatedUnadjustedValue, "CalculatedUnadjustedValue");
            }

            // Pass with null Final Conversion Factor
            {
                // Init Input
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(equationCd: "19-2", modcCd: "01");
                EmParameters.CalculationConcentrationSubstituted = false;
                EmParameters.CalculationDiluentSubstituted = false;
                EmParameters.FinalConversionFactor = null;
                EmParameters.MatsDhvMeasuredModcList = "01,02";
                EmParameters.MatsDhvUnavailableModcList = "03";
                EmParameters.MonitorDefaultRecordsByHourLocation = new CheckDataView<VwMpMonitorDefaultRow>(
                    new VwMpMonitorDefaultRow(parameterCd: "BWA", defaultValue: testValue));
                EmParameters.CalculationDiluent = testValue;
                EmParameters.CalculationConcentration = testValue;
                EmParameters.ValidFwFactorExists = true;
                EmParameters.CalculationConversionFactor = testValue;
                EmParameters.CurrentHourlyOpRecord = new VwMpHrlyOpDataRow(fwFactor: testValue);

                // Init Output
                EmParameters.CalculatedUnadjustedValue = -123456789m;
                category.CheckCatalogResult = null;


                // Run Checks
                actual = cMATSCalculatedHourlyValueChecks.MATSCHV15(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual(null, EmParameters.CalculatedUnadjustedValue, "CalculatedUnadjustedValue");
            }

        }

        [TestMethod()]
        public void MATSCHV15_Formula19_3()
        {
            //static check setup
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            // Variables
            bool log = false;
            string actual;
            decimal testValue = (decimal)0.01;

            //Result A
            {
                // Init Input
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(equationCd: "19-3", modcCd: "01");
                EmParameters.CalculationConcentrationSubstituted = false;
                EmParameters.CalculationDiluentSubstituted = false;
                EmParameters.CalculationMoistureSubstituted = false;
                EmParameters.FinalConversionFactor = 1000000m;
                EmParameters.MatsDhvMeasuredModcList = "01,02";
                EmParameters.MatsDhvUnavailableModcList = "03";

                int[] testComboList = { 1, 2, 3 };

                foreach (int testCombo in testComboList)
                {
                    if (testCombo == 1)
                        EmParameters.CalculationConcentrationSubstituted = true;
                    else if (testCombo == 2)
                        EmParameters.CalculationDiluentSubstituted = true;
                    else
                        EmParameters.CalculationMoistureSubstituted = true;

                    // Init Output
                    EmParameters.CalculatedUnadjustedValue = -123456789m;
                    category.CheckCatalogResult = null;


                    // Run Checks
                    actual = cMATSCalculatedHourlyValueChecks.MATSCHV15(category, ref log);

                    // Check Results
                    Assert.AreEqual(string.Empty, actual);
                    Assert.AreEqual(false, log);
                    Assert.AreEqual("A", category.CheckCatalogResult, "Result");
                    Assert.AreEqual(null, EmParameters.CalculatedUnadjustedValue, "CalculatedUnadjustedValue");
                }
            }

            //Result B
            {
                // Init Input
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(equationCd: null, modcCd: "01");
                EmParameters.CalculationConcentrationSubstituted = false;
                EmParameters.CalculationDiluentSubstituted = false;
                EmParameters.CalculationMoistureSubstituted = false;
                EmParameters.FinalConversionFactor = 1000000m;
                EmParameters.MatsDhvMeasuredModcList = "01,02";
                EmParameters.MatsDhvUnavailableModcList = "03";


                // Init Output
                EmParameters.CalculatedUnadjustedValue = -123456789m;
                category.CheckCatalogResult = null;


                // Run Checks
                actual = cMATSCalculatedHourlyValueChecks.MATSCHV15(category, ref log);

                // Check Results
                string label = "Result B";
                Assert.AreEqual(string.Empty, actual, string.Format("actual: [{0}]", label));
                Assert.AreEqual(false, log, string.Format("log: [{0}]", label));
                Assert.AreEqual("B", category.CheckCatalogResult, string.Format("result: [{0}]", label));
                Assert.AreEqual(null, EmParameters.CalculatedUnadjustedValue, "CalculatedUnadjustedValue");
            }

            //Result C
            {
                // Init Input
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(equationCd: "19-3", modcCd: "01");
                EmParameters.CalculationConcentration = testValue;
                EmParameters.CalculationDiluent = testValue;
                EmParameters.CalculationMoisture = testValue;
                EmParameters.ValidFdFactorExists = true;
                EmParameters.FinalConversionFactor = 1000000m;
                EmParameters.MatsDhvMeasuredModcList = "01,02";
                EmParameters.MatsDhvUnavailableModcList = "03";

                int[] testComboList = { 1, 2, 3, 4 };

                foreach (int testCombo in testComboList)
                {
                    if (testCombo == 1)
                        EmParameters.CalculationConcentration = null;
                    else if (testCombo == 2)
                        EmParameters.CalculationDiluent = null;
                    else if (testCombo == 3)
                        EmParameters.ValidFdFactorExists = false;
                    else
                        EmParameters.CalculationMoisture = null;

                    // Init Output
                    EmParameters.CalculatedUnadjustedValue = -123456789m;
                    category.CheckCatalogResult = null;

                    // Run Checks
                    actual = cMATSCalculatedHourlyValueChecks.MATSCHV15(category, ref log);

                    // Check Results
                    Assert.AreEqual(string.Empty, actual);
                    Assert.AreEqual(false, log);
                    Assert.AreEqual("C", category.CheckCatalogResult, "Result");
                    Assert.AreEqual(null, EmParameters.CalculatedUnadjustedValue, "CalculatedUnadjustedValue");
                }
            }

            //Result D
            {
                // Init Input
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(equationCd: "19-3", modcCd: "01");
                EmParameters.CalculationConcentration = testValue;
                EmParameters.CalculationMoisture = testValue;
                EmParameters.CalculationDiluent = (decimal)20.9 * ((100 - testValue) / 100);
                EmParameters.ValidFdFactorExists = true;
                EmParameters.FinalConversionFactor = 1000000m;
                EmParameters.MatsDhvMeasuredModcList = "01,02";
                EmParameters.MatsDhvUnavailableModcList = "03";


                // Init Output
                EmParameters.CalculatedUnadjustedValue = -123456789m;
                category.CheckCatalogResult = null;


                // Run Checks
                actual = cMATSCalculatedHourlyValueChecks.MATSCHV15(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual("D", category.CheckCatalogResult, "Result");
                Assert.AreEqual(null, EmParameters.CalculatedUnadjustedValue, "CalculatedUnadjustedValue");
            }

            //Pass
            {
                // Init Input
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(equationCd: "19-3", modcCd: "01");
                EmParameters.CalculationConcentration = testValue;
                EmParameters.CalculationDiluent = testValue;
                EmParameters.CalculationConversionFactor = testValue;
                EmParameters.CalculationMoisture = testValue;
                EmParameters.ValidFdFactorExists = true;
                EmParameters.FinalConversionFactor = 1000000m;
                EmParameters.MatsDhvMeasuredModcList = "01,02";
                EmParameters.MatsDhvUnavailableModcList = "03";
                EmParameters.CurrentHourlyOpRecord = new VwMpHrlyOpDataRow(fdFactor: testValue);

                decimal h2oFactor = (100 - testValue) / 100;
                decimal denom = ((decimal)20.9 * h2oFactor) - testValue;
                decimal formula = (testValue * testValue * testValue * ((Decimal)20.9 / denom)) * EmParameters.FinalConversionFactor.Value;

                // Init Output
                EmParameters.CalculatedUnadjustedValue = -123456789m;
                category.CheckCatalogResult = null;

                // Run Checks
                actual = cMATSCalculatedHourlyValueChecks.MATSCHV15(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual(formula, EmParameters.CalculatedUnadjustedValue, "CalculatedUnadjustedValue");
            }

            // Pass with unavailable MODC and existing equation code
            {
                // Init Input
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(equationCd: "19-3", modcCd: "03");
                EmParameters.CalculationConcentrationSubstituted = false;
                EmParameters.CalculationDiluentSubstituted = false;
                EmParameters.FinalConversionFactor = 1000000m;
                EmParameters.MatsDhvMeasuredModcList = "01,02";
                EmParameters.MatsDhvUnavailableModcList = "03";


                // Init Output
                EmParameters.CalculatedUnadjustedValue = -123456789m;
                category.CheckCatalogResult = null;


                // Run Checks
                actual = cMATSCalculatedHourlyValueChecks.MATSCHV15(category, ref log);

                // Check Results
                string label = "Unavailable MODC with Equation";
                Assert.AreEqual(string.Empty, actual, string.Format("actual: [{0}]", label));
                Assert.AreEqual(false, log, string.Format("log: [{0}]", label));
                Assert.AreEqual(null, category.CheckCatalogResult, string.Format("result: [{0}]", label));
                Assert.AreEqual(null, EmParameters.CalculatedUnadjustedValue, "CalculatedUnadjustedValue");
            }

            // Pass with null Final Conversion Factor
            {
                // Init Input
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(equationCd: "19-3", modcCd: "01");
                EmParameters.CalculationConcentration = testValue;
                EmParameters.CalculationDiluent = testValue;
                EmParameters.CalculationConversionFactor = testValue;
                EmParameters.CalculationMoisture = testValue;
                EmParameters.ValidFdFactorExists = true;
                EmParameters.FinalConversionFactor = null;
                EmParameters.MatsDhvMeasuredModcList = "01,02";
                EmParameters.MatsDhvUnavailableModcList = "03";
                EmParameters.CurrentHourlyOpRecord = new VwMpHrlyOpDataRow(fdFactor: testValue);

                decimal h2oFactor = (100 - testValue) / 100;
                decimal denom = ((decimal)20.9 * h2oFactor) - testValue;

                // Init Output
                EmParameters.CalculatedUnadjustedValue = -123456789m;
                category.CheckCatalogResult = null;

                // Run Checks
                actual = cMATSCalculatedHourlyValueChecks.MATSCHV15(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual(null, EmParameters.CalculatedUnadjustedValue, "CalculatedUnadjustedValue");
            }
        }

        [TestMethod()]
        public void MATSCHV15_Formula19_3D()
        {
            //static check setup
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            // Variables
            bool log = false;
            string actual;
            decimal testValue = (decimal)0.01;

            //Result A
            {
                // Init Input
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(equationCd: "19-3D", modcCd: "01");
                EmParameters.CalculationConcentrationSubstituted = false;
                EmParameters.CalculationDiluentSubstituted = false;
                EmParameters.CalculationMoistureSubstituted = false;
                EmParameters.FinalConversionFactor = 1000000m;
                EmParameters.MatsDhvMeasuredModcList = "01,02";
                EmParameters.MatsDhvUnavailableModcList = "03";

                int[] testComboList = { 1, 2, 3 };

                foreach (int testCombo in testComboList)
                {
                    if (testCombo == 1)
                        EmParameters.CalculationConcentrationSubstituted = true;
                    else if (testCombo == 2)
                        EmParameters.CalculationDiluentSubstituted = true;
                    else
                        EmParameters.CalculationMoistureSubstituted = true;

                    // Init Output
                    EmParameters.CalculatedUnadjustedValue = -123456789m;
                    category.CheckCatalogResult = null;


                    // Run Checks
                    actual = cMATSCalculatedHourlyValueChecks.MATSCHV15(category, ref log);

                    // Check Results
                    Assert.AreEqual(string.Empty, actual);
                    Assert.AreEqual(false, log);
                    Assert.AreEqual("A", category.CheckCatalogResult, "Result");
                    Assert.AreEqual(null, EmParameters.CalculatedUnadjustedValue, "CalculatedUnadjustedValue");
                }
            }

            //Result B
            {
                // Init Input
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(equationCd: null, modcCd: "01");
                EmParameters.CalculationConcentrationSubstituted = false;
                EmParameters.CalculationDiluentSubstituted = false;
                EmParameters.CalculationMoistureSubstituted = false;
                EmParameters.FinalConversionFactor = 1000000m;
                EmParameters.MatsDhvMeasuredModcList = "01,02";
                EmParameters.MatsDhvUnavailableModcList = "03";


                // Init Output
                EmParameters.CalculatedUnadjustedValue = -123456789m;
                category.CheckCatalogResult = null;


                // Run Checks
                actual = cMATSCalculatedHourlyValueChecks.MATSCHV15(category, ref log);

                // Check Results
                string label = "Result B";
                Assert.AreEqual(string.Empty, actual, string.Format("actual: [{0}]", label));
                Assert.AreEqual(false, log, string.Format("log: [{0}]", label));
                Assert.AreEqual("B", category.CheckCatalogResult, string.Format("result: [{0}]", label));
                Assert.AreEqual(null, EmParameters.CalculatedUnadjustedValue, "CalculatedUnadjustedValue");
            }

            //Result C
            {
                // Init Input
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(equationCd: "19-3D", modcCd: "01");
                EmParameters.CalculationConcentration = testValue;
                EmParameters.CalculationDiluent = testValue;
                EmParameters.CalculationMoisture = testValue;
                EmParameters.ValidFdFactorExists = true;
                EmParameters.FinalConversionFactor = 1000000m;
                EmParameters.MatsDhvMeasuredModcList = "01,02";
                EmParameters.MatsDhvUnavailableModcList = "03";

                int[] testComboList = { 1, 2, 3, 4 };

                foreach (int testCombo in testComboList)
                {
                    if (testCombo == 1)
                        EmParameters.CalculationConcentration = null;
                    else if (testCombo == 2)
                        EmParameters.CalculationDiluent = null;
                    else if (testCombo == 3)
                        EmParameters.ValidFdFactorExists = false;
                    else
                        EmParameters.CalculationMoisture = null;

                    // Init Output
                    EmParameters.CalculatedUnadjustedValue = -123456789m;
                    category.CheckCatalogResult = null;

                    // Run Checks
                    actual = cMATSCalculatedHourlyValueChecks.MATSCHV15(category, ref log);

                    // Check Results
                    Assert.AreEqual(string.Empty, actual);
                    Assert.AreEqual(false, log);
                    Assert.AreEqual("C", category.CheckCatalogResult, "Result");
                    Assert.AreEqual(null, EmParameters.CalculatedUnadjustedValue, "CalculatedUnadjustedValue");
                }
            }

            //Result D
            {
                // Init Input
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(equationCd: "19-3D", modcCd: "01");
                EmParameters.CalculationConcentration = testValue;
                EmParameters.CalculationMoisture = testValue;
                EmParameters.CalculationDiluent = (decimal)20.9;
                EmParameters.ValidFdFactorExists = true;
                EmParameters.FinalConversionFactor = 1000000m;
                EmParameters.MatsDhvMeasuredModcList = "01,02";
                EmParameters.MatsDhvUnavailableModcList = "03";


                // Init Output
                EmParameters.CalculatedUnadjustedValue = -123456789m;
                category.CheckCatalogResult = null;


                // Run Checks
                actual = cMATSCalculatedHourlyValueChecks.MATSCHV15(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual("D", category.CheckCatalogResult, "Result");
                Assert.AreEqual(null, EmParameters.CalculatedUnadjustedValue, "CalculatedUnadjustedValue");
            }

            //Pass
            {
                // Init Input
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(equationCd: "19-3D", modcCd: "01");
                EmParameters.CalculationConcentration = testValue;
                EmParameters.CalculationDiluent = testValue;
                EmParameters.CalculationConversionFactor = testValue;
                EmParameters.CalculationMoisture = testValue;
                EmParameters.ValidFdFactorExists = true;
                EmParameters.FinalConversionFactor = 1000000m;
                EmParameters.MatsDhvMeasuredModcList = "01,02";
                EmParameters.MatsDhvUnavailableModcList = "03";
                EmParameters.CurrentHourlyOpRecord = new VwMpHrlyOpDataRow(fdFactor: testValue);

                decimal h2oFactor = (100 - testValue) / 100;
                decimal denom = ((decimal)20.9 * h2oFactor) - (testValue * h2oFactor);
                decimal formula = (testValue * testValue * testValue * ((Decimal)20.9 / denom)) * EmParameters.FinalConversionFactor.Value;

                // Init Output
                EmParameters.CalculatedUnadjustedValue = -123456789m;
                category.CheckCatalogResult = null;

                // Run Checks
                actual = cMATSCalculatedHourlyValueChecks.MATSCHV15(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual(formula, EmParameters.CalculatedUnadjustedValue, "CalculatedUnadjustedValue");
            }

            // Pass with unavailable MODC and existing equation code
            {
                // Init Input
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(equationCd: "19-3D", modcCd: "03");
                EmParameters.CalculationConcentrationSubstituted = false;
                EmParameters.CalculationDiluentSubstituted = false;
                EmParameters.FinalConversionFactor = 1000000m;
                EmParameters.MatsDhvMeasuredModcList = "01,02";
                EmParameters.MatsDhvUnavailableModcList = "03";


                // Init Output
                EmParameters.CalculatedUnadjustedValue = -123456789m;
                category.CheckCatalogResult = null;


                // Run Checks
                actual = cMATSCalculatedHourlyValueChecks.MATSCHV15(category, ref log);

                // Check Results
                string label = "Unavailable MODC with Equation";
                Assert.AreEqual(string.Empty, actual, string.Format("actual: [{0}]", label));
                Assert.AreEqual(false, log, string.Format("log: [{0}]", label));
                Assert.AreEqual(null, category.CheckCatalogResult, string.Format("result: [{0}]", label));
                Assert.AreEqual(null, EmParameters.CalculatedUnadjustedValue, "CalculatedUnadjustedValue");
            }

            // Pass with null Final Conversion Factor
            {
                // Init Input
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(equationCd: "19-3D", modcCd: "01");
                EmParameters.CalculationConcentration = testValue;
                EmParameters.CalculationDiluent = testValue;
                EmParameters.CalculationConversionFactor = testValue;
                EmParameters.CalculationMoisture = testValue;
                EmParameters.ValidFdFactorExists = true;
                EmParameters.FinalConversionFactor = null;
                EmParameters.MatsDhvMeasuredModcList = "01,02";
                EmParameters.MatsDhvUnavailableModcList = "03";
                EmParameters.CurrentHourlyOpRecord = new VwMpHrlyOpDataRow(fdFactor: testValue);

                decimal h2oFactor = (100 - testValue) / 100;
                decimal denom = ((decimal)20.9 * h2oFactor) - (testValue * h2oFactor);

                // Init Output
                EmParameters.CalculatedUnadjustedValue = -123456789m;
                category.CheckCatalogResult = null;

                // Run Checks
                actual = cMATSCalculatedHourlyValueChecks.MATSCHV15(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual(null, EmParameters.CalculatedUnadjustedValue, "CalculatedUnadjustedValue");
            }
        }

        [TestMethod()]
        public void MATSCHV15_Formula19_4()
        {
            //static check setup
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            // Variables
            bool log = false;
            string actual;
            decimal testValue = (decimal)0.01;

            //Result A
            {
                // Init Input
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(equationCd: "19-4", modcCd: "01");
                EmParameters.CalculationConcentrationSubstituted = false;
                EmParameters.CalculationDiluentSubstituted = false;
                EmParameters.CalculationMoistureSubstituted = false;
                EmParameters.FinalConversionFactor = 1000000m;
                EmParameters.MatsDhvMeasuredModcList = "01,02";
                EmParameters.MatsDhvUnavailableModcList = "03";

                int[] testComboList = { 1, 2, 3 };

                foreach (int testCombo in testComboList)
                {
                    if (testCombo == 1)
                        EmParameters.CalculationConcentrationSubstituted = true;
                    else if (testCombo == 2)
                        EmParameters.CalculationDiluentSubstituted = true;
                    else
                        EmParameters.CalculationMoistureSubstituted = true;

                    // Init Output
                    EmParameters.CalculatedUnadjustedValue = -123456789m;
                    category.CheckCatalogResult = null;


                    // Run Checks
                    actual = cMATSCalculatedHourlyValueChecks.MATSCHV15(category, ref log);

                    // Check Results
                    Assert.AreEqual(string.Empty, actual);
                    Assert.AreEqual(false, log);
                    Assert.AreEqual("A", category.CheckCatalogResult, "Result");
                    Assert.AreEqual(null, EmParameters.CalculatedUnadjustedValue, "CalculatedUnadjustedValue");
                }
            }

            //Result B
            {
                // Init Input
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(equationCd: null, modcCd: "01");
                EmParameters.CalculationConcentrationSubstituted = false;
                EmParameters.CalculationDiluentSubstituted = false;
                EmParameters.CalculationMoistureSubstituted = false;
                EmParameters.FinalConversionFactor = 1000000m;
                EmParameters.MatsDhvMeasuredModcList = "01,02";
                EmParameters.MatsDhvUnavailableModcList = "03";


                // Init Output
                EmParameters.CalculatedUnadjustedValue = -123456789m;
                category.CheckCatalogResult = null;


                // Run Checks
                actual = cMATSCalculatedHourlyValueChecks.MATSCHV15(category, ref log);

                // Check Results
                string label = "Result B";
                Assert.AreEqual(string.Empty, actual, string.Format("actual: [{0}]", label));
                Assert.AreEqual(false, log, string.Format("log: [{0}]", label));
                Assert.AreEqual("B", category.CheckCatalogResult, string.Format("result: [{0}]", label));
                Assert.AreEqual(null, EmParameters.CalculatedUnadjustedValue, "CalculatedUnadjustedValue");
            }

            //Result C
            {
                // Init Input
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(equationCd: "19-4", modcCd: "01");
                EmParameters.CalculationConcentration = testValue;
                EmParameters.CalculationDiluent = testValue;
                EmParameters.CalculationMoisture = testValue;
                EmParameters.ValidFdFactorExists = true;
                EmParameters.FinalConversionFactor = 1000000m;
                EmParameters.MatsDhvMeasuredModcList = "01,02";
                EmParameters.MatsDhvUnavailableModcList = "03";

                int[] testComboList = { 1, 2, 3, 4 };

                foreach (int testCombo in testComboList)
                {
                    if (testCombo == 1)
                        EmParameters.CalculationConcentration = null;
                    else if (testCombo == 2)
                        EmParameters.CalculationDiluent = null;
                    else if (testCombo == 3)
                        EmParameters.ValidFdFactorExists = false;
                    else
                        EmParameters.CalculationMoisture = null;

                    // Init Output
                    EmParameters.CalculatedUnadjustedValue = -123456789m;
                    category.CheckCatalogResult = null;

                    // Run Checks
                    actual = cMATSCalculatedHourlyValueChecks.MATSCHV15(category, ref log);

                    // Check Results
                    Assert.AreEqual(string.Empty, actual);
                    Assert.AreEqual(false, log);
                    Assert.AreEqual("C", category.CheckCatalogResult, "Result");
                    Assert.AreEqual(null, EmParameters.CalculatedUnadjustedValue, "CalculatedUnadjustedValue");
                }
            }

            //Result D
            {
                // Init Input
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(equationCd: "19-4", modcCd: "01");
                EmParameters.CalculationConcentration = testValue;
                EmParameters.CalculationMoisture = testValue;
                EmParameters.CalculationDiluent = testValue;
                EmParameters.ValidFdFactorExists = true;
                EmParameters.FinalConversionFactor = 1000000m;
                EmParameters.MatsDhvMeasuredModcList = "01,02";
                EmParameters.MatsDhvUnavailableModcList = "03";

                int[] testComboList = { 1, 2 };

                foreach (int testCombo in testComboList)
                {
                    if (testCombo == 1)
                        EmParameters.CalculationDiluent = (decimal)20.9;
                    else
                        EmParameters.CalculationMoisture = 100;

                    // Init Output
                    EmParameters.CalculatedUnadjustedValue = -123456789m;
                    category.CheckCatalogResult = null;


                    // Run Checks
                    actual = cMATSCalculatedHourlyValueChecks.MATSCHV15(category, ref log);

                    // Check Results
                    Assert.AreEqual(string.Empty, actual);
                    Assert.AreEqual(false, log);
                    Assert.AreEqual("D", category.CheckCatalogResult, "Result");
                    Assert.AreEqual(null, EmParameters.CalculatedUnadjustedValue, "CalculatedUnadjustedValue");
                }
            }

            //Pass
            {
                // Init Input
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(equationCd: "19-4", modcCd: "01");
                EmParameters.CalculationConcentration = testValue;
                EmParameters.CalculationDiluent = testValue;
                EmParameters.CalculationConversionFactor = testValue;
                EmParameters.CalculationMoisture = testValue;
                EmParameters.ValidFdFactorExists = true;
                EmParameters.FinalConversionFactor = 1000000m;
                EmParameters.MatsDhvMeasuredModcList = "01,02";
                EmParameters.MatsDhvUnavailableModcList = "03";
                EmParameters.CurrentHourlyOpRecord = new VwMpHrlyOpDataRow(fdFactor: testValue);

                decimal subformula1 = (testValue * testValue) / ((100 - testValue) / 100);
                decimal subformula2 = (decimal)20.9 / ((decimal)20.9 - testValue);
                decimal formula = (testValue * subformula1 * subformula2) * EmParameters.FinalConversionFactor.Value;

                // Init Output
                EmParameters.CalculatedUnadjustedValue = -123456789m;
                category.CheckCatalogResult = null;

                // Run Checks
                actual = cMATSCalculatedHourlyValueChecks.MATSCHV15(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual(formula, EmParameters.CalculatedUnadjustedValue, "CalculatedUnadjustedValue");
            }

            // Pass with unavailable MODC and existing equation code
            {
                // Init Input
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(equationCd: "19-4", modcCd: "03");
                EmParameters.CalculationConcentrationSubstituted = false;
                EmParameters.CalculationDiluentSubstituted = false;
                EmParameters.FinalConversionFactor = 1000000m;
                EmParameters.MatsDhvMeasuredModcList = "01,02";
                EmParameters.MatsDhvUnavailableModcList = "03";


                // Init Output
                EmParameters.CalculatedUnadjustedValue = -123456789m;
                category.CheckCatalogResult = null;


                // Run Checks
                actual = cMATSCalculatedHourlyValueChecks.MATSCHV15(category, ref log);

                // Check Results
                string label = "Unavailable MODC with Equation";
                Assert.AreEqual(string.Empty, actual, string.Format("actual: [{0}]", label));
                Assert.AreEqual(false, log, string.Format("log: [{0}]", label));
                Assert.AreEqual(null, category.CheckCatalogResult, string.Format("result: [{0}]", label));
                Assert.AreEqual(null, EmParameters.CalculatedUnadjustedValue, "CalculatedUnadjustedValue");
            }

            // Pass with null Final Conversion Factor
            {
                // Init Input
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(equationCd: "19-4", modcCd: "01");
                EmParameters.CalculationConcentration = testValue;
                EmParameters.CalculationDiluent = testValue;
                EmParameters.CalculationConversionFactor = testValue;
                EmParameters.CalculationMoisture = testValue;
                EmParameters.ValidFdFactorExists = true;
                EmParameters.FinalConversionFactor = null;
                EmParameters.MatsDhvMeasuredModcList = "01,02";
                EmParameters.MatsDhvUnavailableModcList = "03";
                EmParameters.CurrentHourlyOpRecord = new VwMpHrlyOpDataRow(fdFactor: testValue);

                decimal subformula1 = (testValue * testValue) / ((100 - testValue) / 100);
                decimal subformula2 = (decimal)20.9 / ((decimal)20.9 - testValue);

                // Init Output
                EmParameters.CalculatedUnadjustedValue = -123456789m;
                category.CheckCatalogResult = null;

                // Run Checks
                actual = cMATSCalculatedHourlyValueChecks.MATSCHV15(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual(null, EmParameters.CalculatedUnadjustedValue, "CalculatedUnadjustedValue");
            }
        }

        [TestMethod()]
        public void MATSCHV15_Formula19_5()
        {
            //static check setup
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            // Variables
            bool log = false;
            string actual;
            decimal testValue = (decimal)0.01;

            //Result A
            {
                // Init Input
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(equationCd: "19-5", modcCd: "01");
                EmParameters.CalculationConcentrationSubstituted = false;
                EmParameters.CalculationDiluentSubstituted = false;
                EmParameters.CalculationMoistureSubstituted = false;
                EmParameters.FinalConversionFactor = 1000000m;
                EmParameters.MatsDhvMeasuredModcList = "01,02";
                EmParameters.MatsDhvUnavailableModcList = "03";

                int[] testComboList = { 1, 2, 3 };

                foreach (int testCombo in testComboList)
                {
                    if (testCombo == 1)
                        EmParameters.CalculationConcentrationSubstituted = true;
                    else if (testCombo == 2)
                        EmParameters.CalculationDiluentSubstituted = true;
                    else
                        EmParameters.CalculationMoistureSubstituted = true;

                    // Init Output
                    EmParameters.CalculatedUnadjustedValue = -123456789m;
                    category.CheckCatalogResult = null;


                    // Run Checks
                    actual = cMATSCalculatedHourlyValueChecks.MATSCHV15(category, ref log);

                    // Check Results
                    Assert.AreEqual(string.Empty, actual);
                    Assert.AreEqual(false, log);
                    Assert.AreEqual("A", category.CheckCatalogResult, "Result");
                    Assert.AreEqual(null, EmParameters.CalculatedUnadjustedValue, "CalculatedUnadjustedValue");
                }
            }

            //Result B
            {
                // Init Input
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(equationCd: null, modcCd: "01");
                EmParameters.CalculationConcentrationSubstituted = false;
                EmParameters.CalculationDiluentSubstituted = false;
                EmParameters.CalculationMoistureSubstituted = false;
                EmParameters.FinalConversionFactor = 1000000m;
                EmParameters.MatsDhvMeasuredModcList = "01,02";
                EmParameters.MatsDhvUnavailableModcList = "03";


                // Init Output
                EmParameters.CalculatedUnadjustedValue = -123456789m;
                category.CheckCatalogResult = null;


                // Run Checks
                actual = cMATSCalculatedHourlyValueChecks.MATSCHV15(category, ref log);

                // Check Results
                string label = "Result B";
                Assert.AreEqual(string.Empty, actual, string.Format("actual: [{0}]", label));
                Assert.AreEqual(false, log, string.Format("log: [{0}]", label));
                Assert.AreEqual("B", category.CheckCatalogResult, string.Format("result: [{0}]", label));
                Assert.AreEqual(null, EmParameters.CalculatedUnadjustedValue, "CalculatedUnadjustedValue");
            }

            //Result C
            {
                // Init Input
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(equationCd: "19-5", modcCd: "01");
                EmParameters.CalculationConcentration = testValue;
                EmParameters.CalculationDiluent = testValue;
                EmParameters.CalculationMoisture = testValue;
                EmParameters.ValidFdFactorExists = true;
                EmParameters.FinalConversionFactor = 1000000m;
                EmParameters.MatsDhvMeasuredModcList = "01,02";
                EmParameters.MatsDhvUnavailableModcList = "03";

                int[] testComboList = { 1, 2, 3, 4 };

                foreach (int testCombo in testComboList)
                {
                    if (testCombo == 1)
                        EmParameters.CalculationConcentration = null;
                    else if (testCombo == 2)
                        EmParameters.CalculationDiluent = null;
                    else if (testCombo == 3)
                        EmParameters.ValidFdFactorExists = false;
                    else
                        EmParameters.CalculationMoisture = null;

                    // Init Output
                    EmParameters.CalculatedUnadjustedValue = -123456789m;
                    category.CheckCatalogResult = null;

                    // Run Checks
                    actual = cMATSCalculatedHourlyValueChecks.MATSCHV15(category, ref log);

                    // Check Results
                    Assert.AreEqual(string.Empty, actual);
                    Assert.AreEqual(false, log);
                    Assert.AreEqual("C", category.CheckCatalogResult, "Result");
                    Assert.AreEqual(null, EmParameters.CalculatedUnadjustedValue, "CalculatedUnadjustedValue");
                }
            }

            //Result D
            {
                // Init Input
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(equationCd: "19-5", modcCd: "01");
                EmParameters.CalculationConcentration = testValue;
                EmParameters.CalculationMoisture = testValue;
                EmParameters.CalculationDiluent = testValue;
                EmParameters.ValidFdFactorExists = true;
                EmParameters.FinalConversionFactor = 1000000m;
                EmParameters.MatsDhvMeasuredModcList = "01,02";
                EmParameters.MatsDhvUnavailableModcList = "03";

                int[] testComboList = { 1, 2 };

                foreach (int testCombo in testComboList)
                {
                    if (testCombo == 1)
                        EmParameters.CalculationDiluent = (decimal)20.9;
                    else
                        EmParameters.CalculationMoisture = 100;

                    // Init Output
                    EmParameters.CalculatedUnadjustedValue = -123456789m;
                    category.CheckCatalogResult = null;


                    // Run Checks
                    actual = cMATSCalculatedHourlyValueChecks.MATSCHV15(category, ref log);

                    // Check Results
                    Assert.AreEqual(string.Empty, actual);
                    Assert.AreEqual(false, log);
                    Assert.AreEqual("D", category.CheckCatalogResult, "Result");
                    Assert.AreEqual(null, EmParameters.CalculatedUnadjustedValue, "CalculatedUnadjustedValue");
                }
            }

            //Pass
            {
                // Init Input
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(equationCd: "19-5", modcCd: "01");
                EmParameters.CalculationConcentration = testValue;
                EmParameters.CalculationDiluent = testValue;
                EmParameters.CalculationConversionFactor = testValue;
                EmParameters.CalculationMoisture = testValue;
                EmParameters.ValidFdFactorExists = true;
                EmParameters.FinalConversionFactor = 1000000m;
                EmParameters.MatsDhvMeasuredModcList = "01,02";
                EmParameters.MatsDhvUnavailableModcList = "03";
                EmParameters.CurrentHourlyOpRecord = new VwMpHrlyOpDataRow(fdFactor: testValue);

                decimal h2oFactor = (100 - testValue) / 100;
                decimal denom = (decimal)20.9 - (testValue / h2oFactor);
                decimal formula = (((Decimal)20.9 * testValue * testValue * testValue) / denom) * EmParameters.FinalConversionFactor.Value;

                // Init Output
                EmParameters.CalculatedUnadjustedValue = -123456789m;
                category.CheckCatalogResult = null;

                // Run Checks
                actual = cMATSCalculatedHourlyValueChecks.MATSCHV15(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual(formula, EmParameters.CalculatedUnadjustedValue, "CalculatedUnadjustedValue");
            }

            // Pass with unavailable MODC and existing equation code
            {
                // Init Input
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(equationCd: "19-5", modcCd: "03");
                EmParameters.CalculationConcentrationSubstituted = false;
                EmParameters.CalculationDiluentSubstituted = false;
                EmParameters.FinalConversionFactor = 1000000m;
                EmParameters.MatsDhvMeasuredModcList = "01,02";
                EmParameters.MatsDhvUnavailableModcList = "03";


                // Init Output
                EmParameters.CalculatedUnadjustedValue = -123456789m;
                category.CheckCatalogResult = null;


                // Run Checks
                actual = cMATSCalculatedHourlyValueChecks.MATSCHV15(category, ref log);

                // Check Results
                string label = "Unavailable MODC with Equation";
                Assert.AreEqual(string.Empty, actual, string.Format("actual: [{0}]", label));
                Assert.AreEqual(false, log, string.Format("log: [{0}]", label));
                Assert.AreEqual(null, category.CheckCatalogResult, string.Format("result: [{0}]", label));
                Assert.AreEqual(null, EmParameters.CalculatedUnadjustedValue, "CalculatedUnadjustedValue");
            }

            // Pass with null Final Conversion Factor
            {
                // Init Input
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(equationCd: "19-5", modcCd: "01");
                EmParameters.CalculationConcentration = testValue;
                EmParameters.CalculationDiluent = testValue;
                EmParameters.CalculationConversionFactor = testValue;
                EmParameters.CalculationMoisture = testValue;
                EmParameters.ValidFdFactorExists = true;
                EmParameters.FinalConversionFactor = null;
                EmParameters.MatsDhvMeasuredModcList = "01,02";
                EmParameters.MatsDhvUnavailableModcList = "03";
                EmParameters.CurrentHourlyOpRecord = new VwMpHrlyOpDataRow(fdFactor: testValue);

                decimal h2oFactor = (100 - testValue) / 100;
                decimal denom = (decimal)20.9 - (testValue / h2oFactor);

                // Init Output
                EmParameters.CalculatedUnadjustedValue = -123456789m;
                category.CheckCatalogResult = null;

                // Run Checks
                actual = cMATSCalculatedHourlyValueChecks.MATSCHV15(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual(null, EmParameters.CalculatedUnadjustedValue, "CalculatedUnadjustedValue");
            }
        }


        [TestMethod()]
        public void MATSCHV15_Formula19_5D()
        {
            //static check setup
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            // Variables
            bool log = false;
            string actual;
            decimal testValue = (decimal)0.01;

            //Result A
            {
                // Init Input
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(equationCd: "19-5D", modcCd: "01");
                EmParameters.CalculationConcentrationSubstituted = false;
                EmParameters.CalculationDiluentSubstituted = false;
                EmParameters.FinalConversionFactor = 1000000m;
                EmParameters.MatsDhvMeasuredModcList = "01,02";
                EmParameters.MatsDhvUnavailableModcList = "03";

                int[] testComboList = { 1, 2 };

                foreach (int testCombo in testComboList)
                {
                    if (testCombo == 1)
                        EmParameters.CalculationConcentrationSubstituted = true;
                    else
                        EmParameters.CalculationDiluentSubstituted = true;

                    // Init Output
                    EmParameters.CalculatedUnadjustedValue = -123456789m;
                    category.CheckCatalogResult = null;


                    // Run Checks
                    actual = cMATSCalculatedHourlyValueChecks.MATSCHV15(category, ref log);

                    // Check Results
                    Assert.AreEqual(string.Empty, actual);
                    Assert.AreEqual(false, log);
                    Assert.AreEqual("A", category.CheckCatalogResult, "Result");
                    Assert.AreEqual(null, EmParameters.CalculatedUnadjustedValue, "CalculatedUnadjustedValue");
                }
            }

            //Result B
            {
                // Init Input
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(equationCd: null, modcCd: "01");
                EmParameters.CalculationConcentrationSubstituted = false;
                EmParameters.CalculationDiluentSubstituted = false;
                EmParameters.FinalConversionFactor = 1000000m;
                EmParameters.MatsDhvMeasuredModcList = "01,02";
                EmParameters.MatsDhvUnavailableModcList = "03";


                // Init Output
                EmParameters.CalculatedUnadjustedValue = -123456789m;
                category.CheckCatalogResult = null;


                // Run Checks
                actual = cMATSCalculatedHourlyValueChecks.MATSCHV15(category, ref log);

                // Check Results
                string label = "Result B";
                Assert.AreEqual(string.Empty, actual, string.Format("actual: [{0}]", label));
                Assert.AreEqual(false, log, string.Format("log: [{0}]", label));
                Assert.AreEqual("B", category.CheckCatalogResult, string.Format("result: [{0}]", label));
                Assert.AreEqual(null, EmParameters.CalculatedUnadjustedValue, "CalculatedUnadjustedValue");
            }

            //Result C
            {
                // Init Input
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(equationCd: "19-5D", modcCd: "01");
                EmParameters.CalculationConcentration = testValue;
                EmParameters.CalculationDiluent = testValue;
                EmParameters.ValidFdFactorExists = true;
                EmParameters.FinalConversionFactor = 1000000m;
                EmParameters.MatsDhvMeasuredModcList = "01,02";
                EmParameters.MatsDhvUnavailableModcList = "03";

                int[] testComboList = { 1, 2, 3 };

                foreach (int testCombo in testComboList)
                {
                    if (testCombo == 1)
                        EmParameters.CalculationConcentration = null;
                    else if (testCombo == 2)
                        EmParameters.CalculationDiluent = null;
                    else
                        EmParameters.ValidFdFactorExists = false;

                    // Init Output
                    EmParameters.CalculatedUnadjustedValue = -123456789m;
                    category.CheckCatalogResult = null;

                    // Run Checks
                    actual = cMATSCalculatedHourlyValueChecks.MATSCHV15(category, ref log);

                    // Check Results
                    Assert.AreEqual(string.Empty, actual);
                    Assert.AreEqual(false, log);
                    Assert.AreEqual("C", category.CheckCatalogResult, "Result");
                    Assert.AreEqual(null, EmParameters.CalculatedUnadjustedValue, "CalculatedUnadjustedValue");
                }
            }

            //Result D
            {
                // Init Input
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(equationCd: "19-5D", modcCd: "01");
                EmParameters.CalculationConcentration = testValue;
                EmParameters.CalculationDiluent = (decimal)20.9;
                EmParameters.ValidFdFactorExists = true;
                EmParameters.FinalConversionFactor = 1000000m;
                EmParameters.MatsDhvMeasuredModcList = "01,02";
                EmParameters.MatsDhvUnavailableModcList = "03";

                // Init Output
                EmParameters.CalculatedUnadjustedValue = -123456789m;
                category.CheckCatalogResult = null;


                // Run Checks
                actual = cMATSCalculatedHourlyValueChecks.MATSCHV15(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual("D", category.CheckCatalogResult, "Result");
                Assert.AreEqual(null, EmParameters.CalculatedUnadjustedValue, "CalculatedUnadjustedValue");

            }

            //Pass
            {
                // Init Input
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(equationCd: "19-5D", modcCd: "01");
                EmParameters.CalculationConcentration = testValue;
                EmParameters.CalculationDiluent = testValue;
                EmParameters.CalculationConversionFactor = testValue;
                EmParameters.ValidFdFactorExists = true;
                EmParameters.FinalConversionFactor = 1000000m;
                EmParameters.MatsDhvMeasuredModcList = "01,02";
                EmParameters.MatsDhvUnavailableModcList = "03";
                EmParameters.CurrentHourlyOpRecord = new VwMpHrlyOpDataRow(fdFactor: testValue);

                decimal formula = (testValue * testValue * testValue * ((Decimal)20.9 / ((Decimal)20.9 - testValue))) * EmParameters.FinalConversionFactor.Value;

                // Init Output
                EmParameters.CalculatedUnadjustedValue = -123456789m;
                category.CheckCatalogResult = null;

                // Run Checks
                actual = cMATSCalculatedHourlyValueChecks.MATSCHV15(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual(formula, EmParameters.CalculatedUnadjustedValue, "CalculatedUnadjustedValue");
            }

            // Pass with unavailable MODC and existing equation code
            {
                // Init Input
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(equationCd: "19-5D", modcCd: "03");
                EmParameters.CalculationConcentrationSubstituted = false;
                EmParameters.CalculationDiluentSubstituted = false;
                EmParameters.FinalConversionFactor = 1000000m;
                EmParameters.MatsDhvMeasuredModcList = "01,02";
                EmParameters.MatsDhvUnavailableModcList = "03";


                // Init Output
                EmParameters.CalculatedUnadjustedValue = -123456789m;
                category.CheckCatalogResult = null;


                // Run Checks
                actual = cMATSCalculatedHourlyValueChecks.MATSCHV15(category, ref log);

                // Check Results
                string label = "Unavailable MODC with Equation";
                Assert.AreEqual(string.Empty, actual, string.Format("actual: [{0}]", label));
                Assert.AreEqual(false, log, string.Format("log: [{0}]", label));
                Assert.AreEqual(null, category.CheckCatalogResult, string.Format("result: [{0}]", label));
                Assert.AreEqual(null, EmParameters.CalculatedUnadjustedValue, "CalculatedUnadjustedValue");
            }

            // Pass with null Final Conversion Factor
            {
                // Init Input
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(equationCd: "19-5D", modcCd: "01");
                EmParameters.CalculationConcentration = testValue;
                EmParameters.CalculationDiluent = testValue;
                EmParameters.CalculationConversionFactor = testValue;
                EmParameters.ValidFdFactorExists = true;
                EmParameters.FinalConversionFactor = null;
                EmParameters.MatsDhvMeasuredModcList = "01,02";
                EmParameters.MatsDhvUnavailableModcList = "03";
                EmParameters.CurrentHourlyOpRecord = new VwMpHrlyOpDataRow(fdFactor: testValue);

                // Init Output
                EmParameters.CalculatedUnadjustedValue = -123456789m;
                category.CheckCatalogResult = null;

                // Run Checks
                actual = cMATSCalculatedHourlyValueChecks.MATSCHV15(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual(null, EmParameters.CalculatedUnadjustedValue, "CalculatedUnadjustedValue");
            }
        }

        [TestMethod()]
        public void MATSCHV15_Formula19_6_7()
        {
            //static check setup
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            // Variables
            bool log = false;
            string actual;
            decimal testValue = (decimal)0.01;
            string[] testFormulaList = { "19-6", "19-7", "" };

            foreach (string testFormula in testFormulaList)
            {
                //Result A
                {
                    // Init Input
                    EmParameters.CurrentDhvRecordValid = true;
                    EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(equationCd: testFormula, modcCd: "01");
                    EmParameters.CalculationConcentrationSubstituted = false;
                    EmParameters.CalculationDiluentSubstituted = false;
                    EmParameters.FinalConversionFactor = 1000000m;
                    EmParameters.MatsDhvMeasuredModcList = "01,02";
                    EmParameters.MatsDhvUnavailableModcList = "03";

                    int[] testComboList = { 1, 2 };

                    foreach (int testCombo in testComboList)
                    {
                        if (testCombo == 1)
                            EmParameters.CalculationConcentrationSubstituted = true;
                        else
                            EmParameters.CalculationDiluentSubstituted = true;

                        // Init Output
                        EmParameters.CalculatedUnadjustedValue = -123456789m;
                        category.CheckCatalogResult = null;


                        // Run Checks
                        actual = cMATSCalculatedHourlyValueChecks.MATSCHV15(category, ref log);

                        // Check Results
                        Assert.AreEqual(string.Empty, actual);
                        Assert.AreEqual(false, log);
                        Assert.AreEqual("A", category.CheckCatalogResult, "Result");
                        Assert.AreEqual(null, EmParameters.CalculatedUnadjustedValue, "CalculatedUnadjustedValue");
                    }
                }

                //Result B
                {
                    // Init Input
                    EmParameters.CurrentDhvRecordValid = true;
                    EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(equationCd: null, modcCd: "01");
                    EmParameters.CalculationConcentrationSubstituted = false;
                    EmParameters.CalculationDiluentSubstituted = false;
                    EmParameters.FinalConversionFactor = 1000000m;
                    EmParameters.MatsDhvMeasuredModcList = "01,02";
                    EmParameters.MatsDhvUnavailableModcList = "03";


                    // Init Output
                    EmParameters.CalculatedUnadjustedValue = -123456789m;
                    category.CheckCatalogResult = null;


                    // Run Checks
                    actual = cMATSCalculatedHourlyValueChecks.MATSCHV15(category, ref log);

                    // Check Results
                    string label = string.Format("Result B for {0} Equation", testFormula);
                    Assert.AreEqual(string.Empty, actual, string.Format("actual: [{0}]", label));
                    Assert.AreEqual(false, log, string.Format("log: [{0}]", label));
                    Assert.AreEqual("B", category.CheckCatalogResult, string.Format("result: [{0}]", label));
                    Assert.AreEqual(null, EmParameters.CalculatedUnadjustedValue, "CalculatedUnadjustedValue");
                }

                //Result C
                {
                    // Init Input
                    EmParameters.CurrentDhvRecordValid = true;
                    EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(equationCd: testFormula, modcCd: "01");
                    EmParameters.CalculationConcentration = testValue;
                    EmParameters.CalculationDiluent = testValue;
                    EmParameters.ValidFcFactorExists = true;
                    EmParameters.FinalConversionFactor = 1000000m;
                    EmParameters.MatsDhvMeasuredModcList = "01,02";
                    EmParameters.MatsDhvUnavailableModcList = "03";

                    int[] testComboList = { 1, 2, 3 };

                    foreach (int testCombo in testComboList)
                    {
                        if (testCombo == 1)
                            EmParameters.CalculationConcentration = null;
                        else if (testCombo == 2)
                            EmParameters.CalculationDiluent = null;
                        else
                            EmParameters.ValidFcFactorExists = false;

                        // Init Output
                        EmParameters.CalculatedUnadjustedValue = -123456789m;
                        category.CheckCatalogResult = null;

                        // Run Checks
                        actual = cMATSCalculatedHourlyValueChecks.MATSCHV15(category, ref log);

                        // Check Results
                        Assert.AreEqual(string.Empty, actual);
                        Assert.AreEqual(false, log);
                        Assert.AreEqual("C", category.CheckCatalogResult, "Result");
                        Assert.AreEqual(null, EmParameters.CalculatedUnadjustedValue, "CalculatedUnadjustedValue");
                    }
                }

                //Result D
                {
                    // Init Input
                    EmParameters.CurrentDhvRecordValid = true;
                    EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(equationCd: testFormula, modcCd: "01");
                    EmParameters.CalculationConcentration = testValue;
                    EmParameters.CalculationDiluent = (decimal)0.0;
                    EmParameters.ValidFcFactorExists = true;
                    EmParameters.FinalConversionFactor = 1000000m;
                    EmParameters.MatsDhvMeasuredModcList = "01,02";
                    EmParameters.MatsDhvUnavailableModcList = "03";

                    // Init Output
                    EmParameters.CalculatedUnadjustedValue = -123456789m;
                    category.CheckCatalogResult = null;


                    // Run Checks
                    actual = cMATSCalculatedHourlyValueChecks.MATSCHV15(category, ref log);

                    // Check Results
                    Assert.AreEqual(string.Empty, actual);
                    Assert.AreEqual(false, log);
                    Assert.AreEqual("D", category.CheckCatalogResult, "Result");

                }

                //Pass
                {
                    // Init Input
                    EmParameters.CurrentDhvRecordValid = true;
                    EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(equationCd: testFormula, modcCd: "01");
                    EmParameters.CalculationConcentration = testValue;
                    EmParameters.CalculationDiluent = testValue;
                    EmParameters.CalculationConversionFactor = testValue;
                    EmParameters.ValidFcFactorExists = true;
                    EmParameters.FinalConversionFactor = 1000000m;
                    EmParameters.MatsDhvMeasuredModcList = "01,02";
                    EmParameters.MatsDhvUnavailableModcList = "03";
                    EmParameters.CurrentHourlyOpRecord = new VwMpHrlyOpDataRow(fcFactor: testValue);

                    decimal formula = (testValue * testValue * testValue * (100m / testValue)) * EmParameters.FinalConversionFactor.Value;

                    // Init Output
                    EmParameters.CalculatedUnadjustedValue = -123456789m;
                    category.CheckCatalogResult = null;

                    // Run Checks
                    actual = cMATSCalculatedHourlyValueChecks.MATSCHV15(category, ref log);

                    // Check Results
                    Assert.AreEqual(string.Empty, actual);
                    Assert.AreEqual(false, log);
                    Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                    Assert.AreEqual(formula, EmParameters.CalculatedUnadjustedValue, "CalculatedUnadjustedValue");
                }

                // Pass with unavailable MODC and existing equation code
                {
                    // Init Input
                    EmParameters.CurrentDhvRecordValid = true;
                    EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(equationCd: testFormula, modcCd: "03");
                    EmParameters.CalculationConcentrationSubstituted = false;
                    EmParameters.CalculationDiluentSubstituted = false;
                    EmParameters.FinalConversionFactor = 1000000m;
                    EmParameters.MatsDhvMeasuredModcList = "01,02";
                    EmParameters.MatsDhvUnavailableModcList = "03";


                    // Init Output
                    EmParameters.CalculatedUnadjustedValue = -123456789m;
                    category.CheckCatalogResult = null;


                    // Run Checks
                    actual = cMATSCalculatedHourlyValueChecks.MATSCHV15(category, ref log);

                    // Check Results
                    string label = string.Format("Unavailable MODC with {0} Equation", testFormula);
                    Assert.AreEqual(string.Empty, actual, string.Format("actual: [{0}]", label));
                    Assert.AreEqual(false, log, string.Format("log: [{0}]", label));
                    Assert.AreEqual(null, category.CheckCatalogResult, string.Format("result: [{0}]", label));
                    Assert.AreEqual(null, EmParameters.CalculatedUnadjustedValue, "CalculatedUnadjustedValue");
                }

                // Pass with null Final Conversion Factor
                {
                    // Init Input
                    EmParameters.CurrentDhvRecordValid = true;
                    EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(equationCd: testFormula, modcCd: "01");
                    EmParameters.CalculationConcentration = testValue;
                    EmParameters.CalculationDiluent = testValue;
                    EmParameters.CalculationConversionFactor = testValue;
                    EmParameters.ValidFcFactorExists = true;
                    EmParameters.FinalConversionFactor = null;
                    EmParameters.MatsDhvMeasuredModcList = "01,02";
                    EmParameters.MatsDhvUnavailableModcList = "03";
                    EmParameters.CurrentHourlyOpRecord = new VwMpHrlyOpDataRow(fcFactor: testValue);

                    // Init Output
                    EmParameters.CalculatedUnadjustedValue = -123456789m;
                    category.CheckCatalogResult = null;

                    // Run Checks
                    actual = cMATSCalculatedHourlyValueChecks.MATSCHV15(category, ref log);

                    // Check Results
                    Assert.AreEqual(string.Empty, actual);
                    Assert.AreEqual(false, log);
                    Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                    Assert.AreEqual(null, EmParameters.CalculatedUnadjustedValue, "CalculatedUnadjustedValue");
                }
            }
        }

        [TestMethod()]
        public void MATSCHV15_Formula19_8()
        {
            //static check setup
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            // Variables
            bool log = false;
            string actual;
            decimal testValue = (decimal)0.01;

            //Result A
            {
                // Init Input
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(equationCd: "19-8", modcCd: "01");
                EmParameters.CalculationConcentrationSubstituted = false;
                EmParameters.CalculationDiluentSubstituted = false;
                EmParameters.CalculationMoistureSubstituted = false;
                EmParameters.FinalConversionFactor = 1000000m;
                EmParameters.MatsDhvMeasuredModcList = "01,02";
                EmParameters.MatsDhvUnavailableModcList = "03";

                int[] testComboList = { 1, 2, 3 };

                foreach (int testCombo in testComboList)
                {
                    if (testCombo == 1)
                        EmParameters.CalculationConcentrationSubstituted = true;
                    else if (testCombo == 2)
                        EmParameters.CalculationDiluentSubstituted = true;
                    else
                        EmParameters.CalculationMoistureSubstituted = true;

                    // Init Output
                    EmParameters.CalculatedUnadjustedValue = -123456789m;
                    category.CheckCatalogResult = null;


                    // Run Checks
                    actual = cMATSCalculatedHourlyValueChecks.MATSCHV15(category, ref log);

                    // Check Results
                    Assert.AreEqual(string.Empty, actual);
                    Assert.AreEqual(false, log);
                    Assert.AreEqual("A", category.CheckCatalogResult, "Result");
                    Assert.AreEqual(null, EmParameters.CalculatedUnadjustedValue, "CalculatedUnadjustedValue");
                }
            }

            //Result B
            {
                // Init Input
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(equationCd: null, modcCd: "01");
                EmParameters.CalculationConcentrationSubstituted = false;
                EmParameters.CalculationDiluentSubstituted = false;
                EmParameters.CalculationMoistureSubstituted = false;
                EmParameters.FinalConversionFactor = 1000000m;
                EmParameters.MatsDhvMeasuredModcList = "01,02";
                EmParameters.MatsDhvUnavailableModcList = "03";


                // Init Output
                EmParameters.CalculatedUnadjustedValue = -123456789m;
                category.CheckCatalogResult = null;


                // Run Checks
                actual = cMATSCalculatedHourlyValueChecks.MATSCHV15(category, ref log);

                // Check Results
                string label = "Result B";
                Assert.AreEqual(string.Empty, actual, string.Format("actual: [{0}]", label));
                Assert.AreEqual(false, log, string.Format("log: [{0}]", label));
                Assert.AreEqual("B", category.CheckCatalogResult, string.Format("result: [{0}]", label));
                Assert.AreEqual(null, EmParameters.CalculatedUnadjustedValue, "CalculatedUnadjustedValue");
            }

            //Result C
            {
                // Init Input
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(equationCd: "19-8", modcCd: "01");
                EmParameters.CalculationConcentration = testValue;
                EmParameters.CalculationDiluent = testValue;
                EmParameters.CalculationMoisture = testValue;
                EmParameters.ValidFcFactorExists = true;
                EmParameters.FinalConversionFactor = 1000000m;
                EmParameters.MatsDhvMeasuredModcList = "01,02";
                EmParameters.MatsDhvUnavailableModcList = "03";

                int[] testComboList = { 1, 2, 3, 4 };

                foreach (int testCombo in testComboList)
                {
                    if (testCombo == 1)
                        EmParameters.CalculationConcentration = null;
                    else if (testCombo == 2)
                        EmParameters.CalculationDiluent = null;
                    else if (testCombo == 3)
                        EmParameters.ValidFcFactorExists = false;
                    else
                        EmParameters.CalculationMoisture = null;

                    // Init Output
                    EmParameters.CalculatedUnadjustedValue = -123456789m;
                    category.CheckCatalogResult = null;

                    // Run Checks
                    actual = cMATSCalculatedHourlyValueChecks.MATSCHV15(category, ref log);

                    // Check Results
                    Assert.AreEqual(string.Empty, actual);
                    Assert.AreEqual(false, log);
                    Assert.AreEqual("C", category.CheckCatalogResult, "Result");
                    Assert.AreEqual(null, EmParameters.CalculatedUnadjustedValue, "CalculatedUnadjustedValue");
                }
            }

            //Result D
            {
                // Init Input
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(equationCd: "19-8", modcCd: "01");
                EmParameters.CalculationConcentration = testValue;
                EmParameters.CalculationMoisture = testValue;
                EmParameters.ValidFcFactorExists = true;
                EmParameters.FinalConversionFactor = 1000000m;
                EmParameters.MatsDhvMeasuredModcList = "01,02";
                EmParameters.MatsDhvUnavailableModcList = "03";

                int[] testComboList = { 1, 2 };

                foreach (int testCombo in testComboList)
                {
                    if (testCombo == 1)
                        EmParameters.CalculationDiluent = (decimal)0.0;
                    else
                        EmParameters.CalculationMoisture = 100;

                    // Init Output
                    EmParameters.CalculatedUnadjustedValue = -123456789m;
                    category.CheckCatalogResult = null;


                    // Run Checks
                    actual = cMATSCalculatedHourlyValueChecks.MATSCHV15(category, ref log);

                    // Check Results
                    Assert.AreEqual(string.Empty, actual);
                    Assert.AreEqual(false, log);
                    Assert.AreEqual("D", category.CheckCatalogResult, "Result");
                    Assert.AreEqual(null, EmParameters.CalculatedUnadjustedValue, "CalculatedUnadjustedValue");
                }
            }

            //Pass
            {
                // Init Input
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(equationCd: "19-8", modcCd: "01");
                EmParameters.CalculationConcentration = testValue;
                EmParameters.CalculationDiluent = testValue;
                EmParameters.CalculationConversionFactor = testValue;
                EmParameters.CalculationMoisture = testValue;
                EmParameters.ValidFcFactorExists = true;
                EmParameters.FinalConversionFactor = 1000000m;
                EmParameters.MatsDhvMeasuredModcList = "01,02";
                EmParameters.MatsDhvUnavailableModcList = "03";
                EmParameters.CurrentHourlyOpRecord = new VwMpHrlyOpDataRow(fcFactor: testValue);

                decimal formula = (testValue * ((testValue * testValue) / (((Decimal)100 - testValue) / 100)) * (100 / testValue)) * EmParameters.FinalConversionFactor.Value;

                // Init Output
                EmParameters.CalculatedUnadjustedValue = -123456789m;
                category.CheckCatalogResult = null;

                // Run Checks
                actual = cMATSCalculatedHourlyValueChecks.MATSCHV15(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual(formula, EmParameters.CalculatedUnadjustedValue, "CalculatedUnadjustedValue");
            }

            // Pass with unavailable MODC and existing equation code
            {
                // Init Input
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(equationCd: "19-8", modcCd: "03");
                EmParameters.CalculationConcentrationSubstituted = false;
                EmParameters.CalculationDiluentSubstituted = false;
                EmParameters.CalculationMoistureSubstituted = false;
                EmParameters.FinalConversionFactor = 1000000m;
                EmParameters.MatsDhvMeasuredModcList = "01,02";
                EmParameters.MatsDhvUnavailableModcList = "03";


                // Init Output
                EmParameters.CalculatedUnadjustedValue = -123456789m;
                category.CheckCatalogResult = null;


                // Run Checks
                actual = cMATSCalculatedHourlyValueChecks.MATSCHV15(category, ref log);

                // Check Results
                string label = "Unavailable MODC with Equation";
                Assert.AreEqual(string.Empty, actual, string.Format("actual: [{0}]", label));
                Assert.AreEqual(false, log, string.Format("log: [{0}]", label));
                Assert.AreEqual(null, category.CheckCatalogResult, string.Format("result: [{0}]", label));
                Assert.AreEqual(null, EmParameters.CalculatedUnadjustedValue, "CalculatedUnadjustedValue");
            }

            // Pass with null Final Conversion Factor
            {
                // Init Input
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(equationCd: "19-8", modcCd: "01");
                EmParameters.CalculationConcentration = testValue;
                EmParameters.CalculationDiluent = testValue;
                EmParameters.CalculationConversionFactor = testValue;
                EmParameters.CalculationMoisture = testValue;
                EmParameters.ValidFcFactorExists = true;
                EmParameters.FinalConversionFactor = null;
                EmParameters.MatsDhvMeasuredModcList = "01,02";
                EmParameters.MatsDhvUnavailableModcList = "03";
                EmParameters.CurrentHourlyOpRecord = new VwMpHrlyOpDataRow(fcFactor: testValue);

                // Init Output
                EmParameters.CalculatedUnadjustedValue = -123456789m;
                category.CheckCatalogResult = null;

                // Run Checks
                actual = cMATSCalculatedHourlyValueChecks.MATSCHV15(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual(null, EmParameters.CalculatedUnadjustedValue, "CalculatedUnadjustedValue");
            }
        }

        [TestMethod()]
        public void MATSCHV15_Formula19_9()
        {
            //static check setup
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            // Variables
            bool log = false;
            string actual;
            decimal testValue = (decimal)0.01;

            //Result A
            {
                // Init Input
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(equationCd: "19-9", modcCd: "01");
                EmParameters.CalculationConcentrationSubstituted = false;
                EmParameters.CalculationDiluentSubstituted = false;
                EmParameters.CalculationMoistureSubstituted = false;
                EmParameters.FinalConversionFactor = 1000000m;
                EmParameters.MatsDhvMeasuredModcList = "01,02";
                EmParameters.MatsDhvUnavailableModcList = "03";

                int[] testComboList = { 1, 2, 3 };

                foreach (int testCombo in testComboList)
                {
                    if (testCombo == 1)
                        EmParameters.CalculationConcentrationSubstituted = true;
                    else if (testCombo == 2)
                        EmParameters.CalculationDiluentSubstituted = true;
                    else
                        EmParameters.CalculationMoistureSubstituted = true;

                    // Init Output
                    EmParameters.CalculatedUnadjustedValue = -123456789m;
                    category.CheckCatalogResult = null;


                    // Run Checks
                    actual = cMATSCalculatedHourlyValueChecks.MATSCHV15(category, ref log);

                    // Check Results
                    Assert.AreEqual(string.Empty, actual);
                    Assert.AreEqual(false, log);
                    Assert.AreEqual("A", category.CheckCatalogResult, "Result");
                    Assert.AreEqual(null, EmParameters.CalculatedUnadjustedValue, "CalculatedUnadjustedValue");
                }
            }

            //Result B
            {
                // Init Input
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(equationCd: null, modcCd: "01");
                EmParameters.CalculationConcentrationSubstituted = false;
                EmParameters.CalculationDiluentSubstituted = false;
                EmParameters.CalculationMoistureSubstituted = false;
                EmParameters.FinalConversionFactor = 1000000m;
                EmParameters.MatsDhvMeasuredModcList = "01,02";
                EmParameters.MatsDhvUnavailableModcList = "03";


                // Init Output
                EmParameters.CalculatedUnadjustedValue = -123456789m;
                category.CheckCatalogResult = null;


                // Run Checks
                actual = cMATSCalculatedHourlyValueChecks.MATSCHV15(category, ref log);

                // Check Results
                string label = "Result B";
                Assert.AreEqual(string.Empty, actual, string.Format("actual: [{0}]", label));
                Assert.AreEqual(false, log, string.Format("log: [{0}]", label));
                Assert.AreEqual("B", category.CheckCatalogResult, string.Format("result: [{0}]", label));
                Assert.AreEqual(null, EmParameters.CalculatedUnadjustedValue, "CalculatedUnadjustedValue");
            }

            //Result C
            {
                // Init Input
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(equationCd: "19-9", modcCd: "01");
                EmParameters.CalculationConcentration = testValue;
                EmParameters.CalculationDiluent = testValue;
                EmParameters.CalculationMoisture = testValue;
                EmParameters.ValidFcFactorExists = true;
                EmParameters.FinalConversionFactor = 1000000m;
                EmParameters.MatsDhvMeasuredModcList = "01,02";
                EmParameters.MatsDhvUnavailableModcList = "03";

                int[] testComboList = { 1, 2, 3, 4 };

                foreach (int testCombo in testComboList)
                {
                    if (testCombo == 1)
                        EmParameters.CalculationConcentration = null;
                    else if (testCombo == 2)
                        EmParameters.CalculationDiluent = null;
                    else if (testCombo == 3)
                        EmParameters.ValidFcFactorExists = false;
                    else
                        EmParameters.CalculationMoisture = null;

                    // Init Output
                    EmParameters.CalculatedUnadjustedValue = -123456789m;
                    category.CheckCatalogResult = null;

                    // Run Checks
                    actual = cMATSCalculatedHourlyValueChecks.MATSCHV15(category, ref log);

                    // Check Results
                    Assert.AreEqual(string.Empty, actual);
                    Assert.AreEqual(false, log);
                    Assert.AreEqual("C", category.CheckCatalogResult, "Result");
                    Assert.AreEqual(null, EmParameters.CalculatedUnadjustedValue, "CalculatedUnadjustedValue");
                }
            }

            //Result D
            {
                // Init Input
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(equationCd: "19-9", modcCd: "01");
                EmParameters.CalculationConcentration = testValue;
                EmParameters.CalculationMoisture = testValue;
                EmParameters.ValidFcFactorExists = true;
                EmParameters.FinalConversionFactor = 1000000m;
                EmParameters.MatsDhvMeasuredModcList = "01,02";
                EmParameters.MatsDhvUnavailableModcList = "03";
                EmParameters.CalculationDiluent = (decimal)0.0;

                // Init Output
                EmParameters.CalculatedUnadjustedValue = -123456789m;
                category.CheckCatalogResult = null;


                // Run Checks
                actual = cMATSCalculatedHourlyValueChecks.MATSCHV15(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual("D", category.CheckCatalogResult, "Result");
                Assert.AreEqual(null, EmParameters.CalculatedUnadjustedValue, "CalculatedUnadjustedValue");

            }

            //Pass
            {
                // Init Input
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(equationCd: "19-9", modcCd: "01");
                EmParameters.CalculationConcentration = testValue;
                EmParameters.CalculationDiluent = testValue;
                EmParameters.CalculationConversionFactor = testValue;
                EmParameters.CalculationMoisture = testValue;
                EmParameters.ValidFcFactorExists = true;
                EmParameters.FinalConversionFactor = 1000000m;
                EmParameters.MatsDhvMeasuredModcList = "01,02";
                EmParameters.MatsDhvUnavailableModcList = "03";
                EmParameters.CurrentHourlyOpRecord = new VwMpHrlyOpDataRow(fcFactor: testValue);

                decimal formula = (testValue * testValue * (((decimal)100 - testValue) / 100) * testValue * (100 / testValue)) * EmParameters.FinalConversionFactor.Value;

                // Init Output
                EmParameters.CalculatedUnadjustedValue = -123456789m;
                category.CheckCatalogResult = null;

                // Run Checks
                actual = cMATSCalculatedHourlyValueChecks.MATSCHV15(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual(formula, EmParameters.CalculatedUnadjustedValue, "CalculatedUnadjustedValue");
            }

            // Pass with unavailable MODC and existing equation code
            {
                // Init Input
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(equationCd: "19-9", modcCd: "03");
                EmParameters.CalculationConcentrationSubstituted = false;
                EmParameters.CalculationDiluentSubstituted = false;
                EmParameters.CalculationMoistureSubstituted = false;
                EmParameters.FinalConversionFactor = 1000000m;
                EmParameters.MatsDhvMeasuredModcList = "01,02";
                EmParameters.MatsDhvUnavailableModcList = "03";


                // Init Output
                EmParameters.CalculatedUnadjustedValue = -123456789m;
                category.CheckCatalogResult = null;


                // Run Checks
                actual = cMATSCalculatedHourlyValueChecks.MATSCHV15(category, ref log);

                // Check Results
                string label = "Unavailable MODC with Equation";
                Assert.AreEqual(string.Empty, actual, string.Format("actual: [{0}]", label));
                Assert.AreEqual(false, log, string.Format("log: [{0}]", label));
                Assert.AreEqual(null, category.CheckCatalogResult, string.Format("result: [{0}]", label));
                Assert.AreEqual(null, EmParameters.CalculatedUnadjustedValue, "CalculatedUnadjustedValue");
            }

            // Pass with null Final Conversion Factor
            {
                // Init Input
                EmParameters.CurrentDhvRecordValid = true;
                EmParameters.MatsDhvRecord = new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSDerivedHourlyValueData(equationCd: "19-9", modcCd: "01");
                EmParameters.CalculationConcentration = testValue;
                EmParameters.CalculationDiluent = testValue;
                EmParameters.CalculationConversionFactor = testValue;
                EmParameters.CalculationMoisture = testValue;
                EmParameters.ValidFcFactorExists = true;
                EmParameters.FinalConversionFactor = null;
                EmParameters.MatsDhvMeasuredModcList = "01,02";
                EmParameters.MatsDhvUnavailableModcList = "03";
                EmParameters.CurrentHourlyOpRecord = new VwMpHrlyOpDataRow(fcFactor: testValue);

                // Init Output
                EmParameters.CalculatedUnadjustedValue = -123456789m;
                category.CheckCatalogResult = null;

                // Run Checks
                actual = cMATSCalculatedHourlyValueChecks.MATSCHV15(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual(null, EmParameters.CalculatedUnadjustedValue, "CalculatedUnadjustedValue");
            }
        }

        [TestMethod()]
        public void MATSCHV15_NotValid()
        {
            //static check setup
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            // Variables
            bool log = false;
            string actual;

            // Init Input
            EmParameters.CurrentDhvRecordValid = false;

            // Init Output
            category.CheckCatalogResult = null;

            // Run Checks
            actual = cMATSCalculatedHourlyValueChecks.MATSCHV15(category, ref log);

            // Check Results
            Assert.AreEqual(string.Empty, actual);
            Assert.AreEqual(false, log);
            Assert.AreEqual(null, category.CheckCatalogResult, "Result");
        }

        #endregion

        #region MATSCHV-16

        /// <summary>
        ///A test for MATSCHV-16
        ///</summary>()
        [TestMethod()]
        public void MATSCHV16()
        {
            //static check setup
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            // Variables
            bool log = false;
            string actual;

            // Init Input
            EmParameters.CalculatedUnadjustedValue = (decimal)0.01;

            // Init Output
            category.CheckCatalogResult = null;

            // Run Checks
            actual = cMATSCalculatedHourlyValueChecks.MATSCHV16(category, ref log);

            // Check Results
            Assert.AreEqual(string.Empty, actual);
            Assert.AreEqual(false, log);
            Assert.AreEqual(null, category.CheckCatalogResult, "Result");
            Assert.AreEqual("1.00E-2", EmParameters.MatsCalculatedHgRateValue, "MatsCalculatedHgRateValue");
        }
        #endregion

        #region MATSCHV-17

        /// <summary>
        ///A test for MATSCHV-17
        ///</summary>()
        [TestMethod()]
        public void MATSCHV17()
        {
            //static check setup
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            // Variables
            bool log = false;
            string actual;

            // Init Input
            EmParameters.CalculatedUnadjustedValue = (decimal)0.01;

            // Init Output
            category.CheckCatalogResult = null;

            // Run Checks
            actual = cMATSCalculatedHourlyValueChecks.MATSCHV17(category, ref log);

            // Check Results
            Assert.AreEqual(string.Empty, actual);
            Assert.AreEqual(false, log);
            Assert.AreEqual(null, category.CheckCatalogResult, "Result");
            Assert.AreEqual("1.00E-2", EmParameters.MatsCalculatedHclRateValue, "MatsCalculatedHclRateValue");
        }
        #endregion

        #region MATSCHV-18

        /// <summary>
        ///A test for MATSCHV-18
        ///</summary>()
        [TestMethod()]
        public void MATSCHV18()
        {
            //static check setup
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            // Variables
            bool log = false;
            string actual;

            // Init Input
            EmParameters.CalculatedUnadjustedValue = (decimal)0.01;

            // Init Output
            category.CheckCatalogResult = null;

            // Run Checks
            actual = cMATSCalculatedHourlyValueChecks.MATSCHV18(category, ref log);

            // Check Results
            Assert.AreEqual(string.Empty, actual);
            Assert.AreEqual(false, log);
            Assert.AreEqual(null, category.CheckCatalogResult, "Result");
            Assert.AreEqual("1.00E-2", EmParameters.MatsCalculatedHfRateValue, "MatsCalculatedHfRateValue");

        }
        #endregion

        #region MATSCHV-19

        /// <summary>
        ///A test for MATSCHV-19
        ///</summary>()
        [TestMethod()]
        public void MATSCHV19()
        {
            //static check setup
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            // Variables
            bool log = false;
            string actual;

            // Init Input
            EmParameters.CalculatedUnadjustedValue = (decimal)0.01;

            // Init Output
            category.CheckCatalogResult = null;

            // Run Checks
            actual = cMATSCalculatedHourlyValueChecks.MATSCHV19(category, ref log);

            // Check Results
            Assert.AreEqual(string.Empty, actual);
            Assert.AreEqual(false, log);
            Assert.AreEqual(null, category.CheckCatalogResult, "Result");
            Assert.AreEqual("1.00E-2", EmParameters.MatsCalculatedSo2RateValue, "MatsCalculatedSo2RateValue");
        }
        #endregion


        /// <summary>
        /// 
        /// MatsDhvMeasuredModcList: 36, 37, 39
        /// 
        ///                                             | CurrSo2Mhv | MatsDhv   ||
        /// | ## | Valid | Calculated | Modc | Reported | SO2ParamCd | SO2ModcCd || Result || Notes
        /// |  0 | false |       8.40 | 36   |     7.98 |       null |      null || null   || Would fail tolerance but DHV record is not values
        /// |  1 | true  |       8.40 | 36   |     7.98 |       null |      null || A      || Failed tolerance (5.128% => 5.1%) with MODC 36
        /// |  2 | true  |       8.40 | 37   |     7.98 |      SO2RE |        37 || A      || Failed tolerance (5.128% => 5.1%) with MODC 37
        /// |  3 | true  |       8.40 | 38   |     7.98 |      SO2RE |        38 || null   || Would fail tolerance but, but MODC 38 prevents checking.  Reported value is nornally null.
        /// |  4 | true  |       8.40 | 39   |     7.98 |      SO2RH |        39 || A      || Failed tolerance (5.128% => 5.1%) with MODC 39
        /// |  5 | true  |       null | 36   |     7.98 |      SO2RH |        36 || null   || Tolerance check does not occur becuase the calculated value is null.
        /// |  6 | true  |       8.40 | 36   |     null |       null |      null || null   || Tolerance check does not occur becuase the reported value is null.
        /// |  7 | true  |       0.00 | 36   |     0.00 |       null |      null || null   || Tolerance check does not occur becuase the sum of the reported and calculated values equals zero.
        /// |  8 | true  |       8.40 | 36   |     7.99 |       null |      null || null   || Passed tolerance (5.003% => 5.0%) with calculated value higher than reported.
        /// |  9 | true  |       7.99 | 36   |     8.40 |       null |      null || null   || Passed tolerance (5.003% => 5.0%) with reported value higher than calculated.
        /// | 10 | false |       8.40 | 36   |     7.98 |       null |      null || null   || Would fail tolerance but DHV record is not values
        /// | 11 | true  |       8.40 | 36   |     7.98 |       null |      null || A      || Failed tolerance (5.128% => 5.1%) with MODC 36
        /// | 12 | true  |       8.40 | 37   |     7.98 |      SO2RE |        36 || null   || Would fail tolerance but, but MODC 16 with SO2RE prevents checking.  Reported value is nornally null.
        /// | 13 | true  |       8.40 | 38   |     7.98 |      SO2RH |        36 || null   || Would fail tolerance but, but MODC 16 with SO2RH prevents checking.  Reported value is nornally null.
        /// | 14 | true  |       null | 39   |     7.98 |      SO2RH |        36 || null   || Tolerance check does not occur becuase the calculated value is null.
        /// | 15 | true  |       8.40 | 36   |     null |      SO2RH |        36 || null   || Tolerance check does not occur becuase the reported value is null.
        /// | 16 | true  |       0.00 | 36   |     0.00 |      SO2RH |        36 || null   || Tolerance check does not occur becuase the sum of the reported and calculated values equals zero.
        /// </summary>
        [TestMethod()]
        public void MatsChv20()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;


            /* Input Parameter Values */
            bool?[] validList = { false, true, true, true, true, true, true, true, true, true
                                , false, true, true, true, true, true, true };
            decimal?[] calculatedValueList = { 8.40m, 8.40m, 8.40m, 8.40m, 8.40m, null, 8.40m, 0.00m, 8.40m, 7.99m
                                             , 8.40m, 8.40m, 8.40m, 8.40m, null, 8.40m, 0.00m };
            string[] modcList = { "36", "36", "37", "38", "39", "36", "36", "36", "36", "36"
                                , "36", "36", "37", "38", "39", "36", "36" };
            decimal?[] reportedValueList = { 7.98m, 7.98m, 7.98m, 7.98m, 7.98m, 7.98m, null, 0.00m, 7.99m, 8.40m
                                           , 7.98m, 7.98m, 7.98m, 7.98m, 7.98m, null, 0.00m };
            string[] so2ParamList = { null, null, "SO2RE", "SO2RE", "SO2RH", "SO2RH", null, null, null, null
                                    , null, null, "SO2RE", "SO2RH", "SO2RH", "SO2RH", "SO2RH" };
            string[] so2ModcList = { null, null, "37", "38", "39", "36", null, null, null, null
                                   , null, null, "16", "16", "16", "16", "16" };

            /* Expected Values */
            string[] resultList = { null, "A", "A", null, "A", null, null, null, null, null
                                  , null, "A", null, null, null, null, null };

            /* Test Case Count */
            int caseCount = 17;

            /* Check array lengths */
            Assert.AreEqual(caseCount, validList.Length, "validList length");
            Assert.AreEqual(caseCount, calculatedValueList.Length, "calculatedValueList length");
            Assert.AreEqual(caseCount, modcList.Length, "modcList length");
            Assert.AreEqual(caseCount, reportedValueList.Length, "reportedValueList length");
            Assert.AreEqual(caseCount, resultList.Length, "resultList length");
            Assert.AreEqual(caseCount, so2ParamList.Length, "so2ParamList length");
            Assert.AreEqual(caseCount, so2ModcList.Length, "so2ModcList length");


            /*  Initialize Unchanging Input Parameters*/
            EmParameters.MatsDhvMeasuredModcList = "36,37,39";


            /* Run Cases */
            for (int caseDex = 0; caseDex < caseCount; caseDex++)
            {
                /*  Initialize Input Parameters*/
                EmParameters.CalculatedUnadjustedValue = calculatedValueList[caseDex];
                EmParameters.CurrentDhvRecordValid = validList[caseDex];
                EmParameters.MatsDhvRecord = new MATSDerivedHourlyValueData(unadjustedHrlyValue: reportedValueList[caseDex].DecimaltoScientificNotation(6/* 6 sigs for test case */), modcCd: modcList[caseDex], parameterCd: so2ParamList[caseDex]);

                EmParameters.CurrentSo2MonitorHourlyRecord = null;
                if (so2ModcList[caseDex] != null || so2ParamList[caseDex] != null)
                {
                    EmParameters.CurrentSo2MonitorHourlyRecord = new VwMpMonitorHrlyValueSo2cRow(modcCd: so2ModcList[caseDex]);
                }

                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Initialize variables needed to run the check. */
                bool log = false;
                string actual;

                /* Run Check */
                actual = cMATSCalculatedHourlyValueChecks.MATSCHV20(category, ref log);


                /* Check results */
                Assert.AreEqual(string.Empty, actual, string.Format("actual {0}", caseDex));
                Assert.AreEqual(false, log, string.Format("log {0}", caseDex));
                Assert.AreEqual(resultList[caseDex], category.CheckCatalogResult, String.Format("Result [case {0}]", caseDex));
            }

        }

        #endregion
    }
}