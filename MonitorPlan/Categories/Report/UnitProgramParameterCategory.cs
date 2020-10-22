using System;
using System.Collections;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Definitions.Extensions;
using ECMPS.ErrorSuppression;


namespace ECMPS.Checks.MonitorPlanEvaluation
{

  /// <summary>
  /// The Unit Program Parameter Category (PRGPRAM) class.
  /// </summary>
  public class cUnitProgramParameterCategory : cCategory
  {

    #region Public Constructors

    /// <summary>
    /// Creates a Program Parameter Category object.
    /// </summary>
    /// <param name="unitProgramCategory">The parent Unit Program Category object.</param>
    public cUnitProgramParameterCategory(cUnitProgram unitProgramCategory)
      : base(unitProgramCategory.CheckEngine, unitProgramCategory.Process, "PRGPRAM")
    {
      UnitProgramCategory = unitProgramCategory;
      MonitorPlanProcess = (cMonitorPlan)unitProgramCategory.Process;
      TableName = "UNIT_PROGRAM_PARAMETER";
    }

    #endregion


    #region Public Static Methods

    /// <summary>
    /// Returns an initialized Unit Program Parameter Category
    /// </summary>
    /// <param name="unitProgramCategory">The parent Unit Program Category object.</param>
    /// <returns></returns>
    public static cUnitProgramParameterCategory GetInitialized(cUnitProgram unitProgramCategory)
    {
      cUnitProgramParameterCategory result;

      string ErrorMessage = "";

      try
      {
        result = new cUnitProgramParameterCategory(unitProgramCategory);

        bool Result = result.InitCheckBands(result.CheckEngine.DbAuxConnection, ref ErrorMessage);

        if (!Result)
        {
          result = null;
          System.Diagnostics.Debug.WriteLine(string.Format("{0}: {1}", Label, ErrorMessage));
        }
      }
      catch (Exception ex)
      {
        result = null;
        System.Diagnostics.Debug.WriteLine(string.Format("{0}: {1}", Label, ex.Message));
      }

      return result;
    }

    #endregion


    #region Public Methods

    /// <summary>
    /// The Process Checks method for the Unit Program Parameter Category class.
    /// </summary>
    /// <param name="monLocId"></param>
    /// <param name="prgParamId"></param>
    /// <returns></returns>
    public bool ProcessChecks(string monLocId, long? prgParamId)
    {
      bool result;

      PrgParamId = prgParamId;
      CurrentRowId = prgParamId.AsString();

      result = base.ProcessChecks(monLocId);

      System.Diagnostics.Debug.WriteLine(string.Format("{0}: {1}", Label, RecordIdentifier));

      return result;
    }

    #endregion


    #region Private Constants

    /// <summary>
    /// The label used when display information for the category.
    /// </summary>
    private const string Label = "Unit Program Parameter";

    #endregion


    #region Public Properties

    #region Initialization 

    /// <summary>
    /// The Monitor Plan Process object tow which the category belongs.
    /// </summary>
    public cMonitorPlan MonitorPlanProcess { get; private set; }

    /// <summary>
    /// The parent Unit Program Category.
    /// </summary>
    public cUnitProgram UnitProgramCategory { get; private set; }

    #endregion


    #region Process Checks

    /// <summary>
    /// The current PRG_PARAM_ID being processed.
    /// </summary>
    public long? PrgParamId { get; private set; }

    #endregion

    #endregion


    #region Override Protected Methods

    /// <summary>
    /// Sets filtered data parameters for the category or child categories.
    /// </summary>
    override protected void FilterData()
    {
      SetDataRowCheckParameter("Current_Program_Parameter", 
                               MonitorPlanProcess.SourceData.Tables["UnitProgramParameter"], 
                               string.Format("MON_LOC_ID = '{0}' and PRG_PARAM_ID = {1}", CurrentMonLocId, PrgParamId), 
                               null);
    }


    /// <summary>
    /// Sets the RecordIndentifier value for the current Process Checks.
    /// </summary>
    override protected void SetRecordIdentifier()
    {
      DataRowView currentProgramParameter = GetCheckParameter("Current_Program_Parameter").ValueAsDataRowView();

      if (currentProgramParameter != null)
      {
        RecordIdentifier = string.Format("{0}, unit '{1}' for {2}-{3} beginning {4} ({5})",
                                         currentProgramParameter["ORIS_CODE"],
                                         currentProgramParameter["UNIT_NAME"],
                                         currentProgramParameter["PRG_CD"],
                                         currentProgramParameter["PARAMETER_CD"],
                                         currentProgramParameter["PARAM_BEGIN_DATE"].AsDateTime(DateTime.MinValue).ToShortDateString(),
                                         currentProgramParameter["PRG_PARAM_ID"]);
      }
      else
      {
        // This should never happen
        RecordIdentifier = "Missing Program Parameter";
      }
    }


    /// <summary>
    /// Sets the Error Suppression values for the current Process Checks.
    /// </summary>
    /// <returns></returns>
    override protected bool SetErrorSuppressValues()
    {
      DataRowView currentProgramParameter = GetCheckParameter("Current_Program_Parameter").ValueAsDataRowView();

      if (currentProgramParameter != null)
      {
        long facId = currentProgramParameter["FAC_ID"].AsLong(0);
        string locationName = currentProgramParameter["UNIT_NAME"].AsString();
        string matchDataValue = currentProgramParameter["PARAMETER_CD"].AsString();
        DateTime? matchTimeValue = currentProgramParameter["PARAM_END_DATE"].AsDateTime();

        ErrorSuppressValues = new cErrorSuppressValues(facId, locationName, "PARAM", matchDataValue, "HISTIND", matchTimeValue);
        return true;
      }
      else
        return false;
    }

    #endregion

  }

}
