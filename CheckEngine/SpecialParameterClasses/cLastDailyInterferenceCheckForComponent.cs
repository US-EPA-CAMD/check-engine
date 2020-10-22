using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using ECMPS.Checks.TypeUtilities;

using ECMPS.Common;
using ECMPS.Definitions.Extensions;
using ECMPS.Definitions.SeverityCode;


namespace ECMPS.Checks.CheckEngine.SpecialParameterClasses
{

    internal class cLastDailyInterferenceCheckForComponent
    {

        #region Public Constructor

        /// <summary>
        /// Constructor for cDailyCalibrationComponentData.
        /// </summary>
        /// <param name="componentId">The Component Id represented by the object.</param>
        /// <param name="monLocId">The Monitor Location Id contained in the object.</param>
        /// <param name="lastDailyInterferenceCheck">The parent Last Daily Interference Check object.</param>
        public cLastDailyInterferenceCheckForComponent(string componentId, string monLocId, cLastDailyInterferenceCheck lastDailyInterferenceCheck)
        {
            ComponentId = componentId;
            MonLocId = monLocId;
            LastDailyInterferenceCheck = lastDailyInterferenceCheck;

            LastOpHour = DateTime.MinValue;

            LatestOfflineTest = null;
            LatestOnlineTest = null;
        }

        #endregion


        #region Public Properties

        /// <summary>
        /// The Component Id for this set of daily calibration data.
        /// </summary>
        public string ComponentId { get; private set; }

        /// <summary>
        /// The parant object for this component specific object.
        /// </summary>
        public cLastDailyInterferenceCheck LastDailyInterferenceCheck { get; private set; }

        /// <summary>
        /// Latest offline test information object.
        /// </summary>
        public cLastDailyInterferenceCheckTest LatestOfflineTest { get; private set; }

        /// <summary>
        /// Latest online test information object.
        /// </summary>
        public cLastDailyInterferenceCheckTest LatestOnlineTest { get; private set; }

        /// <summary>
        /// The 
        /// </summary>
        public DateTime LastOpHour { get; private set; }

        /// <summary>
        /// The Monitor Location Id for this set of daily calibration data.
        /// </summary>
        public string MonLocId { get; private set; }

        #endregion


        #region Public Method

        /// <summary>
        /// Replaces the current online or offline Daily Interference Check for the specific component.
        /// 
        /// The method checks the component id and identifier of the test against the current
        /// values for the object to make sure they are the same.
        /// </summary>
        /// <param name="latestTest">The test to add.</param>
        /// <param name="currentOpHour">The current operating hour.</param>
        /// <param name="resultMessage">Message indicating why the addition failed.</param>
        /// <returns>Returns true if the replacement occurred.</returns>
        public bool Add(cLastDailyInterferenceCheckTest latestTest, DateTime currentOpHour, ref string resultMessage)
        {
            bool result;

            if (latestTest == null)
            {
                resultMessage = "cLastDailyInterferenceCheckForComponent.Add() Required test object is null.";
                result = false;
            }
            else if (latestTest.DailyTestDateHour > currentOpHour)
            {
                resultMessage = "cLastDailyInterferenceCheckForComponent.Add() Daily Interference Check hour is after the current op hour.";
                result = false;
            }
            else if (latestTest.DailyTestDateHour < LastOpHour)
            {
                resultMessage = "cLastDailyInterferenceCheckForComponent.Add() Daily Interference Check hour is before the last op hour.";
                result = false;
            }
            else
            {
                if (latestTest.IsOnlineTest)
                {
                    LatestOnlineTest = latestTest;
                }
                else
                {
                    LatestOfflineTest = latestTest;
                }

                LastOpHour = currentOpHour;

                result = true;
            }

            return result;
        }


        /// <summary>
        /// Retrieves the most recent daily interference check matching the passed span scale.
        /// </summary>
        /// <param name="online">Indicates whether to get the online or offline check.</param>
        /// <returns>The check row or null if it was not found.</returns>
        public cLastDailyInterferenceCheckTest Get(bool online)
        {
            cLastDailyInterferenceCheckTest result;

            if (online)
                result = LatestOnlineTest;
            else
                result = LatestOfflineTest;

            return result;
        }


        /// <summary>
        /// Creates Daily Calibration Test Data object from a Daily Test (Location) Supplemental Data row and a list
        /// of Daily Test System Supplemental Data rows.
        /// </summary>
        /// <param name="latestTest">The test to add.</param>
        /// <param name="resultMessage">Message indicating why the addition failed.</param>
        /// <returns>Returns true if the update occurred.</returns>
        public bool InitializeFromPreviousQuarter(cLastDailyInterferenceCheckTest latestTest, ref string resultMessage)
        {
            bool result;

            if (latestTest == null)
            {
                resultMessage = "cLastDailyInterferenceCheckForComponent.InitializeFromPreviousQuarter() Required test object is null.";
                result = false;
            }
            else
            {
                if (latestTest.IsOnlineTest)
                {
                    LatestOnlineTest = latestTest;
                }
                else
                {
                    LatestOfflineTest = latestTest;
                }

                LastOpHour = DateTime.MinValue;

                result = true;
            }

            return result;
        }


        /// <summary>
        /// Updates the supplemental data load tables for each test data queue.
        /// </summary>
        /// <param name="SupplementalDataUpdateTable"></param>
        /// <param name="rptPeriodId"></param>
        /// <param name="workspaceSessionId"></param>
        public void LoadIntoSupplementalDataTables(DataTable SupplementalDataUpdateTable, int rptPeriodId, decimal workspaceSessionId)
        {
            if (LatestOnlineTest != null) LatestOnlineTest.LoadIntoSupplementalDataTables(SupplementalDataUpdateTable, rptPeriodId, workspaceSessionId);
            if (LatestOfflineTest != null) LatestOfflineTest.LoadIntoSupplementalDataTables(SupplementalDataUpdateTable, rptPeriodId, workspaceSessionId);
        }


        /// <summary>
        /// Calls UpdateOperatingInformation for each populated queue.
        /// 
        /// The call will update operating information for the elements in each queue.
        /// </summary>
        /// <param name="monLocId">MON_LOC_ID for the location being updated.</param>
        /// <param name="opHour">The operating hour for which the update is occuring.</param>
        /// <param name="opTime">The operating time for the hour.</param>
        public void UpdateOperatingInformation(string monLocId, DateTime opHour, decimal opTime)
        {
            if (LatestOnlineTest != null) LatestOnlineTest.UpdateOperatingInformation(monLocId, opHour, opTime);
            if (LatestOfflineTest != null) LatestOfflineTest.UpdateOperatingInformation(monLocId, opHour, opTime);
        }

        #endregion

    }

}
