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
  public class cSpanCategory : cCategory
  {

    #region Private Constants

    private const string Label = "Span";

    #endregion


    #region Constructors

    public cSpanCategory(cCheckEngine ACheckEngine, cMonitorPlan AMonitorPlanProcess)
      : base(ACheckEngine, (cProcess)AMonitorPlanProcess, "SPAN")
    {
      FMonitorPlanProcess = AMonitorPlanProcess;
      TableName = "MONITOR_SPAN";
    }

    #endregion


    #region Public Static Methods

    public static cSpanCategory GetInitialized(cCheckEngine ACheckEngine, cMonitorPlan AMonitorPlanProcess)
    {
      cSpanCategory Category;
      string ErrorMessage = "";

      try
      {
        Category = new cSpanCategory(ACheckEngine, AMonitorPlanProcess);

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

    private string FSpanId;
    private cMonitorPlan FMonitorPlanProcess;

    #endregion


    #region Public Methods

    public new bool ProcessChecks(string ASpanId, string AMonitorLocationId)
    {
      FSpanId = ASpanId;
      CurrentRowId = FSpanId;

      System.Diagnostics.Debug.WriteLine(string.Format("{0}: {1}", Label, CurrentRowId));

      return base.ProcessChecks(AMonitorLocationId);
    }

    #endregion


    #region Base Class Overrides

    protected override void FilterData()
    {
      //Set Current_Span
      SetDataRowCheckParameter("Current_Span",
                               FMonitorPlanProcess.SourceData.Tables["MonitorSpan"],
                               "Span_Id = '" + FSpanId + "'", "");


      //Set Component_Type_And_Span_Scale_To_Span_Method_Cross_Check_Table
      SetDataViewCheckParameter("Component_Type_And_Span_Scale_To_Span_Method_Cross_Check_Table",
                                FMonitorPlanProcess.SourceData.Tables["CrossCheck_ComponentTypeAndSpanScaleToSpanMethod"],
                                "", "");

      //Set NOX_MPC_To_Fuel_Category_and_Unit_Type
      SetDataViewCheckParameter("NOX_MPC_To_Fuel_Category_and_Unit_Type",
                                FMonitorPlanProcess.SourceData.Tables["CrossCheck_NoxMpcToFuelCategoryAndUnitType"],
                                "", "");

      //Set Span_Method_Code_Lookup_Table
      SetDataViewCheckParameter("Span_Method_Code_Lookup_Table",
                                FMonitorPlanProcess.SourceData.Tables["SpanMethodCode"],
                                "", "");

    }

    protected override void SetRecordIdentifier()
    {
      DataRowView CurrentSpan = (DataRowView)GetCheckParameter("Current_Span").ParameterValue;
      string Delim = "";

      RecordIdentifier = "";

      if (CurrentSpan["Component_Type_Cd"] != DBNull.Value)
      {
        RecordIdentifier += Delim + (string)CurrentSpan["Component_Type_Cd"];
        Delim = " ";
      }

      if (CurrentSpan["Span_Scale_Cd"] != DBNull.Value)
      {
        RecordIdentifier += Delim + (string)CurrentSpan["Span_Scale_Cd"];
        Delim = " ";
      }

      if (CurrentSpan["Begin_Date"] != DBNull.Value)
      {
        RecordIdentifier += Delim + ((DateTime)CurrentSpan["Begin_Date"]).ToShortDateString();
        Delim = " ";
      }
    }

    protected override bool SetErrorSuppressValues()
    {
      DataRowView currentLocation = GetCheckParameter("Current_Location").ValueAsDataRowView();
      DataRowView currentSpan = GetCheckParameter("Current_Span").ValueAsDataRowView();

      if (currentSpan != null)
      {
        long facId = CheckEngine.FacilityID;
        string locationName = currentLocation["LOCATION_IDENTIFIER"].AsString();
        string matchDataValue = currentSpan["COMPONENT_TYPE_CD"].AsString();
        DateTime? matchTimeValue = currentSpan["END_DATE"].AsDateTime();

        ErrorSuppressValues = new cErrorSuppressValues(facId, locationName, "COMPTYP", matchDataValue, "HISTIND", matchTimeValue);
        return true;
      }
      else
        return false;
    }

    #endregion

  }
}
