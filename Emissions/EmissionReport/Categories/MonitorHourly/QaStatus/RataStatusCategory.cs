using System;
using System.Data;
using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Data.Ecmps.Dbo.View;
using ECMPS.Checks.Em.Parameters;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Extensions;
using ECMPS.ErrorSuppression;


namespace ECMPS.Checks.EmissionsReport
{

    public class cRataStatusCategory : cCategoryHourly
    {

        #region Constructors

        public cRataStatusCategory(cCategory parentCategory, string categoryCd) : base(parentCategory, categoryCd)
        {
        }

        #endregion


        #region Base Class Overrides

        protected override void FilterData()
        {
            SetCheckParameter("Location_Attribute_Records_By_Hour_Location",
                              FilterRanged("LocationAttribute", LocationAttribute, CurrentOpDate, CurrentOpHour, CurrentMonLocId, "Begin_Date", "End_Date"),
                              eParameterDataType.DataView);

            DataView[] OperatingHoursByLocation = (DataView[])EmissionParameters.OperatingHoursByLocation.Value;

            SetCheckParameter("Hourly_Operating_Data_Records_for_Location",
                        OperatingHoursByLocation[CurrentMonLocPos],
                        eParameterDataType.DataView);

            SetCheckParameter("Test_Extension_Exemption_Records",
                               FilterLocation("TEERecords", TEERecords, CurrentMonLocId),
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
            RecordIdentifier = "Monitor System ID " + EmParameters.QaStatusSystemIdentifier;
        }

        protected override bool SetErrorSuppressValues()
        {
            VwCeMpMonitorLocationRow currentMonitorPlanLocationRecord = EmParameters.CurrentMonitorPlanLocationRecord;

            if (currentMonitorPlanLocationRecord != null)
            {
                long facId = CheckEngine.FacilityID;
                string locationName = currentMonitorPlanLocationRecord.LocationName;
                string matchDataValue = EmParameters.QaStatusHourlyParameterCode;
                DateTime? matchTimeValue = EmParameters.CurrentOperatingDatehour;

                ErrorSuppressValues = new cErrorSuppressValues(facId, locationName, "PARAM", matchDataValue, "HOUR", matchTimeValue);
                return true;
            }
            else
                return false;
        }

        #endregion

    }

}
