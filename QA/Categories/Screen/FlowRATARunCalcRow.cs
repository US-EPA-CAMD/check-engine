using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

namespace ECMPS.Checks.QAScreenEvaluation
{
  public class cFlowRATARunCalcRow : cCategory
  {
    #region Private Fields

    private cQAScreenMain mQAScreenMain;
    private DataTable mFlowRATARunCalcTable;
    private string mMonitorLocationID;
    private string mRATASumID;
    private string mRATARunID;

    #endregion

    #region Constructors

    public cFlowRATARunCalcRow(cCheckEngine ACheckEngine, cQAScreenMain QAScreenMain, string MonitorLocationId, string TestSumId, DataTable FlowRATARunCalcTable)
      : base(ACheckEngine, (cProcess)QAScreenMain, "RRNCALC", FlowRATARunCalcTable)
    {
      InitializeCurrent(MonitorLocationId, TestSumId);

      mQAScreenMain = QAScreenMain;
      mFlowRATARunCalcTable = FlowRATARunCalcTable;
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
        SetCheckParameter("Current_Flow_RATA_Run", new DataView(mFlowRATARunCalcTable,
            "", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

        SetCheckParameter("Reference_Method_Code_Lookup_Table", new DataView(mQAScreenMain.SourceData.Tables["RefMethodCode"],
            "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

        mRATASumID = cDBConvert.ToString(((DataRowView)GetCheckParameter("Current_Flow_RATA_Run").ParameterValue)["RATA_SUM_ID"]);

        if (mRATASumID == "")
            SetCheckParameter("Current_RATA_Summary", null, eParameterDataType.DataRowView);
        else
            SetCheckParameter("Current_RATA_Summary", new DataView(mQAScreenMain.SourceData.Tables["QARATASummary"],
              "RATA_SUM_ID = '" + mRATASumID + "'", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

        mRATARunID = cDBConvert.ToString(((DataRowView)GetCheckParameter("Current_Flow_RATA_Run").ParameterValue)["RATA_RUN_ID"]);

        SetCheckParameter("RATA_Traverse_Records", new DataView(mQAScreenMain.SourceData.Tables["QARATATraverse"],
              "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

        SetCheckParameter("Flow_RATA_Run_Records", new DataView(mQAScreenMain.SourceData.Tables["QAFlowRATARun"],
            "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

        SetCheckParameter("Location_Attribute_Records", new DataView(mQAScreenMain.SourceData.Tables["QALocationAttribute"],
            "Mon_loc_id = '" + mMonitorLocationID + "'", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);
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
