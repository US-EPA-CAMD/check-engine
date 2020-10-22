using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using ECMPS.Checks.TypeUtilities;

using ECMPS.Common;
using ECMPS.Definitions.Extensions;
using ECMPS.Definitions.SeverityCode;


namespace ECMPS.Checks.EmissionsReport
{

  #region Public Supporting Declarations

  /// <summary>
  /// Delegate used to determine whether to check whether a particular Daily Calibration row
  /// is the last Daily Calibration row.
  /// </summary>
  /// <param name="dailyCalibrationRow">The row to check.</param>
  /// <returns>Returns true to indicate to check the row.</returns>
  public delegate bool dLastDailyCalConditionCheck(DataRowView dailyCalibrationRow);

  /// <summary>
  /// Delegate used to determine the date and hour to use for logging results.
  /// </summary>
  /// <param name="dailyCalibrationRow">The row to check.</param>
  /// <returns>Returns the date or null if one is not determined.</returns>
  public delegate DateTime? dLastDailyCalLogDateHour(DataRowView dailyCalibrationRow);

  #endregion


  #region Internal Supporting Declarations

  /// <summary>
  /// The component level data for Daily Calibration tests.
  /// </summary>
  internal class cLastDailyCalComponent
  {

    #region Public Constructor

    /// <summary>
    /// Constructor for cDailyCalibrationComponentData.
    /// </summary>
    /// <param name="componentId">The Component Id represented by the object.</param>
    /// <param name="emissionsCheckParameters">The Emissions Report check parameters object</param>
    public cLastDailyCalComponent(string componentId)
    {
      ComponentId = componentId;

      SpanScaleTest = new cLastDailyCalTest[3,2];

      SpanScaleTest[0, 0] = null;
      SpanScaleTest[0, 1] = null;
      SpanScaleTest[1, 0] = null;
      SpanScaleTest[1, 1] = null;
      SpanScaleTest[2, 0] = null;
      SpanScaleTest[2, 1] = null;
    }

    #endregion


    #region Public Properties

    /// <summary>
    /// The Component Id for this set of daily calibration data.
    /// </summary>
    public string ComponentId { get; private set; }

    /// <summary>
    /// The set of important test data for a daily calibration.
    /// </summary>
    public cLastDailyCalTest[,] SpanScaleTest { get; private set; }

    #endregion


    #region Public Method

    /// <summary>
    /// Replaces the current Daily Cal Test for the specific component
    /// and for the same span scale fo the test.  The method checks
    /// the component id and identifier of the test against the current
    /// values for the object to make sure they are the same.
    /// </summary>
    /// <param name="dailyCalibrationRow">The row to add.</param>
    /// <param name="opDateHour">Alternate log date hour for the current row.</param>
    /// <param name="resultMessage">Message indicating why the addition failed.</param>
    /// <returns>Returns true if the replacement occurred.</returns>
    public bool Add(DataRowView dailyCalibrationRow, DateTime? opDateHour, ref string resultMessage)
    {
      bool result;

      if (cLastDailyCalibration.IsDailyCalibrationRow(dailyCalibrationRow))
      {
        string componentId = dailyCalibrationRow["COMPONENT_ID"].AsString();
        eSpanScale spanScale = cLastDailyCalibration.GetSpanScaleEnum(dailyCalibrationRow["SPAN_SCALE_CD"].AsString());

        if (componentId == ComponentId)
        {
          SpanScaleTest[(int)spanScale, 1] = SpanScaleTest[(int)spanScale, 0];
          SpanScaleTest[(int)spanScale, 0] = new cLastDailyCalTest(dailyCalibrationRow, opDateHour);
          result = true;
        }
        else
        {
          resultMessage = "cLastDailyCalTest.Add() passed inconsistent Component Id or Identifier";
          result = false;
        }
      }
      else
      {
        resultMessage = "cLastDailyCalTest constructor passed non Daily Cal row";
        result = false;
      }

      return result;
    }



    /// <summary>
    /// Retrieves the most recent test matching the passed span scale.
    /// </summary>
    /// <param name="spanScaleCd">The span scale code to lookup.</param>
    /// <returns>The test data or null if it was not found.</returns>
    public cLastDailyCalTest Get(DateTime? opDateHour, string spanScaleCd)
    {
      cLastDailyCalTest result;

      eSpanScale spanScale = cLastDailyCalibration.GetSpanScaleEnum(spanScaleCd);

      if (opDateHour.IsNull())
        result = null;
      else if ((SpanScaleTest[(int)spanScale, 0].HasValue()) &&
               (SpanScaleTest[(int)spanScale, 0].OpDateHour < opDateHour.Value))
        result = SpanScaleTest[(int)spanScale, 0];
      else if ((SpanScaleTest[(int)spanScale, 1].HasValue()) &&
               (SpanScaleTest[(int)spanScale, 1].OpDateHour < opDateHour.Value))
        result = SpanScaleTest[(int)spanScale, 1];
      else
        result = null;

      return result;
    }

    #endregion

  }


  /// <summary>
  /// Holds the Daily Test Date, Hour and Minute 
  /// as well as the Daily Calibration Row.
  /// </summary>
  internal class cLastDailyCalTest
  {

    #region Public Constructor

    /// <summary>
    /// Creates an object contain test information for cLastDailyCalibration
    /// </summary>
    /// <param name="dailyCalibrationRow">The row to add.</param>
    /// <param name="opDateHour">Alternate log date hour for the current row.</param>
    public cLastDailyCalTest(DataRowView dailyCalibrationRow, DateTime? opDateHour)
    {
      if (cLastDailyCalibration.IsDailyCalibrationRow(dailyCalibrationRow))
      {
        DateTime? dailyTestDate = dailyCalibrationRow["DAILY_TEST_DATE"].AsDateTime();
        int? dailyTestHour = dailyCalibrationRow["DAILY_TEST_HOUR"].AsInteger();
        int? dailyTestMinute = dailyCalibrationRow["DAILY_TEST_MIN"].AsInteger();

        if (dailyTestDate.HasValue && dailyTestHour.HasValue && dailyTestMinute.HasValue)
        {
          DailyTestDate = dailyCalibrationRow["DAILY_TEST_DATE"].AsDateTime().Default();
          DailyTestHour = dailyCalibrationRow["DAILY_TEST_HOUR"].AsInteger().Default();
          DailyTestMinute = dailyCalibrationRow["DAILY_TEST_MIN"].AsInteger().Default();
          DailyCalibrationRow = dailyCalibrationRow;
          OpDateHour = opDateHour;
        }
        else
        {
          Exception exception = new Exception("cLastDailyCalTest constructor passed null Daily Test Date, Hour or Minute");
          throw exception;
        }
      }
      else
      {
        Exception exception = new Exception("cLastDailyCalTest constructor passed non Daily Cal row");
        throw exception;
      }
    }

    #endregion


    #region Public Properties

    /// <summary>
    /// The daily calibration row used to derive row related properties.
    /// </summary>
    public DataRowView DailyCalibrationRow { get; protected set; }

    /// <summary>
    /// The Daily Test Date for this set of daily calibration data.
    /// </summary>
    public DateTime DailyTestDate { get; protected set; }

    /// <summary>
    /// The Daily Test Hour for this set of daily calibration data.
    /// </summary>
    public int DailyTestHour { get; protected set; }

    /// <summary>
    /// The Daily Test Minute for this set of daily calibration data.
    /// </summary>
    public int DailyTestMinute { get; protected set; }

    /// <summary>
    /// The date/hour under which to log the calibration.
    /// </summary>
    public DateTime? OpDateHour { get; protected set; }

    #endregion

  }

  #endregion


  /// <summary>
  /// Tracks and returns the last Daily Calibration matching particular conditions
  /// </summary>
  public class cLastDailyCalibration
  {

    #region Public Constructors

    /// <summary>
    /// Creates an object used to track and return the last Daily Cal record
    /// that matches particular conditions.
    /// </summary>
    /// <param name="lastDailyCalCondition">The method called to test conditions for a row.</param>
    public cLastDailyCalibration(dLastDailyCalConditionCheck lastDailyCalCondition,
                                 dLastDailyCalLogDateHour lastDailyCalLogDateHour)
    {
      ComponentDictionary = new Dictionary<string, cLastDailyCalComponent>();

      if (lastDailyCalCondition != null)
        LastDailyCalConditionCheck = lastDailyCalCondition;
      else
        LastDailyCalConditionCheck = DefaultDailyCalCondition;

      if (lastDailyCalLogDateHour != null)
        LastDailyCalLogDateHour = lastDailyCalLogDateHour;
      else
        LastDailyCalLogDateHour = DefaultDailyCalLogDateTime;
    }

    #endregion


    #region Static Public Methods

    /// <summary>
    /// Returns the eSpanScale value for the passed span scale code,
    /// returning null if the value is null or not a span scale code.
    /// </summary>
    /// <param name="spanScaleCd">The span scale code.</param>
    /// <returns>The corresponding enumeration value or eSpanScale.None if no match exists.</returns>
    static public eSpanScale GetSpanScaleEnum(string spanScaleCd)
    {
      eSpanScale result;

      switch (spanScaleCd)
      {
        case "H": result = eSpanScale.High; break;
        case "L": result = eSpanScale.Low; break;
        default: result = eSpanScale.None; break;
      }

      return result;
    }

    /// <summary>
    /// Returns true if the row is a Daily Cal row.
    /// </summary>
    /// <param name="dailyCalibrationRow">The row to test.</param>
    /// <returns>Returns true if the row is Daily Cal row.</returns>
    static public bool IsDailyCalibrationRow(DataRowView dailyCalibrationRow)
    {
      bool result;

      if ((dailyCalibrationRow != null) &&
          (dailyCalibrationRow.Row != null) &&
          (dailyCalibrationRow.Row.Table != null))
      {
        DataColumnCollection columns = dailyCalibrationRow.Row.Table.Columns;

        result = columns.Contains("TEST_RESULT_CD") &&
                 columns.Contains("COMPONENT_ID") &&
                 columns.Contains("COMPONENT_IDENTIFIER") &&
                 columns.Contains("SPAN_SCALE_CD") &&
                 columns.Contains("ONLINE_OFFLINE_IND") &&
                 columns.Contains("DAILY_TEST_DATE") &&
                 columns.Contains("DAILY_TEST_HOUR") &&
                 columns.Contains("DAILY_TEST_MIN");
      }
      else
      {
        result = false;
      }

      return result;
    }

    #endregion


    #region Public Methods

    /// <summary>
    /// Adds the current row based on component id and span scale,
    /// limited by the condition set on instantiation.
    /// </summary>
    /// <param name="dailyCalibrationRow">The row to add.</param>
    /// <param name="resultMessage">Reason for failure.</param>
    /// <returns>True if addition was successful.</returns>
    public bool Add(DataRowView dailyCalibrationRow, ref string resultMessage)
    {
      bool result;

      if (!IsDailyCalibrationRow(dailyCalibrationRow))
      {
        resultMessage = "cLastDailyCalibration.Add() passed non Daily Cal row";
        result = false;
      }
      else if (!LastDailyCalConditionCheck(dailyCalibrationRow))
      {
        result = true;
      }
      else if (dailyCalibrationRow["COMPONENT_ID"].IsDbNull())
      {
        resultMessage = "cLastDailyCalibration.Add() passed null COMPONENT_ID";
        result = false;
      }
      else
      {
        string componentId = dailyCalibrationRow["COMPONENT_ID"].AsString();

        cLastDailyCalComponent component;
        {
          if (ComponentDictionary.ContainsKey(componentId))
          {
            component = ComponentDictionary[componentId];
          }
          else
          {
            component = new cLastDailyCalComponent(componentId);
            ComponentDictionary.Add(componentId, component);
          }
        }

        DateTime? logDateHour = LastDailyCalLogDateHour(dailyCalibrationRow);

        component.Add(dailyCalibrationRow, logDateHour, ref resultMessage);

        result = true;
      }

      return result;
    }


    /// <summary>
    /// Returns the last Daily Cal row for the component id and 
    /// </summary>
    /// <param name="componentId"></param>
    /// <param name="spanScaleCd"></param>
    /// <param name="opDateHour"></param>
    /// <param name="dailyCalibrationRow"></param>
    /// <returns></returns>
    public bool Get(string componentId, string spanScaleCd, DateTime? opDateHour, out DataRowView dailyCalibrationRow)
    {
      bool result;

      if (ComponentDictionary.ContainsKey(componentId))
      {
        cLastDailyCalTest test = ComponentDictionary[componentId].Get(opDateHour, spanScaleCd);

        if (test != null)
        {
          dailyCalibrationRow = test.DailyCalibrationRow;
          result = true;
        }
        else
        {
          dailyCalibrationRow = null;
          result = false;
        }
      }
      else
      {
        dailyCalibrationRow = null;
        result = false;
      }

      return result;
    }

    #endregion


    #region Protected Methods

    /// <summary>
    /// Default Last Daily Cal Condition method, which always returns true.
    /// </summary>
    /// <param name="dailyCalibrationRow">The row to test.</param>
    /// <returns>Returns true if the row is not null.</returns>
    protected bool DefaultDailyCalCondition(DataRowView dailyCalibrationRow)
    {
      return true;
    }


    /// <summary>
    /// Default Last Daily Cal Condition method, which always returns true.
    /// </summary>
    /// <param name="dailyCalibrationRow">The row to test.</param>
    /// <returns>Returns true if the row is not null.</returns>
    protected DateTime? DefaultDailyCalLogDateTime(DataRowView dailyCalibrationRow)
    {
      return null;
    }

    #endregion


    #region Private Properties

    /// <summary>
    /// Dictionary with a Component Id lookup to cDailyCalibrationComponentData object.
    /// </summary>
    private Dictionary<string, cLastDailyCalComponent> ComponentDictionary { get; set; }

    /// <summary>
    /// The delegate called to determine whether to test a Daily Cal row 
    /// is the most recent Daily Cal Row.
    /// </summary>
    private dLastDailyCalConditionCheck LastDailyCalConditionCheck { get; set; }

    /// <summary>
    /// The delegate called to determine the date and hour to use for logging results.
    /// </summary>
    private dLastDailyCalLogDateHour LastDailyCalLogDateHour { get; set; }

    #endregion

  }

}
