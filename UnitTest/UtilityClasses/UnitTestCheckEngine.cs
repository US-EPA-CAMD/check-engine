using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.TypeUtilities;


namespace UnitTest.UtilityClasses
{

    /// <summary>
    /// Instaintiates a test version of cCheckEngine.
    /// </summary>
    public class UnitTestCheckEngine : cCheckEngine
    {
        #region Now Settings
        /// <summary>
        /// Returns the current time, which testing may override.
        /// </summary>
        public override DateTime NowDateTime { get { return NowDateTimeOverride.HasValue ? NowDateTimeOverride.Value : ((cCheckEngine)this).NowDateTime; } }

        DateTime? NowDateTimeOverride = null;

        /// <summary>
        /// Sets the EvaluationBeganDate property.
        /// </summary>
        /// <param name="value">The new value for EvaluationBeganDate.</param>
        public void SetNowDateTime(DateTime? value) { NowDateTimeOverride = value; }

        #endregion


        /// <summary>
        /// Sets the EvaluationBeganDate property.
        /// </summary>
        /// <param name="value">The new value for EvaluationBeganDate.</param>
        public void SetEvaluationBeganDate(DateTime? value) { EvaluationBeganDate = value; }

        /// <summary>
        /// Sets the EvaluationEndedDate property.
        /// </summary>
        /// <param name="value">The new value for EvaluationEndedDate.</param>
        public void SetEvaluationEndedDate(DateTime? value) { EvaluationEndedDate = value; }

        /// <summary>
        /// Sets the FirstEcmpsReportingPeriod property.
        /// </summary>
        /// <param name="value">The new value for FirstEcmpsReportingPeriod.</param>
        public void SetFirstEcmpsReportingPeriod(cReportingPeriod value) { FirstEcmpsReportingPeriod = value; }

        /// <summary>
        /// Sets the FirstEcmpsReportingPeriodId property.
        /// </summary>
        /// <param name="value">The new value for FirstEcmpsReportingPeriodId.</param>
        public void SetFirstEcmpsReportingPeriodId(int? value) { FirstEcmpsReportingPeriodId = value; }

        /// <summary>
        /// Sets the EvaluationBeganDate property.
        /// </summary>
        /// <param name="value">The new value for MaximumFutureDate.</param>
        public void SetMaximumFutureDate(DateTime value) { MaximumFutureDate = value; }

        /// <summary>
        /// Sets the ReportingPeriod property.
        /// </summary>
        /// <param name="value">The new value for ReportingPeriod.</param>
        public void SetReportingPeriod(cReportingPeriod value) { ReportingPeriod = value; }
    }

}
