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
    public class cHGLMEDefaultRow : cCategory
    {
        
    #region Private Fields

    private cQAScreenMain mQAScreenMain;
    private DataTable mHGLMEDefaultTable;
    private string mMonitorLocationID;

    #endregion

    #region Constructors

    public cHGLMEDefaultRow(cCheckEngine ACheckEngine, cQAScreenMain QAScreenMain, string MonitorLocationId, string TestSumId, DataTable HGLMEDefaultTable)
        : base(ACheckEngine, (cProcess)QAScreenMain, "SCRHGLM", HGLMEDefaultTable)
    {
      InitializeCurrent(MonitorLocationId, TestSumId);

      mQAScreenMain = QAScreenMain;
      mHGLMEDefaultTable = HGLMEDefaultTable;
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
        SetCheckParameter("Current_Hg_LME_Default_Test", new DataView(mHGLMEDefaultTable,
            "", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

        SetCheckParameter("Current_Test", new DataView(mHGLMEDefaultTable,
            "", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

        //SetCheckParameter("Unit_Stack_Configuration_Records", new DataView(mQAScreenMain.SourceData.Tables["QAUnitStackConfiguration"],
        //    "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);
        
        SetCheckParameter("Hg_LME_Default_Test_Records", new DataView(mQAScreenMain.SourceData.Tables["TestSummary"],
            "test_type_cd = 'HGLME' and mon_loc_id = '" + mMonitorLocationID + "'", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

        SetCheckParameter("Location_Test_Records", new DataView(mQAScreenMain.SourceData.Tables["TestSummary"],
          "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

        SetCheckParameter("QA_Supplemental_Data_Records", new DataView(mQAScreenMain.SourceData.Tables["QASuppData"],
          "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

        SetCheckParameter("Test_Reason_Code_Lookup_Table", new DataView(mQAScreenMain.SourceData.Tables["TestReasonCode"],
          "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

        SetCheckParameter("Common_Stack_Test_Code_Lookup_Table", new DataView(mQAScreenMain.SourceData.Tables["CSTestCode"],
            "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

        DataView CSTestCdLookup = GetCheckParameter("Common_Stack_Test_Code_Lookup_Table").ValueAsDataView();

        SetCheckParameter("Current_Monitor_Location", new DataView(mQAScreenMain.SourceData.Tables["MonitorLocation"],
           "", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataView);

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
