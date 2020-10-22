using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;

using ECMPS.ErrorSuppression;

namespace ECMPS.Checks.MPScreenEvaluation
{
  public class cWAFRowCategory : cCategory
  {

    #region Private Fields

    private cMPScreenMain mMPScreenMain;
    private DataTable mWAFTable;

    #endregion


    #region Constructors

    public cWAFRowCategory(cCheckEngine CheckEngine, cMPScreenMain MPScreenMain, string MonitorLocationId, DataTable WAFTable)
      : base(CheckEngine, (cProcess)MPScreenMain, "SCRRWAF", WAFTable)
    {
      InitializeCurrent(MonitorLocationId);

      mMPScreenMain = MPScreenMain;
      mWAFTable = WAFTable;

      FilterData();

      SetRecordIdentifier();
    }

    public cWAFRowCategory(cCheckEngine CheckEngine, cMPScreenMain MPScreenMain)
      : base(CheckEngine, (cProcess)MPScreenMain, "SCRRWAF")
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

      SetCheckParameter("Current_WAF", new DataView(mWAFTable,
        "", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      SetCheckParameter("WAF_Records", new DataView(mMPScreenMain.SourceData.Tables["RectDuctWaf"],
        "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);


      //lookup tables
      SetCheckParameter("WAF_Method_Code_Lookup_Table", new DataView(mMPScreenMain.SourceData.Tables["WafMethodCode"],
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
