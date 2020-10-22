using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECMPS.Checks.TypeUtilities
{

  /// <summary>
  /// Status class that gives access to testing whether a decimal will fit into a field with
  /// a particular decimal precision.
  /// </summary>
  public static class cDecimalPrecision
  {

    /// <summary>
    /// Checks the decimal value against the decimal precision specification.
    /// </summary>
    /// <param name="AValue">The value to check.</param>
    /// <param name="ADecimalPrecision">The specification to check against.</param>
    /// <returns>True if the value will fit into a field mathcing the specification.</returns>
    public static bool Check(decimal AValue, eDecimalPrecision ADecimalPrecision)
    {
      bool Result = FDecimalPrecision[(int)ADecimalPrecision].IsValid(AValue);
      return Result;
    }

    /// <summary>
    /// Allows testing whether a given value is less than or equal to the 
    /// maximum value based on the defining precision and scale values.
    /// </summary>
    private class cHandleDecimalPrecision
    {

      internal cHandleDecimalPrecision(int APrecision, int AScale)
      {
        decimal LimitingValue = Convert.ToDecimal(Math.Pow(10, APrecision - AScale));
        decimal IncreamentValue = Convert.ToDecimal(Math.Pow(10, -AScale));

        FMaxValue = (LimitingValue - IncreamentValue);
      }

      private decimal FMaxValue = decimal.MinValue;

      /// <summary>
      /// Indicates whether the value will fit into a field with the defining precision and scale.
      /// </summary>
      /// <param name="AValue">The value to test.</param>
      /// <returns>True if the test value will fit into the a field of the defining precision and scale.</returns>
      public bool IsValid(decimal AValue)
      {
        bool Result = (AValue <= FMaxValue);
        return Result;
      }

    }

    private static cHandleDecimalPrecision[] FDecimalPrecision;

    /// <summary>
    /// Initialize the class
    /// </summary>
    public static void Initialize()
    {
      FDecimalPrecision = new cHandleDecimalPrecision[Enum.GetValues(typeof(eDecimalPrecision)).Length];

      FDecimalPrecision[(int)eDecimalPrecision.ADJUSTED_HRLY_VALUE] = new cHandleDecimalPrecision(14, 4);
      FDecimalPrecision[(int)eDecimalPrecision.APPLICABLE_BIAS_ADJ_FACTOR] = new cHandleDecimalPrecision(5, 3);
      FDecimalPrecision[(int)eDecimalPrecision.CURRENT_RPT_PERIOD_TOTAL] = new cHandleDecimalPrecision(13, 3);
      FDecimalPrecision[(int)eDecimalPrecision.MASS_FLOW_RATE] = new cHandleDecimalPrecision(10, 1);
      FDecimalPrecision[(int)eDecimalPrecision.OP_VALUE] = new cHandleDecimalPrecision(13, 3);
      FDecimalPrecision[(int)eDecimalPrecision.OS_TOTAL] = new cHandleDecimalPrecision(13, 3);
      FDecimalPrecision[(int)eDecimalPrecision.PARAM_VAL_FUEL] = new cHandleDecimalPrecision(13, 5);
      FDecimalPrecision[(int)eDecimalPrecision.PCT_DILUENT] = new cHandleDecimalPrecision(5, 1);
      FDecimalPrecision[(int)eDecimalPrecision.PCT_MOISTURE] = new cHandleDecimalPrecision(5, 1);
      FDecimalPrecision[(int)eDecimalPrecision.TOTAL_HEAT_INPUT] = new cHandleDecimalPrecision(10, 0);
      FDecimalPrecision[(int)eDecimalPrecision.UNADJUSTED_HRLY_VALUE] = new cHandleDecimalPrecision(13, 3);
      FDecimalPrecision[(int)eDecimalPrecision.UPSCALE_CAL_ERROR] = new cHandleDecimalPrecision(6, 2);
      FDecimalPrecision[(int)eDecimalPrecision.VOLUMETRIC_FLOW_RATE] = new cHandleDecimalPrecision(10, 1);
      FDecimalPrecision[(int)eDecimalPrecision.YEAR_TOTAL] = new cHandleDecimalPrecision(13, 3);
      FDecimalPrecision[(int)eDecimalPrecision.ZERO_CAL_ERROR] = new cHandleDecimalPrecision(6, 2);
      FDecimalPrecision[(int)eDecimalPrecision.TOTAL_DAILY_EMISSION] = new cHandleDecimalPrecision(10, 1);
      FDecimalPrecision[(int)eDecimalPrecision.FUEL_CARBON_BURNED] = new cHandleDecimalPrecision(13, 1);
      FDecimalPrecision[(int)eDecimalPrecision.TOTAL_DAILY_OP_TIME] = new cHandleDecimalPrecision(4, 2);
      FDecimalPrecision[(int)eDecimalPrecision.FUEL_FLOW_TOTAL] = new cHandleDecimalPrecision(14, 4);
      FDecimalPrecision[(int)eDecimalPrecision.MATS_PERCENT] = new cHandleDecimalPrecision(4, 1);
      FDecimalPrecision[(int)eDecimalPrecision.MATS_PERCENT_BREAKTHROUGH] = new cHandleDecimalPrecision(6, 1);
    }

  }

  /// <summary>
  /// Enumeration for identifying decimal perceion information
  /// </summary>
  public enum eDecimalPrecision
  {
    /// <summary>
    /// Decimal Precision Type
    /// </summary>
    ADJUSTED_HRLY_VALUE,

    /// <summary>
    /// Decimal Precision Type
    /// </summary>
    APPLICABLE_BIAS_ADJ_FACTOR,

    /// <summary>
    /// Decimal Precision Type
    /// </summary>
    CURRENT_RPT_PERIOD_TOTAL,

    /// <summary>
    /// Decimal Precision Type
    /// </summary>
    MASS_FLOW_RATE,

    /// <summary>
    /// Decimal Precision Type
    /// </summary>
    OP_VALUE,

    /// <summary>
    /// Decimal Precision Type
    /// </summary>
    OS_TOTAL,

    /// <summary>
    /// Decimal Precision Type
    /// </summary>
    PARAM_VAL_FUEL,

    /// <summary>
    /// Decimal Precision Type
    /// </summary>
    PCT_DILUENT,

    /// <summary>
    /// Decimal Precision Type
    /// </summary>
    PCT_MOISTURE,

    /// <summary>
    /// Decimal Precision Type
    /// </summary>
    TOTAL_HEAT_INPUT,

    /// <summary>
    /// Decimal Precision Type
    /// </summary>
    UNADJUSTED_HRLY_VALUE,

    /// <summary>
    /// Decimal Precision Type
    /// </summary>
    UPSCALE_CAL_ERROR,

    /// <summary>
    /// Decimal Precision Type
    /// </summary>
    VOLUMETRIC_FLOW_RATE,

    /// <summary>
    /// Decimal Precision Type
    /// </summary>
    YEAR_TOTAL,

    /// <summary>
    /// Decimal Precision Type
    /// </summary>
    ZERO_CAL_ERROR,

        /// <summary>
    /// Decimal Precision Type
    /// </summary>
    TOTAL_DAILY_EMISSION,

    /// <summary>
    /// Decimal Precision Type
    /// </summary>
    FUEL_CARBON_BURNED,

    /// <summary>
    /// Decimal Precision Type
    /// </summary>
    TOTAL_DAILY_OP_TIME,

    /// <summary>
    /// Decimal Precision Type
    /// </summary>
    FUEL_FLOW_TOTAL,

    /// <summary>
    /// MATS Percentage (does not include 100%)
    /// </summary>
    MATS_PERCENT,

    /// <summary>
    /// MATS Breakthrough Percentage (does not include 100%)
    /// </summary>
        MATS_PERCENT_BREAKTHROUGH
    }

}
