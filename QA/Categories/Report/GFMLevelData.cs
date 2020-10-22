using System;
using System.Data;
using System.Collections;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;
using ECMPS.ErrorSuppression;

namespace ECMPS.Checks.QAEvaluation
{
  public class cGFMLevelData : cQaTestReportCategory
  {
    #region Private Fields

    private string mMonitorLocationID;
    private cQAMain mQA;
    private string mCalibrationDataID;

    #endregion


    #region Constructors

    public cGFMLevelData(cCheckEngine CheckEngine, cQAMain QA, string MonitorLocationID, string CalibrationDataID, cGFMTest GFMTest)
      : base(QA, GFMTest, "GFMCLVL")
    {
      InitializeCurrent(MonitorLocationID, GFMTest.TestSumID);

      mMonitorLocationID = MonitorLocationID;
      mQA = QA;
      mCalibrationDataID = CalibrationDataID;

      TableName = "GFM_CALIBRATION_DATA";
      CurrentRowId = mCalibrationDataID;

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
      SetCheckParameter("Current_GFM_Calibration_Data", new DataView(mQA.SourceData.Tables["QAGFMData"],
          "GFM_CALIBRATION_DATA_ID = '" + mCalibrationDataID + "'", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);
    }

    protected override void SetRecordIdentifier()
    {

    }

    #endregion
  }
}
