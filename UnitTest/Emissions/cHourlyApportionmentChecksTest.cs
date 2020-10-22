using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Data.Ecmps.Dbo.View;
using ECMPS.Checks.Data.EcmpsAux.CrossCheck.Virtual;
using ECMPS.Checks.EmissionsChecks;
using ECMPS.Checks.Em.Parameters;


namespace UnitTest.Emissions
{
    [TestClass]
    public class cHourlyApportionmentChecksTest
    {

        /// <summary>
        /// Make sure all new parameters are initialized appropriatly.
        /// 
        /// MatsMs1HgDhvId  == null
        /// MatsMs1HclDhvId == null
        /// MatsMs1HfDhvId  == null
        /// MatsMs1So2DhvId == null
        /// 
        /// Apportionment_Hg_Rate_Array is an array of 8 null values
        /// Apportionment_HCL_Rate_Array is an array of 8 null values
        /// Apportionment_HF_Rate_Array is an array of 8 null values
        /// Apportionment_SO2_Rate_Array is an array of 8 null values
        /// </summary>
        [TestMethod]
        public void HourApp1()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            /* Currently cannot access arrays using the new check parameter access. */
            category.Process.ProcessParameters.RegisterParameter(4642, "Apportionment_Hg_Rate_Array");
            category.Process.ProcessParameters.RegisterParameter(4647, "Apportionment_HCL_Rate_Array");
            category.Process.ProcessParameters.RegisterParameter(4648, "Apportionment_HF_Rate_Array");
            category.Process.ProcessParameters.RegisterParameter(4649, "Apportionment_SO2_Rate_Array");
            category.Process.ProcessParameters.RegisterParameter(4673, "MATS_MS1_Hg_MODC_Code_Array");
            category.Process.ProcessParameters.RegisterParameter(4674, "MATS_MS1_HCL_MODC_Code_Array");
            category.Process.ProcessParameters.RegisterParameter(4675, "MATS_MS1_HF_MODC_Code_Array");
            category.Process.ProcessParameters.RegisterParameter(4676, "MATS_MS1_SO2_MODC_Code_Array");

            /* Initialize Parameters */
            EmParameters.CurrentLocationCount = 8;

            /* Init Cateogry Result */
            category.CheckCatalogResult = null;

            /* Initialize variables needed to run the check. */
            bool log = false;
            string actual;
            string[] expectedRateArray = { null, null, null, null, null, null, null, null };

            actual = cHourlyApportionmentChecks.HOURAPP1(category, ref log);

            /* Get Output Arrays */
            string[] apportionmentHgRateArray = (string[])category.GetCheckParameter("Apportionment_Hg_Rate_Array").ParameterValue;
            string[] apportionmentHclRateArray = (string[])category.GetCheckParameter("Apportionment_HCL_Rate_Array").ParameterValue;
            string[] apportionmentHfRateArray = (string[])category.GetCheckParameter("Apportionment_HF_Rate_Array").ParameterValue;
            string[] apportionmentSo2RateArray = (string[])category.GetCheckParameter("Apportionment_SO2_Rate_Array").ParameterValue;
            string[] matsMS1HgModcCodeArray = (string[])category.GetCheckParameter("MATS_MS1_Hg_MODC_Code_Array").ParameterValue;
            string[] matsMS1HclModcCodeArray = (string[])category.GetCheckParameter("MATS_MS1_HCL_MODC_Code_Array").ParameterValue;
            string[] matsMS1HfModcCodeArray = (string[])category.GetCheckParameter("MATS_MS1_HF_MODC_Code_Array").ParameterValue;
            string[] matsMS1So2ModcCodeArray = (string[])category.GetCheckParameter("MATS_MS1_SO2_MODC_Code_Array").ParameterValue;

            /* Check results */
            Assert.AreEqual(null, EmParameters.MatsMs1HgDhvId, string.Format("MatsMs1HgDhvId actual {0}", EmParameters.MatsMs1HgDhvId));
            Assert.AreEqual(null, EmParameters.MatsMs1HclDhvId, string.Format("MatsMs1HclDhvId actual {0}", EmParameters.MatsMs1HclDhvId));
            Assert.AreEqual(null, EmParameters.MatsMs1HfDhvId, string.Format("MatsMs1HfDhvId actual {0}", EmParameters.MatsMs1HfDhvId));
            Assert.AreEqual(null, EmParameters.MatsMs1So2DhvId, string.Format("MatsMs1So2DhvId actual {0}", EmParameters.MatsMs1So2DhvId));
            Assert.AreEqual(null, EmParameters.MatsMs1HgUnadjustedHourlyValue, string.Format("MatsMs1HgUnadjustedHourlyValue actual {0}", EmParameters.MatsMs1HgUnadjustedHourlyValue));
            Assert.AreEqual(null, EmParameters.MatsMs1HclUnadjustedHourlyValue, string.Format("MatsMs1HclUnadjustedHourlyValue actual {0}", EmParameters.MatsMs1HclUnadjustedHourlyValue));
            Assert.AreEqual(null, EmParameters.MatsMs1HfUnadjustedHourlyValue, string.Format("MatsMs1HfUnadjustedHourlyValue actual {0}", EmParameters.MatsMs1HfUnadjustedHourlyValue));
            Assert.AreEqual(null, EmParameters.MatsMs1So2UnadjustedHourlyValue, string.Format("MatsMs1So2UnadjustedHourlyValue actual {0}", EmParameters.MatsMs1So2UnadjustedHourlyValue));
            CollectionAssert.AreEqual(apportionmentHgRateArray, expectedRateArray, "apportionmentHgRateArray ");
            CollectionAssert.AreEqual(apportionmentHclRateArray, expectedRateArray, "apportionmentHclRateArray ");
            CollectionAssert.AreEqual(apportionmentHfRateArray, expectedRateArray, "apportionmentHfRateArray ");
            CollectionAssert.AreEqual(apportionmentSo2RateArray, expectedRateArray, "apportionmentSo2RateArray ");
            CollectionAssert.AreEqual(matsMS1HgModcCodeArray, expectedRateArray, "matsMS1HgModcCodeArray ");
            CollectionAssert.AreEqual(matsMS1HclModcCodeArray, expectedRateArray, "matsMS1HclModcCodeArray ");
            CollectionAssert.AreEqual(matsMS1HfModcCodeArray, expectedRateArray, "matsMS1HfModcCodeArray ");
            CollectionAssert.AreEqual(matsMS1So2ModcCodeArray, expectedRateArray, "matsMS1So2ModcCodeArray ");
        }

        /// <summary>
        /// 
        /// 
        /// | ## | Config  | StackPipeId | FlowMs1 | OpMs1 | FlowMs2 | OpMs2 | LoadUnt | LoadMs1 | LoadMs2 | Tolerance || Result | CalcLoad || Note
        /// |  0 | COMPLEX | SP1         |    10.0 |  1.00 |    10.0 |  1.00 |    1000 |       1 |     999 |         1 || null   |     null || Check does not apply to config type.
        /// |  1 | CS      | SP1         |    10.0 |  1.00 |    10.0 |  1.00 |    1000 |       1 |     999 |         1 || null   |     null || Check does not apply to config type.
        /// |  2 | CSMS    | SP1         |    10.0 |  1.00 |    10.0 |  1.00 |    1000 |       1 |     999 |         1 || null   |     null || Check does not apply to config type.
        /// |  3 | MS      | null        |    10.0 |  1.00 |    10.0 |  1.00 |    1000 |       1 |     999 |         1 || null   |     null || Check does not apply to units.
        /// |  4 | MS      | SP1         |    null |  1.00 |    10.0 |  1.00 |    1000 |       1 |     999 |         1 || null   |     null || Calculation cannot occur if flow does not exist at every MS.
        /// |  5 | MS      | SP1         |    10.0 |  1.00 |    null |  1.00 |    1000 |       1 |     999 |         1 || null   |     null || Calculation cannot occur if flow does not exist at every MS.
        /// |  6 | MS      | SP1         |    null |  1.00 |    null |  1.00 |    1000 |       1 |     999 |         1 || null   |     null || Calculation cannot occur if flow does not exist at every MS.
        /// |  7 | MS      | SP1         |    10.0 |  1.00 |    10.0 |  1.00 |    null |       1 |     999 |         1 || null   |     null || Calculation connot occur if load does not exist at the unit.
        /// |  8 | MS      | SP1         |     0.0 |  1.00 |     0.0 |  1.00 |    1000 |       1 |     999 |         1 || null   |     null || Calculation connot occur if the sum of the MS stack flow is equal to zero.
        /// |  9 | MS      | SP1         |    -0.1 |  1.00 |     0.0 |  1.00 |    1000 |       1 |     999 |         1 || null   |     null || Calculation connot occur if the sum of the MS stack flow is less than zero.
        /// | 10 | MS      | SP1         |     0.0 |  1.00 |    -0.1 |  1.00 |    1000 |       1 |     999 |         1 || null   |     null || Calculation connot occur if the sum of the MS stack flow is less than zero.
        /// | 11 | MS      | SP1         |     1.3 |  1.00 |     3.9 |  1.00 |    1000 |     250 |     750 |         1 || null   |      250 || Reported value matches calculated value.
        /// | 12 | MS      | SP1         |     1.3 |  1.00 |     3.9 |  1.00 |    1000 |     249 |     751 |         1 || null   |      250 || Reported value within tolerance of calculated value.
        /// | 13 | MS      | SP1         |     1.3 |  1.00 |     3.9 |  1.00 |    1000 |     251 |     749 |         1 || null   |      250 || Reported value within tolerance of calculated value.
        /// | 14 | MS      | SP1         |     1.3 |  1.00 |     3.9 |  1.00 |    1000 |     248 |     752 |         1 || A      |      250 || Reported value outsode of tolerance with calculated value.
        /// | 15 | MS      | SP1         |     1.3 |  1.00 |     3.9 |  1.00 |    1000 |     252 |     748 |         1 || A      |      250 || Reported value outsode of tolerance with calculated value.
        /// | 16 | MS      | SP1         |     1.3 |  1.00 |     3.9 |  1.00 |    1000 |    null |     750 |         1 || null   |     null || Reported value is null (This condtion was added due to null value error)
        /// | 17 | MS      | SP1         |     5.2 |  0.25 |     5.2 |  0.75 |    1000 |     250 |     750 |         1 || null   |      250 || Reported value matches calculated value.
        /// | 18 | MS      | SP1         |     5.2 |  0.25 |     5.2 |  0.75 |    1000 |     249 |     751 |         1 || null   |      250 || Reported value within tolerance of calculated value.
        /// | 19 | MS      | SP1         |     5.2 |  0.25 |     5.2 |  0.75 |    1000 |     251 |     749 |         1 || null   |      250 || Reported value within tolerance of calculated value.
        /// | 20 | MS      | SP1         |     5.2 |  0.25 |     5.2 |  0.75 |    1000 |     248 |     752 |         1 || A      |      250 || Reported value outsode of tolerance with calculated value.
        /// | 21 | MS      | SP1         |     5.2 |  0.25 |     5.2 |  0.75 |    1000 |     252 |     748 |         1 || A      |      250 || Reported value outsode of tolerance with calculated value.
        /// </summary>
        [TestMethod]
        public void HourApp9()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            /* Currently cannot access arrays using the new check parameter access. */
            category.Process.ProcessParameters.RegisterParameter(3619, "Apportionment_MATS_Load_Array");
            category.Process.ProcessParameters.RegisterParameter(2232, "Apportionment_Optime_Array");
            category.Process.ProcessParameters.RegisterParameter(3615, "Apportionment_Stack_Flow_Array");
            category.Process.ProcessParameters.RegisterParameter(3618, "Location_Name_Array");


            /* Input Parameter Values */
            string[] configList = { "COMPLEX", "CS", "CSMS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS" };
            string[] stackPipeIdList = { "SP1", "SP1", "SP1", null, "SP1", "SP1", "SP1", "SP1", "SP1", "SP1", "SP1", "SP1", "SP1", "SP1", "SP1", "SP1", "SP1", "SP1", "SP1", "SP1", "SP1", "SP1" };
            decimal?[] flowMs1List = { 10.0m, 10.0m, 10.0m, 10.0m, null, 10.0m, null, 10.0m, 0.0m, -0.1m, 0.0m, 1.3m, 1.3m, 1.3m, 1.3m, 1.3m, 1.3m, 5.2m, 5.2m, 5.2m, 5.2m, 5.2m };
            decimal[] opTimeMs1List = { 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 0.25m, 0.25m, 0.25m, 0.25m, 0.25m };
            decimal?[] flowMs2List = { 10.0m, 10.0m, 10.0m, 10.0m, 10.0m, null, null, 10.0m, 0.0m, 0.0m, -0.1m, 3.9m, 3.9m, 3.9m, 3.9m, 3.9m, 3.9m, 5.2m, 5.2m, 5.2m, 5.2m, 5.2m };
            decimal[] opTimeMs2List = { 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 0.75m, 0.75m, 0.75m, 0.75m, 0.75m };
            int?[] loadUnitList = { 1000, 1000, 1000, 1000, 1000, 1000, 1000, null, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000 };
            int?[] loadMs1List = { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 250, 249, 251, 248, 252, null, 250, 249, 251, 248, 252 };
            int?[] loadMs2List = { 999, 999, 999, 999, 999, 999, 999, 999, 999, 999, 999, 750, 751, 749, 752, 748, 750, 750, 751, 749, 752, 748 };
            int?[] toleranceList = { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };

            /* Expected Values */
            string[] expResultList = { null, null, null, null, null, null, null, null, null, null, null, null, null, null, "A", "A", null, null, null, null, "A", "A" };
            decimal?[] expCalcLoadList = { null, null, null, null, null, null, null, null, null, null, null, 250, 250, 250, 250, 250, null, 250, 250, 250, 250, 250 };

            /* Test Case Count */
            int caseCount = 22;

            /* Check array lengths */
            Assert.AreEqual(caseCount, configList.Length, "configList length");
            Assert.AreEqual(caseCount, stackPipeIdList.Length, "stackPipeIdList length");
            Assert.AreEqual(caseCount, flowMs1List.Length, "flowMs1List length");
            Assert.AreEqual(caseCount, opTimeMs1List.Length, "opTimeMs1List length");
            Assert.AreEqual(caseCount, flowMs2List.Length, "flowMs2List length");
            Assert.AreEqual(caseCount, opTimeMs2List.Length, "opTimeMs2List length");
            Assert.AreEqual(caseCount, loadUnitList.Length, "loadUnitList length");
            Assert.AreEqual(caseCount, loadMs1List.Length, "loadMs1List length");
            Assert.AreEqual(caseCount, loadMs2List.Length, "loadMs2List length");
            Assert.AreEqual(caseCount, toleranceList.Length, "toleranceList length");
            Assert.AreEqual(caseCount, expResultList.Length, "resultList length");
            Assert.AreEqual(caseCount, expCalcLoadList.Length, "expCalcLoadList length");



            /* Run Cases */
            for (int caseDex = 0; caseDex < caseCount; caseDex++)
            {
                /*  Initialize Required Parameters */
                EmParameters.CurrentMonitorPlanLocationPostion = 1;
                EmParameters.CurrentMonitorPlanLocationRecord = new VwCeMpMonitorLocationRow(stackPipeId: stackPipeIdList[caseDex]);
                EmParameters.MpStackConfigForHourlyChecks = configList[caseDex];
                EmParameters.MwLoadHourlyTolerance = toleranceList[caseDex];

                category.SetCheckParameter("Apportionment_MATS_Load_Array", new int?[] { loadUnitList[caseDex], loadMs1List[caseDex], loadMs2List[caseDex] });
                category.SetCheckParameter("Apportionment_OpTime_Array", new decimal[] { Math.Max(opTimeMs1List[caseDex], opTimeMs2List[caseDex]), opTimeMs1List[caseDex], opTimeMs2List[caseDex] });
                category.SetCheckParameter("Apportionment_Stack_Flow_Array", new decimal?[] { null, flowMs1List[caseDex], flowMs2List[caseDex] });
                category.SetCheckParameter("Location_Name_Array", new string[] { "UN", "MS1", "MS2" });

                /*  Initialize Output Parameters */
                EmParameters.CalculatedMatsMsLoad = -1234;


                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Initialize variables needed to run the check. */
                bool log = false;
                string actual;

                /* Run Check */
                actual = cHourlyApportionmentChecks.HOURAPP9(category, ref log);

                /* Check results */
                Assert.AreEqual(string.Empty, actual, string.Format("actual {0}", caseDex));
                Assert.AreEqual(false, log, string.Format("log {0}", caseDex));
                Assert.AreEqual(expResultList[caseDex], category.CheckCatalogResult, String.Format("Result [case {0}]", caseDex));

                Assert.AreEqual(expCalcLoadList[caseDex], EmParameters.CalculatedMatsMsLoad, string.Format("CalculatedMatsMsLoad [case {0}]", caseDex));
            }

        }

        /// <summary>
        /// 
        /// 
        /// | ## | DhvId | Config  | UnitId | FlowMs1 | OpMs1 | FlowMs2 | OpMs2 |   RateMs1 |   RateMs2 | MODC1 | MODC2 | UnadjHrly || Result | CalcFlow || Note
        /// |  0 |     0 | COMPLEX | 24     |    10.0 |  1.00 |    10.0 |  1.00 |    1.00E0 |    9.99E2 |    36 |    36 |      null ||   null |     null || Check does not apply to config type.
        /// |  1 |     1 | CS      | 24     |    10.0 |  1.00 |    10.0 |  1.00 |    1.00E0 |    9.99E2 |    36 |    36 |      null ||   null |     null || Check does not apply to config type.
        /// |  2 |     2 | CSMS    | 24     |    10.0 |  1.00 |    10.0 |  1.00 |    1.00E0 |    9.99E2 |    36 |    36 |      null ||   null |     null || Check does not apply to config type.
        /// |  3 |     3 | MS      | null   |    10.0 |  1.00 |    10.0 |  1.00 |    1.00E0 |    9.99E2 |    36 |    36 |      null ||   null |     null || Check does not apply to units.
        /// |  4 |     4 | MS      | 24     |    null |  1.00 |    10.0 |  1.00 |    1.00E0 |    9.99E2 |    36 |    36 |    9.99E2 ||      C |     null || Calculation can occur if flow does not exist at every MS.
        /// |  5 |     5 | MS      | 24     |    10.0 |  1.00 |    null |  1.00 |    1.00E0 |    9.99E2 |    36 |    36 |    1.00E0 ||      C |     null || Calculation can occur if flow does not exist at every MS.
        /// |  6 |     6 | MS      | 24     |    null |  1.00 |    null |  1.00 |    1.00E0 |    9.99E2 |    36 |    36 |      null ||      B |     null || Calculation cannot occur if flow does not exist at any MS.
        /// |  7 |     7 | MS      | 24     |    10.0 |  1.00 |    10.0 |  1.00 |    1.00E0 |    9.99E2 |    36 |    36 |    5.00E2 ||   null |   5.00E2 || Calculation con occur if load does not exist at the unit.
        /// |  8 |     8 | MS      | 25     |     0.0 |  1.00 |     0.0 |  1.00 |    1.00E0 |    9.99E2 |    36 |    36 |    5.00E2 ||      B |     null || Calculation connot occur if the sum of the MS stack flow is equal to zero. Should this cause an error? It does not right now.
        /// |  9 |     9 | MS      | 25     |    -0.1 |  1.00 |     0.0 |  1.00 |    1.00E0 |    9.99E2 |    36 |    36 |      null ||      B |     null || Calculation connot occur if the sum of the MS stack flow is less than zero.
        /// | 10 |    10 | MS      | 24     |     0.0 |  1.00 |    -0.1 |  1.00 |    1.00E0 |    9.99E2 |    36 |    36 |      null ||      B |     null || Calculation connot occur if the sum of the MS stack flow is less than zero.
        /// | 11 |    11 | MS      | 24     |     1.3 |  1.00 |     3.9 |  1.00 |    2.50E2 |    7.50E2 |    36 |    36 |    6.25E2 ||   null |   6.25E2 || Reported value matches calculated value.
        /// | 12 |    12 | MS      | 24     |     1.3 |  1.00 |     3.9 |  1.00 |    2.49E2 |    7.51E2 |    36 |    36 |    6.26E2 ||   null |   6.26E2 || Reported value within tolerance of calculated value.
        /// | 13 |  null | MS      | 24     |     1.3 |  1.00 |     3.9 |  1.00 |    2.51E2 |    7.49E2 |    36 |    36 |      null ||   null |     null || Check does not apply to non MS-1 calculations
        /// | 14 |    14 | MS      | 24     |     1.3 |  1.00 |     3.9 |  1.00 |    2.48E2 |    7.52E2 |    36 |    36 |    6.26E2 ||   null |   6.26E2 || Reported value outsode of tolerance with calculated value.
        /// | 15 |  null | MS      | 24     |     1.3 |  1.00 |     3.9 |  1.00 |    2.52E2 |    7.48E2 |    36 |    36 |      null ||   null |     null || Check does not apply to non MS-1 calculations
        /// | 16 |    16 | MS      | 24     |     1.3 |  1.00 |     3.9 |  1.00 |      null |    7.50E2 |    36 |    36 |    7.50E2 ||      C |     null || Reported value is null (This condtion was added due to null value error)
        /// | 17 |    17 | MS      | 24     |     5.2 |  0.25 |     5.2 |  0.75 |    2.50E2 |    7.50E2 |    36 |    36 |    5.00E2 ||   null |   5.00E2 || Reported value matches calculated value.
        /// | 18 |    18 | MS      | 24     |     5.2 |  0.25 |     5.2 |  0.75 |    2.49E2 |    7.51E2 |    36 |    36 |    4.80E2 ||   null |   5.00E2 || Reported value within percent error of calculated value.
        /// | 19 |    19 | MS      | 24     |     5.2 |  0.25 |     5.2 |  0.75 |    2.51E2 |    7.49E2 |    36 |    36 |    5.20E2 ||   null |   5.00E2 || Reported value within percent error of calculated value.
        /// | 20 |    20 | MS      | 24     |     5.2 |  0.25 |     5.2 |  0.75 |    2.48E2 |    7.52E2 |    36 |    36 |    4.71E2 ||      A |   5.00E2 || Reported value outsode of percent error of calculated value.
        /// | 21 |    21 | MS      | 24     |     5.2 |  0.25 |     5.2 |  0.75 |    2.52E2 |    7.48E2 |    36 |    36 |    5.26E2 ||      A |   5.00E2 || Reported value outsode of percent error of calculated value.
        /// | 22 |  null | MS      | 24     |     1.3 |  1.00 |     3.9 |  1.00 |    2.52E2 |    7.48E2 |    36 |    36 |      null ||   null |     null || Check does not apply to non MS-1 calculations
        /// | 23 |    23 | MS      | 24     |     1.3 |  1.00 |     3.9 |  1.00 |      null |    7.50E2 |    36 |    36 |      null ||      B |     null || Reported value is null (This condtion was added due to null value error)
        /// | 24 |    24 | MS      | 24     |     5.2 |  0.25 |     5.2 |  0.75 |    2.50E2 |    7.50E2 |    36 |    36 |      null ||      B |     null || Reported value matches calculated value.
        /// | 25 |    25 | MS      | 24     |     5.2 |  0.25 |     5.2 |  0.75 |    2.49E2 |    7.51E2 |    36 |    36 |      null ||      B |     null || Reported value within percent error of calculated value.
        /// | 26 |    26 | MS      | 24     |     5.2 |  0.25 |     5.2 |  0.75 |    2.52E2 |    7.48E2 |    36 |    36 |      null ||      B |     null || Reported value outsode of percent error of calculated value.
        /// | 27 |  null | MS      | 24     |     1.3 |     0 |     3.9 |     0 |    2.52E2 |    7.48E2 |    36 |    36 |      null ||   null |     null || Check does not apply to non MS-1 calculations
        /// | 28 |    28 | MS      | 24     |     1.3 |     0 |     3.9 |     0 |      null |    7.50E2 |    36 |    36 |      null ||   null |     null || Reported value is null (This condtion was added due to null value error)
        /// | 29 |    29 | MS      | 24     |     5.2 |     0 |     5.2 |     0 |    2.50E2 |    7.50E2 |    36 |    36 |      null ||   null |     null || Reported value matches calculated value.
        /// | 30 |    30 | MS      | 24     |     5.2 |     0 |     5.2 |     0 |    2.49E2 |    7.51E2 |    36 |    36 |      null ||   null |     null || Reported value within percent error of calculated value.
        /// | 31 |    31 | MS      | 24     |     5.2 |     0 |     5.2 |     0 |    2.52E2 |    7.48E2 |    36 |    36 |      null ||   null |     null || Reported value outsode of percent error of calculated value.    
        /// | 32 |    32 | MS      | 24     |    null |  1.00 |    10.0 |     0 |    1.00E0 |    9.99E2 |    36 |    36 |    9.99E2 ||      A |   1.00E0 || Calculation can occur if flow does not exist at every MS.
        /// | 33 |    33 | MS      | 24     |    null |  1.00 |    10.0 |     0 |    1.00E0 |    9.99E2 |    36 |    36 |    1.00E0 ||   null |   1.00E0 || Calculation can occur if flow does not exist at every MS.
        /// | 34 |    34 | MS      | 24     |    10.0 |     0 |    null |  1.00 |    1.00E0 |    9.99E2 |    36 |    36 |    1.00E0 ||      A |   9.99E2 || Calculation can occur if flow does not exist at every MS.
        /// | 35 |    35 | MS      | 24     |    10.0 |     0 |    null |  1.00 |    1.00E0 |    9.99E2 |    36 |    36 |    9.99E2 ||   null |   9.99E2 || Calculation can occur if flow does not exist at every MS.
        /// </summary>
        [TestMethod]
        public void HourApp10()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            /* Currently cannot access arrays using the new check parameter access. */
            category.Process.ProcessParameters.RegisterParameter(4642, "Apportionment_Hg_Rate_Array");
            category.Process.ProcessParameters.RegisterParameter(2232, "Apportionment_Optime_Array");
            category.Process.ProcessParameters.RegisterParameter(3615, "Apportionment_Stack_Flow_Array");
            category.Process.ProcessParameters.RegisterParameter(4673, "MATS_MS1_Hg_MODC_Code_Array");
            category.Process.ProcessParameters.RegisterParameter(3618, "Location_Name_Array");


            /* Input Parameter Values */
            string[] dhvIdList = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", null, "14", null, "16", "17", "18", "19", "20", "21", null, "23", "24", "25", "26", null, "28", "29", "30", "31", "32", "33", "34", "35" };
            string[] configList = { "COMPLEX", "CS", "CSMS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS" };
            int?[] unitIdList = { 24, 24, 24, null, 24, 24, 24, 24, 25, 25, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24 };
            decimal?[] flowMs1List = { 10.0m, 10.0m, 10.0m, 10.0m, null, 10.0m, null, 10.0m, 0.0m, -0.1m, 0.0m, 1.3m, 1.3m, 1.3m, 1.3m, 1.3m, 1.3m, 5.2m, 5.2m, 5.2m, 5.2m, 5.2m, 1.3m, 1.3m, 5.2m, 5.2m, 5.2m, 1.3m, 1.3m, 5.2m, 5.2m, 5.2m, null, null, 10.0m, 10.0m };
            decimal[] opTimeMs1List = { 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 0.25m, 0.25m, 0.25m, 0.25m, 0.25m, 1.00m, 1.00m, 0.25m, 0.25m, 0.25m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 1.00m, 1.00m, 0.00m, 0.00m };
            decimal?[] flowMs2List = { 10.0m, 10.0m, 10.0m, 10.0m, 10.0m, null, null, 10.0m, 0.0m, 0.0m, -0.1m, 3.9m, 3.9m, 3.9m, 3.9m, 3.9m, 3.9m, 5.2m, 5.2m, 5.2m, 5.2m, 5.2m, 3.9m, 3.9m, 5.2m, 5.2m, 5.2m, 3.9m, 3.9m, 5.2m, 5.2m, 5.2m, 10.0m, 10.0m, null, null };
            decimal[] opTimeMs2List = { 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 0.75m, 0.75m, 0.75m, 0.75m, 0.75m, 1.00m, 1.00m, 0.75m, 0.75m, 0.75m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 1.00m, 1.00m };
            string[] hgRateMs1List = { "1.00E0", "1.00E0", "1.00E0", "1.00E0", "1.00E0", "1.00E0", "1.00E0", "1.00E0", "1.00E0", "1.00E0", "1.00E0", "2.50E2", "2.49E2", "2.51E2", "2.48E2", "2.52E2", null, "2.50E2", "2.49E2", "2.51E2", "2.48E2", "2.52E2", "2.52E2", null, "2.50E2", "2.49E2", "2.52E2", "2.52E2", null, "2.50E2", "2.49E2", "2.52E2", "1.00E0", "1.00E0", "1.00E0", "1.00E0" };
            string[] hgRateMs2List = { "9.99E2", "9.99E2", "9.99E2", "9.99E2", "9.99E2", "9.99E2", "9.99E2", "9.99E2", "9.99E2", "9.99E2", "9.99E2", "7.50E2", "7.51E2", "7.49E2", "7.52E2", "7.48E2", "7.50E2", "7.50E2", "7.51E2", "7.49E2", "7.52E2", "7.48E2", "7.48E2", "7.50E2", "7.50E2", "7.51E2", "7.48E2", "7.48E2", "7.50E2", "7.50E2", "7.51E2", "7.48E2", "9.99E2", "9.99E2", "9.99E2", "9.99E2" };
            string[] unadjustedValueList = { null, null, null, null, "9.99E2", "1.00E0", null, "5.00E2", null, null, null, "6.25E2", "6.26E2", null, "6.26E2", null, "7.50E2", "5.00E2", "4.80E2", "5.20E2", "4.71E2", "5.26E2", null, null, null, null, null, null, null, null, null, null, "9.99E2", "1.00E0", "1.00E0", "9.99E2" };
            string[] modcCd1List = { "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36" };
            string[] modcCd2List = { "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36" };

            /* Expected Values */
            string[] expectedResultList = { null, null, null, null, "C", "C", "B", null, "B", "B", "B", null, null, null, null, null, "C", null, null, null, "A", "A", null, "B", "B", "B", "B", null, null, null, null, null, "A", null, "A", null };
            string[] expCalcFlowList = { null, null, null, null, null, null, null, "5.00E2", null, null, null, "6.25E2", "6.26E2", null, "6.26E2", null, null, "5.00E2", "5.00E2", "5.00E2", "5.00E2", "5.00E2", null, null, null, null, null, null, null, null, null, null, "1.00E0", "1.00E0", "9.99E2", "9.99E2" };

            /* Test Case Count */
            int caseCount = 36;

            /* Check array lengths */
            Assert.AreEqual(caseCount, dhvIdList.Length, "dhvIdList length");
            Assert.AreEqual(caseCount, configList.Length, "configList length");
            Assert.AreEqual(caseCount, unitIdList.Length, "unitIdList length");
            Assert.AreEqual(caseCount, flowMs1List.Length, "flowMs1List length");
            Assert.AreEqual(caseCount, opTimeMs1List.Length, "opTimeMs1List length");
            Assert.AreEqual(caseCount, flowMs2List.Length, "flowMs2List length");
            Assert.AreEqual(caseCount, opTimeMs2List.Length, "opTimeMs2List length");
            Assert.AreEqual(caseCount, hgRateMs1List.Length, "hgRateMs1List length");
            Assert.AreEqual(caseCount, hgRateMs2List.Length, "hgRateMs2List length");
            Assert.AreEqual(caseCount, unadjustedValueList.Length, "unadjustedValueList length");
            Assert.AreEqual(caseCount, modcCd1List.Length, "modcCd1List length");
            Assert.AreEqual(caseCount, modcCd2List.Length, "modcCd2List length");
            Assert.AreEqual(caseCount, expectedResultList.Length, "expectedResultList length");
            Assert.AreEqual(caseCount, expCalcFlowList.Length, "expCalcFlowList length");

            /* Run Cases */
            for (int caseDex = 0; caseDex < caseCount; caseDex++)
            {
                /*  Initialize Required Parameters */
                EmParameters.CurrentMonitorPlanLocationPostion = 1;
                EmParameters.CurrentMonitorPlanLocationRecord = new VwCeMpMonitorLocationRow(unitId: unitIdList[caseDex]);
                EmParameters.MpStackConfigForHourlyChecks = configList[caseDex];
                EmParameters.MatsMs1HgDhvId = dhvIdList[caseDex];
                EmParameters.MatsMs1HgUnadjustedHourlyValue = unadjustedValueList[caseDex];

                category.SetCheckParameter("Apportionment_Hg_Rate_Array", new string[] { null, hgRateMs1List[caseDex], hgRateMs2List[caseDex] });
                category.SetCheckParameter("Apportionment_OpTime_Array", new decimal[] { Math.Max(opTimeMs1List[caseDex], opTimeMs2List[caseDex]), opTimeMs1List[caseDex], opTimeMs2List[caseDex] });
                category.SetCheckParameter("Apportionment_Stack_Flow_Array", new decimal?[] { null, flowMs1List[caseDex], flowMs2List[caseDex] });
                category.SetCheckParameter("MATS_MS1_Hg_MODC_Code_Array", new string[] { null, modcCd1List[caseDex], modcCd2List[caseDex] });
                category.SetCheckParameter("Location_Name_Array", new string[] { "UN", "MS1", "MS2" });

                /*  Initialize Output Parameters */
                EmParameters.CalculatedFlowWeightedHg = "-1.23E4";
                EmParameters.MatsReportedPluginHg = "-2.23E4";
                EmParameters.MatsParameterPluginHg = "UNKNOWN";

                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Initialize variables needed to run the check. */
                bool log = false;
                string actual;

                /* Run Check */
                actual = cHourlyApportionmentChecks.HOURAPP10(category, ref log);

                /* Check results */
                Assert.AreEqual(string.Empty, actual, string.Format("actual {0}", caseDex));
                Assert.AreEqual(false, log, string.Format("log {0}", caseDex));
                Assert.AreEqual(expectedResultList[caseDex], category.CheckCatalogResult, string.Format("Result {0}", caseDex));
                Assert.AreEqual(EmParameters.MatsReportedPluginHg, unadjustedValueList[caseDex], string.Format("MATS Reported {0}", caseDex));
                Assert.AreEqual(expCalcFlowList[caseDex], EmParameters.CalculatedFlowWeightedHg, string.Format("CalculatedFlowWeightedHg [case {0}]", caseDex));
            }
        }

        /// <summary>
        /// 
        /// 
        /// | ## | DhvId | Config  | UnitId | FlowMs1 | OpMs1 | FlowMs2 | OpMs2 |   RateMs1 |   RateMs2 | MODC1 | MODC2 | UnadjHrly || Result | CalcFlow || Note
        /// |  0 |     0 | COMPLEX | 24     |    10.0 |  1.00 |    10.0 |  1.00 |    1.00E0 |    9.99E2 |    36 |    36 |      null ||   null |     null || Check does not apply to config type.
        /// |  1 |     1 | CS      | 24     |    10.0 |  1.00 |    10.0 |  1.00 |    1.00E0 |    9.99E2 |    36 |    36 |      null ||   null |     null || Check does not apply to config type.
        /// |  2 |     2 | CSMS    | 24     |    10.0 |  1.00 |    10.0 |  1.00 |    1.00E0 |    9.99E2 |    36 |    36 |      null ||   null |     null || Check does not apply to config type.
        /// |  3 |     3 | MS      | null   |    10.0 |  1.00 |    10.0 |  1.00 |    1.00E0 |    9.99E2 |    36 |    36 |      null ||   null |     null || Check does not apply to units.
        /// |  4 |     4 | MS      | 24     |    null |  1.00 |    10.0 |  1.00 |    1.00E0 |    9.99E2 |    36 |    36 |    9.99E2 ||      C |     null || Calculation can occur if flow does not exist at every MS.
        /// |  5 |     5 | MS      | 24     |    10.0 |  1.00 |    null |  1.00 |    1.00E0 |    9.99E2 |    36 |    36 |    1.00E0 ||      C |     null || Calculation can occur if flow does not exist at every MS.
        /// |  6 |     6 | MS      | 24     |    null |  1.00 |    null |  1.00 |    1.00E0 |    9.99E2 |    36 |    36 |      null ||      B |     null || Calculation cannot occur if flow does not exist at any MS.
        /// |  7 |     7 | MS      | 24     |    10.0 |  1.00 |    10.0 |  1.00 |    1.00E0 |    9.99E2 |    36 |    36 |    5.00E2 ||   null |   5.00E2 || Calculation con occur if load does not exist at the unit.
        /// |  8 |     8 | MS      | 25     |     0.0 |  1.00 |     0.0 |  1.00 |    1.00E0 |    9.99E2 |    36 |    36 |    5.00E2 ||      B |     null || Calculation connot occur if the sum of the MS stack flow is equal to zero. Should this cause an error? It does not right now.
        /// |  9 |     9 | MS      | 25     |    -0.1 |  1.00 |     0.0 |  1.00 |    1.00E0 |    9.99E2 |    36 |    36 |      null ||      B |     null || Calculation connot occur if the sum of the MS stack flow is less than zero.
        /// | 10 |    10 | MS      | 24     |     0.0 |  1.00 |    -0.1 |  1.00 |    1.00E0 |    9.99E2 |    36 |    36 |      null ||      B |     null || Calculation connot occur if the sum of the MS stack flow is less than zero.
        /// | 11 |    11 | MS      | 24     |     1.3 |  1.00 |     3.9 |  1.00 |    2.50E2 |    7.50E2 |    36 |    36 |    6.25E2 ||   null |   6.25E2 || Reported value matches calculated value.
        /// | 12 |    12 | MS      | 24     |     1.3 |  1.00 |     3.9 |  1.00 |    2.49E2 |    7.51E2 |    36 |    36 |    6.26E2 ||   null |   6.26E2 || Reported value within tolerance of calculated value.
        /// | 13 |  null | MS      | 24     |     1.3 |  1.00 |     3.9 |  1.00 |    2.51E2 |    7.49E2 |    36 |    36 |      null ||   null |     null || Check does not apply to non MS-1 calculations
        /// | 14 |    14 | MS      | 24     |     1.3 |  1.00 |     3.9 |  1.00 |    2.48E2 |    7.52E2 |    36 |    36 |    6.26E2 ||   null |   6.26E2 || Reported value outsode of tolerance with calculated value.
        /// | 15 |  null | MS      | 24     |     1.3 |  1.00 |     3.9 |  1.00 |    2.52E2 |    7.48E2 |    36 |    36 |      null ||   null |     null || Check does not apply to non MS-1 calculations
        /// | 16 |    16 | MS      | 24     |     1.3 |  1.00 |     3.9 |  1.00 |      null |    7.50E2 |    36 |    36 |    7.50E2 ||      C |     null || Reported value is null (This condtion was added due to null value error)
        /// | 17 |    17 | MS      | 24     |     5.2 |  0.25 |     5.2 |  0.75 |    2.50E2 |    7.50E2 |    36 |    36 |    5.00E2 ||   null |   5.00E2 || Reported value matches calculated value.
        /// | 18 |    18 | MS      | 24     |     5.2 |  0.25 |     5.2 |  0.75 |    2.49E2 |    7.51E2 |    36 |    36 |    4.80E2 ||   null |   5.00E2 || Reported value within percent error of calculated value.
        /// | 19 |    19 | MS      | 24     |     5.2 |  0.25 |     5.2 |  0.75 |    2.51E2 |    7.49E2 |    36 |    36 |    5.20E2 ||   null |   5.00E2 || Reported value within percent error of calculated value.
        /// | 20 |    20 | MS      | 24     |     5.2 |  0.25 |     5.2 |  0.75 |    2.48E2 |    7.52E2 |    36 |    36 |    4.71E2 ||      A |   5.00E2 || Reported value outsode of percent error of calculated value.
        /// | 21 |    21 | MS      | 24     |     5.2 |  0.25 |     5.2 |  0.75 |    2.52E2 |    7.48E2 |    36 |    36 |    5.26E2 ||      A |   5.00E2 || Reported value outsode of percent error of calculated value.
        /// | 22 |  null | MS      | 24     |     1.3 |  1.00 |     3.9 |  1.00 |    2.52E2 |    7.48E2 |    36 |    36 |      null ||   null |     null || Check does not apply to non MS-1 calculations
        /// | 23 |    23 | MS      | 24     |     1.3 |  1.00 |     3.9 |  1.00 |      null |    7.50E2 |    36 |    36 |      null ||      B |     null || Reported value is null (This condtion was added due to null value error)
        /// | 24 |    24 | MS      | 24     |     5.2 |  0.25 |     5.2 |  0.75 |    2.50E2 |    7.50E2 |    36 |    36 |      null ||      B |     null || Reported value matches calculated value.
        /// | 25 |    25 | MS      | 24     |     5.2 |  0.25 |     5.2 |  0.75 |    2.49E2 |    7.51E2 |    36 |    36 |      null ||      B |     null || Reported value within percent error of calculated value.
        /// | 26 |    26 | MS      | 24     |     5.2 |  0.25 |     5.2 |  0.75 |    2.52E2 |    7.48E2 |    36 |    36 |      null ||      B |     null || Reported value outsode of percent error of calculated value.
        /// | 27 |  null | MS      | 24     |     1.3 |     0 |     3.9 |     0 |    2.52E2 |    7.48E2 |    36 |    36 |      null ||   null |     null || Check does not apply to non MS-1 calculations
        /// | 28 |    28 | MS      | 24     |     1.3 |     0 |     3.9 |     0 |      null |    7.50E2 |    36 |    36 |      null ||   null |     null || Reported value is null (This condtion was added due to null value error)
        /// | 29 |    29 | MS      | 24     |     5.2 |     0 |     5.2 |     0 |    2.50E2 |    7.50E2 |    36 |    36 |      null ||   null |     null || Reported value matches calculated value.
        /// | 30 |    30 | MS      | 24     |     5.2 |     0 |     5.2 |     0 |    2.49E2 |    7.51E2 |    36 |    36 |      null ||   null |     null || Reported value within percent error of calculated value.
        /// | 31 |    31 | MS      | 24     |     5.2 |     0 |     5.2 |     0 |    2.52E2 |    7.48E2 |    36 |    36 |      null ||   null |     null || Reported value outsode of percent error of calculated value.    
        /// | 32 |    32 | MS      | 24     |    null |  1.00 |    10.0 |     0 |    1.00E0 |    9.99E2 |    36 |    36 |    9.99E2 ||      A |   1.00E0 || Calculation can occur if flow does not exist at every MS.
        /// | 33 |    33 | MS      | 24     |    null |  1.00 |    10.0 |     0 |    1.00E0 |    9.99E2 |    36 |    36 |    1.00E0 ||   null |   1.00E0 || Calculation can occur if flow does not exist at every MS.
        /// | 34 |    34 | MS      | 24     |    10.0 |     0 |    null |  1.00 |    1.00E0 |    9.99E2 |    36 |    36 |    1.00E0 ||      A |   9.99E2 || Calculation can occur if flow does not exist at every MS.
        /// | 35 |    35 | MS      | 24     |    10.0 |     0 |    null |  1.00 |    1.00E0 |    9.99E2 |    36 |    36 |    9.99E2 ||   null |   9.99E2 || Calculation can occur if flow does not exist at every MS.
        /// </summary>
        [TestMethod]
        public void HourApp11()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            /* Currently cannot access arrays using the new check parameter access. */
            category.Process.ProcessParameters.RegisterParameter(4647, "Apportionment_HCL_Rate_Array");
            category.Process.ProcessParameters.RegisterParameter(2232, "Apportionment_Optime_Array");
            category.Process.ProcessParameters.RegisterParameter(3615, "Apportionment_Stack_Flow_Array");
            category.Process.ProcessParameters.RegisterParameter(4673, "MATS_MS1_HCL_MODC_Code_Array");
            category.Process.ProcessParameters.RegisterParameter(3618, "Location_Name_Array");


            /* Input Parameter Values */
            string[] dhvIdList = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", null, "14", null, "16", "17", "18", "19", "20", "21", null, "23", "24", "25", "26", null, "28", "29", "30", "31", "32", "33", "34", "35" };
            string[] configList = { "COMPLEX", "CS", "CSMS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS" };
            int?[] unitIdList = { 24, 24, 24, null, 24, 24, 24, 24, 25, 25, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24 };
            decimal?[] flowMs1List = { 10.0m, 10.0m, 10.0m, 10.0m, null, 10.0m, null, 10.0m, 0.0m, -0.1m, 0.0m, 1.3m, 1.3m, 1.3m, 1.3m, 1.3m, 1.3m, 5.2m, 5.2m, 5.2m, 5.2m, 5.2m, 1.3m, 1.3m, 5.2m, 5.2m, 5.2m, 1.3m, 1.3m, 5.2m, 5.2m, 5.2m, null, null, 10.0m, 10.0m };
            decimal[] opTimeMs1List = { 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 0.25m, 0.25m, 0.25m, 0.25m, 0.25m, 1.00m, 1.00m, 0.25m, 0.25m, 0.25m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 1.00m, 1.00m, 0.00m, 0.00m };
            decimal?[] flowMs2List = { 10.0m, 10.0m, 10.0m, 10.0m, 10.0m, null, null, 10.0m, 0.0m, 0.0m, -0.1m, 3.9m, 3.9m, 3.9m, 3.9m, 3.9m, 3.9m, 5.2m, 5.2m, 5.2m, 5.2m, 5.2m, 3.9m, 3.9m, 5.2m, 5.2m, 5.2m, 3.9m, 3.9m, 5.2m, 5.2m, 5.2m, 10.0m, 10.0m, null, null };
            decimal[] opTimeMs2List = { 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 0.75m, 0.75m, 0.75m, 0.75m, 0.75m, 1.00m, 1.00m, 0.75m, 0.75m, 0.75m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 1.00m, 1.00m };
            string[] hclRateMs1List = { "1.00E0", "1.00E0", "1.00E0", "1.00E0", "1.00E0", "1.00E0", "1.00E0", "1.00E0", "1.00E0", "1.00E0", "1.00E0", "2.50E2", "2.49E2", "2.51E2", "2.48E2", "2.52E2", null, "2.50E2", "2.49E2", "2.51E2", "2.48E2", "2.52E2", "2.52E2", null, "2.50E2", "2.49E2", "2.52E2", "2.52E2", null, "2.50E2", "2.49E2", "2.52E2", "1.00E0", "1.00E0", "1.00E0", "1.00E0" };
            string[] hclRateMs2List = { "9.99E2", "9.99E2", "9.99E2", "9.99E2", "9.99E2", "9.99E2", "9.99E2", "9.99E2", "9.99E2", "9.99E2", "9.99E2", "7.50E2", "7.51E2", "7.49E2", "7.52E2", "7.48E2", "7.50E2", "7.50E2", "7.51E2", "7.49E2", "7.52E2", "7.48E2", "7.48E2", "7.50E2", "7.50E2", "7.51E2", "7.48E2", "7.48E2", "7.50E2", "7.50E2", "7.51E2", "7.48E2", "9.99E2", "9.99E2", "9.99E2", "9.99E2" };
            string[] unadjustedValueList = { null, null, null, null, "9.99E2", "1.00E0", null, "5.00E2", null, null, null, "6.25E2", "6.26E2", null, "6.26E2", null, "7.50E2", "5.00E2", "4.80E2", "5.20E2", "4.71E2", "5.26E2", null, null, null, null, null, null, null, null, null, null, "9.99E2", "1.00E0", "1.00E0", "9.99E2" };
            string[] modcCd1List = { "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36" };
            string[] modcCd2List = { "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36" };

            /* Expected Values */
            string[] expectedResultList = { null, null, null, null, "C", "C", "B", null, "B", "B", "B", null, null, null, null, null, "C", null, null, null, "A", "A", null, "B", "B", "B", "B", null, null, null, null, null, "A", null, "A", null };
            string[] expCalcFlowList = { null, null, null, null, null, null, null, "5.00E2", null, null, null, "6.25E2", "6.26E2", null, "6.26E2", null, null, "5.00E2", "5.00E2", "5.00E2", "5.00E2", "5.00E2", null, null, null, null, null, null, null, null, null, null, "1.00E0", "1.00E0", "9.99E2", "9.99E2" };

            /* Test Case Count */
            int caseCount = 36;

            /* Check array lengths */
            Assert.AreEqual(caseCount, dhvIdList.Length, "dhvIdList length");
            Assert.AreEqual(caseCount, configList.Length, "configList length");
            Assert.AreEqual(caseCount, unitIdList.Length, "unitIdList length");
            Assert.AreEqual(caseCount, flowMs1List.Length, "flowMs1List length");
            Assert.AreEqual(caseCount, opTimeMs1List.Length, "opTimeMs1List length");
            Assert.AreEqual(caseCount, flowMs2List.Length, "flowMs2List length");
            Assert.AreEqual(caseCount, opTimeMs2List.Length, "opTimeMs2List length");
            Assert.AreEqual(caseCount, hclRateMs1List.Length, "hclRateMs1List length");
            Assert.AreEqual(caseCount, hclRateMs2List.Length, "hclRateMs2List length");
            Assert.AreEqual(caseCount, unadjustedValueList.Length, "unadjustedValueList length");
            Assert.AreEqual(caseCount, modcCd1List.Length, "modcCd1List length");
            Assert.AreEqual(caseCount, modcCd2List.Length, "modcCd2List length");
            Assert.AreEqual(caseCount, expectedResultList.Length, "expectedResultList length");
            Assert.AreEqual(caseCount, expCalcFlowList.Length, "expCalcFlowList length");

            /* Run Cases */
            for (int caseDex = 0; caseDex < caseCount; caseDex++)
            {
                /*  Initialize Required Parameters */
                EmParameters.CurrentMonitorPlanLocationPostion = 1;
                EmParameters.CurrentMonitorPlanLocationRecord = new VwCeMpMonitorLocationRow(unitId: unitIdList[caseDex]);
                EmParameters.MpStackConfigForHourlyChecks = configList[caseDex];
                EmParameters.MatsMs1HclDhvId = dhvIdList[caseDex];
                EmParameters.MatsMs1HclUnadjustedHourlyValue = unadjustedValueList[caseDex];

                category.SetCheckParameter("Apportionment_HCL_Rate_Array", new string[] { null, hclRateMs1List[caseDex], hclRateMs2List[caseDex] });
                category.SetCheckParameter("Apportionment_OpTime_Array", new decimal[] { Math.Max(opTimeMs1List[caseDex], opTimeMs2List[caseDex]), opTimeMs1List[caseDex], opTimeMs2List[caseDex] });
                category.SetCheckParameter("Apportionment_Stack_Flow_Array", new decimal?[] { null, flowMs1List[caseDex], flowMs2List[caseDex] });
                category.SetCheckParameter("MATS_MS1_HCL_MODC_Code_Array", new string[] { null, modcCd1List[caseDex], modcCd2List[caseDex] });
                category.SetCheckParameter("Location_Name_Array", new string[] { "UN", "MS1", "MS2" });

                /*  Initialize Output Parameters */
                EmParameters.CalculatedFlowWeightedHcl = "-1.23E4";
                EmParameters.MatsReportedPluginHcl = "-2.23E4";
                EmParameters.MatsParameterPluginHcl = "UNKNOWN";

                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Initialize variables needed to run the check. */
                bool log = false;
                string actual;

                /* Run Check */
                actual = cHourlyApportionmentChecks.HOURAPP11(category, ref log);

                /* Check results */
                Assert.AreEqual(string.Empty, actual, string.Format("actual {0}", caseDex));
                Assert.AreEqual(false, log, string.Format("log {0}", caseDex));
                Assert.AreEqual(expectedResultList[caseDex], category.CheckCatalogResult, string.Format("Result {0}", caseDex));
                Assert.AreEqual(EmParameters.MatsReportedPluginHcl, unadjustedValueList[caseDex], string.Format("MATS Reported {0}", caseDex));
                Assert.AreEqual(expCalcFlowList[caseDex], EmParameters.CalculatedFlowWeightedHcl, string.Format("CalculatedFlowWeightedHc; [case {0}]", caseDex));
            }
        }

        /// <summary>
        /// 
        /// 
        /// | ## | DhvId | Config  | UnitId | FlowMs1 | OpMs1 | FlowMs2 | OpMs2 |   RateMs1 |   RateMs2 | MODC1 | MODC2 | UnadjHrly || Result | CalcFlow || Note
        /// |  0 |     0 | COMPLEX | 24     |    10.0 |  1.00 |    10.0 |  1.00 |    1.00E0 |    9.99E2 |    36 |    36 |      null ||   null |     null || Check does not apply to config type.
        /// |  1 |     1 | CS      | 24     |    10.0 |  1.00 |    10.0 |  1.00 |    1.00E0 |    9.99E2 |    36 |    36 |      null ||   null |     null || Check does not apply to config type.
        /// |  2 |     2 | CSMS    | 24     |    10.0 |  1.00 |    10.0 |  1.00 |    1.00E0 |    9.99E2 |    36 |    36 |      null ||   null |     null || Check does not apply to config type.
        /// |  3 |     3 | MS      | null   |    10.0 |  1.00 |    10.0 |  1.00 |    1.00E0 |    9.99E2 |    36 |    36 |      null ||   null |     null || Check does not apply to units.
        /// |  4 |     4 | MS      | 24     |    null |  1.00 |    10.0 |  1.00 |    1.00E0 |    9.99E2 |    36 |    36 |    9.99E2 ||      C |     null || Calculation can occur if flow does not exist at every MS.
        /// |  5 |     5 | MS      | 24     |    10.0 |  1.00 |    null |  1.00 |    1.00E0 |    9.99E2 |    36 |    36 |    1.00E0 ||      C |     null || Calculation can occur if flow does not exist at every MS.
        /// |  6 |     6 | MS      | 24     |    null |  1.00 |    null |  1.00 |    1.00E0 |    9.99E2 |    36 |    36 |      null ||      B |     null || Calculation cannot occur if flow does not exist at any MS.
        /// |  7 |     7 | MS      | 24     |    10.0 |  1.00 |    10.0 |  1.00 |    1.00E0 |    9.99E2 |    36 |    36 |    5.00E2 ||   null |   5.00E2 || Calculation con occur if load does not exist at the unit.
        /// |  8 |     8 | MS      | 25     |     0.0 |  1.00 |     0.0 |  1.00 |    1.00E0 |    9.99E2 |    36 |    36 |    5.00E2 ||      B |     null || Calculation connot occur if the sum of the MS stack flow is equal to zero. Should this cause an error? It does not right now.
        /// |  9 |     9 | MS      | 25     |    -0.1 |  1.00 |     0.0 |  1.00 |    1.00E0 |    9.99E2 |    36 |    36 |      null ||      B |     null || Calculation connot occur if the sum of the MS stack flow is less than zero.
        /// | 10 |    10 | MS      | 24     |     0.0 |  1.00 |    -0.1 |  1.00 |    1.00E0 |    9.99E2 |    36 |    36 |      null ||      B |     null || Calculation connot occur if the sum of the MS stack flow is less than zero.
        /// | 11 |    11 | MS      | 24     |     1.3 |  1.00 |     3.9 |  1.00 |    2.50E2 |    7.50E2 |    36 |    36 |    6.25E2 ||   null |   6.25E2 || Reported value matches calculated value.
        /// | 12 |    12 | MS      | 24     |     1.3 |  1.00 |     3.9 |  1.00 |    2.49E2 |    7.51E2 |    36 |    36 |    6.26E2 ||   null |   6.26E2 || Reported value within tolerance of calculated value.
        /// | 13 |  null | MS      | 24     |     1.3 |  1.00 |     3.9 |  1.00 |    2.51E2 |    7.49E2 |    36 |    36 |      null ||   null |     null || Check does not apply to non MS-1 calculations
        /// | 14 |    14 | MS      | 24     |     1.3 |  1.00 |     3.9 |  1.00 |    2.48E2 |    7.52E2 |    36 |    36 |    6.26E2 ||   null |   6.26E2 || Reported value outsode of tolerance with calculated value.
        /// | 15 |  null | MS      | 24     |     1.3 |  1.00 |     3.9 |  1.00 |    2.52E2 |    7.48E2 |    36 |    36 |      null ||   null |     null || Check does not apply to non MS-1 calculations
        /// | 16 |    16 | MS      | 24     |     1.3 |  1.00 |     3.9 |  1.00 |      null |    7.50E2 |    36 |    36 |    7.50E2 ||      C |     null || Reported value is null (This condtion was added due to null value error)
        /// | 17 |    17 | MS      | 24     |     5.2 |  0.25 |     5.2 |  0.75 |    2.50E2 |    7.50E2 |    36 |    36 |    5.00E2 ||   null |   5.00E2 || Reported value matches calculated value.
        /// | 18 |    18 | MS      | 24     |     5.2 |  0.25 |     5.2 |  0.75 |    2.49E2 |    7.51E2 |    36 |    36 |    4.80E2 ||   null |   5.00E2 || Reported value within percent error of calculated value.
        /// | 19 |    19 | MS      | 24     |     5.2 |  0.25 |     5.2 |  0.75 |    2.51E2 |    7.49E2 |    36 |    36 |    5.20E2 ||   null |   5.00E2 || Reported value within percent error of calculated value.
        /// | 20 |    20 | MS      | 24     |     5.2 |  0.25 |     5.2 |  0.75 |    2.48E2 |    7.52E2 |    36 |    36 |    4.71E2 ||      A |   5.00E2 || Reported value outsode of percent error of calculated value.
        /// | 21 |    21 | MS      | 24     |     5.2 |  0.25 |     5.2 |  0.75 |    2.52E2 |    7.48E2 |    36 |    36 |    5.26E2 ||      A |   5.00E2 || Reported value outsode of percent error of calculated value.
        /// | 22 |  null | MS      | 24     |     1.3 |  1.00 |     3.9 |  1.00 |    2.52E2 |    7.48E2 |    36 |    36 |      null ||   null |     null || Check does not apply to non MS-1 calculations
        /// | 23 |    23 | MS      | 24     |     1.3 |  1.00 |     3.9 |  1.00 |      null |    7.50E2 |    36 |    36 |      null ||      B |     null || Reported value is null (This condtion was added due to null value error)
        /// | 24 |    24 | MS      | 24     |     5.2 |  0.25 |     5.2 |  0.75 |    2.50E2 |    7.50E2 |    36 |    36 |      null ||      B |     null || Reported value matches calculated value.
        /// | 25 |    25 | MS      | 24     |     5.2 |  0.25 |     5.2 |  0.75 |    2.49E2 |    7.51E2 |    36 |    36 |      null ||      B |     null || Reported value within percent error of calculated value.
        /// | 26 |    26 | MS      | 24     |     5.2 |  0.25 |     5.2 |  0.75 |    2.52E2 |    7.48E2 |    36 |    36 |      null ||      B |     null || Reported value outsode of percent error of calculated value.
        /// | 27 |  null | MS      | 24     |     1.3 |     0 |     3.9 |     0 |    2.52E2 |    7.48E2 |    36 |    36 |      null ||   null |     null || Check does not apply to non MS-1 calculations
        /// | 28 |    28 | MS      | 24     |     1.3 |     0 |     3.9 |     0 |      null |    7.50E2 |    36 |    36 |      null ||   null |     null || Reported value is null (This condtion was added due to null value error)
        /// | 29 |    29 | MS      | 24     |     5.2 |     0 |     5.2 |     0 |    2.50E2 |    7.50E2 |    36 |    36 |      null ||   null |     null || Reported value matches calculated value.
        /// | 30 |    30 | MS      | 24     |     5.2 |     0 |     5.2 |     0 |    2.49E2 |    7.51E2 |    36 |    36 |      null ||   null |     null || Reported value within percent error of calculated value.
        /// | 31 |    31 | MS      | 24     |     5.2 |     0 |     5.2 |     0 |    2.52E2 |    7.48E2 |    36 |    36 |      null ||   null |     null || Reported value outsode of percent error of calculated value.    
        /// | 32 |    32 | MS      | 24     |    null |  1.00 |    10.0 |     0 |    1.00E0 |    9.99E2 |    36 |    36 |    9.99E2 ||      A |   1.00E0 || Calculation can occur if flow does not exist at every MS.
        /// | 33 |    33 | MS      | 24     |    null |  1.00 |    10.0 |     0 |    1.00E0 |    9.99E2 |    36 |    36 |    1.00E0 ||   null |   1.00E0 || Calculation can occur if flow does not exist at every MS.
        /// | 34 |    34 | MS      | 24     |    10.0 |     0 |    null |  1.00 |    1.00E0 |    9.99E2 |    36 |    36 |    1.00E0 ||      A |   9.99E2 || Calculation can occur if flow does not exist at every MS.
        /// | 35 |    35 | MS      | 24     |    10.0 |     0 |    null |  1.00 |    1.00E0 |    9.99E2 |    36 |    36 |    9.99E2 ||   null |   9.99E2 || Calculation can occur if flow does not exist at every MS.
        /// </summary>
        [TestMethod]
        public void HourApp12()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            /* Currently cannot access arrays using the new check parameter access. */
            category.Process.ProcessParameters.RegisterParameter(4648, "Apportionment_HF_Rate_Array");
            category.Process.ProcessParameters.RegisterParameter(2232, "Apportionment_Optime_Array");
            category.Process.ProcessParameters.RegisterParameter(3615, "Apportionment_Stack_Flow_Array");
            category.Process.ProcessParameters.RegisterParameter(4673, "MATS_MS1_HF_MODC_Code_Array");
            category.Process.ProcessParameters.RegisterParameter(3618, "Location_Name_Array");


            /* Input Parameter Values */
            string[] dhvIdList = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", null, "14", null, "16", "17", "18", "19", "20", "21", null, "23", "24", "25", "26", null, "28", "29", "30", "31", "32", "33", "34", "35" };
            string[] configList = { "COMPLEX", "CS", "CSMS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS" };
            int?[] unitIdList = { 24, 24, 24, null, 24, 24, 24, 24, 25, 25, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24 };
            decimal?[] flowMs1List = { 10.0m, 10.0m, 10.0m, 10.0m, null, 10.0m, null, 10.0m, 0.0m, -0.1m, 0.0m, 1.3m, 1.3m, 1.3m, 1.3m, 1.3m, 1.3m, 5.2m, 5.2m, 5.2m, 5.2m, 5.2m, 1.3m, 1.3m, 5.2m, 5.2m, 5.2m, 1.3m, 1.3m, 5.2m, 5.2m, 5.2m, null, null, 10.0m, 10.0m };
            decimal[] opTimeMs1List = { 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 0.25m, 0.25m, 0.25m, 0.25m, 0.25m, 1.00m, 1.00m, 0.25m, 0.25m, 0.25m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 1.00m, 1.00m, 0.00m, 0.00m };
            decimal?[] flowMs2List = { 10.0m, 10.0m, 10.0m, 10.0m, 10.0m, null, null, 10.0m, 0.0m, 0.0m, -0.1m, 3.9m, 3.9m, 3.9m, 3.9m, 3.9m, 3.9m, 5.2m, 5.2m, 5.2m, 5.2m, 5.2m, 3.9m, 3.9m, 5.2m, 5.2m, 5.2m, 3.9m, 3.9m, 5.2m, 5.2m, 5.2m, 10.0m, 10.0m, null, null };
            decimal[] opTimeMs2List = { 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 0.75m, 0.75m, 0.75m, 0.75m, 0.75m, 1.00m, 1.00m, 0.75m, 0.75m, 0.75m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 1.00m, 1.00m };
            string[] hfRateMs1List = { "1.00E0", "1.00E0", "1.00E0", "1.00E0", "1.00E0", "1.00E0", "1.00E0", "1.00E0", "1.00E0", "1.00E0", "1.00E0", "2.50E2", "2.49E2", "2.51E2", "2.48E2", "2.52E2", null, "2.50E2", "2.49E2", "2.51E2", "2.48E2", "2.52E2", "2.52E2", null, "2.50E2", "2.49E2", "2.52E2", "2.52E2", null, "2.50E2", "2.49E2", "2.52E2", "1.00E0", "1.00E0", "1.00E0", "1.00E0" };
            string[] hfRateMs2List = { "9.99E2", "9.99E2", "9.99E2", "9.99E2", "9.99E2", "9.99E2", "9.99E2", "9.99E2", "9.99E2", "9.99E2", "9.99E2", "7.50E2", "7.51E2", "7.49E2", "7.52E2", "7.48E2", "7.50E2", "7.50E2", "7.51E2", "7.49E2", "7.52E2", "7.48E2", "7.48E2", "7.50E2", "7.50E2", "7.51E2", "7.48E2", "7.48E2", "7.50E2", "7.50E2", "7.51E2", "7.48E2", "9.99E2", "9.99E2", "9.99E2", "9.99E2" };
            string[] unadjustedValueList = { null, null, null, null, "9.99E2", "1.00E0", null, "5.00E2", null, null, null, "6.25E2", "6.26E2", null, "6.26E2", null, "7.50E2", "5.00E2", "4.80E2", "5.20E2", "4.71E2", "5.26E2", null, null, null, null, null, null, null, null, null, null, "9.99E2", "1.00E0", "1.00E0", "9.99E2" };
            string[] modcCd1List = { "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36" };
            string[] modcCd2List = { "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36" };

            /* Expected Values */
            string[] expectedResultList = { null, null, null, null, "C", "C", "B", null, "B", "B", "B", null, null, null, null, null, "C", null, null, null, "A", "A", null, "B", "B", "B", "B", null, null, null, null, null, "A", null, "A", null };
            string[] expCalcFlowList = { null, null, null, null, null, null, null, "5.00E2", null, null, null, "6.25E2", "6.26E2", null, "6.26E2", null, null, "5.00E2", "5.00E2", "5.00E2", "5.00E2", "5.00E2", null, null, null, null, null, null, null, null, null, null, "1.00E0", "1.00E0", "9.99E2", "9.99E2" };

            /* Test Case Count */
            int caseCount = 36;

            /* Check array lengths */
            Assert.AreEqual(caseCount, dhvIdList.Length, "dhvIdList length");
            Assert.AreEqual(caseCount, configList.Length, "configList length");
            Assert.AreEqual(caseCount, unitIdList.Length, "unitIdList length");
            Assert.AreEqual(caseCount, flowMs1List.Length, "flowMs1List length");
            Assert.AreEqual(caseCount, opTimeMs1List.Length, "opTimeMs1List length");
            Assert.AreEqual(caseCount, flowMs2List.Length, "flowMs2List length");
            Assert.AreEqual(caseCount, opTimeMs2List.Length, "opTimeMs2List length");
            Assert.AreEqual(caseCount, hfRateMs1List.Length, "hfRateMs1List length");
            Assert.AreEqual(caseCount, hfRateMs2List.Length, "hfRateMs2List length");
            Assert.AreEqual(caseCount, unadjustedValueList.Length, "unadjustedValueList length");
            Assert.AreEqual(caseCount, modcCd1List.Length, "modcCd1List length");
            Assert.AreEqual(caseCount, modcCd2List.Length, "modcCd2List length");
            Assert.AreEqual(caseCount, expectedResultList.Length, "expectedResultList length");
            Assert.AreEqual(caseCount, expCalcFlowList.Length, "expCalcFlowList length");

            /* Run Cases */
            for (int caseDex = 0; caseDex < caseCount; caseDex++)
            {
                /*  Initialize Required Parameters */
                EmParameters.CurrentMonitorPlanLocationPostion = 1;
                EmParameters.CurrentMonitorPlanLocationRecord = new VwCeMpMonitorLocationRow(unitId: unitIdList[caseDex]);
                EmParameters.MpStackConfigForHourlyChecks = configList[caseDex];
                EmParameters.MatsMs1HfDhvId = dhvIdList[caseDex];
                EmParameters.MatsMs1HfUnadjustedHourlyValue = unadjustedValueList[caseDex];

                category.SetCheckParameter("Apportionment_HF_Rate_Array", new string[] { null, hfRateMs1List[caseDex], hfRateMs2List[caseDex] });
                category.SetCheckParameter("Apportionment_OpTime_Array", new decimal[] { Math.Max(opTimeMs1List[caseDex], opTimeMs2List[caseDex]), opTimeMs1List[caseDex], opTimeMs2List[caseDex] });
                category.SetCheckParameter("Apportionment_Stack_Flow_Array", new decimal?[] { null, flowMs1List[caseDex], flowMs2List[caseDex] });
                category.SetCheckParameter("MATS_MS1_HF_MODC_Code_Array", new string[] { null, modcCd1List[caseDex], modcCd2List[caseDex] });
                category.SetCheckParameter("Location_Name_Array", new string[] { "UN", "MS1", "MS2" });

                /*  Initialize Output Parameters */
                EmParameters.CalculatedFlowWeightedHf = "-1.23E4";
                EmParameters.MatsReportedPluginHf = "-2.23E4";
                EmParameters.MatsParameterPluginHf = "UNKNOWN";

                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Initialize variables needed to run the check. */
                bool log = false;
                string actual;

                /* Run Check */
                actual = cHourlyApportionmentChecks.HOURAPP12(category, ref log);

                /* Check results */
                Assert.AreEqual(string.Empty, actual, string.Format("actual {0}", caseDex));
                Assert.AreEqual(false, log, string.Format("log {0}", caseDex));
                Assert.AreEqual(expectedResultList[caseDex], category.CheckCatalogResult, string.Format("Result {0}", caseDex));
                Assert.AreEqual(EmParameters.MatsReportedPluginHf, unadjustedValueList[caseDex], string.Format("MATS Reported {0}", caseDex));
                Assert.AreEqual(expCalcFlowList[caseDex], EmParameters.CalculatedFlowWeightedHf, string.Format("CalculatedFlowWeightedHf [case {0}]", caseDex));
            }
        }

        /// <summary>
        /// 
        /// 
        /// | ## | DhvId | Config  | UnitId | FlowMs1 | OpMs1 | FlowMs2 | OpMs2 |   RateMs1 |   RateMs2 | MODC1 | MODC2 | UnadjHrly || Result | CalcFlow || Note
        /// |  0 |     0 | COMPLEX | 24     |    10.0 |  1.00 |    10.0 |  1.00 |    1.00E0 |    9.99E2 |    36 |    36 |      null ||   null |     null || Check does not apply to config type.
        /// |  1 |     1 | CS      | 24     |    10.0 |  1.00 |    10.0 |  1.00 |    1.00E0 |    9.99E2 |    36 |    36 |      null ||   null |     null || Check does not apply to config type.
        /// |  2 |     2 | CSMS    | 24     |    10.0 |  1.00 |    10.0 |  1.00 |    1.00E0 |    9.99E2 |    36 |    36 |      null ||   null |     null || Check does not apply to config type.
        /// |  3 |     3 | MS      | null   |    10.0 |  1.00 |    10.0 |  1.00 |    1.00E0 |    9.99E2 |    36 |    36 |      null ||   null |     null || Check does not apply to units.
        /// |  4 |     4 | MS      | 24     |    null |  1.00 |    10.0 |  1.00 |    1.00E0 |    9.99E2 |    36 |    36 |    9.99E2 ||      C |     null || Calculation can occur if flow does not exist at every MS.
        /// |  5 |     5 | MS      | 24     |    10.0 |  1.00 |    null |  1.00 |    1.00E0 |    9.99E2 |    36 |    36 |    1.00E0 ||      C |     null || Calculation can occur if flow does not exist at every MS.
        /// |  6 |     6 | MS      | 24     |    null |  1.00 |    null |  1.00 |    1.00E0 |    9.99E2 |    36 |    36 |      null ||      B |     null || Calculation cannot occur if flow does not exist at any MS.
        /// |  7 |     7 | MS      | 24     |    10.0 |  1.00 |    10.0 |  1.00 |    1.00E0 |    9.99E2 |    36 |    36 |    5.00E2 ||   null |   5.00E2 || Calculation con occur if load does not exist at the unit.
        /// |  8 |     8 | MS      | 25     |     0.0 |  1.00 |     0.0 |  1.00 |    1.00E0 |    9.99E2 |    36 |    36 |    5.00E2 ||      B |     null || Calculation connot occur if the sum of the MS stack flow is equal to zero. Should this cause an error? It does not right now.
        /// |  9 |     9 | MS      | 25     |    -0.1 |  1.00 |     0.0 |  1.00 |    1.00E0 |    9.99E2 |    36 |    36 |      null ||      B |     null || Calculation connot occur if the sum of the MS stack flow is less than zero.
        /// | 10 |    10 | MS      | 24     |     0.0 |  1.00 |    -0.1 |  1.00 |    1.00E0 |    9.99E2 |    36 |    36 |      null ||      B |     null || Calculation connot occur if the sum of the MS stack flow is less than zero.
        /// | 11 |    11 | MS      | 24     |     1.3 |  1.00 |     3.9 |  1.00 |    2.50E2 |    7.50E2 |    36 |    36 |    6.25E2 ||   null |   6.25E2 || Reported value matches calculated value.
        /// | 12 |    12 | MS      | 24     |     1.3 |  1.00 |     3.9 |  1.00 |    2.49E2 |    7.51E2 |    36 |    36 |    6.26E2 ||   null |   6.26E2 || Reported value within tolerance of calculated value.
        /// | 13 |  null | MS      | 24     |     1.3 |  1.00 |     3.9 |  1.00 |    2.51E2 |    7.49E2 |    36 |    36 |      null ||   null |     null || Check does not apply to non MS-1 calculations
        /// | 14 |    14 | MS      | 24     |     1.3 |  1.00 |     3.9 |  1.00 |    2.48E2 |    7.52E2 |    36 |    36 |    6.26E2 ||   null |   6.26E2 || Reported value outsode of tolerance with calculated value.
        /// | 15 |  null | MS      | 24     |     1.3 |  1.00 |     3.9 |  1.00 |    2.52E2 |    7.48E2 |    36 |    36 |      null ||   null |     null || Check does not apply to non MS-1 calculations
        /// | 16 |    16 | MS      | 24     |     1.3 |  1.00 |     3.9 |  1.00 |      null |    7.50E2 |    36 |    36 |    7.50E2 ||      C |     null || Reported value is null (This condtion was added due to null value error)
        /// | 17 |    17 | MS      | 24     |     5.2 |  0.25 |     5.2 |  0.75 |    2.50E2 |    7.50E2 |    36 |    36 |    5.00E2 ||   null |   5.00E2 || Reported value matches calculated value.
        /// | 18 |    18 | MS      | 24     |     5.2 |  0.25 |     5.2 |  0.75 |    2.49E2 |    7.51E2 |    36 |    36 |    4.80E2 ||   null |   5.00E2 || Reported value within percent error of calculated value.
        /// | 19 |    19 | MS      | 24     |     5.2 |  0.25 |     5.2 |  0.75 |    2.51E2 |    7.49E2 |    36 |    36 |    5.20E2 ||   null |   5.00E2 || Reported value within percent error of calculated value.
        /// | 20 |    20 | MS      | 24     |     5.2 |  0.25 |     5.2 |  0.75 |    2.48E2 |    7.52E2 |    36 |    36 |    4.71E2 ||      A |   5.00E2 || Reported value outsode of percent error of calculated value.
        /// | 21 |    21 | MS      | 24     |     5.2 |  0.25 |     5.2 |  0.75 |    2.52E2 |    7.48E2 |    36 |    36 |    5.26E2 ||      A |   5.00E2 || Reported value outsode of percent error of calculated value.
        /// | 22 |  null | MS      | 24     |     1.3 |  1.00 |     3.9 |  1.00 |    2.52E2 |    7.48E2 |    36 |    36 |      null ||   null |     null || Check does not apply to non MS-1 calculations
        /// | 23 |    23 | MS      | 24     |     1.3 |  1.00 |     3.9 |  1.00 |      null |    7.50E2 |    36 |    36 |      null ||      B |     null || Reported value is null (This condtion was added due to null value error)
        /// | 24 |    24 | MS      | 24     |     5.2 |  0.25 |     5.2 |  0.75 |    2.50E2 |    7.50E2 |    36 |    36 |      null ||      B |     null || Reported value matches calculated value.
        /// | 25 |    25 | MS      | 24     |     5.2 |  0.25 |     5.2 |  0.75 |    2.49E2 |    7.51E2 |    36 |    36 |      null ||      B |     null || Reported value within percent error of calculated value.
        /// | 26 |    26 | MS      | 24     |     5.2 |  0.25 |     5.2 |  0.75 |    2.52E2 |    7.48E2 |    36 |    36 |      null ||      B |     null || Reported value outsode of percent error of calculated value.
        /// | 27 |  null | MS      | 24     |     1.3 |     0 |     3.9 |     0 |    2.52E2 |    7.48E2 |    36 |    36 |      null ||   null |     null || Check does not apply to non MS-1 calculations
        /// | 28 |    28 | MS      | 24     |     1.3 |     0 |     3.9 |     0 |      null |    7.50E2 |    36 |    36 |      null ||   null |     null || Reported value is null (This condtion was added due to null value error)
        /// | 29 |    29 | MS      | 24     |     5.2 |     0 |     5.2 |     0 |    2.50E2 |    7.50E2 |    36 |    36 |      null ||   null |     null || Reported value matches calculated value.
        /// | 30 |    30 | MS      | 24     |     5.2 |     0 |     5.2 |     0 |    2.49E2 |    7.51E2 |    36 |    36 |      null ||   null |     null || Reported value within percent error of calculated value.
        /// | 31 |    31 | MS      | 24     |     5.2 |     0 |     5.2 |     0 |    2.52E2 |    7.48E2 |    36 |    36 |      null ||   null |     null || Reported value outsode of percent error of calculated value.    
        /// | 32 |    32 | MS      | 24     |    null |  1.00 |    10.0 |     0 |    1.00E0 |    9.99E2 |    36 |    36 |    9.99E2 ||      A |   1.00E0 || Calculation can occur if flow does not exist at every MS.
        /// | 33 |    33 | MS      | 24     |    null |  1.00 |    10.0 |     0 |    1.00E0 |    9.99E2 |    36 |    36 |    1.00E0 ||   null |   1.00E0 || Calculation can occur if flow does not exist at every MS.
        /// | 34 |    34 | MS      | 24     |    10.0 |     0 |    null |  1.00 |    1.00E0 |    9.99E2 |    36 |    36 |    1.00E0 ||      A |   9.99E2 || Calculation can occur if flow does not exist at every MS.
        /// | 35 |    35 | MS      | 24     |    10.0 |     0 |    null |  1.00 |    1.00E0 |    9.99E2 |    36 |    36 |    9.99E2 ||   null |   9.99E2 || Calculation can occur if flow does not exist at every MS.
        /// </summary>
        [TestMethod]
        public void HourApp13()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            /* Currently cannot access arrays using the new check parameter access. */
            category.Process.ProcessParameters.RegisterParameter(4649, "Apportionment_SO2_Rate_Array");
            category.Process.ProcessParameters.RegisterParameter(2232, "Apportionment_Optime_Array");
            category.Process.ProcessParameters.RegisterParameter(3615, "Apportionment_Stack_Flow_Array");
            category.Process.ProcessParameters.RegisterParameter(4673, "MATS_MS1_SO2_MODC_Code_Array");
            category.Process.ProcessParameters.RegisterParameter(3618, "Location_Name_Array");


            /* Input Parameter Values */
            string[] dhvIdList = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", null, "14", null, "16", "17", "18", "19", "20", "21", null, "23", "24", "25", "26", null, "28", "29", "30", "31", "32", "33", "34", "35" };
            string[] configList = { "COMPLEX", "CS", "CSMS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS", "MS" };
            int?[] unitIdList = { 24, 24, 24, null, 24, 24, 24, 24, 25, 25, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24 };
            decimal?[] flowMs1List = { 10.0m, 10.0m, 10.0m, 10.0m, null, 10.0m, null, 10.0m, 0.0m, -0.1m, 0.0m, 1.3m, 1.3m, 1.3m, 1.3m, 1.3m, 1.3m, 5.2m, 5.2m, 5.2m, 5.2m, 5.2m, 1.3m, 1.3m, 5.2m, 5.2m, 5.2m, 1.3m, 1.3m, 5.2m, 5.2m, 5.2m, null, null, 10.0m, 10.0m };
            decimal[] opTimeMs1List = { 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 0.25m, 0.25m, 0.25m, 0.25m, 0.25m, 1.00m, 1.00m, 0.25m, 0.25m, 0.25m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 1.00m, 1.00m, 0.00m, 0.00m };
            decimal?[] flowMs2List = { 10.0m, 10.0m, 10.0m, 10.0m, 10.0m, null, null, 10.0m, 0.0m, 0.0m, -0.1m, 3.9m, 3.9m, 3.9m, 3.9m, 3.9m, 3.9m, 5.2m, 5.2m, 5.2m, 5.2m, 5.2m, 3.9m, 3.9m, 5.2m, 5.2m, 5.2m, 3.9m, 3.9m, 5.2m, 5.2m, 5.2m, 10.0m, 10.0m, null, null };
            decimal[] opTimeMs2List = { 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 1.00m, 0.75m, 0.75m, 0.75m, 0.75m, 0.75m, 1.00m, 1.00m, 0.75m, 0.75m, 0.75m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 1.00m, 1.00m };
            string[] so2RateMs1List = { "1.00E0", "1.00E0", "1.00E0", "1.00E0", "1.00E0", "1.00E0", "1.00E0", "1.00E0", "1.00E0", "1.00E0", "1.00E0", "2.50E2", "2.49E2", "2.51E2", "2.48E2", "2.52E2", null, "2.50E2", "2.49E2", "2.51E2", "2.48E2", "2.52E2", "2.52E2", null, "2.50E2", "2.49E2", "2.52E2", "2.52E2", null, "2.50E2", "2.49E2", "2.52E2", "1.00E0", "1.00E0", "1.00E0", "1.00E0" };
            string[] so2RateMs2List = { "9.99E2", "9.99E2", "9.99E2", "9.99E2", "9.99E2", "9.99E2", "9.99E2", "9.99E2", "9.99E2", "9.99E2", "9.99E2", "7.50E2", "7.51E2", "7.49E2", "7.52E2", "7.48E2", "7.50E2", "7.50E2", "7.51E2", "7.49E2", "7.52E2", "7.48E2", "7.48E2", "7.50E2", "7.50E2", "7.51E2", "7.48E2", "7.48E2", "7.50E2", "7.50E2", "7.51E2", "7.48E2", "9.99E2", "9.99E2", "9.99E2", "9.99E2" };
            string[] unadjustedValueList = { null, null, null, null, "9.99E2", "1.00E0", null, "5.00E2", null, null, null, "6.25E2", "6.26E2", null, "6.26E2", null, "7.50E2", "5.00E2", "4.80E2", "5.20E2", "4.71E2", "5.26E2", null, null, null, null, null, null, null, null, null, null, "9.99E2", "1.00E0", "1.00E0", "9.99E2" };
            string[] modcCd1List = { "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36" };
            string[] modcCd2List = { "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36", "36" };

            /* Expected Values */
            string[] expectedResultList = { null, null, null, null, "C", "C", "B", null, "B", "B", "B", null, null, null, null, null, "C", null, null, null, "A", "A", null, "B", "B", "B", "B", null, null, null, null, null, "A", null, "A", null };
            string[] expCalcFlowList = { null, null, null, null, null, null, null, "5.00E2", null, null, null, "6.25E2", "6.26E2", null, "6.26E2", null, null, "5.00E2", "5.00E2", "5.00E2", "5.00E2", "5.00E2", null, null, null, null, null, null, null, null, null, null, "1.00E0", "1.00E0", "9.99E2", "9.99E2" };

            /* Test Case Count */
            int caseCount = 36;

            /* Check array lengths */
            Assert.AreEqual(caseCount, dhvIdList.Length, "dhvIdList length");
            Assert.AreEqual(caseCount, configList.Length, "configList length");
            Assert.AreEqual(caseCount, unitIdList.Length, "unitIdList length");
            Assert.AreEqual(caseCount, flowMs1List.Length, "flowMs1List length");
            Assert.AreEqual(caseCount, opTimeMs1List.Length, "opTimeMs1List length");
            Assert.AreEqual(caseCount, flowMs2List.Length, "flowMs2List length");
            Assert.AreEqual(caseCount, opTimeMs2List.Length, "opTimeMs2List length");
            Assert.AreEqual(caseCount, so2RateMs1List.Length, "so2RateMs1List length");
            Assert.AreEqual(caseCount, so2RateMs2List.Length, "so2RateMs2List length");
            Assert.AreEqual(caseCount, unadjustedValueList.Length, "unadjustedValueList length");
            Assert.AreEqual(caseCount, modcCd1List.Length, "modcCd1List length");
            Assert.AreEqual(caseCount, modcCd2List.Length, "modcCd2List length");
            Assert.AreEqual(caseCount, expectedResultList.Length, "expectedResultList length");
            Assert.AreEqual(caseCount, expCalcFlowList.Length, "expCalcFlowList length");

            /* Run Cases */
            for (int caseDex = 0; caseDex < caseCount; caseDex++)
            {
                /*  Initialize Required Parameters */
                EmParameters.CurrentMonitorPlanLocationPostion = 1;
                EmParameters.CurrentMonitorPlanLocationRecord = new VwCeMpMonitorLocationRow(unitId: unitIdList[caseDex]);
                EmParameters.MpStackConfigForHourlyChecks = configList[caseDex];
                EmParameters.MatsMs1So2DhvId = dhvIdList[caseDex];
                EmParameters.MatsMs1So2UnadjustedHourlyValue = unadjustedValueList[caseDex];

                category.SetCheckParameter("Apportionment_SO2_Rate_Array", new string[] { null, so2RateMs1List[caseDex], so2RateMs2List[caseDex] });
                category.SetCheckParameter("Apportionment_OpTime_Array", new decimal[] { Math.Max(opTimeMs1List[caseDex], opTimeMs2List[caseDex]), opTimeMs1List[caseDex], opTimeMs2List[caseDex] });
                category.SetCheckParameter("Apportionment_Stack_Flow_Array", new decimal?[] { null, flowMs1List[caseDex], flowMs2List[caseDex] });
                category.SetCheckParameter("MATS_MS1_SO2_MODC_Code_Array", new string[] { null, modcCd1List[caseDex], modcCd2List[caseDex] });
                category.SetCheckParameter("Location_Name_Array", new string[] { "UN", "MS1", "MS2" });

                /*  Initialize Output Parameters */
                EmParameters.CalculatedFlowWeightedSo2 = "-1.23E4";
                EmParameters.MatsReportedPluginSo2 = "-2.23E4";
                EmParameters.MatsParameterPluginSo2 = "UNKNOWN";

                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Initialize variables needed to run the check. */
                bool log = false;
                string actual;

                /* Run Check */
                actual = cHourlyApportionmentChecks.HOURAPP13(category, ref log);

                /* Check results */
                Assert.AreEqual(string.Empty, actual, string.Format("actual {0}", caseDex));
                Assert.AreEqual(false, log, string.Format("log {0}", caseDex));
                Assert.AreEqual(expectedResultList[caseDex], category.CheckCatalogResult, string.Format("Result {0}", caseDex));
                Assert.AreEqual(EmParameters.MatsReportedPluginSo2, unadjustedValueList[caseDex], string.Format("MATS Reported {0}", caseDex));
                Assert.AreEqual(expCalcFlowList[caseDex], EmParameters.CalculatedFlowWeightedSo2, string.Format("CalculatedFlowWeightedSo2 [case {0}]", caseDex));
            }
        }

        /// <summary>
        /// 
        /// Tolerance     : 1.1
        /// LocationCount : 4
        /// 
        /// | ## | Config  | StkAcc | UntAcc || Result || Note
        /// |  0 | null    |   40.5 |   36.0 || null   || Not checked because config is null.   
        /// |  1 | COMPLEX |   40.5 |   36.0 || A      || Discrepancy between 40.5 and 36.0.
        /// |  2 | CS      |   40.5 |   36.0 || null   || Not checked because config is CS.
        /// |  3 | CSMS    |   40.5 |   36.0 || A      || Discrepancy between 40.5 and 36.0.
        /// |  4 | MS      |   40.5 |   36.0 || null   || Not checked because config is MS.   
        /// |  5 | COMPLEX |   40.5 |   null || null   || Not checked because a previous error exists.
        /// |  6 | CSMS    |   40.5 |   null || null   || Not checked because a previous error exists.
        /// |  7 | COMPLEX |   40.5 |   -1.0 || null   || Not checked because a previous error exists.
        /// |  8 | CSMS    |   40.5 |   -1.0 || null   || Not checked because a previous error exists.
        /// |  9 | COMPLEX |   null |   36.0 || null   || Not checked because a previous error exists.
        /// | 10 | CSMS    |   null |   36.0 || null   || Not checked because a previous error exists.
        /// | 11 | COMPLEX |   -1.0 |   36.0 || null   || Not checked because a previous error exists.
        /// | 12 | CSMS    |   -1.0 |   36.0 || null   || Not checked because a previous error exists.
        /// | 13 | COMPLEX |   40.5 |   36.1 || null   || 40.5 and 36.1 are within tolerance of 4.4.
        /// | 14 | CSMS    |   40.5 |   36.1 || null   || 40.5 and 36.1 are within tolerance of 4.4.
        /// </summary>
        [TestMethod()]
        public void HourApp14()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;


            /* Input Parameter Values */
            string[] configList = { null, "COMPLEX", "CS", "CSMS", "MS", "COMPLEX", "CSMS", "COMPLEX", "CSMS", "COMPLEX", "CSMS", "COMPLEX", "CSMS", "COMPLEX", "CSMS" };
            decimal?[] stkAccumList = { 40.5m, 40.5m, 40.5m, 40.5m, 40.5m, 40.5m, 40.5m, 40.5m, 40.5m, null, null, -1.0m, -1.0m, 40.5m, 40.5m };
            decimal?[] untAccumList = { 36.0m, 36.0m, 36.0m, 36.0m, 36.0m, null, null, -1.0m, -1.0m, 36.0m, 36.0m, 36.0m, 36.0m, 36.1m, 36.1m };
            /* Expected Values */
            string[] expectedResultList = { null, "A", null, "A", null, null, null, null, null, null, null, null, null, null, null };

            /* Test Case Count */
            int caseCount = 15;

            /* Check array lengths */
            Assert.AreEqual(caseCount, configList.Length, "configList length");
            Assert.AreEqual(caseCount, stkAccumList.Length, "stkAccumList length");
            Assert.AreEqual(caseCount, untAccumList.Length, "untAccumList length");
            Assert.AreEqual(caseCount, expectedResultList.Length, "expectedResultList length");

            /* Run Cases */
            for (int caseDex = 0; caseDex < caseCount; caseDex++)
            {
                /*  Initialize Required Parameters */
                EmParameters.CurrentLocationCount = 4;
                EmParameters.HourlyEmissionsTolerancesCrossCheckTable = new CheckDataView<HourlyEmissionsTolerancesRow>();
                {
                    EmParameters.HourlyEmissionsTolerancesCrossCheckTable.Add(new HourlyEmissionsTolerancesRow(parameter: "HI", uOM: "MMBTUHR", tolerance: "1.1"));
                }
                EmParameters.MpStackConfigForHourlyChecks = configList[caseDex];
                EmParameters.StackHeatinputtimesoptimeAccumulator = stkAccumList[caseDex];
                EmParameters.UnitHeatinputtimesoptimeAccumulator = untAccumList[caseDex];

                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Initialize variables needed to run the check. */
                bool log = false;
                string actual;

                /* Run Check */
                actual = cHourlyApportionmentChecks.HOURAPP14(category, ref log);

                /* Check results */
                Assert.AreEqual(string.Empty, actual, string.Format("actual {0}", caseDex));
                Assert.AreEqual(false, log, string.Format("log {0}", caseDex));
                Assert.AreEqual(expectedResultList[caseDex], category.CheckCatalogResult, string.Format("Result {0}", caseDex));
            }
        }
    }
}
