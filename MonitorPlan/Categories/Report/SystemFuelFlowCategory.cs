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
  public class cSystemFuelFlowCategory : cCategory
  {

    #region Private Constants

    private const string Label = "System Fuel Flow Category";

    #endregion


    #region Constructors

    public cSystemFuelFlowCategory(cCheckEngine ACheckEngine, cMonitorPlan AMonitorPlanProcess)
      : base(ACheckEngine, (cProcess)AMonitorPlanProcess, "FUELFLW")
    {
      FMonitorPlanProcess = AMonitorPlanProcess;
      TableName = "SYSTEM_FUEL_FLOW";
    }

    #endregion


    #region Public Static Methods

    public static cSystemFuelFlowCategory GetInitialized(cCheckEngine ACheckEngine, cMonitorPlan AMonitorPlanProcess)
    {
      cSystemFuelFlowCategory Category;
      string ErrorMessage = "";

      try
      {
        Category = new cSystemFuelFlowCategory(ACheckEngine, AMonitorPlanProcess);

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


    #region Private Fields

    private string FSysFuelId;
    private string FMonSysId;
    private cMonitorPlan FMonitorPlanProcess;

    #endregion


    #region Public Methods

    public bool ProcessChecks(string ASysFuelId, string AMonSysId, string AMonitorLocationId)
    {
      FSysFuelId = ASysFuelId;
      FMonSysId = AMonSysId;
      CurrentRowId = ASysFuelId;

      System.Diagnostics.Debug.WriteLine(string.Format("{0}: {1}", Label, CurrentRowId));

      return base.ProcessChecks(AMonitorLocationId);
    }

    #endregion


    #region Base Class Overrides

    protected override void FilterData()
    {
      //Set Current_Fuel_Flow
      SetCheckParameter("Current_Fuel_Flow",
                        new DataView(FMonitorPlanProcess.SourceData.Tables["SystemFuelFlow"],
                                     "Sys_Fuel_Id = '" + FSysFuelId + "'",
                                     "",
                                     DataViewRowState.CurrentRows)[0],
                        eParameterDataType.DataRowView);


      //Fuel_Flow_Maximum_Rate_Source_Code_Lookup_Table
      SetDataViewCheckParameter("Fuel_Flow_Maximum_Rate_Source_Code_Lookup_Table",
                                FMonitorPlanProcess.SourceData.Tables["MaxRateSourceCode"],
                                "", "");

      //Fuel_Flow_Records
      SetDataViewCheckParameter("Fuel_Flow_Records",
                                FMonitorPlanProcess.SourceData.Tables["SystemFuelFlow"],
                                "Mon_Sys_Id = '" + FMonSysId + "'", "");

    }

    protected override void SetRecordIdentifier()
    {
      DataRowView CurrentFuelFlow = (DataRowView)GetCheckParameter("Current_Fuel_Flow").ParameterValue;

      string SystemIdentifier = cDBConvert.ToString(CurrentFuelFlow["System_Identifier"]);
      string FuelCd = cDBConvert.ToString(CurrentFuelFlow["Fuel_Cd"]);
      string BeginDate = (CurrentFuelFlow["Begin_Date"] != DBNull.Value
                       ? cDBConvert.ToDate(CurrentFuelFlow["Begin_Date"], DateTypes.START).ToShortDateString()
                       : "");

      string FuelFlowIdentifier = SystemIdentifier + (FuelCd != "" ? "-" + FuelCd : "")
                                                   + (BeginDate != "" ? "-" + BeginDate : "");

      RecordIdentifier = "System Fuel Flow " + FuelFlowIdentifier;
    }

    protected override bool SetErrorSuppressValues()
    {
      DataRowView currentLocation = GetCheckParameter("Current_Location").ValueAsDataRowView();
      DataRowView currentSystem = GetCheckParameter("Current_System").ValueAsDataRowView();
      DataRowView currentFuelFlow = GetCheckParameter("Current_Fuel_Flow").ValueAsDataRowView();

      if (currentFuelFlow != null && currentSystem != null && currentLocation != null)
      {
        long facId = CheckEngine.FacilityID;
        string locationName = currentLocation["LOCATION_IDENTIFIER"].AsString();
        string matchDataValue = currentSystem["SYS_TYPE_CD"].AsString();
        DateTime? matchTimeValue = currentFuelFlow["END_DATE"].AsDateTime();

        ErrorSuppressValues = new cErrorSuppressValues(facId, locationName, "SYSTYP", matchDataValue, "HISTIND", matchTimeValue);
        return true;
      }
      else
        return false;
    }

    #endregion

  }
}
