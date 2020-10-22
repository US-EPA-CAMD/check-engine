using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Text;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.CheckEngine.Definitions;
using ECMPS.Checks.DatabaseAccess;
using ECMPS.Checks.Em.Parameters;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Common;
using ECMPS.Definitions.SeverityCode;

namespace ECMPS.Checks.LME
{
    public class cLMEGenerationProcess : cProcess
    {
        # region Constructors

        public cLMEGenerationProcess( cCheckEngine CheckEngine )
            : base( CheckEngine, "LMEGEN" )
        {
        }

        # endregion

        # region Base Class Overides

        /// <summary>
        /// Loads the Check Procedure delegates needed for a process code.
        /// </summary>
        /// <param name="checksDllPath">The path of the checks DLLs.</param>
        /// <param name="errorMessage">The message returned if the initialization fails.</param>
        /// <returns>True if the initialization succeeds.</returns>
        public override bool CheckProcedureInit( string checksDllPath, ref string errorMessage )
        {
            bool result;

            try
            {
                Checks[45] = (cChecks)Activator.CreateInstanceFrom( checksDllPath + "ECMPS.Checks.LME.dll", "ECMPS.Checks.LMEChecks.cLMEChecks" ).Unwrap();
                result = true;
            }
            catch( Exception ex )
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

            cLMEInitializationCategory LMEInitializationCategory = new cLMEInitializationCategory( mCheckEngine, this );
            cLTFFHeatInputCategory LTFFHICategory = new cLTFFHeatInputCategory( mCheckEngine, this, LMEInitializationCategory );
            cHourlyEmissionsDataCategory HourlyEmissionsDataCategory = new cHourlyEmissionsDataCategory( mCheckEngine, this, LMEInitializationCategory );
            cSummaryValueDataCategory SummaryValueDataCategory = new cSummaryValueDataCategory( mCheckEngine, this, LMEInitializationCategory );

            // Initialize Check Bands
            LMEInitializationCategory.InitCheckBands( CheckEngine.DbAuxConnection, ref Result );
            LTFFHICategory.InitCheckBands( CheckEngine.DbAuxConnection, ref Result );
            HourlyEmissionsDataCategory.InitCheckBands( CheckEngine.DbAuxConnection, ref Result );
            SummaryValueDataCategory.InitCheckBands( CheckEngine.DbAuxConnection, ref Result );

            SetCheckParameter( "FACILITY_LOCATION_RECORDS", null, eParameterDataType.DataView );

            int RptPeriodId = ((mCheckEngine.RptPeriodId.Value > 0) ? mCheckEngine.RptPeriodId.Value : int.MinValue);
            SetCheckParameter( "Current_Reporting_Period", RptPeriodId, eParameterDataType.Integer );
            SetCheckParameter( "Current_Reporting_Period_Year", mCheckEngine.RptPeriodYear.Value, eParameterDataType.Integer );
            SetCheckParameter( "Current_Reporting_Period_Quarter", mCheckEngine.RptPeriodQuarter.Value, eParameterDataType.Integer );
            SetCheckParameter( "First_ECMPS_Reporting_Period_Object", mCheckEngine.FirstEcmpsReportingPeriod, eParameterDataType.Object );

            DataView MonitorLocationView = mSourceData.Tables["MPMonitorLocation"].DefaultView;
            MonitorLocationView.Sort = "mon_loc_id";
            SetCheckParameter( "Monitoring_Plan_Location_Records", MonitorLocationView, eParameterDataType.DataView );
            SetCheckParameter( "Current_Location_Count", mSourceData.Tables["MPMonitorLocation"].Rows.Count, eParameterDataType.Integer );
            SetCheckParameter( "Fuel_Type_Reality_Checks_for_GCV_Cross_Check_Table", mSourceData.Tables["CrossCheck_FuelTypeRealityChecksforGCV"].DefaultView, eParameterDataType.DataView );
            SetCheckParameter( "Fuel_Type_Warning_Levels_for_GCV_Cross_Check_Table", mSourceData.Tables["CrossCheck_FuelTypeWarningLevelsforGCV"].DefaultView, eParameterDataType.DataView );

            RunResult = LMEInitializationCategory.ProcessChecks();

            //if severity not fatal, do the rest of the categories
            if( mCheckEngine.SeverityCd != eSeverityCd.FATAL )
            {
                string monLocID;
                for( int locnIndex = 0; locnIndex < MonitorLocationView.Count; locnIndex++ )
                {
                    SetCheckParameter( "Current_Monitor_Plan_Location_Postion", locnIndex, eParameterDataType.Integer );
                    SetCheckParameter( "Current_Monitor_Plan_Location_Record", MonitorLocationView[locnIndex], eParameterDataType.DataRowView );

                    monLocID = cDBConvert.ToString( MonitorLocationView[locnIndex]["mon_loc_id"] );

                    mSourceData.Tables["LTFF"].DefaultView.RowFilter = "rpt_period_id = " + RptPeriodId + " and mon_loc_id = '" + monLocID + "'";

                    foreach( DataRowView LTFFRecord in mSourceData.Tables["LTFF"].DefaultView )
                    {
                        LTFFHICategory.ProcessChecks( cDBConvert.ToString( LTFFRecord["ltff_id"] ) );
                        SaveLTFFData( cDBConvert.ToString( LTFFRecord["ltff_id"] ) );
                        LTFFHICategory.EraseParameters();
                    }
                }
                if( mCheckEngine.SeverityCd != eSeverityCd.FATAL )
                {

                    for( int locnIndex = 0; locnIndex < MonitorLocationView.Count; locnIndex++ )
                    {
                        SetCheckParameter( "Current_Monitor_Plan_Location_Postion", locnIndex, eParameterDataType.Integer );
                        SetCheckParameter( "Current_Monitor_Plan_Location_Record", MonitorLocationView[locnIndex], eParameterDataType.DataRowView );

                        monLocID = cDBConvert.ToString( MonitorLocationView[locnIndex]["mon_loc_id"] );

                        if( MonitorLocationView[locnIndex]["unit_id"] != DBNull.Value )
                        {
                            string sUnitID = cDBConvert.ToString( MonitorLocationView[locnIndex]["unit_id"] );

                            DateTime OpDate = CheckEngine.EvaluationBeganDate.Value;
                            while( OpDate <= CheckEngine.EvaluationEndedDate.Value )
                            {
                                //if (OpDate.ToShortDateString() == new DateTime(2004, 5, 2).ToShortDateString())
                                //{
                                //    //break point here so know time to test checks
                                //}
                                for( int OpHr = 0; OpHr < 24; OpHr++ )
                                {
                                    //HourlyEmissionsDataCategory.ProcessChecks(OpDate, OpHr, sUnitID);
                                    HourlyEmissionsDataCategory.ProcessChecks( OpDate, OpHr, monLocID );
                                    SaveLMEDerivedRecords( monLocID );
                                    HourlyEmissionsDataCategory.EraseParameters();
                                }
                                OpDate = OpDate.AddDays( 1 );
                            }
                        }
                        SummaryValueDataCategory.ProcessChecks( monLocID );
                        SaveLMESummaryValueRecords();
                        SummaryValueDataCategory.EraseParameters();
                    }
                }
            }

            LMEInitializationCategory.EraseParameters();

            DbUpdate( ref Result );

            UpdateViewEmissionsTables( ref Result );

            return Result;
        }

        protected override void InitCalculatedData()
        {
            string ErrorMsg = "";

            FGenLTFF = CloneTable( "ECMPS_WS", "CL_LongTermFuelFlow", mCheckEngine.DbDataConnection.SQLConnection, ref ErrorMsg );
            FGenDerivedHrlyValue = CloneTable( "ECMPS_WS", "CL_DerivedHourlyValue", mCheckEngine.DbDataConnection.SQLConnection, ref ErrorMsg );
            FGenSummaryValue = CloneTable( "ECMPS_WS", "CL_SummaryValue", mCheckEngine.DbDataConnection.SQLConnection, ref ErrorMsg );
        }

        protected override void InitSourceData()
        {
            mSourceData = new DataSet();

            string sMonPlanWhereClause = string.Format( " WHERE MON_PLAN_ID = '{0}'", mCheckEngine.MonPlanId );

            //OrderBy needed for filtering method.
            AddSourceDataTable( "LMEHourlyOp", "VW_MP_HRLY_OP_DATA", sMonPlanWhereClause, "ORDER BY MON_LOC_ID, BEGIN_DATE, BEGIN_HOUR" );

            AddSourceDataTable( "LTFF", "VW_MP_Long_Term_Fuel_Flow", sMonPlanWhereClause );
            AddSourceDataTable( "MPSystem", "vw_mp_monitor_system", sMonPlanWhereClause );
            AddSourceDataTable( "MPMethod", "VW_MP_MONITOR_METHOD", sMonPlanWhereClause );
            AddSourceDataTable( "MPQualification", "VW_MP_MONITOR_QUALIFICATION", sMonPlanWhereClause );
            AddSourceDataTable( "MPMonitorLocation", "vw_MP_Monitor_Location", sMonPlanWhereClause );
            AddSourceDataTable( "MPLoad", "VW_MP_MONITOR_LOAD", sMonPlanWhereClause );
            AddSourceDataTable( "MPDefault", "VW_MP_MONITOR_DEFAULT", sMonPlanWhereClause );
            AddSourceDataTable( "MPOpSuppData", "VW_MP_OP_SUPP_DATA", sMonPlanWhereClause );
            AddSourceDataTable( "UnitStackConfig", "VW_UNIT_STACK_CONFIGURATION" );
            AddSourceDataTable( "LocationProgram", "VW_LOCATION_PROGRAM" );
            AddSourceDataTable( "ReportingPeriod", "vw_reporting_period" );

            LoadCrossCheck( "Fuel Type Warning Levels for GCV" );
            LoadCrossCheck( "Fuel Type Reality Checks for GCV" );
        }

        /// <summary>
        /// Initializes the Check Parameters object to a Default Check Parameters instance.  The default
        /// does not implement any parameters as properties and processes that do should override this
        /// method and set the Check Parameters object to and instance that implements parameters as
        /// properties.
        /// </summary>
        protected override void InitCheckParameters()
        {
            ProcessParameters = new cLmeCheckParameters( this, mCheckEngine.DbAuxConnection );
        }

        /// <summary>
        /// This method initializes the class containing static properties enabling strongly typed access to the parameters used by the process.
        /// </summary>
        protected override void InitStaticParameterClass()
        {
          EmParameters.Init(this);
        }

        /// <summary>
        /// Allows the setting of the current category for which parameters will be set.
        /// </summary>
        /// <param name="category"></param>
        public override void SetStaticParameterCategory(cCategory category)
        {
          EmParameters.Category = category;
        }

        #endregion

        #region Protected Override: DB Update

        /// <summary>
        /// Loads ECMPS_WS tables for the process with calculated values.
        /// </summary>
        /// <param name="sqlTransaction">The transaction to use with any commands.  Use null for no transaction.</param>
        /// <param name="errorMessage">The error message returned on failure.</param>
        /// <returns>Returns true if the update succeeds.</returns>
        protected override bool DbUpdate_CalcWsLoad( SqlTransaction sqlTransaction, ref string errorMessage )
        {
            bool result;

            if( SeverityCd != eSeverityCd.FATAL && SeverityCd != eSeverityCd.CRIT1 )
            {
                if( DbWsConnection.ClearUpdateSession( eWorkspaceDataType.LME, mCheckEngine.WorkspaceSessionId ) )
                {
                    if( DbWsConnection.BulkLoad( FGenDerivedHrlyValue, "CL_DerivedHourlyValue", ref errorMessage ) &&
                        DbWsConnection.BulkLoad( FGenSummaryValue, "CL_SummaryValue", ref errorMessage ) &&
                        DbWsConnection.BulkLoad( FGenLTFF, "CL_LongTermFuelFlow", ref errorMessage ) )
                        result = true;
                    else
                        result = false;
                }
                else
                {
                    errorMessage = DbWsConnection.LastError;
                    result = false;
                }
            }
            else
                result = true;

            return result;
        }

        /// <summary>
        /// The Update ECMPS Status process identifier.
        /// </summary>
        protected override string DbUpdate_EcmpsStatusProcess { get { return "LME Generate"; } }

        /// <summary>
        /// The Update ECMPS Status id key or list for the item(s) for which the update will occur.
        /// </summary>
        protected override string DbUpdate_EcmpsStatusIdKeyOrList { get { return mCheckEngine.MonPlanId; } }

        /// <summary>
        /// The Update ECMPS Status report period id for the item(s) for which the update will occur.
        /// </summary>
        protected override int? DbUpdate_EcmpsStatusPeriodId { get { return mCheckEngine.RptPeriodId.Value; } }

        /// <summary>
        /// Determine whether to update calculated values and ECMPS Status.
        /// </summary>
        protected override bool DbUpdate_UpdateData { get { return ((SeverityCd != eSeverityCd.FATAL && SeverityCd != eSeverityCd.CRIT1)); } }

        /// <summary>
        /// Returns the WS data type for the process.
        /// </summary>
        /// <returns>The workspace data type for the process, or null for none.</returns>
        protected override eWorkspaceDataType? DbUpdate_WorkspaceDataType { get { return eWorkspaceDataType.LME; } }

        #endregion

        #region Handle Generated Data

        private DataTable FGenLTFF;
        private DataTable FGenDerivedHrlyValue;
        private DataTable FGenSummaryValue;

        # region LTFF Data

        private void SaveLTFFData( string LTFFID )
        {
            decimal LMEGenLTFFHI = GetCheckParameter( "LME_Gen_LTFF_Heat_Input" ).ValueAsDecimal();

            if( LMEGenLTFFHI != decimal.MinValue )
            {
                DataRow GenRow = FGenLTFF.NewRow();
                GenRow["LTFF_ID"] = LTFFID;
                GenRow["Total_Heat_Input"] = LMEGenLTFFHI;
                GenRow["Session_Id"] = mCheckEngine.WorkspaceSessionId;

                FGenLTFF.Rows.Add( GenRow );
            }
        }

        # endregion

        # region Derived Data

        private void SaveLMEDerivedRecords( string MonLocID )
        {
            SaveLMEGenHIRecord( MonLocID );
            SaveLMEGenNOXMRecord( MonLocID );
            SaveLMEGenCO2MRecord( MonLocID );
            SaveLMEGenSO2MRecord( MonLocID );
        }

        private void SaveLMEGenHIRecord( string MonLocID )
        {
            DataRowView CurrentDhvRecord = GetCheckParameter( "LME_Gen_Heat_Input_Record" ).ValueAsDataRowView();

            if( CurrentDhvRecord != null )
            {
                DataRow GenRow = FGenDerivedHrlyValue.NewRow();

                GenRow["HOUR_ID"] = cDBConvert.ToString( CurrentDhvRecord["HOUR_ID"] );
                GenRow["ADJUSTED_HRLY_VALUE"] = cDBConvert.ToString( CurrentDhvRecord["ADJUSTED_HRLY_VALUE"] );
                GenRow["PARAMETER_CD"] = cDBConvert.ToString( CurrentDhvRecord["PARAMETER_CD"] );
                GenRow["MODC_CD"] = CurrentDhvRecord["MODC_CD"]; //if null, DBNull.Value else convert to ?string?
                GenRow["SESSION_ID"] = mCheckEngine.WorkspaceSessionId;
                GenRow["RPT_PERIOD_ID"] = mCheckEngine.RptPeriodId;
                GenRow["MON_LOC_ID"] = MonLocID;

                FGenDerivedHrlyValue.Rows.Add( GenRow );
            }
        }

        private void SaveLMEGenNOXMRecord( string MonLocID )
        {
            DataRowView CurrentDhvRecord = GetCheckParameter( "LME_Gen_NOXM_Record" ).ValueAsDataRowView();

            if( CurrentDhvRecord != null )
            {
                DataRow GenRow = FGenDerivedHrlyValue.NewRow();

                GenRow["HOUR_ID"] = cDBConvert.ToString( CurrentDhvRecord["HOUR_ID"] );
                GenRow["ADJUSTED_HRLY_VALUE"] = cDBConvert.ToString( CurrentDhvRecord["ADJUSTED_HRLY_VALUE"] );
                GenRow["PARAMETER_CD"] = cDBConvert.ToString( CurrentDhvRecord["PARAMETER_CD"] );
                GenRow["FUEL_CD"] = CurrentDhvRecord["FUEL_CD"]; //if null, DBNull.Value else convert to ?string?
                GenRow["OPERATING_CONDITION_CD"] = CurrentDhvRecord["OPERATING_CONDITION_CD"];
                GenRow["SESSION_ID"] = mCheckEngine.WorkspaceSessionId;
                GenRow["RPT_PERIOD_ID"] = mCheckEngine.RptPeriodId;
                GenRow["MON_LOC_ID"] = MonLocID;

                FGenDerivedHrlyValue.Rows.Add( GenRow );
            }
        }

        private void SaveLMEGenCO2MRecord( string MonLocID )
        {
            DataRowView CurrentDhvRecord = GetCheckParameter( "LME_Gen_CO2M_Record" ).ValueAsDataRowView();

            if( CurrentDhvRecord != null )
            {
                DataRow GenRow = FGenDerivedHrlyValue.NewRow();

                GenRow["HOUR_ID"] = cDBConvert.ToString( CurrentDhvRecord["HOUR_ID"] );
                GenRow["ADJUSTED_HRLY_VALUE"] = cDBConvert.ToString( CurrentDhvRecord["ADJUSTED_HRLY_VALUE"] );
                GenRow["PARAMETER_CD"] = cDBConvert.ToString( CurrentDhvRecord["PARAMETER_CD"] );
                GenRow["FUEL_CD"] = CurrentDhvRecord["FUEL_CD"]; //if null, DBNull.Value else convert to ?string?
                GenRow["SESSION_ID"] = mCheckEngine.WorkspaceSessionId;
                GenRow["RPT_PERIOD_ID"] = mCheckEngine.RptPeriodId;
                GenRow["MON_LOC_ID"] = MonLocID;

                FGenDerivedHrlyValue.Rows.Add( GenRow );
            }
        }

        private void SaveLMEGenSO2MRecord( string MonLocID )
        {
            DataRowView CurrentDhvRecord = GetCheckParameter( "LME_Gen_SO2M_Record" ).ValueAsDataRowView();

            if( CurrentDhvRecord != null )
            {
                DataRow GenRow = FGenDerivedHrlyValue.NewRow();

                GenRow["HOUR_ID"] = cDBConvert.ToString( CurrentDhvRecord["HOUR_ID"] );
                GenRow["ADJUSTED_HRLY_VALUE"] = cDBConvert.ToString( CurrentDhvRecord["ADJUSTED_HRLY_VALUE"] );
                GenRow["PARAMETER_CD"] = cDBConvert.ToString( CurrentDhvRecord["PARAMETER_CD"] );
                GenRow["FUEL_CD"] = CurrentDhvRecord["FUEL_CD"]; //if null, DBNull.Value else convert to ?string?
                GenRow["SESSION_ID"] = mCheckEngine.WorkspaceSessionId;
                GenRow["RPT_PERIOD_ID"] = mCheckEngine.RptPeriodId;
                GenRow["MON_LOC_ID"] = MonLocID;

                FGenDerivedHrlyValue.Rows.Add( GenRow );
            }
        }

        # endregion

        # region Summary Data

        private void SaveLMESummaryValueRecords()
        {
            SaveLMESummaryRecord( "LME_Summary_Heat_Input_Record" );
            SaveLMESummaryRecord( "LME_Summary_Op_Time_Record" );
            SaveLMESummaryRecord( "LME_Summary_Op_Hours_Record" );
            SaveLMESummaryRecord( "LME_Summary_NOXM_Record" );
            SaveLMESummaryRecord( "LME_Summary_NOXR_Record" );
            SaveLMESummaryRecord( "LME_Summary_SO2M_Record" );
            SaveLMESummaryRecord( "LME_Summary_CO2M_Record" );

        }

        private void SaveLMESummaryRecord( string RecordParameterName )
        {
            DataRowView CurrentSummaryRecord = GetCheckParameter( RecordParameterName ).ValueAsDataRowView();

            if( CurrentSummaryRecord != null )
            {
                DataRow GenRow = FGenSummaryValue.NewRow();

                GenRow["MON_LOC_ID"] = cDBConvert.ToString( CurrentSummaryRecord["MON_LOC_ID"] );
                GenRow["RPT_PERIOD_ID"] = cDBConvert.ToString( CurrentSummaryRecord["RPT_PERIOD_ID"] );
                GenRow["PARAMETER_CD"] = cDBConvert.ToString( CurrentSummaryRecord["PARAMETER_CD"] );
                GenRow["CURRENT_RPT_PERIOD_TOTAL"] = CurrentSummaryRecord["CURRENT_RPT_PERIOD_TOTAL"]; //if null, DBNull.Value else convert to ?string?
                if( CurrentSummaryRecord.Row.Table.Columns.Contains( "os_total" ) )
                    GenRow["OS_TOTAL"] = CurrentSummaryRecord["OS_TOTAL"]; //if null, DBNull.Value else convert to ?string?
                GenRow["YEAR_TOTAL"] = CurrentSummaryRecord["YEAR_TOTAL"]; //if null, DBNull.Value else convert to ?string?
                GenRow["SESSION_ID"] = mCheckEngine.WorkspaceSessionId;

                FGenSummaryValue.Rows.Add( GenRow );
            }
        }

        # endregion

        #endregion

        #region Private Methods

        private void LoadCrossChecks()
        {
            DataTable Catalog = mCheckEngine.DbAuxConnection.GetDataTable( "SELECT * FROM vw_Cross_Check_Catalog" );
            DataTable Value = mCheckEngine.DbAuxConnection.GetDataTable( "SELECT * FROM vw_Cross_Check_Catalog_Value" );
            DataTable CrossCheck;
            DataRow CrossCheckRow;
            string CrossCheckName;
            string Column1Name;
            string Column2Name;
            string Column3Name;

            foreach( DataRow CatalogRow in Catalog.Rows )
            {
                CrossCheckName = (string)CatalogRow["Cross_Chk_Catalog_Name"];
                CrossCheckName = CrossCheckName.Replace( " ", "" );

                CrossCheck = new DataTable( "CrossCheck_" + CrossCheckName );

                Column1Name = (string)CatalogRow["Description1"];
                Column2Name = (string)CatalogRow["Description2"];

                CrossCheck.Columns.Add( Column1Name );
                CrossCheck.Columns.Add( Column2Name );

                if( CatalogRow["Description3"] != DBNull.Value )
                {
                    Column3Name = (string)CatalogRow["Description3"];
                    CrossCheck.Columns.Add( Column3Name );
                }
                else Column3Name = "";

                Column1Name.Replace( " ", "" );
                Column2Name.Replace( " ", "" );
                Column3Name.Replace( " ", "" );

                Value.DefaultView.RowFilter = "Cross_Chk_Catalog_Id = " + cDBConvert.ToString( CatalogRow["Cross_Chk_Catalog_Id"] );

                foreach( DataRowView ValueRow in Value.DefaultView )
                {
                    CrossCheckRow = CrossCheck.NewRow();

                    CrossCheckRow[Column1Name] = ValueRow["Value1"];
                    CrossCheckRow[Column2Name] = ValueRow["Value2"];

                    if( CatalogRow["Description3"] != DBNull.Value )
                        CrossCheckRow[Column3Name] = ValueRow["Value3"];

                    CrossCheck.Rows.Add( CrossCheckRow );
                }
                mSourceData.Tables.Add( CrossCheck );
            }
        }

        private void UpdateViewEmissionsTables( ref string Message )
        {
            //Recreate the View Emission grid data

            cDatabase conn = mCheckEngine.DbDataConnection;
            conn.CreateStoredProcedureCommand( "Client_EM_Grid_Data" );
            conn.AddInputParameter( "@V_MON_PLAN_ID", CheckEngine.MonPlanId );
            conn.AddInputParameter( "@V_RPT_PERIOD_ID", CheckEngine.RptPeriodId.Value );
            conn.AddOutputParameterString( "@V_RESULT", 1 );
            conn.AddOutputParameterString( "@V_ERROR_MSG", 200 );

            conn.ExecuteNonQuery();

            if( conn.GetParameterString( "@V_RESULT" ) != "T" )
                // Message = "";
                //else
                Message = Message == "" ? conn.GetParameterString( "@V_ERROR_MSG" ) : Message + " and " + "\n" + conn.GetParameterString( "@V_ERROR_MSG" );
            //Message = cList.FormatList(Message);
        }

        private void LoadCrossCheck( string sTableNm )
        {
            int nCrossCheckCatalogID = 0;
            string sSQL = string.Format( "SELECT * FROM vw_Cross_Check_Catalog WHERE cross_chk_catalog_name = '{0}'", sTableNm );

            DataTable Catalog = mCheckEngine.DbAuxConnection.GetDataTable( sSQL );
            if( Catalog != null && Catalog.Rows.Count >= 1 )
                nCrossCheckCatalogID = Catalog.Rows[0].Field<int>( "CROSS_CHK_CATALOG_ID" );

            sSQL = string.Format( "SELECT * FROM vw_Cross_Check_Catalog_Value WHERE CROSS_CHK_CATALOG_ID = {0}", nCrossCheckCatalogID );
            DataTable Value = mCheckEngine.DbAuxConnection.GetDataTable( sSQL );
            DataTable CrossCheck;
            DataRow CrossCheckRow;
            string CrossCheckName;
            string Column1Name;
            string Column2Name;
            string Column3Name;

            foreach( DataRow CatalogRow in Catalog.Rows )
            {
                CrossCheckName = (string)CatalogRow["Cross_Chk_Catalog_Name"];
                CrossCheckName = CrossCheckName.Replace( " ", "" );

                CrossCheck = new DataTable( "CrossCheck_" + CrossCheckName );

                Column1Name = (string)CatalogRow["Description1"];
                Column2Name = (string)CatalogRow["Description2"];

                CrossCheck.Columns.Add( Column1Name );
                CrossCheck.Columns.Add( Column2Name );

                if( CatalogRow["Description3"] != DBNull.Value )
                {
                    Column3Name = (string)CatalogRow["Description3"];
                    CrossCheck.Columns.Add( Column3Name );
                }
                else Column3Name = "";

                Column1Name.Replace( " ", "" );
                Column2Name.Replace( " ", "" );
                Column3Name.Replace( " ", "" );

                Value.DefaultView.RowFilter = "Cross_Chk_Catalog_Id = " + cDBConvert.ToString( CatalogRow["Cross_Chk_Catalog_Id"] );

                foreach( DataRowView ValueRow in Value.DefaultView )
                {
                    CrossCheckRow = CrossCheck.NewRow();

                    CrossCheckRow[Column1Name] = ValueRow["Value1"];
                    CrossCheckRow[Column2Name] = ValueRow["Value2"];

                    if( CatalogRow["Description3"] != DBNull.Value )
                        CrossCheckRow[Column3Name] = ValueRow["Value3"];

                    CrossCheck.Rows.Add( CrossCheckRow );
                }
                mSourceData.Tables.Add( CrossCheck );
            }
        }

        #endregion
    }
}
