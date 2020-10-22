using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using ECMPS.DM.Definitions;
using ECMPS.DM.HourlyEmissions;
using ECMPS.DM.Miscellaneous;
using ECMPS.DM.Utilities;

using ECMPS.Definitions.Extensions;


namespace ECMPS.DM
{

  /// <summary>
  /// Used to update DataMart (AMPD) Emissions for an Emission Report
  /// </summary>
  public class cUpdateEmissions
  {

    #region Public Constructors

    /// <summary>
    /// Creates a cUpdateEmissions object.
    /// </summary>
    /// <param name="updateInit">The delegate initialize processing.</param>
    /// <param name="updateSuccess">The delegate used to update after successful apportionment.</param>
    /// <param name="updateFailure">The delegate used to update after apportionment failure.</param>
    /// <param name="getFactorFormulaeArray"></param>
    /// <param name="logError">The delegate called to log errors.</param>
    /// <param name="userId">The user id to use in updated rows.</param>
    public cUpdateEmissions(dUpdateInit updateInit,
                            dUpdateSuccess updateSuccess,
                            dUpdateFailure updateFailure,
                            dGetFactorFormulaeArray getFactorFormulaeArray,
                            dLogError logError, 
                            string userId)
    {
      if (logError != null)
      {
        cErrorHandler.Initialize(logError);

        UpdateInit = updateInit;
        UpdateSuccess = updateSuccess;
        UpdateFailure = updateFailure;
        GetFactorFormulaeArray = getFactorFormulaeArray;
        UserId = userId;
      }
      else
        throw new System.ArgumentException("Argument cannot be null", "logError");
    }

    #endregion


    #region Public Properties

    /// <summary>
    /// The MON_PLAN_ID of the emissions report being processed
    /// </summary>
    public string MonPlanId { get; set; }

    /// <summary>
    /// The RPT_PERIOD_ID of the emissions report being processed
    /// </summary>
    public int RptPeriodId { get; set; }

    #endregion


    #region Private Fields

    private dGetFactorFormulaeArray GetFactorFormulaeArray;
    private dUpdateFailure UpdateFailure;
    private dUpdateInit UpdateInit;
    private dUpdateSuccess UpdateSuccess;
    private string UserId;

    #endregion


    #region Public Methods

    /// <summary>
    /// Performs the actions needed to create the DataMart (AMPD) Emissions data.
    /// </summary>
    /// <param name="monPlanId">The monitor plan id of the emissions report.</param>
    /// <param name="rptPeriodId">The report period id of the emissions report.</param>
    public void ProcessEmissionReport(string monPlanId, int rptPeriodId)
    {
      try
      {
        string errorMessage = "";

        string dmEmissionsId;
        string dataSource;
        bool? isMatsEmissionReport;
        DataTable locationTable;
        DataTable rptPeriodInfoTable;
        DataTable locationTypeCountTable;
        DataTable locationLinkSpanCountTable;
        DataTable locationLinkActiveTable;
        DataTable specialMethodCountTable;
        DataTable monitorHourTable;

        if (UpdateInit(monPlanId, rptPeriodId, UserId,
                       out dmEmissionsId,
                       out dataSource,
                       out isMatsEmissionReport,
                       out locationTable,
                       out rptPeriodInfoTable,
                       out locationTypeCountTable,
                       out locationLinkSpanCountTable,
                       out locationLinkActiveTable,
                       out specialMethodCountTable,
                       out monitorHourTable,
                       ref errorMessage))
        {
          if (rptPeriodInfoTable.Rows.Count == 1)
          {
            int year = rptPeriodInfoTable.Rows[0]["YEAR"].AsInteger().Default();
            int quarter = rptPeriodInfoTable.Rows[0]["QUARTER"].AsInteger().Default();

            eApportionmentType apportionmentType = DetermineApportionmentType(locationTypeCountTable,
                                                                              locationLinkSpanCountTable,
                                                                              locationLinkActiveTable,
                                                                              specialMethodCountTable);

            switch (apportionmentType)
            {
              case eApportionmentType.Error:
                {
                  cErrorHandler.LogError("Unable to determine apportionment type.");
                  UpdateFailure(dmEmissionsId, apportionmentType);
                }
                break;

              case eApportionmentType.MultiplePipe:
                {
                  cErrorHandler.LogError("Multiple pipe apportionment not supported");
                  UpdateFailure(dmEmissionsId, apportionmentType);
                }
                break;

              case eApportionmentType.MultiplePipeInvolved:
                {
                  cErrorHandler.LogError("Apportionment involving multiple pipes not supported");
                  UpdateFailure(dmEmissionsId, apportionmentType);
                }
                break;

              default:
                {
                  cLocationInfo[] locationInfo;

                  if (GetLocationInfo(locationTable, out locationInfo))
                  {
                    cHourlyRawData hourlyRawData = new cHourlyRawData(dmEmissionsId,
                                                                      monPlanId, rptPeriodId,
                                                                      locationInfo,
                                                                      year, quarter,
                                                                      dataSource, UserId);

                    cFactorFormulae[] factorFormulaeArray;
                    cHourlyApportionedData hourlyApportionedData;

                    if (hourlyRawData.Update(monitorHourTable) &&
                        GetFactorFormulae(monPlanId, rptPeriodId, hourlyRawData.UnitInfo, hourlyRawData.LocationInfo, out factorFormulaeArray) &&
                        cHourlyApportionedData.GetApportionedData(apportionmentType,
                                                                  hourlyRawData,
                                                                  factorFormulaeArray,
                                                                  out hourlyApportionedData))
                    {
                      UpdateSuccess(dmEmissionsId,
                                    apportionmentType,
                                    isMatsEmissionReport,
                                    hourlyApportionedData.DmEmissionsIdArray,
                                    hourlyApportionedData.UnitKeyArray,
                                    hourlyApportionedData.OpDateArray,
                                    hourlyApportionedData.OpHourArray,
                                    hourlyApportionedData.OpTimeArray,
                                    hourlyApportionedData.GLoadArray,
                                    hourlyApportionedData.MLoadArray,
                                    hourlyApportionedData.SLoadArray,
                                    hourlyApportionedData.TLoadArray,
                                    hourlyApportionedData.HitArray,
                                    hourlyApportionedData.HitMeasureArray,
                                    hourlyApportionedData.So2mArray,
                                    hourlyApportionedData.So2mMeasureArray,
                                    hourlyApportionedData.So2rArray,
                                    hourlyApportionedData.So2rMeasureArray,
                                    hourlyApportionedData.Co2mArray,
                                    hourlyApportionedData.Co2mMeasureArray,
                                    hourlyApportionedData.Co2rArray,
                                    hourlyApportionedData.Co2rMeasureArray,
                                    hourlyApportionedData.NoxmArray,
                                    hourlyApportionedData.NoxmMeasureArray,
                                    hourlyApportionedData.NoxrArray,
                                    hourlyApportionedData.NoxrMeasureArray,
                                    hourlyApportionedData.HgRateEoArray,
                                    hourlyApportionedData.HgRateHiArray,
                                    hourlyApportionedData.HgMassArray,
                                    hourlyApportionedData.HgMeasureArray,
                                    hourlyApportionedData.HclRateEoArray,
                                    hourlyApportionedData.HclRateHiArray,
                                    hourlyApportionedData.HclMassArray,
                                    hourlyApportionedData.HclMeasureArray,
                                    hourlyApportionedData.HfRateEoArray,
                                    hourlyApportionedData.HfRateHiArray,
                                    hourlyApportionedData.HfMassArray,
                                    hourlyApportionedData.HfMeasureArray,
                                    hourlyApportionedData.MonPlanIdArray,
                                    hourlyApportionedData.RptPeriodIdArray,
                                    hourlyApportionedData.OpYearArray,
                                    hourlyApportionedData.DataSourceArray,
                                    hourlyApportionedData.UserIdArray);
                    }
                    else
                    {
                      UpdateFailure(dmEmissionsId, apportionmentType);
                    }
                  }
                }
                break;
            }
          }
          else
          {
            cErrorHandler.LogError(string.Format("Unknown reporting period: {0}", rptPeriodId));
            UpdateFailure(dmEmissionsId, null);
          }
        }
        else
        {
          cErrorHandler.LogError(errorMessage);
          UpdateFailure(dmEmissionsId, null);
        }
      }
      catch (Exception ex)
      {
        cErrorHandler.LogError(ex.Message);
        UpdateFailure(null, null);
      }
    }

    #endregion


    #region Private Methods

    private eApportionmentType DetermineApportionmentType(DataTable locationTypeCountTable,
                                                          DataTable locationLinkSpanCountTable,
                                                          DataTable locationLinkActiveTable,
                                                          DataTable specialMethodCountTable)
    {
      eApportionmentType result;

      if ((locationTypeCountTable.Rows.Count == 1) &&
          (locationLinkSpanCountTable.Rows.Count <= 1) &&
          (locationLinkActiveTable.Rows.Count <= 1) &&
          (specialMethodCountTable.Rows.Count == 1))
      {
        // Location Type Counts
        int csCount = locationTypeCountTable.Rows[0]["CS"].AsInteger().Default(0);
        int msCount = locationTypeCountTable.Rows[0]["MS"].AsInteger().Default(0);
        int cpCount = locationTypeCountTable.Rows[0]["CP"].AsInteger().Default(0);
        int mpCount = locationTypeCountTable.Rows[0]["MP"].AsInteger().Default(0);
        int unCount = locationTypeCountTable.Rows[0]["UN"].AsInteger().Default(0);

        // Location Link Span Counts
        int csLinkSpanCount = ((locationLinkSpanCountTable.Rows.Count == 1)
                            ? locationLinkSpanCountTable.Rows[0]["CS"].AsInteger().Default(0)
                            : 0);
        int msLinkSpanCount = ((locationLinkSpanCountTable.Rows.Count == 1)
                            ? locationLinkSpanCountTable.Rows[0]["MS"].AsInteger().Default(0)
                            : 0);
        int cpLinkSpanCount = ((locationLinkSpanCountTable.Rows.Count == 1)
                            ? locationLinkSpanCountTable.Rows[0]["CP"].AsInteger().Default(0)
                            : 0);
        int mpLinkSpanCount = ((locationLinkSpanCountTable.Rows.Count == 1)
                            ? locationLinkSpanCountTable.Rows[0]["MP"].AsInteger().Default(0)
                            : 0);

        // Location Link Active Counts
        int csLinkActiveCount = ((locationLinkActiveTable.Rows.Count == 1)
                              ? locationLinkActiveTable.Rows[0]["CS"].AsInteger().Default(0)
                              : 0);
        int msLinkActiveCount = ((locationLinkActiveTable.Rows.Count == 1)
                              ? locationLinkActiveTable.Rows[0]["MS"].AsInteger().Default(0)
                              : 0);
        int cpLinkActiveCount = ((locationLinkActiveTable.Rows.Count == 1)
                              ? locationLinkActiveTable.Rows[0]["CP"].AsInteger().Default(0)
                              : 0);
        int mpLinkActiveCount = ((locationLinkActiveTable.Rows.Count == 1)
                              ? locationLinkActiveTable.Rows[0]["MP"].AsInteger().Default(0)
                              : 0);

        // CP LTFF Active
        bool cpLtffActive = (specialMethodCountTable.Rows[0]["CP_LTFF"].AsInteger().Default(0) >= 1);

        if ((unCount >= 1) && (csCount == 0) && (msCount == 0) && (cpCount == 0) && (mpCount == 0))
        {
          result = eApportionmentType.Unit;
        }
        // Always peform after sunit test
        else if ((csLinkActiveCount == 0) && (msLinkActiveCount == 0) &&
                 (cpLinkActiveCount == 0) && (mpLinkActiveCount == 0))
        {
          result = eApportionmentType.Error;
        }
        // Always perform after error test and before Changed and Complex test.
        else if (mpCount > 0)
        {
          if ((unCount == 1) && (csCount == 0) && (msCount == 0) && (cpCount == 0) && (mpCount > 1))
            result = eApportionmentType.MultiplePipe;
          else
            result = eApportionmentType.MultiplePipeInvolved;
        }
        else if ((csLinkSpanCount != csLinkActiveCount) || (msLinkSpanCount != msLinkActiveCount) ||
                 (cpLinkSpanCount != cpLinkActiveCount) || (mpLinkSpanCount != mpLinkActiveCount))
        {
          result = eApportionmentType.Changed;
        }
        else if (((csCount * unCount) != csLinkSpanCount) ||
                 ((msCount * unCount) != msLinkSpanCount) ||
                 ((cpCount * unCount) != cpLinkSpanCount) ||
                 ((mpCount * unCount) != mpLinkSpanCount))
        {
          result = eApportionmentType.Complex;
        }
        else if ((unCount > 1) && (csCount >= 1) && (msCount == 0) && (cpCount == 0) && (mpCount == 0))
        {
          result = eApportionmentType.CommonStack;
        }
        else if ((unCount > 1) && (csCount == 0) && (msCount == 0) && (cpCount >= 1) && (mpCount == 0))
        {
          if (cpLtffActive)
            result = eApportionmentType.CommonPipeLtff;
          else
            result = eApportionmentType.CommonPipe;
        }
        else if ((unCount == 1) && (csCount == 0) && (msCount > 1) && (cpCount == 0) && (mpCount == 0))
        {
          result = eApportionmentType.MultipleStack;
        }
        else if ((unCount > 1) && (csCount >= 1) && (msCount == 0) && (cpCount >= 1) && (mpCount == 0))
        {
          result = eApportionmentType.CommonStackAndPipe;
        }
        else
        {
          result = eApportionmentType.Complex;
        }
      }
      else
      {
        result = eApportionmentType.Error;
      }

      return result;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="monPlanId"></param>
    /// <param name="rptPeriodId"></param>
    /// <param name="unitInfo"></param>
    /// <param name="locationInfo"></param>
    /// <param name="factorFormulaeArray"></param>
    /// <returns></returns>
    private bool GetFactorFormulae(string monPlanId, int rptPeriodId, cUnitInfo[] unitInfo, cLocationInfo[] locationInfo, out  cFactorFormulae[] factorFormulaeArray)
    {
      bool result;

      string errorMessage = null;

      if (GetFactorFormulaeArray(monPlanId, rptPeriodId, unitInfo, locationInfo, out factorFormulaeArray, ref errorMessage))
      {
        result = true;
      }
      else
      {
        cErrorHandler.LogError(errorMessage, string.Format("MON_PLAN_ID: {0} and RPT_PERIOD_ID: {1}", monPlanId, rptPeriodId));
        result = false;
      }

      return result;
    }

    private bool GetLocationInfo(DataTable locationTable, out cLocationInfo[] locationInfo)
    {
      bool result = true;

      locationInfo = new cLocationInfo[locationTable.Rows.Count];

      for (int locationDex = 0;
           result && locationDex < locationTable.Rows.Count;
           locationDex++)
      {
        DataRow locationRow = locationTable.Rows[locationDex];

        try
        {
          locationInfo[locationDex] = new cLocationInfo(locationRow["MON_LOC_ID"].AsString(),
                                                        locationRow["UNIT_ID"].AsInteger(),
                                                        locationRow["LOCATION_NAME"].AsString());
        }
        catch (Exception ex)
        {
          cErrorHandler.LogError(ex.Message, "MON_LOC_ID: " + locationRow["MON_LOC_ID"].AsString());
          result = false;
        }
      }

      return result;
    }

    #endregion

  }

}
