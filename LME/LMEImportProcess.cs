using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.CheckEngine.Definitions;
using ECMPS.Checks.LmeImport.Parameters;
using ECMPS.Checks.TypeUtilities;
using ECMPS.Checks.DatabaseAccess;

using ECMPS.Common;
using ECMPS.Definitions.SeverityCode;

namespace ECMPS.Checks.LME
{
    public class cLMEImportProcess : cProcess
    {
        #region Constructors

        public cLMEImportProcess( cCheckEngine CheckEngine )
            : base( CheckEngine, "LMEIMPT" )
        {
        }

        #endregion

        #region Base Class Overrides

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
                Checks[45] = (cChecks)Activator.CreateInstanceFrom( checksDllPath + "ECMPS.Checks.LME.dll",
                                                                   "ECMPS.Checks.LMEChecks.cLMEChecks" ).Unwrap();

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

            //LME Emissions Integrity Data

            cLMEEmissionsDataIntegrityCategory LMEEmissionsCategory = new cLMEEmissionsDataIntegrityCategory( mCheckEngine, this );
            cLMEHourlyDataIntegrityCategory LMEHourlyCategory = new cLMEHourlyDataIntegrityCategory( mCheckEngine, this );

            LMEEmissionsCategory.InitCheckBands( CheckEngine.DbAuxConnection, ref Result );
            LMEHourlyCategory.InitCheckBands( CheckEngine.DbAuxConnection, ref Result );

            RunResult = LMEEmissionsCategory.ProcessChecks();

            // process the next category only if we don't have a "FATAL" result 
            if( this.SeverityCd != eSeverityCd.FATAL )
            {
                //LME Hourly Integrity Data
                foreach( DataRow drWSLME in mSourceData.Tables["WS_LMEImport"].Rows )
                {
                    string sLMEPK = drWSLME["LME_PK"].ToString();
                    RunResult = LMEHourlyCategory.ProcessChecks( sLMEPK );

                    if( LMEHourlyCategory.SeverityCd == eSeverityCd.CRIT1 )
                    {   // delete this record from the workspace
                        DeleteRecord( "EM_LMEImport", "LME_PK", sLMEPK );
                    }

                    LMEHourlyCategory.EraseParameters();
                }
            }

            LMEEmissionsCategory.EraseParameters();

            DbUpdate( ref Result );

            return Result;
        }

        protected override void InitCalculatedData()
        {

        }

        protected override void InitSourceData()
        {
            mSourceData = new DataSet();

            // get the records from the workspace tables
            AddSourceDataTable_WS( "WS_LMEImport", "VW_EM_LMEImport" );
            AddSourceDataTable_WS("WS_LmeEmDuplicates", "CheckImp.LmeEmDuplicates(null, null)");

            // get the production tables now
            string sORISCodeFilter = string.Format( "WHERE ORIS_CODE={0}", mCheckEngine.ORISCode );

            AddSourceDataTable_Data( "PROD_Facility", "FACILITY", sORISCodeFilter );
            AddSourceDataFunction( "PROD_Unit", "CheckImp.Units", mCheckEngine.DbDataConnection );
            AddSourceDataFunction( "PROD_Monitor_Method", "CheckImp.Methods", mCheckEngine.DbDataConnection );
            AddSourceDataTable_Data( "PROD_Monitor_Plan_Location", "VW_MP_MONITOR_LOCATION", sORISCodeFilter );
            AddSourceDataTable_Data( "PROD_Monitor_Plan", "VW_MP_MONITOR_PLAN", sORISCodeFilter );
            AddSourceDataTable_Data( "PROD_Reporting_Period_Lookup", "VW_REPORTING_PERIOD" );
            AddSourceDataTable_Data( "PROD_Fuel_Code_Lookup", "FUEL_CODE" );
        }

        /// <summary>
        /// This method initializes the class containing static properties enabling strongly typed access to the parameters used by the process.
        /// </summary>
        protected override void InitStaticParameterClass()
        {
          LmeImportParameters.Init(this);
        }

        /// <summary>
        /// Allows the setting of the current category for which parameters will be set.
        /// </summary>
        /// <param name="category"></param>
        public override void SetStaticParameterCategory(cCategory category)
        {
          LmeImportParameters.Category = category;
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
            AddSourceDataTable( sDataTableNm, sViewNm, null, mCheckEngine.DbWsConnection );
        }

        /// <summary>
        /// Add a DataTable to mSourceData from ECMPS database
        /// </summary>
        /// <param name="sDataTableNm">The name of the new DataTable</param>
        /// <param name="sViewNm">The table/view to get data from</param>
        private void AddSourceDataTable_Data( string sDataTableNm, string sViewNm )
        {
            AddSourceDataTable( sDataTableNm, sViewNm, null, mCheckEngine.DbDataConnection );
        }

        /// <summary>
        /// Add a DataTable to mSourceData from Workspace database
        /// </summary>
        /// <param name="sDataTableNm">The name of the new DataTable</param>
        /// <param name="sViewNm">The table/view to get data from</param>
        /// <param name="sWhereClause">A where clause, if any</param>
        private void AddSourceDataTable_WS( string sDataTableNm, string sViewNm, string sWhereClause )
        {
            AddSourceDataTable( sDataTableNm, sViewNm, sWhereClause, mCheckEngine.DbWsConnection );
        }

        /// <summary>
        /// Add a DataTable to mSourceData from ECMPS database
        /// </summary>
        /// <param name="sDataTableNm">The name of the new DataTable</param>
        /// <param name="sViewNm">The table/view to get data from</param>
        /// <param name="sWhereClause">A where clause, if any</param>
        private void AddSourceDataTable_Data( string sDataTableNm, string sViewNm, string sWhereClause )
        {
            AddSourceDataTable( sDataTableNm, sViewNm, sWhereClause, mCheckEngine.DbDataConnection );
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
        /// <param name="dbConn">The database connection to user</param>
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
            string sSQL = string.Format( "SET NOCOUNT ON; DELETE FROM {0} WHERE {1}='{2}'; SELECT @@ROWCOUNT;", sTableNm, sKeyNm, sKeyValue );
            int nRows = (int)mCheckEngine.DbWsConnection.ExecuteScalar( sSQL );
        }

        #endregion

    }
}
