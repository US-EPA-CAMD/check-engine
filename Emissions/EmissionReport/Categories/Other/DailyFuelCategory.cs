using System;
using System.Data;
using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Extensions;
using ECMPS.ErrorSuppression;


namespace ECMPS.Checks.EmissionsReport
{
  public class cDailyFuelCategory : cCategoryHourly
  {
    #region Constructors

    public cDailyFuelCategory(cCheckEngine ACheckEngine,
                              cEmissionsReportProcess AHourlyEmissionsData,
                              cDailyEmissionsInitializationCategory ADailyEmissionsInitializationCategory)
      : base("CO2FUEL",
             (cEmissionsReportProcess)AHourlyEmissionsData,
             (cCategory)ADailyEmissionsInitializationCategory,
             "DailyFuel")
    {
    }
    #endregion

    #region Base Class Overrides

    protected override void FilterData()
    {

      SetDataViewCheckParameter("Fuel_Records_By_Date_and_Location", LocationFuel,
                            eEmissionFilterType.Ranged,
                            CurrentOpDate, CurrentMonLocId);
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
      DataRowView DailyFuelRecord = GetCheckParameter("Current_Daily_Fuel_Record").ValueAsDataRowView();
      if (DailyFuelRecord != null)
        RecordIdentifier = "Fuel Code " + cDBConvert.ToString(DailyFuelRecord["FUEL_CD"]);
    }

    protected override bool SetErrorSuppressValues()
    {
      DataRowView dailyFuelRecord = GetCheckParameter("Current_Daily_Fuel_Record").ValueAsDataRowView();

      if (dailyFuelRecord != null)
      {
        long facId = CheckEngine.FacilityID;
        string locationName = dailyFuelRecord["LOCATION_NAME"].AsString();
        DateTime? matchTimeValue = dailyFuelRecord["BEGIN_DATE"].AsDateTime();

        ErrorSuppressValues = new cErrorSuppressValues(facId, locationName, null, null, "DATE", matchTimeValue);
        return true;
      }
      else
      {
        ErrorSuppressValues = null;
        return false;
      }
    }

    #endregion
  }
}
