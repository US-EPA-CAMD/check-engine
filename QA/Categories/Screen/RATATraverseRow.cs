using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

namespace ECMPS.Checks.QAScreenEvaluation
{
  public class cRATATraverseRow : cCategory
  {

    #region Private Fields

    private cQAScreenMain mQAScreenMain;
    private DataTable mRATATraverseTable;
    private string mMonitorLocationID, mOpLevelCd;
    private string mFlowRATARunID;
    private int mRunNumber;
    private DataView mFlowRATARun, mRATARun;


    #endregion

    #region Constructors

    public cRATATraverseRow(cCheckEngine ACheckEngine, cQAScreenMain QAScreenMain, string MonitorLocationId, string TestSumId, DataTable RATATraverseTable)
      : base(ACheckEngine, (cProcess)QAScreenMain, "SCRRTRV", RATATraverseTable)
    {
      InitializeCurrent(MonitorLocationId, TestSumId);

      mQAScreenMain = QAScreenMain;
      mRATATraverseTable = RATATraverseTable;
      mMonitorLocationID = MonitorLocationId;

      FilterData();

      SetRecordIdentifier();
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Sets the checkbands for this category to the passed check bands and then executes
    /// those checks.
    /// </summary>
    /// <param name="ACheckBands">The check bands to process.</param>
    /// <returns>True if the processing of check executed normally.</returns>
    public bool ProcessChecks(cCheckParameterBands ACheckBands)
    {
      this.SetCheckBands(ACheckBands);
      return base.ProcessChecks();
    }

    #endregion

    #region Base Class Overrides

    protected override void FilterData()
    {

      SetCheckParameter("Current_RATA_Traverse", new DataView(mRATATraverseTable,
        "", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      SetCheckParameter("Current_Record", new DataView(mRATATraverseTable,
        "", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      mFlowRATARunID = cDBConvert.ToString(((DataRowView)GetCheckParameter("Current_RATA_Traverse").ParameterValue)["FLOW_RATA_RUN_ID"]);

      if (mFlowRATARunID == "")
      {
        mOpLevelCd = cDBConvert.ToString(((DataRowView)GetCheckParameter("Current_RATA_Traverse").ParameterValue)["OP_LEVEL_CD"]);
        mRunNumber = cDBConvert.ToInteger(((DataRowView)GetCheckParameter("Current_RATA_Traverse").ParameterValue)["RUN_NUM"]);
        if (mOpLevelCd != "" && mRunNumber > 0)
        {
          mFlowRATARun = new DataView(mQAScreenMain.SourceData.Tables["QAFlowRATARun"],
                    "OP_LEVEL_CD = '" + mOpLevelCd + "' and RUN_NUM = " + mRunNumber, "", DataViewRowState.CurrentRows);
          if (mFlowRATARun.Count > 0)
            SetCheckParameter("Current_Flow_RATA_Run", mFlowRATARun[0], eParameterDataType.DataRowView);
          else
            SetCheckParameter("Current_Flow_RATA_Run", null, eParameterDataType.DataRowView);
        }
        else
          SetCheckParameter("Current_Flow_RATA_Run", null, eParameterDataType.DataRowView);
      }
      else
        SetCheckParameter("Current_Flow_RATA_Run", new DataView(mQAScreenMain.SourceData.Tables["QAFlowRATARun"],
          "FLOW_RATA_RUN_ID = '" + mFlowRATARunID + "'", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      if (mFlowRATARunID == "")
      {
        if (mOpLevelCd != "" && mRunNumber > 0)
        {
          mRATARun = new DataView(mQAScreenMain.SourceData.Tables["QARATARun"],
                    "OP_LEVEL_CD = '" + mOpLevelCd + "' and RUN_NUM = " + mRunNumber, "", DataViewRowState.CurrentRows);
          if (mRATARun.Count > 0)
            SetCheckParameter("Current_RATA_Run", mRATARun[0], eParameterDataType.DataRowView);
          else
            SetCheckParameter("Current_RATA_Run", null, eParameterDataType.DataRowView);
        }
        else
          SetCheckParameter("Current_RATA_Run", null, eParameterDataType.DataRowView);
      }
      else
        SetCheckParameter("Current_RATA_Run", new DataView(mQAScreenMain.SourceData.Tables["QAFlowRATARun"],//Using Flow tbl for Run parameter
          "FLOW_RATA_RUN_ID = '" + mFlowRATARunID + "'", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      SetCheckParameter("RATA_Traverse_Records", new DataView(mQAScreenMain.SourceData.Tables["QARATATraverse"],
        "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Operating_Level_Code_Lookup_Table", new DataView(mQAScreenMain.SourceData.Tables["OperatingLevelCode"],
        "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Pressure_Measure_Code_Lookup_Table", new DataView(mQAScreenMain.SourceData.Tables["PressureMeasureCode"],
        "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Current_RATA", new DataView(mQAScreenMain.SourceData.Tables["QARATA"],
          "", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

    }
    protected override void SetRecordIdentifier()
    {

      RecordIdentifier = "this record";

    }

    protected override bool SetErrorSuppressValues()
    {
        ErrorSuppressValues = null;
        return true;
    }

    #endregion

  }
}