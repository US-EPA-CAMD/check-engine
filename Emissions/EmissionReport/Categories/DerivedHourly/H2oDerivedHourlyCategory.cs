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
  /// Summary description for H2oDerivedHourlyCategory.
  /// </summary>
  public class cH2oDerivedHourlyCategory : cCategoryHourly
  {

    #region Constructors

    public cH2oDerivedHourlyCategory(cCheckEngine ACheckEngine,
                                     cEmissionsReportProcess AHourlyEmissionsData,
                                     cOperatingHourCategory AOperatingHourCategory)
      : base("H2ODH",
             (cEmissionsReportProcess)AHourlyEmissionsData,
             (cCategory)AOperatingHourCategory,
             "DERIVED_HRLY_VALUE",
             AHourlyEmissionsData.DerivedHourlyValueH2o,
             "ADJUSTED_HRLY_VALUE",
             "H2O_Derived_Hourly_Value_Records_By_Hour_Location")
    {
    }

    #endregion


    #region Base Class Overrides

    protected override void FilterData()
    {
    }

    //public override void FilterPrimary()
    //{
    //  FilterHourly("DerivedHourlyValueH2o", DerivedHourlyValueH2o, FCurrentOpDate, FCurrentOpHour, FCurrentMonLocId);
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
      DataRowView derivedHourlyRecord = GetCheckParameter("Current_H2O_Derived_Hourly_Record").ValueAsDataRowView();

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
