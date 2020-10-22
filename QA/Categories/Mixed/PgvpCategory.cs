using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.QA;
using ECMPS.Checks.QAScreenEvaluation;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Enumerations;
using ECMPS.Definitions.Extensions;

using ECMPS.ErrorSuppression;



namespace  ECMPS.Checks.QA
{
  public class cPgvpCategory : cCategory
  {

    #region Constructors

    /// <summary>
    /// Creates an PGVP category with the given parent category and code for the data view row.
    /// </summary>
    /// <param name="parentCategory">Parent category of the new category.</param>
    /// <param name="categoryCd">Category code of the new category.</param>
    /// <param name="pgvpRow">PGVP row to evaluate.</param>
    public cPgvpCategory(cCategory parentCategory, string categoryCd, DataRowView pgvpRow)
      : base(parentCategory.Process.CheckEngine, parentCategory.Process, parentCategory, categoryCd)
    {
      Initialize((cQaProcess)parentCategory.Process, pgvpRow);
    }


    /// <summary>
    /// Creates an PGVP category with the given parent category and code for the first row in the data table.
    /// </summary>
    /// <param name="qaScreenProcess">Process of the new category.</param>
    /// <param name="categoryCd">Category code of the new category.</param>
    /// <param name="pgvpSingleRowTable">The table containing the PGVP row to evaluate.</param>
    public cPgvpCategory(cQAScreenMain qaScreenProcess, string categoryCd, DataTable pgvpSingleRowTable)
      : base(qaScreenProcess.CheckEngine, qaScreenProcess, categoryCd)
    {
      IsScreenProcess = true;

      DataRowView pgvpRow;
      {
        if ((pgvpSingleRowTable != null) && (pgvpSingleRowTable.DefaultView.Count > 0))
          pgvpRow = pgvpSingleRowTable.DefaultView[0];
        else
          pgvpRow = null;
      }

      qaScreenProcess.CrossCheckProtocolGasParameterToType.LegacySetValue(Process.SourceData.Tables["CrossCheck_ProtocolGasParameterToType"].DefaultView, this);
      qaScreenProcess.GasComponentCodeLookupTable.LegacySetValue(Process.SourceData.Tables["GasComponentCode"].DefaultView, this);
      qaScreenProcess.GasTypeCodeLookupTable.LegacySetValue(new DataView(qaScreenProcess.SourceData.Tables["GasTypeCode"]));
      qaScreenProcess.ProtocolGasVendorLookupTable.LegacySetValue(new DataView(qaScreenProcess.SourceData.Tables["ProtocolGasVendor"]));
      qaScreenProcess.SystemParameterLookupTable.LegacySetValue(new DataView(qaScreenProcess.SourceData.Tables["SystemParameter"]));

      qaScreenProcess.CurrentTest.SetValue(new DataView(qaScreenProcess.SourceData.Tables["TestSummary"],
                                                        "TEST_SUM_ID = '" + qaScreenProcess.CheckEngine.TestSumId + "'",
                                                        "",
                                                        DataViewRowState.CurrentRows)[0]);
      
      Initialize(qaScreenProcess, pgvpRow);
    }

    /// <summary>
    /// Direct Check Tester constructor.
    /// </summary>
    /// <param name="qaScreenProcess">Process of the new category.</param>
    /// <param name="categoryCd">Category code of the new category.</param>
    /// <param name="pgvpSingleRowTable">The table containing the PGVP row to evaluate.</param>
    public cPgvpCategory(cQAScreenMain qaScreenProcess, string categoryCd)
      : base(qaScreenProcess.CheckEngine, qaScreenProcess, categoryCd)
    {
      Initialize(qaScreenProcess, null);
    }

    #region Helper Methods

    private void Initialize(cQaProcess qaProcess, DataRowView pgvpRow)
    {
      QaProcess = (cQaProcess)qaProcess;

      TableName = "PROTOCOL_GAS";

      // Set Current AETB and CurrentRowId
      if ((pgvpRow != null) && (pgvpRow.Row != null) && (pgvpRow.Row.Table != null) && pgvpRow.Row.Table.Columns.Contains("PROTOCOL_GAS_ID"))
      {
        QaProcess.CurrentProtocolGasRecord.SetValue(pgvpRow, this);
        CurrentRowId = pgvpRow["PROTOCOL_GAS_ID"].AsString();
      }
      else
      {
        QaProcess.CurrentProtocolGasRecord.SetValue(this);
        CurrentRowId = null;
      }

      // Set Mon Loc and Test Sum Information
      if (QaProcess.CurrentTest.Value.IsNotNull())
      {
        CurrentMonLocId = QaProcess.CurrentTest.Value["MON_LOC_ID"].AsString();
        CurrentMonLocName = QaProcess.CurrentTest.Value["LOCATION_IDENTIFIER"].AsString();
        CurrentTestSumId = QaProcess.CurrentTest.Value["TEST_SUM_ID"].AsString();
      }
      else
      {
        CurrentMonLocId = null;
        CurrentMonLocName = null;
        CurrentTestSumId = null;
      }

      // Standard Calls
      FilterData();
      SetRecordIdentifier();
    }

    #endregion

    #endregion


    #region Private Fields

    private bool IsScreenProcess = false;
    private cQaProcess QaProcess;

    #endregion


    #region Public Methods

    public new bool ProcessChecks()
    {
      return base.ProcessChecks();
    }


    /// <summary>
    /// Sets the check bands for this category to the passed check bands and then executes
    /// those checks.
    /// </summary>
    /// <param name="checkBands">The check bands to process.</param>
    /// <returns>True if the processing of check executed normally.</returns>
    public bool ProcessChecks(cCheckParameterBands checkBands)
    {
      this.SetCheckBands(checkBands);
      return base.ProcessChecks();
    }

    #endregion


    #region Base Class Overrides

    protected override void FilterData()
    {
    }


    protected override void SetRecordIdentifier()
    {
      if (!IsScreenProcess)
      {
        if (QaProcess.CurrentProtocolGasRecord.Value.IsNotNull())
        {
          string cylinderId = QaProcess.CurrentProtocolGasRecord.Value["CYLINDER_ID"].AsString();

          if (cylinderId != null)
          {
            RecordIdentifier = "Cylinder Identifier " + cylinderId;
          }
          else
          {
            RecordIdentifier = "Gas Level " + QaProcess.CurrentProtocolGasRecord.Value["GAS_LEVEL_CD"].AsString();
            RecordIdentifier += " Gas Type " + QaProcess.CurrentProtocolGasRecord.Value["GAS_TYPE_CD"].AsString();
          }
        }
        else
          RecordIdentifier = null;
      }
      else
        RecordIdentifier = "this record";
    }


    protected override bool SetErrorSuppressValues()
    {
      DataRowView currentTest = GetCheckParameter("Current_Test").AsDataRowView();

      if (currentTest != null)
      {
        long facId = CheckEngine.FacilityID;
        string locationName = currentTest["LOCATION_IDENTIFIER"].AsString();
        string matchDataValue = currentTest["TEST_NUM"].AsString();
        DateTime? matchTimeValue = currentTest["BEGIN_DATE"].AsDateTime();

        ErrorSuppressValues = new cErrorSuppressValues(facId, locationName, "TESTNUM", matchDataValue, "DATE", matchTimeValue);
        return true;
      }
      else
      {
        ErrorSuppressValues = null;
        return false;
      }
    }

    #endregion

  }
}
