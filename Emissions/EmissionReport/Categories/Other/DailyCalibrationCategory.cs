using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Em.Parameters;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Extensions;
using ECMPS.ErrorSuppression;


namespace ECMPS.Checks.EmissionsReport
{
  public class cDailyCalibrationCategory : cCategoryHourly
  {

    #region Constructors

    public cDailyCalibrationCategory(cCheckEngine checkEngine,
                                     cEmissionsReportProcess emissionsReportProcess,
                                     cOperatingHourCategory operatingHourCategory)
      : base("DAYCAL",
             (cEmissionsReportProcess)emissionsReportProcess,
             (cCategory)operatingHourCategory,
             "DailyCalibration")
    {
      //Initializing. Will be reset in FilterData()            
      ComponentID = "";
      CompId = "";
      SpanScale = "";
    }

    #endregion


    #region Private Fields

    string ComponentID;//COMPONENT_IDENTIFIER
    string CompId;//COMPONENT_ID
    //DateTime TestDate;
    string SpanScale;

    #endregion


    #region Base Class Overrides

    protected override void FilterData()
    {
      DataRowView DailyCalTest = GetCheckParameter("Current_Daily_Calibration_Test").ValueAsDataRowView();
      ComponentID = cDBConvert.ToString(DailyCalTest["COMPONENT_IDENTIFIER"]);
      CompId = cDBConvert.ToString(DailyCalTest["COMPONENT_ID"]);
      SpanScale = cDBConvert.ToString(DailyCalTest["SPAN_SCALE_CD"]);

      SetRecordIdentifier();

      SetCheckParameter("Analyzer_Range_Records_By_Component",
                  FilterView("AnalyzerRange", new cFilterCondition("COMPONENT_ID", CompId)),
                  eParameterDataType.DataView);

      SetCheckParameter("Hourly_Operating_Data_Records_for_Configuration", new DataView(SourceTables()["HourlyOperatingData"], "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("System_Component_Records", new DataView(SourceTables()["MonitorSystemComponent"],
          "COMPONENT_ID = '" + CompId + "'", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Span_Records", SourceTables()["MonitorSpan"].DefaultView, eParameterDataType.DataView);

      SetDataViewCheckParameter("Test_Tolerances_Cross_Check_Table", SourceTables()["CrossCheck_TestTolerances"],
          "", "");
    }

    protected override int[] GetDataBorderModcList()
    {
      return null;
    }

    protected override int[] GetQualityAssuranceHoursModcList()
    {
      return null;
    }

    protected override void SetRecordIdentifier()
    {
      if (EmParameters.CurrentDailyCalibrationTest != null)
        RecordIdentifier = string.Format("Location {0}, Component {1} and Type {2} at {3}",
                                         EmParameters.CurrentDailyCalibrationTest.LocationName,
                                         EmParameters.CurrentDailyCalibrationTest.ComponentIdentifier,
                                         EmParameters.CurrentDailyCalibrationTest.ComponentTypeCd,
                                         EmParameters.CurrentDailyCalibrationTest.DailyTestDatetime.AsStartDateTime());
      else
        RecordIdentifier = "this test";
    }

    protected override bool SetErrorSuppressValues()
    {
      DataRowView dailyCalTest = GetCheckParameter("Current_Daily_Calibration_Test").ValueAsDataRowView();

      if (dailyCalTest != null)
      {
        long facId = CheckEngine.FacilityID;
        string locationName = dailyCalTest["LOCATION_NAME"].AsString();
        string matchDataValue = dailyCalTest["COMPONENT_TYPE_CD"].AsString();
        DateTime? matchTimeValue = dailyCalTest.AsDateTime("DAILY_TEST_DATE", "DAILY_TEST_HOUR");

        ErrorSuppressValues = new cErrorSuppressValues(facId, locationName, "COMPTYP", matchDataValue, "HOUR", matchTimeValue);
        return true;
      }
      else
        return false;
    }

    #endregion

  }
}
