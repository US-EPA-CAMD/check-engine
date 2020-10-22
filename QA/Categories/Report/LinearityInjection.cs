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
  public class cLinearityInjection : cQaTestReportCategory
  {

    #region Private Fields

    private string mMonitorLocationID;
    private cQAMain mQA;
    private string mLinInjID;
    //private cLinearitySummary mLinearitySummary;
    private string mComponentID;


    //private long mUnitID;
    //private string mStackPipeID;

    #endregion


    #region Constructors

    public cLinearityInjection(cCheckEngine CheckEngine, cQAMain QA, string MonitorLocationID, string LinInjID, cLinearitySummary LinearitySummary)
      : base(QA, LinearitySummary, "LININJ")
    {
      InitializeCurrent(MonitorLocationID, CheckEngine.TestSumId);

      mMonitorLocationID = MonitorLocationID;
      mQA = QA;
      mLinInjID = LinInjID;
      //mLinearitySummary = LinearitySummary;

      TableName = "LINEARITY_INJECTION";
      CurrentRowId = mLinInjID;

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
      // Process.ElapsedTimes.TimingBegin("FilterData", this);

      SetCheckParameter("Current_Linearity_Injection", new DataView(mQA.SourceData.Tables["QALinearityInjection"],
          "lin_inj_id = '" + mLinInjID + "'", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      mComponentID = cDBConvert.ToString(((DataRowView)GetCheckParameter("Current_Linearity_Injection").ParameterValue)["component_id"]);

      SetCheckParameter("Component_Linearity_Injection_Records", new DataView(mQA.SourceData.Tables["ComponentLinearityInjection"],
          "component_id = '" + mComponentID + "'", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      // Process.ElapsedTimes.TimingEnd("FilterData", this);
    }


    protected override void SetRecordIdentifier()
    {
      DateTime mInjDate = cDBConvert.ToDate(((DataRowView)GetCheckParameter("Current_Linearity_Injection").ParameterValue)["injection_date"], DateTypes.START);
      string mInjDateStr, mInjHour, mInjMin;
      if (mInjDate == DateTime.MinValue)
        mInjDateStr = "";
      else
        mInjDateStr = mInjDate.ToShortDateString() + " ";
      int TempVal = cDBConvert.ToInteger(((DataRowView)GetCheckParameter("Current_Linearity_Injection").ParameterValue)["injection_hour"]);
      if (TempVal == int.MinValue)
        mInjHour = "";
      else
        mInjHour = Convert.ToString(TempVal);
      TempVal = cDBConvert.ToInteger(((DataRowView)GetCheckParameter("Current_Linearity_Injection").ParameterValue)["injection_min"]);
      if (TempVal == int.MinValue)
        mInjMin = "";
      else
        mInjMin = Convert.ToString(TempVal);

      RecordIdentifier = "Injection for Gas Calibration Level " +
        cDBConvert.ToString(((DataRowView)GetCheckParameter("Current_Linearity_Injection").ParameterValue)["gas_level_cd"]) +
        " that was performed on " + mInjDateStr + mInjHour + ":" + mInjMin;
    }

    #endregion

  }
}