using System;
using System.Data;
using System.Collections;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.QA;
using ECMPS.Checks.TypeUtilities;
using ECMPS.ErrorSuppression;


namespace ECMPS.Checks.QAEvaluation
{
  public class cLinearity : cQaTestReportCategory
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

    public cLinearity(cCheckEngine CheckEngine, cQAMain QA, string MonitorLocationID, string TestSumID)
      : base(QA, "LINTEST")
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


    /// <summary>
    /// Constructor used by Direct Check Testing
    /// </summary>
    /// <param name="qaReportProcess"></param>
    /// <param name="processInitialized"></param>
    /// <param name="MonitorLocationID"></param>
    /// <param name="TestSumID"></param>
    public cLinearity(cQAMain qaReportProcess, bool processInitialized, string monLocId, string testSumId)
      : base(qaReportProcess, "LINTEST")
    {
      InitializeCurrent(monLocId, testSumId);

      mMonitorLocationID = monLocId;
      mQA = qaReportProcess;
      mTestSumID = testSumId;

      TableName = "TEST_SUMMARY";
      CurrentRowId = mTestSumID;

      if (processInitialized)
      {
        FilterData();
        SetRecordIdentifier();
      }
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

      SetCheckParameter("Current_Linearity_Test", new DataView(mQA.SourceData.Tables["QALinearityTest"],
          "test_sum_id = '" + mTestSumID + "'", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      SetCheckParameter("Current_Test", new DataView(mQA.SourceData.Tables["QATestSummary"],
        "test_sum_id = '" + mTestSumID + "'", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      mComponentID = cDBConvert.ToString(((DataRowView)GetCheckParameter("Current_Linearity_Test").ParameterValue)["component_id"]);

      SetCheckParameter("Linearity_Test_Injection_Records", new DataView(mQA.SourceData.Tables["QALinearityInjection"],
        "test_sum_id = '" + mTestSumID + "'", "injection_date, injection_hour, injection_min", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Analyzer_Range_Records", new DataView(mQA.SourceData.Tables["QAAnalyzerRange"],
        "component_id = '" + mComponentID + "'", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("System_Component_Records", new DataView(mQA.SourceData.Tables["QASystemComponent"],
        "component_id = '" + mComponentID + "'", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Current_Component", new DataView(mQA.SourceData.Tables["QAComponent"],
        "component_id = '" + mComponentID + "'", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      SetCheckParameter("Span_Records", new DataView(mQA.SourceData.Tables["QASpan"],
        "Mon_loc_id = '" + mMonitorLocationID + "'", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Linearity_Test_Records", new DataView(mQA.SourceData.Tables["QALinearityTest"],
        "component_id = '" + mComponentID + "'", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("QA_Supplemental_Data_Records", new DataView(mQA.SourceData.Tables["QASuppData"],
        "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Test_Reason_Code_Lookup_Table", new DataView(mQA.SourceData.Tables["TestReasonCode"],
        "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Test_Result_Code_Lookup_Table", new DataView(mQA.SourceData.Tables["TestResultCode"],
        "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Monitoring_Plan_Location_Records_for_QA", new DataView(mQA.SourceData.Tables["MonitorPlanLocation"],
        "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      // QA Process Parameters
      {
        cQaProcess qaProcess = (cQaProcess)Process;

        qaProcess.CrossCheckProtocolGasParameterToType.LegacySetValue(Process.SourceData.Tables["CrossCheck_ProtocolGasParameterToType"].DefaultView, this);
        qaProcess.GasComponentCodeLookupTable.LegacySetValue(Process.SourceData.Tables["GasComponentCode"].DefaultView, this);
        qaProcess.GasTypeCodeLookupTable.LegacySetValue(Process.SourceData.Tables["GasTypeCode"].DefaultView, this);
        qaProcess.ProtocolGasVendorLookupTable.LegacySetValue(Process.SourceData.Tables["ProtocolGasVendor"].DefaultView, this);
        qaProcess.SystemParameterLookupTable.LegacySetValue(Process.SourceData.Tables["SystemParameter"].DefaultView, this);
        
      }

    }


    protected override void SetRecordIdentifier()
    {
      RecordIdentifier = "this test";
    }

    #endregion

  }
}