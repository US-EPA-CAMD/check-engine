using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.CheckEngine.SpecialParameterClasses;
using ECMPS.Checks.Em.Parameters;
using ECMPS.Checks.EmissionsReport;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;
using ECMPS.Definitions.Extensions;

namespace ECMPS.Checks.EmissionsChecks
{
    public class cMATSSamplingTrainChecks : cEmissionsChecks
    {

        #region Constructors

        public cMATSSamplingTrainChecks(cEmissionsReportProcess emissionReportProcess)
            : base(emissionReportProcess)
        {
            CheckProcedures = new dCheckProcedure[18];

            CheckProcedures[1] = new dCheckProcedure(MatsTrn1);
            CheckProcedures[2] = new dCheckProcedure(MatsTrn2);
            CheckProcedures[3] = new dCheckProcedure(MatsTrn3);
            CheckProcedures[4] = new dCheckProcedure(MatsTrn4);
            CheckProcedures[5] = new dCheckProcedure(MatsTrn5);
            CheckProcedures[6] = new dCheckProcedure(MatsTrn6);
            CheckProcedures[7] = new dCheckProcedure(MatsTrn7);
            CheckProcedures[8] = new dCheckProcedure(MatsTrn8);
            CheckProcedures[9] = new dCheckProcedure(MatsTrn9);
            CheckProcedures[10] = new dCheckProcedure(MatsTrn10);
            CheckProcedures[11] = new dCheckProcedure(MatsTrn11);
            CheckProcedures[12] = new dCheckProcedure(MatsTrn12);
            CheckProcedures[13] = new dCheckProcedure(MatsTrn13);
            CheckProcedures[14] = new dCheckProcedure(MatsTrn14);
            CheckProcedures[15] = new dCheckProcedure(MatsTrn15);
            CheckProcedures[16] = new dCheckProcedure(MatsTrn16);
            CheckProcedures[17] = new dCheckProcedure(MatsTrn17);
        }

        #endregion


        #region Checks 1-10

        /// <summary>
        /// Component Id Valid
        /// 
        /// Ensure that Component ID exists Sampling Train, and that the type of the associated component is 'STRAIN'.
        /// </summary>
        /// <param name="category"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public static string MatsTrn1(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                EmParameters.MatsSamplingTrainComponentIdValid = false;

                if (EmParameters.MatsSamplingTrainRecord.ComponentId == null)
                {
                    EmParameters.MatsSamplingTrainProblemComponentExists = true;
                    category.CheckCatalogResult = "A";
                }
                else if (EmParameters.MatsSamplingTrainRecord.ComponentTypeCd != "STRAIN")
                {
                    EmParameters.MatsSamplingTrainProblemComponentExists = true;
                    category.CheckCatalogResult = "B";
                }
                else
                {
                    EmParameters.MatsSamplingTrainComponentIdValid = true;

                    SamplingTrainEvalInformation samplingTrainEvalInformation = new SamplingTrainEvalInformation(EmParameters.MatsSamplingTrainRecord.TrapTrainId, 
                                                                                                                 (EmParameters.MatsSamplingTrainRecord.BorderTrapInd == 1), 
                                                                                                                 (EmParameters.MatsSamplingTrainRecord.SuppDataInd == 1));
                    {
                        if (EmParameters.MatsSamplingTrainRecord.SuppDataInd == 1)
                        {
                            /* Initialize all values for supplemental data, since they were previously checked and will not be set by checks in this evaluation. */
                            samplingTrainEvalInformation.HgConcentration = EmParameters.MatsSamplingTrainRecord.HgConcentration.ScientificNotationtoDecimal();
                            samplingTrainEvalInformation.TrainQAStatusCode = EmParameters.MatsSamplingTrainRecord.TrainQaStatusCd;
                            samplingTrainEvalInformation.ReferenceSFSRRatio = EmParameters.MatsSamplingTrainRecord.RefFlowToSamplingRatio;
                            samplingTrainEvalInformation.TotalSFSRRatioCount = EmParameters.MatsSamplingTrainRecord.SfsrTotalCount.Default(0);
                            samplingTrainEvalInformation.DeviatedSFSRRatioCount = EmParameters.MatsSamplingTrainRecord.SfsrDeviatedCount.Default(0);
                            samplingTrainEvalInformation.TotalGfmCount = EmParameters.MatsSamplingTrainRecord.GfmTotalCount.Default(0);
                            samplingTrainEvalInformation.NotAvailablelGfmCount = EmParameters.MatsSamplingTrainRecord.GfmNotAvailableCount.Default(0);
                        }
                        else
                        {
                            samplingTrainEvalInformation.HgConcentration = null;
                            samplingTrainEvalInformation.TrainQAStatusCode = null;
                            samplingTrainEvalInformation.ReferenceSFSRRatio = null;
                            samplingTrainEvalInformation.TotalSFSRRatioCount = 0;
                            samplingTrainEvalInformation.DeviatedSFSRRatioCount = 0;
                            samplingTrainEvalInformation.TotalGfmCount = 0;
                            samplingTrainEvalInformation.NotAvailablelGfmCount = 0;
                        }

                        samplingTrainEvalInformation.SamplingTrainValid = true;
                    }

                    EmParameters.MatsSamplingTrainDictionary[samplingTrainEvalInformation.TrapTrainId] = samplingTrainEvalInformation;
                    EmParameters.MatsSorbentTrapSamplingTrainList.Add(samplingTrainEvalInformation);
                }
            }
            catch (Exception ex)
            {
                returnVal = category.CheckEngine.FormatError(ex);
            }

            return returnVal;
        }

        /// <summary>
        /// Sorbent Trap Serial Number
        /// 
        /// Check that a sorbent trap serial number is provided.
        /// </summary>
        /// <param name="category"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public static string MatsTrn2(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                if (EmParameters.MatsSamplingTrainRecord.SorbentTrapSerialNumber == null)
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

        /// <summary>
        /// Train Quality Assurance Status Valid
        /// 
        /// Check Sampling Train Quality Assurance Status Matches Lookup Table
        /// </summary>
        /// <param name="category"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public static string MatsTrn3(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                EmParameters.MatsSamplingTrainQaStatusCodeValid = false;

                if (EmParameters.MatsSamplingTrainRecord.TrainQaStatusCd == null)
                {
                    if (EmParameters.MatsSamplingTrainComponentIdValid.Default(false))
                        EmParameters.MatsSamplingTrainDictionary[EmParameters.MatsSamplingTrainRecord.TrapTrainId].SamplingTrainValid = false;

                    category.CheckCatalogResult = "A";
                }
                else if (EmParameters.MatsSamplingTrainQaStatusLookupTable.CountRows(new cFilterCondition[] { new cFilterCondition("TRAIN_QA_STATUS_CD", EmParameters.MatsSamplingTrainRecord.TrainQaStatusCd) }) == 0)
                {
                    if (EmParameters.MatsSamplingTrainComponentIdValid.Default(false))
                        EmParameters.MatsSamplingTrainDictionary[EmParameters.MatsSamplingTrainRecord.TrapTrainId].SamplingTrainValid = false;

                    category.CheckCatalogResult = "B";
                }
                else
                {
                    EmParameters.MatsSamplingTrainQaStatusCodeValid = true;

                    if (EmParameters.MatsSamplingTrainComponentIdValid.Default(false))
                        EmParameters.MatsSamplingTrainDictionary[EmParameters.MatsSamplingTrainRecord.TrapTrainId].TrainQAStatusCode = EmParameters.MatsSamplingTrainRecord.TrainQaStatusCd;
                }
            }
            catch (Exception ex)
            {
                returnVal = category.CheckEngine.FormatError(ex);
            }

            return returnVal;
        }

        /// <summary>
        /// Main Trap Hg Valid
        /// 
        /// Main Trap Hg Null or Reported to Two Decimal Places.
        /// </summary>
        /// <param name="category"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public static string MatsTrn4(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                EmParameters.MatsMainTrapHgValid = false;

                if (EmParameters.MatsSamplingTrainQaStatusCodeValid.Default(false))
                {
                    if (EmParameters.MatsSamplingTrainRecord.MainTrapHg == null)
                    {
                        if (EmParameters.MatsSamplingTrainRecord.TrainQaStatusCd.NotInList("INC,EXPIRED,LOST"))
                        {
                            category.CheckCatalogResult = "A";
                        }
                        else
                        {
                            EmParameters.MatsMainTrapHgValid = true;
                        }
                    }
                    else
                    {
                        if (EmParameters.MatsSamplingTrainRecord.TrainQaStatusCd.NotInList("PASSED,FAILED,UNCERTAIN"))
                        {
                            category.CheckCatalogResult = "B";
                        }
                        else if (!EmParameters.MatsSamplingTrainRecord.MainTrapHg.SignificantDigitNotationValid(3))
                        {
                            category.CheckCatalogResult = "C";
                        }
                        else
                        {
                            EmParameters.MatsMainTrapHgValid = true;
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
        /// Breakthrough Trap Hg Valid
        /// 
        /// Breakthrough Trap Hg Null or Reported to Two Decimal Places.
        /// </summary>
        /// <param name="category"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public static string MatsTrn5(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                EmParameters.MatsBtTrapHgValid = false;

                if (EmParameters.MatsSamplingTrainQaStatusCodeValid.Default(false))
                {
                    if (EmParameters.MatsSamplingTrainRecord.BreakthroughTrapHg == null)
                    {
                        if (EmParameters.MatsSamplingTrainRecord.TrainQaStatusCd.NotInList("INC,EXPIRED,LOST"))
                        {
                            category.CheckCatalogResult = "A";
                        }
                        else
                        {
                            EmParameters.MatsBtTrapHgValid = true;
                        }
                    }
                    else
                    {
                        if (EmParameters.MatsSamplingTrainRecord.TrainQaStatusCd.NotInList("PASSED,FAILED,UNCERTAIN"))
                        {
                            category.CheckCatalogResult = "B";
                        }
                        else if (!EmParameters.MatsSamplingTrainRecord.BreakthroughTrapHg.SignificantDigitNotationValid(3))
                        {
                            category.CheckCatalogResult = "C";
                        }
                        else
                        {
                            EmParameters.MatsBtTrapHgValid = true;
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
        /// Spike Trap Hg Valid
        /// 
        /// Spike Trap Hg Null or Reported to Two Decimal Places.
        /// </summary>
        /// <param name="category"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public static string MatsTrn6(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                EmParameters.MatsSpikeTrapHgValid = false;

                if (EmParameters.MatsSamplingTrainQaStatusCodeValid.Default(false))
                {
                    if (EmParameters.MatsSamplingTrainRecord.SpikeTrapHg == null)
                    {
                        if (EmParameters.MatsSamplingTrainRecord.TrainQaStatusCd.NotInList("INC,EXPIRED,LOST"))
                        {
                            category.CheckCatalogResult = "A";
                        }
                        else
                        {
                            EmParameters.MatsSpikeTrapHgValid = true;
                        }
                    }
                    else
                    {
                        if (EmParameters.MatsSamplingTrainRecord.TrainQaStatusCd.NotInList("PASSED,FAILED,UNCERTAIN"))
                        {
                            category.CheckCatalogResult = "B";
                        }
                        else if (!EmParameters.MatsSamplingTrainRecord.SpikeTrapHg.SignificantDigitNotationValid(3))
                        {
                            category.CheckCatalogResult = "C";
                        }
                        else
                        {
                            EmParameters.MatsSpikeTrapHgValid = true;
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
        /// Spike Reference Value Valid
        /// 
        /// Spike Reference Value Null or Reported to Two Decimal Places.
        /// </summary>
        /// <param name="category"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public static string MatsTrn7(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                EmParameters.MatsSpikeReferenceValueValid = false;

                if (EmParameters.MatsSamplingTrainQaStatusCodeValid.Default(false))
                {
                    if (EmParameters.MatsSamplingTrainRecord.SpikeRefValue == null)
                    {
                        if (EmParameters.MatsSamplingTrainRecord.TrainQaStatusCd.NotInList("INC,EXPIRED,LOST"))
                        {
                            category.CheckCatalogResult = "A";
                        }
                        else
                        {
                            EmParameters.MatsSpikeReferenceValueValid = true;
                        }
                    }
                    else
                    {
                        if (EmParameters.MatsSamplingTrainRecord.TrainQaStatusCd.NotInList("PASSED,FAILED,UNCERTAIN"))
                        {
                            category.CheckCatalogResult = "B";
                        }
                        else if (!EmParameters.MatsSamplingTrainRecord.SpikeRefValue.SignificantDigitNotationValid(3))
                        {
                            category.CheckCatalogResult = "C";
                        }
                        else
                        {
                            EmParameters.MatsSpikeReferenceValueValid = true;
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
        /// Total Sample Volume DSCM Valid
        /// 
        /// Total Sample Volume DSCM Null or Reported to Two Decimal Places.
        /// </summary>
        /// <param name="category"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public static string MatsTrn8(cCategory category, ref bool log)
        {
            string returnVal = "";
            decimal dValue = 0.0M;
            try
            {
                EmParameters.MatsTotalSampleVolumeDscmValid = false;

                if (EmParameters.MatsSamplingTrainQaStatusCodeValid.Default(false))
                {
                    if (EmParameters.MatsSamplingTrainRecord.TotalSampleVolume == null)
                    {
                        if (EmParameters.MatsSamplingTrainRecord.TrainQaStatusCd.NotInList("INC,EXPIRED,LOST"))
                        {
                            category.CheckCatalogResult = "A";
                        }
                        else
                        {
                            EmParameters.MatsTotalSampleVolumeDscmValid = true;
                        }
                    }
                    else
                    {
                        if (EmParameters.MatsSamplingTrainRecord.TrainQaStatusCd.NotInList("PASSED,FAILED,UNCERTAIN"))
                        {
                            category.CheckCatalogResult = "B";
                        }
                        //  EC-2451 M. Jones 2015-11-05 
                        //  float type and regular expression pattern enforces at least two (2) digits and 
                        //  no more than four (4) following decimal.
                        else if (!decimal.TryParse(EmParameters.MatsSamplingTrainRecord.TotalSampleVolume.Value.ToString(),
                                                   out dValue))
                        {
                            category.CheckCatalogResult = "C";
                        }
                        else
                        {
                            EmParameters.MatsTotalSampleVolumeDscmValid = true;
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
        /// Sampling Ratio Check Result Code Valid
        /// </summary>
        /// <param name="category"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public static string MatsTrn9(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                if (EmParameters.MatsSamplingTrainQaStatusCodeValid.Default(false))
                {
                    if (EmParameters.MatsSamplingTrainRecord.RefFlowToSamplingRatio == null)
                    {
                        if (EmParameters.MatsSamplingTrainRecord.TrainQaStatusCd.NotInList("INC,EXPIRED,LOST")  &&
                            (EmParameters.MatsSamplingTrainRecord.RataInd != 1))
                        {
                            if (EmParameters.MatsSamplingTrainDictionary.ContainsKey(EmParameters.MatsSamplingTrainRecord.TrapTrainId))
                                EmParameters.MatsSamplingTrainDictionary[EmParameters.MatsSamplingTrainRecord.TrapTrainId].SamplingTrainValid = false;

                            category.CheckCatalogResult = "A";
                        }
                        else
                        {
                            EmParameters.MatsTotalSampleVolumeDscmValid = true;
                        }
                    }
                    else
                    {
                        if (EmParameters.MatsSamplingTrainRecord.TrainQaStatusCd.NotInList("PASSED,FAILED,UNCERTAIN"))
                        {
                            if (EmParameters.MatsSamplingTrainDictionary.ContainsKey(EmParameters.MatsSamplingTrainRecord.TrapTrainId))
                                EmParameters.MatsSamplingTrainDictionary[EmParameters.MatsSamplingTrainRecord.TrapTrainId].SamplingTrainValid = false;

                            category.CheckCatalogResult = "B";
                        }
                        else if (EmParameters.MatsSamplingTrainRecord.RefFlowToSamplingRatio.Value != Math.Round(EmParameters.MatsSamplingTrainRecord.RefFlowToSamplingRatio.Value, 2, MidpointRounding.AwayFromZero))
                        {
                            if (EmParameters.MatsSamplingTrainDictionary.ContainsKey(EmParameters.MatsSamplingTrainRecord.TrapTrainId))
                                EmParameters.MatsSamplingTrainDictionary[EmParameters.MatsSamplingTrainRecord.TrapTrainId].SamplingTrainValid = false;

                            category.CheckCatalogResult = "C";
                        }
                        else
                        {
                            if (EmParameters.MatsSamplingTrainDictionary.ContainsKey(EmParameters.MatsSamplingTrainRecord.TrapTrainId))
                                EmParameters.MatsSamplingTrainDictionary[EmParameters.MatsSamplingTrainRecord.TrapTrainId].ReferenceSFSRRatio = EmParameters.MatsSamplingTrainRecord.RefFlowToSamplingRatio;
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
        /// Total Sample Volume DSCM Valid
        /// 
        /// Total Sample Volume DSCM Null or Reported to Two Decimal Places.
        /// </summary>
        /// <param name="category"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public static string MatsTrn10(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                if (EmParameters.MatsSamplingTrainQaStatusCodeValid.Default(false))
                {
                    if (EmParameters.MatsSamplingTrainRecord.SamplingRatioTestResultCd == null)
                    {
                        if (EmParameters.MatsSamplingTrainRecord.TrainQaStatusCd.NotInList("INC,EXPIRED,LOST"))
                        {
                            if (EmParameters.MatsSamplingTrainComponentIdValid.Default(false))
                                EmParameters.MatsSamplingTrainDictionary[EmParameters.MatsSamplingTrainRecord.TrapTrainId].SamplingTrainValid = false;

                            category.CheckCatalogResult = "A";
                        }
                    }
                    else
                    {
                        if (EmParameters.MatsSamplingTrainRecord.SamplingRatioTestResultCd == "PASSED")
                        {
                            if (EmParameters.MatsSamplingTrainRecord.TrainQaStatusCd.NotInList("PASSED,FAILED,UNCERTAIN"))
                            {
                                if (EmParameters.MatsSamplingTrainComponentIdValid.Default(false))
                                    EmParameters.MatsSamplingTrainDictionary[EmParameters.MatsSamplingTrainRecord.TrapTrainId].SamplingTrainValid = false;

                                category.CheckCatalogResult = "B";
                            }
                        }
                        else if (EmParameters.MatsSamplingTrainRecord.SamplingRatioTestResultCd == "FAILED")
                        {
                            if (EmParameters.MatsSamplingTrainRecord.TrainQaStatusCd != "FAILED")
                            {
                                if (EmParameters.MatsSamplingTrainComponentIdValid.Default(false))
                                    EmParameters.MatsSamplingTrainDictionary[EmParameters.MatsSamplingTrainRecord.TrapTrainId].SamplingTrainValid = false;

                                category.CheckCatalogResult = "C";
                            }
                        }
                        else
                        {
                            if (EmParameters.MatsSamplingTrainComponentIdValid.Default(false))
                                EmParameters.MatsSamplingTrainDictionary[EmParameters.MatsSamplingTrainRecord.TrapTrainId].SamplingTrainValid = false;

                            category.CheckCatalogResult = "D";
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


        #region Checks 11-20

        /// <summary>
        /// Post Leak Check Result Code Valid.
        /// </summary>
        /// <param name="category"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public static string MatsTrn11(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                if (EmParameters.MatsSamplingTrainQaStatusCodeValid.Default(false))
                {
                    if (EmParameters.MatsSamplingTrainRecord.PostLeakTestResultCd == null)
                    {
                        if (EmParameters.MatsSamplingTrainRecord.TrainQaStatusCd.NotInList("INC,EXPIRED,LOST"))
                        {
                            category.CheckCatalogResult = "A";
                        }
                    }
                    else
                    {
                        if (EmParameters.MatsSamplingTrainRecord.PostLeakTestResultCd == "PASSED")
                        {
                            if (EmParameters.MatsSamplingTrainRecord.TrainQaStatusCd.NotInList("PASSED,FAILED,UNCERTAIN"))
                            {
                                category.CheckCatalogResult = "B";
                            }
                        }
                        else if (EmParameters.MatsSamplingTrainRecord.PostLeakTestResultCd == "FAILED")
                        {
                            if (EmParameters.MatsSamplingTrainRecord.TrainQaStatusCd != "FAILED")
                            {
                                category.CheckCatalogResult = "C";
                            }
                        }
                        else
                        {
                            category.CheckCatalogResult = "D";
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
        /// Sample Damage Explanation
        /// 
        /// Sample Damage Explanation is provided if QA Status Code equals LOST.
        /// </summary>
        /// <param name="category"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public static string MatsTrn12(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                if (EmParameters.MatsSamplingTrainQaStatusCodeValid.Default(false))
                {
                    if (EmParameters.MatsSamplingTrainRecord.SampleDamageExplanation == null)
                    {
                        if (EmParameters.MatsSamplingTrainRecord.TrainQaStatusCd == "LOST")
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
        /// Hg Concentration Reported Properly
        /// </summary>
        /// <param name="category"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public static string MatsTrn13(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                EmParameters.MatsCalcTrainHgConcentration = null;

                if (EmParameters.MatsSamplingTrainQaStatusCodeValid.Default(false))
                {
                    if (EmParameters.MatsSamplingTrainRecord.HgConcentration == null)
                    {
                        if (EmParameters.MatsSamplingTrainRecord.TrainQaStatusCd.NotInList("INC,EXPIRED,LOST"))
                        {
                            if (EmParameters.MatsSamplingTrainComponentIdValid.Default(false))
                                EmParameters.MatsSamplingTrainDictionary[EmParameters.MatsSamplingTrainRecord.TrapTrainId].SamplingTrainValid = false;

                            category.CheckCatalogResult = "A";
                        }
                    }
                    else
                    {
                        if (EmParameters.MatsSamplingTrainRecord.TrainQaStatusCd.NotInList("PASSED,FAILED,UNCERTAIN"))
                        {
                            if (EmParameters.MatsSamplingTrainComponentIdValid.Default(false))
                                EmParameters.MatsSamplingTrainDictionary[EmParameters.MatsSamplingTrainRecord.TrapTrainId].SamplingTrainValid = false;

                            category.CheckCatalogResult = "B";
                        }
                        else if (!EmParameters.MatsSamplingTrainRecord.HgConcentration.SignificantDigitNotationValid(3))
                        {
                            if (EmParameters.MatsSamplingTrainComponentIdValid.Default(false))
                                EmParameters.MatsSamplingTrainDictionary[EmParameters.MatsSamplingTrainRecord.TrapTrainId].SamplingTrainValid = false;

                            category.CheckCatalogResult = "C";
                        }
                        else if (EmParameters.MatsSamplingTrainRecord.TrapModcCd.InList("43,44"))
                        {
                            EmParameters.MatsCalcTrainHgConcentration = EmParameters.MatsSamplingTrainRecord.HgConcentration;

                            if (EmParameters.MatsSamplingTrainComponentIdValid.Default(false))
                                EmParameters.MatsSamplingTrainDictionary[EmParameters.MatsSamplingTrainRecord.TrapTrainId].HgConcentration = EmParameters.MatsSamplingTrainRecord.HgConcentration.ScientificNotationtoDecimal();
                        }
                        else if (EmParameters.MatsMainTrapHgValid.Default(false) && EmParameters.MatsBtTrapHgValid.Default(false) && EmParameters.MatsTotalSampleVolumeDscmValid.Default(false) && EmParameters.MatsSamplingTrainRecord.TotalSampleVolume != null && EmParameters.MatsSamplingTrainRecord.TotalSampleVolume != 0)
                        {
                            decimal hgConcentration
                              = ((EmParameters.MatsSamplingTrainRecord.MainTrapHg.ScientificNotationtoDecimal() + EmParameters.MatsSamplingTrainRecord.BreakthroughTrapHg.ScientificNotationtoDecimal())
                                 / EmParameters.MatsSamplingTrainRecord.TotalSampleVolume.Value);

                            EmParameters.MatsCalcTrainHgConcentration = hgConcentration.DecimaltoScientificNotation(3);

                            if (EmParameters.MatsSamplingTrainComponentIdValid.Default(false))
                                EmParameters.MatsSamplingTrainDictionary[EmParameters.MatsSamplingTrainRecord.TrapTrainId].HgConcentration = hgConcentration.DecimaltoScientificNotation(3).ScientificNotationtoDecimal();

                            if (EmParameters.MatsSamplingTrainRecord.HgConcentration != EmParameters.MatsCalcTrainHgConcentration)
                            {
                                if (EmParameters.MatsSamplingTrainComponentIdValid.Default(false))
                                    EmParameters.MatsSamplingTrainDictionary[EmParameters.MatsSamplingTrainRecord.TrapTrainId].SamplingTrainValid = false;

                                category.CheckCatalogResult = "D";
                            }
                        }
                        else // Calculation input is not valid
                        {
                            if (EmParameters.MatsSamplingTrainComponentIdValid.Default(false))
                                EmParameters.MatsSamplingTrainDictionary[EmParameters.MatsSamplingTrainRecord.TrapTrainId].SamplingTrainValid = false;
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
        /// Percent Breakthrough Reported Properly
        /// </summary>
        /// <param name="category"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public static string MatsTrn14(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                EmParameters.MatsCalcTrainPercentBreakthrough = null;

                if (EmParameters.MatsSamplingTrainQaStatusCodeValid.Default(false))
                {
                    if (EmParameters.MatsSamplingTrainRecord.PercentBreakthrough == null)
                    {
                        if ((EmParameters.MatsSamplingTrainRecord.TrainQaStatusCd.InList("PASSED,FAILED,UNCERTAIN")) &&
                            (EmParameters.MatsSamplingTrainRecord.HgConcentration.ScientificNotationtoDecimal() >= 0.2m))
                        {
                            category.CheckCatalogResult = "A";
                        }
                    }
                    else
                    {
                        if (EmParameters.MatsSamplingTrainRecord.TrainQaStatusCd.InList("INC,EXPIRED,LOST"))
                        {
                            category.CheckCatalogResult = "B";
                        }
                        else if (EmParameters.MatsSamplingTrainRecord.PercentBreakthrough != Math.Round(EmParameters.MatsSamplingTrainRecord.PercentBreakthrough.Value, 1, MidpointRounding.AwayFromZero))
                        {
                            category.CheckCatalogResult = "C";
                        }
                        else if (EmParameters.MatsMainTrapHgValid.Default(false) && EmParameters.MatsBtTrapHgValid.Default(false) && EmParameters.MatsSamplingTrainRecord.MainTrapHg != null && EmParameters.MatsSamplingTrainRecord.MainTrapHg.ScientificNotationtoDecimal() != 0)
                        {
                            EmParameters.MatsCalcTrainPercentBreakthrough = Math.Round((EmParameters.MatsSamplingTrainRecord.BreakthroughTrapHg.ScientificNotationtoDecimal() / EmParameters.MatsSamplingTrainRecord.MainTrapHg.ScientificNotationtoDecimal()) * 100, 1, MidpointRounding.AwayFromZero);

                            if (EmParameters.MatsSamplingTrainRecord.PercentBreakthrough != EmParameters.MatsCalcTrainPercentBreakthrough)
                            {
                                category.CheckCatalogResult = "D";
                            }
                            else
                            {
                                decimal hgConcentration = EmParameters.MatsSamplingTrainRecord.HgConcentration.ScientificNotationtoDecimal();
                                decimal roundedPecentBreakthrough = Math.Round(EmParameters.MatsSamplingTrainRecord.PercentBreakthrough.Value, 0, MidpointRounding.AwayFromZero);

                                /* RATA APS*/
                                if (EmParameters.MatsSamplingTrainRecord.SorbentTrapApsCd == "RATA")
                                {
                                    if (((hgConcentration > 1m) && (roundedPecentBreakthrough > 10)) ||
                                        ((hgConcentration > 0.5m) && (roundedPecentBreakthrough > 20)) ||
                                        ((hgConcentration > 0.1m) && (roundedPecentBreakthrough > 50)))
                                    {
                                        if (EmParameters.MatsSamplingTrainRecord.TrainQaStatusCd != "FAILED")
                                        {
                                            category.CheckCatalogResult = "F";
                                        }
                                    }
                                }

                                /* No APS,  PS-12B */
                                else
                                {
                                    if (hgConcentration >= 0.2m)
                                    {
                                        if ((roundedPecentBreakthrough > 10) || ((roundedPecentBreakthrough > 5) && (hgConcentration > 0.5m)))
                                        {
                                            if (EmParameters.MatsSamplingTrainRecord.TrainQaStatusCd != "FAILED")
                                            {
                                                category.CheckCatalogResult = "E";
                                            }
                                        }
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

        /// <summary>
        /// Percent Spike Recovery Reported Properly
        /// </summary>
        /// <param name="category"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public static string MatsTrn15(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                if (EmParameters.MatsSamplingTrainQaStatusCodeValid.Default(false))
                {
                    if (EmParameters.MatsSamplingTrainRecord.PercentSpikeRecovery == null)
                    {
                        if (EmParameters.MatsSamplingTrainRecord.TrainQaStatusCd.NotInList("INC,EXPIRED,LOST"))
                        {
                            category.CheckCatalogResult = "A";
                        }
                    }
                    else
                    {
                        if (EmParameters.MatsSamplingTrainRecord.TrainQaStatusCd.NotInList("PASSED,FAILED,UNCERTAIN"))
                        {
                            category.CheckCatalogResult = "B";
                        }
                        else if (EmParameters.MatsSamplingTrainRecord.PercentSpikeRecovery != Math.Round(EmParameters.MatsSamplingTrainRecord.PercentSpikeRecovery.Value, 1, MidpointRounding.AwayFromZero))
                        {
                            category.CheckCatalogResult = "C";
                        }
                        else if (EmParameters.MatsSpikeTrapHgValid.Default(false) && EmParameters.MatsSpikeReferenceValueValid.Default(false) && EmParameters.MatsSamplingTrainRecord.SpikeRefValue != null && EmParameters.MatsSamplingTrainRecord.SpikeRefValue.ScientificNotationtoDecimal() != 0)
                        {
                            EmParameters.MatsCalcTrainPercentSpikeRecovery = Math.Round((EmParameters.MatsSamplingTrainRecord.SpikeTrapHg.ScientificNotationtoDecimal() / EmParameters.MatsSamplingTrainRecord.SpikeRefValue.ScientificNotationtoDecimal()) * 100, 1, MidpointRounding.AwayFromZero);

                            if (EmParameters.MatsSamplingTrainRecord.PercentSpikeRecovery != EmParameters.MatsCalcTrainPercentSpikeRecovery)
                            {
                                category.CheckCatalogResult = "D";
                            }
                            else if ((EmParameters.MatsSamplingTrainRecord.PercentSpikeRecovery < 75) || (EmParameters.MatsSamplingTrainRecord.PercentSpikeRecovery > 125))
                            {
                                if (EmParameters.MatsSamplingTrainRecord.TrainQaStatusCd != "FAILED")
                                {
                                    category.CheckCatalogResult = "E";
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

        /// <summary>
        /// Check Hourly Sampling Ratios
        /// 
        /// Compare Hourly Sampling Ratio with PS12B Requirement
        /// </summary>
        /// <param name="category"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public static string MatsTrn16(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                SamplingTrainEvalInformation samplingTrainEvalInformation = null;
                bool? sorbentTrapValid = false;
                if (EmParameters.MatsSamplingTrainDictionary.ContainsKey(EmParameters.MatsSamplingTrainRecord.TrapTrainId))
                {
                    samplingTrainEvalInformation = EmParameters.MatsSamplingTrainDictionary[EmParameters.MatsSamplingTrainRecord.TrapTrainId];
                    {
                        sorbentTrapValid = samplingTrainEvalInformation.SamplingTrainValid;
                    }
                }

                if (sorbentTrapValid.Default(false))
                {
                    int totalSfsrRatioCount = samplingTrainEvalInformation.TotalSFSRRatioCount;
                    int deviatedSfsrRatioCount = samplingTrainEvalInformation.DeviatedSFSRRatioCount;
                    bool samplingTrainCountsAreComplete = (samplingTrainEvalInformation.IsBorderTrain == false) || (samplingTrainEvalInformation.IsSupplementalData == true);

                    if (totalSfsrRatioCount >= 100)
                    {
                        decimal matsCalcPercentSfsrRatioDev = Math.Round(((decimal)deviatedSfsrRatioCount / (decimal)totalSfsrRatioCount) * 100, 0, MidpointRounding.AwayFromZero);

                        if (EmParameters.MatsSamplingTrainRecord.SamplingRatioTestResultCd == "PASSED")
                        {
                            if (matsCalcPercentSfsrRatioDev > 5)
                                category.CheckCatalogResult = "A";
                        }
                    }
                    else
                    {
                        if (EmParameters.MatsSamplingTrainRecord.SamplingRatioTestResultCd == "PASSED")
                        {
                            if (deviatedSfsrRatioCount > 5)
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
        /// Checks the percentage of GFM with an "N" (Not Allowed) Begin End Flag.
        /// </summary>
        /// <param name="category"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public static string MatsTrn17(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                if (EmParameters.MatsSamplingTrainRecord.TrainQaStatusCd.InList("PASSED,FAILED,UNCERTAIN") && (EmParameters.MatsSamplingTrainRecord.RataInd.Default(0) == 0))
                {
                    if (EmParameters.MatsSamplingTrainDictionary.ContainsKey(EmParameters.MatsSamplingTrainRecord.TrapTrainId))
                    {
                        SamplingTrainEvalInformation dictionaryEntry = EmParameters.MatsSamplingTrainDictionary[EmParameters.MatsSamplingTrainRecord.TrapTrainId];
                        {
                            if (dictionaryEntry.SamplingTrainValid == true)
                            {
                                if ((dictionaryEntry.TotalGfmCount > 0) && (dictionaryEntry.NotAvailablelGfmCount >= 0))
                                {
                                    decimal notAvailableGfmPercent = 100 * dictionaryEntry.NotAvailablelGfmCount / dictionaryEntry.TotalGfmCount;

                                    if (notAvailableGfmPercent >= 20m)
                                    {
                                        category.CheckCatalogResult = "A";
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