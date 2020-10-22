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


namespace ECMPS.Checks.QA
{
  public class cAetbCategory : cCategory
  {

    #region Constructors

    /// <summary>
    /// Creates an AETB category with the given parent category and code for the data view row.
    /// </summary>
    /// <param name="parentCategory">Parent category of the new category.</param>
    /// <param name="categoryCd">Category code of the new category.</param>
    /// <param name="aetbRow">AETB row to evaluate.</param>
    public cAetbCategory(cCategory parentCategory, string categoryCd, DataRowView aetbRow)
      : base(parentCategory.Process.CheckEngine, parentCategory.Process, parentCategory, categoryCd)
    {
      Initialize((cQaProcess)parentCategory.Process, aetbRow);
    }


    /// <summary>
    /// Creates an AETB category with the given parent category and code for the first row in the data table.
    /// </summary>
    /// <param name="qaScreenProcess">Process of the new category.</param>
    /// <param name="categoryCd">Category code of the new category.</param>
    /// <param name="aetbSingleRowTable">The table containing the AETB row to evaluate.</param>
    public cAetbCategory(cQAScreenMain qaScreenProcess, string categoryCd, DataTable aetbSingleRowTable)
      : base(qaScreenProcess.CheckEngine, qaScreenProcess, categoryCd)
    {
      IsScreenProcess = true;

      DataRowView aetbRow;
      {
        if ((aetbSingleRowTable != null) && (aetbSingleRowTable.DefaultView.Count > 0))
          aetbRow = aetbSingleRowTable.DefaultView[0];
        else
          aetbRow = null;
      }

      qaScreenProcess.CurrentTest.SetValue(new DataView(qaScreenProcess.SourceData.Tables["TestSummary"],
                                                        "TEST_SUM_ID = '" + qaScreenProcess.CheckEngine.TestSumId + "'",
                                                        "",
                                                        DataViewRowState.CurrentRows)[0]);

      Initialize(qaScreenProcess, aetbRow);
    }

    #region Helper Methods

    private void Initialize(cQaProcess qaProcess, DataRowView aetbRow)
    {
      QaProcess = (cQaProcess)qaProcess;

      TableName = "AIR_EMISSION_TESTING";

      // Set Current AETB and CurrentRowId
      if ((aetbRow != null) && (aetbRow.Row != null) && (aetbRow.Row.Table != null) && aetbRow.Row.Table.Columns.Contains("AIR_EMISSION_TEST_ID"))
      {
        QaProcess.CurrentAirEmissionTestingRecord.SetValue(aetbRow, this);
        CurrentRowId = aetbRow["AIR_EMISSION_TEST_ID"].AsString();
      }
      else
      {
        QaProcess.CurrentAirEmissionTestingRecord.SetValue(this);
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
        string name;

        if (QaProcess.CurrentAirEmissionTestingRecord.Value.IsNotNull())
        {
          string nameLast = QaProcess.CurrentAirEmissionTestingRecord.Value["QI_LAST_NAME"].AsString();
          string nameFirst = QaProcess.CurrentAirEmissionTestingRecord.Value["QI_FIRST_NAME"].AsString();
          string nameMiddle = QaProcess.CurrentAirEmissionTestingRecord.Value["QI_MIDDLE_INITIAL"].AsString();

          name = nameFirst;
          if (nameMiddle != null) if (name != null) name += " " + nameMiddle + "."; else name = nameMiddle + ".";
          if (nameLast != null) if (name != null) name += " " + nameLast; else name = nameLast;
        }
        else
          name = null;

        RecordIdentifier = name;
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
