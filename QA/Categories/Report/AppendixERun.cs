using System;
using System.Data;
using System.Collections;
using System.Text.RegularExpressions;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;
using ECMPS.ErrorSuppression;

namespace ECMPS.Checks.QAEvaluation
{
  public class cAppendixERun : cQaTestReportCategory
  {
    #region Private Fields

    private string mMonitorLocationID;
    private cQAMain mQA;
    private string mAPPERunId;
    private string mSystemID;

    #endregion


    #region Constructors

    public cAppendixERun(cCheckEngine CheckEngine, cQAMain QA, string MonitorLocationID, string APPERunId, cAppendixESummary AppendixESummary)
      : base(QA, AppendixESummary, "APPERUN")
    {
      InitializeCurrent(MonitorLocationID, CheckEngine.TestSumId);

      mMonitorLocationID = MonitorLocationID;
      mQA = QA;
      mAPPERunId = APPERunId;
      TableName = "AE_CORRELATION_TEST_RUN";
      CurrentRowId = mAPPERunId;

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
      SetCheckParameter("Current_Appendix_E_Run", new DataView(mQA.SourceData.Tables["QAAppendixERun"],
          "AE_CORR_TEST_RUN_ID = '" + mAPPERunId + "'", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      mSystemID = cDBConvert.ToString(((DataRowView)GetCheckParameter("Current_Appendix_E_Run").ParameterValue)["mon_sys_id"]);

      SetCheckParameter("Test_Tolerances_Cross_Check_Table", new DataView(mQA.SourceData.Tables["CrossCheck_TestTolerances"],
          "TestTypeCode = 'APPE'", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);
    }

    protected override void SetRecordIdentifier()
    {
      RecordIdentifier = "Operating Level " + cDBConvert.ToString(((DataRowView)GetCheckParameter("Current_Appendix_E_Run").ParameterValue)["OP_LEVEL_NUM"]) +
          " Run Number " + cDBConvert.ToInteger(((DataRowView)GetCheckParameter("Current_Appendix_E_Run").ParameterValue)["run_num"]);
    }

    #endregion
  }
}
