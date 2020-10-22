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
  /// Category object for daily emissions data category.
  /// </summary>
  public class cDailyEmissionsCategory : cCategoryHourly
  {
    #region Constructors

    public cDailyEmissionsCategory(cCheckEngine ACheckEngine,
                                  cEmissionsReportProcess AHourlyEmissionsData,
                                  cDailyEmissionsInitializationCategory ADailyEmissionsInitializationCategory)
      : base(ACheckEngine,
             (cEmissionsReportProcess)AHourlyEmissionsData,
             (cCategory)ADailyEmissionsInitializationCategory,
             "CO2DAY")
    {
    }

    #endregion


    #region Base Class Overrides

    protected override void FilterData()
    {
      SetDataViewCheckParameter("Monitor_Formula_Records_By_Day_Location", MonitorFormula,
                                eEmissionFilterType.Ranged,
                                CurrentOpDate, CurrentMonLocId);

      SetDataViewCheckParameter("Monitor_Method_Records_By_Day_Location", MonitorMethod,
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
    }

    protected override bool SetErrorSuppressValues()
    {
      DataView dailyEmissionRecords = GetCheckParameter("CO2M_Daily_Emission_Records_For_Day_Location").ValueAsDataView();

      if (dailyEmissionRecords != null && dailyEmissionRecords.Count == 1)
      {
        DataRowView dailyEmissionRecord = dailyEmissionRecords[0];

        long facId = CheckEngine.FacilityID;
        string locationName = dailyEmissionRecord["LOCATION_NAME"].AsString();
        DateTime? matchTimeValue = dailyEmissionRecord["BEGIN_DATE"].AsDateTime();

        ErrorSuppressValues = new cErrorSuppressValues(facId, locationName, null, null, "DATE", matchTimeValue);
        return true;
      }
      else
        return false;
    }

    #endregion

  }
}
