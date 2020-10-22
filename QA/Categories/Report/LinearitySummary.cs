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
  public class cLinearitySummary : cQaTestReportCategory
  {

    #region Private Fields

    private string mMonitorLocationID;
    private cQAMain mQA;
    private string mLinSumID;
    //private cLinearity mLinearity;

    //private long mUnitID;
    //private string mStackPipeID;

    #endregion


    #region Constructors

    public cLinearitySummary(cCheckEngine CheckEngine, cQAMain QA, string MonitorLocationID, string LinSumID, cLinearity Linearity)
      : base(QA, Linearity, "LINSUM")
    {
      InitializeCurrent(MonitorLocationID, Linearity.TestSumID);

      mMonitorLocationID = MonitorLocationID;
      mQA = QA;
      mLinSumID = LinSumID;
      //mLinearity = Linearity;

      TableName = "LINEARITY_SUMMARY";
      CurrentRowId = mLinSumID;

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

      SetCheckParameter("Current_Linearity_Summary", new DataView(mQA.SourceData.Tables["QALinearitySummary"],
        "lin_sum_id = '" + mLinSumID + "'", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      SetCheckParameter("Test_Tolerances_Cross_Check_Table", new DataView(mQA.SourceData.Tables["CrossCheck_TestTolerances"],
        "TestTypeCode in ('LINE','HGLINE','HGSI3')", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      //SetCheckParameter("Linearity_Test_Injection_Records", new DataView(mQA.SourceData.Tables["QALinearityInjection"],
      //  "lin_sum_id = " + Convert.ToString(mLinSumID), "injection_date, injection_hour, injection_min", DataViewRowState.CurrentRows), ParameterTypes.DATAVW);

      //SetCheckParameter("Test_Reason_Code_Lookup_Table", new DataView(mQA.SourceData.Tables["TestReasonCode"],
      //  "", "", DataViewRowState.CurrentRows), ParameterTypes.DATAVW);

      // Process.ElapsedTimes.TimingEnd("FilterData", this);
    }


    protected override void SetRecordIdentifier()
    {
      string GasLevelCd = cDBConvert.ToString(((DataRowView)GetCheckParameter("Current_Linearity_Summary").ParameterValue)["gas_level_cd"]);
      RecordIdentifier = "Calibration Gas Level " + GasLevelCd;
    }

    #endregion

  }
}