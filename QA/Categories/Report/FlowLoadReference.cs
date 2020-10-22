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
  public class cFlowLoadReference : cQaTestReportCategory
  {

    #region Private Fields

    private string mMonitorLocationID;
    private cQAMain mQA;
    private string mTestSumID;
    private string mSystemID;

    //private long mUnitID;
    //private string mStackPipeID;

    #endregion


    #region Constructors

    public cFlowLoadReference(cCheckEngine CheckEngine, cQAMain QA, string MonitorLocationID, string TestSumID)
      : base(QA, "F2LREF")
    {
      InitializeCurrent(MonitorLocationID, TestSumID);

      mMonitorLocationID = MonitorLocationID;
      mQA = QA;
      mTestSumID = TestSumID;

      TableName = "TEST_SUMMARY";
      CurrentRowId = mTestSumID;

      FilterData();

      SetRecordIdentifier();
    }


    #endregion

    public string TestSumID
    {
      get
      {
        return mTestSumID;
      }
    }


    #region Public Methods

    public new bool ProcessChecks()
    {
      return base.ProcessChecks();
    }

    #endregion


    #region Base Class Overrides

    protected override void FilterData()
    {
      SetCheckParameter("Current_Flow_to_Load_Reference", new DataView(mQA.SourceData.Tables["QAFlowLoadReference"],
        "test_sum_id = '" + mTestSumID + "'", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      SetCheckParameter("Current_Test", new DataView(mQA.SourceData.Tables["QATestSummary"],
        "test_sum_id = '" + mTestSumID + "'", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      mSystemID = cDBConvert.ToString(((DataRowView)GetCheckParameter("Current_Flow_To_Load_Reference").ParameterValue)["mon_sys_id"]);

      SetCheckParameter("Current_Location", new DataView(mQA.SourceData.Tables["QALocation"],
          "", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      SetCheckParameter("Flow_to_Load_Reference_Records", new DataView(mQA.SourceData.Tables["QAFlowLoadReference"],
        "mon_sys_id = '" + mSystemID + "'", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("QA_Supplemental_Data_Records", new DataView(mQA.SourceData.Tables["QASuppData"],
        "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("QA_Supplemental_Attribute_Records", new DataView(mQA.SourceData.Tables["QASuppAttribute"],
        "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Facility_QA_Supplemental_Data_Records", new DataView(mQA.SourceData.Tables["FacilityQASuppData"],
        "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Facility_QA_Supplemental_Attribute_Records", new DataView(mQA.SourceData.Tables["FacilityQASuppAttribute"],
        "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Facility_Unit_Stack_Configuration_Records", new DataView(mQA.SourceData.Tables["FacilityUnitStackConfiguration"],
        "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Facility_RATA_Run_Records", new DataView(mQA.SourceData.Tables["FacilityRATARun"],
        "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Facility_RATA_Summary_Records", new DataView(mQA.SourceData.Tables["FacilityRATASummary"],
        "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("System_RATA_Summary_Records", new DataView(mQA.SourceData.Tables["FacilityRATASummary"],
        "mon_sys_id = '" + mSystemID + "'", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Test_Tolerances_Cross_Check_Table", new DataView(mQA.SourceData.Tables["CrossCheck_TestTolerances"],
         "TestTypeCode = 'F2LREF'", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Operating_Level_Code_Lookup_Table", new DataView(mQA.SourceData.Tables["OperatingLevelCode"],
        "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Load_Records", new DataView(mQA.SourceData.Tables["QALoad"],
        "Mon_loc_id = '" + mMonitorLocationID + "'", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Monitoring_Plan_Location_Records_for_QA", new DataView(mQA.SourceData.Tables["MonitorPlanLocation"],
        "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);


    }


    protected override void SetRecordIdentifier()
    {
      RecordIdentifier = "this test";
    }

    #endregion

  }
}