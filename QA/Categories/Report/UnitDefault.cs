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
  public class cUnitDefault : cQaTestReportCategory
  {
    #region Private Fields

    private string mMonitorLocationID;
    private cQAMain mQA;
    private string mTestSumID;

    #endregion


    #region Constructors

    public cUnitDefault(cCheckEngine CheckEngine, cQAMain QA, string MonitorLocationID, string TestSumID)
      : base(QA, "UNITDEF")
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
    public cUnitDefault(cQAMain qaReportProcess, bool processInitialized, string MonitorLocationID, string TestSumID)
      : base(qaReportProcess, "UNITDEF")
    {
      InitializeCurrent(MonitorLocationID, TestSumID);

      mMonitorLocationID = MonitorLocationID;
      mQA = qaReportProcess;
      mTestSumID = TestSumID;

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
      SetCheckParameter("Current_Unit_Default_Test", new DataView(mQA.SourceData.Tables["QAUnitDefaultTest"],
        "test_sum_id = '" + mTestSumID + "'", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      SetCheckParameter("Current_Test", new DataView(mQA.SourceData.Tables["QATestSummary"],
        "test_sum_id = '" + mTestSumID + "'", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      SetCheckParameter("Fuel_Code_Lookup_Table", new DataView(mQA.SourceData.Tables["FuelCodeLookup"],
        "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Method_Records", new DataView(mQA.SourceData.Tables["QAMethod"],
        "Mon_loc_id = '" + mMonitorLocationID + "'", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Unit_Default_Test_Records", new DataView(mQA.SourceData.Tables["QAUnitDefaultTest"],
        "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Unit_Default_Run_Records", new DataView(mQA.SourceData.Tables["QAUnitDefaultRun"],
        "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("QA_Supplemental_Data_Records", new DataView(mQA.SourceData.Tables["QASuppData"],
        "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Test_Reason_Code_Lookup_Table", new DataView(mQA.SourceData.Tables["TestReasonCode"],
        "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Test_Tolerances_Cross_Check_Table", new DataView(mQA.SourceData.Tables["CrossCheck_TestTolerances"],
        "TestTypeCode = 'APPE'", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Default_Records", new DataView(mQA.SourceData.Tables["QADefault"],
          "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Location_Unit_Type_Records", new DataView(mQA.SourceData.Tables["QAUnitType"],
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

    }

    protected override void SetRecordIdentifier()
    {
      RecordIdentifier = "this test";
    }

    #endregion
  }
}
