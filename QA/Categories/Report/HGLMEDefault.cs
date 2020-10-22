using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;
using ECMPS.ErrorSuppression;

namespace ECMPS.Checks.QAEvaluation
{
  public class cHGLMEDefault : cQaTestReportCategory
  {
    #region Private Fields

    private string mMonitorLocationID;
    private cQAMain mQA;
    private string mTestSumID;
    //private string mComponentID;

    //private long mUnitID;
    //private string mStackPipeID;

    #endregion


    #region Constructors

    public cHGLMEDefault(cCheckEngine CheckEngine, cQAMain QA, string MonitorLocationID, string TestSumID)
      : base(QA, "HGLME")
    {
      InitializeCurrent(MonitorLocationID, TestSumID);

      mMonitorLocationID = MonitorLocationID;
      mQA = QA;
      mTestSumID = TestSumID;

      TableName = "TEST_SUMMARY";//HG_LME_DEFAULT_TEST?
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
      SetCheckParameter("Current_Hg_LME_Default_Test", new DataView(mQA.SourceData.Tables["QAHgLMEDefaultTest"],
          "test_sum_id = '" + mTestSumID + "'", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      SetCheckParameter("Hg_LME_Default_Test_Records", new DataView(mQA.SourceData.Tables["QAHgLMEDefaultTest"],
          "mon_loc_id = '" + mMonitorLocationID + "'", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Facility_Hg_LME_Default_Tests", new DataView(mQA.SourceData.Tables["FacilityHGLMEDefaultTests"],
          "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Hg_LME_Default_Run_Records", new DataView(mQA.SourceData.Tables["QAHgLMEDefaultRun"],
          "TEST_SUM_ID = '" + mTestSumID + "'", "begin_date, begin_hour, begin_min", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Test_Reason_Code_Lookup_Table", new DataView(mQA.SourceData.Tables["TestReasonCode"],
          "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Common_Stack_Test_Code_Lookup_Table", new DataView(mQA.SourceData.Tables["CSTestCode"],
          "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Current_Test", new DataView(mQA.SourceData.Tables["QATestSummary"],
          "test_sum_id = '" + mTestSumID + "'", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      SetCheckParameter("Monitoring_Plan_Location_Records_for_QA", new DataView(mQA.SourceData.Tables["MonitorPlanLocation"],
          "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Facility_Monitor_Plan_Location_Records", new DataView(mQA.SourceData.Tables["FacilityMonitorPlanLocation"],
          "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("QA_Supplemental_Data_Records", new DataView(mQA.SourceData.Tables["QASuppData"],
          "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("QA_Supplemental_Attribute_Records", new DataView(mQA.SourceData.Tables["QASuppAttribute"],
          "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Facility_QA_Supplemental_Data_Records", new DataView(mQA.SourceData.Tables["FacilityQASuppData"],
          "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Facility_QA_Supplemental_Attribute_Records", new DataView(mQA.SourceData.Tables["FacilityQASuppAttribute"],
          "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Default_Records", new DataView(mQA.SourceData.Tables["QADefault"],
          "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Span_Records", new DataView(mQA.SourceData.Tables["QASpan"],
          "Mon_loc_id = '" + mMonitorLocationID + "'", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Facility_Method_Records", new DataView(mQA.SourceData.Tables["QAFacilityMethod"],
          "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Method_Records", new DataView(mQA.SourceData.Tables["QAMethod"],
          "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Unit_Stack_Configuration_Records", new DataView(mQA.SourceData.Tables["QAUnitStackConfiguration"],
          "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Current_Monitor_Location", new DataView(mQA.SourceData.Tables["MonitorLocation"],
          "", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataView);
    }

    protected override void SetRecordIdentifier()
    {
      RecordIdentifier = "this test";
    }

    #endregion

  }
}
