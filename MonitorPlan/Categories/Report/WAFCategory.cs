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
  public class cWAFCategory : cCategory
  {

    #region Private Constants

    private const string Label = "WAF";

    #endregion


    #region Private Fields

    private string mRectDuctWafDataId;
    private cMonitorPlan mMonitorPlan;

    #endregion


    #region Constructors

    public cWAFCategory(cCheckEngine CheckEngine, cMonitorPlan MonitorPlan)
      : base(CheckEngine, (cProcess)MonitorPlan, "WAF")
    {
      mMonitorPlan = MonitorPlan;
      TableName = "RECT_DUCT_WAF";
    }

    #endregion


    #region Public Static Methods

    public static cWAFCategory GetInitialized(cCheckEngine ACheckEngine, cMonitorPlan AMonitorPlanProcess)
    {
      cWAFCategory Category;
      string ErrorMessage = "";

      try
      {
        Category = new cWAFCategory(ACheckEngine, AMonitorPlanProcess);

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

    public new bool ProcessChecks(string RectDuctWafDataId, string MonitorLocationID)
    {
      mRectDuctWafDataId = RectDuctWafDataId;
      CurrentRowId = mRectDuctWafDataId;

      System.Diagnostics.Debug.WriteLine(string.Format("{0}: {1}", Label, CurrentRowId));

      return base.ProcessChecks(MonitorLocationID);
    }

    #endregion


    #region Base Class Overrides

    protected override void FilterData()
    {

      //Process.ElapsedTimes.TimingBegin("FilterData", this);

      //filter RectDuctWaf to find this one based on the mRectDuctWafDataId
      SetDataRowCheckParameter("Current_WAF", mMonitorPlan.SourceData.Tables["RectDuctWaf"],
        "rect_duct_waf_data_id = '" + mRectDuctWafDataId + "'", "rect_duct_waf_data_id");

      //lookup tables
      SetDataViewCheckParameter("WAF_Method_Code_Lookup_Table", mMonitorPlan.SourceData.Tables["WafMethodCode"], "", "");


      //Process.ElapsedTimes.TimingEnd("FilterData", this);
    }

    protected override void SetRecordIdentifier()
    {
      DataRowView CurrentWAF = (DataRowView)GetCheckParameter("Current_WAF").ParameterValue;

      RecordIdentifier = "WAFBeginDate " + ((DateTime)CurrentWAF["waf_effective_date"]).ToShortDateString();
    }

    protected override bool SetErrorSuppressValues()
    {
      DataRowView currentLocation = GetCheckParameter("Current_Location").ValueAsDataRowView();
      DataRowView currentWaf = (DataRowView)GetCheckParameter("Current_WAF").ParameterValue;

      if (currentWaf != null)
      {
        long facId = CheckEngine.FacilityID;
        string locationName = currentLocation["LOCATION_IDENTIFIER"].AsString();
        DateTime? matchTimeValue = currentWaf["END_DATE"].AsDateTime();

        ErrorSuppressValues = new cErrorSuppressValues(facId, locationName, null, null, "HISTIND", matchTimeValue);
        return true;
      }
      else
        return false;
    }

    #endregion

  }

}