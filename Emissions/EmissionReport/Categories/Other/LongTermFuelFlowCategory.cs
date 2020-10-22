using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Extensions;
using ECMPS.ErrorSuppression;


namespace ECMPS.Checks.EmissionsReport
{
    public class cLongTermFuelFlowCategory : cCategoryHourly
    {
        #region Private Fields

        //string LTFFID;

        #endregion

        #region Constructors

        public cLongTermFuelFlowCategory( cCheckEngine ACheckEngine,
                                    cEmissionsReportProcess AHourlyEmissionsData,
                                    cSummaryValueInitializationCategory ASummaryValueInitializationCategory )
            : base( ACheckEngine,
                 (cEmissionsReportProcess)AHourlyEmissionsData,
                 (cCategory)ASummaryValueInitializationCategory,
                 "LTFF" )
        {
            //Initializing. Will be reset in FilterData()            
            // LTFFID = "";

        }

        #endregion

        #region Base Class Overrides

        protected override void FilterData()
        {
            SetCheckParameter( "Fuel_Type_Reality_Checks_for_GCV_Cross_Check_Table", SourceTables()["CrossCheck_FuelTypeRealityChecksforGCV"].DefaultView, eParameterDataType.DataView );
            SetCheckParameter( "Fuel_Type_Warning_Levels_for_GCV_Cross_Check_Table", SourceTables()["CrossCheck_FuelTypeWarningLevelsforGCV"].DefaultView, eParameterDataType.DataView );
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
            DataRowView CurrentLTFF = (DataRowView)GetCheckParameter( "Current_LTFF_Record" ).ParameterValue;
            if( CurrentLTFF != null )
                RecordIdentifier = "MonitoringSystemID " + cDBConvert.ToString( CurrentLTFF["system_identifier"] );
        }

        protected override bool SetErrorSuppressValues()
        {
            DataRowView currentLocation = GetCheckParameter( "Current_Monitor_Plan_Location_Record" ).ValueAsDataRowView();

            if( currentLocation != null )
            {
                long facId = CheckEngine.FacilityID;
                string locationName = currentLocation["LOCATION_NAME"].AsString();
                DateTime? matchTimeValue = CheckEngine.ReportingPeriod.BeganDate;

                ErrorSuppressValues = new cErrorSuppressValues( facId, locationName, null, null, "QUARTER", matchTimeValue );
                return true;
            }
            else
                return false;
        }

        #endregion
    }
}
