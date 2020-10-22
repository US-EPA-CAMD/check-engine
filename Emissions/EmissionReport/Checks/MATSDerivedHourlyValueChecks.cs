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
    public class cMATSDerivedHourlyValueChecks : cEmissionsChecks
    {

        #region Constructors

        public cMATSDerivedHourlyValueChecks(cEmissionsReportProcess emissionReportProcess)
            : base(emissionReportProcess)
        {
            CheckProcedures = new dCheckProcedure[19];

            CheckProcedures[1] = new dCheckProcedure(MATSDHV1);
            CheckProcedures[2] = new dCheckProcedure(MATSDHV2);
            CheckProcedures[3] = new dCheckProcedure(MATSDHV3);
            CheckProcedures[4] = new dCheckProcedure(MATSDHV4);
            CheckProcedures[5] = new dCheckProcedure(MATSDHV5);
            CheckProcedures[6] = new dCheckProcedure(MATSDHV6);
            CheckProcedures[7] = new dCheckProcedure(MATSDHV7);
            CheckProcedures[8] = new dCheckProcedure(MATSDHV8);
            CheckProcedures[9] = new dCheckProcedure(MATSDHV9);
            CheckProcedures[10] = new dCheckProcedure(MATSDHV10);
            CheckProcedures[11] = new dCheckProcedure(MATSDHV11);
            CheckProcedures[12] = new dCheckProcedure(MATSDHV12);
            CheckProcedures[13] = new dCheckProcedure(MATSDHV13);
            CheckProcedures[14] = new dCheckProcedure(MATSDHV14);
            CheckProcedures[15] = new dCheckProcedure(MATSDHV15);
            CheckProcedures[16] = new dCheckProcedure(MATSDHV16);
            CheckProcedures[17] = new dCheckProcedure(MATSDHV17);
            CheckProcedures[18] = new dCheckProcedure(MATSDHV18);

        }


        #endregion

        #region Checks 1-10

        #region MATSDHV1
        public static string MATSDHV1(cCategory Category, ref bool Log)
        // This check sets generic parameters and output parameters for subsequent derived hourly checks for Hg      
        {
            string ReturnVal = "";

            try
            {
                EmParameters.CurrentDhvParameter = "HGRE";
                EmParameters.MatsDhvRecord = EmParameters.MatsHgDhvRecord;
                EmParameters.MatsEquationCodeWithH2o = "A-3";
                EmParameters.MatsEquationCodeWithoutH2o = "A-2";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "MATSDHV1");
            }

            return ReturnVal;
        }
        #endregion

        #region MATSDHV2
        public static string MATSDHV2(cCategory Category, ref bool Log)
        // This check sets generic parameters and output parameters for subsequent derived hourly checks for Hg
        {
            string ReturnVal = "";

            try
            {
                EmParameters.CurrentDhvParameter = "HGRH";
                EmParameters.MatsDhvRecord = EmParameters.MatsHgDhvRecord;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "MATSDHV2");
            }

            return ReturnVal;
        }
        #endregion

        #region MATSDHV3
        public static string MATSDHV3(cCategory Category, ref bool Log)
        // This check sets generic parameters and output parameters for subsequent derived hourly checks for HCL
        {
            string ReturnVal = "";

            try
            {
                EmParameters.CurrentDhvParameter = "HCLRE";
                EmParameters.MatsDhvRecord = EmParameters.MatsHclDhvRecord;
                EmParameters.MatsEquationCodeWithH2o = "HC-3";
                EmParameters.MatsEquationCodeWithoutH2o = "HC-2";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "MATSDHV3");
            }

            return ReturnVal;
        }
        #endregion

        #region MATSDHV4
        public static string MATSDHV4(cCategory Category, ref bool Log)
        // This check sets generic parameters and output parameters for subsequent derived hourly checks for HCL
        {
            string ReturnVal = "";

            try
            {
                EmParameters.CurrentDhvParameter = "HCLRH";
                EmParameters.MatsDhvRecord = EmParameters.MatsHclDhvRecord;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "MATSDHV4");
            }

            return ReturnVal;
        }
        #endregion

        #region MATSDHV5
        public static string MATSDHV5(cCategory Category, ref bool Log)
        // This check sets generic parameters and output parameters for subsequent derived hourly checks for HF
        {
            string ReturnVal = "";

            try
            {
                EmParameters.CurrentDhvParameter = "HFRE";
                EmParameters.MatsDhvRecord = EmParameters.MatsHfDhvRecord;
                EmParameters.MatsEquationCodeWithH2o = "HF-3";
                EmParameters.MatsEquationCodeWithoutH2o = "HF-2";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "MATSDHV5");
            }

            return ReturnVal;
        }
        #endregion

        #region MATSDHV6
        public static string MATSDHV6(cCategory Category, ref bool Log)
        // This check sets generic parameters and output parameters for subsequent derived hourly checks for HF
        {
            string ReturnVal = "";

            try
            {
                EmParameters.CurrentDhvParameter = "HFRH";
                EmParameters.MatsDhvRecord = EmParameters.MatsHfDhvRecord;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "MATSDHV6");
            }

            return ReturnVal;
        }
        #endregion

        #region MATSDHV7
        public static string MATSDHV7(cCategory Category, ref bool Log)
        // This check sets generic parameters and output parameters for subsequent derived hourly checks for SO2
        {
            string ReturnVal = "";

            try
            {
                EmParameters.CurrentDhvParameter = "SO2RE";
                EmParameters.MatsDhvRecord = EmParameters.MatsSo2DhvRecord;
                EmParameters.MatsEquationCodeWithH2o = "S-3";
                EmParameters.MatsEquationCodeWithoutH2o = "S-2";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "MATSDHV7");
            }

            return ReturnVal;
        }
        #endregion

        #region MATSDHV8
        public static string MATSDHV8(cCategory Category, ref bool Log)
        // This check sets generic parameters and output parameters for subsequent derived hourly checks for SO2
        {
            string ReturnVal = "";

            try
            {
                EmParameters.CurrentDhvParameter = "SO2RH";
                EmParameters.MatsDhvRecord = EmParameters.MatsSo2DhvRecord;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "MATSDHV8");
            }

            return ReturnVal;
        }
        #endregion

        #region MATSDHV9
        public static string MATSDHV9(cCategory Category, ref bool Log)
        // Basic check to ensure that Mats MODC reported in the DHV record is valid.
        {
            string ReturnVal = "";

            try
            {
                EmParameters.DerivedHourlyModcStatus = false;

                if (EmParameters.MatsDhvRecord.ModcCd.InList("36,38"))
                {
                    EmParameters.DerivedHourlyModcStatus = true;
                }

                else if (EmParameters.MatsDhvRecord.ModcCd == "37")
                {
                    if (EmParameters.MatsDhvRecord.ParameterCd.InList("HGRH,HCLRH,HFRH,SO2RH") && (EmParameters.CurrentHourlyOpRecord.MatsStartupShutdownFlg != null))
                    {
                        EmParameters.DerivedHourlyModcStatus = true;
                    }
                    else
                        Category.CheckCatalogResult = "B";
                }

                else if (EmParameters.MatsDhvRecord.ModcCd == "39")
                {
                    if (EmParameters.MatsDhvRecord.ParameterCd.InList("HGRE,HCLRE,HFRE,SO2RE") && (EmParameters.CurrentHourlyOpRecord.MatsStartupShutdownFlg != null))
                    {
                        EmParameters.DerivedHourlyModcStatus = true;
                    }
                    else
                        Category.CheckCatalogResult = "C";
                }

                else
                    Category.CheckCatalogResult = "A";

            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "MATSDHV9");
            }

            return ReturnVal;
        }
        #endregion

        #region MATSDHV10
        public static string MATSDHV10(cCategory Category, ref bool Log)
        // Checks the Formula ID in the MATS Derived Hourly Value record and ensures that it can be used for the calculation
        {
            string ReturnVal = "";

            try
            {
                EmParameters.DerivedHourlyFormulaStatus = false;

                if (EmParameters.DerivedHourlyModcStatus == true)
                {
                    if (EmParameters.MatsDhvRecord.MonFormId == null)
                    {
                        if (EmParameters.MatsDhvRecord.ModcCd == "38")
                        {
                            Category.CheckCatalogResult = "G";
                        }
                        else
                        {
                            Category.CheckCatalogResult = "A";
                        }
                    }

                    else  //EquationCode not null
                    {
                        if (EmParameters.MatsDhvRecord.FormulaActiveInd != 1)
                        {
                            Category.CheckCatalogResult = "B";
                        }
                        else if (EmParameters.MatsDhvRecord.FormulaParameterCd != EmParameters.CurrentDhvParameter)
                        {
                            Category.CheckCatalogResult = "C";
                        }
                        else if (EmParameters.CurrentDhvParameter.InList("HGRE,HCLRE,HFRE,SO2RE") && EmParameters.MatsDhvRecord.ModcCd == "37")
                        {
                            Category.CheckCatalogResult = "D";
                        }
                        else if (EmParameters.CurrentDhvParameter.InList("HGRH,HCLRH,HFRH,SO2RH") && EmParameters.MatsDhvRecord.ModcCd == "39")
                        {
                            Category.CheckCatalogResult = "E";
                        }
                        else
                        {
                            EmParameters.DerivedHourlyFormulaStatus = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "MATSDHV10");
            }

            return ReturnVal;
        }
        #endregion

        #endregion

        #region Checks 11-20

        #region MATSDHV11
        public static string MATSDHV11(cCategory Category, ref bool Log)
        // Gets Equation Code from Mats Active Monitor Formula Record and verifies that it is an appropriate equation for calculation of HCLRE,HFRE,HGRE,SO2RE
        {
            string ReturnVal = "";

            try
            {
                EmParameters.DerivedHourlyEquationStatus = false;

                if (EmParameters.DerivedHourlyFormulaStatus == true)
                {
                    if (EmParameters.MatsDhvRecord.EquationCd != null)
                    {

                        if (EmParameters.MatsDhvRecord.EquationCd == EmParameters.MatsEquationCodeWithoutH2o)
                        {
                            EmParameters.DerivedHourlyEquationStatus = true;
                            EmParameters.FlowMonitorHourlyChecksNeeded = true;
                        }
                        else if (EmParameters.MatsDhvRecord.EquationCd == EmParameters.MatsEquationCodeWithH2o)
                        {
                            EmParameters.DerivedHourlyEquationStatus = true;
                            EmParameters.FlowMonitorHourlyChecksNeeded = true;
                            EmParameters.MoistureNeeded = true;
                            EmParameters.H2oMissingDataApproach = EmParameters.H2oMissingDataApproach.ListAdd("MIN");
                        }
                        else
                        {
                            Category.CheckCatalogResult = "A";
                        }
                    }
                    else
                    {
                        EmParameters.DerivedHourlyEquationStatus = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "MATSDHV11");
            }

            return ReturnVal;
        }
        #endregion

        #region MATSDHV12
        public static string MATSDHV12(cCategory Category, ref bool Log)
        // Gets Mats Equation Code from Active Mats Monitor Formula Record and verifies that it is an appropriate equation for Mats Current parameter.
        {
            string ReturnVal = "";

            try
            {
                EmParameters.DerivedHourlyEquationStatus = false;

                if (EmParameters.DerivedHourlyFormulaStatus == true)
                {
                    if (EmParameters.MatsDhvRecord.EquationCd != null)
                    {

                        if (EmParameters.MatsDhvRecord.EquationCd.InList("19-1,19-2,19-3,19-3D,19-4,19-5,19-5D,19-6,19-7,19-8,19-9"))
                        {

                            EmParameters.DerivedHourlyEquationStatus = true;

                            if (EmParameters.MatsDhvRecord.EquationCd.InList("19-1,19-4"))
                            {
                                EmParameters.O2DryNeededForMats = true;
                                EmParameters.FdFactorNeeded = true;
                            }
                            else if (EmParameters.MatsDhvRecord.EquationCd.InList("19-3,19-3D,19-5,19-5D"))
                            {
                                EmParameters.O2WetNeededForMats = true;
                                EmParameters.FdFactorNeeded = true;
                            }
                            else if (EmParameters.MatsDhvRecord.EquationCd.InList("19-2"))
                            {
                                EmParameters.O2WetNeededForMats = true;
                                EmParameters.FwFactorNeeded = true;
                            }
                            else if (EmParameters.MatsDhvRecord.EquationCd.InList("19-6,19-7,19-8,19-9"))
                            {
                                EmParameters.Co2DiluentNeededForMats = true;
                                EmParameters.FcFactorNeeded = true;
                            }
                            if (EmParameters.MatsDhvRecord.EquationCd.InList("19-3,19-3D,19-4,19-5,19-8,19-9"))
                            {
                                EmParameters.MoistureNeeded = true;
                            }
                        }
                        else
                        {
                            Category.CheckCatalogResult = "A";
                        }
                    }
                    else
                    {
                        EmParameters.DerivedHourlyEquationStatus = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "MATSDHV12");
            }

            return ReturnVal;
        }
        #endregion

        #region MATSDHV13
        public static string MATSDHV13(cCategory Category, ref bool Log)
        // This check assigns parameter specific check parameters used by the associated calculation checks.
        {
            string ReturnVal = "";

            try
            {
                EmParameters.MatsHgDhvParameter = EmParameters.CurrentDhvParameter;
                EmParameters.MatsHgDhvValid = EmParameters.DerivedHourlyEquationStatus;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "MATSDHV13");
            }

            return ReturnVal;
        }
        #endregion

        #region MATSDHV14
        public static string MATSDHV14(cCategory Category, ref bool Log)
        // This check assigns parameter specific check parameters used by the associated calculation checks.
        {
            string ReturnVal = "";

            try
            {
                EmParameters.MatsHclDhvParameter = EmParameters.CurrentDhvParameter;
                EmParameters.MatsHclDhvValid = EmParameters.DerivedHourlyEquationStatus;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "MATSDHV14");
            }

            return ReturnVal;
        }
        #endregion

        #region MATSDHV15
        public static string MATSDHV15(cCategory Category, ref bool Log)
        // This check assigns parameter specific check parameters used by the associated calculation checks.
        {
            string ReturnVal = "";

            try
            {
                EmParameters.MatsHfDhvParameter = EmParameters.CurrentDhvParameter;
                EmParameters.MatsHfDhvValid = EmParameters.DerivedHourlyEquationStatus;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "MATSDHV15");
            }

            return ReturnVal;
        }
        #endregion

        #region MATSDHV16
        public static string MATSDHV16(cCategory Category, ref bool Log)
        // This check assigns parameter specific check parameters used by the associated calculation checks.
        {
            string ReturnVal = "";

            try
            {
                EmParameters.MatsSo2DhvParameter = EmParameters.CurrentDhvParameter;
                EmParameters.MatsSo2DhvValid = EmParameters.DerivedHourlyEquationStatus;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "MATSDHV16");
            }

            return ReturnVal;
        }
        #endregion

        #region MATSDHV17
        public static string MATSDHV17(cCategory Category, ref bool Log)
        // This check assigns parameter specific check parameters used by the associated calculation checks.
        {
            string ReturnVal = "";

            try
            {
                EmParameters.DerivedHourlyUnadjustedValueStatus = false;

                if (EmParameters.DerivedHourlyModcStatus == true)
                {
                    if (EmParameters.MatsDhvRecord.ModcCd.InList("36,37,39"))
                    {
                        if (string.IsNullOrEmpty(EmParameters.MatsDhvRecord.UnadjustedHrlyValue))
                        {
                            Category.CheckCatalogResult = "A";
                        }

                        else if (EmParameters.MatsDhvRecord.UnadjustedHrlyValue.ScientificNotationSignificantDigits() != 3)
                        {
                            Category.CheckCatalogResult = "B";
                        }

                        else if (EmParameters.MatsDhvRecord.UnadjustedHrlyValue.ScientificNotationtoDecimal() < 0)
                        {
                            Category.CheckCatalogResult = "C";
                        }

                        else
                        {
                            EmParameters.DerivedHourlyUnadjustedValueStatus = true;
                        }
                    }
                    else // MODC 38
                    {
                        if (!string.IsNullOrEmpty(EmParameters.MatsDhvRecord.UnadjustedHrlyValue))
                        {
                            Category.CheckCatalogResult = "D";
                        }
                        else
                        {
                            EmParameters.DerivedHourlyUnadjustedValueStatus = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "MATSDHV17");
            }

            return ReturnVal;
        }
        #endregion

        /// <summary>
        /// Sets the CO2 Diluent, O2 Dry and O2 Wet Need For MATS Calculation parameters to the CO2 Diluent, 
        /// O2 Dry and O2 Wet Need For MATS parameters respectively, if the MODC is measured.
        /// 
        /// This will enable other parts of the program to determine whether a supporting pollutant parameter
        /// is needed for calculations, or only needed for other reasons.
        /// </summary>
        /// <param name="Category"></param>
        /// <param name="Log"></param>
        /// <returns></returns>
        public static string MATSDHV18(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                if ((EmParameters.DerivedHourlyEquationStatus == true) && (EmParameters.DerivedHourlyModcStatus == true) && EmParameters.MatsDhvRecord.ModcCd.InList("36,37,39"))
                {
                    if (EmParameters.Co2DiluentNeededForMats == true)
                        EmParameters.Co2DiluentNeededForMatsCalculation = true;

                    if (EmParameters.O2DryNeededForMats == true)
                        EmParameters.O2DryNeededForMatsCalculation = true;

                    if (EmParameters.O2WetNeededForMats == true)
                        EmParameters.O2WetNeededForMatsCalculation = true;
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