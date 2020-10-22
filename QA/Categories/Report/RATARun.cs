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
  public class cRATARun : cQaTestReportCategory
  {

    #region Private Fields

    private string mMonitorLocationID;
    private cQAMain mQA;
    private string mRATARunID;
    //private cRATASummary mRATASummary;
    private string mSystemID;


    //private long mUnitID;
    //private string mStackPipeID;

    #endregion


    #region Constructors

    public cRATARun(cCheckEngine CheckEngine, cQAMain QA, string MonitorLocationID, string RATARunID, cRATASummary RATASummary)
      : base(QA, RATASummary, "RATARUN")
    {
      InitializeCurrent(MonitorLocationID, CheckEngine.TestSumId);

      mMonitorLocationID = MonitorLocationID;
      mQA = QA;
      mRATARunID = RATARunID;
      //mRATASummary = RATASummary;

      TableName = "RATA_RUN";
      CurrentRowId = mRATARunID;

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
      if (mQA.SourceData.Tables["QARATARun"] != null)
      {
        DataView rataRunView = new DataView(mQA.SourceData.Tables["QARATARun"], "rata_run_id = '" + mRATARunID + "'", "", DataViewRowState.CurrentRows);

        if (rataRunView.Count > 0)
        {
          SetCheckParameter("Current_RATA_Run", rataRunView[0], eParameterDataType.DataRowView);
          mSystemID = cDBConvert.ToString(((DataRowView)GetCheckParameter("Current_RATA_Run").ParameterValue)["mon_sys_id"]);
        }
        else
        {
          SetCheckParameter("Current_RATA_Run", null, eParameterDataType.DataRowView);
          mSystemID = null;
        }
      }
      else
      {
        SetCheckParameter("Current_RATA_Run", null, eParameterDataType.DataRowView);
        mSystemID = null;
      }
    }


    protected override void SetRecordIdentifier()
    {
      DataRowView currentRataRun = (DataRowView)GetCheckParameter("Current_RATA_Run").ParameterValue;

      if (currentRataRun != null)
      {
        RecordIdentifier = "Operating Level " + cDBConvert.ToString(currentRataRun["op_level_cd"]) +
                           " Run Number " + cDBConvert.ToInteger(((DataRowView)GetCheckParameter("Current_RATA_Run").ParameterValue)["run_num"]);
      }
      else
        RecordIdentifier = null;
    }

    #endregion

  }
}