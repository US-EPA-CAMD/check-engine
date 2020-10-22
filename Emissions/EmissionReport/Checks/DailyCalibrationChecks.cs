using System;
using System.Data;
using System.Linq;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Em.Parameters;
using ECMPS.Checks.EmissionsReport;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Extensions;


namespace ECMPS.Checks.EmissionsChecks
{
	public class cDailyCalibrationChecks : cEmissionsChecks
	{
		#region Constructors

		public cDailyCalibrationChecks(cEmissionsReportProcess emissionReportProcess)
			: base(emissionReportProcess)
		{
			CheckProcedures = new dCheckProcedure[31];

			CheckProcedures[1] = new dCheckProcedure(DAYCAL1);
			CheckProcedures[2] = new dCheckProcedure(DAYCAL2);
			CheckProcedures[3] = new dCheckProcedure(DAYCAL3);
			CheckProcedures[4] = new dCheckProcedure(DAYCAL4);
			CheckProcedures[5] = new dCheckProcedure(DAYCAL5);
			CheckProcedures[6] = new dCheckProcedure(DAYCAL6);
			CheckProcedures[7] = new dCheckProcedure(DAYCAL7);
			CheckProcedures[8] = new dCheckProcedure(DAYCAL8);
			CheckProcedures[9] = new dCheckProcedure(DAYCAL9);
			CheckProcedures[10] = new dCheckProcedure(DAYCAL10);

			CheckProcedures[11] = new dCheckProcedure(DAYCAL11);
			CheckProcedures[12] = new dCheckProcedure(DAYCAL12);
			CheckProcedures[13] = new dCheckProcedure(DAYCAL13);
			CheckProcedures[14] = new dCheckProcedure(DAYCAL14);
			CheckProcedures[15] = new dCheckProcedure(DAYCAL15);
			CheckProcedures[16] = new dCheckProcedure(DAYCAL16);
			CheckProcedures[17] = new dCheckProcedure(DAYCAL17);
			CheckProcedures[18] = new dCheckProcedure(DAYCAL18);
			CheckProcedures[19] = new dCheckProcedure(DAYCAL19);
			CheckProcedures[20] = new dCheckProcedure(DAYCAL20);

			CheckProcedures[21] = new dCheckProcedure(DAYCAL21);
			CheckProcedures[22] = new dCheckProcedure(DAYCAL22);
			CheckProcedures[23] = new dCheckProcedure(DAYCAL23);
			CheckProcedures[24] = new dCheckProcedure(DAYCAL24);
			CheckProcedures[25] = new dCheckProcedure(DAYCAL25);
			CheckProcedures[26] = new dCheckProcedure(DAYCAL26);
			CheckProcedures[27] = new dCheckProcedure(DAYCAL27);
			CheckProcedures[28] = new dCheckProcedure(DAYCAL28);
			CheckProcedures[29] = new dCheckProcedure(DAYCAL29);
			CheckProcedures[30] = new dCheckProcedure(DAYCAL30);
		}

		/// <summary>
		/// Constructor used for testing.
		/// </summary>
		/// <param name="mpManualParameters"></param>
		public cDailyCalibrationChecks(cEmissionsCheckParameters parameters)
		{
			EmManualParameters = parameters;
		}

		#endregion


		#region Public Static Methods: Checks

		#region Checks (1 - 10)

		public string DAYCAL1(cCategory Category, ref bool Log)
		//Daily Calibration Test Component Type Check
		{
			string ReturnVal = "";

			try
			{
				Category.SetCheckParameter("Daily_Cal_Calc_Result", null, eParameterDataType.String);
				Category.SetCheckParameter("Daily_Cal_Fail_Date", null, eParameterDataType.Date);
				Category.SetCheckParameter("Daily_Cal_Fail_Hour", null, eParameterDataType.Integer);

				DataRowView CurrentDayCalTest = (DataRowView)Category.GetCheckParameter("Current_Daily_Calibration_Test").ParameterValue;

				if (CurrentDayCalTest["COMPONENT_IDENTIFIER"] == DBNull.Value)
				{
					Category.SetCheckParameter("Daily_Cal_Component_Type_Valid", false, eParameterDataType.Boolean);
					Category.CheckCatalogResult = "A";
				}
				else
				{
					if (cDBConvert.ToString(CurrentDayCalTest["COMPONENT_TYPE_CD"]).InList("SO2,NOX,CO2,O2,FLOW"))
					{
						Category.SetCheckParameter("Daily_Cal_Component_Type_Valid", true, eParameterDataType.Boolean);
					}
					else if (EmParameters.CurrentDailyCalibrationTest.ComponentTypeCd.InList("HG,HCL"))
					{
						if (EmParameters.CurrentDailyCalibrationTest.OnlineOfflineInd == 1)
						{
							EmParameters.DailyCalComponentTypeValid = true;
						}
						else
						{
							EmParameters.DailyCalComponentTypeValid = false;
							Category.CheckCatalogResult = "C";
						}
					}
					else
					{
						Category.SetCheckParameter("Daily_Cal_Component_Type_Valid", false, eParameterDataType.Boolean);
						Category.CheckCatalogResult = "B";
					}
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "DAYCAL1");
			}

			return ReturnVal;
		}

		public string DAYCAL2(cCategory Category, ref bool Log)
		//Aborted and Incomplete Daily Calibration Test Check
		{
			string ReturnVal = "";

			try
			{
				Category.SetCheckParameter("Evaluate_Upscale_Injection", false, eParameterDataType.Boolean);
				Category.SetCheckParameter("Evaluate_Zero_Injection", false, eParameterDataType.Boolean);
				if (Category.GetCheckParameter("Daily_Cal_Component_Type_Valid").ValueAsBool())
				{
					DataRowView CurrentDayCalTest = (DataRowView)Category.GetCheckParameter("Current_Daily_Calibration_Test").ParameterValue;
					string TestResCd = cDBConvert.ToString(CurrentDayCalTest["TEST_RESULT_CD"]);
					if (TestResCd == "ABORTED")
					{
						Category.SetCheckParameter("Daily_Cal_Calc_Result", "ABORTED", eParameterDataType.String);
						Category.CheckCatalogResult = "A";
					}
					else
						if (TestResCd == "INC")
						{
							Category.SetCheckParameter("Daily_Cal_Calc_Result", "INC", eParameterDataType.String);
							if (CurrentDayCalTest["ZERO_INJECTION_DATE"] != DBNull.Value &&
								CurrentDayCalTest["ZERO_INJECTION_HOUR"] != DBNull.Value &&
								CurrentDayCalTest["ZERO_MEASURED_VALUE"] != DBNull.Value)
								Category.SetCheckParameter("Evaluate_Zero_Injection", true, eParameterDataType.Boolean);
							if (CurrentDayCalTest["UPSCALE_INJECTION_DATE"] != DBNull.Value &&
								CurrentDayCalTest["UPSCALE_INJECTION_HOUR"] != DBNull.Value &&
								CurrentDayCalTest["UPSCALE_MEASURED_VALUE"] != DBNull.Value)
								Category.SetCheckParameter("Evaluate_Upscale_Injection", true, eParameterDataType.Boolean);
						}
						else
						{
							Category.SetCheckParameter("Evaluate_Upscale_Injection", true, eParameterDataType.Boolean);
							Category.SetCheckParameter("Evaluate_Zero_Injection", true, eParameterDataType.Boolean);
						}
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "DAYCAL2");
			}

			return ReturnVal;
		}

		public string DAYCAL3(cCategory Category, ref bool log)
		//Online Offline Indicator Valid
		{
			string returnVal = "";

			try
			{
				DataRowView currentDailyCalibrationTest = Category.GetCheckParameter("Current_Daily_Calibration_Test").ValueAsDataRowView();

				int? onlineOfflineIndicator = currentDailyCalibrationTest["ONLINE_OFFLINE_IND"].AsInteger();

				int? dailyCalCalcOnlineInd = null;
				{
					decimal? upscaleOpTime = currentDailyCalibrationTest["UPSCALE_OP_TIME"].AsDecimal();
					decimal? zeroOpTime = currentDailyCalibrationTest["ZERO_OP_TIME"].AsDecimal();

					if ((zeroOpTime.HasValue && (zeroOpTime.Value == 0)) ||
						(upscaleOpTime.HasValue && (upscaleOpTime.Value == 0)))
						dailyCalCalcOnlineInd = 0;
					else if ((zeroOpTime.HasValue && (zeroOpTime.Value == 1)) &&
						(upscaleOpTime.HasValue && (upscaleOpTime.Value == 1)))
						dailyCalCalcOnlineInd = 1;
					else
						dailyCalCalcOnlineInd = onlineOfflineIndicator;
				}
				Category.SetCheckParameter("Daily_Cal_Calc_Online_Ind", dailyCalCalcOnlineInd, eParameterDataType.Integer);


				if (onlineOfflineIndicator == null)
				{
					Category.CheckCatalogResult = "A";
				}
				else if (EmParameters.CurrentDailyCalibrationTest.ComponentTypeCd.InList("HG,HCL"))
				{
					if (EmParameters.CurrentDailyCalibrationTest.OnlineOfflineInd.Default(0) == 1)
					{
						if (EmParameters.DailyCalCalcOnlineInd.Default(0) == 0)
						{
							Category.CheckCatalogResult = "E";
						}
					}
				}
				else if (dailyCalCalcOnlineInd == 0)
				{
					if (onlineOfflineIndicator == 1)
					{
						Category.CheckCatalogResult = "B";
					}
					else
					{
						string componentId = currentDailyCalibrationTest["COMPONENT_ID"].AsString();
						string spanScaleCd = currentDailyCalibrationTest["SPAN_SCALE_CD"].AsString();
						DateTime? dailyTestDatetime = currentDailyCalibrationTest["DAILY_TEST_DATETIME"].AsDateTime();

						DataView oocTestRecords = Category.GetCheckParameter("OOC_Test_Records_by_Location").ValueAsDataView();

						// oocTestRecords should be sorted by descending end date, hour and minute.
						DataView oocTestView
						  = cRowFilter.FindRows(oocTestRecords,
											   new cFilterCondition[] 
                                     { new cFilterCondition("COMPONENT_ID", componentId),
                                       new cFilterCondition("SPAN_SCALE_CD", spanScaleCd),
                                       new cFilterCondition("END_DATETIME", dailyTestDatetime.Default(DateTypes.START), 
                                                            eFilterDataType.DateEnded,  
                                                            eFilterConditionRelativeCompare.LessThan)});

						if (oocTestView.Count == 0)
						{
							Category.SetCheckParameter("Ignored_Daily_Calibration_Tests", true, eParameterDataType.Boolean);
							if (Category.GetCheckParameter("Daily_Cal_Calc_Result").ValueAsString() != "INVALID")
								Category.SetCheckParameter("Daily_Cal_Calc_Result", "IGNORED", eParameterDataType.String);
						}
						else
						{
							oocTestView.Sort = "END_DATETIME desc";

							DateTime? oocEndDatetime = oocTestView[0]["END_DATETIME"].AsDateTime();

							cFilterCondition[] qceRowFilter = new cFilterCondition[(spanScaleCd.InList("H,L") ? 5 : 4)];
							{
								qceRowFilter[0] = new cFilterCondition("COMPONENT_ID", componentId);
								qceRowFilter[1] = new cFilterCondition("OOC_REQUIRED", "Y");
								qceRowFilter[2] = new cFilterCondition("QA_CERT_EVENT_DATEHOUR", oocEndDatetime.Default(DateTypes.END),
																	   eFilterDataType.DateEnded,
																	   eFilterConditionRelativeCompare.GreaterThan);
								qceRowFilter[3] = new cFilterCondition("QA_CERT_EVENT_DATEHOUR", dailyTestDatetime.Default(DateTypes.START),
																	   eFilterDataType.DateEnded,
																	   eFilterConditionRelativeCompare.LessThanOrEqual);

								switch (spanScaleCd)
								{
									case "H":
										qceRowFilter[4] = new cFilterCondition("QA_CERT_EVENT_CD", "20,25,26,30,172", eFilterConditionStringCompare.InList, true);
										break;

									case "L":
										qceRowFilter[4] = new cFilterCondition("QA_CERT_EVENT_CD", "35,171", eFilterConditionStringCompare.InList, true);
										break;
								}
							}

							DataView qaCertificationEventRecords = Category.GetCheckParameter("Qa_Certification_Event_Records").ValueAsDataView();

							DataView qaCertificationEventView
							  = cRowFilter.FindRows(qaCertificationEventRecords, qceRowFilter);

							if (qaCertificationEventView.Count > 0)
								Category.CheckCatalogResult = "D";
						}
					}
				}
			}
			catch (Exception ex)
			{
				returnVal = Category.CheckEngine.FormatError(ex, "DAYCAL3");
			}

			return returnVal;
		}

		public string DAYCAL4(cCategory Category, ref bool Log)
		//Test Span Scale Valid
		{
			string ReturnVal = "";

			try
			{
				if (Category.GetCheckParameter("Daily_Cal_Component_Type_Valid").ValueAsBool())
				{
					Category.SetCheckParameter("Daily_Cal_Span_Scale_Valid", true, eParameterDataType.Boolean);
					DataRowView CurrentDayCalTest = (DataRowView)Category.GetCheckParameter("Current_Daily_Calibration_Test").ParameterValue;
					string SpanScaleCd = cDBConvert.ToString(CurrentDayCalTest["SPAN_SCALE_CD"]);

					if (EmParameters.CurrentDailyCalibrationTest.ComponentTypeCd.NotInList("FLOW,HG,HCL"))
					{
						if (SpanScaleCd == "")
						{
							Category.SetCheckParameter("Daily_Cal_Span_Scale_Valid", false, eParameterDataType.Boolean);
							Category.CheckCatalogResult = "A";
						}
						else
							if (!SpanScaleCd.InList("H,L"))
							{
								Category.SetCheckParameter("Daily_Cal_Span_Scale_Valid", false, eParameterDataType.Boolean);
								Category.CheckCatalogResult = "B";
							}
							else
								if (Category.GetCheckParameter("EM_Test_Date_Valid").ValueAsBool() &&
									Category.GetCheckParameter("EM_Test_Hour_Valid").ValueAsBool())
								{
                  DataView AnalyzerRangeRecs = (DataView)Category.GetCheckParameter("Analyzer_Range_Records_By_Component").ParameterValue;
									DataView RecordsFound;
									sFilterPair[] Filter = new sFilterPair[1];
									DateTime TestDate = cDBConvert.ToDate(CurrentDayCalTest["DAILY_TEST_DATE"], DateTypes.START);
									int TestHour = cDBConvert.ToHour(CurrentDayCalTest["DAILY_TEST_HOUR"], DateTypes.START);
									if (SpanScaleCd.InList("L,H"))
									{
										if (SpanScaleCd == "H")
											Filter[0].Set("ANALYZER_RANGE_CD", "L");
										else
											Filter[0].Set("ANALYZER_RANGE_CD", "H");

										RecordsFound = FindRows(AnalyzerRangeRecs, Filter);
										RecordsFound = FilterRanged(RecordsFound, TestDate, TestHour, true);

										if (RecordsFound.Count > 0)
										{
											Category.SetCheckParameter("Daily_Cal_Span_Scale_Valid", false, eParameterDataType.Boolean);
											Category.CheckCatalogResult = "C";
										}
									}
								}
					}
					else if (EmParameters.CurrentDailyCalibrationTest.ComponentTypeCd.InList("HG,HCL"))
					{
						if (EmParameters.CurrentDailyCalibrationTest.SpanScaleCd == null)
						{
							EmParameters.DailyCalSpanScaleValid = false;
							Category.CheckCatalogResult = "A";
						}
						else if (EmParameters.CurrentDailyCalibrationTest.SpanScaleCd != "H")
						{
							EmParameters.DailyCalSpanScaleValid = false;
							Category.CheckCatalogResult = "B";
						}
					}
					else
						if (SpanScaleCd != "")
						{
							Category.SetCheckParameter("Daily_Cal_Span_Scale_Valid", false, eParameterDataType.Boolean);
							Category.CheckCatalogResult = "D";
						}
				}
				else
					Log = false;
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "DAYCAL4");
			}

			return ReturnVal;
		}

		public string DAYCAL5(cCategory Category, ref bool Log)
		//Determine Span Value
		{
			string ReturnVal = "";

			try
			{
				Category.SetCheckParameter("Daily_Cal_Span_Value", null, eParameterDataType.Decimal);
				if (Category.GetCheckParameter("EM_Test_Date_Valid").ValueAsBool() &&
					Category.GetCheckParameter("EM_Test_Hour_Valid").ValueAsBool() &&
					Category.GetCheckParameter("Daily_Cal_Span_Scale_Valid").ValueAsBool())
				{
					DataView SysCompRecs = Category.GetCheckParameter("System_Component_Records").ValueAsDataView();

					if (SysCompRecs.Count > 0)
					{
						SysCompRecs.Sort = "BEGIN_DATE";
						DataRowView SysCompRecFound = SysCompRecs[0];
						DateTime SysCompBeginDate = cDBConvert.ToDate(SysCompRecFound["BEGIN_DATE"], DateTypes.START);
						int SysCompBeginHour = cDBConvert.ToInteger(SysCompRecFound["BEGIN_HOUR"]);
						DataRowView CurrentDayCalTest = (DataRowView)Category.GetCheckParameter("Current_Daily_Calibration_Test").ParameterValue;
						DateTime TestDate = cDBConvert.ToDate(CurrentDayCalTest["DAILY_TEST_DATE"], DateTypes.START);
						int TestHour = cDBConvert.ToInteger(CurrentDayCalTest["DAILY_TEST_HOUR"]);

						DataView SpanRecs = Category.GetCheckParameter("Span_Records").ValueAsDataView();
						sFilterPair[] SpanFilter = new sFilterPair[4];
						SpanFilter[0].Set("COMPONENT_TYPE_CD", cDBConvert.ToString(CurrentDayCalTest["COMPONENT_TYPE_CD"]));
						SpanFilter[1].Set("SPAN_SCALE_CD", cDBConvert.ToString(CurrentDayCalTest["SPAN_SCALE_CD"]));
						SpanFilter[2].Set("SPAN_VALUE", 0, eFilterDataType.Decimal, eFilterPairRelativeCompare.GreaterThan);
						SpanFilter[3].Set("MON_LOC_ID", cDBConvert.ToString(CurrentDayCalTest["MON_LOC_ID"]));
						SpanRecs = FindRows(SpanRecs, SpanFilter);
						if (SysCompBeginDate != DateTime.MinValue && 0 <= SysCompBeginHour && SysCompBeginHour <= 23 &&
							(SysCompBeginDate > TestDate || (SysCompBeginDate == TestDate && SysCompBeginHour > TestHour)))
							SpanRecs = FindActiveRows(SpanRecs, SysCompBeginDate, SysCompBeginHour, SysCompBeginDate, SysCompBeginHour, "BEGIN_DATE", "BEGIN_HOUR", "END_DATE", "END_HOUR");
						else
							SpanRecs = FindActiveRows(SpanRecs, TestDate, TestHour, TestDate, TestHour, "BEGIN_DATE", "BEGIN_HOUR", "END_DATE", "END_HOUR");
						if (SpanRecs.Count == 0)
							Category.CheckCatalogResult = "A";
						else
							if (SpanRecs.Count > 1)
								Category.CheckCatalogResult = "B";
							else
								Category.SetCheckParameter("Daily_Cal_Span_Value", cDBConvert.ToDecimal(SpanRecs[0]["SPAN_VALUE"]), eParameterDataType.Decimal);
					}
					else
						Category.CheckCatalogResult = "C";
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "DAYCAL5");
			}

			return ReturnVal;
		}

		public string DAYCAL6(cCategory Category, ref bool Log)
		//Daily Calibration Test Upscale Gas Level Code Valid
		{
			string ReturnVal = "";

			try
			{
				if (Category.GetCheckParameter("Evaluate_Upscale_Injection").ValueAsBool())
				{
					DataRowView CurrentDayCalTest = (DataRowView)Category.GetCheckParameter("Current_Daily_Calibration_Test").ParameterValue;
					string GasLvlCd = cDBConvert.ToString(CurrentDayCalTest["UPSCALE_GAS_LEVEL_CD"]);
					if (GasLvlCd == "")
					{
						Category.SetCheckParameter("Daily_Cal_Upscale_Gas_Level_Valid", false, eParameterDataType.Boolean);
						Category.CheckCatalogResult = "A";
					}
					else
						if (!GasLvlCd.InList("MID,HIGH"))
						{
							Category.SetCheckParameter("Daily_Cal_Upscale_Gas_Level_Valid", false, eParameterDataType.Boolean);
							Category.CheckCatalogResult = "B";
						}
						else
							if (cDBConvert.ToString(CurrentDayCalTest["COMPONENT_TYPE_CD"]) == "FLOW" && GasLvlCd == "MID")
							{
								Category.SetCheckParameter("Daily_Cal_Upscale_Gas_Level_Valid", false, eParameterDataType.Boolean);
								Category.CheckCatalogResult = "C";
							}
							else
								Category.SetCheckParameter("Daily_Cal_Upscale_Gas_Level_Valid", true, eParameterDataType.Boolean);
				}
				else
					Log = false;
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "DAYCAL6");
			}

			return ReturnVal;
		}

		public string DAYCAL7(cCategory Category, ref bool Log)
		//Reference Values Consistent with Calibration Gas Levels
		{
			string ReturnVal = "";

			try
			{
				if (Category.GetCheckParameter("Evaluate_Upscale_Injection").ValueAsBool() &&
					Category.GetCheckParameter("Evaluate_Zero_Injection").ValueAsBool())
				{
					DataRowView CurrentDayCalTest = (DataRowView)Category.GetCheckParameter("Current_Daily_Calibration_Test").ParameterValue;
					decimal ZeroRefVal = cDBConvert.ToDecimal(CurrentDayCalTest["ZERO_REF_VALUE"]);
					decimal UpsRefVal = cDBConvert.ToDecimal(CurrentDayCalTest["UPSCALE_REF_VALUE"]);
					if (ZeroRefVal >= UpsRefVal && ZeroRefVal >= 0 && UpsRefVal > 0)
					{
						Category.SetCheckParameter("Daily_Cal_Calc_Result", "INVALID", eParameterDataType.String);
						Category.CheckCatalogResult = "A";
					}
				}
				else
					Log = false;
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "DAYCAL7");
			}

			return ReturnVal;
		}

		public string DAYCAL8(cCategory Category, ref bool Log)
		//Zero Measured Value Valid
		{
			string ReturnVal = "";

			try
			{
				if (Category.GetCheckParameter("Evaluate_Zero_Injection").ValueAsBool())
				{
					DataRowView CurrentDayCalTest = (DataRowView)Category.GetCheckParameter("Current_Daily_Calibration_Test").ParameterValue;
					if (CurrentDayCalTest["ZERO_MEASURED_VALUE"] == DBNull.Value)
						Category.CheckCatalogResult = "A";
				}
				else
					Log = false;
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "DAYCAL8");
			}

			return ReturnVal;
		}

		public string DAYCAL9(cCategory Category, ref bool Log)
		//Zero Reference Value Valid
		{
			string ReturnVal = "";

			try
			{
				if (Category.GetCheckParameter("Evaluate_Zero_Injection").ValueAsBool())
				{
					DataRowView CurrentDayCalTest = (DataRowView)Category.GetCheckParameter("Current_Daily_Calibration_Test").ParameterValue;
					decimal ZeroRefVal = cDBConvert.ToDecimal(CurrentDayCalTest["ZERO_REF_VALUE"]);
					if (ZeroRefVal == decimal.MinValue)
						Category.CheckCatalogResult = "A";
					else
						if (ZeroRefVal < 0)
							Category.CheckCatalogResult = "B";
				}
				else
					Log = false;
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "DAYCAL9");
			}

			return ReturnVal;
		}

		public string DAYCAL10(cCategory Category, ref bool Log)
		//Zero Calibration Error Valid
		{
			string ReturnVal = "";

			try
			{
				if (Category.GetCheckParameter("Evaluate_Zero_Injection").ValueAsBool())
				{
					DataRowView CurrentDayCalTest = (DataRowView)Category.GetCheckParameter("Current_Daily_Calibration_Test").ParameterValue;
					decimal ZeroCalError = cDBConvert.ToDecimal(CurrentDayCalTest["ZERO_CAL_ERROR"]);
					if (ZeroCalError == decimal.MinValue)
						Category.CheckCatalogResult = "A";
					else
						if (ZeroCalError < 0)
							Category.CheckCatalogResult = "B";
				}
				else
					Log = false;
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "DAYCAL10");
			}

			return ReturnVal;
		}

		#endregion


		#region Checks (11- 20)

		public string DAYCAL11(cCategory Category, ref bool Log)
		//Zero APS Indicator Valid
		{
			string ReturnVal = "";

			try
			{
				if (Category.GetCheckParameter("Evaluate_Zero_Injection").ValueAsBool())
				{
					DataRowView CurrentDayCalTest = (DataRowView)Category.GetCheckParameter("Current_Daily_Calibration_Test").ParameterValue;
					if (CurrentDayCalTest["ZERO_APS_IND"] == DBNull.Value)
						Category.CheckCatalogResult = "A";
				}
				else
					Log = false;
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "DAYCAL11");
			}

			return ReturnVal;
		}

		public string DAYCAL12(cCategory Category, ref bool Log)
		//Upscale Measured Value Valid
		{
			string ReturnVal = "";

			try
			{
				if (Category.GetCheckParameter("Evaluate_Upscale_Injection").ValueAsBool())
				{
					DataRowView CurrentDayCalTest = (DataRowView)Category.GetCheckParameter("Current_Daily_Calibration_Test").ParameterValue;
					if (CurrentDayCalTest["UPSCALE_MEASURED_VALUE"] == DBNull.Value)
						Category.CheckCatalogResult = "A";
				}
				else
					Log = false;
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "DAYCAL12");
			}

			return ReturnVal;
		}

		public string DAYCAL13(cCategory Category, ref bool Log)
		//Upscale Reference Value Valid
		{
			string ReturnVal = "";

			try
			{
				if (Category.GetCheckParameter("Evaluate_Upscale_Injection").ValueAsBool())
				{
					DataRowView CurrentDayCalTest = (DataRowView)Category.GetCheckParameter("Current_Daily_Calibration_Test").ParameterValue;
					decimal UpsRefVal = cDBConvert.ToDecimal(CurrentDayCalTest["UPSCALE_REF_VALUE"]);
					if (UpsRefVal == decimal.MinValue)
						Category.CheckCatalogResult = "A";
					else
						if (UpsRefVal <= 0)
							Category.CheckCatalogResult = "B";
				}
				else
					Log = false;
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "DAYCAL13");
			}

			return ReturnVal;
		}

		public string DAYCAL14(cCategory Category, ref bool Log)
		//Upscale Calibration Error Valid
		{
			string ReturnVal = "";

			try
			{
				if (Category.GetCheckParameter("Evaluate_Upscale_Injection").ValueAsBool())
				{
					DataRowView CurrentDayCalTest = (DataRowView)Category.GetCheckParameter("Current_Daily_Calibration_Test").ParameterValue;
					decimal UpsCalError = cDBConvert.ToDecimal(CurrentDayCalTest["UPSCALE_CAL_ERROR"]);
					if (UpsCalError == decimal.MinValue)
						Category.CheckCatalogResult = "A";
					else
						if (UpsCalError < 0)
							Category.CheckCatalogResult = "B";
				}
				else
					Log = false;
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "DAYCAL14");
			}

			return ReturnVal;
		}

		public string DAYCAL15(cCategory Category, ref bool Log)
		//Upscale APS Indicator Valid
		{
			string ReturnVal = "";

			try
			{
				if (Category.GetCheckParameter("Evaluate_Upscale_Injection").ValueAsBool())
				{
					DataRowView CurrentDayCalTest = (DataRowView)Category.GetCheckParameter("Current_Daily_Calibration_Test").ParameterValue;
					if (CurrentDayCalTest["UPSCALE_APS_IND"] == DBNull.Value)
						Category.CheckCatalogResult = "A";
				}
				else
					Log = false;
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "DAYCAL15");
			}

			return ReturnVal;
		}

		public string DAYCAL16(cCategory Category, ref bool Log)
		//Upscale Injection Time Valid
		{
			string ReturnVal = "";

			try
			{
				if (Category.GetCheckParameter("Evaluate_Upscale_Injection").ValueAsBool())
				{
					DataRowView CurrentDayCalTest = (DataRowView)Category.GetCheckParameter("Current_Daily_Calibration_Test").ParameterValue;
					int UpsInjHr = cDBConvert.ToInteger(CurrentDayCalTest["UPSCALE_INJECTION_HOUR"]);
					int UpsInjMin = cDBConvert.ToInteger(CurrentDayCalTest["UPSCALE_INJECTION_MIN"]);
					if ((UpsInjHr != int.MinValue && (UpsInjHr < 0 || 23 < UpsInjHr)) || (UpsInjMin == int.MinValue && !Convert.ToBoolean(Category.GetCheckParameter("Legacy_Data_Evaluation").ParameterValue)) ||
						(UpsInjMin != int.MinValue && (UpsInjMin < 0 || 59 < UpsInjMin)))
					{
						Category.SetCheckParameter("Daily_Cal_Upscale_Injection_Time_Valid", false, eParameterDataType.Boolean);
						Category.CheckCatalogResult = "A";
					}
					else
						Category.SetCheckParameter("Daily_Cal_Upscale_Injection_Time_Valid", true, eParameterDataType.Boolean);
				}
				else
					Log = false;
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "DAYCAL16");
			}

			return ReturnVal;
		}

		public string DAYCAL17(cCategory Category, ref bool Log)
		//Zero Injection Time Valid
		{
			string ReturnVal = "";

			try
			{
				if (Category.GetCheckParameter("Evaluate_Zero_Injection").ValueAsBool())
				{
					DataRowView CurrentDayCalTest = (DataRowView)Category.GetCheckParameter("Current_Daily_Calibration_Test").ParameterValue;
					int ZeroInjHr = cDBConvert.ToInteger(CurrentDayCalTest["ZERO_INJECTION_HOUR"]);
					int ZeroInjMin = cDBConvert.ToInteger(CurrentDayCalTest["ZERO_INJECTION_MIN"]);

					if ((ZeroInjHr != int.MinValue && (ZeroInjHr < 0 || 23 < ZeroInjHr)) || (ZeroInjMin == int.MinValue && !Convert.ToBoolean(Category.GetCheckParameter("Legacy_Data_Evaluation").ParameterValue)) ||
						(ZeroInjMin != int.MinValue && (ZeroInjMin < 0 || 59 < ZeroInjMin)))
					{
						Category.SetCheckParameter("Daily_Cal_Injection_Times_Valid", false, eParameterDataType.Boolean);
						Category.CheckCatalogResult = "A";
					}
					else if (!Category.GetCheckParameter("Legacy_Data_Evaluation").ValueAsBool()
							 && CurrentDayCalTest["UPSCALE_INJECTION_DATE"].HasDbValue()
							 && CurrentDayCalTest["UPSCALE_INJECTION_HOUR"].AsInteger().InRange(0, 23)
							 && CurrentDayCalTest["UPSCALE_INJECTION_MIN"].AsInteger().InRange(0, 59)
							 && (CurrentDayCalTest["UPSCALE_INJECTION_DATE"].AsDateTime() == CurrentDayCalTest["ZERO_INJECTION_DATE"].AsDateTime())
							 && (CurrentDayCalTest["UPSCALE_INJECTION_HOUR"].AsInteger() == CurrentDayCalTest["ZERO_INJECTION_HOUR"].AsInteger())
							 && (CurrentDayCalTest["UPSCALE_INJECTION_MIN"].AsInteger() == CurrentDayCalTest["ZERO_INJECTION_MIN"].AsInteger())
							  && cDBConvert.ToString(CurrentDayCalTest["COMPONENT_TYPE_CD"]) != "FLOW")
					{
						Category.SetCheckParameter("Daily_Cal_Injection_Times_Valid", false, eParameterDataType.Boolean);
						Category.CheckCatalogResult = "B";
					}
					else
					{
						Category.SetCheckParameter("Daily_Cal_Injection_Times_Valid", Category.GetCheckParameter("Daily_Cal_Upscale_Injection_Time_Valid").ValueAsBool(), eParameterDataType.Boolean);

						if (CurrentDayCalTest["OVERLAP_EXISTS"].AsInteger() == 1)
							Category.CheckCatalogResult = "C";
						else
						{
							DateTime? zeroDate = CurrentDayCalTest["ZERO_INJECTION_DATE"].AsDateTime();
							int? zeroHour = CurrentDayCalTest["ZERO_INJECTION_HOUR"].AsInteger();
							DateTime? upscaleDate = CurrentDayCalTest["UPSCALE_INJECTION_DATE"].AsDateTime();
							int? upscaleHour = CurrentDayCalTest["UPSCALE_INJECTION_HOUR"].AsInteger();

							// If absolute difference between Zero and Upscale Dates and Hours is greater than zero the result D
							// Note: If any date our hour is null then assume greater than zero.
							if (!zeroDate.HasValue || !zeroHour.HasValue || !upscaleDate.HasValue || !upscaleHour.HasValue)
								Category.CheckCatalogResult = "D";
							else
							{
								DateTime zeroDateTime = new DateTime(zeroDate.Value.Year, zeroDate.Value.Month, zeroDate.Value.Day, zeroHour.Value, 0, 0);
								DateTime upscaleDateTime = new DateTime(upscaleDate.Value.Year, upscaleDate.Value.Month, upscaleDate.Value.Day, upscaleHour.Value, 0, 0);

								if (Math.Abs((zeroDateTime - upscaleDateTime).TotalHours) > 1)
									Category.CheckCatalogResult = "D";
							}
						}
					}
				}
				else
					Log = false;
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "DAYCAL17");
			}

			return ReturnVal;
		}

		public string DAYCAL18(cCategory Category, ref bool Log)
		//Zero Reference Value Consistent with Span
		{
			string ReturnVal = "";

			try
			{
				decimal SpanVal = cDBConvert.ToDecimal(Category.GetCheckParameter("Daily_Cal_Span_Value").ParameterValue);
				DataRowView CurrentDayCalTest = (DataRowView)Category.GetCheckParameter("Current_Daily_Calibration_Test").ParameterValue;
				decimal ZeroRefVal = cDBConvert.ToDecimal(CurrentDayCalTest["ZERO_REF_VALUE"]);
				if (SpanVal != decimal.MinValue && ZeroRefVal >= 0)
				{
					if (EmParameters.CurrentDailyCalibrationTest.ComponentTypeCd != "HG")
					{
						decimal ZeroRefPercOfSpan = Math.Round(ZeroRefVal / SpanVal * 100, 1, MidpointRounding.AwayFromZero);
						Category.SetCheckParameter("Zero_Reference_Percent_of_Span", ZeroRefPercOfSpan, eParameterDataType.Decimal);
						if (ZeroRefPercOfSpan > 20)
						{
							DataView ToleranceView = (DataView)Category.GetCheckParameter("Test_Tolerances_Cross_Check_Table").ParameterValue;
							DataRowView ToleranceRow;
							sFilterPair[] ToleranceFilter = new sFilterPair[2];
							ToleranceFilter[0].Set("TestTypeCode", "7DAY");
							ToleranceFilter[1].Set("FieldDescription", "GasPercentOfSpan");
							decimal Tolerance = 0;
							if (FindRow(ToleranceView, ToleranceFilter, out ToleranceRow))
								Tolerance = cDBConvert.ToDecimal(ToleranceRow["Tolerance"]);
							if (ZeroRefPercOfSpan > 20m + Tolerance)
								Category.CheckCatalogResult = "A";
							else
								Category.CheckCatalogResult = "B";
						}
					}
				}
				else
					Log = false;
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "DAYCAL18");
			}

			return ReturnVal;
		}

		public string DAYCAL19(cCategory Category, ref bool Log)
		//Upscale Reference Value Consistent with Span
		{
			string ReturnVal = "";

			try
			{
				decimal SpanVal = cDBConvert.ToDecimal(Category.GetCheckParameter("Daily_Cal_Span_Value").ParameterValue);
				DataRowView CurrentDayCalTest = (DataRowView)Category.GetCheckParameter("Current_Daily_Calibration_Test").ParameterValue;
				decimal UpsRefVal = cDBConvert.ToDecimal(CurrentDayCalTest["UPSCALE_REF_VALUE"]);
				if (SpanVal != decimal.MinValue && UpsRefVal > 0)
				{
					decimal UpsRefPercOfSpan = Math.Round(UpsRefVal / SpanVal * 100, 1, MidpointRounding.AwayFromZero);
					Category.SetCheckParameter("Upscale_Reference_Percent_of_Span", UpsRefPercOfSpan, eParameterDataType.Decimal);

					DataView ToleranceView = (DataView)Category.GetCheckParameter("Test_Tolerances_Cross_Check_Table").ParameterValue;
					DataRowView ToleranceRow;
					sFilterPair[] ToleranceFilter = new sFilterPair[2];
					ToleranceFilter[0].Set("TestTypeCode", "7DAY");
					ToleranceFilter[1].Set("FieldDescription", "GasPercentOfSpan");
					decimal Tolerance = 0;
					if (FindRow(ToleranceView, ToleranceFilter, out ToleranceRow))
						Tolerance = cDBConvert.ToDecimal(ToleranceRow["Tolerance"]);

					string UpsGasLvlCd = cDBConvert.ToString(CurrentDayCalTest["UPSCALE_GAS_LEVEL_CD"]);
					string CompTypeCd = cDBConvert.ToString(CurrentDayCalTest["COMPONENT_TYPE_CD"]);
					switch (UpsGasLvlCd)
					{
						case "MID":
							if (CompTypeCd != "FLOW")
								if (UpsRefPercOfSpan < 50 || 60 < UpsRefPercOfSpan)
									if (UpsRefPercOfSpan < (50m - Tolerance) || (60m + Tolerance) < UpsRefPercOfSpan)
										Category.CheckCatalogResult = "A";
									else
										Category.CheckCatalogResult = "B";
							break;
						case "HIGH":
							if (CompTypeCd == "FLOW")
							{
								if (UpsRefPercOfSpan < 50 || 70 < UpsRefPercOfSpan)
									if (UpsRefPercOfSpan < (50m - Tolerance) || (70m + Tolerance) < UpsRefPercOfSpan)
										Category.CheckCatalogResult = "C";
									else
										Category.CheckCatalogResult = "D";
							}
							else
								if (100 < UpsRefPercOfSpan)
									Category.CheckCatalogResult = "E";
								else
									if (UpsRefPercOfSpan < 80)
										if (UpsRefPercOfSpan < (80m - Tolerance))
											Category.CheckCatalogResult = "E";
										else
											Category.CheckCatalogResult = "F";
							break;
						default:
							break;
					}
				}
				else
					Log = false;
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "DAYCAL19");
			}

			return ReturnVal;
		}

		public string DAYCAL20(cCategory Category, ref bool Log)
		//Calculate Zero Gas Injection or Reference Signal Results
		{
			string ReturnVal = "";

			try
			{
				if (Category.GetCheckParameter("Evaluate_Zero_Injection").ValueAsBool())
				{
					DataRowView CurrentDayCalTest = (DataRowView)Category.GetCheckParameter("Current_Daily_Calibration_Test").ParameterValue;
					decimal ZeroRefVal = cDBConvert.ToDecimal(CurrentDayCalTest["ZERO_REF_VALUE"]);
					decimal CalSpanVal = cDBConvert.ToDecimal(Category.GetCheckParameter("Daily_Cal_Span_Value").ParameterValue);
					decimal ZeroMeasVal = cDBConvert.ToDecimal(CurrentDayCalTest["ZERO_MEASURED_VALUE"]);
					decimal ZeroCalError = cDBConvert.ToDecimal(CurrentDayCalTest["ZERO_CAL_ERROR"]);
					int ZeroAPSInd = cDBConvert.ToInteger(CurrentDayCalTest["ZERO_APS_IND"]);
					DateTime ZeroInjDate = cDBConvert.ToDate(CurrentDayCalTest["ZERO_INJECTION_DATE"], DateTypes.START);
					int ZeroInjHour = cDBConvert.ToInteger(CurrentDayCalTest["ZERO_INJECTION_HOUR"]);

					if (CalSpanVal == decimal.MinValue || ZeroRefVal < 0 || ZeroMeasVal == decimal.MinValue)
					{
						Category.SetCheckParameter("Daily_Cal_Calc_Result", "INVALID", eParameterDataType.String);
						Category.SetCheckParameter("Daily_Cal_Zero_Injection_Calc_Result", null, eParameterDataType.Decimal);
						Category.SetCheckParameter("Daily_Cal_Zero_Injection_Calc_APS_Indicator", null, eParameterDataType.Integer);
						Category.CheckCatalogResult = "A";
					}
					else
					{
						decimal Diff = Math.Abs(ZeroMeasVal - ZeroRefVal);
						Category.SetCheckParameter("Daily_Cal_Zero_Injection_Calc_APS_Indicator", 0, eParameterDataType.Integer);
						bool InjTimesValid = Category.GetCheckParameter("Daily_Cal_Injection_Times_Valid").ValueAsBool();
						DateTime DayCalFailDate = Category.GetCheckParameter("Daily_Cal_Fail_Date").ValueAsDateTime(DateTypes.START);
						int DayCalFailHour = Category.GetCheckParameter("Daily_Cal_Fail_Hour").ValueAsInt();
						string CompTypeCd = cDBConvert.ToString(CurrentDayCalTest["COMPONENT_TYPE_CD"]);
						decimal ZeroInjResult;
						switch (CompTypeCd)
						{
							case "CO2":
							case "O2":
								{
									Diff = Math.Round(Diff, 1, MidpointRounding.AwayFromZero);
									ZeroInjResult = Diff;//for readability in comparison with the other cases
									Category.SetCheckParameter("Daily_Cal_Zero_Injection_Calc_Result", ZeroInjResult, eParameterDataType.Decimal);
									string DailyCalCalcRes = Convert.ToString(Category.GetCheckParameter("Daily_Cal_Calc_Result").ParameterValue);
									if (!DailyCalCalcRes.InList("INVALID,IGNORED"))
										if (ZeroInjResult > 1)
											if (0 <= ZeroCalError && ZeroCalError <= 1)
											{
												decimal Tol = GetTolerance("7DAY", "DifferencePCT", Category);
												if (Math.Abs(Diff - ZeroCalError) <= Tol)
												{
													if (!DailyCalCalcRes.InList("INC,FAILED"))
														Category.SetCheckParameter("Daily_Cal_Calc_Result", "PASSED", eParameterDataType.String);
												}
												else
												{
													Category.SetCheckParameter("Daily_Cal_Calc_Result", "FAILED", eParameterDataType.String);
													if (InjTimesValid)
														if (DayCalFailDate == DateTime.MinValue || DayCalFailDate > ZeroInjDate || (DayCalFailDate == ZeroInjDate && DayCalFailHour > ZeroInjHour))
														{
															Category.SetCheckParameter("Daily_Cal_Fail_Date", ZeroInjDate, eParameterDataType.Date);
															Category.SetCheckParameter("Daily_Cal_Fail_Hour", ZeroInjHour, eParameterDataType.Integer);
														}
												}
											}
											else
											{
												Category.SetCheckParameter("Daily_Cal_Calc_Result", "FAILED", eParameterDataType.String);
												if (InjTimesValid)
													if (DayCalFailDate == DateTime.MinValue || DayCalFailDate > ZeroInjDate || (DayCalFailDate == ZeroInjDate && DayCalFailHour > ZeroInjHour))
													{
														Category.SetCheckParameter("Daily_Cal_Fail_Date", ZeroInjDate, eParameterDataType.Date);
														Category.SetCheckParameter("Daily_Cal_Fail_Hour", ZeroInjHour, eParameterDataType.Integer);
													}
											}
										else
											if (!DailyCalCalcRes.InList("INC,FAILED"))
												Category.SetCheckParameter("Daily_Cal_Calc_Result", "PASSED", eParameterDataType.String);
									break;
								}
							case "SO2":
							case "NOX":
								{
									ZeroInjResult = Math.Min(Math.Round(Diff / CalSpanVal * 100, 1, MidpointRounding.AwayFromZero), (decimal)9999.9);
									Category.SetCheckParameter("Daily_Cal_Zero_Injection_Calc_Result", ZeroInjResult, eParameterDataType.Decimal);
									Diff = Math.Round(Diff, 1, MidpointRounding.AwayFromZero);
									string DailyCalCalcRes = Convert.ToString(Category.GetCheckParameter("Daily_Cal_Calc_Result").ParameterValue);
									if (ZeroInjResult > 5 && ((CalSpanVal <= 50 && Diff <= 5) || (CalSpanVal > 50 && CalSpanVal <= 200 && Diff <= 10)))
									{
										Category.SetCheckParameter("Daily_Cal_Zero_Injection_Calc_Result", Diff, eParameterDataType.Decimal);
										Category.SetCheckParameter("Daily_Cal_Zero_Injection_Calc_APS_Indicator", 1, eParameterDataType.Integer);
										if (!DailyCalCalcRes.InList("INC,FAILED,INVALID,IGNORED"))
											Category.SetCheckParameter("Daily_Cal_Calc_Result", "PASSAPS", eParameterDataType.String);
									}
									else
									{
										if (ZeroInjResult > 5)
										{
											if (!DailyCalCalcRes.InList("INVALID,IGNORED"))
												if (ZeroAPSInd != 1 && 0 <= ZeroCalError && ZeroCalError <= 5)
												{
													decimal Tol = GetTolerance("7DAY", "CalibrationError", Category);
													if (Math.Abs(ZeroInjResult - ZeroCalError) <= Tol)
													{
														if (!DailyCalCalcRes.InList("PASSAPS,INC,FAILED"))
															Category.SetCheckParameter("Daily_Cal_Calc_Result", "PASSED", eParameterDataType.String);
													}
													else
													{
														Category.SetCheckParameter("Daily_Cal_Calc_Result", "FAILED", eParameterDataType.String);
														if (InjTimesValid)
															if (DayCalFailDate == DateTime.MinValue || DayCalFailDate > ZeroInjDate || (DayCalFailDate == ZeroInjDate && DayCalFailHour > ZeroInjHour))
															{
																Category.SetCheckParameter("Daily_Cal_Fail_Date", ZeroInjDate, eParameterDataType.Date);
																Category.SetCheckParameter("Daily_Cal_Fail_Hour", ZeroInjHour, eParameterDataType.Integer);
															}
													}
												}
												else
												{
													if (ZeroAPSInd == 1 && 0 <= ZeroCalError && ((CalSpanVal <= 50 && ZeroCalError <= 5) || (CalSpanVal > 50 && CalSpanVal <= 200 && ZeroCalError <= 10)))
													{
														decimal Tol = GetTolerance("7DAY", "DifferencePPM", Category);
														if (Math.Abs(Diff - ZeroCalError) <= Tol)
														{
															if (!DailyCalCalcRes.InList("INC,FAILED"))
																Category.SetCheckParameter("Daily_Cal_Calc_Result", "PASSED", eParameterDataType.String);
														}
														else
														{
															Category.SetCheckParameter("Daily_Cal_Calc_Result", "FAILED", eParameterDataType.String);
															if (InjTimesValid)
																if (DayCalFailDate == DateTime.MinValue || DayCalFailDate > ZeroInjDate || (DayCalFailDate == ZeroInjDate && DayCalFailHour > ZeroInjHour))
																{
																	Category.SetCheckParameter("Daily_Cal_Fail_Date", ZeroInjDate, eParameterDataType.Date);
																	Category.SetCheckParameter("Daily_Cal_Fail_Hour", ZeroInjHour, eParameterDataType.Integer);
																}
														}
													}
													else
													{
														Category.SetCheckParameter("Daily_Cal_Calc_Result", "FAILED", eParameterDataType.String);
														if (InjTimesValid)
															if (DayCalFailDate == DateTime.MinValue || DayCalFailDate > ZeroInjDate || (DayCalFailDate == ZeroInjDate && DayCalFailHour > ZeroInjHour))
															{
																Category.SetCheckParameter("Daily_Cal_Fail_Date", ZeroInjDate, eParameterDataType.Date);
																Category.SetCheckParameter("Daily_Cal_Fail_Hour", ZeroInjHour, eParameterDataType.Integer);
															}
													}
												}
										}
										else
											if (!DailyCalCalcRes.InList("INC,FAILED,PASSAPS,IGNORED"))
												Category.SetCheckParameter("Daily_Cal_Calc_Result", "PASSED", eParameterDataType.String);
									}
									break;
								}
							case "FLOW":
								{
									ZeroInjResult = Math.Min(Math.Round(Diff / CalSpanVal * 100, 1, MidpointRounding.AwayFromZero), (decimal)9999.9);
									Category.SetCheckParameter("Daily_Cal_Zero_Injection_Calc_Result", ZeroInjResult, eParameterDataType.Decimal);
									Diff = Math.Round(Diff, 2, MidpointRounding.AwayFromZero);
									string DailyCalCalcRes = Convert.ToString(Category.GetCheckParameter("Daily_Cal_Calc_Result").ParameterValue);
									string AcqCd = cDBConvert.ToString(CurrentDayCalTest["ACQ_CD"]);
									if (ZeroInjResult > 6 && AcqCd == "DP" && Diff <= (decimal)0.02)
									{
										Category.SetCheckParameter("Daily_Cal_Zero_Injection_Calc_Result", Diff, eParameterDataType.Decimal);
										Category.SetCheckParameter("Daily_Cal_Zero_Injection_Calc_APS_Indicator", 1, eParameterDataType.Integer);
										if (!DailyCalCalcRes.InList("INVALID,FAILED,INC,IGNORED"))
											Category.SetCheckParameter("Daily_Cal_Calc_Result", "PASSAPS", eParameterDataType.String);
									}
									else
									{
										if (ZeroInjResult > 6)
										{
											if (!DailyCalCalcRes.InList("INVALID,IGNORED"))
												if (ZeroAPSInd != 1 && 0 <= ZeroCalError && ZeroCalError <= 6)
												{
													decimal Tol = GetTolerance("7DAY", "CalibrationError", Category);
													if (Math.Abs(ZeroInjResult - ZeroCalError) <= Tol)
													{
														if (!DailyCalCalcRes.InList("PASSAPS,INC,FAILED"))
															Category.SetCheckParameter("Daily_Cal_Calc_Result", "PASSED", eParameterDataType.String);
													}
													else
													{
														Category.SetCheckParameter("Daily_Cal_Calc_Result", "FAILED", eParameterDataType.String);
														if (InjTimesValid)
															if (DayCalFailDate == DateTime.MinValue || DayCalFailDate > ZeroInjDate || (DayCalFailDate == ZeroInjDate && DayCalFailHour > ZeroInjHour))
															{
																Category.SetCheckParameter("Daily_Cal_Fail_Date", ZeroInjDate, eParameterDataType.Date);
																Category.SetCheckParameter("Daily_Cal_Fail_Hour", ZeroInjHour, eParameterDataType.Integer);
															}
													}
												}
												else
												{
													if (ZeroAPSInd == 1 && AcqCd == "DP" && 0 <= ZeroCalError && ZeroCalError <= (decimal)0.02)
													{
														decimal Tol = GetTolerance("7DAY", "DifferenceINH2O", Category);
														if (Math.Abs(Diff - ZeroCalError) <= Tol)
														{
															if (!DailyCalCalcRes.InList("INC,FAILED"))
																Category.SetCheckParameter("Daily_Cal_Calc_Result", "PASSED", eParameterDataType.String);
														}
														else
														{
															Category.SetCheckParameter("Daily_Cal_Calc_Result", "FAILED", eParameterDataType.String);
															if (InjTimesValid)
																if (DayCalFailDate == DateTime.MinValue || DayCalFailDate > ZeroInjDate || (DayCalFailDate == ZeroInjDate && DayCalFailHour > ZeroInjHour))
																{
																	Category.SetCheckParameter("Daily_Cal_Fail_Date", ZeroInjDate, eParameterDataType.Date);
																	Category.SetCheckParameter("Daily_Cal_Fail_Hour", ZeroInjHour, eParameterDataType.Integer);
																}
														}
													}
													else
													{
														Category.SetCheckParameter("Daily_Cal_Calc_Result", "FAILED", eParameterDataType.String);
														if (InjTimesValid)
															if (DayCalFailDate == DateTime.MinValue || DayCalFailDate > ZeroInjDate || (DayCalFailDate == ZeroInjDate && DayCalFailHour > ZeroInjHour))
															{
																Category.SetCheckParameter("Daily_Cal_Fail_Date", ZeroInjDate, eParameterDataType.Date);
																Category.SetCheckParameter("Daily_Cal_Fail_Hour", ZeroInjHour, eParameterDataType.Integer);
															}
													}
												}
										}
										else
											if (!DailyCalCalcRes.InList("INC,FAILED,PASSAPS,IGNORED"))
												Category.SetCheckParameter("Daily_Cal_Calc_Result", "PASSED", eParameterDataType.String);
									}
									break;
								}

							case "HG":
								{
									EmParameters.DailyCalZeroInjectionCalcResult = Math.Min(Math.Round(Diff / EmParameters.DailyCalSpanValue.Value * 100, 1, MidpointRounding.AwayFromZero), 9999.9m);
									Diff = Math.Round(Diff, 1, MidpointRounding.AwayFromZero);
									if ((EmParameters.DailyCalZeroInjectionCalcResult > 5.0m) && (Diff <= 1.0m))
									{
										EmParameters.DailyCalZeroInjectionCalcResult = Diff;
										EmParameters.DailyCalZeroInjectionCalcApsIndicator = 1;
										if (EmParameters.DailyCalCalcResult.NotInList("INC,FAILED,INVALID,IGNORED"))
											EmParameters.DailyCalCalcResult = "PASSAPS";
									}
									else
									{
										if (EmParameters.DailyCalZeroInjectionCalcResult > 5)
										{
											if (EmParameters.DailyCalCalcResult.NotInList("INVALID,IGNORED"))
												if (ZeroAPSInd != 1 && 0 <= ZeroCalError && ZeroCalError <= 5)
												{
													decimal Tol = GetTolerance("7DAY", "CalibrationError", Category);
													if (Math.Abs(EmParameters.DailyCalZeroInjectionCalcResult.GetValueOrDefault() - ZeroCalError) <= Tol)
													{
														if (EmParameters.DailyCalCalcResult.NotInList("PASSAPS,INC,FAILED"))
															EmParameters.DailyCalCalcResult = "PASSED";
													}
													else
													{
														EmParameters.DailyCalCalcResult = "FAILED";
														if (EmParameters.DailyCalInjectionTimesValid == true)
															if (EmParameters.DailyCalFailDate == DateTime.MinValue
																|| EmParameters.DailyCalFailDate > ZeroInjDate
																|| (EmParameters.DailyCalFailDate == ZeroInjDate && EmParameters.DailyCalFailHour > ZeroInjHour))
															{
																EmParameters.DailyCalFailDate = ZeroInjDate;
																EmParameters.DailyCalFailHour = ZeroInjHour;
															}
													}
												}
												else
												{
													if (ZeroAPSInd == 1 && 0 <= ZeroCalError && ZeroCalError <= 1)
													{
														decimal Tol = GetTolerance("7DAY", "DifferenceUGSCM", Category);
														if (Math.Abs(Diff - ZeroCalError) <= Tol)
														{
															if (EmParameters.DailyCalCalcResult.NotInList("INC,FAILED"))
																EmParameters.DailyCalCalcResult = "PASSAPS";
														}
														else
														{
															EmParameters.DailyCalCalcResult = "FAILED";
															if (EmParameters.DailyCalInjectionTimesValid == true)
																if (EmParameters.DailyCalFailDate == DateTime.MinValue
																	|| EmParameters.DailyCalFailDate > ZeroInjDate
																	|| (EmParameters.DailyCalFailDate == ZeroInjDate && EmParameters.DailyCalFailHour > ZeroInjHour))
																{
																	EmParameters.DailyCalFailDate = ZeroInjDate;
																	EmParameters.DailyCalFailHour = ZeroInjHour;
																}
														}
													}
													else
													{
														EmParameters.DailyCalCalcResult = "FAILED";
														if (EmParameters.DailyCalInjectionTimesValid == true)
															if (EmParameters.DailyCalFailDate == DateTime.MinValue
																|| EmParameters.DailyCalFailDate > ZeroInjDate
																|| (EmParameters.DailyCalFailDate == ZeroInjDate && EmParameters.DailyCalFailHour > ZeroInjHour))
															{
																EmParameters.DailyCalFailDate = ZeroInjDate;
																EmParameters.DailyCalFailHour = ZeroInjHour;
															}
													}
												}
										}
										else
											if (EmParameters.DailyCalCalcResult.NotInList("INC,FAILED,PASSAPS,IGNORED"))
												EmParameters.DailyCalCalcResult = "PASSED";
									}
									break;

								}

							default:
								break;
						}
					}
				}
				else
					Log = false;
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "DAYCAL20");
			}

			return ReturnVal;
		}

		#endregion


		#region (21 - 30)

		public string DAYCAL21(cCategory Category, ref bool Log)
		//Calculate Upscale Gas Injection or Reference Signal Results
		{
			string ReturnVal = "";

			try
			{
				if (Category.GetCheckParameter("Evaluate_Upscale_Injection").ValueAsBool())
				{
					DataRowView CurrentDayCalTest = (DataRowView)Category.GetCheckParameter("Current_Daily_Calibration_Test").ParameterValue;
					decimal UpsRefVal = cDBConvert.ToDecimal(CurrentDayCalTest["UPSCALE_REF_VALUE"]);
					decimal CalSpanVal = cDBConvert.ToDecimal(Category.GetCheckParameter("Daily_Cal_Span_Value").ParameterValue);
					decimal UpsMeasVal = cDBConvert.ToDecimal(CurrentDayCalTest["UPSCALE_MEASURED_VALUE"]);
					decimal UpsCalError = cDBConvert.ToDecimal(CurrentDayCalTest["UPSCALE_CAL_ERROR"]);
					int UpsAPSInd = cDBConvert.ToInteger(CurrentDayCalTest["UPSCALE_APS_IND"]);
					DateTime UpsInjDate = cDBConvert.ToDate(CurrentDayCalTest["UPSCALE_INJECTION_DATE"], DateTypes.START);
					int UpsInjHour = cDBConvert.ToInteger(CurrentDayCalTest["UPSCALE_INJECTION_HOUR"]);

					if (CalSpanVal == decimal.MinValue || !Category.GetCheckParameter("Daily_Cal_Upscale_Gas_Level_Valid").ValueAsBool() ||
						UpsRefVal < 0 || UpsMeasVal == decimal.MinValue)
					{
						Category.SetCheckParameter("Daily_Cal_Calc_Result", "INVALID", eParameterDataType.String);
						Category.SetCheckParameter("Daily_Cal_Upscale_Injection_Calc_Result", null, eParameterDataType.Decimal);
						Category.SetCheckParameter("Daily_Cal_Upscale_Injection_Calc_APS_Indicator", null, eParameterDataType.Integer);
						Category.CheckCatalogResult = "A";
					}
					else
					{
						decimal Diff = Math.Abs(UpsMeasVal - UpsRefVal);
						Category.SetCheckParameter("Daily_Cal_Upscale_Injection_Calc_APS_Indicator", 0, eParameterDataType.Integer);
						string CompTypeCd = cDBConvert.ToString(CurrentDayCalTest["COMPONENT_TYPE_CD"]);
						bool InjTimesValid = Category.GetCheckParameter("Daily_Cal_Injection_Times_Valid").ValueAsBool();
						DateTime DayCalFailDate = Category.GetCheckParameter("Daily_Cal_Fail_Date").ValueAsDateTime(DateTypes.START);
						int DayCalFailHour = Category.GetCheckParameter("Daily_Cal_Fail_Hour").ValueAsInt();
						decimal UpsInjResult;
						switch (CompTypeCd)
						{
							case "CO2":
							case "O2":
								{
									Diff = Math.Round(Diff, 1, MidpointRounding.AwayFromZero);
									UpsInjResult = Diff;//for readability in comparison with the other cases
									Category.SetCheckParameter("Daily_Cal_Upscale_Injection_Calc_Result", UpsInjResult, eParameterDataType.Decimal);
									string DailyCalCalcRes = Convert.ToString(Category.GetCheckParameter("Daily_Cal_Calc_Result").ParameterValue);
									if (!DailyCalCalcRes.InList("INVALID,IGNORED"))
										if (UpsInjResult > 1)
											if (0 <= UpsCalError && UpsCalError <= 1)
											{
												decimal Tol = GetTolerance("7DAY", "DifferencePCT", Category);
												if (Math.Abs(Diff - UpsCalError) <= Tol)
												{
													if (!DailyCalCalcRes.InList("INC,FAILED"))
														Category.SetCheckParameter("Daily_Cal_Calc_Result", "PASSED", eParameterDataType.String);
												}
												else
												{
													Category.SetCheckParameter("Daily_Cal_Calc_Result", "FAILED", eParameterDataType.String);
													if (InjTimesValid)
														if (DayCalFailDate == DateTime.MinValue || DayCalFailDate > UpsInjDate || (DayCalFailDate == UpsInjDate && DayCalFailHour > UpsInjHour))
														{
															Category.SetCheckParameter("Daily_Cal_Fail_Date", UpsInjDate, eParameterDataType.Date);
															Category.SetCheckParameter("Daily_Cal_Fail_Hour", UpsInjHour, eParameterDataType.Integer);
														}
												}
											}
											else
											{
												Category.SetCheckParameter("Daily_Cal_Calc_Result", "FAILED", eParameterDataType.String);
												if (InjTimesValid)
													if (DayCalFailDate == DateTime.MinValue || DayCalFailDate > UpsInjDate || (DayCalFailDate == UpsInjDate && DayCalFailHour > UpsInjHour))
													{
														Category.SetCheckParameter("Daily_Cal_Fail_Date", UpsInjDate, eParameterDataType.Date);
														Category.SetCheckParameter("Daily_Cal_Fail_Hour", UpsInjHour, eParameterDataType.Integer);
													}
											}
										else
											if (!DailyCalCalcRes.InList("INC,FAILED,IGNORED"))
												Category.SetCheckParameter("Daily_Cal_Calc_Result", "PASSED", eParameterDataType.String);
									break;
								}
							case "SO2":
							case "NOX":
								{
									UpsInjResult = Math.Min(Math.Round(Diff / CalSpanVal * 100, 1, MidpointRounding.AwayFromZero), (decimal)9999.9);
									Category.SetCheckParameter("Daily_Cal_Upscale_Injection_Calc_Result", UpsInjResult, eParameterDataType.Decimal);
									Diff = Math.Round(Diff, 1, MidpointRounding.AwayFromZero);
									string DailyCalCalcRes = Convert.ToString(Category.GetCheckParameter("Daily_Cal_Calc_Result").ParameterValue);
									if (UpsInjResult > 5 && ((CalSpanVal <= 50 && Diff <= 5) || (CalSpanVal > 50 && CalSpanVal <= 200 && Diff <= 10)))
									{
										Category.SetCheckParameter("Daily_Cal_Upscale_Injection_Calc_Result", Diff, eParameterDataType.Decimal);
										Category.SetCheckParameter("Daily_Cal_Upscale_Injection_Calc_APS_Indicator", 1, eParameterDataType.Integer);
										if (!DailyCalCalcRes.InList("INC,FAILED,INVALID,IGNORED"))
											Category.SetCheckParameter("Daily_Cal_Calc_Result", "PASSAPS", eParameterDataType.String);
									}
									else
									{
										if (UpsInjResult > 5)
										{
											if (!DailyCalCalcRes.InList("INVALID,IGNORED"))
												if (UpsAPSInd != 1 && 0 <= UpsCalError && UpsCalError <= 5)
												{
													decimal Tol = GetTolerance("7DAY", "CalibrationError", Category);
													if (Math.Abs(UpsInjResult - UpsCalError) <= Tol)
													{
														if (!DailyCalCalcRes.InList("PASSAPS,INC,FAILED"))
															Category.SetCheckParameter("Daily_Cal_Calc_Result", "PASSED", eParameterDataType.String);
													}
													else
													{
														Category.SetCheckParameter("Daily_Cal_Calc_Result", "FAILED", eParameterDataType.String);
														if (InjTimesValid)
															if (DayCalFailDate == DateTime.MinValue || DayCalFailDate > UpsInjDate || (DayCalFailDate == UpsInjDate && DayCalFailHour > UpsInjHour))
															{
																Category.SetCheckParameter("Daily_Cal_Fail_Date", UpsInjDate, eParameterDataType.Date);
																Category.SetCheckParameter("Daily_Cal_Fail_Hour", UpsInjHour, eParameterDataType.Integer);
															}
													}
												}
												else
												{
													if (UpsAPSInd == 1 && 0 <= UpsCalError && ((CalSpanVal <= 50 && UpsCalError <= 5) || (CalSpanVal > 50 && CalSpanVal <= 200 && UpsCalError <= 10)))
													{
														decimal Tol = GetTolerance("7DAY", "DifferencePPM", Category);
														if (Math.Abs(Diff - UpsCalError) <= Tol)
														{
															if (!DailyCalCalcRes.InList("INC,FAILED"))
																Category.SetCheckParameter("Daily_Cal_Calc_Result", "PASSED", eParameterDataType.String);
														}
														else
														{
															Category.SetCheckParameter("Daily_Cal_Calc_Result", "FAILED", eParameterDataType.String);
															if (InjTimesValid)
																if (DayCalFailDate == DateTime.MinValue || DayCalFailDate > UpsInjDate || (DayCalFailDate == UpsInjDate && DayCalFailHour > UpsInjHour))
																{
																	Category.SetCheckParameter("Daily_Cal_Fail_Date", UpsInjDate, eParameterDataType.Date);
																	Category.SetCheckParameter("Daily_Cal_Fail_Hour", UpsInjHour, eParameterDataType.Integer);
																}
														}
													}
													else
													{
														Category.SetCheckParameter("Daily_Cal_Calc_Result", "FAILED", eParameterDataType.String);
														if (InjTimesValid)
															if (DayCalFailDate == DateTime.MinValue || DayCalFailDate > UpsInjDate || (DayCalFailDate == UpsInjDate && DayCalFailHour > UpsInjHour))
															{
																Category.SetCheckParameter("Daily_Cal_Fail_Date", UpsInjDate, eParameterDataType.Date);
																Category.SetCheckParameter("Daily_Cal_Fail_Hour", UpsInjHour, eParameterDataType.Integer);
															}
													}
												}
										}
										else
											if (!DailyCalCalcRes.InList("INC,FAILED,PASSAPS,IGNORED"))
												Category.SetCheckParameter("Daily_Cal_Calc_Result", "PASSED", eParameterDataType.String);
									}
									break;
								}
							case "FLOW":
								{
									UpsInjResult = Math.Min(Math.Round(Diff / CalSpanVal * 100, 1, MidpointRounding.AwayFromZero), (decimal)9999.9);
									Category.SetCheckParameter("Daily_Cal_Upscale_Injection_Calc_Result", UpsInjResult, eParameterDataType.Decimal);
									Diff = Math.Round(Diff, 2, MidpointRounding.AwayFromZero);
									string DailyCalCalcRes = Convert.ToString(Category.GetCheckParameter("Daily_Cal_Calc_Result").ParameterValue);
									string AcqCd = cDBConvert.ToString(CurrentDayCalTest["ACQ_CD"]);
									if (UpsInjResult > 6 && AcqCd == "DP" && Diff <= (decimal)0.02)
									{
										Category.SetCheckParameter("Daily_Cal_Upscale_Injection_Calc_Result", Diff, eParameterDataType.Decimal);
										Category.SetCheckParameter("Daily_Cal_Upscale_Injection_Calc_APS_Indicator", 1, eParameterDataType.Integer);
										if (!DailyCalCalcRes.InList("INVALID,FAILED,INC,IGNORED"))
											Category.SetCheckParameter("Daily_Cal_Calc_Result", "PASSAPS", eParameterDataType.String);
									}
									else
									{
										if (UpsInjResult > 6)
										{
											if (!DailyCalCalcRes.InList("INVALID,IGNORED"))
												if (UpsAPSInd != 1 && 0 <= UpsCalError && UpsCalError <= 6)
												{
													decimal Tol = GetTolerance("7DAY", "CalibrationError", Category);
													if (Math.Abs(UpsInjResult - UpsCalError) <= Tol)
													{
														if (!DailyCalCalcRes.InList("PASSAPS,INC,FAILED"))
															Category.SetCheckParameter("Daily_Cal_Calc_Result", "PASSED", eParameterDataType.String);
													}
													else
														Category.SetCheckParameter("Daily_Cal_Calc_Result", "FAILED", eParameterDataType.String);
												}
												else
												{
													if (UpsAPSInd == 1 && AcqCd == "DP" && 0 <= UpsCalError && UpsCalError <= (decimal)0.02)
													{
														decimal Tol = GetTolerance("7DAY", "DifferenceINH2O", Category);
														if (Math.Abs(Diff - UpsCalError) <= Tol)
														{
															if (!DailyCalCalcRes.InList("INC,FAILED"))
																Category.SetCheckParameter("Daily_Cal_Calc_Result", "PASSED", eParameterDataType.String);
														}
														else
														{
															Category.SetCheckParameter("Daily_Cal_Calc_Result", "FAILED", eParameterDataType.String);
															if (InjTimesValid)
																if (DayCalFailDate == DateTime.MinValue || DayCalFailDate > UpsInjDate || (DayCalFailDate == UpsInjDate && DayCalFailHour > UpsInjHour))
																{
																	Category.SetCheckParameter("Daily_Cal_Fail_Date", UpsInjDate, eParameterDataType.Date);
																	Category.SetCheckParameter("Daily_Cal_Fail_Hour", UpsInjHour, eParameterDataType.Integer);
																}
														}
													}
													else
													{
														Category.SetCheckParameter("Daily_Cal_Calc_Result", "FAILED", eParameterDataType.String);
														if (InjTimesValid)
															if (DayCalFailDate == DateTime.MinValue || DayCalFailDate > UpsInjDate || (DayCalFailDate == UpsInjDate && DayCalFailHour > UpsInjHour))
															{
																Category.SetCheckParameter("Daily_Cal_Fail_Date", UpsInjDate, eParameterDataType.Date);
																Category.SetCheckParameter("Daily_Cal_Fail_Hour", UpsInjHour, eParameterDataType.Integer);
															}
													}
												}
										}
										else
											if (!DailyCalCalcRes.InList("INC,FAILED,PASSAPS,IGNORED"))
												Category.SetCheckParameter("Daily_Cal_Calc_Result", "PASSED", eParameterDataType.String);
									}
									break;
								}

							case "HG":
								{
									EmParameters.DailyCalUpscaleInjectionCalcResult = Math.Min(Math.Round(Diff / EmParameters.DailyCalSpanValue.GetValueOrDefault() * 100, 1, MidpointRounding.AwayFromZero), (decimal)9999.9);
									Diff = Math.Round(Diff, 1, MidpointRounding.AwayFromZero);

									if (EmParameters.DailyCalUpscaleInjectionCalcResult > 5 && Diff <= 1)
									{
										EmParameters.DailyCalUpscaleInjectionCalcResult = Diff;
										EmParameters.DailyCalUpscaleInjectionCalcApsIndicator = 1;
										if (EmParameters.DailyCalCalcResult.NotInList("INC,FAILED,INVALID,IGNORED"))
											EmParameters.DailyCalCalcResult = "PASSAPS";
									}
									else
									{
										if (EmParameters.DailyCalUpscaleInjectionCalcResult > 5)
										{
											if (EmParameters.DailyCalCalcResult.NotInList("INVALID,IGNORED"))
												if (UpsAPSInd != 1 && 0 <= UpsCalError && UpsCalError <= 5)
												{
													decimal Tol = GetTolerance("7DAY", "CalibrationError", Category);
													if (Math.Abs(EmParameters.DailyCalUpscaleInjectionCalcResult.GetValueOrDefault() - UpsCalError) <= Tol)
													{
														if (EmParameters.DailyCalCalcResult.NotInList("PASSAPS,INC,FAILED"))
															EmParameters.DailyCalCalcResult = "PASSED";
													}
													else
													{
														EmParameters.DailyCalCalcResult = "FAILED";
														if (EmParameters.DailyCalInjectionTimesValid == true)
                              if (EmParameters.DailyCalFailDate == null
																|| EmParameters.DailyCalFailDate > UpsInjDate
																|| (EmParameters.DailyCalFailDate == UpsInjDate && EmParameters.DailyCalFailHour > UpsInjHour))
															{
																EmParameters.DailyCalFailDate = UpsInjDate;
																EmParameters.DailyCalFailHour = UpsInjHour;
															}
													}
												}
												else
												{
													if (UpsAPSInd == 1 && 0 <= UpsCalError && UpsCalError <= 1)
													{
														decimal Tol = GetTolerance("7DAY", "DifferenceUGSCM", Category);
														if (Math.Abs(Diff - UpsCalError) <= Tol)
														{
															if (EmParameters.DailyCalCalcResult.NotInList("INC,FAILED"))
																EmParameters.DailyCalCalcResult = "PASSED";
														}
														else
														{
															EmParameters.DailyCalCalcResult = "FAILED";
															if (EmParameters.DailyCalInjectionTimesValid == true)
                                if (EmParameters.DailyCalFailDate == null
																	|| EmParameters.DailyCalFailDate > UpsInjDate
																	|| (EmParameters.DailyCalFailDate == UpsInjDate && EmParameters.DailyCalFailHour > UpsInjHour))
																{
																	EmParameters.DailyCalFailDate = UpsInjDate;
																	EmParameters.DailyCalFailHour = UpsInjHour;
																}
														}
													}
													else
													{
														EmParameters.DailyCalCalcResult = "FAILED";
														if (EmParameters.DailyCalInjectionTimesValid == true)
															if (EmParameters.DailyCalFailDate == null
																|| EmParameters.DailyCalFailDate > UpsInjDate
																|| (EmParameters.DailyCalFailDate == UpsInjDate && EmParameters.DailyCalFailHour > UpsInjHour))
															{
																EmParameters.DailyCalFailDate = UpsInjDate;
																EmParameters.DailyCalFailHour = UpsInjHour;
															}
													}
												}
										}
										else
											if (EmParameters.DailyCalCalcResult.NotInList("INC,FAILED,PASSAPS,IGNORED"))
												EmParameters.DailyCalCalcResult = "PASSED";
									}
									break;
								}

							default:
								break;
						}
					}
				}
				else
					Log = false;
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "DAYCAL21");
			}

			return ReturnVal;
		}

		public string DAYCAL22(cCategory Category, ref bool Log)
		//Daily Calibration Test End Time Valid
		{
			string ReturnVal = "";

			try
			{
				if (Category.GetCheckParameter("Evaluate_Upscale_Injection").ValueAsBool() &&
					Category.GetCheckParameter("Evaluate_Zero_Injection").ValueAsBool() &&
					Category.GetCheckParameter("EM_Test_Date_Valid").ValueAsBool() &&
					Category.GetCheckParameter("EM_Test_Hour_Valid").ValueAsBool() &&
					Category.GetCheckParameter("EM_Test_Minute_Valid").ValueAsBool() &&
					Category.GetCheckParameter("Daily_Cal_Injection_Times_Valid").ValueAsBool())
				{
					DataRowView CurrentDayCalTest = (DataRowView)Category.GetCheckParameter("Current_Daily_Calibration_Test").ParameterValue;
					DateTime TestDate = cDBConvert.ToDate(CurrentDayCalTest["DAILY_TEST_DATE"], DateTypes.START);
					int TestHour = cDBConvert.ToHour(CurrentDayCalTest["DAILY_TEST_HOUR"], DateTypes.START);
					int TestMin = cDBConvert.ToMinute(CurrentDayCalTest["DAILY_TEST_MIN"], DateTypes.START);
					DateTime ZeroDate = cDBConvert.ToDate(CurrentDayCalTest["ZERO_INJECTION_DATE"], DateTypes.END);
					int ZeroHour = cDBConvert.ToHour(CurrentDayCalTest["ZERO_INJECTION_HOUR"], DateTypes.START);
					int ZeroMin = cDBConvert.ToInteger(CurrentDayCalTest["ZERO_INJECTION_MIN"]);
					DateTime UpsDate = cDBConvert.ToDate(CurrentDayCalTest["UPSCALE_INJECTION_DATE"], DateTypes.START);
					int UpsHour = cDBConvert.ToHour(CurrentDayCalTest["UPSCALE_INJECTION_HOUR"], DateTypes.START);
					int UpsMin = cDBConvert.ToInteger(CurrentDayCalTest["UPSCALE_INJECTION_MIN"]);
					bool UpsIsLater = true;
					if (ZeroDate > UpsDate)
						UpsIsLater = false;
					else
						if (ZeroDate == UpsDate)
							if (ZeroHour > UpsHour)
								UpsIsLater = false;
							else
								if (ZeroHour == UpsHour && ZeroMin > UpsMin)
									UpsIsLater = false;
					if (UpsIsLater)
					{
						if (TestDate != UpsDate || TestHour != UpsHour || (UpsMin != int.MinValue && TestMin != UpsMin))
							Category.CheckCatalogResult = "A";
					}
					else
						if (TestDate != ZeroDate || TestHour != ZeroHour || (ZeroMin != int.MinValue && TestMin != ZeroMin))
							Category.CheckCatalogResult = "A";
				}
				else
					Log = false;
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "DAYCAL22");
			}

			return ReturnVal;
		}

		public string DAYCAL23(cCategory Category, ref bool Log)
		//Reported Zero Injection Results Consistent with Recalculated Values
		{
			string ReturnVal = "";

			try
			{
				if (Category.GetCheckParameter("Evaluate_Zero_Injection").ValueAsBool())
				{
					DataRowView CurrentDayCalTest = (DataRowView)Category.GetCheckParameter("Current_Daily_Calibration_Test").ParameterValue;
					int APSInd = cDBConvert.ToInteger(CurrentDayCalTest["ZERO_APS_IND"]);
					string CompTypeCd = cDBConvert.ToString(CurrentDayCalTest["COMPONENT_TYPE_CD"]);
					string AcqCd = cDBConvert.ToString(CurrentDayCalTest["ACQ_CD"]);
					if (APSInd == 1 && CompTypeCd == "FLOW" && AcqCd != "DP")
						Category.CheckCatalogResult = "A";
					else
						if (APSInd == 1 && CompTypeCd.InList("SO2,NOX") && cDBConvert.ToDecimal(Category.GetCheckParameter("Daily_Cal_Span_Value").ParameterValue) >= 200)
							Category.CheckCatalogResult = "B";
						else
							if (APSInd == 1 && CompTypeCd.InList("CO2,O2"))
								Category.CheckCatalogResult = "C";
							else
							{
								decimal ZeroInjCalcResult = cDBConvert.ToDecimal(Category.GetCheckParameter("Daily_Cal_Zero_Injection_Calc_Result").ParameterValue);
								if (ZeroInjCalcResult != decimal.MinValue)
								{
									int ParameterAPSInd = Category.GetCheckParameter("Daily_Cal_Zero_Injection_Calc_APS_Indicator").ValueAsInt();
									if (APSInd != 1 && ParameterAPSInd == 1)
										Category.CheckCatalogResult = "D";
									else
									{
										decimal ZeroCalErr = cDBConvert.ToDecimal(CurrentDayCalTest["ZERO_CAL_ERROR"]);
										if (ZeroCalErr >= 0)
										{
											DataView ToleranceView = (DataView)Category.GetCheckParameter("Test_Tolerances_Cross_Check_Table").ParameterValue;
											DataRowView ToleranceRow;
											sFilterPair[] ToleranceFilter = new sFilterPair[2];
											ToleranceFilter[0].Set("TestTypeCode", "7DAY");
											decimal Tolerance = 0;

											if (CompTypeCd.InList("CO2,O2"))
											{
												ToleranceFilter[1].Set("FieldDescription", "DifferencePCT");
												if (FindRow(ToleranceView, ToleranceFilter, out ToleranceRow))
													Tolerance = cDBConvert.ToDecimal(ToleranceRow["Tolerance"]);
												if (Math.Abs(ZeroInjCalcResult - ZeroCalErr) > Tolerance)
													Category.CheckCatalogResult = "E";
											}
											if (string.IsNullOrEmpty(Category.CheckCatalogResult))
												if (ParameterAPSInd == 1)
													if (CompTypeCd == "FLOW")
													{
														ToleranceFilter[1].Set("FieldDescription", "DifferenceINH2O");
														if (FindRow(ToleranceView, ToleranceFilter, out ToleranceRow))
															Tolerance = cDBConvert.ToDecimal(ToleranceRow["Tolerance"]);
														if (Math.Abs(ZeroInjCalcResult - ZeroCalErr) > Tolerance)
															Category.CheckCatalogResult = "E";
													}
													else if (CompTypeCd == "HG")
													{
														ToleranceFilter[1].Set("FieldDescription", "DifferenceUGSCM");
														if (FindRow(ToleranceView, ToleranceFilter, out ToleranceRow))
															Tolerance = cDBConvert.ToDecimal(ToleranceRow["Tolerance"]);
														if (Math.Abs(ZeroInjCalcResult - ZeroCalErr) > Tolerance)
															Category.CheckCatalogResult = "E";
													}

													else
													{
														ToleranceFilter[1].Set("FieldDescription", "DifferencePPM");
														if (FindRow(ToleranceView, ToleranceFilter, out ToleranceRow))
															Tolerance = cDBConvert.ToDecimal(ToleranceRow["Tolerance"]);
														if (Math.Abs(ZeroInjCalcResult - ZeroCalErr) > Tolerance)
															Category.CheckCatalogResult = "E";
													}
												else
													if (APSInd == 0)
													{
														ToleranceFilter[1].Set("FieldDescription", "CalibrationError");
														if (FindRow(ToleranceView, ToleranceFilter, out ToleranceRow))
															Tolerance = cDBConvert.ToDecimal(ToleranceRow["Tolerance"]);
														if (Math.Abs(ZeroInjCalcResult - ZeroCalErr) > Tolerance)
															Category.CheckCatalogResult = "F";
													}
										}
									}
								}
							}
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "DAYCAL23");
			}

			return ReturnVal;
		}

		public string DAYCAL24(cCategory Category, ref bool Log)
		//Reported Upscale Injection Results Consistent with Recalculated Values
		{
			string ReturnVal = "";

			try
			{
				if (Category.GetCheckParameter("Evaluate_Upscale_Injection").ValueAsBool())
				{
					DataRowView CurrentDayCalTest = (DataRowView)Category.GetCheckParameter("Current_Daily_Calibration_Test").ParameterValue;
					int APSInd = cDBConvert.ToInteger(CurrentDayCalTest["UPSCALE_APS_IND"]);
					string CompTypeCd = cDBConvert.ToString(CurrentDayCalTest["COMPONENT_TYPE_CD"]);
					string AcqCd = cDBConvert.ToString(CurrentDayCalTest["ACQ_CD"]);
					if (APSInd == 1 && CompTypeCd == "FLOW" && AcqCd != "DP")
						Category.CheckCatalogResult = "A";
					else
						if (APSInd == 1 && CompTypeCd.InList("SO2,NOX") && cDBConvert.ToDecimal(Category.GetCheckParameter("Daily_Cal_Span_Value").ParameterValue) >= 200)
							Category.CheckCatalogResult = "B";
						else
							if (APSInd == 1 && CompTypeCd.InList("CO2,O2"))
								Category.CheckCatalogResult = "C";
							else
							{
								decimal UpsInjCalcResult = cDBConvert.ToDecimal(Category.GetCheckParameter("Daily_Cal_Upscale_Injection_Calc_Result").ParameterValue);
								if (UpsInjCalcResult != decimal.MinValue)
								{
									int ParameterAPSInd = Category.GetCheckParameter("Daily_Cal_Upscale_Injection_Calc_APS_Indicator").ValueAsInt();
									if (APSInd != 1 && ParameterAPSInd == 1)
										Category.CheckCatalogResult = "D";
									else
									{
										decimal UpsCalErr = cDBConvert.ToDecimal(CurrentDayCalTest["UPSCALE_CAL_ERROR"]);
										if (UpsCalErr >= 0)
										{
											DataView ToleranceView = (DataView)Category.GetCheckParameter("Test_Tolerances_Cross_Check_Table").ParameterValue;
											DataRowView ToleranceRow;
											sFilterPair[] ToleranceFilter = new sFilterPair[2];
											ToleranceFilter[0].Set("TestTypeCode", "7DAY");
											decimal Tolerance = 0;

											if (CompTypeCd.InList("CO2,O2"))
											{
												ToleranceFilter[1].Set("FieldDescription", "DifferencePCT");
												if (FindRow(ToleranceView, ToleranceFilter, out ToleranceRow))
													Tolerance = cDBConvert.ToDecimal(ToleranceRow["Tolerance"]);
												if (Math.Abs(UpsInjCalcResult - UpsCalErr) > Tolerance)
													Category.CheckCatalogResult = "E";
											}
											if (string.IsNullOrEmpty(Category.CheckCatalogResult))
												if (ParameterAPSInd == 1)
													if (CompTypeCd == "FLOW")
													{
														ToleranceFilter[1].Set("FieldDescription", "DifferenceINH2O");
														if (FindRow(ToleranceView, ToleranceFilter, out ToleranceRow))
															Tolerance = cDBConvert.ToDecimal(ToleranceRow["Tolerance"]);
														if (Math.Abs(UpsInjCalcResult - UpsCalErr) > Tolerance)
															Category.CheckCatalogResult = "E";
													}
													else if (CompTypeCd == "HG")
													{
														ToleranceFilter[1].Set("FieldDescription", "DifferenceUGSCM");
														if (FindRow(ToleranceView, ToleranceFilter, out ToleranceRow))
															Tolerance = cDBConvert.ToDecimal(ToleranceRow["Tolerance"]);
														if (Math.Abs(UpsInjCalcResult - UpsCalErr) > Tolerance)
															Category.CheckCatalogResult = "E";
													}
													else
													{
														ToleranceFilter[1].Set("FieldDescription", "DifferencePPM");
														if (FindRow(ToleranceView, ToleranceFilter, out ToleranceRow))
															Tolerance = cDBConvert.ToDecimal(ToleranceRow["Tolerance"]);
														if (Math.Abs(UpsInjCalcResult - UpsCalErr) > Tolerance)
															Category.CheckCatalogResult = "E";
													}
												else
													if (APSInd == 0)
													{
														ToleranceFilter[1].Set("FieldDescription", "CalibrationError");
														if (FindRow(ToleranceView, ToleranceFilter, out ToleranceRow))
															Tolerance = cDBConvert.ToDecimal(ToleranceRow["Tolerance"]);
														if (Math.Abs(UpsInjCalcResult - UpsCalErr) > Tolerance)
															Category.CheckCatalogResult = "F";
													}
										}
									}
								}
							}
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "DAYCAL24");
			}

			return ReturnVal;
		}

		public string DAYCAL25(cCategory Category, ref bool Log)
		//Determination of Overall Daily Calibration Test Result
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentDayCalTest = (DataRowView)Category.GetCheckParameter("Current_Daily_Calibration_Test").ParameterValue;
				string DailyCalCalcRes = Convert.ToString(Category.GetCheckParameter("Daily_Cal_Calc_Result").ParameterValue);
				if (DailyCalCalcRes == "INVALID")
				{
					Category.SetCheckParameter("Daily_Cal_Calc_Result", null, eParameterDataType.String);
					DailyCalCalcRes = "";
				}
				string TestResCd = cDBConvert.ToString(CurrentDayCalTest["TEST_RESULT_CD"]);
				if (TestResCd == "")
					Category.CheckCatalogResult = "A";
				else
					if (!TestResCd.InList("PASSED,PASSAPS,FAILED,INC,ABORTED"))
						Category.CheckCatalogResult = "B";
					else
						if (DailyCalCalcRes == "FAILED")
						{
							if (TestResCd.InList("PASSED,PASSAPS"))
								Category.CheckCatalogResult = "C";
							else
								if (TestResCd == "INC")
									Category.CheckCatalogResult = "D";
						}
						else
							if (DailyCalCalcRes.InList("PASSED,PASSAPS") && TestResCd == "FAILED")
								Category.CheckCatalogResult = "E";
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "DAYCAL25");
			}

			return ReturnVal;
		}

		/// <summary>
		/// DAYCAL-26: Upscale Gas Type Code Valid
		/// </summary>
		/// <param name="category">Category Object</param>
		/// <param name="log">Indicates whether to log results.</param>
		/// <returns>Returns error message if check fails to run correctly.</returns>
		public string DAYCAL26(cCategory category, ref bool log)
		{
			string returnVal = "";

			try
			{
				if (EvaluateUpscaleInjection.Value.Default())
				{
					UpscaleGasTypeValid.SetValue(true, category);

					DailyCalPgvpRuleDate.SetValue(PgvpAetbRuleDate, category);

					if (CurrentDailyCalibrationTest.Value["UPSCALE_GAS_TYPE_CD"].IsDbNull())
					{
						if (CurrentDailyCalibrationTest.Value["COMPONENT_TYPE_CD"].AsString().InList("SO2,NOX,CO2,O2") &&
							(CurrentDailyCalibrationTest.Value["DAILY_TEST_DATE"].AsDateTime(DateTime.MaxValue) >= new DateTime(2011, 9, 26)))
						{
							UpscaleGasTypeValid.SetValue(false, category);
							category.CheckCatalogResult = "A";
						}
					}

					else
					{
						if (CurrentDailyCalibrationTest.Value["COMPONENT_TYPE_CD"].AsString().InList("FLOW,HG,HCL"))
						{
							UpscaleGasTypeValid.SetValue(false, category);
							category.CheckCatalogResult = "B";
						}
						else if (CurrentDailyCalibrationTest.Value["UPSCALE_GAS_TYPE_CD"].AsString().NotInList("GMIS,PRM,RGM,SRM"))
						{
							DataRowView gasTypeCodeRow;

							if (!cRowFilter.FindRow(GasTypeCodeLookupTable.Value,
										   new cFilterCondition[] { new cFilterCondition("GAS_TYPE_CD", CurrentDailyCalibrationTest.Value["UPSCALE_GAS_TYPE_CD"].AsString()) },
										   out gasTypeCodeRow))
							{
								UpscaleGasTypeValid.SetValue(false, category);
								category.CheckCatalogResult = "C";
							}

							else if (CurrentDailyCalibrationTest.Value["UPSCALE_GAS_TYPE_CD"].AsString().InList("ZERO,ZAM"))
							{
								UpscaleGasTypeValid.SetValue(false, category);
								category.CheckCatalogResult = "C";
							}

							else if (CurrentDailyCalibrationTest.Value["UPSCALE_GAS_TYPE_CD"].AsString() == "APPVD")
							{
								category.CheckCatalogResult = "D";
							}

							else if (CurrentDailyCalibrationTest.Value["COMPONENT_TYPE_CD"].AsString().InList("SO2,NOX,CO2,O2"))
							{
								DataView protocolGasParameterToTypeView
								  = LocateProtocolGasParameterToTypeCrossReference(CurrentDailyCalibrationTest.Value["COMPONENT_TYPE_CD"].AsString(),
																				   CurrentDailyCalibrationTest.Value["UPSCALE_GAS_TYPE_CD"].AsString());

								if ((protocolGasParameterToTypeView == null) || (protocolGasParameterToTypeView.Count == 0))
								{
									UpscaleGasTypeValid.SetValue(false, category);
									category.CheckCatalogResult = "E";
								}
								else if ((CurrentDailyCalibrationTest.Value["COMPONENT_TYPE_CD"].AsString() == "O2") &&
										 (CurrentDailyCalibrationTest.Value["UPSCALE_GAS_TYPE_CD"].AsString() == "AIR") &&
										 (CurrentDailyCalibrationTest.Value["UPSCALE_GAS_LEVEL_CD"].AsString() != "HIGH"))
								{
									UpscaleGasTypeValid.SetValue(false, category);
									category.CheckCatalogResult = "F";
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
		/// DAYCAL-27: Cylinder ID Valid
		/// </summary>
		/// <param name="category">Category Object</param>
		/// <param name="log">Indicates whether to log results.</param>
		/// <returns>Returns error message if check fails to run correctly.</returns>
		public string DAYCAL27(cCategory category, ref bool log)
		{
			string returnVal = "";

			try
			{
				if (EvaluateUpscaleInjection.Value.Default())
				{
					if (CurrentDailyCalibrationTest.Value["CYLINDER_ID"].IsDbNull())
					{
						if (UpscaleGasTypeValid.Value.Default() &&
							CurrentDailyCalibrationTest.Value["UPSCALE_GAS_TYPE_CD"].IsNotDbNull() &&
							(CurrentDailyCalibrationTest.Value["UPSCALE_GAS_TYPE_CD"].AsString().NotInList("AIR")))
						{
							category.CheckCatalogResult = "A";
						}
					}
					else
					{
						if (CurrentDailyCalibrationTest.Value["UPSCALE_GAS_TYPE_CD"].AsString().InList("AIR"))
						{
							category.CheckCatalogResult = "B";
						}
						else if (UpscaleGasTypeValid.Value.Default() &&
								 CurrentDailyCalibrationTest.Value["UPSCALE_GAS_TYPE_CD"].IsDbNull())
						{
							category.CheckCatalogResult = "C";
                        }
                        else if (!EmParameters.CurrentDailyCalibrationTest.CylinderId.All(letter => "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890-&.".Contains(letter.ToString())))
                        {
                            if (!EmParameters.InvalidCylinderIdList.Contains(EmParameters.CurrentDailyCalibrationTest.CylinderId))
                            {
                                EmParameters.InvalidCylinderIdList.Add(EmParameters.CurrentDailyCalibrationTest.CylinderId);
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
		/// DAYCAL-28: Vendor ID Valid
		/// </summary>
		/// <param name="category">Category Object</param>
		/// <param name="log">Indicates whether to log results.</param>
		/// <returns>Returns error message if check fails to run correctly.</returns>
		public string DAYCAL28(cCategory category, ref bool log)
		{
			string returnVal = "";

			try
			{
				if (EvaluateUpscaleInjection.Value.Default())
				{
					DataRowView protocalGasVendorRow;

					if (CurrentDailyCalibrationTest.Value["VENDOR_ID"].IsDbNull())
					{
						if (UpscaleGasTypeValid.Value.Default() &&
							CurrentDailyCalibrationTest.Value["UPSCALE_GAS_TYPE_CD"].IsNotDbNull() &&
							CurrentDailyCalibrationTest.Value["UPSCALE_GAS_TYPE_CD"].AsString().NotInList("AIR,SRM,NTRM,GMIS,RGM,PRM"))
						{
							category.CheckCatalogResult = "A";
						}
					}

					/*
					  The check below includes ensuring that the vendor id equals a trimmed version of the vendor id because the
					  RowMatches_String method used in cRowFilter uses the cDBConvert.ToString method, which trims the result before
					  RowMatches_String compares the value.  At this time correcting cDBConvert.ToString would require too much testing.
					*/
					else if (!cRowFilter.FindRow(ProtocolGasVendorLookupTable.Value,
												 new cFilterCondition[] { new cFilterCondition("VENDOR_ID", CurrentDailyCalibrationTest.Value["VENDOR_ID"].AsString()) },
												 out protocalGasVendorRow) ||
							 (CurrentDailyCalibrationTest.Value["VENDOR_ID"].AsString() != CurrentDailyCalibrationTest.Value["VENDOR_ID"].AsString().Trim()))
					{
						category.CheckCatalogResult = "B";
					}

					else
					{
						if (CurrentDailyCalibrationTest.Value["UPSCALE_GAS_TYPE_CD"].AsString().InList("AIR,SRM,NTRM,GMIS,RGM,PRM"))
						{
							category.CheckCatalogResult = "C";
						}
						else if (protocalGasVendorRow["DEACTIVATION_DATE"].IsNotDbNull() &&
								 (CurrentDailyCalibrationTest.Value["DAILY_TEST_DATE"].AsDateTime()
									>= new DateTime(protocalGasVendorRow["DEACTIVATION_DATE"].AsDateTime().Value.Year + 9, 1, 1))) //Jan 1 of next year after + 8
						
						{
							category.CheckCatalogResult = "F";
						}
                        else if (protocalGasVendorRow["ACTIVATION_DATE"].AsDateTime() > CurrentDailyCalibrationTest.Value["DAILY_TEST_DATE"].AsDateTime())
                        {
                            category.CheckCatalogResult = "G";
                        }
                        else if ((CurrentDailyCalibrationTest.Value["VENDOR_ID"].AsString() == "NONPGVP") &&
								 ((DailyCalPgvpRuleDate.Value == null) ||
								  (CurrentDailyCalibrationTest.Value["DAILY_TEST_DATE"].AsDateTime(DateTime.MaxValue)
									 >= DailyCalPgvpRuleDate.Value.Default().AddDays(60).AddYears(8))))
						{
							category.CheckCatalogResult = "D";
						}
						else if (UpscaleGasTypeValid.Value.Default() &&
								 CurrentDailyCalibrationTest.Value["UPSCALE_GAS_TYPE_CD"].IsDbNull())
						{
							category.CheckCatalogResult = "E";
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
		/// DAYCAL-29: Cylinder Expiration Date Valid
		/// </summary>
		/// <param name="category">Category Object</param>
		/// <param name="log">Indicates whether to log results.</param>
		/// <returns>Returns error message if check fails to run correctly.</returns>
		public string DAYCAL29(cCategory category, ref bool log)
		{
			string returnVal = "";

			try
			{
				if (EvaluateUpscaleInjection.Value.Default())
				{
					if (CurrentDailyCalibrationTest.Value["EXPIRATION_DATE"].IsDbNull())
					{
						if (UpscaleGasTypeValid.Value.Default() &&
							CurrentDailyCalibrationTest.Value["UPSCALE_GAS_TYPE_CD"].IsNotDbNull() &&
							CurrentDailyCalibrationTest.Value["UPSCALE_GAS_TYPE_CD"].AsString().NotInList("AIR"))
						{
							category.CheckCatalogResult = "A";
						}
					}
					else
					{
						if (CurrentDailyCalibrationTest.Value["UPSCALE_GAS_TYPE_CD"].AsString().InList("AIR"))
						{
							category.CheckCatalogResult = "B";
						}
						else if (CurrentDailyCalibrationTest.Value["EXPIRATION_DATE"].AsDateTime(DateTime.MaxValue)
								   < CurrentDailyCalibrationTest.Value["DAILY_TEST_DATE"].AsDateTime(DateTime.MinValue))
						{
							category.CheckCatalogResult = "C";
						}
						else if (CurrentDailyCalibrationTest.Value["EXPIRATION_DATE"].AsDateTime(DateTime.MaxValue)
								   > CurrentDailyCalibrationTest.Value["DAILY_TEST_DATE"].AsDateTime(DateTime.MinValue).AddYears(8))
						{
							category.CheckCatalogResult = "D";
						}
						else if (UpscaleGasTypeValid.Value.Default() &&
								 CurrentDailyCalibrationTest.Value["UPSCALE_GAS_TYPE_CD"].IsDbNull())
						{
							category.CheckCatalogResult = "E";
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
		/// DAYCAL-30: Upscale Gas Type Code Components Valid
		/// </summary>
		/// <param name="category">Category Object</param>
		/// <param name="log">Indicates whether to log results.</param>
		/// <returns>Returns error message if check fails to run correctly.</returns>
		public string DAYCAL30(cCategory category, ref bool log)
		{
			string returnVal = "";

			try
			{
				if (EvaluateUpscaleInjection.Value.Default())
				{
					UpscaleGasTypeValid.SetValue(true, category);

					DailyCalPgvpRuleDate.SetValue(PgvpAetbRuleDate, category);

					if (CurrentDailyCalibrationTest.Value["UPSCALE_GAS_TYPE_CD"].IsDbNull())
					{
						if (CurrentDailyCalibrationTest.Value["COMPONENT_TYPE_CD"].AsString().InList("SO2,NOX,CO2,O2") &&
							(CurrentDailyCalibrationTest.Value["DAILY_TEST_DATE"].AsDateTime(DateTime.MaxValue) >= new DateTime(2011, 9, 26)))
						{
							UpscaleGasTypeValid.SetValue(false, category);
							category.CheckCatalogResult = "A";
						}
					}

					else
					{
						if (CurrentDailyCalibrationTest.Value["COMPONENT_TYPE_CD"].AsString().InList("FLOW,HCL,HG"))
						{
							UpscaleGasTypeValid.SetValue(false, category);
							category.CheckCatalogResult = "B";
						}
						else
						{

							ProtocolGasInvalidComponentList.SetValue(null, category);
							ProtocolGasExclusiveComponentList.SetValue(null, category);
							ProtocolGasBalanceComponentList.SetValue(null, category);
							ProtocolGasDuplicateComponentList.SetValue(null, category);

							string protocolGasComponentList = null;
							bool protocolGasApprovalRequested = false;
							int protocolGasComponentCount = 0;
							int balanceComponentCount = 0;

							// Check above insures GAS_TYPE_CD is not null
							string upscaleGasTypeCd = CurrentDailyCalibrationTest.Value["UPSCALE_GAS_TYPE_CD"].AsString();
							string[] upscaleGasTypeCdPrased = upscaleGasTypeCd.Split(',');

							// Loop through gas component codes stored in GAS_TYPE_CD
							foreach (string gasComponentCd in upscaleGasTypeCdPrased)
							{
								DataRowView gasComponentCodeLookupRow = cRowFilter.FindRow(GasComponentCodeLookupTable.Value,
																						   new cFilterCondition[] 
                                                                         { 
                                                                           new cFilterCondition("GAS_COMPONENT_CD", gasComponentCd) 
                                                                         });

								if ((gasComponentCodeLookupRow == null) || (gasComponentCd == "ZERO"))
								{
									if (gasComponentCd.NotInList(ProtocolGasInvalidComponentList.Value))
										ProtocolGasInvalidComponentList.AggregateValue(gasComponentCd);
								}
								else
								{
									if (gasComponentCodeLookupRow["CAN_COMBINE_IND"].AsInteger(0) == 0)
									{
										if (gasComponentCd.NotInList(ProtocolGasExclusiveComponentList.Value))
											ProtocolGasExclusiveComponentList.AggregateValue(gasComponentCd);
									}

									if (gasComponentCodeLookupRow["BALANCE_COMPONENT_IND"].AsInteger(0) == 1)
									{
										if (gasComponentCd.NotInList(ProtocolGasBalanceComponentList.Value))
											ProtocolGasBalanceComponentList.AggregateValue(gasComponentCd);

										balanceComponentCount += 1;
									}
								}

								if (gasComponentCd == "APPVD")
								{
									protocolGasApprovalRequested = true;
								}

								if (gasComponentCd.NotInList(protocolGasComponentList))
								{
									protocolGasComponentList = protocolGasComponentList.ListAdd(gasComponentCd);
								}
								else if (gasComponentCd.NotInList(ProtocolGasDuplicateComponentList.Value))
								{
									ProtocolGasDuplicateComponentList.AggregateValue(gasComponentCd);
								}

								protocolGasComponentCount += 1;
							}

							// Invalid components exist
							if (ProtocolGasInvalidComponentList.Value != null)
							{
								UpscaleGasTypeValid.SetValue(false);
								category.CheckCatalogResult = "C";
							}
							// Duplicate component reported
							else if (ProtocolGasDuplicateComponentList.Value != null)
							{
								UpscaleGasTypeValid.SetValue(false);
								category.CheckCatalogResult = "L";
							}
							// Exclusive component reported with other components
							else if ((ProtocolGasExclusiveComponentList.Value != null) && (protocolGasComponentCount > 1))
							{
								UpscaleGasTypeValid.SetValue(false);
								category.CheckCatalogResult = "D";
							}
							// Approval of non-standard gas requested.
							else if (protocolGasApprovalRequested)
							{
								UpscaleGasTypeValid.SetValue(false);
								category.CheckCatalogResult = "E";
							}
							else if ((ProtocolGasExclusiveComponentList.Value == null) && (balanceComponentCount == 0))
							{
								UpscaleGasTypeValid.SetValue(false);
								category.CheckCatalogResult = "J";
							}
							else if ((ProtocolGasExclusiveComponentList.Value == null) && (balanceComponentCount > 1))
							{
								UpscaleGasTypeValid.SetValue(false);
								category.CheckCatalogResult = "K";
							}
							else if (upscaleGasTypeCd.NotInList("GMIS,NTRM,PRM,RGM,SRM"))
							{
								string componentTypeCd = CurrentDailyCalibrationTest.Value["COMPONENT_TYPE_CD"].AsString();

								if (componentTypeCd.InList("SO2,CO2"))
								{
									if (componentTypeCd.NotInList(upscaleGasTypeCd))
									{
										UpscaleGasTypeValid.SetValue(false);
										category.CheckCatalogResult = "F";
									}
								}
								else if (componentTypeCd == "O2")
								{
									if ((upscaleGasTypeCd != "AIR") && ("O2").NotInList(upscaleGasTypeCd))
									{
										UpscaleGasTypeValid.SetValue(false);
										category.CheckCatalogResult = "G";
									}
									else if ((upscaleGasTypeCd == "AIR") && (CurrentDailyCalibrationTest.Value["UPSCALE_GAS_LEVEL_CD"].AsString() != "HIGH"))
									{
										UpscaleGasTypeValid.SetValue(false);
										category.CheckCatalogResult = "H";
									}
								}
								else if (componentTypeCd == "NOX")
								{
									if (("NO").NotInList(upscaleGasTypeCd) && ("NO2").NotInList(upscaleGasTypeCd) && ("NOX").NotInList(upscaleGasTypeCd))
									{
										UpscaleGasTypeValid.SetValue(false);
										category.CheckCatalogResult = "I";
									}
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

		#endregion

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

		private static DataView FilterRanged(DataView ASourceView, DateTime AOpDate, int AOpHour, bool Inclusive)
		{
			DataTable AFilterTable = ASourceView.Table.Clone();
			DataRow FilterRow;
			DateTime BeganDate; int BeganHour; DateTime EndedDate; int EndedHour;

			AFilterTable.Rows.Clear();

			foreach (DataRowView SourceRow in ASourceView)
			{
				BeganDate = cDBConvert.ToDate(SourceRow["BEGIN_DATE"], DateTypes.START);
				BeganHour = cDBConvert.ToHour(SourceRow["BEGIN_HOUR"], DateTypes.START);
				EndedDate = cDBConvert.ToDate(SourceRow["END_DATE"], DateTypes.END);
				EndedHour = cDBConvert.ToHour(SourceRow["END_HOUR"], DateTypes.END);

				if (Inclusive)
				{
					if (((BeganDate < AOpDate) || ((BeganDate == AOpDate) && (BeganHour <= AOpHour))) &&
						((EndedDate > AOpDate) || ((EndedDate == AOpDate) && (EndedHour >= AOpHour))))
					{
						FilterRow = AFilterTable.NewRow();

						foreach (DataColumn Column in AFilterTable.Columns)
							FilterRow[Column.ColumnName] = SourceRow[Column.ColumnName];

						AFilterTable.Rows.Add(FilterRow);
					}
				}
				else
					if (((BeganDate < AOpDate) || ((BeganDate == AOpDate) && (BeganHour < AOpHour))) &&
						((EndedDate > AOpDate) || ((EndedDate == AOpDate) && (EndedHour > AOpHour))))
					{
						FilterRow = AFilterTable.NewRow();

						foreach (DataColumn Column in AFilterTable.Columns)
							FilterRow[Column.ColumnName] = SourceRow[Column.ColumnName];

						AFilterTable.Rows.Add(FilterRow);
					}
			}

			AFilterTable.AcceptChanges();

			return AFilterTable.DefaultView;
		}

		/// <summary>
		/// Returns a DataView containing the Cross Check: Protocol Gas Parameter to Type rows
		/// for the passed Protocol Gas Parameters.
		/// </summary>
		/// <param name="protocolParameterList">The protocol gas parameters to locate in the cross check table.</param>
		/// <param name="gasTypeCd">The protocol gas parameters to locate in the cross check table.</param>
		/// <returns>A DataView of the located cross check rows.</returns>
		private DataView LocateProtocolGasParameterToTypeCrossReference(string protocolParameterList, string gasTypeCd)
		{
			DataView result;

			if (CrossCheckProtocolGasParameterToType.Value.HasValue())
			{
				DataTable table = CrossCheckProtocolGasParameterToType.Value.Table.Clone();

				foreach (DataRowView sourceRow in CrossCheckProtocolGasParameterToType.Value)
				{
					if (sourceRow["ProtocolGasParameter"].AsString().InList(protocolParameterList) &&
						gasTypeCd.InList(sourceRow["GasTypeList"].AsString()))
					{
						DataRow targetRow = table.NewRow();
						{
							foreach (DataColumn column in table.Columns)
								targetRow[column.ColumnName] = sourceRow[column.ColumnName];
						}
						table.Rows.Add(targetRow);
					}
				}

				result = table.DefaultView;
			}
			else
				result = null;


			return result;
		}

		#endregion

	}
}
