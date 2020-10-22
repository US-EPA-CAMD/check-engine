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
  public class cFlowRATARun : cQaTestReportCategory
  {

    #region Private Fields

    private string mMonitorLocationID;
    private cQAMain mQA;
    private string mFlowRATARunID;
    //private cRATARun mRATARun;
    private string mSystemID;

    #endregion


    #region Constructors

    public cFlowRATARun(cCheckEngine CheckEngine, cQAMain QA, string MonitorLocationID, string FlowRATARunID, cRATARun RATARun)
      : base(QA, RATARun, "FLOWRUN")
    {
      InitializeCurrent(MonitorLocationID, CheckEngine.TestSumId);

      mMonitorLocationID = MonitorLocationID;
      mQA = QA;
      mFlowRATARunID = FlowRATARunID;
      //mRATARun = RATARun;

      TableName = "FLOW_RATA_RUN";
      CurrentRowId = mFlowRATARunID;

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
      SetCheckParameter("Current_Flow_RATA_Run", new DataView(mQA.SourceData.Tables["QAFlowRATARun"],
        "flow_rata_run_id = '" + mFlowRATARunID + "'", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      mSystemID = cDBConvert.ToString(((DataRowView)GetCheckParameter("Current_Flow_RATA_Run").ParameterValue)["mon_sys_id"]);

      //SetCheckParameter("Test_Tolerances_Cross_Check_Table", new DataView(mQA.SourceData.Tables["CrossCheck_TestTolerances"],
      //  "TestTypeCode = 'RATA'", "", DataViewRowState.CurrentRows), ParameterTypes.DATAVW);

    }


    protected override void SetRecordIdentifier()
    {

      RecordIdentifier = "Operating Level " +
        cDBConvert.ToString(((DataRowView)GetCheckParameter("Current_Flow_RATA_Run").ParameterValue)["op_level_cd"]) +
        " Run Number " +
        cDBConvert.ToInteger(((DataRowView)GetCheckParameter("Current_Flow_RATA_Run").ParameterValue)["run_num"]);
    }

    #endregion

  }
}