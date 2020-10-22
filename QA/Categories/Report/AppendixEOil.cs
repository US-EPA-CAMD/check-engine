using System;
using System.Data;
using System.Collections;
using System.Text.RegularExpressions;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;
using ECMPS.ErrorSuppression;

namespace ECMPS.Checks.QAEvaluation
{
  public class cAppendixEOil : cQaTestReportCategory
  {
    #region Private Fields

    private string mMonitorLocationID;
    private cQAMain mQA;
    private string mAPPEHIOilId;

    private string mSystemID;

    #endregion

    #region Constructors

    public cAppendixEOil(cCheckEngine CheckEngine, cQAMain QA, string MonitorLocationID, string APPEHIOilId, cAppendixERun AppendixERun)
      : base(QA, AppendixERun, "APPEOIL")
    {
      InitializeCurrent(MonitorLocationID, CheckEngine.TestSumId);

      mMonitorLocationID = MonitorLocationID;
      mQA = QA;
      mAPPEHIOilId = APPEHIOilId;
      //cAppendixERun = AppendixERun;

      TableName = "AE_HI_OIL";
      CurrentRowId = mAPPEHIOilId;

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
      SetCheckParameter("Current_Appendix_E_HI_for_Oil", new DataView(mQA.SourceData.Tables["QAAppendixEOil"],
          "AE_HI_OIL_ID = '" + mAPPEHIOilId + "'", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      mSystemID = cDBConvert.ToString(((DataRowView)GetCheckParameter("Current_Appendix_E_HI_for_Oil").ParameterValue)["MON_SYS_ID"]);

      SetCheckParameter("Monitor_System_Records", new DataView(mQA.SourceData.Tables["MonitorSystemRecords"],
          "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Parameter_Units_Of_Measure_Lookup_Table", new DataView(mQA.SourceData.Tables["UnitsOfMeasureLookup"],
          "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Test_Tolerances_Cross_Check_Table", new DataView(mQA.SourceData.Tables["CrossCheck_TestTolerances"],
          "TestTypeCode = 'APPE'", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Oil_Volume_UOM_to_Density_UOM_to_GCV_UOM", new DataView(mQA.SourceData.Tables["CrossCheck_OilVolumeUOMtoDensityUOMtoGCVUOM"],
          "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);
    }


    protected override void SetRecordIdentifier()
    {
      RecordIdentifier = "Operating Level " +
        cDBConvert.ToString(((DataRowView)GetCheckParameter("Current_Appendix_E_HI_for_Oil").ParameterValue)["OP_LEVEL_NUM"]) +
        " Run Number " +
        cDBConvert.ToInteger(((DataRowView)GetCheckParameter("Current_Appendix_E_HI_for_Oil").ParameterValue)["RUN_NUM"]) +
        " Oil System ID " +
        cDBConvert.ToString(((DataRowView)GetCheckParameter("Current_Appendix_E_HI_for_Oil").ParameterValue)["SYSTEM_IDENTIFIER"]);
    }

    #endregion
  }
}
