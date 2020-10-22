using System;
using System.Data;
using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Extensions;
using ECMPS.ErrorSuppression;


namespace ECMPS.Checks.EmissionsReport
{

  public class cLeakStatusCategory : cCategoryHourly
  {

    #region Constructors

    public cLeakStatusCategory(cCategoryHourly categoryEmission)
      : base(categoryEmission.CheckEngine,
             categoryEmission.EmissionsReportProcess,
             categoryEmission,
             "LKSTAT")
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
      if (EmissionParameters.CurrentMhvRecord.Value != null)
        RecordIdentifier = "Component Id " + EmissionParameters.CurrentMhvRecord.Value["COMPONENT_IDENTIFIER"].AsString();
    }

    protected override bool SetErrorSuppressValues()
    {
      DataRowView currentLocation = GetCheckParameter("Current_Monitor_Plan_Location_Record").ValueAsDataRowView();

      if ((currentLocation != null) && (EmissionParameters.CurrentMhvRecord.Value != null))
      {
        long facId = CheckEngine.FacilityID;
        string locationName = currentLocation["LOCATION_NAME"].AsString();
        DateTime? matchTimeValue = EmissionParameters.CurrentMhvRecord.Value.AsDateTime("BEGIN_DATE", "BEGIN_HOUR");

        ErrorSuppressValues = new cErrorSuppressValues(facId, locationName, null, null, "HOUR", matchTimeValue);

        return true;
      }
      else
        return false;
    }

    #endregion

  }

}
