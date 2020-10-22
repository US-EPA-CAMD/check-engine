using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;

namespace ECMPS.Checks.MonitorPlanImport
{
    class cMPAnalyzerRangeCategory : cCategory
    {
        cMonitorPlanImportProcess _MPProcess = null;
        string _PrimaryKey = null;

        #region Constructors

        public cMPAnalyzerRangeCategory( cCheckEngine CheckEngine, cMonitorPlanImportProcess mpProcess, string AR_PK )
            : base( CheckEngine, (cProcess)mpProcess, "MPANAL" )
        {
            _MPProcess = mpProcess;
            _PrimaryKey = "AR_PK";

            TableName = "MP_AnalyzerRange";
            CurrentRowId = AR_PK;

            FilterData();
            SetRecordIdentifier();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Sets the checkbands for this category to the passed check bands and then executes
        /// those checks.
        /// </summary>
        /// <param name="ACheckBands">The check bands to process.</param>
        /// <returns>True if the processing of check executed normally.</returns>
        public bool ProcessChecks(cCheckParameterBands ACheckBands)
        {
          this.SetCheckBands(ACheckBands);
          return base.ProcessChecks();
        }

        #endregion

        #region Base Class Overrides

        protected override void FilterData()
        {
            string sFilter = string.Format( "{0}={1}", _PrimaryKey, CurrentRowId );

            DataRowView currentRecord = new DataView( _MPProcess.SourceData.Tables["WS_AnalyzerRange"], sFilter, "", DataViewRowState.CurrentRows )[0];
            SetCheckParameter( "Current_Workspace_Analyzer_Range", currentRecord, eParameterDataType.DataRowView );

            DataView view = new DataView( _MPProcess.SourceData.Tables["Prod_Component"], "", "", DataViewRowState.CurrentRows );
            SetCheckParameter( "Component_Records", view, eParameterDataType.DataView );
        }

        protected override bool SetErrorSuppressValues()
        {
          ErrorSuppressValues = null;
          return true;
        }

        protected override void SetRecordIdentifier()
        {
            DataRowView CurrentRecord = (DataRowView)GetCheckParameter( "Current_Workspace_Analyzer_Range" ).ParameterValue;
            RecordIdentifier = string.Format( "Component ID {0} - Analyzer Range Code {1}", CurrentRecord["COMPONENT_IDENTIFIER"].ToString(), CurrentRecord["ANALYZER_RANGE_CD"].ToString() );
        }

        #endregion    
    }
}
