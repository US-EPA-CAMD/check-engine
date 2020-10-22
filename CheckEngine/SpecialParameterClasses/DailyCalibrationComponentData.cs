using System;
using System.Collections.Generic;
using System.Data;

using ECMPS.Checks.TypeUtilities;


namespace ECMPS.Checks.CheckEngine.SpecialParameterClasses
{

    /// <summary>
    /// The component level data for Daily Calibration tests.
    /// </summary>
    public class cDailyCalibrationComponentData
    {

        #region Public Constructor

        /// <summary>
        /// Constructor for cDailyCalibrationComponentData.
        /// </summary>
        /// <param name="componentId">The Component Id represented by the object.</param>
        /// <param name="componentIdentifier">The Component Identifier of the Component Id.</param>
        public cDailyCalibrationComponentData(string componentId, string componentIdentifier)
        {
            ComponentId = componentId;
            ComponentIdentifier = componentIdentifier;

            TestDataQueue = new cDailyCalibrationTestDataQueue[3, 2, 2];

            TestDataQueue[0, 0, 0] = new cDailyCalibrationTestDataQueue();
            TestDataQueue[1, 0, 0] = new cDailyCalibrationTestDataQueue();
            TestDataQueue[2, 0, 0] = new cDailyCalibrationTestDataQueue();
            TestDataQueue[0, 1, 0] = new cDailyCalibrationTestDataQueue();
            TestDataQueue[1, 1, 0] = new cDailyCalibrationTestDataQueue();
            TestDataQueue[2, 1, 0] = new cDailyCalibrationTestDataQueue();
            TestDataQueue[0, 0, 1] = new cDailyCalibrationTestDataQueue();
            TestDataQueue[1, 0, 1] = new cDailyCalibrationTestDataQueue();
            TestDataQueue[2, 0, 1] = new cDailyCalibrationTestDataQueue();
            TestDataQueue[0, 1, 1] = new cDailyCalibrationTestDataQueue();
            TestDataQueue[1, 1, 1] = new cDailyCalibrationTestDataQueue();
            TestDataQueue[2, 1, 1] = new cDailyCalibrationTestDataQueue();
        }

        #endregion


        #region Public Properties

        /// <summary>
        /// The Component Id for this set of daily calibration data.
        /// </summary>
        public string ComponentId { get; private set; }

        /// <summary>
        /// The Component Identifier for this set of daily calibration data.
        /// </summary>
        public string ComponentIdentifier { get; private set; }

        /// <summary>
        /// The set of important test data for a daily calibration.
        /// </summary>
        public cDailyCalibrationTestDataQueue[,,] TestDataQueue { get; private set; }

        #endregion


        #region Public Method

        /// <summary>
        /// Retieves the most recent test matching the passed validity, span scale and 
        /// online/offline indicator.
        /// </summary>
        /// <param name="valid">The validity of the test.</param>
        /// <param name="spanScale">The span scale of the test.</param>
        /// <param name="online">The online/offline indicator of the test.</param>
        /// <returns>The test data or null if it was not found.</returns>
        public cDailyCalibrationTestData GetTestData(bool valid, eSpanScale spanScale, bool online)
        {
            cDailyCalibrationTestData result;

            int validityPos = (valid ? 1 : 0);
            int spanScalePos = (int)spanScale;
            int onlineOfflinePos = (online ? 1 : 0);

            result = TestDataQueue[spanScalePos, onlineOfflinePos, validityPos].GetTestData();

            return result;
        }

        /// <summary>
        /// Retieves the most recent test matching the passed validity, span scale and 
        /// online/offline indicator.
        /// </summary>
        /// <param name="valid">The validity of the test.</param>
        /// <param name="spanScale">The span scale of the test.</param>
        /// <param name="online">The online/offline indicator of the test.</param>
        /// <param name="testData">The returned test data.</param>
        /// <returns>True if the data was retrieve successfully</returns>
        public bool GetTestData(bool valid, eSpanScale spanScale, bool online,
                                out cDailyCalibrationTestData testData)
        {
            bool result;

            testData = GetTestData(valid, spanScale, online);

            result = (testData != null);

            return result;
        }

        /// <summary>
        /// Retieves the most recent test matching the passed validity, span scale and 
        /// online/offline indicator.
        /// </summary>
        /// <param name="valid">The validity of the test.</param>
        /// <param name="spanScale">The span scale of the test.</param>
        /// <returns>The test data or null if it was not found.</returns>
        public cDailyCalibrationTestData GetTestData(bool valid, eSpanScale spanScale)
        {
            cDailyCalibrationTestData result;

            cDailyCalibrationTestData onlineTestData = GetTestData(valid, spanScale, true);
            cDailyCalibrationTestData offlineTestData = GetTestData(valid, spanScale, false);

            if ((onlineTestData == null) && (offlineTestData == null))
                result = null;
            else if (onlineTestData == null)
                result = offlineTestData;
            else if (offlineTestData == null)
                result = onlineTestData;
            else if ((onlineTestData.DailyTestDate > offlineTestData.DailyTestDate) ||
                     ((onlineTestData.DailyTestDate == offlineTestData.DailyTestDate) &&
                      (onlineTestData.DailyTestHour > offlineTestData.DailyTestHour)) ||
                     ((onlineTestData.DailyTestDate == offlineTestData.DailyTestDate) &&
                      (onlineTestData.DailyTestHour == offlineTestData.DailyTestHour) &&
                      (onlineTestData.DailyTestMinute >= offlineTestData.DailyTestMinute)))
                result = onlineTestData;
            else
                result = offlineTestData;

            return result;
        }

        /// <summary>
        /// Retieves the most recent test matching the passed validity and span scale.
        /// </summary>
        /// <param name="valid">The validity of the test.</param>
        /// <param name="spanScale">The span scale of the test.</param>
        /// <param name="testData">The returned test data.</param>
        /// <returns>True if the data was retrieve successfully</returns>
        public bool GetTestData(bool valid, eSpanScale spanScale,
                                out cDailyCalibrationTestData testData)
        {
            bool result;

            testData = GetTestData(valid, spanScale);

            result = (testData != null);

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public bool Update(cDailyCalibrationTestData testData, out string errorMessage)
        {
            bool result;

            if (!testData.Valid.HasValue)
            {
                errorMessage = null;
                result = true;
            }
            else if ((testData.ComponentId != ComponentId) ||
                     (testData.ComponentIdentifier != ComponentIdentifier))
            {
                errorMessage = "TestData component does not match CompnentData component";
                result = false;
            }
            else
            {
                int validityPos = (testData.Valid.Value ? 1 : 0);
                int spanScalePos = (int)testData.SpanScaleCd;
                int onlineOfflinePos = (testData.Online ? 1 : 0);

                TestDataQueue[spanScalePos, onlineOfflinePos, validityPos].Enqueue(testData);

                errorMessage = null;
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Calls UpdateOperatingInformation for each populated queue.
        /// 
        /// The call will update operating information for the elements in each queue.
        /// </summary>
        /// <param name="monLocId">MON_LOC_ID for the location being updated.</param>
        /// <param name="opHour">The operating hour for which the update is occuring.</param>
        /// <param name="opTime">The operating time for the hour.</param>
        /// <param name="systemOpTimeDictionary">Contains the MON_SYS_ID and operating times for the primary and primary bypass systems When a primary bypass is active.</param>
        public void UpdateOperatingInformation(string monLocId, DateTime opHour, decimal opTime, Dictionary<string, decimal> systemOpTimeDictionary)
        {
            int[] indicatorList = { 0, 1 };

            cDailyCalibrationTestDataQueue testDataQueue;

            foreach (int spanScalePos in Enum.GetValues(typeof(eSpanScale)))
                foreach (int onlinePos in indicatorList)
                    foreach (int validPos in indicatorList)
                    {
                        testDataQueue = TestDataQueue[spanScalePos, onlinePos, validPos];

                        if (testDataQueue != null)
                            testDataQueue.UpdateOperatingInformation(monLocId, opHour, opTime, systemOpTimeDictionary);
                    }
        }


        /// <summary>
        /// Updates the supplemental data load tables for each test data queue.
        /// </summary>
        /// <param name="SupplementalDataUpdateLocationDataTable"></param>
        /// <param name="SupplementalDataUpdateSystemDataTable"></param>
        /// <param name="rptPeriodId"></param>
        /// <param name="workspaceSessionId"></param>
        public void LoadIntoSupplementalDataTables(DataTable SupplementalDataUpdateLocationDataTable, DataTable SupplementalDataUpdateSystemDataTable, int rptPeriodId, decimal workspaceSessionId)
        {
            int[] indicatorList = { 0, 1 };

            cDailyCalibrationTestDataQueue testDataQueue;

            foreach (int spanScalePos in Enum.GetValues(typeof(eSpanScale)))
                foreach (int onlinePos in indicatorList)
                    foreach (int validPos in indicatorList)
                    {
                        testDataQueue = TestDataQueue[spanScalePos, onlinePos, validPos];

                        if (testDataQueue != null)
                            testDataQueue.LoadIntoSupplementalDataTables(SupplementalDataUpdateLocationDataTable, SupplementalDataUpdateSystemDataTable, rptPeriodId, workspaceSessionId);
                    }
        }

        #endregion

    }

}
