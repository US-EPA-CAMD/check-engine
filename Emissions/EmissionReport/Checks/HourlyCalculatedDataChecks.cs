using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Em.Parameters;
using ECMPS.Checks.EmissionsReport;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;


using ECMPS.Definitions.Extensions;


namespace ECMPS.Checks.EmissionsChecks
{
  public class cHourlyCalculatedDataChecks : cEmissionsChecks
  {

    #region Constructors

    public cHourlyCalculatedDataChecks(cEmissionsReportProcess emissionReportProcess)
      : base(emissionReportProcess)
    {
      CheckProcedures = new dCheckProcedure[44];

      CheckProcedures[1] = new dCheckProcedure(HOURCV1);
      CheckProcedures[2] = new dCheckProcedure(HOURCV2);
      CheckProcedures[3] = new dCheckProcedure(HOURCV3);
      CheckProcedures[4] = new dCheckProcedure(HOURCV4);
      CheckProcedures[6] = new dCheckProcedure(HOURCV6);
      CheckProcedures[7] = new dCheckProcedure(HOURCV7);
      CheckProcedures[9] = new dCheckProcedure(HOURCV9);

      CheckProcedures[12] = new dCheckProcedure(HOURCV12);
      CheckProcedures[13] = new dCheckProcedure(HOURCV13);
      CheckProcedures[15] = new dCheckProcedure(HOURCV15);
      CheckProcedures[16] = new dCheckProcedure(HOURCV16);
      CheckProcedures[18] = new dCheckProcedure(HOURCV18);
      CheckProcedures[19] = new dCheckProcedure(HOURCV19);

      CheckProcedures[25] = new dCheckProcedure(HOURCV25);
      CheckProcedures[30] = new dCheckProcedure(HOURCV30);

      CheckProcedures[31] = new dCheckProcedure(HOURCV31);
      CheckProcedures[32] = new dCheckProcedure(HOURCV32);
      CheckProcedures[33] = new dCheckProcedure(HOURCV33);
      CheckProcedures[34] = new dCheckProcedure(HOURCV34);
      CheckProcedures[35] = new dCheckProcedure(HOURCV35);
      CheckProcedures[36] = new dCheckProcedure(HOURCV36);
      CheckProcedures[37] = new dCheckProcedure(HOURCV37);
      CheckProcedures[38] = new dCheckProcedure(HOURCV38);
      CheckProcedures[39] = new dCheckProcedure(HOURCV39);
      CheckProcedures[40] = new dCheckProcedure(HOURCV40);

      CheckProcedures[41] = new dCheckProcedure(HOURCV41);
      CheckProcedures[42] = new dCheckProcedure(HOURCV42);
      CheckProcedures[43] = new dCheckProcedure(HOURCV43);
    }

    #endregion

    #region Public Static Methods: Checks

    #region Checks 1 - 10

    public static string HOURCV1(cCategory Category, ref bool Log)
    // Calculate Percent H2O
    // Formerly Hourly-78
    {
      string ReturnVal = "";

      try
      {
        string H2oMethodCode = cDBConvert.ToString(Category.GetCheckParameter("H2O_Method_Code").ParameterValue);
        DataRowView CurrentDHVRecord = (DataRowView)Category.GetCheckParameter("Current_DHV_Record").ParameterValue;
        string ModcCd = cDBConvert.ToString(CurrentDHVRecord["MODC_CD"]);

        if (H2oMethodCode == "MWD" && ModcCd.InList("01,02,03,04,53,54"))
        {
          string H2oCemEquationCode = cDBConvert.ToString(Category.GetCheckParameter("H2o_Cem_Equation_Code").ParameterValue);
          decimal H2oReportedValue = cDBConvert.ToDecimal(Category.GetCheckParameter("H2O_Reported_Value").ParameterValue);
          decimal AdjVal = cDBConvert.ToDecimal(CurrentDHVRecord["ADJUSTED_HRLY_VALUE"]);
          if (H2oCemEquationCode == "F-31")
          {
            decimal O2DryCalcValue = cDBConvert.ToDecimal(Category.GetCheckParameter("O2_Dry_Calculated_Adjusted_Value").ParameterValue);
            decimal O2WetCalcValue = cDBConvert.ToDecimal(Category.GetCheckParameter("O2_Wet_Calculated_Adjusted_Value").ParameterValue);
            if (Convert.ToBoolean(Category.GetCheckParameter("Current_DHV_Record_Valid").ParameterValue) &&
                O2DryCalcValue != decimal.MinValue && O2WetCalcValue != decimal.MinValue)
            {
              decimal H2oCalcAdjVal = Math.Round(100 * (O2DryCalcValue - O2WetCalcValue) / O2DryCalcValue, 1, MidpointRounding.AwayFromZero);
              Category.SetCheckParameter("H2O_DHV_Calculated_Adjusted_Value", H2oCalcAdjVal, eParameterDataType.Decimal);

              decimal H2oConcTolerance = GetTolerance("H2O", "PCT", Category);
              if (Convert.ToBoolean(Category.GetCheckParameter("Derived_Hourly_Adjusted_Value_Status").ParameterValue) &&
                  Math.Abs(H2oCalcAdjVal - AdjVal) > H2oConcTolerance)
                Category.CheckCatalogResult = "A";
            }
            else
              Category.CheckCatalogResult = "B";
          }
          else
            if (H2oCemEquationCode == "M-1K")
            {
              if (Convert.ToBoolean(Category.GetCheckParameter("Derived_Hourly_Adjusted_Value_Status").ParameterValue))
              {
                if (AdjVal != decimal.MinValue)
                  Category.SetCheckParameter("H2O_DHV_Calculated_Adjusted_Value", AdjVal, eParameterDataType.Decimal);
                else
                  Category.SetCheckParameter("H2O_DHV_Calculated_Adjusted_Value", null, eParameterDataType.Decimal);
              }
            }
            else
              Category.CheckCatalogResult = "B";
        }
        else
        {
          if (H2oMethodCode == "MDF" && ModcCd == "40")
            Category.SetCheckParameter("H2O_DHV_Calculated_Adjusted_Value", Convert.ToDecimal(Category.GetCheckParameter("H2O_Default_Value").ParameterValue), eParameterDataType.Decimal);
          else
          {
            if (Category.GetCheckParameter("Current_DHV_Calculated_Adjusted_Value").ParameterValue == null)
              Category.SetCheckParameter("H2O_DHV_Calculated_Adjusted_Value", null, eParameterDataType.Decimal);
            else
              Category.SetCheckParameter("H2O_DHV_Calculated_Adjusted_Value", Convert.ToDecimal(Category.GetCheckParameter("Current_DHV_Calculated_Adjusted_Value").ParameterValue), eParameterDataType.Decimal);
          }
        }
      }

      
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "HOURCV1");
      }

      return ReturnVal;
    }

    public static string HOURCV2(cCategory Category, ref bool Log)
    // Generate Final H2O Value
    // Formerly Hourly-68
    {
      string ReturnVal = "";

      try
      {
        string H2oMethodCode = cDBConvert.ToString(Category.GetCheckParameter("H2O_Method_Code").ParameterValue);
        decimal H2oReportedValue = cDBConvert.ToDecimal(Category.GetCheckParameter("H2O_Reported_Value").ParameterValue);

        Category.SetCheckParameter("Valid_H2o_Final_Percentage", false, eParameterDataType.Boolean);
        Category.SetCheckParameter("Final_H2o_Percentage", H2oReportedValue, eParameterDataType.Decimal);
        Category.SetCheckParameter("Update_H2o_Calculated_Value", false, eParameterDataType.Boolean);

        if (cDBConvert.ToBoolean(Category.GetCheckParameter("H2O_Inclusive_Hourly_Status").ParameterValue))
        {
          if (H2oMethodCode == "MWD")
          {
            if (cDBConvert.ToBoolean(Category.GetCheckParameter("H2o_Calculation_Status").ParameterValue))
            {
              decimal H2oCalculatedPercent = cDBConvert.ToDecimal(Category.GetCheckParameter("H2o_Calculated_Percent").ParameterValue);

              Category.SetCheckParameter("Final_H2o_Percentage", H2oCalculatedPercent, eParameterDataType.Decimal);
              Category.SetCheckParameter("Valid_H2o_Final_Percentage", true, eParameterDataType.Boolean);
              Category.SetCheckParameter("Update_H2o_Calculated_Value", true, eParameterDataType.Boolean);
            }
            else
            {
              Category.SetCheckParameter("Final_H2o_Percentage", H2oReportedValue, eParameterDataType.Decimal);
              Category.SetCheckParameter("Valid_H2o_Final_Percentage", false, eParameterDataType.Boolean);
            }
          }
          else if ((H2oMethodCode == "MMS") || (H2oMethodCode == "MTB"))
          {
            Category.SetCheckParameter("Final_H2o_Percentage", H2oReportedValue, eParameterDataType.Decimal);
            Category.SetCheckParameter("Valid_H2o_Final_Percentage", true, eParameterDataType.Boolean);
          }
        }
        else if (H2oMethodCode == "MDF")
        {
          decimal H2oDefaultValue = cDBConvert.ToDecimal(Category.GetCheckParameter("H2O_Default_Value").ParameterValue);

          Category.SetCheckParameter("Final_H2o_Percentage", H2oDefaultValue, eParameterDataType.Decimal);
          Category.SetCheckParameter("Valid_H2o_Final_Percentage", true, eParameterDataType.Boolean);
        }
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "HOURCV2");
      }

      return ReturnVal;
    }

    public static string HOURCV3(cCategory Category, ref bool Log)
    //Determine Diluent Cap and Moisture for CO2 Concentration Calculation Verification
    {
      string ReturnVal = "";

      try
      {
        string EquCd = Convert.ToString(Category.GetCheckParameter("CO2_Conc_Cem_Equation_Code").ParameterValue);
        if (EquCd == "F-14B")
        {
          string MethCd = Convert.ToString(Category.GetCheckParameter("H2O_Method_Code").ParameterValue);
          if (MethCd == "MWD" && Convert.ToBoolean(Category.GetCheckParameter("H2O_Derived_Hourly_Checks_Needed").ParameterValue) &&
              Category.GetCheckParameter("H2O_DHV_Calculated_Adjusted_Value").ParameterValue != null)
            Category.SetCheckParameter("Calculated_Moisture_for_CO2C", Convert.ToDecimal(Category.GetCheckParameter("H2O_DHV_Calculated_Adjusted_Value").ParameterValue), eParameterDataType.Decimal);
          else
              if( MethCd.InList( "MMS,MTB" ) && Convert.ToBoolean( Category.GetCheckParameter( "H2O_Monitor_Hourly_Checks_Needed" ).ParameterValue ) &&
                Category.GetCheckParameter("H2O_MHV_Calculated_Adjusted_Value").ParameterValue != null)
              Category.SetCheckParameter("Calculated_Moisture_for_CO2C", Convert.ToDecimal(Category.GetCheckParameter("H2O_MHV_Calculated_Adjusted_Value").ParameterValue), eParameterDataType.Decimal);
            else
              if (MethCd == "MDF" && Convert.ToBoolean(Category.GetCheckParameter("H2O_Derived_Hourly_Checks_Needed").ParameterValue) &&
                  Category.GetCheckParameter("H2O_DHV_Calculated_Adjusted_Value").ParameterValue != null)
                Category.SetCheckParameter("Calculated_Moisture_for_CO2C", Convert.ToDecimal(Category.GetCheckParameter("H2O_DHV_Calculated_Adjusted_Value").ParameterValue), eParameterDataType.Decimal);
              else
              {
                decimal DefVal = cDBConvert.ToDecimal(Category.GetCheckParameter("H2O_Default_Value").ParameterValue);
                if (MethCd == "MDF" && !Convert.ToBoolean(Category.GetCheckParameter("H2O_Derived_Hourly_Checks_Needed").ParameterValue) &&
                    DefVal != decimal.MinValue)
                  Category.SetCheckParameter("Calculated_Moisture_for_CO2C", DefVal, eParameterDataType.Decimal);
              }
        }
        if (Convert.ToBoolean(Category.GetCheckParameter("Use_O2_Diluent_Cap_for_Co2_Conc_Calc").ParameterValue))
        {
          DataView MonDefRecs = (DataView)Category.GetCheckParameter("Monitor_Default_Records_by_Hour_Location").ParameterValue;
          sFilterPair[] FilterDef = new sFilterPair[3];
          FilterDef[0].Set("DEFAULT_PURPOSE_CD", "DC");
          FilterDef[1].Set("PARAMETER_CD", "O2X");
          FilterDef[2].Set("FUEL_CD", "NFS");
          MonDefRecs = FindRows(MonDefRecs, FilterDef);
          if (MonDefRecs.Count > 1)
            Category.CheckCatalogResult = "A";
          else
            if (MonDefRecs.Count == 0)
              Category.CheckCatalogResult = "B";
            else
            {
              decimal MonDef = cDBConvert.ToDecimal(MonDefRecs[0]["DEFAULT_VALUE"]);
              if (MonDef <= 0)
                Category.CheckCatalogResult = "C";
              else
                Category.SetCheckParameter("Calculated_Diluent_for_CO2C", MonDef, eParameterDataType.Decimal);
            }
        }
        else
          if (EquCd == "F-14A")
            Category.SetCheckParameter("Calculated_Diluent_for_CO2C", cDBConvert.ToDecimal(Category.GetCheckParameter("O2_Dry_Calculated_Adjusted_Value").ParameterValue), eParameterDataType.Decimal);
          else
            if (EquCd == "F-14B")
              Category.SetCheckParameter("Calculated_Diluent_for_CO2C", cDBConvert.ToDecimal(Category.GetCheckParameter("O2_Wet_Calculated_Adjusted_Value").ParameterValue), eParameterDataType.Decimal);
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "HOURCV3");
      }

      return ReturnVal;
    }

    public static string HOURCV4(cCategory Category, ref bool Log)
    // Calculate CO2 Concentration
    {
      string ReturnVal = "";

      try
      {

        DataRowView CurrentDHVRecord = (DataRowView)Category.GetCheckParameter("Current_DHV_Record").ParameterValue;
        string ModcCd = cDBConvert.ToString(CurrentDHVRecord["MODC_CD"]);
        if( ModcCd.InList( "01,02,03,04,53,54" ) )
        {
          string EquCd = Convert.ToString(Category.GetCheckParameter("CO2_Conc_Cem_Equation_Code").ParameterValue);
          decimal CalcDiluent = cDBConvert.ToDecimal(Category.GetCheckParameter("Calculated_Diluent_for_CO2C").ParameterValue);
          decimal Tolerance = GetTolerance("CO2C", "PCT", Category);
          DataRowView CurrentHrlyOpRec = (DataRowView)Category.GetCheckParameter("Current_Hourly_Op_Record").ParameterValue;
          decimal CalcAdjVal;
          if (EquCd == "F-14A")
            if (Convert.ToBoolean(Category.GetCheckParameter("Current_DHV_Record_Valid").ParameterValue) &&
                CalcDiluent != decimal.MinValue &&
                Convert.ToBoolean(Category.GetCheckParameter("Valid_Fc_Factor_Exists").ParameterValue) &&
                Convert.ToBoolean(Category.GetCheckParameter("Valid_Fd_Factor_Exists").ParameterValue))
            {
              CalcAdjVal = 100 * (cDBConvert.ToDecimal(CurrentHrlyOpRec["FC_FACTOR"]) / cDBConvert.ToDecimal(CurrentHrlyOpRec["FD_FACTOR"])) * (((decimal)20.9 - CalcDiluent) / (decimal)20.9);
              if (CalcAdjVal < 0)
                CalcAdjVal = 0m;
              else
                CalcAdjVal = Math.Round(CalcAdjVal, 1, MidpointRounding.AwayFromZero);
              Category.SetCheckParameter("CO2C_DHV_Calculated_Adjusted_Value", CalcAdjVal, eParameterDataType.Decimal);
              if (Convert.ToBoolean(Category.GetCheckParameter("Derived_Hourly_Adjusted_Value_Status").ParameterValue) && Math.Abs(CalcAdjVal - cDBConvert.ToDecimal(CurrentDHVRecord["ADJUSTED_HRLY_VALUE"])) > Tolerance)
                Category.CheckCatalogResult = "A";
            }
            else
              Category.CheckCatalogResult = "B";
          else
            if (EquCd == "F-14B")
            {
              decimal CalcMoistureCO2C = cDBConvert.ToDecimal(Category.GetCheckParameter("Calculated_Moisture_for_CO2C").ParameterValue);
              if (Convert.ToBoolean(Category.GetCheckParameter("Current_DHV_Record_Valid").ParameterValue) &&
                  CalcDiluent != decimal.MinValue && Convert.ToBoolean(Category.GetCheckParameter("Valid_Fc_Factor_Exists").ParameterValue) &&
                  Convert.ToBoolean(Category.GetCheckParameter("Valid_Fd_Factor_Exists").ParameterValue) && CalcMoistureCO2C != decimal.MinValue)
              {
                CalcAdjVal = (100 / (decimal)20.9) * (cDBConvert.ToDecimal(CurrentHrlyOpRec["FC_FACTOR"]) / cDBConvert.ToDecimal(CurrentHrlyOpRec["FD_FACTOR"])) * (((decimal)20.9 * (100 - CalcMoistureCO2C) / 100) - CalcDiluent);
                if (CalcAdjVal < 0)
                  CalcAdjVal = 0m;
                else
                  CalcAdjVal = Math.Round(CalcAdjVal, 1, MidpointRounding.AwayFromZero);
                Category.SetCheckParameter("CO2C_DHV_Calculated_Adjusted_Value", CalcAdjVal, eParameterDataType.Decimal);
                if (Convert.ToBoolean(Category.GetCheckParameter("Derived_Hourly_Adjusted_Value_Status").ParameterValue) && Math.Abs(CalcAdjVal - cDBConvert.ToDecimal(CurrentDHVRecord["ADJUSTED_HRLY_VALUE"])) > Tolerance)
                  Category.CheckCatalogResult = "A";
              }
              else
                Category.CheckCatalogResult = "B";
            }
            else
              Category.CheckCatalogResult = "B";
        }
        else
          Category.SetCheckParameter("CO2C_DHV_Calculated_Adjusted_Value", Convert.ToDecimal(Category.GetCheckParameter("Current_DHV_Calculated_Adjusted_Value").ParameterValue), eParameterDataType.Decimal);
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "HOURCV4");
      }

      return ReturnVal;
    }

    public static string HOURCV6(cCategory Category, ref bool Log)
    //Determine Diluent Cap and Moisture for Heat Input Calculation Verification
    {
      string ReturnVal = "";

      try
      {
        if (Convert.ToString(Category.GetCheckParameter("Heat_Input_Method_Code").ParameterValue) == "CEM")
        {
          string HiEquationCode = cDBConvert.ToString(Category.GetCheckParameter("Heat_Input_CEM_Equation_Code").ParameterValue);
          if( HiEquationCode.InList( "F-16,F-17,F-18" ) )
          {
            decimal H2ODHVCalcAdjVal = cDBConvert.ToDecimal(Category.GetCheckParameter("H2O_DHV_Calculated_Adjusted_Value").ParameterValue);
            decimal H2OMHVCalcAdjVal = cDBConvert.ToDecimal(Category.GetCheckParameter("H2O_MHV_Calculated_Adjusted_Value").ParameterValue);
            decimal H2ODefVal = cDBConvert.ToDecimal(Category.GetCheckParameter("H2O_Default_Value").ParameterValue);
            string H2OMethCd = Convert.ToString(Category.GetCheckParameter("H2O_Method_Code").ParameterValue);
            decimal CalcMoistureHI = decimal.MinValue;
            if (H2OMethCd == "MWD" && Convert.ToBoolean(Category.GetCheckParameter("H2O_Derived_Hourly_Checks_Needed").ParameterValue) && H2ODHVCalcAdjVal != decimal.MinValue)
              CalcMoistureHI = H2ODHVCalcAdjVal;
            else
                if( H2OMethCd.InList( "MMS,MTB" ) && Convert.ToBoolean( Category.GetCheckParameter( "H2O_Monitor_Hourly_Checks_Needed" ).ParameterValue ) && H2OMHVCalcAdjVal != decimal.MinValue )
                CalcMoistureHI = H2OMHVCalcAdjVal;
              else
                if (H2OMethCd == "MDF" && Convert.ToBoolean(Category.GetCheckParameter("H2O_Derived_Hourly_Checks_Needed").ParameterValue) && H2ODHVCalcAdjVal != decimal.MinValue)
                  CalcMoistureHI = H2ODHVCalcAdjVal;
                else
                  if (H2OMethCd == "MDF" && !Convert.ToBoolean(Category.GetCheckParameter("H2O_Derived_Hourly_Checks_Needed").ParameterValue) && H2ODefVal != decimal.MinValue)
                    CalcMoistureHI = H2ODefVal;
            if (CalcMoistureHI != decimal.MinValue)
              Category.SetCheckParameter("Calculated_Moisture_for_HI", CalcMoistureHI, eParameterDataType.Decimal);
          }
          DataRowView CurrentDHVRecord = (DataRowView)Category.GetCheckParameter("Current_DHV_Record").ParameterValue;
          decimal CalcDiluentHI = decimal.MinValue;
          DataView MonDefRecs;
          sFilterPair[] FilterDef;
          if( HiEquationCode.InList( "F-15,F-16" ) )
          {
            if (cDBConvert.ToInteger(CurrentDHVRecord["DILUENT_CAP_IND"]) == 1)
            {
              MonDefRecs = (DataView)Category.GetCheckParameter("Monitor_Default_Records_by_Hour_Location").ParameterValue;
              FilterDef = new sFilterPair[3];
              FilterDef[0].Set("DEFAULT_PURPOSE_CD", "DC");
              FilterDef[1].Set("PARAMETER_CD", "CO2N");
              FilterDef[2].Set("FUEL_CD", "NFS");
              MonDefRecs = FindRows(MonDefRecs, FilterDef);
              if (MonDefRecs.Count > 1)
                Category.CheckCatalogResult = "A";
              else
                if (MonDefRecs.Count == 0)
                  Category.CheckCatalogResult = "B";
                else
                {
                  decimal DefVal = cDBConvert.ToDecimal(MonDefRecs[0]["DEFAULT_VALUE"]);
                  if (DefVal <= 0)
                    Category.CheckCatalogResult = "C";
                  else
                    CalcDiluentHI = DefVal;
                }
            }
            else
              if (Convert.ToBoolean(Category.GetCheckParameter("CO2_Conc_Checks_Needed_for_Heat_Input").ParameterValue))
                if (Category.GetCheckParameter("Current_CO2_Conc_Missing_Data_Monitor_Hourly_Record").ParameterValue != null)
                  CalcDiluentHI = cDBConvert.ToDecimal(Category.GetCheckParameter("CO2C_SD_Calculated_Adjusted_Value").ParameterValue);
                else
                  CalcDiluentHI = cDBConvert.ToDecimal(Category.GetCheckParameter("CO2C_MHV_Calculated_Adjusted_Value").ParameterValue);
          }
          else
              if( HiEquationCode.InList( "F-17,F-18" ) )
              if (cDBConvert.ToInteger(CurrentDHVRecord["DILUENT_CAP_IND"]) == 1)
              {
                MonDefRecs = (DataView)Category.GetCheckParameter("Monitor_Default_Records_by_Hour_Location").ParameterValue;
                FilterDef = new sFilterPair[3];
                FilterDef[0].Set("DEFAULT_PURPOSE_CD", "DC");
                FilterDef[1].Set("PARAMETER_CD", "O2X");
                FilterDef[2].Set("FUEL_CD", "NFS");
                MonDefRecs = FindRows(MonDefRecs, FilterDef);
                if (MonDefRecs.Count > 1)
                  Category.CheckCatalogResult = "D";
                else
                  if (MonDefRecs.Count == 0)
                    Category.CheckCatalogResult = "E";
                  else
                  {
                    decimal DefVal = cDBConvert.ToDecimal(MonDefRecs[0]["DEFAULT_VALUE"]);
                    if (DefVal <= 0)
                      Category.CheckCatalogResult = "F";
                    else
                      CalcDiluentHI = DefVal;
                  }
              }
              else
                if (HiEquationCode == "F-17" && Convert.ToBoolean(Category.GetCheckParameter("O2_Wet_Checks_Needed_for_Heat_Input").ParameterValue))
                  if (Category.GetCheckParameter("Current_O2_Wet_Missing_Data_Monitor_Hourly_Record").ParameterValue != null)
                    CalcDiluentHI = cDBConvert.ToDecimal(Category.GetCheckParameter("O2C_SD_Calculated_Adjusted_Value").ParameterValue);
                  else
                    CalcDiluentHI = cDBConvert.ToDecimal(Category.GetCheckParameter("O2_Wet_Calculated_Adjusted_Value").ParameterValue);
                else
                  if (HiEquationCode == "F-18" && Convert.ToBoolean(Category.GetCheckParameter("O2_Dry_Checks_Needed_for_Heat_Input").ParameterValue))
                    if (Category.GetCheckParameter("Current_O2_Dry_Missing_Data_Monitor_Hourly_Record").ParameterValue != null)
                      CalcDiluentHI = cDBConvert.ToDecimal(Category.GetCheckParameter("O2C_SD_Calculated_Adjusted_Value").ParameterValue);
                    else
                      CalcDiluentHI = cDBConvert.ToDecimal(Category.GetCheckParameter("O2_Dry_Calculated_Adjusted_Value").ParameterValue);
          if (CalcDiluentHI != decimal.MinValue)
            Category.SetCheckParameter("Calculated_Diluent_for_HI", CalcDiluentHI, eParameterDataType.Decimal);
          else//DELETE: for testing only
            Category.SetCheckParameter("Calculated_Diluent_for_HI", null, eParameterDataType.Decimal);
        }
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "HOURCV6");
      }

      return ReturnVal;
    }

    public static string HOURCV7(cCategory Category, ref bool Log)
    // Calculate Heat Input        
    {
      string ReturnVal = "";

      try
      {
        DataRowView HourlyOpDataRow = (DataRowView)Category.GetCheckParameter("Current_Hourly_Op_Record").ParameterValue;
        decimal OpTime = cDBConvert.ToDecimal(HourlyOpDataRow["OP_TIME"]);
        DataRowView CurrentDHVRecord = (DataRowView)Category.GetCheckParameter("Current_DHV_Record").ParameterValue;
        int thisLocation = cDBConvert.ToInteger(Category.GetCheckParameter("Current_Monitor_Plan_Location_Postion").ParameterValue);
        decimal AdjVal = cDBConvert.ToDecimal(CurrentDHVRecord["Adjusted_Hrly_Value"]);
        string EntityType = Convert.ToString(Category.GetCheckParameter("Current_Entity_Type").ParameterValue);
        decimal UnitHIOpTimeAccum = cDBConvert.ToDecimal(Category.GetCheckParameter("Unit_HeatInputTimesOpTime_Accumulator").ParameterValue);
        decimal StackHIOpTimeAccum = cDBConvert.ToDecimal(Category.GetCheckParameter("Stack_HeatInputTimesOpTime_Accumulator").ParameterValue);
        decimal ConfigHIOpTimeAccum = cDBConvert.ToDecimal(Category.GetCheckParameter("Config_HeatInputTimesOpTime_Accumulator").ParameterValue);

        bool AnnRptRequ = Convert.ToBoolean(Category.GetCheckParameter("Annual_Reporting_Requirement").ParameterValue);
        DateTime CurrentOpDate = Category.GetCheckParameter("Current_Operating_Date").ValueAsDateTime(DateTypes.START);
        string CurrMonth = CurrentOpDate.ToString("MMMM");

        if (Convert.ToBoolean(Category.GetCheckParameter("Derived_Hourly_Adjusted_Value_Status").ParameterValue) && 0 <= OpTime && OpTime <= 1)
        {
          decimal HITotalReported = OpTime * AdjVal;

          if (CurrMonth != "April" || AnnRptRequ)
          {
            decimal[] HIAccumArray = Category.GetCheckParameter("Rpt_Period_Hi_Reported_Accumulator_Array").ValueAsDecimalArray();
            if (HIAccumArray[thisLocation] != decimal.MinValue)
            {
              if (HIAccumArray[thisLocation] >= 0)
                Category.AccumCheckAggregate("Rpt_Period_Hi_Reported_Accumulator_Array", thisLocation, HITotalReported);
            }
            else
              Category.SetArrayParameter("Rpt_Period_Hi_Reported_Accumulator_Array", thisLocation, HITotalReported);
          }

          if  (EntityType == "Unit" && UnitHIOpTimeAccum >= 0)
            Category.SetCheckParameter("Unit_HeatInputTimesOpTime_Accumulator", UnitHIOpTimeAccum + HITotalReported, eParameterDataType.Decimal);
          else if ((EntityType == "CS" || EntityType == "MS") && StackHIOpTimeAccum >= 0)
            Category.SetCheckParameter("Stack_HeatInputTimesOpTime_Accumulator", StackHIOpTimeAccum + HITotalReported, eParameterDataType.Decimal);
        }
        else
        {
          if (CurrMonth != "April" || AnnRptRequ)
            Category.AccumCheckAggregate("Rpt_Period_Hi_Reported_Accumulator_Array", thisLocation, -1m);

          if (EntityType == "Unit")
            Category.SetCheckParameter("Unit_HeatInputTimesOpTime_Accumulator", -1, eParameterDataType.Decimal);
          else if ((EntityType == "CS") || (EntityType == "MS"))
            Category.SetCheckParameter("Stack_HeatInputTimesOpTime_Accumulator", -1, eParameterDataType.Decimal);
        }

        // Initialize Total_Heat_Input_From_Fuel_Flow
        Category.SetCheckParameter("Total_Heat_Input_From_Fuel_Flow", null, eParameterDataType.Decimal);

        decimal FcFactor = cDBConvert.ToDecimal(HourlyOpDataRow["FC_Factor"]);
        decimal FdFactor = cDBConvert.ToDecimal(HourlyOpDataRow["FD_Factor"]);
        bool ValidFcFactor = Convert.ToBoolean(Category.GetCheckParameter("Valid_Fc_Factor_Exists").ParameterValue);
        bool ValidFdFactor = Convert.ToBoolean(Category.GetCheckParameter("Valid_Fd_Factor_Exists").ParameterValue);
        bool DHVValid = Convert.ToBoolean(Category.GetCheckParameter("Current_DHV_Record_Valid").ParameterValue);
        decimal CalcDiluentHI = cDBConvert.ToDecimal(Category.GetCheckParameter("Calculated_Diluent_for_HI").ParameterValue);
        decimal FlowCalcAdjVal = cDBConvert.ToDecimal(Category.GetCheckParameter("FLOW_Calculated_Adjusted_Value").ParameterValue);
        decimal CalcMoisture = cDBConvert.ToDecimal(Category.GetCheckParameter("Calculated_Moisture_for_HI").ParameterValue);
        decimal HICalcAdjVal = decimal.MinValue;
        string HIMethCd = Convert.ToString(Category.GetCheckParameter("Heat_Input_Method_Code").ParameterValue);
        bool IsLegacyData = Category.GetCheckParameter("Legacy_Data_Evaluation").ValueAsBool();

        if (HIMethCd == "CEM")
        {
          switch (cDBConvert.ToString(Category.GetCheckParameter("Heat_Input_CEM_Equation_Code").ParameterValue))
          {
            case "F-15":
              if (DHVValid && CalcDiluentHI != decimal.MinValue && ValidFcFactor && FlowCalcAdjVal != decimal.MinValue)
                HICalcAdjVal = Math.Round(FlowCalcAdjVal * CalcDiluentHI / (FcFactor * 100), 1, MidpointRounding.AwayFromZero);
              else
                Category.CheckCatalogResult = "A";
              break;

            case "F-16":
              if (DHVValid && CalcDiluentHI != decimal.MinValue && ValidFcFactor && FlowCalcAdjVal != decimal.MinValue && CalcMoisture != decimal.MinValue)
                HICalcAdjVal = Math.Round(FlowCalcAdjVal * (100 - CalcMoisture) * CalcDiluentHI / (FcFactor * 10000), 1, MidpointRounding.AwayFromZero);
              else
                Category.CheckCatalogResult = "A";
              break;

            case "F-17":
              if (DHVValid && CalcDiluentHI != decimal.MinValue && ValidFdFactor && FlowCalcAdjVal != decimal.MinValue && CalcMoisture != decimal.MinValue)
                HICalcAdjVal = Math.Round(FlowCalcAdjVal * (1 / FdFactor) * ((decimal)0.209 * (100 - CalcMoisture) - CalcDiluentHI) / (decimal)20.9, 1, MidpointRounding.AwayFromZero);
              else
                Category.CheckCatalogResult = "A";
              break;

            case "F-18":
              if (DHVValid && CalcDiluentHI != decimal.MinValue && ValidFdFactor && FlowCalcAdjVal != decimal.MinValue && CalcMoisture != decimal.MinValue)
                HICalcAdjVal = Math.Round(FlowCalcAdjVal * (100 - CalcMoisture) * ((decimal)20.9 - CalcDiluentHI) / (2090 * FdFactor), 1, MidpointRounding.AwayFromZero);
              else
                Category.CheckCatalogResult = "A";
              break;
            default:
              Category.CheckCatalogResult = "A";
              break;
          }
          if (string.IsNullOrEmpty(Category.CheckCatalogResult))
          {
            if (HICalcAdjVal < 1 && !IsLegacyData)
              HICalcAdjVal = 1;

            Category.SetArrayParameter("Apportionment_Calc_Hi_Array", thisLocation, HICalcAdjVal);
            if (Convert.ToString(Category.GetCheckParameter("MP_Stack_Config_For_Hourly_Checks").ParameterValue) == "MS")
              Category.AccumCheckAggregate("Config_HeatInput_Accumulator", HICalcAdjVal);
          }
          else//if result == "A"
          {
            Category.SetArrayParameter("Apportionment_Calc_Hi_Array", thisLocation, -1m);
            Category.SetCheckParameter("Config_HeatInputTimesOpTime_Accumulator", -1, eParameterDataType.Decimal);
            Category.SetArrayParameter("Rpt_Period_Hi_Calculated_Accumulator_Array", thisLocation, -1m);
            if (Convert.ToString(Category.GetCheckParameter("MP_Stack_Config_For_Hourly_Checks").ParameterValue) == "MS")
              Category.SetCheckParameter("Config_HeatInput_Accumulator", -1, eParameterDataType.Decimal);
          }
        }
        else if (Convert.ToBoolean(Category.GetCheckParameter("Heat_Input_App_D_Method_Active_For_Hour").ParameterValue))
        {
          decimal? hiAppdAccumulator = Category.GetCheckParameter("Hi_App_D_Accumulator").AsDecimal();

          // Update Total_Heat_Input_From_Fuel_Flow
          if (hiAppdAccumulator.HasValue && (hiAppdAccumulator.Value >= 0))
            Category.SetCheckParameter("Total_Heat_Input_From_Fuel_Flow", hiAppdAccumulator.Value, eParameterDataType.Decimal);

          decimal AppDAccum = hiAppdAccumulator.Default();
          if (AppDAccum >= 0 && 0 < OpTime && OpTime <= 1)

          {
            decimal AppCalcHI = Math.Round(AppDAccum / OpTime, 1, MidpointRounding.AwayFromZero);
            Category.AccumCheckAggregate("Apportionment_Calc_Hi_Array", thisLocation, AppCalcHI);

            if (HIMethCd == "AD")
            {
              HICalcAdjVal = AppCalcHI;
              Category.SetCheckParameter("App_E_Calc_Hi", HICalcAdjVal, eParameterDataType.Decimal);
            }
            else
            {
              string[] HIAppMethod = Category.GetCheckParameter("Apportionment_Hi_Method_Array").ValueAsStringArray();
              int MonitorLocationCount = cDBConvert.ToInteger(Category.GetCheckParameter("Current_Location_Count").ParameterValue);
              for (int MonitorLocationDex = 0; MonitorLocationDex < MonitorLocationCount; MonitorLocationDex++)
              {
                  if (HIAppMethod[MonitorLocationDex] == "ADCALC" || HIAppMethod[MonitorLocationDex] == "CALC")
                    HIAppMethod[MonitorLocationDex] = "NOCALC";
              }
              Category.SetCheckParameter("Apportionment_Hi_Method_Array", HIAppMethod, eParameterDataType.String, false, true);
            }
          }
          else
          {
            Category.SetArrayParameter("Apportionment_Calc_Hi_Array", thisLocation, -1m);
            Category.SetCheckParameter("Config_HeatInputTimesOpTime_Accumulator", -1, eParameterDataType.Decimal);
            if (CurrMonth != "April" || AnnRptRequ)
              Category.SetArrayParameter("Rpt_Period_Hi_Calculated_Accumulator_Array", thisLocation, -1m);
            Category.CheckCatalogResult = "A";
          }
        }
        else if( !HIMethCd.InList( "ADCALC,CALC" ) )
        {
          HICalcAdjVal = cDBConvert.ToDecimal(Category.GetCheckParameter("Current_DHV_Calculated_Adjusted_Value").ParameterValue);
          Category.AccumCheckAggregate("Apportionment_Calc_Hi_Array", thisLocation, HICalcAdjVal);
        }

        decimal HeatInputTotalCalcVal;

        if (string.IsNullOrEmpty(Category.CheckCatalogResult) && HICalcAdjVal != decimal.MinValue)
        {
          Category.SetCheckParameter("HI_Calculated_Adjusted_Value", Math.Round(HICalcAdjVal, 1, MidpointRounding.AwayFromZero), eParameterDataType.Decimal);

          if (0 <= OpTime && OpTime <= 1)
          {
            HeatInputTotalCalcVal = OpTime * HICalcAdjVal;

            if ((ConfigHIOpTimeAccum >= 0))
              Category.SetCheckParameter("Config_HeatInputTimesOpTime_Accumulator", ConfigHIOpTimeAccum + HeatInputTotalCalcVal, eParameterDataType.Decimal);

            if (CurrMonth != "April" || AnnRptRequ)
            {
              decimal[] HICalcAccum = Category.GetCheckParameter("Rpt_Period_Hi_Calculated_Accumulator_Array").ValueAsDecimalArray();
              if (HICalcAccum[thisLocation] != decimal.MinValue)
              {
                if (HICalcAccum[thisLocation] >= 0)
                  Category.AccumCheckAggregate("Rpt_Period_Hi_Calculated_Accumulator_Array", thisLocation, HeatInputTotalCalcVal);
              }
              else
                Category.SetArrayParameter("Rpt_Period_Hi_Calculated_Accumulator_Array", thisLocation, HeatInputTotalCalcVal);

              if (CurrMonth == "April")
              {
                decimal[] AprilHICalcAccum = Category.GetCheckParameter("April_HI_Calculated_Accumulator_Array").ValueAsDecimalArray();
                if (AprilHICalcAccum[thisLocation] != decimal.MinValue)
                {
                  //if (AprilHICalcAccum[thisLocation] >= 0)1/25/08 removed as found to be not in spec.
                  Category.AccumCheckAggregate("April_HI_Calculated_Accumulator_Array", thisLocation, HeatInputTotalCalcVal);
                }
                else
                  Category.SetArrayParameter("April_HI_Calculated_Accumulator_Array", thisLocation, HeatInputTotalCalcVal);
              }
            }
          }
          else
          {
            Category.SetCheckParameter("Config_HeatInputTimesOpTime_Accumulator", -1, eParameterDataType.Decimal);
            if (CurrMonth != "April" || AnnRptRequ)
              Category.SetArrayParameter("Rpt_Period_Hi_Calculated_Accumulator_Array", thisLocation, -1m);
          }
          if (Convert.ToBoolean(Category.GetCheckParameter("Derived_Hourly_Adjusted_Value_Status").ParameterValue))
              if( HIMethCd.InList( "CEM,AD" ) )
              if (HICalcAdjVal == 1 && AdjVal < 1 && cDBConvert.ToString(CurrentDHVRecord["MODC_CD"]) != "26" && !IsLegacyData)
                Category.CheckCatalogResult = "C";
              else
              {
                decimal Tolerance = GetTolerance("HI", "MMBTUHR", Category);
                if (Math.Abs(AdjVal - HICalcAdjVal) > Tolerance)
                  Category.CheckCatalogResult = "B";
              }
        }
        else
        {
          decimal[] HICalc = Category.GetCheckParameter("Apportionment_Calc_Hi_Array").ValueAsDecimalArray();

          if (HICalc[thisLocation] >= 0)
          {
            if (0 <= OpTime && OpTime <= 1)
            {
              HeatInputTotalCalcVal = HICalc[thisLocation] * OpTime;

              if (ConfigHIOpTimeAccum >= 0)
                Category.SetCheckParameter("Config_HeatInputTimesOpTime_Accumulator", HeatInputTotalCalcVal + ConfigHIOpTimeAccum, eParameterDataType.Decimal);
            }
            else
              Category.SetCheckParameter("Config_HeatInputTimesOpTime_Accumulator", -1, eParameterDataType.Decimal);
          }
          else if( !HIMethCd.InList( "ADCALC,CALC" ) )
          {
            Category.SetArrayParameter("Apportionment_Calc_Hi_Array", thisLocation, -1m);
            Category.SetCheckParameter("Config_HeatInputTimesOpTime_Accumulator", -1, eParameterDataType.Decimal);

            if (CurrMonth != "April" || AnnRptRequ)
              Category.SetArrayParameter("Rpt_Period_Hi_Calculated_Accumulator_Array", thisLocation, -1m);
          }
        }
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "HOURCV7");
      }

      return ReturnVal;
    }

    public static string HOURCV9(cCategory Category, ref bool Log)
    //Calculate SO2 Mass Emissions
    // Formerly Hourly-34
    {
      string ReturnVal = "";

      try
      {
        DataRowView HourlyOpDataRow = (DataRowView)Category.GetCheckParameter("Current_Hourly_Op_Record").ParameterValue;
        decimal OpTime = cDBConvert.ToDecimal(HourlyOpDataRow["OP_TIME"]);
        DataRowView CurrentDHVRecord = (DataRowView)Category.GetCheckParameter("Current_DHV_Record").ParameterValue;
        int thisLocation = cDBConvert.ToInteger(Category.GetCheckParameter("Current_Monitor_Plan_Location_Postion").ParameterValue);
        decimal AdjVal = cDBConvert.ToDecimal(CurrentDHVRecord["Adjusted_Hrly_Value"]);
        if (Convert.ToBoolean(Category.GetCheckParameter("Derived_Hourly_Adjusted_Value_Status").ParameterValue) && 0 <= OpTime && OpTime <= 1)
        {
          decimal SO2TotalReported = OpTime * AdjVal;
          decimal[] SO2MassRepAccum = Category.GetCheckParameter("Rpt_Period_So2_Mass_Reported_Accumulator_Array").ValueAsDecimalArray();
          if (SO2MassRepAccum[thisLocation] != decimal.MinValue)
          {
            if (SO2MassRepAccum[thisLocation] >= 0)
              Category.AccumCheckAggregate("Rpt_Period_So2_Mass_Reported_Accumulator_Array", thisLocation, SO2TotalReported);
          }
          else
            Category.SetArrayParameter("Rpt_Period_So2_Mass_Reported_Accumulator_Array", thisLocation, SO2TotalReported);
        }
        else
          Category.SetArrayParameter("Rpt_Period_So2_Mass_Reported_Accumulator_Array", thisLocation, -1m);
        decimal SO2CCalcAdjVal = cDBConvert.ToDecimal(Category.GetCheckParameter("SO2C_Calculated_Adjusted_Value").ParameterValue);
        bool DHVValid = Convert.ToBoolean(Category.GetCheckParameter("Current_DHV_Record_Valid").ParameterValue);
        decimal SO2CalcAdjVal = decimal.MinValue;
        if (Convert.ToBoolean(Category.GetCheckParameter("SO2_CEM_Method_Active_For_Hour").ParameterValue))
        {
          decimal FlowCalcAdjVal = cDBConvert.ToDecimal(Category.GetCheckParameter("FLOW_Calculated_Adjusted_Value").ParameterValue);
          decimal CalcMoisture = cDBConvert.ToDecimal(Category.GetCheckParameter("Calculated_Moisture_for_SO2").ParameterValue);
          string EquCd = cDBConvert.ToString(Category.GetCheckParameter("SO2_Equation_Code").ParameterValue);

          if (EquCd == "F-1")
            if (DHVValid && SO2CCalcAdjVal != decimal.MinValue && FlowCalcAdjVal != decimal.MinValue)
              SO2CalcAdjVal = Math.Round((decimal)0.000000166 * SO2CCalcAdjVal * FlowCalcAdjVal, 1, MidpointRounding.AwayFromZero);
            else
            {
              Category.SetArrayParameter("Rpt_Period_So2_Mass_Calculated_Accumulator_Array", thisLocation, -1m);
              Category.CheckCatalogResult = "A";
            }
          else
            if (EquCd == "F-2")
              if (DHVValid && SO2CCalcAdjVal != decimal.MinValue && FlowCalcAdjVal != decimal.MinValue && CalcMoisture != decimal.MinValue)
                SO2CalcAdjVal = Math.Round((decimal)0.000000166 * SO2CCalcAdjVal * FlowCalcAdjVal * (100 - CalcMoisture) / 100, 1, MidpointRounding.AwayFromZero);
              else
              {
                Category.SetArrayParameter("Rpt_Period_So2_Mass_Calculated_Accumulator_Array", thisLocation, -1m);
                Category.CheckCatalogResult = "A";
              }
            else
            {
              Category.SetArrayParameter("Rpt_Period_So2_Mass_Calculated_Accumulator_Array", thisLocation, -1m);
              Category.CheckCatalogResult = "A";
            }
        }
        else
          if (Convert.ToBoolean(Category.GetCheckParameter("SO2_F23_Method_Active_For_Hour").ParameterValue))
          {
            decimal HICalcAdjVal = cDBConvert.ToDecimal(Category.GetCheckParameter("HI_Calculated_Adjusted_Value").ParameterValue);
            decimal F23DefVal = cDBConvert.ToDecimal(Category.GetCheckParameter("F23_Default_Value").ParameterValue);
            if (DHVValid && F23DefVal != decimal.MinValue && HICalcAdjVal != decimal.MinValue)
              SO2CalcAdjVal = Math.Round(F23DefVal * HICalcAdjVal, 1, MidpointRounding.AwayFromZero);
            else
            {
              Category.SetArrayParameter("Rpt_Period_So2_Mass_Calculated_Accumulator_Array", thisLocation, -1m);
              Category.CheckCatalogResult = "A";
            }
          }
          else
            if (Convert.ToBoolean(Category.GetCheckParameter("SO2_App_D_Method_Active_For_Hour").ParameterValue))
            {
              decimal AppDAccum = Convert.ToDecimal(Category.GetCheckParameter("So2_App_D_Accumulator").ParameterValue);
              if (AppDAccum >= 0 && 0 < OpTime && OpTime <= 1)
              {
                SO2CalcAdjVal = AppDAccum / OpTime;
                if (Category.GetCheckParameter("Hourly_Fuel_Flow_Count_For_Gas").ValueAsInt() > 0)
                  SO2CalcAdjVal = Math.Round(SO2CalcAdjVal, 4, MidpointRounding.AwayFromZero);
                else
                  SO2CalcAdjVal = Math.Round(SO2CalcAdjVal, 1, MidpointRounding.AwayFromZero);
              }
              else
              {
                Category.SetArrayParameter("Rpt_Period_So2_Mass_Calculated_Accumulator_Array", thisLocation, -1m);
                Category.CheckCatalogResult = "A";
              }
            }
            else
            {
              if (Category.GetCheckParameter("Current_DHV_Calculated_Adjusted_Value").ParameterValue != null)
                SO2CalcAdjVal = cDBConvert.ToDecimal(Category.GetCheckParameter("Current_DHV_Calculated_Adjusted_Value").ParameterValue);
              else
                Category.SetCheckParameter("SO2_Calculated_Adjusted_Value", null, eParameterDataType.Decimal);
            }
        if (string.IsNullOrEmpty(Category.CheckCatalogResult) && SO2CalcAdjVal != decimal.MinValue)
        {
          Category.SetCheckParameter("SO2_Calculated_Adjusted_Value", SO2CalcAdjVal, eParameterDataType.Decimal);
          if (0 <= OpTime && OpTime <= 1)
          {
            decimal SO2TotalCalcValue = SO2CalcAdjVal * OpTime;
            decimal[] SO2MassCalcAccum = Category.GetCheckParameter("Rpt_Period_So2_Mass_Calculated_Accumulator_Array").ValueAsDecimalArray();
            if (SO2MassCalcAccum[thisLocation] != decimal.MinValue)
            {
              if (SO2MassCalcAccum[thisLocation] >= 0)
                Category.AccumCheckAggregate("Rpt_Period_So2_Mass_Calculated_Accumulator_Array", thisLocation, SO2TotalCalcValue);
            }
            else
              Category.SetArrayParameter("Rpt_Period_So2_Mass_Calculated_Accumulator_Array", thisLocation, SO2TotalCalcValue);
          }
          else
            Category.SetArrayParameter("Rpt_Period_So2_Mass_Calculated_Accumulator_Array", thisLocation, -1m);

          if (Category.GetCheckParameter("Derived_Hourly_Adjusted_Value_Status").AsBoolean(false))
          {
            decimal Tolerance = GetTolerance("SO2", "LBHR", Category);
            if (Math.Abs(AdjVal - SO2CalcAdjVal) > Tolerance)
              Category.CheckCatalogResult = "B";
          }
        }
        else
          Category.SetArrayParameter("Rpt_Period_So2_Mass_Calculated_Accumulator_Array", thisLocation, -1m);
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "HOURCV9");
      }

      return ReturnVal;
    }

    #endregion

    #region Checks 11 - 20

    public static string HOURCV12(cCategory Category, ref bool Log)
    //Determine Diluent Cap, Moisture, and NOXC for NOx Rate Calculation Verification        
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentDHVRecord = (DataRowView)Category.GetCheckParameter("Current_DHV_Record").ParameterValue;
        string ModcCd = cDBConvert.ToString(CurrentDHVRecord["MODC_CD"]);
        if( Convert.ToString( Category.GetCheckParameter( "Current_NOxR_Method_Code" ).ParameterValue ) == "CEM" && ModcCd.InList( "01,02,03,04,14,22,53,54" ) )
        {
          DataRowView CurrentNOxCMHVRecord = (DataRowView)Category.GetCheckParameter("Current_Nox_Conc_Monitor_Hourly_Record").ParameterValue;
          if (Convert.ToInt16(Category.GetCheckParameter("NOxC_Monitor_Hourly_Count").ParameterValue) == 1 && CurrentNOxCMHVRecord["UNADJUSTED_HRLY_VALUE"] != DBNull.Value)
            Category.SetCheckParameter("Nox_Conc_For_Nox_Rate_Calculation", cDBConvert.ToDecimal(CurrentNOxCMHVRecord["UNADJUSTED_HRLY_VALUE"]), eParameterDataType.Decimal);
          else
            Category.SetCheckParameter("Nox_Conc_For_Nox_Rate_Calculation", null, eParameterDataType.Decimal);
          string NOxREquCd = Convert.ToString(Category.GetCheckParameter("Nox_Rate_Equation_Code").ParameterValue);
          if( NOxREquCd.InList( "19-3,19-3D,19-4,19-5,19-8,19-9" ) )
          {
            decimal H2ODHVCalcAdjVal = cDBConvert.ToDecimal(Category.GetCheckParameter("H2O_DHV_Calculated_Adjusted_Value").ParameterValue);
            decimal H2OMHVCalcAdjVal = cDBConvert.ToDecimal(Category.GetCheckParameter("H2O_MHV_Calculated_Adjusted_Value").ParameterValue);
            decimal CalcMoistureNOxR = decimal.MinValue;
            decimal H2ODefVal = cDBConvert.ToDecimal(Category.GetCheckParameter("H2O_Default_Value").ParameterValue);
            string H2OMethCd = Convert.ToString(Category.GetCheckParameter("H2O_Method_Code").ParameterValue);
            if (H2OMethCd == "MWD" && Convert.ToBoolean(Category.GetCheckParameter("H2O_Derived_Hourly_Checks_Needed").ParameterValue) && H2ODHVCalcAdjVal != decimal.MinValue)
              CalcMoistureNOxR = H2ODHVCalcAdjVal;
            else
                if( H2OMethCd.InList( "MMS,MTB" ) && Convert.ToBoolean( Category.GetCheckParameter( "H2O_Monitor_Hourly_Checks_Needed" ).ParameterValue ) && H2OMHVCalcAdjVal != decimal.MinValue )
                CalcMoistureNOxR = H2OMHVCalcAdjVal;
              else
                if (H2OMethCd == "MDF" && Convert.ToBoolean(Category.GetCheckParameter("H2O_Derived_Hourly_Checks_Needed").ParameterValue) && H2ODHVCalcAdjVal != decimal.MinValue)
                  CalcMoistureNOxR = H2ODHVCalcAdjVal;
                else
                  if (H2OMethCd == "MDF" && !Convert.ToBoolean(Category.GetCheckParameter("H2O_Derived_Hourly_Checks_Needed").ParameterValue) && H2ODefVal != decimal.MinValue)
                    CalcMoistureNOxR = H2ODefVal;
            Category.SetCheckParameter("Calculated_Moisture_for_NOXR", CalcMoistureNOxR, eParameterDataType.Decimal);
          }
          if( NOxREquCd.InList( "19-3D,19-5D" ) || cDBConvert.ToString( CurrentDHVRecord["MODC_CD"] ) == "14" )
          {
            DataView MonDefRecs = (DataView)Category.GetCheckParameter("Monitor_Default_Records_by_Hour_Location").ParameterValue;
            sFilterPair[] FilterDef;
            if( NOxREquCd.InList( "F-5,19-1,19-2,19-3,19-3D,19-4,19-5,19-5D" ) )
            {
              FilterDef = new sFilterPair[3];
              FilterDef[0].Set("DEFAULT_PURPOSE_CD", "DC");
              FilterDef[1].Set("PARAMETER_CD", "O2X");
              FilterDef[2].Set("FUEL_CD", "NFS");
              MonDefRecs = FindRows(MonDefRecs, FilterDef);
              if (MonDefRecs.Count > 1)
                Category.CheckCatalogResult = "A";
              else
                if (MonDefRecs.Count == 0)
                  Category.CheckCatalogResult = "B";
                else
                {
                  decimal MonDefVal = cDBConvert.ToDecimal(MonDefRecs[0]["DEFAULT_VALUE"]);
                  if (MonDefVal <= 0)
                    Category.CheckCatalogResult = "C";
                  else
                    Category.SetCheckParameter("Calculated_Diluent_for_NOXR", MonDefVal, eParameterDataType.Decimal);
                }
            }
            else
                if( NOxREquCd.InList( "F-6,19-6,19-7,19-8,19-9" ) )
              {
                FilterDef = new sFilterPair[3];
                FilterDef[0].Set("DEFAULT_PURPOSE_CD", "DC");
                FilterDef[1].Set("PARAMETER_CD", "CO2N");
                FilterDef[2].Set("FUEL_CD", "NFS");
                MonDefRecs = FindRows(MonDefRecs, FilterDef);
                if (MonDefRecs.Count > 1)
                  Category.CheckCatalogResult = "D";
                else
                  if (MonDefRecs.Count == 0)
                    Category.CheckCatalogResult = "E";
                  else
                  {
                    decimal MonDefVal = cDBConvert.ToDecimal(MonDefRecs[0]["DEFAULT_VALUE"]);
                    if (MonDefVal <= 0)
                      Category.CheckCatalogResult = "F";
                    else
                      Category.SetCheckParameter("Calculated_Diluent_for_NOXR", MonDefVal, eParameterDataType.Decimal);
                  }
              }
          }
          else
              if( NOxREquCd.InList( "F-5,19-1,19-4" ) && Convert.ToBoolean( Category.GetCheckParameter( "O2_Dry_Checks_Needed_For_Nox_Rate_Calc" ).ParameterValue ) )
              Category.SetCheckParameter("Calculated_Diluent_for_NOXR", Convert.ToDecimal(Category.GetCheckParameter("O2_Dry_Calculated_Adjusted_Value").ParameterValue), eParameterDataType.Decimal);
            else
                  if( NOxREquCd.InList( "19-2,19-3,19-5" ) && Convert.ToBoolean( Category.GetCheckParameter( "O2_Wet_Checks_Needed_For_Nox_Rate_Calc" ).ParameterValue ) )
                Category.SetCheckParameter("Calculated_Diluent_for_NOXR", Convert.ToDecimal(Category.GetCheckParameter("O2_Wet_Calculated_Adjusted_Value").ParameterValue), eParameterDataType.Decimal);
              else
                      if( NOxREquCd.InList( "F-6,19-6,19-7,19-8,19-9" ) && Convert.ToBoolean( Category.GetCheckParameter( "CO2_Conc_Monitor_Checks_Needed" ).ParameterValue ) )
                  Category.SetCheckParameter("Calculated_Diluent_for_NOXR", Convert.ToDecimal(Category.GetCheckParameter("CO2C_MHV_Calculated_Adjusted_Value").ParameterValue), eParameterDataType.Decimal);

        }
        else
          Log = false;
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "HOURCV12");
      }

      return ReturnVal;
    }

    public static string HOURCV13(cCategory Category, ref bool Log)
    // Calculate NOx Emissions Rate
    // Formerly Hourly-115
    {
      string ReturnVal = "";

      try
      {
        decimal NoxrCalcUnadjustedValue = decimal.MinValue;
        Category.SetCheckParameter("Derived_Hourly_Unadjusted_Calculation_Status", false, eParameterDataType.Boolean);
        DataRowView CurrentDhvRecord = Category.GetCheckParameter("Current_DHV_Record").ValueAsDataRowView();

        string ModcCd = cDBConvert.ToString(CurrentDhvRecord["MODC_CD"]);
        decimal UnadjustedHourlyValue = cDBConvert.ToDecimal(CurrentDhvRecord["UNADJUSTED_HRLY_VALUE"]);
        string NOxRMethCode = Convert.ToString(Category.GetCheckParameter("Current_NOxR_Method_Code").ParameterValue);
        if ((NOxRMethCode == "CEM") && ModcCd.InList("01,02,03,14,22,53"))
        {
          decimal CalcDiluentForNoxr = Category.GetCheckParameter("Calculated_Diluent_for_NOXR").ValueAsDecimal();
          decimal CalcMoistureForNoxr = Category.GetCheckParameter("Calculated_Moisture_for_NOxR").ValueAsDecimal();
          bool CurrentDhvRecordValid = Category.GetCheckParameter("Current_DHV_Record_Valid").ValueAsBool();
          DataRowView CurrentHourlyOpRecord = Category.GetCheckParameter("Current_Hourly_Op_Record").ValueAsDataRowView();
          decimal NoxcForNoxrCalc = Category.GetCheckParameter("Nox_Conc_For_Nox_Rate_Calculation").ValueAsDecimal();
          string NoxrEquationCd = Category.GetCheckParameter("Nox_Rate_Equation_Code").ValueAsString();
          bool ValidFcFactorExists = Category.GetCheckParameter("Valid_Fc_Factor_Exists").ValueAsBool();
          bool ValidFdFactorExists = Category.GetCheckParameter("Valid_Fd_Factor_Exists").ValueAsBool();
          bool ValidFwFactorExists = Category.GetCheckParameter("Valid_Fw_Factor_Exists").ValueAsBool();
          if (cDBConvert.ToString(CurrentDhvRecord["SYS_TYPE_CD"]) == "NOX")
            Category.SetCheckParameter("RATA_Status_Required", true, eParameterDataType.Boolean);
          switch (NoxrEquationCd)
          {
            case "19-1":
              if (CurrentDhvRecordValid && (CalcDiluentForNoxr != decimal.MinValue) &&
                  (NoxcForNoxrCalc != decimal.MinValue) && ValidFdFactorExists)
              {
                if (CalcDiluentForNoxr == 20.9m)
                  Category.CheckCatalogResult = "A";
                else
                  NoxrCalcUnadjustedValue
                    = Math.Round(0.0000001194m * NoxcForNoxrCalc *
                                 cDBConvert.ToDecimal(CurrentHourlyOpRecord["FD_Factor"]) *
                                 (20.9m / (20.9m - CalcDiluentForNoxr)), 3, MidpointRounding.AwayFromZero);
              }
              else
                Category.CheckCatalogResult = "B";
              break;

            case "F-5": goto case "19-1";

            case "19-2":
              decimal MoistureFraction = decimal.MinValue;
              DataView MonDefRecs = (DataView)Category.GetCheckParameter("Monitor_Default_Records_by_Hour_Location").ParameterValue;
              sFilterPair[] FilterDef = new sFilterPair[1];
              FilterDef[0].Set("PARAMETER_CD", "BWA");
              MonDefRecs = FindRows(MonDefRecs, FilterDef);
              if (MonDefRecs.Count == 0)
                MoistureFraction = 0.027m;
              else
                if (MonDefRecs.Count == 1 && 0 < cDBConvert.ToDecimal(MonDefRecs[0]["DEFAULT_VALUE"]) && cDBConvert.ToDecimal(MonDefRecs[0]["DEFAULT_VALUE"]) < 1)
                  MoistureFraction = cDBConvert.ToDecimal(MonDefRecs[0]["DEFAULT_VALUE"]);
                else
                  Category.CheckCatalogResult = "D";
              if (string.IsNullOrEmpty(Category.CheckCatalogResult))
                if (CurrentDhvRecordValid && (CalcDiluentForNoxr != decimal.MinValue) &&
                    (NoxcForNoxrCalc != decimal.MinValue) && ValidFwFactorExists && MoistureFraction != decimal.MinValue)
                {
                  if (CalcDiluentForNoxr == 20.9m * (1 - 0.027m))
                    Category.CheckCatalogResult = "A";
                  else
                    NoxrCalcUnadjustedValue
                      = Math.Round(0.0000001194m * NoxcForNoxrCalc *
                                   cDBConvert.ToDecimal(CurrentHourlyOpRecord["FW_Factor"]) *
                                   (20.9m / (20.9m * (1 - 0.027m) - CalcDiluentForNoxr)), 3, MidpointRounding.AwayFromZero);
                }
                else
                  Category.CheckCatalogResult = "B";
              break;

            case "19-3":
              if (CurrentDhvRecordValid && (CalcDiluentForNoxr != decimal.MinValue) &&
                  (NoxcForNoxrCalc != decimal.MinValue) && ValidFdFactorExists &&
                  (CalcMoistureForNoxr != decimal.MinValue))
              {
                decimal TempVal = 20.9m * (100 - CalcMoistureForNoxr) / 100;

                if (CalcDiluentForNoxr == TempVal)
                  Category.CheckCatalogResult = "A";
                else
                  NoxrCalcUnadjustedValue
                    = Math.Round(0.0000001194m * NoxcForNoxrCalc *
                                 cDBConvert.ToDecimal(CurrentHourlyOpRecord["FD_Factor"]) *
                                 (20.9m / (TempVal - CalcDiluentForNoxr)), 3, MidpointRounding.AwayFromZero);
              }
              else
                Category.CheckCatalogResult = "B";
              break;

            case "19-3D":
              if (CurrentDhvRecordValid && (CalcDiluentForNoxr != decimal.MinValue) &&
                  (NoxcForNoxrCalc != decimal.MinValue) && ValidFdFactorExists &&
                  (CalcMoistureForNoxr != decimal.MinValue))
              {
                decimal H2oFactor = (100 - CalcMoistureForNoxr) / 100;
                decimal DenomTerm = (20.9m * H2oFactor) - (CalcDiluentForNoxr * H2oFactor);

                if (DenomTerm == 0)
                  Category.CheckCatalogResult = "A";
                else
                  NoxrCalcUnadjustedValue
                    = Math.Round(0.0000001194m * NoxcForNoxrCalc *
                                 cDBConvert.ToDecimal(CurrentHourlyOpRecord["FD_Factor"]) *
                                 20.9m / DenomTerm, 3, MidpointRounding.AwayFromZero);
              }
              else
                Category.CheckCatalogResult = "B";
              break;

            case "19-4":
              if (CurrentDhvRecordValid && (CalcDiluentForNoxr != decimal.MinValue) &&
                  (NoxcForNoxrCalc != decimal.MinValue) && ValidFdFactorExists &&
                  (CalcMoistureForNoxr != decimal.MinValue))
              {
                if ((CalcDiluentForNoxr == 20.9m) || (CalcMoistureForNoxr == 100))
                  Category.CheckCatalogResult = "A";
                else
                  NoxrCalcUnadjustedValue
                    = Math.Round(0.0000001194m * NoxcForNoxrCalc *
                                 cDBConvert.ToDecimal(CurrentHourlyOpRecord["FD_Factor"]) /
                                 ((100 - CalcMoistureForNoxr) / 100) *
                                 (20.9m / (20.9m - CalcDiluentForNoxr)), 3, MidpointRounding.AwayFromZero);
              }
              else
                Category.CheckCatalogResult = "B";
              break;

            case "19-5":
              if (CurrentDhvRecordValid && (CalcDiluentForNoxr != decimal.MinValue) &&
                  (NoxcForNoxrCalc != decimal.MinValue) && ValidFdFactorExists &&
                  (CalcMoistureForNoxr != decimal.MinValue))
              {
                if (CalcMoistureForNoxr == 100)
                  Category.CheckCatalogResult = "A";
                else
                {
                  decimal H2oFactor = (100 - CalcMoistureForNoxr) / 100;
                  decimal DenomTerm = 20.9m - CalcDiluentForNoxr / H2oFactor;

                  if (DenomTerm == 0)
                    Category.CheckCatalogResult = "A";
                  else
                    NoxrCalcUnadjustedValue
                      = Math.Round(0.0000001194m * NoxcForNoxrCalc *
                                   cDBConvert.ToDecimal(CurrentHourlyOpRecord["FD_Factor"]) /
                                   DenomTerm, 3, MidpointRounding.AwayFromZero);
                }
              }
              else
                Category.CheckCatalogResult = "B";
              break;

            case "19-5D":
              if (CurrentDhvRecordValid && (CalcDiluentForNoxr != decimal.MinValue) &&
                  (NoxcForNoxrCalc != decimal.MinValue) && ValidFdFactorExists)
              {
                if (CalcDiluentForNoxr == 20.9m)
                  Category.CheckCatalogResult = "A";
                else
                  NoxrCalcUnadjustedValue
                    = Math.Round(0.0000001194m * NoxcForNoxrCalc *
                                 cDBConvert.ToDecimal(CurrentHourlyOpRecord["FD_Factor"]) *
                                 20.9m / (20.9m - CalcDiluentForNoxr), 3, MidpointRounding.AwayFromZero);
              }
              else
                Category.CheckCatalogResult = "B";
              break;

            case "19-6":
              if (CurrentDhvRecordValid && (CalcDiluentForNoxr != decimal.MinValue) &&
                  (NoxcForNoxrCalc != decimal.MinValue) && ValidFcFactorExists)
              {
                if (CalcDiluentForNoxr == 0)
                  Category.CheckCatalogResult = "A";
                else
                  NoxrCalcUnadjustedValue
                    = Math.Round(0.0000001194m * NoxcForNoxrCalc *
                                 cDBConvert.ToDecimal(CurrentHourlyOpRecord["FC_Factor"]) *
                                 100 / CalcDiluentForNoxr, 3, MidpointRounding.AwayFromZero);
              }
              else
                Category.CheckCatalogResult = "B";
              break;

            case "19-7": goto case "19-6";

            case "F-6": goto case "19-6";

            case "19-8":
              if (CurrentDhvRecordValid && (CalcDiluentForNoxr != decimal.MinValue) &&
                  (NoxcForNoxrCalc != decimal.MinValue) && ValidFcFactorExists &&
                  (CalcMoistureForNoxr != decimal.MinValue))
              {
                if (CalcDiluentForNoxr == 0 || CalcMoistureForNoxr == 100)
                  Category.CheckCatalogResult = "A";
                else
                  NoxrCalcUnadjustedValue
                    = Math.Round(0.0000001194m * NoxcForNoxrCalc *
                                 cDBConvert.ToDecimal(CurrentHourlyOpRecord["FC_Factor"]) /
                                 ((100 - CalcMoistureForNoxr) / 100) *
                                 (100 / CalcDiluentForNoxr), 3, MidpointRounding.AwayFromZero);
              }
              else
                Category.CheckCatalogResult = "B";
              break;

            case "19-9":
              if (CurrentDhvRecordValid && (CalcDiluentForNoxr != decimal.MinValue) &&
                  (NoxcForNoxrCalc != decimal.MinValue) && ValidFcFactorExists &&
                  (CalcMoistureForNoxr != decimal.MinValue))
              {
                if (CalcDiluentForNoxr == 0)
                  Category.CheckCatalogResult = "A";
                else
                {
                  decimal H2oTerm = (100 - CalcMoistureForNoxr) / 100;
                  decimal Co2Term = 100 / CalcDiluentForNoxr;
                  NoxrCalcUnadjustedValue
                    = Math.Round(0.0000001194m * NoxcForNoxrCalc *
                                 cDBConvert.ToDecimal(CurrentHourlyOpRecord["FC_Factor"]) *
                                 H2oTerm * Co2Term, 3, MidpointRounding.AwayFromZero);
                }
              }
              else
                Category.CheckCatalogResult = "B";
              break;
          }
          if (string.IsNullOrEmpty(Category.CheckCatalogResult) && Category.GetCheckParameter("Derived_Hourly_Unadjusted_Value_Status").ValueAsBool() &&
              (NoxrCalcUnadjustedValue != decimal.MinValue))
          {
            decimal Tolerance = GetTolerance("NOXR", "LBMMBTU", Category);

            if (Math.Abs(UnadjustedHourlyValue - NoxrCalcUnadjustedValue) > Tolerance)
              Category.CheckCatalogResult = "C";
            else
              Category.SetCheckParameter("Derived_Hourly_Unadjusted_Calculation_Status", true, eParameterDataType.Boolean);
          }
        }
        else
        {
            if( ( NOxRMethCode == "PEM" ) && ModcCd.InList( "01,02,03" ) )
          {
            if (cDBConvert.ToString(CurrentDhvRecord["SYS_TYPE_CD"]) == "NOXP")
              Category.SetCheckParameter("RATA_Status_Required", true, eParameterDataType.Boolean);
            if (UnadjustedHourlyValue >= 0)
            {
              NoxrCalcUnadjustedValue = UnadjustedHourlyValue;
              Category.SetCheckParameter("Derived_Hourly_Unadjusted_Calculation_Status", true, eParameterDataType.Boolean);
            }
          }
          else if (NOxRMethCode == "AE")
          {
            if (Category.GetCheckParameter("App_E_Constant_Fuel_Mix").ValueAsBool())
              Category.SetCheckParameter("NOXR_Calculated_Adjusted_Value", Category.GetCheckParameter("App_E_Calculated_Nox_Rate_For_Source").ValueAsDecimal(), eParameterDataType.Decimal);
          }
          else
          {
            Category.SetCheckParameter("NOXR_Calculated_Adjusted_Value", Category.GetCheckParameter("Current_DHV_Calculated_Adjusted_Value").ValueAsDecimal(), eParameterDataType.Decimal);
            if( NOxRMethCode.InList( "PEM,CEM" ) && ModcCd == "21" )
            {
                if( cDBConvert.ToString( CurrentDhvRecord["SYS_TYPE_CD"] ).InList( "NOX,NOXP" ) )
                Category.SetCheckParameter("RATA_Status_Required", true, eParameterDataType.Boolean);
            }
          }
        }
        Category.SetCheckParameter("NOXR_Calculated_Unadjusted_Value", NoxrCalcUnadjustedValue, eParameterDataType.Decimal);
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "HOURCV13");
      }

      return ReturnVal;
    }

    public static string HOURCV15(cCategory Category, ref bool Log)
    // Pre-Verify NOx Mass Calculation Requirements
    // Formerly Hourly-124
    {
      string ReturnVal = "";

      try
      {
        if (Convert.ToString(Category.GetCheckParameter("NOx_Mass_Monitor_Method_Code").ParameterValue) == "CEM")
          if (Convert.ToString(Category.GetCheckParameter("NOx_Mass_Equation_Code").ParameterValue) == "F-26B")
          {
            decimal H2ODHVCalcAdjVal = cDBConvert.ToDecimal(Category.GetCheckParameter("H2O_DHV_Calculated_Adjusted_Value").ParameterValue);
            decimal H2OMHVCalcAdjVal = cDBConvert.ToDecimal(Category.GetCheckParameter("H2O_MHV_Calculated_Adjusted_Value").ParameterValue);
            decimal H2ODefVal = cDBConvert.ToDecimal(Category.GetCheckParameter("H2O_Default_Value").ParameterValue);
            string H2OMethCd = Convert.ToString(Category.GetCheckParameter("H2O_Method_Code").ParameterValue);
            decimal CalcMoistureNOx = decimal.MinValue;
            if (H2OMethCd == "MWD" && Convert.ToBoolean(Category.GetCheckParameter("H2O_Derived_Hourly_Checks_Needed").ParameterValue) && H2ODHVCalcAdjVal != decimal.MinValue)
              CalcMoistureNOx = H2ODHVCalcAdjVal;
            else
                if( H2OMethCd.InList( "MMS,MTB" ) && Convert.ToBoolean( Category.GetCheckParameter( "H2O_Monitor_Hourly_Checks_Needed" ).ParameterValue ) && H2OMHVCalcAdjVal != decimal.MinValue )
                CalcMoistureNOx = H2OMHVCalcAdjVal;
              else
                if (H2OMethCd == "MDF" && Convert.ToBoolean(Category.GetCheckParameter("H2O_Derived_Hourly_Checks_Needed").ParameterValue) && H2ODHVCalcAdjVal != decimal.MinValue)
                  CalcMoistureNOx = H2ODHVCalcAdjVal;
                else
                  if (H2OMethCd == "MDF" && !Convert.ToBoolean(Category.GetCheckParameter("H2O_Derived_Hourly_Checks_Needed").ParameterValue) && H2ODefVal != decimal.MinValue)
                    CalcMoistureNOx = H2ODefVal;
            if (CalcMoistureNOx != decimal.MinValue)
              Category.SetCheckParameter("Calculated_Moisture_for_NOX", CalcMoistureNOx, eParameterDataType.Decimal);
          }
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "HOURCV15");
      }

      return ReturnVal;
    }

    public static string HOURCV16(cCategory Category, ref bool Log)
    //Calculate NOx Mass Emissions        
    {
      string ReturnVal = "";

      try
      {
        DataRowView HourlyOpDataRow = (DataRowView)Category.GetCheckParameter("Current_Hourly_Op_Record").ParameterValue;
        decimal OpTime = cDBConvert.ToDecimal(HourlyOpDataRow["OP_TIME"]);
        DataRowView CurrentDHVRecord = (DataRowView)Category.GetCheckParameter("Current_DHV_Record").ParameterValue;
        int thisLocation = cDBConvert.ToInteger(Category.GetCheckParameter("Current_Monitor_Plan_Location_Postion").ParameterValue);
        decimal AdjVal = cDBConvert.ToDecimal(CurrentDHVRecord["Adjusted_Hrly_Value"]);

        bool AnnRptRequ = Convert.ToBoolean(Category.GetCheckParameter("Annual_Reporting_Requirement").ParameterValue);
        DateTime CurrentOpDate = Category.GetCheckParameter("Current_Operating_Date").ValueAsDateTime(DateTypes.START);
        string CurrMonth = Category.GetCheckParameter("Current_Month").ValueAsString();

        if (Convert.ToBoolean(Category.GetCheckParameter("Derived_Hourly_Adjusted_Value_Status").ParameterValue) && 0 <= OpTime && OpTime <= 1)
        {
          decimal NOxMassTotalReported = OpTime * AdjVal;
          decimal[] NOxAccum = Category.GetCheckParameter("Rpt_Period_Nox_Mass_Reported_Accumulator_Array").ValueAsDecimalArray();
          if (CurrMonth != "April" || AnnRptRequ)
            if (NOxAccum[thisLocation] != decimal.MinValue)
            {
              if (NOxAccum[thisLocation] >= 0)
                Category.AccumCheckAggregate("Rpt_Period_Nox_Mass_Reported_Accumulator_Array", thisLocation, NOxMassTotalReported);
            }
            else
              Category.SetArrayParameter("Rpt_Period_Nox_Mass_Reported_Accumulator_Array", thisLocation, NOxMassTotalReported);
        }
        else
          if (CurrMonth != "April" || AnnRptRequ)
            Category.SetArrayParameter("Rpt_Period_Nox_Mass_Reported_Accumulator_Array", thisLocation, -1m);
        decimal NOxCCalcAdjVal = cDBConvert.ToDecimal(Category.GetCheckParameter("NOxC_Calculated_Adjusted_Value").ParameterValue);
        string NoxMassMonitorMethodCd = Convert.ToString(Category.GetCheckParameter("NOx_Mass_Monitor_Method_Code").ParameterValue);
        decimal NOxCalcAdjVal = decimal.MinValue;
        string[] HIMeth = Category.GetCheckParameter("Apportionment_HI_Method_Array").ValueAsStringArray();
        string NOxMEqnCd = (string)Category.GetCheckParameter("NOx_Mass_Equation_Code").ParameterValue;

        if( NoxMassMonitorMethodCd.InList( "CEM,NOXR,CEMNOXR" ) )
        {
          bool DHVValid = Convert.ToBoolean(Category.GetCheckParameter("Current_DHV_Record_Valid").ParameterValue);
          decimal FlowCalcAdjVal = cDBConvert.ToDecimal(Category.GetCheckParameter("FLOW_Calculated_Adjusted_Value").ParameterValue);
          decimal CalcMoisture = cDBConvert.ToDecimal(Category.GetCheckParameter("Calculated_Moisture_for_NOx").ParameterValue);

          if (NOxMEqnCd == "F-26A")
          {
            if (DHVValid && NOxCCalcAdjVal != decimal.MinValue && FlowCalcAdjVal != decimal.MinValue)
              NOxCalcAdjVal = Math.Round(0.0000001194m * NOxCCalcAdjVal * FlowCalcAdjVal, 1, MidpointRounding.AwayFromZero);
            else
            {
              if (CurrMonth != "April" || AnnRptRequ)
                Category.SetArrayParameter("Rpt_Period_Nox_Mass_Calculated_Accumulator_Array", thisLocation, -1m);
              Category.CheckCatalogResult = "A";
            }
          }
          else
            if (NOxMEqnCd == "F-26B")
            {
              if (DHVValid && NOxCCalcAdjVal != decimal.MinValue && FlowCalcAdjVal != decimal.MinValue && CalcMoisture != decimal.MinValue)
                NOxCalcAdjVal = Math.Round(0.0000001194m * NOxCCalcAdjVal * FlowCalcAdjVal * (100 - CalcMoisture) / 100, 1, MidpointRounding.AwayFromZero);
              else
              {
                if (CurrMonth != "April" || AnnRptRequ)
                  Category.SetArrayParameter("Rpt_Period_Nox_Mass_Calculated_Accumulator_Array", thisLocation, -1m);
                Category.CheckCatalogResult = "A";
              }
            }
            else
              if (NOxMEqnCd == "F-24A")
              {
                  if( !Convert.ToString( Category.GetCheckParameter( "Heat_Input_Method_Code" ).ParameterValue ).InList( "CALC,ADCALC" ) )
                {
                  decimal NOxRCalcAdjVal = cDBConvert.ToDecimal(Category.GetCheckParameter("NOXR_Calculated_Adjusted_Value").ParameterValue);

                  if (DHVValid && NOxRCalcAdjVal != decimal.MinValue)
                  {
                    decimal HICalcAdjVal = cDBConvert.ToDecimal(Category.GetCheckParameter("HI_Calculated_Adjusted_Value").ParameterValue);
                    if (HICalcAdjVal != decimal.MinValue)
                      NOxCalcAdjVal = Math.Round(NOxRCalcAdjVal * HICalcAdjVal, 1, MidpointRounding.AwayFromZero);
                    else
                    {
                      if (CurrMonth != "April" || AnnRptRequ)
                        Category.SetArrayParameter("Rpt_Period_Nox_Mass_Calculated_Accumulator_Array", thisLocation, -1m);
                      Category.CheckCatalogResult = "A";
                    }
                  }
                  else
                  {
                    if (CurrMonth != "April" || AnnRptRequ)
                      Category.SetArrayParameter("Rpt_Period_Nox_Mass_Calculated_Accumulator_Array", thisLocation, -1m);
                    Category.CheckCatalogResult = "A";
                  }
                }
              }
              else
              {
                if (CurrMonth != "April" || AnnRptRequ)
                  Category.SetArrayParameter("Rpt_Period_Nox_Mass_Calculated_Accumulator_Array", thisLocation, -1m);
                Category.CheckCatalogResult = "A";
              }
        }
        else
        {
          if (Category.GetCheckParameter("Current_DHV_Calculated_Adjusted_Value").ParameterValue != null)
            NOxCalcAdjVal = cDBConvert.ToDecimal(Category.GetCheckParameter("Current_DHV_Calculated_Adjusted_Value").ParameterValue);
          else
            Category.SetCheckParameter("NOx_Calculated_Adjusted_Value", null, eParameterDataType.Decimal);
        }
        if (string.IsNullOrEmpty(Category.CheckCatalogResult))
          if (NOxCalcAdjVal != decimal.MinValue)
          {
            Category.SetCheckParameter("NOx_Calculated_Adjusted_Value", NOxCalcAdjVal, eParameterDataType.Decimal);
            if (0 <= OpTime && OpTime <= 1)
            {
              decimal NOxMTotalCalcValue = NOxCalcAdjVal * OpTime;
              decimal[] NOxMassAccum = Category.GetCheckParameter("Rpt_Period_Nox_Mass_Calculated_Accumulator_Array").ValueAsDecimalArray();
              if (CurrMonth != "April" || AnnRptRequ)
              {
                if (NOxMassAccum[thisLocation] != decimal.MinValue)
                {
                  if (NOxMassAccum[thisLocation] >= 0)
                    Category.AccumCheckAggregate("Rpt_Period_Nox_Mass_Calculated_Accumulator_Array", thisLocation, NOxMTotalCalcValue);
                }
                else
                  Category.SetArrayParameter("Rpt_Period_Nox_Mass_Calculated_Accumulator_Array", thisLocation, NOxMTotalCalcValue);

                if (CurrMonth == "April")
                {
                  decimal[] AprilNOxMassAccum = Category.GetCheckParameter("April_NOX_Mass_Calculated_Accumulator_Array").ValueAsDecimalArray();
                  if (AprilNOxMassAccum[thisLocation] != decimal.MinValue)
                  {
                    if (AprilNOxMassAccum[thisLocation] >= 0)
                      Category.AccumCheckAggregate("April_NOX_Mass_Calculated_Accumulator_Array", thisLocation, NOxMTotalCalcValue);
                  }
                  else
                    Category.SetArrayParameter("April_NOX_Mass_Calculated_Accumulator_Array", thisLocation, NOxMTotalCalcValue);
                }
              }
            }
            else
              if (CurrMonth != "April" || AnnRptRequ)
                Category.SetArrayParameter("Rpt_Period_Nox_Mass_Calculated_Accumulator_Array", thisLocation, -1m);
            if (Convert.ToBoolean(Category.GetCheckParameter("Derived_Hourly_Adjusted_Value_Status").ParameterValue) &&
                NoxMassMonitorMethodCd.InList( "CEM,NOXR,CEMNOXR" ) )
            {
              decimal Tolerance = GetTolerance("NOX", "LBHR", Category);
              if (Math.Abs(AdjVal - NOxCalcAdjVal) > Tolerance)
                if (!Category.GetCheckParameter("Legacy_Data_Evaluation").ValueAsBool())
                  Category.CheckCatalogResult = "B";
                else
                  if (0 < OpTime && OpTime <= 1)
                    if (Math.Abs(AdjVal - NOxCalcAdjVal) > Tolerance / OpTime)
                      Category.CheckCatalogResult = "B";
            }
          }
          else
            if (NOxMEqnCd != "F-24A" || !Convert.ToBoolean(Category.GetCheckParameter("Current_DHV_Record_Valid").ParameterValue) ||
                Category.GetCheckParameter( "NOXR_Calculated_Adjusted_Value" ).ParameterValue == null || !Convert.ToString( Category.GetCheckParameter( "Heat_Input_Method_Code" ).ParameterValue ).InList( "CALC,ADCALC" ) )
              if (CurrMonth != "April" || AnnRptRequ)
                Category.SetArrayParameter("Rpt_Period_Nox_Mass_Calculated_Accumulator_Array", thisLocation, -1m);
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "HOURCV16");
      }

      return ReturnVal;
    }

    public static string HOURCV18(cCategory Category, ref bool Log)
    //Determine Diluent Cap and Moisture for CO2 Mass Calculation Verification
    {
      string ReturnVal = "";

      try
      {
        string EquCd = Convert.ToString(Category.GetCheckParameter("CO2_Mass_Cem_Equation_Code").ParameterValue);
        if (Convert.ToString(Category.GetCheckParameter("CO2_Method_Code").ParameterValue) == "CEM")
        {
          string MethCd = Convert.ToString(Category.GetCheckParameter("H2O_Method_Code").ParameterValue);
          if (Convert.ToString(Category.GetCheckParameter("CO2_Mass_Cem_Equation_Code").ParameterValue) == "F-2")
          {
            if (MethCd == "MWD" && Convert.ToBoolean(Category.GetCheckParameter("H2O_Derived_Hourly_Checks_Needed").ParameterValue) &&
                Category.GetCheckParameter("H2O_DHV_Calculated_Adjusted_Value").ParameterValue != null)
              Category.SetCheckParameter("Calculated_Moisture_for_CO2", cDBConvert.ToDecimal(Category.GetCheckParameter("H2O_DHV_Calculated_Adjusted_Value").ParameterValue), eParameterDataType.Decimal);
            else
                if( MethCd.InList( "MMS,MTB" ) && Convert.ToBoolean( Category.GetCheckParameter( "H2O_Monitor_Hourly_Checks_Needed" ).ParameterValue ) &&
                  Category.GetCheckParameter("H2O_MHV_Calculated_Adjusted_Value").ParameterValue != null)
                Category.SetCheckParameter("Calculated_Moisture_for_CO2", cDBConvert.ToDecimal(Category.GetCheckParameter("H2O_MHV_Calculated_Adjusted_Value").ParameterValue), eParameterDataType.Decimal);
              else
                if (MethCd == "MDF" && Convert.ToBoolean(Category.GetCheckParameter("H2O_Derived_Hourly_Checks_Needed").ParameterValue) &&
                    Category.GetCheckParameter("H2O_DHV_Calculated_Adjusted_Value").ParameterValue != null)
                  Category.SetCheckParameter("Calculated_Moisture_for_CO2", cDBConvert.ToDecimal(Category.GetCheckParameter("H2O_DHV_Calculated_Adjusted_Value").ParameterValue), eParameterDataType.Decimal);
                else
                {
                  decimal DefVal = cDBConvert.ToDecimal(Category.GetCheckParameter("H2O_Default_Value").ParameterValue);
                  if (MethCd == "MDF" && !Convert.ToBoolean(Category.GetCheckParameter("H2O_Derived_Hourly_Checks_Needed").ParameterValue) &&
                      DefVal != decimal.MinValue)
                    Category.SetCheckParameter("Calculated_Moisture_for_CO2", DefVal, eParameterDataType.Decimal);
                }
          }
          if (Convert.ToBoolean(Category.GetCheckParameter("Use_CO2_Diluent_Cap_for_Co2_Mass_Calc").ParameterValue))
          {
            DataView MonDefRecs = (DataView)Category.GetCheckParameter("Monitor_Default_Records_by_Hour_Location").ParameterValue;
            sFilterPair[] FilterDef = new sFilterPair[3];
            FilterDef[0].Set("DEFAULT_PURPOSE_CD", "DC");
            FilterDef[1].Set("PARAMETER_CD", "CO2N");
            FilterDef[2].Set("FUEL_CD", "NFS");
            MonDefRecs = FindRows(MonDefRecs, FilterDef);
            if (MonDefRecs.Count > 1)
              Category.CheckCatalogResult = "A";
            else
              if (MonDefRecs.Count == 0)
                Category.CheckCatalogResult = "B";
              else
              {
                decimal MonDef = cDBConvert.ToDecimal(MonDefRecs[0]["DEFAULT_VALUE"]);
                if (MonDef <= 0)
                  Category.CheckCatalogResult = "C";
                else
                  Category.SetCheckParameter("Calculated_Diluent_for_CO2", MonDef, eParameterDataType.Decimal);
              }
          }
          else
            if (Convert.ToBoolean(Category.GetCheckParameter("CO2_Conc_Derived_Checks_Needed").ParameterValue))
              Category.SetCheckParameter("Calculated_Diluent_for_CO2", cDBConvert.ToDecimal(Category.GetCheckParameter("CO2C_DHV_Calculated_Adjusted_Value").ParameterValue), eParameterDataType.Decimal);
            else
              if (Convert.ToBoolean(Category.GetCheckParameter("CO2_Conc_Checks_Needed_For_CO2_Mass_Calc").ParameterValue))
                if (Category.GetCheckParameter("Current_CO2_Conc_Missing_Data_Monitor_Hourly_Record").ParameterValue != null)
                  Category.SetCheckParameter("Calculated_Diluent_for_CO2", cDBConvert.ToDecimal(Category.GetCheckParameter("CO2C_SD_Calculated_Adjusted_Value").ParameterValue), eParameterDataType.Decimal);
                else
                  Category.SetCheckParameter("Calculated_Diluent_for_CO2", cDBConvert.ToDecimal(Category.GetCheckParameter("CO2C_MHV_Calculated_Adjusted_Value").ParameterValue), eParameterDataType.Decimal);
        }
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "HOURCV18");
      }

      return ReturnVal;
    }

    public static string HOURCV19(cCategory Category, ref bool Log)
    //Calculate CO2 Mass Emissions
    {
      string ReturnVal = "";

      try
      {
        DataRowView HourlyOpDataRow = (DataRowView)Category.GetCheckParameter("Current_Hourly_Op_Record").ParameterValue;
        decimal OpTime = cDBConvert.ToDecimal(HourlyOpDataRow["OP_TIME"]);
        DataRowView CurrentDHVRecord = (DataRowView)Category.GetCheckParameter("Current_DHV_Record").ParameterValue;
        int thisLocation = cDBConvert.ToInteger(Category.GetCheckParameter("Current_Monitor_Plan_Location_Postion").ParameterValue);
        decimal AdjVal = cDBConvert.ToDecimal(CurrentDHVRecord["Adjusted_Hrly_Value"]);
        if (Convert.ToBoolean(Category.GetCheckParameter("Derived_Hourly_Adjusted_Value_Status").ParameterValue) && 0 <= OpTime && OpTime <= 1)
        {
          decimal CO2TotalReported = OpTime * AdjVal;
          decimal[] CO2MassRepAccum = Category.GetCheckParameter("Rpt_Period_Co2_Mass_Reported_Accumulator_Array").ValueAsDecimalArray();
          if (CO2MassRepAccum[thisLocation] != decimal.MinValue)
          {
            if (CO2MassRepAccum[thisLocation] >= 0)
              Category.AccumCheckAggregate("Rpt_Period_Co2_Mass_Reported_Accumulator_Array", thisLocation, CO2TotalReported);
          }
          else
            Category.SetArrayParameter("Rpt_Period_Co2_Mass_Reported_Accumulator_Array", thisLocation, CO2TotalReported);
        }
        else
          Category.SetArrayParameter("Rpt_Period_Co2_Mass_Reported_Accumulator_Array", thisLocation, -1m);

        string CO2MethodCode = Convert.ToString(Category.GetCheckParameter("CO2_Method_Code").ParameterValue);
        decimal CO2CalcAdjVal = decimal.MinValue;
        if (CO2MethodCode == "CEM")
        {
          bool DHVValid = Convert.ToBoolean(Category.GetCheckParameter("Current_DHV_Record_Valid").ParameterValue);
          decimal FlowCalcAdjVal = cDBConvert.ToDecimal(Category.GetCheckParameter("FLOW_Calculated_Adjusted_Value").ParameterValue);
          decimal CalcMoisture = cDBConvert.ToDecimal(Category.GetCheckParameter("Calculated_Moisture_for_CO2").ParameterValue);
          decimal CalcDiluentCO2 = cDBConvert.ToDecimal(Category.GetCheckParameter("Calculated_Diluent_for_CO2").ParameterValue);
          string CO2MEqnCd = (string)Category.GetCheckParameter("CO2_Mass_CEM_Equation_Code").ParameterValue;
          if (CO2MEqnCd == "F-11")
          {
            if (DHVValid && CalcDiluentCO2 != decimal.MinValue && FlowCalcAdjVal != decimal.MinValue)
              CO2CalcAdjVal = Math.Round(0.00000057m * CalcDiluentCO2 * FlowCalcAdjVal, 1, MidpointRounding.AwayFromZero);
            else
            {
              Category.SetArrayParameter("Rpt_Period_Co2_Mass_Calculated_Accumulator_Array", thisLocation, -1m);
              Category.CheckCatalogResult = "A";
            }
          }
          else
            if (CO2MEqnCd == "F-2")
            {
              if (DHVValid && CalcDiluentCO2 != decimal.MinValue && FlowCalcAdjVal != decimal.MinValue && CalcMoisture != decimal.MinValue)
                CO2CalcAdjVal = Math.Round(0.00000057m * CalcDiluentCO2 * FlowCalcAdjVal * (100 - CalcMoisture) / 100, 1, MidpointRounding.AwayFromZero);
              else
              {
                Category.SetArrayParameter("Rpt_Period_Co2_Mass_Calculated_Accumulator_Array", thisLocation, -1m);
                Category.CheckCatalogResult = "A";
              }
            }
            else
            {
              Category.SetArrayParameter("Rpt_Period_Co2_Mass_Calculated_Accumulator_Array", thisLocation, -1m);
              Category.CheckCatalogResult = "A";
            }
        }
        else
          if (Convert.ToBoolean(Category.GetCheckParameter("CO2_App_D_Method_Active_For_Hour ").ParameterValue))
          {
            decimal AppDAccum = Convert.ToDecimal(Category.GetCheckParameter("Co2_App_D_Accumulator").ParameterValue);
            if (AppDAccum >= 0 && 0 < OpTime && OpTime <= 1)
              CO2CalcAdjVal = Math.Round(AppDAccum / OpTime, 1, MidpointRounding.AwayFromZero);
            else
              if (!Convert.ToBoolean(Category.GetCheckParameter("Legacy_Data_Evaluation").ParameterValue))
              {
                Category.SetArrayParameter("Rpt_Period_Co2_Mass_Calculated_Accumulator_Array", thisLocation, -1m);
                Category.CheckCatalogResult = "A";
              }
              else
                Category.SetArrayParameter("Rpt_Period_Co2_Mass_Calculated_Accumulator_Array", thisLocation, -2m);
          }
          else
          {
            if (Category.GetCheckParameter("Current_DHV_Calculated_Adjusted_Value").ParameterValue != null)
              CO2CalcAdjVal = cDBConvert.ToDecimal(Category.GetCheckParameter("Current_DHV_Calculated_Adjusted_Value").ParameterValue);
            else
              Category.SetCheckParameter("CO2_Calculated_Adjusted_Value", null, eParameterDataType.Decimal);
          }
        decimal[] CO2MassCalcAccum;
        if (string.IsNullOrEmpty(Category.CheckCatalogResult) && CO2CalcAdjVal != decimal.MinValue)
        {
          Category.SetCheckParameter("CO2_Calculated_Adjusted_Value", CO2CalcAdjVal, eParameterDataType.Decimal);
          if (0 <= OpTime && OpTime <= 1)
          {
            decimal CO2TotalCalcValue = CO2CalcAdjVal * OpTime;
            CO2MassCalcAccum = Category.GetCheckParameter("Rpt_Period_Co2_Mass_Calculated_Accumulator_Array").ValueAsDecimalArray();
            if (CO2MassCalcAccum[thisLocation] != decimal.MinValue)
            {
              if (CO2MassCalcAccum[thisLocation] >= 0)
                Category.AccumCheckAggregate("Rpt_Period_Co2_Mass_Calculated_Accumulator_Array", thisLocation, CO2TotalCalcValue);
            }
            else
              Category.SetArrayParameter("Rpt_Period_Co2_Mass_Calculated_Accumulator_Array", thisLocation, CO2TotalCalcValue);
          }
          else
            Category.SetArrayParameter("Rpt_Period_Co2_Mass_Calculated_Accumulator_Array", thisLocation, -1m);

          if (Category.GetCheckParameter("Derived_Hourly_Adjusted_Value_Status").AsBoolean(false))
          {
            decimal Tolerance = GetTolerance("CO2", "TNHR", Category);
            if (Math.Abs(AdjVal - CO2CalcAdjVal) > Tolerance)
              Category.CheckCatalogResult = "B";
          }
        }
        else
        {
          CO2MassCalcAccum = Category.GetCheckParameter("Rpt_Period_Co2_Mass_Calculated_Accumulator_Array").ValueAsDecimalArray();
          if (CO2MassCalcAccum[thisLocation] != -2m)
            Category.SetArrayParameter("Rpt_Period_Co2_Mass_Calculated_Accumulator_Array", thisLocation, -1m);
        }
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "HOURCV19");
      }

      return ReturnVal;
    }

    #endregion

    #region Checks 21 - 30

    public static string HOURCV25(cCategory Category, ref bool Log)
    //Determine BAF Value for NOx Emission Rate System
    {
      string ReturnVal = "";

      try
      {
        Category.SetCheckParameter("Current_NOX_System_Baf", null, eParameterDataType.Decimal);
        string NOxRMeth = Convert.ToString(Category.GetCheckParameter("Current_NOxR_Method_Code").ParameterValue);
        DataRowView CurrentDHVRecord = (DataRowView)Category.GetCheckParameter("Current_DHV_Record").ParameterValue;
        string ModcCd = cDBConvert.ToString(CurrentDHVRecord["MODC_CD"]);
        decimal CalcUnadjVal = cDBConvert.ToDecimal(Category.GetCheckParameter("NOXR_Calculated_Unadjusted_Value").ParameterValue);
        if (Convert.ToBoolean(Category.GetCheckParameter("Current_NOx_System_Status").ParameterValue) && CalcUnadjVal != decimal.MinValue &&
            NOxRMeth.InList( "CEM,PEM" ) && ModcCd.InList( "01,02,03,14,22,53" ) )
        {
          decimal RATAStatusBAF = Category.GetCheckParameter("RATA_Status_BAF").ValueAsDecimal();
          if (RATAStatusBAF != decimal.MinValue)
            Category.SetCheckParameter("Current_NOX_System_Baf", RATAStatusBAF, eParameterDataType.Decimal);
          else
            Category.CheckCatalogResult = "A";
        }
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "HOURCV25");
      }

      return ReturnVal;
    }

    public static string HOURCV30(cCategory Category, ref bool Log)
    //Initialize SO2 Calculated Hourly Data
    {
      string ReturnVal = "";

      try
      {
        Category.SetCheckParameter("Current_DHV_Parameter", "SO2", eParameterDataType.String);
        Category.SetCheckParameter("Current_DHV_Record_Valid", Convert.ToBoolean(Category.GetCheckParameter("SO2_Derived_Hourly_Status").ParameterValue), eParameterDataType.Boolean);
        Category.SetCheckParameter("SO2_Calculated_Adjusted_Value", null, eParameterDataType.Decimal);
        Category.SetCheckParameter("Calculated_Moisture_for_SO2", null, eParameterDataType.Decimal);
        Category.SetCheckParameter("Current_DHV_Record", Category.GetCheckParameter("Current_SO2_Derived_Hourly_Record").ValueAsDataRowView(), eParameterDataType.DataRowView);
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "HOURCV30");
      }

      return ReturnVal;
    }

    #endregion

    #region Checks 31 - 40

    public static string HOURCV31(cCategory Category, ref bool Log)
    //Initialize NOX Calculated Hourly Data
    {
      string ReturnVal = "";

      try
      {
        Category.SetCheckParameter("Current_DHV_Parameter", "NOX", eParameterDataType.String);
        Category.SetCheckParameter("Current_DHV_Record_Valid", Convert.ToBoolean(Category.GetCheckParameter("NOX_Derived_Hourly_Status").ParameterValue), eParameterDataType.Boolean);
        Category.SetCheckParameter("NOX_Calculated_Adjusted_Value", null, eParameterDataType.Decimal);
        Category.SetCheckParameter("Calculated_Moisture_for_NOX", null, eParameterDataType.Decimal);
        Category.SetCheckParameter("Current_DHV_Record", Category.GetCheckParameter("Current_NOx_Mass_Derived_Hourly_Record").ValueAsDataRowView(), eParameterDataType.DataRowView);
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "HOURCV31");
      }

      return ReturnVal;
    }

    public static string HOURCV32(cCategory Category, ref bool Log)
    //Initialize NOXR Calculated Hourly Data
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentNOxRDHVRec = Category.GetCheckParameter("Current_NoxR_Derived_Hourly_Record").ValueAsDataRowView();
        Category.SetCheckParameter("Current_DHV_Parameter", "NOXR", eParameterDataType.String);
        Category.SetCheckParameter("Current_DHV_Record_Valid", Convert.ToBoolean(Category.GetCheckParameter("NOXR_Derived_Hourly_Status").ParameterValue), eParameterDataType.Boolean);
        Category.SetCheckParameter("NOXR_Calculated_Adjusted_Value", null, eParameterDataType.Decimal);
        Category.SetCheckParameter("Calculated_Diluent_for_NOXR", null, eParameterDataType.Decimal);
        Category.SetCheckParameter("Calculated_Moisture_for_NOXR", null, eParameterDataType.Decimal);
        Category.SetCheckParameter("Current_DHV_HBHA_Value", Convert.ToDecimal(Category.GetCheckParameter("Current_NOXR_HBHA_Value").ParameterValue), eParameterDataType.Decimal);
        Category.SetCheckParameter("Current_DHV_Record", CurrentNOxRDHVRec, eParameterDataType.DataRowView);
        Category.SetCheckParameter("Current_Appendix_E_Status", null, eParameterDataType.Boolean);
        Category.SetCheckParameter("RATA_Status_Required", false, eParameterDataType.Boolean);
        Category.SetCheckParameter("RATA_Status_BAF", null, eParameterDataType.Decimal);
        Category.SetCheckParameter("Current_Hourly_Record_for_RATA_Status", CurrentNOxRDHVRec, eParameterDataType.DataRowView);

        EmParameters.QaStatusComponentId = null;
        EmParameters.QaStatusComponentIdentifier = null;
        EmParameters.QaStatusComponentTypeCode = null;
        EmParameters.QaStatusSystemDesignationCode = EmParameters.CurrentDhvRecord.SysDesignationCd;
        EmParameters.QaStatusSystemId = EmParameters.CurrentDhvRecord.MonSysId;
        EmParameters.QaStatusSystemIdentifier = EmParameters.CurrentDhvRecord.SystemIdentifier;
        EmParameters.QaStatusSystemTypeCode = EmParameters.CurrentDhvRecord.SysTypeCd;
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "HOURCV32");
      }

      return ReturnVal;
    }

    public static string HOURCV33(cCategory Category, ref bool Log)
    //Initialize CO2 Calculated Hourly Data
    {
      string ReturnVal = "";

      try
      {
        Category.SetCheckParameter("Current_DHV_Parameter", "CO2", eParameterDataType.String);
        Category.SetCheckParameter("Current_DHV_Record_Valid", Convert.ToBoolean(Category.GetCheckParameter("CO2_Derived_Hourly_Status").ParameterValue), eParameterDataType.Boolean);
        Category.SetCheckParameter("CO2_Calculated_Adjusted_Value", null, eParameterDataType.Decimal);
        Category.SetCheckParameter("Calculated_Diluent_for_CO2", null, eParameterDataType.Decimal);
        Category.SetCheckParameter("Calculated_Moisture_for_CO2", null, eParameterDataType.Decimal);
        Category.SetCheckParameter("Current_DHV_Record", Category.GetCheckParameter("Current_CO2_Mass_Derived_Hourly_Record").ValueAsDataRowView(), eParameterDataType.DataRowView);
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "HOURCV33");
      }

      return ReturnVal;
    }

    public static string HOURCV34(cCategory Category, ref bool Log)
    //Initialize CO2C Calculated Hourly Data
    {
      string ReturnVal = "";

      try
      {
        Category.SetCheckParameter("Current_DHV_Parameter", "CO2C", eParameterDataType.String);
        Category.SetCheckParameter("Current_DHV_Record_Valid", Convert.ToBoolean(Category.GetCheckParameter("CO2C_Derived_Hourly_Status").ParameterValue), eParameterDataType.Boolean);
        Category.SetCheckParameter("CO2C_DHV_Calculated_Adjusted_Value", null, eParameterDataType.Decimal);
        Category.SetCheckParameter("Calculated_Diluent_for_CO2C", null, eParameterDataType.Decimal);
        Category.SetCheckParameter("Calculated_Moisture_for_CO2C", null, eParameterDataType.Decimal);
        Category.SetCheckParameter("Current_DHV_HBHA_Value", Convert.ToDecimal(Category.GetCheckParameter("Current_CO2C_DHV_HBHA_Value").ParameterValue), eParameterDataType.Decimal);
        Category.SetCheckParameter("Current_DHV_Record", Category.GetCheckParameter("Current_CO2_Conc_Derived_Hourly_Record").ValueAsDataRowView(), eParameterDataType.DataRowView);
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "HOURCV34");
      }

      return ReturnVal;
    }

    public static string HOURCV35(cCategory Category, ref bool Log)
    //Initialize H2O Calculated Hourly Data
    {
      string ReturnVal = "";

      try
      {
        Category.SetCheckParameter("Current_DHV_Parameter", "H2O", eParameterDataType.String);
        Category.SetCheckParameter("Current_DHV_Record_Valid", Convert.ToBoolean(Category.GetCheckParameter("H2O_Derived_Hourly_Status").ParameterValue), eParameterDataType.Boolean);
        Category.SetCheckParameter("H2O_DHV_Calculated_Adjusted_Value", null, eParameterDataType.Decimal);
        Category.SetCheckParameter("Current_DHV_HBHA_Value", Convert.ToDecimal(Category.GetCheckParameter("Current_H2O_DHV_HBHA_Value").ParameterValue), eParameterDataType.Decimal);
        DataRowView DHVRec = Category.GetCheckParameter("Current_H2O_Derived_Hourly_Record").ValueAsDataRowView();
        string SysTypeCd = cDBConvert.ToString(DHVRec["SYS_TYPE_CD"]);
        string ModcCd = cDBConvert.ToString(DHVRec["MODC_CD"]);
        Category.SetCheckParameter("Current_DHV_Record", DHVRec, eParameterDataType.DataRowView);

        EmParameters.QaStatusComponentId = null;
        EmParameters.QaStatusComponentIdentifier = null;
        EmParameters.QaStatusComponentTypeCode = null;
        EmParameters.QaStatusSystemDesignationCode = EmParameters.CurrentDhvRecord.SysDesignationCd;
        EmParameters.QaStatusSystemId = EmParameters.CurrentDhvRecord.MonSysId;
        EmParameters.QaStatusSystemIdentifier = EmParameters.CurrentMhvRecord.SystemIdentifier;
        EmParameters.QaStatusSystemTypeCode = EmParameters.CurrentDhvRecord.SysTypeCd;

        if (SysTypeCd == "H2O" && ModcCd.InList("01,02,03,21,53"))
          Category.SetCheckParameter("RATA_Status_Required", true, eParameterDataType.Boolean);
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "HOURCV35");
      }

      return ReturnVal;
    }

    public static string HOURCV36(cCategory Category, ref bool Log)
    //Initialize CO2C Calculated Hourly Data
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentHIDHVRec = Category.GetCheckParameter("Current_Heat_Input_Derived_Hourly_Record").ValueAsDataRowView();
        Category.SetCheckParameter("Current_DHV_Parameter", "HI", eParameterDataType.String);
        Category.SetCheckParameter("Current_DHV_Record_Valid", Convert.ToBoolean(Category.GetCheckParameter("HI_Derived_Hourly_Status").ParameterValue), eParameterDataType.Boolean);
        Category.SetCheckParameter("HI_Calculated_Adjusted_Value", null, eParameterDataType.Decimal);
        Category.SetCheckParameter("Calculated_Diluent_for_HI", null, eParameterDataType.Decimal);
        Category.SetCheckParameter("Calculated_Moisture_for_HI", null, eParameterDataType.Decimal);
        Category.SetCheckParameter("Current_DHV_Record", CurrentHIDHVRec, eParameterDataType.DataRowView);
        Category.SetCheckParameter("RATA_Status_Required", false, eParameterDataType.Boolean);
        Category.SetCheckParameter("Current_Hourly_Record_for_RATA_Status", CurrentHIDHVRec, eParameterDataType.DataRowView);

        EmParameters.QaStatusComponentId = null;
        EmParameters.QaStatusComponentIdentifier = null;
        EmParameters.QaStatusComponentTypeCode = null;
        EmParameters.QaStatusSystemDesignationCode = EmParameters.CurrentDhvRecord.SysDesignationCd;
        EmParameters.QaStatusSystemId = EmParameters.CurrentDhvRecord.MonSysId;
        EmParameters.QaStatusSystemIdentifier = EmParameters.CurrentDhvRecord.SystemIdentifier;
        EmParameters.QaStatusSystemTypeCode = EmParameters.CurrentDhvRecord.SysTypeCd;
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "HOURCV36");
      }

      return ReturnVal;
    }

    public static string HOURCV37(cCategory category, ref bool log)
    //Check Unadjusted Value
    {
      string returnVal = "";

      try
      {
        category.SetCheckParameter("Derived_Hourly_Unadjusted_Value_Status", false, eParameterDataType.Boolean);

        DataRowView currentDHVRecord = category.GetCheckParameter("Current_DHV_Record").AsDataRowView();
        decimal? unadjustedHrlyValue = currentDHVRecord["UNADJUSTED_HRLY_VALUE"].AsDecimal();

        if (category.GetCheckParameter("Current_NOxR_Method_Code").AsString().InList("CEM,PEM"))
        {
          string modcCd = currentDHVRecord["MODC_CD"].AsString();

          if (modcCd.InList("01,02,03,04,14,21,22,53,54"))
          {
            if (unadjustedHrlyValue.IsNotNull())
            {
              if (unadjustedHrlyValue.Value < 0 && modcCd != "21")
                category.CheckCatalogResult = "A";
              else if (unadjustedHrlyValue.Value > 0 && modcCd == "21")
                category.CheckCatalogResult = "B";
              else if (unadjustedHrlyValue.Value != Math.Round(unadjustedHrlyValue.Value, 3, MidpointRounding.AwayFromZero) && unadjustedHrlyValue != decimal.MinValue)
                category.CheckCatalogResult = "F";
              else
              {
                category.SetCheckParameter("Derived_Hourly_Unadjusted_Value_Status", true, eParameterDataType.Boolean);

                decimal? currentDhvMaxMinValue = category.GetCheckParameter("Current_DHV_Max_Min_Value").AsDecimal();
                string noxConcModc = category.GetCheckParameter("NOx_Conc_Modc").AsString();

                if (currentDhvMaxMinValue.IsNotNull() &&
                    (noxConcModc.IsNull() || noxConcModc.NotInList("19,20")))
                {
                  if (unadjustedHrlyValue > currentDhvMaxMinValue)
                    category.CheckCatalogResult = "C";
                }
              }
            }
            else if (modcCd.NotInList("04,53,54"))
              category.CheckCatalogResult = "A";
            else
              category.SetCheckParameter("Derived_Hourly_Unadjusted_Value_Status", true, eParameterDataType.Boolean);
          }
          else if (category.GetCheckParameter("Derived_Hourly_Modc_Status").AsBoolean().Default())
          {
            if (unadjustedHrlyValue.IsNotNull())
              category.CheckCatalogResult = "D";
            else
              category.SetCheckParameter("Derived_Hourly_Unadjusted_Value_Status", true, eParameterDataType.Boolean);
          }
        }
        else if (unadjustedHrlyValue.IsNotNull())
          category.CheckCatalogResult = "E";
        else
          category.SetCheckParameter("Derived_Hourly_Unadjusted_Value_Status", true, eParameterDataType.Boolean);
      }
      catch (Exception ex)
      {
        returnVal = category.CheckEngine.FormatError(ex, "HOURCV37");
      }

      return returnVal;
    }

    public static string HOURCV38(cCategory Category, ref bool Log)
    //Determine Maximum or Minimum Value for Parameter in DHV Record
    {
      string ReturnVal = "";

      try
      {
        Category.SetCheckParameter("Current_DHV_Max_Min_Value", null, eParameterDataType.Decimal);
        string DHVMeth = Convert.ToString(Category.GetCheckParameter("Current_DHV_Method").ParameterValue);
        string DHVParam = Convert.ToString(Category.GetCheckParameter("Current_DHV_Parameter").ParameterValue);
        if (Convert.ToBoolean(Category.GetCheckParameter("Current_DHV_Record_Valid").ParameterValue) &&
            ((DHVParam == "H2O" && Convert.ToString(Category.GetCheckParameter("H2O_Method_Code").ParameterValue) == "MWD") ||
             (DHVParam == "NOXR" && Convert.ToString(Category.GetCheckParameter("Current_NOxR_Method_Code").ParameterValue).InList("CEM,PEM")) ||
             (DHVParam == "CO2C") ||
             (DHVParam == "HI")))
        {
          DataRowView CurrentDHVRecord = (DataRowView)Category.GetCheckParameter("Current_DHV_Record").ParameterValue;
          string ModcCd = cDBConvert.ToString(CurrentDHVRecord["MODC_CD"]);
          if (DHVParam == "H2O")
          {
            if (Convert.ToBoolean(Category.GetCheckParameter("H2O_Fuel_Specific_Missing_Data").ParameterValue))
              Category.SetCheckParameter("Current_DHV_Fuel_Specific_Hour", true, eParameterDataType.Boolean);
            if (Convert.ToString(Category.GetCheckParameter("H2O_Missing_Data_Approach").ParameterValue) == "MAX")
              Category.SetCheckParameter("Current_DHV_Default_Parameter", "H2OX", eParameterDataType.String);
            else
              if (Convert.ToString(Category.GetCheckParameter("H2O_Missing_Data_Approach").ParameterValue) == "MIN")
                Category.SetCheckParameter("Current_DHV_Default_Parameter", "H2ON", eParameterDataType.String);
              else
                if (ModcCd == "12")
                  Category.CheckCatalogResult = "A";
          }
          else if (DHVParam == "NOXR")
          {
            Category.SetCheckParameter("Current_DHV_Default_Parameter", "NORX", eParameterDataType.String);
            if (ModcCd.InList("23,24"))
              if (Convert.ToString(Category.GetCheckParameter("NOx_Rate_Bypass_Code").ParameterValue) == "BYMAXFS")
                Category.SetCheckParameter("Current_DHV_Fuel_Specific_Hour", true, eParameterDataType.Boolean);
              else
                Category.SetCheckParameter("Current_DHV_Fuel_Specific_Hour", false, eParameterDataType.Boolean);
            else
              if (Convert.ToBoolean(Category.GetCheckParameter("NOx_Rate_Fuel_Specific_Missing_Data").ParameterValue))
                Category.SetCheckParameter("Current_DHV_Fuel_Specific_Hour", true, eParameterDataType.Boolean);
              else
                Category.SetCheckParameter("Current_DHV_Fuel_Specific_Hour", false, eParameterDataType.Boolean);
          }
          else if (DHVParam == "CO2C")
          {
            Category.SetCheckParameter("Current_DHV_Default_Parameter", "CO2X", eParameterDataType.String);
            if (Convert.ToBoolean(Category.GetCheckParameter("CO2_Fuel_Specific_Missing_Data").ParameterValue))
              Category.SetCheckParameter("Current_DHV_Fuel_Specific_Hour", true, eParameterDataType.Boolean);
          }

          if (DHVParam == "HI")
          {
            DataView unitCapacityRecords = Category.GetCheckParameter("Location_Capacity_Records_for_Hour_Location").AsDataView();
            DataView unitCapacityView = cRowFilter.FindRows(unitCapacityRecords, new cFilterCondition[] { new cFilterCondition("MAX_HI_CAPACITY", 0, eFilterDataType.Decimal, eFilterConditionRelativeCompare.GreaterThan )});

            if (unitCapacityView.Count > 0)
            {
              decimal currentDhvMaxMinValue = 0;

              foreach (DataRowView unitCapacityRow in unitCapacityView)
                currentDhvMaxMinValue += unitCapacityRow["MAX_HI_CAPACITY"].AsDecimal(0);

              Category.SetCheckParameter("Current_DHV_Max_Min_Value", currentDhvMaxMinValue, eParameterDataType.Decimal);
            }
          }
          else
          {
            DataRowView CurrentHourlyOpRow = (DataRowView)Category.GetCheckParameter("Current_Hourly_Op_Record").ParameterValue;
            string FuelCd = cDBConvert.ToString(CurrentHourlyOpRow["FUEL_CD"]);
            string DHVDefParam = Convert.ToString(Category.GetCheckParameter("Current_DHV_Default_Parameter").ParameterValue);
            if (string.IsNullOrEmpty(Category.CheckCatalogResult) && DHVDefParam != "")
            {
              DataView MonDefRecs = (DataView)Category.GetCheckParameter("Monitor_Default_Records_by_Hour_Location").ParameterValue;
              sFilterPair[] FilterDef = new sFilterPair[4];
              FilterDef[0].Set("DEFAULT_PURPOSE_CD", "MD");
              FilterDef[1].Set("PARAMETER_CD", DHVDefParam);
              decimal MaxMinVal;
              if (ModcCd.InList("12,23,25") && Convert.ToBoolean(Category.GetCheckParameter("Current_DHV_Fuel_Specific_Hour").ParameterValue))
              {
                if (FuelCd != "")
                {
                  Category.SetCheckParameter("Current_DHV_Missing_Data_Fuel", FuelCd, eParameterDataType.String);
                  FilterDef[2].Set("FUEL_CD", FuelCd);
                  FilterDef[3].Set("OPERATING_CONDITION_CD", "A,U", eFilterPairStringCompare.InList);
                  MonDefRecs = FindRows(MonDefRecs, FilterDef);
                  if (MonDefRecs.Count > 1)
                    Category.CheckCatalogResult = "B";
                  else
                    if (MonDefRecs.Count == 0)
                      Category.CheckCatalogResult = "C";
                    else
                    {
                      MaxMinVal = cDBConvert.ToDecimal(MonDefRecs[0]["DEFAULT_VALUE"]);
                      if (MaxMinVal > 0)
                        Category.SetCheckParameter("Current_DHV_Max_Min_Value", MaxMinVal, eParameterDataType.Decimal);
                      else
                        Category.CheckCatalogResult = "D";
                    }
                }
              }
              else if (ModcCd.InList("13,24"))
              {
                if (Convert.ToBoolean(Category.GetCheckParameter("Current_DHV_Fuel_Specific_Hour").ParameterValue))
                {
                  if (FuelCd != "")
                  {
                    Category.SetCheckParameter("Current_DHV_Missing_Data_Fuel", FuelCd, eParameterDataType.String);
                    FilterDef[2].Set("FUEL_CD", FuelCd);
                    FilterDef[3].Set("OPERATING_CONDITION_CD", "C");
                    MonDefRecs = FindRows(MonDefRecs, FilterDef);
                    if (MonDefRecs.Count > 1)
                      Category.CheckCatalogResult = "B";
                    else
                      if (MonDefRecs.Count == 0)
                        Category.CheckCatalogResult = "C";
                      else
                      {
                        MaxMinVal = cDBConvert.ToDecimal(MonDefRecs[0]["DEFAULT_VALUE"]);
                        if (MaxMinVal > 0)
                          Category.SetCheckParameter("Current_DHV_Max_Min_Value", MaxMinVal, eParameterDataType.Decimal);
                        else
                          Category.CheckCatalogResult = "D";
                      }
                  }
                }
                else
                {
                  Category.SetCheckParameter("Current_DHV_Missing_Data_Fuel", "NFS", eParameterDataType.String);
                  FilterDef[2].Set("FUEL_CD", "NFS");
                  FilterDef[3].Set("OPERATING_CONDITION_CD", "C");
                  MonDefRecs = FindRows(MonDefRecs, FilterDef);
                  if (MonDefRecs.Count > 1)
                    Category.CheckCatalogResult = "B";
                  else
                    if (MonDefRecs.Count == 0)
                      Category.CheckCatalogResult = "C";
                    else
                    {
                      MaxMinVal = cDBConvert.ToDecimal(MonDefRecs[0]["DEFAULT_VALUE"]);
                      if (MaxMinVal > 0)
                        Category.SetCheckParameter("Current_DHV_Max_Min_Value", MaxMinVal, eParameterDataType.Decimal);
                      else
                        Category.CheckCatalogResult = "D";
                    }
                }
              }
              else if (ModcCd != "15")
              {
                Category.SetCheckParameter("Current_DHV_Missing_Data_Fuel", "NFS", eParameterDataType.String);
                FilterDef = new sFilterPair[4];
                FilterDef[0].Set("PARAMETER_CD", Convert.ToString(Category.GetCheckParameter("Current_DHV_Default_Parameter").ParameterValue));
                FilterDef[1].Set("DEFAULT_PURPOSE_CD", "MD");
                FilterDef[2].Set("FUEL_CD", "NFS");
                FilterDef[3].Set("OPERATING_CONDITION_CD", "A,U", eFilterPairStringCompare.InList);

                MonDefRecs = FindRows(MonDefRecs, FilterDef);
                if (MonDefRecs.Count > 1)
                  Category.CheckCatalogResult = "B";

                else if (MonDefRecs.Count == 0 && DHVParam == "CO2C")
                {
                  DataView MonSpanRecs = (DataView)Category.GetCheckParameter("Monitor_Span_Records_By_Hour_Location").ParameterValue;
                  sFilterPair[] FilterSpan = new sFilterPair[2];
                  FilterSpan[0].Set("COMPONENT_TYPE_CD", "CO2");
                  FilterSpan[1].Set("SPAN_SCALE_CD", "H");
                  MonSpanRecs = FindRows(MonSpanRecs, FilterSpan);
                  if (MonSpanRecs.Count > 1)
                    Category.CheckCatalogResult = "E";
                  else if (MonSpanRecs.Count == 0)
                    Category.CheckCatalogResult = "F";
                  else
                  {
                    MaxMinVal = cDBConvert.ToDecimal(MonSpanRecs[0]["MPC_VALUE"]);
                    if (MonSpanRecs[0]["DEFAULT_HIGH_RANGE"] == DBNull.Value && !ModcCd.InList("13,24"))
                      if (MaxMinVal > 0)
                        Category.SetCheckParameter("Current_DHV_Max_Min_Value", MaxMinVal, eParameterDataType.Decimal);
                      else
                        Category.CheckCatalogResult = "G";
                  }
                }
                else if (MonDefRecs.Count == 0 && DHVParam == "NOXR")
                {
                  FilterDef[0].Set("PARAMETER_CD", "MNNX");
                  FilterDef[1].Set("DEFAULT_PURPOSE_CD", "MD");
                  FilterDef[2].Set("FUEL_CD", "NFS");
                  FilterDef[3].Set("OPERATING_CONDITION_CD", "A,U", eFilterPairStringCompare.InList);

                  MonDefRecs = (DataView)Category.GetCheckParameter("Monitor_Default_Records_by_Hour_Location").ParameterValue;
                  MonDefRecs = FindRows(MonDefRecs, FilterDef);
                  if (MonDefRecs.Count > 1)
                  {
                    Category.SetCheckParameter("Current_DHV_Default_Parameter", "MNNX", eParameterDataType.String);
                    Category.CheckCatalogResult = "B";
                  }
                  else if (MonDefRecs.Count == 0)
                    Category.CheckCatalogResult = "C";
                  else
                  {
                    MaxMinVal = cDBConvert.ToDecimal(MonDefRecs[0]["DEFAULT_VALUE"]);
                    Category.SetCheckParameter("Current_DHV_Default_Parameter", "MNNX", eParameterDataType.String);
                    if (MaxMinVal >= 0)
                      Category.SetCheckParameter("Current_DHV_Max_Min_Value", MaxMinVal, eParameterDataType.Decimal);
                    else
                      Category.CheckCatalogResult = "D";
                  }
                }
                else if (MonDefRecs.Count == 0)
                  Category.CheckCatalogResult = "C";
                else
                {
                  MaxMinVal = cDBConvert.ToDecimal(MonDefRecs[0]["DEFAULT_VALUE"]);
                  if (MaxMinVal > 0)
                    Category.SetCheckParameter("Current_DHV_Max_Min_Value", MaxMinVal, eParameterDataType.Decimal);
                  else
                    Category.CheckCatalogResult = "D";
                }
              }
            }
          }
        }
      }
       
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "HOURCV38");
      }

      return ReturnVal;
    }

    public static string HOURCV39(cCategory Category, ref bool Log)
    //Check Adjusted Hourly Value in DHV Record
    {
      string ReturnVal = "";

      try
      {
        Category.SetCheckParameter("Derived_Hourly_Adjusted_Value_Status", false, eParameterDataType.Boolean);
        Category.SetCheckParameter("Current_DHV_Calculated_Adjusted_Value", null, eParameterDataType.Decimal);

        if (Convert.ToBoolean(Category.GetCheckParameter("Current_DHV_Record_Valid").ParameterValue))
        {
          DataRowView CurrentDHVRecord = (DataRowView)Category.GetCheckParameter("Current_DHV_Record").ParameterValue;
          string ModcCd = cDBConvert.ToString(CurrentDHVRecord["MODC_CD"]);
          decimal AdjVal = cDBConvert.ToDecimal(CurrentDHVRecord["ADJUSTED_HRLY_VALUE"]);
          decimal MaxMin = cDBConvert.ToDecimal(Category.GetCheckParameter("Current_DHV_Max_Min_Value").ParameterValue);
          string DHVParameter = Convert.ToString(Category.GetCheckParameter("Current_DHV_Parameter").ParameterValue);
          int CurrentDHVPrecision;

          if (Category.GetCheckParameter("Hourly_Fuel_Flow_Count_For_Gas").ValueAsInt() > 0 && DHVParameter == "SO2")
            CurrentDHVPrecision = 4;
          else
          {
            DataView ParamUOMLookup = Category.GetCheckParameter("Parameter_Units_Of_Measure_Lookup_Table").ValueAsDataView();
            DataView ParamUOMRowsFound;
            sFilterPair[] UOMFilter = new sFilterPair[1];
            UOMFilter[0].Set("PARAMETER_CD", DHVParameter);
            ParamUOMRowsFound = FindRows(ParamUOMLookup, UOMFilter);
            CurrentDHVPrecision = cDBConvert.ToInteger(ParamUOMRowsFound[0]["DECIMALS_HRLY"]);
          }

          if (ModcCd != "")
          {
            string H2OMissDataApproach = Convert.ToString(Category.GetCheckParameter("H2O_Missing_Data_Approach").ParameterValue);
            switch (ModcCd)
            {
              case "21":
                Category.SetCheckParameter("Current_DHV_Calculated_Adjusted_Value", 0m, eParameterDataType.Decimal);
                if (AdjVal == 0)
                  Category.SetCheckParameter("Derived_Hourly_Adjusted_Value_Status", true, eParameterDataType.Boolean);
                else
                  Category.CheckCatalogResult = "A";
                break;
              case "12":
              case "23":
              case "25":
                if (MaxMin != decimal.MinValue)
                {
                  Category.SetCheckParameter("Current_DHV_Calculated_Adjusted_Value", MaxMin, eParameterDataType.Decimal);
                  if (AdjVal == MaxMin)
                    Category.SetCheckParameter("Derived_Hourly_Adjusted_Value_Status", true, eParameterDataType.Boolean);
                  else
                    Category.CheckCatalogResult = "B";
                }
                break;
              case "13":
              case "24":
                if (MaxMin != decimal.MinValue)
                {
                  Category.SetCheckParameter("Current_DHV_Calculated_Adjusted_Value", MaxMin, eParameterDataType.Decimal);
                  if (AdjVal == MaxMin)
                    Category.SetCheckParameter("Derived_Hourly_Adjusted_Value_Status", true, eParameterDataType.Boolean);
                  else
                    Category.CheckCatalogResult = "C";
                }
                break;
              case "06":
                {
                  if (DHVParameter.InList("CO2C,H2O") && (AdjVal < 0 || AdjVal > 100))
                    Category.CheckCatalogResult = "L";
                  else
                  {
                    decimal HBHAVal = cDBConvert.ToDecimal(Category.GetCheckParameter("Current_DHV_HBHA_Value").ParameterValue);
                    if (HBHAVal != decimal.MinValue)
                    {
                      Category.SetCheckParameter("Current_DHV_Calculated_Adjusted_Value", HBHAVal, eParameterDataType.Decimal);
                      if (AdjVal >= 0)
                        if (AdjVal == HBHAVal)
                          Category.SetCheckParameter("Derived_Hourly_Adjusted_Value_Status", true, eParameterDataType.Boolean);
                        else
                          Category.CheckCatalogResult = "D";
                      else
                        Category.CheckCatalogResult = "E";
                    }
                    else
                      if (AdjVal >= 0)
                        if (AdjVal != Math.Round(AdjVal, CurrentDHVPrecision, MidpointRounding.AwayFromZero) && AdjVal != decimal.MinValue)
                          Category.CheckCatalogResult = "M";
                        else
                        {
                          Category.SetCheckParameter("Current_DHV_Calculated_Adjusted_Value", AdjVal, eParameterDataType.Decimal);
                          Category.SetCheckParameter("Derived_Hourly_Adjusted_Value_Status", true, eParameterDataType.Boolean);
                          if (DHVParameter.InList("CO2C,H2O,NOXR") && MaxMin != decimal.MinValue)
                            if (DHVParameter == "H2O" && H2OMissDataApproach == "MIN")
                            {
                              if (AdjVal < MaxMin)
                                Category.CheckCatalogResult = "H";
                            }
                            else
                              if (AdjVal > MaxMin)
                              {
                                if ((DHVParameter == "NOXR") & (AdjVal > MaxMin * 2))
                                  Category.CheckCatalogResult = "O";
                                else
                                  Category.CheckCatalogResult = "G";
                              }
                        }
                      else
                        Category.CheckCatalogResult = "E";
                  }
                }
                break;

              case "08":
              case "09":
                {
                  {
                    if (DHVParameter.InList("CO2C,H2O") && (AdjVal < 0 || AdjVal > 100))
                      Category.CheckCatalogResult = "L";
                    else if (AdjVal >= 0)
                    {
                      decimal CurrentDhvHbhaValue = cDBConvert.ToDecimal(Category.GetCheckParameter("Current_DHV_HBHA_Value").ParameterValue);

                      if ((CurrentDhvHbhaValue != decimal.MinValue)
                          && (DHVParameter == "H2O") && (H2OMissDataApproach == "MIN")
                          && (CurrentDhvHbhaValue < AdjVal))
                      {
                        Category.SetCheckParameter("Current_DHV_Calculated_Adjusted_Value", CurrentDhvHbhaValue, eParameterDataType.Decimal);
                        Category.CheckCatalogResult = "N";
                      }
                      else if ((CurrentDhvHbhaValue != decimal.MinValue)
                          && ((DHVParameter != "H2O") || (H2OMissDataApproach == "MAX"))
                          && (CurrentDhvHbhaValue > AdjVal)
                          && (cDBConvert.ToBoolean(Category.GetCheckParameter("Unit_Is_Load_Based").ParameterValue) || DHVParameter != "NOXR"))
                      {
                        Category.SetCheckParameter("Current_DHV_Calculated_Adjusted_Value", CurrentDhvHbhaValue, eParameterDataType.Decimal);
                        Category.CheckCatalogResult = "F";
                      }
                      else if (AdjVal != Math.Round(AdjVal, CurrentDHVPrecision, MidpointRounding.AwayFromZero) && AdjVal != decimal.MinValue)
                        Category.CheckCatalogResult = "M";
                      else
                      {
                        Category.SetCheckParameter("Current_DHV_Calculated_Adjusted_Value", AdjVal, eParameterDataType.Decimal);
                        Category.SetCheckParameter("Derived_Hourly_Adjusted_Value_Status", true, eParameterDataType.Boolean);

                        if (DHVParameter.InList("CO2C,H2O,NOXR") && (MaxMin != decimal.MinValue))
                        {
                          if ((DHVParameter == "H2O") && (H2OMissDataApproach == "MIN"))
                          {
                            if (AdjVal < MaxMin)
                              Category.CheckCatalogResult = "H";
                          }
                          else
                          {
                            if (AdjVal > MaxMin)
                            {
                              if ((DHVParameter == "NOXR") & (AdjVal > MaxMin * 2))
                                Category.CheckCatalogResult = "O";
                              else
                                Category.CheckCatalogResult = "G";
                            }
                          }
                        }
                      }
                    }
                    else
                      Category.CheckCatalogResult = "E";
                  }
                }
                break;

              case "04":
              case "05":
              case "07":
              case "10":
              case "11":
              case "15":
              case "53":
              case "54":
              case "55":
                {
                  if (DHVParameter.InList("CO2C,H2O") && (AdjVal < 0 || AdjVal > 100))
                    Category.CheckCatalogResult = "L";
                  else
                  {
                    if (AdjVal >= 0)
                      if (AdjVal != Math.Round(AdjVal, CurrentDHVPrecision, MidpointRounding.AwayFromZero) && AdjVal != decimal.MinValue)
                        Category.CheckCatalogResult = "M";
                      else
                      {
                        Category.SetCheckParameter("Current_DHV_Calculated_Adjusted_Value", AdjVal, eParameterDataType.Decimal);
                        Category.SetCheckParameter("Derived_Hourly_Adjusted_Value_Status", true, eParameterDataType.Boolean);
                        if (DHVParameter.InList("CO2C,H2O,NOXR") && MaxMin != decimal.MinValue)
                          if (DHVParameter == "H2O" && H2OMissDataApproach == "MIN")
                          {
                            if (AdjVal < MaxMin)
                              Category.CheckCatalogResult = "H";
                          }
                          else
                            if (AdjVal > MaxMin)
                            {
                              if ((DHVParameter == "NOXR") & (AdjVal > MaxMin * 2))
                                Category.CheckCatalogResult = "O";
                              else
                                Category.CheckCatalogResult = "G";
                            }
                      }
                    else
                      Category.CheckCatalogResult = "E";
                  }
                }
                break;
              case "26":
                if (AdjVal == 1)
                  Category.SetCheckParameter("Derived_Hourly_Adjusted_Value_Status", true, eParameterDataType.Boolean);
                else
                  Category.CheckCatalogResult = "I";
                break;
              case "40":
                break;
              default:
                {
                  if (DHVParameter.InList("CO2C,H2O") && (AdjVal < 0 || AdjVal > 100))
                    Category.CheckCatalogResult = "L";
                  else
                  {
                    if (AdjVal >= 0)
                      if (AdjVal != Math.Round(AdjVal, CurrentDHVPrecision, MidpointRounding.AwayFromZero) && AdjVal != decimal.MinValue)
                        Category.CheckCatalogResult = "M";
                      else
                      {
                        Category.SetCheckParameter("Derived_Hourly_Adjusted_Value_Status", true, eParameterDataType.Boolean);

                        if (DHVParameter.InList("CO2C,H2O") && MaxMin != decimal.MinValue)
                          if (DHVParameter == "H2O" && H2OMissDataApproach == "MIN")
                          {
                            if (AdjVal < MaxMin)
                              Category.CheckCatalogResult = "H";
                          }
                          else
                            if (AdjVal > MaxMin)
                            {
                              if ((DHVParameter == "NOXR") & (AdjVal > MaxMin * 2))
                                Category.CheckCatalogResult = "O";
                              else
                                Category.CheckCatalogResult = "G";
                            }
                      }
                    else
                      Category.CheckCatalogResult = "E";
                  }
                }
                break;
            }
          }
          else
            if (AdjVal >= 0)
              if (AdjVal != Math.Round(AdjVal, CurrentDHVPrecision, MidpointRounding.AwayFromZero) && AdjVal != decimal.MinValue)
                Category.CheckCatalogResult = "M";
              else
              {
                Category.SetCheckParameter("Derived_Hourly_Adjusted_Value_Status", true, eParameterDataType.Boolean);
                switch (DHVParameter)
                {
                  case "HI":
                    {
                      string heatInputMethodCd = Category.GetCheckParameter("Heat_Input_Method_Code").ValueAsString();

                      if (!heatInputMethodCd.InList("AD,ADCALC,CALC"))
                        Category.SetCheckParameter("Current_DHV_Calculated_Adjusted_Value", AdjVal, eParameterDataType.Decimal);

                      if (AdjVal == 0)
                      {
                        DataRowView HourlyOpDataRow = Category.GetCheckParameter("Current_Hourly_Op_Record").ValueAsDataRowView();
                        decimal OpTime = HourlyOpDataRow["OP_TIME"].AsDecimal().Default();

                        if (heatInputMethodCd == "CEM")
                        {
                          if (Category.GetCheckParameter("Legacy_Data_Evaluation").ValueAsBool())
                          {
                            if (OpTime > 0.25m)
                              Category.CheckCatalogResult = "J";
                          }
                          else
                          {
                            if (OpTime > 0)
                              Category.CheckCatalogResult = "K";
                          }
                        }
                      }
                      else if ((MaxMin != decimal.MinValue) && (AdjVal > MaxMin))
                      {
                          Category.CheckCatalogResult = "G";
                      }
                    }
                    break;

                  case "NOXR":
                    {
                      if (Convert.ToString(Category.GetCheckParameter("Current_NOxR_Method_Code").ParameterValue) != "AE")
                        Category.SetCheckParameter("Current_DHV_Calculated_Adjusted_Value", AdjVal, eParameterDataType.Decimal);
                    }
                    break;

                  case "SO2":
                    {
                      if (!Convert.ToBoolean(Category.GetCheckParameter("SO2_App_D_Method_Active_For_Hour").ParameterValue))
                        Category.SetCheckParameter("Current_DHV_Calculated_Adjusted_Value", AdjVal, eParameterDataType.Decimal);
                    }
                    break;

                  case "CO2":
                    {
                      if (!Convert.ToBoolean(Category.GetCheckParameter("CO2_App_D_Method_Active_For_Hour").ParameterValue))
                        Category.SetCheckParameter("Current_DHV_Calculated_Adjusted_Value", AdjVal, eParameterDataType.Decimal);
                    }
                    break;

                  default:
                    {
                      Category.SetCheckParameter("Current_DHV_Calculated_Adjusted_Value", AdjVal, eParameterDataType.Decimal);
                    }
                    break;
                }
              }
            else
              Category.CheckCatalogResult = "E";
        }
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "HOURCV39");
      }

      return ReturnVal;
    }

    public static string HOURCV40(cCategory Category, ref bool Log)
    //Determine Moisture for SO2 Mass Calculation Verification
    {
      string ReturnVal = "";

      try
      {
        if (Convert.ToString(Category.GetCheckParameter("SO2_Method_Code").ParameterValue).StartsWith("CEM"))
          if (Convert.ToString(Category.GetCheckParameter("SO2_Equation_Code").ParameterValue) == "F-2")
          {
            decimal H2ODHVCalcAdjVal = cDBConvert.ToDecimal(Category.GetCheckParameter("H2O_DHV_Calculated_Adjusted_Value").ParameterValue);
            decimal H2OMHVCalcAdjVal = cDBConvert.ToDecimal(Category.GetCheckParameter("H2O_MHV_Calculated_Adjusted_Value").ParameterValue);
            decimal H2ODefVal = cDBConvert.ToDecimal(Category.GetCheckParameter("H2O_Default_Value").ParameterValue);
            string H2OMethCd = Convert.ToString(Category.GetCheckParameter("H2O_Method_Code").ParameterValue);
            decimal CalcMoistureSO2 = decimal.MinValue;
            if (H2OMethCd == "MWD" && Convert.ToBoolean(Category.GetCheckParameter("H2O_Derived_Hourly_Checks_Needed").ParameterValue) && H2ODHVCalcAdjVal != decimal.MinValue)
              CalcMoistureSO2 = H2ODHVCalcAdjVal;
            else
                if( H2OMethCd.InList( "MMS,MTB" ) && Convert.ToBoolean( Category.GetCheckParameter( "H2O_Monitor_Hourly_Checks_Needed" ).ParameterValue ) && H2OMHVCalcAdjVal != decimal.MinValue )
                CalcMoistureSO2 = H2OMHVCalcAdjVal;
              else
                if (H2OMethCd == "MDF" && Convert.ToBoolean(Category.GetCheckParameter("H2O_Derived_Hourly_Checks_Needed").ParameterValue) && H2ODHVCalcAdjVal != decimal.MinValue)
                  CalcMoistureSO2 = H2ODHVCalcAdjVal;
                else
                  if (H2OMethCd == "MDF" && !Convert.ToBoolean(Category.GetCheckParameter("H2O_Derived_Hourly_Checks_Needed").ParameterValue) && H2ODefVal != decimal.MinValue)
                    CalcMoistureSO2 = H2ODefVal;
            if (CalcMoistureSO2 != decimal.MinValue)
              Category.SetCheckParameter("Calculated_Moisture_for_SO2", CalcMoistureSO2, eParameterDataType.Decimal);
          }
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "HOURCV40");
      }

      return ReturnVal;
    }

    #endregion

    #region Checks 41 - 50

    public static string HOURCV41(cCategory Category, ref bool Log)
    //Check BAF Calculation in NOx Rate DHV Record
    {
      string ReturnVal = "";

      try
      {
          DataRowView CurrentDHVRecord = (DataRowView)Category.GetCheckParameter("Current_DHV_Record").ParameterValue;
          int thisLocation = cDBConvert.ToInteger(Category.GetCheckParameter("Current_Monitor_Plan_Location_Postion").ParameterValue);
          decimal AdjVal = cDBConvert.ToDecimal(CurrentDHVRecord["Adjusted_Hrly_Value"]);
          if (Convert.ToBoolean(Category.GetCheckParameter("Derived_Hourly_Adjusted_Value_Status").ParameterValue))
          {
              decimal[] NOxRateRepAccum = Category.GetCheckParameter("Rpt_Period_Nox_Rate_Reported_Accumulator_Array").ValueAsDecimalArray();
              if (NOxRateRepAccum[thisLocation] != decimal.MinValue)
              {
                  if (NOxRateRepAccum[thisLocation] >= 0)
                      Category.AccumCheckAggregate("Rpt_Period_Nox_Rate_Reported_Accumulator_Array", thisLocation, AdjVal);
              }
              else
                  Category.SetArrayParameter("Rpt_Period_Nox_Rate_Reported_Accumulator_Array", thisLocation, AdjVal);
          }
          else
              Category.SetArrayParameter("Rpt_Period_Nox_Rate_Reported_Accumulator_Array", thisLocation, -1m);
          decimal BAF = cDBConvert.ToDecimal(Category.GetCheckParameter("Current_NOX_System_Baf").ParameterValue);
          DataRowView HourlyOpDataRow = (DataRowView)Category.GetCheckParameter("Current_Hourly_Op_Record").ParameterValue;
          decimal OpTime = cDBConvert.ToDecimal(HourlyOpDataRow["OP_TIME"]);
          if (Category.GetCheckParameter("RATA_Status_Required").ValueAsBool() && BAF != decimal.MinValue)
          {
              decimal TempVal = Math.Round(Convert.ToDecimal(Category.GetCheckParameter("NOXR_Calculated_Unadjusted_Value").ParameterValue) *
                  BAF, 3, MidpointRounding.AwayFromZero);
              Category.SetCheckParameter("NOXR_Calculated_Adjusted_Value", TempVal, eParameterDataType.Decimal);
          }
          else
              if (Convert.ToString(Category.GetCheckParameter("Current_NOxR_Method_Code").ParameterValue) == "AE" &&
                   Category.GetCheckParameter("Hourly_Fuel_Flow_Count_For_Gas").ValueAsInt(0) +
                   Category.GetCheckParameter("Hourly_Fuel_Flow_Count_For_Oil").ValueAsInt(0) > 0 &&
                  !Convert.ToBoolean(Category.GetCheckParameter("App_E_Constant_Fuel_Mix").ParameterValue))
              {   
                decimal NOxAppEAccum = cDBConvert.ToDecimal(Category.GetCheckParameter("Noxr_App_E_Accumulator").ParameterValue);
                decimal HICalcAdjVal = cDBConvert.ToDecimal(Category.GetCheckParameter("HI_Calculated_Adjusted_Value").ParameterValue);
                decimal TotalHI = cDBConvert.ToDecimal(Category.GetCheckParameter("Total_Heat_Input_From_Fuel_Flow").ParameterValue);

                if (NOxAppEAccum >= 0 && TotalHI != decimal.MinValue)
                  {
                      decimal NoxCalcAdjVal;
                      if (NOxAppEAccum == 0)
                          NoxCalcAdjVal = 0;
                      else
                          NoxCalcAdjVal = Math.Round(NOxAppEAccum / TotalHI, 3, MidpointRounding.AwayFromZero);
                      Category.SetCheckParameter("NOXR_Calculated_Adjusted_Value", NoxCalcAdjVal, eParameterDataType.Decimal);
                  }
                  else
                  {
                      Category.SetArrayParameter("Apportionment_Calc_NOXR_Array", thisLocation, -1m);
                      Category.SetArrayParameter("Rpt_Period_Nox_Rate_Calculated_Accumulator_Array", thisLocation, -1m);
                      Category.CheckCatalogResult = "A";
                  }
              }
          decimal CalcAdjVal = cDBConvert.ToDecimal(Category.GetCheckParameter("NOXR_Calculated_Adjusted_Value").ParameterValue);
          if (CalcAdjVal != decimal.MinValue)
          {
              Category.SetArrayParameter("Apportionment_Calc_NOXR_Array", thisLocation, CalcAdjVal);
              decimal[] NOxRateCalcAccum = Category.GetCheckParameter("Rpt_Period_Nox_Rate_Calculated_Accumulator_Array").ValueAsDecimalArray();

              if (Convert.ToString(Category.GetCheckParameter("MP_Stack_Config_For_Hourly_Checks").ParameterValue) == "MS" &&
                  (Category.GetCheckParameter("Expected_Summary_Value_Nox_Rate_Array").ValueAsBoolArray())[thisLocation])
              {
                  if (Convert.ToDecimal(Category.GetCheckParameter("Config_NOxRateTimesHeatInput_Accumulator").ParameterValue) >= 0 && Category.GetCheckParameter("HI_Calculated_Adjusted_Value").ParameterValue != null)
                      Category.AccumCheckAggregate("Config_NOxRateTimesHeatInput_Accumulator", CalcAdjVal * Convert.ToDecimal(Category.GetCheckParameter("HI_Calculated_Adjusted_Value").ParameterValue));
                  else
                      Category.SetCheckParameter("Config_NOxRateTimesHeatInput_Accumulator", -1, eParameterDataType.Decimal);

                  if (Convert.ToDecimal(Category.GetCheckParameter("Config_NOxRateTimesOptime_Accumulator").ParameterValue) >= 0 && 0 < OpTime && OpTime <= 1)
                  {
                      Category.AccumCheckAggregate("Config_NOxRateTimesOptime_Accumulator", CalcAdjVal * OpTime);
                      Category.AccumCheckAggregate("Config_Optime_Accumulator", OpTime);
                  }
                  else
                      Category.SetCheckParameter("Config_NOxRateTimesOptime_Accumulator", -1, eParameterDataType.Decimal);
              }

              if (NOxRateCalcAccum[thisLocation] != decimal.MinValue)
              {
                  if (NOxRateCalcAccum[thisLocation] >= 0)
                      Category.AccumCheckAggregate("Rpt_Period_Nox_Rate_Calculated_Accumulator_Array", thisLocation, CalcAdjVal);
              }
              else
                  Category.SetArrayParameter("Rpt_Period_Nox_Rate_Calculated_Accumulator_Array", thisLocation, CalcAdjVal);

              Category.AccumCheckAggregate("Rpt_Period_Nox_Rate_Hours_Accumulator_Array", thisLocation, 1);

              if (Convert.ToBoolean(Category.GetCheckParameter("Derived_Hourly_Adjusted_Value_Status").ParameterValue) &&
                  Convert.ToBoolean(Category.GetCheckParameter("Derived_Hourly_Unadjusted_Calculation_Status").ParameterValue))
              {
                  decimal Tolerance = GetTolerance("NOXR", "LBMMBTU", Category);
                  if (Math.Abs(CalcAdjVal - AdjVal) > Tolerance)
                      Category.CheckCatalogResult = "B";
              }
          }
          else if (Convert.ToString(Category.GetCheckParameter("Current_NOxR_Method_Code").ParameterValue) != "AE" ||
                   Category.GetCheckParameter("Hourly_Fuel_Flow_Count_For_Gas").ValueAsInt(0) +
                   Category.GetCheckParameter("Hourly_Fuel_Flow_Count_For_Oil").ValueAsInt(0) > 0) 
          {
              Category.SetArrayParameter("Apportionment_Calc_NOXR_Array", thisLocation, -1m);
              Category.SetArrayParameter("Rpt_Period_Nox_Rate_Calculated_Accumulator_Array", thisLocation, -1m);
              if (Convert.ToString(Category.GetCheckParameter("MP_Stack_Config_For_Hourly_Checks").ParameterValue) == "MS")
                  Category.SetCheckParameter("Config_NOxRateTimesHeatInput_Accumulator", -1, eParameterDataType.Decimal);
              if (Category.GetCheckParameter("RATA_Status_Required").ValueAsBool() && BAF == decimal.MinValue &&
                  Category.GetCheckParameter("NOXR_Calculated_Unadjusted_Value").ParameterValue != null)
                  Category.CheckCatalogResult = "A";
          }
      }
      catch (Exception ex)
      {
          ReturnVal = Category.CheckEngine.FormatError(ex, "HOURCV41");
      }

      return ReturnVal;
    }

        public static string HOURCV42(cCategory Category, ref bool Log)
        // Check HI System in DHV Record
        {
            string ReturnVal = "";

            try
            {
                string CurrentHIMethod = Category.GetCheckParameter("Heat_Input_Method_Code").ValueAsString();

                if (Category.GetCheckParameter("Current_DHV_Parameter").ValueAsString() == "HI" && CurrentHIMethod.InList("CEM,AMS"))
                {
                    DataRowView CurrentCo2cMonitorHourlyRecord = Category.GetCheckParameter("Current_CO2_Conc_Monitor_Hourly_Record").ValueAsDataRowView();
                    DataRowView CurrentDhvRecord = (DataRowView)Category.GetCheckParameter("Current_DHV_Record").ParameterValue;
                    DataRowView CurrentO2wMonitorHourlyRecord = Category.GetCheckParameter("Current_O2_Wet_Monitor_Hourly_Record").ValueAsDataRowView();
                    DataRowView CurrentO2dMonitorHourlyRecord = Category.GetCheckParameter("Current_O2_Dry_Monitor_Hourly_Record").ValueAsDataRowView();

                    if (CurrentDhvRecord["MON_SYS_ID"] == DBNull.Value)
                    {
                        if (CurrentHIMethod == "CEM")
                        {
                            if (((Category.GetCheckParameter("CO2_Conc_Checks_Needed_for_Heat_Input").ValueAsBool() &&
                                (CurrentCo2cMonitorHourlyRecord != null) &&
                                cDBConvert.ToString(CurrentCo2cMonitorHourlyRecord["MODC_CD"]).InList("01,02,03,04,17,20,21")) ||
                                (Category.GetCheckParameter("O2_Wet_Checks_Needed_for_Heat_Input").ValueAsBool() &&
                                (CurrentO2wMonitorHourlyRecord != null) &&
                                cDBConvert.ToString(CurrentO2wMonitorHourlyRecord["MODC_CD"]).InList("01,02,03,04,17,20")) ||
                                (Category.GetCheckParameter("O2_Dry_Checks_Needed_for_Heat_Input").ValueAsBool() &&
                                (CurrentO2dMonitorHourlyRecord != null) &&
                                cDBConvert.ToString(CurrentO2dMonitorHourlyRecord["MODC_CD"]).InList("01,02,03,04,17,20"))))
                                Category.CheckCatalogResult = "A";
                            else
                                Category.CheckCatalogResult = "E";
                        }
                    }
                    else
                    {
                        string MonitorSystemId = cDBConvert.ToString(CurrentDhvRecord["MON_SYS_ID"]);

                        sFilterPair[] RowFilter;
                        RowFilter = new sFilterPair[1];
                        RowFilter[0].Set("MON_SYS_ID", MonitorSystemId);

                        DataView MonitorSystemRecords = Category.GetCheckParameter("Monitor_System_Records_By_Hour_Location").ValueAsDataView();
                        DataRowView CurrentDhvMonSysRecord = FindRow(MonitorSystemRecords, RowFilter);

                        if (CurrentDhvMonSysRecord == null)
                            Category.CheckCatalogResult = "C";
                        else if (!cDBConvert.ToString(CurrentDhvMonSysRecord["SYS_TYPE_CD"]).InList("CO2,O2"))
                            Category.CheckCatalogResult = "D";
                        else if ((CurrentHIMethod != "CEM") ||
                                 ((Category.GetCheckParameter("CO2_Conc_Checks_Needed_for_Heat_Input").ValueAsBool() &&
                                  (CurrentCo2cMonitorHourlyRecord != null) &&
                                  cDBConvert.ToString(CurrentCo2cMonitorHourlyRecord["MODC_CD"]).InList("01,02,03,04,17,20,21"))) ||
                                 ((Category.GetCheckParameter("O2_Wet_Checks_Needed_for_Heat_Input").ValueAsBool() &&
                                  (CurrentO2wMonitorHourlyRecord != null) &&
                                  cDBConvert.ToString(CurrentO2wMonitorHourlyRecord["MODC_CD"]).InList("01,02,03,04,17,20"))) ||
                                 ((Category.GetCheckParameter("O2_Dry_Checks_Needed_for_Heat_Input").ValueAsBool() &&
                                  (CurrentO2dMonitorHourlyRecord != null) &&
                                  cDBConvert.ToString(CurrentO2dMonitorHourlyRecord["MODC_CD"]).InList("01,02,03,04,17,20"))))
                        {
                            if (Category.GetCheckParameter("CO2_RATA_Required").ValueAsBool())
                                Category.SetCheckParameter("RATA_Status_Required", true, eParameterDataType.Boolean);
                        }
                    }
                }
                else
                    Log = false;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURCV42");
            }

            return ReturnVal;
        }

    public static string HOURCV43(cCategory category, ref bool log)
    // Determine DHV Measure Code
    {
      string returnVal = "";

      try
      {
        string[] monitorMeasureCodeArray = category.GetCheckParameter("Monitor_Measure_Code_Array").ValueAsStringArray();

        string currentMeasureCode = null;

        string currentDhvParameter = category.GetCheckParameter("Current_DHV_Parameter").AsString();

        // CO2C and H2O
        if (currentDhvParameter.InList("CO2C,H2O"))
        {
          eHourMeasureParameter hourMeasureParameter = currentDhvParameter.AsHourMeasureParameter().Value;

          string co2ConcCemEquationCode = category.GetCheckParameter("CO2_Conc_Cem_Equation_Code").AsString();

          DataRowView currentDhvRecord = category.GetCheckParameter("Current_DHV_Record").AsDataRowView();
          string modcCd = currentDhvRecord["MODC_CD"].AsString();

          if (modcCd.InList("01,02,03,04,21,53,54"))
          {
            monitorMeasureCodeArray[(int)hourMeasureParameter] = "MEASURE";

            if ((currentDhvParameter == "CO2C") && (co2ConcCemEquationCode == "F-14B") &&
                (monitorMeasureCodeArray[(int)eHourMeasureParameter.H2o] == "SUB"))
              monitorMeasureCodeArray[(int)eHourMeasureParameter.Co2c] = "MEASSUB";
          }
          else if (modcCd.InList("06,07,08,09,10,12,55"))
          {
            monitorMeasureCodeArray[(int)hourMeasureParameter] = "SUB";

            if ((currentDhvParameter == "CO2C") && (co2ConcCemEquationCode == "F-14B") &&
                (monitorMeasureCodeArray[(int)eHourMeasureParameter.H2o] == "MEASURE"))
              monitorMeasureCodeArray[(int)eHourMeasureParameter.Co2c] = "MEASSUB";
          }

          category.SetArrayParameter("Monitor_Measure_Code_Array", monitorMeasureCodeArray);
        }

        // NOXR
        else if (currentDhvParameter == "NOXR")
        {
          string currentNoxRateMethodCode = category.GetCheckParameter("Current_NOxR_Method_Code").AsString();

          if (currentNoxRateMethodCode.InList("CEM,PEM"))
          {
            string noxRateEquationCode = category.GetCheckParameter("Nox_Rate_Equation_Code").AsString();

            DataRowView currentDhvRecord = category.GetCheckParameter("Current_DHV_Record").AsDataRowView();
            string modcCd = currentDhvRecord["MODC_CD"].AsString();

            if (modcCd.InList("01,02,03,04,05,14,21,22,53,54"))
            {
              currentMeasureCode = "MEASURE";

              if (noxRateEquationCode.InList("19-3,19-3D,19-4,19-5,19-8,19-9") &&
                  (monitorMeasureCodeArray[(int)eHourMeasureParameter.H2o] == "SUB"))
                currentMeasureCode = "MEASSUB";
            }
            else if (modcCd.InList("06,07,08,09,10,11,12,13,15,23,24,25,55"))
            {
              currentMeasureCode = "SUB";

              if (noxRateEquationCode.InList("19-3,19-3D,19-4,19-5,19-8,19-9") &&
                  (monitorMeasureCodeArray[(int)eHourMeasureParameter.H2o] == "MEASURE"))
                currentMeasureCode = "MEASSUB";
            }
          }
          else if (currentNoxRateMethodCode == "AE")
          {
           currentMeasureCode = monitorMeasureCodeArray[(int)eHourMeasureParameter.Noxr];
          }

          category.SetCheckParameter("NOXR_Measure_Code", currentMeasureCode, eParameterDataType.String);
        }

        // HI
        else if (currentDhvParameter == "HI")
        {
          if (category.GetCheckParameter("Heat_Input_Method_Code").AsString() == "CEM")
          {
            string heatInputEquationCode = category.GetCheckParameter("Heat_Input_CEM_Equation_Code").AsString();

            if (heatInputEquationCode.InList("F-15,F-16"))
            {
              if ((monitorMeasureCodeArray[(int)eHourMeasureParameter.Co2c] == "MEASURE") &&
                  (monitorMeasureCodeArray[(int)eHourMeasureParameter.Flow] == "MEASURE"))
                currentMeasureCode = "MEASURE";
              else if ((monitorMeasureCodeArray[(int)eHourMeasureParameter.Co2c] == "SUB") &&
                       (monitorMeasureCodeArray[(int)eHourMeasureParameter.Flow] == "SUB"))
                currentMeasureCode = "SUB";
              else if (monitorMeasureCodeArray[(int)eHourMeasureParameter.Co2c].IsNotEmpty() &&
                       monitorMeasureCodeArray[(int)eHourMeasureParameter.Flow].IsNotEmpty())
                currentMeasureCode = "MEASSUB";
            }
            else if (heatInputEquationCode == "F-18")
            {
              if ((monitorMeasureCodeArray[(int)eHourMeasureParameter.O2d] == "MEASURE") &&
                  (monitorMeasureCodeArray[(int)eHourMeasureParameter.Flow] == "MEASURE"))
                currentMeasureCode = "MEASURE";
              else if ((monitorMeasureCodeArray[(int)eHourMeasureParameter.O2d] == "SUB") &&
                       (monitorMeasureCodeArray[(int)eHourMeasureParameter.Flow] == "SUB"))
                currentMeasureCode = "SUB";
              else if (monitorMeasureCodeArray[(int)eHourMeasureParameter.O2d].IsNotEmpty() &&
                       monitorMeasureCodeArray[(int)eHourMeasureParameter.Flow].IsNotEmpty())
                currentMeasureCode = "MEASSUB";
            }
            else if (heatInputEquationCode == "F-17")
            {
              if ((monitorMeasureCodeArray[(int)eHourMeasureParameter.O2w] == "MEASURE") &&
                  (monitorMeasureCodeArray[(int)eHourMeasureParameter.Flow] == "MEASURE"))
                currentMeasureCode = "MEASURE";
              else if ((monitorMeasureCodeArray[(int)eHourMeasureParameter.O2w] == "SUB") &&
                       (monitorMeasureCodeArray[(int)eHourMeasureParameter.Flow] == "SUB"))
                currentMeasureCode = "SUB";
              else if (monitorMeasureCodeArray[(int)eHourMeasureParameter.O2w].IsNotEmpty() &&
                       monitorMeasureCodeArray[(int)eHourMeasureParameter.Flow].IsNotEmpty())
                currentMeasureCode = "MEASSUB";
            }

            if (heatInputEquationCode.InList("F-16,F-17,F-18") &&
                monitorMeasureCodeArray[(int)eHourMeasureParameter.H2o].IsNotEmpty())
            {
              if (((currentMeasureCode == "MEASURE") &&
                   (monitorMeasureCodeArray[(int)eHourMeasureParameter.H2o] == "SUB")) ||
                  ((currentMeasureCode == "SUB") &&
                   (monitorMeasureCodeArray[(int)eHourMeasureParameter.H2o] == "MEASURE")))
                currentMeasureCode = "MEASSUB";
            }
          }
          else if (category.GetCheckParameter("Heat_Input_App_D_Method_Active_For_Hour").AsBoolean().Default())
          {
            if (monitorMeasureCodeArray[(int)eHourMeasureParameter.Ff].InList("OTHER,MEASSUB"))
              currentMeasureCode = monitorMeasureCodeArray[(int)eHourMeasureParameter.Ff];
            else if ((monitorMeasureCodeArray[(int)eHourMeasureParameter.Ff] == "MEASURE") &&
                     (monitorMeasureCodeArray[(int)eHourMeasureParameter.Gcv] == "MEASURE"))
              currentMeasureCode = "MEASURE";
            else if ((monitorMeasureCodeArray[(int)eHourMeasureParameter.Ff] == "SUB") &&
                     (monitorMeasureCodeArray[(int)eHourMeasureParameter.Gcv] == "SUB"))
              currentMeasureCode = "SUB";
            else if (monitorMeasureCodeArray[(int)eHourMeasureParameter.Ff].IsNotEmpty() &&
                     monitorMeasureCodeArray[(int)eHourMeasureParameter.Gcv].IsNotEmpty())
              currentMeasureCode = "MEASSUB";

            if (monitorMeasureCodeArray[(int)eHourMeasureParameter.Density].IsNotEmpty())
            {
              if (((currentMeasureCode == "MEASURE") &&
                   (monitorMeasureCodeArray[(int)eHourMeasureParameter.Density] == "SUB")) ||
                  ((currentMeasureCode == "SUB") &&
                   (monitorMeasureCodeArray[(int)eHourMeasureParameter.Density] == "MEASURE")))
                currentMeasureCode = "MEASSUB";
            }
          }

          category.SetCheckParameter("HI_Measure_Code", currentMeasureCode, eParameterDataType.String);
        }

        // SO2
        else if (currentDhvParameter == "SO2")
        {
          if (category.GetCheckParameter("SO2_CEM_Method_Active_For_Hour").AsBoolean().Default())
          {
            if ((monitorMeasureCodeArray[(int)eHourMeasureParameter.So2c] == "MEASURE") &&
                (monitorMeasureCodeArray[(int)eHourMeasureParameter.Flow] == "MEASURE"))
              currentMeasureCode = "MEASURE";
            else if ((monitorMeasureCodeArray[(int)eHourMeasureParameter.So2c] == "SUB") &&
                     (monitorMeasureCodeArray[(int)eHourMeasureParameter.Flow] == "SUB"))
              currentMeasureCode = "SUB";
            else if (monitorMeasureCodeArray[(int)eHourMeasureParameter.So2c].IsNotEmpty() &&
                     monitorMeasureCodeArray[(int)eHourMeasureParameter.Flow].IsNotEmpty())
              currentMeasureCode = "MEASSUB";

            if ((category.GetCheckParameter("SO2_Equation_Code").AsString() == "F-2") &&
                monitorMeasureCodeArray[(int)eHourMeasureParameter.H2o].IsNotEmpty())
            {
              if (((currentMeasureCode == "MEASURE") &&
                   (monitorMeasureCodeArray[(int)eHourMeasureParameter.H2o] == "SUB")) ||
                  ((currentMeasureCode == "SUB") &&
                   (monitorMeasureCodeArray[(int)eHourMeasureParameter.H2o] == "MEASURE")))
                currentMeasureCode = "MEASSUB";
            }
          }
          else if (category.GetCheckParameter("SO2_F23_Method_Active_For_Hour").AsBoolean().Default())
          {
            currentMeasureCode = category.GetCheckParameter("HI_Measure_Code").AsString();
          }
          else if (category.GetCheckParameter("SO2_App_D_Method_Active_For_Hour").AsBoolean().Default())
          {
            if (monitorMeasureCodeArray[(int)eHourMeasureParameter.Ff].InList("OTHER,MEASSUB") ||
                monitorMeasureCodeArray[(int)eHourMeasureParameter.Sulfer].IsEmpty())
              currentMeasureCode = monitorMeasureCodeArray[(int)eHourMeasureParameter.Ff];
            else if ((monitorMeasureCodeArray[(int)eHourMeasureParameter.Ff] == "MEASURE") &&
                     (monitorMeasureCodeArray[(int)eHourMeasureParameter.Sulfer] == "MEASURE"))
              currentMeasureCode = "MEASURE";
            else if ((monitorMeasureCodeArray[(int)eHourMeasureParameter.Ff] == "SUB") &&
                     (monitorMeasureCodeArray[(int)eHourMeasureParameter.Sulfer] == "SUB"))
              currentMeasureCode = "SUB";
            else if (monitorMeasureCodeArray[(int)eHourMeasureParameter.Ff].IsNotEmpty() &&
                     monitorMeasureCodeArray[(int)eHourMeasureParameter.Sulfer].IsNotEmpty())
              currentMeasureCode = "MEASSUB";
          }
        }

        // CO2
        else if (currentDhvParameter == "CO2")
        {
          if (category.GetCheckParameter("CO2_Method_Code").AsString() == "CEM")
          {
            if ((monitorMeasureCodeArray[(int)eHourMeasureParameter.Co2c] == "MEASURE") &&
                (monitorMeasureCodeArray[(int)eHourMeasureParameter.Flow] == "MEASURE"))
              currentMeasureCode = "MEASURE";
            else if ((monitorMeasureCodeArray[(int)eHourMeasureParameter.Co2c] == "SUB") &&
                     (monitorMeasureCodeArray[(int)eHourMeasureParameter.Flow] == "SUB"))
              currentMeasureCode = "SUB";
            else if (monitorMeasureCodeArray[(int)eHourMeasureParameter.Co2c].IsNotEmpty() &&
                     monitorMeasureCodeArray[(int)eHourMeasureParameter.Flow].IsNotEmpty())
              currentMeasureCode = "MEASSUB";

            if ((category.GetCheckParameter("CO2_Mass_CEM_Equation_Code").AsString() == "F-2") &&
                monitorMeasureCodeArray[(int)eHourMeasureParameter.H2o].IsNotEmpty())
            {
              if (((currentMeasureCode == "MEASURE") &&
                   (monitorMeasureCodeArray[(int)eHourMeasureParameter.H2o] == "SUB")) ||
                  ((currentMeasureCode == "SUB") &&
                   (monitorMeasureCodeArray[(int)eHourMeasureParameter.H2o] == "MEASURE")))
                currentMeasureCode = "MEASSUB";
            }
          }
          else if (category.GetCheckParameter("CO2_App_D_Method_Active_For_Hour").AsBoolean().Default())
          {
            currentMeasureCode = category.GetCheckParameter("HI_Measure_Code").AsString();
          }
        }

        // NOX
        else if (currentDhvParameter == "NOX")
        {
          string noxMassEquationCode = category.GetCheckParameter("NOx_Mass_Equation_Code").AsString();

          if (noxMassEquationCode == "F-24A")
          {
            string hiMeasureCode = category.GetCheckParameter("HI_Measure_Code").AsString();
            string noxrMeasureCode = category.GetCheckParameter("NOXR_Measure_Code").AsString();

            if ((hiMeasureCode == "MEASURE") && (noxrMeasureCode == "MEASURE"))
              currentMeasureCode = "MEASURE";
            else if ((hiMeasureCode == "SUB") && (noxrMeasureCode == "SUB"))
              currentMeasureCode = "SUB";
            else if (hiMeasureCode.IsNotEmpty() && noxrMeasureCode.IsNotEmpty())
              currentMeasureCode = "MEASSUB";
          }
          else if (noxMassEquationCode.InList("F-26A,F-26B"))
          {
            if ((monitorMeasureCodeArray[(int)eHourMeasureParameter.Noxc] == "MEASURE") &&
                (monitorMeasureCodeArray[(int)eHourMeasureParameter.Flow] == "MEASURE"))
              currentMeasureCode = "MEASURE";
            else if ((monitorMeasureCodeArray[(int)eHourMeasureParameter.Noxc] == "SUB") &&
                     (monitorMeasureCodeArray[(int)eHourMeasureParameter.Flow] == "SUB"))
              currentMeasureCode = "SUB";
            else if (monitorMeasureCodeArray[(int)eHourMeasureParameter.Noxc].IsNotEmpty() &&
                     monitorMeasureCodeArray[(int)eHourMeasureParameter.Flow].IsNotEmpty())
              currentMeasureCode = "MEASSUB";

            if ((noxMassEquationCode == "F-26B") &&
                monitorMeasureCodeArray[(int)eHourMeasureParameter.H2o].IsNotEmpty())
            {
              if (((currentMeasureCode == "MEASURE") &&
                   (monitorMeasureCodeArray[(int)eHourMeasureParameter.H2o] == "SUB")) ||
                  ((currentMeasureCode == "SUB") &&
                   (monitorMeasureCodeArray[(int)eHourMeasureParameter.H2o] == "MEASURE")))
                currentMeasureCode = "MEASSUB";
            }
          }
        }

        // CO2M, HIT, NOXM or SO2M
        else if (currentDhvParameter.InList("SO2M,NOXM,CO2M,HIT"))
        {
          currentMeasureCode = "LME";
        }

        category.SetCheckParameter("Current_Measure_Code", currentMeasureCode, eParameterDataType.String);
      }
      catch (Exception ex)
      {
        returnVal = category.CheckEngine.FormatError(ex, "HOURCV43");
      }

      return returnVal;
    }

    #endregion

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
