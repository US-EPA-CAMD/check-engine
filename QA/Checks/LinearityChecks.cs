using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.Qa.Parameters;

using ECMPS.Checks.TypeUtilities;
using ECMPS.Definitions.Extensions;


namespace ECMPS.Checks.LinearityChecks
{
	public class cLinearityChecks : cChecks
	{
		#region Constructors

		public cLinearityChecks()
		{
			CheckProcedures = new dCheckProcedure[39];

			CheckProcedures[1] = new dCheckProcedure(LINEAR1);
			CheckProcedures[2] = new dCheckProcedure(LINEAR2);
			CheckProcedures[3] = new dCheckProcedure(LINEAR3);
			CheckProcedures[4] = new dCheckProcedure(LINEAR4);
			CheckProcedures[5] = new dCheckProcedure(LINEAR5);
			CheckProcedures[6] = new dCheckProcedure(LINEAR6);
			CheckProcedures[9] = new dCheckProcedure(LINEAR9);
			CheckProcedures[10] = new dCheckProcedure(LINEAR10);
			CheckProcedures[11] = new dCheckProcedure(LINEAR11);
			CheckProcedures[12] = new dCheckProcedure(LINEAR12);
			CheckProcedures[13] = new dCheckProcedure(LINEAR13);
			CheckProcedures[14] = new dCheckProcedure(LINEAR14);
			CheckProcedures[15] = new dCheckProcedure(LINEAR15);
			CheckProcedures[16] = new dCheckProcedure(LINEAR16);
			CheckProcedures[17] = new dCheckProcedure(LINEAR17);
			CheckProcedures[18] = new dCheckProcedure(LINEAR18);
			CheckProcedures[19] = new dCheckProcedure(LINEAR19);
			CheckProcedures[20] = new dCheckProcedure(LINEAR20);
			CheckProcedures[21] = new dCheckProcedure(LINEAR21);
			CheckProcedures[22] = new dCheckProcedure(LINEAR22);
			CheckProcedures[23] = new dCheckProcedure(LINEAR23);
			CheckProcedures[24] = new dCheckProcedure(LINEAR24);
			CheckProcedures[25] = new dCheckProcedure(LINEAR25);
			CheckProcedures[26] = new dCheckProcedure(LINEAR26);
			CheckProcedures[27] = new dCheckProcedure(LINEAR27);
			CheckProcedures[28] = new dCheckProcedure(LINEAR28);
			CheckProcedures[29] = new dCheckProcedure(LINEAR29);
			CheckProcedures[30] = new dCheckProcedure(LINEAR30);
			CheckProcedures[31] = new dCheckProcedure(LINEAR31);
			CheckProcedures[32] = new dCheckProcedure(LINEAR32);
			CheckProcedures[33] = new dCheckProcedure(LINEAR33);
			CheckProcedures[34] = new dCheckProcedure(LINEAR34);
			CheckProcedures[35] = new dCheckProcedure(LINEAR35);
			CheckProcedures[36] = new dCheckProcedure(LINEAR36);
			CheckProcedures[37] = new dCheckProcedure(LINEAR37);
			CheckProcedures[38] = new dCheckProcedure(LINEAR38);
		}


		#endregion


		#region Linearity Checks 1-10

		#region Linearity Check 1
		public static string LINEAR1(cCategory Category, ref bool Log) //Determine Linearity Injection Sequence
		{
			string ReturnVal = "", LastCode = "";
			DateTime LastDate = DateTime.MinValue;
			int LastHour = 0, LastMin = 0;
			Boolean first = true, timesvalid = true;

			try
			{
				DataView InjectionRecords = (DataView)Category.GetCheckParameter("Linearity_Test_Injection_Records").ParameterValue;
				if (InjectionRecords.Count == 0)
					Category.SetCheckParameter("Linearity_Injection_Times_Valid", false, eParameterDataType.Boolean);
				else
				{
					DataRowView CurrentLinearity = (DataRowView)Category.GetCheckParameter("Current_Linearity_Test").ParameterValue;
					Category.SetCheckParameter("Linearity_Injection_Times_Valid", true, eParameterDataType.Boolean);
					Category.SetCheckParameter("Simultaneous_Linearity_Injections", false, eParameterDataType.Boolean);
					Category.SetCheckParameter("Linearity_Sequence_Valid", true, eParameterDataType.Boolean);

					foreach (DataRowView InjectionRecord in InjectionRecords)
					{
						if (LastCode == (string)InjectionRecord["Gas_Level_Cd"])
							Category.SetCheckParameter("Linearity_Sequence_Valid", false, eParameterDataType.Boolean);
						else
							LastCode = (string)InjectionRecord["Gas_Level_Cd"];

						if (timesvalid)
						{

							if (cDBConvert.ToDate(InjectionRecord["Injection_date"], DateTypes.START) == DateTime.MinValue ||
								cDBConvert.ToInteger(InjectionRecord["Injection_hour"]) < 0 || cDBConvert.ToInteger(InjectionRecord["Injection_hour"]) > 23 ||
							  cDBConvert.ToInteger(InjectionRecord["Injection_min"]) < 0 || cDBConvert.ToInteger(InjectionRecord["Injection_min"]) > 59)
							{
								timesvalid = false;
							}
							else
							{
								if (LastDate == cDBConvert.ToDate(InjectionRecord["Injection_Date"], DateTypes.START) &&
									LastHour == cDBConvert.ToInteger(InjectionRecord["Injection_Hour"]) &&
									LastMin == cDBConvert.ToInteger(InjectionRecord["Injection_Min"]))
									Category.SetCheckParameter("Simultaneous_Linearity_Injections", true, eParameterDataType.Boolean);

								else
								{
									LastDate = cDBConvert.ToDate(InjectionRecord["Injection_Date"], DateTypes.START);
									LastHour = cDBConvert.ToInteger(InjectionRecord["Injection_Hour"]);
									LastMin = cDBConvert.ToInteger(InjectionRecord["Injection_Min"]);
								}
							}
						}
						if (first)
						{
							Category.SetCheckParameter("Linearity_Test_Begin_Date", LastDate, eParameterDataType.Date);
							Category.SetCheckParameter("Linearity_Test_Begin_Hour", LastHour, eParameterDataType.Integer);
							Category.SetCheckParameter("Linearity_Test_Begin_Minute", LastMin, eParameterDataType.Integer);
							first = false;
						}
					}

					Category.SetCheckParameter("Linearity_Test_End_Date", LastDate, eParameterDataType.Date);
					Category.SetCheckParameter("Linearity_Test_End_Hour", LastHour, eParameterDataType.Integer);
					Category.SetCheckParameter("Linearity_Test_End_Minute", LastMin, eParameterDataType.Integer);
					Category.SetCheckParameter("Linearity_Injection_Times_Valid", timesvalid, eParameterDataType.Boolean);
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "LINEAR1");
			}

			return ReturnVal;
		}
		#endregion

		#region Linearity Check 2
		public static string LINEAR2(cCategory Category, ref bool Log) //Validate Component
		{
			string ReturnVal = "";

			try
			{

				DataRowView CurrentLinearity = (DataRowView)Category.GetCheckParameter("Current_Linearity_Test").ParameterValue;
				if (cDBConvert.ToString(CurrentLinearity["COMPONENT_ID"]) == "")
				{
					Category.SetCheckParameter("Linearity_Component_Valid", false, eParameterDataType.Boolean);
					Category.CheckCatalogResult = "A";
				}
				else
				{
					string TestTypeCd = cDBConvert.ToString(CurrentLinearity["TEST_TYPE_CD"]);
					string CompTypeCd = cDBConvert.ToString(CurrentLinearity["COMPONENT_TYPE_CD"]);
					if (TestTypeCd == "LINE")
					{
						if (CompTypeCd.InList("SO2,NOX,CO2,O2,HG"))
						{
							Category.SetCheckParameter("Linearity_Test_Type", "linearity check", eParameterDataType.String);
							Category.SetCheckParameter("Linearity_Component_Valid", true, eParameterDataType.Boolean);
						}
						else
						{
							Category.SetCheckParameter("Linearity_Component_Valid", false, eParameterDataType.Boolean);
							Category.CheckCatalogResult = "B";
						}
					}
					else if (TestTypeCd == "HGLINE")
					{
						if (CompTypeCd == "HG")
						{
							Category.SetCheckParameter("Linearity_Test_Type", "Hg linearity check", eParameterDataType.String);
							Category.SetCheckParameter("Linearity_Component_Valid", true, eParameterDataType.Boolean);
						}
						else
						{
							Category.SetCheckParameter("Linearity_Component_Valid", false, eParameterDataType.Boolean);
							Category.CheckCatalogResult = "B";
						}
					}
					else if (TestTypeCd == "HGSI3")
					{
						if (CompTypeCd == "HG")
						{
							Category.SetCheckParameter("Linearity_Test_Type", "three-point system integrity check", eParameterDataType.String);
							Category.SetCheckParameter("Linearity_Component_Valid", true, eParameterDataType.Boolean);
						}
						else
						{
							Category.SetCheckParameter("Linearity_Component_Valid", false, eParameterDataType.Boolean);
							Category.CheckCatalogResult = "C";
						}
					}
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "LINEAR2");
			}

			return ReturnVal;
		}
		#endregion

		#region Linearity Check 3
		public static string LINEAR3(cCategory Category, ref bool Log) // Test Aborted
		{
			string ReturnVal = "";

			try
			{

				DataRowView CurrentLinearity = (DataRowView)Category.GetCheckParameter("Current_Linearity_Test").ParameterValue;

				if (cDBConvert.ToString(CurrentLinearity["Test_Result_Cd"]) == "ABORTED")
				{
					Category.SetCheckParameter("Linearity_Test_Aborted", true, eParameterDataType.Boolean);
					Category.SetCheckParameter("Linearity_Test_Result", "ABORTED", eParameterDataType.String);
					Category.CheckCatalogResult = "A";
				}
				else
					Category.SetCheckParameter("Linearity_Test_Aborted", false, eParameterDataType.Boolean);
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "LINEAR3");
			}

			return ReturnVal;
		}
		#endregion

		#region Linearity Check 4
		public static string LINEAR4(cCategory Category, ref bool Log) // Duplicate Test and Test Number
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentLinearity = (DataRowView)Category.GetCheckParameter("Current_Linearity_Test").ParameterValue;
				string CompID = cDBConvert.ToString(CurrentLinearity["component_id"]);
				if (cDBConvert.ToBoolean(Category.GetCheckParameter("Test_Span_Scale_Valid").ParameterValue) == true &&
					cDBConvert.ToBoolean(Category.GetCheckParameter("Test_End_Date_Valid").ParameterValue) == true &&
					cDBConvert.ToBoolean(Category.GetCheckParameter("Test_End_Hour_Valid").ParameterValue) == true &&
					cDBConvert.ToBoolean(Category.GetCheckParameter("Test_End_Minute_Valid").ParameterValue) == true && CompID != "")
				{
					Category.SetCheckParameter("Linearity_Supp_Data_ID", null, eParameterDataType.String);
					Category.SetCheckParameter("Extra_Linearity_Test", false, eParameterDataType.Boolean);
					DateTime EndDate = cDBConvert.ToDate(CurrentLinearity["end_date"], DateTypes.END);
					int EndHour = cDBConvert.ToInteger(CurrentLinearity["end_hour"]);
					int EndMin = cDBConvert.ToInteger(CurrentLinearity["end_min"]);
					string TestTypeCd = cDBConvert.ToString(CurrentLinearity["test_type_cd"]);
					string Scale = cDBConvert.ToString(CurrentLinearity["span_scale_cd"]);
					string TestNumber = cDBConvert.ToString(CurrentLinearity["test_num"]);
					DataView LinearityTestRecords = (DataView)Category.GetCheckParameter("Linearity_Test_Records").ParameterValue;
					string OldFilter = LinearityTestRecords.RowFilter;
					LinearityTestRecords.RowFilter = AddToDataViewFilter(OldFilter,
					  "test_type_cd = '" + TestTypeCd + "' and span_scale_cd = '" + Scale + "' and test_num <> '" + TestNumber + "' " +
					  "and end_date = '" + EndDate.ToShortDateString() + "' " +
					  "and end_hour = " + EndHour + " " +
					  "and end_Min = " + EndMin);
					if ((LinearityTestRecords.Count > 0 && CurrentLinearity["TEST_SUM_ID"] == DBNull.Value) ||
						(LinearityTestRecords.Count > 1 && CurrentLinearity["TEST_SUM_ID"] != DBNull.Value) ||
						(LinearityTestRecords.Count == 1 && CurrentLinearity["TEST_SUM_ID"] != DBNull.Value && CurrentLinearity["TEST_SUM_ID"].ToString() != LinearityTestRecords[0]["TEST_SUM_ID"].ToString()))
					{
						Category.SetCheckParameter("Extra_Linearity_Test", true, eParameterDataType.Boolean);
						Category.CheckCatalogResult = "A";
						LinearityTestRecords.RowFilter = OldFilter;
					}
					else
					{
						LinearityTestRecords.RowFilter = OldFilter;
						DataView QASuppRecords = (DataView)Category.GetCheckParameter("QA_Supplemental_Data_Records").ParameterValue;
						OldFilter = QASuppRecords.RowFilter;
						QASuppRecords.RowFilter = AddToDataViewFilter(OldFilter,
							"test_type_cd = '" + TestTypeCd + "' and component_id = '" + CompID + "' and span_scale = '" +
							Scale + "' and test_num <> '" + TestNumber + "' " + "and end_date = '" +
							EndDate.ToShortDateString() + "' " + "and end_hour = " + EndHour + " " +
							"and end_Min = " + EndMin);
						if ((QASuppRecords.Count > 0 && CurrentLinearity["TEST_SUM_ID"] == DBNull.Value) ||
							(QASuppRecords.Count > 1 && CurrentLinearity["TEST_SUM_ID"] != DBNull.Value) ||
							(QASuppRecords.Count == 1 && CurrentLinearity["TEST_SUM_ID"] != DBNull.Value && CurrentLinearity["TEST_SUM_ID"].ToString() != QASuppRecords[0]["TEST_SUM_ID"].ToString()))
						{
							Category.SetCheckParameter("Extra_Linearity_Test", true, eParameterDataType.Boolean);
							Category.CheckCatalogResult = "A";
						}
						else
						{
							QASuppRecords.RowFilter = AddToDataViewFilter(OldFilter,
								"test_type_cd = '" + TestTypeCd + "' and test_num = '" + TestNumber + "'");
							if (QASuppRecords.Count > 0)
							{
								Category.SetCheckParameter("Linearity_Supp_Data_ID", cDBConvert.ToString(QASuppRecords[0]["QA_Supp_Data_ID"]), eParameterDataType.String);
								if ((string)QASuppRecords[0]["CAN_SUBMIT"] == "N")
								{
									QASuppRecords.RowFilter = AddToDataViewFilter(QASuppRecords.RowFilter, "COMPONENT_ID <> '" + CompID + "' OR SPAN_SCALE <> '" + Scale + "'" +
										" OR END_DATE <> '" + EndDate.ToShortDateString() + "'" + " OR END_HOUR <> " + EndHour + " OR END_MIN <> " + EndMin);
									if ((QASuppRecords.Count > 0 && CurrentLinearity["TEST_SUM_ID"] == DBNull.Value) ||
										(QASuppRecords.Count > 1 && CurrentLinearity["TEST_SUM_ID"] != DBNull.Value) ||
										(QASuppRecords.Count == 1 && CurrentLinearity["TEST_SUM_ID"] != DBNull.Value && CurrentLinearity["TEST_SUM_ID"].ToString() != QASuppRecords[0]["TEST_SUM_ID"].ToString()))
										Category.CheckCatalogResult = "B";
									else
										Category.CheckCatalogResult = "C";
								}
							}
						}
						QASuppRecords.RowFilter = OldFilter;
					}
				}
				else
					Log = false;
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "LINEAR4");
			}

			return ReturnVal;
		}
		#endregion

		#region Linearity Check 5
		public static string LINEAR5(cCategory Category, ref bool Log) // Validate Test Date
		{
			string ReturnVal = "";

			try
			{

				DataRowView CurrentLinearity = (DataRowView)Category.GetCheckParameter("Current_Linearity_Test").ParameterValue;
				if (cDBConvert.ToBoolean(Category.GetCheckParameter("Linearity_Injection_Times_Valid").ParameterValue) == true &&
					cDBConvert.ToBoolean(Category.GetCheckParameter("Test_Begin_Date_Valid").ParameterValue) == true &&
					cDBConvert.ToBoolean(Category.GetCheckParameter("Test_Begin_Hour_Valid").ParameterValue) == true &&
					cDBConvert.ToBoolean(Category.GetCheckParameter("Test_Begin_Minute_Valid").ParameterValue) == true)
				{
					if (cDBConvert.ToDate(Category.GetCheckParameter("Linearity_Test_Begin_Date").ParameterValue, DateTypes.START) != cDBConvert.ToDate(CurrentLinearity["Begin_Date"], DateTypes.START) ||
						cDBConvert.ToInteger(Category.GetCheckParameter("Linearity_Test_Begin_Hour").ParameterValue) != cDBConvert.ToInteger(CurrentLinearity["Begin_Hour"]) ||
						cDBConvert.ToInteger(Category.GetCheckParameter("Linearity_Test_Begin_Minute").ParameterValue) != cDBConvert.ToInteger(CurrentLinearity["Begin_Min"]))
						Category.CheckCatalogResult = "A";
				}
				else
					Log = false;
			}

			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "LINEAR5");
			}

			return ReturnVal;
		}
		#endregion

		#region Linearity Check 6
		public static string LINEAR6(cCategory Category, ref bool Log) // Validate Test Date
		{
			string ReturnVal = "";

			try
			{

				DataRowView CurrentLinearity = (DataRowView)Category.GetCheckParameter("Current_Linearity_Test").ParameterValue;
				if (cDBConvert.ToBoolean(Category.GetCheckParameter("Linearity_Injection_Times_Valid").ParameterValue) == true &&
					cDBConvert.ToBoolean(Category.GetCheckParameter("Test_End_Date_Valid").ParameterValue) == true &&
					cDBConvert.ToBoolean(Category.GetCheckParameter("Test_End_Hour_Valid").ParameterValue) == true &&
					cDBConvert.ToBoolean(Category.GetCheckParameter("Test_End_Minute_Valid").ParameterValue) == true)
				{
					if (cDBConvert.ToDate(Category.GetCheckParameter("Linearity_Test_End_Date").ParameterValue, DateTypes.START) != cDBConvert.ToDate(CurrentLinearity["End_Date"], DateTypes.START) ||
						cDBConvert.ToInteger(Category.GetCheckParameter("Linearity_Test_End_Hour").ParameterValue) != cDBConvert.ToInteger(CurrentLinearity["End_Hour"]) ||
						cDBConvert.ToInteger(Category.GetCheckParameter("Linearity_Test_End_Minute").ParameterValue) != cDBConvert.ToInteger(CurrentLinearity["End_Min"]))
						Category.CheckCatalogResult = "A";
				}
				else
					Log = false;
			}

			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "LINEAR6");
			}

			return ReturnVal;
		}
		#endregion

		#region Linearity Check 7 (Obsolete)
		//public static string LINEAR7(cCategory Category, ref bool Log) //Validate Span Scale
		//{
		//  string ReturnVal = "";

		//  try
		//  {

		//    DataRowView CurrentLinearity = (DataRowView)Category.GetCheckParameter("Current_Linearity_Test").ParameterValue;
		//    string Scale = cDBConvert.ToString(CurrentLinearity["span_scale_cd"]);
		//    string OtherScale = "";

		//    if (Scale == "") {
		//      Category.SetCheckParameter("Linearity_Scale_Valid", false, ParameterTypes.BOOLEAN);
		//      Category.CheckCatalogResult = "A";
		//    } 
		//    else {
		//      if (!(cList.InList("H,L", Scale))) {
		//        Category.SetCheckParameter("Linearity_Scale_Valid", false, ParameterTypes.BOOLEAN);
		//        Category.CheckCatalogResult = "B";
		//      } 
		//      else {
		//        DateTime BeginDate = cDBConvert.ToDate(Category.GetCheckParameter("Linearity_Test_Begin_Date").ParameterValue, DateTypes.START);
		//        int BeginHour = cDBConvert.ToInteger(Category.GetCheckParameter("Linearity_Test_Begin_Hour").ParameterValue);
		//        DateTime EndDate = cDBConvert.ToDate(Category.GetCheckParameter("Linearity_Test_End_Date").ParameterValue, DateTypes.END);
		//        int EndHour = cDBConvert.ToInteger(Category.GetCheckParameter("Linearity_Test_End_Hour").ParameterValue);

		//        DataView AnalyzerRangeRecords = (DataView)Category.GetCheckParameter("Analyzer_Range_Records").ParameterValue;

		//        string OldFilter = AnalyzerRangeRecords.RowFilter;

		//        if (Scale == "H") 
		//          OtherScale = "L";

		//        else 
		//          OtherScale = "H";

		//        AnalyzerRangeRecords.RowFilter = AddToDataViewFilter(OldFilter,
		//          "analyzer_range_cd = '" + OtherScale + 
		//          "' and (Begin_date < '" + BeginDate.ToShortDateString() + "' " +
		//          "or (Begin_date = '" + BeginDate.ToShortDateString() + "' " +
		//          "and Begin_hour <= " + BeginHour + ")) " +
		//          "and (end_date is null or end_date > '" + EndDate.ToShortDateString() + "' " +
		//          "or (end_date = '" + EndDate.ToShortDateString() + "' " +
		//          "and end_hour > " + EndHour + "))");

		//        if (AnalyzerRangeRecords.Count > 0) {
		//          Category.SetCheckParameter("Linearity_Scale_Valid", false, ParameterTypes.BOOLEAN);
		//          Category.CheckCatalogResult = "C";
		//        } 
		//        else
		//          Category.SetCheckParameter("Linearity_Scale_Valid", true, ParameterTypes.BOOLEAN);

		//        AnalyzerRangeRecords.RowFilter = OldFilter;
		//      }
		//    }
		//  }
		//  catch (Exception ex)
		//  {
		//    ReturnVal = Category.CheckEngine.FormatError(ex, "LINEAR7");
		//  }

		//  return ReturnVal;
		//}

		//public static string LINEAR8(cCategory Category, ref bool Log) //Validate Span Value
		//{
		//  string ReturnVal = "";

		//  try
		//  {

		//    DataRowView CurrentLinearity = (DataRowView)Category.GetCheckParameter("Current_Linearity_Test").ParameterValue;
		//    Category.SetCheckParameter("Linearity_Test_Span_Value", null, ParameterTypes.DECIMAL);

		//    Boolean ScaleValid = cDBConvert.ToBoolean(Category.GetCheckParameter("Linearity_Scale_Valid").ParameterValue);

		//    if (ScaleValid == true) {

		//      DateTime BeginDate = cDBConvert.ToDate(Category.GetCheckParameter("Linearity_Test_Begin_Date").ParameterValue, DateTypes.START);
		//      int BeginHour = cDBConvert.ToInteger(Category.GetCheckParameter("Linearity_Test_Begin_Hour").ParameterValue);
		//      DateTime EndDate = cDBConvert.ToDate(Category.GetCheckParameter("Linearity_Test_End_Date").ParameterValue, DateTypes.END);
		//      int EndHour = cDBConvert.ToInteger(Category.GetCheckParameter("Linearity_Test_End_Hour").ParameterValue);
		//      string ComponentTypeCd = cDBConvert.ToString(CurrentLinearity["component_type_cd"]);
		//      string Scale = cDBConvert.ToString(CurrentLinearity["span_scale_cd"]);

		//      DataView SpanRecords = (DataView)Category.GetCheckParameter("Span_Records").ParameterValue;

		//      string OldFilter = SpanRecords.RowFilter;

		//      SpanRecords.RowFilter = AddToDataViewFilter(OldFilter,
		//              "component_type_cd = '" + ComponentTypeCd + "' and span_scale_cd = '" + Scale +
		//              "' and span_value is not null and span_value >= 0 " +
		//              "and (Begin_date < '" + BeginDate.ToShortDateString() + "' " +
		//              "or (Begin_date = '" + BeginDate.ToShortDateString() + "' " +
		//              "and Begin_hour <= " + BeginHour + ")) " +
		//              "and (end_date is null or end_date > '" + EndDate.ToShortDateString() + "' " +
		//              "or (end_date = '" + EndDate.ToShortDateString() + "' " +
		//              "and end_hour > " + EndHour + "))");

		//      if (SpanRecords.Count == 0)
		//        Category.CheckCatalogResult = "A";

		//      else
		//      {
		//        if (SpanRecords.Count > 1)
		//          Category.CheckCatalogResult = "B";

		//        else
		//        {
		//          Decimal SpanValue = cDBConvert.ToDecimal(SpanRecords[0]["span_value"]);
		//          Category.SetCheckParameter("Linearity_Test_Span_Value", SpanValue, ParameterTypes.DECIMAL);
		//        }

		//        SpanRecords.RowFilter = OldFilter;
		//      }
		//    }
		//  }
		//  catch (Exception ex)
		//  {
		//    ReturnVal = Category.CheckEngine.FormatError(ex, "LINEAR8");
		//  }

		//  return ReturnVal;
		//}
		#endregion

		#region Linearity Check 9
		public static string LINEAR9(cCategory Category, ref bool Log) // Validate Test Reason Code
		{
			string ReturnVal = "";

			try
			{

				DataRowView CurrentLinearity = (DataRowView)Category.GetCheckParameter("Current_Linearity_Test").ParameterValue;

				string TestReasonCd = cDBConvert.ToString(CurrentLinearity["Test_Reason_Cd"]);

				if (TestReasonCd == "")
					Category.CheckCatalogResult = "A";

				else
				{
					DataView TestReasonCodeRecords = (DataView)Category.GetCheckParameter("test_reason_code_lookup_table").ParameterValue;

					if (!LookupCodeExists(TestReasonCd, TestReasonCodeRecords))
						Category.CheckCatalogResult = "B";
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "LINEAR9");
			}

			return ReturnVal;
		}
		#endregion

		#region Linearity Check 10
		public static string LINEAR10(cCategory Category, ref bool Log) // Validate Test Result Code
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentLinearity = (DataRowView)Category.GetCheckParameter("Current_Linearity_Test").ParameterValue;

				string TestResultCd = cDBConvert.ToString(CurrentLinearity["Test_Result_Cd"]);

				if (TestResultCd == "")
					Category.CheckCatalogResult = "A";
				else
				{
					if (!(TestResultCd.InList("PASSED,PASSAPS,FAILED,ABORTED")))
					{

						DataView TestResultCodeRecords = (DataView)Category.GetCheckParameter("test_result_code_lookup_table").ParameterValue;

						if (!LookupCodeExists(TestResultCd, TestResultCodeRecords))
							Category.CheckCatalogResult = "B";
						else
							Category.CheckCatalogResult = "C";
					}
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "LINEAR10");
			}

			return ReturnVal;
		}
		#endregion
		#endregion

		#region Linearity Checks 11-20

		#region Linearity Check 11
		public static string LINEAR11(cCategory Category, ref bool Log) // Simultaneous Injections
		{
			string ReturnVal = "";

			try
			{
				if (Convert.ToBoolean(Category.GetCheckParameter("Linearity_Injection_Times_Valid").ParameterValue))
				{
					DataRowView CurrentLinearity = (DataRowView)Category.GetCheckParameter("Current_Linearity_Test").ParameterValue;
					if (cDBConvert.ToBoolean(Category.GetCheckParameter("Simultaneous_Linearity_Injections").ParameterValue) == true)
						Category.CheckCatalogResult = "A";
				}
				else
					Log = false;
			}

			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "LINEAR11");
			}

			return ReturnVal;
		}
		#endregion

		#region Linearity Check 12
		public static string LINEAR12(cCategory Category, ref bool Log) // Injection Sequence Valid
		{
			string ReturnVal = "";

			try
			{

				DataRowView CurrentLinearity = (DataRowView)Category.GetCheckParameter("Current_Linearity_Test").ParameterValue;

				if (cDBConvert.ToBoolean(Category.GetCheckParameter("Linearity_Injection_Times_Valid").ParameterValue) == true)
				{

					if (cDBConvert.ToBoolean(Category.GetCheckParameter("Linearity_Sequence_Valid").ParameterValue) == false)
						Category.CheckCatalogResult = "A";
				}
				else
					Log = false;
			}

			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "LINEAR12");
			}

			return ReturnVal;
		}
		#endregion

		#region Linearity Check 13
		public static string LINEAR13(cCategory Category, ref bool Log) //Concurrent Test
		{
			string ReturnVal = "";

			try
			{

				if (cDBConvert.ToBoolean(Category.GetCheckParameter("Test_Dates_Consistent").ParameterValue) == true &&
					cDBConvert.ToBoolean(Category.GetCheckParameter("Test_Span_Scale_Valid").ParameterValue) == true)
				{

					DataRowView CurrentLinearity = (DataRowView)Category.GetCheckParameter("Current_Linearity_Test").ParameterValue;

					DateTime BeginDate = cDBConvert.ToDate(CurrentLinearity["begin_date"], DateTypes.START);
					int BeginHour = cDBConvert.ToInteger(CurrentLinearity["begin_hour"]);
					int BeginMin = cDBConvert.ToInteger(CurrentLinearity["begin_min"]);
					DateTime EndDate = cDBConvert.ToDate(CurrentLinearity["end_date"], DateTypes.END);
					int EndHour = cDBConvert.ToInteger(CurrentLinearity["end_hour"]);
					int EndMin = cDBConvert.ToInteger(CurrentLinearity["end_min"]);
					string Scale = cDBConvert.ToString(CurrentLinearity["span_scale_cd"]);
					string TestNumber = cDBConvert.ToString(CurrentLinearity["test_num"]);
					string TestSumID = cDBConvert.ToString(CurrentLinearity["test_sum_id"]);
					string CompID = cDBConvert.ToString(CurrentLinearity["component_id"]);
					string TestTypeCd = cDBConvert.ToString(CurrentLinearity["test_type_cd"]);

					DataView LinearityTestRecords = (DataView)Category.GetCheckParameter("Linearity_Test_Records").ParameterValue;

					string OldFilter = LinearityTestRecords.RowFilter;

					LinearityTestRecords.RowFilter = AddToDataViewFilter(OldFilter,
						"test_type_cd = '" + TestTypeCd + "' and span_scale_cd = '" + Scale + "' and test_sum_id <> '" + TestSumID +
						"' and (Begin_date < '" + EndDate.ToShortDateString() + "' " +
						"or (Begin_date = '" + EndDate.ToShortDateString() + "' " +
						"and Begin_hour < " + EndHour + ") " +
						"or (Begin_date = '" + EndDate.ToShortDateString() + "' " +
						"and Begin_hour = " + EndHour + " " +
						"and Begin_Min < " + EndMin + ")) " +
						"and (end_date > '" + BeginDate.ToShortDateString() + "' " +
						"or (end_date = '" + BeginDate.ToShortDateString() + "' " +
						"and end_hour > " + BeginHour + ") " +
						"or (end_date = '" + BeginDate.ToShortDateString() + "' " +
						"and end_hour = " + BeginHour + " " +
						"and end_Min > " + BeginMin + "))");

					if (LinearityTestRecords.Count > 0)
						Category.CheckCatalogResult = "A";

					else
					{

						DataView QASuppRecords = (DataView)Category.GetCheckParameter("QA_Supplemental_Data_Records").ParameterValue;

						string OldFilter2 = QASuppRecords.RowFilter;

						QASuppRecords.RowFilter = AddToDataViewFilter(OldFilter2,
							"test_sum_id <> '" + TestSumID + "' and test_type_cd = '" + TestTypeCd + "' and component_id = '" + CompID + "' and span_scale = '" + Scale + "' and test_num <> '" + TestNumber +
							"' and (Begin_date < '" + EndDate.ToShortDateString() + "' " +
							"or (Begin_date = '" + EndDate.ToShortDateString() + "' " +
							"and Begin_hour < " + EndHour + ") " +
							"or (Begin_date = '" + EndDate.ToShortDateString() + "' " +
							"and Begin_hour = " + EndHour + " " +
							"and Begin_Min < " + EndMin + ")) " +
							"and (end_date > '" + BeginDate.ToShortDateString() + "' " +
							"or (end_date = '" + BeginDate.ToShortDateString() + "' " +
							"and end_hour > " + BeginHour + ") " +
							"or (end_date = '" + BeginDate.ToShortDateString() + "' " +
							"and end_hour = " + BeginHour + " " +
							"and end_Min > " + BeginMin + "))");

						if (QASuppRecords.Count > 0)
							Category.CheckCatalogResult = "A";

						QASuppRecords.RowFilter = OldFilter2;

					}

					LinearityTestRecords.RowFilter = OldFilter;
				}
				else
					Log = false;
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "LINEAR13");
			}

			return ReturnVal;
		}
		#endregion

		#region Linearity Check 14
		public static string LINEAR14(cCategory Category, ref bool Log) //Duplicate Linearity Summary
		{
			string ReturnVal = "";

			try
			{

				DataRowView CurrentLinearitySummary = (DataRowView)Category.GetCheckParameter("Current_Linearity_Summary").ParameterValue;
				string LastLevel = cDBConvert.ToString(Category.GetCheckParameter("Last_Linearity_Level_Code").ParameterValue);
				string GasLevelCd = cDBConvert.ToString(CurrentLinearitySummary["gas_level_cd"]);
				string LevelList = cDBConvert.ToString(Category.GetCheckParameter("Linearity_Level_List").ParameterValue);

				if (GasLevelCd == LastLevel)
				{
					Category.SetCheckParameter("Linearity_Test_Result", "INVALID", eParameterDataType.String);
					Category.SetCheckParameter("Linearity_Level_Valid", false, eParameterDataType.Boolean);
					Category.CheckCatalogResult = "A";
				}
				else
				{
					Category.SetCheckParameter("Linearity_Level_List", LevelList.ListAdd(GasLevelCd), eParameterDataType.String);
					Category.SetCheckParameter("Last_Linearity_Level_Code", GasLevelCd, eParameterDataType.String);
				}

			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "LINEAR14");
			}

			return ReturnVal;
		}
		#endregion

		#region Linearity Check 15
		public static string LINEAR15(cCategory Category, ref bool Log) // Validate Gas Level Code
		{
			string ReturnVal = "";

			try
			{

				DataRowView CurrentLinearitySummary = (DataRowView)Category.GetCheckParameter("Current_Linearity_Summary").ParameterValue;

				string GasLevelCd = cDBConvert.ToString(CurrentLinearitySummary["Gas_Level_Cd"]);

				Category.SetCheckParameter("Linearity_Gas_Level_Valid", true, eParameterDataType.Boolean);
				if (GasLevelCd == "")
				{
					Category.CheckCatalogResult = "A";
					Category.SetCheckParameter("Linearity_Gas_Level_Valid", false, eParameterDataType.Boolean);

				}
				else
				{
					if (!(GasLevelCd.InList("HIGH,MID,LOW")))
					{
						Category.SetCheckParameter("Linearity_Gas_Level_Valid", false, eParameterDataType.Boolean);
						Category.CheckCatalogResult = "B";
					}
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "LINEAR15");
			}

			return ReturnVal;
		}
		#endregion

		#region Linearity Check 16
		public static string LINEAR16(cCategory Category, ref bool Log) // Validate Mean Measured Value
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentLinearitySummary = (DataRowView)Category.GetCheckParameter("Current_Linearity_Summary").ParameterValue;
				Decimal MeanValue = cDBConvert.ToDecimal(CurrentLinearitySummary["Mean_Measured_Value"]);

				if (MeanValue == decimal.MinValue)
					Category.CheckCatalogResult = "A";
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "LINEAR16");
			}

			return ReturnVal;
		}
		#endregion

		#region Linearity Check 17
		public static string LINEAR17(cCategory Category, ref bool Log) // Validate Mean Ref Value
		{
			string ReturnVal = "";

			try
			{

				DataRowView CurrentLinearitySummary = (DataRowView)Category.GetCheckParameter("Current_Linearity_Summary").ParameterValue;
				Decimal MeanValue = cDBConvert.ToDecimal(CurrentLinearitySummary["Mean_Ref_Value"]);

				if (MeanValue == decimal.MinValue)
					Category.CheckCatalogResult = "A";

				else
				{
					if (MeanValue < 0)
						Category.CheckCatalogResult = "B";
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "LINEAR17");
			}

			return ReturnVal;
		}
		#endregion

		#region Linearity Check 18
		public static string LINEAR18(cCategory Category, ref bool Log) // Validate Percent Error
		{
			string ReturnVal = "";

			try
			{

				DataRowView CurrentLinearitySummary = (DataRowView)Category.GetCheckParameter("Current_Linearity_Summary").ParameterValue;
				Decimal PercentError = cDBConvert.ToDecimal(CurrentLinearitySummary["Percent_Error"]);

				if (PercentError == decimal.MinValue)
				{
					Category.CheckCatalogResult = "A";
					Category.SetCheckParameter("Linearity_Summary_Percent_Error_Valid", false, eParameterDataType.Boolean);
				}
				else
				{

					if (PercentError < 0)
					{
						Category.CheckCatalogResult = "B";
						Category.SetCheckParameter("Linearity_Summary_Percent_Error_Valid", false, eParameterDataType.Boolean);

					}
					else
						Category.SetCheckParameter("Linearity_Summary_Percent_Error_Valid", true, eParameterDataType.Boolean);
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "LINEAR18");
			}

			return ReturnVal;
		}
		#endregion

		#region Linearity Check 19
		public static string LINEAR19(cCategory Category, ref bool Log) //Validate Injection Time
		{
			string ReturnVal = "";

			try
			{

				DataRowView CurrentLinearityInjection = (DataRowView)Category.GetCheckParameter("Current_Linearity_Injection").ParameterValue;

				DateTime InjectionDate = cDBConvert.ToDate(CurrentLinearityInjection["injection_date"], DateTypes.START);
				int InjectionHour = cDBConvert.ToInteger(CurrentLinearityInjection["injection_hour"]);
				int InjectionMin = cDBConvert.ToInteger(CurrentLinearityInjection["injection_min"]);
				Category.SetCheckParameter("Linearity_Injection_Included", false, eParameterDataType.Boolean);

				if (InjectionDate == DateTime.MinValue || InjectionHour < 0 || InjectionHour > 23 ||
					  InjectionMin < 0 || InjectionMin > 59)
				{
					Category.SetCheckParameter("Linearity_Injection_Time_Valid", false, eParameterDataType.Boolean);
					Category.SetCheckParameter("Linearity_Level_Valid", false, eParameterDataType.Boolean);
					Category.SetCheckParameter("Linearity_Injection_Count", -1, eParameterDataType.Integer);
					Category.CheckCatalogResult = "A";
				}
				else
				{
					DateTime InjectionTime = DateTime.Parse(string.Format("{0} {1}:{2}", InjectionDate.ToShortDateString(), InjectionHour, InjectionMin));
					int InjectionCount = (int)(Category.GetCheckParameter("Linearity_Injection_Count").ParameterValue);
					DateTime LastInjectionTime = cDBConvert.ToDate(Category.GetCheckParameter("Last_Injection_Time").ParameterValue, DateTypes.END);
					Category.SetCheckParameter("Linearity_Injection_Time_Valid", true, eParameterDataType.Boolean);

					if (InjectionTime == LastInjectionTime)
					{
						Category.SetCheckParameter("Linearity_Level_Valid", false, eParameterDataType.Boolean);
						Category.SetCheckParameter("Linearity_Injection_Count", -1, eParameterDataType.Integer);
					}

					else
					{

						Category.SetCheckParameter("Last_Injection_Time", InjectionTime, eParameterDataType.String);

						if (InjectionCount >= 0)
						{

							InjectionCount = InjectionCount + 1;
							Category.SetCheckParameter("Linearity_Injection_Count", InjectionCount, eParameterDataType.Integer);

							if (InjectionCount <= 3)
								Category.SetCheckParameter("Linearity_Injection_Included", true, eParameterDataType.Boolean);
						}
					}
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "LINEAR19");
			}

			return ReturnVal;
		}
		#endregion

		#region Linearity Check 20
		public static string LINEAR20(cCategory Category, ref bool Log) // Validate Measured Value
		{
			string ReturnVal = "";

			try
			{

				DataRowView CurrentLinearityInjection = (DataRowView)Category.GetCheckParameter("Current_Linearity_Injection").ParameterValue;
				Decimal ThisValue = cDBConvert.ToDecimal(CurrentLinearityInjection["Measured_Value"]);

				if (ThisValue == decimal.MinValue)
				{
					Category.SetCheckParameter("Linearity_Level_Valid", false, eParameterDataType.Boolean);
					Category.CheckCatalogResult = "A";
				}
				else
				{
					if (cDBConvert.ToBoolean(Category.GetCheckParameter("Linearity_Injection_Included").ParameterValue) == true &&
						cDBConvert.ToBoolean(Category.GetCheckParameter("Linearity_Level_Valid").ParameterValue) == true)
						Category.SetCheckParameter("Linearity_Measured_Value_Total",
						cDBConvert.ToDecimal(Category.GetCheckParameter("Linearity_Measured_Value_Total").ParameterValue) + ThisValue, eParameterDataType.Decimal);
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "LINEAR20");
			}

			return ReturnVal;
		}
		#endregion
		#endregion

		#region Linearity Checks 21-30

		#region Linearity Check 21
		public static string LINEAR21(cCategory Category, ref bool Log) // Validate Ref Value
		{
			string ReturnVal = "";

			try
			{

				DataRowView CurrentLinearityInjection = (DataRowView)Category.GetCheckParameter("Current_Linearity_Injection").ParameterValue;
				Decimal ThisValue = cDBConvert.ToDecimal(CurrentLinearityInjection["Ref_Value"]);

				if (ThisValue == decimal.MinValue)
				{
					Category.SetCheckParameter("Linearity_Injection_Reference_Value_Valid", false, eParameterDataType.Boolean);
					Category.SetCheckParameter("Linearity_Level_Valid", false, eParameterDataType.Boolean);
					Category.CheckCatalogResult = "A";
				}
				else
				{
					if (ThisValue < 0)
					{
						Category.SetCheckParameter("Linearity_Injection_Reference_Value_Valid", false, eParameterDataType.Boolean);
						Category.SetCheckParameter("Linearity_Level_Valid", false, eParameterDataType.Boolean);
						Category.CheckCatalogResult = "B";
					}
					else
					{
						Category.SetCheckParameter("Linearity_Injection_Reference_Value_Valid", true, eParameterDataType.Boolean);
						if (cDBConvert.ToBoolean(Category.GetCheckParameter("Linearity_Injection_Included").ParameterValue) == true &&
							cDBConvert.ToBoolean(Category.GetCheckParameter("Linearity_Level_Valid").ParameterValue) == true)
							Category.SetCheckParameter("Linearity_Reference_Value_Total",
							  cDBConvert.ToDecimal(Category.GetCheckParameter("Linearity_Reference_Value_Total").ParameterValue) + ThisValue, eParameterDataType.Decimal);
					}
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "LINEAR21");
			}

			return ReturnVal;
		}
		#endregion

		#region Linearity Check 22
		public static string LINEAR22(cCategory Category, ref bool Log) // Compare Ref Value and Span to Gas Level
		{
			string ReturnVal = "";

			try
			{

				DataRowView CurrentLinearityInjection = (DataRowView)Category.GetCheckParameter("Current_Linearity_Injection").ParameterValue;
				decimal SpanValue = cDBConvert.ToDecimal(Category.GetCheckParameter("Test_Span_Value").ParameterValue);

				string RVConsistent = cDBConvert.ToString(Category.GetCheckParameter("Linearity_Reference_Value_Consistent_with_Span").ParameterValue);

				if (cDBConvert.ToBoolean(Category.GetCheckParameter("Linearity_Injection_Reference_Value_Valid").ParameterValue) && SpanValue > 0)
				{
					decimal ThisValue = cDBConvert.ToDecimal(CurrentLinearityInjection["Ref_Value"]);
					decimal PercentOfSpan = Math.Round(ThisValue / SpanValue * 100, 1, MidpointRounding.AwayFromZero);
					Category.SetCheckParameter("Linearity_Reference_Percent_of_Span", PercentOfSpan, eParameterDataType.Decimal);
					string GasLevel = cDBConvert.ToString(CurrentLinearityInjection["gas_level_cd"]);
					DataView TestToleranceRecords = (DataView)Category.GetCheckParameter("Test_Tolerances_Cross_Check_Table").ParameterValue;
					string OldFilter = TestToleranceRecords.RowFilter;
					TestToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, "FieldDescription = '" + "GasPercentOfSpan'");
					TestToleranceRecords.RowFilter = AddToDataViewFilter(TestToleranceRecords.RowFilter, "TestTypeCode = '" + QaParameters.CurrentLinearitySummary.TestTypeCd + "'");
					decimal Tolerance = cDBConvert.ToDecimal(((DataRowView)TestToleranceRecords[0])["Tolerance"]);
					TestToleranceRecords.RowFilter = OldFilter;
					decimal Floor;
					decimal Ceiling;

					if (GasLevel == "LOW")
					{
						Floor = (decimal)20.0;
						Ceiling = (decimal)30.0;
						if (PercentOfSpan < Floor || PercentOfSpan > Ceiling)
							if (PercentOfSpan < Floor - Tolerance || PercentOfSpan > Ceiling + Tolerance)
							{
								Category.SetCheckParameter("Linearity_Reference_Value_Consistent_with_Span", "CRITICAL", eParameterDataType.String);
								Category.CheckCatalogResult = "A";
							}
							else
								if (RVConsistent == "")
								{
									Category.SetCheckParameter("Linearity_Reference_Value_Consistent_with_Span", "WARNING", eParameterDataType.String);
									Category.CheckCatalogResult = "B";
								}
					}
					else
					{
						if (GasLevel == "MID")
						{
							Floor = (decimal)50.0;
							Ceiling = (decimal)60.0;
							if (PercentOfSpan < Floor || PercentOfSpan > Ceiling)
								if (PercentOfSpan < Floor - Tolerance || PercentOfSpan > Ceiling + Tolerance)
								{
									Category.SetCheckParameter("Linearity_Reference_Value_Consistent_with_Span", "CRITICAL", eParameterDataType.String);
									Category.CheckCatalogResult = "C";
								}
								else
									if (RVConsistent == "")
									{
										Category.SetCheckParameter("Linearity_Reference_Value_Consistent_with_Span", "WARNING", eParameterDataType.String);
										Category.CheckCatalogResult = "D";
									}
						}
						else
						{
							if (GasLevel == "HIGH")
							{
								Ceiling = (decimal)100.0;
								Floor = (decimal)80.0;
								if (PercentOfSpan > Ceiling)
								{
									Category.SetCheckParameter("Linearity_Reference_Value_Consistent_with_Span", "CRITICAL", eParameterDataType.String);
									Category.CheckCatalogResult = "E";
								}
								else
								{
									if (PercentOfSpan < Floor)
										if (PercentOfSpan < Floor - Tolerance)
										{
											Category.SetCheckParameter("Linearity_Reference_Value_Consistent_with_Span", "CRITICAL", eParameterDataType.String);
											Category.CheckCatalogResult = "E";
										}
										else
											if (RVConsistent == "")
											{
												Category.SetCheckParameter("Linearity_Reference_Value_Consistent_with_Span", "WARNING", eParameterDataType.String);
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
				ReturnVal = Category.CheckEngine.FormatError(ex, "LINEAR22");
			}

			return ReturnVal;
		}
		#endregion

		#region Linearity Check 23
		public static string LINEAR23(cCategory Category, ref bool Log) // Compare Ref Value to Gas Level
		{
			string ReturnVal = "";

			try
			{

				DataRowView CurrentLinearityInjection = (DataRowView)Category.GetCheckParameter("Current_Linearity_Injection").ParameterValue;
				string GasLevel = cDBConvert.ToString(CurrentLinearityInjection["gas_level_cd"]);
				string LinInjID = cDBConvert.ToString(CurrentLinearityInjection["lin_inj_id"]);

				if (cDBConvert.ToBoolean(Category.GetCheckParameter("Linearity_Injection_Reference_Value_Valid").ParameterValue) == true &&
					GasLevel == "MID")
				{

					Decimal ThisValue = cDBConvert.ToDecimal(CurrentLinearityInjection["Ref_Value"]);
					DataView InjectionRecords = (DataView)Category.GetCheckParameter("Linearity_Test_Injection_Records").ParameterValue;

					string OldFilter = InjectionRecords.RowFilter;

					InjectionRecords.RowFilter = AddToDataViewFilter(OldFilter,
					  "GAS_LEVEL_CD = 'HIGH' AND REF_VALUE < " + Convert.ToString(ThisValue) +
					  " AND LIN_INJ_ID <> '" + LinInjID + "'");

					if (InjectionRecords.Count > 0)
					{
						Category.SetCheckParameter("Linearity_Reference_Values_Consistent", false, eParameterDataType.Boolean);
						Category.SetCheckParameter("Linearity_Test_Result", "INVALID", eParameterDataType.String);
						Category.CheckCatalogResult = "A";
					}
					else
					{
						InjectionRecords.RowFilter = AddToDataViewFilter(OldFilter,
						  "GAS_LEVEL_CD = 'LOW' AND REF_VALUE > " + Convert.ToString(ThisValue) +
						  " AND LIN_INJ_ID <> '" + LinInjID + "'");
						if (InjectionRecords.Count > 0)
						{
							Category.SetCheckParameter("Linearity_Reference_Values_Consistent", false, eParameterDataType.Boolean);
							Category.SetCheckParameter("Linearity_Test_Result", "INVALID", eParameterDataType.String);
							Category.CheckCatalogResult = "A";
						}
					}

					InjectionRecords.RowFilter = OldFilter;
				}
				else
					Log = false;

			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "LINEAR23");
			}

			return ReturnVal;
		}
		#endregion

		#region Linearity Check 24
		public static string LINEAR24(cCategory Category, ref bool Log) // Find Simultaneous Injection for Alternate Range
		{
			string ReturnVal = "";

			try
			{

				if ((Boolean)Category.GetCheckParameter("Linearity_Injection_Time_Valid").ParameterValue == true)
				{

					DataRowView CurrentLinearityInjection = (DataRowView)Category.GetCheckParameter("Current_Linearity_Injection").ParameterValue;
					DateTime InjectionDate = cDBConvert.ToDate(CurrentLinearityInjection["injection_date"], DateTypes.START);
					int InjectionHour = cDBConvert.ToInteger(CurrentLinearityInjection["injection_hour"]);
					int InjectionMin = cDBConvert.ToInteger(CurrentLinearityInjection["injection_min"]);
					string Scale = cDBConvert.ToString(CurrentLinearityInjection["span_scale_cd"]);

					DataView InjectionRecords = (DataView)Category.GetCheckParameter("Component_Linearity_Injection_Records").ParameterValue;

					string OldFilter = InjectionRecords.RowFilter;

					if (Scale == "H")
					{

						InjectionRecords.RowFilter = AddToDataViewFilter(OldFilter,
						  "SPAN_SCALE_CD = 'L' AND INJECTION_DATE = '" + InjectionDate.ToShortDateString() + "' AND " +
						  "INJECTION_HOUR = " + InjectionHour + " AND INJECTION_MIN = " + InjectionMin);

						if (InjectionRecords.Count > 0)
						{
							Category.SetCheckParameter("Linearity_Reference_Values_Consistent", false, eParameterDataType.Boolean);
							Category.CheckCatalogResult = "A";
						}
						InjectionRecords.RowFilter = OldFilter;
					}
					else
					{
						if (Scale == "L")
						{

							InjectionRecords.RowFilter = AddToDataViewFilter(OldFilter,
							  "SPAN_SCALE_CD = 'H' AND INJECTION_DATE = '" + InjectionDate.ToShortDateString() + "' AND " +
							  "INJECTION_HOUR = " + InjectionHour + " AND INJECTION_MIN = " + InjectionMin);

							if (InjectionRecords.Count > 0)
							{
								Category.SetCheckParameter("Linearity_Reference_Values_Consistent", false, eParameterDataType.Boolean);
								Category.CheckCatalogResult = "A";
							}

							InjectionRecords.RowFilter = OldFilter;
						}
						else
							Log = false;
					}
				}
				else
					Log = false;
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "LINEAR24");
			}

			return ReturnVal;
		}
		#endregion

		#region Linearity Check 25
		public static string LINEAR25(cCategory Category, ref bool Log) // Appropriate number of injections
		{
			string ReturnVal = "";

			try
			{

				DataRowView CurrentLinearitySummary = (DataRowView)Category.GetCheckParameter("Current_Linearity_Summary").ParameterValue;
				int InjCount = (int)Category.GetCheckParameter("Linearity_Injection_Count").ParameterValue;

				if (InjCount >= 0 && InjCount < 3)
				{
					Category.SetCheckParameter("Calculate_Linearity_Level", false, eParameterDataType.Boolean);
					Category.CheckCatalogResult = "A";
				}
				else
				{
					Category.SetCheckParameter("Calculate_Linearity_Level",
					  (Boolean)Category.GetCheckParameter("Linearity_Level_Valid").ParameterValue, eParameterDataType.Boolean);

					if (InjCount > 3)
						Category.CheckCatalogResult = "B";
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "LINEAR25");
			}

			return ReturnVal;
		}
		#endregion

		#region Linearity Check 26
		public static string LINEAR26(cCategory Category, ref bool Log) // Calculate Level
		{
			string ReturnVal = "";

			try
			{
				if ((Boolean)Category.GetCheckParameter("Calculate_Linearity_Level").ParameterValue == true)
				{

					DataRowView CurrentLinearitySummary = (DataRowView)Category.GetCheckParameter("Current_Linearity_Summary").ParameterValue;
					string CompTypeCode = cDBConvert.ToString(CurrentLinearitySummary["component_type_cd"]);
					int APSInd = cDBConvert.ToInteger(CurrentLinearitySummary["APS_IND"]);
					string TestResult = Category.GetCheckParameter("Linearity_Test_Result").ValueAsString();
					Decimal MeanMeasured = (decimal)Category.GetCheckParameter("Linearity_Measured_Value_Total").ParameterValue / 3;
					Decimal MeanReference = (decimal)Category.GetCheckParameter("Linearity_Reference_Value_Total").ParameterValue / 3;
					Decimal MeanDiff = Math.Abs(MeanReference - MeanMeasured);
					Decimal lsPercentError = QaParameters.CurrentLinearitySummary.PercentError.IsNull() ? Decimal.MaxValue : (decimal)QaParameters.CurrentLinearitySummary.PercentError;
					Decimal calcPercentError = Decimal.MaxValue;
					if (MeanReference > 0)
					{
						calcPercentError = Math.Round(MeanDiff / MeanReference * 100, 1, MidpointRounding.AwayFromZero);
						if (calcPercentError > Convert.ToDecimal(9999.9))
							calcPercentError = Convert.ToDecimal(9999.9);
						Category.SetCheckParameter("Linearity_Summary_Percent_Error", calcPercentError, eParameterDataType.Decimal);
					}
					else
						Category.SetCheckParameter("Linearity_Summary_Percent_Error", null, eParameterDataType.Decimal);

					Category.SetCheckParameter("Linearity_Summary_Mean_Measured_Value", Math.Round(MeanMeasured, 3, MidpointRounding.AwayFromZero), eParameterDataType.Decimal);
					Category.SetCheckParameter("Linearity_Summary_Mean_Reference_Value", Math.Round(MeanReference, 3, MidpointRounding.AwayFromZero), eParameterDataType.Decimal);
					if (CompTypeCode.InList("NOX,SO2"))
						MeanDiff = Math.Round(MeanDiff, 0, MidpointRounding.AwayFromZero);
					else
						MeanDiff = Math.Round(MeanDiff, 1, MidpointRounding.AwayFromZero);
					Category.SetCheckParameter("Linearity_Summary_Mean_Difference", MeanDiff, eParameterDataType.Decimal);

					if (MeanReference > 0 && (calcPercentError <= 5 || (CompTypeCode == "HG" && calcPercentError <= 10)))
					{
						Category.SetCheckParameter("Linearity_Summary_APS_Indicator", 0, eParameterDataType.Integer);
						if (!TestResult.InList("INVALID,FAILED,PASSAPS"))
							TestResult = "PASSED";
					}
					else
					{
						DataView TestToleranceRecords = (DataView)Category.GetCheckParameter("Test_Tolerances_Cross_Check_Table").ParameterValue;
						sFilterPair[] TolFilter = new sFilterPair[2];
						TolFilter[0].Set("TestTypeCode", "LINE");
						DataRowView TolRowFound;
						if (CompTypeCode.InList("NOX,SO2"))
						{
							if (MeanDiff <= 5)
							{
								Category.SetCheckParameter("Linearity_Summary_APS_Indicator", 1, eParameterDataType.Integer);
								Category.SetCheckParameter("Linearity_Summary_Percent_Error", MeanDiff, eParameterDataType.Decimal);

								if (!TestResult.InList("INVALID,FAILED"))
									TestResult = "PASSAPS";
							}
							else
							{
								if (MeanReference > 0)
									Category.SetCheckParameter("Linearity_Summary_APS_Indicator", 0, eParameterDataType.Integer);
								else
								{
									Category.SetCheckParameter("Linearity_Summary_APS_Indicator", 1, eParameterDataType.Integer);
									Category.SetCheckParameter("Linearity_Summary_Percent_Error", MeanDiff, eParameterDataType.Decimal);
								}
								if (!TestResult.InList("FAILED,INVALID"))
									if (APSInd == 1)
									{
										TestResult = "FAILED";
										if (0 <= lsPercentError && lsPercentError <= 5)
										{
											TolFilter[1].Set("FieldDescription", "MeanDifferencePPM");
											if (FindRow(TestToleranceRecords, TolFilter, out TolRowFound))
												if (Math.Abs(MeanDiff - lsPercentError) <= cDBConvert.ToDecimal(TolRowFound["Tolerance"]))
													TestResult = "PASSAPS";
										}
									}
									else
										if (MeanReference > 0 && 0 <= lsPercentError && lsPercentError <= 5)
										{
											TolFilter[1].Set("FieldDescription", "PercentError");
											if (FindRow(TestToleranceRecords, TolFilter, out TolRowFound))
												if (Math.Abs(MeanDiff - lsPercentError) <= cDBConvert.ToDecimal(TolRowFound["Tolerance"]))
													if (TestResult != "PASSAPS")
														TestResult = "PASSED";
													else
														TestResult = "FAILED";
										}
										else
											TestResult = "FAILED";
							}
						}
						else
						{
							if (CompTypeCode.InList("CO2,O2"))
							{
								if (MeanDiff <= (decimal)0.5)
								{
									Category.SetCheckParameter("Linearity_Summary_APS_Indicator", 1, eParameterDataType.Integer);
									Category.SetCheckParameter("Linearity_Summary_Percent_Error", MeanDiff, eParameterDataType.Decimal);

									if (!TestResult.InList("INVALID,FAILED"))
										TestResult = "PASSAPS";
								}
								else
								{
									Category.SetCheckParameter("Linearity_Summary_APS_Indicator", 0, eParameterDataType.Integer);
									if (!TestResult.InList("FAILED,INVALID"))
										if (APSInd == 1)
										{
											TestResult = "FAILED";
											if (0 <= lsPercentError && lsPercentError <= (decimal)0.5)
											{
												TolFilter[1].Set("FieldDescription", "MeanDifferencePCT");
												if (FindRow(TestToleranceRecords, TolFilter, out TolRowFound))
													if (Math.Abs(MeanDiff - lsPercentError) <= cDBConvert.ToDecimal(TolRowFound["Tolerance"]))
														TestResult = "PASSAPS";
											}
										}
										else
											if (0 <= lsPercentError && lsPercentError <= 5)
											{
												TolFilter[1].Set("FieldDescription", "PercentError");
												if (FindRow(TestToleranceRecords, TolFilter, out TolRowFound))
													if (Math.Abs(MeanDiff - lsPercentError) <= cDBConvert.ToDecimal(TolRowFound["Tolerance"]))
														if (TestResult != "PASSAPS")
															TestResult = "PASSED";
														else
															TestResult = "FAILED";
											}
											else
												TestResult = "FAILED";
								}
							}
							else
								if (CompTypeCode == "HG")
								{
									if (MeanDiff <= (decimal)0.8)
									{
										Category.SetCheckParameter("Linearity_Summary_APS_Indicator", 1, eParameterDataType.Integer);
										Category.SetCheckParameter("Linearity_Summary_Percent_Error", MeanDiff, eParameterDataType.Decimal);

										if (!TestResult.InList("INVALID,FAILED"))
											TestResult = "PASSAPS";
									}
									else
									{
										if (MeanReference > 0)
											Category.SetCheckParameter("Linearity_Summary_APS_Indicator", 0, eParameterDataType.Integer);
										else
										{
											Category.SetCheckParameter("Linearity_Summary_APS_Indicator", 1, eParameterDataType.Integer);
											Category.SetCheckParameter("Linearity_Summary_Percent_Error", MeanDiff, eParameterDataType.Decimal);
										}

										if (!TestResult.InList("FAILED,INVALID"))
											if (APSInd == 1)
											{
												TestResult = "FAILED";
												if (0 <= lsPercentError && lsPercentError <= (decimal)0.8)
												{
													TolFilter[0].Set("TestTypeCode", QaParameters.CurrentLinearitySummary.TestTypeCd);
													TolFilter[1].Set("FieldDescription", "MeanDifferenceUGSCM");
													if (FindRow(TestToleranceRecords, TolFilter, out TolRowFound))
														if (Math.Abs(MeanDiff - lsPercentError) <= cDBConvert.ToDecimal(TolRowFound["Tolerance"]))
															TestResult = "PASSAPS";
												}
											}
											else
												if (MeanReference > 0 && 0 <= lsPercentError && lsPercentError <= (decimal)10)
												{
													TolFilter[0].Set("TestTypeCode", QaParameters.CurrentLinearitySummary.TestTypeCd);
													TolFilter[1].Set("FieldDescription", "PercentError");
													if (FindRow(TestToleranceRecords, TolFilter, out TolRowFound))
														if (Math.Abs(MeanDiff - lsPercentError) <= cDBConvert.ToDecimal(TolRowFound["Tolerance"]))
															if (TestResult != "PASSAPS")
																TestResult = "PASSED";
															else
																TestResult = "FAILED";
												}
												else
													TestResult = "FAILED";
									}
								}
						}
					}
					Category.SetCheckParameter("Linearity_Test_Result", TestResult, eParameterDataType.String);
				}
				else
				{
					Category.SetCheckParameter("Linearity_Test_Result", "INVALID", eParameterDataType.String);
					Category.SetCheckParameter("Linearity_Summary_Mean_Measured_Value", null, eParameterDataType.Decimal);
					Category.SetCheckParameter("Linearity_Summary_Mean_Reference_Value", null, eParameterDataType.Decimal);
					Category.SetCheckParameter("Linearity_Summary_Mean_Difference", null, eParameterDataType.Decimal);
					Category.SetCheckParameter("Linearity_Summary_Percent_Error", null, eParameterDataType.Decimal);
					Category.SetCheckParameter("Linearity_Summary_APS_Indicator", null, eParameterDataType.Integer);
					Category.CheckCatalogResult = "A";
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "LINEAR26");
			}

			return ReturnVal;
		}
		#endregion

		#region Linearity Check 27

		public static string LINEAR27(cCategory Category, ref bool Log) // Compare Calculated Results for Level to Reported Results
		{
			string ReturnVal = "";

			try
			{
				if ((Boolean)Category.GetCheckParameter("Calculate_Linearity_Level").ParameterValue == true)
				{
					DataRowView CurrentLinearitySummary = (DataRowView)Category.GetCheckParameter("Current_Linearity_Summary").ParameterValue;
					int APSInd = cDBConvert.ToInteger(CurrentLinearitySummary["APS_Ind"]);

					if (APSInd != 1 && (int)Category.GetCheckParameter("Linearity_Summary_APS_Indicator").ParameterValue == 1)
						Category.CheckCatalogResult = "A";
					else
					{
						string CompTypeCode = cDBConvert.ToString(CurrentLinearitySummary["component_type_cd"]);
						DataView TestToleranceRecords = (DataView)Category.GetCheckParameter("Test_Tolerances_Cross_Check_Table").ParameterValue;
						string OldFilter = TestToleranceRecords.RowFilter;
						decimal ToleranceValue = 0;
						if ((Boolean)Category.GetCheckParameter("Linearity_Summary_Percent_Error_Valid").ParameterValue == true)
						{
							if (APSInd == 0)
							{
								TestToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, "FieldDescription = '" + "PercentError" + "'");
								TestToleranceRecords.RowFilter = AddToDataViewFilter(TestToleranceRecords.RowFilter, "TestTypeCode IN " + "('LINE','HGLINE','HGSI3')");
								ToleranceValue = cDBConvert.ToDecimal(((DataRowView)TestToleranceRecords[0])["Tolerance"]);
								if (System.Math.Abs((Decimal)Category.GetCheckParameter("Linearity_Summary_Percent_Error").ParameterValue -
									  cDBConvert.ToDecimal(CurrentLinearitySummary["Percent_Error"])) > ToleranceValue)
									Category.CheckCatalogResult = "B";
							}
							else
							{
								if (APSInd == 1)
								{
									if (CompTypeCode.InList("NOX,SO2"))
									{

										TestToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, "FieldDescription = '" + "MeanDifferencePPM" + "'");
										TestToleranceRecords.RowFilter = AddToDataViewFilter(TestToleranceRecords.RowFilter, "TestTypeCode = '" + "LINE" + "'");
										ToleranceValue = cDBConvert.ToDecimal(((DataRowView)TestToleranceRecords[0])["Tolerance"]);
										if (System.Math.Abs((Decimal)Category.GetCheckParameter("Linearity_Summary_Mean_Difference").ParameterValue -
											cDBConvert.ToDecimal(CurrentLinearitySummary["Percent_Error"])) > ToleranceValue)
											Category.CheckCatalogResult = "B";
									}

									else if (CompTypeCode == "HG")
									{
										TestToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, "FieldDescription = '" + "MeanDifferenceUGSCM" + "'");
										TestToleranceRecords.RowFilter = AddToDataViewFilter(TestToleranceRecords.RowFilter, "TestTypeCode = '" + QaParameters.CurrentLinearitySummary.TestTypeCd + "'");
										ToleranceValue = cDBConvert.ToDecimal(((DataRowView)TestToleranceRecords[0])["Tolerance"]);
										if (System.Math.Abs((Decimal)Category.GetCheckParameter("Linearity_Summary_Mean_Difference").ParameterValue -
											cDBConvert.ToDecimal(CurrentLinearitySummary["Percent_Error"])) > ToleranceValue)
											Category.CheckCatalogResult = "B";
									}
									else
									{
										TestToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, "FieldDescription = '" + "MeanDifferencePCT" + "'");
										TestToleranceRecords.RowFilter = AddToDataViewFilter(TestToleranceRecords.RowFilter, "TestTypeCode = '" + QaParameters.CurrentLinearitySummary.TestTypeCd + "'");
										ToleranceValue = cDBConvert.ToDecimal(((DataRowView)TestToleranceRecords[0])["Tolerance"]);
										if (System.Math.Abs((Decimal)Category.GetCheckParameter("Linearity_Summary_Mean_Difference").ParameterValue -
											cDBConvert.ToDecimal(CurrentLinearitySummary["Percent_Error"])) > ToleranceValue)
											Category.CheckCatalogResult = "B";
									}
								}
							}
						}

                        if (Category.CheckCatalogResult == null)
                        {
                            string LinIntermedValues = "";
                            if (CompTypeCode.InList("SO2,NOX"))
                            {
                                TestToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, "FieldDescription = '" + "MeanDifferencePPM" + "'");
                                TestToleranceRecords.RowFilter = AddToDataViewFilter(TestToleranceRecords.RowFilter, "TestTypeCode = '" + "LINE" + "'");
                                ToleranceValue = cDBConvert.ToDecimal(((DataRowView)TestToleranceRecords[0])["Tolerance"]);
                            }
                            else if (CompTypeCode == "HG")
                            {
                                TestToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, "FieldDescription = '" + "MeanDifferenceUGSCM" + "'");
                                TestToleranceRecords.RowFilter = AddToDataViewFilter(TestToleranceRecords.RowFilter, "TestTypeCode = '" + QaParameters.CurrentLinearitySummary.TestTypeCd + "'");
                                ToleranceValue = cDBConvert.ToDecimal(((DataRowView)TestToleranceRecords[0])["Tolerance"]);
                            }
                            else
                            {
                                TestToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, "FieldDescription = '" + "MeanDifferencePCT" + "'");
                                TestToleranceRecords.RowFilter = AddToDataViewFilter(TestToleranceRecords.RowFilter, "TestTypeCode = '" + QaParameters.CurrentLinearitySummary.TestTypeCd + "'");
                                ToleranceValue = cDBConvert.ToDecimal(((DataRowView)TestToleranceRecords[0])["Tolerance"]);
                            }
                            TestToleranceRecords.RowFilter = OldFilter;

                            decimal SummaryMeanRefVal = cDBConvert.ToDecimal(CurrentLinearitySummary["MEAN_REF_VALUE"]);

                            if (SummaryMeanRefVal != decimal.MinValue && System.Math.Abs(Convert.ToDecimal(Category.GetCheckParameter("Linearity_Summary_Mean_Reference_Value").ParameterValue) -
                                SummaryMeanRefVal) > ToleranceValue)
                                LinIntermedValues = LinIntermedValues.ListAdd("MeanReferenceValue");

                            decimal SummaryMeanMeasVal = cDBConvert.ToDecimal(CurrentLinearitySummary["MEAN_MEASURED_VALUE"]);

                            if (SummaryMeanMeasVal != decimal.MinValue && System.Math.Abs(Convert.ToDecimal(Category.GetCheckParameter("Linearity_Summary_Mean_Measured_Value").ParameterValue) -
                                SummaryMeanMeasVal) > ToleranceValue)
                                LinIntermedValues = LinIntermedValues.ListAdd("MeanMeasuredValue");
                            if (LinIntermedValues != "")
                            {
                                Category.SetCheckParameter("Linearity_Intermediate_Values", LinIntermedValues, eParameterDataType.String);
                                Category.CheckCatalogResult = "C";
                            }
                            else
                                Category.SetCheckParameter("Linearity_Intermediate_Values", null, eParameterDataType.String);
                        }
					}
				}
				else
					Log = false;
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "LINEAR27");
			}

			return ReturnVal;
		}
		#endregion

		#region Linearity Check 28

		public static string LINEAR28(cCategory Category, ref bool Log) // Appropriate Number of Gas Levels
		{
			string ReturnVal = "";

			try
			{

				DataRowView CurrentLinearity = (DataRowView)Category.GetCheckParameter("Current_Linearity_Test").ParameterValue;

				if ((string)Category.GetCheckParameter("Linearity_Test_Result").ParameterValue == "ABORTED" ||
					(Boolean)Category.GetCheckParameter("Linearity_Component_Valid").ParameterValue == false)
					Category.SetCheckParameter("Calculate_Linearity_Test", false, eParameterDataType.Boolean);

				else
				{
					Category.SetCheckParameter("Calculate_Linearity_Test", true, eParameterDataType.Boolean);
					if (cDBConvert.ToString(Category.GetCheckParameter("Linearity_Level_List").ParameterValue).ListCount() < 3)
					{
						Category.SetCheckParameter("Calculate_Linearity_Test", false, eParameterDataType.Boolean);
						Category.SetCheckParameter("Linearity_Test_Result", null, eParameterDataType.String);
						Category.CheckCatalogResult = "A";
					}
					else
					{

						if ((string)Category.GetCheckParameter("Linearity_Test_Result").ParameterValue == "INVALID")
						{
							Category.SetCheckParameter("Linearity_Test_Result", null, eParameterDataType.String);
							Category.SetCheckParameter("Calculate_Linearity_Test", false, eParameterDataType.Boolean);
						}
					}
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "LINEAR28");
			}

			return ReturnVal;
		}
		#endregion

		#region Linearity Check 29

		public static string LINEAR29(cCategory Category, ref bool Log) // Compare Final Test Result to Reported Result
		{
			string ReturnVal = "";

			try
			{

				DataRowView CurrentLinearity = (DataRowView)Category.GetCheckParameter("Current_Linearity_Test").ParameterValue;

				string TestResultCd = cDBConvert.ToString(CurrentLinearity["Test_Result_Cd"]);

				if (TestResultCd == "")
					Category.CheckCatalogResult = "A";
				else
				{
					if (!(TestResultCd.InList("PASSED,PASSAPS,FAILED,ABORTED")))
					{
						DataView TestResultCodeRecords = (DataView)Category.GetCheckParameter("test_result_code_lookup_table").ParameterValue;

						if (!LookupCodeExists(TestResultCd, TestResultCodeRecords))
							Category.CheckCatalogResult = "B";
						else
							Category.CheckCatalogResult = "C";
					}
				}
				if (string.IsNullOrEmpty(Category.CheckCatalogResult))
				{
					string LinTestRes = Convert.ToString(Category.GetCheckParameter("Linearity_Test_Result").ParameterValue);
					if (LinTestRes == "FAILED" && TestResultCd.InList("PASSED,PASSAPS"))
						Category.CheckCatalogResult = "D";
					else
						if (LinTestRes.InList("PASSED,PASSAPS") && TestResultCd == "FAILED")
							Category.CheckCatalogResult = "E";
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "LINEAR29");
			}

			return ReturnVal;
		}
		#endregion

		#region Linearity Check 30

		public static string LINEAR30(cCategory Category, ref bool Log) // Initialize Linearity Variables
		{
			string ReturnVal = "";

			try
			{
				Category.SetCheckParameter("Linearity_Test_Result", null, eParameterDataType.String);
				Category.SetCheckParameter("Linearity_Level_List", null, eParameterDataType.String);
				Category.SetCheckParameter("Last_Linearity_Level_Code", null, eParameterDataType.String);
				Category.SetCheckParameter("Linearity_Reference_Values_Consistent", true, eParameterDataType.Boolean);
				Category.SetCheckParameter("Simultaneous_Linearity_Injection_for_Alternate_Range", false, eParameterDataType.Boolean);
				Category.SetCheckParameter("Linearity_Test_Type", null, eParameterDataType.String);
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "LINEAR30");
			}

			return ReturnVal;
		}
		#endregion
		#endregion

		#region Linearity Checks 31-40

		#region Linearity Check 31

		public static string LINEAR31(cCategory Category, ref bool Log) // Duplicate Test 
		{
			string ReturnVal = "";

			try
			{
				if (cDBConvert.ToBoolean(Category.GetCheckParameter("Test_Number_Valid").ParameterValue) == true)
				{
					DataRowView CurrentLinearity = (DataRowView)Category.GetCheckParameter("Current_Linearity_Test").ParameterValue;
					string TestSumID = cDBConvert.ToString(CurrentLinearity["test_sum_id"]);
					string TestTypeCd = cDBConvert.ToString(CurrentLinearity["test_type_cd"]);
					string TestNumber = cDBConvert.ToString(CurrentLinearity["test_num"]);
					DataView LocationTestRecords = (DataView)Category.GetCheckParameter("Location_Test_Records").ParameterValue;
					string OldFilter = LocationTestRecords.RowFilter;
					LocationTestRecords.RowFilter = AddToDataViewFilter(OldFilter, "TEST_TYPE_CD = '" + TestTypeCd + "' AND TEST_NUM = '" + TestNumber + "'");
					if ((LocationTestRecords.Count > 0 && CurrentLinearity["TEST_SUM_ID"] == DBNull.Value) ||
						(LocationTestRecords.Count > 1 && CurrentLinearity["TEST_SUM_ID"] != DBNull.Value) ||
						(LocationTestRecords.Count == 1 && CurrentLinearity["TEST_SUM_ID"] != DBNull.Value && CurrentLinearity["TEST_SUM_ID"].ToString() != LocationTestRecords[0]["TEST_SUM_ID"].ToString()))
					{
						Category.SetCheckParameter("Duplicate_Linearity", true, eParameterDataType.Boolean);
						Category.CheckCatalogResult = "A";
					}
					else
					{
						if (TestSumID != "")
						{
							DataView QASuppRecords = (DataView)Category.GetCheckParameter("QA_Supplemental_Data_Records").ParameterValue;
							string OldFilter2 = QASuppRecords.RowFilter;
							QASuppRecords.RowFilter = AddToDataViewFilter(OldFilter2, "TEST_NUM = '" + TestNumber + "' AND TEST_TYPE_CD = '" + TestTypeCd + "'");
							if (QASuppRecords.Count > 0 && cDBConvert.ToString(QASuppRecords[0]["TEST_SUM_ID"]) != TestSumID)
							{
								Category.SetCheckParameter("Duplicate_Linearity", true, eParameterDataType.Boolean);
								Category.CheckCatalogResult = "B";
							}
							else
								Category.SetCheckParameter("Duplicate_Linearity", false, eParameterDataType.Boolean);
							QASuppRecords.RowFilter = OldFilter2;
						}
						else
							Category.SetCheckParameter("Duplicate_Linearity", false, eParameterDataType.Boolean);
					}
					LocationTestRecords.RowFilter = OldFilter;
				}
				else
					Log = false;
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "LINEAR31");
			}

			return ReturnVal;
		}
		#endregion

		#region Linearity Check 32
		public static string LINEAR32(cCategory Category, ref bool Log) // Duplicate Linearity Summary
		{
			string ReturnVal = "";

			try
			{
				if (Convert.ToBoolean(Category.GetCheckParameter("Linearity_Gas_Level_Valid").ParameterValue))
				{
					DataRowView CurrentLinearitySummary = (DataRowView)Category.GetCheckParameter("Current_Linearity_Summary").ParameterValue;
					string GasLevelCd = cDBConvert.ToString(CurrentLinearitySummary["gas_level_cd"]);

					//CurrentLinearitySummary use different tables with different primary keys for (LINE) vs (HGLINE and HGSI3)
					//The view for Linearity_Summary_Records unions the tables and renames the pk to LIN_SUM_ID
					string pkName;
					if (CurrentLinearitySummary.DataView.Table.Columns.IndexOf("HG_TEST_SUM_ID") >= 0)
						pkName = "HG_TEST_SUM_ID";
					else
						pkName = "LIN_SUM_ID";

					DataView LinearitySummaryRecords = (DataView)Category.GetCheckParameter("Linearity_Summary_Records").ParameterValue;
					string OldFilter = LinearitySummaryRecords.RowFilter;
					LinearitySummaryRecords.RowFilter = AddToDataViewFilter(OldFilter, "GAS_LEVEL_CD = '" + GasLevelCd + "'");
					if (LinearitySummaryRecords.Count > 0)
						if ((LinearitySummaryRecords.Count > 0 && CurrentLinearitySummary[pkName] == DBNull.Value) ||
							(LinearitySummaryRecords.Count > 1 && CurrentLinearitySummary[pkName] != DBNull.Value) ||
							(LinearitySummaryRecords.Count == 1 && CurrentLinearitySummary[pkName] != DBNull.Value && CurrentLinearitySummary[pkName].ToString() != LinearitySummaryRecords[0]["LIN_SUM_ID"].ToString()))
							Category.CheckCatalogResult = "A";
					LinearitySummaryRecords.RowFilter = OldFilter;

				}
				else
					Log = false;
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "LINEAR32");
			}

			return ReturnVal;
		}
		#endregion

		#region Linearity Check 33
		public static string LINEAR33(cCategory Category, ref bool Log) // Duplicate Linearity Summary
		{
			string ReturnVal = "";

			try
			{
				Category.SetCheckParameter("Duplicate_Linearity_Injection", false, eParameterDataType.Boolean);
				if ((bool)Category.GetCheckParameter("Linearity_Gas_Level_Valid").ParameterValue == true &&
					  (bool)Category.GetCheckParameter("Linearity_Injection_Time_Valid").ParameterValue == true)
				{
					DataRowView CurrentLinearityInjection = (DataRowView)Category.GetCheckParameter("Current_Linearity_Injection").ParameterValue;
					string GasLevelCd = cDBConvert.ToString(CurrentLinearityInjection["gas_level_cd"]);
					DateTime InjectionDate = cDBConvert.ToDate(CurrentLinearityInjection["injection_date"], DateTypes.START);
					int InjectionHour = cDBConvert.ToInteger(CurrentLinearityInjection["injection_hour"]);
					int InjectionMin = cDBConvert.ToInteger(CurrentLinearityInjection["injection_min"]);
					DataView LinearityInjectionRecords = (DataView)Category.GetCheckParameter("Linearity_Injection_Records").ParameterValue;
					string OldFilter = LinearityInjectionRecords.RowFilter;
					LinearityInjectionRecords.RowFilter = AddToDataViewFilter(OldFilter,
						"gas_level_cd = '" + GasLevelCd +
						"' and injection_date = '" + InjectionDate.ToShortDateString() + "' " +
						" and injection_hour = " + InjectionHour +
						" and injection_min = " + InjectionMin);

					//CurrentLinearityInjection use different tables with different primary keys for (LINE) vs (HGLINE and HGSI3)
					//The view for Linearity_Injection_Records unions the tables and renames the pk to LIN_INJ_ID
					string pkName;
					if (CurrentLinearityInjection.DataView.Table.Columns.IndexOf("HG_TEST_INJ_ID") >= 0)
						pkName = "HG_TEST_INJ_ID";
					else
						pkName = "LIN_INJ_ID";

					if ((LinearityInjectionRecords.Count > 0 && CurrentLinearityInjection[pkName] == DBNull.Value) ||
						(LinearityInjectionRecords.Count > 1 && CurrentLinearityInjection[pkName] != DBNull.Value) ||
						(LinearityInjectionRecords.Count == 1 && CurrentLinearityInjection[pkName] != DBNull.Value && CurrentLinearityInjection[pkName].ToString() != LinearityInjectionRecords[0]["LIN_INJ_ID"].ToString()))
					{
						Category.SetCheckParameter("Duplicate_Linearity_Injection", true, eParameterDataType.Boolean);
						Category.CheckCatalogResult = "A";
					}
					LinearityInjectionRecords.RowFilter = OldFilter;
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "LINEAR33");
			}

			return ReturnVal;
		}
		#endregion

		#region Linearity Check 34
		public static string LINEAR34(cCategory Category, ref bool Log) // Extra Linearity Injections
		{
			string ReturnVal = "";

			try
			{
				{
					if (!Convert.ToBoolean(Category.GetCheckParameter("Duplicate_Linearity_Injection").ParameterValue))
					{
						DataRowView CurrentLinearityInjection = (DataRowView)Category.GetCheckParameter("Current_Linearity_Injection").ParameterValue;
						string GasLevelCd = cDBConvert.ToString(CurrentLinearityInjection["gas_level_cd"]);
						DataView LinearityInjectionRecords = (DataView)Category.GetCheckParameter("Linearity_Injection_Records").ParameterValue;
						string OldFilter = LinearityInjectionRecords.RowFilter;
						LinearityInjectionRecords.RowFilter = AddToDataViewFilter(OldFilter, "gas_level_cd = '" + GasLevelCd + "'");

						//CurrentLinearityInjection use different tables with different primary keys for (LINE) vs (HGLINE and HGSI3)
						//The view for Linearity_Injection_Records unions the tables and renames the pk to LIN_INJ_ID
						string pkName;
						if (CurrentLinearityInjection.DataView.Table.Columns.IndexOf("HG_TEST_INJ_ID") >= 0)
							pkName = "HG_TEST_INJ_ID";
						else
							pkName = "LIN_INJ_ID";

						if (CurrentLinearityInjection[pkName] == DBNull.Value)
						{
							if (LinearityInjectionRecords.Count > 2)
								Category.CheckCatalogResult = "A";
						}
						else
							if (LinearityInjectionRecords.Count > 3)
								Category.CheckCatalogResult = "A";
						LinearityInjectionRecords.RowFilter = OldFilter;
					}
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "LINEAR34");
			}

			return ReturnVal;
		}
		#endregion

		#region Linearity Check 35
		public static string LINEAR35(cCategory Category, ref bool Log)
		//Linearity Component ID Valid  
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentLinearity = (DataRowView)Category.GetCheckParameter("Current_Linearity_Test").ParameterValue;
				if (CurrentLinearity["COMPONENT_ID"] == DBNull.Value)
					Category.CheckCatalogResult = "A";
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "LINEAR35");
			}

			return ReturnVal;
		}
		#endregion

		#region Linearity Check 36
		public static string LINEAR36(cCategory Category, ref bool Log) //Initialize Linearity Summary Variables
		{
			string ReturnVal = "";

			try
			{
				Category.SetCheckParameter("Last_Injection_Time", null, eParameterDataType.String);
				Category.SetCheckParameter("Linearity_Reference_Value_Consistent_with_Span", null, eParameterDataType.String);
				Category.SetCheckParameter("Linearity_Injection_Count", 0, eParameterDataType.Integer);
				Category.SetCheckParameter("Linearity_Measured_Value_Total", 0m, eParameterDataType.Decimal);
				Category.SetCheckParameter("Linearity_Reference_Value_Total", 0m, eParameterDataType.Decimal);
				Category.SetCheckParameter("Linearity_Level_Valid", true, eParameterDataType.Boolean);
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "LINEAR36");
			}

			return ReturnVal;
		}

		#endregion

		#region Linearity Check 37
		public static string LINEAR37(cCategory Category, ref bool Log)
		//APS Indicator Valid
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentLinearitySummary = (DataRowView)Category.GetCheckParameter("Current_Linearity_Summary").ParameterValue;
				if (CurrentLinearitySummary["APS_IND"] == DBNull.Value)
					Category.CheckCatalogResult = "A";
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "LINEAR37");
			}

			return ReturnVal;
		}
		#endregion

		#region Linearity Check 38
		public static string LINEAR38(cCategory Category, ref bool Log)
		//Calculate Linearity Summary Values
		{
			string ReturnVal = "";

			try
			{
				Category.SetCheckParameter("Linearity_Calc_APS", null, eParameterDataType.Integer);
				Category.SetCheckParameter("Linearity_Calc_MMV", null, eParameterDataType.Decimal);
				Category.SetCheckParameter("Linearity_Calc_MRV", null, eParameterDataType.Decimal);
				Category.SetCheckParameter("Linearity_Calc_PE", null, eParameterDataType.Decimal);
				DataRowView CurrentLinearitySummary = (DataRowView)Category.GetCheckParameter("Current_Linearity_Summary").ParameterValue;
				string GasLvlCd = cDBConvert.ToString(CurrentLinearitySummary["GAS_LEVEL_CD"]);
				if (!GasLvlCd.InList("HIGH,LOW,MID"))
					Category.CheckCatalogResult = "A";
				else
				{
					decimal TotalRV = 0;
					decimal TotalMV = 0;
					int InjCt = 0;
					DataView InjectionRecs = (DataView)Category.GetCheckParameter("Linearity_Injection_Records").ParameterValue;
					string OldFilter = InjectionRecs.RowFilter;
					InjectionRecs.RowFilter = AddToDataViewFilter(OldFilter, "GAS_LEVEL_CD = '" + GasLvlCd + "'");
					InjectionRecs.Sort = "INJECTION_DATE DESC, INJECTION_HOUR DESC, INJECTION_MIN DESC";
					DateTime InjDate;
					int InjHour;
					int InjMin;
					decimal RefVal;
					decimal MeasVal;
					foreach (DataRowView drv in InjectionRecs)
					{
						InjDate = cDBConvert.ToDate(drv["INJECTION_DATE"], DateTypes.START);
						InjHour = cDBConvert.ToInteger(drv["INJECTION_HOUR"]);
						InjMin = cDBConvert.ToInteger(drv["INJECTION_MIN"]);
						if (InjDate == DateTime.MinValue || InjHour < 0 || 23 < InjHour || InjMin < 0 || 59 < InjMin)
						{
							Category.CheckCatalogResult = "A";
							break;
						}
						else
						{
							InjCt++;
							if (InjCt <= 3)
							{
								MeasVal = cDBConvert.ToDecimal(drv["MEASURED_VALUE"]);
								RefVal = cDBConvert.ToDecimal(drv["REF_VALUE"]);
								if (MeasVal == decimal.MinValue || RefVal < 0)
								{
									Category.CheckCatalogResult = "A";
									break;
								}
								else
								{
									TotalMV += MeasVal;
									TotalRV += RefVal;
								}
							}
						}
					}
					InjectionRecs.RowFilter = OldFilter;
					if (string.IsNullOrEmpty(Category.CheckCatalogResult))
						if (InjCt < 3)
							Category.CheckCatalogResult = "B";
						else
						{
							decimal CalcMRV = TotalRV / 3;
							decimal CalcMMV = TotalMV / 3;
							Category.SetCheckParameter("Linearity_Calc_MMV", CalcMMV, eParameterDataType.Decimal);
							decimal MeanDiff = Math.Abs(CalcMMV - CalcMRV);
							decimal CalcPE;
							if (CalcMRV == 0)
							{ 
								CalcPE = 0;
							}
							else
							{ 
								CalcPE = Math.Min((MeanDiff / CalcMRV) * 100, (decimal)9999.9);
							}

							CalcMRV = Math.Round(CalcMRV, 3, MidpointRounding.AwayFromZero);
							CalcMMV = Math.Round(CalcMMV, 3, MidpointRounding.AwayFromZero);
							Category.SetCheckParameter("Linearity_Calc_MRV", CalcMRV, eParameterDataType.Decimal);
							Category.SetCheckParameter("Linearity_Calc_MMV", CalcMMV, eParameterDataType.Decimal);
							//This inj recs view has the code, so get it from there, not current linearity rec
							string CompTypeCd = cDBConvert.ToString(InjectionRecs[0]["COMPONENT_TYPE_CD"]);
							if (CompTypeCd.InList("SO2,NOX"))
								MeanDiff = Math.Round(MeanDiff, MidpointRounding.AwayFromZero);
							else
								MeanDiff = Math.Round(MeanDiff, 1, MidpointRounding.AwayFromZero);
							CalcPE = Math.Round(CalcPE, 1, MidpointRounding.AwayFromZero);
							Category.SetCheckParameter("Linearity_Calc_PE", CalcPE, eParameterDataType.Decimal);
							if (CalcPE <= (decimal)5.0 || (CompTypeCd == "HG" && CalcPE <= (decimal)10))
								Category.SetCheckParameter("Linearity_Calc_APS", 0, eParameterDataType.Integer);
							else
							{
								if (CompTypeCd.InList("SO2,NOX"))
									if (MeanDiff <= 5)
									{
										Category.SetCheckParameter("Linearity_Calc_APS", 1, eParameterDataType.Integer);
										Category.SetCheckParameter("Linearity_Calc_PE", MeanDiff, eParameterDataType.Decimal);
									}
									else
										Category.SetCheckParameter("Linearity_Calc_APS", 0, eParameterDataType.Integer);
								else
									if (CompTypeCd.InList("CO2,O2"))
										if (MeanDiff <= (decimal)0.5)
										{
											Category.SetCheckParameter("Linearity_Calc_APS", 1, eParameterDataType.Integer);
											Category.SetCheckParameter("Linearity_Calc_PE", MeanDiff, eParameterDataType.Decimal);
										}
										else
											Category.SetCheckParameter("Linearity_Calc_APS", 0, eParameterDataType.Integer);
									else
										if (CompTypeCd == "HG")
											if (MeanDiff <= (decimal)0.8)
											{
												Category.SetCheckParameter("Linearity_Calc_APS", 1, eParameterDataType.Integer);
												Category.SetCheckParameter("Linearity_Calc_PE", MeanDiff, eParameterDataType.Decimal);
											}
											else
												Category.SetCheckParameter("Linearity_Calc_APS", 0, eParameterDataType.Integer);
							}
						}
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "LINEAR38");
			}

			return ReturnVal;
		}
	}
		#endregion
		#endregion
}
