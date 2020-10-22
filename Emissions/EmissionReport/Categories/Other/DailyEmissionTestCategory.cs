using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Extensions;
using ECMPS.ErrorSuppression;


namespace ECMPS.Checks.EmissionsReport
{
  public class cDailyEmissionTestCategory : cCategoryHourly
  {
    #region Constructors

    public cDailyEmissionTestCategory(cCheckEngine checkEngine,
                                      cEmissionsReportProcess emissionsReportProcess,
                                      cOperatingHourCategory operatingHourCategory)
      : base("MISCTST",
             (cEmissionsReportProcess)emissionsReportProcess,
             (cCategory)operatingHourCategory,
             "DailyMiscellaneousTest")
    {
      //TestDescription = "";
      //ComponentID = "";
      //SystemID = "";            
      //SpanScale = "";
      //TestDescription = "";
    }

    #endregion

    #region Base Class Overrides

    protected override void FilterData()
    {
      //DataRowView CurrentDayEMTest = (DataRowView)GetCheckParameter("Current_Daily_Emission_Test").ParameterValue;
      //ComponentID = cDBConvert.ToString(CurrentDayEMTest["COMPONENT_IDENTIFIER"]);
      //SystemID = cDBConvert.ToString(CurrentDayEMTest["SYSTEM_IDENTIFIER"]);                     
      //SpanScale = cDBConvert.ToString(CurrentDayEMTest["SPAN_SCALE_CD"]);
      //TestDescription = cDBConvert.ToString(CurrentDayEMTest["TEST_TYPE_CD_DESCRIPTION"]);
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
      RecordIdentifier = "this test";
    }

    protected override bool SetErrorSuppressValues()
    {
      DataRowView dailyEmTest = GetCheckParameter("Current_Daily_Emission_Test").ValueAsDataRowView();

      if (dailyEmTest != null)
      {
        long facId = CheckEngine.FacilityID;
        string locationName = dailyEmTest["LOCATION_NAME"].AsString();
        string matchDataValue = dailyEmTest["TEST_TYPE_CD"].AsString();
        DateTime? matchTimeValue = dailyEmTest.AsDateTime("DAILY_TEST_DATE", "DAILY_TEST_HOUR");

        ErrorSuppressValues = new cErrorSuppressValues(facId, locationName, "TESTTYP", matchDataValue, "HOUR", matchTimeValue);
        return true;
      }
      else
        return false;
    }

    #endregion

  }
}
