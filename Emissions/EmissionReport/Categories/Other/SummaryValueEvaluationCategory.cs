using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Extensions;
using ECMPS.ErrorSuppression;


namespace ECMPS.Checks.EmissionsReport
{
  public class cSummaryValueEvaluationCategory : cCategoryHourly
  {

    #region Constructors

    public cSummaryValueEvaluationCategory(cCheckEngine ACheckEngine,
                                           cEmissionsReportProcess AHourlyEmissionsData,
                                           cSummaryValueInitializationCategory ASummaryValueInitializationCategory)
      : base(ACheckEngine,
             (cEmissionsReportProcess)AHourlyEmissionsData,
             (cCategory)ASummaryValueInitializationCategory,
             "SUMEVAL")
    {
    }

    #endregion


    #region Base Class Overrides

    protected override void FilterData()
    {
      SetCheckParameter("Summary_Value_Records_By_Reporting_Period_Location",
                        FilterLocation("SummaryValue", SummaryValue, CurrentMonLocId),
                        eParameterDataType.DataView);

      SetCheckParameter("Method_Records",
                      FilterLocation("MonitorMethod", MonitorMethod, CurrentMonLocId),
                      eParameterDataType.DataView);

      SetCheckParameter("Operating_Supp_Data_Records_by_Location",
                      FilterLocation("OpSuppData", OpSuppData, CurrentMonLocId),
                      eParameterDataType.DataView);

      SetDataViewCheckParameter("Quarterly_Emissions_Tolerances_Cross_Check_Table",
                                SourceTables()["CrossCheck_QuarterlyEmissionsTolerances"],
                                "",
                                "Parameter,Uom");

      SetCheckParameter("Location_Program_Records",
                    FilterLocation("LocationProgram", LocationProgram, CurrentMonLocId),
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
      DataRowView currentLocation = GetCheckParameter("Current_Monitor_Plan_Location_Record").ValueAsDataRowView();
      DataView summaryValueRecords = GetCheckParameter("Summary_Value_Records_By_Reporting_Period_Location").ValueAsDataView();

      if (currentLocation != null && summaryValueRecords != null && summaryValueRecords.Count > 0)
      {
        DataRowView SummaryValueRecord = summaryValueRecords[0];

        long facId = CheckEngine.FacilityID;
        string locationName = currentLocation["LOCATION_NAME"].AsString();
        DateTime? matchTimeValue = CheckEngine.ReportingPeriod.BeganDate;

        ErrorSuppressValues = new cErrorSuppressValues(facId, locationName, null, null, "QUARTER", matchTimeValue);
        return true;
      }
      else
      {
        ErrorSuppressValues = null;
        return false;
      }
    }

    #endregion

  }
}
