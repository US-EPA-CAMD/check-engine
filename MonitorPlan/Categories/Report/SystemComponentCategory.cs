using System;
using System.Data;
using System.Collections;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Extensions;

using ECMPS.ErrorSuppression;


namespace ECMPS.Checks.MonitorPlanEvaluation
{
  public class cSystemComponentCategory : cCategory
  {
    #region Private Constants

    private const string Label = "System Component Category";

    #endregion


    #region Private Fields

    private string MonSysCompId;
    private cMonitorPlan mMonitorPlan;

    #endregion


    #region Constructors

    public cSystemComponentCategory(cCheckEngine CheckEngine, cMonitorPlan MonitorPlan)
      : base(CheckEngine, (cProcess)MonitorPlan, "SYSCOMP")
    {
      mMonitorPlan = MonitorPlan;
      TableName = "MONITOR_SYSTEM_COMPONENT";
    }

    #endregion


    #region Public Static Methods

    public static cSystemComponentCategory GetInitialized(cCheckEngine ACheckEngine, cMonitorPlan AMonitorPlanProcess)
    {
      cSystemComponentCategory Category;
      string ErrorMessage = "";

      try
      {
        Category = new cSystemComponentCategory(ACheckEngine, AMonitorPlanProcess);

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

    public new bool ProcessChecks(string monSysCompId, string monLocId)
    {
      MonSysCompId = monSysCompId;
      CurrentRowId = monSysCompId;

      System.Diagnostics.Debug.WriteLine(string.Format("{0}: {1}", Label, CurrentRowId));

      return base.ProcessChecks(monLocId);
    }

    #endregion


    #region Base Class Overrides

    protected override void FilterData()
    {
      //filter MonitorPlanSystemComponent to find this one based on the mComponentID and mMonSysID
      SetDataRowCheckParameter("Current_System_Component",
                               mMonitorPlan.SourceData.Tables["MonitorSystemComponent"],
                               "mon_sys_comp_id = '" + MonSysCompId + "'",
                               "");

      SetDataViewCheckParameter("Formula_Parameter_And_Component_Type_And_Basis_To_Formula_Code_Cross_Check_Table",
                                mMonitorPlan.SourceData.Tables["CrossCheck_FormulaParameterandComponentTypeandBasistoFormulaCode"],
                                "",
                                "");

      SetDataViewCheckParameter("System_Type_To_Formula_Parameter_Cross_Check_Table",
                                mMonitorPlan.SourceData.Tables["CrossCheck_SystemTypetoFormulaParameter"],
                                "",
                                "");

      SetDataViewCheckParameter("System_Type_to_Optional_Component_Type_Cross_Check_Table",
                                mMonitorPlan.SourceData.Tables["CrossCheck_SystemTypetoOptionalComponentType"],
                                "",
                                "");
    }

    protected override void SetRecordIdentifier()
    {
      DataRowView CurrentSystemComponent = (DataRowView)GetCheckParameter("Current_System_Component").ParameterValue;

      RecordIdentifier = "SystemID " + CurrentSystemComponent["SYSTEM_IDENTIFIER"].AsString() +
                          " ComponentID " + CurrentSystemComponent["COMPONENT_IDENTIFIER"].AsString();
    }

    protected override bool SetErrorSuppressValues()
    {
      DataRowView currentLocation = GetCheckParameter("Current_Location").ValueAsDataRowView();
      DataRowView currentSystem = GetCheckParameter("Current_System").ValueAsDataRowView();
      DataRowView currentSystemComponent = GetCheckParameter("Current_System_Component").ValueAsDataRowView();

      if ((currentLocation != null) && (currentSystem != null) && (currentSystemComponent != null))
      {
        long facId = CheckEngine.FacilityID;
        string locationName = currentLocation["LOCATION_IDENTIFIER"].AsString();
        string matchDataValue = currentSystem["SYS_TYPE_CD"].AsString();
        DateTime? matchTimeValue = currentSystemComponent["END_DATE"].AsDateTime();

        ErrorSuppressValues = new cErrorSuppressValues(facId, locationName, "SYSTYP", matchDataValue, "HISTIND", matchTimeValue);
        return true;
      }
      else
        return false;
    }

    #endregion
  }
}
