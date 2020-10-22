using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;

namespace ECMPS.Checks.QAImport
{
    class cQAIntCategory : cCategory
    {
        cQAImportProcess _QAProcess = null;
        DataRowView _currentRecord = null;

        #region Constructors

        public cQAIntCategory( cCheckEngine CheckEngine, cQAImportProcess qaProcess )
            : base( CheckEngine, (cProcess)qaProcess, "QAINT" )
        {
            _QAProcess = qaProcess;

            _currentRecord = new DataView( _QAProcess.SourceData.Tables["WS_QACert"], null, null, DataViewRowState.CurrentRows )[0];

            TableName = "QA_QualityAssuranceAndCert";
            CurrentRowId = _currentRecord["QA_PK"].ToString();

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
            SetCheckParameter( "Current_Workspace_QualityAssuranceAndCert_File", _currentRecord, eParameterDataType.DataRowView );

            DataView view = new DataView( _QAProcess.SourceData.Tables["PROD_Facility"], null, null, DataViewRowState.CurrentRows );
            SetCheckParameter( "Production_Facility_Records", view, eParameterDataType.DataView );
        }

        protected override bool SetErrorSuppressValues()
        {
          ErrorSuppressValues = null;
          return true;
        }

        protected override void SetRecordIdentifier()
        {
            RecordIdentifier = string.Format( "ORIS Code {0}", _currentRecord["ORIS_CODE"].ToString() );
        }

        #endregion    
    }
}
