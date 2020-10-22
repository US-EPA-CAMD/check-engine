using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Data.Ecmps.CheckEm.Function;
using ECMPS.Checks.Data.Ecmps.Dbo.View;
using ECMPS.Checks.Data.Ecmps.Dbo.Table;
using ECMPS.Checks.Data.Ecmps.Lookup.Table;
using ECMPS.Checks.Data.EcmpsAux.CrossCheck.Virtual;
using ECMPS.Checks.Em.Parameters;
using ECMPS.Checks.EmissionsChecks;
using ECMPS.Checks.EmissionsReport;
using ECMPS.Checks.Parameters;


using UnitTest.UtilityClasses;

namespace UnitTest.Emissions
{
    [TestClass]
    public class cHourlyGeneralChecksTest
    {

        /// <summary>
        /// HOURGEN-1
        /// 
        /// Currently only ensures that the following are set correctly:
        /// 
        ///     InvalidCylinderIdList
        ///     
        /// </summary>
        /// <returns></returns>
        [TestMethod()]
        public void HourGen1_Incomplete()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            cHourlyGeneralChecks target = new cHourlyGeneralChecks(new cEmissionsCheckParameters());


            /* Initialize Input Parameters */
            EmParameters.CurrentLocationCount = 3;
            EmParameters.HourlyFuelFlowRecords = new CheckDataView<VwMpHrlyFuelFlowRow>();


            /* Init Cateogry Result */
            category.CheckCatalogResult = null;

            /* Initialize variables needed to run the check. */
            bool log = false;
            string actual;

            /* Run Check */
            actual = target.HOURGEN1(category, ref log);


            /* Check Results */
            Assert.AreEqual(string.Empty, actual, string.Format("actual"));
            Assert.AreEqual(false, log, string.Format("log"));
            Assert.AreEqual(null, category.CheckCatalogResult, string.Format("category.CheckCatalogResult"));

            Assert.AreEqual(0, EmParameters.InvalidCylinderIdList.Count, string.Format("InvalidCylinderIdList"));
        }


        /// <summary>
        /// 
        /// 
        /// Current Reporting Period
        /// 
        ///     Begin Hour (prdB) : 2017-04-01 00
        ///     End Hour (prdE)   : 2017-06-30 23
        /// 
        ///               |            Qual 1            |  Qual 2  |  Qual 3  |    Id for 2017 % Row     ||
        /// | ## | MpEval | QualType | BegDt   | EndDt   | QualType | QualType | % 1 Id | % 2 Id | % 3 Id || Result | List                                                                           || Note
        /// |  0 | null   | GF       | prdB    | null    |          |          |        |        |        || null   | ""                                                                             || MpSuccessfullyEvaluated is null, so check body is not executed.
        /// |  1 | false  | GF       | prdB    | null    |          |          |        |        |        || null   | ""                                                                             || MpSuccessfullyEvaluated is false, so check body is not executed.
        /// |  2 | true   | COMPLEX  | prdB    | null    |          |          |        |        |        || null   | ""                                                                             || Existing qualification is not GF, PK or SK.
        /// |  3 | true   | GF       | prdB    | null    |          |          |        |        |        || A      | "Gas-Fired Unit 1"                                                             || GF exists without a percent row for current year.
        /// |  4 | true   | HGAVG    | prdB    | null    |          |          |        |        |        || null   | ""                                                                             || Existing qualification is not GF, PK or SK.
        /// |  5 | true   | LEE      | prdB    | null    |          |          |        |        |        || null   | ""                                                                             || Existing qualification is not GF, PK or SK.
        /// |  6 | true   | LMEA     | prdB    | null    |          |          |        |        |        || null   | ""                                                                             || Existing qualification is not GF, PK or SK.
        /// |  7 | true   | LMES     | prdB    | null    |          |          |        |        |        || null   | ""                                                                             || Existing qualification is not GF, PK or SK.
        /// |  8 | true   | LOWSULF  | prdB    | null    |          |          |        |        |        || null   | ""                                                                             || Existing qualification is not GF, PK or SK.
        /// |  9 | true   | PK       | prdB    | null    |          |          |        |        |        || A      | "Year-Round Peaking Unit 1"                                                    || PK exists without a percent row for current year.
        /// | 10 | true   | PRATA1   | prdB    | null    |          |          |        |        |        || null   | ""                                                                             || Existing qualification is not GF, PK or SK.
        /// | 11 | true   | PRATA2   | prdB    | null    |          |          |        |        |        || null   | ""                                                                             || Existing qualification is not GF, PK or SK.
        /// | 12 | true   | SK       | prdB    | null    |          |          |        |        |        || A      | "Ozone-Season Peaking Unit 1"                                                  || SK exists without a percent row for current year.
        /// | 13 | true   | GF       | prdE    | null    |          |          |        |        |        || A      | "Gas-Fired Unit 1"                                                             || GF begins on ending period date without a percent row for current year.
        /// | 14 | true   | GF       | prdB-2d | prdB    |          |          |        |        |        || A      | "Gas-Fired Unit 1"                                                             || GF ends on the beginning period date without a percent row for current year.
        /// | 15 | true   | GF       | prdE+1d | null    |          |          |        |        |        || null   | ""                                                                             || GF begins after the period without a percent row for current year.
        /// | 16 | true   | GF       | prdB-2d | prdB-1d |          |          |        |        |        || null   | ""                                                                             || GF ends before the period without a percent row for current year.
        /// | 17 | true   | GF       | prdB    | null    |          |          | QUAL2  |        |        || A      | "Gas-Fired Unit 1"                                                             || GF exists, but percent row for current year is for another qualification.
        /// | 18 | true   | GF       | prdB    | null    |          |          | QUAL1  |        |        || null   | ""                                                                             || GF exists with a percent row for current year.
        /// | 19 | true   | PK       | prdB    | null    |          |          | QUAL1  |        |        || null   | ""                                                                             || PK exists with a percent row for current year.
        /// | 20 | true   | SK       | prdB    | null    |          |          | QUAL1  |        |        || null   | ""                                                                             || SK exists with a percent row for current year.
        /// | 21 | true   | GF       | prdB    | null    | PK       | SK       | QUAL1  | QUAL2  | QUAL3  || null   | ""                                                                             || GF exists with a percent row for current year.
        /// | 22 | true   | GF       | prdB    | null    | PK       | SK       |        |        |        || A      | "Gas-Fired Unit 1, Year-Round Peaking Unit 2, and Ozone-Season Peaking Unit 3" || GF exists with a percent row for current year.
        /// </summary>
        [TestMethod]
        public void HourGen17()
        {
            /* Initialize objects generally needed for testing checks. */
            cEmissionsCheckParameters emCheckParameters = UnitTestCheckParameters.InstantiateEmParameters(); // Old Instantiated Parameters Used
            cHourlyGeneralChecks target = new cHourlyGeneralChecks(emCheckParameters);
            cCategory category = EmParameters.Category;


            /* Input Parameter Values */
            DateTime prdBegHr = new DateTime(2017, 4, 1, 0, 0, 0);
            DateTime prdBegDt = prdBegHr.Date;
            DateTime prdEndHr = new DateTime(2017, 6, 30, 23, 0, 0);
            DateTime prdEndDt = prdEndHr.Date;
            int prdYear = prdBegHr.Year;

            bool?[] mpEvalList = { null, false, true, true, true, true, true, true, true, true,
                                   true, true, true, true, true, true, true, true, true, true,
                                   true, true, true };
            string[] qual1TypeList = { "GF", "GF", "COMPLEX", "GF", "HGAVG", "LEE", "LMEA", "LMES", "LOWSULF", "PK",
                                       "PRATA1", "PRATA2", "SK", "GF", "GF", "GF", "GF", "GF", "GF", "PK",
                                       "SK", "GF", "GF" };
            DateTime?[] qual1BegDateList = { prdBegDt, prdBegDt, prdBegDt, prdBegDt, prdBegDt, prdBegDt, prdBegDt, prdBegDt, prdBegDt, prdBegDt,
                                             prdBegDt, prdBegDt, prdBegDt, prdBegDt, prdBegDt.AddDays(-2), prdEndDt.AddDays(1), prdBegDt.AddDays(-2), prdBegDt, prdBegDt, prdBegDt,
                                             prdBegDt, prdBegDt, prdBegDt };
            DateTime?[] qual1EndDateList = { null, null, null, null, null, null, null, null, null, null,
                                             null, null, null, null, prdBegDt, null, prdBegDt.AddDays(-1), null, null, null,
                                             null, null, null };
            string[] qual2TypeList = { null, null, null, null, null, null, null, null, null, null,
                                       null, null, null, null, null, null, null, null, null, null,
                                       null, "PK", "PK" };
            string[] qual3TypeList = { null, null, null, null, null, null, null, null, null, null,
                                       null, null, null, null, null, null, null, null, null, null,
                                       null, "SK", "SK" };
            string[] pct1IdList = { null, null, null, null, null, null, null, null, null, null,
                                    null, null, null, null, null, null, null, "QUAL2", "QUAL1", "QUAL1",
                                    "QUAL1", "QUAL1", null };
            string[] pct2IdList = { null, null, null, null, null, null, null, null, null, null,
                                    null, null, null, null, null, null, null, null, null, null,
                                    null, "QUAL2", null };
            string[] pct3IdList = { null, null, null, null, null, null, null, null, null, null,
                                    null, null, null, null, null, null, null, null, null, null,
                                    null, "QUAL3", null };

            /* Expected Values */
            string[] expResultList = { null, null, null, "A", null, null, null, null, null, "A",
                                       null, null, "A", "A", "A", null, null, "A", null, null,
                                       null, null, "A" };
            string[] expMissingList = { "", "", "", "Gas-Fired Unit 1", "", "", "", "", "", "Year-Round Peaking Unit 1",
                                        "", "", "Ozone-Season Peaking Unit 1", "Gas-Fired Unit 1", "Gas-Fired Unit 1", "", "", "Gas-Fired Unit 1", "", "",
                                        "", "", "Gas-Fired Unit 1, Year-Round Peaking Unit 2, and Ozone-Season Peaking Unit 3" };


            /* Test Case Count */
            int caseCount = 23;

            /* Check array lengths */
            Assert.AreEqual(caseCount, mpEvalList.Length, "mpEvalList length");
            Assert.AreEqual(caseCount, qual1TypeList.Length, "qual1TypeList length");
            Assert.AreEqual(caseCount, qual1BegDateList.Length, "qual1BegDateList length");
            Assert.AreEqual(caseCount, qual1EndDateList.Length, "qual1EndDateList length");
            Assert.AreEqual(caseCount, qual2TypeList.Length, "qual2TypeList length");
            Assert.AreEqual(caseCount, qual3TypeList.Length, "qual3TypeList length");
            Assert.AreEqual(caseCount, pct1IdList.Length, "pct1IdList length");
            Assert.AreEqual(caseCount, pct2IdList.Length, "pct2IdList length");
            Assert.AreEqual(caseCount, pct3IdList.Length, "pct3IdList length");
            Assert.AreEqual(caseCount, expResultList.Length, "expResultList length");
            Assert.AreEqual(caseCount, expMissingList.Length, "expMissingList length");


            /* Run Cases */
            for (int caseDex = 0; caseDex < caseCount; caseDex++)
            {
                /*  Initialize Input Parameters*/
                EmParameters.CurrentReportingPeriodBeginHour = prdBegHr;
                EmParameters.CurrentReportingPeriodEndHour = prdEndHr;
                EmParameters.CurrentReportingPeriodYear = prdYear;
                EmParameters.MpQualificationPercentRecords = new CheckDataView<MonitorQualificationPercentData>();
                {
                    EmParameters.MpQualificationPercentRecords.Add(new MonitorQualificationPercentData(monQualId: pct1IdList[caseDex], qualYear: prdYear - 1));

                    if (pct1IdList[caseDex] != null)
                        EmParameters.MpQualificationPercentRecords.Add(new MonitorQualificationPercentData(monQualId: pct1IdList[caseDex], qualYear: prdYear));

                    if (pct2IdList[caseDex] != null)
                        EmParameters.MpQualificationPercentRecords.Add(new MonitorQualificationPercentData(monQualId: pct2IdList[caseDex], qualYear: prdYear));

                    if (pct3IdList[caseDex] != null)
                        EmParameters.MpQualificationPercentRecords.Add(new MonitorQualificationPercentData(monQualId: pct3IdList[caseDex], qualYear: prdYear));

                    EmParameters.MpQualificationPercentRecords.Add(new MonitorQualificationPercentData(monQualId: pct1IdList[caseDex], qualYear: prdYear + 1));
                }
                EmParameters.MpQualificationRecords = new CheckDataView<VwMpMonitorQualificationRow>();
                {
                    EmParameters.MpQualificationRecords.Add(new VwMpMonitorQualificationRow(monQualId: "Bad1", locationId: "Bad1", qualTypeCd: "BAD1", beginDate: prdBegDt));

                    if (qual1TypeList[caseDex] != null)
                        EmParameters.MpQualificationRecords.Add(new VwMpMonitorQualificationRow(monQualId: "QUAL1", locationId: "1", qualTypeCd: qual1TypeList[caseDex], beginDate: qual1BegDateList[caseDex], endDate: qual1EndDateList[caseDex]));

                    if (qual2TypeList[caseDex] != null)
                        EmParameters.MpQualificationRecords.Add(new VwMpMonitorQualificationRow(monQualId: "QUAL2", locationId: "2", qualTypeCd: qual2TypeList[caseDex], beginDate: prdBegDt));

                    if (qual3TypeList[caseDex] != null)
                        EmParameters.MpQualificationRecords.Add(new VwMpMonitorQualificationRow(monQualId: "QUAL3", locationId: "3", qualTypeCd: qual3TypeList[caseDex], beginDate: prdBegDt));

                    EmParameters.MpQualificationRecords.Add(new VwMpMonitorQualificationRow(monQualId: "Bad2", locationId: "Bad2", qualTypeCd: "BAD2", beginDate: prdBegDt));
                }
                EmParameters.MpSuccessfullyEvaluated = mpEvalList[caseDex];


                /*  Initialize Output Parameters*/
                EmParameters.QualificationPercentMissingList = "Bad List";


                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Initialize variables needed to run the check. */
                bool log = false;
                string actual;

                /* Run Check */
                actual = target.HOURGEN17(category, ref log);

                /* Check Results */
                Assert.AreEqual(string.Empty, actual, string.Format("actual {0}", caseDex));
                Assert.AreEqual(false, log, string.Format("log {0}", caseDex));

                Assert.AreEqual(expResultList[caseDex], category.CheckCatalogResult, string.Format("category.CheckCatalogResult {0}", caseDex));
                Assert.AreEqual(expMissingList[caseDex], EmParameters.QualificationPercentMissingList, string.Format("QualificationPercentMissingList {0}", caseDex));
            }

        }


        [TestMethod]
        public void HourGen19()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            cHourlyGeneralChecks target = new cHourlyGeneralChecks(new cEmissionsCheckParameters());


            /* Run cases */
            foreach (bool trap1IsSupplementalData in UnitTestStandardLists.BooleanList)
                foreach (bool trap2IsSupplementalData in UnitTestStandardLists.BooleanList)
                {
                    /* Initialize input parameters */
                    EmParameters.CurrentLocationCount = 3;
                    EmParameters.MatsSorbentTrapRecords = new CheckDataView<MatsSorbentTrapRecord>();
                    {
                        EmParameters.MatsSorbentTrapRecords.Add(new MatsSorbentTrapRecord(trapId: "trap1", suppDataInd: (trap1IsSupplementalData ? 1 : 0)));
                        EmParameters.MatsSorbentTrapRecords.Add(new MatsSorbentTrapRecord(trapId: "trap2", suppDataInd: (trap2IsSupplementalData ? 1 : 0)));
                    }

                    /* Initialize output parameters */
                    EmParameters.MatsSamplingTrainDictionary = null;
                    EmParameters.MatsSorbentTrapListByLocationArray = null;
                    EmParameters.MatsSorbentTrapDictionary = null;
                    EmParameters.MatsSorbentTrapEvaluationNeeded = null;


                    /* Init Cateogry Result */
                    category.CheckCatalogResult = null;

                    /* Initialize variables needed to run the check. */
                    bool log = false;
                    string actual;

                    /* Run Check */
                    actual = target.HOURGEN19(category, ref log);


                    /* Check results */
                    Assert.AreEqual(string.Empty, actual);
                    Assert.AreEqual(false, log);
                    Assert.AreEqual(null, category.CheckCatalogResult, "Result");

                    Assert.AreNotEqual(null, EmParameters.MatsSamplingTrainDictionary, string.Format("MatsSamplingTrainDictionary [{0}, {1}]", trap1IsSupplementalData, trap2IsSupplementalData));
                    Assert.AreEqual(0, EmParameters.MatsSamplingTrainDictionary.Count, string.Format("MatsSamplingTrainDictionary.Count [{0}, {1}]", trap1IsSupplementalData, trap2IsSupplementalData));
                    Assert.AreNotEqual(null, EmParameters.MatsSorbentTrapDictionary, string.Format("MatsSorbentTrapDictionary [{0}, {1}]", trap1IsSupplementalData, trap2IsSupplementalData));
                    Assert.AreEqual(0, EmParameters.MatsSorbentTrapDictionary.Count, string.Format("MatsSorbentTrapDictionary.Count [{0}, {1}]", trap1IsSupplementalData, trap2IsSupplementalData));
                    Assert.AreNotEqual(null, EmParameters.MatsSorbentTrapListByLocationArray, string.Format("MatsSorbentTrapListByLocationArray [{0}, {1}]", trap1IsSupplementalData, trap2IsSupplementalData));
                    Assert.AreEqual(3, EmParameters.MatsSorbentTrapListByLocationArray.Length, string.Format("MatsSorbentTrapListByLocationArray.Length [{0}, {1}]", trap1IsSupplementalData, trap2IsSupplementalData));
                    Assert.AreNotEqual(null, EmParameters.MatsSorbentTrapListByLocationArray[0], string.Format("MatsSorbentTrapListByLocationArray[0] [{0}, {1}]", trap1IsSupplementalData, trap2IsSupplementalData));
                    Assert.AreEqual(0, EmParameters.MatsSorbentTrapListByLocationArray[0].Count, string.Format("MatsSorbentTrapListByLocationArray[0].Count [{0}, {1}]", trap1IsSupplementalData, trap2IsSupplementalData));
                    Assert.AreNotEqual(null, EmParameters.MatsSorbentTrapListByLocationArray[1], string.Format("MatsSorbentTrapListByLocationArray[1] [{0}, {1}]", trap1IsSupplementalData, trap2IsSupplementalData));
                    Assert.AreEqual(0, EmParameters.MatsSorbentTrapListByLocationArray[1].Count, string.Format("MatsSorbentTrapListByLocationArray[1].Count [{0}, {1}]", trap1IsSupplementalData, trap2IsSupplementalData));
                    Assert.AreNotEqual(null, EmParameters.MatsSorbentTrapListByLocationArray[2], string.Format("MatsSorbentTrapListByLocationArray[2] [{0}, {1}]", trap1IsSupplementalData, trap2IsSupplementalData));
                    Assert.AreEqual(0, EmParameters.MatsSorbentTrapListByLocationArray[2].Count, string.Format("MatsSorbentTrapListByLocationArray[2].Count [{0}, {1}]", trap1IsSupplementalData, trap2IsSupplementalData));
                    Assert.AreEqual(!trap1IsSupplementalData || !trap2IsSupplementalData, EmParameters.MatsSorbentTrapEvaluationNeeded, string.Format("MatsSorbentTrapEvaluationNeeded [{0}, {1}]", trap1IsSupplementalData, trap2IsSupplementalData));
                }
        }

        [TestMethod]
        public void HourGen20()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            cHourlyGeneralChecks target = new cHourlyGeneralChecks(new cEmissionsCheckParameters());


            /* Initialize variables needed to run the check. */
            bool log = false;
            string actual;

            /* Initialize output parameters */
            EmParameters.WsiTestDictionary = null;

            /* Call check */
            actual = target.HOURGEN20(category, ref log);

            /* Check results */
            Assert.AreEqual(string.Empty, actual);
            Assert.AreEqual(false, log);
            Assert.AreEqual(null, category.CheckCatalogResult, "Result");

            Assert.AreNotEqual(null, EmParameters.WsiTestDictionary, "WsiTestDictionary");
            Assert.AreEqual(0, EmParameters.WsiTestDictionary.Count, "WsiTestDictionary.Count");
        }

        [TestMethod]
        public void HourGen21()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            cHourlyGeneralChecks target = new cHourlyGeneralChecks(new cEmissionsCheckParameters());


            /* Initialize variables needed to run the check. */
            bool log = false;
            string actual;

            /* Initialize output parameters */
            EmParameters.TestResultCodeLookupTable = new CheckDataView<TestResultCodeRow>
            (
              new TestResultCodeRow(testResultCd: "THREE"),
              new TestResultCodeRow(testResultCd: "TWO"),
              new TestResultCodeRow(testResultCd: "ONE")
            );

            /* Initialize output parameters */
            EmParameters.TestResultCodeList = null;

            /* Call check */
            actual = target.HOURGEN21(category, ref log);

            /* Check results */
            Assert.AreEqual(string.Empty, actual);
            Assert.AreEqual(false, log);
            Assert.AreEqual(null, category.CheckCatalogResult, "Result");

            Assert.AreEqual("THREE,TWO,ONE", EmParameters.TestResultCodeList, "TestResultCodeList.Count");
        }

        /// <summary>
        /// HourGen-23
        /// 
        /// </summary>
        [TestMethod]
        public void HourGen23()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            cHourlyGeneralChecks target = new cHourlyGeneralChecks(new cEmissionsCheckParameters());


            /* Initialize Required Parameters */
            EmParameters.ProgramCodeTable = new CheckDataView<ProgramCodeRow>
                (
                    new ProgramCodeRow(prgCd: "ARP", prgName: "Acid Rain Program", osInd: 0, rueInd: 1, so2CertInd: 1, noxCertInd: 1, noxcCertInd: 0),
                    new ProgramCodeRow(prgCd: "CAIRNOX", prgName: "CAIR NOx Annual Program", osInd: 0, rueInd: 1, so2CertInd: 0, noxCertInd: 1, noxcCertInd: 1),
                    new ProgramCodeRow(prgCd: "CAIROS", prgName: "CAIR NOx Ozone Season Program", osInd: 1, rueInd: 1, so2CertInd: 0, noxCertInd: 1, noxcCertInd: 1),
                    new ProgramCodeRow(prgCd: "CAIRSO2", prgName: "CAIR SO2 Annual Program", osInd: 0, rueInd: 1, so2CertInd: 1, noxCertInd: 0, noxcCertInd: 0),
                    new ProgramCodeRow(prgCd: "CSNOX", prgName: "CAIR NOx Annual Program", osInd: 0, rueInd: 1, so2CertInd: 0, noxCertInd: 1, noxcCertInd: 1),
                    new ProgramCodeRow(prgCd: "CSNOXOS", prgName: "Cross - State Air Pollution Rule NOx Ozone Season Program", osInd: 1, rueInd: 1, so2CertInd: 0, noxCertInd: 1, noxcCertInd: 1),
                    new ProgramCodeRow(prgCd: "CSOSG1", prgName: "Cross - State Air Pollution Rule NOx Ozone Season Group 1 Program", osInd: 1, rueInd: 1, so2CertInd: 0, noxCertInd: 1, noxcCertInd: 1),
                    new ProgramCodeRow(prgCd: "CSOSG2", prgName: "Cross - State Air Pollution Rule NOx Ozone Season Group 2 Program", osInd: 1, rueInd: 1, so2CertInd: 0, noxCertInd: 1, noxcCertInd: 1),
                    new ProgramCodeRow(prgCd: "CSSO2G1", prgName: "Cross - State Air Pollution Rule SO2 Annual Group 1 Program", osInd: 0, rueInd: 1, so2CertInd: 1, noxCertInd: 0, noxcCertInd: 0),
                    new ProgramCodeRow(prgCd: "CSSO2G2", prgName: "Cross - State Air Pollution Rule SO2 Annual Group 2 Program", osInd: 0, rueInd: 1, so2CertInd: 1, noxCertInd: 0, noxcCertInd: 0),
                    new ProgramCodeRow(prgCd: "MATS", prgName: "Mercury and Air Toxics Standards", osInd: 0, rueInd: 0, so2CertInd: 0, noxCertInd: 0, noxcCertInd: 0),
                    new ProgramCodeRow(prgCd: "NBP", prgName: "NOx Budget Trading Program", osInd: 1, rueInd: 0, so2CertInd: 0, noxCertInd: 1, noxcCertInd: 1),
                    new ProgramCodeRow(prgCd: "NHNOX", prgName: "NH NOx Program", osInd: 1, rueInd: 0, so2CertInd: 0, noxCertInd: 1, noxcCertInd: 1),
                    new ProgramCodeRow(prgCd: "OTC", prgName: "OTC NOx Budget Program", osInd: 0, rueInd: 0, so2CertInd: 0, noxCertInd: 0, noxcCertInd: 0, notes: "OTC is not treated as a OS program in ECMPS."),
                    new ProgramCodeRow(prgCd: "RGGI", prgName: "Regional Greenhouse Gas Initiative", osInd: 0, rueInd: 0, so2CertInd: 0, noxCertInd: 0, noxcCertInd: 0),
                    new ProgramCodeRow(prgCd: "SIPNOX", prgName: "SIP NOx Program", osInd: 1, rueInd: 0, so2CertInd: 0, noxCertInd: 1, noxcCertInd: 1, notes: " SIPNOX is treated as a OS program in ECMPS")
                );

            /* Initialize Output Parameters */
            EmParameters.ProgramIsOzoneSeasonList = "Bad List";
            EmParameters.ProgramRequiresNoxSystemCertificationList = "Bad List";
            EmParameters.ProgramRequiresNoxcSystemCertificationList = "Bad List";
            EmParameters.ProgramRequiresSo2SystemCertificationList = "Bad List";
            EmParameters.ProgramUsesRueList = "Bad List";


            /* Init Cateogry Result */
            category.CheckCatalogResult = null;

            /* Initialize variables needed to run the check. */
            bool log = false;
            string actual;

            /* Run Check */
            actual = target.HOURGEN23(category, ref log);

            /* Check Results */
            Assert.AreEqual(string.Empty, actual, string.Format("actual"));
            Assert.AreEqual(false, log, string.Format("log"));
            Assert.AreEqual(null, category.CheckCatalogResult, string.Format("category.CheckCatalogResult"));

            Assert.AreEqual("CAIROS,CSNOXOS,CSOSG1,CSOSG2,NBP,NHNOX,SIPNOX", EmParameters.ProgramIsOzoneSeasonList, string.Format("ProgramIsOzoneSeasonList"));
            Assert.AreEqual("ARP,CAIRNOX,CAIROS,CSNOX,CSNOXOS,CSOSG1,CSOSG2,NBP,NHNOX,SIPNOX", EmParameters.ProgramRequiresNoxSystemCertificationList, string.Format("ProgramRequiresNoxSystemCertificationList"));
            Assert.AreEqual("CAIRNOX,CAIROS,CSNOX,CSNOXOS,CSOSG1,CSOSG2,NBP,NHNOX,SIPNOX", EmParameters.ProgramRequiresNoxcSystemCertificationList, string.Format("ProgramRequiresNoxcSystemCertificationList"));
            Assert.AreEqual("ARP,CAIRSO2,CSSO2G1,CSSO2G2", EmParameters.ProgramRequiresSo2SystemCertificationList, string.Format("ProgramRequiresSo2SystemCertificationList"));
            Assert.AreEqual("ARP,CAIRNOX,CAIROS,CAIRSO2,CSNOX,CSNOXOS,CSOSG1,CSOSG2,CSSO2G1,CSSO2G2", EmParameters.ProgramUsesRueList, string.Format("ProgramUsesRueList"));

        }

        [TestMethod]
        public void HourGen24()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            cHourlyGeneralChecks target = new cHourlyGeneralChecks(new cEmissionsCheckParameters());

            category.Process.ProcessParameters.RegisterParameter(3618, "Location_Name_Array"); // Currently cannot access arrays using the new check parameter access.


            /* Initialize Required Parameters */
            EmParameters.HourlyEmissionsTolerancesCrossCheckTable = new CheckDataView<HourlyEmissionsTolerancesRow>();
            {
                EmParameters.HourlyEmissionsTolerancesCrossCheckTable.Add(new HourlyEmissionsTolerancesRow(parameter: "CARBON", uOM: "MW", tolerance: "0.1"));
                EmParameters.HourlyEmissionsTolerancesCrossCheckTable.Add(new HourlyEmissionsTolerancesRow(parameter: "CO2", uOM: "MW", tolerance: "0.1"));
                EmParameters.HourlyEmissionsTolerancesCrossCheckTable.Add(new HourlyEmissionsTolerancesRow(parameter: "CO2C", uOM: "MW", tolerance: "0.1"));
                EmParameters.HourlyEmissionsTolerancesCrossCheckTable.Add(new HourlyEmissionsTolerancesRow(parameter: "CO2M", uOM: "MW", tolerance: "0.1"));
                EmParameters.HourlyEmissionsTolerancesCrossCheckTable.Add(new HourlyEmissionsTolerancesRow(parameter: "CO2M DAILY", uOM: "MW", tolerance: "0.1"));
                EmParameters.HourlyEmissionsTolerancesCrossCheckTable.Add(new HourlyEmissionsTolerancesRow(parameter: "FLOW", uOM: "MW", tolerance: "1000"));
                EmParameters.HourlyEmissionsTolerancesCrossCheckTable.Add(new HourlyEmissionsTolerancesRow(parameter: "FOIL", uOM: "MW", tolerance: "0.1"));
                EmParameters.HourlyEmissionsTolerancesCrossCheckTable.Add(new HourlyEmissionsTolerancesRow(parameter: "H2O", uOM: "MW", tolerance: "0.1"));
                EmParameters.HourlyEmissionsTolerancesCrossCheckTable.Add(new HourlyEmissionsTolerancesRow(parameter: "HI", uOM: "MW", tolerance: "1"));
                EmParameters.HourlyEmissionsTolerancesCrossCheckTable.Add(new HourlyEmissionsTolerancesRow(parameter: "HI HPFF", uOM: "MW", tolerance: "0.1"));
                EmParameters.HourlyEmissionsTolerancesCrossCheckTable.Add(new HourlyEmissionsTolerancesRow(parameter: "HIT", uOM: "MW", tolerance: "1"));
                EmParameters.HourlyEmissionsTolerancesCrossCheckTable.Add(new HourlyEmissionsTolerancesRow(parameter: "LOAD", uOM: null, tolerance: "0.1"));
                EmParameters.HourlyEmissionsTolerancesCrossCheckTable.Add(new HourlyEmissionsTolerancesRow(parameter: "LOAD", uOM: "LB", tolerance: "0.1"));
                EmParameters.HourlyEmissionsTolerancesCrossCheckTable.Add(new HourlyEmissionsTolerancesRow(parameter: "LOAD", uOM: "LBHR", tolerance: "0.1"));
                EmParameters.HourlyEmissionsTolerancesCrossCheckTable.Add(new HourlyEmissionsTolerancesRow(parameter: "LOAD", uOM: "LBMMBTU", tolerance: "0.005"));
                EmParameters.HourlyEmissionsTolerancesCrossCheckTable.Add(new HourlyEmissionsTolerancesRow(parameter: "LOAD", uOM: "MMBTU", tolerance: "1"));
                EmParameters.HourlyEmissionsTolerancesCrossCheckTable.Add(new HourlyEmissionsTolerancesRow(parameter: "LOAD", uOM: "MMBTUHR", tolerance: "0.1"));
                EmParameters.HourlyEmissionsTolerancesCrossCheckTable.Add(new HourlyEmissionsTolerancesRow(parameter: "LOAD", uOM: "MW", tolerance: "13"));
                EmParameters.HourlyEmissionsTolerancesCrossCheckTable.Add(new HourlyEmissionsTolerancesRow(parameter: "LOAD", uOM: "PCT", tolerance: "0.1"));
                EmParameters.HourlyEmissionsTolerancesCrossCheckTable.Add(new HourlyEmissionsTolerancesRow(parameter: "LOAD", uOM: "PPM", tolerance: "0.1"));
                EmParameters.HourlyEmissionsTolerancesCrossCheckTable.Add(new HourlyEmissionsTolerancesRow(parameter: "LOAD", uOM: "SCFH", tolerance: "1000"));
                EmParameters.HourlyEmissionsTolerancesCrossCheckTable.Add(new HourlyEmissionsTolerancesRow(parameter: "LOAD", uOM: "TNHR", tolerance: "0.1"));
                EmParameters.HourlyEmissionsTolerancesCrossCheckTable.Add(new HourlyEmissionsTolerancesRow(parameter: "LOAD", uOM: "TON", tolerance: "0.1"));
                EmParameters.HourlyEmissionsTolerancesCrossCheckTable.Add(new HourlyEmissionsTolerancesRow(parameter: "NOX", uOM: "MW", tolerance: "0.1"));
                EmParameters.HourlyEmissionsTolerancesCrossCheckTable.Add(new HourlyEmissionsTolerancesRow(parameter: "NOXC", uOM: "MW", tolerance: "0.1"));
                EmParameters.HourlyEmissionsTolerancesCrossCheckTable.Add(new HourlyEmissionsTolerancesRow(parameter: "NOXM", uOM: "MW", tolerance: "0.1"));
                EmParameters.HourlyEmissionsTolerancesCrossCheckTable.Add(new HourlyEmissionsTolerancesRow(parameter: "NOXR", uOM: "MW", tolerance: "0.005"));
                EmParameters.HourlyEmissionsTolerancesCrossCheckTable.Add(new HourlyEmissionsTolerancesRow(parameter: "OILM", uOM: "MW", tolerance: "0.5"));
                EmParameters.HourlyEmissionsTolerancesCrossCheckTable.Add(new HourlyEmissionsTolerancesRow(parameter: "SO2", uOM: "MW", tolerance: "0.1"));
                EmParameters.HourlyEmissionsTolerancesCrossCheckTable.Add(new HourlyEmissionsTolerancesRow(parameter: "SO2 Gas HPFF", uOM: "MW", tolerance: "0.0001"));
                EmParameters.HourlyEmissionsTolerancesCrossCheckTable.Add(new HourlyEmissionsTolerancesRow(parameter: "SO2 Oil HPFF", uOM: "MW", tolerance: "0.1"));
                EmParameters.HourlyEmissionsTolerancesCrossCheckTable.Add(new HourlyEmissionsTolerancesRow(parameter: "SO2C", uOM: "MW", tolerance: "0.1"));
                EmParameters.HourlyEmissionsTolerancesCrossCheckTable.Add(new HourlyEmissionsTolerancesRow(parameter: "SO2M", uOM: "MW", tolerance: "0.1"));
            }
            EmParameters.MonitoringPlanLocationRecords = new CheckDataView<VwMpMonitorLocationRow>();
            {
                EmParameters.MonitoringPlanLocationRecords.Add(new VwMpMonitorLocationRow(locationName: "CS12"));
                EmParameters.MonitoringPlanLocationRecords.Add(new VwMpMonitorLocationRow(locationName: "1"));
                EmParameters.MonitoringPlanLocationRecords.Add(new VwMpMonitorLocationRow(locationName: "2"));
                EmParameters.MonitoringPlanLocationRecords.Add(new VwMpMonitorLocationRow(locationName: "MS2"));
            }

            /* Initialize Output Parameters */
            category.SetCheckParameter("Location_Name_Array", new string[0]);
            EmParameters.MwLoadHourlyTolerance = null;


            /* Init Cateogry Result */
            category.CheckCatalogResult = null;

            /* Initialize variables needed to run the check. */
            bool log = false;
            string actual;

            /* Run Check */
            actual = target.HOURGEN24(category, ref log);


            /* Check Results */
            Assert.AreEqual(string.Empty, actual, string.Format("actual"));
            Assert.AreEqual(false, log, string.Format("log"));
            Assert.AreEqual(null, category.CheckCatalogResult, string.Format("category.CheckCatalogResult"));

            Assert.AreEqual(13, EmParameters.MwLoadHourlyTolerance, string.Format("MwLoadHourlyTolerance"));

            string[] lacationNameArray = (string[])category.GetCheckParameter("Location_Name_Array").ParameterValue;
            {
                Assert.AreNotEqual(null, lacationNameArray, string.Format("lacationNameArray"));
                Assert.AreEqual(4, lacationNameArray.Length, string.Format(" lacationNameArray.Length"));
                Assert.AreEqual("CS12", lacationNameArray[0], string.Format("lacationNameArray[0]"));
                Assert.AreEqual("1", lacationNameArray[1], string.Format("lacationNameArray[1]"));
                Assert.AreEqual("2", lacationNameArray[2], string.Format("lacationNameArray[2]"));
                Assert.AreEqual("MS2", lacationNameArray[3], string.Format("lacationNameArray[3]"));
            }
        }


        /// <summary>
        /// 
        /// </summary>
        [TestMethod()]
        public void HourGen25()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;

            cHourlyGeneralChecks target = new cHourlyGeneralChecks(new cEmissionsCheckParameters());


            /* InvalidCylinderIdList is null */
            {
                /* Initialize Input Parameters */
                EmParameters.InvalidCylinderIdList = null;


                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Initialize variables needed to run the check. */
                bool log = false;
                string actual;

                /* Run Check */
                actual = target.HOURGEN25(category, ref log);


                /* Check Results */
                Assert.AreEqual(string.Empty, actual, string.Format("actual [{0}]", "NullList"));
                Assert.AreEqual(false, log, string.Format("log [{0}]", "EmptyList"));
                Assert.AreEqual(null, category.CheckCatalogResult, string.Format("category.CheckCatalogResult [{0}]", "NullList"));

                Assert.AreEqual("", EmParameters.FormattedCylinderIdList, string.Format("InvalidCylinderIdList [{0}]", "NullList"));
            }


            /* InvalidCylinderIdList contains no items */
            {
                /* Initialize Input Parameters */
                EmParameters.InvalidCylinderIdList = new List<string>();


                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Initialize variables needed to run the check. */
                bool log = false;
                string actual;

                /* Run Check */
                actual = target.HOURGEN25(category, ref log);


                /* Check Results */
                Assert.AreEqual(string.Empty, actual, string.Format("actual [{0}]", "EmptyList"));
                Assert.AreEqual(false, log, string.Format("log [{0}]", "EmptyList"));
                Assert.AreEqual(null, category.CheckCatalogResult, string.Format("category.CheckCatalogResult [{0}]", "EmptyList"));

                Assert.AreEqual("", EmParameters.FormattedCylinderIdList, string.Format("InvalidCylinderIdList [{0}]", "EmptyList"));
            }


            /* InvalidCylinderIdList contains items not entered in alphabetical order */
            {
                /* Initialize Input Parameters */
                EmParameters.InvalidCylinderIdList = new List<string> { "OldId", "NewId", "AbcId" };


                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Initialize variables needed to run the check. */
                bool log = false;
                string actual;

                /* Run Check */
                actual = target.HOURGEN25(category, ref log);


                /* Check Results */
                Assert.AreEqual(string.Empty, actual, string.Format("actual [{0}]", "UnsortedList"));
                Assert.AreEqual(false, log, string.Format("log [{0}]", "UnsortedList"));
                Assert.AreEqual("A", category.CheckCatalogResult, string.Format("category.CheckCatalogResult [{0}]", "UnsortedList"));

                Assert.AreEqual("AbcId, NewId, and OldId", EmParameters.FormattedCylinderIdList, string.Format("InvalidCylinderIdList [{0}]", "UnsortedList"));
            }
        }
    }
}
