using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECMPS.Definitions.Extensions;


namespace ECMPS.ErrorSuppression
{

  /// <summary>
  /// This is the base class for the Error Suppression Values classes.
  /// </summary>
  public class cErrorSuppressValues
  {

    #region Public Constructors

    /// <summary>
    /// The constructor for the Error Suppression Values Class.
    /// </summary>
    /// <param name="facId">The Facility Id of the data being checked.</param>
    /// <param name="locationName">The Location Name of the data being checked.</param>
    /// <param name="esMatchDataTypeCd">The Match Data Type.</param>
    /// <param name="matchDataValue">The Match Data Value.</param>
    /// <param name="esMatchTimeTypeCd">The Match Time Type.</param>
    /// <param name="matchTimeValue">The Match Time Value.</param>
    public cErrorSuppressValues(long facId, string locationName,
                                string esMatchDataTypeCd, string matchDataValue,
                                string esMatchTimeTypeCd, DateTime? matchTimeValue)
    {
      if (matchDataValue.HasValue() && esMatchDataTypeCd.IsEmpty())
      {
        throw new ArgumentNullException("esMatchDataTypeCd", "Match Data Value specified without a Match Data Type Code.");
      }
      else if (matchTimeValue.HasValue() && esMatchTimeTypeCd.IsEmpty())
      {
        throw new ArgumentNullException("esMatchTimeTypeCd", "Match Time Value specified without a Match Time Type Code.");
      }
      else
      {
        FacId = facId;
        LocationName = locationName;
        EsMatchDataTypeCd = esMatchDataTypeCd;
        MatchDataValue = matchDataValue;
        EsMatchTimeTypeCd = esMatchTimeTypeCd;
        MatchTimeValue = matchTimeValue;
      }
    }

    #endregion


    #region Public Properties

    /// <summary>
    /// The Match Data Type.
    /// </summary>
    public string EsMatchDataTypeCd { get; private set; }

    /// <summary>
    /// The Match Time Type.
    /// </summary>
    public string EsMatchTimeTypeCd { get; private set; }

    /// <summary>
    /// The Facility Id of the data being checked.
    /// </summary>
    public long FacId { get; private set; }

    /// <summary>
    /// The Location Name of the data being checked.
    /// </summary>
    public string LocationName { get; private set; }

    /// <summary>
    /// The Match Data Value.
    /// </summary>
    public string MatchDataValue { get; private set; }

    /// <summary>
    /// The Match Time Value.
    /// </summary>
    public DateTime? MatchTimeValue { get; private set; }

    #endregion

  }

}
