using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

namespace ECMPS.Checks.QAScreenEvaluation
{
  class cFlowToLoadReferenceCalcRow : cCategory
  {
    #region Private Fields

    private cQAScreenMain mQAScreenMain;
    private DataTable mFlowToLoadReferenceCalcTable;
    private string mMonitorLocationID;
    private string mMonSysId;

    #endregion

    #region Constructors

    public cFlowToLoadReferenceCalcRow(cCheckEngine ACheckEngine, cQAScreenMain QAScreenMain, string MonitorLocationId, string TestSumId, DataTable FlowToLoadReferenceCalcTable)
      : base(ACheckEngine, (cProcess)QAScreenMain, "F2LCALC", FlowToLoadReferenceCalcTable)
    {
      InitializeCurrent(MonitorLocationId, TestSumId);

      mQAScreenMain = QAScreenMain;
      mFlowToLoadReferenceCalcTable = FlowToLoadReferenceCalcTable;
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
        SetCheckParameter("Current_Flow_To_Load_Reference", new DataView(mFlowToLoadReferenceCalcTable,
            "", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

        SetCheckParameter("Current_Monitor_Location", new DataView(mQAScreenMain.SourceData.Tables["MonitorLocation"],
            "", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataView);

        mMonSysId = cDBConvert.ToString(((DataRowView)GetCheckParameter("Current_Flow_To_Load_Reference").ParameterValue)["MON_SYS_ID"]);

        SetCheckParameter("Facility_RATA_Run_Records", new DataView(mQAScreenMain.SourceData.Tables["QARATARun"],
            "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

        SetCheckParameter("Facility_RATA_Summary_Records", new DataView(mQAScreenMain.SourceData.Tables["QARATASummary"],
            "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

        SetCheckParameter("Unit_Stack_Configuration_Records", new DataView(mQAScreenMain.SourceData.Tables["QAUnitStackConfiguration"],
            "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);
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
