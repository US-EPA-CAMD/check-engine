using System;
using System.Collections.Generic;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Data.Ecmps.Dbo.Table;
using ECMPS.Checks.Data.Ecmps.Dbo.View;
using ECMPS.Checks.Data.EcmpsAux.CrossCheck.Virtual;
using ECMPS.Checks.MonitorPlan;
using ECMPS.Checks.Mp.Parameters;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Enumerations;
using ECMPS.Definitions.Extensions;

namespace ECMPS.Checks.MethodChecks
{
	public class cMethodChecks : cMpChecks
	{

		#region Constructors

		public cMethodChecks(cMpProcess mpProcess)
			: base(mpProcess)
		{
			CheckProcedures = new dCheckProcedure[41];

			CheckProcedures[01] = new dCheckProcedure(METHOD1);
			CheckProcedures[02] = new dCheckProcedure(METHOD2);
			CheckProcedures[03] = new dCheckProcedure(METHOD3);
			CheckProcedures[04] = new dCheckProcedure(METHOD4);
			CheckProcedures[05] = new dCheckProcedure(METHOD5);
			CheckProcedures[06] = new dCheckProcedure(METHOD6);
			CheckProcedures[07] = new dCheckProcedure(METHOD7);
			CheckProcedures[08] = new dCheckProcedure(METHOD8);
			CheckProcedures[09] = new dCheckProcedure(METHOD9);
			CheckProcedures[10] = new dCheckProcedure(METHOD10);
			CheckProcedures[11] = new dCheckProcedure(METHOD11);
			CheckProcedures[12] = new dCheckProcedure(METHOD12);
			CheckProcedures[14] = new dCheckProcedure(METHOD14);
			CheckProcedures[15] = new dCheckProcedure(METHOD15);
			CheckProcedures[16] = new dCheckProcedure(METHOD16);
			CheckProcedures[17] = new dCheckProcedure(METHOD17);
			CheckProcedures[18] = new dCheckProcedure(METHOD18);
			CheckProcedures[19] = new dCheckProcedure(METHOD19);
			CheckProcedures[20] = new dCheckProcedure(METHOD20);
			CheckProcedures[22] = new dCheckProcedure(METHOD22);
			CheckProcedures[23] = new dCheckProcedure(METHOD23);
			CheckProcedures[24] = new dCheckProcedure(METHOD24);
			CheckProcedures[25] = new dCheckProcedure(METHOD25);
			CheckProcedures[26] = new dCheckProcedure(METHOD26);
			CheckProcedures[27] = new dCheckProcedure(METHOD27);
			CheckProcedures[28] = new dCheckProcedure(METHOD28);
			CheckProcedures[29] = new dCheckProcedure(METHOD29);
			CheckProcedures[30] = new dCheckProcedure(METHOD30);
			CheckProcedures[31] = new dCheckProcedure(METHOD31);
			CheckProcedures[32] = new dCheckProcedure(METHOD32);
			CheckProcedures[33] = new dCheckProcedure(METHOD33);
			CheckProcedures[34] = new dCheckProcedure(METHOD34);
			CheckProcedures[35] = new dCheckProcedure(METHOD35);
			CheckProcedures[36] = new dCheckProcedure(METHOD36);
			CheckProcedures[37] = new dCheckProcedure(METHOD37);
			CheckProcedures[38] = new dCheckProcedure(METHOD38);
			CheckProcedures[39] = new dCheckProcedure(METHOD39);
            CheckProcedures[40] = new dCheckProcedure(METHOD40);
        }

		/// <summary>
		/// Constructor used for testing.
		/// </summary>
		/// <param name="mpManualParameters"></param>
		public cMethodChecks(cMpCheckParameters mpManualParameters)
		{
			MpManualParameters = mpManualParameters;
		}

		#endregion


		#region Checks 1- 10

		//Method Start Date Valid
		public string METHOD1(cCategory Category, ref bool Log)
		{
			return Check_ValidStartDate(Category, "Method_Start_Date_Valid", "Current_Method",
												  "Begin_Date", "A", "B", "METHOD1");
		}

		//Method Start Hour Valid
		public string METHOD2(cCategory Category, ref bool Log)
		{
			return Check_ValidStartHour(Category, "Method_Start_Hour_Valid", "Current_Method",
												  "Begin_Hour", "A", "B", "METHOD2");
		}

		//Method End Date Valid
		public string METHOD3(cCategory Category, ref bool Log)
		{
			return Check_ValidEndDate(Category, "Method_End_Date_Valid", "Current_Method",
												"End_Date", "A", "METHOD3");
		}

		//Method End Hour Valid
		public string METHOD4(cCategory Category, ref bool Log)
		{
			return Check_ValidEndHour(Category, "Method_End_Hour_Valid", "Current_Method",
												"End_Hour", "A", "METHOD4");
		}

		//Method Dates and Hours Consistent
		public string METHOD5(cCategory Category, ref bool Log)
		{
			return Check_ConsistentHourRange(Category, "Method_Dates_And_Hours_Consistent",
													   "Current_Method",
													   "Method_Start_Date_Valid",
													   "Method_Start_Hour_Valid",
													   "Method_End_Date_Valid",
													   "Method_End_Hour_Valid",
													   "Begin_Date", "Begin_Hour",
													   "End_Date", "End_Hour",
													   "A", "B", "C", "METHOD5");
		}

		//Monitor Method Active Status
		public string METHOD6(cCategory Category, ref bool Log)
		{
			string ReturnVal = "";

			try
			{
				bool bDatesConsistant = (bool)Category.GetCheckParameter("Method_Dates_And_Hours_Consistent").ParameterValue;
				if (bDatesConsistant)
					ReturnVal = Check_ActiveHourRange(Category, "Method_Active",
																"Current_Method",
																"Method_Evaluation_Begin_Date",
																"Method_Evaluation_Begin_Hour",
																"Method_Evaluation_End_Date",
																"Method_Evaluation_End_Hour");
				else
					Log = false;
			}
			catch (Exception ex)
			{ ReturnVal = Category.CheckEngine.FormatError(ex, "METHOD6"); }

			return ReturnVal;
		}

		public string METHOD7(cCategory category, ref bool log)
		// Method Parameter Code Valid
		{
			string returnVal = "";

			try
			{
				bool methodParamValid = true;
				category.SetCheckParameter("Method_Parameter_Valid", methodParamValid, eParameterDataType.Boolean);

				DataRowView currentMethod = (DataRowView)category.GetCheckParameter("Current_Method").ParameterValue;

				string parameterCd = currentMethod["Parameter_Cd"].AsString();
				string locationType = category.GetCheckParameter("Location_Type").AsString();

				if (parameterCd == null)
				{
					methodParamValid = false;
					category.CheckCatalogResult = "A";
				}
				else if (parameterCd.InList("H2O,OP,NOX,NOXR,NOXM,HGRE,HGRH,HCLRE,HCLRH,HFRE,HFRH,SO2RE,SO2RH") && locationType.InList("CP,MP"))
				{
					methodParamValid = false;
					category.CheckCatalogResult = "B";
				}
				else
				{
					DataView methodParameterList = category.GetCheckParameter("Method_Parameter_List").ValueAsDataView();

					if (cRowFilter.CountRows(methodParameterList,
											 new cFilterCondition[] 
                                       {new cFilterCondition("ParameterCode", parameterCd), 
                                        new cFilterCondition("CategoryCode", "METHOD")}) == 0)
					{
						methodParamValid = false;
						category.CheckCatalogResult = "C";
					}
				}

				category.SetCheckParameter("Method_Parameter_Valid", methodParamValid, eParameterDataType.Boolean);
			}
			catch (Exception ex)
			{
				returnVal = category.CheckEngine.FormatError(ex, "METHOD7");
			}

			return returnVal;
		}

		//Method Method Code Valid
		public string METHOD8(cCategory Category, ref bool Log)
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentMethod = (DataRowView)Category.GetCheckParameter("Current_Method").ParameterValue;

				Category.SetCheckParameter("Method_Method_Code_Valid", true, eParameterDataType.Boolean);
				Category.SetCheckParameter("Method_Substitute_Data_Code_Valid", true, eParameterDataType.Boolean);

				if (CurrentMethod["Method_Cd"] == DBNull.Value)
				{
					Category.CheckCatalogResult = "A";
					Category.SetCheckParameter("Method_Method_Code_Valid", false, eParameterDataType.Boolean);
					return ReturnVal;
				}

				//
				// get the values we need to check
				//

				string MethodCd = cDBConvert.ToString(CurrentMethod["Method_Cd"]);
				string ParameterCd = cDBConvert.ToString(CurrentMethod["Parameter_Cd"]);
				DateTime EndDate = cDBConvert.ToDate(CurrentMethod["End_Date"], DateTypes.END);
				string sLocationType = cDBConvert.ToString(Category.GetCheckParameter("Location_Type").ParameterValue);

				//
				// get the method code lookup table and filter
				//

				DataView dvMethodCodeLookup = (DataView)Category.GetCheckParameter("Method_Code_Lookup_Table").ParameterValue;

				sFilterPair[] MethodCodeFilter = new sFilterPair[1];
				MethodCodeFilter[0].Field = "Method_Cd";
				MethodCodeFilter[0].Value = MethodCd;

				if (CountRows(dvMethodCodeLookup, MethodCodeFilter) == 0)
				{
					Category.CheckCatalogResult = "B";
					Category.SetCheckParameter("Method_Method_Code_Valid", false, eParameterDataType.Boolean);
					Category.SetCheckParameter("Method_Substitute_Data_Code_Valid", false, eParameterDataType.Boolean);
					return ReturnVal;
				}

				if ((MethodCd == "EXP") && (ParameterCd == "HI"))
				{
					if (!sLocationType.StartsWith("U"))
					{
						Category.CheckCatalogResult = "C";
						Category.SetCheckParameter("Method_Method_Code_Valid", false, eParameterDataType.Boolean);
						Category.SetCheckParameter("Method_Substitute_Data_Code_Valid", false, eParameterDataType.Boolean);
						return ReturnVal;
					}
				}
				if ((MethodCd == "CEMNOXR") && (ParameterCd == "NOX"))
				{
					if (EndDate >= Category.GetCheckParameter("ECMPS_MP_Begin_Date").ValueAsDateTime(DateTypes.START))
					{
						Category.CheckCatalogResult = "D";
						return ReturnVal;
					}
				}
				else
				{
					DataView dvParameterMethodCrossCheck = (DataView)Category.GetCheckParameter("Parameter_to_Method_Cross_Check_Table").ParameterValue;
					sFilterPair[] Filter = new sFilterPair[2];

					Filter[0].Field = "MethodParameterCode";
					Filter[0].Value = ParameterCd;
					Filter[1].Field = "MethodCode";
					Filter[1].Value = MethodCd;

					// get the row count
					int nCount = CountRows(dvParameterMethodCrossCheck, Filter);

					if (nCount > 0)
					{
						if (sLocationType == "CS" &&
							(MethodCd.StartsWith("AD") || MethodCd.InList("EXP,AE,FSA,LTFF,MHHI,LME")))
						{
							Category.CheckCatalogResult = "C";
							Category.SetCheckParameter("Method_Method_Code_Valid", false, eParameterDataType.Boolean);
							return ReturnVal;
						}

						if (sLocationType == "MS" &&
							(MethodCd.StartsWith("AD") || MethodCd.InList("EXP,AE,FSA,LTFF,MHHI,CALC,LME")))
						{
							Category.CheckCatalogResult = "C";
							Category.SetCheckParameter("Method_Method_Code_Valid", false, eParameterDataType.Boolean);
							return ReturnVal;
						}

						if (sLocationType == "CP" &&
							(MethodCd.Contains("CEM") || MethodCd.Contains("CALC") || MethodCd.InList("EXP,LME,MHHI,NOXR,AE,F23,ST")))
						{
							Category.CheckCatalogResult = "C";
							Category.SetCheckParameter("Method_Method_Code_Valid", false, eParameterDataType.Boolean);
							return ReturnVal;
						}

						if (sLocationType == "MP" &&
							(MethodCd.Contains("CEM") || MethodCd.Contains("CALC") || MethodCd.InList("EXP,LME,MHHI,LTFF,F23,ST")))
						{
							Category.CheckCatalogResult = "C";
							Category.SetCheckParameter("Method_Method_Code_Valid", false, eParameterDataType.Boolean);
							return ReturnVal;
						}

						if (sLocationType == "U" && MethodCd.Contains("CALC"))
						{
							Category.CheckCatalogResult = "C";
							Category.SetCheckParameter("Method_Method_Code_Valid", false, eParameterDataType.Boolean);
							Category.SetCheckParameter("Method_Substitute_Data_Code_Valid", false, eParameterDataType.Boolean);
							return ReturnVal;
						}
					}
					else // if not found
					{
						Category.CheckCatalogResult = "E";
						Category.SetCheckParameter("Method_Method_Code_Valid", false, eParameterDataType.Boolean);
						Category.SetCheckParameter("Method_Substitute_Data_Code_Valid", false, eParameterDataType.Boolean);
						return ReturnVal;
					}
				}

				// must be ok
				Category.SetCheckParameter("Method_Method_Code_Valid", true, eParameterDataType.Boolean);
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "METHOD8");
			}

			return ReturnVal;
		}

		// Method Substitute Data Code Valid
		public string METHOD9(cCategory Category, ref bool Log)
		{
			string ReturnVal = "";

			try
			{
				bool bMethodCdValid = (bool)Category.GetCheckParameter("Method_Method_Code_Valid").ParameterValue;
				if (bMethodCdValid)
				{
					bool bSubstituteDataCdValid = true;
					DataRowView CurrentMethod = (DataRowView)Category.GetCheckParameter("Current_Method").ParameterValue;
					string MethodCd = cDBConvert.ToString(CurrentMethod["Method_Cd"]);
					string ParameterCd = cDBConvert.ToString(CurrentMethod["Parameter_Cd"]);
					bool bParameterCdValid = (bool)Category.GetCheckParameter("Method_Parameter_Valid").ParameterValue;

					DataView dvMethod2SubDataCdCrossCheck = (DataView)Category.GetCheckParameter("Method_To_Substitute_Data_Code_Cross_Check_Table").ParameterValue;
					string sMethod2SubDataCdFilter = dvMethod2SubDataCdCrossCheck.RowFilter;
					string sFilter = "";
					if (ParameterCd.InList("HGRE,HGRH,HCLRE,HCLRH,HFRE,HFRH,SO2RE,SO2RH"))
					{
						if (CurrentMethod["sub_data_cd"] != DBNull.Value)
						{
							bSubstituteDataCdValid = false;
							Category.CheckCatalogResult = "C";
						}
					}
					else
					{
						if (CurrentMethod["sub_data_cd"] == DBNull.Value)
						{
							if (LookupCodeExists(MethodCd, "MethodCode", dvMethod2SubDataCdCrossCheck) == true)
							{
								bSubstituteDataCdValid = false;
								Category.CheckCatalogResult = "A";
							}
						}
						else
						{
							string SubstituteDataCd = cDBConvert.ToString(CurrentMethod["sub_data_cd"]);

							DataView dvSDLookup = (DataView)Category.GetCheckParameter("Substitute_Data_Code_Lookup_Table").ParameterValue;
							if (LookupCodeExists(SubstituteDataCd, "sub_data_cd", dvSDLookup) == false)
							{
								bSubstituteDataCdValid = false;
								Category.CheckCatalogResult = "B";
							}
							else
							{

								sFilter = AddToDataViewFilter(sMethod2SubDataCdFilter, string.Format("MethodCode='{0}' AND SubstituteDataCode='{1}'", MethodCd, SubstituteDataCd));
								dvMethod2SubDataCdCrossCheck.RowFilter = sFilter;
								if (dvMethod2SubDataCdCrossCheck.Count == 0)
								{
									{
										bSubstituteDataCdValid = false;
										Category.CheckCatalogResult = "C";
									}
								}
								else
								{
									bool bParamCdNull = false;
									if (dvMethod2SubDataCdCrossCheck.Count == 1)
									{
										if (dvMethod2SubDataCdCrossCheck[0]["ParameterCode"] == DBNull.Value)
											bParamCdNull = true;
									}

									if (dvMethod2SubDataCdCrossCheck.Count > 1 || !bParamCdNull)
									{
										if (bParameterCdValid)
										{
											bool bFound = false;
											foreach (DataRowView row in dvMethod2SubDataCdCrossCheck)
											{
												if (ParameterCd == row["ParameterCode"].ToString())
												{
													bFound = true;
													break;
												}
											}

											if (bFound == false)
											{
												bSubstituteDataCdValid = false;
												Category.CheckCatalogResult = "C";
											}
										}
									}
								}
							}
						}
					}

					// reset this dude
					dvMethod2SubDataCdCrossCheck.RowFilter = sMethod2SubDataCdFilter;

					Category.SetCheckParameter("Method_Substitute_Data_Code_Valid", bSubstituteDataCdValid, eParameterDataType.Boolean);
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "METHOD9");
			}

			return ReturnVal;
		}

		// Method Bypass Approach Code Valid
		public string METHOD10(cCategory Category, ref bool Log)
		{
			string ReturnVal = "";

			try
			{
				bool bBypassApproachCdValid = true;

				DataRowView CurrentMethod = (DataRowView)Category.GetCheckParameter("Current_Method").ParameterValue;
				if (CurrentMethod["Bypass_approach_Cd"] != DBNull.Value)
				{
					// Locate BypassApproachCode in the Bypass Approach Code Lookup Table

					string BypassApproachCd = cDBConvert.ToString(CurrentMethod["Bypass_approach_Cd"]);
					DataView dvBACLookup = (DataView)Category.GetCheckParameter("Bypass_Approach_Code_Lookup_Table").ParameterValue;
					if (LookupCodeExists(BypassApproachCd, "bypass_approach_cd", dvBACLookup) == false)
					{
						bBypassApproachCdValid = false;
						Category.CheckCatalogResult = "A";
					}
					else
					{
						string MethodCd = cDBConvert.ToString(CurrentMethod["Method_Cd"]);
						string ParameterCd = cDBConvert.ToString(CurrentMethod["Parameter_Cd"]);
						bool bMethodCdValid = (bool)Category.GetCheckParameter("Method_Method_Code_Valid").ParameterValue;
						bool bParameterCdValid = (bool)Category.GetCheckParameter("Method_Parameter_Valid").ParameterValue;

						if ((bParameterCdValid && !ParameterCd.InList("SO2,NOX,NOXR")) ||
							(bMethodCdValid && !MethodCd.InList("AMS,NOXR,CEM,CEMF23")))
						{
							bBypassApproachCdValid = false;
							Category.CheckCatalogResult = "B";
						}
					}
				}

				Category.SetCheckParameter("Method_Bypass_Approach_Code_Valid", bBypassApproachCdValid, eParameterDataType.Boolean);
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "METHOD10");
			}

			return ReturnVal;
		}

		#endregion


		#region Checks 11 - 20

		// Overlapping Methods
		public string METHOD11(cCategory Category, ref bool Log)
		{
			// For a Monitoring Method record with a valid ParameterCode and consistent dates:

			// Locate another Monitoring Method record for the location with a ParameterCode equal 
			// to the ParameterCode in the current record and a BeginDate/BeginHour that is on or after 
			// the BeginDate/BeginHour in the current record and is on or before the Method Evaluation End Date/Hour, 
			// and a EndDate/EndHour that is null or is on or after the Method Evaluation Begin Date/Hour.

			//      If found,
			//          return result A.

			string ReturnVal = "";

			try
			{
				DataRowView CurrentMethod = (DataRowView)Category.GetCheckParameter("Current_Method").ParameterValue;
				DataView dvMethodRecords = (DataView)Category.GetCheckParameter("Method_Records").ParameterValue;

				bool bParameterCdValid = (bool)Category.GetCheckParameter("Method_Parameter_Valid").ParameterValue;
				bool bDatesConsistant = (bool)Category.GetCheckParameter("Method_Dates_And_Hours_Consistent").ParameterValue;

				if (bParameterCdValid && bDatesConsistant)
				{
					string sFilter = dvMethodRecords.RowFilter;

					string ParameterCd = cDBConvert.ToString(CurrentMethod["Parameter_Cd"]);
					DateTime EvalEndDate = (DateTime)Category.GetCheckParameter("Method_Evaluation_End_Date").ParameterValue;
					DateTime EvalBeginDate = (DateTime)Category.GetCheckParameter("Method_Evaluation_Begin_Date").ParameterValue;
					int EvalEndHour = (int)Category.GetCheckParameter("Method_Evaluation_End_Hour").ParameterValue;
					int EvalBeginHour = (int)Category.GetCheckParameter("Method_Evaluation_Begin_Hour").ParameterValue;
					DateTime CurrentBeginDate = cDBConvert.ToDate(CurrentMethod["begin_date"], DateTypes.START);
					int CurrentBeginHr = cDBConvert.ToInteger(CurrentMethod["begin_hour"]);


					string sParamFilter = string.Format("Parameter_Cd = '{0}' and (begin_date > '{1}' or begin_date = '{1}' and begin_hour >= {2})",
														ParameterCd, CurrentBeginDate.ToShortDateString(), CurrentBeginHr);

					dvMethodRecords.RowFilter = AddToDataViewFilter(sFilter, sParamFilter);
					//bool bNoDups = CheckForDuplicateRecords(Category, dvMethodRecords, EvalBeginDate, EvalEndDate, EvalBeginHour, EvalEndHour);
					dvMethodRecords.RowFilter = AddEvaluationDateHourRangeToDataViewFilter(dvMethodRecords.RowFilter, EvalBeginDate, EvalEndDate,
																						   EvalBeginHour, EvalEndHour, false, true);
					if (dvMethodRecords.Count > 1 ||
						(dvMethodRecords.Count == 1 && dvMethodRecords[0]["mon_method_id"] != CurrentMethod["mon_method_id"]))
						Category.CheckCatalogResult = "A";
					//if (!bNoDups)
					//{
					//    Category.CheckCatalogResult = "A";
					//}
					// reset the filter!
					dvMethodRecords.RowFilter = sFilter;
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "METHOD11");
			}

			return ReturnVal;
		}

		public string METHOD12(cCategory Category, ref bool Log)
		// HI Methods Valid for Linked Locations
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentMethod = (DataRowView)Category.GetCheckParameter("Current_Method").ParameterValue;

				string ParameterCd = cDBConvert.ToString(CurrentMethod["Parameter_Cd"]);
				bool bMethodCodeValid = (bool)Category.GetCheckParameter("Method_Method_Code_Valid").ParameterValue;
				bool bDatesConsistant = (bool)Category.GetCheckParameter("Method_Dates_And_Hours_Consistent").ParameterValue;
				int nNonLoadBasedInd = (int)Category.GetCheckParameter("Non_Load_Based_Indicator").ParameterValue;

				if (ParameterCd.InList("HI,HIT") && bMethodCodeValid && bDatesConsistant)
				{
					string MethodCd = cDBConvert.ToString(CurrentMethod["Method_Cd"]);
					string sLocationType = cDBConvert.ToString(Category.GetCheckParameter("Location_Type").ParameterValue);
					DateTime EvalEndDate = (DateTime)Category.GetCheckParameter("Method_Evaluation_End_Date").ParameterValue;
					DateTime EvalBeginDate = (DateTime)Category.GetCheckParameter("Method_Evaluation_Begin_Date").ParameterValue;
					int EvalEndHour = (int)Category.GetCheckParameter("Method_Evaluation_End_Hour").ParameterValue;
					int EvalBeginHour = (int)Category.GetCheckParameter("Method_Evaluation_Begin_Hour").ParameterValue;

					if (MethodCd.Contains("CALC") || !sLocationType.StartsWith("U"))
					{
						// Locate all Unit Stack Configuration records for the location where the BeginDate 
						// is on or before the Method Evaluation End Date 
						// and the EndDate is null or is on or after the Method Evaluation Begin Date.

						DataView dvAttribRecords = (DataView)Category.GetCheckParameter("Facility_Location_Attribute_Records").ParameterValue;
						//DataView dvMonitorMethodRecords = (DataView)Category.GetCheckParameter("Method_Records").ParameterValue; 
						DataView dvFacilityMethodRecords = (DataView)Category.GetCheckParameter("Facility_Method_Records").ParameterValue;

						DataView unitStackConfigurationView;
						{
							DataView unitStackConfigurationRecords = Category.GetCheckParameter("Unit_Stack_Configuration_Records").AsDataView();

							if (sLocationType.InList("CS,MS,CP,MP"))
								unitStackConfigurationView
								  = cRowFilter.FindRows(unitStackConfigurationRecords,
														new cFilterCondition[] { new cFilterCondition("STACK_PIPE_MON_LOC_ID", Category.CurrentMonLocId) });
							else
								unitStackConfigurationView
								  = cRowFilter.FindRows(unitStackConfigurationRecords,
														new cFilterCondition[] { new cFilterCondition("MON_LOC_ID", Category.CurrentMonLocId) });
						}

						string sAttribFilter = dvAttribRecords.RowFilter;
						string sMonMethodFilter = dvFacilityMethodRecords.RowFilter;
						string sUSConfigFilter = unitStackConfigurationView.RowFilter;
						string sFilter = "";

						sFilter = AddEvaluationDateRangeToDataViewFilter(sUSConfigFilter, EvalBeginDate, EvalEndDate, false, true, false);
						unitStackConfigurationView.RowFilter = sFilter;

						if (MethodCd == "LTFF")
						{
							if (!Method12_Helper(Category, unitStackConfigurationView, dvFacilityMethodRecords,
												 "HIT", "method_cd like '%CALC%'",
												 eMethod12SearchType.AllRecords,
												 EvalBeginDate, EvalBeginHour, EvalEndDate, EvalEndHour))
								Category.CheckCatalogResult = "A";
						}
						else if (sLocationType.InList("CP,MP") && MethodCd == "AD")
						{
							if (!Method12_Helper(Category, unitStackConfigurationView, dvFacilityMethodRecords,
												 "HI", "method_cd like '%CALC%'",
												 eMethod12SearchType.AllRecords,
												 EvalBeginDate, EvalBeginHour, EvalEndDate, EvalEndHour))
								Category.CheckCatalogResult = "B";
						}
						else if (sLocationType.InList("CS,MS") && MethodCd.InList("CEM,AMS"))
						{
							if (!Method12_Helper(Category, unitStackConfigurationView, dvFacilityMethodRecords,
												 "HI", "method_cd like '%CALC%'",
												 eMethod12SearchType.AtLeastOneRecord,
												 EvalBeginDate, EvalBeginHour, EvalEndDate, EvalEndHour))
								Category.CheckCatalogResult = "C";
						}
						else if (sLocationType == "CS" && MethodCd == "CALC")
						{
							if (!Method12_Helper(Category, unitStackConfigurationView, dvFacilityMethodRecords,
												 "HI", "(method_cd like 'AD%' or method_cd in ('CEM','AMS','CALC'))",
												 eMethod12SearchType.AllRecords,
												 EvalBeginDate, EvalBeginHour, EvalEndDate, EvalEndHour))
								Category.CheckCatalogResult = "D";
						}
						else if (sLocationType.StartsWith("U") && MethodCd.Contains("CALC"))
						{
							//bool bContinue = true, 
							bool bSpan = false;
							string sStackPipeID = "";
							//string sBypassInd = "";
							int rowcount = 0;
							int rangeCount = 0;
							string sCommonFilter = "";
							string sMethodFound = "false";

							foreach (DataRowView dvRec in unitStackConfigurationView)
							{
								sStackPipeID = cDBConvert.ToString(dvRec["STACK_NAME"]);

								if ( /*bContinue && */sStackPipeID.StartsWith("CS"))
								{
									sFilter = AddToDataViewFilter(sMonMethodFilter, string.Format("STACK_NAME='{0}'", sStackPipeID));
									sFilter = AddToDataViewFilter(sFilter, "parameter_cd='HI' and method_cd in ('CEM','AMS')");
									sFilter = AddEvaluationDateHourRangeToDataViewFilter(sFilter, EvalBeginDate, EvalEndDate, EvalBeginHour, EvalEndHour, false, true);
									dvFacilityMethodRecords.RowFilter = sFilter;
									rowcount += dvFacilityMethodRecords.Count;

									if (dvFacilityMethodRecords.Count > 0)
									{   // we are at a stopping point
										//bContinue = false;
										if (nNonLoadBasedInd == 1)
										{
											Category.CheckCatalogResult = "E";
											break;
										}
										else
										{
											//DateTime USCRecordBeginDate = cDBConvert.ToDate( dvRec["begin_date"], DateTypes.START );
											//DateTime USCRecordEndDate = cDBConvert.ToDate( dvRec["end_date"], DateTypes.END );
											//if( USCRecordBeginDate > EvalBeginDate )
											//    EvalBeginDate = USCRecordBeginDate;
											//if( USCRecordEndDate < EvalEndDate )
											//    EvalEndDate = USCRecordEndDate;
											bSpan = CheckForHourRangeCovered(Category, dvFacilityMethodRecords, EvalBeginDate, EvalBeginHour, EvalEndDate, EvalEndHour, ref rangeCount);
											sMethodFound = "true";
											if (sCommonFilter == "")
												sCommonFilter = sFilter;
											else
												sCommonFilter = "(" + sCommonFilter + ") or (" + sFilter + ")";
											if (bSpan)
												break;
										}
									}
								}
							} // end -- foreach( DataRowView dvRec in dvUnitStackConfigRecords )

							if ( /*bContinue &&*/ string.IsNullOrEmpty(Category.CheckCatalogResult) && sMethodFound == "false")
							{
								//bContinue = true;
								foreach (DataRowView dvRec in unitStackConfigurationView)
								{
									sStackPipeID = cDBConvert.ToString(dvRec["STACK_NAME"]);

									if ( /*bContinue && */sStackPipeID.StartsWith("CP"))
									{
										sFilter = AddToDataViewFilter(sMonMethodFilter, string.Format("STACK_NAME='{0}'", sStackPipeID));
										sFilter = AddToDataViewFilter(sFilter, "parameter_cd in ('HI','HIT') and (method_cd like 'AD%' or method_cd = 'LTFF')");
										sFilter = AddEvaluationDateHourRangeToDataViewFilter(sFilter, EvalBeginDate, EvalEndDate, EvalBeginHour, EvalEndHour, false, true);
										dvFacilityMethodRecords.RowFilter = sFilter;
										rowcount += dvFacilityMethodRecords.Count;

										if (dvFacilityMethodRecords.Count > 0)
										{   // we are at a stopping point
											//bContinue = false;
											if (nNonLoadBasedInd == 1)
											{
												Category.CheckCatalogResult = "E";
												//bContinue = false;
												break;
											}
											else
											{
												//DateTime USCRecordBeginDate = cDBConvert.ToDate(dvRec["begin_date"], DateTypes.START);
												//DateTime USCRecordEndDate = cDBConvert.ToDate(dvRec["end_date"], DateTypes.END);
												//if (USCRecordBeginDate > EvalBeginDate)
												//    EvalBeginDate = USCRecordBeginDate;
												//if (USCRecordEndDate < EvalEndDate)
												//    EvalEndDate = USCRecordEndDate;
												bSpan = CheckForHourRangeCovered(Category, dvFacilityMethodRecords, EvalBeginDate, EvalBeginHour, EvalEndDate, EvalEndHour, ref rangeCount);
												sMethodFound = "true";
												if (sCommonFilter == "")
													sCommonFilter = sFilter;
												else
													sCommonFilter = "(" + sCommonFilter + ") or (" + sFilter + ")";
												if (bSpan)
													break;
											}
										}
									}
								}
							}

							if (/*bContinue &&*/ string.IsNullOrEmpty(Category.CheckCatalogResult) && (sMethodFound == "false" || !bSpan))
							{
								sMethodFound = "";

								foreach (DataRowView dvRec in unitStackConfigurationView)
								{
									sStackPipeID = cDBConvert.ToString(dvRec["STACK_NAME"]);

									if (sStackPipeID.StartsWith("MS"))
									{
										sFilter = AddToDataViewFilter(sAttribFilter, string.Format("STACK_NAME='{0}'", sStackPipeID));
										sFilter = AddToDataViewFilter(sFilter, "bypass_ind=1");
										sFilter = AddEvaluationDateRangeToDataViewFilter(sFilter, EvalBeginDate, EvalEndDate, false, true, false);
										dvAttribRecords.RowFilter = sFilter;
										if (dvAttribRecords.Count == 0)
										{
											sFilter = AddToDataViewFilter(sMonMethodFilter, string.Format("STACK_NAME='{0}'", sStackPipeID));
											sFilter = AddToDataViewFilter(sFilter, "parameter_cd='HI' and method_cd in ('CEM','AMS')");
											sFilter = AddEvaluationDateHourRangeToDataViewFilter(sFilter, EvalBeginDate, EvalEndDate, EvalBeginHour, EvalEndHour, false, true);
											dvFacilityMethodRecords.RowFilter = sFilter;

											if (dvFacilityMethodRecords.Count > 0)
											{
												//DateTime USCRecordBeginDate = cDBConvert.ToDate( dvRec["begin_date"], DateTypes.START );
												//DateTime USCRecordEndDate = cDBConvert.ToDate( dvRec["end_date"], DateTypes.END );
												//if( USCRecordBeginDate > EvalBeginDate )
												//    EvalBeginDate = USCRecordBeginDate;
												//if( USCRecordEndDate < EvalEndDate )
												//    EvalEndDate = USCRecordEndDate;
												if (sCommonFilter == "")
													dvFacilityMethodRecords.RowFilter = sFilter;
												else
													dvFacilityMethodRecords.RowFilter = "(" + sFilter + ") or (" + sCommonFilter + ")";

												{// Test for span of Method Eval and USC intersection
													DateTime spanBeginDate, spanEndDate; int spanBeginHour, spanEndHour;

													DateTime? uscBeginDate = dvRec["BEGIN_DATE"].AsDateTime();
													{
														if (EvalBeginDate > uscBeginDate.Default(DateTime.MinValue))
														{ spanBeginDate = EvalBeginDate; spanBeginHour = EvalBeginHour; }
														else
														{ spanBeginDate = uscBeginDate.Default(DateTime.MinValue); spanBeginHour = 23; }
													}

													DateTime? uscEndDate = dvRec["END_DATE"].AsDateTime();
													{
														if (EvalEndDate < uscEndDate.Default(DateTime.MaxValue))
														{ spanEndDate = EvalEndDate; spanEndHour = EvalEndHour; }
														else
														{ spanEndDate = uscEndDate.Default(DateTime.MaxValue); spanEndHour = 0; }
													}

													if (spanBeginDate.AddHours(spanBeginHour) <= spanEndDate.AddHours(spanEndHour))
														bSpan = CheckForHourRangeCovered(Category,
																						 dvFacilityMethodRecords,
																						 spanBeginDate, spanBeginHour,
																						 spanEndDate, spanEndHour,
																						 ref rangeCount);
													else
														bSpan = true;
												}

												if (sMethodFound == "false" || !bSpan)
												{
													Category.CheckCatalogResult = "F";
													break;
												}
												else
													sMethodFound = "true";
											}
											if (dvFacilityMethodRecords.Count == 0)
											{
												if (sMethodFound == "true")
												{
													Category.CheckCatalogResult = "F";
													break;
												}
												else
													sMethodFound = "false";
											}
										}
									}
								} // end foreach( DataRowView dvRec in dvUnitStackConfigRecords )

								if (string.IsNullOrEmpty(Category.CheckCatalogResult) && sMethodFound != "true")
								{
									sMethodFound = "";
									foreach (DataRowView dvRec in unitStackConfigurationView)
									{
										sStackPipeID = cDBConvert.ToString(dvRec["STACK_NAME"]);
										if (sStackPipeID.StartsWith("MP"))
										{
											sFilter = AddToDataViewFilter(sMonMethodFilter, string.Format("STACK_NAME='{0}'", sStackPipeID));
											sFilter = AddToDataViewFilter(sFilter, "parameter_cd='HI' and method_cd like 'AD%'");
											sFilter = AddEvaluationDateHourRangeToDataViewFilter(sFilter, EvalBeginDate, EvalEndDate, EvalBeginHour, EvalEndHour, false, true);
											dvFacilityMethodRecords.RowFilter = sFilter;

											if (dvFacilityMethodRecords.Count > 0)
											{
												//DateTime USCRecordBeginDate = cDBConvert.ToDate( dvRec["begin_date"], DateTypes.START );
												//DateTime USCRecordEndDate = cDBConvert.ToDate( dvRec["end_date"], DateTypes.END );
												//if( USCRecordBeginDate > EvalBeginDate )
												//    EvalBeginDate = USCRecordBeginDate;
												//if( USCRecordEndDate < EvalEndDate )
												//    EvalEndDate = USCRecordEndDate;
												if (sCommonFilter == "")
													dvFacilityMethodRecords.RowFilter = sFilter;
												else
													dvFacilityMethodRecords.RowFilter = "(" + sFilter + ") or (" + sCommonFilter + ")";

												{// Test for span of Method Eval and USC intersection
													DateTime spanBeginDate, spanEndDate; int spanBeginHour, spanEndHour;

													DateTime? uscBeginDate = dvRec["BEGIN_DATE"].AsDateTime();
													{
														if (EvalBeginDate > uscBeginDate.Default(DateTime.MinValue))
														{ spanBeginDate = EvalBeginDate; spanBeginHour = EvalBeginHour; }
														else
														{ spanBeginDate = uscBeginDate.Default(DateTime.MinValue); spanBeginHour = 23; }
													}

													DateTime? uscEndDate = dvRec["END_DATE"].AsDateTime();
													{
														if (EvalEndDate < uscEndDate.Default(DateTime.MaxValue))
														{ spanEndDate = EvalEndDate; spanEndHour = EvalEndHour; }
														else
														{ spanEndDate = uscEndDate.Default(DateTime.MaxValue); spanEndHour = 0; }
													}

													if (spanBeginDate.AddHours(spanBeginHour) <= spanEndDate.AddHours(spanEndHour))
														bSpan = CheckForHourRangeCovered(Category,
																						 dvFacilityMethodRecords,
																						 spanBeginDate, spanBeginHour,
																						 spanEndDate, spanEndHour,
																						 ref rangeCount);
													else
														bSpan = true;
												}

												if (sMethodFound == "false" || !bSpan)
												{
													Category.CheckCatalogResult = "F";
													break;
												}
												else
													sMethodFound = "true";
												// break;
											}
											if (dvFacilityMethodRecords.Count == 0)
											{
												if (sMethodFound == "true")
												{
													Category.CheckCatalogResult = "F";
													break;
												}
												else
													sMethodFound = "false";
											}
										}
									}   // end foreach( DataRowView dvRec in dvUnitStackConfigRecords )
								} // end if( bMethodFound == false )
								if (sMethodFound != "true")
									Category.CheckCatalogResult = "F";
							} // end if( rowcount == 0 || !bSpan )
							//if (sMethodFound != "true")
							//    Category.CheckCatalogResult = "F";
						} // end -- else if( sLocationType.StartsWith("U") && MethodCd.Contains( "CALC" ) )

						// reset these suckers
						dvAttribRecords.RowFilter = sAttribFilter;
						dvFacilityMethodRecords.RowFilter = sMonMethodFilter;
						unitStackConfigurationView.RowFilter = sUSConfigFilter;
					} // end -- if( MethodCd.Contains( "CALC" ) || !sLocationType.StartsWith( "U" ) )
				} // end -- if( ParameterCd.InList( "HI,HIT" ) && bMethodCodeValid && bDatesConsistant )
				else
					Log = false;
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "METHOD12");
			}

			return ReturnVal;
		}

		#region Method-12 Helpers

		private enum eMethod12SearchType { AllRecords, AtLeastOneRecord }

		private static bool Method12_Helper(cCategory category, DataView unitStackConfigurationRecords, DataView facilityMethodRecords,
											string parameterCd, string methodCdCondition, eMethod12SearchType searchType,
											DateTime evalBeginDate, int evalBeginHour, DateTime evalEndDate, int evalEndHour)
		{
			bool bPassed = true;
			bool bFound = false;


			int nCount = 0;
			string sUnit = "";
			string sFilter = "";
			string sMethodFilter = facilityMethodRecords.RowFilter;

			foreach (DataRowView row in unitStackConfigurationRecords)
			{
				sUnit = cDBConvert.ToString(row["UNIT_ID"]);
				sFilter = string.Format("Unit_ID = '{0}' and Parameter_cd = '{1}' and ({2})", sUnit, parameterCd, methodCdCondition);
				sFilter = AddEvaluationDateHourRangeToDataViewFilter(sFilter, evalBeginDate, evalEndDate, evalBeginHour, evalEndHour, false, true);
				facilityMethodRecords.RowFilter = sFilter;

				if (facilityMethodRecords.Count > 0)
				{   // found for a unit, check the range
					bFound = true;

					//find intersection dates
					DateTime IntrsxnBeginDate = cDBConvert.ToDate(row["begin_date"], DateTypes.START);//unit stack config start Date
					DateTime IntrsxnEndDate = cDBConvert.ToDate(row["end_date"], DateTypes.END);//unit stack config end Date
					int IntrsxnBeginHour = 23;
					int IntrsxnEndHour = 0;

					if (IntrsxnBeginDate < evalBeginDate)
					{
						IntrsxnBeginDate = evalBeginDate;
						IntrsxnBeginHour = evalBeginHour;
					}

					if (IntrsxnEndDate > evalEndDate)
					{
						IntrsxnEndDate = evalEndDate;
						IntrsxnEndHour = evalEndHour;
					}

					if (!CheckForHourRangeCovered(category, facilityMethodRecords, IntrsxnBeginDate, IntrsxnBeginHour, IntrsxnEndDate, IntrsxnEndHour, ref nCount))
					{   // didn't pass, we are done!
						bPassed = false;
						break;
					}
				}
				else
				{
					if (searchType == eMethod12SearchType.AllRecords)
					{
						bPassed = false;
						break;
					}
				}
			}

			// if no method found for any records
			if (!bFound)
				bPassed = false;

			// reset this sucker
			facilityMethodRecords.RowFilter = sMethodFilter;

			return bPassed;
		}

		#endregion

		#region Method14: Method Consistent with Program

		/// <summary>
		/// PROGRAM-10: Program Parameter Active Status
		/// </summary>
		/// <param name="category">The calling check category object.</param>
		/// <param name="log">The log messages flag.</param>
		/// <returns>Returns formatted exception information.</returns>
		public string METHOD14(cCategory category, ref bool log)
		{
			string returnVal = "";

			try
			{
				if (MethodDatesAndHoursConsistent.Value.Default(false))
				{
					string methodParameterCd = CurrentMethod.Value["Parameter_Cd"].AsString();
					string programParameterList = Method14_GetProgramParameterList(methodParameterCd);

					DataView locationProgramParameterView
					  = cRowFilter.FindRows(LocationProgramParameterRecords.Value,
											new cFilterCondition[] 
                                    { 
                                      new cFilterCondition("CLASS", "N,NA,NB", eFilterConditionStringCompare.InList, true), 
                                      //UMCB Date check for less than Evaluation End and not null
                                      new cFilterCondition("UMCB_DATE", MethodEvaluationEndDate.AsBeginDateTime(), eFilterDataType.DateEnded, eFilterConditionRelativeCompare.LessThanOrEqual),
                                      new cFilterCondition("PARAMETER_CD", programParameterList, eFilterConditionStringCompare.InList)  
                                    });

					if (locationProgramParameterView.Count == 0)
					{
						category.CheckCatalogResult = "A";
					}
					else
					{
						// HI Exempt Checks (Non Generic)
						if ((methodParameterCd == "HI") && (CurrentMethod.Value["Method_Cd"].AsString() == "EXP"))
						{
							DataView locationProgramView
							  = cRowFilter.FindActiveRows(LocationProgramRecords.Value,
														  MethodEvaluationBeginDate.AsBeginDateTime(),
														  MethodEvaluationEndDate.AsEndDateTime(),
														  "UNIT_MONITOR_CERT_BEGIN_DATE",
														  "END_DATE",
														  new cFilterCondition[] 
                                            { 
                                              new cFilterCondition("PRG_CD", "ARP"), 
                                              new cFilterCondition("CLASS", "NA,N", eFilterConditionStringCompare.InList, true), 
                                              //UMCB Date check to insure UMCB Date is not null
                                              new cFilterCondition("UNIT_MONITOR_CERT_BEGIN_DATE", MethodEvaluationEndDate.AsBeginDateTime(), eFilterDataType.DateEnded, eFilterConditionRelativeCompare.LessThanOrEqual)
                                            });

							if (locationProgramView.Count > 0)
								category.CheckCatalogResult = "A";
							else
								if (CurrentMethod.Value["END_DATE"].AsDateTime() == null)
									category.CheckCatalogResult = "B";
						}

						else
						{
							DateTime? earliestUmcbJanuary1AndErbDate, latestEndDate;
							{
								Method14_GetProgramParameterDates(locationProgramParameterView, out earliestUmcbJanuary1AndErbDate, out latestEndDate);
							}

							if (MethodEvaluationBeginDate.AsBeginDateTime() < earliestUmcbJanuary1AndErbDate.Default(DateTime.MinValue))

								category.CheckCatalogResult = "C";

							else if (latestEndDate.HasValue && (latestEndDate.Value < MethodEvaluationEndDate.AsEndDateTime()))

								category.CheckCatalogResult = "G";

							else if (!CheckForDateRangeCovered(category,
															   LocationProgramParameterRecords.Value,
															   "PARAM_BEGIN_DATE", "PARAM_END_DATE",
															   MethodEvaluationBeginDate.AsBeginDateTime(),
															   MethodEvaluationEndDate.AsEndDateTime()))

								category.CheckCatalogResult = "H";

							// CO2 and CO2M Specific Checks (Non Generic)
							else if (methodParameterCd.InList("CO2,CO2M"))
							{
								DataView locationProgramParameterRequiredView
								  = cRowFilter.FindRows(locationProgramParameterView, new cFilterCondition[] { new cFilterCondition("REQUIRED_IND", 1, eFilterDataType.Integer) });

								bool notCoveredByProgram;
								{
									if (locationProgramParameterRequiredView.Count == 0)
										notCoveredByProgram = true;
									else
									{
										DateTime? earliestRequiredUmcbJanuary1AndErbDate, dummyEndDate;
										{
											Method14_GetProgramParameterDates(locationProgramParameterRequiredView, out earliestRequiredUmcbJanuary1AndErbDate, out dummyEndDate);
										}

										notCoveredByProgram = ((MethodEvaluationBeginDate.AsBeginDateTime() < earliestRequiredUmcbJanuary1AndErbDate));
									}
								}

								if (notCoveredByProgram)
								{
									DataView locationReportingFrequencyView;
									{
										DateTime methodEvaluationQuarterBegan
										  = new DateTime(MethodEvaluationBeginDate.AsBeginDateTime().Year,
														 3 * (MethodEvaluationBeginDate.AsBeginDateTime().Quarter() - 1) + 1,
														 1);

										DateTime methodEvaluationQuarterEnded;
										{
											if (MethodEvaluationEndDate.AsEndDateTime().Year < DateTime.MaxValue.Year)
											{
												methodEvaluationQuarterEnded = (new DateTime(MethodEvaluationEndDate.AsEndDateTime().Year,
																							 3 * (MethodEvaluationEndDate.AsEndDateTime().Quarter() - 1) + 1,
																							 1)).AddMonths(3).AddDays(-1);
											}
											else
												methodEvaluationQuarterEnded = MethodEvaluationEndDate.AsEndDateTime();
										}

										locationReportingFrequencyView
										  = cRowFilter.FindActiveRows(LocationReportingFrequencyRecords.Value,
																	  methodEvaluationQuarterBegan,
																	  methodEvaluationQuarterEnded,
																	  new cFilterCondition[] { new cFilterCondition("REPORT_FREQ_CD", "OS") });
									}

									if (locationReportingFrequencyView.Count > 0)
									{
										category.CheckCatalogResult = "D";
									}
									else
									{
										if ((MethodEvaluationBeginDate.AsBeginDateTime().Month != 1) || (MethodEvaluationBeginDate.AsBeginDateTime().Day != 1))
										{
											// Search for methods that begin before this method and end during or after the year this method began
											DataView methodView = cRowFilter.FindRows(MethodRecords.Value,
																					  new cFilterCondition[] { new cFilterCondition("BEGIN_DATE", 
                                                                                                            MethodEvaluationBeginDate.AsBeginDateTime(), 
                                                                                                            eFilterDataType.DateBegan, 
                                                                                                            eFilterConditionRelativeCompare.LessThan),
                                                                                       new cFilterCondition("END_DATE", 
                                                                                                            new DateTime(MethodEvaluationBeginDate.AsBeginDateTime().Year, 1, 1), 
                                                                                                            eFilterDataType.DateEnded, 
                                                                                                            eFilterConditionRelativeCompare.GreaterThanOrEqual),
                                                                                       new cFilterCondition("MON_METHOD_ID",
                                                                                                            CurrentMethod.Value["MON_METHOD_ID"].AsString(),
                                                                                                            true)});

											if (methodView.Count > 0)
												// If method exists that was effective during the begin year of this method
												// and started before this method then the begin date of this method is invalid.
												// CO2 methods not associated with a program must begin on January 1st.
												category.CheckCatalogResult = "E";
											else
												// Warn that method is not associated with a program and is only valid if reported
												// for Greenhouse Gas Mandatory Reporting Rule.
												category.CheckCatalogResult = "F";
										}
										else
										{
											// Warn that method is not associated with a program and is only valid if reported
											// for Greenhouse Gas Mandatory Reporting Rule.
											category.CheckCatalogResult = "F";
										}
									}
								}
							}

						}
					}
				}
				else
					log = false;
			}
			catch (Exception ex)
			{
				returnVal = category.CheckEngine.FormatError(ex);
			}

			return returnVal;
		}


		#region Helper Methods

		/// <summary>
		/// Determines the UNIT_PROGRAM UMCB, ERB and End dates to use from the previously filtered view.
		/// </summary>
		/// <param name="locationProgramView">Previous filtered view from which to pull dates.</param>
		/// <param name="earliestUmcbAdjustedAndErbDate">The earliest date based on January 1st of the UMCB year and the ERB for the earliest UMCB.</param>
		/// <param name="latestEndDate">The latest end date.  If a null end date exists then null.</param>
		private void Method14_GetProgramDates(DataView locationProgramView,
											  out DateTime? earliestUmcbJanuary1AndErbDate,
											  out DateTime? latestEndDate)
		{
			DateTime earliestUmcbDate = DateTime.MaxValue;

			earliestUmcbJanuary1AndErbDate = null;
			latestEndDate = DateTime.MinValue;

			foreach (DataRowView locationProgramRow in locationProgramView)
			{
				DateTime? umcbDate = locationProgramRow["UNIT_MONITOR_CERT_BEGIN_DATE"].AsDateTime();
				DateTime? erbDate = locationProgramRow["EMISSIONS_RECORDING_BEGIN_DATE"].AsDateTime();
				DateTime? endDate = locationProgramRow["END_DATE"].AsDateTime();

				DateTime? adjustedUmcbDate = (umcbDate.HasValue ? new DateTime(umcbDate.Value.Year, 1, 1) : (DateTime?)null);

				if (umcbDate.HasValue && (umcbDate.Value < earliestUmcbDate))
				{
					if (adjustedUmcbDate.Default(DateTime.MinValue) < erbDate.Default(DateTime.MinValue))
						earliestUmcbJanuary1AndErbDate = adjustedUmcbDate;
					else
						earliestUmcbJanuary1AndErbDate = erbDate;

					earliestUmcbDate = umcbDate.Value;
				}

				if (endDate.Default(DateTime.MaxValue) > latestEndDate.Value)
					latestEndDate = endDate.Default(DateTime.MaxValue);
			}

			if (latestEndDate.Value == DateTime.MaxValue)
				latestEndDate = null;
		}


		/// <summary>
		/// Returns values from a Program Parameter To Method Parameter cross check row based on the passed Method Parameter code.
		/// </summary>
		/// <param name="methodParameterCd">The program parameter code to locate in the cross check row.</param>
		/// <returns></returns>
		private string Method14_GetProgramParameterList(string methodParameterCd)
		{
			List<string> programParameterList = new List<string>();

			foreach (DataRowView crossCheckRow in CrossCheckProgramParameterToMethodParameter.Value)
			{
				if (methodParameterCd.InList(crossCheckRow["MethodParameterList"].AsString()))
				{
					string programParameterCd = crossCheckRow["ProgramParameterCd"].AsString();

					if (!programParameterList.Contains(programParameterCd))
						programParameterList.Add(programParameterCd);
				}
			}

			string result = null;
			{
				string delim = null;

				foreach (string programParameterCd in programParameterList)
				{
					result += delim + programParameterCd;
					delim = ",";
				}
			}

			return result;
		}


		/// <summary>
		/// Determines the UNIT_PROGRAM UMCB, ERB and End dates to use from the previously filtered view.
		/// </summary>
		/// <param name="locationProgramParameterView">Previous filtered view from which to pull dates.</param>
		/// <param name="earliestUmcbAdjustedAndErbDate">The earliest date based on January 1st of the UMCB year and the ERB for the earliest UMCB.</param>
		/// <param name="latestEndDate">The latest end date.  If a null end date exists then null.</param>
		private void Method14_GetProgramParameterDates(DataView locationProgramParameterView,
													   out DateTime? earliestUmcbJanuary1AndErbDate,
													   out DateTime? latestEndDate)
		{
			DateTime earliestUmcbDate = DateTime.MaxValue;

			earliestUmcbJanuary1AndErbDate = null;
			latestEndDate = DateTime.MinValue;

			foreach (DataRowView locationProgramRow in locationProgramParameterView)
			{
				DateTime? umcbDate = locationProgramRow["UMCB_DATE"].AsDateTime();
				DateTime? erbDate = locationProgramRow["ERB_DATE"].AsDateTime();
				DateTime? endDate = locationProgramRow["PRG_END_DATE"].AsDateTime();

				DateTime? adjustedUmcbDate = (umcbDate.HasValue ? new DateTime(umcbDate.Value.Year, 1, 1) : (DateTime?)null);

				if (umcbDate.HasValue && (umcbDate.Value < earliestUmcbDate))
				{
					if (adjustedUmcbDate.Default(DateTime.MinValue) < erbDate.Default(DateTime.MinValue))
						earliestUmcbJanuary1AndErbDate = adjustedUmcbDate;
					else
						earliestUmcbJanuary1AndErbDate = erbDate;

					earliestUmcbDate = umcbDate.Value;
				}

				if (endDate.Default(DateTime.MaxValue) > latestEndDate.Value)
					latestEndDate = endDate.Default(DateTime.MaxValue);
			}

			if (latestEndDate.Value == DateTime.MaxValue)
				latestEndDate = null;
		}

		#endregion

		#endregion

		// Method Consistent with Fuels
		public string METHOD15(cCategory Category, ref bool Log)
		{
			string ReturnVal = "";

			try
			{
				bool bMethodCdValid = (bool)Category.GetCheckParameter("Method_Method_Code_Valid").ParameterValue;
				bool bParameterCdValid = (bool)Category.GetCheckParameter("Method_Parameter_Valid").ParameterValue;
				bool bDatesConsistant = (bool)Category.GetCheckParameter("Method_Dates_And_Hours_Consistent").ParameterValue;
				if (bParameterCdValid && bMethodCdValid && bDatesConsistant)
				{
					DataRowView CurrentMethod = (DataRowView)Category.GetCheckParameter("Current_Method").ParameterValue;
					DataView dvFuelCodeLookup = (DataView)Category.GetCheckParameter("Fuel_Code_Lookup_Table").ParameterValue;
					DataView dvLocationFuelRecords = (DataView)Category.GetCheckParameter("Location_Fuel_Records").ParameterValue;
					DataView dvQualRecords = (DataView)Category.GetCheckParameter("Qualification_Records").ParameterValue;

					string MethodCd = cDBConvert.ToString(CurrentMethod["Method_Cd"]);
					string ParameterCd = cDBConvert.ToString(CurrentMethod["Parameter_Cd"]);
					DateTime EvalEndDate = (DateTime)Category.GetCheckParameter("Method_Evaluation_End_Date").ParameterValue;
					DateTime EvalBeginDate = (DateTime)Category.GetCheckParameter("Method_Evaluation_Begin_Date").ParameterValue;

					string sFilter = "", sFuelCd = "", sFuelGroupCd = "";
					string sFuelCdLookupFilter = dvFuelCodeLookup.RowFilter;
					string sQualFilter = dvQualRecords.RowFilter;
					string sLocFuelFilter = dvLocationFuelRecords.RowFilter;

					if (ParameterCd == "OP" && MethodCd == "EXP")
					{
						DataView dvControlRecords = (DataView)Category.GetCheckParameter("Location_Control_Records").ParameterValue;
						string sControlFilter = dvControlRecords.RowFilter;
						dvControlRecords.RowFilter = AddToDataViewFilter(sControlFilter, "control_cd in ('WL','WLS','WS') and Install_Date is null and Orig_ind = 1");
						if (dvControlRecords.Count == 0)
						{
							dvControlRecords.RowFilter = AddToDataViewFilter(sControlFilter, "control_cd in ('WL','WLS','WS')");
							dvControlRecords.RowFilter = AddEvaluationDateRangeToDataViewFilter(dvControlRecords.RowFilter, "INSTALL_DATE", "RETIRE_DATE", EvalBeginDate, EvalEndDate, false, true, true);
						}
						if (dvControlRecords.Count == 0)
						{
							sFilter = AddEvaluationDateRangeToDataViewFilter(sLocFuelFilter, EvalBeginDate, EvalEndDate, false, true, false);
							dvLocationFuelRecords.RowFilter = sFilter;
							foreach (DataRowView row in dvLocationFuelRecords)
							{
								sFuelCd = cDBConvert.ToString(row["FUEL_CD"]);
								if (sFuelCd != "DSL")
								{
									dvFuelCodeLookup.RowFilter = AddToDataViewFilter(sFuelCdLookupFilter, string.Format("FUEL_CD = '{0}'", sFuelCd));
									if (dvFuelCodeLookup.Count == 1)
									{
										sFuelGroupCd = cDBConvert.ToString(dvFuelCodeLookup[0]["FUEL_GROUP_CD"]);
										if (!sFuelGroupCd.InList("GAS,OIL"))
											Category.CheckCatalogResult = "A";

										if (sFuelGroupCd == "OIL")
										{
											sFilter = AddToDataViewFilter(sQualFilter, "QUAL_TYPE_CD = 'GF'");
											sFilter = AddEvaluationDateRangeToDataViewFilter(sFilter, EvalBeginDate, EvalEndDate, false, true, false);
											dvQualRecords.RowFilter = sFilter;

											if (dvQualRecords.Count == 0)
												Category.CheckCatalogResult = "A";
										}
									}
								}

								// break our loop!
								if (Category.CheckCatalogResult == "A")
									break;
							}
						}
						dvControlRecords.RowFilter = sControlFilter;
					}

					if (MethodCd.StartsWith("AD") || MethodCd.StartsWith("LTF") || MethodCd.InList("AE,PEM,LME"))
					{
						sFilter = AddEvaluationDateRangeToDataViewFilter(sLocFuelFilter, EvalBeginDate, EvalEndDate, false, true, false);
						dvLocationFuelRecords.RowFilter = sFilter;
						foreach (DataRowView row in dvLocationFuelRecords)
						{
							sFuelCd = cDBConvert.ToString(row["FUEL_CD"]);
							dvFuelCodeLookup.RowFilter = AddToDataViewFilter(sFuelCdLookupFilter, string.Format("FUEL_CD = '{0}'", sFuelCd));
							if (dvFuelCodeLookup.Count == 1)
							{
								sFuelGroupCd = cDBConvert.ToString(dvFuelCodeLookup[0]["fuel_group_cd"]);
								if (sFuelGroupCd.InList("COAL,OTHER"))
								{
									Category.CheckCatalogResult = "B";
									Category.SetCheckParameter("Invalid_Method_Fuel", "coal or other solid fuels", eParameterDataType.String);
								}
							}

							// break our loop!
							if (Category.CheckCatalogResult == "B")
								break;
						}
					}

					if (MethodCd == "FSA")
					{
						sFilter = AddEvaluationDateRangeToDataViewFilter(sLocFuelFilter, EvalBeginDate, EvalEndDate, false, true, false);
						sFilter = AddToDataViewFilter(sFilter, "FUEL_CD in ('W','OSF')");
						dvLocationFuelRecords.RowFilter = sFilter;
						if (dvLocationFuelRecords.Count > 0)
						{
							Category.CheckCatalogResult = "B";
							Category.SetCheckParameter("Invalid_Method_Fuel", "wood or other solid fuel", eParameterDataType.String);
						}
					}

					// reset these suckers
					dvFuelCodeLookup.RowFilter = sFuelCdLookupFilter;
					dvQualRecords.RowFilter = sQualFilter;
					dvLocationFuelRecords.RowFilter = sLocFuelFilter;
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "METHOD15");
			}

			return ReturnVal;
		}

		// Substitute Data Code Consistent with Non Load Based Indicator
		public string METHOD16(cCategory Category, ref bool Log)
		{
			string ReturnVal = "";

			try
			{
				// For a Monitoring Method record with a valid SubstituteDataCode that begins with "NLB",
				//  If the Location Non Load Based Indicator is not equal to to 1,
				//      return result A.
				DataRowView CurrentMethod = (DataRowView)Category.GetCheckParameter("Current_Method").ParameterValue;
				string SubstituteDataCd = cDBConvert.ToString(CurrentMethod["sub_data_Cd"]);
				int NonLoadBasedIndicator = (int)Category.GetCheckParameter("Non_Load_Based_Indicator").ParameterValue;
				bool bSubstituteDataCdValid = (bool)Category.GetCheckParameter("Method_Substitute_Data_Code_Valid").ParameterValue;

				if (bSubstituteDataCdValid && SubstituteDataCd.StartsWith("NLB") && NonLoadBasedIndicator != 1)
				{
					Category.CheckCatalogResult = "A";
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "METHOD16");
			}

			return ReturnVal;
		}

		// Substitute Data Code Consistent with Program and Reporting Frequency
		public string METHOD17(cCategory Category, ref bool Log)
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentMethod = (DataRowView)Category.GetCheckParameter("Current_Method").ParameterValue;
				string SubstituteDataCd = cDBConvert.ToString(CurrentMethod["sub_data_cd"]);
				bool bSubstituteDataCdValid = (bool)Category.GetCheckParameter("Method_Substitute_Data_Code_Valid").ParameterValue;
				bool bDatesConsistant = (bool)Category.GetCheckParameter("Method_Dates_And_Hours_Consistent").ParameterValue;

				if (bSubstituteDataCdValid && SubstituteDataCd == "OZN75" && bDatesConsistant)
				{
					DateTime EvalEndDate = (DateTime)Category.GetCheckParameter("Method_Evaluation_End_Date").ParameterValue;
					DateTime EvalBeginDate = (DateTime)Category.GetCheckParameter("Method_Evaluation_Begin_Date").ParameterValue;

					int startDateRptPrd = cDateFunctions.ThisReportingPeriod(EvalBeginDate);
					int endDateRptPrd = cDateFunctions.ThisReportingPeriod(EvalEndDate);

					DataView dvReportingFreqRecords = (DataView)Category.GetCheckParameter("Location_Reporting_Frequency_Records").ParameterValue;
					string sOldFilter = dvReportingFreqRecords.RowFilter;

					dvReportingFreqRecords.RowFilter = AddToDataViewFilter(sOldFilter, "REPORT_FREQ_CD = 'Q'");
					//dvReportingFreqRecords.RowFilter = AddEvaluationDateRangeToDataViewFilter(dvReportingFreqRecords.RowFilter, EvalBeginDate, EvalEndDate, false, true, false);
					dvReportingFreqRecords.RowFilter = AddToDataViewFilter(dvReportingFreqRecords.RowFilter, "begin_rpt_period_id <= " + endDateRptPrd +
									" and (end_rpt_period_id is null or end_rpt_period_id >= " + startDateRptPrd + ")");


					if (dvReportingFreqRecords.Count == 0)
					{
						Category.CheckCatalogResult = "A";
					}
					else
					{
						//if (CheckEndQuarterForRetrievedRecords(Category, dvReportingFreqRecords,"END_DATE", EvalEndDate))
						//  Category.CheckCatalogResult = "B";
						int RptFreqCount = dvReportingFreqRecords.Count;
						dvReportingFreqRecords.RowFilter = AddToDataViewFilter(dvReportingFreqRecords.RowFilter, "end_quarter is not null");
						if (dvReportingFreqRecords.Count == RptFreqCount)
						{
							string sort = dvReportingFreqRecords.Sort;
							dvReportingFreqRecords.Sort = "end_rpt_period_id DESC";
							if (cDBConvert.ToInteger(dvReportingFreqRecords[0]["end_rpt_period_id"]) < endDateRptPrd)
								Category.CheckCatalogResult = "B";
							dvReportingFreqRecords.Sort = sort;
						}
					}

					// reset this sucker
					dvReportingFreqRecords.RowFilter = sOldFilter;
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "METHOD17");
			}

			return ReturnVal;
		}

		// Required Unit Control for Bypass Approach
		public string METHOD18(cCategory Category, ref bool Log)
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentMethod = (DataRowView)Category.GetCheckParameter("Current_Method").ParameterValue;
				bool bBypassApproachCdValid = (bool)Category.GetCheckParameter("Method_Bypass_Approach_Code_Valid").ParameterValue;
				bool bDatesConsistant = (bool)Category.GetCheckParameter("Method_Dates_And_Hours_Consistent").ParameterValue;

				if (bBypassApproachCdValid && CurrentMethod["bypass_approach_cd"] != DBNull.Value && bDatesConsistant)
				{
					string BypassApproachCd = cDBConvert.ToString(CurrentMethod["bypass_approach_cd"]);
					string ParameterCd = cDBConvert.ToString(CurrentMethod["Parameter_Cd"]);
					DateTime EvalEndDate = (DateTime)Category.GetCheckParameter("Method_Evaluation_End_Date").ParameterValue;
					DateTime EvalBeginDate = (DateTime)Category.GetCheckParameter("Method_Evaluation_Begin_Date").ParameterValue;

					DataView dvControlRecords = (DataView)Category.GetCheckParameter("Location_Control_Records").ParameterValue;
					DataView dvUnitTypeRecords = (DataView)Category.GetCheckParameter("Location_Unit_Type_Records").ParameterValue;

					string sControlFilter = dvControlRecords.RowFilter;
					string sUnitTypeFilter = dvUnitTypeRecords.RowFilter;

					dvUnitTypeRecords.RowFilter = AddToDataViewFilter(sUnitTypeFilter, "unit_type_cd = 'CC'");
					dvUnitTypeRecords.RowFilter = AddEvaluationDateRangeToDataViewFilter(dvUnitTypeRecords.RowFilter, EvalBeginDate, EvalEndDate, false, true, true);

					int nUnitTypeCount = dvUnitTypeRecords.Count;
					if (nUnitTypeCount == 0 || !CheckEndDateForRetrievedRecords(Category, dvUnitTypeRecords, EvalEndDate))
					{
						string sParamFilter = string.Format("ce_param = '{0}'", ParameterCd);
						if (ParameterCd == "NOXR")
						{
							sParamFilter = "ce_param = 'NOX'";
						}

						dvControlRecords.RowFilter = AddToDataViewFilter(sControlFilter, sParamFilter);
						dvControlRecords.RowFilter = AddEvaluationDateRangeToDataViewFilter(dvControlRecords.RowFilter, "INSTALL_DATE", "RETIRE_DATE", EvalBeginDate, EvalEndDate, false, true, true);

						if (nUnitTypeCount == 0 && dvControlRecords.Count == 0)
							Category.CheckCatalogResult = "A";

						if ((nUnitTypeCount > 0 || dvControlRecords.Count > 0) &&
							(CheckEndDateForRetrievedRecords(Category, dvControlRecords, "RETIRE_DATE", EvalEndDate) &&
							CheckEndDateForRetrievedRecords(Category, dvUnitTypeRecords, EvalEndDate)))
						{
							Category.CheckCatalogResult = "B";
						}
					}

					// reset these bad boys
					dvControlRecords.RowFilter = sControlFilter;
					dvUnitTypeRecords.RowFilter = sUnitTypeFilter;
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "METHOD18");
			}

			return ReturnVal;
		}

		// Bypass Approach Code Consistent with Bypass Stack Indicator
		public string METHOD19(cCategory Category, ref bool Log)
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentMethod = (DataRowView)Category.GetCheckParameter("Current_Method").ParameterValue;
				bool bBypassApproachCdValid = (bool)Category.GetCheckParameter("Method_Bypass_Approach_Code_Valid").ParameterValue;
				bool bDatesConsistant = (bool)Category.GetCheckParameter("Method_Dates_And_Hours_Consistent").ParameterValue;

				string BypassApproachCd = cDBConvert.ToString(CurrentMethod["bypass_approach_cd"]);
				string sLocationType = (string)Category.GetCheckParameter("Location_Type").ParameterValue;

				if (bBypassApproachCdValid && CurrentMethod["bypass_approach_cd"] != DBNull.Value && bDatesConsistant &&
					(sLocationType == "CS" || sLocationType == "MS"))
				{
					DateTime EvalEndDate = (DateTime)Category.GetCheckParameter("Method_Evaluation_End_Date").ParameterValue;
					DateTime EvalBeginDate = (DateTime)Category.GetCheckParameter("Method_Evaluation_Begin_Date").ParameterValue;
					DataView dvAttribRecords = (DataView)Category.GetCheckParameter("Location_Attribute_Records").ParameterValue;

					string sAttribFilter = dvAttribRecords.RowFilter;

					dvAttribRecords.RowFilter = AddToDataViewFilter(sAttribFilter, "bypass_ind = 1");
					dvAttribRecords.RowFilter = AddEvaluationDateRangeToDataViewFilter(dvAttribRecords.RowFilter, EvalBeginDate, EvalEndDate, false, true, false);

					if (dvAttribRecords.Count > 0)
						Category.CheckCatalogResult = "A";

					// reset this sucker
					dvAttribRecords.RowFilter = sAttribFilter;
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "METHOD19");
			}

			return ReturnVal;
		}

		// CEM Methods Consistent
		public string METHOD20(cCategory Category, ref bool Log)
		{
			string ReturnVal = "";

			try
			{
				bool bDatesConsistant = (bool)Category.GetCheckParameter("Method_Dates_And_Hours_Consistent").ParameterValue;

				if (bDatesConsistant)
				{
					DataRowView CurrentMethod = (DataRowView)Category.GetCheckParameter("Current_Method").ParameterValue;

					string MethodCd = cDBConvert.ToString(CurrentMethod["Method_Cd"]);
					string ParameterCd = cDBConvert.ToString(CurrentMethod["Parameter_Cd"]);

					DataView dvMethodRecords = (DataView)Category.GetCheckParameter("Method_Records").ParameterValue;
					string sMethodFilter = dvMethodRecords.RowFilter;

					int EvalEndHour = (int)Category.GetCheckParameter("Method_Evaluation_End_Hour").ParameterValue;
					int EvalBeginHour = (int)Category.GetCheckParameter("Method_Evaluation_Begin_Hour").ParameterValue;
					DateTime EvalEndDate = (DateTime)Category.GetCheckParameter("Method_Evaluation_End_Date").ParameterValue;
					DateTime EvalBeginDate = (DateTime)Category.GetCheckParameter("Method_Evaluation_Begin_Date").ParameterValue;

					int nCount = 0;
					string sFilter = "";

					if (ParameterCd == "H2O")
					{
						sFilter = AddToDataViewFilter(sMethodFilter, "method_cd in ('CEM', 'AMS')");
						sFilter = AddEvaluationDateHourRangeToDataViewFilter(sFilter, EvalBeginDate, EvalEndDate, EvalBeginHour, EvalEndHour, false, true);
						dvMethodRecords.RowFilter = sFilter;

						if (dvMethodRecords.Count == 0)
							Category.CheckCatalogResult = "A";
						else if (!CheckForHourRangeCovered(Category, dvMethodRecords, EvalBeginDate, EvalBeginHour, EvalEndDate, EvalEndHour, ref nCount))
							Category.CheckCatalogResult = "B";
					}

					// reset this sucker
					dvMethodRecords.RowFilter = sMethodFilter;

					if (MethodCd == "CEM")
					{
						if (ParameterCd.InList("NOX,NOXR"))
						{
							sFilter = AddToDataViewFilter(sMethodFilter, "method_cd in ('AE', 'LME', 'MHHI') or method_cd like 'PEM%' or method_cd like 'LTF%'");
							sFilter = AddEvaluationDateHourRangeToDataViewFilter(sFilter, EvalBeginDate, EvalEndDate, EvalBeginHour, EvalEndHour, false, true);
							dvMethodRecords.RowFilter = sFilter;
						}
						else
						{
							sFilter = AddToDataViewFilter(sMethodFilter, "method_cd in ('AE', 'LME', 'MHHI') or method_cd LIKE 'AD%' or method_cd like 'PEM%' or method_cd like 'LTF%'");
							sFilter = AddEvaluationDateHourRangeToDataViewFilter(sFilter, EvalBeginDate, EvalEndDate, EvalBeginHour, EvalEndHour, false, true);
							dvMethodRecords.RowFilter = sFilter;
						}

						if (dvMethodRecords.Count > 0)
						{
							string sInvalidParams = "";
							foreach (DataRowView drMethod in dvMethodRecords)
							{
								sInvalidParams = sInvalidParams.ListAdd(cDBConvert.ToString(drMethod["parameter_cd"]));
							}

							Category.CheckCatalogResult = "C";
							Category.SetCheckParameter("Invalid_Parameters_for_CEM_Method", sInvalidParams.FormatList(), eParameterDataType.String);
						}
					}

					// reset this sucker
					dvMethodRecords.RowFilter = sMethodFilter;
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "METHOD20");
			}

			return ReturnVal;
		}

		#endregion


		#region Checks 21 - 30

		public string METHOD22(cCategory category, ref bool log)
		// Appendix E Method Consistent with HI Method
		{
			string returnVal = "";

			try
			{
				DataRowView currentMethod = (DataRowView)category.GetCheckParameter("Current_Method").ParameterValue;

				string methodCd = cDBConvert.ToString(currentMethod["Method_Cd"]);
				string parameterCd = cDBConvert.ToString(currentMethod["Parameter_Cd"]);

				if (parameterCd.InList("NOXR,CO2,SO2") &&
					category.GetCheckParameter("Method_Method_Code_Valid").AsBoolean().Default() &&
					methodCd.InList("AE,AD"))
				{
					DateTime evalBeginDate = category.GetCheckParameter("Method_Evaluation_Begin_Date").AsDateTime().Default(DateTypes.START);
					int evalBeginHour = category.GetCheckParameter("Method_Evaluation_Begin_Hour").AsHour().Default(DateTypes.START).AsInteger();
					DateTime evalEndedDate = category.GetCheckParameter("Method_Evaluation_End_Date").AsDateTime().Default(DateTypes.END);
					int evalEndedHour = category.GetCheckParameter("Method_Evaluation_End_Hour").AsHour().Default(DateTypes.END).AsInteger();

					DataView methodView;
					{
						DataView methodRecords = category.GetCheckParameter("Method_Records").ValueAsDataView();

						if (methodCd == "AD")
						{
							methodView = cRowFilter.FindActiveRows(methodRecords, evalBeginDate, evalBeginHour, evalEndedDate, evalEndedHour,
																   new cFilterCondition[] 
                                                     {
                                                       new cFilterCondition("PARAMETER_CD", "HI"), 
                                                       new cFilterCondition("METHOD_CD", "AD", eFilterConditionStringCompare.BeginsWith)
                                                     });
						}
						else
						{
							methodView = cRowFilter.FindActiveRows(methodRecords, evalBeginDate, evalBeginHour, evalEndedDate, evalEndedHour,
																   new cFilterCondition[] 
                                                     {
                                                       new cFilterCondition("PARAMETER_CD", "HI"), 
                                                       new cFilterCondition("METHOD_CD", "AD,CALC", eFilterConditionStringCompare.InList)
                                                     });
						}
					}

					if (methodView.Count == 0)
						category.CheckCatalogResult = "A";
					else if (!CheckForHourRangeCovered(category, methodView,
													   evalBeginDate, evalBeginHour,
													   evalEndedDate, evalEndedHour))
						category.CheckCatalogResult = "B";
				}
				else
					log = false;

			}
			catch (Exception ex)
			{
				returnVal = category.CheckEngine.FormatError(ex, "METHOD22");
			}

			return returnVal;
		}

		// LME Methods Consistent
		public string METHOD23(cCategory Category, ref bool Log)
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentMethod = (DataRowView)Category.GetCheckParameter("Current_Method").ParameterValue;
				bool bDatesConsistant = (bool)Category.GetCheckParameter("Method_Dates_And_Hours_Consistent").ParameterValue;
				bool bMethodCdValid = (bool)Category.GetCheckParameter("Method_Method_Code_Valid").ParameterValue;
				string MethodCd = cDBConvert.ToString(CurrentMethod["Method_Cd"]);
				string MonLocId = cDBConvert.ToString(CurrentMethod["mon_loc_Id"]);
				string ParamCd = cDBConvert.ToString(CurrentMethod["parameter_cd"]);

				if (bDatesConsistant && bMethodCdValid && MethodCd.InList("LME,MHHI,LTFF"))
				{
					DateTime EvaluationBeginDate = Category.GetCheckParameter("Method_Evaluation_Begin_Date").ValueAsDateTime(DateTypes.START);
					DateTime EvaluationEndDate = Category.GetCheckParameter("Method_Evaluation_End_Date").ValueAsDateTime(DateTypes.END);
					int EvaluationBeginHour = Category.GetCheckParameter("Method_Evaluation_Begin_Hour").ValueAsInt();
					int EvaluationEndHour = Category.GetCheckParameter("Method_Evaluation_End_Hour").ValueAsInt();

					DataView FacilityMethodRecords = Category.GetCheckParameter("Facility_Method_Records").ValueAsDataView();

					string facilityMethodRecordsOriginalFilter = FacilityMethodRecords.RowFilter;
					{
						FacilityMethodRecords.RowFilter = AddToDataViewFilter(facilityMethodRecordsOriginalFilter, "mon_loc_ID = '" + MonLocId +
							"' and (Method_cd like 'CEM%' or Method_cd like 'AD%' or Method_cd in ('PEM', 'AE', 'FSA', 'AMS'))");
						FacilityMethodRecords.RowFilter = AddEvaluationDateHourRangeToDataViewFilter(FacilityMethodRecords.RowFilter,
							EvaluationBeginDate, EvaluationEndDate, EvaluationBeginHour, EvaluationEndHour, false, true);
						if (FacilityMethodRecords.Count > 0)
						{
							string ParamList = ColumnToDatalist(FacilityMethodRecords, "parameter_cd", false);
							Category.SetCheckParameter("Invalid_Parameters_for_LME_Method", ParamList, eParameterDataType.String);
							Category.CheckCatalogResult = "A";
						}
						else if (Category.GetCheckParameter("Location_Type").ValueAsString() == "CP")
						{
							DataView unitStackConfigurationRecords = Category.GetCheckParameter("Unit_Stack_Configuration_Records").ValueAsDataView();

							string unitStackConfigurationRecordsOriginalFilter = unitStackConfigurationRecords.RowFilter;
							{
								unitStackConfigurationRecords.RowFilter = AddToDataViewFilter(unitStackConfigurationRecordsOriginalFilter, "STACK_PIPE_MON_LOC_ID = '" + MonLocId + "'");
								unitStackConfigurationRecords.RowFilter = AddEvaluationDateRangeToDataViewFilter(unitStackConfigurationRecords.RowFilter, EvaluationBeginDate, EvaluationEndDate, false, true, false);

								string invalidParametersForLmeMethod = "";

								foreach (DataRowView unitStackConfigurationRow in unitStackConfigurationRecords)
								{
									string facilityMethodRecordsFilter = facilityMethodRecordsOriginalFilter;
									{
										facilityMethodRecordsFilter = AddToDataViewFilter(facilityMethodRecordsFilter,
																						  "MON_LOC_ID = '" + unitStackConfigurationRow["MON_LOC_ID"].AsString() + "' " +
																						  "and (METHOD_CD like 'CEM%' or " +
																						  "     METHOD_CD like 'AD%' or " +
																						  "     METHOD_CD in ('PEM','AE','FSA','AMS'))");

										// Checking for later of USC begin date with hour 23 and method evaluation begin date/hour
										// Only requires checking for USC begin date is later than or equal to method evaluation begin date.
										// If both are for the same date the hour assinged will be the hour 23.
										{
											if (unitStackConfigurationRow["BEGIN_DATE"].AsDateTime(DateTime.MinValue) >= EvaluationBeginDate)
												facilityMethodRecordsFilter = AddEvaluationDateHourRangeToDataViewFilter(facilityMethodRecordsFilter,
																														 unitStackConfigurationRow["BEGIN_DATE"].AsDateTime(DateTime.MinValue),
																														 EvaluationEndDate,
																														 23,
																														 EvaluationEndHour,
																														 false,
																														 true);
											else
												facilityMethodRecordsFilter = AddEvaluationDateHourRangeToDataViewFilter(facilityMethodRecordsFilter,
																														 EvaluationBeginDate,
																														 EvaluationEndDate,
																														 EvaluationBeginHour,
																														 EvaluationEndHour,
																														 false,
																														 true);
										}
									}
									FacilityMethodRecords.RowFilter = facilityMethodRecordsFilter;

									if (FacilityMethodRecords.Count > 0)
									{
										foreach (DataRowView facilityMethodRow in FacilityMethodRecords)
											invalidParametersForLmeMethod = invalidParametersForLmeMethod.ListAdd(facilityMethodRow["PARAMETER_CD"].AsString());
									}
								}

								Category.SetCheckParameter("Invalid_Parameters_for_LME_Method", invalidParametersForLmeMethod, eParameterDataType.String);

								if (invalidParametersForLmeMethod != "")
									Category.CheckCatalogResult = "A";
							}
							unitStackConfigurationRecords.RowFilter = unitStackConfigurationRecordsOriginalFilter;
						}
					}
					FacilityMethodRecords.RowFilter = facilityMethodRecordsOriginalFilter;
				}
				else
					Log = false;

			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "METHOD23");
			}

			return ReturnVal;
		}

		// Required Methods Reported for NOX/NOXR Method
		public string METHOD24(cCategory Category, ref bool Log)
		{
			string ReturnVal = "";

			try
			{
				string sMissingMethods = "";

				DataRowView CurrentMethod = (DataRowView)Category.GetCheckParameter("Current_Method").ParameterValue;
				bool bDatesConsistant = (bool)Category.GetCheckParameter("Method_Dates_And_Hours_Consistent").ParameterValue;

				string MethodCd = cDBConvert.ToString(CurrentMethod["Method_Cd"]);
				string ParameterCd = cDBConvert.ToString(CurrentMethod["Parameter_Cd"]);
				if (ParameterCd == "NOX" && MethodCd == "NOXR" & bDatesConsistant)
				{
					DataView dvMethodRecords = (DataView)Category.GetCheckParameter("Method_Records").ParameterValue;
					string sMethodFilter = dvMethodRecords.RowFilter;

					int EvalEndHour = (int)Category.GetCheckParameter("Method_Evaluation_End_Hour").ParameterValue;
					int EvalBeginHour = (int)Category.GetCheckParameter("Method_Evaluation_Begin_Hour").ParameterValue;
					DateTime EvalEndDate = (DateTime)Category.GetCheckParameter("Method_Evaluation_End_Date").ParameterValue;
					DateTime EvalBeginDate = (DateTime)Category.GetCheckParameter("Method_Evaluation_Begin_Date").ParameterValue;

					int nCount = 0;
					string sFilter = "";

					sFilter = AddToDataViewFilter(sMethodFilter, "parameter_cd = 'NOXR'");
					sFilter = AddEvaluationDateHourRangeToDataViewFilter(sFilter, EvalBeginDate, EvalEndDate, EvalBeginHour, EvalEndHour, false, true);
					dvMethodRecords.RowFilter = sFilter;

					if (dvMethodRecords.Count == 0)
					{
						sMissingMethods = "NOXR";
						Category.CheckCatalogResult = "A";
					}
					else
					{
						if (!CheckForHourRangeCovered(Category, dvMethodRecords, EvalBeginDate, EvalBeginHour, EvalEndDate, EvalEndHour, ref nCount))
						{
							sMissingMethods = "NOXR";
							Category.CheckCatalogResult = "B";
						}
					}

					sFilter = AddToDataViewFilter(sMethodFilter, "parameter_cd = 'HI'");
					sFilter = AddEvaluationDateHourRangeToDataViewFilter(sFilter, EvalBeginDate, EvalEndDate, EvalBeginHour, EvalEndHour, false, true);
					dvMethodRecords.RowFilter = sFilter;

					if (dvMethodRecords.Count == 0)
					{
						sMissingMethods = sMissingMethods.ListAdd("HI");
						Category.CheckCatalogResult = "A";
					}
					else
					{
						if (!CheckForHourRangeCovered(Category, dvMethodRecords, EvalBeginDate, EvalBeginHour, EvalEndDate, EvalEndHour, ref nCount))
						{
							sMissingMethods = sMissingMethods.ListAdd("HI");
							Category.CheckCatalogResult = "B";
						}
					}

					// reset this sucker
					dvMethodRecords.RowFilter = sMethodFilter;
				}

				if (sMissingMethods != "")
					Category.SetCheckParameter("Missing_Methods_for_NOX_NOXR_Method", sMissingMethods, eParameterDataType.String);
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "METHOD24");
			}

			return ReturnVal;
		}

		// Required NFS System Reported for Method
		public string METHOD25(cCategory Category, ref bool Log)
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentMethod = (DataRowView)Category.GetCheckParameter("Current_Method").ParameterValue;

				bool bMethodCdValid = (bool)Category.GetCheckParameter("Method_Method_Code_Valid").ParameterValue;
				bool bParameterCdValid = (bool)Category.GetCheckParameter("Method_Parameter_Valid").ParameterValue;
				bool bDatesConsistant = (bool)Category.GetCheckParameter("Method_Dates_And_Hours_Consistent").ParameterValue;

				if (bParameterCdValid && bMethodCdValid && bDatesConsistant)
				{
					string MethodCd = cDBConvert.ToString(CurrentMethod["Method_Cd"]);
					string ParameterCd = cDBConvert.ToString(CurrentMethod["Parameter_Cd"]);
					DateTime EvalEndDate = (DateTime)Category.GetCheckParameter("Method_Evaluation_End_Date").ParameterValue;
					DateTime EvalBeginDate = (DateTime)Category.GetCheckParameter("Method_Evaluation_Begin_Date").ParameterValue;
					int EvalEndHour = (int)Category.GetCheckParameter("Method_Evaluation_End_Hour").ParameterValue;
					int EvalBeginHour = (int)Category.GetCheckParameter("Method_Evaluation_Begin_Hour").ParameterValue;
					string sFilter = "";
					int nCount = 0;

					if (ParameterCd == "HI" && MethodCd == "CEM")
					{
						DataView dvMonSysRecords = (DataView)Category.GetCheckParameter("Monitor_System_Records").ParameterValue;
						string sMonSysFilter = dvMonSysRecords.RowFilter;

						sFilter = AddToDataViewFilter(sMonSysFilter, "sys_type_cd in ('CO2','O2') and sys_designation_cd = 'P'");
						sFilter = AddEvaluationDateHourRangeToDataViewFilter(sFilter, EvalBeginDate, EvalEndDate, EvalBeginHour, EvalEndHour, false, true, true);
						dvMonSysRecords.RowFilter = sFilter;

						DateTime MPDate = Category.GetCheckParameter("ECMPS_MP_Begin_Date").ValueAsDateTime(DateTypes.START);

						if (dvMonSysRecords.Count == 0 && EvalBeginDate >= MPDate)
						{
							Category.SetCheckParameter("Required_System_For_Method", "CO2 or O2", eParameterDataType.String);
							Category.CheckCatalogResult = "A";
						}
						else if ((dvMonSysRecords.Count == 0 && EvalBeginDate < MPDate) ||
								 !CheckForHourRangeCovered(Category, dvMonSysRecords, EvalBeginDate, EvalBeginHour, EvalEndDate, EvalEndHour, ref nCount))
						{
							DataView dvLocationProgramRecords = (DataView)Category.GetCheckParameter("Location_Program_Records").ParameterValue;
							string sLocProgFilter = dvLocationProgramRecords.RowFilter;
							DateTime dtUnitMonitorCertBeginDate = DateTime.MinValue;

							sFilter = AddToDataViewFilter(sLocProgFilter, string.Format("prg_cd = 'ARP' and class <> 'NA' and UNIT_MONITOR_CERT_BEGIN_DATE < '{0}'", MPDate.ToShortDateString()));
							dvLocationProgramRecords.RowFilter = sFilter;

							if (dvLocationProgramRecords.Count > 0)
							{
								if (dvMonSysRecords.Count == 0)
								{
									Category.SetCheckParameter("Required_System_For_Method", "CO2 or O2", eParameterDataType.String);
									Category.CheckCatalogResult = "A";
								}
								else
								{
									dtUnitMonitorCertBeginDate = cDBConvert.ToDate(dvLocationProgramRecords[0]["UNIT_MONITOR_CERT_BEGIN_DATE"], DateTypes.START);

									if (!CheckForHourRangeCovered(Category, dvMonSysRecords, dtUnitMonitorCertBeginDate, 0, EvalEndDate, EvalEndHour, ref nCount))
									{
										Category.SetCheckParameter("Required_System_For_Method", "CO2 or O2", eParameterDataType.String);
										Category.CheckCatalogResult = "B";
									}
								}
							}
							else if (EvalEndDate >= MPDate)
							{
								if (dvMonSysRecords.Count == 0)
								{
									Category.SetCheckParameter("Required_System_For_Method", "CO2 or O2", eParameterDataType.String);
									Category.CheckCatalogResult = "A";
								}
								else if (!CheckForHourRangeCovered(Category, dvMonSysRecords, MPDate, 0, EvalEndDate, EvalEndHour, ref nCount))
								{
									Category.SetCheckParameter("Required_System_For_Method", "CO2 or O2", eParameterDataType.String);
									Category.CheckCatalogResult = "B";
								}
							}

							dvLocationProgramRecords.RowFilter = sLocProgFilter;
						}

						dvMonSysRecords.RowFilter = sMonSysFilter;
					}

					if (Category.CheckCatalogResult != null)
						return ReturnVal;

					if (ParameterCd != "HI" && (MethodCd.Contains("CEM") || MethodCd.Contains("ST") || MethodCd.InList("PEM,MTB,MMS,MWD")))
					{
						DataView dvMonSysRecords = (DataView)Category.GetCheckParameter("Monitor_System_Records").ParameterValue;
						string sMonSysFilter = dvMonSysRecords.RowFilter;

						DataView dvPMSysTypeCrossCheck = (DataView)Category.GetCheckParameter("Method_to_System_Type_Cross_Check_Table").ParameterValue;
						string sPMSysTypeFilter = dvPMSysTypeCrossCheck.RowFilter;

						sFilter = AddToDataViewFilter(sPMSysTypeFilter, string.Format("MethodParameterCode = '{0}' and MethodCode = '{1}' and SystemTypeCode is not null", ParameterCd, MethodCd));
						dvPMSysTypeCrossCheck.RowFilter = sFilter;
						if (dvPMSysTypeCrossCheck.Count == 1)
						{   // found
							string sSysTypeCd = cDBConvert.ToString(dvPMSysTypeCrossCheck[0]["SystemTypeCode"]);

							sFilter = AddToDataViewFilter(sMonSysFilter, string.Format("sys_type_cd = '{0}' and sys_designation_cd in ('P','PB')", sSysTypeCd));
							sFilter = AddEvaluationDateHourRangeToDataViewFilter(sFilter, EvalBeginDate, EvalEndDate, EvalBeginHour, EvalEndHour, false, true, true);
							dvMonSysRecords.RowFilter = sFilter;

							if (dvMonSysRecords.Count == 0)
							{   // not found
								Category.SetCheckParameter("Required_System_For_Method", sSysTypeCd, eParameterDataType.String);
								Category.CheckCatalogResult = "A";
							}
							else if (!CheckForHourRangeCovered(Category, dvMonSysRecords, EvalBeginDate, EvalBeginHour, EvalEndDate, EvalEndHour, ref nCount))
							{
								Category.SetCheckParameter("Required_System_For_Method", sSysTypeCd, eParameterDataType.String);
								Category.CheckCatalogResult = "B";
							}


						}
						else if (dvPMSysTypeCrossCheck.Count > 1)
						{
							Category.SetCheckParameter("Required_System_For_Method", null, eParameterDataType.String);
							string RequiredSystemForMethod = "";
							string IncompleteSystemForMethod = "";
							string primary = "";

							foreach (DataRowView xCheckRecord in dvPMSysTypeCrossCheck)
							{
								string sSysTypeCd = cDBConvert.ToString(xCheckRecord["SystemTypeCode"]);
								sFilter = AddToDataViewFilter(sMonSysFilter, string.Format("sys_type_cd = '{0}'", sSysTypeCd));
								sFilter = AddEvaluationDateHourRangeToDataViewFilter(sFilter, EvalBeginDate, EvalEndDate, EvalBeginHour, EvalEndHour, false, true, true);
								dvMonSysRecords.RowFilter = sFilter;
								int nMonSysCount = dvMonSysRecords.Count;
								if (nMonSysCount == 0)
									RequiredSystemForMethod = RequiredSystemForMethod.ListAdd(sSysTypeCd);
								else
								{
									if (!CheckForHourRangeCovered(Category, dvMonSysRecords, EvalBeginDate, EvalBeginHour, EvalEndDate, EvalEndHour, ref nCount))
										IncompleteSystemForMethod = IncompleteSystemForMethod.ListAdd(sSysTypeCd);
									else
									{
										dvMonSysRecords.RowFilter = AddToDataViewFilter(dvMonSysRecords.RowFilter, "sys_designation_cd in ('P','PB')");
										if (nMonSysCount == dvMonSysRecords.Count)
										{
											if (primary == "FOUND")
												primary = "BOTH";
											else
												primary = "FOUND";
										}
									}
								}
								dvMonSysRecords.RowFilter = sMonSysFilter;
							}
                            
                            MpParameters.RequiredSystemForMethod = RequiredSystemForMethod;
                            MpParameters.IncompleteSystemForMethod = IncompleteSystemForMethod;

                            if (RequiredSystemForMethod != "" && IncompleteSystemForMethod == "")
								Category.CheckCatalogResult = "C";
							else if (RequiredSystemForMethod == "" && IncompleteSystemForMethod != "")
								Category.CheckCatalogResult = "D";
							else if (RequiredSystemForMethod != "" && IncompleteSystemForMethod != "")
								Category.CheckCatalogResult = "E";
							else if (primary == "BOTH")
								Category.CheckCatalogResult = "F";
							else if (primary == "")
							{
								string sSysTypeCodes = ColumnToDatalist(dvPMSysTypeCrossCheck, "SystemTypeCode");
								sSysTypeCodes = "'" + sSysTypeCodes.Replace(",", "','") + "'";

								sFilter = AddToDataViewFilter(sMonSysFilter, string.Format("sys_type_cd in ({0}) and sys_designation_cd in ('P','B')", sSysTypeCodes));
								sFilter = AddEvaluationDateHourRangeToDataViewFilter(sFilter, EvalBeginDate, EvalEndDate, EvalBeginHour, EvalEndHour, false, true, true);
								dvMonSysRecords.RowFilter = sFilter;
								if (!CheckForHourRangeCovered(Category, dvMonSysRecords, EvalBeginDate, EvalBeginHour, EvalEndDate, EvalEndHour, ref nCount))
									Category.CheckCatalogResult = "G";
							}
						}
						dvMonSysRecords.RowFilter = sMonSysFilter;

						// be sure to reset this sucker
						dvPMSysTypeCrossCheck.RowFilter = sPMSysTypeFilter;
					}
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "METHOD25");
			}

			return ReturnVal;
		}

		// Required Flow System Reported for Method
		public string METHOD26(cCategory Category, ref bool Log)
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentMethod = (DataRowView)Category.GetCheckParameter("Current_Method").ParameterValue;

				bool bMethodCdValid = (bool)Category.GetCheckParameter("Method_Method_Code_Valid").ParameterValue;
				bool bParameterCdValid = (bool)Category.GetCheckParameter("Method_Parameter_Valid").ParameterValue;
				bool bDatesConsistant = (bool)Category.GetCheckParameter("Method_Dates_And_Hours_Consistent").ParameterValue;

				if (bParameterCdValid && bMethodCdValid && bDatesConsistant)
				{
					string MethodCd = cDBConvert.ToString(CurrentMethod["Method_Cd"]);
					string ParameterCd = cDBConvert.ToString(CurrentMethod["Parameter_Cd"]);
					DateTime EvalEndDate = (DateTime)Category.GetCheckParameter("Method_Evaluation_End_Date").ParameterValue;
					DateTime EvalBeginDate = (DateTime)Category.GetCheckParameter("Method_Evaluation_Begin_Date").ParameterValue;
					int EvalEndHour = (int)Category.GetCheckParameter("Method_Evaluation_End_Hour").ParameterValue;
					int EvalBeginHour = (int)Category.GetCheckParameter("Method_Evaluation_Begin_Hour").ParameterValue;

					if ((ParameterCd.InList("SO2,HI,CO2,NOX") && MethodCd.Contains("CEM")) || (ParameterCd.InList("HGRE,HCLRE,HFRE,SO2RE") && (MethodCd != "CALC")))
					{
						DataView dvMonSysRecords = (DataView)Category.GetCheckParameter("Monitor_System_Records").ParameterValue;
						string sMonSysFilter = dvMonSysRecords.RowFilter;

						string sFilter = AddToDataViewFilter(sMonSysFilter, "sys_type_cd = 'FLOW' and sys_designation_cd = 'P'");
						sFilter = AddEvaluationDateHourRangeToDataViewFilter(sFilter, EvalBeginDate, EvalEndDate, EvalBeginHour, EvalEndHour, false, true, true);
						dvMonSysRecords.RowFilter = sFilter;

						int nCount = 0;

						if (dvMonSysRecords.Count == 0)
						{
							Category.CheckCatalogResult = "A";
						}
						else if (dvMonSysRecords.Count > 0 && !CheckForHourRangeCovered(Category, dvMonSysRecords, EvalBeginDate, EvalBeginHour, EvalEndDate, EvalEndHour, ref nCount))
						{
							Category.CheckCatalogResult = "B";
						}

						dvMonSysRecords.RowFilter = sMonSysFilter;
					}
				}

			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "METHOD26");
			}

			return ReturnVal;
		}

		public string METHOD27(cCategory Category, ref bool Log)
		// Required Formula Reported for Method
		{
			string ReturnVal = "";

			try
			{
				string MethodCd = MpParameters.CurrentMethod.MethodCd;
				string ParameterCd = MpParameters.CurrentMethod.ParameterCd;

				DateTime? EvalEndDateHour = MpParameters.MethodEvaluationEndDate.AddHours(MpParameters.MethodEvaluationEndHour);
				DateTime? EvalBeginDateHour = MpParameters.MethodEvaluationBeginDate.AddHours(MpParameters.MethodEvaluationBeginHour);

				//DataView dvMonSysRecords = MpReportParameters.MonitorSystemRecords.SourceView;

				if (MpParameters.MethodParameterValid.Default(false) && MpParameters.MethodMethodCodeValid.Default(false) && MpParameters.MethodDatesAndHoursConsistent.Default(false))
				{
					//Find formula in cross check table
					//Using FindRows b/c FindRow does not support the multi-dimensional array FilterCondition
					//should only find 1 row
					DataView ParameterMethodToFormulaView = cRowFilter.FindRows(MpParameters.ParameterAndMethodAndLocationToFormulaCrosscheck.SourceView,
						new cFilterCondition[][] 
                        {new cFilterCondition[]
                            {
                            // Array of defined values
                            new cFilterCondition("Parameter_cd", ParameterCd),
                            new cFilterCondition("Method_cd", MethodCd),
                            new cFilterCondition("Location_type_list", MpParameters.LocationType, eFilterConditionStringCompare.ListHas)
                            },
                        new cFilterCondition[]
                         {
                            // Second array to catch the null condition for location type
                            new cFilterCondition("Parameter_cd", ParameterCd),
                            new cFilterCondition("Method_cd", MethodCd),
                            new cFilterCondition("Location_type_list", null)
                          }});
					if (ParameterMethodToFormulaView.Count > 0)
					{
						DataRowView drParameterMethodToFormula = ParameterMethodToFormulaView[0];

						if (drParameterMethodToFormula["System_type_list"].AsString() == null)
						{ //use Monitor Formula
							DataView MonitorFormulaView = cRowFilter.FindRows(MpParameters.FormulaRecords.SourceView,
							  new cFilterCondition[]
                                    {
                                    new cFilterCondition("Parameter_cd", ParameterCd),
                                    new cFilterCondition("Equation_cd", drParameterMethodToFormula["Formula_list"].AsString(), eFilterConditionStringCompare.InList),  //Formula may have comma delimited list
                                    new cFilterCondition("Begin_datehour", EvalEndDateHour, eFilterDataType.DateBegan, eFilterConditionRelativeCompare.LessThanOrEqual),
                                    new cFilterCondition("End_datehour", EvalBeginDateHour, eFilterDataType.DateEnded, eFilterConditionRelativeCompare.GreaterThanOrEqual)
                                    });
							if (MonitorFormulaView.Count <= 0)
							{
								//set missing
								MpParameters.MissingFormulaForMethod = ParameterCd + " " + drParameterMethodToFormula["Formula_list"].AsString();
								Category.CheckCatalogResult = drParameterMethodToFormula["Not_found_result"].AsString();
							}
							else
							{
								if (!CheckForHourRangeCovered(Category, MonitorFormulaView, MpParameters.MethodEvaluationBeginDate.Default(DateTime.MinValue), MpParameters.MethodEvaluationBeginHour.Default(0), MpParameters.MethodEvaluationEndDate.Default(DateTime.MaxValue), MpParameters.MethodEvaluationEndHour.Default(23)))
								{
									//set missing
									MpParameters.MissingFormulaForMethod = ParameterCd + " " + drParameterMethodToFormula["Formula_list"].AsString();
									Category.CheckCatalogResult = "B";
								}
							}
						}

						else
						{
							DateTime EcmpsMpBeginDate = MpParameters.EcmpsMpBeginDate.Default(DateTime.MinValue);

							DateTime? FormulaRangeBeginDateHour = null;

							if (drParameterMethodToFormula["Ecmps_only"].AsString() == "Yes")
							{
								//ECMPS Only
								FormulaRangeBeginDateHour = ((EcmpsMpBeginDate > EvalBeginDateHour)
									  ? EcmpsMpBeginDate
									  : EvalBeginDateHour);
							}
							else
							{
								FormulaRangeBeginDateHour = EvalBeginDateHour;
							}

							DataView MonitorSystemView = cRowFilter.FindRows(MpParameters.MonitorSystemRecords.SourceView,
								new cFilterCondition[]
                                    {
                                         new cFilterCondition("Sys_type_cd",  drParameterMethodToFormula["System_type_list"].AsString(), eFilterConditionStringCompare.InList),
                                    new cFilterCondition("BEGIN_DATEHOUR", EvalEndDateHour, eFilterDataType.DateBegan, eFilterConditionRelativeCompare.LessThanOrEqual),
                                    new cFilterCondition("END_DATEHOUR", FormulaRangeBeginDateHour, eFilterDataType.DateEnded, eFilterConditionRelativeCompare.GreaterThanOrEqual)
                                    });
							if (MonitorSystemView.Count > 1)
							{
								string errorMessage = null;

								//// Returns a list of distinct date/hour ranges between which the
								//// active rows in the view change.
								cActiveRowHourRanges activeRanges
								  = cActiveRowHourRanges.Create(MonitorSystemView,
																FormulaRangeBeginDateHour,
																MpParameters.MethodEvaluationEndDate.Default(DateTime.MaxValue),
																ref errorMessage);

								bool overlapExists = false;

								foreach (cActiveRowHourRange activeRange in activeRanges)
								{
									if ((activeRange.Rows.Count > 1) &&
										(activeRange.Rows.DistinctValues("FUEL_CD").Count > 1))
									{
										overlapExists = true;
										activeRange.Tag = true;
									}
									else
										activeRange.Tag = false;
								}

								if (overlapExists)
								{
									// Locate Monitor Formula records
									DataView SystemMonitorFormulaView;
									{
										SystemMonitorFormulaView
										  = cRowFilter.FindActiveRows(MpParameters.FormulaRecords.SourceView,
																	  FormulaRangeBeginDateHour.Default(DateTime.MinValue),
																	  EvalEndDateHour.Default(DateTime.MaxValue),
																	  new cFilterCondition[] 
                                                                            {new cFilterCondition("PARAMETER_CD", ParameterCd),
                                                                             new cFilterCondition("EQUATION_CD", drParameterMethodToFormula["Formula_list"].AsString(), eFilterConditionStringCompare.InList)
                                                                   });
										if (SystemMonitorFormulaView.Count == 0)
										{
											//missing
											MpParameters.MissingFormulaForMethod = ParameterCd + " " + drParameterMethodToFormula["Formula_list"].AsString();
											Category.CheckCatalogResult = drParameterMethodToFormula["Not_found_result"].AsString();

										}
										else
										{//check overlaps
											foreach (cActiveRowHourRange activeRange in activeRanges)
											{
												if ((bool)activeRange.Tag && //Indicates that a fuel overlap occurred for the range
													!CheckForHourRangeCovered(Category, SystemMonitorFormulaView,
																			  activeRange.Began.Date,
																			  activeRange.Began.Hour.AsInteger(),
																			  activeRange.Ended.Date,
																			  activeRange.Ended.Hour.AsInteger()
																			  ))
												{
													//missing
													MpParameters.MissingFormulaForMethod = ParameterCd + " " + drParameterMethodToFormula["Formula_list"].AsString();
													Category.CheckCatalogResult = "B";
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "METHOD27");
			}

			return ReturnVal;
		}

		public string METHOD28(cCategory Category, ref bool Log)
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentMethod = (DataRowView)Category.GetCheckParameter("Current_Method").ParameterValue;

				string MethodCd = cDBConvert.ToString(CurrentMethod["Method_Cd"]);
				string ParameterCd = cDBConvert.ToString(CurrentMethod["Parameter_Cd"]);
				bool bMethodCdValid = (bool)Category.GetCheckParameter("Method_Method_Code_Valid").ParameterValue;
				bool bParameterCdValid = (bool)Category.GetCheckParameter("Method_Parameter_Valid").ParameterValue;
				bool bDatesConsistant = (bool)Category.GetCheckParameter("Method_Dates_And_Hours_Consistent").ParameterValue;
				string sLocationType = cDBConvert.ToString(Category.GetCheckParameter("Location_Type").ParameterValue);

				DateTime EvalEndDate = (DateTime)Category.GetCheckParameter("Method_Evaluation_End_Date").ParameterValue;
				DateTime EvalBeginDate = (DateTime)Category.GetCheckParameter("Method_Evaluation_Begin_Date").ParameterValue;
				int EvalEndHour = (int)Category.GetCheckParameter("Method_Evaluation_End_Hour").ParameterValue;
				int EvalBeginHour = (int)Category.GetCheckParameter("Method_Evaluation_Begin_Hour").ParameterValue;

				DateTime IntersectionEndDate = EvalEndDate;
				DateTime IntersectionBeginDate = EvalBeginDate;
				int IntersectionEndHour = 0;
				int IntersectionBeginHour = 23;

				DataView dvDefaultRecords = (DataView)Category.GetCheckParameter("Default_Records").ParameterValue;
				string sDefaultFilter = dvDefaultRecords.RowFilter;

				DataView dvLocFuelRecords = (DataView)Category.GetCheckParameter("Location_Fuel_Records").ParameterValue;
				string sLocFuelFilter = dvLocFuelRecords.RowFilter;

				string sFilter = "";

				string sMissing = null, sIncomplete = null;
				string sMissingFuel = null, sIncompleteFuel = null;

				if (bParameterCdValid && bMethodCdValid && bDatesConsistant)
				{
					Category.SetCheckParameter("Missing_Default_for_Method", null, eParameterDataType.String);
					Category.SetCheckParameter("Incomplete_Default_for_Method", null, eParameterDataType.String);

					if (ParameterCd == "H2O" && MethodCd == "MDF")
					{
						sFilter = AddToDataViewFilter(sDefaultFilter, "PARAMETER_CD='H2O' and DEFAULT_PURPOSE_CD='PM'");
						sFilter = AddEvaluationDateHourRangeToDataViewFilter(sFilter, EvalBeginDate, EvalEndDate, EvalBeginHour, EvalEndHour, false, true);
						dvDefaultRecords.RowFilter = sFilter;
						if (dvDefaultRecords.Count == 0)
							sMissing = sMissing.ListAdd("H2O PM");
						else if (!CheckForHourRangeCovered(Category, dvDefaultRecords, EvalBeginDate, EvalBeginHour, EvalEndDate, EvalEndHour))
						{
							sIncomplete = sIncomplete.ListAdd("H2O PM");
						}
					}

					if (ParameterCd == "SO2" && MethodCd.Contains("F23"))
					{
						sFilter = AddToDataViewFilter(sDefaultFilter, "PARAMETER_CD='SO2R' and DEFAULT_PURPOSE_CD='F23'");
						sFilter = AddEvaluationDateHourRangeToDataViewFilter(sFilter, EvalBeginDate, EvalEndDate, EvalBeginHour, EvalEndHour, false, true);
						dvDefaultRecords.RowFilter = sFilter;
						if (dvDefaultRecords.Count == 0)
							sMissing = sMissing.ListAdd("SO2R F23");
						else if (!CheckForHourRangeCovered(Category, dvDefaultRecords, EvalBeginDate, EvalBeginHour, EvalEndDate, EvalEndHour))
						{
							sIncomplete = sIncomplete.ListAdd("SO2R F23");
						}
					}

					if (ParameterCd == "NOXR" && MethodCd.StartsWith("CEM"))
					{
						sFilter = AddToDataViewFilter(sDefaultFilter, "PARAMETER_CD='NORX' and DEFAULT_PURPOSE_CD='MD' and FUEL_CD='NFS'");
						sFilter = AddEvaluationDateHourRangeToDataViewFilter(sFilter, EvalBeginDate, EvalEndDate, EvalBeginHour, EvalEndHour, false, true);
						dvDefaultRecords.RowFilter = sFilter;
						if (dvDefaultRecords.Count == 0)
							sMissing = sMissing.ListAdd("NORX MD");
						else if (!CheckForHourRangeCovered(Category, dvDefaultRecords, EvalBeginDate, EvalBeginHour, EvalEndDate, EvalEndHour))
						{
							sIncomplete = sIncomplete.ListAdd("NORX MD");
						}
					}

					if (MethodCd == "SO2R")
					{
						sFilter = AddToDataViewFilter(sDefaultFilter, "PARAMETER_CD='SORX' and DEFAULT_PURPOSE_CD='MD' and FUEL_CD='NFS'");
						sFilter = AddEvaluationDateHourRangeToDataViewFilter(sFilter, EvalBeginDate, EvalEndDate, EvalBeginHour, EvalEndHour, false, true);
						dvDefaultRecords.RowFilter = sFilter;
						if (dvDefaultRecords.Count == 0)
							sMissing = sMissing.ListAdd("SORX MD");
						else if (!CheckForHourRangeCovered(Category, dvDefaultRecords, EvalBeginDate, EvalBeginHour, EvalEndDate, EvalEndHour))
						{
							sIncomplete = sIncomplete.ListAdd("SORX MD");
						}
					}

					if (MethodCd == "PEM")
					{
						sFilter = AddToDataViewFilter(sDefaultFilter, "PARAMETER_CD='NOCX' and DEFAULT_PURPOSE_CD='MD' and FUEL_CD='NFS'");
						sFilter = AddEvaluationDateHourRangeToDataViewFilter(sFilter, EvalBeginDate, EvalEndDate, EvalBeginHour, EvalEndHour, false, true);
						dvDefaultRecords.RowFilter = sFilter;
						if (dvDefaultRecords.Count == 0)
							sMissing = sMissing.ListAdd("NOCX MD");
						else if (!CheckForHourRangeCovered(Category, dvDefaultRecords, EvalBeginDate, EvalBeginHour, EvalEndDate, EvalEndHour))
						{
							sIncomplete = sIncomplete.ListAdd("NOCX MD");
						}

						sFilter = AddToDataViewFilter(sDefaultFilter, "PARAMETER_CD='NORX' and DEFAULT_PURPOSE_CD='MD' and FUEL_CD='NFS'");
						sFilter = AddEvaluationDateHourRangeToDataViewFilter(sFilter, EvalBeginDate, EvalEndDate, EvalBeginHour, EvalEndHour, false, true);
						dvDefaultRecords.RowFilter = sFilter;
						if (dvDefaultRecords.Count == 0)
							sMissing = sMissing.ListAdd("NORX MD");
						else if (!CheckForHourRangeCovered(Category, dvDefaultRecords, EvalBeginDate, EvalBeginHour, EvalEndDate, EvalEndHour))
						{
							sIncomplete = sIncomplete.ListAdd("NORX MD");
						}
					}
					if (ParameterCd == "HIT" && MethodCd == "MHHI")
					{
						sFilter = AddToDataViewFilter(sDefaultFilter, string.Format("PARAMETER_CD='MHHI' and DEFAULT_PURPOSE_CD='LM'"));
						sFilter = AddEvaluationDateHourRangeToDataViewFilter(sFilter, EvalBeginDate, EvalEndDate, EvalBeginHour, EvalEndHour, false, true);
						dvDefaultRecords.RowFilter = sFilter;
						if (dvDefaultRecords.Count == 0)
						{
							sMissing = sMissing.ListAdd("MHHI LM");
						}
						else if (!CheckForHourRangeCovered(Category, dvDefaultRecords, EvalBeginDate, EvalBeginHour, EvalEndDate, EvalEndHour))
						{
							sIncomplete = sIncomplete.ListAdd("MHHI LM");
						}
					}

					dvDefaultRecords.RowFilter = sDefaultFilter;
					dvLocFuelRecords.RowFilter = sLocFuelFilter;

					Category.SetCheckParameter("Missing_Default_for_Method", sMissing, eParameterDataType.String);
					Category.SetCheckParameter("Incomplete_Default_for_Method", sIncomplete, eParameterDataType.String);

					if (string.IsNullOrEmpty(sMissing) == false && string.IsNullOrEmpty(sIncomplete) == true)
					{
						Category.CheckCatalogResult = "A";
						return ReturnVal;
					}

					if (string.IsNullOrEmpty(sMissing) == true && string.IsNullOrEmpty(sIncomplete) == false)
					{
						Category.CheckCatalogResult = "B";
						return ReturnVal;
					}

					if (string.IsNullOrEmpty(sMissing) == false && string.IsNullOrEmpty(sIncomplete) == false)
					{
						Category.CheckCatalogResult = "C";
						return ReturnVal;
					}

					if (MethodCd == "AE")
					{
						sFilter = AddToDataViewFilter(sLocFuelFilter, "INDICATOR_CD='E'");
						sFilter = AddEvaluationDateRangeToDataViewFilter(sFilter, EvalBeginDate, EvalEndDate, false, true, false);
						dvLocFuelRecords.RowFilter = sFilter;

						foreach (DataRowView loc in dvLocFuelRecords)
						{
							// get the LATER of the begin dates
							if (DateTime.Compare(EvalBeginDate, cDBConvert.ToDate(loc["BEGIN_DATE"], DateTypes.START)) > 0)
							{
								IntersectionBeginDate = EvalBeginDate;
								IntersectionBeginHour = EvalBeginHour;
							}
							else
							{
								IntersectionBeginDate = cDBConvert.ToDate(loc["BEGIN_DATE"], DateTypes.START);
								IntersectionBeginHour = 23;
							}

							// get the EARLIER of the end dates
							if (DateTime.Compare(EvalEndDate, cDBConvert.ToDate(loc["END_DATE"], DateTypes.END)) < 0)
							{
								IntersectionEndDate = EvalEndDate;
								IntersectionEndHour = EvalEndHour;
							}
							else
							{
								IntersectionEndDate = cDBConvert.ToDate(loc["END_DATE"], DateTypes.END);
								IntersectionEndHour = 0;
							}

							sFilter = AddToDataViewFilter(sDefaultFilter, string.Format("PARAMETER_CD='NOCX' and DEFAULT_PURPOSE_CD='MD' and FUEL_CD='{0}'", loc["FUEL_CD"]));
							sFilter = AddEvaluationDateHourRangeToDataViewFilter(sFilter, IntersectionBeginDate, IntersectionEndDate, IntersectionBeginHour, IntersectionEndHour, false, true);
							dvDefaultRecords.RowFilter = sFilter;
							if (dvDefaultRecords.Count == 0)
							{
								sMissing = sMissing.ListAdd("NOCX MD");
								Category.SetCheckParameter("Missing_Default_for_Method", sMissing, eParameterDataType.String);
							}
							else if (!CheckForHourRangeCovered(Category, dvDefaultRecords, IntersectionBeginDate, IntersectionBeginHour, IntersectionEndDate, IntersectionEndHour))
							{
								sIncomplete = sIncomplete.ListAdd("NOCX MD");
								Category.SetCheckParameter("Incomplete_Default_for_Method", sIncomplete, eParameterDataType.String);
							}

							sFilter = AddToDataViewFilter(sDefaultFilter, string.Format("PARAMETER_CD='NORX' and DEFAULT_PURPOSE_CD='MD' and FUEL_CD='{0}'", loc["FUEL_CD"]));
							sFilter = AddEvaluationDateHourRangeToDataViewFilter(sFilter, IntersectionBeginDate, IntersectionEndDate, IntersectionBeginHour, IntersectionEndHour, false, true);
							dvDefaultRecords.RowFilter = sFilter;
							if (dvDefaultRecords.Count == 0)
							{
								sMissing = sMissing.ListAdd("NORX MD");
								Category.SetCheckParameter("Missing_Default_for_Method", sMissing, eParameterDataType.String);
							}
							else if (!CheckForHourRangeCovered(Category, dvDefaultRecords, IntersectionBeginDate, IntersectionBeginHour, IntersectionEndDate, IntersectionEndHour))
							{
								sIncomplete = sIncomplete.ListAdd("NORX MD");
								Category.SetCheckParameter("Incomplete_Default_for_Method", sIncomplete, eParameterDataType.String);
							}

						}

						dvDefaultRecords.RowFilter = sDefaultFilter;
						dvLocFuelRecords.RowFilter = sLocFuelFilter;

						if (string.IsNullOrEmpty(sMissing) == false && string.IsNullOrEmpty(sIncomplete) == true)
						{
							Category.CheckCatalogResult = "D";
							return ReturnVal;
						}

						if (string.IsNullOrEmpty(sMissing) == true && string.IsNullOrEmpty(sIncomplete) == false)
						{
							Category.CheckCatalogResult = "E";
							return ReturnVal;
						}

						if (string.IsNullOrEmpty(sMissing) == false && string.IsNullOrEmpty(sIncomplete) == false)
						{
							Category.CheckCatalogResult = "F";
							return ReturnVal;
						}
					}//

					//if (ParameterCd == "HI" && MethodCd.StartsWith("AD"))
					//{
					//    sFilter = AddToDataViewFilter(sLocFuelFilter, "INDICATOR_CD='E'");
					//    sFilter = AddEvaluationDateRangeToDataViewFilter(sFilter, EvalBeginDate, EvalEndDate, false, true, false);
					//    dvLocFuelRecords.RowFilter = sFilter;

					//    foreach (DataRowView loc in dvLocFuelRecords)
					//    {
					//        // get the LATER of the begin dates
					//        if (DateTime.Compare(EvalBeginDate, cDBConvert.ToDate(loc["BEGIN_DATE"], DateTypes.START)) > 0)
					//        {
					//            IntersectionBeginDate = EvalBeginDate;
					//            IntersectionBeginHour = EvalBeginHour;
					//        }
					//        else
					//        {
					//            IntersectionBeginDate = cDBConvert.ToDate(loc["BEGIN_DATE"], DateTypes.START);
					//            IntersectionBeginHour = 0;
					//        }

					//        // get the EARLIER of the end dates
					//        if (DateTime.Compare(EvalEndDate, cDBConvert.ToDate(loc["END_DATE"], DateTypes.END)) < 0)
					//        {
					//            IntersectionEndDate = EvalEndDate;
					//            IntersectionEndHour = EvalEndHour;
					//        }
					//        else
					//        {
					//            IntersectionEndDate = cDBConvert.ToDate(loc["END_DATE"], DateTypes.END);
					//            IntersectionEndHour = 23;
					//        }

					//        if (Category.GetCheckParameter("Location_Type").ValueAsString() == "CP")
					//        {
					//            DataView FacDefaultRecords = Category.GetCheckParameter("Facility_Default_Records").ValueAsDataView();
					//            string sFacDefaultFilter = FacDefaultRecords.RowFilter;
					//            FacDefaultRecords.RowFilter = AddToDataViewFilter(sFacDefaultFilter,
					//                string.Format("(mon_loc_id = '{0}' or mon_loc_id = '{1}') and parameter_cd = 'MXFF' and default_purpose_cd = 'MD' and fuel_cd = '{2}'", CurrentMethod["MON_LOC_ID"],loc["MON_LOC_ID"],loc["FUEL_CD"]));
					//            if (FacDefaultRecords.Count == 0)
					//            {
					//                sMissing = sMissing.ListAdd("MXFF MD");
					//                Category.SetCheckParameter("Missing_Default_for_Method", sMissing, eParameterDataType.String);
					//            }
					//            else if (!CheckForHourRangeCovered(Category, FacDefaultRecords, IntersectionBeginDate, IntersectionBeginHour, IntersectionEndDate, IntersectionEndHour))
					//            {
					//                sIncomplete = sIncomplete.ListAdd("MXFF MD");
					//                Category.SetCheckParameter("Incomplete_Default_for_Method", sIncomplete, eParameterDataType.String);
					//            }
					//            FacDefaultRecords.RowFilter = sFacDefaultFilter;                                       

					//        }
					//        else
					//        {
					//            sFilter = AddToDataViewFilter(sDefaultFilter, string.Format("PARAMETER_CD='MXFF' and DEFAULT_PURPOSE_CD='MD' and FUEL_CD='{0}'", loc["FUEL_CD"]));
					//            sFilter = AddEvaluationDateHourRangeToDataViewFilter(sFilter, IntersectionBeginDate, IntersectionEndDate, IntersectionBeginHour, IntersectionEndHour, false, true);
					//            dvDefaultRecords.RowFilter = sFilter;

					//            if (dvDefaultRecords.Count == 0)
					//            {
					//                sMissing = sMissing.ListAdd("MXFF MD");
					//                Category.SetCheckParameter("Missing_Default_for_Method", sMissing, eParameterDataType.String);
					//            }
					//            else if (!CheckForHourRangeCovered(Category, dvDefaultRecords, IntersectionBeginDate, IntersectionBeginHour, IntersectionEndDate, IntersectionEndHour))
					//            {
					//                sIncomplete = sIncomplete.ListAdd("MXFF MD");
					//                Category.SetCheckParameter("Incomplete_Default_for_Method", sIncomplete, eParameterDataType.String);
					//            }
					//        }                 

					//    }

					//    dvDefaultRecords.RowFilter = sDefaultFilter;
					//    dvLocFuelRecords.RowFilter = sLocFuelFilter;

					//    if (string.IsNullOrEmpty(sMissing) == false && string.IsNullOrEmpty(sIncomplete) == true)
					//    {
					//        Category.CheckCatalogResult = "D";
					//        return ReturnVal;
					//    }

					//    if (string.IsNullOrEmpty(sMissing) == true && string.IsNullOrEmpty(sIncomplete) == false)
					//    {
					//        Category.CheckCatalogResult = "E";
					//        return ReturnVal;
					//    }

					//    if (string.IsNullOrEmpty(sMissing) == false && string.IsNullOrEmpty(sIncomplete) == false)
					//    {
					//        Category.CheckCatalogResult = "F";
					//        return ReturnVal;
					//    }
					//}

					if (MethodCd == "LME")
					{
						//sFilter = AddToDataViewFilter(sLocFuelFilter, "INDICATOR_CD='E'");
						sFilter = AddEvaluationDateRangeToDataViewFilter(sLocFuelFilter, EvalBeginDate, EvalEndDate, false, true, false);
						dvLocFuelRecords.RowFilter = sFilter;
						Category.SetCheckParameter("Missing_Default_Fuel_for_Method", null, eParameterDataType.String);
						Category.SetCheckParameter("Incomplete_Default_Fuel_for_Method", null, eParameterDataType.String);

						foreach (DataRowView loc in dvLocFuelRecords)
						{
							// get the LATER of the begin dates
							if (DateTime.Compare(EvalBeginDate, cDBConvert.ToDate(loc["BEGIN_DATE"], DateTypes.START)) > 0)
							{
								IntersectionBeginDate = EvalBeginDate;
								IntersectionBeginHour = EvalBeginHour;
							}
							else
							{
								IntersectionBeginDate = cDBConvert.ToDate(loc["BEGIN_DATE"], DateTypes.START);
								IntersectionBeginHour = 23;
							}

							// get the EARLIER of the end dates
							if (DateTime.Compare(EvalEndDate, cDBConvert.ToDate(loc["END_DATE"], DateTypes.END)) < 0)
							{
								IntersectionEndDate = EvalEndDate;
								IntersectionEndHour = EvalEndHour;
							}
							else
							{
								IntersectionEndDate = cDBConvert.ToDate(loc["END_DATE"], DateTypes.END);
								IntersectionEndHour = 0;
							}

							if (ParameterCd == "NOXM")
							{
								sFilter = AddToDataViewFilter(sDefaultFilter, string.Format("PARAMETER_CD='NOXR' and DEFAULT_PURPOSE_CD like 'LM%' and FUEL_CD='{0}' and OPERATING_CONDITION_CD in ('A','C','B')", loc["FUEL_CD"]));
								sFilter = AddEvaluationDateHourRangeToDataViewFilter(sFilter, EvalBeginDate, EvalEndDate, EvalBeginHour, EvalEndHour, false, true);
								dvDefaultRecords.RowFilter = sFilter;
								if (dvDefaultRecords.Count == 0)
								{
									sMissing = sMissing.ListAdd("NOXR LM");
									sMissingFuel = sMissingFuel.ListAdd((string)loc["FUEL_CD"]);
									Category.SetCheckParameter("Missing_Default_for_Method", sMissing, eParameterDataType.String);
									Category.SetCheckParameter("Missing_Default_Fuel_for_Method", sMissingFuel, eParameterDataType.String);
								}
								else if (!CheckForHourRangeCovered(Category, dvDefaultRecords, IntersectionBeginDate, IntersectionBeginHour, IntersectionEndDate, IntersectionEndHour))
								{
									sIncomplete = sIncomplete.ListAdd("NOXR LM");
									sIncompleteFuel = sMissingFuel.ListAdd((string)loc["FUEL_CD"]);
									Category.SetCheckParameter("Incomplete_Default_for_Method", sIncomplete, eParameterDataType.String);
									Category.SetCheckParameter("Incomplete_Default_Fuel_for_Method", sIncompleteFuel, eParameterDataType.String);
								}
							}

							if (ParameterCd == "CO2M")
							{
								sFilter = AddToDataViewFilter(sDefaultFilter, string.Format("PARAMETER_CD='CO2R' and DEFAULT_PURPOSE_CD like 'LM%' and FUEL_CD='{0}' and OPERATING_CONDITION_CD in ('A')", loc["FUEL_CD"]));
								sFilter = AddEvaluationDateHourRangeToDataViewFilter(sFilter, EvalBeginDate, EvalEndDate, EvalBeginHour, EvalEndHour, false, true);
								dvDefaultRecords.RowFilter = sFilter;
								if (dvDefaultRecords.Count == 0)
								{
									sMissing = sMissing.ListAdd("CO2R LM");
									sMissingFuel = sMissingFuel.ListAdd((string)loc["FUEL_CD"]);
									Category.SetCheckParameter("Missing_Default_for_Method", sMissing, eParameterDataType.String);
									Category.SetCheckParameter("Missing_Default_Fuel_for_Method", sMissingFuel, eParameterDataType.String);
								}
								else if (!CheckForHourRangeCovered(Category, dvDefaultRecords, IntersectionBeginDate, IntersectionBeginHour, IntersectionEndDate, IntersectionEndHour))
								{
									sIncomplete = sIncomplete.ListAdd("CO2R LM");
									sIncompleteFuel = sMissingFuel.ListAdd((string)loc["FUEL_CD"]);
									Category.SetCheckParameter("Incomplete_Default_for_Method", sIncomplete, eParameterDataType.String);
									Category.SetCheckParameter("Incomplete_Default_Fuel_for_Method", sIncompleteFuel, eParameterDataType.String);
								}
							}

							if (ParameterCd == "SO2M")
							{
								sFilter = AddToDataViewFilter(sDefaultFilter, string.Format("PARAMETER_CD='SO2R' and DEFAULT_PURPOSE_CD like 'LM%' and FUEL_CD='{0}' and OPERATING_CONDITION_CD in ('A')", loc["FUEL_CD"]));
								sFilter = AddEvaluationDateHourRangeToDataViewFilter(sFilter, EvalBeginDate, EvalEndDate, EvalBeginHour, EvalEndHour, false, true);
								dvDefaultRecords.RowFilter = sFilter;
								if (dvDefaultRecords.Count == 0)
								{
									sMissing = sMissing.ListAdd("SO2R LM");
									sMissingFuel = sMissingFuel.ListAdd((string)loc["FUEL_CD"]);
									Category.SetCheckParameter("Missing_Default_for_Method", sMissing, eParameterDataType.String);
									Category.SetCheckParameter("Missing_Default_Fuel_for_Method", sMissingFuel, eParameterDataType.String);
								}
								else if (!CheckForHourRangeCovered(Category, dvDefaultRecords, IntersectionBeginDate, IntersectionBeginHour, IntersectionEndDate, IntersectionEndHour))
								{
									sIncomplete = sIncomplete.ListAdd("SO2R LM");
									sIncompleteFuel = sMissingFuel.ListAdd((string)loc["FUEL_CD"]);
									Category.SetCheckParameter("Incomplete_Default_for_Method", sIncomplete, eParameterDataType.String);
									Category.SetCheckParameter("Incomplete_Default_Fuel_for_Method", sIncompleteFuel, eParameterDataType.String);
								}
							}
						}
						//}

						dvDefaultRecords.RowFilter = sDefaultFilter;
						dvLocFuelRecords.RowFilter = sLocFuelFilter;

						if (string.IsNullOrEmpty(sMissing) == false && string.IsNullOrEmpty(sIncomplete) == true)
						{
							Category.CheckCatalogResult = "G";
							return ReturnVal;
						}

						if (string.IsNullOrEmpty(sMissing) == true && string.IsNullOrEmpty(sIncomplete) == false)
						{
							Category.CheckCatalogResult = "H";
							return ReturnVal;
						}

						if (string.IsNullOrEmpty(sMissing) == false && string.IsNullOrEmpty(sIncomplete) == false)
						{
							Category.CheckCatalogResult = "I";
							return ReturnVal;
						}
					}//
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "METHOD28");
			}

			return ReturnVal;
		}

		public string METHOD29(cCategory Category, ref bool Log)
		{
			string ReturnVal = "";

			try
			{
				bool bDatesConsistant = (bool)Category.GetCheckParameter("Method_Dates_And_Hours_Consistent").ParameterValue;
				if (bDatesConsistant)
				{
					DataRowView CurrentMethod = (DataRowView)Category.GetCheckParameter("Current_Method").ParameterValue;
					string MethodCd = cDBConvert.ToString(CurrentMethod["Method_Cd"]);
					string ParameterCd = cDBConvert.ToString(CurrentMethod["Parameter_Cd"]);
					string SubstituteDataCd = cDBConvert.ToString(CurrentMethod["sub_data_cd"]);
					bool bSubstituteDataCdValid = (bool)Category.GetCheckParameter("Method_Substitute_Data_Code_Valid").ParameterValue;

					DataView dvSpanRecords = (DataView)Category.GetCheckParameter("Span_Records").ParameterValue;
					DataView dvDefaultRecords = (DataView)Category.GetCheckParameter("Default_Records").ParameterValue;
					DataView dvParaLookupTable = (DataView)Category.GetCheckParameter("Method_Parameter_To_Maximum_Default_Parameter_Lookup_Table").ParameterValue;

					DateTime EvalEndDate = (DateTime)Category.GetCheckParameter("Method_Evaluation_End_Date").ParameterValue;
					DateTime EvalBeginDate = (DateTime)Category.GetCheckParameter("Method_Evaluation_Begin_Date").ParameterValue;
					int EvalEndHour = (int)Category.GetCheckParameter("Method_Evaluation_End_Hour").ParameterValue;
					int EvalBeginHour = (int)Category.GetCheckParameter("Method_Evaluation_Begin_Hour").ParameterValue;

					bool bBypassApproachCdValid = (bool)Category.GetCheckParameter("Method_Bypass_Approach_Code_Valid").ParameterValue;
					string BypassApproachCd = cDBConvert.ToString(CurrentMethod["bypass_approach_cd"]);

					string MonLocID = cDBConvert.ToString(CurrentMethod["mon_loc_id"]);

					string sSpanFilter = dvSpanRecords.RowFilter;
					string sDefaultFilter = dvDefaultRecords.RowFilter;
					string sParamLookupFilter = dvParaLookupTable.RowFilter;
					string sDefaultSort = dvDefaultRecords.Sort;

					string sFilter = "";
					string sMissing = "";
					string sInvalidMaxDef = "";
					string sMaxDefaultValue = "";
					string sDefaultParamCd = "";

					if (bSubstituteDataCdValid && SubstituteDataCd == "FSP75C")
					{
						if (MethodCd == "SO2R")
							sFilter = AddToDataViewFilter(sParamLookupFilter, string.Format("MethodParameterCode = '{0}' AND DefaultParameterCode <> 'SO2X'", ParameterCd));
						else
							sFilter = AddToDataViewFilter(sParamLookupFilter, string.Format("MethodParameterCode = '{0}' AND DefaultParameterCode <> 'SORX'", ParameterCd));
						dvParaLookupTable.RowFilter = sFilter;

						foreach (DataRowView drLookup in dvParaLookupTable)
						{
							sDefaultParamCd = cDBConvert.ToString(drLookup["DefaultParameterCode"]);
							sFilter = AddToDataViewFilter(sDefaultFilter, string.Format("parameter_cd = '{0}'", sDefaultParamCd));
							sFilter = AddToDataViewFilter(sFilter, string.Format("mon_loc_id = '{0}'", MonLocID));
							sFilter = AddToDataViewFilter(sFilter, "default_purpose_cd = 'MD' and fuel_cd <> 'NFS'");
							sFilter = AddEvaluationDateHourRangeToDataViewFilter(sFilter, EvalBeginDate, EvalEndDate, EvalBeginHour, EvalEndHour, false, true);

							dvDefaultRecords.RowFilter = sFilter;

							if (dvDefaultRecords.Count == 0)
							{
								sMissing = (string)Category.GetCheckParameter("Missing_Maximum_Default").ParameterValue;
								if (string.IsNullOrEmpty(sMissing))
									sMissing = sDefaultParamCd;
								else
									sMissing = sMissing.ListAdd(sDefaultParamCd);
								Category.SetCheckParameter("Missing_Maximum_Default", sMissing, eParameterDataType.String);
							}
							else // found records
							{
								string DfltOldFilter = dvDefaultRecords.RowFilter;
								dvDefaultRecords.RowFilter = AddToDataViewFilter(DfltOldFilter, "fuel_cd = 'MIX'");
								if (dvDefaultRecords.Count == 0)
								{
									sMissing = (string)Category.GetCheckParameter("Missing_Maximum_Default").ParameterValue;
									if (string.IsNullOrEmpty(sMissing))
										sMissing = sDefaultParamCd;
									else
										sMissing = sMissing.ListAdd(sDefaultParamCd);
									Category.SetCheckParameter("Missing_Maximum_Default", sMissing, eParameterDataType.String);
									dvDefaultRecords.RowFilter = DfltOldFilter;
								}
								else
								{
									dvDefaultRecords.RowFilter = DfltOldFilter;
									dvDefaultRecords.Sort = "default_value DESC";
									sMaxDefaultValue = cDBConvert.ToString(dvDefaultRecords[0]["default_value"]);

									string sCompTypeCd = cDBConvert.ToString(drLookup["ComponentTypeCode"]);
									if (sCompTypeCd.InList("NOX,SO2"))
									{
										sFilter = AddToDataViewFilter(sSpanFilter, string.Format("component_type_cd = '{0}'", sCompTypeCd));
										sFilter = AddToDataViewFilter(sFilter, string.Format("mon_loc_id = '{0}'", MonLocID));
										sFilter = AddToDataViewFilter(sFilter, "span_scale_cd = 'H'");
										sFilter = AddToDataViewFilter(sFilter, string.Format("MPC_VALUE = {0}", sMaxDefaultValue));
										sFilter = AddEvaluationDateHourRangeToDataViewFilter(sFilter, EvalBeginDate, EvalEndDate, EvalBeginHour, EvalEndHour, false, true);
										dvSpanRecords.RowFilter = sFilter;

										if (dvSpanRecords.Count == 0)
										{
											sInvalidMaxDef = (string)Category.GetCheckParameter("Invalid_Maximum_Default").ParameterValue;
											if (string.IsNullOrEmpty(sInvalidMaxDef))
												sInvalidMaxDef = sCompTypeCd;
											else
												sInvalidMaxDef = sInvalidMaxDef.ListAdd(sCompTypeCd);
											Category.SetCheckParameter("Invalid_Maximum_Default", sInvalidMaxDef, eParameterDataType.String);
										}
										dvSpanRecords.RowFilter = sSpanFilter;
									}

									if (sCompTypeCd == "FLOW")
									{
										sFilter = AddToDataViewFilter(sSpanFilter, "component_type_cd = 'FLOW'");
										sFilter = AddToDataViewFilter(sFilter, string.Format("mon_loc_id = '{0}'", MonLocID));
										sFilter = AddToDataViewFilter(sFilter, string.Format("MPF_VALUE = {0}", sMaxDefaultValue));
										sFilter = AddEvaluationDateHourRangeToDataViewFilter(sFilter, EvalBeginDate, EvalEndDate, EvalBeginHour, EvalEndHour, false, true);
										dvSpanRecords.RowFilter = sFilter;

										if (dvSpanRecords.Count == 0)
										{
											sInvalidMaxDef = (string)Category.GetCheckParameter("Invalid_Maximum_Default").ParameterValue;
											if (string.IsNullOrEmpty(sInvalidMaxDef))
												sInvalidMaxDef = sCompTypeCd;
											else
												sInvalidMaxDef = sInvalidMaxDef.ListAdd(sCompTypeCd);
											Category.SetCheckParameter("Invalid_Maximum_Default", sInvalidMaxDef, eParameterDataType.String);
										}
										dvSpanRecords.RowFilter = sSpanFilter;
									}

									if (drLookup["ComponentTypeCode"] == DBNull.Value)
									{
										sFilter = AddToDataViewFilter(sDefaultFilter, string.Format("mon_loc_id = '{0}'", MonLocID));
										sFilter = AddToDataViewFilter(sFilter, string.Format("parameter_cd = '{0}'", sDefaultParamCd));
										sFilter = AddToDataViewFilter(sFilter, "default_purpose_cd = 'MD' and fuel_cd = 'NFS'");
										sFilter = AddToDataViewFilter(sFilter, string.Format("default_value = {0}", sMaxDefaultValue));
										sFilter = AddEvaluationDateHourRangeToDataViewFilter(sFilter, EvalBeginDate, EvalEndDate, EvalBeginHour, EvalEndHour, false, true);
										dvDefaultRecords.RowFilter = sFilter;

										if (dvDefaultRecords.Count == 0)
										{
											sInvalidMaxDef = (string)Category.GetCheckParameter("Invalid_Maximum_Default").ParameterValue;
											if (string.IsNullOrEmpty(sInvalidMaxDef))
												sInvalidMaxDef = sDefaultParamCd;
											else
												sInvalidMaxDef = sInvalidMaxDef.ListAdd(sDefaultParamCd);
											Category.SetCheckParameter("Invalid_Maximum_Default", sInvalidMaxDef, eParameterDataType.String);
										}

										dvDefaultRecords.RowFilter = sDefaultFilter;
									}
								}
							} // end else found records

							dvDefaultRecords.RowFilter = sDefaultFilter;
						} // end foreach record in param lookup table

					} // end if( bSubstituteDataCdValid && SubstituteDataCd == "FSP75C" )

					// reset our filters!
					dvSpanRecords.RowFilter = sSpanFilter;
					dvDefaultRecords.RowFilter = sDefaultFilter;
					dvParaLookupTable.RowFilter = sParamLookupFilter;
					dvDefaultRecords.Sort = sDefaultSort;

					if (bSubstituteDataCdValid && SubstituteDataCd == "FSP75")
					{
						if (MethodCd == "SO2R")
							sFilter = AddToDataViewFilter(sParamLookupFilter, string.Format("MethodParameterCode = '{0}' AND DefaultParameterCode <> 'SO2X'", ParameterCd));
						else
							sFilter = AddToDataViewFilter(sParamLookupFilter, string.Format("MethodParameterCode = '{0}' AND DefaultParameterCode <> 'SORX'", ParameterCd));
						dvParaLookupTable.RowFilter = sFilter;

						foreach (DataRowView drLookup in dvParaLookupTable)
						{
							sDefaultParamCd = cDBConvert.ToString(drLookup["DefaultParameterCode"]);
							sFilter = AddToDataViewFilter(sDefaultFilter, string.Format("parameter_cd = '{0}'", sDefaultParamCd));
							sFilter = AddToDataViewFilter(sFilter, string.Format("mon_loc_id = '{0}'", MonLocID));
							sFilter = AddToDataViewFilter(sFilter, "fuel_cd <> 'NFS' and fuel_cd <> 'MIX' and default_purpose_cd = 'MD'");
							sFilter = AddEvaluationDateHourRangeToDataViewFilter(sFilter, EvalBeginDate, EvalEndDate, EvalBeginHour, EvalEndHour, false, true);

							dvDefaultRecords.RowFilter = sFilter;
							dvDefaultRecords.Sort = "fuel_cd ASC";

							string sFuelCd = "";
							int nNumFuelsCds = 1;
							foreach (DataRowView drDefault in dvDefaultRecords)
							{
								if (string.IsNullOrEmpty(sFuelCd))
									sFuelCd = cDBConvert.ToString(drDefault["fuel_cd"]);
								if (sFuelCd != cDBConvert.ToString(drDefault["fuel_cd"]))
								{
									nNumFuelsCds++;
								}
							}

							if (dvDefaultRecords.Count < 2 || nNumFuelsCds == 1)
							{
								sMissing = (string)Category.GetCheckParameter("Missing_Maximum_Default").ParameterValue;
								if (string.IsNullOrEmpty(sMissing))
									sMissing = sDefaultParamCd;
								else
									sMissing = sMissing.ListAdd(sDefaultParamCd);
								Category.SetCheckParameter("Missing_Maximum_Default", sMissing, eParameterDataType.String);
							}
							else
							{
								dvDefaultRecords.Sort = "default_value DESC";
								sMaxDefaultValue = cDBConvert.ToString(dvDefaultRecords[0]["default_value"]);

								string sCompTypeCd = cDBConvert.ToString(drLookup["ComponentTypeCode"]);
								if (sCompTypeCd.InList("NOX,SO2"))
								{
									sFilter = AddToDataViewFilter(sSpanFilter, string.Format("component_type_cd = '{0}'", sCompTypeCd));
									sFilter = AddToDataViewFilter(sFilter, string.Format("mon_loc_id = '{0}'", MonLocID));
									sFilter = AddToDataViewFilter(sFilter, "span_scale_cd = 'H'");
									sFilter = AddToDataViewFilter(sFilter, string.Format("MPC_VALUE = {0}", sMaxDefaultValue));
									sFilter = AddEvaluationDateHourRangeToDataViewFilter(sFilter, EvalBeginDate, EvalEndDate, EvalBeginHour, EvalEndHour, false, true);
									dvSpanRecords.RowFilter = sFilter;

									if (dvSpanRecords.Count == 0)
									{
										sInvalidMaxDef = (string)Category.GetCheckParameter("Invalid_Maximum_Default").ParameterValue;
										if (string.IsNullOrEmpty(sInvalidMaxDef))
											sInvalidMaxDef = sCompTypeCd;
										else
											sInvalidMaxDef = sInvalidMaxDef.ListAdd(sCompTypeCd);
										Category.SetCheckParameter("Invalid_Maximum_Default", sInvalidMaxDef, eParameterDataType.String);
									}
									dvSpanRecords.RowFilter = sSpanFilter;
								}

								if (sCompTypeCd == "FLOW")
								{
									sFilter = AddToDataViewFilter(sSpanFilter, "component_type_cd = 'FLOW'");
									sFilter = AddToDataViewFilter(sFilter, string.Format("mon_loc_id = '{0}'", MonLocID));
									sFilter = AddToDataViewFilter(sFilter, string.Format("MPF_VALUE = {0}", sMaxDefaultValue));
									sFilter = AddEvaluationDateHourRangeToDataViewFilter(sFilter, EvalBeginDate, EvalEndDate, EvalBeginHour, EvalEndHour, false, true);
									dvSpanRecords.RowFilter = sFilter;

									if (dvSpanRecords.Count == 0)
									{
										sInvalidMaxDef = (string)Category.GetCheckParameter("Invalid_Maximum_Default").ParameterValue;
										if (string.IsNullOrEmpty(sInvalidMaxDef))
											sInvalidMaxDef = sCompTypeCd;
										else
											sInvalidMaxDef = sInvalidMaxDef.ListAdd(sCompTypeCd);
										Category.SetCheckParameter("Invalid_Maximum_Default", sInvalidMaxDef, eParameterDataType.String);
									}
									dvSpanRecords.RowFilter = sSpanFilter;
								}

								if (drLookup["ComponentTypeCode"] == DBNull.Value)
								{
									sFilter = AddToDataViewFilter(sDefaultFilter, string.Format("mon_loc_id = '{0}'", MonLocID));
									sFilter = AddToDataViewFilter(sFilter, string.Format("parameter_cd = '{0}'", sDefaultParamCd));
									sFilter = AddToDataViewFilter(sFilter, "default_purpose_cd = 'MD' and fuel_cd = 'NFS'");
									sFilter = AddToDataViewFilter(sFilter, string.Format("default_value = {0}", sMaxDefaultValue));
									sFilter = AddEvaluationDateHourRangeToDataViewFilter(sFilter, EvalBeginDate, EvalEndDate, EvalBeginHour, EvalEndHour, false, true);
									dvDefaultRecords.RowFilter = sFilter;

									if (dvDefaultRecords.Count == 0)
									{
										sInvalidMaxDef = (string)Category.GetCheckParameter("Invalid_Maximum_Default").ParameterValue;
										if (string.IsNullOrEmpty(sInvalidMaxDef))
											sInvalidMaxDef = sDefaultParamCd;
										else
											sInvalidMaxDef = sInvalidMaxDef.ListAdd(sDefaultParamCd);
										Category.SetCheckParameter("Invalid_Maximum_Default", sInvalidMaxDef, eParameterDataType.String);
									}

									dvDefaultRecords.RowFilter = sDefaultFilter;
								}

							} // end else for -- if less than 2 rows or only 1 fuel_cd
						}
					} // end if( bSubstituteDataCdValid && SubstituteDataCd == "FSP75" )

					// reset our filters!
					dvSpanRecords.RowFilter = sSpanFilter;
					dvDefaultRecords.RowFilter = sDefaultFilter;
					dvParaLookupTable.RowFilter = sParamLookupFilter;
					dvDefaultRecords.Sort = sDefaultSort;

					if (cDBConvert.ToBoolean(Category.GetCheckParameter("Method_Method_Code_Valid").ParameterValue) == true &&
						  bBypassApproachCdValid && BypassApproachCd == "BYMAXFS")
					{
						if (MethodCd == "SO2R")
							sFilter = AddToDataViewFilter(sParamLookupFilter, string.Format("MethodParameterCode = '{0}' AND DefaultParameterCode not in ('SO2X','FLOX')", ParameterCd));
						else
							sFilter = AddToDataViewFilter(sParamLookupFilter, string.Format("MethodParameterCode = '{0}' AND DefaultParameterCode not in ('SORX','FLOX')", ParameterCd));
						dvParaLookupTable.RowFilter = sFilter;

						sDefaultParamCd = cDBConvert.ToString(dvParaLookupTable[0].Row["DefaultParameterCode"]);

						sFilter = AddToDataViewFilter(sDefaultFilter, string.Format("parameter_cd = '{0}'", sDefaultParamCd));
						sFilter = AddToDataViewFilter(sFilter, string.Format("mon_loc_id = '{0}'", MonLocID));
						sFilter = AddToDataViewFilter(sFilter, "default_purpose_cd = 'MD' and fuel_cd <> 'NFS'");
						sFilter = AddEvaluationDateHourRangeToDataViewFilter(sFilter, EvalBeginDate, EvalEndDate, EvalBeginHour, EvalEndHour, false, true);
						dvDefaultRecords.RowFilter = sFilter;
						dvDefaultRecords.Sort = "fuel_cd asc";

						string sFuelCd = "", sThisFuelCd = "";
						int nNumFuelsCds = 1;
						foreach (DataRowView drDefault in dvDefaultRecords)
						{
							sThisFuelCd = cDBConvert.ToString(drDefault["fuel_cd"]);

							if (sFuelCd == "")
								sFuelCd = sThisFuelCd;
							if (sFuelCd != sThisFuelCd)
							{
								nNumFuelsCds++;
							}
						}

						if (dvDefaultRecords.Count > 0 && (nNumFuelsCds > 1 || sFuelCd == "MIX"))
						{
							dvDefaultRecords.Sort = "default_value DESC";
							sMaxDefaultValue = cDBConvert.ToString(dvDefaultRecords[0]["default_value"]);

							string ComponentTypeCd = cDBConvert.ToString(dvParaLookupTable[0].Row["ComponentTypeCode"]);
							if (ComponentTypeCd == "NOX" || ComponentTypeCd == "SO2")
							{
								sFilter = AddToDataViewFilter(sSpanFilter, string.Format("component_type_cd = '{0}'", ComponentTypeCd));
								sFilter = AddToDataViewFilter(sFilter, string.Format("mon_loc_id = '{0}'", MonLocID));
								sFilter = AddToDataViewFilter(sFilter, "span_scale_cd = 'H'");
								sFilter = AddToDataViewFilter(sFilter, string.Format("MPC_Value = {0}", sMaxDefaultValue));
								sFilter = AddEvaluationDateHourRangeToDataViewFilter(sFilter, EvalBeginDate, EvalEndDate, EvalBeginHour, EvalEndHour, false, true);
								dvSpanRecords.RowFilter = sFilter;

								if (dvSpanRecords.Count == 0)
								{
									sInvalidMaxDef = (string)Category.GetCheckParameter("Invalid_Maximum_Default").ParameterValue;
									if (string.IsNullOrEmpty(sInvalidMaxDef))
										sInvalidMaxDef = ComponentTypeCd;
									else
										sInvalidMaxDef = sInvalidMaxDef.ListAdd(ComponentTypeCd);
									Category.SetCheckParameter("Invalid_Maximum_Default", sInvalidMaxDef, eParameterDataType.String);
								}
							} // end -- if( ComponentTypeCd == "NOX" || ComponentTypeCd == "SO2" )

							if (dvParaLookupTable[0].Row["ComponentTypeCode"] == DBNull.Value)
							{
								sFilter = AddToDataViewFilter(sDefaultFilter, string.Format("parameter_cd = '{0}'", sDefaultParamCd));
								sFilter = AddToDataViewFilter(sFilter, string.Format("mon_loc_id = '{0}'", MonLocID));
								sFilter = AddToDataViewFilter(sFilter, "default_purpose_cd = 'MD' and fuel_cd = 'NFS'");
								sFilter = AddToDataViewFilter(sFilter, string.Format("default_value = {0}", sMaxDefaultValue));
								sFilter = AddEvaluationDateHourRangeToDataViewFilter(sFilter, EvalBeginDate, EvalEndDate, EvalBeginHour, EvalEndHour, false, true);
								dvDefaultRecords.RowFilter = sFilter;

								if (dvDefaultRecords.Count == 0)
								{
									sInvalidMaxDef = (string)Category.GetCheckParameter("Invalid_Maximum_Default").ParameterValue;
									if (string.IsNullOrEmpty(sInvalidMaxDef))
										sInvalidMaxDef = sDefaultParamCd;
									else
										sInvalidMaxDef = sInvalidMaxDef.ListAdd(sDefaultParamCd);
									Category.SetCheckParameter("Invalid_Maximum_Default", sInvalidMaxDef, eParameterDataType.String);
								}
							} // end -- if( dvParaLookupTable[0].Row["ComponentTypeCode"] == DBNull.Value )
						}
						else
						{
							sMissing = (string)Category.GetCheckParameter("Missing_Maximum_Default").ParameterValue;
							if (string.IsNullOrEmpty(sMissing))
								sMissing = sDefaultParamCd;
							else
								sMissing = sMissing.ListAdd(sDefaultParamCd);
							Category.SetCheckParameter("Missing_Maximum_Default", sMissing, eParameterDataType.String);
						} // end else for -- if( dvDefaultRecords.Count > 0 && ( nNumFuelsCds > 1 || sFuelCd == "MIX" ) )

					} // end if( bBypassApproachCdValid && BypassApproachCd == "BYMAXFS" )

					sMissing = (string)Category.GetCheckParameter("Missing_Maximum_Default").ParameterValue;
					sInvalidMaxDef = (string)Category.GetCheckParameter("Invalid_Maximum_Default").ParameterValue;

					if (string.IsNullOrEmpty(sMissing) == false && string.IsNullOrEmpty(sInvalidMaxDef) == true)
						Category.CheckCatalogResult = "A";

					if (string.IsNullOrEmpty(sMissing) == true && string.IsNullOrEmpty(sInvalidMaxDef) == false)
						Category.CheckCatalogResult = "B";

					if (string.IsNullOrEmpty(sMissing) == false && string.IsNullOrEmpty(sInvalidMaxDef) == false)
						Category.CheckCatalogResult = "C";

					if (bSubstituteDataCdValid && SubstituteDataCd == "MHHI")
					{
						sFilter = AddToDataViewFilter(sDefaultFilter, string.Format("mon_loc_id = '{0}'", MonLocID));
						sFilter = AddToDataViewFilter(sFilter, "parameter_cd = 'MHHI' and default_purpose_cd = 'LM'");
						sFilter = AddEvaluationDateHourRangeToDataViewFilter(sFilter, EvalBeginDate, EvalEndDate, EvalBeginHour, EvalEndHour, false, true);
						dvDefaultRecords.RowFilter = sFilter;
						if (dvDefaultRecords.Count == 0)
							Category.CheckCatalogResult = "D";
					}

					// reset our filters!
					dvSpanRecords.RowFilter = sSpanFilter;
					dvDefaultRecords.RowFilter = sDefaultFilter;
					dvParaLookupTable.RowFilter = sParamLookupFilter;
					dvDefaultRecords.Sort = sDefaultSort;

				} // end if dates consistent
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "METHOD29");
			}

			return ReturnVal;
		}

		public string METHOD30(cCategory Category, ref bool Log)
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentMethod = (DataRowView)Category.GetCheckParameter("Current_Method").ParameterValue;
				bool bMethodCodeValid = (bool)Category.GetCheckParameter("Method_Method_Code_Valid").ParameterValue;
				bool bDatesConsistant = (bool)Category.GetCheckParameter("Method_Dates_And_Hours_Consistent").ParameterValue;
				DateTime EvalEndDate = (DateTime)Category.GetCheckParameter("Method_Evaluation_End_Date").ParameterValue;
				DateTime EvalBeginDate = (DateTime)Category.GetCheckParameter("Method_Evaluation_Begin_Date").ParameterValue;
				int EvalEndHour = (int)Category.GetCheckParameter("Method_Evaluation_End_Hour").ParameterValue;
				int EvalBeginHour = (int)Category.GetCheckParameter("Method_Evaluation_Begin_Hour").ParameterValue;
				string sLocationType = cDBConvert.ToString(Category.GetCheckParameter("Location_Type").ParameterValue);
				DataRowView CurrentLocation = (DataRowView)Category.GetCheckParameter("Current_Location").ParameterValue;

				DataView dvFuelRecords = (DataView)Category.GetCheckParameter("Location_Fuel_Records").ParameterValue;
				DataView dvMonSysRecords = (DataView)Category.GetCheckParameter("Monitor_System_Records").ParameterValue;
				DataView dvFacSysRecords = (DataView)Category.GetCheckParameter("Facility_System_Records").ParameterValue;
				DataView dvFuelCodeLookup = (DataView)Category.GetCheckParameter("Fuel_Code_Lookup_Table").ParameterValue;
				DataView dvUnitStackConfigurationRecords = (DataView)Category.GetCheckParameter("Unit_Stack_Configuration_Records").ParameterValue;
				string sFuelFilter = dvFuelRecords.RowFilter;
				string sMonSysFilter = dvMonSysRecords.RowFilter;
				string sFacSysFilter = dvFacSysRecords.RowFilter;
				string sFuelCdLookupFilter = dvFuelCodeLookup.RowFilter;
				string sUSC_Filter = dvUnitStackConfigurationRecords.RowFilter;

				string MethodCd = cDBConvert.ToString(CurrentMethod["Method_Cd"]);
				string ParameterCd = cDBConvert.ToString(CurrentMethod["Parameter_Cd"]);
				string MonLocID = cDBConvert.ToString(CurrentMethod["mon_loc_id"]);
				string sUnit_ID = cDBConvert.ToString(CurrentLocation["Unit_ID"]);
				string sStackPipe_ID = cDBConvert.ToString(CurrentLocation["Stack_Pipe_ID"]);

				string sFilter = "";
				string sMissingFuel = "";
				string sIncompleteFuel = "";
				int nCount = 0;
				string sAllPipesMonLocIDs = "";
				string sDelim = "";
				string sFuelCd = "";
				string sAllFuelCds = "";

				if (ParameterCd.InList("HI,HIT") && bMethodCodeValid && bDatesConsistant)
				{
					if (MethodCd.StartsWith("AD") && sLocationType.StartsWith("U"))
					{
						sFilter = string.Format("MON_LOC_ID='{0}'", MonLocID);
						sFilter = AddToDataViewFilter(sFilter, "stack_name like 'CP%' or stack_name like 'MP%'");
						sFilter = AddEvaluationDateRangeToDataViewFilter(sFilter, EvalBeginDate, EvalEndDate, false, true, false);
						dvUnitStackConfigurationRecords.RowFilter = sFilter;

						foreach (DataRowView row in dvUnitStackConfigurationRecords)
						{
							sAllPipesMonLocIDs += string.Format("{0}'{1}'", sDelim, cDBConvert.ToString(row["STACK_PIPE_MON_LOC_ID"]));
							sDelim = ",";
						}

						sFilter = AddToDataViewFilter(sFuelFilter, string.Format("unit_id = {0}", sUnit_ID));
						sFilter = AddToDataViewFilter(sFilter, "fuel_group_cd = 'OIL' and indicator_cd in ('P','S')");
						sFilter = AddEvaluationDateRangeToDataViewFilter(sFilter, EvalBeginDate, EvalEndDate, false, true, false);
						dvFuelRecords.RowFilter = sFilter;
						foreach (DataRowView row in dvFuelRecords)
						{
							sFuelCd = cDBConvert.ToString(row["fuel_cd"]);

							sFilter = AddToDataViewFilter(sFuelCdLookupFilter, string.Format("unit_fuel_cd = '{0}'", sFuelCd));
							dvFuelCodeLookup.RowFilter = sFilter;

							sAllFuelCds = "";
							sDelim = "";
							foreach (DataRowView drFuelRow in dvFuelCodeLookup)
							{
								sAllFuelCds += string.Format("{1}'{0}'", cDBConvert.ToString(drFuelRow["fuel_cd"]), sDelim);
								sDelim = ",";
							}
							dvFuelCodeLookup.RowFilter = sFuelCdLookupFilter;

							if (sAllPipesMonLocIDs != "")
								sFilter = AddToDataViewFilter(sFacSysFilter, string.Format("MON_LOC_ID IN ({0}) or MON_LOC_ID = '{1}'", sAllPipesMonLocIDs, MonLocID));
							else
								sFilter = AddToDataViewFilter(sFacSysFilter, string.Format("MON_LOC_ID = '{0}'", MonLocID));

							sFilter = AddToDataViewFilter(sFilter, "sys_type_cd in ('OILM','OILV') and sys_designation_cd = 'P'");
							sFilter = AddToDataViewFilter(sFilter, string.Format("fuel_cd in ({0})", sAllFuelCds));
							sFilter = AddEvaluationDateHourRangeToDataViewFilter(sFilter, EvalBeginDate, EvalEndDate, EvalBeginHour, EvalEndHour, false, true, true);
							dvFacSysRecords.RowFilter = sFilter;

							if (dvFacSysRecords.Count == 0)
							{
								sMissingFuel = (string)Category.GetCheckParameter("Missing_Fuel_System_For_Method").ParameterValue;
								if (string.IsNullOrEmpty(sMissingFuel))
									sMissingFuel = sFuelCd;
								else
									sMissingFuel = sMissingFuel.ListAdd(sFuelCd);
								Category.SetCheckParameter("Missing_Fuel_System_For_Method", sMissingFuel.FormatList(), eParameterDataType.String);
							}
							else
							{
								//find intersection dates
								DateTime IntrsxnBeginDate = cDBConvert.ToDate(row["begin_date"], DateTypes.START);//fuel start Date
								DateTime IntrsxnEndDate = cDBConvert.ToDate(row["end_date"], DateTypes.END);//fuel end Date

								if (IntrsxnBeginDate < EvalBeginDate)
									IntrsxnBeginDate = EvalBeginDate;
								if (IntrsxnEndDate > EvalEndDate)
									IntrsxnEndDate = EvalEndDate;

								if (!CheckForHourRangeCovered(Category, dvFacSysRecords, IntrsxnBeginDate, 23, IntrsxnEndDate, 0, ref nCount))
								{
									sIncompleteFuel = (string)Category.GetCheckParameter("Incomplete_Fuel_System_For_Method").ParameterValue;
									if (string.IsNullOrEmpty(sIncompleteFuel))
										sIncompleteFuel = sFuelCd;
									else
										sIncompleteFuel = sIncompleteFuel.ListAdd(sFuelCd);
									Category.SetCheckParameter("Incomplete_Fuel_System_For_Method", sIncompleteFuel.FormatList(), eParameterDataType.String);
								}
							}

							dvFacSysRecords.RowFilter = sFacSysFilter;
						} // end -- foreach row in fuels
						dvFuelRecords.RowFilter = sFuelFilter;

						sFilter = AddToDataViewFilter(sFuelFilter, string.Format("unit_id = {0}", sUnit_ID));
						sFilter = AddToDataViewFilter(sFilter, "fuel_group_cd = 'GAS' and indicator_cd in ('P','S')");
						sFilter = AddEvaluationDateRangeToDataViewFilter(sFilter, EvalBeginDate, EvalEndDate, false, true, false);
						dvFuelRecords.RowFilter = sFilter;
						foreach (DataRowView row in dvFuelRecords)
						{
							sFuelCd = cDBConvert.ToString(row["fuel_cd"]);

							sFilter = AddToDataViewFilter(sFuelCdLookupFilter, string.Format("unit_fuel_cd = '{0}'", sFuelCd));
							dvFuelCodeLookup.RowFilter = sFilter;

							sAllFuelCds = "";
							sDelim = "";
							foreach (DataRowView drFuelRow in dvFuelCodeLookup)
							{
								sAllFuelCds += string.Format("{1}'{0}'", cDBConvert.ToString(drFuelRow["fuel_cd"]), sDelim);
								sDelim = ",";
							}
							dvFuelCodeLookup.RowFilter = sFuelCdLookupFilter;

							if (sAllPipesMonLocIDs != "")
								sFilter = AddToDataViewFilter(sFacSysFilter, string.Format("MON_LOC_ID IN ({0}) or MON_LOC_ID = '{1}'", sAllPipesMonLocIDs, MonLocID));
							else
								sFilter = AddToDataViewFilter(sFacSysFilter, string.Format("MON_LOC_ID = '{0}'", MonLocID));

							sFilter = AddToDataViewFilter(sFilter, "sys_type_cd = 'GAS' and sys_designation_cd = 'P'");
							sFilter = AddToDataViewFilter(sFilter, string.Format("fuel_cd in ({0})", sAllFuelCds));
							sFilter = AddEvaluationDateHourRangeToDataViewFilter(sFilter, EvalBeginDate, EvalEndDate, EvalBeginHour, EvalEndHour, false, true, true);
							dvFacSysRecords.RowFilter = sFilter;

							if (dvFacSysRecords.Count == 0)
							{
								sMissingFuel = (string)Category.GetCheckParameter("Missing_Fuel_System_For_Method").ParameterValue;
								if (string.IsNullOrEmpty(sMissingFuel))
									sMissingFuel = sFuelCd;
								else
									sMissingFuel = sMissingFuel.ListAdd(sFuelCd);
								Category.SetCheckParameter("Missing_Fuel_System_For_Method", sMissingFuel.FormatList(), eParameterDataType.String);
							}
							else
							{
								//find intersection dates
								DateTime IntrsxnBeginDate = cDBConvert.ToDate(row["begin_date"], DateTypes.START);//fuel start Date
								DateTime IntrsxnEndDate = cDBConvert.ToDate(row["end_date"], DateTypes.END);//fuel end Date

								if (IntrsxnBeginDate < EvalBeginDate)
									IntrsxnBeginDate = EvalBeginDate;
								if (IntrsxnEndDate > EvalEndDate)
									IntrsxnEndDate = EvalEndDate;

								if (!CheckForHourRangeCovered(Category, dvFacSysRecords, IntrsxnBeginDate, 23, IntrsxnEndDate, 0, ref nCount))
								{
									sIncompleteFuel = (string)Category.GetCheckParameter("Incomplete_Fuel_System_For_Method").ParameterValue;
									if (string.IsNullOrEmpty(sIncompleteFuel))
										sIncompleteFuel = sFuelCd;
									else
										sIncompleteFuel = sIncompleteFuel.ListAdd(sFuelCd);
									Category.SetCheckParameter("Incomplete_Fuel_System_For_Method", sIncompleteFuel.FormatList(), eParameterDataType.String);
								}
							}

							dvFacSysRecords.RowFilter = sFacSysFilter;
						} // end -- foreach row in fuels
						dvFuelRecords.RowFilter = sFuelFilter;

					} // end -- if( MethodCd.StartsWith( "AD" ) && sLocationType.StartsWith( "U" ) )

					if (MethodCd.StartsWith("LTF") && sLocationType.StartsWith("U"))
					{
						sFilter = string.Format("unit_id = {0}", sUnit_ID);
						sFilter = AddToDataViewFilter(sFilter, "stack_name like 'CP%'");
						sFilter = AddEvaluationDateRangeToDataViewFilter(sFilter, EvalBeginDate, EvalEndDate, false, true, false);
						dvUnitStackConfigurationRecords.RowFilter = sFilter;

						foreach (DataRowView row in dvUnitStackConfigurationRecords)
						{
							sAllPipesMonLocIDs += string.Format("{0}'{1}'", sDelim, cDBConvert.ToString(row["MON_LOC_ID"]));
							sDelim = ",";
						}

						sFilter = AddToDataViewFilter(sFuelFilter, string.Format("unit_id = {0}", sUnit_ID));
						sFilter = AddToDataViewFilter(sFilter, "fuel_group_cd = 'OIL' and indicator_cd in ('P','S')");
						sFilter = AddEvaluationDateRangeToDataViewFilter(sFilter, EvalBeginDate, EvalEndDate, false, true, false);
						dvFuelRecords.RowFilter = sFilter;
						foreach (DataRowView row in dvFuelRecords)
						{
							sFuelCd = cDBConvert.ToString(row["fuel_cd"]);

							sFilter = AddToDataViewFilter(sFuelCdLookupFilter, string.Format("unit_fuel_cd = '{0}'", sFuelCd));
							dvFuelCodeLookup.RowFilter = sFilter;

							sAllFuelCds = "";
							sDelim = "";
							foreach (DataRowView drFuelRow in dvFuelCodeLookup)
							{
								sAllFuelCds += string.Format("{1}'{0}'", cDBConvert.ToString(drFuelRow["fuel_cd"]), sDelim);
								sDelim = ",";
							}
							dvFuelCodeLookup.RowFilter = sFuelCdLookupFilter;

							if (sAllPipesMonLocIDs != "")
								sFilter = AddToDataViewFilter(sFacSysFilter, string.Format("MON_LOC_ID IN ({0}) or MON_LOC_ID = '{1}'", sAllPipesMonLocIDs, MonLocID));
							else
								sFilter = AddToDataViewFilter(sFacSysFilter, string.Format("MON_LOC_ID = '{0}'", MonLocID));

							sFilter = AddToDataViewFilter(sFilter, "sys_type_cd = 'LTOL' and sys_designation_cd = 'P'");
							sFilter = AddToDataViewFilter(sFilter, string.Format("fuel_cd in ({0})", sAllFuelCds));
							sFilter = AddEvaluationDateHourRangeToDataViewFilter(sFilter, EvalBeginDate, EvalEndDate, EvalBeginHour, EvalEndHour, false, true, true);
							dvFacSysRecords.RowFilter = sFilter;

							if (dvFacSysRecords.Count == 0)
							{
								sMissingFuel = (string)Category.GetCheckParameter("Missing_Fuel_System_For_Method").ParameterValue;
								if (string.IsNullOrEmpty(sMissingFuel))
									sMissingFuel = sFuelCd;
								else
									sMissingFuel = sMissingFuel.ListAdd(sFuelCd);
								Category.SetCheckParameter("Missing_Fuel_System_For_Method", sMissingFuel.FormatList(), eParameterDataType.String);
							}
							else
							{
								//find intersection dates
								DateTime IntrsxnBeginDate = cDBConvert.ToDate(row["begin_date"], DateTypes.START);//fuel start Date
								DateTime IntrsxnEndDate = cDBConvert.ToDate(row["end_date"], DateTypes.END);//fuel end Date

								if (IntrsxnBeginDate < EvalBeginDate)
									IntrsxnBeginDate = EvalBeginDate;
								if (IntrsxnEndDate > EvalEndDate)
									IntrsxnEndDate = EvalEndDate;

								if (!CheckForHourRangeCovered(Category, dvFacSysRecords, IntrsxnBeginDate, 23, IntrsxnEndDate, 0, ref nCount))
								{
									sIncompleteFuel = (string)Category.GetCheckParameter("Incomplete_Fuel_System_For_Method").ParameterValue;
									if (string.IsNullOrEmpty(sIncompleteFuel))
										sIncompleteFuel = sFuelCd;
									else
										sIncompleteFuel = sIncompleteFuel.ListAdd(sFuelCd);
									Category.SetCheckParameter("Incomplete_Fuel_System_For_Method", sIncompleteFuel.FormatList(), eParameterDataType.String);
								}
							}

							dvFacSysRecords.RowFilter = sFacSysFilter;
						} // end -- foreach row in fuels
						dvFuelRecords.RowFilter = sFuelFilter;

						sFilter = AddToDataViewFilter(sFuelFilter, string.Format("unit_id = {0}", sUnit_ID));
						sFilter = AddToDataViewFilter(sFilter, "fuel_group_cd = 'GAS' and indicator_cd in ('P','S')");
						sFilter = AddEvaluationDateRangeToDataViewFilter(sFilter, EvalBeginDate, EvalEndDate, false, true, false);
						dvFuelRecords.RowFilter = sFilter;
						foreach (DataRowView row in dvFuelRecords)
						{
							sFuelCd = cDBConvert.ToString(row["fuel_cd"]);

							sFilter = AddToDataViewFilter(sFuelCdLookupFilter, string.Format("unit_fuel_cd = '{0}'", sFuelCd));
							dvFuelCodeLookup.RowFilter = sFilter;

							sAllFuelCds = "";
							sDelim = "";
							foreach (DataRowView drFuelRow in dvFuelCodeLookup)
							{
								sAllFuelCds += string.Format("{1}'{0}'", cDBConvert.ToString(drFuelRow["fuel_cd"]), sDelim);
								sDelim = ",";
							}
							dvFuelCodeLookup.RowFilter = sFuelCdLookupFilter;

							if (sAllPipesMonLocIDs != "")
								sFilter = AddToDataViewFilter(sFacSysFilter, string.Format("MON_LOC_ID IN ({0}) or MON_LOC_ID = '{1}'", sAllPipesMonLocIDs, MonLocID));
							else
								sFilter = AddToDataViewFilter(sFacSysFilter, string.Format("MON_LOC_ID = '{0}'", MonLocID));

							sFilter = AddToDataViewFilter(sFilter, "sys_type_cd = 'LTGS' and sys_designation_cd = 'P'");
							sFilter = AddToDataViewFilter(sFilter, string.Format("fuel_cd in ({0})", sAllFuelCds));
							sFilter = AddEvaluationDateHourRangeToDataViewFilter(sFilter, EvalBeginDate, EvalEndDate, EvalBeginHour, EvalEndHour, false, true, true);
							dvFacSysRecords.RowFilter = sFilter;

							if (dvFacSysRecords.Count == 0)
							{
								sMissingFuel = (string)Category.GetCheckParameter("Missing_Fuel_System_For_Method").ParameterValue;
								if (string.IsNullOrEmpty(sMissingFuel))
									sMissingFuel = sFuelCd;
								else
									sMissingFuel = sMissingFuel.ListAdd(sFuelCd);
								Category.SetCheckParameter("Missing_Fuel_System_For_Method", sMissingFuel.FormatList(), eParameterDataType.String);
							}
							else
							{
								//find intersection dates
								DateTime IntrsxnBeginDate = cDBConvert.ToDate(row["begin_date"], DateTypes.START);//fuel start Date
								DateTime IntrsxnEndDate = cDBConvert.ToDate(row["end_date"], DateTypes.END);//fuel end Date

								if (IntrsxnBeginDate < EvalBeginDate)
									IntrsxnBeginDate = EvalBeginDate;
								if (IntrsxnEndDate > EvalEndDate)
									IntrsxnEndDate = EvalEndDate;

								if (!CheckForHourRangeCovered(Category, dvFacSysRecords, IntrsxnBeginDate, 23, IntrsxnEndDate, 0, ref nCount))
								{
									sIncompleteFuel = (string)Category.GetCheckParameter("Incomplete_Fuel_System_For_Method").ParameterValue;
									if (string.IsNullOrEmpty(sIncompleteFuel))
										sIncompleteFuel = sFuelCd;
									else
										sIncompleteFuel = sIncompleteFuel.ListAdd(sFuelCd);
									Category.SetCheckParameter("Incomplete_Fuel_System_For_Method", sIncompleteFuel.FormatList(), eParameterDataType.String);
								}
							}

							dvFacSysRecords.RowFilter = sFacSysFilter;
						} // end -- foreach row in fuels
						dvFuelRecords.RowFilter = sFuelFilter;
					} // end -- if( MethodCd.StartsWith( "LTF" ) && sLocationType.StartsWith( "U" ) )

					sMissingFuel = (string)Category.GetCheckParameter("Missing_Fuel_System_For_Method").ParameterValue;
					sIncompleteFuel = (string)Category.GetCheckParameter("Incomplete_Fuel_System_For_Method").ParameterValue;

					if (string.IsNullOrEmpty(sMissingFuel) == false && string.IsNullOrEmpty(sIncompleteFuel) == true)
						Category.CheckCatalogResult = "A";
					else if (string.IsNullOrEmpty(sMissingFuel) == true && string.IsNullOrEmpty(sIncompleteFuel) == false)
						Category.CheckCatalogResult = "B";
					else if (string.IsNullOrEmpty(sMissingFuel) == false && string.IsNullOrEmpty(sIncompleteFuel) == false)
						Category.CheckCatalogResult = "C";
					else
					{
						if (MethodCd.StartsWith("AD"))
						{
							sFilter = AddToDataViewFilter(sMonSysFilter, "sys_type_cd in ('OILM','OILV','GAS') and sys_designation_cd = 'P'");
							sFilter = AddEvaluationDateHourRangeToDataViewFilter(sFilter, EvalBeginDate, EvalEndDate, EvalBeginHour, EvalEndHour, false, true, true);
							dvMonSysRecords.RowFilter = sFilter;

							if (dvMonSysRecords.Count == 0)
							{
								Category.CheckCatalogResult = "D";
							}
							else if (!CheckForHourRangeCovered(Category, dvMonSysRecords, EvalBeginDate, EvalBeginHour, EvalEndDate, EvalEndHour, ref nCount))
							{
								Category.CheckCatalogResult = "E";
							}

							dvMonSysRecords.RowFilter = sMonSysFilter;
						}

						if (MethodCd.StartsWith("LTF"))
						{
							sFilter = AddToDataViewFilter(sMonSysFilter, "sys_type_cd in ('LTOL','LTGS') and sys_designation_cd = 'P'");
							sFilter = AddEvaluationDateHourRangeToDataViewFilter(sFilter, EvalBeginDate, EvalEndDate, EvalBeginHour, EvalEndHour, false, true, true);
							dvMonSysRecords.RowFilter = sFilter;

							if (dvMonSysRecords.Count == 0)
							{
								Category.CheckCatalogResult = "D";
							}
							else if (!CheckForHourRangeCovered(Category, dvMonSysRecords, EvalBeginDate, EvalBeginHour, EvalEndDate, EvalEndHour, ref nCount))
							{
								Category.CheckCatalogResult = "E";
							}

							dvMonSysRecords.RowFilter = sMonSysFilter;
						}
					}
				}

				// make sure these get reset!
				dvFuelRecords.RowFilter = sFuelFilter;
				dvMonSysRecords.RowFilter = sMonSysFilter;
				dvFacSysRecords.RowFilter = sFacSysFilter;
				dvFuelCodeLookup.RowFilter = sFuelCdLookupFilter;
				dvUnitStackConfigurationRecords.RowFilter = sUSC_Filter;
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "METHOD30");
			}

			return ReturnVal;
		}

		#endregion


		#region Checks 31 - 40

		public string METHOD31(cCategory Category, ref bool Log)
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentMethod = (DataRowView)Category.GetCheckParameter("Current_Method").ParameterValue;
				//bool bMethodCodeValid = (bool)Category.GetCheckParameter("Method_Method_Code_Valid").ParameterValue;
				bool bDatesConsistant = (bool)Category.GetCheckParameter("Method_Dates_And_Hours_Consistent").ParameterValue;
				DateTime EvalEndDate = (DateTime)Category.GetCheckParameter("Method_Evaluation_End_Date").ParameterValue;
				DateTime EvalBeginDate = (DateTime)Category.GetCheckParameter("Method_Evaluation_Begin_Date").ParameterValue;

				string MethodCd = cDBConvert.ToString(CurrentMethod["Method_Cd"]);
				string ParameterCd = cDBConvert.ToString(CurrentMethod["Parameter_Cd"]);
				string MonLocID = cDBConvert.ToString(CurrentMethod["MON_LOC_ID"]);

				if (ParameterCd == "NOXR" && MethodCd == "AE" && bDatesConsistant)
				{
					string sFilter = null;

					DataView dvUSCRecords = (DataView)Category.GetCheckParameter("Unit_Stack_Configuration_Records").ParameterValue;
					string sUSCFilter = dvUSCRecords.RowFilter;

					DataView dvQualRecords = (DataView)Category.GetCheckParameter("Facility_Qualification_Records").ParameterValue;
					string sQualFilter = dvQualRecords.RowFilter;

					string sLocationType = (string)Category.GetCheckParameter("Location_Type").ParameterValue;
					if (sLocationType == "MP")
					{
						sFilter = AddToDataViewFilter(sUSCFilter, string.Format("STACK_PIPE_MON_LOC_ID='{0}'", MonLocID));
						dvUSCRecords.RowFilter = sFilter;
						string sUnitID = dvUSCRecords[0]["UNITID"].ToString();

						sFilter = AddToDataViewFilter(sQualFilter, "QUAL_TYPE_CD in ('PK','SK')");
						sFilter = AddToDataViewFilter(sFilter, string.Format("LOCATION_ID='{0}'", sUnitID));
						sFilter = AddEvaluationDateRangeToDataViewFilter(sFilter, EvalBeginDate, EvalEndDate, false, true, false);
						dvQualRecords.RowFilter = sFilter;
					}
					else
					{
						sFilter = AddToDataViewFilter(sQualFilter, "QUAL_TYPE_CD in ('PK','SK')");
						sFilter = AddToDataViewFilter(sFilter, string.Format("mon_loc_id = '{0}'", MonLocID));
						sFilter = AddEvaluationDateRangeToDataViewFilter(sFilter, EvalBeginDate, EvalEndDate, false, true, false);
						dvQualRecords.RowFilter = sFilter;
					}

					if (dvQualRecords.Count == 0)
					{
						Category.CheckCatalogResult = "A";
					}
					else if (!CheckForDateRangeCovered(Category, dvQualRecords, EvalBeginDate, EvalEndDate, false))
					{
						Category.CheckCatalogResult = "B";
					}

					dvUSCRecords.RowFilter = sUSCFilter;
					dvQualRecords.RowFilter = sQualFilter;
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "METHOD31");
			}

			return ReturnVal;
		}

		public string METHOD32(cCategory Category, ref bool Log)
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentMethod = (DataRowView)Category.GetCheckParameter("Current_Method").ParameterValue;
				bool bMethodCodeValid = (bool)Category.GetCheckParameter("Method_Method_Code_Valid").ParameterValue;
				bool bDatesConsistant = (bool)Category.GetCheckParameter("Method_Dates_And_Hours_Consistent").ParameterValue;
				DateTime EvalEndDate = (DateTime)Category.GetCheckParameter("Method_Evaluation_End_Date").ParameterValue;
				DateTime EvalBeginDate = (DateTime)Category.GetCheckParameter("Method_Evaluation_Begin_Date").ParameterValue;
				int EvalEndHour = (int)Category.GetCheckParameter("Method_Evaluation_End_Hour").ParameterValue;
				int EvalBeginHour = (int)Category.GetCheckParameter("Method_Evaluation_Begin_Hour").ParameterValue;

				string MethodCd = cDBConvert.ToString(CurrentMethod["Method_Cd"]);
				string ParameterCd = cDBConvert.ToString(CurrentMethod["Parameter_Cd"]);
				string MonLocID = cDBConvert.ToString(CurrentMethod["mon_loc_id"]);

				string sMissingNox = "";
				string sIncompleteNox = "";

				if (ParameterCd == "NOXR" && MethodCd == "AE" && bDatesConsistant)
				{
					Category.SetCheckParameter("Nox_System_Type", "NOXE", eParameterDataType.String);

					DataView dvMonSysRecords = (DataView)Category.GetCheckParameter("Monitor_System_Records").ParameterValue;
					string sMonSysFilter = dvMonSysRecords.RowFilter;

					int nCount = 0;

					string sFilter = AddToDataViewFilter(sMonSysFilter, "sys_type_cd = 'NOXE' and sys_designation_cd = 'P' and fuel_cd = 'MIX'");
					sFilter = AddToDataViewFilter(sFilter, string.Format("mon_loc_id = '{0}'", MonLocID));
					sFilter = AddEvaluationDateHourRangeToDataViewFilter(sFilter, EvalBeginDate, EvalEndDate, EvalBeginHour, EvalEndHour, false, true, true);
					dvMonSysRecords.RowFilter = sFilter;

					if (dvMonSysRecords.Count == 0)
					{
						DataView dvFuelRecords = (DataView)Category.GetCheckParameter("Location_Fuel_Records").ParameterValue;
						DataView dvFuelCodeLookup = (DataView)Category.GetCheckParameter("Fuel_Code_Lookup_Table").ParameterValue;
						string sFuelFilter = dvFuelRecords.RowFilter;
						string sFuelCdLookupFilter = dvFuelCodeLookup.RowFilter;

						sFilter = AddToDataViewFilter(sFuelFilter, "fuel_group_cd in ('OIL','GAS') and indicator_cd in ('P','S')");
						sFilter = AddToDataViewFilter(sFilter, string.Format("mon_loc_id = '{0}'", MonLocID));
						sFilter = AddEvaluationDateRangeToDataViewFilter(sFilter, EvalBeginDate, EvalEndDate, false, true, false);
						dvFuelRecords.RowFilter = sFilter;

						string sFuelCd = "";

						foreach (DataRowView row in dvFuelRecords)
						{
							sFuelCd = cDBConvert.ToString(row["fuel_cd"]);
							sFilter = AddToDataViewFilter(sFuelCdLookupFilter, string.Format("Unit_Fuel_CD = '{0}'", sFuelCd));
							dvFuelCodeLookup.RowFilter = sFilter;

							string sAllFuelCds = "";
							string sDelim = "";
							foreach (DataRowView drFuelRow in dvFuelCodeLookup)
							{
								sAllFuelCds += string.Format("{1}'{0}'", cDBConvert.ToString(drFuelRow["fuel_cd"]), sDelim);
								sDelim = ",";
							}

							sFilter = AddToDataViewFilter(sMonSysFilter, "sys_type_cd = 'NOXE' and sys_designation_cd = 'P'");
							sFilter = AddToDataViewFilter(sFilter, string.Format("fuel_cd in ({0})", sAllFuelCds));
							sFilter = AddEvaluationDateHourRangeToDataViewFilter(sFilter, EvalBeginDate, EvalEndDate, EvalBeginHour, EvalEndHour, false, true, true);
							dvMonSysRecords.RowFilter = sFilter;

							if (dvMonSysRecords.Count == 0)
							{
								sMissingNox = (string)Category.GetCheckParameter("Missing_Nox_System_For_Method").ParameterValue;
								if (string.IsNullOrEmpty(sMissingNox))
									sMissingNox = sFuelCd;
								else
									sMissingNox = sMissingNox.ListAdd(sFuelCd);
								Category.SetCheckParameter("Missing_Nox_System_For_Method", sMissingNox.FormatList(), eParameterDataType.String);
							}
							else
							{
								//find intersection dates
								DateTime IntrsxnBeginDate = cDBConvert.ToDate(row["begin_date"], DateTypes.START);//fuel start Date
								DateTime IntrsxnEndDate = cDBConvert.ToDate(row["end_date"], DateTypes.END);//fuel end Date

								if (IntrsxnBeginDate < EvalBeginDate)
									IntrsxnBeginDate = EvalBeginDate;
								if (IntrsxnEndDate > EvalEndDate)
									IntrsxnEndDate = EvalEndDate;

								if (!CheckForHourRangeCovered(Category, dvMonSysRecords, IntrsxnBeginDate, EvalBeginHour, IntrsxnEndDate, EvalEndHour, ref nCount))
								{
									sIncompleteNox = (string)Category.GetCheckParameter("Incomplete_Nox_System_For_Method").ParameterValue;
									if (string.IsNullOrEmpty(sIncompleteNox))
										sIncompleteNox = sFuelCd;
									else
										sIncompleteNox = sIncompleteNox.ListAdd(sFuelCd);
									Category.SetCheckParameter("Incomplete_Nox_System_For_Method", sIncompleteNox.FormatList(), eParameterDataType.String);
								}
							}
							dvMonSysRecords.RowFilter = sMonSysFilter;
							dvFuelCodeLookup.RowFilter = sFuelCdLookupFilter;
						}

						// reset all the filters!
						dvFuelRecords.RowFilter = sFuelFilter;
						dvMonSysRecords.RowFilter = sMonSysFilter;
						dvFuelCodeLookup.RowFilter = sFuelCdLookupFilter;
					}

					if (dvMonSysRecords.Count > 0 && !CheckForHourRangeCovered(Category, dvMonSysRecords, EvalBeginDate, EvalBeginHour, EvalEndDate, EvalEndHour, ref nCount))
					{
						sIncompleteNox = (string)Category.GetCheckParameter("Incomplete_Nox_System_For_Method").ParameterValue;
						if (string.IsNullOrEmpty(sIncompleteNox))
							sIncompleteNox = "MIX";
						else
							sIncompleteNox += ",MIX";
						Category.SetCheckParameter("Incomplete_Nox_System_For_Method", sIncompleteNox.FormatList(), eParameterDataType.String);
					}

					dvMonSysRecords.RowFilter = sMonSysFilter;

					sMissingNox = (string)Category.GetCheckParameter("Missing_Nox_System_For_Method").ParameterValue;
					sIncompleteNox = (string)Category.GetCheckParameter("Incomplete_Nox_System_For_Method").ParameterValue;

					if (string.IsNullOrEmpty(sMissingNox) == false && string.IsNullOrEmpty(sIncompleteNox) == true)
						Category.CheckCatalogResult = "A";
					if (string.IsNullOrEmpty(sMissingNox) == true && string.IsNullOrEmpty(sIncompleteNox) == false)
						Category.CheckCatalogResult = "B";
					if (string.IsNullOrEmpty(sMissingNox) == false && string.IsNullOrEmpty(sIncompleteNox) == false)
						Category.CheckCatalogResult = "C";
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "METHOD32");
			}

			return ReturnVal;
		}

		// Required LME Qualification Reported for LME Method
		public string METHOD33(cCategory Category, ref bool Log)
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentMethod = (DataRowView)Category.GetCheckParameter("Current_Method").ParameterValue;
				bool MethodCdValid = (bool)Category.GetCheckParameter("Method_Method_Code_Valid").ParameterValue;
				bool DatesConsistent = (bool)Category.GetCheckParameter("Method_Dates_and_Hours_Consistent").ParameterValue;
				string MethodCd = cDBConvert.ToString(CurrentMethod["method_cd"]);
				string ParamCd = cDBConvert.ToString(CurrentMethod["parameter_cd"]);


				if (MethodCdValid && MethodCd.InList("LME") && DatesConsistent)
				{
					DataView QualRecs = Category.GetCheckParameter("Facility_Qualification_Records").ValueAsDataView();
					DateTime EvalBeginDate = Category.GetCheckParameter("Method_Evaluation_Begin_Date").ValueAsDateTime(DateTypes.START);
					DateTime EvalEndDate = Category.GetCheckParameter("Method_Evaluation_End_Date").ValueAsDateTime(DateTypes.END);

					string monLocId = cDBConvert.ToString(CurrentMethod["mon_loc_Id"]);
					string QualFilter = QualRecs.RowFilter;
					DateTime SpanEndDate = new DateTime(cDBConvert.ToDate(CurrentMethod["end_date"], DateTypes.END).Year - 1, 12, 31);

					if (ParamCd == "CO2M" || ParamCd == "SO2M")
					{
						QualRecs.RowFilter = AddToDataViewFilter(QualFilter, "mon_loc_id = '" + monLocId + "' and qual_type_cd = 'LMEA'");
						QualRecs.RowFilter = AddEvaluationDateRangeToDataViewFilter(QualRecs.RowFilter, EvalBeginDate, EvalEndDate, false, true, false);
						if (QualRecs.Count == 0)
						{
							Category.SetCheckParameter("Missing_Qualification_for_Method", "LMEA", eParameterDataType.String);
							Category.CheckCatalogResult = "A";
						}
						else
						{
							if (CurrentMethod["end_date"] == DBNull.Value)
							{
								if (!CheckForDateRangeCovered(Category, QualRecs, EvalBeginDate, EvalEndDate))
								{
									Category.SetCheckParameter("Incomplete_Qualification_for_Method", "LMEA", eParameterDataType.String);
									Category.CheckCatalogResult = "B";
								}
							}
							else
							{
								if (!CheckForDateRangeCovered(Category, QualRecs, EvalBeginDate, SpanEndDate))
								{
									Category.SetCheckParameter("Incomplete_Qualification_for_Method", "LMEA", eParameterDataType.String);
									Category.CheckCatalogResult = "B";
								}
							}
						}
					}
					else
					{
						string MissingQual = "", IncompleteQual = "";
						Category.SetCheckParameter("Missing_Qualification_for_Method", "", eParameterDataType.String);
						Category.SetCheckParameter("Incomplete_Qualification_for_Method", "", eParameterDataType.String);

						bool annualReportingFrequencyFound = false;

						DataView ReportingFreqRecs = Category.GetCheckParameter("Location_Reporting_Frequency_Records").ValueAsDataView();
						string RptFreqFilter = ReportingFreqRecs.RowFilter;

						ReportingFreqRecs.RowFilter = AddToDataViewFilter(RptFreqFilter, "mon_loc_id = '" + monLocId + "' and report_freq_cd = 'Q'");
						// ReportingFreqRecs.RowFilter = AddEvaluationDateRangeToDataViewFilter(ReportingFreqRecs.RowFilter, EvalBeginDate, EvalEndDate, false, true, false);

						DateTime BeginDate = DateTime.MinValue, EndDate;

						foreach (DataRowView RptFreqRec in ReportingFreqRecs)
						{
							BeginDate = cDateFunctions.StartDateThisQuarter(cDBConvert.ToInteger(RptFreqRec["begin_rpt_period_id"]));

							EndDate = cDateFunctions.StartDateThisQuarter(cDBConvert.ToInteger(RptFreqRec["end_rpt_period_id"]) + 1).AddDays(-1);

							if (BeginDate <= EvalEndDate && (RptFreqRec["end_quarter"] == DBNull.Value || EndDate >= EvalBeginDate))
							{
								annualReportingFrequencyFound = true;
								break;
							}
						}

						if (annualReportingFrequencyFound)
						{
							DateTime LaterBeginDate = BeginDate;
							if (EvalBeginDate > LaterBeginDate)
								LaterBeginDate = EvalBeginDate;

							QualRecs.RowFilter = AddToDataViewFilter(QualFilter, "mon_loc_id = '" + monLocId + "' and qual_type_cd = 'LMEA'");
							QualRecs.RowFilter = AddEvaluationDateRangeToDataViewFilter(QualRecs.RowFilter, LaterBeginDate, EvalEndDate, false, true, false);
							if (QualRecs.Count == 0)
							{
								Category.SetCheckParameter("Missing_Qualification_for_Method", "LMEA", eParameterDataType.String);
								MissingQual = "LMEA";
							}
							else
							{
								if (CurrentMethod["end_date"] == DBNull.Value)
								{
									if (!CheckForDateRangeCovered(Category, QualRecs, LaterBeginDate, EvalEndDate))
									{
										Category.SetCheckParameter("Incomplete_Qualification_for_Method", "LMEA", eParameterDataType.String);
										IncompleteQual = "LMEA";
									}
								}
								else
								{
									if (!CheckForDateRangeCovered(Category, QualRecs, LaterBeginDate, SpanEndDate))
									{
										Category.SetCheckParameter("Incomplete_Qualification_for_Method", "LMEA", eParameterDataType.String);
										IncompleteQual = "LMEA";
									}
								}
							}
						}
						ReportingFreqRecs.RowFilter = RptFreqFilter;


						// Determine whether LMES is required, and if so whether it is missin.
						{
							DataView locationProgramParameterView;
							{
								locationProgramParameterView
									= cRowFilter.FindRows(LocationProgramParameterRecords.Value,
														new cFilterCondition[] 
                                        { 
                                        new cFilterCondition("PARAMETER_CD", "NOX"),
                                        new cFilterCondition("PRG_CD", MpParameters.ProgramIsOzoneSeasonList, eFilterConditionStringCompare.InList),
                                        new cFilterCondition("REQUIRED_IND", 1, eFilterDataType.Integer),
                                        new cFilterCondition("CLASS", "A,B", eFilterConditionStringCompare.InList),
                                        //UMCB Date check for less than Evaluation End and not null
                                        new cFilterCondition("UMCB_DATE", MethodEvaluationEndDate.AsBeginDateTime(), eFilterDataType.DateEnded, eFilterConditionRelativeCompare.LessThanOrEqual),
                                        //Program End Date check for greater than Evaluation Begin or is null
                                        new cFilterCondition("PRG_END_DATE", MethodEvaluationBeginDate.AsBeginDateTime(), eFilterDataType.DateEnded, eFilterConditionRelativeCompare.GreaterThanOrEqual)
                                        });
							}

							if (locationProgramParameterView.Count > 0)
							{
								locationProgramParameterView.Sort = "UMCB_DATE";

								DateTime LaterBeginDate = cDBConvert.ToDate(locationProgramParameterView[0]["UMCB_DATE"], DateTypes.START);
								{
									if (EvalBeginDate > LaterBeginDate)
										LaterBeginDate = EvalBeginDate;
								}

								QualRecs.RowFilter = AddToDataViewFilter(QualFilter, "mon_loc_id = '" + monLocId + "' and qual_type_cd = 'LMES'");
								QualRecs.RowFilter = AddEvaluationDateRangeToDataViewFilter(QualRecs.RowFilter, LaterBeginDate, EvalEndDate, false, true, false);

								if (QualRecs.Count == 0)
								{
									MissingQual = MissingQual.ListAdd("LMES");
									Category.SetCheckParameter("Missing_Qualification_for_Method", MissingQual, eParameterDataType.String);
								}
								else
								{
									DateTime lmesBeginDate;
									{
										DateTime unitMonitorCertBeginDate = locationProgramParameterView[0]["UMCB_DATE"].AsDateTime().Default(DateTypes.START);

										if (unitMonitorCertBeginDate > EvalBeginDate)
											lmesBeginDate = unitMonitorCertBeginDate;
										else
											lmesBeginDate = EvalBeginDate;

										DateTime ozoneBeganDate = new DateTime(lmesBeginDate.Year, 5, 1);

										if (lmesBeginDate < ozoneBeganDate)
											lmesBeginDate = ozoneBeganDate;
									}

									if (CurrentMethod["end_date"] == DBNull.Value)
									{
										if (!CheckForDateRangeCovered(Category, QualRecs, lmesBeginDate, EvalEndDate))
										{
											IncompleteQual = IncompleteQual.ListAdd("LMES");
											Category.SetCheckParameter("Incomplete_Qualification_for_Method", IncompleteQual, eParameterDataType.String);
										}
									}
									else
									{
										DateTime ozoneEndedDate = new DateTime(CurrentMethod["end_date"].AsDateTime().Default(DateTypes.END).Year - 1, 9, 30);

										if (!CheckForDateRangeCovered(Category, QualRecs, lmesBeginDate, ozoneEndedDate))
										{
											IncompleteQual = IncompleteQual.ListAdd("LMES");
											Category.SetCheckParameter("Incomplete_Qualification_for_Method", IncompleteQual, eParameterDataType.String);
										}
									}
								}
							}
						}

						if (string.IsNullOrEmpty(MissingQual) == false && string.IsNullOrEmpty(IncompleteQual))
							Category.CheckCatalogResult = "A";
						else if (string.IsNullOrEmpty(MissingQual) && !string.IsNullOrEmpty(IncompleteQual))
							Category.CheckCatalogResult = "B";
						else if (!string.IsNullOrEmpty(MissingQual) && !string.IsNullOrEmpty(IncompleteQual))
							Category.CheckCatalogResult = "C";


					}
					QualRecs.RowFilter = QualFilter;
				}
				else
					Log = false;
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "METHOD33");
			}

			return ReturnVal;
		}

		public string METHOD34(cCategory Category, ref bool Log)
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentMethod = (DataRowView)Category.GetCheckParameter("Current_Method").ParameterValue;
				bool bMethodCodeValid = (bool)Category.GetCheckParameter("Method_Method_Code_Valid").ParameterValue;
				string MethodCd = cDBConvert.ToString(CurrentMethod["Method_Cd"]);

				if (bMethodCodeValid && MethodCd.InList("AMS,PEM,SO2R"))
					Category.CheckCatalogResult = "A";
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "METHOD34");
			}

			return ReturnVal;
		}

		// Required Unit Control for MTB Method
		public string METHOD35(cCategory Category, ref bool Log)
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentMethod = (DataRowView)Category.GetCheckParameter("Current_Method").ParameterValue;
				bool bMethodCodeValid = (bool)Category.GetCheckParameter("Method_Method_Code_Valid").ParameterValue;
				bool bDatesConsistant = (bool)Category.GetCheckParameter("Method_Dates_And_Hours_Consistent").ParameterValue;
				DateTime EvalEndDate = (DateTime)Category.GetCheckParameter("Method_Evaluation_End_Date").ParameterValue;
				DateTime EvalBeginDate = (DateTime)Category.GetCheckParameter("Method_Evaluation_Begin_Date").ParameterValue;

				string MethodCd = cDBConvert.ToString(CurrentMethod["Method_Cd"]);
				string MonLocID = cDBConvert.ToString(CurrentMethod["mon_loc_id"]);

				if (bMethodCodeValid && MethodCd == "MTB" && bDatesConsistant)
				{
					DataView dvControlRecords = (DataView)Category.GetCheckParameter("Location_Control_Records").ParameterValue;
					string sControlFilter = dvControlRecords.RowFilter;
					int nCount = 0;

					string sFilter = AddToDataViewFilter(sControlFilter, String.Format("control_cd in ('WS','WL','WLS') and Install_Date is null and (Retire_Date is null or Retire_Date >= '{0}') and Orig_Ind = 1", EvalEndDate.ToShortDateString()));
					sFilter = AddToDataViewFilter(sFilter, string.Format("mon_loc_id = '{0}'", MonLocID));
					dvControlRecords.RowFilter = sFilter;

					if (dvControlRecords.Count == 0)
					{
						sFilter = AddToDataViewFilter(sControlFilter, "control_cd in ('WS','WL','WLS')");
						sFilter = AddToDataViewFilter(sFilter, string.Format("mon_loc_id = '{0}'", MonLocID));
						sFilter = AddEvaluationDateRangeToDataViewFilter(sFilter, "INSTALL_DATE", "RETIRE_DATE", EvalBeginDate, EvalEndDate, false, true, false);
						dvControlRecords.RowFilter = sFilter;

						if (dvControlRecords.Count == 0)
						{
							Category.CheckCatalogResult = "A";
						}
						else if (!CheckForDateRangeCovered(Category, dvControlRecords, "INSTALL_DATE", "RETIRE_DATE", EvalBeginDate, EvalEndDate, ref nCount))
						{
							Category.CheckCatalogResult = "B";
						}
					}
					dvControlRecords.RowFilter = sControlFilter;
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "METHOD35");
			}

			return ReturnVal;
		}

		public string METHOD36(cCategory Category, ref bool Log)
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentMethod = (DataRowView)Category.GetCheckParameter("Current_Method").ParameterValue;
				string ParameterCd = cDBConvert.ToString(CurrentMethod["Parameter_Cd"]);
				DateTime EndDate = cDBConvert.ToDate(CurrentMethod["END_DATE"], DateTypes.END);
				DateTime BeginDate = cDBConvert.ToDate(CurrentMethod["BEGIN_DATE"], DateTypes.START);
				int EndHour = cDBConvert.ToHour(CurrentMethod["END_HOUR"], DateTypes.END);
				int BeginHour = cDBConvert.ToHour(CurrentMethod["BEGIN_HOUR"], DateTypes.START);

				bool bNullEndDate = (CurrentMethod["END_DATE"] == DBNull.Value);

				DataView dvMethodRecords = (DataView)Category.GetCheckParameter("Method_Records").ParameterValue;
				string sMethodFilter = dvMethodRecords.RowFilter;

				string sFilter = AddToDataViewFilter(sMethodFilter, string.Format("PARAMETER_CD = '{0}'", ParameterCd));
				sFilter = AddToDataViewFilter(sFilter, string.Format("BEGIN_DATE = '{0}' AND BEGIN_HOUR={1}", BeginDate.ToShortDateString(), BeginHour));
				dvMethodRecords.RowFilter = sFilter;

				// we expect to find one row, the current row, is there more than 1?
				// or we are a new record and the count is equal to 1
				if ((dvMethodRecords.Count > 1) || (dvMethodRecords.Count == 1 && CurrentMethod["MON_METHOD_ID"] == DBNull.Value))
				{
					Category.CheckCatalogResult = "A";
				}
				else if (!bNullEndDate)
				{
					sFilter = AddToDataViewFilter(sMethodFilter, string.Format("PARAMETER_CD = '{0}'", ParameterCd));
					sFilter = AddToDataViewFilter(sFilter, string.Format("END_DATE = '{0}' AND END_HOUR={1}", EndDate.ToShortDateString(), EndHour));
					dvMethodRecords.RowFilter = sFilter;

					if ((dvMethodRecords.Count > 1) || (dvMethodRecords.Count == 1 && CurrentMethod["MON_METHOD_ID"] == DBNull.Value))
						Category.CheckCatalogResult = "A";
				}

				// reset this sucker
				dvMethodRecords.RowFilter = sMethodFilter;
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "METHOD36");
			}

			return ReturnVal;
		}


		public string METHOD37(cCategory Category, ref bool Log)
		{
			string ReturnVal = "";

			try
			{
				bool ConsistentDates = (bool)Category.GetCheckParameter("Method_Dates_and_Hours_Consistent").ParameterValue;
				bool ValidSubsDataCd = (bool)Category.GetCheckParameter("Method_Substitute_Data_Code_Valid").ParameterValue;
				bool ValidMethodCd = (bool)Category.GetCheckParameter("Method_Method_Code_Valid").ParameterValue;
				DataRowView CurrentMethod = (DataRowView)Category.GetCheckParameter("Current_Method").ParameterValue;

				string ParameterCode = cDBConvert.ToString(CurrentMethod["parameter_cd"]);
				string MethodCode = cDBConvert.ToString(CurrentMethod["method_cd"]);

				if (ParameterCode == "H2O" && MethodCode != "MDF" && ValidMethodCd && ValidSubsDataCd && ConsistentDates)
				{
					DataView FormulaRecords = (DataView)Category.GetCheckParameter("Formula_Records").ParameterValue;
					DateTime MethodEvalBeginDt = (DateTime)Category.GetCheckParameter("Method_Evaluation_Begin_Date").ParameterValue;
					DateTime MethodEvalEndDt = (DateTime)Category.GetCheckParameter("Method_Evaluation_End_Date").ParameterValue;
					int MethodEvalBeginHr = (int)Category.GetCheckParameter("Method_Evaluation_Begin_Hour").ParameterValue;
					int MethodEvalEndHr = (int)Category.GetCheckParameter("Method_Evaluation_End_Hour").ParameterValue;

					string SubDataCd = cDBConvert.ToString(CurrentMethod["sub_data_cd"]);

					Category.SetCheckParameter("Moisture_Default_Parameter", null, eParameterDataType.String);

					string OldFilter = FormulaRecords.RowFilter;
					FormulaRecords.RowFilter = AddToDataViewFilter(OldFilter, "equation_cd in ('19-3','19-3D','19-4','19-8')");
					FormulaRecords.RowFilter = AddEvaluationDateHourRangeToDataViewFilter(FormulaRecords.RowFilter, MethodEvalBeginDt, MethodEvalEndDt,
																   MethodEvalBeginHr, MethodEvalEndHr, false, true);

					if (FormulaRecords.Count > 0)
					{
						if (SubDataCd == "REV75")
						{
							FormulaRecords.RowFilter = AddToDataViewFilter(OldFilter, "equation_cd in ('F-2','F-14B','F-16','F-17','F-18','F-26B','19-5','19-9')");
							FormulaRecords.RowFilter = AddEvaluationDateHourRangeToDataViewFilter(FormulaRecords.RowFilter, MethodEvalBeginDt, MethodEvalEndDt,
																		   MethodEvalBeginHr, MethodEvalEndHr, false, true);
							if (FormulaRecords.Count == 0)
								Category.CheckCatalogResult = "A";
							else
								Category.SetCheckParameter("Moisture_Default_Parameter", "H2ON", eParameterDataType.String);

						}
						else if (SubDataCd == "SPTS")
							Category.SetCheckParameter("Moisture_Default_Parameter", "H2OX", eParameterDataType.String);
					}
					else
					{
						if (SubDataCd == "SPTS")
						{
							FormulaRecords.RowFilter = AddToDataViewFilter(OldFilter, "equation_cd in ('F-2','F-14B','F-16','F-17','F-18','F-26B','19-5','19-9')");
							FormulaRecords.RowFilter = AddEvaluationDateHourRangeToDataViewFilter(FormulaRecords.RowFilter, MethodEvalBeginDt, MethodEvalEndDt,
																		   MethodEvalBeginHr, MethodEvalEndHr, false, true);
							if (FormulaRecords.Count > 0)
								Category.CheckCatalogResult = "B";
							else
								Category.SetCheckParameter("Moisture_Default_Parameter", "H2OX", eParameterDataType.String);


						}
						else if (SubDataCd == "REV75")
							Category.SetCheckParameter("Moisture_Default_Parameter", "H2ON", eParameterDataType.String);
					}
					string MoistureDefaultParam = Category.GetCheckParameter("Moisture_Default_Parameter").ValueAsString();
					if (MoistureDefaultParam != "")
					{
						DataView DefaultRecords = Category.GetCheckParameter("Default_Records").ValueAsDataView();
						string DefaultFilter = DefaultRecords.RowFilter;
						DefaultRecords.RowFilter = AddToDataViewFilter(DefaultFilter, "parameter_cd = '" + MoistureDefaultParam + "'");
						DefaultRecords.RowFilter = AddEvaluationDateHourRangeToDataViewFilter(DefaultRecords.RowFilter, MethodEvalBeginDt, MethodEvalEndDt,
																							  MethodEvalBeginHr, MethodEvalEndHr, false, true);
						if (DefaultRecords.Count == 0)
						{
							Category.CheckCatalogResult = "C";
						}
						else if (!CheckForHourRangeCovered(Category, DefaultRecords, MethodEvalBeginDt, MethodEvalBeginHr, MethodEvalEndDt, MethodEvalEndHr))
							Category.CheckCatalogResult = "D";
						DefaultRecords.RowFilter = DefaultFilter;
					}

					FormulaRecords.RowFilter = OldFilter;
				}
				else
					Log = false;
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "METHOD37");
			}

			return ReturnVal;
		}

		public string METHOD38(cCategory Category, ref bool Log)
		{
			string ReturnVal = "";

			try
			{
				bool ConsistentDates = (bool)Category.GetCheckParameter("Method_Dates_and_Hours_Consistent").ParameterValue;
				bool ValidParamCd = (bool)Category.GetCheckParameter("Method_Parameter_Valid").ParameterValue;
				DataRowView CurrentMethod = (DataRowView)Category.GetCheckParameter("Current_Method").ParameterValue;

				string ParameterCode = cDBConvert.ToString(CurrentMethod["parameter_cd"]);
				string MethodCode = cDBConvert.ToString(CurrentMethod["method_cd"]);

				if (ValidParamCd && ParameterCode == "HGM" && MethodCode == "LME" && ConsistentDates)
				{
					DataView QASuppRecords = Category.GetCheckParameter("QA_Supplemental_Data_Records").ValueAsDataView();
					DateTime MethodEvalBeginDate = Category.GetCheckParameter("Method_Evaluation_Begin_Date").ValueAsDateTime(DateTypes.START);
					int MethodEvalBeginHour = Category.GetCheckParameter("Method_Evaluation_Begin_Hour").ValueAsInt();

					string sQASuppFilter = QASuppRecords.RowFilter;
					QASuppRecords.RowFilter = AddToDataViewFilter(sQASuppFilter,
						"test_type_cd = 'HGLME' and test_reason_cd = 'INITIAL' and (end_date < '" + MethodEvalBeginDate +
						"' or (end_date = '" + MethodEvalBeginDate + "' and end_hour <= '" + MethodEvalBeginHour + "'))");
					if (QASuppRecords.Count == 0)
					{
						DataView DefaultRecords = Category.GetCheckParameter("Default_Records").ValueAsDataView();
						string sDefaultFilter = DefaultRecords.RowFilter;
						DefaultRecords.RowFilter = AddToDataViewFilter(sDefaultFilter,
							"parameter_cd = 'HGC' and default_purpose_cd = 'LM' and group_id is not null and (begin_date < '" + MethodEvalBeginDate +
						"' or (begin_date = '" + MethodEvalBeginDate + "' and begin_hour <= '" + MethodEvalBeginHour + "'))");
						if (DefaultRecords.Count == 0)
						{
							if (MethodEvalBeginDate >= DateTime.Now)
								Category.CheckCatalogResult = "A";
							else
								Category.CheckCatalogResult = "B";
						}
						DefaultRecords.RowFilter = sDefaultFilter;
					}
					QASuppRecords.RowFilter = sQASuppFilter;
				}
				else
					Log = false;
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "METHOD38");
			}

			return ReturnVal;
		}

    /// <summary>
    /// Method-39: Mutually Exclusive Method Parameters
    /// </summary>
    /// <param name="category">The calling check category object.</param>
    /// <param name="log">The log messages flag.</param>
    /// <returns>Returns formatted exception information.</returns>
		public string METHOD39(cCategory category, ref bool log)
		{
			string returnVal = "";

			try
			{
        /* Initialize Output Parameers */
        MpParameters.OverlappingParameterList = null;

        /* Perform check if dates are consistent */
        if (MpParameters.MethodDatesAndHoursConsistent.Default(false))
        {
          /* Find Equivalent Parameter Codes for Current Parameter */
          CheckDataView<MethodParameterEquivalentCrosscheckRow> crosscheckView 
            = MpParameters.MethodParameterEquivalentCrosscheck.FindRows(new cFilterCondition("ParameterCode", MpParameters.CurrentMethod.ParameterCd));

          /* Perform check if equivalent parameters exist */
          if (crosscheckView.Count > 0)
          {
            /* Search for active, and concurrent or succeeding methods with the equivalent parameter */
            for (int crosscheckDex = 0; crosscheckDex < crosscheckView.Count; crosscheckDex++)
            {
              MethodParameterEquivalentCrosscheckRow crosscheckRow = crosscheckView[crosscheckDex];

              string equivalentCd = crosscheckRow.EquivalentCode;

              DateTime methodEvaluationBeginDateHour = MpParameters.MethodEvaluationBeginDate.Value.AddHours(MpParameters.MethodEvaluationBeginHour.Value);
              DateTime methodEvaluationEndDateHour = MpParameters.MethodEvaluationEndDate.Value.AddHours(MpParameters.MethodEvaluationEndHour.Value);

              CheckDataView<VwMonitorMethodRow> equivalentMethodView
                = MpParameters.MethodRecords.FindRows
                  (
                    new cFilterCondition("PARAMETER_CD", equivalentCd),
                    new cFilterCondition("BEGIN_DATEHOUR", MpParameters.CurrentMethod.BeginDatehour, eFilterDataType.DateBegan, eFilterConditionRelativeCompare.GreaterThanOrEqual),
                    new cFilterCondition("BEGIN_DATEHOUR", methodEvaluationEndDateHour, eFilterDataType.DateBegan, eFilterConditionRelativeCompare.LessThanOrEqual),
                    new cFilterCondition("END_DATEHOUR", methodEvaluationBeginDateHour, eFilterDataType.DateEnded, eFilterConditionRelativeCompare.GreaterThanOrEqual) // Handles null, on or after
                  );

              /* Append the equivalent parameter code to OverlappingParameterList if at least one method was found */
              if (equivalentMethodView.Count > 0)
              {
                MpParameters.OverlappingParameterList = MpParameters.OverlappingParameterList.ListAdd(equivalentCd);
              }
            }

            /* Return result if overlapping parameters where found */
            if (MpParameters.OverlappingParameterList != null)
            {
              MpParameters.OverlappingParameterList = MpParameters.OverlappingParameterList.FormatList();
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
        /// For a method with a substitute data code of FSP75 or FSP75C, require a fuel-specific default for each primary and secondary fuel active during the evaluation period for the method.
        /// </summary>
        /// <param name="category"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public string METHOD40(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                MpParameters.FuelsWithMissingDefaults = "";
                MpParameters.FuelsWithIncompleteDefaults = "";

                if ((MpParameters.MethodDatesAndHoursConsistent == true) && (MpParameters.CurrentMethod.SubDataCd.InList("FSP75,FSP75C")))
                {
                    /* Get primary and secondary fuels */
                    CheckDataView<VwLocationFuelRow> locationFuelRecords 
                        = MpParameters.LocationFuelRecords.FindRows(new cFilterCondition("INDICATOR_CD", "P,S", eFilterConditionStringCompare.InList),
                                                                    new cFilterCondition("BEGIN_DATE", eFilterConditionRelativeCompare.LessThanOrEqual, MpParameters.MethodEvaluationEndDate.Value, eNullDateDefault.Min),
                                                                    new cFilterCondition("END_DATE", eFilterConditionRelativeCompare.GreaterThanOrEqual, MpParameters.MethodEvaluationBeginDate.Value, eNullDateDefault.Max));

                    /* Process each primary and secondary fueld */
                    foreach (VwLocationFuelRow locationFuelRecord in locationFuelRecords)
                    {
                        /* Locate ECMPS fuel rows using the unit fuel */
                        CheckDataView<FuelCodeRow> fuelCodeLookupTable = MpParameters.FuelCodeLookupTable.FindRows(new cFilterCondition("UNIT_FUEL_CD", locationFuelRecord.FuelCd));

                        if (fuelCodeLookupTable.Count == 0)
                        {
                            MpParameters.FuelsWithMissingDefaults = MpParameters.FuelsWithMissingDefaults.ListAdd(locationFuelRecord.FuelCd);
                        }
                        else
                        {
                            MethodParameterToMaximumDefaultParameterToComponentTypeRow crosscheckRecord
                                = MpParameters.MethodParameterToMaximumDefaultParameterLookupTable.FindRow(new cFilterCondition("MethodParameterCode", MpParameters.CurrentMethod.ParameterCd),
                                                                                                           new cFilterCondition("ComponentTypeCode", null));

                            if (crosscheckRecord != null)
                            {
                                DateTime rangeBeginDateHour;
                                {
                                    rangeBeginDateHour = MpParameters.MethodEvaluationBeginDate.Value.AddHours(MpParameters.MethodEvaluationBeginHour.Value);

                                    if (locationFuelRecord.BeginDate.Default(DateTime.MinValue).AddHours(23) > rangeBeginDateHour)
                                        rangeBeginDateHour = locationFuelRecord.BeginDate.Default(DateTime.MinValue).AddHours(23);
                                }

                                DateTime rangeEndDateHour;
                                {
                                    rangeEndDateHour = MpParameters.MethodEvaluationEndDate.Value.AddHours(MpParameters.MethodEvaluationEndHour.Value);

                                    if (locationFuelRecord.EndDate.Default(DateTime.MaxValue) < rangeEndDateHour)
                                        rangeEndDateHour = locationFuelRecord.EndDate.Default(DateTime.MaxValue);
                                }

                                string defaultFuelList = cDataFunctions.ColumnToDatalist(fuelCodeLookupTable.SourceView, "FUEL_CD");

                                /* Locate default records  */
                                CheckDataView<VwMonitorDefaultRow> defaultRecords
                                    = MpParameters.DefaultRecords.FindRows(new cFilterCondition("DEFAULT_PURPOSE_CD", "MD"),
                                                                           new cFilterCondition("PARAMETER_CD", crosscheckRecord.DefaultParameterCode, eFilterConditionStringCompare.InList),
                                                                           new cFilterCondition("FUEL_CD", defaultFuelList, eFilterConditionStringCompare.InList),
                                                                           new cFilterCondition("BEGIN_DATEHOUR", eFilterConditionRelativeCompare.LessThanOrEqual, rangeEndDateHour, eNullDateDefault.Min),
                                                                           new cFilterCondition("END_DATEHOUR", eFilterConditionRelativeCompare.GreaterThanOrEqual, rangeBeginDateHour, eNullDateDefault.Max));

                                if (defaultRecords.Count == 0)
                                {
                                    MpParameters.FuelsWithMissingDefaults = MpParameters.FuelsWithMissingDefaults.ListAdd(locationFuelRecord.FuelCd);
                                }
                                else
                                {
                                    if (!CheckForHourRangeCovered(category, defaultRecords.SourceView, rangeBeginDateHour, rangeEndDateHour))
                                    {
                                        MpParameters.FuelsWithIncompleteDefaults = MpParameters.FuelsWithIncompleteDefaults.ListAdd(locationFuelRecord.FuelCd);
                                    }
                                }
                            }
                        }
                    }

                    if ((MpParameters.FuelsWithMissingDefaults != "") && (MpParameters.FuelsWithIncompleteDefaults != ""))
                    {
                        category.CheckCatalogResult = "A";
                    }
                    else if (MpParameters.FuelsWithMissingDefaults != "")
                    {
                        category.CheckCatalogResult = "B";
                    }
                    else if (MpParameters.FuelsWithIncompleteDefaults != "")
                    {
                        category.CheckCatalogResult = "C";
                    }

                    MpParameters.FuelsWithMissingDefaults = MpParameters.FuelsWithMissingDefaults.FormatList();
                    MpParameters.FuelsWithIncompleteDefaults = MpParameters.FuelsWithIncompleteDefaults.FormatList();
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
