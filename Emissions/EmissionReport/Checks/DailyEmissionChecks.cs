using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.EmissionsReport;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Extensions;

namespace ECMPS.Checks.EmissionsChecks
{
  public class cDailyEmissionChecks : cEmissionsChecks
  {

    #region Constructors

    public cDailyEmissionChecks(cEmissionsReportProcess emissionReportProcess)
      : base(emissionReportProcess)
    {
      CheckProcedures = new dCheckProcedure[14];

      CheckProcedures[1] = new dCheckProcedure(DAILY1);
      CheckProcedures[2] = new dCheckProcedure(DAILY2);
      CheckProcedures[3] = new dCheckProcedure(DAILY3);
      CheckProcedures[4] = new dCheckProcedure(DAILY4);
      CheckProcedures[5] = new dCheckProcedure(DAILY5);
      CheckProcedures[6] = new dCheckProcedure(DAILY6);
      CheckProcedures[7] = new dCheckProcedure(DAILY7);
      CheckProcedures[8] = new dCheckProcedure(DAILY8);
      CheckProcedures[9] = new dCheckProcedure(DAILY9);
      CheckProcedures[10] = new dCheckProcedure(DAILY10);
      CheckProcedures[11] = new dCheckProcedure(DAILY11);
      CheckProcedures[12] = new dCheckProcedure(DAILY12);
      CheckProcedures[13] = new dCheckProcedure(DAILY13);
    }

    #endregion


    #region Public Static Methods: Checks

    public static string DAILY1(cCategory Category, ref bool Log)
    // Determine Need for Daily CO2 Emissions Record        
    {
      string ReturnVal = "";

      try
      {
        sFilterPair[] RowFilter;

        Category.SetCheckParameter("Current_CO2_Mass_Daily_Record", null, eParameterDataType.DataRowView);

        decimal? DailyOpTime = Category.GetCheckParameter("Daily_Op_Time_Accumulator_Array").ValueAsDecimalArray()[Category.CurrentMonLocPos];
        {
          if (DailyOpTime < 0) 
            DailyOpTime = null;

          Category.SetCheckParameter("Daily_Op_Time", DailyOpTime, eParameterDataType.Decimal);
        }

        Category.SetArrayParameter("Daily_Op_Time_Accumulator_Array", Category.CurrentMonLocPos, 0.0m);

        DataView MonitorMethodRecords = Category.GetCheckParameter("Monitor_Method_Records_By_Day_Location").ValueAsDataView();

        // Monitor Method CO2% Record Count
        RowFilter = new sFilterPair[1];
        RowFilter[0].Set("PARAMETER_CD", "CO2", eFilterPairStringCompare.BeginsWith);

        int Co2MethodCount = CountRows(MonitorMethodRecords, RowFilter);

        // Monitor Method CO2M and FSA Record Count
        RowFilter = new sFilterPair[2];
        RowFilter[0].Set("PARAMETER_CD", "CO2M");
        RowFilter[1].Set("METHOD_CD", "FSA");

        int FsaMethodCount = CountRows(MonitorMethodRecords, RowFilter);

        if ((FsaMethodCount > 0) && (Co2MethodCount > 1))
        {
          Category.CheckCatalogResult = "A";
        }
        else
        {
          if (FsaMethodCount > 0)
            Category.SetArrayParameter("Expected_Summary_Value_Co2_Array", Category.CurrentMonLocPos, true);

          DataView Co2mDailyEmissionRecords = Category.GetCheckParameter("CO2M_Daily_Emission_Records_For_Day_Location").ValueAsDataView();
          int Co2mDailyEmissionCount = Co2mDailyEmissionRecords.Count;

          if (Co2mDailyEmissionCount > 1)
          {
            Category.SetArrayParameter("Rpt_Period_Co2_Mass_Reported_Accumulator_Array", Category.CurrentMonLocPos, -1.0m);
            Category.SetArrayParameter("Rpt_Period_Co2_Mass_Calculated_Accumulator_Array", Category.CurrentMonLocPos, -1.0m);
            Category.CheckCatalogResult = "B";
          }
          else if ((FsaMethodCount == 0) && (Co2mDailyEmissionCount > 0))
          {
            Category.CheckCatalogResult = "C";
          }
          else if ((FsaMethodCount > 0) && (Co2mDailyEmissionCount == 0) && 
                   (DailyOpTime.HasValue & (DailyOpTime.Value > 0)))
          {
            Category.CheckCatalogResult = "D";
          }
          else if ((FsaMethodCount > 0) && (Co2mDailyEmissionCount == 1))
          {
            DataRowView CurrentCo2mDailyRecord = Co2mDailyEmissionRecords[0];

            Category.SetCheckParameter("Current_CO2_Mass_Daily_Record", CurrentCo2mDailyRecord, eParameterDataType.DataRowView);

            if (DailyOpTime.HasValue & (DailyOpTime.Value == 0))
              Category.CheckCatalogResult = "E";
          }
        }
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "DAILY1");
      }

      return ReturnVal;
    }

    public static string DAILY2(cCategory Category, ref bool log)
    // Check Total Daily Emissions Value        
    {
      string returnVal = "";

      try
      {
        decimal? calcTde = null;

        DataRowView currentCo2MassDailyRecord = Category.GetCheckParameter("Current_CO2_Mass_Daily_Record").ValueAsDataRowView();

        if (currentCo2MassDailyRecord != null)
        {
          decimal? adjustedDailyEmission = currentCo2MassDailyRecord["ADJUSTED_DAILY_EMISSION"].AsDecimal();
          decimal? sorbentMassEmission = currentCo2MassDailyRecord["SORBENT_MASS_EMISSION"].AsDecimal();
          decimal? totalDailyEmission = currentCo2MassDailyRecord["TOTAL_DAILY_EMISSION"].AsDecimal();
          decimal? unadjustedDailyEmission = currentCo2MassDailyRecord["UNADJUSTED_DAILY_EMISSION"].AsDecimal();

          if (totalDailyEmission.HasValue && (totalDailyEmission.Value >= 0) && 
              (Category.GetCheckParameter("Rpt_Period_Co2_Mass_Reported_Accumulator_Array").ValueAsDecimalArray()[Category.CurrentMonLocPos] >= 0))
          {
            Category.AccumCheckAggregate("Rpt_Period_Co2_Mass_Reported_Accumulator_Array", Category.CurrentMonLocPos, totalDailyEmission.Value);
          }

          decimal? calcCo2Unadj = Category.GetCheckParameter("Calc_CO2_Unadj").AsDecimal();

          if (unadjustedDailyEmission.IsNotNull() || calcCo2Unadj.IsNotNull())
          {
            if (adjustedDailyEmission.IsNull())
              calcTde = calcCo2Unadj.Value;
            else if (unadjustedDailyEmission.IsNotNull() &&
                     (adjustedDailyEmission.Value >= 0) && 
                     (adjustedDailyEmission.Value <= unadjustedDailyEmission.Value))
              calcTde = adjustedDailyEmission.Value;
          }
          else if (adjustedDailyEmission.HasValue && (adjustedDailyEmission.Value >= 0))
          {
            calcTde = adjustedDailyEmission;
          }

          if (calcTde != null)
          {
            if (sorbentMassEmission != null)
            {
              if (sorbentMassEmission >= 0)
                calcTde += sorbentMassEmission;
              else
                calcTde = null;
            }
          }
          else if ((unadjustedDailyEmission == null) &&
                   Category.GetCheckParameter("Legacy_Data_Evaluation").ValueAsBool() &&
                   (totalDailyEmission >= 0))
          {
            calcTde = totalDailyEmission;
          }

          if (calcTde == null)
          {
            Category.SetArrayParameter("Rpt_Period_Co2_Mass_Calculated_Accumulator_Array", Category.CurrentMonLocPos, -1.0m);
          }
          else
          {
            if (Category.GetCheckParameter("Rpt_Period_Co2_Mass_Calculated_Accumulator_Array").ValueAsDecimalArray()[Category.CurrentMonLocPos] >= 0)
            {
              Category.AccumCheckAggregate("Rpt_Period_Co2_Mass_Calculated_Accumulator_Array", Category.CurrentMonLocPos, calcTde.Value);
            }
          }

          if (totalDailyEmission.HasValue && (totalDailyEmission.Value >= 0))
          {
            if (calcTde != null)
            {
              decimal tolerance = GetTolerance("CO2M DAILY", "TON", Category);

              if (Math.Abs(totalDailyEmission.Value - calcTde.Value) > tolerance)
                Category.CheckCatalogResult = "A";
            }
            else
            {
              Category.CheckCatalogResult = "C";
            }
          }
          else
          {
            Category.SetArrayParameter("Rpt_Period_Co2_Mass_Reported_Accumulator_Array", Category.CurrentMonLocPos, -1.0m);
            Category.CheckCatalogResult = "B";
          }
        }

        Category.SetCheckParameter("Calc_TDE", calcTde, eParameterDataType.Decimal);
      }
      catch (Exception ex)
      {
        returnVal = Category.CheckEngine.FormatError(ex, "DAILY2");
      }

      return returnVal;
    }

    public static string DAILY3(cCategory Category, ref bool Log)
    // Check Adjusted Daily Emissions Value        
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentCo2mDailyRecord = Category.GetCheckParameter("Current_CO2_Mass_Daily_Record").ValueAsDataRowView();

        if (CurrentCo2mDailyRecord != null)
        {
          decimal? AdjustedDailyEmission = cDBConvert.ToNullableDecimal(CurrentCo2mDailyRecord["ADJUSTED_DAILY_EMISSION"]);
          decimal? UnadjustedDailyEmission = cDBConvert.ToNullableDecimal(CurrentCo2mDailyRecord["UNADJUSTED_DAILY_EMISSION"]);

          if (AdjustedDailyEmission != null)
          {
            if (AdjustedDailyEmission < 0)
              Category.CheckCatalogResult = "A";
            else
            {
              if ((UnadjustedDailyEmission != null) &&
                  ((UnadjustedDailyEmission >= 0) &&
                   (UnadjustedDailyEmission < AdjustedDailyEmission)))
                Category.CheckCatalogResult = "B";
            }
          }
        }
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "DAILY3");
      }

      return ReturnVal;
    }

    public static string DAILY4(cCategory Category, ref bool Log)
    // Check Sorbent Related Emissions        
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentCo2mDailyRecord = Category.GetCheckParameter("Current_CO2_Mass_Daily_Record").ValueAsDataRowView();

        if (CurrentCo2mDailyRecord != null)
        {
          decimal? SorbentMassEmission = cDBConvert.ToNullableDecimal(CurrentCo2mDailyRecord["SORBENT_MASS_EMISSION"]);

          if ((SorbentMassEmission != null) && (SorbentMassEmission < 0))
          {
            Category.CheckCatalogResult = "A";
          }
        }
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "DAILY4");
      }

      return ReturnVal;
    }

    public static string DAILY5(cCategory Category, ref bool Log)
    // Validate Presence of Adjusted Daily Emissions        
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentCo2mDailyRecord = Category.GetCheckParameter("Current_CO2_Mass_Daily_Record").ValueAsDataRowView();

        if (CurrentCo2mDailyRecord != null)
        {
          decimal? AdjustedDailyEmission = cDBConvert.ToNullableDecimal(CurrentCo2mDailyRecord["ADJUSTED_DAILY_EMISSION"]);

          if (AdjustedDailyEmission != null)
          {
            sFilterPair[] RowFilter;

            DataView MonitorFormulaRecords = Category.GetCheckParameter("Monitor_Formula_Records_By_Day_Location").ValueAsDataView();

            RowFilter = new sFilterPair[2];
            RowFilter[0].Set("PARAMETER_CD", "CO2M");
            RowFilter[1].Set("EQUATION_CD", "G-2,G-3", eFilterPairStringCompare.InList);

            if (CountRows(MonitorFormulaRecords, RowFilter) == 0)
              Category.CheckCatalogResult = "A";
          }
        }
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "DAILY5");
      }

      return ReturnVal;
    }

    public static string DAILY6(cCategory Category, ref bool Log)
    // Validate Presence of Sorbent Related Emissions        
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentCo2mDailyRecord = Category.GetCheckParameter("Current_CO2_Mass_Daily_Record").ValueAsDataRowView();

        if (CurrentCo2mDailyRecord != null)
        {
          decimal? SorbentMassEmission = cDBConvert.ToNullableDecimal(CurrentCo2mDailyRecord["SORBENT_MASS_EMISSION"]);

          if (SorbentMassEmission != null)
          {
            sFilterPair[] RowFilter;

            DataView MonitorFormulaRecords = Category.GetCheckParameter("Monitor_Formula_Records_By_Day_Location").ValueAsDataView();

            string MissingCo2mFormula = null;

            RowFilter = new sFilterPair[2];
            RowFilter[0].Set("PARAMETER_CD", "CO2M");
            RowFilter[1].Set("EQUATION_CD", "G-5,G-6", eFilterPairStringCompare.InList);

            if (CountRows(MonitorFormulaRecords, RowFilter) == 0)
              MissingCo2mFormula = MissingCo2mFormula.ListAdd("G-5 or G-6");

            RowFilter = new sFilterPair[2];
            RowFilter[0].Set("PARAMETER_CD", "CO2M");
            RowFilter[1].Set("EQUATION_CD", "G-8");

            if (CountRows(MonitorFormulaRecords, RowFilter) == 0)
              MissingCo2mFormula = MissingCo2mFormula.ListAdd("G-8");

            Category.SetCheckParameter( "Missing_CO2M_Formula", MissingCo2mFormula.FormatList(), eParameterDataType.String );

            if (MissingCo2mFormula != null)
              Category.CheckCatalogResult = "A";
          }
        }
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "DAILY6");
      }

      return ReturnVal;
    }

    public static string DAILY7(cCategory category, ref bool log)
    // Check Unadjusted Daily Emissions Value        
    {
      string returnVal = "";

      category.SetCheckParameter("Calc_CO2_Unadj", null, eParameterDataType.Decimal);

      try
      {
        DataRowView currentCo2mDailyRecord = category.GetCheckParameter("Current_CO2_Mass_Daily_Record").AsDataRowView();

        if (currentCo2mDailyRecord != null)
        {
          decimal? calcTotalCarbonBurned = category.GetCheckParameter("Calc_Total_Carbon_Burned").AsDecimal();
          decimal? unadjustedDailyEmission = currentCo2mDailyRecord["UNADJUSTED_DAILY_EMISSION"].AsDecimal();

          if (calcTotalCarbonBurned.HasValue && (calcTotalCarbonBurned > 0))
          {
            decimal calcCo2Unadj = Math.Round(calcTotalCarbonBurned.Value * 44 / 24000, 1, MidpointRounding.AwayFromZero);
            {
              category.SetCheckParameter("Calc_CO2_Unadj", calcCo2Unadj, eParameterDataType.Decimal);
            }

            if (unadjustedDailyEmission.IsNull() || (unadjustedDailyEmission.Value < 0))
            {
              category.CheckCatalogResult = "A";
            }
            else if (unadjustedDailyEmission.HasValue && (unadjustedDailyEmission >= 0))
            {
              if (!TestHourlyEmissionsTolerance(unadjustedDailyEmission.Value, calcCo2Unadj, 
                                                "CO2M DAILY", "TON", category))
                category.CheckCatalogResult = "B";
            }
          }
          else if (unadjustedDailyEmission == null)
          {
            if (!category.GetCheckParameter("Legacy_Data_Evaluation").AsBoolean().Default())
              category.CheckCatalogResult = "A";
          }
          else if (unadjustedDailyEmission < 0)
            category.CheckCatalogResult = "A";
          else
          {
            category.SetCheckParameter("Calc_CO2_Unadj", unadjustedDailyEmission, eParameterDataType.Decimal);
          }
        }
      }
      catch (Exception ex)
      {
        returnVal = category.CheckEngine.FormatError(ex, "DAILY7");
      }

      return returnVal;
    }

    public static string DAILY8(cCategory Category, ref bool Log)
    // Check Fuel in Daily Fuel Record        
    {
        string ReturnVal = "";

        try
        {
            DataRowView CurrentDailyFuelRecord = Category.GetCheckParameter("Current_Daily_Fuel_Record").ValueAsDataRowView();

            if (CurrentDailyFuelRecord != null)
            {
                sFilterPair[] Filter = new sFilterPair[1];
                Filter[0].Set("FUEL_CD", cDBConvert.ToString(CurrentDailyFuelRecord["FUEL_CD"]));
                DataView FuelRecs = Category.GetCheckParameter("Fuel_Records_By_Date_and_Location").ValueAsDataView();
                if (!(FindRows(FuelRecs, Filter).Count > 0))
                    Category.CheckCatalogResult = "A";
            }
        }
        catch (Exception ex)
        {
            ReturnVal = Category.CheckEngine.FormatError(ex, "DAILY8");
        }

        return ReturnVal;
    }

    public static string DAILY9(cCategory Category, ref bool Log)
    // Check Daily Fuel Feed  
    {
        string ReturnVal = "";

        try
        {
            DataRowView CurrentDailyFuelRecord = Category.GetCheckParameter("Current_Daily_Fuel_Record").ValueAsDataRowView();

            if (CurrentDailyFuelRecord != null)
                if (cDBConvert.ToDecimal(CurrentDailyFuelRecord["DAILY_FUEL_FEED"]) <= 0)
                    Category.CheckCatalogResult = "A";
        }
        catch (Exception ex)
        {
            ReturnVal = Category.CheckEngine.FormatError(ex, "DAILY9");
        }

        return ReturnVal;
    }

    public static string DAILY10(cCategory Category, ref bool Log)
    // Check Carbon Content Used      
    {
        string ReturnVal = "";

        try
        {
            DataRowView CurrentDailyFuelRecord = Category.GetCheckParameter("Current_Daily_Fuel_Record").ValueAsDataRowView();

            if (CurrentDailyFuelRecord != null)
            {
                decimal CarbonContUsed = cDBConvert.ToDecimal(CurrentDailyFuelRecord["CARBON_CONTENT_USED"]);
                if (CarbonContUsed <= 0 || CarbonContUsed > 100)
                    Category.CheckCatalogResult = "A";
            }
        }
        catch (Exception ex)
        {
            ReturnVal = Category.CheckEngine.FormatError(ex, "DAILY10");
        }

        return ReturnVal;
    }

    public static string DAILY11(cCategory Category, ref bool Log)
    // Check Fuel Carbon Burned    
    {
        string ReturnVal = "";

        try
        {
            Category.SetCheckParameter("Calc_Fuel_Carbon_Burned", null, eParameterDataType.Decimal);
            
            DataRowView CurrentDailyFuelRecord = Category.GetCheckParameter("Current_Daily_Fuel_Record").ValueAsDataRowView();

            if (CurrentDailyFuelRecord != null)
            {
                decimal DailyFuelFeed = cDBConvert.ToDecimal(CurrentDailyFuelRecord["DAILY_FUEL_FEED"]);
                decimal CarbonContUsed = cDBConvert.ToDecimal(CurrentDailyFuelRecord["CARBON_CONTENT_USED"]);

                if (DailyFuelFeed > 0 && CarbonContUsed > 0 && CarbonContUsed <= 100)
                {
                    decimal CalcFuelCarbonBurned = Math.Round(DailyFuelFeed * CarbonContUsed / 100, 1, MidpointRounding.AwayFromZero);
                    Category.SetCheckParameter("Calc_Fuel_Carbon_Burned", CalcFuelCarbonBurned, eParameterDataType.Decimal);
                    decimal CalcTotalCarbonBurned = Category.GetCheckParameter("Calc_Total_Carbon_Burned").ValueAsDecimal();
                    if (CalcTotalCarbonBurned >= 0)
                    {
                        CalcTotalCarbonBurned += CalcFuelCarbonBurned;
                        Category.SetCheckParameter("Calc_Total_Carbon_Burned", CalcTotalCarbonBurned, eParameterDataType.Decimal);
                    }
                }
                else
                    Category.SetCheckParameter("Calc_Total_Carbon_Burned", -1m, eParameterDataType.Decimal);

                decimal FuelCarbonBurned = cDBConvert.ToDecimal(CurrentDailyFuelRecord["FUEL_CARBON_BURNED"]);
                if (FuelCarbonBurned <= 0)
                    Category.CheckCatalogResult = "A";
                else
                {
                    decimal? CalcFuelCarbonBurned = Category.GetCheckParameter("Calc_Fuel_Carbon_Burned").ValueAsNullOrDecimal();
                    if (CalcFuelCarbonBurned != null && FuelCarbonBurned != CalcFuelCarbonBurned)
                    {
                        decimal Tolerance = GetTolerance("CARBON", "LB", Category);
                        if (Math.Abs(FuelCarbonBurned - CalcFuelCarbonBurned.Value) > Tolerance)
                            Category.CheckCatalogResult = "B";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ReturnVal = Category.CheckEngine.FormatError(ex, "DAILY11");
        }

        return ReturnVal;
    }

    public static string DAILY12(cCategory Category, ref bool Log)
    // Intialize Daily Emissions        
    {
        string ReturnVal = "";

        try
        {
            Category.SetCheckParameter("Calc_Total_Carbon_Burned", 0m, eParameterDataType.Decimal);
        }
        catch (Exception ex)
        {
            ReturnVal = Category.CheckEngine.FormatError(ex, "DAILY12");
        }

        return ReturnVal;
    }

    public static string DAILY13(cCategory Category, ref bool Log)
    // Check Unadjusted Daily Emissions Value        
    {
        string ReturnVal = "";

        try
        {
            Category.SetCheckParameter("Calculate_CO2M_TDE", true, eParameterDataType.Boolean);

            DataRowView CurrentCo2mDailyRecord = Category.GetCheckParameter("Current_CO2_Mass_Daily_Record").ValueAsDataRowView();
            decimal CalcTotalCarbonBurned = Category.GetCheckParameter("Calc_Total_Carbon_Burned").ValueAsDecimal();
            if (CurrentCo2mDailyRecord != null)
            {
                decimal? TotalCarbonBurned = cDBConvert.ToNullableDecimal(CurrentCo2mDailyRecord["TOTAL_CARBON_BURNED"]);

                if (TotalCarbonBurned == null)
                {
                    if (CalcTotalCarbonBurned != 0)
                        Category.CheckCatalogResult = "A";
                }
                else
                {
                    if (TotalCarbonBurned < 0)
                        Category.CheckCatalogResult = "B";
                    else
                    {
                        if (CalcTotalCarbonBurned > 0 && TotalCarbonBurned != CalcTotalCarbonBurned)
                        {
                            decimal tolerance = GetTolerance("CARBON", "LB", Category);

                            if (Math.Abs(TotalCarbonBurned.Value - CalcTotalCarbonBurned) > tolerance)
                                Category.CheckCatalogResult = "C";
                        }
                        else
                            if (CalcTotalCarbonBurned == 0)
                                Category.SetCheckParameter("Calc_Total_Carbon_Burned", TotalCarbonBurned, eParameterDataType.Decimal);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ReturnVal = Category.CheckEngine.FormatError(ex, "DAILY13");
        }

        return ReturnVal;
    }

    #endregion


    #region Private Methods: Utilities

    private static decimal GetTolerance(string AParameterCd, String AUom, cCategory ACategory)
    {
      DataView ToleranceView = (DataView)ACategory.GetCheckParameter("Hourly_Emissions_Tolerances_Cross_Check_Table").ParameterValue;
      DataRowView ToleranceRow;
      sFilterPair[] ToleranceFilter = new sFilterPair[2];

      ToleranceFilter[0].Set("Parameter", AParameterCd);
      ToleranceFilter[1].Set("UOM", AUom);

      if (FindRow(ToleranceView, ToleranceFilter, out ToleranceRow))
        return cDBConvert.ToDecimal(ToleranceRow["Tolerance"]);
      else
        return decimal.MinValue;
    }

    #endregion

  }
}
