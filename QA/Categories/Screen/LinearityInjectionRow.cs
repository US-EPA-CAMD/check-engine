using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

namespace ECMPS.Checks.QAScreenEvaluation
{
  public class cLinearityInjectionRow : cCategory
  {

    #region Private Fields

    private cQAScreenMain mQAScreenMain;
    private DataTable mLinearityInjectionTable;
    //private string mGasLevelCd;

    #endregion

    #region Constructors

    public cLinearityInjectionRow(cCheckEngine ACheckEngine, cQAScreenMain QAScreenMain, string MonitorLocationId, string TestSumId, DataTable LinearityInjectionTable)
      : base(ACheckEngine, (cProcess)QAScreenMain, "SCRLINJ", LinearityInjectionTable)
    {
      InitializeCurrent(MonitorLocationId, TestSumId);

      mQAScreenMain = QAScreenMain;
      mLinearityInjectionTable = LinearityInjectionTable;

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
      SetCheckParameter("Current_Linearity_Injection", new DataView(mLinearityInjectionTable,
        "", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

//      mGasLevelCd = cDBConvert.ToString(((DataRowView)GetCheckParameter("Current_Linearity_Injection").ParameterValue)["gas_level_cd"]);

      SetCheckParameter("Current_Linearity_Test", new DataView(mQAScreenMain.SourceData.Tables["QATestSummary"],
        "", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      SetCheckParameter("Current_Linearity_Summary", new DataView(mLinearityInjectionTable,
        "", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

//      SetCheckParameter("Current_Linearity_Summary", new DataView(mQAScreenMain.SourceData.Tables["QALinearitySummary"],
//        "GAS_LEVEL_CD = '" + mGasLevelCd + "'", "", DataViewRowState.CurrentRows)[0], ParameterTypes.DATAROW);

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
