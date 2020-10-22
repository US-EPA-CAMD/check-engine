using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Extensions;
using ECMPS.ErrorSuppression;


namespace ECMPS.Checks.EmissionsReport
{
  public class cCo2cCalculationCategory : cCategoryHourly
  {
    #region Constructors

    public cCo2cCalculationCategory(cCheckEngine ACheckEngine,
                                    cEmissionsReportProcess AHourlyEmissionsData,
                                    cOperatingHourCategory AOperatingHourCategory)
      : base(ACheckEngine,
             (cEmissionsReportProcess)AHourlyEmissionsData,
             (cCategory)AOperatingHourCategory,
             "CO2CVC")
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




