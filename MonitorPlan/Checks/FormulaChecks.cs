using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;
using ECMPS.Checks.Mp.Parameters;
using ECMPS.Definitions.Enumerations;

using ECMPS.Definitions.Extensions;


namespace ECMPS.Checks.FormulaChecks
{
	public class cFormulaChecks : cChecks
	{
		public cFormulaChecks()
		{
			CheckProcedures = new dCheckProcedure[21];

			CheckProcedures[1] = new dCheckProcedure(FORMULA1);
			CheckProcedures[2] = new dCheckProcedure(FORMULA2);
			CheckProcedures[3] = new dCheckProcedure(FORMULA3);
			CheckProcedures[4] = new dCheckProcedure(FORMULA4);
			CheckProcedures[5] = new dCheckProcedure(FORMULA5);
			CheckProcedures[6] = new dCheckProcedure(FORMULA6);
			CheckProcedures[7] = new dCheckProcedure(FORMULA7);
			CheckProcedures[8] = new dCheckProcedure(FORMULA8);
			CheckProcedures[9] = new dCheckProcedure(FORMULA9);
			CheckProcedures[10] = new dCheckProcedure(FORMULA10);
			CheckProcedures[11] = new dCheckProcedure(FORMULA11);
			CheckProcedures[12] = new dCheckProcedure(FORMULA12);
			CheckProcedures[13] = new dCheckProcedure(FORMULA13);
			CheckProcedures[14] = new dCheckProcedure(FORMULA14);
			CheckProcedures[15] = new dCheckProcedure(FORMULA15);
			CheckProcedures[16] = new dCheckProcedure(FORMULA16);
			CheckProcedures[17] = new dCheckProcedure(FORMULA17);
			CheckProcedures[18] = new dCheckProcedure(FORMULA18);
            CheckProcedures[19] = new dCheckProcedure(FORMULA19);
            CheckProcedures[20] = new dCheckProcedure(FORMULA20);

        }

		public static string FORMULA1(cCategory Category, ref bool Log) //Formula Start Date Valid
		{
			string ReturnVal = "";

			try
			{
				ReturnVal = Check_ValidStartDate(Category, "Formula_Start_Date_Valid", "Current_Formula");
			}
			catch (Exception ex)
			{ ReturnVal = Category.CheckEngine.FormatError(ex, "FORMULA1"); }

			return ReturnVal;
		}

		public static string FORMULA2(cCategory Category, ref bool Log) //Formula Start Hour Valid
		{
			string ReturnVal = "";

			try
			{
				ReturnVal = Check_ValidStartHour(Category, "Formula_Start_Hour_Valid", "Current_Formula");
			}
			catch (Exception ex)
			{ ReturnVal = Category.CheckEngine.FormatError(ex, "FORMULA2"); }

			return ReturnVal;
		}

		public static string FORMULA3(cCategory Category, ref bool Log) //Formula End Date Valid
		{
			string ReturnVal = "";

			try
			{
				ReturnVal = Check_ValidEndDate(Category, "Formula_End_Date_Valid",
														 "Current_Formula");
			}
			catch (Exception ex)
			{ ReturnVal = Category.CheckEngine.FormatError(ex, "FORMULA3"); }

			return ReturnVal;
		}

		public static string FORMULA4(cCategory Category, ref bool Log) //Formula End Hour Valid
		{
			string ReturnVal = "";

			try
			{
				ReturnVal = Check_ValidEndHour(Category, "Formula_End_Hour_Valid", "Current_Formula");
			}
			catch (Exception ex)
			{ ReturnVal = Category.CheckEngine.FormatError(ex, "FORMULA4"); }

			return ReturnVal;
		}

		public static string FORMULA5(cCategory Category, ref bool Log) //Formula Dates and Hours Consistent
		{
			string ReturnVal = "";

			try
			{
				ReturnVal = Check_ConsistentHourRange(Category, "Formula_Dates_And_Hours_Consistent",
																"Current_Formula",
																"Formula_Start_Date_Valid",
																"Formula_Start_Hour_Valid",
																"Formula_End_Date_Valid",
																"Formula_End_Hour_Valid");
			}
			catch (Exception ex)
			{ ReturnVal = Category.CheckEngine.FormatError(ex, "FORMULA5"); }

			return ReturnVal;

		}

		public static string FORMULA6(cCategory Category, ref bool Log) //Formula Active Status
		{
			string ReturnVal = "";

			try
			{
				Category.SetCheckParameter("Formula_Active_Status", null, eParameterDataType.Boolean);

				bool FormulaDatesAndHoursConsistent = (bool)Category.GetCheckParameter("Formula_Dates_And_Hours_Consistent").ParameterValue;

				if (FormulaDatesAndHoursConsistent)
					ReturnVal = Check_ActiveHourRange(Category, "Formula_Active_Status",
																"Current_Formula",
																"Formula_Evaluation_Begin_Date",
																"Formula_Evaluation_Begin_Hour",
																"Formula_Evaluation_End_Date",
																"Formula_Evaluation_End_Hour");
				else
					Log = false;
			}
			catch (Exception ex)
			{ ReturnVal = Category.CheckEngine.FormatError(ex, "FORMULA6"); }

			return ReturnVal;
		}

		public static string FORMULA7(cCategory Category, ref bool Log) //Formula ID Valid
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentFormula = (DataRowView)Category.GetCheckParameter("Current_Formula").ParameterValue;
				string FormulaIdentifier = cDBConvert.ToString(CurrentFormula["Formula_Identifier"]);


				if (CurrentFormula["Formula_identifier"] == DBNull.Value)
					Category.CheckCatalogResult = "A";
				else if (FormulaIdentifier.Length != 3 || !(cStringValidator.IsAlphaNumeric(FormulaIdentifier.Replace("-", ""))))
					Category.CheckCatalogResult = "B";
				//else if (!(cStringValidator.IsAlphaNumeric(FormulaIdentifier)))
				//{
				//    if (!FormulaIdentifier.Contains("-"))
				//        Category.CheckCatalogResult = "B";
				//    else
				//    {
				//        int dash = FormulaIdentifier.IndexOf("-");
				//        string FormID = FormulaIdentifier.re
				//    }
				//}
			}
			catch (Exception ex)
			{ ReturnVal = Category.CheckEngine.FormatError(ex, "FORMULA7"); }

			return ReturnVal;
		}

		public static string FORMULA8(cCategory Category, ref bool Log) //Formula Parameter Code Valid
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentFormula = (DataRowView)Category.GetCheckParameter("Current_Formula").ParameterValue;
				DataView FormulaParameterList = (DataView)Category.GetCheckParameter("Formula_Parameter_List").ParameterValue;
				bool FormulaParameterValid = true;

				if (CurrentFormula["parameter_cd"] == DBNull.Value)
				{
					Category.CheckCatalogResult = "A";
					FormulaParameterValid = false;
				}
				else
				{
					string ParameterCode = cDBConvert.ToString(CurrentFormula["parameter_cd"]);
					string OldFilter = FormulaParameterList.RowFilter;
					FormulaParameterList.RowFilter = AddToDataViewFilter(OldFilter,
						"ParameterCode = '" + ParameterCode + "' and CategoryCode = 'FORMULA'");
					if (FormulaParameterList.Count == 0)
					{
						Category.CheckCatalogResult = "B";
						FormulaParameterValid = false;
					}
					else
					{
						string FormulaIdenitifier = cDBConvert.ToString(CurrentFormula["formula_identifier"]);
						DataView UsedIdentifierRecords = (DataView)Category.GetCheckParameter("Used_Identifier_Records").ParameterValue;
						string UsedIdentifierRecordsOldFilter = UsedIdentifierRecords.RowFilter;
						UsedIdentifierRecords.RowFilter = AddToDataViewFilter(UsedIdentifierRecordsOldFilter,
							"mon_loc_id = '" + Category.CurrentMonLocId + "' and table_cd = 'F' and identifier = '" + FormulaIdenitifier + "'");
						if (UsedIdentifierRecords.Count > 0)
						{
							DataRowView UsedIdentifierRecord = UsedIdentifierRecords[0];
							string TypeOrParameterCode = cDBConvert.ToString(UsedIdentifierRecord["type_or_parameter_cd"]);
							if (ParameterCode != TypeOrParameterCode)
							{
								Category.CheckCatalogResult = "C";
								FormulaParameterValid = false;
							}
						}
						UsedIdentifierRecords.RowFilter = UsedIdentifierRecordsOldFilter;
					}
					FormulaParameterList.RowFilter = OldFilter;
				}

				Category.SetCheckParameter("formula_parameter_valid", FormulaParameterValid, eParameterDataType.Boolean);
			}
			catch (Exception ex)
			{ ReturnVal = Category.CheckEngine.FormatError(ex, "FORMULA8"); }

			return ReturnVal;
		}

		public static string FORMULA9(cCategory Category, ref bool Log) //Formula Code Valid
		{
			string ReturnVal = "";

			try
			{
				bool FormulaParameterValid = (bool)Category.GetCheckParameter("Formula_Parameter_Valid").ParameterValue;

				//Category.SetCheckParameter("Formula_Code_Valid", null, ParameterTypes.BOOLEAN);

				if (FormulaParameterValid)
				{
					Category.SetCheckParameter("O2_component_required", false, eParameterDataType.Boolean);
					Category.SetCheckParameter("moisture_method_required", false, eParameterDataType.Boolean);
					Category.SetCheckParameter("Formula_Code_Valid", true, eParameterDataType.Boolean);

					DataRowView CurrentFormula = (DataRowView)Category.GetCheckParameter("Current_Formula").ParameterValue;
					string FormulaIdentifier = cDBConvert.ToString(CurrentFormula["Formula_identifier"]);

					bool FormulaCodeValid = true;

					if (CurrentFormula["equation_Cd"] == DBNull.Value)
					{
						if (CurrentFormula["formula_equation"] == DBNull.Value)
							Category.CheckCatalogResult = "A";
					}
					else
					{
						string FormulaCode = cDBConvert.ToString(CurrentFormula["equation_Cd"]);
						DataView FormulaCodeRecords = (DataView)Category.GetCheckParameter("Formula_code_lookup_table").ParameterValue;
						//DataView UsedIdentifierRecordsByLocation= (DataView)Category.GetCheckParameter("Used_Identifier_Records_By_Location").ParameterValue;
						DataView UsedIdentifierRecords = (DataView)Category.GetCheckParameter("Used_Identifier_Records").ParameterValue;


						bool LookupFound = false;

						foreach (DataRowView drFormulaCode in FormulaCodeRecords)
						{
							if ((string)drFormulaCode["equation_cd"] == FormulaCode)
							{
								LookupFound = true;
								int MoistureIndicator = cDBConvert.ToInteger(drFormulaCode["moisture_ind"]);

								if (MoistureIndicator == 1)
									Category.SetCheckParameter("moisture_method_required", true, eParameterDataType.Boolean);

								string ParameterCode = cDBConvert.ToString(CurrentFormula["Parameter_cd"]);
								DataView FormulaParameterAndComponentTypeAndBasisToFormulaCodeRecords = (DataView)Category.GetCheckParameter("Formula_Parameter_And_Component_Type_And_Basis_To_Formula_Code_Cross_Check_Table").ParameterValue;

								string OldFilter = FormulaParameterAndComponentTypeAndBasisToFormulaCodeRecords.RowFilter;

								FormulaParameterAndComponentTypeAndBasisToFormulaCodeRecords.RowFilter = AddToDataViewFilter(OldFilter,
								  " ParameterCode = '" + ParameterCode + "' and FormulaCode = '" + FormulaCode + "' ");

								if (FormulaParameterAndComponentTypeAndBasisToFormulaCodeRecords.Count == 0)
								{
									Category.CheckCatalogResult = "C";
									FormulaCodeValid = false;
								}
								else if (FormulaParameterAndComponentTypeAndBasisToFormulaCodeRecords.Count > 0)
								{
									DataRowView FormulaParameterAndComponentTypeAndBasisToFormulaCodeRecord = FormulaParameterAndComponentTypeAndBasisToFormulaCodeRecords[0];
									string ComponentTypeAndBasis = cDBConvert.ToString(FormulaParameterAndComponentTypeAndBasisToFormulaCodeRecord["ComponentTypeAndBasis"]);
									if (ComponentTypeAndBasis.Length >= 2 && ComponentTypeAndBasis.Substring(0, 2) == "O2")
										Category.SetCheckParameter("O2_component_required", true, eParameterDataType.Boolean);

									string UsedIdentifierRecordsOldFilter = UsedIdentifierRecords.RowFilter;
									UsedIdentifierRecords.RowFilter = AddToDataViewFilter(UsedIdentifierRecordsOldFilter,
										"table_cd = 'F' and identifier = '" + FormulaIdentifier + "'");
									if (UsedIdentifierRecords.Count > 0)
									{
										DataRowView UsedIdentifierRecord = UsedIdentifierRecords[0];
										if (UsedIdentifierRecord["formula_or_basis_cd"] != DBNull.Value)
										{
											string FormulaOrBasisCd = cDBConvert.ToString(UsedIdentifierRecord["formula_or_basis_cd"]);
											if (FormulaCode != FormulaOrBasisCd)
												Category.CheckCatalogResult = "D";
										}
									}
									UsedIdentifierRecords.RowFilter = UsedIdentifierRecordsOldFilter;
								}

								FormulaParameterAndComponentTypeAndBasisToFormulaCodeRecords.RowFilter = OldFilter;
								break;
							}
						}
						if (!LookupFound)
						{
							Category.CheckCatalogResult = "B";
							FormulaCodeValid = false;
						}
					}
					Category.SetCheckParameter("Formula_Code_Valid", FormulaCodeValid, eParameterDataType.Boolean);
				}
				else
					Log = false;
			}
			catch (Exception ex)
			{ ReturnVal = Category.CheckEngine.FormatError(ex, "FORMULA9"); }

			return ReturnVal;
		}

		public static string FORMULA10(cCategory Category, ref bool Log) //Formula Text Valid
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentFormula = (DataRowView)Category.GetCheckParameter("Current_Formula").ParameterValue;

				if (CurrentFormula["equation_cd"] == DBNull.Value && CurrentFormula["formula_equation"] == DBNull.Value)
					Category.CheckCatalogResult = "A";
			}
			catch (Exception ex)
			{ ReturnVal = Category.CheckEngine.FormatError(ex, "FORMULA10"); }

			return ReturnVal;
		}

		public static string FORMULA11(cCategory Category, ref bool Log) //Heat Input Apportionment/Summary Formula Valid
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentFormula = (DataRowView)Category.GetCheckParameter("Current_Formula").ParameterValue;
				string FormulaCode = cDBConvert.ToString(CurrentFormula["equation_cd"]);
				string ParameterCode = cDBConvert.ToString(CurrentFormula["parameter_cd"]);

				if (ParameterCode == "HI" && FormulaCode.InList("F-21A,F-21B,F-21C,F-21D,F-25"))
				{
					DateTime FormulaEvaluationBeginDate = cDBConvert.ToDate(Category.GetCheckParameter("Formula_Evaluation_Begin_Date").ParameterValue, DateTypes.START);
					DateTime FormulaEvaluationEndDate = cDBConvert.ToDate(Category.GetCheckParameter("Formula_Evaluation_End_Date").ParameterValue, DateTypes.END);
					int FormulaEvaluationBeginHour = cDBConvert.ToHour(Category.GetCheckParameter("Formula_Evaluation_Begin_Hour").ParameterValue, DateTypes.START);
					int FormulaEvaluationEndHour = cDBConvert.ToHour(Category.GetCheckParameter("Formula_Evaluation_End_Hour").ParameterValue, DateTypes.END);
					DataView FacilityMethodRecords = (DataView)Category.GetCheckParameter("Facility_Method_Records").ParameterValue;

					string OldFilter = FacilityMethodRecords.RowFilter;

					FacilityMethodRecords.RowFilter = AddToDataViewFilter(OldFilter, " mon_loc_id = '" + cDBConvert.ToString(CurrentFormula["mon_loc_id"]) +
																			"' and parameter_cd = 'HI' and method_cd like '%CALC%' ");
					FacilityMethodRecords.RowFilter = AddEvaluationDateHourRangeToDataViewFilter(FacilityMethodRecords.RowFilter,
					  FormulaEvaluationBeginDate, FormulaEvaluationEndDate, FormulaEvaluationBeginHour, FormulaEvaluationEndHour, false, true);

					if (FacilityMethodRecords.Count == 0)
						Category.CheckCatalogResult = "A";
					else
					{
						string LocationType = cDBConvert.ToString(Category.GetCheckParameter("Location_Type").ParameterValue);

						if ((FormulaCode == "F-25" && LocationType != "CS") ||
							(FormulaCode.StartsWith("F-21") && !LocationType.InList("US,UP,UB")))
							Category.CheckCatalogResult = "B";
						else
						{
							if (FormulaCode.InList("F-21A,F-21B"))
							{
								string UOMCode = "";
								DataView LoadRecords = (DataView)Category.GetCheckParameter("Load_Records").ParameterValue;
								string LROldFilter = LoadRecords.RowFilter;

								if (FormulaCode == "F-21A")
									UOMCode = "('KLBHR', 'MMBTUHR')";
								else if (FormulaCode == "F-21B")
									UOMCode = "('MW')";

								LoadRecords.RowFilter = AddToDataViewFilter(LROldFilter, " MAX_LOAD_UOM_CD in " + UOMCode + " ");

								LoadRecords.RowFilter = AddEvaluationDateRangeToDataViewFilter(LoadRecords.RowFilter,
								  FormulaEvaluationBeginDate, FormulaEvaluationEndDate, false, true, false);

								if (LoadRecords.Count != 0)
									Category.CheckCatalogResult = "C";

								LoadRecords.RowFilter = LROldFilter;
							}
							else if (FormulaCode == "F-21D")
							{
								DataView UnitStackConfigurationRecords = (DataView)Category.GetCheckParameter("Unit_Stack_Configuration_Records").ParameterValue;

								string USOldFilter = UnitStackConfigurationRecords.RowFilter;
								UnitStackConfigurationRecords.RowFilter = AddToDataViewFilter(USOldFilter, " mon_loc_Id = '" + cDBConvert.ToString(CurrentFormula["mon_loc_id"]) +
																								"' and stack_name like 'CP%' ");

								UnitStackConfigurationRecords.RowFilter = AddEvaluationDateRangeToDataViewFilter(UnitStackConfigurationRecords.RowFilter,
								  FormulaEvaluationBeginDate, FormulaEvaluationEndDate, false, true, false);

								if (UnitStackConfigurationRecords.Count == 0)
									Category.CheckCatalogResult = "D";
								else
								{
									bool PipeMethodFound = false;

									foreach (DataRowView drvUnitStackConfiguration in UnitStackConfigurationRecords)
									{
										FacilityMethodRecords.RowFilter = "";
										string MethodRecordsFilter = FacilityMethodRecords.RowFilter;

										FacilityMethodRecords.RowFilter = AddToDataViewFilter(MethodRecordsFilter,
										  " parameter_cd = 'HI' and method_cd = 'AD' " +
										  " and mon_loc_id = '" + cDBConvert.ToString(drvUnitStackConfiguration["stack_pipe_mon_loc_id"]) + "' ");

										FacilityMethodRecords.RowFilter = AddEvaluationDateHourRangeToDataViewFilter(FacilityMethodRecords.RowFilter,
										  FormulaEvaluationBeginDate, FormulaEvaluationEndDate, FormulaEvaluationBeginHour, FormulaEvaluationEndHour, false, true);

										if (FacilityMethodRecords.Count != 0)
											PipeMethodFound = true;

										FacilityMethodRecords.RowFilter = MethodRecordsFilter;
									}

									if (!PipeMethodFound)
										Category.CheckCatalogResult = "D";
								}

								UnitStackConfigurationRecords.RowFilter = USOldFilter;
							}
						}
					}

					FacilityMethodRecords.RowFilter = OldFilter;

				}
				else
					Log = false;
			}
			catch (Exception ex)
			{ ReturnVal = Category.CheckEngine.FormatError(ex, "FORMULA11"); }

			return ReturnVal;
		}

		public static string FORMULA12(cCategory Category, ref bool Log) //Formula Parameter and Code Consisent with Method
		{
			string ReturnVal = "";

			try
			{
				if (MpParameters.FormulaParameterValid.Default(false) && MpParameters.FormulaCodeValid.Default(false))
				{
					DataView formulaToRequiredMethodRows
				= cRowFilter.FindRows(MpParameters.FormulaToRequiredMethodCrosscheck.SourceView,
											 new cFilterCondition[] 
                                       {
                                         new cFilterCondition("FormulaCode", MpParameters.CurrentFormula.EquationCd, eFilterConditionStringCompare.Equals)
                                       });
					if (formulaToRequiredMethodRows.Count > 0)
					{
						// find method records that match the found crosscheck records (not actually using the records, just checking for existence)
						bool methodRecordsFound = false;
						string appropriateMethod = string.Empty;

						foreach (DataRowView formulaToMethodRow in formulaToRequiredMethodRows)
						{
							DataView methodRecordsMatch =
									  cRowFilter.FindActiveRows(MpParameters.MethodRecords.SourceView,
														MpParameters.FormulaEvaluationBeginDate.Default(DateTime.MinValue), MpParameters.FormulaEvaluationBeginHour.Default(0),
														MpParameters.FormulaEvaluationEndDate.Default(DateTime.MaxValue), MpParameters.FormulaEvaluationEndHour.Default(0),
														"BEGIN_DATE", "BEGIN_HOUR",
														"END_DATE", "END_HOUR",
														false,
												 new cFilterCondition[] 
                                       {
                                         new cFilterCondition("MON_LOC_ID", MpParameters.CurrentFormula.MonLocId, eFilterConditionStringCompare.Equals),
										 new cFilterCondition("PARAMETER_CD", formulaToMethodRow["MethodParameter"].AsString(), eFilterConditionStringCompare.Equals),
										 new cFilterCondition("METHOD_CD", formulaToMethodRow["MethodCode"].AsString(), eFilterConditionStringCompare.Equals)
                                       });
							if (methodRecordsMatch.Count > 0)
							{
								methodRecordsFound = true;
							}
							else
							{
								appropriateMethod = appropriateMethod.ListAdd("(" + formulaToMethodRow["MethodParameter"].ToString() + "/" + formulaToMethodRow["MethodCode"].ToString() + ")"); 
							}
						}
						if (methodRecordsFound == false)
						{
							//Set AppropriateMethodForFormula to the list of MethodParameter/MethodCode in the located crosscheck records.
							//Parameter1/Method1), (Parameter2/Method2) ...
							MpParameters.AppropriateMethodForFormula = appropriateMethod;
							Category.CheckCatalogResult = "A";
						}
						else
						{
							//Locate records in the FormulaToRequiredUnitFuelCrosscheck
							DataView formulaToUnitFuelRows = cRowFilter.FindRows(MpParameters.FormulaToRequiredUnitFuelCrosscheck.SourceView,
									new cFilterCondition[] 
                                       {new cFilterCondition("FormulaCode", MpParameters.CurrentFormula.EquationCd, eFilterConditionStringCompare.Equals)
									   });
							if (formulaToUnitFuelRows.Count > 0)
							{
								bool fuelRecordsFound = false;

								foreach (DataRowView formulaUnitFuelRow in formulaToUnitFuelRows)
								{
									//	Locate records in LocationFuelRecords (datetime begin/end dates)
									DataView locationFuelMatch = cRowFilter.FindActiveRows(MpParameters.LocationFuelRecords.SourceView,
															MpParameters.FormulaEvaluationBeginDate.Default(DateTime.MinValue).AddHours(MpParameters.FormulaEvaluationBeginHour.Default(0)),
															MpParameters.FormulaEvaluationEndDate.Default(DateTime.MaxValue).AddHours(MpParameters.FormulaEvaluationEndHour.Default(0)),
															"BEGIN_DATE", 
															"END_DATE",
															false,
													 new cFilterCondition[] 
                                       {new cFilterCondition("FUEL_CD", formulaUnitFuelRow["UnitFuelCode"].ToString(), eFilterConditionStringCompare.Equals)
									   });

									if (locationFuelMatch.Count > 0)
									{
										fuelRecordsFound = true;
									}
								}
								if (fuelRecordsFound == false)
								{
									Category.CheckCatalogResult = "B";
								}
							}
						}
					}
				}
			}

			catch (Exception ex)
			{ ReturnVal = Category.CheckEngine.FormatError(ex, "FORMULA12"); }

			return ReturnVal;
		}

		public static string FORMULA13(cCategory Category, ref bool Log) //Formula Parameter and Code Consisent with System
		{
			string ReturnVal = "";

			try
			{
				bool FormulaCodeValid = (bool)Category.GetCheckParameter("Formula_Code_Valid").ParameterValue;
				bool FormulaParameterValid = (bool)Category.GetCheckParameter("Formula_Parameter_Valid").ParameterValue;

				if (FormulaCodeValid && FormulaParameterValid)
				{
					DataRowView CurrentFormula = (DataRowView)Category.GetCheckParameter("Current_Formula").ParameterValue;
					string FormulaCode = cDBConvert.ToString(CurrentFormula["equation_cd"]);
					string ParameterCode = cDBConvert.ToString(CurrentFormula["parameter_cd"]);

					DataView MonitorSystemRecords = (DataView)Category.GetCheckParameter("Monitor_System_Records").ParameterValue;
					DataView SystemComponentRecords = (DataView)Category.GetCheckParameter("Location_System_Component_Records").ParameterValue;
					DataView MethodRecords = Category.GetCheckParameter("Method_Records").ValueAsDataView();
					DataView FormulaParameterAndComponentTypeAndBasisToFormulaCodeRecords = (DataView)Category.GetCheckParameter("Formula_Parameter_And_Component_Type_And_Basis_To_Formula_Code_Cross_Check_Table").ParameterValue;
					string OldFormulaParameterAndComponentTypeAndBasisToFormulaCodeFilter = FormulaParameterAndComponentTypeAndBasisToFormulaCodeRecords.RowFilter;
					string OldFilter = "";
					string OldSystemComponentFilter = "";
					string OldMethodFilter = "";

					DateTime FormulaEvaluationBeginDate = cDBConvert.ToDate(Category.GetCheckParameter("Formula_Evaluation_Begin_Date").ParameterValue, DateTypes.START);
					DateTime FormulaEvaluationEndDate = cDBConvert.ToDate(Category.GetCheckParameter("Formula_Evaluation_End_Date").ParameterValue, DateTypes.END);
					int FormulaEvaluationBeginHour = cDBConvert.ToHour(Category.GetCheckParameter("Formula_Evaluation_Begin_Hour").ParameterValue, DateTypes.START);
					int FormulaEvaluationEndHour = cDBConvert.ToHour(Category.GetCheckParameter("Formula_Evaluation_End_Hour").ParameterValue, DateTypes.END);

					bool SystemFound = false;
					string AppropriateSystemOrComponent = "";

					OldFilter = MonitorSystemRecords.RowFilter;
					OldMethodFilter = MethodRecords.RowFilter;

					DataView LocationFuelRecords = (DataView)Category.GetCheckParameter("Location_Fuel_Records").ParameterValue;
					string OldFuelFilter = LocationFuelRecords.RowFilter;


					if (FormulaCode.InList("D-12,D-15A,G-4A"))
					{
						MonitorSystemRecords.RowFilter = AddToDataViewFilter(OldFilter, " sys_type_cd in ('OILM','OILV','GAS')and sys_designation_cd = 'P' ");

						MonitorSystemRecords.RowFilter = AddEvaluationDateHourRangeToDataViewFilter(MonitorSystemRecords.RowFilter, FormulaEvaluationBeginDate,
						  FormulaEvaluationEndDate, FormulaEvaluationBeginHour, FormulaEvaluationEndHour, false, true);

						if (MonitorSystemRecords.Count < 2 ||
							CheckForDuplicateRecords(Category, MonitorSystemRecords,
													 FormulaEvaluationBeginDate, FormulaEvaluationEndDate,
													 FormulaEvaluationBeginHour, FormulaEvaluationEndHour))
						{
							DataView UnitMonitorSystemRecords = (DataView)Category.GetCheckParameter("Unit_Monitor_System_Records").ParameterValue;
							string OldFilter2 = UnitMonitorSystemRecords.RowFilter;
							UnitMonitorSystemRecords.RowFilter = AddToDataViewFilter(OldFilter2, " sys_type_cd in ('OILM','OILV','GAS') and sys_designation_cd = 'P' and (stack_name is null or stack_name like 'CP%')");
							if (UnitMonitorSystemRecords.Count < 2 ||
								CheckForDuplicateRecords(Category, UnitMonitorSystemRecords,
														 FormulaEvaluationBeginDate, FormulaEvaluationEndDate,
														 FormulaEvaluationBeginHour, FormulaEvaluationEndHour))
							{
								if (MonitorSystemRecords.Count >= 1)
								{
									LocationFuelRecords.RowFilter = AddToDataViewFilter(OldFuelFilter, " fuel_group_cd in ('OIL','GAS') and indicator_cd in ('I', 'E')");
									LocationFuelRecords.RowFilter = AddEvaluationDateRangeToDataViewFilter(LocationFuelRecords.RowFilter,
									  FormulaEvaluationBeginDate, FormulaEvaluationEndDate, false, true, false);
									if (LocationFuelRecords.Count == 0)
									{
										if (FormulaCode == "D-15A")
										{
											DataView formulaRecords = Category.GetCheckParameter("Formula_Records").ValueAsDataView();
											DataView formulaView
											  = cRowFilter.FindActiveRows(formulaRecords,
																		  FormulaEvaluationBeginDate, FormulaEvaluationBeginHour,
																		  FormulaEvaluationEndDate, FormulaEvaluationEndHour,
																		  new cFilterCondition[] { new cFilterCondition("EQUATION_CD", "F-21A,F-21B,F-21D", eFilterConditionStringCompare.InList) });

											bool noOverlap;
											{
												if (formulaView.Count == 0)
													noOverlap = true;
												else
												{
													noOverlap = false;

													foreach (DataRowView formulaRow in formulaView)
													{
														DateTime formulaBeganDate = formulaRow["BEGIN_DATE"].AsDateTime().Default(DateTypes.START);
														int formulaBeganHour = formulaRow["BEGIN_HOUR"].AsHour().Default(DateTypes.START).AsInteger();
														DateTime formulaEndedDate = formulaRow["END_DATE"].AsDateTime().Default(DateTypes.END);
														int formulaEndedHour = formulaRow["END_HOUR"].AsHour().Default(DateTypes.END).AsInteger();

														DateTime testBeganDate;
														int testBeganHour;
														{
															if (formulaBeganDate < FormulaEvaluationBeginDate)
															{ testBeganDate = FormulaEvaluationBeginDate; testBeganHour = 0; }
															else
															{ testBeganDate = formulaBeganDate; testBeganHour = formulaBeganHour; }
														}

														DateTime testEndedDate;
														int testEndedHour;
														{
															if (formulaEndedDate > FormulaEvaluationEndDate)
															{ testEndedDate = FormulaEvaluationEndDate; testEndedHour = 23; }
															else
															{ testEndedDate = formulaEndedDate; testEndedHour = formulaEndedHour; }
														}

														DataView monitorSystemView
														  = cRowFilter.FindActiveRows(MonitorSystemRecords,
																					  testBeganDate, testBeganHour,
																					  testEndedDate, testEndedHour);

														// Determines whether individual formula overlaps any monitor system
														if (monitorSystemView.Count == 0)
															noOverlap = true;
													}
												}
											}

											if (noOverlap)
											{
												Category.CheckCatalogResult = "A";
												AppropriateSystemOrComponent = "oil or gas";
											}
										}
										else
										{
											Category.CheckCatalogResult = "A";
											AppropriateSystemOrComponent = "oil or gas";
										}
									}
								}
								else
								{
									Category.CheckCatalogResult = "A";
									AppropriateSystemOrComponent = "oil or gas";
								}
							}
							UnitMonitorSystemRecords.RowFilter = OldFilter2;
						}
					}
					else if (FormulaCode == "E-2")
					{
						MonitorSystemRecords.RowFilter = AddToDataViewFilter(OldFilter, " sys_type_cd = 'NOXE' and fuel_cd <> 'MIX'");

						MonitorSystemRecords.RowFilter = AddEvaluationDateHourRangeToDataViewFilter(MonitorSystemRecords.RowFilter, FormulaEvaluationBeginDate,
						  FormulaEvaluationEndDate, FormulaEvaluationBeginHour, FormulaEvaluationEndHour, false, true);

						if (MonitorSystemRecords.Count < 2 || CheckForDuplicateRecords(Category, MonitorSystemRecords, FormulaEvaluationBeginDate,
							  FormulaEvaluationEndDate, FormulaEvaluationBeginHour, FormulaEvaluationEndHour))
						{
							Category.CheckCatalogResult = "A";
							AppropriateSystemOrComponent = "NOXE";
						}
					}
					else if (FormulaCode == "D-3")
					{
						MonitorSystemRecords.RowFilter = AddToDataViewFilter(OldFilter, " sys_type_cd = 'OILV' ");

						MonitorSystemRecords.RowFilter = AddEvaluationDateHourRangeToDataViewFilter(MonitorSystemRecords.RowFilter, FormulaEvaluationBeginDate,
						  FormulaEvaluationEndDate, FormulaEvaluationBeginHour, FormulaEvaluationEndHour, false, true);

						if (MonitorSystemRecords.Count == 0)
						{
							Category.CheckCatalogResult = "B";
							AppropriateSystemOrComponent = "OILV";
						}
					}
					else if (FormulaCode == "N-GAS")
					{
						MonitorSystemRecords.RowFilter = AddToDataViewFilter(OldFilter, " (sys_type_cd = 'GAS' or sys_type_cd = 'LTGS') ");

						MonitorSystemRecords.RowFilter = AddEvaluationDateHourRangeToDataViewFilter(MonitorSystemRecords.RowFilter, FormulaEvaluationBeginDate,
						  FormulaEvaluationEndDate, FormulaEvaluationBeginHour, FormulaEvaluationEndHour, false, true);

						foreach (DataRowView drvMonitorSystem in MonitorSystemRecords)
						{
							if (SystemFound)
								break;

							OldSystemComponentFilter = SystemComponentRecords.RowFilter;

							SystemComponentRecords.RowFilter = AddToDataViewFilter(OldSystemComponentFilter,
							  " mon_sys_id = '" + cDBConvert.ToString(drvMonitorSystem["mon_sys_id"]) + "' and component_type_cd in ('GFFM','BGFF') ");

							SystemComponentRecords.RowFilter = AddEvaluationDateHourRangeToDataViewFilter(SystemComponentRecords.RowFilter, FormulaEvaluationBeginDate,
							  FormulaEvaluationEndDate, FormulaEvaluationBeginHour, FormulaEvaluationEndHour, false, true);

							if (SystemComponentRecords.Count >= 2 &&
								!CheckForDuplicateRecords(Category, SystemComponentRecords, FormulaEvaluationBeginDate, FormulaEvaluationEndDate))
							{
								SystemFound = true;
							}

							SystemComponentRecords.RowFilter = OldSystemComponentFilter;
						}

						if (!SystemFound)
						{
							Category.CheckCatalogResult = "C";
							AppropriateSystemOrComponent = "GAS or LTGS";
						}
					}
					else if (FormulaCode == "N-OIL")
					{
						MonitorSystemRecords.RowFilter = AddToDataViewFilter(OldFilter, " sys_type_cd in ('OILM','OILV') ");

						MonitorSystemRecords.RowFilter = AddEvaluationDateHourRangeToDataViewFilter(MonitorSystemRecords.RowFilter, FormulaEvaluationBeginDate,
						  FormulaEvaluationEndDate, FormulaEvaluationBeginHour, FormulaEvaluationEndHour, false, true);

						foreach (DataRowView drvMonitorSystem in MonitorSystemRecords)
						{
							if (SystemFound)
								break;

							OldSystemComponentFilter = SystemComponentRecords.RowFilter;

							SystemComponentRecords.RowFilter = AddToDataViewFilter(OldSystemComponentFilter,
							  " mon_sys_id = '" + cDBConvert.ToString(drvMonitorSystem["mon_sys_id"]) + "' and component_type_cd in ('OFFM','BOFF') ");

							SystemComponentRecords.RowFilter = AddEvaluationDateHourRangeToDataViewFilter(SystemComponentRecords.RowFilter, FormulaEvaluationBeginDate,
							  FormulaEvaluationEndDate, FormulaEvaluationBeginHour, FormulaEvaluationEndHour, false, true);

							if (SystemComponentRecords.Count >= 2 &&
								!CheckForDuplicateRecords(Category, SystemComponentRecords, FormulaEvaluationBeginDate, FormulaEvaluationEndDate))
							{
								SystemFound = true;
							}

							SystemComponentRecords.RowFilter = OldSystemComponentFilter;
						}

						if (!SystemFound)
						{
							Category.CheckCatalogResult = "C";
							AppropriateSystemOrComponent = "OILM or OILV";
						}
					}
					else if (FormulaCode == "X-FL" || FormulaCode == "T-FL")
					{
						MonitorSystemRecords.RowFilter = AddToDataViewFilter(OldFilter, " sys_type_cd = 'FLOW' ");

						MonitorSystemRecords.RowFilter = AddEvaluationDateHourRangeToDataViewFilter(MonitorSystemRecords.RowFilter, FormulaEvaluationBeginDate,
						  FormulaEvaluationEndDate, FormulaEvaluationBeginHour, FormulaEvaluationEndHour, false, true);

						foreach (DataRowView drvMonitorSystem in MonitorSystemRecords)
						{
							if (SystemFound)
								break;

							OldSystemComponentFilter = SystemComponentRecords.RowFilter;

							SystemComponentRecords.RowFilter = AddToDataViewFilter(OldSystemComponentFilter,
							  " mon_sys_id = '" + cDBConvert.ToString(drvMonitorSystem["mon_sys_id"]) + "' and component_type_cd = 'FLOW' ");

							SystemComponentRecords.RowFilter = AddEvaluationDateHourRangeToDataViewFilter(SystemComponentRecords.RowFilter, FormulaEvaluationBeginDate,
							  FormulaEvaluationEndDate, FormulaEvaluationBeginHour, FormulaEvaluationEndHour, false, true);

							if (SystemComponentRecords.Count >= 2 &&
								!CheckForDuplicateRecords(Category, SystemComponentRecords, FormulaEvaluationBeginDate, FormulaEvaluationEndDate))
							{
								SystemFound = true;
							}

							SystemComponentRecords.RowFilter = OldSystemComponentFilter;
						}

						if (!SystemFound)
						{
							Category.CheckCatalogResult = "C";
							AppropriateSystemOrComponent = "FLOW";
						}
					}
					else if (ParameterCode == "H2O")
					{
						MonitorSystemRecords.RowFilter = AddToDataViewFilter(OldFilter, " sys_type_cd = 'H2O' ");

						MonitorSystemRecords.RowFilter = AddEvaluationDateHourRangeToDataViewFilter(MonitorSystemRecords.RowFilter, FormulaEvaluationBeginDate,
						  FormulaEvaluationEndDate, FormulaEvaluationBeginHour, FormulaEvaluationEndHour, false, true);

						if (MonitorSystemRecords.Count == 0)
						{
							Category.CheckCatalogResult = "B";
							AppropriateSystemOrComponent = "H2O";
						}
					}

					else
					{

						FormulaParameterAndComponentTypeAndBasisToFormulaCodeRecords.RowFilter = AddToDataViewFilter(FormulaParameterAndComponentTypeAndBasisToFormulaCodeRecords.RowFilter,
						  " FormulaCode = '" + FormulaCode + "' and ParameterCode = '" + ParameterCode + "' and ComponentTypeAndBasis is not null ");

						if (FormulaParameterAndComponentTypeAndBasisToFormulaCodeRecords.Count > 0)
						{
							string ComponentTypeAndBasis = "";

							if (ParameterCode == "NOXR")
							{
								MonitorSystemRecords.RowFilter = AddToDataViewFilter(OldFilter, " sys_type_cd = 'NOX' ");

								MonitorSystemRecords.RowFilter = AddEvaluationDateHourRangeToDataViewFilter(MonitorSystemRecords.RowFilter, FormulaEvaluationBeginDate,
								  FormulaEvaluationEndDate, FormulaEvaluationBeginHour, FormulaEvaluationEndHour, false, true);

								string CrossCheckOldFilter = FormulaParameterAndComponentTypeAndBasisToFormulaCodeRecords.RowFilter;
								FormulaParameterAndComponentTypeAndBasisToFormulaCodeRecords.RowFilter = AddToDataViewFilter(CrossCheckOldFilter, "ComponentTypeAndBasis < > 'O2B'");

								if (FormulaParameterAndComponentTypeAndBasisToFormulaCodeRecords.Count > 0) //!O2B xcheck recs
								{
									//if (MonitorSystemRecords.Count > 0)
									//    SystemFound = true;
									foreach (DataRowView drvFormulaParameterAndComponentTypeAndBasisToFormulaCode in FormulaParameterAndComponentTypeAndBasisToFormulaCodeRecords)
									{
										SystemFound = false;
										foreach (DataRowView drvMonitorSystem in MonitorSystemRecords)
										{
											//if (!SystemFound)
											//    break;
											if (SystemFound)//must be false b4 gets her if coming from outer loop :. need to reset somewhere
												break;

											OldSystemComponentFilter = SystemComponentRecords.RowFilter;

											ComponentTypeAndBasis = cDBConvert.ToString(drvFormulaParameterAndComponentTypeAndBasisToFormulaCode["ComponentTypeAndBasis"]);

											SystemComponentRecords.RowFilter = AddToDataViewFilter(OldSystemComponentFilter,
											  " mon_sys_id = '" + cDBConvert.ToString(drvMonitorSystem["mon_sys_id"]) + "' " +
											  (ComponentTypeAndBasis.Substring(0, 2) == "O2" ?
												" and (component_type_cd + isnull(basis_cd,'') = '" + ComponentTypeAndBasis + "' or component_type_cd + isnull(basis_cd,'') = 'O2B') " :
												" and component_type_cd + isnull(basis_cd,'') = '" + ComponentTypeAndBasis + "' "));

											SystemComponentRecords.RowFilter = AddEvaluationDateHourRangeToDataViewFilter(SystemComponentRecords.RowFilter, FormulaEvaluationBeginDate,
											  FormulaEvaluationEndDate, FormulaEvaluationBeginHour, FormulaEvaluationEndHour, false, true);

											if (SystemComponentRecords.Count == 0)
												SystemFound = false;
											else
												SystemFound = true;

											SystemComponentRecords.RowFilter = OldSystemComponentFilter;
										}
										if (!SystemFound)
											break;

									}

									if (!SystemFound)
									{
										Category.CheckCatalogResult = "B";
										AppropriateSystemOrComponent = "NOX";
									}
								}
							}
							else if (ParameterCode == "SO2R")
							{
								MonitorSystemRecords.RowFilter = AddToDataViewFilter(OldFilter, " sys_type_cd = 'SO2R' ");

								MonitorSystemRecords.RowFilter = AddEvaluationDateHourRangeToDataViewFilter(MonitorSystemRecords.RowFilter, FormulaEvaluationBeginDate,
								  FormulaEvaluationEndDate, FormulaEvaluationBeginHour, FormulaEvaluationEndHour, false, true);

								//if(MonitorSystemRecords.Count > 0)
								//    SystemFound = true;
								foreach (DataRowView drvFormulaParameterAndComponentTypeAndBasisToFormulaCode in FormulaParameterAndComponentTypeAndBasisToFormulaCodeRecords)
								{
									SystemFound = false;
									foreach (DataRowView drvMonitorSystem in MonitorSystemRecords)
									{
										if (SystemFound)
											break;

										OldSystemComponentFilter = SystemComponentRecords.RowFilter;

										ComponentTypeAndBasis = cDBConvert.ToString(drvFormulaParameterAndComponentTypeAndBasisToFormulaCode["ComponentTypeAndBasis"]);

										SystemComponentRecords.RowFilter = AddToDataViewFilter(OldSystemComponentFilter,
										  " mon_sys_id = '" + cDBConvert.ToString(drvMonitorSystem["mon_sys_id"]) + "' " +
										  (ComponentTypeAndBasis.Substring(0, 2) == "O2" ?
											" and (component_type_cd + isnull(basis_cd,'') = '" + ComponentTypeAndBasis + "' or component_type_cd + isnull(basis_cd,'') = 'O2B') " :
											" and component_type_cd + isnull(basis_cd,'') = '" + ComponentTypeAndBasis + "' "));

										SystemComponentRecords.RowFilter = AddEvaluationDateHourRangeToDataViewFilter(SystemComponentRecords.RowFilter, FormulaEvaluationBeginDate,
										  FormulaEvaluationEndDate, FormulaEvaluationBeginHour, FormulaEvaluationEndHour, false, true);

										if (SystemComponentRecords.Count == 0)
											SystemFound = false;
										else
											SystemFound = true;

										SystemComponentRecords.RowFilter = OldSystemComponentFilter;
									}
									if (!SystemFound)
										break;
								}

								if (!SystemFound)
								{
									Category.CheckCatalogResult = "B";
									AppropriateSystemOrComponent = "SO2R";
								}
							}
							else //parameter code != SO2R / NOXR, locate a system component record for the location where the...
							{
								string ComponentTypeAndBasisValues = "";

								foreach (DataRowView drvFormulaParameterAndComponentTypeAndBasisToFormulaCode in FormulaParameterAndComponentTypeAndBasisToFormulaCodeRecords)
								{
									if (SystemFound)
										break;

									OldSystemComponentFilter = SystemComponentRecords.RowFilter;

									SystemComponentRecords.RowFilter = AddToDataViewFilter(OldSystemComponentFilter,
									  " component_type_cd + isnull(basis_cd,'') = '" + cDBConvert.ToString(drvFormulaParameterAndComponentTypeAndBasisToFormulaCode["ComponentTypeAndBasis"]) + "' ");

									SystemComponentRecords.RowFilter = AddEvaluationDateHourRangeToDataViewFilter(SystemComponentRecords.RowFilter, FormulaEvaluationBeginDate,
									  FormulaEvaluationEndDate, FormulaEvaluationBeginHour, FormulaEvaluationEndHour, false, true);

									ComponentTypeAndBasisValues = ComponentTypeAndBasisValues.ListAdd(cDBConvert.ToString(drvFormulaParameterAndComponentTypeAndBasisToFormulaCode["ComponentTypeAndBasis"]));

									if (SystemComponentRecords.Count > 0)
										SystemFound = true;
									else
									{
										if (cDBConvert.ToString(drvFormulaParameterAndComponentTypeAndBasisToFormulaCode["ComponentTypeAndBasis"]).InList("GFFM,BGFF"))
										{
											LocationFuelRecords.RowFilter = AddToDataViewFilter(OldFuelFilter, " fuel_group_cd = 'GAS' and indicator_cd in ('I', 'E')");
											LocationFuelRecords.RowFilter = AddEvaluationDateRangeToDataViewFilter(LocationFuelRecords.RowFilter,
											  FormulaEvaluationBeginDate, FormulaEvaluationEndDate, false, true, false);
											if (LocationFuelRecords.Count > 0)
												SystemFound = true;
										}
										else if (cDBConvert.ToString(drvFormulaParameterAndComponentTypeAndBasisToFormulaCode["ComponentTypeAndBasis"]).InList("OFFM,BOFF"))
										{
											LocationFuelRecords.RowFilter = AddToDataViewFilter(OldFuelFilter, " fuel_group_cd = 'OIL' and indicator_cd in ('I', 'E')");
											LocationFuelRecords.RowFilter = AddEvaluationDateRangeToDataViewFilter(LocationFuelRecords.RowFilter,
											  FormulaEvaluationBeginDate, FormulaEvaluationEndDate, false, true, false);
											if (LocationFuelRecords.Count > 0)
												SystemFound = true;
										}
									}
									SystemComponentRecords.RowFilter = OldSystemComponentFilter;
								}

								if (!SystemFound)
								{
									Category.CheckCatalogResult = "D";
									AppropriateSystemOrComponent = ComponentTypeAndBasisValues;
								}
							}
						}

					}

					Category.SetCheckParameter("appropriate_system_or_component_for_formula", AppropriateSystemOrComponent.FormatList(true), eParameterDataType.String);

					MonitorSystemRecords.RowFilter = OldFilter;
					MethodRecords.RowFilter = OldMethodFilter;
					FormulaParameterAndComponentTypeAndBasisToFormulaCodeRecords.RowFilter = OldFormulaParameterAndComponentTypeAndBasisToFormulaCodeFilter;
					LocationFuelRecords.RowFilter = OldFuelFilter;

				}
				else
					Log = false;
			}
			catch (Exception ex)
			{ ReturnVal += Category.CheckEngine.FormatError(ex, "FORMULA13"); }

			return ReturnVal;
		}

		public static string FORMULA14(cCategory Category, ref bool Log) //Formula Code Consistent with Fuel
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentFormula = (DataRowView)Category.GetCheckParameter("Current_Formula").ParameterValue;
				string ParameterCode = cDBConvert.ToString(CurrentFormula["Parameter_cd"]);
				string FormulaCode = cDBConvert.ToString(CurrentFormula["equation_cd"]);

				if (ParameterCode == "SO2" && FormulaCode.InList("F-23,D-5"))
				{
					DataView LocationFuelRecords = (DataView)Category.GetCheckParameter("Location_Fuel_Records").ParameterValue;
					DateTime FormulaEvaluationBeginDate = (DateTime)Category.GetCheckParameter("Formula_Evaluation_Begin_Date").ParameterValue;
					DateTime FormulaEvaluationEndDate = (DateTime)Category.GetCheckParameter("Formula_Evaluation_End_Date").ParameterValue;

					string OldFilter = LocationFuelRecords.RowFilter;

					if (FormulaCode == "D-5")
					{

						LocationFuelRecords.RowFilter = AddToDataViewFilter(OldFilter, " fuel_cd in ('PNG','NNG') ");

						LocationFuelRecords.RowFilter = AddEvaluationDateRangeToDataViewFilter(LocationFuelRecords.RowFilter,
						  FormulaEvaluationBeginDate, FormulaEvaluationEndDate, false, true, false);

						if (LocationFuelRecords.Count == 0)
							Category.CheckCatalogResult = "A";
					}
					else if (FormulaCode == "F-23")
					{

						LocationFuelRecords.RowFilter = AddToDataViewFilter(OldFilter, " fuel_group_cd in ('GAS','OIL') ");

						LocationFuelRecords.RowFilter = AddEvaluationDateRangeToDataViewFilter(LocationFuelRecords.RowFilter,
						  FormulaEvaluationBeginDate, FormulaEvaluationEndDate, false, true, false);

						if (LocationFuelRecords.Count == 0)
							Category.CheckCatalogResult = "A";
					}

					LocationFuelRecords.RowFilter = OldFilter;
				}
				else
					Log = false;
			}
			catch (Exception ex)
			{ ReturnVal = Category.CheckEngine.FormatError(ex, "FORMULA14"); }

			return ReturnVal;
		}

		public static string FORMULA15(cCategory Category, ref bool Log) //Required H2O Method Reported for Formula
		{
			string ReturnVal = "";

			try
			{
				bool ParameterCodeValid = (bool)Category.GetCheckParameter("formula_Parameter_valid").ParameterValue;
				bool FormulaCodeValid = (bool)Category.GetCheckParameter("formula_code_valid").ParameterValue;
				bool MoistureMethodRequired = (bool)Category.GetCheckParameter("moisture_method_required").ParameterValue;

				if (ParameterCodeValid && FormulaCodeValid)
				{
					Category.SetCheckParameter("Moisture_Default_Required", false, eParameterDataType.Boolean);

					if (MoistureMethodRequired)
					{
						DataView MethodRecords = (DataView)Category.GetCheckParameter("Method_Records").ParameterValue;
						DateTime FormulaEvaluationBeginDate = (DateTime)Category.GetCheckParameter("Formula_Evaluation_Begin_Date").ParameterValue;
						DateTime FormulaEvaluationEndDate = (DateTime)Category.GetCheckParameter("Formula_Evaluation_End_Date").ParameterValue;
						int FormulaEvaluationBeginHour = cDBConvert.ToHour(Category.GetCheckParameter("Formula_Evaluation_Begin_Hour").ParameterValue, DateTypes.START);
						int FormulaEvaluationEndHour = cDBConvert.ToHour(Category.GetCheckParameter("Formula_Evaluation_End_Hour").ParameterValue, DateTypes.END);

						string OldFilter = MethodRecords.RowFilter;
						int MethodCount = 0;

						MethodRecords.RowFilter = AddToDataViewFilter(OldFilter, " parameter_cd = 'H2O' ");
						MethodRecords.RowFilter = AddEvaluationDateHourRangeToDataViewFilter(MethodRecords.RowFilter, FormulaEvaluationBeginDate, FormulaEvaluationEndDate,
																							FormulaEvaluationBeginHour, FormulaEvaluationEndHour, false, true);

						if (MethodRecords.Count == 0)
							Category.CheckCatalogResult = "A";
						else
						{
							string tempFilter = MethodRecords.RowFilter;
							MethodRecords.RowFilter = AddToDataViewFilter(tempFilter, "method_cd in ('MMS','MWD','MTB')");
							if (MethodRecords.Count > 0)
								Category.SetCheckParameter("Moisture_Default_Required", true, eParameterDataType.Boolean);
							MethodRecords.RowFilter = tempFilter;
							if (!CheckForHourRangeCovered(Category, MethodRecords, FormulaEvaluationBeginDate, FormulaEvaluationBeginHour,
												  FormulaEvaluationEndDate, FormulaEvaluationEndHour, ref MethodCount))
								Category.CheckCatalogResult = "B";
						}

						MethodRecords.RowFilter = OldFilter;
					}
				}
				else
					Log = false;
			}
			catch (Exception ex)
			{ ReturnVal = Category.CheckEngine.FormatError(ex, "FORMULA15"); }

			return ReturnVal;
		}

		public static string FORMULA16(cCategory Category, ref bool Log) //Required Formula Reported for F-Factor Formula
		{
			string ReturnVal = "";

			try
			{
				bool FormulaCodeValid = (bool)Category.GetCheckParameter("formula_code_valid").ParameterValue;
				DataRowView CurrentFormula = (DataRowView)Category.GetCheckParameter("current_formula").ParameterValue;
				string ParameterCode = cDBConvert.ToString(CurrentFormula["parameter_cd"]);

				if (ParameterCode.InList("FD,FC,FW") && FormulaCodeValid)
				{
					DataView FormulaCodeToFFactorParameterRecords = (DataView)Category.GetCheckParameter("Formula_Code_To_F-Factor_Parameter_Cross_Check_Table").ParameterValue;
					string OldFilter = FormulaCodeToFFactorParameterRecords.RowFilter;

					FormulaCodeToFFactorParameterRecords.RowFilter = AddToDataViewFilter(FormulaCodeToFFactorParameterRecords.RowFilter,
					  " ParameterCode = '" + ParameterCode + "' ");

					DataView FormulaRecords = (DataView)Category.GetCheckParameter("Formula_Records").ParameterValue;
					string OldFormulaFilter = FormulaRecords.RowFilter;

					DateTime FormulaEvaluationBeginDate = (DateTime)Category.GetCheckParameter("Formula_Evaluation_Begin_Date").ParameterValue;
					DateTime FormulaEvaluationEndDate = (DateTime)Category.GetCheckParameter("Formula_Evaluation_End_Date").ParameterValue;
					int FormulaEvaluationBeginHour = cDBConvert.ToHour(Category.GetCheckParameter("Formula_Evaluation_Begin_Hour").ParameterValue, DateTypes.START);
					int FormulaEvaluationEndHour = cDBConvert.ToHour(Category.GetCheckParameter("Formula_Evaluation_End_Hour").ParameterValue, DateTypes.END);

					int FormulaCount = 0;
					string FormulaCodes = "";

					foreach (DataRowView drvFormulaCodeToFFactorParameter in FormulaCodeToFFactorParameterRecords)
					{
						FormulaCodes = FormulaCodes.ListAdd("'" + cDBConvert.ToString(drvFormulaCodeToFFactorParameter["FormulaCode"]) + "'");
					}

					if (FormulaCodes.Length > 0)
					{
						FormulaRecords.RowFilter = " mon_loc_id = '" + cDBConvert.ToString(CurrentFormula["mon_loc_id"]) + "' ";

						FormulaRecords.RowFilter = AddToDataViewFilter(OldFormulaFilter,
						  " equation_cd in (" + FormulaCodes + ") ");

						FormulaRecords.RowFilter = AddEvaluationDateHourRangeToDataViewFilter(FormulaRecords.RowFilter,
						  FormulaEvaluationBeginDate, FormulaEvaluationEndDate, FormulaEvaluationBeginHour, FormulaEvaluationEndHour, false, true);

						if (!(CheckForHourRangeCovered(Category, FormulaRecords, FormulaEvaluationBeginDate, FormulaEvaluationBeginHour,
													 FormulaEvaluationEndDate, FormulaEvaluationEndHour, ref FormulaCount)))
						{
							if (FormulaCount == 0)
								Category.CheckCatalogResult = "A";
							else
								Category.CheckCatalogResult = "B";
						}
					}

					FormulaRecords.RowFilter = OldFormulaFilter;
					FormulaCodeToFFactorParameterRecords.RowFilter = OldFilter;
				}
				else
					Log = false;
			}
			catch (Exception ex)
			{ ReturnVal = Category.CheckEngine.FormatError(ex, "FORMULA16"); }

			return ReturnVal;
		}

		public static string FORMULA17(cCategory Category, ref bool Log) //determines if a minimum O2 and H2O default values are reported for HI formulas
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentFormula = (DataRowView)Category.GetCheckParameter("Current_Formula").ParameterValue;
				bool FormulaCodeValid = (bool)Category.GetCheckParameter("Formula_Code_Valid").ParameterValue;
				string ParameterCode = cDBConvert.ToString(CurrentFormula["parameter_cd"]);

				string IncompleteDefaultForFormula = "";
				string MissingDefaultForFormula = "";

				if (ParameterCode == "HI" && FormulaCodeValid)
				{
					bool O2ComponentRequired = (bool)Category.GetCheckParameter("O2_Component_Required").ParameterValue;
					DateTime FormulaEvaluationBeginDate = (DateTime)Category.GetCheckParameter("Formula_Evaluation_Begin_Date").ParameterValue;
					DateTime FormulaEvaluationEndDate = (DateTime)Category.GetCheckParameter("Formula_Evaluation_End_Date").ParameterValue;
					int FormulaEvaluationBeginHour = cDBConvert.ToHour(Category.GetCheckParameter("Formula_Evaluation_Begin_Hour").ParameterValue, DateTypes.START);
					int FormulaEvaluationEndHour = cDBConvert.ToHour(Category.GetCheckParameter("Formula_Evaluation_End_Hour").ParameterValue, DateTypes.END);
					DataView MonitorDefaultRecords = (DataView)Category.GetCheckParameter("Default_Records").ParameterValue;

					string OldFilter = MonitorDefaultRecords.RowFilter;
					//MonitorDefaultRecords.RowFilter = AddEvaluationDateHourRangeToDataViewFilter(OldFilter, FormulaEvaluationBeginDate,
					//    FormulaEvaluationEndDate, FormulaEvaluationBeginHour, FormulaEvaluationEndHour);


					int FormulaCount = 0;
					if (O2ComponentRequired)
					{
						MonitorDefaultRecords.RowFilter = AddToDataViewFilter(OldFilter, "parameter_cd = 'O2N'");
						if (!(CheckForHourRangeCovered(Category, MonitorDefaultRecords, FormulaEvaluationBeginDate, FormulaEvaluationBeginHour,
							FormulaEvaluationEndDate, FormulaEvaluationEndHour, ref FormulaCount)))
						{
							if (FormulaCount == 0)
								MissingDefaultForFormula += MissingDefaultForFormula == "" ? "O2N" : ",O2N";
							else
							{
								FormulaCount = 0;
								IncompleteDefaultForFormula += IncompleteDefaultForFormula == "" ? "O2N" : ",O2N";
							}
						}
						MonitorDefaultRecords.RowFilter = OldFilter;
					}
					Category.SetCheckParameter("Incomplete_Default_for_Formula", IncompleteDefaultForFormula.FormatList(), eParameterDataType.String);
					Category.SetCheckParameter("Missing_Default_for_Formula", MissingDefaultForFormula.FormatList(), eParameterDataType.String);

					if (MissingDefaultForFormula != "" && IncompleteDefaultForFormula == "")
						Category.CheckCatalogResult = "A";
					else if (MissingDefaultForFormula == "" && IncompleteDefaultForFormula != "")
						Category.CheckCatalogResult = "B";
					else if (MissingDefaultForFormula != "" && IncompleteDefaultForFormula != "")
						Category.CheckCatalogResult = "C";

				}
				else Log = false;
			}
			catch (Exception ex)
			{ ReturnVal = Category.CheckEngine.FormatError(ex, "FORMULA17"); }

			return ReturnVal;
		}

		public static string FORMULA18(cCategory Category, ref bool Log) //Duplicate Formula Records
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentFormula = (DataRowView)Category.GetCheckParameter("Current_Formula").ParameterValue;
				string FormulaIdentifier = cDBConvert.ToString(CurrentFormula["Formula_identifier"]);
				DataView FormulaRecords = (DataView)Category.GetCheckParameter("Formula_Records").ParameterValue;
				string FormulaRecordsOldFilter = FormulaRecords.RowFilter;
				string FormulaRecordsNewFilter = AddToDataViewFilter(FormulaRecordsOldFilter, "formula_identifier = '" + FormulaIdentifier + "'");
				FormulaRecords.RowFilter = FormulaRecordsNewFilter;

				if (FormulaRecords.Count > 1 ||
					(FormulaRecords.Count == 1 &&
						cDBConvert.ToString(FormulaRecords[0]["mon_form_id"]) != cDBConvert.ToString(CurrentFormula["mon_form_id"])))
				{
					Category.CheckCatalogResult = "A";
				}
				FormulaRecords.RowFilter = FormulaRecordsOldFilter;
			}
			catch (Exception ex)
			{ ReturnVal = Category.CheckEngine.FormatError(ex, "FORMULA18"); }

			return ReturnVal;
		}

        /// <summary>
        /// MATS Apportionment/Summary Formula Validation
        /// 
        /// Ensures that for an "MS-1" equation:
        /// 
        /// 1) A MATS RE or RH method exists that was active during the period of the formula.
        /// 2) The formula exists at a unit linked multiple stacks, and is not linked to any other type of location.
        /// </summary>
        /// <param name="category"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public static string FORMULA19(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                if (MpParameters.CurrentFormula.ParameterCd.InList("HGRE,HCLRE,HFRE,SO2RE,HGRH,HCLRH,HFRH,SO2RH"))
                {
                    if (MpParameters.CurrentFormula.EquationCd == "MS-1")
                    {
                        int methodCount =
                            MpParameters.MethodRecords.CountRows
                            (
                                new cFilterCondition[]
                                {
                                    new cFilterCondition("PARAMETER_CD", MpParameters.CurrentFormula.ParameterCd),
                                    new cFilterCondition("METHOD_CD", "CALC"),
                                    new cFilterCondition("BEGIN_DATEHOUR", eFilterConditionRelativeCompare.LessThanOrEqual, 
                                                         MpParameters.FormulaEvaluationEndDate.Value.AddHours(MpParameters.FormulaEvaluationEndHour.Value)),
                                    new cFilterCondition("END_DATEHOUR", eFilterConditionRelativeCompare.GreaterThanOrEqual, 
                                                         MpParameters.FormulaEvaluationBeginDate.Value.AddHours(MpParameters.FormulaEvaluationBeginHour.Value), 
                                                         eNullDateDefault.Max)
                                }
                            );

                        if (methodCount == 0)
                        {
                            category.CheckCatalogResult = "A";
                        }
                        else if (MpParameters.LocationType != "US")
                        {
                            category.CheckCatalogResult = "B";
                        }
                        else
                        {
                            int configCount =
                                MpParameters.UnitStackConfigurationRecords.CountRows
                                (
                                    new cFilterCondition[]
                                    {
                                        new cFilterCondition("MON_LOC_ID", MpParameters.CurrentFormula.MonLocId),
                                        new cFilterCondition("STACK_NAME", "MS", eFilterConditionStringCompare.BeginsWith, true),
                                        new cFilterCondition("BEGIN_DATE", eFilterConditionRelativeCompare.LessThanOrEqual, MpParameters.FormulaEvaluationEndDate.Value),
                                        new cFilterCondition("END_DATE", eFilterConditionRelativeCompare.GreaterThanOrEqual, MpParameters.FormulaEvaluationBeginDate.Value, eNullDateDefault.Max)
                                    }
                                );

                            if (configCount > 0)
                            {
                                category.CheckCatalogResult = "B";
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
        /// Formula Valid for Location Type
        /// 
        /// Ensures that formulas codes that are limited to specific location types are only reported at locations of those types.
        /// </summary>
        /// <param name="category"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public static string FORMULA20(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                MpParameters.ValidLocationTypes = "";

                if (MpParameters.FormulaCodeValid == true)
                {
                    if (MpParameters.CurrentFormula.EquationCd == "MS-2")
                    {
                        if (MpParameters.LocationType != "MS")
                        {
                            MpParameters.ValidLocationTypes = "multiple stacks";
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
	}
}
