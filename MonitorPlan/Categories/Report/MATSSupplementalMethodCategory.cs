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
	public class cMATSSupplementalMethodCategory : cCategory
	{

    #region Constructors

    public cMATSSupplementalMethodCategory(cMonitorPlan monitorPlan)
      : base(monitorPlan.CheckEngine, monitorPlan, "MATSMTH")
    {
      mMonitorPlan = monitorPlan;
      TableName = "MATS_METHOD_DATA";
    }

    #endregion


    #region Private Constants

    private const string Label = "MATS Supplemental Method";

    #endregion


    #region Private Fields

    private string mMATSMethodID;
    private cMonitorPlan mMonitorPlan;

    #endregion


    #region Public Static Methods

    public static cMATSSupplementalMethodCategory GetInitialized(cCheckEngine ACheckEngine, cMonitorPlan AMonitorPlanProcess)
    {
	  cMATSSupplementalMethodCategory Category;
      string ErrorMessage = "";

      try
      {
        Category = new cMATSSupplementalMethodCategory(AMonitorPlanProcess);

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

    public new bool ProcessChecks(string MATSMethodID, string MonitorLocationID)
    {
		mMATSMethodID = MATSMethodID;
		CurrentRowId = mMATSMethodID;

      System.Diagnostics.Debug.WriteLine(string.Format("{0}: {1}", Label, CurrentRowId));

      return base.ProcessChecks(MonitorLocationID);
    }

    #endregion


    #region Base Class Overrides

    protected override void FilterData()
    {
      //filter MATSMethodData to find this one based on the MATS_Method_ID
      SetDataRowCheckParameter("MATS_Supplemental_Compliance_Method_Record", mMonitorPlan.SourceData.Tables["MATSMethodData"],
		"MATS_Method_ID = '" + mMATSMethodID + "'", "MATS_Method_ID");
    }

    protected override void SetRecordIdentifier()
    {
      DataRowView CurrentLocation = (DataRowView)GetCheckParameter("Current_Location").ParameterValue;
      DataRowView matsSupplementalComplianceMethodRecord = (DataRowView)GetCheckParameter("MATS_Supplemental_Compliance_Method_Record").ParameterValue;

      if (matsSupplementalComplianceMethodRecord["BEGIN_DATE"] == DBNull.Value)
        RecordIdentifier = "MATSMethodParameterCode " + cDBConvert.ToString(matsSupplementalComplianceMethodRecord["MATS_Method_Parameter_Cd"]) + " MATSMethodCode " + cDBConvert.ToString(matsSupplementalComplianceMethodRecord["MATS_Method_Cd"]) + " No BeginDate";
      else
        RecordIdentifier = "MATSMethodParameterCode " + cDBConvert.ToString(matsSupplementalComplianceMethodRecord["MATS_Method_Parameter_Cd"]) + " MATSMethodCode " + cDBConvert.ToString(matsSupplementalComplianceMethodRecord["MATS_Method_Cd"]) + " BeginDate " + ((DateTime)matsSupplementalComplianceMethodRecord["BEGIN_DATE"]).ToShortDateString();
    }

    protected override bool SetErrorSuppressValues()
    {
      DataRowView currentLocation = (DataRowView)GetCheckParameter("Current_Location").ParameterValue;
      DataRowView matsSupplementalComplianceMethodRecord = (DataRowView)GetCheckParameter("MATS_Supplemental_Compliance_Method_Record").ParameterValue;

      if (currentLocation != null) // && currentMethod != null)
      {
        long facId = CheckEngine.FacilityID;
        string locationName = currentLocation["LOCATION_IDENTIFIER"].AsString();
        string matchDataValue = matsSupplementalComplianceMethodRecord["MATS_METHOD_PARAMETER_CD"].AsString();
        DateTime? matchTimeValue = matsSupplementalComplianceMethodRecord["END_DATE"].AsDateTime();

        ErrorSuppressValues = new cErrorSuppressValues(facId, locationName, "MATSPAR", matchDataValue, "HISTIND", matchTimeValue);
        return true;
      }
      else
	   return false;
    }

    #endregion

  }
}
