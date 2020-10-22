using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;

namespace ECMPS.Checks.MonitorPlanImport
{
    class cMPStackCategory : cCategory
    {
        cMonitorPlanImportProcess _MPProcess = null;
        string _PrimaryKey = null;

        #region Constructors

        public cMPStackCategory( cCheckEngine CheckEngine, cMonitorPlanImportProcess mpProcess, string SP_PK )
            : base( CheckEngine, (cProcess)mpProcess, "MPSTACK" )
        {
            _MPProcess = mpProcess;
            _PrimaryKey = "SP_PK";

            TableName = "MP_StackPipe";
            CurrentRowId = SP_PK;

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

            DataRowView currentRecord = new DataView( _MPProcess.SourceData.Tables["WS_StackPipe"], sFilter, "", DataViewRowState.CurrentRows )[0];
            SetCheckParameter( "Current_Workspace_Stack_Pipe", currentRecord, eParameterDataType.DataRowView );

            DataView view = new DataView( _MPProcess.SourceData.Tables["WS_UnitStackConfig"], "", "", DataViewRowState.CurrentRows );
            SetCheckParameter( "Workspace_Unit_Stack_Configuration_Records", view, eParameterDataType.DataView );

            //I do not believe that this parameter is used by this category (djw2)
            //SetCheckParameter( "Workspace_ORIS_Code", _ORISCode, eParameterDataType.String );
        }

        protected override bool SetErrorSuppressValues()
        {
          ErrorSuppressValues = null;
          return true;
        }

        protected override void SetRecordIdentifier()
        {
            DataRowView CurrentRecord = (DataRowView)GetCheckParameter( "Current_Workspace_Stack_Pipe" ).ParameterValue;
            RecordIdentifier = string.Format( "ORISCode {0} - Stack/Pipe {1}", CurrentRecord["ORIS_CODE"].ToString(), CurrentRecord["STACK_NAME"].ToString() );
        }

        #endregion    
    }
}
