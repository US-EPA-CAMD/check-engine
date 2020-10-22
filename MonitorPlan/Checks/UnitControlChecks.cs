using System;
using System.Text;
using System.Data;
using System.Collections.Generic;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Data.Ecmps.Dbo.View;
using ECMPS.Checks.Mp.Parameters;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;
using ECMPS.Definitions.Extensions;

namespace ECMPS.Checks.UnitControlChecks
{
	public class cUnitControlChecks : cChecks
	{
		#region Constructors

		public cUnitControlChecks()
		{
			CheckProcedures = new dCheckProcedure[17];
			CheckProcedures[01] = new dCheckProcedure(CONTROL1);
			CheckProcedures[02] = new dCheckProcedure(CONTROL2);
			CheckProcedures[04] = new dCheckProcedure(CONTROL4);
			CheckProcedures[05] = new dCheckProcedure(CONTROL5);
			CheckProcedures[06] = new dCheckProcedure(CONTROL6);
			CheckProcedures[08] = new dCheckProcedure(CONTROL8);
			CheckProcedures[09] = new dCheckProcedure(CONTROL9);
			CheckProcedures[11] = new dCheckProcedure(CONTROL11);
			CheckProcedures[13] = new dCheckProcedure(CONTROL13);
			CheckProcedures[14] = new dCheckProcedure(CONTROL14);
			CheckProcedures[15] = new dCheckProcedure(CONTROL15);
			CheckProcedures[16] = new dCheckProcedure(CONTROL16);
		}

		#endregion

		public static string CONTROL1(cCategory Category, ref bool Log)
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentRecord = (DataRowView)Category.GetCheckParameter("Current_Control").ParameterValue;
				if (CurrentRecord["CE_PARAM"] == DBNull.Value)
					Category.CheckCatalogResult = "A";
				else
				{
					DataView dvControlCodeLookup = (DataView)Category.GetCheckParameter("Control_Code_Lookup_Table").ParameterValue;
					sFilterPair[] criteria = new sFilterPair[1];
					criteria[0].Set("CE_PARAM", (string)CurrentRecord["CE_PARAM"]);

					if (CountRows(dvControlCodeLookup, criteria) == 0)
						Category.CheckCatalogResult = "B";
				}

				bool bValid = false;
				if (string.IsNullOrEmpty(Category.CheckCatalogResult))
					bValid = true;

				Category.SetCheckParameter("Control_Parameter_Code_Valid", bValid, eParameterDataType.Boolean);
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "CONTROL1");
			}

			return ReturnVal;
		}

		public static string CONTROL2(cCategory Category, ref bool Log)
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentRecord = (DataRowView)Category.GetCheckParameter("Current_Control").ParameterValue;
				if (CurrentRecord["CONTROL_CD"] == DBNull.Value)
				{
					Category.CheckCatalogResult = "A";
				}
				else
				{
					DataView dvControlCodeLookup = (DataView)Category.GetCheckParameter("Control_Code_Lookup_Table").ParameterValue;
					sFilterPair[] criteria = new sFilterPair[2];
					criteria[0].Set("CE_PARAM", cDBConvert.ToString(CurrentRecord["CE_PARAM"]));
					criteria[1].Set("CONTROL_CD", cDBConvert.ToString(CurrentRecord["CONTROL_CD"]));

					if (CountRows(dvControlCodeLookup, criteria) == 0)
					{
						bool bParamValid = (bool)Category.GetCheckParameter("Control_Parameter_Code_Valid").ParameterValue;

						criteria = new sFilterPair[1];
						criteria[0].Set("CONTROL_CD", (string)CurrentRecord["CONTROL_CD"]);
						if (CountRows(dvControlCodeLookup, criteria) == 0)
							Category.CheckCatalogResult = "B";
						else if (bParamValid)
							Category.CheckCatalogResult = "C";
					}
				}

				bool bValid = false;
				if (string.IsNullOrEmpty(Category.CheckCatalogResult))
					bValid = true;

				Category.SetCheckParameter("Control_Code_Valid", bValid, eParameterDataType.Boolean);
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "CONTROL2");
			}

			return ReturnVal;
		}

		public static string CONTROL4(cCategory Category, ref bool Log)
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentRecord = (DataRowView)Category.GetCheckParameter("Current_Control").ParameterValue;
				if (CurrentRecord["OPT_DATE"] != DBNull.Value)
				{
					DateTime OPT_DATE = (DateTime)CurrentRecord["OPT_DATE"];
					bool bRetireDateValid = (bool)Category.GetCheckParameter("Control_Retire_Date_Valid").ParameterValue;
					bool bInstallDateValid = (bool)Category.GetCheckParameter("Control_Install_Date_Valid").ParameterValue;

					bool bRetireCheck = bRetireDateValid && CurrentRecord["RETIRE_DATE"] != DBNull.Value && DateTime.Compare(OPT_DATE, (DateTime)CurrentRecord["RETIRE_DATE"]) > 0;
					bool bInstallCheck = bInstallDateValid && CurrentRecord["INSTALL_DATE"] != DBNull.Value && DateTime.Compare(OPT_DATE, (DateTime)CurrentRecord["INSTALL_DATE"]) < 0;
					if (bInstallCheck || bRetireCheck)
						Category.CheckCatalogResult = "A";
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "CONTROL4");
			}

			return ReturnVal;
		}

		public static string CONTROL5(cCategory Category, ref bool Log)
		{
			string ReturnVal = "";

			try
			{
				Category.SetCheckParameter("Control_Install_Date_Valid", true, eParameterDataType.Boolean);
				DataRowView CurrentControl = (DataRowView)Category.GetCheckParameter("Current_Control").ParameterValue;
				DateTime BeginDate = cDBConvert.ToDate(CurrentControl["INSTALL_DATE"], DateTypes.START);
				int OrigInd = cDBConvert.ToInteger(CurrentControl["ORIG_IND"]);
				if (BeginDate == DateTime.MinValue)
				{
					if (OrigInd != 1)
					{
						Category.SetCheckParameter("Control_Install_Date_Valid", false, eParameterDataType.Boolean);
						Category.CheckCatalogResult = "A";
					}
				}
				else
				{
					if (BeginDate < new DateTime(1930, 1, 1) || Category.Process.MaximumFutureDate < BeginDate)
						Category.CheckCatalogResult = "B";
					else
					{
						DataRowView CurrentUnit = (DataRowView)Category.GetCheckParameter("Current_Unit").ParameterValue;
						DateTime CommOpDt = cDBConvert.ToDate(CurrentUnit["COMM_OP_DATE"], DateTypes.END);
						DateTime ComrOpDt = cDBConvert.ToDate(CurrentUnit["COMR_OP_DATE"], DateTypes.END);
						if ((CommOpDt != DateTime.MaxValue || ComrOpDt != DateTime.MaxValue) &&
							BeginDate < CommOpDt && BeginDate < ComrOpDt)
						{
							Category.SetCheckParameter("Control_Install_Date_Valid", true, eParameterDataType.Boolean);
							Category.CheckCatalogResult = "C";
						}
						else
							if (OrigInd == 1)
								Category.CheckCatalogResult = "D";
					}
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "CONTROL5");
			}

			return ReturnVal;
		}

		public static string CONTROL6(cCategory Category, ref bool Log)
		{
			string ReturnVal = "";

			try
			{
				return Check_ValidEndDate(Category, "Control_Retire_Date_Valid", "Current_Control", "RETIRE_DATE", "A", "CONTROL6");
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "CONTROL6");
			}

			return ReturnVal;
		}

		public static string CONTROL8(cCategory Category, ref bool Log)
		{
			string ReturnVal = "";

			try
			{
				bool bDatesConsistent = (bool)Category.GetCheckParameter("Control_Install_Date_Consistent_With_Retire_Date").ParameterValue;
				if (bDatesConsistent)
				{
					ReturnVal = Check_ActiveDateRange(Category, "Control_Active_Status", "Current_Control", "Control_Evaluation_Start_Date",
													   "Control_Evaluation_End_Date", "INSTALL_DATE", "RETIRE_DATE");
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "CONTROL8");
			}

			return ReturnVal;
		}

		public static string CONTROL9(cCategory Category, ref bool Log)
		{
			string ReturnVal = "";

			try
			{
				return Check_ConsistentDateRange(Category, "Control_Install_Date_Consistent_With_Retire_Date", "Current_Control",
												  "Control_Install_Date_Valid", "Control_Retire_Date_Valid", "INSTALL_DATE", "RETIRE_DATE");
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "CONTROL9");
			}

			return ReturnVal;
		}

		public static string CONTROL11(cCategory Category, ref bool Log)
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentRecord = (DataRowView)Category.GetCheckParameter("Current_Control").ParameterValue;
				bool bCtlCdValid = (bool)Category.GetCheckParameter("Control_Code_Valid").ParameterValue;
				bool bParamValid = (bool)Category.GetCheckParameter("Control_Parameter_Code_Valid").ParameterValue;

				if (bCtlCdValid && bParamValid)
				{
					DataView dvUnitTypeRecords = (DataView)Category.GetCheckParameter("Location_Unit_Type_Records").ParameterValue;
					DataView dvCtl2UnitType = (DataView)Category.GetCheckParameter("Control_to_Unit_Type_Cross_Check_Table").ParameterValue;

					string sUTFilter = dvUnitTypeRecords.RowFilter;

					DateTime dtEvalBeginDate = (DateTime)Category.GetCheckParameter("Control_Evaluation_Start_Date").ParameterValue;
					DateTime dtEvalEndedDate = (DateTime)Category.GetCheckParameter("Control_Evaluation_End_Date").ParameterValue;

					sFilterPair[] criteria = new sFilterPair[1];
					criteria[0].Set("ControlCode", (string)CurrentRecord["CONTROL_CD"]);
					if (CountRows(dvCtl2UnitType, criteria) > 0)
					{
						criteria = new sFilterPair[2];
						criteria[0].Set("ControlCode", (string)CurrentRecord["CONTROL_CD"]);

						dvUnitTypeRecords.RowFilter = AddEvaluationDateRangeToDataViewFilter(sUTFilter, dtEvalBeginDate, dtEvalEndedDate, false, true, false);
						foreach (DataRowView row in dvUnitTypeRecords)
						{
							string UnitTypeCd = cDBConvert.ToString(row["UNIT_TYPE_CD"]);

							criteria[1].Set("UnitTypeCode", UnitTypeCd);

							if (CountRows(dvCtl2UnitType, criteria) == 0)
							{   // not valid, bail now
								Category.SetCheckParameter("Invalid_Unit_Type_for_Control", UnitTypeCd, eParameterDataType.String);
								Category.CheckCatalogResult = "A";
								break;
							}
						}
					}

					// reset this sucker
					dvUnitTypeRecords.RowFilter = sUTFilter;
				}

			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "CONTROL11");
			}

			return ReturnVal;
		}

		public static string CONTROL13(cCategory Category, ref bool Log)
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentRecord = (DataRowView)Category.GetCheckParameter("Current_Control").ParameterValue;
				bool bCtlCdValid = (bool)Category.GetCheckParameter("Control_Code_Valid").ParameterValue;
				bool bParamValid = (bool)Category.GetCheckParameter("Control_Parameter_Code_Valid").ParameterValue;
				bool bDatesConsistent = (bool)Category.GetCheckParameter("Control_Install_Date_Consistent_With_Retire_Date").ParameterValue;

				if (bCtlCdValid && bParamValid && bDatesConsistent)
				{
					DataView dvControlRecords = (DataView)Category.GetCheckParameter("Control_Records").ParameterValue;
					string sControlFilter = dvControlRecords.RowFilter;

					DateTime dtEvalBeginDate = (DateTime)Category.GetCheckParameter("Control_Evaluation_Start_Date").ParameterValue;
					DateTime dtEvalEndedDate = (DateTime)Category.GetCheckParameter("Control_Evaluation_End_Date").ParameterValue;
					DateTime dtInstallDate = cDBConvert.ToDate(CurrentRecord["INSTALL_DATE"], DateTypes.START);

					// make sure we filter out the STACK's control records - VW_LOCATION_CONTROL is very screwy!
					string sFilter = string.Format("UNIT_ID={0} and LOCATION_IDENTIFIER = UNITID and CE_PARAM='{1}' and CONTROL_CD='{2}' ", CurrentRecord["UNIT_ID"].ToString(), CurrentRecord["CE_PARAM"].ToString(), CurrentRecord["CONTROL_CD"].ToString());
					sFilter += string.Format("AND INSTALL_DATE >= '{0}' and INSTALL_DATE <= '{1}' ", dtInstallDate.ToShortDateString(), dtEvalEndedDate.ToShortDateString());
					sFilter += string.Format("AND ( RETIRE_DATE IS NULL OR RETIRE_DATE >= '{0}' )", dtEvalBeginDate.ToShortDateString());
					dvControlRecords.RowFilter = sFilter;
					if (dvControlRecords.Count > 1 || (dvControlRecords.Count == 1 && dvControlRecords[0]["CTL_ID"] != CurrentRecord["CTL_ID"]))
						Category.CheckCatalogResult = "A";

					dvControlRecords.RowFilter = sControlFilter;
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "CONTROL13");
			}

			return ReturnVal;
		}

		public static string CONTROL14(cCategory Category, ref bool Log)
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentRecord = (DataRowView)Category.GetCheckParameter("Current_Control").ParameterValue;
				if (CurrentRecord["CE_PARAM"] != DBNull.Value)
				{
					string CE_PARAM = (string)CurrentRecord["CE_PARAM"];
					if (CE_PARAM.InList("SO2,PART") == true)
					{
						string sUnit_ID = CurrentRecord["UNIT_ID"].ToString();
						string sLocationID = CurrentRecord["LOCATION_IDENTIFIER"].ToString();

						DateTime dtEvalBeginDate = (DateTime)Category.GetCheckParameter("Control_Evaluation_Start_Date").ParameterValue;
						DateTime dtEvalEndedDate = (DateTime)Category.GetCheckParameter("Control_Evaluation_End_Date").ParameterValue;

						DataView dvFuelRecords = (DataView)Category.GetCheckParameter("Location_Fuel_Records").ParameterValue;
						string sFuelFilter = dvFuelRecords.RowFilter;

						string sFilter = string.Format("LOCATION_IDENTIFIER='{1}' and FUEL_GROUP_CD='GAS'", sUnit_ID, sLocationID);
						sFilter = AddEvaluationDateRangeToDataViewFilter(sFilter, dtEvalBeginDate, dtEvalEndedDate, false, true, false);
						dvFuelRecords.RowFilter = sFilter;
						if (dvFuelRecords.Count > 0)
						{
							sFilter = string.Format("UNIT_ID={0} and LOCATION_IDENTIFIER='{1}' and FUEL_GROUP_CD<>'GAS'", sUnit_ID, sLocationID);
							sFilter = AddEvaluationDateRangeToDataViewFilter(sFilter, dtEvalBeginDate, dtEvalEndedDate, false, true, false);
							dvFuelRecords.RowFilter = sFilter;
							if (dvFuelRecords.Count == 0)
								Category.CheckCatalogResult = "A";
						}

						dvFuelRecords.RowFilter = sFuelFilter;
					}
				}
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "CONTROL14");
			}

			return ReturnVal;
		}

		public static string CONTROL15(cCategory Category, ref bool Log)
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentRecord = (DataRowView)Category.GetCheckParameter("Current_Control").ParameterValue;

				DataView dvControlRecords = (DataView)Category.GetCheckParameter("Control_Records").ParameterValue;
				sFilterPair[] criteria1 = new sFilterPair[4];
				criteria1[0].Set("UNIT_ID", CurrentRecord["UNIT_ID"], eFilterDataType.Integer);
				criteria1[1].Set("CE_PARAM", CurrentRecord["CE_PARAM"].ToString());
				criteria1[2].Set("CONTROL_CD", CurrentRecord["CONTROL_CD"].ToString());
				criteria1[3].Set("INSTALL_DATE", CurrentRecord["INSTALL_DATE"], eFilterDataType.DateBegan);
				sFilterPair[] criteria2 = new sFilterPair[4];
				criteria1.CopyTo(criteria2, 0);
				criteria2[3].Set("RETIRE_DATE", CurrentRecord["RETIRE_DATE"], eFilterDataType.DateEnded);

				if (CurrentRecord["RETIRE_DATE"] == DBNull.Value)
				{
					if (DuplicateRecordCheck(CurrentRecord, dvControlRecords, "CTL_ID", criteria1))
						Category.CheckCatalogResult = "A";
				}
				else if (DuplicateRecordCheck(CurrentRecord, dvControlRecords, "CTL_ID", criteria1, criteria2))
					Category.CheckCatalogResult = "A";
			}
			catch (Exception ex)
			{
				ReturnVal = Category.CheckEngine.FormatError(ex, "CONTROL15");
			}

			return ReturnVal;
		}
		public static string CONTROL16(cCategory category, ref bool log)
		// Control Equipment Dual Range Analyzer Check
		{
			string returnValue = "";

			try
			{
				if (MpParameters.CurrentControl != null)
				{
					//don't get dates that precede facility ECMPS usage
					DateTime controlEvalStartDate;
					if (MpParameters.ControlEvaluationStartDate.Value < MpParameters.EcmpsMpBeginDate)
					{
						controlEvalStartDate = MpParameters.EcmpsMpBeginDate.Value;
					}
					else
					{
						controlEvalStartDate = MpParameters.ControlEvaluationStartDate.Value;
					}

					DataView uscRecords = cRowFilter.FindActiveRows(MpParameters.UnitStackConfigurationRecords.SourceView,
										controlEvalStartDate,
										MpParameters.ControlEvaluationEndDate.Value,
										new cFilterCondition[] 
                                        { 
                                          new cFilterCondition("Mon_Loc_Id", MpParameters.CurrentLocation.MonLocId)
                                        });

					DataView componentRecords = new DataView();
					DataView coveredRecords = new DataView();

					if (MpParameters.CurrentControl.ControlCd.InList("SCR,SNCR"))
					{
						componentRecords = cRowFilter.FindActiveRows(MpParameters.SystemComponentRecords.SourceView,
											controlEvalStartDate, MpParameters.ControlEvaluationEndDate.Value,
											 new cFilterCondition[] 
											{
												new cFilterCondition("COMPONENT_TYPE_CD", "NOX", eFilterConditionStringCompare.Equals),
											});

						if (componentRecords == null || componentRecords.Count == 0 || !cChecks.CheckForRangeCoveredUnitStackConfig(MpParameters.CurrentLocation.MonLocId, componentRecords, uscRecords,
							controlEvalStartDate, MpParameters.ControlEvaluationEndDate.Value,
							out coveredRecords))
						{
							category.CheckCatalogResult = "A";
						}


						//check the components we like for detail
						foreach (DataRowView componentRow in coveredRecords)
						{

							DataView analyzerRangeRecords = cRowFilter.FindActiveRows(MpParameters.AnalyzerRangeRecords.SourceView,
															componentRow["BEGIN_DATE"].AsDateTime().Default(controlEvalStartDate),
															componentRow["BEGIN_HOUR"].AsInteger().Default(0),
															componentRow["END_DATE"].AsDateTime().Default(MpParameters.ControlEvaluationEndDate.Value),
															componentRow["END_HOUR"].AsInteger().Default(23),
											new cFilterCondition[] 
							                    {
							                    new cFilterCondition("COMPONENT_ID", componentRow["COMPONENT_ID"].ToString(), eFilterConditionStringCompare.Equals),
							                    new cFilterCondition("DUAL_RANGE_IND", "1", eFilterConditionStringCompare.Equals)
							                    });

							if (analyzerRangeRecords.Count == 0 || !CheckForHourRangeCovered(analyzerRangeRecords,
															componentRow["BEGIN_DATE"].AsDateTime().Default(controlEvalStartDate),
															componentRow["BEGIN_HOUR"].AsInteger().Default(0),
															componentRow["END_DATE"].AsDateTime().Default(MpParameters.ControlEvaluationEndDate.Value),
															componentRow["END_HOUR"].AsInteger().Default(23)))
							{
								DataView spanRecords = cRowFilter.FindActiveRows(MpParameters.SpanRecords.SourceView,
															componentRow["BEGIN_DATE"].AsDateTime().Default(controlEvalStartDate),
															componentRow["BEGIN_HOUR"].AsInteger().Default(0),
															componentRow["END_DATE"].AsDateTime().Default(MpParameters.ControlEvaluationEndDate.Value),
															componentRow["END_HOUR"].AsInteger().Default(23),

												new cFilterCondition[] 
							                       {
							                         new cFilterCondition("COMPONENT_TYPE_CD", "NOX", eFilterConditionStringCompare.Equals),
							                         new cFilterCondition("SPAN_SCALE_CD", "H", eFilterConditionStringCompare.Equals),
							                         new cFilterCondition("DEFAULT_HIGH_RANGE", string.Empty, eFilterConditionStringCompare.Equals, true)

							                       });

								// range covered by *either* span or analyzer range records
								if (spanRecords == null || spanRecords.Count == 0)
								{
									category.CheckCatalogResult = "A";
								}
								else
								{
									if (analyzerRangeRecords.Count == 0)
									//only need to check the spans
									{
										if (!CheckForRangeCoveredUnitStackConfig(MpParameters.CurrentLocation.MonLocId, spanRecords, uscRecords,
															componentRow["BEGIN_DATE"].AsDateTime().Default(controlEvalStartDate),
															componentRow["END_DATE"].AsDateTime().Default(MpParameters.ControlEvaluationEndDate.Value).AddHours(componentRow["END_HOUR"].AsInteger().Default(23)),
															out coveredRecords))
										{
											category.CheckCatalogResult = "A";
										}
									}
									else
									{
										//combine the span records with any analyzer range records for span evaluation

										DataView combinedDetailRecords = Control16_CombineLocationViews(spanRecords, analyzerRangeRecords);

										if (!CheckForRangeCoveredUnitStackConfig(MpParameters.CurrentLocation.MonLocId, combinedDetailRecords, uscRecords,
															componentRow["BEGIN_DATE"].AsDateTime().Default(controlEvalStartDate).AddHours(componentRow["BEGIN_HOUR"].AsInteger().Default(0)),
															componentRow["END_DATE"].AsDateTime().Default(MpParameters.ControlEvaluationEndDate.Value).AddHours(componentRow["END_HOUR"].AsInteger().Default(23)),
															out coveredRecords))
										{
											category.CheckCatalogResult = "A";
										}
									}
								}
							}
						}

					} //end Control SCR

					if (MpParameters.CurrentControl.ControlCd.InList("DA,DL,MO,SB,WL,WLS"))
					{
						componentRecords = cRowFilter.FindActiveRows(MpParameters.SystemComponentRecords.SourceView,
											controlEvalStartDate, MpParameters.ControlEvaluationEndDate.Value,
											 new cFilterCondition[] 
											{
												new cFilterCondition("COMPONENT_TYPE_CD", "SO2", eFilterConditionStringCompare.Equals),
											});

						if (componentRecords == null || componentRecords.Count == 0 || !cChecks.CheckForRangeCoveredUnitStackConfig(MpParameters.CurrentLocation.MonLocId, componentRecords, uscRecords,
							controlEvalStartDate, MpParameters.ControlEvaluationEndDate.Value,
							out coveredRecords))
						{
							category.CheckCatalogResult = "A";
						}

						//check the components we like for detail
						foreach (DataRowView componentRow in coveredRecords)
						{
							//DateTime rangeBegin = componentRow["BEGIN_DATE"].AsDateTime().Default(MpParameters.ControlEvaluationStartDate.Value);
							//DateTime rangeEnd = componentRow["END_DATE"].AsDateTime().Default(MpParameters.ControlEvaluationEndDate.Value);

							DataView analyzerRangeRecords = cRowFilter.FindActiveRows(MpParameters.AnalyzerRangeRecords.SourceView,
															componentRow["BEGIN_DATE"].AsDateTime().Default(controlEvalStartDate),
															componentRow["BEGIN_HOUR"].AsInteger().Default(0),
															componentRow["END_DATE"].AsDateTime().Default(MpParameters.ControlEvaluationEndDate.Value),
															componentRow["END_HOUR"].AsInteger().Default(23),
													new cFilterCondition[] 
											        {
														new cFilterCondition("COMPONENT_ID", componentRow["COMPONENT_ID"].ToString(), eFilterConditionStringCompare.Equals),
														new cFilterCondition("DUAL_RANGE_IND", "1", eFilterConditionStringCompare.Equals)
													});

							if (analyzerRangeRecords.Count == 0 || !CheckForHourRangeCovered(analyzerRangeRecords,
															componentRow["BEGIN_DATE"].AsDateTime().Default(controlEvalStartDate),
															componentRow["BEGIN_HOUR"].AsInteger().Default(0),
															componentRow["END_DATE"].AsDateTime().Default(MpParameters.ControlEvaluationEndDate.Value),
															componentRow["END_HOUR"].AsInteger().Default(23)))
							{
								DataView spanRecords = cRowFilter.FindActiveRows(MpParameters.SpanRecords.SourceView,
															componentRow["BEGIN_DATE"].AsDateTime().Default(controlEvalStartDate),
															componentRow["BEGIN_HOUR"].AsInteger().Default(0),
															componentRow["END_DATE"].AsDateTime().Default(MpParameters.ControlEvaluationEndDate.Value),
															componentRow["END_HOUR"].AsInteger().Default(23),
												new cFilterCondition[] 
			                                    {
													new cFilterCondition("COMPONENT_TYPE_CD", "SO2", eFilterConditionStringCompare.Equals),
													new cFilterCondition("SPAN_SCALE_CD", "H", eFilterConditionStringCompare.Equals),
 													new cFilterCondition("DEFAULT_HIGH_RANGE", string.Empty, eFilterConditionStringCompare.Equals, true)
												});

								// range covered by *either* span or analyzer range records
								if (spanRecords == null || spanRecords.Count == 0)
								{
									category.CheckCatalogResult = "A";
								}
								else
								{
									if (analyzerRangeRecords.Count == 0)
									//only need to check the spans
									{
										if (!CheckForRangeCoveredUnitStackConfig(MpParameters.CurrentLocation.MonLocId, spanRecords, uscRecords,
															componentRow["BEGIN_DATE"].AsDateTime().Default(controlEvalStartDate).AddHours(componentRow["BEGIN_HOUR"].AsInteger().Default(0)),
															componentRow["END_DATE"].AsDateTime().Default(MpParameters.ControlEvaluationEndDate.Value).AddHours(componentRow["END_HOUR"].AsInteger().Default(23)),
														out coveredRecords))
										{
											category.CheckCatalogResult = "A";
										}
									}
									else
									{
										//combine the span records with any analyzer range records for span evaluation

										DataView combinedDetailRecords = Control16_CombineLocationViews(spanRecords, analyzerRangeRecords);

										if (!CheckForRangeCoveredUnitStackConfig(MpParameters.CurrentLocation.MonLocId, combinedDetailRecords, uscRecords,
																								controlEvalStartDate, MpParameters.ControlEvaluationEndDate.Value,
																								out coveredRecords))
										{
											category.CheckCatalogResult = "A";
										}
									}
								}

							}
						}
					} //end Control "DA","DL","MO","SB","WL","WS"

				} //end CurrentControl
				else
					log = false;
			} //end try
			catch (Exception ex)
			{
				returnValue = category.CheckEngine.FormatError(ex, "CONTROL16");
			}
			return returnValue;
		}

		private static DataView Control16_CombineLocationViews(DataView View1, DataView View2)
		{
			DataView dv = new DataView();
			DataTable dt = new DataTable();
			dt.Columns.Add("MON_LOC_ID", typeof(string));
			dt.Columns.Add("BEGIN_DATE", typeof(DateTime));
			dt.Columns.Add("BEGIN_HOUR", typeof(int));
			dt.Columns.Add("END_DATE", typeof(DateTime));
			dt.Columns.Add("END_HOUR", typeof(int));

			foreach (DataRowView row in View1)
			{
				DataRow dr = dt.NewRow();
				dr.ItemArray[0] = row["MON_LOC_ID"].ToString();
				dr.ItemArray[1] = row["BEGIN_DATE"].AsDateTime();
				dr.ItemArray[2] = row["BEGIN_HOUR"].AsInteger();
				dr.ItemArray[3] = row["END_DATE"].AsDateTime();
				dr.ItemArray[4] = row["END_HOUR"].AsInteger();
				dt.Rows.Add(dr);

			}
			foreach (DataRowView row in View2)
			{
				DataRow dr = dt.NewRow();
				dr.ItemArray[0] = row["MON_LOC_ID"].ToString();
				dr.ItemArray[1] = row["BEGIN_DATE"].AsDateTime();
				dr.ItemArray[2] = row["BEGIN_HOUR"].AsInteger();
				dr.ItemArray[3] = row["END_DATE"].AsDateTime();
				dr.ItemArray[4] = row["END_HOUR"].AsInteger();
				dt.Rows.Add(dr);
			}
			dv.Table = dt;

			return dv;
		}
		/*
		public static string CONTROL_template( cCategory Category, ref bool Log )
		{
			string ReturnVal = "";

			try
			{
				DataRowView CurrentRecord = (DataRowView)Category.GetCheckParameter( "Current_Control" ).ParameterValue;
			}
			catch( Exception ex )
			{
				ReturnVal = Category.CheckEngine.FormatError( ex, "CONTROL_template" );
			}

			return ReturnVal;
		}
		*/

	}
}
