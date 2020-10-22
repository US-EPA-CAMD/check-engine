using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

namespace ECMPS.Checks.QAScreenEvaluation
{
  public class cRATACalcRow : cCategory
  {
    #region Private Fields

    private cQAScreenMain mQAScreenMain;
    private DataTable mRATACalcTable;
    private string mMonitorLocationID;

    #endregion

    #region Constructors

    public cRATACalcRow(cCheckEngine ACheckEngine, cQAScreenMain QAScreenMain, string MonitorLocationId, string TestSumId, DataTable RATACalcTable)
      : base(ACheckEngine, (cProcess)QAScreenMain, "RATCALC", RATACalcTable)
    {
      InitializeCurrent(MonitorLocationId, TestSumId);

      mQAScreenMain = QAScreenMain;
      mRATACalcTable = RATACalcTable;
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
      SetCheckParameter("Current_RATA", new DataView(mRATACalcTable,
          "", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      SetCheckParameter("RATA_Summary_Records", new DataView(mQAScreenMain.SourceData.Tables["QARATASummary"],
          "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Monitor_System_Records", new DataView(mQAScreenMain.SourceData.Tables["MonitorSystemRecords"],
          "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Load_Records", new DataView(mQAScreenMain.SourceData.Tables["QALoad"],
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
