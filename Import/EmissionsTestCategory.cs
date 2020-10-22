using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;

namespace ECMPS.Checks.HourlyEmissionsImport
{
  class cEmissionsTestCategory : cCategory
  {
    cEmissionsImportProcess _EMProcess = null;
    string _PrimaryKey = null;

    #region Constructors

    public cEmissionsTestCategory(cCheckEngine CheckEngine, cEmissionsImportProcess emProcess, string DTSD_PK)
      : base(CheckEngine, (cProcess)emProcess, "EMTEST")
    {
      _EMProcess = emProcess;
      _PrimaryKey = "DTSD_PK";

      TableName = "EM_DailyTestSummary";
      CurrentRowId = DTSD_PK;

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
      string sFilter = string.Format("{0}={1}", _PrimaryKey, CurrentRowId);

      DataRowView currentRecord = new DataView(_EMProcess.SourceData.Tables["WS_DailyTestSummary"], sFilter, "", DataViewRowState.CurrentRows)[0];
      SetCheckParameter("Current_Workspace_Daily_Test_Summary", currentRecord, eParameterDataType.DataRowView);

      DataView view = new DataView(_EMProcess.SourceData.Tables["WS_DailyCalibration"], null, null, DataViewRowState.CurrentRows);
      SetCheckParameter("Workspace_Daily_Calibration_Records", view, eParameterDataType.DataView);
    }

    protected override bool SetErrorSuppressValues()
    {
      ErrorSuppressValues = null;
      return true;
    }

    protected override void SetRecordIdentifier()
    {
      DataRowView CurrentRecord = (DataRowView)GetCheckParameter("Current_Workspace_Daily_Test_Summary").ParameterValue;
      RecordIdentifier = string.Format("Test Type Code {0}", CurrentRecord["TEST_TYPE_CD"].ToString());
    }

    #endregion
  }
}
