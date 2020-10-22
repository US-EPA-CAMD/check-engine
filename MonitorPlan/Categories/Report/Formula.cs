using System;
using System.Data;
using System.Collections;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Extensions;
using ECMPS.ErrorSuppression;


namespace ECMPS.Checks.MonitorPlanEvaluation
{
  public class cFormula : cCategory
  {

    #region Private Constants

    private const string Label = "Formula";

    #endregion


    #region Private Fields

    private string mFormulaID;
    private cMonitorPlan mMonitorPlan;

    #endregion


    #region Constructors

    public cFormula(cCheckEngine CheckEngine, cMonitorPlan MonitorPlan)
      : base(CheckEngine, (cProcess)MonitorPlan, "FORMULA")
    {
      mMonitorPlan = MonitorPlan;
      TableName = "FORMULA";
    }

    #endregion


    #region Public Static Methods

    public static cFormula GetInitialized(cCheckEngine ACheckEngine, cMonitorPlan AMonitorPlanProcess)
    {
      cFormula Category;
      string ErrorMessage = "";

      try
      {
        Category = new cFormula(ACheckEngine, AMonitorPlanProcess);

        bool Result = Category.InitCheckBands(ACheckEngine.DbAuxConnection, ref ErrorMessage);

        if (!Result)
        {
          Category = null;
          System.Diagnostics.Debug.WriteLine(string.Format("{0}: {1}", Label, ErrorMessage));
        }
      }
      catch (Exception ex)
      {
        Category = null;
        System.Diagnostics.Debug.WriteLine(string.Format("{0}: {1}", Label, ex.Message));
      }

      return Category;
    }

    #endregion


    #region Public Methods

    public new bool ProcessChecks(string FormulaID, string MonitorLocationID)
    {
      mFormulaID = FormulaID;
      CurrentRowId = mFormulaID;

      System.Diagnostics.Debug.WriteLine(string.Format("{0}: {1}", Label, CurrentRowId));

      return base.ProcessChecks(MonitorLocationID);
    }

    #endregion


    #region Base Class Overrides

    protected override void FilterData()
    {
      string MonLocFilter = "Mon_Loc_Id = '" + CurrentMonLocId + "'";

      //filter MonitorPlanComponent to find this one based on the mComponentID
      SetCheckParameter("Current_formula",
                        new DataView(mMonitorPlan.SourceData.Tables["MonitorFormula"],
                                     "mon_form_id = '" + mFormulaID + "'", "mon_form_id",
                                     DataViewRowState.CurrentRows)[0],
                        eParameterDataType.DataRowView);

      //get FormulaCode
      SetCheckParameter("Formula_Code_lookup_table",
                        new DataView(mMonitorPlan.SourceData.Tables["FormulaCode"],
                                     "", "",
                                     DataViewRowState.CurrentRows),
                        eParameterDataType.DataView);

      //get FormulaCodeToF-FactorParameterCrossCheckTable
      SetCheckParameter("Formula_Code_To_F-Factor_Parameter_Cross_Check_Table",
                        new DataView(mMonitorPlan.SourceData.Tables["CrossCheck_FormulaCodeToF-FactorParameter"],
                                     "", "",
                                     DataViewRowState.CurrentRows),
                        eParameterDataType.DataView);

      //get FormulaParameterAndComponentTypeAndBasisToFormulaCodeCrossCheckTable
      SetCheckParameter("Formula_Parameter_And_Component_Type_And_Basis_To_Formula_Code_Cross_Check_Table",
                        new DataView(mMonitorPlan.SourceData.Tables["CrossCheck_FormulaParameterAndComponentTypeAndBasisToFormulaCode"],
                                     "", "",
                                     DataViewRowState.CurrentRows),
                        eParameterDataType.DataView);

      //get FormulaParameterCrossCheckTable
      SetCheckParameter("Formula_Parameter_List",
                        new DataView(mMonitorPlan.SourceData.Tables["CrossCheck_ParameterToCategory"],
                                     "CategoryCode = 'FORMULA'", "",
                                     DataViewRowState.CurrentRows),
                        eParameterDataType.DataView);

    }


    protected override void SetRecordIdentifier()
    {
      DataRowView CurrentFormula = (DataRowView)GetCheckParameter("Current_Formula").ParameterValue;

      RecordIdentifier = "Formula ID " + (string)CurrentFormula["formula_identifier"];
    }

    protected override bool SetErrorSuppressValues()
    {
      DataRowView currentLocation = GetCheckParameter("Current_Location").ValueAsDataRowView();
      DataRowView currentFormula = GetCheckParameter("Current_Formula").ValueAsDataRowView();

      if (currentFormula != null)
      {
        long facId = CheckEngine.FacilityID;
        string locationName = currentLocation["LOCATION_IDENTIFIER"].AsString();
        string matchDataValue = currentFormula["PARAMETER_CD"].AsString();
        DateTime? matchTimeValue = currentFormula["END_DATE"].AsDateTime();

        ErrorSuppressValues = new cErrorSuppressValues(facId, locationName, "PARAM", matchDataValue, "HISTIND", matchTimeValue);
        return true;
      }
      else
        return false;
    }

    #endregion

  }
}
