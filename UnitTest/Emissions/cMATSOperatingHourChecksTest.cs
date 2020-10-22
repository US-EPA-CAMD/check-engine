using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.EmissionsChecks;
using ECMPS.Checks.EmissionsReport;
using ECMPS.Definitions.Extensions;

using ECMPS.Checks.Data.Ecmps.CheckEm.Function;
using ECMPS.Checks.Data.Ecmps.Dbo.Table;
using ECMPS.Checks.Data.Ecmps.Dbo.View;
using ECMPS.Checks.Data.EcmpsAux.CrossCheck.Virtual;
using ECMPS.Checks.Data.Ecmps.CrossCheck.Table;
using ECMPS.Checks.Em.Parameters;

using UnitTest.UtilityClasses;
using ECMPS.Checks.Parameters;

namespace UnitTest.Emissions
{
    [TestClass()]
    public class cMATSOperatingHourChecksTest
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

        #region MATSHOD Checks 1-10

        /// <summary>
        /// 
        /// 
        /// | ## | Needed | Key | Param | Meth | Key | Param | Meth  | Allowed || Result | Record | Param | Meth  | Allowed || Note
        /// |  0 | true   | ONE | HGRE  | CEM  | TWO | HGC   | ST    | false   || null   | true   | HGRE  | CEM   | false   || One HGRE method exists.
        /// |  1 | true   | ONE | HGRH  | CEM  | TWO | HGC   | ST    | false   || null   | true   | HGRH  | CEM   | false   || One HGRH method exists.
        /// |  2 | false  | ONE | HGRE  | CEM  | TWO | HGC   | ST    | false   || null   | false  | null  | null  | false   || DerivedHourlyChecksNeeded is false.
        /// |  3 | null   | ONE | HGRE  | CEM  | TWO | HGC   | ST    | false   || null   | false  | null  | null  | false   || DerivedHourlyChecksNeeded is false.
        /// |  4 | true   | ONE | HGC   | CEM  | TWO | HGC   | ST    | false   || null   | false  | null  | null  | false   || No valid HGRE or HGRH methods exist.
        /// |  5 | true   | ONE | HGRE  | CEM  | TWO | HGRH  | ST    | false   || A      | false  | null  | null  | false   || A valid HGRE and a valid HGRH method exist.
        /// |  6 | true   | ONE | HGC   | CEM  | TWO | HGRE  | ST    | false   || null   | true   | HGRE  | ST    | true    || One HGRE ST method exists.
        /// |  7 | true   | ONE | HGC   | CEM  | TWO | HGRH  | ST    | false   || null   | true   | HGRH  | ST    | true    || One HGRH ST method exists.
        /// |  8 | true   | ONE | HGC   | CEM  | TWO | HGRE  | CEMST | false   || null   | true   | HGRE  | CEMST | true    || One HGRE CEMST method exists.
        /// |  9 | true   | ONE | HGC   | CEM  | TWO | HGRH  | CEMST | false   || null   | true   | HGRH  | CEMST | true    || One HGRH CEMST method exists.
        /// | 10 | true   | ONE | HGRE  | CEM  | TWO | HGC   | ST    | true    || null   | true   | HGRE  | CEM   | true    || One HGRE method exists, with Flow MHV Allowed initially true.
        /// </summary>
        [TestMethod()]
        public void MatsHod1()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            /* Input Parameter Values */
            bool?[] neededList = { true, true, false, null, true, true, true, true, true, true, true };
            string[] mth1ParamList = { "HGRE", "HGRH", "HGRE", "HGRE", "HGC", "HGRE", "HGC", "HGC", "HGC", "HGC", "HGRE" };
            string[] mth1CodeList = { "CEM", "CEM", "CEM", "CEM", "CEM", "CEM", "CEM", "CEM", "CEM", "CEM", "CEM" };
            string[] mth2ParamList = { "HGC", "HGC", "HGC", "HGC", "HGC", "HGRH", "HGRE", "HGRH", "HGRE", "HGRH", "HGC" };
            string[] mth2CodeList = { "ST", "ST", "ST", "ST", "ST", "ST", "ST", "ST", "CEMST", "CEMST", "ST" };
            bool?[] allowedList = { false, false, false, false, false, false, false, false, false, false, true };

            /* Expected Values */
            string[] resultList = { null, null, null, null, null, "A", null, null, null, null, null };
            bool[] expRecord = { true, true, false, false, false, false, true, true, true, true, true };
            string[] expParamList = { "HGRE", "HGRH", null, null, null, null, "HGRE", "HGRH", "HGRE", "HGRH", "HGRE" };
            string[] expMethodList = { "CEM", "CEM", null, null, null, null, "ST", "ST", "CEMST", "CEMST", "CEM" };
            bool?[] expAllowedList = { false, false, false, false, false, false, true, true, true, true, true };

            /* Test Case Count */
            int caseCount = 11;

            /* Check array lengths */
            Assert.AreEqual(caseCount, neededList.Length, "neededList length");
            Assert.AreEqual(caseCount, mth1ParamList.Length, "mth1ParamList length");
            Assert.AreEqual(caseCount, mth1CodeList.Length, "mth1CodeList length");
            Assert.AreEqual(caseCount, mth2ParamList.Length, "mth2ParamList length");
            Assert.AreEqual(caseCount, mth2CodeList.Length, "mth2CodeList length");
            Assert.AreEqual(caseCount, allowedList.Length, "allowedList length");
            Assert.AreEqual(caseCount, resultList.Length, "resultList length");
            Assert.AreEqual(caseCount, expRecord.Length, "expRecord length");
            Assert.AreEqual(caseCount, expParamList.Length, "expParamList length");
            Assert.AreEqual(caseCount, expMethodList.Length, "expMethodList length");
            Assert.AreEqual(caseCount, expAllowedList.Length, "expAllowedList length");

            /* Run Cases */
            for (int caseDex = 0; caseDex < caseCount; caseDex++)
            {
                /* Initialize Input Parameters */
                EmParameters.DerivedHourlyChecksNeeded = neededList[caseDex];
                EmParameters.FlowMhvOptionallyAllowed = allowedList[caseDex];
                EmParameters.MonitorMethodRecordsByHourLocation = new CheckDataView<VwMpMonitorMethodRow>();
                {
                    if (mth1ParamList[caseDex] != null)
                        EmParameters.MonitorMethodRecordsByHourLocation.Add(new VwMpMonitorMethodRow(monLocId: "ONE", parameterCd: mth1ParamList[caseDex], methodCd: mth1CodeList[caseDex]));
                    if (mth2ParamList[caseDex] != null)
                        EmParameters.MonitorMethodRecordsByHourLocation.Add(new VwMpMonitorMethodRow(monLocId: "TWO", parameterCd: mth2ParamList[caseDex], methodCd: mth2CodeList[caseDex]));
                }

                /* Initialize Output Parameters */
                EmParameters.MatsHgMethodCode = "OLD";
                EmParameters.MatsHgMethodRecord = new VwMpMonitorMethodRow(monLocId: "OLD", parameterCd: "OLD", methodCd: "OLD");
                EmParameters.MatsHgParameterCode = "OLD";

                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Initialize variables needed to run the check. */
                bool log = false;
                string actual;

                /* Run Check */
                actual = cMATSOperatingHourChecks.MATSHOD1(category, ref log);

                /* Check Results */
                Assert.AreEqual(string.Empty, actual, string.Format("actual[case {0}]", caseDex));
                Assert.AreEqual(false, log, string.Format("log [case {0}]", caseDex));
                Assert.AreEqual(resultList[caseDex], category.CheckCatalogResult, string.Format("category.CheckCatalogResult [case {0}]", caseDex));

                Assert.AreEqual(expMethodList[caseDex], EmParameters.MatsHgMethodCode, string.Format("MatsHgMethodCode [case {0}]", caseDex));
                Assert.AreEqual(expRecord[caseDex], (EmParameters.MatsHgMethodRecord != null), string.Format("MatsHgMethodRecord [case {0}]", caseDex));
                Assert.AreEqual(expParamList[caseDex], EmParameters.MatsHgParameterCode, string.Format("MatsHgParameterCode [case {0}]", caseDex));
                Assert.AreEqual(expAllowedList[caseDex], EmParameters.FlowMhvOptionallyAllowed, string.Format("FlowMhvOptionallyAllowed [case {0}]", caseDex));
            }
        }

        /// <summary>
        /// 
        /// 
        /// | ## | Needed | Key | Param | Meth | Key | Param | Meth || Result | Record | Param | Meth || Note
        /// |  0 | true   | ONE | HCLRE | TSTM | TWO | HCLC  | BAD  || null   | true   | HCLRE | TSTM || One HCLRE method exists.
        /// |  1 | true   | ONE | HCLRH | TSTM | TWO | HCLC  | BAD  || null   | true   | HCLRH | TSTM || One HCLRH method exists.
        /// |  2 | false  | ONE | HCLRE | TSTM | TWO | HCLC  | BAD  || null   | false  | null  | null || DerivedHourlyChecksNeeded is false.
        /// |  3 | null   | ONE | HCLRE | TSTM | TWO | HCLC  | BAD  || null   | false  | null  | null || DerivedHourlyChecksNeeded is false.
        /// |  4 | true   | ONE | HCLC  | BAD  | TWO | HCLC  | BAD  || null   | false  | null  | null || No valid HCL methods exist.
        /// |  5 | true   | ONE | HCLRE | TSTM | TWO | HCLRH | TSTM || A      | false  | null  | null || Two valid HCL methods exist.
        /// </summary>
        [TestMethod()]
        public void MatsHod2()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            /* Input Parameter Values */
            bool?[] neededList = { true, true, false, null, true, true };
            string[] mth1ParamList = { "HCLRE", "HCLRH", "HCLRE", "HCLRE", "HCLC", "HCLRE" };
            string[] mth1CodeList = { "TSTM", "TSTM", "TSTM", "TSTM", "BAD", "TSTM" };
            string[] mth2ParamList = { "HCLC", "HCLC", "HCLC", "HCLC", "HCLC", "HCLRH" };
            string[] mth2CodeList = { "BAD", "BAD", "BAD", "BAD", "BAD", "TSTM" };

            /* Expected Values */
            string[] resultList = { null, null, null, null, null, "A" };
            bool[] expRecord = { true, true, false, false, false, false };
            string[] expParamList = { "HCLRE", "HCLRH", null, null, null, null };
            string[] expMethodList = { "TSTM", "TSTM", null, null, null, null };

            /* Test Case Count */
            int caseCount = 6;

            /* Check array lengths */
            Assert.AreEqual(caseCount, neededList.Length, "neededList length");
            Assert.AreEqual(caseCount, mth1ParamList.Length, "mth1ParamList length");
            Assert.AreEqual(caseCount, mth1CodeList.Length, "mth1CodeList length");
            Assert.AreEqual(caseCount, mth2ParamList.Length, "mth2ParamList length");
            Assert.AreEqual(caseCount, mth2CodeList.Length, "mth2CodeList length");
            Assert.AreEqual(caseCount, resultList.Length, "resultList length");
            Assert.AreEqual(caseCount, expRecord.Length, "expRecord length");
            Assert.AreEqual(caseCount, expParamList.Length, "expParamList length");
            Assert.AreEqual(caseCount, expMethodList.Length, "expMethodList length");

            /* Run Cases */
            for (int caseDex = 0; caseDex < caseCount; caseDex++)
            {
                /* Initialize Input Parameters */
                EmParameters.DerivedHourlyChecksNeeded = neededList[caseDex];
                EmParameters.MonitorMethodRecordsByHourLocation = new CheckDataView<VwMpMonitorMethodRow>();
                {
                    if (mth1ParamList[caseDex] != null)
                        EmParameters.MonitorMethodRecordsByHourLocation.Add(new VwMpMonitorMethodRow(monLocId: "ONE", parameterCd: mth1ParamList[caseDex], methodCd: mth1CodeList[caseDex]));
                    if (mth2ParamList[caseDex] != null)
                        EmParameters.MonitorMethodRecordsByHourLocation.Add(new VwMpMonitorMethodRow(monLocId: "TWO", parameterCd: mth2ParamList[caseDex], methodCd: mth2CodeList[caseDex]));
                }

                /* Initialize Output Parameters */
                EmParameters.MatsHclMethodCode = "OLD";
                EmParameters.MatsHclMethodRecord = new VwMpMonitorMethodRow(monLocId: "OLD", parameterCd: "OLD", methodCd: "OLD");
                EmParameters.MatsHclParameterCode = "OLD";

                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Initialize variables needed to run the check. */
                bool log = false;
                string actual;

                /* Run Check */
                actual = cMATSOperatingHourChecks.MATSHOD2(category, ref log);

                /* Check Results */
                Assert.AreEqual(string.Empty, actual, string.Format("actual {0}", caseDex));
                Assert.AreEqual(false, log, string.Format("log {0}", caseDex));
                Assert.AreEqual(resultList[caseDex], category.CheckCatalogResult, string.Format("category.CheckCatalogResult {0}", caseDex));

                Assert.AreEqual(expMethodList[caseDex], EmParameters.MatsHclMethodCode, string.Format("MatsHclMethodCode {0}", caseDex));
                Assert.AreEqual(expRecord[caseDex], (EmParameters.MatsHclMethodRecord != null), string.Format("MatsHclMethodRecord {0}", caseDex));
                Assert.AreEqual(expParamList[caseDex], EmParameters.MatsHclParameterCode, string.Format("MatsHclParameterCode {0}", caseDex));
            }
        }

        /// <summary>
        /// 
        /// 
        /// | ## | Needed | Key | Param | Meth | Key | Param | Meth || Result | Record | Param | Meth || Note
        /// |  0 | true   | ONE | HFRE  | TSTM | TWO | HFC   | BAD  || null   | true   | HFRE  | TSTM || One HFRE method exists.
        /// |  1 | true   | ONE | HFRH  | TSTM | TWO | HFC   | BAD  || null   | true   | HFRH  | TSTM || One HFRH method exists.
        /// |  2 | false  | ONE | HFRE  | TSTM | TWO | HFC   | BAD  || null   | false  | null  | null || DerivedHourlyChecksNeeded is false.
        /// |  3 | null   | ONE | HFRE  | TSTM | TWO | HFC   | BAD  || null   | false  | null  | null || DerivedHourlyChecksNeeded is false.
        /// |  4 | true   | ONE | HFC   | BAD  | TWO | HFC   | BAD  || null   | false  | null  | null || No valid HG methods exist.
        /// |  5 | true   | ONE | HFRE  | TSTM | TWO | HFRH  | TSTM || A      | false  | null  | null || Two valid HG methods exist.
        /// </summary>
        [TestMethod()]
        public void MatsHod3()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            /* Input Parameter Values */
            bool?[] neededList = { true, true, false, null, true, true };
            string[] mth1ParamList = { "HFRE", "HFRH", "HFRE", "HFRE", "HFC", "HFRE" };
            string[] mth1CodeList = { "TSTM", "TSTM", "TSTM", "TSTM", "BAD", "TSTM" };
            string[] mth2ParamList = { "HFC", "HFC", "HFC", "HFC", "HFC", "HFRH" };
            string[] mth2CodeList = { "BAD", "BAD", "BAD", "BAD", "BAD", "TSTM" };

            /* Expected Values */
            string[] resultList = { null, null, null, null, null, "A" };
            bool[] expRecord = { true, true, false, false, false, false };
            string[] expParamList = { "HFRE", "HFRH", null, null, null, null };
            string[] expMethodList = { "TSTM", "TSTM", null, null, null, null };

            /* Test Case Count */
            int caseCount = 6;

            /* Check array lengths */
            Assert.AreEqual(caseCount, neededList.Length, "neededList length");
            Assert.AreEqual(caseCount, mth1ParamList.Length, "mth1ParamList length");
            Assert.AreEqual(caseCount, mth1CodeList.Length, "mth1CodeList length");
            Assert.AreEqual(caseCount, mth2ParamList.Length, "mth2ParamList length");
            Assert.AreEqual(caseCount, mth2CodeList.Length, "mth2CodeList length");
            Assert.AreEqual(caseCount, resultList.Length, "resultList length");
            Assert.AreEqual(caseCount, expRecord.Length, "expRecord length");
            Assert.AreEqual(caseCount, expParamList.Length, "expParamList length");
            Assert.AreEqual(caseCount, expMethodList.Length, "expMethodList length");

            /* Run Cases */
            for (int caseDex = 0; caseDex < caseCount; caseDex++)
            {
                /* Initialize Input Parameters */
                EmParameters.DerivedHourlyChecksNeeded = neededList[caseDex];
                EmParameters.MonitorMethodRecordsByHourLocation = new CheckDataView<VwMpMonitorMethodRow>();
                {
                    if (mth1ParamList[caseDex] != null)
                        EmParameters.MonitorMethodRecordsByHourLocation.Add(new VwMpMonitorMethodRow(monLocId: "ONE", parameterCd: mth1ParamList[caseDex], methodCd: mth1CodeList[caseDex]));
                    if (mth2ParamList[caseDex] != null)
                        EmParameters.MonitorMethodRecordsByHourLocation.Add(new VwMpMonitorMethodRow(monLocId: "TWO", parameterCd: mth2ParamList[caseDex], methodCd: mth2CodeList[caseDex]));
                }

                /* Initialize Output Parameters */
                EmParameters.MatsHfMethodCode = "OLD";
                EmParameters.MatsHfMethodRecord = new VwMpMonitorMethodRow(monLocId: "OLD", parameterCd: "OLD", methodCd: "OLD");
                EmParameters.MatsHfParameterCode = "OLD";

                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Initialize variables needed to run the check. */
                bool log = false;
                string actual;

                /* Run Check */
                actual = cMATSOperatingHourChecks.MATSHOD3(category, ref log);

                /* Check Results */
                Assert.AreEqual(string.Empty, actual, string.Format("actual {0}", caseDex));
                Assert.AreEqual(false, log, string.Format("log {0}", caseDex));
                Assert.AreEqual(resultList[caseDex], category.CheckCatalogResult, string.Format("category.CheckCatalogResult {0}", caseDex));

                Assert.AreEqual(expMethodList[caseDex], EmParameters.MatsHfMethodCode, string.Format("MatsHfMethodCode {0}", caseDex));
                Assert.AreEqual(expRecord[caseDex], (EmParameters.MatsHfMethodRecord != null), string.Format("MatsHfMethodRecord {0}", caseDex));
                Assert.AreEqual(expParamList[caseDex], EmParameters.MatsHfParameterCode, string.Format("MatsHfParameterCode {0}", caseDex));
            }
        }

        /// <summary>
        /// 
        /// 
        /// | ## | Needed | Key | Param | Meth | Key | Param | Meth || Result | Record | Param | Meth || Note
        /// |  0 | true   | ONE | SO2RE | TSTM | TWO | SO2C  | BAD  || null   | true   | SO2RE | TSTM || One SO2RE method exists.
        /// |  1 | true   | ONE | SO2RH | TSTM | TWO | SO2C  | BAD  || null   | true   | SO2RH | TSTM || One SO2RH method exists.
        /// |  2 | false  | ONE | SO2RE | TSTM | TWO | SO2C  | BAD  || null   | false  | null  | null || DerivedHourlyChecksNeeded is false.
        /// |  3 | null   | ONE | SO2RE | TSTM | TWO | SO2C  | BAD  || null   | false  | null  | null || DerivedHourlyChecksNeeded is false.
        /// |  4 | true   | ONE | SO2C  | BAD  | TWO | SO2C  | BAD  || null   | false  | null  | null || No valid SO2 methods exist.
        /// |  5 | true   | ONE | SO2RE | TSTM | TWO | SO2RH | TSTM || A      | false  | null  | null || Two valid SO2 methods exist.
        /// </summary>
        [TestMethod()]
        public void MatsHod4()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            /* Input Parameter Values */
            bool?[] neededList = { true, true, false, null, true, true };
            string[] mth1ParamList = { "SO2RE", "SO2RH", "SO2RE", "SO2RE", "SO2C", "SO2RE" };
            string[] mth1CodeList = { "TSTM", "TSTM", "TSTM", "TSTM", "BAD", "TSTM" };
            string[] mth2ParamList = { "SO2C", "SO2C", "SO2C", "SO2C", "SO2C", "SO2RH" };
            string[] mth2CodeList = { "BAD", "BAD", "BAD", "BAD", "BAD", "TSTM" };

            /* Expected Values */
            string[] resultList = { null, null, null, null, null, "A" };
            bool[] expRecord = { true, true, false, false, false, false };
            string[] expParamList = { "SO2RE", "SO2RH", null, null, null, null };
            string[] expMethodList = { "TSTM", "TSTM", null, null, null, null };

            /* Test Case Count */
            int caseCount = 6;

            /* Check array lengths */
            Assert.AreEqual(caseCount, neededList.Length, "neededList length");
            Assert.AreEqual(caseCount, mth1ParamList.Length, "mth1ParamList length");
            Assert.AreEqual(caseCount, mth1CodeList.Length, "mth1CodeList length");
            Assert.AreEqual(caseCount, mth2ParamList.Length, "mth2ParamList length");
            Assert.AreEqual(caseCount, mth2CodeList.Length, "mth2CodeList length");
            Assert.AreEqual(caseCount, resultList.Length, "resultList length");
            Assert.AreEqual(caseCount, expRecord.Length, "expRecord length");
            Assert.AreEqual(caseCount, expParamList.Length, "expParamList length");
            Assert.AreEqual(caseCount, expMethodList.Length, "expMethodList length");

            /* Run Cases */
            for (int caseDex = 0; caseDex < caseCount; caseDex++)
            {
                /* Initialize Input Parameters */
                EmParameters.DerivedHourlyChecksNeeded = neededList[caseDex];
                EmParameters.MonitorMethodRecordsByHourLocation = new CheckDataView<VwMpMonitorMethodRow>();
                {
                    if (mth1ParamList[caseDex] != null)
                        EmParameters.MonitorMethodRecordsByHourLocation.Add(new VwMpMonitorMethodRow(monLocId: "ONE", parameterCd: mth1ParamList[caseDex], methodCd: mth1CodeList[caseDex]));
                    if (mth2ParamList[caseDex] != null)
                        EmParameters.MonitorMethodRecordsByHourLocation.Add(new VwMpMonitorMethodRow(monLocId: "TWO", parameterCd: mth2ParamList[caseDex], methodCd: mth2CodeList[caseDex]));
                }

                /* Initialize Output Parameters */
                EmParameters.MatsSo2MethodCode = "OLD";
                EmParameters.MatsSo2MethodRecord = new VwMpMonitorMethodRow(monLocId: "OLD", parameterCd: "OLD", methodCd: "OLD");
                EmParameters.MatsSo2ParameterCode = "OLD";

                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Initialize variables needed to run the check. */
                bool log = false;
                string actual;

                /* Run Check */
                actual = cMATSOperatingHourChecks.MATSHOD4(category, ref log);

                /* Check Results */
                Assert.AreEqual(string.Empty, actual, string.Format("actual {0}", caseDex));
                Assert.AreEqual(false, log, string.Format("log {0}", caseDex));
                Assert.AreEqual(resultList[caseDex], category.CheckCatalogResult, string.Format("category.CheckCatalogResult {0}", caseDex));

                Assert.AreEqual(expMethodList[caseDex], EmParameters.MatsSo2MethodCode, string.Format("MatsSo2MethodCode {0}", caseDex));
                Assert.AreEqual(expRecord[caseDex], (EmParameters.MatsSo2MethodRecord != null), string.Format("MatsSo2MethodRecord {0}", caseDex));
                Assert.AreEqual(expParamList[caseDex], EmParameters.MatsSo2ParameterCode, string.Format("MatsSo2ParameterCode {0}", caseDex));
            }
        }

        #region MATSHOD-5
        /// <summary>
        ///A test for MATSHOD-5
        ///</summary>()
        [TestMethod()]
        public void MATSHOD5()
        {
            //static check setup
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            // Variables
            bool log = false;
            string actual;

            //True HG
            {
                // Init Input
                EmParameters.MatsHgParameterCode = "NOTNULL";
                EmParameters.MatsHclParameterCode = null;
                EmParameters.MatsHfParameterCode = null;
                EmParameters.MatsSo2ParameterCode = null;

                // Init Output
                EmParameters.MatsExpected = null;
                category.CheckCatalogResult = null;

                // Run Checks
                actual = cMATSOperatingHourChecks.MATSHOD5(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual(true, EmParameters.MatsExpected, "MATS Expected");
            }

            //True HCL
            {
                // Init Input
                EmParameters.MatsHgParameterCode = null;
                EmParameters.MatsHclParameterCode = "NOTNULL";
                EmParameters.MatsHfParameterCode = null;
                EmParameters.MatsSo2ParameterCode = null;

                // Init Output
                EmParameters.MatsExpected = null;
                category.CheckCatalogResult = null;

                // Run Checks
                actual = cMATSOperatingHourChecks.MATSHOD5(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual(true, EmParameters.MatsExpected, "MATS Expected");
            }

            //True HF
            {
                // Init Input
                EmParameters.MatsHgParameterCode = null;
                EmParameters.MatsHclParameterCode = null;
                EmParameters.MatsHfParameterCode = "NOTNULL";
                EmParameters.MatsSo2ParameterCode = null;

                // Init Output
                EmParameters.MatsExpected = null;
                category.CheckCatalogResult = null;

                // Run Checks
                actual = cMATSOperatingHourChecks.MATSHOD5(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual(true, EmParameters.MatsExpected, "MATS Expected");
            }

            //True SO2
            {
                // Init Input
                EmParameters.MatsHgParameterCode = null;
                EmParameters.MatsHclParameterCode = null;
                EmParameters.MatsHfParameterCode = null;
                EmParameters.MatsSo2ParameterCode = "NOTNULL";

                // Init Output
                EmParameters.MatsExpected = null;
                category.CheckCatalogResult = null;

                // Run Checks
                actual = cMATSOperatingHourChecks.MATSHOD5(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual(true, EmParameters.MatsExpected, "MATS Expected");
            }

            //False
            {
                // Init Input
                EmParameters.MatsHgParameterCode = null;
                EmParameters.MatsHclParameterCode = null;
                EmParameters.MatsHfParameterCode = null;
                EmParameters.MatsSo2ParameterCode = null;

                // Init Output
                EmParameters.MatsExpected = null;
                category.CheckCatalogResult = null;

                // Run Checks
                actual = cMATSOperatingHourChecks.MATSHOD5(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual(false, EmParameters.MatsExpected, "MATS Expected");
            }
        }
        #endregion

        /// <summary>
        /// MatsHod6
        /// 
        /// | ## | Needed | OpTime | MethParam | MethCode | DhvParam    | DhvEquat    | dhvModc | UnadjHrlyValue || Result | Hgre  | Hgrh  | Hgc   | Record | DHV ID | HgUnadjHrly | Description  || Note
        /// |  0 | null   |    0.1 | HGRE      | CEM      | []          | []          | []      | 0.01E2         || null   | false | false | false | null   | null   | null        | MATS Hg Rate || DerivedHourlyChecksNeeded is null (treat null as false).
        /// |  1 | false  |    0.1 | HGRH      | CEM      | []          | []          | []      | 0.10E2         || null   | false | false | false | null   | null   | null        | MATS Hg Rate || DerivedHourlyChecksNeeded is false.
        /// |  2 | true   |    0.1 | HGRE      | CEM      | []          | []          | []      | 0.20E2         || E      | false | false | false | null   | null   | null        | MATS Hg Rate || Method exists, but no DHV.
        /// |  3 | true   |    0.1 | HGRH      | CEM      | []          | []          | []      | 0.30E2         || E      | false | false | false | null   | null   | null        | MATS Hg Rate || Method exists, but no DHV.
        /// |  4 | true   |    0.1 | HGRE      | CEM      | [HGR]       | [NORM]      | [36]    | 0.40E2         || E      | false | false | false | null   | null   | null        | MATS Hg Rate || Method exists, but DHV only exists with excluded parameter.
        /// |  5 | true   |    0.1 | HGRH      | CEM      | [HGR]       | [NORM]      | [36]    | 0.50E2         || E      | false | false | false | null   | null   | null        | MATS Hg Rate || Method exists, but DHV only exists with excluded parameter.
        /// |  6 | true   |    0.1 | HGRE      | CEM      | [HGRE,HGRH] | [NORM,NORM] | [36,36] | 0.60E2         || B      | false | false | false | null   | null   | null        | MATS Hg Rate || Method exists, but more than one DHV exists.
        /// |  7 | true   |    0.1 | HGRH      | CEM      | [HGRE,HGRH] | [NORM,NORM] | [36,36] | 0.70E2         || B      | false | false | false | null   | null   | null        | MATS Hg Rate || Method exists, but more than one DHV exists.
        /// |  8 | true   |    0.1 | HGRE      | CEM      | [HGRH]      | [NORM]      | [36]    | 0.80E2         || C      | false | false | false | [HGRH] | null   | null        | MATS Hg Rate || Method exists and one DHV exists, but parameters do not match.
        /// |  9 | true   |    0.1 | HGRH      | CEM      | [HGRE]      | [NORM]      | [36]    | 0.90E2         || C      | false | false | false | [HGRE] | null   | null        | MATS Hg Rate || Method exists and one DHV exists, but parameters do not match.
        /// | 10 | true   |    0.1 | HGRE      | CEM      | [HGRE]      | [NORM]      | [36]    | 1.00E2         || null   | true  | false | true  | [HGRE] | null   | null        | MATS Hg Rate || Method exists, one DHV exists, and parameters match.
        /// | 11 | true   |    0.1 | HGRH      | CEM      | [HGRH]      | [NORM]      | [36]    | 1.10E2         || null   | false | true  | true  | [HGRH] | null   | null        | MATS Hg Rate || Method exists, one DHV exists, and parameters match.
        /// | 12 | true   |    0.1 | null      | CEM      | []          | []          | []      | 1.20E2         || null   | false | false | false | null   | null   | null        | MATS Hg Rate || Neither Method nor DHV exist.
        /// | 13 | true   |    0.1 | null      | CEM      | [HGRE]      | [NORM]      | [36]    | 1.30E2         || A      | false | false | false | null   | null   | null        | MATS Hg Rate || Method does not exist, but DHV exists.
        /// | 14 | true   |    0.1 | null      | CEM      | [HGRH]      | [NORM]      | [36]    | 1.40E2         || A      | false | false | false | null   | null   | null        | MATS Hg Rate || Method does not exist, but DHV exists.
        /// | 15 | true   |    0.0 | HGRE      | CEM      | []          | []          | []      | 1.50E2         || null   | false | false | false | null   | null   | null        | MATS Hg Rate || Not operating, method exists, and no DHV.
        /// | 16 | true   |    0.0 | HGRH      | CEM      | []          | []          | []      | 1.60E2         || null   | false | false | false | null   | null   | null        | MATS Hg Rate || Not operating, method exists, and no DHV.
        /// | 17 | true   |    0.0 | HGRE      | CEM      | [HGRE]      | [NORM]      | [36]    | 1.70E2         || D      | false | false | false | [HGRE] | null   | null        | MATS Hg Rate || Not operating, method exists, one DHV exists, and parameters match.
        /// | 18 | true   |    0.0 | HGRH      | CEM      | [HGRH]      | [NORM]      | [36]    | 1.80E2         || D      | false | false | false | [HGRH] | null   | null        | MATS Hg Rate || Not operating, method exists, one DHV exists, and parameters match.
        /// | 19 | true   |    0.1 | HGRE      | CEM      | [HGRE]      | [MS-1]      | [36]    | 1.90E2         || G      | false | false | false | [HGRE] | null   | null        | MATS Hg Rate || CEM method with apportionment formula.
        /// | 20 | true   |    0.1 | HGRH      | CEM      | [HGRH]      | [MS-1]      | [36]    | 2.01E2         || G      | false | false | false | [HGRH] | null   | null        | MATS Hg Rate || CEM method with apportionment formula.
        /// | 21 | true   |    0.1 | HGRE      | ST       | [HGRE]      | [MS-1]      | [36]    | 2.10E2         || G      | false | false | false | [HGRE] | null   | null        | MATS Hg Rate || ST method with apportionment formula.
        /// | 22 | true   |    0.1 | HGRH      | ST       | [HGRH]      | [MS-1]      | [36]    | 2.20E2         || G      | false | false | false | [HGRH] | null   | null        | MATS Hg Rate || ST method with apportionment formula.
        /// | 23 | true   |    0.1 | HGRE      | CALC     | [HGRE]      | [MS-1]      | [36]    | 2.30E2         || null   | false | false | false | [HGRE] | 23     | 2.30E2      | MATS Hg Rate || CALC method with apportionment formula.
        /// | 24 | true   |    0.1 | HGRH      | CALC     | [HGRH]      | [MS-1]      | [36]    | 2.40E2         || null   | false | false | false | [HGRH] | 24     | 2.40E2      | MATS Hg Rate || CALC method with apportionment formula.
        /// | 25 | true   |    0.1 | HGRE      | CALC     | [HGRE]      | [NORM]      | [36]    | 2.50E2         || F      | false | false | false | [HGRE] | null   | null        | MATS Hg Rate || CALC method with non-apportionment formula.
        /// | 26 | true   |    0.1 | HGRH      | CALC     | [HGRH]      | [NORM]      | [36]    | 2.60E2         || F      | false | false | false | [HGRH] | null   | null        | MATS Hg Rate || CALC method with non-apportionment formula.
        /// | 27 | true   |    0.1 | HGRE      | CALC     | [HGRE]      | null        | [36]    | 2.70E2         || F      | false | false | false | [HGRE] | null   | null        | MATS Hg Rate || CALC method with null formula.
        /// | 28 | true   |    0.1 | HGRH      | CALC     | [HGRH]      | null        | [36]    | 2.80E2         || F      | false | false | false | [HGRH] | null   | null        | MATS Hg Rate || CALC method with null formula.
        /// | 29 | true   |    0.1 | HGRE      | CALC     | [HGRE]      | [MS-1]      | [37]    | 2.90E2         || null   | false | false | false | [HGRE] | 29     | 2.90E2      | MATS Hg Rate || CALC method with 37 MODC and apportionment formula.
        /// | 30 | true   |    0.1 | HGRH      | CALC     | [HGRH]      | [MS-1]      | [37]    | 3.01E2         || null   | false | false | false | [HGRH] | 30     | 3.01E2      | MATS Hg Rate || CALC method with 37 MODC and apportionment formula.
        /// | 31 | true   |    0.1 | HGRE      | CALC     | [HGRE]      | null        | [37]    | 3.10E2         || F      | false | false | false | [HGRE] | null   | null        | MATS Hg Rate || CALC method with 37 MODC and null formula.
        /// | 32 | true   |    0.1 | HGRH      | CALC     | [HGRH]      | null        | [37]    | 3.20E2         || F      | false | false | false | [HGRH] | null   | null        | MATS Hg Rate || CALC method with 37 MODC and null formula.
        /// | 33 | true   |    0.1 | HGRE      | CALC     | [HGRE]      | [MS-1]      | [38]    | 3.30E2         || null   | false | false | false | [HGRE] | null   | null        | MATS Hg Rate || CALC method with 38 MODC and apportionment formula.
        /// | 34 | true   |    0.1 | HGRH      | CALC     | [HGRH]      | [MS-1]      | [38]    | 3.40E2         || null   | false | false | false | [HGRH] | null   | null        | MATS Hg Rate || CALC method with 38 MODC and apportionment formula.
        /// | 35 | true   |    0.1 | HGRE      | CALC     | [HGRE]      | null        | [38]    | 3.50E2         || H      | false | false | false | [HGRE] | null   | null        | MATS Hg Rate || CALC method with 38 MODC and null formula.
        /// | 36 | true   |    0.1 | HGRH      | CALC     | [HGRH]      | null        | [38]    | 3.60E2         || H      | false | false | false | [HGRH] | null   | null        | MATS Hg Rate || CALC method with 38 MODC and null formula.
        /// | 37 | true   |    0.1 | HGRE      | CALC     | [HGRE]      | [MS-1]      | [39]    | 3.70E2         || null   | false | false | false | [HGRE] | 37     | 3.70E2      | MATS Hg Rate || CALC method with 39 MODC and apportionment formula.
        /// | 38 | true   |    0.1 | HGRH      | CALC     | [HGRH]      | [MS-1]      | [39]    | 3.80E2         || null   | false | false | false | [HGRH] | 38     | 3.80E2      | MATS Hg Rate || CALC method with 39 MODC and apportionment formula.
        /// | 39 | true   |    0.1 | HGRE      | CALC     | [HGRE]      | null        | [39]    | 3.90E2         || F      | false | false | false | [HGRE] | null   | null        | MATS Hg Rate || CALC method with 39 MODC and null formula.
        /// | 40 | true   |    0.1 | HGRH      | CALC     | [HGRH]      | null        | [39]    | 4.01E2         || F      | false | false | false | [HGRH] | null   | null        | MATS Hg Rate || CALC method with 39 MODC and null formula.
        /// 
        /// </summary>
        [TestMethod()]
        public void MatsHod6()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            int caseCount = 41;

            /* Input Parameter Values */
            string[] dhvIdList = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31", "32", "33", "34", "35", "36", "37", "38", "39", "40" };
            bool?[] checksNeededList = { null, false, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true };
            decimal?[] opTimeNeededList = { 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.0m, 0.0m, 0.0m, 0.0m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m };
            string[] methodParameterList = { "HGRE", "HGRH", "HGRE", "HGRH", "HGRE", "HGRH", "HGRE", "HGRH", "HGRE", "HGRH", "HGRE", "HGRH", null, null, null, "HGRE", "HGRH", "HGRE", "HGRH", "HGRE", "HGRH", "HGRE", "HGRH", "HGRE", "HGRH", "HGRE", "HGRH", "HGRE", "HGRH", "HGRE", "HGRH", "HGRE", "HGRH", "HGRE", "HGRH", "HGRE", "HGRH", "HGRE", "HGRH", "HGRE", "HGRH" };
            string[] methodCodeList = { "CEM", "CEM", "CEM", "CEM", "CEM", "CEM", "CEM", "CEM", "CEM", "CEM", "CEM", "CEM", "CEM", "CEM", "CEM", "CEM", "CEM", "CEM", "CEM", "CEM", "CEM", "ST", "ST", "CALC", "CALC", "CALC", "CALC", "CALC", "CALC", "CALC", "CALC", "CALC", "CALC", "CALC", "CALC", "CALC", "CALC", "CALC", "CALC", "CALC", "CALC" };
            string[][] dhvParameterList = { new string[] { }, new string[] { }, new string[] { }, new string[] { }, new string[] { "HGR" }, new string[] { "HGR" }, new string[] { "HGRE", "HGRH" }, new string[] { "HGRE", "HGRH" }, new string[] { "HGRH" }, new string[] { "HGRE" }, new string[] { "HGRE" }, new string[] { "HGRH" }, new string[] { }, new string[] { "HGRE" }, new string[] { "HGRH" }, new string[] { }, new string[] { }, new string[] { "HGRE" }, new string[] { "HGRH" }, new string[] { "HGRE" }, new string[] { "HGRH" }, new string[] { "HGRE" }, new string[] { "HGRH" }, new string[] { "HGRE" }, new string[] { "HGRH" }, new string[] { "HGRE" }, new string[] { "HGRH" }, new string[] { "HGRE" }, new string[] { "HGRH" }, new string[] { "HGRE" }, new string[] { "HGRH" }, new string[] { "HGRE" }, new string[] { "HGRH" }, new string[] { "HGRE" }, new string[] { "HGRH" }, new string[] { "HGRE" }, new string[] { "HGRH" }, new string[] { "HGRE" }, new string[] { "HGRH" }, new string[] { "HGRE" }, new string[] { "HGRH" } };
            string[][] dhvEquationList = { new string[] { }, new string[] { }, new string[] { }, new string[] { }, new string[] { "NORM" }, new string[] { "NORM" }, new string[] { "NORM", "NORM" }, new string[] { "NORM", "NORM" }, new string[] { "NORM" }, new string[] { "NORM" }, new string[] { "NORM" }, new string[] { "NORM" }, new string[] { }, new string[] { "NORM" }, new string[] { "NORM" }, new string[] { }, new string[] { }, new string[] { "NORM" }, new string[] { "NORM" }, new string[] { "MS-1" }, new string[] { "MS-1" }, new string[] { "MS-1" }, new string[] { "MS-1" }, new string[] { "MS-1" }, new string[] { "MS-1" }, new string[] { "NORM" }, new string[] { "NORM" }, new string[] { null }, new string[] { null }, new string[] { "MS-1" }, new string[] { "MS-1" }, new string[] { null }, new string[] { null }, new string[] { "MS-1" }, new string[] { "MS-1" }, new string[] { null }, new string[] { null }, new string[] { "MS-1" }, new string[] { "MS-1" }, new string[] { null }, new string[] { null } };
            string[][] dhvModcList = { new string[] { }, new string[] { }, new string[] { }, new string[] { }, new string[] { "36" }, new string[] { "36" }, new string[] { "36", "36" }, new string[] { "36", "36" }, new string[] { "36" }, new string[] { "36" }, new string[] { "36" }, new string[] { "36" }, new string[] { }, new string[] { "36" }, new string[] { "36" }, new string[] { }, new string[] { }, new string[] { "36" }, new string[] { "36" }, new string[] { "36" }, new string[] { "36" }, new string[] { "36" }, new string[] { "36" }, new string[] { "36" }, new string[] { "36" }, new string[] { "36" }, new string[] { "36" }, new string[] { "36" }, new string[] { "36" }, new string[] { "37" }, new string[] { "37" }, new string[] { "37" }, new string[] { "37" }, new string[] { "38" }, new string[] { "38" }, new string[] { "38" }, new string[] { "38" }, new string[] { "39" }, new string[] { "39" }, new string[] { "39" }, new string[] { "39" } };
            string[] unadjHrlyValueList = {"0.01E2", "0.10E2", "0.20E2", "0.30E2", "0.40E2", "0.50E2", "0.60E2", "0.70E2", "0.80E2", "0.90E2",
                                           "1.01E2", "1.10E2", "1.20E2", "1.30E2", "1.40E2", "1.50E2", "1.60E2", "1.70E2", "1.80E2", "1.90E2",
                                           "2.01E2", "2.10E2", "2.20E2", "2.30E2", "2.40E2", "2.50E2", "2.60E2", "2.70E2", "2.80E2", "2.90E2",
                                           "3.01E2", "3.10E2", "3.20E2", "3.30E2", "3.40E2", "3.50E2", "3.60E2", "3.70E2", "3.80E2", "3.90E2",
                                           "4.01E2", };
            string[] appHgRateArrayList = { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null };

            /* Expected Values */
            string[] expectedResultList = { null, null, "E", "E", "E", "E", "B", "B", "C", "C", null, null, null, "A", "A", null, null, "D", "D", "G", "G", "G", "G", null, null, "F", "F", "F", "F", null, null, "F", "F", null, null, "H", "H", null, null, "F", "F" };
            bool[] expectedReNeededList = { false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false };
            bool[] expectedRhNeededList = { false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false };
            bool[] expectedConcNeededList = { false, false, false, false, false, false, false, false, false, false, true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false };
            string[] expectedParameterList = { null, null, null, null, null, null, null, null, "HGRH", "HGRE", "HGRE", "HGRH", null, null, null, null, null, null, null, "HGRE", "HGRH", "HGRE", "HGRH", "HGRE", "HGRH", "HGRE", "HGRH", "HGRE", "HGRH", "HGRE", "HGRH", "HGRE", "HGRH", "HGRE", "HGRH", "HGRE", "HGRH", "HGRE", "HGRH", "HGRE", "HGRH" };
            string[] expectedIdList = { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, "23", "24", null, null, null, null, "29", "30", null, null, null, null, null, null, "37", "38", null, null };
            string[] expectedUnadjHrlyList = { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, "2.30E2", "2.40E2", null, null, null, null, "2.90E2", "3.01E2", null, null, null, null, null, null, "3.70E2", "3.80E2", null, null };
            string expectedParameterDescription = "MATS Hg Rate";

            string[] expectedAppHgRateArray = {null, null, null, null, null, null, null, null, "0.80E2", "0.90E2",
                                              "1.01E2", "1.10E2", null, null, null, null, null, null, null, "1.90E2",
                                              "2.01E2", "2.10E2", "2.20E2", "2.30E2", "2.40E2", "2.50E2", "2.60E2", "2.70E2", "2.80E2", "2.90E2",
                                              "3.01E2", "3.10E2", "3.20E2", "3.30E2", "3.40E2", "3.50E2", "3.60E2", "3.70E2", "3.80E2", "3.90E2",
                                              "4.01E2", };
            string[] expectedMatsMs1ModcArray = {null, null, null, null, null, null, null, null, "36", "36",
                                              "36", "36", null, null, null, null, null, null, null, "36",
                                              "36", "36", "36", "36", "36", "36", "36", "36", "36", "37",
                                              "37", "37", "37", "38", "38", "38", "38", "39", "39", "39",
                                              "39", };

            /* Check array lengths */
            Assert.AreEqual(caseCount, dhvIdList.Length, "dhvIdList length");
            Assert.AreEqual(caseCount, checksNeededList.Length, "checksNeededList length");
            Assert.AreEqual(caseCount, opTimeNeededList.Length, "opTimeNeededList length");
            Assert.AreEqual(caseCount, methodParameterList.Length, "methodParameterList length");
            Assert.AreEqual(caseCount, methodCodeList.Length, "methodCodeList length");
            Assert.AreEqual(caseCount, dhvParameterList.Length, "dhvParameterList length");
            Assert.AreEqual(caseCount, dhvEquationList.Length, "dhvEquationList length");
            Assert.AreEqual(caseCount, dhvModcList.Length, "dhvModcList length");
            Assert.AreEqual(caseCount, expectedResultList.Length, "expectedResultList length");
            Assert.AreEqual(caseCount, expectedReNeededList.Length, "expectedReNeededList length");
            Assert.AreEqual(caseCount, expectedRhNeededList.Length, "expectedRhNeededList length");
            Assert.AreEqual(caseCount, expectedConcNeededList.Length, "expectedConcNeededList length");
            Assert.AreEqual(caseCount, expectedParameterList.Length, "expectedParameterList length");
            Assert.AreEqual(caseCount, expectedIdList.Length, "expectedIdList length");

            for (int caseDex = 0; caseDex < caseCount; caseDex++)
            {
                /* Generate MATS DHV REcord By Hour Location rows */
                MATSDerivedHourlyValueData[] MatsDhvRecordsByHourLocationRows;
                {
                    string[] parameterList = dhvParameterList[caseDex];
                    string[] equationList = dhvEquationList[caseDex];
                    string[] modcList = dhvModcList[caseDex];

                    MatsDhvRecordsByHourLocationRows = new MATSDerivedHourlyValueData[parameterList.Length];

                    for (int rowDex = 0; rowDex < parameterList.Length; rowDex++)
                        MatsDhvRecordsByHourLocationRows[rowDex] = new MATSDerivedHourlyValueData(matsDhvId: dhvIdList[caseDex], parameterCd: parameterList[rowDex]
                                                                                                , equationCd: equationList[rowDex], modcCd: modcList[rowDex]
                                                                                                , unadjustedHrlyValue: unadjHrlyValueList[caseDex]);
                }

                /* Initialize Input Parameters */
                EmParameters.CurrentHourlyOpRecord = new VwMpHrlyOpDataRow(opTime: opTimeNeededList[caseDex]);
                EmParameters.DerivedHourlyChecksNeeded = checksNeededList[caseDex];
                EmParameters.MatsDhvRecordsByHourLocation = new CheckDataView<MATSDerivedHourlyValueData>(MatsDhvRecordsByHourLocationRows);
                EmParameters.MatsHgMethodCode = methodCodeList[caseDex];
                EmParameters.MatsHgParameterCode = methodParameterList[caseDex];
                EmParameters.CurrentMonitorPlanLocationPostion = caseDex;

                /* Initialize Optional Parameters */
                EmParameters.MatsMs1HgDhvId = null;
                EmParameters.MatsMs1HgUnadjustedHourlyValue = null;
                category.Process.ProcessParameters.RegisterParameter(4642, "Apportionment_Hg_Rate_Array");
                category.SetCheckParameter("Apportionment_Hg_Rate_Array", appHgRateArrayList, eParameterDataType.String, false, true);
                category.Process.ProcessParameters.RegisterParameter(4673, "MATS_MS1_Hg_MODC_Code_Array");
                category.SetCheckParameter("MATS_MS1_Hg_MODC_Code_Array", expectedMatsMs1ModcArray, eParameterDataType.String, false, true);

                /* Initialize Output Parameters */
                EmParameters.MatsHgreDhvChecksNeeded = null;
                EmParameters.MatsHgrhDhvChecksNeeded = null;
                EmParameters.MatsHgcNeeded = null;
                EmParameters.MatsHgDhvRecord = new MATSDerivedHourlyValueData(); // Should always exit check as null or with parameter of HGRE or HGRH.
                EmParameters.MatsHgDhvParameterDescription = null;

                /* Init Cateogry Result */
                category.CheckCatalogResult = null;


                /* Initialize variables needed to run the check. */
                bool log = false;
                string actual;

                /* Run Check */
                actual = cMATSOperatingHourChecks.MATSHOD6(category, ref log);

                /* Check results */
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);

                Assert.AreEqual(expectedResultList[caseDex], category.CheckCatalogResult, string.Format("Result {0}", caseDex));
                Assert.AreEqual(expectedParameterList[caseDex], (EmParameters.MatsHgDhvRecord != null ? EmParameters.MatsHgDhvRecord.ParameterCd : null), string.Format("MatsHgDhvRecord {0}", caseDex));
                Assert.AreEqual(expectedParameterDescription, EmParameters.MatsHgDhvParameterDescription, string.Format("MatsHgDhvParameterDescription {0}", caseDex));
                Assert.AreEqual(expectedConcNeededList[caseDex], EmParameters.MatsHgcNeeded, string.Format("MatsHgcNeeded {0}", caseDex));
                Assert.AreEqual(expectedReNeededList[caseDex], EmParameters.MatsHgreDhvChecksNeeded, string.Format("MatsHgreDhvChecksNeeded {0}", caseDex));
                Assert.AreEqual(expectedRhNeededList[caseDex], EmParameters.MatsHgrhDhvChecksNeeded, string.Format("MatsHgrhDhvChecksNeeded {0}", caseDex));
                Assert.AreEqual(expectedIdList[caseDex], EmParameters.MatsMs1HgDhvId, string.Format("MatsMs1HgDhvId {0}", caseDex));
                Assert.AreEqual(expectedUnadjHrlyList[caseDex], EmParameters.MatsMs1HgUnadjustedHourlyValue, string.Format("MatsMs1HgUnadjustedHourlyValue {0}", caseDex));

            }

            /* Check the array results */
            string[] apportionmentHgRateArray = (string[])category.GetCheckParameter("Apportionment_Hg_Rate_Array").ParameterValue;
            string[] matsMS1HgModcCodeArray = (string[])category.GetCheckParameter("MATS_MS1_Hg_MODC_Code_Array").ParameterValue; 
            CollectionAssert.AreEqual(expectedAppHgRateArray, apportionmentHgRateArray, string.Format("ApportionmentHgRateArray does not equal the expected result."));
            CollectionAssert.AreEqual(expectedMatsMs1ModcArray, matsMS1HgModcCodeArray, string.Format("MatsMS1So2ModcCodeArray does not equal the expected result."));
        }

        /// <summary>
        /// MatsHod7
        /// 
        /// | ## | Needed | OpTime | MethParam | MethCode | DhvParam      | DhvEquat    | dhvModc | UnadjHrlyValue || Result | Hgre  | Hgrh  | Hgc   | Record  | DHV ID | HclUnadjHrly| Description   || Note
        /// |  0 | null   |    0.1 | HCLRE     | CEM      | []            | []          | []      | 0.01E2         || null   | false | false | false | null    | null   | null        | MATS HCl Rate || DerivedHourlyChecksNeeded is null (treat null as false).
        /// |  1 | false  |    0.1 | HCLRH     | CEM      | []            | []          | []      | 0.10E2         || null   | false | false | false | null    | null   | null        | MATS HCl Rate || DerivedHourlyChecksNeeded is false.
        /// |  2 | true   |    0.1 | HCLRE     | CEM      | []            | []          | []      | 0.20E2         || E      | false | false | false | null    | null   | null        | MATS HCl Rate || Method exists, but no DHV.
        /// |  3 | true   |    0.1 | HCLRH     | CEM      | []            | []          | []      | 0.30E2         || E      | false | false | false | null    | null   | null        | MATS HCl Rate || Method exists, but no DHV.
        /// |  4 | true   |    0.1 | HCLRE     | CEM      | [HCLR]        | [NORM]      | [36]    | 0.40E2         || E      | false | false | false | null    | null   | null        | MATS HCl Rate || Method exists, but DHV only exists with excluded parameter.
        /// |  5 | true   |    0.1 | HCLRH     | CEM      | [HCLR]        | [NORM]      | [36]    | 0.50E2         || E      | false | false | false | null    | null   | null        | MATS HCl Rate || Method exists, but DHV only exists with excluded parameter.
        /// |  6 | true   |    0.1 | HCLRE     | CEM      | [HCLRE,HCLRH] | [NORM,NORM] | [36,36] | 0.60E2         || B      | false | false | false | null    | null   | null        | MATS HCl Rate || Method exists, but more than one DHV exists.
        /// |  7 | true   |    0.1 | HCLRH     | CEM      | [HCLRE,HCLRH] | [NORM,NORM] | [36,36] | 0.70E2         || B      | false | false | false | null    | null   | null        | MATS HCl Rate || Method exists, but more than one DHV exists.
        /// |  8 | true   |    0.1 | HCLRE     | CEM      | [HCLRH]       | [NORM]      | [36]    | 0.80E2         || C      | false | false | false | [HCLRH] | null   | null        | MATS HCl Rate || Method exists and one DHV exists, but parameters do not match.
        /// |  9 | true   |    0.1 | HCLRH     | CEM      | [HCLRE]       | [NORM]      | [36]    | 0.90E2         || C      | false | false | false | [HCLRE] | null   | null        | MATS HCl Rate || Method exists and one DHV exists, but parameters do not match.
        /// | 10 | true   |    0.1 | HCLRE     | CEM      | [HCLRE]       | [NORM]      | [36]    | 1.01E2         || null   | true  | false | true  | [HCLRE] | null   | null        | MATS HCl Rate || Method exists, one DHV exists, and parameters match.
        /// | 11 | true   |    0.1 | HCLRH     | CEM      | [HCLRH]       | [NORM]      | [36]    | 1.10E2         || null   | false | true  | true  | [HCLRH] | null   | null        | MATS HCl Rate || Method exists, one DHV exists, and parameters match.
        /// | 12 | true   |    0.1 | null      | CEM      | []            | []          | []      | 1.20E2         || null   | false | false | false | null    | null   | null        | MATS HCl Rate || Neither Method nor DHV exist.
        /// | 13 | true   |    0.1 | null      | CEM      | [HCLRE]       | [NORM]      | [36]    | 1.30E2         || A      | false | false | false | null    | null   | null        | MATS HCl Rate || Method does not exist, but DHV exists.
        /// | 14 | true   |    0.1 | null      | CEM      | [HCLRH]       | [NORM]      | [36]    | 1.40E2         || A      | false | false | false | null    | null   | null        | MATS HCl Rate || Method does not exist, but DHV exists.
        /// | 15 | true   |    0.0 | HCLRE     | CEM      | []            | []          | []      | 1.50E2         || null   | false | false | false | null    | null   | null        | MATS HCl Rate || Not operating, method exists, and no DHV.
        /// | 16 | true   |    0.0 | HCLRH     | CEM      | []            | []          | []      | 1.60E2         || null   | false | false | false | null    | null   | null        | MATS HCl Rate || Not operating, method exists, and no DHV.
        /// | 17 | true   |    0.0 | HCLRE     | CEM      | [HCLRE]       | [NORM]      | [36]    | 1.70E2         || D      | false | false | false | [HCLRE] | null   | null        | MATS HCl Rate || Not operating, method exists, one DHV exists, and parameters match.
        /// | 18 | true   |    0.0 | HCLRH     | CEM      | [HCLRH]       | [NORM]      | [36]    | 1.80E2         || D      | false | false | false | [HCLRH] | null   | null        | MATS HCl Rate || Not operating, method exists, one DHV exists, and parameters match.
        /// | 19 | true   |    0.1 | HCLRE     | CEM      | [HCLRE]       | [MS-1]      | [36]    | 1.90E2         || G      | false | false | false | [HCLRE] | null   | null        | MATS HCl Rate || CEM method with apportionment formula.
        /// | 20 | true   |    0.1 | HCLRH     | CEM      | [HCLRH]       | [MS-1]      | [36]    | 2.01E2         || G      | false | false | false | [HCLRH] | null   | null        | MATS HCl Rate || CEM method with apportionment formula.
        /// | 21 | true   |    0.1 | HCLRE     | CALC     | [HCLRE]       | [MS-1]      | [36]    | 2.30E2         || null   | false | false | false | [HCLRE] | 21     | 2.30E2      | MATS HCl Rate || CALC method with apportionment formula.
        /// | 22 | true   |    0.1 | HCLRH     | CALC     | [HCLRH]       | [MS-1]      | [36]    | 2.40E2         || null   | false | false | false | [HCLRH] | 22     | 2.40E2      | MATS HCl Rate || CALC method with apportionment formula.
        /// | 23 | true   |    0.1 | HCLRE     | CALC     | [HCLRE]       | [NORM]      | [36]    | 2.50E2         || F      | false | false | false | [HCLRE] | null   | null        | MATS HCl Rate || CALC method with non-apportionment formula.
        /// | 24 | true   |    0.1 | HCLRH     | CALC     | [HCLRH]       | [NORM]      | [36]    | 2.60E2         || F      | false | false | false | [HCLRH] | null   | null        | MATS HCl Rate || CALC method with non-apportionment formula.
        /// | 25 | true   |    0.1 | HCLRE     | CALC     | [HCLRE]       | null        | [36]    | 2.70E2         || F      | false | false | false | [HCLRE] | null   | null        | MATS HCl Rate || CALC method with null formula.
        /// | 26 | true   |    0.1 | HCLRH     | CALC     | [HCLRH]       | null        | [36]    | 2.80E2         || F      | false | false | false | [HCLRH] | null   | null        | MATS HCl Rate || CALC method with null formula.
        /// | 27 | true   |    0.1 | HCLRE     | CALC     | [HCLRE]       | [MS-1]      | [37]    | 2.90E2         || null   | false | false | false | [HCLRE] | 27     | 2.90E2      | MATS HCl Rate || CALC method with 37 MODC and apportionment formula.
        /// | 28 | true   |    0.1 | HCLRH     | CALC     | [HCLRH]       | [MS-1]      | [37]    | 3.01E2         || null   | false | false | false | [HCLRH] | 28     | 3.01E2      | MATS HCl Rate || CALC method with 37 MODC and apportionment formula.
        /// | 29 | true   |    0.1 | HCLRE     | CALC     | [HCLRE]       | null        | [37]    | 3.10E2         || F      | false | false | false | [HCLRE] | null   | null        | MATS HCl Rate || CALC method with 37 MODC and null formula.
        /// | 30 | true   |    0.1 | HCLRH     | CALC     | [HCLRH]       | null        | [37]    | 3.20E2         || F      | false | false | false | [HCLRH] | null   | null        | MATS HCl Rate || CALC method with 37 MODC and null formula.
        /// | 31 | true   |    0.1 | HCLRE     | CALC     | [HCLRE]       | [MS-1]      | [38]    | 3.30E2         || null   | false | false | false | [HCLRE] | null   | null        | MATS HCl Rate || CALC method with 38 MODC and apportionment formula.
        /// | 32 | true   |    0.1 | HCLRH     | CALC     | [HCLRH]       | [MS-1]      | [38]    | 3.40E2         || null   | false | false | false | [HCLRH] | null   | null        | MATS HCl Rate || CALC method with 38 MODC and apportionment formula.
        /// | 33 | true   |    0.1 | HCLRE     | CALC     | [HCLRE]       | null        | [38]    | 3.50E2         || H      | false | false | false | [HCLRE] | null   | null        | MATS HCl Rate || CALC method with 38 MODC and null formula.
        /// | 34 | true   |    0.1 | HCLRH     | CALC     | [HCLRH]       | null        | [38]    | 3.60E2         || H      | false | false | false | [HCLRH] | null   | null        | MATS HCl Rate || CALC method with 38 MODC and null formula.
        /// | 35 | true   |    0.1 | HCLRE     | CALC     | [HCLRE]       | [MS-1]      | [39]    | 3.70E2         || null   | false | false | false | [HCLRE] | 35     | 3.70E2      | MATS HCl Rate || CALC method with 39 MODC and apportionment formula.
        /// | 36 | true   |    0.1 | HCLRH     | CALC     | [HCLRH]       | [MS-1]      | [39]    | 3.80E2         || null   | false | false | false | [HCLRH] | 36     | 3.80E2      | MATS HCl Rate || CALC method with 39 MODC and apportionment formula.
        /// | 37 | true   |    0.1 | HCLRE     | CALC     | [HCLRE]       | null        | [39]    | 3.90E2         || F      | false | false | false | [HCLRE] | null   | null        | MATS HCl Rate || CALC method with 39 MODC and null formula.
        /// | 38 | true   |    0.1 | HCLRH     | CALC     | [HCLRH]       | null        | [39]    | 4.01E2         || F      | false | false | false | [HCLRH] | null   | null        | MATS HCl Rate || CALC method with 39 MODC and null formula.
        /// 
        /// </summary>
        [TestMethod()]
        public void MatsHod7()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            int caseCount = 39;

            /* Input Parameter Values */
            string[] dhvIdList = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31", "32", "33", "34", "35", "36", "37", "38" };
            bool?[] checksNeededList = { null, false, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true };
            decimal?[] opTimeNeededList = { 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.0m, 0.0m, 0.0m, 0.0m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m };
            string[] methodParameterList = { "HCLRE", "HCLRH", "HCLRE", "HCLRH", "HCLRE", "HCLRH", "HCLRE", "HCLRH", "HCLRE", "HCLRH", "HCLRE", "HCLRH", null, null, null, "HCLRE", "HCLRH", "HCLRE", "HCLRH", "HCLRE", "HCLRH", "HCLRE", "HCLRH", "HCLRE", "HCLRH", "HCLRE", "HCLRH", "HCLRE", "HCLRH", "HCLRE", "HCLRH", "HCLRE", "HCLRH", "HCLRE", "HCLRH", "HCLRE", "HCLRH", "HCLRE", "HCLRH" };
            string[] methodCodeList = { "CEM", "CEM", "CEM", "CEM", "CEM", "CEM", "CEM", "CEM", "CEM", "CEM", "CEM", "CEM", "CEM", "CEM", "CEM", "CEM", "CEM", "CEM", "CEM", "CEM", "CEM", "CALC", "CALC", "CALC", "CALC", "CALC", "CALC", "CALC", "CALC", "CALC", "CALC", "CALC", "CALC", "CALC", "CALC", "CALC", "CALC", "CALC", "CALC" };
            string[][] dhvParameterList = { new string[] { }, new string[] { }, new string[] { }, new string[] { }, new string[] { "HCLR" }, new string[] { "HCLR" }, new string[] { "HCLRE", "HCLRH" }, new string[] { "HCLRE", "HCLRH" }, new string[] { "HCLRH" }, new string[] { "HCLRE" }, new string[] { "HCLRE" }, new string[] { "HCLRH" }, new string[] { }, new string[] { "HCLRE" }, new string[] { "HCLRH" }, new string[] { }, new string[] { }, new string[] { "HCLRE" }, new string[] { "HCLRH" }, new string[] { "HCLRE" }, new string[] { "HCLRH" }, new string[] { "HCLRE" }, new string[] { "HCLRH" }, new string[] { "HCLRE" }, new string[] { "HCLRH" }, new string[] { "HCLRE" }, new string[] { "HCLRH" }, new string[] { "HCLRE" }, new string[] { "HCLRH" }, new string[] { "HCLRE" }, new string[] { "HCLRH" }, new string[] { "HCLRE" }, new string[] { "HCLRH" }, new string[] { "HCLRE" }, new string[] { "HCLRH" }, new string[] { "HCLRE" }, new string[] { "HCLRH" }, new string[] { "HCLRE" }, new string[] { "HCLRH" } };
            string[][] dhvEquationList = { new string[] { }, new string[] { }, new string[] { }, new string[] { }, new string[] { "NORM" }, new string[] { "NORM" }, new string[] { "NORM", "NORM" }, new string[] { "NORM", "NORM" }, new string[] { "NORM" }, new string[] { "NORM" }, new string[] { "NORM" }, new string[] { "NORM" }, new string[] { }, new string[] { "NORM" }, new string[] { "NORM" }, new string[] { }, new string[] { }, new string[] { "NORM" }, new string[] { "NORM" }, new string[] { "MS-1" }, new string[] { "MS-1" }, new string[] { "MS-1" }, new string[] { "MS-1" }, new string[] { "NORM" }, new string[] { "NORM" }, new string[] { null }, new string[] { null }, new string[] { "MS-1" }, new string[] { "MS-1" }, new string[] { null }, new string[] { null }, new string[] { "MS-1" }, new string[] { "MS-1" }, new string[] { null }, new string[] { null }, new string[] { "MS-1" }, new string[] { "MS-1" }, new string[] { null }, new string[] { null } };
            string[][] dhvModcList = { new string[] { }, new string[] { }, new string[] { }, new string[] { }, new string[] { "36" }, new string[] { "36" }, new string[] { "36", "36" }, new string[] { "36", "36" }, new string[] { "36" }, new string[] { "36" }, new string[] { "36" }, new string[] { "36" }, new string[] { }, new string[] { "36" }, new string[] { "36" }, new string[] { }, new string[] { }, new string[] { "36" }, new string[] { "36" }, new string[] { "36" }, new string[] { "36" }, new string[] { "36" }, new string[] { "36" }, new string[] { "36" }, new string[] { "36" }, new string[] { "36" }, new string[] { "36" }, new string[] { "37" }, new string[] { "37" }, new string[] { "37" }, new string[] { "37" }, new string[] { "38" }, new string[] { "38" }, new string[] { "38" }, new string[] { "38" }, new string[] { "39" }, new string[] { "39" }, new string[] { "39" }, new string[] { "39" } };
            string[] unadjHrlyValueList = {"0.01E2", "0.10E2", "0.20E2", "0.30E2", "0.40E2", "0.50E2", "0.60E2", "0.70E2", "0.80E2", "0.90E2",
                                           "1.01E2", "1.10E2", "1.20E2", "1.30E2", "1.40E2", "1.50E2", "1.60E2", "1.70E2", "1.80E2", "1.90E2",
                                           "2.01E2", "2.30E2", "2.40E2", "2.50E2", "2.60E2", "2.70E2", "2.80E2", "2.90E2", "3.01E2", "3.10E2",
                                           "3.20E2", "3.30E2", "3.40E2", "3.50E2", "3.60E2", "3.70E2", "3.80E2", "3.90E2", "4.01E2", };
            string[] appHclRateArrayList = { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null };
            
            /* Expected Values */
            string[] expectedResultList = { null, null, "E", "E", "E", "E", "B", "B", "C", "C", null, null, null, "A", "A", null, null, "D", "D", "G", "G", null, null, "F", "F", "F", "F", null, null, "F", "F", null, null, "H", "H", null, null, "F", "F" };
            bool[] expectedReNeededList = { false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false };
            bool[] expectedRhNeededList = { false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false };
            bool[] expectedConcNeededList = { false, false, false, false, false, false, false, false, false, false, true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false };
            string[] expectedParameterList = { null, null, null, null, null, null, null, null, "HCLRH", "HCLRE", "HCLRE", "HCLRH", null, null, null, null, null, null, null, "HCLRE", "HCLRH", "HCLRE", "HCLRH", "HCLRE", "HCLRH", "HCLRE", "HCLRH", "HCLRE", "HCLRH", "HCLRE", "HCLRH", "HCLRE", "HCLRH", "HCLRE", "HCLRH", "HCLRE", "HCLRH", "HCLRE", "HCLRH" };
            string[] expectedIdList = { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, "21", "22", null, null, null, null, "27", "28", null, null, null, null, null, null, "35", "36", null, null };
            string[] expectedUnadjHrlyList = { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, "2.30E2", "2.40E2", null, null, null, null, "2.90E2", "3.01E2", null, null, null, null, null, null, "3.70E2", "3.80E2", null, null };
            string expectedParameterDescription = "MATS HCl Rate";

            string[] expectedAppHclRateArray = {null, null, null, null, null, null, null, null, "0.80E2", "0.90E2",
                                              "1.01E2", "1.10E2", null, null, null, null, null, null, null, "1.90E2",
                                              "2.01E2", "2.30E2", "2.40E2", "2.50E2", "2.60E2", "2.70E2", "2.80E2", "2.90E2", "3.01E2", "3.10E2",
                                              "3.20E2", "3.30E2", "3.40E2", "3.50E2", "3.60E2", "3.70E2", "3.80E2", "3.90E2", "4.01E2", };
            string[] expectedMatsMs1ModcArray = {null, null, null, null, null, null, null, null, "36", "36",
                                              "36", "36", null, null, null, null, null, null, null, "36",
                                              "36", "36", "36", "36", "36", "36", "36", "36", "36", "37",
                                              "37", "37", "37", "38", "38", "38", "38", "39", "39", "39",
                                              "39", };

            /* Check array lengths */
            Assert.AreEqual(caseCount, dhvIdList.Length, "dhvIdList length");
            Assert.AreEqual(caseCount, checksNeededList.Length, "checksNeededList length");
            Assert.AreEqual(caseCount, opTimeNeededList.Length, "opTimeNeededList length");
            Assert.AreEqual(caseCount, methodParameterList.Length, "methodParameterList length");
            Assert.AreEqual(caseCount, methodCodeList.Length, "methodCodeList length");
            Assert.AreEqual(caseCount, dhvParameterList.Length, "dhvParameterList length");
            Assert.AreEqual(caseCount, dhvEquationList.Length, "dhvEquationList length");
            Assert.AreEqual(caseCount, dhvModcList.Length, "dhvModcList length");
            Assert.AreEqual(caseCount, expectedResultList.Length, "expectedResultList length");
            Assert.AreEqual(caseCount, expectedReNeededList.Length, "expectedReNeededList length");
            Assert.AreEqual(caseCount, expectedRhNeededList.Length, "expectedRhNeededList length");
            Assert.AreEqual(caseCount, expectedConcNeededList.Length, "expectedConcNeededList length");
            Assert.AreEqual(caseCount, expectedParameterList.Length, "expectedParameterList length");
            Assert.AreEqual(caseCount, expectedIdList.Length, "expectedIdList length");

            for (int caseDex = 0; caseDex < caseCount; caseDex++)
            {
                /* Generate MATS DHV REcord By Hour Location rows */
                MATSDerivedHourlyValueData[] MatsDhvRecordsByHourLocationRows;
                {
                    string[] parameterList = dhvParameterList[caseDex];
                    string[] equationList = dhvEquationList[caseDex];
                    string[] modcList = dhvModcList[caseDex];

                    MatsDhvRecordsByHourLocationRows = new MATSDerivedHourlyValueData[parameterList.Length];

                    for (int rowDex = 0; rowDex < parameterList.Length; rowDex++)
                        MatsDhvRecordsByHourLocationRows[rowDex] = new MATSDerivedHourlyValueData(matsDhvId: dhvIdList[caseDex], parameterCd: parameterList[rowDex]
                                                                                                , equationCd: equationList[rowDex], modcCd: modcList[rowDex]
                                                                                                , unadjustedHrlyValue: unadjHrlyValueList[caseDex]);
                }

                /* Initialize Input Parameters */
                EmParameters.CurrentHourlyOpRecord = new VwMpHrlyOpDataRow(opTime: opTimeNeededList[caseDex]);
                EmParameters.DerivedHourlyChecksNeeded = checksNeededList[caseDex];
                EmParameters.MatsDhvRecordsByHourLocation = new CheckDataView<MATSDerivedHourlyValueData>(MatsDhvRecordsByHourLocationRows);
                EmParameters.MatsHclMethodCode = methodCodeList[caseDex];
                EmParameters.MatsHclParameterCode = methodParameterList[caseDex];
                EmParameters.CurrentMonitorPlanLocationPostion = caseDex;

                /* Initialize Optional Parameters */
                EmParameters.MatsMs1HclDhvId = null;
                EmParameters.MatsMs1HclUnadjustedHourlyValue = null;
                category.Process.ProcessParameters.RegisterParameter(4647, "Apportionment_HCL_Rate_Array");
                category.SetCheckParameter("Apportionment_HCL_Rate_Array", appHclRateArrayList, eParameterDataType.String, false, true);
                category.Process.ProcessParameters.RegisterParameter(4674, "MATS_MS1_HCL_MODC_Code_Array");
                category.SetCheckParameter("MATS_MS1_HCL_MODC_Code_Array", expectedMatsMs1ModcArray, eParameterDataType.String, false, true);

                /* Initialize Output Parameters */
                EmParameters.MatsHclreDhvChecksNeeded = null;
                EmParameters.MatsHclrhDhvChecksNeeded = null;
                EmParameters.MatsHclcNeeded = null;
                EmParameters.MatsHclDhvRecord = new MATSDerivedHourlyValueData(); // Should always exit check as null or with parameter of HCLRE or HCLRH.
                EmParameters.MatsHclDhvParameterDescription = null;

                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Initialize variables needed to run the check. */
                bool log = false;
                string actual;

                /* Run Check */
                actual = cMATSOperatingHourChecks.MATSHOD7(category, ref log);

                /* Check results */
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);

                Assert.AreEqual(expectedResultList[caseDex], category.CheckCatalogResult, string.Format("Result {0}", caseDex));
                Assert.AreEqual(expectedParameterList[caseDex], (EmParameters.MatsHclDhvRecord != null ? EmParameters.MatsHclDhvRecord.ParameterCd : null), string.Format("MatsHclDhvRecord {0}", caseDex));
                Assert.AreEqual(expectedParameterDescription, EmParameters.MatsHclDhvParameterDescription, string.Format("MatsHclDhvParameterDescription {0}", caseDex));
                Assert.AreEqual(expectedConcNeededList[caseDex], EmParameters.MatsHclcNeeded, string.Format("MatsHclcNeeded {0}", caseDex));
                Assert.AreEqual(expectedReNeededList[caseDex], EmParameters.MatsHclreDhvChecksNeeded, string.Format("MatsHclreDhvChecksNeeded {0}", caseDex));
                Assert.AreEqual(expectedRhNeededList[caseDex], EmParameters.MatsHclrhDhvChecksNeeded, string.Format("MatsHclrhDhvChecksNeeded {0}", caseDex));
                Assert.AreEqual(expectedIdList[caseDex], EmParameters.MatsMs1HclDhvId, string.Format("MatsMs1HclDhvId {0}", caseDex));
                Assert.AreEqual(expectedUnadjHrlyList[caseDex], EmParameters.MatsMs1HclUnadjustedHourlyValue, string.Format("MatsMs1HclUnadjustedHourlyValue {0}", caseDex));
            }

            /* Check the array results */
            string[] apportionmentHclRateArray = (string[])category.GetCheckParameter("Apportionment_HCL_Rate_Array").ParameterValue;
            string[] matsMS1HclModcCodeArray = (string[])category.GetCheckParameter("MATS_MS1_HCL_MODC_Code_Array").ParameterValue;
            CollectionAssert.AreEqual(expectedAppHclRateArray, apportionmentHclRateArray, string.Format("ApportionmentHclRateArray does not equal the expected result."));
            CollectionAssert.AreEqual(expectedMatsMs1ModcArray, matsMS1HclModcCodeArray, string.Format("MatsMS1HclModcCodeArray does not equal the expected result."));
        }

        /// <summary>
        /// MatsHod8
        /// 
        /// | ## | Needed | OpTime | MethParam | MethCode | DhvParam      | DhvEquat    | dhvModc | UnadjHrlyValue || Result | Hgre  | Hgrh  | Hgc   | Record  | DHV ID | HfUnadjHrly | Description   || Note
        /// |  0 | null   |    0.1 | HFRE      | CEM      | []            | []          | []      | 0.01E2         || null   | false | false | false | null    | null   | null        | MATS HCl Rate || DerivedHourlyChecksNeeded is null (treat null as false).
        /// |  1 | false  |    0.1 | HFRH      | CEM      | []            | []          | []      | 0.10E2         || null   | false | false | false | null    | null   | null        | MATS HCl Rate || DerivedHourlyChecksNeeded is false.
        /// |  2 | true   |    0.1 | HFRE      | CEM      | []            | []          | []      | 0.20E2         || E      | false | false | false | null    | null   | null        | MATS HCl Rate || Method exists, but no DHV.
        /// |  3 | true   |    0.1 | HFRH      | CEM      | []            | []          | []      | 0.30E2         || E      | false | false | false | null    | null   | null        | MATS HCl Rate || Method exists, but no DHV.
        /// |  4 | true   |    0.1 | HFRE      | CEM      | [HFR]         | [NORM]      | [36]    | 0.40E2         || E      | false | false | false | null    | null   | null        | MATS HCl Rate || Method exists, but DHV only exists with excluded parameter.
        /// |  5 | true   |    0.1 | HFRH      | CEM      | [HFR]         | [NORM]      | [36]    | 0.50E2         || E      | false | false | false | null    | null   | null        | MATS HCl Rate || Method exists, but DHV only exists with excluded parameter.
        /// |  6 | true   |    0.1 | HFRE      | CEM      | [HFRE,HFRH]   | [NORM,NORM] | [36,36] | 0.60E2         || B      | false | false | false | null    | null   | null        | MATS HCl Rate || Method exists, but more than one DHV exists.
        /// |  7 | true   |    0.1 | HFRH      | CEM      | [HFRE,HFRH]   | [NORM,NORM] | [36,36] | 0.70E2         || B      | false | false | false | null    | null   | null        | MATS HCl Rate || Method exists, but more than one DHV exists.
        /// |  8 | true   |    0.1 | HFRE      | CEM      | [HFRH]        | [NORM]      | [36]    | 0.80E2         || C      | false | false | false | [HFRH]  | null   | null        | MATS HCl Rate || Method exists and one DHV exists, but parameters do not match.
        /// |  9 | true   |    0.1 | HFRH      | CEM      | [HFRE]        | [NORM]      | [36]    | 0.90E2         || C      | false | false | false | [HFRE]  | null   | null        | MATS HCl Rate || Method exists and one DHV exists, but parameters do not match.
        /// | 10 | true   |    0.1 | HFRE      | CEM      | [HFRE]        | [NORM]      | [36]    | 1.01E2         || null   | true  | false | true  | [HFRE]  | null   | null        | MATS HCl Rate || Method exists, one DHV exists, and parameters match.
        /// | 11 | true   |    0.1 | HFRH      | CEM      | [HFRH]        | [NORM]      | [36]    | 1.10E2         || null   | false | true  | true  | [HFRH]  | null   | null        | MATS HCl Rate || Method exists, one DHV exists, and parameters match.
        /// | 12 | true   |    0.1 | null      | CEM      | []            | []          | []      | 1.20E2         || null   | false | false | false | null    | null   | null        | MATS HCl Rate || Neither Method nor DHV exist.
        /// | 13 | true   |    0.1 | null      | CEM      | [HFRE]        | [NORM]      | [36]    | 1.30E2         || A      | false | false | false | null    | null   | null        | MATS HCl Rate || Method does not exist, but DHV exists.
        /// | 14 | true   |    0.1 | null      | CEM      | [HFRH]        | [NORM]      | [36]    | 1.40E2         || A      | false | false | false | null    | null   | null        | MATS HCl Rate || Method does not exist, but DHV exists.
        /// | 15 | true   |    0.0 | HFRE      | CEM      | []            | []          | []      | 1.50E2         || null   | false | false | false | null    | null   | null        | MATS HCl Rate || Not operating, method exists, and no DHV.
        /// | 16 | true   |    0.0 | HFRH      | CEM      | []            | []          | []      | 1.60E2         || null   | false | false | false | null    | null   | null        | MATS HCl Rate || Not operating, method exists, and no DHV.
        /// | 17 | true   |    0.0 | HFRE      | CEM      | [HFRE]        | [NORM]      | [36]    | 1.70E2         || D      | false | false | false | [HFRE]  | null   | null        | MATS HCl Rate || Not operating, method exists, one DHV exists, and parameters match.
        /// | 18 | true   |    0.0 | HFRH      | CEM      | [HFRH]        | [NORM]      | [36]    | 1.80E2         || D      | false | false | false | [HFRH]  | null   | null        | MATS HCl Rate || Not operating, method exists, one DHV exists, and parameters match.
        /// | 19 | true   |    0.1 | HFRE      | CEM      | [HFRE]        | [MS-1]      | [36]    | 1.90E2         || G      | false | false | false | [HFRE]  | null   | null        | MATS HCl Rate || CEM method with apportionment formula.
        /// | 20 | true   |    0.1 | HFRH      | CEM      | [HFRH]        | [MS-1]      | [36]    | 2.01E2         || G      | false | false | false | [HFRH]  | null   | null        | MATS HCl Rate || CEM method with apportionment formula.
        /// | 21 | true   |    0.1 | HFRE      | CALC     | [HFRE]        | [MS-1]      | [36]    | 2.30E2         || null   | false | false | false | [HFRE]  | 21     | 2.30E2      | MATS HCl Rate || CALC method with apportionment formula.
        /// | 22 | true   |    0.1 | HFRH      | CALC     | [HFRH]        | [MS-1]      | [36]    | 2.40E2         || null   | false | false | false | [HFRH]  | 22     | 2.40E2      | MATS HCl Rate || CALC method with apportionment formula.
        /// | 23 | true   |    0.1 | HFRE      | CALC     | [HFRE]        | [NORM]      | [36]    | 2.50E2         || F      | false | false | false | [HFRE]  | null   | null        | MATS HCl Rate || CALC method with non-apportionment formula.
        /// | 24 | true   |    0.1 | HFRH      | CALC     | [HFRH]        | [NORM]      | [36]    | 2.60E2         || F      | false | false | false | [HFRH]  | null   | null        | MATS HCl Rate || CALC method with non-apportionment formula.
        /// | 25 | true   |    0.1 | HFRE      | CALC     | [HFRE]        | null        | [36]    | 2.70E2         || F      | false | false | false | [HFRE]  | null   | null        | MATS HCl Rate || CALC method with null formula.
        /// | 26 | true   |    0.1 | HFRH      | CALC     | [HFRH]        | null        | [36]    | 2.80E2         || F      | false | false | false | [HFRH]  | null   | null        | MATS HCl Rate || CALC method with null formula.
        /// | 27 | true   |    0.1 | HFRE      | CALC     | [HFRE]        | [MS-1]      | [37]    | 2.90E2         || null   | false | false | false | [HFRE]  | 27     | 2.90E2      | MATS HCl Rate || CALC method with 37 MODC and apportionment formula.
        /// | 28 | true   |    0.1 | HFRH      | CALC     | [HFRH]        | [MS-1]      | [37]    | 3.01E2         || null   | false | false | false | [HFRH]  | 28     | 3.01E2      | MATS HCl Rate || CALC method with 37 MODC and apportionment formula.
        /// | 29 | true   |    0.1 | HFRE      | CALC     | [HFRE]        | null        | [37]    | 3.10E2         || F      | false | false | false | [HFRE]  | null   | null        | MATS HCl Rate || CALC method with 37 MODC and null formula.
        /// | 30 | true   |    0.1 | HFRH      | CALC     | [HFRH]        | null        | [37]    | 3.20E2         || F      | false | false | false | [HFRH]  | null   | null        | MATS HCl Rate || CALC method with 37 MODC and null formula.
        /// | 31 | true   |    0.1 | HFRE      | CALC     | [HFRE]        | [MS-1]      | [38]    | 3.30E2         || null   | false | false | false | [HFRE]  | null   | null        | MATS HCl Rate || CALC method with 38 MODC and apportionment formula.
        /// | 32 | true   |    0.1 | HFRH      | CALC     | [HFRH]        | [MS-1]      | [38]    | 3.40E2         || null   | false | false | false | [HFRH]  | null   | null        | MATS HCl Rate || CALC method with 38 MODC and apportionment formula.
        /// | 33 | true   |    0.1 | HFRE      | CALC     | [HFRE]        | null        | [38]    | 3.50E2         || H      | false | false | false | [HFRE]  | null   | null        | MATS HCl Rate || CALC method with 38 MODC and null formula.
        /// | 34 | true   |    0.1 | HFRH      | CALC     | [HFRH]        | null        | [38]    | 3.60E2         || H      | false | false | false | [HFRH]  | null   | null        | MATS HCl Rate || CALC method with 38 MODC and null formula.
        /// | 35 | true   |    0.1 | HFRE      | CALC     | [HFRE]        | [MS-1]      | [39]    | 3.70E2         || null   | false | false | false | [HFRE]  | 35     | 3.70E2      | MATS HCl Rate || CALC method with 39 MODC and apportionment formula.
        /// | 36 | true   |    0.1 | HFRH      | CALC     | [HFRH]        | [MS-1]      | [39]    | 3.80E2         || null   | false | false | false | [HFRH]  | 36     | 3.80E2      | MATS HCl Rate || CALC method with 39 MODC and apportionment formula.
        /// | 37 | true   |    0.1 | HFRE      | CALC     | [HFRE]        | null        | [39]    | 3.90E2         || F      | false | false | false | [HFRE]  | null   | null        | MATS HCl Rate || CALC method with 39 MODC and null formula.
        /// | 38 | true   |    0.1 | HFRH      | CALC     | [HFRH]        | null        | [39]    | 4.01E2         || F      | false | false | false | [HFRH]  | null   | null        | MATS HCl Rate || CALC method with 39 MODC and null formula.
        /// 
        /// </summary>
        [TestMethod()]
        public void MatsHod8()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            int caseCount = 39;

            /* Input Parameter Values */
            string[] dhvIdList = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31", "32", "33", "34", "35", "36", "37", "38" };
            bool?[] checksNeededList = { null, false, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true };
            decimal?[] opTimeNeededList = { 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.0m, 0.0m, 0.0m, 0.0m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m };
            string[] methodParameterList = { "HFRE", "HFRH", "HFRE", "HFRH", "HFRE", "HFRH", "HFRE", "HFRH", "HFRE", "HFRH", "HFRE", "HFRH", null, null, null, "HFRE", "HFRH", "HFRE", "HFRH", "HFRE", "HFRH", "HFRE", "HFRH", "HFRE", "HFRH", "HFRE", "HFRH", "HFRE", "HFRH", "HFRE", "HFRH", "HFRE", "HFRH", "HFRE", "HFRH", "HFRE", "HFRH", "HFRE", "HFRH" };
            string[] methodCodeList = { "CEM", "CEM", "CEM", "CEM", "CEM", "CEM", "CEM", "CEM", "CEM", "CEM", "CEM", "CEM", "CEM", "CEM", "CEM", "CEM", "CEM", "CEM", "CEM", "CEM", "CEM", "CALC", "CALC", "CALC", "CALC", "CALC", "CALC", "CALC", "CALC", "CALC", "CALC", "CALC", "CALC", "CALC", "CALC", "CALC", "CALC", "CALC", "CALC" };
            string[][] dhvParameterList = { new string[] { }, new string[] { }, new string[] { }, new string[] { }, new string[] { "HFR" }, new string[] { "HFR" }, new string[] { "HFRE", "HFRH" }, new string[] { "HFRE", "HFRH" }, new string[] { "HFRH" }, new string[] { "HFRE" }, new string[] { "HFRE" }, new string[] { "HFRH" }, new string[] { }, new string[] { "HFRE" }, new string[] { "HFRH" }, new string[] { }, new string[] { }, new string[] { "HFRE" }, new string[] { "HFRH" }, new string[] { "HFRE" }, new string[] { "HFRH" }, new string[] { "HFRE" }, new string[] { "HFRH" }, new string[] { "HFRE" }, new string[] { "HFRH" }, new string[] { "HFRE" }, new string[] { "HFRH" }, new string[] { "HFRE" }, new string[] { "HFRH" }, new string[] { "HFRE" }, new string[] { "HFRH" }, new string[] { "HFRE" }, new string[] { "HFRH" }, new string[] { "HFRE" }, new string[] { "HFRH" }, new string[] { "HFRE" }, new string[] { "HFRH" }, new string[] { "HFRE" }, new string[] { "HFRH" } };
            string[][] dhvEquationList = { new string[] { }, new string[] { }, new string[] { }, new string[] { }, new string[] { "NORM" }, new string[] { "NORM" }, new string[] { "NORM", "NORM" }, new string[] { "NORM", "NORM" }, new string[] { "NORM" }, new string[] { "NORM" }, new string[] { "NORM" }, new string[] { "NORM" }, new string[] { }, new string[] { "NORM" }, new string[] { "NORM" }, new string[] { }, new string[] { }, new string[] { "NORM" }, new string[] { "NORM" }, new string[] { "MS-1" }, new string[] { "MS-1" }, new string[] { "MS-1" }, new string[] { "MS-1" }, new string[] { "NORM" }, new string[] { "NORM" }, new string[] { null }, new string[] { null }, new string[] { "MS-1" }, new string[] { "MS-1" }, new string[] { null }, new string[] { null }, new string[] { "MS-1" }, new string[] { "MS-1" }, new string[] { null }, new string[] { null }, new string[] { "MS-1" }, new string[] { "MS-1" }, new string[] { null }, new string[] { null } };
            string[][] dhvModcList = { new string[] { }, new string[] { }, new string[] { }, new string[] { }, new string[] { "36" }, new string[] { "36" }, new string[] { "36", "36" }, new string[] { "36", "36" }, new string[] { "36" }, new string[] { "36" }, new string[] { "36" }, new string[] { "36" }, new string[] { }, new string[] { "36" }, new string[] { "36" }, new string[] { }, new string[] { }, new string[] { "36" }, new string[] { "36" }, new string[] { "36" }, new string[] { "36" }, new string[] { "36" }, new string[] { "36" }, new string[] { "36" }, new string[] { "36" }, new string[] { "36" }, new string[] { "36" }, new string[] { "37" }, new string[] { "37" }, new string[] { "37" }, new string[] { "37" }, new string[] { "38" }, new string[] { "38" }, new string[] { "38" }, new string[] { "38" }, new string[] { "39" }, new string[] { "39" }, new string[] { "39" }, new string[] { "39" } };
            string[] unadjHrlyValueList = {"0.01E2", "0.10E2", "0.20E2", "0.30E2", "0.40E2", "0.50E2", "0.60E2", "0.70E2", "0.80E2", "0.90E2",
                                           "1.01E2", "1.10E2", "1.20E2", "1.30E2", "1.40E2", "1.50E2", "1.60E2", "1.70E2", "1.80E2", "1.90E2",
                                           "2.01E2", "2.30E2", "2.40E2", "2.50E2", "2.60E2", "2.70E2", "2.80E2", "2.90E2", "3.01E2", "3.10E2",
                                           "3.20E2", "3.30E2", "3.40E2", "3.50E2", "3.60E2", "3.70E2", "3.80E2", "3.90E2", "4.01E2", };
            string[] appHfRateArrayList = { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null };

            /* Expected Values */
            string[] expectedResultList = { null, null, "E", "E", "E", "E", "B", "B", "C", "C", null, null, null, "A", "A", null, null, "D", "D", "G", "G", null, null, "F", "F", "F", "F", null, null, "F", "F", null, null, "H", "H", null, null, "F", "F" };
            bool[] expectedReNeededList = { false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false };
            bool[] expectedRhNeededList = { false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false };
            bool[] expectedConcNeededList = { false, false, false, false, false, false, false, false, false, false, true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false };
            string[] expectedParameterList = { null, null, null, null, null, null, null, null, "HFRH", "HFRE", "HFRE", "HFRH", null, null, null, null, null, null, null, "HFRE", "HFRH", "HFRE", "HFRH", "HFRE", "HFRH", "HFRE", "HFRH", "HFRE", "HFRH", "HFRE", "HFRH", "HFRE", "HFRH", "HFRE", "HFRH", "HFRE", "HFRH", "HFRE", "HFRH" };
            string[] expectedIdList = { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, "21", "22", null, null, null, null, "27", "28", null, null, null, null, null, null, "35", "36", null, null };
            string[] expectedUnadjHrlyList = { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, "2.30E2", "2.40E2", null, null, null, null, "2.90E2", "3.01E2", null, null, null, null, null, null, "3.70E2", "3.80E2", null, null };
            string expectedParameterDescription = "MATS HF Rate";

            string[] expectedAppHfRateArray = {null, null, null, null, null, null, null, null, "0.80E2", "0.90E2",
                                              "1.01E2", "1.10E2", null, null, null, null, null, null, null, "1.90E2",
                                              "2.01E2", "2.30E2", "2.40E2", "2.50E2", "2.60E2", "2.70E2", "2.80E2", "2.90E2", "3.01E2", "3.10E2",
                                              "3.20E2", "3.30E2", "3.40E2", "3.50E2", "3.60E2", "3.70E2", "3.80E2", "3.90E2", "4.01E2", };
            string[] expectedMatsMs1ModcArray = {null, null, null, null, null, null, null, null, "36", "36",
                                              "36", "36", null, null, null, null, null, null, null, "36",
                                              "36", "36", "36", "36", "36", "36", "36", "36", "36", "37",
                                              "37", "37", "37", "38", "38", "38", "38", "39", "39", "39",
                                              "39", };

            /* Check array lengths */
            Assert.AreEqual(caseCount, dhvIdList.Length, "dhvIdList length");
            Assert.AreEqual(caseCount, checksNeededList.Length, "checksNeededList length");
            Assert.AreEqual(caseCount, opTimeNeededList.Length, "opTimeNeededList length");
            Assert.AreEqual(caseCount, methodParameterList.Length, "methodParameterList length");
            Assert.AreEqual(caseCount, methodCodeList.Length, "methodCodeList length");
            Assert.AreEqual(caseCount, dhvParameterList.Length, "dhvParameterList length");
            Assert.AreEqual(caseCount, dhvEquationList.Length, "dhvEquationList length");
            Assert.AreEqual(caseCount, dhvModcList.Length, "dhvModcList length");
            Assert.AreEqual(caseCount, expectedResultList.Length, "expectedResultList length");
            Assert.AreEqual(caseCount, expectedReNeededList.Length, "expectedReNeededList length");
            Assert.AreEqual(caseCount, expectedRhNeededList.Length, "expectedRhNeededList length");
            Assert.AreEqual(caseCount, expectedConcNeededList.Length, "expectedConcNeededList length");
            Assert.AreEqual(caseCount, expectedParameterList.Length, "expectedParameterList length");
            Assert.AreEqual(caseCount, expectedIdList.Length, "expectedIdList length");

            for (int caseDex = 0; caseDex < caseCount; caseDex++)
            {
                /* Generate MATS DHV REcord By Hour Location rows */
                MATSDerivedHourlyValueData[] MatsDhvRecordsByHourLocationRows;
                {
                    string[] parameterList = dhvParameterList[caseDex];
                    string[] equationList = dhvEquationList[caseDex];
                    string[] modcList = dhvModcList[caseDex];

                    MatsDhvRecordsByHourLocationRows = new MATSDerivedHourlyValueData[parameterList.Length];

                    for (int rowDex = 0; rowDex < parameterList.Length; rowDex++)
                        MatsDhvRecordsByHourLocationRows[rowDex] = new MATSDerivedHourlyValueData(matsDhvId: dhvIdList[caseDex], parameterCd: parameterList[rowDex]
                                                                                                , equationCd: equationList[rowDex], modcCd: modcList[rowDex]
                                                                                                , unadjustedHrlyValue: unadjHrlyValueList[caseDex]);
                }

                /* Initialize Input Parameters */
                EmParameters.CurrentHourlyOpRecord = new VwMpHrlyOpDataRow(opTime: opTimeNeededList[caseDex]);
                EmParameters.DerivedHourlyChecksNeeded = checksNeededList[caseDex];
                EmParameters.MatsDhvRecordsByHourLocation = new CheckDataView<MATSDerivedHourlyValueData>(MatsDhvRecordsByHourLocationRows);
                EmParameters.MatsHfMethodCode = methodCodeList[caseDex];
                EmParameters.MatsHfParameterCode = methodParameterList[caseDex];
                EmParameters.CurrentMonitorPlanLocationPostion = caseDex;

                /* Initialize Optional Parameters */
                EmParameters.MatsMs1HfDhvId = null;
                EmParameters.MatsMs1HfUnadjustedHourlyValue = null;
                category.Process.ProcessParameters.RegisterParameter(4648, "Apportionment_HF_Rate_Array");
                category.SetCheckParameter("Apportionment_HF_Rate_Array", appHfRateArrayList, eParameterDataType.String, false, true);
                category.Process.ProcessParameters.RegisterParameter(4675, "MATS_MS1_HF_MODC_Code_Array");
                category.SetCheckParameter("MATS_MS1_HF_MODC_Code_Array", expectedMatsMs1ModcArray, eParameterDataType.String, false, true);

                /* Initialize Output Parameters */
                EmParameters.MatsHfreDhvChecksNeeded = null;
                EmParameters.MatsHfrhDhvChecksNeeded = null;
                EmParameters.MatsHfcNeeded = null;
                EmParameters.MatsHfDhvRecord = new MATSDerivedHourlyValueData(); // Should always exit check as null or with parameter of HFRE or HFRH.
                EmParameters.MatsHfDhvParameterDescription = null;

                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Initialize variables needed to run the check. */
                bool log = false;
                string actual;

                /* Run Check */
                actual = cMATSOperatingHourChecks.MATSHOD8(category, ref log);

                /* Check results */
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);

                Assert.AreEqual(expectedResultList[caseDex], category.CheckCatalogResult, string.Format("Result {0}", caseDex));
                Assert.AreEqual(expectedParameterList[caseDex], (EmParameters.MatsHfDhvRecord != null ? EmParameters.MatsHfDhvRecord.ParameterCd : null), string.Format("MatsHfDhvRecord {0}", caseDex));
                Assert.AreEqual(expectedParameterDescription, EmParameters.MatsHfDhvParameterDescription, string.Format("MatsHfDhvParameterDescription {0}", caseDex));
                Assert.AreEqual(expectedConcNeededList[caseDex], EmParameters.MatsHfcNeeded, string.Format("MatsHfcNeeded {0}", caseDex));
                Assert.AreEqual(expectedReNeededList[caseDex], EmParameters.MatsHfreDhvChecksNeeded, string.Format("MatsHfreDhvChecksNeeded {0}", caseDex));
                Assert.AreEqual(expectedRhNeededList[caseDex], EmParameters.MatsHfrhDhvChecksNeeded, string.Format("MatsHfrhDhvChecksNeeded {0}", caseDex));
                Assert.AreEqual(expectedIdList[caseDex], EmParameters.MatsMs1HfDhvId, string.Format("MatsMs1HfDhvId {0}", caseDex));
                Assert.AreEqual(expectedUnadjHrlyList[caseDex], EmParameters.MatsMs1HfUnadjustedHourlyValue, string.Format("MatsMs1HfUnadjustedHourlyValue {0}", caseDex));
            }
            
            /* Check the array results */
            string[] apportionmentHfRateArray = (string[])category.GetCheckParameter("Apportionment_Hf_Rate_Array").ParameterValue;
            string[] matsMS1HfModcCodeArray = (string[])category.GetCheckParameter("MATS_MS1_HF_MODC_Code_Array").ParameterValue;
            CollectionAssert.AreEqual(expectedAppHfRateArray, apportionmentHfRateArray, string.Format("ApportionmentHfRateArray does not equal the expected result."));
            CollectionAssert.AreEqual(expectedMatsMs1ModcArray, matsMS1HfModcCodeArray, string.Format("MatsMS1HfModcCodeArray does not equal the expected result."));
        }

        /// <summary>
        /// MatsHod9
        /// 
        /// | ## | Needed | OpTime | MethParam | MethCode | DhvParam      | DhvEquat    | dhvModc | UnadjHrlyValue || Result | Hgre  | Hgrh  | Hgc   | Record  | DHV ID | So2UnadjHrly| Description   || Note
        /// |  0 | null   |    0.1 | SO2RE     | CEM      | []            | []          | []      | 0.01E2         || null   | false | false | false | null    | null   | null        | MATS HCl Rate || DerivedHourlyChecksNeeded is null (treat null as false).
        /// |  1 | false  |    0.1 | SO2RH     | CEM      | []            | []          | []      | 0.10E2         || null   | false | false | false | null    | null   | null        | MATS HCl Rate || DerivedHourlyChecksNeeded is false.
        /// |  2 | true   |    0.1 | SO2RE     | CEM      | []            | []          | []      | 0.20E2         || E      | false | false | false | null    | null   | null        | MATS HCl Rate || Method exists, but no DHV.
        /// |  3 | true   |    0.1 | SO2RH     | CEM      | []            | []          | []      | 0.30E2         || E      | false | false | false | null    | null   | null        | MATS HCl Rate || Method exists, but no DHV.
        /// |  4 | true   |    0.1 | SO2RE     | CEM      | [SO2R]        | [NORM]      | [36]    | 0.40E2         || E      | false | false | false | null    | null   | null        | MATS HCl Rate || Method exists, but DHV only exists with excluded parameter.
        /// |  5 | true   |    0.1 | SO2RH     | CEM      | [SO2R]        | [NORM]      | [36]    | 0.50E2         || E      | false | false | false | null    | null   | null        | MATS HCl Rate || Method exists, but DHV only exists with excluded parameter.
        /// |  6 | true   |    0.1 | SO2RE     | CEM      | [SO2RE,SO2RH] | [NORM,NORM] | [36,36] | 0.60E2         || B      | false | false | false | null    | null   | null        | MATS HCl Rate || Method exists, but more than one DHV exists.
        /// |  7 | true   |    0.1 | SO2RH     | CEM      | [SO2RE,SO2RH] | [NORM,NORM] | [36,36] | 0.70E2         || B      | false | false | false | null    | null   | null        | MATS HCl Rate || Method exists, but more than one DHV exists.
        /// |  8 | true   |    0.1 | SO2RE     | CEM      | [SO2RH]       | [NORM]      | [36]    | 0.80E2         || C      | false | false | false | [SO2RH] | null   | null        | MATS HCl Rate || Method exists and one DHV exists, but parameters do not match.
        /// |  9 | true   |    0.1 | SO2RH     | CEM      | [SO2RE]       | [NORM]      | [36]    | 0.90E2         || C      | false | false | false | [SO2RE] | null   | null        | MATS HCl Rate || Method exists and one DHV exists, but parameters do not match.
        /// | 10 | true   |    0.1 | SO2RE     | CEM      | [SO2RE]       | [NORM]      | [36]    | 1.01E2         || null   | true  | false | true  | [SO2RE] | null   | null        | MATS HCl Rate || Method exists, one DHV exists, and parameters match.
        /// | 11 | true   |    0.1 | SO2RH     | CEM      | [SO2RH]       | [NORM]      | [36]    | 1.10E2         || null   | false | true  | true  | [SO2RH] | null   | null        | MATS HCl Rate || Method exists, one DHV exists, and parameters match.
        /// | 12 | true   |    0.1 | null      | CEM      | []            | []          | []      | 1.20E2         || null   | false | false | false | null    | null   | null        | MATS HCl Rate || Neither Method nor DHV exist.
        /// | 13 | true   |    0.1 | null      | CEM      | [SO2RE]       | [NORM]      | [36]    | 1.30E2         || A      | false | false | false | null    | null   | null        | MATS HCl Rate || Method does not exist, but DHV exists.
        /// | 14 | true   |    0.1 | null      | CEM      | [SO2RH]       | [NORM]      | [36]    | 1.40E2         || A      | false | false | false | null    | null   | null        | MATS HCl Rate || Method does not exist, but DHV exists.
        /// | 15 | true   |    0.0 | SO2RE     | CEM      | []            | []          | []      | 1.50E2         || null   | false | false | false | null    | null   | null        | MATS HCl Rate || Not operating, method exists, and no DHV.
        /// | 16 | true   |    0.0 | SO2RH     | CEM      | []            | []          | []      | 1.60E2         || null   | false | false | false | null    | null   | null        | MATS HCl Rate || Not operating, method exists, and no DHV.
        /// | 17 | true   |    0.0 | SO2RE     | CEM      | [SO2RE]       | [NORM]      | [36]    | 1.70E2         || D      | false | false | false | [SO2RE] | null   | null        | MATS HCl Rate || Not operating, method exists, one DHV exists, and parameters match.
        /// | 18 | true   |    0.0 | SO2RH     | CEM      | [SO2RH]       | [NORM]      | [36]    | 1.80E2         || D      | false | false | false | [SO2RH] | null   | null        | MATS HCl Rate || Not operating, method exists, one DHV exists, and parameters match.
        /// | 19 | true   |    0.1 | SO2RE     | CEM      | [SO2RE]       | [MS-1]      | [36]    | 1.90E2         || G      | false | false | false | [SO2RE] | null   | null        | MATS HCl Rate || CEM method with apportionment formula.
        /// | 20 | true   |    0.1 | SO2RH     | CEM      | [SO2RH]       | [MS-1]      | [36]    | 2.01E2         || G      | false | false | false | [SO2RH] | null   | null        | MATS HCl Rate || CEM method with apportionment formula.
        /// | 21 | true   |    0.1 | SO2RE     | CALC     | [SO2RE]       | [MS-1]      | [36]    | 2.30E2         || null   | false | false | false | [SO2RE] | 21     | 2.30E2      | MATS HCl Rate || CALC method with apportionment formula.
        /// | 22 | true   |    0.1 | SO2RH     | CALC     | [SO2RH]       | [MS-1]      | [36]    | 2.40E2         || null   | false | false | false | [SO2RH] | 22     | 2.40E2      | MATS HCl Rate || CALC method with apportionment formula.
        /// | 23 | true   |    0.1 | SO2RE     | CALC     | [SO2RE]       | [NORM]      | [36]    | 2.50E2         || F      | false | false | false | [SO2RE] | null   | null        | MATS HCl Rate || CALC method with non-apportionment formula.
        /// | 24 | true   |    0.1 | SO2RH     | CALC     | [SO2RH]       | [NORM]      | [36]    | 2.60E2         || F      | false | false | false | [SO2RH] | null   | null        | MATS HCl Rate || CALC method with non-apportionment formula.
        /// | 25 | true   |    0.1 | SO2RE     | CALC     | [SO2RE]       | null        | [36]    | 2.70E2         || F      | false | false | false | [SO2RE] | null   | null        | MATS HCl Rate || CALC method with null formula.
        /// | 26 | true   |    0.1 | SO2RH     | CALC     | [SO2RH]       | null        | [36]    | 2.80E2         || F      | false | false | false | [SO2RH] | null   | null        | MATS HCl Rate || CALC method with null formula.
        /// | 27 | true   |    0.1 | SO2RE     | CALC     | [SO2RE]       | [MS-1]      | [37]    | 2.90E2         || null   | false | false | false | [SO2RE] | 27     | 2.90E2      | MATS HCl Rate || CALC method with 37 MODC and apportionment formula.
        /// | 28 | true   |    0.1 | SO2RH     | CALC     | [SO2RH]       | [MS-1]      | [37]    | 3.01E2         || null   | false | false | false | [SO2RH] | 28     | 3.01E2      | MATS HCl Rate || CALC method with 37 MODC and apportionment formula.
        /// | 29 | true   |    0.1 | SO2RE     | CALC     | [SO2RE]       | null        | [37]    | 3.10E2         || F      | false | false | false | [SO2RE] | null   | null        | MATS HCl Rate || CALC method with 37 MODC and null formula.
        /// | 30 | true   |    0.1 | SO2RH     | CALC     | [SO2RH]       | null        | [37]    | 3.20E2         || F      | false | false | false | [SO2RH] | null   | null        | MATS HCl Rate || CALC method with 37 MODC and null formula.
        /// | 31 | true   |    0.1 | SO2RE     | CALC     | [SO2RE]       | [MS-1]      | [38]    | 3.30E2         || null   | false | false | false | [SO2RE] | null   | null        | MATS HCl Rate || CALC method with 38 MODC and apportionment formula.
        /// | 32 | true   |    0.1 | SO2RH     | CALC     | [SO2RH]       | [MS-1]      | [38]    | 3.40E2         || null   | false | false | false | [SO2RH] | null   | null        | MATS HCl Rate || CALC method with 38 MODC and apportionment formula.
        /// | 33 | true   |    0.1 | SO2RE     | CALC     | [SO2RE]       | null        | [38]    | 3.50E2         || H      | false | false | false | [SO2RE] | null   | null        | MATS HCl Rate || CALC method with 38 MODC and null formula.
        /// | 34 | true   |    0.1 | SO2RH     | CALC     | [SO2RH]       | null        | [38]    | 3.60E2         || H      | false | false | false | [SO2RH] | null   | null        | MATS HCl Rate || CALC method with 38 MODC and null formula.
        /// | 35 | true   |    0.1 | SO2RE     | CALC     | [SO2RE]       | [MS-1]      | [39]    | 3.70E2         || null   | false | false | false | [SO2RE] | 35     | 3.70E2      | MATS HCl Rate || CALC method with 39 MODC and apportionment formula.
        /// | 36 | true   |    0.1 | SO2RH     | CALC     | [SO2RH]       | [MS-1]      | [39]    | 3.80E2         || null   | false | false | false | [SO2RH] | 36     | 3.80E2      | MATS HCl Rate || CALC method with 39 MODC and apportionment formula.
        /// | 37 | true   |    0.1 | SO2RE     | CALC     | [SO2RE]       | null        | [39]    | 3.90E2         || F      | false | false | false | [SO2RE] | null   | null        | MATS HCl Rate || CALC method with 39 MODC and null formula.
        /// | 38 | true   |    0.1 | SO2RH     | CALC     | [SO2RH]       | null        | [39]    | 4.01E2         || F      | false | false | false | [SO2RH] | null   | null        | MATS HCl Rate || CALC method with 39 MODC and null formula.
        /// 
        /// </summary>
        [TestMethod()]
        public void MatsHod9()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            int caseCount = 39;

            /* Input Parameter Values */
            string[] dhvIdList = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31", "32", "33", "34", "35", "36", "37", "38" };
            bool?[] checksNeededList = { null, false, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true };
            decimal?[] opTimeNeededList = { 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.0m, 0.0m, 0.0m, 0.0m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m };
            string[] methodParameterList = { "SO2RE", "SO2RH", "SO2RE", "SO2RH", "SO2RE", "SO2RH", "SO2RE", "SO2RH", "SO2RE", "SO2RH", "SO2RE", "SO2RH", null, null, null, "SO2RE", "SO2RH", "SO2RE", "SO2RH", "SO2RE", "SO2RH", "SO2RE", "SO2RH", "SO2RE", "SO2RH", "SO2RE", "SO2RH", "SO2RE", "SO2RH", "SO2RE", "SO2RH", "SO2RE", "SO2RH", "SO2RE", "SO2RH", "SO2RE", "SO2RH", "SO2RE", "SO2RH" };
            string[] methodCodeList = { "CEM", "CEM", "CEM", "CEM", "CEM", "CEM", "CEM", "CEM", "CEM", "CEM", "CEM", "CEM", "CEM", "CEM", "CEM", "CEM", "CEM", "CEM", "CEM", "CEM", "CEM", "CALC", "CALC", "CALC", "CALC", "CALC", "CALC", "CALC", "CALC", "CALC", "CALC", "CALC", "CALC", "CALC", "CALC", "CALC", "CALC", "CALC", "CALC" };
            string[][] dhvParameterList = { new string[] { }, new string[] { }, new string[] { }, new string[] { }, new string[] { "SO2R" }, new string[] { "SO2R" }, new string[] { "SO2RE", "SO2RH" }, new string[] { "SO2RE", "SO2RH" }, new string[] { "SO2RH" }, new string[] { "SO2RE" }, new string[] { "SO2RE" }, new string[] { "SO2RH" }, new string[] { }, new string[] { "SO2RE" }, new string[] { "SO2RH" }, new string[] { }, new string[] { }, new string[] { "SO2RE" }, new string[] { "SO2RH" }, new string[] { "SO2RE" }, new string[] { "SO2RH" }, new string[] { "SO2RE" }, new string[] { "SO2RH" }, new string[] { "SO2RE" }, new string[] { "SO2RH" }, new string[] { "SO2RE" }, new string[] { "SO2RH" }, new string[] { "SO2RE" }, new string[] { "SO2RH" }, new string[] { "SO2RE" }, new string[] { "SO2RH" }, new string[] { "SO2RE" }, new string[] { "SO2RH" }, new string[] { "SO2RE" }, new string[] { "SO2RH" }, new string[] { "SO2RE" }, new string[] { "SO2RH" }, new string[] { "SO2RE" }, new string[] { "SO2RH" } };
            string[][] dhvEquationList = { new string[] { }, new string[] { }, new string[] { }, new string[] { }, new string[] { "NORM" }, new string[] { "NORM" }, new string[] { "NORM", "NORM" }, new string[] { "NORM", "NORM" }, new string[] { "NORM" }, new string[] { "NORM" }, new string[] { "NORM" }, new string[] { "NORM" }, new string[] { }, new string[] { "NORM" }, new string[] { "NORM" }, new string[] { }, new string[] { }, new string[] { "NORM" }, new string[] { "NORM" }, new string[] { "MS-1" }, new string[] { "MS-1" }, new string[] { "MS-1" }, new string[] { "MS-1" }, new string[] { "NORM" }, new string[] { "NORM" }, new string[] { null }, new string[] { null }, new string[] { "MS-1" }, new string[] { "MS-1" }, new string[] { null }, new string[] { null }, new string[] { "MS-1" }, new string[] { "MS-1" }, new string[] { null }, new string[] { null }, new string[] { "MS-1" }, new string[] { "MS-1" }, new string[] { null }, new string[] { null } };
            string[][] dhvModcList = { new string[] { }, new string[] { }, new string[] { }, new string[] { }, new string[] { "36" }, new string[] { "36" }, new string[] { "36", "36" }, new string[] { "36", "36" }, new string[] { "36" }, new string[] { "36" }, new string[] { "36" }, new string[] { "36" }, new string[] { }, new string[] { "36" }, new string[] { "36" }, new string[] { }, new string[] { }, new string[] { "36" }, new string[] { "36" }, new string[] { "36" }, new string[] { "36" }, new string[] { "36" }, new string[] { "36" }, new string[] { "36" }, new string[] { "36" }, new string[] { "36" }, new string[] { "36" }, new string[] { "37" }, new string[] { "37" }, new string[] { "37" }, new string[] { "37" }, new string[] { "38" }, new string[] { "38" }, new string[] { "38" }, new string[] { "38" }, new string[] { "39" }, new string[] { "39" }, new string[] { "39" }, new string[] { "39" } };
            string[] unadjHrlyValueList = {"0.01E2", "0.10E2", "0.20E2", "0.30E2", "0.40E2", "0.50E2", "0.60E2", "0.70E2", "0.80E2", "0.90E2",
                                           "1.01E2", "1.10E2", "1.20E2", "1.30E2", "1.40E2", "1.50E2", "1.60E2", "1.70E2", "1.80E2", "1.90E2",
                                           "2.01E2", "2.30E2", "2.40E2", "2.50E2", "2.60E2", "2.70E2", "2.80E2", "2.90E2", "3.01E2", "3.10E2",
                                           "3.20E2", "3.30E2", "3.40E2", "3.50E2", "3.60E2", "3.70E2", "3.80E2", "3.90E2", "4.01E2", };
            string[] appSo2RateArrayList = { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null };

            /* Expected Values */
            string[] expectedResultList = { null, null, "E", "E", "E", "E", "B", "B", "C", "C", null, null, null, "A", "A", null, null, "D", "D", "G", "G", null, null, "F", "F", "F", "F", null, null, "F", "F", null, null, "H", "H", null, null, "F", "F" };
            bool[] expectedReNeededList = { false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false };
            bool[] expectedRhNeededList = { false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false };
            bool[] expectedConcNeededList = { false, false, false, false, false, false, false, false, false, false, true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false };
            string[] expectedParameterList = { null, null, null, null, null, null, null, null, "SO2RH", "SO2RE", "SO2RE", "SO2RH", null, null, null, null, null, null, null, "SO2RE", "SO2RH", "SO2RE", "SO2RH", "SO2RE", "SO2RH", "SO2RE", "SO2RH", "SO2RE", "SO2RH", "SO2RE", "SO2RH", "SO2RE", "SO2RH", "SO2RE", "SO2RH", "SO2RE", "SO2RH", "SO2RE", "SO2RH" };
            string[] expectedIdList = { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, "21", "22", null, null, null, null, "27", "28", null, null, null, null, null, null, "35", "36", null, null };
            string[] expectedUnadjHrlyList = { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, "2.30E2", "2.40E2", null, null, null, null, "2.90E2", "3.01E2", null, null, null, null, null, null, "3.70E2", "3.80E2", null, null };
            string expectedParameterDescription = "MATS SO2 Rate";

            string[] expectedAppSo2RateArray = {null, null, null, null, null, null, null, null, "0.80E2", "0.90E2",
                                              "1.01E2", "1.10E2", null, null, null, null, null, null, null, "1.90E2",
                                              "2.01E2", "2.30E2", "2.40E2", "2.50E2", "2.60E2", "2.70E2", "2.80E2", "2.90E2", "3.01E2", "3.10E2",
                                              "3.20E2", "3.30E2", "3.40E2", "3.50E2", "3.60E2", "3.70E2", "3.80E2", "3.90E2", "4.01E2", };
            string[] expectedMatsMs1ModcArray = {null, null, null, null, null, null, null, null, "36", "36",
                                              "36", "36", null, null, null, null, null, null, null, "36",
                                              "36", "36", "36", "36", "36", "36", "36", "36", "36", "37",
                                              "37", "37", "37", "38", "38", "38", "38", "39", "39", "39",
                                              "39", };

            /* Check array lengths */
            Assert.AreEqual(caseCount, dhvIdList.Length, "dhvIdList length");
            Assert.AreEqual(caseCount, checksNeededList.Length, "checksNeededList length");
            Assert.AreEqual(caseCount, opTimeNeededList.Length, "opTimeNeededList length");
            Assert.AreEqual(caseCount, methodParameterList.Length, "methodParameterList length");
            Assert.AreEqual(caseCount, methodCodeList.Length, "methodCodeList length");
            Assert.AreEqual(caseCount, dhvParameterList.Length, "dhvParameterList length");
            Assert.AreEqual(caseCount, dhvEquationList.Length, "dhvEquationList length");
            Assert.AreEqual(caseCount, dhvModcList.Length, "dhvModcList length");
            Assert.AreEqual(caseCount, expectedResultList.Length, "expectedResultList length");
            Assert.AreEqual(caseCount, expectedReNeededList.Length, "expectedReNeededList length");
            Assert.AreEqual(caseCount, expectedRhNeededList.Length, "expectedRhNeededList length");
            Assert.AreEqual(caseCount, expectedConcNeededList.Length, "expectedConcNeededList length");
            Assert.AreEqual(caseCount, expectedParameterList.Length, "expectedParameterList length");
            Assert.AreEqual(caseCount, expectedIdList.Length, "expectedIdList length");

            for (int caseDex = 0; caseDex < caseCount; caseDex++)
            {
                /* Generate MATS DHV REcord By Hour Location rows */
                MATSDerivedHourlyValueData[] MatsDhvRecordsByHourLocationRows;
                {
                    string[] parameterList = dhvParameterList[caseDex];
                    string[] equationList = dhvEquationList[caseDex];
                    string[] modcList = dhvModcList[caseDex];

                    MatsDhvRecordsByHourLocationRows = new MATSDerivedHourlyValueData[parameterList.Length];

                    for (int rowDex = 0; rowDex < parameterList.Length; rowDex++)
                        MatsDhvRecordsByHourLocationRows[rowDex] = new MATSDerivedHourlyValueData(matsDhvId: dhvIdList[caseDex], parameterCd: parameterList[rowDex]
                                                                                                , equationCd: equationList[rowDex], modcCd: modcList[rowDex]
                                                                                                , unadjustedHrlyValue: unadjHrlyValueList[caseDex]);
                }

                /* Initialize Input Parameters */
                EmParameters.CurrentHourlyOpRecord = new VwMpHrlyOpDataRow(opTime: opTimeNeededList[caseDex]);
                EmParameters.DerivedHourlyChecksNeeded = checksNeededList[caseDex];
                EmParameters.MatsDhvRecordsByHourLocation = new CheckDataView<MATSDerivedHourlyValueData>(MatsDhvRecordsByHourLocationRows);
                EmParameters.MatsSo2MethodCode = methodCodeList[caseDex];
                EmParameters.MatsSo2ParameterCode = methodParameterList[caseDex];
                EmParameters.CurrentMonitorPlanLocationPostion = caseDex;

                /* Initialize Optional Parameters */
                EmParameters.MatsMs1So2DhvId = null;
                EmParameters.MatsMs1So2UnadjustedHourlyValue = null;
                category.Process.ProcessParameters.RegisterParameter(4649, "Apportionment_SO2_Rate_Array");
                category.SetCheckParameter("Apportionment_SO2_Rate_Array", appSo2RateArrayList, eParameterDataType.String, false, true);
                category.Process.ProcessParameters.RegisterParameter(4676, "MATS_MS1_SO2_MODC_Code_Array");
                category.SetCheckParameter("MATS_MS1_SO2_MODC_Code_Array", expectedMatsMs1ModcArray, eParameterDataType.String, false, true);

                /* Initialize Output Parameters */
                EmParameters.MatsSo2reDhvChecksNeeded = null;
                EmParameters.MatsSo2rhDhvChecksNeeded = null;
                EmParameters.MatsSo2cNeeded = null;
                EmParameters.MatsSo2DhvRecord = new MATSDerivedHourlyValueData(); // Should always exit check as null or with parameter of SO2RE or SO2RH.
                EmParameters.MatsSo2DhvParameterDescription = null;

                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Initialize variables needed to run the check. */
                bool log = false;
                string actual;

                /* Run Check */
                actual = cMATSOperatingHourChecks.MATSHOD9(category, ref log);

                /* Check results */
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);

                Assert.AreEqual(expectedResultList[caseDex], category.CheckCatalogResult, string.Format("Result {0}", caseDex));
                Assert.AreEqual(expectedParameterList[caseDex], (EmParameters.MatsSo2DhvRecord != null ? EmParameters.MatsSo2DhvRecord.ParameterCd : null), string.Format("MatsSo2DhvRecord {0}", caseDex));
                Assert.AreEqual(expectedParameterDescription, EmParameters.MatsSo2DhvParameterDescription, string.Format("MatsSo2DhvParameterDescription {0}", caseDex));
                Assert.AreEqual(expectedConcNeededList[caseDex], EmParameters.MatsSo2cNeeded, string.Format("MatsSo2cNeeded {0}", caseDex));
                Assert.AreEqual(expectedReNeededList[caseDex], EmParameters.MatsSo2reDhvChecksNeeded, string.Format("MatsSo2reDhvChecksNeeded {0}", caseDex));
                Assert.AreEqual(expectedRhNeededList[caseDex], EmParameters.MatsSo2rhDhvChecksNeeded, string.Format("MatsSo2rhDhvChecksNeeded {0}", caseDex));
                Assert.AreEqual(expectedIdList[caseDex], EmParameters.MatsMs1So2DhvId, string.Format("MatsMs1So2DhvId {0}", caseDex));
                Assert.AreEqual(expectedUnadjHrlyList[caseDex], EmParameters.MatsMs1So2UnadjustedHourlyValue, string.Format("MatsMs1So2UnadjustedHourlyValue {0}", caseDex));
            }

            /* Check the array results */
            string[] apportionmentSo2RateArray = (string[])category.GetCheckParameter("Apportionment_So2_Rate_Array").ParameterValue;
            string[] matsMS1So2ModcCodeArray = (string[])category.GetCheckParameter("MATS_MS1_SO2_MODC_Code_Array").ParameterValue;
            CollectionAssert.AreEqual(expectedAppSo2RateArray, apportionmentSo2RateArray, string.Format("ApportionmentSo2RateArray does not equal the expected result."));
            CollectionAssert.AreEqual(expectedMatsMs1ModcArray, matsMS1So2ModcCodeArray, string.Format("MatsMS1So2ModcCodeArray does not equal the expected result."));
        }

        /// <summary>
        /// MatsHod-10
        /// 
        /// | ## | DhvNeeded | OpTime | HgcNeeded | Mhv1SysT | Mhv2SysT | FlowNeeded || Result | MhvNeeded | MhvRecSet | FlowNeeded || Note
        /// |  0 | null      |    0.0 | false     | HG       | null     | false      || null   | false     | false     | false      || DerivedHourlyChecksNeeded is null.
        /// |  1 | false     |    0.0 | false     | HG       | null     | false      || null   | false     | false     | false      || DerivedHourlyChecksNeeded is false.
        /// |  2 | true      |    0.0 | false     | HG       | null     | false      || D      | false     | false     | false      || Non-op hour but MHV exist.
        /// |  3 | true      |    1.0 | null      | null     | null     | false      || B      | false     | false     | false      || MATS HgC needed is null (defaults to true) and MHV does not exist.
        /// |  4 | true      |    1.0 | false     | null     | null     | false      || null   | false     | false     | false      || MATS HgC not required and MHV does not exist.
        /// |  5 | true      |    1.0 | false     | HG       | null     | false      || A      | false     | false     | false      || MATS HgC not required, but MHV exist.
        /// |  6 | true      |    1.0 | false     | ST       | null     | false      || A      | false     | false     | false      || MATS HgC not required, but MHV exist.
        /// |  7 | true      |    1.0 | true      | HG       | null     | null       || null   | true      | true      | null       || Only one MHV with HG system, Flow Needed is initially null.
        /// |  8 | true      |    1.0 | true      | ST       | null     | null       || null   | true      | true      | true       || Only one MHV with ST system, Flow Needed is initially null.
        /// |  9 | true      |    1.0 | true      | HG       | null     | false      || null   | true      | true      | false      || Only one MHV with HG system, Flow Needed is initially false.
        /// | 10 | true      |    1.0 | true      | ST       | null     | false      || null   | true      | true      | true       || Only one MHV with ST system, Flow Needed is initially false.
        /// | 11 | true      |    1.0 | true      | HG       | null     | true       || null   | true      | true      | true       || Only one MHV with HG system, Flow Needed is initially true.
        /// | 12 | true      |    1.0 | true      | ST       | null     | true       || null   | true      | true      | true       || Only one MHV with ST system, Flow Needed is initially true.
        /// | 13 | true      |    1.0 | true      | null     | null     | false      || B      | false     | false     | false      || No MHV exist.
        /// | 14 | true      |    1.0 | true      | HG       | ST       | false      || C      | false     | false     | false      || Two MHV exist.
        /// 
        ///</summary>()
        [TestMethod()]
        public void MatsHod10()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;


            /* Input Parameter Values */
            bool?[] dhvNeededList = { null, false, true, true, true, true, true, true, true, true, true, true, true, true, true };
            decimal?[] opTimeList = { 0.0m, 0.0m, 0.0m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m, 0.1m };
            bool?[] hgcNeededList = { false, false, false, null, false, false, false, true, true, true, true, true, true, true, true };
            string[] mhv1SysTypeList = { "HG", "HG", "HG", null, null, "HG", "ST", "HG", "ST", "HG", "ST", "HG", "ST", null, "ST" };
            string[] mhv2SysTypeList = { null, null, null, null, null, null, null, null, null, null, null, null, null, null, "ST" };

            /* Input-Output Parameter Values */
            bool?[] flowNeededList = { false, false, false, false, false, false, false, null, null, false, false, true, true, false, false };

            /* Expected Values */
            string[] expResultList = { null, null, "D", "B", null, "A", "A", null, null, null, null, null, null, "B", "C" };
            bool[] expMhvNeededList = { false, false, false, false, false, false, false, true, true, true, true, true, true, false, false };
            bool[] expMhvRecSetList = { false, false, false, false, false, false, false, true, true, true, true, true, true, false, false };
            bool?[] expFlowNeededList = { false, false, false, false, false, false, false, null, true, false, true, true, true, false, false };

            /* Case Count */
            int caseCount = 15;

            /* Check array lengths */
            Assert.AreEqual(caseCount, dhvNeededList.Length, "dhvNeededList length");
            Assert.AreEqual(caseCount, opTimeList.Length, "opTimeList length");
            Assert.AreEqual(caseCount, hgcNeededList.Length, "hgcNeededList length");
            Assert.AreEqual(caseCount, mhv1SysTypeList.Length, "mhv1SysTypeList length");
            Assert.AreEqual(caseCount, mhv2SysTypeList.Length, "mhv2SysTypeList length");
            Assert.AreEqual(caseCount, flowNeededList.Length, "flowNeededList length");
            Assert.AreEqual(caseCount, expResultList.Length, "expResultList length");
            Assert.AreEqual(caseCount, expMhvNeededList.Length, "expMhvNeededList length");
            Assert.AreEqual(caseCount, expMhvRecSetList.Length, "expMhvRecSetList length");
            Assert.AreEqual(caseCount, expFlowNeededList.Length, "expFlowNeededList length");


            for (int caseDex = 0; caseDex < caseCount; caseDex++)
            {
                /* Initialize Input Parameters */
                EmParameters.CurrentHourlyOpRecord = new VwMpHrlyOpDataRow(opTime: opTimeList[caseDex]);
                EmParameters.DerivedHourlyChecksNeeded = dhvNeededList[caseDex];
                EmParameters.MatsHgcNeeded = hgcNeededList[caseDex];
                EmParameters.MatsMhvHgcRecordsByHourLocation = new CheckDataView<MATSMonitorHourlyValueData>();
                {
                    if (mhv1SysTypeList[caseDex] != null)
                        EmParameters.MatsMhvHgcRecordsByHourLocation.Add(new MATSMonitorHourlyValueData(matsMhvId: "ONE", sysTypeCd: mhv1SysTypeList[caseDex], parameterCd: "HGC"));
                    if (mhv2SysTypeList[caseDex] != null)
                        EmParameters.MatsMhvHgcRecordsByHourLocation.Add(new MATSMonitorHourlyValueData(matsMhvId: "TWO", sysTypeCd: mhv2SysTypeList[caseDex], parameterCd: "HGC"));
                    //TODO: Included setting parameterCd to "HGC", because the check performs a filter using it.  The table parameter on which the filter is performed should have already been filtered to "HGC".
                }

                /* Initialize Input-Output Parameters */
                EmParameters.FlowMonitorHourlyChecksNeeded = flowNeededList[caseDex];

                /* Initialize Output Parameters */
                EmParameters.MatsHgcMhvChecksNeeded = null;
                EmParameters.MatsHgcMhvRecord = null;


                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Initialize variables needed to run the check. */
                bool log = false;
                string actual;


                /* Run Check */
                actual = cMATSOperatingHourChecks.MATSHOD10(category, ref log);

                /* Check results */
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);

                Assert.AreEqual(expResultList[caseDex], category.CheckCatalogResult, string.Format("Result {0}", caseDex));
                Assert.AreEqual(expMhvNeededList[caseDex], EmParameters.MatsHgcMhvChecksNeeded, string.Format("MatsHgcMhvChecksNeeded {0}", caseDex));
                Assert.AreEqual(expMhvRecSetList[caseDex], ((EmParameters.MatsHgcMhvRecord != null ? EmParameters.MatsHgcMhvRecord.MatsMhvId : null) == "ONE"), string.Format("MatsHfcMhvRecord {0}", caseDex));
                Assert.AreEqual(expFlowNeededList[caseDex], EmParameters.FlowMonitorHourlyChecksNeeded, string.Format("FlowMonitorHourlyChecksNeeded {0}", caseDex));
            }

        }

        #endregion

        #region MATSHOD Checks 11-20
        #region MATSHOD-11
        /// <summary>
        ///A test for MATSHOD-11
        ///</summary>()
        [TestMethod()]
        public void MATSHOD11()
        {
            //static check setup
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            // Variables
            bool log = false;
            string actual;

            //Result A
            {
                // Init Input
                EmParameters.DerivedHourlyChecksNeeded = true;
                EmParameters.CurrentHourlyOpRecord = new VwMpHrlyOpDataRow(opTime: 1);
                EmParameters.MatsHclcNeeded = false;
                EmParameters.MatsMhvHclcRecordsByHourLocation = new CheckDataView<ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData>(
                                new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData(parameterCd: "HCLC"));

                // Init Output
                category.CheckCatalogResult = null;

                // Run Checks
                actual = cMATSOperatingHourChecks.MATSHOD11(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual("A", category.CheckCatalogResult, "Result");
                Assert.AreEqual(null, EmParameters.MatsHclcMhvRecord, "MATSHclcMHVRecord");
                Assert.AreEqual(false, EmParameters.MatsHclcMhvChecksNeeded, "MatsHclcMhvChecksNeeded");
            }

            //Result B
            {
                // Init Input
                EmParameters.DerivedHourlyChecksNeeded = true;
                EmParameters.CurrentHourlyOpRecord = new VwMpHrlyOpDataRow(opTime: 1);
                EmParameters.MatsHclcNeeded = true;
                EmParameters.MatsMhvHclcRecordsByHourLocation = new CheckDataView<ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData>();

                // Init Output
                category.CheckCatalogResult = null;

                // Run Checks
                actual = cMATSOperatingHourChecks.MATSHOD11(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual("B", category.CheckCatalogResult, "Result");
                Assert.AreEqual(null, EmParameters.MatsHclcMhvRecord, "MATSHclcMHVRecord");
                Assert.AreEqual(false, EmParameters.MatsHclcMhvChecksNeeded, "MatsHclcMhvChecksNeeded");
            }

            //Result C
            {
                // Init Input
                EmParameters.DerivedHourlyChecksNeeded = true;
                EmParameters.CurrentHourlyOpRecord = new VwMpHrlyOpDataRow(opTime: 1);
                EmParameters.MatsHclcNeeded = true;
                EmParameters.MatsMhvHclcRecordsByHourLocation = new CheckDataView<ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData>(
                                new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData(parameterCd: "HCLC"),
                                new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData(parameterCd: "HCLC"));

                // Init Output
                category.CheckCatalogResult = null;

                // Run Checks
                actual = cMATSOperatingHourChecks.MATSHOD11(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual("C", category.CheckCatalogResult, "Result");
                Assert.AreEqual(null, EmParameters.MatsHclcMhvRecord, "MATSHclcMHVRecord");
                Assert.AreEqual(false, EmParameters.MatsHclcMhvChecksNeeded, "MatsHclcMhvChecksNeeded");
            }

            //Pass
            {
                // Init Input
                EmParameters.DerivedHourlyChecksNeeded = true;
                EmParameters.CurrentHourlyOpRecord = new VwMpHrlyOpDataRow(opTime: 1);
                EmParameters.MatsHclcNeeded = true;
                EmParameters.MatsMhvHclcRecordsByHourLocation = new CheckDataView<ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData>(
                                new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData(parameterCd: "HCLC"));

                // Init Output
                category.CheckCatalogResult = null;

                // Run Checks
                actual = cMATSOperatingHourChecks.MATSHOD11(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual("HCLC", EmParameters.MatsHclcMhvRecord.ParameterCd, "MATSHclcMHVRecord");
                Assert.AreEqual(true, EmParameters.MatsHclcMhvChecksNeeded, "MatsHclcMhvChecksNeeded");
            }

            //Result D
            {
                // Init Input
                EmParameters.DerivedHourlyChecksNeeded = true;
                EmParameters.CurrentHourlyOpRecord = new VwMpHrlyOpDataRow(opTime: 0);
                EmParameters.MatsHclcNeeded = true;
                EmParameters.MatsMhvHclcRecordsByHourLocation = new CheckDataView<ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData>(
                                new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData(parameterCd: "HCLC"));

                // Init Output
                category.CheckCatalogResult = null;

                // Run Checks
                actual = cMATSOperatingHourChecks.MATSHOD11(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual("D", category.CheckCatalogResult, "Result");
                Assert.AreEqual(null, EmParameters.MatsHclcMhvRecord, "MATSHclcMHVRecord");
                Assert.AreEqual(false, EmParameters.MatsHclcMhvChecksNeeded, "MatsHclcMhvChecksNeeded");
            }
        }
        #endregion

        #region MATSHOD-12
        /// <summary>
        ///A test for MATSHOD-12
        ///</summary>()
        [TestMethod()]
        public void MATSHOD12()
        {
            //static check setup
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            // Variables
            bool log = false;
            string actual;

            //Result A
            {
                // Init Input
                EmParameters.DerivedHourlyChecksNeeded = true;
                EmParameters.CurrentHourlyOpRecord = new VwMpHrlyOpDataRow(opTime: 1);
                EmParameters.MatsHfcNeeded = false;
                EmParameters.MatsMhvHfcRecordsByHourLocation = new CheckDataView<ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData>(
                                new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData(parameterCd: "HFC"));

                // Init Output
                category.CheckCatalogResult = null;

                // Run Checks
                actual = cMATSOperatingHourChecks.MATSHOD12(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual("A", category.CheckCatalogResult, "Result");
                Assert.AreEqual(null, EmParameters.MatsHfcMhvRecord, "MATSHfcMHVRecord");
                Assert.AreEqual(false, EmParameters.MatsHfcMhvChecksNeeded, "MatsHfcMhvChecksNeeded");
            }

            //Result B
            {
                // Init Input
                EmParameters.DerivedHourlyChecksNeeded = true;
                EmParameters.CurrentHourlyOpRecord = new VwMpHrlyOpDataRow(opTime: 1);
                EmParameters.MatsHfcNeeded = true;
                EmParameters.MatsMhvHfcRecordsByHourLocation = new CheckDataView<ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData>();

                // Init Output
                category.CheckCatalogResult = null;

                // Run Checks
                actual = cMATSOperatingHourChecks.MATSHOD12(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual("B", category.CheckCatalogResult, "Result");
                Assert.AreEqual(null, EmParameters.MatsHfcMhvRecord, "MATSHfcMHVRecord");
                Assert.AreEqual(false, EmParameters.MatsHfcMhvChecksNeeded, "MatsHfcMhvChecksNeeded");
            }

            //Result C
            {
                // Init Input
                EmParameters.DerivedHourlyChecksNeeded = true;
                EmParameters.CurrentHourlyOpRecord = new VwMpHrlyOpDataRow(opTime: 1);
                EmParameters.MatsHfcNeeded = true;
                EmParameters.MatsMhvHfcRecordsByHourLocation = new CheckDataView<ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData>(
                                new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData(parameterCd: "HFC"),
                                new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData(parameterCd: "HFC"));

                // Init Output
                category.CheckCatalogResult = null;

                // Run Checks
                actual = cMATSOperatingHourChecks.MATSHOD12(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual("C", category.CheckCatalogResult, "Result");
                Assert.AreEqual(null, EmParameters.MatsHfcMhvRecord, "MATSHfcMHVRecord");
                Assert.AreEqual(false, EmParameters.MatsHfcMhvChecksNeeded, "MatsHfcMhvChecksNeeded");
            }

            //Pass
            {
                // Init Input
                EmParameters.DerivedHourlyChecksNeeded = true;
                EmParameters.CurrentHourlyOpRecord = new VwMpHrlyOpDataRow(opTime: 1);
                EmParameters.MatsHfcNeeded = true;
                EmParameters.MatsMhvHfcRecordsByHourLocation = new CheckDataView<ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData>(
                                new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData(parameterCd: "HFC"));

                // Init Output
                category.CheckCatalogResult = null;

                // Run Checks
                actual = cMATSOperatingHourChecks.MATSHOD12(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(null, category.CheckCatalogResult, "Result");
                Assert.AreEqual("HFC", EmParameters.MatsHfcMhvRecord.ParameterCd, "MATSHfcMHVRecord");
                Assert.AreEqual(true, EmParameters.MatsHfcMhvChecksNeeded, "MatsHfcMhvChecksNeeded");
            }

            //Result D
            {
                // Init Input
                EmParameters.DerivedHourlyChecksNeeded = true;
                EmParameters.CurrentHourlyOpRecord = new VwMpHrlyOpDataRow(opTime: 0);
                EmParameters.MatsHfcNeeded = true;
                EmParameters.MatsMhvHfcRecordsByHourLocation = new CheckDataView<ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData>(
                                new ECMPS.Checks.Data.Ecmps.CheckEm.Function.MATSMonitorHourlyValueData(parameterCd: "HFC"));

                // Init Output
                category.CheckCatalogResult = null;

                // Run Checks
                actual = cMATSOperatingHourChecks.MATSHOD12(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual("D", category.CheckCatalogResult, "Result");
                Assert.AreEqual(null, EmParameters.MatsHfcMhvRecord, "MATSHfcMHVRecord");
                Assert.AreEqual(false, EmParameters.MatsHfcMhvChecksNeeded, "MatsHfcMhvChecksNeeded");
            }
        }
        #endregion


        /// <summary>
        /// MatsHod-13
        /// 
        /// 
        /// Cases:
        /// 
        /// |    |      - Current Hourly Op Record -         |  - Method Recs -  ||                       ||
        /// | ## | LocId    | OpTime | Mats | Load | UOM     | LocId    | Param  || Result | LoadArray    || Note
        /// |  0 | null     |   null | null | null | null    | TargetId | HGRE   || null   | [0, null, 0] || No result because CurrentHourlyOpRecord is null.
        /// |  1 | TargetId |   0.00 |  100 |  100 | MW      | TargetId | HGRE   || C      | [0, null, 0] || Result returned because operating time is 0, but MATS load is not null.
        /// |  2 | TargetId |   0.00 | null |  100 | MW      | TargetId | HGRE   || null   | [0, null, 0] || No result because operating time is 0 and MATS load is null.
        /// |  3 | TargetId |   0.01 | null |  100 | MW      | TargetId | HGRE   || A      | [0, null, 0] || Result returned because operating time greater than 0, RE method exists, but MATS load is null.
        /// |  4 | TargetId |   0.01 | null |  100 | MW      | TargetId | HCLRE  || A      | [0, null, 0] || Result returned because operating time greater than 0, RE method exists, but MATS load is null.
        /// |  5 | TargetId |   0.01 | null |  100 | MW      | TargetId | HFRE   || A      | [0, null, 0] || Result returned because operating time greater than 0, RE method exists, but MATS load is null.
        /// |  6 | TargetId |   0.01 | null |  100 | MW      | TargetId | SO2RE  || A      | [0, null, 0] || Result returned because operating time greater than 0, RE method exists, but MATS load is null.
        /// |  7 | TargetId |   0.01 | null |  100 | MW      | TargetId | HGRH   || null   | [0, null, 0] || No result because operating time greater than 0, RE method does not exist, but MATS load is null.
        /// |  8 | TargetId |   0.01 | null |  100 | MW      | TargetId | HCLRH  || null   | [0, null, 0] || No result because operating time greater than 0, RE method does not exist, but MATS load is null.
        /// |  9 | TargetId |   0.01 | null |  100 | MW      | TargetId | HFRH   || null   | [0, null, 0] || No result because operating time greater than 0, RE method does not exist, but MATS load is null.
        /// | 10 | TargetId |   0.01 | null |  100 | MW      | TargetId | SO2RH  || null   | [0, null, 0] || No result because operating time greater than 0, RE method does not exist, but MATS load is null.
        /// | 11 | TargetId |   0.01 |  100 |  100 | MW      | TargetId | HGRE   || null   | [0, 100, 0]  || No result because operating time greater than 0, RE method exists, and MATS load equals Hour load.
        /// | 12 | TargetId |   0.01 |  100 |  100 | MW      | TargetId | HCLRE  || null   | [0, 100, 0]  || No result because operating time greater than 0, RE method exists, and MATS load equals Hour load.
        /// | 13 | TargetId |   0.01 |  100 |  100 | MW      | TargetId | HFRE   || null   | [0, 100, 0]  || No result because operating time greater than 0, RE method exists, and MATS load equals Hour load.
        /// | 14 | TargetId |   0.01 |  100 |  100 | MW      | TargetId | SO2RE  || null   | [0, 100, 0]  || No result because operating time greater than 0, RE method exists, and MATS load equals Hour load.
        /// | 15 | TargetId |   0.01 |  100 |  100 | MW      | TargetId | HGRH   || B      | [0, null, 0] || No result because operating time greater than 0, RE method does not exist, and MATS load equals Hour load.
        /// | 16 | TargetId |   0.01 |  100 |  100 | MW      | TargetId | HCLRH  || B      | [0, null, 0] || No result because operating time greater than 0, RE method does not exist, and MATS load equals Hour load.
        /// | 17 | TargetId |   0.01 |  100 |  100 | MW      | TargetId | HFRH   || B      | [0, null, 0] || No result because operating time greater than 0, RE method does not exist, and MATS load equals Hour load.
        /// | 18 | TargetId |   0.01 |  100 |  100 | MW      | TargetId | SO2RH  || B      | [0, null, 0] || No result because operating time greater than 0, RE method does not exist, and MATS load equals Hour load.
        /// | 19 | TargetId |   0.01 |   99 |  100 | MW      | TargetId | HGRE   || D      | [0, 99, 0]   || Result returned because operating time greater than 0, HGRE method exists, and MATS load less than Hour load.
        /// | 20 | TargetId |   0.01 |  101 |  100 | MW      | TargetId | HGRE   || null   | [0, 101, 0]  || No result because operating time greater than 0, RE method exists, and MATS load is greater than Hour load.
        /// | 21 | TargetId |   0.01 |   99 |  100 | MMBTUHR | TargetId | HGRE   || null   | [0, 99, 0]   || No result because operating time greater than 0, RE method exists, and MATS load less than Hour load, but load UOM does not equal MW.
        /// | 22 | TargetId |   0.01 |   99 |  100 | KLBHR   | TargetId | HGRE   || null   | [0, 99, 0]   || No result because operating time greater than 0, RE method exists, and MATS load less than Hour load, but load UOM does not equal MW.
        /// | 23 | TargetId |   0.01 | null |  100 | MW      | Other1Id | HGRE   || A      | [0, null, 0] || Result returned because operating time greater than 0, RE method exists in MP, but MATS load is null.
        /// | 24 | TargetId |   0.01 |  100 |  100 | MW      | Other1Id | HGRE   || null   | [0, 100, 0]  || No result because operating time greater than 0, RE method exists in MP, and MATS load equals Hour load.
        /// | 25 | TargetId |   0.01 | null |  100 | MW      | Other1Id | HGRE   || A      | [0, null, 0] || Result returned because operating time greater than 0, at least one RE method exists in MP, but MATS load is null.
        /// |    |          |        |      |      |         | Other2Id | HCLRH  ||        |              || 
        /// |    |          |        |      |      |         | Other3Id | HFRH   ||        |              || 
        /// |    |          |        |      |      |         | Other4Id | SO2RH  ||        |              || 
        /// | 26 | TargetId |   0.01 | null |  100 | MW      | Other1Id | HGRH   || A      | [0, null, 0] || Result returned because operating time greater than 0, at least one RE method exists in MP, but MATS load is null.
        /// |    |          |        |      |      |         | Other2Id | HCLRE  ||        |              || 
        /// |    |          |        |      |      |         | Other3Id | HFRH   ||        |              || 
        /// |    |          |        |      |      |         | Other4Id | SO2RH  ||        |              || 
        /// | 27 | TargetId |   0.01 | null |  100 | MW      | Other1Id | HGRH   || A      | [0, null, 0] || Result returned because operating time greater than 0, at least one RE method exists in MP, but MATS load is null.
        /// |    |          |        |      |      |         | Other2Id | HCLRH  ||        |              || 
        /// |    |          |        |      |      |         | Other3Id | HFRE   ||        |              || 
        /// |    |          |        |      |      |         | Other4Id | SO2RH  ||        |              || 
        /// | 28 | TargetId |   0.01 | null |  100 | MW      | Other1Id | HGRH   || A      | [0, null, 0] || Result returned because operating time greater than 0, at least one RE method exists in MP, but MATS load is null.
        /// |    |          |        |      |      |         | Other2Id | HCLRH  ||        |              || 
        /// |    |          |        |      |      |         | Other3Id | HFRH   ||        |              || 
        /// |    |          |        |      |      |         | Other4Id | SO2RE  ||        |              || 
        /// | 29 | TargetId |   0.01 |  100 |  100 | MW      | Other1Id | HGRH   || B      | [0, null, 0] || Result returned because operating time greater than 0, no RE method exist in MP, but MATS load is not null.
        /// |    |          |        |      |      |         | Other2Id | HCLRH  ||        |              || 
        /// |    |          |        |      |      |         | Other3Id | HFRH   ||        |              || 
        /// |    |          |        |      |      |         | Other4Id | SO2RH  ||        |              || 
        /// </summary>
        [TestMethod()]
        public void MatsHod13()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            category.Process.ProcessParameters.RegisterParameter(3618, "Apportionment_MATS_Load_Array"); // Currently cannot access arrays using the new check parameter access.


            /* Helper Variables */
            string[] aTargetId = new string[] { "TargetId" };
            string[] aOther1Id = new string[] { "Other1Id" };
            string[] aOtherAId = new string[] { "Other1Id", "Other2Id", "Other3Id", "Other4Id" };

            string[] aHgre = new string[] { "HGRE" };
            string[] aHclre = new string[] { "HCLRE" };
            string[] aHfre = new string[] { "HFRE" };
            string[] aSo2re = new string[] { "SO2RE" };
            string[] aHgrh = new string[] { "HGRH" };
            string[] aHclrh = new string[] { "HCLRH" };
            string[] aHfrh = new string[] { "HFRH" };
            string[] aSo2rh = new string[] { "SO2RH" };

            /* Input Parameter Values */
            Decimal?[] hodOpTimeList = { null, 0.00m, 0.00m, 0.01m, 0.01m, 0.01m, 0.01m, 0.01m, 0.01m, 0.01m, 0.01m, 0.01m, 0.01m, 0.01m, 0.01m, 0.01m, 0.01m, 0.01m, 0.01m, 0.01m, 0.01m, 0.01m, 0.01m, 0.01m, 0.01m, 0.01m, 0.01m, 0.01m, 0.01m, 0.01m };
            int?[] hodMatsLoadList = { null, 100, null, null, null, null, null, null, null, null, null, 100, 100, 100, 100, 100, 100, 100, 100, 99, 101, 99, 99, null, 100, null, null, null, null, 100 };
            string[] hodLoadUomList = { null, "MW", "MW", "MW", "MW", "MW", "MW", "MW", "MW", "MW", "MW", "MW", "MW", "MW", "MW", "MW", "MW", "MW", "MW", "MW", "MW", "MMBTUHR", "KLBHR", "MW", "MW", "MW", "MW", "MW", "MW", "MW" };
            int[] methodCountList = { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 4, 4, 4, 4, 4 };
            string[][] methodLocIdList = { aTargetId, aTargetId, aTargetId, aTargetId, aTargetId, aTargetId, aTargetId, aTargetId, aTargetId, aTargetId, aTargetId, aTargetId, aTargetId, aTargetId, aTargetId, aTargetId, aTargetId, aTargetId, aTargetId, aTargetId, aTargetId, aTargetId, aTargetId, aOther1Id, aOther1Id, aOtherAId, aOtherAId, aOtherAId, aOtherAId, aOtherAId };
            string[][] methodParamList = { aHgre, aHgre, aHgre, aHgre, aHclre, aHfre, aSo2re, aHgrh, aHclrh, aHfrh, aSo2rh, aHgre, aHclre, aHfre, aSo2re, aHgrh, aHclrh, aHfrh, aSo2rh, aHgre, aHgre, aHgre, aHgre, aHgre, aHgre, new string[] { "HGRE", "HCLRH", "HFRH", "SO2RH" }, new string[] { "HGRH", "HCLRE", "HFRH", "SO2RH" }, new string[] { "HGRH", "HCLRH", "HFRE", "SO2RH" }, new string[] { "HGRH", "HCLRH", "HFRH", "SO2RE" }, new string[] { "HGRH", "HCLRH", "HFRH", "SO2RH" } };

            decimal?[][] exploadArrayList = { new decimal?[] {0m, null, 0m }, new decimal?[] { 0m, null, 0m }, new decimal?[] { 0m, null, 0m }, new decimal?[] { 0m, null, 0m }, new decimal?[] { 0m, null, 0m },
                                              new decimal?[] {0m, null, 0m }, new decimal?[] { 0m, null, 0m }, new decimal?[] { 0m, null, 0m }, new decimal?[] { 0m, null, 0m }, new decimal?[] { 0m, null, 0m },
                                              new decimal?[] {0m, null, 0m }, new decimal?[] { 0m,  100, 0m }, new decimal?[] { 0m,  100, 0m }, new decimal?[] { 0m,  100, 0m }, new decimal?[] { 0m,  100, 0m },
                                              new decimal?[] {0m, null, 0m }, new decimal?[] { 0m, null, 0m }, new decimal?[] { 0m, null, 0m }, new decimal?[] { 0m, null, 0m }, new decimal?[] { 0m,   99, 0m },
                                              new decimal?[] {0m,  101, 0m }, new decimal?[] { 0m,   99, 0m }, new decimal?[] { 0m,   99, 0m }, new decimal?[] { 0m, null, 0m }, new decimal?[] { 0m,  100, 0m },
                                              new decimal?[] {0m, null, 0m }, new decimal?[] { 0m, null, 0m }, new decimal?[] { 0m, null, 0m }, new decimal?[] { 0m, null, 0m }, new decimal?[] { 0m, null, 0m } };

            int hodHourLoad = 100;
            string hodLocId = "TargetId";

            /* Expected Values */
            string[] resultList = { null, "C", null, "A", "A", "A", "A", null, null, null, null, null, null, null, null, "B", "B", "B", "B", "D", null, null, null, "A", null, "A", "A", "A", "A", "B" };

            /* Test Case Count */
            int caseCount = 30;

            /* Check array lengths */
            Assert.AreEqual(caseCount, hodOpTimeList.Length, "hodOpTimeList length");
            Assert.AreEqual(caseCount, hodMatsLoadList.Length, "hodMatsLoadList length");
            Assert.AreEqual(caseCount, hodLoadUomList.Length, "hodLoadUomList length");
            Assert.AreEqual(caseCount, methodCountList.Length, "methodCountList length");
            Assert.AreEqual(caseCount, methodLocIdList.Length, "methodLocIdList length");
            Assert.AreEqual(caseCount, methodParamList.Length, "methodCodeList length");
            Assert.AreEqual(caseCount, resultList.Length, "resultList length");
            Assert.AreEqual(caseCount, exploadArrayList.Length, "exploadArrayList length");

            /* Check child array lengths */
            for (int caseDex = 0; caseDex < caseCount; caseDex++)
            {
                Assert.AreEqual(methodCountList[caseDex], methodLocIdList[caseDex].Length, string.Format("methodLocIdList child {0} length", caseDex));
                Assert.AreEqual(methodCountList[caseDex], methodParamList[caseDex].Length, string.Format("methodCodeList child {0} length", caseDex));
            }

            /* Run Cases */
            for (int caseDex = 0; caseDex < caseCount; caseDex++)
            {
                /*  Initialize Input Parameters*/
                EmParameters.CurrentHourlyOpRecord = (hodOpTimeList[caseDex] != null) ? new VwMpHrlyOpDataRow(monLocId: hodLocId, opTime: hodOpTimeList[caseDex], matsHourLoad: hodMatsLoadList[caseDex], hrLoad: hodHourLoad, loadUomCd: hodLoadUomList[caseDex]) : null;
                EmParameters.CurrentMonitorPlanLocationPostion = 1;
                EmParameters.MonitorMethodRecordsByHour = new CheckDataView<VwMpMonitorMethodRow>();
                {
                    for (int rowDex = 0; rowDex < methodCountList[caseDex]; rowDex++)
                    {
                        EmParameters.MonitorMethodRecordsByHour.Add
                            (
                                new VwMpMonitorMethodRow(monLocId: methodLocIdList[caseDex][rowDex],
                                                         parameterCd: methodParamList[caseDex][rowDex])
                            );
                    }
                }

                /* Initialize Input-Output Parameters */
                category.SetCheckParameter("Apportionment_MATS_Load_Array", new int?[] { 0, 14, 0 });

                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Initialize variables needed to run the check. */
                bool log = false;
                string actual;

                /* Run Check */
                actual = cMATSOperatingHourChecks.MATSHOD13(category, ref log);

                /* Check results */
                Assert.AreEqual(string.Empty, actual, string.Format("actual {0}", caseDex));
                Assert.AreEqual(false, log, string.Format("log {0}", caseDex));
                Assert.AreEqual(resultList[caseDex], category.CheckCatalogResult, String.Format("Result [case {0}]", caseDex));

                int?[] apportionmentMatsLoadArray = (int?[])category.GetCheckParameter("Apportionment_MATS_Load_Array").ParameterValue;
                {
                    Assert.AreEqual(exploadArrayList[caseDex].Length, apportionmentMatsLoadArray.Length, string.Format("ApportionmentMatsLoadArray.Length [case {0}]", caseDex));
                    Assert.AreEqual(exploadArrayList[caseDex][0], apportionmentMatsLoadArray[0], string.Format("ApportionmentMatsLoadArray[0] [case {0}]", caseDex));
                    Assert.AreEqual(exploadArrayList[caseDex][1], apportionmentMatsLoadArray[1], string.Format("ApportionmentMatsLoadArray[1] [case {0}]", caseDex));
                    Assert.AreEqual(exploadArrayList[caseDex][2], apportionmentMatsLoadArray[2], string.Format("ApportionmentMatsLoadArray[2] [case {0}]", caseDex));
                }
            }
        }

        /// <summary>
        ///A test for MATSHOD-15
        ///</summary>()
        [TestMethod()]
        public void MatsHod15()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            /* Initialize variables needed to run the check. */
            bool log = false;
            string actual;

            /* Initialize Output Parameters */
            EmParameters.MatsHclDhvParameterDescription = null;
            EmParameters.MatsHclMhvParameterDescription = null;
            EmParameters.MatsHfDhvParameterDescription = null;
            EmParameters.MatsHfMhvParameterDescription = null;
            EmParameters.MatsHgDhvParameterDescription = null;
            EmParameters.MatsHgMhvParameterDescription = null;
            EmParameters.MatsSo2DhvParameterDescription = null;

            /* Init Cateogry Result */
            category.CheckCatalogResult = null;

            // Run Checks
            actual = cMATSOperatingHourChecks.MATSHOD15(category, ref log);

            // Check Results
            Assert.AreEqual(string.Empty, actual);
            Assert.AreEqual(false, log);
            Assert.AreEqual(null, category.CheckCatalogResult, "Result");
            Assert.AreEqual("HCLRE or HCLRH", EmParameters.MatsHclDhvParameterDescription, "MatsHclDhvParameterDescription");
            Assert.AreEqual("HCLC", EmParameters.MatsHclMhvParameterDescription, "MatsHclMhvParameterDescription");
            Assert.AreEqual("HFRE or HFRH", EmParameters.MatsHfDhvParameterDescription, "MatsHfDhvParameterDescription");
            Assert.AreEqual("HFC", EmParameters.MatsHfMhvParameterDescription, "MatsHfMhvParameterDescription");
            Assert.AreEqual("HGRE or HGRH", EmParameters.MatsHgDhvParameterDescription, "MatsHgDhvParameterDescription");
            Assert.AreEqual("HGC", EmParameters.MatsHgMhvParameterDescription, "MatsHgMhvParameterDescription");
            Assert.AreEqual("SO2RE or SO2RH", EmParameters.MatsSo2DhvParameterDescription, "MatsSo2DhvParameterDescription");
        }

        #region MatsHod-16

        /// <summary>
        /// 
        /// Notes:
        /// 
        /// Hours :
        /// 
        /// * B = 2016-06-17-21
        /// * C = 2016-06-17-22
        /// * E = 2016-06-17-23
        /// 
        /// Values not in the table:
        /// 
        /// For Train 2 : LocId = GOOD, CmpId = GOOD2, BegHr = B, EndHr = E
        /// For GFM 2   : LocId = GOOD, OprHr = C
        /// 
        ///                        |             Sampling Train 1              | Train 2  |         GFM 1         | GFM 2 | GFM 3 | GFM 4 ||                                     ||
        /// | ## | Needed | OpTime | LocId | CmpId | StatusCd  | BegHr | EndHr | StatusCd | LocId | CmpId | OprHr | CmpId | CmpId | CmpId || Result | MissingList | MultipleList || Note
        /// |  0 | null   |   0.00 | GOOD  | GOOD1 | PASSED    | B     | E     | PASSED   | GOOD  | GOOD1 | C     | GOOD2 | null  | null  || null   | null        | null         || Null DerivedHourlyChecksNeeded prevents negative result for GFM when op time = 0.
        /// |  1 | false  |   0.00 | GOOD  | GOOD1 | PASSED    | B     | E     | PASSED   | GOOD  | GOOD1 | C     | GOOD2 | null  | null  || null   | null        | null         || False DerivedHourlyChecksNeeded prevents negative result for GFM when op time = 0.
        /// |  2 | true   |   0.00 | GOOD  | GOOD1 | PASSED    | B     | E     | PASSED   | GOOD  | GOOD1 | C     | GOOD2 | null  | null  || D      | null        | null         || GFM exists when op time = 0, which returns result D.
        /// |  3 | true   |   0.00 | GOOD  | GOOD1 | PASSED    | B     | E     | PASSED   | GOOD  | GOOD1 | C     | null  | null  | null  || D      | null        | null         || One GFM exists when op time = 0, which returns result D.
        /// |  4 | true   |   0.00 | GOOD  | GOOD1 | PASSED    | B     | E     | PASSED   | null  | null  | null  | null  | null  | null  || null   | null        | null         || No GFM exists, which is expected when the op time = 0.
        /// |  5 | true   |   0.00 | GOOD  | GOOD1 | PASSED    | B     | E     | null     | GOOD  | GOOD1 | C     | null  | null  | null  || D      | null        | null         || One GFM exists for the only train when op time = 0, which returns result D.
        /// |  6 | true   |   0.00 | GOOD  | GOOD1 | PASSED    | B     | E     | null     | BAD   | GOOD1 | C     | null  | null  | null  || null   | ONE         | null         || Op time = 0 and one GFM exists but it's component id does not match the only train, which avoids result D.
        /// |  7 | null   |   0.01 | GOOD  | GOOD1 | PASSED    | B     | E     | PASSED   | null  | null  | null  | null  | null  | null  || null   | null        | null         || Null DerivedHourlyChecksNeeded prevents negative result for no GFM when op time > 0.
        /// |  8 | false  |   0.01 | GOOD  | GOOD1 | PASSED    | B     | E     | PASSED   | null  | null  | null  | null  | null  | null  || null   | null        | null         || False DerivedHourlyChecksNeeded prevents negative result for no GFM when op time > 0.
        /// |  9 | true   |   0.01 | GOOD  | GOOD1 | PASSED    | B     | E     | PASSED   | null  | null  | null  | null  | null  | null  || B      | ONE and TWO | null         || GFM do not exists when op time > 0, which returns result B.
        /// | 10 | true   |   0.01 | GOOD  | GOOD1 | PASSED    | B     | E     | PASSED   | GOOD  | GOOD1 | C     | null  | null  | null  || B      | TWO         | null         || One GFM exists when op time > 0, which returns result B.
        /// | 11 | true   |   0.01 | GOOD  | GOOD1 | PASSED    | B     | E     | PASSED   | GOOD  | GOOD1 | C     | GOOD2 | null  | null  || null   | null        | null         || One GFM exists for each train, which is expected when the op time > 0.
        /// | 12 | true   |   0.01 | GOOD  | GOOD1 | PASSED    | B     | E     | PASSED   | GOOD  | GOOD1 | C     | GOOD1 | GOOD2 | GOOD2 || C      | null        | ONE and TWO  || Two GFM exist for each train when op time > 0, which returns result C.
        /// | 13 | true   |   0.01 | GOOD  | GOOD1 | PASSED    | B     | E     | PASSED   | GOOD  | GOOD1 | C     | GOOD1 | GOOD2 | null  || C      | null        | ONE          || Two GFM exists for one train and one exists for the other when op time > 0, which returns result C.
        /// | 14 | true   |   0.01 | GOOD  | GOOD1 | PASSED    | B     | E     | PASSED   | GOOD  | GOOD1 | C     | GOOD1 | null  | null  || A      | TWO         | ONE          || Two GFM exist for one train, no GFM exist for the other and op time > 0, which returns result A.
        /// | 15 | true   |   0.01 | GOOD  | GOOD1 | PASSED    | B     | E     | null     | null  | null  | null  | null  | null  | null  || B      | ONE         | null         || GFM do not exist for the train, which returns result B.
        /// | 16 | true   |   0.01 | BAD   | GOOD1 | PASSED    | B     | E     | null     | null  | null  | null  | null  | null  | null  || null   | null        | null         || The only trains is not for the current location, so result B is not returned.
        /// | 17 | true   |   0.01 | GOOD  | GOOD1 | PASSED    | C - 2 | C     | null     | null  | null  | null  | null  | null  | null  || B      | ONE         | null         || GFM do not exist for the train and the train ends on the current hour, which returns result B.
        /// | 18 | true   |   0.01 | GOOD  | GOOD1 | PASSED    | C - 2 | C - 1 | null     | null  | null  | null  | null  | null  | null  || null   | null        | null         || The only train ends before the current hour, so result B is not returned.
        /// | 19 | true   |   0.01 | GOOD  | GOOD1 | PASSED    | C     | C + 2 | null     | null  | null  | null  | null  | null  | null  || B      | ONE         | null         || GFM do not exist for the train and the train begins on the current hour, which returns result B.
        /// | 20 | true   |   0.01 | GOOD  | GOOD1 | PASSED    | C + 1 | C + 2 | null     | null  | null  | null  | null  | null  | null  || null   | null        | null         || The only train begins after the current hour, so result B is not returned.
        /// | 21 | true   |   0.01 | GOOD  | GOOD1 | PASSED    | B     | E     | null     | GOOD  | GOOD1 | C     | null  | null  | null  || null   | null        | null         || One GFM exists for the only train, which is expected.
        /// | 22 | true   |   0.01 | GOOD  | GOOD1 | PASSED    | B     | E     | null     | GOOD  | BAD1  | C     | null  | null  | null  || B      | ONE         | null         || One GFM exists but it's component id does not match the only train, which returns result B.
        /// | 23 | true   |   0.01 | GOOD  | GOOD1 | FAILED    | B     | E     | null     | null  | null  | null  | null  | null  | null  || null   | null        | null         || GFM do not exist for the failed train, which does not return result B.
        /// | 24 | true   |   0.01 | GOOD  | GOOD1 | UNCERTAIN | B     | E     | null     | null  | null  | null  | null  | null  | null  || B      | ONE         | null         || GFM do not exist for the uncertain train, which returns result B.
        /// | 25 | true   |   0.01 | GOOD  | GOOD1 | EXPIRED   | B     | E     | null     | null  | null  | null  | null  | null  | null  || null   | null        | null         || GFM do not exist for the expired train, which does not return result B.
        /// | 26 | true   |   0.01 | GOOD  | GOOD1 | INC       | B     | E     | null     | null  | null  | null  | null  | null  | null  || null   | null        | null         || GFM do not exist for the incomplete train, which does not return result B.
        /// | 27 | true   |   0.01 | GOOD  | GOOD1 | LOST      | B     | E     | null     | null  | null  | null  | null  | null  | null  || null   | null        | null         || GFM do not exist for the lost train, which does not return result B.
        /// | 28 | true   |   0.01 | GOOD  | GOOD1 | FAILED    | B     | E     | null     | GOOD  | GOOD1 | C     | null  | null  | null  || null   | null        | null         || GFM exists for the failed train, which is expected.
        /// | 29 | true   |   0.01 | GOOD  | GOOD1 | UNCERTAIN | B     | E     | null     | GOOD  | GOOD1 | C     | null  | null  | null  || null   | null        | null         || GFM exists for the uncertain train, which is expected.
        /// | 30 | true   |   0.01 | GOOD  | GOOD1 | EXPIRED   | B     | E     | null     | GOOD  | GOOD1 | C     | null  | null  | null  || null   | null        | null         || GFM exists for the expired train, which is expected.
        /// | 31 | true   |   0.01 | GOOD  | GOOD1 | INC       | B     | E     | null     | GOOD  | GOOD1 | C     | null  | null  | null  || null   | null        | null         || GFM exists for the incomplete train, which is expected.
        /// | 32 | true   |   0.01 | GOOD  | GOOD1 | LOST      | B     | E     | null     | GOOD  | GOOD1 | C     | null  | null  | null  || null   | null        | null         || GFM exists for the lost train, which is expected.
        /// </summary>
        [TestMethod()]
        public void MatsHod16()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            /* Input Parameter Values */
            DateTime hourB = new DateTime(2016, 6, 17, 21, 0, 0);
            DateTime hourC = new DateTime(2016, 6, 17, 22, 0, 0);
            DateTime hourE = new DateTime(2016, 6, 17, 23, 0, 0);

            string bad = "BAD";
            string bad1 = "BAD1";
            string good = "GOOD";
            string good1 = "GOOD1";
            string good2 = "GOOD2";

            bool?[] neededList = { null, false, true, true, true, true, true, null, false, true,
                                   true, true, true, true, true, true, true, true, true, true,
                                   true, true, true, true, true, true, true, true, true, true,
                                   true, true, true };

            decimal?[] opTimeList = { 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.00m, 0.01m, 0.01m, 0.01m,
                                      0.01m, 0.01m, 0.01m, 0.01m, 0.01m, 0.01m, 0.01m, 0.01m, 0.01m, 0.01m,
                                      0.01m, 0.01m, 0.01m, 0.01m, 0.01m, 0.01m, 0.01m, 0.01m, 0.01m, 0.01m,
                                      0.01m, 0.01m, 0.01m };

            string[] trn1LocList = { good, good, good, good, good, good, good, good, good, good,
                                     good, good, good, good, good, good, bad, good, good, good,
                                     good, good, good, good, good, good, good, good, good, good,
                                     good, good, good };

            string[] trn1CmpList = { good1, good1, good1, good1, good1, good1, good1, good1, good1, good1,
                                     good1, good1, good1, good1, good1, good1, good1, good1, good1, good1,
                                     good1, good1, good1, good1, good1, good1, good1, good1, good1, good1,
                                     good1, good1, good1 };

            string[] trn1StatusList = { "PASSED", "PASSED", "PASSED", "PASSED", "PASSED", "PASSED", "PASSED", "PASSED", "PASSED", "PASSED",
                                        "PASSED", "PASSED", "PASSED", "PASSED", "PASSED", "PASSED", "PASSED", "PASSED", "PASSED", "PASSED",
                                        "PASSED", "PASSED", "PASSED", "FAILED", "UNCERTAIN", "EXPIRED", "INC", "LOST", "FAILED", "UNCERTAIN",
                                        "EXPIRED", "INC", "LOST"};

            DateTime?[] trn1BegList = { hourB, hourB, hourB, hourB, hourB, hourB, hourB, hourB, hourB, hourB,
                                        hourB, hourB, hourB, hourB, hourB, hourB, hourB, hourC.AddHours(-2), hourC.AddHours(-2), hourC,
                                        hourC.AddHours(1), hourB, hourB, hourB, hourB, hourB, hourB, hourB, hourB, hourB,
                                        hourB, hourB, hourB };

            DateTime?[] trn1EndList = { hourE, hourE, hourE, hourE, hourE, hourE, hourE, hourE, hourE, hourE,
                                        hourE, hourE, hourE, hourE, hourE, hourE, hourE, hourC, hourC.AddHours(-1), hourC.AddHours(2),
                                        hourC.AddHours(2), hourE, hourE, hourE, hourE, hourE, hourE, hourE, hourE, hourE,
                                        hourE, hourE, hourE };

            string[] trn2StatusList = { "PASSED", "PASSED", "PASSED", "PASSED", "PASSED", null, null, "PASSED", "PASSED", "PASSED",
                                        "PASSED", "PASSED", "PASSED", "PASSED", "PASSED", null, null, null, null, null,
                                        null, null, null, null, null, null, null, null, null, null,
                                        null, null, null };

            string[] gfm1LocList = { good, good, good, good, null, good, bad, null, null, null,
                                     good, good, good, good, good, null, null, null, null, null,
                                     null, good, good, null, null, null, null, null, good, good,
                                     good, good, good };

            string[] gfm1CmpList = { good1, good1, good1, good1, null, good1, good1, null, null, null,
                                     good1, good1, good1, good1, good1, null, null, null, null, null,
                                     null, good1, bad1, null, null, null, null, null, good1, good1,
                                     good1, good1, good1 };

            DateTime?[] gfm1OprHrList = { hourC, hourC, hourC, hourC, null, hourC, hourC, null, null, null,
                                          hourC, hourC, hourC, hourC, hourC, null, null, null, null, null,
                                          null, hourC, hourC, null, null, null, null, null, hourC, hourC,
                                          hourC, hourC, hourC };

            string[] gfm2CmpList = { good2, good2, good2, null, null, null, null, null, null, null,
                                     null, good2, good1, good1, good1, null, null, null, null, null,
                                     null, null, null, null, null, null, null, null, null, null,
                                     null, null, null };

            string[] gfm3CmpList = { null, null, null, null, null, null, null, null, null, null,
                                     null, null, good2, good2, null, null, null, null, null, null,
                                     null, null, null, null, null, null, null, null, null, null,
                                     null, null, null };

            string[] gfm4CmpList = { null, null, null, null, null, null, null, null, null, null,
                                     null, null, good2, null, null, null, null, null, null, null,
                                     null, null, null, null, null, null, null, null, null, null,
                                     null, null, null };

            /* Expected Values */
            string[] expResultList = { null, null, "D", "D", null, "D", null, null, null, "B",
                                       "B", null, "C", "C", "A", "B", null, "B", null, "B",
                                       null, null, "B", null, "B", null, null, null, null, null,
                                       null, null, null };

            string[] expMissingList = { "", "", "", "", "", "", "", "", "", "ONE and TWO",
                                        "TWO", "", "", "", "TWO", "ONE", "", "ONE", "", "ONE",
                                        "", "", "ONE", "", "ONE", "", "", "", "", "",
                                        "", "", "" };

            string[] expMultipleList = { "", "", "", "", "", "", "", "", "", "",
                                         "", "", "ONE and TWO", "ONE", "ONE", "", "", "", "", "",
                                         "", "", "", "", "", "", "", "", "", "",
                                         "", "", "" };

            /* Test Case Count */
            int caseCount = 33;

            /* Check array lengths */
            Assert.AreEqual(caseCount, neededList.Length, "neededList length");
            Assert.AreEqual(caseCount, opTimeList.Length, "opTimeList length");
            Assert.AreEqual(caseCount, trn1LocList.Length, "trn1LocList length");
            Assert.AreEqual(caseCount, trn1CmpList.Length, "trn1CmpList length");
            Assert.AreEqual(caseCount, trn1StatusList.Length, "trn1StatusList length");
            Assert.AreEqual(caseCount, trn1BegList.Length, "trn1BegList length");
            Assert.AreEqual(caseCount, trn1EndList.Length, "trn1EndList length");
            Assert.AreEqual(caseCount, trn2StatusList.Length, "trn2StatusList length");
            Assert.AreEqual(caseCount, gfm1LocList.Length, "gfm1LocList length");
            Assert.AreEqual(caseCount, gfm1CmpList.Length, "gfm1CmpList length");
            Assert.AreEqual(caseCount, gfm1OprHrList.Length, "gfm1OprHrList length");
            Assert.AreEqual(caseCount, gfm2CmpList.Length, "gfm2CmpList length");
            Assert.AreEqual(caseCount, gfm3CmpList.Length, "gfm3CmpList length");
            Assert.AreEqual(caseCount, gfm4CmpList.Length, "gfm4CmpList length");
            Assert.AreEqual(caseCount, expResultList.Length, "expResultList length");
            Assert.AreEqual(caseCount, expMissingList.Length, "expMissingList length");
            Assert.AreEqual(caseCount, expMultipleList.Length, "expMultipleList length");


            /* Run Cases */
            for (int caseDex = 0; caseDex < caseCount; caseDex++)
            {
                /* Initialize Input Parameters */
                EmParameters.CurrentDateHour = hourC;
                EmParameters.CurrentHourlyOpRecord = new VwMpHrlyOpDataRow(opTime: opTimeList[caseDex], monLocId: good);
                EmParameters.DerivedHourlyChecksNeeded = neededList[caseDex];
                EmParameters.MatsHourlyGfmRecordsForHourAndLocation = new CheckDataView<MatsHourlyGfmRecord>();
                {
                    if (gfm1CmpList[caseDex] != null)
                        EmParameters.MatsHourlyGfmRecordsForHourAndLocation.Add(new MatsHourlyGfmRecord(componentId: gfm1CmpList[caseDex], monLocId: gfm1LocList[caseDex]));
                    if (gfm2CmpList[caseDex] != null)
                        EmParameters.MatsHourlyGfmRecordsForHourAndLocation.Add(new MatsHourlyGfmRecord(componentId: gfm2CmpList[caseDex], monLocId: good));
                    if (gfm3CmpList[caseDex] != null)
                        EmParameters.MatsHourlyGfmRecordsForHourAndLocation.Add(new MatsHourlyGfmRecord(componentId: gfm3CmpList[caseDex], monLocId: good));
                    if (gfm4CmpList[caseDex] != null)
                        EmParameters.MatsHourlyGfmRecordsForHourAndLocation.Add(new MatsHourlyGfmRecord(componentId: gfm4CmpList[caseDex], monLocId: good));
                }
                EmParameters.MatsSamplingTrainRecords = new CheckDataView<MatsSamplingTrainRecord>();
                {
                    if (trn1StatusList[caseDex] != null)
                        EmParameters.MatsSamplingTrainRecords.Add(new MatsSamplingTrainRecord(description: "ONE", trainQaStatusCd: trn1StatusList[caseDex], beginDatehour: trn1BegList[caseDex], endDatehour: trn1EndList[caseDex], componentId: trn1CmpList[caseDex], monLocId: trn1LocList[caseDex]));
                    if (trn2StatusList[caseDex] != null)
                        EmParameters.MatsSamplingTrainRecords.Add(new MatsSamplingTrainRecord(description: "TWO", trainQaStatusCd: trn2StatusList[caseDex], beginDatehour: hourB, endDatehour: hourE, componentId: good2, monLocId: good));
                }

                /* Initialize Ouput Parameters */
                EmParameters.MatsMissingGfmList = "Bad List";
                EmParameters.MatsMultipleGfmList = "Bad List";


                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Initialize variables needed to run the check. */
                bool log = false;
                string actual;

                /* Run Check */
                actual = cMATSOperatingHourChecks.MATSHOD16(category, ref log);


                /* Check Results */
                Assert.AreEqual(string.Empty, actual, string.Format("actual[case {0}]", caseDex));
                Assert.AreEqual(false, log, string.Format("log [case {0}]", caseDex));
                Assert.AreEqual(expResultList[caseDex], category.CheckCatalogResult, string.Format("category.CheckCatalogResult [case {0}]", caseDex));

                Assert.AreEqual(expMissingList[caseDex], EmParameters.MatsMissingGfmList, string.Format("MatsMissingGfmList [case {0}]", caseDex));
                Assert.AreEqual(expMultipleList[caseDex], EmParameters.MatsMultipleGfmList, string.Format("MatsMultipleGfmList [case {0}]", caseDex));
            }
        }


        /// <summary>
        /// 
        /// | ## | MODC | GfmCount || Result | MissingList | MultipleList || Note
        /// | 00 | 01   |        0 || B      | ONE         | null         || Primary Monitoring System MODC recieves the error.
        /// | 01 | 02   |        0 || B      | ONE         | null         || Redundant Backup or Regular Non-Redundant Backup Monitoring System MODC recieves the error.
        /// | 02 | 17   |        0 || B      | ONE         | null         || Temporary Like-Kind Replacement Analyzer (CEMS only) MODC recieves the error.
        /// | 03 | 21   |        0 || B      | ONE         | null         || Negative Hourly Average Concentration Replaced with Zero (CEMS only) MODC recieves the error.
        /// | 04 | 32   |        0 || B      | ONE         | null         || Hourly Hg concentration multiplied by a factor of 1.111 MODC recieves the error.
        /// | 05 | 33   |        0 || B      | ONE         | null         || Hourly Hg concentration from higher Hg concentration MODC recieves the error.
        /// | 06 | 34   |        0 || null   | null        | null         || Hourly Hg, HCl, or HF concentration missing or invalid MODC recieves the error.
        /// | 07 | 35   |        0 || B      | ONE         | null         || Hourly Hg, HCl, or HF concentration not monitored MODC recieves the error.
        /// | 08 | 41   |        0 || null   | null        | null         || Hourly Hg concentration determined from two different pairs of sorbent traps during the hour MODC does not receive the error.
        /// | 09 | 42   |        0 || null   | null        | null         || Hourly Hg concentration determined from two different pairs of sorbent traps during the hour. MODC does not receive the error.        /// </summary>
        [TestMethod()]
        public void MatsHod16_MODC()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;


            /* Input Parameter Values */
            DateTime hourC = new DateTime(2016, 6, 17, 22, 0, 0);
            string good = "GOOD";
            string good1 = "GOOD1";

            string[] modcList = { "01", "02", "17", "21", "32", "33", "34", "35", "41", "42" };

            int[] gfmCountList = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            /* Expected Values */
            string[] expResultList = { "B", "B", "B", "B", "B", "B", null, "B", null, null };

            string[] expMissingList = { "ONE", "ONE", "ONE", "ONE", "ONE", "ONE", "", "ONE", "", "" };

            string[] expMultipleList = { "", "", "", "", "", "", "", "", "", "" };

            /* Test Case Count */
            int caseCount = 10;

            /* Check array lengths */
            Assert.AreEqual(caseCount, modcList.Length, "modcList length");
            Assert.AreEqual(caseCount, gfmCountList.Length, "gfmCountList length");
            Assert.AreEqual(caseCount, expResultList.Length, "expResultList length");
            Assert.AreEqual(caseCount, expMissingList.Length, "expMissingList length");
            Assert.AreEqual(caseCount, expMultipleList.Length, "expMultipleList length");


            /* Run Cases */
            for (int caseDex = 0; caseDex < caseCount; caseDex++)
            {
                /* Initialize Input Parameters */
                EmParameters.CurrentDateHour = hourC;
                EmParameters.CurrentHourlyOpRecord = new VwMpHrlyOpDataRow(opTime:1.00m, monLocId: good);
                EmParameters.DerivedHourlyChecksNeeded = true;
                EmParameters.MatsHgcMhvRecord = new MATSMonitorHourlyValueData(modcCd: modcList[caseDex]);
                EmParameters.MatsHourlyGfmRecordsForHourAndLocation = new CheckDataView<MatsHourlyGfmRecord>();
                {
                    EmParameters.MatsHourlyGfmRecordsForHourAndLocation.Add(new MatsHourlyGfmRecord(componentId: "BAD"));

                    for (int dex = 0; dex < gfmCountList[caseDex]; dex++)
                        EmParameters.MatsHourlyGfmRecordsForHourAndLocation.Add(new MatsHourlyGfmRecord(componentId: good1));
                }
                EmParameters.MatsSamplingTrainRecords = new CheckDataView<MatsSamplingTrainRecord>();
                {
                    EmParameters.MatsSamplingTrainRecords.Add(new MatsSamplingTrainRecord(description: "ONE", trainQaStatusCd: "PASSED", beginDatehour: hourC, endDatehour: hourC, componentId: good1, monLocId: good));
                }

                /* Initialize Ouput Parameters */
                EmParameters.MatsMissingGfmList = "Bad List";
                EmParameters.MatsMultipleGfmList = "Bad List";


                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Initialize variables needed to run the check. */
                bool log = false;
                string actual;

                /* Run Check */
                actual = cMATSOperatingHourChecks.MATSHOD16(category, ref log);


                /* Check Results */
                Assert.AreEqual(string.Empty, actual, string.Format("actual[case {0}]", caseDex));
                Assert.AreEqual(false, log, string.Format("log [case {0}]", caseDex));
                Assert.AreEqual(expResultList[caseDex], category.CheckCatalogResult, string.Format("category.CheckCatalogResult [case {0}]", caseDex));

                Assert.AreEqual(expMissingList[caseDex], EmParameters.MatsMissingGfmList, string.Format("MatsMissingGfmList [case {0}]", caseDex));
                Assert.AreEqual(expMultipleList[caseDex], EmParameters.MatsMultipleGfmList, string.Format("MatsMultipleGfmList [case {0}]", caseDex));
            }
        }


        /// <summary>
        /// 
        /// CurrentDateHour : 2017-06-17 10
        /// CurrentHourlyOpRecord.OperatingTime : 1.00
        /// DerivedHourlyChecksNeeded : true
        /// 
        /// When MODC is null, MatsHgcMhvRecord is null.
        /// 
        /// Outgoing Traps are from 2017-06-03 00 to 2017-06-17 10.
        /// Incoming Traps are from 2017-06-17 10 to 2017-07-01 00.
        /// 
        /// Sampling Train Descriptions:
        /// 
        ///     * Component 1 Outgoing: O1
        ///     * Component 1 Incoming: I1
        ///     * Component 2 Outgoing: O2
        ///     * Component 2 Incoming: I2
        /// 
        /// 
        ///             |             Outgoing Trap             |             Incoming Trap             | 
        /// | ## | Modc | Modc| TrainQaStatus1 | TrainQaStatus2 | Modc| TrainQaStatus1 | TrainQaStatus2 | GfmCount1 | GfmCount2 || Result | MissingList        | MultipleList       || Note
        /// |  0 | 01   | 01  | PASSED         | PASSED         | 02  | PASSED         | PASSED         |         0 |         0 || B      | O1, O2, I1, and I2 | null               || MODC 01 at MHV and first train, 02 at second train, and no GFM.
        /// |  1 | 02   | 02  | PASSED         | PASSED         | 01  | PASSED         | PASSED         |         0 |         0 || B      | O1, O2, I1, and I2 | null               || MODC 02 at MHV and first train, 01 at second train, and no GFM.
        /// |  2 | 32   | 32  | FAILED         | PASSED         | 01  | PASSED         | PASSED         |         0 |         0 || B      | O2 and I2          | null               || MODC 32 at MHV and first train, 01 at second train, and no GFM.
        /// |  3 | 32   | 32  | LOST           | PASSED         | 01  | PASSED         | PASSED         |         0 |         0 || B      | O2 and I2          | null               || MODC 32 at MHV and first train, 01 at second train, and no GFM.
        /// |  4 | 33   | 33  | UNCERTAIN      | UNCERTAIN      | 01  | PASSED         | PASSED         |         0 |         0 || B      | O1, O2, I1, and I2 | null               || MODC 33 at MHV and first train, 01 at second train, and no GFM.
        /// |  5 | 34   | 34  | EXPIRED        | EXPIRED        | 01  | PASSED         | PASSED         |         0 |         0 || null   | null               | null               || MODC 34 at MHV and first train, 01 at second train, and no GFM.
        /// |  6 | 34   | 34  | EXPIRED        | FAILED         | 01  | PASSED         | PASSED         |         0 |         0 || null   | null               | null               || MODC 34 at MHV and first train, 01 at second train, and no GFM.
        /// |  7 | 34   | 34  | EXPIRED        | INC            | 01  | PASSED         | PASSED         |         0 |         0 || null   | null               | null               || MODC 34 at MHV and first train, 01 at second train, and no GFM.
        /// |  8 | 34   | 34  | EXPIRED        | PASSED         | 01  | PASSED         | PASSED         |         0 |         0 || null   | null               | null               || MODC 34 at MHV and first train, 01 at second train, and no GFM.
        /// |  9 | 34   | 34  | FAILED         | FAILED         | 01  | PASSED         | PASSED         |         0 |         0 || null   | null               | null               || MODC 34 at MHV and first train, 01 at second train, and no GFM.
        /// | 10 | 34   | 34  | INC            | FAILED         | 01  | PASSED         | PASSED         |         0 |         0 || null   | null               | null               || MODC 34 at MHV and first train, 01 at second train, and no GFM.
        /// | 11 | 34   | 34  | INC            | INC            | 01  | PASSED         | PASSED         |         0 |         0 || null   | null               | null               || MODC 34 at MHV and first train, 01 at second train, and no GFM.
        /// | 12 | 34   | 34  | INC            | PASSED         | 01  | PASSED         | PASSED         |         0 |         0 || null   | null               | null               || MODC 34 at MHV and first train, 01 at second train, and no GFM.
        /// | 13 | 34   | 34  | LOST           | LOST           | 01  | PASSED         | PASSED         |         0 |         0 || null   | null               | null               || MODC 34 at MHV and first train, 01 at second train, and no GFM.
        /// | 14 | 34   | 34  | UNCERTAIN      | UNCERTAIN      | 01  | PASSED         | PASSED         |         0 |         0 || null   | null               | null               || MODC 34 at MHV and first train, 01 at second train, and no GFM.
        /// | 15 | 35   | 35  | PASSED         | PASSED         | 01  | PASSED         | PASSED         |         0 |         0 || B      | O1, O2, I1, and I2 | null               || MODC 35 at MHV and first train, 01 at second train, and no GFM.
        /// | 16 | 01   | 01  | PASSED         | PASSED         | 02  | PASSED         | PASSED         |         1 |         1 || null   | null               | null               || MODC 01 at MHV and first train, 02 at second train, and one GFM for each train.
        /// | 17 | 02   | 02  | PASSED         | PASSED         | 01  | PASSED         | PASSED         |         1 |         1 || null   | null               | null               || MODC 02 at MHV and first train, 01 at second train, and one GFM for each train.
        /// | 18 | 32   | 32  | FAILED         | PASSED         | 01  | PASSED         | PASSED         |         1 |         1 || null   | null               | null               || MODC 32 at MHV and first train, 01 at second train, and one GFM for each train.
        /// | 19 | 33   | 33  | UNCERTAIN      | UNCERTAIN      | 01  | PASSED         | PASSED         |         1 |         1 || null   | null               | null               || MODC 33 at MHV and first train, 01 at second train, and one GFM for each train.
        /// | 20 | 34   | 34  | FAILED         | FAILED         | 01  | PASSED         | PASSED         |         1 |         1 || null   | null               | null               || MODC 34 at MHV and first train, 01 at second train, and one GFM for each train.
        /// | 21 | 34   | 34  | UNCERTAIN      | UNCERTAIN      | 01  | PASSED         | PASSED         |         1 |         1 || null   | null               | null               || MODC 34 at MHV and first train, 01 at second train, and one GFM for each train.
        /// | 22 | 35   | 35  | PASSED         | PASSED         | 01  | PASSED         | PASSED         |         1 |         1 || null   | null               | null               || MODC 35 at MHV and first train, 01 at second train, and one GFM for each train.
        /// | 23 | 01   | 01  | PASSED         | PASSED         | 02  | PASSED         | PASSED         |         2 |         2 || C      | null               | O1, O2, I1, and I2 || MODC 01 at MHV and first train, 02 at second train, and one GFM for each train.
        /// | 24 | 02   | 02  | PASSED         | PASSED         | 01  | PASSED         | PASSED         |         2 |         2 || C      | null               | O1, O2, I1, and I2 || MODC 02 at MHV and first train, 01 at second train, and two GFM for each train.
        /// | 25 | 32   | 32  | FAILED         | PASSED         | 01  | PASSED         | PASSED         |         2 |         2 || C      | null               | O1, O2, I1, and I2 || MODC 32 at MHV and first train, 01 at second train, and two GFM for each train.
        /// | 26 | 33   | 33  | UNCERTAIN      | UNCERTAIN      | 01  | PASSED         | PASSED         |         2 |         2 || C      | null               | O1, O2, I1, and I2 || MODC 33 at MHV and first train, 01 at second train, and two GFM for each train.
        /// | 27 | 34   | 34  | FAILED         | FAILED         | 01  | PASSED         | PASSED         |         2 |         2 || C      | null               | O1, O2, I1, and I2 || MODC 34 at MHV and first train, 01 at second train, and two GFM for each train.
        /// | 28 | 34   | 34  | UNCERTAIN      | UNCERTAIN      | 01  | PASSED         | PASSED         |         2 |         2 || C      | null               | O1, O2, I1, and I2 || MODC 34 at MHV and first train, 01 at second train, and two GFM for each train.
        /// | 29 | 35   | 35  | PASSED         | PASSED         | 01  | PASSED         | PASSED         |         2 |         2 || C      | null               | O1, O2, I1, and I2 || MODC 35 at MHV and first train, 01 at second train, and two GFM for each train.
        /// | 30 | 32   | 01  | PASSED         | PASSED         | 32  | FAILED         | PASSED         |         0 |         0 || B      | O2 and I2          | null               || MODC 32 at MHV and second train, 01 at first train, and no GFM.
        /// | 31 | 34   | 01  | PASSED         | PASSED         | 34  | FAILED         | FAILED         |         0 |         0 || null   | null               | null               || MODC 34 at MHV and second train, 01 at first train, and no GFM.
        /// | 32 | 01   | 01  | PASSED         | PASSED         | 32  | FAILED         | PASSED         |         0 |         0 || B      | O1, O2, and I2     | null               || MODC 32 at MHV and second train, 01 at first train, and no GFM.
        /// | 33 | 01   | 01  | PASSED         | PASSED         | 34  | FAILED         | FAILED         |         0 |         0 || B      | O1 and O2          | null               || MODC 34 at MHV and second train, 01 at first train, and no GFM.
        /// | 34 | null | 01  | PASSED         | PASSED         | 02  | PASSED         | PASSED         |         0 |         0 || B      | O1, O2, I1, and I2 | null               || MHV is null, MODC 01 at first train, 02 at second train, and no GFM.
        /// </summary>
        [TestMethod()]
        public void MatsHod16_TransitionHours()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;


            /* Input Parameter Values */
            string componentId1 = "CmpId1";
            string componentId2 = "CmpId2";
            DateTime? currentDatehour = new DateTime(2017, 6, 17, 22, 0, 0);
            bool? derivedHourlyChecksNeeded = true;
            DateTime? incomingTrainBeginDatehour = currentDatehour;
            DateTime? incomingTrainEndDatehour = new DateTime(2017, 7, 1, 0, 0, 0);
            string monLocId = "LocId";
            decimal? opTime = 1.00m;
            DateTime? outgoingTrainBeginDatehour = new DateTime(2017, 6, 3, 0, 0, 0);
            DateTime? outgoingTrainEndDatehour = currentDatehour;

            /* Input Parameter Lists */
            string[] mhvModcList = {"01", "02", "32", "32", "33", "34", "34", "34", "34", "34",
                                    "34", "34", "34", "34", "34", "35", "01", "02", "32", "33",
                                    "34", "34", "35", "01", "02", "32", "33", "34", "34", "35",
                                    "32", "34", "01", "01", null};
            string[] oTrapModcList = {"01", "02", "32", "32", "33", "34", "34", "34", "34", "34",
                                      "34", "34", "34", "34", "34", "35", "01", "02", "32", "33",
                                      "34", "34", "35", "01", "02", "32", "33", "34", "34", "35",
                                      "01", "01", "01", "01", "01"};
            string[] oTrian1StatusList = {"PASSED", "PASSED", "FAILED", "LOST", "UNCERTAIN", "EXPIRED", "EXPIRED", "EXPIRED", "EXPIRED", "FAILED",
                                          "INC", "INC", "INC", "LOST", "UNCERTAIN", "PASSED", "PASSED", "PASSED", "FAILED", "UNCERTAIN",
                                          "FAILED", "UNCERTAIN", "PASSED", "PASSED", "PASSED", "FAILED", "UNCERTAIN", "FAILED", "UNCERTAIN", "PASSED",
                                          "PASSED", "PASSED", "PASSED", "PASSED", "PASSED"};
            string[] oTrian2StatusList = {"PASSED", "PASSED", "PASSED", "PASSED", "UNCERTAIN", "EXPIRED", "FAILED", "INC", "PASSED", "FAILED",
                                          "FAILED", "INC", "PASSED", "LOST", "UNCERTAIN", "PASSED", "PASSED", "PASSED", "PASSED", "UNCERTAIN",
                                          "FAILED", "UNCERTAIN", "PASSED", "PASSED", "PASSED", "PASSED", "UNCERTAIN", "FAILED", "UNCERTAIN", "PASSED",
                                          "PASSED", "PASSED", "PASSED", "PASSED", "PASSED"};
            string[] iTrapModcList = {"02", "01", "01", "01", "01", "01", "01", "01", "01", "01",
                                      "01", "01", "01", "01", "01", "01", "02", "01", "01", "01",
                                      "01", "01", "01", "02", "01", "01", "01", "01", "01", "01",
                                      "32", "34", "32", "34", "02"};
            string[] iTrian1StatusList = {"PASSED", "PASSED", "PASSED", "PASSED", "PASSED", "PASSED", "PASSED", "PASSED", "PASSED", "PASSED",
                                          "PASSED", "PASSED", "PASSED", "PASSED", "PASSED", "PASSED", "PASSED", "PASSED", "PASSED", "PASSED",
                                          "PASSED", "PASSED", "PASSED", "PASSED", "PASSED", "PASSED", "PASSED", "PASSED", "PASSED", "PASSED",
                                          "FAILED", "FAILED", "FAILED", "FAILED", "PASSED"};
            string[] iTrian2StatusList = {"PASSED", "PASSED", "PASSED", "PASSED", "PASSED", "PASSED", "PASSED", "PASSED", "PASSED", "PASSED",
                                          "PASSED", "PASSED", "PASSED", "PASSED", "PASSED", "PASSED", "PASSED", "PASSED", "PASSED", "PASSED",
                                          "PASSED", "PASSED", "PASSED", "PASSED", "PASSED", "PASSED", "PASSED", "PASSED", "PASSED", "PASSED",
                                          "PASSED", "FAILED", "PASSED", "FAILED", "PASSED"};
            int?[] gfm1CountList = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                     0, 0, 0, 0, 0, 0, 1, 1, 1, 1,
                                     1, 1, 1, 2, 2, 2, 2, 2, 2, 2,
                                     0, 0, 0, 0, 0 };
            int?[] gfm2CountList = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                     0, 0, 0, 0, 0, 0, 1, 1, 1, 1,
                                     1, 1, 1, 2, 2, 2, 2, 2, 2, 2,
                                     0, 0, 0, 0, 0 };

            /* Expected Value Lists */
            string[] expResultList = { "B", "B", "B", "B", "B", null, null, null, null, null,
                                       null, null, null, null, null, "B", null, null, null, null,
                                       null, null, null, "C", "C", "C", "C", "C", "C", "C",
                                       "B", null, "B", "B", "B" };
            string[] expMissingList = { "O1, O2, I1, and I2", "O1, O2, I1, and I2", "O2 and I2", "O2 and I2", "O1, O2, I1, and I2", "", "", "", "", "",
                                        "", "", "", "", "",  "O1, O2, I1, and I2", "", "", "", "",
                                        "", "", "", "", "", "", "", "", "", "",
                                         "O2 and I2", "", "O1, O2, and I2", "O1 and O2", "O1, O2, I1, and I2" };
            string[] expMultipleList = { "", "", "", "", "", "", "", "", "", "",
                                         "", "", "", "", "", "", "", "", "", "",
                                         "", "", "", "O1, O2, I1, and I2", "O1, O2, I1, and I2", "O1, O2, I1, and I2", "O1, O2, I1, and I2", "O1, O2, I1, and I2", "O1, O2, I1, and I2", "O1, O2, I1, and I2",
                                         "", "", "", "", "" };

            /* Test Case Count */
            int caseCount = 35;

            /* Check array lengths */
            Assert.AreEqual(caseCount, mhvModcList.Length, "mhvModcList length");
            Assert.AreEqual(caseCount, oTrapModcList.Length, "oTrapModcList length");
            Assert.AreEqual(caseCount, oTrian1StatusList.Length, "oTrian1StatusList length");
            Assert.AreEqual(caseCount, oTrian2StatusList.Length, "oTrian2StatusList length");
            Assert.AreEqual(caseCount, iTrapModcList.Length, "iTrapModcList length");
            Assert.AreEqual(caseCount, iTrian1StatusList.Length, "iTrian1StatusList length");
            Assert.AreEqual(caseCount, iTrian2StatusList.Length, "iTrian2StatusList length");
            Assert.AreEqual(caseCount, gfm1CountList.Length, "gfm1Count length");
            Assert.AreEqual(caseCount, gfm2CountList.Length, "gfm2Count length");
            Assert.AreEqual(caseCount, expResultList.Length, "expResultList length");
            Assert.AreEqual(caseCount, expMissingList.Length, "expMissingList length");
            Assert.AreEqual(caseCount, expMultipleList.Length, "expMultipleList length");


            /* Run Cases */
            for (int caseDex = 0; caseDex < caseCount; caseDex++)
            {
                /* Initialize Input Parameters */
                EmParameters.CurrentDateHour = currentDatehour;
                EmParameters.CurrentHourlyOpRecord = new VwMpHrlyOpDataRow(opTime: opTime, monLocId: monLocId);
                EmParameters.DerivedHourlyChecksNeeded = derivedHourlyChecksNeeded;
                EmParameters.MatsHgcMhvRecord = (mhvModcList[caseDex] != null) ? new MATSMonitorHourlyValueData(modcCd: mhvModcList[caseDex]) : null;
                EmParameters.MatsHourlyGfmRecordsForHourAndLocation = new CheckDataView<MatsHourlyGfmRecord>();
                {
                    EmParameters.MatsHourlyGfmRecordsForHourAndLocation.Add(new MatsHourlyGfmRecord(componentId: "BAD"));

                    for (int dex = 0; dex < gfm1CountList[caseDex]; dex++)
                        EmParameters.MatsHourlyGfmRecordsForHourAndLocation.Add(new MatsHourlyGfmRecord(componentId: componentId1));

                    for (int dex = 0; dex < gfm2CountList[caseDex]; dex++)
                        EmParameters.MatsHourlyGfmRecordsForHourAndLocation.Add(new MatsHourlyGfmRecord(componentId: componentId2));
                }
                EmParameters.MatsSamplingTrainRecords = new CheckDataView<MatsSamplingTrainRecord>();
                {
                    if (oTrian1StatusList[caseDex] != null)
                        EmParameters.MatsSamplingTrainRecords.Add(new MatsSamplingTrainRecord(description: "O1", trainQaStatusCd: oTrian1StatusList[caseDex], trapModcCd: oTrapModcList[caseDex], componentId: componentId1,
                                                                                              beginDatehour: outgoingTrainBeginDatehour, endDatehour: outgoingTrainEndDatehour, monLocId: monLocId));
                    if (oTrian2StatusList[caseDex] != null)
                        EmParameters.MatsSamplingTrainRecords.Add(new MatsSamplingTrainRecord(description: "O2", trainQaStatusCd: oTrian2StatusList[caseDex], trapModcCd: oTrapModcList[caseDex], componentId: componentId2,
                                                                                              beginDatehour: outgoingTrainBeginDatehour, endDatehour: outgoingTrainEndDatehour, monLocId: monLocId));
                    if (iTrian1StatusList[caseDex] != null)
                        EmParameters.MatsSamplingTrainRecords.Add(new MatsSamplingTrainRecord(description: "I1", trainQaStatusCd: iTrian1StatusList[caseDex], trapModcCd: iTrapModcList[caseDex], componentId: componentId1,
                                                                                              beginDatehour: incomingTrainBeginDatehour, endDatehour: incomingTrainEndDatehour, monLocId: monLocId));
                    if (iTrian2StatusList[caseDex] != null)
                        EmParameters.MatsSamplingTrainRecords.Add(new MatsSamplingTrainRecord(description: "I2", trainQaStatusCd: iTrian2StatusList[caseDex], trapModcCd: iTrapModcList[caseDex], componentId: componentId2,
                                                                                              beginDatehour: incomingTrainBeginDatehour, endDatehour: incomingTrainEndDatehour, monLocId: monLocId));
                }

                /* Initialize Ouput Parameters */
                EmParameters.MatsMissingGfmList = "Bad List";
                EmParameters.MatsMultipleGfmList = "Bad List";


                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Initialize variables needed to run the check. */
                bool log = false;
                string actual;

                /* Run Check */
                actual = cMATSOperatingHourChecks.MATSHOD16(category, ref log);


                /* Check Results */
                Assert.AreEqual(string.Empty, actual, string.Format("actual[case {0}]", caseDex));
                Assert.AreEqual(false, log, string.Format("log [case {0}]", caseDex));

                Assert.AreEqual(expResultList[caseDex], category.CheckCatalogResult, string.Format("category.CheckCatalogResult [case {0}]", caseDex));

                Assert.AreEqual(expMissingList[caseDex], EmParameters.MatsMissingGfmList, string.Format("MatsMissingGfmList [case {0}]", caseDex));
                Assert.AreEqual(expMultipleList[caseDex], EmParameters.MatsMultipleGfmList, string.Format("MatsMultipleGfmList [case {0}]", caseDex));
            }
        }

        #endregion

        #endregion
    }
}