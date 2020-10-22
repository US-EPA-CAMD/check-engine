using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

namespace ECMPS.Checks.QAScreenEvaluation
{
  public class cAppendixERow : cCategory
  {
    #region Private Fields

    private cQAScreenMain mQAScreenMain;
    private DataTable mAPPETable;
    private string mSystemID;
    private string mMonitorLocationID;

    #endregion

    #region Constructors

    public cAppendixERow(cCheckEngine ACheckEngine, cQAScreenMain QAScreenMain, string MonitorLocationID, string TestSumId, DataTable APPETable)
      : base(ACheckEngine, (cProcess)QAScreenMain, "SCRAPPE", APPETable)
    {
      InitializeCurrent(MonitorLocationID, TestSumId);

      mQAScreenMain = QAScreenMain;
      mAPPETable = APPETable;
      mMonitorLocationID = MonitorLocationID;

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
      SetCheckParameter("Current_Appendix_E_Test", new DataView(mAPPETable,
          "", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      SetCheckParameter("Current_Test", new DataView(mAPPETable,
          "", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      mSystemID = cDBConvert.ToString(((DataRowView)GetCheckParameter("Current_Appendix_E_Test").ParameterValue)["mon_sys_id"]);

      SetCheckParameter("APPE_Test_Records", new DataView(mQAScreenMain.SourceData.Tables["TestSummary"],
          "test_type_cd = 'APPE' and mon_sys_id = '" + mSystemID + "'", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("QA_Supplemental_Data_Records", new DataView(mQAScreenMain.SourceData.Tables["QASuppData"],
          "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Test_Reason_Code_Lookup_Table", new DataView(mQAScreenMain.SourceData.Tables["TestReasonCode"],
          "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Location_Test_Records", new DataView(mQAScreenMain.SourceData.Tables["TestSummary"],
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
