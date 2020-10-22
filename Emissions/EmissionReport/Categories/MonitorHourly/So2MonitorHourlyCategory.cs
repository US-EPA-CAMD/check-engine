using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Extensions;
using ECMPS.ErrorSuppression;


namespace ECMPS.Checks.EmissionsReport
{
  /// <summary>
  /// Summary description for So2MonitorHourlyCategory.
  /// </summary>
  public class cSo2MonitorHourlyCategory : cCategoryHourly
  {

    #region Constructors

    public cSo2MonitorHourlyCategory(cCheckEngine ACheckEngine,
                                     cEmissionsReportProcess AHourlyEmissionsData,
                                     cOperatingHourCategory AOperatingHourCategory)
      : base(AOperatingHourCategory,
             "SO2MH",
             "MONITOR_HRLY_VALUE",
             AHourlyEmissionsData.MonitorHourlyValueSo2c,
             "SO2_Monitor_Hourly_Value_Records_By_Hour_Location",
             "ADJUSTED_HRLY_VALUE",
             "MONITOR",
             "SO2C",
             null)
    {
    }

    #endregion


    #region Base Class Overrides

    protected override void FilterData()
    {
      SetCheckParameter("Monitor_Span_Records_By_Hour_Location",
                        FilterRanged("MonitorSpan", MonitorSpan, CurrentOpDate, CurrentOpHour, CurrentMonLocId),
                        eParameterDataType.DataView);

      SetCheckParameter("Monitor_System_Component_Records_By_Hour_Location",
                    FilterRanged("MonitorSystemComponent", MonitorSystemComponent, CurrentOpDate, CurrentOpHour, CurrentMonLocId),
                    eParameterDataType.DataView);
    }

    protected override int[] GetDataBorderModcList()
    {
      int[] ModcList = new int[12];

      ModcList[0] = 1;
      ModcList[1] = 2;
      ModcList[2] = 3;
      ModcList[3] = 4;
      ModcList[4] = 16;
      ModcList[5] = 17;
      ModcList[6] = 19;
      ModcList[7] = 20;
      ModcList[8] = 21;
      ModcList[9] = 22;
      ModcList[10] = 53;
      ModcList[11] = 54;

      return ModcList;
    }

    protected override int[] GetQualityAssuranceHoursModcList()
    {
      int[] ModcList = new int[10];

      ModcList[0] = 1;
      ModcList[1] = 2;
      ModcList[2] = 4;
      ModcList[3] = 16;
      ModcList[4] = 17;
      ModcList[5] = 19;
      ModcList[6] = 20;
      ModcList[7] = 21;
      ModcList[8] = 22;
      ModcList[9] = 53;

      return ModcList;
    }

    protected override void SetRecordIdentifier()
    {
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
