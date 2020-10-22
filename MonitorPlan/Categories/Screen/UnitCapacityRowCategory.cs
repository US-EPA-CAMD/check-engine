using System;
using System.Text;
using System.Data;
using System.Collections.Generic;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;

using ECMPS.ErrorSuppression;

namespace ECMPS.Checks.MPScreenEvaluation
{
  class cUnitCapacityRowCategory : cCategory
  {
    #region Private Fields

    private DataTable _UnitCapTable;
    private cMPScreenMain _MonitorPlan;

    #endregion

    #region Constructors

    public cUnitCapacityRowCategory(cCheckEngine CheckEngine, cMPScreenMain MonitorPlan, string MonitorLocationID, DataTable UnitCapTable)
      : base(CheckEngine, (cProcess)MonitorPlan, "SCRUCAP")
    {
      InitializeCurrent(MonitorLocationID);

      _UnitCapTable = UnitCapTable;
      _MonitorPlan = MonitorPlan;

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
      DataRowView drvCurrentRecord = new DataView(_UnitCapTable, "", "", DataViewRowState.CurrentRows)[0];
      SetCheckParameter("Current_Unit_Capacity", drvCurrentRecord, eParameterDataType.DataRowView);
      SetCheckParameter("Current_Unit", new DataView(_MonitorPlan.SourceData.Tables["MPLocation"],
          "", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      string sUnitID = string.Format("UNIT_ID={0}", drvCurrentRecord["UNIT_ID"].ToString());
      DataView dvUnitCap = new DataView(_MonitorPlan.SourceData.Tables["MPUnitCapacity"], sUnitID, "", DataViewRowState.CurrentRows);
      SetCheckParameter("Unit_Capacity_Records", dvUnitCap, eParameterDataType.DataView);
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
