using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

namespace ECMPS.Checks.QAScreenEvaluation
{
  public class cFF2LBASRow : cCategory
  {

    #region Private Fields

    private cQAScreenMain mQAScreenMain;
    private DataTable mFF2LBASTable;
    private string mMonitorLocationID;
    private string mMonSysID;

    #endregion

    #region Constructors

    public cFF2LBASRow(cCheckEngine ACheckEngine, cQAScreenMain QAScreenMain, string MonitorLocationId, string TestSumId, DataTable FF2LBASTable)
      : base(ACheckEngine, (cProcess)QAScreenMain, "SCRFFLB", FF2LBASTable)
    {
      InitializeCurrent(MonitorLocationId, TestSumId);

      mQAScreenMain = QAScreenMain;
      mFF2LBASTable = FF2LBASTable;
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
      SetCheckParameter("Current_Fuel_Flow_To_Load_Baseline", new DataView(mFF2LBASTable,
        "", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      SetCheckParameter("Current_Test", new DataView(mFF2LBASTable,
        "", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      mMonSysID = cDBConvert.ToString(((DataRowView)GetCheckParameter("Current_Fuel_Flow_To_Load_Baseline").ParameterValue)["mon_sys_id"]);

      SetCheckParameter("System_System_Component_Records", new DataView(mQAScreenMain.SourceData.Tables["QASystemComponent"],
        "mon_sys_id = '" + mMonSysID + "'", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      if (mMonSysID == "")
        SetCheckParameter("Associated_System", null, eParameterDataType.DataRowView);
      else
        SetCheckParameter("Associated_System", new DataView(mQAScreenMain.SourceData.Tables["QASystem"],
          "mon_sys_id = '" + mMonSysID + "'", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      SetCheckParameter("Fuel_Flow_To_Load_Baseline_Records", new DataView(mQAScreenMain.SourceData.Tables["TestSummary"],
        "test_type_cd = 'FF2LBAS' and mon_sys_id = '" + mMonSysID + "'", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Location_Test_Records", new DataView(mQAScreenMain.SourceData.Tables["TestSummary"],
        "Mon_loc_id = '" + mMonitorLocationID + "'", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("QA_Supplemental_Data_Records", new DataView(mQAScreenMain.SourceData.Tables["QASuppData"],
        "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Load_Records", new DataView(mQAScreenMain.SourceData.Tables["QALoad"],
          "Mon_loc_id = '" + mMonitorLocationID + "'", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Fuel_Flow_To_Load_Baseline_Uom_To_Load_Uom_And_Systemtype_Cross_Check_Table", new DataView(mQAScreenMain.SourceData.Tables["Crosscheck_FuelFlowtoLoadBaselineUOMtoLoadUOMandSystemType"],
          "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Parameter_Units_Of_Measure_Lookup_Table", new DataView(mQAScreenMain.SourceData.Tables["UnitsOfMeasureLookup"],
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
