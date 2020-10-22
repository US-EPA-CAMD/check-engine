using System;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.Data.Ecmps.Lookup.Table;
using ECMPS.Checks.EmGeneration.Parameters;


namespace ECMPS.Checks.NonOperatingEmissionGeneration
{

  /// <summary>
  /// The category class for all generation category objects.
  /// </summary>
  public class cGenerationCategory : cCategory
  {
    
    #region Public Constructors

    /// <summary>
    /// Creates a generation category with a parent generation category.
    /// </summary>
    /// <param name="parentGenerationCategory">The parent generation category for the new category.</param>
    /// <param name="categoryCd">The category code of the new category.</param>
    public cGenerationCategory(cGenerationProcess generationProcess,
                               string categoryCd)
      : base(generationProcess.CheckEngine,
             generationProcess,
             categoryCd)
    {
      GenerationProcess = generationProcess;
    }

    /// <summary>
    /// Creates a generation category with a parent generation category.
    /// </summary>
    /// <param name="parentGenerationCategory">The parent generation category for the new category.</param>
    /// <param name="categoryCd">The category code of the new category.</param>
    public cGenerationCategory(cGenerationCategory parentGenerationCategory,
                               string categoryCd)
      : base(parentGenerationCategory.CheckEngine,
             parentGenerationCategory.GenerationProcess,
             (cCategory)parentGenerationCategory,
             categoryCd)
    {
      GenerationProcess = parentGenerationCategory.GenerationProcess;
    }

    #endregion


    #region Public Properties

    /// <summary>
    /// The Generation Check Parameters for this category.
    /// </summary>
    public cGenerationParameters GenerationParameters { get { return GenerationProcess.GenerationParameters; } }

    /// <summary>
    /// THe Generation Process object for the category
    /// </summary>
    public cGenerationProcess GenerationProcess { get; private set; }

    #endregion


    #region Public Methods: Process Checks

    /// <summary>
    /// Processes checks for a monitor location specific category.
    /// </summary>
    /// <param name="monLocId">MON_LOC_ID of the monitor location record being processed.</param>
    /// <param name="monLocPos">View position of the monitor location record being processed.</param>
    /// <param name="monLocName">Location name of the monitor location record begin processed.</param>
    /// <returns>Returns false if the check run fails.</returns>
    public virtual bool ProcessChecks(string monLocId, int monLocPos, string monLocName)
    {
      try
      {
        CurrentMonLocId = monLocId;
        CurrentMonLocPos = monLocPos;
        CurrentMonLocName = monLocName;

        CurrentRowId = monLocId;

        return ProcessChecksDo_WithPrep();
      }
      catch (Exception ex)
      {
        Process.UpdateErrors(string.Format("Category: {0}  MonLocId: {1}  Message: {2}",
                                           this.CategoryCd,
                                           monLocId,
                                           ex.Message));
        return false;
      }
    }

    /// <summary>
    /// Processes checks for a monitor location and hour specific category.
    /// </summary>
    /// <param name="monLocId">MON_LOC_ID of the monitor location record being processed.</param>
    /// <param name="monLocPos">View position of the monitor location record being processed.</param>
    /// <param name="monLocName">Location name of the monitor location record begin processed.</param>
    /// <param name="opDate">Op date of the current hour being processed.</param>
    /// <param name="opHour">Op hour of the current hour being processed.</param>
    /// <returns>Returns false if the check run fails.</returns>
    public virtual bool ProcessChecks(string monLocId, int monLocPos, string monLocName, DateTime opDate, int opHour)
    {
      try
      {
        CurrentMonLocId = monLocId;
        CurrentMonLocPos = monLocPos;
        CurrentMonLocName = monLocName;
        CurrentOpDate = opDate;
        CurrentOpHour = opHour;

        CurrentRowId = monLocId;

        return ProcessChecksDo_WithPrep();
      }
      catch (Exception ex)
      {
        Process.UpdateErrors(string.Format("Category: {0}  MonLocId: {1}  OpDate: {2}  OpHour: {3}  Message: {4}",
                                           this.CategoryCd,
                                           monLocId,
                                           opDate.ToShortDateString(),
                                           opHour.ToString(),
                                           ex.Message));
        return false;
      }
    }

    #endregion


    #region Base Class Overrides

    /// <summary>
    /// Filter data for the check cycle.
    /// </summary>
    protected override void FilterData()
    {
            EmGenerationParameters.ProgramCodeTable = new CheckDataView<ProgramCodeRow>(new System.Data.DataView(SourceTable("ProgramCode"), "", "Prg_Cd", System.Data.DataViewRowState.CurrentRows));
    }

    /// <summary>
    /// Sets the Record Identifier for the check cycle.
    /// </summary>
    protected override void SetRecordIdentifier()
    {
    }

    /// <summary>
    /// Sets the error suppression values for the check cycle.
    /// </summary>
    /// <returns></returns>
    protected override bool SetErrorSuppressValues()
    {
      return true;
    }

    #endregion

  }

}
