using System;
using System.Collections.Generic;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.CheckEngine.SpecialParameterClasses;
using ECMPS.Checks.Em.Parameters;
using ECMPS.Checks.EmissionsReport;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Extensions;


namespace ECMPS.Checks.EmissionsChecks
{
    public class cHourlyAppendixDChecks : cEmissionsChecks
    {

        #region Constructors

        public cHourlyAppendixDChecks(cEmissionsReportProcess emissionReportProcess)
          : base(emissionReportProcess)
        {
            CheckProcedures = new dCheckProcedure[47];

            CheckProcedures[1] = new dCheckProcedure(HOURAD1);
            CheckProcedures[2] = new dCheckProcedure(HOURAD2);
            CheckProcedures[3] = new dCheckProcedure(HOURAD3);
            CheckProcedures[4] = new dCheckProcedure(HOURAD4);
            CheckProcedures[5] = new dCheckProcedure(HOURAD5);
            CheckProcedures[6] = new dCheckProcedure(HOURAD6);
            CheckProcedures[7] = new dCheckProcedure(HOURAD7);
            CheckProcedures[8] = new dCheckProcedure(HOURAD8);
            CheckProcedures[9] = new dCheckProcedure(HOURAD9);
            CheckProcedures[10] = new dCheckProcedure(HOURAD10);

            CheckProcedures[11] = new dCheckProcedure(HOURAD11);
            CheckProcedures[12] = new dCheckProcedure(HOURAD12);
            CheckProcedures[13] = new dCheckProcedure(HOURAD13);
            CheckProcedures[14] = new dCheckProcedure(HOURAD14);
            CheckProcedures[15] = new dCheckProcedure(HOURAD15);
            CheckProcedures[16] = new dCheckProcedure(HOURAD16);
            CheckProcedures[17] = new dCheckProcedure(HOURAD17);
            CheckProcedures[18] = new dCheckProcedure(HOURAD18);
            CheckProcedures[19] = new dCheckProcedure(HOURAD19);
            CheckProcedures[20] = new dCheckProcedure(HOURAD20);

            CheckProcedures[21] = new dCheckProcedure(HOURAD21);
            CheckProcedures[22] = new dCheckProcedure(HOURAD22);
            CheckProcedures[23] = new dCheckProcedure(HOURAD23);
            CheckProcedures[24] = new dCheckProcedure(HOURAD24);
            CheckProcedures[25] = new dCheckProcedure(HOURAD25);
            CheckProcedures[26] = new dCheckProcedure(HOURAD26);
            CheckProcedures[27] = new dCheckProcedure(HOURAD27);
            CheckProcedures[28] = new dCheckProcedure(HOURAD28);
            CheckProcedures[29] = new dCheckProcedure(HOURAD29);
            CheckProcedures[30] = new dCheckProcedure(HOURAD30);

            CheckProcedures[31] = new dCheckProcedure(HOURAD31);
            CheckProcedures[32] = new dCheckProcedure(HOURAD32);
            CheckProcedures[33] = new dCheckProcedure(HOURAD33);
            CheckProcedures[34] = new dCheckProcedure(HOURAD34);
            CheckProcedures[35] = new dCheckProcedure(HOURAD35);
            CheckProcedures[36] = new dCheckProcedure(HOURAD36);
            CheckProcedures[37] = new dCheckProcedure(HOURAD37);
            CheckProcedures[38] = new dCheckProcedure(HOURAD38);
            CheckProcedures[39] = new dCheckProcedure(HOURAD39);
            CheckProcedures[40] = new dCheckProcedure(HOURAD40);

            CheckProcedures[44] = new dCheckProcedure(HOURAD44);
            CheckProcedures[45] = new dCheckProcedure(HOURAD45);
            CheckProcedures[46] = new dCheckProcedure(HOURAD46);
        }

        #endregion


        #region Public Static Methods: Checks

        #region 1 - 10

        public static string HOURAD1(cCategory Category, ref bool Log)
        //Initialize Accumulators for Appendix D Calculations
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("Co2_App_D_Accumulator", 0.0m, eParameterDataType.Decimal);
                Category.SetCheckParameter("Hi_App_D_Accumulator", 0.0m, eParameterDataType.Decimal);
                Category.SetCheckParameter("Noxr_App_E_Accumulator", 0.0m, eParameterDataType.Decimal);
                Category.SetCheckParameter("So2_App_D_Accumulator", 0.0m, eParameterDataType.Decimal);
                Category.SetCheckParameter("Current_Fuel_Flow_Record", null, eParameterDataType.DataRowView);
                Category.SetCheckParameter("Current_Fuel_Group", null, eParameterDataType.String);
                Category.SetCheckParameter("Fuels_Used_List", null, eParameterDataType.String);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURAD1");
            }

            return ReturnVal;
        }

        public static string HOURAD2(cCategory Category, ref bool Log)
        //Initialize Oil Fuel Flow Record
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("Current_Fuel_Group", "OIL", eParameterDataType.String);
                DataRowView CurrentFFRecord = (DataRowView)Category.GetCheckParameter("Current_Oil_Fuel_Flow_Record").ParameterValue;
                Category.SetCheckParameter("Current_Fuel_Flow_Record", CurrentFFRecord, eParameterDataType.DataRowView);
                if (cDBConvert.ToString(CurrentFFRecord["UNIT_FUEL_CD"]) == "OOL")
                    Category.SetCheckParameter("Special_Fuel_Burned", true, eParameterDataType.Boolean);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURAD2");
            }

            return ReturnVal;
        }

        public static string HOURAD3(cCategory Category, ref bool Log)
        //Initialize Gas Fuel Flow Record
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentFFRecord = (DataRowView)Category.GetCheckParameter("Current_Fuel_Flow_Record").ParameterValue;
                Category.SetCheckParameter("Current_Fuel_Group", cDBConvert.ToString(CurrentFFRecord["fuel_group_Cd"]), eParameterDataType.String);
                //Category.SetCheckParameter("Current_Fuel_Flow_Record", CurrentFFRecord, eParameterDataType.DataRowView);
                if (cDBConvert.ToString(CurrentFFRecord["UNIT_FUEL_CD"]).InList("OGS,PRG,OOL"))
                    Category.SetCheckParameter("Special_Fuel_Burned", true, eParameterDataType.Boolean);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURAD3");
            }

            return ReturnVal;
        }

        public static string HOURAD4(cCategory Category, ref bool Log)
        //Check Fuel Usage Time
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("HFF_Usage_Time_Status", true, eParameterDataType.Boolean);

                DataRowView CurrentFFRecord = Category.GetCheckParameter("Current_Fuel_Flow_Record").ValueAsDataRowView();

                decimal UsageTime = cDBConvert.ToDecimal(CurrentFFRecord["FUEL_USAGE_TIME"]);

                if ((UsageTime < 0) || (UsageTime > 1)) //null handled by < 0
                {
                    Category.SetCheckParameter("HFF_Usage_Time_Status", false, eParameterDataType.Boolean);
                    Category.CheckCatalogResult = "A";
                }
                else if ((UsageTime > 0) && (UsageTime <= 1)) //operating althoug <= 1 is double checking part of if condition
                {
                    DataRowView HourlyOpDataRow = Category.GetCheckParameter("Current_Hourly_Op_Record").ValueAsDataRowView();

                    decimal OpTime = cDBConvert.ToDecimal(HourlyOpDataRow["OP_TIME"]);

                    if (0 < OpTime && OpTime <= 1)
                    {
                        string FuelList = Category.GetCheckParameter("Fuels_Used_List").ValueAsString();

                        string FuelCd = cDBConvert.ToString(CurrentFFRecord["FUEL_CD"]);

                        if (!FuelCd.InList(FuelList)) //Prevents duplicate counting of a fuel in a single hour.
                        {
                            Dictionary<string, int>[] FuelOpHoursAccumulatorArray = ((Dictionary<string, int>[])Category.GetCheckParameter("Fuel_Op_Hours_Accumulator_Array").ParameterValue);

                            if (FuelOpHoursAccumulatorArray[Category.CurrentMonLocPos].ContainsKey(FuelCd))
                                FuelOpHoursAccumulatorArray[Category.CurrentMonLocPos][FuelCd] += 1;
                            else
                                FuelOpHoursAccumulatorArray[Category.CurrentMonLocPos][FuelCd] = 1;

                            Category.SetCheckParameter("Fuel_Op_Hours_Accumulator_Array", FuelOpHoursAccumulatorArray, eParameterDataType.Object);
                            Category.SetCheckParameter("Fuels_Used_List", FuelList.ListAdd(FuelCd), eParameterDataType.String);
                        }

                        if (UsageTime > OpTime)
                        {
                            Category.SetCheckParameter("HFF_Usage_Time_Status", false, eParameterDataType.Boolean);
                            Category.CheckCatalogResult = "B";
                        }
                        else if (Category.GetCheckParameter("Hourly_Fuel_Flow_Count_For_Gas").ValueAsInt(0) +
                                   Category.GetCheckParameter("Hourly_Fuel_Flow_Count_For_Oil").ValueAsInt(0) == 1 &&
                                   (Category.GetCheckParameter("MP_Pipe_Config_for_Hourly_Checks").ValueAsString() == "" ||
                                    cDBConvert.ToString(HourlyOpDataRow["LOCATION_NAME"]).StartsWith("CP")) &&
                                  UsageTime != OpTime)
                        {
                            Category.SetCheckParameter("HFF_Usage_Time_Status", false, eParameterDataType.Boolean);
                            Category.CheckCatalogResult = "B";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURAD4");
            }

            return ReturnVal;
        }

        public static string HOURAD5(cCategory Category, ref bool Log)
        //Check Volumetric SODC Code
        {
            string ReturnVal = "";

            try
            {
                bool Status = true;
                DataRowView CurrentFFRecord = (DataRowView)Category.GetCheckParameter("Current_Fuel_Flow_Record").ParameterValue;
                string SODVolCd = cDBConvert.ToString(CurrentFFRecord["SOD_VOLUMETRIC_CD"]);
                if (SODVolCd == "")
                {
                    if (CurrentFFRecord["VOLUMETRIC_FLOW_RATE"] != DBNull.Value)
                    {
                        Status = false;
                        Category.CheckCatalogResult = "A";
                    }
                }
                else
                {
                    if (CurrentFFRecord["VOLUMETRIC_FLOW_RATE"] == DBNull.Value)
                    {
                        Status = false;
                        Category.CheckCatalogResult = "B";
                    }
                    else
                      if (Convert.ToString(Category.GetCheckParameter("Current_Fuel_Group").ParameterValue) == "GAS" && SODVolCd.InList("5,6"))
                    {
                        Status = false;
                        Category.CheckCatalogResult = "C";
                    }
                    else
                        if (SODVolCd == "3" && !Convert.ToBoolean(Category.GetCheckParameter("Current_Unit_is_Peaking").ParameterValue))
                    {
                        Status = false;
                        Category.CheckCatalogResult = "D";
                    }
                    else
                    {
                        string FuelIndCd = Convert.ToString(Category.GetCheckParameter("HFF_Fuel_Indicator_Code").ParameterValue);
                        if (FuelIndCd != "")
                            if (SODVolCd == "4" && FuelIndCd != "E")
                            {
                                Status = false;
                                Category.CheckCatalogResult = "E";
                            }
                            else
                              if (SODVolCd.InList("5,6") && FuelIndCd != "I")
                            {
                                Status = false;
                                Category.CheckCatalogResult = "F";
                            }
                    }
                }
                Category.SetCheckParameter("HFF_SODC_Status", Status, eParameterDataType.Boolean);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURAD5");
            }

            return ReturnVal;
        }

        public static string HOURAD6(cCategory Category, ref bool Log)
        //Check Oil Mass SODC Code
        {
            string ReturnVal = "";

            try
            {
                bool Status = true;
                DataRowView CurrentFFRecord = (DataRowView)Category.GetCheckParameter("Current_Fuel_Flow_Record").ParameterValue;
                string SODMassCd = cDBConvert.ToString(CurrentFFRecord["SOD_MASS_CD"]);
                if (SODMassCd == "")
                {
                    if (CurrentFFRecord["MASS_FLOW_RATE"] != DBNull.Value)
                    {
                        Status = false;
                        Category.CheckCatalogResult = "A";
                    }
                }
                else
                {
                    if (CurrentFFRecord["MASS_FLOW_RATE"] == DBNull.Value)
                    {
                        Status = false;
                        Category.CheckCatalogResult = "B";
                    }
                    else
                      if (CurrentFFRecord["VOLUMETRIC_FLOW_RATE"] != DBNull.Value && SODMassCd != "2")
                    {
                        Status = false;
                        Category.CheckCatalogResult = "C";
                    }
                    else
                        if (CurrentFFRecord["VOLUMETRIC_FLOW_RATE"] == DBNull.Value && SODMassCd == "2")
                    {
                        Status = false;
                        Category.CheckCatalogResult = "D";
                    }
                    else
                    {
                        string FuelIndCd = Convert.ToString(Category.GetCheckParameter("HFF_Fuel_Indicator_Code").ParameterValue);
                        if (SODMassCd == "3" && !Convert.ToBoolean(Category.GetCheckParameter("Current_Unit_is_Peaking").ParameterValue))
                        {
                            Status = false;
                            Category.CheckCatalogResult = "E";
                        }
                        else
                          if (FuelIndCd != "")
                        {
                            if (SODMassCd == "4" && FuelIndCd != "E")
                            {
                                Status = false;
                                Category.CheckCatalogResult = "F";
                            }
                            else
                                if (SODMassCd.InList("5,6") && FuelIndCd != "I")
                            {
                                Status = false;
                                Category.CheckCatalogResult = "G";
                            }
                        }
                    }
                }
                Category.SetCheckParameter("HFF_Mass_SODC_Status", Status, eParameterDataType.Boolean);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURAD6");
            }

            return ReturnVal;
        }

        public static string HOURAD7(cCategory Category, ref bool Log)
        //Check Fuel Flow Monitoring System
        {
            string ReturnVal = "";

            try
            {
                string HFFSysType = "";
                Category.SetCheckParameter("Fuel_Flow_Component_Records", null, eParameterDataType.DataView);
                Category.SetCheckParameter("Current_Appendix_D_Status", null, eParameterDataType.String);
                DataRowView CurrentFFRecord = (DataRowView)Category.GetCheckParameter("Current_Fuel_Flow_Record").ParameterValue;
                string MonSysId = cDBConvert.ToString(CurrentFFRecord["MON_SYS_ID"]);
                string SODVolCd = cDBConvert.ToString(CurrentFFRecord["SOD_VOLUMETRIC_CD"]);
                string SODMassCd = cDBConvert.ToString(CurrentFFRecord["SOD_MASS_CD"]);
                string FuelGrp = Convert.ToString(Category.GetCheckParameter("Current_Fuel_Group").ParameterValue);
                if (MonSysId == "")
                {
                    if (SODVolCd.InList("0,9") || (FuelGrp == "OIL" && (SODMassCd.InList("0,9"))))
                        Category.CheckCatalogResult = "A";
                    else
                    if (!Convert.ToBoolean(Category.GetCheckParameter("Legacy_Data_Evaluation").ParameterValue) &&
                        (SODVolCd.InList("1,3") || (FuelGrp == "OIL" && SODMassCd.InList("1,3"))))
                        Category.CheckCatalogResult = "B";
                    else
                      if (FuelGrp == "GAS")
                        HFFSysType = "GAS";
                    else
                        if (cDBConvert.ToDecimal(CurrentFFRecord["VOLUMETRIC_FLOW_RATE"]) != decimal.MinValue)
                        HFFSysType = "OILV";
                    else
                        HFFSysType = "OILM";
                }
                else
                  if (SODVolCd == "4")
                    Category.CheckCatalogResult = "C";
                else
                      if (FuelGrp == "OIL" && (SODVolCd.InList("5,6") || SODMassCd.InList("5,6")))
                    Category.CheckCatalogResult = "C";
                else
                {
                    DataView MonitorSystemView = (DataView)Category.GetCheckParameter("Monitor_System_Records_By_Hour_Location").ParameterValue;
                    DataRowView MonitorSystemRow;
                    sFilterPair[] MonSysRecsFilter = new sFilterPair[1];
                    MonSysRecsFilter[0].Set("MON_SYS_ID", MonSysId);
                    if (!FindRow(MonitorSystemView, MonSysRecsFilter, out MonitorSystemRow))
                        Category.CheckCatalogResult = "D";
                    else
                    {
                        string MonSysRecSysTypeCd = cDBConvert.ToString(MonitorSystemRow["SYS_TYPE_CD"]);
                        if (FuelGrp == "GAS" && MonSysRecSysTypeCd != "GAS")
                            Category.CheckCatalogResult = "E";
                        else
                            if (FuelGrp == "OIL" && !MonSysRecSysTypeCd.InList("OILV,OILM"))
                            Category.CheckCatalogResult = "F";
                        else
                            if (FuelGrp == "OIL" && SODMassCd == "2" && MonSysRecSysTypeCd != "OILV")
                            Category.CheckCatalogResult = "G";
                        else
                        {
                            HFFSysType = MonSysRecSysTypeCd;
                            string FuelCd = cDBConvert.ToString(MonitorSystemRow["FUEL_CD"]);
                            if (FuelCd != "" && FuelCd != cDBConvert.ToString(CurrentFFRecord["FUEL_CD"]))
                            {
                                Category.SetCheckParameter("HFF_System_Fuel", FuelCd, eParameterDataType.String);
                                Category.CheckCatalogResult = "H";
                            }
                            else
                                if (SODVolCd.InList("0,9") || (FuelGrp == "OIL" && (SODMassCd.InList("0,9"))))
                            {
                                DataView MonSysCompRecs = Category.GetCheckParameter("Monitor_System_Component_Records_By_Hour_Location").ValueAsDataView();
                                sFilterPair[] Filter = new sFilterPair[2];
                                Filter[0].Set("MON_SYS_ID", MonSysId);
                                DataView MonSysCompRecsFound;
                                DataTable MonSysCompRecsFoundTbl;
                                DataTable FFCompRecsTbl = MonSysCompRecs.Table.Clone();
                                FFCompRecsTbl.Rows.Clear();//not necessary?
                                DataRow FilterRow;
                                if (FuelGrp == "OIL")
                                {
                                    Filter[1].Set("COMPONENT_TYPE_CD", "OFFM,BOFF", eFilterPairStringCompare.InList);
                                    MonSysCompRecsFound = FindRows(MonSysCompRecs, Filter);
                                    MonSysCompRecsFoundTbl = MonSysCompRecsFound.Table.Copy();
                                    if (MonSysCompRecsFound.Count > 0)
                                    {
                                        foreach (DataRow SourceRow in MonSysCompRecsFoundTbl.Rows)
                                            if (cDBConvert.ToString(SourceRow["COMPONENT_TYPE_CD"]) == "OFFM")
                                            {
                                                FilterRow = FFCompRecsTbl.NewRow();
                                                foreach (DataColumn Column in FFCompRecsTbl.Columns)
                                                    FilterRow[Column.ColumnName] = SourceRow[Column.ColumnName];

                                                FFCompRecsTbl.Rows.Add(FilterRow);
                                            }
                                        Category.SetCheckParameter("Fuel_Flow_Component_Records", FFCompRecsTbl.DefaultView, eParameterDataType.DataView);
                                    }
                                    else
                                        Category.CheckCatalogResult = "I";

                                }
                                else
                                  if (FuelGrp == "GAS")
                                {
                                    Filter[1].Set("COMPONENT_TYPE_CD", "GFFM,BGFF", eFilterPairStringCompare.InList);
                                    MonSysCompRecsFound = FindRows(MonSysCompRecs, Filter);
                                    MonSysCompRecsFoundTbl = MonSysCompRecsFound.Table.Copy();
                                    if (MonSysCompRecsFound.Count > 0)
                                    {
                                        foreach (DataRow SourceRow in MonSysCompRecsFoundTbl.Rows)
                                            if (cDBConvert.ToString(SourceRow["COMPONENT_TYPE_CD"]) == "GFFM")
                                            {
                                                FilterRow = FFCompRecsTbl.NewRow();
                                                foreach (DataColumn Column in FFCompRecsTbl.Columns)
                                                    FilterRow[Column.ColumnName] = SourceRow[Column.ColumnName];

                                                FFCompRecsTbl.Rows.Add(FilterRow);
                                            }
                                        Category.SetCheckParameter("Fuel_Flow_Component_Records", FFCompRecsTbl.DefaultView, eParameterDataType.DataView);
                                    }
                                    else
                                        Category.CheckCatalogResult = "I";
                                }
                            }
                        }
                    }
                }
                if (HFFSysType != "")
                    Category.SetCheckParameter("HFF_System_Type", HFFSysType, eParameterDataType.String);
                else
                    Category.SetCheckParameter("HFF_System_Type", null, eParameterDataType.String);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURAD7");
            }

            return ReturnVal;
        }

        public static string HOURAD8(cCategory Category, ref bool Log)
        //Check Volumetric Units of Measure
        {
            string ReturnVal = "";

            try
            {
                bool Status = true;
                DataRowView CurrentFFRecord = (DataRowView)Category.GetCheckParameter("Current_Fuel_Flow_Record").ParameterValue;
                string UOMCd = cDBConvert.ToString(CurrentFFRecord["VOLUMETRIC_UOM_CD"]);
                decimal VolFlowRt = cDBConvert.ToDecimal(CurrentFFRecord["VOLUMETRIC_FLOW_RATE"]);
                if (UOMCd == "")
                {
                    if (VolFlowRt != decimal.MinValue)
                    {
                        Status = false;
                        Category.CheckCatalogResult = "A";
                    }
                }
                else
                  if (VolFlowRt == decimal.MinValue)
                {
                    Status = false;
                    Category.CheckCatalogResult = "B";
                }
                else
                {
                    string FuelGrp = Convert.ToString(Category.GetCheckParameter("Current_Fuel_Group").ParameterValue);
                    if (FuelGrp == "OIL" && !UOMCd.InList("GALHR,BBLHR,M3HR,SCFH"))
                    {
                        Status = false;
                        Category.CheckCatalogResult = "C";
                    }
                    else
                      if (FuelGrp == "GAS" && UOMCd != "HSCF")
                    {
                        Status = false;
                        Category.CheckCatalogResult = "C";
                    }
                }
                Category.SetCheckParameter("HFF_UOM_Status", Status, eParameterDataType.Boolean);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURAD8");
            }

            return ReturnVal;
        }

        public static string HOURAD9(cCategory Category, ref bool Log)
        //Check Fuel in HFF Record
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("HFF_Fuel_Indicator_Code", null, eParameterDataType.String);
                DataView UnitFuelRecs = (DataView)Category.GetCheckParameter("Fuel_Records_By_Hour_Location").ParameterValue;
                sFilterPair[] Filter = new sFilterPair[1];
                DataRowView CurrentFFRecord = (DataRowView)Category.GetCheckParameter("Current_Fuel_Flow_Record").ParameterValue;
                Filter[0].Set("FUEL_CD", cDBConvert.ToString(CurrentFFRecord["UNIT_FUEL_CD"]));
                UnitFuelRecs = FindRows(UnitFuelRecs, Filter);
                if (UnitFuelRecs.Count > 0)
                    Category.SetCheckParameter("HFF_Fuel_Indicator_Code", cDBConvert.ToString(UnitFuelRecs[0]["INDICATOR_CD"]), eParameterDataType.String);
                else
                    Category.CheckCatalogResult = "A";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURAD9");
            }

            return ReturnVal;
        }

        public static string HOURAD10(cCategory Category, ref bool Log)
        //Check Volumetric Flow in HFF Record
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("HFF_Calc_Volumetric_Rate", null, eParameterDataType.Decimal);
                Category.SetCheckParameter("HFF_Max_Heat_Input_for_Volume", null, eParameterDataType.Decimal);
                string HFFSysType = cDBConvert.ToString(Category.GetCheckParameter("HFF_System_Type").ParameterValue);
                if (HFFSysType != "" && Convert.ToBoolean(Category.GetCheckParameter("HFF_SODC_Status").ParameterValue) &&
                    Convert.ToBoolean(Category.GetCheckParameter("HFF_Mass_SODC_Status").ParameterValue) && Convert.ToBoolean(Category.GetCheckParameter("HFF_UOM_Status").ParameterValue))
                {
                    DataRowView CurrentFFRecord = (DataRowView)Category.GetCheckParameter("Current_Fuel_Flow_Record").ParameterValue;
                    decimal VolFlowRt = cDBConvert.ToDecimal(CurrentFFRecord["VOLUMETRIC_FLOW_RATE"]);
                    string UOMCd = cDBConvert.ToString(CurrentFFRecord["VOLUMETRIC_UOM_CD"]);
                    string SODVolCd = cDBConvert.ToString(CurrentFFRecord["SOD_VOLUMETRIC_CD"]);
                    if (VolFlowRt == decimal.MinValue)
                    {
                        if (HFFSysType != "OILM")
                            Category.CheckCatalogResult = "A";
                    }
                    else
                      if (HFFSysType == "OILM")
                        Category.CheckCatalogResult = "B";
                    else
                        if (SODVolCd == "4")
                        if (Convert.ToString(Category.GetCheckParameter("Current_Entity_Type").ParameterValue) == "Unit")
                        {
                            DataView UnitCapRecs = (DataView)Category.GetCheckParameter("Location_Capacity_Records_for_Hour_Location").ParameterValue;
                            if (UnitCapRecs.Count == 1)
                            {
                                decimal MaxHI = cDBConvert.ToDecimal(UnitCapRecs[0]["MAX_HI_CAPACITY"]);
                                if (MaxHI > 0)
                                {
                                    Category.SetCheckParameter("HFF_Max_Heat_Input_for_Volume", MaxHI, eParameterDataType.Decimal);
                                    decimal HFFGCV = Category.GetCheckParameter("HFF_GCV").ValueAsDecimal();
                                    if (HFFGCV != decimal.MinValue)
                                        if (CurrentFFRecord["MASS_FLOW_RATE"] == DBNull.Value)
                                            Category.SetCheckParameter("HFF_Calc_Volumetric_Rate", Math.Round(MaxHI / HFFGCV * 1000000, 1, MidpointRounding.AwayFromZero), eParameterDataType.Decimal);
                                        else
                                        {
                                            decimal HFFDensity = Category.GetCheckParameter("HFF_Density").ValueAsDecimal();
                                            if (HFFDensity != decimal.MinValue)
                                                Category.SetCheckParameter("HFF_Calc_Volumetric_Rate", Math.Round(MaxHI / HFFGCV / HFFDensity * 1000000, 1, MidpointRounding.AwayFromZero), eParameterDataType.Decimal);
                                        }
                                }
                                else
                                    Category.CheckCatalogResult = "M";
                            }
                            else
                                Category.CheckCatalogResult = "M";
                        }
                        else
                          if (VolFlowRt <= 0)
                            Category.CheckCatalogResult = "E";
                        else
                            Category.SetCheckParameter("HFF_Calc_Volumetric_Rate", VolFlowRt, eParameterDataType.Decimal);
                    else
                          if (SODVolCd == "9")
                    {
                        string VolDefParam;
                        if (Convert.ToString(Category.GetCheckParameter("Current_Fuel_Group").ParameterValue) == "GAS")
                            VolDefParam = "MNGF";
                        else
                            VolDefParam = "MNOF";
                        Category.SetCheckParameter("HFF_Volumetric_Default_Parameter", VolDefParam, eParameterDataType.String);
                        DataView MonDefRecs = (DataView)Category.GetCheckParameter("Monitor_Default_Records_by_Hour_Location").ParameterValue;
                        sFilterPair[] Filter1 = new sFilterPair[2];
                        Filter1[0].Set("PARAMETER_CD", VolDefParam);
                        Filter1[1].Set("FUEL_CD", cDBConvert.ToString(CurrentFFRecord["FUEL_CD"]));
                        DataView MonDefRecsFound = FindRows(MonDefRecs, Filter1);
                        if (MonDefRecsFound.Count != 1)
                            Category.CheckCatalogResult = "C";
                        else
                        {
                            decimal DefVal = cDBConvert.ToDecimal(MonDefRecsFound[0]["DEFAULT_VALUE"]);
                            if (DefVal <= 0)
                                Category.CheckCatalogResult = "D";
                            else
                              if (cDBConvert.ToString(MonDefRecsFound[0]["DEFAULT_UOM_CD"]) == UOMCd)
                            {
                                Category.SetCheckParameter("HFF_Calc_Volumetric_Rate", DefVal, eParameterDataType.Decimal);
                                if (VolFlowRt <= 0)
                                    Category.CheckCatalogResult = "E";
                                else
                                  if (VolFlowRt != DefVal)
                                    Category.CheckCatalogResult = "F";
                            }
                            else
                                if (VolFlowRt <= 0)
                                Category.CheckCatalogResult = "E";
                            else
                                Category.CheckCatalogResult = "G";
                        }
                    }
                    else
                            if (CurrentFFRecord["MON_SYS_ID"] != DBNull.Value)
                    {
                        if (VolFlowRt <= 0)
                            Category.CheckCatalogResult = "E";
                        else
                        {
                            if (SODVolCd != "3")
                                Category.SetCheckParameter("HFF_Calc_Volumetric_Rate", VolFlowRt, eParameterDataType.Decimal);
                            DataView SysFFRecs = (DataView)Category.GetCheckParameter("System_Fuel_Flow_Records_For_Hour").ParameterValue;
                            if (SysFFRecs.Count != 1)
                                Category.CheckCatalogResult = "H";
                            else
                            {
                                decimal MaxFFRate = cDBConvert.ToDecimal(SysFFRecs[0]["MAX_RATE"]);
                                if (MaxFFRate <= 0)
                                    Category.CheckCatalogResult = "I";
                                else
                                  if (cDBConvert.ToString(SysFFRecs[0]["SYS_FUEL_UOM_CD"]) == UOMCd)
                                    if (SODVolCd == "3")
                                    {
                                        Category.SetCheckParameter("HFF_Calc_Volumetric_Rate", MaxFFRate, eParameterDataType.Decimal);
                                        if (VolFlowRt != MaxFFRate)
                                            Category.CheckCatalogResult = "J";
                                    }
                                    else
                                    {
                                        if (Convert.ToDecimal(Category.GetCheckParameter("HFF_Calc_Volumetric_Rate").ParameterValue) > MaxFFRate)
                                            Category.CheckCatalogResult = "K";
                                    }
                                else
                                    Category.CheckCatalogResult = "L";
                            }
                        }
                    }
                    else
                    {
                        if (VolFlowRt <= 0)
                            Category.CheckCatalogResult = "E";
                        else
                            Category.SetCheckParameter("HFF_Calc_Volumetric_Rate", VolFlowRt, eParameterDataType.Decimal);
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURAD10");
            }

            return ReturnVal;
        }

        #endregion


        #region 11 - 20

        public static string HOURAD11(cCategory Category, ref bool Log)
        //Check Mass Oil Flow in HFF Record
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("HFF_Calc_Mass_Oil_Rate", null, eParameterDataType.Decimal);
                Category.SetCheckParameter("HFF_Max_Heat_Input_for_Mass", null, eParameterDataType.Decimal);
                string HFFSysType = cDBConvert.ToString(Category.GetCheckParameter("HFF_System_Type").ParameterValue);
                if (HFFSysType != "" && Convert.ToBoolean(Category.GetCheckParameter("HFF_SODC_Status").ParameterValue) &&
                    Convert.ToBoolean(Category.GetCheckParameter("HFF_Mass_SODC_Status").ParameterValue) && Convert.ToBoolean(Category.GetCheckParameter("HFF_UOM_Status").ParameterValue))
                {
                    DataRowView CurrentFFRecord = (DataRowView)Category.GetCheckParameter("Current_Fuel_Flow_Record").ParameterValue;
                    decimal MassFlowRt = cDBConvert.ToDecimal(CurrentFFRecord["MASS_FLOW_RATE"]);
                    string SODMassCd = cDBConvert.ToString(CurrentFFRecord["SOD_MASS_CD"]);
                    if (MassFlowRt == decimal.MinValue)
                    {
                        if (HFFSysType == "OILM")
                            Category.CheckCatalogResult = "A";
                        else
                          if (HFFSysType == "OILV")
                            if (SODMassCd == "2")
                                Category.CheckCatalogResult = "B";
                            else
                              if (Convert.ToBoolean(Category.GetCheckParameter("Current_Unit_Is_Arp").ParameterValue))
                                Category.CheckCatalogResult = "C";
                    }
                    else
                      if (HFFSysType == "GAS")
                        Category.CheckCatalogResult = "D";
                    else
                        if (HFFSysType == "OILV")
                    {
                        if (SODMassCd == "2" && MassFlowRt <= 0)
                            Category.CheckCatalogResult = "E";
                    }
                    else
                          if (SODMassCd == "4")
                        if (Convert.ToString(Category.GetCheckParameter("Current_Entity_Type").ParameterValue) == "Unit")
                        {
                            DataView UnitCapRecs = (DataView)Category.GetCheckParameter("Location_Capacity_Records_for_Hour_Location").ParameterValue;
                            if (UnitCapRecs.Count == 1)
                            {
                                decimal MaxHI = cDBConvert.ToDecimal(UnitCapRecs[0]["MAX_HI_CAPACITY"]);
                                if (MaxHI > 0)
                                {
                                    Category.SetCheckParameter("HFF_Max_Heat_Input_for_Mass", MaxHI, eParameterDataType.Decimal);
                                    decimal HFFGCV = Category.GetCheckParameter("HFF_GCV").ValueAsDecimal();
                                    if (HFFGCV != decimal.MinValue)
                                        Category.SetCheckParameter("HFF_Calc_Mass_Oil_Rate", Math.Round(MaxHI / HFFGCV * 1000000, 1, MidpointRounding.AwayFromZero), eParameterDataType.Decimal);
                                }
                                else
                                    Category.CheckCatalogResult = "M";
                            }
                            else
                                Category.CheckCatalogResult = "M";
                        }
                        else
                          if (MassFlowRt <= 0)
                            Category.CheckCatalogResult = "E";
                        else
                            Category.SetCheckParameter("HFF_Calc_Mass_Oil_Rate", MassFlowRt, eParameterDataType.Decimal);
                    else
                            if (SODMassCd == "9")
                    {
                        string OilDefParam = "MNOF";
                        Category.SetCheckParameter("HFF_Volumetric_Default_Parameter", OilDefParam, eParameterDataType.String);
                        DataView MonDefRecs = (DataView)Category.GetCheckParameter("Monitor_Default_Records_by_Hour_Location").ParameterValue;
                        sFilterPair[] Filter1 = new sFilterPair[2];
                        Filter1[0].Set("PARAMETER_CD", OilDefParam);
                        Filter1[1].Set("FUEL_CD", cDBConvert.ToString(CurrentFFRecord["FUEL_CD"]));
                        DataView MonDefRecsFound = FindRows(MonDefRecs, Filter1);
                        if (MonDefRecsFound.Count != 1)
                            Category.CheckCatalogResult = "F";
                        else
                        {
                            decimal DefVal = cDBConvert.ToDecimal(MonDefRecsFound[0]["DEFAULT_VALUE"]);
                            if (DefVal <= 0 || cDBConvert.ToString(MonDefRecsFound[0]["DEFAULT_UOM_CD"]) != "LBHR")
                                Category.CheckCatalogResult = "G";
                            else
                            {
                                Category.SetCheckParameter("HFF_Calc_Mass_Oil_Rate", DefVal, eParameterDataType.Decimal);
                                if (MassFlowRt <= 0)
                                    Category.CheckCatalogResult = "E";
                                else
                                  if (MassFlowRt != DefVal)
                                    Category.CheckCatalogResult = "H";
                            }
                        }
                    }
                    else
                              if (CurrentFFRecord["MON_SYS_ID"] != DBNull.Value)
                    {
                        if (MassFlowRt <= 0)
                            Category.CheckCatalogResult = "E";
                        else
                        {
                            if (SODMassCd != "3")
                                Category.SetCheckParameter("HFF_Calc_Mass_Oil_Rate", MassFlowRt, eParameterDataType.Decimal);
                            DataView SysFFRecs = (DataView)Category.GetCheckParameter("System_Fuel_Flow_Records_For_Hour").ParameterValue;
                            if (SysFFRecs.Count != 1)
                                Category.CheckCatalogResult = "I";
                            else
                            {
                                decimal MaxFFRate = cDBConvert.ToDecimal(SysFFRecs[0]["MAX_RATE"]);
                                if (MaxFFRate <= 0 || cDBConvert.ToString(SysFFRecs[0]["SYS_FUEL_UOM_CD"]) != "LBHR")
                                    Category.CheckCatalogResult = "J";
                                else
                                  if (SODMassCd == "3")
                                {
                                    Category.SetCheckParameter("HFF_Calc_Mass_Oil_Rate", MaxFFRate, eParameterDataType.Decimal);
                                    if (MassFlowRt != MaxFFRate)
                                        Category.CheckCatalogResult = "K";
                                }
                                else
                                    if (Convert.ToDecimal(Category.GetCheckParameter("HFF_Calc_Mass_Oil_Rate").ParameterValue) > MaxFFRate)
                                    Category.CheckCatalogResult = "L";
                            }
                        }
                    }
                    else
                    {
                        if (MassFlowRt <= 0)
                            Category.CheckCatalogResult = "E";
                        else
                            Category.SetCheckParameter("HFF_Calc_Mass_Oil_Rate", MassFlowRt, eParameterDataType.Decimal);
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURAD11");
            }

            return ReturnVal;
        }

        public static string HOURAD12(cCategory Category, ref bool Log)
        //Determine Density
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("HFF_Density", null, eParameterDataType.Decimal);
                Category.SetCheckParameter("Current_Density_Record", null, eParameterDataType.DataRowView);
                DataRowView CurrentFFRecord = (DataRowView)Category.GetCheckParameter("Current_Fuel_Flow_Record").ParameterValue;
                sFilterPair[] ParamFFRecsFilter = new sFilterPair[1];
                DataView ParamFFRecords = (DataView)Category.GetCheckParameter("Hourly_Param_Fuel_Flow_Records_For_Current_Fuel_Flow").ParameterValue;
                ParamFFRecsFilter[0].Set("PARAMETER_CD", "DENSOIL");
                ParamFFRecords = FindRows(ParamFFRecords, ParamFFRecsFilter);
                if (ParamFFRecords.Count > 1)
                    Category.CheckCatalogResult = "A";
                else
                {
                    string SODMassCd = cDBConvert.ToString(CurrentFFRecord["SOD_MASS_CD"]);
                    if (ParamFFRecords.Count == 0)
                    {
                        if (Convert.ToString(Category.GetCheckParameter("HFF_System_Type").ParameterValue) == "OILV" && SODMassCd == "2")
                            Category.CheckCatalogResult = "B";
                    }
                    else
                    if (Convert.ToString(Category.GetCheckParameter("HFF_System_Type").ParameterValue) == "OILV" && SODMassCd == "2")
                    {
                        Category.SetCheckParameter("Current_Density_Record", ParamFFRecords[0], eParameterDataType.DataRowView);
                        string DensityUOM = cDBConvert.ToString(ParamFFRecords[0]["PARAMETER_UOM_CD"]);
                        string VolUOMCd = cDBConvert.ToString(CurrentFFRecord["VOLUMETRIC_UOM_CD"]);
                        if (!DensityUOM.InList("LBGAL,LBBBL,LBM3,LBSCF"))
                            Category.CheckCatalogResult = "C";
                        else
                          if (VolUOMCd == "GALHR" && DensityUOM != "LBGAL")
                            Category.CheckCatalogResult = "D";
                        else
                            if (VolUOMCd == "BBLHR" && DensityUOM != "LBBBL")
                            Category.CheckCatalogResult = "D";
                        else
                              if (VolUOMCd == "M3HR" && DensityUOM != "LBM3")
                            Category.CheckCatalogResult = "D";
                        else
                                if (VolUOMCd == "SCFH" && DensityUOM != "LBSCF")
                            Category.CheckCatalogResult = "D";
                        else
                        {
                            decimal ParamValFuel = cDBConvert.ToDecimal(ParamFFRecords[0]["PARAM_VAL_FUEL"]);
                            if (ParamValFuel > 0)
                            {
                                decimal DensityDef = decimal.MinValue;
                                string FuelCd = cDBConvert.ToString(CurrentFFRecord["FUEL_CD"]);
                                if (cDBConvert.ToString(ParamFFRecords[0]["SAMPLE_TYPE_CD"]) == "8")
                                {
                                    DataView D6Table = (DataView)Category.GetCheckParameter("Table_D-6_Missing_Data_Values").ParameterValue;
                                    sFilterPair[] D6Filter = new sFilterPair[2];
                                    D6Filter[0].Set("Parameter", "DENSOIL - " + DensityUOM);
                                    D6Filter[1].Set("FuelCode", FuelCd);
                                    DataRowView D6Row;
                                    if (FindRow(D6Table, D6Filter, out D6Row))
                                        DensityDef = cDBConvert.ToDecimal(D6Row["MissingDataValue"]);
                                }
                                if (DensityDef == decimal.MinValue)
                                {
                                    DataView FuelWarningTbl = (DataView)Category.GetCheckParameter("Fuel_Type_Warning_Levels_For_Density_Cross_Check_Table").ParameterValue;
                                    DataView FuelRealityTbl = (DataView)Category.GetCheckParameter("Fuel_Type_Reality_Checks_For_Density_Cross_Check_Table").ParameterValue;
                                    sFilterPair[] FuelFilter = new sFilterPair[1];
                                    FuelFilter[0].Set("Fuel Code - Units Of Measure", FuelCd + " - " + DensityUOM);
                                    DataRowView FuelWarnRow;
                                    DataRowView FuelRealRow;
                                    decimal MaxExp = decimal.MinValue;
                                    decimal MinExp = decimal.MinValue;
                                    decimal MaxAllow = decimal.MinValue;
                                    decimal MinAllow = decimal.MinValue;
                                    if (FindRow(FuelWarningTbl, FuelFilter, out FuelWarnRow))
                                    {
                                        MaxExp = cDBConvert.ToDecimal(FuelWarnRow["Upper Value"]);
                                        MinExp = cDBConvert.ToDecimal(FuelWarnRow["Lower Value"]);
                                    }
                                    if (FindRow(FuelRealityTbl, FuelFilter, out FuelRealRow))
                                    {
                                        MaxAllow = cDBConvert.ToDecimal(FuelRealRow["Upper Value"]);
                                        MinAllow = cDBConvert.ToDecimal(FuelRealRow["Lower Value"]);
                                    }
                                    if ((MaxAllow != decimal.MinValue && ParamValFuel > MaxAllow) || (MinAllow != decimal.MinValue && ParamValFuel < MinAllow))
                                        Category.CheckCatalogResult = "E";
                                    else
                                    {
                                        Category.SetCheckParameter("HFF_Density", ParamValFuel, eParameterDataType.Decimal);
                                        if ((MinExp != decimal.MinValue && ParamValFuel < MinExp) || (MaxExp != decimal.MinValue && ParamValFuel > MaxExp))
                                            Category.CheckCatalogResult = "F";
                                    }
                                }
                                else
                                  if (DensityDef == ParamValFuel)
                                    Category.SetCheckParameter("HFF_Density", ParamValFuel, eParameterDataType.Decimal);
                                else
                                    Category.CheckCatalogResult = "G";
                            }
                            else
                                Category.CheckCatalogResult = "H";
                        }
                    }
                    else
                        Category.CheckCatalogResult = "I";
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURAD12");
            }

            return ReturnVal;
        }

        public static string HOURAD13(cCategory Category, ref bool Log)
        //Check Density Sample Type
        {
            string ReturnVal = "";

            try
            {
                if (Category.GetCheckParameter("Current_Density_Record").ParameterValue != null)
                    if (!cDBConvert.ToString(Category.GetCheckParameter("Current_Density_Record").ValueAsDataRowView()["SAMPLE_TYPE_CD"]).InList("1,2,5,6,7,8"))
                        Category.CheckCatalogResult = "A";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURAD13");
            }

            return ReturnVal;
        }

        public static string HOURAD14(cCategory Category, ref bool Log)
        //Check Extraneous Density Record Fields
        {
            string ReturnVal = "";

            try
            {
                string HrlyExtFields = "";
                if (Category.GetCheckParameter("Current_Density_Record").ParameterValue != null)
                {
                    DataRowView CurrentDensityRec = (DataRowView)Category.GetCheckParameter("Current_Density_Record").ParameterValue;
                    if (CurrentDensityRec["FORMULA_IDENTIFIER"] != DBNull.Value)
                        HrlyExtFields = HrlyExtFields.ListAdd("FormulaIdentifier");
                    if (CurrentDensityRec["MON_SYS_ID"] != DBNull.Value)
                        HrlyExtFields = HrlyExtFields.ListAdd("MonitoringSystemID");
                    if (CurrentDensityRec["SEGMENT_NUM"] != DBNull.Value)
                        HrlyExtFields = HrlyExtFields.ListAdd("SegmentNumber");
                    if (CurrentDensityRec["OPERATING_CONDITION_CD"] != DBNull.Value)
                        HrlyExtFields = HrlyExtFields.ListAdd("OperatingConditionCode");
                    if (HrlyExtFields != "")
                    {
                        Category.SetCheckParameter("Hourly_Extraneous_Fields", HrlyExtFields.FormatList(), eParameterDataType.String);
                        Category.CheckCatalogResult = "A";
                    }
                    else
                        Category.SetCheckParameter("Hourly_Extraneous_Fields", null, eParameterDataType.String);
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURAD14");
            }

            return ReturnVal;
        }

        public static string HOURAD15(cCategory Category, ref bool Log)
        //Calculate Mass Oil Flow
        {
            string ReturnVal = "";

            try
            {
                decimal HFFCalcVolRate = cDBConvert.ToDecimal(Category.GetCheckParameter("HFF_Calc_Volumetric_Rate").ParameterValue);
                decimal HFFDensity = cDBConvert.ToDecimal(Category.GetCheckParameter("HFF_Density").ParameterValue);
                if (HFFCalcVolRate != decimal.MinValue && HFFDensity != decimal.MinValue)
                {
                    decimal HFFMassOilCalc = Math.Round(HFFCalcVolRate * HFFDensity, 1, MidpointRounding.AwayFromZero);
                    Category.SetCheckParameter("HFF_Calc_Mass_Oil_Rate", HFFMassOilCalc, eParameterDataType.Decimal);
                    decimal Tolerance = GetHourlyEmissionsTolerance("OILM", "LBHR", Category);
                    DataRowView CurrentFFRecord = (DataRowView)Category.GetCheckParameter("Current_Fuel_Flow_Record").ParameterValue;
                    decimal MassFlowRate = cDBConvert.ToDecimal(CurrentFFRecord["MASS_FLOW_RATE"]);
                    if (MassFlowRate > 0)
                        if (Math.Abs(MassFlowRate - HFFMassOilCalc) > Tolerance)
                            Category.CheckCatalogResult = "A";
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURAD15");
            }

            return ReturnVal;
        }

        public static string HOURAD16(cCategory Category, ref bool Log)
        //Determine GCV
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("HFF_GCV", null, eParameterDataType.Decimal);
                Category.SetCheckParameter("Current_GCV_Record", null, eParameterDataType.DataRowView);
                DataRowView CurrentFFRecord = (DataRowView)Category.GetCheckParameter("Current_Fuel_Flow_Record").ParameterValue;
                sFilterPair[] ParamFFRecsFilter = new sFilterPair[1];
                DataView ParamFFRecords = (DataView)Category.GetCheckParameter("Hourly_Param_Fuel_Flow_Records_For_Current_Fuel_Flow").ParameterValue;
                ParamFFRecsFilter[0].Set("PARAMETER_CD", "GCV");
                ParamFFRecords = FindRows(ParamFFRecords, ParamFFRecsFilter);
                if (ParamFFRecords.Count > 1)
                    Category.CheckCatalogResult = "A";
                else
                  if (ParamFFRecords.Count == 0)
                {
                    if (Category.GetCheckParameter("Current_HI_HPFF_Record").ParameterValue != null)
                        Category.CheckCatalogResult = "B";
                }
                else
                    if (Category.GetCheckParameter("Current_HI_HPFF_Record").ParameterValue != null)
                {
                    Category.SetCheckParameter("Current_GCV_Record", ParamFFRecords[0], eParameterDataType.DataRowView);
                    string GCVUOM = cDBConvert.ToString(ParamFFRecords[0]["PARAMETER_UOM_CD"]);
                    string FuelGrp = Convert.ToString(Category.GetCheckParameter("Current_Fuel_Group").ParameterValue);
                    string VolUOMCd = cDBConvert.ToString(CurrentFFRecord["VOLUMETRIC_UOM_CD"]);
                    if (!GCVUOM.InList("BTUGAL,BTUBBL,BTUM3,BTUSCF,BTULB,BTUHSCF"))
                        Category.CheckCatalogResult = "C";
                    else
                      if (FuelGrp == "GAS" && GCVUOM != "BTUHSCF")
                        Category.CheckCatalogResult = "D";
                    else
                        if (FuelGrp == "OIL" && CurrentFFRecord["MASS_FLOW_RATE"] != DBNull.Value && GCVUOM != "BTULB")
                        Category.CheckCatalogResult = "D";
                    else
                          if (FuelGrp == "OIL" && CurrentFFRecord["MASS_FLOW_RATE"] == DBNull.Value && VolUOMCd == "GALHR" && GCVUOM != "BTUGAL")
                        Category.CheckCatalogResult = "D";
                    else
                            if (FuelGrp == "OIL" && CurrentFFRecord["MASS_FLOW_RATE"] == DBNull.Value && VolUOMCd == "BBLHR" && GCVUOM != "BTUBBL")
                        Category.CheckCatalogResult = "D";
                    else
                              if (FuelGrp == "OIL" && CurrentFFRecord["MASS_FLOW_RATE"] == DBNull.Value && VolUOMCd == "M3HR" && GCVUOM != "BTUM3")
                        Category.CheckCatalogResult = "D";
                    else
                                if (FuelGrp == "OIL" && CurrentFFRecord["MASS_FLOW_RATE"] == DBNull.Value && VolUOMCd == "SCFH" && GCVUOM != "BTUSCF")
                        Category.CheckCatalogResult = "D";
                    else
                    {
                        decimal ParamValFuel = cDBConvert.ToDecimal(ParamFFRecords[0]["PARAM_VAL_FUEL"]);
                        if (ParamValFuel > 0)
                        {
                            decimal GCVDef = decimal.MinValue;
                            string FuelCd = cDBConvert.ToString(CurrentFFRecord["FUEL_CD"]);
                            if (cDBConvert.ToString(ParamFFRecords[0]["SAMPLE_TYPE_CD"]) == "8")
                            {
                                DataView D6Table = (DataView)Category.GetCheckParameter("Table_D-6_Missing_Data_Values").ParameterValue;
                                sFilterPair[] D6Filter = new sFilterPair[2];
                                D6Filter[0].Set("Parameter", "GCV - " + GCVUOM);
                                D6Filter[1].Set("FuelCode", FuelCd);
                                DataRowView D6Row;
                                if (FindRow(D6Table, D6Filter, out D6Row))
                                    GCVDef = cDBConvert.ToDecimal(D6Row["MissingDataValue"]);
                                else
                                {
                                    D6Filter = new sFilterPair[2];
                                    D6Filter[0].Set("Parameter", "GCV - " + GCVUOM);
                                    D6Filter[1].Set("FuelCode", DBNull.Value, eFilterDataType.String);
                                    if (FindRow(D6Table, D6Filter, out D6Row))
                                        GCVDef = cDBConvert.ToDecimal(D6Row["MissingDataValue"]);
                                }
                            }
                            if (GCVDef == decimal.MinValue)
                            {
                                DataView FuelWarningTbl = (DataView)Category.GetCheckParameter("Fuel_Type_Warning_Levels_For_GCV_Cross_Check_Table").ParameterValue;
                                DataView FuelRealityTbl = (DataView)Category.GetCheckParameter("Fuel_Type_Reality_Checks_For_GCV_Cross_Check_Table").ParameterValue;
                                sFilterPair[] FuelFilter = new sFilterPair[1];
                                FuelFilter[0].Set("Fuel Code - Units Of Measure", FuelCd + " - " + GCVUOM);
                                DataRowView FuelWarnRow;
                                DataRowView FuelRealRow;
                                decimal MaxExp = decimal.MinValue;
                                decimal MinExp = decimal.MinValue;
                                decimal MaxAllow = decimal.MinValue;
                                decimal MinAllow = decimal.MinValue;
                                if (FindRow(FuelWarningTbl, FuelFilter, out FuelWarnRow))
                                {
                                    MaxExp = cDBConvert.ToDecimal(FuelWarnRow["Upper Value"]);
                                    MinExp = cDBConvert.ToDecimal(FuelWarnRow["Lower Value"]);
                                }
                                if (FindRow(FuelRealityTbl, FuelFilter, out FuelRealRow))
                                {
                                    MaxAllow = cDBConvert.ToDecimal(FuelRealRow["Upper Value"]);
                                    MinAllow = cDBConvert.ToDecimal(FuelRealRow["Lower Value"]);
                                }
                                if ((MaxAllow != decimal.MinValue && ParamValFuel > MaxAllow) || (MinAllow != decimal.MinValue && ParamValFuel < MinAllow))
                                    Category.CheckCatalogResult = "E";
                                else
                                  if (ParamValFuel != Math.Round(ParamValFuel, 1, MidpointRounding.AwayFromZero) && ParamValFuel != decimal.MinValue)
                                    Category.CheckCatalogResult = "J";
                                else
                                {
                                    Category.SetCheckParameter("HFF_GCV", ParamValFuel, eParameterDataType.Decimal);
                                    if ((MinExp != decimal.MinValue && ParamValFuel < MinExp) || (MaxExp != decimal.MinValue && ParamValFuel > MaxExp))
                                        Category.CheckCatalogResult = "F";
                                }
                            }
                            else
                              if (ParamValFuel != Math.Round(ParamValFuel, 1, MidpointRounding.AwayFromZero) && ParamValFuel != decimal.MinValue)
                                Category.CheckCatalogResult = "J";
                            else
                                if (GCVDef == ParamValFuel)
                                Category.SetCheckParameter("HFF_GCV", ParamValFuel, eParameterDataType.Decimal);
                            else
                                Category.CheckCatalogResult = "G";
                        }
                        else
                            Category.CheckCatalogResult = "H";
                    }
                }
                else
                    Category.CheckCatalogResult = "I";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURAD16");
            }

            return ReturnVal;
        }

        public static string HOURAD17(cCategory Category, ref bool Log)
        //Check GCV Sample Type
        {
            string ReturnVal = "";

            try
            {
                if (Category.GetCheckParameter("Current_GCV_Record").ParameterValue != null)
                {
                    DataRowView GCVRec = Category.GetCheckParameter("Current_GCV_Record").ValueAsDataRowView();
                    string SampleTypeCd = cDBConvert.ToString(GCVRec["SAMPLE_TYPE_CD"]);
                    string FuelGrp = Convert.ToString(Category.GetCheckParameter("Current_Fuel_Group").ParameterValue);
                    if (FuelGrp == "OIL" && !SampleTypeCd.InList("1,2,5,6,7,8"))
                        Category.CheckCatalogResult = "A";
                    else
                        if (FuelGrp == "GAS" && !SampleTypeCd.InList("0,2,3,4,6,7,8"))
                        Category.CheckCatalogResult = "A";
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURAD17");
            }

            return ReturnVal;
        }

        public static string HOURAD18(cCategory Category, ref bool Log)
        //Check Extraneous GCV Record Fields
        {
            string ReturnVal = "";

            try
            {
                string HrlyExtFields = "";
                if (Category.GetCheckParameter("Current_GCV_Record").ParameterValue != null)
                {
                    DataRowView CurrentGCVRec = (DataRowView)Category.GetCheckParameter("Current_GCV_Record").ParameterValue;
                    if (CurrentGCVRec["FORMULA_IDENTIFIER"] != DBNull.Value)
                        HrlyExtFields = HrlyExtFields.ListAdd("FormulaIdentifier");
                    if (CurrentGCVRec["MON_SYS_ID"] != DBNull.Value)
                        HrlyExtFields = HrlyExtFields.ListAdd("MonitoringSystemID");
                    if (CurrentGCVRec["SEGMENT_NUM"] != DBNull.Value)
                        HrlyExtFields = HrlyExtFields.ListAdd("SegmentNumber");
                    if (CurrentGCVRec["OPERATING_CONDITION_CD"] != DBNull.Value)
                        HrlyExtFields = HrlyExtFields.ListAdd("OperatingConditionCode");
                    if (HrlyExtFields != "")
                    {
                        Category.SetCheckParameter("Hourly_Extraneous_Fields", HrlyExtFields.FormatList(), eParameterDataType.String);
                        Category.CheckCatalogResult = "A";
                    }
                    else
                        Category.SetCheckParameter("Hourly_Extraneous_Fields", null, eParameterDataType.String);
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURAD18");
            }

            return ReturnVal;
        }

        public static string HOURAD19(cCategory Category, ref bool Log)
        //Validate Heat Input Record
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("Current_HI_HPFF_Record", null, eParameterDataType.DataRowView);
                DataRowView CurrentFFRecord = (DataRowView)Category.GetCheckParameter("Current_Fuel_Flow_Record").ParameterValue;
                sFilterPair[] ParamFFRecsFilter = new sFilterPair[1];
                DataView ParamFFRecords = (DataView)Category.GetCheckParameter("Hourly_Param_Fuel_Flow_Records_For_Current_Fuel_Flow").ParameterValue;
                ParamFFRecsFilter[0].Set("PARAMETER_CD", "HI");
                ParamFFRecords = FindRows(ParamFFRecords, ParamFFRecsFilter);
                if (ParamFFRecords.Count > 1)
                {
                    Category.SetCheckParameter("HI_App_D_Accumulator", -1, eParameterDataType.Decimal);
                    Category.CheckCatalogResult = "A";
                }
                else
                  if (ParamFFRecords.Count == 0)
                {
                    if (Convert.ToBoolean(Category.GetCheckParameter("Heat_Input_App_D_Method_Active_For_Hour").ParameterValue))
                    {
                        Category.SetCheckParameter("HI_App_D_Accumulator", -1, eParameterDataType.Decimal);
                        Category.CheckCatalogResult = "B";
                    }
                }
                else
                    if (Convert.ToBoolean(Category.GetCheckParameter("Heat_Input_App_D_Method_Active_For_Hour").ParameterValue))
                {
                    Category.SetCheckParameter("Current_HI_HPFF_Record", ParamFFRecords[0], eParameterDataType.DataRowView);
                    EmParameters.HiHpffExists = true;

                    string ParamFFRecMonFormId = cDBConvert.ToString(ParamFFRecords[0]["MON_FORM_ID"]);
                    if (ParamFFRecMonFormId == "")
                        Category.CheckCatalogResult = "C";
                    else
                    {
                        DataView MonFormRecs = (DataView)Category.GetCheckParameter("Monitor_Formula_Records_By_Hour_Location").ParameterValue;
                        sFilterPair[] Filter = new sFilterPair[1];
                        Filter[0].Set("MON_FORM_ID", ParamFFRecMonFormId);
                        DataRowView MonFormRec;
                        if (!FindRow(MonFormRecs, Filter, out MonFormRec))
                            Category.CheckCatalogResult = "D";
                        else
                          if (cDBConvert.ToString(MonFormRec["PARAMETER_CD"]) != "HI")
                            Category.CheckCatalogResult = "E";
                        else
                        {
                            string EquCd = cDBConvert.ToString(MonFormRec["EQUATION_CD"]);
                            if (Convert.ToString(Category.GetCheckParameter("Current_Fuel_Group").ParameterValue) == "GAS")
                            {
                                if (!EquCd.InList("D-6,F-20"))
                                    Category.CheckCatalogResult = "F";
                            }
                            else
                            if (CurrentFFRecord["MASS_FLOW_RATE"] != DBNull.Value)
                            {
                                if (!EquCd.InList("D-8,F-19"))
                                    Category.CheckCatalogResult = "F";
                            }
                            else
                              if (EquCd != "F-19V")
                                Category.CheckCatalogResult = "F";
                        }
                    }
                }
                else
                    Category.CheckCatalogResult = "G";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURAD19");
            }

            return ReturnVal;
        }

        public static string HOURAD20(cCategory Category, ref bool Log)
        //Check Extraneous Heat Input Record Fields
        {
            string ReturnVal = "";

            try
            {
                string HrlyExtFields = "";
                if (Category.GetCheckParameter("Current_HI_HPFF_Record ").ParameterValue != null)
                {
                    DataRowView CurrentHIHPFFRec = (DataRowView)Category.GetCheckParameter("Current_HI_HPFF_Record").ParameterValue;
                    if (CurrentHIHPFFRec["SAMPLE_TYPE_CD"] != DBNull.Value)
                        HrlyExtFields = HrlyExtFields.ListAdd("SampleTypeCode");
                    if (CurrentHIHPFFRec["MON_SYS_ID"] != DBNull.Value)
                        HrlyExtFields = HrlyExtFields.ListAdd("MonitoringSystemID");
                    if (CurrentHIHPFFRec["SEGMENT_NUM"] != DBNull.Value)
                        HrlyExtFields = HrlyExtFields.ListAdd("SegmentNumber");
                    if (CurrentHIHPFFRec["OPERATING_CONDITION_CD"] != DBNull.Value)
                        HrlyExtFields = HrlyExtFields.ListAdd("OperatingConditionCode");
                    if (HrlyExtFields != "")
                    {
                        HrlyExtFields = HrlyExtFields.FormatList();
                        Category.SetCheckParameter("Hourly_Extraneous_Fields", HrlyExtFields.FormatList(), eParameterDataType.String);
                        Category.CheckCatalogResult = "A";
                    }
                    else
                        Category.SetCheckParameter("Hourly_Extraneous_Fields", null, eParameterDataType.String);
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURAD20");
            }

            return ReturnVal;
        }

        #endregion


        #region 21 - 30

        public static string HOURAD21(cCategory Category, ref bool Log)
        //Calculate Heat Input Rate
        {
            string ReturnVal = "";

            try
            {
                DataRowView HIHPFFRec = (DataRowView)Category.GetCheckParameter("Current_HI_HPFF_Record").ParameterValue;
                if (HIHPFFRec != null)
                {
                    decimal CalcHIRate = decimal.MinValue;
                    string HFFSysType = cDBConvert.ToString(Category.GetCheckParameter("HFF_System_Type").ParameterValue);
                    decimal HFFGCV = cDBConvert.ToDecimal(Category.GetCheckParameter("HFF_GCV").ParameterValue);
                    DataRowView CurrentFFRecord = (DataRowView)Category.GetCheckParameter("Current_Fuel_Flow_Record").ParameterValue;
                    string VolCd = cDBConvert.ToString(CurrentFFRecord["SOD_VOLUMETRIC_CD"]);
                    string MassCd = cDBConvert.ToString(CurrentFFRecord["SOD_MASS_CD"]);
                    decimal Tolerance1 = GetHourlyEmissionsTolerance("HI HPFF", "MMBTUHR", Category);
                    decimal CalcVolRt = cDBConvert.ToDecimal(Category.GetCheckParameter("HFF_Calc_Volumetric_Rate").ParameterValue);
                    decimal CalcMassOilRt = cDBConvert.ToDecimal(Category.GetCheckParameter("HFF_Calc_Mass_Oil_Rate").ParameterValue);

                    if (VolCd == "4")
                    {
                        decimal HFFMaxHIVol = Category.GetCheckParameter("HFF_Max_Heat_Input_for_Volume").ValueAsDecimal();
                        if (HFFMaxHIVol != decimal.MinValue)
                            CalcHIRate = HFFMaxHIVol;
                        else
                        {
                            Category.SetCheckParameter("HI_App_D_Accumulator", -1, eParameterDataType.Decimal);
                            Category.CheckCatalogResult = "A";
                        }
                    }
                    else
                      if (MassCd == "4")
                    {
                        decimal HFFMaxHIMass = Category.GetCheckParameter("HFF_Max_Heat_Input_for_Mass").ValueAsDecimal();
                        if (HFFMaxHIMass != decimal.MinValue)
                            CalcHIRate = HFFMaxHIMass;
                        else
                        {
                            Category.SetCheckParameter("HI_App_D_Accumulator", -1, eParameterDataType.Decimal);
                            Category.CheckCatalogResult = "A";
                        }
                    }
                    else
                        if (HFFGCV != decimal.MinValue)
                    {
                        if (Convert.ToString(Category.GetCheckParameter("HFF_System_Type").ParameterValue) == "GAS" || CurrentFFRecord["MASS_FLOW_RATE"] == DBNull.Value)
                            if (CalcVolRt != decimal.MinValue)
                                CalcHIRate = Math.Round(CalcVolRt * HFFGCV / 1000000, 1, MidpointRounding.AwayFromZero);
                            else
                            {
                                Category.SetCheckParameter("HI_App_D_Accumulator", -1, eParameterDataType.Decimal);
                                Category.CheckCatalogResult = "A";
                            }
                        else
                          if (CalcMassOilRt != decimal.MinValue)
                            CalcHIRate = Math.Round(CalcMassOilRt * HFFGCV / 1000000, 1, MidpointRounding.AwayFromZero);
                        else
                        {
                            Category.SetCheckParameter("HI_App_D_Accumulator", -1, eParameterDataType.Decimal);
                            Category.CheckCatalogResult = "A";
                        }
                    }
                    if (CalcHIRate != decimal.MinValue && string.IsNullOrEmpty(Category.CheckCatalogResult))
                    {
                        Category.SetCheckParameter("HFF_Calc_HI_Rate", CalcHIRate, eParameterDataType.Decimal);
                        decimal UsageTime = cDBConvert.ToDecimal(CurrentFFRecord["FUEL_USAGE_TIME"]);
                        decimal AppDAccum = Convert.ToDecimal(Category.GetCheckParameter("HI_App_D_Accumulator").ParameterValue);
                        if (UsageTime > 0 && UsageTime <= 1 && AppDAccum >= 0)
                            Category.AccumCheckAggregate("HI_App_D_Accumulator", UsageTime * CalcHIRate);
                        else
                            Category.SetCheckParameter("HI_App_D_Accumulator", -1, eParameterDataType.Decimal);
                        decimal ParamValFuel = cDBConvert.ToDecimal(HIHPFFRec["PARAM_VAL_FUEL"]);
                        if (ParamValFuel > 0)
                            if (VolCd == "4" || MassCd == "4")
                            {
                                if (CalcHIRate == ParamValFuel)
                                {
                                    decimal Tolerance2;
                                    if (VolCd == "4")
                                    {
                                        decimal VolFlowRt = cDBConvert.ToDecimal(CurrentFFRecord["VOLUMETRIC_FLOW_RATE"]);
                                        if (VolFlowRt > 0 && VolFlowRt != CalcVolRt)
                                        {
                                            Tolerance2 = GetHourlyEmissionsTolerance("FOIL", "", Category);
                                            if (Math.Abs(CalcVolRt - VolFlowRt) > Tolerance1)
                                                Category.CheckCatalogResult = "C";
                                        }
                                    }
                                    else
                                    {
                                        decimal MassFlowRt = cDBConvert.ToDecimal(CurrentFFRecord["MASS_FLOW_RATE"]);
                                        if (MassFlowRt > 0 && MassFlowRt != CalcMassOilRt)
                                        {
                                            Tolerance2 = GetHourlyEmissionsTolerance("FOIL", "", Category);
                                            if (Math.Abs(CalcMassOilRt - MassFlowRt) > Tolerance1)
                                                Category.CheckCatalogResult = "D";
                                        }
                                    }
                                }
                            }
                            else
                              if (Math.Abs(CalcHIRate - ParamValFuel) > Tolerance1)
                                Category.CheckCatalogResult = "B";
                    }
                    else
                    {
                        Category.SetCheckParameter("HI_App_D_Accumulator", -1, eParameterDataType.Decimal);
                        Category.CheckCatalogResult = "A";
                    }

                    if (CalcHIRate == decimal.MinValue)
                        Category.SetCheckParameter("HFF_Calc_HI_Rate", null, eParameterDataType.Decimal);
                    else
                        Category.SetCheckParameter("HFF_Calc_HI_Rate", CalcHIRate, eParameterDataType.Decimal);
                }
                else
                    Category.SetCheckParameter("HFF_Calc_HI_Rate", null, eParameterDataType.Decimal);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURAD21");
            }

            return ReturnVal;
        }

        public static string HOURAD22(cCategory Category, ref bool Log)
        //Check Reported Heat Input
        {
            string ReturnVal = "";

            try
            {
                if (Category.GetCheckParameter("Current_HI_HPFF_Record").ParameterValue != null)
                {
                    DataRowView HIHPFFRec = (DataRowView)Category.GetCheckParameter("Current_HI_HPFF_Record").ParameterValue;
                    decimal ParamValFuel = cDBConvert.ToDecimal(HIHPFFRec["PARAM_VAL_FUEL"]);
                    DataView UnitCapRecs = (DataView)Category.GetCheckParameter("Location_Capacity_Records_for_Hour_Location").ParameterValue;
                    decimal MaxHI = 0;
                    if (ParamValFuel >= 0)
                        if (ParamValFuel != Math.Round(ParamValFuel, 1, MidpointRounding.AwayFromZero) && ParamValFuel != decimal.MinValue)
                            Category.CheckCatalogResult = "D";
                        else
                        {
                            DataRowView CurrentFFRecord = (DataRowView)Category.GetCheckParameter("Current_Fuel_Flow_Record").ParameterValue;
                            string VolCd = cDBConvert.ToString(CurrentFFRecord["SOD_VOLUMETRIC_CD"]);
                            string MassCd = cDBConvert.ToString(CurrentFFRecord["SOD_MASS_CD"]);
                            decimal HFFMaxHIVol = Category.GetCheckParameter("HFF_Max_Heat_Input_for_Volume").ValueAsDecimal();
                            if (VolCd == "4" && HFFMaxHIVol != decimal.MinValue)
                            {
                                if (ParamValFuel != HFFMaxHIVol)
                                    Category.CheckCatalogResult = "E";
                            }
                            else
                            {
                                decimal HFFMaxHIMass = Category.GetCheckParameter("HFF_Max_Heat_Input_for_Mass").ValueAsDecimal();
                                if (MassCd == "4" && HFFMaxHIMass != decimal.MinValue)
                                {
                                    if (ParamValFuel != HFFMaxHIMass)
                                        Category.CheckCatalogResult = "F";
                                }
                                else
                                {
                                    if (Convert.ToString(Category.GetCheckParameter("Current_Entity_Type").ParameterValue) == "CP")
                                    {
                                        DataView UnitStackConfigRecs = (DataView)Category.GetCheckParameter("Unit_Stack_Configuration_Records_By_Hour_Location").ParameterValue;
                                        sFilterPair[] Filter = new sFilterPair[1];
                                        DataView UnitCapRecsFiltered;
                                        foreach (DataRowView drv in UnitCapRecs)
                                        {
                                            Filter[0].Set("UNIT_ID", cDBConvert.ToString(drv["UNIT_ID"]));
                                            UnitCapRecsFiltered = FindRows(UnitCapRecs, Filter);
                                            if (UnitCapRecsFiltered.Count != 1)
                                            {
                                                Category.CheckCatalogResult = "A";
                                                break;
                                            }
                                            else
                                                MaxHI += cDBConvert.ToDecimal(drv["MAX_HI_CAPACITY"]);
                                        }
                                    }
                                    else
                                    {
                                        if (UnitCapRecs.Count != 1)
                                            Category.CheckCatalogResult = "A";
                                        else
                                            MaxHI = cDBConvert.ToDecimal(UnitCapRecs[0]["MAX_HI_CAPACITY"]);
                                    }
                                    if (ParamValFuel > MaxHI && string.IsNullOrEmpty(Category.CheckCatalogResult))
                                        Category.CheckCatalogResult = "B";
                                }
                            }
                        }
                    else
                        Category.CheckCatalogResult = "C";
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURAD22");
            }

            return ReturnVal;
        }

        public static string HOURAD23(cCategory Category, ref bool Log)
        //Check Heat Input Units Of Measure
        {
            string ReturnVal = "";

            try
            {
                if (Category.GetCheckParameter("Current_HI_HPFF_Record").ParameterValue != null)
                    if (cDBConvert.ToString(Category.GetCheckParameter("Current_HI_HPFF_Record").ValueAsDataRowView()["PARAMETER_UOM_CD"]) != "MMBTUHR")
                        Category.CheckCatalogResult = "A";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURAD23");
            }

            return ReturnVal;
        }

        public static string HOURAD24(cCategory Category, ref bool Log)
        //Validate SO2 Record
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("Current_SO2_HPFF_Record", null, eParameterDataType.DataRowView);
                Category.SetCheckParameter("HFF_SO2_Equation_Code", null, eParameterDataType.String);
                DataRowView CurrentFFRecord = (DataRowView)Category.GetCheckParameter("Current_Fuel_Flow_Record").ParameterValue;
                sFilterPair[] ParamFFRecsFilter = new sFilterPair[1];
                DataView ParamFFRecords = (DataView)Category.GetCheckParameter("Hourly_Param_Fuel_Flow_Records_For_Current_Fuel_Flow").ParameterValue;
                ParamFFRecsFilter[0].Set("PARAMETER_CD", "SO2");
                ParamFFRecords = FindRows(ParamFFRecords, ParamFFRecsFilter);
                if (ParamFFRecords.Count > 1)
                {
                    Category.SetCheckParameter("SO2_App_D_Accumulator", -1, eParameterDataType.Decimal);
                    Category.CheckCatalogResult = "A";
                }
                else
                  if (ParamFFRecords.Count == 0)
                {
                    if (Convert.ToBoolean(Category.GetCheckParameter("SO2_App_D_Method_Active_For_Hour").ParameterValue))
                    {
                        Category.SetCheckParameter("SO2_App_D_Accumulator", -1, eParameterDataType.Decimal);
                        Category.CheckCatalogResult = "B";
                    }
                }
                else
                    if (Convert.ToBoolean(Category.GetCheckParameter("SO2_App_D_Method_Active_For_Hour").ParameterValue))
                {
                    Category.SetCheckParameter("Current_SO2_HPFF_Record", ParamFFRecords[0], eParameterDataType.DataRowView);
                    EmParameters.So2HpffExists = true;

                    string ParamFFRecMonFormId = cDBConvert.ToString(ParamFFRecords[0]["MON_FORM_ID"]);
                    if (ParamFFRecMonFormId == "")
                        Category.CheckCatalogResult = "C";
                    else
                    {
                        DataView MonFormRecs = (DataView)Category.GetCheckParameter("Monitor_Formula_Records_By_Hour_Location").ParameterValue;
                        sFilterPair[] MonFormFilter = new sFilterPair[1];
                        MonFormFilter[0].Set("MON_FORM_ID", ParamFFRecMonFormId);
                        DataRowView MonFormRec;
                        if (!FindRow(MonFormRecs, MonFormFilter, out MonFormRec))
                            Category.CheckCatalogResult = "D";
                        else
                          if (cDBConvert.ToString(MonFormRec["PARAMETER_CD"]) != "SO2")
                            Category.CheckCatalogResult = "E";
                        else
                        {
                            string EquCd = cDBConvert.ToString(MonFormRec["EQUATION_CD"]);
                            if (Convert.ToString(Category.GetCheckParameter("Current_Fuel_Group").ParameterValue) == "GAS")
                                if (EquCd.InList("D-4,D-5"))
                                    Category.SetCheckParameter("HFF_SO2_Equation_Code", EquCd, eParameterDataType.String);
                                else
                                    Category.CheckCatalogResult = "F";
                            else
                              if (EquCd == "D-2")
                                Category.SetCheckParameter("HFF_SO2_Equation_Code", EquCd, eParameterDataType.String);
                            else
                                Category.CheckCatalogResult = "F";
                        }
                    }
                }
                else
                    Category.CheckCatalogResult = "G";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURAD24");
            }

            return ReturnVal;
        }

        public static string HOURAD25(cCategory Category, ref bool Log)
        //Check Extraneous SO2 Record Fields
        {
            string ReturnVal = "";

            try
            {
                string HrlyExtFields = "";
                if (Category.GetCheckParameter("Current_SO2_HPFF_Record ").ParameterValue != null)
                {
                    DataRowView CurrentSO2HPFFRec = (DataRowView)Category.GetCheckParameter("Current_SO2_HPFF_Record").ParameterValue;
                    if (CurrentSO2HPFFRec["SAMPLE_TYPE_CD"] != DBNull.Value)
                        HrlyExtFields = HrlyExtFields.ListAdd("SampleTypeCode");
                    if (CurrentSO2HPFFRec["MON_SYS_ID"] != DBNull.Value)
                        HrlyExtFields = HrlyExtFields.ListAdd("MonitoringSystemID");
                    if (CurrentSO2HPFFRec["SEGMENT_NUM"] != DBNull.Value)
                        HrlyExtFields = HrlyExtFields.ListAdd("SegmentNumber");
                    if (CurrentSO2HPFFRec["OPERATING_CONDITION_CD"] != DBNull.Value)
                        HrlyExtFields = HrlyExtFields.ListAdd("OperatingConditionCode");
                    if (HrlyExtFields != "")
                    {
                        HrlyExtFields = HrlyExtFields.FormatList();
                        Category.SetCheckParameter("Hourly_Extraneous_Fields", HrlyExtFields.FormatList(), eParameterDataType.String);
                        Category.CheckCatalogResult = "A";
                    }
                    else
                        Category.SetCheckParameter("Hourly_Extraneous_Fields", null, eParameterDataType.String);
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURAD25");
            }

            return ReturnVal;
        }

        public static string HOURAD26(cCategory Category, ref bool Log)
        //Check SO2 Units Of Measure
        {
            string ReturnVal = "";

            try
            {
                if (Category.GetCheckParameter("Current_SO2_HPFF_Record").ParameterValue != null)
                    if (cDBConvert.ToString(Category.GetCheckParameter("Current_SO2_HPFF_Record").ValueAsDataRowView()["PARAMETER_UOM_CD"]) != "LBHR")
                        Category.CheckCatalogResult = "A";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURAD26");
            }

            return ReturnVal;
        }

        public static string HOURAD27(cCategory category, ref bool log)
        //Calculate SO2 Mass Rate
        {
            string ReturnVal = "";

            try
            {
                decimal? hffCalcSo2 = null;
                category.SetCheckParameter("HFF_Calc_SO2", hffCalcSo2, eParameterDataType.Decimal);

                DataRowView currentSo2HpffRecord = category.GetCheckParameter("Current_SO2_HPFF_Record").ValueAsDataRowView();

                if (currentSo2HpffRecord != null)
                {
                    decimal? tolerance = null;

                    string currentFuelGroup = category.GetCheckParameter("Current_Fuel_Group").AsString();
                    string hffSo2EquationCode = category.GetCheckParameter("HFF_SO2_Equation_Code").AsString();

                    if ((currentFuelGroup == "GAS") && (hffSo2EquationCode == "D-4"))
                    {
                        decimal? hffSulfur = category.GetCheckParameter("HFF_Sulfur").AsDecimal();
                        decimal? hffCalcVolumetricRate = category.GetCheckParameter("HFF_Calc_Volumetric_Rate").AsDecimal();

                        if (hffSulfur.HasValue && hffCalcVolumetricRate.HasValue)
                        {
                            tolerance = GetHourlyEmissionsTolerance("SO2 Gas HPFF", "LBHR", category);
                            hffCalcSo2 = Math.Round(hffSulfur.Value * hffCalcVolumetricRate.Value * 2 / 7000, 5, MidpointRounding.AwayFromZero);
                            category.SetCheckParameter("HFF_Calc_SO2", hffCalcSo2, eParameterDataType.Decimal);
                        }
                    }
                    else if ((currentFuelGroup == "GAS") && (hffSo2EquationCode == "D-5"))
                    {
                        decimal? hffSo2EmissionRate = category.GetCheckParameter("HFF_SO2_Emission_Rate").AsDecimal();
                        decimal? hffCalcHiRate = category.GetCheckParameter("HFF_Calc_HI_Rate").AsDecimal();

                        if (hffSo2EmissionRate.HasValue && hffCalcHiRate.HasValue)
                        {
                            tolerance = GetHourlyEmissionsTolerance("SO2 Gas HPFF", "LBHR", category);
                            hffCalcSo2 = Math.Round(hffSo2EmissionRate.Value * hffCalcHiRate.Value, 5, MidpointRounding.AwayFromZero);
                            category.SetCheckParameter("HFF_Calc_SO2", hffCalcSo2, eParameterDataType.Decimal);
                        }
                    }
                    else if (currentFuelGroup == "OIL")
                    {
                        decimal? hffSulfur = category.GetCheckParameter("HFF_Sulfur").AsDecimal();
                        decimal? hffCalcMassOilRate = category.GetCheckParameter("HFF_Calc_Mass_Oil_Rate").AsDecimal();

                        if (hffSulfur.HasValue && hffCalcMassOilRate.HasValue)
                        {
                            tolerance = GetHourlyEmissionsTolerance("SO2 Oil HPFF", "LBHR", category);
                            hffCalcSo2 = Math.Round(hffSulfur.Value * hffCalcMassOilRate.Value * 2 / 100, 1, MidpointRounding.AwayFromZero);
                            category.SetCheckParameter("HFF_Calc_SO2", hffCalcSo2, eParameterDataType.Decimal);
                        }
                    }

                    if (hffCalcSo2.HasValue)
                    {
                        DataRowView currentFuelFlowRecord = (DataRowView)category.GetCheckParameter("Current_Fuel_Flow_Record").ParameterValue;
                        decimal so2AppdAccumulator = category.GetCheckParameter("SO2_App_D_Accumulator").AsDecimal().Default();

                        decimal fuelUsageTime = currentFuelFlowRecord["FUEL_USAGE_TIME"].AsDecimal().Default();

                        if (((0 < fuelUsageTime) && (fuelUsageTime <= 1)) && (so2AppdAccumulator >= 0))
                            category.AccumCheckAggregate("SO2_App_D_Accumulator", fuelUsageTime * hffCalcSo2.Value);
                        else
                            category.SetCheckParameter("SO2_App_D_Accumulator", -1, eParameterDataType.Decimal);

                        decimal paramValFuel = currentSo2HpffRecord["PARAM_VAL_FUEL"].AsDecimal().Default();

                        if (paramValFuel >= 0)
                            if (Math.Abs(hffCalcSo2.Value - paramValFuel) > tolerance.Value)
                                category.CheckCatalogResult = "A";
                    }
                    else
                    {
                        category.SetCheckParameter("SO2_App_D_Accumulator", -1, eParameterDataType.Decimal);
                        category.CheckCatalogResult = "B";
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = category.CheckEngine.FormatError(ex, "HOURAD27");
            }

            return ReturnVal;
        }

        public static string HOURAD28(cCategory Category, ref bool Log)
        //Determine Sulfur Content
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("HFF_Sulfur", null, eParameterDataType.Decimal);
                Category.SetCheckParameter("Current_Sulfur_Record", null, eParameterDataType.DataRowView);
                DataRowView CurrentFFRecord = (DataRowView)Category.GetCheckParameter("Current_Fuel_Flow_Record").ParameterValue;
                sFilterPair[] ParamFFRecsFilter = new sFilterPair[1];
                DataView ParamFFRecords = (DataView)Category.GetCheckParameter("Hourly_Param_Fuel_Flow_Records_For_Current_Fuel_Flow").ParameterValue;
                ParamFFRecsFilter[0].Set("PARAMETER_CD", "SULFUR");
                ParamFFRecords = FindRows(ParamFFRecords, ParamFFRecsFilter);
                if (ParamFFRecords.Count > 1)
                    Category.CheckCatalogResult = "A";
                else
                {
                    string EquCd = Convert.ToString(Category.GetCheckParameter("HFF_SO2_Equation_Code").ParameterValue);
                    if (ParamFFRecords.Count == 0)
                    {
                        if (EquCd.InList("D-2,D-4"))
                            Category.CheckCatalogResult = "B";
                    }
                    else
                        if (EquCd.InList("D-2,D-4"))
                    {
                        Category.SetCheckParameter("Current_Sulfur_Record", ParamFFRecords[0], eParameterDataType.DataRowView);
                        string SulfurUOM = cDBConvert.ToString(ParamFFRecords[0]["PARAMETER_UOM_CD"]);
                        string FuelGrp = Convert.ToString(Category.GetCheckParameter("Current_Fuel_Group").ParameterValue);
                        string VolUOMCd = cDBConvert.ToString(CurrentFFRecord["VOLUMETRIC_UOM_CD"]);
                        if (FuelGrp == "GAS" && SulfurUOM != "GRHSCF")
                            Category.CheckCatalogResult = "C";
                        else
                          if (FuelGrp == "OIL" && SulfurUOM != "PCT")
                            Category.CheckCatalogResult = "C";
                        else
                        {
                            decimal ParamValFuel = cDBConvert.ToDecimal(ParamFFRecords[0]["PARAM_VAL_FUEL"]);
                            if (ParamValFuel > 0)
                            {
                                int SulfurPrecision;
                                if (SulfurUOM == "GRHSCF")
                                    SulfurPrecision = 1;
                                else
                                    SulfurPrecision = 4;

                                decimal SulfurDef = decimal.MinValue;
                                string FuelCd = cDBConvert.ToString(CurrentFFRecord["FUEL_CD"]);
                                if (cDBConvert.ToString(ParamFFRecords[0]["SAMPLE_TYPE_CD"]) == "8")
                                {
                                    DataView D6Table = (DataView)Category.GetCheckParameter("Table_D-6_Missing_Data_Values").ParameterValue;
                                    sFilterPair[] D6Filter = new sFilterPair[2];
                                    D6Filter[0].Set("Parameter", "SULFUR");
                                    D6Filter[1].Set("FuelCode", FuelCd);
                                    DataRowView D6Row;
                                    if (FindRow(D6Table, D6Filter, out D6Row))
                                        SulfurDef = cDBConvert.ToDecimal(D6Row["MissingDataValue"]);
                                }
                                if (SulfurDef == decimal.MinValue)
                                {
                                    DataView FuelWarningTbl = (DataView)Category.GetCheckParameter("Fuel_Type_Warning_Levels_For_Sulfur_Content_Cross_Check_Table").ParameterValue;
                                    DataView FuelRealityTbl = (DataView)Category.GetCheckParameter("Fuel_Type_Reality_Checks_For_Sulfur_Content_Cross_Check_Table").ParameterValue;
                                    sFilterPair[] FuelFilter = new sFilterPair[1];
                                    FuelFilter[0].Set("FUEL CODE", FuelCd);
                                    DataRowView FuelWarnRow;
                                    DataRowView FuelRealRow;
                                    decimal MaxExp = decimal.MinValue;
                                    decimal MinExp = decimal.MinValue;
                                    decimal MaxAllow = decimal.MinValue;
                                    decimal MinAllow = decimal.MinValue;
                                    if (FindRow(FuelWarningTbl, FuelFilter, out FuelWarnRow))
                                    {
                                        MaxExp = cDBConvert.ToDecimal(FuelWarnRow["Upper Value"]);
                                        MinExp = cDBConvert.ToDecimal(FuelWarnRow["Lower Value"]);
                                    }
                                    if (FindRow(FuelRealityTbl, FuelFilter, out FuelRealRow))
                                    {
                                        MaxAllow = cDBConvert.ToDecimal(FuelRealRow["Upper Value"]);
                                        MinAllow = cDBConvert.ToDecimal(FuelRealRow["Lower Value"]);
                                    }
                                    if ((MaxAllow != decimal.MinValue && ParamValFuel > MaxAllow) || (MinAllow != decimal.MinValue && ParamValFuel < MinAllow))
                                        Category.CheckCatalogResult = "D";
                                    else
                                      if (ParamValFuel != Math.Round(ParamValFuel, SulfurPrecision, MidpointRounding.AwayFromZero) && ParamValFuel != decimal.MinValue)
                                        Category.CheckCatalogResult = "I";
                                    else
                                    {
                                        Category.SetCheckParameter("HFF_Sulfur", ParamValFuel, eParameterDataType.Decimal);
                                        if ((MinExp != decimal.MinValue && ParamValFuel < MinExp) || (MaxExp != decimal.MinValue && ParamValFuel > MaxExp))
                                            Category.CheckCatalogResult = "E";
                                    }
                                }
                                else
                                if (ParamValFuel != Math.Round(ParamValFuel, SulfurPrecision, MidpointRounding.AwayFromZero) && ParamValFuel != decimal.MinValue)
                                    Category.CheckCatalogResult = "I";
                                else
                                  if (SulfurDef == ParamValFuel)
                                    Category.SetCheckParameter("HFF_Sulfur", ParamValFuel, eParameterDataType.Decimal);
                                else
                                    Category.CheckCatalogResult = "F";
                            }
                            else
                                Category.CheckCatalogResult = "G";
                        }
                    }
                    else
                        Category.CheckCatalogResult = "H";
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURAD28");
            }

            return ReturnVal;
        }

        public static string HOURAD29(cCategory Category, ref bool Log)
        //Check Extraneous Sulfur Record Fields
        {
            string ReturnVal = "";

            try
            {
                string HrlyExtFields = "";
                if (Category.GetCheckParameter("Current_Sulfur_Record").ParameterValue != null)
                {
                    DataRowView CurrentSulfurRec = (DataRowView)Category.GetCheckParameter("Current_Sulfur_Record").ParameterValue;
                    if (CurrentSulfurRec["FORMULA_IDENTIFIER"] != DBNull.Value)
                        HrlyExtFields = HrlyExtFields.ListAdd("FormulaIdentifier");
                    if (CurrentSulfurRec["MON_SYS_ID"] != DBNull.Value)
                        HrlyExtFields = HrlyExtFields.ListAdd("MonitoringSystemID");
                    if (CurrentSulfurRec["SEGMENT_NUM"] != DBNull.Value)
                        HrlyExtFields = HrlyExtFields.ListAdd("SegmentNumber");
                    if (CurrentSulfurRec["OPERATING_CONDITION_CD"] != DBNull.Value)
                        HrlyExtFields = HrlyExtFields.ListAdd("OperatingConditionCode");
                    if (HrlyExtFields != "")
                    {
                        Category.SetCheckParameter("Hourly_Extraneous_Fields", HrlyExtFields.FormatList(), eParameterDataType.String);
                        Category.CheckCatalogResult = "A";
                    }
                    else
                        Category.SetCheckParameter("Hourly_Extraneous_Fields", null, eParameterDataType.String);
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURAD29");
            }

            return ReturnVal;
        }

        public static string HOURAD30(cCategory Category, ref bool Log)
        //Check Sulfur Sample Type
        {
            string ReturnVal = "";

            try
            {
                if (Category.GetCheckParameter("Current_Sulfur_Record").ParameterValue != null)
                {
                    string SampleTypeCd = cDBConvert.ToString(Category.GetCheckParameter("Current_Sulfur_Record").ValueAsDataRowView()["SAMPLE_TYPE_CD"]);
                    string FuelGrp = Convert.ToString(Category.GetCheckParameter("Current_Fuel_Group").ParameterValue);
                    if (FuelGrp == "OIL" && !SampleTypeCd.InList("1,2,5,6,7,8"))
                        Category.CheckCatalogResult = "A";
                    else
                        if (FuelGrp == "GAS" && !SampleTypeCd.InList("0,2,4,5,6,7,8"))
                        Category.CheckCatalogResult = "A";
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURAD30");
            }

            return ReturnVal;
        }

        #endregion


        #region 31 - 40

        public static string HOURAD31(cCategory Category, ref bool Log)
        //Determine SO2 Emission Rate
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("HFF_SO2_Emission_Rate", null, eParameterDataType.Decimal);
                Category.SetCheckParameter("Current_SO2R_Record", null, eParameterDataType.DataRowView);
                DataView ParamFFRecs = (DataView)Category.GetCheckParameter("Hourly_Param_Fuel_Flow_Records_For_Current_Fuel_Flow").ParameterValue;
                DataRowView CurrentFFRecord = (DataRowView)Category.GetCheckParameter("Current_Fuel_Flow_Record").ParameterValue;
                sFilterPair[] Filter = new sFilterPair[1];
                Filter[0].Set("PARAMETER_CD", "SO2R");
                ParamFFRecs = FindRows(ParamFFRecs, Filter);
                if (ParamFFRecs.Count > 1)
                    Category.CheckCatalogResult = "A";
                else
                {
                    string EquCd = Convert.ToString(Category.GetCheckParameter("HFF_SO2_Equation_Code").ParameterValue);
                    if (ParamFFRecs.Count == 0)
                    {
                        if (EquCd == "D-5")
                            Category.CheckCatalogResult = "B";
                    }
                    else
                      if (EquCd == "D-5")
                    {
                        Category.SetCheckParameter("Current_SO2R_Record", ParamFFRecs[0], eParameterDataType.DataRowView);
                        if (cDBConvert.ToString(ParamFFRecs[0]["PARAMETER_UOM_CD"]) != "LBMMBTU")
                            Category.CheckCatalogResult = "C";
                        else
                        {
                            decimal ParamValFuel = cDBConvert.ToDecimal(ParamFFRecs[0]["PARAM_VAL_FUEL"]);
                            if (ParamValFuel > 0)
                                Category.SetCheckParameter("HFF_SO2_Emission_Rate", ParamValFuel, eParameterDataType.Decimal);
                            else
                                Category.CheckCatalogResult = "D";
                        }
                    }
                    else
                        Category.CheckCatalogResult = "E";
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURAD31");
            }

            return ReturnVal;
        }

        public static string HOURAD32(cCategory Category, ref bool Log)
        //Check Extraneous SO2R Record Fields
        {
            string ReturnVal = "";

            try
            {
                string HrlyExtFields = "";
                if (Category.GetCheckParameter("Current_SO2R_Record ").ParameterValue != null)
                {
                    DataRowView CurrentSO2RRec = (DataRowView)Category.GetCheckParameter("Current_SO2R_Record").ParameterValue;
                    if (CurrentSO2RRec["SAMPLE_TYPE_CD"] != DBNull.Value)
                        HrlyExtFields = HrlyExtFields.ListAdd("SampleTypeCode");
                    if (CurrentSO2RRec["MON_SYS_ID"] != DBNull.Value)
                        HrlyExtFields = HrlyExtFields.ListAdd("MonitoringSystemID");
                    if (CurrentSO2RRec["SEGMENT_NUM"] != DBNull.Value)
                        HrlyExtFields = HrlyExtFields.ListAdd("SegmentNumber");
                    if (CurrentSO2RRec["OPERATING_CONDITION_CD"] != DBNull.Value)
                        HrlyExtFields = HrlyExtFields.ListAdd("OperatingConditionCode");
                    if (HrlyExtFields != "")
                    {
                        Category.SetCheckParameter("Hourly_Extraneous_Fields", HrlyExtFields.FormatList(), eParameterDataType.String);
                        Category.CheckCatalogResult = "A";
                    }
                    else
                        Category.SetCheckParameter("Hourly_Extraneous_Fields", null, eParameterDataType.String);
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURAD32");
            }

            return ReturnVal;
        }

        public static string HOURAD33(cCategory Category, ref bool Log)
        //Check SO2R Formula
        {
            string ReturnVal = "";

            try
            {
                if (Category.GetCheckParameter("Current_SO2R_Record").ParameterValue != null)
                {
                    DataRowView CurrentSO2Record = (DataRowView)Category.GetCheckParameter("Current_SO2R_Record").ParameterValue;
                    DataRowView CurrentFFRecord = (DataRowView)Category.GetCheckParameter("Current_Fuel_Flow_Record").ParameterValue;
                    if (CurrentSO2Record["FORMULA_IDENTIFIER"] == DBNull.Value)
                    {
                        if (cDBConvert.ToString(CurrentFFRecord["FUEL_CD"]) != "PNG" ||
                            cDBConvert.ToDecimal(CurrentSO2Record["PARAM_VAL_FUEL"]) != (decimal)0.0006)
                            Category.CheckCatalogResult = "A";
                    }
                    else
                    {
                        DataView MonFormRecs = (DataView)Category.GetCheckParameter("Monitor_Formula_Records_By_Hour_Location").ParameterValue;
                        sFilterPair[] Filter = new sFilterPair[1];
                        Filter[0].Set("MON_FORM_ID", cDBConvert.ToString(CurrentSO2Record["MON_FORM_ID"]));
                        DataRowView MonFormRec;
                        if (!FindRow(MonFormRecs, Filter, out MonFormRec))
                            Category.CheckCatalogResult = "B";
                        else
                          if (cDBConvert.ToString(MonFormRec["PARAMETER_CD"]) != "SO2R")
                            Category.CheckCatalogResult = "C";
                        else
                            if (cDBConvert.ToString(MonFormRec["EQUATION_CD"]) != "D-1H")
                            Category.CheckCatalogResult = "D";
                    }
                }
                else
                    Log = false;
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURAD33");
            }

            return ReturnVal;
        }

        public static string HOURAD34(cCategory Category, ref bool Log)
        //Check Reported SO2 Mass Rate
        {
            string ReturnVal = "";

            try
            {
                if (Category.GetCheckParameter("Current_SO2_HPFF_Record").ParameterValue != null)
                {
                    decimal ParamValFuel = cDBConvert.ToDecimal(Category.GetCheckParameter("Current_SO2_HPFF_Record").ValueAsDataRowView()["PARAM_VAL_FUEL"]);
                    if (ParamValFuel < 0)
                        Category.CheckCatalogResult = "A";
                    else
                      if (Category.GetCheckParameter("Current_Fuel_Group").ValueAsString() == "OIL"
                          && ParamValFuel != Math.Round(ParamValFuel, 1, MidpointRounding.AwayFromZero)
                          && ParamValFuel != decimal.MinValue)
                        Category.CheckCatalogResult = "B";
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURAD34");
            }

            return ReturnVal;
        }

        public static string HOURAD35(cCategory Category, ref bool Log)
        //Determine FC Factor
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("HFF_Fc_Factor", null, eParameterDataType.Decimal);
                Category.SetCheckParameter("Current_Fc_Factor_Record", null, eParameterDataType.DataRowView);
                DataRowView CurrentFFRecord = (DataRowView)Category.GetCheckParameter("Current_Fuel_Flow_Record").ParameterValue;
                DataView ParamFFRecords = (DataView)Category.GetCheckParameter("Hourly_Param_Fuel_Flow_Records_For_Current_Fuel_Flow").ParameterValue;
                DataView ParamFFRecordsFound;
                sFilterPair[] Filter = new sFilterPair[1];
                Filter[0].Set("PARAMETER_CD", "FC");
                ParamFFRecordsFound = FindRows(ParamFFRecords, Filter);
                if (ParamFFRecordsFound.Count > 1)
                    Category.CheckCatalogResult = "A";
                else
                  if (ParamFFRecordsFound.Count == 0)
                {
                    if (Category.GetCheckParameter("Current_CO2_HPFF_Record").ParameterValue != null)
                        Category.CheckCatalogResult = "B";
                }
                else
                    if (Category.GetCheckParameter("Current_CO2_HPFF_Record").ParameterValue != null)
                {
                    Category.SetCheckParameter("Current_Fc_Factor_Record", ParamFFRecordsFound[0], eParameterDataType.DataRowView);
                    if (cDBConvert.ToString(ParamFFRecordsFound[0]["PARAMETER_UOM_CD"]) != "SCFCBTU")
                        Category.CheckCatalogResult = "C";
                    else
                    {
                        decimal ParamValFuel = cDBConvert.ToDecimal(ParamFFRecordsFound[0]["PARAM_VAL_FUEL"]);
                        if (ParamValFuel > 0)
                        {
                            if (ParamValFuel != Math.Round(ParamValFuel, 1, MidpointRounding.AwayFromZero) && ParamValFuel != decimal.MinValue)
                                Category.CheckCatalogResult = "G";
                            else
                            {
                                Category.SetCheckParameter("HFF_Fc_Factor", ParamValFuel, eParameterDataType.Decimal);
                                DataView FuelTypeCrossCheck = (DataView)Category.GetCheckParameter("Fuel_Type_Reality_Checks_For_Fc_Factor_Cross_Check_Table").ParameterValue;
                                DataRowView CrossCheckRec;
                                sFilterPair[] Filter2 = new sFilterPair[1];
                                Filter2[0].Set("FuelType", Convert.ToString(Category.GetCheckParameter("Current_Fuel_Group").ParameterValue));
                                decimal MaxFc = decimal.MinValue;
                                decimal MinFc = decimal.MinValue;
                                if (FindRow(FuelTypeCrossCheck, Filter2, out CrossCheckRec))
                                {
                                    MaxFc = cDBConvert.ToDecimal(CrossCheckRec["Upper Value"]);
                                    MinFc = cDBConvert.ToDecimal(CrossCheckRec["Lower Value"]);
                                }
                                if ((MaxFc != decimal.MinValue && ParamValFuel > MaxFc) || (MinFc != decimal.MinValue && ParamValFuel < MinFc))
                                    Category.CheckCatalogResult = "D";
                            }
                        }
                        else
                            Category.CheckCatalogResult = "E";
                    }
                }
                else
                    Category.CheckCatalogResult = "F";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURAD35");
            }

            return ReturnVal;
        }

        public static string HOURAD36(cCategory Category, ref bool Log)
        //Check Extraneous Fc Factor Record Fields
        {
            string ReturnVal = "";

            try
            {
                string HrlyExtFields = "";
                if (Category.GetCheckParameter("Current_Fc_Factor_Record ").ParameterValue != null)
                {
                    DataRowView CurrentFcFactorRec = (DataRowView)Category.GetCheckParameter("Current_Fc_Factor_Record").ParameterValue;
                    if (CurrentFcFactorRec["SAMPLE_TYPE_CD"] != DBNull.Value)
                        HrlyExtFields = HrlyExtFields.ListAdd("SampleTypeCode");
                    if (CurrentFcFactorRec["MON_SYS_ID"] != DBNull.Value)
                        HrlyExtFields = HrlyExtFields.ListAdd("MonitoringSystemID");
                    if (CurrentFcFactorRec["SEGMENT_NUM"] != DBNull.Value)
                        HrlyExtFields = HrlyExtFields.ListAdd("SegmentNumber");
                    if (CurrentFcFactorRec["OPERATING_CONDITION_CD"] != DBNull.Value)
                        HrlyExtFields = HrlyExtFields.ListAdd("OperatingConditionCode");
                    if (CurrentFcFactorRec["FORMULA_IDENTIFIER"] != DBNull.Value)
                        HrlyExtFields = HrlyExtFields.ListAdd("FormulaIdentifier");
                    if (HrlyExtFields != "")
                    {
                        Category.SetCheckParameter("Hourly_Extraneous_Fields", HrlyExtFields.FormatList(), eParameterDataType.String);
                        Category.CheckCatalogResult = "A";
                    }
                    else
                        Category.SetCheckParameter("Hourly_Extraneous_Fields", null, eParameterDataType.String);
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURAD36");
            }

            return ReturnVal;
        }

        public static string HOURAD37(cCategory Category, ref bool Log)
        //Validate CO2 Record
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("Current_CO2_HPFF_Record", null, eParameterDataType.DataRowView);
                DataRowView CurrentFFRecord = (DataRowView)Category.GetCheckParameter("Current_Fuel_Flow_Record").ParameterValue;
                sFilterPair[] ParamFFRecsFilter = new sFilterPair[1];
                DataView ParamFFRecords = (DataView)Category.GetCheckParameter("Hourly_Param_Fuel_Flow_Records_For_Current_Fuel_Flow").ParameterValue;
                ParamFFRecsFilter[0].Set("PARAMETER_CD", "CO2");
                ParamFFRecords = FindRows(ParamFFRecords, ParamFFRecsFilter);
                if (ParamFFRecords.Count > 1)
                {
                    Category.SetCheckParameter("CO2_App_D_Accumulator", -1, eParameterDataType.Decimal);
                    Category.CheckCatalogResult = "A";
                }
                else
                  if (ParamFFRecords.Count == 0)
                {
                    if (Convert.ToBoolean(Category.GetCheckParameter("CO2_App_D_Method_Active_For_Hour").ParameterValue))
                    {
                        Category.SetCheckParameter("CO2_App_D_Accumulator", -1, eParameterDataType.Decimal);
                        if (!Convert.ToBoolean(Category.GetCheckParameter("Legacy_Data_Evaluation").ParameterValue))
                            Category.CheckCatalogResult = "B";
                        else
                            Category.CheckCatalogResult = "H";
                    }
                }
                else
                    if (Convert.ToBoolean(Category.GetCheckParameter("CO2_App_D_Method_Active_For_Hour").ParameterValue))
                {
                    Category.SetCheckParameter("Current_CO2_HPFF_Record", ParamFFRecords[0], eParameterDataType.DataRowView);
                    EmParameters.Co2HpffExists = true;

                    string ParamFFRecMonFormId = cDBConvert.ToString(ParamFFRecords[0]["MON_FORM_ID"]);
                    if (ParamFFRecMonFormId == "")
                        Category.CheckCatalogResult = "C";
                    else
                    {
                        DataView MonFormRecs = (DataView)Category.GetCheckParameter("Monitor_Formula_Records_By_Hour_Location").ParameterValue;
                        sFilterPair[] MonFormFilter = new sFilterPair[1];
                        MonFormFilter[0].Set("MON_FORM_ID", ParamFFRecMonFormId);
                        DataRowView MonFormRec;
                        if (!FindRow(MonFormRecs, MonFormFilter, out MonFormRec))
                            Category.CheckCatalogResult = "D";
                        else
                          if (cDBConvert.ToString(MonFormRec["PARAMETER_CD"]) != "CO2")
                            Category.CheckCatalogResult = "E";
                        else
                            if (cDBConvert.ToString(MonFormRec["EQUATION_CD"]) != "G-4")
                            Category.CheckCatalogResult = "F";
                    }
                }
                else
                    Category.CheckCatalogResult = "G";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURAD37");
            }

            return ReturnVal;
        }

        public static string HOURAD38(cCategory Category, ref bool Log)
        //Check Extraneous CO2 Record Fields
        {
            string ReturnVal = "";

            try
            {
                string HrlyExtFields = "";
                if (Category.GetCheckParameter("Current_CO2_HPFF_Record").ParameterValue != null)
                {
                    DataRowView CurrentCO2HPFFRec = (DataRowView)Category.GetCheckParameter("Current_CO2_HPFF_Record").ParameterValue;
                    if (CurrentCO2HPFFRec["SAMPLE_TYPE_CD"] != DBNull.Value)
                        HrlyExtFields = HrlyExtFields.ListAdd("SampleTypeCode");
                    if (CurrentCO2HPFFRec["MON_SYS_ID"] != DBNull.Value)
                        HrlyExtFields = HrlyExtFields.ListAdd("MonitoringSystemID");
                    if (CurrentCO2HPFFRec["SEGMENT_NUM"] != DBNull.Value)
                        HrlyExtFields = HrlyExtFields.ListAdd("SegmentNumber");
                    if (CurrentCO2HPFFRec["OPERATING_CONDITION_CD"] != DBNull.Value)
                        HrlyExtFields = HrlyExtFields.ListAdd("OperatingConditionCode");

                    if (HrlyExtFields != "")
                    {
                        Category.SetCheckParameter("Hourly_Extraneous_Fields", HrlyExtFields.FormatList(), eParameterDataType.String);
                        Category.CheckCatalogResult = "A";
                    }
                    else
                        Category.SetCheckParameter("Hourly_Extraneous_Fields", null, eParameterDataType.String);
                }
                else
                    Category.SetCheckParameter("Hourly_Extraneous_Fields", null, eParameterDataType.String);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURAD38");
            }

            return ReturnVal;
        }

        public static string HOURAD39(cCategory Category, ref bool Log)
        //Calculate CO2 Mass Rate
        {
            string ReturnVal = "";

            try
            {
                DataRowView HPFFRecord = Category.GetCheckParameter("Current_CO2_HPFF_Record").ValueAsDataRowView();
                if (HPFFRecord != null)
                {
                    decimal CalcRate = cDBConvert.ToDecimal(Category.GetCheckParameter("HFF_Calc_HI_Rate").ParameterValue);
                    decimal FcFactor = cDBConvert.ToDecimal(Category.GetCheckParameter("HFF_Fc_Factor").ParameterValue);
                    if (CalcRate != decimal.MinValue && FcFactor != decimal.MinValue)
                    {
                        decimal CalcCO2 = Math.Round(CalcRate * FcFactor * 44 / (385 * 2000), 1, MidpointRounding.AwayFromZero);
                        decimal Tolerance = GetHourlyEmissionsTolerance("CO2", "TNHR", Category);
                        DataRowView CurrentFFRecord = (DataRowView)Category.GetCheckParameter("Current_Fuel_Flow_Record").ParameterValue;
                        decimal UsageTime = cDBConvert.ToDecimal(CurrentFFRecord["FUEL_USAGE_TIME"]);
                        decimal AppDAccum = Category.GetCheckParameter("Co2_App_D_Accumulator").ValueAsDecimal();
                        if (UsageTime > 0 && UsageTime <= 1 && AppDAccum >= 0)
                            Category.AccumCheckAggregate("CO2_App_D_Accumulator", UsageTime * CalcCO2);
                        else
                            Category.SetCheckParameter("CO2_App_D_Accumulator", -1, eParameterDataType.Decimal);
                        decimal ParamValFuel = cDBConvert.ToDecimal(HPFFRecord["PARAM_VAL_FUEL"]);
                        if (ParamValFuel >= 0)
                            if (Math.Abs(ParamValFuel - CalcCO2) > Tolerance)
                                Category.CheckCatalogResult = "A";
                        Category.SetCheckParameter("HFF_Calc_CO2", CalcCO2, eParameterDataType.Decimal);
                    }
                    else
                    {
                        Category.SetCheckParameter("CO2_App_D_Accumulator", -1, eParameterDataType.Decimal);
                        Category.SetCheckParameter("HFF_Calc_CO2", null, eParameterDataType.Decimal);
                        Category.CheckCatalogResult = "B";
                    }
                }
                else
                    Category.SetCheckParameter("HFF_Calc_CO2", null, eParameterDataType.Decimal);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURAD39");
            }

            return ReturnVal;
        }

        public static string HOURAD40(cCategory Category, ref bool Log)
        //Check Reported CO2 Mass Rate
        {
            string ReturnVal = "";

            try
            {
                if (Category.GetCheckParameter("Current_CO2_HPFF_Record").ParameterValue != null)
                {
                    decimal ParamValFuel = cDBConvert.ToDecimal(Category.GetCheckParameter("Current_CO2_HPFF_Record").ValueAsDataRowView()["PARAM_VAL_FUEL"]);
                    if (ParamValFuel < 0)
                        Category.CheckCatalogResult = "A";
                    else
                      if (ParamValFuel != Math.Round(ParamValFuel, 1, MidpointRounding.AwayFromZero) && ParamValFuel != decimal.MinValue)
                        Category.CheckCatalogResult = "B";
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURAD40");
            }

            return ReturnVal;
        }

        #endregion


        #region 41 - 50

        public static string HOURAD44(cCategory Category, ref bool Log)
        //Check CO2 Units Of Measure
        {
            string ReturnVal = "";

            try
            {
                if (Category.GetCheckParameter("Current_CO2_HPFF_Record").ParameterValue != null)
                    if (cDBConvert.ToString(Category.GetCheckParameter("Current_CO2_HPFF_Record").ValueAsDataRowView()["PARAMETER_UOM_CD"]) != "TNHR")
                        Category.CheckCatalogResult = "A";
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURAD44");
            }

            return ReturnVal;
        }

        public static string HOURAD45(cCategory category, ref bool log)
        // Determine Appendix D Measure Codes
        {
            string returnVal = "";

            try
            {
                string[] monitorMeasureCodeArray = category.GetCheckParameter("Monitor_Measure_Code_Array").ValueAsStringArray();

                // Fuel Flow Hour Measure Code
                {
                    DataRowView currentFuelFlowRecord = category.GetCheckParameter("Current_Fuel_Flow_Record").AsDataRowView();

                    string sourceOfDataMassCd = currentFuelFlowRecord["SOD_MASS_CD"].AsString();
                    string sourceOfDataVolumetricCd = currentFuelFlowRecord["SOD_VOLUMETRIC_CD"].AsString();

                    if (sourceOfDataMassCd.InList("4,5,6") || sourceOfDataVolumetricCd.InList("4,5,6") ||
                        (monitorMeasureCodeArray[(int)eHourMeasureParameter.Ff] == "OTHER"))
                        monitorMeasureCodeArray[(int)eHourMeasureParameter.Ff] = "OTHER";
                    else if (sourceOfDataMassCd.InList("1,3") || sourceOfDataVolumetricCd.InList("1,3"))
                    {
                        if (monitorMeasureCodeArray[(int)eHourMeasureParameter.Ff].IsNotNull() &&
                            monitorMeasureCodeArray[(int)eHourMeasureParameter.Ff].StartsWith("MEAS"))
                            monitorMeasureCodeArray[(int)eHourMeasureParameter.Ff] = "MEASSUB";
                        else
                            monitorMeasureCodeArray[(int)eHourMeasureParameter.Ff] = "SUB";
                    }
                    else if (sourceOfDataMassCd.InList("0,9") || sourceOfDataVolumetricCd.InList("0,9"))
                    {
                        if (monitorMeasureCodeArray[(int)eHourMeasureParameter.Ff].IsNotNull() &&
                            monitorMeasureCodeArray[(int)eHourMeasureParameter.Ff].Contains("SUB"))
                            monitorMeasureCodeArray[(int)eHourMeasureParameter.Ff] = "MEASSUB";
                        else
                            monitorMeasureCodeArray[(int)eHourMeasureParameter.Ff] = "MEASURE";
                    }
                }

                // Sulfer Hour Measure Code
                {
                    DataRowView currentSulferRecord = category.GetCheckParameter("Current_Sulfur_Record").AsDataRowView();

                    if (currentSulferRecord != null)
                    {
                        string sampleTypeCd = currentSulferRecord["SAMPLE_TYPE_CD"].AsString();

                        if (sampleTypeCd == "8")
                        {
                            if (monitorMeasureCodeArray[(int)eHourMeasureParameter.Sulfer].IsNotNull() &&
                                monitorMeasureCodeArray[(int)eHourMeasureParameter.Sulfer].StartsWith("MEAS"))
                                monitorMeasureCodeArray[(int)eHourMeasureParameter.Sulfer] = "MEASSUB";
                            else
                                monitorMeasureCodeArray[(int)eHourMeasureParameter.Sulfer] = "SUB";
                        }
                        else if (sampleTypeCd.InList("0,1,2,4,5,6,7"))
                        {
                            if (monitorMeasureCodeArray[(int)eHourMeasureParameter.Sulfer].IsNotNull() &&
                                monitorMeasureCodeArray[(int)eHourMeasureParameter.Sulfer].Contains("SUB"))
                                monitorMeasureCodeArray[(int)eHourMeasureParameter.Sulfer] = "MEASSUB";
                            else
                                monitorMeasureCodeArray[(int)eHourMeasureParameter.Sulfer] = "MEASURE";
                        }
                    }
                }

                // GCV Hour Measure Code
                {
                    DataRowView currentGcvRecord = category.GetCheckParameter("Current_GCV_Record").AsDataRowView();

                    if (currentGcvRecord != null)
                    {
                        string sampleTypeCd = currentGcvRecord["SAMPLE_TYPE_CD"].AsString();

                        if (sampleTypeCd == "8")
                        {
                            if (monitorMeasureCodeArray[(int)eHourMeasureParameter.Gcv].IsNotNull() &&
                                monitorMeasureCodeArray[(int)eHourMeasureParameter.Gcv].StartsWith("MEAS"))
                                monitorMeasureCodeArray[(int)eHourMeasureParameter.Gcv] = "MEASSUB";
                            else
                                monitorMeasureCodeArray[(int)eHourMeasureParameter.Gcv] = "SUB";
                        }
                        else if (sampleTypeCd.InList("0,1,2,3,4,5,6,7"))
                        {
                            if (monitorMeasureCodeArray[(int)eHourMeasureParameter.Gcv].IsNotNull() &&
                                monitorMeasureCodeArray[(int)eHourMeasureParameter.Gcv].Contains("SUB"))
                                monitorMeasureCodeArray[(int)eHourMeasureParameter.Gcv] = "MEASSUB";
                            else
                                monitorMeasureCodeArray[(int)eHourMeasureParameter.Gcv] = "MEASURE";
                        }
                    }
                }

                // Density Hour Measure Code
                {
                    DataRowView currentDensityRecord = category.GetCheckParameter("Current_Density_Record").AsDataRowView();

                    if (currentDensityRecord != null)
                    {
                        string sampleTypeCd = currentDensityRecord["SAMPLE_TYPE_CD"].AsString();

                        if (sampleTypeCd == "8")
                        {
                            if (monitorMeasureCodeArray[(int)eHourMeasureParameter.Density].IsNotNull() &&
                                monitorMeasureCodeArray[(int)eHourMeasureParameter.Density].StartsWith("MEAS"))
                                monitorMeasureCodeArray[(int)eHourMeasureParameter.Density] = "MEASSUB";
                            else
                                monitorMeasureCodeArray[(int)eHourMeasureParameter.Density] = "SUB";
                        }
                        else if (sampleTypeCd.InList("1,2,5,6,7"))
                        {
                            if (monitorMeasureCodeArray[(int)eHourMeasureParameter.Density].IsNotNull() &&
                                monitorMeasureCodeArray[(int)eHourMeasureParameter.Density].Contains("SUB"))
                                monitorMeasureCodeArray[(int)eHourMeasureParameter.Density] = "MEASSUB";
                            else
                                monitorMeasureCodeArray[(int)eHourMeasureParameter.Density] = "MEASURE";
                        }
                    }
                }

                category.SetArrayParameter("Monitor_Measure_Code_Array", monitorMeasureCodeArray);
            }
            catch (Exception ex)
            {
                returnVal = category.CheckEngine.FormatError(ex, "HOURAD45");
            }

            return returnVal;
        }

        /// <summary>
        /// Updates the System Operarting Supplemental Data for the system reported with the Current Hourly Fuel Flow reocrd.
        /// </summary>
        /// <param name="category"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public static string HOURAD46(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                if (EmParameters.DerivedHourlyChecksNeeded.Default(false) && (EmParameters.CurrentOperatingTime.Value > 0))
                {
                    string monSysId = EmParameters.CurrentFuelFlowRecord.MonSysId;

                    if (monSysId.IsNotEmpty())
                    {
                        Dictionary<string, SystemOperatingSupplementalData> supplementalDataDictionary = EmParameters.SystemOperatingSuppDataDictionaryArray[EmParameters.CurrentMonitorPlanLocationPostion.Value];

                        // Get or created supplemental data record
                        SystemOperatingSupplementalData supplementalDataRecord;
                        {
                            if (supplementalDataDictionary.ContainsKey(monSysId))
                            {
                                supplementalDataRecord = supplementalDataDictionary[monSysId];
                            }
                            else
                            {
                                supplementalDataRecord = new SystemOperatingSupplementalData(EmParameters.CurrentReportingPeriod.Value,
                                                                                             monSysId,
                                                                                             EmParameters.CurrentFuelFlowRecord.MonLocId,
                                                                                             true);

                                supplementalDataDictionary.Add(monSysId, supplementalDataRecord);
                            }
                        }

                        // Update with null modcCd will skip Quality Assured and Monitor Available counting, witch are not needed.
                        supplementalDataRecord.IncreamentForCurrentHour(EmParameters.CurrentOperatingDatehour.Value, null);
                    }
                }
            }
            catch (Exception ex)
            {
                returnVal = category.CheckEngine.FormatError(ex);
            }

            return returnVal;
        }

        #endregion

        #endregion


        #region Private Methods: Utilities

        private static new decimal GetHourlyEmissionsTolerance(string AParameterCd, String AUom, cCategory ACategory)
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
