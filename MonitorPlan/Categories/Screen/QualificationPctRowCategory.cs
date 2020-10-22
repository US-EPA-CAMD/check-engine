using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;

using ECMPS.ErrorSuppression;

namespace ECMPS.Checks.MPScreenEvaluation
{
  public class cQualificationPctRowCategory : cCategory
  {

    #region Private Fields

    private cMPScreenMain mMPScreenMain;
    private DataTable mQualPctTable;

    #endregion


    #region Constructors

    public cQualificationPctRowCategory(cCheckEngine CheckEngine, cMPScreenMain MPScreenMain, string MonitorLocationId, DataTable QualPctTable)
      : base(CheckEngine, (cProcess)MPScreenMain, "SCRQPCT", QualPctTable)
    {
      InitializeCurrent(MonitorLocationId);

      mMPScreenMain = MPScreenMain;
      mQualPctTable = QualPctTable;

      FilterData();

      SetRecordIdentifier();
    }

    public cQualificationPctRowCategory(cCheckEngine CheckEngine, cMPScreenMain MPScreenMain)
      : base(CheckEngine, (cProcess)MPScreenMain, "SCRQPCT")
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

      SetCheckParameter("Current_Qualification_Percent", new DataView(mQualPctTable,
        "", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      SetCheckParameter("Qualification_Percent_Records", new DataView(mMPScreenMain.SourceData.Tables["MPQualificationPct"],
        "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      //DataRowView CurrentQualPct = GetCheckParameter("Current_Qualification_Percent").ValueAsDataRowView();
      //string CurrentId = CurrentQualPct["mon_qual_id"].ToString();
      string MonQualFilter = string.Format("MON_QUAL_ID = '{0}'", mQualPctTable.Rows[0]["Mon_Qual_Id"]);
      SetCheckParameter("Current_Qualification", new DataView(mMPScreenMain.SourceData.Tables["MPQualification"],
        MonQualFilter, "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);
      //SetCheckParameter("Current_Qualification", new DataView(mMPScreenMain.SourceData.Tables["MPQualification"],
      //  "", "", DataViewRowState.CurrentRows)[0], ParameterTypes.DATAROW);
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