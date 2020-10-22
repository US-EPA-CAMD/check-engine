using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

namespace ECMPS.Checks.QAScreenEvaluation
{
  public class cTEERow : cCategory
  {
    #region Private Fields

    private cQAScreenMain mQAScreenMain;
    private DataTable mTEETable;
    private string mMonitorLocationID;
    private string mSystemID;

    #endregion

    #region Constructors

    public cTEERow(cCheckEngine ACheckEngine, cQAScreenMain QAScreenMain, string MonitorLocationId, string TestSumId, DataTable TEETable)
      : base(ACheckEngine, (cProcess)QAScreenMain, "SCRTEE", TEETable)
    {
      InitializeCurrent(MonitorLocationId, TestSumId);

      mQAScreenMain = QAScreenMain;
      mTEETable = TEETable;
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
      SetCheckParameter("Current_Test_Extension_Exemption", new DataView(mTEETable,
          "", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      mSystemID = cDBConvert.ToString(((DataRowView)GetCheckParameter("Current_Test_Extension_Exemption").ParameterValue)["mon_sys_id"]);

      SetCheckParameter("Test_Extension_Exemption_Records", new DataView(mQAScreenMain.SourceData.Tables["QATEE"],
              "mon_loc_id = '" + mMonitorLocationID + "'", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Monitor_System_Records", new DataView(mQAScreenMain.SourceData.Tables["MonitorSystemRecords"],
          "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Component_Records", new DataView(mQAScreenMain.SourceData.Tables["QAComponent"],
          "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      // I do not believe that this parameter is used in this category. (djw2)
      //SetCheckParameter("System_Records", new DataView(mQAScreenMain.SourceData.Tables["QASystem"],
      //    "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Location_Analyzer_Range_Records", new DataView(mQAScreenMain.SourceData.Tables["AnalyzerRangeRecords"],
          "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Location_Reporting_Frequency_Records", new DataView(mQAScreenMain.SourceData.Tables["LocationReportingFrequency"],
          "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Location_System_Component_Records", new DataView(mQAScreenMain.SourceData.Tables["LocationSystemComponent"],
          "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Fuel_Code_Lookup_Table", new DataView(mQAScreenMain.SourceData.Tables["FuelCodeLookup"],
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
