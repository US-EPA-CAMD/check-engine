using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

namespace ECMPS.Checks.QAScreenEvaluation
{
    public class cGFMLevelDataRow : cCategory
    {
        #region Private Fields

        private cQAScreenMain mQAScreenMain;
        private DataTable mGFMTestDataTable;
        private string mMonitorLocationID;

        #endregion

        #region Constructors

        public cGFMLevelDataRow(cCheckEngine ACheckEngine, cQAScreenMain QAScreenMain, string MonitorLocationId, string TestSumId, DataTable GFMTestDataTable)
            : base(ACheckEngine, (cProcess)QAScreenMain, "SCRGFML", GFMTestDataTable)
        {
            InitializeCurrent(MonitorLocationId, TestSumId);

            mQAScreenMain = QAScreenMain;
            mGFMTestDataTable = GFMTestDataTable;
            mMonitorLocationID = MonitorLocationId;

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
            SetCheckParameter("Current_GFM_Calibration_Data", new DataView(mGFMTestDataTable,
                "", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

            SetCheckParameter("Current_GFM_Calibration_Test", new DataView(mQAScreenMain.SourceData.Tables["QAGFM"],
                "", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

            SetCheckParameter("GFM_Cal_Data_Records", new DataView(mQAScreenMain.SourceData.Tables["QAGFMData"],
                "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);
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
