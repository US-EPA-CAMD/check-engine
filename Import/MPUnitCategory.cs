using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;

namespace ECMPS.Checks.MonitorPlanImport
{
    class cMPUnitCategory : cCategory
    {
        cMonitorPlanImportProcess _MPProcess = null;
        string _PrimaryKey = null;

        #region Constructors

        public cMPUnitCategory( cCheckEngine CheckEngine, cMonitorPlanImportProcess mpProcess, string Unit_PK )
            : base( CheckEngine, (cProcess)mpProcess, "MPUNIT" )
        {
            _MPProcess = mpProcess;
            _PrimaryKey = "Unit_PK";

            TableName = "MP_Unit";
            CurrentRowId = Unit_PK;

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

            DataRowView currentRecord = new DataView( _MPProcess.SourceData.Tables["WS_Unit"], sFilter, "", DataViewRowState.CurrentRows )[0];
            SetCheckParameter( "Current_Workspace_Unit", currentRecord, eParameterDataType.DataRowView );

            DataView view = new DataView( _MPProcess.SourceData.Tables["WS_UnitStackConfig"], "", "", DataViewRowState.CurrentRows );
            SetCheckParameter( "Workspace_Unit_Stack_Configuration_Records", view, eParameterDataType.DataView );

            view = new DataView( _MPProcess.SourceData.Tables["PROD_Unit"], "", "", DataViewRowState.CurrentRows );
            SetCheckParameter( "Production_Unit_Records", view, eParameterDataType.DataView );

            //I do not believe this parameter is used in this category (djw2)
            //view = new DataView( _MPProcess.SourceData.Tables["PROD_Monitor_Plan_Location"], "", "", DataViewRowState.CurrentRows );
            //SetCheckParameter( "Production_Monitor_Plan_Location_Records", view, eParameterDataType.DataView );
        }

        protected override bool SetErrorSuppressValues()
        {
          ErrorSuppressValues = null;
          return true;
        }

        protected override void SetRecordIdentifier()
        {
            DataRowView CurrentRecord = (DataRowView)GetCheckParameter( "Current_Workspace_Unit" ).ParameterValue;
            RecordIdentifier = string.Format( "ORISCode {0} - UnitID {1}", CurrentRecord["ORIS_CODE"].ToString(), CurrentRecord["UNITID"].ToString() );
        }

        #endregion
    }
}
