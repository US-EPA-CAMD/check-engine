using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

namespace ECMPS.Checks.QAScreenEvaluation
{
  public class cRATASummaryRow : cCategory
  {

    #region Private Fields

    private cQAScreenMain mQAScreenMain;
    private DataTable mRATASummaryTable;
    private string mMonitorLocationID;
    private string mRATAID;

    #endregion

    #region Constructors

    public cRATASummaryRow(cCheckEngine ACheckEngine, cQAScreenMain QAScreenMain, string MonitorLocationId, string TestSumId, DataTable RATASummaryTable)
      : base(ACheckEngine, (cProcess)QAScreenMain, "SCRRSUM", RATASummaryTable)
    {
      InitializeCurrent(MonitorLocationId, TestSumId);

      mQAScreenMain = QAScreenMain;
      mRATASummaryTable = RATASummaryTable;
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
      SetCheckParameter("Current_RATA_Summary", new DataView(mRATASummaryTable,
        "", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      SetCheckParameter("Current_Record", new DataView(mRATASummaryTable,
        "", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      mRATAID = cDBConvert.ToString(((DataRowView)GetCheckParameter("Current_RATA_Summary").ParameterValue)["RATA_ID"]);

      SetCheckParameter("Current_RATA", new DataView(mQAScreenMain.SourceData.Tables["QARATA"],
        "RATA_ID = '" + mRATAID + "'", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      SetCheckParameter("RATA_Summary_Records", new DataView(mQAScreenMain.SourceData.Tables["QARATASummary"],
        "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);
      
      SetCheckParameter("Operating_Level_Code_Lookup_Table", new DataView(mQAScreenMain.SourceData.Tables["OperatingLevelCode"],
        "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Reference_Method_Code_Lookup_Table", new DataView(mQAScreenMain.SourceData.Tables["RefMethodCode"],
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