using System;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Em.Parameters;
using ECMPS.ErrorSuppression;


namespace ECMPS.Checks.EmissionsReport
{

    public class HourlyApportionmentVerificatonCategory : cCategoryHourly
    {

        #region Constructors

        /// <summary>
        /// Creates a category with a specific parent category and category code.
        /// </summary>
        /// <param name="parentCategory">The parent category of the new category.</param>
        /// <param name="categoryCd">The category code of the new category.</param>
        /// <param name="componentIdentifier">The component identifier associated with the test.</param>
        /// <param name="componentTypeCd">The type of the component associated with the test.</param>
        /// <param name="testDateHour">The date and hour of the test.</param>
        public HourlyApportionmentVerificatonCategory(cCategory parentCategory) 
            : base(parentCategory, "APPVERI")
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
            RecordIdentifier = EmParameters.CurrentOperatingDatehour.ToString();
        }

        protected override bool SetErrorSuppressValues()
        {
            ErrorSuppressValues = new cErrorSuppressValues(CheckEngine.FacilityID, null, "MONPLAN", CheckEngine.MonPlanId, "HOUR", EmParameters.CurrentOperatingDatehour);
            return true;
        }

        #endregion

    }

}
