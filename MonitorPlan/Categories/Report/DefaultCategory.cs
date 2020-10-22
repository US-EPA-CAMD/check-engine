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
  public class cDefaultCategory : cCategory
  {

    #region Private Constants

    private const string Label = "Default";

    #endregion


    #region Private Fields

    private string mMondefId;
    private cMonitorPlan mMonitorPlan;

    #endregion


    #region Constructors

    public cDefaultCategory(cCheckEngine CheckEngine, cMonitorPlan MonitorPlan)
      : base(CheckEngine, (cProcess)MonitorPlan, "DEFAULT")
    {
      mMonitorPlan = MonitorPlan;
      TableName = "MONITOR_DEFAULT";
    }

    #endregion


    #region Public Static Methods

    public static cDefaultCategory GetInitialized(cCheckEngine ACheckEngine, cMonitorPlan AMonitorPlanProcess)
    {
      cDefaultCategory Category;
      string ErrorMessage = "";

      try
      {
        Category = new cDefaultCategory(ACheckEngine, AMonitorPlanProcess);

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

    public new bool ProcessChecks(string MondefId, string MonitorLocationID)
    {
      mMondefId = MondefId;
      CurrentRowId = mMondefId;

      System.Diagnostics.Debug.WriteLine(string.Format("{0}: {1}", Label, CurrentRowId));

      return base.ProcessChecks(MonitorLocationID);
    }

    #endregion


    #region Base Class Overrides

    protected override void FilterData()
    {

      //Process.ElapsedTimes.TimingBegin("FilterData", this);

      //filter MonitorDefault to find this one based on the mMondefId
      SetDataRowCheckParameter("Current_Default", mMonitorPlan.SourceData.Tables["MonitorDefault"],
        "mondef_id = '" + mMondefId + "'", "mondef_id");

      //Set Location_System_Fuel_Flow_Records
      SetDataViewCheckParameter("Location_System_Fuel_Flow_Records", mMonitorPlan.SourceData.Tables["SystemFuelFlow"],
                                  "mon_loc_id = '" + CurrentMonLocId + "'", "");


      //lookup tables
      SetDataViewCheckParameter("Default_Purpose_Code_Lookup_Table", mMonitorPlan.SourceData.Tables["DefaultPurposeCode"], "", "");

      SetDataViewCheckParameter("Default_Source_Code_Lookup_Table", mMonitorPlan.SourceData.Tables["DefaultSourceCode"], "", "");

      //cross check tables   
      SetDataViewCheckParameter("Default_Parameter_List", mMonitorPlan.SourceData.Tables["CrossCheck_ParameterToCategory"], "CategoryCode = 'DEFAULT'", "");

      SetDataViewCheckParameter("Default_Parameter_To_Purpose_Cross_Check_Table",
                                  mMonitorPlan.SourceData.Tables["CrossCheck_DefaultParametertoPurpose"], "", "");

      SetDataViewCheckParameter("Default_Parameter_To_Source_Of_Value_Cross_Check_Table",
                                  mMonitorPlan.SourceData.Tables["CrossCheck_DefaultParametertoSourceofValue"], "", "");

      SetDataViewCheckParameter("Fuel_Code_To_Minimum_And_Maximum_Moisture_Default_Cross_Check_Table",
                                  mMonitorPlan.SourceData.Tables["CrossCheck_FuelCodetoMinimumandMaximumMoistureDefaultValue"], "", "");

      SetDataViewCheckParameter("Default_Parameter,_Boiler_Type,_And_Fuel_Type_To_Default_Value_Cross_Check_Table",
                                  mMonitorPlan.SourceData.Tables["CrossCheck_DefaultParameter,BoilerType,andFuelTypetoDefaultValue"], "", "");


      //Process.ElapsedTimes.TimingEnd("FilterData", this);
    }

    protected override void SetRecordIdentifier()
    {
      DataRowView CurrentDefault = (DataRowView)GetCheckParameter("Current_Default").ParameterValue;

      if (CurrentDefault["BEGIN_DATE"] == DBNull.Value)
        RecordIdentifier = "ParameterCode " + cDBConvert.ToString(CurrentDefault["Parameter_Cd"]) + " FuelCode " + cDBConvert.ToString(CurrentDefault["Fuel_Cd"]) +
            " OperatingConditionCode " + cDBConvert.ToString(CurrentDefault["operating_condition_cd"]) + " No BeginDate";
      else
        RecordIdentifier = "ParameterCode " + cDBConvert.ToString(CurrentDefault["Parameter_Cd"]) + " FuelCode " + cDBConvert.ToString(CurrentDefault["Fuel_Cd"]) +
            " OperatingConditionCode " + cDBConvert.ToString(CurrentDefault["operating_condition_cd"]) + " BeginDate " + ((DateTime)CurrentDefault["BEGIN_DATE"]).ToShortDateString();
    }

    protected override bool SetErrorSuppressValues()
    {
      DataRowView currentLocation = GetCheckParameter("Current_Location").ValueAsDataRowView();
      DataRowView currentDefault = GetCheckParameter("Current_Default").ValueAsDataRowView();

      if (currentDefault != null)
      {
        long facId = CheckEngine.FacilityID;
        string locationName = currentLocation["LOCATION_IDENTIFIER"].AsString();
        string matchDataValue = currentDefault["PARAMETER_CD"].AsString();
        DateTime? matchTimeValue = currentDefault["END_DATE"].AsDateTime();

        ErrorSuppressValues = new cErrorSuppressValues(facId, locationName, "PARAM", matchDataValue, "HISTIND", matchTimeValue);
        return true;
      }
      else
        return false;
    }

    #endregion
  }

}
