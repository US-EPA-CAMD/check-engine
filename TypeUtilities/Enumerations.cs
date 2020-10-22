using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECMPS.Checks.TypeUtilities
{

  /// <summary>
  /// Span Scale
  /// </summary>
  public enum eSpanScale
  {
    /// <summary>
    /// Span Scale Code of 'H'
    /// </summary>
    High,

    /// <summary>
    /// Span Scale Code of 'L'
    /// </summary>
    Low,

    /// <summary>
    /// No span scale specified
    /// </summary>
    None
  }

  /// <summary>
  /// Enumeration of DataMart (AMPD) Emission Parameters
  /// </summary>
  public enum eHourMeasureParameter
  {

    /// <summary>
    /// CO2 Concentration
    /// </summary>
    Co2c,

    /// <summary>
    /// Density
    /// </summary>
    Density,

    /// <summary>
    /// Fuel Flow
    /// </summary>
    Ff,

    /// <summary>
    /// Flow
    /// </summary>
    Flow,

    /// <summary>
    /// GCV
    /// </summary>
    Gcv,

    /// <summary>
    /// H2O
    /// </summary>
    H2o,

    /// <summary>
    /// NOX Concentration
    /// </summary>
    Noxc,

    /// <summary>
    /// NOX Rate
    /// </summary>
    Noxr,

    /// <summary>
    /// O2 Dry
    /// </summary>
    O2d,

    /// <summary>
    /// O2 Wet
    /// </summary>
    O2w,

    /// <summary>
    /// SO2 Concentration
    /// </summary>
    So2c,

    /// <summary>
    /// Sulfer
    /// </summary>
    Sulfer

  }

}
