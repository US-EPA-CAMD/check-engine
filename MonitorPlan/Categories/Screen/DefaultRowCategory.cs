using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;

using ECMPS.ErrorSuppression;

namespace ECMPS.Checks.MPScreenEvaluation
{
  public class cDefaultRowCategory : cCategory
  {

    #region Private Fields

    private cMPScreenMain mMPScreenMain;
    private DataTable mDefaultTable;

    #endregion


    #region Constructors

    public cDefaultRowCategory(cCheckEngine CheckEngine, cMPScreenMain MPScreenMain, string MonitorLocationId, DataTable DefaultTable)
      : base(CheckEngine, (cProcess)MPScreenMain, "SCRDFLT", DefaultTable)
    {
      InitializeCurrent(MonitorLocationId);

      mMPScreenMain = MPScreenMain;
      mDefaultTable = DefaultTable;

      FilterData();

      SetRecordIdentifier();
    }

    public cDefaultRowCategory(cCheckEngine CheckEngine, cMPScreenMain MPScreenMain)
      : base(CheckEngine, (cProcess)MPScreenMain, "SCRDFLT")
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
      SetCheckParameter("Current_Location", new DataView(mMPScreenMain.SourceData.Tables["MPLocation"],
          "", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataView);

      SetCheckParameter("Current_Default", new DataView(mDefaultTable,
        "", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      SetCheckParameter("Default_Records", new DataView(mMPScreenMain.SourceData.Tables["MPDefault"],
        "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Location_Fuel_Records", new DataView(mMPScreenMain.SourceData.Tables["LocationFuel"],
        "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Location_Unit_Type_Records", new DataView(mMPScreenMain.SourceData.Tables["LocationUnitType"],
          "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Unit_Stack_Configuration_Records", new DataView(mMPScreenMain.SourceData.Tables["MPUnitStackConfiguration"],
          "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Method_Records", new DataView(mMPScreenMain.SourceData.Tables["MPMethod"],
          "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      //lookup tables
      SetCheckParameter("Fuel_Code_Lookup_Table", new DataView(mMPScreenMain.SourceData.Tables["FuelCode"],
          "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Default_Purpose_Code_Lookup_Table", new DataView(mMPScreenMain.SourceData.Tables["DefaultPurposeCode"],
          "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Default_Source_Code_Lookup_Table", new DataView(mMPScreenMain.SourceData.Tables["DefaultSourceCode"],
          "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Parameter_Units_Of_Measure_Lookup_Table", new DataView(mMPScreenMain.SourceData.Tables["ParameterUOM"],
          "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Units_of_Measure_Code_Lookup_Table", new DataView(mMPScreenMain.SourceData.Tables["UnitsOfMeasureCode"],
          "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      //cross check tables
      SetCheckParameter("Default_Parameter_List",
         new DataView(mMPScreenMain.SourceData.Tables["CrossCheck_ParametertoCategory"], "", "", DataViewRowState.CurrentRows),
         eParameterDataType.DataView);

      SetCheckParameter("Default_Parameter_To_Purpose_Cross_Check_Table",
          new DataView(mMPScreenMain.SourceData.Tables["CrossCheck_DefaultParametertoPurpose"], "", "", DataViewRowState.CurrentRows),
          eParameterDataType.DataView);

      SetCheckParameter("Default_Parameter_To_Source_Of_Value_Cross_Check_Table",
          new DataView(mMPScreenMain.SourceData.Tables["CrossCheck_DefaultParametertoSourceofValue"], "", "", DataViewRowState.CurrentRows),
          eParameterDataType.DataView);

      SetCheckParameter("Fuel_Code_To_Minimum_And_Maximum_Moisture_Default_Cross_Check_Table",
          new DataView(mMPScreenMain.SourceData.Tables["CrossCheck_FuelCodetoMinimumandMaximumMoistureDefaultValue"], "", "", DataViewRowState.CurrentRows),
          eParameterDataType.DataView);
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
