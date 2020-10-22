using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Em.Parameters;
using ECMPS.Checks.EmissionsReport;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;
using ECMPS.Definitions.Extensions;

namespace ECMPS.Checks.EmissionsChecks
{
    public class cMATSCalculatedHourlyValueChecks : cEmissionsChecks
    {

        #region Constructors

        public cMATSCalculatedHourlyValueChecks(cEmissionsReportProcess emissionReportProcess)
            : base(emissionReportProcess)
        {
            CheckProcedures = new dCheckProcedure[21];

            CheckProcedures[1] = new dCheckProcedure(MATSCHV1);
            CheckProcedures[2] = new dCheckProcedure(MATSCHV2);
            CheckProcedures[3] = new dCheckProcedure(MATSCHV3);
            CheckProcedures[4] = new dCheckProcedure(MATSCHV4);
            CheckProcedures[5] = new dCheckProcedure(MATSCHV5);
            CheckProcedures[6] = new dCheckProcedure(MATSCHV6);
            CheckProcedures[7] = new dCheckProcedure(MATSCHV7);
            CheckProcedures[8] = new dCheckProcedure(MATSCHV8);
            CheckProcedures[9] = new dCheckProcedure(MATSCHV9);
            CheckProcedures[10] = new dCheckProcedure(MATSCHV10);
            CheckProcedures[11] = new dCheckProcedure(MATSCHV11);
            CheckProcedures[12] = new dCheckProcedure(MATSCHV12);
            CheckProcedures[13] = new dCheckProcedure(MATSCHV13);
            CheckProcedures[14] = new dCheckProcedure(MATSCHV14);
            CheckProcedures[15] = new dCheckProcedure(MATSCHV15);
            CheckProcedures[16] = new dCheckProcedure(MATSCHV16);
            CheckProcedures[17] = new dCheckProcedure(MATSCHV17);
            CheckProcedures[18] = new dCheckProcedure(MATSCHV18);
            CheckProcedures[19] = new dCheckProcedure(MATSCHV19);
            CheckProcedures[20] = new dCheckProcedure(MATSCHV20);
        }

        #endregion

        #region Checks 1-10

        public static string MATSCHV1(cCategory Category, ref bool Log)
        //This check sets generic parameters and output parameters for subsequent Calculated hourly checks for HGRE.
        {
            string ReturnVal = "";

            try
            {
                EmParameters.CalculationConversionFactor = ECMPS.Definitions.Extensions.cExtensions.ScientificNotationtoDecimal("6.24E-11");
                EmParameters.CurrentDhvParameter = EmParameters.MatsHgDhvParameter;
                EmParameters.CurrentDhvRecordValid = EmParameters.MatsHgDhvValid;
                EmParameters.MatsDhvRecord = EmParameters.MatsHgDhvRecord;
                EmParameters.MatsMhvCalculatedValue = EmParameters.MatsMhvCalculatedHgcValue;
                EmParameters.MatsMhvRecord = EmParameters.MatsHgcMhvRecord;
                EmParameters.MatsMoistureEquationList = "A-3";

                EmParameters.MatsDhvMeasuredModcList = "36,39";
                EmParameters.MatsDhvUnavailableModcList = "38";

                EmParameters.FinalConversionFactor = (EmParameters.CurrentHourlyOpRecord.MatsHourLoad.Default(0m) != 0)
                                                   ? 1000 / EmParameters.CurrentHourlyOpRecord.MatsHourLoad.Value
                                                   : (decimal?) null;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "MATSCHV1");
            }

            return ReturnVal;
        }

        public static string MATSCHV2(cCategory Category, ref bool Log)
        //This check sets generic parameters and output parameters for subsequent Calculated hourly checks for HCLRE. 
        {
            string ReturnVal = "";

            try
            {
                EmParameters.CalculationConversionFactor = ECMPS.Definitions.Extensions.cExtensions.ScientificNotationtoDecimal("9.43E-8");
                EmParameters.CurrentDhvParameter = EmParameters.MatsHclDhvParameter;
                EmParameters.CurrentDhvRecordValid = EmParameters.MatsHclDhvValid;
                EmParameters.MatsDhvRecord = EmParameters.MatsHclDhvRecord;
                EmParameters.MatsMhvCalculatedValue = EmParameters.MatsMhvCalculatedHclcValue;
                EmParameters.MatsMhvRecord = EmParameters.MatsHclcMhvRecord;
                EmParameters.MatsMoistureEquationList = "HC-3";

                EmParameters.MatsDhvMeasuredModcList = "36,39";
                EmParameters.MatsDhvUnavailableModcList = "38";

                EmParameters.FinalConversionFactor = (EmParameters.CurrentHourlyOpRecord.MatsHourLoad.Default(0m) != 0)
                                                   ? 1 / EmParameters.CurrentHourlyOpRecord.MatsHourLoad.Value
                                                   : (decimal?)null;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "MATSCHV2");
            }

            return ReturnVal;
        }

        public static string MATSCHV3(cCategory Category, ref bool Log)
        //This check sets generic parameters and output parameters for subsequent Calculated hourly checks for HFRE.
        {
            string ReturnVal = "";

            try
            {
                EmParameters.CalculationConversionFactor = ECMPS.Definitions.Extensions.cExtensions.ScientificNotationtoDecimal("5.18E-8");
                EmParameters.CurrentDhvParameter = EmParameters.MatsHfDhvParameter;
                EmParameters.CurrentDhvRecordValid = EmParameters.MatsHfDhvValid;
                EmParameters.MatsDhvRecord = EmParameters.MatsHfDhvRecord;
                EmParameters.MatsMhvCalculatedValue = EmParameters.MatsMhvCalculatedHfcValue;
                EmParameters.MatsMhvRecord = EmParameters.MatsHfcMhvRecord;
                EmParameters.MatsMoistureEquationList = "HF-3";

                EmParameters.MatsDhvMeasuredModcList = "36,39";
                EmParameters.MatsDhvUnavailableModcList = "38";

                EmParameters.FinalConversionFactor = (EmParameters.CurrentHourlyOpRecord.MatsHourLoad.Default(0m) != 0)
                                                   ? 1 / EmParameters.CurrentHourlyOpRecord.MatsHourLoad.Value
                                                   : (decimal?)null;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "MATSCHV3");
            }

            return ReturnVal;
        }

        public static string MATSCHV4(cCategory Category, ref bool Log)
        //This check sets generic parameters and output parameters for subsequent Calculated hourly checks for SO2RE.
        {
            string ReturnVal = "";

            try
            {
                EmParameters.CalculationConversionFactor = ECMPS.Definitions.Extensions.cExtensions.ScientificNotationtoDecimal("1.66E-7");
                EmParameters.CurrentDhvParameter = EmParameters.MatsSo2DhvParameter;
                EmParameters.CurrentDhvRecordValid = EmParameters.MatsSo2DhvValid;
                EmParameters.MatsDhvRecord = EmParameters.MatsSo2DhvRecord;
                EmParameters.MatsMoistureEquationList = "S-3";

                EmParameters.MatsDhvMeasuredModcList = "36,39";
                EmParameters.MatsDhvUnavailableModcList = "38";

                EmParameters.FinalConversionFactor = (EmParameters.CurrentHourlyOpRecord.MatsHourLoad.Default(0m) != 0)
                                                   ? 1 / EmParameters.CurrentHourlyOpRecord.MatsHourLoad.Value
                                                   : (decimal?)null;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "MATSCHV4");
            }

            return ReturnVal;
        }

        public static string MATSCHV5(cCategory Category, ref bool Log)
        //This check sets generic parameters and output parameters for subsequent Calculated hourly checks for HGRH. 
        {
            string ReturnVal = "";

            try
            {
                EmParameters.CalculationConversionFactor = ECMPS.Definitions.Extensions.cExtensions.ScientificNotationtoDecimal("6.24E-11");
                EmParameters.CurrentDhvParameter = EmParameters.MatsHgDhvParameter;
                EmParameters.CurrentDhvRecordValid = EmParameters.MatsHgDhvValid;
                EmParameters.MatsDhvRecord = EmParameters.MatsHgDhvRecord;
                EmParameters.MatsMhvCalculatedValue = EmParameters.MatsMhvCalculatedHgcValue;
                EmParameters.MatsMhvRecord = EmParameters.MatsHgcMhvRecord;
                EmParameters.MatsMoistureEquationList = "19-3,19-3D,19-4,19-5,19-8,19-9";

                EmParameters.MatsDhvMeasuredModcList = "36,37";
                EmParameters.MatsDhvUnavailableModcList = "38";

                EmParameters.FinalConversionFactor = 1000000m;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "MATSCHV5");
            }

            return ReturnVal;
        }

        public static string MATSCHV6(cCategory Category, ref bool Log)
        //This check sets generic parameters and output parameters for subsequent Calculated hourly checks for HCLRH. 
        {
            string ReturnVal = "";

            try
            {
                EmParameters.CalculationConversionFactor = ECMPS.Definitions.Extensions.cExtensions.ScientificNotationtoDecimal("9.43E-8");
                EmParameters.CurrentDhvParameter = EmParameters.MatsHclDhvParameter;
                EmParameters.CurrentDhvRecordValid = EmParameters.MatsHclDhvValid;
                EmParameters.MatsDhvRecord = EmParameters.MatsHclDhvRecord;
                EmParameters.MatsMhvCalculatedValue = EmParameters.MatsMhvCalculatedHclcValue;
                EmParameters.MatsMhvRecord = EmParameters.MatsHclcMhvRecord;
                EmParameters.MatsMoistureEquationList = "19-3,19-3D,19-4,19-5,19-8,19-9";

                EmParameters.MatsDhvMeasuredModcList = "36,37";
                EmParameters.MatsDhvUnavailableModcList = "38";

                EmParameters.FinalConversionFactor = 1m;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "MATSCHV6");
            }

            return ReturnVal;
        }

        public static string MATSCHV7(cCategory Category, ref bool Log)
        //This check sets generic parameters and output parameters for subsequent Calculated hourly checks for HFRH. 
        {
            string ReturnVal = "";

            try
            {
                EmParameters.CalculationConversionFactor = ECMPS.Definitions.Extensions.cExtensions.ScientificNotationtoDecimal("5.18E-8");
                EmParameters.CurrentDhvParameter = EmParameters.MatsHfDhvParameter;
                EmParameters.CurrentDhvRecordValid = EmParameters.MatsHfDhvValid;
                EmParameters.MatsDhvRecord = EmParameters.MatsHfDhvRecord;
                EmParameters.MatsMhvCalculatedValue = EmParameters.MatsMhvCalculatedHfcValue;
                EmParameters.MatsMhvRecord = EmParameters.MatsHfcMhvRecord;
                EmParameters.MatsMoistureEquationList = "19-3,19-3D,19-4,19-5,19-8,19-9";

                EmParameters.MatsDhvMeasuredModcList = "36,37";
                EmParameters.MatsDhvUnavailableModcList = "38";

                EmParameters.FinalConversionFactor = 1m;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "MATSCHV7");
            }

            return ReturnVal;
        }

        public static string MATSCHV8(cCategory Category, ref bool Log)
        //This check sets generic parameters and output parameters for subsequent Calculated hourly checks for SO2RH. 
        {
            string ReturnVal = "";

            try
            {
                EmParameters.CalculationConversionFactor = ECMPS.Definitions.Extensions.cExtensions.ScientificNotationtoDecimal("1.66E-7");
                EmParameters.CurrentDhvParameter = EmParameters.MatsSo2DhvParameter;
                EmParameters.CurrentDhvRecordValid = EmParameters.MatsSo2DhvValid;
                EmParameters.MatsDhvRecord = EmParameters.MatsSo2DhvRecord;
                EmParameters.MatsMoistureEquationList = "19-3,19-3D,19-4,19-5,19-8,19-9";

                EmParameters.MatsDhvMeasuredModcList = "36,37";
                EmParameters.MatsDhvUnavailableModcList = "38";

                EmParameters.FinalConversionFactor = 1m;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "MATSCHV8");
            }

            return ReturnVal;
        }

        #region MATSCHV9
        public static string MATSCHV9(cCategory Category, ref bool Log)
        //Determines the main concentration value to use in calculations.
        {
            string ReturnVal = "";

            try
            {
                EmParameters.CalculationConcentration = null;
                EmParameters.CalculationConcentrationSubstituted = false;

                if ((bool)EmParameters.CurrentDhvRecordValid && (EmParameters.MatsDhvRecord.ModcCd.InList(EmParameters.MatsDhvMeasuredModcList.ToString())))
                {
                    if (EmParameters.MatsMhvCalculatedValue != null)
                    {
                        EmParameters.CalculationConcentration = EmParameters.MatsMhvCalculatedValue.ScientificNotationtoDecimal();
                    }
                    if ((EmParameters.MatsMhvRecord != null) && EmParameters.MatsMhvRecord.ModcCd.InList("34,35"))
                    {
                        EmParameters.CalculationConcentrationSubstituted = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "MATSCHV9");
            }

            return ReturnVal;
        }
        #endregion

        #region MATSCHV10
        public static string MATSCHV10(cCategory Category, ref bool Log)
        //Determines the SO2 concentration value to use in calculations.
        {
            string ReturnVal = "";

            try
            {
                EmParameters.CalculationConcentration = null;
                EmParameters.CalculationConcentrationSubstituted = false;

                if ((bool)EmParameters.CurrentDhvRecordValid && (EmParameters.MatsDhvRecord.ModcCd.InList(EmParameters.MatsDhvMeasuredModcList.ToString())))
                {
                    if (EmParameters.CurrentSo2MonitorHourlyRecord != null)
                    {
                        EmParameters.CalculationConcentration = EmParameters.CurrentSo2MonitorHourlyRecord.UnadjustedHrlyValue;

                        if (EmParameters.CurrentSo2MonitorHourlyRecord.ModcCd.InList("05,06,07,08,09,10,12,13,15,18,23,55"))
                        {
                            EmParameters.CalculationConcentrationSubstituted = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "MATSCHV10");
            }

            return ReturnVal;
        }
        #endregion

        #endregion

        #region Checks 11-20

        #region MATSCHV11
        public static string MATSCHV11(cCategory Category, ref bool Log)
        //
        {
            string ReturnVal = "";

            try
            {
                EmParameters.CalculationFlow = null;
                EmParameters.CalculationFlowSubstituted = false;

                if (EmParameters.CurrentDhvRecordValid.Default(false) && (EmParameters.MatsDhvRecord.ModcCd.InList(EmParameters.MatsDhvMeasuredModcList.ToString())))
                {
                    if (EmParameters.CurrentFlowMonitorHourlyRecord != null)
                    {
                        EmParameters.CalculationFlow = EmParameters.CurrentFlowMonitorHourlyRecord.UnadjustedHrlyValue;

                        if (EmParameters.CurrentFlowMonitorHourlyRecord.ModcCd.NotInList("01,02,03,04,20,53,54"))
                        {
                            EmParameters.CalculationFlowSubstituted = true;
                        }
                    }
                    else
                    {
                        EmParameters.CalculationFlow = null;
                        EmParameters.CalculationFlowSubstituted = false;
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "MATSCHV11");
            }

            return ReturnVal;
        }
        #endregion

        /// <summary>
        /// MatsChv-12:
        /// 
        /// Determine the Calculation Diluent Value
        /// </summary>
        /// <param name="Category">Category object for category in which the check is running.</param>
        /// <param name="Log">Obsolete flag indicating whether to update the log.</param>
        /// <returns></returns>
        public static string MATSCHV12(cCategory Category, ref bool Log)
        //
        {
            string ReturnVal = "";

            try
            {
                EmParameters.CalculationDiluent = null;
                EmParameters.CalculationDiluentSubstituted = false;

                if ((bool)EmParameters.CurrentDhvRecordValid && (EmParameters.MatsDhvRecord.ModcCd.InList(EmParameters.MatsDhvMeasuredModcList.ToString())))
                {
                    if (EmParameters.MatsDhvRecord.EquationCd.InList("19-3D,19-5D") || EmParameters.MatsDhvRecord.ModcCd == "37")
                    {
                        if (EmParameters.MatsDhvRecord.EquationCd.InList("19-1,19-2,19-3,19-3D,19-4,19-5,19-5D"))
                        {
                            DataView O2MonitorDefaultRecords = cRowFilter.FindRows(EmParameters.MonitorDefaultRecordsByHourLocation.SourceView,
                                new cFilterCondition[] {
                                new cFilterCondition("PARAMETER_CD", "O2X"),
                                new cFilterCondition("DEFAULT_PURPOSE_CD", "DC"),
                                new cFilterCondition("FUEL_CD", "NFS")
                            });
                            int O2MonitorDefaultMatches = O2MonitorDefaultRecords.Count;

                            if (O2MonitorDefaultMatches > 1)
                            {
                                Category.CheckCatalogResult = "A";
                            }
                            else if (O2MonitorDefaultMatches == 0)
                            {
                                Category.CheckCatalogResult = "B";
                            }
                            else
                            {
                                DataRowView O2MonitorDefaultRecord = O2MonitorDefaultRecords[0];
                                if (O2MonitorDefaultRecord["DEFAULT_VALUE"] == null || O2MonitorDefaultRecord["DEFAULT_VALUE"].AsDecimal() <= 0)
                                {
                                    Category.CheckCatalogResult = "C";
                                }
                                else
                                {
                                    EmParameters.CalculationDiluent = O2MonitorDefaultRecord["DEFAULT_VALUE"].AsDecimal();
                                }
                            }
                        }
                        else if (EmParameters.MatsDhvRecord.EquationCd.InList("19-6,19-7,19-8,19-9"))
                        {
                            DataView Co2MonitorDefaultRecords = cRowFilter.FindRows(EmParameters.MonitorDefaultRecordsByHourLocation.SourceView,
                                new cFilterCondition[] {
                                new cFilterCondition("PARAMETER_CD", "CO2N"),
                                new cFilterCondition("DEFAULT_PURPOSE_CD", "DC"),
                                new cFilterCondition("FUEL_CD", "NFS")
                            });
                            int Co2MonitorDefaultMatches = Co2MonitorDefaultRecords.Count;
                            if (Co2MonitorDefaultMatches > 1)
                            {
                                Category.CheckCatalogResult = "D";
                            }
                            else if (Co2MonitorDefaultMatches == 0)
                            {
                                Category.CheckCatalogResult = "E";
                            }
                            else
                            {
                                DataRowView Co2MonitorDefaultRecord = Co2MonitorDefaultRecords[0];
                                if (Co2MonitorDefaultRecord["DEFAULT_VALUE"] == null || Co2MonitorDefaultRecord["DEFAULT_VALUE"].AsDecimal() <= 0)
                                {
                                    Category.CheckCatalogResult = "F";
                                }
                                else
                                {
                                    EmParameters.CalculationDiluent = Co2MonitorDefaultRecord["DEFAULT_VALUE"].AsDecimal();
                                }
                            }
                        }
                    }

                    else //not Equation 19-3D,19-5D or Modc 14
                        if (EmParameters.MatsDhvRecord.EquationCd.InList("19-1,19-4") && EmParameters.O2DryNeededForMats == true)
                    {
                        EmParameters.CalculationDiluent = EmParameters.O2DryCalculatedAdjustedValue;
                        if (EmParameters.O2DryModc.NotInList("01,02,03,04,17,20,53,54"))
                        {
                            EmParameters.CalculationDiluentSubstituted = true;
                        }
                    }
                    else if (EmParameters.MatsDhvRecord.EquationCd.InList("19-2,19-3,19-5") && EmParameters.O2WetNeededForMats == true)
                    {
                        EmParameters.CalculationDiluent = EmParameters.O2WetCalculatedAdjustedValue;

                        if (EmParameters.O2WetModc.NotInList("01,02,03,04,17,20,53,54"))
                        {
                            EmParameters.CalculationDiluentSubstituted = true;
                        }
                    }
                    else if (EmParameters.MatsDhvRecord.EquationCd.InList("19-6,19-7,19-8,19-9") && EmParameters.Co2DiluentNeededForMats == true)
                    {
                        EmParameters.CalculationDiluent = EmParameters.Co2cMhvCalculatedAdjustedValue;
                        if (EmParameters.Co2cMhvModc.NotInList("01,02,03,04,17,20,21,53,54"))
                        {
                            EmParameters.CalculationDiluentSubstituted = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "MATSCHV12");
            }

            return ReturnVal;
        }

        #region MATSCHV13
        public static string MATSCHV13(cCategory Category, ref bool Log)
        //
        {
            string ReturnVal = "";

            try
            {
                EmParameters.CalculationMoisture = null;
                EmParameters.CalculationMoistureSubstituted = false;

                if ((bool)EmParameters.CurrentDhvRecordValid && EmParameters.MatsDhvRecord.ModcCd.InList(EmParameters.MatsDhvMeasuredModcList.ToString()))
                {
                    if (EmParameters.MatsDhvRecord.EquationCd.InList(EmParameters.MatsMoistureEquationList.ToString()))
                    {
                        if ((EmParameters.H2oMethodCode == "MWD") && (bool)EmParameters.H2oDerivedHourlyChecksNeeded && (EmParameters.H2oDhvCalculatedAdjustedValue != null))
                        {
                            EmParameters.CalculationMoisture = EmParameters.H2oDhvCalculatedAdjustedValue;

                            if (EmParameters.H2oDhvModc.NotInList("01,02,03,04,05,06,07,08,09,10,12,21,53,54,55"))
                            {
                                EmParameters.CalculationMoistureSubstituted = true;
                            }
                        }
                        else if (EmParameters.H2oMethodCode.InList("MMS,MTB") && (bool)EmParameters.H2oMonitorHourlyChecksNeeded && EmParameters.H2oMhvCalculatedAdjustedValue != null)
                        {
                            EmParameters.CalculationMoisture = EmParameters.H2oMhvCalculatedAdjustedValue;

                            if (EmParameters.H2oMhvModc.NotInList("01,02,03,04,06,07,08,09,10,12,21,53,54,55"))
                            {
                                EmParameters.CalculationMoistureSubstituted = true;
                            }
                        }
                        else if (EmParameters.H2oMethodCode == "MDF" && (bool)EmParameters.H2oDerivedHourlyChecksNeeded && EmParameters.H2oDhvCalculatedAdjustedValue != null)
                        {
                            EmParameters.CalculationMoisture = EmParameters.H2oDhvCalculatedAdjustedValue;

                            if (EmParameters.H2oDhvModc.NotInList("01,02,03,04,05,06,07,08,09,10,12,21,53,54,55"))
                            {
                                EmParameters.CalculationMoistureSubstituted = true;
                            }
                        }
                        else if (EmParameters.H2oMethodCode == "MDF" && (bool)EmParameters.H2oDerivedHourlyChecksNeeded == false && EmParameters.H2oDefaultValue != null)
                        {
                            EmParameters.CalculationMoisture = EmParameters.H2oDefaultValue;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "MATSCHV13");
            }

            return ReturnVal;
        }
        #endregion

        /// <summary>
        /// MatsChv-14: Check MODC and determine the MATS Formula Calculated Unadjusted Value
        /// </summary>
        /// <param name="Category">Check category in which the check is running.</param>
        /// <param name="Log">Obsolete parameter.</param>
        /// <returns>Returns the exception message if the check fails to run.</returns>
        public static string MATSCHV14(cCategory Category, ref bool Log)
        {
            string ReturnVal = "";

            try
            {
                EmParameters.CalculatedUnadjustedValue = null;

                if (EmParameters.CurrentDhvRecordValid.Default(false))
                {
                    /* Measured MODC */
                    if (EmParameters.MatsDhvRecord.ModcCd.InList(EmParameters.MatsDhvMeasuredModcList))
                    {
                        if (EmParameters.MatsDhvRecord.EquationCd != null)
                        {
                            /* Moisture Equation */
                            if (EmParameters.MatsDhvRecord.EquationCd.InList(EmParameters.MatsMoistureEquationList))
                            {
                                /* An equation input was substituted */
                                if (EmParameters.CalculationConcentrationSubstituted.Default(false) || EmParameters.CalculationFlowSubstituted.Default(false) || EmParameters.CalculationMoistureSubstituted.Default(false))
                                {
                                    Category.CheckCatalogResult = "A";
                                }

                                /* An equation input is null */
                                else if ((EmParameters.CalculationConcentration == null) || (EmParameters.CalculationFlow == null) || (EmParameters.CalculationMoisture == null))
                                {
                                    Category.CheckCatalogResult = "B";
                                }

                                /* Calculate that baby! */
                                else if (EmParameters.FinalConversionFactor.HasValue)
                                {
                                    EmParameters.CalculatedUnadjustedValue = EmParameters.CalculationConversionFactor.Value
                                                                           * EmParameters.CalculationConcentration.Value
                                                                           * EmParameters.CalculationFlow.Value
                                                                           * (1m - EmParameters.CalculationMoisture.Value / 100)
                                                                           * EmParameters.FinalConversionFactor.Value;
                                }
                            }

                            /* Non Moisture Equation */
                            else
                            {
                                /* An equation input was substituted */
                                if (EmParameters.CalculationConcentrationSubstituted.Default(false) || EmParameters.CalculationFlowSubstituted.Default(false))
                                {
                                    Category.CheckCatalogResult = "C";
                                }

                                /* An equation input is null */
                                else if ((EmParameters.CalculationConcentration == null) || (EmParameters.CalculationFlow == null))
                                {
                                    Category.CheckCatalogResult = "D";
                                }

                                /* Calculate that baby! */
                                else if (EmParameters.FinalConversionFactor.HasValue)
                                {
                                    EmParameters.CalculatedUnadjustedValue = EmParameters.CalculationConversionFactor.Value
                                                                           * EmParameters.CalculationConcentration.Value
                                                                           * EmParameters.CalculationFlow.Value
                                                                           * EmParameters.FinalConversionFactor.Value;
                                }
                            }
                        }
                        else
                        {
                            Category.CheckCatalogResult = "E";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "MATSCHV14");
            }

            return ReturnVal;
        }

        /// <summary>
        /// MatsChv-15: Check MODC and determine the Formula 19 Calculated Unadjusted Value
        /// </summary>
        /// <param name="Category">Check category in which the check is running.</param>
        /// <param name="Log">Obsolete parameter.</param>
        /// <returns>Returns the exception message if the check fails to run.</returns>
        public static string MATSCHV15(cCategory Category, ref bool Log)
        {
            string ReturnVal = "";

            try
            {
                EmParameters.CalculatedUnadjustedValue = null;

                if (EmParameters.CurrentDhvRecordValid.Default(false))
                {
                    /* Measured MODC */
                    if (EmParameters.MatsDhvRecord.ModcCd.InList(EmParameters.MatsDhvMeasuredModcList))
                    {
                        if (EmParameters.MatsDhvRecord.EquationCd != null)
                        {
                            switch (EmParameters.MatsDhvRecord.EquationCd)
                            {

                                case "19-1":
                                    {
                                        if (EmParameters.CalculationConcentrationSubstituted.Default(true) || EmParameters.CalculationDiluentSubstituted.Default(true))
                                        {
                                            Category.CheckCatalogResult = "A";
                                        }
                                        else if (EmParameters.CalculationDiluent == null || EmParameters.CalculationConcentration == null || EmParameters.ValidFdFactorExists.Default(false) == false)
                                        {
                                            Category.CheckCatalogResult = "C";
                                        }
                                        else if (EmParameters.CalculationDiluent == (decimal)20.9)
                                        {
                                            Category.CheckCatalogResult = "D";
                                        }
                                        else if (EmParameters.FinalConversionFactor.HasValue)
                                        {
                                            EmParameters.CalculatedUnadjustedValue = (EmParameters.CalculationConversionFactor * EmParameters.CalculationConcentration * EmParameters.CurrentHourlyOpRecord.FdFactor * ((decimal)20.9 / ((decimal)20.9 - EmParameters.CalculationDiluent))) * EmParameters.FinalConversionFactor.Value;
                                        }
                                        break;
                                    }
                                case "19-2":
                                    {
                                        if (EmParameters.CalculationConcentrationSubstituted.Default(true) || EmParameters.CalculationDiluentSubstituted.Default(true))
                                        {
                                            Category.CheckCatalogResult = "A";
                                        }
                                        else
                                        {
                                            decimal? MoistureFraction = null;
                                            DataView BwaDefaultRecords = cRowFilter.FindRows(EmParameters.MonitorDefaultRecordsByHourLocation.SourceView,
                                                                                                              new cFilterCondition[] {
                                                                                        new cFilterCondition("PARAMETER_CD", "BWA")
                                                                                                          });

                                            int BwaDefaultRecordCount = BwaDefaultRecords.Count;

                                            if (BwaDefaultRecordCount == 0)
                                            {
                                                MoistureFraction = (decimal)0.027;
                                            }

                                            else if (BwaDefaultRecordCount == 1)
                                            {
                                                decimal DefaultValue = (decimal)BwaDefaultRecords[0]["DEFAULT_VALUE"];
                                                if (DefaultValue > (decimal)0 && DefaultValue < (decimal)1)
                                                {
                                                    MoistureFraction = DefaultValue;
                                                }
                                            }
                                            else
                                            {
                                                Category.CheckCatalogResult = "F";
                                            }

                                            if (Category.CheckCatalogResult == null)
                                            {
                                                if (EmParameters.CalculationDiluent == null || EmParameters.CalculationConcentration == null || EmParameters.ValidFwFactorExists.Default(false) == false || MoistureFraction == null)
                                                {
                                                    Category.CheckCatalogResult = "C";
                                                }
                                                else if (EmParameters.CalculationDiluent == (decimal)20.9 * (1 - MoistureFraction))
                                                {
                                                    Category.CheckCatalogResult = "D";
                                                }
                                                else if (EmParameters.FinalConversionFactor.HasValue)
                                                {
                                                    EmParameters.CalculatedUnadjustedValue = (EmParameters.CalculationConversionFactor * EmParameters.CalculationConcentration * EmParameters.CurrentHourlyOpRecord.FwFactor * ((decimal)20.9 / ((decimal)20.9 * (1 - MoistureFraction) - EmParameters.CalculationDiluent))) * EmParameters.FinalConversionFactor.Value;
                                                }
                                            }
                                        }
                                        break;
                                    }

                                case "19-3":
                                    {
                                        if (EmParameters.CalculationConcentrationSubstituted.Default(true) || EmParameters.CalculationDiluentSubstituted.Default(true) || EmParameters.CalculationMoistureSubstituted.Default(true))
                                        {
                                            Category.CheckCatalogResult = "A";
                                        }
                                        else if (EmParameters.CalculationDiluent == null || EmParameters.CalculationConcentration == null || EmParameters.ValidFdFactorExists.Default(false) == false || EmParameters.CalculationMoisture == null)
                                        {
                                            Category.CheckCatalogResult = "C";
                                        }
                                        else if (EmParameters.CalculationDiluent == (decimal)20.9 * ((100 - EmParameters.CalculationMoisture) / 100))
                                        {
                                            Category.CheckCatalogResult = "D";
                                        }
                                        else if (EmParameters.FinalConversionFactor.HasValue)
                                        {
                                            decimal h2oFactor = ((decimal)100 - (decimal)EmParameters.CalculationMoisture) / (decimal)100.0;
                                            decimal denom = ((decimal)20.9 * h2oFactor) - (decimal)EmParameters.CalculationDiluent;

                                            EmParameters.CalculatedUnadjustedValue = (EmParameters.CalculationConversionFactor * EmParameters.CalculationConcentration * EmParameters.CurrentHourlyOpRecord.FdFactor * ((decimal)20.9 / denom)) * EmParameters.FinalConversionFactor.Value;
                                        }

                                        break;
                                    }

                                case "19-3D":
                                    {
                                        if (EmParameters.CalculationConcentrationSubstituted.Default(true) || EmParameters.CalculationDiluentSubstituted.Default(true) || EmParameters.CalculationMoistureSubstituted.Default(true))
                                        {
                                            Category.CheckCatalogResult = "A";
                                        }
                                        else if (EmParameters.CalculationDiluent == null || EmParameters.CalculationConcentration == null || EmParameters.ValidFdFactorExists.Default(false) == false || EmParameters.CalculationMoisture == null)
                                        {
                                            Category.CheckCatalogResult = "C";
                                        }

                                        else if (EmParameters.CalculationDiluent == (decimal)20.9)
                                        {
                                            Category.CheckCatalogResult = "D";
                                        }

                                        else if (EmParameters.FinalConversionFactor.HasValue)
                                        {
                                            decimal h2oFactor = ((decimal)100 - (decimal)EmParameters.CalculationMoisture) / (decimal)100.0;
                                            decimal denom = ((decimal)20.9 * h2oFactor) - ((decimal)EmParameters.CalculationDiluent * h2oFactor);

                                            EmParameters.CalculatedUnadjustedValue = (EmParameters.CalculationConversionFactor * EmParameters.CalculationConcentration * EmParameters.CurrentHourlyOpRecord.FdFactor * ((decimal)20.9 / denom)) * EmParameters.FinalConversionFactor.Value;
                                        }

                                        break;
                                    }

                                case "19-4":
                                    {
                                        if (EmParameters.CalculationConcentrationSubstituted.Default(true) || EmParameters.CalculationDiluentSubstituted.Default(true) || EmParameters.CalculationMoistureSubstituted.Default(true))
                                        {
                                            Category.CheckCatalogResult = "A";
                                        }
                                        else if (EmParameters.CalculationDiluent == null || EmParameters.CalculationConcentration == null || EmParameters.ValidFdFactorExists.Default(false) == false || EmParameters.CalculationMoisture == null)
                                        {
                                            Category.CheckCatalogResult = "C";
                                        }
                                        else if (EmParameters.CalculationDiluent == (decimal)20.9 || EmParameters.CalculationMoisture == 100)
                                        {
                                            Category.CheckCatalogResult = "D";
                                        }
                                        else if (EmParameters.FinalConversionFactor.HasValue)
                                        {
                                            EmParameters.CalculatedUnadjustedValue = (EmParameters.CalculationConversionFactor * ((EmParameters.CalculationConcentration * EmParameters.CurrentHourlyOpRecord.FdFactor) / ((100 - EmParameters.CalculationMoisture) / (decimal)100.0)) * ((decimal)20.9 / ((decimal)20.9 - EmParameters.CalculationDiluent))) * EmParameters.FinalConversionFactor.Value;
                                        }

                                        break;
                                    }
                                case "19-5":
                                    {
                                        if (EmParameters.CalculationConcentrationSubstituted.Default(true) || EmParameters.CalculationDiluentSubstituted.Default(true) || EmParameters.CalculationMoistureSubstituted.Default(true))
                                        {
                                            Category.CheckCatalogResult = "A";
                                        }
                                        else if (EmParameters.CalculationDiluent == null || EmParameters.CalculationConcentration == null || EmParameters.ValidFdFactorExists.Default(false) == false || EmParameters.CalculationMoisture == null)
                                        {
                                            Category.CheckCatalogResult = "C";
                                        }
                                        else if (EmParameters.CalculationDiluent == (decimal)20.9 || EmParameters.CalculationMoisture == 100)
                                        {
                                            Category.CheckCatalogResult = "D";
                                        }

                                        else if (EmParameters.FinalConversionFactor.HasValue)
                                        {
                                            decimal h2oFactor = (100 - (decimal)EmParameters.CalculationMoisture) / (decimal)100.0;
                                            decimal denom = (decimal)20.9 - (decimal)EmParameters.CalculationDiluent / h2oFactor;

                                            EmParameters.CalculatedUnadjustedValue = (EmParameters.CalculationConversionFactor * EmParameters.CalculationConcentration * EmParameters.CurrentHourlyOpRecord.FdFactor * (decimal)20.9 / denom) * EmParameters.FinalConversionFactor.Value;
                                        }


                                        break;
                                    }

                                case "19-5D":
                                    {
                                        if (EmParameters.CalculationConcentrationSubstituted.Default(true) || EmParameters.CalculationDiluentSubstituted.Default(true))
                                        {
                                            Category.CheckCatalogResult = "A";
                                        }
                                        else if (EmParameters.CalculationDiluent == null || EmParameters.CalculationConcentration == null || EmParameters.ValidFdFactorExists.Default(false) == false)
                                        {
                                            Category.CheckCatalogResult = "C";
                                        }
                                        else if (EmParameters.CalculationDiluent == (decimal)20.9)
                                        {
                                            Category.CheckCatalogResult = "D";
                                        }
                                        else if (EmParameters.FinalConversionFactor.HasValue)
                                        {
                                            EmParameters.CalculatedUnadjustedValue = (EmParameters.CalculationConversionFactor * EmParameters.CalculationConcentration * EmParameters.CurrentHourlyOpRecord.FdFactor * ((decimal)20.9 / ((decimal)20.9 - EmParameters.CalculationDiluent))) * EmParameters.FinalConversionFactor.Value;
                                        }

                                        break;
                                    }

                                case "19-6":
                                case "19-7":
                                case "":
                                    {
                                        if (EmParameters.CalculationConcentrationSubstituted.Default(true) || EmParameters.CalculationDiluentSubstituted.Default(true))
                                        {
                                            Category.CheckCatalogResult = "A";
                                        }
                                        else if (EmParameters.CalculationDiluent == null || EmParameters.CalculationConcentration == null || EmParameters.ValidFcFactorExists.Default(false) == false)
                                        {
                                            Category.CheckCatalogResult = "C";
                                        }

                                        else if (EmParameters.CalculationDiluent == (decimal)0.0)
                                        {
                                            Category.CheckCatalogResult = "D";
                                        }

                                        else if (EmParameters.FinalConversionFactor.HasValue)
                                        {
                                            EmParameters.CalculatedUnadjustedValue = (EmParameters.CalculationConversionFactor * EmParameters.CalculationConcentration * EmParameters.CurrentHourlyOpRecord.FcFactor * ((decimal)100.0 / EmParameters.CalculationDiluent)) * EmParameters.FinalConversionFactor.Value;
                                        }
                                        break;
                                    }

                                case "19-8":
                                    {
                                        if (EmParameters.CalculationConcentrationSubstituted.Default(true) || EmParameters.CalculationDiluentSubstituted.Default(true) || EmParameters.CalculationMoistureSubstituted.Default(true))
                                        {
                                            Category.CheckCatalogResult = "A";
                                        }
                                        else if (EmParameters.CalculationDiluent == null || EmParameters.CalculationConcentration == null || EmParameters.ValidFcFactorExists.Default(false) == false || EmParameters.CalculationMoisture == null)
                                        {
                                            Category.CheckCatalogResult = "C";
                                        }
                                        else if (EmParameters.CalculationDiluent == (decimal)0.0 || EmParameters.CalculationMoisture == 100)
                                        {
                                            Category.CheckCatalogResult = "D";
                                        }
                                        else if (EmParameters.FinalConversionFactor.HasValue)
                                        {
                                            EmParameters.CalculatedUnadjustedValue = (EmParameters.CalculationConversionFactor * ((EmParameters.CalculationConcentration * EmParameters.CurrentHourlyOpRecord.FcFactor) / (((decimal)100 - EmParameters.CalculationMoisture) / (decimal)100.0)) * ((decimal)100.0 / EmParameters.CalculationDiluent)) * EmParameters.FinalConversionFactor.Value;
                                        }
                                        break;
                                    }

                                case "19-9":
                                    {
                                        if (EmParameters.CalculationConcentrationSubstituted.Default(true) || EmParameters.CalculationDiluentSubstituted.Default(true) || EmParameters.CalculationMoistureSubstituted.Default(true))
                                        {
                                            Category.CheckCatalogResult = "A";
                                        }
                                        else if (EmParameters.CalculationDiluent == null || EmParameters.CalculationConcentration == null || EmParameters.ValidFcFactorExists.Default(false) == false || EmParameters.CalculationMoisture == null)
                                        {
                                            Category.CheckCatalogResult = "C";
                                        }
                                        else if (EmParameters.CalculationDiluent == (decimal)0.0)
                                        {
                                            Category.CheckCatalogResult = "D";
                                        }
                                        else if (EmParameters.FinalConversionFactor.HasValue)
                                        {
                                            decimal h2oFactor = (100 - (decimal)EmParameters.CalculationMoisture) / (decimal)100.0;
                                            decimal co2Term = (decimal)100.0 / (decimal)EmParameters.CalculationDiluent;

                                            EmParameters.CalculatedUnadjustedValue = (EmParameters.CalculationConversionFactor * EmParameters.CalculationConcentration * EmParameters.CurrentHourlyOpRecord.FcFactor * h2oFactor * co2Term) * EmParameters.FinalConversionFactor.Value;
                                        }
                                        break;
                                    }
                            }
                        }
                        else
                        {
                            Category.CheckCatalogResult = "B";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "MATSCHV15");
            }

            return ReturnVal;
        }

        #region MATSCHV16
        public static string MATSCHV16(cCategory Category, ref bool Log)
        //Stores the HGRE or HGRH Calculated Unadjusted Value in the appropriate check parameters.
        {
            string ReturnVal = "";

            try
            {
                EmParameters.MatsCalculatedHgRateValue = EmParameters.CalculatedUnadjustedValue.DecimaltoScientificNotation();
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "MATSCHV16");
            }

            return ReturnVal;
        }
        #endregion

        #region MATSCHV17
        public static string MATSCHV17(cCategory Category, ref bool Log)
        //Stores the HCLRE or HCLRH Calculated Unadjusted Value in the appropriate check parameters.
        {
            string ReturnVal = "";

            try
            {
                EmParameters.MatsCalculatedHclRateValue = EmParameters.CalculatedUnadjustedValue.DecimaltoScientificNotation();
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "MATSCHV17");
            }

            return ReturnVal;
        }
        #endregion

        #region MATSCHV18
        public static string MATSCHV18(cCategory Category, ref bool Log)
        //Stores the HFRE or HFRH Calculated Unadjusted Value in the appropriate check parameters.
        {
            string ReturnVal = "";

            try
            {
                EmParameters.MatsCalculatedHfRateValue = EmParameters.CalculatedUnadjustedValue.DecimaltoScientificNotation();
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "MATSCHV18");
            }

            return ReturnVal;
        }
        #endregion

        #region MATSCHV19
        public static string MATSCHV19(cCategory Category, ref bool Log)
        //Stores the SO2RE or SO2RH Calculated Unadjusted Value in the appropriate check parameters.
        {
            string ReturnVal = "";

            try
            {
                EmParameters.MatsCalculatedSo2RateValue = EmParameters.CalculatedUnadjustedValue.DecimaltoScientificNotation();
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "MATSCHV19");
            }

            return ReturnVal;
        }
        #endregion

        /// <summary>
        /// MATSCHV-20: Check Unadjusted Hourly Value Tolerance
        /// 
        /// Determines whether the reported and calculated MATS rate values agree within a 5% difference.
        /// </summary>
        /// <param name="category"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public static string MATSCHV20(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {

                if (EmParameters.CurrentDhvRecordValid.Default(false))
                {
                    if (EmParameters.MatsDhvRecord.ModcCd.InList(EmParameters.MatsDhvMeasuredModcList)
                        && EmParameters.MatsDhvRecord.UnadjustedHrlyValue != null
                        && EmParameters.CalculatedUnadjustedValue != null)
                    {
                        if (EmParameters.CurrentSo2MonitorHourlyRecord == null || 
                                (EmParameters.CurrentSo2MonitorHourlyRecord.ModcCd != "16" || !EmParameters.MatsDhvRecord.ParameterCd.InList("SO2RE,SO2RH", ","))
                           )
                        {
                            decimal reportedValue = EmParameters.MatsDhvRecord.UnadjustedHrlyValue.ScientificNotationtoDecimal();
                            decimal calculatedValue = EmParameters.CalculatedUnadjustedValue.DecimaltoScientificNotation().ScientificNotationtoDecimal();

                            if ((reportedValue + calculatedValue) != 0)
                            {
                                decimal percentDifference = Math.Round(100 * Math.Abs(reportedValue - calculatedValue) / ((reportedValue + calculatedValue) / 2), 1, MidpointRounding.AwayFromZero);

                                if (percentDifference > 5m)
                                {
                                    category.CheckCatalogResult = "A";
                                }
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