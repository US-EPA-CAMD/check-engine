using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

namespace ECMPS.Checks.QAScreenEvaluation
{
  public class cLinearitySummaryCalcRow : cCategory
  {
    #region Private Fields

    private cQAScreenMain mQAScreenMain;
    private DataTable mLinearitySummaryCalcTable;

    #endregion

    #region Constructors

    public cLinearitySummaryCalcRow(cCheckEngine ACheckEngine, cQAScreenMain QAScreenMain, string MonitorLocationId, string TestSumId, DataTable LinearitySummaryCalcTable)
      : base(ACheckEngine, (cProcess)QAScreenMain, "LINCALC", LinearitySummaryCalcTable)
    {
      InitializeCurrent(MonitorLocationId, TestSumId);

      mQAScreenMain = QAScreenMain;
      mLinearitySummaryCalcTable = LinearitySummaryCalcTable;

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
      SetCheckParameter("Current_Linearity_Summary", new DataView(mLinearitySummaryCalcTable,
          "", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      SetCheckParameter("Current_Linearity_Test", new DataView(mQAScreenMain.SourceData.Tables["QATestSummary"],
          "", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      SetCheckParameter("Linearity_Injection_Records", new DataView(mQAScreenMain.SourceData.Tables["QALinearityInjection"],
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
