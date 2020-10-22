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
  public class cCalibrationInjection : cQaTestReportCategory
  {

    #region Private Fields

    private string mMonitorLocationID;
    private cQAMain mQA;
    private string mCalInjID;
    //private cCalibration mCalibration;


    #endregion


    #region Constructors

    public cCalibrationInjection(cCheckEngine CheckEngine, cQAMain QA, string MonitorLocationID, string CalInjID, cCalibration Calibration)
      : base(QA, Calibration, "7DAYINJ")
    {
      InitializeCurrent(MonitorLocationID, Calibration.TestSumID);

      mMonitorLocationID = MonitorLocationID;
      mQA = QA;
      mCalInjID = CalInjID;
      //mCalibration = Calibration;

      TableName = "CALIBRATION_INJECTION";
      CurrentRowId = mCalInjID;

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
      SetCheckParameter("Current_Calibration_Injection", new DataView(mQA.SourceData.Tables["QACalibrationInjection"],
        "cal_inj_id = '" + mCalInjID + "'", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);
    }

    protected override void SetRecordIdentifier()
    {
      string InjectionDate = cDBConvert.ToDate(((DataRowView)GetCheckParameter("Current_Calibration_Injection").ParameterValue)["zero_Injection_Date"], DateTypes.START).ToShortDateString();
      RecordIdentifier = "Zero Injection Date " + InjectionDate;
    }

    #endregion

  }
}