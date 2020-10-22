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
  public class cCycleTimeInjection : cQaTestReportCategory
  {

    #region Private Fields

    private string mMonitorLocationID;
    private cQAMain mQA;
    private string mCycleTimeInjID;
    //private cCycleTime mCycleTime;


    #endregion


    #region Constructors

    public cCycleTimeInjection(cCheckEngine CheckEngine, cQAMain QA, string MonitorLocationID, string CycleTimeInjID, cCycleTime CycleTime)
      : base(QA, CycleTime, "CYCLINJ")
    {
      InitializeCurrent(MonitorLocationID, CycleTime.TestSumID);

      mMonitorLocationID = MonitorLocationID;
      mQA = QA;
      mCycleTimeInjID = CycleTimeInjID;
      //mCycleTime = CycleTime;

      TableName = "CYCLE_TIME_INJECTION";
      CurrentRowId = mCycleTimeInjID;

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
      SetCheckParameter("Current_Cycle_Time_Injection", new DataView(mQA.SourceData.Tables["QACycleTimeInjection"],
        "cycle_time_inj_id = '" + mCycleTimeInjID + "'", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      SetCheckParameter("Test_Tolerances_Cross_Check_Table", new DataView(mQA.SourceData.Tables["CrossCheck_TestTolerances"],
            "TestTypeCode = 'CYCLE'", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);
    }


    protected override void SetRecordIdentifier()
    {
      string GasLevel = cDBConvert.ToString(((DataRowView)GetCheckParameter("Current_Cycle_Time_Injection").ParameterValue)["gas_level_cd"]);
      RecordIdentifier = "Gas Level " + GasLevel;
    }

    #endregion

  }
}