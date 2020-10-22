using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;
using ECMPS.ErrorSuppression;


namespace ECMPS.Checks.LME
{
    public class cLMEInitializationCategory : cCategory
    {
        # region Private Fields

        private cLMEGenerationProcess _LMEProcess;

        # endregion


        # region Constructors

        public cLMEInitializationCategory( cCheckEngine ACheckEngine, cLMEGenerationProcess ALMEGenerationProcess )
            : base( ACheckEngine, (cLMEGenerationProcess)ALMEGenerationProcess, "LOADGEN" )
        {
            _LMEProcess = ALMEGenerationProcess;
        }

        # endregion

        #region Public Methods

        public new bool ProcessChecks()
        {
            return base.ProcessChecks();
        }

        #endregion

        #region Base Class Overrides

        protected override void FilterData()
        {

            SetCheckParameter( "Hourly_Operating_Data_Records_for_LME_Configuration", new DataView( _LMEProcess.SourceData.Tables["LMEHourlyOp"], "rpt_period_id = " + CheckEngine.RptPeriodId, "", DataViewRowState.CurrentRows ), eParameterDataType.DataView );

            SetCheckParameter( "LTFF_Records", new DataView( _LMEProcess.SourceData.Tables["LTFF"], "", "", DataViewRowState.CurrentRows ), eParameterDataType.DataView );

            SetCheckParameter( "MP_Method_Records", new DataView( _LMEProcess.SourceData.Tables["MPMethod"], "", "", DataViewRowState.CurrentRows ), eParameterDataType.DataView );

            SetCheckParameter( "MP_Qualification_Records", new DataView( _LMEProcess.SourceData.Tables["MPQualification"], "", "", DataViewRowState.CurrentRows ), eParameterDataType.DataView );

            SetCheckParameter( "Reporting_Period_Lookup_Table", new DataView( _LMEProcess.SourceData.Tables["ReportingPeriod"], "", "", DataViewRowState.CurrentRows ), eParameterDataType.DataView );

            SetCheckParameter( "Fuel_Type_Reality_Checks_for_GCV_Cross_Check_Table", _LMEProcess.SourceData.Tables["CrossCheck_FuelTypeRealityChecksforGCV"].DefaultView, eParameterDataType.DataView );
            SetCheckParameter( "Fuel_Type_Warning_Levels_for_GCV_Cross_Check_Table", _LMEProcess.SourceData.Tables["CrossCheck_FuelTypeWarningLevelsforGCV"].DefaultView, eParameterDataType.DataView );
        }

        protected override bool SetErrorSuppressValues()
        {
            bool result;

            result = true;

            return result;
        }

        protected override void SetRecordIdentifier()
        {
        }

        # endregion
    }
}
