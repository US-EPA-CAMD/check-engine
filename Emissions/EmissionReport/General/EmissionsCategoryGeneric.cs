using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Extensions;
using ECMPS.ErrorSuppression;


namespace ECMPS.Checks.EmissionsReport
{
  public class cCategoryHourlyGeneric : cCategoryHourly
  {

    #region Constructors

    /// <summary>
    /// Creates a generic hourly category instance.
    /// </summary>
    /// <param name="ACheckEngine">The owner check engine object.</param>
    /// <param name="AHourlyEmissionsData">The owner hourly process object.</param>
    /// <param name="ACategoryCode">The category code of this category object.</param>
    public cCategoryHourlyGeneric(cCheckEngine ACheckEngine,
                                  cEmissionsReportProcess AHourlyEmissionsData,
                                  string ACategoryCode)
      : base(ACheckEngine, (cEmissionsReportProcess)AHourlyEmissionsData,
             ACategoryCode)
    {
      FFilterData = null;
    }

    /// <summary>
    /// Creates a generic hourly category instance.
    /// </summary>
    /// <param name="ACheckEngine">The owner check engine object.</param>
    /// <param name="AHourlyEmissionsData">The owner hourly process object.</param>
    /// <param name="AParentCategory">The parent category object.</param>
    /// <param name="ACategoryCode">The category code of this category object.</param>
    public cCategoryHourlyGeneric(cCheckEngine ACheckEngine,
                                  cEmissionsReportProcess AHourlyEmissionsData,
                                  cCategory AParentCategory,
                                  string ACategoryCode)
      : base(ACheckEngine, (cEmissionsReportProcess)AHourlyEmissionsData,
             AParentCategory, ACategoryCode)
    {
      FFilterData = null;
    }

    /// <summary>
    /// Creates a generic hourly category instance.
    /// </summary>
    /// <param name="ACheckEngine">The owner check engine object.</param>
    /// <param name="AHourlyEmissionsData">The owner hourly process object.</param>
    /// <param name="ACategoryCode">The category code of this category object.</param>
    /// <param name="AFilterData">The delgate for filter data in the category.</param>
    public cCategoryHourlyGeneric(cCheckEngine ACheckEngine,
                                  cEmissionsReportProcess AHourlyEmissionsData,
                                  string ACategoryCode,
                                  dFilterData AFilterData)
      : base(ACheckEngine, (cEmissionsReportProcess)AHourlyEmissionsData,
             ACategoryCode)
    {
      FFilterData = AFilterData;
    }

    /// <summary>
    /// Creates a generic hourly category instance.
    /// </summary>
    /// <param name="ACheckEngine">The owner check engine object.</param>
    /// <param name="AHourlyEmissionsData">The owner hourly process object.</param>
    /// <param name="AParentCategory">The parent category object.</param>
    /// <param name="ACategoryCode">The category code of this category object.</param>
    /// <param name="AFilterData">The delgate for filter data in the category.</param>
    public cCategoryHourlyGeneric(cCheckEngine ACheckEngine,
                                  cEmissionsReportProcess AHourlyEmissionsData,
                                  cCategory AParentCategory,
                                  string ACategoryCode,
                                  dFilterData AFilterData)
      : base(ACheckEngine, (cEmissionsReportProcess)AHourlyEmissionsData,
             AParentCategory, ACategoryCode)
    {
      FFilterData = AFilterData;
    }

    #endregion


    #region Public Static Methods: Category Specific Filter Data Methods

    public static bool FilterData_LmeHit(cCategoryHourly ACategory, ref string AErrorMessage)
    {
      bool Result;

      try
      {
        ACategory.SetCheckParameter("LME_Derived_Hourly_Value_Records_By_Hour_Location",
                                    ACategory.FilterHourly("DerivedHourlyValueLme",
                                                           ACategory.DerivedHourlyValueSo2r,
                                                           ACategory.CurrentOpDate, ACategory.CurrentOpHour,
                                                           ACategory.CurrentMonLocId),
                                    eParameterDataType.DataView);

        Result = true;
      }
      catch (Exception ex)
      {
        AErrorMessage = ex.Message;
        Result = false;
      }

      return Result;
    }

    #endregion


    #region Public Delegates

    public delegate bool dFilterData(cCategoryHourly ACategory, ref string AErrorMessage);

    #endregion


    #region Private Fields

    private dFilterData FFilterData = null;

    #endregion


    #region Base Class Overrides

    protected override void FilterData()
    {
      if (FFilterData != null)
      {
        string ErrorMessage = "";

        FFilterData(this, ref ErrorMessage);
      }
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

    #endregion

    protected override bool SetErrorSuppressValues()
    {
      DataRowView currentLocation = GetCheckParameter("Current_Monitor_Plan_Location_Record").ValueAsDataRowView();

      DataRowView derivedHourlyRecord;
      {
        switch (CategoryCd)
        {
          case "CO2MDHV": derivedHourlyRecord = GetCheckParameter("Current_CO2_Mass_Derived_Hourly_Record").AsDataRowView(); break;
          case "HITDHV": derivedHourlyRecord = GetCheckParameter("Current_Heat_Input_Derived_Hourly_Record").AsDataRowView(); break;
          case "NOXMDHV": derivedHourlyRecord = GetCheckParameter("Current_NOx_Mass_Derived_Hourly_Record").AsDataRowView(); break;
          case "SO2MDHV": derivedHourlyRecord = GetCheckParameter("Current_SO2_Derived_Hourly_Record").AsDataRowView(); break;
          default: derivedHourlyRecord = null; break;
        }
      }

      bool result;

      if (currentLocation != null && derivedHourlyRecord != null)
      {
        long facId = CheckEngine.FacilityID;
        string locationName = currentLocation["LOCATION_NAME"].AsString();
        string matchDataValue = derivedHourlyRecord["PARAMETER_CD"].AsString();
        DateTime? matchTimeValue = derivedHourlyRecord.AsDateTime("BEGIN_DATE", "BEGIN_HOUR");

        ErrorSuppressValues = new cErrorSuppressValues(facId, locationName, "PARAM", matchDataValue, "HOUR", matchTimeValue);
        result = true;
      }
      else
        result = false;

      return result;
    }
  }
}
