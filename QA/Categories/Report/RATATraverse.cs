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
  public class cRATATraverse : cQaTestReportCategory
  {

    #region Private Fields

    private string mMonitorLocationID;
    private cQAMain mQA;
    private string mRATATraverseID;
    //private cFlowRATARun mFlowRATARun;
    private string mSystemID;

    #endregion


    #region Constructors

    public cRATATraverse(cCheckEngine CheckEngine, cQAMain QA, string MonitorLocationID, string RATATraverseID, cFlowRATARun FlowRATARun)
      : base(QA, FlowRATARun, "TRAVPT")
    {
      InitializeCurrent(MonitorLocationID, CheckEngine.TestSumId);

      mMonitorLocationID = MonitorLocationID;
      mQA = QA;
      mRATATraverseID = RATATraverseID;
      //mFlowRATARun = FlowRATARun;

      TableName = "RATA_TRAVERSE";
      CurrentRowId = mRATATraverseID;

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
      SetCheckParameter("Current_RATA_Traverse", new DataView(mQA.SourceData.Tables["QARATATraverse"],
        "rata_Traverse_id = '" + mRATATraverseID + "'", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      mSystemID = cDBConvert.ToString(((DataRowView)GetCheckParameter("Current_RATA_Traverse").ParameterValue)["mon_sys_id"]);

      SetCheckParameter("Pressure_Measure_Code_Lookup_Table", new DataView(mQA.SourceData.Tables["PressureMeasureCode"],
        "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      //SetCheckParameter("Test_Tolerances_Cross_Check_Table", new DataView(mQA.SourceData.Tables["CrossCheck_TestTolerances"],
      //        "TestTypeCode = 'RATA'", "", DataViewRowState.CurrentRows), ParameterTypes.DATAVW);

    }


    protected override void SetRecordIdentifier()
    {

      RecordIdentifier = "Operating Level " +
        cDBConvert.ToString(((DataRowView)GetCheckParameter("Current_RATA_Traverse").ParameterValue)["op_level_cd"]) +
        " Run Number " +
        cDBConvert.ToInteger(((DataRowView)GetCheckParameter("Current_RATA_Traverse").ParameterValue)["run_num"]) +
        " Traverse Point ID " +
        cDBConvert.ToString(((DataRowView)GetCheckParameter("Current_RATA_Traverse").ParameterValue)["method_traverse_point_id"]);
    }

    #endregion

  }
}