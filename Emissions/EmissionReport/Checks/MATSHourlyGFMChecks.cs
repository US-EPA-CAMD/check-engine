using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

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
    public class cMATSHourlyGFMChecks : cEmissionsChecks
    {

        #region Constructors

        public cMATSHourlyGFMChecks(cEmissionsReportProcess emissionReportProcess)
            : base(emissionReportProcess)

        {
            CheckProcedures = new dCheckProcedure[8];

            CheckProcedures[1] = new dCheckProcedure(MatsGfm1);
            CheckProcedures[2] = new dCheckProcedure(MatsGfm2);
            CheckProcedures[3] = new dCheckProcedure(MatsGfm3);
            CheckProcedures[4] = new dCheckProcedure(MatsGfm4);
            CheckProcedures[5] = new dCheckProcedure(MatsGfm5);
            CheckProcedures[6] = new dCheckProcedure(MatsGfm6);
            CheckProcedures[7] = new dCheckProcedure(MatsGfm7);
        }

        #endregion


        #region Checks 1-10

        /// <summary>
        /// Component Id Valid
        /// 
        /// Ensure that the component id is associated with a sampling train.
        /// </summary>
        /// <param name="category"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public static string MatsGfm1(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                EmParameters.MatsGfmSamplingTrainRecords = null;
                EmParameters.MatsHourlyGfmComponentIdValid = false;
                EmParameters.MatsSamplingTrainRecord = null;
                EmParameters.MatsSamplingTrainQaStatus = null;
                EmParameters.MatsSorbentTrapBeginDatehour = null;
                EmParameters.MatsSorbentTrapEndDatehour = null;
                EmParameters.MatsSamplingTrainCount = null;

                if (EmParameters.MatsHourlyGfmRecord.ComponentId == null)
                {
                    category.CheckCatalogResult = "A";
                }
                else
                {
                    CheckDataView<MatsSamplingTrainRecord> locatedMatsSamplingTrainRecords
                      = EmParameters.MatsSamplingTrainRecords.FindRows
                        (
                          new cFilterCondition("COMPONENT_ID", EmParameters.MatsHourlyGfmRecord.ComponentId),
                          new cFilterCondition("BEGIN_DATEHOUR", EmParameters.CurrentDateHour, eFilterDataType.DateBegan, eFilterConditionRelativeCompare.LessThanOrEqual),
                          new cFilterCondition("END_DATEHOUR", EmParameters.CurrentDateHour, eFilterDataType.DateEnded, eFilterConditionRelativeCompare.GreaterThanOrEqual)
                        // Sort by Begin Hour and End Hour handled in load of underlying source table
                        );

                    EmParameters.MatsSamplingTrainCount = locatedMatsSamplingTrainRecords.Count;
                    EmParameters.MatsGfmSamplingTrainRecords = locatedMatsSamplingTrainRecords;

                    if (locatedMatsSamplingTrainRecords.Count == 0)
                    {
                        category.CheckCatalogResult = "B";
                    }
                    else
                    {
                        EmParameters.MatsHourlyGfmComponentIdValid = true;
                        EmParameters.MatsSamplingTrainRecord = locatedMatsSamplingTrainRecords[0];
                        EmParameters.MatsSamplingTrainQaStatus = EmParameters.MatsSamplingTrainRecord.TrainQaStatusCd;
                        EmParameters.MatsSorbentTrapBeginDatehour = EmParameters.MatsSamplingTrainRecord.BeginDatehour;
                        EmParameters.MatsSorbentTrapEndDatehour = EmParameters.MatsSamplingTrainRecord.EndDatehour;
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
        /// Begin and End Hour Flags Valid
        /// 
        /// Check that Begin and End Hour Flags are valid..
        /// </summary>
        /// <param name="category"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public static string MatsGfm2(cCategory category, ref bool log)
        {
            string returnVal = "";
            List<string> MonSysIdList = new List<string>();

            try
            {
                if (EmParameters.MatsHourlyGfmComponentIdValid.Default(false))
                {
                    if (EmParameters.MatsHourlyGfmRecord.BeginEndHourFlg == "I")
                    {
                        if ((EmParameters.CurrentDateHour != EmParameters.MatsSorbentTrapBeginDatehour) &&
                            (EmParameters.CurrentDateHour != EmParameters.MatsSorbentTrapBeginDatehour.Value.AddHours(1)))
                        {
                            category.CheckCatalogResult = "A";
                        }
                    }
                    else if (EmParameters.MatsHourlyGfmRecord.BeginEndHourFlg == "F")
                    {
                        if ((EmParameters.CurrentDateHour != EmParameters.MatsSorbentTrapEndDatehour) &&
                            (EmParameters.CurrentDateHour != EmParameters.MatsSorbentTrapEndDatehour.Value.AddHours(-1)))
                        {
                            category.CheckCatalogResult = "B";
                        }
                    }
                    else if (EmParameters.MatsHourlyGfmRecord.BeginEndHourFlg == null)
                    {
                        if (EmParameters.CurrentDateHour == EmParameters.MatsSorbentTrapBeginDatehour)
                        {
                            category.CheckCatalogResult = "C";
                        }
                        else if (EmParameters.CurrentDateHour == EmParameters.MatsSorbentTrapEndDatehour)
                        {
                            category.CheckCatalogResult = "D";
                        }
                    }
                    else if (EmParameters.MatsHourlyGfmRecord.BeginEndHourFlg == "T")
                    {
                        if (EmParameters.MatsSamplingTrainCount <= 1)
                        {
                            // Current hour does not have two active traps/trains.
                            category.CheckCatalogResult = "E";
                        }
                        else if (EmParameters.CurrentDateHour != EmParameters.MatsSorbentTrapEndDatehour)
                        {
                            // Current hour is not the end hour of the current sorbent trap.
                            category.CheckCatalogResult = "F";
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
        /// Hourly GFM Reading Valid
        /// 
        /// Hourly GFM Reading Null or Reported to Two Decimal Places
        /// </summary>
        /// <param name="category"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public static string MatsGfm3(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                if (EmParameters.MatsHourlyGfmComponentIdValid.Default(false))
                {
                    if (EmParameters.MatsHourlyGfmRecord.GfmReading == null)
                    {
                        if (EmParameters.MatsSamplingTrainQaStatus.NotInList("INC,EXPIRED,LOST") && (EmParameters.MatsHourlyGfmRecord.BeginEndHourFlg != "N"))
                        {
                            category.CheckCatalogResult = "A";
                        }
                    }
                    else
                    {
                        if (EmParameters.MatsHourlyGfmRecord.BeginEndHourFlg == "N")
                        {
                            category.CheckCatalogResult = "D";
                        }
                        else if (EmParameters.MatsSamplingTrainQaStatus.NotInList("PASSED,FAILED,UNCERTAIN"))
                        {
                            category.CheckCatalogResult = "B";
                        }
                        else if (EmParameters.MatsHourlyGfmRecord.GfmReading != Math.Round(EmParameters.MatsHourlyGfmRecord.GfmReading.Value, 2, MidpointRounding.AwayFromZero))
                        {
                            category.CheckCatalogResult = "C";
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
        /// Average Hourly Sampling Rate Valid
        /// 
        /// Average Hourly Sampling Rate Null or Reported to Two Decimal Places
        /// </summary>
        /// <param name="category"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public static string MatsGfm4(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                if (EmParameters.MatsHourlyGfmComponentIdValid.Default(false))
                {
                    if (EmParameters.MatsHourlyGfmRecord.AvgSamplingRate == null)
                    {
                        if (EmParameters.MatsSamplingTrainQaStatus.NotInList("INC,EXPIRED,LOST") && (EmParameters.MatsHourlyGfmRecord.BeginEndHourFlg != "N"))
                        {
                            category.CheckCatalogResult = "A";
                        }
                    }
                    else
                    {
                        if (EmParameters.MatsHourlyGfmRecord.BeginEndHourFlg == "N")
                        {
                            category.CheckCatalogResult = "D";
                        }
                        else if (EmParameters.MatsSamplingTrainQaStatus.NotInList("PASSED,FAILED,UNCERTAIN"))
                        {
                            category.CheckCatalogResult = "B";
                        }
                        else if (EmParameters.MatsHourlyGfmRecord.AvgSamplingRate != Math.Round(EmParameters.MatsHourlyGfmRecord.AvgSamplingRate.Value, 2, MidpointRounding.AwayFromZero))
                        {
                            category.CheckCatalogResult = "C";
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
        /// Sampling Rate UOM Valid
        /// 
        /// Sampling Rate UOM Null or Matches UOM Code
        /// </summary>
        /// <param name="category"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public static string MatsGfm5(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                if (EmParameters.MatsHourlyGfmComponentIdValid.Default(false))
                {
                    if (EmParameters.MatsHourlyGfmRecord.SamplingRateUom == null)
                    {
                        if (EmParameters.MatsSamplingTrainQaStatus.NotInList("INC,EXPIRED,LOST") && (EmParameters.MatsHourlyGfmRecord.BeginEndHourFlg != "N"))
                        {
                            category.CheckCatalogResult = "A";
                        }
                    }
                    else
                    {
                        if (EmParameters.MatsHourlyGfmRecord.BeginEndHourFlg == "N")
                        {
                            category.CheckCatalogResult = "D";
                        }
                        else if (EmParameters.MatsSamplingTrainQaStatus.NotInList("PASSED,FAILED,UNCERTAIN"))
                        {
                            category.CheckCatalogResult = "B";
                        }
                        else if (EmParameters.MatsHourlyGfmRecord.SamplingRateUom.NotInList("CCMIN,DSCMMIN,LMIN,CCHR,DSCMHR,LHR"))
                        {
                            category.CheckCatalogResult = "C";
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
        /// Average Hourly Sampling Rate Valid
        /// 
        /// Average Hourly Sampling Rate Null or Reported to Two Decimal Places
        /// </summary>
        /// <param name="category"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public static string MatsGfm6(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                if (EmParameters.MatsHourlyGfmComponentIdValid.Default(false))
                {
                    if (EmParameters.MatsHourlyGfmRecord.FlowToSamplingRatio == null)
                    {
                        if (EmParameters.MatsSamplingTrainQaStatus.NotInList("INC,EXPIRED,LOST,FAILED") &&
                            ((EmParameters.CurrentFlowMonitorHourlyRecord != null) && EmParameters.CurrentFlowMonitorHourlyRecord.ModcCd.InList("01,02,03,04,20,53,54")) &&
                            (EmParameters.MatsHourlyGfmRecord.BeginEndHourFlg != "N"))
                        {
                            category.CheckCatalogResult = "A";
                        }
                    }
                    else
                    {
                        if (EmParameters.MatsHourlyGfmRecord.BeginEndHourFlg == "N")
                        {
                            category.CheckCatalogResult = "G";
                        }
                        else if (EmParameters.MatsSamplingTrainQaStatus.NotInList("PASSED,FAILED,UNCERTAIN"))
                        {
                            category.CheckCatalogResult = "B";
                        }
                        else if (EmParameters.CurrentFlowMonitorHourlyRecord == null)
                        {
                            category.CheckCatalogResult = "F";
                        }
                        else if (EmParameters.CurrentFlowMonitorHourlyRecord.ModcCd.NotInList("01,02,03,04,20,53,54"))
                        {
                            category.CheckCatalogResult = "E";
                        }
                        else if (EmParameters.MatsHourlyGfmRecord.FlowToSamplingRatio != Math.Round(EmParameters.MatsHourlyGfmRecord.FlowToSamplingRatio.Value, 1, MidpointRounding.AwayFromZero))
                        {
                            category.CheckCatalogResult = "C";
                        }
                        else if ((EmParameters.MatsHourlyGfmRecord.FlowToSamplingRatio < 1) || (100 < EmParameters.MatsHourlyGfmRecord.FlowToSamplingRatio))
                        {
                            category.CheckCatalogResult = "D";
                        }
                        else if (EmParameters.MatsHourlyGfmComponentIdValid.Default(false) &&
                                 EmParameters.MatsSamplingTrainDictionary.ContainsKey(EmParameters.MatsSamplingTrainRecord.TrapTrainId) &&
                                 (EmParameters.MatsSamplingTrainDictionary[EmParameters.MatsSamplingTrainRecord.TrapTrainId].ReferenceSFSRRatio.Default(0) != 0))
                        {
                            decimal referenceRatio = EmParameters.MatsSamplingTrainDictionary[EmParameters.MatsSamplingTrainRecord.TrapTrainId].ReferenceSFSRRatio.Value;
                            decimal hourlySfsrRatioDev = Math.Round(Math.Abs(1 - (EmParameters.MatsHourlyGfmRecord.FlowToSamplingRatio.Value / referenceRatio)) * 100, 0, MidpointRounding.AwayFromZero);

                            EmParameters.MatsSamplingTrainDictionary[EmParameters.MatsSamplingTrainRecord.TrapTrainId].TotalSFSRRatioCount++;

                            if (hourlySfsrRatioDev > 25)
                                EmParameters.MatsSamplingTrainDictionary[EmParameters.MatsSamplingTrainRecord.TrapTrainId].DeviatedSFSRRatioCount++;
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
        /// Updates the total hours that a GFM exists for a sampling train and the count of hours where the Begin and End Flag equals "N".
        /// </summary>
        /// <param name="category">Category object for the category in which the check is running.</param>
        /// <param name="log">obsolete</param>
        /// <returns></returns>
        public static string MatsGfm7(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                if (EmParameters.MatsGfmSamplingTrainRecords != null)
                {
                    foreach (MatsSamplingTrainRecord samplingTrainRecord in EmParameters.MatsGfmSamplingTrainRecords)
                    {
                        if (samplingTrainRecord.TrainQaStatusCd.InList("PASSED,FAILED,UNCERTAIN") && (samplingTrainRecord.RataInd.Default(0) == 0))
                        {
                            if (EmParameters.MatsSamplingTrainDictionary.ContainsKey(samplingTrainRecord.TrapTrainId))
                            {
                                SamplingTrainEvalInformation dictionaryEntry = EmParameters.MatsSamplingTrainDictionary[samplingTrainRecord.TrapTrainId];
                                {
                                    dictionaryEntry.TotalGfmCount += 1;

                                    if (EmParameters.MatsHourlyGfmRecord.BeginEndHourFlg == "N")
                                    {
                                        dictionaryEntry.NotAvailablelGfmCount += 1;
                                    }
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