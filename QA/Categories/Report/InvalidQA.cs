using System;
using System.Data;
using System.Collections;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;
using ECMPS.ErrorSuppression;


namespace ECMPS.Checks.QAEvaluation
{
  public class cInvalidQA : cQaTestReportCategory
  {

    #region Private Fields

    private string mMonitorLocationID;
    private cQAMain mQA;
    private string mTestSumID;

    #endregion


    #region Constructors

    public cInvalidQA(cCheckEngine CheckEngine, cQAMain QA, string MonitorLocationID, string TestSumID)
      : base(QA, "CRITLV2")
    {
      InitializeCurrent(MonitorLocationID, TestSumID);

      mMonitorLocationID = MonitorLocationID;
      mQA = QA;
      mTestSumID = TestSumID;

      TableName = "TEST_SUMMARY";
      CurrentRowId = mTestSumID;

      FilterData();

      SetRecordIdentifier();
    }


    #endregion


    #region Public Methods

    public new bool ProcessChecks()
    {
      return base.ProcessChecks();
    }

    #endregion


    #region Base Class Overrides

    protected override void FilterData()
    {

      SetCheckParameter("Current_Test", new DataView(mQA.SourceData.Tables["QATestSummary"],
          "", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

    }


    protected override void SetRecordIdentifier()
    {
      RecordIdentifier = "this test";
    }

    #endregion

  }
}