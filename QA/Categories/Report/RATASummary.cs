using System;
using System.Data;
using System.Collections;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;
using ECMPS.ErrorSuppression;


namespace ECMPS.Checks.QAEvaluation
{
  public class cRATASummary : cQaTestReportCategory
  {

    #region Private Fields

    private string mMonitorLocationID;
    private cQAMain mQA;
    private string mRATASumID;
    //private cRATA mRATA;

    //private long mUnitID;
    //private string mStackPipeID;

    #endregion


    #region Constructors

    public cRATASummary(cCheckEngine CheckEngine, cQAMain QA, string MonitorLocationID, string RATASumID, cRATA RATA)
      : base(QA, RATA, "RATASUM")
    {
      InitializeCurrent(MonitorLocationID, RATA.TestSumID);

      mMonitorLocationID = MonitorLocationID;
      mQA = QA;
      mRATASumID = RATASumID;
      //mRATA = RATA;

      TableName = "RATA_SUMMARY";
      CurrentRowId = mRATASumID;

      FilterData();

      SetRecordIdentifier();
    }


    /// <summary>
    /// Constructor used by Direct Check Testing
    /// </summary>
    /// <param name="qaReportProcess"></param>
    /// <param name="processInitialized"></param>
    /// <param name="monLocId"></param>
    /// <param name="TestSumID"></param>
    public cRATASummary(cRATA rataCategory, bool processInitialized, string monLocId, string rataSumId)
      : base((cQAMain)rataCategory.Process, "RATA")
    {
      InitializeCurrent(monLocId, rataCategory.TestSumID);

      mMonitorLocationID = monLocId;
      mQA = (cQAMain)rataCategory.Process;
      mRATASumID = rataSumId;

      TableName = "TEST_SUMMARY";
      CurrentRowId = rataSumId;

      if (processInitialized)
      {
        FilterData();
        SetRecordIdentifier();
      }
    }

    #endregion


    #region Public Methods

    public new bool ProcessChecks()
    {
      return base.ProcessChecks();
    }

    #endregion


    #region Base Class Overrides

    protected override void FilterData()
    {
      SetCheckParameter("Current_RATA_Summary", new DataView(mQA.SourceData.Tables["QARATASummary"],
          "rata_sum_id = '" + mRATASumID + "'", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      SetCheckParameter("TValues_Cross_Check_Table", new DataView(mQA.SourceData.Tables["CrossCheck_T-Values"],
        "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Rectangular_Duct_WAF_Records", new DataView(mQA.SourceData.Tables["QARectDuctWAF"],
        "Mon_loc_id = '" + mMonitorLocationID + "'", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Operating_Level_Code_Lookup_Table", new DataView(mQA.SourceData.Tables["OperatingLevelCode"],
        "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Reference_Method_Code_Lookup_Table", new DataView(mQA.SourceData.Tables["ReferenceMethodCode"],
        "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Unit_Stack_Configuration_Records", new DataView(mQA.SourceData.Tables["QAUnitStackConfiguration"],
        "Stack_Pipe_Mon_loc_id = '" + mMonitorLocationID + "'", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);
    }


    protected override void SetRecordIdentifier()
    {
      string OpLevelCd = cDBConvert.ToString(((DataRowView)GetCheckParameter("Current_RATA_Summary").ParameterValue)["op_level_cd"]);
      RecordIdentifier = "Operating Level " + OpLevelCd;
    }

    #endregion

  }
}