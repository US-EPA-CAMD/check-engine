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
  public class cQualificationLEECategory : cCategory
  {

    #region Constructors

    public cQualificationLEECategory(cMonitorPlan monitorPlan)
      : base(monitorPlan.CheckEngine, monitorPlan, "QUALLEE")
    {
      mMonitorPlan = monitorPlan;
      TableName = "MONITOR_QUALIFICATION_LEE";
    }

    #endregion


    #region Private Constants

    private const string Label = "Qualification LEE";

    #endregion


    #region Private Fields

    private string mMonLeeID;
    private cMonitorPlan mMonitorPlan;

    #endregion


    #region Public Static Methods

    public static cQualificationLEECategory GetInitialized(cCheckEngine ACheckEngine, cMonitorPlan AMonitorPlanProcess)
    {
      cQualificationLEECategory Category;
      string ErrorMessage = "";

      try
      {
        Category = new cQualificationLEECategory(AMonitorPlanProcess);

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

    public new bool ProcessChecks(string MonLeeId, string MonitorLocationID)
    {
		mMonLeeID = MonLeeId;
		CurrentRowId = mMonLeeID;

      System.Diagnostics.Debug.WriteLine(string.Format("{0}: {1}", Label, CurrentRowId));

      return base.ProcessChecks(MonitorLocationID);
    }

    #endregion


    #region Base Class Overrides

    protected override void FilterData()
    {
      //filter MonitorQualificationLEE to find this one based on the mMonLeeID
      SetDataRowCheckParameter("Current_Qualification_LEE", mMonitorPlan.SourceData.Tables["MonitorQualificationLEE"],
		"mon_LEE_id = '" + mMonLeeID + "'", "mon_LEE_id");

	  //lookup tables
	  SetDataViewCheckParameter("Qualification_LEE_Test_Type_Code_Lookup_Table", mMonitorPlan.SourceData.Tables["QualLEETestTypeCode"], "", "");
    }

    protected override void SetRecordIdentifier()
    {
      DataRowView CurrentQualLEE = (DataRowView)GetCheckParameter("Current_Qualification_LEE").ParameterValue;


      RecordIdentifier = "QualificationTypeCode " + CurrentQualLEE["qual_type_cd"].ToString() +
                              " QualificationTestDate " + CurrentQualLEE["qual_test_date"].ToString();

    }

    protected override bool SetErrorSuppressValues()
    {
      DataRowView currentLocation = GetCheckParameter("Current_Location").ValueAsDataRowView();
      DataRowView currentQualification = GetCheckParameter("Current_Qualification").ValueAsDataRowView();
      DataRowView currentQualificationLEE = GetCheckParameter("Current_Qualification_LEE").ValueAsDataRowView();

      if (currentQualificationLEE != null)
      {
        long facId = CheckEngine.FacilityID;
        string locationName = currentLocation["LOCATION_IDENTIFIER"].AsString();
        string matchDataValue = currentQualificationLEE["QUAL_TYPE_CD"].AsString();
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
