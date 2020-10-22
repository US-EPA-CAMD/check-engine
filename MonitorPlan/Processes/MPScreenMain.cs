using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.CheckEngine.Definitions;
using ECMPS.Checks.DatabaseAccess;
using ECMPS.Checks.MethodChecks;
using ECMPS.Checks.MonitorPlan;
using ECMPS.Checks.Mp.Parameters;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

namespace ECMPS.Checks.MPScreenEvaluation
{
    public class cMPScreenMain : cMpProcess
    {

        #region Constructors
        string mCategoryCd;

        public cMPScreenMain(cCheckEngine CheckEngine, string CategoryCd)
            : base(CheckEngine)
        {
            mCategoryCd = CategoryCd;
        }

        #endregion

        #region Public Fields


        #endregion

        #region Base Class Overrides

        /// <summary>
        /// Loads the Check Procedure delegates needed for a process code.
        /// </summary>
        /// <param name="checksDllPath">The path of the checks DLLs.</param>
        /// <param name="errorMessage">The message returned if the initialization fails.</param>
        /// <returns>True if the initialization succeeds.</returns>
        public override bool CheckProcedureInit(string checksDllPath, ref string errorMessage)
        {
            bool result;

            try
            {
                switch (mCategoryCd)
                {
                    case "SCRANRG":
                    case "SCRCOMP":
                    case "SCRSYSC":
                    case "SCRCALS":
                        {
                            Checks[10] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.MonitorPlan.dll",
                                                                               "ECMPS.Checks.ComponentChecks.cComponentChecks").Unwrap();
                            result = true;
                        }
                        break;

                    case "SCRDFLT":
                        {
                            Checks[7] = InstantiateChecks("cLocationChecks", "LocationChecks", checksDllPath, true);
                            Checks[15] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.MonitorPlan.dll",
                                                                               "ECMPS.Checks.DefaultChecks.cDefaultChecks").Unwrap();
                            result = true;
                        }
                        break;

                    case "SCRFSPN":
                    case "SCRSPAN":
                        {
                            Checks[14] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.MonitorPlan.dll",
                                                                               "ECMPS.Checks.SpanChecks.cSpanChecks").Unwrap();
                            result = true;
                        }
                        break;

                    case "SCRFORM":
                        {
                            Checks[13] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.MonitorPlan.dll",
                                                                               "ECMPS.Checks.FormulaChecks.cFormulaChecks").Unwrap();
                            result = true;
                        }
                        break;

                    case "SCRLOAD":
                        {
                            Checks[7] = InstantiateChecks("cLocationChecks", "LocationChecks", checksDllPath, true);
                            Checks[16] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.MonitorPlan.dll",
                                                                               "ECMPS.Checks.LoadChecks.cLoadChecks").Unwrap();
                            result = true;
                        }
                        break;

                    case "SCRLOCA":
                    case "SCRSTCK":
                    case "SCRUSCF":
                        {
                            Checks[6] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.MonitorPlan.dll",
                                                                               "ECMPS.Checks.MonitorPlanChecks.cMonitorPlanChecks").Unwrap();
                            Checks[7] = InstantiateChecks("cLocationChecks", "LocationChecks", checksDllPath, true);
                            result = true;
                        }
                        break;

                    case "SCRMETH":
                        {
                            Checks[6] = InstantiateChecks("cMonitorPlanChecks", "MonitorPlanChecks", checksDllPath, false);
                            Checks[7] = InstantiateChecks("cLocationChecks", "LocationChecks", checksDllPath, true);
                            Checks[9] = new cMethodChecks(this);
                            result = true;
                        }
                        break;

                    case "SCRMPCM":
                        {
                            Checks[6] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.MonitorPlan.dll",
                                                                                "ECMPS.Checks.MonitorPlanChecks.cMonitorPlanChecks").Unwrap();
                            result = true;
                        }
                        break;

                    case "SCRQUAL":
                        {
							Checks[6] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.MonitorPlan.dll",
															 "ECMPS.Checks.MonitorPlanChecks.cMonitorPlanChecks").Unwrap();
							Checks[7] = InstantiateChecks("cLocationChecks", "LocationChecks", checksDllPath, true);
                            Checks[17] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.MonitorPlan.dll",
                                                                              "ECMPS.Checks.QualificationChecks.cQualificationChecks").Unwrap();
                            result = true;
                        }
                        break;

					case "SCRQLME":
                    case "SCRQPCT":
                        {
                            Checks[17] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.MonitorPlan.dll",
                                                                              "ECMPS.Checks.QualificationChecks.cQualificationChecks").Unwrap();
                            result = true;
                        }
                        break;

                    case "SCRRWAF":
                        {
                            Checks[15] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.MonitorPlan.dll",
                                                                              "ECMPS.Checks.DefaultChecks.cDefaultChecks").Unwrap();
                            result = true;
                        }
                        break;

                    case "SCRSYST":
                        {
                            Checks[11] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.MonitorPlan.dll",
                                                                              "ECMPS.Checks.SystemChecks.cSystemChecks").Unwrap();
                            result = true;
                        }
                        break;

                    case "SCRSYSF":
                        {
                            Checks[12] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.MonitorPlan.dll",
                                                                              "ECMPS.Checks.FuelFlowChecks.cFuelFlowChecks").Unwrap();
                            result = true;
                        }
                        break;

                    case "SCRUCAP":
                        {
                            Checks[20] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.MonitorPlan.dll",
                                                                              "ECMPS.Checks.UnitCapacityChecks.cCapacityChecks").Unwrap();
                            result = true;
                        }
                        break;

                    case "SCRUCTL":
                        {
                            Checks[19] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.MonitorPlan.dll",
                                                                              "ECMPS.Checks.UnitControlChecks.cUnitControlChecks").Unwrap();
                            result = true;
                        }
                        break;

                    case "SCRFUEL":
                        {
                            Checks[18] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.MonitorPlan.dll",
                                                                              "ECMPS.Checks.FuelChecks.cFuelChecks").Unwrap();
                            result = true;
                        }
                        break;

                    case "SCRQLEE":
                        {
							Checks[58] = InstantiateChecks("cQualificationLEEChecks", "QualificationLEEChecks", checksDllPath, false);

                            result = true;
                        }
                        break;

                    case "SCRMMD":
                        {
							Checks[59] = InstantiateChecks("cMATSSupplementalMethodChecks", "MATSSupplementalMethodChecks", checksDllPath, false);
							result = true;
                        }
                        break;

                    default:
                        {
                            errorMessage = string.Format("Unhandled category code for process '{0}'.", this.ProcessCd).FormatError();
                            result = false;
                        }
                        break;
                }

            }
            catch (Exception ex)
            {
                errorMessage = ex.FormatError();
                result = false;
            }

            return result;
        }

        protected override string ExecuteChecksWork()
        {
            bool RunResult;
            string Result = "";

            // Initialize category object; checks object
            // run checks for each record in each category
            // update database (logs and calculate values)


            {
                //ElapsedTimes.TimingBegin("ExecuteChecks", this);

                cCheckParameterBands ScreenChecks = GetCheckBands(mCategoryCd);

                SetCheckParameter("First_ECMPS_Reporting_Period", CheckEngine.FirstEcmpsReportingPeriodId);

                switch (mCategoryCd)
                {
                    case "SCRANRG":
                        {
                            cAnalyzerRangeRow AnalyzerRangeRow;
                            AnalyzerRangeRow = new cAnalyzerRangeRow(mCheckEngine, this, mCheckEngine.MonLocId, mCheckEngine.ThisTable);
                            RunResult = AnalyzerRangeRow.ProcessChecks(ScreenChecks);
                            AnalyzerRangeRow.EraseParameters();
                            break;
                        }
                    case "SCRCALS":
                        {
                            cCalibrationStandardDataRowCategory CalibrationStandardDataRow;
                            CalibrationStandardDataRow = new cCalibrationStandardDataRowCategory(mCheckEngine, this, mCheckEngine.MonLocId, mCheckEngine.ThisTable);
                            RunResult = CalibrationStandardDataRow.ProcessChecks(ScreenChecks);
                            CalibrationStandardDataRow.EraseParameters();
                            break;
                        }
                    case "SCRCOMP":
                        {
                            cComponentRow ComponentRow;
                            ComponentRow = new cComponentRow(mCheckEngine, this, mCheckEngine.MonLocId, mCheckEngine.ThisTable);
                            RunResult = ComponentRow.ProcessChecks(ScreenChecks);
                            ComponentRow.EraseParameters();
                            break;
                        }
                    case "SCRDFLT":
                        {
                            cDefaultRowCategory DefaultRowCategory;
                            DefaultRowCategory = new cDefaultRowCategory(mCheckEngine, this, mCheckEngine.MonLocId, mCheckEngine.ThisTable);
                            RunResult = DefaultRowCategory.ProcessChecks(ScreenChecks);
                            DefaultRowCategory.EraseParameters();
                            break;
                        }
                    case "SCRFORM":
                        {
                            cFormulaRow FormulaRow;
                            FormulaRow = new cFormulaRow(mCheckEngine, this, mCheckEngine.MonLocId, mCheckEngine.ThisTable);
                            RunResult = FormulaRow.ProcessChecks(ScreenChecks);
                            FormulaRow.EraseParameters();
                            break;
                        }
                    case "SCRFSPN":
                        {
                            cSpanRowCategory SpanRow;
                            SpanRow = new cSpanRowCategory(mCheckEngine, this, mCategoryCd, mCheckEngine.MonLocId, mCheckEngine.ThisTable);
                            RunResult = SpanRow.ProcessChecks(ScreenChecks);
                            SpanRow.EraseParameters();
                            break;
                        }
                    case "SCRFUEL":
                        {
                            cUnitFuelRowCategory UnitFuelRow;
                            UnitFuelRow = new cUnitFuelRowCategory(mCheckEngine, this, mCheckEngine.MonLocId, mCheckEngine.ThisTable);
                            RunResult = UnitFuelRow.ProcessChecks(ScreenChecks);
                            UnitFuelRow.EraseParameters();
                            break;
                        }
                    case "SCRLOAD":
                        {
                            cLoadRowCategory LoadRow;
                            LoadRow = new cLoadRowCategory(mCheckEngine, this, mCheckEngine.MonLocId, mCheckEngine.ThisTable);
                            RunResult = LoadRow.ProcessChecks(ScreenChecks);
                            LoadRow.EraseParameters();
                            break;
                        }
                    case "SCRLOCA":
                        {
                            cAttributeRow AttributeRow;
                            AttributeRow = new cAttributeRow(mCheckEngine, this, mCheckEngine.MonLocId, mCheckEngine.ThisTable);
                            RunResult = AttributeRow.ProcessChecks(ScreenChecks);
                            AttributeRow.EraseParameters();
                            break;
                        }
                    case "SCRMETH":
                        {
                            cMethodRow MethodRow;
                            MethodRow = new cMethodRow(mCheckEngine, this, mCheckEngine.MonLocId, mCheckEngine.ThisTable);
                            RunResult = MethodRow.ProcessChecks(ScreenChecks);
                            MethodRow.EraseParameters();
                            break;
                        }
					case "SCRMMD":
						{
							cMATSSupplementalMethodRowCategory MATSSupplementalMethodRow;
							MATSSupplementalMethodRow = new cMATSSupplementalMethodRowCategory(mCheckEngine, this, mCheckEngine.MonLocId, mCheckEngine.ThisTable);
							RunResult = MATSSupplementalMethodRow.ProcessChecks(ScreenChecks);
							MATSSupplementalMethodRow.EraseParameters();

							break;
						}
					case "SCRMPCM":
                        {
                            cMonitorPlanCommentRowCategory MonitorPlanCommentRow;
                            MonitorPlanCommentRow = new cMonitorPlanCommentRowCategory(mCheckEngine, this, mCheckEngine.ThisTable);
                            RunResult = MonitorPlanCommentRow.ProcessChecks(ScreenChecks);
                            MonitorPlanCommentRow.EraseParameters();
                            break;
                        }
                    case "SCRQLEE":
                        {
                            cQualificationLEERowCategory QualificationLEERow;
                            QualificationLEERow = new cQualificationLEERowCategory(mCheckEngine, this, mCheckEngine.MonLocId, mCheckEngine.ThisTable);
                            RunResult = QualificationLEERow.ProcessChecks(ScreenChecks);
                            QualificationLEERow.EraseParameters();
                            break;
                        }
					case "SCRQLME":
						{
							cQualificationLMERowCategory QualificationLMERow;
							QualificationLMERow = new cQualificationLMERowCategory(mCheckEngine, this, mCheckEngine.MonLocId, mCheckEngine.ThisTable);
							RunResult = QualificationLMERow.ProcessChecks(ScreenChecks);
							QualificationLMERow.EraseParameters();
							break;
						}
					case "SCRQPCT":
                        {
                            cQualificationPctRowCategory QualificationPctRow;
                            QualificationPctRow = new cQualificationPctRowCategory(mCheckEngine, this, mCheckEngine.MonLocId, mCheckEngine.ThisTable);
                            RunResult = QualificationPctRow.ProcessChecks(ScreenChecks);
                            QualificationPctRow.EraseParameters();
                            break;
                        }
                    case "SCRQUAL":
                        {
                            cQualificationRowCategory QualificationRow;
                            QualificationRow = new cQualificationRowCategory(mCheckEngine, this, mCheckEngine.MonLocId, mCheckEngine.ThisTable);
                            RunResult = QualificationRow.ProcessChecks(ScreenChecks);
                            QualificationRow.EraseParameters();
                            break;
                        }
                    case "SCRRWAF":
                        {
                            cWAFRowCategory WAFRow;
                            WAFRow = new cWAFRowCategory(mCheckEngine, this, mCheckEngine.MonLocId, mCheckEngine.ThisTable);
                            RunResult = WAFRow.ProcessChecks(ScreenChecks);
                            WAFRow.EraseParameters();
                            break;
                        }
                    case "SCRSPAN":
                        {
                            cSpanRowCategory SpanRow;
                            SpanRow = new cSpanRowCategory(mCheckEngine, this, mCategoryCd, mCheckEngine.MonLocId, mCheckEngine.ThisTable);
                            RunResult = SpanRow.ProcessChecks(ScreenChecks);
                            SpanRow.EraseParameters();
                            break;
                        }
                    case "SCRSYSC":
                        {
                            cSystemComponentRowCategory SystemComponentCategory;
                            SystemComponentCategory = new cSystemComponentRowCategory(mCheckEngine, this, mCheckEngine.MonLocId, mCheckEngine.ThisTable);
                            RunResult = SystemComponentCategory.ProcessChecks(ScreenChecks);
                            SystemComponentCategory.EraseParameters();
                            break;
                        }
                    case "SCRSYSF":
                        {
                            cSystemFuelFlowRowCategory SystemFuelFlowCategory;
                            SystemFuelFlowCategory = new cSystemFuelFlowRowCategory(mCheckEngine, this, mCheckEngine.MonLocId, mCheckEngine.ThisTable);
                            RunResult = SystemFuelFlowCategory.ProcessChecks(ScreenChecks);
                            SystemFuelFlowCategory.EraseParameters();
                            break;
                        }
                    case "SCRSYST":
                        {
                            cSystemRowCategory SystemCategory;
                            SystemCategory = new cSystemRowCategory(mCheckEngine, this, mCheckEngine.MonLocId, mCheckEngine.ThisTable);
                            RunResult = SystemCategory.ProcessChecks(ScreenChecks);
                            SystemCategory.EraseParameters();
                            break;
                        }
                    case "SCRUCAP":
                        {
                            cUnitCapacityRowCategory UnitCapacityRow;
                            UnitCapacityRow = new cUnitCapacityRowCategory(mCheckEngine, this, mCheckEngine.MonLocId, mCheckEngine.ThisTable);
                            RunResult = UnitCapacityRow.ProcessChecks(ScreenChecks);
                            UnitCapacityRow.EraseParameters();
                            break;
                        }
                    case "SCRUCTL":
                        {
                            cUnitControlRowCategory UnitControlRow;
                            UnitControlRow = new cUnitControlRowCategory(mCheckEngine, this, mCheckEngine.MonLocId, mCheckEngine.ThisTable);
                            RunResult = UnitControlRow.ProcessChecks(ScreenChecks);
                            UnitControlRow.EraseParameters();
                            break;
                        }
                    case "SCRSTCK":
                        {
                            cStackPipeRowCategory StackPipeRow;
                            StackPipeRow = new cStackPipeRowCategory(mCheckEngine, this, mCheckEngine.MonLocId, mCheckEngine.ThisTable);
                            RunResult = StackPipeRow.ProcessChecks(ScreenChecks);
                            StackPipeRow.EraseParameters();
                            break;
                        }
                    case "SCRUSCF":
                        {
                            cUnitStackConfigurationRowCategory UnitStackConfigurationRow;
                            UnitStackConfigurationRow = new cUnitStackConfigurationRowCategory(mCheckEngine, this, mCheckEngine.MonLocId, mCheckEngine.ThisTable);
                            RunResult = UnitStackConfigurationRow.ProcessChecks(ScreenChecks);
                            UnitStackConfigurationRow.EraseParameters();
                            break;
                        }
                }
            }

            DbUpdate(ref Result);

            return Result;
        }

        protected override void InitCalculatedData()
        {

            //string ErrorMsg = "";

        }

        protected override void InitSourceData()
        {

            // Initialize all data tables in process

            mSourceData = new DataSet();
            mFacilityID = GetFacilityID();

            string sMonPlanWhereClause = string.Format(" WHERE MON_PLAN_ID = '{0}'", mCheckEngine.MonPlanId);
            string sMonLocWhereClause = string.Format(" WHERE MON_LOC_ID = '{0}'", mCheckEngine.MonLocId);

            switch (mCheckEngine.CategoryCd)
            {
                case "SCRANRG":
                    {
                        //get AnalyzerRange records for this location ID
                        AddSourceData("MPAnalyzerRange", "SELECT * FROM VW_Analyzer_Range " + sMonLocWhereClause);
                        AddSourceData("AnalyzerRangeCode", "SELECT * FROM analyzer_range_code ");
                        AddSourceData("MPComponent", "SELECT * FROM VW_Component " + sMonLocWhereClause);
                        break;
                    }
                case "SCRCALS":
                    {
                        //get Calibration Standard Data records for this location ID
                        AddSourceDataTable("MPCalibrationStandardData", "VW_CALIBRATION_STANDARD", sMonLocWhereClause);
                        AddSourceDataTable("CalibrationStandardCode", "VW_CALIBRATION_STANDARD_CODE", "");
                        AddSourceDataTable("MPComponent", "VW_COMPONENT", sMonLocWhereClause);
                        break;
                    }
                case "SCRCOMP":
                    {
                        //get Component records for this location ID
                        AddSourceData("MPComponent", "SELECT * FROM VW_Component " + sMonLocWhereClause);
                        AddSourceData("ComponentTypeCode", "SELECT * FROM component_type_code");
                        AddSourceData("SAMCode", "SELECT * FROM acquisition_method_code ");
                        break;
                    }
                case "SCRDFLT":
                    {
                        //add default records
                        AddSourceDataTable("MPDefault", "VW_MONITOR_DEFAULT", sMonLocWhereClause);
                        AddSourceDataTable("LocationFuel", "VW_LOCATION_FUEL", sMonLocWhereClause);
                        AddSourceDataTable("LocationUnitType", "VW_LOCATION_UNIT_TYPE", sMonLocWhereClause);

                        // lookup tables
                        AddSourceDataTable("FuelCode", "VW_FUEL_CODE");
                        AddSourceDataTable("DefaultPurposeCode", "VW_DEFAULT_PURPOSE_CODE");
                        AddSourceDataTable("DefaultSourceCode", "VW_DEFAULT_SOURCE_CODE");
                        AddSourceDataTable("ParameterUOM", "VW_PARAMETER_UOM");
                        AddSourceDataTable("UnitsOfMeasureCode", "VW_UNITS_OF_MEASURE_CODE");

                        break;
                    }
                case "SCRFORM":
                    {
                        //get Formula records for this location ID
                        AddSourceData("MPFormula", "SELECT * FROM VW_Monitor_Formula " + sMonLocWhereClause);
                        //get formula codes
                        AddSourceData("FormulaCode", "SELECT * FROM Equation_code ");
                        break;
                    }
                case "SCRFSPN":
                    {
                        //Monitor Plan Data Tables for Locations
                        AddSourceDataTable("AnalyzerRange", "VW_Analyzer_range", sMonLocWhereClause);
                        AddSourceDataTable("MonitorSpan", "VW_MONITOR_SPAN", sMonLocWhereClause);

                        //Facility and Unit Data Tables for Locations
                        AddSourceDataTable("LocationCapacity", "VW_LOCATION_CAPACITY", sMonLocWhereClause);
                        AddSourceDataTable("LocationControl", "VW_LOCATION_CONTROL", sMonLocWhereClause);
                        AddSourceDataTable("LocationFuel", "VW_LOCATION_FUEL", sMonLocWhereClause);
                        AddSourceDataTable("LocationUnitType", "VW_LOCATION_UNIT_TYPE", sMonLocWhereClause);

                        //Lookup Code Tables
                        AddSourceDataTable("ComponentTypeCode", "VW_component_Type_code");
                        AddSourceDataTable("ParameterUom", "VW_Parameter_Uom");
                        AddSourceDataTable("SpanMethodCode", "VW_SPAN_METHOD_CODE");
                        AddSourceDataTable("UnitsOfMeasureCode", "VW_Units_Of_Measure_Code");

                        break;
                    }
                case "SCRFUEL":
                    {
                        AddSourceDataTable("IndicatorCode", "INDICATOR_CODE");
                        AddSourceDataTable("DemMethodCode", "DEM_METHOD_CODE");
                        AddSourceDataTable("FuelCode", "VW_FUEL_CODE");

                        //get UnitFuel records for this location ID
                        AddSourceData("MPUnitFuel", "SELECT * FROM VW_LOCATION_FUEL " + sMonLocWhereClause);
                        break;
                    }
                case "SCRLOAD":
                    {
                        //get load records for this location id
                        AddSourceDataTable("MPLoad", "VW_MONITOR_LOAD", sMonLocWhereClause);
                        AddSourceDataTable("ParameterUOM", "VW_PARAMETER_UOM");

                        break;
                    }
                case "SCRLOCA":
                    {
                        AddSourceData("MPLocationAttribute", "SELECT * FROM VW_LOCATION_ATTRIBUTE " + sMonLocWhereClause);
                        AddSourceData("MPSystem", "SELECT * FROM VW_MONITOR_SYSTEM " + sMonLocWhereClause);
                        AddSourceDataTable("LocationUnitType", "VW_LOCATION_UNIT_TYPE", sMonLocWhereClause);
                        AddSourceData("MaterialCode", "SELECT * FROM material_code ");
                        AddSourceData("ShapeCode", "SELECT * FROM shape_code ");
                        break;
                    }
                case "SCRMETH":
                    {
                        AddSourceData("MethodCode", "SELECT * FROM method_code ");
                        AddSourceData("BypassApproachCode", "SELECT * FROM bypass_approach_code ");
                        AddSourceData("SubstituteDataCode", "SELECT * FROM substitute_data_code ");
                        break;
                    }
				case "SCRMMD":
					{
            AddSourceDataTable("MATSMethodData", string.Format("CheckMp.MATSMethodDataParameterByLocation('{0}')", mCheckEngine.MonLocId));
						AddSourceDataTable("MatsMethodParameterCodeLookup", "Lookup.MATS_METHOD_PARAMETER_CODE");
						AddSourceDataTable("MatsMethodCodeLookup", "Lookup.MATS_METHOD_CODE");
						break;
					}
                case "SCRMPCM":
                    {
                        //get monitor plan comment records for this location id
                        AddSourceDataTable("MpMonitorPlanComment", "VW_MONITOR_PLAN_COMMENT", sMonPlanWhereClause);
                        break;
                    }
				case "SCRQLEE":
					{
						//get qualification LEE records for this location id
						AddSourceDataTable("MonitorQualificationLEE", string.Format("CheckMp.MonitorQualificationLEEParameterByLocation('{0}')", mCheckEngine.MonLocId));
						AddSourceDataTable("QualLEETestTypeCode", "VW_QUAL_LEE_TEST_TYPE_CD");
						break;
					}
				case "SCRQLME":
                    {
                        //get qualification lme records for this location id
                        AddSourceDataTable("MPQualificationLME", "VW_MONITOR_QUALIFICATION_LME", sMonLocWhereClause);
                        break;
                    }
                case "SCRQPCT":
                    {
                        //get qualification percent records for this location id
                        AddSourceDataTable("MPQualificationPct", "VW_MONITOR_QUALIFICATION_PCT", sMonLocWhereClause);
                        break;
                    }
                case "SCRQUAL":
                    {
                        //lookup table
                        AddSourceDataTable("QualificationTypeCode", "VW_QUAL_TYPE_CODE");
                        AddSourceDataTable("MPMethodRecords", "VW_MP_MONITOR_METHOD", sMonLocWhereClause);
                        AddSourceDataTable("MATSMethodData", string.Format("CheckMp.MATSMethodDataParameterByLocation('{0}')", mCheckEngine.MonLocId));
                        AddSourceDataTable("SystemParameter", "SYSTEM_PARAMETER");

                        break;
                    }
                case "SCRRWAF":
                    {
                        //get waf records for this location id
                        AddSourceDataTable("RectDuctWaf", "VW_RECT_DUCT_WAF", sMonLocWhereClause);
                        //lookup table
                        AddSourceDataTable("WafMethodCode", "VW_WAF_METHOD_CODE");
                        break;
                    }
                case "SCRSPAN":
                    {
                        //Monitor Plan Data Tables for Locations
                        AddSourceDataTable("AnalyzerRange", "VW_Analyzer_range", sMonLocWhereClause);
                        AddSourceDataTable("MonitorSpan", "VW_MONITOR_SPAN", sMonLocWhereClause);

                        //Facility and Unit Data Tables for Locations
                        AddSourceDataTable("LocationCapacity", "VW_LOCATION_CAPACITY", sMonLocWhereClause);
                        AddSourceDataTable("LocationControl", "VW_LOCATION_CONTROL", sMonLocWhereClause);
                        AddSourceDataTable("LocationFuel", "VW_LOCATION_FUEL", sMonLocWhereClause);
                        AddSourceDataTable("LocationUnitType", "VW_LOCATION_UNIT_TYPE", sMonLocWhereClause);

                        //Lookup Code Tables
                        AddSourceDataTable("ComponentTypeCode", "VW_component_Type_code");
                        AddSourceDataTable("ParameterUom", "VW_Parameter_Uom");
                        AddSourceDataTable("SpanMethodCode", "VW_SPAN_METHOD_CODE");
                        AddSourceDataTable("UnitsOfMeasureCode", "VW_Units_Of_Measure_Code");

                        break;
                    }
                case "SCRSYSC":
                    {
                        //Monitor Plan Based Tables
                        AddSourceDataTable("MonitorLocationMonitorSystemComponent", "VW_MONITOR_SYSTEM_COMPONENT", sMonLocWhereClause);

                        break;
                    }
                case "SCRSYSF":
                    {
                        //Monitor Plan Based Tables
                        AddSourceDataTable("MonitorLocationMonitorSystemFuelFlow", "VW_System_Fuel_Flow", sMonLocWhereClause);
                        AddSourceDataTable("MonitorLocationMonitorSystem", "VW_Monitor_System", sMonLocWhereClause);

                        //Lookup Code Tables
                        AddSourceDataTable("MaxRateSourceCode", "VW_Max_Rate_Source_Code");
                        AddSourceDataTable("ParameterUom", "VW_Parameter_Uom");
                        AddSourceDataTable("SystemTypeCode", "VW_System_Type_Code");
                        AddSourceDataTable("UnitsOfMeasureCode", "VW_Units_Of_Measure_Code");

                        break;
                    }
                case "SCRSYST":
                    {
                        //Monitor Plan Based Tables
                        AddSourceDataTable("MonitorPlanMonitorSystem", "VW_MONITOR_SYSTEM", sMonLocWhereClause);

                        //Lookup Code Tables
                        AddSourceDataTable("FuelCode", "VW_FUEL_CODE");
                        AddSourceDataTable("SystemDesignationCode", "VW_System_Designation_Code");
                        AddSourceDataTable("SystemTypeCode", "VW_System_Type_Code");
                        break;
                    }
                case "SCRUCAP":
                    {
                        //get UnitCapacity records for this location ID
                        AddSourceDataTable("MPUnitCapacity", "VW_LOCATION_CAPACITY", sMonLocWhereClause);
                        break;
                    }
                case "SCRUCTL":
                    {
                        //get UnitControl records for this location ID
                        AddSourceDataTable("MPUnitControl", "VW_LOCATION_CONTROL", sMonLocWhereClause);

                        // control needs this lookup table
                        AddSourceDataTable("ControlCode", "VW_CONTROL_CODE");
                        break;
                    }
                case "SCRSTCK":
                    {
                        //get UnitCapacity records for this location ID
                        string sFacIdWhereClause = string.Format(" WHERE FAC_ID = '{0}'", mCheckEngine.ThisTable.Rows[0]["fac_id"]);
                        AddSourceDataTable("MPStackPipe", "VW_STACK_PIPE", sFacIdWhereClause);
                        break;
                    }
            }

            //get location record for this location ID
            AddSourceData("MPLocation", "SELECT * FROM VW_MONITOR_LOCATION " + sMonLocWhereClause);

            //get Unit Stack Configuration records for this stack
            AddSourceData("MPUnitStackConfiguration", string.Format("SELECT * FROM VW_UNIT_STACK_CONFIGURATION WHERE STACK_PIPE_MON_LOC_ID = '{0}' OR MON_LOC_ID = '{0}'", mCheckEngine.MonLocId));

            //get Used_identifier records for this location ID
            AddSourceData("UsedIdentifier", "SELECT * FROM Used_Identifier " + sMonLocWhereClause);

            //get qualification records for this location id
            AddSourceDataTable("MPQualification", "VW_MONITOR_QUALIFICATION", sMonLocWhereClause);

            //get method records for this location ID
            AddSourceData("MPMethod", "SELECT * FROM VW_MONITOR_METHOD " + sMonLocWhereClause);

            LoadCrossChecks();
        }

        /// <summary>
        /// This method initializes the class containing static properties enabling strongly typed access to the parameters used by the process.
        /// </summary>
        protected override void InitStaticParameterClass()
        {
          MpParameters.Init(this);
        }

        /// <summary>
        /// Allows the setting of the current category for which parameters will be set.
        /// </summary>
        /// <param name="category"></param>
        public override void SetStaticParameterCategory(cCategory category)
        {
          MpParameters.Category = category;
        }


        #region Helper Methods

        /// <summary>
        /// Instantiate an object of a particular Emission Checks class.
        /// </summary>
        /// <param name="className">The class to instantiate.</param>
        /// <param name="namespaceLeaf">The namespace of the checks DLLs.</param>
        /// <param name="dllPath">The location of the checks DLLs.</param>
        /// <returns>The resulting checks object.</returns>
        private cChecks InstantiateChecks(string className, string namespaceLeaf, string dllPath, bool isMpChecksChild)
        {
            const string dllName = "ECMPS.Checks.MonitorPlan.dll";
            const string nameSpacePath = "ECMPS.Checks";

            object[] constructorArgements = new object[] { this };

            cChecks result;

            if (isMpChecksChild)
                result = (cMpChecks)Activator.CreateInstanceFrom(dllPath + dllName,
                                                                 nameSpacePath + "." + namespaceLeaf + "." + className,
                                                                 true, 0, null,
                                                                 constructorArgements,
                                                                 null, null).Unwrap();
            else
                result = (cChecks)Activator.CreateInstanceFrom(dllPath + dllName,
                                                               nameSpacePath + "." + namespaceLeaf + "." + className).Unwrap();

            return result;
        }

        #endregion

        #endregion

        #region Private Methods

        private void LoadCrossChecks()
        {
            DataTable Catalog = mCheckEngine.DbAuxConnection.GetDataTable("SELECT * FROM vw_Cross_Check_Catalog");
            DataTable Value = mCheckEngine.DbAuxConnection.GetDataTable("SELECT * FROM vw_Cross_Check_Catalog_Value");
            DataTable CrossCheck;
            DataRow CrossCheckRow;
            string CrossCheckName;
            string Column1Name;
            string Column2Name;
            string Column3Name;

            foreach (DataRow CatalogRow in Catalog.Rows)
            {
                CrossCheckName = (string)CatalogRow["Cross_Chk_Catalog_Name"];
                CrossCheckName = CrossCheckName.Replace(" ", "");

                CrossCheck = new DataTable("CrossCheck_" + CrossCheckName);

                Column1Name = (string)CatalogRow["Description1"];
                Column2Name = (string)CatalogRow["Description2"];

                CrossCheck.Columns.Add(Column1Name);
                CrossCheck.Columns.Add(Column2Name);

                if (CatalogRow["Description3"] != DBNull.Value)
                {
                    Column3Name = (string)CatalogRow["Description3"];
                    CrossCheck.Columns.Add(Column3Name);
                }
                else Column3Name = "";

                Column1Name.Replace(" ", "");
                Column2Name.Replace(" ", "");
                Column3Name.Replace(" ", "");

                Value.DefaultView.RowFilter = "Cross_Chk_Catalog_Id = " + cDBConvert.ToString(CatalogRow["Cross_Chk_Catalog_Id"]);

                foreach (DataRowView ValueRow in Value.DefaultView)
                {
                    CrossCheckRow = CrossCheck.NewRow();

                    CrossCheckRow[Column1Name] = ValueRow["Value1"];
                    CrossCheckRow[Column2Name] = ValueRow["Value2"];

                    if (CatalogRow["Description3"] != DBNull.Value)
                        CrossCheckRow[Column3Name] = ValueRow["Value3"];

                    CrossCheck.Rows.Add(CrossCheckRow);
                }

                mSourceData.Tables.Add(CrossCheck);
            }
        }


        #endregion

    }
}