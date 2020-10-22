using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Data.Ecmps.Lookup.Table;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.QA;
using ECMPS.Checks.Qa.Parameters;
using ECMPS.Checks.TypeUtilities;


namespace ECMPS.Checks.QAEvaluation
{
  public class cRATA : cQaTestReportCategory
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

    public cRATA(cCheckEngine CheckEngine, cQAMain QA, string MonitorLocationID, string TestSumID)
      : base(QA, "RATA")
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
    public cRATA(cQAMain qaReportProcess, bool processInitialized, string MonitorLocationID, string TestSumID)
      : base(qaReportProcess, "RATA")
    {
      InitializeCurrent(MonitorLocationID, TestSumID);

      mMonitorLocationID = MonitorLocationID;
      mQA = qaReportProcess;
      mTestSumID = TestSumID;

      if (processInitialized)
      {
        FilterData();
        SetRecordIdentifier();
      }

      TableName = "TEST_SUMMARY";
      CurrentRowId = mTestSumID;
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
      SetCheckParameter("Current_RATA", new DataView(mQA.SourceData.Tables["QARATA"],
          "test_sum_id = '" + mTestSumID + "'", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      SetCheckParameter("Current_Test", new DataView(mQA.SourceData.Tables["QATestSummary"],
        "test_sum_id = '" + mTestSumID + "'", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      mSystemID = cDBConvert.ToString(((DataRowView)GetCheckParameter("Current_RATA").ParameterValue)["mon_sys_id"]);

      SetCheckParameter("RATA_Run_Records", new DataView(mQA.SourceData.Tables["QARATARun"],
        "test_sum_id = '" + mTestSumID + "'", "begin_date, begin_hour, begin_min", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("RATA_Records", new DataView(mQA.SourceData.Tables["QARATA"],
        "mon_sys_id = '" + mSystemID + "'", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("QA_Supplemental_Data_Records", new DataView(mQA.SourceData.Tables["QASuppData"],
        "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Test_Reason_Code_Lookup_Table", new DataView(mQA.SourceData.Tables["TestReasonCode"],
        "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Test_Result_Code_Lookup_Table", new DataView(mQA.SourceData.Tables["TestResultCode"],
        "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Test_Tolerances_Cross_Check_Table", new DataView(mQA.SourceData.Tables["CrossCheck_TestTolerances"],
        "TestTypeCode = 'RATA'", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Span_Records", new DataView(mQA.SourceData.Tables["QASpan"],
              "Mon_loc_id = '" + mMonitorLocationID + "'", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Default_Records", new DataView(mQA.SourceData.Tables["QADefault"],
        "Mon_loc_id = '" + mMonitorLocationID + "'", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Load_Records", new DataView(mQA.SourceData.Tables["QALoad"],
        "Mon_loc_id = '" + mMonitorLocationID + "'", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("RATA_Frequency_Code_Lookup_Table", new DataView(mQA.SourceData.Tables["RATAFrequencyCode"],
        "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Facility_Qualification_Records", new DataView(mQA.SourceData.Tables["QAMonitorQualification"],
        "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Location_Attribute_Records", new DataView(mQA.SourceData.Tables["QALocationAttribute"],
        "Mon_loc_id = '" + mMonitorLocationID + "'", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Location_Reporting_Frequency_Records", new DataView(mQA.SourceData.Tables["QAReportingFrequency"],
        "Mon_loc_id = '" + mMonitorLocationID + "'", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("RATA_Summary_Records", new DataView(mQA.SourceData.Tables["QARATASummary"],
        "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Monitoring_Plan_Location_Records_for_QA", new DataView(mQA.SourceData.Tables["MonitorPlanLocation"],
        "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      // QA Process Parameters
      {
        cQaProcess qaProcess = (cQaProcess)Process;

        qaProcess.AirEmissionTestingRecords.LegacySetValue(Process.SourceData.Tables["AirEmissionTesting"].DefaultView, this);
        qaProcess.CrossCheckProtocolGasParameterToType.LegacySetValue(Process.SourceData.Tables["CrossCheck_ProtocolGasParameterToType"].DefaultView, this);
        qaProcess.GasComponentCodeLookupTable.LegacySetValue(Process.SourceData.Tables["GasComponentCode"].DefaultView, this);
        qaProcess.GasTypeCodeLookupTable.LegacySetValue(Process.SourceData.Tables["GasTypeCode"].DefaultView, this);
        qaProcess.ProtocolGasVendorLookupTable.LegacySetValue(Process.SourceData.Tables["ProtocolGasVendor"].DefaultView, this);
        qaProcess.SystemParameterLookupTable.LegacySetValue(Process.SourceData.Tables["SystemParameter"].DefaultView, this);
      }

      QaParameters.ApsCodeLookupTable = new CheckDataView<ApsCodeRow>(Process.SourceData.Tables["ApsCode"].DefaultView);

    }

    protected override void SetRecordIdentifier()
    {
      RecordIdentifier = "this test";
    }


    #endregion

  }
}