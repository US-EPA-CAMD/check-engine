using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Extensions;
using ECMPS.ErrorSuppression;


namespace ECMPS.Checks.EmissionsReport
{
  public class cNoxcMonitorHourlyCategory : cCategoryHourly
  {

    #region Constructors

    public cNoxcMonitorHourlyCategory(cCheckEngine ACheckEngine,
                                      cEmissionsReportProcess AHourlyEmissionsData,
                                      cOperatingHourCategory AOperatingHourCategory)
      : base(AOperatingHourCategory,
             "NOXCMH",
             "MONITOR_HRLY_VALUE",
             AHourlyEmissionsData.MonitorHourlyValueNoxc,
             "NOxC_Monitor_Hourly_Value_Records_By_Hour_Location",
             "ADJUSTED_HRLY_VALUE",
             "MONITOR",
             "NOXC",
             null)
    {
    }

    #endregion


    #region Base Class Overrides

    protected override void FilterData()
    {
    }

    protected override int[] GetDataBorderModcList()
    {
      int[] ModcList = new int[11];

      ModcList[0] = 1;
      ModcList[1] = 2;
      ModcList[2] = 3;
      ModcList[3] = 4;
      ModcList[4] = 17;
      ModcList[5] = 19;
      ModcList[6] = 20;
      ModcList[7] = 21;
      ModcList[8] = 22;
      ModcList[9] = 53;
      ModcList[10] = 54;

      return ModcList;
    }

    protected override int[] GetQualityAssuranceHoursModcList()
    {

      int[] ModcList = new int[9];

      ModcList[0] = 1;
      ModcList[1] = 2;
      ModcList[2] = 4;
      ModcList[3] = 17;
      ModcList[4] = 19;
      ModcList[5] = 20;
      ModcList[6] = 21;
      ModcList[7] = 22;
      ModcList[8] = 53;

      return ModcList;
    }

    protected override void SetRecordIdentifier()
    {
    }

    protected override bool SetErrorSuppressValues()
    {
      DataRowView currentLocation = GetCheckParameter("Current_Monitor_Plan_Location_Record").ValueAsDataRowView();
      DataRowView monitorHourlyRecord = GetCheckParameter("Current_Nox_Conc_Monitor_Hourly_Record").ValueAsDataRowView();

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
