using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Extensions;
using ECMPS.ErrorSuppression;


namespace ECMPS.Checks.EmissionsReport
{
  public class cFuelFlowCategory : cCategoryHourly
  {

    #region Constructors

    public cFuelFlowCategory(cCheckEngine ACheckEngine,
                                cEmissionsReportProcess AHourlyEmissionsData,
                                cFuelFlowInit AFuelFlowInitializationCategory)
      : base(ACheckEngine,
             (cEmissionsReportProcess)AHourlyEmissionsData,
             (cCategory)AFuelFlowInitializationCategory,
             "HFF")
    {
    }

    #endregion


    #region Base Class Overrides

    protected override void FilterData()
    {
      DataRowView CurrentFFRecord = (DataRowView)GetCheckParameter("Current_Fuel_Flow_Record").ParameterValue;

      string MonSys = cDBConvert.ToString(CurrentFFRecord["MON_SYS_ID"]);

      DataView TempView = (DataView)FilterRanged("SystemHourlyFuelFlow", SystemHourlyFuelFlow, CurrentOpDate, CurrentOpHour, CurrentMonLocId);

      TempView.RowFilter = "MON_SYS_ID = '" + MonSys + "'";

      SetCheckParameter("System_Fuel_Flow_Records_For_Hour", TempView, eParameterDataType.DataView);
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
      DataRowView currentFuelFlow = (DataRowView)GetCheckParameter("Current_Fuel_Flow_Record").ParameterValue;

      if (currentLocation != null && currentFuelFlow != null)
      {
        long facId = CheckEngine.FacilityID;
        string locationName = currentLocation["LOCATION_NAME"].AsString();
        string matchDataValue = currentFuelFlow["FUEL_CD"].AsString();
        DateTime? matchTimeValue = currentFuelFlow.AsDateTime("BEGIN_DATE", "BEGIN_HOUR");

        ErrorSuppressValues = new cErrorSuppressValues(facId, locationName, "FUELTYP", matchDataValue, "HOUR", matchTimeValue);
        return true;
      }
      else
        return false;
    }

    #endregion

  }
}
