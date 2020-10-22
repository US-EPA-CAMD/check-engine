using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Data.Ecmps.Dbo.View;

namespace UnitTest.Miscellaneous
{
    [TestClass]
    public class CheckEngineTests
    {
        [TestMethod]
        public void CheckDataEnumerator()
        {
            CheckDataView<VwStackPipeRow> checkDataView;
            int testCount;

            {   // Check that enumerator works with no rows
                checkDataView = new CheckDataView<VwStackPipeRow>();
                testCount = 0;

                foreach (VwStackPipeRow row in checkDataView) testCount++;

                Assert.AreEqual(0, testCount, "No rows");
            }

            {   // Check that enumerator works with rows
                checkDataView = new CheckDataView<VwStackPipeRow>
                    (
                        new VwStackPipeRow(monLocId: "ONE", stackName: "MS4", activeDate: new DateTime(2015, 6, 17)),
                        new VwStackPipeRow(monLocId: "TWO", stackName: "CP3", activeDate: new DateTime(2013, 6, 17)),
                        new VwStackPipeRow(monLocId: "THREE", stackName: "CS2", activeDate: new DateTime(2014, 6, 17)),
                        new VwStackPipeRow(monLocId: "FOUR", stackName: "MP1", activeDate: new DateTime(2015, 6, 17))
                    );

                string[] expectedList = { "ONE", "TWO", "THREE", "FOUR" };

                testCount = 0;

                foreach (VwStackPipeRow row in checkDataView)
                {
                    Assert.AreEqual(expectedList[testCount], row.MonLocId, "Four rows");
                    testCount++;
                }

                Assert.AreEqual(4, testCount, "No rows");
            }
        }


        [TestMethod]
        public void cHourRangeCollection_UnionAndSort()
        {
            cHourRangeCollection hourRangeCollection;
            int caseDex = -1;
            bool continueCases = true;
            int count, pos;


            while (continueCases)
            {
                continueCases = false;
                caseDex++;
                
                hourRangeCollection = new cHourRangeCollection();
                count = 0;

                Assert.AreEqual(0, hourRangeCollection.Count);


                /* add initial range */
                hourRangeCollection.Union(new DateTime(2017, 6, 1), 0, new DateTime(2017, 6, 30), 23);

                if (count == caseDex)
                {
                    hourRangeCollection.Sort();
                    pos = 0;
                    Assert.AreEqual(1, hourRangeCollection.Count, String.Format("Union{0} Count", caseDex));
                    Assert.AreEqual(new DateTime(2017, 6, 1), hourRangeCollection.Item(pos).BeganDate, String.Format("Union{0}[{1}] BeginDate", caseDex, pos));
                    Assert.AreEqual(0, hourRangeCollection.Item(pos).BeganHour, String.Format("Union{0}[{1}] BeginHour", caseDex, pos));
                    Assert.AreEqual(new DateTime(2017, 6, 30), hourRangeCollection.Item(pos).EndedDate, String.Format("Union{0}[{1}] EndedDate", caseDex, pos));
                    Assert.AreEqual(23, hourRangeCollection.Item(pos).EndedHour, String.Format("Union{0}[{1}] EndedHour", caseDex, pos));
                    continueCases = true;
                    continue;
                }
                else
                    count++;


                /* add duplicate range */
                hourRangeCollection.Union(new DateTime(2017, 6, 1), 0, new DateTime(2017, 6, 30), 23);

                if (count == caseDex)
                {
                    hourRangeCollection.Sort();
                    pos = 0;
                    Assert.AreEqual(1, hourRangeCollection.Count, String.Format("Union{0} Count", caseDex));
                    Assert.AreEqual(new DateTime(2017, 6, 1), hourRangeCollection.Item(pos).BeganDate, String.Format("Union{0}[{1}] BeginDate", caseDex, pos));
                    Assert.AreEqual(0, hourRangeCollection.Item(pos).BeganHour, String.Format("Union{0}[{1}] BeginHour", caseDex, pos));
                    Assert.AreEqual(new DateTime(2017, 6, 30), hourRangeCollection.Item(pos).EndedDate, String.Format("Union{0}[{1}] EndedDate", caseDex, pos));
                    Assert.AreEqual(23, hourRangeCollection.Item(pos).EndedHour, String.Format("Union{0}[{1}] EndedHour", caseDex, pos));
                    continueCases = true;
                    continue;
                }
                else
                    count++;


                /* add adjacent previous range */
                hourRangeCollection.Union(new DateTime(2017, 5, 1), 0, new DateTime(2017, 5, 31), 23);

                if (count == caseDex)
                {
                    hourRangeCollection.Sort();
                    pos = 0;
                    Assert.AreEqual(1, hourRangeCollection.Count, String.Format("Union{0} Count", caseDex));
                    Assert.AreEqual(new DateTime(2017, 5, 1), hourRangeCollection.Item(pos).BeganDate, String.Format("Union{0}[{1}] BeginDate", caseDex, pos));
                    Assert.AreEqual(0, hourRangeCollection.Item(pos).BeganHour, String.Format("Union{0}[{1}] BeginHour", caseDex, pos));
                    Assert.AreEqual(new DateTime(2017, 6, 30), hourRangeCollection.Item(pos).EndedDate, String.Format("Union{0}[{1}] EndedDate", caseDex, pos));
                    Assert.AreEqual(23, hourRangeCollection.Item(pos).EndedHour, String.Format("Union{0}[{1}] EndedHour", caseDex, pos));
                    continueCases = true;
                    continue;
                }
                else
                    count++;


                /* add adjacent subsequent range */
                hourRangeCollection.Union(new DateTime(2017, 7, 1), 0, new DateTime(2017, 7, 31), 23);

                if (count == caseDex)
                {
                    hourRangeCollection.Sort();
                    pos = 0;
                    Assert.AreEqual(1, hourRangeCollection.Count, String.Format("Union{0} Count", caseDex));
                    Assert.AreEqual(new DateTime(2017, 5, 1), hourRangeCollection.Item(pos).BeganDate, String.Format("Union{0}[{1}] BeginDate", caseDex, pos));
                    Assert.AreEqual(0, hourRangeCollection.Item(pos).BeganHour, String.Format("Union{0}[{1}] BeginHour", caseDex, pos));
                    Assert.AreEqual(new DateTime(2017, 7, 31), hourRangeCollection.Item(pos).EndedDate, String.Format("Union{0}[{1}] EndedDate", caseDex, pos));
                    Assert.AreEqual(23, hourRangeCollection.Item(pos).EndedHour, String.Format("Union{0}[{1}] EndedHour", caseDex, pos));
                    continueCases = true;
                    continue;
                }
                else
                    count++;


                /* add separated previous range without month or day boundary */
                hourRangeCollection.Union(new DateTime(2017, 4, 15), 12, new DateTime(2017, 4, 30), 22);

                if (count == caseDex)
                {
                    hourRangeCollection.Sort();
                    pos = 0;
                    Assert.AreEqual(2, hourRangeCollection.Count, String.Format("Union{0} Count", caseDex));
                    Assert.AreEqual(new DateTime(2017, 4, 15), hourRangeCollection.Item(pos).BeganDate, String.Format("Union{0}[{1}] BeginDate", caseDex, pos));
                    Assert.AreEqual(12, hourRangeCollection.Item(pos).BeganHour, String.Format("Union{0}[{1}] BeginHour", caseDex, pos));
                    Assert.AreEqual(new DateTime(2017, 4, 30), hourRangeCollection.Item(pos).EndedDate, String.Format("Union{0}[{1}] EndedDate", caseDex, pos));
                    Assert.AreEqual(22, hourRangeCollection.Item(pos).EndedHour, String.Format("Union{0}[{1}] EndedHour", caseDex, pos));
                    pos = 1;
                    Assert.AreEqual(new DateTime(2017, 5, 1), hourRangeCollection.Item(pos).BeganDate, String.Format("Union{0}[{1}] BeginDate", caseDex, pos));
                    Assert.AreEqual(0, hourRangeCollection.Item(pos).BeganHour, String.Format("Union{0}[{1}] BeginHour", caseDex, pos));
                    Assert.AreEqual(new DateTime(2017, 7, 31), hourRangeCollection.Item(pos).EndedDate, String.Format("Union{0}[{1}] EndedDate", caseDex, pos));
                    Assert.AreEqual(23, hourRangeCollection.Item(pos).EndedHour, String.Format("Union{0}[{1}] EndedHour", caseDex, pos));
                    continueCases = true;
                    continue;
                }
                else
                    count++;


                /* add separated subsequent range without month or day boundary */
                hourRangeCollection.Union(new DateTime(2017, 8, 1), 1, new DateTime(2017, 8, 16), 11);

                if (count == caseDex)
                {
                    hourRangeCollection.Sort();
                    Assert.AreEqual(3, hourRangeCollection.Count, String.Format("Union{0} Count", caseDex));
                    pos = 0;
                    Assert.AreEqual(new DateTime(2017, 4, 15), hourRangeCollection.Item(pos).BeganDate, String.Format("Union{0}[{1}] BeginDate", caseDex, pos));
                    Assert.AreEqual(12, hourRangeCollection.Item(pos).BeganHour, String.Format("Union{0}[{1}] BeginHour", caseDex, pos));
                    Assert.AreEqual(new DateTime(2017, 4, 30), hourRangeCollection.Item(pos).EndedDate, String.Format("Union{0}[{1}] EndedDate", caseDex, pos));
                    Assert.AreEqual(22, hourRangeCollection.Item(pos).EndedHour, String.Format("Union{0}[{1}] EndedHour", caseDex, pos));
                    pos = 1;
                    Assert.AreEqual(new DateTime(2017, 5, 1), hourRangeCollection.Item(pos).BeganDate, String.Format("Union{0}[{1}] BeginDate", caseDex, pos));
                    Assert.AreEqual(0, hourRangeCollection.Item(pos).BeganHour, String.Format("Union{0}[{1}] BeginHour", caseDex, pos));
                    Assert.AreEqual(new DateTime(2017, 7, 31), hourRangeCollection.Item(pos).EndedDate, String.Format("Union{0}[{1}] EndedDate", caseDex, pos));
                    Assert.AreEqual(23, hourRangeCollection.Item(pos).EndedHour, String.Format("Union{0}[{1}] EndedHour", caseDex, pos));
                    pos = 2;
                    Assert.AreEqual(new DateTime(2017, 8, 1), hourRangeCollection.Item(pos).BeganDate, String.Format("Union{0}[{1}] BeginDate", caseDex, pos));
                    Assert.AreEqual(1, hourRangeCollection.Item(pos).BeganHour, String.Format("Union{0}[{1}] BeginHour", caseDex, pos));
                    Assert.AreEqual(new DateTime(2017, 8, 16), hourRangeCollection.Item(pos).EndedDate, String.Format("Union{0}[{1}] EndedDate", caseDex, pos));
                    Assert.AreEqual(11, hourRangeCollection.Item(pos).EndedHour, String.Format("Union{0}[{1}] EndedHour", caseDex, pos));
                    continueCases = true;
                    continue;
                }
                else
                    count++;


                /* add separated previous range with large gap and without month or day boundary */
                hourRangeCollection.Union(new DateTime(2017, 2, 14), 12, new DateTime(2017, 3, 15), 11);

                if (count == caseDex)
                {
                    hourRangeCollection.Sort();
                    Assert.AreEqual(4, hourRangeCollection.Count, String.Format("Union{0} Count", caseDex));
                    pos = 0;
                    Assert.AreEqual(new DateTime(2017, 2, 14), hourRangeCollection.Item(pos).BeganDate, String.Format("Union{0}[{1}] BeginDate", caseDex, pos));
                    Assert.AreEqual(12, hourRangeCollection.Item(pos).BeganHour, String.Format("Union{0}[{1}] BeginHour", caseDex, pos));
                    Assert.AreEqual(new DateTime(2017, 3, 15), hourRangeCollection.Item(pos).EndedDate, String.Format("Union{0}[{1}] EndedDate", caseDex, pos));
                    Assert.AreEqual(11, hourRangeCollection.Item(pos).EndedHour, String.Format("Union{0}[{1}] EndedHour", caseDex, pos));
                    pos = 1;
                    Assert.AreEqual(new DateTime(2017, 4, 15), hourRangeCollection.Item(pos).BeganDate, String.Format("Union{0}[{1}] BeginDate", caseDex, pos));
                    Assert.AreEqual(12, hourRangeCollection.Item(pos).BeganHour, String.Format("Union{0}[{1}] BeginHour", caseDex, pos));
                    Assert.AreEqual(new DateTime(2017, 4, 30), hourRangeCollection.Item(pos).EndedDate, String.Format("Union{0}[{1}] EndedDate", caseDex, pos));
                    Assert.AreEqual(22, hourRangeCollection.Item(pos).EndedHour, String.Format("Union{0}[{1}] EndedHour", caseDex, pos));
                    pos = 2;
                    Assert.AreEqual(new DateTime(2017, 5, 1), hourRangeCollection.Item(pos).BeganDate, String.Format("Union{0}[{1}] BeginDate", caseDex, pos));
                    Assert.AreEqual(0, hourRangeCollection.Item(pos).BeganHour, String.Format("Union{0}[{1}] BeginHour", caseDex, pos));
                    Assert.AreEqual(new DateTime(2017, 7, 31), hourRangeCollection.Item(pos).EndedDate, String.Format("Union{0}[{1}] EndedDate", caseDex, pos));
                    Assert.AreEqual(23, hourRangeCollection.Item(pos).EndedHour, String.Format("Union{0}[{1}] EndedHour", caseDex, pos));
                    pos = 3;
                    Assert.AreEqual(new DateTime(2017, 8, 1), hourRangeCollection.Item(pos).BeganDate, String.Format("Union{0}[{1}] BeginDate", caseDex, pos));
                    Assert.AreEqual(1, hourRangeCollection.Item(pos).BeganHour, String.Format("Union{0}[{1}] BeginHour", caseDex, pos));
                    Assert.AreEqual(new DateTime(2017, 8, 16), hourRangeCollection.Item(pos).EndedDate, String.Format("Union{0}[{1}] EndedDate", caseDex, pos));
                    Assert.AreEqual(11, hourRangeCollection.Item(pos).EndedHour, String.Format("Union{0}[{1}] EndedHour", caseDex, pos));
                    continueCases = true;
                    continue;
                }
                else
                    count++;


                /* add range that is adjacent to two ranges */
                hourRangeCollection.Union(new DateTime(2017, 3, 15), 12, new DateTime(2017, 4, 15), 11);

                if (count == caseDex)
                {
                    hourRangeCollection.Sort();
                    Assert.AreEqual(3, hourRangeCollection.Count, String.Format("Union{0} Count", caseDex));
                    pos = 0;
                    Assert.AreEqual(new DateTime(2017, 2, 14), hourRangeCollection.Item(pos).BeganDate, String.Format("Union{0}[{1}] BeginDate", caseDex, pos));
                    Assert.AreEqual(12, hourRangeCollection.Item(pos).BeganHour, String.Format("Union{0}[{1}] BeginHour", caseDex, pos));
                    Assert.AreEqual(new DateTime(2017, 4, 30), hourRangeCollection.Item(pos).EndedDate, String.Format("Union{0}[{1}] EndedDate", caseDex, pos));
                    Assert.AreEqual(22, hourRangeCollection.Item(pos).EndedHour, String.Format("Union{0}[{1}] EndedHour", caseDex, pos));
                    pos = 1;
                    Assert.AreEqual(new DateTime(2017, 5, 1), hourRangeCollection.Item(pos).BeganDate, String.Format("Union{0}[{1}] BeginDate", caseDex, pos));
                    Assert.AreEqual(0, hourRangeCollection.Item(pos).BeganHour, String.Format("Union{0}[{1}] BeginHour", caseDex, pos));
                    Assert.AreEqual(new DateTime(2017, 7, 31), hourRangeCollection.Item(pos).EndedDate, String.Format("Union{0}[{1}] EndedDate", caseDex, pos));
                    Assert.AreEqual(23, hourRangeCollection.Item(pos).EndedHour, String.Format("Union{0}[{1}] EndedHour", caseDex, pos));
                    pos = 2;
                    Assert.AreEqual(new DateTime(2017, 8, 1), hourRangeCollection.Item(pos).BeganDate, String.Format("Union{0}[{1}] BeginDate", caseDex, pos));
                    Assert.AreEqual(1, hourRangeCollection.Item(pos).BeganHour, String.Format("Union{0}[{1}] BeginHour", caseDex, pos));
                    Assert.AreEqual(new DateTime(2017, 8, 16), hourRangeCollection.Item(pos).EndedDate, String.Format("Union{0}[{1}] EndedDate", caseDex, pos));
                    Assert.AreEqual(11, hourRangeCollection.Item(pos).EndedHour, String.Format("Union{0}[{1}] EndedHour", caseDex, pos));
                    continueCases = true;
                    continue;
                }
                else
                    count++;


                /* add separated subsequent range with large gap and without month or day boundary */
                hourRangeCollection.Union(new DateTime(2017, 9, 15), 12, new DateTime(2017, 10, 16), 11);

                if (count == caseDex)
                {
                    hourRangeCollection.Sort();
                    Assert.AreEqual(4, hourRangeCollection.Count, String.Format("Union{0} Count", caseDex));
                    pos = 0;
                    Assert.AreEqual(new DateTime(2017, 2, 14), hourRangeCollection.Item(pos).BeganDate, String.Format("Union{0}[{1}] BeginDate", caseDex, pos));
                    Assert.AreEqual(12, hourRangeCollection.Item(pos).BeganHour, String.Format("Union{0}[{1}] BeginHour", caseDex, pos));
                    Assert.AreEqual(new DateTime(2017, 4, 30), hourRangeCollection.Item(pos).EndedDate, String.Format("Union{0}[{1}] EndedDate", caseDex, pos));
                    Assert.AreEqual(22, hourRangeCollection.Item(pos).EndedHour, String.Format("Union{0}[{1}] EndedHour", caseDex, pos));
                    pos = 1;
                    Assert.AreEqual(new DateTime(2017, 5, 1), hourRangeCollection.Item(pos).BeganDate, String.Format("Union{0}[{1}] BeginDate", caseDex, pos));
                    Assert.AreEqual(0, hourRangeCollection.Item(pos).BeganHour, String.Format("Union{0}[{1}] BeginHour", caseDex, pos));
                    Assert.AreEqual(new DateTime(2017, 7, 31), hourRangeCollection.Item(pos).EndedDate, String.Format("Union{0}[{1}] EndedDate", caseDex, pos));
                    Assert.AreEqual(23, hourRangeCollection.Item(pos).EndedHour, String.Format("Union{0}[{1}] EndedHour", caseDex, pos));
                    pos = 2;
                    Assert.AreEqual(new DateTime(2017, 8, 1), hourRangeCollection.Item(pos).BeganDate, String.Format("Union{0}[{1}] BeginDate", caseDex, pos));
                    Assert.AreEqual(1, hourRangeCollection.Item(pos).BeganHour, String.Format("Union{0}[{1}] BeginHour", caseDex, pos));
                    Assert.AreEqual(new DateTime(2017, 8, 16), hourRangeCollection.Item(pos).EndedDate, String.Format("Union{0}[{1}] EndedDate", caseDex, pos));
                    Assert.AreEqual(11, hourRangeCollection.Item(pos).EndedHour, String.Format("Union{0}[{1}] EndedHour", caseDex, pos));
                    pos = 3;
                    Assert.AreEqual(new DateTime(2017, 9, 15), hourRangeCollection.Item(pos).BeganDate, String.Format("Union{0}[{1}] BeginDate", caseDex, pos));
                    Assert.AreEqual(12, hourRangeCollection.Item(pos).BeganHour, String.Format("Union{0}[{1}] BeginHour", caseDex, pos));
                    Assert.AreEqual(new DateTime(2017, 10, 16), hourRangeCollection.Item(pos).EndedDate, String.Format("Union{0}[{1}] EndedDate", caseDex, pos));
                    Assert.AreEqual(11, hourRangeCollection.Item(pos).EndedHour, String.Format("Union{0}[{1}] EndedHour", caseDex, pos));
                    continueCases = true;
                    continue;
                }
                else
                    count++;


                /* add separated range between two ranges but not adjacent to either */
                hourRangeCollection.Union(new DateTime(2017, 8, 16), 13, new DateTime(2017, 9, 15), 10);

                if (count == caseDex)
                {
                    hourRangeCollection.Sort();
                    Assert.AreEqual(5, hourRangeCollection.Count, String.Format("Union{0} Count", caseDex));
                    pos = 0;
                    Assert.AreEqual(new DateTime(2017, 2, 14), hourRangeCollection.Item(pos).BeganDate, String.Format("Union{0}[{1}] BeginDate", caseDex, pos));
                    Assert.AreEqual(12, hourRangeCollection.Item(pos).BeganHour, String.Format("Union{0}[{1}] BeginHour", caseDex, pos));
                    Assert.AreEqual(new DateTime(2017, 4, 30), hourRangeCollection.Item(pos).EndedDate, String.Format("Union{0}[{1}] EndedDate", caseDex, pos));
                    Assert.AreEqual(22, hourRangeCollection.Item(pos).EndedHour, String.Format("Union{0}[{1}] EndedHour", caseDex, pos));
                    pos = 1;
                    Assert.AreEqual(new DateTime(2017, 5, 1), hourRangeCollection.Item(pos).BeganDate, String.Format("Union{0}[{1}] BeginDate", caseDex, pos));
                    Assert.AreEqual(0, hourRangeCollection.Item(pos).BeganHour, String.Format("Union{0}[{1}] BeginHour", caseDex, pos));
                    Assert.AreEqual(new DateTime(2017, 7, 31), hourRangeCollection.Item(pos).EndedDate, String.Format("Union{0}[{1}] EndedDate", caseDex, pos));
                    Assert.AreEqual(23, hourRangeCollection.Item(pos).EndedHour, String.Format("Union{0}[{1}] EndedHour", caseDex, pos));
                    pos = 2;
                    Assert.AreEqual(new DateTime(2017, 8, 1), hourRangeCollection.Item(pos).BeganDate, String.Format("Union{0}[{1}] BeginDate", caseDex, pos));
                    Assert.AreEqual(1, hourRangeCollection.Item(pos).BeganHour, String.Format("Union{0}[{1}] BeginHour", caseDex, pos));
                    Assert.AreEqual(new DateTime(2017, 8, 16), hourRangeCollection.Item(pos).EndedDate, String.Format("Union{0}[{1}] EndedDate", caseDex, pos));
                    Assert.AreEqual(11, hourRangeCollection.Item(pos).EndedHour, String.Format("Union{0}[{1}] EndedHour", caseDex, pos));
                    pos = 3;
                    Assert.AreEqual(new DateTime(2017, 8, 16), hourRangeCollection.Item(pos).BeganDate, String.Format("Union{0}[{1}] BeginDate", caseDex, pos));
                    Assert.AreEqual(13, hourRangeCollection.Item(pos).BeganHour, String.Format("Union{0}[{1}] BeginHour", caseDex, pos));
                    Assert.AreEqual(new DateTime(2017, 9, 15), hourRangeCollection.Item(pos).EndedDate, String.Format("Union{0}[{1}] EndedDate", caseDex, pos));
                    Assert.AreEqual(10, hourRangeCollection.Item(pos).EndedHour, String.Format("Union{0}[{1}] EndedHour", caseDex, pos));
                    pos = 4;
                    Assert.AreEqual(new DateTime(2017, 9, 15), hourRangeCollection.Item(pos).BeganDate, String.Format("Union{0}[{1}] BeginDate", caseDex, pos));
                    Assert.AreEqual(12, hourRangeCollection.Item(pos).BeganHour, String.Format("Union{0}[{1}] BeginHour", caseDex, pos));
                    Assert.AreEqual(new DateTime(2017, 10, 16), hourRangeCollection.Item(pos).EndedDate, String.Format("Union{0}[{1}] EndedDate", caseDex, pos));
                    Assert.AreEqual(11, hourRangeCollection.Item(pos).EndedHour, String.Format("Union{0}[{1}] EndedHour", caseDex, pos));
                    continueCases = true;
                    continue;
                }
                else
                    count++;
            }
        }
    }
}
