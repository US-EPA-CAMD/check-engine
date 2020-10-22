using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Data.Ecmps.CheckEm.Function;
using ECMPS.Checks.Data.Ecmps.Dbo.View;
using ECMPS.Checks.Em.Parameters;
using ECMPS.Checks.EmissionsReport;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Enumerations;
using ECMPS.Definitions.Extensions;

namespace ECMPS.Checks.EmissionsChecks
{
    public class cMATSMonitorHourlyValueChecks : cEmissionsChecks
    {

        #region Constructors

        public cMATSMonitorHourlyValueChecks(cEmissionsReportProcess emissionReportProcess)
            : base(emissionReportProcess)
        {
            CheckProcedures = new dCheckProcedure[21];

            CheckProcedures[1] = new dCheckProcedure(MATSMHV1);
            CheckProcedures[2] = new dCheckProcedure(MATSMHV2);
            CheckProcedures[3] = new dCheckProcedure(MATSMHV3);
            CheckProcedures[4] = new dCheckProcedure(MATSMHV4);
            CheckProcedures[5] = new dCheckProcedure(MATSMHV5);
            CheckProcedures[6] = new dCheckProcedure(MATSMHV6);
            CheckProcedures[7] = new dCheckProcedure(MATSMHV7);
            CheckProcedures[8] = new dCheckProcedure(MATSMHV8);
            CheckProcedures[9] = new dCheckProcedure(MATSMHV9);
            CheckProcedures[10] = new dCheckProcedure(MATSMHV10);
            CheckProcedures[11] = new dCheckProcedure(MATSMHV11);
            CheckProcedures[12] = new dCheckProcedure(MATSMHV12);
            CheckProcedures[13] = new dCheckProcedure(MATSMHV13);
            CheckProcedures[14] = new dCheckProcedure(MATSMHV14);
            CheckProcedures[15] = new dCheckProcedure(MATSMHV15);
            CheckProcedures[16] = new dCheckProcedure(MATSMHV16);
            CheckProcedures[17] = new dCheckProcedure(MATSMHV17);
            CheckProcedures[18] = new dCheckProcedure(MATSMHV18);
            CheckProcedures[19] = new dCheckProcedure(MATSMHV19);
            CheckProcedures[20] = new dCheckProcedure(MATSMHV20);
        }

        #endregion

        #region Checks 1-10

        /// <summary>
        /// MATSMHV1: MATS HgC: Initialize
        /// 
        /// Assumes that MATSHOD-10 either set MatsHgcMhvRecord to an actual record, or set MatsHgcMhvChecksNeeded to false.
        /// Setting MatsHgcMhvChecksNeeded to false will prevent the running of MATS MHV checks for HgC, including MATSMHV-1.
        /// </summary>
        /// <param name="Category"></param>
        /// <param name="Log"></param>
        /// <returns></returns>
        public static string MATSMHV1(cCategory Category, ref bool Log)
        {
            string ReturnVal = "";

            try
            {
                EmParameters.CurrentMhvParameter = "HGC";
                EmParameters.MatsMhvRecord = EmParameters.MatsHgcMhvRecord;
                EmParameters.MatsMhvSorbentTraps = null;


                if ((EmParameters.MatsHgMethodRecord.MethodCd == "ST") ||
                    ((EmParameters.MatsHgMethodRecord.MethodCd == "CEMST") && (EmParameters.MatsHgcMhvRecord.SysTypeCd == "ST")))
                {
                    EmParameters.CurrentMhvComponentType = "STRAIN";
                    EmParameters.CurrentMhvSystemType = "ST";
                    EmParameters.MatsMhvMeasuredModcList = ("01,02,32,33,41,42,43,44");

                    CheckDataView<MatsSorbentTrapRecord> matsSorbentTrapRecords
                        = EmParameters.MatsSorbentTrapRecords.FindRows(new cFilterCondition("MON_SYS_ID", EmParameters.MatsMhvRecord.MonSysId),
                                                                       new cFilterCondition("BEGIN_DATEHOUR", EmParameters.CurrentDateHour, eFilterDataType.DateBegan, eFilterConditionRelativeCompare.LessThanOrEqual),
                                                                       new cFilterCondition("END_DATEHOUR", EmParameters.CurrentDateHour, eFilterDataType.DateEnded, eFilterConditionRelativeCompare.GreaterThanOrEqual));

                    EmParameters.MatsMhvSorbentTraps = matsSorbentTrapRecords;
                }
                else
                {
                    EmParameters.CurrentMhvComponentType = "HG";
                    EmParameters.CurrentMhvSystemType = "HG";
                    EmParameters.MatsMhvMeasuredModcList = ("01,02,17,21");
                }

                EmParameters.MatsMhvUnavailableModcList = ("34,35");
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "MATSMHV1");
            }

            return ReturnVal;
        }

        #region MATSMHV2
        public static string MATSMHV2(cCategory Category, ref bool Log)
        // MATS HCLC: Initialize
        {
            string ReturnVal = "";

            try
            {
                EmParameters.CurrentMhvParameter = "HCLC";
                EmParameters.MatsMhvRecord = EmParameters.MatsHclcMhvRecord;
                EmParameters.CurrentMhvComponentType = "HCL";
                EmParameters.CurrentMhvSystemType = "HCL";
                EmParameters.MatsMhvMeasuredModcList = ("01,02,17,21");
                EmParameters.MatsMhvUnavailableModcList = ("34,35");
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "MATSMHV2");
            }

            return ReturnVal;
        }
        #endregion

        #region MATSMHV3
        public static string MATSMHV3(cCategory Category, ref bool Log)
        // MATS HFC: Initialize
        {
            string ReturnVal = "";

            try
            {
                EmParameters.CurrentMhvParameter = "HFC";
                EmParameters.MatsMhvRecord = EmParameters.MatsHfcMhvRecord;
                EmParameters.CurrentMhvComponentType = "HF";
                EmParameters.CurrentMhvSystemType = "HF";
                EmParameters.MatsMhvMeasuredModcList = ("01,02,17,21");
                EmParameters.MatsMhvUnavailableModcList = ("34,35");

            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "MATSMHV3");
            }

            return ReturnVal;
        }
        #endregion

        #region MATSMHV4
        public static string MATSMHV4(cCategory Category, ref bool Log)
        // Ensure that the reported MODC is one of the valid measured or unavailable MODC for the MATS parameter.
        {
            string ReturnVal = "";

            try
            {
                EmParameters.MonitorHourlyModcStatus = false;

                if (EmParameters.MatsMhvRecord.ModcCd == null)
                {
                    Category.CheckCatalogResult = "A";
                }
                else if (EmParameters.MatsMhvRecord.ModcCd.NotInList(EmParameters.MatsMhvMeasuredModcList.ToString()) && EmParameters.MatsMhvRecord.ModcCd.NotInList(EmParameters.MatsMhvUnavailableModcList.ToString()))
                {
                    Category.CheckCatalogResult = "B";
                }
                else if ((EmParameters.CurrentMhvSystemType == "ST") && EmParameters.MatsMhvRecord.ModcCd.NotInList("41,42"))
                {
                    if ((EmParameters.MatsMhvSorbentTraps != null) && EmParameters.MatsMhvSorbentTraps.Count > 0)
                    {
                        bool matchFound = false;

                        foreach (MatsSorbentTrapRecord matsSorbentTrapRecord in EmParameters.MatsMhvSorbentTraps)
                        {
                            if ((matsSorbentTrapRecord.ModcCd == EmParameters.MatsMhvRecord.ModcCd) && (matsSorbentTrapRecord.HgConcentration == EmParameters.MatsMhvRecord.UnadjustedHrlyValue))
                            {
                                matchFound = true;
                                break;
                            }
                            else if ((EmParameters.MatsMhvRecord.ModcCd == "35") && matsSorbentTrapRecord.ModcCd.InList("01,02"))
                            {
                                matchFound = true;
                                break;
                            }
                        }

                        if (matchFound)
                        {
                            EmParameters.MonitorHourlyModcStatus = true;
                        }
                        else
                        {
                            Category.CheckCatalogResult = "D";
                        }
                    }
                    else
                    {
                        EmParameters.MonitorHourlyModcStatus = true;
                    }
                }
                else
                {
                    EmParameters.MonitorHourlyModcStatus = true;
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "MATSMHV4");
            }

            return ReturnVal;
        }
        #endregion

        #region MATSMHV5
        public static string MATSMHV5(cCategory Category, ref bool Log)
        // Ensures that the Percent Monitor Availability (PMA) was reported and is inclusively between 0 and 100.
        {
            string ReturnVal = "";

            try
            {
                EmParameters.MonitorHourlyPmaStatus = false;

                if (EmParameters.MonitorHourlyModcStatus == true)
                {
                    if (EmParameters.MatsMhvRecord.PctAvailable == null)
                    {
                        Category.CheckCatalogResult = "A";
                    }

                    else if (EmParameters.MatsMhvRecord.PctAvailable > (decimal)100.0 || EmParameters.MatsMhvRecord.PctAvailable < (decimal)0.0)
                    {
                        Category.CheckCatalogResult = "B";
                    }

                    else
                    {
                        EmParameters.MonitorHourlyPmaStatus = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "MATSMHV5");
            }

            return ReturnVal;
        }
        #endregion

        #region MATSMHV6
        public static string MATSMHV6(cCategory Category, ref bool Log)
        // Ensures that a Monitor System is reported when a measured MODC is reported, and that is not reported when an unavailable MODC is reported.  
        //	When Monitor System is and should have been reported, the check ensures that the system type is valid for the MATS parameter being reported.
        {
            string ReturnVal = "";

            try
            {
                EmParameters.MonitorHourlySystemStatus = false;

                if (EmParameters.MonitorHourlyModcStatus == true)
                {
                    if (EmParameters.MatsMhvRecord.MonSysId == null)
                    {
                        if (EmParameters.MatsMhvRecord.ModcCd.InList(EmParameters.MatsMhvMeasuredModcList.ToString()))
                            Category.CheckCatalogResult = "A";
                        else
                            Category.CheckCatalogResult = "F";
                    }

                    else if (EmParameters.MatsMhvRecord.SystemIdentifier == null)
                    {
                        Category.CheckCatalogResult = "B";
                    }
                    else if (EmParameters.MatsMhvRecord.SysTypeCd != EmParameters.CurrentMhvSystemType)
                    {
                        Category.CheckCatalogResult = "C";
                    }
                    else if (EmParameters.MatsMhvRecord.SysTypeCd == "ST")
                    {
                        if ((EmParameters.MatsMhvSorbentTraps != null) && (EmParameters.MatsMhvSorbentTraps.Count > 0))
                        {
                            EmParameters.MonitorHourlySystemStatus = true;
                        }
                        else
                        {
                            if (EmParameters.MatsMhvRecord.ModcCd.InList(EmParameters.MatsMhvMeasuredModcList.ToString()))
                                Category.CheckCatalogResult = "E";
                        }
                    }
                    else
                    {
                        EmParameters.MonitorHourlySystemStatus = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "MATSMHV6");
            }

            return ReturnVal;
        }
        #endregion

        #region MATSMHV7
        public static string MATSMHV7(cCategory Category, ref bool Log)
        // Ensure that the System Designation Code is valid for the reported MODC.
        //Currently, the system designation should be 'P' for MODC 01 and 17, 'B' or 'PB' for MODC 02.
        {
            string ReturnVal = "";

            try
            {
                if (EmParameters.MonitorHourlyModcStatus == true && EmParameters.MonitorHourlySystemStatus == true && EmParameters.MatsMhvRecord.SystemIdentifier != null)
                {
                    switch (EmParameters.MatsMhvRecord.ModcCd)
                    {
                        case "01":
                        case "17":
                            {
                                if (EmParameters.MatsMhvRecord.SysDesignationCd != "P")
                                {
                                    Category.CheckCatalogResult = "A";
                                }
                                break;
                            }
                        case "02":
                            {
                                if (EmParameters.MatsMhvRecord.SysDesignationCd.NotInList("B,RB"))
                                {
                                    Category.CheckCatalogResult = "B";
                                }
                                break;
                            }
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "MATSMHV7");
            }

            return ReturnVal;
        }
        #endregion

        #region MATSMHV8
        public static string MATSMHV8(cCategory Category, ref bool Log)
        // Ensures that the conditions exist that allow the use of particular MODC. Currently only checks MODC 17.
        {
            string ReturnVal = "";

            try
            {
                if (EmParameters.MonitorHourlyModcStatus == true && EmParameters.MonitorHourlySystemStatus == true)
                {
                    if (EmParameters.MatsMhvRecord.ModcCd == "17")
                    {
                        if (Category.ModcHourCounts.LikeKindHourCount(Category.CurrentMonLocPos) >= 720)
                        {
                            //Locate a RATATestRecordsByLocationForQAStatus for the location
                            // if not found
                            if (cRowFilter.FindRows(EmParameters.RataTestRecordsByLocationForQaStatus.SourceView,
                                        new cFilterCondition[]
                                                {
                                                new cFilterCondition("MON_SYS_ID", EmParameters.MatsMhvRecord.MonSysId),
                                             new cFilterCondition("TEST_RESULT_CD", "PASS", eFilterConditionStringCompare.BeginsWith),
                                             new cFilterCondition("END_DATEHOUR",Category.ModcHourCounts.FirstLikeKindDateHour(Category.CurrentMonLocPos).Value, eFilterDataType.DateEnded, eFilterConditionRelativeCompare.GreaterThan),
                                                                     new cFilterCondition("END_DATEHOUR", EmParameters.CurrentOperatingDatehour, eFilterDataType.DateBegan, eFilterConditionRelativeCompare.LessThan)
                                                }
                                ).Count == 0)
                            {
                                Category.CheckCatalogResult = "A";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "MATSMHV8");
            }

            return ReturnVal;
        }
        #endregion

        #region MATSMHV9
        public static string MATSMHV9(cCategory Category, ref bool Log)
        // Ensures that a Component is reported when a measured MODC is reported, and that is not reported when an unavailable MODC is reported.
        //	When Component is and should have been reported, the check ensures that the system type is valid for the MATS parameter being reported.
        {
            string ReturnVal = "";

            try
            {
                EmParameters.MonitorHourlyComponentStatus = false;

                if (EmParameters.CurrentMhvSystemType != "ST")
                {
                    if (EmParameters.MonitorHourlyModcStatus == true)
                    {
                        if (EmParameters.MatsMhvRecord.ComponentId == null)
                        {
                            if (EmParameters.MatsMhvRecord.ModcCd.InList(EmParameters.MatsMhvMeasuredModcList.ToString()))
                                Category.CheckCatalogResult = "A";
                            else
                                Category.CheckCatalogResult = "G";
                        }
                        else
                            if (EmParameters.MatsMhvRecord.ComponentIdentifier == null)
                        {
                            Category.CheckCatalogResult = "B";
                        }

                        else if (EmParameters.MatsMhvRecord.ComponentTypeCd != EmParameters.CurrentMhvComponentType)
                        {
                            Category.CheckCatalogResult = "C";
                        }

                        else if (EmParameters.MatsMhvRecord.ModcCd == "17" && !EmParameters.MatsMhvRecord.ComponentIdentifier.StartsWith("LK"))
                        {
                            Category.CheckCatalogResult = "D";
                        }

                        else if (EmParameters.MatsMhvRecord.ComponentIdentifier.StartsWith("LK") && EmParameters.MatsMhvRecord.ModcCd != "17")
                        {
                            Category.CheckCatalogResult = "H";
                        }

                        else
                        {
                            EmParameters.MonitorHourlyComponentStatus = true;
                        }
                    }
                }
                else // STRAIN Component Type
                {
                    if (EmParameters.MatsMhvRecord.ComponentId != null)
                    {
                        Category.CheckCatalogResult = "F";
                    }
                    else
                    {
                        EmParameters.MonitorHourlyComponentStatus = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "MATSMHV9");
            }

            return ReturnVal;
        }
        #endregion

        #region MATSMHV10
        public static string MATSMHV10(cCategory Category, ref bool Log)
        // Ensure that at least one active Monitoring System Component record exists for the Monitoring System Id and Component Id 
        //	in the current MATS MHV record.
        {
            string ReturnVal = "";

            try
            {
                if ((EmParameters.MonitorHourlySystemStatus == true)
                    && (EmParameters.MatsMhvRecord.MonSysId != null)
                    && (EmParameters.MonitorHourlyComponentStatus == true)
                    && (EmParameters.MatsMhvRecord.ComponentId != null))
                {
                    int CountMonSysCompRecord = cRowFilter.FindRows(EmParameters.MonitorSystemComponentRecordsByHourLocation.SourceView,
                                                new cFilterCondition[]
                                                {
                                                new cFilterCondition("MON_SYS_ID", EmParameters.MatsMhvRecord.MonSysId),
                                                new cFilterCondition("COMPONENT_ID", EmParameters.MatsMhvRecord.ComponentId)
                                                }
                        ).Count;

                    if (CountMonSysCompRecord == 0)
                    {
                        Category.CheckCatalogResult = "A";
                    }
                }

            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "MATSMHV10");
            }

            return ReturnVal;
        }
        #endregion
        #endregion

        #region Checks 11-20

        #region MATSMHV11
        public static string MATSMHV11(cCategory Category, ref bool Log)
        // Determines the MPC for the active Monitor Span record for the hour, location and component type, 
        // returning a check result if a single row is not found or the MPC is not greater than 0.
        {
            string ReturnVal = "";

            try
            {
                EmParameters.CurrentMhvMaxMinValue = null;

                if (EmParameters.MonitorHourlyModcStatus == true && EmParameters.MatsMhvRecord.ModcCd.InList(EmParameters.MatsMhvMeasuredModcList.ToString()))
                {
                    if (EmParameters.CurrentMhvComponentType == "HG")
                    {
                        DataView FilteredMonitorSpanRecords
                          = cRowFilter.FindRows(EmParameters.MonitorSpanRecordsByHourLocation.SourceView,
                                                new cFilterCondition[]
                                              {
                                      new cFilterCondition("COMPONENT_TYPE_CD", EmParameters.CurrentMhvComponentType),
                                                new cFilterCondition("SPAN_SCALE_CD", "H")
                                              });

                        if (FilteredMonitorSpanRecords != null)
                        {
                            if (FilteredMonitorSpanRecords.Count > 1)
                            {
                                Category.CheckCatalogResult = "A";
                            }
                            else if (FilteredMonitorSpanRecords.Count == 0)
                            {
                                Category.CheckCatalogResult = "B";
                            }
                            else // 1 record
                            {
                                DataRowView CurrentMonitorSpanRecord = FilteredMonitorSpanRecords[0];
                                if (CurrentMonitorSpanRecord["MPC_VALUE"] != null && CurrentMonitorSpanRecord["MPC_VALUE"].AsDecimal() > 0)
                                {
                                    EmParameters.CurrentMhvMaxMinValue = CurrentMonitorSpanRecord["MPC_VALUE"].AsDecimal();
                                }
                                else
                                {
                                    Category.CheckCatalogResult = "C";
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "MATSMHV11");
            }

            return ReturnVal;
        }
        #endregion

        #region MATSMHV12

        /// <summary>
        /// Validates the value and format of the reported unadjusted hourly value.
        /// </summary>
        /// <param name="Category">The object for the category in which the check is running.</param>
        /// <param name="Log">Obsolete.</param>
        /// <returns>Null if the check runs successfully, otherwise the exception message.</returns>
        public static string MATSMHV12(cCategory Category, ref bool Log)
        {
            string ReturnVal = "";

            try
            {
                EmParameters.MonitorHourlyUnadjustedValueStatus = false;
                EmParameters.MatsMhvCalculatedValue = null;

                if (EmParameters.MonitorHourlyModcStatus == true)
                {
                    switch (EmParameters.MatsMhvRecord.ModcCd)
                    {
                        case "21":
                            {
                                EmParameters.MatsMhvCalculatedValue = ECMPS.Definitions.Extensions.cExtensions.DecimaltoScientificNotation(0);
                                if (ECMPS.Definitions.Extensions.cExtensions.ScientificNotationtoDecimal(EmParameters.MatsMhvRecord.UnadjustedHrlyValue) == 0)
                                {
                                    EmParameters.MonitorHourlyUnadjustedValueStatus = true;
                                }
                                else
                                {
                                    Category.CheckCatalogResult = "A";
                                }
                                break;
                            }
                        default:
                            {
                                if (EmParameters.MatsMhvRecord.ModcCd.InList(EmParameters.MatsMhvMeasuredModcList.ToString()))
                                {
                                    if (string.IsNullOrEmpty(EmParameters.MatsMhvRecord.UnadjustedHrlyValue))
                                    {
                                        Category.CheckCatalogResult = "B";
                                    }
                                    else if (!ECMPS.Definitions.Extensions.cExtensions.ScientificNotationValid(EmParameters.MatsMhvRecord.UnadjustedHrlyValue) || ECMPS.Definitions.Extensions.cExtensions.ScientificNotationSignificantDigits(EmParameters.MatsMhvRecord.UnadjustedHrlyValue) != 3)
                                    {
                                        Category.CheckCatalogResult = "C";
                                    }
                                    else if (ECMPS.Definitions.Extensions.cExtensions.ScientificNotationtoDecimal(EmParameters.MatsMhvRecord.UnadjustedHrlyValue) < 0)
                                    {
                                        Category.CheckCatalogResult = "D";
                                    }
                                    else
                                    {
                                        EmParameters.MonitorHourlyUnadjustedValueStatus = true;
                                        EmParameters.MatsMhvCalculatedValue = EmParameters.MatsMhvRecord.UnadjustedHrlyValue;

                                        if (EmParameters.CurrentMhvMaxMinValue != null && ECMPS.Definitions.Extensions.cExtensions.ScientificNotationtoDecimal(EmParameters.MatsMhvRecord.UnadjustedHrlyValue) > EmParameters.CurrentMhvMaxMinValue)
                                        {
                                            Category.CheckCatalogResult = "E";
                                        }
                                    }
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(EmParameters.MatsMhvRecord.UnadjustedHrlyValue))
                                    {
                                        Category.CheckCatalogResult = "F";
                                    }
                                }
                                break;
                            }
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "MATSMHV12");
            }

            return ReturnVal;
        }
        #endregion

        /// <summary>
        /// MATS: QA Status Required
        /// 
        /// Sets the following parameter to determine whether or not the corresponding status checks should run 
        /// for the the current MATS MHV record.
        /// 
        ///     DailyCalStatusRequired
        ///     LinearityStatusRequired
        ///     QuarterlyGasAuditStatus
        ///     RataStatusRequired
        ///     WsiStatusRequired
        /// 
        ///     No status checks are required unless the following conditions are met:
        ///     
        ///     1) MonitorHourlyModcStatus is true.
        ///     2) The MatsMhvRecord.ModcCd is a measured MODC.
        ///     3) The MatsMhvRecord.UnadjustedValue is not null.
        ///     
        ///     Otherwise:
        ///         
        ///         When the following conditions are met:
        ///         
        ///         1) MonitorHourlyComponentStatus is true.
        ///         2) MatsMhvRecord.ComponentId is not null.
        ///         3) MatsMhvRecord.ParameterCd equals "HGC".
        ///         4) MatsMhvRecord.SysTypeCd equals "HG".
        ///         
        ///         Then the following are set to true:
        ///         
        ///         * DailyCalStatusRequired
        ///         * LinearityStatusRequired
        ///         * WsiStatusRequired
        ///         
        ///         When the following conditions are met:
        ///         
        ///         1) MonitorHourlyComponentStatus is true.
        ///         2) MatsMhvRecord.ComponentId is not null.
        ///         3) MatsMhvRecord.ParameterCd equals "HCLC" or "HFC".
        ///         
        ///         Then the following are set to true:
        ///         
        ///         * QuarterlyGasAuditStatus
        ///         
        ///         When the following conditions are met:
        ///         
        ///         1) MonitorHourlySystemStatus is true.
        ///         2) MatsMhvRecord.MonSysId is not null.
        ///         
        ///         Then the follwoing are set to true:
        ///         
        ///         * RataStatusRequired
        /// 
        /// 
        /// Additionally, the check sets various component and system values used by the status checks.
        /// 
        /// </summary>
        /// <param name="Category"></param>
        /// <param name="Log"></param>
        /// <returns></returns>
        public static string MATSMHV13(cCategory Category, ref bool Log)
        // MATS: QA Status Required
        {
            string ReturnVal = "";

            try
            {
                EmParameters.QaStatusComponentBeginDate = EmParameters.MatsMhvRecord.ComponentBeginDate;
                EmParameters.QaStatusComponentBeginDatehour = EmParameters.MatsMhvRecord.ComponentBeginDatehour;
                EmParameters.QaStatusComponentId = EmParameters.MatsMhvRecord.ComponentId;
                EmParameters.QaStatusComponentIdentifier = EmParameters.MatsMhvRecord.ComponentIdentifier;
                EmParameters.QaStatusComponentTypeCode = EmParameters.MatsMhvRecord.ComponentTypeCd;
                EmParameters.QaStatusSystemDesignationCode = EmParameters.MatsMhvRecord.SysDesignationCd;
                EmParameters.QaStatusSystemId = EmParameters.MatsMhvRecord.MonSysId;
                EmParameters.QaStatusSystemIdentifier = EmParameters.MatsMhvRecord.SystemIdentifier;
                EmParameters.QaStatusSystemTypeCode = EmParameters.MatsMhvRecord.SysTypeCd;

                /* Set QA Status MATS ERB Date */
                if (EmParameters.MatsMhvRecord.ParameterCd.InList("HGC,HCLC,HFC"))
                {
                    VwMpLocationProgramRow programRow
                        = EmParameters.EmLocationProgramRecords.FindEarliestRowByDate
                        (
                            "EMISSIONS_RECORDING_BEGIN_DATE",
                            "END_DATE",
                            new cFilterCondition[][]
                            {
                                    new cFilterCondition[]
                                    {
                                        new cFilterCondition("PRG_CD", "MATS"),
                                        new cFilterCondition("EMISSIONS_RECORDING_BEGIN_DATE", eFilterConditionRelativeCompare.LessThanOrEqual, EmParameters.CurrentDateHour.Value.Date),
                                        new cFilterCondition("END_DATE", null)
                                    },
                                    new cFilterCondition[]
                                    {
                                        new cFilterCondition("PRG_CD", "MATS"),
                                        new cFilterCondition("EMISSIONS_RECORDING_BEGIN_DATE", eFilterConditionRelativeCompare.LessThanOrEqual, EmParameters.CurrentDateHour.Value.Date),
                                        new cFilterCondition("END_DATE", eFilterConditionRelativeCompare.GreaterThanOrEqual, EmParameters.CurrentDateHour.Value.Date)
                                    }
                            }
                        );

                    EmParameters.QaStatusMatsErbDate = (programRow != null) ? programRow.EmissionsRecordingBeginDate : null;
                }
                else
                {
                    EmParameters.QaStatusMatsErbDate = null;
                }

                EmParameters.DailyCalStatusRequired = false;
                EmParameters.LinearityStatusRequired = false;
                EmParameters.QuarterlyGasAuditStatus = false;
                EmParameters.RataStatusRequired = false;
                EmParameters.WsiStatusRequired = false;


                if (EmParameters.MonitorHourlyModcStatus == true
                    && EmParameters.MatsMhvRecord.ModcCd.InList(EmParameters.MatsMhvMeasuredModcList.ToString())
                    && !string.IsNullOrEmpty(EmParameters.MatsMhvRecord.UnadjustedHrlyValue))
                {

                    if (EmParameters.MonitorHourlyComponentStatus == true && EmParameters.MatsMhvRecord.ComponentId != null)
                    {

                        if (EmParameters.MatsMhvRecord.ParameterCd == "HGC")
                        {
                            if (EmParameters.MatsMhvRecord.ComponentTypeCd == "HG")
                            {
                                EmParameters.DailyCalStatusRequired = true;
                                EmParameters.LinearityStatusRequired = true;
                                EmParameters.WsiStatusRequired = true;
                            }
                        }

                        else if (EmParameters.MatsMhvRecord.ParameterCd.InList("HCLC,HFC"))
                        {
                            EmParameters.QuarterlyGasAuditStatus = true;
                        }
                    }
                    if (EmParameters.MonitorHourlySystemStatus == true && EmParameters.MatsMhvRecord.MonSysId != null)
                    {
                        EmParameters.RataStatusRequired = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "MATSMHV13");
            }

            return ReturnVal;
        }

        #region MATSMHV14
        public static string MATSMHV14(cCategory Category, ref bool Log)
        // Assigns the calculated values for MATS Hg Concentration Monitor Hourly.
        {
            string ReturnVal = "";

            try
            {
                EmParameters.MatsMhvCalculatedHgcValue = EmParameters.MatsMhvCalculatedValue;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "MATSMHV14");
            }

            return ReturnVal;
        }
        #endregion

        #region MATSMHV15
        public static string MATSMHV15(cCategory Category, ref bool Log)
        // Assigns the calculated values for MATS HCl Concentration Monitor Hourly.
        {
            string ReturnVal = "";

            try
            {
                EmParameters.MatsMhvCalculatedHclcValue = EmParameters.MatsMhvCalculatedValue;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "MATSMHV15");
            }

            return ReturnVal;
        }
        #endregion

        #region MATSMHV16
        public static string MATSMHV16(cCategory Category, ref bool Log)
        // Assigns the calculated values for MATS HF Concentration Monitor Hourly.
        {
            string ReturnVal = "";

            try
            {
                EmParameters.MatsMhvCalculatedHfcValue = EmParameters.MatsMhvCalculatedValue;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "MATSMHV16");
            }

            return ReturnVal;
        }
        #endregion

        #region MATSMHV17
        public static string MATSMHV17(cCategory Category, ref bool Log)
        //
        {
            string ReturnVal = "";

            try
            {
                if (EmParameters.LinearityStatusRequired == true || EmParameters.DailyCalStatusRequired == true)
                {
                    // Initialize
                    EmParameters.DualRangeStatus = false;
                    EmParameters.ApplicableComponentId = null;
                    EmParameters.ApplicableSystemIds = null;
                    EmParameters.CurrentAnalyzerRangeUsed = null;
                    EmParameters.HighRangeComponentId = null;
                    EmParameters.LowRangeComponentId = null;

                    //Find Analyzer Range Records
                    cFilterCondition[] AnalyzerFilter = new cFilterCondition[] { new cFilterCondition("COMPONENT_ID", EmParameters.QaStatusComponentId) };
                    DataView AnalyzerRangeRecs = cRowFilter.FindRows(EmParameters.AnalyzerRangeRecordsByHourLocation.SourceView,
                                AnalyzerFilter);

                    if (AnalyzerRangeRecs == null || AnalyzerRangeRecs.Count == 0 || AnalyzerRangeRecs.Count > 1)
                    {
                        EmParameters.LinearityStatusRequired = false;
                        EmParameters.DailyCalStatusRequired = false;
                        Category.CheckCatalogResult = "A";
                    }
                    else if (AnalyzerRangeRecs[0]["DUAL_RANGE_IND"].AsInteger() == 1)
                    {
                        EmParameters.LinearityStatusRequired = false;
                        EmParameters.DailyCalStatusRequired = false;
                        Category.CheckCatalogResult = "B";
                    }

                    else if (AnalyzerRangeRecs[0]["ANALYZER_RANGE_CD"].AsString() != "H")
                    {
                        EmParameters.LinearityStatusRequired = false;
                        EmParameters.DailyCalStatusRequired = false;
                        Category.CheckCatalogResult = "C";
                    }
                    else
                    {
                        EmParameters.CurrentAnalyzerRangeUsed = AnalyzerRangeRecs[0]["ANALYZER_RANGE_CD"].AsString();
                        EmParameters.ApplicableComponentId = EmParameters.QaStatusComponentId;
                        EmParameters.HighRangeComponentId = EmParameters.QaStatusComponentId;

                        //Find System Records
                        cFilterCondition[] MonSysFilter = new cFilterCondition[] { new cFilterCondition("COMPONENT_ID", EmParameters.ApplicableComponentId) };
                        DataView MonSysRecs = cRowFilter.FindRows(EmParameters.MonitorSystemComponentRecordsByHourLocation.SourceView,
                                    MonSysFilter);

                        if (MonSysRecs != null && MonSysRecs.Count > 0)
                        {
                            foreach (DataRowView MonSysRec in MonSysRecs)
                            {
                                EmParameters.ApplicableSystemIds.AsString().ListAdd(MonSysRec["MON_SYS_ID"].AsString());
                            }
                        }
                        else
                        {
                            EmParameters.LinearityStatusRequired = false;
                            EmParameters.DailyCalStatusRequired = false;
                            Category.CheckCatalogResult = "D";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "MATSMHV17");
            }

            return ReturnVal;
        }
        #endregion

        /// <summary>
        /// MATS HgC: 3-Level System Integrity Status Check
        /// 
        /// Ensures that a 3-Level system integrity was performed after the most recent 120 or 125 certification event, 
        /// or after the system component begin datehour if an event does not exist.
        /// </summary>
        /// <param name="category">Category Object</param>
        /// <param name="log">Indicates whether to log results.</param>
        /// <returns>Returns error message if check fails to run correctly.</returns>
        public static string MATSMHV18(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                if (EmParameters.WsiStatusRequired.Default(false))
                {
                    /* Locate the certification event and set the test required datehour. */
                    VwQaCertEventRow mostRecentEventRecord
                      = EmParameters.QaCertificationEventRecords.FindMostRecentRow
                        (
                          EmParameters.CurrentDateHour.Value,
                          "QA_CERT_EVENT_DATEHOUR",
                          new cFilterCondition[]
                          {
                  new cFilterCondition("COMPONENT_ID", EmParameters.QaStatusComponentId),
                  new cFilterCondition("QA_CERT_EVENT_CD", "120,125", eFilterConditionStringCompare.InList)
                          }
                        );

                    DateTime? testRequiredDateHour = null;

                    if (mostRecentEventRecord != null)
                    {
                        testRequiredDateHour = mostRecentEventRecord.QaCertEventDatehour.Default(DateTime.MinValue).AddHours(1);
                    }
                    else
                    {
                        /* Use the most recent system component row to set the test required datehour if an event was not found */
                        VwMpMonitorSystemComponentRow mostRecentMonitorSystemComponentRecord
                          = EmParameters.MonitorSystemComponentRecordsByHourLocation.FindMostRecentRow
                            (
                              EmParameters.CurrentDateHour.Value.AddHours(1), // AddHours(1) will include CurrentDateHour in the "most recent search"
                              "BEGIN_DATEHOUR",
                              new cFilterCondition[]
                              {
                    new cFilterCondition("MON_SYS_ID", EmParameters.QaStatusSystemId),
                    new cFilterCondition("COMPONENT_ID", EmParameters.QaStatusComponentId)
                              }
                            );

                        if (mostRecentMonitorSystemComponentRecord != null)
                        {
                            testRequiredDateHour = mostRecentMonitorSystemComponentRecord.BeginDatehour.Default(DateTime.MinValue).AddHours(1);
                        }
                        else
                        {
                            category.CheckCatalogResult = "B";
                        }
                    }


                    if (category.CheckCatalogResult == null)
                    {
                        /* Determine whether a test exists on or after the test required datehour and before the current date. */
                        int mats3LevelSystemIntegrityRecordCount
                          = EmParameters.Mats3LevelSystemIntegrityRecordsForQaStatus.CountRows
                            (
                              new cFilterCondition[]
                              {
                    new cFilterCondition("COMPONENT_ID", EmParameters.QaStatusComponentId),
                    new cFilterCondition("END_DATEHOUR", EmParameters.CurrentDateHour, eFilterDataType.DateEnded, eFilterConditionRelativeCompare.LessThan),
                    new cFilterCondition("END_DATEHOUR", testRequiredDateHour, eFilterDataType.DateEnded, eFilterConditionRelativeCompare.GreaterThanOrEqual)
                              }
                            );

                        if (mats3LevelSystemIntegrityRecordCount == 0)
                        {
                            category.CheckCatalogResult = "A";
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
        /// MATS HgC: 3-Level System Integrity Status Check
        /// 
        /// Ensures that a 3-Level system integrity was performed for a non-like-kind analyzer component.  If a 120 or 125 
        /// certification event occurred, this check ensures that either 168 hours or 90/180 has not elapsed since the event, or 
        /// that a 3-Level system integrity was performed after the event.
        /// </summary>
        /// <param name="category">Category Object</param>
        /// <param name="log">Indicates whether to log results.</param>
        /// <returns>Returns error message if check fails to run correctly.</returns>
        public static string MATSMHV19(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                VwQaSuppDataHourlyStatusRow hgTestRecord;
                VwQaCertEventRow certEventRecord;

                MatsHgLinearityLikeCertificationCheck(category, EmParameters.Mats3LevelSystemIntegrityRecordsForQaStatus, out hgTestRecord, out certEventRecord);

                EmParameters.MatsHg3LevelSiTestRecord = hgTestRecord;
                EmParameters.MatsHg3LevelSiEventRecord = certEventRecord;
            }
            catch (Exception ex)
            {
                EmParameters.MatsHg3LevelSiEventRecord = null;

                returnVal = category.CheckEngine.FormatError(ex);
            }

            return returnVal;
        }

        /// <summary>
        /// MATS HgC: Hg Linearity Status Check
        /// 
        /// Ensures that a Hg Linearity was performed for a non-like-kind analyzer component.  If a 120 or 125 certification event 
        /// occurred, this check ensures that either 168 hours or 90/180 has not elapsed since the event, or that an Hg Linearity 
        /// was performed after the event.
        /// </summary>
        /// <param name="category">Category Object</param>
        /// <param name="log">Indicates whether to log results.</param>
        /// <returns>Returns error message if check fails to run correctly.</returns>
        public static string MATSMHV20(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                VwQaSuppDataHourlyStatusRow hgTestRecord;
                VwQaCertEventRow certEventRecord;

                MatsHgLinearityLikeCertificationCheck(category, EmParameters.MatsHgLinearityRecordsForQaStatus, out hgTestRecord, out certEventRecord);

                EmParameters.MatsHgLinearityTestRecord = hgTestRecord;
                EmParameters.MatsHgLinearityEventRecord = certEventRecord;
            }
            catch (Exception ex)
            {
                EmParameters.MatsHgLinearityEventRecord = null;

                returnVal = category.CheckEngine.FormatError(ex);
            }

            return returnVal;
        }


        #region Helper Methods

        /// <summary>
        /// Helper Method for MATSMHV 19 and 20.
        /// 
        /// Ensures that a test belonging to matsTestRecordsForQaStatus was performed for a non-like-kind analyzer component.  If 
        /// a 120 or 125 certification event occurred, this method ensures that either 168 hours or 90/180 has not elapsed since 
        /// the event, or that an Hg Linearity was performed after the event.
        /// </summary>
        /// <param name="category">Category object for category in which the check is running.</param>
        /// <param name="matsTestRecordsForQaStatus">The set of either 3-Level System Integrity or set of Hg Linearity tests.</param>
        /// <param name="hgTestRecord">The located Hg Linearity or 3-Level SI test record.</param>
        /// <param name="certEventRecord">The located event record.</param>
        /// <returns></returns>
        public static void MatsHgLinearityLikeCertificationCheck(cCategory category, CheckDataView<VwQaSuppDataHourlyStatusRow> matsTestRecordsForQaStatus, 
                                                                 out VwQaSuppDataHourlyStatusRow hgTestRecord, out VwQaCertEventRow certEventRecord)
        {
            hgTestRecord = null;
            certEventRecord = null;

            if (EmParameters.WsiStatusRequired.Default(false) && !EmParameters.QaStatusComponentIdentifier.StartsWith("LK"))
            {
                /* Locate most recent passed or passed APS MATS Hg test for the current component id in the current hour but before the 45  minute  */
                hgTestRecord
                  = matsTestRecordsForQaStatus.FindMostRecentRow
                    (
                      EmParameters.CurrentDateHour.Value.AddHours(1),
                      "END_DATETIME",
                      new cFilterCondition[] 
                      {
                          new cFilterCondition("COMPONENT_ID", EmParameters.QaStatusComponentId),
                          new cFilterCondition("END_DATEHOUR", eFilterConditionRelativeCompare.Equals, EmParameters.CurrentDateHour.Value),
                          new cFilterCondition("END_MIN", eFilterConditionRelativeCompare.LessThan, 45),
                          new cFilterCondition("TEST_RESULT_CD", "PASSED,PASSAPS", eFilterConditionStringCompare.InList)
                      }
                  );

                if (hgTestRecord == null)
                {
                    /* Locate most recent MATS Hg test for the current component id that occurred before the current hour */
                    hgTestRecord
                      = matsTestRecordsForQaStatus.FindMostRecentRow
                        (
                          EmParameters.CurrentDateHour.Value,
                          "END_DATEHOUR",
                          new cFilterCondition[] 
                          {
                              new cFilterCondition("COMPONENT_ID", EmParameters.QaStatusComponentId),
                              new cFilterCondition("TEST_RESULT_CD", "PASSED,PASSAPS", eFilterConditionStringCompare.InList)
                          }
                      );
                }

                /* Locate most recent cert event or most recent intervening cert event if a system integrity test was found */
                {
                    cFilterCondition[] eventConditon
                        = (hgTestRecord != null)
                        ? new cFilterCondition[]
                            {
                                new cFilterCondition("COMPONENT_ID", EmParameters.QaStatusComponentId),
                                new cFilterCondition("QA_CERT_EVENT_CD", "100,101,120,125", eFilterConditionStringCompare.InList),
                                new cFilterCondition("QA_CERT_EVENT_DATEHOUR", hgTestRecord.EndDatehour, eFilterDataType.DateEnded, eFilterConditionRelativeCompare.GreaterThan),
                                new cFilterCondition("QA_CERT_EVENT_DATEHOUR", eFilterConditionRelativeCompare.Equals, EmParameters.CurrentDateHour.Value, eNullDateDefault.Max),
                                new cFilterCondition("CONDITIONAL_DATA_BEGIN_DATEHOUR", eFilterConditionRelativeCompare.Equals, EmParameters.CurrentDateHour.Value, eNullDateDefault.Max)
                            }
                        : new cFilterCondition[]
                            {
                                new cFilterCondition("COMPONENT_ID", EmParameters.QaStatusComponentId),
                                new cFilterCondition("QA_CERT_EVENT_CD", "100,101,120,125", eFilterConditionStringCompare.InList),
                                new cFilterCondition("QA_CERT_EVENT_DATEHOUR", eFilterConditionRelativeCompare.Equals, EmParameters.CurrentDateHour.Value, eNullDateDefault.Max),
                                new cFilterCondition("CONDITIONAL_DATA_BEGIN_DATEHOUR", eFilterConditionRelativeCompare.Equals, EmParameters.CurrentDateHour.Value, eNullDateDefault.Max)
                            };

                    /*  Locate the most recent hour on or before the current hour */
                    certEventRecord = EmParameters.QaCertificationEventRecords.FindMostRecentRow(EmParameters.CurrentDateHour.Value.AddHours(1), 
                                                                                                 "QA_CERT_EVENT_DATEHOUR", 
                                                                                                 eventConditon);

                    if (certEventRecord == null)
                    {
                        eventConditon
                            = (hgTestRecord != null)
                            ? new cFilterCondition[]
                                {
                                new cFilterCondition("COMPONENT_ID", EmParameters.QaStatusComponentId),
                                new cFilterCondition("QA_CERT_EVENT_CD", "100,101,120,125", eFilterConditionStringCompare.InList),
                                new cFilterCondition("QA_CERT_EVENT_DATEHOUR", hgTestRecord.EndDatehour, eFilterDataType.DateEnded, eFilterConditionRelativeCompare.GreaterThan)
                                }
                            : new cFilterCondition[]
                                {
                                new cFilterCondition("COMPONENT_ID", EmParameters.QaStatusComponentId),
                                new cFilterCondition("QA_CERT_EVENT_CD", "100,101,120,125", eFilterConditionStringCompare.InList)
                                };

                        /* Locate the most recent hour before the current hour */
                        certEventRecord = EmParameters.QaCertificationEventRecords.FindMostRecentRow(EmParameters.CurrentDateHour.Value, 
                                                                                                     "QA_CERT_EVENT_DATEHOUR", 
                                                                                                     eventConditon);
                    }
                }

                /*  Determine Results */
                if (certEventRecord != null)
                {
                    if ((certEventRecord.ConditionalDataBeginDatehour != null) && (certEventRecord.ConditionalDataBeginDatehour.Value <= EmParameters.CurrentDateHour.Value))
                    {
                        StatusDetermination.eConditionalDataStatus status = certEventRecord.QaCertEventCd == "125"
                                                                          ? StatusDetermination.ConditionalDataEvent125ForMats()
                                                                          : StatusDetermination.ConditionalDataEvent120ForMats(certEventRecord.ConditionalDataBeginDatehour,
                                                                                                                               certEventRecord.ConditionalBeginHourSuppDataExistsInd,
                                                                                                                               certEventRecord.ConditionalBeginOpHourCount);

                        switch (status)
                        {
                            case StatusDetermination.eConditionalDataStatus.EXPIRED: { category.CheckCatalogResult = (hgTestRecord == null) ? "A" : "B"; } break;
                            case StatusDetermination.eConditionalDataStatus.UNDETERMINED: { category.CheckCatalogResult = (hgTestRecord == null) ? "D" : "E"; } break;
                            case StatusDetermination.eConditionalDataStatus.MISSINGPROGRAM: { category.CheckCatalogResult = "F"; } break;
                            case StatusDetermination.eConditionalDataStatus.MISSINGACCUM: { category.CheckCatalogResult = "G"; } break;
                            case StatusDetermination.eConditionalDataStatus.MISSINGOPSUPP: { category.CheckCatalogResult = "H"; } break;
                            case StatusDetermination.eConditionalDataStatus.MISSINGVALUE: { category.CheckCatalogResult = "I"; } break;
                        }
                    }
                    else
                        category.CheckCatalogResult = "J";
                }
                else if (hgTestRecord == null)
                {
                    category.CheckCatalogResult = "C";
                }
                else if (hgTestRecord.QaNeedsEvalFlg == "Y")
                {
                    category.CheckCatalogResult = "K";
                }
            }
        }

        #endregion

        #endregion

    }
}