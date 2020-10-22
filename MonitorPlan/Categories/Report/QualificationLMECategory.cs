using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Extensions;
using ECMPS.ErrorSuppression;


namespace ECMPS.Checks.MonitorPlanEvaluation
{
  public class cQualificationLMECategory : cCategory
  {

    #region Private Constants

    private const string Label = "Qualification LME";

    #endregion


    #region Private Fields

    private string mMonLmeID;
    private cMonitorPlan mMonitorPlan;

    #endregion


    #region Constructors

    public cQualificationLMECategory(cCheckEngine CheckEngine, cMonitorPlan MonitorPlan)
      : base(CheckEngine, (cProcess)MonitorPlan, "QUALLME")
    {
      mMonitorPlan = MonitorPlan;
      TableName = "MONITOR_QUALIFICATION_LME";
    }

    #endregion


    #region Public Static Methods

    public static cQualificationLMECategory GetInitialized(cCheckEngine ACheckEngine, cMonitorPlan AMonitorPlanProcess)
    {
      cQualificationLMECategory Category;
      string ErrorMessage = "";

      try
      {
        Category = new cQualificationLMECategory(ACheckEngine, AMonitorPlanProcess);

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

    public new bool ProcessChecks(string MonLmeId, string MonitorLocationID)
    {
      mMonLmeID = MonLmeId;
      CurrentRowId = mMonLmeID;

      System.Diagnostics.Debug.WriteLine(string.Format("{0}: {1}", Label, CurrentRowId));

      return base.ProcessChecks(MonitorLocationID);
    }

    #endregion


    #region Base Class Overrides

    protected override void FilterData()
    {

      //Process.ElapsedTimes.TimingBegin("FilterData", this);

      //filter MonitorQualificationPercent to find this one based on the mMonPctID
      SetDataRowCheckParameter("Current_Qualification_LME", mMonitorPlan.SourceData.Tables["MonitorQualificationLME"],
        "mon_lme_id = '" + mMonLmeID + "'", "mon_lme_id");

      //Process.ElapsedTimes.TimingEnd("FilterData", this);
    }

    protected override void SetRecordIdentifier()
    {
      DataRowView CurrentQualLME = (DataRowView)GetCheckParameter("Current_Qualification_LME").ParameterValue;


      RecordIdentifier = "QualificationTypeCode " + CurrentQualLME["qual_type_cd"].ToString() +
                              " QualificationDataYear " + CurrentQualLME["qual_data_year"].ToString();

    }

    protected override bool SetErrorSuppressValues()
    {
      DataRowView currentLocation = GetCheckParameter("Current_Location").ValueAsDataRowView();
      DataRowView currentQualification = GetCheckParameter("Current_Qualification").ValueAsDataRowView();
      DataRowView currentQualificationLme = GetCheckParameter("Current_Qualification_LME").ValueAsDataRowView();

      if (currentQualificationLme != null)
      {
        long facId = CheckEngine.FacilityID;
        string locationName = currentLocation["LOCATION_IDENTIFIER"].AsString();
        string matchDataValue = currentQualificationLme["QUAL_TYPE_CD"].AsString();
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
