using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;

namespace ECMPS.Checks.QAImport
{
    class cQATestCategory : cCategory
    {
        cQAImportProcess _QAProcess = null;
        string _PrimaryKey = null;

        #region Constructors

        public cQATestCategory( cCheckEngine CheckEngine, cQAImportProcess qaProcess, string TS_PK )
            : base( CheckEngine, (cProcess)qaProcess, "QATEST" )
        {
            _QAProcess = qaProcess;
            _PrimaryKey = "TS_PK";

            TableName = "QA_TestSummary";
            CurrentRowId = TS_PK;

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
            string sFilter = string.Format( "{0}={1}", _PrimaryKey, CurrentRowId );

            DataRowView currentRecord = new DataView( _QAProcess.SourceData.Tables["WS_TestSummary"], sFilter, "", DataViewRowState.CurrentRows )[0];
            SetCheckParameter( "Current_Workspace_Test_Summary", currentRecord, eParameterDataType.DataRowView );

            // filter our row out of this result set
            sFilter = string.Format( "{0}<>{1}", _PrimaryKey, CurrentRowId );
            DataView view = new DataView( _QAProcess.SourceData.Tables["WS_TestSummary"], sFilter, null, DataViewRowState.CurrentRows );
            SetCheckParameter( "Workspace_Test_Summary_Records", view, eParameterDataType.DataView );

            sFilter = string.Format( "{0}={1}", "TS_FK", CurrentRowId );

            view = new DataView( _QAProcess.SourceData.Tables["WS_Rata"], sFilter, null, DataViewRowState.CurrentRows );
            SetCheckParameter( "Workspace_RATA_Records", view, eParameterDataType.DataView );

            view = new DataView( _QAProcess.SourceData.Tables["WS_FlowRATARun"], sFilter, null, DataViewRowState.CurrentRows );
            SetCheckParameter( "Workspace_Flow_RATA_Run_Records", view, eParameterDataType.DataView );

            view = new DataView( _QAProcess.SourceData.Tables["WS_AECorrTestSummary"], sFilter, null, DataViewRowState.CurrentRows );
            SetCheckParameter( "Workspace_AE_Corr_Summary_Records", view, eParameterDataType.DataView );

            view = new DataView( _QAProcess.SourceData.Tables["WS_CalibrationInjection"], sFilter, null, DataViewRowState.CurrentRows );
            SetCheckParameter( "Workspace_Calibration_Injection_Records", view, eParameterDataType.DataView );

            view = new DataView( _QAProcess.SourceData.Tables["WS_CycleTimeSummary"], sFilter, null, DataViewRowState.CurrentRows );
            SetCheckParameter( "Workspace_Cycle_Time_Summary_Records", view, eParameterDataType.DataView );

            view = new DataView( _QAProcess.SourceData.Tables["WS_FlowToLoadCheck"], sFilter, null, DataViewRowState.CurrentRows );
            SetCheckParameter( "Workspace_Flow_to_Load_Check_Records", view, eParameterDataType.DataView );

            view = new DataView( _QAProcess.SourceData.Tables["WS_FlowToLoadReference"], sFilter, null, DataViewRowState.CurrentRows );
            SetCheckParameter( "Workspace_Flow_to_Load_Reference_Records", view, eParameterDataType.DataView );

            view = new DataView( _QAProcess.SourceData.Tables["WS_FuelFlowmeterAccuracy"], sFilter, null, DataViewRowState.CurrentRows );
            SetCheckParameter( "Workspace_Fuel_Flowmeter_Accuracy_Records", view, eParameterDataType.DataView );

            view = new DataView( _QAProcess.SourceData.Tables["WS_FuelFlowToLoadBaseline"], sFilter, null, DataViewRowState.CurrentRows );
            SetCheckParameter( "Workspace_Fuelflow_to_Load_Baseline_Records", view, eParameterDataType.DataView );

            view = new DataView( _QAProcess.SourceData.Tables["WS_FuelFlowToLoadTest"], sFilter, null, DataViewRowState.CurrentRows );
            SetCheckParameter( "Workspace_Fuelflow_to_Load_Test_Records", view, eParameterDataType.DataView );

            view = new DataView( _QAProcess.SourceData.Tables["WS_LinearitySummary"], sFilter, null, DataViewRowState.CurrentRows );
            SetCheckParameter( "Workspace_Linearity_Summary_Records", view, eParameterDataType.DataView );

			view = new DataView(_QAProcess.SourceData.Tables["WS_HgTestSummary"], sFilter, null, DataViewRowState.CurrentRows);
			SetCheckParameter("Workspace_Hg_Summary_Records", view, eParameterDataType.DataView);

            view = new DataView( _QAProcess.SourceData.Tables["WS_OnOffCalibration"], sFilter, null, DataViewRowState.CurrentRows );
            SetCheckParameter( "Workspace_Online_Offline_Calibration_Records", view, eParameterDataType.DataView );

            view = new DataView( _QAProcess.SourceData.Tables["WS_TestQualification"], sFilter, null, DataViewRowState.CurrentRows );
            SetCheckParameter( "Workspace_Test_Qualification_Records", view, eParameterDataType.DataView );

            view = new DataView( _QAProcess.SourceData.Tables["WS_TransAccuracy"], sFilter, null, DataViewRowState.CurrentRows );
            SetCheckParameter( "Workspace_Transmitter_Transducer_Records", view, eParameterDataType.DataView );

            view = new DataView( _QAProcess.SourceData.Tables["WS_UnitDefaultTest"], sFilter, null, DataViewRowState.CurrentRows );
            SetCheckParameter( "Workspace_Unit_Default_Test_Records", view, eParameterDataType.DataView );

            view = new DataView( _QAProcess.SourceData.Tables["PROD_Test_Summary"], null, null, DataViewRowState.CurrentRows );
            SetCheckParameter( "Production_Test_Summary_Records", view, eParameterDataType.DataView );

            view = new DataView( _QAProcess.SourceData.Tables["PROD_Monitor_System"], null, null, DataViewRowState.CurrentRows );
            SetCheckParameter( "Production_Monitor_System_Records", view, eParameterDataType.DataView );

            view = new DataView( _QAProcess.SourceData.Tables["PROD_Monitor_Method"], null, null, DataViewRowState.CurrentRows );
            SetCheckParameter( "Production_Monitor_Method_Records", view, eParameterDataType.DataView );

            view = new DataView( _QAProcess.SourceData.Tables["PROD_Component"], null, null, DataViewRowState.CurrentRows );
            SetCheckParameter( "Production_Component_Records", view, eParameterDataType.DataView );

            view = new DataView( _QAProcess.SourceData.Tables["PROD_QA_Supp_Data"], null, null, DataViewRowState.CurrentRows );
            SetCheckParameter( "QA_Supplemental_Data_Records", view, eParameterDataType.DataView );

            view = new DataView( _QAProcess.SourceData.Tables["WS_AE_HI_OIL"], null, null, DataViewRowState.CurrentRows );
            SetCheckParameter( "Workspace_Appendix_E_Heat_Input_From_Oil_Records", view, eParameterDataType.DataView );

            view = new DataView( _QAProcess.SourceData.Tables["WS_AE_HI_GAS"], null, null, DataViewRowState.CurrentRows );
            SetCheckParameter( "Workspace_Appendix_E_Heat_Input_From_Gas_Records", view, eParameterDataType.DataView );

            view = new DataView( _QAProcess.SourceData.Tables["WS_ProtocolGas"], null, null, DataViewRowState.CurrentRows );
            SetCheckParameter( "Workspace_ProtocolGas_Records", view, eParameterDataType.DataView );

            view = new DataView( _QAProcess.SourceData.Tables["WS_AirEmissionTesting"], null, null, DataViewRowState.CurrentRows );
            SetCheckParameter( "Workspace_AirEmissionTesting_Records", view, eParameterDataType.DataView );
        }

        protected override bool SetErrorSuppressValues()
        {
            ErrorSuppressValues = null;
            return true;
        }

        protected override void SetRecordIdentifier()
        {
            DataRowView CurrentRecord = (DataRowView)GetCheckParameter( "Current_Workspace_Test_Summary" ).ParameterValue;
            RecordIdentifier = string.Format( "Test Type Code {0}", CurrentRecord["TEST_TYPE_CD"].ToString() );
        }

        #endregion
    }
}
