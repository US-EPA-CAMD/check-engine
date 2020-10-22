using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECMPS.Checks.TypeUtilities
{

  #region Date List and Range Objects

  /// <summary>
  /// Implements additional methods for a List of cOperatingHour
  /// </summary>
  public class cDateTimeList : List<DateTime?>
  {

    /// <summary>
    /// Creates a cDateTimeList.
    /// </summary>
    public cDateTimeList()
    {
    }

    /// <summary>
    /// Creates a cDateTimeList from a DateTime array.
    /// </summary>
    /// <param name="dateTimeArray">The DateTime array.</param>
    public cDateTimeList(DateTime[] dateTimeArray)
    {
      foreach (DateTime dateTime in dateTimeArray)
      {
        if (!Contains(dateTime))
        {
          Add(dateTime);
        }
      }
    }


    /// <summary>
    /// Creates a cDateTimeList from a DateTime array.
    /// </summary>
    /// <param name="dateTimeArray">The DateTime array.</param>
    public cDateTimeList(DateTime?[] dateTimeArray)
    {
      foreach (DateTime? dateTime in dateTimeArray)
      {
        if (!Contains(dateTime))
        {
          Add(dateTime);
        }
      }
    }

    /// <summary>
    /// Adds the items in the DateTime array.
    /// </summary>
    /// <param name="dateTimeArray">The array of datetimes.</param>
    public void Add(DateTime[] dateTimeArray)
    {
      foreach (DateTime dateTime in dateTimeArray)
      {
        if (!Contains(dateTime))
        {
          Add(dateTime);
        }
      }
    }


    /// <summary>
    /// Adds the items in the DateTime array.
    /// </summary>
    /// <param name="dateTimeArray">The array of datetimes.</param>
    public void Add(DateTime?[] dateTimeArray)
    {
      foreach (DateTime? dateTime in dateTimeArray)
      {
        if (!Contains(dateTime))
        {
          Add(dateTime);
        }
      }
    }

    /// <summary>
    /// Adds the date with time as the passed hour
    /// </summary>
    /// <param name="date">The date to use.</param>
    /// <param name="hour">The hour to use as the time.</param>
    public void Add(DateTime date, int hour)
    {
      DateTime dateTime = new DateTime(date.Year, date.Month, date.Day, hour, 0, 0);

      if (!Contains(dateTime))
      {
        Add(dateTime);
      }
    }

  }


  /// <summary>
  /// Represents a datetime range.
  /// </summary>
  public class cDateTimeRange : IEquatable<cOperatingDateRange>
  {

    #region Public Constructors

    /// <summary>
    /// The base constructor.
    /// </summary>
    public cDateTimeRange(DateTime? began, DateTime? ended)
    {
      Began = began;
      Ended = ended;
    }

    #endregion


    #region Public Properties

    /// <summary>
    /// The beginning date of the range.
    /// </summary>
    public DateTime? Began { get; private set; }

    /// <summary>
    /// The ending date of the range.
    /// </summary>
    public DateTime? Ended { get; private set; }

    /// <summary>
    /// An object for any users purpose.
    /// </summary>
    public object Tag { get; set; }

    #endregion


    #region Public Methods

    /// <summary>
    /// Returns the number of days in the range including the end dates.
    /// </summary>
    /// <returns></returns>
    public int DaysInRange()
    {
      int result;

      DateTime began;
      {
        if (Began.HasValue)
          began = Began.Value.Date;
        else
          began = DateTime.MinValue;
      }

      DateTime ended;
      {
        if (Ended.HasValue)
          ended = Ended.Value.Date;
        else
          ended = DateTime.MaxValue;
      }

      TimeSpan timeSpan = new TimeSpan(ended.Ticks - began.Ticks);

      result = timeSpan.Days + 1;

      return result;
    }

    /// <summary>
    /// Returns the number of hours in the range including the end hours.
    /// </summary>
    /// <returns></returns>
    public int HoursInRange()
    {
      int result;

      DateTime began;
      {
        if (Began.HasValue)
          began = new DateTime(Began.Value.Year, Began.Value.Month, Began.Value.Day, Began.Value.Hour, 0, 0);
        else
          began = new DateTime(DateTime.MinValue.Year, DateTime.MinValue.Month, DateTime.MinValue.Day, 0, 0, 0);
      }

      DateTime ended;
      {
        if (Ended.HasValue)
          ended = new DateTime(Ended.Value.Year, Ended.Value.Month, Ended.Value.Day, Ended.Value.Hour, 0, 0);
        else
          ended = new DateTime(DateTime.MaxValue.Year, DateTime.MaxValue.Month, DateTime.MaxValue.Day, 23, 0, 0);
      }

      TimeSpan timeSpan = new TimeSpan(ended.Ticks - began.Ticks);

      result = timeSpan.Hours + 1;

      return result;
    }

    #endregion


    #region Public Methods: Interface Implementations

    /// <summary>
    /// Determines whether this DateTime has the same value as the passed DateTime
    /// </summary>
    /// <param name="other">The DateTime to which this cOperatingHour is compared.</param>
    /// <returns>0: Equal, 1: Greater than other, -1:Less than other </returns>
    public bool Equals(cOperatingDateRange other)
    {
      bool result = true;

      if ((this.Began == null) || (other.Began == null))
        result = result && ((this.Began == null) && (other.Began == null));
      else
        result = result && this.Began.Equals(other.Began);

      if ((this.Ended == null) || (other.Ended == null))
        result = result && ((this.Ended == null) && (other.Ended == null));
      else
        result = result && this.Ended.Equals(other.Ended);

      return result;
    }

    #endregion

}


  /// <summary>
  /// Provides a list of cDateRange objects.
  /// </summary>
  public class cDateTimeRanges : List<cDateTimeRange>
  {

    /// <summary>
    /// Tag element used to store any information needed by the user.
    /// </summary>
    public object Tag { get; set; }

    /// <summary>
    /// Adds a cDateTimeRange object for a non existing Began/Ended date combination.
    /// </summary>
    /// <param name="began">Began date of range.</param>
    /// <param name="ended">Ended date of range</param>
    /// <returns>True if range is unique and was inserted.</returns>
    public bool AddUnique(DateTime? began, DateTime? ended)
    {
      bool result;

      cDateTimeRange dateTimeRange = new cDateTimeRange(began, ended);

      if (!Contains(dateTimeRange))
      {
        Add(dateTimeRange);
        result = true;
      }
      else
        result = false;

      return result;
    }

  }


  /// <summary>
  /// Provides a list of cDateTimeRanges objects.
  /// </summary>
  public class cDateTimeRangesList : List<cDateTimeRanges>
  {
  }

  #endregion


  #region Old and Revisit

  /// <summary>
  /// An enumeration of hours
  /// </summary>
  public enum eHour
  {
    /// <summary>
    /// 12am
    /// </summary>
    h00,

    /// <summary>
    /// 1am
    /// </summary>
    h01,

    /// <summary>
    /// 2am
    /// </summary>
    h02,

    /// <summary>
    /// 3am
    /// </summary>
    h03,

    /// <summary>
    /// 4am
    /// </summary>
    h04,

    /// <summary>
    /// 5am
    /// </summary>
    h05,

    /// <summary>
    /// 6am
    /// </summary>
    h06,

    /// <summary>
    /// 7am
    /// </summary>
    h07,

    /// <summary>
    /// 8am
    /// </summary>
    h08,

    /// <summary>
    /// 9am
    /// </summary>
    h09,

    /// <summary>
    /// 10am
    /// </summary>
    h10,

    /// <summary>
    /// 11am
    /// </summary>
    h11,

    /// <summary>
    /// 12pm
    /// </summary>
    h12,

    /// <summary>
    /// 1pm
    /// </summary>
    h13,

    /// <summary>
    /// 2pm
    /// </summary>
    h14,

    /// <summary>
    /// 3pm
    /// </summary>
    h15,

    /// <summary>
    /// 4pm
    /// </summary>
    h16,

    /// <summary>
    /// 5pm
    /// </summary>
    h17,

    /// <summary>
    /// 6pm
    /// </summary>
    h18,

    /// <summary>
    /// 7pm
    /// </summary>
    h19,

    /// <summary>
    /// 8pm
    /// </summary>
    h20,

    /// <summary>
    /// 9pm
    /// </summary>
    h21,

    /// <summary>
    /// 10pm
    /// </summary>
    h22,

    /// <summary>
    /// 11pm
    /// </summary>
    h23
  }


  /// <summary>
  /// An enumeration of quarters
  /// </summary>
  public enum eQuarter
  {
    /// <summary>
    /// Q1
    /// </summary>
    q1 = 1,

    /// <summary>
    /// Q2
    /// </summary>
    q2 = 2,

    /// <summary>
    /// Q3
    /// </summary>
    q3 = 3,

    /// <summary>
    /// Q4
    /// </summary>
    q4 = 4
  }


  /// <summary>
  /// Holds the date and hour parts of an operating hour.
  /// </summary>
  public class cOperatingHour : IComparable<cOperatingHour>, IEquatable<cOperatingHour>
  {

    #region Public Constructors

    /// <summary>
    /// Constructor for an Operating hour.
    /// </summary>
    /// <param name="date">The initial date of the operating hour.</param>
    /// <param name="hour">The initial hour of the operating hour.</param>
    public cOperatingHour(DateTime date, eHour hour)
      : base()
    {
      Date = date;
      Hour = hour;
    }

    /// <summary>
    /// Constructor for an Operating hour.
    /// </summary>
    public cOperatingHour(cOperatingHour operatingHour)
      : base()
    {
      Date = operatingHour.Date;
      Hour = operatingHour.Hour;
    }

    #endregion


    #region Public Properties

    /// <summary>
    /// The date part of the operating hour
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// The hour part of the operating hour
    /// </summary>
    public eHour Hour { get; set; }

    #endregion


    #region Public Override Methods

    /// <summary>
    /// Returns a string representation of the object.
    /// </summary>
    /// <returns>The string representation of the object.</returns>
    public override string ToString()
    {
      string result;

      result = string.Format("{0}-{1}", Date.ToShortDateString(), Hour.AsInteger());

      return result;
    }

    #endregion


    #region Public Methods

    /// <summary>
    /// Returns an operating hour with the hours shifted by the designated amount.
    /// </summary>
    /// <param name="shift">The number of hours to add.</param>
    /// <returns>Returns a copy of the updated operating hour.</returns>
    public cOperatingHour AddHours(int shift)
    {
      cOperatingHour result;

      int dayShift, hour;

      dayShift = (shift + Hour.AsInteger()) / 24;
      hour = (shift + Hour.AsInteger()) % 24;

      if (hour < 0)
      {
        dayShift -= 1;
        hour += 24;
      }

      DateTime Limit = ((dayShift >= 0) ? DateTime.MaxValue : DateTime.MinValue);

      if (Math.Abs(Limit.Subtract(Date.Date).Days) >= Math.Abs(dayShift))
        result = new cOperatingHour(Date.Date.AddDays(dayShift), (eHour)hour);
      else
        result = new cOperatingHour(Limit.Date, ((dayShift >= 0) ? eHour.h23 : eHour.h00));

      return result;
    }

    /// <summary>
    /// Returns an operating hour with the days shifted by the designated amount.
    /// </summary>
    /// <param name="shift">The number of days to add.</param>
    /// <returns>Returns a copy of the updated operating hour.</returns>
    public cOperatingHour AddDays(int shift)
    {
      cOperatingHour result;

      DateTime Limit = ((shift >= 0) ? DateTime.MaxValue : DateTime.MinValue);

      if (Math.Abs(Limit.Subtract(Date.Date).Days) >= Math.Abs(shift))
        result = new cOperatingHour(Date.Date.AddDays(shift), Hour);
      else
        result = new cOperatingHour(Limit.Date, ((shift >= 0) ? eHour.h23 : eHour.h00));

      return result;
    }

    #endregion


    #region Public Methods: Interface Implementations

    /// <summary>
    /// Compare this cOperatingHour to the passed cOperater
    /// </summary>
    /// <param name="other">The cOperatingHour to which this cOperatingHour is compared.</param>
    /// <returns>0: Equal, 1: Greater than other, -1:Less than other </returns>
    public int CompareTo(cOperatingHour other)
    {
      int result;

      if (other != null)
      {
        result = Date.CompareTo(other.Date);

        if (result == 0)
          result = Hour.CompareTo(other.Hour);
      }
      else
        result = -2;

      return result;
    }

    /// <summary>
    /// Determines whether this cOperatingHour has the same value as the passed cOperater
    /// </summary>
    /// <param name="other">The cOperatingHour to which this cOperatingHour is compared.</param>
    /// <returns>0: Equal, 1: Greater than other, -1:Less than other </returns>
    public bool Equals(cOperatingHour other)
    {
      bool result;

      result = (other != null) && (this.Date == other.Date) && (this.Hour == other.Hour);

      return result;
    }

    #endregion

  }


  /// <summary>
  /// Represents a date range.
  /// </summary>
  public class cOperatingDateRange : IEquatable<cOperatingDateRange>
  {

    /// <summary>
    /// The base constructor.
    /// </summary>
    public cOperatingDateRange(DateTime? began, DateTime? ended)
    {
      Began = began;
      Ended = ended;
    }

    /// <summary>
    /// The beginning date of the range.
    /// </summary>
    public DateTime? Began { get; private set; }

    /// <summary>
    /// The ending date of the range.
    /// </summary>
    public DateTime? Ended { get; private set; }

    /// <summary>
    /// An object for any users purpose.
    /// </summary>
    public object Tag { get; set; }


    #region Public Methods: Interface Implementations

    /// <summary>
    /// Determines whether this DateTime has the same value as the passed DateTime
    /// </summary>
    /// <param name="other">The DateTime to which this cOperatingHour is compared.</param>
    /// <returns>0: Equal, 1: Greater than other, -1:Less than other </returns>
    public bool Equals(cOperatingDateRange other)
    {
      bool result = true;

      if ((this.Began == null) || (other.Began == null))
        result = result && ((this.Began == null) && (other.Began == null));
      else
        result = result && this.Began.Equals(other.Began);

      if ((this.Ended == null) || (other.Ended == null))
        result = result && ((this.Ended == null) && (other.Ended == null));
      else
        result = result && this.Ended.Equals(other.Ended);

      return result;
    }

    #endregion

  }


  /// <summary>
  /// Provides a list of cDateRange objects.
  /// </summary>
  public class cOperatingDateRanges : List<cOperatingDateRange>
  {

    /// <summary>
    /// Adds a COperatingDateRange object for a non existing Began/Ended date combination.
    /// </summary>
    /// <param name="began">Began date of range.</param>
    /// <param name="ended">Ended date of range</param>
    /// <returns>True if range is unique and was inserted.</returns>
    public bool AddUnique(DateTime? began, DateTime? ended)
    {
      bool result;

      cOperatingDateRange operatingDateRange = new cOperatingDateRange(began, ended);

      if (!Contains(operatingDateRange))
      {
        Add(operatingDateRange);
        result = true;
      }
      else
        result = false;

      return result;
    }

  }


  /// <summary>
  /// Provides a list of cDateRange objects.
  /// </summary>
  public class cOperatingDateRangesList : List<cOperatingDateRanges>
  {
  }


  /// <summary>
  /// Implements additional methods for a List of cOperatingHour
  /// </summary>
  public class cOperatingHourList : List<cOperatingHour>
  {

    /// <summary>
    /// Creates a cOperatingHour and adds it to the list.
    /// </summary>
    /// <param name="date">The date for the cOperatingHour.</param>
    /// <param name="hour">The hour for the cOperatingHour.</param>
    public void Add(DateTime date, eHour hour)
    {
      cOperatingHour operatingHour = new cOperatingHour(date, hour);

      Add(operatingHour);
    }

  }


  /// <summary>
  /// Represents a date range.
  /// </summary>
  public class cOperatingHourRange : IEquatable<cOperatingHourRange>
  {

    #region Public Constructors

    /// <summary>
    /// The base constructor
    /// </summary>
    /// <param name="began">The operating hour the range began.</param>
    /// <param name="ended">The operating hour the range ended.</param>
    public cOperatingHourRange(cOperatingHour began, cOperatingHour ended)
    {
      if (began != null)
        Began = new cOperatingHour(began.Date, began.Hour);
      else
        Began = null;

      if (ended != null)
        Ended = new cOperatingHour(ended.Date, ended.Hour);
      else
        Ended = null;
    }

    /// <summary>
    /// The base constructor
    /// </summary>
    /// <param name="operatingHourRange">The operating hour range.</param>
    public cOperatingHourRange(cOperatingHourRange operatingHourRange)
    {
      if (operatingHourRange.Began != null)
        Began = new cOperatingHour(operatingHourRange.Began);
      else
        Began = null;

      if (operatingHourRange.Ended != null)
        Ended = new cOperatingHour(operatingHourRange.Ended);
      else
        Ended = null;
    }

    #endregion

    /// <summary>
    /// The beginning date/hour of the range.
    /// </summary>
    public cOperatingHour Began { get; private set; }

    /// <summary>
    /// The ending date/hour of the range.
    /// </summary>
    public cOperatingHour Ended { get; private set; }

    /// <summary>
    /// An object for any users purpose.
    /// </summary>
    public object Tag { get; set; }

    /// <summary>
    /// The length of the range.
    /// </summary>
    public int? Length
    {
      get
      {
        int? result;

        if ((Began == null) || (Ended == null))
          result = null;
        else
        {
          TimeSpan span = new TimeSpan(Ended.Date.Ticks - Began.Date.Ticks);
          result = 24 * span.Days + (Ended.Hour.AsInteger() - Began.Hour.AsInteger() + 1);
        }

        return result;
      }
    }


    #region Public Methods: Interface Implementations

    /// <summary>
    /// Determines whether this cOperatingHour has the same value as the passed cOperater
    /// </summary>
    /// <param name="other">The cOperatingHour to which this cOperatingHour is compared.</param>
    /// <returns>0: Equal, 1: Greater than other, -1:Less than other </returns>
    public bool Equals(cOperatingHourRange other)
    {
      bool result = true;

      if ((this.Began == null) || (other.Began == null))
        result = result && ((this.Began == null) && (other.Began == null));
      else
        result = result && this.Began.Equals(other.Began);

      if ((this.Ended == null) || (other.Ended == null))
        result = result && ((this.Ended == null) && (other.Ended == null));
      else
        result = result && this.Ended.Equals(other.Ended);

      return result;
    }

    #endregion

  }


  /// <summary>
  /// Provides a list of cDateRange objects.
  /// </summary>
  public class cOperatingHourRanges : List<cOperatingHourRange>
  {

    #region Public Methods

    /// <summary>
    /// Adds an operating hour reange for the passed range of operating hours.
    /// </summary>
    /// <param name="began">The beginning operating hour of the range.</param>
    /// <param name="ended">The ending operating hour of the range.</param>
    public void Add(cOperatingHour began, cOperatingHour ended)
    {
      Add(new cOperatingHourRange(began, ended));
    }

    #endregion

  }


  /// <summary>
  /// Provides a list of cHourange objects.
  /// </summary>
  public class cOperatingHourRangesList : List<cOperatingHourRanges>
  {
  }

  #endregion

}
