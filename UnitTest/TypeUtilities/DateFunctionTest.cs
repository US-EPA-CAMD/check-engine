using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ECMPS.Checks.TypeUtilities;

namespace UnitTest.DateFunctions
{
    [TestClass]
    public class DateFunctionTest
    {
        [TestMethod]
        public void Earliest()
        {
            DateTime?[] list = new DateTime?[] { new DateTime(2016, 6, 18), null, new DateTime(2017, 7, 31), new DateTime(2016, 6, 17) };
            DateTime? result;

            /* Test for earliest where null is not deemed the earliest and null exists. */
            list = new DateTime?[] { new DateTime(2016, 6, 18), null, new DateTime(2017, 7, 31), new DateTime(2016, 6, 17) };
            result = cDateFunctions.Earliest(false, list);
            Assert.AreEqual(new DateTime(2016, 6, 17), result, "Test for earliest where null is not deemed the earliest and null exists.");

            /* Test for earliest where null is deemed the earliest and null exists. */
            list = new DateTime?[] { new DateTime(2016, 6, 18), null, new DateTime(2017, 7, 31), new DateTime(2016, 6, 17) };
            result = cDateFunctions.Earliest(true, list);
            Assert.AreEqual(null, result, "Test for earliest where null is deemed the earliest and null exists.");

            /* Test for earliest where null is not deemed the earliest and does not null exists. */
            list = new DateTime?[] { new DateTime(2016, 6, 18), new DateTime(2017, 7, 31), new DateTime(2016, 6, 17) };
            result = cDateFunctions.Earliest(false, list);
            Assert.AreEqual(new DateTime(2016, 6, 17), result, "Test for earliest where null is not deemed the earliest and does not null exists.");

            /* Test for earliest where null is deemed the earliest and nulld oes not exists. */
            list = new DateTime?[] { new DateTime(2016, 6, 18), new DateTime(2017, 7, 31), new DateTime(2016, 6, 17) };
            result = cDateFunctions.Earliest(true, list);
            Assert.AreEqual(new DateTime(2016, 6, 17), result, "Test for earliest where null is deemed the earliest and nulld oes not exists.");

            /* Test for earliest where nulls are ignored and a null exists. */
            list = new DateTime?[] { new DateTime(2016, 6, 18), null, new DateTime(2017, 7, 31), new DateTime(2016, 6, 17) };
            result = cDateFunctions.Earliest(list);
            Assert.AreEqual(new DateTime(2016, 6, 17), result, "Test for earliest where null is not deemed the earliest and null exists.");

            /* Test for earliest where nulls are ignored and a null does not exists. */
            list = new DateTime?[] { new DateTime(2016, 6, 18), new DateTime(2017, 7, 31), new DateTime(2016, 6, 17) };
            result = cDateFunctions.Earliest(list);
            Assert.AreEqual(new DateTime(2016, 6, 17), result, "Test for earliest where null is not deemed the earliest and does not null exists.");
        }

        [TestMethod]
        public void HourDifference()
        {
            DateTime referenceDate = new DateTime(2016, 6, 17, 12, 0, 0);
            int offset;

            offset = -365; Assert.AreEqual(offset, cDateFunctions.HourDifference(referenceDate, referenceDate.AddHours(offset)), string.Format("DateDifference test for reference {0} and test {1}", referenceDate, referenceDate.AddHours(offset)));
            offset = -37; Assert.AreEqual(offset, cDateFunctions.HourDifference(referenceDate, referenceDate.AddHours(offset)), string.Format("DateDifference test for reference {0} and test {1}", referenceDate, referenceDate.AddHours(offset)));
            offset = -13; Assert.AreEqual(offset, cDateFunctions.HourDifference(referenceDate, referenceDate.AddHours(offset)), string.Format("DateDifference test for reference {0} and test {1}", referenceDate, referenceDate.AddHours(offset)));
            offset = -12; Assert.AreEqual(offset, cDateFunctions.HourDifference(referenceDate, referenceDate.AddHours(offset)), string.Format("DateDifference test for reference {0} and test {1}", referenceDate, referenceDate.AddHours(offset)));
            offset = -1; Assert.AreEqual(offset, cDateFunctions.HourDifference(referenceDate, referenceDate.AddHours(offset)), string.Format("DateDifference test for reference {0} and test {1}", referenceDate, referenceDate.AddHours(offset)));
            offset = 0; Assert.AreEqual(offset, cDateFunctions.HourDifference(referenceDate, referenceDate.AddHours(offset)), string.Format("DateDifference test for reference {0} and test {1}", referenceDate, referenceDate.AddHours(offset)));
            offset = 1; Assert.AreEqual(offset, cDateFunctions.HourDifference(referenceDate, referenceDate.AddHours(offset)), string.Format("DateDifference test for reference {0} and test {1}", referenceDate, referenceDate.AddHours(offset)));
            offset = 11; Assert.AreEqual(offset, cDateFunctions.HourDifference(referenceDate, referenceDate.AddHours(offset)), string.Format("DateDifference test for reference {0} and test {1}", referenceDate, referenceDate.AddHours(offset)));
            offset = 12; Assert.AreEqual(offset, cDateFunctions.HourDifference(referenceDate, referenceDate.AddHours(offset)), string.Format("DateDifference test for reference {0} and test {1}", referenceDate, referenceDate.AddHours(offset)));
            offset = 36; Assert.AreEqual(offset, cDateFunctions.HourDifference(referenceDate, referenceDate.AddHours(offset)), string.Format("DateDifference test for reference {0} and test {1}", referenceDate, referenceDate.AddHours(offset)));
            offset = 365; Assert.AreEqual(offset, cDateFunctions.HourDifference(referenceDate, referenceDate.AddHours(offset)), string.Format("DateDifference test for reference {0} and test {1}", referenceDate, referenceDate.AddHours(offset)));
        }
    }
}
