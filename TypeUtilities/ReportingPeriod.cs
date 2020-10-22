using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using ECMPS.Checks.DatabaseAccess;

using ECMPS.Definitions.Extensions;


namespace ECMPS.Checks.TypeUtilities
{
 
  /// <summary>
  /// Holds the date and hour parts of an operating hour.
  /// </summary>
  public class cReportingPeriod
  {

    #region Public Constructors

    /// <summary>
    /// Create an object to hold reporting period information.
    /// </summary>
    /// <param name="rptPeriodId">The reporting period id for which to create the object.</param>
    public cReportingPeriod(int rptPeriodId)
      : base()
    {
      DataRow rptPeriodRow = LookupRow(rptPeriodId);

      if ((rptPeriodRow != null) && rptPeriodRow["CALENDAR_YEAR"].HasDbValue()
                                 && rptPeriodRow["QUARTER"].HasDbValue())
      {
        RptPeriodId = rptPeriodId;

        Year = rptPeriodRow["CALENDAR_YEAR"].AsInteger().Default();
        Quarter = (eQuarter)rptPeriodRow["QUARTER"].AsInteger().Default();
        Description = rptPeriodRow["PERIOD_DESCRIPTION"].AsString();
        Abbreviation = rptPeriodRow["PERIOD_ABBREVIATION"].AsString();
        BeganDate = rptPeriodRow["BEGIN_DATE"].AsDateTime().Default(DateTypes.START);
        EndedDate = rptPeriodRow["END_DATE"].AsDateTime().Default(DateTypes.END);

        BeganMonth = BeganDate.Month;
        EndedMonth = EndedDate.Month;
      }
      else
      {
        throw new System.ArgumentException(string.Format("Reporting Period ({0}) does not exist", rptPeriodId), "original");
      }
    }
    
    /// <summary>
    /// Create an object to hold reporting period information.
    /// </summary>
    /// <param name="rptPeriodId"></param>
    /// <param name="year"></param>
    /// <param name="quarter"></param>
    /// <param name="description"></param>
    /// <param name="abbreviation"></param>
    /// <param name="beganDate"></param>
    /// <param name="endedDate"></param>
    public cReportingPeriod(int? rptPeriodId,
                            int? year, int? quarter,
                            string description, string abbreviation,
                            DateTime? beganDate, DateTime? endedDate)
    {
      RptPeriodId = rptPeriodId.Default();

      Year = year.Default();
      Quarter = quarter.AsQuarter().Value;
      Description = description;
      Abbreviation = abbreviation;
      BeganDate = beganDate.Default(DateTypes.START);
      EndedDate = endedDate.Default(DateTypes.END);

      BeganMonth = BeganDate.Month;
      EndedMonth = EndedDate.Month;
    }

    /// <summary>
    /// Create an object to hold reporting period information.
    /// </summary>
    /// <param name="dateTime">The date/time for which to create the object.</param>
    public cReportingPeriod(DateTime dateTime)
      : base()
    {
      DataRow rptPeriodRow = LookupRow(dateTime.Year, (dateTime.Month - 1) / 3 + 1 );

      if ((rptPeriodRow != null) && rptPeriodRow["CALENDAR_YEAR"].HasDbValue()
                                 && rptPeriodRow["QUARTER"].HasDbValue())
      {
        RptPeriodId = rptPeriodRow["RPT_PERIOD_ID"].AsInteger().Default();

        Year = rptPeriodRow["CALENDAR_YEAR"].AsInteger().Default();
        Quarter = (eQuarter)rptPeriodRow["QUARTER"].AsInteger().Default();
        Description = rptPeriodRow["PERIOD_DESCRIPTION"].AsString();
        Abbreviation = rptPeriodRow["PERIOD_ABBREVIATION"].AsString();
        BeganDate = rptPeriodRow["BEGIN_DATE"].AsDateTime().Default(DateTypes.START);
        EndedDate = rptPeriodRow["END_DATE"].AsDateTime().Default(DateTypes.END);

        BeganMonth = BeganDate.Month;
        EndedMonth = EndedDate.Month;
      }
      else
      {
        throw new System.ArgumentException(string.Format("Date ({0}) does not have a Reporting Period", dateTime), "original");
      }
    }

    /// <summary>
    /// Create an object to hold reporting period information.
    /// </summary>
    /// <param name="year">Year of reporting period.</param>
    /// <param name="quarter">Quarter of reporting period.</param>
    public cReportingPeriod(int year, int quarter)
    {
      DataRow rptPeriodRow = LookupRow(year, quarter);

      if ((rptPeriodRow != null) && rptPeriodRow["CALENDAR_YEAR"].HasDbValue()
                                 && rptPeriodRow["QUARTER"].HasDbValue())
      {
        RptPeriodId = rptPeriodRow["RPT_PERIOD_ID"].AsInteger().Default();

        Year = rptPeriodRow["CALENDAR_YEAR"].AsInteger().Default();
        Quarter = (eQuarter)rptPeriodRow["QUARTER"].AsInteger().Default();
        Description = rptPeriodRow["PERIOD_DESCRIPTION"].AsString();
        Abbreviation = rptPeriodRow["PERIOD_ABBREVIATION"].AsString();
        BeganDate = rptPeriodRow["BEGIN_DATE"].AsDateTime().Default(DateTypes.START);
        EndedDate = rptPeriodRow["END_DATE"].AsDateTime().Default(DateTypes.END);

        BeganMonth = BeganDate.Month;
        EndedMonth = EndedDate.Month;
      }
      else
      {
        throw new System.ArgumentException(string.Format("Quarter ({0}q{1}) does not have a Reporting Period", year, quarter), "original");
      }
    }

    #endregion


    #region Private Constructors

    /// <summary>
    /// Constructor for an operating quarter.
    /// </summary>
    /// <param name="rptPeriodRow">A reporting period data row.</param>
    private cReportingPeriod(DataRow rptPeriodRow)
      : base()
    {
      if ((rptPeriodRow != null) && rptPeriodRow["Calendar_Year"].HasDbValue()
                                 && rptPeriodRow["Quarter"].HasDbValue())
      {
        RptPeriodId = rptPeriodRow["Rpt_Period_Id"].AsInteger().Default();

        Year = rptPeriodRow["Calendar_Year"].AsInteger().Default();
        Quarter = (eQuarter)rptPeriodRow["Quarter"].AsInteger().Default();

        BeganMonth = 3 * (Quarter.AsInteger() - 1) + 1;
        EndedMonth = 3 * Quarter.AsInteger();

        BeganDate = new DateTime(Year, BeganMonth, 1);
        EndedDate = new DateTime(Year, EndedMonth, 1).AddMonths(1).AddDays(-1); ;
      }
      else
      {
        throw new System.ArgumentException("Reporting Period does not exist", "original");
      }
    }

    #endregion


    #region Public Static Properties and Methods

    /// <summary>
    /// Returns a reporting period for the passed RPT_PERIOD_ID.
    /// </summary>
    /// <param name="rptPeriodId">The report period id.</param>
    /// <returns>The reporting period for the passed year and quarter.</returns>
    public static cReportingPeriod GetReportingPeriod(int rptPeriodId)
    {
      cReportingPeriod result;

      DataRow lookupRow = LookupRow(rptPeriodId);

      if (lookupRow != null)
      {
        try
        {
          result = new cReportingPeriod(lookupRow);
        }
        catch
        {
          result = null;
        }
      }
      else
        result = null;

      return result;
    }

    /// <summary>
    /// Returns a reporting period for the passed year and quarter.
    /// </summary>
    /// <param name="year">The year for which to return a quarter.</param>
    /// <param name="quarter">The quarter for which to return a quarter.</param>
    /// <returns>The reporting period for the passed year and quarter.</returns>
    public static cReportingPeriod GetReportingPeriod(int year, int quarter)
    {
      cReportingPeriod result;

      DataRow lookupRow = LookupRow(year, quarter);

      if (lookupRow != null)
      {
        try
        {
          result = new cReportingPeriod(lookupRow);
        }
        catch
        {
          result = null;
        }
      }
      else
        result = null;

      return result;
    }

    /// <summary>
    /// Returns a reporting period for the passed year and quarter.
    /// </summary>
    /// <param name="year">The year for which to return a quarter.</param>
    /// <param name="quarter">The quarter for which to return a quarter.</param>
    /// <returns>The reporting period for the passed year and quarter.</returns>
    public static cReportingPeriod GetReportingPeriod(int year, eQuarter quarter)
    {
      cReportingPeriod result;

      result = cReportingPeriod.GetReportingPeriod(year, quarter.AsInteger());

      return result;
    }

    /// <summary>
    /// Get the start date of the quarter realitive to the passed in reporting period
    /// </summary>
    /// <param name="rptPeriodId">The reference reporting period</param>
    /// <returns>System.DateTime containing the start date of this quarter</returns>
    public static DataRow LookupRow(int rptPeriodId)
    {
      DataRow result = null;

      if (LookupTable != null)
      {
        foreach (DataRow lookupRow in LookupTable.Rows)
        {
          if (lookupRow["RPT_PERIOD_ID"].AsInteger() == rptPeriodId)
          {
            result = lookupRow;
            break;
          }
        }
      }

      return result;
    }

    /// <summary>
    /// Get the start date of the quarter realitive to the passed in reporting period
    /// </summary>
    /// <param name="year">The year for which to return a quarter.</param>
    /// <param name="quarter">The quarter for which to return a quarter.</param>
    /// <returns>System.DateTime containing the start date of this quarter</returns>
    public static DataRow LookupRow(int year, int quarter)
    {
      DataRow result = null;

      if (LookupTable != null)
      {
        foreach (DataRow lookupRow in LookupTable.Rows)
        {
          if ((lookupRow["CALENDAR_YEAR"].AsInteger() == year) &&
              (lookupRow["QUARTER"].AsInteger() == quarter))
          {
            result = lookupRow;
            break;
          }
        }
      }

      return result;
    }

    /// <summary>
    /// Returns the current Reporting Period Table
    /// </summary>
    public static DataTable LookupTable
    {
      get
      {
        if (FLookupTable == null)
        {
          try
          {
            cDatabase Database = cDatabase.GetConnection(cDatabase.eCatalog.DATA, "cDateFunctions");

            FLookupTable = Database.GetDataTable("select * from Reporting_Period order by Calendar_Year, Quarter");
          }
          catch
          {
            FLookupTable = null;
          }
        }

        return FLookupTable;
      }

      set
      {
        FLookupTable = value;
      }
    }


    #region Property Fields

    /// <summary>
    /// Contains the lookup table used to read reporting period information.
    /// </summary>
    protected static DataTable FLookupTable = null;

    #endregion

    #endregion


    #region Public Properties

    /// <summary>
    /// The period abbreviation
    /// </summary>
    public string Abbreviation { get; private set; }

    /// <summary>
    /// The began date of the reporting period.
    /// </summary>
    public DateTime BeganDate { get; private set; }

    /// <summary>
    /// The began date of the reporting period.
    /// </summary>
    public int BeganMonth { get; private set; }

    /// <summary>
    /// The period description.
    /// </summary>
    public string Description { get; private set; }

    /// <summary>
    /// The ended date of the reporting period.
    /// </summary>
    public DateTime EndedDate { get; private set; }

    /// <summary>
    /// The ended date of the reporting period.
    /// </summary>
    public int EndedMonth { get; private set; }

    /// <summary>
    /// The Reporting Period Id for the Reporting Period.
    /// </summary>
    public int RptPeriodId { get; private set; }

    /// <summary>
    /// The hour part of the operating hour
    /// </summary>
    public eQuarter Quarter { get; private set; }

    /// <summary>
    /// The date part of the operating hour
    /// </summary>
    public int Year { get; private set; }


    #endregion


    #region Public Methods

    /// <summary>
    /// Adds a quarter to an operating quarter.
    /// </summary>
    /// <param name="shift">The number of quaters to add.</param>
    /// <returns>The updated operating quarter.</returns>
    public cReportingPeriod AddQuarter(int shift)
    {
      int yearShift, quarter;

      yearShift = ((shift + Quarter.AsInteger() - 1) / 4);
      quarter = ((shift + Quarter.AsInteger() - 1) % 4) + 1;

      if (quarter < 1)
      {
        yearShift -= 1;
        quarter += 4;
      }

      cReportingPeriod result = cReportingPeriod.GetReportingPeriod(Year + yearShift, (eQuarter)quarter);

      return result;
    }

    /// <summary>
    /// Adds a year to an operating quater.
    /// </summary>
    /// <param name="shift">The number of years to add.</param>
    /// <returns>The updated operating quarter.</returns>
    public cReportingPeriod AddYear(int shift)
    {
      Year += shift;

      cReportingPeriod result = cReportingPeriod.GetReportingPeriod(Year + shift, Quarter);

      return result;
    }

    /// <summary>
    /// Returns a string representation of the object.
    /// </summary>
    /// <returns>A string representation of the object.</returns>
    public override string ToString()
    {
      string result;

      result = string.Format("{0}q{1} ({2})", Year, Quarter.AsInteger(), RptPeriodId);

      return result;
    }

    #endregion


    #region Public Methods: Interface Implementations

    /// <summary>
    /// Compare this cOperatingHour to the passed cOperater
    /// </summary>
    /// <param name="other">The cOperatingHour to which this cOperatingHour is compared.</param>
    /// <returns>0: Equal, 1: Greater than other, -1:Less than other </returns>
    public int CompareTo(cReportingPeriod other)
    {
      int result;

      result = Year.CompareTo(other.Year);

      if (result == 0)
        result = Quarter.CompareTo(other.Quarter);

      return result;
    }

    /// <summary>
    /// Determines whether this cOperatingHour has the same value as the passed cOperater
    /// </summary>
    /// <param name="other">The cOperatingHour to which this cOperatingHour is compared.</param>
    /// <returns>0: Equal, 1: Greater than other, -1:Less than other </returns>
    public bool Equals(cReportingPeriod other)
    {
      bool result;

      result = (this.Year == other.Year) && (this.Quarter == other.Quarter);

      return result;
    }

    #endregion

  }

}
