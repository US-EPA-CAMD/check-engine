using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

namespace ECMPS.Checks.QAScreenEvaluation
{
  public class cTestClaimRow : cCategory
  {

    #region Private Fields

    private cQAScreenMain mQAScreenMain;
    private DataTable mTestClaimTable;
    private string mMonitorLocationID;
    
    #endregion

    #region Constructors

    public cTestClaimRow(cCheckEngine ACheckEngine, cQAScreenMain QAScreenMain, string MonitorLocationId, string TestSumId, DataTable TestClaimTable)
      : base(ACheckEngine, (cProcess)QAScreenMain, "SCRTSQL", TestClaimTable)
    {
      InitializeCurrent(MonitorLocationId, TestSumId);

      mQAScreenMain = QAScreenMain;
      mTestClaimTable = TestClaimTable;
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

      SetCheckParameter("Current_Test_Qualification", new DataView(mTestClaimTable,
        "", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      //mRATAID = (int)((DataRowView)GetCheckParameter("Current_Test_Qualification").ParameterValue)["RATA_ID"];

      SetCheckParameter("Current_RATA", new DataView(mQAScreenMain.SourceData.Tables["QARATA"],
        "", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      SetCheckParameter("Test_Qualification_Records", new DataView(mQAScreenMain.SourceData.Tables["QATestClaim"],
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
