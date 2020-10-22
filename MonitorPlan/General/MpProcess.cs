using System;
using System.Data;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.DatabaseAccess;
using ECMPS.Checks.Parameters;


namespace ECMPS.Checks.MonitorPlan
{

  abstract public class cMpProcess : cProcess
  {

    #region Constructors

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="processCd">Process code for the QA process.</param>
    /// <param name="checkEngine">Check engine running the process.</param>
    public cMpProcess(cCheckEngine checkEngine)
      : base(checkEngine, checkEngine.ProcessCd)
    {
    }

    #endregion


    #region Base Class Overrides

    /// <summary>
    /// Initializes the Check Parameters object to a Default Check Parameters instance.  The default
    /// does not implement any parameters as properties and processes that do should override this
    /// method and set the Check Parameters object to and instance that implements parameters as
    /// properties.
    /// </summary>
    protected override void InitCheckParameters()
    {
      ProcessParameters = new cMpCheckParameters(this, mCheckEngine.DbAuxConnection);
    }

    #endregion


    #region Public Properties: Parameters

    /// <summary>
    /// The check parameters object that implements the check parameters for this process.
    /// </summary>
    public cMpCheckParameters MpManualParameters { get { return (cMpCheckParameters)ProcessParameters; } }


    /// <summary>
    /// Cross Check: Program Parameter to Location Type parameter.
    /// </summary>
    public cCheckParameterDataViewLegacy CrossCheckProgramParameterToLocationType { get { return MpManualParameters.CrossCheckProgramParameterToLocationType; } }

    /// <summary>
    /// Cross Check: Program Parameter to Method Code parameter.
    /// </summary>
    public cCheckParameterDataViewLegacy CrossCheckProgramParameterToMethodCode { get { return MpManualParameters.CrossCheckProgramParameterToMethodCode; } }

    /// <summary>
    /// Cross Check: Program Parameter to Method Parameter parameter.
    /// </summary>
    public cCheckParameterDataViewLegacy CrossCheckProgramParameterToMethodParameter { get { return MpManualParameters.CrossCheckProgramParameterToMethodParameter; } }

    /// <summary>
    /// Cross Check: Program Parameter to Method Parameter parameter.
    /// </summary>
    public cCheckParameterDataViewLegacy CrossCheckProgramParameterToSeverity { get { return MpManualParameters.CrossCheckProgramParameterToSeverity; } }

    /// <summary>
    /// Current Program parameter.
    /// </summary>
    public cCheckParameterDataRowViewValue CurrentProgram { get { return MpManualParameters.CurrentProgram; } }

    /// <summary>
    /// Current Program Active parameter.
    /// </summary>
    public cCheckParameterBooleanValue CurrentProgramActive { get { return MpManualParameters.CurrentProgramActive; } }

    /// <summary>
    /// Current Program Parameter parameter.
    /// </summary>
    public cCheckParameterDataRowViewValue CurrentProgramParameter { get { return MpManualParameters.CurrentProgramParameter; } }

    /// <summary>
    /// Current Program Parameter Active parameter.
    /// </summary>
    public cCheckParameterBooleanValue CurrentProgramParameterActive { get { return MpManualParameters.CurrentProgramParameterActive; } }

    /// <summary>
    /// Facility Method Records parameter.
    /// </summary>
    public cCheckParameterDataViewLegacy FacilityMethodRecords { get { return MpManualParameters.FacilityMethodRecords; } }

    /// <summary>
    /// Program Evaluation Begin Date parameter.
    /// </summary>
    public cCheckParameterDateValue ProgramEvaluationBeginDate { get { return MpManualParameters.ProgramEvaluationBeginDate; } }

    /// <summary>
    /// Program Evaluation End Date parameter.
    /// </summary>
    public cCheckParameterDateValue ProgramEvaluationEndDate { get { return MpManualParameters.ProgramEvaluationEndDate; } }

    /// <summary>
    /// Program Method Parameter Description parameter.
    /// </summary>
    public cCheckParameterStringValue ProgramMethodParameterDescription { get { return MpManualParameters.ProgramMethodParameterDescription; } }

    /// <summary>
    /// Program Parameter Evaluation Begin Date parameter.
    /// </summary>
    public cCheckParameterDateValue ProgramParameterEvaluationBeginDate { get { return MpManualParameters.ProgramParameterEvaluationBeginDate; } }

    /// <summary>
    /// Program Parameter Evaluation End Date parameter.
    /// </summary>
    public cCheckParameterDateValue ProgramParameterEvaluationEndDate { get { return MpManualParameters.ProgramParameterEvaluationEndDate; } }

    /// <summary>
    /// Unit Stack Configuration Records parameter.
    /// </summary>
    public cCheckParameterDataViewLegacy UnitStackConfigurationRecords { get { return MpManualParameters.UnitStackConfigurationRecords; } }

    #endregion

  }

}
