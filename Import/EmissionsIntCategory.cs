using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;

namespace ECMPS.Checks.HourlyEmissionsImport
{
  class cEmissionsIntCategory : cCategory
  {
    cEmissionsImportProcess _EMProcess = null;
    DataRowView _currentRecord = null;

    #region Constructors

    public cEmissionsIntCategory(cCheckEngine CheckEngine, cEmissionsImportProcess emProcess)
      : base(CheckEngine, (cProcess)emProcess, "EMINT")
    {
      _EMProcess = emProcess;

      _currentRecord = new DataView(_EMProcess.SourceData.Tables["WS_Emissions"], null, null, DataViewRowState.CurrentRows)[0];

      TableName = "EM_Emissions";
      CurrentRowId = _currentRecord["EM_PK"].ToString();

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
      SetCheckParameter("Current_Workspace_Emission_File", _currentRecord, eParameterDataType.DataRowView);

      DataView view = new DataView(_EMProcess.SourceData.Tables["PROD_Facility"], null, null, DataViewRowState.CurrentRows);
      SetCheckParameter("Production_Facility_Records", view, eParameterDataType.DataView);

      view = new DataView(_EMProcess.SourceData.Tables["WS_EMDates"], null, null, DataViewRowState.CurrentRows);
      SetCheckParameter("Workspace_Date_Records", view, eParameterDataType.DataView);

      view = new DataView(_EMProcess.SourceData.Tables["WS_EMLocations"], null, "MON_LOC_TYPE, STACK_NAME, UNITID", DataViewRowState.CurrentRows);
      SetCheckParameter("Workspace_EM_Location_Records", view, eParameterDataType.DataView);

      view = new DataView(_EMProcess.SourceData.Tables["WS_Unit"], null, null, DataViewRowState.CurrentRows);
      SetCheckParameter("Workspace_Unit_Records", view, eParameterDataType.DataView);

      view = new DataView(_EMProcess.SourceData.Tables["PROD_Monitor_Plan_Location"], null, null, DataViewRowState.CurrentRows);
      SetCheckParameter("Production_Monitor_Plan_Location_Records", view, eParameterDataType.DataView);
    }

    protected override bool SetErrorSuppressValues()
    {
      ErrorSuppressValues = null;
      return true;
    }

    protected override void SetRecordIdentifier()
    {
      RecordIdentifier = string.Format("ORIS Code {0}", _currentRecord["ORIS_CODE"].ToString());
    }

    #endregion
  }
}
