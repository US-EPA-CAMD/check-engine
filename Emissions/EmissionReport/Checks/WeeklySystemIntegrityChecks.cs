using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.CheckEngine.SpecialParameterClasses;
using ECMPS.Checks.Data.Ecmps.CheckEm.Function;
using ECMPS.Checks.Em.Parameters;
using ECMPS.Checks.EmissionsReport;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Extensions;


namespace ECMPS.Checks.EmissionsChecks
{

    public class WeeklySystemIntegrityChecks : cEmissionsChecks
    {

        #region Constructors

        public WeeklySystemIntegrityChecks(cEmissionsReportProcess emissionReportProcess)
            : base(emissionReportProcess)
        {
            CheckProcedures = new dCheckProcedure[12];

            CheckProcedures[1] = new dCheckProcedure(EMWSI1);
            CheckProcedures[2] = new dCheckProcedure(EMWSI2);
            CheckProcedures[3] = new dCheckProcedure(EMWSI3);
            CheckProcedures[4] = new dCheckProcedure(EMWSI4);
            CheckProcedures[5] = new dCheckProcedure(EMWSI5);
            CheckProcedures[6] = new dCheckProcedure(EMWSI6);
            CheckProcedures[7] = new dCheckProcedure(EMWSI7);
            CheckProcedures[8] = new dCheckProcedure(EMWSI8);
            CheckProcedures[9] = new dCheckProcedure(EMWSI9);
            CheckProcedures[10] = new dCheckProcedure(EMWSI10);
            CheckProcedures[11] = new dCheckProcedure(EMWSI11);
        }

        #endregion


        #region Check Methods 1 - 10

        /// <summary>
        /// Check Hg Converter Indicator of the Component
        /// </summary>
        /// <param name="category">Category Object</param>
        /// <param name="log">Indicates whether to log results.</param>
        /// <returns>Returns error message if check fails to run correctly.</returns>
        public static string EMWSI1(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                if (EmParameters.CurrentWeeklySystemIntegrityTest.HgConverterInd != 1)
                {
                    EmParameters.WeeklyTestSummaryValid = false;
                    category.CheckCatalogResult = "A";
                }
            }
            catch (Exception ex)
            {
                returnVal = category.CheckEngine.FormatError(ex);
            }

            return returnVal;
        }

        /// <summary>
        /// Check Gas Level
        /// </summary>
        /// <param name="category">Category Object</param>
        /// <param name="log">Indicates whether to log results.</param>
        /// <returns>Returns error message if check fails to run correctly.</returns>
        public static string EMWSI2(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                if (EmParameters.CurrentWeeklySystemIntegrityTest.GasLevelCd == null)
                {
                    EmParameters.WeeklyTestSummaryValid = false;
                    category.CheckCatalogResult = "A";
                }
                else if (EmParameters.CurrentWeeklySystemIntegrityTest.GasLevelCd.NotInList("HIGH,MID,LOW,ZERO"))
                {
                    EmParameters.WeeklyTestSummaryValid = false;
                    category.CheckCatalogResult = "B";
                }
                else if (EmParameters.CurrentWeeklySystemIntegrityTest.GasLevelCd.NotInList("HIGH,MID"))
                {
                    EmParameters.WeeklyTestSummaryValid = false;
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
        /// Check Weekly System Integrity Reference Value
        /// </summary>
        /// <param name="category">Category Object</param>
        /// <param name="log">Indicates whether to log results.</param>
        /// <returns>Returns error message if check fails to run correctly.</returns>
        public static string EMWSI3(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                EmParameters.InjectionReferenceValueValid = false;

                if (EmParameters.CurrentWeeklySystemIntegrityTest.RefValue == null)
                {
                    EmParameters.WeeklyTestSummaryValid = false;
                    category.CheckCatalogResult = "A";
                }
                else if (!EmParameters.CurrentWeeklySystemIntegrityTest.RefValue.Value.IsRounded(1))
                {
                    EmParameters.WeeklyTestSummaryValid = false;
                    category.CheckCatalogResult = "B";
                }
                else if (EmParameters.CurrentWeeklySystemIntegrityTest.RefValue.Value <= 0)
                {
                    if (EmParameters.CurrentWeeklySystemIntegrityTest.TestResultCd != "FAILED")
                    {
                        EmParameters.WeeklyTestSummaryValid = false;
                        category.CheckCatalogResult = "C";
                    }
                }
                else
                {
                    EmParameters.InjectionReferenceValueValid = true;
                }
            }
            catch (Exception ex)
            {
                returnVal = category.CheckEngine.FormatError(ex);
            }

            return returnVal;
        }


        /// <summary>
        /// Check Weekly System Integrity Measured Value
        /// </summary>
        /// <param name="category">Category Object</param>
        /// <param name="log">Indicates whether to log results.</param>
        /// <returns>Returns error message if check fails to run correctly.</returns>
        public static string EMWSI4(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                EmParameters.InjectionMeasuredValueValid = false;

                if (EmParameters.CurrentWeeklySystemIntegrityTest.MeasuredValue == null)
                {
                    EmParameters.WeeklyTestSummaryValid = false;
                    category.CheckCatalogResult = "A";
                }
                else if (!EmParameters.CurrentWeeklySystemIntegrityTest.MeasuredValue.Value.IsRounded(1))
                {
                    EmParameters.WeeklyTestSummaryValid = false;
                    category.CheckCatalogResult = "B";
                }
                else if (EmParameters.CurrentWeeklySystemIntegrityTest.MeasuredValue.Value <= 0)
                {
                    if (EmParameters.CurrentWeeklySystemIntegrityTest.TestResultCd != "FAILED")
                    {
                        EmParameters.WeeklyTestSummaryValid = false;
                        category.CheckCatalogResult = "C";
                    }
                }
                else
                {
                    EmParameters.InjectionMeasuredValueValid = true;
                }
            }
            catch (Exception ex)
            {
                returnVal = category.CheckEngine.FormatError(ex);
            }

            return returnVal;
        }


        /// <summary>
        /// Calculate System Integrity Error and Alternate Performance Spec Indicator
        /// </summary>
        /// <param name="category">Category Object</param>
        /// <param name="log">Indicates whether to log results.</param>
        /// <returns>Returns error message if check fails to run correctly.</returns>
        public static string EMWSI5(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                EmParameters.CalculatedSystemIntegrityApsIndicator = null;
                EmParameters.CalculatedSystemIntegrityError = null;

                if (EmParameters.InjectionReferenceValueValid.Default(false) && EmParameters.InjectionMeasuredValueValid.Default(false))
                {
                    decimal percentError = 100
                                         * Math.Abs(EmParameters.CurrentWeeklySystemIntegrityTest.RefValue.Value - EmParameters.CurrentWeeklySystemIntegrityTest.MeasuredValue.Value)
                                         / EmParameters.CurrentWeeklySystemIntegrityTest.RefValue.Value;
                    {
                        percentError = Math.Round(percentError, 1, MidpointRounding.AwayFromZero);
                    }

                    if (percentError <= 10)
                    {
                        EmParameters.CalculatedSystemIntegrityApsIndicator = 0;
                        EmParameters.CalculatedSystemIntegrityError = percentError;
                        EmParameters.CalculatedWeeklyTestSummaryResult = "PASSED";
                    }
                    else
                    {
                        decimal absoluteError = Math.Abs(EmParameters.CurrentWeeklySystemIntegrityTest.RefValue.Value - EmParameters.CurrentWeeklySystemIntegrityTest.MeasuredValue.Value);
                        {
                            absoluteError = Math.Round(absoluteError, 1, MidpointRounding.AwayFromZero);
                        }

                        if (absoluteError <= 0.8m)
                        {
                            EmParameters.CalculatedSystemIntegrityApsIndicator = 1;
                            EmParameters.CalculatedSystemIntegrityError = absoluteError;
                            EmParameters.CalculatedWeeklyTestSummaryResult = "PASSAPS";
                        }
                        else
                        {
                            EmParameters.CalculatedSystemIntegrityApsIndicator = 0;
                            EmParameters.CalculatedSystemIntegrityError = percentError;
                            EmParameters.CalculatedWeeklyTestSummaryResult = "FAILED";
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
        /// Check Weekly System Integrity Alternative Performance Spec
        /// </summary>
        /// <param name="category">Category Object</param>
        /// <param name="log">Indicates whether to log results.</param>
        /// <returns>Returns error message if check fails to run correctly.</returns>
        public static string EMWSI6(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                EmParameters.WeeklySystemIntegrityApsIsValid = false;

                if (EmParameters.InjectionReferenceValueValid.Default(false) && EmParameters.InjectionMeasuredValueValid.Default(false))
                {
                    if (EmParameters.CurrentWeeklySystemIntegrityTest.ApsInd == null)
                    {
                        EmParameters.WeeklyTestSummaryValid = false;
                        category.CheckCatalogResult = "A";
                    }
                    else if ((EmParameters.CurrentWeeklySystemIntegrityTest.ApsInd != 0) && (EmParameters.CurrentWeeklySystemIntegrityTest.ApsInd != 1))
                    {
                        EmParameters.WeeklyTestSummaryValid = false;
                        category.CheckCatalogResult = "B";
                    }
                    else if (EmParameters.CurrentWeeklySystemIntegrityTest.ApsInd != EmParameters.CalculatedSystemIntegrityApsIndicator)
                    {
                        EmParameters.WeeklyTestSummaryValid = false;
                        category.CheckCatalogResult = "C";
                    }
                    else
                    {
                        EmParameters.WeeklySystemIntegrityApsIsValid = true;
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
        /// <param name="category">Category Object</param>
        /// <param name="log">Indicates whether to log results.</param>
        /// <returns>Returns error message if check fails to run correctly.</returns>
        public static string EMWSI7(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                EmParameters.WeeklySystemIntegrityErrorIsValid = false;

                if (EmParameters.InjectionReferenceValueValid.Default(false) && EmParameters.InjectionMeasuredValueValid.Default(false))
                {
                    if (EmParameters.CurrentWeeklySystemIntegrityTest.SystemIntegrityError == null)
                    {
                        EmParameters.WeeklyTestSummaryValid = false;
                        category.CheckCatalogResult = "A";
                    }
                    else if (EmParameters.WeeklySystemIntegrityApsIsValid.Default(false))
                    {
                        if (!EmParameters.CurrentWeeklySystemIntegrityTest.SystemIntegrityError.Value.IsRounded(1))
                        {
                            EmParameters.WeeklyTestSummaryValid = false;
                            category.CheckCatalogResult = "B";
                        }
                        else if (EmParameters.CurrentWeeklySystemIntegrityTest.SystemIntegrityError != EmParameters.CalculatedSystemIntegrityError)
                        {
                            EmParameters.WeeklyTestSummaryValid = false;
                            category.CheckCatalogResult = "C";
                        }
                        else
                        {
                            EmParameters.WeeklySystemIntegrityErrorIsValid = true;
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
        /// Check Weekly Test Summary Result Against Calculated Value
        /// </summary>
        /// <param name="category">Category Object</param>
        /// <param name="log">Indicates whether to log results.</param>
        /// <returns>Returns error message if check fails to run correctly.</returns>
        public static string EMWSI8(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                if ((EmParameters.CalculatedWeeklyTestSummaryResult != null) &&
                    (EmParameters.CurrentWeeklySystemIntegrityTest.TestResultCd != EmParameters.CalculatedWeeklyTestSummaryResult))
                {
                    EmParameters.CalculatedWeeklyTestSummaryResult = null;
                    EmParameters.WeeklyTestSummaryValid = false;
                    category.CheckCatalogResult = "A";
                }
            }
            catch (Exception ex)
            {
                returnVal = category.CheckEngine.FormatError(ex);
            }

            return returnVal;
        }


        /// <summary>
        /// Check Weekly Test Summary Result Against Calculated Value
        /// </summary>
        /// <param name="category">Category Object</param>
        /// <param name="log">Indicates whether to log results.</param>
        /// <returns>Returns error message if check fails to run correctly.</returns>
        public static string EMWSI9(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                if (EmParameters.CurrentWeeklySystemIntegrityTest.ComponentId != null)
                {
                    if (EmParameters.WsiTestDictionary.Count == 0 ||
                    (EmParameters.WsiTestDictionary.Count > 0 && !EmParameters.WsiTestDictionary.ContainsKey(EmParameters.CurrentWeeklySystemIntegrityTest.ComponentId)))
                    {
                        EmParameters.WsiTestDictionary[EmParameters.CurrentWeeklySystemIntegrityTest.ComponentId] = new WsiTestStatusInformation();
                    }

                    EmParameters.WsiTestDictionary[EmParameters.CurrentWeeklySystemIntegrityTest.ComponentId].MostRecentTestRecord = EmParameters.CurrentWeeklySystemIntegrityTest;
                }
            }

            catch (Exception ex)
            {
                returnVal = category.CheckEngine.FormatError(ex);
            }

            return returnVal;
        }


        /// <summary>
        /// Update Weekly System Integrity Dictionary Operating date Information.
        /// 
        /// 1) Clear the Operating Date List when the last evaluated test becomes the most recent test.
        /// 2) Only add the curent operating date to Operating Date List if the date is after the most recent test.
        ///    Operating after the test on the day of the test does not count as an operating day.
        /// </summary>
        /// <param name="category">Category Object</param>
        /// <param name="log">Indicates whether to log results.</param>
        /// <returns>Returns error message if check fails to run correctly.</returns>
        public static string EMWSI10(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                if (EmParameters.CurrentOperatingTime.Default(0) > 0)
                {
                    foreach (WsiTestStatusInformation wsiTestEntry in EmParameters.WsiTestDictionary.Values)
                    {
                        if ((wsiTestEntry.MostRecentTestRecord != null) && (wsiTestEntry.MostRecentTestRecord.MonLocId == EmParameters.CurrentMonitorLocationId))
                        {
                            //reset the opDates on the first hour
                            if (wsiTestEntry.MostRecentTestRecord.TestDatehour == EmParameters.CurrentDateHour)
                            {
                                wsiTestEntry.OperatingDateList.Clear();
                            }

                            //add the current OpDate if it's not already there
                            if ((wsiTestEntry.MostRecentTestRecord.TestDate < EmParameters.CurrentOperatingDate.Value) &&
                                (!wsiTestEntry.OperatingDateList.Contains(EmParameters.CurrentOperatingDate.Value)))
                            {
                                wsiTestEntry.OperatingDateList.Add(EmParameters.CurrentOperatingDate.Value);
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


        #region Check Methods 11 - 20

        /// <summary>
        /// Ensure that Weekly System Integrity Test Occurred During an Operating Hour
        /// </summary>
        /// <param name="category">Category Object</param>
        /// <param name="log">Indicates whether to log results.</param>
        /// <returns>Returns error message if check fails to run correctly.</returns>
        public static string EMWSI11(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                if (EmParameters.CurrentOperatingTime == 0)
                {
                    category.CheckCatalogResult = "A";
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
