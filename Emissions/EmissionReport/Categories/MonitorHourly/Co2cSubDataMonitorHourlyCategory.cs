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
  public class cCo2cSubDataMonitorHourlyCategory : cCategoryHourly
  {
    #region Constructors

    public cCo2cSubDataMonitorHourlyCategory(cCheckEngine ACheckEngine,
                                             cEmissionsReportProcess AHourlyEmissionsData,
                                             cOperatingHourCategory AOperatingHourCategory,
                                             cCategoryHourly APrimaryDataCategory)
      : base(ACheckEngine,
             (cEmissionsReportProcess)AHourlyEmissionsData,
             (cCategory)AOperatingHourCategory,
             (cCategoryHourly)APrimaryDataCategory,
             "CO2SDMH")
    {
    }

    #endregion


    #region Public Methods

    //Should move this to the constructor.
    public void SetPrimaryDataCategory(cCategoryHourly APrimaryDataCategory)
    {
      FMissingDataBorders = APrimaryDataCategory.MissingDataBorders;
      FQualityAssuredHours = APrimaryDataCategory.ModcHourCounts;
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
      DataRowView monitorHourlyRecord = GetCheckParameter("Current_CO2_Conc_Missing_Data_Monitor_Hourly_Record").ValueAsDataRowView();

      if (currentLocation != null && monitorHourlyRecord != null)
      {
        long facId = CheckEngine.FacilityID;
        string locationName = currentLocation["LOCATION_NAME"].AsString();
        string matchDataValue = monitorHourlyRecord["PARAMETER_CD"].AsString();
        DateTime? matchTimeValue = monitorHourlyRecord.AsDateTime("BEGIN_DATE", "BEGIN_HOUR");

        ErrorSuppressValues = new cErrorSuppressValues(facId, locationName, "PARAM", matchDataValue, "HOUR", matchTimeValue);
        return true;
      }
      else
        return false;
    }

    #endregion
  }
}
