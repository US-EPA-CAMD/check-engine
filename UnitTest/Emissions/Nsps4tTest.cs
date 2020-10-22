using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Data.Ecmps.CheckEm.Function;
using ECMPS.Checks.Data.Ecmps.Dbo.View;
using ECMPS.Checks.Em.Parameters;
using ECMPS.Checks.EmissionsChecks;


namespace UnitTest.Emissions
{
    [TestClass]
    public class Nsps4tTest
    {

        /// <summary>
        /// 
        /// 
        /// CurrentMonitorPlanLocationRecord.MonLocId: Good
        /// 
        ///     MonLocId     : Good
        ///     LocationName : ONE
        /// 
        /// 
        /// | ## | LocName | Loc1   | Loc2   | PrgCd   | ClassCd | UMCB   | PrgE   || Result | Current || Note
        /// |  0 | ONE     | Bad    | Bad    | NSPS4T  | A       | PrdB   | null   || null   | null    || NSPS4T activebut no matching NSPS4T Summary.
        /// |  1 | ONE     | Good   | Bad    | NSPS4T  | A       | PrdB   | null   || null   | Good    || NSPS4T active and one matching NSPS4T Summary.
        /// |  2 | ONE     | Good   | Bad    | NSPS4T  | A       | PrdB   | PrdE   || null   | Good    || NSPS4T active and one matching NSPS4T Summary.  Program has ends with the reporting period.
        /// |  3 | ONE     | Good   | Bad    | NSPS4T  | A       | PrdE   | PrdE+1 || null   | Good    || NSPS4T active and one matching NSPS4T Summary.  Program begins the last day of the reporting period.
        /// |  4 | ONE     | Good   | Bad    | NSPS4T  | A       | PrdB-1 | PrdB   || null   | Good    || NSPS4T active and one matching NSPS4T Summary.  Program ends the first day of the reporting period.
        /// |  5 | ONE     | Good   | Bad    | MATS    | A       | PrdB   | null   || A      | null    || NSPS4T not activ, but one matching NSPS4T Summary.
        /// |  6 | ONE     | Good   | Bad    | NSPS4T  | N       | PrdB   | null   || A      | null    || NSPS4T not active due to class, but one matching NSPS4T Summary.
        /// |  7 | ONE     | Good   | Bad    | NSPS4T  | A       | PrdE+1 | PrdE+2 || A      | null    || NSPS4T not active because it began after the reporting period, but one matching NSPS4T Summary.
        /// |  8 | ONE     | Good   | Bad    | NSPS4T  | A       | PrdB-2 | PrdB-1 || A      | null    || NSPS4T not active because it ended before the reporting period, but one matching NSPS4T Summary.
        /// |  9 | ONE     | Good   | Good   | NSPS4T  | A       | PrdB   | null   || B      | null    || NSPS4T active, but two matching NSPS4T Summary.
        /// | 10 | CS1     | Good   | Bad    | NSPS4T  | A       | PrdB   | null   || C      | null    || NSPS4T active and one matching NSPS4T Summary, but summary at CS.
        /// | 11 | CP1     | Good   | Bad    | NSPS4T  | A       | PrdB   | null   || C      | null    || NSPS4T active and one matching NSPS4T Summary, but summary at CP.
        /// | 12 | MS1     | Good   | Bad    | NSPS4T  | A       | PrdB   | null   || C      | null    || NSPS4T active and one matching NSPS4T Summary, but summary at MS.
        /// | 13 | MP1     | Good   | Bad    | NSPS4T  | A       | PrdB   | null   || C      | null    || NSPS4T active and one matching NSPS4T Summary, but summary at MP.
        /// | 14 | CS1     | Bad    | Bad    | NSPS4T  | A       | PrdB   | null   || null   | null    || NSPS4T active for CS, but no matching NSPS4T Summary.
        /// | 15 | CP1     | Bad    | Bad    | NSPS4T  | A       | PrdB   | null   || null   | null    || NSPS4T active for CS, but no matching NSPS4T Summary.
        /// | 16 | MS1     | Bad    | Bad    | NSPS4T  | A       | PrdB   | null   || null   | null    || NSPS4T active for CS, but no matching NSPS4T Summary.
        /// | 17 | MP1     | Bad    | Bad    | NSPS4T  | A       | PrdB   | null   || null   | null    || NSPS4T active for CS, but no matching NSPS4T Summary.
        /// 
        /// Additional EmLocationProgramRecords :
        /// 
        /// | PrgCd   | ClassCd | UMCB | PrgE |
        /// | ARP     | A       | PrdB | null |
        /// | CSNOX   | A       | PrdB | null |
        /// | CSOSG1  | A       | PrdB | null |
        /// | CSOSG2  | A       | PrdB | null |
        /// | CSSO2G1 | A       | PrdB | null |
        /// | CSSO2G2 | A       | PrdB | null |
        /// </summary>
        [TestMethod]
        public void Nsps4t1()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;


            /* Input Parameter Values */
            DateTime prdB = new DateTime(2018, 1, 1, 0, 0, 0);
            DateTime prdE = new DateTime(2018, 3, 31, 0, 0, 0);

            string[] locNameList = { "ONE", "ONE", "ONE", "ONE", "ONE", "ONE", "ONE", "ONE", "ONE", "ONE", "CS", "CP", "MS", "MP", "CS", "CP", "MS", "MP" };
            string[] loc1List = { "Bad", "Good", "Good", "Good", "Good", "Good", "Good", "Good", "Good", "Good", "Good", "Good", "Good", "Good", "Bad", "Bad", "Bad", "Bad" };
            string[] loc2List = { "Bad", "Bad", "Bad", "Bad", "Bad", "Bad", "Bad", "Bad", "Bad", "Good", "Bad", "Bad", "Bad", "Bad", "Bad", "Bad", "Bad", "Bad" };
            string[] prgLocList = { "Good", "Good", "Good", "Good", "Good", "Good", "Good", "Good", "Good", "Good", "Good", "Good", "Good", "Good", "Good", "Good", "Good", "Good" };
            string[] prgCdList = { "NSPS4T", "NSPS4T", "NSPS4T", "NSPS4T", "NSPS4T", "MATS", "NSPS4T", "NSPS4T", "NSPS4T", "NSPS4T", "NSPS4T", "NSPS4T", "NSPS4T", "NSPS4T", "NSPS4T", "NSPS4T", "NSPS4T", "NSPS4T" };
            string[] classCdList = { "A", "A", "A", "A", "A", "A", "N", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A", "A" };
            DateTime?[] umcbList = { prdB, prdB, prdB, prdE, prdB.AddDays(-1), prdB, prdB, prdE.AddDays(1), prdB.AddDays(-2), prdB, prdB, prdB, prdB, prdB, prdB, prdB, prdB, prdB };
            DateTime?[] prgEList = { null, null, prdE, prdE.AddDays(1), prdB, null, null, prdE.AddDays(2), prdB.AddDays(-1), null, null, null, null, null, null, null, null, null };

            /* Expected Values */
            string[] expResultList = { null, null, null, null, null,  "A", "A", "A", "A", "B", "C", "C", "C", "C", null, null, null, null };
            string[] expLocList = { null, "Good", "Good", "Good", "Good", null, null, null, null, null, null, null, null, null, null, null, null, null };


            /* Test Case Count */
            int caseCount = 18;

            /* Check array lengths */
            Assert.AreEqual(caseCount, locNameList.Length, "loc1NameList length");
            Assert.AreEqual(caseCount, loc1List.Length, "loc1IdList length");
            Assert.AreEqual(caseCount, loc2List.Length, "loc2IdList length");
            Assert.AreEqual(caseCount, prgCdList.Length, "prgCdList length");
            Assert.AreEqual(caseCount, classCdList.Length, "classCdList length");
            Assert.AreEqual(caseCount, umcbList.Length, "umcbList length");
            Assert.AreEqual(caseCount, prgEList.Length, "prgEList length");
            Assert.AreEqual(caseCount, expResultList.Length, "expResultList length");
            Assert.AreEqual(caseCount, expLocList.Length, "expLocList length");


            /* Run Cases */
            for (int caseDex = 0; caseDex < caseCount; caseDex++)
            {
                /*  Initialize Input Parameters*/
                EmParameters.CurrentMonitorPlanLocationRecord = new VwCeMpMonitorLocationRow(monLocId: "Good", locationName: locNameList[caseDex]);
                EmParameters.CurrentReportingPeriodBeginDate = prdB;
                EmParameters.CurrentReportingPeriodEndDate = prdE;
                EmParameters.EmLocationProgramRecords = new CheckDataView<VwMpLocationProgramRow>
                (
                    new VwMpLocationProgramRow(prgCd: "ARP", classCd: "A", unitMonitorCertBeginDate: prdB, endDate: null, monLocId: "Good"),
                    new VwMpLocationProgramRow(prgCd: "CSNOX", classCd: "A", unitMonitorCertBeginDate: prdB, endDate: null, monLocId: "Good"),
                    new VwMpLocationProgramRow(prgCd: "CSOSG1", classCd: "A", unitMonitorCertBeginDate: prdB, endDate: null, monLocId: "Good"),
                    new VwMpLocationProgramRow(prgCd: "CSSO2G1", classCd: "A", unitMonitorCertBeginDate: prdB, endDate: null, monLocId: "Good"),
                    new VwMpLocationProgramRow(prgCd: prgCdList[caseDex], classCd: classCdList[caseDex], unitMonitorCertBeginDate: umcbList[caseDex], endDate: prgEList[caseDex], monLocId: prgLocList[caseDex]),
                    new VwMpLocationProgramRow(prgCd: "CSOSG2", classCd: "A", unitMonitorCertBeginDate: prdB, endDate: null, monLocId: "Good"),
                    new VwMpLocationProgramRow(prgCd: "CSSO2G2", classCd: "A", unitMonitorCertBeginDate: prdB, endDate: null, monLocId: "Good")
                );
                EmParameters.Nsps4tSummaryRecords = new CheckDataView<Nsps4tSummary>
                (
                    new Nsps4tSummary(monLocId: loc1List[caseDex]),
                    new Nsps4tSummary(monLocId: loc2List[caseDex])
                );


                /*  Initialize Output Parameters*/
                EmParameters.Nsps4tCurrentSummaryRecord = new Nsps4tSummary(monLocId: "Init");


                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Initialize variables needed to run the check. */
                bool log = false;
                string actual;

                /* Run Check */
                actual = Nsps4tChecks.NSPS4T1(category, ref log);

                /* Check Results */
                Assert.AreEqual(string.Empty, actual, string.Format("actual {0}", caseDex));
                Assert.AreEqual(false, log, string.Format("log {0}", caseDex));

                Assert.AreEqual(expResultList[caseDex], category.CheckCatalogResult, string.Format("category.CheckCatalogResult {0}", caseDex));
                Assert.AreEqual(expLocList[caseDex], (EmParameters.Nsps4tCurrentSummaryRecord == null ? null : EmParameters.Nsps4tCurrentSummaryRecord.MonLocId), string.Format("Nsps4tCurrentSummaryRecord {0}", caseDex));
            }
        }


        /// <summary>
        /// 
        /// 
        /// | ## | Ind  | CpCnt || Result | Cp1Null | Cp2Null | Cp3Null || Note
        /// |  0 | 1    | 0     || null   | true    | true    | true    || Indicator is 1 and no CP exist.
        /// |  1 | 1    | 1     || A      | true    | true    | true    || Indicator is 1, but a CP exists.
        /// |  2 | 0    | 0     || C      | true    | true    | true    || Indicator is 0, but no CP exist.
        /// |  3 | 0    | 1     || null   | false   | true    | true    || Indicator is 0 and one CP exist.
        /// |  4 | 0    | 2     || null   | false   | false   | true    || Indicator is 0 and two CP exist.
        /// |  5 | 0    | 3     || null   | false   | false   | false   || Indicator is 0 and three CP exist.
        /// |  6 | 0    | 4     || B      | true    | true    | true    || Indicator is 0, but four CP exist.
        /// |  7 | null | 4     || null   | true    | true    | true    || NSPS4T Suumary record does not exist, although more than three CP exist.
        /// 
        /// Compliance Period rows:
        /// 
        /// 1) Use Good as the NSPS4T_SUM_ID for the Summary and test Compliance Period rows.
        /// 2) Add Bad1 and Bad2 as NSPS4T_SUM_ID for additional Compliance Period rows.
        /// </summary>
        [TestMethod]
        public void Nsps4t2()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;


            /* Input Parameter Values */
            int?[] indList = { 1, 1, 0, 0, 0, 0, 0, null };
            int[] cpCntList = { 0, 1, 0, 1, 2, 3, 4, 4 };

            /* Expected Values */
            string[] expResultList = { null, "A", "C", null, null, null, "B", null };
            bool[] expCp1NullList = { true, true, true, false, false, false, true, true };
            bool[] expCp2NullList = { true, true, true, true, false, false, true, true };
            bool[] expCp3NullList = { true, true, true, true, true, false, true, true };


            /* Test Case Count */
            int caseCount = 8;

            /* Check array lengths */
            Assert.AreEqual(caseCount, indList.Length, "indList length");
            Assert.AreEqual(caseCount, cpCntList.Length, "cpCntList length");
            Assert.AreEqual(caseCount, expResultList.Length, "expResultList length");
            Assert.AreEqual(caseCount, expCp1NullList.Length, "expCp1LocList length");
            Assert.AreEqual(caseCount, expCp2NullList.Length, "expCp2LocList length");
            Assert.AreEqual(caseCount, expCp3NullList.Length, "expCp3LocList length");


            /* Run Cases */
            for (int caseDex = 0; caseDex < caseCount; caseDex++)
            {
                /*  Initialize Input Parameters*/
                EmParameters.Nsps4tCompliancePeriodRecords = new CheckDataView<Nsps4tCompliancePeriod>();
                {
                    EmParameters.Nsps4tCompliancePeriodRecords.Add(new Nsps4tCompliancePeriod(nsps4tSumId: "Bad1", nsps4tCmpId: "Bad1Cmp"));

                    for (int dex = 0; dex < cpCntList[caseDex]; dex++)
                        EmParameters.Nsps4tCompliancePeriodRecords.Add(new Nsps4tCompliancePeriod(nsps4tSumId: "Good", nsps4tCmpId: string.Format("CP{0}", dex + 1)));

                    EmParameters.Nsps4tCompliancePeriodRecords.Add(new Nsps4tCompliancePeriod(nsps4tSumId: "Bad2", nsps4tCmpId: "Bad2Cmp"));
                }
                EmParameters.Nsps4tCurrentSummaryRecord = (indList[caseDex] != null ? new Nsps4tSummary(nsps4tSumId: "Good", noPeriodEndedInd: indList[caseDex]) : null);


                /*  Initialize Output Parameters*/
                EmParameters.Nsps4tCurrentCompliancePeriod1Record = new Nsps4tCompliancePeriod(nsps4tSumId: "Init");
                EmParameters.Nsps4tCurrentCompliancePeriod2Record = new Nsps4tCompliancePeriod(nsps4tSumId: "Init");
                EmParameters.Nsps4tCurrentCompliancePeriod3Record = new Nsps4tCompliancePeriod(nsps4tSumId: "Init");


                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Initialize variables needed to run the check. */
                bool log = false;
                string actual;

                /* Run Check */
                actual = Nsps4tChecks.NSPS4T2(category, ref log);

                /* Check Results */
                Assert.AreEqual(string.Empty, actual, string.Format("actual {0}", caseDex));
                Assert.AreEqual(false, log, string.Format("log {0}", caseDex));

                Assert.AreEqual(expResultList[caseDex], category.CheckCatalogResult, string.Format("category.CheckCatalogResult {0}", caseDex));
                Assert.AreEqual(expCp1NullList[caseDex], EmParameters.Nsps4tCurrentCompliancePeriod1Record == null, string.Format("Nsps4tCurrentCompliancePeriod1Record {0}", caseDex));
                Assert.AreEqual(expCp2NullList[caseDex], EmParameters.Nsps4tCurrentCompliancePeriod2Record == null, string.Format("Nsps4tCurrentCompliancePeriod2Record {0}", caseDex));
                Assert.AreEqual(expCp3NullList[caseDex], EmParameters.Nsps4tCurrentCompliancePeriod3Record == null, string.Format("Nsps4tCurrentCompliancePeriod3Record {0}", caseDex));
            }
        }


        /// <summary>
        /// 
        /// 
        /// | ## | Qtr  | SumExists | RowCnt || Result | expCurrent || Note
        /// |  0 | 4    | false     | 2      || null   | null       || NSPS4T Suumary record does not exist, although more than one annual rows exist.
        /// |  1 | 1    | true      | 0      || null   | null       || First quarter and no annual rows exist.
        /// |  2 | 2    | true      | 0      || null   | null       || Second quarter and no annual rows exist.
        /// |  3 | 3    | true      | 0      || null   | null       || Third quarter and no annual rows exist.
        /// |  4 | 1    | true      | 1      || A      | null       || First quarter, but an annual ros exist.
        /// |  5 | 2    | true      | 1      || A      | null       || Second quarter, but an annual row exist.
        /// |  6 | 3    | true      | 1      || A      | null       || Third quarter, but an annual row exist.
        /// |  7 | 4    | true      | 2      || B      | null       || Fourth quarter, but more than one annual row exists.
        /// |  8 | 4    | true      | 0      || C      | null       || Fourth quarter, but no annual row exists.
        /// |  9 | 4    | true      | 1      || null   | Ann1       || Fourth quarter and one annual row exists.
        /// 
        /// Annual rows:
        /// 
        /// 1) Use Good as the NSPS4T_SUM_ID for the Summary and test Compliance Period rows.
        /// 2) Add Bad1 and Bad2 as NSPS4T_SUM_ID for additional Compliance Period rows.
        /// </summary>
        [TestMethod]
        public void Nsps4t3()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;


            /* Input Parameter Values */
            int[] qtrList = { 4, 1, 2, 3, 1, 2, 3, 4, 4, 4 };
            bool[] sumExistsList = { false, true, true, true, true, true, true, true, true, true };
            int[] rowCntList = { 2, 0, 0, 0, 1, 1, 1, 2, 0, 1 };

            /* Expected Values */
            string[] expResultList = { null, null, null, null, "A", "A", "A", "B", "C", null };
            string[] expCurrentList = { null, null, null, null, null, null, null, null, null, "Ann1" };


            /* Test Case Count */
            int caseCount = 10;

            /* Check array lengths */
            Assert.AreEqual(caseCount, qtrList.Length, "qtrList length");
            Assert.AreEqual(caseCount, sumExistsList.Length, "sumExistsList length");
            Assert.AreEqual(caseCount, rowCntList.Length, "rowCntList length");
            Assert.AreEqual(caseCount, expResultList.Length, "expResultList length");
            Assert.AreEqual(caseCount, expCurrentList.Length, "expCurrentList length");


            /* Run Cases */
            for (int caseDex = 0; caseDex < caseCount; caseDex++)
            {
                /*  Initialize Input Parameters*/
                EmParameters.CurrentReportingPeriodQuarter = qtrList[caseDex];
                EmParameters.Nsps4tAnnualRecords = new CheckDataView<Nsps4tAnnual>();
                {
                    EmParameters.Nsps4tAnnualRecords.Add(new Nsps4tAnnual(nsps4tSumId: "Bad1", nsps4tAnnId: "Bad1Ann"));

                    for (int dex = 0; dex < rowCntList[caseDex]; dex++)
                        EmParameters.Nsps4tAnnualRecords.Add(new Nsps4tAnnual(nsps4tSumId: "Good", nsps4tAnnId: string.Format("Ann{0}", dex + 1)));

                    EmParameters.Nsps4tAnnualRecords.Add(new Nsps4tAnnual(nsps4tSumId: "Bad2", nsps4tAnnId: "Bad2Ann"));
                }
                EmParameters.Nsps4tCurrentSummaryRecord = (sumExistsList[caseDex] ? new Nsps4tSummary(nsps4tSumId: "Good") : null);


                /*  Initialize Output Parameters*/
                EmParameters.Nsps4tCurrentAnnualRecord = new Nsps4tAnnual(nsps4tSumId: "Init");


                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Initialize variables needed to run the check. */
                bool log = false;
                string actual;

                /* Run Check */
                actual = Nsps4tChecks.NSPS4T3(category, ref log);

                /* Check Results */
                Assert.AreEqual(string.Empty, actual, string.Format("actual {0}", caseDex));
                Assert.AreEqual(false, log, string.Format("log {0}", caseDex));

                Assert.AreEqual(expResultList[caseDex], category.CheckCatalogResult, string.Format("category.CheckCatalogResult {0}", caseDex));
                Assert.AreEqual(expCurrentList[caseDex], (EmParameters.Nsps4tCurrentAnnualRecord == null ? null : EmParameters.Nsps4tCurrentAnnualRecord.Nsps4tAnnId), string.Format("Nsps4tCurrentAnnualRecord {0}", caseDex));
            }
        }


        /// <summary>
        /// 
        /// 
        /// | ## | SumExists | Standard | Reported || Result || Note
        /// |  0 | false     | null     | null     || null   || NSPS4T Summary row does not exist.
        /// |  1 | true      | null     | null     || null   || NSPS4T standard and reported are null.
        /// |  2 | true      | null     | GROSS    || null   || NSPS4T standard is null and reported is GROSS.
        /// |  3 | true      | null     | NET      || null   || NSPS4T standard is null and reported is NET.
        /// |  4 | true      | GROSS    | null     || A      || NSPS4T standard is GROSS and reported is null.
        /// |  5 | true      | GROSS    | GROSS    || null   || NSPS4T standard and reported are GROSS.
        /// |  6 | true      | GROSS    | NET      || A      || NSPS4T standard is GROSS and reported is NET.
        /// |  7 | true      | NET      | null     || A      || NSPS4T standard is NET and reported is null.
        /// |  8 | true      | NET      | GROSS    || A      || NSPS4T standard is NET and reported is GROSS.
        /// |  9 | true      | NET      | NET      || null   || NSPS4T standard and reported are NET.
        /// </summary>
        [TestMethod]
        public void Nsps4t4()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;


            /* Input Parameter Values */
            bool[] sumExistsList = { false, true, true, true, true, true, true, true, true, true };
            string[] standardList = { null, null, null, null, "GROSS", "GROSS", "GROSS", "NET", "NET", "NET" };
            string[] reportedList = { null, null, "GROSS", "NET", null, "GROSS", "NET", null, "GROSS", "NET" };

            /* Expected Values */
            string[] expResultList = { null, null, null, null, "A", null, "A", "A", "A", null };


            /* Test Case Count */
            int caseCount = 10;

            /* Check array lengths */
            Assert.AreEqual(caseCount, sumExistsList.Length, "sumExistsList length");
            Assert.AreEqual(caseCount, standardList.Length, "standardList length");
            Assert.AreEqual(caseCount, reportedList.Length, "reportedList length");
            Assert.AreEqual(caseCount, expResultList.Length, "expResultList length");


            /* Run Cases */
            for (int caseDex = 0; caseDex < caseCount; caseDex++)
            {
                /*  Initialize Input Parameters*/
                EmParameters.Nsps4tCurrentSummaryRecord = (sumExistsList[caseDex] ? new Nsps4tSummary(nsps4tSumId: "Good", emissionStandardLoadCd: standardList[caseDex], electricalLoadCd: reportedList[caseDex]) : null);


                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Initialize variables needed to run the check. */
                bool log = false;
                string actual;

                /* Run Check */
                actual = Nsps4tChecks.NSPS4T4(category, ref log);

                /* Check Results */
                Assert.AreEqual(string.Empty, actual, string.Format("actual {0}", caseDex));
                Assert.AreEqual(false, log, string.Format("log {0}", caseDex));

                Assert.AreEqual(expResultList[caseDex], category.CheckCatalogResult, string.Format("category.CheckCatalogResult {0}", caseDex));
            }
        }


        /// <summary>
        /// 
        /// 
        ///                                     |       CP1      |       CP2      |       CP3      ||        |                                               ||
        /// | ## | SumExists | Standard | Uom   | Exists | Uom1  | Exists | Uom2  | Exists | Uom3  || Result | Invalid                                       || Note
        /// |  0 | false     | null     | null  | true   | KGMWH | true   | MMBTU | true   | LBMWH || null   | empty                                         || NSPS4t Summary does not exist, and three Compliance Period rows exist (just for testing).
        /// |  1 | true      | null     | null  | true   | KGMWH | true   | MMBTU | true   | LBMWH || null   | empty                                         || NSPS4t Summary does exists with null Standard UOM, and three Compliance Period rows exist with reported UOM.
        /// |  2 | true      | null     | KGMWH | true   | KGMWH | true   | MMBTU | true   | LBMWH || A      | mmBtu and Pounds / Megawatt Hour              || NSPS4t Summary does exists with KGMWH Standard UOM, and three Compliance Period rows exist with different reported UOM.
        /// |  3 | true      | null     | MMBTU | true   | KGMWH | true   | MMBTU | true   | LBMWH || A      | kg / Megawatt Hour and Pounds / Megawatt Hour || NSPS4t Summary does exists with MMBTU Standard UOM, and three Compliance Period rows exist with different reported UOM.
        /// |  4 | true      | null     | LBMWH | true   | KGMWH | true   | MMBTU | true   | LBMWH || A      | kg / Megawatt Hour and mmBtu                  || NSPS4t Summary does exists with LBMWH Standard UOM, and three Compliance Period rows exist with different reported UOM.
        /// |  5 | true      | null     | KGMWH | true   | MMBTU | true   | MMBTU | true   | MMBTU || A      | mmBtu                                         || Some discrepant value used multiple times but only returned once.
        /// |  6 | true      | null     | KGMWH | false  | null  | false  | null  | false  | null  || null   | null                                          || NSPS4t Summary does exists, but no Compliance Period rows exist.
        /// |  7 | true      | null     | KGMWH | true   | null  | false  | null  | false  | null  || null   | null                                          || NSPS4t Summary does exists and one Compliance Period, but its UOM is null.
        /// |  8 | true      | null     | KGMWH | true   | KGMWH | true   | null  | false  | null  || null   | null                                          || NSPS4t Summary does exists and two Compliance Period, but second UOM is null.
        /// |  9 | true      | null     | KGMWH | true   | KGMWH | true   | KGMWH | true   | null  || null   | null                                          || NSPS4t Summary does exists and three Compliance Period, but third UOM is null.
        /// | 10 | true      | null     | KGMWH | true   | KGMWH | true   | KGMWH | true   | KGMWH || null   | null                                          || NSPS4t Summary does exists and three Compliance Period with matching UOM.
        /// 
        /// | 11 | true      | MODUS    | null  | true   | KGMWH | true   | KGMWH | true   | KGMWH || null   | empty                                         || NSPS4t Summary does exists with MODUS standard and three Compliance Period with matching UOM.
        /// | 12 | true      | MODUS    | null  | true   | MMBTU | true   | MMBTU | true   | MMBTU || null   | empty                                         || NSPS4t Summary does exists with MODUS standard and three Compliance Period with matching UOM.
        /// | 13 | true      | MODUS    | null  | true   | LBMWH | true   | LBMWH | true   | LBMWH || null   | empty                                         || NSPS4t Summary does exists with MODUS standard and three Compliance Period with matching UOM.
        /// | 14 | true      | MODUS    | null  | true   | KGMWH | true   | MMBTU | true   | KGMWH || B      | empty                                         || NSPS4t Summary does exists with MODUS standard and three Compliance Period with one unmatched UOM.
        /// | 15 | true      | MODUS    | null  | true   | KGMWH | true   | KGMWH | true   | LBMWH || B      | empty                                         || NSPS4t Summary does exists with MODUS standard and three Compliance Period with one unmatched UOM.
        /// | 16 | true      | MODUS    | null  | true   | MMBTU | true   | MMBTU | true   | KGMWH || B      | empty                                         || NSPS4t Summary does exists with MODUS standard and three Compliance Period with one unmatched UOM.
        /// | 17 | true      | MODUS    | null  | true   | MMBTU | true   | LBMWH | true   | MMBTU || B      | empty                                         || NSPS4t Summary does exists with MODUS standard and three Compliance Period with one unmatched UOM.
        /// | 18 | true      | MODUS    | null  | true   | LBMWH | true   | KGMWH | true   | LBMWH || B      | empty                                         || NSPS4t Summary does exists with MODUS standard and three Compliance Period with one unmatched UOM.
        /// | 19 | true      | MODUS    | null  | true   | LBMWH | true   | LBMWH | true   | MMBTU || B      | empty                                         || NSPS4t Summary does exists with MODUS standard and three Compliance Period with one unmatched UOM.
        /// | 20 | true      | MODUS    | null  | true   | KGMWH | true   | KGMWH | false  | null  || null   | empty                                         || NSPS4t Summary does exists with MODUS standard and two Compliance Period with matching UOM.
        /// | 21 | true      | MODUS    | null  | true   | KGMWH | true   | MMBTU | false  | null  || B      | empty                                         || NSPS4t Summary does exists with MODUS standard and two Compliance Period with unmatched UOM.
        /// | 22 | true      | MODUS    | null  | true   | KGMWH | false  | null  | false  | null  || null   | empty                                         || NSPS4t Summary does exists with MODUS standard and one Compliance Period.
        /// </summary>
        [TestMethod]
        public void Nsps4t5()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;


            /* Input Parameter Values */
            bool[] sumExistsList = { false, true, true, true, true, true, true, true, true, true, true,
                                     true, true, true, true, true, true, true, true, true, true, true, true };
            string[] standardList = { null, null, null, null, null, null, null, null, null, null, null,
                                      "MODUS", "MODUS", "MODUS", "MODUS", "MODUS", "MODUS", "MODUS", "MODUS", "MODUS", "MODUS", "MODUS", "MODUS" };
            string[] uomList = { null, null , "KGMWH", "MMBTU", "LBMWH", "KGMWH", "KGMWH", "KGMWH", "KGMWH", "KGMWH", "KGMWH",
                                 null, null, null, null, null, null, null, null, null, null, null, null };
            bool[] cp1ExistsList = { true, true, true, true, true, true, false, true, true, true, true,
                                     true, true, true, true, true, true, true, true, true, true, true, true };
            string[] cp1ReportedList = { "KGMWH", "KGMWH", "KGMWH", "KGMWH", "KGMWH", "MMBTU", null, null, "KGMWH", "KGMWH", "KGMWH",
                                         "KGMWH", "MMBTU", "LBMWH", "KGMWH", "KGMWH", "MMBTU", "MMBTU", "LBMWH", "LBMWH", "KGMWH", "KGMWH", "KGMWH" };
            string[] cp1LabelList = { "kg / Megawatt Hour", "kg / Megawatt Hour", "kg / Megawatt Hour", "kg / Megawatt Hour", "kg / Megawatt Hour", "mmBtu", null, null, "kg / Megawatt Hour", "kg / Megawatt Hour", "kg / Megawatt Hour",
                                      "kg / Megawatt Hour", "mmBtu", "Pounds / Megawatt Hour", "kg / Megawatt Hour", "kg / Megawatt Hour", "mmBtu", "mmBtu", "Pounds / Megawatt Hour", "Pounds / Megawatt Hour", "kg / Megawatt Hour", "kg / Megawatt Hour", "kg / Megawatt Hour" };
            bool[] cp2ExistsList = { true, true, true, true, true, true, false, false, true, true, true,
                                     true, true, true, true, true, true, true, true, true, true, true, false  };
            string[] cp2ReportedList = { "MMBTU", "MMBTU", "MMBTU", "MMBTU", "MMBTU", "MMBTU", null, null, null, "KGMWH", "KGMWH",
                                         "KGMWH", "MMBTU", "LBMWH", "MMBTU", "KGMWH", "MMBTU", "LBMWH", "KGMWH", "LBMWH", "KGMWH", "MMBTU", null };
            string[] cp2LabelList = { "mmBtu", "mmBtu", "mmBtu", "mmBtu", "mmBtu", "mmBtu", null, null, null, "kg / Megawatt Hour", "kg / Megawatt Hour",
                                      "kg / Megawatt Hour", "mmBtu", "Pounds / Megawatt Hour", "mmBtu", "kg / Megawatt Hour", "mmBtu", "Pounds / Megawatt Hour", "kg / Megawatt Hour", "Pounds / Megawatt Hour", "kg / Megawatt Hour", "mmBtu", null };
            bool[] cp3ExistsList = { true, true, true, true, true, true, false, false, false, true, true,
                                     true, true, true, true, true, true, true, true, true, false, false, false };
            string[] cp3ReportedList = { "LBMWH", "LBMWH", "LBMWH", "LBMWH", "LBMWH", "MMBTU", null, null, null, null, "KGMWH",
                                         "KGMWH", "MMBTU", "LBMWH", "KGMWH", "LBMWH", "KGMWH", "MMBTU", "LBMWH", "MMBTU", null, null, null };
            string[] cp3LabelList = { "Pounds / Megawatt Hour", "Pounds / Megawatt Hour", "Pounds / Megawatt Hour", "Pounds / Megawatt Hour", "Pounds / Megawatt Hour", "mmBtu", null, null, null, null, "kg / Megawatt Hour",
                                      "kg / Megawatt Hour", "mmBtu", "Pounds / Megawatt Hour", "kg / Megawatt Hour", "Pounds / Megawatt Hour", "kg / Megawatt Hour", "mmBtu", "Pounds / Megawatt Hour", "mmBtu", null, null, null };

            /* Expected Values */
            string[] expResultList = { null, null, "A", "A", "A", "A", null, null, null, null, null,
                                       null, null, null, "B", "B", "B", "B", "B", "B", null, "B", null };
            string[] expInvalidList = { "", "", "mmBtu and Pounds / Megawatt Hour", "kg / Megawatt Hour and Pounds / Megawatt Hour", "kg / Megawatt Hour and mmBtu", "mmBtu", "", "", "", "", "",
                                        "", "", "", "", "", "", "", "", "", "", "", "" };


            /* Test Case Count */
            int caseCount = 23;

            /* Check array lengths */
            Assert.AreEqual(caseCount, sumExistsList.Length, "sumExistsList length");
            Assert.AreEqual(caseCount, standardList.Length, "standardList length");
            Assert.AreEqual(caseCount, uomList.Length, "uomList length");
            Assert.AreEqual(caseCount, cp1ExistsList.Length, "cp1ExistsList length");
            Assert.AreEqual(caseCount, cp1ReportedList.Length, "cp1ReportedList length");
            Assert.AreEqual(caseCount, cp1LabelList.Length, "cp1LabelList length");
            Assert.AreEqual(caseCount, cp2ExistsList.Length, "cp2ExistsList length");
            Assert.AreEqual(caseCount, cp2ReportedList.Length, "cp2ReportedList length");
            Assert.AreEqual(caseCount, cp2LabelList.Length, "cp2LabelList length");
            Assert.AreEqual(caseCount, cp3ExistsList.Length, "cp3ExistsList length");
            Assert.AreEqual(caseCount, cp3ReportedList.Length, "cp3ReportedList length");
            Assert.AreEqual(caseCount, cp3LabelList.Length, "cp3LabelList length");
            Assert.AreEqual(caseCount, expResultList.Length, "expResultList length");
            Assert.AreEqual(caseCount, expInvalidList.Length, "expInvalidList length");


            /* Run Cases */
            for (int caseDex = 0; caseDex < caseCount; caseDex++)
            {
                /*  Initialize Input Parameters*/
                EmParameters.Nsps4tCurrentCompliancePeriod1Record = (cp1ExistsList[caseDex] ? new Nsps4tCompliancePeriod(co2EmissionRateUomCd: cp1ReportedList[caseDex], co2EmissionRateUomLabel: cp1LabelList[caseDex]) : null);
                EmParameters.Nsps4tCurrentCompliancePeriod2Record = (cp2ExistsList[caseDex] ? new Nsps4tCompliancePeriod(co2EmissionRateUomCd: cp2ReportedList[caseDex], co2EmissionRateUomLabel: cp2LabelList[caseDex]) : null);
                EmParameters.Nsps4tCurrentCompliancePeriod3Record = (cp3ExistsList[caseDex] ? new Nsps4tCompliancePeriod(co2EmissionRateUomCd: cp3ReportedList[caseDex], co2EmissionRateUomLabel: cp3LabelList[caseDex]) : null);
                EmParameters.Nsps4tCurrentSummaryRecord = (sumExistsList[caseDex] ? new Nsps4tSummary(nsps4tSumId: "Good", emissionStandardCd: standardList[caseDex], emissionStandardUomCd: uomList[caseDex]) : null);


                /*  Initialize Output Parameters*/
                EmParameters.Nsps4tInvalidCo2EmissionRateUomList = "Bad List";


                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Initialize variables needed to run the check. */
                bool log = false;
                string actual;

                /* Run Check */
                actual = Nsps4tChecks.NSPS4T5(category, ref log);

                /* Check Results */
                Assert.AreEqual(string.Empty, actual, string.Format("actual {0}", caseDex));
                Assert.AreEqual(false, log, string.Format("log {0}", caseDex));

                Assert.AreEqual(expResultList[caseDex], category.CheckCatalogResult, string.Format("category.CheckCatalogResult {0}", caseDex));
                Assert.AreEqual(expInvalidList[caseDex], EmParameters.Nsps4tInvalidCo2EmissionRateUomList, string.Format("Nsps4tInvalidCo2EmissionRateUomList {0}", caseDex));
            }
        }
    }
}
