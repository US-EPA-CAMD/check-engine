using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.TypeUtilities;


namespace UnitTest.UtilityClasses
{
  public static class UnitTestExtensions
  {

    /// <summary>
    /// Get a 
    /// </summary>
    /// <returns></returns>
    public static void SetReportingPeriodTable()
    {
      DataTable table = new DataTable("REPORTING_PERIOD");
      {
        table.Columns.Add("RPT_PERIOD_ID", typeof(int));
        table.Columns.Add("CALENDAR_YEAR", typeof(int));
        table.Columns.Add("QUARTER", typeof(int));
        table.Columns.Add("PERIOD_DESCRIPTION", typeof(string));
        table.Columns.Add("PERIOD_ABBREVIATION", typeof(string));
        table.Columns.Add("BEGIN_DATE", typeof(DateTime));
        table.Columns.Add("END_DATE", typeof(DateTime));
      }

      for (int year = 1993; year < DateTime.Now.Year + 20; year++)
        for (int quarter = 1; quarter <= 4; quarter++)
        {
          int rptPeriodId = 4 * (year - 1993) + quarter;

          DataRow row = table.NewRow();
          {
            row["RPT_PERIOD_ID"] = 4 * (year - 1993) + quarter; ;
            row["CALENDAR_YEAR"] = year;
            row["QUARTER"] = quarter;
            row["PERIOD_DESCRIPTION"] = String.Format("{0} QTR {1}", year, quarter);
            row["PERIOD_ABBREVIATION"] = String.Format("{0} Q{1}", year, quarter);
            row["BEGIN_DATE"] = new DateTime(year, 3 * (quarter - 1) + 1, 1);
            row["END_DATE"] = new DateTime(year, 3 * (quarter - 1) + 1, 1).AddMonths(3).AddDays(-1);
          }

          table.Rows.Add(row);
        }

      cDateFunctions.ReportingPeriodTable = table;
      cReportingPeriod.LookupTable = table;
    }

    /// <summary>
    /// Sets the EvaluationBeganDate property.
    /// </summary>
    /// <param name="value">The new value for EvaluationBeganDate.</param>
    public static void SetEvaluationBeganDate(this cCheckEngine checkEngine, DateTime? value) { ((UnitTestCheckEngine)checkEngine).SetEvaluationBeganDate(value); }

    /// <summary>
    /// Sets the EvaluationEndedDate property.
    /// </summary>
    /// <param name="value">The new value for EvaluationEndedDate.</param>
    public static void SetEvaluationEndedDate(this cCheckEngine checkEngine, DateTime? value) { ((UnitTestCheckEngine)checkEngine).SetEvaluationEndedDate(value); }

    /// <summary>
    /// Sets the FirstEcmpsReportingPeriod property.
    /// </summary>
    /// <param name="value">The new value for FirstEcmpsReportingPeriod.</param>
    public static void SetFirstEcmpsReportingPeriod(this cCheckEngine checkEngine, cReportingPeriod value) { ((UnitTestCheckEngine)checkEngine).SetFirstEcmpsReportingPeriod(value); }

    /// <summary>
    /// Sets the FirstEcmpsReportingPeriodId property.
    /// </summary>
    /// <param name="value">The new value for FirstEcmpsReportingPeriodId.</param>
    public static void SetFirstEcmpsReportingPeriodId(this cCheckEngine checkEngine, int? value) { ((UnitTestCheckEngine)checkEngine).SetFirstEcmpsReportingPeriodId(value); }

    /// <summary>
    /// Sets the EvaluationBeganDate property.
    /// </summary>
    /// <param name="value">The new value for MaximumFutureDate.</param>
    public static void SetMaximumFutureDate(this cCheckEngine checkEngine, DateTime? value) { ((UnitTestCheckEngine)checkEngine).SetMaximumFutureDate(value); }

    /// <summary>
    /// Sets the ReportingPeriod property.
    /// </summary>
    /// <param name="value">The new value for ReportingPeriod.</param>
    public static void SetReportingPeriod(this cCheckEngine checkEngine, cReportingPeriod value) { ((UnitTestCheckEngine)checkEngine).SetReportingPeriod(value); }

  }
}
