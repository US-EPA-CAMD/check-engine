using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;


namespace ECMPS.Checks.QAScreenEvaluation
{
  public class cUnitDefaultRunRow : cCategory
  {
    #region Private Fields

    private cQAScreenMain mQAScreenMain;
    private DataTable mUnitDefaultRunTable;
    private string mMonitorLocationID;
    private string mTestSumId;

    #endregion

    #region Constructors

    public cUnitDefaultRunRow(cCheckEngine ACheckEngine, cQAScreenMain QAScreenMain, string MonitorLocationId, string TestSumId, DataTable UnitDefaultRunTable)
      : base(ACheckEngine, (cProcess)QAScreenMain, "SCRUDRN", UnitDefaultRunTable)
    {
      InitializeCurrent(MonitorLocationId, TestSumId);

      mQAScreenMain = QAScreenMain;
      mUnitDefaultRunTable = UnitDefaultRunTable;
      mMonitorLocationID = MonitorLocationId;
      mTestSumId = TestSumId;

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
      SetCheckParameter("Current_Unit_Default_Run", new DataView(mUnitDefaultRunTable,
          "", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      SetCheckParameter("Current_Record", new DataView(mUnitDefaultRunTable,
        "", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      SetCheckParameter("Current_Unit_Default_Test", new DataView(mQAScreenMain.SourceData.Tables["QATestSummary"],
          "", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      SetCheckParameter("Unit_Default_Run_Records", new DataView(mQAScreenMain.SourceData.Tables["QAUnitDefaultRun"],
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
