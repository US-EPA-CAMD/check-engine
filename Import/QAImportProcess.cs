using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.CheckEngine.Definitions;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.QaImport.Parameters;
using ECMPS.Checks.TypeUtilities;
using ECMPS.Checks.DatabaseAccess;

using ECMPS.Common;
using ECMPS.Definitions.SeverityCode;


namespace ECMPS.Checks.QAImport
{
    public class cQAImportProcess : cProcess
    {
        private static decimal _nSessionID = 0;

        #region Constructors

        public cQAImportProcess( cCheckEngine CheckEngine, decimal SessionID )
            : base( CheckEngine, SaveSessionID( SessionID, "QAIMPRT" ) )
        {
            _nSessionID = SessionID;
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
            string Result = "";
            bool RunResult = false;

            // Facility check
            cCheckParameterBands ImportChecks = GetCheckBands( "QAINT" );
            cQAIntCategory intCategory = new cQAIntCategory( mCheckEngine, this );

            RunResult = intCategory.ProcessChecks( ImportChecks );

            // process the next category only if we don't have a "FATAL" result 
            if( this.SeverityCd != eSeverityCd.FATAL )
            {   // Locations
                cQALocationCategory locCategory = null;
                ImportChecks = GetCheckBands( "QALOC" );
                locCategory = new cQALocationCategory( mCheckEngine, this );
                RunResult = locCategory.ProcessChecks( ImportChecks );

                locCategory.EraseParameters();
            }

            // process the next category only if we don't have a "FATAL" result 
            if( this.SeverityCd != eSeverityCd.FATAL )
            {   // Tests
                cQATestCategory testCategory = null;
                ImportChecks = GetCheckBands( "QATEST" );
                foreach( DataRow drWSTest in mSourceData.Tables["WS_TestSummary"].Rows )
                {
                    string sTSPK = drWSTest["TS_PK"].ToString();
                    testCategory = new cQATestCategory( mCheckEngine, this, sTSPK );
                    RunResult = testCategory.ProcessChecks( ImportChecks );

                    if( testCategory.SeverityCd == eSeverityCd.CRIT1 )
                    {   // delete this record from the workspace
                        DeleteRecord( "QA_TestSummary", "TS_PK", sTSPK );
                    }

                    testCategory.EraseParameters();
                }
            }

            // process the next category only if we don't have a "FATAL" result 
            if( this.SeverityCd != eSeverityCd.FATAL )
            {   // RATA
                cQARataCategory rataCategory = null;
                ImportChecks = GetCheckBands( "QARATA" );
                foreach( DataRow drWSRATAs in mSourceData.Tables["WS_RataSummary"].Rows )
                {
                    string sRS_PK = drWSRATAs["RS_PK"].ToString();
                    rataCategory = new cQARataCategory( mCheckEngine, this, sRS_PK );
                    RunResult = rataCategory.ProcessChecks( ImportChecks );

                    if( rataCategory.SeverityCd == eSeverityCd.CRIT1 )
                    {   // delete this record from the workspace
                        DeleteRecord( "QA_RataSummary", "RS_PK", sRS_PK );
                    }

                    rataCategory.EraseParameters();
                }
            }

            intCategory.EraseParameters();

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
            AddSourceDataTable_WS( "WS_QACert", "QA_QualityAssuranceAndCert" );
            AddSourceDataTable_WS( "WS_QALocations", "VW_CHECK_QA_Locations" );
            AddSourceDataTable_WS( "WS_QASystems", "VW_CHECK_QA_Systems" );
            AddSourceDataTable_WS( "WS_QAComponents", "VW_CHECK_QA_Components" );
            AddSourceDataTable_WS( "WS_TestSummary", "VW_CHECK_QA_TestSummary" );
            AddSourceDataTable_WS( "WS_Rata", "QA_RATA" );
            AddSourceDataTable_WS( "WS_RataSummary", "VW_CHECK_QA_RATASummary" );
            AddSourceDataTable_WS( "WS_FlowRATARun", "VW_CHECK_QA_FlowRATARun" );
            AddSourceDataTable_WS( "WS_AECorrTestSummary", "QA_AECorrTestSummary" );
            AddSourceDataTable_WS( "WS_CalibrationInjection", "QA_CalibrationInjection" );
            AddSourceDataTable_WS( "WS_CycleTimeSummary", "QA_CycleTimeSummary" );
            AddSourceDataTable_WS( "WS_FlowToLoadCheck", "QA_FlowToLoadCheck" );
            AddSourceDataTable_WS( "WS_FlowToLoadReference", "QA_FlowToLoadReference" );
            AddSourceDataTable_WS( "WS_FuelFlowmeterAccuracy", "QA_FuelFlowmeterAccuracy" );
            AddSourceDataTable_WS( "WS_FuelFlowToLoadBaseline", "QA_FuelFlowToLoadBaseline" );
            AddSourceDataTable_WS( "WS_FuelFlowToLoadTest", "QA_FuelFlowToLoadTest" );
			AddSourceDataTable_WS("WS_LinearitySummary", "QA_LinearitySummary");
			AddSourceDataTable_WS("WS_HgTestSummary", "QA_HGTestSummary");
			AddSourceDataTable_WS("WS_HgSummary", "VW_QA_HgTestSummaryData");
            AddSourceDataTable_WS( "WS_OnOffCalibration", "QA_OnOffCalibration" );
            AddSourceDataTable_WS( "WS_TestQualification", "QA_TestQualification" );
            AddSourceDataTable_WS( "WS_TransAccuracy", "QA_TransAccuracy" );
            AddSourceDataTable_WS( "WS_UnitDefaultTest", "QA_UnitDefaultTest" );
            AddSourceDataTable_WS( "WS_AE_HI_OIL", "VW_CHECK_QA_AE_HI_OIL" );
            AddSourceDataTable_WS( "WS_AE_HI_GAS", "VW_CHECK_QA_AE_HI_GAS" );
            AddSourceDataTable_WS( "WS_ProtocolGas", "VW_CHECK_QA_ProtocolGas" );
            AddSourceDataTable_WS( "WS_AirEmissionTesting", "VW_CHECK_QA_AirEmissionTesting" );

            AddSourceDataTable_Data( "PROD_Facility", "FACILITY", "WHERE ORIS_CODE=" + mCheckEngine.ORISCode );
            AddSourceDataFunction( "PROD_Unit", "CheckImp.Units", mCheckEngine.DbDataConnection );
            AddSourceDataFunction( "PROD_Monitor_System", "CheckImp.Systems", mCheckEngine.DbDataConnection );
            AddSourceDataFunction( "PROD_Monitor_Method", "CheckImp.Methods", mCheckEngine.DbDataConnection );
            AddSourceDataFunction( "PROD_Monitor_Location", "CheckImp.Locations", mCheckEngine.DbDataConnection );
            AddSourceDataFunction( "PROD_Component", "CheckImp.Components", mCheckEngine.DbDataConnection );
            AddSourceDataFunction( "PROD_Test_Summary", "CheckImp.Tests", mCheckEngine.DbDataConnection );
            AddSourceDataFunction( "PROD_QA_Supp_Data", "CheckImp.QASuppData", mCheckEngine.DbDataConnection );
            AddSourceDataFunction( "PROD_QA_Cert_Event", "CheckImp.QAEvents", mCheckEngine.DbDataConnection );
            AddSourceDataFunction( "PROD_Test_ExtExemption", "CheckImp.TEEs", mCheckEngine.DbDataConnection );
        }

        /// <summary>
        /// This method initializes the class containing static properties enabling strongly typed access to the parameters used by the process.
        /// </summary>
        protected override void InitStaticParameterClass()
        {
          QaImportParameters.Init(this);
        }

        /// <summary>
        /// Allows the setting of the current category for which parameters will be set.
        /// </summary>
        /// <param name="category"></param>
        public override void SetStaticParameterCategory(cCategory category)
        {
          QaImportParameters.Category = category;
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
            string sAnd = string.Format( " and SUBMISSION_ID={0}", _nSessionID );
            AddSourceDataTable( sDataTableNm, sViewNm, sWhereClause + sAnd, mCheckEngine.DbWsConnection );
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
            string sSQL = string.Format( "SET NOCOUNT ON; DELETE FROM {0} WHERE {1}='{2}' AND SUBMISSION_ID={3}; SELECT @@ROWCOUNT;", sTableNm, sKeyNm, sKeyValue, _nSessionID );
            int nRows = (int)mCheckEngine.DbWsConnection.ExecuteScalar( sSQL );
        }

        #endregion
    }
}
