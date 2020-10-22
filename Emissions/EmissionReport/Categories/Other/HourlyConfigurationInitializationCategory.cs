using System;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Extensions;
using ECMPS.ErrorSuppression;


namespace ECMPS.Checks.EmissionsReport
{
  public class cHourlyConfigurationInitializationCategory : cCategoryHourly
  {

    #region Constructors

    public cHourlyConfigurationInitializationCategory(cCheckEngine ACheckEngine,
                                                      cEmissionsReportProcess AHourlyEmissionsData,
                                                      cDailyEmissionsCategory ADailyEmissionsCategory)
      : base(ACheckEngine, 
             (cEmissionsReportProcess)AHourlyEmissionsData,
             (cCategory)ADailyEmissionsCategory,
             "CFGINIT")
    {
    }

    #endregion


    #region Base Class Overrides

    protected override void FilterData()
    {
      string MonitorPlanMonLocList = cDataFunctions.ColumnToDatalist(Process.SourceData.Tables["MonitorLocation"].DefaultView, "Mon_Loc_Id");
      //string MonitorPlanMonLocList = cDataFunctions.ColumnToDatalist(MonitorLocation.DefaultView, "Mon_Loc_Id");

      SetCheckParameter("Unit_Stack_Configuration_Records_By_Hour_Monitor_Plan",
                        FilterUnitStackConfiguration(CurrentOpDate, MonitorPlanMonLocList),
                        eParameterDataType.DataView);
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
      DateTime? matchTimeValue = GetCheckParameter("Current_Operating_Date").AsDateTime().AddHours(GetCheckParameter("Current_Operating_Hour").AsInteger());

      ErrorSuppressValues = new cErrorSuppressValues(CheckEngine.FacilityID, null, null, null, "HOUR", matchTimeValue);

      return true;
    }

    #endregion

  }
}
