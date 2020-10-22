using System;
using System.Data;
using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Em.Parameters;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Extensions;
using ECMPS.ErrorSuppression;


namespace ECMPS.Checks.EmissionsReport
{
  public class cDailyCalibrationStatusCategory : cCategoryHourly
  {
    #region Constructors
    
    /// <summary>
    /// Creates a category with a specific parent category, category code, and parameter code.
    /// </summary>
    /// <param name="parentCategory">The parent category of the new category.</param>
    /// <param name="categoryCd">The category code of the new category.</param>
    /// <param name="parameterCd">The parameter code of the associated monitor or derived hourly data.</param>
    public cDailyCalibrationStatusCategory(cCategory parentCategory, string categoryCd, string parameterCd)
      : base(parentCategory, categoryCd)
    {
      ParameterCd = parameterCd;
    }

        /// <summary>
        /// Creates a category with a specific parent category and category code.
        /// </summary>
        /// <param name="parentCategory">The parent category of the new category.</param>
        /// <param name="categoryCd">The category code of the new category.</param>
        public cDailyCalibrationStatusCategory(cCategory parentCategory, string categoryCd)
          : base(parentCategory, categoryCd)
        {
            ParameterCd = null;
        }

        public cDailyCalibrationStatusCategory(cCheckEngine ACheckEngine,
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


    #region Public Properties

    /// <summary>
    /// The parameter code of the associated monitor or derived hourly data.
    /// </summary>
    public string ParameterCd { get; protected set; }

    #endregion


    #region Base Class Overrides

    protected override void FilterData()
    {
      DataView[] OperatingHoursByLocation = (DataView[])EmissionParameters.OperatingHoursByLocation.Value;
      DataView[] NonOperatingHoursByLocation = (DataView[])EmissionParameters.NonOperatingHoursByLocation.Value;

      SetCheckParameter("Hourly_Operating_Data_Records_for_Location",
                        OperatingHoursByLocation[CurrentMonLocPos],
                        eParameterDataType.DataView);

      SetCheckParameter("Hourly_Non_Operating_Data_Records_for_Location",
                        NonOperatingHoursByLocation[CurrentMonLocPos],
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
      String stringFormat = (EmParameters.CurrentAnalyzerRangeUsed.IsNotEmpty()) ? "Component ID {0}, Span Scale {1}" : "Component ID {0}";

      RecordIdentifier = String.Format(stringFormat, EmParameters.QaStatusComponentIdentifier, EmParameters.CurrentAnalyzerRangeUsed);
    }

    protected override bool SetErrorSuppressValues()
    {
      if ((EmParameters.CurrentMonitorPlanLocationRecord != null) && ((ParameterCd != null) || (EmParameters.CurrentMhvRecord != null)))
      {
        long facId = CheckEngine.FacilityID;
        string locationName = EmParameters.CurrentMonitorPlanLocationRecord.LocationName;
        string matchDataValue = (ParameterCd != null) ? ParameterCd : EmParameters.CurrentMhvRecord.ParameterCd;
        DateTime? matchTimeValue = EmParameters.CurrentDateHour.AsStartDateTime();

        ErrorSuppressValues = new cErrorSuppressValues(facId, locationName, "PARAM", matchDataValue, "HOUR", matchTimeValue);
        return true;
      }
      else
        return false;
    }

    #endregion
  }
}
