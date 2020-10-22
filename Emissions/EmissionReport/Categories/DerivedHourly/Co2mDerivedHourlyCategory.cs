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
  /// Summary description for Co2DerivedHourlyCategory.
  /// </summary>
  public class cCo2mDerivedHourlyCategory : cCategoryHourly
  {
    #region Constructors

    public cCo2mDerivedHourlyCategory(cCheckEngine ACheckEngine,
                                      cEmissionsReportProcess AHourlyEmissionsData,
                                      cOperatingHourCategory AOperatingHourCategory)
      : base("CO2MDH",
             (cEmissionsReportProcess)AHourlyEmissionsData,
             (cCategory)AOperatingHourCategory,
             "DERIVED_HRLY_VALUE", 
             AHourlyEmissionsData.DerivedHourlyValueCo2,
             "ADJUSTED_HRLY_VALUE",
             "CO2_Derived_Hourly_Records_By_Hour_Location")
    {
    }


    #endregion


    #region Base Class Overrides

    protected override void FilterData()
    {
      //all data params are already set in OperatingHourCategory            
    }

    //public override void FilterPrimary()
    //{
    //    FilterHourly("DerivedHourlyValueCo2", DerivedHourlyValueCo2, FCurrentOpDate, FCurrentOpHour, FCurrentMonLocId);
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
        DataRowView derivedHourlyRecord = GetCheckParameter("Current_CO2_Mass_Derived_Hourly_Record").ValueAsDataRowView();

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
