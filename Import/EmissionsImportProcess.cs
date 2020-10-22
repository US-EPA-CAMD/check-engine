using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.CheckEngine.Definitions;
using ECMPS.Checks.DatabaseAccess;
using ECMPS.Checks.EmImport.Parameters;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Common;
using ECMPS.Definitions.SeverityCode;

namespace ECMPS.Checks.HourlyEmissionsImport
{
    class cEmissionsImportProcess : cProcess
    {
        private static decimal _nSessionID = 0;

        #region Constructors

        public cEmissionsImportProcess( cCheckEngine CheckEngine, decimal nSessionID )
            : base( CheckEngine, SaveSessionID( nSessionID, "EMIMPRT" ) )
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

            // Facility check
            cCheckParameterBands ImportChecks = GetCheckBands( "EMINT" );
            cEmissionsIntCategory intCategory = new cEmissionsIntCategory( mCheckEngine, this );

            RunResult = intCategory.ProcessChecks( ImportChecks );
            intCategory.EraseParameters();

            // process the next category only if we don't have a "FATAL" result 
            if( this.SeverityCd != eSeverityCd.FATAL )
            {   // Tests
                cEmissionsTestCategory testCategory = null;
                ImportChecks = GetCheckBands( "EMTEST" );
                foreach( DataRow drWSTest in mSourceData.Tables["WS_DailyTestSummary"].Rows )
                {
                    string sDTSD_PK = drWSTest["DTSD_PK"].ToString();
                    testCategory = new cEmissionsTestCategory( mCheckEngine, this, sDTSD_PK );
                    RunResult = testCategory.ProcessChecks( ImportChecks );

                    if (testCategory.SeverityCd == eSeverityCd.CRIT1)
                    {   // delete this record from the workspace
                        DeleteRecord( "EM_DailyTestSummary", "DTSD_PK", sDTSD_PK );
                    }

                    testCategory.EraseParameters();
                }
            }

			// process the next category only if we don't have a "FATAL" result 
			if (this.SeverityCd != eSeverityCd.FATAL)
			{   // Tests
				cEmissionsWeeklyTestCategory WeeklyTestCategory = null;
				ImportChecks = GetCheckBands("EMWKTST");
				foreach (DataRow drWSTest in mSourceData.Tables["WS_WeeklyTestSummary"].Rows)
				{
					string sWTSD_PK = drWSTest["WTSD_PK"].ToString();
					WeeklyTestCategory = new cEmissionsWeeklyTestCategory(mCheckEngine, this, sWTSD_PK);
					RunResult = WeeklyTestCategory.ProcessChecks(ImportChecks);

					if (WeeklyTestCategory.SeverityCd == eSeverityCd.CRIT1)
					{   // delete this record from the workspace
						DeleteRecord("EM_WeeklyTestSummary", "WTSD_PK", sWTSD_PK);
					}

					WeeklyTestCategory.EraseParameters();
				}
			}

            // process the next category only if we don't have a "FATAL" result 
            if( this.SeverityCd != eSeverityCd.FATAL )
            {   // Locations
                cEmissionsLocationCategory locCategory = null;
                ImportChecks = GetCheckBands( "EMLOC" );
                locCategory = new cEmissionsLocationCategory( mCheckEngine, this );
                RunResult = locCategory.ProcessChecks( ImportChecks );

                locCategory.EraseParameters();
            }

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
            AddSourceDataTable_WS( "WS_Emissions", "EM_Emissions" );
            AddSourceDataTable_WS( "WS_EMLocations", "VW_CHECK_EM_Locations" );
            AddSourceDataTable_WS( "WS_EMSystems", "VW_CHECK_EM_Systems" );
            AddSourceDataTable_WS( "WS_EMComponents", "VW_CHECK_EM_Components" );
            AddSourceDataTable_WS( "WS_EMFormulas", "VW_CHECK_EM_Formulas" );
            AddSourceDataTable_WS( "WS_EMDates", "VW_CHECK_EM_Dates" );
            AddSourceDataTable_WS( "WS_Unit", "VW_CHECK_EM_Locations", "WHERE MON_LOC_TYPE='UNIT'" );
            AddSourceDataTable_WS( "WS_DailyTestSummary", "VW_CHECK_EM_DailyTestSummary" );
            AddSourceDataTable_WS( "WS_DailyCalibration", "EM_DailyCalibration" );
            AddSourceDataTable_WS( "WS_LongTermFuelFlows", "VW_CHECK_EM_LongTermFuelFlow" );
			AddSourceDataTable_WS("WS_WeeklyTestSummary", "VW_CHECK_EM_WeeklyTestSummary");
			AddSourceDataTable_WS("WS_WeeklySystemIntegrity", "EM_WeeklySystemIntegrity");

            // get the data from the production tables
            AddSourceDataTable_Data( "PROD_Facility", "FACILITY", "WHERE ORIS_CODE=" + mCheckEngine.ORISCode );
            AddSourceDataFunction( "PROD_Unit", "CheckImp.Units", mCheckEngine.DbDataConnection );
            AddSourceDataFunction( "PROD_Monitor_System", "CheckImp.Systems", mCheckEngine.DbDataConnection );
            AddSourceDataFunction( "PROD_Monitor_Location", "CheckImp.Locations", mCheckEngine.DbDataConnection );
            AddSourceDataFunction( "PROD_Monitor_Formula", "CheckImp.Formulas", mCheckEngine.DbDataConnection );
            AddSourceDataFunction( "PROD_Component", "CheckImp.Components",  mCheckEngine.DbDataConnection );
            AddSourceDataFunction( "PROD_Monitor_Plan_Location", "CheckImp.MPLocations", mCheckEngine.DbDataConnection );
        }

        /// <summary>
        /// This method initializes the class containing static properties enabling strongly typed access to the parameters used by the process.
        /// </summary>
        protected override void InitStaticParameterClass()
        {
          EmImportParameters.Init(this);
        }

        /// <summary>
        /// Allows the setting of the current category for which parameters will be set.
        /// </summary>
        /// <param name="category"></param>
        public override void SetStaticParameterCategory(cCategory category)
        {
          EmImportParameters.Category = category;
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
