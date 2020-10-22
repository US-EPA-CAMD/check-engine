using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

namespace ECMPS.Checks.QAScreenEvaluation
{
  public class cCycleTimeInjectionRow : cCategory
  {

    #region Private Fields

    private cQAScreenMain mQAScreenMain;
    private DataTable mCycleTimeInjectionTable;
    private string mMonitorLocationID;

    #endregion

    #region Constructors

    public cCycleTimeInjectionRow(cCheckEngine ACheckEngine, cQAScreenMain QAScreenMain, string MonitorLocationId, string TestSumId, DataTable CycleTimeInjectionTable)
      : base(ACheckEngine, (cProcess)QAScreenMain, "SCRCYCI", CycleTimeInjectionTable)
    {
      InitializeCurrent(MonitorLocationId, TestSumId);

      mQAScreenMain = QAScreenMain;
      mCycleTimeInjectionTable = CycleTimeInjectionTable;
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

      SetCheckParameter("Current_Cycle_Time_Injection", new DataView(mCycleTimeInjectionTable,
        "", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      SetCheckParameter("Cycle_Time_Injection_Records", new DataView(mQAScreenMain.SourceData.Tables["QACycleTimeInjection"],
        "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Current_Cycle_Time_Test", new DataView(mQAScreenMain.SourceData.Tables["QATestSummary"],
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