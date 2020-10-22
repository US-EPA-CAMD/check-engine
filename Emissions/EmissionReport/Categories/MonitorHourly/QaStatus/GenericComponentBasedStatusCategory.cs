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

  public class GenericComponentBasedStatusCategory : cCategoryHourly
  {

    #region Constructors

    /// <summary>
    /// Creates a category with a specific parent category and category code.
    /// </summary>
    /// <param name="parentCategory">The parent category of the new category.</param>
    /// <param name="categoryCd">The category code of the new category.</param>
    /// <param name="parameterCd">The parameter code of the associated monitor or derived hourly data.</param>
    public GenericComponentBasedStatusCategory(cCategory parentCategory, string categoryCd, string parameterCd)
      : base(parentCategory, categoryCd)
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
      return null;
    }

    protected override void SetRecordIdentifier()
    {
      if (EmParameters.QaStatusSystemIdentifier != null)
        RecordIdentifier = "Component ID " + EmParameters.QaStatusComponentIdentifier;
      else
        RecordIdentifier = "Component ID null";
    }

    protected override bool SetErrorSuppressValues()
    {
      if (EmParameters.CurrentMonitorPlanLocationRecord != null)
      {
        ErrorSuppressValues 
          = new cErrorSuppressValues
            (
              CheckEngine.FacilityID, 
              EmParameters.CurrentMonitorPlanLocationRecord.LocationName,
              "PARAM", ParameterCd,
              "HOUR", EmParameters.CurrentDateHour.AsStartDateTime()
            );

        return true;
      }
      else
        return false;
    }

    #endregion

  }

}
