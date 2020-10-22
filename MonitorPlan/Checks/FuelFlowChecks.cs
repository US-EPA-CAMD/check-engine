using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

namespace ECMPS.Checks.FuelFlowChecks
{
  public class cFuelFlowChecks : cChecks
  {

    #region Constructors

    public cFuelFlowChecks()
    {
      CheckProcedures = new dCheckProcedure[20];

      CheckProcedures[ 2] = new dCheckProcedure(FUELFLW2);
      CheckProcedures[ 3] = new dCheckProcedure(FUELFLW3);
      CheckProcedures[ 4] = new dCheckProcedure(FUELFLW4);
      CheckProcedures[ 5] = new dCheckProcedure(FUELFLW5);
      CheckProcedures[ 6] = new dCheckProcedure(FUELFLW6);
      CheckProcedures[ 7] = new dCheckProcedure(FUELFLW7);
      CheckProcedures[ 8] = new dCheckProcedure(FUELFLW8);
      CheckProcedures[10] = new dCheckProcedure(FUELFLW10);
      CheckProcedures[11] = new dCheckProcedure(FUELFLW11);
      CheckProcedures[17] = new dCheckProcedure(FUELFLW17);
      CheckProcedures[18] = new dCheckProcedure(FUELFLW18);
      CheckProcedures[19] = new dCheckProcedure(FUELFLW19);
    }

    #endregion

    #region Public Static Methods: Checks

    public static string FUELFLW2(cCategory Category, ref bool Log)
    // Fuel Flow Maximum Fuel Flow Rate Valid
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentFuelFlow = (DataRowView)Category.GetCheckParameter("Current_Fuel_Flow").ParameterValue;

        if (CurrentFuelFlow["MAX_RATE"] == DBNull.Value)
          Category.CheckCatalogResult = "A";
        else
        {
          decimal MaximumFuelFlowRate = cDBConvert.ToDecimal(CurrentFuelFlow["MAX_RATE"]);

          if (MaximumFuelFlowRate <= 0)
            Category.CheckCatalogResult = "B";
        }
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "FUELFLW2");
      }

      return ReturnVal;
    }

    public static string FUELFLW3(cCategory Category, ref bool Log)
    // Fuel Flow Begin Date Valid
    {
      string ReturnVal = "";

      try
      {
        ReturnVal = Check_ValidStartDate(Category, "Fuel_Flow_Start_Date_Valid", "Current_Fuel_Flow");
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "FUELFLW3");
      }

      return ReturnVal;
    }

    public static string FUELFLW4(cCategory Category, ref bool Log)
    // Fuel Flow Begin Hour Valid
    {
      string ReturnVal = "";

      try
      {
        ReturnVal = Check_ValidStartHour(Category, "Fuel_Flow_Start_Hour_Valid", "Current_Fuel_Flow");
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "FUELFLW4");
      }

      return ReturnVal;
    }

    public static string FUELFLW5(cCategory Category, ref bool Log)
    // Fuel Flow End Date Valid
    {
      string ReturnVal = "";

      try
      {
        ReturnVal = Check_ValidEndDate(Category, "Fuel_Flow_End_Date_Valid", "Current_Fuel_Flow");
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "FUELFLW5");
      }

      return ReturnVal;
    }

    public static string FUELFLW6(cCategory Category, ref bool Log)
    // Fuel Flow End Hour Valid
    {
      string ReturnVal = "";

      try
      {
        ReturnVal = Check_ValidEndHour(Category, "Fuel_Flow_End_Hour_Valid", "Current_Fuel_Flow");
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "FUELFLW6");
      }

      return ReturnVal;
    }

    public static string FUELFLW7(cCategory Category, ref bool Log)
    // Fuel Flow Dates and Hours Consistent
    {
      string ReturnVal = "";

      try
      {
        ReturnVal = Check_ConsistentHourRange(Category, "Fuel_Flow_Dates_and_Hours_Consistent",
                                                        "Current_Fuel_Flow",
                                                        "Fuel_Flow_Start_Date_Valid",
                                                        "Fuel_Flow_Start_Hour_Valid",
                                                        "Fuel_Flow_End_Date_Valid",
                                                        "Fuel_Flow_End_Hour_Valid");
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "FUELFLW7");
      }

      return ReturnVal;
    }

    public static string FUELFLW8(cCategory Category, ref bool Log)
    // Fuel Flow Maximum Fuel Flow Rate Source Code Valid
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentFuelFlow = (DataRowView)Category.GetCheckParameter("Current_Fuel_Flow").ParameterValue;

        if (CurrentFuelFlow["MAX_RATE_SOURCE_CD"] == DBNull.Value)
          Category.CheckCatalogResult = "A";
        else
        {
          string SourceCd = cDBConvert.ToString(CurrentFuelFlow["MAX_RATE_SOURCE_CD"]);
          DataView SourceCodeRecords = (DataView)Category.GetCheckParameter("Fuel_Flow_Maximum_Rate_Source_Code_Lookup_Table").ParameterValue;

          if (!LookupCodeExists(SourceCd, SourceCodeRecords))
            Category.CheckCatalogResult = "B";
        }
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "FUELFLW8");
      }

      return ReturnVal;
    }

    public static string FUELFLW10(cCategory Category, ref bool Log)
    // Fuel Flow Units of Measure Code Valid
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentFuelFlow = (DataRowView)Category.GetCheckParameter("Current_Fuel_Flow").ParameterValue;

        if (CurrentFuelFlow["SYS_FUEL_UOM_CD"] == DBNull.Value)
          Category.CheckCatalogResult = "A";
        else
        {
          DataRowView CurrentSystem = (DataRowView)Category.GetCheckParameter("Current_System").ParameterValue;
          DataView SystemTypeCodeRecords = (DataView)Category.GetCheckParameter("System_Type_Lookup_Table").ParameterValue;

          string UomCd = cDBConvert.ToString(CurrentFuelFlow["Sys_Fuel_Uom_Cd"]);
          string SystemTypeCd = cDBConvert.ToString(CurrentSystem["Sys_Type_Cd"]);
          string ParameterCd;

          bool FoundParametersUomRecord;

          if (LookupCodeValue("Sys_Type_Cd", "Parameter_Cd", SystemTypeCodeRecords, SystemTypeCd, out ParameterCd))
          {
            DataView ParameterUomRecords = (DataView)Category.GetCheckParameter("Parameter_Units_Of_Measure_Lookup_Table").ParameterValue;
            sFilterPair[] ParameterUomFilter = new sFilterPair[2];

            ParameterUomFilter[0].Set("Uom_Cd", UomCd);
            ParameterUomFilter[1].Set("Parameter_Cd", ParameterCd);

            FoundParametersUomRecord = (CountRows(ParameterUomRecords, ParameterUomFilter) > 0);
          }
          else
            FoundParametersUomRecord = false;

          if (!FoundParametersUomRecord)
          {
            DataView UomRecords = (DataView)Category.GetCheckParameter("Units_of_Measure_Code_Lookup_Table").ParameterValue;
            sFilterPair[] UomFilter = new sFilterPair[1];

            UomFilter[0].Set("Uom_Cd", UomCd);

            if (CountRows(UomRecords, UomFilter) == 0)
              Category.CheckCatalogResult = "B";
            else
              Category.CheckCatalogResult = "C";
          }
        }
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "FUELFLW10");
      }

      return ReturnVal;
    }

    public static string FUELFLW11(cCategory Category, ref bool Log)
    // Fuel Flow Active Status
    {
      string ReturnVal = "";

      try
      {
        bool SystemDatesAndHoursConsistent = cDBConvert.ToBoolean(Category.GetCheckParameter("Fuel_Flow_Dates_and_Hours_Consistent").ParameterValue);

        if (SystemDatesAndHoursConsistent)
          ReturnVal = Check_ActiveHourRange(Category, "Fuel_Flow_Active_Status",
                                                      "Current_Fuel_Flow",
                                                      "Fuel_Flow_Evaluation_Start_Date",
                                                      "Fuel_Flow_Evaluation_Start_Hour",
                                                      "Fuel_Flow_Evaluation_End_Date",
                                                      "Fuel_Flow_Evaluation_End_Hour");
        else
          Log = false;
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "FUELFLW11");
      }

      return ReturnVal;
    }

    public static string FUELFLW17(cCategory Category, ref bool Log)
    // Overlapping Fuel Flow Records
    {
      string ReturnVal = "";

      try
      {
        if (cDBConvert.ToBoolean(Category.GetCheckParameter("Fuel_Flow_Dates_and_Hours_Consistent").ParameterValue))
        {
          DateTime EvalBeganDate = cDBConvert.ToDate(Category.GetCheckParameter("Fuel_Flow_Evaluation_Start_Date").ParameterValue, DateTypes.START);
          int EvalBeganHour = cDBConvert.ToHour(Category.GetCheckParameter("Fuel_Flow_Evaluation_Start_Hour").ParameterValue, DateTypes.START);
          DateTime EvalEndedDate = cDBConvert.ToDate(Category.GetCheckParameter("Fuel_Flow_Evaluation_End_Date").ParameterValue, DateTypes.END);
          int EvalEndedHour = cDBConvert.ToHour(Category.GetCheckParameter("Fuel_Flow_Evaluation_End_Hour").ParameterValue, DateTypes.END);

          DataRowView CurrentFuelFlow = (DataRowView)Category.GetCheckParameter("Current_Fuel_Flow").ParameterValue;
          string SysFuelId = cDBConvert.ToString(CurrentFuelFlow["Sys_Fuel_Id"]);
          DateTime CurrBeganDate = cDBConvert.ToDate(CurrentFuelFlow["Begin_Date"], DateTypes.START);
          int CurrBeganHour = cDBConvert.ToHour(CurrentFuelFlow["Begin_Hour"], DateTypes.START);

          DataView FuelFlowRecords = (DataView)Category.GetCheckParameter("Fuel_Flow_Records").ParameterValue;
          sFilterPair[] FuelFlowFilter = new sFilterPair[1];

          FuelFlowFilter[0].Set("Sys_Fuel_Id", SysFuelId, true);

          DataView FuelFlowView = FindActiveRows(FuelFlowRecords,
                                                 EvalBeganDate, EvalBeganHour, EvalEndedDate, EvalEndedHour,
                                                 FuelFlowFilter);

          if (FuelFlowView.Count > 0)
          {
            foreach (DataRowView FuelFlowRow in FuelFlowView)
            {
              DateTime BeganDate = cDBConvert.ToDate(FuelFlowRow["Begin_Date"], DateTypes.START);
              int BeganHour = cDBConvert.ToHour(FuelFlowRow["Begin_Hour"], DateTypes.START);

              if ((BeganDate > CurrBeganDate) || ((BeganDate == CurrBeganDate) && (BeganHour >= CurrBeganHour)))
                Category.CheckCatalogResult = "A";
            }
          }
        }
        else
          Log = false;
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "FUELFLW17");
      }

      return ReturnVal;
    }

    public static string FUELFLW18(cCategory Category, ref bool Log)
    // System and Fuelflow Dates Consistent
    {
      string ReturnVal = "";

      try
      {
        if (cDBConvert.ToBoolean(Category.GetCheckParameter("Fuel_Flow_Dates_and_Hours_Consistent").ParameterValue) &&
            cDBConvert.ToBoolean(Category.GetCheckParameter("System_Dates_And_Hours_Consistent").ParameterValue))
        {
          DataRowView CurrentFuelFlow = (DataRowView)Category.GetCheckParameter("Current_Fuel_Flow").ParameterValue;

          DateTime FuelFlowBeganDate = cDBConvert.ToDate(CurrentFuelFlow["Begin_Date"], DateTypes.START);
          int FuelFlowBeganHour = cDBConvert.ToHour(CurrentFuelFlow["Begin_Hour"], DateTypes.START);
          DateTime FuelFlowEndedDate = cDBConvert.ToDate(CurrentFuelFlow["End_Date"], DateTypes.END);
          int FuelFlowEndedHour = cDBConvert.ToHour(CurrentFuelFlow["End_Hour"], DateTypes.END);

          DataRowView CurrentMonitoringSystem = (DataRowView)Category.GetCheckParameter("Current_System").ParameterValue;

          DateTime SystemBeganDate = cDBConvert.ToDate(CurrentMonitoringSystem["Begin_Date"], DateTypes.START);
          int SystemBeganHour = cDBConvert.ToHour(CurrentMonitoringSystem["Begin_Hour"], DateTypes.START);
          DateTime SystemEndedDate = cDBConvert.ToDate(CurrentMonitoringSystem["End_Date"], DateTypes.END);
          int SystemEndedHour = cDBConvert.ToHour(CurrentMonitoringSystem["End_Hour"], DateTypes.END);

          if (SystemBeganDate > FuelFlowBeganDate)
            Category.CheckCatalogResult = "A";
          else if (SystemBeganDate == FuelFlowBeganDate && SystemBeganHour > FuelFlowBeganHour)
            Category.CheckCatalogResult = "A";
          else if (SystemEndedDate != DateTime.MaxValue && FuelFlowEndedDate == DateTime.MaxValue)
            Category.CheckCatalogResult = "A";
          else if (SystemEndedDate < FuelFlowEndedDate)
            Category.CheckCatalogResult = "A";
          else if (SystemEndedDate == FuelFlowEndedDate && SystemEndedHour < FuelFlowEndedHour)
            Category.CheckCatalogResult = "A";
        }
        else
          Log = false;
      }
      catch (Exception ex)
      { 
        ReturnVal = Category.CheckEngine.FormatError(ex, "FUELFLW18"); 
      }

      return ReturnVal;

    }

    public static string FUELFLW19(cCategory Category, ref bool Log)
    // Duplicate System Fuel Flow Records
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentFuelFlow = (DataRowView)Category.GetCheckParameter("Current_Fuel_Flow").ParameterValue;
        string SystemFuelId = cDBConvert.ToString(CurrentFuelFlow["Sys_Fuel_Id"]);
        string MonSysId = cDBConvert.ToString(CurrentFuelFlow["Mon_Sys_Id"]);

        DataView FuelFlowRecords = (DataView)Category.GetCheckParameter("Fuel_Flow_Records").ParameterValue;
        sFilterPair[] FuelFlowFilter = new sFilterPair[4];

        FuelFlowFilter[0].Set("Mon_Sys_Id", MonSysId);
        FuelFlowFilter[1].Set("Begin_Date", CurrentFuelFlow["Begin_Date"], eFilterDataType.DateBegan);
        FuelFlowFilter[2].Set("Begin_Hour", CurrentFuelFlow["Begin_Hour"], eFilterDataType.Integer);
        FuelFlowFilter[3].Set("Sys_Fuel_Id", SystemFuelId, true);

        DataView FuelFlowView = FindRows(FuelFlowRecords, FuelFlowFilter);

        if (FuelFlowView.Count > 0)
          Category.CheckCatalogResult = "A";
        else if (CurrentFuelFlow["End_Date"] != DBNull.Value)
        {
          FuelFlowFilter[1].Set("End_Date", CurrentFuelFlow["End_Date"], eFilterDataType.DateEnded);
          FuelFlowFilter[2].Set("End_Hour", CurrentFuelFlow["End_Hour"], eFilterDataType.Integer);

          FuelFlowView = FindRows(FuelFlowRecords, FuelFlowFilter);

          if (FuelFlowView.Count > 0)
            Category.CheckCatalogResult = "A";
        }
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "FUELFLW19");
      }

      return ReturnVal;
    }

    #endregion

  }
}
