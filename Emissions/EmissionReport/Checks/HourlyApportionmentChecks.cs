using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Data.Ecmps.Dbo.View;
using ECMPS.Checks.EmissionsReport;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;
using ECMPS.Definitions.Extensions;
using ECMPS.Checks.Em.Parameters;

namespace ECMPS.Checks.EmissionsChecks
{
    public class cHourlyApportionmentChecks : cEmissionsChecks
    {

        #region Constructors

        public cHourlyApportionmentChecks(cEmissionsReportProcess emissionReportProcess)
          : base(emissionReportProcess)
        {
            CheckProcedures = new dCheckProcedure[15];

            CheckProcedures[1] = new dCheckProcedure(HOURAPP1);
            CheckProcedures[2] = new dCheckProcedure(HOURAPP2);
            CheckProcedures[3] = new dCheckProcedure(HOURAPP3);
            CheckProcedures[4] = new dCheckProcedure(HOURAPP4);
            CheckProcedures[5] = new dCheckProcedure(HOURAPP5);
            CheckProcedures[6] = new dCheckProcedure(HOURAPP6);
            CheckProcedures[7] = new dCheckProcedure(HOURAPP7);
            CheckProcedures[9] = new dCheckProcedure(HOURAPP9);
            CheckProcedures[10] = new dCheckProcedure(HOURAPP10);
            CheckProcedures[11] = new dCheckProcedure(HOURAPP11);
            CheckProcedures[12] = new dCheckProcedure(HOURAPP12);
            CheckProcedures[13] = new dCheckProcedure(HOURAPP13);
            CheckProcedures[14] = new dCheckProcedure(HOURAPP14);
        }

        #endregion


        #region Public Static Methods: Checks

        public static string HOURAPP1(cCategory Category, ref bool Log)
        // Determine Monitoring Plan Configuration 
        {
            string ReturnVal = "";

            try
            {
                sFilterPair[] RowFilter;

                Category.SetCheckParameter("MP_Stack_Config_For_Hourly_Checks", null, eParameterDataType.String);
                Category.SetCheckParameter("MP_Pipe_Config_for_Hourly_Checks", null, eParameterDataType.String);
                Category.SetCheckParameter("MP_Load_UOM", null, eParameterDataType.String);
                Category.SetCheckParameter("MP_Unit_Load", null, eParameterDataType.Decimal);
                Category.SetCheckParameter("Stack_Optime_Accumulator", 0m, eParameterDataType.Decimal);
                Category.SetCheckParameter("Stack_Loadtimesoptime_Accumulator", 0m, eParameterDataType.Decimal);
                Category.SetCheckParameter("Stack_HeatInputTimesOpTime_Accumulator", 0m, eParameterDataType.Decimal);
                Category.SetCheckParameter("Pipe_LoadTimesOpTime_Accumulator", 0m, eParameterDataType.Decimal);
                Category.SetCheckParameter("Config_HeatInputTimesOpTime_Accumulator", 0m, eParameterDataType.Decimal);
                Category.SetCheckParameter("Config_NOxRateTimesHeatInput_Accumulator", 0m, eParameterDataType.Decimal);
                Category.SetCheckParameter("Config_NOxRateTimesOptime_Accumulator", 0m, eParameterDataType.Decimal);
                Category.SetCheckParameter("Config_Optime_Accumulator", 0m, eParameterDataType.Decimal);
                Category.SetCheckParameter("Config_HeatInput_Accumulator", 0m, eParameterDataType.Decimal);
                Category.SetCheckParameter("Max_Stack_Optime", 0m, eParameterDataType.Decimal);
                Category.SetCheckParameter("Unit_Optime_Accumulator", 0m, eParameterDataType.Decimal);
                Category.SetCheckParameter("Unit_Loadtimesoptime_Accumulator", 0m, eParameterDataType.Decimal);
                Category.SetCheckParameter("Unit_HeatInputTimesOpTime_Accumulator", 0m, eParameterDataType.Decimal);
                Category.SetCheckParameter("Max_Unit_Optime", 0m, eParameterDataType.Decimal);
                Category.SetCheckParameter("CP_Fuel_Count", 0, eParameterDataType.Integer);
                DateTime CurrentOpDate = Category.GetCheckParameter("Current_Operating_Date").ValueAsDateTime(DateTypes.START);
                string CurrMonth = CurrentOpDate.ToString("MMMM");
                Category.SetCheckParameter("Current_Month", CurrMonth, eParameterDataType.String);
                int CurrentLocationCount = Convert.ToInt16(Category.GetCheckParameter("Current_Location_Count").ParameterValue);
                Category.SetCheckParameter("App_E_Reporting_Method", null, eParameterDataType.String);
                Category.SetCheckParameter("App_E_Op_Code", null, eParameterDataType.String);
                Category.SetCheckParameter("App_E_Segment_Number", null, eParameterDataType.Integer);
                Category.SetCheckParameter("App_E_Reported_Value", null, eParameterDataType.Decimal);
                Category.SetCheckParameter("App_E_Fuel_Code", null, eParameterDataType.String);
                Category.SetCheckParameter("App_E_Calc_Hi", null, eParameterDataType.Decimal);
                Category.SetCheckParameter("App_E_NOXE_System_ID", null, eParameterDataType.String);
                Category.SetCheckParameter("App_E_NOXE_System_Identifier", null, eParameterDataType.String);
                Category.SetCheckParameter("Current_Appendix_E_Status", null, eParameterDataType.String);
                Category.SetCheckParameter("Earliest_Location_Report_Date", null, eParameterDataType.Date);
                Category.SetCheckParameter("Current_Measure_Code", null, eParameterDataType.String);
                Category.SetCheckParameter("MATS_MS1_Hg_DHV_ID", null, eParameterDataType.String);
                Category.SetCheckParameter("MATS_MS1_HCL_DHV_ID", null, eParameterDataType.String);
                Category.SetCheckParameter("MATS_MS1_HF_DHV_ID", null, eParameterDataType.String);
                Category.SetCheckParameter("MATS_MS1_SO2_DHV_ID", null, eParameterDataType.String);
                Category.SetCheckParameter("MATS_Parameter_Plugin_Hg", null, eParameterDataType.String);
                Category.SetCheckParameter("MATS_Parameter_Plugin_HCL", null, eParameterDataType.String);
                Category.SetCheckParameter("MATS_Parameter_Plugin_HF", null, eParameterDataType.String);
                Category.SetCheckParameter("MATS_Parameter_Plugin_SO2", null, eParameterDataType.String);
                Category.SetCheckParameter("MATS_MS1_Hg_Unadjusted_Hourly_Value", null, eParameterDataType.String);
                Category.SetCheckParameter("MATS_MS1_HCL_Unadjusted_Hourly_Value", null, eParameterDataType.String);
                Category.SetCheckParameter("MATS_MS1_HF_Unadjusted_Hourly_Value", null, eParameterDataType.String);
                Category.SetCheckParameter("MATS_MS1_SO2_Unadjusted_Hourly_Value", null, eParameterDataType.String);

                string[] monitorMeasureCodeArray;
                {
                    monitorMeasureCodeArray = new string[Enum.GetNames(typeof(eHourMeasureParameter)).Length];

                    for (int hourMeasureParameterDex = 0;
                         hourMeasureParameterDex < Enum.GetNames(typeof(eHourMeasureParameter)).Length;
                         hourMeasureParameterDex++)
                    {
                        monitorMeasureCodeArray[hourMeasureParameterDex] = "";
                    }
                }
                Category.SetCheckParameter("Monitor_Measure_Code_Array", monitorMeasureCodeArray, eParameterDataType.String, false, true);

                decimal[] ApportionmentOptimeArray = new decimal[CurrentLocationCount];
                int[] ApportionmentLoadArray = new int[CurrentLocationCount];
                decimal[] ApportionmentCalcHiArray = new decimal[CurrentLocationCount];
                decimal[] ApportionmentCalcNoxrArray = new decimal[CurrentLocationCount];
                string[] ApportionmentHiMethodArray = new string[CurrentLocationCount];
                string[] ApportionmentNoxMethodArray = new string[CurrentLocationCount];
                string[] ApportionmentNoxrMethodArray = new string[CurrentLocationCount];
                string[] ApportionmentStackUnitList = new string[CurrentLocationCount];
                decimal?[] ApportionmentStackFlowArray = new decimal?[CurrentLocationCount];
                int?[] ApportionmentMatsLoadArray = new int?[CurrentLocationCount];
                string[] ApportionmentHgRateArray = new string[CurrentLocationCount];
                string[] ApportionmentHclRateArray = new string[CurrentLocationCount];
                string[] ApportionmentHfRateArray = new string[CurrentLocationCount];
                string[] ApportionmentSo2RateArray = new string[CurrentLocationCount];
                string[] MatsMS1HgModcCodeArray = new string[CurrentLocationCount];
                string[] MatsMS1HclModcCodeArray = new string[CurrentLocationCount];
                string[] MatsMS1HfModcCodeArray = new string[CurrentLocationCount];
                string[] MatsMS1So2ModcCodeArray = new string[CurrentLocationCount];

                for (int CurrentLocationDex = 0; CurrentLocationDex < CurrentLocationCount; CurrentLocationDex++)
                {
                    ApportionmentOptimeArray[CurrentLocationDex] = 0.0m;
                    ApportionmentLoadArray[CurrentLocationDex] = 0;
                    ApportionmentCalcHiArray[CurrentLocationDex] = 0.0m;
                    ApportionmentCalcNoxrArray[CurrentLocationDex] = 0.0m;
                    ApportionmentHiMethodArray[CurrentLocationDex] = null;
                    ApportionmentNoxMethodArray[CurrentLocationDex] = null;
                    ApportionmentNoxrMethodArray[CurrentLocationDex] = null;
                    ApportionmentStackUnitList[CurrentLocationDex] = null;
                    ApportionmentStackFlowArray[CurrentLocationDex] = null;
                    ApportionmentMatsLoadArray[CurrentLocationDex] = null;
                    ApportionmentHgRateArray[CurrentLocationDex] = null;
                    ApportionmentHclRateArray[CurrentLocationDex] = null;
                    ApportionmentHfRateArray[CurrentLocationDex] = null;
                    ApportionmentSo2RateArray[CurrentLocationDex] = null;
                    MatsMS1HgModcCodeArray[CurrentLocationDex] = null;
                    MatsMS1HclModcCodeArray[CurrentLocationDex] = null;
                    MatsMS1HfModcCodeArray[CurrentLocationDex] = null;
                    MatsMS1So2ModcCodeArray[CurrentLocationDex] = null;
                }

                Category.SetCheckParameter("Apportionment_Optime_Array", ApportionmentOptimeArray, eParameterDataType.Decimal, false, true);
                Category.SetCheckParameter("Apportionment_Load_Array", ApportionmentLoadArray, eParameterDataType.Integer, false, true);
                Category.SetCheckParameter("Apportionment_Calc_Hi_Array", ApportionmentCalcHiArray, eParameterDataType.Decimal, false, true);
                Category.SetCheckParameter("Apportionment_Calc_NOXR_Array", ApportionmentCalcNoxrArray, eParameterDataType.Decimal, false, true);
                Category.SetCheckParameter("Apportionment_HI_Method_Array", ApportionmentHiMethodArray, eParameterDataType.String, false, true);
                Category.SetCheckParameter("Apportionment_NOX_Method_Array", ApportionmentNoxMethodArray, eParameterDataType.String, false, true);
                Category.SetCheckParameter("Apportionment_NOXR_Method_Array", ApportionmentNoxrMethodArray, eParameterDataType.String, false, true);
                Category.SetCheckParameter("Apportionment_Stack_Flow_Array", ApportionmentStackFlowArray, eParameterDataType.Decimal, false, true);
                Category.SetCheckParameter("Apportionment_MATS_Load_Array", ApportionmentMatsLoadArray, eParameterDataType.Integer, false, true);
                Category.SetCheckParameter("Apportionment_Hg_Rate_Array", ApportionmentHgRateArray, eParameterDataType.String, false, true);
                Category.SetCheckParameter("Apportionment_HCL_Rate_Array", ApportionmentHclRateArray, eParameterDataType.String, false, true);
                Category.SetCheckParameter("Apportionment_HF_Rate_Array", ApportionmentHfRateArray, eParameterDataType.String, false, true);
                Category.SetCheckParameter("Apportionment_SO2_Rate_Array", ApportionmentSo2RateArray, eParameterDataType.String, false, true);
                Category.SetCheckParameter("MATS_MS1_Hg_MODC_Code_Array", MatsMS1HgModcCodeArray, eParameterDataType.String, false, true);
                Category.SetCheckParameter("MATS_MS1_HCL_MODC_Code_Array", MatsMS1HclModcCodeArray, eParameterDataType.String, false, true);
                Category.SetCheckParameter("MATS_MS1_HF_MODC_Code_Array", MatsMS1HfModcCodeArray, eParameterDataType.String, false, true);
                Category.SetCheckParameter("MATS_MS1_SO2_MODC_Code_Array", MatsMS1So2ModcCodeArray, eParameterDataType.String, false, true);

                if (CurrentLocationCount > 1)
                {
                    DataView MonitoringPlanLocationRecords = Category.GetCheckParameter("Monitoring_Plan_Location_Records").ValueAsDataView();
                    DataView UnitStackConfigurationRecords = Category.GetCheckParameter("Unit_Stack_Configuration_Records_By_Hour_Monitor_Plan").ValueAsDataView();

                    string StackPipeList = "";
                    string UnitList = "";
                    string MonLocList = "";

                    MonitoringPlanLocationRecords.Sort = "MON_LOC_ID";
                    foreach (DataRowView MonitorPlanRow in MonitoringPlanLocationRecords)
                    {
                        if (MonitorPlanRow["STACK_PIPE_ID"] != DBNull.Value)
                            StackPipeList = StackPipeList.ListAdd(cDBConvert.ToString(MonitorPlanRow["STACK_PIPE_ID"]));
                        if (MonitorPlanRow["UNIT_ID"] != DBNull.Value)
                            UnitList = UnitList.ListAdd(cDBConvert.ToString(MonitorPlanRow["UNIT_ID"]));
                        MonLocList = MonLocList.ListAdd(cDBConvert.ToString(MonitorPlanRow["MON_LOC_ID"]));
                    }

                    int MsCount = 0; int MpCount = 0; int CsCount = 0; int CpCount = 0;
                    int UnitCount = 0; int CsUnitCount = 0; int CpUnitCount = 0; int UnitMsCount = 0;
                    int LocationPosition = -1;
                    string LocationName, StackUnitList;

                    foreach (DataRowView MonitorPlanRow in MonitoringPlanLocationRecords)
                    {
                        LocationName = cDBConvert.ToString(MonitorPlanRow["LOCATION_NAME"]);
                        StackUnitList = "";
                        LocationPosition = LocationPosition + 1;

                        if (MonitorPlanRow["STACK_PIPE_ID"] != DBNull.Value)
                        {
                            string StackPipeId = cDBConvert.ToString(MonitorPlanRow["STACK_PIPE_ID"]);

                            RowFilter = new sFilterPair[3];
                            RowFilter[0].Set("BEGIN_DATE", Category.CurrentOpDate, eFilterDataType.DateBegan, eFilterPairRelativeCompare.LessThanOrEqual);
                            RowFilter[1].Set("END_DATE", Category.CurrentOpDate, eFilterDataType.DateEnded, eFilterPairRelativeCompare.GreaterThanOrEqual);
                            RowFilter[2].Set("STACK_PIPE_ID", StackPipeId);

                            int StackUnitCount = CountRows(UnitStackConfigurationRecords, RowFilter);
                            DataView USCFiltered = FindRows(UnitStackConfigurationRecords, RowFilter);

                            foreach (DataRowView drv in USCFiltered)
                            {
                                for (int UnitPosition = 0; UnitPosition < MonLocList.ListCount(); UnitPosition++)
                                {
                                    if (cDBConvert.ToString(drv["MON_LOC_ID"]) == MonLocList.ListItem(UnitPosition))
                                    {
                                        StackUnitList = StackUnitList.ListAdd(cDBConvert.ToString(UnitPosition));
                                        UnitPosition = MonLocList.ListCount();
                                    }
                                }
                            }
                            ApportionmentStackUnitList[LocationPosition] = StackUnitList;


                            if (LocationName.PadRight(2).StartsWith("MS"))
                                MsCount += 1;
                            else if (LocationName.PadRight(2).StartsWith("MP"))
                                MpCount += 1;
                            else if (LocationName.PadRight(2).StartsWith("CS"))
                            {
                                CsCount += 1;

                                if (CsCount == 1)
                                    CsUnitCount = StackUnitCount;
                            }
                            else if (LocationName.PadRight(2).StartsWith("CP"))
                            {
                                CpCount += 1;

                                if (CpCount == 1)
                                    CpUnitCount = StackUnitCount;
                            }
                        }
                        else if (MonitorPlanRow["UNIT_ID"] != DBNull.Value)
                        {
                            string UnitId = cDBConvert.ToString(MonitorPlanRow["UNIT_ID"]);

                            UnitCount += 1;

                            if (UnitCount == 1)
                            {
                                RowFilter = new sFilterPair[4];
                                RowFilter[0].Set("BEGIN_DATE", Category.CurrentOpDate, eFilterDataType.DateBegan, eFilterPairRelativeCompare.LessThanOrEqual);
                                RowFilter[1].Set("END_DATE", Category.CurrentOpDate, eFilterDataType.DateEnded, eFilterPairRelativeCompare.GreaterThanOrEqual);
                                RowFilter[2].Set("UNIT_ID", UnitId);
                                RowFilter[3].Set("STACK_NAME", "MS", eFilterPairStringCompare.BeginsWith);

                                UnitMsCount = CountRows(UnitStackConfigurationRecords, RowFilter);
                            }
                        }
                    }

                    if ((MsCount > 1) && (CsCount == 0) && (UnitCount == 1) && (MsCount == UnitMsCount))
                    {
                        Category.SetCheckParameter("MP_Stack_Config_For_Hourly_Checks", "MS", eParameterDataType.String);
                        Category.SetCheckParameter("Multiple_Stack_Configuration", true, eParameterDataType.Boolean);
                    }
                    else if ((CsCount == 1) && (UnitCount > 1) && (UnitCount == CsUnitCount))
                    {
                        if (MsCount == 0)
                            Category.SetCheckParameter("MP_Stack_Config_For_Hourly_Checks", "CS", eParameterDataType.String);
                        else
                            Category.SetCheckParameter("MP_Stack_Config_For_Hourly_Checks", "CSMS", eParameterDataType.String);
                    }
                    else if (CsCount + MsCount > 0)
                        Category.SetCheckParameter("MP_Stack_Config_For_Hourly_Checks", "COMPLEX", eParameterDataType.String);

                    if ((CpCount == 1) && (MpCount == 0) && (UnitCount > 1) && (UnitCount == CpUnitCount))
                        Category.SetCheckParameter("MP_Pipe_Config_for_Hourly_Checks", "CP", eParameterDataType.String);
                    else if ((CpCount + MpCount) > 0)
                        Category.SetCheckParameter("MP_Pipe_Config_for_Hourly_Checks", "MULTIPLE", eParameterDataType.String);
                }

                Category.SetCheckParameter("Apportionment_Stack_Unit_List", ApportionmentStackUnitList, eParameterDataType.String, false, true);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURAPP1");
            }

            return ReturnVal;
        }

        public static string HOURAPP2(cCategory Category, ref bool Log)
        // Pre-Validate Heat Input Calculation
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("Calculate_Apportioned_HI", false, eParameterDataType.Boolean);
                Category.SetCheckParameter("Calculate_NOXM_From_Apportioned_HI", false, eParameterDataType.Boolean);

                DataRowView CurrentMonitorPlanLocationRecord = Category.GetCheckParameter("Current_Monitor_Plan_Location_Record").ValueAsDataRowView();

                string CurrentHiEntityType = "";

                if (CurrentMonitorPlanLocationRecord["STACK_PIPE_ID"] != DBNull.Value)
                {
                    string LocationName = cDBConvert.ToString(CurrentMonitorPlanLocationRecord["LOCATION_NAME"]);

                    if (LocationName.PadRight(2).StartsWith("CS"))
                    {
                        CurrentHiEntityType = "CS";
                        Category.SetCheckParameter("Current_HI_Entity_Type", CurrentHiEntityType, eParameterDataType.String);
                    }
                    else if (LocationName.PadRight(2).StartsWith("CP"))
                    {
                        CurrentHiEntityType = "CP";
                        Category.SetCheckParameter("Current_HI_Entity_Type", CurrentHiEntityType, eParameterDataType.String);
                    }
                    else if (LocationName.PadRight(2).StartsWith("MS"))
                    {
                        CurrentHiEntityType = "MS";
                        Category.SetCheckParameter("Current_HI_Entity_Type", CurrentHiEntityType, eParameterDataType.String);
                    }
                    else if (LocationName.PadRight(2).StartsWith("MP"))
                    {
                        CurrentHiEntityType = "MP";
                        Category.SetCheckParameter("Current_HI_Entity_Type", CurrentHiEntityType, eParameterDataType.String);
                    }
                }
                else if (CurrentMonitorPlanLocationRecord["UNIT_ID"] != DBNull.Value)
                {
                    CurrentHiEntityType = "Unit";
                    Category.SetCheckParameter("Current_HI_Entity_Type", CurrentHiEntityType, eParameterDataType.String);
                }

                string[] ApportionmentHiMethodArray = Category.GetCheckParameter("Apportionment_HI_Method_Array").ValueAsStringArray();
                int[] ApportionmentLoadArray = Category.GetCheckParameter("Apportionment_Load_Array").ValueAsIntArray();
                decimal[] ApportionmentOpTimeArray = Category.GetCheckParameter("Apportionment_Optime_Array").ValueAsDecimalArray();

                int NumberOfItems = 0;
                {
                    foreach (decimal Item in ApportionmentOpTimeArray)
                        if (Item > 0) NumberOfItems += 1;
                }

                if ((ApportionmentHiMethodArray[Category.CurrentMonLocPos] != null) &&
                    (ApportionmentHiMethodArray[Category.CurrentMonLocPos].Contains("CALC") ||
                     (ApportionmentHiMethodArray[Category.CurrentMonLocPos] == "COMPLEX")))
                {
                    string MpStackConfigForHourlyChecks = Category.GetCheckParameter("MP_Stack_Config_For_Hourly_Checks").ValueAsString();
                    string MpPipeConfigForHourlyChecks = Category.GetCheckParameter("MP_Pipe_Config_for_Hourly_Checks").ValueAsString();
                    int CPFuelCount = Category.GetCheckParameter("CP_Fuel_Count").ValueAsInt();

                    if (CPFuelCount > 1 && MpPipeConfigForHourlyChecks == "CP")
                        Category.SetArrayParameter("Apportionment_HI_Method_Array", Category.CurrentMonLocPos, "NOCALC");

                    if ((ApportionmentOpTimeArray[Category.CurrentMonLocPos] > 0) &&
                        (ApportionmentOpTimeArray[Category.CurrentMonLocPos] <= 1))
                    {

                        //Equation F-25
                        if ((MpStackConfigForHourlyChecks == "CS") && (CurrentHiEntityType == "CS"))
                        {
                            if (Category.GetCheckParameter("Apportionment_NOX_Method_Array").ValueAsStringArray()[Category.CurrentMonLocPos] == "NOXR")
                                Category.SetCheckParameter("Calculate_NOXM_From_Apportioned_HI", true, eParameterDataType.Boolean);

                            if (ApportionmentOpTimeArray[Category.CurrentMonLocPos]
                                < Category.GetCheckParameter("Max_Unit_Optime").ValueAsDecimal())
                                Category.CheckCatalogResult = "A";
                            else if (ApportionmentOpTimeArray[Category.CurrentMonLocPos]
                                       > (Category.GetCheckParameter("Unit_Optime_Accumulator").ValueAsDecimal()
                                          + (Category.GetCheckParameter("Current_Unit_Count").AsInteger(0) * 0.005m)))
                                Category.CheckCatalogResult = "B";
                            else
                            {
                                decimal StackLoadTimesOpTimeAccumulator = Category.GetCheckParameter("Stack_Loadtimesoptime_Accumulator").ValueAsDecimal();
                                decimal UnitLoadTimesOpTimeAccumulator = Category.GetCheckParameter("Unit_Loadtimesoptime_Accumulator").ValueAsDecimal();

                                if (Category.GetCheckParameter("Config_HeatInputTimesOpTime_Accumulator").ValueAsDecimal() > 0)
                                    Category.SetCheckParameter("Calculate_Apportioned_HI", true, eParameterDataType.Boolean);

                                if ((Category.GetCheckParameter("MP_Load_UOM").ValueAsString() != "INVALID") &&
                                    (StackLoadTimesOpTimeAccumulator > 0) && (UnitLoadTimesOpTimeAccumulator > 0) &&
                                    (Math.Abs(StackLoadTimesOpTimeAccumulator - UnitLoadTimesOpTimeAccumulator) >= NumberOfItems))
                                    Category.CheckCatalogResult = "C";
                            }
                        }

                        //Equation F-21A/B
                        else if (((MpStackConfigForHourlyChecks == "CS") || (MpPipeConfigForHourlyChecks == "CP")) &&
                                 !ApportionmentHiMethodArray[Category.CurrentMonLocPos].InList("NOCALC,COMPLEX"))
                        {
                            if (Category.GetCheckParameter("Apportionment_NOX_Method_Array").ValueAsStringArray()[Category.CurrentMonLocPos] == "NOXR")
                                Category.SetCheckParameter("Calculate_NOXM_From_Apportioned_HI", true, eParameterDataType.Boolean);

                            if (Category.GetCheckParameter("Max_Stack_Optime").ValueAsDecimal()
                                < Category.GetCheckParameter("Max_Unit_Optime").ValueAsDecimal() && MpPipeConfigForHourlyChecks == "")
                                Category.CheckCatalogResult = "A";
                            else if (Category.GetCheckParameter("Max_Stack_Optime").ValueAsDecimal()
                                       > (Category.GetCheckParameter("Unit_Optime_Accumulator").ValueAsDecimal()
                                          + (Category.GetCheckParameter("Current_Unit_Count").AsInteger(0) * 0.005m))
                                     && MpPipeConfigForHourlyChecks == "")
                                Category.CheckCatalogResult = "B";
                            else if (Category.GetCheckParameter("MP_Load_UOM").ValueAsString() != "INVALID")
                            {
                                decimal StackLoadTimesOpTimeAccumulator = Category.GetCheckParameter("Stack_Loadtimesoptime_Accumulator").ValueAsDecimal();
                                decimal PipeLoadTimesOpTimeAccumulator = Category.GetCheckParameter("Pipe_LoadTimesOpTime_Accumulator").ValueAsDecimal();
                                decimal UnitLoadTimesOpTimeAccumulator = Category.GetCheckParameter("Unit_Loadtimesoptime_Accumulator").ValueAsDecimal();
                                decimal[] ApportionmentCalcHiArray = Category.GetCheckParameter("Apportionment_Calc_Hi_Array").ValueAsDecimalArray();

                                if ((MpPipeConfigForHourlyChecks != "CP") && (MpStackConfigForHourlyChecks == "CS") &&
                                         (StackLoadTimesOpTimeAccumulator > 0) && (UnitLoadTimesOpTimeAccumulator > 0) &&
                                         (Math.Abs(StackLoadTimesOpTimeAccumulator - UnitLoadTimesOpTimeAccumulator) >= NumberOfItems))
                                    Category.CheckCatalogResult = "C";
                                else if ((Category.GetCheckParameter("Config_HeatInputTimesOpTime_Accumulator").ValueAsDecimal() >= 0) &&
                                         (ApportionmentLoadArray[Category.CurrentMonLocPos] >= 0) &&
                                         (UnitLoadTimesOpTimeAccumulator >= 0) &&
                                         (ApportionmentCalcHiArray[Category.CurrentMonLocPos] >= 0))
                                    Category.SetCheckParameter("Calculate_Apportioned_HI", true, eParameterDataType.Boolean);
                                if (UnitLoadTimesOpTimeAccumulator == 0)
                                    Category.SetArrayParameter("Apportionment_HI_Method_Array", Category.CurrentMonLocPos, "NOCALC");
                            }
                        }

                        //Equation F-21D
                        else if ((MpStackConfigForHourlyChecks.StartsWith("CS") || MpPipeConfigForHourlyChecks == "CP" || MpPipeConfigForHourlyChecks == "MULTIPLE") &&
                                 ApportionmentHiMethodArray[Category.CurrentMonLocPos] != "COMPLEX")
                        {
                            if (Category.GetCheckParameter("Apportionment_NOX_Method_Array").ValueAsStringArray()[Category.CurrentMonLocPos] == "NOXR")
                                Category.SetCheckParameter("Calculate_NOXM_From_Apportioned_HI", true, eParameterDataType.Boolean);

                            if (Category.GetCheckParameter("Max_Stack_Optime").ValueAsDecimal()
                                  > (Category.GetCheckParameter("Unit_Optime_Accumulator").ValueAsDecimal()
                                     + (Category.GetCheckParameter("Current_Unit_Count").AsInteger(0) * 0.005m))
                                && MpPipeConfigForHourlyChecks == "")
                                Category.CheckCatalogResult = "B";
                            else if ((Category.GetCheckParameter("Config_HeatInputTimesOpTime_Accumulator").ValueAsDecimal() == 0 &&
                                Category.GetCheckParameter("Unit_HeatInputTimesOpTime_Accumulator").ValueAsDecimal() > 0) ||
                                (Category.GetCheckParameter("Config_HeatInputTimesOpTime_Accumulator").ValueAsDecimal() > 0 &&
                                Category.GetCheckParameter("Unit_HeatInputTimesOpTime_Accumulator").ValueAsDecimal() == 0))
                                Category.CheckCatalogResult = "G";
                            else if ((Category.GetCheckParameter("Config_HeatInputTimesOpTime_Accumulator").ValueAsDecimal() >= 0) &&
                                     (Category.GetCheckParameter("Unit_HeatInputTimesOpTime_Accumulator").ValueAsDecimal() >= 0))
                                Category.SetCheckParameter("Calculate_Apportioned_HI", true, eParameterDataType.Boolean);
                        }

                        //COMPLEX
                        else if (MpStackConfigForHourlyChecks == "COMPLEX" ||
                          ApportionmentHiMethodArray[Category.CurrentMonLocPos] == "COMPLEX")
                        {
                            if (Category.GetCheckParameter("Apportionment_NOX_Method_Array").ValueAsStringArray()[Category.CurrentMonLocPos] == "NOXR")
                                Category.SetCheckParameter("Calculate_NOXM_From_Apportioned_HI", true, eParameterDataType.Boolean);

                            if ((Category.GetCheckParameter("Config_HeatInputTimesOpTime_Accumulator").ValueAsDecimal() == 0 &&
                                Category.GetCheckParameter("Unit_HeatInputTimesOpTime_Accumulator").ValueAsDecimal() > 0) ||
                                (Category.GetCheckParameter("Config_HeatInputTimesOpTime_Accumulator").ValueAsDecimal() > 0 &&
                                Category.GetCheckParameter("Unit_HeatInputTimesOpTime_Accumulator").ValueAsDecimal() == 0))
                                Category.CheckCatalogResult = "G";
                            else if ((Category.GetCheckParameter("Config_HeatInputTimesOpTime_Accumulator").ValueAsDecimal() >= 0) &&
                                (Category.GetCheckParameter("Unit_HeatInputTimesOpTime_Accumulator").ValueAsDecimal() >= 0))
                                Category.SetCheckParameter("Calculate_Apportioned_HI", true, eParameterDataType.Boolean);
                        }

                        //Equation F-21C
                        else if (MpStackConfigForHourlyChecks == "MS")
                        {
                            if (Category.GetCheckParameter("Apportionment_NOX_Method_Array").ValueAsStringArray()[Category.CurrentMonLocPos] == "NOXR")
                                Category.SetCheckParameter("Calculate_NOXM_From_Apportioned_HI", true, eParameterDataType.Boolean);

                            if (Category.GetCheckParameter("Config_HeatInputTimesOpTime_Accumulator").ValueAsDecimal() >= 0)
                                Category.SetCheckParameter("Calculate_Apportioned_HI", true, eParameterDataType.Boolean);

                            if (ApportionmentOpTimeArray[Category.CurrentMonLocPos]
                                < Category.GetCheckParameter("Max_Stack_Optime").ValueAsDecimal())
                                Category.CheckCatalogResult = "D";
                            else if (ApportionmentOpTimeArray[Category.CurrentMonLocPos]
                                     > Category.GetCheckParameter("Stack_Optime_Accumulator").ValueAsDecimal())
                                Category.CheckCatalogResult = "E";
                        }
                    }
                }
                else
                {
                    if (CurrentHiEntityType != "Unit" && ApportionmentOpTimeArray[Category.CurrentMonLocPos] > 0)
                    {
                        decimal TotalUnitOpTime = 0;
                        string StackUnitList = Category.GetCheckParameter("Apportionment_Stack_Unit_List").ValueAsStringArray()[Category.CurrentMonLocPos];
                        for (int unitcount = 0; unitcount < StackUnitList.ListCount(); unitcount++)
                        {
                            TotalUnitOpTime = TotalUnitOpTime + ApportionmentOpTimeArray[cDBConvert.ToInteger(StackUnitList.ListItem(unitcount))];
                        }
                        if (TotalUnitOpTime == 0)
                            if (CurrentHiEntityType.StartsWith("C"))
                                Category.CheckCatalogResult = "B";
                            else
                                Category.CheckCatalogResult = "D";
                    }
                    if (string.IsNullOrEmpty(Category.CheckCatalogResult) && (CurrentHiEntityType == "MS") &&
                           (Category.GetCheckParameter("MP_Load_UOM").ValueAsString() != "INVALID") &&
                           (Category.GetCheckParameter("MP_Unit_Load").ValueAsInt() > 0) &&
                           (ApportionmentLoadArray[Category.CurrentMonLocPos] > 0))
                    {
                        if (Category.GetCheckParameter("MP_Unit_Load").ValueAsInt() != ApportionmentLoadArray[Category.CurrentMonLocPos])
                            Category.CheckCatalogResult = "F";
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURAPP2");
            }

            return ReturnVal;
        }

        public static string HOURAPP3(cCategory Category, ref bool Log)
        // Calculate Apportioned or Summed Heat Input Rate
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentHiApportionmentRecord = null;

                Category.SetCheckParameter("Current_HI_Apportionment_Record", CurrentHiApportionmentRecord, eParameterDataType.DataRowView);
                Category.SetCheckParameter("HI_Calculated_Apportioned_Value", null, eParameterDataType.Decimal);

                string[] ApportionmentHiMethodArray = Category.GetCheckParameter("Apportionment_HI_Method_Array").ValueAsStringArray();
                string[] ApportionmentNoxrMethodArray = Category.GetCheckParameter("Apportionment_NOXR_Method_Array").ValueAsStringArray();
                Category.SetCheckParameter("APP_E_Checks_Needed", false, eParameterDataType.Boolean);


                if ((ApportionmentHiMethodArray[Category.CurrentMonLocPos] != null) &&
                    (ApportionmentHiMethodArray[Category.CurrentMonLocPos].Contains("CALC") ||
                     (ApportionmentHiMethodArray[Category.CurrentMonLocPos] == "COMPLEX")))
                {
                    decimal HiTolerance = decimal.MinValue;

                    bool CalculateApportionedHi = Category.GetCheckParameter("Calculate_Apportioned_HI").ValueAsBool();

                    if (CalculateApportionedHi)
                    {
                        DataView HiDerivedHourlyValueRecords = Category.GetCheckParameter("HI_Derived_Hourly_Value_Records_By_Hour_Location").ValueAsDataView();
                        sFilterPair[] DHVRecordsFilter = new sFilterPair[1];
                        DHVRecordsFilter[0].Set("PARAMETER_CD", "HI");
                        DataView HiDerivedHourlyValueRecordsFound = FindRows(HiDerivedHourlyValueRecords, DHVRecordsFilter);
                        if (HiDerivedHourlyValueRecordsFound.Count == 1)
                        {
                            CurrentHiApportionmentRecord = HiDerivedHourlyValueRecordsFound[0];
                            Category.SetCheckParameter("Current_HI_Apportionment_Record", CurrentHiApportionmentRecord, eParameterDataType.DataRowView);

                            HiTolerance = GetTolerance("HI", "MMBTUHR", Category);
                        }
                        else
                        {
                            CalculateApportionedHi = false;
                            Category.SetCheckParameter("Calculate_Apportioned_HI", CalculateApportionedHi, eParameterDataType.Boolean);
                        }
                    }

                    decimal[] ApportionmentOpTimeArray = Category.GetCheckParameter("Apportionment_Optime_Array").ValueAsDecimalArray();
                    string CurrentHiEntityType = Category.GetCheckParameter("Current_HI_Entity_Type").ValueAsString();
                    string MpStackConfigForHourlyChecks = Category.GetCheckParameter("MP_Stack_Config_For_Hourly_Checks").ValueAsString();
                    string MpPipeConfigForHourlyChecks = Category.GetCheckParameter("MP_Pipe_Config_for_Hourly_Checks").ValueAsString();

                    //Equation F-25
                    if ((MpStackConfigForHourlyChecks == "CS") && (CurrentHiEntityType == "CS"))
                    {
                        if (CalculateApportionedHi)
                        {
                            decimal ConfigHeatInputTimesOpTimeAccumulator = Category.GetCheckParameter("Config_HeatInputTimesOpTime_Accumulator").ValueAsDecimal();
                            decimal[] RptPeriodHiCalculatedAccumulator = Category.GetCheckParameter("Rpt_Period_Hi_Calculated_Accumulator_Array").ValueAsDecimalArray();
                            decimal StackOpTimeAccumulator = Category.GetCheckParameter("Stack_Optime_Accumulator").ValueAsDecimal();

                            // Calculate HI Apportioned Value
                            decimal HiCalculatedApportionedValue = Math.Round(ConfigHeatInputTimesOpTimeAccumulator / ApportionmentOpTimeArray[Category.CurrentMonLocPos], 1, MidpointRounding.AwayFromZero);
                            Category.SetCheckParameter("HI_Calculated_Apportioned_Value", HiCalculatedApportionedValue, eParameterDataType.Decimal);

                            // Accumulate Reporting Period Value
                            if ((Category.GetCheckParameter("Current_Month").ValueAsString() != "April") ||
                                Category.GetCheckParameter("Annual_Reporting_Requirement").ValueAsBool())
                            {
                                if (RptPeriodHiCalculatedAccumulator[Category.CurrentMonLocPos] >= 0)
                                {
                                    RptPeriodHiCalculatedAccumulator[Category.CurrentMonLocPos] += (HiCalculatedApportionedValue
                                                                                                      * ApportionmentOpTimeArray[Category.CurrentMonLocPos]);
                                    Category.SetArrayParameter("Rpt_Period_Hi_Calculated_Accumulator_Array", RptPeriodHiCalculatedAccumulator);
                                }

                                if (Category.GetCheckParameter("Current_Month").ValueAsString() == "April")
                                {
                                    Category.AccumCheckAggregate("April_HI_Calculated_Accumulator_Array",
                                                                 Category.CurrentMonLocPos,
                                                                 (HiCalculatedApportionedValue * ApportionmentOpTimeArray[Category.CurrentMonLocPos]));
                                }
                            }

                            // Compare Against Tolerance
                            decimal AdjustedHourlyValue = cDBConvert.ToDecimal(CurrentHiApportionmentRecord["ADJUSTED_HRLY_VALUE"]);

                            if ((AdjustedHourlyValue >= 0) &&
                                (Math.Abs(AdjustedHourlyValue - HiCalculatedApportionedValue) > HiTolerance))
                                Category.CheckCatalogResult = "A";
                        }
                        else if (ApportionmentOpTimeArray[Category.CurrentMonLocPos] != 0)
                        {
                            if ((Category.GetCheckParameter("Current_Month").ValueAsString() != "April") ||
                                Category.GetCheckParameter("Annual_Reporting_Requirement").ValueAsBool())
                                Category.SetArrayParameter("Rpt_Period_Hi_Calculated_Accumulator_Array", Category.CurrentMonLocPos, -1.0m);

                            Category.CheckCatalogResult = "B";
                        }
                    }
                    // Other Complex
                    else if (CurrentHiEntityType.ToUpper() != "UNIT")
                    {
                        decimal AdjustedHourlyValue = decimal.MinValue;
                        if (CalculateApportionedHi)
                            AdjustedHourlyValue = cDBConvert.ToDecimal(CurrentHiApportionmentRecord["ADJUSTED_HRLY_VALUE"]);

                        if (CalculateApportionedHi && AdjustedHourlyValue >= 0)
                        {
                            // Set HI Apportioned Value
                            decimal HiCalculatedApportionedValue = AdjustedHourlyValue;
                            Category.SetCheckParameter("HI_Calculated_Apportioned_Value", HiCalculatedApportionedValue, eParameterDataType.Decimal);

                            // Accumulate Reporting Period Value
                            decimal[] RptPeriodHiCalculatedAccumulator = Category.GetCheckParameter("Rpt_Period_Hi_Calculated_Accumulator_Array").ValueAsDecimalArray();

                            if ((Category.GetCheckParameter("Current_Month").ValueAsString() != "April") ||
                                Category.GetCheckParameter("Annual_Reporting_Requirement").ValueAsBool())
                            {
                                if (RptPeriodHiCalculatedAccumulator[Category.CurrentMonLocPos] >= 0)
                                {
                                    RptPeriodHiCalculatedAccumulator[Category.CurrentMonLocPos] += (HiCalculatedApportionedValue
                                                                                                      * ApportionmentOpTimeArray[Category.CurrentMonLocPos]);
                                    Category.SetArrayParameter("Rpt_Period_Hi_Calculated_Accumulator_Array", RptPeriodHiCalculatedAccumulator);
                                }

                                if (Category.GetCheckParameter("Current_Month").ValueAsString() == "April")
                                {
                                    Category.AccumCheckAggregate("April_HI_Calculated_Accumulator_Array",
                                                                 Category.CurrentMonLocPos,
                                                                 (HiCalculatedApportionedValue * ApportionmentOpTimeArray[Category.CurrentMonLocPos]));
                                }
                            }
                        }
                        else if (ApportionmentOpTimeArray[Category.CurrentMonLocPos] != 0)
                        {
                            if ((Category.GetCheckParameter("Current_Month").ValueAsString() != "April") ||
                                Category.GetCheckParameter("Annual_Reporting_Requirement").ValueAsBool())
                                Category.SetArrayParameter("Rpt_Period_Hi_Calculated_Accumulator_Array", Category.CurrentMonLocPos, -1.0m);

                            Category.CheckCatalogResult = "B";
                        }
                    }
                    //Equation F-21A/B
                    else if (((MpStackConfigForHourlyChecks == "CS") || (MpPipeConfigForHourlyChecks == "CP")) &&
                             !ApportionmentHiMethodArray[Category.CurrentMonLocPos].InList("NOCALC,COMPLEX"))
                    {

                        if (Category.GetCheckParameter("Apportionment_NOXR_Method_Array").ValueAsStringArray()[Category.CurrentMonLocPos] == "AE")
                            Category.SetCheckParameter("APP_E_Checks_Needed", true, eParameterDataType.Boolean);

                        if (CalculateApportionedHi)
                        {
                            decimal UnitLoadTimesOpTimeAccumulator = Category.GetCheckParameter("Unit_Loadtimesoptime_Accumulator").ValueAsDecimal();
                            decimal AdjustedHourlyValue = cDBConvert.ToDecimal(CurrentHiApportionmentRecord["ADJUSTED_HRLY_VALUE"]);
                            if (UnitLoadTimesOpTimeAccumulator > 0 || AdjustedHourlyValue >= 0)
                            {
                                int[] ApportionmentLoadArray = Category.GetCheckParameter("Apportionment_Load_Array").ValueAsIntArray();
                                decimal ConfigHeatInputTimesOpTimeAccumulator = Category.GetCheckParameter("Config_HeatInputTimesOpTime_Accumulator").ValueAsDecimal();
                                decimal HiCalculatedApportionedValue = 0;
                                // Calculate HI Apportioned Value
                                if (UnitLoadTimesOpTimeAccumulator > 0)
                                    HiCalculatedApportionedValue = Math.Round((ConfigHeatInputTimesOpTimeAccumulator
                                                                                     * ApportionmentOpTimeArray[Category.CurrentMonLocPos]
                                                                                     * ApportionmentLoadArray[Category.CurrentMonLocPos]
                                                                                     / UnitLoadTimesOpTimeAccumulator)
                                                                                    / ApportionmentOpTimeArray[Category.CurrentMonLocPos], 1, MidpointRounding.AwayFromZero);
                                else
                                    HiCalculatedApportionedValue = AdjustedHourlyValue;

                                HiCalculatedApportionedValue += Category.GetCheckParameter("Apportionment_Calc_Hi_Array").ValueAsDecimalArray()[Category.CurrentMonLocPos];
                                Category.SetCheckParameter("HI_Calculated_Apportioned_Value", HiCalculatedApportionedValue, eParameterDataType.Decimal);

                                // Accumulate Reporting Period Value
                                decimal[] RptPeriodHiCalculatedAccumulator = Category.GetCheckParameter("Rpt_Period_Hi_Calculated_Accumulator_Array").ValueAsDecimalArray();

                                if ((Category.GetCheckParameter("Current_Month").ValueAsString() != "April") ||
                                    Category.GetCheckParameter("Annual_Reporting_Requirement").ValueAsBool())
                                {
                                    if (RptPeriodHiCalculatedAccumulator[Category.CurrentMonLocPos] >= 0)
                                    {
                                        RptPeriodHiCalculatedAccumulator[Category.CurrentMonLocPos] += (HiCalculatedApportionedValue
                                                                                                          * ApportionmentOpTimeArray[Category.CurrentMonLocPos]);
                                        Category.SetArrayParameter("Rpt_Period_Hi_Calculated_Accumulator_Array", RptPeriodHiCalculatedAccumulator);
                                    }

                                    if (Category.GetCheckParameter("Current_Month").ValueAsString() == "April")
                                    {
                                        Category.AccumCheckAggregate("April_HI_Calculated_Accumulator_Array",
                                                                     Category.CurrentMonLocPos,
                                                                     (HiCalculatedApportionedValue * ApportionmentOpTimeArray[Category.CurrentMonLocPos]));
                                    }
                                }

                                // Compare Against Tolerance

                                if ((AdjustedHourlyValue >= 0) &&
                                    (Math.Abs(AdjustedHourlyValue - HiCalculatedApportionedValue) > HiTolerance))
                                    Category.CheckCatalogResult = "A";
                            }
                            else if (ApportionmentOpTimeArray[Category.CurrentMonLocPos] != 0)
                            {
                                if ((Category.GetCheckParameter("Current_Month").ValueAsString() != "April") ||
                                    Category.GetCheckParameter("Annual_Reporting_Requirement").ValueAsBool())
                                    Category.SetArrayParameter("Rpt_Period_Hi_Calculated_Accumulator_Array", Category.CurrentMonLocPos, -1.0m);

                                Category.CheckCatalogResult = "B";
                            }
                        }
                        else if (ApportionmentOpTimeArray[Category.CurrentMonLocPos] != 0)
                        {
                            if ((Category.GetCheckParameter("Current_Month").ValueAsString() != "April") ||
                                Category.GetCheckParameter("Annual_Reporting_Requirement").ValueAsBool())
                                Category.SetArrayParameter("Rpt_Period_Hi_Calculated_Accumulator_Array", Category.CurrentMonLocPos, -1.0m);

                            Category.CheckCatalogResult = "B";
                        }
                    }

                    //Equation F-21D or Unknown apportionment method or COMPLEX configuration
                    else if (MpStackConfigForHourlyChecks.StartsWith("CS") || MpPipeConfigForHourlyChecks == "CP" ||
                          MpStackConfigForHourlyChecks.StartsWith("COMPLEX") || MpPipeConfigForHourlyChecks == "MULTIPLE")
                    {
                        if (Category.GetCheckParameter("Apportionment_NOXR_Method_Array").ValueAsStringArray()[Category.CurrentMonLocPos] == "AE")
                            Category.SetCheckParameter("APP_E_Checks_Needed", true, eParameterDataType.Boolean);
                        decimal ConfigHeatInputTimesOpTimeAccumulator = Category.GetCheckParameter("Config_HeatInputTimesOpTime_Accumulator").ValueAsDecimal();

                        if (CalculateApportionedHi)
                        {
                            decimal UnitHeatInputTimesOpTimeAccumulator = Category.GetCheckParameter("Unit_HeatInputTimesOpTime_Accumulator").ValueAsDecimal();

                            if (Math.Abs(ConfigHeatInputTimesOpTimeAccumulator - UnitHeatInputTimesOpTimeAccumulator) <= HiTolerance ||
                               ApportionmentHiMethodArray[Category.CurrentMonLocPos] == "COMPLEX" ||
                               (MpStackConfigForHourlyChecks == "COMPLEX" && MpPipeConfigForHourlyChecks == ""))

                            {

                                // Set HI Apportioned Value
                                decimal AdjustedHourlyValue = cDBConvert.ToDecimal(CurrentHiApportionmentRecord["ADJUSTED_HRLY_VALUE"]);
                                decimal HiCalculatedApportionedValue = AdjustedHourlyValue;
                                Category.SetCheckParameter("HI_Calculated_Apportioned_Value", HiCalculatedApportionedValue, eParameterDataType.Decimal);

                                // Accumulate Reporting Period Value
                                decimal[] RptPeriodHiCalculatedAccumulator = Category.GetCheckParameter("Rpt_Period_Hi_Calculated_Accumulator_Array").ValueAsDecimalArray();

                                if ((Category.GetCheckParameter("Current_Month").ValueAsString() != "April") ||
                                    Category.GetCheckParameter("Annual_Reporting_Requirement").ValueAsBool())
                                {
                                    if (RptPeriodHiCalculatedAccumulator[Category.CurrentMonLocPos] >= 0)
                                    {
                                        RptPeriodHiCalculatedAccumulator[Category.CurrentMonLocPos] += (HiCalculatedApportionedValue
                                                                                                          * ApportionmentOpTimeArray[Category.CurrentMonLocPos]);
                                        Category.SetArrayParameter("Rpt_Period_Hi_Calculated_Accumulator_Array", RptPeriodHiCalculatedAccumulator);
                                    }

                                    if (Category.GetCheckParameter("Current_Month").ValueAsString() == "April")
                                    {
                                        Category.AccumCheckAggregate("April_HI_Calculated_Accumulator_Array",
                                                                     Category.CurrentMonLocPos,
                                                                     (HiCalculatedApportionedValue * ApportionmentOpTimeArray[Category.CurrentMonLocPos]));
                                    }
                                }
                            }
                            else
                            {
                                if ((Category.GetCheckParameter("Current_Month").ValueAsString() != "April") ||
                                    Category.GetCheckParameter("Annual_Reporting_Requirement").ValueAsBool())
                                    Category.SetArrayParameter("Rpt_Period_Hi_Calculated_Accumulator_Array", Category.CurrentMonLocPos, -1.0m);

                                Category.CheckCatalogResult = "C";
                            }
                        }
                        else if (ApportionmentOpTimeArray[Category.CurrentMonLocPos] != 0)
                        {
                            if ((Category.GetCheckParameter("Current_Month").ValueAsString() != "April") ||
                                Category.GetCheckParameter("Annual_Reporting_Requirement").ValueAsBool())
                                Category.SetArrayParameter("Rpt_Period_Hi_Calculated_Accumulator_Array", Category.CurrentMonLocPos, -1.0m);

                            DataView HiDerivedHourlyValueRecords = Category.GetCheckParameter("HI_Derived_Hourly_Value_Records_By_Hour_Location").ValueAsDataView();
                            sFilterPair[] DHVRecordsFilter = new sFilterPair[1];
                            DHVRecordsFilter[0].Set("PARAMETER_CD", "HI");
                            DataView HiDerivedHourlyValueRecordsFound = FindRows(HiDerivedHourlyValueRecords, DHVRecordsFilter);
                            if (HiDerivedHourlyValueRecordsFound.Count == 1)
                            {
                                CurrentHiApportionmentRecord = HiDerivedHourlyValueRecordsFound[0];
                                Category.SetCheckParameter("Current_HI_Apportionment_Record", CurrentHiApportionmentRecord, eParameterDataType.DataRowView);
                                decimal AdjustedHourlyValue = cDBConvert.ToDecimal(CurrentHiApportionmentRecord["ADJUSTED_HRLY_VALUE"]);
                                if (AdjustedHourlyValue > 0 && ConfigHeatInputTimesOpTimeAccumulator == 0)
                                    Category.CheckCatalogResult = "D";
                                else
                                    Category.CheckCatalogResult = "B";
                            }
                            else
                                Category.CheckCatalogResult = "B";
                        }
                    }

                    //Equation F-21C
                    else if (MpStackConfigForHourlyChecks == "MS")
                    {
                        if (CalculateApportionedHi)
                        {
                            decimal ConfigHeatInputTimesOpTimeAccumulator = Category.GetCheckParameter("Config_HeatInputTimesOpTime_Accumulator").ValueAsDecimal();
                            decimal[] RptPeriodHiCalculatedAccumulator = Category.GetCheckParameter("Rpt_Period_Hi_Calculated_Accumulator_Array").ValueAsDecimalArray();
                            decimal UnitOpTimeAccumulator = Category.GetCheckParameter("Unit_Optime_Accumulator").ValueAsDecimal();

                            // Calculate HI Apportioned Value
                            decimal HiCalculatedApportionedValue = Math.Round(ConfigHeatInputTimesOpTimeAccumulator
                                                                              / UnitOpTimeAccumulator, 1, MidpointRounding.AwayFromZero);
                            Category.SetCheckParameter("HI_Calculated_Apportioned_Value", HiCalculatedApportionedValue, eParameterDataType.Decimal);

                            // Accumulate Reporting Period Value
                            if ((Category.GetCheckParameter("Current_Month").ValueAsString() != "April") ||
                                Category.GetCheckParameter("Annual_Reporting_Requirement").ValueAsBool())
                            {
                                if (RptPeriodHiCalculatedAccumulator[Category.CurrentMonLocPos] >= 0)
                                {
                                    RptPeriodHiCalculatedAccumulator[Category.CurrentMonLocPos] += (HiCalculatedApportionedValue
                                                                                                      * UnitOpTimeAccumulator);
                                    Category.SetArrayParameter("Rpt_Period_Hi_Calculated_Accumulator_Array", RptPeriodHiCalculatedAccumulator);
                                }

                                if (Category.GetCheckParameter("Current_Month").ValueAsString() == "April")
                                {
                                    Category.AccumCheckAggregate("April_HI_Calculated_Accumulator_Array",
                                                                 Category.CurrentMonLocPos,
                                                                 (HiCalculatedApportionedValue * UnitOpTimeAccumulator));
                                }
                            }

                            // Compare Against Tolerance
                            decimal AdjustedHourlyValue = cDBConvert.ToDecimal(CurrentHiApportionmentRecord["ADJUSTED_HRLY_VALUE"]);

                            if ((AdjustedHourlyValue >= 0) &&
                                (Math.Abs(AdjustedHourlyValue - HiCalculatedApportionedValue) > HiTolerance))
                                Category.CheckCatalogResult = "A";
                        }
                        else if (ApportionmentOpTimeArray[Category.CurrentMonLocPos] != 0)
                        {
                            if ((Category.GetCheckParameter("Current_Month").ValueAsString() != "April") ||
                                Category.GetCheckParameter("Annual_Reporting_Requirement").ValueAsBool())
                                Category.SetArrayParameter("Rpt_Period_Hi_Calculated_Accumulator_Array", Category.CurrentMonLocPos, -1.0m);

                            Category.CheckCatalogResult = "B";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURAPP3");
            }

            return ReturnVal;
        }

        public static string HOURAPP4(cCategory Category, ref bool Log)
        // Calculate NOx Mass Rate from Apportioned or Summed Heat Input Rate 
        {
            string ReturnVal = "";

            try
            {
                Category.SetCheckParameter("Current_NOX_Apportionment_Based_Record", null, eParameterDataType.DataRowView);
                Category.SetCheckParameter("NOX_Calculated_Apportionment_Based_Value", null, eParameterDataType.Decimal);
                bool CalculateNOXM = Category.GetCheckParameter("Calculate_NOXM_From_Apportioned_HI").ValueAsBool();

                if (CalculateNOXM)
                {
                    decimal[] ApportionmentCalcNoxrArray = Category.GetCheckParameter("Apportionment_Calc_NOXR_Array").ValueAsDecimalArray();
                    decimal[] ApportionmentOpTimeArray = Category.GetCheckParameter("Apportionment_Optime_Array").ValueAsDecimalArray();
                    decimal HiCalculatedApportionedValue = Category.GetCheckParameter("HI_Calculated_Apportioned_Value").ValueAsDecimal();
                    string CurrentMonth = Category.GetCheckParameter("Current_Month").ValueAsString();
                    bool AnnRptReq = Category.GetCheckParameter("Annual_Reporting_Requirement").ValueAsBool();

                    if ((HiCalculatedApportionedValue != decimal.MinValue) &&
                        (ApportionmentCalcNoxrArray[Category.CurrentMonLocPos] >= 0))
                    {
                        DataView NoxDerivedHourlyValueRecords = Category.GetCheckParameter("NOx_Derived_Hourly_Value_Records_By_Hour_Location").ValueAsDataView();

                        if (NoxDerivedHourlyValueRecords.Count == 1)
                        {
                            // Set NOx Apportionment Based Record
                            DataRowView CurrentNoxApportionmentBasedRecord = NoxDerivedHourlyValueRecords[0];
                            Category.SetCheckParameter("Current_NOX_Apportionment_Based_Record", CurrentNoxApportionmentBasedRecord, eParameterDataType.DataRowView);

                            // Calculate
                            decimal NoxCalculatedApportionedBasedValue = Math.Round(HiCalculatedApportionedValue * ApportionmentCalcNoxrArray[Category.CurrentMonLocPos], 1, MidpointRounding.AwayFromZero);
                            Category.SetCheckParameter("NOX_Calculated_Apportionment_Based_Value", NoxCalculatedApportionedBasedValue, eParameterDataType.Decimal);

                            // Update Report Period Value
                            decimal[] RptPeriodNoxmCalculatedAccumulatorArray = Category.GetCheckParameter("Rpt_Period_Nox_Mass_Calculated_Accumulator_Array").ValueAsDecimalArray();

                            if (CurrentMonth != "April" || AnnRptReq)
                            {
                                decimal ApportionmentOpTime = ApportionmentOpTimeArray[Category.CurrentMonLocPos];

                                if ((0m <= ApportionmentOpTime) && (ApportionmentOpTime <= 1m))
                                {
                                    if (RptPeriodNoxmCalculatedAccumulatorArray[Category.CurrentMonLocPos] >= 0)
                                    {
                                        RptPeriodNoxmCalculatedAccumulatorArray[Category.CurrentMonLocPos]
                                          += (NoxCalculatedApportionedBasedValue * ApportionmentOpTime);

                                        Category.SetArrayParameter("Rpt_Period_Nox_Mass_Calculated_Accumulator_Array", RptPeriodNoxmCalculatedAccumulatorArray);
                                    }

                                    if (CurrentMonth == "April")
                                    {
                                        decimal[] AprilNoxmCalculatedAccumulatorArray = Category.GetCheckParameter("April_NOX_Mass_Calculated_Accumulator_Array").ValueAsDecimalArray();

                                        AprilNoxmCalculatedAccumulatorArray[Category.CurrentMonLocPos]
                                          += (NoxCalculatedApportionedBasedValue * ApportionmentOpTime);

                                        Category.SetArrayParameter("April_NOX_Mass_Calculated_Accumulator_Array", AprilNoxmCalculatedAccumulatorArray);
                                    }
                                }
                            }
                            // Check Against Tolerance
                            decimal AdjustedHourlyValue = cDBConvert.ToDecimal(CurrentNoxApportionmentBasedRecord["ADJUSTED_HRLY_VALUE"]);
                            decimal Tolerance = GetTolerance("NOX", "LBHR", Category);
                            if ((AdjustedHourlyValue >= 0) &&
                                (Math.Abs(AdjustedHourlyValue - NoxCalculatedApportionedBasedValue) > Tolerance))
                                if (!Category.GetCheckParameter("Legacy_Data_Evaluation").ValueAsBool())
                                    Category.CheckCatalogResult = "A";
                                else
                                {
                                    if (0 < ApportionmentOpTimeArray[Category.CurrentMonLocPos] && ApportionmentOpTimeArray[Category.CurrentMonLocPos] <= 1)
                                    {
                                        DataRowView HiAppRec = Category.GetCheckParameter("Current_HI_Apportionment_Record").ValueAsDataRowView();
                                        if (Math.Abs(cDBConvert.ToDecimal(HiAppRec["ADJUSTED_HRLY_VALUE"]) - NoxCalculatedApportionedBasedValue) >
                                            Tolerance / ApportionmentOpTimeArray[Category.CurrentMonLocPos])
                                            Category.CheckCatalogResult = "A";
                                    }
                                }
                        }
                        else
                        {
                            if (CurrentMonth != "April" || AnnRptReq)
                                Category.SetArrayParameter("Rpt_Period_Nox_Mass_Calculated_Accumulator_Array", Category.CurrentMonLocPos, -1.0m);
                            Category.CheckCatalogResult = "B";
                        }
                    }
                    else
                    {
                        if (CurrentMonth != "April" || AnnRptReq)
                            Category.SetArrayParameter("Rpt_Period_Nox_Mass_Calculated_Accumulator_Array", Category.CurrentMonLocPos, -1.0m);
                        Category.CheckCatalogResult = "B";
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURAPP4");
            }

            return ReturnVal;
        }

        public static string HOURAPP5(cCategory Category, ref bool Log)
        // Sum Weighted NOx Emission Rate from Multiple Stacks
        {
            string ReturnVal = "";

            try
            {
                if (Convert.ToString(Category.GetCheckParameter("MP_Stack_Config_For_Hourly_Checks").ParameterValue) == "MS" &&
                    Convert.ToString(Category.GetCheckParameter("Current_HI_Entity_Type").ParameterValue) == "Unit")
                {
                    decimal ConfigNoxHIAccum = Convert.ToDecimal(Category.GetCheckParameter("Config_NOxRateTimesHeatInput_Accumulator").ParameterValue);
                    decimal ConfigNoxOpTimeAccum = Convert.ToDecimal(Category.GetCheckParameter("Config_NOxRateTimesOptime_Accumulator").ParameterValue);
                    if (ConfigNoxHIAccum != 0 && ConfigNoxOpTimeAccum != 0)
                    {
                        int CurrentLocationPos = Category.GetCheckParameter("Current_Monitor_Plan_Location_Postion").ValueAsInt();
                        Category.AccumCheckAggregate("Expected_Summary_Value_Nox_Rate_Array", CurrentLocationPos, true, true);
                        decimal[] NOxRateCalcAccum = Category.GetCheckParameter("Rpt_Period_Nox_Rate_Calculated_Accumulator_Array").ValueAsDecimalArray();
                        int[] NOxRateHoursAccum = Category.GetCheckParameter("Rpt_Period_Nox_Rate_Hours_Accumulator_Array").ValueAsIntArray();
                        decimal ConfigHIAccum = Convert.ToDecimal(Category.GetCheckParameter("Config_HeatInput_Accumulator").ParameterValue);
                        if (ConfigNoxHIAccum > 0 && ConfigHIAccum > 0 && NOxRateCalcAccum[CurrentLocationPos] >= 0)
                        {
                            decimal Accum = NOxRateCalcAccum[CurrentLocationPos] + Convert.ToDecimal(Math.Round(ConfigNoxHIAccum / ConfigHIAccum, 3, MidpointRounding.AwayFromZero));
                            Category.SetArrayParameter("Rpt_Period_Nox_Rate_Calculated_Accumulator_Array", CurrentLocationPos, Accum);
                            Category.SetArrayParameter("Rpt_Period_Nox_Rate_Hours_Accumulator_Array", CurrentLocationPos, NOxRateHoursAccum[CurrentLocationPos] + 1);
                        }
                        else
                        {
                            decimal ConfigOpTimeAccum = Convert.ToDecimal(Category.GetCheckParameter("Config_Optime_Accumulator").ParameterValue);
                            if (ConfigNoxOpTimeAccum > 0 && ConfigOpTimeAccum > 0 && NOxRateCalcAccum[CurrentLocationPos] >= 0)
                            {
                                decimal Accum = NOxRateCalcAccum[CurrentLocationPos] + Convert.ToDecimal(Math.Round(ConfigNoxOpTimeAccum / ConfigOpTimeAccum, 3, MidpointRounding.AwayFromZero));
                                Category.SetArrayParameter("Rpt_Period_Nox_Rate_Calculated_Accumulator_Array", CurrentLocationPos, Accum);
                                Category.SetArrayParameter("Rpt_Period_Nox_Rate_Hours_Accumulator_Array", CurrentLocationPos, NOxRateHoursAccum[CurrentLocationPos] + 1);
                            }
                            else
                                Category.SetArrayParameter("Rpt_Period_Nox_Rate_Calculated_Accumulator_Array", CurrentLocationPos, -1m);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURAPP5");
            }

            return ReturnVal;
        }

        public static string HOURAPP6(cCategory Category, ref bool Log)
        // Invitialize Values for calculatubg Appendix E Nox Rate from Apptnd HI
        {
            string ReturnVal = "";

            try
            {

                bool APPENeeded = Category.GetCheckParameter("APP_E_Checks_Needed").ValueAsBool();

                if (APPENeeded)
                {
                    Category.SetCheckParameter("App_E_Reporting_Method", "APPORTIONED", eParameterDataType.String);
                    DataView NoxrDerivedHourlyValueRecords = Category.GetCheckParameter("NOxR_Derived_Hourly_Value_Records_By_Hour_Location").ValueAsDataView();
                    Category.SetCheckParameter("App_E_Op_Code", null, eParameterDataType.String);

                    if (NoxrDerivedHourlyValueRecords.Count == 1)
                    {
                        // Set NOx Apportionment Based Record
                        DataRowView CurrentDhvRecord = NoxrDerivedHourlyValueRecords[0];
                        Category.SetCheckParameter("Current_NOXR_Apportionment_Based_Record", CurrentDhvRecord, eParameterDataType.DataRowView);

                        string MonitorSystemId = cDBConvert.ToString(CurrentDhvRecord["MON_SYS_ID"]);
                        string OperatingConditionCd = cDBConvert.ToString(CurrentDhvRecord["OPERATING_CONDITION_CD"]);
                        if (MonitorSystemId != "")
                        {
                            sFilterPair[] RowFilter;
                            RowFilter = new sFilterPair[1];
                            RowFilter[0].Set("MON_SYS_ID", MonitorSystemId);

                            DataView MonitorSystemRecords = Category.GetCheckParameter("Monitor_System_Records_By_Hour_Location").ValueAsDataView();
                            DataRowView MonSysRecord = FindRow(MonitorSystemRecords, RowFilter);

                            if (MonSysRecord != null && cDBConvert.ToString(MonSysRecord["SYS_TYPE_CD"]) == "NOXE" &&
                                  cDBConvert.ToString(MonSysRecord["FUEL_CD"]) != "")
                            {
                                if (OperatingConditionCd.InList("X,Y,Z,U,W,N,M"))
                                {
                                    Category.SetCheckParameter("App_E_Reported_Value", cDBConvert.ToDecimal(CurrentDhvRecord["ADJUSTED_HRLY_VALUE"]), eParameterDataType.Decimal);
                                    Category.SetCheckParameter("App_E_Segment_Number", cDBConvert.ToInteger(CurrentDhvRecord["SEGMENT_NUM"]), eParameterDataType.Integer);
                                    Category.SetCheckParameter("App_E_Fuel_Code", cDBConvert.ToString(MonSysRecord["FUEL_CD"]), eParameterDataType.String);
                                    Category.SetCheckParameter("App_E_NOXE_System_ID", MonitorSystemId, eParameterDataType.String);
                                    Category.SetCheckParameter("App_E_NOXE_System_Identifier", cDBConvert.ToString(CurrentDhvRecord["SYSTEM_IDENTIFIER"]), eParameterDataType.String);
                                    Category.SetCheckParameter("App_E_Op_Code", OperatingConditionCd, eParameterDataType.String);
                                    Category.SetCheckParameter("App_E_Calc_HI", Category.GetCheckParameter("HI_Calculated_Apportioned_Value").ValueAsDecimal(), eParameterDataType.Decimal);
                                    DateTime EarlyDate;
                                    DataRowView MPLRecord = Category.GetCheckParameter("Current_Monitor_Plan_Location_Record").ValueAsDataRowView();
                                    EarlyDate = cDBConvert.ToDate(MPLRecord["EARLIEST_REPORT_DATE"], DateTypes.START);
                                    Category.SetCheckParameter("Earliest_Location_Report_Date", EarlyDate, eParameterDataType.Date);

                                }
                                else if (OperatingConditionCd == "E")
                                    Category.CheckCatalogResult = "A";
                                else
                                    Category.CheckCatalogResult = "B";
                            }
                        }
                        else
                            Category.CheckCatalogResult = "C";
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "HOURAPP6");
            }

            return ReturnVal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="category"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public static string HOURAPP7(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                int currentMonitorPlanLocationPostion = category.GetCheckParameter("Current_Monitor_Plan_Location_Postion").ValueAsInt();
                bool[] expectedSummaryValueNoxRateArray = (bool[])category.GetCheckParameter("Expected_Summary_Value_Nox_Rate_Array").ParameterValue;

                if ((category.GetCheckParameter("Current_HI_Entity_Type").AsString() == "Unit") &&
                    (category.GetCheckParameter("MP_Stack_Config_For_Hourly_Checks").AsString() == "MS") &&
                    (expectedSummaryValueNoxRateArray[currentMonitorPlanLocationPostion] == false))
                {
                    DataRowView locationProgramRecord = cRowFilter.FindRow(category.GetCheckParameter("Location_Program_Records_By_Hour_Location").AsDataView(),
                                                                           new cFilterCondition[]
                                                                           {
                                                                   new cFilterCondition("PRG_CD", "ARP"),
                                                                   new cFilterCondition("CLASS", "P1,P2", eFilterConditionStringCompare.InList)
                                                                           });

                    if (locationProgramRecord != null)
                    {
                        category.AccumCheckAggregate("Expected_Summary_Value_Nox_Rate_Array", currentMonitorPlanLocationPostion, true, true);
                    }
                }
            }
            catch (Exception ex)
            {
                returnVal = category.CheckEngine.FormatError(ex);
            }

            return returnVal;
        }

        /// <summary>
        /// This checks the apportionment of load value to MS in MS configurations.  The load is apportioned based on stack flow.  
        /// The check calculates the apportioned value and compares it to the reported value with a tolerance determine by 
        /// HourlyEmissionsTolerances's LOAD-MW entry.
        /// </summary>
        /// <param name="category"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public static string HOURAPP9(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                EmParameters.CalculatedMatsMsLoad = null;

                if ((EmParameters.MpStackConfigForHourlyChecks == "MS") && (EmParameters.CurrentMonitorPlanLocationRecord.StackPipeId != null))
                {
                    int?[] apportionmentMatsLoadArray = (int?[])category.GetCheckParameter("Apportionment_MATS_Load_Array").ParameterValue;

                    int? currentMsLoad = apportionmentMatsLoadArray[EmParameters.CurrentMonitorPlanLocationPostion.Value];

                    if (currentMsLoad.HasValue)
                    {
                        decimal[] apportionmentOpTimeArray = (decimal[])category.GetCheckParameter("Apportionment_Optime_Array").ParameterValue;
                        decimal?[] apportionmentStackFlowArray = (decimal?[])category.GetCheckParameter("Apportionment_Stack_Flow_Array").ParameterValue;
                        string[] locationNameArray = (string[])category.GetCheckParameter("Location_Name_Array").ParameterValue;

                        bool msStackFlowNoNulls = true;
                        decimal msStackFlowSum = 0;
                        int? unitLoad = null;
                        {
                            for (int locationDex = 0; locationDex < apportionmentStackFlowArray.Length; locationDex++)
                            {
                                if (locationNameArray[locationDex].StartsWith("MS"))
                                {
                                    if (apportionmentStackFlowArray[locationDex] == null)
                                        msStackFlowNoNulls = false;
                                    else
                                        msStackFlowSum += apportionmentStackFlowArray[locationDex].Value * apportionmentOpTimeArray[locationDex];
                                }
                                else
                                    unitLoad = apportionmentMatsLoadArray[locationDex];
                            }
                        }


                        if (msStackFlowNoNulls)
                        {
                            decimal currentMsFlow = apportionmentStackFlowArray[EmParameters.CurrentMonitorPlanLocationPostion.Value].Value 
                                                  * apportionmentOpTimeArray[EmParameters.CurrentMonitorPlanLocationPostion.Value];

                            if ((msStackFlowSum > 0) && (unitLoad.HasValue && unitLoad.Value > 0))
                            {
                                EmParameters.CalculatedMatsMsLoad = (int)Math.Round((decimal)unitLoad.Value * currentMsFlow / msStackFlowSum, 0, MidpointRounding.AwayFromZero);

                                if (Math.Abs(currentMsLoad.Value - EmParameters.CalculatedMatsMsLoad.Value) > EmParameters.MwLoadHourlyTolerance.Value)
                                {
                                    category.CheckCatalogResult = "A";
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                returnVal = category.CheckEngine.FormatError(ex);
            }

            return returnVal;

        }

        /// <summary>
        /// Calculates and checks the MATS Hg flow weighted average rate when the source is using the MS-1 formula.
        /// </summary>
        /// <param name="category"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public static string HOURAPP10(cCategory category, ref bool log)
        {
            string ReturnVal = "";
			
            try
            {
                EmParameters.CalculatedFlowWeightedHg = null;
                EmParameters.MatsReportedPluginHg = null;

                //MatsMs1HgDhvId is being used to verify that MS-1 calculation is needed
                if ((EmParameters.MpStackConfigForHourlyChecks == "MS") && (EmParameters.CurrentMonitorPlanLocationRecord.UnitId != null) && (EmParameters.MatsMs1HgDhvId != null))
                {
                    decimal[] apportionmentOpTimeArray = (decimal[])category.GetCheckParameter("Apportionment_Optime_Array").ParameterValue;
					decimal?[] apportionmentStackFlowArray = (decimal?[])category.GetCheckParameter("Apportionment_Stack_Flow_Array").ParameterValue;
                    string[] apportionmentHgRateArray = (string[])category.GetCheckParameter("Apportionment_Hg_Rate_Array").ParameterValue;
                    string[] matsMS1HgModcCodeArray  = (string[])category.GetCheckParameter("MATS_MS1_Hg_MODC_Code_Array").ParameterValue;
					string[] locationNameArray = (string[])category.GetCheckParameter("Location_Name_Array").ParameterValue;
                    EmParameters.MatsReportedPluginHg = EmParameters.MatsMs1HgUnadjustedHourlyValue;

                    bool msStackFlowNoNulls = true;
                    bool modc38Used = false;
                    bool stackOperated = false;
                    bool stackMissingData = false;
                    decimal numOperatingStacks = 0;
                    decimal singleStackHgRate = 0;
                    decimal msStackFlowSum = 0;
                    decimal msStackEmissionRateFlow = 0;
                    {
                        for (int locationDex = 0; locationDex < apportionmentStackFlowArray.Length; locationDex++)
                        {
                            if (locationNameArray[locationDex].StartsWith("MS"))
                            {
                                //Set flag if any stack has an MODC of 38
								if (matsMS1HgModcCodeArray[locationDex] == "38") {
                                    modc38Used = true;
								}

                                //Set flag if any stack operated
                                if (apportionmentOpTimeArray[locationDex] > 0)
                                {
                                    stackOperated = true;
                                    numOperatingStacks += 1;
                                    singleStackHgRate = ECMPS.Definitions.Extensions.cExtensions.ScientificNotationtoDecimal(apportionmentHgRateArray[locationDex]);
                                }

                                //Set flag for stack missing data
                                if (apportionmentOpTimeArray[locationDex] > 0 && (apportionmentStackFlowArray[locationDex] == null || apportionmentHgRateArray[locationDex] == null))
                                {
                                    stackMissingData = true;
                                }

                                if (apportionmentStackFlowArray[locationDex] != null)
                                {
                                    msStackFlowSum += apportionmentStackFlowArray[locationDex].Value;
                                    msStackEmissionRateFlow += apportionmentStackFlowArray[locationDex].Value * ECMPS.Definitions.Extensions.cExtensions.ScientificNotationtoDecimal(apportionmentHgRateArray[locationDex]);
                                }
                            }
                        }
                    }

                    /* Valid data was reported at each operating stack but unit data was not provided */
                    if (!modc38Used && stackOperated && EmParameters.MatsMs1HgUnadjustedHourlyValue == null)
                    {
                        category.CheckCatalogResult = "B";
                    }
                    /* Validate the data for the special condition of only 1 stack operating */
                    else if (numOperatingStacks == 1)
                    {
                        EmParameters.CalculatedFlowWeightedHg = ECMPS.Definitions.Extensions.cExtensions.DecimaltoScientificNotation(singleStackHgRate);

                        //Convert the checking values to decimals from sci notation
                        decimal reportedValue = ECMPS.Definitions.Extensions.cExtensions.ScientificNotationtoDecimal(EmParameters.MatsMs1HgUnadjustedHourlyValue);

                        /* The rates must match when only 1 stack is operating */
                        if (singleStackHgRate != reportedValue)
                        {
                            category.CheckCatalogResult = "A";
                        }
                    }
                    /* Unit data was provided but valid data was not reported at each operating stack */
                    else if (stackMissingData && EmParameters.MatsMs1HgUnadjustedHourlyValue != null)
                    {
                        category.CheckCatalogResult = "C";
                    }
                    else if (msStackFlowSum > 0 && EmParameters.MatsMs1HgUnadjustedHourlyValue != null)
                    { //All good continue with check
                        EmParameters.CalculatedFlowWeightedHg = ECMPS.Definitions.Extensions.cExtensions.DecimaltoScientificNotation(msStackEmissionRateFlow / msStackFlowSum);

                        //Convert the checking values to decimals from sci notation
                        decimal reportedValue = ECMPS.Definitions.Extensions.cExtensions.ScientificNotationtoDecimal(EmParameters.MatsMs1HgUnadjustedHourlyValue);
                        decimal calculatedValue = ECMPS.Definitions.Extensions.cExtensions.ScientificNotationtoDecimal(EmParameters.CalculatedFlowWeightedHg);
                        if ((reportedValue + calculatedValue) != 0)
                        {
                            decimal percentDifference = Math.Round(100 * Math.Abs(reportedValue - calculatedValue) / ((reportedValue + calculatedValue) / 2), 1, MidpointRounding.AwayFromZero);

                            if (percentDifference > 5m)
                            {
                                category.CheckCatalogResult = "A";
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ReturnVal = category.CheckEngine.FormatError(ex, "HOURAPP10");
            }

            return ReturnVal;
        }

        /// <summary>
        /// Calculates and checks the MATS HCL flow weighted average rate when the source is using the MS-1 formula.
        /// </summary>
        /// <param name="category"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public static string HOURAPP11(cCategory category, ref bool log)
        {
            string ReturnVal = "";

            try
            {
                EmParameters.CalculatedFlowWeightedHcl = null;
                EmParameters.MatsReportedPluginHcl = null;

                //MatsMs1HclDhvId is being used to verify that MS-1 calculation is needed
                if ((EmParameters.MpStackConfigForHourlyChecks == "MS") && (EmParameters.CurrentMonitorPlanLocationRecord.UnitId != null) && (EmParameters.MatsMs1HclDhvId != null))
                {
                    decimal[] apportionmentOpTimeArray = (decimal[])category.GetCheckParameter("Apportionment_Optime_Array").ParameterValue;
                    decimal?[] apportionmentStackFlowArray = (decimal?[])category.GetCheckParameter("Apportionment_Stack_Flow_Array").ParameterValue;
                    string[] apportionmentHclRateArray = (string[])category.GetCheckParameter("Apportionment_HCL_Rate_Array").ParameterValue;
                    string[] matsMS1HclModcCodeArray = (string[])category.GetCheckParameter("MATS_MS1_HCL_MODC_Code_Array").ParameterValue;
                    string[] locationNameArray = (string[])category.GetCheckParameter("Location_Name_Array").ParameterValue;
                    EmParameters.MatsReportedPluginHcl = EmParameters.MatsMs1HclUnadjustedHourlyValue;

                    bool msStackFlowNoNulls = true;
                    bool modc38Used = false;
                    bool stackOperated = false;
                    bool stackMissingData = false;
                    decimal numOperatingStacks = 0;
                    decimal singleStackHclRate = 0;
                    decimal msStackFlowSum = 0;
                    decimal msStackEmissionRateFlow = 0;
                    {
                        for (int locationDex = 0; locationDex < apportionmentStackFlowArray.Length; locationDex++)
                        {
                            if (locationNameArray[locationDex].StartsWith("MS"))
                            {
                                //Set flag if any stack has an MODC of 38
                                if (matsMS1HclModcCodeArray[locationDex] == "38")
                                {
                                    modc38Used = true;
                                }

                                //Set flag if any stack operated
                                if (apportionmentOpTimeArray[locationDex] > 0)
                                {
                                    stackOperated = true;
                                    numOperatingStacks += 1;
                                    singleStackHclRate = ECMPS.Definitions.Extensions.cExtensions.ScientificNotationtoDecimal(apportionmentHclRateArray[locationDex]);
                                }

                                //Set flag for stack missing data
                                if (apportionmentOpTimeArray[locationDex] > 0 && (apportionmentStackFlowArray[locationDex] == null || apportionmentHclRateArray[locationDex] == null))
                                {
                                    stackMissingData = true;
                                }

                                if (apportionmentStackFlowArray[locationDex] != null)
                                {
                                    msStackFlowSum += apportionmentStackFlowArray[locationDex].Value;
                                    msStackEmissionRateFlow += apportionmentStackFlowArray[locationDex].Value * ECMPS.Definitions.Extensions.cExtensions.ScientificNotationtoDecimal(apportionmentHclRateArray[locationDex]);
                                }
                            }
                        }
                    }

                    /* Valid data was reported at each operating stack but unit data was not provided */
                    if (!modc38Used && stackOperated && EmParameters.MatsMs1HclUnadjustedHourlyValue == null)
                    {
                        category.CheckCatalogResult = "B";
                    }
                    /* Validate the data for the special condition of only 1 stack operating */
                    else if (numOperatingStacks == 1)
                    {
                        EmParameters.CalculatedFlowWeightedHcl = ECMPS.Definitions.Extensions.cExtensions.DecimaltoScientificNotation(singleStackHclRate);

                        //Convert the checking values to decimals from sci notation
                        decimal reportedValue = ECMPS.Definitions.Extensions.cExtensions.ScientificNotationtoDecimal(EmParameters.MatsMs1HclUnadjustedHourlyValue);

                        /* The rates must match when only 1 stack is operating */
                        if (singleStackHclRate != reportedValue)
                        {
                            category.CheckCatalogResult = "A";
                        }
                    }
                    /* Unit data was provided but valid data was not reported at each operating stack */
                    else if (stackMissingData && EmParameters.MatsMs1HclUnadjustedHourlyValue != null)
                    {
                        category.CheckCatalogResult = "C";
                    }
                    else if (msStackFlowSum > 0 && EmParameters.MatsMs1HclUnadjustedHourlyValue != null)
                    { //All good continue with check
                        EmParameters.CalculatedFlowWeightedHcl = ECMPS.Definitions.Extensions.cExtensions.DecimaltoScientificNotation(msStackEmissionRateFlow / msStackFlowSum);

                        //Convert the checking values to decimals from sci notation
                        decimal reportedValue = ECMPS.Definitions.Extensions.cExtensions.ScientificNotationtoDecimal(EmParameters.MatsMs1HclUnadjustedHourlyValue);
                        decimal calculatedValue = ECMPS.Definitions.Extensions.cExtensions.ScientificNotationtoDecimal(EmParameters.CalculatedFlowWeightedHcl);
                        if ((reportedValue + calculatedValue) != 0)
                        {
                            decimal percentDifference = Math.Round(100 * Math.Abs(reportedValue - calculatedValue) / ((reportedValue + calculatedValue) / 2), 1, MidpointRounding.AwayFromZero);

                            if (percentDifference > 5m)
                            {
                                category.CheckCatalogResult = "A";
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ReturnVal = category.CheckEngine.FormatError(ex, "HOURAPP11");
            }

            return ReturnVal;
        }

        /// <summary>
        /// Calculates and checks the MATS HF flow weighted average rate when the source is using the MS-1 formula.
        /// </summary>
        /// <param name="category"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public static string HOURAPP12(cCategory category, ref bool log)
        {
            string ReturnVal = "";

            try
            {
                EmParameters.CalculatedFlowWeightedHf = null;
                EmParameters.MatsReportedPluginHf = null;

                //MatsMs1HfDhvId is being used to verify that MS-1 calculation is needed
                if ((EmParameters.MpStackConfigForHourlyChecks == "MS") && (EmParameters.CurrentMonitorPlanLocationRecord.UnitId != null) && (EmParameters.MatsMs1HfDhvId != null))
                {
                    decimal[] apportionmentOpTimeArray = (decimal[])category.GetCheckParameter("Apportionment_Optime_Array").ParameterValue;
                    decimal?[] apportionmentStackFlowArray = (decimal?[])category.GetCheckParameter("Apportionment_Stack_Flow_Array").ParameterValue;
                    string[] apportionmentHfRateArray = (string[])category.GetCheckParameter("Apportionment_HF_Rate_Array").ParameterValue;
                    string[] matsMS1HfModcCodeArray = (string[])category.GetCheckParameter("MATS_MS1_HF_MODC_Code_Array").ParameterValue;
                    string[] locationNameArray = (string[])category.GetCheckParameter("Location_Name_Array").ParameterValue;
                    EmParameters.MatsReportedPluginHf = EmParameters.MatsMs1HfUnadjustedHourlyValue;

                    bool msStackFlowNoNulls = true;
                    bool modc38Used = false;
                    bool stackOperated = false;
                    bool stackMissingData = false;
                    decimal numOperatingStacks = 0;
                    decimal singleStackHfRate = 0;
                    decimal msStackFlowSum = 0;
                    decimal msStackEmissionRateFlow = 0;
                    {
                        for (int locationDex = 0; locationDex < apportionmentStackFlowArray.Length; locationDex++)
                        {
                            if (locationNameArray[locationDex].StartsWith("MS"))
                            {
                                //Set flag if any stack has an MODC of 38
                                if (matsMS1HfModcCodeArray[locationDex] == "38")
                                {
                                    modc38Used = true;
                                }

                                //Set flag if any stack operated
                                if (apportionmentOpTimeArray[locationDex] > 0)
                                {
                                    stackOperated = true;
                                    numOperatingStacks += 1;
                                    singleStackHfRate = ECMPS.Definitions.Extensions.cExtensions.ScientificNotationtoDecimal(apportionmentHfRateArray[locationDex]);
                                }

                                //Set flag for stack missing data
                                if (apportionmentOpTimeArray[locationDex] > 0 && (apportionmentStackFlowArray[locationDex] == null || apportionmentHfRateArray[locationDex] == null))
                                {
                                    stackMissingData = true;
                                }

                                if (apportionmentStackFlowArray[locationDex] != null)
                                {
                                    msStackFlowSum += apportionmentStackFlowArray[locationDex].Value;
                                    msStackEmissionRateFlow += apportionmentStackFlowArray[locationDex].Value * ECMPS.Definitions.Extensions.cExtensions.ScientificNotationtoDecimal(apportionmentHfRateArray[locationDex]);
                                }
                            }
                        }
                    }

                    /* Valid data was reported at each operating stack but unit data was not provided */
                    if (!modc38Used && stackOperated && EmParameters.MatsMs1HfUnadjustedHourlyValue == null)
                    {
                        category.CheckCatalogResult = "B";
                    }
                    /* Validate the data for the special condition of only 1 stack operating */
                    else if (numOperatingStacks == 1)
                    {
                        EmParameters.CalculatedFlowWeightedHf = ECMPS.Definitions.Extensions.cExtensions.DecimaltoScientificNotation(singleStackHfRate);

                        //Convert the checking values to decimals from sci notation
                        decimal reportedValue = ECMPS.Definitions.Extensions.cExtensions.ScientificNotationtoDecimal(EmParameters.MatsMs1HfUnadjustedHourlyValue);

                        /* The rates must match when only 1 stack is operating */
                        if (singleStackHfRate != reportedValue)
                        {
                            category.CheckCatalogResult = "A";
                        }
                    }
                    /* Unit data was provided but valid data was not reported at each operating stack */
                    else if (stackMissingData && EmParameters.MatsMs1HfUnadjustedHourlyValue != null)
                    {
                        category.CheckCatalogResult = "C";
                    }
                    else if (msStackFlowSum > 0 && EmParameters.MatsMs1HfUnadjustedHourlyValue != null)
                    { //All good continue with check
                        EmParameters.CalculatedFlowWeightedHf = ECMPS.Definitions.Extensions.cExtensions.DecimaltoScientificNotation(msStackEmissionRateFlow / msStackFlowSum);

                        //Convert the checking values to decimals from sci notation
                        decimal reportedValue = ECMPS.Definitions.Extensions.cExtensions.ScientificNotationtoDecimal(EmParameters.MatsMs1HfUnadjustedHourlyValue);
                        decimal calculatedValue = ECMPS.Definitions.Extensions.cExtensions.ScientificNotationtoDecimal(EmParameters.CalculatedFlowWeightedHf);
                        if ((reportedValue + calculatedValue) != 0)
                        {
                            decimal percentDifference = Math.Round(100 * Math.Abs(reportedValue - calculatedValue) / ((reportedValue + calculatedValue) / 2), 1, MidpointRounding.AwayFromZero);

                            if (percentDifference > 5m)
                            {
                                category.CheckCatalogResult = "A";
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ReturnVal = category.CheckEngine.FormatError(ex, "HOURAPP12");
            }

            return ReturnVal;
        }

        /// <summary>
        /// Calculates and checks the MATS SO2 flow weighted average rate when the source is using the MS-1 formula.
        /// </summary>
        /// <param name="category"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public static string HOURAPP13(cCategory category, ref bool log)
        {
            string ReturnVal = "";

            try
            {
                EmParameters.CalculatedFlowWeightedSo2 = null;
                EmParameters.MatsReportedPluginSo2 = null;

                //MatsMs1So2DhvId is being used to verify that MS-1 calculation is needed
                if ((EmParameters.MpStackConfigForHourlyChecks == "MS") && (EmParameters.CurrentMonitorPlanLocationRecord.UnitId != null) && (EmParameters.MatsMs1So2DhvId != null))
                {
                    decimal[] apportionmentOpTimeArray = (decimal[])category.GetCheckParameter("Apportionment_Optime_Array").ParameterValue;
                    decimal?[] apportionmentStackFlowArray = (decimal?[])category.GetCheckParameter("Apportionment_Stack_Flow_Array").ParameterValue;
                    string[] apportionmentSo2RateArray = (string[])category.GetCheckParameter("Apportionment_SO2_Rate_Array").ParameterValue;
                    string[] matsMS1So2ModcCodeArray = (string[])category.GetCheckParameter("MATS_MS1_SO2_MODC_Code_Array").ParameterValue;
                    string[] locationNameArray = (string[])category.GetCheckParameter("Location_Name_Array").ParameterValue;
                    EmParameters.MatsReportedPluginSo2 = EmParameters.MatsMs1So2UnadjustedHourlyValue;

                    bool msStackFlowNoNulls = true;
                    bool modc38Used = false;
                    bool stackOperated = false;
                    bool stackMissingData = false;
                    decimal numOperatingStacks = 0;
                    decimal singleStackSo2Rate = 0;
                    decimal msStackFlowSum = 0;
                    decimal msStackEmissionRateFlow = 0;
                    {
                        for (int locationDex = 0; locationDex < apportionmentStackFlowArray.Length; locationDex++)
                        {
                            if (locationNameArray[locationDex].StartsWith("MS"))
                            {
                                //Set flag if any stack has an MODC of 38
                                if (matsMS1So2ModcCodeArray[locationDex] == "38")
                                {
                                    modc38Used = true;
                                }

                                //Set flag if any stack operated
                                if (apportionmentOpTimeArray[locationDex] > 0)
                                {
                                    stackOperated = true;
                                    numOperatingStacks += 1;
                                    singleStackSo2Rate = ECMPS.Definitions.Extensions.cExtensions.ScientificNotationtoDecimal(apportionmentSo2RateArray[locationDex]);
                                }

                                //Set flag for stack missing data
                                if (apportionmentOpTimeArray[locationDex] > 0 && (apportionmentStackFlowArray[locationDex] == null || apportionmentSo2RateArray[locationDex] == null))
                                {
                                    stackMissingData = true;
                                }

                                if (apportionmentStackFlowArray[locationDex] != null)
                                {
                                    msStackFlowSum += apportionmentStackFlowArray[locationDex].Value;
                                    msStackEmissionRateFlow += apportionmentStackFlowArray[locationDex].Value * ECMPS.Definitions.Extensions.cExtensions.ScientificNotationtoDecimal(apportionmentSo2RateArray[locationDex]);
                                }
                            }
                        }
                    }

                    /* Valid data was reported at each operating stack but unit data was not provided */
                    if (!modc38Used && stackOperated && EmParameters.MatsMs1So2UnadjustedHourlyValue == null)
                    {
                        category.CheckCatalogResult = "B";
                    }
                    /* Validate the data for the special condition of only 1 stack operating */
                    else if (numOperatingStacks == 1)
                    {
                        EmParameters.CalculatedFlowWeightedSo2 = ECMPS.Definitions.Extensions.cExtensions.DecimaltoScientificNotation(singleStackSo2Rate);

                        //Convert the checking values to decimals from sci notation
                        decimal reportedValue = ECMPS.Definitions.Extensions.cExtensions.ScientificNotationtoDecimal(EmParameters.MatsMs1So2UnadjustedHourlyValue);

                        /* The rates must match when only 1 stack is operating */
                        if (singleStackSo2Rate != reportedValue)
                        {
                            category.CheckCatalogResult = "A";
                        }
                    }
                    /* Unit data was provided but valid data was not reported at each operating stack */
                    else if (stackMissingData && EmParameters.MatsMs1So2UnadjustedHourlyValue != null)
                    {
                        category.CheckCatalogResult = "C";
                    }
                    else if (msStackFlowSum > 0 && EmParameters.MatsMs1So2UnadjustedHourlyValue != null)
                    { //All good continue with check
                        EmParameters.CalculatedFlowWeightedSo2 = ECMPS.Definitions.Extensions.cExtensions.DecimaltoScientificNotation(msStackEmissionRateFlow / msStackFlowSum);

                        //Convert the checking values to decimals from sci notation
                        decimal reportedValue = ECMPS.Definitions.Extensions.cExtensions.ScientificNotationtoDecimal(EmParameters.MatsMs1So2UnadjustedHourlyValue);
                        decimal calculatedValue = ECMPS.Definitions.Extensions.cExtensions.ScientificNotationtoDecimal(EmParameters.CalculatedFlowWeightedSo2);
                        if ((reportedValue + calculatedValue) != 0)
                        {
                            decimal percentDifference = Math.Round(100 * Math.Abs(reportedValue - calculatedValue) / ((reportedValue + calculatedValue) / 2), 1, MidpointRounding.AwayFromZero);

                            if (percentDifference > 5m)
                            {
                                category.CheckCatalogResult = "A";
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ReturnVal = category.CheckEngine.FormatError(ex, "HOURAPP13");
            }

            return ReturnVal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="category"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public static string HOURAPP14(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                if ((EmParameters.MpStackConfigForHourlyChecks == "COMPLEX") || (EmParameters.MpStackConfigForHourlyChecks == "CSMS"))
                {
                    if (EmParameters.StackHeatinputtimesoptimeAccumulator.HasValue && (EmParameters.StackHeatinputtimesoptimeAccumulator.Value >= 0) &&
                        EmParameters.UnitHeatinputtimesoptimeAccumulator.HasValue && (EmParameters.UnitHeatinputtimesoptimeAccumulator.Value >= 0))
                    {
                        decimal heatInputTolerance = GetTolerance("HI", "MMBTUHR", category);

                        // Return result if total stack and unit heat input are not within tolerance (HI Tolerance * number of locations).
                        if ((heatInputTolerance == Decimal.MinValue) || 
                            (Math.Abs(EmParameters.StackHeatinputtimesoptimeAccumulator.Value - EmParameters.UnitHeatinputtimesoptimeAccumulator.Value) 
                             > (heatInputTolerance * EmParameters.CurrentLocationCount.Value)))
                        {
                            category.CheckCatalogResult = "A";
                        }
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
