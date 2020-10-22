using System;
using System.Data;
using System.Collections;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Extensions;
using ECMPS.ErrorSuppression;


namespace ECMPS.Checks.OtherQAEvaluation
{
  public class cCertEvent : cCategory
  {

    #region Private Fields

    private string mMonitorLocationID;
    private cOtherQAMain mQA;
    private string mCertEventID;
    private string mSystemID;
    private string mComponentID;

    #endregion


    #region Constructors

    public cCertEvent(cCheckEngine CheckEngine, cOtherQAMain QA, string MonitorLocationID, string CertEventID)
      : base(CheckEngine, (cProcess)QA, "EVENT")
    {
      InitializeCurrent(MonitorLocationID);

      mMonitorLocationID = MonitorLocationID;
      mQA = QA;
      mCertEventID = CertEventID;

      TableName = "QA_CERT_EVENT";
      CurrentRowId = mCertEventID;

      FilterData();

      SetRecordIdentifier();
    }


    #endregion

    public string CertEventID
    {
      get
      {
        return mCertEventID;
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
      SetCheckParameter("Current_QA_Cert_Event", new DataView(mQA.SourceData.Tables["QACertEvent"],
          "QA_CERT_EVENT_ID = '" + mCertEventID + "'", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      mSystemID = cDBConvert.ToString(((DataRowView)GetCheckParameter("Current_QA_Cert_Event").ParameterValue)["mon_sys_id"]);

      mComponentID = cDBConvert.ToString(((DataRowView)GetCheckParameter("Current_QA_Cert_Event").ParameterValue)["component_id"]);

      // I do not believe that this parameter is used in this process (djw2)
      //SetCheckParameter("Qa_Certification_Event_Records", new DataView(mQA.SourceData.Tables["QACertEvent"],
      //    "mon_sys_id = '" + mSystemID + "'", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Analyzer_Range_Records", new DataView(mQA.SourceData.Tables["QAAnalyzerRange"],
          "component_id = '" + mComponentID + "'", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Monitor_System_Records", new DataView(mQA.SourceData.Tables["MonitorSystemRecords"],
          "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Component_Records", new DataView(mQA.SourceData.Tables["QAComponent"],
          "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Location_System_Component_Records", new DataView(mQA.SourceData.Tables["QASystemComponent"],
          "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Location_Control_Records", new DataView(mQA.SourceData.Tables["LocationControl"],
          "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Location_Operating_Status_Records", new DataView(mQA.SourceData.Tables["LocationOperatingStatus"],
          "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Location_Reporting_Frequency_Records", new DataView(mQA.SourceData.Tables["LocationReportingFrequency"],
        "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Facility_Qualification_Records", new DataView(mQA.SourceData.Tables["FacilityQualification"],
          "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Unit_Stack_Configuration_Records", new DataView(mQA.SourceData.Tables["QAUnitStackConfiguration"],
          "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Required_Test_Code_to_Required_ID_and_System_or_Component_Type_Cross_Check_Table", new DataView(mQA.SourceData.Tables["CrossCheck_RequiredTestCodetoRequiredIDandSystemorComponentType"],
        "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Event_Code_to_System_or_Component_Type_Cross_Check_Table", new DataView(mQA.SourceData.Tables["CrossCheck_EventCodetoSystemorComponentType"],
        "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Event_Code_to_Test_Type_Codes_Cross_Check_Table", new DataView(mQA.SourceData.Tables["CrossCheck_EventCodetoTestTypeCodes"],
          "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Test_Type_to_Required_Test_Code_Cross_Check_Table", new DataView(mQA.SourceData.Tables["CrossCheck_TestTypetoRequiredTestCode"],
        "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Reporting_Period_Lookup_Table", new DataView(mQA.SourceData.Tables["ReportingPeriodLookup"],
        "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Span_Records", new DataView(mQA.SourceData.Tables["QASpan"],
        "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Monitoring_Plan_Location_Records_for_QA", new DataView(mQA.SourceData.Tables["MonitorPlanLocation"],
        "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);
    }

    protected override void SetRecordIdentifier()
    {
      RecordIdentifier = "this event";
    }

    protected override bool SetErrorSuppressValues()
    {
        DataRowView CertEventRecord = GetCheckParameter("Current_QA_Cert_Event").ValueAsDataRowView();

        if (CertEventRecord != null)
        {
          long facId = CheckEngine.FacilityID;
          string locationName = CertEventRecord["LOCATION_IDENTIFIER"].AsString();
          DateTime? matchTimeValue = CertEventRecord["QA_CERT_EVENT_DATE"].AsDateTime();

          ErrorSuppressValues = new cErrorSuppressValues(facId, locationName, null, null, "DATE", matchTimeValue);
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