using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;

namespace ECMPS.Checks.MonitorPlanImport
{
    class cMPUnitStackConfigCategory : cCategory
    {
        cMonitorPlanImportProcess _MPProcess = null;
        string _PrimaryKey = null;

        #region Constructors

        public cMPUnitStackConfigCategory( cCheckEngine CheckEngine, cMonitorPlanImportProcess mpProcess, string USC_PK )
            : base( CheckEngine, (cProcess)mpProcess, "MPUNSTK" )
        {
            _MPProcess = mpProcess;
            _PrimaryKey = "USC_PK";

            TableName = "MP_UnitStackConfig";
            CurrentRowId = USC_PK;

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

            DataRowView currentRecord = new DataView( _MPProcess.SourceData.Tables["WS_UnitStackConfig"], sFilter, null, DataViewRowState.CurrentRows )[0];
            SetCheckParameter( "Current_Workspace_Unit_Stack_Configuation", currentRecord, eParameterDataType.DataRowView );

            DataView view = new DataView( _MPProcess.SourceData.Tables["WS_StackPipe"], null, null, DataViewRowState.CurrentRows );
            SetCheckParameter( "Workspace_Stack_Pipe_Records", view, eParameterDataType.DataView );

            // this was set in another category?
            //view = new DataView( _MPProcess.SourceData.Tables["WS_Unit"], null, null, DataViewRowState.CurrentRows );
            //SetCheckParameter( "Workspace_Unit_Records", view, ParameterTypes.DATAVW );
        }

        protected override bool SetErrorSuppressValues()
        {
          ErrorSuppressValues = null;
          return true;
        }

        protected override void SetRecordIdentifier()
        {
            DataRowView CurrentRecord = (DataRowView)GetCheckParameter( "Current_Workspace_Unit_Stack_Configuation" ).ParameterValue;
            RecordIdentifier = string.Format( "StackPipe ID {0} - UnitID {1}", CurrentRecord["STACK_NAME"].ToString(), CurrentRecord["UNITID"].ToString() );
        }

        #endregion
    }
}
