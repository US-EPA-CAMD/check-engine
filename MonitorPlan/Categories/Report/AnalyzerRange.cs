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
  public class cAnalyzerRange : cCategory
  {

    #region Private Constants

    private const string Label = "Analyzer Range";

    #endregion


    #region Private Fields

    private string mAnalyzerRangeID;
    private cMonitorPlan mMonitorPlan;

    #endregion


    #region Constructors

    public cAnalyzerRange(cCheckEngine CheckEngine, cMonitorPlan MonitorPlan)
      : base(CheckEngine, (cProcess)MonitorPlan, "ANRANGE")
    {
      mMonitorPlan = MonitorPlan;
      TableName = "ANALYZER_RANGE";
    }

    #endregion


    #region Public Static Methods

    public static cAnalyzerRange GetInitialized(cCheckEngine ACheckEngine, cMonitorPlan AMonitorPlanProcess)
    {
      cAnalyzerRange Category;
      string ErrorMessage = "";

      try
      {
        Category = new cAnalyzerRange(ACheckEngine, AMonitorPlanProcess);

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

    public new bool ProcessChecks(string AnalyzerRangeID, string MonitorLocationID)
    {
      mAnalyzerRangeID = AnalyzerRangeID;
      CurrentRowId = mAnalyzerRangeID;

      System.Diagnostics.Debug.WriteLine(string.Format("{0}: {1}", Label, CurrentRowId));

      return base.ProcessChecks(MonitorLocationID);
    }

    #endregion


    #region Base Class Overrides

    protected override void FilterData()
    {
      //filter MonitorPlanAnalyzerRange to find this one based on the mAnalyzerRangeID
      SetCheckParameter("Current_Analyzer_Range", new DataView(mMonitorPlan.SourceData.Tables["AnalyzerRange"],
        "analyzer_range_id = '" + mAnalyzerRangeID + "'", "analyzer_range_id", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      //get AnalyzerRangeCode
      SetCheckParameter("Analyzer_Range_Code_Lookup_Table", new DataView(mMonitorPlan.SourceData.Tables["AnalyzerRangeCode"],
        "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);
    }


    protected override void SetRecordIdentifier()
    {
      DataRowView CurrentAnalyzerRange = (DataRowView)GetCheckParameter("Current_Analyzer_Range").ParameterValue;

      RecordIdentifier = "Component ID " + (string)CurrentAnalyzerRange["component_identifier"] +
        " Analyzer Range " + (string)CurrentAnalyzerRange["analyzer_range_Cd"];
    }

    protected override bool SetErrorSuppressValues()
    {
      DataRowView currentLocation = GetCheckParameter("Current_Location").ValueAsDataRowView();
      DataRowView analyzerRange = GetCheckParameter("Current_Analyzer_Range").ValueAsDataRowView();

      if (analyzerRange != null)
      {
        long facId = CheckEngine.FacilityID;
        string locationName = currentLocation["LOCATION_IDENTIFIER"].AsString();
        string matchDataValue = analyzerRange["COMPONENT_TYPE_CD"].AsString();
        DateTime? matchTimeValue = analyzerRange["END_DATE"].AsDateTime();

        ErrorSuppressValues = new cErrorSuppressValues(facId, locationName, "COMPTYP", matchDataValue, "HISTIND", matchTimeValue);
        return true;
      }
      else
        return false;

    }


    #endregion

  }
}
