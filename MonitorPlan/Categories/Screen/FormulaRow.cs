using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;

using ECMPS.ErrorSuppression;

namespace ECMPS.Checks.MPScreenEvaluation
{
  public class cFormulaRow : cCategory
  {

    #region Private Fields

    private cMPScreenMain mMPScreenMain;
    private DataTable mFormulaTable;

    #endregion


    #region Constructors

    public cFormulaRow(cCheckEngine CheckEngine, cMPScreenMain MPScreenMain, string MonitorLocationId, DataTable FormulaTable)
      : base(CheckEngine, (cProcess)MPScreenMain, "SCRFORM", FormulaTable)
    {
      InitializeCurrent(MonitorLocationId);

      mMPScreenMain = MPScreenMain;
      mFormulaTable = FormulaTable;

      FilterData();

      SetRecordIdentifier();
    }

      public cFormulaRow(cCheckEngine CheckEngine, cMPScreenMain MPScreenMain)
        : base(CheckEngine, (cProcess)MPScreenMain, "SCRFORM")
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
      
      SetCheckParameter("Current_Formula", new DataView(mFormulaTable,
        "", "", DataViewRowState.CurrentRows)[0], eParameterDataType.DataRowView);

      SetCheckParameter("Formula_Records", new DataView(mMPScreenMain.SourceData.Tables["MPFormula"],
        "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Used_Identifier_Records", new DataView(mMPScreenMain.SourceData.Tables["UsedIdentifier"],
        "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

      SetCheckParameter("Formula_Parameter_List", 
                        new DataView(mMPScreenMain.SourceData.Tables["CrossCheck_ParameterToCategory"],
                                     "CategoryCode = 'FORMULA'", "ParameterCode", 
                                     DataViewRowState.CurrentRows), 
                        eParameterDataType.DataView);	

      SetCheckParameter("Formula_Parameter_And_Component_Type_And_Basis_To_Formula_Code_Cross_Check_Table",
                        new DataView(mMPScreenMain.SourceData.Tables["CrossCheck_FormulaParameterandComponentTypeandBasisToFormulaCode"],
                                     "", "",
                                     DataViewRowState.CurrentRows),
                        eParameterDataType.DataView);

      SetCheckParameter("Formula_Code_Lookup_Table",
                        new DataView(mMPScreenMain.SourceData.Tables["FormulaCode"], "", "", DataViewRowState.CurrentRows), eParameterDataType.DataView);

 
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
