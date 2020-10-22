using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Extensions;
using ECMPS.ErrorSuppression;


namespace ECMPS.Checks.EmissionsReport
{
  public class cCo2cOverallHourlyCategory : cCategoryHourly
  {
    #region Constructors

    public cCo2cOverallHourlyCategory(cCheckEngine ACheckEngine,
                                      cEmissionsReportProcess AHourlyEmissionsData,
                                      cOperatingHourCategory AOperatingHourCategory)
      : base(ACheckEngine,
             (cEmissionsReportProcess)AHourlyEmissionsData,
             (cCategory)AOperatingHourCategory,
             "CO2COVR")
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
      DataView hourlyOperatingDataRecords = GetCheckParameter("Hourly_Operating_Data_Records_By_Hour_Location").ValueAsDataView();

      if (hourlyOperatingDataRecords != null && hourlyOperatingDataRecords.Count == 1)
      {
        DataRowView hourlyOperatingDataRecord = hourlyOperatingDataRecords[0];

        long facId = CheckEngine.FacilityID;
        string locationName = hourlyOperatingDataRecord["LOCATION_NAME"].AsString();
        DateTime? matchTimeValue = hourlyOperatingDataRecord.AsDateTime("BEGIN_DATE", "BEGIN_HOUR");

        ErrorSuppressValues = new cErrorSuppressValues(facId, locationName, null, null, "HOUR", matchTimeValue);
        return true;
      }
      else
        return false;
    }

    #endregion
  }
}
