using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Extensions;
using ECMPS.ErrorSuppression;

namespace ECMPS.Checks.EmissionsReport
{
  public class cHiDerivedHourlyCategory : cCategoryHourly
  {

    #region Constructors

    public cHiDerivedHourlyCategory(cCheckEngine ACheckEngine,
                                    cEmissionsReportProcess AHourlyEmissionsData,
                                    cOperatingHourCategory AOperatingHourCategory)
      : base("HIDH",
             (cEmissionsReportProcess)AHourlyEmissionsData,
             (cCategory)AOperatingHourCategory,
             "DERIVED_HRLY_VALUE",
             AHourlyEmissionsData.DerivedHourlyValueHi,
             "ADJUSTED_HRLY_VALUE",
             "HI_Derived_Hourly_Value_Records_By_Hour_Location")
    {
    }


    #endregion


    #region Base Class Overrides

    protected override void FilterData()
    {
      /*      SetCheckParameter("Co2n_Monitor_Default_Records_For_Nfs_By_Hour_Location",
                              FilterRanged("MonitorDefaultCo2nNfs", MonitorDefaultCo2nNfs, FCurrentOpDate, FCurrentOpHour, FCurrentMonLocId),
                              eParameterDataType.DataView);
      */
    }

    //public override void FilterPrimary()
    //{
    //  FilterHourly("DerivedHourlyValueHi", DerivedHourlyValueHi, FCurrentOpDate, FCurrentOpHour, FCurrentMonLocId);
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
      DataRowView derivedHourlyRecord = GetCheckParameter("Current_Heat_Input_Derived_Hourly_Record").ValueAsDataRowView();

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
