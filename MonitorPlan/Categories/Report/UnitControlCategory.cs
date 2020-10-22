using System;
using System.Text;
using System.Data;
using System.Collections.Generic;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Extensions;
using ECMPS.ErrorSuppression;


namespace ECMPS.Checks.MonitorPlanEvaluation
{
  public class cUnitControlCategory : cCategory
  {

    #region Private Constants

    private const string Label = "Unit Control";

    #endregion


    #region Private Fields

    private string _CtlID;
    private cMonitorPlan _MonitorPlan;

    #endregion


    #region Constructors

    public cUnitControlCategory(cCheckEngine CheckEngine, cMonitorPlan MonitorPlan)
      : base(CheckEngine, (cProcess)MonitorPlan, "CONTROL")
    {
      _MonitorPlan = MonitorPlan;
      TableName = "UNIT_CONTROL";
    }

    #endregion


    #region Public Static Methods

    public static cUnitControlCategory GetInitialized(cCheckEngine ACheckEngine, cMonitorPlan AMonitorPlanProcess)
    {
      cUnitControlCategory Category;
      string ErrorMessage = "";

      try
      {
        Category = new cUnitControlCategory(ACheckEngine, AMonitorPlanProcess);

        bool Result = Category.InitCheckBands(ACheckEngine.DbAuxConnection, ref ErrorMessage);

        if (!Result)
        {
          Category = null;
          System.Diagnostics.Debug.WriteLine(string.Format("{0}: {1}", Label, ErrorMessage));
        }
      }
      catch (Exception ex)
      {
        Category = null;
        System.Diagnostics.Debug.WriteLine(string.Format("{0}: {1}", Label, ex.Message));
      }

      return Category;
    }

    #endregion


    #region Public Methods

    public new bool ProcessChecks(string CtlID, string MonitorLocationID)
    {
      _CtlID = CtlID;
      CurrentRowId = _CtlID;

      System.Diagnostics.Debug.WriteLine(string.Format("{0}: {1}", Label, CurrentRowId));

      return base.ProcessChecks(MonitorLocationID);
    }

    #endregion


    #region Base Class Overrides

    protected override void FilterData()
    {
      string sRowFilter = string.Format("CTL_ID='{0}' and LOCATION_IDENTIFIER = UNITID", _CtlID);
      DataRowView drvCurrentRecord = new DataView(_MonitorPlan.SourceData.Tables["LocationControl"], sRowFilter, "CTL_ID", DataViewRowState.CurrentRows)[0];
      SetCheckParameter("Current_Control", drvCurrentRecord, eParameterDataType.DataRowView);
      SetCheckParameter("Current_Unit", drvCurrentRecord, eParameterDataType.DataRowView);


      string sUnitIDFilter = string.Format("UNIT_ID={0} and LOCATION_IDENTIFIER = UNITID", drvCurrentRecord["UNIT_ID"].ToString());
      SetDataViewCheckParameter("Control_Records", _MonitorPlan.SourceData.Tables["LocationControl"], sUnitIDFilter, "");

      SetDataViewCheckParameter("Control_Code_Lookup_Table", _MonitorPlan.SourceData.Tables["ControlCode"], "", "CONTROL_CD");

      SetDataViewCheckParameter("Control_to_Unit_Type_Cross_Check_Table", _MonitorPlan.SourceData.Tables["CrossCheck_ControltoUnitTypeCrossCheckTable"], "", "");

	  SetDataViewCheckParameter("System_Component_Records",	_MonitorPlan.SourceData.Tables["MonitorSystemComponent"],"","");

	  SetDataViewCheckParameter("Analyzer_Range_Records", _MonitorPlan.SourceData.Tables["AnalyzerRange"],"","");
	}

    protected override void SetRecordIdentifier()
    {
      DataRowView CurrentRecord = (DataRowView)GetCheckParameter("Current_Control").ParameterValue;

      string sCEParam = "";
      string sControlCd = "";

      if (CurrentRecord["CE_PARAM"] != DBNull.Value)
        sCEParam = Convert.ToString(CurrentRecord["CE_PARAM"]);
      if (CurrentRecord["CONTROL_CD"] != DBNull.Value)
        sControlCd = Convert.ToString(CurrentRecord["CONTROL_CD"]);
      RecordIdentifier = string.Format(@"{0} {1}", sCEParam, sControlCd);
    }

    protected override bool SetErrorSuppressValues()
    {
      DataRowView currentLocation = GetCheckParameter("Current_Location").ValueAsDataRowView();
      DataRowView currentControl = GetCheckParameter("Current_Control").ValueAsDataRowView();

      if (currentControl != null)
      {
        long facId = CheckEngine.FacilityID;
        string locationName = currentLocation["LOCATION_IDENTIFIER"].AsString();
        string matchDataValue = currentControl["CE_PARAM"].AsString();
        DateTime? matchTimeValue = currentControl["RETIRE_DATE"].AsDateTime();

        ErrorSuppressValues = new cErrorSuppressValues(facId, locationName, "CEPARAM", matchDataValue, "HISTIND", matchTimeValue);
        return true;
      }
      else
        return false;
    }
    #endregion

  }
}
