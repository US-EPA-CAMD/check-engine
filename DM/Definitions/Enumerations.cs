using System;
using System.Data;

using ECMPS.DM.Utilities;

namespace ECMPS.DM.Definitions
{

  /// <summary>
  /// Apportionment Type Enumeration
  /// </summary>
  public enum eApportionmentType
  {

    #region Automatically Handled

    /// <summary>
    /// One or more common pipes with more than one unit 
    /// and each pipe attached to each unit.
    /// </summary>
    CommonPipe,

    /// <summary>
    /// One or more common pipes with LTFF methods attached to more than one unit.
    /// </summary>
    CommonPipeLtff,

    /// <summary>
    /// One or more common stacks with more than one unit 
    /// and each stack attached to each unit.
    /// </summary>
    CommonStack,

    /// <summary>
    /// One or more common stacks and one or more common pipes with more than one unit 
    /// and each stack and each pipe attached to each unit.
    /// </summary>
    CommonStackAndPipe,

    /// <summary>
    /// More than one multiple stack attached to a single unit.
    /// </summary>
    MultipleStack,

    /// <summary>
    /// Unit level.
    /// </summary>
    Unit,

    #endregion

    #region Formula Handled

    /// <summary>
    /// Configuration changed during reporting period.
    /// </summary>
    Changed,

    /// <summary>
    /// Apportionment must be handled by an apportionment spec
    /// </summary>
    Complex,

    #endregion

    #region Error or Obsolete

    /// <summary>
    /// More than one multiple pipe attached to a single unit. (obsolete)
    /// </summary>
    MultiplePipe,

    /// <summary>
    /// Multiple pipe in configuration but not a multiple pipe configuration (obsolete)
    /// </summary>
    MultiplePipeInvolved,

    /// <summary>
    /// Error processing configuration
    /// </summary>
    Error

    #endregion

  };

  /// <summary>
  /// Complex Apportionment Parameters
  /// </summary>
  public enum eComplexParameter
  {

    /// <summary>
    /// SO2 Mass
    /// </summary>
    So2m,

    /// <summary>
    /// CO2 Mass
    /// </summary>
    Co2m,

    /// <summary>
    /// NOx Mass
    /// </summary>
    Noxm,

    /// <summary>
    /// Hg Mass
    /// </summary>
    Hgm,

    /// <summary>
    /// HCl Mass
    /// </summary>
    Hclm,

    /// <summary>
    /// HF Mass
    /// </summary>
    Hfm

  }

}
