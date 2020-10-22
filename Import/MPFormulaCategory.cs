using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;

namespace ECMPS.Checks.MonitorPlanImport
{
    class cMPFormulaCategory : cCategory
    {
        cMonitorPlanImportProcess _MPProcess = null;
        string _PrimaryKey = null;

        #region Constructors

        public cMPFormulaCategory( cCheckEngine CheckEngine, cMonitorPlanImportProcess mpProcess, string MF_PK )
            : base( CheckEngine, (cProcess)mpProcess, "MPFORM" )
        {
            _MPProcess = mpProcess;
            _PrimaryKey = "MF_PK";

            TableName = "MP_MonitoringFormula";
            CurrentRowId = MF_PK;

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

            DataRowView currentRecord = new DataView( _MPProcess.SourceData.Tables["WS_MonitoringFormula"], sFilter, "", DataViewRowState.CurrentRows )[0];
            SetCheckParameter( "Current_Workspace_Formula", currentRecord, eParameterDataType.DataRowView );

            DataView view = new DataView( _MPProcess.SourceData.Tables["PROD_Monitor_Formula"], "", "", DataViewRowState.CurrentRows );
            SetCheckParameter( "Formula_Records", view, eParameterDataType.DataView );
        }

        protected override bool SetErrorSuppressValues()
        {
          ErrorSuppressValues = null;
          return true;
        }

        protected override void SetRecordIdentifier()
        {
            DataRowView CurrentRecord = (DataRowView)GetCheckParameter( "Current_Workspace_Formula" ).ParameterValue;
            RecordIdentifier = string.Format( "Formula ID {0} - Parameter Code {1}", CurrentRecord["FORMULA_IDENTIFIER"].ToString(), CurrentRecord["PARAMETER_CD"].ToString() );
        }

        #endregion    
    }
}
