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
  /// <summary>
  /// Summary description for MATSMonitorHourlyCategory.
  /// </summary>
  public class cMATSMonitorHourlyCategory : cCategoryHourly
  {

    #region Constructors

    public cMATSMonitorHourlyCategory(cOperatingHourCategory parentCategory,
                                      string categoryCd,
                                      string primaryTableName,
                                      string primaryCheckParameterName,
                                      string parameterCd)
      : base(categoryCd,
             parentCategory,
             primaryTableName,
             "", // valueColumnName is only need for MODC Data Borders object which is not used for MATS.
             primaryCheckParameterName)
    {
      ParameterCd = parameterCd;
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
    }

    protected override int[] GetDataBorderModcList()
    {
      return null;
    }

    protected override int[] GetQualityAssuranceHoursModcList()
    {
      int[] ModcList = new int[10];

      ModcList[0] = 1;
      ModcList[1] = 2;
      ModcList[2] = 4;
      ModcList[3] = 16;
      ModcList[4] = 17;
      ModcList[5] = 19;
      ModcList[6] = 20;
      ModcList[7] = 21;
      ModcList[8] = 22;
      ModcList[9] = 53;

      return ModcList;
    }

    protected override void SetRecordIdentifier()
    {
      string locationName = (EmParameters.CurrentMonitorPlanLocationRecord != null) ? EmParameters.CurrentMonitorPlanLocationRecord.LocationName : null;

      RecordIdentifier = string.Format("Location {0}, Parameter {1}", locationName, ParameterCd);
    }

	protected override bool SetErrorSuppressValues()
	{
    if (EmParameters.CurrentMonitorPlanLocationRecord != null && EmParameters.CurrentHourlyOpRecord != null)
    {
      long facId = CheckEngine.FacilityID;
      string locationName = EmParameters.CurrentMonitorPlanLocationRecord.LocationName;
      string matchDataValue = ParameterCd;
      DateTime? matchTimeValue = EmParameters.CurrentHourlyOpRecord.BeginDatehour.Default();

      ErrorSuppressValues = new cErrorSuppressValues(facId, locationName, "PARAM", matchDataValue, "HOUR", matchTimeValue);
      return true;
    }
    else
      return false;
  }

    #endregion

  }
}
