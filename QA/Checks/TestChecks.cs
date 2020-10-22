using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Extensions;
using ECMPS.Checks.Qa.Parameters;

namespace ECMPS.Checks.TestChecks
{
	public class cTestChecks : cChecks
	{
		#region Constructors

		public cTestChecks()
		{
			CheckProcedures = new dCheckProcedure[25];

			CheckProcedures[1] = new dCheckProcedure(TEST1);
			CheckProcedures[2] = new dCheckProcedure(TEST2);
			CheckProcedures[3] = new dCheckProcedure(TEST3);
			CheckProcedures[4] = new dCheckProcedure(TEST4);
			CheckProcedures[5] = new dCheckProcedure(TEST5);
			CheckProcedures[6] = new dCheckProcedure(TEST6);
			CheckProcedures[7] = new dCheckProcedure(TEST7);
			CheckProcedures[8] = new dCheckProcedure(TEST8);
			CheckProcedures[9] = new dCheckProcedure(TEST9);
			CheckProcedures[10] = new dCheckProcedure(TEST10);
			CheckProcedures[11] = new dCheckProcedure(TEST11);
			CheckProcedures[12] = new dCheckProcedure(TEST12);
			CheckProcedures[13] = new dCheckProcedure(TEST13);
			CheckProcedures[14] = new dCheckProcedure(TEST14);
			CheckProcedures[15] = new dCheckProcedure(TEST15);
			CheckProcedures[16] = new dCheckProcedure(TEST16);
			CheckProcedures[17] = new dCheckProcedure(TEST17);
			CheckProcedures[18] = new dCheckProcedure(TEST18);
			CheckProcedures[19] = new dCheckProcedure(TEST19);
			CheckProcedures[20] = new dCheckProcedure(TEST20);
			CheckProcedures[21] = new dCheckProcedure(TEST21);
			CheckProcedures[22] = new dCheckProcedure(TEST22);
			CheckProcedures[23] = new dCheckProcedure(TEST23);
			CheckProcedures[24] = new dCheckProcedure(TEST24);

		}


		#endregion


		#region Test Checks

		#region Test Checks 1-10
		public static string TEST1(cCategory Category, ref bool Log) // Valid Begin Date
		{
			string ReturnVal = "";

			try
			{

				Category.SetCheckParameter("Test_Begin_Date_Valid", true, eParameterDataType.Boolean);
				DataRowView CurrentTest = (DataRowView)Category.GetCheckParameter("Current_Test").ParameterValue;
				if (cDBConvert.ToDate(CurrentTest["begin_date"], DateTypes.START) == DateTime.MinValue)
				{
					Category.SetCheckParameter("Test_Begin_Date_Valid", false, eParameterDataType.Boolean);
					Category.CheckCatalogResult = "A";
				}
				else
				{
					if (cDBConvert.ToDate(CurrentTest["begin_date"], DateTypes.START) < new DateTime(1993, 1, 1) ||
					  cDBConvert.ToDate(CurrentTest["begin_date"], DateTypes.START) > DateTime.Now.Date)
					{
						Category.SetCheckParameter("Test_Begin_Date_Valid", false, eParameterDataType.Boolean);
						Category.CheckCatalogResult = "B";
					}
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "TEST1");
			}

			return ReturnVal;
		}

		public static string TEST2(cCategory Category, ref bool Log) // Valid Begin Hour
		{
			string ReturnVal = "";

			try
			{

				Category.SetCheckParameter("Test_Begin_Hour_Valid", true, eParameterDataType.Boolean);
				DataRowView CurrentTest = (DataRowView)Category.GetCheckParameter("Current_Test").ParameterValue;
				if (cDBConvert.ToInteger(CurrentTest["begin_hour"]) == int.MinValue)
				{
					Category.SetCheckParameter("Test_Begin_Hour_Valid", false, eParameterDataType.Boolean);
					Category.CheckCatalogResult = "A";
				}
				else
				{
					if (cDBConvert.ToInteger(CurrentTest["begin_hour"]) < 0 || cDBConvert.ToInteger(CurrentTest["begin_hour"]) > 23)
					{
						Category.SetCheckParameter("Test_Begin_Hour_Valid", false, eParameterDataType.Boolean);
						Category.CheckCatalogResult = "B";
					}
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "TEST2");
			}

			return ReturnVal;
		}

		public static string TEST3(cCategory Category, ref bool Log) // Valid Begin Minute
		{
			string ReturnVal = "";

			try
			{

				Category.SetCheckParameter("Test_Begin_Minute_Valid", true, eParameterDataType.Boolean);
				DataRowView CurrentTest = (DataRowView)Category.GetCheckParameter("Current_Test").ParameterValue;
				if (cDBConvert.ToInteger(CurrentTest["begin_min"]) == int.MinValue)
				{
					DateTime MPDate = Category.GetCheckParameter("ECMPS_MP_Begin_Date").ValueAsDateTime(DateTypes.START);
					if (cDBConvert.ToDate(CurrentTest["begin_date"], DateTypes.START) >= MPDate ||
					  cDBConvert.ToString(CurrentTest["test_type_cd"]).InList("RATA,LINE,CYCLE,F2LREF,UNITDEF,APPE"))
					{
						Category.SetCheckParameter("Test_Begin_Minute_Valid", false, eParameterDataType.Boolean);
						Category.CheckCatalogResult = "A";
					}
					else
						Category.CheckCatalogResult = "B";
				}
				else
				{
					if (cDBConvert.ToInteger(CurrentTest["begin_min"]) < 0 || cDBConvert.ToInteger(CurrentTest["begin_min"]) > 59)
					{
						Category.SetCheckParameter("Test_Begin_Minute_Valid", false, eParameterDataType.Boolean);
						Category.CheckCatalogResult = "C";
					}
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "TEST3");
			}

			return ReturnVal;
		}

		public static string TEST4(cCategory Category, ref bool Log) // Valid End Date
		{
			string ReturnVal = "";

			try
			{

				Category.SetCheckParameter("Test_End_Date_Valid", true, eParameterDataType.Boolean);
				DataRowView CurrentTest = (DataRowView)Category.GetCheckParameter("Current_Test").ParameterValue;
				if (cDBConvert.ToDate(CurrentTest["end_date"], DateTypes.START) == DateTime.MinValue)
				{
					Category.SetCheckParameter("Test_End_Date_Valid", false, eParameterDataType.Boolean);
					Category.CheckCatalogResult = "A";
				}
				else
				{
					if (cDBConvert.ToDate(CurrentTest["end_date"], DateTypes.START) < new DateTime(1993, 1, 1) ||
					  cDBConvert.ToDate(CurrentTest["end_date"], DateTypes.START) > DateTime.Now.Date)
					{
						Category.SetCheckParameter("Test_End_Date_Valid", false, eParameterDataType.Boolean);
						Category.CheckCatalogResult = "B";
					}
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "TEST4");
			}

			return ReturnVal;
		}

		public static string TEST5(cCategory Category, ref bool Log) // Valid End Hour
		{
			string ReturnVal = "";

			try
			{

				Category.SetCheckParameter("Test_End_Hour_Valid", true, eParameterDataType.Boolean);
				DataRowView CurrentTest = (DataRowView)Category.GetCheckParameter("Current_Test").ParameterValue;
				if (cDBConvert.ToInteger(CurrentTest["end_hour"]) == int.MinValue)
				{
					Category.SetCheckParameter("Test_End_Hour_Valid", false, eParameterDataType.Boolean);
					Category.CheckCatalogResult = "A";
				}
				else
				{
					if (cDBConvert.ToInteger(CurrentTest["end_hour"]) < 0 || cDBConvert.ToInteger(CurrentTest["end_hour"]) > 23)
					{
						Category.SetCheckParameter("Test_End_Hour_Valid", false, eParameterDataType.Boolean);
						Category.CheckCatalogResult = "B";
					}
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "TEST5");
			}

			return ReturnVal;
		}

		public static string TEST6(cCategory Category, ref bool Log) // Valid End Minute
		{
			string ReturnVal = "";

			try
			{

				Category.SetCheckParameter("Test_End_Minute_Valid", true, eParameterDataType.Boolean);
				DataRowView CurrentTest = (DataRowView)Category.GetCheckParameter("Current_Test").ParameterValue;
				if (cDBConvert.ToString(CurrentTest["TEST_TYPE_CD"]) != "ONOFF")
					if (cDBConvert.ToInteger(CurrentTest["end_min"]) == int.MinValue)
					{
						DateTime MPDate = Category.GetCheckParameter("ECMPS_MP_Begin_Date").ValueAsDateTime(DateTypes.START);
						if (cDBConvert.ToDate(CurrentTest["end_date"], DateTypes.START) >= MPDate ||
							cDBConvert.ToString(CurrentTest["test_type_cd"]).InList("RATA,LINE,CYCLE,F2LREF,UNITDEF,APPE"))
						{
							Category.SetCheckParameter("Test_End_Minute_Valid", false, eParameterDataType.Boolean);
							Category.CheckCatalogResult = "A";
						}
						else
							Category.CheckCatalogResult = "B";
					}
					else
					{
						if (cDBConvert.ToInteger(CurrentTest["end_min"]) < 0 || cDBConvert.ToInteger(CurrentTest["end_min"]) > 59)
						{
							Category.SetCheckParameter("Test_End_Minute_Valid", false, eParameterDataType.Boolean);
							Category.CheckCatalogResult = "C";
						}
					}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "TEST6");
			}

			return ReturnVal;
		}

		public static string TEST7(cCategory Category, ref bool Log) // Linearity Dates Consistent
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentTest = (DataRowView)Category.GetCheckParameter("Current_Test").ParameterValue;
				string TestTypeCd = cDBConvert.ToString(CurrentTest["TEST_TYPE_CD"]);
				if ((bool)Category.GetCheckParameter("Test_Begin_Date_Valid").ParameterValue &&
					(bool)Category.GetCheckParameter("Test_Begin_Hour_Valid").ParameterValue &&
					(bool)Category.GetCheckParameter("Test_End_Date_Valid").ParameterValue &&
					(bool)Category.GetCheckParameter("Test_End_Hour_Valid").ParameterValue)
				{
					if (TestTypeCd.InList("ONOFF,FF2LBAS"))
					{
						if (cDBConvert.ToDate(CurrentTest["begin_date"], DateTypes.START) > cDBConvert.ToDate(CurrentTest["end_date"], DateTypes.END) ||
							(cDBConvert.ToDate(CurrentTest["begin_date"], DateTypes.START) == cDBConvert.ToDate(CurrentTest["end_date"], DateTypes.END) &&
							 cDBConvert.ToInteger(CurrentTest["begin_hour"]) >= cDBConvert.ToInteger(CurrentTest["end_hour"])))
						{
							Category.SetCheckParameter("Test_Dates_Consistent", false, eParameterDataType.Boolean);
							Category.CheckCatalogResult = "A";
						}
						else
							Category.SetCheckParameter("Test_Dates_Consistent", true, eParameterDataType.Boolean);
					}
					else
						if ((bool)Category.GetCheckParameter("Test_Begin_Minute_Valid").ParameterValue &&
							(bool)Category.GetCheckParameter("Test_End_Minute_Valid").ParameterValue)
							if (cDBConvert.ToDate(CurrentTest["begin_date"], DateTypes.START) > cDBConvert.ToDate(CurrentTest["end_date"], DateTypes.END) ||
								(cDBConvert.ToDate(CurrentTest["begin_date"], DateTypes.START) == cDBConvert.ToDate(CurrentTest["end_date"], DateTypes.END) && cDBConvert.ToInteger(CurrentTest["begin_hour"]) > cDBConvert.ToInteger(CurrentTest["end_hour"])) ||
								(cDBConvert.ToDate(CurrentTest["begin_date"], DateTypes.START) == cDBConvert.ToDate(CurrentTest["end_date"], DateTypes.END) &&
								cDBConvert.ToInteger(CurrentTest["begin_hour"]) == cDBConvert.ToInteger(CurrentTest["end_hour"]) &&
								cDBConvert.ToInteger(CurrentTest["begin_min"]) >= cDBConvert.ToInteger(CurrentTest["end_min"])))
							{
								Category.SetCheckParameter("Test_Dates_Consistent", false, eParameterDataType.Boolean);
								Category.CheckCatalogResult = "A";
							}
							else
								Category.SetCheckParameter("Test_Dates_Consistent", true, eParameterDataType.Boolean);
				}
				else
					Category.SetCheckParameter("Test_Dates_Consistent", false, eParameterDataType.Boolean);
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "TEST7");
			}

			return ReturnVal;
		}

		public static string TEST8(cCategory Category, ref bool Log) //Validate Span Scale
		{
			string ReturnVal = "";

			try
			{

				DataRowView CurrentTest = (DataRowView)Category.GetCheckParameter("Current_Test").ParameterValue;
				string CompTypeCd = "";
				string ComponentID = "";
				if (Category.GetCheckParameter("Current_Component").ParameterValue != null)
				{
					DataRowView CurrentComponent = (DataRowView)Category.GetCheckParameter("Current_Component").ParameterValue;
					ComponentID = cDBConvert.ToString(CurrentComponent["COMPONENT_ID"]);
					CompTypeCd = cDBConvert.ToString(CurrentComponent["COMPONENT_TYPE_CD"]);
				}
				Category.SetCheckParameter("Test_Span_Scale_Valid", true, eParameterDataType.Boolean);
				Category.SetCheckParameter("Test_Span_Value", null, eParameterDataType.Decimal);
				if (ComponentID != "")
				{
					string Scale = cDBConvert.ToString(CurrentTest["span_scale_cd"]);
					string OtherScale = "";

					if (CompTypeCd != "FLOW")
					{
						if (Scale == "")
						{
							Category.SetCheckParameter("Test_Span_Scale_Valid", false, eParameterDataType.Boolean);
							Category.CheckCatalogResult = "A";
						}
						else
						{
							if (!(Scale.InList("H,L")))
							{
								Category.SetCheckParameter("Test_Span_Scale_Valid", false, eParameterDataType.Boolean);
								Category.CheckCatalogResult = "B";
							}
							else
							{
								if ((bool)Category.GetCheckParameter("Test_Dates_Consistent").ParameterValue == true)
								{

									DateTime BeginDate = cDBConvert.ToDate(CurrentTest["begin_date"], DateTypes.START);
									int BeginHour = cDBConvert.ToInteger(CurrentTest["begin_hour"]);
									DateTime EndDate = cDBConvert.ToDate(CurrentTest["end_date"], DateTypes.END);
									int EndHour = cDBConvert.ToInteger(CurrentTest["end_hour"]);

									DataView AnalyzerRangeRecords = (DataView)Category.GetCheckParameter("Analyzer_Range_Records").ParameterValue;

									string OldFilter = AnalyzerRangeRecords.RowFilter;

									if (Scale == "H")
										OtherScale = "L";

									else
										OtherScale = "H";

									AnalyzerRangeRecords.RowFilter = AddToDataViewFilter(OldFilter,
												"analyzer_range_cd = '" + OtherScale +
												"' and (Begin_date < '" + BeginDate.ToShortDateString() + "' " +
												"or (Begin_date = '" + BeginDate.ToShortDateString() + "' " +
												"and Begin_hour <= " + BeginHour + ")) " +
												"and (end_date is null or end_date > '" + EndDate.ToShortDateString() + "' " +
												"or (end_date = '" + EndDate.ToShortDateString() + "' " +
												"and end_hour > " + EndHour + "))");

									if (AnalyzerRangeRecords.Count > 0)
									{
										Category.SetCheckParameter("Test_Span_Scale_Valid", false, eParameterDataType.Boolean);
										Category.CheckCatalogResult = "C";
									}

									AnalyzerRangeRecords.RowFilter = OldFilter;
								}
							}
						}
					}
					else
					{
						if (Scale != "")
						{
							Category.SetCheckParameter("Test_Span_Scale_Valid", false, eParameterDataType.Boolean);
							Category.CheckCatalogResult = "D";
						}
					}
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "TEST8");
			}

			return ReturnVal;
		}

		public static string TEST9(cCategory Category, ref bool Log) //Validate Span Value
		{
			string ReturnVal = "";

			try
			{

				DataRowView CurrentTest = (DataRowView)Category.GetCheckParameter("Current_Test").ParameterValue;

				Boolean ScaleValid = cDBConvert.ToBoolean(Category.GetCheckParameter("Test_Span_Scale_Valid").ParameterValue);

				if (ScaleValid && cDBConvert.ToBoolean(Category.GetCheckParameter("Test_Dates_Consistent").ParameterValue) == true)
				{
					Category.SetCheckParameter("Test_Span_Determined", true, eParameterDataType.Boolean);
					DataView SystemComponentRecords = (DataView)Category.GetCheckParameter("System_Component_Records").ParameterValue;
					SystemComponentRecords.Sort = "BEGIN_DATE";
					if (SystemComponentRecords.Count > 0)
					{
						DateTime BeginDate = cDBConvert.ToDate(CurrentTest["begin_date"], DateTypes.START);
						int BeginHour = cDBConvert.ToInteger(CurrentTest["begin_hour"]);
						DateTime EndDate = cDBConvert.ToDate(CurrentTest["end_date"], DateTypes.END);
						int EndHour = cDBConvert.ToInteger(CurrentTest["end_hour"]);
						string ComponentTypeCd = cDBConvert.ToString(CurrentTest["component_type_cd"]);
						string Scale = cDBConvert.ToString(CurrentTest["span_scale_cd"]);

						DateTime CompBeginDate = cDBConvert.ToDate(SystemComponentRecords[0]["BEGIN_DATE"], DateTypes.START);
						int CompBeginHour = cDBConvert.ToInteger(SystemComponentRecords[0]["BEGIN_HOUR"]);
						if (CompBeginDate != DateTime.MinValue && CompBeginHour >= 0 && CompBeginHour <= 23 &&
							(CompBeginDate > BeginDate || (CompBeginDate == BeginDate && CompBeginHour > BeginHour)))
						{
							BeginDate = CompBeginDate;
							BeginHour = CompBeginHour;
							EndDate = CompBeginDate;
							EndHour = CompBeginHour;
						}

						DataView SpanRecords = (DataView)Category.GetCheckParameter("Span_Records").ParameterValue;

						string OldFilter = SpanRecords.RowFilter;

						if (Scale == "")
							SpanRecords.RowFilter = AddToDataViewFilter(OldFilter,
								"component_type_cd = '" + ComponentTypeCd + "' and span_scale_cd is null and span_value is not null and span_value > 0 " +
								"and (Begin_date < '" + EndDate.ToShortDateString() + "' " +
								"or (Begin_date = '" + EndDate.ToShortDateString() + "' " +
								"and Begin_hour <= " + EndHour + ")) " +
								"and (end_date is null or end_date > '" + BeginDate.ToShortDateString() + "' " +
								"or (end_date = '" + BeginDate.ToShortDateString() + "' " +
								"and end_hour > " + BeginHour + "))");
						else
							SpanRecords.RowFilter = AddToDataViewFilter(OldFilter,
								"component_type_cd = '" + ComponentTypeCd + "' and span_scale_cd = '" + Scale +
								"' and span_value is not null and span_value > 0 " +
								"and (Begin_date < '" + EndDate.ToShortDateString() + "' " +
								"or (Begin_date = '" + EndDate.ToShortDateString() + "' " +
								"and Begin_hour <= " + EndHour + ")) " +
								"and (end_date is null or end_date > '" + BeginDate.ToShortDateString() + "' " +
								"or (end_date = '" + BeginDate.ToShortDateString() + "' " +
								"and end_hour > " + BeginHour + "))");


						if (SpanRecords.Count == 0)
							Category.CheckCatalogResult = "A";

						else
						{
							decimal SpanValue = cDBConvert.ToDecimal(SpanRecords[0]["span_value"]);
							Category.SetCheckParameter("Test_Span_Value", SpanValue, eParameterDataType.Decimal);
							if (SpanRecords.Count > 1)
							{
								for (int i = 1; i < SpanRecords.Count; i++)
								{
									if (cDBConvert.ToDecimal(SpanRecords[i]["span_value"]) != SpanValue)
									{
										Category.CheckCatalogResult = "B";
										Category.SetCheckParameter("Test_Span_Value", null, eParameterDataType.Decimal);
										break;
									}
								}
							}
						}
						SpanRecords.RowFilter = OldFilter;
					}
					else
						Category.CheckCatalogResult = "C";
				}
				else
					Log = false;
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "TEST9");
			}

			return ReturnVal;
		}

		public static string TEST10(cCategory Category, ref bool Log)
		//Test Year Valid
		{
			string ReturnVal = "";

			try
			{
				Category.SetCheckParameter("Test_Reporting_Period_Valid", true, eParameterDataType.Boolean);
				Category.SetCheckParameter("Test_Reporting_Period_Begin_Date", null, eParameterDataType.Date);
				DataRowView CurrentTest = (DataRowView)Category.GetCheckParameter("Current_Test").ParameterValue;
				int RptPerID = cDBConvert.ToInteger(CurrentTest["RPT_PERIOD_ID"]);
				if (RptPerID == int.MinValue)
				{
					Category.SetCheckParameter("Test_Reporting_Period_Valid", false, eParameterDataType.Boolean);
					Category.CheckCatalogResult = "A";
				}
				else
				{
					DataView ReportingPeriodLookup = (DataView)Category.GetCheckParameter("Reporting_Period_Lookup_Table").ParameterValue;
					string OldFilter = ReportingPeriodLookup.RowFilter;
					ReportingPeriodLookup.RowFilter = AddToDataViewFilter(OldFilter, "RPT_PERIOD_ID = " + RptPerID);
					int TestYear = cDBConvert.ToInteger(ReportingPeriodLookup[0]["CALENDAR_YEAR"]);
					int TestQuarter = cDBConvert.ToInteger(ReportingPeriodLookup[0]["QUARTER"]);
					int QuarterFirstMonth = TestQuarter * 3 - 2;
					DateTime QuarterFirstDate = new DateTime(TestYear, QuarterFirstMonth, 1);
					Category.SetCheckParameter("Test_Reporting_Period_Begin_Date", QuarterFirstDate, eParameterDataType.Date);
					ReportingPeriodLookup.RowFilter = OldFilter;
					int CurrentYear = DateTime.Now.Year;
					int CurrentQuarter = cDateFunctions.ThisQuarter(DateTime.Now);
					if (TestYear > CurrentYear || (TestYear == CurrentYear && TestQuarter > CurrentQuarter))
					{
						Category.SetCheckParameter("Test_Reporting_Period_Valid", false, eParameterDataType.Boolean);
						Category.CheckCatalogResult = "B";
					}
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "TEST10");
			}

			return ReturnVal;
		}

		#endregion

		#region Test Checks 11-20

		public static string TEST11(cCategory Category, ref bool Log)
		//Test Quarter Valid
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentTest = (DataRowView)Category.GetCheckParameter("Current_Test").ParameterValue;
				int Quarter = cDBConvert.ToInteger(CurrentTest["QUARTER"]);
				if (Quarter == int.MinValue)
					Category.CheckCatalogResult = "A";
				else
					if (Quarter < 1 || 4 < Quarter)
						Category.CheckCatalogResult = "B";
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "TEST11");
			}

			return ReturnVal;
		}


		public static string TEST12(cCategory Category, ref bool Log)
		//Miscellaneous Test Type Code Valid
		{
			string ReturnVal = "";

			try
			{
				Category.SetCheckParameter("Miscellaneous_Test_Type_Valid", true, eParameterDataType.Boolean);

				DataRowView CurrentTest = (DataRowView)Category.GetCheckParameter("Current_Test").ParameterValue;
				string TestTypeCd = cDBConvert.ToString(CurrentTest["TEST_TYPE_CD"]);
				if (TestTypeCd == "")
					Category.CheckCatalogResult = "A";
				else
				{
					if (!TestTypeCd.InList("LEAK,PEI,DAHS,PEMSACC,OTHER,DGFMCAL,MFMCAL,TSCAL,BCAL,QGA"))
					{
						Category.SetCheckParameter("Miscellaneous_Test_Type_Valid", false, eParameterDataType.Boolean);
						Category.CheckCatalogResult = "B";
					}
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "TEST12");
			}

			return ReturnVal;
		}


		public static string TEST13(cCategory Category, ref bool Log)
		// Valid Test Number
		{
			string ReturnVal = "";

			try
			{

				Category.SetCheckParameter("Test_Number_Valid", true, eParameterDataType.Boolean);
				DataRowView CurrentTest = (DataRowView)Category.GetCheckParameter("Current_Test").ParameterValue;
				if (cDBConvert.ToString(CurrentTest["test_num"]) == "")
				{
					Category.SetCheckParameter("Test_Number_Valid", false, eParameterDataType.Boolean);
					Category.CheckCatalogResult = "A";
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "TEST13");
			}

			return ReturnVal;
		}


		public static string TEST14(cCategory Category, ref bool Log)
		//Miscellaneous Test Description Valid
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentTest = (DataRowView)Category.GetCheckParameter("Current_Test").ParameterValue;
				string TestTypeCd = cDBConvert.ToString(CurrentTest["TEST_TYPE_CD"]);
				if (CurrentTest["TEST_DESCRIPTION"] == DBNull.Value)
				{
					if (TestTypeCd == "OTHER")
						Category.CheckCatalogResult = "A";
				}
				else
					if (TestTypeCd != "OTHER")
						Category.CheckCatalogResult = "B";
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "TEST14");
			}

			return ReturnVal;
		}


		public static string TEST15(cCategory Category, ref bool Log)
		//Miscellaneous Test Grace Period Indicator Valid
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentTest = (DataRowView)Category.GetCheckParameter("Current_Test").ParameterValue;
				if (cDBConvert.ToInteger(CurrentTest["GP_IND"]) == 1)
					if (cDBConvert.ToString(CurrentTest["TEST_TYPE_CD"]) != "LEAK")
						Category.CheckCatalogResult = "A";
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "TEST15");
			}

			return ReturnVal;
		}


		public static string TEST16(cCategory Category, ref bool Log)
		//Miscellaneous Test System or Component Valid
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentTest = (DataRowView)Category.GetCheckParameter("Current_Test").ParameterValue;
				string MonSysId = cDBConvert.ToString(CurrentTest["MON_SYS_ID"]);
				string CompId = cDBConvert.ToString(CurrentTest["COMPONENT_ID"]);
				string TestTypeCd = cDBConvert.ToString(CurrentTest["TEST_TYPE_CD"]);
				if (MonSysId != "" && CompId != "")
					Category.CheckCatalogResult = "A";
				else
				{
					if (TestTypeCd.InList("LEAK,PEI,DAHS"))
					{
						if (CompId == "")
						{
							Category.SetCheckParameter("Miscellaneous_Test_ID_Fieldname", "ComponentID", eParameterDataType.String);
							Category.CheckCatalogResult = "B";
						}
					}
					else
						if (TestTypeCd == "PEMSACC")
							if (MonSysId == "")
							{
								Category.SetCheckParameter("Miscellaneous_Test_ID_Fieldname", "MonitoringSystemID", eParameterDataType.String);
								Category.CheckCatalogResult = "B";
							}
					if (MonSysId != "" && string.IsNullOrEmpty(Category.CheckCatalogResult))
					{
						if (TestTypeCd == "PEMSACC")
						{
							DataView MonSysRecs = (DataView)Category.GetCheckParameter("Monitor_System_Records").ParameterValue;
							string OldFilter = MonSysRecs.RowFilter;
							MonSysRecs.RowFilter = AddToDataViewFilter(OldFilter, "MON_SYS_ID = '" + MonSysId + "'");
							if (cDBConvert.ToString(MonSysRecs[0]["SYS_TYPE_CD"]) != "NOXP")
							{
								Category.SetCheckParameter("Miscellaneous_Test_ID_Fieldname", "monitoring system", eParameterDataType.String);
								Category.CheckCatalogResult = "C";
							}
							MonSysRecs.RowFilter = OldFilter;
						}
					}
					if (CompId != "" && string.IsNullOrEmpty(Category.CheckCatalogResult))
					{
						DataView CompRecs = (DataView)Category.GetCheckParameter("Component_Records").ParameterValue;
						string OldFilter = CompRecs.RowFilter;
						CompRecs.RowFilter = AddToDataViewFilter(OldFilter, "COMPONENT_ID = '" + CompId + "'");
						string RecordCompTypeCd = cDBConvert.ToString(CompRecs[0]["COMPONENT_TYPE_CD"]);
						string RecordAcqCd = cDBConvert.ToString(CompRecs[0]["ACQ_CD"]);
						CompRecs.RowFilter = OldFilter;
						if (TestTypeCd == "DAHS")
						{
							if (RecordCompTypeCd != "DAHS")
							{
								Category.SetCheckParameter("Miscellaneous_Test_ID_Fieldname", "component", eParameterDataType.String);
								Category.CheckCatalogResult = "C";
							}
						}
						else if (TestTypeCd == "LEAK")
						{
							if (RecordCompTypeCd != "FLOW")
							{
								Category.SetCheckParameter("Miscellaneous_Test_ID_Fieldname", "component", eParameterDataType.String);
								Category.CheckCatalogResult = "C";
							}
							else
								if (!RecordAcqCd.InList("DP,O"))
									Category.CheckCatalogResult = "D";
						}
						else if (TestTypeCd == "PEI")
						{
							if (!RecordCompTypeCd.InList("OFFM,GFFM"))
							{
								Category.SetCheckParameter("Miscellaneous_Test_ID_Fieldname", "component", eParameterDataType.String);
								Category.CheckCatalogResult = "C";
							}
						}
						else if (TestTypeCd.InList("DGFMTCAL,MFMCAL,TSCAL,BCAL"))
						{
							if (RecordCompTypeCd != "STRAIN")
							{
								Category.SetCheckParameter("Miscellaneous_Test_ID_Fieldname", "component", eParameterDataType.String);
								Category.CheckCatalogResult = "C";
							}
						}
						else if (TestTypeCd == "QGA")
						{
							if (RecordCompTypeCd.NotInList("HCL,HF"))
							{
								Category.SetCheckParameter("Miscellaneous_Test_ID_Fieldname", "component", eParameterDataType.String);
								Category.CheckCatalogResult = "C";
							}
						}
					}
					if (string.IsNullOrEmpty(MonSysId) && string.IsNullOrEmpty(CompId))
					{
						Category.CheckCatalogResult = "E";
					}
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "TEST16");
			}

			return ReturnVal;
		}


		public static string TEST17(cCategory Category, ref bool Log)
		//Miscellaneous Test Reason Code Valid
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentTest = (DataRowView)Category.GetCheckParameter("Current_Test").ParameterValue;
				string TestReasCd = cDBConvert.ToString(CurrentTest["TEST_REASON_CD"]);
				if (TestReasCd == "")
					Category.CheckCatalogResult = "A";
				else
				{
					DataView TestReasonCodeRecords = (DataView)Category.GetCheckParameter("Test_Reason_Code_Lookup_Table").ParameterValue;
					if (!LookupCodeExists(TestReasCd, TestReasonCodeRecords))
						Category.CheckCatalogResult = "B";
					else
					{
						string TestTypeCd = cDBConvert.ToString(CurrentTest["TEST_TYPE_CD"]);
						if ((TestTypeCd == "LEAK" && TestReasCd.InList("RECERT,INITIAL")) || (TestTypeCd == "DAHS" && TestReasCd == "QA"))
							Category.CheckCatalogResult = "C";
					}
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "TEST17");
			}

			return ReturnVal;
		}


		public static string TEST18(cCategory Category, ref bool Log)
		//Miscellaneous Test Result Code Valid
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentTest = (DataRowView)Category.GetCheckParameter("Current_Test").ParameterValue;
				string TestResCd = cDBConvert.ToString(CurrentTest["TEST_RESULT_CD"]);
				if (TestResCd == "")
					Category.CheckCatalogResult = "A";
				else
					if (!TestResCd.InList("PASSED,FAILED,ABORTED"))
					{
						DataView TestResultLookup = (DataView)Category.GetCheckParameter("Test_Result_Code_Lookup_Table").ParameterValue;
						if (!LookupCodeExists(TestResCd, TestResultLookup))
							Category.CheckCatalogResult = "B";
						else
							Category.CheckCatalogResult = "C";
					}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "TEST18");
			}

			return ReturnVal;
		}


		public static string TEST19(cCategory Category, ref bool Log)
		//Duplicate Miscellaneous Test
		{
			string ReturnVal = "";

			try
			{
				if (Convert.ToBoolean(Category.GetCheckParameter("Test_Number_Valid").ParameterValue))
				{
					DataRowView CurrentTest = (DataRowView)Category.GetCheckParameter("Current_Test").ParameterValue;
					DataView TestRecs = (DataView)Category.GetCheckParameter("Location_Test_Records").ParameterValue;
					string OldFilter = TestRecs.RowFilter;
					string TestTypeCd = cDBConvert.ToString(CurrentTest["TEST_TYPE_CD"]);
					string TestNum = cDBConvert.ToString(CurrentTest["TEST_NUM"]);
					TestRecs.RowFilter = AddToDataViewFilter(OldFilter, "TEST_TYPE_CD = '" + TestTypeCd + "'" + " AND TEST_NUM = '" + TestNum + "'");
					if ((TestRecs.Count > 0 && CurrentTest["TEST_SUM_ID"] == DBNull.Value) ||
						(TestRecs.Count > 1 && CurrentTest["TEST_SUM_ID"] != DBNull.Value) ||
						(TestRecs.Count == 1 && CurrentTest["TEST_SUM_ID"] != DBNull.Value && CurrentTest["TEST_SUM_ID"].ToString() != TestRecs[0]["TEST_SUM_ID"].ToString()))
					{
						Category.SetCheckParameter("Duplicate_Miscellaneous_Test", true, eParameterDataType.Boolean);
						Category.CheckCatalogResult = "A";
					}
					else
					{
						string TestSumID = cDBConvert.ToString(CurrentTest["TEST_SUM_ID"]);
						if (TestSumID != "")
						{
							DataView QASuppRecords = (DataView)Category.GetCheckParameter("QA_Supplemental_Data_Records").ParameterValue;
							string OldFilter2 = QASuppRecords.RowFilter;
							QASuppRecords.RowFilter = AddToDataViewFilter(OldFilter2, "TEST_NUM = '" + TestNum + "' AND TEST_TYPE_CD = '" + TestTypeCd + "'");
							if (QASuppRecords.Count > 0 && cDBConvert.ToString(QASuppRecords[0]["TEST_SUM_ID"]) != TestSumID)
							{
								Category.SetCheckParameter("Duplicate_Miscellaneous_Test", true, eParameterDataType.Boolean);
								Category.CheckCatalogResult = "B";
							}
							else
								Category.SetCheckParameter("Duplicate_Miscellaneous_Test", false, eParameterDataType.Boolean);
							QASuppRecords.RowFilter = OldFilter2;
						}
						else
							Category.SetCheckParameter("Duplicate_Miscellaneous_Test", false, eParameterDataType.Boolean);
					}
					TestRecs.RowFilter = OldFilter;
				}
				else
					Log = false;
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "TEST19");
			}

			return ReturnVal;
		}


		public static string TEST20(cCategory Category, ref bool Log)
		//Identification of Previously Reported Test or Test Number for Miscellaneous Test
		{
			string ReturnVal = "";

			try
			{
				if (Convert.ToBoolean(Category.GetCheckParameter("Test_End_Date_Valid").ParameterValue) &&
					Convert.ToBoolean(Category.GetCheckParameter("Test_End_Hour_Valid").ParameterValue) &&
					Convert.ToBoolean(Category.GetCheckParameter("Test_End_Minute_Valid").ParameterValue))
				{
					DataRowView CurrentTest = (DataRowView)Category.GetCheckParameter("Current_Test").ParameterValue;
					string TestTypeCd = cDBConvert.ToString(CurrentTest["TEST_TYPE_CD"]);
					string MonSysID = cDBConvert.ToString(CurrentTest["MON_SYS_ID"]);
					string ComponentID = cDBConvert.ToString(CurrentTest["COMPONENT_ID"]);
					DateTime EndDate = cDBConvert.ToDate(CurrentTest["END_DATE"], DateTypes.END);
					int EndHour = cDBConvert.ToInteger(CurrentTest["END_HOUR"]);
					int EndMin = cDBConvert.ToInteger(CurrentTest["END_MIN"]);
					DataView TestRecords = (DataView)Category.GetCheckParameter("Location_Test_Records").ParameterValue;
					string OldFilter1 = TestRecords.RowFilter;
					if (EndMin != int.MinValue)
					{
						if (MonSysID == "" && ComponentID != "")
							TestRecords.RowFilter = AddToDataViewFilter(OldFilter1, "TEST_TYPE_CD = '" + TestTypeCd + "'" + " AND MON_SYS_ID IS NULL AND COMPONENT_ID = '" + ComponentID + "'" +
								" AND END_DATE = '" + EndDate.ToShortDateString() + "'" + " AND END_HOUR = " + EndHour + " AND END_MIN = " + EndMin);
						else
							if (MonSysID != "" && ComponentID == "")
								TestRecords.RowFilter = AddToDataViewFilter(OldFilter1, "TEST_TYPE_CD = '" + TestTypeCd + "'" + " AND MON_SYS_ID = '" + MonSysID + "'" + " AND COMPONENT_ID IS NULL AND END_DATE = '" +
									EndDate.ToShortDateString() + "'" + " AND END_HOUR = " + EndHour + " AND END_MIN = " + EndMin);
							else
								if (MonSysID != "" && ComponentID != "")
									TestRecords.RowFilter = AddToDataViewFilter(OldFilter1, "TEST_TYPE_CD = '" + TestTypeCd + "'" + " AND MON_SYS_ID = '" + MonSysID + "'" + " AND COMPONENT_ID = '" + ComponentID + "'" +
										" AND END_DATE = '" + EndDate.ToShortDateString() + "'" + " AND END_HOUR = " + EndHour + " AND END_MIN = " + EndMin);
								else
									TestRecords.RowFilter = AddToDataViewFilter(OldFilter1, "TEST_TYPE_CD = '" + TestTypeCd + "'" + " AND MON_SYS_ID IS NULL AND COMPONENT_ID IS NULL AND END_DATE = '" +
										EndDate.ToShortDateString() + "'" + " AND END_HOUR = " + EndHour + " AND END_MIN = " + EndMin);
					}
					else
					{
						if (MonSysID == "" && ComponentID != "")
							TestRecords.RowFilter = AddToDataViewFilter(OldFilter1, "TEST_TYPE_CD = '" + TestTypeCd + "'" + " AND MON_SYS_ID IS NULL AND COMPONENT_ID = '" + ComponentID + "'" +
								" AND END_DATE = '" + EndDate.ToShortDateString() + "'" + " AND END_HOUR = " + EndHour + " AND END_MIN IS NULL");
						else
							if (MonSysID != "" && ComponentID == "")
								TestRecords.RowFilter = AddToDataViewFilter(OldFilter1, "TEST_TYPE_CD = '" + TestTypeCd + "'" + " AND MON_SYS_ID = '" + MonSysID + "'" + " AND COMPONENT_ID IS NULL AND END_DATE = '" +
									EndDate.ToShortDateString() + "'" + " AND END_HOUR = " + EndHour + " AND END_MIN IS NULL");
							else
								if (MonSysID != "" && ComponentID != "")
									TestRecords.RowFilter = AddToDataViewFilter(OldFilter1, "TEST_TYPE_CD = '" + TestTypeCd + "'" + " AND MON_SYS_ID = '" + MonSysID + "'" + " AND COMPONENT_ID = '" + ComponentID + "'" +
										" AND END_DATE = '" + EndDate.ToShortDateString() + "'" + " AND END_HOUR = " + EndHour + " AND END_MIN IS NULL");
								else
									TestRecords.RowFilter = AddToDataViewFilter(OldFilter1, "TEST_TYPE_CD = '" + TestTypeCd + "'" + " AND MON_SYS_ID IS NULL AND COMPONENT_ID IS NULL AND END_DATE = '" +
										EndDate.ToShortDateString() + "'" + " AND END_HOUR = " + EndHour + " AND END_MIN IS NULL");
					}
					if ((TestRecords.Count > 0 && CurrentTest["TEST_SUM_ID"] == DBNull.Value) ||
						(TestRecords.Count > 1 && CurrentTest["TEST_SUM_ID"] != DBNull.Value) ||
						(TestRecords.Count == 1 && CurrentTest["TEST_SUM_ID"] != DBNull.Value && CurrentTest["TEST_SUM_ID"].ToString() != TestRecords[0]["TEST_SUM_ID"].ToString()))
						Category.CheckCatalogResult = "A";
					else
					{
						DataView QASuppRecs = (DataView)Category.GetCheckParameter("QA_Supplemental_Data_Records").ParameterValue;
						string OldFilter2 = QASuppRecs.RowFilter;
						string TestNumber = cDBConvert.ToString(CurrentTest["TEST_NUM"]);

						if (EndMin != int.MinValue)
						{
							if (MonSysID == "" && ComponentID != "")
								QASuppRecs.RowFilter = AddToDataViewFilter(OldFilter2, "TEST_TYPE_CD = '" + TestTypeCd + "'" + " AND MON_SYS_ID IS NULL AND COMPONENT_ID = '" +
									ComponentID + "'" + " AND TEST_NUM <> '" + TestNumber + "'" + " AND END_DATE = '" + EndDate.ToShortDateString() + "'" + " AND END_HOUR = " +
									EndHour + " AND (END_MIN IS NULL OR END_MIN = " + EndMin + ")");
							else
								if (MonSysID != "" && ComponentID == "")
									QASuppRecs.RowFilter = AddToDataViewFilter(OldFilter2, "TEST_TYPE_CD = '" + TestTypeCd + "'" + " AND MON_SYS_ID = '" + MonSysID + "'" +
										" AND COMPONENT_ID IS NULL AND TEST_NUM <> '" + TestNumber + "'" + " AND END_DATE = '" +
										EndDate.ToShortDateString() + "'" + " AND END_HOUR = " + EndHour + " AND (END_MIN IS NULL OR END_MIN = " + EndMin + ")");
								else
									if (MonSysID != "" && ComponentID != "")
										QASuppRecs.RowFilter = AddToDataViewFilter(OldFilter2, "TEST_TYPE_CD = '" + TestTypeCd + "'" + " AND MON_SYS_ID = '" + MonSysID + "'" +
											" AND COMPONENT_ID = '" + ComponentID + "'" + " AND TEST_NUM <> '" + TestNumber + "'" + " AND END_DATE = '" +
											EndDate.ToShortDateString() + "'" + " AND END_HOUR = " + EndHour + " AND (END_MIN IS NULL OR END_MIN = " + EndMin + ")");
									else
										QASuppRecs.RowFilter = AddToDataViewFilter(OldFilter2, "TEST_TYPE_CD = '" + TestTypeCd + "'" + " AND MON_SYS_ID IS NULL AND COMPONENT_ID IS NULL AND TEST_NUM <> '" +
											TestNumber + "'" + " AND END_DATE = '" + EndDate.ToShortDateString() + "'" + " AND END_HOUR = " + EndHour + " AND (END_MIN IS NULL OR END_MIN = " + EndMin + ")");
						}
						else
						{
							if (MonSysID == "" && ComponentID != "")
								QASuppRecs.RowFilter = AddToDataViewFilter(OldFilter2, "TEST_TYPE_CD = '" + TestTypeCd + "'" + " AND MON_SYS_ID IS NULL AND COMPONENT_ID = '" +
									ComponentID + "'" + " AND TEST_NUM <> '" + TestNumber + "'" + " AND END_DATE = '" + EndDate.ToShortDateString() + "'" + " AND END_HOUR = " +
									EndHour + " AND END_MIN IS NULL");
							else
								if (MonSysID != "" && ComponentID == "")
									QASuppRecs.RowFilter = AddToDataViewFilter(OldFilter2, "TEST_TYPE_CD = '" + TestTypeCd + "'" + " AND MON_SYS_ID = '" + MonSysID + "'" +
										" AND COMPONENT_ID IS NULL AND TEST_NUM <> '" + TestNumber + "'" + " AND END_DATE = '" +
										EndDate.ToShortDateString() + "'" + " AND END_HOUR = " + EndHour + " AND END_MIN IS NULL");
								else
									if (MonSysID != "" && ComponentID != "")
										QASuppRecs.RowFilter = AddToDataViewFilter(OldFilter2, "TEST_TYPE_CD = '" + TestTypeCd + "'" + " AND MON_SYS_ID = '" + MonSysID + "'" +
											" AND COMPONENT_ID = '" + ComponentID + "'" + " AND TEST_NUM <> '" + TestNumber + "'" + " AND END_DATE = '" +
											EndDate.ToShortDateString() + "'" + " AND END_HOUR = " + EndHour + " AND END_MIN IS NULL");
									else
										QASuppRecs.RowFilter = AddToDataViewFilter(OldFilter2, "TEST_TYPE_CD = '" + TestTypeCd + "'" + " AND MON_SYS_ID IS NULL AND COMPONENT_ID IS NULL AND TEST_NUM <> '" +
											TestNumber + "'" + " AND END_DATE = '" + EndDate.ToShortDateString() + "'" + " AND END_HOUR = " + EndHour + " AND END_MIN IS NULL");
						}

						if ((QASuppRecs.Count > 0 && CurrentTest["TEST_SUM_ID"] == DBNull.Value) ||
							(QASuppRecs.Count > 1 && CurrentTest["TEST_SUM_ID"] != DBNull.Value) ||
							(QASuppRecs.Count == 1 && CurrentTest["TEST_SUM_ID"] != DBNull.Value && CurrentTest["TEST_SUM_ID"].ToString() != QASuppRecs[0]["TEST_SUM_ID"].ToString()))
							Category.CheckCatalogResult = "A";
						else
						{
							QASuppRecs.RowFilter = AddToDataViewFilter(OldFilter2, "TEST_TYPE_CD = '" + TestTypeCd + "'" + " AND TEST_NUM = '" + TestNumber + "'");
							if (QASuppRecs.Count > 0)
							{
								if (cDBConvert.ToString(((DataRowView)QASuppRecs[0])["CAN_SUBMIT"]) == "N")
								{
									if (MonSysID == "" && ComponentID != "")
										QASuppRecs.RowFilter = AddToDataViewFilter(QASuppRecs.RowFilter, "COMPONENT_ID <> '" + ComponentID + "' AND MON_SYS_ID IS NOT NULL" +
											" OR END_DATE <> '" + EndDate.ToShortDateString() + "'" + " OR END_HOUR <> " + EndHour + " OR END_MIN <> " + EndMin);
									else
										if (MonSysID != "" && ComponentID == "")
											QASuppRecs.RowFilter = AddToDataViewFilter(QASuppRecs.RowFilter, "COMPONENT_ID IS NOT NULL" + " AND MON_SYS_ID <> '" + MonSysID +
												"' OR END_DATE <> '" + EndDate.ToShortDateString() + "'" + " OR END_HOUR <> " + EndHour + " OR END_MIN <> " + EndMin);
										else
											if (MonSysID != "" && ComponentID != "")
												QASuppRecs.RowFilter = AddToDataViewFilter(QASuppRecs.RowFilter, "COMPONENT_ID <> '" + ComponentID + "' AND MON_SYS_ID <> '" + MonSysID +
													"' OR END_DATE <> '" + EndDate.ToShortDateString() + "'" + " OR END_HOUR <> " + EndHour + " OR END_MIN <> " + EndMin);
											else
												QASuppRecs.RowFilter = AddToDataViewFilter(QASuppRecs.RowFilter, "COMPONENT_ID IS NOT NULL OR MON_SYS_ID IS NOT NULL" +
													" OR END_DATE <> '" + EndDate.ToShortDateString() + "'" + " OR END_HOUR <> " + EndHour + " OR END_MIN <> " + EndMin);
									if ((QASuppRecs.Count > 0 && CurrentTest["TEST_SUM_ID"] == DBNull.Value) ||
										(QASuppRecs.Count > 1 && CurrentTest["TEST_SUM_ID"] != DBNull.Value) ||
										(QASuppRecs.Count == 1 && CurrentTest["TEST_SUM_ID"] != DBNull.Value && CurrentTest["TEST_SUM_ID"].ToString() != QASuppRecs[0]["TEST_SUM_ID"].ToString()))
										Category.CheckCatalogResult = "B";
									else
										Category.CheckCatalogResult = "C";
								}
							}
						}
						QASuppRecs.RowFilter = OldFilter2;
					}
					TestRecords.RowFilter = OldFilter1;
				}
				else
					Log = false;
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "TEST20");
			}

			return ReturnVal;
		}
		#endregion

		#region Test Checks 21-30

		public static string TEST21(cCategory Category, ref bool Log)
		//Critical Level 2 Message
		{
			string ReturnVal = "";

			try
			{
				Category.CheckCatalogResult = "A";
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "TEST21");
			}

			return ReturnVal;
		}


		public static string TEST22(cCategory Category, ref bool Log)
		//MP Evaluation Check
		{
			string ReturnVal = "";

			try
			{

				DataView MPLocationRecords = (DataView)Category.GetCheckParameter("Monitoring_Plan_Location_Records_for_QA").ParameterValue;
				DataRowView CurrentTest = (DataRowView)Category.GetCheckParameter("Current_Test").ParameterValue;
				DateTime Date = cDBConvert.ToDate(CurrentTest["END_DATE"], DateTypes.END);
				int TestQtr = cDateFunctions.ThisQuarter(Date);
				int TestYr = Date.Year;
				string OldFilter1 = MPLocationRecords.RowFilter;
				MPLocationRecords.RowFilter = AddToDataViewFilter(OldFilter1, "(SEVERITY_CD = 'CRIT1' OR SEVERITY_CD = 'FATAL') AND (END_YEAR IS NULL OR END_QUARTER IS NULL OR END_YEAR > " + TestYr +
						" OR (END_YEAR = " + TestYr + " AND END_QUARTER >= " + TestQtr + "))");
				if (MPLocationRecords.Count > 0)
					Category.CheckCatalogResult = "A";
				else
				{
					MPLocationRecords.RowFilter = AddToDataViewFilter(OldFilter1, "(NEEDS_EVAL_FLG = 'Y' AND MUST_SUBMIT = 'Y') AND (END_YEAR IS NULL OR END_QUARTER IS NULL OR END_YEAR > " + TestYr +
						  " OR (END_YEAR = " + TestYr + " AND END_QUARTER >= " + TestQtr + "))");
					if (MPLocationRecords.Count > 0)
						Category.CheckCatalogResult = "B";
				}
				MPLocationRecords.RowFilter = OldFilter1;
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "TEST22");
			}

			return ReturnVal;
		}

		public static string TEST23(cCategory Category, ref bool Log)
		//Ensures that the Injection Protocol is null when the test is not a 7 Day Calibration Test or a Cycle Time Test, or when the component type is not 'HG'.
		{
			string ReturnVal = "";

			try
			{
				if (QaParameters.CurrentTest.TestTypeCd.InList("7DAY,CYCLE"))
				{
					DataRowView ComponentRecord = cRowFilter.FindRow(QaParameters.ComponentRecords.SourceView, new cFilterCondition[] { new cFilterCondition("COMPONENT_ID", QaParameters.CurrentTest.ComponentId) }
					);

					if (ComponentRecord != null)
					{

						if (ComponentRecord["COMPONENT_TYPE_CD"].ToString() == "HG")
						{
							if (QaParameters.CurrentTest.InjectionProtocolCd == null)
							{
								Category.CheckCatalogResult = "A";
							}
							else if (QaParameters.CurrentTest.InjectionProtocolCd.NotInList("HGE,HGO"))
							{
								Category.CheckCatalogResult = "B";
							}
						}
						else
						{
							if (QaParameters.CurrentTest.InjectionProtocolCd != null)
							{
								Category.CheckCatalogResult = "C";
							}
						}
					}
				}
				else
				{

					if (QaParameters.CurrentTest.InjectionProtocolCd != null)
					{
						Category.CheckCatalogResult = "D";
					}
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "TEST23");
			}

			return ReturnVal;
		}

		
		public static string TEST24(cCategory Category, ref bool Log)
		//Initialize Test Parameters
		{
			string ReturnVal = "";

			try
			{
				if (QaParameters.FirstEcmpsReportingPeriod == null)
				{
					QaParameters.EcmpsMpBeginDate = new DateTime(2009, 1, 1);
				}
				else
				{
					QaParameters.EcmpsMpBeginDate = cDateFunctions.StartDateThisQuarter(QaParameters.FirstEcmpsReportingPeriod.Value);
				}

				QaParameters.ProtocolGases = null;
				QaParameters.ProtocolGasCylinderIdList = null;
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "TEST24");
			}

			return ReturnVal;
		}
		#endregion


	}
}

		#endregion
