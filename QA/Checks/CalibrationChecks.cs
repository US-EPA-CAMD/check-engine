using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.Qa.Parameters;
using ECMPS.Checks.TypeUtilities;
using ECMPS.Definitions.Extensions;

namespace ECMPS.Checks.CalibrationChecks
{
	public class cCalibrationChecks : cChecks
	{
		#region Constructors

		public cCalibrationChecks()
		{
			CheckProcedures = new dCheckProcedure[35];

			CheckProcedures[1] = new dCheckProcedure(SEVNDAY1);
			CheckProcedures[2] = new dCheckProcedure(SEVNDAY2);
			CheckProcedures[3] = new dCheckProcedure(SEVNDAY3);
			CheckProcedures[4] = new dCheckProcedure(SEVNDAY4);
			CheckProcedures[5] = new dCheckProcedure(SEVNDAY5);
			CheckProcedures[6] = new dCheckProcedure(SEVNDAY6);
			CheckProcedures[7] = new dCheckProcedure(SEVNDAY7);
			CheckProcedures[8] = new dCheckProcedure(SEVNDAY8);
			CheckProcedures[9] = new dCheckProcedure(SEVNDAY9);
			CheckProcedures[10] = new dCheckProcedure(SEVNDAY10);
			CheckProcedures[11] = new dCheckProcedure(SEVNDAY11);
			CheckProcedures[12] = new dCheckProcedure(SEVNDAY12);
			CheckProcedures[13] = new dCheckProcedure(SEVNDAY13);
			CheckProcedures[14] = new dCheckProcedure(SEVNDAY14);
			CheckProcedures[15] = new dCheckProcedure(SEVNDAY15);
			CheckProcedures[16] = new dCheckProcedure(SEVNDAY16);
			CheckProcedures[17] = new dCheckProcedure(SEVNDAY17);
			CheckProcedures[18] = new dCheckProcedure(SEVNDAY18);
			CheckProcedures[19] = new dCheckProcedure(SEVNDAY19);
			CheckProcedures[20] = new dCheckProcedure(SEVNDAY20);
			CheckProcedures[21] = new dCheckProcedure(SEVNDAY21);
			CheckProcedures[22] = new dCheckProcedure(SEVNDAY22);
			CheckProcedures[23] = new dCheckProcedure(SEVNDAY23);
			CheckProcedures[24] = new dCheckProcedure(SEVNDAY24);
			CheckProcedures[25] = new dCheckProcedure(SEVNDAY25);
			CheckProcedures[26] = new dCheckProcedure(SEVNDAY26);
			CheckProcedures[27] = new dCheckProcedure(SEVNDAY27);
			CheckProcedures[28] = new dCheckProcedure(SEVNDAY28);
			CheckProcedures[29] = new dCheckProcedure(SEVNDAY29);
			CheckProcedures[30] = new dCheckProcedure(SEVNDAY30);
			CheckProcedures[31] = new dCheckProcedure(SEVNDAY31);
			CheckProcedures[32] = new dCheckProcedure(SEVNDAY32);
			CheckProcedures[33] = new dCheckProcedure(SEVNDAY33);
			CheckProcedures[34] = new dCheckProcedure(SEVNDAY34);
		}


		#endregion


		#region Calibration Checks

		public static string SEVNDAY1(cCategory Category, ref bool Log)
		//Initialize 7-Day Calibration Test Variables
		{
			string ReturnVal = "";

			try
			{
				Category.SetCheckParameter("Calibration_Injection_Count", 0, eParameterDataType.Integer);
				Category.SetCheckParameter("Calibration_Minimum_Zero_Reference_Value", null, eParameterDataType.Decimal);
				Category.SetCheckParameter("Calibration_Minimum_Upscale_Reference_Value", null, eParameterDataType.Decimal);
				Category.SetCheckParameter("Calibration_Maximum_Zero_Reference_Value", 0m, eParameterDataType.Decimal);
				Category.SetCheckParameter("Calibration_Maximum_Upscale_Reference_Value", 0m, eParameterDataType.Decimal);
				Category.SetCheckParameter("Calibration_Injection_Times_Appropriate", true, eParameterDataType.Boolean);
				Category.SetCheckParameter("Calibration_Injection_Times_Valid", true, eParameterDataType.Boolean);

				Category.SetCheckParameter("Calibration_Test_Begin_Date", null, eParameterDataType.Date);
				Category.SetCheckParameter("Calibration_Test_Begin_Hour", null, eParameterDataType.Integer);
				Category.SetCheckParameter("Calibration_Test_Begin_Minute", null, eParameterDataType.Integer);
				Category.SetCheckParameter("Calibration_Test_End_Date", null, eParameterDataType.Date);
				Category.SetCheckParameter("Calibration_Test_End_Hour", null, eParameterDataType.Integer);
				Category.SetCheckParameter("Calibration_Test_End_Minute", null, eParameterDataType.Integer);
				Category.SetCheckParameter("Last_Calibration_Injection_Date", null, eParameterDataType.Date);

				Category.SetCheckParameter("Calibration_Test_Result", null, eParameterDataType.String);
				Category.SetCheckParameter("Calibration_Upscale_Gas_Level_Code", null, eParameterDataType.String);
			}

			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "SEVNDAY1");
			}

			return ReturnVal;
		}


		public static string SEVNDAY2(cCategory Category, ref bool Log)
		//7-Day Calibration Test Component Type Valid
		{
			string ReturnVal = "";

			try
			{
				DataRowView Current7DayCalibrationTest = (DataRowView)Category.GetCheckParameter("Current_7Day_Calibration_Test").ParameterValue;
				if (Current7DayCalibrationTest["COMPONENT_TYPE_CD"] == DBNull.Value)
				{
					Category.SetCheckParameter("Calibration_Test_Component_Type_Valid", false, eParameterDataType.Boolean);
					Category.CheckCatalogResult = "A";
				}
				else
					if (cDBConvert.ToString(Current7DayCalibrationTest["COMPONENT_TYPE_CD"]).InList("SO2,NOX,CO2,O2,FLOW,HG"))
						Category.SetCheckParameter("Calibration_Test_Component_Type_Valid", true, eParameterDataType.Boolean);
					else
					{
						Category.SetCheckParameter("Calibration_Test_Component_Type_Valid", false, eParameterDataType.Boolean);
						Category.CheckCatalogResult = "B";
					}
			}

			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "SEVNDAY2");
			}

			return ReturnVal;
		}

		public static string SEVNDAY3(cCategory Category, ref bool Log)
		//7-Day Calibration Test Reason Code Valid
		{
			string ReturnVal = "";
			string ReasonCode;

			try
			{
				DataRowView Current7DayCalibrationTest = (DataRowView)Category.GetCheckParameter("Current_7Day_Calibration_Test").ParameterValue;
				ReasonCode = cDBConvert.ToString(Current7DayCalibrationTest["test_reason_cd"]);
				if (ReasonCode == "")
					Category.CheckCatalogResult = "A";
				else
					if (!(ReasonCode.InList("INITIAL,RECERT,DIAG")))
					{
						DataView TestReasonLookup = (DataView)Category.GetCheckParameter("Test_Reason_Code_Lookup_Table").ParameterValue;
						string OldFilter = TestReasonLookup.RowFilter;
						TestReasonLookup.RowFilter = AddToDataViewFilter(OldFilter, "TEST_REASON_CD = '" + ReasonCode + "'");
						if (TestReasonLookup.Count == 0)
							Category.CheckCatalogResult = "B";
						else
							Category.CheckCatalogResult = "C";
						TestReasonLookup.RowFilter = OldFilter;
					}
			}

			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "SEVNDAY3");
			}

			return ReturnVal;
		}

		public static string SEVNDAY4(cCategory Category, ref bool Log)
		//Aborted 7-Day Calibration Test Not Evaluated
		{
			string ReturnVal = "";

			try
			{
				DataRowView Current7DayCalibrationTest = (DataRowView)Category.GetCheckParameter("Current_7Day_Calibration_Test").ParameterValue;
				if (cDBConvert.ToString(Current7DayCalibrationTest["test_result_cd"]) == "ABORTED")
				{
					Category.SetCheckParameter("Calibration_Test_Aborted", true, eParameterDataType.Boolean);
					Category.SetCheckParameter("Calibration_Test_Result", "ABORTED", eParameterDataType.String);
					Category.CheckCatalogResult = "A";
				}
				else
					Category.SetCheckParameter("Calibration_Test_Aborted", false, eParameterDataType.Boolean);
			}

			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "SEVNDAY4");
			}

			return ReturnVal;
		}

		public static string SEVNDAY5(cCategory Category, ref bool Log)
		//Identification of Previously Reported Test or Test Number for 7-Day Calibration Test
		{
			string ReturnVal = "";

			try
			{
				DataRowView Current7DayCalibrationTest = (DataRowView)Category.GetCheckParameter("Current_7Day_Calibration_Test").ParameterValue;
				string CompID = cDBConvert.ToString(Current7DayCalibrationTest["component_id"]);
				if (Convert.ToBoolean(Category.GetCheckParameter("Test_Span_Scale_Valid").ParameterValue) &&
					Convert.ToBoolean(Category.GetCheckParameter("Test_End_Date_Valid").ParameterValue) &&
					Convert.ToBoolean(Category.GetCheckParameter("Test_End_Hour_Valid").ParameterValue) &&
					Convert.ToBoolean(Category.GetCheckParameter("Test_End_Minute_Valid").ParameterValue))
				{
					Category.SetCheckParameter("Calibration_Test_Supp_Data_ID", null, eParameterDataType.String);

					string Scale = cDBConvert.ToString(Current7DayCalibrationTest["span_scale_cd"]);
					DateTime EndDate = cDBConvert.ToDate(Current7DayCalibrationTest["end_date"], DateTypes.END);
					int EndHour = cDBConvert.ToHour(Current7DayCalibrationTest["end_hour"], DateTypes.END);
					int EndMin = cDBConvert.ToInteger(Current7DayCalibrationTest["end_min"]);
					string TestNumber = cDBConvert.ToString(Current7DayCalibrationTest["test_num"]);

					DataView SevnDayCalTestRecords = (DataView)Category.GetCheckParameter("Calibration_Test_Records").ParameterValue;
					string OldFilter = SevnDayCalTestRecords.RowFilter;
					if (EndMin != int.MinValue)
						if (Scale != "")
							SevnDayCalTestRecords.RowFilter = AddToDataViewFilter(OldFilter,
								"span_scale_cd = '" + Scale + "' and test_num <> '" + TestNumber + "' " +
								"and end_date = '" + EndDate.ToShortDateString() + "' " + "and end_hour = " +
								EndHour + " and end_min = " + EndMin);
						else
							SevnDayCalTestRecords.RowFilter = AddToDataViewFilter(OldFilter,
								"span_scale_cd is null and test_num <> '" + TestNumber + "' " +
								"and end_date = '" + EndDate.ToShortDateString() + "' " + "and end_hour = " +
								EndHour + " and end_min = " + EndMin);
					else
						if (Scale != "")
							SevnDayCalTestRecords.RowFilter = AddToDataViewFilter(OldFilter,
								"span_scale_cd = '" + Scale + "' and test_num <> '" + TestNumber + "' " +
								"and end_date = '" + EndDate.ToShortDateString() + "' " + "and end_hour = " +
								EndHour + " and end_min is null");
						else
							SevnDayCalTestRecords.RowFilter = AddToDataViewFilter(OldFilter,
								"span_scale_cd is null and test_num <> '" + TestNumber + "' " +
								"and end_date = '" + EndDate.ToShortDateString() + "' " + "and end_hour = " +
								EndHour + " and end_min = " + EndMin);
					if ((SevnDayCalTestRecords.Count > 0 && Current7DayCalibrationTest["TEST_SUM_ID"] == DBNull.Value) ||
						(SevnDayCalTestRecords.Count > 1 && Current7DayCalibrationTest["TEST_SUM_ID"] != DBNull.Value) ||
						(SevnDayCalTestRecords.Count == 1 && Current7DayCalibrationTest["TEST_SUM_ID"] != DBNull.Value && Current7DayCalibrationTest["TEST_SUM_ID"].ToString() != SevnDayCalTestRecords[0]["TEST_SUM_ID"].ToString()))
						Category.CheckCatalogResult = "A";
					else
					{
						DataView QASuppRecords = (DataView)Category.GetCheckParameter("QA_Supplemental_Data_Records").ParameterValue;
						OldFilter = QASuppRecords.RowFilter;
						if (EndMin != int.MinValue)
							if (Scale != "")
								QASuppRecords.RowFilter = AddToDataViewFilter(OldFilter, "test_type_cd = '7DAY' and " + "component_id = '" +
									CompID + "' and span_scale = '" + Scale + "' " + "and end_date = '" + EndDate.ToShortDateString() +
									"' " + "and end_hour = " + EndHour + " and (end_min is null or end_min = " + EndMin + ") and test_num <> '" + TestNumber + "'");
							else
								QASuppRecords.RowFilter = AddToDataViewFilter(OldFilter, "test_type_cd = '7DAY' and " + "component_id = '" +
									CompID + "' and span_scale is null and end_date = '" + EndDate.ToShortDateString() +
									"' " + "and end_hour = " + EndHour + " and (end_min is null or end_min = " + EndMin + ") and test_num <> '" + TestNumber + "'");
						else
							if (Scale != "")
								QASuppRecords.RowFilter = AddToDataViewFilter(OldFilter, "test_type_cd = '7DAY' and " + "component_id = '" +
									CompID + "' and span_scale = '" + Scale + "' " + "and end_date = '" + EndDate.ToShortDateString() + "' " +
									"and end_hour = " + EndHour + " and end_min is null" + " and test_num <> '" + TestNumber + "'");
							else
								QASuppRecords.RowFilter = AddToDataViewFilter(OldFilter, "test_type_cd = '7DAY' and " + "component_id = '" +
									CompID + "' and span_scale is null and end_date = '" + EndDate.ToShortDateString() + "' " +
									"and end_hour = " + EndHour + " and end_min is null" + " and test_num <> '" + TestNumber + "'");
						if ((QASuppRecords.Count > 0 && Current7DayCalibrationTest["TEST_SUM_ID"] == DBNull.Value) ||
							(QASuppRecords.Count > 1 && Current7DayCalibrationTest["TEST_SUM_ID"] != DBNull.Value) ||
							(QASuppRecords.Count == 1 && Current7DayCalibrationTest["TEST_SUM_ID"] != DBNull.Value && Current7DayCalibrationTest["TEST_SUM_ID"].ToString() != QASuppRecords[0]["TEST_SUM_ID"].ToString()))
							Category.CheckCatalogResult = "A";
						else
						{
							QASuppRecords.RowFilter = AddToDataViewFilter(OldFilter, "test_type_cd = '7DAY' and " +
								"test_num = '" + TestNumber + "'");
							if (QASuppRecords.Count > 0)
							{
								Category.SetCheckParameter("Calibration_Test_Supp_Data_ID", cDBConvert.ToString(((DataRowView)QASuppRecords[0])["QA_Supp_Data_ID"]),
									eParameterDataType.String);
								if (cDBConvert.ToString(((DataRowView)QASuppRecords[0])["CAN_SUBMIT"]) == "N")
								{
									if (EndMin != int.MinValue)
										if (Scale != "")
											QASuppRecords.RowFilter = AddToDataViewFilter(OldFilter, "test_type_cd = '7DAY' and " +
												"test_num = '" + TestNumber + "' " + "and (component_id <> '" + CompID +
												"' or span_scale <> '" + Scale + "' " + "or end_date <> '" +
												EndDate.ToShortDateString() + "' " + "or end_hour <> " + EndHour + " " +
												"or end_min <> " + EndMin + ")");
										else
											QASuppRecords.RowFilter = AddToDataViewFilter(OldFilter, "test_type_cd = '7DAY' and " +
												"test_num = '" + TestNumber + "' " + "and (component_id <> '" + CompID +
												"' or span_scale is not null or end_date <> '" +
												EndDate.ToShortDateString() + "' " + "or end_hour <> " + EndHour + " " +
												"or end_min <> " + EndMin + ")");
									else
										if (Scale != "")
											QASuppRecords.RowFilter = AddToDataViewFilter(OldFilter, "test_type_cd = '7DAY' and " +
												"test_num = '" + TestNumber + "' " + "and (component_id <> '" + CompID +
												"' or span_scale <> '" + Scale + "' " + "or end_date <> '" +
												EndDate.ToShortDateString() + "' " + "or end_hour <> " + EndHour + " " +
												"or end_min is not null)");
										else
											QASuppRecords.RowFilter = AddToDataViewFilter(OldFilter, "test_type_cd = '7DAY' and " +
												"test_num = '" + TestNumber + "' " + "and (component_id <> '" + CompID +
												"' or span_scale is not null or end_date <> '" +
												EndDate.ToShortDateString() + "' " + "or end_hour <> " + EndHour + " " +
												"or end_min is not null)");
									if ((QASuppRecords.Count > 0 && Current7DayCalibrationTest["TEST_SUM_ID"] == DBNull.Value) ||
										(QASuppRecords.Count > 1 && Current7DayCalibrationTest["TEST_SUM_ID"] != DBNull.Value) ||
										(QASuppRecords.Count == 1 && Current7DayCalibrationTest["TEST_SUM_ID"] != DBNull.Value && Current7DayCalibrationTest["TEST_SUM_ID"].ToString() != QASuppRecords[0]["TEST_SUM_ID"].ToString()))
										Category.CheckCatalogResult = "B";
									else
										Category.CheckCatalogResult = "C";
								}
							}
						}
						QASuppRecords.RowFilter = OldFilter;
					}
					SevnDayCalTestRecords.RowFilter = OldFilter;
				}
				else
					Log = false;
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "SEVNDAY5");
			}

			return ReturnVal;
		}

		public static string SEVNDAY6(cCategory Category, ref bool Log)
		//Zero Injection Time Valid
		{
			string ReturnVal = "";
			try
			{
				DataRowView CurrentCalibrationInjection = (DataRowView)Category.GetCheckParameter("Current_Calibration_Injection").ParameterValue;
				Category.SetCheckParameter("Zero_Calibration_Injection_Valid", true, eParameterDataType.Boolean);

				DateTime ZeroInjDate = cDBConvert.ToDate(CurrentCalibrationInjection["Zero_Injection_Date"], DateTypes.START);
				int ZeroInjHour = cDBConvert.ToInteger(CurrentCalibrationInjection["Zero_Injection_Hour"]);
				int ZeroInjMinute = cDBConvert.ToInteger(CurrentCalibrationInjection["Zero_Injection_Min"]);
				DateTime MPDate = Category.GetCheckParameter("ECMPS_MP_Begin_Date").ValueAsDateTime(DateTypes.START);

				if ((ZeroInjDate == DateTime.MinValue || ZeroInjHour == int.MinValue || ZeroInjHour < 0 || ZeroInjHour > 23) ||
					(ZeroInjMinute == int.MinValue && ZeroInjDate >= MPDate) ||
					((ZeroInjMinute != int.MinValue) && (ZeroInjMinute < 0 || ZeroInjMinute > 59)))
				{
					Category.SetCheckParameter("Calibration_Injection_Times_Valid", false, eParameterDataType.Boolean);
					Category.CheckCatalogResult = "A";
				}
				else
				{
					DateTime LastInjDate = Convert.ToDateTime(Category.GetCheckParameter("Last_Calibration_Injection_Date").ParameterValue);
					if (Category.GetCheckParameter("Last_Calibration_Injection_Date").ParameterValue == null || LastInjDate < ZeroInjDate)
						Category.SetCheckParameter("Last_Calibration_Injection_Date", ZeroInjDate, eParameterDataType.Date);
					else
						Category.SetCheckParameter("Calibration_Injection_Times_Appropriate", false, eParameterDataType.Boolean);
					DateTime UpsInjDate = cDBConvert.ToDate(CurrentCalibrationInjection["Upscale_Injection_Date"], DateTypes.START);
					int UpsInjMinute = cDBConvert.ToInteger(CurrentCalibrationInjection["Upscale_Injection_Min"]);
					if ((ZeroInjMinute == int.MinValue && ZeroInjDate < MPDate) || (UpsInjMinute == int.MinValue && UpsInjDate < MPDate))
						Category.CheckCatalogResult = "B";
				}
			}

			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "SEVNDAY6");
			}
			return ReturnVal;
		}

		public static string SEVNDAY7(cCategory Category, ref bool Log)
		//Upscale Injection Time Valid
		{
			string ReturnVal = "";

			try
			{
				Category.SetCheckParameter("Upscale_Calibration_Injection_Valid", true, eParameterDataType.Boolean);
				DataRowView CurrentCalibrationInjection = (DataRowView)Category.GetCheckParameter("Current_Calibration_Injection").ParameterValue;
				string AppendedCode = Convert.ToString(Category.GetCheckParameter("Calibration_Upscale_Gas_Level_Code").ParameterValue).ListAdd(Convert.ToString(CurrentCalibrationInjection["upscale_gas_level_cd"]));
				Category.SetCheckParameter("Calibration_Upscale_Gas_Level_Code", AppendedCode, eParameterDataType.String);

				DateTime UpscaleInjDate = cDBConvert.ToDate(CurrentCalibrationInjection["upscale_injection_date"], DateTypes.START);
				int UpscaleInjHour = cDBConvert.ToInteger(CurrentCalibrationInjection["upscale_injection_hour"]);
				int UpscaleInjMinute = cDBConvert.ToInteger(CurrentCalibrationInjection["upscale_injection_min"]);
				DateTime MPDate = Category.GetCheckParameter("ECMPS_MP_Begin_Date").ValueAsDateTime(DateTypes.START);

				DateTime ZeroInjDate = cDBConvert.ToDate(CurrentCalibrationInjection["Zero_Injection_Date"], DateTypes.START);

				if ((UpscaleInjDate == DateTime.MinValue || UpscaleInjHour == int.MinValue || UpscaleInjHour < 0 || UpscaleInjHour > 23) ||
					(UpscaleInjMinute == int.MinValue && UpscaleInjDate >= MPDate) ||
					((UpscaleInjMinute != int.MinValue) && (UpscaleInjMinute < 0 || UpscaleInjMinute > 59)))
				{
					Category.SetCheckParameter("Calibration_Injection_Times_Valid", false, eParameterDataType.Boolean);
					Category.CheckCatalogResult = "A";
				}
				else
					if ((ZeroInjDate != DateTime.MinValue) && (UpscaleInjDate != ZeroInjDate))
						Category.CheckCatalogResult = "B";
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "SEVNDAY7");
			}

			return ReturnVal;
		}

		public static string SEVNDAY8(cCategory Category, ref bool Log)
		//Zero Calibration Error Valid
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentCalibrationInjection = (DataRowView)Category.GetCheckParameter("Current_Calibration_Injection").ParameterValue;
				decimal ZeroCalErr = cDBConvert.ToDecimal(CurrentCalibrationInjection["zero_cal_error"]);
				if (ZeroCalErr == decimal.MinValue)
					Category.CheckCatalogResult = "A";
				else
					if (ZeroCalErr < 0)
						Category.CheckCatalogResult = "B";
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "SEVNDAY8");
			}

			return ReturnVal;
		}

		public static string SEVNDAY9(cCategory Category, ref bool Log)
		//Upscale Calibration Error Valid
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentCalibrationInjection = (DataRowView)Category.GetCheckParameter("Current_Calibration_Injection").ParameterValue;
				decimal UpscaleCalErr = cDBConvert.ToDecimal(CurrentCalibrationInjection["upscale_cal_error"]);
				if (UpscaleCalErr == decimal.MinValue)
					Category.CheckCatalogResult = "A";
				else
					if (UpscaleCalErr < 0)
						Category.CheckCatalogResult = "B";
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "SEVNDAY9");
			}

			return ReturnVal;
		}

		public static string SEVNDAY10(cCategory Category, ref bool Log)
		//Injection Upscale Gas Level Code Valid
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentCalibrationInjection = (DataRowView)Category.GetCheckParameter("Current_Calibration_Injection").ParameterValue;
				string UpscaleGasLvlCd = cDBConvert.ToString(CurrentCalibrationInjection["UPSCALE_GAS_LEVEL_CD"]);
				if (UpscaleGasLvlCd == "")
				{
					Category.SetCheckParameter("Upscale_Calibration_Injection_Valid", false, eParameterDataType.Boolean);
					Category.CheckCatalogResult = "A";
				}
				else
					if (!UpscaleGasLvlCd.InList("MID,HIGH"))
					{
						Category.SetCheckParameter("Upscale_Calibration_Injection_Valid", false, eParameterDataType.Boolean);
						Category.CheckCatalogResult = "B";
					}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "SEVNDAY10");
			}

			return ReturnVal;
		}

		public static string SEVNDAY11(cCategory Category, ref bool Log)
		//Zero Measured Value Valid
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentCalibrationInjection = (DataRowView)Category.GetCheckParameter("Current_Calibration_Injection").ParameterValue;
				if ((cDBConvert.ToDecimal(CurrentCalibrationInjection["ZERO_MEASURED_VALUE"])) == decimal.MinValue)
				{
					Category.SetCheckParameter("Zero_Calibration_Injection_Valid", false, eParameterDataType.Boolean);
					Category.CheckCatalogResult = "A";
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "SEVNDAY11");
			}

			return ReturnVal;
		}

		public static string SEVNDAY12(cCategory Category, ref bool Log)
		//Upscale Measured Value Valid
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentCalibrationInjection = (DataRowView)Category.GetCheckParameter("Current_Calibration_Injection").ParameterValue;
				if ((cDBConvert.ToDecimal(CurrentCalibrationInjection["UPSCALE_MEASURED_VALUE"])) == decimal.MinValue)
				{
					Category.SetCheckParameter("Upscale_Calibration_Injection_Valid", false, eParameterDataType.Boolean);
					Category.CheckCatalogResult = "A";
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "SEVNDAY12");
			}

			return ReturnVal;
		}

		public static string SEVNDAY13(cCategory Category, ref bool Log)
		//Zero Reference Value Valid
		{
			string ReturnVal = "";

			try
			{
				Category.SetCheckParameter("Calculate_Zero_Calibration_Injection",
					Convert.ToBoolean(Category.GetCheckParameter("Zero_Calibration_Injection_Valid").ParameterValue), eParameterDataType.Boolean);
				DataRowView CurrentCalibrationInjection = (DataRowView)Category.GetCheckParameter("Current_Calibration_Injection").ParameterValue;
				decimal ZeroRefVal = cDBConvert.ToDecimal(CurrentCalibrationInjection["ZERO_REF_VALUE"]);
				if ((ZeroRefVal == decimal.MinValue) || (ZeroRefVal < 0))
				{
					Category.SetCheckParameter("Calculate_Zero_Calibration_Injection", false, eParameterDataType.Boolean);
					Category.SetCheckParameter("Calibration_Maximum_Zero_Reference_Value", null, eParameterDataType.Decimal);
					if (ZeroRefVal == decimal.MinValue)
						Category.CheckCatalogResult = "A";
					else
						Category.CheckCatalogResult = "B";
				}
				else
				{
					if (Category.GetCheckParameter("Calibration_Maximum_Zero_Reference_Value").ParameterValue != null)
					{
						decimal CalMaxZeroRefVal = Convert.ToDecimal(Category.GetCheckParameter("Calibration_Maximum_Zero_Reference_Value").ParameterValue);
						if (ZeroRefVal > CalMaxZeroRefVal)
							Category.SetCheckParameter("Calibration_Maximum_Zero_Reference_Value", ZeroRefVal, eParameterDataType.Decimal);
						if (Category.GetCheckParameter("Calibration_Minimum_Zero_Reference_Value").ParameterValue == null || ZeroRefVal < CalMaxZeroRefVal)
							Category.SetCheckParameter("Calibration_Minimum_Zero_Reference_Value", ZeroRefVal, eParameterDataType.Decimal);
					}
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "SEVNDAY13");
			}

			return ReturnVal;
		}

		public static string SEVNDAY14(cCategory Category, ref bool Log)
		//Upscale Reference Value Valid
		{
			string ReturnVal = "";

			try
			{
				Category.SetCheckParameter("Calculate_Upscale_Calibration_Injection",
					Convert.ToBoolean(Category.GetCheckParameter("Upscale_Calibration_Injection_Valid").ParameterValue), eParameterDataType.Boolean);
				DataRowView CurrentCalibrationInjection = (DataRowView)Category.GetCheckParameter("Current_Calibration_Injection").ParameterValue;
				decimal UpscaleRefVal = cDBConvert.ToDecimal(CurrentCalibrationInjection["UPSCALE_REF_VALUE"]);
				if ((UpscaleRefVal == decimal.MinValue) || (UpscaleRefVal <= 0))
				{
					Category.SetCheckParameter("Calculate_Upscale_Calibration_Injection", false, eParameterDataType.Boolean);
					Category.SetCheckParameter("Calibration_Maximum_Upscale_Reference_Value", null, eParameterDataType.Decimal);
					if (UpscaleRefVal == decimal.MinValue)
						Category.CheckCatalogResult = "A";
					else
						Category.CheckCatalogResult = "B";
				}
				else
				{
					if (Category.GetCheckParameter("Calibration_Maximum_Upscale_Reference_Value").ParameterValue != null)
					{
						decimal CalMaxUpscaleRefVal = Convert.ToDecimal(Category.GetCheckParameter("Calibration_Maximum_Upscale_Reference_Value").ParameterValue);
						if (UpscaleRefVal > CalMaxUpscaleRefVal)
							Category.SetCheckParameter("Calibration_Maximum_Upscale_Reference_Value", UpscaleRefVal, eParameterDataType.Boolean);
						if (Category.GetCheckParameter("Calibration_Minimum_Upscale_Reference_Value").ParameterValue == null || UpscaleRefVal < (decimal)Category.GetCheckParameter("Calibration_Minimum_Upscale_Reference_Value").ParameterValue)
							Category.SetCheckParameter("Calibration_Minimum_Upscale_Reference_Value", UpscaleRefVal, eParameterDataType.Boolean);
					}
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "SEVNDAY14");
			}

			return ReturnVal;
		}

		public static string SEVNDAY15(cCategory Category, ref bool Log)
		//Calculate Zero Injection Results
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentCalibrationInjection = (DataRowView)Category.GetCheckParameter("Current_Calibration_Injection").ParameterValue;
				Category.SetCheckParameter("Calibration_Injection_Count",
					(Convert.ToInt32(Category.GetCheckParameter("Calibration_Injection_Count").ParameterValue) + 1), eParameterDataType.Boolean);
				if (Convert.ToBoolean(Category.GetCheckParameter("Calibration_Injection_Times_Valid").ParameterValue))
				{
					DateTime CalTestBeginDate = Convert.ToDateTime(Category.GetCheckParameter("Calibration_Test_Begin_Date").ParameterValue);
					int CalTestBeginHour = Convert.ToInt32(Category.GetCheckParameter("Calibration_Test_Begin_Hour").ParameterValue);
					int CalTestBeginMinute = Convert.ToInt32(Category.GetCheckParameter("Calibration_Test_Begin_Minute").ParameterValue);
					DateTime CalTestEndDate = Convert.ToDateTime(Category.GetCheckParameter("Calibration_Test_End_Date").ParameterValue);
					int CalTestEndHour = Convert.ToInt32(Category.GetCheckParameter("Calibration_Test_End_Hour").ParameterValue);
					int CalTestEndMinute = Convert.ToInt32(Category.GetCheckParameter("Calibration_Test_End_Minute").ParameterValue);
					DateTime ZeroInjDate = cDBConvert.ToDate(CurrentCalibrationInjection["ZERO_INJECTION_DATE"], DateTypes.START);
					int ZeroInjHour = cDBConvert.ToHour(CurrentCalibrationInjection["ZERO_INJECTION_HOUR"], DateTypes.START);
					int ZeroInjMinute = cDBConvert.ToInteger(CurrentCalibrationInjection["ZERO_INJECTION_MIN"]);
					DateTime UpscaleInjDate = cDBConvert.ToDate(CurrentCalibrationInjection["UPSCALE_INJECTION_DATE"], DateTypes.START);
					int UpscaleInjHour = cDBConvert.ToHour(CurrentCalibrationInjection["UPSCALE_INJECTION_HOUR"], DateTypes.START);
					int UpscaleInjMinute = cDBConvert.ToInteger(CurrentCalibrationInjection["UPSCALE_INJECTION_MIN"]);

					if (Category.GetCheckParameter("Calibration_Test_Begin_Date").ParameterValue == null || CalTestBeginDate > ZeroInjDate ||
						(CalTestBeginDate == ZeroInjDate && CalTestBeginHour > ZeroInjHour) ||
						(CalTestBeginDate == ZeroInjDate && CalTestBeginHour == ZeroInjHour && CalTestBeginMinute > ZeroInjMinute))
					{
						Category.SetCheckParameter("Calibration_Test_Begin_Date", ZeroInjDate, eParameterDataType.Date);
						Category.SetCheckParameter("Calibration_Test_Begin_Hour", ZeroInjHour, eParameterDataType.Integer);
						Category.SetCheckParameter("Calibration_Test_Begin_Minute", ZeroInjMinute, eParameterDataType.Integer);
						CalTestBeginDate = Convert.ToDateTime(Category.GetCheckParameter("Calibration_Test_Begin_Date").ParameterValue);
						CalTestBeginHour = Convert.ToInt32(Category.GetCheckParameter("Calibration_Test_Begin_Hour").ParameterValue);
						CalTestBeginMinute = Convert.ToInt32(Category.GetCheckParameter("Calibration_Test_Begin_Minute").ParameterValue);
					}
					if (CalTestBeginDate > UpscaleInjDate ||
						(CalTestBeginDate == UpscaleInjDate && CalTestBeginHour > UpscaleInjHour) ||
						(CalTestBeginDate == UpscaleInjDate && CalTestBeginHour == UpscaleInjHour && CalTestBeginMinute > UpscaleInjMinute))
					{
						Category.SetCheckParameter("Calibration_Test_Begin_Date", UpscaleInjDate, eParameterDataType.Date);
						Category.SetCheckParameter("Calibration_Test_Begin_Hour", UpscaleInjHour, eParameterDataType.Integer);
						Category.SetCheckParameter("Calibration_Test_Begin_Minute", UpscaleInjMinute, eParameterDataType.Integer);
					}
					if (Category.GetCheckParameter("Calibration_Test_End_Date").ParameterValue == null || CalTestEndDate < ZeroInjDate ||
						(CalTestEndDate == ZeroInjDate && CalTestEndHour < ZeroInjHour) ||
						(CalTestEndDate == ZeroInjDate && CalTestEndHour == ZeroInjHour && CalTestEndMinute < ZeroInjMinute))
					{
						Category.SetCheckParameter("Calibration_Test_End_Date", ZeroInjDate, eParameterDataType.Date);
						Category.SetCheckParameter("Calibration_Test_End_Hour", ZeroInjHour, eParameterDataType.Integer);
						Category.SetCheckParameter("Calibration_Test_End_Minute", ZeroInjMinute, eParameterDataType.Integer);
						CalTestEndDate = Convert.ToDateTime(Category.GetCheckParameter("Calibration_Test_End_Date").ParameterValue);
						CalTestEndHour = Convert.ToInt32(Category.GetCheckParameter("Calibration_Test_End_Hour").ParameterValue);
						CalTestEndMinute = Convert.ToInt32(Category.GetCheckParameter("Calibration_Test_End_Minute").ParameterValue);
					}
					if (CalTestEndDate < UpscaleInjDate ||
						(CalTestEndDate == UpscaleInjDate && CalTestEndHour < UpscaleInjHour) ||
						(CalTestEndDate == UpscaleInjDate && CalTestEndHour == UpscaleInjHour && CalTestEndMinute < UpscaleInjMinute))
					{
						Category.SetCheckParameter("Calibration_Test_End_Date", UpscaleInjDate, eParameterDataType.Date);
						Category.SetCheckParameter("Calibration_Test_End_Hour", UpscaleInjHour, eParameterDataType.Integer);
						Category.SetCheckParameter("Calibration_Test_End_Minute", UpscaleInjMinute, eParameterDataType.Integer);
					}
				}

				if (Category.GetCheckParameter("Test_Span_Value").ParameterValue == null)
					Category.SetCheckParameter("Calculate_Zero_Calibration_Injection", false, eParameterDataType.Boolean);

				if (!Convert.ToBoolean(Category.GetCheckParameter("Calculate_Zero_Calibration_Injection").ParameterValue))
				{
					Category.SetCheckParameter("Calibration_Test_Result", "INVALID", eParameterDataType.String);
					Category.SetCheckParameter("Calibration_Zero_Injection_Calc_Result", null, eParameterDataType.Decimal);
					Category.SetCheckParameter("Calibration_Zero_Injection_Calc_APS_Indicator", null, eParameterDataType.Integer);
					Category.CheckCatalogResult = "A";
				}
				else
				{
					decimal diff = Math.Abs(cDBConvert.ToDecimal(CurrentCalibrationInjection["ZERO_MEASURED_VALUE"]) -
						cDBConvert.ToDecimal(CurrentCalibrationInjection["ZERO_REF_VALUE"]));
					Category.SetCheckParameter("Calibration_Zero_Injection_Calc_APS_Indicator",
						0, eParameterDataType.Integer);
					string CalTestResult = Convert.ToString(Category.GetCheckParameter("Calibration_Test_Result").ParameterValue);
					decimal TestSpanVal = Convert.ToDecimal(Category.GetCheckParameter("Test_Span_Value").ParameterValue);
					decimal ZeroInjCalcResult;
					decimal ZeroCalError = cDBConvert.ToDecimal(CurrentCalibrationInjection["ZERO_CAL_ERROR"]);
					string CompTypeCd = cDBConvert.ToString(CurrentCalibrationInjection["COMPONENT_TYPE_CD"]);
					DataView TestToleranceRecords = (DataView)Category.GetCheckParameter("Test_Tolerances_Cross_Check_Table").ParameterValue;
					string OldFilter = TestToleranceRecords.RowFilter;
					decimal ToleranceValue = 0;

					if (CompTypeCd.InList("CO2,O2"))
					{
						diff = Math.Round(diff, 1, MidpointRounding.AwayFromZero);
						Category.SetCheckParameter("Calibration_Zero_Injection_Calc_Result", diff, eParameterDataType.Decimal);
						ZeroInjCalcResult = diff;
						if (!CalTestResult.InList("INVALID,FAILED"))
							if (ZeroInjCalcResult > (decimal)0.5)
                            {
                                Category.SetCheckParameter("Calibration_Test_Result", "FAILED", eParameterDataType.String);

                                if ((ZeroCalError >= 0m) && (ZeroCalError <= 0.5m))
                                {
                                    ToleranceValue = GetTolerance("7DAY", "DifferencePCT", Category);

                                    if (Math.Abs(diff - ZeroCalError) <= ToleranceValue)
                                        Category.SetCheckParameter("Calibration_Test_Result", "PASSED", eParameterDataType.String);
                                }
							}
							else
								Category.SetCheckParameter("Calibration_Test_Result", "PASSED", eParameterDataType.String);
					}
					else
						if (CompTypeCd.InList("SO2,NOX"))
						{
							ZeroInjCalcResult = Math.Min(Math.Round(diff / TestSpanVal * 100, 1, MidpointRounding.AwayFromZero), (decimal)9999.9);
							Category.SetCheckParameter("Calibration_Zero_Injection_Calc_Result", ZeroInjCalcResult, eParameterDataType.Decimal);
							diff = Math.Round(diff, 0, MidpointRounding.AwayFromZero);
							if ((ZeroInjCalcResult > (decimal)2.5) && (TestSpanVal < 200) && (diff <= (decimal)5))
							{
								Category.SetCheckParameter("Calibration_Zero_Injection_Calc_Result", diff, eParameterDataType.Decimal);
								Category.SetCheckParameter("Calibration_Zero_Injection_Calc_APS_Indicator", 1, eParameterDataType.Integer);
								if ((CalTestResult != "INVALID") && (CalTestResult != "FAILED"))
									Category.SetCheckParameter("Calibration_Test_Result", "PASSAPS", eParameterDataType.String);
							}
							else
								if (ZeroInjCalcResult > (decimal)2.5)
								{
									if (!CalTestResult.InList("INVALID,FAILED"))
										if (Category.GetCheckParameter("Calibration_Zero_Injection_Calc_APS_Indicator").ValueAsInt() != 1 &&
											0 <= ZeroCalError && ZeroCalError <= (decimal)2.5)
										{
											ToleranceValue = GetTolerance("7DAY", "CalibrationError", Category);
											if (Math.Abs(ZeroInjCalcResult - ZeroCalError) <= ToleranceValue)
											{
												if (CalTestResult != "PASSAPS")
													Category.SetCheckParameter("Calibration_Test_Result", "PASSED", eParameterDataType.String);
											}
											else
												Category.SetCheckParameter("Calibration_Test_Result", "FAILED", eParameterDataType.String);
										}
										else
										{
											Category.SetCheckParameter("Calibration_Test_Result", "FAILED", eParameterDataType.String);
											if (Category.GetCheckParameter("Calibration_Zero_Injection_Calc_APS_Indicator").ValueAsInt() == 1 &&
												0 <= ZeroCalError && ZeroCalError <= 5 && TestSpanVal < 200)
											{
												ToleranceValue = GetTolerance("7DAY", "DifferencePPM", Category);
												if (Math.Abs(diff - ZeroCalError) <= ToleranceValue)
													Category.SetCheckParameter("Calibration_Test_Result", "PASSAPS", eParameterDataType.String);
											}
										}
								}
								else
									if (!CalTestResult.InList("INVALID,FAILED,PASSAPS"))
										Category.SetCheckParameter("Calibration_Test_Result", "PASSED", eParameterDataType.String);
						}
						else
							if (CompTypeCd.InList("FLOW"))
							{
								ZeroInjCalcResult = Math.Min(Math.Round(diff / TestSpanVal * 100, 1, MidpointRounding.AwayFromZero), (decimal)9999.9);
								Category.SetCheckParameter("Calibration_Zero_Injection_Calc_Result",
									ZeroInjCalcResult, eParameterDataType.Decimal);
								diff = Math.Round(diff, 2, MidpointRounding.AwayFromZero);
								string AcqCd = cDBConvert.ToString(CurrentCalibrationInjection["ACQ_CD"]);
								if ((ZeroInjCalcResult > (decimal)3.0) && AcqCd == "DP" && diff <= (decimal)0.01)
								{
									Category.SetCheckParameter("Calibration_Zero_Injection_Calc_Result",
										0m, eParameterDataType.Decimal);
									Category.SetCheckParameter("Calibration_Zero_Injection_Calc_APS_Indicator",
										1, eParameterDataType.Integer);
									if ((CalTestResult != "INVALID") && (CalTestResult != "FAILED"))
										Category.SetCheckParameter("Calibration_Test_Result", "PASSAPS", eParameterDataType.String);
								}
								else
									if (ZeroInjCalcResult > (decimal)3.0)
									{
										if (!CalTestResult.InList("INVALID,FAILED"))
											if (Category.GetCheckParameter("Calibration_Zero_Injection_Calc_APS_Indicator").ValueAsInt() != 1 &&
												0 <= ZeroCalError && ZeroCalError <= 3)
											{
												ToleranceValue = GetTolerance("7DAY", "CalibrationError", Category);
												if (Math.Abs(ZeroInjCalcResult - ZeroCalError) <= ToleranceValue)
												{
													if (CalTestResult != "PASSAPS")
														Category.SetCheckParameter("Calibration_Test_Result", "PASSED", eParameterDataType.String);
												}
												else
													Category.SetCheckParameter("Calibration_Test_Result", "FAILED", eParameterDataType.String);
											}
											else
											{
												Category.SetCheckParameter("Calibration_Test_Result", "FAILED", eParameterDataType.String);
												if (Category.GetCheckParameter("Calibration_Zero_Injection_Calc_APS_Indicator").ValueAsInt() == 1 &&
													AcqCd == "DP" && 0 <= ZeroCalError && ZeroCalError <= (decimal)0.01)
												{
													ToleranceValue = GetTolerance("7DAY", "DifferenceINH2O", Category);
													if (Math.Abs(diff - ZeroCalError) <= ToleranceValue)
														Category.SetCheckParameter("Calibration_Test_Result", "PASSAPS", eParameterDataType.String);
												}
											}
									}
									else
										if (!CalTestResult.InList("INVALID,FAILED,PASSAPS"))
											Category.SetCheckParameter("Calibration_Test_Result", "PASSED", eParameterDataType.String);
							}
							else
								if (CompTypeCd == "HG")
								{
									ZeroInjCalcResult = Math.Min(Math.Round(diff / TestSpanVal * 100, 1, MidpointRounding.AwayFromZero), (decimal)9999.9);
									Category.SetCheckParameter("Calibration_Zero_Injection_Calc_Result", ZeroInjCalcResult, eParameterDataType.Decimal);
									diff = Math.Round(diff, 1, MidpointRounding.AwayFromZero);
									if (ZeroInjCalcResult > (decimal)5.0 && TestSpanVal <= (decimal)10 && diff <= (decimal)1)
									{
										Category.SetCheckParameter("Calibration_Zero_Injection_Calc_Result", diff, eParameterDataType.Decimal);
										Category.SetCheckParameter("Calibration_Zero_Injection_Calc_APS_Indicator", 1, eParameterDataType.Integer);
										if (CalTestResult != "INVALID" && CalTestResult != "FAILED")
											Category.SetCheckParameter("Calibration_Test_Result", "PASSAPS", eParameterDataType.String);
									}
									else
										if (ZeroInjCalcResult > (decimal)5.0)
										{
											if (!CalTestResult.InList("INVALID,FAILED"))
												if (Category.GetCheckParameter("Calibration_Zero_Injection_Calc_APS_Indicator").ValueAsInt() != 1 &&
													0 <= ZeroCalError && ZeroCalError <= 5)
												{
													ToleranceValue = GetTolerance("7DAY", "CalibrationError", Category);
													if (Math.Abs(ZeroInjCalcResult - ZeroCalError) <= ToleranceValue)
													{
														if (CalTestResult != "PASSAPS")
															Category.SetCheckParameter("Calibration_Test_Result", "PASSED", eParameterDataType.String);
													}
													else
														Category.SetCheckParameter("Calibration_Test_Result", "FAILED", eParameterDataType.String);
												}
												else
												{
													Category.SetCheckParameter("Calibration_Test_Result", "FAILED", eParameterDataType.String);
													if (Category.GetCheckParameter("Calibration_Zero_Injection_Calc_APS_Indicator").ValueAsInt() == 1 &&
														0 <= ZeroCalError && ZeroCalError <= 1 && TestSpanVal < 10)
													{
														ToleranceValue = GetTolerance("7DAY", "DifferenceUGSCM", Category);
														if (Math.Abs(diff - ZeroCalError) <= ToleranceValue)
															Category.SetCheckParameter("Calibration_Test_Result", "PASSAPS", eParameterDataType.String);
													}
												}
										}
										else
											if (!CalTestResult.InList("INVALID,FAILED,PASSAPS"))
												Category.SetCheckParameter("Calibration_Test_Result", "PASSED", eParameterDataType.String);
								}
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "SEVNDAY15");
			}

			return ReturnVal;
		}

		public static string SEVNDAY16(cCategory Category, ref bool Log)
		//Calculate Upscale Injection Results
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentCalibrationInjection = (DataRowView)Category.GetCheckParameter("Current_Calibration_Injection").ParameterValue;

				if (Category.GetCheckParameter("Test_Span_Value").ParameterValue == null)
					Category.SetCheckParameter("Calculate_Upscale_Calibration_Injection", false, eParameterDataType.Boolean);

				if (!Convert.ToBoolean(Category.GetCheckParameter("Calculate_Upscale_Calibration_Injection").ParameterValue))
				{
					Category.SetCheckParameter("Calibration_Test_Result", "INVALID", eParameterDataType.String);
					Category.SetCheckParameter("Calibration_Upscale_Injection_Calc_Result", null, eParameterDataType.Decimal);
					Category.SetCheckParameter("Calibration_Upscale_Injection_Calc_APS_Indicator", null, eParameterDataType.Integer);
					Category.CheckCatalogResult = "A";
				}
				else
				{
					decimal diff = Math.Abs(cDBConvert.ToDecimal(CurrentCalibrationInjection["UPSCALE_MEASURED_VALUE"]) -
						cDBConvert.ToDecimal(CurrentCalibrationInjection["UPSCALE_REF_VALUE"]));
					Category.SetCheckParameter("Calibration_Upscale_Injection_Calc_APS_Indicator", 0, eParameterDataType.Integer);
					string CalTestResult = Convert.ToString(Category.GetCheckParameter("Calibration_Test_Result").ParameterValue);
					decimal TestSpanVal = Convert.ToDecimal(Category.GetCheckParameter("Test_Span_Value").ParameterValue);
					decimal UpscaleInjCalcResult;
					string CompTypeCd = cDBConvert.ToString(CurrentCalibrationInjection["COMPONENT_TYPE_CD"]);
					decimal UpsCalError = cDBConvert.ToDecimal(CurrentCalibrationInjection["UPSCALE_CAL_ERROR"]);
					DataView TestToleranceRecords = (DataView)Category.GetCheckParameter("Test_Tolerances_Cross_Check_Table").ParameterValue;
					string OldFilter = TestToleranceRecords.RowFilter;
					decimal ToleranceValue = 0;

					if (CompTypeCd.InList("CO2,O2"))
					{
						diff = Math.Round(diff, 1, MidpointRounding.AwayFromZero);
						Category.SetCheckParameter("Calibration_Upscale_Injection_Calc_Result", diff, eParameterDataType.Decimal);
						UpscaleInjCalcResult = diff;

						if (!CalTestResult.InList("INVALID,FAILED"))
							if (UpscaleInjCalcResult > (decimal)0.5)
                            {
                                Category.SetCheckParameter("Calibration_Test_Result", "FAILED", eParameterDataType.String);

                                if ((UpsCalError >= 0.0m) && (UpsCalError <= 0.5m))
                                {
                                    ToleranceValue = GetTolerance("7DAY", "DifferencePCT", Category);
                                    if (Math.Abs(diff - UpsCalError) <= ToleranceValue)
                                        Category.SetCheckParameter("Calibration_Test_Result", "PASSED", eParameterDataType.String);
                                }
							}
							else
								Category.SetCheckParameter("Calibration_Test_Result", "PASSED", eParameterDataType.String);
					}
					else
						if (CompTypeCd.InList("SO2,NOX"))
						{
							UpscaleInjCalcResult = Math.Min(Math.Round(diff / TestSpanVal * 100, 1, MidpointRounding.AwayFromZero), (decimal)9999.9);
							Category.SetCheckParameter("Calibration_Upscale_Injection_Calc_Result",
								UpscaleInjCalcResult, eParameterDataType.Decimal);
							diff = Math.Round(diff, 0, MidpointRounding.AwayFromZero);
							if ((UpscaleInjCalcResult > (decimal)2.5) && (TestSpanVal < 200) && (diff <= (decimal)5))
							{
								Category.SetCheckParameter("Calibration_Upscale_Injection_Calc_Result", diff, eParameterDataType.Decimal);
								Category.SetCheckParameter("Calibration_Upscale_Injection_Calc_APS_Indicator", 1, eParameterDataType.Integer);
								if ((CalTestResult != "INVALID") && (CalTestResult != "FAILED"))
									Category.SetCheckParameter("Calibration_Test_Result", "PASSAPS", eParameterDataType.String);
							}
							else
								if (UpscaleInjCalcResult > (decimal)2.5)
								{
									if (!CalTestResult.InList("INVALID,FAILED"))
										if (Category.GetCheckParameter("Calibration_Upscale_Injection_Calc_APS_Indicator").ValueAsInt() != 1 &&
											0 <= UpsCalError && UpsCalError <= (decimal)2.5)
										{
											ToleranceValue = GetTolerance("7DAY", "CalibrationError", Category);
											if (Math.Abs(UpscaleInjCalcResult - UpsCalError) <= ToleranceValue)
											{
												if (CalTestResult != "PASSAPS")
													Category.SetCheckParameter("Calibration_Test_Result", "PASSED", eParameterDataType.String);
											}
											else
												Category.SetCheckParameter("Calibration_Test_Result", "FAILED", eParameterDataType.String);
										}
										else
										{
											Category.SetCheckParameter("Calibration_Test_Result", "FAILED", eParameterDataType.String);
											if (Category.GetCheckParameter("Calibration_Upscale_Injection_Calc_APS_Indicator").ValueAsInt() == 1 &&
												0 <= UpsCalError && UpsCalError <= 5 && TestSpanVal < 200)
											{
												ToleranceValue = GetTolerance("7DAY", "DifferencePPM", Category);
												if (Math.Abs(diff - UpsCalError) <= ToleranceValue)
													Category.SetCheckParameter("Calibration_Test_Result", "PASSAPS", eParameterDataType.String);
											}
										}
								}
								else
									if (!CalTestResult.InList("INVALID,FAILED,PASSAPS"))
										Category.SetCheckParameter("Calibration_Test_Result", "PASSED", eParameterDataType.String);
						}
						else
							if (CompTypeCd.InList("FLOW"))
							{
								UpscaleInjCalcResult = Math.Min(Math.Round(diff / TestSpanVal * 100, 1, MidpointRounding.AwayFromZero), (decimal)9999.9);
								Category.SetCheckParameter("Calibration_Upscale_Injection_Calc_Result",
									UpscaleInjCalcResult, eParameterDataType.Decimal);
								diff = Math.Round(diff, 2, MidpointRounding.AwayFromZero);
								string AcqCd = cDBConvert.ToString(CurrentCalibrationInjection["ACQ_CD"]);
								if (UpscaleInjCalcResult > (decimal)3.0 && AcqCd == "DP" && diff <= (decimal)0.01)
								{
									Category.SetCheckParameter("Calibration_Upscale_Injection_Calc_Result", 0m, eParameterDataType.Decimal);
									Category.SetCheckParameter("Calibration_Upscale_Injection_Calc_APS_Indicator", 1, eParameterDataType.Integer);
									if ((CalTestResult != "INVALID") && (CalTestResult != "FAILED"))
										Category.SetCheckParameter("Calibration_Test_Result", "PASSAPS", eParameterDataType.String);
								}
								else
									if (UpscaleInjCalcResult > (decimal)3.0)
									{
										if (!CalTestResult.InList("INVALID,FAILED"))
											if (Category.GetCheckParameter("Calibration_Upscale_Injection_Calc_APS_Indicator").ValueAsInt() != 1 &&
												0 <= UpsCalError && UpsCalError <= 3)
											{
												ToleranceValue = GetTolerance("7DAY", "CalibrationError", Category);
												if (Math.Abs(UpscaleInjCalcResult - UpsCalError) <= ToleranceValue)
												{
													if (CalTestResult != "PASSAPS")
														Category.SetCheckParameter("Calibration_Test_Result", "PASSED", eParameterDataType.String);
												}
												else
													Category.SetCheckParameter("Calibration_Test_Result", "FAILED", eParameterDataType.String);
											}
											else
											{
												Category.SetCheckParameter("Calibration_Test_Result", "FAILED", eParameterDataType.String);
												if (Category.GetCheckParameter("Calibration_Upscale_Injection_Calc_APS_Indicator").ValueAsInt() == 1 &&
													AcqCd == "DP" && 0 <= UpsCalError && UpsCalError <= (decimal)0.01)
												{
													ToleranceValue = GetTolerance("7DAY", "DifferenceINH2O", Category);
													if (Math.Abs(diff - UpsCalError) <= ToleranceValue)
														Category.SetCheckParameter("Calibration_Test_Result", "PASSAPS", eParameterDataType.String);
												}
											}
									}
									else
										if (!CalTestResult.InList("INVALID,FAILED,PASSAPS"))
											Category.SetCheckParameter("Calibration_Test_Result", "PASSED", eParameterDataType.String);
							}
							else
								if (CompTypeCd == "HG")
								{
									UpscaleInjCalcResult = Math.Min(Math.Round(diff / TestSpanVal * 100, 1, MidpointRounding.AwayFromZero), (decimal)9999.9);
									Category.SetCheckParameter("Calibration_Upscale_Injection_Calc_Result", UpscaleInjCalcResult, eParameterDataType.Decimal);
									diff = Math.Round(diff, 1, MidpointRounding.AwayFromZero);
									if (UpscaleInjCalcResult > (decimal)5 && TestSpanVal <= 10 && diff <= (decimal)1)
									{
										Category.SetCheckParameter("Calibration_Upscale_Injection_Calc_Result", diff, eParameterDataType.Decimal);
										Category.SetCheckParameter("Calibration_Upscale_Injection_Calc_APS_Indicator", 1, eParameterDataType.Integer);
										if ((CalTestResult != "INVALID") && (CalTestResult != "FAILED"))
											Category.SetCheckParameter("Calibration_Test_Result", "PASSAPS", eParameterDataType.String);
									}
									else
										if (UpscaleInjCalcResult > (decimal)5)
										{
											if (!CalTestResult.InList("INVALID,FAILED"))
												if (Category.GetCheckParameter("Calibration_Upscale_Injection_Calc_APS_Indicator").ValueAsInt() != 1 &&
													0 <= UpsCalError && UpsCalError <= 5)
												{
													ToleranceValue = GetTolerance("7DAY", "CalibrationError", Category);
													if (Math.Abs(UpscaleInjCalcResult - UpsCalError) <= ToleranceValue)
													{
														if (CalTestResult != "PASSAPS")
															Category.SetCheckParameter("Calibration_Test_Result", "PASSED", eParameterDataType.String);
													}
													else
														Category.SetCheckParameter("Calibration_Test_Result", "FAILED", eParameterDataType.String);
												}
												else
												{
													Category.SetCheckParameter("Calibration_Test_Result", "FAILED", eParameterDataType.String);
													if (Category.GetCheckParameter("Calibration_Upscale_Injection_Calc_APS_Indicator").ValueAsInt() == 1 &&
														0 <= UpsCalError && UpsCalError <= 1 && TestSpanVal < 10)
													{
														ToleranceValue = GetTolerance("7DAY", "DifferenceUGSCM", Category);
														if (Math.Abs(diff - UpsCalError) <= ToleranceValue)
															Category.SetCheckParameter("Calibration_Test_Result", "PASSAPS", eParameterDataType.String);
													}
												}
										}
										else
											if (!CalTestResult.InList("INVALID,FAILED,PASSAPS"))
												Category.SetCheckParameter("Calibration_Test_Result", "PASSED", eParameterDataType.String);
								}
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "SEVNDAY16");
			}

			return ReturnVal;
		}

		public static string SEVNDAY17(cCategory Category, ref bool Log)
		//Reported Zero Injection Results Consistent with Recalculated Values
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentCalibrationInjection = (DataRowView)Category.GetCheckParameter("Current_Calibration_Injection").ParameterValue;
				int RecordZeroAPSInd = cDBConvert.ToInteger(CurrentCalibrationInjection["ZERO_APS_IND"]);
				if (RecordZeroAPSInd == 1 && cDBConvert.ToString(CurrentCalibrationInjection["COMPONENT_TYPE_CD"]).InList("FLOW") &&
					!cDBConvert.ToString(CurrentCalibrationInjection["ACQ_CD"]).InList("DP"))
					Category.CheckCatalogResult = "A";
				else
					if (RecordZeroAPSInd == 1 && cDBConvert.ToString(CurrentCalibrationInjection["COMPONENT_TYPE_CD"]).InList("SO2,NOX") &&
						Convert.ToDecimal(Category.GetCheckParameter("Test_Span_Value").ParameterValue) >= 200)
						Category.CheckCatalogResult = "B";
					else
						if (RecordZeroAPSInd == 1 && cDBConvert.ToString(CurrentCalibrationInjection["COMPONENT_TYPE_CD"]).InList("CO2,O2"))
							Category.CheckCatalogResult = "C";
						else
						{
							if (Convert.ToBoolean(Category.GetCheckParameter("Calculate_Zero_Calibration_Injection").ParameterValue))
							{
								int ParameterAPSInd = Category.GetCheckParameter("Calibration_Zero_Injection_Calc_APS_Indicator").ValueAsInt();
								if ((RecordZeroAPSInd != 1) && ParameterAPSInd == 1)
									Category.CheckCatalogResult = "D";
								else
									if (cDBConvert.ToDecimal(CurrentCalibrationInjection["ZERO_CAL_ERROR"]) >= 0)
									{
										DataView TestToleranceRecords = (DataView)Category.GetCheckParameter("Test_Tolerances_Cross_Check_Table").ParameterValue;
										string OldFilter = TestToleranceRecords.RowFilter;
										decimal ToleranceValue = 0;

										if (cDBConvert.ToString(CurrentCalibrationInjection["COMPONENT_TYPE_CD"]).InList("CO2,O2"))
										{
											TestToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, "FieldDescription = '" + "DifferencePCT'");
											ToleranceValue = cDBConvert.ToDecimal(((DataRowView)TestToleranceRecords[0])["Tolerance"]);
											if (Math.Abs((Decimal)Category.GetCheckParameter("Calibration_Zero_Injection_Calc_Result").ParameterValue -
												cDBConvert.ToDecimal(CurrentCalibrationInjection["ZERO_CAL_ERROR"])) > ToleranceValue)
												Category.CheckCatalogResult = "E";
										}
										else
										{
											if (ParameterAPSInd == 1)
											{
												if (cDBConvert.ToString(CurrentCalibrationInjection["COMPONENT_TYPE_CD"]).InList("FLOW"))
												{
													TestToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, "FieldDescription = '" + "DifferenceINH2O'");
													ToleranceValue = cDBConvert.ToDecimal(((DataRowView)TestToleranceRecords[0])["Tolerance"]);
													if (Math.Abs((Decimal)Category.GetCheckParameter("Calibration_Zero_Injection_Calc_Result").ParameterValue -
														cDBConvert.ToDecimal(CurrentCalibrationInjection["ZERO_CAL_ERROR"])) > ToleranceValue)
														Category.CheckCatalogResult = "E";
												}
												else
												{
													TestToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, "FieldDescription = '" + "DifferencePPM'");
													ToleranceValue = cDBConvert.ToDecimal(((DataRowView)TestToleranceRecords[0])["Tolerance"]);
													if (Math.Abs((Decimal)Category.GetCheckParameter("Calibration_Zero_Injection_Calc_Result").ParameterValue -
														cDBConvert.ToDecimal(CurrentCalibrationInjection["ZERO_CAL_ERROR"])) > ToleranceValue)
														Category.CheckCatalogResult = "E";
												}
											}
											else
												if (RecordZeroAPSInd == 0)
												{
													TestToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, "FieldDescription = '" + "CalibrationError'");
													ToleranceValue = cDBConvert.ToDecimal(((DataRowView)TestToleranceRecords[0])["Tolerance"]);
													if (Math.Abs((Decimal)Category.GetCheckParameter("Calibration_Zero_Injection_Calc_Result").ParameterValue -
														cDBConvert.ToDecimal(CurrentCalibrationInjection["ZERO_CAL_ERROR"])) > ToleranceValue)
														Category.CheckCatalogResult = "F";
												}
										}
										TestToleranceRecords.RowFilter = OldFilter;
									}
							}
						}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "SEVNDAY17");
			}

			return ReturnVal;
		}

		public static string SEVNDAY18(cCategory Category, ref bool Log)
		//Reported Upscale Injection Results Consistent with Recalculated Values
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentCalibrationInjection = (DataRowView)Category.GetCheckParameter("Current_Calibration_Injection").ParameterValue;
				int RecordUpscaleAPSInd = cDBConvert.ToInteger(CurrentCalibrationInjection["UPSCALE_APS_IND"]);
				if (RecordUpscaleAPSInd == 1 && cDBConvert.ToString(CurrentCalibrationInjection["COMPONENT_TYPE_CD"]).InList("FLOW") &&
					!cDBConvert.ToString(CurrentCalibrationInjection["ACQ_CD"]).InList("DP"))
					Category.CheckCatalogResult = "A";
				else
					if (RecordUpscaleAPSInd == 1 && cDBConvert.ToString(CurrentCalibrationInjection["COMPONENT_TYPE_CD"]).InList("SO2,NOX") &&
						cDBConvert.ToDecimal(Category.GetCheckParameter("Test_Span_Value").ParameterValue) >= 200)
						Category.CheckCatalogResult = "B";
					else
						if (RecordUpscaleAPSInd == 1 && cDBConvert.ToString(CurrentCalibrationInjection["COMPONENT_TYPE_CD"]).InList("CO2,O2"))
							Category.CheckCatalogResult = "C";
						else
						{
							if (Convert.ToBoolean(Category.GetCheckParameter("Calculate_Upscale_Calibration_Injection").ParameterValue))
							{
								int ParameterAPSInd = Category.GetCheckParameter("Calibration_Upscale_Injection_Calc_APS_Indicator").ValueAsInt();
								if ((RecordUpscaleAPSInd != 1) && ParameterAPSInd == 1)
									Category.CheckCatalogResult = "D";
								else
									if (cDBConvert.ToDecimal(CurrentCalibrationInjection["UPSCALE_CAL_ERROR"]) >= 0)
									{
										DataView TestToleranceRecords = (DataView)Category.GetCheckParameter("Test_Tolerances_Cross_Check_Table").ParameterValue;
										string OldFilter = TestToleranceRecords.RowFilter;
										decimal ToleranceValue = 0;

										if (cDBConvert.ToString(CurrentCalibrationInjection["COMPONENT_TYPE_CD"]).InList("CO2,O2"))
										{
											TestToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, "FieldDescription = '" + "DifferencePCT'");
											ToleranceValue = cDBConvert.ToDecimal(((DataRowView)TestToleranceRecords[0])["Tolerance"]);
											if (Math.Abs(Convert.ToDecimal(Category.GetCheckParameter("Calibration_Upscale_Injection_Calc_Result").ParameterValue) -
												cDBConvert.ToDecimal(CurrentCalibrationInjection["UPSCALE_CAL_ERROR"])) > ToleranceValue)
												Category.CheckCatalogResult = "E";
										}
										else
										{
											if (ParameterAPSInd == 1)
											{
												if (cDBConvert.ToString(CurrentCalibrationInjection["COMPONENT_TYPE_CD"]).InList("FLOW"))
												{
													TestToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, "FieldDescription = '" + "DifferenceINH2O'");
													ToleranceValue = cDBConvert.ToDecimal(((DataRowView)TestToleranceRecords[0])["Tolerance"]);
													if (Math.Abs((Decimal)Category.GetCheckParameter("Calibration_Upscale_Injection_Calc_Result").ParameterValue -
														cDBConvert.ToDecimal(CurrentCalibrationInjection["UPSCALE_CAL_ERROR"])) > ToleranceValue)
														Category.CheckCatalogResult = "E";
												}
												else
													if (cDBConvert.ToString(CurrentCalibrationInjection["COMPONENT_TYPE_CD"]).InList("HG"))
													{
														TestToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, "FieldDescription = '" + "DifferenceUGSCM'");
														ToleranceValue = cDBConvert.ToDecimal(((DataRowView)TestToleranceRecords[0])["Tolerance"]);
														if (Math.Abs((Decimal)Category.GetCheckParameter("Calibration_Upscale_Injection_Calc_Result").ParameterValue -
															cDBConvert.ToDecimal(CurrentCalibrationInjection["UPSCALE_CAL_ERROR"])) > ToleranceValue)
															Category.CheckCatalogResult = "E";
													}
													else
													{
														TestToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, "FieldDescription = '" + "DifferencePPM'");
														ToleranceValue = cDBConvert.ToDecimal(((DataRowView)TestToleranceRecords[0])["Tolerance"]);
														if (Math.Abs((Decimal)Category.GetCheckParameter("Calibration_Upscale_Injection_Calc_Result").ParameterValue -
															cDBConvert.ToDecimal(CurrentCalibrationInjection["UPSCALE_CAL_ERROR"])) > ToleranceValue)
															Category.CheckCatalogResult = "E";
													}
											}
											else
												if (RecordUpscaleAPSInd == 0)
												{
													TestToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, "FieldDescription = '" + "CalibrationError'");
													ToleranceValue = cDBConvert.ToDecimal(((DataRowView)TestToleranceRecords[0])["Tolerance"]);
													if (Math.Abs((Decimal)Category.GetCheckParameter("Calibration_Upscale_Injection_Calc_Result").ParameterValue -
														cDBConvert.ToDecimal(CurrentCalibrationInjection["UPSCALE_CAL_ERROR"])) > ToleranceValue)
														Category.CheckCatalogResult = "F";
												}
										}
										TestToleranceRecords.RowFilter = OldFilter;
									}
							}
						}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "SEVNDAY18");
			}

			return ReturnVal;
		}


		public static string SEVNDAY19(cCategory Category, ref bool Log)
		//7-Day Calibration Test Begin Time Consistent with Injection Times
		{
			string ReturnVal = "";

			try
			{
				DataRowView Current7DayCalibrationTest = (DataRowView)Category.GetCheckParameter("Current_7Day_Calibration_Test").ParameterValue;
				if (Convert.ToBoolean(Category.GetCheckParameter("Calibration_Injection_Times_Valid").ParameterValue) &&
					Convert.ToBoolean(Category.GetCheckParameter("Test_Begin_Date_Valid").ParameterValue) &&
					Convert.ToBoolean(Category.GetCheckParameter("Test_Begin_Hour_Valid").ParameterValue) &&
					Convert.ToBoolean(Category.GetCheckParameter("Test_Begin_Minute_Valid").ParameterValue) &&
					Convert.ToInt16(Category.GetCheckParameter("Calibration_Injection_Count").ParameterValue) > 0)
					if (cDBConvert.ToDate(Current7DayCalibrationTest["BEGIN_DATE"], DateTypes.START) !=
						Convert.ToDateTime(Category.GetCheckParameter("Calibration_Test_Begin_Date").ParameterValue) ||
						cDBConvert.ToHour(Current7DayCalibrationTest["BEGIN_HOUR"], DateTypes.START) !=
						Convert.ToInt32(Category.GetCheckParameter("Calibration_Test_Begin_Hour").ParameterValue) ||
						cDBConvert.ToInteger(Current7DayCalibrationTest["BEGIN_MIN"]) !=
						Convert.ToInt32(Category.GetCheckParameter("Calibration_Test_Begin_Minute").ParameterValue))
						Category.CheckCatalogResult = "A";
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "SEVNDAY19");
			}

			return ReturnVal;
		}

		public static string SEVNDAY20(cCategory Category, ref bool Log)
		//7-Day Calibration Test End Time Consistent with Injection Times
		{
			string ReturnVal = "";

			try
			{
				DataRowView Current7DayCalibrationTest = (DataRowView)Category.GetCheckParameter("Current_7Day_Calibration_Test").ParameterValue;
				if (Convert.ToBoolean(Category.GetCheckParameter("Calibration_Injection_Times_Valid").ParameterValue) &&
					Convert.ToBoolean(Category.GetCheckParameter("Test_End_Date_Valid").ParameterValue) &&
					Convert.ToBoolean(Category.GetCheckParameter("Test_End_Hour_Valid").ParameterValue) &&
					Convert.ToBoolean(Category.GetCheckParameter("Test_End_Minute_Valid").ParameterValue) &&
					Convert.ToInt16(Category.GetCheckParameter("Calibration_Injection_Count").ParameterValue) > 0)
					if (cDBConvert.ToDate(Current7DayCalibrationTest["END_DATE"], DateTypes.END) !=
						Convert.ToDateTime(Category.GetCheckParameter("Calibration_Test_End_Date").ParameterValue) ||
						cDBConvert.ToHour(Current7DayCalibrationTest["END_HOUR"], DateTypes.END) !=
						Convert.ToInt32(Category.GetCheckParameter("Calibration_Test_End_Hour").ParameterValue) ||
						cDBConvert.ToInteger(Current7DayCalibrationTest["END_MIN"]) !=
						Convert.ToInt32(Category.GetCheckParameter("Calibration_Test_End_Minute").ParameterValue))
						Category.CheckCatalogResult = "A";
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "SEVNDAY20");
			}

			return ReturnVal;
		}

		public static string SEVNDAY21(cCategory Category, ref bool Log)
		//Correct Number of Injections
		{
			string ReturnVal = "";

			try
			{
				if (Convert.ToInt32(Category.GetCheckParameter("Calibration_Injection_Count").ParameterValue) < 7)
				{
					Category.SetCheckParameter("Calibration_Test_Result", "INVALID", eParameterDataType.String);
					Category.CheckCatalogResult = "A";
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "SEVNDAY21");
			}

			return ReturnVal;
		}

		public static string SEVNDAY22(cCategory Category, ref bool Log)
		//Upscale Gas Level Codes Valid
		{
			string ReturnVal = "";

			try
			{
				DataRowView Current7DayCalibrationTest = (DataRowView)Category.GetCheckParameter("Current_7Day_Calibration_Test").ParameterValue;
				if (Convert.ToString(Category.GetCheckParameter("Calibration_Upscale_Gas_Level_Code").ParameterValue).ListCount() > 1)
				{
					Category.SetCheckParameter("Calibration_Test_Result", "INVALID", eParameterDataType.String);
					Category.CheckCatalogResult = "A";
				}
				else
					if (cDBConvert.ToString(Current7DayCalibrationTest["COMPONENT_TYPE_CD"]) == "FLOW" &&
						Convert.ToString(Category.GetCheckParameter("Calibration_Upscale_Gas_Level_Code").ParameterValue) == "MID")
					{
						Category.SetCheckParameter("Calibration_Test_Result", "INVALID", eParameterDataType.String);
						Category.CheckCatalogResult = "B";
					}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "SEVNDAY22");
			}

			return ReturnVal;
		}

		public static string SEVNDAY23(cCategory Category, ref bool Log)
		//Reference Values Consistent with Gas Level
		{
			string ReturnVal = "";

			try
			{
				if (Category.GetCheckParameter("Calibration_Maximum_Zero_Reference_Value").ParameterValue != null &&
					Category.GetCheckParameter("Calibration_Minimum_Upscale_Reference_Value").ParameterValue != null)
				{
					decimal MaxZeroRefVal = Convert.ToDecimal(Category.GetCheckParameter("Calibration_Maximum_Zero_Reference_Value").ParameterValue);
					decimal MinUpscaleRefVal = Convert.ToDecimal(Category.GetCheckParameter("Calibration_Minimum_Upscale_Reference_Value").ParameterValue);
					if (MaxZeroRefVal >= MinUpscaleRefVal)
					{
						Category.SetCheckParameter("Calibration_Test_Result", "INVALID", eParameterDataType.String);
						Category.CheckCatalogResult = "A";
					}
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "SEVNDAY23");
			}

			return ReturnVal;
		}

		public static string SEVNDAY24(cCategory Category, ref bool Log)
		//Zero Injection Reference Value Consistent with Span
		{
			string ReturnVal = "";

			try
			{
				if (Category.GetCheckParameter("Calibration_Maximum_Zero_Reference_Value").ParameterValue != null &&
					Category.GetCheckParameter("Test_Span_Value").ParameterValue != null)
				{
					if (QaParameters.Current7DayCalibrationTest.ComponentTypeCd != "HG")
					{
						decimal MaxZeroRefVal = Convert.ToDecimal(Category.GetCheckParameter("Calibration_Maximum_Zero_Reference_Value").ParameterValue);
						decimal TestSpanVal = Convert.ToDecimal(Category.GetCheckParameter("Test_Span_Value").ParameterValue);
						decimal ZeroRefPercOfSpan = Math.Round((MaxZeroRefVal / TestSpanVal) * 100, 1, MidpointRounding.AwayFromZero);
						Category.SetCheckParameter("Calibration_Zero_Reference_Value_Percent_of_Span", ZeroRefPercOfSpan, eParameterDataType.Decimal);
						if (ZeroRefPercOfSpan > (decimal)20.0)
						{
							DataView TestToleranceRecords = (DataView)Category.GetCheckParameter("Test_Tolerances_Cross_Check_Table").ParameterValue;
							string OldFilter = TestToleranceRecords.RowFilter;
							TestToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, "FieldDescription = '" + "GasPercentOfSpan'");
							decimal Tolerance = cDBConvert.ToDecimal(((DataRowView)TestToleranceRecords[0])["Tolerance"]);
							TestToleranceRecords.RowFilter = OldFilter;
							if (ZeroRefPercOfSpan > (decimal)20.0 + Tolerance)
								Category.CheckCatalogResult = "A";
							else
								Category.CheckCatalogResult = "B";
						}
					}
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "SEVNDAY24");
			}

			return ReturnVal;
		}

		public static string SEVNDAY25(cCategory Category, ref bool Log)
		//Upscale Injection Reference Value Consistent with Span          
		{
			string ReturnVal = "";

			try
			{
				DataRowView Current7DayCalibrationTest = (DataRowView)Category.GetCheckParameter("Current_7Day_Calibration_Test").ParameterValue;
				if (Category.GetCheckParameter("Test_Span_Value").ParameterValue != null &&
					Category.GetCheckParameter("Calibration_Minimum_Upscale_Reference_Value").ParameterValue != null)
				{
					decimal MaxUpscaleRefVal = Convert.ToDecimal(Category.GetCheckParameter("Calibration_Maximum_Upscale_Reference_Value").ParameterValue);
					decimal MinUpscaleRefVal = Convert.ToDecimal(Category.GetCheckParameter("Calibration_Minimum_Upscale_Reference_Value").ParameterValue);
					decimal TestSpanVal = Convert.ToDecimal(Category.GetCheckParameter("Test_Span_Value").ParameterValue);
					decimal UpscaleRefPercOfSpan = Math.Round((MaxUpscaleRefVal / TestSpanVal) * 100, 1, MidpointRounding.AwayFromZero);
					Category.SetCheckParameter("Calibration_Upscale_Reference_Value_Percent_of_Span", UpscaleRefPercOfSpan, eParameterDataType.Decimal);
					string UpscaleGasLevelCd = Convert.ToString(Category.GetCheckParameter("Calibration_Upscale_Gas_Level_Code").ParameterValue);
					string CompTypeCd = cDBConvert.ToString(Current7DayCalibrationTest["COMPONENT_TYPE_CD"]);
					bool NonCritical = false;

					DataView TestToleranceRecords = (DataView)Category.GetCheckParameter("Test_Tolerances_Cross_Check_Table").ParameterValue;
					string OldFilter = TestToleranceRecords.RowFilter;
					TestToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, "FieldDescription = '" + "GasPercentOfSpan'");
					decimal Tolerance = cDBConvert.ToDecimal(((DataRowView)TestToleranceRecords[0])["Tolerance"]);
					TestToleranceRecords.RowFilter = OldFilter;
					decimal TempVal = Math.Round(100 * MinUpscaleRefVal / TestSpanVal, 1, MidpointRounding.AwayFromZero);

					if (UpscaleGasLevelCd == "MID" && CompTypeCd != "FLOW")
					{
						if (UpscaleRefPercOfSpan < (decimal)50.0 || UpscaleRefPercOfSpan > (decimal)60.0)
							if (UpscaleRefPercOfSpan < ((decimal)50.0 - Tolerance) || UpscaleRefPercOfSpan > ((decimal)60.0 + Tolerance))
								Category.CheckCatalogResult = "A";
							else
								NonCritical = true;
						if (string.IsNullOrEmpty(Category.CheckCatalogResult) && MinUpscaleRefVal != MaxUpscaleRefVal)
							if (TempVal < ((decimal)50.0 - Tolerance) || TempVal > ((decimal)60.0 + Tolerance))
							{
								Category.SetCheckParameter("Calibration_Upscale_Reference_Value_Percent_of_Span", TempVal, eParameterDataType.Decimal);
								Category.CheckCatalogResult = "A";
							}
							else
								if (TempVal < (decimal)50.0 || TempVal > (decimal)60.0)
								{
									Category.SetCheckParameter("Calibration_Upscale_Reference_Value_Percent_of_Span", TempVal, eParameterDataType.Decimal);
									NonCritical = true;
								}
						if (string.IsNullOrEmpty(Category.CheckCatalogResult) && NonCritical)
							Category.CheckCatalogResult = "B";
					}
					else
					{
						if (UpscaleGasLevelCd == "HIGH")
							if (CompTypeCd == "FLOW")
							{
								if (UpscaleRefPercOfSpan < (decimal)50.0 || UpscaleRefPercOfSpan > (decimal)70.0)
									if (UpscaleRefPercOfSpan < ((decimal)50.0 - Tolerance) || UpscaleRefPercOfSpan > ((decimal)70.0 + Tolerance))
										Category.CheckCatalogResult = "C";
									else
										NonCritical = true;
								if (string.IsNullOrEmpty(Category.CheckCatalogResult) && MinUpscaleRefVal != MaxUpscaleRefVal)
									if (TempVal < ((decimal)50.0 - Tolerance) || TempVal > ((decimal)70.0 + Tolerance))
									{
										Category.SetCheckParameter("Calibration_Upscale_Reference_Value_Percent_of_Span", TempVal, eParameterDataType.Decimal);
										Category.CheckCatalogResult = "C";
									}
									else
										if (TempVal < (decimal)50.0 || TempVal > (decimal)60.0)
										{
											Category.SetCheckParameter("Calibration_Upscale_Reference_Value_Percent_of_Span", TempVal, eParameterDataType.Decimal);
											NonCritical = true;
										}
								if (string.IsNullOrEmpty(Category.CheckCatalogResult) && NonCritical)
									Category.CheckCatalogResult = "D";
							}
							else
								if (UpscaleRefPercOfSpan > (decimal)100.0)
									Category.CheckCatalogResult = "E";
								else
								{
									if (UpscaleRefPercOfSpan < (decimal)80.0)
										if (UpscaleRefPercOfSpan < (decimal)80.0 - Tolerance)
											Category.CheckCatalogResult = "E";
										else
											NonCritical = true;
									if (MinUpscaleRefVal != MaxUpscaleRefVal)
										if (TempVal < ((decimal)80.0 - Tolerance) || TempVal > (decimal)100.0)
										{
											Category.SetCheckParameter("Calibration_Upscale_Reference_Value_Percent_of_Span", TempVal, eParameterDataType.Decimal);
											Category.CheckCatalogResult = "E";
										}
										else
											if (TempVal < (decimal)80.0)
											{
												Category.SetCheckParameter("Calibration_Upscale_Reference_Value_Percent_of_Span", TempVal, eParameterDataType.Decimal);
												NonCritical = true;
											}
									if (string.IsNullOrEmpty(Category.CheckCatalogResult) && NonCritical)
										Category.CheckCatalogResult = "F";
								}
					}
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "SEVNDAY25");
			}

			return ReturnVal;
		}

		public static string SEVNDAY26(cCategory Category, ref bool Log)
		//Injections Performed at Appropriate Times
		{
			string ReturnVal = "";

			try
			{
				Category.SetCheckParameter("Calibration_Test_Validity_Performed", true, eParameterDataType.Boolean);
				if (!Convert.ToBoolean(Category.GetCheckParameter("Calibration_Injection_Times_Appropriate").ParameterValue))
				{
					Category.SetCheckParameter("Calibration_Test_Result", "INVALID", eParameterDataType.String);
					Category.CheckCatalogResult = "A";
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "SEVNDAY26");
			}

			return ReturnVal;
		}

		public static string SEVNDAY27(cCategory Category, ref bool Log)
		//Determination of Overall 7-Day Calibration Test Status
		{
			string ReturnVal = "";

			try
			{
				DataRowView Current7DayCalibrationTest = (DataRowView)Category.GetCheckParameter("Current_7Day_Calibration_Test").ParameterValue;
				string TestCalcRes = Convert.ToString(Category.GetCheckParameter("Calibration_Test_Result").ParameterValue);
				string TestRes = cDBConvert.ToString(Current7DayCalibrationTest["TEST_RESULT_CD"]);
				if ((TestCalcRes) == "INVALID")
					Category.SetCheckParameter("Calibration_Test_Result", null, eParameterDataType.String);
				if (TestRes == "")
					Category.CheckCatalogResult = "A";
				else
					if (!TestRes.InList("PASSED,PASSAPS,FAILED,ABORTED"))
					{
						DataView TestResultLookup = (DataView)Category.GetCheckParameter("Test_Result_Code_Lookup_Table").ParameterValue;
						string OldFilter = TestResultLookup.RowFilter;
						TestResultLookup.RowFilter = AddToDataViewFilter(OldFilter, "TEST_RESULT_CD = '" + TestRes + "'");
						if (TestResultLookup.Count == 0)
							Category.CheckCatalogResult = "B";
						else
							Category.CheckCatalogResult = "C";
						TestResultLookup.RowFilter = OldFilter;
					}
				if (string.IsNullOrEmpty(Category.CheckCatalogResult))
				{
					if (TestCalcRes == "FAILED")
						if (TestRes.InList("PASSED,PASSAPS"))
							Category.CheckCatalogResult = "D";
						else
							Category.CheckCatalogResult = "E";
					else
						if (TestCalcRes.InList("PASSED,PASSAPS") && TestRes == "FAILED")
							Category.CheckCatalogResult = "F";
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "SEVNDAY27");
			}

			return ReturnVal;
		}


		public static string SEVNDAY28(cCategory Category, ref bool Log)
		//7-Day Calibration Test Result Code Valid
		{
			string ReturnVal = "";

			try
			{
				DataRowView Current7DayCalibrationTest = (DataRowView)Category.GetCheckParameter("Current_7Day_Calibration_Test").ParameterValue;
				string CalTestRes = cDBConvert.ToString(Current7DayCalibrationTest["TEST_RESULT_CD"]);
				if (CalTestRes == "")
					Category.CheckCatalogResult = "A";
				else
					if (!CalTestRes.InList("ABORTED,PASSED,PASSAPS,FAILED"))
					{
						DataView TestResultLookup = (DataView)Category.GetCheckParameter("Test_Result_Code_Lookup_Table").ParameterValue;
						string OldFilter = TestResultLookup.RowFilter;
						TestResultLookup.RowFilter = AddToDataViewFilter(OldFilter, "TEST_RESULT_CD = '" + CalTestRes + "'");
						if (TestResultLookup.Count == 0)
							Category.CheckCatalogResult = "B";
						else
							Category.CheckCatalogResult = "C";
						TestResultLookup.RowFilter = OldFilter;
					}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "SEVNDAY28");
			}

			return ReturnVal;
		}

		public static string SEVNDAY29(cCategory Category, ref bool Log)
		//Duplicate Calibration Test
		{
			string ReturnVal = "";

			try
			{
				if (Convert.ToBoolean(Category.GetCheckParameter("Test_Number_Valid").ParameterValue))
				{
					DataRowView Current7DayCalibrationTest = (DataRowView)Category.GetCheckParameter("Current_7Day_Calibration_Test").ParameterValue;
					string TestNumber = cDBConvert.ToString(Current7DayCalibrationTest["TEST_NUM"]);
					DataView LocationTestRecords = (DataView)Category.GetCheckParameter("Location_Test_Records").ParameterValue;
					string OldFilter = LocationTestRecords.RowFilter;
					LocationTestRecords.RowFilter = AddToDataViewFilter(OldFilter, "TEST_NUM = '" + TestNumber + "' AND TEST_TYPE_CD = '7DAY'");
					if ((LocationTestRecords.Count > 0 && Current7DayCalibrationTest["TEST_SUM_ID"] == DBNull.Value) ||
						(LocationTestRecords.Count > 1 && Current7DayCalibrationTest["TEST_SUM_ID"] != DBNull.Value) ||
						(LocationTestRecords.Count == 1 && Current7DayCalibrationTest["TEST_SUM_ID"] != DBNull.Value && Current7DayCalibrationTest["TEST_SUM_ID"].ToString() != LocationTestRecords[0]["TEST_SUM_ID"].ToString()))
					{
						Category.SetCheckParameter("Duplicate_7Day_Calibration", true, eParameterDataType.Boolean);
						Category.CheckCatalogResult = "A";
					}
					else
					{
						string TestSumID = cDBConvert.ToString(Current7DayCalibrationTest["TEST_SUM_ID"]);
						if (TestSumID != "")
						{
							DataView QASuppRecords = (DataView)Category.GetCheckParameter("QA_Supplemental_Data_Records").ParameterValue;
							string OldFilter2 = QASuppRecords.RowFilter;
							QASuppRecords.RowFilter = AddToDataViewFilter(OldFilter2, "TEST_NUM = '" + TestNumber + "' AND TEST_TYPE_CD = '7DAY'");
							if (QASuppRecords.Count > 0 && cDBConvert.ToString(QASuppRecords[0]["TEST_SUM_ID"]) != TestSumID)
							{
								Category.SetCheckParameter("Duplicate_7Day_Calibration", true, eParameterDataType.Boolean);
								Category.CheckCatalogResult = "B";
							}
							else
								Category.SetCheckParameter("Duplicate_7Day_Calibration", false, eParameterDataType.Boolean);
							QASuppRecords.RowFilter = OldFilter2;
						}
						else
							Category.SetCheckParameter("Duplicate_7Day_Calibration", false, eParameterDataType.Boolean);
					}
					LocationTestRecords.RowFilter = OldFilter;
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "SEVNDAY29");
			}

			return ReturnVal;
		}

		public static string SEVNDAY30(cCategory Category, ref bool Log)
		//Component ID Valid
		{
			string ReturnVal = "";

			try
			{
				DataRowView Current7DayCalibrationTest = (DataRowView)Category.GetCheckParameter("Current_7Day_Calibration_Test").ParameterValue;
				if (Current7DayCalibrationTest["COMPONENT_ID"] == DBNull.Value)
					Category.CheckCatalogResult = "A";
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "SEVNDAY30");
			}

			return ReturnVal;
		}

		public static string SEVNDAY31(cCategory Category, ref bool Log)
		//Online Offline Indicator Valid
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentCalibrationInjection = (DataRowView)Category.GetCheckParameter("Current_Calibration_Injection").ParameterValue;
				int OnOffInd = cDBConvert.ToInteger(CurrentCalibrationInjection["ONLINE_OFFLINE_IND"]);
				if (OnOffInd == int.MinValue)
					Category.CheckCatalogResult = "A";
				else
					if (OnOffInd == 0)
						Category.CheckCatalogResult = "B";
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "SEVNDAY31");
			}

			return ReturnVal;
		}

		public static string SEVNDAY32(cCategory Category, ref bool Log)
		//Zero APS Indicator Valid
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentCalibrationInjection = (DataRowView)Category.GetCheckParameter("Current_Calibration_Injection").ParameterValue;
				if (CurrentCalibrationInjection["ZERO_APS_IND"] == DBNull.Value)
					Category.CheckCatalogResult = "A";
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "SEVNDAY32");
			}

			return ReturnVal;
		}
		public static string SEVNDAY33(cCategory Category, ref bool Log)
		//Upscale APS Indicator Valid
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentCalibrationInjection = (DataRowView)Category.GetCheckParameter("Current_Calibration_Injection").ParameterValue;
				if (CurrentCalibrationInjection["UPSCALE_APS_IND"] == DBNull.Value)
					Category.CheckCatalogResult = "A";
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "SEVNDAY33");
			}

			return ReturnVal;
		}

		public static string SEVNDAY34(cCategory Category, ref bool Log)
		//Calculate 7-Day Calibration
		{
			string ReturnVal = "";

			try
			{
				Category.SetCheckParameter("7DAY_Upscale_Calc_APS", null, eParameterDataType.Integer);
				Category.SetCheckParameter("7DAY_Upscale_Calc_Result", null, eParameterDataType.Decimal);
				Category.SetCheckParameter("7DAY_Zero_Calc_APS", null, eParameterDataType.Integer);
				Category.SetCheckParameter("7DAY_Zero_Calc_Result", null, eParameterDataType.Decimal);
				DataRowView CurrentCalibrationInjection = (DataRowView)Category.GetCheckParameter("Current_Calibration_Injection").ParameterValue;
				string UpsGasLvlCd = cDBConvert.ToString(CurrentCalibrationInjection["UPSCALE_GAS_LEVEL_CD"]);
				decimal UpsMeasVal = cDBConvert.ToDecimal(CurrentCalibrationInjection["UPSCALE_MEASURED_VALUE"]);
				decimal UpsRefVal = cDBConvert.ToDecimal(CurrentCalibrationInjection["UPSCALE_REF_VALUE"]);
				decimal ZeroMeasVal = cDBConvert.ToDecimal(CurrentCalibrationInjection["ZERO_MEASURED_VALUE"]);
				decimal ZeroRefVal = cDBConvert.ToDecimal(CurrentCalibrationInjection["ZERO_REF_VALUE"]);
				if (!UpsGasLvlCd.InList("MID,HIGH") || UpsMeasVal == decimal.MinValue || UpsRefVal == decimal.MinValue ||
					ZeroMeasVal == decimal.MinValue || ZeroRefVal == decimal.MinValue || ZeroRefVal >= UpsRefVal)
					Category.CheckCatalogResult = "A";
				else
				{
					DataRowView Current7DayCalibrationTest = (DataRowView)Category.GetCheckParameter("Current_7Day_Calibration_Test").ParameterValue;
					DataView SysCompRecs = (DataView)Category.GetCheckParameter("System_Component_Records").ParameterValue;
					SysCompRecs.Sort = "BEGIN_DATE";
					string CompTypeCd = cDBConvert.ToString(SysCompRecs[0]["COMPONENT_TYPE_CD"]);
					string SpanScaleCd = cDBConvert.ToString(Current7DayCalibrationTest["SPAN_SCALE_CD"]);
					if ((CompTypeCd == "FLOW" && SpanScaleCd != "") || (CompTypeCd != "FLOW" && !SpanScaleCd.InList("H,L")))
						Category.CheckCatalogResult = "A";
					else
					{

						DateTime CompRecBegDate = cDBConvert.ToDate(SysCompRecs[0]["BEGIN_DATE"], DateTypes.START);
						string CompRecBegDateString = CompRecBegDate.ToShortDateString();
						int CompRecBegHr = cDBConvert.ToInteger(SysCompRecs[0]["BEGIN_HOUR"]);
						DataView SpanRecs = (DataView)Category.GetCheckParameter("Span_Records").ParameterValue;
						string OldFilter = SpanRecs.RowFilter;
						DateTime TestBegDate = cDBConvert.ToDate(Current7DayCalibrationTest["BEGIN_DATE"], DateTypes.START);
						int TestBegHr = cDBConvert.ToInteger(Current7DayCalibrationTest["BEGIN_HOUR"]);

						if (CompRecBegDate != DateTime.MinValue && 0 <= CompRecBegHr && CompRecBegHr <= 23 &&
							(CompRecBegDate > TestBegDate || (CompRecBegDate == TestBegDate && CompRecBegHr > TestBegHr)))
						{
							if (SpanScaleCd != "")
								SpanRecs.RowFilter = AddToDataViewFilter(OldFilter, "COMPONENT_TYPE_CD = '" + CompTypeCd + "' AND SPAN_SCALE_CD = '" +
									SpanScaleCd + "' AND SPAN_VALUE > 0 AND (BEGIN_DATE < '" + CompRecBegDateString + "' OR (BEGIN_DATE = '" +
									CompRecBegDateString + "' AND BEGIN_HOUR <= " + CompRecBegHr + ")) AND (END_DATE IS NULL OR (END_DATE > '" +
									CompRecBegDateString + "' OR (END_DATE = '" + CompRecBegDateString + "' AND END_HOUR > " + CompRecBegHr + ")))");
							else
								SpanRecs.RowFilter = AddToDataViewFilter(OldFilter, "COMPONENT_TYPE_CD = '" + CompTypeCd + "' AND SPAN_SCALE_CD IS NULL" +
									" AND SPAN_VALUE > 0 AND (BEGIN_DATE < '" + CompRecBegDateString + "' OR (BEGIN_DATE = '" +
									CompRecBegDateString + "' AND BEGIN_HOUR <= " + CompRecBegHr + ")) AND (END_DATE IS NULL OR (END_DATE > '" +
									CompRecBegDateString + "' OR (END_DATE = '" + CompRecBegDateString + "' AND END_HOUR > " + CompRecBegHr + ")))");
						}
						else
						{
							string TestBegDateString = TestBegDate.ToShortDateString();
							string TestEndDateString = cDBConvert.ToDate(Current7DayCalibrationTest["END_DATE"], DateTypes.END).ToShortDateString();
							int TestEndHr = cDBConvert.ToInteger(Current7DayCalibrationTest["END_HOUR"]);
							if (SpanScaleCd != "")
								SpanRecs.RowFilter = AddToDataViewFilter(OldFilter, "COMPONENT_TYPE_CD = '" + CompTypeCd + "' AND SPAN_SCALE_CD = '" +
									SpanScaleCd + "' AND SPAN_VALUE > 0 AND (BEGIN_DATE < '" + TestBegDateString + "' OR (BEGIN_DATE = '" +
									TestBegDateString + "' AND BEGIN_HOUR <= " + TestBegHr + ")) AND (END_DATE IS NULL OR (END_DATE > '" +
									TestEndDateString + "' OR (END_DATE = '" + TestEndDateString + "' AND END_HOUR > " + TestEndHr + ")))");
							else
								SpanRecs.RowFilter = AddToDataViewFilter(OldFilter, "COMPONENT_TYPE_CD = '" + CompTypeCd + "' AND SPAN_SCALE_CD IS NULL" +
									" AND SPAN_VALUE > 0 AND (BEGIN_DATE < '" + TestBegDateString + "' OR (BEGIN_DATE = '" +
									TestBegDateString + "' AND BEGIN_HOUR <= " + TestBegHr + ")) AND (END_DATE IS NULL OR (END_DATE > '" +
									TestEndDateString + "' OR (END_DATE = '" + TestEndDateString + "' AND END_HOUR > " + TestEndHr + ")))");
						}
						if (SpanRecs.Count == 0)
							Category.CheckCatalogResult = "B";
						else
						{
							decimal diff = Math.Abs(cDBConvert.ToDecimal(CurrentCalibrationInjection["ZERO_MEASURED_VALUE"]) -
								cDBConvert.ToDecimal(CurrentCalibrationInjection["ZERO_REF_VALUE"]));
							decimal SpanVal = cDBConvert.ToDecimal(SpanRecs[0]["SPAN_VALUE"]);
							string AcqCd = cDBConvert.ToString(SysCompRecs[0]["ACQ_CD"]);
							Category.SetCheckParameter("7DAY_Zero_Calc_APS", 0, eParameterDataType.Integer);
							if (CompTypeCd.InList("CO2,O2"))
							{
								diff = Math.Round(diff, 1, MidpointRounding.AwayFromZero);
								Category.SetCheckParameter("7DAY_Zero_Calc_Result", diff, eParameterDataType.Decimal);
							}
							else
							{
								decimal ZeroCalcResult;
								if (CompTypeCd.InList("SO2,NOX"))
								{
									ZeroCalcResult = Math.Min(Math.Round(diff / SpanVal * 100, 1, MidpointRounding.AwayFromZero), (decimal)9999.9);
									Category.SetCheckParameter("7DAY_Zero_Calc_Result", ZeroCalcResult, eParameterDataType.Decimal);
									diff = Math.Round(diff, MidpointRounding.AwayFromZero);
									if (ZeroCalcResult > (decimal)2.5 && SpanVal < 200 && diff <= 5)
									{
										Category.SetCheckParameter("7DAY_Zero_Calc_APS", 1, eParameterDataType.Integer);
										Category.SetCheckParameter("7DAY_Zero_Calc_Result", diff, eParameterDataType.Decimal);
									}
								}
								else
									if (CompTypeCd == "FLOW")
									{
										ZeroCalcResult = Math.Min(Math.Round(diff / SpanVal * 100, 1, MidpointRounding.AwayFromZero), (decimal)9999.9);
										Category.SetCheckParameter("7DAY_Zero_Calc_Result", ZeroCalcResult, eParameterDataType.Decimal);
										diff = Math.Round(diff, 2, MidpointRounding.AwayFromZero);
										if (ZeroCalcResult > 3 && AcqCd == "DP" && diff <= (decimal)0.01)
										{
											Category.SetCheckParameter("7DAY_Zero_Calc_APS", 1, eParameterDataType.Integer);
											Category.SetCheckParameter("7DAY_Zero_Calc_Result", 0m, eParameterDataType.Decimal);
										}
									}
									else
										if (CompTypeCd == "HG")
										{
											ZeroCalcResult = Math.Min(Math.Round(diff / SpanVal * 100, 1, MidpointRounding.AwayFromZero), (decimal)9999.9);
											Category.SetCheckParameter("7DAY_Zero_Calc_Result", ZeroCalcResult, eParameterDataType.Decimal);
											diff = Math.Round(diff, 1, MidpointRounding.AwayFromZero);
											if (ZeroCalcResult > (decimal)5 && SpanVal <= 10 && diff <= 1)
											{
												Category.SetCheckParameter("7DAY_Zero_Calc_APS", 1, eParameterDataType.Integer);
												Category.SetCheckParameter("7DAY_Zero_Calc_Result", diff, eParameterDataType.Decimal);
											}
										}
							}
							diff = Math.Abs(cDBConvert.ToDecimal(CurrentCalibrationInjection["UPSCALE_MEASURED_VALUE"]) -
								cDBConvert.ToDecimal(CurrentCalibrationInjection["UPSCALE_REF_VALUE"]));
							Category.SetCheckParameter("7DAY_Upscale_Calc_APS", 0, eParameterDataType.Integer);
							if (CompTypeCd.InList("CO2,O2"))
							{
								diff = Math.Round(diff, 1, MidpointRounding.AwayFromZero);
								Category.SetCheckParameter("7DAY_Upscale_Calc_Result", diff, eParameterDataType.Decimal);
							}
							else
							{
								decimal UpscaleCalcResult;
								if (CompTypeCd.InList("SO2,NOX"))
								{
									UpscaleCalcResult = Math.Min(Math.Round(diff / SpanVal * 100, 1, MidpointRounding.AwayFromZero), (decimal)9999.9);
									Category.SetCheckParameter("7DAY_Upscale_Calc_Result", UpscaleCalcResult, eParameterDataType.Decimal);
									diff = Math.Round(diff, MidpointRounding.AwayFromZero);
									if (UpscaleCalcResult > (decimal)2.5 && SpanVal < 200 && diff <= 5)
									{
										Category.SetCheckParameter("7DAY_Upscale_Calc_APS", 1, eParameterDataType.Integer);
										Category.SetCheckParameter("7DAY_Upscale_Calc_Result", diff, eParameterDataType.Decimal);
									}
								}
								else
									if (CompTypeCd == "FLOW")
									{
										UpscaleCalcResult = Math.Min(Math.Round(diff / SpanVal * 100, 1, MidpointRounding.AwayFromZero), (decimal)9999.9);
										Category.SetCheckParameter("7DAY_Upscale_Calc_Result", UpscaleCalcResult, eParameterDataType.Decimal);
										diff = Math.Round(diff, 2, MidpointRounding.AwayFromZero);
										if (UpscaleCalcResult > 3 && AcqCd == "DP" && diff <= (decimal)0.01)
										{
											Category.SetCheckParameter("7DAY_Upscale_Calc_APS", 1, eParameterDataType.Integer);
											Category.SetCheckParameter("7DAY_Upscale_Calc_Result", 0m, eParameterDataType.Decimal);
										}
									}
									else
										if (CompTypeCd == "HG")
										{
											UpscaleCalcResult = Math.Min(Math.Round(diff / SpanVal * 100, 1, MidpointRounding.AwayFromZero), (decimal)9999.9);
											Category.SetCheckParameter("7DAY_Upscale_Calc_Result", UpscaleCalcResult, eParameterDataType.Decimal);
											diff = Math.Round(diff, 1, MidpointRounding.AwayFromZero);
											if (UpscaleCalcResult > (decimal)5 && SpanVal <= 10 && diff <= 1)
											{
												Category.SetCheckParameter("7DAY_Upscale_Calc_APS", 1, eParameterDataType.Integer);
												Category.SetCheckParameter("7DAY_Upscale_Calc_Result", diff, eParameterDataType.Decimal);
											}
										}
							}
						}
						SpanRecs.RowFilter = OldFilter;

					}
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "SEVNDAY34");
			}

			return ReturnVal;
		}

		#endregion

		#region Private Methods: Utilities

		private static decimal GetTolerance(string ATestTypeCd, String AFieldDescription, cCategory ACategory)
		{
			DataView ToleranceView = (DataView)ACategory.GetCheckParameter("Test_Tolerances_Cross_Check_Table").ParameterValue;
			DataRowView ToleranceRow;
			sFilterPair[] ToleranceFilter = new sFilterPair[2];

			ToleranceFilter[0].Set("TestTypeCode", ATestTypeCd);
			ToleranceFilter[1].Set("FieldDescription", AFieldDescription);

			if (FindRow(ToleranceView, ToleranceFilter, out ToleranceRow))
				return cDBConvert.ToDecimal(ToleranceRow["Tolerance"]);
			else
				return decimal.MinValue;
		}

		#endregion
	}
}
