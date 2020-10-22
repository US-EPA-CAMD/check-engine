using System;
using System.Data;
using System.Collections;
using System.Text.RegularExpressions;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Extensions;
using ECMPS.ErrorSuppression;


namespace ECMPS.Checks.MonitorPlanEvaluation
{
  public class cUnitFuelCategory : cCategory
  {

    #region Private Constants

    private const string Label = "Unit Fuel";

    #endregion


    #region Private Fields

    private string _UnitFuelID;
    private cMonitorPlan _MonitorPlan;

    #endregion


    #region Constructors

    public cUnitFuelCategory(cCheckEngine CheckEngine, cMonitorPlan MonitorPlan)
      : base(CheckEngine, (cProcess)MonitorPlan, "FUEL")
    {
      _MonitorPlan = MonitorPlan;
      TableName = "UNIT_FUEL";
    }

    #endregion


    #region Public Static Methods

    public static cUnitFuelCategory GetInitialized(cCheckEngine ACheckEngine, cMonitorPlan AMonitorPlanProcess)
    {
      cUnitFuelCategory Category;
      string ErrorMessage = "";

      try
      {
        Category = new cUnitFuelCategory(ACheckEngine, AMonitorPlanProcess);

        bool Result = Category.InitCheckBands(ACheckEngine.DbAuxConnection, ref ErrorMessage);

        if (!Result)
        {
          Category = null;
          System.Diagnostics.Debug.WriteLine(string.Format("{0}: {1}", Label, ErrorMessage));
        }
      }
      catch (Exception ex)
      {
        Category = null;
        System.Diagnostics.Debug.WriteLine(string.Format("{0}: {1}", Label, ex.Message));
      }

      return Category;
    }

    #endregion


    #region Public Methods

    public new bool ProcessChecks(string UnitFuelID, string MonitorLocationID)
    {
      _UnitFuelID = UnitFuelID;
      CurrentRowId = _UnitFuelID;

      System.Diagnostics.Debug.WriteLine(string.Format("{0}: {1}", Label, CurrentRowId));

      return base.ProcessChecks(MonitorLocationID);
    }

    #endregion


    #region Base Class Overrides

    protected override void FilterData()
    {
      string sUnitFuelIDFilter = string.Format("UF_ID='{0}' and LOCATION_IDENTIFIER = UNITID", _UnitFuelID);
      DataRowView drvCurrentRecord = new DataView(_MonitorPlan.SourceData.Tables["LocationFuel"],
                                                   sUnitFuelIDFilter, "UF_ID", DataViewRowState.CurrentRows)[0];

      SetCheckParameter("Current_Fuel", drvCurrentRecord, eParameterDataType.DataRowView);
      SetCheckParameter("Current_Unit", drvCurrentRecord, eParameterDataType.DataRowView);

      SetDataViewCheckParameter("Fuel_Indicator_Code_Lookup_Table",
                                _MonitorPlan.SourceData.Tables["IndicatorCode"],
                                "", "");

      SetDataViewCheckParameter("Fuel_Demonstration_Method_Lookup_Table",
                                _MonitorPlan.SourceData.Tables["DemMethodCode"],
                                "", "");

    }

    protected override void SetRecordIdentifier()
    {
      DataRowView CurrentFuel = (DataRowView)GetCheckParameter("Current_Fuel").ParameterValue;

      RecordIdentifier = string.Format("Fuel {0}, Begin Date {1}", cDBConvert.ToString(CurrentFuel["fuel_cd"]), cDBConvert.ToDate(CurrentFuel["begin_date"], DateTypes.START).ToShortDateString());
    }

    protected override bool SetErrorSuppressValues()
    {
      DataRowView currentLocation = GetCheckParameter("Current_Location").ValueAsDataRowView();
      DataRowView currentFuel = GetCheckParameter("Current_Fuel").ValueAsDataRowView();

      if (currentFuel != null)
      {
        long facId = CheckEngine.FacilityID;
        string locationName = currentLocation["LOCATION_IDENTIFIER"].AsString();
        string matchDataValue = currentFuel["FUEL_CD"].AsString();
        DateTime? matchTimeValue = currentFuel["END_DATE"].AsDateTime();

        ErrorSuppressValues = new cErrorSuppressValues(facId, locationName, "FUELTYP", matchDataValue, "HISTIND", matchTimeValue);
        return true;
      }
      else
        return false;
    }

    #endregion

  }
}
