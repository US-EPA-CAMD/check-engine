using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;

using ECMPS.ErrorSuppression;

namespace ECMPS.Checks.MPScreenEvaluation
{
  public class cMonitorPlanCommentRowCategory : cCategory
  {

    #region Constructors

    public cMonitorPlanCommentRowCategory(cCheckEngine ACheckEngine, cMPScreenMain AMpScreenProcess, DataTable AMonitorPlanCommentTable)
      : base(ACheckEngine, (cProcess)AMpScreenProcess, "SCRMPCM", AMonitorPlanCommentTable)
    {
      FMpScreenProcess = AMpScreenProcess;
      FMonitorPlanCommentTable = AMonitorPlanCommentTable;

      FilterData();

      SetRecordIdentifier();
    }

    public cMonitorPlanCommentRowCategory(cCheckEngine ACheckEngine, cMPScreenMain AMpScreenProcess)
      : base(ACheckEngine, (cProcess)AMpScreenProcess, "SCRMPCM")
    {

    }

    #endregion

    #region Private Fields

    private cMPScreenMain FMpScreenProcess;
    private DataTable FMonitorPlanCommentTable;

    #endregion	

    #region Public Methods

    /// <summary>
    /// Sets the checkbands for this category to the passed check bands and then executes
    /// those checks.
    /// </summary>
    /// <param name="ACheckBands">The check bands to process.</param>
    /// <returns>True if the processing of check executed normally.</returns>
    public bool ProcessChecks(cCheckParameterBands ACheckBands)
    {
      this.SetCheckBands(ACheckBands);
      return base.ProcessChecks();
    }

    #endregion
	
    #region Base Class Overrides

    protected override void FilterData()
    {
      //Set Current_Monitoring_Plan_Comment
      SetDataRowCheckParameter("Current_Monitoring_Plan_Comment", FMonitorPlanCommentTable, "", "");


      string MonPlanFilter = string.Format("MON_PLAN_ID = '{0}'", FMonitorPlanCommentTable.Rows[0]["Mon_Plan_Id"]);

      //Set Monitoring_Plan_Comment_Records
      SetDataViewCheckParameter("Monitoring_Plan_Comment_Records",
                                FMpScreenProcess.SourceData.Tables["MpMonitorPlanComment"],
                                MonPlanFilter, "");
    }


    protected override void SetRecordIdentifier()
    {
      RecordIdentifier = "this record";
    }

    protected override bool SetErrorSuppressValues()
    {
        ErrorSuppressValues = null;
        return true;
    }

    #endregion

  }
}
