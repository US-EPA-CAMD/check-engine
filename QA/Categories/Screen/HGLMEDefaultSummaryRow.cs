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
    public class cHGLMEDefaultSummaryRow : cCategory
    {
        #region Private Fields

    private cQAScreenMain mQAScreenMain;
    private DataTable mHGLMEDefaultSummaryTable;

    #endregion

    #region Constructors

    public cHGLMEDefaultSummaryRow(cCheckEngine ACheckEngine, cQAScreenMain QAScreenMain, string MonitorLocationId, string TestSumId, DataTable HGLMEDefaultSummaryTable)
        : base(ACheckEngine, (cProcess)QAScreenMain, "SCRHGSM", HGLMEDefaultSummaryTable)
    {
      InitializeCurrent(MonitorLocationId, TestSumId);

      mQAScreenMain = QAScreenMain;
      mHGLMEDefaultSummaryTable = HGLMEDefaultSummaryTable;

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
        SetCheckParameter("Current_Hg_LME_Default_Test_Level", new DataView(mHGLMEDefaultSummaryTable,
            "", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

        SetCheckParameter("Current_Hg_LME_Default_Test", new DataView(mQAScreenMain.SourceData.Tables["QAHgLMEDefaultTest"],
            "", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

        SetCheckParameter("Hg_LME_Default_Test_Level_Records", new DataView(mQAScreenMain.SourceData.Tables["QAHgLMEDefaultLevel"],
            "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

        SetCheckParameter("Hg_LME_Operating_Level_Code_Lookup_Table", new DataView(mQAScreenMain.SourceData.Tables["OperatingLevelCode"],
            "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

        SetCheckParameter("Qualification_Records", new DataView(mQAScreenMain.SourceData.Tables["QAQualification"],
            "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

        SetCheckParameter("Facility_Location_Records", new DataView(mQAScreenMain.SourceData.Tables["QAFacilityLocation"],
            "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

        SetCheckParameter("Unit_Stack_Configuration_Records", new DataView(mQAScreenMain.SourceData.Tables["QAUnitStackConfiguration"],
            "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

        SetCheckParameter("Reference_Method_Code_Lookup_Table", new DataView(mQAScreenMain.SourceData.Tables["RefMethodCode"],
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
