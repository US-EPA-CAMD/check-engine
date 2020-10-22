using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Data.Ecmps.Dbo.View;
using ECMPS.Checks.Em.Parameters;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;
using ECMPS.Definitions.Extensions;
using ECMPS.ErrorSuppression;


namespace ECMPS.Checks.EmissionsReport
{

  public class WeeklySystemIntegrityTestOperatingDatesCategory : cCategoryHourly
  {

    #region Constructors

    /// <summary>
    /// Creates a category with a specific parent category and category code.
    /// </summary>
    /// <param name="parentCategory">The parent category of the new category.</param>
    /// <param name="categoryCd">The category code of the new category.</param>
	  public WeeklySystemIntegrityTestOperatingDatesCategory(cCategory parentCategory)
      : base(parentCategory, "WSIOP", "WeeklySystemIntegrity")
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
      if (EmParameters.CurrentWeeklySystemIntegrityTest != null)
        RecordIdentifier = string.Format("Location {0}, Component {1} and Type {2} at {3}",
                                         EmParameters.CurrentMonitorPlanLocationRecord.LocationName,
                                         EmParameters.CurrentWeeklySystemIntegrityTest.ComponentIdentifier,
                                         EmParameters.CurrentWeeklySystemIntegrityTest.ComponentTypeCd,
                                         EmParameters.CurrentWeeklySystemIntegrityTest.TestDatehour);
      else
        RecordIdentifier = "this test";
    }

    protected override bool SetErrorSuppressValues()
    {
      ErrorSuppressValues = new cErrorSuppressValues(CheckEngine.FacilityID,
                                                     EmParameters.CurrentMonitorPlanLocationRecord.LocationName,
                                                    null, null,
                                                     "HOUR", EmParameters.CurrentDateHour);
      return true;
    }

    #endregion

  }

}
