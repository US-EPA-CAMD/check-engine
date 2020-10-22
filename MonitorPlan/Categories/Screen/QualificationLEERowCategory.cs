using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;

using ECMPS.ErrorSuppression;

namespace ECMPS.Checks.MPScreenEvaluation
{
  public class cQualificationLEERowCategory : cCategory
  {

    #region Private Fields

    private cMPScreenMain mMPScreenMain;
    private DataTable mQualLEETable;

    #endregion


    #region Constructors

    public cQualificationLEERowCategory(cCheckEngine CheckEngine, cMPScreenMain MPScreenMain, string MonitorLocationId, DataTable QualLEETable)
      : base(CheckEngine, (cProcess)MPScreenMain, "SCRQLEE", QualLEETable)
    {
      InitializeCurrent(MonitorLocationId);

      mMPScreenMain = MPScreenMain;
      mQualLEETable = QualLEETable;

      FilterData();

      SetRecordIdentifier();
    }

    public cQualificationLEERowCategory(cCheckEngine CheckEngine, cMPScreenMain MPScreenMain)
      : base(CheckEngine, (cProcess)MPScreenMain, "SCRQLEE")
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

      SetCheckParameter("Current_Qualification_LEE", new DataView(mQualLEETable,
        "", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);
	
      SetCheckParameter("QualificationLEE_Records", new DataView(mMPScreenMain.SourceData.Tables["MonitorQualificationLEE"],
        "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      string MonQualFilter = string.Format("MON_QUAL_ID = '{0}'", mQualLEETable.Rows[0]["Mon_Qual_Id"]);
      SetCheckParameter("Current_Qualification", new DataView(mMPScreenMain.SourceData.Tables["MPQualification"],
        MonQualFilter, "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

	  //lookup tables
	  SetDataViewCheckParameter("Qualification_LEE_Test_Type_Code_Lookup_Table", mMPScreenMain.SourceData.Tables["QualLEETestTypeCode"], "", "");
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