using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;
using ECMPS.Definitions.Extensions;

namespace ECMPS.Checks.OOCChecks
{
	public class cOOCChecks : cChecks
	{
		public cOOCChecks()
		{
			CheckProcedures = new dCheckProcedure[46];

			CheckProcedures[1] = new dCheckProcedure(ONOFF1);
			CheckProcedures[2] = new dCheckProcedure(ONOFF2);
			CheckProcedures[3] = new dCheckProcedure(ONOFF3);
			CheckProcedures[4] = new dCheckProcedure(ONOFF4);
			CheckProcedures[5] = new dCheckProcedure(ONOFF5);
			CheckProcedures[6] = new dCheckProcedure(ONOFF6);
			CheckProcedures[7] = new dCheckProcedure(ONOFF7);
			CheckProcedures[8] = new dCheckProcedure(ONOFF8);
			CheckProcedures[9] = new dCheckProcedure(ONOFF9);
			CheckProcedures[10] = new dCheckProcedure(ONOFF10);
			CheckProcedures[11] = new dCheckProcedure(ONOFF11);
			CheckProcedures[12] = new dCheckProcedure(ONOFF12);
			CheckProcedures[13] = new dCheckProcedure(ONOFF13);
			CheckProcedures[14] = new dCheckProcedure(ONOFF14);
			CheckProcedures[15] = new dCheckProcedure(ONOFF15);
			CheckProcedures[16] = new dCheckProcedure(ONOFF16);
			CheckProcedures[17] = new dCheckProcedure(ONOFF17);
			CheckProcedures[18] = new dCheckProcedure(ONOFF18);
			CheckProcedures[19] = new dCheckProcedure(ONOFF19);
			CheckProcedures[20] = new dCheckProcedure(ONOFF20);
			CheckProcedures[21] = new dCheckProcedure(ONOFF21);
			CheckProcedures[22] = new dCheckProcedure(ONOFF22);
			CheckProcedures[23] = new dCheckProcedure(ONOFF23);
			CheckProcedures[24] = new dCheckProcedure(ONOFF24);
			CheckProcedures[25] = new dCheckProcedure(ONOFF25);
			CheckProcedures[26] = new dCheckProcedure(ONOFF26);
			CheckProcedures[27] = new dCheckProcedure(ONOFF27);
			CheckProcedures[28] = new dCheckProcedure(ONOFF28);
			CheckProcedures[29] = new dCheckProcedure(ONOFF29);
			CheckProcedures[30] = new dCheckProcedure(ONOFF30);
			CheckProcedures[31] = new dCheckProcedure(ONOFF31);
			CheckProcedures[32] = new dCheckProcedure(ONOFF32);
			CheckProcedures[33] = new dCheckProcedure(ONOFF33);
			CheckProcedures[34] = new dCheckProcedure(ONOFF34);
			CheckProcedures[35] = new dCheckProcedure(ONOFF35);
			CheckProcedures[36] = new dCheckProcedure(ONOFF36);
			CheckProcedures[37] = new dCheckProcedure(ONOFF37);
			CheckProcedures[38] = new dCheckProcedure(ONOFF38);
			CheckProcedures[39] = new dCheckProcedure(ONOFF39);
			CheckProcedures[40] = new dCheckProcedure(ONOFF40);
			CheckProcedures[41] = new dCheckProcedure(ONOFF41);
			CheckProcedures[42] = new dCheckProcedure(ONOFF42);
			CheckProcedures[43] = new dCheckProcedure(ONOFF43);
			CheckProcedures[44] = new dCheckProcedure(ONOFF44);
			CheckProcedures[45] = new dCheckProcedure(ONOFF45);
		}


		#region OOC Checks

		#region OOC 1-10
		#region ONOFF1
		public static string ONOFF1(cCategory Category, ref bool Log)
		//Online Offline Calibration Test Component Type Check
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentOOCTest = (DataRowView)Category.GetCheckParameter("Current_OOC_Test").ParameterValue;
				if (CurrentOOCTest["COMPONENT_ID"] == DBNull.Value)
				{
					Category.SetCheckParameter("OOC_Test_Component_Type_Valid", false, eParameterDataType.Boolean);
					Category.CheckCatalogResult = "A";
				}
				else
					if (cDBConvert.ToString(CurrentOOCTest["COMPONENT_TYPE_CD"]).InList("SO2,NOX,CO2,O2,FLOW"))
						Category.SetCheckParameter("OOC_Test_Component_Type_Valid", true, eParameterDataType.Boolean);
					else
					{
						Category.SetCheckParameter("OOC_Test_Component_Type_Valid", false, eParameterDataType.Boolean);
						Category.CheckCatalogResult = "B";
					}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "ONOFF1");
			}

			return ReturnVal;
		}
		#endregion

		#region ONOFF2
		public static string ONOFF2(cCategory Category, ref bool Log)
		//Aborted Online Offline Calibration Test Check
		{
			string ReturnVal = "";

			try
			{
				if (Convert.ToBoolean(Category.GetCheckParameter("OOC_Test_Component_Type_Valid").ParameterValue))
					Category.SetCheckParameter("Evaluate_OOC_Injections", true, eParameterDataType.Boolean);
				else
					Category.SetCheckParameter("Evaluate_OOC_Injections", false, eParameterDataType.Boolean);
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "ONOFF2");
			}

			return ReturnVal;
		}

		#endregion

		#region ONOFF3
		public static string ONOFF3(cCategory Category, ref bool Log)
		//Online Offline Calibration Test Reason Code Valid
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentOOCTest = (DataRowView)Category.GetCheckParameter("Current_OOC_Test").ParameterValue;
				string TestReasonCd = cDBConvert.ToString(CurrentOOCTest["TEST_REASON_CD"]);
				if (TestReasonCd == "")
					Category.CheckCatalogResult = "A";
				else
					if (!TestReasonCd.InList("INITIAL,DIAG"))
					{
						DataView TestReasonLookup = (DataView)Category.GetCheckParameter("Test_Reason_Code_Lookup_Table").ParameterValue;
						if (!LookupCodeExists(TestReasonCd, TestReasonLookup))
							Category.CheckCatalogResult = "B";
						else
							Category.CheckCatalogResult = "C";
					}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "ONOFF3");
			}

			return ReturnVal;
		}
		#endregion

		#region ONOFF4
		public static string ONOFF4(cCategory Category, ref bool Log)
		//Online Offline Calibration Test Upscale Gas Level Code Valid
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentOOCTest = (DataRowView)Category.GetCheckParameter("Current_OOC_Test").ParameterValue;
				string UGLCode = cDBConvert.ToString(CurrentOOCTest["UPSCALE_GAS_LEVEL_CD"]);
				if (UGLCode == "")
				{
					Category.SetCheckParameter("Upscale_OOC_Gas_Level_Valid", false, eParameterDataType.Boolean);
					Category.CheckCatalogResult = "A";
				}
				else
					if (!UGLCode.InList("MID,HIGH"))
					{
						Category.SetCheckParameter("Upscale_OOC_Gas_Level_Valid", false, eParameterDataType.Boolean);
						Category.CheckCatalogResult = "B";
					}
					else
					{
						if (UGLCode == "MID")
						{
							DataRowView CurrentComponent = (DataRowView)Category.GetCheckParameter("Current_Component").ParameterValue;
							if (cDBConvert.ToString(CurrentComponent["COMPONENT_TYPE_CD"]) == "FLOW")
							{
								Category.SetCheckParameter("Upscale_OOC_Gas_Level_Valid", false, eParameterDataType.Boolean);
								Category.CheckCatalogResult = "C";
							}
							else
								Category.SetCheckParameter("Upscale_OOC_Gas_Level_Valid", true, eParameterDataType.Boolean);
						}
						else
							Category.SetCheckParameter("Upscale_OOC_Gas_Level_Valid", true, eParameterDataType.Boolean);
					}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "ONOFF4");
			}

			return ReturnVal;
		}


		#endregion

		#region ONOFF5
		public static string ONOFF5(cCategory Category, ref bool Log)
		//Offline Upscale Injection Time Valid
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentOOCTest = (DataRowView)Category.GetCheckParameter("Current_OOC_Test").ParameterValue;
				int OffUpsInjHour = cDBConvert.ToInteger(CurrentOOCTest["OFFLINE_UPSCALE_INJECTION_HOUR"]);
				if (CurrentOOCTest["OFFLINE_UPSCALE_INJECTION_DATE"] == DBNull.Value || OffUpsInjHour == int.MinValue ||
					OffUpsInjHour < 0 || 23 < OffUpsInjHour)
				{
					Category.SetCheckParameter("OOC_Injection_Times_Valid", false, eParameterDataType.Boolean);
					Category.CheckCatalogResult = "A";
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "ONOFF5");
			}

			return ReturnVal;
		}

		#endregion

		#region ONOFF6
		public static string ONOFF6(cCategory Category, ref bool Log)
		//Offline Upscale Measured Value Valid
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentOOCTest = (DataRowView)Category.GetCheckParameter("Current_OOC_Test").ParameterValue;
				decimal OffUpsMeasVal = cDBConvert.ToDecimal(CurrentOOCTest["OFFLINE_UPSCALE_MEASURED_VALUE"]);
				if (OffUpsMeasVal == decimal.MinValue)
					Category.CheckCatalogResult = "A";
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "ONOFF6");
			}

			return ReturnVal;
		}

		#endregion

		#region ONOFF7
		public static string ONOFF7(cCategory Category, ref bool Log)
		//Offline Upscale Reference Value Valid
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentOOCTest = (DataRowView)Category.GetCheckParameter("Current_OOC_Test").ParameterValue;
				decimal OffUpsRefVal = cDBConvert.ToDecimal(CurrentOOCTest["OFFLINE_UPSCALE_REF_VALUE"]);
				if (OffUpsRefVal == decimal.MinValue)
					Category.CheckCatalogResult = "A";
				else
					if (OffUpsRefVal <= 0)
						Category.CheckCatalogResult = "B";
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "ONOFF7");
			}

			return ReturnVal;
		}

		#endregion

		#region ONOFF8
		public static string ONOFF8(cCategory Category, ref bool Log)
		//Offline Upscale Calibration Error Valid
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentOOCTest = (DataRowView)Category.GetCheckParameter("Current_OOC_Test").ParameterValue;
				decimal OffUpsCalError = cDBConvert.ToDecimal(CurrentOOCTest["OFFLINE_UPSCALE_CAL_ERROR"]);
				if (OffUpsCalError == decimal.MinValue)
					Category.CheckCatalogResult = "A";
				else
					if (OffUpsCalError < 0)
						Category.CheckCatalogResult = "B";
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "ONOFF8");
			}

			return ReturnVal;
		}

		#endregion

		#region ONOFF9
		public static string ONOFF9(cCategory Category, ref bool Log)
		//Offline Zero Injection Time Valid
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentOOCTest = (DataRowView)Category.GetCheckParameter("Current_OOC_Test").ParameterValue;
				int OffZeroInjHour = cDBConvert.ToInteger(CurrentOOCTest["OFFLINE_ZERO_INJECTION_HOUR"]);
				if (CurrentOOCTest["OFFLINE_ZERO_INJECTION_DATE"] == DBNull.Value || OffZeroInjHour == int.MinValue ||
					OffZeroInjHour < 0 || 23 < OffZeroInjHour)
				{
					Category.SetCheckParameter("OOC_Injection_Times_Valid", false, eParameterDataType.Boolean);
					Category.CheckCatalogResult = "A";
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "ONOFF9");
			}

			return ReturnVal;
		}

		#endregion

		#region ONOFF10
		public static string ONOFF10(cCategory Category, ref bool Log)
		//Offline Zero Measured Value Valid
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentOOCTest = (DataRowView)Category.GetCheckParameter("Current_OOC_Test").ParameterValue;
				decimal OffZeroMeasVal = cDBConvert.ToDecimal(CurrentOOCTest["OFFLINE_ZERO_MEASURED_VALUE"]);
				if (OffZeroMeasVal == decimal.MinValue)
					Category.CheckCatalogResult = "A";
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "ONOFF10");
			}

			return ReturnVal;
		}

		#endregion
		#endregion

		#region OOC 11-20
		#region ONOFF11
		public static string ONOFF11(cCategory Category, ref bool Log)
		//Offline Zero Reference Value Valid
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentOOCTest = (DataRowView)Category.GetCheckParameter("Current_OOC_Test").ParameterValue;
				decimal OffZeroRefVal = cDBConvert.ToDecimal(CurrentOOCTest["OFFLINE_ZERO_REF_VALUE"]);
				if (OffZeroRefVal == decimal.MinValue)
					Category.CheckCatalogResult = "A";
				else
					if (OffZeroRefVal < 0)
						Category.CheckCatalogResult = "B";
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "ONOFF11");
			}

			return ReturnVal;
		}

		#endregion

		#region ONOFF12
		public static string ONOFF12(cCategory Category, ref bool Log)
		//Offline Zero Calibration Error Valid
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentOOCTest = (DataRowView)Category.GetCheckParameter("Current_OOC_Test").ParameterValue;
				decimal OffZeroCalError = cDBConvert.ToDecimal(CurrentOOCTest["OFFLINE_ZERO_CAL_ERROR"]);
				if (OffZeroCalError == decimal.MinValue)
					Category.CheckCatalogResult = "A";
				else
					if (OffZeroCalError < 0)
						Category.CheckCatalogResult = "B";
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "ONOFF12");
			}

			return ReturnVal;
		}

		#endregion

		#region ONOFF13
		public static string ONOFF13(cCategory Category, ref bool Log)
		//Online Upscale Injection Time Valid
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentOOCTest = (DataRowView)Category.GetCheckParameter("Current_OOC_Test").ParameterValue;
				int OnUpsInjHour = cDBConvert.ToInteger(CurrentOOCTest["ONLINE_UPSCALE_INJECTION_HOUR"]);
				if (CurrentOOCTest["ONLINE_UPSCALE_INJECTION_DATE"] == DBNull.Value || OnUpsInjHour == int.MinValue ||
					OnUpsInjHour < 0 || 23 < OnUpsInjHour)
				{
					Category.SetCheckParameter("OOC_Injection_Times_Valid", false, eParameterDataType.Boolean);
					Category.CheckCatalogResult = "A";
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "ONOFF13");
			}

			return ReturnVal;
		}

		#endregion

		#region ONOFF14
		public static string ONOFF14(cCategory Category, ref bool Log)
		//Online Upscale Measured Value Valid
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentOOCTest = (DataRowView)Category.GetCheckParameter("Current_OOC_Test").ParameterValue;
				decimal OnUpsMeasVal = cDBConvert.ToDecimal(CurrentOOCTest["ONLINE_UPSCALE_MEASURED_VALUE"]);
				if (OnUpsMeasVal == decimal.MinValue)
					Category.CheckCatalogResult = "A";
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "ONOFF14");
			}

			return ReturnVal;
		}

		#endregion

		#region ONOFF15
		public static string ONOFF15(cCategory Category, ref bool Log)
		//Online Upscale Reference Value Valid
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentOOCTest = (DataRowView)Category.GetCheckParameter("Current_OOC_Test").ParameterValue;
				decimal OnUpsRefVal = cDBConvert.ToDecimal(CurrentOOCTest["ONLINE_UPSCALE_REF_VALUE"]);
				if (OnUpsRefVal == decimal.MinValue)
					Category.CheckCatalogResult = "A";
				else
					if (OnUpsRefVal <= 0)
						Category.CheckCatalogResult = "B";
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "ONOFF15");
			}

			return ReturnVal;
		}

		#endregion

		#region ONOFF16
		public static string ONOFF16(cCategory Category, ref bool Log)
		//Online Upscale Calibration Error Valid
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentOOCTest = (DataRowView)Category.GetCheckParameter("Current_OOC_Test").ParameterValue;
				decimal OnUpsCalError = cDBConvert.ToDecimal(CurrentOOCTest["ONLINE_UPSCALE_CAL_ERROR"]);
				if (OnUpsCalError == decimal.MinValue)
					Category.CheckCatalogResult = "A";
				else
					if (OnUpsCalError < 0)
						Category.CheckCatalogResult = "B";
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "ONOFF16");
			}

			return ReturnVal;
		}

		#endregion

		#region ONOFF17
		public static string ONOFF17(cCategory Category, ref bool Log)
		//Online Zero Injection Time Valid
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentOOCTest = (DataRowView)Category.GetCheckParameter("Current_OOC_Test").ParameterValue;
				int OnZeroInjHour = cDBConvert.ToInteger(CurrentOOCTest["ONLINE_ZERO_INJECTION_HOUR"]);
				if (CurrentOOCTest["ONLINE_ZERO_INJECTION_DATE"] == DBNull.Value || OnZeroInjHour == int.MinValue ||
					OnZeroInjHour < 0 || 23 < OnZeroInjHour)
				{
					Category.SetCheckParameter("OOC_Injection_Times_Valid", false, eParameterDataType.Boolean);
					Category.CheckCatalogResult = "A";
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "ONOFF17");
			}

			return ReturnVal;
		}

		#endregion

		#region ONOFF18
		public static string ONOFF18(cCategory Category, ref bool Log)
		//Online Zero Measured Value Valid
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentOOCTest = (DataRowView)Category.GetCheckParameter("Current_OOC_Test").ParameterValue;
				decimal OnZeroMeasVal = cDBConvert.ToDecimal(CurrentOOCTest["ONLINE_ZERO_MEASURED_VALUE"]);
				if (OnZeroMeasVal == decimal.MinValue)
					Category.CheckCatalogResult = "A";
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "ONOFF18");
			}

			return ReturnVal;
		}

		#endregion

		#region ONOFF19
		public static string ONOFF19(cCategory Category, ref bool Log)
		//Online Zero Reference Value Valid
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentOOCTest = (DataRowView)Category.GetCheckParameter("Current_OOC_Test").ParameterValue;
				decimal OnZeroRefVal = cDBConvert.ToDecimal(CurrentOOCTest["ONLINE_ZERO_REF_VALUE"]);
				if (OnZeroRefVal == decimal.MinValue)
					Category.CheckCatalogResult = "A";
				else
					if (OnZeroRefVal < 0)
						Category.CheckCatalogResult = "B";
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "ONOFF19");
			}

			return ReturnVal;
		}

		#endregion

		#region ONOFF20
		public static string ONOFF20(cCategory Category, ref bool Log)
		//Online Zero Calibration Error Valid
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentOOCTest = (DataRowView)Category.GetCheckParameter("Current_OOC_Test").ParameterValue;
				decimal OnZeroCalError = cDBConvert.ToDecimal(CurrentOOCTest["ONLINE_ZERO_CAL_ERROR"]);
				if (OnZeroCalError == decimal.MinValue)
					Category.CheckCatalogResult = "A";
				else
					if (OnZeroCalError < 0)
						Category.CheckCatalogResult = "B";
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "ONOFF20");
			}

			return ReturnVal;
		}

		#endregion
		#endregion

		#region OOC 21-30
		#region ONOFF21
		public static string ONOFF21(cCategory Category, ref bool Log)
		//Reference Values Consistent with Calibration Gas Levels
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentOOCTest = (DataRowView)Category.GetCheckParameter("Current_OOC_Test").ParameterValue;
				decimal OnZeroRefVal = cDBConvert.ToDecimal(CurrentOOCTest["ONLINE_ZERO_REF_VALUE"]);
				decimal OffZeroRefVal = cDBConvert.ToDecimal(CurrentOOCTest["OFFLINE_ZERO_REF_VALUE"]);
				decimal OnUpsRefVal = cDBConvert.ToDecimal(CurrentOOCTest["ONLINE_UPSCALE_REF_VALUE"]);
				decimal OffUpsRefVal = cDBConvert.ToDecimal(CurrentOOCTest["OFFLINE_UPSCALE_REF_VALUE"]);
				if (OnZeroRefVal >= 0 && OffZeroRefVal >= 0 && OnUpsRefVal > 0 && OffUpsRefVal > 0)
					if (Math.Max(OnZeroRefVal, OffZeroRefVal) >= Math.Min(OnUpsRefVal, OffUpsRefVal))
					{
						Category.SetCheckParameter("OOC_Test_Calc_Result", "INVALID", eParameterDataType.Boolean);
						Category.CheckCatalogResult = "A";
					}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "ONOFF21");
			}

			return ReturnVal;
		}

		#endregion

		#region ONOFF22
		public static string ONOFF22(cCategory Category, ref bool Log)
		//Online Offline Calibration Test Injection Sequence Valid
		{
			string ReturnVal = "";

			try
			{
				if (Convert.ToBoolean(Category.GetCheckParameter("OOC_Injection_Times_Valid").ParameterValue))
				{
					DataRowView CurrentOOCTest = (DataRowView)Category.GetCheckParameter("Current_OOC_Test").ParameterValue;
					int OffZeroInjHour = cDBConvert.ToHour(CurrentOOCTest["OFFLINE_ZERO_INJECTION_HOUR"], DateTypes.START);
					int OffUpsInjHour = cDBConvert.ToHour(CurrentOOCTest["OFFLINE_UPSCALE_INJECTION_HOUR"], DateTypes.START);
					DateTime OffZeroInjDate = cDBConvert.ToDate(CurrentOOCTest["OFFLINE_ZERO_INJECTION_DATE"], DateTypes.START);
					DateTime OffUpsInjDate = cDBConvert.ToDate(CurrentOOCTest["OFFLINE_UPSCALE_INJECTION_DATE"], DateTypes.START);
					int OnZeroInjHour = cDBConvert.ToHour(CurrentOOCTest["ONLINE_ZERO_INJECTION_HOUR"], DateTypes.START);
					int OnUpsInjHour = cDBConvert.ToHour(CurrentOOCTest["ONLINE_UPSCALE_INJECTION_HOUR"], DateTypes.START);
					DateTime OnZeroInjDate = cDBConvert.ToDate(CurrentOOCTest["ONLINE_ZERO_INJECTION_DATE"], DateTypes.START);
					DateTime OnUpsInjDate = cDBConvert.ToDate(CurrentOOCTest["ONLINE_UPSCALE_INJECTION_DATE"], DateTypes.START);
					DateTime OffZero = DateTime.Parse(string.Format("{0} {1}:00", OffZeroInjDate.ToShortDateString(), OffZeroInjHour));
					DateTime OffUps = DateTime.Parse(string.Format("{0} {1}:00", OffUpsInjDate.ToShortDateString(), OffUpsInjHour));
					DateTime OnZero = DateTime.Parse(string.Format("{0} {1}:00", OnZeroInjDate.ToShortDateString(), OnZeroInjHour));
					DateTime OnUps = DateTime.Parse(string.Format("{0} {1}:00", OnUpsInjDate.ToShortDateString(), OnUpsInjHour));
					DateTime OffMax = OffUps;
					DateTime OnMin = OnZero;
					DateTime OffMin = OffZero;
					DateTime OnMax = OnUps;
					if (DateTime.Compare(OffZero, OffUps) > 0)
					{
						OffMax = OffZero;
						OffMin = OffUps;
					}
					if (DateTime.Compare(OnZero, OnUps) > 0)
					{
						OnMin = OnUps;
						OnMax = OnZero;
					}
					if (!(DateTime.Compare(OffMax, OnMin) < 0))
						Category.CheckCatalogResult = "A";
					else
					{
						TimeSpan ts = OnMax - OffMin;
						int DiffInHours = (int)ts.TotalHours;
						if (DiffInHours > 26)
							Category.CheckCatalogResult = "B";
					}
				}
				else
					Log = false;
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "ONOFF22");
			}

			return ReturnVal;
		}

		#endregion

		#region ONOFF23
		public static string ONOFF23(cCategory Category, ref bool Log)
		//Identification of Previously Reported Test or Test Number for Online Offline Calibration Test
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentOOCTest = (DataRowView)Category.GetCheckParameter("Current_OOC_Test").ParameterValue;
				string CompID = cDBConvert.ToString(CurrentOOCTest["COMPONENT_ID"]);
				if (Convert.ToBoolean(Category.GetCheckParameter("Test_End_Date_Valid").ParameterValue) &&
					Convert.ToBoolean(Category.GetCheckParameter("Test_End_Hour_Valid").ParameterValue) &&
					Convert.ToBoolean(Category.GetCheckParameter("Test_Span_Scale_Valid").ParameterValue))
				{
					string SpanScaleCd = cDBConvert.ToString(CurrentOOCTest["SPAN_SCALE_CD"]);
					DateTime EndDate = cDBConvert.ToDate(CurrentOOCTest["END_DATE"], DateTypes.END);
					int EndHour = cDBConvert.ToInteger(CurrentOOCTest["END_HOUR"]);
					string TestNum = cDBConvert.ToString(CurrentOOCTest["TEST_NUM"]);
					DataView OOCTestRecs = (DataView)Category.GetCheckParameter("OOC_Test_Records").ParameterValue;
					string OldFilter = OOCTestRecs.RowFilter;
					if (SpanScaleCd != "")
						OOCTestRecs.RowFilter = AddToDataViewFilter(OldFilter, "SPAN_SCALE_CD = '" + SpanScaleCd + "'" + " AND TEST_NUM <> '" + TestNum + "'" +
							" AND END_DATE = '" + EndDate.ToShortDateString() + "'" + " AND END_HOUR = " + EndHour);
					else
						OOCTestRecs.RowFilter = AddToDataViewFilter(OldFilter, "SPAN_SCALE_CD IS NULL AND TEST_NUM <> '" + TestNum + "'" +
							" AND END_DATE = '" + EndDate.ToShortDateString() + "'" + " AND END_HOUR = " + EndHour);
					if ((OOCTestRecs.Count > 0 && CurrentOOCTest["TEST_SUM_ID"] == DBNull.Value) ||
						(OOCTestRecs.Count > 1 && CurrentOOCTest["TEST_SUM_ID"] != DBNull.Value) ||
						(OOCTestRecs.Count == 1 && CurrentOOCTest["TEST_SUM_ID"] != DBNull.Value && CurrentOOCTest["TEST_SUM_ID"].ToString() != OOCTestRecs[0]["TEST_SUM_ID"].ToString()))
						Category.CheckCatalogResult = "A";
					else
					{
						DataView QASuppRecs = (DataView)Category.GetCheckParameter("QA_Supplemental_Data_Records").ParameterValue;
						string OldFilter2 = QASuppRecs.RowFilter;
						if (SpanScaleCd != "")
							QASuppRecs.RowFilter = AddToDataViewFilter(OldFilter2, "TEST_TYPE_CD = 'ONOFF' AND COMPONENT_ID = '" + CompID + "'" +
								" AND SPAN_SCALE = '" + SpanScaleCd + "'" + " AND END_DATE = '" + EndDate.ToShortDateString() + "'" +
								" AND END_HOUR = " + EndHour + " AND TEST_NUM <> '" + TestNum + "'");
						else
							QASuppRecs.RowFilter = AddToDataViewFilter(OldFilter2, "TEST_TYPE_CD = 'ONOFF' AND COMPONENT_ID = '" + CompID + "'" +
								" AND SPAN_SCALE IS NULL AND END_DATE = '" + EndDate.ToShortDateString() + "'" +
								" AND END_HOUR = " + EndHour + " AND TEST_NUM <> '" + TestNum + "'");
						if ((QASuppRecs.Count > 0 && CurrentOOCTest["TEST_SUM_ID"] == DBNull.Value) ||
							(QASuppRecs.Count > 1 && CurrentOOCTest["TEST_SUM_ID"] != DBNull.Value) ||
							(QASuppRecs.Count == 1 && CurrentOOCTest["TEST_SUM_ID"] != DBNull.Value && CurrentOOCTest["TEST_SUM_ID"].ToString() != QASuppRecs[0]["TEST_SUM_ID"].ToString()))
							Category.CheckCatalogResult = "A";
						else
						{
							QASuppRecs.RowFilter = AddToDataViewFilter(OldFilter2, "TEST_TYPE_CD = 'ONOFF' AND TEST_NUM = '" + TestNum + "'");
							if (QASuppRecs.Count > 0)
								if (cDBConvert.ToString(((DataRowView)QASuppRecs[0])["CAN_SUBMIT"]) == "N")
								{
									if (SpanScaleCd != "")
										QASuppRecs.RowFilter = AddToDataViewFilter(OldFilter2, "TEST_TYPE_CD = 'ONOFF' AND " +
										  "TEST_NUM = '" + TestNum + "'" + " AND (COMPONENT_ID <> '" + CompID +
										  "' OR SPAN_SCALE <> '" + SpanScaleCd + "'" + " OR END_DATE <> '" +
										  EndDate.ToShortDateString() + "'" + " OR END_HOUR <> " + EndHour + ")");
									else
										QASuppRecs.RowFilter = AddToDataViewFilter(OldFilter2, "TEST_TYPE_CD = 'ONOFF' AND " +
										  "TEST_NUM = '" + TestNum + "'" + " AND (COMPONENT_ID <> '" + CompID +
										  "' OR SPAN_SCALE IS NOT NULL OR END_DATE <> '" +
										  EndDate.ToShortDateString() + "'" + " OR END_HOUR <> " + EndHour + ")");
									if ((QASuppRecs.Count > 0 && CurrentOOCTest["TEST_SUM_ID"] == DBNull.Value) ||
										(QASuppRecs.Count > 1 && CurrentOOCTest["TEST_SUM_ID"] != DBNull.Value) ||
										(QASuppRecs.Count == 1 && CurrentOOCTest["TEST_SUM_ID"] != DBNull.Value && CurrentOOCTest["TEST_SUM_ID"].ToString() != QASuppRecs[0]["TEST_SUM_ID"].ToString()))
										Category.CheckCatalogResult = "B";
									else
										Category.CheckCatalogResult = "C";
								}
						}
						QASuppRecs.RowFilter = OldFilter2;
					}
					OOCTestRecs.RowFilter = OldFilter;
				}
				else
					Log = false;
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "ONOFF23");
			}

			return ReturnVal;
		}

		#endregion

		#region ONOFF24
		public static string ONOFF24(cCategory Category, ref bool Log)
		//Online Offline Calibration Test Begin Time Valid
		{
			string ReturnVal = "";

			try
			{
				if (Convert.ToBoolean(Category.GetCheckParameter("OOC_Injection_Times_Valid").ParameterValue) &&
					Convert.ToBoolean(Category.GetCheckParameter("Test_Begin_Date_Valid").ParameterValue) &&
					Convert.ToBoolean(Category.GetCheckParameter("Test_Begin_Hour_Valid").ParameterValue))
				{
					DataRowView CurrentOOCTest = (DataRowView)Category.GetCheckParameter("Current_OOC_Test").ParameterValue;
					int BeginHour = cDBConvert.ToInteger(CurrentOOCTest["BEGIN_HOUR"]);
					DateTime BeginDate = cDBConvert.ToDate(CurrentOOCTest["BEGIN_DATE"], DateTypes.START);
					DateTime BeginDateTime = DateTime.Parse(string.Format("{0} {1}:00", BeginDate.ToShortDateString(), BeginHour));
					int OffZeroInjHour = cDBConvert.ToInteger(CurrentOOCTest["OFFLINE_ZERO_INJECTION_HOUR"]);
					int OffUpsInjHour = cDBConvert.ToInteger(CurrentOOCTest["OFFLINE_UPSCALE_INJECTION_HOUR"]);
					DateTime OffZeroInjDate = cDBConvert.ToDate(CurrentOOCTest["OFFLINE_ZERO_INJECTION_DATE"], DateTypes.START);
					DateTime OffUpsInjDate = cDBConvert.ToDate(CurrentOOCTest["OFFLINE_UPSCALE_INJECTION_DATE"], DateTypes.START);
					int OnZeroInjHour = cDBConvert.ToInteger(CurrentOOCTest["ONLINE_ZERO_INJECTION_HOUR"]);
					int OnUpsInjHour = cDBConvert.ToInteger(CurrentOOCTest["ONLINE_UPSCALE_INJECTION_HOUR"]);
					DateTime OnZeroInjDate = cDBConvert.ToDate(CurrentOOCTest["ONLINE_ZERO_INJECTION_DATE"], DateTypes.START);
					DateTime OnUpsInjDate = cDBConvert.ToDate(CurrentOOCTest["ONLINE_UPSCALE_INJECTION_DATE"], DateTypes.START);
					DateTime OffZero = DateTime.Parse(string.Format("{0} {1}:00", OffZeroInjDate.ToShortDateString(), OffZeroInjHour));
					DateTime OffUps = DateTime.Parse(string.Format("{0} {1}:00", OffUpsInjDate.ToShortDateString(), OffUpsInjHour));
					DateTime OnZero = DateTime.Parse(string.Format("{0} {1}:00", OnZeroInjDate.ToShortDateString(), OnZeroInjHour));
					DateTime OnUps = DateTime.Parse(string.Format("{0} {1}:00", OnUpsInjDate.ToShortDateString(), OnUpsInjHour));
					DateTime InjMin = OffZero;
					if (OffUps < InjMin)
						InjMin = OffUps;
					if (OnZero < InjMin)
						InjMin = OnZero;
					if (OnUps < InjMin)
						InjMin = OnUps;
					if (BeginDateTime != InjMin)
						Category.CheckCatalogResult = "A";
				}
				else
					Log = false;
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "ONOFF24");
			}

			return ReturnVal;
		}

		#endregion

		#region ONOFF25
		public static string ONOFF25(cCategory Category, ref bool Log)
		//Online Offline Calibration Test End Time Valid
		{
			string ReturnVal = "";

			try
			{
				if (Convert.ToBoolean(Category.GetCheckParameter("OOC_Injection_Times_Valid").ParameterValue) &&
					Convert.ToBoolean(Category.GetCheckParameter("Test_End_Date_Valid").ParameterValue) &&
					Convert.ToBoolean(Category.GetCheckParameter("Test_End_Hour_Valid").ParameterValue))
				{
					DataRowView CurrentOOCTest = (DataRowView)Category.GetCheckParameter("Current_OOC_Test").ParameterValue;
					int EndHour = cDBConvert.ToInteger(CurrentOOCTest["END_HOUR"]);
					DateTime EndDate = cDBConvert.ToDate(CurrentOOCTest["END_DATE"], DateTypes.END);
					DateTime EndDateTime = DateTime.Parse(string.Format("{0} {1}:00", EndDate.ToShortDateString(), EndHour));
					int OffZeroInjHour = cDBConvert.ToInteger(CurrentOOCTest["OFFLINE_ZERO_INJECTION_HOUR"]);
					int OffUpsInjHour = cDBConvert.ToInteger(CurrentOOCTest["OFFLINE_UPSCALE_INJECTION_HOUR"]);
					DateTime OffZeroInjDate = cDBConvert.ToDate(CurrentOOCTest["OFFLINE_ZERO_INJECTION_DATE"], DateTypes.END);
					DateTime OffUpsInjDate = cDBConvert.ToDate(CurrentOOCTest["OFFLINE_UPSCALE_INJECTION_DATE"], DateTypes.END);
					int OnZeroInjHour = cDBConvert.ToInteger(CurrentOOCTest["ONLINE_ZERO_INJECTION_HOUR"]);
					int OnUpsInjHour = cDBConvert.ToInteger(CurrentOOCTest["ONLINE_UPSCALE_INJECTION_HOUR"]);
					DateTime OnZeroInjDate = cDBConvert.ToDate(CurrentOOCTest["ONLINE_ZERO_INJECTION_DATE"], DateTypes.END);
					DateTime OnUpsInjDate = cDBConvert.ToDate(CurrentOOCTest["ONLINE_UPSCALE_INJECTION_DATE"], DateTypes.END);
					DateTime OffZero = DateTime.Parse(string.Format("{0} {1}:00", OffZeroInjDate.ToShortDateString(), OffZeroInjHour));
					DateTime OffUps = DateTime.Parse(string.Format("{0} {1}:00", OffUpsInjDate.ToShortDateString(), OffUpsInjHour));
					DateTime OnZero = DateTime.Parse(string.Format("{0} {1}:00", OnZeroInjDate.ToShortDateString(), OnZeroInjHour));
					DateTime OnUps = DateTime.Parse(string.Format("{0} {1}:00", OnUpsInjDate.ToShortDateString(), OnUpsInjHour));
					DateTime InjMax = OffZero;
					if (OffUps > InjMax)
						InjMax = OffUps;
					if (OnZero > InjMax)
						InjMax = OnZero;
					if (OnUps > InjMax)
						InjMax = OnUps;
					if (EndDateTime != InjMax)
						Category.CheckCatalogResult = "A";
				}
				else
					Log = false;
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "ONOFF25");
			}

			return ReturnVal;
		}

		#endregion

		#region ONOFF26
		public static string ONOFF26(cCategory Category, ref bool Log)
		//Upscale Reference Values Consistent with Span
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentOOCTest = (DataRowView)Category.GetCheckParameter("Current_OOC_Test").ParameterValue;
				decimal OffUpsRefVal = cDBConvert.ToDecimal(CurrentOOCTest["OFFLINE_UPSCALE_REF_VALUE"]);
				decimal OnUpsRefVal = cDBConvert.ToDecimal(CurrentOOCTest["ONLINE_UPSCALE_REF_VALUE"]);
				if (Category.GetCheckParameter("Test_Span_Value").ParameterValue != null && OffUpsRefVal > 0 && OnUpsRefVal > 0)
				{
					decimal TestSpanVal = Convert.ToDecimal(Category.GetCheckParameter("Test_Span_Value").ParameterValue);
					decimal UpscaleRefPercOfSpan = Math.Round((100 * Math.Max(OffUpsRefVal, OnUpsRefVal) / TestSpanVal), 1, MidpointRounding.AwayFromZero);
					Category.SetCheckParameter("OOC_Upscale_Reference_Value_Percent_of_Span", UpscaleRefPercOfSpan, eParameterDataType.Decimal);
					string UpscaleGasLevelCd = cDBConvert.ToString(CurrentOOCTest["UPSCALE_GAS_LEVEL_CD"]);
					string CompTypeCd = cDBConvert.ToString(CurrentOOCTest["COMPONENT_TYPE_CD"]);
					bool NonCritical = false;

					DataView TestToleranceRecords = (DataView)Category.GetCheckParameter("Test_Tolerances_Cross_Check_Table").ParameterValue;
					string OldFilter = TestToleranceRecords.RowFilter;
					TestToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, "FieldDescription = '" + "GasPercentOfSpan'");
					decimal Tolerance = cDBConvert.ToDecimal(((DataRowView)TestToleranceRecords[0])["Tolerance"]);
					TestToleranceRecords.RowFilter = OldFilter;
					decimal TempVal = Math.Round((100 * Math.Min(OnUpsRefVal, OffUpsRefVal) / TestSpanVal), 1, MidpointRounding.AwayFromZero);

					if (UpscaleGasLevelCd == "MID" && CompTypeCd != "FLOW")
					{
						if (UpscaleRefPercOfSpan < (decimal)50.0 || UpscaleRefPercOfSpan > (decimal)60.0)
							if (UpscaleRefPercOfSpan < ((decimal)50.0 - Tolerance) || UpscaleRefPercOfSpan > ((decimal)60.0 + Tolerance))
								Category.CheckCatalogResult = "A";
							else
								NonCritical = true;
						if (string.IsNullOrEmpty(Category.CheckCatalogResult) && OffUpsRefVal != OnUpsRefVal)
							if (TempVal < ((decimal)50.0 - Tolerance) || TempVal > ((decimal)60.0 + Tolerance))
							{
								Category.SetCheckParameter("OOC_Upscale_Reference_Value_Percent_of_Span", TempVal, eParameterDataType.Decimal);
								Category.CheckCatalogResult = "A";
							}
							else
								if (TempVal < (decimal)50.0 || TempVal > (decimal)60.0)
								{
									Category.SetCheckParameter("OOC_Upscale_Reference_Value_Percent_of_Span", TempVal, eParameterDataType.Decimal);
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
								if (string.IsNullOrEmpty(Category.CheckCatalogResult) && OffUpsRefVal != OnUpsRefVal)
									if (TempVal < ((decimal)50.0 - Tolerance) || TempVal > ((decimal)70.0 + Tolerance))
									{
										Category.SetCheckParameter("OOC_Upscale_Reference_Value_Percent_of_Span", TempVal, eParameterDataType.Decimal);
										Category.CheckCatalogResult = "C";
									}
									else
										if (TempVal < (decimal)50.0 || TempVal > (decimal)70.0)
										{
											Category.SetCheckParameter("OOC_Upscale_Reference_Value_Percent_of_Span", TempVal, eParameterDataType.Decimal);
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
									if (string.IsNullOrEmpty(Category.CheckCatalogResult) && OffUpsRefVal != OnUpsRefVal)
										if (TempVal < ((decimal)80.0 - Tolerance) || TempVal > (decimal)100.0)
										{
											Category.SetCheckParameter("OOC_Upscale_Reference_Value_Percent_of_Span", TempVal, eParameterDataType.Decimal);
											Category.CheckCatalogResult = "E";
										}
										else
											if (TempVal < (decimal)80.0)
											{
												Category.SetCheckParameter("OOC_Upscale_Reference_Value_Percent_of_Span", TempVal, eParameterDataType.Decimal);
												NonCritical = true;
											}
									if (string.IsNullOrEmpty(Category.CheckCatalogResult) && NonCritical)
										Category.CheckCatalogResult = "F";
								}
					}
				}
				else
					Log = false;
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "ONOFF26");
			}

			return ReturnVal;
		}

		#endregion

		#region ONOFF27
		public static string ONOFF27(cCategory Category, ref bool Log)
		//Zero Reference Values Consistent with Span
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentOOCTest = (DataRowView)Category.GetCheckParameter("Current_OOC_Test").ParameterValue;
				decimal OffZeroRefVal = cDBConvert.ToDecimal(CurrentOOCTest["OFFLINE_ZERO_REF_VALUE"]);
				decimal OnZeroRefVal = cDBConvert.ToDecimal(CurrentOOCTest["ONLINE_ZERO_REF_VALUE"]);
				if (Category.GetCheckParameter("Test_Span_Value").ParameterValue != null && OffZeroRefVal >= 0 && OnZeroRefVal >= 0)
				{
					decimal TestSpanVal = Convert.ToDecimal(Category.GetCheckParameter("Test_Span_Value").ParameterValue);
					decimal ZeroRefPercOfSpan = Math.Round((100 * Math.Max(OffZeroRefVal, OnZeroRefVal) / TestSpanVal), 1, MidpointRounding.AwayFromZero);
					Category.SetCheckParameter("OOC_Zero_Reference_Value_Percent_of_Span", ZeroRefPercOfSpan, eParameterDataType.Decimal);
					if (ZeroRefPercOfSpan > (decimal)20.0)
					{
						DataView TestToleranceRecords = (DataView)Category.GetCheckParameter("Test_Tolerances_Cross_Check_Table").ParameterValue;
						string OldFilter = TestToleranceRecords.RowFilter;
						TestToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, "FieldDescription = '" + "GasPercentOfSpan'");
						decimal Tolerance = cDBConvert.ToDecimal(((DataRowView)TestToleranceRecords[0])["Tolerance"]);
						TestToleranceRecords.RowFilter = OldFilter;
						if (ZeroRefPercOfSpan > ((decimal)20.0 + Tolerance))
							Category.CheckCatalogResult = "A";
						else
							Category.CheckCatalogResult = "B";
					}
				}
				else
					Log = false;
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "ONOFF27");
			}

			return ReturnVal;
		}

		#endregion

		#region ONOFF28
		public static string ONOFF28(cCategory Category, ref bool Log)
		//Calculate Offline Upscale Gas Injection or Reference Signal Results
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentOOCTest = (DataRowView)Category.GetCheckParameter("Current_OOC_Test").ParameterValue;
				decimal OffUpsRefVal = cDBConvert.ToDecimal(CurrentOOCTest["OFFLINE_UPSCALE_REF_VALUE"]);
				decimal OffUpsMeasVal = cDBConvert.ToDecimal(CurrentOOCTest["OFFLINE_UPSCALE_MEASURED_VALUE"]);
				decimal OffUpsCalError = cDBConvert.ToDecimal(CurrentOOCTest["OFFLINE_UPSCALE_CAL_ERROR"]);
				if (Category.GetCheckParameter("Test_Span_Value").ParameterValue == null ||
					!Convert.ToBoolean(Category.GetCheckParameter("Upscale_OOC_Gas_Level_Valid").ParameterValue) || OffUpsRefVal <= 0 || OffUpsMeasVal == decimal.MinValue)
				{
					Category.SetCheckParameter("OOC_Test_Calc_Result", "INVALID", eParameterDataType.String);
					Category.SetCheckParameter("OOC_Offline_Upscale_Injection_Calc_Result", null, eParameterDataType.Decimal);
					Category.SetCheckParameter("OOC_Offline_Upscale_Injection_Calc_APS_Indicator", null, eParameterDataType.Integer);
					Category.CheckCatalogResult = "A";
				}
				else
				{
					decimal diff = Math.Abs(OffUpsMeasVal - OffUpsRefVal);
					Category.SetCheckParameter("OOC_Offline_Upscale_Injection_Calc_APS_Indicator", 0, eParameterDataType.Integer);
					string TestCalcResult = Convert.ToString(Category.GetCheckParameter("OOC_Test_Calc_Result").ParameterValue);
					decimal TestSpanVal = Convert.ToDecimal(Category.GetCheckParameter("Test_Span_Value").ParameterValue);
					decimal OffUpscaleInjCalcResult;
					string CompTypeCd = cDBConvert.ToString(CurrentOOCTest["COMPONENT_TYPE_CD"]);
					if (CompTypeCd.InList("CO2,O2"))
					{
						diff = Math.Round(diff, 1, MidpointRounding.AwayFromZero);
						Category.SetCheckParameter("OOC_Offline_Upscale_Injection_Calc_Result", diff, eParameterDataType.Decimal);
						OffUpscaleInjCalcResult = diff;
						if (!TestCalcResult.InList("INVALID,FAILED"))
						{
							if (OffUpscaleInjCalcResult > (decimal)0.5)
							{
								Category.SetCheckParameter("OOC_Test_Calc_Result", "FAILED", eParameterDataType.String);
								if (0 <= OffUpsCalError && OffUpsCalError <= (decimal)0.5)
								{
									decimal Tolerance = GetTolerance("7DAY", "DifferencePCT", Category);
									if (Math.Abs(diff - OffUpsCalError) <= Tolerance)
										Category.SetCheckParameter("OOC_Test_Calc_Result", "PASSED", eParameterDataType.String);
								}
							}
							else
								Category.SetCheckParameter("OOC_Test_Calc_Result", "PASSED", eParameterDataType.String);
						}
					}
					else
						if (CompTypeCd.InList("SO2,NOX"))
						{
							OffUpscaleInjCalcResult = Math.Min(Math.Round(diff / TestSpanVal * 100, 1, MidpointRounding.AwayFromZero), (decimal)9999.9);
							Category.SetCheckParameter("OOC_Offline_Upscale_Injection_Calc_Result", OffUpscaleInjCalcResult, eParameterDataType.Decimal);
							diff = Math.Round(diff, MidpointRounding.AwayFromZero);
							if (OffUpscaleInjCalcResult > (decimal)2.5 && TestSpanVal < 200 && diff <= (decimal)5)
							{
								Category.SetCheckParameter("OOC_Offline_Upscale_Injection_Calc_Result", diff, eParameterDataType.Decimal);
								Category.SetCheckParameter("OOC_Offline_Upscale_Injection_Calc_APS_Indicator", 1, eParameterDataType.Integer);
								if (!TestCalcResult.InList("INVALID,FAILED"))
									Category.SetCheckParameter("OOC_Test_Calc_Result", "PASSAPS", eParameterDataType.String);
							}
							else
								if (OffUpscaleInjCalcResult > (decimal)2.5)
								{
									if (!TestCalcResult.InList("INVALID,FAILED"))
										if (Category.GetCheckParameter("OOC_Offline_Upscale_Injection_Calc_APS_Indicator").ValueAsInt() != 1 &&
											0 <= OffUpsCalError && OffUpsCalError <= (decimal)2.5)
										{
											decimal Tolerance = GetTolerance("7DAY", "CalibrationError", Category);
											if (Math.Abs(OffUpscaleInjCalcResult - OffUpsCalError) <= Tolerance)
											{
												if (TestCalcResult != "PASSAPS")
													Category.SetCheckParameter("OOC_Test_Calc_Result", "PASSED", eParameterDataType.String);
											}
											else
												Category.SetCheckParameter("OOC_Test_Calc_Result", "FAILED", eParameterDataType.String);
										}
										else
										{
											Category.SetCheckParameter("OOC_Test_Calc_Result", "FAILED", eParameterDataType.String);
											if (Category.GetCheckParameter("OOC_Offline_Upscale_Injection_Calc_APS_Indicator").ValueAsInt() == 1 &&
												0 <= OffUpsCalError && OffUpsCalError <= 5 && TestSpanVal < 200)
											{
												decimal Tolerance = GetTolerance("7DAY", "DifferencePPM", Category);
												if (Math.Abs(diff - OffUpsCalError) <= Tolerance)
													Category.SetCheckParameter("OOC_Test_Calc_Result", "PASSAPS", eParameterDataType.String);
											}
										}
								}
								else
									if (!TestCalcResult.InList("INVALID,FAILED,PASSAPS"))
										Category.SetCheckParameter("OOC_Test_Calc_Result", "PASSED", eParameterDataType.String);
						}
						else
							if (CompTypeCd == "FLOW")
							{
								OffUpscaleInjCalcResult = Math.Min(Math.Round(diff / TestSpanVal * 100, 1, MidpointRounding.AwayFromZero), (decimal)9999.9);
								Category.SetCheckParameter("OOC_Offline_Upscale_Injection_Calc_Result", OffUpscaleInjCalcResult, eParameterDataType.Decimal);
								diff = Math.Round(diff, 2, MidpointRounding.AwayFromZero);
								string AcqCd = cDBConvert.ToString(CurrentOOCTest["ACQ_CD"]);
								if ((OffUpscaleInjCalcResult > (decimal)3.0) && AcqCd == "DP" && diff <= (decimal)0.01)
								{
									Category.SetCheckParameter("OOC_Offline_Upscale_Injection_Calc_Result", 0m, eParameterDataType.Decimal);
									Category.SetCheckParameter("OOC_Offline_Upscale_Injection_Calc_APS_Indicator", 1, eParameterDataType.Integer);
									if ((TestCalcResult != "INVALID") && (TestCalcResult != "FAILED"))
										Category.SetCheckParameter("OOC_Test_Calc_Result", "PASSAPS", eParameterDataType.String);
								}
								else
									if (OffUpscaleInjCalcResult > (decimal)3.0)
									{
										if (!TestCalcResult.InList("INVALID,FAILED"))
											if (Category.GetCheckParameter("OOC_Offline_Upscale_Injection_Calc_APS_Indicator").ValueAsInt() != 1 &&
												0 <= OffUpsCalError && OffUpsCalError <= 3)
											{
												decimal Tolerance = GetTolerance("7DAY", "CalibrationError", Category);
												if (Math.Abs(OffUpscaleInjCalcResult - OffUpsCalError) <= Tolerance)
												{
													if (TestCalcResult != "PASSAPS")
														Category.SetCheckParameter("OOC_Test_Calc_Result", "PASSED", eParameterDataType.String);
												}
												else
													Category.SetCheckParameter("OOC_Test_Calc_Result", "FAILED", eParameterDataType.String);
											}
											else
											{
												Category.SetCheckParameter("OOC_Test_Calc_Result", "FAILED", eParameterDataType.String);
												if (Category.GetCheckParameter("OOC_Offline_Upscale_Injection_Calc_APS_Indicator").ValueAsInt() == 1 &&
													AcqCd == "DP" && 0 <= OffUpsCalError && OffUpsCalError <= (decimal)0.01)
												{
													decimal Tolerance = GetTolerance("7DAY", "DifferenceINH2O", Category);
													if (Math.Abs(diff - OffUpsCalError) <= Tolerance)
														Category.SetCheckParameter("OOC_Test_Calc_Result", "PASSAPS", eParameterDataType.String);
												}
											}
									}
									else
										if (!TestCalcResult.InList("INVALID,FAILED,PASSAPS"))
											Category.SetCheckParameter("OOC_Test_Calc_Result", "PASSED", eParameterDataType.String);
							}
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "ONOFF28");
			}

			return ReturnVal;
		}

		#endregion

		#region ONOFF29
		public static string ONOFF29(cCategory Category, ref bool Log)
		//Calculate Offline Zero Gas Injection or Reference Signal Results
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentOOCTest = (DataRowView)Category.GetCheckParameter("Current_OOC_Test").ParameterValue;
				decimal OffZeroRefVal = cDBConvert.ToDecimal(CurrentOOCTest["OFFLINE_ZERO_REF_VALUE"]);
				decimal OffZeroMeasVal = cDBConvert.ToDecimal(CurrentOOCTest["OFFLINE_ZERO_MEASURED_VALUE"]);
				decimal OffZeroCalError = cDBConvert.ToDecimal(CurrentOOCTest["OFFLINE_ZERO_CAL_ERROR"]);
				if (Category.GetCheckParameter("Test_Span_Value").ParameterValue == null || OffZeroRefVal < 0 || OffZeroMeasVal == decimal.MinValue)
				{
					Category.SetCheckParameter("OOC_Test_Calc_Result", "INVALID", eParameterDataType.String);
					Category.SetCheckParameter("OOC_Offline_Zero_Injection_Calc_Result", null, eParameterDataType.Decimal);
					Category.SetCheckParameter("OOC_Offline_Zero_Injection_Calc_APS_Indicator", null, eParameterDataType.Integer);
					Category.CheckCatalogResult = "A";
				}
				else
				{
					decimal diff = Math.Abs(OffZeroMeasVal - OffZeroRefVal);
					Category.SetCheckParameter("OOC_Offline_Zero_Injection_Calc_APS_Indicator", 0, eParameterDataType.Integer);
					string TestCalcResult = Convert.ToString(Category.GetCheckParameter("OOC_Test_Calc_Result").ParameterValue);
					decimal TestSpanVal = Convert.ToDecimal(Category.GetCheckParameter("Test_Span_Value").ParameterValue);
					decimal OffZeroInjCalcResult;
					string CompTypeCd = cDBConvert.ToString(CurrentOOCTest["COMPONENT_TYPE_CD"]);
					if (CompTypeCd.InList("CO2,O2"))
					{
						diff = Math.Round(diff, 1, MidpointRounding.AwayFromZero);
						Category.SetCheckParameter("OOC_Offline_Zero_Injection_Calc_Result", diff, eParameterDataType.Decimal);
						OffZeroInjCalcResult = diff;
						if (!TestCalcResult.InList("INVALID,FAILED"))
						{
							if (OffZeroInjCalcResult > (decimal)0.5)
							{
								Category.SetCheckParameter("OOC_Test_Calc_Result", "FAILED", eParameterDataType.String);
								if (0 <= OffZeroCalError && OffZeroCalError <= (decimal)0.5)
								{
									decimal Tolerance = GetTolerance("7DAY", "DifferencePCT", Category);
									if (Math.Abs(diff - OffZeroCalError) <= Tolerance)
										Category.SetCheckParameter("OOC_Test_Calc_Result", "PASSED", eParameterDataType.String);
								}
							}
							else
								Category.SetCheckParameter("OOC_Test_Calc_Result", "PASSED", eParameterDataType.String);
						}
					}
					else
						if (CompTypeCd.InList("SO2,NOX"))
						{
							OffZeroInjCalcResult = Math.Min(Math.Round(diff / TestSpanVal * 100, 1, MidpointRounding.AwayFromZero), (decimal)9999.9);
							Category.SetCheckParameter("OOC_Offline_Zero_Injection_Calc_Result", OffZeroInjCalcResult, eParameterDataType.Decimal);
							diff = Math.Round(diff, MidpointRounding.AwayFromZero);
							if ((OffZeroInjCalcResult > (decimal)2.5) && (TestSpanVal < 200) && (diff <= (decimal)5))
							{
								Category.SetCheckParameter("OOC_Offline_Zero_Injection_Calc_Result", diff, eParameterDataType.Decimal);
								Category.SetCheckParameter("OOC_Offline_Zero_Injection_Calc_APS_Indicator", 1, eParameterDataType.Integer);
								if ((TestCalcResult != "INVALID") && (TestCalcResult != "FAILED"))
									Category.SetCheckParameter("OOC_Test_Calc_Result", "PASSAPS", eParameterDataType.String);
							}
							else
								if (OffZeroInjCalcResult > (decimal)2.5)
								{
									if (!TestCalcResult.InList("INVALID,FAILED"))
										if (Category.GetCheckParameter("OOC_Offline_Zero_Injection_Calc_APS_Indicator").ValueAsInt() != 1 &&
											0 <= OffZeroCalError && OffZeroCalError <= (decimal)2.5)
										{
											decimal Tolerance = GetTolerance("7DAY", "CalibrationError", Category);
											if (Math.Abs(OffZeroInjCalcResult - OffZeroCalError) <= Tolerance)
											{
												if (TestCalcResult != "PASSAPS")
													Category.SetCheckParameter("OOC_Test_Calc_Result", "PASSED", eParameterDataType.String);
											}
											else
												Category.SetCheckParameter("OOC_Test_Calc_Result", "FAILED", eParameterDataType.String);
										}
										else
										{
											Category.SetCheckParameter("OOC_Test_Calc_Result", "FAILED", eParameterDataType.String);
											if (Category.GetCheckParameter("OOC_Offline_Zero_Injection_Calc_APS_Indicator").ValueAsInt() == 1 &&
												0 <= OffZeroCalError && OffZeroCalError <= 5 && TestSpanVal < 200)
											{
												decimal Tolerance = GetTolerance("7DAY", "DifferencePPM", Category);
												if (Math.Abs(diff - OffZeroCalError) <= Tolerance)
													Category.SetCheckParameter("OOC_Test_Calc_Result", "PASSAPS", eParameterDataType.String);
											}
										}
								}
								else
									if (!TestCalcResult.InList("INVALID,FAILED,PASSAPS"))
										Category.SetCheckParameter("OOC_Test_Calc_Result", "PASSED", eParameterDataType.String);
						}
						else
							if (CompTypeCd == "FLOW")
							{
								OffZeroInjCalcResult = Math.Min(Math.Round(diff / TestSpanVal * 100, 1, MidpointRounding.AwayFromZero), (decimal)9999.9);
								Category.SetCheckParameter("OOC_Offline_Zero_Injection_Calc_Result", OffZeroInjCalcResult, eParameterDataType.Decimal);
								diff = Math.Round(diff, 2, MidpointRounding.AwayFromZero);
								string AcqCd = cDBConvert.ToString(CurrentOOCTest["ACQ_CD"]);
								if ((OffZeroInjCalcResult > (decimal)3.0) && AcqCd == "DP" && (diff <= (decimal)0.01))
								{
									Category.SetCheckParameter("OOC_Offline_Zero_Injection_Calc_Result", 0m, eParameterDataType.Decimal);
									Category.SetCheckParameter("OOC_Offline_Zero_Injection_Calc_APS_Indicator", 1, eParameterDataType.Integer);
									if ((TestCalcResult != "INVALID") && (TestCalcResult != "FAILED"))
										Category.SetCheckParameter("OOC_Test_Calc_Result", "PASSAPS", eParameterDataType.String);
								}
								else
									if (OffZeroInjCalcResult > (decimal)3.0)
									{
										if (!TestCalcResult.InList("INVALID,FAILED"))
											if (Category.GetCheckParameter("OOC_Offline_Zero_Injection_Calc_APS_Indicator").ValueAsInt() != 1 &&
												0 <= OffZeroCalError && OffZeroCalError <= 3)
											{
												decimal Tolerance = GetTolerance("7DAY", "CalibrationError", Category);
												if (Math.Abs(OffZeroInjCalcResult - OffZeroCalError) <= Tolerance)
												{
													if (TestCalcResult != "PASSAPS")
														Category.SetCheckParameter("OOC_Test_Calc_Result", "PASSED", eParameterDataType.String);
												}
												else
													Category.SetCheckParameter("OOC_Test_Calc_Result", "FAILED", eParameterDataType.String);
											}
											else
											{
												Category.SetCheckParameter("OOC_Test_Calc_Result", "FAILED", eParameterDataType.String);
												if (Category.GetCheckParameter("OOC_Offline_Zero_Injection_Calc_APS_Indicator").ValueAsInt() == 1 &&
													AcqCd == "DP" && 0 <= OffZeroCalError && OffZeroCalError <= (decimal)0.01)
												{
													decimal Tolerance = GetTolerance("7DAY", "DifferenceINH2O", Category);
													if (Math.Abs(diff - OffZeroCalError) <= Tolerance)
														Category.SetCheckParameter("OOC_Test_Calc_Result", "PASSAPS", eParameterDataType.String);
												}
											}
									}
									else
										if (!TestCalcResult.InList("INVALID,FAILED,PASSAPS"))
											Category.SetCheckParameter("OOC_Test_Calc_Result", "PASSED", eParameterDataType.String);
							}
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "ONOFF29");
			}

			return ReturnVal;
		}

		#endregion

		#region ONOFF30
		public static string ONOFF30(cCategory Category, ref bool Log)
		//Calculate Online Upscale Gas Injection or Reference Signal Results
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentOOCTest = (DataRowView)Category.GetCheckParameter("Current_OOC_Test").ParameterValue;
				decimal OnUpsRefVal = cDBConvert.ToDecimal(CurrentOOCTest["ONLINE_UPSCALE_REF_VALUE"]);
				decimal OnUpsMeasVal = cDBConvert.ToDecimal(CurrentOOCTest["ONLINE_UPSCALE_MEASURED_VALUE"]);
				decimal OnUpsCalError = cDBConvert.ToDecimal(CurrentOOCTest["ONLINE_UPSCALE_CAL_ERROR"]);
				if (Category.GetCheckParameter("Test_Span_Value").ParameterValue == null ||
					!Category.GetCheckParameter("Upscale_OOC_Gas_Level_Valid").ValueAsBool() ||
					OnUpsRefVal <= 0 || OnUpsMeasVal == decimal.MinValue)
				{
					Category.SetCheckParameter("OOC_Test_Calc_Result", "INVALID", eParameterDataType.String);
					Category.SetCheckParameter("OOC_Online_Upscale_Injection_Calc_Result", null, eParameterDataType.Decimal);
					Category.SetCheckParameter("OOC_Online_Upscale_Injection_Calc_APS_Indicator", null, eParameterDataType.Integer);
					Category.CheckCatalogResult = "A";
				}
				else
				{
					decimal diff = Math.Abs(OnUpsMeasVal - OnUpsRefVal);
					Category.SetCheckParameter("OOC_Online_Upscale_Injection_Calc_APS_Indicator", 0, eParameterDataType.Integer);
					string TestCalcResult = Convert.ToString(Category.GetCheckParameter("OOC_Test_Calc_Result").ParameterValue);
					decimal TestSpanVal = Convert.ToDecimal(Category.GetCheckParameter("Test_Span_Value").ParameterValue);
					decimal OnUpscaleInjCalcResult;
					string CompTypeCd = cDBConvert.ToString(CurrentOOCTest["COMPONENT_TYPE_CD"]);

					if (CompTypeCd.InList("CO2,O2"))
					{
						diff = Math.Round(diff, 1, MidpointRounding.AwayFromZero);
						Category.SetCheckParameter("OOC_Online_Upscale_Injection_Calc_Result", diff, eParameterDataType.Decimal);
						OnUpscaleInjCalcResult = diff;
						if (!TestCalcResult.InList("INVALID,FAILED"))
						{
							if (OnUpscaleInjCalcResult > (decimal)0.5)
							{
								Category.SetCheckParameter("OOC_Test_Calc_Result", "FAILED", eParameterDataType.String);
								if (0 <= OnUpsCalError && OnUpsCalError <= (decimal)0.5)
								{
									decimal Tolerance = GetTolerance("7DAY", "DifferencePCT", Category);
									if (Math.Abs(diff - OnUpsCalError) <= Tolerance)
										Category.SetCheckParameter("OOC_Test_Calc_Result", "PASSED", eParameterDataType.String);
								}
							}
							else
								Category.SetCheckParameter("OOC_Test_Calc_Result", "PASSED", eParameterDataType.String);
						}
					}
					else
						if (CompTypeCd.InList("SO2,NOX"))
						{
							OnUpscaleInjCalcResult = Math.Min(Math.Round(diff / TestSpanVal * 100, 1, MidpointRounding.AwayFromZero), (decimal)9999.9);
							Category.SetCheckParameter("OOC_Online_Upscale_Injection_Calc_Result", OnUpscaleInjCalcResult, eParameterDataType.Decimal);
							diff = Math.Round(diff, MidpointRounding.AwayFromZero);
							if (OnUpscaleInjCalcResult > (decimal)2.5 && TestSpanVal < 200 && diff <= (decimal)5)
							{
								Category.SetCheckParameter("OOC_Online_Upscale_Injection_Calc_Result", diff, eParameterDataType.Decimal);
								Category.SetCheckParameter("OOC_Online_Upscale_Injection_Calc_APS_Indicator", 1, eParameterDataType.Integer);
								if (TestCalcResult != "INVALID" && TestCalcResult != "FAILED")
									Category.SetCheckParameter("OOC_Test_Calc_Result", "PASSAPS", eParameterDataType.String);
							}
							else
								if (OnUpscaleInjCalcResult > (decimal)2.5)
								{
									if (!TestCalcResult.InList("INVALID,FAILED"))
										if (Category.GetCheckParameter("OOC_Online_Upscale_Injection_Calc_APS_Indicator").ValueAsInt() != 1 &&
											0 <= OnUpsCalError && OnUpsCalError <= (decimal)2.5)
										{
											decimal Tolerance = GetTolerance("7DAY", "CalibrationError", Category);
											if (Math.Abs(OnUpscaleInjCalcResult - OnUpsCalError) <= Tolerance)
											{
												if (TestCalcResult != "PASSAPS")
													Category.SetCheckParameter("OOC_Test_Calc_Result", "PASSED", eParameterDataType.String);
											}
											else
												Category.SetCheckParameter("OOC_Test_Calc_Result", "FAILED", eParameterDataType.String);
										}
										else
										{
											Category.SetCheckParameter("OOC_Test_Calc_Result", "FAILED", eParameterDataType.String);
											if (Category.GetCheckParameter("OOC_Online_Upscale_Injection_Calc_APS_Indicator").ValueAsInt() == 1 &&
												0 <= OnUpsCalError && OnUpsCalError <= 5 && TestSpanVal < 200)
											{
												decimal Tolerance = GetTolerance("7DAY", "DifferencePPM", Category);
												if (Math.Abs(diff - OnUpsCalError) <= Tolerance)
													Category.SetCheckParameter("OOC_Test_Calc_Result", "PASSAPS", eParameterDataType.String);
											}
										}
								}
								else
									if (!TestCalcResult.InList("INVALID,FAILED,PASSAPS"))
										Category.SetCheckParameter("OOC_Test_Calc_Result", "PASSED", eParameterDataType.String);
						}
						else
							if (CompTypeCd == "FLOW")
							{
								OnUpscaleInjCalcResult = Math.Min(Math.Round(diff / TestSpanVal * 100, 1, MidpointRounding.AwayFromZero), (decimal)9999.9);
								Category.SetCheckParameter("OOC_Online_Upscale_Injection_Calc_Result", OnUpscaleInjCalcResult, eParameterDataType.Decimal);
								diff = Math.Round(diff, 2, MidpointRounding.AwayFromZero);
								string AcqCd = cDBConvert.ToString(CurrentOOCTest["ACQ_CD"]);
								if (OnUpscaleInjCalcResult > (decimal)3.0 && AcqCd == "DP" && diff <= (decimal)0.01)
								{
									Category.SetCheckParameter("OOC_Online_Upscale_Injection_Calc_Result", 0m, eParameterDataType.Decimal);
									Category.SetCheckParameter("OOC_Online_Upscale_Injection_Calc_APS_Indicator", 1, eParameterDataType.Integer);
									if (TestCalcResult != "INVALID" && TestCalcResult != "FAILED")
										Category.SetCheckParameter("OOC_Test_Calc_Result", "PASSAPS", eParameterDataType.String);
								}
								else
									if (OnUpscaleInjCalcResult > (decimal)3.0)
									{
										if (!TestCalcResult.InList("INVALID,FAILED"))
											if (Category.GetCheckParameter("OOC_Online_Upscale_Injection_Calc_APS_Indicator").ValueAsInt() != 1 &&
												0 <= OnUpsCalError && OnUpsCalError <= 3)
											{
												decimal Tolerance = GetTolerance("7DAY", "CalibrationError", Category);
												if (Math.Abs(OnUpscaleInjCalcResult - OnUpsCalError) <= Tolerance)
												{
													if (TestCalcResult != "PASSAPS")
														Category.SetCheckParameter("OOC_Test_Calc_Result", "PASSED", eParameterDataType.String);
												}
												else
													Category.SetCheckParameter("OOC_Test_Calc_Result", "FAILED", eParameterDataType.String);
											}
											else
											{
												Category.SetCheckParameter("OOC_Test_Calc_Result", "FAILED", eParameterDataType.String);
												if (Category.GetCheckParameter("OOC_Online_Upscale_Injection_Calc_APS_Indicator").ValueAsInt() == 1 &&
													AcqCd == "DP" && 0 <= OnUpsCalError && OnUpsCalError <= (decimal)0.01)
												{
													decimal Tolerance = GetTolerance("7DAY", "DifferenceINH2O", Category);
													if (Math.Abs(diff - OnUpsCalError) <= Tolerance)
														Category.SetCheckParameter("OOC_Test_Calc_Result", "PASSAPS", eParameterDataType.String);
												}
											}
									}
									else
										if (!TestCalcResult.InList("INVALID,FAILED,PASSAPS"))
											Category.SetCheckParameter("OOC_Test_Calc_Result", "PASSED", eParameterDataType.String);
							}
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "ONOFF30");
			}

			return ReturnVal;
		}

		#endregion
		#endregion

		#region OOC 31-40

		#region ONOFF31
		public static string ONOFF31(cCategory Category, ref bool Log)
		//Calculate Online Zero Gas Injection or Reference Signal Results
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentOOCTest = (DataRowView)Category.GetCheckParameter("Current_OOC_Test").ParameterValue;
				decimal OnZeroRefVal = cDBConvert.ToDecimal(CurrentOOCTest["ONLINE_ZERO_REF_VALUE"]);
				decimal OnZeroMeasVal = cDBConvert.ToDecimal(CurrentOOCTest["ONLINE_ZERO_MEASURED_VALUE"]);
				decimal OnZeroCalError = cDBConvert.ToDecimal(CurrentOOCTest["ONLINE_ZERO_CAL_ERROR"]);
				if (Category.GetCheckParameter("Test_Span_Value").ParameterValue == null || OnZeroRefVal < 0 || OnZeroMeasVal == decimal.MinValue)
				{
					Category.SetCheckParameter("OOC_Test_Calc_Result", "INVALID", eParameterDataType.String);
					Category.SetCheckParameter("OOC_Online_Zero_Injection_Calc_Result", null, eParameterDataType.Decimal);
					Category.SetCheckParameter("OOC_Online_Zero_Injection_Calc_APS_Indicator", null, eParameterDataType.Integer);
					Category.CheckCatalogResult = "A";
				}
				else
				{
					decimal diff = Math.Abs(OnZeroMeasVal - OnZeroRefVal);
					Category.SetCheckParameter("OOC_Online_Zero_Injection_Calc_APS_Indicator", 0, eParameterDataType.Integer);
					string TestCalcResult = Convert.ToString(Category.GetCheckParameter("OOC_Test_Calc_Result").ParameterValue);
					decimal TestSpanVal = Convert.ToDecimal(Category.GetCheckParameter("Test_Span_Value").ParameterValue);
					decimal OnZeroInjCalcResult;
					string CompTypeCd = cDBConvert.ToString(CurrentOOCTest["COMPONENT_TYPE_CD"]);
					if (CompTypeCd.InList("CO2,O2"))
					{
						diff = Math.Round(diff, 1, MidpointRounding.AwayFromZero);
						Category.SetCheckParameter("OOC_Online_Zero_Injection_Calc_Result", diff, eParameterDataType.Decimal);
						OnZeroInjCalcResult = diff;
						if (!TestCalcResult.InList("INVALID,FAILED"))
						{
							if (OnZeroInjCalcResult > (decimal)0.5)
							{
								Category.SetCheckParameter("OOC_Test_Calc_Result", "FAILED", eParameterDataType.String);
								if (0 <= OnZeroCalError && OnZeroCalError <= (decimal)0.5)
								{
									decimal Tolerance = GetTolerance("7DAY", "DifferencePCT", Category);
									if (Math.Abs(diff - OnZeroCalError) <= Tolerance)
										Category.SetCheckParameter("OOC_Test_Calc_Result", "PASSED", eParameterDataType.String);
								}
							}
							else
								Category.SetCheckParameter("OOC_Test_Calc_Result", "PASSED", eParameterDataType.String);
						}
					}
					else
						if (CompTypeCd.InList("SO2,NOX"))
						{
							OnZeroInjCalcResult = Math.Min(Math.Round(diff / TestSpanVal * 100, 1, MidpointRounding.AwayFromZero), (decimal)9999.9);
							Category.SetCheckParameter("OOC_Online_Zero_Injection_Calc_Result", OnZeroInjCalcResult, eParameterDataType.Decimal);
							diff = Math.Round(diff, MidpointRounding.AwayFromZero);
							if ((OnZeroInjCalcResult > (decimal)2.5) && (TestSpanVal < 200) && (diff <= (decimal)5))
							{
								Category.SetCheckParameter("OOC_Online_Zero_Injection_Calc_Result", diff, eParameterDataType.Decimal);
								Category.SetCheckParameter("OOC_Online_Zero_Injection_Calc_APS_Indicator", 1, eParameterDataType.Integer);
								if ((TestCalcResult != "INVALID") && (TestCalcResult != "FAILED"))
									Category.SetCheckParameter("OOC_Test_Calc_Result", "PASSAPS", eParameterDataType.String);
							}
							else
								if (OnZeroInjCalcResult > (decimal)2.5)
								{
									if (!TestCalcResult.InList("INVALID,FAILED"))
										if (Category.GetCheckParameter("OOC_Online_Zero_Injection_Calc_APS_Indicator").ValueAsInt() != 1 &&
											0 <= OnZeroCalError && OnZeroCalError <= (decimal)2.5)
										{
											decimal Tolerance = GetTolerance("7DAY", "CalibrationError", Category);
											if (Math.Abs(OnZeroInjCalcResult - OnZeroCalError) <= Tolerance)
											{
												if (TestCalcResult != "PASSAPS")
													Category.SetCheckParameter("OOC_Test_Calc_Result", "PASSED", eParameterDataType.String);
											}
											else
												Category.SetCheckParameter("OOC_Test_Calc_Result", "FAILED", eParameterDataType.String);
										}
										else
										{
											Category.SetCheckParameter("OOC_Test_Calc_Result", "FAILED", eParameterDataType.String);
											if (Category.GetCheckParameter("OOC_Online_Zero_Injection_Calc_APS_Indicator").ValueAsInt() == 1 &&
												0 <= OnZeroCalError && OnZeroCalError <= 5 && TestSpanVal < 200)
											{
												decimal Tolerance = GetTolerance("7DAY", "DifferencePPM", Category);
												if (Math.Abs(diff - OnZeroCalError) <= Tolerance)
													Category.SetCheckParameter("OOC_Test_Calc_Result", "PASSAPS", eParameterDataType.String);
											}
										}
								}
								else
									if (!TestCalcResult.InList("INVALID,FAILED,PASSAPS"))
										Category.SetCheckParameter("OOC_Test_Calc_Result", "PASSED", eParameterDataType.String);
						}
						else
							if (CompTypeCd == "FLOW")
							{
								OnZeroInjCalcResult = Math.Min(Math.Round(diff / TestSpanVal * 100, 1, MidpointRounding.AwayFromZero), (decimal)9999.9);
								Category.SetCheckParameter("OOC_Online_Zero_Injection_Calc_Result", OnZeroInjCalcResult, eParameterDataType.Decimal);
								diff = Math.Round(diff, 2, MidpointRounding.AwayFromZero);
								string AcqCd = cDBConvert.ToString(CurrentOOCTest["ACQ_CD"]);
								if ((OnZeroInjCalcResult > (decimal)3.0) && AcqCd == "DP" && (diff <= (decimal)0.01))
								{
									Category.SetCheckParameter("OOC_Online_Zero_Injection_Calc_Result", 0m, eParameterDataType.Decimal);
									Category.SetCheckParameter("OOC_Online_Zero_Injection_Calc_APS_Indicator", 1, eParameterDataType.Integer);
									if ((TestCalcResult != "INVALID") && (TestCalcResult != "FAILED"))
										Category.SetCheckParameter("OOC_Test_Calc_Result", "PASSAPS", eParameterDataType.String);
								}
								else
									if (OnZeroInjCalcResult > (decimal)3.0)
									{
										if (!TestCalcResult.InList("INVALID,FAILED"))
											if (Category.GetCheckParameter("OOC_Online_Zero_Injection_Calc_APS_Indicator").ValueAsInt() != 1 &&
												0 <= OnZeroCalError && OnZeroCalError <= 3)
											{
												decimal Tolerance = GetTolerance("7DAY", "CalibrationError", Category);
												if (Math.Abs(OnZeroInjCalcResult - OnZeroCalError) <= Tolerance)
												{
													if (TestCalcResult != "PASSAPS")
														Category.SetCheckParameter("OOC_Test_Calc_Result", "PASSED", eParameterDataType.String);
												}
												else
													Category.SetCheckParameter("OOC_Test_Calc_Result", "FAILED", eParameterDataType.String);
											}
											else
											{
												Category.SetCheckParameter("OOC_Test_Calc_Result", "FAILED", eParameterDataType.String);
												if (Category.GetCheckParameter("OOC_Online_Zero_Injection_Calc_APS_Indicator").ValueAsInt() == 1 &&
													AcqCd == "DP" && 0 <= OnZeroCalError && OnZeroCalError <= (decimal)0.01)
												{
													decimal Tolerance = GetTolerance("7DAY", "DifferenceINH2O", Category);
													if (Math.Abs(diff - OnZeroCalError) <= Tolerance)
														Category.SetCheckParameter("OOC_Test_Calc_Result", "PASSAPS", eParameterDataType.String);
												}
											}
									}
									else
										if (!TestCalcResult.InList("INVALID,FAILED,PASSAPS"))
											Category.SetCheckParameter("OOC_Test_Calc_Result", "PASSED", eParameterDataType.String);
							}
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "ONOFF31");
			}

			return ReturnVal;
		}

		#endregion

		#region ONOFF32
		public static string ONOFF32(cCategory Category, ref bool Log)
		//Reported Offline Upscale Injection Results Consistent with Recalculated Values
		//Note: Test Tolerance Cross Check records are pre-filtered for "7DAY" TestTypeCode by the Category
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentOOCTest = (DataRowView)Category.GetCheckParameter("Current_OOC_Test").ParameterValue;
				int UpscaleAPSInd = cDBConvert.ToInteger(CurrentOOCTest["OFFLINE_UPSCALE_APS_IND"]);
				string CompTypeCd = cDBConvert.ToString(CurrentOOCTest["COMPONENT_TYPE_CD"]);
				if (UpscaleAPSInd == 1 && CompTypeCd == "FLOW" && cDBConvert.ToString(CurrentOOCTest["ACQ_CD"]) != "DP")
					Category.CheckCatalogResult = "A";
				else
					if (UpscaleAPSInd == 1 && CompTypeCd.InList("SO2,NOX") && Convert.ToDecimal(Category.GetCheckParameter("Test_Span_Value").ParameterValue) >= 200)
						Category.CheckCatalogResult = "B";
					else
						if (UpscaleAPSInd == 1 && CompTypeCd.InList("CO2,O2"))
							Category.CheckCatalogResult = "C";
						else
						{
							if (Category.GetCheckParameter("OOC_Offline_Upscale_Injection_Calc_Result").ParameterValue != null)
							{
								int ParameterAPSInd = Category.GetCheckParameter("OOC_Offline_Upscale_Injection_Calc_APS_Indicator").ValueAsInt();
								if ((UpscaleAPSInd != 1) && ParameterAPSInd == 1)
									Category.CheckCatalogResult = "D";
								else
								{
									decimal OffUpsCalErr = cDBConvert.ToDecimal(CurrentOOCTest["OFFLINE_UPSCALE_CAL_ERROR"]);
									if (OffUpsCalErr >= 0)
									{
										DataView TestToleranceRecords = (DataView)Category.GetCheckParameter("Test_Tolerances_Cross_Check_Table").ParameterValue;
										string OldFilter = TestToleranceRecords.RowFilter;
										decimal ToleranceValue = 0;

										if (CompTypeCd.InList("CO2,O2"))
										{
											TestToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, "FieldDescription = '" + "DifferencePCT'"); //pre-filtered for 7DAY by category
											ToleranceValue = cDBConvert.ToDecimal(((DataRowView)TestToleranceRecords[0])["Tolerance"]);
											if (Math.Abs(Convert.ToDecimal(Category.GetCheckParameter("OOC_Offline_Upscale_Injection_Calc_Result").ParameterValue) - OffUpsCalErr) > ToleranceValue)
												Category.CheckCatalogResult = "E";
										}
										else
										{
											if (ParameterAPSInd == 1)
											{
												if (CompTypeCd == "FLOW")
												{
													TestToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, "FieldDescription = '" + "DifferenceINH2O'");  //pre-filtered for 7DAY by category
													ToleranceValue = cDBConvert.ToDecimal(((DataRowView)TestToleranceRecords[0])["Tolerance"]);
													if (Math.Abs(Convert.ToDecimal(Category.GetCheckParameter("OOC_Offline_Upscale_Injection_Calc_Result").ParameterValue) - OffUpsCalErr) > ToleranceValue)
														Category.CheckCatalogResult = "E";
												}
												else
												{
													TestToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, "FieldDescription = '" + "DifferencePPM'");  //pre-filtered for 7DAY by category
													ToleranceValue = cDBConvert.ToDecimal(((DataRowView)TestToleranceRecords[0])["Tolerance"]);
													if (Math.Abs(Convert.ToDecimal(Category.GetCheckParameter("OOC_Offline_Upscale_Injection_Calc_Result").ParameterValue) - OffUpsCalErr) > ToleranceValue)
														Category.CheckCatalogResult = "E";
												}
											}
											else
												if (UpscaleAPSInd == 0)
												{
													TestToleranceRecords.RowFilter = AddToDataViewFilter(OldFilter, "FieldDescription = '" + "CalibrationError'");  //pre-filtered for 7DAY by category
													ToleranceValue = cDBConvert.ToDecimal(((DataRowView)TestToleranceRecords[0])["Tolerance"]);
													if (Math.Abs(Convert.ToDecimal(Category.GetCheckParameter("OOC_Offline_Upscale_Injection_Calc_Result").ParameterValue) - OffUpsCalErr) > ToleranceValue)
														Category.CheckCatalogResult = "F";
												}
										}
										TestToleranceRecords.RowFilter = OldFilter;
									}
								}
							}
						}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "ONOFF32");
			}

			return ReturnVal;
		}

		#endregion

		#region ONOFF33
		public static string ONOFF33(cCategory Category, ref bool Log)
		//Reported Offline Zero Injection Results Consistent with Recalculated Values
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentOOCTest = (DataRowView)Category.GetCheckParameter("Current_OOC_Test").ParameterValue;
				int OffZeroAPSInd = cDBConvert.ToInteger(CurrentOOCTest["OFFLINE_ZERO_APS_IND"]);
				string CompTypeCd = cDBConvert.ToString(CurrentOOCTest["COMPONENT_TYPE_CD"]);
				if (OffZeroAPSInd == 1 && CompTypeCd == "FLOW" && cDBConvert.ToString(CurrentOOCTest["ACQ_CD"]) != "DP")
					Category.CheckCatalogResult = "A";
				else
					if (OffZeroAPSInd == 1 && CompTypeCd.InList("SO2,NOX") && Convert.ToDecimal(Category.GetCheckParameter("Test_Span_Value").ParameterValue) >= 200)
						Category.CheckCatalogResult = "B";
					else
						if (OffZeroAPSInd == 1 && CompTypeCd.InList("CO2,O2"))
							Category.CheckCatalogResult = "C";
						else
						{
							if (Category.GetCheckParameter("OOC_Offline_Zero_Injection_Calc_Result").ParameterValue != null)
							{
								int ParameterAPSInd = Category.GetCheckParameter("OOC_Offline_Zero_Injection_Calc_APS_Indicator").ValueAsInt();
								if ((OffZeroAPSInd != 1) && ParameterAPSInd == 1)
									Category.CheckCatalogResult = "D";
								else
								{
									decimal OffZeroCalErr = cDBConvert.ToDecimal(CurrentOOCTest["OFFLINE_ZERO_CAL_ERROR"]);
									if (OffZeroCalErr >= 0)
									{
										decimal Tolerance = 0;

										if (CompTypeCd.InList("CO2,O2"))
										{
											Tolerance = GetTolerance("7DAY", "DifferencePCT", Category);
											if (Math.Abs(Convert.ToDecimal(Category.GetCheckParameter("OOC_Offline_Zero_Injection_Calc_Result").ParameterValue) - OffZeroCalErr) > Tolerance)
												Category.CheckCatalogResult = "E";
										}
										else
										{
											if (ParameterAPSInd == 1)
											{
												if (CompTypeCd == "FLOW")
												{
													Tolerance = GetTolerance("7DAY", "DifferenceINH2O", Category);
													if (Math.Abs(Convert.ToDecimal(Category.GetCheckParameter("OOC_Offline_Zero_Injection_Calc_Result").ParameterValue) - OffZeroCalErr) > Tolerance)
														Category.CheckCatalogResult = "E";
												}
												else
												{
													Tolerance = GetTolerance("7DAY", "DifferencePPM", Category);
													if (Math.Abs(Convert.ToDecimal(Category.GetCheckParameter("OOC_Offline_Zero_Injection_Calc_Result").ParameterValue) - OffZeroCalErr) > Tolerance)
														Category.CheckCatalogResult = "E";
												}
											}
											else
												if (OffZeroAPSInd == 0)
												{
													Tolerance = GetTolerance("7DAY", "CalibrationError", Category);
													if (Math.Abs(Convert.ToDecimal(Category.GetCheckParameter("OOC_Offline_Zero_Injection_Calc_Result").ParameterValue) - OffZeroCalErr) > Tolerance)
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
				ReturnVal = Category.CheckEngine.FormatError(ex, "ONOFF33");
			}

			return ReturnVal;
		}

		#endregion

		#region ONOFF34
		public static string ONOFF34(cCategory Category, ref bool Log)
		//Calculate Online Upscale Gas Injection or Reference Signal Results
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentOOCTest = (DataRowView)Category.GetCheckParameter("Current_OOC_Test").ParameterValue;
				int UpscaleAPSInd = cDBConvert.ToInteger(CurrentOOCTest["ONLINE_UPSCALE_APS_IND"]);
				string CompTypeCd = cDBConvert.ToString(CurrentOOCTest["COMPONENT_TYPE_CD"]);
				if (UpscaleAPSInd == 1 && CompTypeCd == "FLOW" && cDBConvert.ToString(CurrentOOCTest["ACQ_CD"]) != "DP")
					Category.CheckCatalogResult = "A";
				else
					if (UpscaleAPSInd == 1 && CompTypeCd.InList("SO2,NOX") && Convert.ToDecimal(Category.GetCheckParameter("Test_Span_Value").ParameterValue) >= 200)
						Category.CheckCatalogResult = "B";
					else
						if (UpscaleAPSInd == 1 && CompTypeCd.InList("CO2,O2"))
							Category.CheckCatalogResult = "C";
						else
						{
							if (Category.GetCheckParameter("OOC_Online_Upscale_Injection_Calc_Result").ParameterValue != null)
							{
								int ParameterAPSInd = Category.GetCheckParameter("OOC_Online_Upscale_Injection_Calc_APS_Indicator").ValueAsInt();
								if ((UpscaleAPSInd != 1) && ParameterAPSInd == 1)
									Category.CheckCatalogResult = "D";
								else
								{
									decimal OnUpsCalErr = cDBConvert.ToDecimal(CurrentOOCTest["ONLINE_UPSCALE_CAL_ERROR"]);
									if (OnUpsCalErr >= 0)
									{
										decimal Tolerance = 0;

										if (CompTypeCd.InList("CO2,O2"))
										{
											Tolerance = GetTolerance("7DAY", "DifferencePCT", Category);
											if (Math.Abs(Convert.ToDecimal(Category.GetCheckParameter("OOC_Online_Upscale_Injection_Calc_Result").ParameterValue) - OnUpsCalErr) > Tolerance)
												Category.CheckCatalogResult = "E";
										}
										else
										{
											if (ParameterAPSInd == 1)
											{
												if (CompTypeCd == "FLOW")
												{
													Tolerance = GetTolerance("7DAY", "DifferenceINH2O", Category);
													if (Math.Abs(Convert.ToDecimal(Category.GetCheckParameter("OOC_Online_Upscale_Injection_Calc_Result").ParameterValue) - OnUpsCalErr) > Tolerance)
														Category.CheckCatalogResult = "E";
												}
												else
												{
													Tolerance = GetTolerance("7DAY", "DifferencePPM", Category);
													if (Math.Abs(Convert.ToDecimal(Category.GetCheckParameter("OOC_Online_Upscale_Injection_Calc_Result").ParameterValue) - OnUpsCalErr) > Tolerance)
														Category.CheckCatalogResult = "E";
												}
											}
											else
												if (UpscaleAPSInd == 0)
												{
													Tolerance = GetTolerance("7DAY", "CalibrationError", Category);
													if (Math.Abs(Convert.ToDecimal(Category.GetCheckParameter("OOC_Online_Upscale_Injection_Calc_Result").ParameterValue) - OnUpsCalErr) > Tolerance)
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
				ReturnVal = Category.CheckEngine.FormatError(ex, "ONOFF34");
			}

			return ReturnVal;
		}

		#endregion

		#region ONOFF35
		public static string ONOFF35(cCategory Category, ref bool Log)
		//Reported Online Zero Injection Results Consistent with Recalculated Values
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentOOCTest = (DataRowView)Category.GetCheckParameter("Current_OOC_Test").ParameterValue;
				int OnZeroAPSInd = cDBConvert.ToInteger(CurrentOOCTest["ONLINE_ZERO_APS_IND"]);
				string CompTypeCd = cDBConvert.ToString(CurrentOOCTest["COMPONENT_TYPE_CD"]);
				if (OnZeroAPSInd == 1 && CompTypeCd == "FLOW" && cDBConvert.ToString(CurrentOOCTest["ACQ_CD"]) != "DP")
					Category.CheckCatalogResult = "A";
				else
					if (OnZeroAPSInd == 1 && CompTypeCd.InList("SO2,NOX") && Convert.ToDecimal(Category.GetCheckParameter("Test_Span_Value").ParameterValue) >= 200)
						Category.CheckCatalogResult = "B";
					else
						if (OnZeroAPSInd == 1 && CompTypeCd.InList("CO2,O2"))
							Category.CheckCatalogResult = "C";
						else
						{
							if (Category.GetCheckParameter("OOC_Online_Zero_Injection_Calc_Result").ParameterValue != null)
							{
								int ParameterAPSInd = Category.GetCheckParameter("OOC_Online_Zero_Injection_Calc_APS_Indicator").ValueAsInt();
								if ((OnZeroAPSInd != 1) && ParameterAPSInd == 1)
									Category.CheckCatalogResult = "D";
								else
								{
									decimal OnZeroCalErr = cDBConvert.ToDecimal(CurrentOOCTest["ONLINE_ZERO_CAL_ERROR"]);
									if (OnZeroCalErr >= 0)
									{
										decimal Tolerance = 0;

										if (CompTypeCd.InList("CO2,O2"))
										{
											Tolerance = GetTolerance("7DAY", "DifferencePCT", Category);
											if (Math.Abs(Convert.ToDecimal(Category.GetCheckParameter("OOC_Online_Zero_Injection_Calc_Result").ParameterValue) - OnZeroCalErr) > Tolerance)
												Category.CheckCatalogResult = "E";
										}
										else
										{
											if (ParameterAPSInd == 1)
											{
												if (CompTypeCd == "FLOW")
												{
													Tolerance = GetTolerance("7DAY", "DifferenceINH2O", Category);
													if (Math.Abs(Convert.ToDecimal(Category.GetCheckParameter("OOC_Online_Zero_Injection_Calc_Result").ParameterValue) - OnZeroCalErr) > Tolerance)
														Category.CheckCatalogResult = "E";
												}
												else
												{
													Tolerance = GetTolerance("7DAY", "DifferencePPM", Category);
													if (Math.Abs(Convert.ToDecimal(Category.GetCheckParameter("OOC_Online_Zero_Injection_Calc_Result").ParameterValue) - OnZeroCalErr) > Tolerance)
														Category.CheckCatalogResult = "E";
												}
											}
											else
												if (OnZeroAPSInd == 0)
												{
													Tolerance = GetTolerance("7DAY", "CalibrationError", Category);
													if (Math.Abs(Convert.ToDecimal(Category.GetCheckParameter("OOC_Online_Zero_Injection_Calc_Result").ParameterValue) - OnZeroCalErr) > Tolerance)
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
				ReturnVal = Category.CheckEngine.FormatError(ex, "ONOFF35");
			}

			return ReturnVal;
		}

		#endregion

		#region ONOFF36
		public static string ONOFF36(cCategory Category, ref bool Log)
		//Determination of Overall Online Offline Calibration Test Result
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentOOCTest = (DataRowView)Category.GetCheckParameter("Current_OOC_Test").ParameterValue;
				if (Convert.ToString(Category.GetCheckParameter("OOC_Test_Calc_Result").ParameterValue) == "INVALID")
					Category.SetCheckParameter("OOC_Test_Calc_Result", null, eParameterDataType.String);
				string TestResultCd = cDBConvert.ToString(CurrentOOCTest["TEST_RESULT_CD"]);
				if (TestResultCd == "")
					Category.CheckCatalogResult = "A";
				else
				{
					if (!TestResultCd.InList("PASSED,PASSAPS"))
					{
						DataView TestResultLookup = (DataView)Category.GetCheckParameter("Test_Result_Code_Lookup_Table").ParameterValue;
						if (!LookupCodeExists(TestResultCd, TestResultLookup))
							Category.CheckCatalogResult = "B";
						else
							Category.CheckCatalogResult = "C";
					}
				}
				if (string.IsNullOrEmpty(Category.CheckCatalogResult))
					if (Category.GetCheckParameter("OOC_Test_Calc_Result").ValueAsString() == "FAILED")
						Category.CheckCatalogResult = "D";
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "ONOFF36");
			}

			return ReturnVal;
		}

		#endregion

		#region ONOFF37
		public static string ONOFF37(cCategory Category, ref bool Log)
		//Online Offline Calibration Test Component ID Valid
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentOOCTest = (DataRowView)Category.GetCheckParameter("Current_OOC_Test").ParameterValue;
				if (CurrentOOCTest["COMPONENT_ID"] == DBNull.Value)
				{
					Category.SetCheckParameter("Evaluate_OOC_Screen", false, eParameterDataType.Boolean);
					Category.CheckCatalogResult = "A";
				}
				else
					if (cDBConvert.ToString(CurrentOOCTest["TEST_RESULT_CD"]) == "ABORTED")
						Category.SetCheckParameter("Evaluate_OOC_Screen", false, eParameterDataType.Boolean);
					else
						Category.SetCheckParameter("Evaluate_OOC_Screen", true, eParameterDataType.Boolean);
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "ONOFF37");
			}

			return ReturnVal;
		}

		#endregion

		#region ONOFF38
		public static string ONOFF38(cCategory Category, ref bool Log)
		//Duplicate Online Offline Calibration Test
		{
			string ReturnVal = "";

			try
			{
				if (Convert.ToBoolean(Category.GetCheckParameter("Test_Number_Valid").ParameterValue))
				{
					DataRowView CurrentOOCTest = (DataRowView)Category.GetCheckParameter("Current_OOC_Test").ParameterValue;
					string TestNum = cDBConvert.ToString(CurrentOOCTest["TEST_NUM"]);
					DataView LocTestRecs = (DataView)Category.GetCheckParameter("Location_Test_Records").ParameterValue;
					string OldFilter = LocTestRecs.RowFilter;
					LocTestRecs.RowFilter = AddToDataViewFilter(OldFilter, "TEST_TYPE_CD = 'ONOFF' AND TEST_NUM = '" + TestNum + "'");
					if ((LocTestRecs.Count > 0 && CurrentOOCTest["TEST_SUM_ID"] == DBNull.Value) ||
						(LocTestRecs.Count > 1 && CurrentOOCTest["TEST_SUM_ID"] != DBNull.Value) ||
						(LocTestRecs.Count == 1 && CurrentOOCTest["TEST_SUM_ID"] != DBNull.Value && CurrentOOCTest["TEST_SUM_ID"].ToString() != LocTestRecs[0]["TEST_SUM_ID"].ToString()))
					{
						Category.SetCheckParameter("Duplicate_Online_Offline_Calibration", true, eParameterDataType.Boolean);
						Category.CheckCatalogResult = "A";
					}
					else
					{
						string TestSumID = cDBConvert.ToString(CurrentOOCTest["TEST_SUM_ID"]);
						if (TestSumID != "")
						{
							DataView QASuppRecords = (DataView)Category.GetCheckParameter("QA_Supplemental_Data_Records").ParameterValue;
							string OldFilter2 = QASuppRecords.RowFilter;
							QASuppRecords.RowFilter = AddToDataViewFilter(OldFilter2, "TEST_NUM = '" + TestNum + "' AND TEST_TYPE_CD = 'ONOFF'");
							if (QASuppRecords.Count > 0 && cDBConvert.ToString(QASuppRecords[0]["TEST_SUM_ID"]) != TestSumID)
							{
								Category.SetCheckParameter("Duplicate_Online_Offline_Calibration", true, eParameterDataType.Boolean);
								Category.CheckCatalogResult = "B";
							}
							else
								Category.SetCheckParameter("Duplicate_Online_Offline_Calibration", false, eParameterDataType.Boolean);
							QASuppRecords.RowFilter = OldFilter2;
						}
						else
							Category.SetCheckParameter("Duplicate_Online_Offline_Calibration", false, eParameterDataType.Boolean);
					}
					LocTestRecs.RowFilter = OldFilter;
				}
				else
					Log = false;
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "ONOFF38");
			}

			return ReturnVal;
		}

		#endregion

		#region ONOFF39
		public static string ONOFF39(cCategory Category, ref bool Log)
		//Online Offline Calibration Test Result Code Valid
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentOOCTest = (DataRowView)Category.GetCheckParameter("Current_OOC_Test").ParameterValue;
				string TestResCd = cDBConvert.ToString(CurrentOOCTest["TEST_RESULT_CD"]);
				if (TestResCd == "")
					Category.CheckCatalogResult = "A";
				else
					if (!TestResCd.InList("PASSED,PASSAPS"))
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
				ReturnVal = Category.CheckEngine.FormatError(ex, "ONOFF39");
			}

			return ReturnVal;
		}

		#endregion

		#region ONOFF40
		public static string ONOFF40(cCategory Category, ref bool Log)
		//Online Offline Calibration Test Component Type Check
		{
			string ReturnVal = "";

			try
			{
				Category.SetCheckParameter("OOC_Injection_Times_Valid", true, eParameterDataType.Boolean);
				Category.SetCheckParameter("OOC_Test_Calc_Result", null, eParameterDataType.String);
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "ONOFF40");
			}

			return ReturnVal;
		}

		#endregion
		#endregion

		#region OOC 41-50
		#region ONOFF41
		public static string ONOFF41(cCategory Category, ref bool Log)
		//Online Zero APS Indicator Valid
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentOOCTest = (DataRowView)Category.GetCheckParameter("Current_OOC_Test").ParameterValue;
				if (CurrentOOCTest["ONLINE_ZERO_APS_IND"] == DBNull.Value)
					Category.CheckCatalogResult = "A";
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "ONOFF41");
			}

			return ReturnVal;
		}

		#endregion

		#region ONOFF42
		public static string ONOFF42(cCategory Category, ref bool Log)
		//Offline Zero APS Indicator Valid
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentOOCTest = (DataRowView)Category.GetCheckParameter("Current_OOC_Test").ParameterValue;
				if (CurrentOOCTest["OFFLINE_ZERO_APS_IND"] == DBNull.Value)
					Category.CheckCatalogResult = "A";
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "ONOFF42");
			}

			return ReturnVal;
		}

		#endregion

		#region ONOFF43
		public static string ONOFF43(cCategory Category, ref bool Log)
		//Online Upscale APS Indicator Valid
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentOOCTest = (DataRowView)Category.GetCheckParameter("Current_OOC_Test").ParameterValue;
				if (CurrentOOCTest["ONLINE_UPSCALE_APS_IND"] == DBNull.Value)
					Category.CheckCatalogResult = "A";
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "ONOFF43");
			}

			return ReturnVal;
		}

		#endregion

		#region ONOFF44
		public static string ONOFF44(cCategory Category, ref bool Log)
		//Offline Upscale APS Indicator Valid
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentOOCTest = (DataRowView)Category.GetCheckParameter("Current_OOC_Test").ParameterValue;
				if (CurrentOOCTest["OFFLINE_UPSCALE_APS_IND"] == DBNull.Value)
					Category.CheckCatalogResult = "A";
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "ONOFF44");
			}

			return ReturnVal;
		}

		#endregion

		#region ONOFF45
		public static string ONOFF45(cCategory Category, ref bool Log)
		//Calculate Online Offline Calibration
		{
			string ReturnVal = "";

			try
			{
				Category.SetCheckParameter("Offline_Upscale_Calc_Result", null, eParameterDataType.Decimal);
				Category.SetCheckParameter("Offline_Upscale_Calc_APS", null, eParameterDataType.Integer);
				Category.SetCheckParameter("Offline_Zero_Calc_Result", null, eParameterDataType.Decimal);
				Category.SetCheckParameter("Offline_Zero_Calc_APS", null, eParameterDataType.Integer);
				Category.SetCheckParameter("Online_Upscale_Calc_Result", null, eParameterDataType.Decimal);
				Category.SetCheckParameter("Online_Upscale_Calc_APS", null, eParameterDataType.Integer);
				Category.SetCheckParameter("Online_Zero_Calc_Result", null, eParameterDataType.Decimal);
				Category.SetCheckParameter("Online_Zero_Calc_APS", null, eParameterDataType.Integer);

				DataRowView CurrentOOCTest = (DataRowView)Category.GetCheckParameter("Current_OOC_Test").ParameterValue;

				string UpsGasCd = cDBConvert.ToString(CurrentOOCTest["UPSCALE_GAS_LEVEL_CD"]);
				decimal OnUpsRefVal = cDBConvert.ToDecimal(CurrentOOCTest["ONLINE_UPSCALE_REF_VALUE"]);
				decimal OnUpsMeasVal = cDBConvert.ToDecimal(CurrentOOCTest["ONLINE_UPSCALE_MEASURED_VALUE"]);
				decimal OnZeroRefVal = cDBConvert.ToDecimal(CurrentOOCTest["ONLINE_ZERO_REF_VALUE"]);
				decimal OnZeroMeasVal = cDBConvert.ToDecimal(CurrentOOCTest["ONLINE_ZERO_MEASURED_VALUE"]);
				bool OnlineOrCodeProblem = !UpsGasCd.InList("MID,HIGH") || OnUpsRefVal == decimal.MinValue || OnUpsMeasVal == decimal.MinValue || OnZeroRefVal == decimal.MinValue || OnZeroMeasVal == decimal.MinValue || OnZeroRefVal >= OnUpsRefVal;

				decimal OffUpsRefVal = cDBConvert.ToDecimal(CurrentOOCTest["OFFLINE_UPSCALE_REF_VALUE"]);
				decimal OffUpsMeasVal = cDBConvert.ToDecimal(CurrentOOCTest["OFFLINE_UPSCALE_MEASURED_VALUE"]);
				decimal OffZeroRefVal = cDBConvert.ToDecimal(CurrentOOCTest["OFFLINE_ZERO_REF_VALUE"]);
				decimal OffZeroMeasVal = cDBConvert.ToDecimal(CurrentOOCTest["OFFLINE_ZERO_MEASURED_VALUE"]);
				bool OfflineProblem = OffUpsRefVal == decimal.MinValue || OffUpsMeasVal == decimal.MinValue || OffZeroRefVal == decimal.MinValue || OffZeroMeasVal == decimal.MinValue;

				bool OnOffProblem = Math.Max(OnZeroRefVal, OffZeroRefVal) >= Math.Min(OnUpsRefVal, OffUpsRefVal);
				DataView SysCompRecs = (DataView)Category.GetCheckParameter("System_Component_Records").ParameterValue;
				SysCompRecs.Sort = "BEGIN_DATE";
				string CompTypeCd = cDBConvert.ToString(SysCompRecs[0]["COMPONENT_TYPE_CD"]);
				string SpanScaleCd = cDBConvert.ToString(CurrentOOCTest["SPAN_SCALE_CD"]);
				bool ComponentSpanProblem = (CompTypeCd == "FLOW" && SpanScaleCd != "") || (CompTypeCd != "FLOW" && !SpanScaleCd.InList("H,L"));

				if (OnlineOrCodeProblem || OfflineProblem || OnOffProblem || ComponentSpanProblem)
					Category.CheckCatalogResult = "A";
				else
				{
					DateTime CompRecBegDate = cDBConvert.ToDate(SysCompRecs[0]["BEGIN_DATE"], DateTypes.START);
					string CompRecBegDateString = CompRecBegDate.ToShortDateString();
					int CompRecBegHr = cDBConvert.ToInteger(SysCompRecs[0]["BEGIN_HOUR"]);
					DataView SpanRecs = (DataView)Category.GetCheckParameter("Span_Records").ParameterValue;
					string OldFilter = SpanRecs.RowFilter;
					DateTime TestBegDate = cDBConvert.ToDate(CurrentOOCTest["BEGIN_DATE"], DateTypes.START);
					int TestBegHr = cDBConvert.ToInteger(SysCompRecs[0]["BEGIN_HOUR"]);

					if (CompRecBegDate != DateTime.MinValue && 0 <= CompRecBegHr && CompRecBegHr <= 23 &&
						(CompRecBegDate > TestBegDate || (CompRecBegDate == TestBegDate && CompRecBegHr > TestBegHr)))
					{
						if (SpanScaleCd != "")
							SpanRecs.RowFilter = AddToDataViewFilter(OldFilter, "COMPONENT_TYPE_CD = '" + CompTypeCd + "' AND SPAN_SCALE_CD = '" + SpanScaleCd +
								"' AND SPAN_VALUE > 0 AND (BEGIN_DATE < '" + CompRecBegDateString + "' OR (BEGIN_DATE = '" + CompRecBegDateString +
								"' AND BEGIN_HOUR <= " + CompRecBegHr + ")) AND (END_DATE IS NULL OR (END_DATE > '" + CompRecBegDateString +
								"' OR (END_DATE = '" + CompRecBegDateString + "' AND END_HOUR > " + CompRecBegHr + ")))");
						else
							SpanRecs.RowFilter = AddToDataViewFilter(OldFilter, "COMPONENT_TYPE_CD = '" + CompTypeCd + "' AND SPAN_SCALE_CD IS NULL" +
								" AND SPAN_VALUE > 0 AND (BEGIN_DATE < '" + CompRecBegDateString + "' OR (BEGIN_DATE = '" + CompRecBegDateString +
								"' AND BEGIN_HOUR <= " + CompRecBegHr + ")) AND (END_DATE IS NULL OR (END_DATE > '" + CompRecBegDateString +
								"' OR (END_DATE = '" + CompRecBegDateString + "' AND END_HOUR > " + CompRecBegHr + ")))");
					}
					else
					{
						string TestBegDateString = TestBegDate.ToShortDateString();
						string TestEndDateString = cDBConvert.ToDate(CurrentOOCTest["END_DATE"], DateTypes.END).ToShortDateString();
						int TestEndHr = cDBConvert.ToInteger(SysCompRecs[0]["END_HOUR"]);
						if (SpanScaleCd != "")
							SpanRecs.RowFilter = AddToDataViewFilter(OldFilter, "COMPONENT_TYPE_CD = '" + CompTypeCd + "' AND SPAN_SCALE_CD = '" + SpanScaleCd +
								"' AND SPAN_VALUE > 0 AND (BEGIN_DATE < '" + TestBegDateString + "' OR (BEGIN_DATE = '" + TestBegDateString +
								"' AND BEGIN_HOUR <= " + TestBegHr + ")) AND (END_DATE IS NULL OR (END_DATE > '" + TestEndDateString +
								"' OR (END_DATE = '" + TestEndDateString + "' AND END_HOUR > " + TestEndHr + ")))");
						else
							SpanRecs.RowFilter = AddToDataViewFilter(OldFilter, "COMPONENT_TYPE_CD = '" + CompTypeCd + "' AND SPAN_SCALE_CD IS NULL" +
								" AND SPAN_VALUE > 0 AND (BEGIN_DATE < '" + TestBegDateString + "' OR (BEGIN_DATE = '" + TestBegDateString +
								"' AND BEGIN_HOUR <= " + TestBegHr + ")) AND (END_DATE IS NULL OR (END_DATE > '" + TestEndDateString +
								"' OR (END_DATE = '" + TestEndDateString + "' AND END_HOUR > " + TestEndHr + ")))");
					}
					if (SpanRecs.Count == 0)
						Category.CheckCatalogResult = "B";
					else
					{
						decimal diff = Math.Abs(OnZeroMeasVal - OnZeroRefVal);
						decimal SpanVal = cDBConvert.ToDecimal(SpanRecs[0]["SPAN_VALUE"]);
						string AcqCd = cDBConvert.ToString(SysCompRecs[0]["ACQ_CD"]);
						Category.SetCheckParameter("Online_Zero_Calc_APS", 0, eParameterDataType.Integer);
						if (CompTypeCd.InList("CO2,O2"))
							Category.SetCheckParameter("Online_Zero_Calc_Result", Math.Round(diff, 1, MidpointRounding.AwayFromZero), eParameterDataType.Decimal);
						else
						{
							decimal OnZeroCalcResult;
							if (CompTypeCd.InList("SO2,NOX"))
							{
								OnZeroCalcResult = Math.Min(Math.Round(diff / SpanVal * 100, 1, MidpointRounding.AwayFromZero), (decimal)9999.9);
								Category.SetCheckParameter("Online_Zero_Calc_Result", OnZeroCalcResult, eParameterDataType.Decimal);
								diff = Math.Round(diff, MidpointRounding.AwayFromZero);
								if (OnZeroCalcResult > (decimal)2.5 && SpanVal < 200 && diff <= 5)
								{
									Category.SetCheckParameter("Online_Zero_Calc_APS", 1, eParameterDataType.Integer);
									Category.SetCheckParameter("Online_Zero_Calc_Result", diff, eParameterDataType.Decimal);
								}
							}
							else
								if (CompTypeCd == "FLOW")
								{
									OnZeroCalcResult = Math.Min(Math.Round(diff / SpanVal * 100, 1, MidpointRounding.AwayFromZero), (decimal)9999.9);
									Category.SetCheckParameter("Online_Zero_Calc_Result", OnZeroCalcResult, eParameterDataType.Decimal);
									diff = Math.Round(diff, 2, MidpointRounding.AwayFromZero);
									if (OnZeroCalcResult > 3 && AcqCd == "DP" && diff <= (decimal)0.01)
									{
										Category.SetCheckParameter("Online_Zero_Calc_APS", 1, eParameterDataType.Integer);
										Category.SetCheckParameter("Online_Zero_Calc_Result", 0m, eParameterDataType.Decimal);
									}
								}
						}
						diff = Math.Abs(OnUpsMeasVal - OnUpsRefVal);
						Category.SetCheckParameter("Online_Upscale_Calc_APS", 0, eParameterDataType.Integer);
						if (CompTypeCd.InList("CO2,O2"))
							Category.SetCheckParameter("Online_Upscale_Calc_Result", Math.Round(diff, 1, MidpointRounding.AwayFromZero), eParameterDataType.Decimal);
						else
						{
							decimal OnUpscaleCalcResult;
							if (CompTypeCd.InList("SO2,NOX"))
							{
								OnUpscaleCalcResult = Math.Min(Math.Round(diff / SpanVal * 100, 1, MidpointRounding.AwayFromZero), (decimal)9999.9);
								Category.SetCheckParameter("Online_Upscale_Calc_Result", OnUpscaleCalcResult, eParameterDataType.Decimal);
								diff = Math.Round(diff, MidpointRounding.AwayFromZero);
								if (OnUpscaleCalcResult > (decimal)2.5 && SpanVal < 200 && diff <= 5)
								{
									Category.SetCheckParameter("Online_Upscale_Calc_APS", 1, eParameterDataType.Integer);
									Category.SetCheckParameter("Online_Upscale_Calc_Result", diff, eParameterDataType.Decimal);
								}
							}
							else
								if (CompTypeCd == "FLOW")
								{
									OnUpscaleCalcResult = Math.Min(Math.Round(diff / SpanVal * 100, 1, MidpointRounding.AwayFromZero), (decimal)9999.9);
									Category.SetCheckParameter("Online_Upscale_Calc_Result", OnUpscaleCalcResult, eParameterDataType.Decimal);
									diff = Math.Round(diff, 2, MidpointRounding.AwayFromZero);
									if (OnUpscaleCalcResult > 3 && AcqCd == "DP" && diff <= (decimal)0.01)
									{
										Category.SetCheckParameter("Online_Upscale_Calc_APS", 1, eParameterDataType.Integer);
										Category.SetCheckParameter("Online_Upscale_Calc_Result", 0m, eParameterDataType.Decimal);
									}
								}
						}
						diff = Math.Abs(OffZeroMeasVal - OffZeroRefVal);
						Category.SetCheckParameter("Offline_Zero_Calc_APS", 0, eParameterDataType.Integer);
						if (CompTypeCd.InList("CO2,O2"))
						{
							diff = Math.Round(diff, 1, MidpointRounding.AwayFromZero);
							Category.SetCheckParameter("Offline_Zero_Calc_Result", diff, eParameterDataType.Decimal);
						}
						else
						{
							decimal OffZeroCalcResult;
							if (CompTypeCd.InList("SO2,NOX"))
							{
								OffZeroCalcResult = Math.Min(Math.Round(diff / SpanVal * 100, 1, MidpointRounding.AwayFromZero), (decimal)9999.9);
								Category.SetCheckParameter("Offline_Zero_Calc_Result", OffZeroCalcResult, eParameterDataType.Decimal);
								diff = Math.Round(diff, MidpointRounding.AwayFromZero);
								if (OffZeroCalcResult > (decimal)2.5 && SpanVal < 200 && diff <= 5)
								{
									Category.SetCheckParameter("Offline_Zero_Calc_APS", 1, eParameterDataType.Integer);
									Category.SetCheckParameter("Offline_Zero_Calc_Result", diff, eParameterDataType.Decimal);
								}
							}
							else
								if (CompTypeCd == "FLOW")
								{
									OffZeroCalcResult = Math.Min(Math.Round(diff / SpanVal * 100, 1, MidpointRounding.AwayFromZero), (decimal)9999.9);
									Category.SetCheckParameter("Offline_Zero_Calc_Result", OffZeroCalcResult, eParameterDataType.Decimal);
									diff = Math.Round(diff, 2, MidpointRounding.AwayFromZero);
									if (OffZeroCalcResult > 3 && AcqCd == "DP" && diff <= (decimal)0.01)
									{
										Category.SetCheckParameter("Offline_Zero_Calc_APS", 1, eParameterDataType.Integer);
										Category.SetCheckParameter("Offline_Zero_Calc_Result", 0m, eParameterDataType.Decimal);
									}
								}
						}
						diff = Math.Abs(OffUpsMeasVal - OffUpsRefVal);
						Category.SetCheckParameter("Offline_Upscale_Calc_APS", 0, eParameterDataType.Integer);
						if (CompTypeCd.InList("CO2,O2"))
							Category.SetCheckParameter("Offline_Upscale_Calc_Result", Math.Round(diff, 1, MidpointRounding.AwayFromZero), eParameterDataType.Decimal);
						else
						{
							decimal OffUpscaleCalcResult;
							if (CompTypeCd.InList("SO2,NOX"))
							{
								OffUpscaleCalcResult = Math.Min(Math.Round(diff / SpanVal * 100, 1, MidpointRounding.AwayFromZero), (decimal)9999.9);
								Category.SetCheckParameter("Offline_Upscale_Calc_Result", OffUpscaleCalcResult, eParameterDataType.Decimal);
								diff = Math.Round(diff, MidpointRounding.AwayFromZero);
								if (OffUpscaleCalcResult > (decimal)2.5 && SpanVal < 200 && diff <= 5)
								{
									Category.SetCheckParameter("Offline_Upscale_Calc_APS", 1, eParameterDataType.Integer);
									Category.SetCheckParameter("Offline_Upscale_Calc_Result", diff, eParameterDataType.Decimal);
								}
							}
							else
								if (CompTypeCd == "FLOW")
								{
									OffUpscaleCalcResult = Math.Min(Math.Round(diff / SpanVal * 100, 1, MidpointRounding.AwayFromZero), (decimal)9999.9);
									Category.SetCheckParameter("Offline_Upscale_Calc_Result", OffUpscaleCalcResult, eParameterDataType.Decimal);
									diff = Math.Round(diff, 2, MidpointRounding.AwayFromZero);
									if (OffUpscaleCalcResult > 3 && AcqCd == "DP" && diff <= (decimal)0.01)
									{
										Category.SetCheckParameter("Offline_Upscale_Calc_APS", 1, eParameterDataType.Integer);
										Category.SetCheckParameter("Offline_Upscale_Calc_Result", 0m, eParameterDataType.Decimal);
									}
								}
						}
					}
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "ONOFF45");
			}

			return ReturnVal;
		}

		#endregion
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

		#endregion
	}
}

