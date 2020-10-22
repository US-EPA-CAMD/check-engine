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
  public class cQualificationPercentCategory : cCategory
  {

    #region Private Constants

    private const string Label = "Qualification Percent";

    #endregion


    #region Private Fields

    private string mMonPctID;
    private cMonitorPlan mMonitorPlan;

    #endregion


    #region Constructors

    public cQualificationPercentCategory(cCheckEngine CheckEngine, cMonitorPlan MonitorPlan)
      : base(CheckEngine, (cProcess)MonitorPlan, "QUALPCT")
    {
      mMonitorPlan = MonitorPlan;
      TableName = "MONITOR_QUALIFICATION_PCT";
    }

    #endregion


    #region Public Static Methods

    public static cQualificationPercentCategory GetInitialized(cCheckEngine ACheckEngine, cMonitorPlan AMonitorPlanProcess)
    {
      cQualificationPercentCategory Category;
      string ErrorMessage = "";

      try
      {
        Category = new cQualificationPercentCategory(ACheckEngine, AMonitorPlanProcess);

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

    public new bool ProcessChecks(string MonPctId, string MonitorLocationID)
    {
      mMonPctID = MonPctId;
      CurrentRowId = mMonPctID;

      System.Diagnostics.Debug.WriteLine(string.Format("{0}: {1}", Label, CurrentRowId));

      return base.ProcessChecks(MonitorLocationID);
    }

    #endregion


    #region Base Class Overrides

    protected override void FilterData()
    {

      //Process.ElapsedTimes.TimingBegin("FilterData", this);

      //filter MonitorQualificationPercent to find this one based on the mMonPctID
      SetDataRowCheckParameter("Current_Qualification_Percent", mMonitorPlan.SourceData.Tables["MonitorQualificationPct"],
        "mon_pct_id = '" + mMonPctID + "'", "mon_pct_id");

      //Process.ElapsedTimes.TimingEnd("FilterData", this);
    }

    protected override void SetRecordIdentifier()
    {
      DataRowView CurrentQualPct = (DataRowView)GetCheckParameter("Current_Qualification_Percent").ParameterValue;


      RecordIdentifier = "QualificationTypeCode " + CurrentQualPct["qual_type_cd"].ToString() +
                              " QualificationYear " + CurrentQualPct["qual_year"].ToString();

    }

    protected override bool SetErrorSuppressValues()
    {
      DataRowView currentLocation = GetCheckParameter("Current_Location").ValueAsDataRowView();
      DataRowView currentQualification = GetCheckParameter("Current_Qualification").ValueAsDataRowView();
      DataRowView currentQualificationPct = GetCheckParameter("Current_Qualification_Percent").ValueAsDataRowView();
      
      if (currentQualificationPct != null)
      {
        long facId = CheckEngine.FacilityID;
        string locationName = currentLocation["LOCATION_IDENTIFIER"].AsString();
        string matchDataValue = currentQualificationPct["QUAL_TYPE_CD"].AsString();
        DateTime? matchTimeValue = currentQualification["END_DATE"].AsDateTime();

        ErrorSuppressValues = new cErrorSuppressValues(facId, locationName, "QUALTYP", matchDataValue, "HISTIND", matchTimeValue);
        return true;
      }
      else
        return false;
    }


    #endregion

  }
}
