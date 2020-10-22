using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

namespace ECMPS.Checks.QAScreenEvaluation
{
  public class cCalibrationInjectionCalcRow : cCategory
  {
    #region Private Fields

    private cQAScreenMain mQAScreenMain;
    private DataTable mCalibrationInjectionCalcTable;
    private string mMonitorLocationID;
    private string mComponentID;

    #endregion

    #region Constructors

    public cCalibrationInjectionCalcRow(cCheckEngine ACheckEngine, cQAScreenMain QAScreenMain, string MonitorLocationId, string TestSumId, DataTable CalibrationInjectionCalcTable)
      : base(ACheckEngine, (cProcess)QAScreenMain, "7DYCALC", CalibrationInjectionCalcTable)
    {
      InitializeCurrent(MonitorLocationId, TestSumId);

      mQAScreenMain = QAScreenMain;
      mCalibrationInjectionCalcTable = CalibrationInjectionCalcTable;
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
      SetCheckParameter("Current_Calibration_Injection", new DataView(mCalibrationInjectionCalcTable,
          "", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      SetCheckParameter("Span_Records", new DataView(mQAScreenMain.SourceData.Tables["QASpan"],
          "Mon_loc_id = '" + mMonitorLocationID + "'", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Current_7Day_Calibration_Test", new DataView(mQAScreenMain.SourceData.Tables["QATestSummary"],
          "", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      mComponentID = cDBConvert.ToString(((DataRowView)GetCheckParameter("Current_7Day_Calibration_Test").ParameterValue)["component_id"]);

      SetCheckParameter("System_Component_Records", new DataView(mQAScreenMain.SourceData.Tables["QASystemComponent"],
          "component_id = '" + mComponentID + "'", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);
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
