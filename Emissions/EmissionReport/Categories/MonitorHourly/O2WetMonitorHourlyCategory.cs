using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Extensions;
using ECMPS.ErrorSuppression;


namespace ECMPS.Checks.EmissionsReport
{
  public class cO2WetMonitorHourlyCategory : cCategoryHourly
  {

    #region Constructors

    public cO2WetMonitorHourlyCategory(cCheckEngine ACheckEngine,
                                       cEmissionsReportProcess AHourlyEmissionsData,
                                       cOperatingHourCategory AOperatingHourCategory)
      : base(AOperatingHourCategory,
             "O2WMH",
             "MONITOR_HRLY_VALUE",
             AHourlyEmissionsData.MonitorHourlyValueO2Wet,
             "O2_Wet_Monitor_Hourly_Value_Records_By_Hour_Location",
             "UNADJUSTED_HRLY_VALUE",
             "MONITOR",
             "O2C",
             "W")
    {
      EmissionsReportProcess = AHourlyEmissionsData;

      TableName = "";

      SetRecordIdentifier();
    }


    #endregion


    #region Base Class Overrides

    protected override void FilterData()
    {
    }

    protected override int[] GetDataBorderModcList()
    {
      int[] ModcList = new int[8];

      ModcList[0] = 1;
      ModcList[1] = 2;
      ModcList[2] = 3;
      ModcList[3] = 4;
      ModcList[4] = 17;
      ModcList[5] = 20;
      ModcList[6] = 53;
      ModcList[7] = 54;

      return ModcList;
    }

    protected override int[] GetQualityAssuranceHoursModcList()
    {
      int[] ModcList = new int[6];

      ModcList[0] = 1;
      ModcList[1] = 2;
      ModcList[2] = 4;
      ModcList[3] = 17;
      ModcList[4] = 20;
      ModcList[5] = 53;

      return ModcList;
    }

    protected override void SetRecordIdentifier()
    {
    }

    protected override bool SetErrorSuppressValues()
    {
      DataRowView currentLocation = GetCheckParameter("Current_Monitor_Plan_Location_Record").ValueAsDataRowView();
      DataRowView monitorHourlyRecord = GetCheckParameter("Current_O2_Wet_Monitor_Hourly_Record").ValueAsDataRowView();

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
