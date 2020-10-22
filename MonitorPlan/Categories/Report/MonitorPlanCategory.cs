using System;
using System.Data;
using System.Collections;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Data.Ecmps.Dbo.View;
using ECMPS.Checks.Mp.Parameters;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Extensions;
using ECMPS.ErrorSuppression;


namespace ECMPS.Checks.MonitorPlanEvaluation
{
    public class cMonitorPlanCategory : cCategory
    {

        #region Private Constants

        private const string Label = "Monitor Plan Category";

        #endregion


        #region Private Fields

        private cMonitorPlan mMonitorPlan;

        #endregion


        #region Constructors

        public cMonitorPlanCategory(cCheckEngine CheckEngine, cMonitorPlan MonitorPlan)
            : base(CheckEngine, (cProcess)MonitorPlan, "MONPLAN")
        {
            mMonitorPlan = MonitorPlan;
            TableName = "MONITOR_PLAN";
        }

        #endregion


        #region Public Static Methods

        public static cMonitorPlanCategory GetInitialized(cCheckEngine ACheckEngine, cMonitorPlan AMonitorPlanProcess)
        {
            cMonitorPlanCategory Category;
            string ErrorMessage = "";

            try
            {
                Category = new cMonitorPlanCategory(ACheckEngine, AMonitorPlanProcess);

                bool Result = Category.InitCheckBands(ACheckEngine.DbAuxConnection, ref ErrorMessage);

                if (!Result)
                {
                    Category = null;
                    System.Diagnostics.Debug.WriteLine(string.Format("{0}: {1}", Label, ErrorMessage));
                }
            }
            catch (Exception ex)
            {
                Category = null;
                System.Diagnostics.Debug.WriteLine(string.Format("{0}: {1}", Label, ex.Message));
            }

            return Category;
        }

        #endregion


        #region Public Methods

        public new bool ProcessChecks(string MonitorPlanId)
        {
            CurrentRowId = MonitorPlanId;

            System.Diagnostics.Debug.WriteLine(string.Format("{0}: {1}", Label, CurrentRowId));

            return base.ProcessChecks();
        }

        #endregion


        #region Base Class Overrides

        protected override void FilterData()
        {
            //get MonitorPlan
            SetCheckParameter("Current_Monitoring_Plan_Configuration", new DataView(mMonitorPlan.SourceData.Tables["MpMonitorPlan"],
                      "", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

            //get MonitorPlanProgram
            SetCheckParameter("Monitoring_Plan_Program_List", new DataView(mMonitorPlan.SourceData.Tables["MpUnitProgram"],
                      "", "mon_plan_id", DataViewRowState.CurrentRows), eParameterDataType.DataView);

            /* Selecting MP Method Records requires a select on MON_PLAN_ID since the source table contains al 'related' locations not just MP locaitons. */
            SetDataViewCheckParameter("MP_Method_Records", mMonitorPlan.SourceData.Tables["MpMonitorMethod"], "", "");

            //Facility Level Parameters

            // Set Facility_Method_Records
            SetCheckParameter("Facility_Method_Records",
                              new DataView(mMonitorPlan.SourceData.Tables["MonitorMethod"],
                                           "", "",
                                           DataViewRowState.CurrentRows),
                              eParameterDataType.DataView);

            //Set Combined_Facility_Method_Records
            SetDataViewCheckParameter("Combined_Facility_Method_Records",
                                      mMonitorPlan.SourceData.Tables["CombinedFacilityMethodRecords"],
                                      "", "");

            SetDataViewCheckParameter("Emissions_File_Records", mMonitorPlan.SourceData.Tables["EmissionsEvaluation"], "", "");
            SetDataViewCheckParameter("Facility_Unit_Stack_Configuration_Records", mMonitorPlan.SourceData.Tables["FacilityUnitStackConfiguration"], "", "");
            SetDataViewCheckParameter("Facility_Location_Records", mMonitorPlan.SourceData.Tables["FacilityLocation"], "", "");
            SetDataViewCheckParameter("MATS_MP_Supplemental_Compliance_Method_Records", mMonitorPlan.SourceData.Tables["MATSMethodData"], "", "");
            SetDataViewCheckParameter("Monitoring_Plan_Op_Status_Records", mMonitorPlan.SourceData.Tables["UnitOpStatus"], "", "");
            SetDataViewCheckParameter("MP_Locations", mMonitorPlan.SourceData.Tables["MpLocation"], "", "");

            // Set MP_Unit_Stack_Configuration_Records
            {
                string rowFilter;
                {
                    if (mMonitorPlan.SourceData.Tables["MpLocation"].Rows.Count > 0)
                    {
                        string monLocList = "";
                        {
                            string monLocDelim = "";

                            foreach (DataRow monitorLocationRow in mMonitorPlan.SourceData.Tables["MpLocation"].Rows)
                            {
                                monLocList += monLocDelim + "'" + monitorLocationRow["MON_LOC_ID"].AsString() + "'";
                                monLocDelim = ", ";
                            }
                        }
                        rowFilter = string.Format("MON_LOC_ID in ({0}) and STACK_PIPE_MON_LOC_ID in ({0})", monLocList);
                    }
                    else
                    {
                        rowFilter = "null is not null";
                    }
                }

                SetDataViewCheckParameter("MP_Unit_Stack_Configuration_Records", mMonitorPlan.SourceData.Tables["UnitStackConfiguration"], rowFilter, "");
            }


            // Lookup Table Parameters
            SetDataViewCheckParameter("MATS_Method_Code_Lookup", mMonitorPlan.SourceData.Tables["MatsMethodCode"], "", "");
            SetDataViewCheckParameter("MATS_Method_Parameter_Code_Lookup", mMonitorPlan.SourceData.Tables["MatsMethodParameterCode"], "", "");
            SetDataViewCheckParameter("Program_Code_Table", mMonitorPlan.SourceData.Tables["ProgramCode"], "", "Prg_Cd");

            // System Parameter Table
            MpParameters.SystemParameterLookupTable = new CheckDataView<VwSystemParameterRow>(Process.SourceData.Tables["SystemParameter"], null, null);

            // This Crosscheck has its own table - not a virtual crosscheck - used in MATSMethodChecks
            SetDataViewCheckParameter("Parameter_And_Method_And_Location_To_Formula_Crosscheck", mMonitorPlan.SourceData.Tables["ParameterAndMethodAndLocationToFormulaCrosscheck"], "", "");

            // Set Virtual Cross Check Records
            SetCrossCheckParameter("CrossCheck_ProgramParameterToLocationType");
            SetCrossCheckParameter("CrossCheck_ProgramParameterToMethodCode");
            SetCrossCheckParameter("CrossCheck_ProgramParameterToMethodParameter");
            SetCrossCheckParameter("CrossCheck_ProgramParameterToSeverity");
            SetCrossCheckParameter("CrossCheck_MatsSupplementalComplianceParameterToMethod");
            SetCrossCheckParameter("Formula_to_Required_Method_Crosscheck", "CrossCheck_FormulaToRequiredMethod");
            SetCrossCheckParameter("Formula_to_Required_Unit_Fuel_Crosscheck", "CrossCheck_FormulaToRequiredUnitFuel");
            SetCrossCheckParameter("Method_Parameter_Equivalent_Crosscheck", "CrossCheck_MethodParameterEquivalentCrosscheck");

        }


        protected override void SetRecordIdentifier()
        {
            DataRowView CurrentMonitoringPlanConfiguration = (DataRowView)GetCheckParameter("Current_Monitoring_Plan_Configuration").ParameterValue;

            RecordIdentifier = "Monitoring Plan ID " + (string)CurrentMonitoringPlanConfiguration["mon_plan_id"];
        }

        protected override bool SetErrorSuppressValues()
        {
            DataRowView currentMonitorPlan = GetCheckParameter("Current_Monitoring_Plan_Configuration").AsDataRowView();

            if (currentMonitorPlan != null)
            {
                long facId = CheckEngine.FacilityID;
                string matchDataValue = currentMonitorPlan["MON_PLAN_ID"].AsString();

                DateTime? matchTimeValue;
                {
                    int? rptPeriodId = currentMonitorPlan["END_RPT_PERIOD_ID"].AsInteger();

                    if (rptPeriodId.HasValue)
                        matchTimeValue = cDateFunctions.LastDateThisQuarter(rptPeriodId.Value);
                    else
                        matchTimeValue = null;
                }

                ErrorSuppressValues = new cErrorSuppressValues(facId, null, "MONPLAN", matchDataValue, "HISTIND", matchTimeValue);
                return true;
            }
            else
                return false;
        }


        #endregion


        #region Private Methods

        /// <summary>
        /// Sets a Cross Check table parameter with a specific parameter name.
        /// </summary>
        /// <param name="parameterName">Tne parameter name.</param>
        /// <param name="crossCheckName">The name of the cross check table.</param>
        private void SetCrossCheckParameter(string parameterName, string crossCheckName)
        {
            SetDataViewCheckParameter(parameterName, mMonitorPlan.SourceData.Tables[crossCheckName], "", "");
        }


        /// <summary>
        /// Sets a Cross Check table parameter with the parameter name set to the cross check table name.
        /// </summary>
        /// <param name="crossCheckName">The name of the cross check table.</param>
        private void SetCrossCheckParameter(string crossCheckName)
        {
            SetDataViewCheckParameter(crossCheckName, mMonitorPlan.SourceData.Tables[crossCheckName], "", "");
        }

        #endregion

    }
}