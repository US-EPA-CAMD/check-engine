using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Data.Ecmps.Dbo.View;
using ECMPS.Checks.Qa.Parameters;
using ECMPS.Checks.UnitDefaultChecks;

using UnitTest.UtilityClasses;


namespace UnitTest.QA
{
    [TestClass]
    public class cUnitDefaultTestChecksTest
    {

        /// <summary>
        /// UnitDef-3
        /// 
        /// Runs cases where the OperatingConditionCode in  CurrentUnitDefaultTest is equalt to A, B, C, E, M, N, P, U, W, X, Y and Z.
        /// 
        /// The case should return a result of 'A' when the code is not A, B, E or P.
        /// </summary>
        [TestMethod]
        public void UnitDef3()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            QaParameters.Init(category.Process);
            QaParameters.Category = category;


            /* Test Values */
            List<string> allowedOperatingConditionsCodes = new List<string>  { "A", "B", "P" };


            foreach (string operatingConditionCd in UnitTestStandardLists.OperatingConditionCodeList)
            {
                /*  Initialize Input Parameters*/
                QaParameters.CurrentUnitDefaultTest = new VwQaTestSummaryUnitdefRow(operatingConditionCd: operatingConditionCd);


                /* Init Cateogry Result */
                category.CheckCatalogResult = null;

                /* Initialize variables needed to run the check. */
                bool log = false;
                string actual = null;

                /* Run Check */
                actual = cUnitDefaultChecks.UNITDEF3(category, ref log);


                /* Check results */
                Assert.AreEqual(string.Empty, actual, string.Format("Return [OperatingConditionCd]: {0}", operatingConditionCd));
                Assert.AreEqual(false, log, string.Format("Log  [OperatingConditionCd]: {0}", operatingConditionCd));
                Assert.AreEqual(allowedOperatingConditionsCodes.Contains(operatingConditionCd) ? null : "A", category.CheckCatalogResult, string.Format("Result  [OperatingConditionCd]: {0}", operatingConditionCd));
            }

        }

    }
}
