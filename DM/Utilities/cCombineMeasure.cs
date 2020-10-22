using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECMPS.Definitions.Extensions;


namespace ECMPS.DM.Utilities
{

  /// <summary>
  /// Used to combine measured values.
  /// </summary>
  public class cCombineMeasure
  {

    #region Public Properties

    /// <summary>
    /// Indicates whether there are any 'calculated' measure flags to combine.
    /// </summary>
    public bool CalculatedFound { get; private set; }

    /// <summary>
    /// Indicates whether there are any 'lme' measure flags to combine.
    /// </summary>
    public bool LmeFound { get; private set; }

    /// <summary>
    /// Indicates whether there are any 'measured' measure flags to combine.
    /// </summary>
    public bool MeasuredFound { get; private set; }

    /// <summary>
    /// Indicates whether there are any 'measured and substitute' measure flags to combine.
    /// </summary>
    public bool MeasureAndSubstituteFound { get; private set; }

    /// <summary>
    /// Indicates whether there are any 'not applicable' measure flags to combine.
    /// </summary>
    public bool OtherFound { get; private set; }

    /// <summary>
    /// The current combination of the applied measure codes.
    /// </summary>
    public string Result { get { return GetCombined(); } }

    /// <summary>
    /// Indicates whether there are any 'substitute' measure flags to combine.
    /// </summary>
    public bool SubstituteFound { get; private set; }

    /// <summary>
    /// Indicates whether theare are any 'unavailable' (MATS) measure flags to combine.
    /// </summary>
    public bool UnavailableFound { get; private set; }

    /// <summary>
    /// Indicates whether theare are any 'startup/shutdown' (MATS) measure flags to combine.
    /// </summary>
    public bool UpDownFound { get; private set; }

    #endregion


    #region Public Methods

    /// <summary>
    /// Combines the measure flag into the previously combined flags.
    /// </summary>
    /// <param name="measure">The flag to combine.</param>
    public void Combine(string measure)
    {
      if (measure.HasValue())
      {
        switch (measure)
        {
          case "MEASURE": MeasuredFound = true; break;
          case "SUB": SubstituteFound = true; break;
          case "CALC": CalculatedFound = true; break;
          case "LME": LmeFound = true; break;
          case "MEASSUB": MeasureAndSubstituteFound = true; break;
          case "OTHER": OtherFound = true; break;
          case "UNAVAIL": UnavailableFound = true; break;
          case "UPDOWN": UpDownFound = true; break;
        }
      }
    }

    /// <summary>
    /// Combines the measure flag into the previously combined flags
    /// and returns the new combined flag.
    /// </summary>
    /// <param name="measure">The flag to combine.</param>
    /// <returns>The new combined value.</returns>
    public string CombineAndGet(string measure)
    {
      string result;

      Combine(measure);

      result = GetCombined();

      return result;
    }

    /// <summary>
    /// Combines the measure flag into the previously combined flags
    /// and returns the new combined flag.
    /// </summary>
    /// <param name="measure">The flag to combine.</param>
    /// <param name="combinedMeasure">The new combined value.</param>
    public void CombineAndSet(string measure, out string combinedMeasure)
    {
      Combine(measure);
      combinedMeasure = GetCombined();
    }

    /// <summary>
    /// Returns the combined flag based on the passed booleans.
    /// 
    /// MATS Unavailable trumps all else with Startup/Shutdown trumping all others.
    /// </summary>
    /// <param name="measuredFound">True if 'measured' flag found.</param>
    /// <param name="substituteFound">True if 'substitute' flag found.</param>
    /// <param name="calculatedFound">True if 'measured' flag found.</param>
    /// <param name="lmeFound">True if 'lme' flag found.</param>
    /// <param name="measureAndSubstituteFound">True if 'measured and substitute' flag found.</param>
    /// <param name="otherFound">True if 'other' flag found.</param>
    /// <param name="unavailableFound">True if 'unavailable' flag found.</param>
    /// <param name="upDownFound">True if 'startup/shutdown' flag found.</param>
    /// <returns></returns>
    public string GetCombined(bool measuredFound,
                              bool substituteFound,
                              bool calculatedFound,
                              bool lmeFound,
                              bool measureAndSubstituteFound, 
                              bool otherFound,
                              bool unavailableFound,
                              bool upDownFound)
    {
      string result;

      if (unavailableFound)
        result = "UNAVAIL";
      else if (upDownFound)
        result = "UPDOWN";
      else if (measuredFound && !substituteFound && !calculatedFound && !lmeFound && !measureAndSubstituteFound)
        result = "MEASURE";
      else if (!measuredFound && substituteFound && !calculatedFound && !lmeFound && !measureAndSubstituteFound)
        result = "SUB";
      else if (!measuredFound && !substituteFound && calculatedFound && !lmeFound && !measureAndSubstituteFound)
        result = "CALC";
      else if (!measuredFound && !substituteFound && !calculatedFound && lmeFound && !measureAndSubstituteFound)
        result = "LME";
      else if (measureAndSubstituteFound && !lmeFound)
        result = "MEASSUB";
      else if ((measuredFound || calculatedFound) && substituteFound && !lmeFound)
        result = "MEASSUB";
      else if (otherFound || measuredFound || substituteFound || calculatedFound || lmeFound || measureAndSubstituteFound)
        result = "OTHER";
      else
        result = null;

      return result;
    }

    /// <summary>
    /// Returns the current combined measure flag for the object.
    /// </summary>
    /// <returns>The current combined measure flag.</returns>
    public string GetCombined()
    {
      string result;

      result = GetCombined(MeasuredFound,
                           SubstituteFound,
                           CalculatedFound,
                           LmeFound,
                           MeasureAndSubstituteFound,
                           OtherFound,
                           UnavailableFound,
                           UpDownFound);

      return result;
    }

    /// <summary>
    /// Returns the combined value that would result when the passed measure is included
    /// but does not include the measure.
    /// </summary>
    /// <param name="measure">The flag to combine.</param>
    /// <returns>The potential combined measure flag.</returns>
    public string Preview(string measure)
    {
      string result;

      if (measure.HasValue())
      {
        bool measuredFound = MeasuredFound || (measure == "MEASURE");
        bool substituteFound = SubstituteFound || (measure == "SUB");
        bool calculatedFound = CalculatedFound || (measure == "CALC");
        bool lmeFound = LmeFound || (measure == "LME");
        bool measureAndSubstituteFound = MeasureAndSubstituteFound || (measure == "MEASSUB");
        bool otherFound = OtherFound || (measure == "OTHER");
        bool unavailableFound = UnavailableFound || (measure == "UNAVAIL");
        bool upDownFound = UpDownFound || (measure == "UPDOWN");

        result = GetCombined(measuredFound,
                             substituteFound,
                             calculatedFound,
                             lmeFound,
                             measureAndSubstituteFound, 
                             otherFound,
                             unavailableFound,
                             upDownFound);
      }
      else
        result = GetCombined();

      return result;
    }

    #endregion

  }

}
