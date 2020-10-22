using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Em.Parameters;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Extensions;
using ECMPS.ErrorSuppression;


namespace ECMPS.Checks.EmissionsReport
{

  /// <summary>
  /// Category class used for categories whose checks are driven by the MatsHourlyGfmRecord check parameter.
  /// </summary>
  public class MatsSorbentTrapAllRowsCategory : cCategoryHourly
  {

    #region Constructors

    /// <summary>
    /// Creates a category object to represent the category indicated by the passed code, 
    /// and links that object to a parent category object.
    /// </summary>
    /// <param name="parentCategory">The parent category.</param>
    /// <param name="categoryCd">The category code of the category the object will represent.</param>
    public MatsSorbentTrapAllRowsCategory(cCategory parentCategory, string categoryCd)
      : base(parentCategory, categoryCd)
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
    }

    /// <summary>
    /// Populates the values used in matching category data to error supressions.
    /// </summary>
    /// <returns></returns>
    protected override bool SetErrorSuppressValues()
    {
      if (EmParameters.MatsSorbentTrapRecord != null)
      {
        long facId = CheckEngine.FacilityID;
        string locationName = EmParameters.MatsSorbentTrapRecord.LocationName;
        DateTime? matchTimeValue = CheckEngine.ReportingPeriod.BeganDate;

        ErrorSuppressValues = new cErrorSuppressValues(facId, locationName, null, null, "QUARTER", matchTimeValue);
        return true;
      }
      else
        return false;
    }

    #endregion

  }

}
