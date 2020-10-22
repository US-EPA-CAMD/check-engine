using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

namespace ECMPS.Checks.QAScreenEvaluation
{
  public class cRATARunRow : cCategory
  {

    #region Private Fields

    private cQAScreenMain mQAScreenMain;
    private DataTable mRATARunTable;
    private string mMonitorLocationID;
    private string mTestSumId;
    //private int mRATASumID;

    #endregion

    #region Constructors

    public cRATARunRow(cCheckEngine ACheckEngine, cQAScreenMain QAScreenMain, string MonitorLocationId, string TestSumId, DataTable RATARunTable)
      : base(ACheckEngine, (cProcess)QAScreenMain, "SCRRRUN", RATARunTable)
    {
      InitializeCurrent(MonitorLocationId, TestSumId);

      mQAScreenMain = QAScreenMain;
      mRATARunTable = RATARunTable;
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
      SetCheckParameter("Current_RATA_Run", new DataView(mRATARunTable,
        "", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      SetCheckParameter("Current_Record", new DataView(mRATARunTable,
        "", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      //mRATASumID = (int)((DataRowView)GetCheckParameter("Current_RATA_Run").ParameterValue)["RATA_SUM_ID"];

      //SetCheckParameter("Current_RATA_Summary", new DataView(mQAScreenMain.SourceData.Tables["QARATASummary"],
      //  "RATA_SUM_ID = " + mRATASumID, "", DataViewRowState.CurrentRows)[0], ParameterTypes.DATAROW);

      SetCheckParameter("Current_RATA", new DataView(mQAScreenMain.SourceData.Tables["QARATA"],
         "", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      SetCheckParameter("RATA_Run_Records", new DataView(mQAScreenMain.SourceData.Tables["QARATARun"],
        "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Operating_Level_Code_Lookup_Table", new DataView(mQAScreenMain.SourceData.Tables["OperatingLevelCode"],
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