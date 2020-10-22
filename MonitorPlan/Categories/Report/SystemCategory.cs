using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Extensions;
using ECMPS.ErrorSuppression;


namespace ECMPS.Checks.MonitorPlanEvaluation
{
  public class cSystemCategory : cCategory
  {

    #region Private Constants

    private const string Label = "Monitor System";

    #endregion


    #region Constructors

    public cSystemCategory(cCheckEngine ACheckEngine, cMonitorPlan AMonitorPlanProcess)
      : base(ACheckEngine, (cProcess)AMonitorPlanProcess, "SYSTEM")
    {
      FMonitorPlanProcess = AMonitorPlanProcess;
      TableName = "MONITOR_SYSTEM";
    }

    #endregion


    #region Private Fields

    private string FMonSysId;
    private cMonitorPlan FMonitorPlanProcess;

    #endregion


    #region Public Static Methods

    public static cSystemCategory GetInitialized(cCheckEngine ACheckEngine, cMonitorPlan AMonitorPlanProcess)
    {
      cSystemCategory Category;
      string ErrorMessage = "";

      try
      {
        Category = new cSystemCategory(ACheckEngine, AMonitorPlanProcess);

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

    public new bool ProcessChecks(string AMonSysId, string AMonitorLocationId)
    {
      FMonSysId = AMonSysId;
      CurrentRowId = FMonSysId;

      System.Diagnostics.Debug.WriteLine(string.Format("{0}: {1}", Label, CurrentRowId));

      return base.ProcessChecks(AMonitorLocationId);
    }

    #endregion


    #region Base Class Overrides

    protected override void FilterData()
    {
      //Set Current_System
      SetCheckParameter("Current_System",
                        new DataView(FMonitorPlanProcess.SourceData.Tables["MonitorSystem"],
                                     "Mon_Sys_Id = '" + FMonSysId + "'",
                                     "",
                                     DataViewRowState.CurrentRows)[0],
                        eParameterDataType.DataRowView);


      //I do not believe this parameter is used (djw2)
      ////Component_Records_By_Location
      //SetDataViewCheckParameter("Component_Records_By_Location",
      //                          FMonitorPlanProcess.SourceData.Tables["Component"],
      //                          "Mon_Loc_Id = '" + CurrentMonLocId + "'",
      //                          "Component_Id");

      //Set Method_to_System_Type_Cross_Check_Table
      SetDataViewCheckParameter("Method_to_System_Type_Cross_Check_Table",
                                FMonitorPlanProcess.SourceData.Tables["CrossCheck_MethodParameterToMethodToSystemType"],
                                "",
                                "SystemTypeCode,MethodCode,MethodParameterCode");

      //Set System_Fuel_Flow_Records
      SetDataViewCheckParameter("System_Fuel_Flow_Records",
                                FMonitorPlanProcess.SourceData.Tables["SystemFuelFlow"],
                                "Mon_Sys_Id = '" + FMonSysId + "'",
                                "Begin_Date,Begin_Hour,End_Date,End_Hour");

      //Set System_Analyzer_Range_Records
      SetDataViewCheckParameter("System_Analyzer_Range_Records",
                                FMonitorPlanProcess.SourceData.Tables["SystemAnalyzerRange"],
                                "Mon_Sys_Id = '" + FMonSysId + "'",
                                "Begin_Date,Begin_Hour,End_Date,End_Hour");

      //Set System_System_Component_Records
      SetDataViewCheckParameter("System_System_Component_Records",
                                FMonitorPlanProcess.SourceData.Tables["MonitorSystemComponent"],
                                "Mon_Sys_Id = '" + FMonSysId + "'",
                                "Component_id");

      //Set System_Designation_Code_Lookup_Table
      SetDataViewCheckParameter("System_Designation_Code_Lookup_Table",
                                FMonitorPlanProcess.SourceData.Tables["SystemDesignationCode"],
                                "",
                                "Sys_Designation_Cd");

      //Set System_Type_Lookup_Table
      SetDataViewCheckParameter("System_Type_Lookup_Table",
                                FMonitorPlanProcess.SourceData.Tables["SystemTypeCode"],
                                "",
                                "Sys_Type_Cd");

      //Set System_Type_To_Component_Type_Cross_Check_Table
      SetDataViewCheckParameter("System_Type_To_Component_Type_Cross_Check_Table",
                                FMonitorPlanProcess.SourceData.Tables["CrossCheck_SystemTypeToComponentType"],
                                "",
                                "SystemTypeCode,ComponentTypeCode");

      //Set System_Type_To_Fuel_Group_Cross_Check_Table
      SetDataViewCheckParameter("System_Type_To_Fuel_Group_Cross_Check_Table",
                                FMonitorPlanProcess.SourceData.Tables["CrossCheck_SystemTypeToFuelGroup"],
                                "",
                                "SystemTypeCode,FuelGroupCode");

    }

    protected override void SetRecordIdentifier()
    {
      DataRowView CurrentSystem = (DataRowView)GetCheckParameter("Current_System").ParameterValue;

      RecordIdentifier = "System ID " + (string)CurrentSystem["System_Identifier"];
    }

    protected override bool SetErrorSuppressValues()
    {
      DataRowView currentLocation = GetCheckParameter("Current_Location").ValueAsDataRowView();
      DataRowView currentSystem = GetCheckParameter("Current_System").ValueAsDataRowView();

      if (currentSystem != null)
      {
        long facId = CheckEngine.FacilityID;
        string locationName = currentLocation["LOCATION_IDENTIFIER"].AsString();
        string matchDataValue = currentSystem["SYS_TYPE_CD"].AsString();
        DateTime? matchTimeValue = currentSystem["END_DATE"].AsDateTime();

        ErrorSuppressValues = new cErrorSuppressValues(facId, locationName, "SYSTYP", matchDataValue, "HISTIND", matchTimeValue);
        return true;
      }
      else
        return false;
    }

    #endregion

  }
}
