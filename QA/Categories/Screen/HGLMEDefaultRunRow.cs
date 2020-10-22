using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

namespace ECMPS.Checks.QAScreenEvaluation
{
    public class cHGLMEDefaultRunRow : cCategory
    {
        #region Private Fields

        private cQAScreenMain mQAScreenMain;
        private DataTable mHGLMEDefaultRunTable;
        //private string mGasLevelCd;

        #endregion

        #region Constructors

        public cHGLMEDefaultRunRow(cCheckEngine ACheckEngine, cQAScreenMain QAScreenMain, string MonitorLocationId, string TestSumId, DataTable HGLMEDefaultRunTable)
            : base(ACheckEngine, (cProcess)QAScreenMain, "SCRHGRN", HGLMEDefaultRunTable)
        {
            InitializeCurrent(MonitorLocationId, TestSumId);

            mQAScreenMain = QAScreenMain;
            mHGLMEDefaultRunTable = HGLMEDefaultRunTable;

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
            SetCheckParameter("Current_Hg_LME_Default_Run", new DataView(mHGLMEDefaultRunTable,
                "", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

            SetCheckParameter("Hg_LME_Default_Run_Records", new DataView(mQAScreenMain.SourceData.Tables["QAHgLMEDefaultRun"],
                "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

            SetCheckParameter("Current_Hg_LME_Default_Test", new DataView(mQAScreenMain.SourceData.Tables["QATestSummary"],
                "", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

            SetCheckParameter("Current_Hg_LME_Default_Test_Level", new DataView(mQAScreenMain.SourceData.Tables["QAHgLMEDefaultLevel"],
                "", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);
        }


        protected override void SetRecordIdentifier()
        {

            RecordIdentifier = "this record";

        }

        protected override bool SetErrorSuppressValues()
        {
            ErrorSuppressValues = null;
            return true;
        }

        #endregion
    }
}
