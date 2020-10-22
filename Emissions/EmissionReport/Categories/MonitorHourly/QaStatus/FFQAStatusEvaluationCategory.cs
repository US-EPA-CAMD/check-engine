using System;
using System.Data;
using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Extensions;
using ECMPS.ErrorSuppression;


namespace ECMPS.Checks.EmissionsReport
{
  public class cFFQAStatusEvaluationCategory : cCategoryHourly
  {
    #region Constructors

    public cFFQAStatusEvaluationCategory(cCheckEngine ACheckEngine,
                             cEmissionsReportProcess AHourlyEmissionsData,
                             cCategory ACategory,
                             string ACategoryCode)
      : base(ACheckEngine,
       (cEmissionsReportProcess)AHourlyEmissionsData,
       ACategory,
       ACategoryCode)
    {
    }

    #endregion

    #region Base Class Overrides

    protected override void FilterData()
    {

      /*SetCheckParameter("FF2L_Test_Records_By_Location_For_QA_Status",
                           FilterLocation("FF2LStatusRecords", FF2LStatusRecords, FCurrentMonLocId), 
                           eParameterDataType.DataView);

      SetCheckParameter("PEI_Test_Records_By_Location_For_QA_Status",
                           FilterLocation("PEIStatusRecords", PEIStatusRecords, FCurrentMonLocId),
                           eParameterDataType.DataView);
      */
      //SetCheckParameter("Operating_Supp_Data_Records_by_Location",
      //                     FilterLocation("OpSuppData", OpSuppData, FCurrentMonLocId),
      //                     eParameterDataType.DataView);

      SetCheckParameter("Test_Extension_Exemption_Records",
                         FilterLocation("TEERecords", TEERecords, CurrentMonLocId),
                         eParameterDataType.DataView);

      //SetCheckParameter("Location_Reporting_Frequency_Records",
      //                   FilterLocation("LocationRepFreqRecords", LocationRepFreqRecords, FCurrentMonLocId),
      //                   eParameterDataType.DataView);
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
      DataRowView FFCompRecToCheck = GetCheckParameter("Fuel_Flow_Component_Record_to_Check").ValueAsDataRowView();
      if (FFCompRecToCheck != null)
        RecordIdentifier = "ComponentID " + cDBConvert.ToString(FFCompRecToCheck["COMPONENT_IDENTIFIER"]);
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
