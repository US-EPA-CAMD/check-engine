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
  /// Summary description for So2DerivedHourlyCategory.
  /// </summary>
  public class cSo2DerivedHourlyCategory : cCategoryHourly
  {

    #region Constructors

    public cSo2DerivedHourlyCategory(cCheckEngine ACheckEngine,
                                     cEmissionsReportProcess AHourlyEmissionsData,
                                     cOperatingHourCategory AOperatingHourCategory)
      : base("SO2DH",
             (cEmissionsReportProcess)AHourlyEmissionsData,
             (cCategory)AOperatingHourCategory,
             "DERIVED_HRLY_VALUE",
             AHourlyEmissionsData.DerivedHourlyValueSo2,
             "ADJUSTED_HRLY_VALUE",
             "SO2_Derived_Hourly_Value_Records_By_Hour_Location")
    {
    }


    #endregion


    #region Base Class Overrides

    protected override void FilterData()
    {
      // I do not believe this parameter is used (djw2)
      //SetCheckParameter("SO2_Monitor_Formula_Records_By_Hour_Location",
      //                  FilterRanged("MonitorFormulaSo2", MonitorFormulaSo2, FCurrentOpDate, FCurrentOpHour, FCurrentMonLocId), 
      //                  eParameterDataType.DataView);
    }

    //public override void FilterPrimary()
    //{
    //  FilterHourly("DerivedHourlyValueSo2", DerivedHourlyValueSo2, FCurrentOpDate, FCurrentOpHour, FCurrentMonLocId);
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
      DataRowView derivedHourlyRecord = GetCheckParameter("Current_SO2_Derived_Hourly_Record").ValueAsDataRowView();

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
