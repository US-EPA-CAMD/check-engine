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
  public class cComponent : cCategory
  {

    #region Private Constants

    private const string Label = "Component";

    #endregion


    #region Private Fields

    private string mComponentID;
    private cMonitorPlan mMonitorPlan;

    #endregion


    #region Constructors

    public cComponent(cCheckEngine CheckEngine, cMonitorPlan MonitorPlan)
      : base(CheckEngine, (cProcess)MonitorPlan, "COMP")
    {
      mMonitorPlan = MonitorPlan;
      TableName = "COMPONENT";
    }

    #endregion


    #region Public Static Methods

    public static cComponent GetInitialized(cCheckEngine ACheckEngine, cMonitorPlan AMonitorPlanProcess)
    {
      cComponent Category;
      string ErrorMessage = "";

      try
      {
        Category = new cComponent(ACheckEngine, AMonitorPlanProcess);

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

    public new bool ProcessChecks(string ComponentID, string MonitorLocationID)
    {
      mComponentID = ComponentID;
      CurrentRowId = mComponentID;

      System.Diagnostics.Debug.WriteLine(string.Format("{0}: {1}", Label, CurrentRowId));

      return base.ProcessChecks(MonitorLocationID);
    }

    #endregion


    #region Base Class Overrides

    protected override void FilterData()
    {
      //filter MonitorPlanComponent to find this one based on the mComponentID
      SetCheckParameter("Current_component", new DataView(mMonitorPlan.SourceData.Tables["Component"],
        "Component_id = '" + mComponentID + "'", "Component_id", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      //filter MonitorPlanSystemComponent to find this one based on the mComponentID
      SetDataViewCheckParameter("System_Component_Records",
        mMonitorPlan.SourceData.Tables["MonitorSystemComponent"],
        "Component_id = '" + mComponentID + "'", "Component_id");

      //filter MonitorPlanAnalyzerRange to find this one based on the mComponentID
      SetDataViewCheckParameter("Component_Analyzer_Range_Records",
        mMonitorPlan.SourceData.Tables["AnalyzerRange"],
        "Component_id = '" + mComponentID + "'", "Component_id");

      //get SampleAcquisitionMethodCode
      SetCheckParameter("sample_acquisition_method_code_lookup_table", new DataView(mMonitorPlan.SourceData.Tables["SampleAcquisitionMethodCode"],
        "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      //get ComponentTypeAndBasisToSampleAcquisitionMethodCrossCheckTable
      SetCheckParameter("Component_Type_And_Basis_To_Sample_Acquisition_Method_Cross_Check_Table", new DataView(mMonitorPlan.SourceData.Tables["CrossCheck_ComponentTypeAndBasisToSampleAcquisitionMethod"],
        "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);
    }


    protected override void SetRecordIdentifier()
    {
      DataRowView CurrentComponent = (DataRowView)GetCheckParameter("Current_Component").ParameterValue;

      RecordIdentifier = "Component ID " + (string)CurrentComponent["Component_identifier"];
    }

    protected override bool SetErrorSuppressValues()
    {
      DataRowView currentLocation = GetCheckParameter("Current_Location").ValueAsDataRowView();
      DataRowView currentComponent = (DataRowView)GetCheckParameter("Current_Component").ParameterValue;

      if (currentComponent != null)
      {
        long facId = CheckEngine.FacilityID;
        string locationName = currentLocation["LOCATION_IDENTIFIER"].AsString();
        string matchDataValue = currentComponent["COMPONENT_TYPE_CD"].AsString();

        ErrorSuppressValues = new cErrorSuppressValues(facId, locationName, "COMPTYP", matchDataValue, null, null);
        return true;
      }
      else
        return false;
    }
    #endregion

  }
}