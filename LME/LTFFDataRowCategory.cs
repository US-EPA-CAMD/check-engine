using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;

namespace ECMPS.Checks.LME
{
    public class cLTFFDataRowCategory : cCategory
    {
        #region Private Fields

        private cLMEScreenProcess mLMEScreenProcess;
        private DataTable mLTFFDataTable;

        #endregion

        #region Constructors

        public cLTFFDataRowCategory(cCheckEngine CheckEngine, cLMEScreenProcess LMEScreenProcess, DataTable LTFFDataTable)
            : base(CheckEngine, (cProcess)LMEScreenProcess, "LMELTFF", LTFFDataTable)
        {
            mLMEScreenProcess = LMEScreenProcess;
            mLTFFDataTable = LTFFDataTable;

            FilterData();

            SetRecordIdentifier();
        }

        public cLTFFDataRowCategory(cCheckEngine CheckEngine, cLMEScreenProcess LMEScreenProcess)
            : base(CheckEngine, (cProcess)LMEScreenProcess, "LMELTFF")
        {
        }

        #endregion

      #region Public Methods

      public new bool ProcessChecks(string AMonitorLocationId)
      {
        return base.ProcessChecks(AMonitorLocationId);
      }

      #endregion

        #region Base Class Overrides

        protected override void FilterData()
        {

            SetCheckParameter("Current_LTFF_Record", new DataView(mLTFFDataTable,
                "", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataView);

            SetDataViewCheckParameter("Monitor_System_Records", mLMEScreenProcess.SourceData.Tables["MonitorSystem"], "", "");

            SetCheckParameter("Reporting_Period_Lookup_Table", new DataView(mLMEScreenProcess.SourceData.Tables["ReportingPeriod"],
                    "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);
        }

        protected override bool SetErrorSuppressValues()
        {
          ErrorSuppressValues = null;
          return true;
        }

        protected override void SetRecordIdentifier()
        {
            RecordIdentifier = "this record";
        }

        # endregion
    }
}
