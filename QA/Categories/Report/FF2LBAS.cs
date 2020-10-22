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
  public class cFF2LBAS : cQaTestReportCategory
  {
    #region Private Fields

    private string mMonitorLocationID;
    private cQAMain mQA;
    private string mTestSumID;
    private string mMonSysID;

    #endregion


    #region Constructors

    public cFF2LBAS(cCheckEngine CheckEngine, cQAMain QA, string MonitorLocationID, string TestSumID)
      : base(QA, "FF2LBAS")
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
      SetCheckParameter("Current_Fuel_Flow_To_Load_Baseline", new DataView(mQA.SourceData.Tables["QAFuelFlowToLoadBaselineTest"],
        "test_sum_id = '" + mTestSumID + "'", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      SetCheckParameter("Current_Test", new DataView(mQA.SourceData.Tables["QATestSummary"],
        "test_sum_id = '" + mTestSumID + "'", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      mMonSysID = cDBConvert.ToString(((DataRowView)GetCheckParameter("Current_Fuel_Flow_To_Load_Baseline").ParameterValue)["mon_sys_id"]);

      SetCheckParameter("Associated_System", new DataView(mQA.SourceData.Tables["QATestSummary"],
        "test_sum_id = '" + mTestSumID + "'", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      SetCheckParameter("System_System_Component_Records", new DataView(mQA.SourceData.Tables["QASystemComponent"],
        "mon_sys_id = '" + mMonSysID + "'", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Fuel_Flow_To_Load_Baseline_Records", new DataView(mQA.SourceData.Tables["QAFuelFlowToLoadBaselineTest"],
        "mon_sys_id = '" + mMonSysID + "'", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("QA_Supplemental_Data_Records", new DataView(mQA.SourceData.Tables["QASuppData"],
        "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Location_Test_Records", new DataView(mQA.SourceData.Tables["TestSummary"],
          "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Load_Records", new DataView(mQA.SourceData.Tables["QALoad"],
          "Mon_loc_id = '" + mMonitorLocationID + "'", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Unit_Stack_Configuration_Records", new DataView(mQA.SourceData.Tables["QAUnitStackConfiguration"],
          "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Facility_Unit_Capacity_Records", new DataView(mQA.SourceData.Tables["QAUnitCapacity"],
          "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Parameter_Units_Of_Measure_Lookup_Table", new DataView(mQA.SourceData.Tables["UnitsOfMeasureLookup"],
          "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Fuel_Flow_To_Load_Baseline_Uom_To_Load_Uom_And_Systemtype_Cross_Check_Table", new DataView(mQA.SourceData.Tables["Crosscheck_FuelFlowtoLoadBaselineUOMtoLoadUOMandSystemType"],
          "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

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
