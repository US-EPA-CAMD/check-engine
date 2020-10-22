using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

namespace ECMPS.Checks.FuelChecks
{
    public class cFuelChecks : cChecks
    {
        public cFuelChecks()
        {
            CheckProcedures = new dCheckProcedure[53];

            CheckProcedures[39] = new dCheckProcedure(FUEL39);
            CheckProcedures[40] = new dCheckProcedure(FUEL40);
            CheckProcedures[41] = new dCheckProcedure(FUEL41);
            CheckProcedures[42] = new dCheckProcedure(FUEL42);
            CheckProcedures[43] = new dCheckProcedure(FUEL43);
            CheckProcedures[44] = new dCheckProcedure(FUEL44);
            CheckProcedures[45] = new dCheckProcedure(FUEL45);
            CheckProcedures[46] = new dCheckProcedure(FUEL46);
            CheckProcedures[48] = new dCheckProcedure(FUEL48);
            CheckProcedures[49] = new dCheckProcedure(FUEL49);
            CheckProcedures[51] = new dCheckProcedure(FUEL51);
            CheckProcedures[52] = new dCheckProcedure(FUEL52);

        }

        public static string FUEL39(cCategory Category, ref bool Log) //Fuel Active Status
        {
            string ReturnVal = "";

            Category.SetCheckParameter("Fuel_Active_Status", null, eParameterDataType.Boolean);

            try
            {
              bool FuelDatesConsistent = (bool)Category.GetCheckParameter("Fuel_Dates_Consistent").ParameterValue;

              if (FuelDatesConsistent)
                  ReturnVal = Check_ActiveDateRange(Category, "Fuel_Active_Status", "Current_Fuel", "Fuel_Evaluation_Begin_Date", "Fuel_Evaluation_End_Date");
                     
            }
            catch (Exception ex)
            { ReturnVal = Category.CheckEngine.FormatError(ex, "FUEL39"); }

            return ReturnVal;
        }

        public static string FUEL40(cCategory Category, ref bool Log) //Unit Fuel Demonstration GCV Valid
        {
            string ReturnVal = "";

            try
            {
               DataRowView CurrentFuel = (DataRowView)Category.GetCheckParameter("Current_Fuel").ParameterValue;
               bool GCVValid = true;

               DataView FuelDemMethodRecords = (DataView)Category.GetCheckParameter("Fuel_Demonstration_Method_Lookup_Table").ParameterValue;
               FuelDemMethodRecords.RowFilter = "dem_parameter='GCV' and DEM_METHOD_CD='" + cDBConvert.ToString(CurrentFuel["dem_gcv"]) + "'";

               if (CurrentFuel["dem_gcv"] == DBNull.Value)
               {
                 Category.SetCheckParameter("Unit_Fuel_Demonstration_GCV_Valid", true, eParameterDataType.Boolean);
                 return ReturnVal;
               }

               if (FuelDemMethodRecords.Count == 0)
               {
                   GCVValid = false;
                   Category.CheckCatalogResult = "A";
               }
               else
               {
                   string FuelGroup = (string)Category.GetCheckParameter("Fuel_Group").ParameterValue;

                   if (FuelGroup !="" && FuelGroup != "GAS" && FuelGroup != "OIL")
                   {
                       GCVValid = false;
                       Category.CheckCatalogResult = "B";
                   }
               }

               Category.SetCheckParameter("Unit_Fuel_Demonstration_GCV_Valid", GCVValid, eParameterDataType.Boolean);

           }
            catch (Exception ex)
            { ReturnVal = Category.CheckEngine.FormatError(ex, "FUEL40"); }

            return ReturnVal;
        }

        public static string FUEL41(cCategory Category, ref bool Log) //Unit Fuel Demonstration SO2 Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentFuel = (DataRowView)Category.GetCheckParameter("Current_Fuel").ParameterValue;
                bool SO2Valid = true;

                DataView FuelDemMethodRecords = (DataView)Category.GetCheckParameter("Fuel_Demonstration_Method_Lookup_Table").ParameterValue;
                FuelDemMethodRecords.RowFilter ="dem_parameter = 'SULFUR' and DEM_METHOD_CD='" + cDBConvert.ToString(CurrentFuel["dem_so2"]) + "'";

                if (CurrentFuel["dem_so2"] == DBNull.Value)
                {
                  Category.SetCheckParameter("Unit_Fuel_Demonstration_SO2_Valid", true, eParameterDataType.Boolean);
                  return ReturnVal;
                }

                if (FuelDemMethodRecords.Count == 0)
                {
                    SO2Valid = false;
                    Category.CheckCatalogResult = "A";
                }
                else
                {
                    string FuelGroup = (string)Category.GetCheckParameter("Fuel_Group").ParameterValue;

                    if (FuelGroup != "" && FuelGroup != "GAS" && FuelGroup != "OIL")
                    {
                        SO2Valid = false;
                        Category.CheckCatalogResult = "B";
                    }
                }

                Category.SetCheckParameter("Unit_Fuel_Demonstration_SO2_Valid", SO2Valid, eParameterDataType.Boolean);

            }
            catch (Exception ex)
            { ReturnVal = Category.CheckEngine.FormatError(ex, "FUEL41"); }

            return ReturnVal;
        }

        public static string FUEL42(cCategory Category, ref bool Log) //Unit Fuel Begin Date Valid
        {
            string ReturnVal = "";

            try
            {
                ReturnVal = Check_ValidStartDate(Category, "Fuel_Begin_Date_Valid", "Current_Fuel", "BEGIN_DATE", new DateTime(1930,1,1), "A", "B", "FUEL42");
                if (string.IsNullOrEmpty(Category.CheckCatalogResult))
                {
                    DataRowView CurrentFuel = (DataRowView)Category.GetCheckParameter("Current_Fuel").ParameterValue;
                    DataRowView CurrentUnit = (DataRowView)Category.GetCheckParameter("Current_Unit").ParameterValue;
                    DateTime CommOpDt = cDBConvert.ToDate(CurrentUnit["COMM_OP_DATE"], DateTypes.END);
                    DateTime ComrOpDt = cDBConvert.ToDate(CurrentUnit["COMR_OP_DATE"], DateTypes.END);
                    DateTime BeginDate = cDBConvert.ToDate(CurrentFuel["BEGIN_DATE"], DateTypes.START);
                    if (CommOpDt != DateTime.MaxValue || ComrOpDt != DateTime.MaxValue)
                    {
                        if (BeginDate < CommOpDt && BeginDate < ComrOpDt)
                            Category.CheckCatalogResult = "C";
                    }
                }
            }
            catch (Exception ex)
            { ReturnVal = Category.CheckEngine.FormatError(ex, "FUEL42"); }

            return ReturnVal;
        }

        public static string FUEL43(cCategory Category, ref bool Log) //Unit Fuel End Date Valid
        {
            string ReturnVal = "";

            try
            {
                ReturnVal = Check_ValidEndDate(Category, "Fuel_End_Date_Valid",
                                                               "Current_Fuel",
                                                               "End_Date");
            }
            catch (Exception ex)
            { ReturnVal = Category.CheckEngine.FormatError(ex, "FUEL43"); }

            return ReturnVal;
        }

        public static string FUEL44(cCategory Category, ref bool Log) //Fuel Dates Consistent
        {
            string ReturnVal = "";

            try
            {
                ReturnVal = Check_ConsistentDateRange( Category, "Fuel_Dates_Consistent", "Current_Fuel", "fuel_begin_date_valid",
                    "fuel_end_date_valid");
            }
            catch (Exception ex)
            { ReturnVal = Category.CheckEngine.FormatError(ex, "FUEL44"); }

            return ReturnVal;
        }

        public static string FUEL45(cCategory Category, ref bool Log) //Unit Fuel Code Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentFuel = (DataRowView)Category.GetCheckParameter("Current_Fuel").ParameterValue;
                string FuelCd = cDBConvert.ToString(CurrentFuel["Fuel_Cd"]);
                string FuelGrp = "";

                bool FuelCodeValid = true;

                if (FuelCd == "")
                {
                    FuelCodeValid = false;
                    Category.CheckCatalogResult = "A";
                }
                else
                {
                    DataView FuelCodeView = (DataView)Category.GetCheckParameter("Fuel_Code_Lookup_Table").ParameterValue;
                    FuelCodeView.RowFilter = "Fuel_Cd='" + FuelCd + "'";

                    if (FuelCodeView.Count == 0)
                    {
                        FuelCodeValid = false;
                        Category.CheckCatalogResult = "B";
                    }
                    else
                        FuelGrp = cDBConvert.ToString(FuelCodeView[0]["Fuel_Group_Cd"]);
                }
                Category.SetCheckParameter("Fuel_Code_Valid", FuelCodeValid, eParameterDataType.Boolean);
                Category.SetCheckParameter("Fuel_Group", FuelGrp, eParameterDataType.String);

            }
            catch (Exception ex)
            { ReturnVal = Category.CheckEngine.FormatError(ex, "FUEL45"); }

            return ReturnVal;
        }

        public static string FUEL46(cCategory Category, ref bool Log) //Fuel Consistent With Unit Type
        {

            string ReturnVal = "";

            try
            {
                DataRowView CurrentFuel = (DataRowView)Category.GetCheckParameter("Current_Fuel").ParameterValue;
                string FuelCd = cDBConvert.ToString(CurrentFuel["Fuel_Cd"]);

                if (FuelCd == "C")
                {
                    DateTime FuelEvalBeginDate = DateTime.MinValue;
                    DateTime FuelEvalEndDate = DateTime.MaxValue;

                    FuelEvalBeginDate = (DateTime)Category.GetCheckParameter("Fuel_Evaluation_Begin_Date").ParameterValue;
                    FuelEvalEndDate = (DateTime)Category.GetCheckParameter("Fuel_Evaluation_End_Date").ParameterValue;

                    DataView UnitTypeRecords = (DataView)Category.GetCheckParameter("Location_Unit_Type_Records").ParameterValue;
                    UnitTypeRecords.RowFilter = "unit_type_cd in('CT','CC','ICE','OT')";

                    UnitTypeRecords.RowFilter = AddEvaluationDateRangeToDataViewFilter(UnitTypeRecords.RowFilter,
                      FuelEvalBeginDate, FuelEvalEndDate, false, true, false);

                    if (UnitTypeRecords.Count > 0)
                        Category.CheckCatalogResult = "A";
                }

            }
            catch (Exception ex)
            { ReturnVal = Category.CheckEngine.FormatError(ex, "FUEL46"); }

            return ReturnVal;
        }

        public static string FUEL48(cCategory Category, ref bool Log) //Fuel Demonstration Methods Consistent with Method
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentFuel = (DataRowView)Category.GetCheckParameter("Current_Fuel").ParameterValue;
                bool GCVValid = Convert.ToBoolean(Category.GetCheckParameter("Unit_Fuel_Demonstration_GCV_Valid").ParameterValue);
                bool SO2Valid = Convert.ToBoolean(Category.GetCheckParameter("Unit_Fuel_Demonstration_SO2_Valid").ParameterValue);

                if ((GCVValid && CurrentFuel["dem_gcv"] != DBNull.Value) || (SO2Valid && CurrentFuel["dem_so2"] != DBNull.Value))
                {
                    DateTime FuelEvalBeginDate = DateTime.MinValue;
                    DateTime FuelEvalEndDate = DateTime.MaxValue;

                    DataView MethodRecords = (DataView)Category.GetCheckParameter("Facility_Method_Records").ParameterValue;

                    FuelEvalBeginDate = (DateTime)Category.GetCheckParameter("Fuel_Evaluation_Begin_Date").ParameterValue;
                    FuelEvalEndDate = (DateTime)Category.GetCheckParameter("Fuel_Evaluation_End_Date").ParameterValue;

                    string MethodFilter = MethodRecords.RowFilter;
                    MethodRecords.RowFilter = AddToDataViewFilter(MethodFilter, "method_cd like 'AD%' and unit_id = " + cDBConvert.ToInteger(CurrentFuel["unit_id"]));

                    MethodRecords.RowFilter = AddEvaluationDateRangeToDataViewFilter(MethodRecords.RowFilter,
                      FuelEvalBeginDate, FuelEvalEndDate, false, true, false);

                    if (MethodRecords.Count == 0)
                    {
                        DataView UnitStackConfigRecords = (DataView)Category.GetCheckParameter("Unit_Stack_Configuration_Records").ParameterValue;
                        string USFilter = UnitStackConfigRecords.RowFilter;
                        UnitStackConfigRecords.RowFilter = AddToDataViewFilter(USFilter, "(stack_name like 'CP%' or stack_name like 'MP%')");

                        UnitStackConfigRecords.RowFilter = AddEvaluationDateRangeToDataViewFilter(UnitStackConfigRecords.RowFilter,
                          FuelEvalBeginDate, FuelEvalEndDate, false, true, false);

                        if (UnitStackConfigRecords.Count == 0)
                            Category.CheckCatalogResult = "A";
                        else
                        {
                            bool recFound = false;

                            foreach (DataRowView rec in UnitStackConfigRecords)
                            {
                                MethodRecords.RowFilter = AddToDataViewFilter(MethodFilter, "method_cd like 'AD%' and mon_loc_id = '" + rec["stack_pipe_mon_loc_id"] + "'");

                                MethodRecords.RowFilter = AddEvaluationDateRangeToDataViewFilter(MethodRecords.RowFilter,
                                  FuelEvalBeginDate, FuelEvalEndDate, false, true, false);
                                if (MethodRecords.Count > 0)
                                {
                                    recFound = true;
                                    break;
                                }
                                MethodRecords.RowFilter = MethodFilter;
                            }

                            if (!recFound)
                                Category.CheckCatalogResult = "A";
                        }
                        UnitStackConfigRecords.RowFilter = USFilter;
                    }
                    MethodRecords.RowFilter = MethodFilter;
                }
                else
                    Log = false;
            }
            catch (Exception ex)
            { ReturnVal = Category.CheckEngine.FormatError(ex, "FUEL48"); }

            return ReturnVal;
        }

        public static string FUEL49(cCategory Category, ref bool Log) //Ozone Season Indicator Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentFuel = (DataRowView)Category.GetCheckParameter("Current_Fuel").ParameterValue;
            
                int OzoneIndicator = cDBConvert.ToInteger(CurrentFuel["ozone_seas_ind"]);
                string PSIndicator = cDBConvert.ToString(CurrentFuel["indicator_cd"]);

                bool IndicatorValid = true;
                if( OzoneIndicator == 1 && PSIndicator != "S" )
                {
                    IndicatorValid = false;
                    Category.CheckCatalogResult = "A";
                }

                Category.SetCheckParameter("Unit_Fuel_Ozone_Season_Indicator_Valid", IndicatorValid, eParameterDataType.Boolean);
            }
            catch (Exception ex)
            { ReturnVal = Category.CheckEngine.FormatError(ex, "FUEL49"); }

            return ReturnVal;
        }

        public static string FUEL51(cCategory Category, ref bool Log) // Primary/Secondary Code Valid
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentFuel = (DataRowView)Category.GetCheckParameter("Current_Fuel").ParameterValue;
                string PSIndicator = cDBConvert.ToString(CurrentFuel["indicator_cd"]);
                bool IndicatorValid = false;

                if (PSIndicator == "")
                    Category.CheckCatalogResult = "A";
                else
                {
                    DataView IndicatorCodeLookup = (DataView)Category.GetCheckParameter("Fuel_Indicator_Code_Lookup_Table").ParameterValue;
                    IndicatorCodeLookup.RowFilter = "indicator_cd = '" + PSIndicator + "'";

                    if (IndicatorCodeLookup.Count == 0)
                        Category.CheckCatalogResult = "B";
                    else
                        IndicatorValid = true;
                }

                Category.SetCheckParameter("Unit_Fuel_Primary_Secondary_Indicator_Code_Valid",IndicatorValid, eParameterDataType.Boolean);
            }
            catch (Exception ex)
            { ReturnVal = Category.CheckEngine.FormatError(ex, "FUEL51"); }

            return ReturnVal;
        }

        public static string FUEL52(cCategory Category, ref bool Log) // Duplicate UnitFuel Records
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentFuel = (DataRowView)Category.GetCheckParameter("Current_Fuel").ParameterValue;
                DataView FuelRecords = (DataView)Category.GetCheckParameter("Location_Fuel_Records").ParameterValue;
                
                string FuelCode = cDBConvert.ToString(CurrentFuel["fuel_cd"]);
                DateTime BeginDate = cDBConvert.ToDate(CurrentFuel["begin_date"], DateTypes.START);
                string UFid = cDBConvert.ToString(CurrentFuel["uf_id"]);
                string OldFilter = FuelRecords.RowFilter;
                FuelRecords.RowFilter = AddToDataViewFilter(OldFilter, "begin_date = '" + BeginDate.ToShortDateString() + "' and fuel_cd = '" + FuelCode + "'" + " and uf_id <> '" + UFid + "'");

                if (FuelRecords.Count > 0)
                    Category.CheckCatalogResult = "A";
                else
                {
                    DateTime EndDate = cDBConvert.ToDate(CurrentFuel["end_date"], DateTypes.END);
                    if (EndDate != DateTime.MaxValue)
                    {
                        FuelRecords.RowFilter = AddToDataViewFilter(OldFilter, "end_date = '" + EndDate.ToShortDateString() + "' and fuel_cd = '" + FuelCode + "'" + " and uf_id <> '" + UFid + "'");
                        if (FuelRecords.Count > 0)
                            Category.CheckCatalogResult = "A";
                    }
                }
                FuelRecords.RowFilter = OldFilter;
            }
            catch (Exception ex)
            { ReturnVal = Category.CheckEngine.FormatError(ex, "FUEL52"); }

            return ReturnVal;
        }

    }
}
