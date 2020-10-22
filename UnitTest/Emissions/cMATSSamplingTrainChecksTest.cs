using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.CheckEngine.SpecialParameterClasses;
using ECMPS.Checks.Data.Ecmps.CheckEm.Function;
using ECMPS.Checks.Data.Ecmps.Lookup.Table;
using ECMPS.Checks.Em.Parameters;
using ECMPS.Checks.EmissionsChecks;
using ECMPS.Checks.EmissionsReport;
using ECMPS.Definitions.Extensions;

using UnitTest.UtilityClasses;

namespace UnitTest.Emissions
{
    [TestClass()]
    public class cMATSSamplingTrainChecksTest
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

        /// <summary>
        ///A test for MatsTrn1
        ///</summary>()
        [TestMethod()]
        public void MatsTrn1()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            /* Initialize variables needed to run the check. */
            bool log = false;
            string actual;

            /* Initialize General Variables */
            string[] componentCaseList = { "Null", "NotSTrain", "New", "Border", "Supplemental" };

            foreach (string componentCase in componentCaseList)
            {
                /* Initialize Variables */
                string componentId, componentTypeCd, result, trapTrainId;
                int? borderInd, suppInd;
                {
                    switch (componentCase)
                    {
                        case "New": trapTrainId = "NewId";  componentId = "CompId"; componentTypeCd = "STRAIN"; borderInd = 0; suppInd = 0; result = null; break;
                        case "NotSTrain": trapTrainId = "NewId"; componentId = "CompId"; componentTypeCd = "OTRAIN"; borderInd = 0; suppInd = 0; result = "B"; break;
                        case "Border": trapTrainId = "NewId"; componentId = "CompId"; componentTypeCd = "STRAIN"; borderInd = 1; suppInd = 0; result = null; break;
                        case "Supplemental": trapTrainId = "NewId"; componentId = "CompId"; componentTypeCd = "STRAIN"; borderInd = 0; suppInd = 1; result = null; break;
                        default: trapTrainId = null; componentId = null; componentTypeCd = null; borderInd = null; suppInd = null; result = "A"; break;
                    }
                }

                /*  Initialize Input Parameters*/
                EmParameters.MatsSamplingTrainRecord = new MatsSamplingTrainRecord(trapTrainId: trapTrainId, componentId: componentId, componentTypeCd: componentTypeCd, borderTrapInd: borderInd, suppDataInd: suppInd);

                /* Initialize Updatable Parameters*/
                EmParameters.MatsSamplingTrainDictionary = new Dictionary<string, SamplingTrainEvalInformation>();
                {
                    EmParameters.MatsSamplingTrainDictionary["OldId"] = new SamplingTrainEvalInformation("OldId");
                }
                EmParameters.MatsSamplingTrainProblemComponentExists = null;
                EmParameters.MatsSorbentTrapSamplingTrainList = new List<SamplingTrainEvalInformation>();

                /* Initialize Output Parameters */
                EmParameters.MatsSamplingTrainComponentIdValid = null;

                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                // Run Checks
                actual = cMATSSamplingTrainChecks.MatsTrn1(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(result, category.CheckCatalogResult, componentCase + ".Result");

                Assert.AreEqual((result == null ? (bool?)null : true), EmParameters.MatsSamplingTrainProblemComponentExists, componentCase + ".MatsSamplingTrainProblemComponentExists");
                Assert.AreEqual((result == null), EmParameters.MatsSamplingTrainComponentIdValid, componentCase + ".MatsSamplingTrainComponentIdValid");
                Assert.AreEqual((result == null), EmParameters.MatsSamplingTrainDictionary.ContainsKey("NewId"), componentCase + ".MatsSamplingTrainDictionary");

                if (result == null)
                {
                    Assert.AreEqual((borderInd == 1), EmParameters.MatsSamplingTrainDictionary["NewId"].IsBorderTrain, componentCase + ".IsBorderTrain");
                    Assert.AreEqual((suppInd == 1), EmParameters.MatsSamplingTrainDictionary["NewId"].IsSupplementalData, componentCase + ".IsSupplementalData");
                }
            }
        }

        /// <summary>
        ///A test for MatsTrn2
        ///</summary>()
        [TestMethod()]
        public void MatsTrn2()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            /* Initialize variables needed to run the check. */
            bool log = false;
            string actual;

            /* Initialize General Variables */
            string[] serialNumberList = { null, "SomeDumbNumber" };

            /* Initialize MatsSamplingTrainDictionary */
            EmParameters.MatsSamplingTrainDictionary = new Dictionary<string, SamplingTrainEvalInformation>();
            EmParameters.MatsSamplingTrainDictionary["OldId"] = new SamplingTrainEvalInformation("OldId");

            foreach (string serialNumber in serialNumberList)
            {
                /*  Initialize Input Parameters*/
                EmParameters.MatsSamplingTrainRecord = new MatsSamplingTrainRecord(sorbentTrapSerialNumber: serialNumber);

                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                // Run Checks
                actual = cMATSSamplingTrainChecks.MatsTrn2(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual((serialNumber == null ? "A" : null), category.CheckCatalogResult, string.Format("[serialNumber: {0}].Result", serialNumber));
            }
        }

        /// <summary>
        ///A test for MatsTrn3
        ///</summary>()
        [TestMethod()]
        public void MatsTrn3()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            /* Initialize variables needed to run the check. */
            bool log = false;
            string actual;

            /* Initialize General Variables */
            string[] trainQaStatusList = { null, "Bad", "Good" };
            bool?[] validList = { null, false, true };

            /* Initialize MatsSamplingTrainDictionary */
            EmParameters.MatsSamplingTrainDictionary = new Dictionary<string, SamplingTrainEvalInformation>();
            EmParameters.MatsSamplingTrainDictionary["TrapTrainId"] = new SamplingTrainEvalInformation("TrapTrainId");

            foreach (string trainQaStatusCd in trainQaStatusList)
                foreach (bool? componentIdValid in validList)
                {
                    /*  Initialize Input Parameters*/
                    EmParameters.MatsSamplingTrainComponentIdValid = componentIdValid;
                    EmParameters.MatsSamplingTrainQaStatusLookupTable = new CheckDataView<TrainQaStatusCodeRow>(new TrainQaStatusCodeRow(trainQaStatusCd: "Good"));
                    EmParameters.MatsSamplingTrainRecord = new MatsSamplingTrainRecord(trapTrainId: "TrapTrainId", trainQaStatusCd: trainQaStatusCd);

                    /* Initialize Updatable Parameters */
                    EmParameters.MatsSamplingTrainDictionary["TrapTrainId"].SamplingTrainValid = null;
                    EmParameters.MatsSamplingTrainDictionary["TrapTrainId"].TrainQAStatusCode = null;

                    /* Initialize Output Parameters */
                    EmParameters.MatsSamplingTrainQaStatusCodeValid = null;

                    /* Expected Results */
                    string result;
                    bool? expSamplingTrainValid = null;
                    string expTrainQaStatus = null;

                    switch (trainQaStatusCd)
                    {
                        case "Good": result = null; break;
                        case "Bad": result = "B"; break;
                        default: result = "A"; break;
                    }

                    if (componentIdValid.Default(false))
                    {
                        if (result == null)
                            expTrainQaStatus = trainQaStatusCd;
                        else
                            expSamplingTrainValid = false;
                    }

                    /* Init Cateogry Result */
                    category.CheckCatalogResult = null;

                    // Run Checks
                    actual = cMATSSamplingTrainChecks.MatsTrn3(category, ref log);

                    /* Check Result Label */
                    string resultPrefix = string.Format("[trainQaStatusCd: {0}, componentIdValid: {1}]", trainQaStatusCd, componentIdValid);

                    // Check Results
                    Assert.AreEqual(string.Empty, actual);
                    Assert.AreEqual(false, log);
                    Assert.AreEqual(result, category.CheckCatalogResult, resultPrefix + ".Result");

                    Assert.AreEqual((result == null), EmParameters.MatsSamplingTrainQaStatusCodeValid, resultPrefix + ".MatsSamplingTrainQaStatusCodeValid");
                    Assert.AreEqual(expSamplingTrainValid, EmParameters.MatsSamplingTrainDictionary["TrapTrainId"].SamplingTrainValid, resultPrefix + ".MatsSamplingTrainDictionary.SamplingTrainValid");
                    Assert.AreEqual(expTrainQaStatus, EmParameters.MatsSamplingTrainDictionary["TrapTrainId"].TrainQAStatusCode, resultPrefix + ".MatsSamplingTrainDictionary.TrainQAStatusCode");
                }
        }

        /// <summary>
        ///A test for MatsTrn4
        ///</summary>()
        [TestMethod()]
        public void MatsTrn4()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            /* Initialize variables needed to run the check. */
            bool log = false;
            string actual;

            /* Initialize General Variables */
            string[] trainQaStatusList = { "EXPIRED", "FAILED", "INC", "LOST", "PASSED", "UNCERTAIN" };
            bool?[] validList = { null, false, true };
            string[] valueTypeList = { null, "NotRounded", "Rounded" };

            /* Initialize MatsSamplingTrainDictionary */
            EmParameters.MatsSamplingTrainDictionary = new Dictionary<string, SamplingTrainEvalInformation>();
            EmParameters.MatsSamplingTrainDictionary["ComponentId"] = new SamplingTrainEvalInformation("ComponentId");

            foreach (bool? valid in validList)
                foreach (string trainQaStatusCd in trainQaStatusList)
                    foreach (string valueType in valueTypeList)
                    {
                        /*  */
                        string value;
                        {
                            switch (valueType)
                            {
                                case "Rounded": value = "1.23E0"; break;
                                case "NotRounded": value = "1.234E0"; break;
                                default: value = null; break;
                            }
                        }

                        /*  Initialize Input Parameters*/
                        EmParameters.MatsSamplingTrainQaStatusCodeValid = valid;
                        EmParameters.MatsSamplingTrainRecord = new MatsSamplingTrainRecord(trainQaStatusCd: trainQaStatusCd, mainTrapHg: value);

                        /* Initialize Output Parameters */
                        EmParameters.MatsMainTrapHgValid = null;

                        /* Expected Results */
                        string result = null;
                        bool? expectedValid = false;

                        if (valid.Default(false))
                        {
                            if (valueType == null)
                            {
                                if (trainQaStatusCd.NotInList("INC,EXPIRED,LOST"))
                                    result = "A";
                                else
                                    expectedValid = true;
                            }
                            else
                            {
                                if (trainQaStatusCd.NotInList("PASSED,FAILED,UNCERTAIN"))
                                    result = "B";
                                else if (valueType == "NotRounded")
                                    result = "C";
                                else
                                    expectedValid = true;
                            }
                        }

                        /* Init Cateogry Result */
                        category.CheckCatalogResult = null;

                        // Run Checks
                        actual = cMATSSamplingTrainChecks.MatsTrn4(category, ref log);

                        /* Check Result Label */
                        string resultPrefix = string.Format("[valid: {0}, trainQaStatusCd: {1}, valueType: {2}]", valid, trainQaStatusCd, valueType);

                        // Check Results
                        Assert.AreEqual(string.Empty, actual);
                        Assert.AreEqual(false, log);
                        Assert.AreEqual(result, category.CheckCatalogResult, resultPrefix + ".Result");
                        Assert.AreEqual(expectedValid, EmParameters.MatsMainTrapHgValid, resultPrefix + ".Valid");
                    }
        }

        /// <summary>
        ///A test for MatsTrn5
        ///</summary>()
        [TestMethod()]
        public void MatsTrn5()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            /* Initialize variables needed to run the check. */
            bool log = false;
            string actual;

            /* Initialize General Variables */
            string[] trainQaStatusList = { "EXPIRED", "FAILED", "INC", "LOST", "PASSED", "UNCERTAIN" };
            bool?[] validList = { null, false, true };
            string[] valueTypeList = { null, "NotRounded", "Rounded" };

            /* Initialize MatsSamplingTrainDictionary */
            EmParameters.MatsSamplingTrainDictionary = new Dictionary<string, SamplingTrainEvalInformation>();
            EmParameters.MatsSamplingTrainDictionary["ComponentId"] = new SamplingTrainEvalInformation("ComponentId");

            foreach (bool? valid in validList)
                foreach (string trainQaStatusCd in trainQaStatusList)
                    foreach (string valueType in valueTypeList)
                    {
                        /*  */
                        string value;
                        {
                            switch (valueType)
                            {
                                case "Rounded": value = "1.23E0"; break;
                                case "NotRounded": value = "1.234E0"; break;
                                default: value = null; break;
                            }
                        }

                        /*  Initialize Input Parameters*/
                        EmParameters.MatsSamplingTrainQaStatusCodeValid = valid;
                        EmParameters.MatsSamplingTrainRecord = new MatsSamplingTrainRecord(trainQaStatusCd: trainQaStatusCd, breakthroughTrapHg: value);

                        /* Initialize Output Parameters */
                        EmParameters.MatsBtTrapHgValid = null;

                        /* Expected Results */
                        string result = null;
                        bool? expectedValid = false;

                        if (valid.Default(false))
                        {
                            if (valueType == null)
                            {
                                if (trainQaStatusCd.NotInList("INC,EXPIRED,LOST"))
                                    result = "A";
                                else
                                    expectedValid = true;
                            }
                            else
                            {
                                if (trainQaStatusCd.NotInList("PASSED,FAILED,UNCERTAIN"))
                                    result = "B";
                                else if (valueType == "NotRounded")
                                    result = "C";
                                else
                                    expectedValid = true;
                            }
                        }

                        /* Init Cateogry Result */
                        category.CheckCatalogResult = null;

                        // Run Checks
                        actual = cMATSSamplingTrainChecks.MatsTrn5(category, ref log);

                        /* Check Result Label */
                        string resultPrefix = string.Format("[valid: {0}, trainQaStatusCd: {1}, valueType: {2}]", valid, trainQaStatusCd, valueType);

                        // Check Results
                        Assert.AreEqual(string.Empty, actual);
                        Assert.AreEqual(false, log);
                        Assert.AreEqual(result, category.CheckCatalogResult, resultPrefix + ".Result");
                        Assert.AreEqual(expectedValid, EmParameters.MatsBtTrapHgValid, resultPrefix + ".Valid");
                    }
        }

        /// <summary>
        ///A test for MatsTrn6
        ///</summary>()
        [TestMethod()]
        public void MatsTrn6()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            /* Initialize variables needed to run the check. */
            bool log = false;
            string actual;

            /* Initialize General Variables */
            string[] trainQaStatusList = { "EXPIRED", "FAILED", "INC", "LOST", "PASSED", "UNCERTAIN" };
            bool?[] validList = { null, false, true };
            string[] valueTypeList = { null, "NotRounded", "Rounded" };

            /* Initialize MatsSamplingTrainDictionary */
            EmParameters.MatsSamplingTrainDictionary = new Dictionary<string, SamplingTrainEvalInformation>();
            EmParameters.MatsSamplingTrainDictionary["ComponentId"] = new SamplingTrainEvalInformation("ComponentId");

            foreach (bool? valid in validList)
                foreach (string trainQaStatusCd in trainQaStatusList)
                    foreach (string valueType in valueTypeList)
                    {
                        /*  */
                        string value;
                        {
                            switch (valueType)
                            {
                                case "Rounded": value = "1.23E0"; break;
                                case "NotRounded": value = "1.234E0"; break;
                                default: value = null; break;
                            }
                        }

                        /*  Initialize Input Parameters*/
                        EmParameters.MatsSamplingTrainQaStatusCodeValid = valid;
                        EmParameters.MatsSamplingTrainRecord = new MatsSamplingTrainRecord(trainQaStatusCd: trainQaStatusCd, spikeTrapHg: value);

                        /* Initialize Output Parameters */
                        EmParameters.MatsSpikeTrapHgValid = null;

                        /* Expected Results */
                        string result = null;
                        bool? expectedValid = false;

                        if (valid.Default(false))
                        {
                            if (valueType == null)
                            {
                                if (trainQaStatusCd.NotInList("INC,EXPIRED,LOST"))
                                    result = "A";
                                else
                                    expectedValid = true;
                            }
                            else
                            {
                                if (trainQaStatusCd.NotInList("PASSED,FAILED,UNCERTAIN"))
                                    result = "B";
                                else if (valueType == "NotRounded")
                                    result = "C";
                                else
                                    expectedValid = true;
                            }
                        }

                        /* Init Cateogry Result */
                        category.CheckCatalogResult = null;

                        // Run Checks
                        actual = cMATSSamplingTrainChecks.MatsTrn6(category, ref log);

                        /* Check Result Label */
                        string resultPrefix = string.Format("[valid: {0}, trainQaStatusCd: {1}, valueType: {2}]", valid, trainQaStatusCd, valueType);

                        // Check Results
                        Assert.AreEqual(string.Empty, actual);
                        Assert.AreEqual(false, log);
                        Assert.AreEqual(result, category.CheckCatalogResult, resultPrefix + ".Result");
                        Assert.AreEqual(expectedValid, EmParameters.MatsSpikeTrapHgValid, resultPrefix + ".Valid");
                    }
        }

        /// <summary>
        ///A test for MatsTrn7
        ///</summary>()
        [TestMethod()]
        public void MatsTrn7()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            /* Initialize variables needed to run the check. */
            bool log = false;
            string actual;

            /* Initialize General Variables */
            string[] trainQaStatusList = { "EXPIRED", "FAILED", "INC", "LOST", "PASSED", "UNCERTAIN" };
            bool?[] validList = { null, false, true };
            string[] valueTypeList = { null, "NotRounded", "Rounded" };

            /* Initialize MatsSamplingTrainDictionary */
            EmParameters.MatsSamplingTrainDictionary = new Dictionary<string, SamplingTrainEvalInformation>();
            EmParameters.MatsSamplingTrainDictionary["ComponentId"] = new SamplingTrainEvalInformation("ComponentId");

            foreach (bool? valid in validList)
                foreach (string trainQaStatusCd in trainQaStatusList)
                    foreach (string valueType in valueTypeList)
                    {
                        /*  */
                        string value;
                        {
                            switch (valueType)
                            {
                                case "Rounded": value = "1.23E0"; break;
                                case "NotRounded": value = "1.234E0"; break;
                                default: value = null; break;
                            }
                        }

                        /*  Initialize Input Parameters*/
                        EmParameters.MatsSamplingTrainQaStatusCodeValid = valid;
                        EmParameters.MatsSamplingTrainRecord = new MatsSamplingTrainRecord(trainQaStatusCd: trainQaStatusCd, spikeRefValue: value);

                        /* Initialize Output Parameters */
                        EmParameters.MatsSpikeReferenceValueValid = null;

                        /* Expected Results */
                        string result = null;
                        bool? expectedValid = false;

                        if (valid.Default(false))
                        {
                            if (valueType == null)
                            {
                                if (trainQaStatusCd.NotInList("INC,EXPIRED,LOST"))
                                    result = "A";
                                else
                                    expectedValid = true;
                            }
                            else
                            {
                                if (trainQaStatusCd.NotInList("PASSED,FAILED,UNCERTAIN"))
                                    result = "B";
                                else if (valueType == "NotRounded")
                                    result = "C";
                                else
                                    expectedValid = true;
                            }
                        }

                        /* Init Cateogry Result */
                        category.CheckCatalogResult = null;

                        // Run Checks
                        actual = cMATSSamplingTrainChecks.MatsTrn7(category, ref log);

                        /* Check Result Label */
                        string resultPrefix = string.Format("[valid: {0}, trainQaStatusCd: {1}, valueType: {2}]", valid, trainQaStatusCd, valueType);

                        // Check Results
                        Assert.AreEqual(string.Empty, actual);
                        Assert.AreEqual(false, log);
                        Assert.AreEqual(result, category.CheckCatalogResult, resultPrefix + ".Result");
                        Assert.AreEqual(expectedValid, EmParameters.MatsSpikeReferenceValueValid, resultPrefix + ".Valid");
                    }
        }

        /// <summary>
        ///A test for MatsTrn8
        ///</summary>()
        //[TestMethod()]
        public void MatsTrn8()
        {
            //TODO (EC-3519): Replace or rethink this test.  Removed TestMethod attribute to stop it from running.  If fails because of changes subsequent to when it was written.
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            /* Initialize variables needed to run the check. */
            bool log = false;
            string actual;

            /* Initialize General Variables */
            string[] trainQaStatusList = { "EXPIRED", "FAILED", "INC", "LOST", "PASSED", "UNCERTAIN" };
            bool?[] validList = { null, false, true };
            string[] valueTypeList = { null, "NotRounded", "Rounded" };

            /* Initialize MatsSamplingTrainDictionary */
            EmParameters.MatsSamplingTrainDictionary = new Dictionary<string, SamplingTrainEvalInformation>();
            EmParameters.MatsSamplingTrainDictionary["ComponentId"] = new SamplingTrainEvalInformation("ComponentId");

            foreach (bool? valid in validList)
                foreach (string trainQaStatusCd in trainQaStatusList)
                    foreach (string valueType in valueTypeList)
                    {
                        /*  */
                        decimal? value;
                        {
                            switch (valueType)
                            {
                                case "Rounded": value = 1.23m; break;
                                case "NotRounded": value = 1.234m; break;
                                default: value = null; break;
                            }
                        }

                        /*  Initialize Input Parameters*/
                        EmParameters.MatsSamplingTrainQaStatusCodeValid = valid;
                        EmParameters.MatsSamplingTrainRecord = new MatsSamplingTrainRecord(trainQaStatusCd: trainQaStatusCd, totalSampleVolume: value);

                        /* Initialize Output Parameters */
                        EmParameters.MatsTotalSampleVolumeDscmValid = null;

                        /* Expected Results */
                        string result = null;
                        bool? expectedValid = false;

                        if (valid.Default(false))
                        {
                            if (valueType == null)
                            {
                                if (trainQaStatusCd.NotInList("INC,EXPIRED,LOST"))
                                    result = "A";
                                else
                                    expectedValid = true;
                            }
                            else
                            {
                                if (trainQaStatusCd.NotInList("PASSED,FAILED,UNCERTAIN"))
                                    result = "B";
                                else if (valueType == "NotRounded")
                                    result = "C";
                                else
                                    expectedValid = true;
                            }
                        }

                        /* Init Cateogry Result */
                        category.CheckCatalogResult = null;

                        // Run Checks
                        actual = cMATSSamplingTrainChecks.MatsTrn8(category, ref log);

                        /* Check Result Label */
                        string resultPrefix = string.Format("[valid: {0}, trainQaStatusCd: {1}, valueType: {2}]", valid, trainQaStatusCd, valueType);

                        // Check Results
                        Assert.AreEqual(string.Empty, actual);
                        Assert.AreEqual(false, log);
                        Assert.AreEqual(result, category.CheckCatalogResult, resultPrefix + ".Result");
                        Assert.AreEqual(expectedValid, EmParameters.MatsTotalSampleVolumeDscmValid, resultPrefix + ".Valid");
                    }
        }


        /// <summary>
        /// MatsTrn-9
        /// 
        /// | ## | Valid | SFSR | QaStatus  | TrainId | RataInd | DictId | DictValid | DictSFSR || Result | DictExists | Valid | SFSR || Note
        /// |  0 | false | null | BAD       | GoodId  | null    | GoodId | true      | null     || null   | true       | true  | null || QA Status Code is not valid.
        /// |  1 | true  | null | PASSED    | GoodId  | null    | GoodId | true      | null     || A      | true       | false | null || Null SFSR, but 'PASSED' QA Status results in A and updated dictionary.
        /// |  2 | true  | null | PASSED    | GoodId  | null    | BadId  | true      | null     || A      | false      | true  | null || Null SFSR, but 'PASSED' QA Status results in A. No dictionary entry to update.
        /// |  3 | true  | null | FAILED    | GoodId  | null    | GoodId | true      | null     || A      | true       | false | null || Null SFSR, but 'FAILED' QA Status results in A and updated dictionary.
        /// |  4 | true  | null | FAILED    | GoodId  | null    | BadId  | true      | null     || A      | false      | true  | null || Null SFSR, but 'FAILED' QA Status results in A. No dictionary entry to update.
        /// |  5 | true  | null | UNCERTAIN | GoodId  | null    | GoodId | true      | null     || A      | true       | false | null || Null SFSR, but 'UNCERTAIN' QA Status results in A and updated dictionary.
        /// |  6 | true  | null | UNCERTAIN | GoodId  | null    | BadId  | true      | null     || A      | false      | true  | null || Null SFSR, but 'UNCERTAIN' QA Status results in A. No dictionary entry to update.
        /// |  7 | true  | null | INC       | GoodId  | null    | GoodId | true      | null     || null   | true       | true  | null || Null SFSR and 'INC' QA Status is allowed.
        /// |  8 | true  | null | EXPIRED   | GoodId  | null    | GoodId | true      | null     || null   | true       | true  | null || Null SFSR and 'EXPIRED' QA Status is allowed.
        /// |  9 | true  | null | LOST      | GoodId  | null    | GoodId | true      | null     || null   | true       | true  | null || Null SFSR and 'LOST' QA Status is allowed.
        /// | 10 | true  | null | PASSED    | GoodId  | 1       | GoodId | true      | null     || null   | true       | true  | null || Null SFSR and 'PASSED' QA Status for RATA is allowed.
        /// | 11 | true  | null | FAILED    | GoodId  | 1       | GoodId | true      | null     || null   | true       | true  | null || Null SFSR and 'FAILED' QA Status for RATA is allowed.
        /// | 12 | true  | null | UNCERTAIN | GoodId  | 1       | GoodId | true      | null     || null   | true       | true  | null || Null SFSR and 'UNCERTAIN' QA Status for RATA is allowed.
        /// | 13 | true  | 0.01 | INC       | GoodId  | null    | GoodId | true      | null     || B      | true       | false | null || Valued SFSR, but 'INC' QA Status results in B and updated dictionary.
        /// | 14 | true  | 0.01 | INC       | GoodId  | null    | BadId  | true      | null     || B      | false      | true  | null || Valued SFSR, but 'INC' QA Status results in B. No dictionary entry to update.
        /// | 15 | true  | 0.01 | EXPIRED   | GoodId  | null    | GoodId | true      | null     || B      | true       | false | null || Valued SFSR, but 'EXPIRED' QA Status results in B and updated dictionary.
        /// | 16 | true  | 0.01 | EXPIRED   | GoodId  | null    | BadId  | true      | null     || B      | false      | true  | null || Valued SFSR, but 'EXPIRED' QA Status results in B. No dictionary entry to update.
        /// | 17 | true  | 0.01 | LOST      | GoodId  | null    | GoodId | true      | null     || B      | true       | false | null || Valued SFSR, but 'LOST' QA Status results in B and updated dictionary.
        /// | 18 | true  | 0.01 | LOST      | GoodId  | null    | BadId  | true      | null     || B      | false      | true  | null || Valued SFSR, but 'LOST' QA Status results in B. No dictionary entry to update.
        /// | 19 | true  | 0.01 | PASSED    | GoodId  | null    | GoodId | true      | null     || C      | true       | false | null || SFSR with excess decimals and 'PASSED' QA Status results in C and updated dictionary.
        /// | 20 | true  | 0.01 | PASSED    | GoodId  | null    | BadId  | true      | null     || C      | false      | true  | null || SFSR with excess decimals and 'PASSED' QA Status results in C. No dictionary entry to update.
        /// | 21 | true  | 0.01 | FAILED    | GoodId  | null    | GoodId | true      | null     || C      | true       | false | null || SFSR with excess decimals and 'FAILED' QA Status results in C and updated dictionary.
        /// | 22 | true  | 0.01 | FAILED    | GoodId  | null    | BadId  | true      | null     || C      | false      | true  | null || SFSR with excess decimals and 'FAILED' QA Status results in C. No dictionary entry to update.
        /// | 23 | true  | 0.01 | UNCERTAIN | GoodId  | null    | GoodId | true      | null     || C      | true       | false | null || SFSR with excess decimals and 'UNCERTAIN' QA Status results in C and updated dictionary.
        /// | 24 | true  | 0.01 | UNCERTAIN | GoodId  | null    | BadId  | true      | null     || C      | false      | true  | null || SFSR with excess decimals and 'UNCERTAIN' QA Status results in C. No dictionary entry to update.
        /// | 25 | true  | 0.1  | PASSED    | GoodId  | null    | GoodId | true      | null     || null   | true       | true  | 0.1  || Valid SFSR and 'PASSED' QA Status is allowed. Dictionary SFSR is updated.
        /// | 26 | true  | 0.1  | PASSED    | GoodId  | null    | BadId  | true      | null     || null   | false      | true  | null || Valid SFSR and 'PASSED' QA Status is allowed. No dictionary entry to update.
        /// | 27 | true  | 0.1  | FAILED    | GoodId  | null    | GoodId | true      | null     || null   | true       | true  | 0.1  || Valid SFSR and 'FAILED' QA Status is allowed. Dictionary SFSR is updated.
        /// | 28 | true  | 0.1  | FAILED    | GoodId  | null    | BadId  | true      | null     || null   | false      | true  | null || Valid SFSR and 'FAILED' QA Status is allowed. No dictionary entry to update.
        /// | 29 | true  | 0.1  | UNCERTAIN | GoodId  | null    | GoodId | true      | null     || null   | true       | true  | 0.1  || Valid SFSR and 'UNCERTAIN' QA Status is allowed. Dictionary SFSR is updated.
        /// | 30 | true  | 0.1  | UNCERTAIN | GoodId  | null    | BadId  | true      | null     || null   | false      | true  | null || Valid SFSR and 'UNCERTAIN' QA Status is allowed. No dictionary entry to update.
        /// | 31 | true  | 0.1  | PASSED    | GoodId  | null    | GoodId | false     | 12.3     || null   | true       | false | 0.1  || Valid SFSR and  'PASSED' QA Status is allowed. Dictionary SFSR is updated, but dictionary valid remains false.
        /// | 32 | true  | 0.1  | FAILED    | GoodId  | null    | GoodId | false     | 12.3     || null   | true       | false | 0.1  || Valid SFSR and  'FAILED' QA Status is allowed. Dictionary SFSR is updated, but dictionary valid remains false.
        /// | 33 | true  | 0.1  | UNCERTAIN | GoodId  | null    | GoodId | false     | 12.3     || null   | true       | false | 0.1  || Valid SFSR and  'UNCERTAIN' QA Status is allowed. Dictionary SFSR is updated, but dictionary valid remains false.
        /// | 34 | true  | null | PASSED    | GoodId  | 0       | GoodId | true      | null     || A      | true       | false | null || Null SFSR and not RATA, but 'PASSED' QA Status results in A and updated dictionary.
        /// </summary>
        [TestMethod()]
        public void MatsTrn9()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            /* Input Parameter Values */
            string goodId = "GoodId";
            string badId = "BadId";

            bool?[] validList = { false, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true };
            //TODO: Currently the spec and code disagree about whether the value should be rounded to 1 or two decimal places.  Changed to agree with the existing code for now.
            decimal?[] sfsrList = { null, null, null, null, null, null, null, null, null, null, null, null, null, 0.001m, 0.001m, 0.001m, 0.001m, 0.001m, 0.001m, 0.001m, 0.001m, 0.001m, 0.001m, 0.001m, 0.001m, 0.01m, 0.01m, 0.01m, 0.01m, 0.01m, 0.01m, 0.01m, 0.01m, 0.01m, null };
            string[] qaStatusList = { "BAD", "PASSED", "PASSED", "FAILED", "FAILED", "UNCERTAIN", "UNCERTAIN", "INC", "EXPIRED", "LOST", "PASSED", "FAILED", "UNCERTAIN", "INC", "INC", "EXPIRED", "EXPIRED", "LOST", "LOST", "PASSED", "PASSED", "FAILED", "FAILED", "UNCERTAIN", "UNCERTAIN", "PASSED", "PASSED", "FAILED", "FAILED", "UNCERTAIN", "UNCERTAIN", "PASSED", "FAILED", "UNCERTAIN", "PASSED" };
            int?[] rataList = { null, null, null, null, null, null, null, null, null, null, 1, 1, 1, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, 0 };
            string[] dictTrainIdList = { goodId, goodId, badId, goodId, badId, goodId, badId, goodId, goodId, goodId, goodId, goodId, goodId, goodId, badId, goodId, badId, goodId, badId, goodId, badId, goodId, badId, goodId, badId, goodId, badId, goodId, badId, goodId, badId, goodId, goodId, goodId, goodId };
            bool?[] dictValidList = { true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, false, false, false, true };
            decimal?[] dictSfsrList = { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, 12.3m, 12.3m, 12.3m, null };

            /* Expected Values */
            string[] expResultList = { null, "A", "A", "A", "A", "A", "A", null, null, null, null, null, null, "B", "B", "B", "B", "B", "B", "C", "C", "C", "C", "C", "C", null, null, null, null, null, null, null, null, null, "A" };
            bool?[] expDictExists = { true, true, false, true, false, true, false, true, true, true, true, true, true, true, false, true, false, true, false, true, false, true, false, true, false, true, false, true, false, true, false, true, true, true, true };
            bool?[] expDictValid = { true, false, true, false, true, false, true, true, true, true, true, true, true, false, true, false, true, false, true, false, true, false, true, false, true, true, true, true, true, true, true, false, false, false, false };
            //TODO: Currently the spec and code disagree about whether the value should be rounded to 1 or two decimal places.  Changed to agree with the existing code for now.
            decimal?[] expDictSfsr = { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, 0.01m, null, 0.01m, null, 0.01m, null, 0.01m, 0.01m, 0.01m, null };

            /* Test Case Count */
            int caseCount = 35;

            /* Check array lengths */
            Assert.AreEqual(caseCount, validList.Length, "validList length");
            Assert.AreEqual(caseCount, sfsrList.Length, "sfsrList length");
            Assert.AreEqual(caseCount, qaStatusList.Length, "qaStatusList length");
            Assert.AreEqual(caseCount, rataList.Length, "apsList length");
            Assert.AreEqual(caseCount, dictTrainIdList.Length, "dictTrainIdList length");
            Assert.AreEqual(caseCount, dictValidList.Length, "dictValidList length");
            Assert.AreEqual(caseCount, dictSfsrList.Length, "dictSfsrList length");
            Assert.AreEqual(caseCount, expResultList.Length, "expResultList length");
            Assert.AreEqual(caseCount, expDictExists.Length, "expDictExists length");
            Assert.AreEqual(caseCount, expDictValid.Length, "expDictValid length");
            Assert.AreEqual(caseCount, expDictSfsr.Length, "expDictSfsr length");


            /* Run Cases */
            for (int caseDex = 0; caseDex < caseCount; caseDex++)
            {
                /* Initialize Input Parameters */
                EmParameters.MatsSamplingTrainQaStatusCodeValid = validList[caseDex];
                EmParameters.MatsSamplingTrainRecord = new MatsSamplingTrainRecord(trainQaStatusCd: qaStatusList[caseDex],
                                                                                   refFlowToSamplingRatio: sfsrList[caseDex],
                                                                                   rataInd: rataList[caseDex],
                                                                                   trapTrainId: goodId);

                /* Initialize Updatable Parameters */
                EmParameters.MatsSamplingTrainDictionary = new Dictionary<string, SamplingTrainEvalInformation>();
                {
                    SamplingTrainEvalInformation samplingTrainEvalInformation = new SamplingTrainEvalInformation("TrainId");
                    {
                        samplingTrainEvalInformation.SamplingTrainValid = dictValidList[caseDex];
                        samplingTrainEvalInformation.ReferenceSFSRRatio = dictSfsrList[caseDex];

                        EmParameters.MatsSamplingTrainDictionary[dictTrainIdList[caseDex]] = samplingTrainEvalInformation;
                    }
                }


                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Initialize variables needed to run the check. */
                bool log = false;
                string actual;

                /* Run Check */
                actual = cMATSSamplingTrainChecks.MatsTrn9(category, ref log);


                /* Check Results */
                Assert.AreEqual(string.Empty, actual, string.Format("actual[case {0}]", caseDex));
                Assert.AreEqual(false, log, string.Format("log [case {0}]", caseDex));
                Assert.AreEqual(expResultList[caseDex], category.CheckCatalogResult, string.Format("category.CheckCatalogResult [case {0}]", caseDex));

                Assert.AreEqual(expDictExists[caseDex], EmParameters.MatsSamplingTrainDictionary.ContainsKey(goodId), string.Format("MatsSamplingTrainDictionary [case {0}]", caseDex));
                Assert.AreEqual(expDictValid[caseDex], EmParameters.MatsSamplingTrainDictionary[dictTrainIdList[caseDex]].SamplingTrainValid, string.Format("SamplingTrainValid [case {0}]", caseDex));
                Assert.AreEqual(expDictSfsr[caseDex], EmParameters.MatsSamplingTrainDictionary[dictTrainIdList[caseDex]].ReferenceSFSRRatio, string.Format("ReferenceSFSRRatio [case {0}]", caseDex));
            }
        }

        /// <summary>
        ///A test for MatsTrn10
        ///</summary>()
        [TestMethod()]
        public void MatsTrn10()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            /* Initialize variables needed to run the check. */
            bool log = false;
            string actual;

            /* Initialize General Variables */
            string[] trainQaStatusList = { "EXPIRED", "FAILED", "INC", "LOST", "PASSED", "UNCERTAIN" };
            bool?[] validList = { null, false, true };
            string[] valueTypeList = { null, "PASSED", "FAILED", "OTHER" };

            /* Initialize MatsSamplingTrainDictionary */
            EmParameters.MatsSamplingTrainDictionary = new Dictionary<string, SamplingTrainEvalInformation>();
            EmParameters.MatsSamplingTrainDictionary["TrapTrainId"] = new SamplingTrainEvalInformation("TrapTrainId");

            foreach (bool? valid in validList)
                foreach (string trainQaStatusCd in trainQaStatusList)
                    foreach (string value in valueTypeList)
                        foreach (bool? componentIdValid in validList)
                        {
                            /*  Initialize Input Parameters*/
                            EmParameters.MatsSamplingTrainComponentIdValid = componentIdValid;
                            EmParameters.MatsSamplingTrainQaStatusCodeValid = valid;
                            EmParameters.MatsSamplingTrainRecord = new MatsSamplingTrainRecord(trapTrainId: "TrapTrainId", trainQaStatusCd: trainQaStatusCd, samplingRatioTestResultCd: value);

                            /* Initialize Updatable Parameters */
                            EmParameters.MatsSamplingTrainDictionary["TrapTrainId"].SamplingTrainValid = null;

                            /* Expected Results */
                            string result = null;
                            bool? expSamplingTrainValid = null;

                            if (valid.Default(false))
                            {
                                switch (value)
                                {
                                    case "PASSED": { if (trainQaStatusCd.NotInList("PASSED,FAILED,UNCERTAIN")) result = "B"; } break;
                                    case "FAILED": { if (trainQaStatusCd != "FAILED") result = "C"; } break;
                                    case null: { if (trainQaStatusCd.NotInList("INC,EXPIRED,LOST")) result = "A"; } break;
                                    default: result = "D"; break;
                                }

                                if (componentIdValid.Default(false))
                                {
                                    if (result != null)
                                        expSamplingTrainValid = false;
                                }
                            }

                            /* Init Cateogry Result */
                            category.CheckCatalogResult = null;

                            // Run Checks
                            actual = cMATSSamplingTrainChecks.MatsTrn10(category, ref log);

                            /* Check Result Label */
                            string resultPrefix = string.Format("[valid: {0}, trainQaStatusCd: {1}, valueType: {2}]", valid, trainQaStatusCd, value);

                            // Check Results
                            Assert.AreEqual(string.Empty, actual, resultPrefix + ".Result");
                            Assert.AreEqual(false, log, resultPrefix + ".log");
                            Assert.AreEqual(result, category.CheckCatalogResult, resultPrefix + ".CheckResult");

                            Assert.AreEqual(expSamplingTrainValid, EmParameters.MatsSamplingTrainDictionary["TrapTrainId"].SamplingTrainValid, resultPrefix + ".MatsSamplingTrainDictionary.SamplingTrainValid");
                        }
        }

        /// <summary>
        ///A test for MatsTrn11
        ///</summary>()
        [TestMethod()]
        public void MatsTrn11()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            /* Initialize variables needed to run the check. */
            bool log = false;
            string actual;

            /* Initialize General Variables */
            string[] trainQaStatusList = { "EXPIRED", "FAILED", "INC", "LOST", "PASSED", "UNCERTAIN" };
            bool?[] validList = { null, false, true };
            string[] valueTypeList = { null, "PASSED", "FAILED", "OTHER" };

            foreach (bool? valid in validList)
                foreach (string trainQaStatusCd in trainQaStatusList)
                    foreach (string value in valueTypeList)
                    {
                        /*  Initialize Input Parameters*/
                        EmParameters.MatsSamplingTrainQaStatusCodeValid = valid;
                        EmParameters.MatsSamplingTrainRecord = new MatsSamplingTrainRecord(trainQaStatusCd: trainQaStatusCd, postLeakTestResultCd: value);

                        /* Expected Results */
                        string result = null;

                        if (valid.Default(false))
                        {
                            switch (value)
                            {
                                case "PASSED": { if (trainQaStatusCd.NotInList("PASSED,FAILED,UNCERTAIN")) result = "B"; } break;
                                case "FAILED": { if (trainQaStatusCd != "FAILED") result = "C"; } break;
                                case null: { if (trainQaStatusCd.NotInList("INC,EXPIRED,LOST")) result = "A"; } break;
                                default: result = "D"; break;
                            }
                        }

                        /* Init Cateogry Result */
                        category.CheckCatalogResult = null;

                        // Run Checks
                        actual = cMATSSamplingTrainChecks.MatsTrn11(category, ref log);

                        /* Check Result Label */
                        string resultPrefix = string.Format("[valid: {0}, trainQaStatusCd: {1}, valueType: {2}]", valid, trainQaStatusCd, value);

                        // Check Results
                        Assert.AreEqual(string.Empty, actual);
                        Assert.AreEqual(false, log);
                        Assert.AreEqual(result, category.CheckCatalogResult, resultPrefix + ".Result");
                    }
        }

        /// <summary>
        ///A test for MatsTrn12
        ///</summary>()
        [TestMethod()]
        public void MatsTrn12()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            /* Initialize variables needed to run the check. */
            bool log = false;
            string actual;

            /* Initialize General Variables */
            string[] trainQaStatusList = { "EXPIRED", "FAILED", "INC", "LOST", "PASSED", "UNCERTAIN" };
            bool?[] validList = { null, false, true };
            string[] valueTypeList = { null, "I accidently dropped it ... on purpose" };

            foreach (bool? valid in validList)
                foreach (string trainQaStatusCd in trainQaStatusList)
                    foreach (string value in valueTypeList)
                    {
                        /*  Initialize Input Parameters*/
                        EmParameters.MatsSamplingTrainQaStatusCodeValid = valid;
                        EmParameters.MatsSamplingTrainRecord = new MatsSamplingTrainRecord(trainQaStatusCd: trainQaStatusCd, sampleDamageExplanation: value);

                        /* Expected Results */
                        string result = null;

                        if (valid.Default(false))
                        {
                            if ((value == null) && (trainQaStatusCd == "LOST"))
                                result = "A";
                        }

                        /* Init Cateogry Result */
                        category.CheckCatalogResult = null;

                        // Run Checks
                        actual = cMATSSamplingTrainChecks.MatsTrn12(category, ref log);

                        /* Check Result Label */
                        string resultPrefix = string.Format("[valid: {0}, trainQaStatusCd: {1}, valueType: {2}]", valid, trainQaStatusCd, value);

                        // Check Results
                        Assert.AreEqual(string.Empty, actual);
                        Assert.AreEqual(false, log);
                        Assert.AreEqual(result, category.CheckCatalogResult, resultPrefix + ".Result");
                    }
        }

        /// <summary>
        ///A test for MatsTrn13
        ///</summary>()
        [TestMethod()]
        public void MatsTrn13()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            /* Initialize variables needed to run the check. */
            bool log = false;
            string actual;

            /* Initialize General Variables */
            string mainHg = "4.44E-1";
            string breakthroughHg = "1.11E-1";
            decimal totalSampleVolume = 3m;
            decimal calculatedHg = (0.444m + 0.111m) / totalSampleVolume;
            string[] hgTypeList = new string[] { null, "LessThan", "EqualTo", "GreaterThan", "NotScientific", "NotSignificant" };
            string[] invalidFactorList = new string[] { null, "Main", "Breakthrough", "TotalSampleVolume" };
            string[] trainQaStatusList = { "EXPIRED", "FAILED", "INC", "LOST", "PASSED", "UNCERTAIN" };
            bool?[] validList = { null, false, true };
            string[] modcList = { "01", "43", "44" };

            /* Initialize MatsSamplingTrainDictionary */
            EmParameters.MatsSamplingTrainDictionary = new Dictionary<string, SamplingTrainEvalInformation>();
            EmParameters.MatsSamplingTrainDictionary["ComponentId"] = new SamplingTrainEvalInformation("ComponentId");

            foreach (bool? valid in validList)
                foreach (string trainQaStatusCd in trainQaStatusList)
                    foreach (string hgType in hgTypeList)
                        foreach (string invalidFactor in invalidFactorList)
                            foreach (bool? componentIdValid in validList)
                                foreach (string modcNo in modcList)
                                {
                                    //for
                                    /* Initialize Variables */
                                    string hgConcentration;
                                    {
                                        switch (hgType)
                                        {
                                            case "LessThan": hgConcentration = "1.84E-1"; break;
                                            case "EqualTo": hgConcentration = "1.85E-1"; break;
                                            case "GreaterThan": hgConcentration = "1.86E-1"; break;
                                            case "NotScientific": hgConcentration = "0.185"; break;
                                            case "NotSignificant": hgConcentration = "1.850E-1"; break;
                                            default: hgConcentration = null; break;
                                        }
                                    }

                                    /*  Initialize Input Parameters*/
                                    EmParameters.MatsBtTrapHgValid = (invalidFactor != "Breakthrough");
                                    EmParameters.MatsMainTrapHgValid = (invalidFactor != "Main");
                                    EmParameters.MatsSamplingTrainComponentIdValid = componentIdValid;
                                    EmParameters.MatsSamplingTrainQaStatusCodeValid = valid;
                                    EmParameters.MatsSamplingTrainRecord
                                      = new MatsSamplingTrainRecord(componentId: "ComponentId", trainQaStatusCd: trainQaStatusCd, hgConcentration: hgConcentration,
                                                                     mainTrapHg: mainHg, breakthroughTrapHg: breakthroughHg, totalSampleVolume: totalSampleVolume,
                                                                     trapModcCd: modcNo.ToString(), trapTrainId: "ComponentId");
                                    EmParameters.MatsTotalSampleVolumeDscmValid = (invalidFactor != "TotalSampleVolume");

                                    /* Initialize Updatable Parameters */
                                    EmParameters.MatsSamplingTrainDictionary["ComponentId"].HgConcentration = null;
                                    EmParameters.MatsSamplingTrainDictionary["ComponentId"].SamplingTrainValid = null;

                                    /* Expected Results */
                                    string result = null;
                                    decimal? expHgConcentration = null;
                                    bool? expSamplingTrainValid = null;

                                    if (valid.Default(false))
                                    {
                                        if (hgType == null)
                                        {
                                            if (trainQaStatusCd.NotInList("INC,EXPIRED,LOST"))
                                                result = "A";
                                        }
                                        else
                                        {
                                            if (trainQaStatusCd.NotInList("PASSED,FAILED,UNCERTAIN"))
                                                result = "B";
                                            else if (hgType.InList("NotScientific,NotSignificant"))
                                                result = "C";
                                            else if (modcNo.InList("43,44"))
                                            {
                                                if (componentIdValid.Default(false))
                                                    expHgConcentration = hgConcentration.ScientificNotationtoDecimal();
                                            }
                                            else if (invalidFactor == null)
                                            {
                                                if (componentIdValid.Default(false))
                                                    expHgConcentration = calculatedHg;

                                                if (hgType.InList("LessThan,GreaterThan"))
                                                    result = "D";
                                            }
                                            else
                                            {
                                                if (componentIdValid.Default(false))
                                                    expSamplingTrainValid = false;
                                            }
                                        }

                                        if (componentIdValid.Default(false))
                                        {
                                            if (result != null)
                                                expSamplingTrainValid = false;
                                        }
                                    }

                                    /* Init Cateogry Result */
                                    category.CheckCatalogResult = null;

                                    // Run Checks
                                    actual = cMATSSamplingTrainChecks.MatsTrn13(category, ref log);

                                    /* Check Result Label */
                                    string resultPrefix = string.Format("[valid: {0}, trainQaStatusCd: {1}, hgType: {2}, invalidFactor: {3}, componentIdValid: {4}]", valid, trainQaStatusCd, hgType, invalidFactor, componentIdValid);

                                    // Check Results
                                    Assert.AreEqual(string.Empty, actual);
                                    Assert.AreEqual(false, log);
                                    Assert.AreEqual(result, category.CheckCatalogResult, resultPrefix + ".Result");

                                    Assert.AreEqual(expHgConcentration, EmParameters.MatsSamplingTrainDictionary["ComponentId"].HgConcentration, resultPrefix + ".MatsSamplingTrainDictionary.HgConcentration");
                                    Assert.AreEqual(expSamplingTrainValid, EmParameters.MatsSamplingTrainDictionary["ComponentId"].SamplingTrainValid, resultPrefix + ".MatsSamplingTrainDictionary.SamplingTrainValid");
                                }
        }

        /// <summary>
        /// MatsTrn14
        /// 
        /// QS Valid : MatsSamplingTrainQaStatusCodeValid
        /// MT Valid : MatsMainTrapHgValid 
        /// BT Valid : MatsBtTrapHgValid 
        /// QS Code  : MatsSamplingTrainRecord.TrainQAStatusCode
        /// % BT     : MatsSamplingTrainRecord.PercentBreakthrough
        /// BT Val   : MatsSamplingTrainRecord.BTTrapHg
        /// MT Val   : MatsSamplingTrainRecord.MainTrapHg
        /// Hg Conc  : MatsSamplingTrainRecord.HgConcentration
        /// Calc BT  : MatsCalcTrainPercentBreakthrough 
        /// 
        /// | ## | QS Valid | MT Valid | BT Valid | QS Code   | APS  | % BT  | BT Val | MT Val | Hg Conc || Result | Calc BT || Note
        /// |  0 | null     | true     | true     | PASSED    | null | null  | null   | 0.200  | 2.00E-1 || null   | null    || MatsSamplingTrainQaStatusCodeValid is null
        /// |  1 | false    | true     | true     | PASSED    | null | null  | null   | 0.200  | 2.00E-1 || null   | null    || MatsSamplingTrainQaStatusCodeValid is false
        /// |  2 | true     | true     | true     | PASSED    | null | null  | null   | 0.200  | 2.00E-1 || A      | null    || %BT is null and result A conditions met.
        /// |  3 | true     | true     | true     | FAILED    | null | null  | null   | 0.200  | 2.00E-1 || A      | null    || %BT is null and result A conditions met.
        /// |  4 | true     | true     | true     | UNCERTAIN | null | null  | null   | 0.200  | 2.00E-1 || A      | null    || %BT is null and result A conditions met.
        /// |  5 | true     | true     | true     | PASSED    | null | null  | null   | 0.200  | 2.01E-1 || A      | null    || %BT is null and result A conditions met.
        /// |  6 | true     | true     | true     | FAILED    | null | null  | null   | 0.200  | 2.99E-1 || A      | null    || %BT is null and result A conditions met.
        /// |  7 | true     | true     | true     | UNCERTAIN | null | null  | null   | 0.200  | 3.00E-1 || A      | null    || %BT is null and result A conditions met.
        /// |  8 | true     | true     | true     | PASSED    | null | null  | null   | 0.200  | 1.99E-1 || null   | null    || %BT is null and Hg condition not met.
        /// |  9 | true     | true     | true     | FAILED    | null | null  | null   | 0.200  | 1.99E-1 || null   | null    || %BT is null and Hg condition not met.
        /// | 10 | true     | true     | true     | UNCERTAIN | null | null  | null   | 0.200  | 1.99E-1 || null   | null    || %BT is null and Hg condition not met.
        /// | 11 | true     | true     | true     | LOST      | null | null  | null   | 0.200  | 2.00E-1 || null   | null    || %BT is null and status condition not met.
        /// | 12 | true     | true     | true     | EXPIRED   | null | null  | null   | 0.200  | 2.00E-1 || null   | null    || %BT is null and status condition not met.
        /// | 13 | true     | true     | true     | INC       | null | null  | null   | 0.200  | 2.00E-1 || null   | null    || %BT is null and status condition not met.
        /// | 14 | true     | true     | true     | PASSED    | null | 10.55 | 0.0211 | 0.200  | 2.00E-1 || C      | null    || %BT is not null and %BT rounding condtion not met.
        /// | 15 | true     | true     | true     | FAILED    | null | 10.55 | 0.0211 | 0.200  | 2.00E-1 || C      | null    || %BT is not null and %BT rounding condtion not met.
        /// | 16 | true     | true     | true     | UNCERTAIN | null | 10.55 | 0.0211 | 0.200  | 2.00E-1 || C      | null    || %BT is not null and %BT rounding condtion not met.
        /// | 17 | true     | true     | true     | LOST      | null | 10.55 | 0.0211 | 0.200  | 2.00E-1 || B      | null    || %BT is not null and status condtion not met.
        /// | 18 | true     | true     | true     | EXPIRED   | null | 10.55 | 0.0211 | 0.200  | 2.00E-1 || B      | null    || %BT is not null and status condtion not met.
        /// | 19 | true     | true     | true     | INC       | null | 10.55 | 0.0211 | 0.200  | 2.00E-1 || B      | null    || %BT is not null and status condtion not met.
        /// | 20 | true     | true     | true     | PASSED    | null | 10.5  | 0.0210 | 0.200  | 2.00E-1 || E      | 10.5    || %BT is not null, %BT rounding condtion met, and Hg less than 0.2.
        /// | 21 | true     | true     | true     | FAILED    | null | 10.5  | 0.0210 | 0.200  | 2.00E-1 || null   | 10.5    || %BT is not null, %BT rounding condtion met, and Hg less than 0.2, but FAILED status prevents result E.
        /// | 22 | true     | true     | true     | UNCERTAIN | null | 10.5  | 0.0210 | 0.200  | 2.00E-1 || E      | 10.5    || %BT is not null, %BT rounding condtion met, and Hg less than 0.2.
        /// | 23 | true     | true     | true     | LOST      | null | 10.5  | 0.0210 | 0.200  | 2.00E-1 || B      | null    || %BT is not null, %BT rounding condtion met, and status condtion not met.
        /// | 24 | true     | true     | true     | EXPIRED   | null | 10.5  | 0.0210 | 0.200  | 2.00E-1 || B      | null    || %BT is not null, %BT rounding condtion met, and status condtion not met.
        /// | 25 | true     | true     | true     | INC       | null | 10.5  | 0.0210 | 0.200  | 2.00E-1 || B      | null    || %BT is not null, %BT rounding condtion met, and status condtion not met.
        /// | 26 | true     | true     | true     | PASSED    | null | 10.5  | 0.0200 | 0.200  | 2.00E-1 || D      | 10.0    || %BT is not null, result D conditions met, and Main and BT values are valid.
        /// | 27 | true     | true     | false    | PASSED    | null | 10.5  | 0.0200 | 0.200  | 2.00E-1 || null   | null    || %BT is not null, result D conditions met, and Main valid but BT invalid.
        /// | 28 | true     | true     | null     | PASSED    | null | 10.5  | 0.0200 | 0.200  | 2.00E-1 || null   | null    || %BT is not null, result D conditions met, and Main valid but BT invalid.
        /// | 29 | true     | false    | true     | PASSED    | null | 10.5  | 0.0200 | 0.200  | 2.00E-1 || null   | null    || %BT is not null, result D conditions met, and BT valid but Main invalid.
        /// | 30 | true     | null     | true     | PASSED    | null | 10.5  | 0.0200 | 0.200  | 2.00E-1 || null   | null    || %BT is not null, result D conditions met, and BT valid but Main invalid.
        /// | 31 | true     | false    | false    | PASSED    | null | 10.5  | 0.0200 | 0.200  | 2.00E-1 || null   | null    || %BT is not null, result D conditions met, and Main and BT values are invalid.
        /// | 32 | true     | true     | true     | PASSED    | null | 10.5  | 0.0210 | 0.200  | 2.00E-1 || E      | 10.5    || %BT is not null, result E conditions met, and Main and BT values are valid.
        /// | 33 | true     | true     | false    | PASSED    | null | 10.5  | 0.0210 | 0.200  | 2.00E-1 || null   | null    || %BT is not null, result E conditions met, and Main valid but BT invalid.
        /// | 34 | true     | true     | null     | PASSED    | null | 10.5  | 0.0210 | 0.200  | 2.00E-1 || null   | null    || %BT is not null, result E conditions met, and Main valid but BT invalid.
        /// | 35 | true     | false    | true     | PASSED    | null | 10.5  | 0.0210 | 0.200  | 2.00E-1 || null   | null    || %BT is not null, result E conditions met, and Main and BT values are invalid.
        /// | 36 | true     | null     | true     | PASSED    | null | 10.5  | 0.0210 | 0.200  | 2.00E-1 || null   | null    || %BT is not null, result E conditions met, and Main and BT values are invalid.
        /// | 37 | true     | false    | false    | PASSED    | null | 10.5  | 0.0210 | 0.200  | 2.00E-1 || null   | null    || %BT is not null, result E conditions met, and Main and BT values are invalid.
        /// | 38 | true     | true     | true     | PASSED    | null | 10.5  | 0.0210 | 0.200  | 1.99E-1 || null   | 10.5    || %BT is not null, result E conditions met, but Hg is less than 0.2.
        /// | 39 | true     | true     | true     | FAILED    | null | 10.5  | 0.0210 | 0.200  | 2.00E-1 || null   | 10.5    || %BT is not null, result E conditions met, but Status is FAILED.
        /// | 40 | true     | true     | true     | PASSED    | null | 10.5  | 0.0210 | 0.200  | 5.00E-1 || E      | 10.5    || %BT is not null, %BT equals 10.5 (rounds to 10) and Hg is 0.5 so result E condition not met.
        /// | 41 | true     | true     | true     | PASSED    | null | 10.5  | 0.0210 | 0.200  | 5.01E-1 || E      | 10.5    || %BT is not null, %BT equals 10.5 (rounds to 10) and Hg is 0.501 so result E condition met.
        /// | 42 | true     | true     | true     | PASSED    | null | 10.4  | 0.0208 | 0.200  | 5.00E-1 || null   | 10.4    || %BT is not null, %BT equals 10.5 (rounds to 10) and Hg is 0.5 so result E condition not met.
        /// | 43 | true     | true     | true     | PASSED    | null | 10.4  | 0.0208 | 0.200  | 5.01E-1 || E      | 10.4    || %BT is not null, %BT equals 10.5 (rounds to 10) and Hg is 0.501 so result E condition met.
        /// | 44 | true     | true     | true     | PASSED    | null |  5.5  | 0.0110 | 0.200  | 5.00E-1 || null   |  5.5    || %BT is not null, %BT equals 5.6 (rounds to 6) and Hg is 0.5 so result E condition not met.
        /// | 45 | true     | true     | true     | PASSED    | null |  5.5  | 0.0110 | 0.200  | 5.01E-1 || E      |  5.5    || %BT is not null, %BT equals 5.6 (rounds to 6) and Hg is 0.501 so result E condition met.
        /// | 46 | true     | true     | true     | PASSED    | null |  5.4  | 0.0108 | 0.200  | 5.00E-1 || null   |  5.4    || %BT is not null, %BT equals 5.5 (rounds to 5) and Hg is 0.5 so result E condition not met.
        /// | 47 | true     | true     | true     | PASSED    | null |  5.4  | 0.0108 | 0.200  | 5.01E-1 || null   |  5.4    || %BT is not null, %BT equals 5.5 (rounds to 5) and Hg is 0.501 so result E condition met.
        /// | 48 | true     | true     | true     | PASSED    | RATA | 10.5  | 0.0210 | 0.200  | 1.01E0  || F      | 10.5    || RATA APS with %BT equals 10.5 (rounds to 11), Hg is 1.01, and Status is PASSED so result F condition met.
        /// | 49 | true     | true     | true     | UNCERTAIN | RATA | 10.5  | 0.0210 | 0.200  | 1.01E0  || F      | 10.5    || RATA APS with %BT equals 10.5 (rounds to 11), Hg is 1.01, and Status is UNCERTAIN so result F condition met.
        /// | 50 | true     | true     | true     | FAILED    | RATA | 10.5  | 0.0210 | 0.200  | 1.01E0  || null   | 10.5    || RATA APS with %BT equals 10.5 (rounds to 11), Hg is 1.01, but Status is FAILED so result F condition not met.
        /// | 51 | true     | true     | true     | PASSED    | RATA | 10.4  | 0.0208 | 0.200  | 1.01E0  || null   | 10.4    || RATA APS withHg is 1.01, Status is PASSED, but %BT equals 10.4 (rounds to 10) so result F condition not met.
        /// | 52 | true     | true     | true     | PASSED    | RATA | 10.5  | 0.0210 | 0.200  | 1.00E0  || null   | 10.5    || RATA APS with %BT equals 10.5 (rounds to 21), Status is PASSED, but Hg is 1.00 so result F condition not met.
        /// | 53 | true     | true     | true     | PASSED    | RATA | 20.5  | 0.0410 | 0.200  | 5.01E-1 || F      | 20.5    || RATA APS with %BT equals 20.5 (rounds to 21), Hg is 0.501, and Status is PASSED so result F condition met.
        /// | 54 | true     | true     | true     | UNCERTAIN | RATA | 20.5  | 0.0410 | 0.200  | 5.01E-1 || F      | 20.5    || RATA APS with %BT equals 20.5 (rounds to 21), Hg is 0.501, and Status is UNCERTAIN so result F condition met.
        /// | 55 | true     | true     | true     | FAILED    | RATA | 20.5  | 0.0410 | 0.200  | 5.01E-1 || null   | 20.5    || RATA APS with %BT equals 20.5 (rounds to 21), Hg is 0.501, but Status is FAILED so result F condition not met.
        /// | 56 | true     | true     | true     | PASSED    | RATA | 20.4  | 0.0408 | 0.200  | 5.01E-1 || null   | 20.4    || RATA APS withHg is 0.501, Status is PASSED, but %BT equals 20.4 (rounds to 20) so result F condition not met.
        /// | 57 | true     | true     | true     | PASSED    | RATA | 20.5  | 0.0410 | 0.200  | 5.00E-1 || null   | 20.5    || RATA APS with %BT equals 20.5 (rounds to 21), Status is PASSED, but Hg is 0.500 so result F condition not met.
        /// | 58 | true     | true     | true     | PASSED    | RATA | 50.5  | 0.101  | 0.200  | 1.01E-1 || F      | 50.5    || RATA APS with %BT equals 50.5 (rounds to 51), Hg is 0.101, and Status is PASSED so result F condition met.
        /// | 59 | true     | true     | true     | UNCERTAIN | RATA | 50.5  | 0.101  | 0.200  | 1.01E-1 || F      | 50.5    || RATA APS with %BT equals 50.5 (rounds to 51), Hg is 0.101, and Status is UNCERTAIN so result F condition met.
        /// | 60 | true     | true     | true     | FAILED    | RATA | 50.5  | 0.101  | 0.200  | 1.01E-1 || null   | 50.5    || RATA APS with %BT equals 50.5 (rounds to 51), Hg is 0.101, but Status is FAILED so result F condition not met.
        /// | 61 | true     | true     | true     | PASSED    | RATA | 50.0  | 0.100  | 0.200  | 1.01E-1 || null   | 50.0    || RATA APS withHg is 0.1.01, Status is PASSED, but %BT equals 50.4 (rounds to 50) so result F condition not met.
        /// | 62 | true     | true     | true     | PASSED    | RATA | 50.5  | 0.101  | 0.200  | 1.00E-1 || null   | 50.5    || RATA APS with %BT equals 50.5 (rounds to 51), Status is PASSED, but Hg is 0.100 so result F condition not met.
        /// 
        /// </summary>
        [TestMethod()]
        public void MatsTrn14()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            /* Case Count */
            int caseCount = 63;

            /* Input Parameter Values */
            bool?[] qsValidList = { null, false, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true };
            bool?[] mtValidList = { true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, false, null, false, true, true, true, false, null, false, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true };
            bool?[] btValidList = { true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, false, null, true, true, false, true, false, null, true, true, false, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true };
            string[] qaStatusList = { "PASSED", "PASSED", "PASSED", "FAILED", "UNCERTAIN", "PASSED", "FAILED", "UNCERTAIN", "PASSED", "FAILED", "UNCERTAIN", "LOST", "EXPIRED", "INC", "PASSED", "FAILED", "UNCERTAIN", "LOST", "EXPIRED", "INC", "PASSED", "FAILED", "UNCERTAIN", "LOST", "EXPIRED", "INC", "PASSED", "PASSED", "PASSED", "PASSED", "PASSED", "PASSED", "PASSED", "PASSED", "PASSED", "PASSED", "PASSED", "PASSED", "PASSED", "FAILED", "PASSED", "PASSED", "PASSED", "PASSED", "PASSED", "PASSED", "PASSED", "PASSED", "PASSED", "UNCERTAIN", "FAILED", "PASSED", "PASSED", "PASSED", "UNCERTAIN", "FAILED", "PASSED", "PASSED", "PASSED", "UNCERTAIN", "FAILED", "PASSED", "PASSED" };
            string[] apsCodeList = { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, "RATA", "RATA", "RATA", "RATA", "RATA", "RATA", "RATA", "RATA", "RATA", "RATA", "RATA", "RATA", "RATA", "RATA", "RATA" };
            decimal?[] percentBtList = { null, null, null, null, null, null, null, null, null, null, null, null, null, null, 10.55m, 10.55m, 10.55m, 10.55m, 10.55m, 10.55m, 10.5m, 10.5m, 10.5m, 10.5m, 10.5m, 10.5m, 10.5m, 10.5m, 10.5m, 10.5m, 10.5m, 10.5m, 10.5m, 10.5m, 10.5m, 10.5m, 10.5m, 10.5m, 10.5m, 10.5m, 10.5m, 10.5m, 10.4m, 10.4m, 5.5m, 5.5m, 5.4m, 5.4m, 10.5m, 10.5m, 10.5m, 10.4m, 10.5m, 20.5m, 20.5m, 20.5m, 20.4m, 20.5m, 50.5m, 50.5m, 50.5m, 50.0m, 50.5m };
            decimal?[] btValueList = { null, null, null, null, null, null, null, null, null, null, null, null, null, null, 0.0211m, 0.0211m, 0.0211m, 0.0211m, 0.0211m, 0.0211m, 0.0210m, 0.0210m, 0.0210m, 0.0210m, 0.0210m, 0.0210m, 0.02m, 0.02m, 0.02m, 0.02m, 0.02m, 0.02m, 0.0210m, 0.0210m, 0.0210m, 0.0210m, 0.0210m, 0.0210m, 0.0210m, 0.0210m, 0.0210m, 0.0210m, 0.0208m, 0.0208m, 0.0110m, 0.0110m, 0.0108m, 0.0108m, 0.0210m, 0.0210m, 0.0210m, 0.0208m, 0.0210m, 0.0410m, 0.0410m, 0.0410m, 0.0408m, 0.0410m, 0.101m, 0.101m, 0.101m, 0.100m, 0.101m };
            string mtValue = "2.00E-1";
            string[] hgValueList = { "2.00E-1", "2.00E-1", "2.00E-1", "2.00E-1", "2.00E-1", "2.01E-1", "2.99E-1", "3.00E-1", "1.99E-1", "1.99E-1", "1.99E-1", "2.00E-1", "2.00E-1", "2.00E-1", "2.00E-1", "2.00E-1", "2.00E-1", "2.00E-1", "2.00E-1", "2.00E-1", "2.00E-1", "2.00E-1", "2.00E-1", "2.00E-1", "2.00E-1", "2.00E-1", "2.00E-1", "2.00E-1", "2.00E-1", "2.00E-1", "2.00E-1", "2.00E-1", "2.00E-1", "2.00E-1", "2.00E-1", "2.00E-1", "2.00E-1", "2.00E-1", "1.99E-1", "2.00E-1", "5.00E-1", "5.01E-1", "5.00E-1", "5.01E-1", "5.00E-1", "5.01E-1", "5.00E-1", "5.01E-1", "1.01E0", "1.01E0", "1.01E0", "1.01E0", "1.00E0", "5.01E-1", "5.01E-1", "5.01E-1", "5.01E-1", "5.00E-1", "1.01E-1", "1.01E-1", "1.01E-1", "1.01E-1", "1.00E-1" };

            /* Expected Values */
            string[] expectedResultList = { null, null, "A", "A", "A", "A", "A", "A", null, null, null, null, null, null, "C", "C", "C", "B", "B", "B", "E", null, "E", "B", "B", "B", "D", null, null, null, null, null, "E", null, null, null, null, null, null, null, "E", "E", null, "E", null, "E", null, null, "F", "F", null, null, null, "F", "F", null, null, null, "F", "F", null, null, null };
            decimal?[] expectedCalcBt = { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, 10.5m, 10.5m, 10.5m, null, null, null, 10.0m, null, null, null, null, null, 10.5m, null, null, null, null, null, 10.5m, 10.5m, 10.5m, 10.5m, 10.4m, 10.4m, 5.5m, 5.5m, 5.4m, 5.4m, 10.5m, 10.5m, 10.5m, 10.4m, 10.5m, 20.5m, 20.5m, 20.5m, 20.4m, 20.5m, 50.5m, 50.5m, 50.5m, 50.0m, 50.5m };

            /* Check array lengths */
            Assert.AreEqual(caseCount, qsValidList.Length, "qsValidList length");
            Assert.AreEqual(caseCount, mtValidList.Length, "mtValidList length");
            Assert.AreEqual(caseCount, btValidList.Length, "btValidList length");
            Assert.AreEqual(caseCount, qaStatusList.Length, "qaStatusList length");
            Assert.AreEqual(caseCount, apsCodeList.Length, "apsCodeList length");
            Assert.AreEqual(caseCount, percentBtList.Length, "percentBtList length");
            Assert.AreEqual(caseCount, btValueList.Length, "btValueList length");
            Assert.AreEqual(caseCount, hgValueList.Length, "hgValueList length");
            Assert.AreEqual(caseCount, expectedResultList.Length, "expectedResultList length");
            Assert.AreEqual(caseCount, expectedCalcBt.Length, "expectedCalcBt length");

            for (int caseDex = 0; caseDex < caseCount; caseDex++)
            {
                /* Initialize Input Parameters */
                EmParameters.MatsBtTrapHgValid = btValidList[caseDex];
                EmParameters.MatsMainTrapHgValid = mtValidList[caseDex];
                EmParameters.MatsSamplingTrainQaStatusCodeValid = qsValidList[caseDex];
                EmParameters.MatsSamplingTrainRecord
                  = new MatsSamplingTrainRecord(trainQaStatusCd: qaStatusList[caseDex], sorbentTrapApsCd: apsCodeList[caseDex],
                                                hgConcentration: hgValueList[caseDex], percentBreakthrough: percentBtList[caseDex],
                                                mainTrapHg: mtValue, breakthroughTrapHg: btValueList[caseDex].DecimaltoScientificNotation(3));

                /* Initialize Output Parameters */
                EmParameters.MatsCalcTrainPercentBreakthrough = -999.9m;

                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Initialize variables needed to run the check. */
                bool log = false;
                string actual;

                // Run Checks
                actual = cMATSSamplingTrainChecks.MatsTrn14(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(expectedResultList[caseDex], category.CheckCatalogResult, string.Format("Result {0}", caseDex));
                Assert.AreEqual(expectedCalcBt[caseDex], EmParameters.MatsCalcTrainPercentBreakthrough, string.Format("MatsCalcTrainPercentBreakthrough {0}", caseDex));
            }

        }

        /// <summary>
        ///A test for MatsTrn15
        ///</summary>()
        [TestMethod()]
        public void MatsTrn15()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            /* Initialize variables needed to run the check. */
            bool log = false;
            string actual;

            /* Initialize General Variables */
            bool[] booleanList = new bool[] { false, true };
            string[] valueTypeList = new string[] { null, "NotRounded", "LessThan75", "EqualTo75", "Delta75", "Delta125", "EqualTo125", "GreaterThan125" };
            string[] invalidFactorList = new string[] { null, "SpikeTrap", "SpikeReference" };
            string[] trainQaStatusList = { "EXPIRED", "FAILED", "INC", "LOST", "PASSED", "UNCERTAIN" };
            bool?[] validList = { null, false, true };

            foreach (bool? valid in validList)
                foreach (string trainQaStatusCd in trainQaStatusList)
                    foreach (string valueType in valueTypeList)
                        foreach (string invalidFactor in invalidFactorList)
                        {
                            /* Initialize Variables */
                            decimal spikeReference;
                            decimal spikeTrap;
                            decimal calculated;
                            decimal? reported;
                            {
                                switch (valueType)
                                {
                                    case "LessThan75": spikeReference = 1.000m; spikeTrap = 0.740m; break;
                                    case "EqualTo75": spikeReference = 1.000m; spikeTrap = 0.750m; break;
                                    case "Delta75": spikeReference = 1.000m; spikeTrap = 0.750m; break;
                                    case "Delta125": spikeReference = 1.000m; spikeTrap = 1.250m; break;
                                    case "EqualTo125": spikeReference = 1.000m; spikeTrap = 1.250m; break;
                                    case "GreaterThan125": spikeReference = 1.000m; spikeTrap = 1.260m; break;
                                    default: spikeReference = 0.700m; spikeTrap = 0.600m; break;
                                }

                                calculated = Math.Round(100 * (spikeTrap / spikeReference), 1);

                                switch (valueType)
                                {
                                    case "NotRounded": reported = 100 * (spikeTrap / spikeReference); break;
                                    case "Delta75": reported = calculated + 0.1m; break;
                                    case "Delta125": reported = calculated - 0.1m; break;
                                    case null: reported = null; break;
                                    default: reported = calculated; break;
                                }
                            }

                            /*  Initialize Input Parameters*/
                            EmParameters.MatsSpikeReferenceValueValid = (invalidFactor != "SpikeReference");
                            EmParameters.MatsSpikeTrapHgValid = (invalidFactor != "SpikeTrap");
                            EmParameters.MatsSamplingTrainQaStatusCodeValid = valid;
                            EmParameters.MatsSamplingTrainRecord
                              = new MatsSamplingTrainRecord(componentId: "ComponentId", trainQaStatusCd: trainQaStatusCd, percentSpikeRecovery: reported,
                                                            spikeTrapHg: spikeTrap.DecimaltoScientificNotation(3),
                                                            spikeRefValue: spikeReference.DecimaltoScientificNotation(3));

                            /* Expected Results */
                            string result = null;

                            if (valid.Default(false))
                            {
                                if (valueType == null)
                                {
                                    if (trainQaStatusCd.NotInList("INC,EXPIRED,LOST"))
                                        result = "A";
                                }
                                else
                                {
                                    if (trainQaStatusCd.NotInList("PASSED,FAILED,UNCERTAIN"))
                                        result = "B";
                                    else if (valueType == "NotRounded")
                                        result = "C";
                                    else if (invalidFactor == null)
                                    {
                                        if (valueType.InList("Delta75,Delta125"))
                                            result = "D";
                                        else if (valueType.InList("LessThan75,GreaterThan125"))
                                        {
                                            if (trainQaStatusCd != "FAILED")
                                                result = "E";
                                        }
                                    }
                                }
                            }

                            /* Init Cateogry Result */
                            category.CheckCatalogResult = null;

                            // Run Checks
                            actual = cMATSSamplingTrainChecks.MatsTrn15(category, ref log);

                            /* Check Result Label */
                            string resultPrefix = string.Format("[valid: {0}, trainQaStatusCd: {1}, hgType: {2}, invalidFactor: {3}]", valid, trainQaStatusCd, valueType, invalidFactor);

                            // Check Results
                            Assert.AreEqual(string.Empty, actual);
                            Assert.AreEqual(false, log);
                            Assert.AreEqual(result, category.CheckCatalogResult, resultPrefix + ".Result");
                        }
        }

        /// <summary>
        ///A test for MatsTrn16
        ///</summary>()
        [TestMethod()]
        public void MatsTrn16()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            /* Initialize variables needed to run the check. */
            bool log = false;
            string actual;

            /* Case Arrays */
            int caseCount = 19;
            string[] caseLabel = new string[caseCount];
            string[] caseResult = new string[caseCount];
            string[] caseStatus = new string[caseCount];
            bool?[] caseTrainValid = new bool?[caseCount];
            int[] caseRatioTot = new int[caseCount];
            int[] caseRatioDev = new int[caseCount];
            string[] caseTrapId = new string[caseCount];
            bool[] caseBorder = new bool[caseCount];
            bool[] caseSupplemental = new bool[caseCount];
            {
                int i = 0;
                caseLabel[i] = "Total = 100; Passed; Deviation% > 5"; caseResult[i] = null; caseStatus[i] = "PASSED"; caseTrainValid[i] = true; caseRatioTot[i] = 100; caseRatioDev[i] = 5; caseTrapId[i] = "trapId2"; caseBorder[i] = false; caseSupplemental[i] = false; i++;
                caseLabel[i] = "Total = 100; Failed; Deviation% = 6"; caseResult[i] = null; caseStatus[i] = "FAILED"; caseTrainValid[i] = true; caseRatioTot[i] = 100; caseRatioDev[i] = 6; caseTrapId[i] = "trapId2"; caseBorder[i] = false; caseSupplemental[i] = false; i++;
                caseLabel[i] = "Total =  99; Failed; Deviation = 5"; caseResult[i] = null; caseStatus[i] = "PASSED"; caseTrainValid[i] = true; caseRatioTot[i] = 99; caseRatioDev[i] = 5; caseTrapId[i] = "trapId2"; caseBorder[i] = false; caseSupplemental[i] = false; i++;
                caseLabel[i] = "Total =  99; Failed; Deviation = 6"; caseResult[i] = null; caseStatus[i] = "FAILED"; caseTrainValid[i] = true; caseRatioTot[i] = 99; caseRatioDev[i] = 6; caseTrapId[i] = "trapId2"; caseBorder[i] = false; caseSupplemental[i] = false; i++;
                caseLabel[i] = "Total = 100; Passed; Deviation% > 6"; caseResult[i] = "A"; caseStatus[i] = "PASSED"; caseTrainValid[i] = true; caseRatioTot[i] = 100; caseRatioDev[i] = 6; caseTrapId[i] = "trapId2"; caseBorder[i] = false; caseSupplemental[i] = false; i++;
                caseLabel[i] = "Total = 100; Failed; Deviation% = 5"; caseResult[i] = null; caseStatus[i] = "FAILED"; caseTrainValid[i] = true; caseRatioTot[i] = 100; caseRatioDev[i] = 5; caseTrapId[i] = "trapId2"; caseBorder[i] = false; caseSupplemental[i] = false; i++;
                caseLabel[i] = "Total =  99; Failed; Deviation = 6"; caseResult[i] = "C"; caseStatus[i] = "PASSED"; caseTrainValid[i] = true; caseRatioTot[i] = 99; caseRatioDev[i] = 6; caseTrapId[i] = "trapId2"; caseBorder[i] = false; caseSupplemental[i] = false; i++;
                caseLabel[i] = "Total =  99; Failed; Deviation = 5"; caseResult[i] = null; caseStatus[i] = "FAILED"; caseTrainValid[i] = true; caseRatioTot[i] = 99; caseRatioDev[i] = 5; caseTrapId[i] = "trapId2"; caseBorder[i] = false; caseSupplemental[i] = false; i++;
                caseLabel[i] = "Total =  99; Failed; Deviation = 5"; caseResult[i] = null; caseStatus[i] = "FAILED"; caseTrainValid[i] = true; caseRatioTot[i] = 99; caseRatioDev[i] = 5; caseTrapId[i] = "trapId2"; caseBorder[i] = true; caseSupplemental[i] = false; i++;
                caseLabel[i] = "Total =  99; Failed; Deviation = 5"; caseResult[i] = null; caseStatus[i] = "FAILED"; caseTrainValid[i] = true; caseRatioTot[i] = 99; caseRatioDev[i] = 5; caseTrapId[i] = "trapId2"; caseBorder[i] = true; caseSupplemental[i] = true; i++;
                caseLabel[i] = "Total = 100; Passed; Deviation% > 6"; caseResult[i] = null; caseStatus[i] = "OTHER"; caseTrainValid[i] = true; caseRatioTot[i] = 100; caseRatioDev[i] = 6; caseTrapId[i] = "trapId2"; caseBorder[i] = false; caseSupplemental[i] = false; i++;
                caseLabel[i] = "Total = 100; Failed; Deviation% = 5"; caseResult[i] = null; caseStatus[i] = "OTHER"; caseTrainValid[i] = true; caseRatioTot[i] = 100; caseRatioDev[i] = 5; caseTrapId[i] = "trapId2"; caseBorder[i] = false; caseSupplemental[i] = false; i++;
                caseLabel[i] = "Total =  99; Failed; Deviation = 6"; caseResult[i] = null; caseStatus[i] = "OTHER"; caseTrainValid[i] = true; caseRatioTot[i] = 99; caseRatioDev[i] = 6; caseTrapId[i] = "trapId2"; caseBorder[i] = false; caseSupplemental[i] = false; i++;
                caseLabel[i] = "Total =  99; Failed; Deviation = 5"; caseResult[i] = null; caseStatus[i] = "OTHER"; caseTrainValid[i] = true; caseRatioTot[i] = 99; caseRatioDev[i] = 5; caseTrapId[i] = "trapId2"; caseBorder[i] = false; caseSupplemental[i] = false; i++;
                caseLabel[i] = "Total = 100; Passed; Deviation% > 6"; caseResult[i] = null; caseStatus[i] = "PASSED"; caseTrainValid[i] = false; caseRatioTot[i] = 100; caseRatioDev[i] = 6; caseTrapId[i] = "trapId2"; caseBorder[i] = false; caseSupplemental[i] = false; i++;
                caseLabel[i] = "Total = 100; Failed; Deviation% = 5"; caseResult[i] = null; caseStatus[i] = "FAILED"; caseTrainValid[i] = false; caseRatioTot[i] = 100; caseRatioDev[i] = 5; caseTrapId[i] = "trapId2"; caseBorder[i] = false; caseSupplemental[i] = false; i++;
                caseLabel[i] = "Total =  99; Failed; Deviation = 6"; caseResult[i] = null; caseStatus[i] = "PASSED"; caseTrainValid[i] = false; caseRatioTot[i] = 99; caseRatioDev[i] = 6; caseTrapId[i] = "trapId2"; caseBorder[i] = false; caseSupplemental[i] = false; i++;
                caseLabel[i] = "Total =  99; Failed; Deviation = 5"; caseResult[i] = null; caseStatus[i] = "FAILED"; caseTrainValid[i] = false; caseRatioTot[i] = 99; caseRatioDev[i] = 5; caseTrapId[i] = "trapId2"; caseBorder[i] = false; caseSupplemental[i] = false; i++;
            }

            for (int caseDex = 0; caseDex < caseCount; caseDex++)
            {
                /* Initialize Sampling Train Dictionary */
                EmParameters.MatsSamplingTrainDictionary = new Dictionary<string, SamplingTrainEvalInformation>();
                EmParameters.MatsSamplingTrainDictionary["TrapTrainId2"] = new SamplingTrainEvalInformation("TrapTrainId2", caseBorder[caseDex], caseSupplemental[caseDex]);
                EmParameters.MatsSamplingTrainDictionary["TrapTrainId2"].SamplingTrainValid = caseTrainValid[caseDex];
                EmParameters.MatsSamplingTrainDictionary["TrapTrainId2"].TotalSFSRRatioCount = caseRatioTot[caseDex];
                EmParameters.MatsSamplingTrainDictionary["TrapTrainId2"].DeviatedSFSRRatioCount = caseRatioDev[caseDex];

                /* Init Input Parameters */
                EmParameters.MatsSamplingTrainRecord = new MatsSamplingTrainRecord(trapTrainId: "TrapTrainId2", trapId: "trapId2", samplingRatioTestResultCd: caseStatus[caseDex]);

                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                // Run Checks
                actual = cMATSSamplingTrainChecks.MatsTrn16(category, ref log);

                /* Check Result Label */

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(caseResult[caseDex], category.CheckCatalogResult, caseLabel[caseDex]);
            }
        }

        /// <summary>
        /// 
        /// | ## | Status    | RataInd | TrainId | TrainValid | Total | Count || Result || Note
        /// |  0 | EXPIRED   | 0       | GoodId  | true       | 2000  | 400   || null   || Expired train which is not checked.
        /// |  1 | INC       | 0       | GoodId  | true       | 2000  | 400   || null   || Incomplete train which is not checked.
        /// |  2 | LOST      | 0       | GoodId  | true       | 2000  | 400   || null   || Lost train which is not checked.
        /// |  3 | FAILED    | 1       | GoodId  | true       | 2000  | 400   || null   || Failed RATA train which is not checked.
        /// |  4 | PASSED    | 1       | GoodId  | true       | 2000  | 400   || null   || Passed RATA train which is not checked.
        /// |  5 | UNCERTAIN | 1       | GoodId  | true       | 2000  | 400   || null   || Uncertain RATA train which is not checked.
        /// |  6 | FAILED    | 0       | GoodId  | true       | 2000  | 400   || A      || Failed train with Not Allowed GFM percentage of 20%, which returns result A.
        /// |  7 | PASSED    | 0       | GoodId  | true       | 2000  | 400   || A      || Passed train with Not Allowed GFM percentage of 20%, which returns result A.
        /// |  8 | UNCERTAIN | 0       | GoodId  | true       | 2000  | 400   || A      || Uncertain train with Not Allowed GFM percentage of 20%, which returns result A.
        /// |  9 | FAILED    | null    | GoodId  | true       | 2000  | 400   || A      || Failed train with null RATA indicator and Not Allowed GFM percentage of 20%, which returns result A.
        /// | 10 | PASSED    | null    | GoodId  | true       | 2000  | 400   || A      || Passed train with null RATA indicator and Not Allowed GFM percentage of 20%, which returns result A.
        /// | 11 | UNCERTAIN | null    | GoodId  | true       | 2000  | 400   || A      || Uncertain train with null RATA indicator and Not Allowed GFM percentage of 20%, which returns result A.
        /// | 12 | PASSED    | 0       | BadId   | true       | 2000  | 400   || null   || Passed train with no matching dictionary entry because of mismatched train id.
        /// | 13 | PASSED    | 0       | GoodId  | false      | 2000  | 400   || null   || Passed train but marked as invalid by other checks.
        /// | 14 | PASSED    | 0       | GoodId  | true       | 2000  | 399   || null   || Passed train with Not Allowed GFM percentage below 20%.
        /// | 15 | PASSED    | 0       | GoodId  | true       | 0     | 400   || null   || Divide by zero check.
        /// | 16 | PASSED    | 0       | GoodId  | true       | -1    | 400   || null   || Negative value check.
        /// | 17 | PASSED    | 0       | GoodId  | true       | 2000  | -1    || null   || Negative value check.
        /// | 18 | PASSED    | 0       | GoodId  | true       | 2000  | 0     || null   || Zero value check.
        /// 
        /// </summary>
        [TestMethod()]
        public void MatsTrn17()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            /* Input Parameter Values */
            string goodId = "GoodId";
            string badId = "BadId";

            string[] qaStatusList = { "EXPIRED", "INC", "LOST", "FAILED", "PASSED", "UNCERTAIN", "FAILED", "PASSED", "UNCERTAIN", "FAILED", "PASSED", "UNCERTAIN", "PASSED", "PASSED", "PASSED", "PASSED", "PASSED", "PASSED", "PASSED" };
            int?[] rataIndList = { 0, 0, 0, 1, 1, 1, 0, 0, 0, null, null, null, 0, 0, 0, 0, 0, 0, 0 };
            string[] dictTrainIdList = { goodId, goodId, goodId, goodId, goodId, goodId, goodId, goodId, goodId, goodId, goodId, goodId, badId, goodId, goodId, goodId, goodId, goodId, goodId };
            bool?[] dictTrainValidList = { true, true, true, true, true, true, true, true, true, true, true, true, true, false, true, true, true, true, true };
            int[] dictTotalList = { 2000, 2000, 2000, 2000, 2000, 2000, 2000, 2000, 2000, 2000, 2000, 2000, 2000, 2000, 2000, 0, -1, 2000, 2000 };
            int[] dictCountList = { 400, 400, 400, 400, 400, 400, 400, 400, 400, 400, 400, 400, 400, 400, 399, 400, 400, -1, 0 };

            /* Expected Values */
            string[] expectedResultList = { null, null, null, null, null, null, "A", "A", "A", "A", "A", "A", null, null, null, null, null, null, null };

            /* Case Count */
            int caseCount = 19;

            /* Check array lengths */
            Assert.AreEqual(caseCount, qaStatusList.Length, "qaStatusList length");
            Assert.AreEqual(caseCount, rataIndList.Length, "rataIndList length");
            Assert.AreEqual(caseCount, dictTrainIdList.Length, "dictTrainIdList length");
            Assert.AreEqual(caseCount, dictTrainValidList.Length, "dictTrainValidList length");
            Assert.AreEqual(caseCount, dictTotalList.Length, "dictTotalList length");
            Assert.AreEqual(caseCount, dictCountList.Length, "dictCountList length");
            Assert.AreEqual(caseCount, expectedResultList.Length, "expectedResultList length");

            for (int caseDex = 0; caseDex < caseCount; caseDex++)
            {
                /* Initialize Input Parameters */
                EmParameters.MatsSamplingTrainDictionary = new Dictionary<string, SamplingTrainEvalInformation>();
                {
                    EmParameters.MatsSamplingTrainDictionary[dictTrainIdList[caseDex]] = new SamplingTrainEvalInformation("TrainId");
                    EmParameters.MatsSamplingTrainDictionary[dictTrainIdList[caseDex]].TrainQAStatusCode = qaStatusList[caseDex];
                    EmParameters.MatsSamplingTrainDictionary[dictTrainIdList[caseDex]].SamplingTrainValid = dictTrainValidList[caseDex];
                    EmParameters.MatsSamplingTrainDictionary[dictTrainIdList[caseDex]].TotalGfmCount = dictTotalList[caseDex];
                    EmParameters.MatsSamplingTrainDictionary[dictTrainIdList[caseDex]].NotAvailablelGfmCount = dictCountList[caseDex];
                }
                EmParameters.MatsSamplingTrainRecord
                  = new MatsSamplingTrainRecord(trapTrainId: goodId, trainQaStatusCd: qaStatusList[caseDex], rataInd: rataIndList[caseDex]);

                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Initialize variables needed to run the check. */
                bool log = false;
                string actual;

                // Run Checks
                actual = cMATSSamplingTrainChecks.MatsTrn17(category, ref log);

                // Check Results
                Assert.AreEqual(string.Empty, actual);
                Assert.AreEqual(false, log);
                Assert.AreEqual(expectedResultList[caseDex], category.CheckCatalogResult, string.Format("Result {0}", caseDex));
            }


        }

        #endregion

    }
}