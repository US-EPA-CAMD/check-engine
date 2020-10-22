using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections.Generic;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.CheckEngine.SpecialParameterClasses;
using ECMPS.Checks.EmissionsChecks;
using ECMPS.Checks.Data.Ecmps.CheckEm.Function;
using ECMPS.Checks.Data.Ecmps.Dbo.View;
using ECMPS.Checks.Em.Parameters;


namespace UnitTest.Emissions
{

    [TestClass]
    public class EmissionsAuditUnitTests
    {

        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            //populates Reporting Period Table for checks without making db call
            UnitTest.UtilityClasses.UnitTestExtensions.SetReportingPeriodTable();
        }


        /// <summary>
        /// 
        /// Location Position Lookup parameter:
        /// 
        ///     | ID  | Position |
        ///     | UNA |        0 |
        ///     | DUE |        1 |
        ///     
        /// Component Operating Data Dictionary Array
        /// 
        ///     Each location will contain three compoents.
        /// 
        ///     | Loc | Cmp1 | Cmp2 |
        ///     | UNA | AAA  | BBB  |
        ///     | DUE | CCC  | DDD  |
        /// 
        /// Current Reporting Period Object (Year: 2020)
        /// 
        ///     | Qtr |  Id |
        ///     |   1 | 109 |
        ///     |   2 | 110 |
        ///     |   3 | 111 |
        ///     |   4 | 112 |
        /// 
        /// Cases:
        /// 
        ///                                |                    Target                     | Other  ||
        /// | ## | Qtr | Loc | Cmp | Ident | Cmp | HrsDict | HrsQ1 | HrsQ2 | HrsQ3 | HrsQ4 | Counts || Result | Hours || Note
        /// |  0 |   1 | UNA | AAA | LA1   | AAA |     721 |  null |  null |  null |  null |    721 || null   |  null || Not a like-kind analyzer.
        /// |  1 |   1 | UNA | AAA | LKA   | AAA |     721 |  null |  null |  null |  null |    720 || A      |   721 || Like-kind used for 721 in the current quarter.
        /// |  2 |   1 | UNA | AAA | LKA   | BBB |     721 |  null |  null |  null |  null |    720 || B      |   720 || Like-kind used for 720 in the current quarter. Matched Other.
        /// |  3 |   1 | UNA | AAA | LKA   | CCC |     721 |  null |  null |  null |  null |      0 || null   |  null || Like-kind not used (recorded) for the current quarter. No match.
        /// |  4 |   2 | UNA | AAA | LKA   | AAA |     720 |     1 |  null |  null |  null |      0 || A      |   721 || Like-kind used for 721 total hours.
        /// |  5 |   2 | UNA | AAA | LKA   | AAA |     719 |     1 |  null |  null |  null |      0 || B      |   720 || Like-kind used for 720 total hours.
        /// |  6 |   2 | UNA | AAA | LKA   | AAA |       1 |   721 |  null |  null |  null |      0 || A      |   722 || Like-kind used for 722 total hours.
        /// |  7 |   2 | UNA | AAA | LKA   | AAA |       0 |   721 |  null |  null |  null |    721 || null   |  null || Like-kind was not used in the current quarter.
        /// |  8 |   2 | UNA | AAA | LKA   | AAA |     360 |   360 |   721 |   721 |   721 |    721 || B      |   720 || Like-kind used for 720 hours in Q1 and Q2.
        /// |  9 |   2 | UNA | AAA | LKA   | AAA |     361 |   360 |   721 |   721 |   721 |      0 || A      |   721 || Like-kind used for 721 hours in Q1 and Q2.
        /// | 10 |   3 | UNA | AAA | LKA   | AAA |     240 |   239 |   241 |   721 |   721 |    721 || B      |   720 || Like-kind used for 720 hours in Q1, Q2 and Q3.
        /// | 11 |   3 | UNA | AAA | LKA   | AAA |     241 |   239 |   241 |   721 |   721 |      0 || A      |   721 || Like-kind used for 721 hours in Q1, Q2 and Q3.
        /// | 12 |   4 | UNA | AAA | LKA   | AAA |     180 |   179 |   180 |   181 |   721 |    721 || B      |   720 || Like-kind used for 720 hours in Q1, Q2, Q3 and Q4
        /// | 13 |   4 | UNA | AAA | LKA   | AAA |     181 |   179 |   180 |   181 |   721 |      0 || A      |   721 || Like-kind used for 721 hours in Q1, Q2, Q3 and Q4.
        /// | 14 |   1 | DUE | DDD | LKD   | DDD |     721 |  null |  null |  null |  null |    720 || A      |   721 || Like-kind used for 721 in the current quarter.
        /// | 15 |   1 | DUE | EEE | LKD   | DDD |     721 |  null |  null |  null |  null |    720 || null   |  null || Component does not exist in dictionary.
        /// </summary>
        [TestMethod]
        public void Audit1()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            EmParameters.Init(category.Process);
            EmParameters.Category = category;


            /* Input Parameter Values */
            int[] quarterList = { 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 3, 3, 4, 4, 1, 1 };
            string[] locIdList = { "UNA", "UNA", "UNA", "UNA", "UNA", "UNA", "UNA", "UNA", "UNA", "UNA", "UNA", "UNA", "UNA", "UNA", "DUE", "DUE" };
            string[] compIdList = { "AAA", "AAA", "AAA", "AAA", "AAA", "AAA", "AAA", "AAA", "AAA", "AAA", "AAA", "AAA", "AAA", "AAA", "DDD", "EEE" };
            string[] compIdentList = { "LA1", "LKA", "LKA", "LKA", "LKA", "LKA", "LKA", "LKA", "LKA", "LKA", "LKA", "LKA", "LKA", "LKA", "LKD", "LKD" };

            string[] tarCompIdList = { "AAA", "AAA", "BBB", "CCC", "AAA", "AAA", "AAA", "AAA", "AAA", "AAA", "AAA", "AAA", "AAA", "AAA", "DDD", "DDD" };
            ushort[] tarHrsDictList = { 721, 721, 721, 721, 720, 719, 1, 0, 360, 361, 240, 241, 180, 181, 721, 721 };
            short?[] tarHrsQ1List = { null, null, null, null, 1, 1, 721, 721, 360, 360, 239, 239, 179, 179, null, null };
            short?[] tarHrsQ2List = { null, null, null, null, null, null, null, null, 721, 721, 241, 241, 180, 180, null, null };
            short?[] tarHrsQ3List = { null, null, null, null, null, null, null, null, 721, 721, 721, 721, 181, 181, null, null };
            short?[] tarHrsQ4List = { null, null, null, null, null, null, null, null, 721, 721, 721, 721, 721, 721, null, null };

            short[] otherShortHrsList = { 721, 720, 720, 0, 0, 0, 0, 721, 721, 0, 721, 0, 721, 0, 720, 720 };
            ushort[] otherUshortHrsList = { 721, 720, 720, 0, 0, 0, 0, 721, 721, 0, 721, 0, 721, 0, 720, 720 };

            /* Expected Values */
            string[] expectedResultList = { null, "A", "B", null, "A", "B", "A", null, "B", "A", "B", "A", "B", "A", "A", null };
            int?[] expectedHrsList = { null, 721, 720, null, 721, 720, 722, null, 720, 721, 720, 721, 720, 721, 721, null };

            /* Test Case Count */
            int caseCount = 16;

            /* Check array lengths */
            Assert.AreEqual(caseCount, quarterList.Length, "quarterList length");
            Assert.AreEqual(caseCount, locIdList.Length, "locIdList length");
            Assert.AreEqual(caseCount, compIdList.Length, "compIdList length");
            Assert.AreEqual(caseCount, compIdentList.Length, "compIdentList length");
            Assert.AreEqual(caseCount, tarCompIdList.Length, "tarCompIdList length");
            Assert.AreEqual(caseCount, tarHrsDictList.Length, "tarHrsDictList length");
            Assert.AreEqual(caseCount, tarHrsQ1List.Length, "tarHrsQ1List length");
            Assert.AreEqual(caseCount, tarHrsQ2List.Length, "tarHrsQ2List length");
            Assert.AreEqual(caseCount, tarHrsQ3List.Length, "tarHrsQ3List length");
            Assert.AreEqual(caseCount, tarHrsQ4List.Length, "tarHrsQ4List length");
            Assert.AreEqual(caseCount, otherShortHrsList.Length, "otherShortHrsList length");
            Assert.AreEqual(caseCount, otherUshortHrsList.Length, "otherUshortHrsList length");
            Assert.AreEqual(caseCount, expectedResultList.Length, "expectedResultList length");
            Assert.AreEqual(caseCount, expectedHrsList.Length, "expectedHrsList length");

            /* Helper Variables */
            string[] componentIdArray = { "AAA", "BBB", "CCC", "DDD" };
            Dictionary<string, string> componentToLocation = new Dictionary<string, string> { { "AAA", "UNA" }, { "BBB", "UNA" }, { "CCC", "DUE" }, { "DDD", "DUE" } };

            /* Run Cases */
            for (int caseDex = 0; caseDex < caseCount; caseDex++)
            {
                /* Initialize Helper Variables */
                Dictionary<string, int> locationPositionLookup = new Dictionary<string, int> { { "UNA", 0 }, { "DUE", 1 } };

                /*  Initialize Required Parameters */
                string tarCompId = tarCompIdList[caseDex];

                EmParameters.ComponentOperatingSuppDataDictionaryArray = new Dictionary<string, ComponentOperatingSupplementalData>[2];
                {
                    EmParameters.ComponentOperatingSuppDataDictionaryArray[0] = new Dictionary<string, ComponentOperatingSupplementalData>();
                    EmParameters.ComponentOperatingSuppDataDictionaryArray[1] = new Dictionary<string, ComponentOperatingSupplementalData>();

                    foreach (string componentId in componentIdArray)
                    {
                        if (componentToLocation.ContainsKey(componentId))
                        {
                            string monLocId = componentToLocation[componentId];
                            int monLocPos = locationPositionLookup[monLocId];

                            EmParameters.ComponentOperatingSuppDataDictionaryArray[monLocPos].Add(componentId, new ComponentOperatingSupplementalData(0, componentId, monLocId));
                            EmParameters.ComponentOperatingSuppDataDictionaryArray[monLocPos][componentId].ResetValuesForTesting(otherUshortHrsList[caseDex], otherUshortHrsList[caseDex]);

                            if (componentId == tarCompIdList[caseDex])
                            {
                                EmParameters.ComponentOperatingSuppDataDictionaryArray[monLocPos][componentId].ResetValuesForTesting(otherUshortHrsList[caseDex], tarHrsDictList[caseDex],
                                                                                                                                     eOperatingSupplementalDataTimePeriod.Quarterly,
                                                                                                                                     eOperatingSupplementalDataCountType.Operating);
                            }
                        }
                    }
                }
                EmParameters.ComponentOperatingSuppDataRecordsForMpAndYear = new CheckDataView<ComponentOpSuppData>();
                {
                    short otherHrs = otherShortHrsList[caseDex];

                    for (int quarter = 1; quarter <= quarterList[caseDex]; quarter += 1)
                    {
                        short? quarterHrs;
                        {
                            switch (quarter)
                            {
                                case 1: quarterHrs = tarHrsQ1List[caseDex]; break;
                                case 2: quarterHrs = tarHrsQ2List[caseDex]; break;
                                case 3: quarterHrs = tarHrsQ3List[caseDex]; break;
                                case 4: quarterHrs = tarHrsQ4List[caseDex]; break;
                                default: quarterHrs = null; break;
                            }
                        }

                        if (quarterHrs != null)
                        {
                            foreach (string componentId in componentIdArray)
                            {
                                short componentHrs = (componentId == tarCompIdList[caseDex]) ? quarterHrs.Value : otherHrs;

                                EmParameters.ComponentOperatingSuppDataRecordsForMpAndYear.Add(new ComponentOpSuppData(opSuppDataTypeCd: "OP", hours: componentHrs, days: otherHrs, componentId: componentId, quarter: quarter, calendarYear: 2020));
                                EmParameters.ComponentOperatingSuppDataRecordsForMpAndYear.Add(new ComponentOpSuppData(opSuppDataTypeCd: "QA", hours: otherHrs, days: otherHrs, componentId: componentId, quarter: quarter, calendarYear: 2020));
                                EmParameters.ComponentOperatingSuppDataRecordsForMpAndYear.Add(new ComponentOpSuppData(opSuppDataTypeCd: "MA", hours: otherHrs, days: otherHrs, componentId: componentId, quarter: quarter, calendarYear: 2020));

                                if (quarter == 2)
                                {
                                    EmParameters.ComponentOperatingSuppDataRecordsForMpAndYear.Add(new ComponentOpSuppData(opSuppDataTypeCd: "OPMJ", hours: otherHrs, days: otherHrs, componentId: componentId, quarter: quarter, calendarYear: 2020));
                                    EmParameters.ComponentOperatingSuppDataRecordsForMpAndYear.Add(new ComponentOpSuppData(opSuppDataTypeCd: "QAMJ", hours: otherHrs, days: otherHrs, componentId: componentId, quarter: quarter, calendarYear: 2020));
                                    EmParameters.ComponentOperatingSuppDataRecordsForMpAndYear.Add(new ComponentOpSuppData(opSuppDataTypeCd: "MAMJ", hours: otherHrs, days: otherHrs, componentId: componentId, quarter: quarter, calendarYear: 2020));
                                }
                            }
                        }
                    }
                }
                EmParameters.ComponentRecordForAudit = new VwMpComponentRow(componentId: compIdList[caseDex], componentIdentifier: compIdentList[caseDex], monLocId: locIdList[caseDex]);
                EmParameters.CurrentReportingPeriodObject = new ECMPS.Checks.TypeUtilities.cReportingPeriod(2020, quarterList[caseDex]);
                EmParameters.LocationPositionLookup = locationPositionLookup;

                /*  Initialize Output Parameters */
                EmParameters.LikeKindHours = int.MinValue;


                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Initialize variables needed to run the check. */
                bool log = false;
                string actual;

                /* Run Check */
                actual = EmissionAuditChecks.EMAUDIT1(category, ref log);

                /* Check results */
                Assert.AreEqual(string.Empty, actual, string.Format("actual {0}", caseDex));
                Assert.AreEqual(false, log, string.Format("log {0}", caseDex));
                Assert.AreEqual(expectedResultList[caseDex], category.CheckCatalogResult, string.Format("Result {0}", caseDex));

                Assert.AreEqual(expectedHrsList[caseDex], EmParameters.LikeKindHours, string.Format("LikeKindHours {0}", caseDex));
            }
        }

    }

}
