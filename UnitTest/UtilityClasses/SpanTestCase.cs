using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnitTest.UtilityClasses
{

  public enum SpanCaseType { Spans, NotActive, GapBefore, GapBetween, GapAfter };

  public enum SpanPeriod { Hour, Day };

  public class SpanTestCase
  {

    public SpanTestCase(DateTime begin, DateTime end, SpanPeriod spanPeriod, SpanCaseType spanCaseType)
    {
      CaseType = spanCaseType;

      NormalizeDates(spanPeriod, ref begin, ref end);

      if (end >= begin)
      {
        long delta;

        switch (spanPeriod)
        {
          case SpanPeriod.Day: delta = (new TimeSpan(end.Ticks - begin.Ticks)).Days; break;
          case SpanPeriod.Hour: delta = (new TimeSpan(end.Ticks - begin.Ticks)).Days; break;
          default: delta = 0; break;
        }

        int factor = (spanPeriod == SpanPeriod.Day ? 24 : 1);

        switch (spanCaseType)
        {
          case SpanCaseType.Spans:
            {
              BeginRange = new DateTimeRange(begin, begin.AddHours(factor * (int)(delta / 2)), spanPeriod);
              EndRange = new DateTimeRange(BeginRange.End.Value.AddHours(factor), end, spanPeriod);
            }
            break;

          case SpanCaseType.NotActive:
            {
              BeginRange = new DateTimeRange(begin.AddHours(-2 * factor), begin.AddHours(-1 * factor), spanPeriod);
              EndRange = new DateTimeRange(end.AddHours(1 * factor), end.AddHours(2 * factor), spanPeriod);
            }
            break;

          case SpanCaseType.GapBefore:
            {
              BeginRange = new DateTimeRange(begin.AddHours(factor), begin.AddHours(factor * (int)((delta - 1) / 2)), spanPeriod);
              EndRange = new DateTimeRange(BeginRange.End.Value.AddHours(factor), end, spanPeriod);
            }
            break;

          case SpanCaseType.GapBetween:
            {
              BeginRange = new DateTimeRange(begin, begin.AddHours(factor * (int)((delta - 1) / 2)), spanPeriod);
              EndRange = new DateTimeRange(BeginRange.End.Value.AddHours(factor * 2), end, spanPeriod);
            }
            break;

          case SpanCaseType.GapAfter:
            {
              EndRange = new DateTimeRange(end.AddHours(-1 * factor * (int)((delta - 1) / 2)), end.AddHours(-1 * factor), spanPeriod);
              BeginRange = new DateTimeRange(begin, EndRange.Begin.Value.AddHours(-1 * factor), spanPeriod);
            }
            break;

          default:
            {
              BeginRange = new DateTimeRange();
              EndRange = new DateTimeRange();
            }
            break;
        }
      }
      else
      {
        BeginRange = new DateTimeRange();
        EndRange = new DateTimeRange();
      }
    }

    static private void NormalizeDates(SpanPeriod spanPeriod, ref DateTime begin, ref DateTime end)
    {
      if (spanPeriod == SpanPeriod.Day)
      {
        begin = begin.Date;
        end = end.Date;
      }
      else
      {
        begin = begin.Date.AddHours(begin.Hour);
        end = end.Date.AddHours(end.Hour);
      }
    }

    public DateTimeRange BeginRange { get; private set; }
    public DateTimeRange EndRange { get; private set; }
    public SpanCaseType CaseType { get; private set; }

    public override string ToString()
    {
      string result;

      result = string.Format("{0} | {1} | {2}", BeginRange, EndRange, CaseType);

      return result;
    }

    #region Types and Enumerations

    public class DateTimeRange
    {
      public DateTimeRange()
      {
        Begin = null;
        End = null;
      }

      public DateTimeRange(DateTime? begin, DateTime? end, SpanPeriod spanPeriod)
      {
        Begin = begin;
        End = end;
        SpanPeriod = spanPeriod;
      }


      public DateTime? Begin { get; set; }
      public DateTime? End { get; set; }
      SpanPeriod SpanPeriod { get; set; }

      public override string ToString()
      {
        string result;

        string format;
        {
          if (Begin.HasValue && End.HasValue)
            format = (SpanPeriod == SpanPeriod.Hour) ? "{0}-{1} .. {2}-{3}" : "{0} .. {2}";
          else if (Begin.HasValue)
            format = (SpanPeriod == SpanPeriod.Hour) ? "{0}-{1} .. " : "{0} .. ";
          else if (End.HasValue)
            format = (SpanPeriod == SpanPeriod.Hour) ? " .. {2}-{3}" : " .. {2}";
          else
            format = "*";
        }

        result = string.Format(format,
                               Begin.HasValue ? Begin.Value.Date.ToShortDateString() : null,
                               Begin.HasValue ? Begin.Value.Hour : (int?)null,
                               End.HasValue ? End.Value.Date.ToShortDateString() : null,
                               End.HasValue ? End.Value.Hour : (int?)null);

        return result;
      }
    }

    #endregion

  }

}
