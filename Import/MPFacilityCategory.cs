using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;

namespace ECMPS.Checks.MonitorPlanImport
{
    class cMPFacilityCategory : cCategory
    {
        cMonitorPlanImportProcess _MPProcess = null;
        DataRowView _currentRecord = null;

        #region Constructors

        public cMPFacilityCategory( cCheckEngine CheckEngine, cMonitorPlanImportProcess mpProcess )
            : base( CheckEngine, (cProcess)mpProcess, "MPFAC" )
        {
            _MPProcess = mpProcess;

            _currentRecord = new DataView( _MPProcess.SourceData.Tables["WS_MonitorPlan"], null, null, DataViewRowState.CurrentRows )[0];
            TableName = "MP_MonitoringPlan";
            CurrentRowId = _currentRecord["ORIS_CODE"].ToString();

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
            SetCheckParameter( "Current_Workspace_MonitoringPlan", _currentRecord, eParameterDataType.DataRowView );

            DataView view = new DataView( _MPProcess.SourceData.Tables["WS_Unit"], "", "", DataViewRowState.CurrentRows );
            SetCheckParameter( "Workspace_Unit_Records", view, eParameterDataType.DataView );

            view = new DataView( _MPProcess.SourceData.Tables["PROD_Facility"], "", "", DataViewRowState.CurrentRows );
            SetCheckParameter( "Production_Facility_Records", view, eParameterDataType.DataView );
        }

        protected override bool SetErrorSuppressValues()
        {
          ErrorSuppressValues = null;
          return true;
        }

        protected override void SetRecordIdentifier()
        {
            RecordIdentifier = "ORISCode " + CurrentRowId;
        }

        #endregion
    }
}
