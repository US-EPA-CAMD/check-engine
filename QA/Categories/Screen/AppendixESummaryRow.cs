using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

namespace ECMPS.Checks.QAScreenEvaluation
{
  public class cAppendixESummaryRow : cCategory
  {
    #region Private Fields

    private cQAScreenMain mQAScreenMain;
    private DataTable mAPPESummaryTable;
    private string mMonitorLocationID;

    #endregion

    #region Constructors

    public cAppendixESummaryRow(cCheckEngine ACheckEngine, cQAScreenMain QAScreenMain, string MonitorLocationId, string TestSumId, DataTable APPESummaryTable)
      : base(ACheckEngine, (cProcess)QAScreenMain, "SCRAESM", APPESummaryTable)
    {
      InitializeCurrent(MonitorLocationId, TestSumId);

      mQAScreenMain = QAScreenMain;
      mAPPESummaryTable = APPESummaryTable;
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
      SetCheckParameter("Current_Appendix_E_Summary", new DataView(mAPPESummaryTable,
        "", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      //SetCheckParameter("Current_Record", new DataView(mAPPESummaryTable,
      //  "", "", DataViewRowState.CurrentRows)[0], ParameterTypes.DATAROW);

      //mRATAID = (int)((DataRowView)GetCheckParameter("Current_RATA_Summary").ParameterValue)["RATA_ID"];

      SetCheckParameter("Current_Appendix_E_Test", new DataView(mQAScreenMain.SourceData.Tables["QATestSummary"],
        "", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      SetCheckParameter("APPE_Summary_Records", new DataView(mQAScreenMain.SourceData.Tables["QAAppendixESummary"],
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
