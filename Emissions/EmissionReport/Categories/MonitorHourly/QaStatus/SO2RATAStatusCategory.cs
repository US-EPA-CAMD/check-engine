using System;
using System.Data;
using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Extensions;
using ECMPS.ErrorSuppression;


namespace ECMPS.Checks.EmissionsReport
{
  public class cSO2RATAStatusCategory : cCategoryHourly
  {
    #region Constructors

    public cSO2RATAStatusCategory(cCheckEngine ACheckEngine,
                             cEmissionsReportProcess AHourlyEmissionsData,
                             cCategory ACategory,
                             string ACategoryCode)
      : base(ACheckEngine,
       (cEmissionsReportProcess)AHourlyEmissionsData,
       ACategory,
       ACategoryCode)
    {
    }

    #endregion

    #region Base Class Overrides

    protected override void FilterData()
    {
      /*SetCheckParameter("RATA_Test_Records_By_Location_For_QA_Status",
                        FilterLocation("RATATestQAStatus", RATATestQAStatus, FCurrentMonLocId),
                        eParameterDataType.DataView);
      */
      DataView[] OperatingHoursByLocation = (DataView[])EmissionParameters.OperatingHoursByLocation.Value;

      SetCheckParameter("Hourly_Operating_Data_Records_for_Location",
                  OperatingHoursByLocation[CurrentMonLocPos],
                  eParameterDataType.DataView);

      SetCheckParameter("Location_Attribute_Records_By_Hour_Location",
                  FilterRanged("LocationAttribute", LocationAttribute, CurrentOpDate, CurrentOpHour, CurrentMonLocId, "Begin_Date", "End_Date"),
                  eParameterDataType.DataView);

      SetCheckParameter("Test_Extension_Exemption_Records",
                         FilterLocation("TEERecords", TEERecords, CurrentMonLocId),
                         eParameterDataType.DataView);
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
      DataRowView CurrentRATAStatRec = (DataRowView)GetCheckParameter("Current_Hourly_Record_for_RATA_Status").ParameterValue;
      if (CurrentRATAStatRec != null)
        RecordIdentifier = "Monitor System ID " + cDBConvert.ToString(CurrentRATAStatRec["SYSTEM_IDENTIFIER"]);
    }

    protected override bool SetErrorSuppressValues()
    {
      DataRowView currentLocation = GetCheckParameter("Current_Monitor_Plan_Location_Record").ValueAsDataRowView();
      DataRowView monitorHourlyRecord = GetCheckParameter("Current_SO2_Monitor_Hourly_Record").ValueAsDataRowView();

      if (currentLocation != null && monitorHourlyRecord != null)
      {
        long facId = CheckEngine.FacilityID;
        string locationName = currentLocation["LOCATION_NAME"].AsString();
        string matchDataValue = monitorHourlyRecord["PARAMETER_CD"].AsString();
        DateTime? matchTimeValue = monitorHourlyRecord.AsDateTime("BEGIN_DATE", "BEGIN_HOUR");

        ErrorSuppressValues = new cErrorSuppressValues(facId, locationName, "PARAM", matchDataValue, "HOUR", matchTimeValue);
        return true;
      }
      else
        return false;
    }

    #endregion
  }
}
