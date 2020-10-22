using System;
using System.Collections.Generic;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Extensions;
using ECMPS.ErrorSuppression;

namespace ECMPS.Checks.EmissionsReport
{
    public class cNoxrDerivedHourlyCategory : cCategoryHourly
    {

        #region Constructors

        public cNoxrDerivedHourlyCategory(cCheckEngine ACheckEngine,
                                          cEmissionsReportProcess AHourlyEmissionsData,
                                          cOperatingHourCategory AOperatingHourCategory)
          : base(AOperatingHourCategory,
                 "NOXRDH",
                 "DERIVED_HRLY_VALUE",
                 AHourlyEmissionsData.DerivedHourlyValueNoxr,
                 "NOxR_Derived_Hourly_Value_Records_By_Hour_Location",
                 "ADJUSTED_HRLY_VALUE",
                 "DERIVED",
                 "NOXR",
                 null,
                 PrimaryBypassRelatedSystems(AHourlyEmissionsData))
        {
        }


        #region Helper Method

        /// <summary>
        /// Returns an array of MON_SYS_ID where the MON_SYS_ID are for active primary or primary-bypass systems during the 
        /// quarter.  Returns null no active primary-bypass systems were active during the quarter.
        /// </summary>
        /// <param name="process">The process in which the category is running.</param>
        /// <returns>The array of primary/primary-bypass MON_LOC_ID lists.</returns>
        private static List<string>[] PrimaryBypassRelatedSystems(cProcess process)
        {
            List<string>[] result;

            DataView monitorLocationView = new DataView(process.SourceData.Tables["MonitorLocation"]);

            if (monitorLocationView.Count > 0)
            {
                result = new List<string>[monitorLocationView.Count];

                bool primaryBypassExists = false;

                string reportingPeriodBegan = process.CheckEngine.ReportingPeriod.BeganDate.ToString("MM/dd/yyyy");
                string reportingPeriodEnded = process.CheckEngine.ReportingPeriod.EndedDate.ToString("MM/dd/yyyy");
                DataView monitorSystemView;

                monitorLocationView.Sort = "Mon_Loc_Id"; // Must occur to match location position used by emission report evaluations.

                for (int locationDex = 0; locationDex < monitorLocationView.Count; locationDex++)
                {
                    // Filter systems to location, system type NOX, designation P (primary) or PB (primary-bypass), and active during the quarter.
                    monitorSystemView = new DataView(process.SourceData.Tables["MonitorSystem"],
                                                     $"Mon_Loc_Id = '{monitorLocationView[locationDex]["Mon_Loc_Id"].AsString()}' and Sys_Type_Cd = 'NOX' and Sys_Designation_Cd in ('P', 'PB') and Begin_Datehour <= #{reportingPeriodEnded}# and (End_Datehour is null or End_Datehour >= #{reportingPeriodBegan}#)",
                                                     "",
                                                     DataViewRowState.CurrentRows);

                    // Initialize array element.
                    result[locationDex] = new List<string>();

                    // Add MON_SYS_ID values to the list for the specific location.
                    foreach (DataRowView monitorSystemRow in monitorSystemView)
                    {
                        if (monitorSystemRow["Sys_Designation_Cd"].AsString() == "PB") primaryBypassExists = true;

                        result[locationDex].Add(monitorSystemRow["Mon_Sys_Id"].AsString());
                    }
                }

                // If an active primary-bypass system was not active during the quarter for any location, return the list as a null.
                if (!primaryBypassExists)
                {
                    result = null;
                }
            }
            else
                result = null;

            return result;
        }

        #endregion

        #endregion


        #region Base Class Overrides

        protected override void FilterData()
        {
            // I do not believe this parameter is used (djw2)
            //SetCheckParameter("NORX_Monitor_Default_Records_By_Hour_Location",
            //                  FilterRanged("MonitorDefaultNorx", MonitorDefaultNorx, FCurrentOpDate, FCurrentOpHour, FCurrentMonLocId),
            //                  eParameterDataType.DataView);      
        }

        //public override void FilterPrimary()
        //{
        //  FilterHourly("DerivedHourlyValueNoxr", DerivedHourlyValueNoxr, FCurrentOpDate, FCurrentOpHour, FCurrentMonLocId);
        //}

        protected override int[] GetDataBorderModcList()
        {
            int[] ModcList = new int[9];

            ModcList[0] = 1;
            ModcList[1] = 2;
            ModcList[2] = 3;
            ModcList[3] = 4;
            ModcList[4] = 14;
            ModcList[5] = 21;
            ModcList[6] = 22;
            ModcList[7] = 53;
            ModcList[8] = 54;

            return ModcList;
        }

        protected override int[] GetQualityAssuranceHoursModcList()
        {
            int[] ModcList = new int[7];

            ModcList[0] = 1;
            ModcList[1] = 2;
            ModcList[2] = 4;
            ModcList[3] = 14;
            ModcList[4] = 21;
            ModcList[5] = 22;
            ModcList[6] = 53;

            return ModcList;
        }

        protected override void SetRecordIdentifier()
        {
        }

        protected override bool SetErrorSuppressValues()
        {
            DataRowView currentLocation = GetCheckParameter("Current_Monitor_Plan_Location_Record").ValueAsDataRowView();
            DataRowView derivedHourlyRecord = GetCheckParameter("Current_NoxR_Derived_Hourly_Record").ValueAsDataRowView();

            if (currentLocation != null && derivedHourlyRecord != null)
            {
                long facId = CheckEngine.FacilityID;
                string locationName = currentLocation["LOCATION_NAME"].AsString();
                string matchDataValue = derivedHourlyRecord["PARAMETER_CD"].AsString();
                DateTime? matchTimeValue = derivedHourlyRecord.AsDateTime("BEGIN_DATE", "BEGIN_HOUR");

                ErrorSuppressValues = new cErrorSuppressValues(facId, locationName, "PARAM", matchDataValue, "HOUR", matchTimeValue);
                return true;
            }
            else
                return false;
        }

        #endregion

    }
}
