using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

namespace ECMPS.Checks.QAScreenEvaluation
{
  public class cFlowRATARunRow : cCategory
  {
    #region Private Fields

    private cQAScreenMain mQAScreenMain;
    private DataTable mFlowRATARunTable;
    private string mMonitorLocationID;
    private string mRATARunID;
    private int mRunNumber;
    private string mOpLevelCd;
    private DataView mRATARun;

    #endregion

    #region Constructors

    public cFlowRATARunRow(cCheckEngine ACheckEngine, cQAScreenMain QAScreenMain, string MonitorLocationId, string TestSumId, DataTable FlowRATARunTable)
      : base(ACheckEngine, (cProcess)QAScreenMain, "SCRFRUN", FlowRATARunTable)
    {
      InitializeCurrent(MonitorLocationId, TestSumId);

      mQAScreenMain = QAScreenMain;
      mFlowRATARunTable = FlowRATARunTable;
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
      SetCheckParameter("Current_Flow_RATA_Run", new DataView(mFlowRATARunTable,
        "", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      SetCheckParameter("Current_Record", new DataView(mFlowRATARunTable,
        "", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      mRATARunID = cDBConvert.ToString(((DataRowView)GetCheckParameter("Current_Flow_RATA_Run").ParameterValue)["RATA_RUN_ID"]);

      if (mRATARunID == "")
      {
        mOpLevelCd = cDBConvert.ToString(((DataRowView)GetCheckParameter("Current_Flow_RATA_Run").ParameterValue)["OP_LEVEL_CD"]);
        mRunNumber = cDBConvert.ToInteger(((DataRowView)GetCheckParameter("Current_Flow_RATA_Run").ParameterValue)["RUN_NUM"]);
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
        SetCheckParameter("Current_RATA_Run", new DataView(mQAScreenMain.SourceData.Tables["QARATARun"],
          "RATA_RUN_ID = '" + mRATARunID + "'", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      SetCheckParameter("Flow_RATA_Run_Records", new DataView(mQAScreenMain.SourceData.Tables["QAFlowRATARun"],
        "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Operating_Level_Code_Lookup_Table", new DataView(mQAScreenMain.SourceData.Tables["OperatingLevelCode"],
        "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Current_RATA", new DataView(mQAScreenMain.SourceData.Tables["QARATA"],
            "", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

        SetCheckParameter("Test_Tolerances_Cross_Check_Table", new DataView(mQAScreenMain.SourceData.Tables["CrossCheck_TestTolerances"],
            "TestTypeCode = 'RATA'", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

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