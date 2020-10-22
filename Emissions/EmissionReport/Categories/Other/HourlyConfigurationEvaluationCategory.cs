using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Extensions;

using ECMPS.ErrorSuppression;


namespace ECMPS.Checks.EmissionsReport
{
  public class cHourlyConfigurationEvaluationCategory : cCategoryHourly
  {

    #region Constructors

    public cHourlyConfigurationEvaluationCategory(cCheckEngine ACheckEngine,
                                                  cEmissionsReportProcess AHourlyEmissionsData,
                                                  cHourlyConfigurationInitializationCategory AHourlyConfigurationInitializationCategory)
      : base(ACheckEngine,
             (cEmissionsReportProcess)AHourlyEmissionsData,
             (cCategory)AHourlyConfigurationInitializationCategory,
             "CFGEVAL")
    {
    }

    #endregion


    #region Base Class Overrides

    protected override void FilterData()
    {
      SetCheckParameter("HI_Derived_Hourly_Value_Records_By_Hour_Location",
                        FilterHourly(DerivedHourlyValueHi, CurrentOpDate, CurrentOpHour, CurrentMonLocId), 
                        eParameterDataType.DataView);

      SetCheckParameter("NOx_Derived_Hourly_Value_Records_By_Hour_Location",
                        FilterHourly(DerivedHourlyValueNox, CurrentOpDate, CurrentOpHour, CurrentMonLocId),
                        eParameterDataType.DataView);

      SetCheckParameter("NOxR_Derived_Hourly_Value_Records_By_Hour_Location",
                        FilterHourly(DerivedHourlyValueNoxr, CurrentOpDate, CurrentOpHour, CurrentMonLocId),
                        eParameterDataType.DataView);

      SetCheckParameter("Operating_Supp_Data_Records_by_Location",
                FilterLocation("OpSuppData", OpSuppData, CurrentMonLocId),
                eParameterDataType.DataView);

      SetCheckParameter("Monitor_Default_Records_By_Hour_Location",
                FilterRanged("MonitorDefault", MonitorDefault, CurrentOpDate, CurrentOpHour, CurrentMonLocId),
                eParameterDataType.DataView);

      SetCheckParameter("Monitor_System_Records_By_Hour_Location",
                        FilterRanged("MonitorSystem", MonitorSystem, CurrentOpDate, CurrentOpHour, CurrentMonLocId),
                        eParameterDataType.DataView);

      SetCheckParameter("Fuel_Records_By_Hour_Location",
                  FilterRanged("LocationFuel", LocationFuel, CurrentOpDate, CurrentOpHour, CurrentMonLocId),
                eParameterDataType.DataView);

      SetCheckParameter("Location_Program_Records_By_Hour_Location",
                        FilterRanged("LocationProgramHourLocation", LocationProgramHourLocation, CurrentOpDate, CurrentMonLocId, "Emissions_Recording_Begin_Date", "Unit_Monitor_Cert_Begin_Date", "End_Date"),
                        eParameterDataType.DataView);

      SetCheckParameter("QA_Certification_Event_Records",
                FilterLocation("QaCertEvent", QaCertEvent, CurrentMonLocId),
                eParameterDataType.DataView);

      SetCheckParameter("QA_Supplemental_Attribute_Records",
                        FilterLocation("QaSuppAttribute", QaSuppAttribute, CurrentMonLocId), eParameterDataType.DataView);


      SetDataViewCheckParameter("Fuel_Code_Lookup_Table", SourceTables()["FuelCode"], "", "");


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

      DateTime? matchTimeValue = EmissionParameters.CurrentOperatingDate.Value.Value.AddHours((double)EmissionParameters.CurrentOperatingHour.Value); //GetCheckParameter("Current_Operating_Date").AsDateTime().AddHours(GetCheckParameter("Current_Operating_Hour").AsInteger());

      string locationName;
      {
        DataRowView currentMonitorPlanLocationRecord = GetCheckParameter("Current_Monitor_Plan_Location_Record").AsDataRowView();
        locationName = currentMonitorPlanLocationRecord["LOCATION_NAME"].AsString();
      }

      ErrorSuppressValues = new cErrorSuppressValues(CheckEngine.FacilityID, locationName, null, null, "HOUR", matchTimeValue);
        
      result = true;

      return result;
    }

    #endregion

  }
}
