using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;

namespace ECMPS.Checks.QAImport
{
    class cQALocationCategory : cCategory
    {
        cQAImportProcess _QAProcess = null;

        #region Constructors

        public cQALocationCategory( cCheckEngine CheckEngine, cQAImportProcess qaProcess )
            : base( CheckEngine, (cProcess)qaProcess, "QALOC" )
        {
            _QAProcess = qaProcess;
            TableName = "VW_IMPORT_QA_Locations";

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
            DataView view = new DataView( _QAProcess.SourceData.Tables["PROD_Monitor_Location"], null, null, DataViewRowState.CurrentRows );
            SetCheckParameter( "Location_Records", view, eParameterDataType.DataView );

            view = new DataView( _QAProcess.SourceData.Tables["WS_QALocations"], null, null, DataViewRowState.CurrentRows );
            SetCheckParameter( "Workspace_Location_Records", view, eParameterDataType.DataView );

            view = new DataView( _QAProcess.SourceData.Tables["PROD_Monitor_System"], null, null, DataViewRowState.CurrentRows );
            SetCheckParameter( "Monitor_System_Records", view, eParameterDataType.DataView );

            view = new DataView( _QAProcess.SourceData.Tables["PROD_Component"], null, null, DataViewRowState.CurrentRows );
            SetCheckParameter( "Component_Records", view, eParameterDataType.DataView );

            // I do not believe this parameter is used in this category. (djw2)
            //view = new DataView( _QAProcess.SourceData.Tables["PROD_QA_Cert_Event"], null, null, DataViewRowState.CurrentRows );
            //SetCheckParameter( "Qa_Certification_Event_Records", view, eParameterDataType.DataView );

            // I do not believe this parameter is used in this category. (djw2)
            //view = new DataView( _QAProcess.SourceData.Tables["PROD_Test_ExtExemption"], null, null, DataViewRowState.CurrentRows );
            //SetCheckParameter( "Test_Extension_Exemption_Records", view, eParameterDataType.DataView );

            view = new DataView( _QAProcess.SourceData.Tables["WS_QASystems"], null, null, DataViewRowState.CurrentRows );
            SetCheckParameter( "Workspace_System_Records", view, eParameterDataType.DataView );

            view = new DataView( _QAProcess.SourceData.Tables["WS_QAComponents"], null, null, DataViewRowState.CurrentRows );
            SetCheckParameter( "Workspace_Component_Records", view, eParameterDataType.DataView );
        }

        protected override bool SetErrorSuppressValues()
        {
          ErrorSuppressValues = null;
          return true;
        }

        protected override void SetRecordIdentifier()
        {
            RecordIdentifier = "import file";
        }

        #endregion    
    }
}
