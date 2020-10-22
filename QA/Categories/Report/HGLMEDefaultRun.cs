using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;
using ECMPS.ErrorSuppression;

namespace ECMPS.Checks.QAEvaluation
{
  public class cHGLMEDefaultRun : cQaTestReportCategory
  {
    #region Private Fields

    private string mMonitorLocationID;
    private cQAMain mQA;
    private string mHGLMEDefaultRunID;
    //private cHGLMEDefaultSummary mHGLMEDefaultSummary;
    //private string mSystemID;
    //private long mUnitID;
    //private string mStackPipeID;

    #endregion


    #region Constructors

    public cHGLMEDefaultRun(cCheckEngine CheckEngine, cQAMain QA, string MonitorLocationID, string HGLMEDefaultRunID, cHGLMEDefaultSummary HGLMEDefaultSummary)
      : base(QA, HGLMEDefaultSummary, "HGLMERN")
    {
      InitializeCurrent(MonitorLocationID, CheckEngine.TestSumId);

      mMonitorLocationID = MonitorLocationID;
      mQA = QA;
      mHGLMEDefaultRunID = HGLMEDefaultRunID;
      //mRATASummary = RATASummary;

      TableName = "HG_LME_DEFAULT_TEST_RUN";
      CurrentRowId = mHGLMEDefaultRunID;

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
      SetCheckParameter("Current_Hg_LME_Default_Run", new DataView(mQA.SourceData.Tables["QAHgLMEDefaultRun"],
          "HG_LME_DEFAULT_TEST_RUN_ID = '" + mHGLMEDefaultRunID + "'", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      //SetCheckParameter("Current_Hg_LME_Default_Test", new DataView(mQA.SourceData.Tables["QAHgLMEDefaultTest"], NOT NEEDED, RIGHT?
      //    "HG_DEFAULT_TEST_SUM_ID = '" + mTestSumID + "'", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      SetCheckParameter("Load_Records", new DataView(mQA.SourceData.Tables["QALoad"],
          "Mon_loc_id = '" + mMonitorLocationID + "'", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      //mSystemID = cDBConvert.ToString(((DataRowView)GetCheckParameter("Current_Hg_LME_Default_Run").ParameterValue)["mon_sys_id"]);
    }


    protected override void SetRecordIdentifier()
    {
      DataRowView RunRec = GetCheckParameter("Current_Hg_LME_Default_Run").ValueAsDataRowView();
      RecordIdentifier = "Test Location ID " + cDBConvert.ToString(RunRec["TEST_LOCATION_ID"]) + " Run Number " + cDBConvert.ToString(RunRec["RUN_NUM"]);
    }

    #endregion
  }
}
