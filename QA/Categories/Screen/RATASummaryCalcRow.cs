using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

namespace ECMPS.Checks.QAScreenEvaluation
{
  public class cRATASummaryCalcRow : cCategory
  {
    #region Private Fields

    private cQAScreenMain mQAScreenMain;
    private DataTable mRATASummaryCalcTable;
    private string mMonitorLocationID;
    private string mRATAID;

    #endregion

    #region Constructors

    public cRATASummaryCalcRow(cCheckEngine ACheckEngine, cQAScreenMain QAScreenMain, string MonitorLocationId, string TestSumId, DataTable RATASummaryCalcTable)
      : base(ACheckEngine, (cProcess)QAScreenMain, "RSMCALC", RATASummaryCalcTable)
    {
      InitializeCurrent(MonitorLocationId, TestSumId);

      mQAScreenMain = QAScreenMain;
      mRATASummaryCalcTable = RATASummaryCalcTable;
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
      SetCheckParameter("Current_RATA_Summary", new DataView(mRATASummaryCalcTable,
          "", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      mRATAID = cDBConvert.ToString(((DataRowView)GetCheckParameter("Current_RATA_Summary").ParameterValue)["RATA_ID"]);

      SetCheckParameter("Current_RATA", new DataView(mQAScreenMain.SourceData.Tables["QARATA"],
          "RATA_ID = '" + mRATAID + "'", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      SetCheckParameter("RATA_Run_Records", new DataView(mQAScreenMain.SourceData.Tables["QARATARun"],
          "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Reference_Method_Code_Lookup_Table", new DataView(mQAScreenMain.SourceData.Tables["RefMethodCode"],
          "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("TValues_Cross_Check_Table", new DataView(mQAScreenMain.SourceData.Tables["CrossCheck_T-Values"],
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
