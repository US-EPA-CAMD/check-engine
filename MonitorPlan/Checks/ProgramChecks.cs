using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Data.Ecmps.Dbo.View;
using ECMPS.Checks.MonitorPlan;
using ECMPS.Checks.Mp.Parameters;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Enumerations;
using ECMPS.Definitions.Extensions;

namespace ECMPS.Checks.ProgramChecks
{

	public class cProgramChecks : cMpChecks
	{

		public cProgramChecks(cMpProcess mpProcess)
			: base(mpProcess)
		{
			CheckProcedures = new dCheckProcedure[15];

			CheckProcedures[1] = new dCheckProcedure(PROGRAM1);
			CheckProcedures[10] = new dCheckProcedure(PROGRAM10);
			CheckProcedures[11] = new dCheckProcedure(PROGRAM11);
            CheckProcedures[12] = new dCheckProcedure(PROGRAM12);
            CheckProcedures[13] = new dCheckProcedure(PROGRAM13);
            CheckProcedures[14] = new dCheckProcedure(PROGRAM14);
        }

		/// <summary>
		/// Constructor used for testing.
		/// </summary>
		/// <param name="mpManualParameters"></param>
		public cProgramChecks(cMpCheckParameters mpManualParameters)
		{
			MpManualParameters = mpManualParameters;
		}

		/// <summary>
		/// PROGRAM-1: Program Active Status
		/// </summary>
		/// <param name="category">The calling check category object.</param>
		/// <param name="log">The log messages flag.</param>
		/// <returns>Returns formatted exception information.</returns>
		public string PROGRAM1(cCategory category, ref bool log)
		{
			string returnVal = "";

			try
			{
				DateTime evaluationBeganDate = MpParameters.EvaluationBeginDate.Value;
				DateTime evaluationEndedDate = MpParameters.EvaluationEndDate.Value;

				DataRowView currentProgram = (DataRowView)category.GetCheckParameter("Current_Program").ParameterValue;
				DataRowView currentMonitoringPlanConfiguration = category.GetCheckParameter("Current_Monitoring_Plan_Configuration").AsDataRowView();

				DateTime? umcbDate = currentProgram["UNIT_MONITOR_CERT_BEGIN_DATE"].AsDateTime();
				DateTime? erbDate = currentProgram["EMISSIONS_RECORDING_BEGIN_DATE"].AsDateTime();
				DateTime? endDate = currentProgram["END_DATE"].AsDateTime();
				int? endRptPeriodId = currentMonitoringPlanConfiguration["END_RPT_PERIOD_ID"].AsInteger();


				if (umcbDate.HasValue)
				{
                    MpParameters.CurrentProgramActive = true;

					if ((umcbDate.Value > category.CheckEngine.NowDate) ||
						(endDate.Default(DateTime.MaxValue) < evaluationBeganDate) ||
						(endRptPeriodId.HasValue &&
						  (cReportingPeriod.GetReportingPeriod(endRptPeriodId.Value).EndedDate < umcbDate.Value)))
					{
                        MpParameters.CurrentProgramActive = false;
                    }

                    /* Special logic for MATS odd opt in */
                    else if ((MpParameters.CurrentProgram.PrgCd == "MATS") && (MpParameters.MatsEvaluationBeginDate != null))
                    {
                        /* Inactive if program end date is on or before MATS evaluation begin date */
                        if (endDate.HasValue && (endDate <= MpParameters.MatsEvaluationBeginDate))
                        {
                            MpParameters.CurrentProgramActive = false;
                        }

                        /* Inactive if evaluation end date is on or before MATS evaluation begin date */
                        else if (MpParameters.EvaluationEndDate <= MpParameters.MatsEvaluationBeginDate)
                        {
                            MpParameters.CurrentProgramActive = false;
                        }
                    }


                    if (MpParameters.CurrentProgramActive == true)
					{
						// Set Program Evaluation Begin Date
						if (erbDate.HasValue && (erbDate.Value > evaluationBeganDate))
						{
							category.SetCheckParameter("Program_Evaluation_Begin_Date", erbDate.Value, eParameterDataType.Date);
						}
						else if (umcbDate.Value.AddDays(180) > evaluationBeganDate)
						{
							category.SetCheckParameter("Program_Evaluation_Begin_Date", umcbDate.Value.AddDays(180), eParameterDataType.Date);
						}
						else
						{
							category.SetCheckParameter("Program_Evaluation_Begin_Date", evaluationBeganDate, eParameterDataType.Date);
						}

						if (MpParameters.CurrentProgram.PrgCd == "MATS" && MpParameters.MatsRequiredCheck == true && MpParameters.MatsEvaluationBeginDate > MpParameters.ProgramEvaluationBeginDate)
						{
							MpParameters.ProgramEvaluationBeginDate = MpParameters.MatsEvaluationBeginDate;
						}

						// Set Program Evaluation End Date
						if (!endDate.HasValue || (endDate.Value > evaluationEndedDate))
						{
							category.SetCheckParameter("Program_Evaluation_End_Date", evaluationEndedDate, eParameterDataType.Date);
						}
						else
						{
							category.SetCheckParameter("Program_Evaluation_End_Date", endDate.Value, eParameterDataType.Date);
						}

						// Check for Retired Operating Status
						DataRowView unitOperatingStatusRetiredRow
						  = cRowFilter.FindRow(category.GetCheckParameter("Unit_Operating_Status_Records").AsDataView(),
												new cFilterCondition[] 
                                  { 
                                    new cFilterCondition("OP_STATUS_CD", "RET"),
                                    new cFilterCondition("END_DATE", "")
                                  });

						if ((unitOperatingStatusRetiredRow != null) &&
							(unitOperatingStatusRetiredRow["BEGIN_DATE"].AsDateTime().Value
							   <= category.GetCheckParameter("Program_Evaluation_End_Date").AsDateTime().Value))
						{
							category.SetCheckParameter("Program_Evaluation_End_Date",
													   unitOperatingStatusRetiredRow["BEGIN_DATE"].AsDateTime().Value.AddDays(-1),
													   eParameterDataType.Date);
						}
					}
				}
				else
				{
					category.SetCheckParameter("Current_Program_Active", false, eParameterDataType.Boolean);
				}
			}
			catch (Exception ex)
			{
				returnVal = category.CheckEngine.FormatError(ex);
			}

			return returnVal;
		}


		/// <summary>
		/// PROGRAM-10: Program Parameter Active Status
		/// </summary>
		/// <param name="category">The calling check category object.</param>
		/// <param name="log">The log messages flag.</param>
		/// <returns>Returns formatted exception information.</returns>
		public string PROGRAM10(cCategory category, ref bool log)
		{
			string returnVal = "";

			try
			{
        MpParameters.CurrentProgramParameterActive = true;
        MpParameters.ProgramParameterEvaluationBeginDate = null;
        MpParameters.ProgramParameterEvaluationEndDate = null;

        /* Inactive if current program is inactive */
        if (MpParameters.CurrentProgramActive.Default(false) == false)
        {
          MpParameters.CurrentProgramParameterActive = false;
        }
        
        /* Inactive if parameter begin date is on or after program evaluation end date */
        else if (MpParameters.CurrentProgramParameter.ParamBeginDate >= MpParameters.ProgramEvaluationEndDate)
        {
          MpParameters.CurrentProgramParameterActive = false;
        }

        /* Inactive if parameter end date exists and is on or before the program evaluation begin date */
        else if ((MpParameters.CurrentProgramParameter.ParamEndDate != null) && (MpParameters.CurrentProgramParameter.ParamEndDate <= MpParameters.ProgramEvaluationBeginDate))
        {
          MpParameters.CurrentProgramParameterActive = false;
        }

        /* Special logic for MATS odd opt in */
        else if ((MpParameters.CurrentProgramParameter.PrgCd == "MATS") && (MpParameters.MatsEvaluationBeginDate != null))
        {
          /* Inactive if program evalution end date is on or before MATS evaluation begin date */
          if (MpParameters.ProgramEvaluationEndDate <= MpParameters.MatsEvaluationBeginDate)
          {
            MpParameters.CurrentProgramParameterActive = false;
          }

          /* Inactive if parameter end date exists and is on or before MATS evaluation begin date */
          else if ((MpParameters.CurrentProgramParameter.ParamEndDate != null) && (MpParameters.CurrentProgramParameter.ParamEndDate <= MpParameters.MatsEvaluationBeginDate))
          {
            MpParameters.CurrentProgramParameterActive = false;
          }
        }

        /* Set Current Program Parameter Begin and End Dates if Current Program Parameter is active */
        if (MpParameters.CurrentProgramParameterActive.Default(false) == true)
        {
          // Set ProgramParameterEvaluationBeginDate
          {
            if (MpParameters.ProgramEvaluationBeginDate > MpParameters.CurrentProgramParameter.ParamBeginDate)
              MpParameters.ProgramParameterEvaluationBeginDate = MpParameters.ProgramEvaluationBeginDate;
            else
              MpParameters.ProgramParameterEvaluationBeginDate = MpParameters.CurrentProgramParameter.ParamBeginDate;

            if ((MpParameters.CurrentProgramParameter.PrgCd == "MATS") && (MpParameters.MatsEvaluationBeginDate != null) &&
                (MpParameters.MatsEvaluationBeginDate > MpParameters.ProgramParameterEvaluationBeginDate))
              MpParameters.ProgramParameterEvaluationBeginDate = MpParameters.MatsEvaluationBeginDate;
          }

          // Set ProgramParameterEvaluationEndDate
          {
            if ((MpParameters.CurrentProgramParameter.ParamEndDate == null) || (MpParameters.ProgramEvaluationEndDate < MpParameters.CurrentProgramParameter.ParamEndDate))
              MpParameters.ProgramParameterEvaluationEndDate = MpParameters.ProgramEvaluationEndDate;
            else
              MpParameters.ProgramParameterEvaluationEndDate = MpParameters.CurrentProgramParameter.ParamEndDate;
          }
        }
				else
				{
          MpParameters.CurrentProgramParameterActive = false;
				}
			}
			catch (Exception ex)
			{
				returnVal = category.CheckEngine.FormatError(ex);
			}

			return returnVal;
		}


		/// <summary>
		/// PROGRAM-11: Required Method Reported for Program
		/// </summary>
		/// <param name="category">The calling check category object.</param>
		/// <param name="log">The log messages flag.</param>
		/// <returns>Returns formatted exception information.</returns>
		public string PROGRAM11(cCategory category, ref bool log)
		{ 
			string returnVal = "";

			try
			{
				//logic block
				if (MpParameters.CurrentProgramParameter.RequiredInd.AsBoolean(false)
                    && (MpParameters.CurrentProgramParameter.PrgCd != "MATS")
                    && (MpParameters.CurrentProgramParameter.PrgCd != "NSPS4T")
                    && MpParameters.CurrentProgramParameter.Class.NotInList("N,NA,NB"))
                {
					if (MpParameters.CurrentProgramParameterActive.Default(false))
					{
						string resultType = null;
						string methodParameterList;
						string commonTypeList, multipleTypeList;
						string methodCodeList;
						string severityCd;
						{
							string programMethodParameterDescription;

							Program11_GetProgramParameterToMethodParameterValues(MpParameters.CurrentProgramParameter.ParameterCd,
																				 out methodParameterList,
																				 out programMethodParameterDescription);

							Program11_GetProgramParameterToLocationTypeValues(MpParameters.CurrentProgramParameter.ParameterCd,
																			  out commonTypeList,
																			  out multipleTypeList);

							Program11_GetProgramParameterToMethodCodeValues(MpParameters.CurrentProgramParameter.ParameterCd,
																			  out methodCodeList);

							Program11_GetProgramParameterToSeverityValue(MpParameters.CurrentProgramParameter.ParameterCd,
																		 out severityCd);

							ProgramMethodParameterDescription.SetValue(programMethodParameterDescription, category);
						}


						string unitMonLocId = MpParameters.CurrentProgramParameter.MonLocId;

						DataView unitCommonMethodView;
            {
              string unitCommonMonLocList;
              {
                if (MpParameters.CurrentProgramParameter.PrgCd == "MATS")

                  unitCommonMonLocList = Program11_UnitStackConfigurationMonLocList(unitMonLocId,
                                                                                    MpParameters.ProgramParameterEvaluationBeginDate.AsBeginDateTime(),
                                                                                    MpParameters.ProgramParameterEvaluationEndDate.AsEndDateTime(),
                                                                                    commonTypeList, multipleTypeList);
                else
                  unitCommonMonLocList = Program11_UnitStackConfigurationMonLocList(unitMonLocId,
                                                                                    MpParameters.ProgramParameterEvaluationBeginDate.AsBeginDateTime(),
                                                                                    MpParameters.ProgramParameterEvaluationEndDate.AsEndDateTime(),
                                                                                    commonTypeList);
              }

              //matches current unit PLUS associated common stacks
              unitCommonMethodView
                = cRowFilter.FindActiveRows(MpParameters.CombinedFacilityMethodRecords.SourceView,
                             MpParameters.ProgramParameterEvaluationBeginDate.AsBeginDateTime(),
                             MpParameters.ProgramParameterEvaluationEndDate.AsEndDateTime(),
                             new cFilterCondition[] 
                                          { 
                                            new cFilterCondition("MON_LOC_ID", unitCommonMonLocList, eFilterConditionStringCompare.InList), 
                                            new cFilterCondition("PARAMETER_CD", methodParameterList, eFilterConditionStringCompare.InList)  
                                          },
                              new cFilterCondition[] 
                                          { 
                                            new cFilterCondition("MON_LOC_ID", unitCommonMonLocList, eFilterConditionStringCompare.InList), 
                                            new cFilterCondition("METHOD_CD", methodCodeList, eFilterConditionStringCompare.InList) 
                                          });
            }


						if ((unitCommonMethodView.Count == 0) ||
							  !CheckForHourRangeCovered(category,
													                unitCommonMethodView,
													                MpParameters.ProgramParameterEvaluationBeginDate.AsBeginDateTime(), 23,
													                MpParameters.ProgramParameterEvaluationEndDate.AsEndDateTime(), 0))
            {
              /* For MATS, the check for covering MS methods was completed above. */
              if (MpParameters.CurrentProgramParameter.PrgCd == "MATS")
              {
                resultType = ((unitCommonMethodView.Count == 0) ? "Missing" : "Incomplete");
              }

              /* For Part 75, check for covering MS, and that if an MS is covering part of an evaluation period, all active MS cover that part of the period. */
              else
              {
                if (!Program11_UnitStackConfigurationExists(unitMonLocId,
                                                            MpParameters.ProgramParameterEvaluationBeginDate.AsBeginDateTime(),
                                                            MpParameters.ProgramParameterEvaluationEndDate.AsEndDateTime(),
                                                            multipleTypeList))
                {
                  if (unitCommonMethodView.Count == 0)
                  {
                    resultType = "Missing";
                  }
                  else
                  {
                    resultType = "Incomplete";
                  }
                }
                else
                {
                  DataView allMethodView;
                  {
                    string allLocList = Program11_UnitStackConfigurationMonLocList(unitMonLocId,
                                      MpParameters.ProgramParameterEvaluationBeginDate.AsBeginDateTime(),
                                      MpParameters.ProgramParameterEvaluationEndDate.AsEndDateTime(),
                                      commonTypeList, multipleTypeList);

                    allMethodView
                      = cRowFilter.FindActiveRows(MpParameters.FacilityMethodRecords.SourceView,
                                    MpParameters.ProgramParameterEvaluationBeginDate.AsBeginDateTime(),
                                    MpParameters.ProgramParameterEvaluationEndDate.AsEndDateTime(),
                                    new cFilterCondition[] 
                                              { 
                                                new cFilterCondition("MON_LOC_ID", allLocList, eFilterConditionStringCompare.InList), 
                                                new cFilterCondition("PARAMETER_CD", methodParameterList, eFilterConditionStringCompare.InList)  
                                              },
                                    new cFilterCondition[] 
                                              { 
                                                new cFilterCondition("MON_LOC_ID", allLocList, eFilterConditionStringCompare.InList), 
                                                new cFilterCondition("METHOD_CD", methodCodeList, eFilterConditionStringCompare.InList) 
                                              });
                  }

                  if (allMethodView.Count == 0)
                  {
                    resultType = "Missing";
                  }
                  else if (!CheckForHourRangeCovered(category,
                                     allMethodView,
                                     MpParameters.ProgramParameterEvaluationBeginDate.AsBeginDateTime(), 23,
                                     MpParameters.ProgramParameterEvaluationEndDate.AsEndDateTime(), 0))
                  {
                    resultType = "Incomplete";
                  }
                  else
                  {
                    // US Config records don't have hour precision: go through and look for Method records to cover any part of each day in the period
                    // Use last hour of first day and first hour of last day as cut off

                    for (DateTime hour = MpParameters.ProgramParameterEvaluationBeginDate.Value.AsBeginDateTime().Date.AddHours(23);
                       hour <= MpParameters.ProgramParameterEvaluationEndDate.Value.AsEndDateTime();
                       hour = hour.AddHours(1))
                    {
                      DataView hourUnitCommonMethodView;
                      {
                        string hourUnitCommonLocList = Program11_UnitStackConfigurationMonLocList(unitMonLocId, hour, commonTypeList);

                        hourUnitCommonMethodView
                          = cRowFilter.FindActiveRows(MpParameters.FacilityMethodRecords.SourceView,
                                        hour.Date,
                                        hour.Hour,
                                        new cFilterCondition[] 
                                                  { 
                                                    new cFilterCondition("MON_LOC_ID", hourUnitCommonLocList, eFilterConditionStringCompare.InList), 
                                                    new cFilterCondition("PARAMETER_CD", methodParameterList, eFilterConditionStringCompare.InList)  
                                                  },
                                          new cFilterCondition[] 
                                                  { 
                                                    new cFilterCondition("MON_LOC_ID", hourUnitCommonLocList, eFilterConditionStringCompare.InList), 
                                                    new cFilterCondition("METHOD_CD", methodCodeList, eFilterConditionStringCompare.InList) 
                                                  });
                      }

                      if (hourUnitCommonMethodView.Count > 0)
                      {
                        // Check Next Hour
                      }
                      else
                      {
                        DataView hourMultipleConfigurationView, hourMultipleMethodView;
                        {
                          cFilterCondition[] filterConditions;

                          if (hour.Hour == 0)
                          {
                            filterConditions
                              = new cFilterCondition[] 
                            { 
                              new cFilterCondition("Mon_Loc_Id", unitMonLocId),
                              new cFilterCondition("STACK_NAME", multipleTypeList, eFilterConditionStringCompare.InList, 0, 2),
                              new cFilterCondition("BEGIN_DATE", hour.Date, eFilterDataType.DateBegan, eFilterConditionRelativeCompare.LessThan),
                              new cFilterCondition("END_DATE", hour.Date, eFilterDataType.DateEnded, eFilterConditionRelativeCompare.GreaterThanOrEqual)
                            };
                          }
                          else if (hour.Hour == 23)
                          {
                            filterConditions
                              = new cFilterCondition[] 
                            { 
                              new cFilterCondition("Mon_Loc_Id", unitMonLocId),
                              new cFilterCondition("STACK_NAME", multipleTypeList, eFilterConditionStringCompare.InList, 0, 2),
                              new cFilterCondition("BEGIN_DATE", hour.Date, eFilterDataType.DateBegan, eFilterConditionRelativeCompare.LessThanOrEqual),
                              new cFilterCondition("END_DATE", hour.Date, eFilterDataType.DateEnded, eFilterConditionRelativeCompare.GreaterThan)
                            };
                          }
                          else
                          {
                            filterConditions
                              = new cFilterCondition[] 
                            { 
                              new cFilterCondition("Mon_Loc_Id", unitMonLocId),
                              new cFilterCondition("STACK_NAME", multipleTypeList, eFilterConditionStringCompare.InList, 0, 2),
                              new cFilterCondition("BEGIN_DATE", hour.Date, eFilterDataType.DateBegan, eFilterConditionRelativeCompare.LessThan),
                              new cFilterCondition("END_DATE", hour.Date, eFilterDataType.DateEnded, eFilterConditionRelativeCompare.GreaterThan)
                            };
                          }

                          hourMultipleConfigurationView = cRowFilter.FindActiveRows(MpParameters.UnitStackConfigurationRecords.SourceView,
                                                        MpParameters.ProgramParameterEvaluationBeginDate.AsBeginDateTime(),
                                                        MpParameters.ProgramParameterEvaluationEndDate.AsEndDateTime(),
                                                        filterConditions);

                          string hourMultipleLocList = Program11_UnitStackConfigurationStackPipeLocList(filterConditions);


                          hourMultipleMethodView
                            = cRowFilter.FindActiveRows(MpParameters.FacilityMethodRecords.SourceView,
                                          hour.Date,
                                          hour.Hour,
                                          new cFilterCondition[] 
                                                        { 
                                                          new cFilterCondition("MON_LOC_ID", hourMultipleLocList, eFilterConditionStringCompare.InList), 
                                                          new cFilterCondition("PARAMETER_CD", methodParameterList, eFilterConditionStringCompare.InList)  
                                                        },
                                          new cFilterCondition[] 
                                                        { 
                                                          new cFilterCondition("MON_LOC_ID", hourMultipleLocList, eFilterConditionStringCompare.InList), 
                                                          new cFilterCondition("METHOD_CD", methodCodeList, eFilterConditionStringCompare.InList) 
                                                        });
                        }

                        int multipleStackCount, multipleStackMethod, multiplePipeCount, multiplePipeMethod;
                        {
                          multipleStackCount = multipleStackMethod = multiplePipeCount = multiplePipeMethod = 0;

                          // Check Stacks/Pipes if unit method not found
                          foreach (DataRowView hourMultipleConfigurationRow in hourMultipleConfigurationView)
                          {
                            bool methodFound = false;

                            foreach (DataRowView hourMultipleMethodRow in hourMultipleMethodView)
                            {
                              if (hourMultipleConfigurationRow["STACK_PIPE_MON_LOC_ID"].AsString() == hourMultipleMethodRow["MON_LOC_ID"].AsString())
                              {
                                methodFound = true;
                                break;
                              }
                            }

                            switch (hourMultipleConfigurationRow["STACK_NAME"].AsString().Substring(0, 2))
                            {
                              case "MS": { multipleStackCount += 1; if (methodFound) multipleStackMethod += 1; } break;
                              case "MP": { multiplePipeCount += 1; if (methodFound) multiplePipeMethod += 1; } break;
                            }
                          }
                        }

                        if ((multipleStackMethod > 0) && (multipleStackMethod != multipleStackCount))
                        {
                          {
                            resultType = "Incomplete";
                          }
                          break;
                        }
                        else if ((multiplePipeMethod > 0) && (multiplePipeMethod != multiplePipeCount))
                        {
                          {
                            resultType = "Incomplete";
                          }
                          break;
                        }
                        else
                        {
                          // Check next hour
                        }
                      }
                    }
                  }
                }
              }
            }

						//result block
						if (resultType == "Missing")
						{
							if (MpParameters.MatsRequiredCheck == true || MpParameters.CurrentProgramParameter.ParameterCd.NotInList("HG,HCL,HF"))
							{
								if (severityCd == "NONCRIT")
								{
									category.CheckCatalogResult = "C";
								}
								else if (severityCd == "INFORM")
								{
									category.CheckCatalogResult = "E";
								}
								else
								{
									category.CheckCatalogResult = "A";
								}
							}
						}
						else if (resultType == "Incomplete")
						{
							if (severityCd == "NONCRIT")
							{
								category.CheckCatalogResult = "D";
							}
							else if (severityCd == "INFORM")
							{
								category.CheckCatalogResult = "F";
							}
							else
							{
								category.CheckCatalogResult = "B";
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

        /// <summary>
        /// 
        /// Program-12
        /// 
        /// For units with a MATS Program record that is active during the MP Evaluation Period,  the following conditions must exist for each MATS parameter:
        /// 
        /// 1) MATS methods for the parameter must exists.
        /// 2) The methods must span the Program Parameter Evaluation Period.
        /// 3) A method is only required at one stack to cover a period of time, even when multiple stacks exist.
        /// 
        /// Note that the Program Parameter Evaluation Begin Date cannot be earlier than the Evaluation Begin Date, but is otherwise the earlier of the:
        /// 
        /// 1) Monitoring Method begin dates for MATS parameters.
        /// 2) MATS Supplemental Method begin dates.
        /// 3) The MATS Rule Compliance Date from the System Parameters.
        /// </summary>
		/// <param name="category">The calling check category object.</param>
		/// <param name="log">The log messages flag.</param>
		/// <returns>Returns formatted exception information.</returns>
        /// 
        public string PROGRAM12(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                /* Only perform the body of this check for required MATS parameter where the unit is affected by MATS */
                if ((MpParameters.CurrentProgram.PrgCd == "MATS") && (MpParameters.CurrentProgram.Class == "A"))
                {
                    if (MpParameters.CurrentProgramActive.Default(true))
                    {
                        /* Get list with unit MON_LOC_ID and associated active MON_LOC_ID */
                        string monLocIdList 
                            = Program11_UnitStackConfigurationMonLocList(MpParameters.CurrentProgram.MonLocId,
                                                                         MpParameters.ProgramEvaluationBeginDate.AsBeginDateTime(),
                                                                         MpParameters.ProgramEvaluationEndDate.AsEndDateTime(),
                                                                         "CS", "MS");

                        /* Locate MONITOR_METHOD rows for the associated MON_LOC_ID and Method Parameters */
                        DataView methodView
                            = cRowFilter.FindActiveRows(MpParameters.CombinedFacilityMethodRecords.SourceView,
                                                        MpParameters.ProgramEvaluationBeginDate.AsBeginDateTime(),
                                                        MpParameters.ProgramEvaluationEndDate.AsEndDateTime(),
                                                        new cFilterCondition[]
                                                        {
                                                            new cFilterCondition("MON_LOC_ID", monLocIdList, eFilterConditionStringCompare.InList),
                                                            new cFilterCondition("CROSSCHECK_PARAMETER", "HGRE,HGRH,HCLRE,HCLRH,HFRE,HFRH,SO2RE,SO2RH,MATSSUP", eFilterConditionStringCompare.InList)
                                                        });

                        /* Produce result if no methods exist */
                        if (methodView.Count == 0)
                        {
                            category.CheckCatalogResult = "A";
                        }

                        /* Produce result if methods do not span the Program Parameter evaluation period */
                        else if (!CheckForHourRangeCovered(category,
                                                           methodView,
                                                           MpParameters.ProgramEvaluationBeginDate.AsBeginDateTime(), 23,
                                                           MpParameters.ProgramEvaluationEndDate.AsEndDateTime(), 0))
                        {
                            category.CheckCatalogResult = "B";
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
        /// For units with a NSPS4T Program record that is active during the MP Evaluation Period,  the following conditions must exist for each MATS parameter:
        /// 
        /// 1) For a required parameter in a NSPS4T Program Parameter record, at least one Monitoing Method record must exist with a matching method parameter.
        /// 2) Monitoring Methods must span the Program Parameter Evaluation Period.
        /// 3) For singe unit configurations, methods must exist at the unit and span the Program Parameter evaluation period.
        /// 4) Otherwise for each unit in a configuration, any distinct period of time within the evaluation period must contain Monitoring Methods at either the unit, all stacks connected to the unit, or all pipes connected to the unit.
        /// 5) As long as the above are met, there is no restriction to having Monitor Methods at other locations.
        /// 
        /// The logic in this check does the following:
        /// 
        /// 1) Determine whether any method exist for the program parameter unit and any conncected stacks and pipes
        ///    a) If none exist, result A is returned.
        ///    b) If methods exists but do not span the program parameter evaluation period, result B is returned.
        /// 2) Checking continues if a result was not returned above and stack and/or pipes are connected to the unit.
        /// 3) The remainder of the check ensures that any range within the program parameter evaluation period not 
        ///    covered by unit methods, are covered by either methods at all of the stacks, or at all of the pipes.  If
        ///    any period is not fully covered by all of the stacks or all of the pipes, result B is returned.
        ///    
        /// Note that because Unit Stack Configuration does not have begin and end hours, a specific stack or pipe is 
        /// only expected to cover hour 23 of it USC begin date, and hour 0 of it end date (if one exists).  Those are
        /// the only hours for which the stack or pipe was definitely connected to the unit on those days.
        /// </summary>
        /// <param name="category"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public string PROGRAM13(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                MpParameters.Nsps4tMethodParameterDescription = null;

                /* Only perform the body of this check for required NSPS4T parameter where the unit is affected by NSPS4T */
                if ((MpParameters.CurrentProgramParameter.PrgCd == "NSPS4T") &&
                    (MpParameters.CurrentProgramParameter.Class == "A") &&
                    (MpParameters.CurrentProgramParameter.RequiredInd == 1))
                {
                    /* Only perform the check if the NSPS4T parameter is active for the evaluation period */
                    if (MpParameters.CurrentProgramParameterActive.Default(true))
                    {
                        /* Dereference widely used check parameter values */
                        DateTime evaluationBeginDate = MpParameters.ProgramParameterEvaluationBeginDate.Value;
                        DateTime evaluationEndDate = MpParameters.ProgramParameterEvaluationEndDate.Value;
                        string unitMonLocId = MpParameters.CurrentProgramParameter.MonLocId;
                        int unitId = MpParameters.CurrentProgramParameter.UnitId.Value;


                        /* Get Method Parameter list and description for current Program Parameter row */
                        string methodParameterList;
                        {
                            string programMethodParameterDescription;

                            Program11_GetProgramParameterToMethodParameterValues(MpParameters.CurrentProgramParameter.ParameterCd, out methodParameterList, out programMethodParameterDescription);
                            MpParameters.Nsps4tMethodParameterDescription = programMethodParameterDescription;
                        }


                        /* Get the Unit Stack Configuration rows for the unit */
                        CheckDataView<VwUnitStackConfigurationRow> unitStackConfigurationView
                            = MpParameters.UnitStackConfigurationRecords.FindActiveRows(evaluationBeginDate, evaluationEndDate, new cFilterCondition("UNIT_ID", unitId));

                        /* Check Monitor Methods for the unit */
                        string monLocList = unitMonLocId
                                          + (unitStackConfigurationView.Count > 0 ? "," + ColumnToDatalist(unitStackConfigurationView.SourceView, "Stack_Pipe_Mon_Loc_Id") : "");

                        CheckDataView<VwMonitorMethodRow> facilityMethodRecords
                            = MpParameters.FacilityMethodRecords.FindActiveRows(evaluationBeginDate, evaluationEndDate,
                                                                                new cFilterCondition("MON_LOC_ID", monLocList, eFilterConditionStringCompare.InList),
                                                                                new cFilterCondition("PARAMETER_CD", methodParameterList, eFilterConditionStringCompare.InList));

                        /* Check all methods at units, stacks and pipes.  Rules for covering methods at stacks and pipes will not matter if all methods do not cover perid. */
                        if (facilityMethodRecords.Count == 0)
                        {
                            category.CheckCatalogResult = "A";
                        }
                        else if (!CheckForHourRangeCovered(category, facilityMethodRecords.SourceView, evaluationBeginDate, 23, evaluationEndDate, 0))
                        {
                            category.CheckCatalogResult = "B";
                        }

                        /* Passed for all methods, but check for stack and pipe rules if the configuration contains stacks, pipes or both */
                        else if (unitStackConfigurationView.Count > 0)
                        {
                            /* Get methods at unit to determine whether any gaps exist that stacks and pipes are covering. */
                            facilityMethodRecords
                                = MpParameters.FacilityMethodRecords.FindActiveRows(evaluationBeginDate, evaluationEndDate,
                                                                                    new cFilterCondition("UNIT_ID", unitId),
                                                                                    new cFilterCondition("PARAMETER_CD", methodParameterList, eFilterConditionStringCompare.InList));

                            /* Determine parts of range not covered by unit methods */
                            cHourRangeCollection missingUnitMethodRangeList;
                            {
                                if (facilityMethodRecords.Count == 0)
                                {
                                    missingUnitMethodRangeList = new cHourRangeCollection();
                                    missingUnitMethodRangeList.Add(evaluationBeginDate, 23, evaluationEndDate, 0);
                                }
                                else
                                    missingUnitMethodRangeList = GetMethodGaps(facilityMethodRecords, evaluationBeginDate.Date.AddHours(23), evaluationEndDate.Date);
                            }

                            /* If unit methods do not cover the whole range */
                            if (missingUnitMethodRangeList.Count > 0)
                            {
                                DateTime rangeBeginHour, rangeEndHour;

                                eTimespanCoverageState stackState = eTimespanCoverageState.None;
                                eTimespanCoverageState pipeState = eTimespanCoverageState.None;

                                /* Loop through ranges not covered by unit methods */
                                foreach (cHourRange missingMethodHourRange in missingUnitMethodRangeList)
                                {
                                    /* Determine USC rows that are active during this gap */
                                    unitStackConfigurationView
                                        = MpParameters.UnitStackConfigurationRecords.FindActiveRows(missingMethodHourRange.BeganDate, missingMethodHourRange.EndedDate, new cFilterCondition("UNIT_ID", unitId));

                                    /* Loop through USC that are active for this gap */
                                    foreach (VwUnitStackConfigurationRow unitStackConfigurationRow in unitStackConfigurationView)
                                    {
                                        /* Use hour 23 of the USC begin date if the begin date of the gap is on or before the USC begin date */
                                        rangeBeginHour = (missingMethodHourRange.BeganDate > unitStackConfigurationRow.BeginDate.Value.Date) 
                                                        ? missingMethodHourRange.BeganDateHour
                                                        : unitStackConfigurationRow.BeginDate.Value.Date.AddHours(23);
                                        /* Use hour 0 of the USC end date if the end date of the gap is on or after the USC end date */
                                        rangeEndHour = (missingMethodHourRange.EndedDate < unitStackConfigurationRow.EndDate.Default(DateTime.MaxValue).Date)
                                                      ? missingMethodHourRange.EndedDateHour
                                                      : unitStackConfigurationRow.EndDate.Value.Date;

                                        /* Get the methods that are active for the stack or pipe during the search hour range */
                                        facilityMethodRecords
                                            = MpParameters.FacilityMethodRecords.FindActiveRowsByHour(rangeBeginHour, rangeEndHour,
                                                                                                      new cFilterCondition("STACK_PIPE_ID", unitStackConfigurationRow.StackPipeId),
                                                                                                      new cFilterCondition("PARAMETER_CD", methodParameterList, eFilterConditionStringCompare.InList));

                                        /* 
                                         * Update either stackState or pipeState based on the type of the connected location.
                                         * 
                                         * For either all stacks or all pipes:
                                         * 
                                         *  If no locations exists then the cumulative value for the gaps will be NONE.
                                         *  If an incomplete gap exists for any location, the cumulative value for the gaps will be INCOMPLETE.
                                         *  If a covered gap and a gap that is not covered exists, the cummulative value for the gaps will be INCOMPLETE.
                                         *  If all the gaps are covered or all the gaps are not covered, the cumulative value for the gaps will be VALID or MISSING respectively.
                            
                                         */
                                        switch (unitStackConfigurationRow.StackName.PadRight(2).Substring(0, 2))
                                        {
                                            case "CS":
                                            case "MS":
                                                {
                                                    stackState = UpdateMethodState(stackState, facilityMethodRecords, rangeBeginHour, rangeEndHour, category);
                                                }
                                                break;

                                            case "CP":
                                            case "MP":
                                                {
                                                    pipeState = UpdateMethodState(pipeState, facilityMethodRecords, rangeBeginHour, rangeEndHour, category);
                                                }
                                                break;
                                        }
                                    } // Loop through USC that are active for this gap
                                } // Loop through ranges not covered by unit methods

                                /* 
                                 *  If either the stack or the pipe state is SPANS, then all gaps are covered and no result is returned.
                                 *  Otherwise, the stack and pipe results are incomplete or missing (implies unit methods exist), so the
                                 *  result is INCOMPLETE.
                                 */
                                if ((stackState != eTimespanCoverageState.Spans) && (pipeState != eTimespanCoverageState.Spans))
                                {
                                    category.CheckCatalogResult = "B";
                                }
                            } // Unit methods do not cover the whole range
                        } // Configuration contains stacks, pipes or both
                    } // Program Parameter is Active
                } //NSPS4T Affected and Required Parameter
            }
            catch (Exception ex)
            {
                returnVal = category.CheckEngine.FormatError(ex);
            }

            return returnVal;
        }


        /// <summary>
        /// For units with a NSPS4T Program record that is active during the MP Evaluation Period,  the following conditions must exist for each MATS parameter:
        /// 
        /// 1) For a required parameter in a NSPS4T Program Parameter record, at least one Monitoing Method record must exist with a matching method parameter.
        /// 2) Monitoring Methods must span the Program Parameter Evaluation Period.
        /// 3) For singe unit configurations, methods must exist at the unit and span the Program Parameter evaluation period.
        /// 3) For multiple stack configurations(contains only units and MS), the methods must exist and span the evaluation period at each MS.
        /// 4) Otherwise for each unit in a configuration, any distinct period of time within the evaluation period must contain Monitoring Methods at either the unit, all stacks connected to the unit, or all pipes connected to the unit.
        /// 5) As long as the above are met, there is no restriction to having Monitor Methods at other locations.
        /// </summary>
        /// <param name="category"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public string PROGRAM13_Old(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                MpParameters.Nsps4tMethodParameterDescription = null;

                /* Only perform the body of this check for required NSPS4T parameter where the unit is affected by NSPS4T */
                if ((MpParameters.CurrentProgramParameter.PrgCd == "NSPS4T") &&
                    (MpParameters.CurrentProgramParameter.Class == "A") &&
                    (MpParameters.CurrentProgramParameter.RequiredInd == 1))
                {
                    /* Only perform the check if the NSPS4T parameter is active for the evaluation period */
                    if (MpParameters.CurrentProgramParameterActive.Default(true))
                    {
                        /* Dereference widely used check parameter values */
                        DateTime evaluationBeginDate = MpParameters.ProgramParameterEvaluationBeginDate.Value;
                        DateTime evaluationEndDate = MpParameters.ProgramParameterEvaluationEndDate.Value;
                        string unitMonLocId = MpParameters.CurrentProgramParameter.MonLocId;
                        int unitId = MpParameters.CurrentProgramParameter.UnitId.Value;

                        /* Widely Used Result Sets*/
                        CheckDataView<VwUnitStackConfigurationRow> unitStackConfigurationView;

                        /* Get Method Parameter list and description for current Program Parameter row */
                        string methodParameterList;
                        {
                            string programMethodParameterDescription;

                            Program11_GetProgramParameterToMethodParameterValues(MpParameters.CurrentProgramParameter.ParameterCd, out methodParameterList, out programMethodParameterDescription);
                            MpParameters.Nsps4tMethodParameterDescription = programMethodParameterDescription;
                        }

                        /* Get the Unit Stack Configuration rows for the unit */
                        unitStackConfigurationView
                            = MpParameters.UnitStackConfigurationRecords.FindActiveRows(evaluationBeginDate, evaluationEndDate, new cFilterCondition("UNIT_ID", unitId));

                        /* If Unit Stack Configuration rows are not found, this is a single unit configuration */
                        if (unitStackConfigurationView.Count == 0)
                        {
                            CheckDataView<VwMonitorMethodRow> facilityMethodRecords
                                = MpParameters.FacilityMethodRecords.FindActiveRows(evaluationBeginDate, evaluationEndDate,
                                                                                    new cFilterCondition("UNIT_ID", unitId),
                                                                                    new cFilterCondition("PARAMETER_CD", methodParameterList, eFilterConditionStringCompare.InList));

                            if (facilityMethodRecords.Count == 0)
                            {
                                category.CheckCatalogResult = "A";
                            }
                            else if (!CheckForHourRangeCovered(category, facilityMethodRecords.SourceView, evaluationBeginDate, 23, evaluationEndDate, 0))
                            {
                                category.CheckCatalogResult = "B";
                            }
                        }

                        /*  Unit connected to at least one stack or pipe */
                        else
                        {
                            int cpCount = 0, csCount = 0, mpCount = 0, msCount = 0;

                            /* Determine type and count of connected stacks and pipes. */
                            foreach (VwUnitStackConfigurationRow unitStackConfigurationRow in unitStackConfigurationView)
                            {
                                switch (unitStackConfigurationRow.StackName.PadRight(2).Substring(0, 2))
                                {
                                    case "CS": csCount += 1; break;
                                    case "MS": msCount += 1; break;
                                    case "CP": cpCount += 1; break;
                                    case "MP": mpCount += 1; break;
                                }
                            }


                            /* Handle MS configurations separately */
                            if ((msCount > 0) && (csCount == 0) && (cpCount == 0) && (mpCount == 0))
                            {
                                string methodState = "NONE";

                                foreach (VwUnitStackConfigurationRow unitStackConfigurationRow in unitStackConfigurationView)
                                {
                                    CheckDataView<VwMonitorMethodRow> facilityMethodRecords
                                        = MpParameters.FacilityMethodRecords.FindActiveRows(evaluationBeginDate, evaluationEndDate,
                                                        new cFilterCondition("STACK_PIPE_ID", unitStackConfigurationRow.StackPipeId),
                                                        new cFilterCondition("PARAMETER_CD", methodParameterList, eFilterConditionStringCompare.InList));

                                    /* Check for no methods at MS */
                                    if (facilityMethodRecords.Count == 0)
                                    {
                                        /* If methodState is NONE or VALID set it to MISSING or INCOMPLETE respectively.  
                                         * If methodState is MISSING or INCOMPLETE, it should not change. */
                                        if (methodState == "NONE")
                                            methodState = "MISSING";
                                        else if (methodState == "VALID")
                                            methodState = "INCOMPLETE";
                                    }
                                    /* Check for methods do not span (modified) evaluation period */
                                    else if (!CheckForHourRangeCovered(category, facilityMethodRecords.SourceView, evaluationBeginDate, 23, evaluationEndDate, 0))
                                    {
                                        methodState = "INCOMPLETE";
                                    }
                                    else
                                    {
                                        /* If methodState is NONE or MISSING set it to VALID or INCOMPLETE respectively.  
                                         * If methodState is VALID or INCOMPLETE, it should not change. */
                                        if (methodState == "NONE")
                                            methodState = "VALID";
                                        else if (methodState == "MISSING")
                                            methodState = "INCOMPLETE";
                                    }
                                }

                                if (methodState == "MISSING")
                                {
                                    category.CheckCatalogResult = "A";
                                }
                                else if (methodState == "INCOMPLETE")
                                {
                                    category.CheckCatalogResult = "C";
                                }
                            }

                            /* Handle non MS configurations (or configuration changes) where stacks or pipes are involved. */
                            else
                            {
                                /* Locate the method records for the evaluation period */
                                string monLocIdList = BuildNsps4tMonLocIdList(false, unitMonLocId, unitStackConfigurationView);

                                CheckDataView<VwMonitorMethodRow> facilityMethodRecords
                                    = MpParameters.FacilityMethodRecords.FindActiveRows(evaluationBeginDate, evaluationEndDate,
                                                                                        new cFilterCondition("MON_LOC_ID", monLocIdList, eFilterConditionStringCompare.InList),
                                                                                        new cFilterCondition("PARAMETER_CD", methodParameterList, eFilterConditionStringCompare.InList));

                                /* Check for no existing methods for any location during the evaluation period */
                                if (facilityMethodRecords.Count == 0)
                                {
                                    category.CheckCatalogResult = "A";
                                }
                                /* Check for methods for all locations not spanning the evaluation period */
                                else if (!CheckForHourRangeCovered(category, facilityMethodRecords.SourceView, evaluationBeginDate, 23, evaluationEndDate, 0))
                                {
                                    category.CheckCatalogResult = "B";
                                }

                                /* 
                                 * Since methods for all locations span the evalution period, ensure that period not covered by the unit are 
                                 * covered by either all connected stacks or all connected pipes
                                 */
                                else
                                {
                                    /*  
                                     *  Add the following to a set of range start hours:
                                     *  
                                     *  1) Evaluation Begin Date, hour 23
                                     *  2) Evaluation End Date, hour 1
                                     *  3) Hours 1 and 23 of each Unit Stack Configuration Begin Date
                                     *     that occurs (exclusively) between the Evaluation Begin and End Dates
                                     *  4) Hours 1 and 23 of each non null Unit Stack Configuration End Date
                                     *     that occurs (exclusively) between the Evaluation Begin and End Dates
                                     *  
                                     *  
                                     */
                                    List<DateTime> hourRangeStartDates = new List<DateTime>();
                                    {
                                        hourRangeStartDates.Add(evaluationBeginDate.Date.AddHours(23));
                                        hourRangeStartDates.Add(evaluationEndDate.Date.AddHours(1)); // Determines the last end hour

                                        foreach (VwUnitStackConfigurationRow unitStackConfigurationRow in unitStackConfigurationView)
                                        {
                                            if ((unitStackConfigurationRow.BeginDate.Value.Date > evaluationBeginDate.Date) &&
                                                (unitStackConfigurationRow.BeginDate.Value.Date < evaluationEndDate.Date))
                                            {
                                                hourRangeStartDates.Add(unitStackConfigurationRow.BeginDate.Value.Date.AddHours(1));
                                                hourRangeStartDates.Add(unitStackConfigurationRow.BeginDate.Value.Date.AddHours(23));
                                            }

                                            if (unitStackConfigurationRow.EndDate.HasValue &&
                                                (unitStackConfigurationRow.EndDate.Value.Date > evaluationBeginDate.Date) &&
                                                (unitStackConfigurationRow.EndDate.Value.Date < evaluationEndDate.Date))
                                            {
                                                hourRangeStartDates.Add(unitStackConfigurationRow.EndDate.Value.Date.AddHours(1));
                                                hourRangeStartDates.Add(unitStackConfigurationRow.EndDate.Value.Date.AddHours(23));
                                            }
                                        }

                                        hourRangeStartDates = hourRangeStartDates.Distinct().ToList();
                                        hourRangeStartDates.Sort();
                                    }

                                    /* 
                                     * Check to ensure that for each hour during the period, a unit, all stacks or all pipes have methods. 
                                     * For MS configurations, the methods must be at the MS.
                                     */

                                    int cpMethodCount, csMethodCount, mpMethodCount, msMethodCount;
                                    int methodIncreament;
                                    DateTime rangeStartHour, rangeEndHour;
                                    eFilterConditionRelativeCompare beginUscCompare, endUscCompare;

                                    for (int startHourDex = 0; startHourDex < (hourRangeStartDates.Count - 1); startHourDex++)
                                    {
                                        rangeStartHour = hourRangeStartDates[startHourDex];
                                        rangeEndHour = hourRangeStartDates[startHourDex + 1].AddHours(-1);  // Get the hour before the next start hour

                                        facilityMethodRecords
                                            = MpParameters.FacilityMethodRecords.FindActiveRowsByHour(rangeStartHour, rangeEndHour,
                                                                                                      new cFilterCondition("UNIT_ID", unitId),
                                                                                                      new cFilterCondition("PARAMETER_CD", methodParameterList, eFilterConditionStringCompare.InList));

                                        if (facilityMethodRecords.Count > 0)
                                        {
                                            continue; // with next range
                                        }
                                        else
                                        {
                                            /* Only include USC that begin on Range Start Date if Range Start Hour equals 23.  The USC is gauranteed to have started by the 23rd hour of it's begin date. */
                                            beginUscCompare = (rangeStartHour.Hour == 23) ? eFilterConditionRelativeCompare.LessThanOrEqual : eFilterConditionRelativeCompare.LessThan;
                                            /* Only include USC that end on Range End Date if Range End Hour equals 0.  The USC is gauranteed to still be active during hour 0 of it's end date. */
                                            endUscCompare = (rangeEndHour.Hour == 0) ? eFilterConditionRelativeCompare.GreaterThanOrEqual : eFilterConditionRelativeCompare.GreaterThan;

                                            /* Get USC to include in method check */
                                            unitStackConfigurationView
                                                = MpParameters.UnitStackConfigurationRecords.FindRows(new cFilterCondition("UNIT_ID", unitId),
                                                                                                      new cFilterCondition("BEGIN_DATE", beginUscCompare, rangeStartHour.Date, eNullDateDefault.Min),
                                                                                                      new cFilterCondition("END_DATE", endUscCompare, rangeEndHour.Date, eNullDateDefault.Max));

                                            /* Locate the method records for the range */
                                            monLocIdList = BuildNsps4tMonLocIdList(false, unitMonLocId, unitStackConfigurationView);

                                            facilityMethodRecords
                                                = MpParameters.FacilityMethodRecords.FindActiveRowsByHour(rangeStartHour, rangeEndHour,
                                                                                                          new cFilterCondition("MON_LOC_ID", monLocIdList, eFilterConditionStringCompare.InList),
                                                                                                          new cFilterCondition("PARAMETER_CD", methodParameterList, eFilterConditionStringCompare.InList));

                                            /* Generate counts to determine whether all stacks or all pipes have a method for the hour */
                                            cpCount = csCount = mpCount = msCount = 0;
                                            cpMethodCount = csMethodCount = mpMethodCount = msMethodCount = 0;

                                            foreach (VwUnitStackConfigurationRow unitStackConfigurationRow in unitStackConfigurationView)
                                            {
                                                methodIncreament = 0;

                                                foreach (VwMonitorMethodRow monitorMethodRow in facilityMethodRecords)
                                                {
                                                    if (monitorMethodRow.MonLocId == unitStackConfigurationRow.StackPipeMonLocId)
                                                    {
                                                        methodIncreament = 1;
                                                        break;
                                                    }
                                                }

                                                switch (unitStackConfigurationRow.StackName.PadRight(2).Substring(0, 2))
                                                {
                                                    case "CS": csCount += 1; csMethodCount += methodIncreament; break;
                                                    case "MS": msCount += 1; msMethodCount += methodIncreament; break;
                                                    case "CP": cpCount += 1; cpMethodCount += methodIncreament; break;
                                                    case "MP": mpCount += 1; mpMethodCount += methodIncreament; break;
                                                }
                                            }

                                            if ((csCount + msCount > 0) && !((csCount == csMethodCount) && (msCount == msMethodCount)) ||
                                                (cpCount + mpCount > 0) && !((cpCount == cpMethodCount) && (mpCount == mpMethodCount)))
                                            {
                                                category.CheckCatalogResult = "B";
                                                break;
                                            }
                                        }
                                    } // Range Loop
                                }
                            } // Handle non MS Stack and/or Pipe configurations
                        }
                    } // Program Parameter is Active
                } //NSPS4T Affected and Required Parameter
            }
            catch (Exception ex)
            {
                returnVal = category.CheckEngine.FormatError(ex);
            }

            return returnVal;
        }

        /// <summary>
        /// 
        /// Program-14
        /// 
        /// For combustion turbine units with NOX controls and a ARP Program record that is active during the MP Evaluation Period, 
        /// there must be a NOX control record. 
        /// </summary>
		/// <param name="category">The calling check category object.</param>
		/// <param name="log">The log messages flag.</param>
		/// <returns>Returns formatted exception information.</returns>
        /// 
        public string PROGRAM14(cCategory category, ref bool log)
        {
            string returnVal = "";

            try
            {
                /* Only perform the body of this check for required ARP parameter where the program is active */
                if (MpParameters.CurrentProgram.PrgCd == "ARP" && MpParameters.CurrentProgramActive == true)
                {
                    if (MpParameters.CurrentLocation.ComrOpDate.HasValue && (MpParameters.CurrentLocation.ComrOpDate.Value.Year > 1984))
                    {
                        CheckDataView<VwLocationUnitTypeRow> unitTypeRecords = MpParameters.LocationUnitTypeRecords.FindRows(
                            new cFilterCondition[]
                            {
                            new cFilterCondition("UNIT_TYPE_CD", "CC,CT", eFilterConditionStringCompare.InList),
                            new cFilterCondition("END_DATE", null)
                            }
                        );

                        if (unitTypeRecords.Count > 0)
                        {
                            CheckDataView<VwLocationControlRow> controlRecords = MpParameters.LocationControlRecords.FindRows(
                                new cFilterCondition[]
                                {
                                new cFilterCondition("CE_PARAM", "NOX"),
                                new cFilterCondition("RETIRE_DATE", null)
                                }
                            );

                            if (controlRecords.Count == 0)
                            {
                                category.CheckCatalogResult = "A";
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

        #region Helper Methods

        /// <summary>
        /// Returns the list of MON_LOC_ID values to use for NSPS4T Monitor Method locates.
        /// </summary>
        /// <param name="msMethodRequired">Indicates whether MS methods are required.</param>
        /// <param name="unitMonLocId">The MON_LOC_ID for the unit.</param>
        /// <param name="unitStackConfigurationView">The Unit Stack Configuration rows to use when building the list.</param>
        /// <returns></returns>
        private string BuildNsps4tMonLocIdList(bool msMethodRequired, string unitMonLocId, CheckDataView<VwUnitStackConfigurationRow> unitStackConfigurationView)
        {
            string result = msMethodRequired ? "" : unitMonLocId;
            string delim = msMethodRequired ? "" : ",";

            foreach (VwUnitStackConfigurationRow unitStackConfigurationRow in unitStackConfigurationView)
            {
                result += delim + unitStackConfigurationRow.StackPipeMonLocId;
                delim = ",";
            }

            return result;
        }


        /// <summary>
        /// Returns missingMethodRanges with gaps between the cover begin and end hours that are not covered by a method in facilityMethodRecords.
        /// </summary>
        /// <param name="facilityMethodRecords">Facility Method rows that should cover the hour range.</param>
        /// <param name="coverBeginHour">The begining of the hour range to cover.</param>
        /// <param name="coverEndHour">The end of the hour range to cover.</param>
        /// <returns>Returns hour ranges withing the cover range that are not covered by facilityMethodRecords.</returns>
        public cHourRangeCollection GetMethodGaps(CheckDataView<VwMonitorMethodRow> facilityMethodRecords, DateTime coverBeginHour, DateTime coverEndHour)
        {
            cHourRangeCollection result = new cHourRangeCollection();

            cHourRangeCollection existingMethodRanges = new cHourRangeCollection();

            foreach (VwMonitorMethodRow facilityMethodRow in facilityMethodRecords)
            {
                existingMethodRanges.Union(facilityMethodRow.BeginDatehour.Default(coverBeginHour).Date,
                                           facilityMethodRow.BeginDatehour.Default(coverBeginHour).Hour,
                                           facilityMethodRow.EndDatehour.Default(coverEndHour).Date,
                                           facilityMethodRow.EndDatehour.Default(coverEndHour).Hour);
            }

            existingMethodRanges.Sort();

            DateTime nextGapBeginHour = coverBeginHour;
            DateTime nextGapEndHour;

            foreach (cHourRange unitMethodRange in existingMethodRanges)
            {
                if ((unitMethodRange.BeganDateHour <= coverEndHour) && (unitMethodRange.EndedDateHour >= coverBeginHour))
                {
                    if (nextGapBeginHour < unitMethodRange.BeganDateHour)
                    {
                        nextGapEndHour = unitMethodRange.BeganDateHour.AddHours(-1);
                        result.Add(nextGapBeginHour.Date, nextGapBeginHour.Hour, nextGapEndHour.Date, nextGapEndHour.Hour);
                    }

                    nextGapBeginHour = unitMethodRange.EndedDateHour.AddHours(1);
                }
            }

            if (nextGapBeginHour <= coverEndHour)
            {
                result.Add(nextGapBeginHour.Date, nextGapBeginHour.Hour, coverEndHour.Date, coverEndHour.Hour);
            }

            return result;
        }


        /// <summary>
        /// Given a starting method state, a set of method records, and the date range they need to cover, this method returns an updated method state.
        /// 
        /// 1) If the incoming methodState equals INCOMPLETE, the outgoing methodState is always INCOMPLETE.
        /// 2) Otherwise if facilityMethodRecords does not contain rows, the outgoing methodState is:
        ///    a) MISSING if the incoming is NONE or SPANS.
        ///    b) INCOMPLETE if the incoming is SPANS or INCOMPLETE.
        /// 3) Otherwise if facilityMethodRecords contains rows that do not cover the range, the outgoing methodState is:
        ///    a) Always INCOMPLETE.
        /// 4) Otherwise, the outgoing methodState is:
        ///    a) SPANS if the incoming methodState is NONE or SPANS.
        ///    b) INCOMPLETE if the incoming methodState is MISSING or INCOMPLETE.
        /// </summary>
        /// <param name="methodState">The starting method state.</param>
        /// <param name="facilityMethodRecords">The facility method records to check.</param>
        /// <param name="coverBeginHour">The begin hour of the range to cover.</param>
        /// <param name="coverEndHour">The end hour of the range to cover.</param>
        /// <param name="category">The object for the category in which the check is running.</param>
        /// <returns></returns>
        public eTimespanCoverageState UpdateMethodState(eTimespanCoverageState methodState, CheckDataView<VwMonitorMethodRow> facilityMethodRecords, DateTime coverBeginHour, DateTime coverEndHour, cCategory category)
        {
            /* Check for no methods at locations */
            if (facilityMethodRecords.Count == 0)
            {
                /* If methodState is NONE or SPANS set it to MISSING or INCOMPLETE respectively.  
                 * If methodState is MISSING or INCOMPLETE, it should not change. */
                if (methodState == eTimespanCoverageState.None)
                    methodState = eTimespanCoverageState.Missing;
                else if (methodState == eTimespanCoverageState.Spans)
                    methodState = eTimespanCoverageState.Incomplete;
            }
            /* Check for methods span the period */
            else if (CheckForHourRangeCovered(category, facilityMethodRecords.SourceView, coverBeginHour.Date, coverBeginHour.Hour, coverEndHour.Date, coverEndHour.Hour))
            {
                /* If methodState is NONE or MISSING set it to SPANS or INCOMPLETE respectively.  
                 * If methodState is SPANS or INCOMPLETE, it should not change. */
                if (methodState == eTimespanCoverageState.None)
                    methodState = eTimespanCoverageState.Spans;
                else if (methodState == eTimespanCoverageState.Missing)
                    methodState = eTimespanCoverageState.Incomplete;
            }
            /* Method exist but do not span the period */
            else
            {
                methodState = eTimespanCoverageState.Incomplete;
            }

            return methodState;
        }


        /// <summary>
        /// Returns values from a Program Parameter To Method Parameter cross check row based on the passed Program Parameter code.
        /// </summary>
        /// <param name="programParameterCd">The program parameter code to locate in the cross check row.</param>
        /// <param name="methodParameterList">The method parameter list of the cross check row.</param>
        /// <param name="programMethodParameterDescription">The program method parameter description of the cross check row.</param>
        private void Program11_GetProgramParameterToMethodParameterValues(string programParameterCd,
																		  out string methodParameterList,
																		  out string programMethodParameterDescription)
		{
			DataRowView crossCheckRow;

			if (cRowFilter.FindRow(MpParameters.CrosscheckProgramparametertomethodparameter.SourceView,
								   new cFilterCondition[] { new cFilterCondition("ProgramParameterCd", programParameterCd) },
								   out crossCheckRow))
			{
				methodParameterList = crossCheckRow["MethodParameterList"].AsString();
				programMethodParameterDescription = crossCheckRow["MethodParameterDescription"].AsString();
			}
			else
			{
				methodParameterList = null;
				programMethodParameterDescription = null;
			}
		}


		/// <summary>
		/// Returns values from a Program Parameter To Location Type cross check row based on the passed Program Parameter code.
		/// </summary>
		/// <param name="programParameterCd">The program parameter code to locate in the cross check row.</param>
		/// <param name="commonTypeList">The common type list of the cross check row.</param>
		/// <param name="multipleTypeList">The multiple type list of the cross check row.</param>
		private void Program11_GetProgramParameterToLocationTypeValues(string programParameterCd,
																	   out string commonTypeList,
																	   out string multipleTypeList)
		{
			DataRowView crossCheckRow;

			if (cRowFilter.FindRow(MpParameters.CrosscheckProgramparametertolocationtype.SourceView,
								   new cFilterCondition[] { new cFilterCondition("ProgramParameterCd", programParameterCd) },
								   out crossCheckRow))
			{
				commonTypeList = crossCheckRow["CommonLocationTypeList"].AsString();
				multipleTypeList = crossCheckRow["MultipleLocationTypeList"].AsString();
			}
			else
			{
				commonTypeList = null;
				multipleTypeList = null;
			}
		}


		/// <summary>
		/// Returns values from a Program Parameter to Method Code cross check row based on the passed Program Parameter code.
		/// </summary>
		/// <param name="programParameterCd">The program parameter code to locate in the cross check row.</param>
		/// <param name="methodCodeList">The method code list of the cross check row.</param>
		private void Program11_GetProgramParameterToMethodCodeValues(string programParameterCd,
																	 out string methodCodeList)
		{
			DataRowView crossCheckRow;

			if (cRowFilter.FindRow(MpParameters.CrosscheckProgramparametertomethodcode.SourceView,
								   new cFilterCondition[] { new cFilterCondition("ProgramParameterCd", programParameterCd) },
								   out crossCheckRow))
			{
				methodCodeList = crossCheckRow["MethodCdList"].AsString();
			}
			else
			{
				methodCodeList = null;
			}
		}


		/// <summary>
		/// Returns values from a Program Parameter to Severity Code cross check row based on the passed Program Parameter code.
		/// </summary>
		/// <param name="programParameterCd">The program parameter code to locate in the cross check row.</param>
		/// <param name="severityCd">The severity code from the cross check row.</param>
		private void Program11_GetProgramParameterToSeverityValue(string programParameterCd,
																   out string severityCd)
		{
			DataRowView crossCheckRow;

			if (cRowFilter.FindRow(MpParameters.CrosscheckProgramparametertoseverity.SourceView,
								   new cFilterCondition[] { new cFilterCondition("ProgramParameterCd", programParameterCd) },
								   out crossCheckRow))
			{
				severityCd = crossCheckRow["SeverityCd"].AsString();
			}
			else
			{
				severityCd = null;
			}
		}


		/// <summary>
		/// This method returns true if an active Unit Stack Configuration exists for the passed location types.
		/// </summary>
		/// <param name="unitMonLocId">The Unit Monitor Location.</param>
		/// <param name="dateRangeBegin">The Begin Date Range.</param>
		/// <param name="dateRangeEnd">The End Date Range.</param>
		/// <param name="locationTypesArray">One or more location type lists for location to include.</param>
		/// <returns></returns>
		private bool Program11_UnitStackConfigurationExists(string unitMonLocId,
															DateTime dateRangeBegin,
															DateTime dateRangeEnd,
															params string[] locationTypesArray)
		{
			bool result;

			string locationTypeList = null;
			string delim = null;

			foreach (string locationTypes in locationTypesArray)
			{
				if (locationTypes.IsNotEmpty())
				{
					locationTypeList = string.Format("{0}{1}{2}", locationTypeList, delim, locationTypes);
					delim = ",";
				}
			}

			result = false;
			{
				DataView unitStackConfigurationView;
				{
					unitStackConfigurationView
					  = cRowFilter.FindActiveRows(MpParameters.UnitStackConfigurationRecords.SourceView,
												  dateRangeBegin,
												  dateRangeEnd,
												  new cFilterCondition[] 
                                        { 
                                          new cFilterCondition("Mon_Loc_Id", unitMonLocId),
                                          new cFilterCondition("Stack_Name", locationTypeList, eFilterConditionStringCompare.InList, 0, 2) 
                                        });
					if (unitStackConfigurationView.Count > 0)
						result = true;
				}
			}

			return result;
		}


		/// <summary>
		/// This method returns the list of MON_LOC_ID for the unit in the
		/// current program row and its associated stacks and pipes.
		/// </summary>
		/// <param name="unitMonLocId">The unit MON_LOC_ID.</param>
		/// <param name="dateRangeBegin">The Begin Date Range.</param>
		/// <param name="dateRangeEnd">The End Date Range.</param>
		/// <param name="locationTypesArray">One or more location type lists for location to include.</param>
		/// <returns></returns>
		private string Program11_UnitStackConfigurationMonLocList(string unitMonLocId,
																  DateTime dateRangeBegin,
																  DateTime dateRangeEnd,
																  params string[] locationTypesArray)
		{
			string result;

			string locationTypeList = null;
			string delim = null;

			foreach (string locationTypes in locationTypesArray)
			{
				if (locationTypes.IsNotEmpty())
				{
					locationTypeList = string.Format("{0}{1}{2}", locationTypeList, delim, locationTypes);
					delim = ",";
				}
			}

			//always get the current unit
			result = unitMonLocId;
			{
				DataView unitStackConfigurationView;
				{
					unitStackConfigurationView
					  = cRowFilter.FindActiveRows(MpParameters.UnitStackConfigurationRecords.SourceView,
												  dateRangeBegin,
												  dateRangeEnd,
												  new cFilterCondition[] 
                                        { 
                                          new cFilterCondition("Mon_Loc_Id", unitMonLocId),
                                          new cFilterCondition("Stack_Name", locationTypeList, eFilterConditionStringCompare.InList, 0, 2) 
                                        });

					//add associated units if they meet the location type list criteria
					if (unitStackConfigurationView.Count > 0)
						result += "," + ColumnToDatalist(unitStackConfigurationView, "Stack_Pipe_Mon_Loc_Id");
				}
			}

			return result;
		}


		/// <summary>
		/// This method returns the list of MON_LOC_ID for the unit in the
		/// current program row and its associated stacks and pipes.
		/// </summary>
		/// <param name="unitMonLocId">The unit MON_LOC_ID.</param>
		/// <param name="opHour">The Op Hour.</param>
		/// <param name="locationTypesArray">One or more location type lists for location to include.</param>
		/// <returns></returns>
		private string Program11_UnitStackConfigurationMonLocList(string unitMonLocId,
																  DateTime opHour,
																  params string[] locationTypesArray)
		{
			string result;

			string locationTypeList = null;
			string delim = null;

			foreach (string locationTypes in locationTypesArray)
			{
				if (locationTypes.IsNotEmpty())
				{
					locationTypeList = string.Format("{0}{1}{2}", locationTypeList, delim, locationTypes);
					delim = ",";
				}
			}

			result = unitMonLocId;
			{
				DataView unitStackConfigurationView;
				{
					unitStackConfigurationView
					  = cRowFilter.FindActiveRows(MpParameters.UnitStackConfigurationRecords.SourceView,
												  opHour.Date,
												  new cFilterCondition[] 
                                        { 
                                          new cFilterCondition("Mon_Loc_Id", unitMonLocId),
                                          new cFilterCondition("Stack_Name", locationTypeList, eFilterConditionStringCompare.InList, 0, 2) 
                                        });

					if (unitStackConfigurationView.Count > 0)
						result += "," + ColumnToDatalist(unitStackConfigurationView, "Stack_Pipe_Mon_Loc_Id");
				}
			}

			return result;
		}


		/// <summary>
		/// This method returns the list of MON_LOC_ID for the unit in the
		/// current program row and its associated stacks and pipes.
		/// </summary>
		/// <param name="ARowFilterList">Filters to apply to the Unit Stack Configuration Records parameters</param>
		/// <returns></returns>
		private string Program11_UnitStackConfigurationStackPipeLocList(params cFilterCondition[][] ARowFilterList)
		{
			string result = null;

			string delim = null;

			DataView unitStackConfigurationView;
			{
				unitStackConfigurationView = cRowFilter.FindActiveRows(MpParameters.UnitStackConfigurationRecords.SourceView,
																	   ProgramParameterEvaluationBeginDate.AsBeginDateTime(),
																	   ProgramParameterEvaluationEndDate.AsEndDateTime(),
																	   ARowFilterList);

				if (unitStackConfigurationView.Count > 0)
				{
					result += delim + ColumnToDatalist(unitStackConfigurationView, "Stack_Pipe_Mon_Loc_Id");
					delim = ",";
				}
			}

			return result;
		}

		#endregion

		/*

    #region Utility Methods and Types

    /// <summary>
    /// Provides a generic check of method coverage for a particular program.
    /// 
    /// The calling check should catch any exceptions.
    /// </summary>
    /// <param name="currentProgram">The current Program DataRowView.</param>
    /// <param name="unitStackConfigurationRecords">The current Unit Stack Configuration DataRowView potentially limited by location type.</param>
    /// <param name="facilityMethodRecords">The current Facility Method DataRowView potentially limited by parameter or potentially method.</param>
    /// <param name="category">The check category object.</param>
    public static void ProgramMethodCheck(DataRowView currentProgram,
                                          DataView unitStackConfigurationRecords,
                                          DataView facilityMethodRecords,
                                          cCategory category)
    {
      DateTime programEvaluationBeginDate = (DateTime)category.GetCheckParameter("Program_Evaluation_Begin_Date").ParameterValue;
      DateTime programEvaluationEndDate = (DateTime)category.GetCheckParameter("Program_Evaluation_End_Date").ParameterValue;

      string unitMonLocId = currentProgram["MON_LOC_ID"].AsString();

      DataView facilityMethodView;


      // Locate all Unit Stack Configuration
      DataView unitStackConfigurationForPrgEval
        = cRowFilter.FindActiveRows(unitStackConfigurationRecords,
                                    programEvaluationBeginDate,
                                    programEvaluationEndDate,
                                    new cFilterCondition[] { new cFilterCondition("Mon_Loc_Id", currentProgram["Mon_Loc_Id"].AsString()) });

      // Location Monitor Methods for Unit and connected Common Stacks and Pipes
      {
        string unitCommonLocList = ProgramMethodCheck_GetMonLocList(true, unitMonLocId, unitStackConfigurationForPrgEval);

        facilityMethodView
          = cRowFilter.FindActiveRows(facilityMethodRecords,
                                      programEvaluationBeginDate,
                                      programEvaluationEndDate,
                                      new cFilterCondition[] { new cFilterCondition("Mon_Loc_Id", unitCommonLocList, eFilterConditionStringCompare.InList) });
      }

      // Check No Methods or Do Not Span
      if ((facilityMethodView.Count == 0) ||
          !CheckForHourRangeCovered(category, facilityMethodView, programEvaluationBeginDate, 23, programEvaluationEndDate, 0))
      {
        // Check for MS or MP
        if (!ProgramMethodCheck_HasMultiple(unitStackConfigurationForPrgEval))
        {
          if (facilityMethodView.Count == 0)
            category.CheckCatalogResult = "A";
          else
            category.CheckCatalogResult = "B";
        }
        else
        {
          string allLocList = ProgramMethodCheck_GetMonLocList(false, unitMonLocId, unitStackConfigurationForPrgEval);

          // Location Monitor Methods for Unit and all connected Stacks and Pipes
          {
            facilityMethodView
              = cRowFilter.FindActiveRows(facilityMethodRecords,
                                          programEvaluationBeginDate,
                                          programEvaluationEndDate,
                                          new cFilterCondition[] { new cFilterCondition("Mon_Loc_Id", allLocList, eFilterConditionStringCompare.InList) });
          }

          // Check for no methods or do not span, even with MS and MP
          if (facilityMethodView.Count == 0)
            category.CheckCatalogResult = "A";
          else if (!CheckForHourRangeCovered(category, facilityMethodView, programEvaluationBeginDate, 23, programEvaluationEndDate, 0))
            category.CheckCatalogResult = "B";
          else
          {
            for (DateTime hour = programEvaluationBeginDate.AddHours(23);
                 hour <= programEvaluationEndDate;
                 hour = hour.AddHours(1))
            {
              DataView unitStackConfigurationView
                    = cRowFilter.FindActiveRows(unitStackConfigurationForPrgEval, hour.Date);

              string hourLocList = ProgramMethodCheck_GetMonLocList(false, unitMonLocId, unitStackConfigurationView);

              facilityMethodView
                = cRowFilter.FindActiveRows(facilityMethodRecords,
                                            hour.Date, hour.Hour,
                                            new cFilterCondition[] { new cFilterCondition("Mon_Loc_Id", hourLocList, eFilterConditionStringCompare.InList) });


              if (ProgramMethodCheck_UnitCommonMethodExists(unitMonLocId, unitStackConfigurationView, facilityMethodView))
              {
                // Check next hour
              }
              else
              {
                // Determine USC filter based on hour being checked.
                DataView unitStackConfigurationMpCountView;
                {
                  cFilterCondition[] uscMpCountFilter;

                  if (hour.Hour == 0)
                  {
                    uscMpCountFilter
                      = new cFilterCondition[] 
                        { 
                          new cFilterCondition("BEGIN_DATE", hour.Date, eFilterDataType.DateBegan, eFilterConditionRelativeCompare.LessThan) 
                        };
                  }
                  else if (hour.Hour == 23)
                  {
                    uscMpCountFilter
                      = new cFilterCondition[] 
                        { 
                          new cFilterCondition("END_DATE", hour.Date, eFilterDataType.DateEnded, eFilterConditionRelativeCompare.GreaterThan) 
                        };
                  }
                  else
                  {
                    uscMpCountFilter
                      = new cFilterCondition[] 
                        { 
                          new cFilterCondition("BEGIN_DATE", hour.Date, eFilterDataType.DateBegan, eFilterConditionRelativeCompare.LessThan),
                          new cFilterCondition("END_DATE", hour.Date, eFilterDataType.DateEnded, eFilterConditionRelativeCompare.GreaterThan) 
                        };
                  }

                  unitStackConfigurationMpCountView = cRowFilter.FindRows(unitStackConfigurationView, uscMpCountFilter);
                }

                // Get a check multiple stack and pipe counts
                {
                  int multiplePipeCount, multiplePipeMethod, multipleStackCount, multipleStackMethod;

                  ProgramMethodCheck_MultipleCounts(unitStackConfigurationMpCountView,
                                                    facilityMethodView,
                                                    out multipleStackCount,
                                                    out multipleStackMethod,
                                                    out multiplePipeCount,
                                                    out multiplePipeMethod);

                  if ((multipleStackMethod > 0) && (multipleStackMethod != multipleStackCount))
                  {
                    category.CheckCatalogResult = "B";
                    break;
                  }
                  else if ((multiplePipeMethod > 0) && (multiplePipeMethod != multiplePipeCount))
                  {
                    category.CheckCatalogResult = "B";
                    break;
                  }
                  else
                  {
                    // Check next hour
                  }
                }
              }
            }
          }
        }
      }
    }


    /// <summary>
    /// This method returns the list of MON_LOC_ID for the unit in the
    /// current program row and its associated stacks and pipes.
    /// 
    /// The method will exclude multiple stacks and pipes depending on
    /// the value of unitAndCommonOnly attribute.
    /// </summary>
    /// <param name="unitAndCommonOnly">Excludes multiple stacks and pipes if true.</param>
    /// <param name="unitMonLocId">The unit MON_LOC_ID.</param>
    /// <param name="unitStackConfigurationForPrgEval">The appropriate unit stack configuration rows.</param>
    /// <returns></returns>
    private static string ProgramMethodCheck_GetMonLocList(bool unitAndCommonOnly,
                                                           string unitMonLocId,
                                                           DataView unitStackConfigurationForPrgEval)
    {
      string result;
      {
        DataView unitStackConfigurationView;
        {
          if (unitAndCommonOnly)
          {
            unitStackConfigurationView
              = cRowFilter.FindRows(unitStackConfigurationForPrgEval,
                                    new cFilterCondition[] { new cFilterCondition("Stack_Name", "CS,CP", eFilterConditionStringCompare.InList, 0, 2) });
          }
          else
            unitStackConfigurationView = unitStackConfigurationForPrgEval;
        }

        result = unitMonLocId;

        if (unitStackConfigurationView.Count > 0)
          result += "," + ColumnToDatalist(unitStackConfigurationView, "Stack_Pipe_Mon_Loc_Id");
      }

      return result;
    }


    /// <summary>
    /// Returns true if the passed Unit Stack Configuration rows contain a multiple stack or pipe.
    /// </summary>
    /// <param name="unitStackConfigurationForPrgEval">The view containing the Unit Stack Configuration rows.</param>
    /// <returns>True if multiple stack and pipes exist.</returns>
    private static bool ProgramMethodCheck_HasMultiple(DataView unitStackConfigurationForPrgEval)
    {
      DataView unitStackConfigurationView 
        = cRowFilter.FindRows(unitStackConfigurationForPrgEval,
                              new cFilterCondition[] { new cFilterCondition("Stack_Name", "MS,MP", eFilterConditionStringCompare.InList, 0, 2) });

      return (unitStackConfigurationView.Count > 0);
    }


    /// <summary>
    /// Returns counts for Multiple Stacks and Multiple Pipes as well as counts of those
    /// that have methods, all based on the passed Unit Stack Configuration and Facility Method views.
    /// 
    /// The procedure assumes that the Unit Stack Configuration and Facility Method views only
    /// contain the rows appropriate for the counting and comparison.
    /// </summary>
    /// <param name="unitStackConfigurationMpCountView">he appropriate unit stack configuration rows.</param>
    /// <param name="facilityMethodView">The appropriate facility method rows.</param>
    /// <param name="multipleStackCount">The count of MS in the Unit Stack Configuration view.</param>
    /// <param name="multipleStackMethod">The count of the MS in the USC that have methods.</param>
    /// <param name="multiplePipeCount">The count of MP in the Unit Stack Configuration view.</param>
    /// <param name="multiplePipeMethod">The count of the MP in the USC that have methods.</param>
    private static void ProgramMethodCheck_MultipleCounts(DataView unitStackConfigurationMpCountView,
                                                          DataView facilityMethodView,
                                                          out int multipleStackCount,
                                                          out int multipleStackMethod,
                                                          out int multiplePipeCount,
                                                          out int multiplePipeMethod)
    {
      multipleStackCount = 0;
      multipleStackMethod = 0;
      multiplePipeCount = 0;
      multiplePipeMethod = 0;

      // Check Stacks/Pipes if unit method not found
      foreach (DataRowView unitStackConfigurationRow in unitStackConfigurationMpCountView)
      {
        bool methodFound = false;

        foreach (DataRowView monitorMethodRow in facilityMethodView)
        {
          if (unitStackConfigurationRow["STACK_PIPE_MON_LOC_ID"].AsString() == monitorMethodRow["MON_LOC_ID"].AsString())
          {
            methodFound = true;
            break;
          }
        }

        switch (unitStackConfigurationRow["STACK_NAME"].AsString().Substring(0, 2))
        {
          case "MS": { multipleStackCount += 1; if (methodFound) multipleStackMethod += 1; } break;
          case "MP": { multiplePipeCount += 1; if (methodFound) multiplePipeMethod += 1; } break;
        }
      }
    }


    /// <summary>
    /// Determines whether a unit, common stack or common pipe method exists based on the passed
    /// Unit MON_LOC_ID, Unit Stack Configuration view and Facility Method view.
    /// 
    /// The procedure assumes that the Unit Stack Configuration and Facility Method views only
    /// contain the rows appropriate for the determination.
    /// </summary>
    /// <param name="unitMonLocId">The MON_LOC_ID for the Current Unit Program row.</param>
    /// <param name="unitStackConfigurationView">he appropriate unit stack configuration rows.</param>
    /// <param name="facilityMethodView">he appropriate facility method rows.</param>
    /// <returns>True when either unit or common stack method was found.</returns>
    private static bool ProgramMethodCheck_UnitCommonMethodExists(string unitMonLocId,
                                                                  DataView unitStackConfigurationView,
                                                                  DataView facilityMethodView)
    {
      bool result = false;

      // Check for unit method
      {
        bool methodFound = false;

        foreach (DataRowView monitorMethodRow in facilityMethodView)
        {
          if (unitMonLocId == monitorMethodRow["MON_LOC_ID"].AsString())
          {
            methodFound = true;
            break;
          }
        }

        if (methodFound) result = true;
      }

      // Check Common Stacks/Pipes if unit method not found
      if (!result)
      {
        foreach (DataRowView unitStackConfigurationRow in unitStackConfigurationView)
        {
          bool methodFound = false;

          if (unitStackConfigurationRow["STACK_NAME"].AsString().Substring(0, 2).InList("CS,CP"))
          {
            foreach (DataRowView monitorMethodRow in facilityMethodView)
            {
              if (unitStackConfigurationRow["STACK_PIPE_MON_LOC_ID"].AsString() == monitorMethodRow["MON_LOC_ID"].AsString())
              {
                methodFound = true;
                break;
              }
            }

            if (methodFound) result = true;
          }
        }
      }

      return result;
    }

    #endregion

    */

		/*
     
		#region Old Versions

		#region Obsolete

		/// <summary>
		/// PROGRAM-2: Required NOXR Method Reported for ARP Affected Units
		/// </summary>
		/// <param name="category">The calling check category object.</param>
		/// <param name="log">The log messages flag.</param>
		/// <returns>Returns formatted exception information.</returns>
		public static string PROGRAM2(cCategory category, ref bool log)
		{
		  string returnVal = "";

		  try
		  {
			DataRowView currentProgram = category.GetCheckParameter("Current_Program").AsDataRowView();

			if ((currentProgram["Prg_Cd"].AsString() == "ARP") && (currentProgram["Class"].AsString() != "NA"))
			{
			  DataView unitStackConfigurationRecords = category.GetCheckParameter("Unit_Stack_Configuration_Records").AsDataView();

			  DataView facilityMethodView;
			  {
				DataView facilityMethodRecords = category.GetCheckParameter("Facility_Method_Records").AsDataView();

				facilityMethodView
				  = cRowFilter.FindRows(facilityMethodRecords,
										new cFilterCondition[] { new cFilterCondition("Parameter_Cd", "NOXR,NOXM", eFilterConditionStringCompare.InList) });
			  }

			  ProgramMethodCheck(currentProgram, unitStackConfigurationRecords, facilityMethodView, category);
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

		/// <summary>
		/// PROGRAM-3: Required NOX Method Reported for NOx Trading Program Affected Units
		/// </summary>
		/// <param name="category">The calling check category object.</param>
		/// <param name="log">The log messages flag.</param>
		/// <returns>Returns formatted exception information.</returns>
		public static string PROGRAM3(cCategory category, ref bool log)
		{
		  string returnVal = "";

		  try
		  {
			DataRowView currentProgram = category.GetCheckParameter("Current_Program").AsDataRowView();

			if (currentProgram["Prg_Cd"].AsString().InList("NBP,NHNOX,CAIRNOX,CAIROS,SIPNOX") &&
				currentProgram["Class"].AsString().NotInList("N,NB"))
			{
			  DataView unitStackConfigurationRecords = category.GetCheckParameter("Unit_Stack_Configuration_Records").AsDataView();

			  DataView facilityMethodView;
			  {
				DataView facilityMethodRecords = category.GetCheckParameter("Facility_Method_Records").AsDataView();

				facilityMethodView
				  = cRowFilter.FindRows(facilityMethodRecords,
										new cFilterCondition[] { new cFilterCondition("Parameter_Cd", "NOX,NOXM", eFilterConditionStringCompare.InList) });
			  }

			  ProgramMethodCheck(currentProgram, unitStackConfigurationRecords, facilityMethodView, category);
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

		/// <summary>
		/// PROGRAM-5: Required OP Method Reported for ARP Affected Units
		/// </summary>
		/// <param name="category">The calling check category object.</param>
		/// <param name="log">The log messages flag.</param>
		/// <returns>Returns formatted exception information.</returns>
		public static string PROGRAM5(cCategory category, ref bool log)
		{
		  string returnVal = "";

		  try
		  {
			DataRowView currentProgram = category.GetCheckParameter("Current_Program").AsDataRowView();

			if ((currentProgram["Prg_Cd"].AsString() == "ARP") && (currentProgram["Class"].AsString() != "NA"))
			{
			  DataView unitStackConfigurationView;
			  {
				DataView unitStackConfigurationRecords = category.GetCheckParameter("Unit_Stack_Configuration_Records").AsDataView();

				unitStackConfigurationView
				  = cRowFilter.FindRows(unitStackConfigurationRecords,
										new cFilterCondition[] { new cFilterCondition("Stack_Name", "CS,MS", eFilterConditionStringCompare.InList, 0, 2) });
			  }

			  DataView facilityMethodView;
			  {
				DataView facilityMethodRecords = category.GetCheckParameter("Facility_Method_Records").AsDataView();

				facilityMethodView
				  = cRowFilter.FindRows(facilityMethodRecords,
										new cFilterCondition[] { new cFilterCondition("Parameter_Cd", "OP") },
										new cFilterCondition[] { new cFilterCondition("Method_Cd", "LME") });
			  }

			  ProgramMethodCheck(currentProgram, unitStackConfigurationView, facilityMethodView, category);
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

		/// <summary>
		/// PROGRAM-6: Required SO2 Method Reported for SO2 Trading Program Affected Units
		/// </summary>
		/// <param name="category">The calling check category object.</param>
		/// <param name="log">The log messages flag.</param>
		/// <returns>Returns formatted exception information.</returns>
		public static string PROGRAM6(cCategory category, ref bool log)
		{
		  string returnVal = "";

		  try
		  {
			DataRowView currentProgram = category.GetCheckParameter("Current_Program").AsDataRowView();

			if (((currentProgram["Prg_Cd"].AsString() == "ARP") && (currentProgram["Class"].AsString() != "NA")) ||
				((currentProgram["Prg_Cd"].AsString() == "CAIRSO2") && (currentProgram["Class"].AsString() != "N")))
			{
			  DataView unitStackConfigurationRecords = category.GetCheckParameter("Unit_Stack_Configuration_Records").AsDataView();

			  DataView facilityMethodView;
			  {
				DataView facilityMethodRecords = category.GetCheckParameter("Facility_Method_Records").AsDataView();

				facilityMethodView
				  = cRowFilter.FindRows(facilityMethodRecords,
										new cFilterCondition[] { new cFilterCondition("Parameter_Cd", "SO2,SO2M", eFilterConditionStringCompare.InList) });
			  }

			  ProgramMethodCheck(currentProgram, unitStackConfigurationRecords, facilityMethodView, category);
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

		/// <summary>
		/// PROGRAM-8: Required CO2 Method Reported for Affected Units
		/// </summary>
		/// <param name="category">The calling check category object.</param>
		/// <param name="log">The log messages flag.</param>
		/// <returns>Returns formatted exception information.</returns>
		public static string PROGRAM8(cCategory category, ref bool log)
		{
		  string returnVal = "";

		  try
		  {
			DataRowView currentProgram = category.GetCheckParameter("Current_Program").AsDataRowView();

			if (((currentProgram["Prg_Cd"].AsString() == "ARP") && (currentProgram["Class"].AsString() != "NA")) ||
				((currentProgram["Prg_Cd"].AsString() == "RGGI") && (currentProgram["Class"].AsString() != "N")))
			{
			  DataView unitStackConfigurationRecords = category.GetCheckParameter("Unit_Stack_Configuration_Records").AsDataView();

			  DataView facilityMethodView;
			  {
				DataView facilityMethodRecords = category.GetCheckParameter("Facility_Method_Records").AsDataView();

				facilityMethodView
				  = cRowFilter.FindRows(facilityMethodRecords,
										new cFilterCondition[] { new cFilterCondition("Parameter_Cd", "CO2,CO2M", eFilterConditionStringCompare.InList) });
			  }

			  ProgramMethodCheck(currentProgram, unitStackConfigurationRecords, facilityMethodView, category);
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

		/// <summary>
		/// PROGRAM-9: Required HI Method Reported for Affected Units
		/// </summary>
		/// <param name="category">The calling check category object.</param>
		/// <param name="log">The log messages flag.</param>
		/// <returns>Returns formatted exception information.</returns>
		public static string PROGRAM9(cCategory category, ref bool log)
		{
		  string returnVal = "";

		  try
		  {
			DataRowView currentProgram = category.GetCheckParameter("Current_Program").AsDataRowView();

			if (currentProgram["Class"].AsString().NotInList("N,NA,NB"))
			{
			  DataView unitStackConfigurationRecords = category.GetCheckParameter("Unit_Stack_Configuration_Records").AsDataView();

			  DataView facilityMethodView;
			  {
				DataView facilityMethodRecords = category.GetCheckParameter("Facility_Method_Records").AsDataView();

				facilityMethodView
				  = cRowFilter.FindRows(facilityMethodRecords,
										new cFilterCondition[] { new cFilterCondition("Parameter_Cd", "HI,HIT", eFilterConditionStringCompare.InList) });
			  }

			  ProgramMethodCheck(currentProgram, unitStackConfigurationRecords, facilityMethodView, category);
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

		#endregion


		#region Replaced

		public static string PROGRAM2_old(cCategory category, ref bool log)
		// Required Methods Reported for ARP Affected Units
		{
		  string ReturnVal = "";

		  try
		  {
			DataRowView CurrentProgram = (DataRowView)category.GetCheckParameter("Current_Program").ParameterValue;

			if (cDBConvert.ToString(CurrentProgram["Prg_Cd"]) == "ARP" && cDBConvert.ToString(CurrentProgram["Class"]) != "NA")
			{
			  DateTime ProgramEvalBeganDate = (DateTime)category.GetCheckParameter("Program_Evaluation_Begin_Date").ParameterValue;
			  DateTime ProgramEvalEndedDate = (DateTime)category.GetCheckParameter("Program_Evaluation_End_Date").ParameterValue;
			  DataView ConfigurationView = (DataView)category.GetCheckParameter("Unit_Stack_Configuration_Records").ParameterValue;
			  DataView FacilityMethodView = (DataView)category.GetCheckParameter("Facility_Method_Records").ParameterValue;

			  eMethodStatus MethodStatus;

			  string MissingList = ""; string MissingDelim = "";
			  string IncompleteList = ""; string IncompleteDelim = "";


			  //Handle Heat Input
			  MethodStatus = DetermineMethodStatus("HI,HIT", "", false, true,
												   ProgramEvalBeganDate,
												   ProgramEvalEndedDate,
												   CurrentProgram,
												   FacilityMethodView,
												   ConfigurationView,
												   category);

			  if (MethodStatus == eMethodStatus.Missing)
			  {
				MissingList += MissingDelim + "HI (or HIT for LME units)";
				MissingDelim = ",";
			  }
			  else if (MethodStatus == eMethodStatus.Incomplete)
			  {
				IncompleteList += IncompleteDelim + "HI (or HIT for LME units)";
				IncompleteDelim = ",";
			  }


			  //Handle SO2
			  MethodStatus = DetermineMethodStatus("SO2,SO2M", "", false, true,
												   ProgramEvalBeganDate,
												   ProgramEvalEndedDate,
												   CurrentProgram,
												   FacilityMethodView,
												   ConfigurationView,
												   category);

			  if (MethodStatus == eMethodStatus.Missing)
			  {
				MissingList += MissingDelim + "SO2 (or SO2M for LME units)";
				MissingDelim = ",";
			  }
			  else if (MethodStatus == eMethodStatus.Incomplete)
			  {
				IncompleteList += IncompleteDelim + "SO2 (or SO2M for LME units)";
				IncompleteDelim = ",";
			  }


			  //Handle NOx
			  MethodStatus = DetermineMethodStatus("NOXR,NOXM", "", false, true,
												   ProgramEvalBeganDate,
												   ProgramEvalEndedDate,
												   CurrentProgram,
												   FacilityMethodView,
												   ConfigurationView,
												   category);

			  if (MethodStatus == eMethodStatus.Missing)
			  {
				MissingList += MissingDelim + "NOXR (or NOXM for LME units)";
				MissingDelim = ",";
			  }
			  else if (MethodStatus == eMethodStatus.Incomplete)
			  {
				IncompleteList += IncompleteDelim + "NOXR (or NOXM for LME units)";
				IncompleteDelim = ",";
			  }


			  //Handle CO2
			  MethodStatus = DetermineMethodStatus("CO2,CO2M", "", false, true,
												   ProgramEvalBeganDate,
												   ProgramEvalEndedDate,
												   CurrentProgram,
												   FacilityMethodView,
												   ConfigurationView,
												   category);

			  if (MethodStatus == eMethodStatus.Missing)
			  {
				MissingList += MissingDelim + "CO2 (or CO2M)";
				MissingDelim = ",";
			  }
			  else if (MethodStatus == eMethodStatus.Incomplete)
			  {
				IncompleteList += IncompleteDelim + "CO2 (or CO2M)";
				IncompleteDelim = ",";
			  }


			  //Set Output Parameters
			  category.SetCheckParameter("Missing_ARP_Method_List", MissingList.FormatList(), eParameterDataType.String);
			  category.SetCheckParameter("Incomplete_ARP_Method_List", IncompleteList.FormatList(), eParameterDataType.String);


			  //Produce Results
			  if ((MissingList != "") && IncompleteList == "")
				category.CheckCatalogResult = "A";
			  else if ((MissingList == "") && IncompleteList != "")
				category.CheckCatalogResult = "B";
			  else if ((MissingList != "") && IncompleteList != "")
				category.CheckCatalogResult = "C";
			}
			else
			  log = false;
		  }
		  catch (Exception ex)
		  {
			ReturnVal = category.CheckEngine.FormatError(ex, "PROGRAM2");
		  }

		  return ReturnVal;
		}

		public static string PROGRAM3_old(cCategory Category, ref bool Log)
		// Required Methods Reported for NBP Affected Units
		{
		  string ReturnVal = "";

		  try
		  {
			DataRowView CurrentProgram = (DataRowView)Category.GetCheckParameter("Current_Program").ParameterValue;

			if (cDBConvert.ToString(CurrentProgram["Prg_Cd"]).InList("NBP,NHNOX,CAIRNOX,CAIROS,SIPNOX") &&
				!cDBConvert.ToString(CurrentProgram["Class"]).InList("N", "NB"))
			{
			  DateTime ProgramEvalBeganDate = (DateTime)Category.GetCheckParameter("Program_Evaluation_Begin_Date").ParameterValue;
			  DateTime ProgramEvalEndedDate = (DateTime)Category.GetCheckParameter("Program_Evaluation_End_Date").ParameterValue;
			  DataView ConfigurationView = (DataView)Category.GetCheckParameter("Unit_Stack_Configuration_Records").ParameterValue;
			  DataView FacilityMethodView = (DataView)Category.GetCheckParameter("Facility_Method_Records").ParameterValue;

			  eMethodStatus MethodStatus;

			  string MissingList = ""; string MissingDelim = "";
			  string IncompleteList = ""; string IncompleteDelim = "";


			  //Handle NOx
			  MethodStatus = DetermineMethodStatus("NOX,NOXM", "", false, true,
												   ProgramEvalBeganDate,
												   ProgramEvalEndedDate,
												   CurrentProgram,
												   FacilityMethodView,
												   ConfigurationView,
												   Category);

			  if (MethodStatus == eMethodStatus.Missing)
			  {
				MissingList += MissingDelim + "NOX (or NOXM for LME units)";
				MissingDelim = ",";
			  }
			  else if (MethodStatus == eMethodStatus.Incomplete)
			  {
				IncompleteList += IncompleteDelim + "NOX (or NOXM for LME units)";
				IncompleteDelim = ",";
			  }


			  //Handle Heat Input
			  MethodStatus = DetermineMethodStatus("HI,HIT", "", false, true,
												   ProgramEvalBeganDate,
												   ProgramEvalEndedDate,
												   CurrentProgram,
												   FacilityMethodView,
												   ConfigurationView,
												   Category);

			  if (MethodStatus == eMethodStatus.Missing)
			  {
				MissingList += MissingDelim + "HI (or HIT for LME units)";
				MissingDelim = ",";
			  }
			  else if (MethodStatus == eMethodStatus.Incomplete)
			  {
				IncompleteList += IncompleteDelim + "HI (or HIT for LME units)";
				IncompleteDelim = ",";
			  }


			  //Set Output Parameters
			  Category.SetCheckParameter("Missing_Nbp_Method_List", MissingList.FormatList(), eParameterDataType.String);
			  Category.SetCheckParameter("Incomplete_NBP_Method_List", IncompleteList.FormatList(), eParameterDataType.String);

			  //Produce Results
			  if ((MissingList != "") && IncompleteList == "")
				Category.CheckCatalogResult = "A";
			  else if ((MissingList == "") && IncompleteList != "")
				Category.CheckCatalogResult = "B";
			  else if ((MissingList != "") && IncompleteList != "")
				Category.CheckCatalogResult = "C";
			}
			else
			  Log = false;
		  }
		  catch (Exception ex)
		  {
			ReturnVal = Category.CheckEngine.FormatError(ex, "PROGRAM3");
		  }

		  return ReturnVal;
		}

		public static string PROGRAM5_old(cCategory Category, ref bool Log)
		// Required OP Methods Reported for ARP Affected Units
		{
		  string ReturnVal = "";

		  try
		  {
			DataRowView CurrentProgram = (DataRowView)Category.GetCheckParameter("Current_Program").ParameterValue;

			if (cDBConvert.ToString(CurrentProgram["Prg_Cd"]) == "ARP" &&
				(cDBConvert.ToString(CurrentProgram["Class"]) != "NA"))
			{
			  DateTime ProgramEvalBeganDate = (DateTime)Category.GetCheckParameter("Program_Evaluation_Begin_Date").ParameterValue;
			  DateTime ProgramEvalEndedDate = (DateTime)Category.GetCheckParameter("Program_Evaluation_End_Date").ParameterValue;
			  DataView ConfigurationView = (DataView)Category.GetCheckParameter("Unit_Stack_Configuration_Records").ParameterValue;
			  DataView FacilityMethodView = (DataView)Category.GetCheckParameter("Facility_Method_Records").ParameterValue;

			  eMethodStatus MethodStatus = DetermineMethodStatus("OP", "LME", true, false,
																 ProgramEvalBeganDate,
																 ProgramEvalEndedDate,
																 CurrentProgram,
																 FacilityMethodView,
																 ConfigurationView,
																 Category);

			  if (MethodStatus == eMethodStatus.Missing)
				Category.CheckCatalogResult = "A";
			  else if (MethodStatus == eMethodStatus.Incomplete)
				Category.CheckCatalogResult = "B";
			}
			else
			  Log = false;
		  }
		  catch (Exception ex)
		  {
			ReturnVal = Category.CheckEngine.FormatError(ex, "PROGRAM5");
		  }

		  return ReturnVal;
		}

		public static string PROGRAM6_old(cCategory Category, ref bool Log)
		// Required Methods Reported for CAIRSO2 Affected Units
		{
		  string ReturnVal = "";

		  try
		  {
			DataRowView CurrentProgram = (DataRowView)Category.GetCheckParameter("Current_Program").ParameterValue;

			if (cDBConvert.ToString(CurrentProgram["Prg_Cd"]).InList("CAIRSO2") &&
				(cDBConvert.ToString(CurrentProgram["Class"]) != "N"))
			{
			  DateTime ProgramEvalBeganDate = (DateTime)Category.GetCheckParameter("Program_Evaluation_Begin_Date").ParameterValue;
			  DateTime ProgramEvalEndedDate = (DateTime)Category.GetCheckParameter("Program_Evaluation_End_Date").ParameterValue;
			  DataView ConfigurationView = (DataView)Category.GetCheckParameter("Unit_Stack_Configuration_Records").ParameterValue;
			  DataView FacilityMethodView = (DataView)Category.GetCheckParameter("Facility_Method_Records").ParameterValue;

			  eMethodStatus MethodStatus;

			  string MissingList = ""; string MissingDelim = "";
			  string IncompleteList = ""; string IncompleteDelim = "";


			  //Handle NOx
			  MethodStatus = DetermineMethodStatus("SO2,SO2M", "", false, true,
												   ProgramEvalBeganDate,
												   ProgramEvalEndedDate,
												   CurrentProgram,
												   FacilityMethodView,
												   ConfigurationView,
												   Category);

			  if (MethodStatus == eMethodStatus.Missing)
			  {
				MissingList += MissingDelim + "SO2 (or SO2M for LME units)";
				MissingDelim = ",";
			  }
			  else if (MethodStatus == eMethodStatus.Incomplete)
			  {
				IncompleteList += IncompleteDelim + "SO2 (or SO2M for LME units)";
				IncompleteDelim = ",";
			  }


			  //Handle Heat Input
			  MethodStatus = DetermineMethodStatus("HI,HIT", "", false, true,
												   ProgramEvalBeganDate,
												   ProgramEvalEndedDate,
												   CurrentProgram,
												   FacilityMethodView,
												   ConfigurationView,
												   Category);

			  if (MethodStatus == eMethodStatus.Missing)
			  {
				MissingList += MissingDelim + "HI (or HIT for LME units)";
				MissingDelim = ",";
			  }
			  else if (MethodStatus == eMethodStatus.Incomplete)
			  {
				IncompleteList += IncompleteDelim + "HI (or HIT for LME units)";
				IncompleteDelim = ",";
			  }


			  //Set Output Parameters
			  Category.SetCheckParameter("Missing_CAIR_Method_List", MissingList.FormatList(), eParameterDataType.String);
			  Category.SetCheckParameter("Incomplete_CAIR_Method_List", IncompleteList.FormatList(), eParameterDataType.String);

			  //Produce Results
			  if ((MissingList != "") && IncompleteList == "")
				Category.CheckCatalogResult = "A";
			  else if ((MissingList == "") && IncompleteList != "")
				Category.CheckCatalogResult = "B";
			  else if ((MissingList != "") && IncompleteList != "")
				Category.CheckCatalogResult = "C";
			}
			else
			  Log = false;
		  }
		  catch (Exception ex)
		  {
			ReturnVal = Category.CheckEngine.FormatError(ex, "PROGRAM6");
		  }

		  return ReturnVal;
		}

		public static string PROGRAM8_old(cCategory Category, ref bool Log)
		// Required Methods Reported for RGGI Affected Units
		{
		  string ReturnVal = "";

		  try
		  {
			DataRowView CurrentProgram = (DataRowView)Category.GetCheckParameter("Current_Program").ParameterValue;

			if ((CurrentProgram["Prg_Cd"].AsString() == "RGGI") &&
				(CurrentProgram["Class"].AsString() != "N"))
			{
			  DateTime ProgramEvalBeganDate = (DateTime)Category.GetCheckParameter("Program_Evaluation_Begin_Date").ParameterValue;
			  DateTime ProgramEvalEndedDate = (DateTime)Category.GetCheckParameter("Program_Evaluation_End_Date").ParameterValue;
			  DataView ConfigurationView = (DataView)Category.GetCheckParameter("Unit_Stack_Configuration_Records").ParameterValue;
			  DataView FacilityMethodView = (DataView)Category.GetCheckParameter("Facility_Method_Records").ParameterValue;

			  eMethodStatus MethodStatus = DetermineMethodStatus("CO2,CO2M", "", false, true,
																 ProgramEvalBeganDate,
																 ProgramEvalEndedDate,
																 CurrentProgram,
																 FacilityMethodView,
																 ConfigurationView,
																 Category);

			  if (MethodStatus == eMethodStatus.Missing)
			  {
				Category.CheckCatalogResult = "A";
			  }
			  else if (MethodStatus == eMethodStatus.Incomplete)
			  {
				Category.CheckCatalogResult = "B";
			  }
			}
			else
			  Log = false;
		  }
		  catch (Exception ex)
		  {
			ReturnVal = Category.CheckEngine.FormatError(ex, "PROGRAM8");
		  }

		  return ReturnVal;
		}


		#region Program Checks Utility Methods and Types

		private enum eMethodStatus { Missing, Incomplete, Complete };

		private static eMethodStatus DetermineMethodStatus(string AParameterCodeList,
														   string AMethodCodeList,
														   bool AOrParameterAndMethod,
														   bool AIncludePipe,
														   DateTime AProgramEvalBeganDate,
														   DateTime AProgramEvalEndedDate,
														   DataRowView AProgramRow,
														   DataView AFacilityMethodView,
														   DataView AConfigurationView,
														   cCategory ACategory)
		{
		  sFilterPair[] Filter;

		  Filter = new sFilterPair[1];
		  Filter[0].Set("Mon_Loc_Id", cDBConvert.ToString(AProgramRow["Mon_Loc_Id"]));

		  DataView ConfigurationRows = FindActiveRows(AConfigurationView,
													  AProgramEvalBeganDate, AProgramEvalEndedDate,
													  Filter);

		  eUnitCommonMethodStatus CommonStackStatus = DeterimeUnitCommonMethodStatus(AParameterCodeList, 
																					 AMethodCodeList,
																					 AOrParameterAndMethod,
																					 AIncludePipe,
																					 AProgramEvalBeganDate,
																					 AProgramEvalEndedDate,
																					 AProgramRow,
																					 AFacilityMethodView,
																					 ConfigurationRows,
																					 ACategory);

		  if (CommonStackStatus == eUnitCommonMethodStatus.Missing)
		  {
			eMultipleMethodStatus MultipleStackStatus = DeterimeMultipleMethodStatus(AParameterCodeList,
																					 AIncludePipe,
																					 AProgramEvalBeganDate,
																					 AProgramEvalEndedDate,
																					 AProgramRow,
																					 AFacilityMethodView,
																					 ConfigurationRows,
																					 ACategory);

			if (MultipleStackStatus == eMultipleMethodStatus.NoMultiple)
			  return eMethodStatus.Missing;
			else if (MultipleStackStatus == eMultipleMethodStatus.Missing)
			  return eMethodStatus.Missing;
			else if (MultipleStackStatus == eMultipleMethodStatus.Incomplete)
			  return eMethodStatus.Incomplete;
			else
			  return eMethodStatus.Complete;
		  }
		  else if (CommonStackStatus == eUnitCommonMethodStatus.Incomplete)
		  {
			eMultipleMethodStatus MultipleStackStatus = DeterimeMultipleMethodStatus(AParameterCodeList,
																					 AIncludePipe,
																					 AProgramEvalBeganDate,
																					 AProgramEvalEndedDate,
																					 AProgramRow,
																					 AFacilityMethodView,
																					 ConfigurationRows,
																					 ACategory);

			if (MultipleStackStatus == eMultipleMethodStatus.NoMultiple)
			  return eMethodStatus.Incomplete;
			else if (MultipleStackStatus == eMultipleMethodStatus.Missing)
			  return eMethodStatus.Incomplete;
			else if (MultipleStackStatus == eMultipleMethodStatus.Incomplete)
			  return eMethodStatus.Incomplete;
			else
			  return eMethodStatus.Complete;
		  }
		  else
			return eMethodStatus.Complete;
		}

		private enum eUnitCommonMethodStatus { Missing, Incomplete, Complete };

		private static eUnitCommonMethodStatus DeterimeUnitCommonMethodStatus(string AParameterCodeList,
																			  string AMethodCodeList,
																			  bool AOrParameterAndMethod,
																			  bool AIncludePipe,
																			  DateTime AProgramEvalBeganDate,
																			  DateTime AProgramEvalEndedDate,
																			  DataRowView AProgramRow,
																			  DataView AFacilityMethodView,
																			  DataView AConfigurationView,
																			  cCategory ACategory)
		{
		  string StackPipeTypeList = AIncludePipe ? "CS,CP" : "CS";
		  sFilterPair[] RowFilter;

		  RowFilter = new sFilterPair[1];
		  RowFilter[0].Set("Stack_Name", StackPipeTypeList, eFilterPairStringCompare.InList, 0, 2);

		  DataView ConfigurationRows = FindActiveRows(AConfigurationView,
													  AProgramEvalBeganDate, AProgramEvalEndedDate, 
													  RowFilter);

		  string MonLocId = cDBConvert.ToString(AProgramRow["Mon_Loc_Id"]);
		  string MonLocList = ColumnToDatalist(ConfigurationRows, "Stack_Pipe_Mon_Loc_Id");

		  MonLocList = (MonLocList == "") ? MonLocId : MonLocId + "," + MonLocList;

		  DataView MethodRows;

		  if (AMethodCodeList == "")
		  {
			RowFilter = new sFilterPair[2];
			RowFilter[0].Set("Mon_Loc_Id", MonLocList, eFilterPairStringCompare.InList);
			RowFilter[1].Set("Parameter_Cd", AParameterCodeList, eFilterPairStringCompare.InList);

			MethodRows = FindActiveRows(AFacilityMethodView,
										AProgramEvalBeganDate, AProgramEvalEndedDate,
										RowFilter);
		  }
		  else
		  {
			if (AOrParameterAndMethod)
			{
			  RowFilter = new sFilterPair[2];
			  RowFilter[0].Set("Mon_Loc_Id", MonLocList, eFilterPairStringCompare.InList);
			  RowFilter[1].Set("Parameter_Cd", AParameterCodeList, eFilterPairStringCompare.InList);

			  sFilterPair[] SecFilter;
			  SecFilter = new sFilterPair[2];
			  SecFilter[0].Set("Mon_Loc_Id", MonLocList, eFilterPairStringCompare.InList);
			  SecFilter[1].Set("Method_Cd", AMethodCodeList, eFilterPairStringCompare.InList);

			  MethodRows = FindActiveRows(AFacilityMethodView,
										  AProgramEvalBeganDate, AProgramEvalEndedDate,
										  RowFilter, SecFilter);
			}
			else
			{
			  RowFilter = new sFilterPair[3];
			  RowFilter[0].Set("Mon_Loc_Id", MonLocList, eFilterPairStringCompare.InList);
			  RowFilter[1].Set("Parameter_Cd", AParameterCodeList, eFilterPairStringCompare.InList);
			  RowFilter[2].Set("Method_Cd", AMethodCodeList, eFilterPairStringCompare.InList);

			  MethodRows = FindActiveRows(AFacilityMethodView,
										  AProgramEvalBeganDate, AProgramEvalEndedDate,
										  RowFilter);
			}
		  }
      

		  if (MethodRows.Count == 0)
			return eUnitCommonMethodStatus.Missing;
		  else if (!CheckForDateRangeCovered(ACategory, MethodRows, AProgramEvalBeganDate, AProgramEvalEndedDate))
			return eUnitCommonMethodStatus.Incomplete;
		  else
			return eUnitCommonMethodStatus.Complete;
		}

		private enum eMultipleMethodStatus { NoMultiple, Missing, Incomplete, None };

		private static eMultipleMethodStatus DeterimeMultipleMethodStatus(string AParameterCodeList,
																		  bool AIncludePipe,
																		  DateTime AProgramEvalBeganDate,
																		  DateTime AProgramEvalEndedDate,
																		  DataRowView AProgramRow,
																		  DataView AFacilityMethodView,
																		  DataView AConfigurationView,
																		  cCategory ACategory)
		{
		  string StackPipeTypeList = AIncludePipe ? "MS,MP" : "MS";
		  sFilterPair[] Filter;

		  Filter = new sFilterPair[1];
		  Filter[0].Set("Stack_Name", StackPipeTypeList, eFilterPairStringCompare.InList, 0, 2);

		  DataView ConfigurationRows = FindActiveRows(AConfigurationView,
													  AProgramEvalBeganDate, AProgramEvalEndedDate,
													  Filter);

		  if (ConfigurationRows.Count == 0)
			return eMultipleMethodStatus.NoMultiple;
		  else
		  {
			string UnitMonLocId = cDBConvert.ToString(AProgramRow["Mon_Loc_Id"]);

			eMultipleMethodStatus Result = eMultipleMethodStatus.None;

			foreach (DataRowView MultipleConfigurationRow in ConfigurationRows)
			{
			  string MultipleMonLocId = cDBConvert.ToString(MultipleConfigurationRow["Stack_Pipe_Mon_Loc_Id"]);
			  string MultipleLocType = cDBConvert.ToString(MultipleConfigurationRow["Stack_Name"]).PadRight(2).Substring(0, 2);

			  Filter = new sFilterPair[2];
			  Filter[0].Set("Mon_Loc_Id", MultipleMonLocId);
			  Filter[1].Set("Parameter_Cd", AParameterCodeList, eFilterPairStringCompare.InList);

			  DataView MethodRows = FindActiveRows(AFacilityMethodView,
												   AProgramEvalBeganDate, AProgramEvalEndedDate,
												   Filter);

			  if (MethodRows.Count == 0)
				Result = eMultipleMethodStatus.Missing;
			  else
			  {
				StackPipeTypeList = AIncludePipe ? "CS,CP" : "CS";
				Filter = new sFilterPair[1];
				Filter[0].Set("Stack_Name", StackPipeTypeList, eFilterPairStringCompare.InList, 0, 2);

				DataView CommonConfigurationRows = FindActiveRows(AConfigurationView,
																  AProgramEvalBeganDate, AProgramEvalEndedDate,
																  Filter);

				string CommonMonLocList = ColumnToDatalist(CommonConfigurationRows, "Stack_Pipe_Mon_Loc_Id");

				string MonLocList = (CommonMonLocList == "") ? MultipleMonLocId + "," + UnitMonLocId
															 : MultipleMonLocId + "," + UnitMonLocId + "," + CommonMonLocList;

				Filter = new sFilterPair[2];
				Filter[0].Set("Mon_Loc_Id", MonLocList, eFilterPairStringCompare.InList);
				Filter[1].Set("Parameter_Cd", AParameterCodeList, eFilterPairStringCompare.InList);

				MethodRows = FindActiveRows(AFacilityMethodView,
											AProgramEvalBeganDate, AProgramEvalEndedDate,
											Filter);

				if (!CheckForDateRangeCovered(ACategory, MethodRows, AProgramEvalBeganDate, AProgramEvalEndedDate))
				{
				  if (Result != eMultipleMethodStatus.Missing)
					Result = eMultipleMethodStatus.Incomplete;
				}
			  }
			}

			return Result;
		  }
		}

		#endregion

		#endregion


		#region Cancelled

		public static string PROGRAM7(cCategory Category, ref bool Log)
		// Required Methods Reported for CAMR Affected Units
		{
		  string ReturnVal = "";

		  try
		  {
			DataRowView CurrentProgram = (DataRowView)Category.GetCheckParameter("Current_Program").ParameterValue;

			if (cDBConvert.ToString(CurrentProgram["Prg_Cd"]).InList("CAMR") &&
				(cDBConvert.ToString(CurrentProgram["Class"]) != "N"))
			{
			  DateTime ProgramEvalBeganDate = (DateTime)Category.GetCheckParameter("Program_Evaluation_Begin_Date").ParameterValue;
			  DateTime ProgramEvalEndedDate = (DateTime)Category.GetCheckParameter("Program_Evaluation_End_Date").ParameterValue;
			  DataView ConfigurationView = (DataView)Category.GetCheckParameter("Unit_Stack_Configuration_Records").ParameterValue;
			  DataView FacilityMethodView = (DataView)Category.GetCheckParameter("Facility_Method_Records").ParameterValue;

			  eMethodStatus MethodStatus;

			  string MissingList = ""; string MissingDelim = "";
			  string IncompleteList = ""; string IncompleteDelim = "";


			  //Handle NOx
			  MethodStatus = DetermineMethodStatus("HGM", "", false, false,
												   ProgramEvalBeganDate,
												   ProgramEvalEndedDate,
												   CurrentProgram,
												   FacilityMethodView,
												   ConfigurationView,
												   Category);

			  if (MethodStatus == eMethodStatus.Missing)
			  {
				MissingList += MissingDelim + "HGM";
				MissingDelim = ",";
			  }
			  else if (MethodStatus == eMethodStatus.Incomplete)
			  {
				IncompleteList += IncompleteDelim + "HGM";
				IncompleteDelim = ",";
			  }


			  //Handle Heat Input
			  MethodStatus = DetermineMethodStatus("HI", "", false, false,
												   ProgramEvalBeganDate,
												   ProgramEvalEndedDate,
												   CurrentProgram,
												   FacilityMethodView,
												   ConfigurationView,
												   Category);

			  if (MethodStatus == eMethodStatus.Missing)
			  {
				MissingList += MissingDelim + "HI";
				MissingDelim = ",";
			  }
			  else if (MethodStatus == eMethodStatus.Incomplete)
			  {
				IncompleteList += IncompleteDelim + "HI";
				IncompleteDelim = ",";
			  }


			  //Set Output Parameters
			  Category.SetCheckParameter("Missing_CAMR_Method_List", MissingList.FormatList(), eParameterDataType.String);
			  Category.SetCheckParameter("Incomplete_CAMR_Method_List", IncompleteList.FormatList(), eParameterDataType.String);

			  //Produce Results
			  if ((MissingList != "") && IncompleteList == "")
				Category.CheckCatalogResult = "A";
			  else if ((MissingList == "") && IncompleteList != "")
				Category.CheckCatalogResult = "B";
			  else if ((MissingList != "") && IncompleteList != "")
				Category.CheckCatalogResult = "C";
			}
			else
			  Log = false;
		  }
		  catch (Exception ex)
		  {
			ReturnVal = Category.CheckEngine.FormatError(ex, "PROGRAM7");
		  }

		  return ReturnVal;
		}

		#endregion

		#endregion

		*/

	}

}
