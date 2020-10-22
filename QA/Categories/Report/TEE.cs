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


namespace ECMPS.Checks.OtherQAEvaluation
{
  public class cTEE : cCategory
  {

    #region Private Fields

    private string mMonitorLocationID;
    private cOtherQAMain mQA;
    private string mTEEID;
    private string mSystemID;

    //private long mUnitID;
    //private string mStackPipeID;

    #endregion


    #region Constructors

    public cTEE(cCheckEngine CheckEngine, cOtherQAMain QA, string MonitorLocationID, string TEEID)
      : base(CheckEngine, (cProcess)QA, "TEE")
    {
      InitializeCurrent(MonitorLocationID);

      mMonitorLocationID = MonitorLocationID;
      mQA = QA;
      mTEEID = TEEID;

      TableName = "TEST_EXTENSION_EXEMPTION";
      CurrentRowId = mTEEID;

      FilterData();

      SetRecordIdentifier();
    }


    #endregion

    public string TEEID
    {
      get
      {
        return mTEEID;
      }
    }

    #region Public Methods

    /// <summary>
    /// Sets the checkbands for this category to the passed check bands and then executes
    /// those checks.
    /// </summary>
    /// <param name="ACheckBands">The check bands to process.</param>
    /// <returns>True if the processing of check executed normally.</returns>
    public bool ProcessChecks(cCheckParameterBands ACheckBands)
    {
      this.SetCheckBands(ACheckBands);
      return base.ProcessChecks();
    }

    #endregion

    #region Base Class Overrides

    protected override void FilterData()
    {
      SetCheckParameter("Current_Test_Extension_Exemption", new DataView(mQA.SourceData.Tables["QATEE"],
          "TEST_EXTENSION_EXEMPTION_ID = '" + mTEEID + "'", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      mSystemID = cDBConvert.ToString(((DataRowView)GetCheckParameter("Current_Test_Extension_Exemption").ParameterValue)["mon_sys_id"]);

      // I do not believe that this parameter is used in this process. (djw2)
      //SetCheckParameter("Test_Extension_Exemption_Records", new DataView(mQA.SourceData.Tables["QATEE"],
      //    "mon_sys_id = '" + mSystemID + "'", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Monitor_System_Records", new DataView(mQA.SourceData.Tables["MonitorSystemRecords"],
          "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Component_Records", new DataView(mQA.SourceData.Tables["QAComponent"],
          "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      // I do not believe that this parameter is used in this process. (djw2)
      //SetCheckParameter("System_Records", new DataView(mQA.SourceData.Tables["QASystem"],
      //    "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Location_Analyzer_Range_Records", new DataView(mQA.SourceData.Tables["AnalyzerRangeRecords"],
          "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Location_Reporting_Frequency_Records", new DataView(mQA.SourceData.Tables["LocationReportingFrequency"],
          "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Location_System_Component_Records", new DataView(mQA.SourceData.Tables["LocationSystemComponent"],
          "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Reporting_Period_Lookup_Table", new DataView(mQA.SourceData.Tables["ReportingPeriodLookup"],
          "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Fuel_Code_Lookup_Table", new DataView(mQA.SourceData.Tables["FuelCodeLookup"],
          "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Monitoring_Plan_Location_Records_for_QA", new DataView(mQA.SourceData.Tables["MonitorPlanLocation"],
        "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

    }


    protected override void SetRecordIdentifier()
    {
      RecordIdentifier = "this extension/exemption";
    }


    protected override bool SetErrorSuppressValues()
    {
      DataRowView CurrentTEE = GetCheckParameter("Current_Test_Extension_Exemption").ValueAsDataRowView();

      if (CurrentTEE != null)
      {
        long facId = CheckEngine.FacilityID;
        string locationName = CurrentTEE["LOCATION_IDENTIFIER"].AsString();

        DateTime? matchTimeValue;
        {
          int? year = CurrentTEE["CALENDAR_YEAR"].AsInteger();
          int? quarter = CurrentTEE["QUARTER"].AsInteger();

          if (year.HasValue && quarter.HasValue)
            matchTimeValue = new DateTime(year.Value, 3 * (quarter.Value - 1) + 1, 1);
          else
            matchTimeValue = null;
        }

        ErrorSuppressValues = new cErrorSuppressValues(facId, locationName, null, null, "QUARTER", matchTimeValue);
        return true;
      }
      else
      {
        ErrorSuppressValues = null;
        return false;
      }
    }

    #endregion

  }
}