using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Data.Ecmps.CheckEm.Function;
using ECMPS.Checks.Data.Ecmps.Dbo.View;
using ECMPS.Checks.Em.Parameters;
using ECMPS.Checks.EmissionsReport;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Enumerations;
using ECMPS.Definitions.Extensions;


namespace ECMPS.Checks.EmissionsChecks
{

    /// <summary>
    /// Contains the checks methods for NSPS4T Summary, Compliance Period and Annual (fourth quarter) data.
    /// </summary>
    public class Nsps4tChecks : cEmissionsChecks
    {

        #region Constructors

        public Nsps4tChecks(cEmissionsReportProcess emissionReportProcess) : base(emissionReportProcess)
        {
            CheckProcedures = new dCheckProcedure[6];

            CheckProcedures[1] = new dCheckProcedure(NSPS4T1);
            CheckProcedures[2] = new dCheckProcedure(NSPS4T2);
            CheckProcedures[3] = new dCheckProcedure(NSPS4T3);
            CheckProcedures[4] = new dCheckProcedure(NSPS4T4);
            CheckProcedures[5] = new dCheckProcedure(NSPS4T5);
        }

        #endregion


        #region Checks 1 - 10

        /// <summary>
        /// Determines wheter NSPS4T Summary rows are allowed based on whether a NSPS4T program is active
        /// and limits the number of rows per location to one.
        /// </summary>
        /// <param name="category">Check Category Object</param>
        /// <param name="log">Indicates whether to log results. (obsolete)</param>
        /// <returns>Returns an error message if the check fails to run correctly.</returns>
        public static string NSPS4T1(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                EmParameters.Nsps4tCurrentSummaryRecord = null;


                // Get all NSPS4T Summary records for the current location.
                CheckDataView<Nsps4tSummary> currentSummaryRecords
                    = EmParameters.Nsps4tSummaryRecords.FindRows(new cFilterCondition("MON_LOC_ID", EmParameters.CurrentMonitorPlanLocationRecord.MonLocId));


                // If the location is a unit
                if ((EmParameters.CurrentMonitorPlanLocationRecord.LocationName.Length < 2) ||
                    EmParameters.CurrentMonitorPlanLocationRecord.LocationName.Substring(0, 2).NotInList("CS,CP,MS,MP"))
                {

                    // Get first active NSPS4T program record.  There should only be one.
                    VwMpLocationProgramRow currentLocationProgramRecord
                        = EmParameters.EmLocationProgramRecords.FindRow(new cFilterCondition("PRG_CD", "NSPS4T"),
                                                                        new cFilterCondition("CLASS", "A"),
                                                                        new cFilterCondition("UNIT_MONITOR_CERT_BEGIN_DATE", eFilterConditionRelativeCompare.LessThanOrEqual,
                                                                                             EmParameters.CurrentReportingPeriodEndDate.Value, eNullDateDefault.Min),
                                                                        new cFilterCondition("END_DATE", eFilterConditionRelativeCompare.GreaterThanOrEqual,
                                                                                             EmParameters.CurrentReportingPeriodBeginDate.Value, eNullDateDefault.Max));

                    // NSPS4T is not active during the quarter
                    if (currentLocationProgramRecord == null)
                    {
                        if (currentSummaryRecords.Count > 0)
                            category.CheckCatalogResult = "A"; // Cannot have NSPS4T records if NSPS4t is not active.
                    }

                    // NSPS4T is active during the quarter
                    else
                    {
                        if (currentSummaryRecords.Count > 1)
                        {
                            category.CheckCatalogResult = "B"; // Cannot have more than one summary record per location.
                        }
                        else if (currentSummaryRecords.Count == 1)
                        {
                            EmParameters.Nsps4tCurrentSummaryRecord = currentSummaryRecords[0];
                        }
                    }
                }
                // if the location is a stack or pipe
                else
                {
                    if (currentSummaryRecords.Count > 0)
                        category.CheckCatalogResult = "C";
                }
            }
            catch (Exception ex)
            {
                returnVal = category.CheckEngine.FormatError(ex);
            }

            return returnVal;
        }


        /// <summary>
        /// Determines whether NSPS4T Compliance Period rows are expected based on the No Compliance Period Ended Indicator.
        /// If the indicator is set to 1, then no compliance period rows are expected.  Otherwise, at least one and no more
        /// than three compliance period rows are expected.
        /// </summary>
        /// <param name="category">Check Category Object</param>
        /// <param name="log">Indicates whether to log results. (obsolete)</param>
        /// <returns>Returns an error message if the check fails to run correctly.</returns>
        public static string NSPS4T2(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                EmParameters.Nsps4tCurrentCompliancePeriod1Record = null;
                EmParameters.Nsps4tCurrentCompliancePeriod2Record = null;
                EmParameters.Nsps4tCurrentCompliancePeriod3Record = null;


                // Only continue checking if an NSPS4T Summary row was previously located.
                if ( EmParameters.Nsps4tCurrentSummaryRecord != null)
                {
                    // Get all NSPS4T Compliance Period records for the current NSPS4T Summary record.
                    CheckDataView<Nsps4tCompliancePeriod> currentCompliancePeriodRecords
                        = EmParameters.Nsps4tCompliancePeriodRecords.FindRows(new cFilterCondition("NSPS4T_SUM_ID", EmParameters.Nsps4tCurrentSummaryRecord.Nsps4tSumId));

                    // No Compliance Period Ended
                    if (EmParameters.Nsps4tCurrentSummaryRecord.NoPeriodEndedInd == 1)
                    {
                        if (currentCompliancePeriodRecords.Count > 0)
                            category.CheckCatalogResult = "A"; // Cannot have compliance period rows if summary indicates no compliance periods exist.
                    }

                    // At least one compliance period ended
                    else
                    {
                        if (currentCompliancePeriodRecords.Count > 3)
                        {
                            category.CheckCatalogResult = "B"; // Cannot have more than three compliance period rows.
                        }
                        else if (currentCompliancePeriodRecords.Count == 0)
                        {
                            category.CheckCatalogResult = "C"; // Must have at least one compliance period row.
                        }
                        else
                        {
                            EmParameters.Nsps4tCurrentCompliancePeriod1Record = currentCompliancePeriodRecords[0];
                            if (currentCompliancePeriodRecords.Count >= 2)
                                EmParameters.Nsps4tCurrentCompliancePeriod2Record = currentCompliancePeriodRecords[1];
                            if (currentCompliancePeriodRecords.Count >= 3)
                                EmParameters.Nsps4tCurrentCompliancePeriod3Record = currentCompliancePeriodRecords[2];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                returnVal = category.CheckEngine.FormatError(ex);
            }

            return returnVal;
        }


        /// <summary>
        /// Determines whether NSPS4T Annual (Q4) rows are allowed and whether the correct number were reported.
        /// Annual rows are allowed only during the 4th quarter and only one is allowed per location.
        /// </summary>
        /// <param name="category">Check Category Object</param>
        /// <param name="log">Indicates whether to log results. (obsolete)</param>
        /// <returns>Returns an error message if the check fails to run correctly.</returns>
        public static string NSPS4T3(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                EmParameters.Nsps4tCurrentAnnualRecord = null;


                // Only continue checking if an NSPS4T Summary row was previously located.
                if (EmParameters.Nsps4tCurrentSummaryRecord != null)
                {
                    // Get all NSPS4T Annual records for the current NSPS4T Summary record.
                    CheckDataView<Nsps4tAnnual> currentAnnualRecords
                        = EmParameters.Nsps4tAnnualRecords.FindRows(new cFilterCondition("NSPS4T_SUM_ID", EmParameters.Nsps4tCurrentSummaryRecord.Nsps4tSumId));

                    // This is not a 4th quarter emissions report.
                    if (EmParameters.CurrentReportingPeriodQuarter != 4)
                    {
                        if (currentAnnualRecords.Count > 0)
                            category.CheckCatalogResult = "A"; // Cannot report NSPS4T Annual rows outside of Q4.
                    }

                    // This is a 4th quarter emissions report.
                    else
                    {
                        if (currentAnnualRecords.Count > 1)
                        {
                            category.CheckCatalogResult = "B"; // Cannot report more than one NSPS4T Annual row.
                        }
                        else if (currentAnnualRecords.Count == 0)
                        {
                            category.CheckCatalogResult = "C";
                        }
                        else
                        {
                            EmParameters.Nsps4tCurrentAnnualRecord = currentAnnualRecords[0];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                returnVal = category.CheckEngine.FormatError(ex);
            }

            return returnVal;
        }


        /// <summary>
        /// Ensures that the reported electrical load matches the load defined for the emissions standard, if one exists.
        /// </summary>
        /// <param name="category">Check Category Object</param>
        /// <param name="log">Indicates whether to log results. (obsolete)</param>
        /// <returns>Returns an error message if the check fails to run correctly.</returns>
        public static string NSPS4T4(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                // Only continue checking if an NSPS4T Summary row was previously located.
                if (EmParameters.Nsps4tCurrentSummaryRecord != null)
                {
                    // If the emission standard electrical load is not null, it and the reported electrical load must have the same value.
                    if ((EmParameters.Nsps4tCurrentSummaryRecord.EmissionStandardLoadCd != null) &&
                        (EmParameters.Nsps4tCurrentSummaryRecord.EmissionStandardLoadCd != EmParameters.Nsps4tCurrentSummaryRecord.ElectricalLoadCd))
                    {
                        category.CheckCatalogResult = "A";
                    }
                }
            }
            catch (Exception ex)
            {
                returnVal = category.CheckEngine.FormatError(ex);
            }

            return returnVal;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="category">Check Category Object</param>
        /// <param name="log">Indicates whether to log results. (obsolete)</param>
        /// <returns>Returns an error message if the check fails to run correctly.</returns>
        public static string NSPS4T5(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                EmParameters.Nsps4tInvalidCo2EmissionRateUomList = "";


                // Only continue checking if an NSPS4T Summary row was previously located.
                if (EmParameters.Nsps4tCurrentSummaryRecord != null)
                {
                    // Only continue checking if the emission standard's rate UOM is not null.
                    if (EmParameters.Nsps4tCurrentSummaryRecord.EmissionStandardUomCd != null)
                    {
                        /* Check each compliance period row, ensuring that the rate UOM is either null or matches the standard's rate UOM.  */

                        // Compliancre Record 1
                        if (EmParameters.Nsps4tCurrentCompliancePeriod1Record != null)
                        {
                            if ((EmParameters.Nsps4tCurrentCompliancePeriod1Record.Co2EmissionRateUomCd != null) &
                                (EmParameters.Nsps4tCurrentCompliancePeriod1Record.Co2EmissionRateUomCd != EmParameters.Nsps4tCurrentSummaryRecord.EmissionStandardUomCd))
                            {
                                EmParameters.Nsps4tInvalidCo2EmissionRateUomList 
                                    = EmParameters.Nsps4tInvalidCo2EmissionRateUomList.ListAdd(EmParameters.Nsps4tCurrentCompliancePeriod1Record.Co2EmissionRateUomLabel);
                            }
                        }

                        // Compliancre Record 2
                        if (EmParameters.Nsps4tCurrentCompliancePeriod2Record != null)
                        {
                            if ((EmParameters.Nsps4tCurrentCompliancePeriod2Record.Co2EmissionRateUomCd != null) &
                            (EmParameters.Nsps4tCurrentCompliancePeriod2Record.Co2EmissionRateUomCd != EmParameters.Nsps4tCurrentSummaryRecord.EmissionStandardUomCd))
                            {
                                EmParameters.Nsps4tInvalidCo2EmissionRateUomList 
                                    = EmParameters.Nsps4tInvalidCo2EmissionRateUomList.ListAdd(EmParameters.Nsps4tCurrentCompliancePeriod2Record.Co2EmissionRateUomLabel);
                            }
                        }

                        // Compliancre Record 3
                        if (EmParameters.Nsps4tCurrentCompliancePeriod3Record != null)
                        {
                            if ((EmParameters.Nsps4tCurrentCompliancePeriod3Record.Co2EmissionRateUomCd != null) &
                            (EmParameters.Nsps4tCurrentCompliancePeriod3Record.Co2EmissionRateUomCd != EmParameters.Nsps4tCurrentSummaryRecord.EmissionStandardUomCd))
                            {
                                EmParameters.Nsps4tInvalidCo2EmissionRateUomList
                                    = EmParameters.Nsps4tInvalidCo2EmissionRateUomList.ListAdd(EmParameters.Nsps4tCurrentCompliancePeriod3Record.Co2EmissionRateUomLabel);
                            }
                        }


                        // If items where added to tne Invalid UOM List, the return an error.
                        if (EmParameters.Nsps4tInvalidCo2EmissionRateUomList != "")
                        {
                            EmParameters.Nsps4tInvalidCo2EmissionRateUomList = EmParameters.Nsps4tInvalidCo2EmissionRateUomList.FormatList();// Format list to include an 'and' instead of the last comma.
                            category.CheckCatalogResult = "A";
                        }
                    }
                    // Ensure that the compliance period UOM all match for the MODUS standard.
                    else if (EmParameters.Nsps4tCurrentSummaryRecord.EmissionStandardCd == "MODUS")
                    {
                        // If compliance period 1 does not exist then no compliance period exist, so no further checking is need.
                        if (EmParameters.Nsps4tCurrentCompliancePeriod1Record != null)
                        {
                            // Compare the UOM for compliance period 2 and 3 to compliance period 1 if they exist.
                            if ( (EmParameters.Nsps4tCurrentCompliancePeriod2Record != null) &&
                                 (EmParameters.Nsps4tCurrentCompliancePeriod2Record.Co2EmissionRateUomCd != EmParameters.Nsps4tCurrentCompliancePeriod1Record.Co2EmissionRateUomCd)
                                 ||
                                 (EmParameters.Nsps4tCurrentCompliancePeriod3Record != null) &&
                                 (EmParameters.Nsps4tCurrentCompliancePeriod3Record.Co2EmissionRateUomCd != EmParameters.Nsps4tCurrentCompliancePeriod1Record.Co2EmissionRateUomCd) )
                            {
                                category.CheckCatalogResult = "B";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                returnVal = category.CheckEngine.FormatError(ex);
            }

            return returnVal;
        }

        #endregion

    }

}
