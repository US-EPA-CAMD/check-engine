using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;

using ECMPS.ErrorSuppression;

namespace ECMPS.Checks.MPScreenEvaluation
{
    public class cCalibrationStandardDataRowCategory : cCategory
    {
        #region Private Fields

        private cMPScreenMain mMPScreenMain;
        private DataTable mCalibrationStandardDataTable;

        #endregion

        #region Constructors

        public cCalibrationStandardDataRowCategory(cCheckEngine CheckEngine, cMPScreenMain MPScreenMain, string MonitorLocationId, DataTable CalibrationStandardDataTable)
            : base(CheckEngine, (cProcess)MPScreenMain, "SCRCALS", CalibrationStandardDataTable)
    {
      InitializeCurrent(MonitorLocationId);

      mMPScreenMain = MPScreenMain;
      mCalibrationStandardDataTable = CalibrationStandardDataTable;

      FilterData();

      SetRecordIdentifier();
    }

    public cCalibrationStandardDataRowCategory(cCheckEngine ACheckEngine, cMPScreenMain AMPScreenMain)
            : base(ACheckEngine, (cProcess)AMPScreenMain, "SCRCALS")
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

            SetCheckParameter("Current_Calibration_Standard_Data", new DataView(mCalibrationStandardDataTable,
              "", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

            string Filter = string.Format("Component_ID = '{0}'", mCalibrationStandardDataTable.Rows[0]["Component_ID"]);
            SetCheckParameter("Current_Component", new DataView(mMPScreenMain.SourceData.Tables["MPComponent"],
              Filter, "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

            SetCheckParameter("Location_Calibration_Standard_Data_Records", new DataView(mMPScreenMain.SourceData.Tables["MPCalibrationStandardData"],
               "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

            SetCheckParameter("Calibration_Standard_Code_Lookup_Table",
                                 new DataView(mMPScreenMain.SourceData.Tables["CalibrationStandardCode"], "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

            SetCheckParameter("Calibration_Standard_Code_to_Component_Type_Cross_Check_Table",
                new DataView(mMPScreenMain.SourceData.Tables["CrossCheck_CalibrationStandardCodetoComponentType"], "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);
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
