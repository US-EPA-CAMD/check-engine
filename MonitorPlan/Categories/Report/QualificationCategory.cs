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
  public class cQualificationCategory : cCategory
  {

    #region Private Constants

    private const string Label = "Qualification";

    #endregion


    #region Private Fields

    private string mMonQualId;
    private cMonitorPlan mMonitorPlan;

    #endregion


    #region Constructors

    public cQualificationCategory(cCheckEngine CheckEngine, cMonitorPlan MonitorPlan)
      : base(CheckEngine, (cProcess)MonitorPlan, "QUAL")
    {
      mMonitorPlan = MonitorPlan;
      TableName = "MONITOR_QUALIFICATION";
    }

    #endregion


    #region Public Static Methods

    public static cQualificationCategory GetInitialized(cCheckEngine ACheckEngine, cMonitorPlan AMonitorPlanProcess)
    {
      cQualificationCategory Category;
      string ErrorMessage = "";

      try
      {
        Category = new cQualificationCategory(ACheckEngine, AMonitorPlanProcess);

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

    public new bool ProcessChecks(string MonQualId, string MonitorLocationID)
    {
      mMonQualId = MonQualId;
      CurrentRowId = mMonQualId;

      System.Diagnostics.Debug.WriteLine(string.Format("{0}: {1}", Label, CurrentRowId));

      return base.ProcessChecks(MonitorLocationID);
    }

    #endregion


    #region Base Class Overrides

    protected override void FilterData()
    {

      //Process.ElapsedTimes.TimingBegin("FilterData", this);

      //filter MonitorQualification to find this one based on the mMonQualId
      SetDataRowCheckParameter("Current_Qualification", mMonitorPlan.SourceData.Tables["MonitorQualification"],
        "mon_qual_id = '" + mMonQualId + "'", "mon_qual_id");

      //Set Qualification_Percent_Records
      SetDataViewCheckParameter("Qualification_Percent_Records", mMonitorPlan.SourceData.Tables["MonitorQualificationPct"],
                                  "mon_loc_id = '" + CurrentMonLocId + "'", "");

      //Set QualificationLME_Records
      SetDataViewCheckParameter("QualificationLME_Records", mMonitorPlan.SourceData.Tables["MonitorQualificationLME"],
                                  "mon_loc_id = '" + CurrentMonLocId + "'", "");

      //Set QualificationLME_Records
      SetDataViewCheckParameter("QualificationLEE_Records", mMonitorPlan.SourceData.Tables["MonitorQualificationLEE"],
                                  "mon_loc_id = '" + CurrentMonLocId + "'", "");

      //lookup tables
      SetDataViewCheckParameter("Qualification_Type_Code_Lookup_Table", mMonitorPlan.SourceData.Tables["QualTypeCode"], "", "");


      //Process.ElapsedTimes.TimingEnd("FilterData", this);
    }

    protected override void SetRecordIdentifier()
    {
      DataRowView CurrentQual = (DataRowView)GetCheckParameter("Current_Qualification").ParameterValue;

      RecordIdentifier = "QualificationTypeCode " + (string)CurrentQual["qual_type_cd"] +
                          " BeginDate " + ((DateTime)CurrentQual["begin_date"]).ToShortDateString();
    }

    protected override bool SetErrorSuppressValues()
    {
      DataRowView currentLocation = GetCheckParameter("Current_Location").ValueAsDataRowView();
      DataRowView currentQualification = GetCheckParameter("Current_Qualification").ValueAsDataRowView();

      if (currentQualification != null)
      {
        long facId = CheckEngine.FacilityID;
        string locationName = currentLocation["LOCATION_IDENTIFIER"].AsString();
        string matchDataValue = currentQualification["QUAL_TYPE_CD"].AsString();
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
