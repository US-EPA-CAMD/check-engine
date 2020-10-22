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
  public class cAppendixEGas : cQaTestReportCategory
  {
    #region Private Fields

    private string mMonitorLocationID;
    private cQAMain mQA;
    private string mAPPEHIGasId;
    private string mSystemID;

    #endregion

    #region Constructors

    public cAppendixEGas(cCheckEngine CheckEngine, cQAMain QA, string MonitorLocationID, string APPEHIGasId, cAppendixERun AppendixERun)
      : base(QA, AppendixERun, "APPEGAS")
    {
      InitializeCurrent(MonitorLocationID, CheckEngine.TestSumId);

      mMonitorLocationID = MonitorLocationID;
      mQA = QA;
      mAPPEHIGasId = APPEHIGasId;
      //cAppendixERun = AppendixERun;

      TableName = "AE_HI_GAS";
      CurrentRowId = mAPPEHIGasId;

      FilterData();

      SetRecordIdentifier();
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
      SetCheckParameter("Current_Appendix_E_HI_for_Gas", new DataView(mQA.SourceData.Tables["QAAppendixEGas"],
        "AE_HI_GAS_ID = '" + mAPPEHIGasId + "'", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      mSystemID = cDBConvert.ToString(((DataRowView)GetCheckParameter("Current_Appendix_E_HI_for_Gas").ParameterValue)["MON_SYS_ID"]);

      SetCheckParameter("Parameter_Units_Of_Measure_Lookup_Table", new DataView(mQA.SourceData.Tables["UnitsOfMeasureLookup"],
          "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Test_Tolerances_Cross_Check_Table", new DataView(mQA.SourceData.Tables["CrossCheck_TestTolerances"],
          "TestTypeCode = 'APPE'", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);
    }


    protected override void SetRecordIdentifier()
    {
      RecordIdentifier = "Operating Level " +
        cDBConvert.ToString(((DataRowView)GetCheckParameter("Current_Appendix_E_HI_for_Gas").ParameterValue)["OP_LEVEL_NUM"]) +
        " Run Number " +
        cDBConvert.ToInteger(((DataRowView)GetCheckParameter("Current_Appendix_E_HI_for_Gas").ParameterValue)["RUN_NUM"]) +
        " Gas System ID " +
        cDBConvert.ToString(((DataRowView)GetCheckParameter("Current_Appendix_E_HI_for_Gas").ParameterValue)["SYSTEM_IDENTIFIER"]);
    }


    #endregion
  }
}
