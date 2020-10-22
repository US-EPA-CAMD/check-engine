using System;
using System.Collections.Generic;
using System.Data;

using ECMPS.Checks.Em.Parameters;
using ECMPS.Definitions.Extensions;


namespace ECMPS.Checks.CheckEngine.SpecialParameterClasses
{

    /// <summary>
    /// Contains a queue of cDailyCalibrationTestData with the method to enqueue items
    /// and retrieve an item based on the passed op date and hour.  The QA only keeps
    /// two elements, one for the current info and the other is either the previous or
    /// next information. based on the current op date and hour.
    /// </summary>
    public class cDailyCalibrationTestDataQueue
    {

        #region Public Constructors

        /// <summary>
        /// The constructor for an object of cDailyCalibrationTestDataQueue.
        /// </summary>
        public cDailyCalibrationTestDataQueue()
        {
            TestDataQueue = new cDailyCalibrationTestData[] { null, null };
        }

        #endregion


        #region Public Methods

        /// <summary>
        /// Queues the passed element, which removes the oldes queued element.
        /// </summary>
        /// <param name="testdata">The element to queue.</param>
        public void Enqueue(cDailyCalibrationTestData testdata)
        {
            TestDataQueue[1] = TestDataQueue[0];
            TestDataQueue[0] = testdata;
        }

        /// <summary>
        /// Retrieves the test data for the given hour.
        /// 
        /// If the last queued item has a test hour on or before the passed hour
        /// information, then it is used.  Otherwise if the older queued item has
        /// a test hour on or before the passed hour then it is used.  Otherwise 
        /// a null is returned.
        /// </summary>
        /// <returns>The test data based on the passed op date and hour.</returns>
        public cDailyCalibrationTestData GetTestData()
        {
            cDailyCalibrationTestData result;

            if (OpDate.IsNull() || OpHour.IsNull())
                result = null;
            else if ((TestDataQueue[0].HasValue()) &&
                     ((TestDataQueue[0].DailyTestDate < OpDate.Value) ||
                      ((TestDataQueue[0].DailyTestDate == OpDate.Value) &&
                       (TestDataQueue[0].DailyTestHour < OpHour.Value)) ||
                      ((TestDataQueue[0].DailyTestDate == OpDate.Value) &&
                       (TestDataQueue[0].DailyTestHour == OpHour.Value) &&
                       (TestDataQueue[0].DailyTestMinute <= 44))))
                result = TestDataQueue[0];
            else if ((TestDataQueue[1].HasValue()) &&
                     ((TestDataQueue[1].DailyTestDate < OpDate.Value) ||
                      ((TestDataQueue[1].DailyTestDate == OpDate.Value) &&
                       (TestDataQueue[1].DailyTestHour <= OpHour.Value)) ||
                      ((TestDataQueue[1].DailyTestDate == OpDate.Value) &&
                       (TestDataQueue[1].DailyTestHour <= OpHour.Value) &&
                       (TestDataQueue[1].DailyTestMinute <= 44))))
                result = TestDataQueue[1];
            else
                result = null;

            return result;
        }

        /// <summary>
        /// Calls UpdateOperatingInformation for each populated queue element.
        /// </summary>
        /// <param name="monLocId">MON_LOC_ID for the location being updated.</param>
        /// <param name="opHour">The operating hour for which the update is occuring.</param>
        /// <param name="opTime">The operating time for the hour.</param>
        /// <param name="systemOpTimeDictionary">Contains the MON_SYS_ID and operating times for the primary and primary bypass systems When a primary bypass is active.</param>
        public void UpdateOperatingInformation(string monLocId, DateTime opHour, decimal opTime, Dictionary<string, decimal> systemOpTimeDictionary)
        {
            foreach (cDailyCalibrationTestData testData in TestDataQueue)
                if (testData != null)
                    testData.UpdateOperatingInformation(monLocId, opHour, opTime, systemOpTimeDictionary);
        }


        /// <summary>
        /// Updates the supplemental data load tables for the last Daily Calibration Test added to the queue, which is in position 0.
        /// </summary>
        /// <param name="SupplementalDataUpdateLocationDataTable"></param>
        /// <param name="SupplementalDataUpdateSystemDataTable"></param>
        /// <param name="rptPeriodId"></param>
        /// <param name="workspaceSessionId"></param>
        public void LoadIntoSupplementalDataTables(DataTable SupplementalDataUpdateLocationDataTable, DataTable SupplementalDataUpdateSystemDataTable, int rptPeriodId, decimal workspaceSessionId)
        {
            if (TestDataQueue[0] != null)
                TestDataQueue[0].LoadIntoSupplementalDataTables(SupplementalDataUpdateLocationDataTable, SupplementalDataUpdateSystemDataTable, rptPeriodId, workspaceSessionId);
        }

        #endregion


        #region Public Properties

        /// <summary>
        /// The Op Date based on the Current_Operating_Date check parameter.
        /// </summary>
        public DateTime? OpDate
        {
            get
            {
               return EmParameters.CurrentOperatingDate;
            }
        }

        /// <summary>
        /// The Op Hour based on the Current_Operating_Hour check parameter.
        /// </summary>
        public int? OpHour
        {
            get
            {
                return EmParameters.CurrentOperatingHour;
            }
        }

        #endregion


        #region Private Fields

        /// <summary>
        /// Two element queue of test data information.
        /// </summary>
        private cDailyCalibrationTestData[] TestDataQueue;

        #endregion

    }

}
