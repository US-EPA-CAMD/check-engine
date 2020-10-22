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
  /// Summary description for Co2cDerivedHourlyCategory.
  /// </summary>
  public class cCo2cDerivedHourlyCategory : cCategoryHourly
  {
    #region Constructors

    public cCo2cDerivedHourlyCategory(cCheckEngine ACheckEngine,
                                      cEmissionsReportProcess AHourlyEmissionsData,
                                      cOperatingHourCategory AOperatingHourCategory)
      : base("CO2CDH",
             (cEmissionsReportProcess)AHourlyEmissionsData,
             (cCategory)AOperatingHourCategory,
             "DERIVED_HRLY_VALUE",
             AHourlyEmissionsData.DerivedHourlyValueCo2c,
             "ADJUSTED_HRLY_VALUE",
             "CO2C_Derived_Hourly_Records_By_Hour_Location")
    {
    }

    #endregion


    #region Base Class Overrides

    protected override void FilterData()
    {
      // I do not believe this parameter is used (djw2)
      ////other parameters already set in operatingHourCategory
      //SetCheckParameter("CO2X_Monitor_Default_Records_By_Hour_Location",
      //            FilterRanged("MonitorDefaultCo2x", MonitorDefaultCo2x, FCurrentOpDate, FCurrentOpHour, FCurrentMonLocId),
      //            eParameterDataType.DataView);
    }

    //public override void FilterPrimary()
    //{
    //    FilterHourly("DerivedHourlyValueCo2c", DerivedHourlyValueCo2c, FCurrentOpDate, FCurrentOpHour, FCurrentMonLocId);
    //}

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
      DataRowView derivedHourlyRecord = GetCheckParameter("Current_CO2_Conc_Derived_Hourly_Record").ValueAsDataRowView();

      if (currentLocation != null && derivedHourlyRecord != null)
      {
        long facId = CheckEngine.FacilityID;
        string locationName = currentLocation["LOCATION_NAME"].AsString();
        string matchDataValue = derivedHourlyRecord["PARAMETER_CD"].AsString();
        DateTime? matchTimeValue = derivedHourlyRecord.AsDateTime("BEGIN_DATE", "BEGIN_HOUR");

        ErrorSuppressValues = new cErrorSuppressValues(facId, locationName, "PARAM", matchDataValue, "HOUR", matchTimeValue);
        return true;
      }
      else
        return false;
    }

    #endregion
  }
}
