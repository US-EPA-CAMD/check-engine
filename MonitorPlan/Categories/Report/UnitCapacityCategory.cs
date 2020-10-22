using System;
using System.Text;
using System.Data;
using System.Collections.Generic;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Extensions;
using ECMPS.ErrorSuppression;


namespace ECMPS.Checks.MonitorPlanEvaluation
{
  public class cUnitCapacityCategory : cCategory
  {

    #region Private Constants

    private const string Label = "Unit Capacity";

    #endregion


    #region Private Fields

    private string _UnitCapID;
    private cMonitorPlan _MonitorPlan;

    #endregion


    #region Constructors

    public cUnitCapacityCategory(cCheckEngine CheckEngine, cMonitorPlan MonitorPlan)
      : base(CheckEngine, (cProcess)MonitorPlan, "CAPAC")
    {
      _MonitorPlan = MonitorPlan;
      TableName = "UNIT_CAPACITY";
    }


    #endregion


    #region Public Static Methods

    public static cUnitCapacityCategory GetInitialized(cCheckEngine ACheckEngine, cMonitorPlan AMonitorPlanProcess)
    {
      cUnitCapacityCategory Category;
      string ErrorMessage = "";

      try
      {
        Category = new cUnitCapacityCategory(ACheckEngine, AMonitorPlanProcess);

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

    public new bool ProcessChecks(string UnitCapID, string MonitorLocationID)
    {
      _UnitCapID = UnitCapID;
      CurrentRowId = _UnitCapID;

      System.Diagnostics.Debug.WriteLine(string.Format("{0}: {1}", Label, CurrentRowId));

      return base.ProcessChecks(MonitorLocationID);
    }

    #endregion


    #region Base Class Overrides

    protected override void FilterData()
    {
      string sUnitCapIDFilter = string.Format("UNIT_CAP_ID='{0}' and LOCATION_IDENTIFIER = UNITID", _UnitCapID);
      DataRowView drvCurrentRecord = new DataView(_MonitorPlan.SourceData.Tables["LocationCapacity"],
                                                   sUnitCapIDFilter, "UNIT_CAP_ID", DataViewRowState.CurrentRows)[0];

      SetCheckParameter("Current_Unit_Capacity", drvCurrentRecord, eParameterDataType.DataRowView);
      SetCheckParameter("Current_Unit", drvCurrentRecord, eParameterDataType.DataRowView);
    }

    protected override void SetRecordIdentifier()
    {
      DataRowView CurrentRecord = (DataRowView)GetCheckParameter("Current_Unit_Capacity").ParameterValue;

      string sBeginDate = "";
      if (CurrentRecord["BEGIN_DATE"] != DBNull.Value)
        sBeginDate = " BeginDate " + ((DateTime)CurrentRecord["BEGIN_DATE"]).ToShortDateString();

      RecordIdentifier = string.Format("MaximumHourlyHeatInputCapacity {0}{1}", CurrentRecord["MAX_HI_CAPACITY"].ToString(), sBeginDate);
    }

    protected override bool SetErrorSuppressValues()
    {
      DataRowView currentLocation = GetCheckParameter("Current_Location").ValueAsDataRowView();
      DataRowView currentUnitCapacity = GetCheckParameter("Current_Unit_Capacity").ValueAsDataRowView();

      if (currentUnitCapacity != null)
      {
        long facId = CheckEngine.FacilityID;
        string locationName = currentLocation["LOCATION_IDENTIFIER"].AsString();
        DateTime? matchTimeValue = currentUnitCapacity["END_DATE"].AsDateTime();

        ErrorSuppressValues = new cErrorSuppressValues(facId, locationName, null, null, "HISTIND", matchTimeValue);
        return true;
      }
      else
        return false;
    }

    #endregion

  }
}
