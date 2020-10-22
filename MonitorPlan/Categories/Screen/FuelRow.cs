using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;

using ECMPS.ErrorSuppression;

namespace ECMPS.Checks.MPScreenEvaluation
{
  public class cFuelRow : cCategory
  {

    #region Private Fields

    private cMPScreenMain mMPScreenMain;
    private DataTable mFuelTable;

    #endregion


    #region Constructors

    public cFuelRow(cCheckEngine CheckEngine, cMPScreenMain MPScreenMain, string MonitorLocationId, DataTable FuelTable)
      : base(CheckEngine, (cProcess)MPScreenMain, "SCRFUEL", FuelTable)
    {
      InitializeCurrent(MonitorLocationId);

      mMPScreenMain = MPScreenMain;
      mFuelTable = FuelTable;

      FilterData();

      SetRecordIdentifier();
    }

    public cFuelRow(cCheckEngine CheckEngine, cMPScreenMain MPScreenMain)
      : base(CheckEngine, (cProcess)MPScreenMain, "SCRFUEL")
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
      SetCheckParameter("Current_Fuel",
                        new DataView(mMPScreenMain.SourceData.Tables["MPUnitFuel"],
                                     "", "",
                                     DataViewRowState.CurrentRows)[0],
                        eParameterDataType.DataRowView);

      SetDataViewCheckParameter("Location_Fuel_Records",
                                mMPScreenMain.SourceData.Tables["MPUnitFuel"],
                                "", "");

      SetDataViewCheckParameter("Fuel_Indicator_Code_Lookup_Table",
                                mMPScreenMain.SourceData.Tables["IndicatorCode"],
                                "", "");

      SetDataViewCheckParameter("Fuel_Demonstration_Method_Lookup_Table",
                                mMPScreenMain.SourceData.Tables["DemMethodCode"],
                                "", "");

      SetDataViewCheckParameter("Fuel_Code_Lookup_Table",
                                mMPScreenMain.SourceData.Tables["FuelCode"],
                                "", "");

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
