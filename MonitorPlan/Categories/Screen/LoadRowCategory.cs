using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;

using ECMPS.ErrorSuppression;

namespace ECMPS.Checks.MPScreenEvaluation
{
  public class cLoadRowCategory : cCategory
  {

    #region Private Fields

    private cMPScreenMain mMPScreenMain;
    private DataTable mLoadTable;

    #endregion


    #region Constructors

    public cLoadRowCategory(cCheckEngine CheckEngine, cMPScreenMain MPScreenMain, string MonitorLocationId, DataTable LoadTable)
      : base(CheckEngine, (cProcess)MPScreenMain, "SCRLOAD", LoadTable)
    {
      InitializeCurrent(MonitorLocationId);

      mMPScreenMain = MPScreenMain;
      mLoadTable = LoadTable;

      FilterData();

      SetRecordIdentifier();
    }

    public cLoadRowCategory(cCheckEngine CheckEngine, cMPScreenMain MPScreenMain)
      : base(CheckEngine, (cProcess)MPScreenMain, "SCRLOAD")
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

      SetCheckParameter("Current_Load", new DataView(mLoadTable,
        "", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      SetCheckParameter("Load_Records", new DataView(mMPScreenMain.SourceData.Tables["MPLoad"],
        "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Current_Location", new DataView(mMPScreenMain.SourceData.Tables["MPLocation"],
          "", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      SetCheckParameter("Unit_Stack_Configuration_Records", new DataView(mMPScreenMain.SourceData.Tables["MPUnitStackConfiguration"],
        "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

	  //Used in screen parameter - using USC associated with current location allows the checks using this parameter to work correctly in both screen and report categories
	  SetCheckParameter("MP_Unit_Stack_Configuration_Records", new DataView(mMPScreenMain.SourceData.Tables["MPUnitStackConfiguration"],
		"", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Parameter_Units_Of_Measure_Lookup_Table", new DataView(mMPScreenMain.SourceData.Tables["ParameterUOM"],
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
