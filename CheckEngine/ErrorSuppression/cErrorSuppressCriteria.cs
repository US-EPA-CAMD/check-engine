using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECMPS.Definitions.Extensions;
using ECMPS.Definitions.SeverityCode;


namespace ECMPS.ErrorSuppression
{

  /// <summary>
  /// The class used to contain and execute individual Error Suppression.
  /// </summary>
  public class cErrorSuppressCriteria
  {

    #region Public Constructors

    /// <summary>
    /// Creates an Error Suppression object.
    /// </summary>
    /// <param name="esSpecId">The key to the ES Spec row.</param>
    /// <param name="checkCatalogResultId">The key to the suppressed Check Catalog Result.</param>
    /// <param name="severityCd">The new Severity Code for the suppression.</param>
    /// <param name="facId">The Facility Id on which to apply the suppression.</param>
    /// <param name="locationNameList">A comma delimited list of location names to which the suppression applies.</param>
    /// <param name="esMatchDataTypeCd">The Match Data Type.</param>
    /// <param name="matchDataValue">The Match Data Value.</param>
    /// <param name="esMatchTimeTypeCd">The Match Time Type.</param>
    /// <param name="matchHistoricalInd">The Match Time Historical Indicator.</param>
    /// <param name="matchTimeBeginValue">The Match Time Begin Value.</param>
    /// <param name="matchTimeEndValue">The Match Time End Value.</param>
    public cErrorSuppressCriteria(long esSpecId, long checkCatalogResultId, eSeverityCd? severityCd,
                                  long? facId, string locationNameList,
                                  string esMatchDataTypeCd, string matchDataValue,
                                  string esMatchTimeTypeCd, bool? matchHistoricalInd, DateTime? matchTimeBeginValue, DateTime? matchTimeEndValue)
    {
      EsSpecId = esSpecId;
      CheckCatalogResultId = checkCatalogResultId;
      SeverityCd = severityCd;

      FacId = facId;
      LocationNameList = locationNameList;

      EsMatchDataTypeCd = esMatchDataTypeCd;
      MatchDataValue = matchDataValue;

      EsMatchTimeTypeCd = esMatchTimeTypeCd;
      MatchHistoricalInd = matchHistoricalInd;
      MatchTimeBeginValue = matchTimeBeginValue;
      MatchTimeEndValue = matchTimeEndValue;
    }

    #endregion


    #region Public Methods

    /// <summary>
    /// Evaluates the passed data to determine the new severity code.
    /// </summary>
    /// <param name="errorSuppressValues">The Error Suppression Values to evaluate.</param>
    /// <returns></returns>
    public eSeverityCd? Evaluate(cErrorSuppressValues errorSuppressValues)
    {
      eSeverityCd? result = null;

      bool match = true;

      if (FacId.HasValue && (errorSuppressValues.FacId != FacId.Value))
      {
        match = false;
      }
      else if (LocationNameList.HasValue() && 
               (!errorSuppressValues.LocationName.HasValue() ||
                errorSuppressValues.LocationName.ToUpper().NotInList(LocationNameList.ToUpper())))
      {
        match = false;
      }
      else if ((EsMatchDataTypeCd != null) && (MatchDataValue != null) &&
          ((errorSuppressValues.EsMatchDataTypeCd != EsMatchDataTypeCd) ||
           (errorSuppressValues.MatchDataValue != MatchDataValue)))
      {
        match = false;
      }

      else if ((EsMatchTimeTypeCd == "HISTIND") && (MatchHistoricalInd != null) &&
               ((errorSuppressValues.EsMatchTimeTypeCd != EsMatchTimeTypeCd) ||
                (errorSuppressValues.MatchTimeValue.HasValue != MatchHistoricalInd)))
      {
        match = false;
      }
      else if ((EsMatchTimeTypeCd != null) && (EsMatchTimeTypeCd != "HISTIND") &&
               ((MatchTimeBeginValue != null) || (MatchTimeEndValue != null)) &&
               ((errorSuppressValues.EsMatchTimeTypeCd != EsMatchTimeTypeCd) ||
                errorSuppressValues.MatchTimeValue.NotBetween(MatchTimeBeginValue, MatchTimeEndValue)))
      {
        match = false;
      }

      if (match)
        result = SeverityCd;
      else
        result = null;

      return result;
    }

    #endregion


    #region Public Properties

    /// <summary>
    /// The key to the suppressed Check Catalog Result.
    /// </summary>
    public long CheckCatalogResultId { get; private set; }

    /// <summary>
    /// The Match Data Type.
    /// </summary>
    public string EsMatchDataTypeCd { get; private set; }

    /// <summary>
    /// The Match Time Type.
    /// </summary>
    public string EsMatchTimeTypeCd { get; private set; }

    /// <summary>
    /// The key to the ES Spec row.
    /// </summary>
    public long EsSpecId { get; private set; }

    /// <summary>
    /// The Facility Id on which to apply the suppression.
    /// 
    /// This should be null if the suppression applies to all facilities.
    /// </summary>
    public long? FacId { get; private set; }

    /// <summary>
    /// A comma delimited list of location names to which the suppression applies.
    /// 
    /// This should be null if the suppression does not apply to specific location, 
    /// and will be null if the Facility Id is null.
    /// </summary>
    public string LocationNameList { get; private set; }

    /// <summary>
    /// The Match Data Value.
    /// </summary>
    public string MatchDataValue { get; private set; }

    /// <summary>
    /// The Match Time Historical Indicator.
    /// </summary>
    public bool? MatchHistoricalInd { get; private set; }

    /// <summary>
    /// The Match Time Begin Value.
    /// </summary>
    public DateTime? MatchTimeBeginValue { get; private set; }

    /// <summary>
    /// The Match Time End Value.
    /// </summary>
    public DateTime? MatchTimeEndValue { get; private set; }

    /// <summary>
    /// The new Severity Code for the suppression.
    /// </summary>
    public eSeverityCd? SeverityCd { get; private set; }

    #endregion

  }
}
