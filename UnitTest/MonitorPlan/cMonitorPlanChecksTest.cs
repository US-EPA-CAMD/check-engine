using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

using ECMPS.Checks;
using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Data.Ecmps.CheckMp.Function;
using ECMPS.Checks.Data.Ecmps.Dbo.View;
using ECMPS.Checks.Data.Ecmps.Lookup.Table;
using ECMPS.Checks.MonitorPlanChecks;
using ECMPS.Checks.Mp.Parameters;
using ECMPS.Definitions.Extensions;

using UnitTest.UtilityClasses;


namespace UnitTest.MonitorPlan
{
  [TestClass]
  public class cMonitorPlanChecksTest
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

    #region MONPLAN11

    /// <summary>
    ///A test for MONPLAN11
    ///</summary>()
    [TestMethod()]
    public void MONPLAN11()
    {
      //static check setup
      cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

      MpParameters.Init(category.Process);
      MpParameters.Category = category;

      // MP Method Parameter Code Test
      {
        // Variables
        bool log = false;
        string actual;

        string[] testParameterList = { "PARAMBAD", "HGRE", "HGRH", "HCLRE", "HCLRH", "HFRE", "HFRH", "SO2RE", "SO2RH" };
        DateTime evaluationBeginDate = DateTime.Now.AddMonths(-6);
        DateTime[] beginDateList = { evaluationBeginDate.AddDays(-1), evaluationBeginDate, evaluationBeginDate.AddDays(1) };

        // Init Input
        foreach (string testParameterCode in testParameterList)
          foreach (DateTime testBeginDate in beginDateList)
          {
            MpParameters.MpMethodRecords = new CheckDataView<VwMpMonitorMethodRow>(new VwMpMonitorMethodRow(parameterCd: testParameterCode, beginDate: testBeginDate, beginHour: 0));
            MpParameters.MatsMpSupplementalComplianceMethodRecords = new CheckDataView<ECMPS.Checks.Data.Ecmps.CheckMp.Function.MATSMethodDataParameter>();
            MpParameters.EvaluationBeginDate = evaluationBeginDate;

            // Init Output
            category.CheckCatalogResult = null;
            MpParameters.MatsEvaluationBeginDate = null;

            // Run Checks
            actual = cMonitorPlanChecks.MONPLAN11(category, ref log);

            // Check Results
            Assert.AreEqual(string.Empty, actual);
            Assert.AreEqual(false, log);
            Assert.AreEqual(null, category.CheckCatalogResult, "Result");

            if (testParameterCode.InList("HGRE,HGRH,HCLRE,HCLRH,HFRE,HFRH,SO2RE,SO2RH"))
            {
              if (testBeginDate > evaluationBeginDate)
                Assert.AreEqual(testBeginDate.ToString(), MpParameters.MatsEvaluationBeginDate.ToString(), "MatsEvaluationBeginDate");
              else
                Assert.AreEqual(evaluationBeginDate.ToString(), MpParameters.MatsEvaluationBeginDate.ToString(), "MatsEvaluationBeginDate");
            }
            else
            {
              Assert.AreEqual(null, MpParameters.MatsEvaluationBeginDate, "MatsEvaluationBeginDate");
            }
          }
      }

      // MATS MP Supplemental Method Test; MPMethod was not found
      {
        // Variables
        bool log = false;
        string actual;

        DateTime evaluationBeginDate = DateTime.Now.AddMonths(-6);
        DateTime[] beginDateList = { evaluationBeginDate.AddDays(-1), evaluationBeginDate, evaluationBeginDate.AddDays(1) };

        // Init Input
        foreach (DateTime testBeginDate in beginDateList)
        {
          MpParameters.MpMethodRecords = new CheckDataView<VwMpMonitorMethodRow>(new VwMpMonitorMethodRow(parameterCd: "PARAMBAD"));
          MpParameters.MatsMpSupplementalComplianceMethodRecords = new CheckDataView<ECMPS.Checks.Data.Ecmps.CheckMp.Function.MATSMethodDataParameter>
          (new ECMPS.Checks.Data.Ecmps.CheckMp.Function.MATSMethodDataParameter(beginDate: testBeginDate, beginHour: 0));
          MpParameters.EvaluationBeginDate = evaluationBeginDate;
          MpParameters.MatsEvaluationBeginDate = null;

          // Init Output
          category.CheckCatalogResult = null;

          // Run Checks
          actual = cMonitorPlanChecks.MONPLAN11(category, ref log);

          // Check Results
          Assert.AreEqual(string.Empty, actual);
          Assert.AreEqual(false, log);
          Assert.AreEqual(null, category.CheckCatalogResult, "Result");

          if (testBeginDate > evaluationBeginDate)
          {
            Assert.AreEqual(testBeginDate.ToString(), MpParameters.MatsEvaluationBeginDate.ToString(), "MatsEvaluationBeginDate");
          }
          else
          {
            Assert.AreEqual(evaluationBeginDate.ToString(), MpParameters.MatsEvaluationBeginDate.ToString(), "MatsEvaluationBeginDate");
          }
        }
      }

      // MATS MP Supplemental Method Test; MPMethod was found
      {
        // Variables
        bool log = false;
        string actual;

        DateTime evaluationBeginDate = DateTime.Today.AddMonths(-6);
        DateTime monitorMethodBeginDate = evaluationBeginDate.AddDays(2);
        DateTime[] beginDateList = { evaluationBeginDate.AddDays(-1), evaluationBeginDate, evaluationBeginDate.AddDays(1), evaluationBeginDate.AddDays(2) };

        // Init Input
        foreach (DateTime supplementalMethodBeginDate in beginDateList)
        {
          MpParameters.MpMethodRecords = new CheckDataView<VwMpMonitorMethodRow>(new VwMpMonitorMethodRow(parameterCd: "HGRE", beginDate: monitorMethodBeginDate, beginHour: 0));
          MpParameters.MatsMpSupplementalComplianceMethodRecords = new CheckDataView<ECMPS.Checks.Data.Ecmps.CheckMp.Function.MATSMethodDataParameter>
          (new ECMPS.Checks.Data.Ecmps.CheckMp.Function.MATSMethodDataParameter(beginDate: supplementalMethodBeginDate, beginHour: 0));
          MpParameters.EvaluationBeginDate = evaluationBeginDate;

          // Init Output
          category.CheckCatalogResult = null;
          MpParameters.MatsEvaluationBeginDate = null;

          // Run Checks
          actual = cMonitorPlanChecks.MONPLAN11(category, ref log);

          // Check Results
          Assert.AreEqual(string.Empty, actual);
          Assert.AreEqual(false, log);
          Assert.AreEqual(null, category.CheckCatalogResult, "Result");

          if ((evaluationBeginDate >= monitorMethodBeginDate) || (evaluationBeginDate >= supplementalMethodBeginDate))
          {
            Assert.AreEqual(evaluationBeginDate.ToString(), MpParameters.MatsEvaluationBeginDate.ToString(), "MatsEvaluationBeginDate");
          }
          else if (monitorMethodBeginDate <= supplementalMethodBeginDate)
          {
            Assert.AreEqual(monitorMethodBeginDate.ToString(), MpParameters.MatsEvaluationBeginDate.ToString(), "MatsEvaluationBeginDate");
          }
          else
          {
            Assert.AreEqual(supplementalMethodBeginDate.ToString(), MpParameters.MatsEvaluationBeginDate.ToString(), "MatsEvaluationBeginDate");
          }
        }
      }

      // Date and Parameter Combinations
      {
        DateTime[] dateList = new DateTime[7];
        {
          dateList[0] = DateTime.Now.Date;
          dateList[1] = dateList[0].AddDays(1);
          dateList[2] = dateList[1].AddDays(1);
          dateList[3] = dateList[2].AddDays(1);
          dateList[4] = dateList[3].AddDays(1);
          dateList[5] = dateList[4].AddDays(1);
          dateList[6] = dateList[5].AddDays(1);
        }

        string[] parameterList = { "BAD", "HGRE", "HGRH", "HCLRE", "HCLRH", "HFRE", "HFRH", "SO2RE", "SO2RH" };
        int?[] posList = { null, 0, 1, 2 };
        int?[] gapList = { null, 0, 1, 2, 3 };

        for (int evaluationPos = 0; evaluationPos <= 2; evaluationPos++)
          foreach (int? monitorMethodPos in posList)
            foreach (int? supplementalMethodPos in posList)
              if ((!monitorMethodPos.HasValue || (evaluationPos != monitorMethodPos)) &&
                  (!supplementalMethodPos.HasValue || (evaluationPos != supplementalMethodPos)) &&
                  (!monitorMethodPos.HasValue || !supplementalMethodPos.HasValue || (monitorMethodPos != supplementalMethodPos)))
                foreach (int? complianceGap in gapList)
                  foreach (string parameterCd in parameterList)
                  {
                    DateTime evaluationBeginDate = dateList[(2 * evaluationPos) + 1];
                    DateTime? monitorMethodBeginDate = (monitorMethodPos.HasValue ? dateList[(2 * monitorMethodPos.Value) + 1] : (DateTime?)null);
                    DateTime? supplementalMethodBeginDate = (supplementalMethodPos.HasValue ? dateList[(2 * supplementalMethodPos.Value) + 1] : (DateTime?)null);
                    DateTime? matsRuleComplianceDate = (complianceGap.HasValue ? dateList[2 * complianceGap.Value] : (DateTime?)null);
                    string matsRuleComplianceDateString = (matsRuleComplianceDate.HasValue ? matsRuleComplianceDate.Value.ToShortDateString() : null);

                    /* Initialize Input Parameters */
                    MpParameters.EvaluationBeginDate = evaluationBeginDate;
                    MpParameters.MatsMpSupplementalComplianceMethodRecords = new CheckDataView<MATSMethodDataParameter>(new MATSMethodDataParameter(beginDate: supplementalMethodBeginDate));
                    MpParameters.MpMethodRecords = new CheckDataView<VwMpMonitorMethodRow>(new VwMpMonitorMethodRow(parameterCd: parameterCd, beginDate: monitorMethodBeginDate));
                    MpParameters.SystemParameterLookupTable = new CheckDataView<VwSystemParameterRow>(new VwSystemParameterRow(sysParamName: "MATS_RULE", paramValue1: matsRuleComplianceDateString));

                    /* Initialize Output Parameters */
                    MpParameters.MatsEvaluationBeginDate = null;

                    DateTime? matsEvaluationBeginDate = null;
                    {
                      if ((supplementalMethodBeginDate != null) && ((monitorMethodBeginDate != null) && (parameterCd != "BAD")))
                        matsEvaluationBeginDate = (monitorMethodBeginDate <= supplementalMethodBeginDate) ? monitorMethodBeginDate : supplementalMethodBeginDate;
                      else if (supplementalMethodPos != null)
                        matsEvaluationBeginDate = supplementalMethodBeginDate;
                      else if ((monitorMethodBeginDate != null) && (parameterCd != "BAD"))
                        matsEvaluationBeginDate = monitorMethodBeginDate;

                      if ((matsRuleComplianceDate.HasValue) && ((matsEvaluationBeginDate == null) || (matsEvaluationBeginDate > matsRuleComplianceDate)))
                        matsEvaluationBeginDate = matsRuleComplianceDate;

                      if ((matsEvaluationBeginDate != null) && (matsEvaluationBeginDate < evaluationBeginDate))
                        matsEvaluationBeginDate = evaluationBeginDate;
                    }

                    /* Check Result Label */
                    string resultPrefix = string.Format("[eval: {0}, mon: {1}, supp: {2}, comp: {3}, param: {4}]",
                                                        evaluationBeginDate.ToShortDateString(),
                                                        monitorMethodBeginDate.HasValue ? monitorMethodBeginDate.Value.ToShortDateString() : null,
                                                        supplementalMethodBeginDate.HasValue ? supplementalMethodBeginDate.Value.ToShortDateString() : null,
                                                        matsRuleComplianceDateString,
                                                        parameterCd);

                    /* Init Cateogry Result */
                    category.CheckCatalogResult = null;

                    /* Declare Check Call Variables */
                    bool log = false;
                    string actual;

                    /* Run Check */
                    actual = cMonitorPlanChecks.MONPLAN11(category, ref log);

                    /* Check Results */
                    Assert.AreEqual(string.Empty, actual, resultPrefix + ".Actual");
                    Assert.AreEqual(false, log, resultPrefix + ".Log");

                    Assert.AreEqual(null, category.CheckCatalogResult, resultPrefix + ".Result");
                    Assert.AreEqual(matsEvaluationBeginDate, MpParameters.MatsEvaluationBeginDate, resultPrefix + ".MatsEvaluationBeginDate");
                  }
      }
    }

        #endregion

        /// <summary>
        /// MonPlan-13
        /// 
        /// </summary>
        [TestMethod]
        public void MonPlan13()
        {
            /* Initialize objects generally needed for testing checks. */
            cCategory category = new UnitTest.UtilityClasses.UnitTestCategory();

            MpParameters.Init(category.Process);
            MpParameters.Category = category;


            /* Initialize Required Parameters */
            MpParameters.ProgramCodeTable = new CheckDataView<ProgramCodeRow>
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
                    new ProgramCodeRow(prgCd: "SIPNOX", prgName: "SIP NOx Program", osInd: 0, rueInd: 0, so2CertInd: 0, noxCertInd: 1, noxcCertInd: 1, notes: " SIPNOX is treated as a OS program in ECMPS")
                );

            /* Initialize Output Parameters */
            MpParameters.ProgramIsOzoneSeasonList = "Bad List";


            /* Init Cateogry Result */
            category.CheckCatalogResult = null;

            /* Initialize variables needed to run the check. */
            bool log = false;
            string actual;

            /* Run Check */
            actual = cMonitorPlanChecks.MONPLAN13(category, ref log);

            /* Check Results */
            Assert.AreEqual(string.Empty, actual, string.Format("actual"));
            Assert.AreEqual(false, log, string.Format("log"));
            Assert.AreEqual(null, category.CheckCatalogResult, string.Format("category.CheckCatalogResult"));

            Assert.AreEqual("CAIROS,CSNOXOS,CSOSG1,CSOSG2,NBP,NHNOX", MpParameters.ProgramIsOzoneSeasonList, string.Format("ProgramIsOzoneSeasonList"));
        }

    }
}
