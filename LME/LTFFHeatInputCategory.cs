using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;
using ECMPS.ErrorSuppression;

using ECMPS.Definitions.Extensions;


namespace ECMPS.Checks.LME
{
    public class cLTFFHeatInputCategory : cCategory
    {
        # region Private Fields

        private string _LTFFID;
        private cLMEGenerationProcess _LMEProcess;

        # endregion

        # region Constructors

        public cLTFFHeatInputCategory( cCheckEngine ACheckEngine, cLMEGenerationProcess ALMEGenerationProcess, cLMEInitializationCategory AParentCategory )
            : base( ACheckEngine, (cLMEGenerationProcess)ALMEGenerationProcess, AParentCategory, "LTFFGEN" )
        {
            _LMEProcess = ALMEGenerationProcess;
        }

        # endregion

        #region Public Methods

        public new bool ProcessChecks( string ALTFFID )
        {
            _LTFFID = ALTFFID;

            return base.ProcessChecks();
        }

        #endregion

        #region Base Class Overrides

        protected override void FilterData()
        {
            SetDataRowCheckParameter( "Current_LTFF_Record", _LMEProcess.SourceData.Tables["LTFF"], "ltff_id = '" + _LTFFID + "'", "ltff_id" );
        }

        protected override bool SetErrorSuppressValues()
        {
            bool result;

            result = true;

            return result;
        }

        protected override void SetRecordIdentifier()
        {
            //“Fuel Flow System ID “ + [system identifier]
            DataRowView CurrentLTFF = GetCheckParameter( "Current_LTFF_Record" ).ValueAsDataRowView();
            RecordIdentifier = "Fuel Flow System ID " + cDBConvert.ToString( CurrentLTFF["system_identifier"] );
        }

        # endregion

    }
}
