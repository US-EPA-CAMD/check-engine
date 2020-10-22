using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Extensions;
using ECMPS.ErrorSuppression;


namespace ECMPS.Checks.EmissionsReport
{
  public class cDailyEmissionsInitializationCategory : cCategoryHourly
  {
    #region Constructors

    public cDailyEmissionsInitializationCategory(cCheckEngine ACheckEngine,
                              cEmissionsReportProcess AHourlyEmissionsData,
                              cSummaryValueInitializationCategory ASummaryValueInitializationCategory)
      : base(ACheckEngine,
             (cEmissionsReportProcess)AHourlyEmissionsData,
             (cCategory)ASummaryValueInitializationCategory,
             "CO2INIT")
    {
    }
    #endregion

    #region Base Class Overrides

    protected override void FilterData()
    {

      // CO2M_Daily_Emission_Records_For_Day_Location
      SetDataViewCheckParameter("CO2M_Daily_Emission_Records_For_Day_Location",
                                DailyEmissionCo2m,
                                eEmissionFilterType.Specific,
                                CurrentOpDate,
                                CurrentMonLocId);


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

      DataView dailyEmissionRecords = GetCheckParameter("CO2M_Daily_Emission_Records_For_Day_Location").AsDataView();

      if (dailyEmissionRecords != null && dailyEmissionRecords.Count == 1)
      {
        DataRowView dailyEmissionRecord = dailyEmissionRecords[0];

        long facId = CheckEngine.FacilityID;
        string locationName = dailyEmissionRecord["LOCATION_NAME"].AsString();
        DateTime? matchTimeValue = dailyEmissionRecord["BEGIN_DATE"].AsDateTime();

        ErrorSuppressValues = new cErrorSuppressValues(facId, locationName, null, null, "DATE", matchTimeValue);
        result = true;
      }
      else
        result = false;

      return result;
    }

    #endregion
  }
}