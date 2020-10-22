using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Mp.Parameters;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Extensions;

namespace ECMPS.Checks.ComponentChecks
{
	public class cComponentChecks : cChecks
	{

		public cComponentChecks()
		{
			CheckProcedures = new dCheckProcedure[82];

			CheckProcedures[3] = new dCheckProcedure(COMPON3);
			CheckProcedures[4] = new dCheckProcedure(COMPON4);
			CheckProcedures[5] = new dCheckProcedure(COMPON5);
			CheckProcedures[6] = new dCheckProcedure(COMPON6);
			CheckProcedures[7] = new dCheckProcedure(COMPON7);
			CheckProcedures[8] = new dCheckProcedure(COMPON8);
			CheckProcedures[10] = new dCheckProcedure(COMPON10);

			CheckProcedures[11] = new dCheckProcedure(COMPON11);
			CheckProcedures[12] = new dCheckProcedure(COMPON12);
			CheckProcedures[13] = new dCheckProcedure(COMPON13);
			CheckProcedures[14] = new dCheckProcedure(COMPON14);
			CheckProcedures[16] = new dCheckProcedure(COMPON16);
			CheckProcedures[18] = new dCheckProcedure(COMPON18);
			CheckProcedures[19] = new dCheckProcedure(COMPON19);
			CheckProcedures[20] = new dCheckProcedure(COMPON20);

			CheckProcedures[21] = new dCheckProcedure(COMPON21);
			CheckProcedures[22] = new dCheckProcedure(COMPON22);
			CheckProcedures[26] = new dCheckProcedure(COMPON26);
			CheckProcedures[30] = new dCheckProcedure(COMPON30);
			CheckProcedures[33] = new dCheckProcedure(COMPON33);
			CheckProcedures[34] = new dCheckProcedure(COMPON34);
			CheckProcedures[36] = new dCheckProcedure(COMPON36);
			CheckProcedures[37] = new dCheckProcedure(COMPON37);
			CheckProcedures[38] = new dCheckProcedure(COMPON38);
			CheckProcedures[39] = new dCheckProcedure(COMPON39);

			CheckProcedures[44] = new dCheckProcedure(COMPON44);
			CheckProcedures[45] = new dCheckProcedure(COMPON45);
			CheckProcedures[46] = new dCheckProcedure(COMPON46);
			CheckProcedures[47] = new dCheckProcedure(COMPON47);
			CheckProcedures[48] = new dCheckProcedure(COMPON48);

			CheckProcedures[51] = new dCheckProcedure(COMPON51);
			CheckProcedures[52] = new dCheckProcedure(COMPON52);
			CheckProcedures[53] = new dCheckProcedure(COMPON53);
			CheckProcedures[54] = new dCheckProcedure(COMPON54);
			CheckProcedures[55] = new dCheckProcedure(COMPON55);
			CheckProcedures[56] = new dCheckProcedure(COMPON56);
			CheckProcedures[57] = new dCheckProcedure(COMPON57);
			CheckProcedures[58] = new dCheckProcedure(COMPON58);
			CheckProcedures[59] = new dCheckProcedure(COMPON59);
			CheckProcedures[60] = new dCheckProcedure(COMPON60);

			CheckProcedures[61] = new dCheckProcedure(COMPON61);
			CheckProcedures[62] = new dCheckProcedure(COMPON62);
			CheckProcedures[63] = new dCheckProcedure(COMPON63);
			CheckProcedures[64] = new dCheckProcedure(COMPON64);
			CheckProcedures[65] = new dCheckProcedure(COMPON65);
			CheckProcedures[66] = new dCheckProcedure(COMPON66);
			CheckProcedures[67] = new dCheckProcedure(COMPON67);
			CheckProcedures[68] = new dCheckProcedure(COMPON68);
			CheckProcedures[69] = new dCheckProcedure(COMPON69);
			CheckProcedures[70] = new dCheckProcedure(COMPON70);

			CheckProcedures[80] = new dCheckProcedure(COMPON80);
			CheckProcedures[81] = new dCheckProcedure(COMPON81);

		}

		#region Checks 11 - 20

		public static string COMPON13(cCategory Category, ref bool Log)
		// Component Sample Acquisiton Method Code Valid
		{
			string ReturnVal = "";

			try
			{
				bool ComponentComponentTypeValid = (bool)Category.GetCheckParameter("component_Component_Type_Valid").ParameterValue;
				bool ComponentBasisCodeValid = (bool)Category.GetCheckParameter("component_Basis_Code_Valid").ParameterValue;

				if (ComponentComponentTypeValid && ComponentBasisCodeValid)
				{
					DataRowView CurrentComponent = (DataRowView)Category.GetCheckParameter("Current_Component").ParameterValue;
					string ComponentTypeCode = cDBConvert.ToString(CurrentComponent["Component_Type_Cd"]);
					string BasisCode = cDBConvert.ToString(CurrentComponent["Basis_Cd"]);
					DataView ComponentTypeAndBasisToSampleAcquisitionMethodCrossCheckTable = (DataView)Category.GetCheckParameter("Component_Type_And_Basis_To_Sample_Acquisition_Method_Cross_Check_Table").ParameterValue;

					string ComponentTypeAndBasisToSampleAcquisitionMethodOldFilter = ComponentTypeAndBasisToSampleAcquisitionMethodCrossCheckTable.RowFilter;

					if (CurrentComponent["acq_cd"] == DBNull.Value)
					{
						if (BasisCode == "")
							ComponentTypeAndBasisToSampleAcquisitionMethodCrossCheckTable.RowFilter =
							  AddToDataViewFilter(ComponentTypeAndBasisToSampleAcquisitionMethodOldFilter,
							  "GenericComponentType = '" + ComponentTypeCode + "' and BasisCode is null and SampleAcquisitionMethodCode is null");
						else
							ComponentTypeAndBasisToSampleAcquisitionMethodCrossCheckTable.RowFilter =
							  AddToDataViewFilter(ComponentTypeAndBasisToSampleAcquisitionMethodOldFilter,
							  "GenericComponentType = '" + ComponentTypeCode + "' and BasisCode = '" + BasisCode +
							  "' and SampleAcquisitionMethodCode is null");

						if (ComponentTypeAndBasisToSampleAcquisitionMethodCrossCheckTable.Count == 0)
							Category.CheckCatalogResult = "A";
					}
					else
					{
						string SampleAcquisitionMethodCode = cDBConvert.ToString(CurrentComponent["acq_cd"]);
						DataView SampleAcquisitionMethodCodeRecords = (DataView)Category.GetCheckParameter("Sample_Acquisition_Method_Code_lookup_table").ParameterValue;

						if (!LookupCodeExists(SampleAcquisitionMethodCode, SampleAcquisitionMethodCodeRecords))
							Category.CheckCatalogResult = "B";
						else
						{
							string GenericComponentType = "";

							if (ComponentTypeCode.InList("SO2,NOX,CO2,O2,PRB,HG,HCL,HF,PM"))
								GenericComponentType = "CONC";
							else if (ComponentTypeCode.InList("OFFM,GFFM,BOFF,BGFF,DP,TEMP,PRES,FLC,GCH,MS,CALR"))
								GenericComponentType = "FUELFLOW";
							else
								GenericComponentType = ComponentTypeCode;

							if (BasisCode == "B" || ComponentTypeCode.InList("FLOW,PRB,PM"))
							{
								ComponentTypeAndBasisToSampleAcquisitionMethodCrossCheckTable.RowFilter =
								  AddToDataViewFilter(ComponentTypeAndBasisToSampleAcquisitionMethodOldFilter,
								  "GenericComponentType = '" + GenericComponentType +
								  "' and SampleAcquisitionMethodCode = '" + SampleAcquisitionMethodCode + "'");

								if (ComponentTypeAndBasisToSampleAcquisitionMethodCrossCheckTable.Count == 0)
									Category.CheckCatalogResult = "C";
							}
							else
							{
								if (BasisCode == "")
								{
									ComponentTypeAndBasisToSampleAcquisitionMethodCrossCheckTable.RowFilter =
									 AddToDataViewFilter(ComponentTypeAndBasisToSampleAcquisitionMethodOldFilter,
									 "GenericComponentType = '" + GenericComponentType +
									 "' and SampleAcquisitionMethodCode = '" + SampleAcquisitionMethodCode +
									 "' and BasisCode is null ");
								}
								else
								{
									ComponentTypeAndBasisToSampleAcquisitionMethodCrossCheckTable.RowFilter =
									 AddToDataViewFilter(ComponentTypeAndBasisToSampleAcquisitionMethodOldFilter,
									 "GenericComponentType = '" + GenericComponentType +
									 "' and SampleAcquisitionMethodCode = '" + SampleAcquisitionMethodCode +
									 "' and BasisCode = '" + BasisCode + "'");
								}

								if (ComponentTypeAndBasisToSampleAcquisitionMethodCrossCheckTable.Count == 0)
									Category.CheckCatalogResult = "C";
							}
						}
					}

					ComponentTypeAndBasisToSampleAcquisitionMethodCrossCheckTable.RowFilter = ComponentTypeAndBasisToSampleAcquisitionMethodOldFilter;
				}
				else
					Log = false;
			}
			catch (Exception ex)
			{ ReturnVal = Category.CheckEngine.FormatError(ex, "COMPON13"); }

			return ReturnVal;
		}

		public static string COMPON14(cCategory Category, ref bool Log)
		// Component Basis Code Valid
		{
			string ReturnVal = "";

			try
			{
				bool ComponentComponentTypeValid = cDBConvert.ToBoolean(Category.GetCheckParameter("Component_Component_Type_Valid").ParameterValue);
				Category.SetCheckParameter("Component_Basis_Code_Valid", false, eParameterDataType.Boolean);

				if (ComponentComponentTypeValid)
				{
					bool ComponentBasisCodeValid = true;

					DataRowView CurrentComponent = (DataRowView)Category.GetCheckParameter("Current_Component").ParameterValue;
					string ComponentTypeCode = cDBConvert.ToString(CurrentComponent["Component_Type_Cd"]);
					string BasisCode = cDBConvert.ToString(CurrentComponent["Basis_Cd"]);

					if (ComponentTypeCode.InList("NOX,SO2,CO2,O2,FLOW,HG,HCL,HF,STRAIN"))
					{
						if (CurrentComponent["Basis_Cd"] == DBNull.Value)
						{
							Category.CheckCatalogResult = "A";
							ComponentBasisCodeValid = false;
						}
						else if (!BasisCode.InList("W,D,B"))
						{
							Category.CheckCatalogResult = "B";
							ComponentBasisCodeValid = false;
						}
						else if ((ComponentTypeCode == "FLOW") && (BasisCode != "W"))
						{
							Category.CheckCatalogResult = "B";
							ComponentBasisCodeValid = false;
						}
						else if (ComponentTypeCode == "STRAIN" && BasisCode != "D")
						{
							Category.CheckCatalogResult = "B";
							ComponentBasisCodeValid = false;
						}
						else if ((ComponentTypeCode != "O2") && (BasisCode == "B"))
						{
							Category.CheckCatalogResult = "B";
							ComponentBasisCodeValid = false;
						}
						else if (BasisCode != "B")
						{
							string ComponentIdentifier = cDBConvert.ToString(CurrentComponent["Component_Identifier"]);
							DataView UsedIdentifierRecords = (DataView)Category.GetCheckParameter("Used_Identifier_Records").ParameterValue;
							DataRowView UsedIdentifierRow;
							sFilterPair[] UsedIdentifierFilter = new sFilterPair[2];

							UsedIdentifierFilter[0].Set("Table_Cd", "C");
							UsedIdentifierFilter[1].Set("Identifier", ComponentIdentifier);

							if (FindRow(UsedIdentifierRecords, UsedIdentifierFilter, out UsedIdentifierRow))
							{
								if ((UsedIdentifierRow["Formula_Or_Basis_Cd"] != DBNull.Value) &&
									(cDBConvert.ToString(UsedIdentifierRow["Formula_Or_Basis_Cd"]) != BasisCode))
								{
									Category.CheckCatalogResult = "C";
									ComponentBasisCodeValid = false;
								}
							}
						}
					}
					else
					{
						if (CurrentComponent["Basis_Cd"] != DBNull.Value)
						{
							Category.CheckCatalogResult = "D";
							ComponentBasisCodeValid = false;
						}
					}

					Category.SetCheckParameter("Component_Basis_Code_Valid", ComponentBasisCodeValid, eParameterDataType.Boolean);
				}
				else
					Log = false;
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "COMPON14");
			}

			return ReturnVal;
		}

		#endregion

		#region Checks 71 - 80

		public static string COMPON80(cCategory category, ref bool log)
		// Overlapping System Component Records
		{
			string returnValue = "";

			try
			{

				if (category.GetCheckParameter("System_Component_Dates_And_Hours_Consistent").AsBoolean().Default())
				{
					DataRowView currentSystemComponent = category.GetCheckParameter("Current_System_Component").ValueAsDataRowView();
					DataView systemSystemComponentRecords = category.GetCheckParameter("System_System_Component_Records").ValueAsDataView();
					DateTime systemComponentEvaluationBeginDate = category.GetCheckParameter("System_Component_Evaluation_Begin_Date").ValueAsDateTime(DateTypes.START);
					int systemComponentEvaluationBeginHour = category.GetCheckParameter("System_Component_Evaluation_Begin_Hour").AsHour().Default(DateTypes.START).AsInteger();
					DateTime systemComponentEvaluationEndDate = category.GetCheckParameter("System_Component_Evaluation_End_Date").ValueAsDateTime(DateTypes.END);
					int systemComponentEvaluationEndHour = category.GetCheckParameter("System_Component_Evaluation_End_Hour").AsHour().Default(DateTypes.END).AsInteger();

					string currentMonSysCompId = currentSystemComponent["MON_SYS_COMP_ID"].AsString();
					string currentComponentId = currentSystemComponent["COMPONENT_ID"].AsString();
					string currentComponentIdentifier = currentSystemComponent["COMPONENT_IDENTIFIER"].AsString();
					DateTime currentBeginDate = currentSystemComponent["BEGIN_DATE"].AsDateTime().Default(DateTypes.START);
					int currentBeginHour = currentSystemComponent["BEGIN_HOUR"].AsHour().Default(DateTypes.START).AsInteger();

					bool found = false;

					foreach (DataRowView systemSystemComponent in systemSystemComponentRecords)
					{
						if ((systemSystemComponent["MON_SYS_COMP_ID"].AsString() != currentMonSysCompId) &&
							(systemSystemComponent["COMPONENT_ID"].AsString() == currentComponentId))
						{
							DateTime systemBeginDate = systemSystemComponent["BEGIN_DATE"].AsDateTime().Default(DateTypes.START);
							int systemBeginHour = systemSystemComponent["BEGIN_HOUR"].AsHour().Default(DateTypes.START).AsInteger();
							DateTime systemEndDate = systemSystemComponent["END_DATE"].AsDateTime().Default(DateTypes.END);
							int systemEndHour = systemSystemComponent["END_HOUR"].AsHour().Default(DateTypes.END).AsInteger();

							if (((systemBeginDate > currentBeginDate) ||
								 ((systemBeginDate == currentBeginDate) &&
								  (systemBeginHour >= currentBeginHour))) &&
								((systemBeginDate < systemComponentEvaluationEndDate) ||
								 ((systemBeginDate == systemComponentEvaluationEndDate) &&
								  (systemBeginHour <= systemComponentEvaluationEndHour))) &&
								((systemEndDate > systemComponentEvaluationBeginDate) ||
								 ((systemEndDate == systemComponentEvaluationBeginDate) &&
								  (systemEndHour >= systemComponentEvaluationBeginHour))))
							{
								found = true;
								break;
							}
						}
					}

					if (found)
						category.CheckCatalogResult = "A";
				}
			}
			catch (Exception ex)
			{ returnValue = category.CheckEngine.FormatError(ex, "COMPON80"); }

			return returnValue;
		}

		#endregion

		#region Check 81 - 90

		public static string COMPON81(cCategory category, ref bool log)
		// Hg Converter Indicator
		{
			string returnValue = "";

			try
			{
				//DataRowView CurrentComponent = MpReportParameters.CurrentComponent;

				if (MpParameters.ComponentComponentTypeValid.AsBoolean().Default(false))
				{
					if (MpParameters.CurrentComponent.ComponentTypeCd == "HG")
					{
						if (MpParameters.CurrentComponent.HgConverterInd == null)
						{
							category.CheckCatalogResult = "A";
						}
						else if (MpParameters.CurrentComponent.HgConverterInd != 1 && MpParameters.CurrentComponent.HgConverterInd != 0)
						{
							category.CheckCatalogResult = "B";
						}
					}
					else if (MpParameters.CurrentComponent.HgConverterInd != null)
					{
						category.CheckCatalogResult = "C";
					}

				}
				else
					log = false;
			}
			catch (Exception ex)
			{
				returnValue = category.CheckEngine.FormatError(ex, "COMPON81");
			}
			return returnValue;
		}

		#endregion

		#region Old Grouping by Category

		#region Analyzer Range Checks

		public static string COMPON16(cCategory Category, ref bool Log) //Analyzer Range Code Valid
		{
			string ReturnVal = "";

			try
			{
				//Category.SetCheckParameter("Analyzer_Range_Code_Valid", null, ParameterTypes.BOOLEAN);


				Category.SetCheckParameter("Analyzer_Range_Code_Valid", true, eParameterDataType.Boolean);
				DataRowView CurrentAnalyzerRange = (DataRowView)Category.GetCheckParameter("Current_Analyzer_Range").ParameterValue;

				if (CurrentAnalyzerRange["analyzer_range_cd"] != DBNull.Value)
				{
					string AnalyzerRangeCode = cDBConvert.ToString(CurrentAnalyzerRange["analyzer_range_cd"]);
					DataView AnalyzerRangeCodeRecords = (DataView)Category.GetCheckParameter("Analyzer_Range_Code_Lookup_Table").ParameterValue;

					if (!LookupCodeExists(AnalyzerRangeCode, AnalyzerRangeCodeRecords))
					{
						Category.CheckCatalogResult = "B";
						Category.SetCheckParameter("Analyzer_Range_Code_Valid", false, eParameterDataType.Boolean);
					}
					else if (MpParameters.CurrentComponent.ComponentTypeCd == "HG" && AnalyzerRangeCode != "H")
					{
						Category.CheckCatalogResult = "C";
					}
				}
				else
				{
					Category.CheckCatalogResult = "A";
					Category.SetCheckParameter("Analyzer_Range_Code_Valid", false, eParameterDataType.Boolean);
				}
			}
			catch (Exception ex)
			{ ReturnVal = Category.CheckEngine.FormatError(ex, "COMPON16"); }

			return ReturnVal;
		}

		public static string COMPON18(cCategory Category, ref bool Log) //Analyzer Range Start Date Valid
		{
			string ReturnVal = "";

			try
			{
				//Category.SetCheckParameter("Analyzer_Range_Start_Date_valid", null, ParameterTypes.BOOLEAN);

				ReturnVal = Check_ValidStartDate(Category, "Analyzer_Range_Start_Date_Valid", "Current_Analyzer_Range");
			}
			catch (Exception ex)
			{ ReturnVal = Category.CheckEngine.FormatError(ex, "COMPON18"); }

			return ReturnVal;
		}

		public static string COMPON19(cCategory Category, ref bool Log) //Analyzer Range Start Hour Valid
		{
			string ReturnVal = "";

			try
			{
				//Category.SetCheckParameter("Analyzer_Range_Start_hour_valid", null, ParameterTypes.BOOLEAN);

				ReturnVal = Check_ValidStartHour(Category, "Analyzer_Range_Start_Hour_Valid", "Current_Analyzer_Range");
			}
			catch (Exception ex)
			{ ReturnVal = Category.CheckEngine.FormatError(ex, "COMPON19"); }

			return ReturnVal;
		}

		public static string COMPON20(cCategory Category, ref bool Log) //Analyzer Range End Date Valid
		{
			string ReturnVal = "";

			try
			{
				//Category.SetCheckParameter("Analyzer_Range_end_Date_valid", null, ParameterTypes.BOOLEAN);

				ReturnVal = Check_ValidEndDate(Category, "Analyzer_Range_End_Date_Valid", "Current_Analyzer_Range");
			}
			catch (Exception ex)
			{ ReturnVal = Category.CheckEngine.FormatError(ex, "COMPON20"); }

			return ReturnVal;
		}

		public static string COMPON21(cCategory Category, ref bool Log) //Analyzer Range End Hour Valid
		{
			string ReturnVal = "";

			try
			{
				//Category.SetCheckParameter("Analyzer_Range_end_hour_valid", null, ParameterTypes.BOOLEAN);

				ReturnVal = Check_ValidEndHour(Category, "Analyzer_Range_End_Hour_Valid", "Current_Analyzer_Range");
			}
			catch (Exception ex)
			{ ReturnVal = Category.CheckEngine.FormatError(ex, "COMPON21"); }

			return ReturnVal;
		}

		public static string COMPON22(cCategory Category, ref bool Log)
		// Analyzer Range Dates and Hours Consistent
		{
			string ReturnVal = "";

			try
			{
				ReturnVal = Check_ConsistentHourRange(Category, "Analyzer_Range_Dates_And_Hours_Consistent",
																"Current_Analyzer_Range",
																"Analyzer_Range_Start_Date_Valid",
																"Analyzer_Range_Start_Hour_Valid",
																"Analyzer_Range_End_Date_Valid",
																"Analyzer_Range_End_Hour_Valid");
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "COMPON22");
			}

			return ReturnVal;

		}

		public static string COMPON34(cCategory Category, ref bool Log)
		// Required Second Component Reported for Dual Range Analyzer
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentAnalyzerRange = (DataRowView)Category.GetCheckParameter("Current_Analyzer_Range").ParameterValue;
				string AnalyzerRangeCode = cDBConvert.ToString(CurrentAnalyzerRange["Analyzer_Range_Cd"]);
				int DualRangeIndicator = cDBConvert.ToInteger(CurrentAnalyzerRange["Dual_Range_Ind"]);

				if (AnalyzerRangeCode.InList("H,L") && DualRangeIndicator == 1)
				{
					string AnalyzerRangeId = cDBConvert.ToString(CurrentAnalyzerRange["Analyzer_Range_Id"]);
					DateTime AnalyzerRangeEvaluationBeginDate = (DateTime)Category.GetCheckParameter("Analyzer_Range_Evaluation_Begin_Date").ParameterValue;
					int AnalyzerRangeEvaluationBeginHour = cDBConvert.ToHour(Category.GetCheckParameter("Analyzer_Range_Evaluation_Begin_Hour").ParameterValue, DateTypes.START);
					DateTime AnalyzerRangeEvaluationEndDate = (DateTime)Category.GetCheckParameter("Analyzer_Range_Evaluation_End_Date").ParameterValue;
					int AnalyzerRangeEvaluationEndHour = cDBConvert.ToHour(Category.GetCheckParameter("Analyzer_Range_Evaluation_End_Hour").ParameterValue, DateTypes.END);

					DataRowView CurrentComponent = (DataRowView)Category.GetCheckParameter("Current_Component").ParameterValue;
					string ComponentTypeCode = cDBConvert.ToString(CurrentComponent["Component_Type_Cd"]);

					DataView AnalyzerRangeRecords = (DataView)Category.GetCheckParameter("Location_Analyzer_Range_Records").ParameterValue;
					string DualAnalyzerRangeCd = (AnalyzerRangeCode == "H") ? "L" : "H";
					sFilterPair[] AnalyzerRangeFilter = new sFilterPair[4];

					AnalyzerRangeFilter[0].Set("Component_Type_Cd", ComponentTypeCode);
					AnalyzerRangeFilter[1].Set("Analyzer_Range_Cd", DualAnalyzerRangeCd);
					AnalyzerRangeFilter[2].Set("Analyzer_Range_Id", AnalyzerRangeId, true);
					AnalyzerRangeFilter[3].Set("Dual_Range_Ind", "1");

					DataView AnalyzerRangeView = FindActiveRows(AnalyzerRangeRecords,
																AnalyzerRangeEvaluationBeginDate,
																AnalyzerRangeEvaluationBeginHour,
																AnalyzerRangeEvaluationEndDate,
																AnalyzerRangeEvaluationEndHour,
																AnalyzerRangeFilter);

					if (AnalyzerRangeView.Count == 0)
						Category.CheckCatalogResult = "A";
					else
					{
						string RangeReplaceNormal = (AnalyzerRangeCode == "H") ? "HIGH" : "LOW";
						string RangeReplaceAbbrev = (AnalyzerRangeCode == "H") ? "HI" : "LO";
						string DualRangeReplaceNormal = (AnalyzerRangeCode == "H") ? "LOW" : "HIGH";
						string DualRangeReplaceAbbrev = (AnalyzerRangeCode == "H") ? "LO" : "HI";

						string SerialNumber = cDBConvert.ToString(CurrentAnalyzerRange["Serial_Number"]);
						string SerialNumberTemp = SerialNumber.Replace(RangeReplaceNormal, "").Replace(RangeReplaceAbbrev, "");

						DateTime AnalyzerRangeBeganDate = cDBConvert.ToDate(CurrentAnalyzerRange["Begin_Date"], DateTypes.START);
						int AnalyzerRangeBeganHour = cDBConvert.ToHour(CurrentAnalyzerRange["Begin_Hour"], DateTypes.START);
						DateTime AnalyzerRangeEndedDate = cDBConvert.ToDate(CurrentAnalyzerRange["End_Date"], DateTypes.END);
						int AnalyzerRangeEndedHour = cDBConvert.ToHour(CurrentAnalyzerRange["End_Hour"], DateTypes.END);

						bool NoneEqual = true;
						foreach (DataRowView AnalyzerRangeRecord in AnalyzerRangeView)
						{
							string DualSerialNumber = cDBConvert.ToString(AnalyzerRangeRecord["Serial_Number"]);
							string DualSerialNumberTemp = DualSerialNumber.Replace(DualRangeReplaceNormal, "").Replace(DualRangeReplaceAbbrev, "");

							if (DualSerialNumberTemp == SerialNumberTemp)
							{
								DateTime DualAnalyzerRangeBeganDate = cDBConvert.ToDate(AnalyzerRangeRecord["Begin_Date"], DateTypes.START);
								int DualAnalyzerRangeBeganHour = cDBConvert.ToHour(AnalyzerRangeRecord["Begin_Hour"], DateTypes.START);
								DateTime DualAnalyzerRangeEndedDate = cDBConvert.ToDate(AnalyzerRangeRecord["End_Date"], DateTypes.END);
								int DualAnalyzerRangeEndedHour = cDBConvert.ToHour(AnalyzerRangeRecord["End_Hour"], DateTypes.END);

								NoneEqual = false;
								if ((DualAnalyzerRangeBeganDate != AnalyzerRangeBeganDate) ||
									(DualAnalyzerRangeBeganHour != AnalyzerRangeBeganHour) ||
									(DualAnalyzerRangeEndedDate != AnalyzerRangeEndedDate) ||
									(DualAnalyzerRangeEndedHour != AnalyzerRangeEndedHour))
								{
									Category.CheckCatalogResult = "C";
									break;
								}
							}
						}

						if (NoneEqual)
							Category.CheckCatalogResult = "B";
					}
				}
				else
					Log = false;
			}
			catch (Exception ex)
			{ ReturnVal = Category.CheckEngine.FormatError(ex, "COMPON34"); }

			return ReturnVal;
		}

		public static string COMPON36(cCategory Category, ref bool Log) //Analyzer Range Record Valid
		{
			string ReturnVal = "";

			try
			{
				Category.SetCheckParameter("Analyzer_Range_record_Valid", true, eParameterDataType.Boolean);

				DataRowView CurrentAnalyzerRange = (DataRowView)Category.GetCheckParameter("Current_Analyzer_Range").ParameterValue;
				string ComponentTypeCode = cDBConvert.ToString(CurrentAnalyzerRange["Component_Type_cd"]);

				if (!ComponentTypeCode.InList("SO2,CO2,NOX,O2"))
				{
					Category.CheckCatalogResult = "A";
					Category.SetCheckParameter("Analyzer_Range_record_Valid", false, eParameterDataType.Boolean);
				}
			}
			catch (Exception ex)
			{ ReturnVal = Category.CheckEngine.FormatError(ex, "COMPON36"); }

			return ReturnVal;
		}

		public static string COMPON37(cCategory Category, ref bool Log) //Dual Range Indicator Consistent with Analyzer Range Code
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentAnalyzerRange = (DataRowView)Category.GetCheckParameter("Current_Analyzer_Range").ParameterValue;
				string AnalyzerRangeCode = cDBConvert.ToString(CurrentAnalyzerRange["Analyzer_range_cd"]);

				if (CurrentAnalyzerRange["dual_range_ind"] == DBNull.Value)
					Category.CheckCatalogResult = "A";
				else
				{
					int DualRangeIndicator = cDBConvert.ToInteger(CurrentAnalyzerRange["dual_range_ind"]);

					if ((MpParameters.CurrentComponent.ComponentTypeCd == "HG") && (DualRangeIndicator != 0))
					{
						Category.CheckCatalogResult = "C";
					}
					else
					{
						if (AnalyzerRangeCode == "A" && DualRangeIndicator != 1)
							Category.CheckCatalogResult = "B";
					}
				}

			}
			catch (Exception ex)
			{ ReturnVal = Category.CheckEngine.FormatError(ex, "COMPON37"); }

			return ReturnVal;
		}

		public static string COMPON38(cCategory Category, ref bool Log)
		// Analyzer Range Active Status
		{
			string ReturnVal = "";

			try
			{
				bool AnalyzerRangeDatesAndHoursConsistent = (bool)Category.GetCheckParameter("Analyzer_Range_Dates_And_Hours_Consistent").ParameterValue;

				if (AnalyzerRangeDatesAndHoursConsistent)
				{
					//Category.SetCheckParameter("Analyzer_Range_Active", null, ParameterTypes.BOOLEAN);

					ReturnVal = Check_ActiveHourRange(Category, "Analyzer_Range_Active",
																"Current_Analyzer_Range",
																"Analyzer_Range_Evaluation_Begin_Date",
																"Analyzer_Range_Evaluation_Begin_Hour",
																"Analyzer_Range_Evaluation_End_Date",
																"Analyzer_Range_Evaluation_End_Hour");
				}
				else
					Log = false;
			}
			catch (Exception ex)
			{ ReturnVal = Category.CheckEngine.FormatError(ex, "COMPON38"); }

			return ReturnVal;
		}

		public static string COMPON45(cCategory Category, ref bool Log)
		// Required High-Scale Span Reported for Component
		{
			string ReturnVal = "";

			try
			{
				int SpanCount = 0;
				Category.SetCheckParameter("Required_Span_Reported_for_Component_Type_and_Analyzer_Range",
								true, eParameterDataType.Boolean);
				DataRowView CurrentAnalyzerRange = (DataRowView)Category.GetCheckParameter("Current_Analyzer_Range").ParameterValue;
				DataRowView CurrentComponent = (DataRowView)Category.GetCheckParameter("Current_Component").ParameterValue;
				// bool AnalyzerRangeRecordValid = (bool)Category.GetCheckParameter("Analyzer_Range_Record_Valid").ParameterValue;
				string AnalyzerRangeCode = cDBConvert.ToString(CurrentAnalyzerRange["Analyzer_range_cd"]);
				bool AnalyzerRangeDatesAndHoursConsistent = cDBConvert.ToBoolean(Category.GetCheckParameter("Analyzer_Range_dates_and_hours_consistent").ParameterValue);
				string ComponentTypeCode = cDBConvert.ToString(CurrentComponent["component_type_cd"]);

				if (ComponentTypeCode.NotInList("HF,HCL"))
				{
					if (AnalyzerRangeCode.InList("H,A") && AnalyzerRangeDatesAndHoursConsistent)
					{
						DateTime AnalyzerRangeEvaluationBeginDate = (DateTime)Category.GetCheckParameter("analyzer_range_Evaluation_Begin_Date").ParameterValue;
						DateTime AnalyzerRangeEvaluationEndDate = (DateTime)Category.GetCheckParameter("analyzer_range_Evaluation_End_Date").ParameterValue;
						int AnalyzerRangeEvaluationBeginHour = cDBConvert.ToHour(Category.GetCheckParameter("analyzer_range_Evaluation_Begin_Hour").ParameterValue, DateTypes.START);
						int AnalyzerRangeEvaluationEndHour = cDBConvert.ToHour(Category.GetCheckParameter("analyzer_range_Evaluation_End_Hour").ParameterValue, DateTypes.END);
						DataView SpanRecords = (DataView)Category.GetCheckParameter("Span_Records").ParameterValue;


						string OldFilter = SpanRecords.RowFilter;

						SpanRecords.RowFilter = AddToDataViewFilter(OldFilter,
						  "component_type_cd = '" + ComponentTypeCode + "' and span_scale_Cd = 'H'");

						if (!(CheckForHourRangeCovered(Category, SpanRecords,
							AnalyzerRangeEvaluationBeginDate, AnalyzerRangeEvaluationBeginHour,
							AnalyzerRangeEvaluationEndDate, AnalyzerRangeEvaluationEndHour, ref SpanCount)))
						{
							if (SpanCount == 0)
							{
								Category.CheckCatalogResult = "A";
								Category.SetCheckParameter("Required_Span_Reported_for_Component_Type_and_Analyzer_Range",
									false, eParameterDataType.Boolean);
							}
							else
							{
								Category.CheckCatalogResult = "B";
								Category.SetCheckParameter("Required_Span_Reported_for_Component_Type_and_Analyzer_Range",
									false, eParameterDataType.Boolean);
							}
						}

						SpanRecords.RowFilter = OldFilter;

					}
					else
						Log = false;
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "COMPON45");
			}

			return ReturnVal;
		}

		public static string COMPON48(cCategory Category, ref bool Log)
		// Component and Analyzer Range Dates Consistent
		{
			string ReturnVal = "";

			try
			{
				bool AnalyzerRangeDatesAndHoursConsistent = (bool)Category.GetCheckParameter("Analyzer_Range_Dates_And_Hours_Consistent").ParameterValue;
				bool ComponentDatesAndHoursConsistent = cDBConvert.ToBoolean(Category.GetCheckParameter("component_dates_and_hours_consistent").ParameterValue);

				if (AnalyzerRangeDatesAndHoursConsistent && ComponentDatesAndHoursConsistent)
				{
					DateTime ComponentEvaluationBeginDate = (DateTime)Category.GetCheckParameter("Component_Evaluation_Begin_Date").ParameterValue;
					DateTime ComponentEvaluationEndDate = (DateTime)Category.GetCheckParameter("Component_Evaluation_End_Date").ParameterValue;
					int ComponentEvaluationBeginHour = cDBConvert.ToHour(Category.GetCheckParameter("Component_Evaluation_Begin_Hour").ParameterValue, DateTypes.START);
					int ComponentEvaluationEndHour = cDBConvert.ToHour(Category.GetCheckParameter("Component_Evaluation_End_Hour").ParameterValue, DateTypes.END);

					DateTime AnalyzerRangeEvaluationBeginDate = (DateTime)Category.GetCheckParameter("Analyzer_Range_Evaluation_Begin_Date").ParameterValue;
					DateTime AnalyzerRangeEvaluationEndDate = (DateTime)Category.GetCheckParameter("Analyzer_Range_Evaluation_End_Date").ParameterValue;
					int AnalyzerRangeEvaluationBeginHour = cDBConvert.ToHour(Category.GetCheckParameter("analyzer_range_Evaluation_Begin_Hour").ParameterValue, DateTypes.START);
					int AnalyzerRangeEvaluationEndHour = cDBConvert.ToHour(Category.GetCheckParameter("analyzer_range_Evaluation_End_Hour").ParameterValue, DateTypes.END);

					if ((ComponentEvaluationBeginDate > AnalyzerRangeEvaluationBeginDate) ||
						(ComponentEvaluationBeginDate == AnalyzerRangeEvaluationBeginDate && ComponentEvaluationBeginHour > AnalyzerRangeEvaluationBeginHour) ||
						(ComponentEvaluationEndDate < DateTime.MaxValue && AnalyzerRangeEvaluationEndDate == DateTime.MaxValue) ||
						(ComponentEvaluationEndDate < AnalyzerRangeEvaluationEndDate) ||
						(ComponentEvaluationEndDate == AnalyzerRangeEvaluationEndDate && ComponentEvaluationEndHour < AnalyzerRangeEvaluationEndHour))
						Category.CheckCatalogResult = "A";
				}
				else
					Log = false;
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "COMPON48");
			}

			return ReturnVal;
		}

		public static string COMPON51(cCategory Category, ref bool Log) //Required Low-Scale Span Reported for Component
		{
			string ReturnVal = "";

			try
			{
				int SpanCount = 0;

				DataRowView CurrentAnalyzerRange = (DataRowView)Category.GetCheckParameter("Current_Analyzer_Range").ParameterValue;
				DataRowView CurrentComponent = (DataRowView)Category.GetCheckParameter("Current_Component").ParameterValue;
				// bool AnalyzerRangeRecordValid = (bool)Category.GetCheckParameter("Analyzer_Range_Record_Valid").ParameterValue;
				string AnalyzerRangeCode = cDBConvert.ToString(CurrentAnalyzerRange["Analyzer_range_cd"]);
				bool AnalyzerRangeDatesAndHoursConsistent = cDBConvert.ToBoolean(Category.GetCheckParameter("Analyzer_Range_dates_and_hours_consistent").ParameterValue);

				if (AnalyzerRangeCode.InList("L,A") && AnalyzerRangeDatesAndHoursConsistent)
				{
					string ComponentTypeCode = cDBConvert.ToString(CurrentComponent["component_type_cd"]);
					if(ComponentTypeCode.NotInList("HCL,HF"))
					{
					DateTime AnalyzerRangeEvaluationBeginDate = (DateTime)Category.GetCheckParameter("analyzer_range_Evaluation_Begin_Date").ParameterValue;
					DateTime AnalyzerRangeEvaluationEndDate = (DateTime)Category.GetCheckParameter("analyzer_range_Evaluation_End_Date").ParameterValue;
					int AnalyzerRangeEvaluationBeginHour = cDBConvert.ToHour(Category.GetCheckParameter("analyzer_range_Evaluation_Begin_Hour").ParameterValue, DateTypes.START);
					int AnalyzerRangeEvaluationEndHour = cDBConvert.ToHour(Category.GetCheckParameter("analyzer_range_Evaluation_End_Hour").ParameterValue, DateTypes.END);
					DataView SpanRecords = (DataView)Category.GetCheckParameter("Span_Records").ParameterValue;
					

					string OldFilter = SpanRecords.RowFilter;

					SpanRecords.RowFilter = AddToDataViewFilter(OldFilter,
					  "component_type_cd = '" + ComponentTypeCode + "' and span_scale_Cd = 'L'");

					if (!(CheckForHourRangeCovered(Category, SpanRecords,
						AnalyzerRangeEvaluationBeginDate, AnalyzerRangeEvaluationBeginHour,
						AnalyzerRangeEvaluationEndDate, AnalyzerRangeEvaluationEndHour, ref SpanCount)))
					{
						if (SpanCount == 0)
							Category.CheckCatalogResult = "A";
						else
							Category.CheckCatalogResult = "B";
					}
					
					SpanRecords.RowFilter = OldFilter;
					}
				}
				else
					Log = false;
			}
			catch (Exception ex)
			{ ReturnVal = Category.CheckEngine.FormatError(ex, "COMPON51"); }

			return ReturnVal;
		}

		public static string COMPON54(cCategory Category, ref bool Log)
		// Duplicate Analyzer Range Records
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentAnalyzerRange = (DataRowView)Category.GetCheckParameter("Current_Analyzer_Range").ParameterValue;
				string AnalyzerRangeId = cDBConvert.ToString(CurrentAnalyzerRange["Analyzer_Range_Id"]);
				string ComponentId = cDBConvert.ToString(CurrentAnalyzerRange["Component_Id"]);

				DataView AnalyzerRangeRecords = (DataView)Category.GetCheckParameter("Location_Analyzer_Range_Records").ParameterValue;
				sFilterPair[] AnalyzerRangeFilter = new sFilterPair[4];

				AnalyzerRangeFilter[0].Set("Component_Id", ComponentId);
				AnalyzerRangeFilter[1].Set("Begin_Date", CurrentAnalyzerRange["Begin_Date"], eFilterDataType.DateBegan);
				AnalyzerRangeFilter[2].Set("Begin_Hour", CurrentAnalyzerRange["Begin_Hour"], eFilterDataType.Integer);
				AnalyzerRangeFilter[3].Set("Analyzer_Range_Id", AnalyzerRangeId, true);

				DataView AnalyzerRangeView = FindRows(AnalyzerRangeRecords, AnalyzerRangeFilter);

				if (AnalyzerRangeView.Count > 0)
					Category.CheckCatalogResult = "A";
				else if (CurrentAnalyzerRange["End_Date"] != DBNull.Value)
				{
					AnalyzerRangeFilter[1].Set("End_Date", CurrentAnalyzerRange["End_Date"], eFilterDataType.DateEnded);
					AnalyzerRangeFilter[2].Set("End_Hour", CurrentAnalyzerRange["End_Hour"], eFilterDataType.Integer);

					AnalyzerRangeView = FindRows(AnalyzerRangeRecords, AnalyzerRangeFilter);

					if (AnalyzerRangeView.Count > 0)
						Category.CheckCatalogResult = "A";
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "COMPON54");
			}

			return ReturnVal;
		}

		#endregion


		#region Component Checks

		public static string COMPON8(cCategory Category, ref bool Log)
		// Component Identifier Valid
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentComponent = (DataRowView)Category.GetCheckParameter("Current_Component").ParameterValue;
				string ComponentIdentifier = cDBConvert.ToString(CurrentComponent["Component_Identifier"]);

				if (CurrentComponent["Component_Identifier"] == DBNull.Value)
					Category.CheckCatalogResult = "A";
				else if (ComponentIdentifier.Length != 3 || !(cStringValidator.IsAlphaNumeric(ComponentIdentifier)))
					Category.CheckCatalogResult = "B";

			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "COMPON8");
			}

			return ReturnVal;
		}

		public static string COMPON10(cCategory Category, ref bool Log) //Component Serial Number Valid
		{
			string ReturnVal = "";

			try
			{
				bool ComponentComponentTypeValid = (bool)Category.GetCheckParameter("component_Component_Type_Valid").ParameterValue;

				if (ComponentComponentTypeValid)
				{
					DataRowView CurrentComponent = (DataRowView)Category.GetCheckParameter("Current_Component").ParameterValue;
					string ComponentTypeCode = cDBConvert.ToString(CurrentComponent["Component_Type_Cd"]);

					if (!(ComponentTypeCode.InList("BGFF,BOFF,TANK,DAHS,DL,PLC,FLC")) &&
						CurrentComponent["serial_number"] == DBNull.Value)
						Category.CheckCatalogResult = "A";
				}
				else
					Log = false;
			}
			catch (Exception ex)
			{ ReturnVal = Category.CheckEngine.FormatError(ex, "COMPON10"); }

			return ReturnVal;
		}

		public static string COMPON11(cCategory Category, ref bool Log) //Component Manufacturer Valid
		{
			string ReturnVal = "";

			try
			{
				bool ComponentComponentTypeValid = (bool)Category.GetCheckParameter("component_Component_Type_Valid").ParameterValue;

				if (ComponentComponentTypeValid)
				{
					DataRowView CurrentComponent = (DataRowView)Category.GetCheckParameter("Current_Component").ParameterValue;
					string ComponentTypeCode = cDBConvert.ToString(CurrentComponent["component_type_cd"]);

					if (!(ComponentTypeCode.InList("BGFF,BOFF,TANK")) && CurrentComponent["manufacturer"] == DBNull.Value)
						Category.CheckCatalogResult = "A";
				}
				else
					Log = false;
			}
			catch (Exception ex)
			{ ReturnVal = Category.CheckEngine.FormatError(ex, "COMPON11"); }

			return ReturnVal;
		}

		public static string COMPON12(cCategory Category, ref bool Log)
		// Component Type Code Valid
		{
			string ReturnVal = "";

			try
			{
				Category.SetCheckParameter("Component_Component_Type_Valid", true, eParameterDataType.Boolean);

				DataRowView CurrentComponent = (DataRowView)Category.GetCheckParameter("Current_Component").ParameterValue;

				if (CurrentComponent["Component_Type_Cd"] == DBNull.Value)
				{
					Category.SetCheckParameter("Component_Component_Type_Valid", false, eParameterDataType.Boolean);
					Category.CheckCatalogResult = "A";
				}
				else
				{
					string ComponentTypeCode = cDBConvert.ToString(CurrentComponent["Component_Type_Cd"]);
					DataView ComponentTypeCodeRecords = (DataView)Category.GetCheckParameter("Component_Type_Code_Lookup_Table").ParameterValue;

					if (!LookupCodeExists(ComponentTypeCode, ComponentTypeCodeRecords))
					{
						Category.SetCheckParameter("Component_Component_Type_Valid", false, eParameterDataType.Boolean);
						Category.CheckCatalogResult = "B";
					}
					else
					{
						string ComponentIdentifier = cDBConvert.ToString(CurrentComponent["Component_Identifier"]);
						DataView UsedIdentifierRecords = (DataView)Category.GetCheckParameter("Used_Identifier_Records").ParameterValue;
						DataRowView UsedIdentifierRow;
						sFilterPair[] UsedIdentifierFilter = new sFilterPair[2];

						UsedIdentifierFilter[0].Set("Table_Cd", "C");
						UsedIdentifierFilter[1].Set("Identifier", ComponentIdentifier);

						if (FindRow(UsedIdentifierRecords, UsedIdentifierFilter, out UsedIdentifierRow))
						{
							if (ComponentTypeCode != cDBConvert.ToString(UsedIdentifierRow["Type_Or_Parameter_Cd"]))
							{
								Category.SetCheckParameter("Component_Component_Type_Valid", false, eParameterDataType.Boolean);
								Category.CheckCatalogResult = "C";
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "COMPON12");
			}

			return ReturnVal;
		}

		public static string COMPON26(cCategory Category, ref bool Log) //Component Active Status
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentComponent = (DataRowView)Category.GetCheckParameter("Current_Component").ParameterValue;
				DataView SystemComponentRecords = (DataView)Category.GetCheckParameter("System_Component_Records").ParameterValue;
				bool ComponentActive = false;

				string OldFilter = SystemComponentRecords.RowFilter;

				SystemComponentRecords.RowFilter = AddEvaluationDateRangeToDataViewFilter(OldFilter,
				  Category.CheckEngine.EvalDefaultedBeganDate, Category.CheckEngine.EvalDefaultedEndedDate, false, true, false);

				SystemComponentRecords.RowFilter = AddToDataViewFilter(SystemComponentRecords.RowFilter, " end_date is null or (end_date >= begin_date) ");

				if (SystemComponentRecords.Count == 0)
				{
					ComponentActive = false;

					string ComponentID = cDBConvert.ToString(CurrentComponent["component_id"]);

					SystemComponentRecords.RowFilter = "component_id = '" + ComponentID + "'";

					if (SystemComponentRecords.Count == 0)
						Category.CheckCatalogResult = "A";
				}
				else
				{
					DateTime EvaluationBeginDate = DateTime.MinValue;
					DateTime EvaluationEndDate = DateTime.MaxValue;
					int EvaluationBeginHour = 0;
					int EvaluationEndHour = 23;

					ComponentActive = true;

					DateTime EarliestBeginDate = DateTime.MinValue;
					DateTime LatestEndDate = DateTime.MaxValue;
					int EarliestBeginHour = 0;
					int LatestEndHour = 23;

					DateTime StartDate = DateTime.MinValue;
					DateTime EndDate = DateTime.MaxValue;
					int StartHour = 0;
					int EndHour = 23;

					bool FirstSystemComponent = true;
					bool ComponentDatesAndHoursConsistent = true;

					foreach (DataRowView SystemComponentRecord in SystemComponentRecords)
					{
						StartDate = cDBConvert.ToDate(SystemComponentRecord["Begin_date"], DateTypes.START);
						StartHour = cDBConvert.ToHour(SystemComponentRecord["Begin_hour"], DateTypes.START);

						if (StartDate <= EarliestBeginDate || FirstSystemComponent)
						{
							if (StartDate == EarliestBeginDate && StartHour < EarliestBeginHour)
								EarliestBeginHour = StartHour;
							else if (StartDate < EarliestBeginDate || FirstSystemComponent)
							{
								EarliestBeginDate = StartDate;
								EarliestBeginHour = StartHour;
							}
						}

						EndDate = cDBConvert.ToDate(SystemComponentRecord["end_date"], DateTypes.END);
						EndHour = cDBConvert.ToHour(SystemComponentRecord["end_hour"], DateTypes.END);

						if (EndDate >= LatestEndDate || FirstSystemComponent)
						{
							if (EndDate == LatestEndDate && EndHour > LatestEndHour)
								LatestEndHour = EndHour;
							else if (EndDate > LatestEndDate || FirstSystemComponent)
							{
								LatestEndDate = EndDate;
								LatestEndHour = EndHour;
							}
						}

						if (StartDate > EndDate || (StartDate == EndDate && StartHour > EndHour))
							ComponentDatesAndHoursConsistent = false;

						FirstSystemComponent = false;
					}

					Category.SetCheckParameter("component_dates_and_hours_consistent", ComponentDatesAndHoursConsistent, eParameterDataType.Boolean);

					if (ComponentDatesAndHoursConsistent)
					{
						if (EarliestBeginDate >= Category.CheckEngine.EvalDefaultedBeganDate)
						{
							EvaluationBeginDate = EarliestBeginDate;
							EvaluationBeginHour = EarliestBeginHour;
						}
						else
						{
							EvaluationBeginDate = Category.CheckEngine.EvalDefaultedBeganDate;
							EvaluationBeginHour = 0;
						}

						if (LatestEndDate <= Category.CheckEngine.EvalDefaultedEndedDate)
						{
							EvaluationEndDate = LatestEndDate;
							EvaluationEndHour = LatestEndHour;
						}
						else
						{
							EvaluationEndDate = Category.CheckEngine.EvalDefaultedEndedDate;
							EvaluationEndHour = 23;
						}

						Category.SetCheckParameter("component_evaluation_begin_date", EvaluationBeginDate, eParameterDataType.Date);
						Category.SetCheckParameter("component_evaluation_begin_hour", EvaluationBeginHour, eParameterDataType.Integer);
						Category.SetCheckParameter("component_evaluation_end_date", EvaluationEndDate, eParameterDataType.Date);
						Category.SetCheckParameter("component_evaluation_end_hour", EvaluationEndHour, eParameterDataType.Integer);
					}
				}

				Category.SetCheckParameter("Component_active", ComponentActive, eParameterDataType.Boolean);

				SystemComponentRecords.RowFilter = OldFilter;
			}
			catch (Exception ex)
			{ ReturnVal = Category.CheckEngine.FormatError(ex, "COMPON26"); }

			return ReturnVal;
		}

		public static string COMPON30(cCategory Category, ref bool Log) //Required Analyzer Range Reported for Component
		{
			string ReturnVal = "";

			try
			{
				int AnalyzerRangeCount = 0;

				DataRowView CurrentComponent = (DataRowView)Category.GetCheckParameter("Current_Component").ParameterValue;
				string ComponentTypeCode = cDBConvert.ToString(CurrentComponent["component_Type_cd"]);
				bool ComponentDatesAndHoursConsistent = cDBConvert.ToBoolean(Category.GetCheckParameter("component_dates_and_hours_consistent").ParameterValue);

				if (ComponentTypeCode.InList("SO2,NOX,CO2,O2,HG") && ComponentDatesAndHoursConsistent)
				{
					DateTime ComponentEvaluationBeginDate = (DateTime)Category.GetCheckParameter("Component_Evaluation_Begin_Date").ParameterValue;
					DateTime ComponentEvaluationEndDate = (DateTime)Category.GetCheckParameter("Component_Evaluation_End_Date").ParameterValue;
					int ComponentEvaluationBeginHour = cDBConvert.ToHour(Category.GetCheckParameter("Component_Evaluation_Begin_Hour").ParameterValue, DateTypes.START);
					int ComponentEvaluationEndHour = cDBConvert.ToHour(Category.GetCheckParameter("Component_Evaluation_End_Hour").ParameterValue, DateTypes.END);
					DataView ComponentAnalyzerRangeRecords = (DataView)Category.GetCheckParameter("Component_Analyzer_Range_Records").ParameterValue;

					if (!(CheckForHourRangeCovered(Category, ComponentAnalyzerRangeRecords,
						ComponentEvaluationBeginDate, ComponentEvaluationBeginHour,
						ComponentEvaluationEndDate, ComponentEvaluationEndHour, ref AnalyzerRangeCount)))
					{
						if (AnalyzerRangeCount == 0)
							Category.CheckCatalogResult = "A";
						else
							Category.CheckCatalogResult = "B";
					}
				}
				else
					Log = false;
			}
			catch (Exception ex)
			{ ReturnVal = Category.CheckEngine.FormatError(ex, "COMPON30"); }

			return ReturnVal;
		}

		public static string COMPON47(cCategory Category, ref bool Log) //Duplicate Analyzer Range Reported for Component
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentComponent = (DataRowView)Category.GetCheckParameter("Current_Component").ParameterValue;
				string ComponentTypeCode = cDBConvert.ToString(CurrentComponent["component_Type_cd"]);
				bool ComponentDatesAndHoursConsistent = cDBConvert.ToBoolean(Category.GetCheckParameter("component_dates_and_hours_consistent").ParameterValue);

				if (ComponentTypeCode.InList("SO2,NOX,CO2,O2,HG,HCL,HF") && ComponentDatesAndHoursConsistent)
				{
					DateTime ComponentEvaluationBeginDate = (DateTime)Category.GetCheckParameter("Component_Evaluation_Begin_Date").ParameterValue;
					DateTime ComponentEvaluationEndDate = (DateTime)Category.GetCheckParameter("Component_Evaluation_End_Date").ParameterValue;
					int ComponentEvaluationBeginHour = cDBConvert.ToHour(Category.GetCheckParameter("Component_Evaluation_Begin_Hour").ParameterValue, DateTypes.START);
					int ComponentEvaluationEndHour = cDBConvert.ToHour(Category.GetCheckParameter("Component_Evaluation_End_Hour").ParameterValue, DateTypes.END);
					DataView ComponentAnalyzerRangeRecords = (DataView)Category.GetCheckParameter("Component_Analyzer_Range_Records").ParameterValue;

					string OldFilter = ComponentAnalyzerRangeRecords.RowFilter;

					if (!(CheckForDuplicateRecords(Category, ComponentAnalyzerRangeRecords, ComponentEvaluationBeginDate, ComponentEvaluationEndDate, ComponentEvaluationBeginHour, ComponentEvaluationEndHour)))
						Category.CheckCatalogResult = "A";
					ComponentAnalyzerRangeRecords.RowFilter = OldFilter;
				}
				else
					Log = false;
			}
			catch (Exception ex)
			{ ReturnVal = Category.CheckEngine.FormatError(ex, "COMPON47"); }

			return ReturnVal;
		}

		public static string COMPON52(cCategory Category, ref bool Log) //Required FLOW Span Reported for Component
		{
			string ReturnVal = "";

			try
			{
				int SpanCount = 0;

				DataRowView CurrentComponent = (DataRowView)Category.GetCheckParameter("Current_Component").ParameterValue;
				string ComponentTypeCode = cDBConvert.ToString(CurrentComponent["component_Type_cd"]);
				bool ComponentDatesAndHoursConsistent = cDBConvert.ToBoolean(Category.GetCheckParameter("Component_dates_and_hours_consistent").ParameterValue);

				if (ComponentTypeCode.InList("FLOW") && ComponentDatesAndHoursConsistent)
				{
					DateTime ComponentEvaluationBeginDate = (DateTime)Category.GetCheckParameter("Component_Evaluation_Begin_Date").ParameterValue;
					DateTime ComponentEvaluationEndDate = (DateTime)Category.GetCheckParameter("Component_Evaluation_End_Date").ParameterValue;
					int ComponentEvaluationBeginHour = cDBConvert.ToHour(Category.GetCheckParameter("Component_Evaluation_Begin_Hour").ParameterValue, DateTypes.START);
					int ComponentEvaluationEndHour = cDBConvert.ToHour(Category.GetCheckParameter("Component_Evaluation_End_Hour").ParameterValue, DateTypes.END);
					DataView SpanRecords = (DataView)Category.GetCheckParameter("Span_Records").ParameterValue;

					string OldFilter = SpanRecords.RowFilter;

					SpanRecords.RowFilter = AddToDataViewFilter(OldFilter, "component_type_cd = 'FLOW'");

					if (!(CheckForHourRangeCovered(Category, SpanRecords,
						ComponentEvaluationBeginDate, ComponentEvaluationBeginHour,
						ComponentEvaluationEndDate, ComponentEvaluationEndHour, ref SpanCount)))
					{
						if (SpanCount == 0)
							Category.CheckCatalogResult = "A";
						else
							Category.CheckCatalogResult = "B";
					}

					SpanRecords.RowFilter = OldFilter;
				}
				else
					Log = false;
			}
			catch (Exception ex)
			{ ReturnVal = Category.CheckEngine.FormatError(ex, "COMPON52"); }

			return ReturnVal;
		}

		public static string COMPON57(cCategory Category, ref bool Log) //Hg Converter Indicator Valid
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentComponent = (DataRowView)Category.GetCheckParameter("Current_Component").ParameterValue;
				bool ComponentComponentTypeValid = cDBConvert.ToBoolean(Category.GetCheckParameter("Component_Component_Type_Valid").ParameterValue);

				if (ComponentComponentTypeValid)
				{
					if (cDBConvert.ToInteger(CurrentComponent["hg_converter_ind"]) == 1 &&
						cDBConvert.ToString(CurrentComponent["component_type_cd"]) != "HG")
						Category.CheckCatalogResult = "A";
				}
				else
					Log = false;
			}
			catch (Exception ex)
			{ ReturnVal = Category.CheckEngine.FormatError(ex, "COMPON57"); }

			return ReturnVal;
		}

		public static string COMPON67(cCategory Category, ref bool Log) //Required Calibration Standard Data Reported for Component
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentComponent = (DataRowView)Category.GetCheckParameter("Current_Component").ParameterValue;
				string ComponentTypeCode = cDBConvert.ToString(CurrentComponent["component_Type_cd"]);
				bool ComponentDatesAndHoursConsistent = cDBConvert.ToBoolean(Category.GetCheckParameter("component_dates_and_hours_consistent").ParameterValue);

				if (ComponentTypeCode.InList("SO2,NOX,CO2,O2,HG") && ComponentDatesAndHoursConsistent)
				{

					DataView CalibrationStandardData = Category.GetCheckParameter("Calibration_Standard_Data_Records").ValueAsDataView();

					DateTime ComponentEvaluationBeginDate = (DateTime)Category.GetCheckParameter("Component_Evaluation_Begin_Date").ParameterValue;
					DateTime ComponentEvaluationEndDate = (DateTime)Category.GetCheckParameter("Component_Evaluation_End_Date").ParameterValue;
					int ComponentEvaluationBeginHour = cDBConvert.ToHour(Category.GetCheckParameter("Component_Evaluation_Begin_Hour").ParameterValue, DateTypes.START);
					int ComponentEvaluationEndHour = cDBConvert.ToHour(Category.GetCheckParameter("Component_Evaluation_End_Hour").ParameterValue, DateTypes.END);

					DateTime SpanDate = new DateTime(2009, 1, 1);
					int SpanHour = 0;

					if (ComponentEvaluationEndDate >= SpanDate)
					{
						string sfilter = CalibrationStandardData.RowFilter;
						CalibrationStandardData.RowFilter = AddEvaluationDateHourRangeToDataViewFilter(sfilter, ComponentEvaluationBeginDate, ComponentEvaluationEndDate,
															ComponentEvaluationBeginHour, ComponentEvaluationEndHour, false, true);


						if (CalibrationStandardData.Count == 0)
						{
							if (ComponentTypeCode == "HG")
								Category.CheckCatalogResult = "A";
							else
							{
								if (DateTime.Now >= SpanDate)
									Category.CheckCatalogResult = "A";
								else
									Category.CheckCatalogResult = "C";
							}

						}
						else
						{

							if (ComponentEvaluationBeginDate >= SpanDate)
							{
								SpanDate = ComponentEvaluationBeginDate;
								SpanHour = ComponentEvaluationBeginHour;
							}

							if (!CheckForHourRangeCovered(Category, CalibrationStandardData, SpanDate, SpanHour,
								ComponentEvaluationEndDate, ComponentEvaluationEndHour))
								Category.CheckCatalogResult = "B";

						}

						CalibrationStandardData.RowFilter = sfilter;
					}
				}
				else
					Log = false;

			}
			catch (Exception ex)
			{ ReturnVal = Category.CheckEngine.FormatError(ex, "COMPON67"); }

			return ReturnVal;
		}

		public static string COMPON69(cCategory Category, ref bool Log) //Overlapping Calibration Standard Data Reported for Component
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentComponent = (DataRowView)Category.GetCheckParameter("Current_Component").ParameterValue;
				string ComponentTypeCode = cDBConvert.ToString(CurrentComponent["component_Type_cd"]);
				bool ComponentDatesAndHoursConsistent = cDBConvert.ToBoolean(Category.GetCheckParameter("component_dates_and_hours_consistent").ParameterValue);

				if (ComponentTypeCode.InList("SO2,NOX,CO2,O2,HG") && ComponentDatesAndHoursConsistent)
				{
					DataView CalibrationStandardData = Category.GetCheckParameter("Calibration_Standard_Data_Records").ValueAsDataView();

					DateTime ComponentEvaluationBeginDate = (DateTime)Category.GetCheckParameter("Component_Evaluation_Begin_Date").ParameterValue;
					DateTime ComponentEvaluationEndDate = (DateTime)Category.GetCheckParameter("Component_Evaluation_End_Date").ParameterValue;
					int ComponentEvaluationBeginHour = cDBConvert.ToHour(Category.GetCheckParameter("Component_Evaluation_Begin_Hour").ParameterValue, DateTypes.START);
					int ComponentEvaluationEndHour = cDBConvert.ToHour(Category.GetCheckParameter("Component_Evaluation_End_Hour").ParameterValue, DateTypes.END);

					string sfilter = CalibrationStandardData.RowFilter;
					CalibrationStandardData.RowFilter = AddEvaluationDateHourRangeToDataViewFilter(sfilter, ComponentEvaluationBeginDate, ComponentEvaluationEndDate,
						ComponentEvaluationBeginHour, ComponentEvaluationEndHour, false, true);
					if (CalibrationStandardData.Count > 1)
					{
						if (!CheckForDuplicateRecords(Category, CalibrationStandardData, ComponentEvaluationBeginDate, ComponentEvaluationEndDate, ComponentEvaluationBeginHour, ComponentEvaluationEndHour))
							Category.CheckCatalogResult = "A";
					}
					CalibrationStandardData.RowFilter = sfilter;
				}

			}
			catch (Exception ex)
			{ ReturnVal = Category.CheckEngine.FormatError(ex, "COMPON69"); }

			return ReturnVal;
		}

		#endregion


		#region System Component Checks

		public static string COMPON3(cCategory Category, ref bool Log)
		// system component Begin Date Valid
		{
			string ReturnVal = "";

			try
			{
				Category.SetCheckParameter("system_component_Start_Date_valid", true, eParameterDataType.Boolean);

				ReturnVal = Check_ValidStartDate(Category, "System_Component_Start_Date_Valid",
														   "Current_System_Component");
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "COMPON3");
			}

			return ReturnVal;
		}

		public static string COMPON4(cCategory Category, ref bool Log)
		// system component Begin Hour Valid
		{
			string ReturnVal = "";

			try
			{
				Category.SetCheckParameter("system_component_Start_hour_valid", true, eParameterDataType.Boolean);

				ReturnVal = Check_ValidStartHour(Category, "System_Component_Start_Hour_Valid",
														   "Current_System_Component");
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "COMPON4");
			}

			return ReturnVal;
		}

		public static string COMPON5(cCategory Category, ref bool Log)
		// system component End Date Valid
		{
			string ReturnVal = "";

			try
			{
				Category.SetCheckParameter("system_component_end_Date_valid", true, eParameterDataType.Boolean);

				ReturnVal = Check_ValidEndDate(Category, "System_Component_End_Date_Valid",
														 "Current_System_Component");
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "COMPON5");
			}

			return ReturnVal;
		}

		public static string COMPON6(cCategory Category, ref bool Log)
		// system component End Hour Valid
		{
			string ReturnVal = "";

			try
			{
				Category.SetCheckParameter("system_component_end_hour_valid", true, eParameterDataType.Boolean);

				ReturnVal = Check_ValidEndHour(Category, "System_Component_End_Hour_Valid",
														   "Current_System_Component");
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "COMPON6");
			}

			return ReturnVal;
		}

		public static string COMPON7(cCategory Category, ref bool Log)
		// System Component Dates and Hours Consistent
		{
			string ReturnVal = "";

			try
			{
				Category.SetCheckParameter("System_Component_Dates_And_Hours_Consistent", true, eParameterDataType.Boolean);

				ReturnVal = Check_ConsistentHourRange(Category, "System_Component_Dates_And_Hours_Consistent",
																  "Current_System_Component",
																  "System_Component_Start_Date_Valid",
																  "System_Component_Start_Hour_Valid",
																  "System_Component_End_Date_Valid",
																  "System_Component_End_Hour_Valid");
			}
			catch (Exception ex)
			{ ReturnVal = Category.CheckEngine.FormatError(ex, "COMPON7"); }

			return ReturnVal;
		}

		public static string COMPON33(cCategory Category, ref bool Log)
		// System And Component Dates Consistent
		{
			string ReturnVal = "";

			try
			{
				bool SystemComponentRecordValid = (bool)Category.GetCheckParameter("System_Component_Record_valid").ParameterValue;
				bool SystemComponentDatesAndHoursConsistent = (bool)Category.GetCheckParameter("System_Component_Dates_And_Hours_Consistent").ParameterValue;
				bool SystemDatesAndHoursConsistent = (bool)Category.GetCheckParameter("System_Dates_And_Hours_Consistent").ParameterValue;

				if (SystemComponentRecordValid && SystemComponentDatesAndHoursConsistent && SystemDatesAndHoursConsistent)
				{
					DataRowView CurrentSystemComponent = (DataRowView)Category.GetCheckParameter("current_System_Component").ParameterValue;

					DateTime SystemComponentStartDate = cDBConvert.ToDate(CurrentSystemComponent["Begin_date"], DateTypes.START);
					DateTime SystemComponentEndDate = cDBConvert.ToDate(CurrentSystemComponent["end_date"], DateTypes.END);
					int SystemComponentStartHour = cDBConvert.ToHour(CurrentSystemComponent["Begin_hour"], DateTypes.START);
					int SystemComponentEndHour = cDBConvert.ToHour(CurrentSystemComponent["end_hour"], DateTypes.END);

					DataRowView CurrentMonitoringSystem = (DataRowView)Category.GetCheckParameter("Current_System").ParameterValue;

					DateTime SystemStartDate = cDBConvert.ToDate(CurrentMonitoringSystem["begin_date"], DateTypes.START);
					DateTime SystemEndDate = cDBConvert.ToDate(CurrentMonitoringSystem["end_date"], DateTypes.END);
					int SystemStartHour = cDBConvert.ToHour(CurrentMonitoringSystem["begin_hour"], DateTypes.START);
					int SystemEndHour = cDBConvert.ToHour(CurrentMonitoringSystem["end_hour"], DateTypes.END);
					//DateTime SystemStartDate = cDBConvert.ToDate(CurrentSystemComponent["System_Begin_date"], DateTypes.START);
					//DateTime SystemEndDate = cDBConvert.ToDate(CurrentSystemComponent["System_end_date"], DateTypes.END);
					//int SystemStartHour = cDBConvert.ToHour(CurrentSystemComponent["System_Begin_hour"], DateTypes.START);
					//int SystemEndHour = cDBConvert.ToHour(CurrentSystemComponent["System_end_hour"], DateTypes.END);

					if (SystemStartDate > SystemComponentStartDate)
						Category.CheckCatalogResult = "A";
					else if (SystemStartDate == SystemComponentStartDate && SystemStartHour > SystemComponentStartHour)
						Category.CheckCatalogResult = "A";
					else if (SystemEndDate != DateTime.MaxValue && SystemComponentEndDate == DateTime.MaxValue)
						Category.CheckCatalogResult = "A";
					else if (SystemEndDate < SystemComponentEndDate)
						Category.CheckCatalogResult = "A";
					else if (SystemEndDate == SystemComponentEndDate && SystemEndHour < SystemComponentEndHour)
						Category.CheckCatalogResult = "A";
				}
				else
					Log = false;
			}
			catch (Exception ex)
			{ ReturnVal = Category.CheckEngine.FormatError(ex, "COMPON33"); }

			return ReturnVal;

		}

		public static string COMPON39(cCategory Category, ref bool Log)
		// System Component Active Status
		{
			string ReturnVal = "";

			try
			{
				bool SystemComponentDatesAndHoursConsistent = (bool)Category.GetCheckParameter("System_Component_Dates_And_Hours_Consistent").ParameterValue;
				if (SystemComponentDatesAndHoursConsistent)
				{
					//Category.SetCheckParameter("System_Component_Active", true, ParameterTypes.BOOLEAN);

					ReturnVal = Check_ActiveHourRange(Category, "System_Component_Active",
																"Current_System_Component",
																"System_Component_Evaluation_Begin_Date",
																"System_Component_Evaluation_Begin_Hour",
																"System_Component_Evaluation_End_Date",
																"System_Component_Evaluation_End_Hour");
				}
				else
					Category.SetCheckParameter("System_Component_Active", false, eParameterDataType.Boolean);
			}
			catch (Exception ex)
			{ ReturnVal = Category.CheckEngine.FormatError(ex, "COMPON39"); }

			return ReturnVal;
		}

		public static string COMPON44(cCategory Category, ref bool Log)
		// Required Formula Reported for System and Component
		{
			string ReturnVal = "";

			try
			{
				if (cDBConvert.ToBoolean(Category.GetCheckParameter("System_Component_Record_Valid").ParameterValue) &&
					cDBConvert.ToBoolean(Category.GetCheckParameter("System_Component_Dates_and_Hours_Consistent").ParameterValue) &&
					cDBConvert.ToBoolean(Category.GetCheckParameter("System_Type_Code_Valid").ParameterValue))
				{
					string MissingFormulaForComponent = "";
					string IncompleteFormulaForComponent = "";

					DataRowView CurrentSystem = (DataRowView)Category.GetCheckParameter("Current_System").ParameterValue;
					DataRowView CurrentSystemComponent = (DataRowView)Category.GetCheckParameter("Current_System_Component").ParameterValue;
					DataView SysTypeToFormulaParamCrossCheck = (DataView)Category.GetCheckParameter("System_Type_To_Formula_Parameter_Cross_Check_Table").ParameterValue;

					DataView MethodRecords = (DataView)Category.GetCheckParameter("Method_Records").ParameterValue;
					DataView FormulaRecords = (DataView)Category.GetCheckParameter("Formula_Records").ParameterValue;
					DataView ParamAndCompTypeBasisToFormulaCrossCheck = (DataView)Category.GetCheckParameter("Formula_Parameter_And_Component_Type_And_Basis_To_Formula_Code_Cross_Check_Table").ParameterValue;
					DateTime SysCompEvalBeganDate = (DateTime)Category.GetCheckParameter("System_Component_Evaluation_Begin_Date").ParameterValue;
					int SysCompEvalBeganHour = cDBConvert.ToHour(Category.GetCheckParameter("System_Component_Evaluation_Begin_Hour").ParameterValue, DateTypes.START);
					DateTime SysCompEvalEndedDate = (DateTime)Category.GetCheckParameter("System_Component_Evaluation_End_Date").ParameterValue;
					int SysCompEvalEndedHour = cDBConvert.ToHour(Category.GetCheckParameter("System_Component_Evaluation_End_Hour").ParameterValue, DateTypes.END);

					string SystemTypeCd = cDBConvert.ToString(CurrentSystem["Sys_Type_Cd"]);
					string ComponentTypeCd = cDBConvert.ToString(CurrentSystemComponent["Component_Type_Cd"]);
					string BasisCd = cDBConvert.ToString(CurrentSystemComponent["Basis_Cd"]);
					sFilterPair[] Filter;

					DataRowView MethodRecord = null;

					if (SystemTypeCd == "CO2")
					{
						Filter = new sFilterPair[1];
						Filter[0].Set("parameter_cd", "CO2");
						DataView TempRecs = FindActiveRows(MethodRecords, SysCompEvalBeganDate, SysCompEvalBeganHour, SysCompEvalEndedDate, SysCompEvalEndedHour, Filter);
						if (TempRecs.Count > 0)
						{
							TempRecs.Sort = "begin_date,end_date";
							MethodRecord = TempRecs[0];
						}
					}

					if (SystemTypeCd == "CO2" && MethodRecord != null)
					{
						string FormulaFilter = FormulaRecords.RowFilter;
						if (ComponentTypeCd == "CO2")
						{
							Filter = new sFilterPair[2];
							Filter[0].Set("ParameterCode", "CO2,HI", eFilterPairStringCompare.InList);
							Filter[1].Set("ComponentTypeAndBasis", ComponentTypeCd + BasisCd);
							DataView ParamAndCompTypeBasisToFormulaView = FindRows(ParamAndCompTypeBasisToFormulaCrossCheck, Filter);

							if (ParamAndCompTypeBasisToFormulaView.Count > 0)
							{
								string sFilter = "";
								foreach (DataRowView Row in ParamAndCompTypeBasisToFormulaView)
								{
									sFilter = AddToDataViewFilter(sFilter,
																  "parameter_cd + isnull(equation_cd,'') = '" + cDBConvert.ToString(Row["ParameterCode"]) + cDBConvert.ToString(Row["FormulaCode"]) + "'",
																  true);
								}

								DateTime EvalBeganDate = cDBConvert.ToDate(MethodRecord["begin_date"], DateTypes.START);
								DateTime EvalEndedDate = cDBConvert.ToDate(MethodRecord["end_date"], DateTypes.END);
								int EvalBeganHour = cDBConvert.ToInteger(MethodRecord["begin_hour"]);
								int EvalEndedHour = cDBConvert.ToInteger(MethodRecord["end_hour"]);
								if (SysCompEvalBeganDate > EvalBeganDate)
								{
									EvalBeganDate = SysCompEvalBeganDate;
									EvalBeganHour = SysCompEvalBeganHour;
								}
								else if (SysCompEvalBeganDate == EvalBeganDate)
								{
									if (SysCompEvalBeganHour > EvalBeganHour)
									{
										EvalBeganDate = SysCompEvalBeganDate;
										EvalBeganHour = SysCompEvalBeganHour;
									}
								}
								if (SysCompEvalEndedDate < EvalEndedDate)
								{
									EvalEndedDate = SysCompEvalEndedDate;
									EvalEndedHour = SysCompEvalEndedHour;
								}
								else if (SysCompEvalEndedDate == EvalEndedDate)
								{
									if (SysCompEvalEndedHour < EvalEndedHour)
									{
										EvalEndedDate = SysCompEvalEndedDate;
										EvalEndedHour = SysCompEvalEndedHour;
									}
								}

								FormulaRecords.RowFilter = AddToDataViewFilter(FormulaRecords.RowFilter, sFilter);
								FormulaRecords.RowFilter = AddEvaluationDateHourRangeToDataViewFilter(FormulaRecords.RowFilter, EvalBeganDate, EvalEndedDate,
																									  EvalBeganHour, EvalEndedHour, false, true);
								string ParameterCdsFormulaCds = "";
								string Delim = "";

								foreach (DataRowView ParamFormRow in ParamAndCompTypeBasisToFormulaView)
								{
									ParameterCdsFormulaCds += Delim + cDBConvert.ToString(ParamFormRow["ParameterCode"]) + "/" + cDBConvert.ToString(ParamFormRow["FormulaCode"]);
									Delim = ",";
								}

								if (FormulaRecords.Count == 0)
									MissingFormulaForComponent = MissingFormulaForComponent.ListAdd(ParameterCdsFormulaCds);
								else if (!CheckForHourRangeCovered(Category, FormulaRecords,
																   EvalBeganDate, EvalBeganHour,
																   EvalEndedDate, EvalEndedHour))
									IncompleteFormulaForComponent = IncompleteFormulaForComponent.ListAdd(ParameterCdsFormulaCds);
							}
						}
						else if (ComponentTypeCd == "O2")
						{
							Filter = new sFilterPair[2];
							Filter[0].Set("ParameterCode", "CO2C,HI", eFilterPairStringCompare.InList);
							Filter[1].Set("ComponentTypeAndBasis", ComponentTypeCd + BasisCd);
							DataView ParamAndCompTypeBasisToFormulaView = FindRows(ParamAndCompTypeBasisToFormulaCrossCheck, Filter);

							if (ParamAndCompTypeBasisToFormulaView.Count > 0)
							{
								string sFilter = "";
								foreach (DataRowView Row in ParamAndCompTypeBasisToFormulaView)
								{
									sFilter = AddToDataViewFilter(sFilter,
																  "parameter_cd + isnull(equation_cd,'') = '" + cDBConvert.ToString(Row["ParameterCode"]) + cDBConvert.ToString(Row["FormulaCode"]) + "'",
																  true);
								}

								DateTime EvalBeganDate = cDBConvert.ToDate(MethodRecord["begin_date"], DateTypes.START);
								DateTime EvalEndedDate = cDBConvert.ToDate(MethodRecord["end_date"], DateTypes.END);
								int EvalBeganHour = cDBConvert.ToInteger(MethodRecord["begin_hour"]);
								int EvalEndedHour = cDBConvert.ToInteger(MethodRecord["end_hour"]);
								if (SysCompEvalBeganDate > EvalBeganDate)
								{
									EvalBeganDate = SysCompEvalBeganDate;
									EvalBeganHour = SysCompEvalBeganHour;
								}
								else if (SysCompEvalBeganDate == EvalBeganDate)
								{
									if (SysCompEvalBeganHour > EvalBeganHour)
									{
										EvalBeganDate = SysCompEvalBeganDate;
										EvalBeganHour = SysCompEvalBeganHour;
									}
								}
								if (SysCompEvalEndedDate < EvalEndedDate)
								{
									EvalEndedDate = SysCompEvalEndedDate;
									EvalEndedHour = SysCompEvalEndedHour;
								}
								else if (SysCompEvalEndedDate == EvalEndedDate)
								{
									if (SysCompEvalEndedHour < EvalEndedHour)
									{
										EvalEndedDate = SysCompEvalEndedDate;
										EvalEndedHour = SysCompEvalEndedHour;
									}
								}

								FormulaRecords.RowFilter = AddToDataViewFilter(FormulaRecords.RowFilter, sFilter);
								FormulaRecords.RowFilter = AddEvaluationDateHourRangeToDataViewFilter(FormulaRecords.RowFilter, EvalBeganDate, EvalEndedDate,
																									  EvalBeganHour, EvalEndedHour, false, true);

								string ParameterCdsFormulaCds = "";
								string Delim = "";

								foreach (DataRowView ParamFormRow in ParamAndCompTypeBasisToFormulaView)
								{
									ParameterCdsFormulaCds += Delim + cDBConvert.ToString(ParamFormRow["ParameterCode"]) + "/" + cDBConvert.ToString(ParamFormRow["FormulaCode"]);
									Delim = ",";
								}
								if (FormulaRecords.Count == 0)
									MissingFormulaForComponent = MissingFormulaForComponent.ListAdd(ParameterCdsFormulaCds);
								else
								{
									if (!CheckForHourRangeCovered(Category, FormulaRecords,
																   EvalBeganDate, EvalBeganHour,
																   EvalEndedDate, EvalEndedHour))
										IncompleteFormulaForComponent = IncompleteFormulaForComponent.ListAdd(ParameterCdsFormulaCds);

									FormulaRecords.RowFilter = AddToDataViewFilter(FormulaRecords.RowFilter, "parameter_cd = 'CO2C'");
									if (FormulaRecords.Count > 0)
									{
										Filter = new sFilterPair[2];
										Filter[0].Set("ParameterCode", "CO2");
										Filter[1].Set("ComponentTypeAndBasis", ComponentTypeCd + BasisCd);
										DataView CO2ParamAndCompTypeBasisToFormulaView = FindRows(ParamAndCompTypeBasisToFormulaCrossCheck, Filter);

										if (CO2ParamAndCompTypeBasisToFormulaView.Count > 0)
										{
											string CO2FormulaCodes = ColumnToDatalist(CO2ParamAndCompTypeBasisToFormulaView, "FormulaCode");

											Filter = new sFilterPair[2];
											Filter[0].Set("Parameter_Cd", "CO2");
											Filter[1].Set("Equation_Cd", CO2FormulaCodes, eFilterPairStringCompare.InList);

											FormulaRecords.RowFilter = FormulaFilter;
											DataView FormulaView = FindActiveRows(FormulaRecords,
																				  EvalBeganDate, EvalBeganHour,
																				  EvalEndedDate, EvalEndedHour,
																				  Filter);

											if (FormulaView.Count == 0)
												MissingFormulaForComponent = MissingFormulaForComponent.ListAdd("CO2/" + CO2FormulaCodes);
											else if (!CheckForHourRangeCovered(Category, FormulaView,
																			   EvalBeganDate, EvalBeganHour,
																			   EvalEndedDate, EvalEndedHour))
												IncompleteFormulaForComponent = IncompleteFormulaForComponent.ListAdd("CO2/" + CO2FormulaCodes);
										}

									}
								}

							}
						}
						FormulaRecords.RowFilter = FormulaFilter;
						Category.SetCheckParameter("Missing_Formula_For_Component", MissingFormulaForComponent.FormatList(true), eParameterDataType.String);
						Category.SetCheckParameter("Incomplete_Formula_For_Component", IncompleteFormulaForComponent.FormatList(true), eParameterDataType.String);
					}
					else
					{
						sFilterPair[] RowFilter;
						RowFilter = new sFilterPair[1];
						RowFilter[0].Set("SystemTypeCode", SystemTypeCd);

						DataView SysTypeToFormulaParamView = FindRows(SysTypeToFormulaParamCrossCheck, RowFilter);

						if (SysTypeToFormulaParamView.Count > 0)
						{

							foreach (DataRowView SysTypeToFormulaParamRow in SysTypeToFormulaParamView)
							{
								MethodRecord = null;
								string optionalCode = cDBConvert.ToString(SysTypeToFormulaParamRow["Optional"]);
								string[] delim = { "/" };
								string[] splitOption = optionalCode.Split(delim, StringSplitOptions.None);

								if (SysTypeToFormulaParamRow["Optional"] != DBNull.Value && SysTypeToFormulaParamRow["Optional"].ToString() != string.Empty)
								{
									Filter = new sFilterPair[2];
									Filter[0].Set("PARAMETER_CD", splitOption[0]);
									Filter[1].Set("METHOD_CD", (splitOption.Length > 1 ? splitOption[1] : string.Empty));
									DataView MethodView = FindActiveRows(MethodRecords, SysCompEvalBeganDate, SysCompEvalBeganHour, SysCompEvalEndedDate, SysCompEvalEndedHour, Filter);

									if (MethodView.Count > 0)
									{
										MethodView.Sort = "begin_date,end_date";
										MethodRecord = MethodView[0];
									}
								}

								if ((SysTypeToFormulaParamRow["Optional"] == DBNull.Value) || (SysTypeToFormulaParamRow["Optional"].ToString() == string.Empty) || MethodRecord != null)
								{
									string ParameterCd = cDBConvert.ToString(SysTypeToFormulaParamRow["ParameterCode"]);

									RowFilter = new sFilterPair[2];
									RowFilter[0].Set("ParameterCode", ParameterCd);
									RowFilter[1].Set("ComponentTypeAndBasis", ComponentTypeCd + BasisCd);

									DataView ParamAndCompTypeBasisToFormulaView = FindRows(ParamAndCompTypeBasisToFormulaCrossCheck, RowFilter);

									if (ParamAndCompTypeBasisToFormulaView.Count > 0)
									{
										string FormulaCodes = ColumnToDatalist(ParamAndCompTypeBasisToFormulaView, "FormulaCode");
										RowFilter = new sFilterPair[2];
										RowFilter[0].Set("Parameter_Cd", ParameterCd);
										RowFilter[1].Set("Equation_Cd", FormulaCodes, eFilterPairStringCompare.InList);

										DateTime EvalBeganDate;
										DateTime EvalEndedDate;
										int EvalBeganHour;
										int EvalEndedHour;

										if (MethodRecord != null)
										{
											EvalBeganDate = cDBConvert.ToDate(MethodRecord["begin_date"], DateTypes.START);
											EvalEndedDate = cDBConvert.ToDate(MethodRecord["end_date"], DateTypes.END);
											EvalBeganHour = cDBConvert.ToInteger(MethodRecord["begin_hour"]);
											EvalEndedHour = cDBConvert.ToInteger(MethodRecord["end_hour"]);
											if (SysCompEvalBeganDate > EvalBeganDate)
											{
												EvalBeganDate = SysCompEvalBeganDate;
												EvalBeganHour = SysCompEvalBeganHour;
											}
											else if (SysCompEvalBeganDate == EvalBeganDate)
											{
												if (SysCompEvalBeganHour > EvalBeganHour)
												{
													EvalBeganDate = SysCompEvalBeganDate;
													EvalBeganHour = SysCompEvalBeganHour;
												}
											}
											if (SysCompEvalEndedDate < EvalEndedDate)
											{
												EvalEndedDate = SysCompEvalEndedDate;
												EvalEndedHour = SysCompEvalEndedHour;
											}
											else if (SysCompEvalEndedDate == EvalEndedDate)
											{
												if (SysCompEvalEndedHour < EvalEndedHour)
												{
													EvalEndedDate = SysCompEvalEndedDate;
													EvalEndedHour = SysCompEvalEndedHour;
												}
											}

										}
										else
										{
											EvalBeganDate = SysCompEvalBeganDate;
											EvalBeganHour = SysCompEvalBeganHour;
											EvalEndedDate = SysCompEvalEndedDate;
											EvalEndedHour = SysCompEvalEndedHour;
										}
										DataView FormulaView = FindActiveRows(FormulaRecords,
																				 EvalBeganDate, EvalBeganHour,
																				 EvalEndedDate, EvalEndedHour,
																				 RowFilter);

										if (FormulaView.Count == 0)
											MissingFormulaForComponent = MissingFormulaForComponent.ListAdd(ParameterCd + "/" + FormulaCodes.FormatList(true), " and ", false);
										else if (!CheckForHourRangeCovered(Category, FormulaView,
																		   EvalBeganDate, EvalBeganHour,
																		   EvalEndedDate, EvalEndedHour))
											IncompleteFormulaForComponent = IncompleteFormulaForComponent.ListAdd(ParameterCd + "/" + FormulaCodes.FormatList(true), " and ", false);

									}
								}
							}
						}
						Category.SetCheckParameter("Missing_Formula_For_Component", MissingFormulaForComponent, eParameterDataType.String);
						Category.SetCheckParameter("Incomplete_Formula_For_Component", IncompleteFormulaForComponent, eParameterDataType.String);
					}

					if (MissingFormulaForComponent != "" && IncompleteFormulaForComponent == "")
						Category.CheckCatalogResult = "A";
					else if (IncompleteFormulaForComponent != "" && MissingFormulaForComponent == "")
						Category.CheckCatalogResult = "B";
					else if (MissingFormulaForComponent != "" && IncompleteFormulaForComponent != "")
						Category.CheckCatalogResult = "C";
				}
				else
					Log = false;
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "COMPON44");
			}

			return ReturnVal;
		}

		public static string COMPON46(cCategory Category, ref bool Log)
		// System Type Consistent with Component Type
		{
			string ReturnVal = "";

			try
			{
				if (!Category.GetCheckParameter("System_Record_Valid").ValueAsBool())
					Category.SetCheckParameter("System_Component_Record_Valid", false, eParameterDataType.Boolean);
				else
				{
					Category.SetCheckParameter("System_Component_Record_Valid", true, eParameterDataType.Boolean);

					DataRowView CurrentSystem = (DataRowView)Category.GetCheckParameter("Current_System").ParameterValue;
					DataRowView CurrentSystemComponent = Category.GetCheckParameter("Current_System_Component").ValueAsDataRowView();

					string ComponentTypeCode = cDBConvert.ToString(CurrentSystemComponent["Component_Type_Cd"]);
					DateTime EndDate = cDBConvert.ToDate(CurrentSystemComponent["End_Date"], DateTypes.END);

					if (!(ComponentTypeCode.InList("DAHS,PLC,DL")) && (EndDate >= new DateTime(2001, 1, 1)))
					{
						sFilterPair[] RowFilter;

						string SystemTypeCode = cDBConvert.ToString(CurrentSystem["Sys_Type_Cd"]);

						RowFilter = new sFilterPair[2];
						RowFilter[0].Set("SystemTypeCode", SystemTypeCode);
						RowFilter[1].Set("ComponentTypeCode", ComponentTypeCode);

						if (CountRows(Category.GetCheckParameter("System_Type_To_Component_Type_Cross_Check_Table").ValueAsDataView(), RowFilter) == 0)
						{
							RowFilter[1].Set("OptionalComponentTypeCode", ComponentTypeCode);

							if (CountRows(Category.GetCheckParameter("System_Type_To_Optional_Component_Type_Cross_Check_Table").ValueAsDataView(), RowFilter) == 0)
							{
								RowFilter = new sFilterPair[1];
								RowFilter[0].Set("Component_Type_Cd", ComponentTypeCode);

								if (CountRows(Category.GetCheckParameter("Component_Type_Code_Lookup_Table").ValueAsDataView(), RowFilter) > 0)
								{
									Category.SetCheckParameter("System_Component_Record_Valid", false, eParameterDataType.Boolean);
									Category.CheckCatalogResult = "A";
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "COMPON46");
			}

			return ReturnVal;
		}

		public static string COMPON53(cCategory Category, ref bool Log)
		// Duplicate Component Records
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentComponent = (DataRowView)Category.GetCheckParameter("Current_Component").ParameterValue;
				string ComponentId = cDBConvert.ToString(CurrentComponent["Component_Id"]);
				string ComponentIdentifier = cDBConvert.ToString(CurrentComponent["Component_Identifier"]);

				DataView ComponentRecords = (DataView)Category.GetCheckParameter("Component_Records").ParameterValue;
				sFilterPair[] ComponentFilter = new sFilterPair[2];

				ComponentFilter[0].Set("Component_Identifier", ComponentIdentifier);
				ComponentFilter[1].Set("Component_Id", ComponentId, true);

				DataView ComponentView = FindRows(ComponentRecords, ComponentFilter);

				if (ComponentView.Count > 0)
					Category.CheckCatalogResult = "A";
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "COMPON53");
			}

			return ReturnVal;
		}

		public static string COMPON55(cCategory Category, ref bool Log)
		// System Component Record Valid
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentSystemComponent = (DataRowView)Category.GetCheckParameter("Current_System_Component").ParameterValue;

				if (CurrentSystemComponent["Component_Id"] == DBNull.Value)
					Category.CheckCatalogResult = "A";
				else
				{
					string SystemID = cDBConvert.ToString(CurrentSystemComponent["Mon_Sys_Id"]);
					string ComponentID = cDBConvert.ToString(CurrentSystemComponent["Component_Id"]);
					DateTime EndDate = cDBConvert.ToDate(CurrentSystemComponent["END_DATE"], DateTypes.END);
					DateTime BeginDate = cDBConvert.ToDate(CurrentSystemComponent["BEGIN_DATE"], DateTypes.START);
					int EndHour = cDBConvert.ToHour(CurrentSystemComponent["END_HOUR"], DateTypes.END);
					int BeginHour = cDBConvert.ToHour(CurrentSystemComponent["BEGIN_HOUR"], DateTypes.START);

					bool bNullEndDate = (CurrentSystemComponent["END_DATE"] == DBNull.Value);

					DataView MonitorSystemComponentRecords = (DataView)Category.GetCheckParameter("Location_System_Component_Records").ParameterValue;
					string sMSCFilter = MonitorSystemComponentRecords.RowFilter;

					string sFilter = AddToDataViewFilter(sMSCFilter, string.Format("MON_SYS_ID = '{0}'", SystemID));
					sFilter = AddToDataViewFilter(sFilter, string.Format("COMPONENT_ID = '{0}'", ComponentID));
					sFilter = AddToDataViewFilter(sFilter, string.Format("BEGIN_DATE = '{0}' AND BEGIN_HOUR={1}", BeginDate.ToShortDateString(), BeginHour));
					MonitorSystemComponentRecords.RowFilter = sFilter;

					// we expect to find one row, the current row, is there more than 1?
					// or we are a new record and the count is equal to 1
					if ((MonitorSystemComponentRecords.Count > 1) || (MonitorSystemComponentRecords.Count == 1 && CurrentSystemComponent["MON_SYS_COMP_ID"] == DBNull.Value))
					{
						Category.CheckCatalogResult = "B";
					}
					else if (!bNullEndDate)
					{
						sFilter = AddToDataViewFilter(sMSCFilter, string.Format("MON_SYS_ID = '{0}'", SystemID));
						sFilter = AddToDataViewFilter(sFilter, string.Format("COMPONENT_ID = '{0}'", ComponentID));
						sFilter = AddToDataViewFilter(sFilter, string.Format("END_DATE = '{0}' AND END_HOUR={1}", EndDate.ToShortDateString(), EndHour));
						MonitorSystemComponentRecords.RowFilter = sFilter;

						if ((MonitorSystemComponentRecords.Count > 1) || (MonitorSystemComponentRecords.Count == 1 && CurrentSystemComponent["MON_SYS_COMP_ID"] == DBNull.Value))
							Category.CheckCatalogResult = "B";
					}
					// reset this sucker
					MonitorSystemComponentRecords.RowFilter = sMSCFilter;
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "COMPON55");
			}

			return ReturnVal;
		}

		public static string COMPON56(cCategory Category, ref bool Log)
		// Required Default Reported for System and Component
		{
			string ReturnVal = "";

			try
			{
				bool SystemComponentRecordValid = (bool)Category.GetCheckParameter("System_Component_Record_Valid").ParameterValue;

				if (SystemComponentRecordValid)
				{
					if (cDBConvert.ToBoolean(Category.GetCheckParameter("System_Component_Dates_And_Hours_Consistent").ParameterValue))
					{
						DataRowView CurrentSystemComponent = (DataRowView)Category.GetCheckParameter("Current_System_Component").ParameterValue;
						string ComponentTypeCd = cDBConvert.ToString(CurrentSystemComponent["Component_Type_Cd"]);
						DataRowView CurrentSystem = (DataRowView)Category.GetCheckParameter("Current_System").ParameterValue;
						string SystemTypeCd = cDBConvert.ToString(CurrentSystem["Sys_Type_Cd"]);

						if ((SystemTypeCd == "CO2") && (ComponentTypeCd == "O2"))
						{
							Category.SetCheckParameter("Missing_Default_For_System_And_Component", "CO2X Default for Purpose MD and Fuel NFS", eParameterDataType.String);

							DateTime EvalBeganDate = (DateTime)Category.GetCheckParameter("System_Component_Evaluation_Begin_Date").ParameterValue;
							int EvalBeganHour = cDBConvert.ToHour(Category.GetCheckParameter("System_Component_Evaluation_Begin_Hour").ParameterValue, DateTypes.START);
							DateTime EvalEndedDate = (DateTime)Category.GetCheckParameter("System_Component_Evaluation_End_Date").ParameterValue;
							int EvalEndedHour = cDBConvert.ToHour(Category.GetCheckParameter("System_Component_Evaluation_End_Hour").ParameterValue, DateTypes.END);

							DataView DefaultRecords = (DataView)Category.GetCheckParameter("Default_Records").ParameterValue;
							sFilterPair[] DefaultFilter = new sFilterPair[3];

							DefaultFilter[0].Set("Parameter_Cd", "CO2X");
							DefaultFilter[1].Set("Default_Purpose_Cd", "MD");
							DefaultFilter[2].Set("Fuel_Cd", "NFS");

							DataView DefaultView = FindActiveRows(DefaultRecords,
																  EvalBeganDate, EvalBeganHour, EvalEndedDate, EvalEndedHour,
																  DefaultFilter);

							if (DefaultView.Count == 0 || !CheckForHourRangeCovered(Category, DefaultView,
															   EvalBeganDate, EvalBeganHour, EvalEndedDate, EvalEndedHour))
							{
								DataView SpanRecords = (DataView)Category.GetCheckParameter("Span_Records").ParameterValue;
								sFilterPair[] SpanFilter = new sFilterPair[2];

								SpanFilter[0].Set("Component_Type_Cd", "CO2");
								SpanFilter[1].Set("Span_Scale_Cd", "H");

								DataView SpanView = FindActiveRows(SpanRecords,
																	EvalBeganDate, EvalBeganHour, EvalEndedDate, EvalEndedHour,
																	SpanFilter);

								if (SpanView.Count == 0 && DefaultView.Count == 0)
									Category.CheckCatalogResult = "A";
								else
								{
									if (SpanView.Count == 0)
									{
										if (!CheckForHourRangeCovered(Category, DefaultView,
																   EvalBeganDate, EvalBeganHour, EvalEndedDate, EvalEndedHour))
											Category.CheckCatalogResult = "B";
									}
									else
									{
										if (DefaultView.Count == 0)
										{
											if (!CheckForHourRangeCovered(Category, SpanView,
																EvalBeganDate, EvalBeganHour, EvalEndedDate, EvalEndedHour))
												Category.CheckCatalogResult = "B";
										}
										else
										{
											if (!CheckForDateRangeCovered(Category, DefaultView, SpanView,
																  EvalBeganDate, EvalEndedDate, false))
												Category.CheckCatalogResult = "B";
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
				ReturnVal = Category.CheckEngine.FormatError(ex, "COMPON56");
			}

			return ReturnVal;
		}

		#endregion


		#region Component Calibration Standard Data

		public static string COMPON58(cCategory Category, ref bool Log) //Calibration Standard Data Active Status
		{
			string ReturnVal = "";

			try
			{
				//DataRowView CurrentCalibration = (DataRowView)Category.GetCheckParameter("Current_Calibration_Standard_Data").ParameterValue;
				bool CalibrationStandardDatesAndHoursConsistent = cDBConvert.ToBoolean(Category.GetCheckParameter("Calibration_Standard_Data_Dates_and_Hours_Consistent").ParameterValue);

				if (CalibrationStandardDatesAndHoursConsistent)
					ReturnVal = Check_ActiveHourRange(Category, "Current_Calibration_Standard_Data_Active", "Current_Calibration_Standard_Data",
						"Calibration_Standard_Data_Evaluation_Begin_Date", "Calibration_Standard_Data_Evaluation_Begin_Hour",
						"Calibration_Standard_Data_Evaluation_End_Date", "Calibration_Standard_Data_Evaluation_End_Hour");


			}
			catch (Exception ex)
			{ ReturnVal = Category.CheckEngine.FormatError(ex, "COMPON58"); }

			return ReturnVal;
		}

		public static string COMPON59(cCategory Category, ref bool Log) //Calibration Standard Data Begin Date Valid
		{
			string ReturnVal = "";

			try
			{
				//DataRowView CurrentCalibration = (DataRowView)Category.GetCheckParameter("Current_Calibration_Standard_Data").ParameterValue;
				Category.SetCheckParameter("Calibration_Standard_Data_Start_Date_Valid", null, eParameterDataType.Boolean);

				ReturnVal = Check_ValidStartDate(Category, "Calibration_Standard_Data_Start_Date_Valid", "Current_Calibration_Standard_Data",
					"begin_date", "A", "B", "COMPON59");

			}
			catch (Exception ex)
			{ ReturnVal = Category.CheckEngine.FormatError(ex, "COMPON59"); }

			return ReturnVal;
		}

		public static string COMPON60(cCategory Category, ref bool Log) //Calibration Standard Data Begin Hour Valid
		{
			string ReturnVal = "";

			try
			{
				//DataRowView CurrentCalibration = (DataRowView)Category.GetCheckParameter("Current_Calibration_Standard_Data").ParameterValue;
				Category.SetCheckParameter("Calibration_Standard_Data_Start_Hour_Valid", null, eParameterDataType.Boolean);

				ReturnVal = Check_ValidStartHour(Category, "Calibration_Standard_Data_Start_Hour_Valid", "Current_Calibration_Standard_Data", "begin_hour", "A", "B", "COMPON60");

			}
			catch (Exception ex)
			{ ReturnVal = Category.CheckEngine.FormatError(ex, "COMPON60"); }

			return ReturnVal;
		}

		public static string COMPON61(cCategory Category, ref bool Log) //Calibration Standard Data End Date Valid
		{
			string ReturnVal = "";

			try
			{
				//DataRowView CurrentCalibration = (DataRowView)Category.GetCheckParameter("Current_Calibration_Standard_Data").ParameterValue;
				Category.SetCheckParameter("Calibration_Standard_Data_End_Date_Valid", null, eParameterDataType.Boolean);

				ReturnVal = Check_ValidEndDate(Category, "Calibration_Standard_Data_End_Date_Valid", "Current_Calibration_Standard_Data", "end_date", "A", "COMPON61");


			}
			catch (Exception ex)
			{ ReturnVal = Category.CheckEngine.FormatError(ex, "COMPON61"); }

			return ReturnVal;
		}

		public static string COMPON62(cCategory Category, ref bool Log) //Calibration Standard Data End Hour Valid
		{
			string ReturnVal = "";

			try
			{
				//DataRowView CurrentCalibration = (DataRowView)Category.GetCheckParameter("Current_Calibration_Standard_Data").ParameterValue;
				Category.SetCheckParameter("Calibration_Standard_Data_End_Hour_Valid", null, eParameterDataType.Boolean);

				ReturnVal = Check_ValidEndHour(Category, "Calibration_Standard_Data_End_Hour_Valid", "Current_Calibration_Standard_Data", "end_hour", "A", "COMPON62");

			}
			catch (Exception ex)
			{ ReturnVal = Category.CheckEngine.FormatError(ex, "COMPON62"); }

			return ReturnVal;
		}

		public static string COMPON63(cCategory Category, ref bool Log) //Calibration Standard Data Dates and Hours Consistent
		{
			string ReturnVal = "";

			try
			{
				//DataRowView CurrentCalibration = (DataRowView)Category.GetCheckParameter("Current_Calibration_Standard_Data").ParameterValue;
				ReturnVal = Check_ConsistentHourRange(Category, "Calibration_Standard_Data_Dates_and_Hours_Consistent", "Current_Calibration_Standard_Data",
					"Calibration_Standard_Data_Start_Date_Valid", "Calibration_Standard_Data_Start_Hour_Valid",
					"Calibration_Standard_Data_End_Date_Valid", "Calibration_Standard_Data_End_Hour_Valid", "begin_date", "begin_hour", "end_date", "end_hour",
					"A", "B", "C", "COMPON63");

			}
			catch (Exception ex)
			{ ReturnVal = Category.CheckEngine.FormatError(ex, "COMPON63"); }

			return ReturnVal;
		}

		public static string COMPON64(cCategory Category, ref bool Log) //Calibration Standard Code Valid
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentCalibration = (DataRowView)Category.GetCheckParameter("Current_Calibration_Standard_Data").ParameterValue;
				DataRowView CurrentComponent = (DataRowView)Category.GetCheckParameter("Current_Component").ParameterValue;

				Category.SetCheckParameter("Calibration_Standard_Code_Valid", true, eParameterDataType.Boolean);

				if (CurrentCalibration["cal_standard_cd"] == DBNull.Value)
				{
					Category.CheckCatalogResult = "A";
					Category.SetCheckParameter("Calibration_Standard_Code_Valid", false, eParameterDataType.Boolean);
				}
				else
				{
					string calStdCd = cDBConvert.ToString(CurrentCalibration["cal_standard_cd"]);
					string componTypeCd = cDBConvert.ToString(CurrentComponent["component_type_cd"]);
					if (componTypeCd == "HG" && calStdCd == "O")
					{
						if (cDBConvert.ToInteger(CurrentComponent["hg_converter_ind"]) != 1)
						{
							Category.CheckCatalogResult = "B";
							Category.SetCheckParameter("Calibration_Standard_Code_Valid", false, eParameterDataType.Boolean);
						}
					}
					else
					{
						DataView xCheck = Category.GetCheckParameter("Calibration_Standard_Code_to_Component_Type_Cross_Check_Table").ValueAsDataView();
						string xCheckFilter = xCheck.RowFilter;
						xCheck.RowFilter = AddToDataViewFilter(xCheckFilter, "CalibrationStandardCode = '" + calStdCd +
							"' and (ComponentTypeCode is null or ComponentTypeCode = '" + componTypeCd + "')");
						if (xCheck.Count == 0)
						{
							DataView Lookup = Category.GetCheckParameter("Calibration_Standard_Code_Lookup_Table").ValueAsDataView();
							if (!LookupCodeExists(calStdCd, Lookup))
								Category.CheckCatalogResult = "C";
							else
								Category.CheckCatalogResult = "D";
							Category.SetCheckParameter("Calibration_Standard_Code_Valid", false, eParameterDataType.Boolean);
						}
						xCheck.RowFilter = xCheckFilter;
					}
				}

			}
			catch (Exception ex)
			{ ReturnVal = Category.CheckEngine.FormatError(ex, "COMPON64"); }

			return ReturnVal;
		}

		public static string COMPON65(cCategory Category, ref bool Log)
		// Calibration Source Code Valid
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentCalibration = (DataRowView)Category.GetCheckParameter("Current_Calibration_Standard_Data").ParameterValue;

				string CalibrationSourceCode = CurrentCalibration["cal_source_cd"].AsString();
				string CalibrationStandardCode = CurrentCalibration["cal_standard_cd"].AsString();

				switch (CalibrationSourceCode)
				{
					case null:
						{
							Category.CheckCatalogResult = "A";
						}
						break;

					case "CYL":
						{
							if (CalibrationStandardCode == "O")
								Category.CheckCatalogResult = "B";
						}
						break;

					case "GEN":
						{
							if (Category.GetCheckParameter("Calibration_Standard_Code_Valid").ValueAsBool() &&
								!CalibrationStandardCode.InList("O,E"))
								Category.CheckCatalogResult = "B";
						}
						break;

					case "SIA":
						{
							if (Category.GetCheckParameter("Calibration_Standard_Code_Valid").ValueAsBool() &&
								!CalibrationStandardCode.InList("SIA,ZAM"))
								Category.CheckCatalogResult = "B";
						}
						break;

					default:
						{
							Category.CheckCatalogResult = "C";
						}
						break;
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "COMPON65");
			}

			return ReturnVal;
		}

		public static string COMPON66(cCategory Category, ref bool Log) //Component and Calibration Standard Data Dates Consistent
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentCalibration = (DataRowView)Category.GetCheckParameter("Current_Calibration_Standard_Data").ParameterValue;
				bool CalibrationStandardDatesAndHoursConsistent = cDBConvert.ToBoolean(Category.GetCheckParameter("Calibration_Standard_Data_Dates_and_Hours_Consistent").ParameterValue);
				bool ComponentDatesAndHoursConsistent = cDBConvert.ToBoolean(Category.GetCheckParameter("Component_Dates_and_Hours_Consistent").ParameterValue);

				if (CalibrationStandardDatesAndHoursConsistent && ComponentDatesAndHoursConsistent)
				{
					DateTime CompEvalBeginDate = Category.GetCheckParameter("Component_Evaluation_Begin_Date").ValueAsDateTime(DateTypes.START);
					int CompEvalBeginHour = Category.GetCheckParameter("Component_Evaluation_Begin_Hour").ValueAsInt();
					DateTime CompEvalEndDate = Category.GetCheckParameter("Component_Evaluation_End_Date").ValueAsDateTime(DateTypes.END);
					int CompEvalEndHour = Category.GetCheckParameter("Component_Evaluation_End_hour").ValueAsInt();

					DateTime CalibEvalBeginDate = Category.GetCheckParameter("Calibration_Standard_Data_Evaluation_Begin_Date").ValueAsDateTime(DateTypes.START);
					int CalibEvalBeginHour = Category.GetCheckParameter("Calibration_Standard_Data_Evaluation_Begin_Hour").ValueAsInt();
					DateTime CalibEvalEndDate = Category.GetCheckParameter("Calibration_Standard_Data_Evaluation_End_Date").ValueAsDateTime(DateTypes.END);
					int CalibEvalEndHour = Category.GetCheckParameter("Calibration_Standard_Data_Evaluation_End_Hour").ValueAsInt();

					if (CompEvalBeginDate > CalibEvalBeginDate || (CompEvalBeginDate == CalibEvalBeginDate && CompEvalBeginHour > CalibEvalBeginHour))
						Category.CheckCatalogResult = "A";
					else if (CompEvalEndDate != DateTime.MaxValue && CalibEvalEndDate == DateTime.MaxValue)
						Category.CheckCatalogResult = "A";
					else if (CompEvalEndDate < CalibEvalEndDate || (CompEvalEndDate == CalibEvalEndDate && CompEvalEndHour < CalibEvalEndHour))
						Category.CheckCatalogResult = "A";
				}


			}
			catch (Exception ex)
			{ ReturnVal = Category.CheckEngine.FormatError(ex, "COMPON66"); }

			return ReturnVal;
		}

		public static string COMPON70(cCategory Category, ref bool Log) //Calibration Standard Data Record Valid
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentCalibration = (DataRowView)Category.GetCheckParameter("Current_Calibration_Standard_Data").ParameterValue;
				DataRowView CurrentComponent = (DataRowView)Category.GetCheckParameter("Current_Component").ParameterValue;

				if (cDBConvert.ToString(CurrentComponent["component_type_cd"]) == "HG" &&
					cDBConvert.ToInteger(CurrentComponent["hg_converter_ind"]) != 1)
					Category.CheckCatalogResult = "A";

			}
			catch (Exception ex)
			{ ReturnVal = Category.CheckEngine.FormatError(ex, "COMPON70"); }

			return ReturnVal;
		}

		public static string COMPON68(cCategory Category, ref bool Log) //Duplicate Calibration Standard Data Records
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentCalibration = (DataRowView)Category.GetCheckParameter("Current_Calibration_Standard_Data").ParameterValue;
				//bool CalibrationStandardDatesAndHoursConsistent = cDBConvert.ToBoolean(Category.GetCheckParameter("Calibration_Standard_Dates_and_Hours_Consistent").ParameterValue);
				DataView LocationCalibrationStandardData = Category.GetCheckParameter("Location_Calibration_Standard_Data_Records").ValueAsDataView();

				sFilterPair[] dupCriteria = new sFilterPair[3];
				dupCriteria[0].Set("Component_ID", cDBConvert.ToString(CurrentCalibration["component_Id"]));
				dupCriteria[1].Set("Begin_Date", cDBConvert.ToDate(CurrentCalibration["begin_date"], DateTypes.START), eFilterDataType.DateBegan);
				dupCriteria[2].Set("Begin_Hour", cDBConvert.ToInteger(CurrentCalibration["begin_hour"]), eFilterDataType.Integer);

				if (DuplicateRecordCheck(CurrentCalibration, LocationCalibrationStandardData, "calibration_standard_id", dupCriteria))
					Category.CheckCatalogResult = "A";
				else
				{
					if (CurrentCalibration["end_date"] != DBNull.Value)
					{
						sFilterPair[] dupCriteria2 = new sFilterPair[3];
						dupCriteria2[0].Set("Component_ID", cDBConvert.ToString(CurrentCalibration["component_Id"]));
						dupCriteria2[1].Set("End_Date", cDBConvert.ToDate(CurrentCalibration["end_date"], DateTypes.END), eFilterDataType.DateEnded);
						dupCriteria2[2].Set("End_Hour", cDBConvert.ToInteger(CurrentCalibration["end_hour"]), eFilterDataType.Integer);

						if (DuplicateRecordCheck(CurrentCalibration, LocationCalibrationStandardData, "calibration_standard_id", dupCriteria2))
							Category.CheckCatalogResult = "A";

					}
				}

			}
			catch (Exception ex)
			{ ReturnVal = Category.CheckEngine.FormatError(ex, "COMPON68"); }

			return ReturnVal;
		}


		#endregion

		#endregion

	}
}
