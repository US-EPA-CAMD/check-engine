using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Data.Ecmps.CheckEm.Function;
using ECMPS.Checks.Data.Ecmps.Dbo.View;
using ECMPS.Checks.Em.Parameters;
using ECMPS.Checks.Parameters;

using ECMPS.ErrorSuppression;


namespace ECMPS.Checks.EmissionsReport
{

    public class Nsps4tSummaryDataCategory : cCategoryHourly
    {

        /// <summary>
        /// The NSPS4T Summary Data Category class always useds "NSPS4T" as the category code for the class.  No setup speicifc
        /// to the category occurs.
        /// 
        /// The category contains its own run check loop.
        /// </summary>
        /// <param name="parentCategory"></param>
        public Nsps4tSummaryDataCategory(cSummaryValueInitializationCategory parentCategory) : base(parentCategory, "NSPS4T")
        {
        }


        /// <summary>
        /// Executes the NSPS4T Summary Data checks for each location in the monitoring plan.
        /// 
        /// Note that the created of the category object occurs in cEmissionsReportProcess for must check categories.  This category
        /// only runs once so creating it here works.
        /// </summary>
        /// <param name="summaryValueInitializationCategory">The parent Summary Value Initialization Category object.</param>
        /// <param name="MonitorLocationView">The view containing the monitor location rows for the MP.</param>
        /// <returns>Returns true if the checks were successfully executed.</returns>
        public static bool ExecuteChecks(cSummaryValueInitializationCategory summaryValueInitializationCategory, DataView MonitorLocationView)
        {
            bool result = true;


            /* Create the NSPS4T Summary Data check category object */
            Nsps4tSummaryDataCategory nsps4tSummaryDataCategory = new Nsps4tSummaryDataCategory(summaryValueInitializationCategory);

            try
            {
                string errorMessage = null;

                if (nsps4tSummaryDataCategory.InitCheckBands(summaryValueInitializationCategory.CheckEngine.DbAuxConnection, ref errorMessage))
                {
                    /* Setup non changing check parameters needed by the checks */
                    DataSet sourceDataSet = summaryValueInitializationCategory.Process.SourceData;
                    EmParameters.Nsps4tSummaryRecords = new CheckDataView<Nsps4tSummary>(new DataView(sourceDataSet.Tables["Nsps4tSummary"]));
                    EmParameters.Nsps4tCompliancePeriodRecords = new CheckDataView<Nsps4tCompliancePeriod>(new DataView(sourceDataSet.Tables["Nsps4tCompliancePeriod"]));
                    EmParameters.Nsps4tAnnualRecords = new CheckDataView<Nsps4tAnnual>(new DataView(sourceDataSet.Tables["Nsps4tAnnual"]));

                    /* Loop through each location in the MP */
                    for (int MonitorLocationDex = 0; MonitorLocationDex < MonitorLocationView.Count; MonitorLocationDex++)
                    {
                        /* Setup loop dependent check parameters needed by the checks */
                        EmParameters.CurrentMonitorPlanLocationRecord = new VwCeMpMonitorLocationRow(MonitorLocationView[MonitorLocationDex]);
                        EmParameters.EmLocationProgramRecords 
                            = new CheckDataView<VwMpLocationProgramRow>(nsps4tSummaryDataCategory.FilterLocation("LocationProgram", 
                                                                                                                 nsps4tSummaryDataCategory.LocationProgram,
                                                                                                                 EmParameters.CurrentMonitorPlanLocationRecord.MonLocId));

                        /* Set CurrentMonLocId in the category object */
                        nsps4tSummaryDataCategory.CurrentMonLocId = EmParameters.CurrentMonitorPlanLocationRecord.MonLocId;


                        /* Execute the actual checks */
                        if (!nsps4tSummaryDataCategory.ProcessChecks())
                        {
                            result = false;
                        }

                        /* Erase any parameters created while processing the check category for this location */
                        nsps4tSummaryDataCategory.EraseParameters();
                    }
                }
                else
                {
                    // Update errors with the returned init check band error.
                    summaryValueInitializationCategory.Process.UpdateErrors(string.Format("Category: {0}  Message: {1}", nsps4tSummaryDataCategory.CategoryCd, errorMessage));
                    result = false;
                }
            }
            finally
            {
                nsps4tSummaryDataCategory = null;
            }


            return result;
        }



        public static bool InitSourceData(cEmissionsReportProcess emissionsReportProcess)
        {
            bool result = true;

            string monPlanId = emissionsReportProcess.CheckEngine.MonPlanId;
            int rptPeriodId = emissionsReportProcess.CheckEngine.RptPeriodId.Value;

            result = InitSourceDataDo("Nsps4tSummary", "ORIS_CODE, LOCATION_NAME", monPlanId, rptPeriodId, emissionsReportProcess) && result;
            result = InitSourceDataDo("Nsps4tCompliancePeriod", "ORIS_CODE, LOCATION_NAME, END_YEAR desc, END_MONTH desc", monPlanId, rptPeriodId, emissionsReportProcess) && result;
            result = InitSourceDataDo("Nsps4tAnnual", "ORIS_CODE, LOCATION_NAME", monPlanId, rptPeriodId, emissionsReportProcess) && result;

            return result;
        }


        private static bool InitSourceDataDo(string sourceName, string sourceSort, string monPlanId, int rptPeriodId,
                                             cEmissionsReportProcess emissionsReportProcess)
        {
            bool result = true;

            try
            {
                string sql = string.Format("select * from CheckEm.{0}('{2}', {3}) order by {1}", sourceName, sourceSort, monPlanId, rptPeriodId);

                SqlConnection sqlConnection = emissionsReportProcess.CheckEngine.DbDataConnection.SQLConnection;
                SqlDataAdapter Adapter = new SqlDataAdapter(sql, sqlConnection);
                DataSet sourceDataSet = emissionsReportProcess.SourceData;
                DataTable Table = new DataTable(sourceName);
                
                // this defaults to 30 seconds if we don't override it
                if (Adapter.SelectCommand != null)
                    Adapter.SelectCommand.CommandTimeout = emissionsReportProcess.CheckEngine.CommandTimeout;

                Adapter.Fill(Table);
                sourceDataSet.Tables.Add(Table);

                result = true;
            }
            catch (Exception ex)
            {
                // Update errors with the returned init check band error.
                emissionsReportProcess.UpdateErrors(string.Format("AddTable[{0}]: {1}", sourceName, ex.Message));
                result = false;
            }

            return result;
        }


        /// <summary>
        /// Sets the error suppression information for the NSPS4T Summary category.  The suppression can be monitoring plan
        /// and quarter specific, but are not location specific.
        /// </summary>
        /// <returns></returns>
        protected override bool SetErrorSuppressValues()
        {
            long facId = CheckEngine.FacilityID;
            string locationName = EmParameters.CurrentMonitorPlanLocationRecord.LocationName;
            DateTime? matchTimeValue = CheckEngine.ReportingPeriod.BeganDate;

            ErrorSuppressValues = new cErrorSuppressValues(facId, locationName, null, null, "QUARTER", matchTimeValue);

            return true;
        }


        #region Other Overrides

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

        #endregion

    }

}
