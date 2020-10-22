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
  public class cOOC : cQaTestReportCategory
	{
 
		#region Private Fields

		private string mMonitorLocationID;
		private cQAMain mQA;
        private string mTestSumID;
        private string mComponentID;

		//private long mUnitID;
		//private string mStackPipeID;
		
		#endregion	


		#region Constructors

		public cOOC(cCheckEngine CheckEngine, cQAMain QA, string MonitorLocationID, string TestSumID) 
      : base(QA, "ONOFF")
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
            SetCheckParameter("Current_OOC_Test", new DataView(mQA.SourceData.Tables["QAOOCTest"],
              "test_sum_id = '" + mTestSumID + "'", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

            SetCheckParameter("Current_Test", new DataView(mQA.SourceData.Tables["QATestSummary"],
              "test_sum_id = '" + mTestSumID + "'", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

            mComponentID = cDBConvert.ToString(((DataRowView)GetCheckParameter("Current_OOC_Test").ParameterValue)["component_id"]);

            SetCheckParameter("Analyzer_Range_Records", new DataView(mQA.SourceData.Tables["QAAnalyzerRange"],
              "component_id = '" + mComponentID + "'", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

            SetCheckParameter("System_Component_Records", new DataView(mQA.SourceData.Tables["QASystemComponent"],
              "component_id = '" + mComponentID + "'", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

            SetCheckParameter("Component_Records", new DataView(mQA.SourceData.Tables["QAComponent"],
                "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

            SetCheckParameter("Current_Component", new DataView(mQA.SourceData.Tables["QAComponent"],
              "component_id = '" + mComponentID + "'", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

            SetCheckParameter("Span_Records", new DataView(mQA.SourceData.Tables["QASpan"],
              "Mon_loc_id = '" + mMonitorLocationID + "'", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

            SetCheckParameter("OOC_Test_Records", new DataView(mQA.SourceData.Tables["QAOOCTest"],
              "component_id = '" + mComponentID + "'", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

            SetCheckParameter("QA_Supplemental_Data_Records", new DataView(mQA.SourceData.Tables["QASuppData"],
              "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

            SetCheckParameter("Test_Reason_Code_Lookup_Table", new DataView(mQA.SourceData.Tables["TestReasonCode"],
              "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

            SetCheckParameter("Test_Result_Code_Lookup_Table", new DataView(mQA.SourceData.Tables["TestResultCode"],
              "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

            SetCheckParameter("Test_Tolerances_Cross_Check_Table", new DataView(mQA.SourceData.Tables["CrossCheck_TestTolerances"],
              "TestTypeCode = '7DAY'", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

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