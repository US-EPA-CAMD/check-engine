using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;

namespace ECMPS.Checks.MonitorPlanImport
{
    class cMPQualLMECategory : cCategory
    {
        cMonitorPlanImportProcess _MPProcess = null;
        string _PrimaryKey = null;

        #region Constructors

        public cMPQualLMECategory( cCheckEngine CheckEngine, cMonitorPlanImportProcess mpProcess, string MQLME_PK )
            : base( CheckEngine, (cProcess)mpProcess, "MPQLME" )
        {
            _MPProcess = mpProcess;
            _PrimaryKey = "MQLME_PK";

            TableName = "MP_MonitoringQualLME";
            CurrentRowId = MQLME_PK;

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

            DataRowView currentRecord = new DataView( _MPProcess.SourceData.Tables["WS_MonitoringQualLME"], sFilter, "", DataViewRowState.CurrentRows )[0];
            SetCheckParameter( "Current_Workspace_Qualification_LME", currentRecord, eParameterDataType.DataRowView );
        }

        protected override bool SetErrorSuppressValues()
        {
          ErrorSuppressValues = null;
          return true;
        }

        protected override void SetRecordIdentifier()
        {
            DataRowView CurrentRecord = (DataRowView)GetCheckParameter( "Current_Workspace_Qualification_LME" ).ParameterValue;
            RecordIdentifier = string.Format( "Qualification Type Code {0} - Qualification Data Year {1}", CurrentRecord["QUAL_TYPE_CD"].ToString(), CurrentRecord["QUAL_DATA_YEAR"].ToString() );
        }

        #endregion    
    }
}
