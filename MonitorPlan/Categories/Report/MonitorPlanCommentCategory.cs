using System;
using System.Collections.Generic;
using System.Data;
using System.Text;


using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;

using ECMPS.Definitions.Extensions;
using ECMPS.ErrorSuppression;


namespace ECMPS.Checks.MonitorPlanEvaluation
{
  public class cMonitorPlanCommentCategory : cCategory
  {

    #region Private Constants

    private const string Label = "Monitor Plan Comment";

    #endregion


    #region Constructors

    public cMonitorPlanCommentCategory(cCheckEngine ACheckEngine, cMonitorPlan AMonitorPlanProcess)
      : base(ACheckEngine, (cProcess)AMonitorPlanProcess, "MPCOMM")
    {
      FMonitorPlanProcess = AMonitorPlanProcess;

      TableName = "MONITOR_PLAN_COMMENT";
    }

    #endregion


    #region Public Static Methods

    public static cMonitorPlanCommentCategory GetInitialized(cCheckEngine ACheckEngine, cMonitorPlan AMonitorPlanProcess)
    {
      cMonitorPlanCommentCategory Category;
      string ErrorMessage = "";

      try
      {
        Category = new cMonitorPlanCommentCategory(ACheckEngine, AMonitorPlanProcess);

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


    #region Private Fields

    private string FMonitorPlanCommentId;
    private cMonitorPlan FMonitorPlanProcess;

    #endregion


    #region Public Methods

    public new bool ProcessChecks(string AMonitorPlanCommentId)
    {
      FMonitorPlanCommentId = AMonitorPlanCommentId;
      CurrentRowId = FMonitorPlanCommentId;

      System.Diagnostics.Debug.WriteLine(string.Format("{0}: {1}", Label, CurrentRowId));

      return base.ProcessChecks();
    }

    #endregion


    #region Base Class Overrides

    protected override void FilterData()
    {
      //Set Current_Monitoring_Plan_Comment
      SetDataRowCheckParameter("Current_Monitoring_Plan_Comment",
                               FMonitorPlanProcess.SourceData.Tables["MpMonitorPlanComment"],
                               "Mon_Plan_Comment_Id = '" + FMonitorPlanCommentId + "'", "");


      string MonPlanFilter = string.Format("MON_PLAN_ID = '{0}'", CheckEngine.MonPlanId);

      //Set Monitoring_Plan_Comment_Records
      SetDataViewCheckParameter("Monitoring_Plan_Comment_Records",
                                FMonitorPlanProcess.SourceData.Tables["MpMonitorPlanComment"],
                                MonPlanFilter, "");

    }

    protected override void SetRecordIdentifier()
    {
      DataRowView CurrentMonitoringPlanComment = (DataRowView)GetCheckParameter("Current_Monitoring_Plan_Comment").ParameterValue;

      RecordIdentifier = "the comment beginning on " + Convert.ToDateTime(CurrentMonitoringPlanComment["Begin_Date"]).ToShortDateString();
    }

    protected override bool SetErrorSuppressValues()
    {
      DataRowView currentMonitorPlanComment = GetCheckParameter("Current_Monitoring_Plan_Comment").AsDataRowView();

      if (currentMonitorPlanComment != null)
      {
        long facId = CheckEngine.FacilityID;
        string matchDataValue = currentMonitorPlanComment["MON_PLAN_ID"].AsString();
        DateTime? matchTimeValue = currentMonitorPlanComment["END_DATE"].AsDateTime();

        ErrorSuppressValues = new cErrorSuppressValues(facId, null, "MONPLAN", matchDataValue, "HISTIND", matchTimeValue);
        return true;
      }
      else
        return false;
    }

    #endregion

  }
}
