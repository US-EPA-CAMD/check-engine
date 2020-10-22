using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;

using ECMPS.ErrorSuppression;

namespace ECMPS.Checks.MPScreenEvaluation
{
  public class cQualificationLMERowCategory : cCategory
  {

    #region Private Fields

    private cMPScreenMain mMPScreenMain;
    private DataTable mQualLMETable;

    #endregion


    #region Constructors

    public cQualificationLMERowCategory(cCheckEngine CheckEngine, cMPScreenMain MPScreenMain, string MonitorLocationId, DataTable QualLMETable)
      : base(CheckEngine, (cProcess)MPScreenMain, "SCRQLME", QualLMETable)
    {
      InitializeCurrent(MonitorLocationId);

      mMPScreenMain = MPScreenMain;
      mQualLMETable = QualLMETable;

      FilterData();

      SetRecordIdentifier();
    }

    public cQualificationLMERowCategory(cCheckEngine CheckEngine, cMPScreenMain MPScreenMain)
      : base(CheckEngine, (cProcess)MPScreenMain, "SCRQLME")
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

      SetCheckParameter("Current_Qualification_LME", new DataView(mQualLMETable,
        "", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      SetCheckParameter("QualificationLME_Records", new DataView(mMPScreenMain.SourceData.Tables["MPQualificationLME"],
        "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      string MonQualFilter = string.Format("MON_QUAL_ID = '{0}'", mQualLMETable.Rows[0]["Mon_Qual_Id"]);
      SetCheckParameter("Current_Qualification", new DataView(mMPScreenMain.SourceData.Tables["MPQualification"],
        MonQualFilter, "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

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