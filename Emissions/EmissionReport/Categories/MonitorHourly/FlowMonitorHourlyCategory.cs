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
  /// Summary description for FlowMonitorHourlyCategory.
  /// </summary>
  public class cFlowMonitorHourlyCategory : cCategoryHourly
  {

    #region Constructors

    public cFlowMonitorHourlyCategory(cCheckEngine ACheckEngine,
                                      cEmissionsReportProcess AHourlyEmissionsData,
                                      cOperatingHourCategory AOperatingHourCategory)
      : base(AOperatingHourCategory,
             "FLOWMH",
             "MONITOR_HRLY_VALUE",
             AHourlyEmissionsData.MonitorHourlyValueFlow,
             "Flow_Monitor_Hourly_Value_Records_By_Hour_Location",
             "ADJUSTED_HRLY_VALUE",
             "MONITOR",
             "FLOW",
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
      int[] ModcList = new int[7];

      ModcList[0] = 1;
      ModcList[1] = 2;
      ModcList[2] = 3;
      ModcList[3] = 4;
      ModcList[4] = 20;
      ModcList[5] = 53;
      ModcList[6] = 54;

      return ModcList;
    }

    protected override int[] GetQualityAssuranceHoursModcList()
    {
      int[] ModcList = new int[5];

      ModcList[0] = 1;
      ModcList[1] = 2;
      ModcList[2] = 4;
      ModcList[3] = 20;
      ModcList[4] = 53;

      return ModcList;
    }

    protected override void SetRecordIdentifier()
    {
    }

    protected override bool SetErrorSuppressValues()
    {
      DataRowView currentLocation = GetCheckParameter("Current_Monitor_Plan_Location_Record").ValueAsDataRowView();
      DataRowView monitorHourlyRecord = GetCheckParameter("Current_Flow_Monitor_Hourly_Record").ValueAsDataRowView();

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
