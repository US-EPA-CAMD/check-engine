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
  public class cAppendixESummary : cQaTestReportCategory
  {
    #region Private Fields

    private string mMonitorLocationID;
    private cQAMain mQA;
    private string mAPPESummaryId;

    #endregion

    #region Constructors

    public cAppendixESummary(cCheckEngine CheckEngine, cQAMain QA, string MonitorLocationID, string APPESummaryId, cAppendixE AppendixE)
      : base(QA, AppendixE, "APPESUM")
    {
      InitializeCurrent(MonitorLocationID, AppendixE.TestSumID);

      mMonitorLocationID = MonitorLocationID;
      mQA = QA;
      mAPPESummaryId = APPESummaryId;

      TableName = "AE_CORRELATION_TEST_SUM";
      CurrentRowId = mAPPESummaryId;

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
      SetCheckParameter("Current_Appendix_E_Summary", new DataView(mQA.SourceData.Tables["QAAppendixESummary"],
          "AE_CORR_TEST_SUM_ID = '" + mAPPESummaryId + "'", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      SetCheckParameter("Test_Tolerances_Cross_Check_Table", new DataView(mQA.SourceData.Tables["CrossCheck_TestTolerances"],
          "TestTypeCode = 'APPE'", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);
    }


    protected override void SetRecordIdentifier()
    {
      string OpLevelNum = cDBConvert.ToString(((DataRowView)GetCheckParameter("Current_Appendix_E_Summary").ParameterValue)["OP_LEVEL_NUM"]);
      RecordIdentifier = "Operating Level " + OpLevelNum;
    }

    #endregion

  }
}
