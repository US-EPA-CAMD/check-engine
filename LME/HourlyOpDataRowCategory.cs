using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Extensions;

namespace ECMPS.Checks.LME
{
  public class cHourlyOpDataRowCategory : cCategory
  {
    #region Private Fields

    private cLMEScreenProcess mLMEScreenProcess;
    private DataTable mHourlyOpDataTable;

    #endregion

    #region Constructors

    public cHourlyOpDataRowCategory(cCheckEngine CheckEngine, cLMEScreenProcess LMEScreenProcess, DataTable HourlyOpDataTable)
      : base(CheckEngine, (cProcess)LMEScreenProcess, "LMEHRLY", HourlyOpDataTable)
    {
      mLMEScreenProcess = LMEScreenProcess;

      mHourlyOpDataTable = HourlyOpDataTable;
    }

    public cHourlyOpDataRowCategory(cCheckEngine CheckEngine, cLMEScreenProcess LMEScreenProcess)
      : base(CheckEngine, (cProcess)LMEScreenProcess, "LMEHRLY")
    {
    }


    #endregion

    #region Public Methods

    public new bool ProcessChecks(string MonLocID)
    {
      return base.ProcessChecks(MonLocID);
    }

    #endregion

    #region Base Class Overrides

    protected override void FilterData()
    {
      DataRowView currentLmeHourlyOpRecord = new DataView(mHourlyOpDataTable, "", "", DataViewRowState.CurrentRows)[0];

      string monLocId = currentLmeHourlyOpRecord["MON_LOC_ID"].AsString();
      DateTime opDate = currentLmeHourlyOpRecord["BEGIN_DATE"].AsDateTime(DateTime.MinValue);
      int opHour = currentLmeHourlyOpRecord["BEGIN_HOUR"].AsInteger(0);

      SetCheckParameter("Current_LME_Hourly_Op_Record", currentLmeHourlyOpRecord, eParameterDataType.DataRowView);

      SetCheckParameter("Hourly_Operating_Data_Records_for_LME_Configuration", new DataView(mLMEScreenProcess.SourceData.Tables["LMEHourlyOp"],
          "rpt_period_id = " + CheckEngine.RptPeriodId, "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetDataViewCheckParameter("Monitor_Load_Records_by_Hour_and_Location",
                                mLMEScreenProcess.SourceData.Tables["MonitorLoad"],
                                cFilterFormat.ActiveRowsForLocation(monLocId, opDate, opHour),
                                "");

      SetCheckParameter("Reporting_Period_Lookup_Table", new DataView(mLMEScreenProcess.SourceData.Tables["ReportingPeriod"],
          "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

    }

    protected override bool SetErrorSuppressValues()
    {
      ErrorSuppressValues = null;
      return true;
    }

    protected override void SetRecordIdentifier()
    {
      RecordIdentifier = "this record";
    }

    # endregion
  }
}
