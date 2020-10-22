using System;
using System.Data;
using System.Collections;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Extensions;
using ECMPS.ErrorSuppression;


namespace ECMPS.Checks.MonitorPlanEvaluation
{
  public class cMonitorLocationAttribute : cCategory
  {

    #region Private Constants

    private const string Label = "Monitor Location Attribute";

    #endregion


    #region Private Fields

    private string mMonitorLocationAttributeID;
    private cMonitorPlan mMonitorPlan;

    #endregion


    #region Constructors

    public cMonitorLocationAttribute(cCheckEngine CheckEngine, cMonitorPlan MonitorPlan)
      : base(CheckEngine, (cProcess)MonitorPlan, "LOCCHAR")
    {
      mMonitorPlan = MonitorPlan;
      TableName = "MONITOR_LOCATION_ATTRIBUTE";
    }

    #endregion


    #region Public Static Methods

    public static cMonitorLocationAttribute GetInitialized(cCheckEngine ACheckEngine, cMonitorPlan AMonitorPlanProcess)
    {
      cMonitorLocationAttribute Category;
      string ErrorMessage = "";

      try
      {
        Category = new cMonitorLocationAttribute(ACheckEngine, AMonitorPlanProcess);

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

    public new bool ProcessChecks(string MonitorLocationAttributeID, string MonitorLocationID)
    {
      mMonitorLocationAttributeID = MonitorLocationAttributeID;

      CurrentRowId = mMonitorLocationAttributeID;

      System.Diagnostics.Debug.WriteLine(string.Format("{0}: {1}", Label, CurrentRowId));

      return base.ProcessChecks(MonitorLocationID);
    }

    #endregion


    #region Base Class Overrides

    protected override void FilterData()
    {

      //filter MonitorPlanLocation to find this one based on the mMonitorLocationAttributeID
      SetCheckParameter("Current_Location_Attribute", new DataView(mMonitorPlan.SourceData.Tables["LocationAttribute"],
                "mon_loc_attrib_id = '" + mMonitorLocationAttributeID + "'", "mon_loc_attrib_id", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      //get MaterialCode
      SetCheckParameter("MATERIAL_CODE_lookup_table", new DataView(mMonitorPlan.SourceData.Tables["MaterialCode"],
          "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      //get ShapeCode
      SetCheckParameter("SHAPE_CODE_lookup_table", new DataView(mMonitorPlan.SourceData.Tables["ShapeCode"],
          "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);
    }


    protected override void SetRecordIdentifier()
    {
      DataRowView CurrentLocation = (DataRowView)GetCheckParameter("Current_Location").ParameterValue;

      if (CurrentLocation["stack_pipe_id"] == DBNull.Value)
      {
        RecordIdentifier = "Unit ID " + (string)CurrentLocation["unitid"];
      }
      else
      {
        RecordIdentifier = "Stack/Pipe ID " + (string)CurrentLocation["stack_name"];
      }
    }

    protected override bool SetErrorSuppressValues()
    {
      DataRowView currentLocation = (DataRowView)GetCheckParameter("Current_Location").ParameterValue;
      DataRowView currentLocationAttribute = (DataRowView)GetCheckParameter("Current_Location_Attribute").ParameterValue;

      if ((currentLocation != null) && (currentLocationAttribute != null))
      {
        long facId = CheckEngine.FacilityID;
        string locationName = currentLocation["LOCATION_IDENTIFIER"].AsString();
        DateTime? matchTimeValue = currentLocationAttribute["END_DATE"].AsDateTime();

        ErrorSuppressValues = new cErrorSuppressValues(facId, locationName, null, null, "HISTIND", matchTimeValue);
        return true;
      }
      else
        return false;
    }

    #endregion

  }
}