using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.CheckEngine.Definitions;
using ECMPS.Checks.DatabaseAccess;
using ECMPS.Checks.MpImport.Parameters;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Common;
using ECMPS.Definitions.SeverityCode;

namespace ECMPS.Checks.MonitorPlanImport
{
    public class cMonitorPlanImportProcess : cProcess
    {
        private static decimal _nSessionID = 0;

        #region Constructors

        public cMonitorPlanImportProcess( cCheckEngine CheckEngine, decimal nSessionID )
            : base( CheckEngine, SaveSessionID( nSessionID, "MPIMPRT" ) )
        {
            _nSessionID = nSessionID;
        } 

        //
        // Due to the way that constructors are processed, and the fact that the
        // base constructor for cProcess calls InitSourceData(), which needs the session_id,
        // we have to do this kludge!
        //
        private static string SaveSessionID( decimal session, string ProcessCd )
        {
            _nSessionID = session;
            return ProcessCd;
        }

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
            Checks[28] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.Import.dll",
                                                               "ECMPS.Checks.ImportChecks.cImportChecks").Unwrap();

            result = true;
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

            // Facility
            cCheckParameterBands ImportChecks = GetCheckBands(  "MPFAC" );
            cMPFacilityCategory facilityCategory = new cMPFacilityCategory( mCheckEngine, this );

            RunResult = facilityCategory.ProcessChecks( ImportChecks );

            // process the next category only if we don't have a "FATAL" result 
            if( this.SeverityCd != eSeverityCd.FATAL )
            {   // Unit
                cMPUnitCategory unitCategory = null;
                ImportChecks = GetCheckBands(  "MPUNIT" );
                foreach( DataRow drWSUnit in mSourceData.Tables["WS_Unit"].Rows )
                {
                    string sUnitPK = drWSUnit["UNIT_PK"].ToString();
                    unitCategory = new cMPUnitCategory( mCheckEngine, this, sUnitPK );
                    RunResult = unitCategory.ProcessChecks( ImportChecks );

                    if (unitCategory.SeverityCd == eSeverityCd.CRIT1)
                    {   // delete this record from the workspace
                        DeleteRecord( "MP_Unit", "UNIT_PK", sUnitPK );
                    }

                    unitCategory.EraseParameters();
                }
            }

            // process the next category only if we don't have a "FATAL" result 
            if( this.SeverityCd != eSeverityCd.FATAL )
            {   // Stack/Pipe
                cMPStackCategory stackCategory = null;
                ImportChecks = GetCheckBands(  "MPSTACK" );
                foreach( DataRow drWSStack in mSourceData.Tables["WS_StackPipe"].Rows )
                {
                    string sSP_PK = drWSStack["SP_PK"].ToString();
                    stackCategory = new cMPStackCategory( mCheckEngine, this, sSP_PK );
                    RunResult = stackCategory.ProcessChecks( ImportChecks );

                    if( stackCategory.SeverityCd == eSeverityCd.CRIT1 )
                    {   // delete this record from the workspace
                        DeleteRecord( "MP_StackPipe", "SP_PK", sSP_PK );
                    }

                    stackCategory.EraseParameters();
                }
            }

            // process the next category only if we don't have a "FATAL" result 
            if( this.SeverityCd != eSeverityCd.FATAL )
            {   // Unit Stack Config
                cMPUnitStackConfigCategory uscCategory = null;
                ImportChecks = GetCheckBands(  "MPUNSTK" );
                foreach( DataRow drWSStack in mSourceData.Tables["WS_UnitStackConfig"].Rows )
                {
                    string sUSC_PK = drWSStack["USC_PK"].ToString();
                    uscCategory = new cMPUnitStackConfigCategory( mCheckEngine, this, sUSC_PK );
                    RunResult = uscCategory.ProcessChecks( ImportChecks );

                    if( uscCategory.SeverityCd == eSeverityCd.CRIT1 )
                    {   // delete this record from the workspace
                        DeleteRecord( "MP_UnitStackConfig", "USC_PK", sUSC_PK );
                    }

                    uscCategory.EraseParameters();
                }
            }

            // process the next category only if we don't have a "FATAL" result 
            if( this.SeverityCd != eSeverityCd.FATAL )
            {   // Component
                cMPComponentCategory componentCategory = null;
                ImportChecks = GetCheckBands(  "MPCOMP" );
                foreach( DataRow drWSComp in mSourceData.Tables["WS_Component"].Rows )
                {
                    string sComponent_PK = drWSComp["COMPONENT_PK"].ToString();
                    componentCategory = new cMPComponentCategory( mCheckEngine, this, sComponent_PK );
                    RunResult = componentCategory.ProcessChecks( ImportChecks );

                    if( componentCategory.SeverityCd == eSeverityCd.CRIT1 )
                    {   // delete this record from the workspace
                        DeleteRecord( "MP_Component", "COMPONENT_PK", sComponent_PK );
                    }

                    componentCategory.EraseParameters();
                }
            }

            // process the next category only if we don't have a "FATAL" result 
            if( this.SeverityCd != eSeverityCd.FATAL )
            {   // Analyzer Range
                cMPAnalyzerRangeCategory arCategory = null;
                ImportChecks = GetCheckBands(  "MPANAL" );
                foreach( DataRow drWSAR in mSourceData.Tables["WS_AnalyzerRange"].Rows )
                {
                    string sAR_PK = drWSAR["AR_PK"].ToString();
                    arCategory = new cMPAnalyzerRangeCategory( mCheckEngine, this, sAR_PK );
                    RunResult = arCategory.ProcessChecks( ImportChecks );

                    if( arCategory.SeverityCd == eSeverityCd.CRIT1 )
                    {   // delete this record from the workspace
                        DeleteRecord( "MP_AnalyzerRange", "AR_PK", sAR_PK );
                    }

                    arCategory.EraseParameters();
                }
            }

            // process the next category only if we don't have a "FATAL" result 
            if( this.SeverityCd != eSeverityCd.FATAL )
            {   // Analyzer Range
                cMPCalibrationStandardCategory csCategory = null;
                ImportChecks = GetCheckBands( "MPCALST" );
                foreach( DataRow drWSCS in mSourceData.Tables["WS_CalibrationStandard"].Rows )
                {
                    string sCS_PK = drWSCS["CS_PK"].ToString();
                    csCategory = new cMPCalibrationStandardCategory( mCheckEngine, this, sCS_PK );
                    RunResult = csCategory.ProcessChecks( ImportChecks );

                    if( csCategory.SeverityCd == eSeverityCd.CRIT1 )
                    {   // delete this record from the workspace
                        DeleteRecord( "MP_CalibrationStandard", "CS_PK", sCS_PK );
                    }

                    csCategory.EraseParameters();
                }
            }

            // process the next category only if we don't have a "FATAL" result 
            if( this.SeverityCd != eSeverityCd.FATAL )
            {   // System
                cMPSystemCategory sysCategory = null;
                ImportChecks = GetCheckBands(  "MPSYS" );
                foreach( DataRow drWSSystem in mSourceData.Tables["WS_MonitoringSys"].Rows )
                {
                    string sMonSys_PK = drWSSystem["MonSys_PK"].ToString();
                    sysCategory = new cMPSystemCategory( mCheckEngine, this, sMonSys_PK );
                    RunResult = sysCategory.ProcessChecks( ImportChecks );

                    if( sysCategory.SeverityCd == eSeverityCd.CRIT1 )
                    {   // delete this record from the workspace
                        DeleteRecord( "MP_MonitoringSys", "MonSys_PK", sMonSys_PK );
                    }

                    sysCategory.EraseParameters();
                }
            }

            // process the next category only if we don't have a "FATAL" result 
            if( this.SeverityCd != eSeverityCd.FATAL )
            {   // System Component
                cMPSysComponentCategory sysCompCategory = null;
                ImportChecks = GetCheckBands(  "MPSYSCP" );
                foreach( DataRow drWSSysComp in mSourceData.Tables["WS_MonitoringSysComponent"].Rows )
                {
                    string sMSC_PK = drWSSysComp["MSC_PK"].ToString();
                    sysCompCategory = new cMPSysComponentCategory( mCheckEngine, this, sMSC_PK );
                    RunResult = sysCompCategory.ProcessChecks( ImportChecks );

                    if( sysCompCategory.SeverityCd == eSeverityCd.CRIT1 )
                    {   // delete this record from the workspace
                        DeleteRecord( "MP_MonitoringSysComponent", "MSC_PK", sMSC_PK );
                    }

                    sysCompCategory.EraseParameters();
                }
            }

            // process the next category only if we don't have a "FATAL" result 
            if( this.SeverityCd != eSeverityCd.FATAL )
            {   // System Fuel Flow
                cMPSysFuelFlowCategory sysFFCategory = null;
                ImportChecks = GetCheckBands(  "MPSYSFF" );
                foreach( DataRow drWSSysComp in mSourceData.Tables["WS_MonitoringSysFuelFlow"].Rows )
                {
                    string sMSFF_PK = drWSSysComp["MSFF_PK"].ToString();
                    sysFFCategory = new cMPSysFuelFlowCategory( mCheckEngine, this, sMSFF_PK );
                    RunResult = sysFFCategory.ProcessChecks( ImportChecks );

                    if( sysFFCategory.SeverityCd == eSeverityCd.CRIT1 )
                    {   // delete this record from the workspace
                        DeleteRecord( "MP_MonitoringSysFuelFlow", "MSFF_PK", sMSFF_PK );
                    }

                    sysFFCategory.EraseParameters();
                }
            }

            // process the next category only if we don't have a "FATAL" result 
            if( this.SeverityCd != eSeverityCd.FATAL )
            {   // Formula
                cMPFormulaCategory formCategory = null;
                ImportChecks = GetCheckBands(  "MPFORM" );
                foreach( DataRow drWSFormula in mSourceData.Tables["WS_MonitoringFormula"].Rows )
                {
                    string sMF_PK = drWSFormula["MF_PK"].ToString();
                    formCategory = new cMPFormulaCategory( mCheckEngine, this, sMF_PK );
                    RunResult = formCategory.ProcessChecks( ImportChecks );

                    if( formCategory.SeverityCd == eSeverityCd.CRIT1 )
                    {   // delete this record from the workspace
                        DeleteRecord( "MP_MonitoringFormula", "MF_PK", sMF_PK );
                    }

                    formCategory.EraseParameters();
                }
            }

            // process the next category only if we don't have a "FATAL" result 
            if( this.SeverityCd != eSeverityCd.FATAL )
            {   // SPAN
                cMPSpanCategory spanCategory = null;
                ImportChecks = GetCheckBands(  "MPSPAN" );
                foreach( DataRow drWSSpan in mSourceData.Tables["WS_MonitoringSpan"].Rows )
                {
                    string sMS_PK = drWSSpan["MS_PK"].ToString();
                    spanCategory = new cMPSpanCategory( mCheckEngine, this, sMS_PK );
                    RunResult = spanCategory.ProcessChecks( ImportChecks );

                    if( spanCategory.SeverityCd == eSeverityCd.CRIT1 )
                    {   // delete this record from the workspace
                        DeleteRecord( "MP_MonitoringSpan", "MS_PK", sMS_PK );
                    }

                    spanCategory.EraseParameters();
                }
            }

            // process the next category only if we don't have a "FATAL" result 
            if( this.SeverityCd != eSeverityCd.FATAL )
            {   // QUAL
                cMPQualCategory qualCategory = null;
                ImportChecks = GetCheckBands(  "MPQUAL" );
                foreach( DataRow drWSQaul in mSourceData.Tables["WS_MonitoringQual"].Rows )
                {
                    string sMQ_PK = drWSQaul["MQ_PK"].ToString();
                    qualCategory = new cMPQualCategory( mCheckEngine, this, sMQ_PK );
                    RunResult = qualCategory.ProcessChecks( ImportChecks );

                    if( qualCategory.SeverityCd == eSeverityCd.CRIT1 )
                    {   // delete this record from the workspace
                        DeleteRecord( "MP_MonitoringQual", "MQ_PK", sMQ_PK );
                    }

                    qualCategory.EraseParameters();
                }
            }

            // process the next category only if we don't have a "FATAL" result 
            if( this.SeverityCd != eSeverityCd.FATAL )
            {   // QUAL LME
                cMPQualLMECategory lmeCategory = null;
                ImportChecks = GetCheckBands(  "MPQLME" );
                foreach( DataRow drWSLME in mSourceData.Tables["WS_MonitoringQualLME"].Rows )
                {
                    string sMQLME_PK = drWSLME["MQLME_PK"].ToString();
                    lmeCategory = new cMPQualLMECategory( mCheckEngine, this, sMQLME_PK );
                    RunResult = lmeCategory.ProcessChecks( ImportChecks );

                    if( lmeCategory.SeverityCd == eSeverityCd.CRIT1 )
                    {   // delete this record from the workspace
                        DeleteRecord( "MP_MonitoringQualLME", "MQLME_PK", sMQLME_PK );
                    }

                    lmeCategory.EraseParameters();
                }
            }

            facilityCategory.EraseParameters();

            DbUpdate(ref Result);

            return Result;
        }

        protected override void InitCalculatedData()
        {
        }

        protected override void InitSourceData()
        {
            mSourceData = new DataSet();

            // get the records from the workspace tables
            AddSourceDataTable_WS( "WS_MonitorPlan", "MP_MonitoringPlan" );
            AddSourceDataTable_WS( "WS_StackPipe", "VW_CHECK_MP_StackPipe" );
            AddSourceDataTable_WS( "WS_UnitStackConfig", "MP_UnitStackConfig" );
            AddSourceDataTable_WS( "WS_Unit", "VW_CHECK_MP_Unit" );
            AddSourceDataTable_WS( "WS_Component", "VW_CHECK_MP_Component" );
            AddSourceDataTable_WS( "WS_AnalyzerRange", "VW_CHECK_MP_AnalyzerRange" );
            AddSourceDataTable_WS( "WS_CalibrationStandard", "VW_CHECK_MP_CalibrationStandard" );
            AddSourceDataTable_WS( "WS_MonitoringSys", "VW_CHECK_MP_MonitoringSys" );
            AddSourceDataTable_WS( "WS_MonitoringSysComponent", "VW_CHECK_MP_MonitoringSysComponent" );
            AddSourceDataTable_WS( "WS_MonitoringSysFuelFlow", "VW_CHECK_MP_MonitoringSysFuelFlow" );
            AddSourceDataTable_WS( "WS_MonitoringFormula", "VW_CHECK_MP_MonitoringFormula" );
            AddSourceDataTable_WS( "WS_MonitoringSpan", "VW_CHECK_MP_MonitoringSpan" );
            AddSourceDataTable_WS( "WS_MonitoringQual", "VW_CHECK_MP_MonitoringQual" );
            AddSourceDataTable_WS( "WS_MonitoringQualPCT", "MP_MonitoringQualPCT" );
            AddSourceDataTable_WS( "WS_MonitoringQualLME", "VW_CHECK_MP_MonitoringQualLME" );

            // get the production tables now
            AddSourceDataFunction( "PROD_Unit", "CheckImp.Units", mCheckEngine.DbDataConnection );
            AddSourceDataFunction( "PROD_Component", "CheckImp.Components", mCheckEngine.DbDataConnection );
            AddSourceDataFunction( "PROD_Monitor_System", "CheckImp.Systems", mCheckEngine.DbDataConnection );
            AddSourceDataFunction( "PROD_Monitor_Formula", "CheckImp.Formulas", mCheckEngine.DbDataConnection );
            AddSourceDataFunction( "PROD_Monitor_Plan_Location", "CheckImp.MPLocations", mCheckEngine.DbDataConnection );
            AddSourceDataTable( "PROD_Facility", "FACILITY", "WHERE ORIS_CODE=" + mCheckEngine.ORISCode, mCheckEngine.DbDataConnection );
        }

        /// <summary>
        /// This method initializes the class containing static properties enabling strongly typed access to the parameters used by the process.
        /// </summary>
        protected override void InitStaticParameterClass()
        {
          MpImportParameters.Init(this);
        }

        /// <summary>
        /// Allows the setting of the current category for which parameters will be set.
        /// </summary>
        /// <param name="category"></param>
        public override void SetStaticParameterCategory(cCategory category)
        {
          MpImportParameters.Category = category;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Add a DataTable to mSourceData from Workspace database
        /// </summary>
        /// <param name="sDataTableNm">The name of the new DataTable</param>
        /// <param name="sViewNm">The table/view to get data from</param>
        private void AddSourceDataTable_WS( string sDataTableNm, string sViewNm )
        {
            string sWhere = string.Format( "WHERE SUBMISSION_ID={0}", _nSessionID );
            AddSourceDataTable( sDataTableNm, sViewNm, sWhere, mCheckEngine.DbWsConnection );
        }

        /// <summary>
        /// Add a DataTable to mSourceData from Workspace database
        /// </summary>
        /// <param name="sDataTableNm">The name of the new DataTable</param>
        /// <param name="sViewNm">The table/view to get data from</param>
        /// <param name="sWhereClause">A where clause, if any</param>
        private void AddSourceDataTable_WS( string sDataTableNm, string sViewNm, string sWhereClause )
        {
            string sAnd = string.Format( " and SUBMISSION_ID={0}", _nSessionID );
            AddSourceDataTable( sDataTableNm, sViewNm, sWhereClause + sAnd, mCheckEngine.DbWsConnection );
        }

        /// <summary>
        /// Add a DataTable to mSourceData
        /// </summary>
        /// <param name="sDataTableNm">The name of the new DataTable</param>
        /// <param name="sViewNm">The table/view to get data from</param>
        /// <param name="sWhereClause">A where clause, if any</param>
        /// <param name="dbConn">The database connection to use</param>
        private void AddSourceDataTable( string sDataTableNm, string sViewNm, string sWhereClause, cDatabase dbConn )
        {
            SqlDataAdapter SourceDataAdapter;
            DataTable SourceDataTable;

            string sSQL = string.Format( "SELECT * FROM {0} ", sViewNm );
            if( string.IsNullOrEmpty( sWhereClause ) == false )
                sSQL += sWhereClause;

            SourceDataTable = new DataTable( sDataTableNm );
            SourceDataAdapter = new SqlDataAdapter( sSQL, dbConn.SQLConnection );
            // this defaults to 30 seconds if we don't override it
            if( SourceDataAdapter.SelectCommand != null )
                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill( SourceDataTable );
            mSourceData.Tables.Add( SourceDataTable );
        }


        /// <summary>
        /// Add Source Data from a function
        /// </summary>
        /// <param name="sDataTableNm">The name of the table</param>
        /// <param name="sFunctionNm">The function that returns the data</param>
        /// <param name="dbConn"></param>
        private void AddSourceDataFunction( string sDataTableNm, string sFunctionNm, cDatabase dbConn )
        {
            SqlDataAdapter SourceDataAdapter;
            DataTable SourceDataTable;

            string sSQL = string.Format( "SELECT * FROM {0}({1}) ", sFunctionNm, mCheckEngine.ORISCode );

            SourceDataTable = new DataTable( sDataTableNm );
            SourceDataAdapter = new SqlDataAdapter( sSQL, dbConn.SQLConnection );
            // this defaults to 30 seconds if we don't override it
            if( SourceDataAdapter.SelectCommand != null )
                SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
            SourceDataAdapter.Fill( SourceDataTable );
            mSourceData.Tables.Add( SourceDataTable );
        }

        /// <summary>
        /// Delete a workspace record from the given table
        /// </summary>
        /// <param name="sTableNm">The workspace table to delete from</param>
        /// <param name="sKeyNm">The name of the key field</param>
        /// <param name="sKeyValue">The value of the key field to delete</param>
        private void DeleteRecord( string sTableNm, string sKeyNm, string sKeyValue )
        {
            string sSQL = string.Format( "SET NOCOUNT ON; DELETE FROM {0} WHERE {1}='{2}' AND SUBMISSION_ID={3}; SELECT @@ROWCOUNT;", sTableNm, sKeyNm, sKeyValue, _nSessionID );
            int nRows = (int)mCheckEngine.DbWsConnection.ExecuteScalar( sSQL );
        }

        #endregion
    }
}
