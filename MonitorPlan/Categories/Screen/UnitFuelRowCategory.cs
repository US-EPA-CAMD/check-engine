using System;
using System.Text;
using System.Data;
using System.Collections.Generic;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;

using ECMPS.ErrorSuppression;


namespace ECMPS.Checks.MPScreenEvaluation
{
  public class cUnitFuelRowCategory : cCategory
  {
    #region Private Fields

    private DataTable _UnitFuelTable;
    private cMPScreenMain _MonitorPlan;

    #endregion

    #region Constructors

    public cUnitFuelRowCategory(cCheckEngine CheckEngine, cMPScreenMain MonitorPlan, string MonitorLocationID, DataTable UnitFuelTable)
      : base(CheckEngine, (cProcess)MonitorPlan, "SCRFUEL")
    {
      InitializeCurrent(MonitorLocationID);

      _UnitFuelTable = UnitFuelTable;
      _MonitorPlan = MonitorPlan;

      FilterData();

      SetRecordIdentifier();
    }

    public cUnitFuelRowCategory(cCheckEngine ACheckEngine, cMPScreenMain AMpScreenProcess)
        : base(ACheckEngine, (cProcess)AMpScreenProcess, "SCRFUEL")
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
          
          DataRowView drvCurrentRecord = new DataView( _UnitFuelTable, "", "", DataViewRowState.CurrentRows )[0];
          SetCheckParameter( "Current_Fuel", drvCurrentRecord, eParameterDataType.DataRowView );
          SetCheckParameter("Current_Unit", new DataView(_MonitorPlan.SourceData.Tables["MPLocation"],
              "", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

          SetDataViewCheckParameter( "Location_Fuel_Records", _MonitorPlan.SourceData.Tables["MPUnitFuel"], "", "" );
          SetDataViewCheckParameter( "Fuel_Demonstration_Method_Lookup_Table", _MonitorPlan.SourceData.Tables["DemMethodCode"], "", "" );
          SetDataViewCheckParameter( "Fuel_Indicator_Code_Lookup_Table", _MonitorPlan.SourceData.Tables["IndicatorCode"], "", "" );
          SetDataViewCheckParameter( "Fuel_Code_Lookup_Table", _MonitorPlan.SourceData.Tables["FuelCode"], "", "" );
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
