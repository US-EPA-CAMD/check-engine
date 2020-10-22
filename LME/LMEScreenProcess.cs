using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Text;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.CheckEngine.Definitions;
using ECMPS.Checks.Em.Parameters;
using ECMPS.Checks.EmissionsChecks;
using ECMPS.Checks.EmissionsReport;
using ECMPS.Checks.DatabaseAccess;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

namespace ECMPS.Checks.LME
{
    public class cLMEScreenProcess : cProcess
    {
        #region Constructors
        string mCategoryCd;

        public cLMEScreenProcess( cCheckEngine CheckEngine, string CategoryCd )
            : base( CheckEngine, "LMESCRN" )
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
        public override bool CheckProcedureInit( string checksDllPath, ref string errorMessage )
        {
            bool result;

            try
            {
                cEmissionsCheckParameters emissionsCheckParameters = new cEmissionsCheckParameters( this, mCheckEngine.DbAuxConnection );

                Checks[40] = new cHourlyGeneralChecks( mCheckEngine, emissionsCheckParameters );

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

            // Initialize category object; checks object
            // run checks for each record in each category
            // update database (logs and calculate values)

            //ElapsedTimes.TimingBegin("ExecuteChecks", this);

            string ErrorMessage = "";

            int RptPeriodId = (mCheckEngine.RptPeriodId.HasValue && (mCheckEngine.RptPeriodId.Value > 0) ? mCheckEngine.RptPeriodId.Value : int.MinValue);
            SetCheckParameter( "Current_Reporting_Period", RptPeriodId, eParameterDataType.Integer );
            SetCheckParameter( "Fuel_Type_Reality_Checks_for_GCV_Cross_Check_Table", mSourceData.Tables["CrossCheck_FuelTypeRealityChecksforGCV"].DefaultView, eParameterDataType.DataView );
            SetCheckParameter( "Fuel_Type_Warning_Levels_for_GCV_Cross_Check_Table", mSourceData.Tables["CrossCheck_FuelTypeWarningLevelsforGCV"].DefaultView, eParameterDataType.DataView );

            switch( mCategoryCd )
            {
                case "LMEHRLY":
                    {
                        cHourlyOpDataRowCategory HourlyOpDataRow;
                        HourlyOpDataRow = new cHourlyOpDataRowCategory( mCheckEngine, this, mCheckEngine.ThisTable );
                        HourlyOpDataRow.InitCheckBands( CheckEngine.DbAuxConnection, ref ErrorMessage );
                        RunResult = HourlyOpDataRow.ProcessChecks( mCheckEngine.MonLocId );
                        HourlyOpDataRow.EraseParameters();
                        break;
                    }

                case "LMELTFF":
                    {
                        cLTFFDataRowCategory LTFFDataRow;
                        LTFFDataRow = new cLTFFDataRowCategory( mCheckEngine, this, mCheckEngine.ThisTable );
                        LTFFDataRow.InitCheckBands( CheckEngine.DbAuxConnection, ref ErrorMessage );
                        RunResult = LTFFDataRow.ProcessChecks( mCheckEngine.MonLocId );
                        LTFFDataRow.EraseParameters();
                        break;
                    }

                case "EMCOMM":
                    {
                        cEmissionCommentCategory EmissionCommentCategory;
                        EmissionCommentCategory = new cEmissionCommentCategory( mCheckEngine, this, mCheckEngine.ThisTable );
                        EmissionCommentCategory.InitCheckBands( CheckEngine.DbAuxConnection, ref ErrorMessage );
                        RunResult = EmissionCommentCategory.ProcessChecks( mCheckEngine.MonLocId );
                        EmissionCommentCategory.EraseParameters();
                        break;
                    }
            }

            DbUpdate( ref Result );

            return Result;
        }

        protected override void InitCalculatedData()
        {
        }

        protected override void InitSourceData()
        {
            // Initialize all data tables in process

            mSourceData = new DataSet();
            mFacilityID = GetFacilityID();


            string sMonLocWhereClause = string.Format( " WHERE MON_LOC_ID = '{0}'", mCheckEngine.MonLocId );
            string sMonPlanWhereClause = string.Format( " WHERE MON_PLAN_ID = '{0}'", mCheckEngine.MonPlanId );

            switch( mCheckEngine.CategoryCd )
            {
                case "LMEHRLY":
                    {
                        //add hourly op records
                        AddSourceDataTable( "LMEHourlyOp", "VW_MP_HRLY_OP_DATA", sMonPlanWhereClause );
                        AddSourceDataTable( "MonitorLoad", "VW_MONITOR_LOAD", sMonLocWhereClause );
                        break;
                    }

                case "LMELTFF":
                    {
                        AddSourceDataTable( "MonitorSystem", "VW_MONITOR_SYSTEM", sMonLocWhereClause );
                        break;
                    }

                case "EMCOMM":
                    {
                        AddSourceDataTable( "HourlySubmissionComment", "VW_MP_HRLY_SUBMISSION_COMMENT", sMonPlanWhereClause );
                        break;
                    }
            }
            AddSourceDataTable( "ReportingPeriod", "vw_reporting_period" );
            LoadCrossCheck( "Fuel Type Warning Levels for GCV" );
            LoadCrossCheck( "Fuel Type Reality Checks for GCV" );
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

        #region Private Methods

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
