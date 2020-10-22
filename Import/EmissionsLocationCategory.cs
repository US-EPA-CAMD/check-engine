using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;

namespace ECMPS.Checks.HourlyEmissionsImport
{
    class cEmissionsLocationCategory : cCategory
    {
        cEmissionsImportProcess _EMProcess = null;

        #region Constructors

        public cEmissionsLocationCategory( cCheckEngine CheckEngine, cEmissionsImportProcess emProcess )
            : base( CheckEngine, (cProcess)emProcess, "EMLOC" )
        {
            _EMProcess = emProcess;

            TableName = "VW_IMPORT_EM_Locations";

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
        public bool ProcessChecks( cCheckParameterBands ACheckBands )
        {
            this.SetCheckBands( ACheckBands );
            return base.ProcessChecks();
        }

        #endregion

        #region Base Class Overrides

        protected override void FilterData()
        {
            DataView view;

            // I do not believe that this parameter is used in this category. (djw2)
            //view = new DataView(_EMProcess.SourceData.Tables["PROD_Monitor_Location"], null, null, DataViewRowState.CurrentRows);
            //SetCheckParameter("Location_Records", view, eParameterDataType.DataView);

            view = new DataView( _EMProcess.SourceData.Tables["WS_EMLocations"], null, "MON_LOC_TYPE,UNIT_OR_STACK", DataViewRowState.CurrentRows );
            SetCheckParameter( "Workspace_Location_Records", view, eParameterDataType.DataView );

            view = new DataView( _EMProcess.SourceData.Tables["PROD_Monitor_System"], null, null, DataViewRowState.CurrentRows );
            SetCheckParameter( "Monitor_System_Records", view, eParameterDataType.DataView );

            view = new DataView( _EMProcess.SourceData.Tables["PROD_Component"], null, null, DataViewRowState.CurrentRows );
            SetCheckParameter( "Component_Records", view, eParameterDataType.DataView );

            view = new DataView( _EMProcess.SourceData.Tables["PROD_Monitor_Formula"], null, null, DataViewRowState.CurrentRows );
            SetCheckParameter( "Formula_Records", view, eParameterDataType.DataView );

            view = new DataView( _EMProcess.SourceData.Tables["WS_EMSystems"], null, null, DataViewRowState.CurrentRows );
            SetCheckParameter( "Workspace_System_Records", view, eParameterDataType.DataView );

            view = new DataView( _EMProcess.SourceData.Tables["WS_EMComponents"], null, null, DataViewRowState.CurrentRows );
            SetCheckParameter( "Workspace_Component_Records", view, eParameterDataType.DataView );

            view = new DataView( _EMProcess.SourceData.Tables["WS_EMFormulas"], null, null, DataViewRowState.CurrentRows );
            SetCheckParameter( "Workspace_Formula_Records", view, eParameterDataType.DataView );

            view = new DataView( _EMProcess.SourceData.Tables["WS_LongTermFuelFlows"], null, null, DataViewRowState.CurrentRows );
            SetCheckParameter( "Workspace_Long_Term_Fuel_Flow_Records", view, eParameterDataType.DataView );
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
