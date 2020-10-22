using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ECMPS.Checks.CheckEngine.SpecialParameterClasses
{
    /// <summary>
    /// 
    /// </summary>
    public class DailyCalibrationSupplementalValues
    {

        #region Public Constructor

        /// <summary>
        /// Initializes the supplemental data values and additional information.
        /// </summary>
        /// <param name="dailyCalibrationTestData">The time (date and hour, and optionally minutes) that the daily calibration test occurred.</param>
        public DailyCalibrationSupplementalValues(cDailyCalibrationTestData dailyCalibrationTestData)
        {
            if (dailyCalibrationTestData == null)
            {
                throw new ArgumentNullException("dailyCalibrationTestData");
            }

            DailyCalibrationTestData = dailyCalibrationTestData;

            FirstOpHourAfterLastCoveredNonOpHour = null;
            LastCoveredNonOpHour = null;
            LastHandledOpHour = null;
            OperatingHourCount = 0;
        }

        /// <summary>
        /// Initialized the supplemental data values and additional information from the previous quarter's last values.
        /// </summary>
        /// <param name="dailyCalibrationTestData"></param>
        /// <param name="firstOpHourAfterLastCoveredNonOpHour"></param>
        /// <param name="lastCoveredNonOpHour"></param>
        /// <param name="lastHandledOpHour"></param>
        /// <param name="operatingHourCount"></param>
        public DailyCalibrationSupplementalValues(cDailyCalibrationTestData dailyCalibrationTestData,
                                                  DateTime? firstOpHourAfterLastCoveredNonOpHour,
                                                  DateTime? lastCoveredNonOpHour, 
                                                  DateTime? lastHandledOpHour,
                                                  int operatingHourCount)
        {
            if (dailyCalibrationTestData == null)
            {
                throw new ArgumentNullException("dailyCalibrationTestData");
            }

            DailyCalibrationTestData = dailyCalibrationTestData;

            FirstOpHourAfterLastCoveredNonOpHour = firstOpHourAfterLastCoveredNonOpHour;
            LastCoveredNonOpHour = lastCoveredNonOpHour;
            LastHandledOpHour = lastHandledOpHour;
            OperatingHourCount = operatingHourCount;
        }

        #endregion


        #region Public Properties

        /// <summary>
        /// The cDailyCalibrationTestData object for which the supplemental values are being tracked.
        /// </summary>
        public cDailyCalibrationTestData DailyCalibrationTestData;

        /// <summary>
        /// The time (date and hour) that the daily calibration test occurred.
        /// </summary>
        public DateTime DailyTestDateHour { get { return DailyCalibrationTestData.DailyTestDateHour; } }

        /// <summary>
        /// Contains the firest operating hour that occurred after LastCoveredNonOpHour.
        /// 
        /// The value is reset to null whenever LastCoveredNonOpHour is updated.
        /// 
        /// Used in supplemental data.
        /// </summary>
        public DateTime? FirstOpHourAfterLastCoveredNonOpHour { get; private set; }

        /// <summary>
        /// Contains the last non operating hour that occurred during the hours coverred by the test.
        /// 
        /// Used in supplemental data.
        /// </summary>
        public DateTime? LastCoveredNonOpHour { get; private set; }

        /// <summary>
        /// Indicates the last hour for which potential updates to OperatingHourCount, LastCoveredNonOpHour and FirstOpHourAfterLastCoveredNonOpHour
        /// were handled.
        /// </summary>
        public DateTime? LastHandledOpHour { get; private set; }

        /// <summary>
        /// The count of operating hours on or after the the test hour.  
        /// 
        /// Used in supplemental data and should be the count from the test to the end of the quarter for any data used in supplemantal data.
        /// </summary>
        public int OperatingHourCount { get; private set; }

        #endregion


        #region Public Methods

        /// <summary>
        /// Updates OperatingHourCount, LastCoveredNonOpHour and FirstOpHourAfterLastCoveredNonOpHour for an hour.
        /// 
        ///     OperatingHourCount: Updated when the op hour is a new hour and and an operating hour.
        ///     LastCoveredNonOpHour: Update to the op hour when the hour falls within the 26 hours covered by the test starting with the test hour.
        ///     FirstOpHourAfterLastCoveredNonOpHour: When null, set to a new hour when it is operating.
        ///                                           If LastCoveredNonOpHour is updated, FirstOpHourAfterLastCoveredNonOpHour is set to null.
        /// </summary>
        /// <param name="opHour">The operating hour for which the update is occuring.</param>
        /// <param name="opTime">The operating time for the hour.</param>
        public void Update(DateTime opHour, decimal opTime)
        {
            // Only update if the op hour has not been handled.
            if ((LastHandledOpHour == null) || (opHour > LastHandledOpHour.Value))
            {
                if (opTime > 0m)
                {
                    OperatingHourCount += 1;

                    if (LastCoveredNonOpHour.HasValue && !FirstOpHourAfterLastCoveredNonOpHour.HasValue)
                        FirstOpHourAfterLastCoveredNonOpHour = opHour;
                }
                else
                {
                    TimeSpan interval = opHour - DailyTestDateHour;
                    int hourCountIncludingTestHourAndOpHour = (int)Math.Round( interval.TotalHours) + 1; // Plus one include the hour of the test in the count.

                    // The test is affective for 26 hours including the hour of the test and the non-op hour must fall within that period or in the very next hour.
                    if ((1 <= hourCountIncludingTestHourAndOpHour) && (hourCountIncludingTestHourAndOpHour <= 27))
                    {
                        LastCoveredNonOpHour = opHour;
                        FirstOpHourAfterLastCoveredNonOpHour = null;
                    }
                }
            }
        }

        #endregion

    }
}
