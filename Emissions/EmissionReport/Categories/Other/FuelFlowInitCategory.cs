using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Extensions;
using ECMPS.ErrorSuppression;


namespace ECMPS.Checks.EmissionsReport
{
  public class cFuelFlowInit : cCategoryHourly
  {

    #region Constructors

    public cFuelFlowInit(cCheckEngine ACheckEngine,
                         cEmissionsReportProcess AHourlyEmissionsData,
                         cOperatingHourCategory AOperatingHourCategory)
      : base(ACheckEngine,
             (cEmissionsReportProcess)AHourlyEmissionsData,
             (cCategory)AOperatingHourCategory,
             "APDINIT")
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
      bool result;

      DataView hourlyOperatingDataRecords = GetCheckParameter("Hourly_Operating_Data_Records_By_Hour_Location").AsDataView();

      if (hourlyOperatingDataRecords != null && hourlyOperatingDataRecords.Count == 1)
      {
        DataRowView hourlyOperatingDataRecord = hourlyOperatingDataRecords[0];

        long facId = CheckEngine.FacilityID;
        string locationName = hourlyOperatingDataRecord["LOCATION_NAME"].AsString();
        DateTime? matchTimeValue = hourlyOperatingDataRecord.AsDateTime("BEGIN_DATE", "BEGIN_HOUR");

        ErrorSuppressValues = new cErrorSuppressValues(facId, locationName, null, null, "HOUR", matchTimeValue);
        result = true;
      }
      else
        result = false;

      return result;
    }

    #endregion

  }
}
