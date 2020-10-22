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
  /// Summary description for H20MonitorHourlyCategory.
  /// </summary>
  public class cH2oMonitorHourlyCategory : cCategoryHourly
  {

    #region Constructors

    public cH2oMonitorHourlyCategory(cCheckEngine ACheckEngine,
                                     cEmissionsReportProcess AHourlyEmissionsData,
                                     cOperatingHourCategory AOperatingHourCategory)
      : base("H2OMH",
             (cEmissionsReportProcess)AHourlyEmissionsData,
             (cCategory)AOperatingHourCategory,
             "MONITOR_HRLY_VALUE",
             AHourlyEmissionsData.MonitorHourlyValueH2o,
             "UNADJUSTED_HRLY_VALUE",
             "H2O_Monitor_Hourly_Value_Records_By_Hour_Location")
    {
    }

    #endregion


    #region Base Class Overrides

    protected override void FilterData()
    {
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
    }

    protected override bool SetErrorSuppressValues()
    {
      DataRowView currentLocation = GetCheckParameter("Current_Monitor_Plan_Location_Record").ValueAsDataRowView();
      DataRowView monitorHourlyRecord = GetCheckParameter("Current_H2O_Monitor_Hourly_Record").ValueAsDataRowView();

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
