using System;
using System.Text;
using System.Data;
using System.Collections.Generic;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;

using ECMPS.ErrorSuppression;

namespace ECMPS.Checks.MPScreenEvaluation
{
  public class cUnitStackConfigurationRowCategory : cCategory
  {
    #region Private Fields

    private DataTable mUSCTable;
    private cMPScreenMain _MonitorPlan;

    #endregion

    #region Constructors

    public cUnitStackConfigurationRowCategory(cCheckEngine CheckEngine, cMPScreenMain MonitorPlan, string MonitorLocationID, DataTable USCTable)
      : base(CheckEngine, (cProcess)MonitorPlan, "SCRUSCF")
    {
      InitializeCurrent(MonitorLocationID);

      mUSCTable = USCTable;
      _MonitorPlan = MonitorPlan;

      FilterData();

      SetRecordIdentifier();
    }

      public cUnitStackConfigurationRowCategory(cCheckEngine ACheckEngine, cMPScreenMain AMpScreenProcess)
          : base(ACheckEngine, (cProcess)AMpScreenProcess, "SCRUSCF")
    {

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
      DataRowView drvCurrentRecord = new DataView(mUSCTable, "", "", DataViewRowState.CurrentRows)[0];
      SetCheckParameter("Current_Unit_Stack_Configuration", drvCurrentRecord, eParameterDataType.DataRowView);

      SetCheckParameter("Unit_Stack_Configuration_Records", new DataView(_MonitorPlan.SourceData.Tables["MPUnitStackConfiguration"],
        "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      //SetCheckParameter("Current_Location", new DataView(_MonitorPlan.SourceData.Tables["MPLocation"],
      //  "", "", DataViewRowState.CurrentRows)[0], ParameterTypes.DATAROW);
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
