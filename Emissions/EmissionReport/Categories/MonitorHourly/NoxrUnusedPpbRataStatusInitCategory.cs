using System;

using ECMPS.Checks.Em.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Extensions;
using ECMPS.ErrorSuppression;


namespace ECMPS.Checks.EmissionsReport
{

    /// <summary>
    /// Initializes the RATA status processing for the primary or primary bypass NOX system for NOXR that was not reported.
    /// </summary>
    public class cNoxrUnusedPpbRataStatusInitCategory : cCategoryHourly
    {

        #region Constructors

        public cNoxrUnusedPpbRataStatusInitCategory(cNoxrUnusedPpbMonitorHourlyCategory parentCategory)
            : base(parentCategory, "NXPPBRI")
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
            string locationName = (EmParameters.CurrentMonitorPlanLocationRecord != null) ? EmParameters.CurrentMonitorPlanLocationRecord.LocationName : null;
            string parameterCd = (EmParameters.CurrentNoxrPrimaryOrPrimaryBypassMhvRecord != null) ? EmParameters.CurrentNoxrPrimaryOrPrimaryBypassMhvRecord.ParameterCd : null;
            string modcCd = (EmParameters.CurrentNoxrPrimaryOrPrimaryBypassMhvRecord != null) ? EmParameters.CurrentNoxrPrimaryOrPrimaryBypassMhvRecord.ModcCd : null;

            RecordIdentifier = string.Format("Location {0}, Parameter {1}, MODC {2}", locationName, parameterCd, modcCd);
        }

        protected override bool SetErrorSuppressValues()
        {
            if (EmParameters.CurrentMonitorPlanLocationRecord != null && EmParameters.CurrentHourlyOpRecord != null && EmParameters.CurrentMonitorPlanLocationRecord != null)
            {
                long facId = CheckEngine.FacilityID;
                string locationName = EmParameters.CurrentMonitorPlanLocationRecord.LocationName;
                string matchDataValue = EmParameters.CurrentNoxrPrimaryOrPrimaryBypassMhvRecord.ParameterCd;
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
