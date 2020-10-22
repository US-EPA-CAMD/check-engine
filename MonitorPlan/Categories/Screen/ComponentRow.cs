using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;

using ECMPS.ErrorSuppression;

namespace ECMPS.Checks.MPScreenEvaluation
{
  public class cComponentRow : cCategory
  {

    #region Private Fields

    private cMPScreenMain mMPScreenMain;
    private DataTable mComponentTable;

    #endregion


    #region Constructors

    public cComponentRow(cCheckEngine CheckEngine, cMPScreenMain MPScreenMain, string MonitorLocationId, DataTable ComponentTable)
      : base(CheckEngine, (cProcess)MPScreenMain, "SCRCOMP", ComponentTable)
    {
      InitializeCurrent(MonitorLocationId);

      mMPScreenMain = MPScreenMain;
      mComponentTable = ComponentTable;

      FilterData();

      SetRecordIdentifier();
    }

    public cComponentRow(cCheckEngine ACheckEngine, cMPScreenMain AMPScreenMain)
      : base(ACheckEngine, (cProcess)AMPScreenMain, "SCRCOMP")
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
      
      SetCheckParameter("Current_Component", new DataView(mComponentTable,
        "", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      SetCheckParameter("Component_Records", new DataView(mMPScreenMain.SourceData.Tables["MPComponent"],
        "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Used_Identifier_Records", new DataView(mMPScreenMain.SourceData.Tables["UsedIdentifier"],
        "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Component_Type_And_Basis_To_Sample_Acquisition_Method_Cross_Check_Table",
                        new DataView(mMPScreenMain.SourceData.Tables["CrossCheck_ComponentTypeandBasisToSampleAcquisitionMethod"],
                                     "", "",
                                     DataViewRowState.CurrentRows),
                        eParameterDataType.DataView);

      SetCheckParameter("Component_Type_Code_Lookup_Table",
                        new DataView(mMPScreenMain.SourceData.Tables["ComponentTypeCode"], "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Sample_Acquisition_Method_Code_Lookup_Table",
                        new DataView(mMPScreenMain.SourceData.Tables["SAMCode"], "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

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
