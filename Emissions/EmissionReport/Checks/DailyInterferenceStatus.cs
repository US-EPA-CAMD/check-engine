using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.CheckEngine.SpecialParameterClasses;
using ECMPS.Checks.Em.Parameters;
using ECMPS.Checks.TypeUtilities;
using ECMPS.Checks.EmissionsReport;

using ECMPS.Definitions.Extensions;


namespace ECMPS.Checks.EmissionsChecks
{

    public class cDailyInterferenceStatusChecks : cEmissionsChecks
    {

        #region Constructors

        /// <summary>
        /// Creates and instance of the Flow-to-Load checks object and populates its CheckProcedures array.
        /// </summary>
        /// <param name="emissionReportProcess">The owning emission report process object.</param>
        public cDailyInterferenceStatusChecks(cEmissionsReportProcess emissionReportProcess)
          : base(emissionReportProcess)
        {
            CheckProcedures = new dCheckProcedure[4];

            CheckProcedures[1] = new dCheckProcedure(INTSTAT1);
            CheckProcedures[2] = new dCheckProcedure(INTSTAT2);
            CheckProcedures[3] = new dCheckProcedure(INTSTAT3);
        }

        #endregion


        #region Checks

        #region Checks (1 - 10)

        /// <summary>
        /// INTSTAT1: Determine the Online Daily Interference Check
        /// </summary>
        /// <param name="category">Category Object</param>
        /// <param name="log">Indicates whether to log results.</param>
        /// <returns>Returns error message if check fails to run correctly.</returns>
        public string INTSTAT1(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                EmParameters.OnlineDailyIntCheck = null;
                OnlineDailyIntRecord.SetValue(null, category);
                OfflineDailyIntRecord.SetValue(null, category);
                DailyIntStatusResult.SetValue(null, category);

                string componentId = CurrentMhvRecord.Value["COMPONENT_ID"].AsString();
                DateTime currentOpHour = CurrentMhvRecord.Value["BEGIN_DATE"].AsBeginDateTime().AddHours(CurrentMhvRecord.Value["BEGIN_HOUR"].AsInteger(0));

                // Most Recent Daily Interference Check Object contains the latest/most recent  
                // online (Calculated Test Result Code not equal to 'IGNORED') and 
                // offline (Calculated Test Result Code is equal to 'IGNORED')
                // Daily Interference Checks for specific components.
                cLastDailyInterferenceCheck lastestDailyInterferenceCheckObject = (cLastDailyInterferenceCheck)LatestDailyInterferenceCheckObject.Value;

                DataRowView latestDailyIntCheckRecord;

                // Get latest online Daily Interference Check row
                {
                    if (lastestDailyInterferenceCheckObject != null)
                    {
                        EmParameters.OnlineDailyIntCheck = lastestDailyInterferenceCheckObject.Get(componentId, true);
                        latestDailyIntCheckRecord = (EmParameters.OnlineDailyIntCheck != null) ? EmParameters.OnlineDailyIntCheck.DailyInterferenceCheckRow : null;
                    }
                    else
                        latestDailyIntCheckRecord = null;
                }

                if (latestDailyIntCheckRecord != null)
                {
                    OnlineDailyIntRecord.SetValue(latestDailyIntCheckRecord, category);

                    DateTime DailyInterferenceCheckDateHour = latestDailyIntCheckRecord["DAILY_TEST_DATEHOUR"].AsEndDateTime();

                    if (OnlineDailyIntRecord.Value["TEST_RESULT_CD"].AsString() == "PASSED")
                    {
                        if ((int)(currentOpHour.Subtract(DailyInterferenceCheckDateHour).TotalHours) < 26)
                            DailyIntStatusResult.SetValue("IC", category);
                    }
                    else if (OnlineDailyIntRecord.Value["TEST_RESULT_CD"].AsString() == "FAILED")
                    {
                        DailyIntStatusResult.SetValue("OOC-Test Failed", category);
                    }
                    else if (OnlineDailyIntRecord.Value["TEST_RESULT_CD"].AsString() == "ABORTED")
                    {
                        DailyIntStatusResult.SetValue("OOC-Test Aborted", category);
                    }
                    else
                    {
                        DailyIntStatusResult.SetValue("OOC-Test Has Critical Errors", category);
                    }

                    if (DailyIntStatusResult.Value != "IC")
                    {
                        // Get latest offline Daily Interference Check row
                        {
                            if (lastestDailyInterferenceCheckObject != null)
                                latestDailyIntCheckRecord = lastestDailyInterferenceCheckObject.GetTestRow(componentId, false);
                            else
                                latestDailyIntCheckRecord = null;
                        }

                        if (latestDailyIntCheckRecord != null)
                        {
                            OfflineDailyIntRecord.SetValue(latestDailyIntCheckRecord, category);

                            if (DailyIntStatusResult.Value != null)
                                DailyIntStatusResult.SetValue(DailyIntStatusResult.Value + "*", category);
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
        /// INTSTAT2: Determine Daily Interference Status for No Prior Check
        /// </summary>
        /// <param name="category">Category Object</param>
        /// <param name="log">Indicates whether to log results.</param>
        /// <returns>Returns error message if check fails to run correctly.</returns>
        public string INTSTAT2(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                if ((DailyIntStatusResult.Value == null) && (OnlineDailyIntRecord.Value == null))
                {
                    DateTime currentOpHour = CurrentMhvRecord.Value["BEGIN_DATE"].AsBeginDateTime().AddHours(CurrentMhvRecord.Value["BEGIN_HOUR"].AsInteger(0));

                    DateTime? firstHourOfOperation;
                    {
                        if (FirstDayofOperation.Value != null)
                            firstHourOfOperation = FirstDayofOperation.Value.AsBeginDateTime().AddHours(FirstHourofOperation.Value.Default(0));
                        else
                            firstHourOfOperation = null;
                    }

                    if ((firstHourOfOperation == null) || (((int)(currentOpHour.Subtract(firstHourOfOperation.Value).TotalHours) - 1) < 25))
                    {
                        DailyIntStatusResult.SetValue("IC-Undetermined", category);
                    }
                    else
                    {
                        DataRowView hourlyNonOperatingRow = cRowFilter.FindLastRow(HourlyNonOperatingDataRecordsforLocation.Value,
                                                                                   "BEGIN_DATEHOUR",
                                                                                   new cFilterCondition[]
                                                                                   {
                                                                         new cFilterCondition("BEGIN_DATEHOUR", firstHourOfOperation.Value.AddHours(24), eFilterDataType.DateBegan, eFilterConditionRelativeCompare.LessThanOrEqual)
                                                                                   });

                        if (hourlyNonOperatingRow != null)
                        {
                            DataRowView hourlyOperatingRow = cRowFilter.FindFirstRow(HourlyOperatingDataRecordsforLocation.Value,
                                                                                     "BEGIN_DATEHOUR",
                                                                                     new cFilterCondition[]
                                                                                     {
                                                                         new cFilterCondition("BEGIN_DATEHOUR", hourlyNonOperatingRow["BEGIN_DATEHOUR"].AsBeginDateTime(), eFilterDataType.DateBegan, eFilterConditionRelativeCompare.GreaterThan),
                                                                         new cFilterCondition("BEGIN_DATEHOUR", currentOpHour, eFilterDataType.DateBegan, eFilterConditionRelativeCompare.LessThanOrEqual)
                                                                                     });

                            if ((hourlyOperatingRow != null) && (((int)(currentOpHour.Subtract(hourlyOperatingRow["BEGIN_DATEHOUR"].AsBeginDateTime()).TotalHours)) > 7))
                            {
                                DailyIntStatusResult.SetValue("OOC-No Prior Test", category);
                            }
                            else
                            {
                                DailyIntStatusResult.SetValue("IC-Undetermined", category);
                            }
                        }
                        else
                        {
                            DailyIntStatusResult.SetValue("OOC-No Prior Test", category);
                        }
                    }

                    if (DailyIntStatusResult.Value.StartsWith("OOC"))
                    {

                        // Most Recent Daily Interference Check Object contains the latest/most recent  
                        // online (Calculated Test Result Code not equal to 'IGNORED') and 
                        // offline (Calculated Test Result Code is equal to 'IGNORED')
                        // Daily Interference Checks for specific components.
                        cLastDailyInterferenceCheck lastestDailyInterferenceCheckObject = (cLastDailyInterferenceCheck)LatestDailyInterferenceCheckObject.Value;

                        string componentId = CurrentMhvRecord.Value["COMPONENT_ID"].AsString();

                        DataRowView latestDailyIntCheckRecord;

                        // Get latest offline Daily Interference Check row
                        {
                            if (lastestDailyInterferenceCheckObject != null)
                                latestDailyIntCheckRecord = lastestDailyInterferenceCheckObject.GetTestRow(componentId, false);
                            else
                                latestDailyIntCheckRecord = null;
                        }

                        if (latestDailyIntCheckRecord != null)
                        {
                            OfflineDailyIntRecord.SetValue(latestDailyIntCheckRecord, category);
                            DailyIntStatusResult.SetValue(DailyIntStatusResult.Value + "*", category);
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
        /// INTSTAT3: Determine Expiration Status for Prior Daily Interference Check
        /// </summary>
        /// <param name="category">Category Object</param>
        /// <param name="log">Indicates whether to log results.</param>
        /// <returns>Returns error message if check fails to run correctly.</returns>
        public string INTSTAT3(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                if (DailyIntStatusResult.Value == null)
                {
                    if (EmParameters.OnlineDailyIntCheck.LastCoveredNonOpHour != null)
                    {
                        DateTime MHVDate = EmParameters.CurrentDateHour.AsStartDate();
                        int MHVHour = EmParameters.CurrentDateHour.AsStartHour();

                        if ((EmParameters.OnlineDailyIntCheck.FirstOpHourAfterLastCoveredNonOpHour != null) &&
                            ((cDateFunctions.HourDifference(EmParameters.OnlineDailyIntCheck.FirstOpHourAfterLastCoveredNonOpHour.Value, EmParameters.CurrentDateHour.Default(DateTypes.START)) + 1) > 8))
                        {
                            EmParameters.DailyIntStatusResult = "OOC-Expired";
                        }
                        else
                        {
                            EmParameters.DailyIntStatusResult = "IC-Grace";
                        }
                    }
                    else
                    {
                        EmParameters.DailyIntStatusResult = "OOC-Expired";
                    }

                    if (EmParameters.DailyIntStatusResult.StartsWith("OOC") && (EmParameters.OfflineDailyIntRecord != null))
                    {
                        EmParameters.DailyIntStatusResult += "*";
                    }
                }

                //EC-2490:  MJ: 2015-12-10
                //EC-3141:  MH:  Moved to the correct location matching the spec
                if (!EmParameters.DailyIntStatusResult.StartsWith("IC"))
                {
                    category.CheckCatalogResult = EmParameters.DailyIntStatusResult;
                }
            }
            catch (Exception ex)
            {
                returnVal = category.CheckEngine.FormatError(ex);
            }

            return returnVal;
        }

        #endregion

        #endregion

    }

}
