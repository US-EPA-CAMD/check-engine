using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

namespace ECMPS.Checks.QAScreenEvaluation
{
  public class cFF2LTSTRow : cCategory
  {

    #region Private Fields

    private cQAScreenMain mQAScreenMain;
    private DataTable mFF2LTSTTable;
    private string mSystemID;
    private string mMonitorLocationID;

    #endregion

    #region Constructors

    public cFF2LTSTRow(cCheckEngine ACheckEngine, cQAScreenMain QAScreenMain, string MonitorLocationId, string TestSumId, DataTable FF2LTSTTable)
      : base(ACheckEngine, (cProcess)QAScreenMain, "SCRFFLT", FF2LTSTTable)
    {
      InitializeCurrent(MonitorLocationId, TestSumId);

      mQAScreenMain = QAScreenMain;
      mFF2LTSTTable = FF2LTSTTable;
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
      SetCheckParameter("Current_Fuel_Flow_To_Load_Test", new DataView(mFF2LTSTTable,
        "", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      SetCheckParameter("Current_Test", new DataView(mFF2LTSTTable,
        "", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      mSystemID = cDBConvert.ToString(((DataRowView)GetCheckParameter("Current_Fuel_Flow_To_Load_Test").ParameterValue)["mon_sys_id"]);

      SetCheckParameter("Fuel_Flow_To_Load_Test_Records", new DataView(mQAScreenMain.SourceData.Tables["TestSummary"],
        "test_type_cd = 'FF2LTST' and mon_sys_id = '" + mSystemID + "'", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Location_Test_Records", new DataView(mQAScreenMain.SourceData.Tables["TestSummary"],
        "Mon_loc_id = '" + mMonitorLocationID + "'", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("QA_Supplemental_Data_Records", new DataView(mQAScreenMain.SourceData.Tables["QASuppData"],
        "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Operating_Level_Code_Lookup_Table", new DataView(mQAScreenMain.SourceData.Tables["OperatingLevelCode"],
        "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Test_Reason_Code_Lookup_Table", new DataView(mQAScreenMain.SourceData.Tables["TestReasonCode"],
        "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Test_Result_Code_Lookup_Table", new DataView(mQAScreenMain.SourceData.Tables["TestResultCode"],
        "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Test_Basis_Code_Lookup_Table", new DataView(mQAScreenMain.SourceData.Tables["TestBasisCode"],
        "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Reporting_Period_Lookup_Table", new DataView(mQAScreenMain.SourceData.Tables["ReportingPeriodLookup"],
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
