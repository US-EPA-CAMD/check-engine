using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECMPS.DM.Definitions;
using ECMPS.DM.Utilities;

namespace ECMPS.DM.HourlyEmissions
{

  /// <summary>
  /// Handles apportionment for emissions reports that only contain actual emissions data for units.
  /// </summary>
  public class cHourlyApportionedDataUnit : cHourlyApportionedData
  {

    #region Public Constructors

    /// <summary>
    /// Creates an instance of the  Unit Hourly Apportioned Data class.
    /// </summary>
    /// <param name="hourlyRawData">The raw data on which the apportionment will occur.</param>
    public cHourlyApportionedDataUnit(cHourlyRawData hourlyRawData)
      : base(hourlyRawData)
    {
    }

    #endregion


    #region Public Override Properties

    /// <summary>
    /// The apportionment type of the class.
    /// </summary>
    public override eApportionmentType ApportionmentType { get { return eApportionmentType.Unit; } }

    #endregion


    #region Public Override Methods

    /// <summary>
    /// The method that apportions an individual hour.
    /// </summary>
    /// <param name="date">The date of the hour to apportion.</param>
    /// <param name="hour">The hour to apportion.</param>
    /// <returns></returns>
    public override bool Apportion(DateTime date, int hour)
    {
      bool result;

      int? rawHourOffset, apportionHourOffset;

      if (GetHourOffset(date, hour, out apportionHourOffset) &&
          HourlyRawData.GetHourOffset(date, hour, out rawHourOffset))
      {
        CopyHour(apportionHourOffset.Value, rawHourOffset.Value);

        // Calculate and fill missing electrical output and heat input based rates.
        FillMissingMatsRates(apportionHourOffset.Value);

        result = true;
      }
      else
        result = false;

      return result;
    }

    #endregion

  }

}
