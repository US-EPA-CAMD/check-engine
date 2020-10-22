using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;

using ECMPS.ErrorSuppression;

namespace ECMPS.Checks.MPScreenEvaluation
{
    public class cAnalyzerRangeRow : cCategory
    {

        #region Private Fields

        private cMPScreenMain mMPScreenMain;
        private DataTable mAnalyzerRangeTable;

        #endregion


        #region Constructors

        public cAnalyzerRangeRow(cCheckEngine CheckEngine, cMPScreenMain MPScreenMain, string MonitorLocationId, DataTable AnalyzerRangeTable)
            : base(CheckEngine, (cProcess)MPScreenMain, "SCRANRG", AnalyzerRangeTable)
        {
            InitializeCurrent(MonitorLocationId);

            mMPScreenMain = MPScreenMain;
            mAnalyzerRangeTable = AnalyzerRangeTable;

            FilterData();

            SetRecordIdentifier();
        }

        public cAnalyzerRangeRow(cCheckEngine ACheckEngine, cMPScreenMain AMPScreenMain)
            : base(ACheckEngine, (cProcess)AMPScreenMain, "SCRANRG")
        {

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


            SetCheckParameter("Current_Analyzer_Range", new DataView(mAnalyzerRangeTable,
              "", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

            string MonCompFilter = string.Format("COMPONENT_ID = '{0}'", mAnalyzerRangeTable.Rows[0]["Component_Id"]);

            SetCheckParameter("Current_Component", new DataView(mMPScreenMain.SourceData.Tables["MPComponent"],
             MonCompFilter, "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

            SetCheckParameter("Location_Analyzer_Range_Records", new DataView(mMPScreenMain.SourceData.Tables["MPAnalyzerRange"],
               "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

            SetCheckParameter("Analyzer_Range_Code_Lookup_Table",
                                 new DataView(mMPScreenMain.SourceData.Tables["AnalyzerRangeCode"], "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);
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
