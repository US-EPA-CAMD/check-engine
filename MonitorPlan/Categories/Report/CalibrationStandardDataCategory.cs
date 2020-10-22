using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.ErrorSuppression;

namespace ECMPS.Checks.MonitorPlanEvaluation
{
  public class cCalibrationStandardDataCategory : cCategory
  {
    #region Private Fields

    private string mCalibrationStandardID;
    private cMonitorPlan mMonitorPlan;

    #endregion

    #region Constructors

    public cCalibrationStandardDataCategory(cCheckEngine CheckEngine, cMonitorPlan MonitorPlan)
      : base(CheckEngine, (cProcess)MonitorPlan, "CALSTD")
    {
      mMonitorPlan = MonitorPlan;
      TableName = "CALIBRATION_STANDARD";
    }

    #endregion

    #region Public Methods

    public new bool ProcessChecks(string CalibrationStandardID, string MonitorLocationID)
    {
      mCalibrationStandardID = CalibrationStandardID;
      CurrentRowId = mCalibrationStandardID;

      System.Diagnostics.Debug.WriteLine(string.Format("{0}: {1}", "Calibration Standard Data", CurrentRowId));

      return base.ProcessChecks(MonitorLocationID);
    }

    #endregion

    #region Public Static Methods

    public static cCalibrationStandardDataCategory GetInitialized(cCheckEngine ACheckEngine, cMonitorPlan AMonitorPlanProcess)
    {
      cCalibrationStandardDataCategory Category;
      string ErrorMessage = "";

      try
      {
        Category = new cCalibrationStandardDataCategory(ACheckEngine, AMonitorPlanProcess);

        bool Result = Category.InitCheckBands(ACheckEngine.DbAuxConnection, ref ErrorMessage);

        if (!Result)
        {
          Category = null;
          System.Diagnostics.Debug.WriteLine(string.Format("{0}: {1}", "Calibration Standard Data", ErrorMessage));
        }
      }
      catch (Exception ex)
      {
        Category = null;
        System.Diagnostics.Debug.WriteLine(string.Format("{0}: {1}", "Calibration Standard Data", ex.Message));
      }

      return Category;
    }

    #endregion


    #region Base Class Overrides

    protected override void FilterData()
    {
      SetCheckParameter("Current_Calibration_Standard_Data", new DataView(mMonitorPlan.SourceData.Tables["CalibrationStandard"],
      "calibration_standard_id = '" + mCalibrationStandardID + "'", "calibration_standard_id", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      SetDataViewCheckParameter("Calibration_Standard_Code_Lookup_Table", mMonitorPlan.SourceData.Tables["CalibrationStdCode"], "", "");

      SetDataViewCheckParameter("Calibration_Standard_Code_to_Component_Type_Cross_Check_Table",
          mMonitorPlan.SourceData.Tables["CrossCheck_CalibrationStandardCodetoComponentType"], "", "");
    }

    protected override void SetRecordIdentifier()
    {
      DataRowView CurrentCalibrationStd = (DataRowView)GetCheckParameter("Current_Calibration_Standard_Data").ParameterValue;

      RecordIdentifier = (string)CurrentCalibrationStd["Component_identifier"] + " " + ((DateTime)CurrentCalibrationStd["begin_date"]).ToShortDateString();
    }

    protected override bool SetErrorSuppressValues()
    {
      return true;
    }

    #endregion
  }
}
