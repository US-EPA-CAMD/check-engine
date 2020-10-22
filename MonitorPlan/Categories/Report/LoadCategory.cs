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
  public class cLoadCategory : cCategory
  {

    #region Private Constants

    private const string Label = "Load";

    #endregion


    #region Private Fields

    private string mLoadID;
    private cMonitorPlan mMonitorPlan;

    #endregion


    #region Constructors

    public cLoadCategory(cCheckEngine CheckEngine, cMonitorPlan MonitorPlan)
      : base(CheckEngine, (cProcess)MonitorPlan, "LOAD")
    {
      mMonitorPlan = MonitorPlan;
      TableName = "MONITOR_LOAD";
    }

    #endregion


    #region Public Static Methods

    public static cLoadCategory GetInitialized(cCheckEngine ACheckEngine, cMonitorPlan AMonitorPlanProcess)
    {
      cLoadCategory Category;
      string ErrorMessage = "";

      try
      {
        Category = new cLoadCategory(ACheckEngine, AMonitorPlanProcess);

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

    public new bool ProcessChecks(string LoadId, string MonitorLocationID)
    {
      mLoadID = LoadId;
      CurrentRowId = mLoadID;

      System.Diagnostics.Debug.WriteLine(string.Format("{0}: {1}", Label, CurrentRowId));

      return base.ProcessChecks(MonitorLocationID);
    }

    #endregion


    #region Base Class Overrides

    protected override void FilterData()
    {

      //Process.ElapsedTimes.TimingBegin("FilterData", this);

      //filter MonitorPlanLoad to find this one based on the mLoadID
      SetDataRowCheckParameter("Current_Load", mMonitorPlan.SourceData.Tables["MonitorLoad"],
        "load_id = '" + mLoadID + "'", "load_id");

      //Set QA_Supplemental_Data_Records
      SetDataViewCheckParameter("QA_Supplemental_Data_Records", mMonitorPlan.SourceData.Tables["QASuppData"], "", "");

      //Set Facility_Load_Records
      SetDataViewCheckParameter("Facility_Load_Records", mMonitorPlan.SourceData.Tables["MonitorLoad"], "", "");


      //Process.ElapsedTimes.TimingEnd("FilterData", this);
    }

    protected override void SetRecordIdentifier()
    {
      DataRowView CurrentLoad = (DataRowView)GetCheckParameter("Current_Load").ParameterValue;

      if (CurrentLoad["BEGIN_DATE"] == DBNull.Value)
        RecordIdentifier = "No BeginDate";
      else
        RecordIdentifier = "BeginDate " + ((DateTime)CurrentLoad["BEGIN_DATE"]).ToShortDateString();

    }

    protected override bool SetErrorSuppressValues()
    {
      DataRowView currentLocation = GetCheckParameter("Current_Location").ValueAsDataRowView();
      DataRowView currentLoad = GetCheckParameter("Current_Load").ValueAsDataRowView();

      if (currentLoad != null)
      {
        long facId = CheckEngine.FacilityID;
        string locationName = currentLocation["LOCATION_IDENTIFIER"].AsString();
        DateTime? matchTimeValue = currentLoad["END_DATE"].AsDateTime();

        ErrorSuppressValues = new cErrorSuppressValues(facId, locationName, null, null, "HISTIND", matchTimeValue);
        return true;
      }
      else
        return false;
    }


    #endregion

  }
}
