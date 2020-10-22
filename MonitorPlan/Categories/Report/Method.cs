using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Extensions;
using ECMPS.ErrorSuppression;


namespace ECMPS.Checks.MonitorPlanEvaluation
{
  public class cMethod : cCategory
  {

    #region Private Constants

    private const string Label = "Method";

    #endregion


    #region Private Fields

    private string mMonitorMethodId;
    private cMonitorPlan mMonitorPlan;

    #endregion


    #region Constructors

    public cMethod(cCheckEngine CheckEngine, cMonitorPlan MonitorPlan)
      : base(CheckEngine, (cProcess)MonitorPlan, "METHOD")
    {
      mMonitorPlan = MonitorPlan;
      TableName = "MONITOR_METHOD";
    }

    #endregion


    #region Public Static Methods

    public static cMethod GetInitialized(cCheckEngine ACheckEngine, cMonitorPlan AMonitorPlanProcess)
    {
      cMethod Category;
      string ErrorMessage = "";

      try
      {
        Category = new cMethod(ACheckEngine, AMonitorPlanProcess);

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

    public new bool ProcessChecks(string MonitorMethodId, string MonitorLocationID)
    {
      mMonitorMethodId = MonitorMethodId;
      CurrentRowId = mMonitorMethodId;

      System.Diagnostics.Debug.WriteLine(string.Format("{0}: {1}", Label, CurrentRowId));

      return base.ProcessChecks(MonitorLocationID);
    }

    #endregion


    #region Base Class Overrides

    protected override void FilterData()
    {
      string sMonMethodIDFilter = string.Format("mon_method_id = '{0}'", mMonitorMethodId);
      DataRowView drvCurrentMethod = new DataView(mMonitorPlan.SourceData.Tables["MonitorMethod"],
                                   sMonMethodIDFilter, "mon_method_id", DataViewRowState.CurrentRows)[0];

      //filter MonitorMethod to find this one based on the mMonitorMethodId
      SetCheckParameter("Current_Method", drvCurrentMethod, eParameterDataType.DataRowView);

      //Now set in Monitor Location Category (DJW2: 2006-12-13)
      //SetCheckParameter("facility_method_records",
      //                  new DataView(mMonitorPlan.SourceData.Tables["MonitorMethod"],
      //                               "", "", DataViewRowState.CurrentRows), ParameterTypes.DATAVW);

      //SetCheckParameter("Facility_Default_Records",
      //                    new DataView(mMonitorPlan.SourceData.Tables["FacilityDefault"], "", "", DataViewRowState.CurrentRows),
      //                    eParameterDataType.DataView); 

      SetCheckParameter("Facility_Location_Attribute_Records",
                        new DataView(mMonitorPlan.SourceData.Tables["LocationAttribute"],
                                     "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("QA_Supplemental_Data_Records",
                          new DataView(mMonitorPlan.SourceData.Tables["QASuppData"], "", "", DataViewRowState.CurrentRows),
                          eParameterDataType.DataView);

      SetCheckParameter("Method_Parameter_List",
                        new DataView(mMonitorPlan.SourceData.Tables["CrossCheck_ParameterToCategory"],
                                     "CategoryCode = 'METHOD'", "ParameterCode", DataViewRowState.CurrentRows),
                        eParameterDataType.DataView);

      SetCheckParameter("Parameter_to_Method_Cross_Check_Table",
                        new DataView(mMonitorPlan.SourceData.Tables["CrossCheck_MethodParameterToMethodToSystemType"],
                                     "", "MethodCode", DataViewRowState.CurrentRows),
                        eParameterDataType.DataView);

      SetCheckParameter("Method_To_Substitute_Data_Code_Cross_Check_Table",
                        new DataView(mMonitorPlan.SourceData.Tables["CrossCheck_MethodtoSubstituteDataCode"],
                                     "", "MethodCode", DataViewRowState.CurrentRows),
                        eParameterDataType.DataView);

      SetCheckParameter("Method_to_System_Type_Cross_Check_Table",
                new DataView(mMonitorPlan.SourceData.Tables["CrossCheck_MethodParametertoMethodtoSystemType"],
                             "SystemTypeCode is not null", "MethodCode", DataViewRowState.CurrentRows),
                eParameterDataType.DataView);

      SetCheckParameter("Method_Code_Lookup_Table", new DataView(mMonitorPlan.SourceData.Tables["MethodCode"]), eParameterDataType.DataView);

      SetCheckParameter("Bypass_Approach_Code_Lookup_Table", new DataView(mMonitorPlan.SourceData.Tables["BypassApproachCode"]), eParameterDataType.DataView);

      SetCheckParameter("Substitute_Data_Code_Lookup_Table", new DataView(mMonitorPlan.SourceData.Tables["SubstituteDataCode"]), eParameterDataType.DataView);


      SetCheckParameter("Method_Parameter_To_Maximum_Default_Parameter_Lookup_Table",
                         new DataView(mMonitorPlan.SourceData.Tables["CrossCheck_MethodParametertoMaximumDefaultParametertoComponentType"]), eParameterDataType.DataView);

      string sMonLocIDFilter = string.Format("MON_LOC_ID = '{0}'", drvCurrentMethod["MON_LOC_ID"].ToString());

    }


    protected override void SetRecordIdentifier()
    {
      DataRowView CurrentLocation = (DataRowView)GetCheckParameter("Current_Location").ParameterValue;
      DataRowView CurrentMethod = (DataRowView)GetCheckParameter("Current_Method").ParameterValue;

      if (CurrentMethod["BEGIN_DATE"] == DBNull.Value)
        RecordIdentifier = "ParameterCode " + cDBConvert.ToString(CurrentMethod["Parameter_Cd"]) + " MethodCode " + cDBConvert.ToString(CurrentMethod["Method_Cd"]) + " No BeginDate";
      else
        RecordIdentifier = "ParameterCode " + cDBConvert.ToString(CurrentMethod["Parameter_Cd"]) + " MethodCode " + cDBConvert.ToString(CurrentMethod["Method_Cd"]) + " BeginDate " + ((DateTime)CurrentMethod["BEGIN_DATE"]).ToShortDateString();
    }

    protected override bool SetErrorSuppressValues()
    {
      DataRowView currentLocation = (DataRowView)GetCheckParameter("Current_Location").ParameterValue;
      DataRowView currentMethod = (DataRowView)GetCheckParameter("Current_Method").ParameterValue;

      if (currentLocation != null && currentMethod != null)
      {
        long facId = CheckEngine.FacilityID;
        string locationName = currentLocation["LOCATION_IDENTIFIER"].AsString();
        string matchDataValue = currentMethod["PARAMETER_CD"].AsString();
        DateTime? matchTimeValue = currentMethod["END_DATE"].AsDateTime();

        ErrorSuppressValues = new cErrorSuppressValues(facId, locationName, "PARAM", matchDataValue, "HISTIND", matchTimeValue);
        return true;
      }
      else
        return false;
    }


    #endregion

  }
}
