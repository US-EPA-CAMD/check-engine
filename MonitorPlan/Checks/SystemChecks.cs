using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Extensions;


namespace ECMPS.Checks.SystemChecks
{
  public class cSystemChecks : cChecks
  {
    public cSystemChecks()
    {
      CheckProcedures = new dCheckProcedure[25];

      CheckProcedures[1] = new dCheckProcedure(SYSTEM1);
      CheckProcedures[2] = new dCheckProcedure(SYSTEM2);
      CheckProcedures[3] = new dCheckProcedure(SYSTEM3);
      CheckProcedures[4] = new dCheckProcedure(SYSTEM4);
      CheckProcedures[5] = new dCheckProcedure(SYSTEM5);
      CheckProcedures[6] = new dCheckProcedure(SYSTEM6);
      CheckProcedures[7] = new dCheckProcedure(SYSTEM7);
      CheckProcedures[8] = new dCheckProcedure(SYSTEM8);
      CheckProcedures[9] = new dCheckProcedure(SYSTEM9);
      CheckProcedures[10] = new dCheckProcedure(SYSTEM10);
      CheckProcedures[11] = new dCheckProcedure(SYSTEM11);
      CheckProcedures[12] = new dCheckProcedure(SYSTEM12);
      CheckProcedures[13] = new dCheckProcedure(SYSTEM13);
      CheckProcedures[14] = new dCheckProcedure(SYSTEM14);
      CheckProcedures[15] = new dCheckProcedure(SYSTEM15);
      CheckProcedures[16] = new dCheckProcedure(SYSTEM16);
      CheckProcedures[17] = new dCheckProcedure(SYSTEM17);
      CheckProcedures[18] = new dCheckProcedure(SYSTEM18);
      CheckProcedures[19] = new dCheckProcedure(SYSTEM19);
      CheckProcedures[20] = new dCheckProcedure(SYSTEM20);
      CheckProcedures[21] = new dCheckProcedure(SYSTEM21);
      CheckProcedures[22] = new dCheckProcedure(SYSTEM22);
      CheckProcedures[23] = new dCheckProcedure(SYSTEM23);
      CheckProcedures[24] = new dCheckProcedure(SYSTEM24);
    }

    public static string SYSTEM1(cCategory Category, ref bool Log)
    // System Begin Date Valid
    {
      string ReturnVal = "";

      try
      {
        Category.SetCheckParameter("System_Record_Valid", false, eParameterDataType.Boolean);
        ReturnVal = Check_ValidStartDate(Category, "System_Start_Date_valid", "Current_System");
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "SYSTEM1");
      }

      return ReturnVal;
    }

    public static string SYSTEM2(cCategory Category, ref bool Log)
    // System Begin Hour Valid
    {
      string ReturnVal = "";

      try
      {
        ReturnVal = Check_ValidStartHour(Category, "System_start_hour_valid", "current_System");
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "SYSTEM2");
      }

      return ReturnVal;
    }

    public static string SYSTEM3(cCategory Category, ref bool Log)
    // System End Date Valid
    {
      string ReturnVal = "";

      try
      {
        ReturnVal = Check_ValidEndDate(Category, "System_end_date_valid", "current_System");
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "SYSTEM3");
      }

      return ReturnVal;
    }

    public static string SYSTEM4(cCategory Category, ref bool Log)
    // System End Hour Valid
    {
      string ReturnVal = "";

      try
      {
        ReturnVal = Check_ValidEndHour(Category, "System_end_hour_valid", "current_System");
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "SYSTEM4");
      }

      return ReturnVal;
    }

    public static string SYSTEM5(cCategory Category, ref bool Log)
    // System Dates and Hours Consistent
    {
      string ReturnVal = "";

      try
      {
        ReturnVal = Check_ConsistentHourRange(Category, "System_Dates_And_Hours_Consistent",
                                                        "Current_System",
                                                        "System_Start_Date_Valid",
                                                        "System_Start_Hour_Valid",
                                                        "System_End_Date_Valid",
                                                        "System_End_Hour_Valid");
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "SYSTEM5");
      }

      return ReturnVal;
    }

    public static string SYSTEM6(cCategory Category, ref bool Log)
    // System Active Status
    {
      string ReturnVal = "";

      try
      {
        Category.SetCheckParameter("System_Active", null, eParameterDataType.Boolean);

        bool SystemDatesAndHoursConsistent = cDBConvert.ToBoolean(Category.GetCheckParameter("System_Dates_And_Hours_Consistent").ParameterValue);

        if (SystemDatesAndHoursConsistent)
          ReturnVal = Check_ActiveHourRange(Category, "System_Active",
                                                      "Current_System",
                                                      "System_Evaluation_Begin_Date",
                                                      "System_Evaluation_Begin_Hour",
                                                      "System_Evaluation_End_Date",
                                                      "System_Evaluation_End_Hour");
        else
          Log = false;
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "SYSTEM6");
      }

      return ReturnVal;
    }

    public static string SYSTEM7(cCategory Category, ref bool Log)
    // Monitoring System ID Valid
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentSystem = (DataRowView)Category.GetCheckParameter("Current_System").ParameterValue;
        string SystemIdentifier = cDBConvert.ToString(CurrentSystem["System_Identifier"]);

        if (SystemIdentifier == "")
          Category.CheckCatalogResult = "A";
        else if (SystemIdentifier.Length != 3 || !(cStringValidator.IsAlphaNumeric(SystemIdentifier)))
          Category.CheckCatalogResult = "B";
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "SYSTEM7");
      }

      return ReturnVal;
    }

    public static string SYSTEM8(cCategory Category, ref bool Log)
    // System Type Code Valid
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentSystem = (DataRowView)Category.GetCheckParameter("Current_System").ParameterValue;
        string SystemIdentifier = cDBConvert.ToString(CurrentSystem["Sys_Type_Cd"]);

        if (CurrentSystem["Sys_Type_Cd"] == DBNull.Value)
        {
          Category.CheckCatalogResult = "A";
          Category.SetCheckParameter("System_Type_Code_Valid", false, eParameterDataType.Boolean);
        }
        else
        {
          DataView SystemTypeCodeView = (DataView)Category.GetCheckParameter("System_Type_Lookup_Table").ParameterValue;
          DataRowView SystemTypeCodeRow;
          sFilterPair[] SystemTypeFilter = new sFilterPair[1];

          SystemTypeFilter[0].Field = "Sys_Type_Cd";
          SystemTypeFilter[0].Value = cDBConvert.ToString(CurrentSystem["Sys_Type_Cd"]);

          if (!FindRow(SystemTypeCodeView, SystemTypeFilter, out SystemTypeCodeRow))
          {
            Category.CheckCatalogResult = "B";
            Category.SetCheckParameter("System_Type_Code_Valid", false, eParameterDataType.Boolean);
          }
          else
          {
            Category.SetCheckParameter("System_Parameter_Code", cDBConvert.ToString(SystemTypeCodeRow["Parameter_Cd"]), eParameterDataType.String);
            Category.SetCheckParameter("System_Record_Valid", true, eParameterDataType.Boolean);

            DataView UsedIdentifierView = (DataView)Category.GetCheckParameter("Used_Identifier_Records").ParameterValue;
            DataRowView UsedIdentifierRow;
            sFilterPair[] UsedIdentifierFilter = new sFilterPair[2];

            UsedIdentifierFilter[0].Field = "TABLE_CD";
            UsedIdentifierFilter[0].Value = "S";
            UsedIdentifierFilter[1].Field = "IDENTIFIER";
            UsedIdentifierFilter[1].Value = cDBConvert.ToString(CurrentSystem["System_Identifier"]);

            if (FindRow(UsedIdentifierView, UsedIdentifierFilter, out UsedIdentifierRow))
            {
              string TypeOrParameterCd = cDBConvert.ToString(UsedIdentifierRow["TYPE_OR_PARAMETER_CD"]);

              if (SystemIdentifier != TypeOrParameterCd)
              {
                if ((SystemIdentifier.PadRight(3).Substring(0, 3) == "H2O") &&
                    (TypeOrParameterCd.PadRight(3).Substring(0, 3) == "H2O"))
                  Category.CheckCatalogResult = "C";
                else
                  Category.CheckCatalogResult = "D";

                Category.SetCheckParameter("System_Type_Code_Valid", false, eParameterDataType.Boolean);
              }
              else
                Category.SetCheckParameter("System_Type_Code_Valid", true, eParameterDataType.Boolean);
            }
            else
              Category.SetCheckParameter("System_Type_Code_Valid", true, eParameterDataType.Boolean);
          }
        }
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "SYSTEM8");
      }

      return ReturnVal;
    }

    public static string SYSTEM9(cCategory Category, ref bool Log)
    // System Designation Code Valid
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentSystem = (DataRowView)Category.GetCheckParameter("Current_System").ParameterValue;
        Category.SetCheckParameter("System_Designation_Code_Valid", true, eParameterDataType.Boolean);
        bool SystemTypeCodeValid = cDBConvert.ToBoolean(Category.GetCheckParameter("System_Type_Code_Valid").ParameterValue);
        string SystemDesignationCode = cDBConvert.ToString(CurrentSystem["Sys_Designation_Cd"]);
        string SystemTypeCode = cDBConvert.ToString(CurrentSystem["Sys_Type_Cd"]);

        DataView SystemDesignationCodeLookupTable = (DataView)Category.GetCheckParameter("System_Designation_Code_Lookup_Table").ParameterValue;

            if (!LookupCodeExists(SystemDesignationCode, SystemDesignationCodeLookupTable))
            {
              Category.CheckCatalogResult = "C";
              Category.SetCheckParameter("System_Designation_Code_Valid", false, eParameterDataType.Boolean);
            }
        else if (CurrentSystem["Sys_Designation_Cd"] == DBNull.Value)
        {
          Category.CheckCatalogResult = "A";
          Category.SetCheckParameter("System_Designation_Code_Valid", false, eParameterDataType.Boolean);
        }
        else if ((SystemDesignationCode == "CI") && Category.GetCheckParameter("System_Type_Code_Valid").ValueAsBool()
                && !SystemTypeCode.InList( "SO2,NOX,NOXC,SO2R" ) )
            {
              Category.CheckCatalogResult = "B";
              Category.SetCheckParameter("System_Designation_Code_Valid", false, eParameterDataType.Boolean);
            }

            else if (SystemDesignationCode == "RM" && Category.GetCheckParameter("System_Type_Code_Valid").ValueAsBool() 
                && SystemTypeCode.InList("GAS,OILM,OILV,OP,NOXP,NOXE,HG,HCL,HF,ST"))
          {
            Category.CheckCatalogResult = "B";
            Category.SetCheckParameter("System_Designation_Code_Valid", false, eParameterDataType.Boolean);
          }
          else if ((SystemDesignationCode == "PB") && Category.GetCheckParameter("System_Type_Code_Valid").ValueAsBool()
                && !SystemTypeCode.InList( "NOX,NOXC" ) )
            {
              Category.CheckCatalogResult = "B";
              Category.SetCheckParameter("System_Designation_Code_Valid", false, eParameterDataType.Boolean);
            }
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "SYSTEM9");
      }

      return ReturnVal;
    }

    public static string SYSTEM10(cCategory Category, ref bool Log)
    // System Fuel Code Valid
    {
      string ReturnVal = "";

      try
      {
        Category.SetCheckParameter("System_Fuel_Code_Valid", true, eParameterDataType.Boolean);
        Category.SetCheckParameter("System_Unit_Fuel", null, eParameterDataType.String);

        DataRowView CurrentSystem = (DataRowView)Category.GetCheckParameter("Current_System").ParameterValue;
        string FuelCd = cDBConvert.ToString(CurrentSystem["Fuel_Cd"]);

        if (FuelCd == "")
        {
          Category.SetCheckParameter("System_Fuel_Code_Valid", false, eParameterDataType.Boolean);
          Category.CheckCatalogResult = "A";
        }
        else
        {
          DataView FuelCodeView = (DataView)Category.GetCheckParameter("Fuel_Code_Lookup_Table").ParameterValue;
          DataRowView FuelCodeRow;
          sFilterPair[] FuelFilter = new sFilterPair[1];

          FuelFilter[0].Field = "Fuel_Cd";
          FuelFilter[0].Value = FuelCd;

          if (!FindRow(FuelCodeView, FuelFilter, out FuelCodeRow))
          {
            Category.SetCheckParameter("System_Fuel_Code_Valid", false, eParameterDataType.Boolean);
            Category.CheckCatalogResult = "B";
          }
          else if (cDBConvert.ToBoolean(Category.GetCheckParameter("System_Type_Code_Valid").ParameterValue))
          {
            DataView SystemTypeToFuelGroupView = (DataView)Category.GetCheckParameter("System_Type_To_Fuel_Group_Cross_Check_Table").ParameterValue;
            DataRowView SystemTypeToFuelGroupRow;
            sFilterPair[] SystemTypeToFuelGroupFilter = new sFilterPair[2];

            SystemTypeToFuelGroupFilter[0].Field = "SystemTypeCode";
            SystemTypeToFuelGroupFilter[0].Value = cDBConvert.ToString(CurrentSystem["Sys_Type_Cd"]);
            SystemTypeToFuelGroupFilter[1].Field = "FuelGroupCode";
            SystemTypeToFuelGroupFilter[1].Value = cDBConvert.ToString(FuelCodeRow["Fuel_Group_Cd"]);

            if (!FindRow(SystemTypeToFuelGroupView, SystemTypeToFuelGroupFilter, out SystemTypeToFuelGroupRow))
            {
              Category.SetCheckParameter("System_Fuel_Code_Valid", false, eParameterDataType.String);
              Category.CheckCatalogResult = "C";
            }
            else
              Category.SetCheckParameter("System_Unit_Fuel", cDBConvert.ToString(FuelCodeRow["Unit_Fuel_Cd"]), eParameterDataType.String);
          }
        }
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "SYSTEM10");
      }

      return ReturnVal;
    }

    public static string SYSTEM11(cCategory Category, ref bool Log)
    // Overlapping Primary NFS Systems 
    {
      string ReturnVal = "";

      try
      {
        if (cDBConvert.ToBoolean(Category.GetCheckParameter("System_Type_Code_Valid").ParameterValue) &&
            cDBConvert.ToBoolean(Category.GetCheckParameter("System_Fuel_Code_Valid").ParameterValue))
        {
          DataRowView CurrentSystem = (DataRowView)Category.GetCheckParameter("Current_System").ParameterValue;
          string MonSysId = cDBConvert.ToString(CurrentSystem["Mon_Sys_Id"]);
          string SystemDesignationCd = cDBConvert.ToString(CurrentSystem["Sys_Designation_Cd"]);
          string FuelCd = cDBConvert.ToString(CurrentSystem["Fuel_Cd"]);
          string SystemTypeCd = cDBConvert.ToString(CurrentSystem["Sys_Type_Cd"]);

          if (SystemDesignationCd == "P")
          {
            if (FuelCd == "NFS")
            {
              DateTime SysEvalBeganDate = cDBConvert.ToDate(Category.GetCheckParameter("System_Evaluation_Begin_Date").ParameterValue, DateTypes.START);
              int SysEvalBeganHour = cDBConvert.ToHour(Category.GetCheckParameter("System_Evaluation_Begin_Hour").ParameterValue, DateTypes.START);
              DateTime SysEvalEndedDate = cDBConvert.ToDate(Category.GetCheckParameter("System_Evaluation_End_Date").ParameterValue, DateTypes.END);
              int SysEvalEndedHour = cDBConvert.ToHour(Category.GetCheckParameter("System_Evaluation_End_Hour").ParameterValue, DateTypes.END);

              DataView SystemView = (DataView)Category.GetCheckParameter("Monitor_System_Records").ParameterValue;
              sFilterPair[] SystemFilter = new sFilterPair[3];

              SystemFilter[0].Set("Sys_Type_Cd", SystemTypeCd);
              SystemFilter[1].Set("Sys_Designation_Cd", "P");
              SystemFilter[2].Set("Mon_Sys_Id", MonSysId, true);

              DataView OtherSystemView = FindActiveRows(SystemView,
                                                        SysEvalBeganDate, SysEvalBeganHour,
                                                        SysEvalEndedDate, SysEvalEndedHour,
                                                        SystemFilter);

              DateTime CurrentDateBegan = cDBConvert.ToDate(CurrentSystem["Begin_Date"], DateTypes.START);
              int CurrentHourBegan = cDBConvert.ToHour(CurrentSystem["Begin_Hour"], DateTypes.START);

              for (int SystemDex = (OtherSystemView.Count - 1); SystemDex >= 0; SystemDex--)
              {
                DateTime SystemDateBegan = cDBConvert.ToDate(OtherSystemView[SystemDex]["Begin_Date"], DateTypes.START);
                int SystemHourBegan = cDBConvert.ToHour(OtherSystemView[SystemDex]["Begin_Hour"], DateTypes.START);

                if ((SystemDateBegan < CurrentDateBegan) ||
                    ((SystemDateBegan == CurrentDateBegan) && (SystemHourBegan < CurrentHourBegan)))
                  OtherSystemView[SystemDex].Delete();
              }

              if (OtherSystemView.Count > 0)
              {
                  if( SystemTypeCd.InList( "NOXP,H2OT,H2OM,FLOW" ) )
                  Category.CheckCatalogResult = "A";
                else
                {
                  bool CheckAnalyzerRange = true;

                  if (SystemTypeCd == "NOX")
                  {
                    DataView UnitTypeRecords = (DataView)Category.GetCheckParameter("Location_Unit_Type_Records").ParameterValue;
                    sFilterPair[] UnitTypeFilter = new sFilterPair[1];

                    UnitTypeFilter[0].Set("Unit_Type_Cd", "CC");

                    CheckAnalyzerRange = (FindActiveRows(UnitTypeRecords,
                                                         SysEvalBeganDate, SysEvalEndedDate,
                                                         UnitTypeFilter).Count == 0);
                  }

                  if (CheckAnalyzerRange)
                  {
                    DataView CurrentSystemComponentView = (DataView)Category.GetCheckParameter("System_System_Component_Records").ParameterValue;
                    sFilterPair[] SystemComponentFilter = new sFilterPair[1];

                    SystemComponentFilter[0].Set("Component_Type_Cd", "SO2,NOX,CO2,O2", eFilterPairStringCompare.InList);

                    DataView SystemComponentFilterView = FindActiveRows(CurrentSystemComponentView,
                                                                        SysEvalBeganDate, SysEvalBeganHour,
                                                                        SysEvalEndedDate, SysEvalEndedHour,
                                                                        SystemComponentFilter);

                    DataView AnalyzerRangeView = (DataView)Category.GetCheckParameter("Location_Analyzer_Range_Records").ParameterValue;
                    sFilterPair[] AnalyzerRangeFilter = new sFilterPair[2];

                    AnalyzerRangeFilter[0].Set("Component_Id",
                                               ColumnToDatalist(SystemComponentFilterView, "Component_Id"),
                                               eFilterPairStringCompare.InList);

                    AnalyzerRangeFilter[1].Set("Analyzer_Range_Cd", "A");
                    DataView AnalyzerRangeAView = FindActiveRows(AnalyzerRangeView,
                                                                 SysEvalBeganDate, SysEvalBeganHour,
                                                                 SysEvalEndedDate, SysEvalEndedHour,
                                                                 AnalyzerRangeFilter);
                    AnalyzerRangeFilter[1].Set("Analyzer_Range_Cd", "H");
                    DataView AnalyzerRangeHView = FindActiveRows(AnalyzerRangeView,
                                                                 SysEvalBeganDate, SysEvalBeganHour,
                                                                 SysEvalEndedDate, SysEvalEndedHour,
                                                                 AnalyzerRangeFilter);
                    AnalyzerRangeFilter[1].Set("Analyzer_Range_Cd", "L");
                    DataView AnalyzerRangeLView = FindActiveRows(AnalyzerRangeView,
                                                                 SysEvalBeganDate, SysEvalBeganHour,
                                                                 SysEvalEndedDate, SysEvalEndedHour,
                                                                 AnalyzerRangeFilter);

                    if (AnalyzerRangeAView.Count > 0)
                      Category.CheckCatalogResult = "A";
                    else if ((AnalyzerRangeAView.Count == 0) &&
                             (((AnalyzerRangeHView.Count >= 1) && (AnalyzerRangeLView.Count == 0)) ||
                              ((AnalyzerRangeHView.Count == 0) && (AnalyzerRangeLView.Count >= 1))))
                    {
                      string AnalyzerRangeList = ((AnalyzerRangeHView.Count >= 1)) ? "H,A" : "L,A";

                      foreach (DataRowView OtherSystem in OtherSystemView)
                      {
                        DataView OtherSystemComponentView = (DataView)Category.GetCheckParameter("Location_System_Component_Records").ParameterValue;
                        sFilterPair[] OtherSystemComponentFilter = new sFilterPair[2];

                        OtherSystemComponentFilter[0].Set("Mon_Sys_Id", cDBConvert.ToString(OtherSystem["Mon_Sys_Id"]));
                        OtherSystemComponentFilter[1].Set("Component_Type_Cd", "SO2,NOX,CO2,O2", eFilterPairStringCompare.InList);

                        DataView OtherSystemComponentFilterView = FindActiveRows(OtherSystemComponentView,
                                                                                 SysEvalBeganDate, SysEvalBeganHour,
                                                                                 SysEvalEndedDate, SysEvalEndedHour,
                                                                                 OtherSystemComponentFilter);

                        AnalyzerRangeFilter[0].Set("Component_Id",
                                                   ColumnToDatalist(OtherSystemComponentFilterView, "Component_Id"),
                                                   eFilterPairStringCompare.InList);
                        AnalyzerRangeFilter[1].Set("Analyzer_Range_Cd", AnalyzerRangeList, eFilterPairStringCompare.InList);

                        DataView AnalyzerRangeFilterView = FindActiveRows(AnalyzerRangeView,
                                                                          SysEvalBeganDate, SysEvalBeganHour,
                                                                          SysEvalEndedDate, SysEvalEndedHour,
                                                                          AnalyzerRangeFilter);

                        if (AnalyzerRangeFilterView.Count > 0)
                          Category.CheckCatalogResult = "A";
                      }
                    }
                  }
                }
              }
            }
          }
          else
            Log = false;
        }
        else
          Log = false;
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "SYSTEM11");
      }

      return ReturnVal;
    }

    public static string SYSTEM12(cCategory Category, ref bool Log)
    // System Type Consistent with Method 
    {
      string ReturnVal = "";

      try
      {
        if (cDBConvert.ToBoolean(Category.GetCheckParameter("System_Type_Code_Valid").ParameterValue) &&
            cDBConvert.ToBoolean(Category.GetCheckParameter("System_Dates_and_Hours_Consistent").ParameterValue))
        {
          DateTime SysEvalBeganDate = cDBConvert.ToDate(Category.GetCheckParameter("System_Evaluation_Begin_Date").ParameterValue, DateTypes.START);
          int SysEvalBeganHour = cDBConvert.ToHour(Category.GetCheckParameter("System_Evaluation_Begin_Hour").ParameterValue, DateTypes.START);
          DateTime SysEvalEndedDate = cDBConvert.ToDate(Category.GetCheckParameter("System_Evaluation_End_Date").ParameterValue, DateTypes.END);
          int SysEvalEndedHour = cDBConvert.ToHour(Category.GetCheckParameter("System_Evaluation_End_Hour").ParameterValue, DateTypes.END);

          DataRowView CurrentSystem = (DataRowView)Category.GetCheckParameter("Current_System").ParameterValue;
          string SystemTypeCd = cDBConvert.ToString(CurrentSystem["Sys_Type_Cd"]);
          DataView MonitorMethodView = (DataView)Category.GetCheckParameter("Method_Records").ParameterValue;

          if( SystemTypeCd.InList( "OILV,OILM,GAS" ) )
          {
            sFilterPair[] MonitorMethodFilter = new sFilterPair[1];

            MonitorMethodFilter[0].Set("Method_Cd", "AD", eFilterPairStringCompare.BeginsWith);

            DataView MonitorMethodFilterView = FindActiveRows(MonitorMethodView,
                                                              SysEvalBeganDate, SysEvalBeganHour,
                                                              SysEvalEndedDate, SysEvalEndedHour,
                                                              MonitorMethodFilter);

            if (MonitorMethodFilterView.Count == 0)
              Category.CheckCatalogResult = "A";
          }
          else if( SystemTypeCd.InList( "LTGS,LTOL" ) )
          {
            sFilterPair[] MonitorMethodFilter = new sFilterPair[1];

            MonitorMethodFilter[0].Set("Method_Cd", "LTF", eFilterPairStringCompare.BeginsWith);

            DataView MonitorMethodFilterView = FindActiveRows(MonitorMethodView,
                                                              SysEvalBeganDate, SysEvalBeganHour,
                                                              SysEvalEndedDate, SysEvalEndedHour,
                                                              MonitorMethodFilter);

            if (MonitorMethodFilterView.Count == 0)
              Category.CheckCatalogResult = "A";
          }
          else
          {
            DataView MethodToSystemTypeView = (DataView)Category.GetCheckParameter("Method_to_System_Type_Cross_Check_Table").ParameterValue;
            sFilterPair[] MethodToSystemTypeFilter = new sFilterPair[1];

            MethodToSystemTypeFilter[0].Field = "SystemTypeCode";
            MethodToSystemTypeFilter[0].Value = cDBConvert.ToString(CurrentSystem["Sys_Type_Cd"]);

            DataView MethodToSystemTypeFilterView = FindRows(MethodToSystemTypeView, MethodToSystemTypeFilter);

            if (MethodToSystemTypeFilterView.Count > 0)
            {
              bool Found = false;

              foreach (DataRowView MethodToSystemTypeRow in MethodToSystemTypeFilterView)
              {
                sFilterPair[] MonitorMethodFilter = new sFilterPair[2];

                MonitorMethodFilter[0].Field = "Method_Cd";
                MonitorMethodFilter[0].Value = cDBConvert.ToString(MethodToSystemTypeRow["MethodCode"]);
                MonitorMethodFilter[1].Field = "Parameter_Cd";
                MonitorMethodFilter[1].Value = cDBConvert.ToString(MethodToSystemTypeRow["MethodParameterCode"]);

                DataView MonitorMethodFilterView = FindActiveRows(MonitorMethodView,
                                                                  SysEvalBeganDate, SysEvalBeganHour,
                                                                  SysEvalEndedDate, SysEvalEndedHour,
                                                                  MonitorMethodFilter);

                if (MonitorMethodFilterView.Count > 0)
                  Found = true;
              }

              if (!Found)
                Category.CheckCatalogResult = "A";
            }
          }
        }
        else
          Log = false;
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "SYSTEM12");
      }

      return ReturnVal;
    }

    public static string SYSTEM13(cCategory Category, ref bool Log)
    // System Type Consistent with Components
    {
      string ReturnVal = "";

      try
      {
        if (cDBConvert.ToBoolean(Category.GetCheckParameter("System_Dates_and_Hours_Consistent").ParameterValue) &&
            cDBConvert.ToBoolean(Category.GetCheckParameter("System_Type_Code_Valid").ParameterValue))
        {
          DateTime SysEvalBeganDate = cDBConvert.ToDate(Category.GetCheckParameter("System_Evaluation_Begin_Date").ParameterValue, DateTypes.START);
          int SysEvalBeganHour = cDBConvert.ToHour(Category.GetCheckParameter("System_Evaluation_Begin_Hour").ParameterValue, DateTypes.START);
          DateTime SysEvalEndedDate = cDBConvert.ToDate(Category.GetCheckParameter("System_Evaluation_End_Date").ParameterValue, DateTypes.END);
          int SysEvalEndedHour = cDBConvert.ToHour(Category.GetCheckParameter("System_Evaluation_End_Hour").ParameterValue, DateTypes.END);

          DataRowView CurrentSystem = (DataRowView)Category.GetCheckParameter("Current_System").ParameterValue;
          string SystemTypeCd = cDBConvert.ToString(CurrentSystem["Sys_Type_Cd"]);

          if( SystemTypeCd.InList( "SO2,SO2R,NOX,NOXC,CO2,O2,HG,HF,HCL,ST" ) )
          {
            bool ResultSet = false;

            if( SystemTypeCd.InList( "SO2R,NOX,CO2" ) )
            {
              DataView SystemComponentView = (DataView)Category.GetCheckParameter("System_System_Component_Records").ParameterValue;
              sFilterPair[] SystemComponentFilter = new sFilterPair[1];

              SystemComponentFilter[0].Set("Component_Type_Cd", "CO2");

              DataView SystemComponentCo2View = FindActiveRows(SystemComponentView,
                                                               SysEvalBeganDate, SysEvalBeganHour,
                                                               SysEvalEndedDate, SysEvalEndedHour,
                                                               SystemComponentFilter);

              foreach (DataRowView SystemComponentCo2Row in SystemComponentCo2View)
              {
                DateTime Co2BeganDate = cDBConvert.ToDate(SystemComponentCo2Row["Begin_Date"], DateTypes.START);
                int Co2BeganHour = cDBConvert.ToHour(SystemComponentCo2Row["Begin_Hour"], DateTypes.START);
                DateTime Co2EndedDate = cDBConvert.ToDate(SystemComponentCo2Row["End_Date"], DateTypes.END);
                int Co2EndedHour = cDBConvert.ToHour(SystemComponentCo2Row["End_Hour"], DateTypes.END);

                DateTime IntBeganDate; int IntBeganHour;
                DateTime IntEndedDate; int IntEndedHour;

                if (GetRangeIntersection(SysEvalBeganDate, SysEvalBeganHour, SysEvalEndedDate, SysEvalEndedHour,
                                         Co2BeganDate, Co2BeganHour, Co2EndedDate, Co2EndedHour,
                                         out IntBeganDate, out IntBeganHour, out IntEndedDate, out IntEndedHour))
                {
                  SystemComponentFilter[0].Set("Component_Type_Cd", "O2");

                  DataView SystemComponentO2View = FindActiveRows(SystemComponentView,
                                                                  IntBeganDate, IntBeganHour,
                                                                  IntEndedDate, IntEndedHour,
                                                                  SystemComponentFilter);

                  if (SystemComponentO2View.Count > 0)
                  {
                    Category.CheckCatalogResult = "A";
                    ResultSet = true;
                  }
                }
              }
            }

            if (!ResultSet)
            {
              DataView CrossCheckView = (DataView)Category.GetCheckParameter("System_Type_To_Component_Type_Cross_Check_Table").ParameterValue;
              sFilterPair[] CrossCheckFilter = new sFilterPair[1];

              CrossCheckFilter[0].Set("SystemTypeCode", SystemTypeCd);

              DataView CrossCheckFilterView = FindRows(CrossCheckView, CrossCheckFilter);

              foreach (DataRowView CrossCheckRow in CrossCheckFilterView)
              {
                DataView SystemComponentView = (DataView)Category.GetCheckParameter("System_System_Component_Records").ParameterValue;
                sFilterPair[] SystemComponentFilter = new sFilterPair[2];

                SystemComponentFilter[0].Set("Component_Type_Cd", cDBConvert.ToString(CrossCheckRow["ComponentTypeCode"]));
                SystemComponentFilter[1].Set("Component_Identifier", "LK", eFilterPairStringCompare.BeginsWith, true);

                DataView SystemComponentFilterView = FindActiveRows(SystemComponentView,
                                                                    SysEvalBeganDate, SysEvalBeganHour,
                                                                    SysEvalEndedDate, SysEvalEndedHour,
                                                                    SystemComponentFilter);

                if (SystemComponentFilterView.Count > 1)
                {
                  System13_AnalyzerRangeCheck(Category, SystemComponentFilterView,
                                              SysEvalBeganDate, SysEvalBeganHour,
                                              SysEvalEndedDate, SysEvalEndedHour);
                }
              }
            }
          }
          else if (SystemTypeCd == "H2O")
          {
            bool ResultSet = false;

            DataView SystemComponentView = (DataView)Category.GetCheckParameter("System_System_Component_Records").ParameterValue;
            DataView[] SystemComponentViewList = new DataView[3];
            const int PosB = 0; const int PosD = 2; const int PosW = 1;
            sFilterPair[] SystemComponentFilter;

            SystemComponentFilter = new sFilterPair[2];
            SystemComponentFilter[0].Set("Component_Type_Cd", "O2");
            SystemComponentFilter[1].Set("Component_Identifier", "LK", eFilterPairStringCompare.BeginsWith, true);

            DataView SystemComponentO2View = FindActiveRows(SystemComponentView,
                                                            SysEvalBeganDate, SysEvalBeganHour,
                                                            SysEvalEndedDate, SysEvalEndedHour,
                                                            SystemComponentFilter);

            SystemComponentFilter = new sFilterPair[1];
            SystemComponentFilter[0].Set("Basis_Cd", "B");
            SystemComponentViewList[PosB] = FindActiveRows(SystemComponentO2View,
                                                           SysEvalBeganDate, SysEvalBeganHour,
                                                           SysEvalEndedDate, SysEvalEndedHour,
                                                           SystemComponentFilter);
            SystemComponentFilter[0].Set("Basis_Cd", "D");
            SystemComponentViewList[PosD] = FindActiveRows(SystemComponentO2View,
                                                           SysEvalBeganDate, SysEvalBeganHour,
                                                           SysEvalEndedDate, SysEvalEndedHour,
                                                           SystemComponentFilter);
            SystemComponentFilter[0].Set("Basis_Cd", "W");
            SystemComponentViewList[PosW] = FindActiveRows(SystemComponentO2View,
                                                           SysEvalBeganDate, SysEvalBeganHour,
                                                           SysEvalEndedDate, SysEvalEndedHour,
                                                           SystemComponentFilter);

            if ((SystemComponentViewList[PosB].Count >= 1) &&
                ((SystemComponentViewList[PosW].Count >= 1) || (SystemComponentViewList[PosD].Count >= 1)))
            {
              if ((SystemComponentO2View.Count > 1) &&
                  IsRangeIntersection(SystemComponentO2View, System13_BasisKey))
              {
                Category.CheckCatalogResult = "C";
                ResultSet = true;
              }
            }

            if (!ResultSet)
            {
              foreach (DataView SystemComponentBasisView in SystemComponentViewList)
              {
                if (SystemComponentBasisView.Count > 1)
                {
                  System13_AnalyzerRangeCheck(Category, SystemComponentBasisView,
                                              SysEvalBeganDate, SysEvalBeganHour,
                                              SysEvalEndedDate, SysEvalEndedHour);
                }
              }
            }
          }
        }
        else
          Log = false;
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "SYSTEM13");
      }

      return ReturnVal;
    }

    private static string System13_BasisKey(DataRowView ARow)
    {
      if ((cDBConvert.ToString(ARow["Basis_Cd"]) == "W") ||
          (cDBConvert.ToString(ARow["Basis_Cd"]) == "D"))
        return "WetOrDry";
      else
        return "WetAndDry";
    }

    private static void System13_AnalyzerRangeCheck(cCategory ACategory, DataView ASystemComponentView,
                                                    DateTime ASysEvalBeganDate, int ASysEvalBeganHour,
                                                    DateTime ASysEvalEndedDate, int ASysEvalEndedHour)
    {
      string ComponentIdentifierList = ColumnToDatalist(ASystemComponentView, "Component_Identifier");
      DataView AnalyzerRangeView = (DataView)ACategory.GetCheckParameter("System_Analyzer_Range_Records").ParameterValue;
      sFilterPair[] AnalyzerRangeFilter = new sFilterPair[2];

      AnalyzerRangeFilter[0].Set("Component_Identifier", ComponentIdentifierList, eFilterPairStringCompare.InList);
      AnalyzerRangeFilter[1].Set("Analyzer_Range_Cd", "H,A", eFilterPairStringCompare.InList);

      DataView AnalyzerRangeFilterView = FindActiveRows(AnalyzerRangeView,
                                                        ASysEvalBeganDate, ASysEvalBeganHour,
                                                        ASysEvalEndedDate, ASysEvalEndedHour,
                                                        AnalyzerRangeFilter);

      if ((AnalyzerRangeFilterView.Count > 1) &&
          IsRangeIntersection(AnalyzerRangeFilterView, "Component_Id"))
        ACategory.CheckCatalogResult = "B";
      else
      {
        AnalyzerRangeFilter[0].Set("Component_Identifier", ComponentIdentifierList, eFilterPairStringCompare.InList);
        AnalyzerRangeFilter[1].Set("Analyzer_Range_Cd", "L,A", eFilterPairStringCompare.InList);

        AnalyzerRangeFilterView = FindActiveRows(AnalyzerRangeView,
                                                 ASysEvalBeganDate, ASysEvalBeganHour,
                                                 ASysEvalEndedDate, ASysEvalEndedHour,
                                                 AnalyzerRangeFilter);

        if ((AnalyzerRangeFilterView.Count > 1) &&
            IsRangeIntersection(AnalyzerRangeFilterView, "Component_Id"))
          ACategory.CheckCatalogResult = "B";
      }
    }

    public static string SYSTEM14(cCategory Category, ref bool Log)
    // System Fuel Consistent with Unit Fuel  
    {
      string ReturnVal = "";

      try
      {
        if (cDBConvert.ToBoolean(Category.GetCheckParameter("System_Type_Code_Valid").ParameterValue) &&
            cDBConvert.ToBoolean(Category.GetCheckParameter("System_Fuel_Code_Valid").ParameterValue) &&
            cDBConvert.ToBoolean(Category.GetCheckParameter("System_Dates_and_Hours_Consistent").ParameterValue))
        {
          DataRowView CurrentSystem = (DataRowView)Category.GetCheckParameter("Current_System").ParameterValue;
          string FuelCd = cDBConvert.ToString(CurrentSystem["Fuel_Cd"]);

          if ((FuelCd != "NFS") && (FuelCd != "MIX"))
          {
            DateTime SysEvalBeganDate = cDBConvert.ToDate(Category.GetCheckParameter("System_Evaluation_Begin_Date").ParameterValue, DateTypes.START);
            int SysEvalBeganHour = cDBConvert.ToHour(Category.GetCheckParameter("System_Evaluation_Begin_Hour").ParameterValue, DateTypes.START);
            DateTime SysEvalEndedDate = cDBConvert.ToDate(Category.GetCheckParameter("System_Evaluation_End_Date").ParameterValue, DateTypes.END);
            int SysEvalEndedHour = cDBConvert.ToHour(Category.GetCheckParameter("System_Evaluation_End_Hour").ParameterValue, DateTypes.END);

            string SystemUnitFuel = Category.GetCheckParameter("System_Unit_Fuel").ValueAsString();
            DataView UnitFuelRecords = (DataView)Category.GetCheckParameter("Location_Fuel_Records").ParameterValue;
            cFilterCondition[] RowFilter = new cFilterCondition[] { new cFilterCondition("Fuel_Cd", SystemUnitFuel) };

            DataView UnitFuelView = cRowFilter.FindActiveRows(UnitFuelRecords, 
                                                              SysEvalBeganDate, SysEvalEndedDate,
                                                              RowFilter);
            DataRowView UnitFuelRow = UnitFuelView.Latest("BEGIN_DATE");

            if (UnitFuelRow == null)
              Category.CheckCatalogResult = "A";
            else if (UnitFuelRow["INDICATOR_CD"].AsString().InList("I,E"))
            {
              Category.CheckCatalogResult = "C";
            }
            else
            {
              DateTime? UnitFuelEndDate = UnitFuelRow["END_DATE"].AsDateTime();
              DateTime? SystemEndDate = CurrentSystem["END_DATE"].AsDateTime();

              if (UnitFuelEndDate.HasValue
                  && (!SystemEndDate.HasValue || (SystemEndDate > UnitFuelEndDate)))
                Category.CheckCatalogResult = "B";
            }
          }
        }
        else
          Log = false;
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "SYSTEM14");
      }

      return ReturnVal;
    }

    public static string SYSTEM15(cCategory Category, ref bool Log)
    // DB System Consistent with P or RB System 
    {
      string ReturnVal = "";

      try
      {
        if (cDBConvert.ToBoolean(Category.GetCheckParameter("System_Designation_Code_Valid").ParameterValue) &&
            cDBConvert.ToBoolean(Category.GetCheckParameter("Required_DAHS_Reported_for_System").ParameterValue) &&
            cDBConvert.ToBoolean(Category.GetCheckParameter("Required_Non_DAHS_Components_Reported_For_System").ParameterValue))
        {
          DataRowView CurrentSystem = (DataRowView)Category.GetCheckParameter("Current_System").ParameterValue;
          string MonSysId = cDBConvert.ToString(CurrentSystem["Mon_Sys_Id"]);
          string SystemTypeCd = cDBConvert.ToString(CurrentSystem["Sys_Type_Cd"]);
          string SystemDesignationCd = cDBConvert.ToString(CurrentSystem["Sys_Designation_Cd"]);

          if (SystemDesignationCd == "DB")
          {
            DateTime SysEvalBeganDate = cDBConvert.ToDate(Category.GetCheckParameter("System_Evaluation_Begin_Date").ParameterValue, DateTypes.START);
            int SysEvalBeganHour = cDBConvert.ToHour(Category.GetCheckParameter("System_Evaluation_Begin_Hour").ParameterValue, DateTypes.START);
            DateTime SysEvalEndedDate = cDBConvert.ToDate(Category.GetCheckParameter("System_Evaluation_End_Date").ParameterValue, DateTypes.END);
            int SysEvalEndedHour = cDBConvert.ToHour(Category.GetCheckParameter("System_Evaluation_End_Hour").ParameterValue, DateTypes.END);

            DataView SystemView = (DataView)Category.GetCheckParameter("Monitor_System_Records").ParameterValue;
            sFilterPair[] SystemFilter = new sFilterPair[3];

            SystemFilter[0].Set("Sys_Type_Cd", SystemTypeCd);
            SystemFilter[1].Set("Sys_Designation_Cd", "P,RB", eFilterPairStringCompare.InList);
            SystemFilter[2].Set("Mon_Sys_Id", MonSysId, true);

            DataView OtherSystemView = FindActiveRows(SystemView,
                                                      SysEvalBeganDate, SysEvalBeganHour,
                                                      SysEvalEndedDate, SysEvalEndedHour,
                                                      SystemFilter);

            if (OtherSystemView.Count == 0)
              Category.CheckCatalogResult = "A";
            else
            {
              bool VerbotenComponentFailure = false;
              bool RequiredComponentFailure = true;

              foreach (DataRowView OtherSystem in OtherSystemView)
              {
                DataView CurrentSystemComponentView = (DataView)Category.GetCheckParameter("System_System_Component_Records").ParameterValue;
                DataView SystemComponentView = (DataView)Category.GetCheckParameter("Location_System_Component_Records").ParameterValue;
                sFilterPair[] SystemComponentFilter = new sFilterPair[1];

                SystemComponentFilter[0].Set("Mon_Sys_Id", cDBConvert.ToString(OtherSystem["Mon_Sys_Id"]));

                DataView OtherSystemComponentView = FindActiveRows(SystemComponentView,
                                                                   SysEvalBeganDate, SysEvalBeganHour,
                                                                   SysEvalEndedDate, SysEvalEndedHour,
                                                                   SystemComponentFilter);

                bool AllNonDahsMatch = true;

                foreach (DataRowView OtherSystemComponent in OtherSystemComponentView)
                {
                  string OtherComponentId = cDBConvert.ToString(OtherSystemComponent["Component_Id"]);
                  string ComponentTypeCd = cDBConvert.ToString(OtherSystemComponent["Component_Type_Cd"]);

                  SystemComponentFilter[0].Set("Component_Id", OtherComponentId);

                  DataView FilterSystemComponentView = FindActiveRows(CurrentSystemComponentView,
                                                                      SysEvalBeganDate, SysEvalBeganHour,
                                                                      SysEvalEndedDate, SysEvalEndedHour,
                                                                      SystemComponentFilter);

                  if( ComponentTypeCd.InList( "DAHS,PLC,FLC" ) )
                  {
                    if (FilterSystemComponentView.Count > 0)
                      VerbotenComponentFailure = true;
                  }
                  else
                  {
                    if (FilterSystemComponentView.Count == 0)
                      AllNonDahsMatch = false;
                  }
                }

                if (AllNonDahsMatch)
                  RequiredComponentFailure = false;
              }

              if (VerbotenComponentFailure)
                Category.CheckCatalogResult = "B";
              else if (RequiredComponentFailure)
                Category.CheckCatalogResult = "C";
            }
          }
          else
            Log = false;
        }
        else
          Log = false;
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "SYSTEM15");
      }

      return ReturnVal;
    }

    public static string SYSTEM16(cCategory Category, ref bool Log)
    // RM System Consistent with Non-RM Systems  
    {
      string ReturnVal = "";

      try
      {
        if (cDBConvert.ToBoolean(Category.GetCheckParameter("System_Designation_Code_Valid").ParameterValue) &&
            cDBConvert.ToBoolean(Category.GetCheckParameter("Required_DAHS_Reported_for_System").ParameterValue) &&
            cDBConvert.ToBoolean(Category.GetCheckParameter("Required_Non_DAHS_Components_Reported_For_System").ParameterValue))
        {
          DataRowView CurrentSystem = (DataRowView)Category.GetCheckParameter("Current_System").ParameterValue;
          string MonSysId = cDBConvert.ToString(CurrentSystem["Mon_Sys_Id"]);
          string SystemTypeCd = cDBConvert.ToString(CurrentSystem["Sys_Type_Cd"]);
          string SystemDesignationCd = cDBConvert.ToString(CurrentSystem["Sys_Designation_Cd"]);

          if (SystemDesignationCd == "RM")
          {
            DateTime SysEvalBeganDate = cDBConvert.ToDate(Category.GetCheckParameter("System_Evaluation_Begin_Date").ParameterValue, DateTypes.START);
            int SysEvalBeganHour = cDBConvert.ToHour(Category.GetCheckParameter("System_Evaluation_Begin_Hour").ParameterValue, DateTypes.START);
            DateTime SysEvalEndedDate = cDBConvert.ToDate(Category.GetCheckParameter("System_Evaluation_End_Date").ParameterValue, DateTypes.END);
            int SysEvalEndedHour = cDBConvert.ToHour(Category.GetCheckParameter("System_Evaluation_End_Hour").ParameterValue, DateTypes.END);

            DataView SystemView = (DataView)Category.GetCheckParameter("Monitor_System_Records").ParameterValue;
            sFilterPair[] SystemFilter = new sFilterPair[3];

            SystemFilter[0].Set("Sys_Type_Cd", SystemTypeCd);
            SystemFilter[1].Set("Sys_Designation_Cd", "RM", true);
            SystemFilter[2].Set("Mon_Sys_Id", MonSysId, true);

            DataView OtherSystemView = FindActiveRows(SystemView,
                                                      SysEvalBeganDate, SysEvalBeganHour,
                                                      SysEvalEndedDate, SysEvalEndedHour,
                                                      SystemFilter);

            if (OtherSystemView.Count == 0)
              Category.CheckCatalogResult = "A";
            else
            {
              DataView CurrentSystemComponentView = (DataView)Category.GetCheckParameter("System_System_Component_Records").ParameterValue;
              string OtherSystemList = ColumnToDatalist(OtherSystemView, "Mon_Sys_Id");
              DataView SystemComponentView = (DataView)Category.GetCheckParameter("Location_System_Component_Records").ParameterValue;
              sFilterPair[] SystemComponentFilter = new sFilterPair[2];

              SystemComponentFilter[0].Set("Mon_Sys_Id", OtherSystemList, eFilterPairStringCompare.InList);
              SystemComponentFilter[1].Set("Component_Type_Cd", "DAHS,PLC,FLC,PRB", eFilterPairStringCompare.InList, true);

              DataView OtherSystemComponentView = FindActiveRows(SystemComponentView,
                                                                 SysEvalBeganDate, SysEvalBeganHour,
                                                                 SysEvalEndedDate, SysEvalEndedHour,
                                                                 SystemComponentFilter);

              foreach (DataRowView OtherSystemComponent in OtherSystemComponentView)
              {
                SystemComponentFilter[0].Set("Component_Id", cDBConvert.ToString(OtherSystemComponent["Component_Id"]));

                DataView FilterSystemComponentView = FindActiveRows(CurrentSystemComponentView,
                                                                    SysEvalBeganDate, SysEvalBeganHour,
                                                                    SysEvalEndedDate, SysEvalEndedHour,
                                                                    SystemComponentFilter);

                if (FilterSystemComponentView.Count > 0)
                  Category.CheckCatalogResult = "B";
              }
            }
          }
          else
            Log = false;
        }
        else
          Log = false;
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "SYSTEM16");
      }

      return ReturnVal;
    }

    public static string SYSTEM17(cCategory Category, ref bool Log)
    // Backup System Consistent with Primary System
    {
      string ReturnVal = "";

      try
      {
        if (cDBConvert.ToBoolean(Category.GetCheckParameter("System_Type_Code_Valid").ParameterValue) &&
            cDBConvert.ToBoolean(Category.GetCheckParameter("System_Dates_and_Hours_Consistent").ParameterValue))
        {
          DataRowView CurrentSystem = (DataRowView)Category.GetCheckParameter("Current_System").ParameterValue;

          if( cDBConvert.ToString( CurrentSystem["Sys_Designation_Cd"] ).InList( "B,RB" ) )
          {
            string BackupSysId = cDBConvert.ToString(CurrentSystem["Mon_Sys_Id"]);
            string SystemTypeCd = cDBConvert.ToString(CurrentSystem["Sys_Type_Cd"]);

            DateTime SysEvalBeginDate = cDBConvert.ToDate(Category.GetCheckParameter("System_Evaluation_Begin_Date").ParameterValue, DateTypes.START);
            int SysEvalBeginHour = cDBConvert.ToHour(Category.GetCheckParameter("System_Evaluation_Begin_Hour").ParameterValue, DateTypes.START);
            DateTime SysEvalEndDate = cDBConvert.ToDate(Category.GetCheckParameter("System_Evaluation_End_Date").ParameterValue, DateTypes.END);
            int SysEvalEndHour = cDBConvert.ToHour(Category.GetCheckParameter("System_Evaluation_End_Hour").ParameterValue, DateTypes.END);

            DataView SystemComponentView = (DataView)Category.GetCheckParameter("Location_System_Component_Records").ParameterValue;
            sFilterPair[] SystemComponentFilter = new sFilterPair[4];

            SystemComponentFilter[0].Set("Mon_Sys_Id", BackupSysId);
            SystemComponentFilter[1].Set("Component_Type_Cd", "DAHS", true);
            SystemComponentFilter[2].Set("Component_Type_Cd", "PLC", true);
            SystemComponentFilter[3].Set("Component_Type_Cd", "FLC", true);

            DataView BackupSystemComponentView = FindActiveRows(SystemComponentView,
                                                                SysEvalBeginDate, SysEvalBeginHour,
                                                                SysEvalEndDate, SysEvalEndHour,
                                                                SystemComponentFilter);
            BackupSystemComponentView.Sort = "Component_ID";

            DataView SystemView = (DataView)Category.GetCheckParameter("Monitor_System_Records").ParameterValue;
            sFilterPair[] SystemFilter = new sFilterPair[2];

            SystemFilter[0].Set("Sys_Type_Cd", SystemTypeCd);
            SystemFilter[1].Set("Sys_Designation_Cd", "P");

            DataView PrimarySystemView = FindActiveRows(SystemView,
                                                        SysEvalBeginDate, SysEvalBeginHour,
                                                        SysEvalEndDate, SysEvalEndHour,
                                                        SystemFilter);

            if (PrimarySystemView.Count > 0)
            {
              bool Problem = false;

              foreach (DataRowView PrimarySystemRow in PrimarySystemView)
              {
                SystemComponentFilter[0].Set("Mon_Sys_Id", cDBConvert.ToString(PrimarySystemRow["Mon_Sys_Id"]));
                SystemComponentFilter[1].Set("Component_Type_Cd", "DAHS", true);
                SystemComponentFilter[2].Set("Component_Type_Cd", "PLC", true);
                SystemComponentFilter[3].Set("Component_Type_Cd", "FLC", true);

                DataView PrimarySystemComponentView = FindActiveRows(SystemComponentView,
                                                                     SysEvalBeginDate, SysEvalBeginHour,
                                                                     SysEvalEndDate, SysEvalEndHour,
                                                                     SystemComponentFilter);
                PrimarySystemComponentView.Sort = "Component_ID";

                //According to Michael A. Ackerman list of differing lengths are differnt 
                //even if one is a subset of the other

                if (PrimarySystemComponentView.Count == BackupSystemComponentView.Count)
                {
                  bool Same = true;

                  for (int Row = 0; Row < PrimarySystemComponentView.Count; Row++)
                  {
                    string PrimaryComponentId = cDBConvert.ToString(PrimarySystemComponentView[Row]["Component_Id"]);
                    string BackupComponentId = cDBConvert.ToString(BackupSystemComponentView[Row]["Component_Id"]);

                    if (PrimaryComponentId != BackupComponentId)
                      Same = false;
                  }

                  if (Same) Problem = true;
                }
              }

              if (Problem)
                Category.CheckCatalogResult = "A";
            }
          }
        }
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "SYSTEM17");
      }

      return ReturnVal;
    }

    public static string SYSTEM18(cCategory Category, ref bool Log)
    // Overlapping Primary NFS Systems 
    {
      string ReturnVal = "";

      try
      {
        if (cDBConvert.ToBoolean(Category.GetCheckParameter("System_Dates_and_Hours_Consistent").ParameterValue))
        {
          Category.SetCheckParameter("Required_DAHS_Reported_for_System", true, eParameterDataType.Boolean);

          DataView SysMonSysComponentView = (DataView)Category.GetCheckParameter("System_System_Component_Records").ParameterValue;
          sFilterPair[] SysMonSysComponentFilter = new sFilterPair[1];

          SysMonSysComponentFilter[0].Field = "Component_Type_Cd";
          SysMonSysComponentFilter[0].Value = "DAHS";

          DataView DahsMonSysComponentView = FindRows(SysMonSysComponentView, SysMonSysComponentFilter);

          if (DahsMonSysComponentView.Count == 0)
          {
            Category.SetCheckParameter("Required_DAHS_Reported_for_System", false, eParameterDataType.Boolean);
            Category.CheckCatalogResult = "A";
          }
          else
          {
            DateTime SysEvalBeginDate = cDBConvert.ToDate(Category.GetCheckParameter("System_Evaluation_Begin_Date").ParameterValue, DateTypes.START);
            int SysEvalBeginHour = cDBConvert.ToInteger(Category.GetCheckParameter("System_Evaluation_Begin_Hour").ParameterValue);
            DateTime SysEvalEndDate = cDBConvert.ToDate(Category.GetCheckParameter("System_Evaluation_End_Date").ParameterValue, DateTypes.END);
            int SysEvalEndHour = cDBConvert.ToInteger(Category.GetCheckParameter("System_Evaluation_End_Hour").ParameterValue);

            if (!CheckForHourRangeCovered(Category, DahsMonSysComponentView,
                                          SysEvalBeginDate, SysEvalBeginHour,
                                          SysEvalEndDate, SysEvalEndHour))
            {
              Category.CheckCatalogResult = "B";
            }
          }
        }
        else
          Log = false;
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "SYSTEM18");
      }

      return ReturnVal;
    }

    public static string SYSTEM19(cCategory Category, ref bool Log)
    // Required Formula Reported for System
    {
      string ReturnVal = "";

      try
      {
        if (cDBConvert.ToBoolean(Category.GetCheckParameter("System_Type_Code_Valid").ParameterValue) &&
            cDBConvert.ToBoolean(Category.GetCheckParameter("System_Dates_and_Hours_Consistent").ParameterValue))
        {
          Category.SetCheckParameter("Required_Non_DAHS_Components_Reported_For_System", true, eParameterDataType.Boolean);
          Category.SetCheckParameter("Required_Probe", false, eParameterDataType.Boolean);

          DateTime SysEvalBeginDate = cDBConvert.ToDate(Category.GetCheckParameter("System_Evaluation_Begin_Date").ParameterValue, DateTypes.START);
          int SysEvalBeginHour = cDBConvert.ToHour(Category.GetCheckParameter("System_Evaluation_Begin_Hour").ParameterValue, DateTypes.START);
          DateTime SysEvalEndDate = cDBConvert.ToDate(Category.GetCheckParameter("System_Evaluation_End_Date").ParameterValue, DateTypes.END);
          int SysEvalEndHour = cDBConvert.ToHour(Category.GetCheckParameter("System_Evaluation_End_Hour").ParameterValue, DateTypes.END);

          DataRowView CurrentSystem = (DataRowView)Category.GetCheckParameter("Current_System").ParameterValue;
          string SystemTypeCd = cDBConvert.ToString(CurrentSystem["Sys_Type_Cd"]);

          if (SystemTypeCd != "H2O" && SystemTypeCd != "ST")
          {
            string MissingComponentsList = ""; string MissingComponentsDelim = "";
            string IncompleteComponentsList = ""; string IncompleteComponentsDelim = "";

            DataView CrossCheckView = (DataView)Category.GetCheckParameter("System_Type_To_Component_Type_Cross_Check_Table").ParameterValue;
            DataRowView CrossCheckManRow;
            sFilterPair[] CrossCheckFilter = new sFilterPair[2];

            //non H2O Mandatory
            CrossCheckFilter[0].Set("SystemTypeCode", SystemTypeCd);
            CrossCheckFilter[1].Set("Mandatory", "Yes");

            if (FindRow(CrossCheckView, CrossCheckFilter, out CrossCheckManRow))
            {
              string ComponentTypeCd = cDBConvert.ToString(CrossCheckManRow["ComponentTypeCode"]);
              DataView SystemComponentView = (DataView)Category.GetCheckParameter("System_System_Component_Records").ParameterValue;
              sFilterPair[] SystemComponentFilter = new sFilterPair[2];

              SystemComponentFilter[0].Set("Component_Type_Cd", ComponentTypeCd);
              SystemComponentFilter[1].Set("Component_Identifier", "LK", eFilterPairStringCompare.BeginsWith, true);

              DataView SystemComponentFilterView = FindActiveRows(SystemComponentView,
                                                                  SysEvalBeginDate, SysEvalBeginHour,
                                                                  SysEvalEndDate, SysEvalEndHour,
                                                                  SystemComponentFilter);

              if (SystemComponentFilterView.Count == 0)
              {
                MissingComponentsList += MissingComponentsDelim + ComponentTypeCd;
                MissingComponentsDelim = ", and ";
              }
              else
              {
                if (!CheckForHourRangeCovered(Category, SystemComponentFilterView,
                                              SysEvalBeginDate, SysEvalBeginHour,
                                              SysEvalEndDate, SysEvalEndHour))
                {
                  IncompleteComponentsList += IncompleteComponentsDelim + ComponentTypeCd;
                  IncompleteComponentsDelim = ", and ";
                }

                if( ComponentTypeCd.InList( "SO2,NOX,CO2,O2,H2O,HCL,HF,HG" ) )
                {
                  bool RequiredProbe = false;

                  foreach (DataRowView SystemComponentRow in SystemComponentFilterView)
                  {
                      if( cDBConvert.ToString( SystemComponentRow["Acq_Cd"] ).InList( "DIL,DOU,DIN,EXT,WXT" ) )
                      RequiredProbe = true;
                  }

                  if (RequiredProbe)
                    Category.SetCheckParameter("Required_Probe", true, eParameterDataType.Boolean);
                }
              }
            }

            //non H2O non-Mandatory
            CrossCheckFilter[0].Set("SystemTypeCode", SystemTypeCd);
            CrossCheckFilter[1].Set("Mandatory", "Yes", true);

            DataView CrossCheckFilterView = FindRows(CrossCheckView, CrossCheckFilter);

            if (CrossCheckFilterView.Count > 0)
            {
              string ComponentTypeList = "";
              string ComponentTypeDelim = "";

              foreach (DataRowView CrossCheckNonRow in CrossCheckFilterView)
              {
                ComponentTypeList += ComponentTypeDelim + cDBConvert.ToString(CrossCheckNonRow["ComponentTypeCode"]);
                ComponentTypeDelim = ",";
              }

              DataView SystemComponentView = (DataView)Category.GetCheckParameter("System_System_Component_Records").ParameterValue;
              sFilterPair[] SystemComponentFilter = new sFilterPair[2];

              SystemComponentFilter[0].Set("Component_Type_Cd", ComponentTypeList, eFilterPairStringCompare.InList);
              SystemComponentFilter[1].Set("Component_Identifier", "LK", eFilterPairStringCompare.BeginsWith, true);

              DataView SystemComponentFilterView = FindActiveRows(SystemComponentView,
                                                                  SysEvalBeginDate, SysEvalBeginHour,
                                                                  SysEvalEndDate, SysEvalEndHour,
                                                                  SystemComponentFilter);

              if (SystemComponentFilterView.Count == 0)
              {
                MissingComponentsList += MissingComponentsDelim + ComponentTypeList.FormatList(true);
                MissingComponentsDelim = ",";
              }
              else
              {
                if (!CheckForHourRangeCovered(Category, SystemComponentFilterView,
                                              SysEvalBeginDate, SysEvalBeginHour,
                                              SysEvalEndDate, SysEvalEndHour))
                {
                  IncompleteComponentsList += IncompleteComponentsDelim + ComponentTypeList.FormatList(true);
                  IncompleteComponentsDelim = ",";
                }

                bool RequiredProbe = false;

                foreach (DataRowView SystemComponentRow in SystemComponentFilterView)
                {
                    if( cDBConvert.ToString( SystemComponentRow["Component_Type_Cd"] ).InList( "SO2,NOX,CO2,O2,H2O,HCL,HF,HG" ) &&
                        cDBConvert.ToString( SystemComponentRow["Acq_Cd"] ).InList( "DIL,DOU,DIN,EXT,WXT" ) )
                    RequiredProbe = true;
                }

                if (RequiredProbe)
                  Category.SetCheckParameter("Required_Probe", true, eParameterDataType.Boolean);
              }
            }

            Category.SetCheckParameter("Missing_Components_for_System", MissingComponentsList, eParameterDataType.String);
            Category.SetCheckParameter("Incomplete_Components_for_System", IncompleteComponentsList, eParameterDataType.String);

            if ((MissingComponentsList != "") && (IncompleteComponentsList == ""))
            {
              Category.SetCheckParameter("Required_Non_DAHS_Components_Reported_For_System", false, eParameterDataType.Boolean);
              Category.CheckCatalogResult = "A";
            }
            else if ((IncompleteComponentsList != "") && (MissingComponentsList == ""))
            {
              Category.CheckCatalogResult = "B";
            }
            else if ((MissingComponentsList != "") && (IncompleteComponentsList != ""))
            {
              Category.SetCheckParameter("Required_Non_DAHS_Components_Reported_For_System", false, eParameterDataType.Boolean);
              Category.CheckCatalogResult = "C";
            }
          }
          else if (SystemTypeCd == "H2O")
          {
            DataView SystemComponentView = (DataView)Category.GetCheckParameter("System_System_Component_Records").ParameterValue;
            sFilterPair[] SystemComponentFilter = new sFilterPair[3];

            SystemComponentFilter[0].Set("Component_Type_Cd", "O2");
            SystemComponentFilter[1].Set("Component_Identifier", "LK", eFilterPairStringCompare.BeginsWith, true);
            SystemComponentFilter[2].Set("Basis_Cd", "B,W", eFilterPairStringCompare.InList);

            DataView SystemComponentFilterViewBW = FindActiveRows(SystemComponentView,
                                                                  SysEvalBeginDate, SysEvalBeginHour,
                                                                  SysEvalEndDate, SysEvalEndHour,
                                                                  SystemComponentFilter);

            if (SystemComponentFilterViewBW.Count == 0)
              Category.CheckCatalogResult = "D";
            else
            {
              if (!CheckForHourRangeCovered(Category, SystemComponentFilterViewBW,
                                            SysEvalBeginDate, SysEvalBeginHour,
                                            SysEvalEndDate, SysEvalEndHour))
                Category.CheckCatalogResult = "E";

              bool RequiredProbe = false;

              foreach (DataRowView SystemComponentRowBW in SystemComponentFilterViewBW)
              {
                  if( cDBConvert.ToString( SystemComponentRowBW["Acq_Cd"] ).InList( "DIL,DOU,DIN,EXT,WXT" ) )
                  RequiredProbe = true;


                if (cDBConvert.ToString(SystemComponentRowBW["Basis_Cd"]) == "W")
                {
                  SystemComponentFilter[0].Set("Component_Type_Cd", "O2");
                  SystemComponentFilter[1].Set("Component_Identifier", "LK", eFilterPairStringCompare.BeginsWith, true);
                  SystemComponentFilter[2].Set("Basis_Cd", "D");

                  DataView SystemComponentFilterViewD = FindActiveRows(SystemComponentView,
                                                                       SysEvalBeginDate, SysEvalBeginHour,
                                                                       SysEvalEndDate, SysEvalEndHour,
                                                                       SystemComponentFilter);

                  if (SystemComponentFilterViewD.Count == 0)
                    Category.CheckCatalogResult = "D";
                  else
                  {
                    foreach (DataRowView SystemComponentRowD in SystemComponentFilterViewD)
                    {
                      DateTime WetBeganDate = cDBConvert.ToDate(SystemComponentRowBW["Begin_Date"], DateTypes.START);
                      int WetBeganHour = cDBConvert.ToHour(SystemComponentRowBW["Begin_Hour"], DateTypes.START);
                      DateTime WetEndedDate = cDBConvert.ToDate(SystemComponentRowBW["End_Date"], DateTypes.END);
                      int WetEndedHour = cDBConvert.ToHour(SystemComponentRowBW["End_Hour"], DateTypes.END);
                      DateTime TestBeganDate; int TestBeganHour;
                      DateTime TestEndedDate; int TestEndedHour;

                      if ((WetBeganDate > SysEvalBeginDate) ||
                          ((WetBeganDate == SysEvalBeginDate) && (WetBeganHour >= SysEvalBeginHour)))
                      { TestBeganDate = WetBeganDate; TestBeganHour = WetBeganHour; }
                      else
                      { TestBeganDate = SysEvalBeginDate; TestBeganHour = SysEvalBeginHour; }

                      if ((WetEndedDate < SysEvalEndDate) ||
                          ((WetEndedDate == SysEvalEndDate) && (WetEndedHour <= SysEvalEndHour)))
                      { TestEndedDate = WetEndedDate; TestEndedHour = WetEndedHour; }
                      else
                      { TestEndedDate = SysEvalEndDate; TestEndedHour = SysEvalEndHour; }

                      if (!CheckForHourRangeCovered(Category, SystemComponentFilterViewD,
                                                    TestBeganDate, TestBeganHour,
                                                    TestEndedDate, TestEndedHour))
                        Category.CheckCatalogResult = "E";
                    }
                  }
                }
              }
              if (RequiredProbe)
                Category.SetCheckParameter("Required_Probe", true, eParameterDataType.Boolean);
            }
          }
          else if (SystemTypeCd == "ST")
          {
            DataView SystemComponentView = (DataView)Category.GetCheckParameter("System_System_Component_Records").ParameterValue;
            sFilterPair[] SystemComponentFilter = new sFilterPair[2];

            SystemComponentFilter[0].Set("Component_Type_Cd", "STRAIN");
            SystemComponentFilter[1].Set("Component_Identifier", "LK", eFilterPairStringCompare.BeginsWith, true);

            DataView SystemComponentFilterViewBW = FindActiveRows(SystemComponentView,
                                                                  SysEvalBeginDate, SysEvalBeginHour,
                                                                  SysEvalEndDate, SysEvalEndHour,
                                                                  SystemComponentFilter);
            if (SystemComponentFilterViewBW.Count < 2)
            {
              Category.SetCheckParameter("Required_Non_DAHS_Components_Reported_For_System", false, eParameterDataType.Boolean);
              Category.SetCheckParameter("Missing_Components_for_System", "STRAIN", eParameterDataType.String);
            }
            else
            {
              foreach (DataRowView drSysCompon in SystemComponentFilterViewBW)
              {
                string sCompID = cDBConvert.ToString(drSysCompon["component_identifier"]);

                sFilterPair[] SystemComponentFilter2 = new sFilterPair[2];

                SystemComponentFilter2[0].Set("Component_Type_Cd", "STRAIN");
                SystemComponentFilter2[1].Set("Component_Identifier", sCompID, true);

                DataView SystemComponentFilterViewBW2 = FindActiveRows(SystemComponentView,
                                                                      SysEvalBeginDate, SysEvalBeginHour,
                                                                      SysEvalEndDate, SysEvalEndHour,
                                                                      SystemComponentFilter2);

                if (!CheckForHourRangeCovered(Category, SystemComponentFilterViewBW2, SysEvalBeginDate, SysEvalBeginHour, SysEvalEndDate, SysEvalEndHour))
                {
                  Category.SetCheckParameter("Missing_Components_for_System", "STRAIN", eParameterDataType.String);
                  break;
                }
              }
            }

            string MissingComp4Sys = Category.GetCheckParameter("Missing_Components_for_System").ValueAsString();

            if (MissingComp4Sys != "")
              Category.CheckCatalogResult = "F";
            Category.SetCheckParameter( "Missing_Components_for_System", MissingComp4Sys.FormatList(), eParameterDataType.String );
          }
        }
        else
          Log = false;
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "SYSTEM19");
      }

      return ReturnVal;
    }

    public static string SYSTEM20(cCategory Category, ref bool Log)
    // Required Non-DAHS Components Reported for System
    {
      string ReturnVal = "";

      try
      {
        if (cDBConvert.ToBoolean(Category.GetCheckParameter("System_Dates_and_Hours_Consistent").ParameterValue) &&
            cDBConvert.ToBoolean(Category.GetCheckParameter("System_Type_Code_Valid").ParameterValue))
        {
          Category.SetCheckParameter("Missing_Formula_For_System", "", eParameterDataType.Boolean);

          DateTime SysEvalBeginDate = cDBConvert.ToDate(Category.GetCheckParameter("System_Evaluation_Begin_Date").ParameterValue, DateTypes.START);
          int SysEvalBeginHour = cDBConvert.ToHour(Category.GetCheckParameter("System_Evaluation_Begin_Hour").ParameterValue, DateTypes.START);
          DateTime SysEvalEndDate = cDBConvert.ToDate(Category.GetCheckParameter("System_Evaluation_End_Date").ParameterValue, DateTypes.END);
          int SysEvalEndHour = cDBConvert.ToHour(Category.GetCheckParameter("System_Evaluation_End_Hour").ParameterValue, DateTypes.END);

          DataRowView CurrentSystem = (DataRowView)Category.GetCheckParameter("Current_System").ParameterValue;
          string SystemTypeCd = cDBConvert.ToString(CurrentSystem["Sys_Type_Cd"]);

          if ((SystemTypeCd == "OILV") || (SystemTypeCd == "OILM"))
            System20_OverlapChecking(Category, "OFFM,BOFF", "FOIL", "N-OIL", "FOIL N-OIL",
                                               SysEvalBeginDate, SysEvalBeginHour,
                                               SysEvalEndDate, SysEvalEndHour);

          else if (SystemTypeCd == "GAS")
            System20_OverlapChecking(Category, "GFFM,BGFF", "FGAS", "N-GAS", "FGAS N-GAS",
                                               SysEvalBeginDate, SysEvalBeginHour,
                                               SysEvalEndDate, SysEvalEndHour);

          else if (SystemTypeCd == "FLOW")
            System20_OverlapChecking(Category, "FLOW", "FLOW", null, "FLOW",
                                               SysEvalBeginDate, SysEvalBeginHour,
                                               SysEvalEndDate, SysEvalEndHour);

          else if (SystemTypeCd == "H2O")
          {
            DataView FormulaView = (DataView)Category.GetCheckParameter("Formula_Records").ParameterValue;
            sFilterPair[] FormulaFilter = new sFilterPair[2];

            FormulaFilter[0].Set("Parameter_Cd", "H2O");
            FormulaFilter[1].Set("Equation_Cd", "F-31,M-1K", eFilterPairStringCompare.InList);

            DataView FormulaFilterView = FindActiveRows(FormulaView,
                                                        SysEvalBeginDate, SysEvalBeginHour,
                                                        SysEvalEndDate, SysEvalEndHour,
                                                        FormulaFilter);

            if (FormulaFilterView.Count == 0)
              Category.CheckCatalogResult = "C";
            else if (!CheckForHourRangeCovered(Category, FormulaFilterView,
                                               SysEvalBeginDate, SysEvalBeginHour,
                                               SysEvalEndDate, SysEvalEndHour))
              Category.CheckCatalogResult = "D";
          }
        }
        else
          Log = false;
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "SYSTEM20");
      }

      return ReturnVal;
    }

    private static void System20_OverlapChecking(cCategory ACategory, string AComponentTypeList,
                                                 string AFormulaParameterCd, string AFormulaEquationCd, string AFormulaMissingTag,
                                                 DateTime ASysEvalBeganDate, int ASysEvalBeganHour,
                                                 DateTime ASysEvalEndedDate, int ASysEvalEndedHour)
    {
      DataView SystemComponentView = (DataView)ACategory.GetCheckParameter("System_System_Component_Records").ParameterValue;
      sFilterPair[] SystemComponentFilter = new sFilterPair[1];

      SystemComponentFilter[0].Set("Component_Type_Cd", AComponentTypeList, eFilterPairStringCompare.InList);

      DataView SystemComponentFilterView = FindActiveRows(SystemComponentView,
                                                          ASysEvalBeganDate, ASysEvalBeganHour,
                                                          ASysEvalEndedDate, ASysEvalEndedHour,
                                                          SystemComponentFilter);

      if (SystemComponentFilterView.Count > 1)
      {
        DataView SystemComponentOverlapView = GetHourRangeIntersections(SystemComponentFilterView,
                                                                        ASysEvalBeganDate, ASysEvalBeganHour,
                                                                        ASysEvalEndedDate, ASysEvalEndedHour);

        if (SystemComponentOverlapView.Count > 0)
        {
          DataView FormulaView = (DataView)ACategory.GetCheckParameter("Formula_Records").ParameterValue;
          sFilterPair[] FormulaFilter;

          if (AFormulaEquationCd != null)
          {
            FormulaFilter = new sFilterPair[2];
            FormulaFilter[0].Set("Parameter_Cd", AFormulaParameterCd);
            FormulaFilter[1].Set("Equation_Cd", AFormulaEquationCd);
          }
          else
          {
            FormulaFilter = new sFilterPair[1];
            FormulaFilter[0].Set("Parameter_Cd", AFormulaParameterCd);
          }

          DataView FormulaFilterView = FindActiveRows(FormulaView,
                                                      ASysEvalBeganDate, ASysEvalBeganHour,
                                                      ASysEvalEndedDate, ASysEvalEndedHour,
                                                      FormulaFilter);

          if (FormulaFilterView.Count == 0)
          {
            ACategory.SetCheckParameter("Missing_Formula_For_System", AFormulaMissingTag, eParameterDataType.String);
            ACategory.CheckCatalogResult = "A";
          }
          else
            foreach (DataRowView SystemComponentOverlapRow in SystemComponentOverlapView)
            {
              if (!CheckForHourRangeCovered(ACategory, FormulaFilterView,
                                            cDBConvert.ToDate(SystemComponentOverlapRow["Begin_Date"], DateTypes.START),
                                            cDBConvert.ToHour(SystemComponentOverlapRow["Begin_Hour"], DateTypes.START),
                                            cDBConvert.ToDate(SystemComponentOverlapRow["End_Date"], DateTypes.END),
                                            cDBConvert.ToHour(SystemComponentOverlapRow["End_Hour"], DateTypes.END)))
              {
                ACategory.SetCheckParameter("Missing_Formula_For_System", AFormulaMissingTag, eParameterDataType.String);
                ACategory.CheckCatalogResult = "B";
              }
            }
        }
      }
    }

    public static string SYSTEM21(cCategory Category, ref bool Log)
    // Required Defaults Reported for System 
    {
      string ReturnVal = "";

      try
      {
        if (cDBConvert.ToBoolean(Category.GetCheckParameter("System_Type_Code_Valid").ParameterValue) &&
            cDBConvert.ToBoolean(Category.GetCheckParameter("System_Fuel_Code_Valid").ParameterValue) &&
            cDBConvert.ToBoolean(Category.GetCheckParameter("System_Dates_and_Hours_Consistent").ParameterValue))
        {
          DataRowView CurrentSystem = (DataRowView)Category.GetCheckParameter("Current_System").ParameterValue;
          string SystemTypeCd = cDBConvert.ToString(CurrentSystem["Sys_Type_Cd"]);
          string FuelCd = cDBConvert.ToString(CurrentSystem["Fuel_Cd"]);

          if (SystemTypeCd == "NOXE")
          {
            DateTime SysEvalBeganDate = cDBConvert.ToDate(Category.GetCheckParameter("System_Evaluation_Begin_Date").ParameterValue, DateTypes.START);
            int SysEvalBeganHour = cDBConvert.ToHour(Category.GetCheckParameter("System_Evaluation_Begin_Hour").ParameterValue, DateTypes.START);
            DateTime SysEvalEndedDate = cDBConvert.ToDate(Category.GetCheckParameter("System_Evaluation_End_Date").ParameterValue, DateTypes.END);
            int SysEvalEndedHour = cDBConvert.ToHour(Category.GetCheckParameter("System_Evaluation_End_Hour").ParameterValue, DateTypes.END);

            string MissingDefault = ""; string MissingDelim = "";
            string IncompleteDefault = ""; string IncompleteDelim = "";

            DataView MonitorDefaultView = (DataView)Category.GetCheckParameter("Default_Records").ParameterValue;
            sFilterPair[] MonitorDefaultFilter = new sFilterPair[4];

            MonitorDefaultFilter[0].Set("Parameter_Cd", "NORX");
            MonitorDefaultFilter[1].Set("Default_Purpose_Cd", "MD");
            MonitorDefaultFilter[2].Set("Fuel_Cd", FuelCd);
            MonitorDefaultFilter[3].Set("Operating_Condition_Cd", "A,U", eFilterPairStringCompare.InList);

            DataView NorxMonitorDefaultView = FindActiveRows(MonitorDefaultView,
                                                             SysEvalBeganDate, SysEvalBeganHour,
                                                             SysEvalEndedDate, SysEvalEndedHour,
                                                             MonitorDefaultFilter);

            if (NorxMonitorDefaultView.Count == 0)
            {
              MissingDefault += MissingDelim + "NORX MD";
              MissingDelim = ", ";
            }
            else if (!CheckForHourRangeCovered(Category, NorxMonitorDefaultView,
                                               SysEvalBeganDate, SysEvalBeganHour,
                                               SysEvalEndedDate, SysEvalEndedHour))
            {
              IncompleteDefault += IncompleteDelim + "NORX MD";
              IncompleteDelim = ", ";
            }

            MonitorDefaultFilter[0].Set("Parameter_Cd", "NOCX");
            MonitorDefaultFilter[1].Set("Default_Purpose_Cd", "MD");
            MonitorDefaultFilter[2].Set("Fuel_Cd", FuelCd);
            MonitorDefaultFilter[3].Set("Operating_Condition_Cd", "A,U", eFilterPairStringCompare.InList);

            DataView NocxMonitorDefaultView = FindActiveRows(MonitorDefaultView,
                                                             SysEvalBeganDate, SysEvalBeganHour,
                                                             SysEvalEndedDate, SysEvalEndedHour,
                                                             MonitorDefaultFilter);

            if (NocxMonitorDefaultView.Count == 0)
            {
              MissingDefault += MissingDelim + "NOCX MD";
              MissingDelim = ", ";
            }
            else if (!CheckForHourRangeCovered(Category, NocxMonitorDefaultView,
                                               SysEvalBeganDate, SysEvalBeganHour,
                                               SysEvalEndedDate, SysEvalEndedHour))
            {
              IncompleteDefault += IncompleteDelim + "NOCX MD";
              IncompleteDelim = ", ";
            }

            Category.SetCheckParameter("Missing_Default_for_System", MissingDefault.FormatList(false), eParameterDataType.String);
            Category.SetCheckParameter("Incomplete_Default_for_System", IncompleteDefault.FormatList(false), eParameterDataType.String);

            if ((MissingDefault != "") && (IncompleteDefault == ""))
              Category.CheckCatalogResult = "A";
            else if ((MissingDefault == "") && (IncompleteDefault != ""))
              Category.CheckCatalogResult = "B";
            else if ((MissingDefault != "") && (IncompleteDefault != ""))
              Category.CheckCatalogResult = "C";
          }
        }
        else
          Log = false;
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "SYSTEM21");
      }

      return ReturnVal;
    }

    public static string SYSTEM22(cCategory Category, ref bool Log)
    // Overlapping Primary NFS Systems 
    {
      string ReturnVal = "";

      try
      {
        if (cDBConvert.ToBoolean(Category.GetCheckParameter("System_Dates_and_Hours_Consistent").ParameterValue))
        {
          DataRowView CurrentSystem = (DataRowView)Category.GetCheckParameter("Current_System").ParameterValue;
          string SystemTypeCd = cDBConvert.ToString(CurrentSystem["Sys_Type_Cd"]);

          if( SystemTypeCd.InList( "OILV,OILM,GAS" ) )
          {
              string ComponentTypeCd = ( SystemTypeCd.InList( "OILV,OILM" ) ) ? "OFFM" : "GFFM";

            DateTime SysEvalBeginDate = cDBConvert.ToDate(Category.GetCheckParameter("System_Evaluation_Begin_Date").ParameterValue, DateTypes.START);
            int SysEvalBeginHour = cDBConvert.ToHour(Category.GetCheckParameter("System_Evaluation_Begin_Hour").ParameterValue, DateTypes.START);
            DateTime SysEvalEndDate = cDBConvert.ToDate(Category.GetCheckParameter("System_Evaluation_End_Date").ParameterValue, DateTypes.END);
            int SysEvalEndHour = cDBConvert.ToHour(Category.GetCheckParameter("System_Evaluation_End_Hour").ParameterValue, DateTypes.END);

            DataView SystemComponentView = (DataView)Category.GetCheckParameter("System_System_Component_Records").ParameterValue;
            sFilterPair[] SystemComponentFilter = new sFilterPair[1];

            SystemComponentFilter[0].Field = "Component_Type_Cd";
            SystemComponentFilter[0].Value = ComponentTypeCd;

            DataView SystemComponentFilterView = FindActiveRows(SystemComponentView,
                                                                SysEvalBeginDate, SysEvalBeginHour,
                                                                SysEvalEndDate, SysEvalEndHour,
                                                                SystemComponentFilter);

            if (SystemComponentFilterView.Count > 0)
            {
              DataView SystemFuelView = (DataView)Category.GetCheckParameter("System_Fuel_Flow_Records").ParameterValue;
              int RangeCount = 0;

              if (!CheckForHourRangeCovered(Category, SystemFuelView,
                                            SysEvalBeginDate, SysEvalBeginHour, SysEvalEndDate, SysEvalEndHour,
                                            ref RangeCount))
              {
                if (RangeCount == 0)
                  Category.CheckCatalogResult = "A";
                else
                  Category.CheckCatalogResult = "B";
              }
            }
          }
          else
            Log = false;
        }
        else
          Log = false;
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "SYSTEM22");
      }

      return ReturnVal;
    }

    public static string SYSTEM23(cCategory Category, ref bool Log)
    // Required Probe Reported for CEM System  
    {
      string ReturnVal = "";

      try
      {
        if (Category.GetCheckParameter("System_Type_Code_Valid").ValueAsBool() &&
            Category.GetCheckParameter("System_Dates_and_Hours_Consistent").ValueAsBool())
        {
          DataRowView CurrentSystem = Category.GetCheckParameter("Current_System").ValueAsDataRowView();
          DateTime? SystemEndedDate = cDBConvert.ToNullableDateTime(CurrentSystem["END_DATE"]);

          if (Category.GetCheckParameter("Required_Probe").ValueAsBool() &&
              (!SystemEndedDate.HasValue || (SystemEndedDate >= new DateTime(2008, 1, 1))))
          {
            DataView CurrentSystemComponentView = (DataView)Category.GetCheckParameter("System_System_Component_Records").ParameterValue;
            DateTime SysEvalBeganDate = cDBConvert.ToDate(Category.GetCheckParameter("System_Evaluation_Begin_Date").ParameterValue, DateTypes.START);
            int SysEvalBeganHour = cDBConvert.ToHour(Category.GetCheckParameter("System_Evaluation_Begin_Hour").ParameterValue, DateTypes.START);
            DateTime SysEvalEndedDate = cDBConvert.ToDate(Category.GetCheckParameter("System_Evaluation_End_Date").ParameterValue, DateTypes.END);
            int SysEvalEndedHour = cDBConvert.ToHour(Category.GetCheckParameter("System_Evaluation_End_Hour").ParameterValue, DateTypes.END);

            DataView SystemProbeView = cRowFilter.FindActiveRows(CurrentSystemComponentView,
                                                                 SysEvalBeganDate, SysEvalBeganHour,
                                                                 SysEvalEndedDate, SysEvalEndedHour,
                                                                 new cFilterCondition[] { new cFilterCondition("Component_Type_Cd", "PRB") });

            if (SystemProbeView.Count == 0)
              Category.CheckCatalogResult = "A";
            else
            {
              DateTime SysAltBeganDate; int SysAltBeganHour;
              {
                DateTime RequiredDate = new DateTime(2008, 1, 1);

                if (SysEvalBeganDate >= RequiredDate)
                { SysAltBeganDate = SysEvalBeganDate; SysAltBeganHour = SysEvalBeganHour; }
                else
                { SysAltBeganDate = RequiredDate; SysAltBeganHour = 0; }
              }

              if (!CheckForHourRangeCovered(Category, SystemProbeView,
                                            SysAltBeganDate, SysAltBeganHour,
                                            SysEvalEndedDate, SysEvalEndedHour))
                Category.CheckCatalogResult = "B";
            }
          }
        }
        else
          Log = false;
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "SYSTEM23");
      }

      return ReturnVal;
    }

    public static string SYSTEM24(cCategory Category, ref bool Log)
    // Duplicate System Records
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentSystem = (DataRowView)Category.GetCheckParameter("Current_System").ParameterValue;
        string MonSysId = cDBConvert.ToString(CurrentSystem["Mon_Sys_Id"]);
        string SystemIdentifier = cDBConvert.ToString(CurrentSystem["System_Identifier"]);

        DataView SystemView = (DataView)Category.GetCheckParameter("Monitor_System_Records").ParameterValue;
        sFilterPair[] SystemFilter = new sFilterPair[2];

        SystemFilter[0].Set("System_Identifier", SystemIdentifier);
        SystemFilter[1].Set("Mon_Sys_Id", MonSysId, true);

        if (CountRows(SystemView, SystemFilter) > 0)
          Category.CheckCatalogResult = "A";
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "SYSTEM24");
      }

      return ReturnVal;
    }


    #region Move to cChecks

    #endregion

  }
}
