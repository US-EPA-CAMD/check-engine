using System;
using System.Data;
using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

namespace ECMPS.Checks.QAScreenEvaluation
{
    public class cGFMTestRow : cCategory
    {

        #region Private Fields

        private cQAScreenMain mQAScreenMain;
        private DataTable mGFMTestTable;
        private string mComponentID;
        private string mMonitorLocationID;

        #endregion

        #region Constructors

        public cGFMTestRow(cCheckEngine ACheckEngine, cQAScreenMain QAScreenMain, string MonitorLocationId, string TestSumId, DataTable GFMTestTable)
            : base(ACheckEngine, (cProcess)QAScreenMain, "SCRGFMC", GFMTestTable)
        {
            InitializeCurrent(MonitorLocationId, TestSumId);

            mQAScreenMain = QAScreenMain;
            mGFMTestTable = GFMTestTable;
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
            SetCheckParameter("Current_GFM_Calibration_Test", new DataView(mGFMTestTable,
                "", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

            SetCheckParameter("Current_Test", new DataView(mGFMTestTable,
                "", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

            mComponentID = cDBConvert.ToString(((DataRowView)GetCheckParameter("Current_GFM_Calibration_Test").ParameterValue)["COMPONENT_ID"]);

            SetCheckParameter("Test_Result_Code_Lookup_Table", new DataView(mQAScreenMain.SourceData.Tables["TestResultCode"],
                "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

            SetCheckParameter("Test_Reason_Code_Lookup_Table", new DataView(mQAScreenMain.SourceData.Tables["TestReasonCode"],
                "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

            SetCheckParameter("Location_Test_Records", new DataView(mQAScreenMain.SourceData.Tables["TestSummary"],
                "Mon_loc_id = '" + mMonitorLocationID + "'", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

            SetCheckParameter("GFM_Cal_Test_Records", new DataView(mQAScreenMain.SourceData.Tables["TestSummary"],
                "test_type_cd = 'GFMCAL' and component_id = '" + mComponentID + "'", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

            SetCheckParameter("QA_Supplemental_Data_Records", new DataView(mQAScreenMain.SourceData.Tables["QASuppData"],
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
