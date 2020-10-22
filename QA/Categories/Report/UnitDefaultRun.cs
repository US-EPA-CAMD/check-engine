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
  public class cUnitDefaultRun : cQaTestReportCategory
  {
    #region Private Fields

    private string mMonitorLocationID;
    private cQAMain mQA;
    private string mUnitDefaultRunID;

    #endregion


    #region Constructors

    public cUnitDefaultRun(cCheckEngine CheckEngine, cQAMain QA, string MonitorLocationID, string UnitDefaultRunID, cUnitDefault UnitDefault)
      : base(QA, UnitDefault, "UDEFRUN")
    {
      InitializeCurrent(MonitorLocationID, CheckEngine.TestSumId);

      mMonitorLocationID = MonitorLocationID;
      mQA = QA;
      mUnitDefaultRunID = UnitDefaultRunID;
      TableName = "UNIT_DEFAULT_TEST_RUN";
      CurrentRowId = mUnitDefaultRunID;

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
      SetCheckParameter("Current_Unit_Default_Run", new DataView(mQA.SourceData.Tables["QAUnitDefaultRun"],
          "UNIT_DEFAULT_TEST_RUN_ID = '" + mUnitDefaultRunID + "'", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);
    }


    protected override void SetRecordIdentifier()
    {
      RecordIdentifier = "Operating Level " +
        cDBConvert.ToString(((DataRowView)GetCheckParameter("Current_Unit_Default_Run").ParameterValue)["OP_LEVEL_NUM"]) +
        " Run Number " +
        cDBConvert.ToInteger(((DataRowView)GetCheckParameter("Current_Unit_Default_Run").ParameterValue)["run_num"]);
    }

    #endregion
  }
}
