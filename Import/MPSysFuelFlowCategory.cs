using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;

namespace ECMPS.Checks.MonitorPlanImport
{
    class cMPSysFuelFlowCategory : cCategory
    {
        cMonitorPlanImportProcess _MPProcess = null;
        string _PrimaryKey = null;

        #region Constructors

        public cMPSysFuelFlowCategory( cCheckEngine CheckEngine, cMonitorPlanImportProcess mpProcess, string MSFF_PK )
            : base( CheckEngine, (cProcess)mpProcess, "MPSYSFF" )
        {
            _MPProcess = mpProcess;
            _PrimaryKey = "MSFF_PK";

            TableName = "MP_MonitoringSysFuelFlow";
            CurrentRowId = MSFF_PK;

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

            DataRowView currentRecord = new DataView( _MPProcess.SourceData.Tables["WS_MonitoringSysFuelFlow"], sFilter, "", DataViewRowState.CurrentRows )[0];
            SetCheckParameter( "Current_Workspace_System_FuelFlow", currentRecord, eParameterDataType.DataRowView );

            DataView view = new DataView( _MPProcess.SourceData.Tables["PROD_Monitor_System"], "", "", DataViewRowState.CurrentRows );
            SetCheckParameter( "Monitor_System_Records", view, eParameterDataType.DataView );
        }

        protected override bool SetErrorSuppressValues()
        {
          ErrorSuppressValues = null;
          return true;
        }

        protected override void SetRecordIdentifier()
        {
            DataRowView CurrentRecord = (DataRowView)GetCheckParameter( "Current_Workspace_System_FuelFlow" ).ParameterValue;
            RecordIdentifier = string.Format( "System ID {0}", CurrentRecord["SYSTEM_IDENTIFIER"].ToString() );
        }

        #endregion    
    }
}
