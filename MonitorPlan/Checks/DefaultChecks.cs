using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Extensions;


namespace ECMPS.Checks.DefaultChecks
{
	public class cDefaultChecks : cChecks
	{
		public cDefaultChecks()
		{
			CheckProcedures = new dCheckProcedure[100];
			CheckProcedures[37] = new dCheckProcedure(DEFAULT37);
			CheckProcedures[38] = new dCheckProcedure(DEFAULT38);
			CheckProcedures[39] = new dCheckProcedure(DEFAULT39);
			CheckProcedures[40] = new dCheckProcedure(DEFAULT40);
			CheckProcedures[41] = new dCheckProcedure(DEFAULT41);
			CheckProcedures[42] = new dCheckProcedure(DEFAULT42);
			CheckProcedures[47] = new dCheckProcedure(DEFAULT47);
			CheckProcedures[48] = new dCheckProcedure(DEFAULT48);
			CheckProcedures[49] = new dCheckProcedure(DEFAULT49);
			CheckProcedures[50] = new dCheckProcedure(DEFAULT50);
			CheckProcedures[51] = new dCheckProcedure(DEFAULT51);
			CheckProcedures[52] = new dCheckProcedure(DEFAULT52);
			CheckProcedures[53] = new dCheckProcedure(DEFAULT53);
			CheckProcedures[54] = new dCheckProcedure(DEFAULT54);
			CheckProcedures[56] = new dCheckProcedure(DEFAULT56);
			CheckProcedures[58] = new dCheckProcedure(DEFAULT58);
			CheckProcedures[73] = new dCheckProcedure(DEFAULT73);
			CheckProcedures[74] = new dCheckProcedure(DEFAULT74);
			CheckProcedures[75] = new dCheckProcedure(DEFAULT75);
			CheckProcedures[95] = new dCheckProcedure(DEFAULT95);
			CheckProcedures[98] = new dCheckProcedure(DEFAULT98);
			CheckProcedures[99] = new dCheckProcedure(DEFAULT99);
			//waf
			CheckProcedures[78] = new dCheckProcedure(DEFAULT78);
			CheckProcedures[79] = new dCheckProcedure(DEFAULT79);
			CheckProcedures[80] = new dCheckProcedure(DEFAULT80);
			CheckProcedures[81] = new dCheckProcedure(DEFAULT81);
			CheckProcedures[82] = new dCheckProcedure(DEFAULT82);
			CheckProcedures[83] = new dCheckProcedure(DEFAULT83);
			CheckProcedures[84] = new dCheckProcedure(DEFAULT84);
			CheckProcedures[85] = new dCheckProcedure(DEFAULT85);
			CheckProcedures[86] = new dCheckProcedure(DEFAULT86);
			CheckProcedures[87] = new dCheckProcedure(DEFAULT87);
			CheckProcedures[88] = new dCheckProcedure(DEFAULT88);
			CheckProcedures[89] = new dCheckProcedure(DEFAULT89);
			CheckProcedures[90] = new dCheckProcedure(DEFAULT90);
			CheckProcedures[91] = new dCheckProcedure(DEFAULT91);
			CheckProcedures[92] = new dCheckProcedure(DEFAULT92);
			CheckProcedures[93] = new dCheckProcedure(DEFAULT93);
			CheckProcedures[96] = new dCheckProcedure(DEFAULT96);
			CheckProcedures[97] = new dCheckProcedure(DEFAULT97);
		}

		# region default

		public static string DEFAULT37(cCategory Category, ref bool Log) //Default Active Status
		{
			string ReturnVal = "";
			try
			{
				bool DatesAndHoursConsistent = (bool)Category.GetCheckParameter("Default_Dates_and_Hours_Consistent").ParameterValue;

				if (DatesAndHoursConsistent)
					ReturnVal = Check_ActiveHourRange(Category, "Default_Active_Status",
																"Current_Default",
																"Default_Evaluation_Begin_Date",
																"Default_Evaluation_Begin_Hour",
																"Default_Evaluation_End_Date",
																"Default_Evaluation_End_Hour");
				else
					Log = false;
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "DEFAULT37");
			}
			return ReturnVal;
		}
		public static string DEFAULT38(cCategory Category, ref bool Log) //Default Dates and Hours Consistent
		{
			string ReturnVal = "";
			try
			{
				ReturnVal = Check_ConsistentHourRange(Category, "Default_Dates_And_Hours_Consistent",
																"Current_Default",
																"Default_Start_Date_Valid",
																"Default_Start_Hour_Valid",
																"Default_End_Date_Valid",
																"Default_End_Hour_Valid");
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "DEFAULT38");
			}
			return ReturnVal;
		}
		public static string DEFAULT39(cCategory Category, ref bool Log) //Default Begin Date Valid
		{
			string ReturnVal = "";
			try
			{
				ReturnVal = Check_ValidStartDate(Category, "Default_Start_Date_Valid", "Current_Default");
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "DEFAULT39");
			}
			return ReturnVal;
		}
		public static string DEFAULT40(cCategory Category, ref bool Log) //Default Begin Hour Valid
		{
			string ReturnVal = "";
			try
			{
				ReturnVal = Check_ValidStartHour(Category, "Default_Start_Hour_Valid", "Current_Default");
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "DEFAULT40");
			}
			return ReturnVal;
		}
		public static string DEFAULT41(cCategory Category, ref bool Log) //Default End Date Valid
		{
			string ReturnVal = "";
			try
			{
				ReturnVal = Check_ValidEndDate(Category, "Default_End_Date_Valid", "Current_Default");
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "DEFAULT41");
			}
			return ReturnVal;
		}
		public static string DEFAULT42(cCategory Category, ref bool Log) //Default End Hour Valid
		{
			string ReturnVal = "";
			try
			{
				ReturnVal = Check_ValidEndHour(Category, "Default_End_Hour_Valid", "Current_Default");
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "DEFAULT42");
			}
			return ReturnVal;
		}
		public static string DEFAULT47(cCategory Category, ref bool Log) //Default Parameter Code Valid
		{
			string ReturnVal = "";
			try
			{
				DataRowView CurrentDefault = (DataRowView)Category.GetCheckParameter("Current_Default").ParameterValue;
				DataView DefaultParameterList = (DataView)Category.GetCheckParameter("Default_Parameter_List").ParameterValue;
				bool DefaultParameterValid = true;

				if (CurrentDefault["parameter_cd"] == DBNull.Value)
				{
					Category.CheckCatalogResult = "A";
					DefaultParameterValid = false;
				}
				else
				{
					string ParameterCode = cDBConvert.ToString(CurrentDefault["parameter_cd"]);
					string OldFilter = DefaultParameterList.RowFilter;
					DefaultParameterList.RowFilter = AddToDataViewFilter(OldFilter,
						"ParameterCode = '" + ParameterCode + "' and CategoryCode = 'DEFAULT'");
					if (DefaultParameterList.Count == 0)
					{
						Category.CheckCatalogResult = "B";
						DefaultParameterValid = false;
					}
					else
					{
						string LocationType = (string)Category.GetCheckParameter("Location_Type").ParameterValue;
						if (LocationType == "CP")
						{
							if (ParameterCode.InList("FLOX,MNHI,MNNX") || ParameterCode.StartsWith("H2O") ||
								ParameterCode.StartsWith("SO") || ParameterCode.StartsWith("CO") ||
								ParameterCode.StartsWith("O2") || ParameterCode.StartsWith("NO"))
							{
								Category.CheckCatalogResult = "C";
								DefaultParameterValid = false;
							}
						}
						else if (LocationType == "MP")
						{
							if (ParameterCode.InList("NOXR,FLOX,MNHI,MNNX") || ParameterCode.StartsWith("H2O") ||
								ParameterCode.StartsWith("SO") || ParameterCode.StartsWith("CO") || ParameterCode.StartsWith("O2"))
							{
								Category.CheckCatalogResult = "C";
								DefaultParameterValid = false;
							}
						}
						else if (LocationType == "CS" || LocationType == "MS")
						{
							if (ParameterCode.InList("CO2R,NOXR,MNGF,MNOF"))
							{
								Category.CheckCatalogResult = "C";
								DefaultParameterValid = false;
							}
						}
					}
					DefaultParameterList.RowFilter = OldFilter;
				}
				Category.SetCheckParameter("Default_Parameter_Code_Valid", DefaultParameterValid, eParameterDataType.Boolean);
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "DEFAULT47");
			}
			return ReturnVal;
		}
		public static string DEFAULT48(cCategory Category, ref bool Log) //Default Operating Condition Code Valid
		{
			string ReturnVal = "";
			try
			{
				DataRowView DefaultRecord = (DataRowView)Category.GetCheckParameter("Current_Default").ParameterValue;
				bool ParamCodeValid = (bool)Category.GetCheckParameter("Default_Parameter_Code_Valid").ParameterValue;
				if (ParamCodeValid)
				{
					if (DefaultRecord["operating_condition_cd"] == DBNull.Value)
						Category.CheckCatalogResult = "A";
					else
					{
						string OpConditionCd = cDBConvert.ToString(DefaultRecord["operating_condition_cd"]);
						if (!OpConditionCd.InList("A,C,U,B,P"))
							Category.CheckCatalogResult = "B";
						else
						{
							string ParamCd = cDBConvert.ToString(DefaultRecord["parameter_cd"]);
							string DfltSrcCd = cDBConvert.ToString(DefaultRecord["default_source_cd"]);
							if (OpConditionCd.InList("B,P") && ParamCd != "NOXR")
								Category.CheckCatalogResult = "C";
							else if (OpConditionCd == "C" && !ParamCd.InList("SO2X,SORX,NORX,NOCX,NOXR"))
								Category.CheckCatalogResult = "C";
							else if (OpConditionCd.InList("B,P,C") && ParamCd == "NOXR" && DfltSrcCd == "DEF")
								Category.CheckCatalogResult = "D";
							else if (OpConditionCd == "U" && ParamCd == "NOXR")
								Category.CheckCatalogResult = "E";
						}
					}
				}
				else
					Log = false;
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "DEFAULT48");
			}
			return ReturnVal;
		}
		public static string DEFAULT49(cCategory Category, ref bool Log) //Default Value Valid
		{
			string ReturnVal = "";
			try
			{
				DataRowView DefaultRecord = (DataRowView)Category.GetCheckParameter("Current_Default").ParameterValue;
				bool ParamCodeValid = (bool)Category.GetCheckParameter("Default_Parameter_Code_Valid").ParameterValue;
				if (ParamCodeValid)
				{
					decimal DftlVal = cDBConvert.ToDecimal(DefaultRecord["default_value"]);
					string ParamCd = cDBConvert.ToString(DefaultRecord["parameter_cd"]);
					string DfltUOM = cDBConvert.ToString(DefaultRecord["default_uom_cd"]);

					Category.SetCheckParameter("Default_Value_Valid", true, eParameterDataType.Boolean);
					if (DefaultRecord["default_value"] == DBNull.Value)
					{
						Category.CheckCatalogResult = "A";
						Category.SetCheckParameter("Default_Value_Valid", false, eParameterDataType.Boolean);
					}
					else
					{
						if (DftlVal <= 0)
						{
							Category.CheckCatalogResult = "B";
							Category.SetCheckParameter("Default_Value_Valid", false, eParameterDataType.Boolean);
						}
						else
						{
							DataView ParamUOMTbl = Category.GetCheckParameter("Parameter_Units_Of_Measure_Lookup_Table").ValueAsDataView();
							string LookupFilter = ParamUOMTbl.RowFilter;
							ParamUOMTbl.RowFilter = AddToDataViewFilter(LookupFilter, "parameter_cd = '" + ParamCd + "' and uom_cd = '" + DfltUOM + "'");
							if (ParamUOMTbl.Count > 0)
							{
								int dp = cDBConvert.ToInteger(ParamUOMTbl[0]["decimals_hrly"]);
								decimal DefaultValue = DftlVal.RoundTo(dp);//Math.Round(DftlVal,dp,MidpointRounding.AwayFromZero);                                
								if (DefaultValue != DftlVal)
									Category.CheckCatalogResult = "G";
							}
							if (ParamUOMTbl.Count == 0 || string.IsNullOrEmpty(Category.CheckCatalogResult))
							{

								DateTime EvalBgnDt = (DateTime)Category.GetCheckParameter("Default_Evaluation_Begin_Date").ParameterValue;
								DateTime EvalEndDt = (DateTime)Category.GetCheckParameter("Default_Evaluation_End_Date").ParameterValue;
								int EvalBeginHour = Category.GetCheckParameter("Default_Evaluation_Begin_Hour").ValueAsInt();
								int EvalEndHour = Category.GetCheckParameter("Default_Evaluation_End_Hour").ValueAsInt();

								if (cDBConvert.ToString(DefaultRecord["default_purpose_cd"]) == "DC")
								{
									DataView UnitTypeRecs = (DataView)Category.GetCheckParameter("Location_Unit_Type_Records").ParameterValue;
									string UnitTypeOldFilter = UnitTypeRecs.RowFilter;
									UnitTypeRecs.RowFilter = AddEvaluationDateRangeToDataViewFilter(UnitTypeOldFilter, EvalBgnDt, EvalEndDt, false, true, true);
									if (UnitTypeRecs.Count > 0)
									{
										string BoilerType = "";
										UnitTypeRecs.RowFilter = AddToDataViewFilter(UnitTypeRecs.RowFilter,
																					"unit_type_cd = 'CC' or unit_type_cd = 'CT' or unit_type_cd = 'ICE' or unit_type_cd = 'OT' or unit_type_cd = 'IGC'");
										if (UnitTypeRecs.Count > 0)
											BoilerType = "TURBINE";
										else
											BoilerType = "BOILER";
										if (ParamCd == "CO2N" && BoilerType == "TURBINE" && DftlVal != 1)
										{
											Category.CheckCatalogResult = "C";
											Category.SetCheckParameter("Default_Value_Valid", false, eParameterDataType.Boolean);
										}
										else if (ParamCd == "CO2N" && BoilerType == "BOILER" && DftlVal != 5)
										{
											Category.CheckCatalogResult = "C";
											Category.SetCheckParameter("Default_Value_Valid", false, eParameterDataType.Boolean);
										}
										else if (ParamCd == "O2X" && BoilerType == "TURBINE" && DftlVal != 19)
										{
											Category.CheckCatalogResult = "C";
											Category.SetCheckParameter("Default_Value_Valid", false, eParameterDataType.Boolean);
										}
										else if (ParamCd == "O2X" && BoilerType == "BOILER" && DftlVal != 14)
										{
											Category.CheckCatalogResult = "C";
											Category.SetCheckParameter("Default_Value_Valid", false, eParameterDataType.Boolean);
										}
									}
									UnitTypeRecs.RowFilter = UnitTypeOldFilter;
								}
								else
								{
									string DfltSrcCd = cDBConvert.ToString(DefaultRecord["default_source_cd"]);
									if (ParamCd == "H2O" && DfltSrcCd == "DEF")
									{
										DataView FuelCodeXCheck =
											(DataView)Category.GetCheckParameter("Fuel_Code_To_Minimum_And_Maximum_Moisture_Default_Cross_Check_Table").ParameterValue;
										string XCheckOldFilter = FuelCodeXCheck.RowFilter;
										FuelCodeXCheck.RowFilter = AddToDataViewFilter(XCheckOldFilter, "FuelCode = '" +
																						cDBConvert.ToString(DefaultRecord["fuel_cd"]) + "'");
										if (FuelCodeXCheck.Count > 0)
										{
											if (DftlVal < cDBConvert.ToDecimal(FuelCodeXCheck[0]["MinimumValue"]) ||
												DftlVal > cDBConvert.ToDecimal(FuelCodeXCheck[0]["MaximumValue"]))
											{
												Category.CheckCatalogResult = "D";
												Category.SetCheckParameter("Default_Value_Valid", false, eParameterDataType.Boolean);
											}
										}
										FuelCodeXCheck.RowFilter = XCheckOldFilter;
									}
									else if (ParamCd == "H2ON" && DfltSrcCd == "DEF")
									{
										if (DftlVal != 3.0m)
											Category.CheckCatalogResult = "F";

									}
									else if (ParamCd == "H2OX" && DfltSrcCd == "DEF")
									{
										if (DftlVal != 15.0m)
											Category.CheckCatalogResult = "F";
									}
									else
									{
										decimal MaxDfltVal = cDBConvert.ToDecimal(Category.GetCheckParameter("Maximum_Default_Value").ParameterValue);
										decimal MinDfltVal = cDBConvert.ToDecimal(Category.GetCheckParameter("Minimum_Default_Value").ParameterValue);

										if (MinDfltVal != decimal.MinValue && MaxDfltVal != decimal.MinValue)
										{
											if ((cDBConvert.ToString(DefaultRecord["parameter_cd"]) == "NOCX" || cDBConvert.ToString(DefaultRecord["parameter_cd"]) == "NORX") &&
												cDBConvert.ToString(DefaultRecord["default_purpose_cd"]) == "MD")
											{
												DataView MethodRecords = (DataView)Category.GetCheckParameter("Method_Records").ParameterValue;
												string MethodFilter = MethodRecords.RowFilter;
												MethodRecords.RowFilter = AddToDataViewFilter(MethodFilter, "parameter_cd like 'NOX%' and method_cd = 'CEM'");
												MethodRecords.RowFilter = AddEvaluationDateHourRangeToDataViewFilter(MethodRecords.RowFilter, EvalBgnDt, EvalEndDt, EvalBeginHour, EvalEndHour, false, true, false);
												if (MethodRecords.Count > 0)
												{
													if (cDBConvert.ToString(DefaultRecord["parameter_cd"]) == "NORX" &&
														 cDBConvert.ToString(DefaultRecord["operating_condition_cd"]) == "C")
													{
														if (DftlVal < 0m || DftlVal > MaxDfltVal)
														{
															Category.CheckCatalogResult = "E";
															Category.SetCheckParameter("Default_Value_Valid", false, eParameterDataType.Boolean);
														}
													}
													else
													{
														if (DftlVal < MinDfltVal || DftlVal > MaxDfltVal)
														{
															Category.CheckCatalogResult = "E";
															Category.SetCheckParameter("Default_Value_Valid", false, eParameterDataType.Boolean);
														}
													}
												}
												MethodRecords.RowFilter = MethodFilter;
											}
											else if (cDBConvert.ToString(DefaultRecord["default_purpose_cd"]) != "LM" || cDBConvert.ToString(DefaultRecord["default_source_cd"]) != "DEF")
											{
												if (DftlVal < MinDfltVal || DftlVal > MaxDfltVal)
												{
													Category.CheckCatalogResult = "E";
													Category.SetCheckParameter("Default_Value_Valid", false, eParameterDataType.Boolean);
												}
											}
										}

									}
								}
							}
							ParamUOMTbl.RowFilter = LookupFilter;
						}


					}
				}
				else
					Log = false;
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "DEFAULT49");
			}
			return ReturnVal;
		}
		public static string DEFAULT50(cCategory Category, ref bool Log) //Default Units of Measure Code Valid
		{
			string ReturnVal = "";
			try
			{
				bool ParamCdValid = (bool)Category.GetCheckParameter("Default_Parameter_Code_Valid").ParameterValue;
				if (ParamCdValid)
				{
					//DataRowView DefaultRecord = (DataRowView)Category.GetCheckParameter("Current_Default").ParameterValue;
					//string UOMCd = cDBConvert.ToString(DefaultRecord["default_uom_cd"]);
					DataRowView DefaultRecord = (DataRowView)Category.GetCheckParameter("Current_Default").ParameterValue;
					string UOMCd = cDBConvert.ToString(DefaultRecord["default_uom_cd"]);
					string ParamCd = cDBConvert.ToString(DefaultRecord["parameter_cd"]);

					if (DefaultRecord["default_uom_cd"] == DBNull.Value)
					{
						Category.CheckCatalogResult = "A";
					}
					else
					{
						DataView ParamUOMLookup = (DataView)Category.GetCheckParameter("Parameter_Units_Of_Measure_Lookup_Table").ParameterValue;
						string OldFilter = ParamUOMLookup.RowFilter;
						ParamUOMLookup.RowFilter = AddToDataViewFilter(OldFilter, "parameter_cd = '" + ParamCd + "' and uom_cd = '" + UOMCd + "'");
						if (ParamUOMLookup.Count > 0)
						{
							Category.SetCheckParameter("Maximum_Default_Value", cDBConvert.ToDecimal(ParamUOMLookup[0]["max_value"]), eParameterDataType.Decimal);
							if (ParamCd == "SO2R")
								Category.SetCheckParameter("Minimum_Default_Value", 0.0001m, eParameterDataType.Decimal);
							else
								Category.SetCheckParameter("Minimum_Default_Value", cDBConvert.ToDecimal(ParamUOMLookup[0]["min_value"]), eParameterDataType.Decimal);
						}
						else
						{
							DataView UOMLookup = (DataView)Category.GetCheckParameter("Units_of_Measure_Code_Lookup_Table").ParameterValue;
							string UOMOldFilter = UOMLookup.RowFilter;
							UOMLookup.RowFilter = AddToDataViewFilter(UOMOldFilter, "uom_cd = '" + UOMCd + "'");
							if (UOMLookup.Count == 0)
								Category.CheckCatalogResult = "B";
							else
								Category.CheckCatalogResult = "C";
							UOMLookup.RowFilter = UOMOldFilter;
						}
						ParamUOMLookup.RowFilter = OldFilter;
					}
				}
				else
					Log = false;
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "DEFAULT50");
				System.Diagnostics.Debug.WriteLine("inner exception " + ex.InnerException);
			}
			return ReturnVal;
		}
		public static string DEFAULT51(cCategory Category, ref bool Log) //Default Purpose Code Valid
		{
			string ReturnVal = "";
			try
			{
				bool ParamCdValid = (bool)Category.GetCheckParameter("Default_Parameter_Code_Valid").ParameterValue;
				if (ParamCdValid)
				{
					DataRowView DefaultRecord = (DataRowView)Category.GetCheckParameter("Current_Default").ParameterValue;
					string DfltPurposeCd = cDBConvert.ToString(DefaultRecord["default_purpose_cd"]);
					if (DefaultRecord["default_purpose_cd"] == DBNull.Value)
					{
						Category.CheckCatalogResult = "A";
						Category.SetCheckParameter("Default_Purpose_Code_Valid", false, eParameterDataType.Boolean);
					}
					else
					{
						string ParamCd = cDBConvert.ToString(DefaultRecord["parameter_cd"]);
						DataView ParamToPurposeXCheck = (DataView)Category.GetCheckParameter("Default_Parameter_To_Purpose_Cross_Check_Table").ParameterValue;
						string OldFilter = ParamToPurposeXCheck.RowFilter;
						ParamToPurposeXCheck.RowFilter = AddToDataViewFilter(OldFilter, "ParameterCode = '" + ParamCd + "' and DefaultPurposeCode = '" + DfltPurposeCd + "'");
						if (ParamToPurposeXCheck.Count > 0)
							Category.SetCheckParameter("Default_Purpose_Code_Valid", true, eParameterDataType.Boolean);
						else
						{
							Category.SetCheckParameter("Default_Purpose_Code_Valid", false, eParameterDataType.Boolean);

							DataView DfltPurposeLookup = (DataView)Category.GetCheckParameter("Default_Purpose_Code_Lookup_Table").ParameterValue;
							string DfltPurposeOldFilter = DfltPurposeLookup.RowFilter;
							DfltPurposeLookup.RowFilter = AddToDataViewFilter(DfltPurposeOldFilter, "default_purpose_cd = '" + DfltPurposeCd + "'");
							if (DfltPurposeLookup.Count == 0)
								Category.CheckCatalogResult = "B";
							else
								Category.CheckCatalogResult = "C";
							DfltPurposeLookup.RowFilter = DfltPurposeOldFilter;
						}
						ParamToPurposeXCheck.RowFilter = OldFilter;
					}
				}
				else
					Log = false;
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "DEFAULT51");
			}
			return ReturnVal;
		}
		public static string DEFAULT52(cCategory Category, ref bool Log) //Default Source Code Valid
		{
			string ReturnVal = "";
			try
			{
				bool ParamCdValid = (bool)Category.GetCheckParameter("Default_Parameter_Code_Valid").ParameterValue;
				if (ParamCdValid)
				{
					DataRowView DefaultRecord = (DataRowView)Category.GetCheckParameter("Current_Default").ParameterValue;
					string DfltSourceCd = cDBConvert.ToString(DefaultRecord["default_source_cd"]);
					if (DefaultRecord["default_source_cd"] == DBNull.Value)
					{
						Category.CheckCatalogResult = "A";
						Category.SetCheckParameter("Default_Source_Code_Valid", false, eParameterDataType.Boolean);
					}
					else
					{
						string ParamCd = cDBConvert.ToString(DefaultRecord["parameter_cd"]);
						DataView ParamToSrcOfValXCheck = (DataView)Category.GetCheckParameter("Default_Parameter_To_Source_Of_Value_Cross_Check_Table").ParameterValue;
						string OldFilter = ParamToSrcOfValXCheck.RowFilter;
						ParamToSrcOfValXCheck.RowFilter = AddToDataViewFilter(OldFilter, "ParameterCode = '" + ParamCd + "' and DefaultSourceCode = '" + DfltSourceCd + "'");
						if (ParamToSrcOfValXCheck.Count > 0)
							Category.SetCheckParameter("Default_Source_Code_Valid", true, eParameterDataType.Boolean);
						else
						{
							Category.SetCheckParameter("Default_Source_Code_Valid", false, eParameterDataType.Boolean);

							DataView DfltSourceLookup = (DataView)Category.GetCheckParameter("Default_Source_Code_Lookup_Table").ParameterValue;
							string DfltSrcOldFilter = DfltSourceLookup.RowFilter;
							DfltSourceLookup.RowFilter = AddToDataViewFilter(DfltSrcOldFilter, "default_source_cd = '" + DfltSourceCd + "'");
							if (DfltSourceLookup.Count == 0)
								Category.CheckCatalogResult = "B";
							else
								Category.CheckCatalogResult = "C";
							DfltSourceLookup.RowFilter = DfltSrcOldFilter;
						}
						ParamToSrcOfValXCheck.RowFilter = OldFilter;
					}
				}
				else
					Log = false;
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "DEFAULT52");
			}
			return ReturnVal;
		}
		public static string DEFAULT53(cCategory Category, ref bool Log) //Default Fuel Code Valid
		{
			string ReturnVal = "";
			try
			{
				DataRowView DefaultRecord = (DataRowView)Category.GetCheckParameter("Current_Default").ParameterValue;
				bool validParam = (bool)Category.GetCheckParameter("Default_Parameter_Code_Valid").ParameterValue;
				if (validParam)
				{
					Category.SetCheckParameter("Default_Fuel_Code_Valid", true, eParameterDataType.Boolean);
					string FuelCd = cDBConvert.ToString(DefaultRecord["fuel_cd"]);
					if (DefaultRecord["fuel_cd"] == DBNull.Value)
					{
						Category.SetCheckParameter("Default_Fuel_Code_Valid", false, eParameterDataType.Boolean);
						Category.CheckCatalogResult = "A";
					}
					else
					{
						DataView FuelCdLookupTbl = (DataView)Category.GetCheckParameter("Fuel_Code_Lookup_Table").ParameterValue;
						string FuelCdOldFilter = FuelCdLookupTbl.RowFilter;
						FuelCdLookupTbl.RowFilter = AddToDataViewFilter(FuelCdOldFilter, "fuel_cd = '" + FuelCd + "'");
						if (FuelCdLookupTbl.Count == 0)
						{
							Category.SetCheckParameter("Default_Fuel_Code_Valid", false, eParameterDataType.Boolean);
							Category.CheckCatalogResult = "B";
						}
						else
						{
							string ParamCd = cDBConvert.ToString(DefaultRecord["parameter_cd"]);
							string DfltSrcCd = cDBConvert.ToString(DefaultRecord["default_source_cd"]);
							DataRowView FuelCdRec = FuelCdLookupTbl[0];
							string FuelGrp = cDBConvert.ToString(FuelCdRec["fuel_group_cd"]);
							string DfltPrpsCd = cDBConvert.ToString(DefaultRecord["default_purpose_cd"]);
							Category.SetCheckParameter("Default_Unit_Fuel", cDBConvert.ToString(FuelCdRec["unit_fuel_cd"]), eParameterDataType.String);

							if (ParamCd.InList("SO2R,CO2R,NOXR") && DfltPrpsCd != "F23")
							{

								if (!(FuelGrp == "GAS" || FuelGrp == "OIL" || FuelGrp == "MIX"))
								{
									Category.SetCheckParameter("Default_Fuel_Code_Valid", false, eParameterDataType.Boolean);
									Category.CheckCatalogResult = "C";
								}
							}
							else if (ParamCd == "SO2R" && DfltPrpsCd == "F23" && DfltSrcCd != "APP")
							{
								if (!(FuelCd == "NNG" || FuelCd == "PNG" || FuelCd == "OGS"))
								{
									Category.SetCheckParameter("Default_Fuel_Code_Valid", false, eParameterDataType.Boolean);
									Category.CheckCatalogResult = "C";
								}
							}
							else if (ParamCd.StartsWith("O2") || ParamCd.InList("CO2N,CO2X,H2ON,H2OX"))
							{
								if (FuelCd != "NFS")
								{
									Category.SetCheckParameter("Default_Fuel_Code_Valid", false, eParameterDataType.Boolean);
									Category.CheckCatalogResult = "C";
								}
							}
							else if (ParamCd == "H2O" && DfltSrcCd != "APP")
							{
								if (!(FuelCd.InList("NNG,PNG,CRF,W") || FuelGrp == "COAL"))
								{
									Category.SetCheckParameter("Default_Fuel_Code_Valid", false, eParameterDataType.Boolean);
									Category.CheckCatalogResult = "C";
								}
							}
							else if (ParamCd == "MNGF")
							{
								if (!(FuelGrp == "GAS" || FuelGrp == "MIX"))
								{
									Category.SetCheckParameter("Default_Fuel_Code_Valid", false, eParameterDataType.Boolean);
									Category.CheckCatalogResult = "C";
								}
							}
							else if (ParamCd == "MNOF")
							{
								if (!(FuelGrp == "OIL" || FuelGrp == "MIX"))
								{
									Category.SetCheckParameter("Default_Fuel_Code_Valid", false, eParameterDataType.Boolean);
									Category.CheckCatalogResult = "C";
								}
							}
						}
						FuelCdLookupTbl.RowFilter = FuelCdOldFilter;
					}
				}
				else
					Log = false;

			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "DEFAULT53");
			}
			return ReturnVal;
		}
		public static string DEFAULT54(cCategory Category, ref bool Log) //Generic LME Default Emission Rate Valid
		{
			string ReturnVal = "";
			try
			{
				bool validParam = (bool)Category.GetCheckParameter("Default_Parameter_Code_Valid").ParameterValue;
				bool validPurpose = Category.GetCheckParameter("Default_Purpose_Code_Valid").ValueAsBool();
				bool validFuel = Category.GetCheckParameter("Default_Fuel_Code_Valid").ValueAsBool();

				DataRowView DefaultRecord = (DataRowView)Category.GetCheckParameter("Current_Default").ParameterValue;

				string DfltPrpsCd = cDBConvert.ToString(DefaultRecord["default_purpose_cd"]);
				string DfltSrcCd = cDBConvert.ToString(DefaultRecord["default_source_cd"]);
				string ParamCd = cDBConvert.ToString(DefaultRecord["parameter_cd"]);
				string FuelCd = cDBConvert.ToString(DefaultRecord["fuel_cd"]);

				if (validParam && validPurpose && DfltSrcCd == "DEF" && validFuel && FuelCd != "NFS")
				{
					DataView ParamBoilerFuelXCheck =
						(DataView)Category.GetCheckParameter("Default_Parameter,_Boiler_Type,_And_Fuel_Type_To_Default_Value_Cross_Check_Table").ParameterValue;
					string OldFilter = ParamBoilerFuelXCheck.RowFilter;
					decimal DefaultVal = cDBConvert.ToDecimal(DefaultRecord["default_value"]);

					if ((ParamCd == "NOXR" && DfltPrpsCd == "LM") || (ParamCd == "NORX" && DfltPrpsCd == "MD"))
					{
						bool NOXLME = true;

						if (DfltPrpsCd == "MD")
						{
							//Locate another Monitor Default record for the location where the ParameterCode is equal to "NOXR", the DefaultPurposeCode is equal to "LM", the BeginDate is null or is on or before the Default Evaluation End Date, and the EndDate is null or is on or after the Default Evaluation Start Date
							DateTime DfltEvalBgnDt = (DateTime)Category.GetCheckParameter("Default_Evaluation_Begin_Date").ParameterValue;
							DateTime DfltEvalEndDt = (DateTime)Category.GetCheckParameter("Default_Evaluation_End_Date").ParameterValue;
							int EvalBeginHour = Category.GetCheckParameter("Default_Evaluation_Begin_Hour").ValueAsInt();
							int EvalEndHour = Category.GetCheckParameter("Default_Evaluation_End_Hour").ValueAsInt();
							DataView DefaultRecords = (DataView)Category.GetCheckParameter("Default_Records").ParameterValue;
							string DefaultFilter = DefaultRecords.RowFilter;
							string tempFilter = DefaultFilter;
							tempFilter = AddToDataViewFilter(tempFilter, "parameter_cd = 'NOXR' and default_purpose_cd = 'LM'");
							tempFilter = AddEvaluationDateHourRangeToDataViewFilter(tempFilter, DfltEvalBgnDt, DfltEvalEndDt, EvalBeginHour, EvalBeginHour, false, true);
							DefaultRecords.RowFilter = tempFilter;
							if (DefaultRecords.Count == 0)
								NOXLME = false;
							DefaultRecords.RowFilter = DefaultFilter;
							if (NOXLME == true)
							{
								string BoilerType = "";
								DataView LcnUnitTypeRecs = (DataView)Category.GetCheckParameter("Location_Unit_Type_Records").ParameterValue;
								string UnitOldFilter = LcnUnitTypeRecs.RowFilter;
								LcnUnitTypeRecs.RowFilter = AddEvaluationDateRangeToDataViewFilter(UnitOldFilter, DfltEvalBgnDt, DfltEvalEndDt,
																									false, true, false);
								if (LcnUnitTypeRecs.Count > 0)
								{
									int bgnCnt = LcnUnitTypeRecs.Count;
									tempFilter = LcnUnitTypeRecs.RowFilter;
									LcnUnitTypeRecs.RowFilter = AddToDataViewFilter(tempFilter, "unit_type_cd = 'CC' or unit_type_cd = 'CT' " +
																					"or unit_type_cd = 'ICE' or unit_type_cd = 'OT' or unit_type_cd = 'IGC'");
									if (LcnUnitTypeRecs.Count == bgnCnt)
										BoilerType = "TURBINE";
									else
									{
										LcnUnitTypeRecs.RowFilter = tempFilter;
										LcnUnitTypeRecs.RowFilter = AddToDataViewFilter(tempFilter, "unit_type_cd in ('CFB','DB','OB','T','PFB','BFB')");
										if (LcnUnitTypeRecs.Count == bgnCnt)
											BoilerType = "BOILER";
									}
									LcnUnitTypeRecs.RowFilter = tempFilter;
								}
								LcnUnitTypeRecs.RowFilter = UnitOldFilter;
								if (BoilerType == "")
									Category.CheckCatalogResult = "A";
								else
								{
									ParamBoilerFuelXCheck.RowFilter = AddToDataViewFilter(OldFilter,
																							"ParameterCodeAndBoilerType = 'NOXR-" + BoilerType +
																							"' and FuelCode = '" + FuelCd + "'");
									if (ParamBoilerFuelXCheck.Count == 0)
										Category.CheckCatalogResult = "A";
									else
									{
										decimal xCheckDfltVal = cDBConvert.ToDecimal(ParamBoilerFuelXCheck[0]["DefaultValue"]);
										if (DefaultVal > 0 && DefaultVal != xCheckDfltVal)
											Category.CheckCatalogResult = "B";
									}
								}
							}
						}
					}
					else
					{
						if (DfltPrpsCd == "LM")
						{
							ParamBoilerFuelXCheck.RowFilter = AddToDataViewFilter(OldFilter,
																						"ParameterCodeAndBoilerType = '" + ParamCd +
																						"' and FuelCode = '" + FuelCd + "'");
							if (ParamBoilerFuelXCheck.Count == 0)
								Category.CheckCatalogResult = "A";
							else
							{
								decimal xCheckDfltVal = cDBConvert.ToDecimal(ParamBoilerFuelXCheck[0]["DefaultValue"]);
								if (DefaultVal > 0 && DefaultVal != xCheckDfltVal)
									Category.CheckCatalogResult = "B";
							}
						}

					}
					ParamBoilerFuelXCheck.RowFilter = OldFilter;

				}
				else
					Log = false;
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "DEFAULT54");
			}
			return ReturnVal;
		}
		public static string DEFAULT56(cCategory Category, ref bool Log) //NOXR LME Default Consistent with Controls and Unit Type
		{
			string ReturnVal = "";
			try
			{
				bool validParam = (bool)Category.GetCheckParameter("Default_Parameter_Code_Valid").ParameterValue;
				bool consistent = (bool)Category.GetCheckParameter("Default_Dates_and_Hours_Consistent").ParameterValue;
				DataRowView DefaultRecord = (DataRowView)Category.GetCheckParameter("Current_Default").ParameterValue;

				string ParamCd = cDBConvert.ToString(DefaultRecord["parameter_cd"]);
				string DfltSrcCd = cDBConvert.ToString(DefaultRecord["default_source_cd"]);
				string DfltPrpsCd = cDBConvert.ToString(DefaultRecord["default_purpose_cd"]);

				if (validParam && ParamCd == "NOXR" && DfltPrpsCd == "LM" && DfltSrcCd != "DEF" && consistent)
				{
					string OpConditionCd = cDBConvert.ToString(DefaultRecord["operating_condition_cd"]);
					DateTime EvalBeginDate = Category.GetCheckParameter("Default_Evaluation_Begin_Date").ValueAsDateTime(DateTypes.START);
					DateTime EvalEndDate = Category.GetCheckParameter("Default_Evaluation_End_Date").ValueAsDateTime(DateTypes.END);

					if (OpConditionCd == "A")
					{
						DataView UnitControlRecords = Category.GetCheckParameter("Location_Control_Records").ValueAsDataView();
						string CtrlFilter = UnitControlRecords.RowFilter;
						UnitControlRecords.RowFilter = AddToDataViewFilter(CtrlFilter, "ce_param = 'NOX' and control_cd in ('DLNB','H2O','NH3','SCR','SNCR','STM') ");
						UnitControlRecords.RowFilter = AddEvaluationDateRangeToDataViewFilter(UnitControlRecords.RowFilter, "install_date", "retire_date", EvalBeginDate, EvalEndDate, false, false, true);
						if (UnitControlRecords.Count > 0)
							Category.CheckCatalogResult = "A";
						UnitControlRecords.RowFilter = CtrlFilter;
					}
					else if (OpConditionCd == "B" || OpConditionCd == "P")
					{
						DataView UnitTypeRecords = Category.GetCheckParameter("Location_Unit_Type_Records").ValueAsDataView();
						string TypeFilter = UnitTypeRecords.RowFilter;
						UnitTypeRecords.RowFilter = AddToDataViewFilter(TypeFilter, "unit_type_cd not in ('CC','CT','ICE','IGC','OT')");
						UnitTypeRecords.RowFilter = AddEvaluationDateRangeToDataViewFilter(UnitTypeRecords.RowFilter, EvalBeginDate, EvalEndDate, false, false, true);
						if (UnitTypeRecords.Count > 0)
							Category.CheckCatalogResult = "B";
						UnitTypeRecords.RowFilter = TypeFilter;
					}
					else if (OpConditionCd == "C")
					{
						int RangeCount = 0;
						DataView UnitControlRecords = Category.GetCheckParameter("Location_Control_Records").ValueAsDataView();
						string CtrlFilter = UnitControlRecords.RowFilter;
						UnitControlRecords.RowFilter = AddToDataViewFilter(CtrlFilter, "ce_param = 'NOX'");
						UnitControlRecords.RowFilter = AddEvaluationDateRangeToDataViewFilter(UnitControlRecords.RowFilter, "install_date", "retire_date", EvalBeginDate, EvalEndDate, false, false, true);
						if (UnitControlRecords.Count == 0)
							Category.CheckCatalogResult = "C";
						else if (!CheckForDateRangeCovered(Category, UnitControlRecords, "install_date", "retire_date", EvalBeginDate, EvalEndDate, true, ref RangeCount))
							Category.CheckCatalogResult = "D";
						UnitControlRecords.RowFilter = CtrlFilter;
					}
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "DEFAULT56");
			}
			return ReturnVal;
		}
		public static string DEFAULT58(cCategory Category, ref bool Log) //Default Fuel Type Consistent with Unit Fuel
		{
			string ReturnVal = "";
			try
			{
				bool validParam = (bool)Category.GetCheckParameter("Default_Parameter_Code_Valid").ParameterValue;
				bool consistent = (bool)Category.GetCheckParameter("Default_Dates_and_Hours_Consistent").ParameterValue;

				if (validParam && consistent)
				{
					bool validFuel = Category.GetCheckParameter("Default_Fuel_Code_Valid").ValueAsBool();
					DataRowView DefaultRecord = (DataRowView)Category.GetCheckParameter("Current_Default").ParameterValue;
					string FuelCd = cDBConvert.ToString(DefaultRecord["fuel_cd"]);
					if ((!(FuelCd == "NFS" || FuelCd == "MIX")) && validFuel)
					{
						int FuelCnt = 0;
						DataView LcnUnitFuel = (DataView)Category.GetCheckParameter("Location_Fuel_Records").ParameterValue;
						DateTime DfltEvalBeginDt = (DateTime)Category.GetCheckParameter("Default_Evaluation_Begin_Date").ParameterValue;
						int DfltEvalBeginHr = (int)Category.GetCheckParameter("Default_Evaluation_Begin_Hour").ParameterValue;
						DateTime DfltEvalEndDt = (DateTime)Category.GetCheckParameter("Default_Evaluation_End_Date").ParameterValue;
						int DfltEvalEndHr = (int)Category.GetCheckParameter("Default_Evaluation_End_Hour").ParameterValue;

						string DefaultUnitFuel = Category.GetCheckParameter("Default_Unit_Fuel").ValueAsString();
						string OldFilter = LcnUnitFuel.RowFilter;
						LcnUnitFuel.RowFilter = AddToDataViewFilter(OldFilter, "fuel_cd = '" + DefaultUnitFuel + "'");
						LcnUnitFuel.RowFilter = AddEvaluationDateRangeToDataViewFilter(LcnUnitFuel.RowFilter, DfltEvalBeginDt, DfltEvalEndDt,
																						false, true, false);
						if (LcnUnitFuel.Count == 0)
							Category.CheckCatalogResult = "A";
						else if (!CheckForDateRangeCovered(Category, LcnUnitFuel, DfltEvalBeginDt, DfltEvalEndDt, false, ref FuelCnt))
						{
							if (FuelCnt > 0)
								Category.CheckCatalogResult = "B";
						}
						LcnUnitFuel.RowFilter = OldFilter;
					}
				}
				else
					Log = false;

			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "DEFAULT58");
			}
			return ReturnVal;
		}
		public static string DEFAULT73(cCategory Category, ref bool Log) //NOXR LME Defaults Consistent
		{
			string ReturnVal = "";
			try
			{
				bool validParam = (bool)Category.GetCheckParameter("Default_Parameter_Code_Valid").ParameterValue;
				bool consistent = (bool)Category.GetCheckParameter("Default_Dates_and_Hours_Consistent").ParameterValue;
				DataRowView DefaultRecord = (DataRowView)Category.GetCheckParameter("Current_Default").ParameterValue;

				string ParamCd = cDBConvert.ToString(DefaultRecord["parameter_cd"]);
				string DfltSrcCd = cDBConvert.ToString(DefaultRecord["default_source_cd"]);
				string DfltPrpsCd = cDBConvert.ToString(DefaultRecord["default_purpose_cd"]);

				if (validParam && ParamCd == "NOXR" && DfltPrpsCd == "LM" && consistent)
				{
					string OpConditionCd = cDBConvert.ToString(DefaultRecord["operating_condition_cd"]);
					string FuelCd = cDBConvert.ToString(DefaultRecord["fuel_cd"]);

					DataView DefaultRecords = (DataView)Category.GetCheckParameter("Default_Records").ParameterValue;
					string DefaultFilter = DefaultRecords.RowFilter;
					string tempFilter = DefaultFilter;

					DateTime EvalBeginDate = Category.GetCheckParameter("Default_Evaluation_Begin_Date").ValueAsDateTime(DateTypes.START);
					DateTime EvalEndDate = Category.GetCheckParameter("Default_Evaluation_End_Date").ValueAsDateTime(DateTypes.END);
					int EvalBeginHour = Category.GetCheckParameter("Default_Evaluation_Begin_Hour").ValueAsInt();
					int EvalEndHour = Category.GetCheckParameter("Default_Evaluation_End_Hour").ValueAsInt();

					if (OpConditionCd == "A" || OpConditionCd == "C")
					{
						tempFilter = AddToDataViewFilter(tempFilter,
							"parameter_cd = 'NOXR' and default_purpose_cd = 'LM' and fuel_cd = '" +
							FuelCd + "' and operating_condition_cd <> '" + OpConditionCd + "'");
						tempFilter = AddEvaluationDateHourRangeToDataViewFilter(tempFilter, EvalBeginDate, EvalEndDate, EvalBeginHour, EvalBeginHour, false, true);
						DefaultRecords.RowFilter = tempFilter;
						if (DefaultRecords.Count > 0)
							Category.CheckCatalogResult = "A";
					}
					else if (OpConditionCd == "B")
					{
						tempFilter = AddToDataViewFilter(tempFilter,
							"parameter_cd = 'NOXR' and default_purpose_cd = 'LM' and default_source_cd <> 'DEF' and fuel_cd = '" +
							FuelCd + "' and operating_condition_cd = 'P'");
						tempFilter = AddEvaluationDateHourRangeToDataViewFilter(tempFilter, EvalBeginDate, EvalEndDate, EvalBeginHour, EvalBeginHour, false, true);
						DefaultRecords.RowFilter = tempFilter;
						if (DefaultRecords.Count == 0)
							Category.CheckCatalogResult = "B";
						else if (!(CheckForHourRangeCovered(Category, DefaultRecords, EvalBeginDate, EvalBeginHour, EvalEndDate, EvalEndHour)))
							Category.CheckCatalogResult = "C";
					}
					else if (OpConditionCd == "P")
					{
						tempFilter = AddToDataViewFilter(tempFilter,
							"parameter_cd = 'NOXR' and default_purpose_cd = 'LM' and default_source_cd <> 'DEF' and fuel_cd = '" +
							FuelCd + "' and operating_condition_cd = 'B'");
						tempFilter = AddEvaluationDateHourRangeToDataViewFilter(tempFilter, EvalBeginDate, EvalEndDate, EvalBeginHour, EvalBeginHour, false, true);
						DefaultRecords.RowFilter = tempFilter;
						if (DefaultRecords.Count == 0)
							Category.CheckCatalogResult = "D";
						else if (!(CheckForHourRangeCovered(Category, DefaultRecords, EvalBeginDate, EvalBeginHour, EvalEndDate, EvalEndHour)))
							Category.CheckCatalogResult = "E";
					}

					DefaultRecords.RowFilter = DefaultFilter;
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "DEFAULT73");
			}
			return ReturnVal;
		}
		public static string DEFAULT74(cCategory Category, ref bool Log) //Overlapping Default Records
		{
			string ReturnVal = "";
			try
			{
				bool validParam = (bool)Category.GetCheckParameter("Default_Parameter_Code_Valid").ParameterValue;
				if (validParam)
				{
					bool validPurpose = (bool)Category.GetCheckParameter("Default_Purpose_Code_Valid").ParameterValue;
					if (validPurpose)
					{
						DataRowView DefaultRecord = (DataRowView)Category.GetCheckParameter("Current_Default").ParameterValue;
						DataView DefaultRecords = (DataView)Category.GetCheckParameter("Default_Records").ParameterValue;

						DateTime DfltEvalBgnDt = (DateTime)Category.GetCheckParameter("Default_Evaluation_Begin_Date").ParameterValue;
						int DfltEvalBgnHr = (int)Category.GetCheckParameter("Default_Evaluation_Begin_Hour").ParameterValue;
						DateTime DfltEvalEndDt = (DateTime)Category.GetCheckParameter("Default_Evaluation_End_Date").ParameterValue;
						int DfltEvalEndHr = (int)Category.GetCheckParameter("Default_Evaluation_End_Hour").ParameterValue;

						string ParamCd = cDBConvert.ToString(DefaultRecord["parameter_cd"]);
						string DfltPrpsCd = cDBConvert.ToString(DefaultRecord["default_purpose_cd"]);
						string FuelCd = cDBConvert.ToString(DefaultRecord["fuel_cd"]);
						string OpCondnCd = cDBConvert.ToString(DefaultRecord["operating_condition_cd"]);
						string MondefId = cDBConvert.ToString(DefaultRecord["mondef_id"]);
						DateTime BeginDt = cDBConvert.ToDate(DefaultRecord["begin_date"], DateTypes.START);
						int BeginHr = cDBConvert.ToInteger(DefaultRecord["begin_hour"]);

						string OldFilter = DefaultRecords.RowFilter;
						DefaultRecords.RowFilter = AddToDataViewFilter(OldFilter, "(parameter_cd = '" + ParamCd + "' and fuel_cd = '" +
																	   FuelCd + "' and default_purpose_cd = '" + DfltPrpsCd +
																	   "' and operating_condition_cd = '" + OpCondnCd +
																	   "' and mondef_id <> '" + MondefId + "') and (begin_date > '" +
																	   BeginDt.ToShortDateString() + "' or (begin_date = '" +
																	   BeginDt.ToShortDateString() + "' and begin_hour >= " + BeginHr + "))");

						DefaultRecords.RowFilter = AddEvaluationDateHourRangeToDataViewFilter(DefaultRecords.RowFilter, DfltEvalBgnDt,
																								DfltEvalEndDt, DfltEvalBgnHr,
																								DfltEvalEndHr, false, true);
						if (DefaultRecords.Count > 0)
							Category.CheckCatalogResult = "A";
						DefaultRecords.RowFilter = OldFilter;
					}
				}
				else
					Log = false;
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "DEFAULT74");
			}
			return ReturnVal;
		}
		public static string DEFAULT75(cCategory Category, ref bool Log) //Default Value Consistent with Maximum Value
		{
			string ReturnVal = "";
			try
			{
				bool validParam = (bool)Category.GetCheckParameter("Default_Parameter_Code_Valid").ParameterValue;
				if (validParam)
				{
					bool validDefault = (bool)Category.GetCheckParameter("Default_Value_Valid").ParameterValue;
					if (validDefault)
					{
						DataRowView DefaultRecord = (DataRowView)Category.GetCheckParameter("Current_Default").ParameterValue;
						string ParamCd = cDBConvert.ToString(DefaultRecord["parameter_cd"]);
						decimal DefaultVal = cDBConvert.ToDecimal(DefaultRecord["default_value"]);
						DataView SpanRecs = (DataView)Category.GetCheckParameter("Span_Records").ParameterValue;
						DateTime EvalEndDt = (DateTime)Category.GetCheckParameter("Default_Evaluation_End_Date").ParameterValue;
						int EvalEndHr = (int)Category.GetCheckParameter("Default_Evaluation_End_Hour").ParameterValue;
						DateTime EvalBeginDt = (DateTime)Category.GetCheckParameter("Default_Evaluation_Begin_Date").ParameterValue;
						int EvalBeginHr = (int)Category.GetCheckParameter("Default_Evaluation_Begin_Hour").ParameterValue;

						string ComponentTypeCd = "";
						string RelatedMaximum = "";
						string SpanScaleCode = "";

						string spanOldFilter = SpanRecs.RowFilter;
						if (ParamCd == "CO2N")
						{
							ComponentTypeCd = "CO2";
							RelatedMaximum = "MEC or MPC for CO2";
						}
						else if (ParamCd == "O2N")
						{
							ComponentTypeCd = "O2";
							RelatedMaximum = "MEC or MPC for O2";
						}
						else if (ParamCd == "SO2X")
						{
							ComponentTypeCd = "SO2";
							RelatedMaximum = "MPC for SO2";
							SpanScaleCode = "H";
						}
						else if (ParamCd == "NOCX")
						{
							ComponentTypeCd = "NOX";
							RelatedMaximum = "MPC for NOX";
							SpanScaleCode = "H";
						}
						else if (ParamCd == "FLOX")
						{
							SpanRecs.RowFilter = AddToDataViewFilter(spanOldFilter, "component_type_cd = 'FLOW'");
							SpanRecs.RowFilter = AddEvaluationDateHourRangeToDataViewFilter(SpanRecs.RowFilter, EvalBeginDt, EvalEndDt, EvalBeginHr, EvalEndHr, false, true);/*check whether all*/
							if (SpanRecs.Count > 0)
							{
								//int newDfltVal = (int)DefaultVal;

								SpanRecs.RowFilter = AddToDataViewFilter(SpanRecs.RowFilter, "mpf_value > 0 and mpf_value < " + DefaultVal);

								if (SpanRecs.Count > 0)
								{
									Category.SetCheckParameter("Related_Maximum", "MPF", eParameterDataType.String);
									Category.CheckCatalogResult = "A";
								}
							}
						}
						else if (ParamCd == "MNNX")
						{
							DataView DefaultRecs = (DataView)Category.GetCheckParameter("Default_Records").ParameterValue;
							string DefaultOldFilter = DefaultRecs.RowFilter;
							DefaultRecs.RowFilter = AddToDataViewFilter(DefaultOldFilter, "parameter_cd = 'NORX'");
							DefaultRecs.RowFilter = AddEvaluationDateHourRangeToDataViewFilter(DefaultRecs.RowFilter, EvalBeginDt, EvalEndDt,
																									EvalBeginHr, EvalEndHr, false, true);
							DefaultRecs.RowFilter = AddToDataViewFilter(DefaultRecs.RowFilter, "default_value > 0 and default_value < " + DefaultVal.ToString());
							if (DefaultRecs.Count > 0)
							{
								Category.SetCheckParameter("Related_Maximum", "Maximum NOx Emission Rate", eParameterDataType.String);
								Category.CheckCatalogResult = "A";
							}
							DefaultRecs.RowFilter = DefaultOldFilter;
						}
						else if (ParamCd == "MNGF" || ParamCd == "MNOF")
						{
							DataView SysFuelFlowRecs = (DataView)Category.GetCheckParameter("Location_System_Fuel_Flow_Records").ParameterValue;
							string FuelCode = cDBConvert.ToString(DefaultRecord["fuel_cd"]);
							string UOMCd = cDBConvert.ToString(DefaultRecord["default_uom_cd"]);
							string FuelFlowOldFilter = SysFuelFlowRecs.RowFilter;
							SysFuelFlowRecs.RowFilter = AddToDataViewFilter(FuelFlowOldFilter, "fuel_cd = '" + FuelCode + "' and sys_fuel_uom_cd = '" + UOMCd + "'");
							SysFuelFlowRecs.RowFilter = AddEvaluationDateHourRangeToDataViewFilter(SysFuelFlowRecs.RowFilter, EvalBeginDt, EvalEndDt,
																									EvalBeginHr, EvalEndHr, false, true);
							SysFuelFlowRecs.RowFilter = AddToDataViewFilter(SysFuelFlowRecs.RowFilter, "max_rate > 0 and max_rate < " + DefaultVal.ToString());
							if (SysFuelFlowRecs.Count > 0)
							{
								Category.SetCheckParameter("Related_Maximum", "Maximum Fuel Flow Rate", eParameterDataType.String);
								Category.CheckCatalogResult = "A";
							}
							SysFuelFlowRecs.RowFilter = FuelFlowOldFilter;
						}
						else if (ParamCd == "H2ON")
						{
							DataView DefaultRecs = (DataView)Category.GetCheckParameter("Default_Records").ParameterValue;
							string DefaultOldFilter = DefaultRecs.RowFilter;
							DefaultRecs.RowFilter = AddToDataViewFilter(DefaultOldFilter, "parameter_cd = 'H2OX'");
							DefaultRecs.RowFilter = AddEvaluationDateHourRangeToDataViewFilter(DefaultRecs.RowFilter, EvalBeginDt, EvalEndDt,
																									EvalBeginHr, EvalEndHr, false, true);
							DefaultRecs.RowFilter = AddToDataViewFilter(DefaultRecs.RowFilter, "default_value > 0 and default_value < " + DefaultVal.ToString());
							if (DefaultRecs.Count > 0)
							{
								Category.SetCheckParameter("Related_Maximum", "Maximum Percent H2O", eParameterDataType.String);
								Category.CheckCatalogResult = "A";
							}
							DefaultRecs.RowFilter = DefaultOldFilter;
						}
						else if (ParamCd == "MNHI")
						{
							DataView UnitCapacityRecs = (DataView)Category.GetCheckParameter("Location_Capacity_Records").ParameterValue;/*add param*/
							string UnitCapacityOldFilter = UnitCapacityRecs.RowFilter;
							//UnitCapacityRecs.RowFilter = AddToDataViewFilter(UnitCapacityOldFilter, "max_hi_capacity > '0' and max_hi_capacity < '" + DefaultVal.ToString() + "'");
							UnitCapacityRecs.RowFilter = AddEvaluationDateRangeToDataViewFilter(UnitCapacityRecs.RowFilter, EvalBeginDt, EvalEndDt, false, true, false);
							UnitCapacityRecs.RowFilter = AddToDataViewFilter(UnitCapacityOldFilter, "max_hi_capacity > 0 and max_hi_capacity < " + DefaultVal.ToString());

							if (UnitCapacityRecs.Count > 0)
							{
								Category.SetCheckParameter("Related_Maximum", "Maximum Hourly Heat Input", eParameterDataType.String);
								Category.CheckCatalogResult = "A";
							}
							UnitCapacityRecs.RowFilter = UnitCapacityOldFilter;
						}

						if (ParamCd.InList("CO2N,O2N,SO2X,NOCX"))
						{
							SpanRecs.RowFilter = AddToDataViewFilter(spanOldFilter, "component_type_cd = '" + ComponentTypeCd + "'");
							if (SpanScaleCode != "")
								SpanRecs.RowFilter = AddToDataViewFilter(SpanRecs.RowFilter, "span_scale_cd = '" + SpanScaleCode + "'");
							SpanRecs.RowFilter = AddEvaluationDateHourRangeToDataViewFilter(SpanRecs.RowFilter, EvalBeginDt, EvalEndDt, EvalBeginHr, EvalEndHr, false, true);
							if (SpanRecs.Count > 0)
							{
								if (SpanScaleCode == "")
									SpanRecs.RowFilter = AddToDataViewFilter(SpanRecs.RowFilter, "(mpc_value > 0 and mpc_value < " + DefaultVal.ToString() +
																			") or (mec_value > 0 and mec_value < " + DefaultVal + ")");
								else//SpanScaleCode == H
									SpanRecs.RowFilter = AddToDataViewFilter(SpanRecs.RowFilter, "mpc_value > 0 and mpc_value < " + DefaultVal.ToString());

								if (SpanRecs.Count > 0)
								{
									Category.SetCheckParameter("Related_Maximum", RelatedMaximum, eParameterDataType.String);
									Category.CheckCatalogResult = "A";
								}
							}
						}

						SpanRecs.RowFilter = spanOldFilter;
					}
				}
				else
					Log = false;
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "DEFAULT75");
			}
			return ReturnVal;
		}
		public static string DEFAULT95(cCategory Category, ref bool Log) //Duplicate Default Records
		{
			string ReturnVal = "";
			try
			{
				DataRowView DefaultRecord = (DataRowView)Category.GetCheckParameter("Current_Default").ParameterValue;
				DataView DefaultRecords = (DataView)Category.GetCheckParameter("Default_Records").ParameterValue;

				string ParamCd = cDBConvert.ToString(DefaultRecord["parameter_cd"]);
				string DfltPrpsCd = cDBConvert.ToString(DefaultRecord["default_purpose_cd"]);
				string FuelCd = cDBConvert.ToString(DefaultRecord["fuel_cd"]);
				string OpCondnCd = cDBConvert.ToString(DefaultRecord["operating_condition_cd"]);
				string MondefId = cDBConvert.ToString(DefaultRecord["mondef_id"]);
				DateTime BeginDt = cDBConvert.ToDate(DefaultRecord["begin_date"], DateTypes.START);
				int BeginHr = cDBConvert.ToInteger(DefaultRecord["begin_hour"]);
				DateTime EndDt = cDBConvert.ToDate(DefaultRecord["end_date"], DateTypes.END);
				int EndHr = cDBConvert.ToInteger(DefaultRecord["end_hour"]);

				string OldFilter = DefaultRecords.RowFilter;
				DefaultRecords.RowFilter = AddToDataViewFilter(OldFilter, "parameter_cd = '" + ParamCd + "' and fuel_cd = '" +
															   FuelCd + "' and default_purpose_cd = '" + DfltPrpsCd +
															   "' and operating_condition_cd = '" + OpCondnCd +
															   "' and begin_date = '" + BeginDt.ToShortDateString() +
															   "'and begin_hour = '" + BeginHr.ToString() + "' and mondef_id <> '" + MondefId + "'");
				if (DefaultRecords.Count > 0)
					Category.CheckCatalogResult = "A";
				else
				{
					if (DefaultRecord["end_date"] != DBNull.Value)
					{
						DefaultRecords.RowFilter = AddToDataViewFilter(OldFilter, "parameter_cd = '" + ParamCd + "' and fuel_cd = '" +
															   FuelCd + "' and default_purpose_cd = '" + DfltPrpsCd +
															   "' and operating_condition_cd = '" + OpCondnCd +
															   "' and end_date = '" + EndDt.ToShortDateString() +
															   "'and end_hour = '" + EndHr.ToString() + "' and mondef_id <> '" + MondefId + "'");
						if (DefaultRecords.Count > 0)
							Category.CheckCatalogResult = "A";
					}
				}
				DefaultRecords.RowFilter = OldFilter;

			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "DEFAULT95");
			}
			return ReturnVal;
		}
		public static string DEFAULT98(cCategory Category, ref bool Log) //Default Group ID Valid
		{
			string ReturnVal = "";
			try
			{
				DataRowView CurrentDefault = Category.GetCheckParameter("Current_Default").ValueAsDataRowView();
				bool ValidParamCode = Category.GetCheckParameter("Default_Parameter_Code_Valid").ValueAsBool();

				if (ValidParamCode)
				{
					if (CurrentDefault["group_id"] != DBNull.Value)
					{
						if ((cDBConvert.ToString(CurrentDefault["parameter_cd"]) != "NOXR") || cDBConvert.ToString(CurrentDefault["default_purpose_cd"]) != "LM")
							Category.CheckCatalogResult = "B";
					}
				}
				else
					Log = false;
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "DEFAULT98");
			}
			return ReturnVal;
		}
		public static string DEFAULT99(cCategory Category, ref bool Log) //Required Missing Data Default for NOXR LME Default
		{
			string ReturnVal = "";
			try
			{
				DataView DefaultRecords = (DataView)Category.GetCheckParameter("Default_Records").ParameterValue;
				DataRowView DefaultRecord = Category.GetCheckParameter("Current_Default").ValueAsDataRowView();
				bool ValidParamCode = Category.GetCheckParameter("Default_Parameter_Code_Valid").ValueAsBool();
				bool consistent = (bool)Category.GetCheckParameter("Default_Dates_and_Hours_Consistent").ParameterValue;

				string ParamCd = cDBConvert.ToString(DefaultRecord["parameter_cd"]);
				string DfltPrpsCd = cDBConvert.ToString(DefaultRecord["default_purpose_cd"]);
				string DfltSrcCd = cDBConvert.ToString(DefaultRecord["default_source_cd"]);

				if (ValidParamCode && ParamCd == "NOXR" && DfltPrpsCd == "LM" && DfltSrcCd != "DEF" && consistent)
				{
					string FuelCd = cDBConvert.ToString(DefaultRecord["fuel_cd"]);

					DateTime EvalBeginDate = Category.GetCheckParameter("Default_Evaluation_Begin_Date").ValueAsDateTime(DateTypes.START);
					DateTime EvalEndDate = Category.GetCheckParameter("Default_Evaluation_End_Date").ValueAsDateTime(DateTypes.END);
					int EvalBeginHour = Category.GetCheckParameter("Default_Evaluation_Begin_Hour").ValueAsInt();
					int EvalEndHour = Category.GetCheckParameter("Default_Evaluation_End_Hour").ValueAsInt();

					string sDfltFilter = DefaultRecords.RowFilter;
					DefaultRecords.RowFilter = AddToDataViewFilter(sDfltFilter, "parameter_cd = 'NORX' and default_purpose_cd = 'MD' and fuel_cd = '" + FuelCd + "'");
					DefaultRecords.RowFilter = AddEvaluationDateHourRangeToDataViewFilter(DefaultRecords.RowFilter, EvalBeginDate, EvalEndDate, EvalBeginHour, EvalEndHour, false, true);

					if (DefaultRecords.Count == 0)
						Category.CheckCatalogResult = "A";
					else if (!(CheckForHourRangeCovered(Category, DefaultRecords, EvalBeginDate, EvalBeginHour, EvalEndDate, EvalEndHour)))
						Category.CheckCatalogResult = "B";
					DefaultRecords.RowFilter = sDfltFilter;
				}
				else
					Log = false;
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "DEFAULT99");
			}
			return ReturnVal;
		}

		#endregion

		# region waf

		public static string DEFAULT78(cCategory Category, ref bool Log) //Rectangular Duct WAF Duct Width at Test Location Valid
		{
			string ReturnVal = "";
			try
			{
				DataRowView CurrentWAF = (DataRowView)Category.GetCheckParameter("Current_WAF").ParameterValue;

				if (CurrentWAF["duct_width"] == DBNull.Value)
					Category.CheckCatalogResult = "A";
				else if (cDBConvert.ToDecimal(CurrentWAF["duct_width"]) <= 0)
					Category.CheckCatalogResult = "B";
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "DEFAULT78");
			}
			return ReturnVal;
		}
		public static string DEFAULT79(cCategory Category, ref bool Log) //Rectangular Duct WAF Duct Depth at Test Location Valid
		{
			string ReturnVal = "";
			try
			{
				DataRowView CurrentWAF = (DataRowView)Category.GetCheckParameter("Current_WAF").ParameterValue;
				if (CurrentWAF["duct_depth"] == DBNull.Value)
					Category.CheckCatalogResult = "A";
				else if (cDBConvert.ToDecimal(CurrentWAF["duct_depth"]) <= 0)
					Category.CheckCatalogResult = "B";
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "DEFAULT79");
			}
			return ReturnVal;
		}
		public static string DEFAULT80(cCategory Category, ref bool Log) //Rectangular Duct WAF Value Valid
		{
			string ReturnVal = "";
			try
			{
				DataRowView CurrentWAF = (DataRowView)Category.GetCheckParameter("Current_WAF").ParameterValue;
				if (CurrentWAF["waf_value"] == DBNull.Value)
					Category.CheckCatalogResult = "A";
				else
				{
					string WAFMethodCd = cDBConvert.ToString(CurrentWAF["waf_method_cd"]);
					decimal WAFVal = cDBConvert.ToDecimal(CurrentWAF["waf_value"]);
					if (WAFVal > 0m && WAFVal < 1m)
					{
						if ((WAFMethodCd == "FT" || WAFMethodCd == "AT") && WAFVal < 0.94m)
							Category.CheckCatalogResult = "B";
						else if (WAFMethodCd == "DF" && WAFVal < 0.95m)
							Category.CheckCatalogResult = "C";
					}
					else
						Category.CheckCatalogResult = "D";
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "DEFAULT80");
			}
			return ReturnVal;
		}
		public static string DEFAULT81(cCategory Category, ref bool Log) //Rectangular Duct WAF Method of Determination Valid
		{
			string ReturnVal = "";
			try
			{
				DataRowView CurrentWAF = (DataRowView)Category.GetCheckParameter("Current_WAF").ParameterValue;
				if (CurrentWAF["waf_method_cd"] == DBNull.Value)
					Category.CheckCatalogResult = "A";
				else
				{
					DataView WAFMethodCodeLookup = (DataView)Category.GetCheckParameter("WAF_Method_COde_Lookup_Table").ParameterValue;
					string OldFilter = WAFMethodCodeLookup.RowFilter;
					WAFMethodCodeLookup.RowFilter = AddToDataViewFilter(OldFilter, "waf_method_cd = '" +
																		cDBConvert.ToString(CurrentWAF["waf_method_cd"]) + "'");
					if (WAFMethodCodeLookup.Count == 0)
						Category.CheckCatalogResult = "B";

					WAFMethodCodeLookup.RowFilter = OldFilter;
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "DEFAULT81");
			}
			return ReturnVal;
		}
		public static string DEFAULT82(cCategory Category, ref bool Log) //Rectangular Duct WAF Effective Date Valid
		{
			string ReturnVal = "";
			try
			{
				DataRowView CurrentWAF = (DataRowView)Category.GetCheckParameter("Current_WAF").ParameterValue;
				DateTime WAFEffectiveDate = cDBConvert.ToDate(CurrentWAF["waf_effective_date"], DateTypes.START);
				bool DateValid = true;

				if (CurrentWAF["waf_effective_date"] != DBNull.Value)
				{
					if (WAFEffectiveDate < new DateTime(2004, 1, 1) || WAFEffectiveDate > DateTime.Now)
					{
						Category.CheckCatalogResult = "B";
						DateValid = false;
					}
				}
				else
				{
					Category.CheckCatalogResult = "A";
					DateValid = false;
				}

				Category.SetCheckParameter("Rectangular_Duct_WAF_Effective_Date_Valid", DateValid, eParameterDataType.Boolean);
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "DEFAULT82");
			}
			return ReturnVal;
		}
		public static string DEFAULT83(cCategory Category, ref bool Log) //Rectangular Duct WAF Effective Hour Valid
		{
			string ReturnVal = "";
			try
			{
				ReturnVal = Check_ValidStartHour(Category, "Rectangular_Duct_WAF_Effective_Hour_Valid",
													  "Current_WAF", "waf_effective_hour", "A", "B", "");
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "DEFAULT83");
			}
			return ReturnVal;
		}
		public static string DEFAULT84(cCategory Category, ref bool Log) //Rectangular Duct WAF End Date Valid
		{
			string ReturnVal = "";
			try
			{
				DataRowView CurrentWAF = (DataRowView)Category.GetCheckParameter("Current_WAF").ParameterValue;
				DateTime EndDate = cDBConvert.ToDate(CurrentWAF["end_date"], DateTypes.END);
				bool EndDateValid = true;

				if (CurrentWAF["end_date"] != DBNull.Value)
				{
					if (EndDate < new DateTime(2004, 1, 1) || EndDate > DateTime.Now)
					{
						Category.CheckCatalogResult = "A";
						EndDateValid = false;
					}
				}

				Category.SetCheckParameter("Rectangular_Duct_WAF_End_Date_Valid", EndDateValid, eParameterDataType.Boolean);
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "DEFAULT84");
			}
			return ReturnVal;
		}
		public static string DEFAULT85(cCategory Category, ref bool Log) //Rectangular Duct WAF End Hour Valid
		{
			string ReturnVal = "";
			try
			{
				ReturnVal = Check_ValidEndHour(Category, "Rectangular_Duct_WAF_End_Hour_Valid", "Current_WAF");
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "DEFAULT85");
			}
			return ReturnVal;
		}
		public static string DEFAULT86(cCategory Category, ref bool Log) //Rectangular Duct WAF Determination Date Valid
		{
			string ReturnVal = "";
			try
			{
				DataRowView CurrentWAF = (DataRowView)Category.GetCheckParameter("Current_WAF").ParameterValue;
				DateTime determinationDate = cDBConvert.ToDate(CurrentWAF["waf_determined_date"], DateTypes.END);
				bool WafEffDateValid = (bool)Category.GetCheckParameter("Rectangular_Duct_WAF_Effective_Date_Valid").ParameterValue;

				if (CurrentWAF["waf_determined_date"] == DBNull.Value)
					Category.CheckCatalogResult = "A";
				else if (determinationDate < new DateTime(2004, 1, 1) || determinationDate > DateTime.Now)
					Category.CheckCatalogResult = "B";
				else if (WafEffDateValid && cDBConvert.ToDate(CurrentWAF["waf_effective_date"], DateTypes.START).Year < determinationDate.Year)
					Category.CheckCatalogResult = "C";

			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "DEFAULT86");
			}
			return ReturnVal;
		}
		public static string DEFAULT87(cCategory Category, ref bool Log) //Rectangular Duct WAF Number of WAF Test Runs Valid
		{
			string ReturnVal = "";
			try
			{
				DataRowView CurrentWAF = (DataRowView)Category.GetCheckParameter("Current_WAF").ParameterValue;
				if (CurrentWAF["num_test_runs"] == DBNull.Value)
					Category.CheckCatalogResult = "A";
				else
				{
					int numTestRuns = cDBConvert.ToInteger(CurrentWAF["num_test_runs"]);
					if (numTestRuns < 1 || numTestRuns > 99)
						Category.CheckCatalogResult = "B";
					else
					{
						string wafMethodCd = cDBConvert.ToString(CurrentWAF["waf_method_cd"]);
						if ((wafMethodCd == "FT" || wafMethodCd == "AT") && numTestRuns < 3)
							Category.CheckCatalogResult = "C";
					}
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "DEFAULT87");
			}
			return ReturnVal;
		}
		public static string DEFAULT88(cCategory Category, ref bool Log) //Rectangular Duct WAF Number of Method 1 Traverse Points in WAF Test Valid
		{
			string ReturnVal = "";
			try
			{
				DataRowView CurrentWAF = (DataRowView)Category.GetCheckParameter("Current_WAF").ParameterValue;
				Category.SetCheckParameter("Rectangular_Duct_WAF_Number_of_Method_1_Traverse_Points_in_WAF_Test_Valid", true, eParameterDataType.Boolean);

				if (CurrentWAF["num_traverse_points_waf"] == DBNull.Value)
				{
					Category.CheckCatalogResult = "A";
					Category.SetCheckParameter("Rectangular_Duct_WAF_Number_of_Method_1_Traverse_Points_in_WAF_Test_Valid", false, eParameterDataType.Boolean);
				}
				else
				{
					int numTrvrsPtsWaf = cDBConvert.ToInteger(CurrentWAF["num_traverse_points_waf"]);
					if (numTrvrsPtsWaf < 12 || numTrvrsPtsWaf > 99)
					{
						Category.CheckCatalogResult = "B";
						Category.SetCheckParameter("Rectangular_Duct_WAF_Number_of_Method_1_Traverse_Points_in_WAF_Test_Valid", false, eParameterDataType.Boolean);
					}
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "DEFAULT88");
			}
			return ReturnVal;
		}
		public static string DEFAULT89(cCategory Category, ref bool Log) //Rectangular Duct WAF Number of Test Ports in WAF Test Valid
		{
			string ReturnVal = "";
			try
			{
				DataRowView CurrentWAF = (DataRowView)Category.GetCheckParameter("Current_WAF").ParameterValue;
				if (CurrentWAF["num_test_ports"] == DBNull.Value)
					Category.CheckCatalogResult = "A";
				else
				{
					int numTestPorts = cDBConvert.ToInteger(CurrentWAF["num_test_ports"]);
					string wafMethodCd = cDBConvert.ToString(CurrentWAF["waf_method_cd"]);
					if (numTestPorts < 1 || numTestPorts > 99)
						Category.CheckCatalogResult = "B";
					else if ((wafMethodCd == "FT" || wafMethodCd == "AT") && numTestPorts < 4)
						Category.CheckCatalogResult = "C";
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "DEFAULT89");
			}
			return ReturnVal;
		}
		public static string DEFAULT90(cCategory Category, ref bool Log) //Rectangular Duct WAF Number of Method 1 Traverse Points in Reference Flow Rata Test Valid
		{
			string ReturnVal = "";
			try
			{
				DataRowView CurrentWAF = (DataRowView)Category.GetCheckParameter("Current_WAF").ParameterValue;
				bool numTrvrsPtsWafValid =
					(bool)Category.GetCheckParameter("Rectangular_Duct_WAF_Number_of_Method_1_Traverse_Points_in_WAF_Test_Valid").ParameterValue;

				if (CurrentWAF["num_traverse_points_ref"] == DBNull.Value)
					Category.CheckCatalogResult = "A";
				else
				{
					int numTrvrsPtsRef = cDBConvert.ToInteger(CurrentWAF["num_traverse_points_ref"]);
					int numTrvrsPtsWaf = cDBConvert.ToInteger(CurrentWAF["num_traverse_points_waf"]);
					if (numTrvrsPtsRef < 12 || numTrvrsPtsRef > 99)
						Category.CheckCatalogResult = "B";
					else if (numTrvrsPtsWafValid && numTrvrsPtsRef != numTrvrsPtsWaf)
						Category.CheckCatalogResult = "C";
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "DEFAULT90");
			}
			return ReturnVal;
		}
		public static string DEFAULT91(cCategory Category, ref bool Log) //WAF Dates and Hours Consistent
		{
			string ReturnVal = "";
			try
			{
				ReturnVal = Check_ConsistentHourRange(Category, "WAF_Dates_And_Hours_Consistent",
															   "Current_WAF",
															   "Rectangular_Duct_WAF_Effective_Date_Valid",
															   "Rectangular_Duct_WAF_Effective_Hour_Valid",
															   "Rectangular_Duct_WAF_End_Date_Valid",
															   "Rectangular_Duct_WAF_End_Hour_Valid",
															   "waf_effective_date", "waf_effective_hour",
															   "end_date", "end_hour", "A", "B", "C", "");
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "DEFAULT91");
			}
			return ReturnVal;
		}
		public static string DEFAULT92(cCategory Category, ref bool Log) //WAF Active Status
		{
			string ReturnVal = "";
			try
			{
				bool DatesAndHoursConsistent = (bool)Category.GetCheckParameter("WAF_Dates_and_Hours_Consistent").ParameterValue;

				if (DatesAndHoursConsistent)
					ReturnVal = Check_ActiveHourRange(Category, "WAF_Active_Status",
																"Current_WAF",
																"WAF_Evaluation_Begin_Date",
																"WAF_Evaluation_Begin_Hour",
																"WAF_Evaluation_End_Date",
																"WAF_Evaluation_End_Hour",
																"waf_effective_date", "waf_effective_hour",
																"end_date", "end_hour");
				else
					Log = false;
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "DEFAULT92");
			}
			return ReturnVal;
		}
		public static string DEFAULT93(cCategory Category, ref bool Log) //Flow System reported for WAF Record 
		{
			string ReturnVal = "";
			try
			{
				DataRowView CurrentWAF = (DataRowView)Category.GetCheckParameter("Current_WAF").ParameterValue;
				DataView MonSysRecs = (DataView)Category.GetCheckParameter("Monitor_System_Records").ParameterValue;
				DateTime EvalBeginDt = (DateTime)Category.GetCheckParameter("WAF_Evaluation_Begin_Date").ParameterValue;
				int EvalBeginHr = (int)Category.GetCheckParameter("WAF_Evaluation_Begin_Hour").ParameterValue;
				DateTime EvalEndDt = (DateTime)Category.GetCheckParameter("WAF_Evaluation_End_Date").ParameterValue;
				int EvalEndHr = (int)Category.GetCheckParameter("WAF_Evaluation_End_Hour").ParameterValue;

				string OldFilter = MonSysRecs.RowFilter;
				MonSysRecs.RowFilter = AddToDataViewFilter(OldFilter, "sys_type_cd = 'FLOW'");
				MonSysRecs.RowFilter = AddEvaluationDateHourRangeToDataViewFilter(MonSysRecs.RowFilter, EvalBeginDt, EvalEndDt,
																					EvalBeginHr, EvalEndHr, false, true);
				if (MonSysRecs.Count == 0)
					Category.CheckCatalogResult = "A";

				MonSysRecs.RowFilter = OldFilter;
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "DEFAULT93");
			}
			return ReturnVal;
		}
		public static string DEFAULT96(cCategory Category, ref bool Log) //Duplicate WAF Records
		{
			string ReturnVal = "";
			try
			{
				DataRowView CurrentWAF = (DataRowView)Category.GetCheckParameter("Current_WAF").ParameterValue;
				DataView WAFRecords = (DataView)Category.GetCheckParameter("WAF_Records").ParameterValue;
				DateTime EffDt = cDBConvert.ToDate(CurrentWAF["waf_effective_date"], DateTypes.START);
				int EffHr = cDBConvert.ToInteger(CurrentWAF["waf_effective_hour"]);
				string OldFilter = WAFRecords.RowFilter;
				WAFRecords.RowFilter = AddToDataViewFilter(OldFilter, "waf_effective_date = '" + EffDt.ToShortDateString() +
															"' and waf_effective_hour = '" + EffHr.ToString() + "' and rect_duct_waf_data_id <> '" +
															cDBConvert.ToString(CurrentWAF["rect_duct_waf_data_id"]) + "'");
				if (WAFRecords.Count > 0)
					Category.CheckCatalogResult = "A";
				else
				{
					DateTime EndDt = cDBConvert.ToDate(CurrentWAF["end_date"], DateTypes.END);
					int EndHr = cDBConvert.ToInteger(CurrentWAF["end_hour"]);
					if (CurrentWAF["end_date"] != DBNull.Value)
					{
						WAFRecords.RowFilter = AddToDataViewFilter(OldFilter, "end_date = '" + EndDt.ToShortDateString() +
																	 "' and end_hour = '" + EndHr.ToString() + "' and rect_duct_waf_data_id <> '" +
																	  cDBConvert.ToString(CurrentWAF["rect_duct_waf_data_id"]) + "'");
						if (WAFRecords.Count > 0)
							Category.CheckCatalogResult = "A";
					}
				}
				WAFRecords.RowFilter = OldFilter;
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "DEFAULT96");
			}
			return ReturnVal;
		}
		public static string DEFAULT97(cCategory Category, ref bool Log) //WAF Record Consistent with Stack Shape
		{
			string ReturnVal = "";
			try
			{
				DataRowView CurrentWAF = (DataRowView)Category.GetCheckParameter("Current_WAF").ParameterValue;
				DataView LcnAtrrbtRecs = (DataView)Category.GetCheckParameter("Location_Attribute_Records").ParameterValue;
				DateTime EvalBeginDt = (DateTime)Category.GetCheckParameter("WAF_Evaluation_Begin_Date").ParameterValue;
				DateTime EvalEndDt = (DateTime)Category.GetCheckParameter("WAF_Evaluation_End_Date").ParameterValue;


				string OldFilter = LcnAtrrbtRecs.RowFilter;
				LcnAtrrbtRecs.RowFilter = AddToDataViewFilter(OldFilter, "shape_cd = 'RECT'");
				LcnAtrrbtRecs.RowFilter = AddEvaluationDateRangeToDataViewFilter(LcnAtrrbtRecs.RowFilter, EvalBeginDt, EvalEndDt,
																					false, true, false);
				if (LcnAtrrbtRecs.Count == 0)
					Category.CheckCatalogResult = "A";

				LcnAtrrbtRecs.RowFilter = OldFilter;
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "DEFAULT97");
			}
			return ReturnVal;
		}

		# endregion

	}
}
