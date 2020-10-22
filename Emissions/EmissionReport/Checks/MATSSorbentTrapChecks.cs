using System;
using System.Data;
using System.Linq;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.CheckEngine.SpecialParameterClasses;
using ECMPS.Checks.Data.Ecmps.CheckEm.Function;
using ECMPS.Checks.Data.Ecmps.Dbo.View;
using ECMPS.Checks.Em.Parameters;
using ECMPS.Checks.EmissionsReport;
using ECMPS.Checks.TypeUtilities;
using ECMPS.Definitions.Enumerations;
using ECMPS.Definitions.Extensions;

namespace ECMPS.Checks.EmissionsChecks
{
    public class cMatsSorbentTrapChecks : cEmissionsChecks
    {

        #region Constructors

        public cMatsSorbentTrapChecks(cEmissionsReportProcess emissionReportProcess)
            : base(emissionReportProcess)

        {
            CheckProcedures = new dCheckProcedure[16];

            CheckProcedures[1] = new dCheckProcedure(MatsTrp1);
            CheckProcedures[2] = new dCheckProcedure(MatsTrp2);
            CheckProcedures[3] = new dCheckProcedure(MatsTrp3);
            CheckProcedures[4] = new dCheckProcedure(MatsTrp4);
            CheckProcedures[5] = new dCheckProcedure(MatsTrp5);
            CheckProcedures[6] = new dCheckProcedure(MatsTrp6);
            CheckProcedures[7] = new dCheckProcedure(MatsTrp7);
            CheckProcedures[8] = new dCheckProcedure(MatsTrp8);
            CheckProcedures[9] = new dCheckProcedure(MatsTrp9);
            CheckProcedures[10] = new dCheckProcedure(MatsTrp10);
            CheckProcedures[11] = new dCheckProcedure(MatsTrp11);
            CheckProcedures[12] = new dCheckProcedure(MatsTrp12);
            CheckProcedures[13] = new dCheckProcedure(MatsTrp13);
            CheckProcedures[14] = new dCheckProcedure(MatsTrp14);
            CheckProcedures[15] = new dCheckProcedure(MatsTrp15);
        }

        #endregion


        #region Checks 1-10

        /// <summary>
        /// Begin Date Valid
        /// 
        /// This check determines if the sorbent trap data begin date is valid.
        /// </summary>
        /// <param name="category">Category Object</param>
        /// <param name="log">Indicates whether to log results.</param>
        /// <returns>Returns error message if check fails to run correctly.</returns>
        public static string MatsTrp1(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                EmParameters.MatsSorbentTrapBeginDateValid = false;

                if (EmParameters.MatsSorbentTrapRecord.BeginDate == null)
                {
                    EmParameters.MatsSorbentTrapEvaluationNeeded = false;
                    category.CheckCatalogResult = "A";
                }
                else
                {
                    EmParameters.MatsSorbentTrapBeginDateValid = true;
                }
            }
            catch (Exception ex)
            {
                returnVal = category.CheckEngine.FormatError(ex);
            }

            return returnVal;
        }

        /// <summary>
        /// Begin Hour Valid
        /// 
        /// This check determines if the sorbent trap data begin  hour is valid.
        /// </summary>
        /// <param name="category">Category Object</param>
        /// <param name="log">Indicates whether to log results.</param>
        /// <returns>Returns error message if check fails to run correctly.</returns>
        public static string MatsTrp2(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                EmParameters.MatsSorbentTrapBeginDateHourValid = false;

                if (EmParameters.MatsSorbentTrapBeginDateValid.Default(false))
                {
                    if (EmParameters.MatsSorbentTrapRecord.BeginHour == null)
                    {
                        EmParameters.MatsSorbentTrapEvaluationNeeded = false;
                        category.CheckCatalogResult = "A";
                    }
                    else if ((EmParameters.MatsSorbentTrapRecord.BeginHour < 0) || (23 < EmParameters.MatsSorbentTrapRecord.BeginHour))
                    {
                        EmParameters.MatsSorbentTrapEvaluationNeeded = false;
                        category.CheckCatalogResult = "B";
                    }
                    else
                    {
                        EmParameters.MatsSorbentTrapBeginDateHourValid = true;
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
        /// End Date Valid
        /// 
        /// This check determines if the sorbent trap data end date and  hour is valid.
        /// </summary>
        /// <param name="category">Category Object</param>
        /// <param name="log">Indicates whether to log results.</param>
        /// <returns>Returns error message if check fails to run correctly.</returns>
        public static string MatsTrp3(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                EmParameters.MatsSorbentTrapEndDateValid = false;

                if (EmParameters.MatsSorbentTrapRecord.EndDate == null)
                {
                    EmParameters.MatsSorbentTrapEvaluationNeeded = false;
                    category.CheckCatalogResult = "A";
                }
                else
                {
                    EmParameters.MatsSorbentTrapEndDateValid = true;
                }
            }
            catch (Exception ex)
            {
                returnVal = category.CheckEngine.FormatError(ex);
            }

            return returnVal;
        }

        /// <summary>
        /// End Hour Valid
        /// 
        /// This check determines if the sorbent trap data end hour is valid.
        /// </summary>
        /// <param name="category">Category Object</param>
        /// <param name="log">Indicates whether to log results.</param>
        /// <returns>Returns error message if check fails to run correctly.</returns>
        public static string MatsTrp4(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                EmParameters.MatsSorbentTrapEndDateHourValid = false;

                if (EmParameters.MatsSorbentTrapEndDateValid.Default(false))
                {
                    if (EmParameters.MatsSorbentTrapRecord.EndHour == null)
                    {
                        EmParameters.MatsSorbentTrapEvaluationNeeded = false;
                        category.CheckCatalogResult = "A";
                    }
                    else if ((EmParameters.MatsSorbentTrapRecord.EndHour < 0) || (23 < EmParameters.MatsSorbentTrapRecord.EndHour))
                    {
                        EmParameters.MatsSorbentTrapEvaluationNeeded = false;
                        category.CheckCatalogResult = "B";
                    }
                    else
                    {
                        EmParameters.MatsSorbentTrapEndDateHourValid = true;
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
        /// Begin and End Times Consistent
        /// 
        /// Check that the Sorbent Trap end date and time occurs after the begin date and time. 
        /// </summary>
        /// <param name="category">Category Object</param>
        /// <param name="log">Indicates whether to log results.</param>
        /// <returns>Returns error message if check fails to run correctly.</returns>
        public static string MatsTrp5(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                EmParameters.MatsSorbentTrapDatesAndHoursConsistent = false;

                if (EmParameters.MatsSorbentTrapBeginDateHourValid.Default(false) && EmParameters.MatsSorbentTrapEndDateHourValid.Default(false))
                {
                    if (EmParameters.MatsSorbentTrapRecord.BeginDatehour.Value > EmParameters.MatsSorbentTrapRecord.EndDatehour.Value)
                    {
                        EmParameters.MatsSorbentTrapEvaluationNeeded = false;
                        category.CheckCatalogResult = "A";
                    }
                    else
                        EmParameters.MatsSorbentTrapDatesAndHoursConsistent = true;
                }
            }
            catch (Exception ex)
            {
                returnVal = category.CheckEngine.FormatError(ex);
            }

            return returnVal;
        }

        /// <summary>
        /// Check For Overlap With Another Sorbent Trap
        /// 
        /// Check for overlap with the last Sorbent Trap from the previous emission report or 
        /// with another Sorbent Trap reported in the current emission report. 
        /// </summary>
        /// <param name="category">Category Object</param>
        /// <param name="log">Indicates whether to log results.</param>
        /// <returns>Returns error message if check fails to run correctly.</returns>
        public static string MatsTrp6(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                CheckDataView<MatsSorbentTrapRecord> sorbentTrapRecords
                  = EmParameters.MatsSorbentTrapRecords.FindRows(new cFilterCondition[]
                                                                     {
                                                               new cFilterCondition("MON_SYS_ID", EmParameters.MatsSorbentTrapRecord.MonSysId),
                                                               new cFilterCondition("TRAP_ID", EmParameters.MatsSorbentTrapRecord.TrapId, true),
                                                               new cFilterCondition("BEGIN_DATEHOUR", EmParameters.MatsSorbentTrapRecord.EndDatehour, eFilterDataType.DateBegan, eFilterConditionRelativeCompare.LessThan),
                                                               new cFilterCondition("END_DATEHOUR", EmParameters.MatsSorbentTrapRecord.BeginDatehour, eFilterDataType.DateEnded, eFilterConditionRelativeCompare.GreaterThan)
                                                                     });

                if (sorbentTrapRecords.Count > 0)
                {
                    EmParameters.MatsSorbentTrapEvaluationNeeded = false;
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
        /// Initialize MATS Sorbent Trap Parameters
        /// 
        /// Initialize MATS Sampling Train Data.
        /// </summary>
        /// <param name="category">Category Object</param>
        /// <param name="log">Indicates whether to log results.</param>
        /// <returns>Returns error message if check fails to run correctly.</returns>
        public static string MatsTrp7(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                EmParameters.MatsSorbentTrapValidExists = false;
                EmParameters.MatsSorbentTrapSamplingTrainList = null;
                EmParameters.MatsSamplingTrainProblemComponentExists = false;

                SorbentTrapEvalInformation sorbentTrapEvalInformation = new SorbentTrapEvalInformation();
                {
                    sorbentTrapEvalInformation.SorbentTrapValidExists = true;
                    sorbentTrapEvalInformation.IsBorderTrap = (EmParameters.MatsSorbentTrapRecord.BorderTrapInd == 1);
                    sorbentTrapEvalInformation.IsSupplementalData = (EmParameters.MatsSorbentTrapRecord.SuppDataInd == 1);
                    sorbentTrapEvalInformation.SorbentTrapId = EmParameters.MatsSorbentTrapRecord.TrapId;
                    sorbentTrapEvalInformation.SorbentTrapBeginDateHour = EmParameters.MatsSorbentTrapRecord.BeginDatehour;
                    sorbentTrapEvalInformation.SorbentTrapEndDateHour = EmParameters.MatsSorbentTrapRecord.EndDatehour;
                    sorbentTrapEvalInformation.SorbentTrapModcCd = EmParameters.MatsSorbentTrapRecord.ModcCd;
                    sorbentTrapEvalInformation.SamplingTrainProblemComponentExists = false;
                    //Setting of SamplingTrainList and OperatingDateList happen through object references
                }

                string trapId = EmParameters.MatsSorbentTrapRecord.TrapId;

                EmParameters.MatsSorbentTrapDictionary[trapId] = sorbentTrapEvalInformation;
                EmParameters.MatsSorbentTrapListByLocationArray[EmParameters.CurrentMonitorPlanLocationPostion.Value].Add(sorbentTrapEvalInformation);

                EmParameters.MatsSorbentTrapValidExists = EmParameters.MatsSorbentTrapDictionary[trapId].SorbentTrapValidExists;
                EmParameters.MatsSorbentTrapSamplingTrainList = EmParameters.MatsSorbentTrapDictionary[trapId].SamplingTrainList;
            }
            catch (Exception ex)
            {
                returnVal = category.CheckEngine.FormatError(ex);
            }

            return returnVal;
        }

        /// <summary>
        /// Monitoring System Check
        /// 
        /// This check determines if the sorbent trap data begin date is valid.
        /// </summary>
        /// <param name="category">Category Object</param>
        /// <param name="log">Indicates whether to log results.</param>
        /// <returns>Returns error message if check fails to run correctly.</returns>
        public static string MatsTrp8(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                if (EmParameters.MatsSorbentTrapRecord.MonSysId == null)
                {
                    EmParameters.MatsSorbentTrapValidExists = false;
                    category.CheckCatalogResult = "A";
                }
                else if (EmParameters.MatsSorbentTrapRecord.SysTypeCd != "ST")
                {
                    EmParameters.MatsSorbentTrapValidExists = false;
                    category.CheckCatalogResult = "B";
                }
                else if ((EmParameters.MatsSorbentTrapRecord.SystemBeginDatehour.Default(DateTime.MinValue) > EmParameters.MatsSorbentTrapRecord.BeginDatehour.Default(DateTime.MinValue)) ||
                         (EmParameters.MatsSorbentTrapRecord.SystemEndDatehour.Default(DateTime.MaxValue) < EmParameters.MatsSorbentTrapRecord.EndDatehour.Default(DateTime.MaxValue)))
                {
                    EmParameters.MatsSorbentTrapValidExists = false;
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
        /// Number and Validity of Sampling Trains
        /// 
        /// Check that two Sorbent Train Data Records are provided for each Sorbent Trap Data Record. 
        /// </summary>
        /// <param name="category">Category Object</param>
        /// <param name="log">Indicates whether to log results.</param>
        /// <returns>Returns error message if check fails to run correctly.</returns>
        public static string MatsTrp9(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                EmParameters.MatsSamplingTrainsValid = false;

                if (EmParameters.MatsSamplingTrainProblemComponentExists == false)
                {
                    if (EmParameters.MatsSorbentTrapSamplingTrainList.Count != 2)
                    {
                        EmParameters.MatsSorbentTrapValidExists = false;
                        category.CheckCatalogResult = "A";
                    }
                    // Check that SamplingTrainValid for both sampling trains is equal to true.
                    else if (EmParameters.MatsSorbentTrapSamplingTrainList.Count(item => item.SamplingTrainValid.Default(false) == false) > 0)
                    {
                        EmParameters.MatsSorbentTrapValidExists = false;
                    }
                    else
                    {
                        EmParameters.MatsSamplingTrainsValid = true;
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
        /// Sorbent Trap MODC Code is Valid
        /// 
        /// Check Sorbent Trap MODC Code Valid. 
        /// </summary>
        /// <param name="category">Category Object</param>
        /// <param name="log">Indicates whether to log results.</param>
        /// <returns>Returns error message if check fails to run correctly.</returns>
        public static string MatsTrp10(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                EmParameters.MatsSorbentTrapModcCodeValid = false;

                if (EmParameters.MatsSorbentTrapRecord.ModcCd.NotInList("01,02,32,33,34,35,43,44"))
                {
                    EmParameters.MatsSorbentTrapValidExists = false;
                    category.CheckCatalogResult = "A";
                }

                else if (EmParameters.MatsSamplingTrainsValid.Default(false)) // Insures that two valid sampling trains exist.
                {
                    if (EmParameters.MatsSorbentTrapRecord.ModcCd.InList("01,02,43"))
                    {
                        if (EmParameters.MatsSorbentTrapSamplingTrainList.Count(item => item.TrainQAStatusCode == "PASSED") == 2)
                        {
                            EmParameters.MatsSorbentTrapModcCodeValid = true;
                        }
                        else
                        {
                            EmParameters.MatsSorbentTrapValidExists = false;
                            category.CheckCatalogResult = "B";
                        }
                    }

                    else if (EmParameters.MatsSorbentTrapRecord.ModcCd.InList("32,44"))
                    {
                        if ((EmParameters.MatsSorbentTrapSamplingTrainList.Count(item => item.TrainQAStatusCode == "PASSED") == 1) &&
                            (EmParameters.MatsSorbentTrapSamplingTrainList.Count(item => item.TrainQAStatusCode.InList("FAILED,LOST")) == 1))
                        {
                            EmParameters.MatsSorbentTrapModcCodeValid = true;
                        }
                        else
                        {
                            EmParameters.MatsSorbentTrapValidExists = false;
                            category.CheckCatalogResult = "C";
                        }
                    }

                    else if (EmParameters.MatsSorbentTrapRecord.ModcCd == "33")
                    {
                        if (EmParameters.MatsSorbentTrapSamplingTrainList.Count(item => item.TrainQAStatusCode == "UNCERTAIN") == 2)
                        {
                            EmParameters.MatsSorbentTrapModcCodeValid = true;
                        }
                        else
                        {
                            EmParameters.MatsSorbentTrapValidExists = false;
                            category.CheckCatalogResult = "D";
                        }
                    }

                    else if (EmParameters.MatsSorbentTrapRecord.ModcCd == "34")
                    {
                        if (EmParameters.MatsSorbentTrapSamplingTrainList.Count(item => item.TrainQAStatusCode == "FAILED") == 2)
                        {
                            EmParameters.MatsSorbentTrapModcCodeValid = true;
                        }
                        else if (EmParameters.MatsSorbentTrapSamplingTrainList.Count(item => item.TrainQAStatusCode == "UNCERTAIN") == 2)
                        {
                            EmParameters.MatsSorbentTrapModcCodeValid = true;
                        }
                        else if ((EmParameters.MatsSorbentTrapSamplingTrainList.Count(item => item.TrainQAStatusCode == "LOST") >= 1) ||
                                 (EmParameters.MatsSorbentTrapSamplingTrainList.Count(item => item.TrainQAStatusCode == "EXPIRED") >= 1) ||
                                 (EmParameters.MatsSorbentTrapSamplingTrainList.Count(item => item.TrainQAStatusCode == "INC") >= 1))
                        {
                            EmParameters.MatsSorbentTrapModcCodeValid = true;
                        }
                        else
                        {
                            EmParameters.MatsSorbentTrapValidExists = false;
                            category.CheckCatalogResult = "E";
                        }
                    }

                    else if (EmParameters.MatsSorbentTrapRecord.ModcCd == "35")
                    {
                        EmParameters.MatsSorbentTrapModcCodeValid = true;
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


        #region Checks 11-20

        /// <summary>
        /// Paired Trap Agreement Validation and Re-calculation
        /// 
        /// Determine if the Paired Trap Agreement is Valid. 
        /// </summary>
        /// <param name="category">Category Object</param>
        /// <param name="log">Indicates whether to log results.</param>
        /// <returns>Returns error message if check fails to run correctly.</returns>
        public static string MatsTrp11(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                EmParameters.MatsSorbentTrapPairedTrapAgreementValid = false;
                EmParameters.MatsCalcTrapAbsoluteDifference = null;
                EmParameters.MatsCalcTrapPercentDifference = null;

                if (EmParameters.MatsSorbentTrapModcCodeValid.Default(false))
                {
                    /* Paired Trap Agreement is null */
                    if (EmParameters.MatsSorbentTrapRecord.PairedTrapAgreement == null)
                    {
                        /* Check for bad MODC */
                        if (EmParameters.MatsSorbentTrapRecord.ModcCd.NotInList("32,34,35,44"))
                        {
                            EmParameters.MatsSorbentTrapValidExists = false;
                            category.CheckCatalogResult = "A";
                        }
                        /* Check for Absolute Difference Indicator not equal to null */
                        else if (EmParameters.MatsSorbentTrapRecord.AbsoluteDifferenceInd != null)
                        {
                            EmParameters.MatsSorbentTrapValidExists = false;
                            category.CheckCatalogResult = "B";
                        }
                        else
                        {
                            EmParameters.MatsSorbentTrapPairedTrapAgreementValid = true;
                        }
                    }

                    /* Paired Trap Agreement is not null */
                    else
                    {
                        /* Check for bad MODC */
                        if (EmParameters.MatsSorbentTrapRecord.ModcCd.NotInList("01,02,33,43"))
                        {
                            EmParameters.MatsSorbentTrapValidExists = false;
                            category.CheckCatalogResult = "C";
                        }
                        /* Check for Paired Trap Agreement not rounded to 2 */
                        else if (EmParameters.MatsSorbentTrapRecord.PairedTrapAgreement != Math.Round(EmParameters.MatsSorbentTrapRecord.PairedTrapAgreement.Value, 2, MidpointRounding.AwayFromZero))
                        {
                            EmParameters.MatsSorbentTrapValidExists = false;
                            category.CheckCatalogResult = "D";
                        }
                        /* Handle when absolute difference indicator is 0 or 1 (not null) */
                        else if ((EmParameters.MatsSorbentTrapRecord.AbsoluteDifferenceInd == 0) || (EmParameters.MatsSorbentTrapRecord.AbsoluteDifferenceInd == 1))
                        {
                            decimal hgConcentration1 = EmParameters.MatsSorbentTrapSamplingTrainList[0].HgConcentration.Value;
                            decimal hgConcentration2 = EmParameters.MatsSorbentTrapSamplingTrainList[1].HgConcentration.Value;

                            EmParameters.MatsCalcTrapAbsoluteDifference = Math.Abs(hgConcentration1 - hgConcentration2);
                            EmParameters.MatsCalcTrapPercentDifference = ((hgConcentration1 + hgConcentration2) != 0) ?  100 * EmParameters.MatsCalcTrapAbsoluteDifference / (hgConcentration1 + hgConcentration2) : 0m;
                            EmParameters.MatsCalcTrapAbsoluteDifference = Math.Round(EmParameters.MatsCalcTrapAbsoluteDifference.Value, 2, MidpointRounding.AwayFromZero);
                            EmParameters.MatsCalcTrapPercentDifference = Math.Round(EmParameters.MatsCalcTrapPercentDifference.Value, 2, MidpointRounding.AwayFromZero);

                            /* Check when Absolute Difference Indicator is 0 */
                            if (EmParameters.MatsSorbentTrapRecord.AbsoluteDifferenceInd == 0)
                            {
                                /* Check for Percent Difference Discrepancy */
                                if (EmParameters.MatsSorbentTrapRecord.PairedTrapAgreement != EmParameters.MatsCalcTrapPercentDifference)
                                {
                                    EmParameters.MatsSorbentTrapValidExists = false;
                                    category.CheckCatalogResult = "G";
                                }

                                /* Handle when Paired Trap Agreement is less than or equal to 10 */
                                else if (EmParameters.MatsSorbentTrapRecord.PairedTrapAgreement <= 10)
                                {
                                    if (EmParameters.MatsSorbentTrapRecord.ModcCd.NotInList("01,02,43"))
                                    {
                                        EmParameters.MatsSorbentTrapValidExists = false;
                                        category.CheckCatalogResult = "H";
                                    }
                                    else
                                    {
                                        EmParameters.MatsSorbentTrapPairedTrapAgreementValid = true;
                                    }
                                }

                                /* Handle when Paired Trap Agreement is less than or equal to 20, and concentration is less than or equal to 1.0 */
                                else if ((EmParameters.MatsSorbentTrapRecord.PairedTrapAgreement <= 20) &&
                                         (EmParameters.MatsSorbentTrapRecord.HgConcentration.ScientificNotationtoDecimal() <= 1.0m))
                                {
                                    if (EmParameters.MatsSorbentTrapRecord.ModcCd.NotInList("01,02,43"))
                                    {
                                        EmParameters.MatsSorbentTrapValidExists = false;
                                        category.CheckCatalogResult = "I";
                                    }
                                    else
                                    {
                                        EmParameters.MatsSorbentTrapPairedTrapAgreementValid = true;
                                    }
                                }

                                /* Handle Paired Trap Aggreement Failed */
                                else
                                {
                                    if (EmParameters.MatsSorbentTrapRecord.ModcCd != "33")
                                    {
                                        EmParameters.MatsSorbentTrapValidExists = false;
                                        category.CheckCatalogResult = "J";
                                    }
                                    else
                                    {
                                        EmParameters.MatsSorbentTrapPairedTrapAgreementValid = true;
                                    }
                                }
                            }

                            /* Check when Absolute Difference Indicator is 1 */
                            else
                            {
                                /* Ensure that Paired Trap Agreement is less than or equal to 0.03 */
                                if (EmParameters.MatsSorbentTrapRecord.PairedTrapAgreement.Value <= 0.03m)
                                {
                                    /* Check for Absolute Difference Discrepancy */
                                    if (EmParameters.MatsSorbentTrapRecord.PairedTrapAgreement != EmParameters.MatsCalcTrapAbsoluteDifference)
                                    {
                                        EmParameters.MatsSorbentTrapValidExists = false;
                                        category.CheckCatalogResult = "E";
                                    }
                                    else
                                    {
                                        EmParameters.MatsSorbentTrapPairedTrapAgreementValid = true;
                                    }
                                }
                                else
                                {
                                    EmParameters.MatsSorbentTrapValidExists = false;
                                    category.CheckCatalogResult = "F";
                                }
                            }
                        }

                        /* Absolute Difference is null (not 0 or 1) */
                        else
                        {
                            EmParameters.MatsSorbentTrapValidExists = false;
                            category.CheckCatalogResult = "K";
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
        /// Hg System Concentration Validation and Re-calculation
        /// 
        /// Determin whether the Hg System Concentration is valid. 
        /// </summary>
        /// <param name="category">Category Object</param>
        /// <param name="log">Indicates whether to log results.</param>
        /// <returns>Returns error message if check fails to run correctly.</returns>
        public static string MatsTrp12(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                EmParameters.MatsCalcHgSystemConcentration = null;

                if (EmParameters.MatsSorbentTrapPairedTrapAgreementValid.Default(false))
                {
                    if (EmParameters.MatsSorbentTrapRecord.HgConcentration == null)
                    {
                        if (EmParameters.MatsSorbentTrapRecord.ModcCd.NotInList("34,35"))
                        {
                            EmParameters.MatsSorbentTrapValidExists = false;
                            category.CheckCatalogResult = "A";
                        }
                    }
                    else
                    {
                        if (EmParameters.MatsSorbentTrapRecord.ModcCd.NotInList("01,02,32,33,43,44"))
                        {
                            EmParameters.MatsSorbentTrapValidExists = false;
                            category.CheckCatalogResult = "B";
                        }
                        else if (!EmParameters.MatsSorbentTrapRecord.HgConcentration.SignificantDigitNotationValid(3))
                        {
                            EmParameters.MatsSorbentTrapValidExists = false;
                            category.CheckCatalogResult = "C";
                        }
                        else if (ECMPS.Definitions.Extensions.cExtensions.ScientificNotationtoDecimal(EmParameters.MatsSorbentTrapRecord.HgConcentration) == 0)
                        {
                            EmParameters.MatsSorbentTrapValidExists = false;
                            category.CheckCatalogResult = "E";
                        }
                        else if (EmParameters.MatsSorbentTrapSamplingTrainList.Any(item => item.HgConcentration == 0 ))
                        {
                            EmParameters.MatsSorbentTrapValidExists = false;
                            category.CheckCatalogResult = "F";
                        }
                        else
                        {
                            decimal hgConcentrationCalculation;

                            if (EmParameters.MatsSorbentTrapRecord.ModcCd.InList("32,44"))
                            {
                                hgConcentrationCalculation = 1.111m * EmParameters.MatsSorbentTrapSamplingTrainList.First(train => train.TrainQAStatusCode == "PASSED").HgConcentration.Value;
                            }
                            else if (EmParameters.MatsSorbentTrapRecord.ModcCd == "33")
                            {
                                hgConcentrationCalculation = EmParameters.MatsSorbentTrapSamplingTrainList.Select(item => item.HgConcentration.Value).ToList().Max();
                            }
                            else // MODC 01, 02 or 43
                            {
                                hgConcentrationCalculation = EmParameters.MatsSorbentTrapSamplingTrainList.Sum(item => item.HgConcentration.Value) / 2;
                            }

                            EmParameters.MatsCalcHgSystemConcentration = hgConcentrationCalculation.DecimaltoScientificNotation();

                            if (EmParameters.MatsSorbentTrapRecord.HgConcentration != EmParameters.MatsCalcHgSystemConcentration)
                            {
                                EmParameters.MatsSorbentTrapValidExists = false;
                                category.CheckCatalogResult = "D";
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

        /// <summary>
        /// Update Sorbent Trap Parameters
        /// 
        /// Update Sorbent Trap Dictionary for the current location with values from individual check parameters.
        /// The Sampling Train Dictionary and Operating Date List are updated through object referencing and therefore are update here.
        /// </summary>
        /// <param name="category">Category Object</param>
        /// <param name="log">Indicates whether to log results.</param>
        /// <returns>Returns error message if check fails to run correctly.</returns>
        public static string MatsTrp13(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                EmParameters.MatsSorbentTrapDictionary[EmParameters.MatsSorbentTrapRecord.TrapId].SorbentTrapValidExists = EmParameters.MatsSorbentTrapValidExists;
                EmParameters.MatsSorbentTrapDictionary[EmParameters.MatsSorbentTrapRecord.TrapId].SamplingTrainProblemComponentExists = EmParameters.MatsSamplingTrainProblemComponentExists;
            }
            catch (Exception ex)
            {
                returnVal = category.CheckEngine.FormatError(ex);
            }

            return returnVal;
        }

        /// <summary>
        /// Number of Unit Operating Days
        /// 
        /// Check the Number of Unit Operating Days During Sampling Period.
        /// </summary>
        /// <param name="category">Category Object</param>
        /// <param name="log">Indicates whether to log results.</param>
        /// <returns>Returns error message if check fails to run correctly.</returns>
        public static string MatsTrp14(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {

                if (EmParameters.MatsSorbentTrapRecord.ModcCd != "34")
                {
                    if (EmParameters.MatsSorbentTrapDictionary[EmParameters.MatsSorbentTrapRecord.TrapId].OperatingDateList.Count > 15)
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
        /// Ensure that Active ST or CEMST Methods Span the Sorbent Trap Period.
        /// </summary>
        /// <param name="category"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public static string MatsTrp15(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                DateTime trapBeginDateHour = EmParameters.MatsSorbentTrapRecord.BeginDatehour.Default(DateTime.MinValue);
                DateTime trapEndDateHour = EmParameters.MatsSorbentTrapRecord.EndDatehour.Default(DateTime.MaxValue);

                CheckDataView<VwMonitorMethodRow> methodView 
                    = EmParameters.MethodRecords.FindRows(new cFilterCondition("METHOD_CD", "ST,CEMST", eFilterConditionStringCompare.InList),
                                                          new cFilterCondition("BEGIN_DATEHOUR", eFilterConditionRelativeCompare.LessThanOrEqual, trapEndDateHour, eNullDateDefault.Min),
                                                          new cFilterCondition("END_DATEHOUR", eFilterConditionRelativeCompare.GreaterThanOrEqual, trapBeginDateHour, eNullDateDefault.Max));

                if (methodView.Count == 0)
                {
                    category.CheckCatalogResult = "A";
                }
                else if (!(CheckForHourRangeCovered(category, methodView.SourceView, trapBeginDateHour.Date, trapBeginDateHour.Hour, trapEndDateHour.Date, trapEndDateHour.Hour)))
                {
                    category.CheckCatalogResult = "B";
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