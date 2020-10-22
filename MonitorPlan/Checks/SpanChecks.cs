using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Mp.Parameters;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;
using ECMPS.Definitions.Extensions;


namespace ECMPS.Checks.SpanChecks
{
  public class cSpanChecks : cChecks
  {

    #region Constructors

    public cSpanChecks()
    {
      CheckProcedures = new dCheckProcedure[62];

      CheckProcedures[1] = new dCheckProcedure(SPAN1);
      CheckProcedures[2] = new dCheckProcedure(SPAN2);
      CheckProcedures[3] = new dCheckProcedure(SPAN3);
      CheckProcedures[4] = new dCheckProcedure(SPAN4);
      CheckProcedures[6] = new dCheckProcedure(SPAN6);
      CheckProcedures[7] = new dCheckProcedure(SPAN7);
      CheckProcedures[8] = new dCheckProcedure(SPAN8);
      CheckProcedures[9] = new dCheckProcedure(SPAN9);
      CheckProcedures[10] = new dCheckProcedure(SPAN10);

      CheckProcedures[11] = new dCheckProcedure(SPAN11);
      CheckProcedures[12] = new dCheckProcedure(SPAN12);
      CheckProcedures[13] = new dCheckProcedure(SPAN13);
      CheckProcedures[16] = new dCheckProcedure(SPAN16);
      CheckProcedures[17] = new dCheckProcedure(SPAN17);
      CheckProcedures[18] = new dCheckProcedure(SPAN18);
      CheckProcedures[20] = new dCheckProcedure(SPAN20);

      CheckProcedures[21] = new dCheckProcedure(SPAN21);

      CheckProcedures[36] = new dCheckProcedure(SPAN36);
      CheckProcedures[37] = new dCheckProcedure(SPAN37);
      CheckProcedures[47] = new dCheckProcedure(SPAN47);
      CheckProcedures[48] = new dCheckProcedure(SPAN48);
      CheckProcedures[50] = new dCheckProcedure(SPAN50);

      CheckProcedures[52] = new dCheckProcedure(SPAN52);
      CheckProcedures[53] = new dCheckProcedure(SPAN53);
      CheckProcedures[54] = new dCheckProcedure(SPAN54);
      CheckProcedures[55] = new dCheckProcedure(SPAN55);
      CheckProcedures[56] = new dCheckProcedure(SPAN56);
      CheckProcedures[57] = new dCheckProcedure(SPAN57);
      CheckProcedures[58] = new dCheckProcedure(SPAN58);
      CheckProcedures[59] = new dCheckProcedure(SPAN59);
      CheckProcedures[60] = new dCheckProcedure(SPAN60);
      CheckProcedures[61] = new dCheckProcedure(SPAN61);
    }

    #endregion

    #region Public Static Methods: Checks

    public static string SPAN1(cCategory Category, ref bool Log)
    // Span MPC Value Valid 
    {
      string ReturnVal = "";

      try
      {
        if (cDBConvert.ToBoolean(Category.GetCheckParameter("Span_Component_Type_Code_Valid").ParameterValue))
        {
          DataRowView CurrentSpan = (DataRowView)Category.GetCheckParameter("Current_Span").ParameterValue;
          string ComponentTypeCd = cDBConvert.ToString(CurrentSpan["Component_Type_Cd"]);
          string SpanScaleCd = cDBConvert.ToString(CurrentSpan["Span_Scale_Cd"]);

          if ((CurrentSpan["Mpc_Value"] == DBNull.Value) &&
              ( !ComponentTypeCd.InList( "FLOW,O2" ) && ( SpanScaleCd == "H" ) ) )
          {
            Category.SetCheckParameter("Span_MPC_Value_Valid", false, eParameterDataType.Boolean);
            Category.CheckCatalogResult = "A";
          }
          else if ((CurrentSpan["Mpc_Value"] != DBNull.Value) &&
                   ( ComponentTypeCd.InList( "FLOW,O2" ) || ( SpanScaleCd == "L" ) ) )
          {
            Category.SetCheckParameter("Span_MPC_Value_Valid", false, eParameterDataType.Boolean);
            Category.CheckCatalogResult = "B";
          }
          else
          {
            decimal MpcValue = cDBConvert.ToDecimal(CurrentSpan["Mpc_Value"]);

            if ((MpcValue <= 0) && (MpcValue != decimal.MinValue))
            {
              Category.SetCheckParameter("Span_MPC_Value_Valid", false, eParameterDataType.Boolean);
              Category.CheckCatalogResult = "C";
            }
            else
            {
              string SpanMethodCd = cDBConvert.ToString(CurrentSpan["Span_Method_Cd"]);

              if ((ComponentTypeCd == "NOX") && (SpanMethodCd == "TB"))
              {
                DateTime EvalBeganDate = cDBConvert.ToDate(Category.GetCheckParameter("Span_Evaluation_Begin_Date").ParameterValue, DateTypes.START);
                DateTime EvalEndedDate = cDBConvert.ToDate(Category.GetCheckParameter("Span_Evaluation_End_Date").ParameterValue, DateTypes.END);

                DataView LocationFuelRecords = (DataView)Category.GetCheckParameter("Location_Fuel_Records").ParameterValue;
                DataView LocationFuelView = FindActiveRows(LocationFuelRecords, EvalBeganDate, EvalEndedDate);

                bool CoalCode = false; bool NaturalGasCode = false; bool OtherCode = false;
                bool GasGroup = false; bool OilGroup = false; bool OtherGroup = false;

                foreach (DataRowView LocationFuelRow in LocationFuelView)
                {
                  string FuelCd = cDBConvert.ToString(LocationFuelRow["Fuel_Cd"]);
                  string FuelGroupCd = cDBConvert.ToString(LocationFuelRow["Fuel_Group_Cd"]);

                  if (FuelCd == "C")
                    CoalCode = true;
                  else if ((FuelCd == "NNG") || (FuelCd == "PNG"))
                    NaturalGasCode = true;
                  else
                    OtherCode = true;

                  if (FuelGroupCd == "GAS")
                    GasGroup = true;
                  else if (FuelGroupCd == "OIL")
                    OilGroup = true;
                  else
                    OtherGroup = true;
                }

                bool NaturalGasLocation = false;
                string LocationFuelCategory = "";

                if (CoalCode)
                  LocationFuelCategory = "COAL";
                else if (NaturalGasCode && !CoalCode && !OtherCode)
                {
                  LocationFuelCategory = "GAS";
                  NaturalGasLocation = true;
                }
                else if (GasGroup && !OilGroup && !OtherGroup)
                  LocationFuelCategory = "GAS";
                else if (OilGroup && !GasGroup && !OtherGroup)
                  LocationFuelCategory = "OIL";
                else if (OilGroup && GasGroup && !OtherGroup)
                  LocationFuelCategory = "OIL/GAS";

                DataView MpcFuelUnitTypeCross = (DataView)Category.GetCheckParameter("NOX_MPC_to_Fuel_Category_and_Unit_Type").ParameterValue;
                sFilterPair[] MpcFuelUnitTypeFilter = new sFilterPair[3];

                MpcFuelUnitTypeFilter[0].Set("NoxMpc", MpcValue, eFilterDataType.Decimal);
                MpcFuelUnitTypeFilter[1].Set("FuelCategory", LocationFuelCategory);
                MpcFuelUnitTypeFilter[2].Set("UnitTypeCode", "");

                DataView MpcFuelUnitTypeView = FindRows(MpcFuelUnitTypeCross, MpcFuelUnitTypeFilter);

                if (MpcFuelUnitTypeView.Count == 0)
                {
                  DataView UnitTypeRecords = (DataView)Category.GetCheckParameter("Location_Unit_Type_Records").ParameterValue;
                  DataView UnitTypeView = FindActiveRows(UnitTypeRecords, EvalBeganDate, EvalEndedDate);
                  string UnitTypeList = ColumnToDatalist(UnitTypeView, "Unit_Type_Cd");

                  string FuelCategoryList = ""; string FuelCategoryDelim = "";

                  if (LocationFuelCategory != "")
                  {
                    FuelCategoryList += FuelCategoryDelim + LocationFuelCategory;
                    FuelCategoryDelim = ",";
                  }

                  if (NaturalGasLocation)
                  {
                    FuelCategoryList += FuelCategoryDelim + "NG";
                    FuelCategoryDelim = ",";
                  }

                  MpcFuelUnitTypeFilter[0].Set("NoxMpc", MpcValue, eFilterDataType.Decimal);
                  MpcFuelUnitTypeFilter[1].Set("FuelCategory", FuelCategoryList, eFilterPairStringCompare.InList);
                  MpcFuelUnitTypeFilter[2].Set("UnitTypeCode", UnitTypeList, eFilterPairStringCompare.InList);

                  MpcFuelUnitTypeView = FindRows(MpcFuelUnitTypeCross, MpcFuelUnitTypeFilter);

                  if (MpcFuelUnitTypeView.Count == 0)
                  {
                    MpcFuelUnitTypeFilter[0].Set("NoxMpc", MpcValue, eFilterDataType.Decimal);
                    MpcFuelUnitTypeFilter[1].Set("FuelCategory", "");
                    MpcFuelUnitTypeFilter[2].Set("UnitTypeCode", UnitTypeList, eFilterPairStringCompare.InList);

                    MpcFuelUnitTypeView = FindRows(MpcFuelUnitTypeCross, MpcFuelUnitTypeFilter);

                    if (MpcFuelUnitTypeView.Count == 0)
                    {
                      DateTime EndDate = cDBConvert.ToDate(CurrentSpan["End_Date"], DateTypes.END);

                      if ((MpcValue != 50) ||
                          ((EndDate == DateTime.MaxValue) || (EndDate > new DateTime(2003, 3, 31))) ||
                          !"CT".InList( UnitTypeList ) )
                        Category.CheckCatalogResult = "D";
                    }
                  }
                }
              }
              else if ((ComponentTypeCd == "HG") && (SpanMethodCd == "TB"))
              {
                  if ((MpcValue != decimal.MinValue) && (MpcValue != 10) && (MpcValue != 16))
                      Category.CheckCatalogResult = "E";
              }

              else if ((ComponentTypeCd == "CO2") && (SpanMethodCd == "TB"))
              {
                DateTime evalBeganDate = Category.GetCheckParameter("Span_Evaluation_Begin_Date").AsDateTime(DateTime.MinValue);
                DateTime evalEndedDate = Category.GetCheckParameter("Span_Evaluation_End_Date").AsDateTime(DateTime.MaxValue);
                DataView unitTypeRecords = Category.GetCheckParameter("Location_Unit_Type_Records").AsDataView();
                DataView unitTypeView = FindActiveRows(unitTypeRecords, evalBeganDate, evalEndedDate);

                bool hasKlnOrPrh = false;
                bool allInTurbineAndCombustionList = true;
                bool noneInTurbineAndCombustionList = true;

                foreach (DataRowView unitTypeRow in unitTypeView)
                {
                  string unitType = unitTypeRow["UNIT_TYPE_CD"].AsString();

                  if (unitType.InList("KLN,PRH"))
                    hasKlnOrPrh = true;

                  if (unitType.InList("CC,CT,ICE,OT,IGC"))
                    noneInTurbineAndCombustionList = false;
                  else
                    allInTurbineAndCombustionList = false;
                }

                if (!hasKlnOrPrh)
                {

                  if (MpcValue != 6 && MpcValue != 14)
                  {
                    Category.CheckCatalogResult = "F";
                  }
                  else
                  {
                    if (allInTurbineAndCombustionList && (MpcValue != 6))
                      Category.CheckCatalogResult = "F";

                    if (noneInTurbineAndCombustionList && (MpcValue != 14))
                      Category.CheckCatalogResult = "F";
                  }

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
        ReturnVal = Category.CheckEngine.FormatError(ex, "SPAN1");
      }

      return ReturnVal;
    }

    public static string SPAN2(cCategory Category, ref bool Log)
    // Span MEC Value Valid 
    {
      string ReturnVal = "";

      try
      {
        if (Category.GetCheckParameter("Span_Component_Type_Code_Valid").ValueAsBool())
        {
          sFilterPair[] RowFilter;

          DataRowView CurrentSpan = (DataRowView)Category.GetCheckParameter("Current_Span").ParameterValue;
          DateTime SpanEvalBeganDate = cDBConvert.ToDate(Category.GetCheckParameter("Span_Evaluation_Begin_Date").ParameterValue, DateTypes.START);
          int SpanEvalBeganHour = cDBConvert.ToHour(Category.GetCheckParameter("Span_Evaluation_Begin_Hour").ParameterValue, DateTypes.START);
          DateTime SpanEvalEndedDate = cDBConvert.ToDate(Category.GetCheckParameter("Span_Evaluation_End_Date").ParameterValue, DateTypes.END);
          int SpanEvalEndedHour = cDBConvert.ToHour(Category.GetCheckParameter("Span_Evaluation_End_Hour").ParameterValue, DateTypes.END);

          string ComponentTypeCd = cDBConvert.ToString(CurrentSpan["Component_Type_Cd"]);
          string SpanScaleCd = cDBConvert.ToString(CurrentSpan["Span_Scale_Cd"]);

          if (CurrentSpan["Mec_Value"] == DBNull.Value)
          {
              if( ComponentTypeCd.InList( "SO2,NOX" ) && ( SpanScaleCd == "L" ) )
            {
              Category.SetCheckParameter("Span_MEC_Value_Valid", false, eParameterDataType.Boolean);
              Category.CheckCatalogResult = "A";
            }
            else if( ComponentTypeCd.InList( "SO2,NOX" ) && ( SpanScaleCd == "H" ) &&
                     (CurrentSpan["Default_High_Range"] != DBNull.Value))
            {
              Category.SetCheckParameter("Span_MEC_Value_Valid", false, eParameterDataType.Boolean);
              Category.CheckCatalogResult = "B";
            }
            else if ((ComponentTypeCd == "SO2") && (SpanScaleCd == "H"))
            {
              DataView LocationControlRecords = (DataView)Category.GetCheckParameter("Location_Control_Records").ParameterValue;

              RowFilter = new sFilterPair[1];
              RowFilter[0].Set("CE_Param", "SO2");

              DateTime SpanAltEndDate = (SpanEvalEndedDate != DateTime.MaxValue) 
                                      ? SpanEvalEndedDate.AddDays(-180) 
                                      : SpanEvalEndedDate;

              DataView LocationControlView = FindActiveRows(LocationControlRecords,
                                                            SpanEvalBeganDate, SpanAltEndDate,
                                                            "Install_Date", "Retire_Date",
                                                            true, true,
                                                            RowFilter);

              if (LocationControlView.Count > 0)
              {
                DataView LocationAttributeRecords = (DataView)Category.GetCheckParameter("Location_Attribute_Records").ParameterValue;

                RowFilter = new sFilterPair[1];
                RowFilter[0].Set("BYPASS_IND", "1");

                DataView LocationAttributeView = FindActiveRows(LocationAttributeRecords,
                                                                SpanEvalBeganDate, SpanEvalEndedDate,
                                                                RowFilter);

                if (LocationAttributeView.Count == 0)
                {
                    Category.CheckCatalogResult = "C";
                    return ReturnVal;
                }
              }
            }
            else if ((ComponentTypeCd == "NOX") && (SpanScaleCd == "H"))
            {
              DataView LocationControlRecords = (DataView)Category.GetCheckParameter("Location_Control_Records").ParameterValue;

              RowFilter = new sFilterPair[2];
              RowFilter[0].Set("CE_Param", "NOX");
              RowFilter[1].Set("Control_Cd", "H2O,STM,SCR,SNCR,DLNB,NH3", eFilterPairStringCompare.InList);

              DateTime SpanAltEndDate = (SpanEvalEndedDate != DateTime.MaxValue)
                                      ? SpanEvalEndedDate.AddDays(-180)
                                      : SpanEvalEndedDate;


              DataView LocationControlView = FindActiveRows(LocationControlRecords,
                                                            SpanEvalBeganDate, SpanAltEndDate,
                                                            "Install_Date", "Retire_Date",
                                                            true, true,
                                                            RowFilter);

              if (LocationControlView.Count > 0)
              {
                DataView LocationAttributeRecords = (DataView)Category.GetCheckParameter("Location_Attribute_Records").ParameterValue;

                RowFilter = new sFilterPair[1];
                RowFilter[0].Set("BYPASS_IND", "1");

                DataView LocationAttributeView = FindActiveRows(LocationAttributeRecords,
                                                                SpanEvalBeganDate, SpanEvalEndedDate,
                                                                RowFilter);

                if (LocationAttributeView.Count == 0)
                Category.CheckCatalogResult = "E";
              }
            }
          }
          else
          {
              if( ComponentTypeCd.InList( "FLOW,O2,HG,HCL" ) )
            {
              Category.SetCheckParameter("Span_MEC_Value_Valid", false, eParameterDataType.Boolean);
              Category.CheckCatalogResult = "F";
            }
            else if (cDBConvert.ToDecimal(CurrentSpan["Mec_Value"]) <= 0)
            {
              Category.SetCheckParameter("Span_MEC_Value_Valid", false, eParameterDataType.Boolean);
              Category.CheckCatalogResult = "G";
            }
          }
        }
        else
          Log = false;
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "SPAN2");
      }

      return ReturnVal;
    }

    public static string SPAN3(cCategory Category, ref bool Log)
    // Span MPF Value Valid
    {
      string ReturnVal = "";

      try
      {
        if (cDBConvert.ToBoolean(Category.GetCheckParameter("Span_Component_Type_Code_Valid").ParameterValue))
        {
          DataRowView CurrentSpan = (DataRowView)Category.GetCheckParameter("Current_Span").ParameterValue;
          string ComponentTypeCd = cDBConvert.ToString(CurrentSpan["Component_Type_Cd"]);

          if (ComponentTypeCd == "FLOW")
          {
            if (CurrentSpan["Mpf_Value"] == DBNull.Value)
              Category.CheckCatalogResult = "A";
            else if (cDBConvert.ToLong(CurrentSpan["Mpf_Value"]) < 500000)
              Category.CheckCatalogResult = "B";
          }
          else if (CurrentSpan["Mpf_Value"] != DBNull.Value)
            Category.CheckCatalogResult = "C";
        }
        else
          Log = false;
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "SPAN3");
      }

      return ReturnVal;
    }

    public static string SPAN4(cCategory Category, ref bool Log)
    // Span Scale Transition Point Value (Max Low Range) Valid
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentSpan = (DataRowView)Category.GetCheckParameter("Current_Span").ParameterValue;
        DateTime EvalBeganDate = cDBConvert.ToDate(Category.GetCheckParameter("Span_Evaluation_Begin_Date").ParameterValue, DateTypes.START);
        int EvalBeganHour = cDBConvert.ToHour(Category.GetCheckParameter("Span_Evaluation_Begin_Hour").ParameterValue, DateTypes.START);
        DateTime EvalEndedDate = cDBConvert.ToDate(Category.GetCheckParameter("Span_Evaluation_End_Date").ParameterValue, DateTypes.END);
        int EvalEndedHour = cDBConvert.ToHour(Category.GetCheckParameter("Span_Evaluation_End_Hour").ParameterValue, DateTypes.END);

        string SpanScaleCd = cDBConvert.ToString(CurrentSpan["Span_Scale_Cd"]);
        string ComponentTypeCd = cDBConvert.ToString(CurrentSpan["Component_Type_Cd"]);

        if (cDBConvert.ToBoolean(Category.GetCheckParameter("Span_Component_Type_Code_Valid").ParameterValue) &&
            (ComponentTypeCd != "HG") && (SpanScaleCd == "H"))
        {
          if (CurrentSpan["Max_Low_Range"] != DBNull.Value)
          {
            if ((CurrentSpan["Span_Value"] == DBNull.Value) &&
                (CurrentSpan["Default_High_Range"] != DBNull.Value))
              Category.CheckCatalogResult = "A";
            else
            {

              if (cDBConvert.ToBoolean(Category.GetCheckParameter("Span_Dates_and_Hours_Consistent").ParameterValue))
              {
                sFilterPair[] RowFilter;

                DataView SpanRecords = (DataView)Category.GetCheckParameter("Span_Records").ParameterValue;


                RowFilter = new sFilterPair[2];
                RowFilter[0].Set("Component_Type_Cd", ComponentTypeCd);
                RowFilter[1].Set("Span_Scale_Cd", "L");

                DataView SpanView = FindActiveRows(SpanRecords,
                                                   EvalBeganDate, EvalBeganHour, EvalEndedDate, EvalEndedHour,
                                                   RowFilter);

                if (SpanView.Count > 0)
                {
                  decimal CurrMaxLowRange = cDBConvert.ToDecimal(CurrentSpan["Max_Low_Range"]);
                  string SpanFilter = SpanView.RowFilter;
                  SpanView.RowFilter = AddToDataViewFilter(SpanFilter, "max_low_range is null or max_low_range <> " + CurrMaxLowRange);
                  if (SpanView.Count == 0)
                  {
                    SpanView.RowFilter = SpanFilter;
                    foreach (DataRowView row in SpanView)
                    {
                      decimal MaxLowRange = cDBConvert.ToDecimal(row["Max_Low_Range"]);
                      decimal FullScaleRange = cDBConvert.ToDecimal(row["Full_Scale_Range"]);
                      decimal HalfFullScaleRange = 0.5m * FullScaleRange;
                      if (FullScaleRange != decimal.MinValue && !(InRange(MaxLowRange, HalfFullScaleRange, FullScaleRange)))
                      {
                        Category.CheckCatalogResult = "B";
                        break;
                      }
                    }

                  }
                  else
                    Category.CheckCatalogResult = "C";
                  SpanView.RowFilter = SpanFilter;
                }
              }
            }
          }
          else if (cDBConvert.ToBoolean(Category.GetCheckParameter("Span_Dates_and_Hours_Consistent").ParameterValue))
          {
            if ((CurrentSpan["END_DATE"] == DBNull.Value) ||
                cDBConvert.ToDate(CurrentSpan["END_DATE"], DateTypes.END) >= Category.GetCheckParameter("ECMPS_MP_Begin_Date").ValueAsDateTime(DateTypes.START))
            {
            sFilterPair[] RowFilter;

            DataView LocationAnalyzerRangeRecords = (DataView)Category.GetCheckParameter("Location_Analyzer_Range_Records").ParameterValue;

            RowFilter = new sFilterPair[2];
            RowFilter[0].Set("Component_Type_Cd", ComponentTypeCd);
            RowFilter[1].Set("Dual_Range_Ind", 1, eFilterDataType.Integer);

            DataView LocationAnalyzerRangeView = FindActiveRows(LocationAnalyzerRangeRecords,
                                                                EvalBeganDate, EvalBeganHour, EvalEndedDate, EvalEndedHour,
                                                                RowFilter);

            if (LocationAnalyzerRangeView.Count > 0)
              Category.CheckCatalogResult = "D";
            }
          }
        }
        else
          Log = false;
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "SPAN4");
      }

      return ReturnVal;
    }

    public static string SPAN6(cCategory Category, ref bool Log)
    // Span Value Valid
    {
      string ReturnVal = "";

      try
      {
        if (cDBConvert.ToBoolean(Category.GetCheckParameter("Span_Component_Type_Code_Valid").ParameterValue))
        {
          DataRowView CurrentSpan = (DataRowView)Category.GetCheckParameter("Current_Span").ParameterValue;
          string ComponentTypeCd = cDBConvert.ToString(CurrentSpan["Component_Type_Cd"]);

          if (CurrentSpan["Span_Value"] == DBNull.Value)
          {
              if( !ComponentTypeCd.InList( "NOX,SO2" ) ||
                (cDBConvert.ToString(CurrentSpan["Span_Scale_Cd"]) != "H"))
              Category.CheckCatalogResult = "A";
          }
          else
          {
            decimal MinimumSpanValue;

            if (ComponentTypeCd == "FLOW")
            {
              string SpanUomCd = cDBConvert.ToString(CurrentSpan["Span_Uom_Cd"]);

              if (SpanUomCd == "SCFH")
                MinimumSpanValue = 500000;
              else
                MinimumSpanValue = 0.001m;
            }
            else if( ComponentTypeCd.InList( "SO2,NOX,HG,HCL" ) )
              MinimumSpanValue = 1;
            else if( ComponentTypeCd.InList( "CO2,O2" ) )
              MinimumSpanValue = 0.1m;
            else
              MinimumSpanValue = decimal.MinValue;

            Category.SetCheckParameter("Minimum_Span_Value", MinimumSpanValue, eParameterDataType.Decimal);

            decimal SpanValue = cDBConvert.ToDecimal(CurrentSpan["Span_Value"]);

            if (SpanValue < MinimumSpanValue)
              Category.CheckCatalogResult = "B";
            else
            {
              decimal MaximumSpanValue = cDBConvert.ToDecimal(Category.GetCheckParameter("Maximum_Span_Value").ParameterValue);

              if ((MaximumSpanValue != decimal.MinValue) && (SpanValue > MaximumSpanValue))
                Category.CheckCatalogResult = "C";
              else if (ComponentTypeCd.NotInList("FLOW,HG,HCL"))
              {
                string SpanScaleCd = cDBConvert.ToString(CurrentSpan["Span_Scale_Cd"]);

                if ((SpanScaleCd == "H") &&
                    cDBConvert.ToBoolean(Category.GetCheckParameter("Span_MPC_Value_Valid").ParameterValue))
                {
                  Category.SetCheckParameter("MPC_MEC", "MPC", eParameterDataType.String);

                  decimal MpcValue = cDBConvert.ToDecimal(CurrentSpan["Mpc_Value"]);

                  if (SpanValue < MpcValue)
                    Category.CheckCatalogResult = "D";
                  else if( ComponentTypeCd.InList( "SO2,NOX" ) &&
                           (SpanValue > (100 * Math.Ceiling(1.25m * MpcValue / 100))))
                    Category.CheckCatalogResult = "E";
                }
                else if ((SpanScaleCd == "L") &&
                    cDBConvert.ToBoolean(Category.GetCheckParameter("Span_MEC_Value_Valid").ParameterValue))
                {
                  Category.SetCheckParameter("MPC_MEC", "MEC", eParameterDataType.String);

                  decimal MecValue = cDBConvert.ToDecimal(CurrentSpan["Mec_Value"]);

                  if (SpanValue < MecValue)
                    Category.CheckCatalogResult = "D";
                  else if( ComponentTypeCd.InList( "SO2,NOX" ) &&
                           (SpanValue > (100 * Math.Ceiling(1.25m * MecValue / 100))))
                    Category.CheckCatalogResult = "E";
                }
              }
            }
          }

          Category.SetCheckParameter("Span_Value_Valid", (Category.CheckCatalogResult == null), eParameterDataType.Boolean);

        }
        else
          Log = false;
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "SPAN6");
      }

      return ReturnVal;
    }

    public static string SPAN7(cCategory Category, ref bool Log)
    // Span Full Scale Range Value Valid 
    {
      string ReturnVal = "";

      try
      {
        if (cDBConvert.ToBoolean(Category.GetCheckParameter("Span_Component_Type_Code_Valid").ParameterValue))
        {
          DataRowView CurrentSpan = (DataRowView)Category.GetCheckParameter("Current_Span").ParameterValue;

          string ComponentTypeCd = cDBConvert.ToString(CurrentSpan["Component_Type_Cd"]);

          if (CurrentSpan["Full_Scale_Range"] == DBNull.Value)
          {
              if( !ComponentTypeCd.InList( "NOX,SO2" ) ||
                (CurrentSpan["Default_High_Range"] == DBNull.Value) ||
                (cDBConvert.ToString(CurrentSpan["Span_Scale_Cd"]) == "L"))
              Category.CheckCatalogResult = "A";
          }
          else if (cDBConvert.ToBoolean(Category.GetCheckParameter("Span_Value_Valid").ParameterValue) &&
                   (cDBConvert.ToDecimal(CurrentSpan["Full_Scale_Range"]) < cDBConvert.ToDecimal(CurrentSpan["Span_Value"])))
            Category.CheckCatalogResult = "B";

          Category.SetCheckParameter("Span_Full_Scale_Range_Value_Valid", (Category.CheckCatalogResult == null), eParameterDataType.Boolean);
        }
        else
          Log = false;
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "SPAN7");
      }

      return ReturnVal;
    }

    public static string SPAN8(cCategory Category, ref bool Log)
    // Span Begin Date Valid
    {
      string ReturnVal = "";

      try
      {
        ReturnVal = Check_ValidStartDate(Category, "Span_Start_Date_Valid", "Current_Span");
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "SPAN8");
      }

      return ReturnVal;
    }

    public static string SPAN9(cCategory Category, ref bool Log)
    // Span Begin Hour Valid
    {
      string ReturnVal = "";

      try
      {
        ReturnVal = Check_ValidStartHour(Category, "Span_Start_Hour_Valid", "Current_Span");
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "SPAN9");
      }

      return ReturnVal;
    }

    public static string SPAN10(cCategory Category, ref bool Log)
    // Span End Date Valid
    {
      string ReturnVal = "";

      try
      {
        ReturnVal = Check_ValidEndDate(Category, "Span_End_Date_Valid", "Current_Span");
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "SPAN10");
      }

      return ReturnVal;
    }

    public static string SPAN11(cCategory Category, ref bool Log)
    // Span End Hour Valid
    {
      string ReturnVal = "";

      try
      {
        ReturnVal = Check_ValidEndHour(Category, "Span_End_Hour_Valid", "Current_Span");
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "SPAN11");
      }

      return ReturnVal;
    }

    public static string SPAN12(cCategory Category, ref bool Log)
    // Span Dates and Hours Consistent
    {
      string ReturnVal = "";

      try
      {
        ReturnVal = Check_ConsistentHourRange(Category, "Span_Dates_and_Hours_Consistent",
                                                        "Current_Span",
                                                        "Span_Start_Date_Valid",
                                                        "Span_Start_Hour_Valid",
                                                        "Span_End_Date_Valid",
                                                        "Span_End_Hour_Valid");
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "SPAN12");
      }

      return ReturnVal;
    }

    public static string SPAN13(cCategory Category, ref bool Log)
    // Span Active Status
    {
      string ReturnVal = "";

      try
      {
        bool SpanDatesAndHoursConsistent = cDBConvert.ToBoolean(Category.GetCheckParameter("Span_Dates_and_Hours_Consistent").ParameterValue);

        if (SpanDatesAndHoursConsistent)
          ReturnVal = Check_ActiveHourRange(Category, "Span_Active_Status",
                                                      "Current_Span",
                                                      "Span_Evaluation_Begin_Date",
                                                      "Span_Evaluation_Begin_Hour",
                                                      "Span_Evaluation_End_Date",
                                                      "Span_Evaluation_End_Hour");
        else
          Log = false;
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "SPAN13");
      }

      return ReturnVal;
    }

    public static string SPAN16(cCategory Category, ref bool Log)
    // Flow Span Value Valid
    {
      string ReturnVal = "";

      try
      {
        if (Convert.ToBoolean(Category.GetCheckParameter("Span_Component_Type_Code_Valid").ParameterValue))
        {
          DataRowView CurrentSpan = (DataRowView)Category.GetCheckParameter("Current_Span").ParameterValue;
          string ComponentTypeCd = cDBConvert.ToString(CurrentSpan["Component_Type_Cd"]);
          long FlowSpanValue = cDBConvert.ToLong(CurrentSpan["Flow_Span_Value"]);

          if (ComponentTypeCd == "FLOW")
          {
            if (FlowSpanValue == long.MinValue)
              Category.CheckCatalogResult = "A";
            else
            {
              decimal MpfValue = cDBConvert.ToDecimal(CurrentSpan["Mpf_Value"]);

              if (MpfValue > 0 &&
                  (FlowSpanValue < MpfValue ||
                   FlowSpanValue > Math.Round(MpfValue * 1.25m / 1000, 0, MidpointRounding.AwayFromZero) * 1000))
                Category.CheckCatalogResult = "B";
              else if (Convert.ToBoolean(Category.GetCheckParameter("Span_Value_Valid").ParameterValue))
              {
                decimal SpanValue = cDBConvert.ToDecimal(CurrentSpan["Span_Value"]);
                string SpanUomCd = cDBConvert.ToString(CurrentSpan["Span_Uom_Cd"]);

                if (SpanUomCd == "SCFH")
                {
                  if (SpanValue != FlowSpanValue)
                    Category.CheckCatalogResult = "C";
                }
                else
                {
                  SpanValue = Math.Round(SpanValue);
                  if (SpanUomCd == "KSCFH")
                  {
                    if (SpanValue != Math.Round((decimal)FlowSpanValue / 1000))
                      Category.CheckCatalogResult = "C";
                  }
                  else
                    if (SpanUomCd == "MSCFH")
                    {
                      if (SpanValue != Math.Round((decimal)FlowSpanValue / 1000000))
                        Category.CheckCatalogResult = "C";
                    }
                    else
                      if (SpanUomCd == "SCFM")
                      {
                        if (SpanValue != Math.Round((decimal)FlowSpanValue / 60))
                          Category.CheckCatalogResult = "C";
                      }
                      else
                        if (SpanUomCd == "KSCFM")
                        {
                          if (SpanValue != Math.Round((decimal)FlowSpanValue / 60000))
                            Category.CheckCatalogResult = "C";
                        }
                }
              }
            }
          }
          else
            if (FlowSpanValue != long.MinValue)
              Category.CheckCatalogResult = "D";

          Category.SetCheckParameter("Flow_Span_Value_Valid", (Category.CheckCatalogResult == null), eParameterDataType.Boolean);
        }
        else
          Log = false;
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "SPAN16");
      }

      return ReturnVal;
    }

    public static string SPAN17(cCategory Category, ref bool Log)
    // Flow Span Full Scale Range Value Valid
    {
      string ReturnVal = "";

      try
      {
        if (cDBConvert.ToBoolean(Category.GetCheckParameter("Span_Component_Type_Code_Valid").ParameterValue))
        {
          DataRowView CurrentSpan = (DataRowView)Category.GetCheckParameter("Current_Span").ParameterValue;
          string ComponentTypeCd = cDBConvert.ToString(CurrentSpan["Component_Type_Cd"]);

          if (ComponentTypeCd == "FLOW")
          {
            if (CurrentSpan["Flow_Full_Scale_Range"] == DBNull.Value)
              Category.CheckCatalogResult = "A";
            else if (cDBConvert.ToBoolean(Category.GetCheckParameter("Flow_Span_Value_Valid").ParameterValue) &&
                     (cDBConvert.ToDecimal(CurrentSpan["Flow_Full_Scale_Range"]) < cDBConvert.ToDecimal(CurrentSpan["Flow_Span_Value"])))
              Category.CheckCatalogResult = "B";
          }
          else if (CurrentSpan["Flow_Full_Scale_Range"] != DBNull.Value)
            Category.CheckCatalogResult = "C";
        }
        else
          Log = false;
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "SPAN17");
      }

      return ReturnVal;
    }

    public static string SPAN18(cCategory Category, ref bool Log)
    // Span Scale Code Valid 
    {
      string ReturnVal = "";

      try
      {
        if (MpParameters.SpanComponentTypeCodeValid.AsBoolean().Default())
        {
            MpParameters.SpanScaleCodeValid = true;

          if (MpParameters.CurrentSpan.ComponentTypeCd != "FLOW")
          {
              if (MpParameters.CurrentSpan.SpanScaleCd == null)
              {
                  MpParameters.SpanScaleCodeValid = false;
                  Category.CheckCatalogResult = "A";
              }
              else if (!MpParameters.CurrentSpan.SpanScaleCd.InList("H,L"))
              {
                  MpParameters.SpanScaleCodeValid = false;
                  Category.CheckCatalogResult = "B";
              }
              else if (MpParameters.CurrentSpan.ComponentTypeCd.InList("HG,HCL") && MpParameters.CurrentSpan.SpanScaleCd != "H")
              {
                  MpParameters.SpanScaleCodeValid = false;
                  Category.CheckCatalogResult = "D";
              }
          }
          else if (MpParameters.CurrentSpan.ComponentTypeCd == "FLOW" && MpParameters.CurrentSpan.SpanScaleCd != null)
          {
              MpParameters.SpanScaleCodeValid = false;
              Category.CheckCatalogResult = "C";
          }
        }
        else
          Log = false;
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "SPAN18");
      }

      return ReturnVal;
    }

    public static string SPAN20(cCategory Category, ref bool Log)
    // Span Component Type Code Valid
    {
      string ReturnVal = "";

      try
      {
        Category.SetCheckParameter("Span_MPC_Value_Valid", true, eParameterDataType.Boolean);
        Category.SetCheckParameter("Span_MEC_Value_Valid", true, eParameterDataType.Boolean);

        DataRowView CurrentSpan = (DataRowView)Category.GetCheckParameter("Current_Span").ParameterValue;

        if (CurrentSpan["Component_Type_Cd"] == DBNull.Value)
        {
          Category.SetCheckParameter("Span_Component_Type_Code_Valid", false, eParameterDataType.Boolean);
          Category.CheckCatalogResult = "A";
        }
        else
        {
          string ComponentTypeCd = cDBConvert.ToString(CurrentSpan["Component_Type_Cd"]);
          DataView ComponentTypeRecords = (DataView)Category.GetCheckParameter("Component_Type_Code_Lookup_Table").ParameterValue;
          DataRowView ComponentTypeRow;
          sFilterPair[] ComponentTypeFilter = new sFilterPair[2];

          ComponentTypeFilter[0].Set("Component_Type_Cd", ComponentTypeCd);
          ComponentTypeFilter[1].Set("Span_Indicator", 1, eFilterDataType.Integer);

          if (!FindRow(ComponentTypeRecords, ComponentTypeFilter, out ComponentTypeRow))
          {
            Category.SetCheckParameter("Span_Component_Type_Code_Valid", false, eParameterDataType.Boolean);
            Category.CheckCatalogResult = "B";
          }
          else
          {
            string ComponentTypeParameter = cDBConvert.ToString(ComponentTypeRow["Parameter_Cd"]);

            Category.SetCheckParameter("Component_Parameter_Code", ComponentTypeParameter, eParameterDataType.String);
            Category.SetCheckParameter("Span_Component_Type_Code_Valid", true, eParameterDataType.Boolean);
          }
        }
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "SPAN20");
      }

      return ReturnVal;
    }

    public static string SPAN21(cCategory Category, ref bool Log)
    // Span Units of Measure Code Valid
    {
      string ReturnVal = "";

      try
      {
        if (cDBConvert.ToBoolean(Category.GetCheckParameter("Span_Component_Type_Code_Valid").ParameterValue))
        {
          Category.SetCheckParameter("Maximum_Span_Value", null, eParameterDataType.Decimal);

          DataRowView CurrentSpan = (DataRowView)Category.GetCheckParameter("Current_Span").ParameterValue;

          if (CurrentSpan["Span_Uom_Cd"] == DBNull.Value)
            Category.CheckCatalogResult = "A";
          else
          {
            sFilterPair[] RowFilter;

            string ComponentParameterCd = cDBConvert.ToString(Category.GetCheckParameter("Component_Parameter_Code").ParameterValue);
            DataView ParameterUnitsOfMeasureLookupTable = (DataView)Category.GetCheckParameter("Parameter_Units_Of_Measure_Lookup_Table").ParameterValue;

            string SpanUomCd = cDBConvert.ToString(CurrentSpan["Span_Uom_Cd"]);

            RowFilter = new sFilterPair[2];
            RowFilter[0].Set("Parameter_Cd", ComponentParameterCd);
            RowFilter[1].Set("Uom_Cd", SpanUomCd);

            DataView ParameterUnitsOfMeasureView = FindRows(ParameterUnitsOfMeasureLookupTable, RowFilter);

            if (ParameterUnitsOfMeasureView.Count > 0)
            {
              if (ParameterUnitsOfMeasureView[0]["Max_Value"] != DBNull.Value)
                Category.SetCheckParameter("Maximum_Span_Value", ParameterUnitsOfMeasureView[0]["Max_Value"], eParameterDataType.Decimal);
              else
                Category.SetCheckParameter("Maximum_Span_Value", null, eParameterDataType.Decimal);
            }
            else
            {
              DataView UnitsOfMeasureCodeLookupTable = (DataView)Category.GetCheckParameter("Units_of_Measure_Code_Lookup_Table").ParameterValue;

              RowFilter = new sFilterPair[1];
              RowFilter[0].Set("Uom_Cd", SpanUomCd);

              DataView UnitsOfMeasureCodeView = FindRows(UnitsOfMeasureCodeLookupTable, RowFilter);

              if (UnitsOfMeasureCodeView.Count == 0)
                Category.CheckCatalogResult = "B";
              else
                Category.CheckCatalogResult = "C";
            }
          }
        }
        else
          Log = false;
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "SPAN21");
      }

      return ReturnVal;
    }

    public static string SPAN36(cCategory Category, ref bool Log)
    // Span Default High Range Value Valid
    {
      string ReturnVal = "";

      try
      {
        if (cDBConvert.ToBoolean(Category.GetCheckParameter("Span_Component_Type_Code_Valid").ParameterValue))
        {
          DataRowView CurrentSpan = (DataRowView)Category.GetCheckParameter("Current_Span").ParameterValue;

          if (CurrentSpan["Default_High_Range"] != DBNull.Value)
          {
            string ComponentTypeCd = cDBConvert.ToString(CurrentSpan["Component_Type_Cd"]);
            string SpanScaleCd = cDBConvert.ToString(CurrentSpan["Span_Scale_Cd"]);
            long DefaultHighRange = cDBConvert.ToLong(CurrentSpan["Default_High_Range"]);
            decimal MpcValue = cDBConvert.ToDecimal(CurrentSpan["Mpc_Value"]);
            DateTime EndDate = cDBConvert.ToDate(CurrentSpan["End_Date"], DateTypes.END);

            if( !ComponentTypeCd.InList( "SO2,NOX" ) || ( SpanScaleCd == "L" ) )
              Category.CheckCatalogResult = "A";
            else if (DefaultHighRange <= 0)
              Category.CheckCatalogResult = "B";
            else if ((MpcValue > 0) && (MpcValue != decimal.MinValue) && (DefaultHighRange != (2 * MpcValue)))
            {
              if ((ComponentTypeCd != "NOX") || (MpcValue != 50) || (DefaultHighRange != 200) ||
                  (EndDate == DateTime.MaxValue) || (EndDate > new DateTime(2003, 3, 31)))
                Category.CheckCatalogResult = "C";
            }
          }
          else if (CurrentSpan["Span_Value"] == DBNull.Value)
          {
            string ComponentTypeCd = cDBConvert.ToString(CurrentSpan["Component_Type_Cd"]);
            string SpanScaleCd = cDBConvert.ToString(CurrentSpan["Span_Scale_Cd"]);

            if( ( SpanScaleCd == "H" ) && ComponentTypeCd.InList( "NOX,SO2" ) )
              Category.CheckCatalogResult = "D";
          }

          Category.SetCheckParameter("Span_Default_High_Range_Value_Valid", (Category.CheckCatalogResult == null), eParameterDataType.Boolean);
        }
        else
          Log = false;
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "SPAN36");
      }

      return ReturnVal;
    }

    public static string SPAN37(cCategory Category, ref bool Log)
    // Default High Range Value Consistent with Span Value and Full Scale Range
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentSpan = (DataRowView)Category.GetCheckParameter("Current_Span").ParameterValue;
        string ComponentTypeCd = cDBConvert.ToString(CurrentSpan["Component_Type_Cd"]);
        string SpanScaleCd = cDBConvert.ToString(CurrentSpan["Span_Scale_Cd"]);

        if( ComponentTypeCd.InList( "SO2,NOX" ) && ( SpanScaleCd == "H" ) )
        {
          if ((CurrentSpan["Default_High_Range"] != DBNull.Value) &&
              ((CurrentSpan["Span_Value"] != DBNull.Value) ||
               (CurrentSpan["Full_Scale_Range"] != DBNull.Value)))
            Category.CheckCatalogResult = "A";
        }
        else
          Log = false;
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "SPAN37");
      }

      return ReturnVal;
    }

    public static string SPAN47(cCategory Category, ref bool Log)
    // High Scale Span Consistent with Low Scale Span 
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentSpan = (DataRowView)Category.GetCheckParameter("Current_Span").ParameterValue;
        string SpanScaleCd = cDBConvert.ToString(CurrentSpan["Span_Scale_Cd"]);

        if (cDBConvert.ToBoolean(Category.GetCheckParameter("Span_Component_Type_Code_Valid").ParameterValue) &&
            (SpanScaleCd == "L"))
        {
          sFilterPair[] SpanFilter;

          DateTime EvalBeganDate = cDBConvert.ToDate(Category.GetCheckParameter("Span_Evaluation_Begin_Date").ParameterValue, DateTypes.START);
          int EvalBeganHour = cDBConvert.ToHour(Category.GetCheckParameter("Span_Evaluation_Begin_Hour").ParameterValue, DateTypes.START);
          DateTime EvalEndedDate = cDBConvert.ToDate(Category.GetCheckParameter("Span_Evaluation_End_Date").ParameterValue, DateTypes.END);
          int EvalEndedHour = cDBConvert.ToHour(Category.GetCheckParameter("Span_Evaluation_End_Hour").ParameterValue, DateTypes.END);
          DataView SpanRecords = (DataView)Category.GetCheckParameter("Span_Records").ParameterValue;
          bool SpanValueValid = cDBConvert.ToBoolean(Category.GetCheckParameter("Span_Value_Valid").ParameterValue);

          string ComponentTypeCd = cDBConvert.ToString(CurrentSpan["Component_Type_Cd"]);
          decimal LoMecValue = cDBConvert.ToDecimal(CurrentSpan["Mec_Value"]);
          decimal LoSpanValue = cDBConvert.ToDecimal(CurrentSpan["Span_Value"]);

          SpanFilter = new sFilterPair[2];
          SpanFilter[0].Set("Component_Type_Cd", ComponentTypeCd);
          SpanFilter[1].Set("Span_Scale_Cd", "H");

          DataView SpanView = FindActiveRows(SpanRecords,
                                             EvalBeganDate, EvalBeganHour, EvalEndedDate, EvalEndedHour,
                                             SpanFilter);

          bool SpanValueProblem = false;
          bool MecValueProblem = false;

          foreach (DataRowView SpanRow in SpanView)
          {
            if (SpanValueValid)
            {
              decimal HiSpanValue = cDBConvert.ToDecimal(SpanRow["Span_Value"]);

              if ((HiSpanValue > 0) && (HiSpanValue < LoSpanValue))
                SpanValueProblem = true;
            }

            if (LoMecValue > 0)
            {
              decimal HiMecValue = cDBConvert.ToDecimal(SpanRow["Mec_Value"]);

              if ((HiMecValue > 0) && (HiMecValue != LoMecValue))
                MecValueProblem = true;
            }
          }

          if (SpanValueProblem)
            Category.CheckCatalogResult = "A";
          else if (MecValueProblem)
            Category.CheckCatalogResult = "B";
        }
        else
          Log = false;
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "SPAN47");
      }

      return ReturnVal;
    }

    public static string SPAN48(cCategory Category, ref bool Log)
    // Required Low Scale Span Record Reported for Low MEC or Default High Range 
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentSpan = (DataRowView)Category.GetCheckParameter("Current_Span").ParameterValue;
        string ComponentTypeCd = cDBConvert.ToString(CurrentSpan["Component_Type_Cd"]);
        string SpanScaleCd = cDBConvert.ToString(CurrentSpan["Span_Scale_Cd"]);

        if (SpanScaleCd == "H")
        {

            if( ComponentTypeCd.InList( "SO2,NOX" ) )
          {
            if ((cDBConvert.ToBoolean(Category.GetCheckParameter("Span_Full_Scale_Range_Value_Valid").ParameterValue)) &&
                (CurrentSpan["Full_Scale_range"] != DBNull.Value) &&
                (cDBConvert.ToBoolean(Category.GetCheckParameter("Span_MEC_Value_Valid").ParameterValue)) &&
                (CurrentSpan["Mec_Value"] != DBNull.Value))
            {
              decimal FullScaleRange = cDBConvert.ToDecimal(CurrentSpan["Full_Scale_Range"]);
              decimal MecValue = cDBConvert.ToDecimal(CurrentSpan["Mec_Value"]);

              if (MecValue < (0.2m * FullScaleRange))
              {
                DateTime EvalBeganDate = cDBConvert.ToDate(Category.GetCheckParameter("Span_Evaluation_Begin_Date").ParameterValue, DateTypes.START);
                int EvalBeganHour = cDBConvert.ToHour(Category.GetCheckParameter("Span_Evaluation_Begin_Hour").ParameterValue, DateTypes.START);
                DateTime EvalEndedDate = cDBConvert.ToDate(Category.GetCheckParameter("Span_Evaluation_End_Date").ParameterValue, DateTypes.END);
                int EvalEndedHour = cDBConvert.ToHour(Category.GetCheckParameter("Span_Evaluation_End_Hour").ParameterValue, DateTypes.END);

                DataView SpanRecords = (DataView)Category.GetCheckParameter("Span_Records").ParameterValue;
                sFilterPair[] SpanFilter = new sFilterPair[2];

                SpanFilter[0].Set("Component_Type_Cd", ComponentTypeCd);
                SpanFilter[1].Set("Span_Scale_Cd", "L");

                DataView SpanView = FindActiveRows(SpanRecords,
                                                   EvalBeganDate, EvalBeganHour, EvalEndedDate, EvalEndedHour,
                                                   SpanFilter);

                if (SpanView.Count == 0)
                  Category.CheckCatalogResult = "A";
              }
            }
            else if ((cDBConvert.ToBoolean(Category.GetCheckParameter("Span_Default_High_Range_Value_Valid").ParameterValue)) &&
                     (CurrentSpan["Default_High_Range"] != DBNull.Value))
            {
              DateTime EvalBeganDate = cDBConvert.ToDate(Category.GetCheckParameter("Span_Evaluation_Begin_Date").ParameterValue, DateTypes.START);
              int EvalBeganHour = cDBConvert.ToHour(Category.GetCheckParameter("Span_Evaluation_Begin_Hour").ParameterValue, DateTypes.START);
              DateTime EvalEndedDate = cDBConvert.ToDate(Category.GetCheckParameter("Span_Evaluation_End_Date").ParameterValue, DateTypes.END);
              int EvalEndedHour = cDBConvert.ToHour(Category.GetCheckParameter("Span_Evaluation_End_Hour").ParameterValue, DateTypes.END);

              DataView SpanRecords = (DataView)Category.GetCheckParameter("Span_Records").ParameterValue;
              sFilterPair[] SpanFilter = new sFilterPair[2];

              SpanFilter[0].Set("Component_Type_Cd", ComponentTypeCd);
              SpanFilter[1].Set("Span_Scale_Cd", "L");

              DataView SpanView = FindActiveRows(SpanRecords,
                                                 EvalBeganDate, EvalBeganHour, EvalEndedDate, EvalEndedHour,
                                                 SpanFilter);

              if (SpanView.Count == 0)
                Category.CheckCatalogResult = "B";
              else
              {
                foreach (DataRowView SpanRow in SpanView)
                {
                  decimal FullScaleRange = cDBConvert.ToDecimal(SpanRow["Full_Scale_Range"]);
                  decimal MecValue = cDBConvert.ToDecimal(SpanRow["Mec_Value"]);
                  decimal MecProxy = 10 * Math.Ceiling(MecValue / 10);

                  if ((MecValue > 0) && (FullScaleRange != decimal.MinValue) &&
                      (MecProxy < (0.2m * FullScaleRange)))
                    Category.CheckCatalogResult = "C";
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
        ReturnVal = Category.CheckEngine.FormatError(ex, "SPAN48");
      }

      return ReturnVal;
    }

    public static string SPAN50(cCategory Category, ref bool Log)
    // Span Method Code Valid
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentSpan = (DataRowView)Category.GetCheckParameter("Current_Span").ParameterValue;
        string ComponentTypeCd = cDBConvert.ToString(CurrentSpan["Component_Type_Cd"]);

        if (cDBConvert.ToBoolean(Category.GetCheckParameter("Span_Component_Type_Code_Valid").ParameterValue) &&
            ((ComponentTypeCd == "FLOW") || cDBConvert.ToBoolean(Category.GetCheckParameter("Span_Scale_Code_Valid").ParameterValue)))
        {
          sFilterPair[] RowFilter;

          DataView CrossCheckRecords = (DataView)Category.GetCheckParameter("Component_Type_And_Span_Scale_To_Span_Method_Cross_Check_Table").ParameterValue;

          if (ComponentTypeCd == "FLOW")
          {
            RowFilter = new sFilterPair[2];
            RowFilter[0].Set("ComponentTypeCode", ComponentTypeCd);
            RowFilter[1].Set("SpanMethodCode", cDBConvert.ToString(CurrentSpan["Span_Method_Cd"]));
          }
          else
          {
            RowFilter = new sFilterPair[3];
            RowFilter[0].Set("ComponentTypeCode", ComponentTypeCd);
            RowFilter[1].Set("SpanScaleCode", cDBConvert.ToString(CurrentSpan["Span_Scale_Cd"]));
            RowFilter[2].Set("SpanMethodCode", cDBConvert.ToString(CurrentSpan["Span_Method_Cd"]));
          }

          DataView CrossCheckView = FindRows(CrossCheckRecords, RowFilter);

          if (CrossCheckView.Count == 0)
          {
            if (CurrentSpan["Span_Method_Cd"] == DBNull.Value)
            {
              if (ComponentTypeCd != "O2")
                Category.CheckCatalogResult = "A";
            }
            else
            {
              DataView SpanMethodRecords = (DataView)Category.GetCheckParameter("Span_Method_Code_Lookup_Table").ParameterValue;

              RowFilter = new sFilterPair[1];
              RowFilter[0].Set("Span_Method_Cd", cDBConvert.ToString(CurrentSpan["Span_Method_Cd"]));

              DataView SpanMethodRView = FindRows(SpanMethodRecords, RowFilter);

              if (SpanMethodRView.Count == 0)
                Category.CheckCatalogResult = "B";
              else
                Category.CheckCatalogResult = "C";
            }
          }
        }
        else
          Log = false;
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "SPAN50");
      }

      return ReturnVal;
    }

    public static string SPAN52(cCategory Category, ref bool Log)
    // Required Component Reported for Span
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentSpan = (DataRowView)Category.GetCheckParameter("Current_Span").ParameterValue;

        if (cDBConvert.ToBoolean(Category.GetCheckParameter("Span_Component_Type_Code_Valid").ParameterValue) &&
            cDBConvert.ToBoolean(Category.GetCheckParameter("Span_Scale_Code_Valid").ParameterValue) &&
            (CurrentSpan["Default_High_Range"] == DBNull.Value))
        {
          sFilterPair[] RowFilter;

          DateTime EvalBeganDate = cDBConvert.ToDate(Category.GetCheckParameter("Span_Evaluation_Begin_Date").ParameterValue, DateTypes.START);
          int EvalBeganHour = cDBConvert.ToHour(Category.GetCheckParameter("Span_Evaluation_Begin_Hour").ParameterValue, DateTypes.START);
          DateTime EvalEndedDate = cDBConvert.ToDate(Category.GetCheckParameter("Span_Evaluation_End_Date").ParameterValue, DateTypes.END);
          int EvalEndedHour = cDBConvert.ToHour(Category.GetCheckParameter("Span_Evaluation_End_Hour").ParameterValue, DateTypes.END);

          string ComponentTypeCd = cDBConvert.ToString(CurrentSpan["Component_Type_Cd"]);

          if (ComponentTypeCd == "FLOW")
          {
            DataView LocationSystemComponentRecords = (DataView)Category.GetCheckParameter("Location_System_Component_Records").ParameterValue;

            RowFilter = new sFilterPair[1];
            RowFilter[0].Set("Component_Type_Cd", ComponentTypeCd);

            DataView LocationSystemComponentView = FindActiveRows(LocationSystemComponentRecords,
                                                                  EvalBeganDate, EvalBeganHour,
                                                                  EvalEndedDate, EvalEndedHour,
                                                                  RowFilter);

            if (LocationSystemComponentView.Count == 0)
              Category.CheckCatalogResult = "A";
            else if (!CheckForHourRangeCovered(Category, LocationSystemComponentView,
                                               EvalBeganDate, EvalBeganHour, EvalEndedDate, EvalEndedHour))
              Category.CheckCatalogResult = "B";
          }
          else
          {
            DataView LocationAnalyzerRangeRecords = (DataView)Category.GetCheckParameter("Location_Analyzer_Range_Records").ParameterValue;

            string SpanScaleCd = cDBConvert.ToString(CurrentSpan["Span_Scale_Cd"]);
            string AnalyzerRangeList = (SpanScaleCd == "") ? "A" : "A," + SpanScaleCd;

            RowFilter = new sFilterPair[2];
            RowFilter[0].Set("Component_Type_Cd", ComponentTypeCd);
            RowFilter[1].Set("Analyzer_Range_Cd", AnalyzerRangeList, eFilterPairStringCompare.InList);

            DataView LocationAnalyzerRangeView = FindActiveRows(LocationAnalyzerRangeRecords,
                                                                EvalBeganDate, EvalBeganHour,
                                                                EvalEndedDate, EvalEndedHour,
                                                                RowFilter);

            if (LocationAnalyzerRangeView.Count == 0)
              Category.CheckCatalogResult = "A";
            else if (!CheckForHourRangeCovered(Category, LocationAnalyzerRangeView,
                                               EvalBeganDate, EvalBeganHour, EvalEndedDate, EvalEndedHour))
              Category.CheckCatalogResult = "B";
          }
        }
        else
          Log = false;
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "SPAN52");
      }

      return ReturnVal;
    }

    public static string SPAN53(cCategory Category, ref bool Log)
    // Overlapping Span Records
    {
      string ReturnVal = "";

      try
      {
        if (cDBConvert.ToBoolean(Category.GetCheckParameter("Span_Component_Type_Code_Valid").ParameterValue) &&
            cDBConvert.ToBoolean(Category.GetCheckParameter("Span_Scale_Code_Valid").ParameterValue))
        {
          sFilterPair[] RowFilter;

          DataRowView CurrentSpan = (DataRowView)Category.GetCheckParameter("Current_Span").ParameterValue;
          DateTime EvalBeganDate = cDBConvert.ToDate(Category.GetCheckParameter("Span_Evaluation_Begin_Date").ParameterValue, DateTypes.START);
          int EvalBeganHour = cDBConvert.ToHour(Category.GetCheckParameter("Span_Evaluation_Begin_Hour").ParameterValue, DateTypes.START);
          DateTime EvalEndedDate = cDBConvert.ToDate(Category.GetCheckParameter("Span_Evaluation_End_Date").ParameterValue, DateTypes.END);
          int EvalEndedHour = cDBConvert.ToHour(Category.GetCheckParameter("Span_Evaluation_End_Hour").ParameterValue, DateTypes.END);
          DataView SpanRecords = (DataView)Category.GetCheckParameter("Span_Records").ParameterValue;

          string ComponentTypeCd = cDBConvert.ToString(CurrentSpan["Component_Type_Cd"]);
          string SpanScaleCd = cDBConvert.ToString(CurrentSpan["Span_Scale_Cd"]);
          string SpanId = cDBConvert.ToString(CurrentSpan["Span_Id"]);

          RowFilter = new sFilterPair[3];
          RowFilter[0].Set("Component_Type_Cd", ComponentTypeCd);
          RowFilter[1].Set("Span_Scale_Cd", SpanScaleCd);
          RowFilter[2].Set("Span_Id", SpanId, true);

          DataView SpanView = FindActiveRows(SpanRecords,
                                             EvalBeganDate, EvalBeganHour, EvalEndedDate, EvalEndedHour,
                                             RowFilter);

          if (SpanView.Count > 0)
          {
            DateTime CurrentDateBegan = cDBConvert.ToDate(CurrentSpan["Begin_Date"], DateTypes.START);
            int CurrentHourBegan = cDBConvert.ToHour(CurrentSpan["Begin_Hour"], DateTypes.START);

            foreach (DataRowView SpanRow in SpanView)
            {
              DateTime SpanDateBegan = cDBConvert.ToDate(SpanRow["Begin_Date"], DateTypes.START);
              int SpanHourBegan = cDBConvert.ToHour(SpanRow["Begin_Hour"], DateTypes.START);

              if ((SpanDateBegan > CurrentDateBegan) ||
                  ((SpanDateBegan == CurrentDateBegan) && (SpanHourBegan >= CurrentHourBegan)))
                Category.CheckCatalogResult = "A";
            }
          }
        }
        else
          Log = false;
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "SPAN53");
      }

      return ReturnVal;
    }

    public static string SPAN54(cCategory Category, ref bool Log)
    // Required High-Scale Span Record Reported
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentSpan = (DataRowView)Category.GetCheckParameter("Current_Span").ParameterValue;
        string SpanScaleCd = cDBConvert.ToString(CurrentSpan["Span_Scale_Cd"]);

        if (cDBConvert.ToBoolean(Category.GetCheckParameter("Span_Component_Type_Code_Valid").ParameterValue) &&
            (SpanScaleCd == "L"))
        {
          sFilterPair[] RowFilter;

          DateTime EvalBeganDate = cDBConvert.ToDate(Category.GetCheckParameter("Span_Evaluation_Begin_Date").ParameterValue, DateTypes.START);
          int EvalBeganHour = cDBConvert.ToHour(Category.GetCheckParameter("Span_Evaluation_Begin_Hour").ParameterValue, DateTypes.START);
          DateTime EvalEndedDate = cDBConvert.ToDate(Category.GetCheckParameter("Span_Evaluation_End_Date").ParameterValue, DateTypes.END);
          int EvalEndedHour = cDBConvert.ToHour(Category.GetCheckParameter("Span_Evaluation_End_Hour").ParameterValue, DateTypes.END);

          DataView SpanRecords = (DataView)Category.GetCheckParameter("Span_Records").ParameterValue;

          string ComponentTypeCd = cDBConvert.ToString(CurrentSpan["Component_Type_Cd"]);

          RowFilter = new sFilterPair[2];
          RowFilter[0].Set("Component_Type_Cd", ComponentTypeCd);
          RowFilter[1].Set("Span_Scale_Cd", "H");

          DataView SpanView = FindActiveRows(SpanRecords,
                                             EvalBeganDate, EvalBeganHour, EvalEndedDate, EvalEndedHour,
                                             RowFilter);

          if (SpanView.Count == 0)
            Category.CheckCatalogResult = "A";
          else if (!CheckForHourRangeCovered(Category, SpanView,
                                             EvalBeganDate, EvalBeganHour, EvalEndedDate, EvalEndedHour))
            Category.CheckCatalogResult = "B";
        }
        else
          Log = false;
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "SPAN54");
      }

      return ReturnVal;
    }

    public static string SPAN55(cCategory Category, ref bool Log)
    // Duplicate Span Records
    {
      string ReturnVal = "";

      try
      {
        if (cDBConvert.ToBoolean(Category.GetCheckParameter("Span_Component_Type_Code_Valid").ParameterValue))
        {
          sFilterPair[] RowFilter;

          DataRowView CurrentSpan = (DataRowView)Category.GetCheckParameter("Current_Span").ParameterValue;
          DataView SpanRecords = (DataView)Category.GetCheckParameter("Span_Records").ParameterValue;

          string SpanId = cDBConvert.ToString(CurrentSpan["Span_Id"]);
          string ComponentTypeCd = cDBConvert.ToString(CurrentSpan["Component_Type_Cd"]);

          if (ComponentTypeCd == "FLOW")
          {
            RowFilter = new sFilterPair[4];
            RowFilter[0].Set("Component_Type_Cd", ComponentTypeCd);
            RowFilter[1].Set("Begin_Date", CurrentSpan["Begin_Date"], eFilterDataType.DateBegan);
            RowFilter[2].Set("Begin_Hour", CurrentSpan["Begin_Hour"], eFilterDataType.Integer);
            RowFilter[3].Set("Span_Id", SpanId, true);
          }
          else
          {
            string SpanScaleCd = cDBConvert.ToString(CurrentSpan["Span_Scale_Cd"]);

            RowFilter = new sFilterPair[5];
            RowFilter[0].Set("Component_Type_Cd", ComponentTypeCd);
            RowFilter[1].Set("Begin_Date", CurrentSpan["Begin_Date"], eFilterDataType.DateBegan);
            RowFilter[2].Set("Begin_Hour", CurrentSpan["Begin_Hour"], eFilterDataType.Integer);
            RowFilter[3].Set("Span_Id", SpanId, true);
            RowFilter[4].Set("Span_Scale_Cd", SpanScaleCd);
          }

          DataView SpanView = FindRows(SpanRecords, RowFilter);

          if (SpanView.Count > 0)
            Category.CheckCatalogResult = "A";
          else if (CurrentSpan["End_Date"] != DBNull.Value)
          {
            RowFilter[1].Set("End_Date", CurrentSpan["End_Date"], eFilterDataType.DateEnded);
            RowFilter[2].Set("End_Hour", CurrentSpan["End_Hour"], eFilterDataType.Integer);

            SpanView = FindRows(SpanRecords, RowFilter);

            if (SpanView.Count > 0)
              Category.CheckCatalogResult = "A";
          }
        }
        else
          Log = false;
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "SPAN55");
      }

      return ReturnVal;
    }

    public static string SPAN56(cCategory Category, ref bool Log)
    // Span MPC Value Valid 
    {
      string ReturnVal = "";

      try
      {
        if (Category.GetCheckParameter("Span_Component_Type_Code_Valid").ValueAsBool())
        {
          DataRowView CurrentSpan = Category.GetCheckParameter("Current_Span").ValueAsDataRowView();

          string ComponentTypeCd = cDBConvert.ToString(CurrentSpan["Component_Type_Cd"]);
          string SpanScaleCd = cDBConvert.ToString(CurrentSpan["Span_Scale_Cd"]);

          if (CurrentSpan["Mpc_Value"] == DBNull.Value)
          {
              if( !ComponentTypeCd.InList( "FLOW,O2" ) && ( SpanScaleCd == "H" ) )
            {
              Category.SetCheckParameter("Span_MPC_Value_Valid", false, eParameterDataType.Boolean);
              Category.CheckCatalogResult = "A";
            }
          }
          else //Mpc_Value is not null
          {
              if( ComponentTypeCd.InList( "FLOW,O2" ) || ( SpanScaleCd == "L" ) )
            {
              Category.SetCheckParameter("Span_MPC_Value_Valid", false, eParameterDataType.Boolean);
              Category.CheckCatalogResult = "B";
            }
            else
            {
              decimal MpcValue = cDBConvert.ToDecimal(CurrentSpan["Mpc_Value"]);

              if (MpcValue <= 0)
              {
                Category.SetCheckParameter("Span_MPC_Value_Valid", false, eParameterDataType.Boolean);
                Category.CheckCatalogResult = "C";
              }
            }
          }
        }
        else
          Log = false;
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "SPAN56");
      }

      return ReturnVal;
    }

    public static string SPAN57(cCategory Category, ref bool Log)
    // Span MEC Value Valid 
    {
      string ReturnVal = "";

      try
      {
        if (cDBConvert.ToBoolean(Category.GetCheckParameter("Span_Component_Type_Code_Valid").ParameterValue))
        {
          DataRowView CurrentSpan = (DataRowView)Category.GetCheckParameter("Current_Span").ParameterValue;

          string ComponentTypeCd = cDBConvert.ToString(CurrentSpan["Component_Type_Cd"]);
          string SpanScaleCd = cDBConvert.ToString(CurrentSpan["Span_Scale_Cd"]);

          if (CurrentSpan["Mec_Value"] == DBNull.Value)
          {
            if ((ComponentTypeCd != "FLOW") && (ComponentTypeCd != "O2") && (SpanScaleCd == "L"))
            {
              Category.SetCheckParameter("Span_MEC_Value_Valid", false, eParameterDataType.Boolean);
              Category.CheckCatalogResult = "A";
            }
            else if( ComponentTypeCd.InList( "SO2,HG,NOX" ) && ( SpanScaleCd == "H" ) &&
                     (CurrentSpan["Default_High_Range"] != DBNull.Value))
            {
              Category.SetCheckParameter("Span_MEC_Value_Valid", false, eParameterDataType.Boolean);
              Category.CheckCatalogResult = "B";
            }
          }
          else
          {
            if (ComponentTypeCd.InList("FLOW,HG,O2"))
            {
              Category.SetCheckParameter("Span_MEC_Value_Valid", false, eParameterDataType.Boolean);
              Category.CheckCatalogResult = "C";
            }
            else if (cDBConvert.ToDecimal(CurrentSpan["Mec_Value"]) <= 0)
            {
              Category.SetCheckParameter("Span_MEC_Value_Valid", false, eParameterDataType.Boolean);
              Category.CheckCatalogResult = "D";
            }
          }
        }
        else
          Log = false;
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "SPAN57");
      }

      return ReturnVal;
    }

    public static string SPAN58(cCategory Category, ref bool Log)
    // High Span Scale Transition Point Value (Max Low Range) Valid
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentSpan = (DataRowView)Category.GetCheckParameter("Current_Span").ParameterValue;

        string SpanScaleCd = cDBConvert.ToString(CurrentSpan["Span_Scale_Cd"]);

        if (cDBConvert.ToBoolean(Category.GetCheckParameter("Span_Component_Type_Code_Valid").ParameterValue) &&
            (SpanScaleCd == "H"))
        {
          if (CurrentSpan["Max_Low_Range"] != DBNull.Value)
          {
            if ((CurrentSpan["Span_Value"] == DBNull.Value) &&
                (CurrentSpan["Default_High_Range"] != DBNull.Value))
              Category.CheckCatalogResult = "A";
          }
        }
        else
          Log = false;
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "SPAN58");
      }

      return ReturnVal;
    }

    public static string SPAN59(cCategory Category, ref bool Log)
    // Low Span Scale Transition Point Value (Max Low Range) Valid
    {
      string ReturnVal = "";

      try
      {
        DataRowView CurrentSpan = (DataRowView)Category.GetCheckParameter("Current_Span").ParameterValue;

        string SpanScaleCd = cDBConvert.ToString(CurrentSpan["Span_Scale_Cd"]);

        if (cDBConvert.ToBoolean(Category.GetCheckParameter("Span_Component_Type_Code_Valid").ParameterValue) &&
            (SpanScaleCd == "L"))
        {
          if (CurrentSpan["Max_Low_Range"] != DBNull.Value)
          {
            decimal MaxLowRange = cDBConvert.ToDecimal(CurrentSpan["Max_Low_Range"]);
            decimal FullScaleRange = cDBConvert.ToDecimal(CurrentSpan["Full_Scale_Range"]);

            if ((FullScaleRange != decimal.MinValue) &&
                !InRange(MaxLowRange, 0.5m * FullScaleRange, FullScaleRange))
              Category.CheckCatalogResult = "A";
          }
        }
        else
          Log = false;
      }
      catch (Exception ex)
      {
        ReturnVal = Category.CheckEngine.FormatError(ex, "SPAN59");
      }

      return ReturnVal;
    }

    /// <summary>
    /// Span MPC MEC Value Consistency Check
    /// </summary>
    /// <param name="category">Category Object</param>
    /// <param name="log">Indicates whether to log results.</param>
    /// <returns>Returns error message if check fails to run correctly.</returns>
    public static string SPAN60(cCategory category, ref bool log)
    {
      string returnVal = "";

      try
      {
        if (category.GetCheckParameter("Span_Component_Type_Code_Valid").ValueAsBool())
        {
          DataRowView currentSpan = category.GetCheckParameter("Current_Span").ValueAsDataRowView();

          if (currentSpan["END_DATE"].AsString() == null)
          {
            if ((currentSpan["MPC_VALUE"] != DBNull.Value) && (currentSpan["MEC_VALUE"] != DBNull.Value))
            {
              if (currentSpan["MEC_VALUE"].AsDecimal(0) >= currentSpan["MPC_VALUE"].AsDecimal(0))
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

    /// <summary>
    /// Span Scale Transition Point (Max Low Range) Check
    /// </summary>
    /// <param name="category">Category Object</param>
    /// <param name="log">Indicates whether to log results.</param>
    /// <returns>Returns error message if check fails to run correctly.</returns>
    public static string SPAN61(cCategory category, ref bool log)
    {
        string returnVal = "";

        try
        {
            if (MpParameters.SpanComponentTypeCodeValid.AsBoolean().Default(false))
            {
                if (MpParameters.CurrentSpan.ComponentTypeCd.InList("HG,HCL") && MpParameters.CurrentSpan.MaxLowRange != null)
                {
                    category.CheckCatalogResult = "A";
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

  }
}
