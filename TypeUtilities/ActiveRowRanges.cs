using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using ECMPS.Definitions.Extensions;


namespace ECMPS.Checks.TypeUtilities
{

  /// <summary>
  /// Provides methods for lists of cActiveRowHourRange.
  /// </summary>
  public class cActiveRowHourRanges : List<cActiveRowHourRange>
  {

    /// <summary>
    /// Creates a list of distinct hour ranges, with the rows 
    /// active in those ranges.
    /// </summary>
    /// <param name="view">The view to inspect.</param>
    /// <param name="dateBeganDateColumnName">The name of the began date field.</param>
    /// <param name="dateBeganHourColumnName">The name of the began hour field.</param>
    /// <param name="dateEndedDateColumnName">The name of the ended date field.</param>
    /// <param name="dateEndedHourColumnName">The name of the ended hour field.</param>
    /// <param name="compareRangeBegan">The begin date/hour of effective window of the ranges.</param>
    /// <param name="compareRangeEnded">The end date/hour of effective window of the ranges.</param>
    /// <param name="errorMessage">The error message returned on failure.</param>
    /// <returns>Returns a list of ranges or null if method fails.</returns>
    public static cActiveRowHourRanges Create(DataView view, 
                                              string dateBeganDateColumnName,
                                              string dateBeganHourColumnName,
                                              string dateEndedDateColumnName,
                                              string dateEndedHourColumnName,
                                              cOperatingHour compareRangeBegan,
                                              cOperatingHour compareRangeEnded,
                                              ref string errorMessage)
    {
      cActiveRowHourRanges result;

      if ((view == null) || (view.Table == null))
      {
        errorMessage = "View or its table Table is null.";
        result = null;
      }
      else if (!view.Table.Columns.Contains(dateBeganDateColumnName) ||
               !view.Table.Columns.Contains(dateBeganHourColumnName) ||
               !view.Table.Columns.Contains(dateEndedDateColumnName) ||
               !view.Table.Columns.Contains(dateEndedHourColumnName))
      {
        errorMessage = "Began/Ended Date/Hour column does not exist in view.";
        result = null;
      }
      else
      {
        string sort = string.Format("{0}, {1}, {2}, {3}",
                                    dateBeganDateColumnName, dateBeganHourColumnName,
                                    dateEndedDateColumnName, dateEndedHourColumnName);

        DataView orderedView = new DataView(view.Table, view.RowFilter, sort, view.RowStateFilter);

        cOperatingHourList opHours = new cOperatingHourList();

        opHours.Add(compareRangeBegan);
        opHours.Add(compareRangeEnded.AddHours(1));

        foreach (DataRowView row in orderedView)
        {
          // Update Hours list for Began Date/Hour
          {
            DateTime beganDate = row[dateBeganDateColumnName].AsDateTime().Default(DateTypes.START);
            eHour beganHour = row[dateBeganHourColumnName].AsHour().Default(DateTypes.START);
            cOperatingHour beganRowHour = new cOperatingHour(beganDate, beganHour);

            if ((beganRowHour.CompareTo(compareRangeBegan) >= 0) &&
                (beganRowHour.CompareTo(compareRangeEnded) <= 0))
            { opHours.Add(beganRowHour); }
          }


          // Update Hours list for Ended Date/Hour
          {
            DateTime endedDate = row[dateEndedDateColumnName].AsDateTime().Default(DateTypes.END);
            eHour endedHour = row[dateEndedHourColumnName].AsHour().Default(DateTypes.END);
            cOperatingHour endedRowHour = new cOperatingHour(endedDate, endedHour);

            if ((endedRowHour.CompareTo(compareRangeBegan) >= 0) &&
                (endedRowHour.CompareTo(compareRangeEnded) <= 0))
            { opHours.Add(endedRowHour.AddHours(1)); }
          }
        }


        // Create and Load cActiveRowHourRanges object
        {
          if (opHours.Count > 0)
          {
            result = new cActiveRowHourRanges();

            opHours.Sort();

            for (int dex = opHours.Count - 1; dex > 0; dex--)
            {
              cOperatingHour beganListHour = opHours[dex - 1];
              cOperatingHour endedListHour = opHours[dex].AddHours(-1);

              cActiveRowHourRange range = new cActiveRowHourRange(beganListHour, endedListHour);

              foreach (DataRowView row in orderedView)
              {
                DateTime beganDate = row[dateBeganDateColumnName].AsDateTime().Default(DateTypes.START);
                eHour beganHour = row[dateBeganHourColumnName].AsHour().Default(DateTypes.START);
                cOperatingHour beganRowHour = new cOperatingHour(beganDate, beganHour);

                DateTime endedDate = row[dateEndedDateColumnName].AsDateTime().Default(DateTypes.END);
                eHour endedHour = row[dateEndedHourColumnName].AsHour().Default(DateTypes.END);
                cOperatingHour endedRowHour = new cOperatingHour(endedDate, endedHour);


                if ((beganRowHour.CompareTo(endedListHour) <= 0) &&
                    (endedRowHour.CompareTo(beganListHour) >= 0))
                { range.Rows.Add(row); }
              }

              if (range.Rows.Count > 0)
                result.Add(range);
            }
          }
          else
          {
            errorMessage = "No active rows within active range.";
            result = null;
          }
        }
      }

      return result;
    }

    /// <summary>
    /// Creates a list of distinct hour ranges, with the rows 
    /// active in those ranges.
    /// </summary>
    /// <param name="view">The view to inspect.</param>
    /// <param name="compareRangeBegan">The begin date/hour of effective window of the ranges.</param>
    /// <param name="compareRangeEnded">The end date/hour of effective window of the ranges.</param>
    /// <param name="errorMessage">The error message returned on failure.</param>
    /// <returns>Returns a list of ranges or null if method fails.</returns>
    public static cActiveRowHourRanges Create(DataView view,
                                              cOperatingHour compareRangeBegan,
                                              cOperatingHour compareRangeEnded,
                                              ref string errorMessage)
    {
      cActiveRowHourRanges result;

      result = cActiveRowHourRanges.Create(view,
                                           "BEGIN_DATE",
                                           "BEGIN_HOUR",
                                           "END_DATE",
                                           "END_HOUR",
                                           compareRangeBegan,
                                           compareRangeEnded,
                                           ref errorMessage);

      return result;
    }

    /// <summary>
    /// Creates a list of distinct hour ranges, with the rows 
    /// active in those ranges, using datetime format for the passed dates.
    /// </summary>
    /// <param name="view">The view to inspect.</param>
    /// <param name="compareRangeBegan">The begin date/hour of effective window of the ranges in datetime format.</param>
    /// <param name="compareRangeEnded">The end date/hour of effective window of the ranges in datetime format.</param>
    /// <param name="errorMessage">The error message returned on failure.</param>
    /// <returns>Returns a list of ranges or null if method fails.</returns>
    public static cActiveRowHourRanges Create(DataView view,
                                              DateTime? compareRangeBegan,
                                              DateTime? compareRangeEnded,
                                              ref string errorMessage)
    {
        cActiveRowHourRanges result;

        result = cActiveRowHourRanges.Create(view,
                                             "BEGIN_DATE",
                                             "BEGIN_HOUR",
                                             "END_DATE",
                                             "END_HOUR",
                                             new cOperatingHour(compareRangeBegan.AsDate().Default(DateTime.MinValue), (eHour)compareRangeBegan.AsHour().Default(0)),
                                             new cOperatingHour(compareRangeEnded.AsDate().Default(DateTime.MinValue), (eHour)compareRangeEnded.AsHour().Default(0)),
                                             ref errorMessage);

        return result;
    }
  }


  /// <summary>
  /// Hold the range information for and Date/Hour range, 
  /// as well as a list of rows active during the range.
  /// </summary>
  public class cActiveRowHourRange : cOperatingHourRange
  {

    /// <summary>
    /// The base constructor.
    /// </summary>
    public cActiveRowHourRange(cOperatingHour began, cOperatingHour ended)
      : base(began, ended)
    {
      Rows = new List<DataRowView>();
    }

    /// <summary>
    /// The list of rows active during the date/hour range.
    /// </summary>
    public List<DataRowView> Rows { get; private set; }

  }

}
