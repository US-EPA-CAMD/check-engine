using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;

namespace ECMPS.Checks.QAImport
{
    class cQARataCategory : cCategory
    {
        cQAImportProcess _QAProcess = null;
        string _PrimaryKey = null;

        #region Constructors

        public cQARataCategory( cCheckEngine CheckEngine, cQAImportProcess qaProcess, string RS_PK )
            : base( CheckEngine, (cProcess)qaProcess, "QARATA" )
        {
            _QAProcess = qaProcess;
            _PrimaryKey = "RS_PK";

            TableName = "QA_RATASummary";
            CurrentRowId = RS_PK;

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

            DataRowView currentRecord = new DataView( _QAProcess.SourceData.Tables["WS_RataSummary"], sFilter, null, DataViewRowState.CurrentRows )[0];
            SetCheckParameter( "Current_Workspace_RATA_Summary", currentRecord, eParameterDataType.DataRowView );

            // filter our row out of this result set
            DataView view = new DataView( _QAProcess.SourceData.Tables["PROD_Monitor_System"], null, null, DataViewRowState.CurrentRows );
            SetCheckParameter( "Monitor_System_Records", view, eParameterDataType.DataView );
        }

        protected override bool SetErrorSuppressValues()
        {
          ErrorSuppressValues = null;
          return true;
        }

        protected override void SetRecordIdentifier()
        {
            DataRowView CurrentRecord = (DataRowView)GetCheckParameter( "Current_Workspace_RATA_Summary" ).ParameterValue;
            RecordIdentifier = string.Format( "Test Number {0}", CurrentRecord["TEST_NUM"].ToString() );
        }

        #endregion    
    }
}
