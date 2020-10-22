using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.CheckEngine.Definitions;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;


namespace ECMPS.Checks.MonitorPlan
{

  public class cMpChecks : cChecks
  {

    #region Public Constructors

    /// <summary>
    /// Public Constructors
    /// </summary>
    /// <param name="mpScreenProcess">The parent generation process object for the generation checks.</param>
    public cMpChecks(cMpProcess mpProcess)
      : base()
    {
      MpProcess = mpProcess;
      MpManualParameters = mpProcess.MpManualParameters;
    }

    #endregion


    #region Protected Constructors

    /// <summary>
    /// Constructor used for testing.
    /// </summary>
    protected cMpChecks()
    {
    }

    #endregion


    #region Public Properties: General

    /// <summary>
    /// The check parameters object that implements the check parameters for this process.
    /// </summary>
    public cMpCheckParameters MpManualParameters { get; protected set; }

    /// <summary>
    /// The MP Report Process associated with this category.
    /// </summary>
    public cMpProcess MpProcess { get; protected set; }

    #endregion


    #region Public Properties: Parameters

    protected cCheckParameterDateValue AttributeEvaluationBeginDate { get { return MpManualParameters.AttributeEvaluationBeginDate; } }
    protected cCheckParameterDateValue AttributeEvaluationEndDate { get { return MpManualParameters.AttributeEvaluationEndDate; } }
    protected cCheckParameterDataViewLegacy CrossCheckProgramParameterToLocationType { get { return MpManualParameters.CrossCheckProgramParameterToLocationType; } }
    protected cCheckParameterDataViewLegacy CrossCheckProgramParameterToMethodCode { get { return MpManualParameters.CrossCheckProgramParameterToMethodCode; } }
    protected cCheckParameterDataViewLegacy CrossCheckProgramParameterToMethodParameter { get { return MpManualParameters.CrossCheckProgramParameterToMethodParameter; } }
    protected cCheckParameterDataViewLegacy CrossCheckProgramParameterToSeverity { get { return MpManualParameters.CrossCheckProgramParameterToSeverity; } }
    protected cCheckParameterDataRowViewValue CurrentLocationAttribute { get { return MpManualParameters.CurrentLocationAttribute; } }
    protected cCheckParameterDataRowViewValue CurrentMethod { get { return MpManualParameters.CurrentMethod; } }
    protected cCheckParameterDataRowViewValue CurrentProgram { get { return MpManualParameters.CurrentProgram; } }
    protected cCheckParameterBooleanValue CurrentProgramActive { get { return MpManualParameters.CurrentProgramActive; } }
    protected cCheckParameterDataRowViewValue CurrentProgramParameter { get { return MpManualParameters.CurrentProgramParameter; } }
    protected cCheckParameterBooleanValue CurrentProgramParameterActive { get { return MpManualParameters.CurrentProgramParameterActive; } }
    protected cCheckParameterDataViewLegacy FacilityMethodRecords { get { return MpManualParameters.FacilityMethodRecords; } }
    protected cCheckParameterDataViewLegacy LocationProgramParameterRecords { get { return MpManualParameters.LocationProgramParameterRecords; } }
    protected cCheckParameterBooleanValue LocationActive { get { return MpManualParameters.LocationActive; } }
    protected cCheckParameterBooleanValue LocationDatesConsistent { get { return MpManualParameters.LocationDatesConsistent; } }
    protected cCheckParameterDataViewLegacy LocationProgramRecords { get { return MpManualParameters.LocationProgramRecords; } }
    protected cCheckParameterDataViewLegacy LocationReportingFrequencyRecords { get { return MpManualParameters.LocationReportingFrequencyRecords; } }
    protected cCheckParameterStringValue LocationType { get { return MpManualParameters.LocationType; } }
    protected cCheckParameterDataViewLegacy LocationUnitTypeRecords { get { return MpManualParameters.LocationUnitTypeRecords; } }
    protected cCheckParameterBooleanValue MethodDatesAndHoursConsistent { get { return MpManualParameters.MethodDatesAndHoursConsistent; } }
    protected cCheckParameterDateValue MethodEvaluationBeginDate { get { return MpManualParameters.MethodEvaluationBeginDate; } }
    protected cCheckParameterDateValue MethodEvaluationEndDate { get { return MpManualParameters.MethodEvaluationEndDate; } }
    protected cCheckParameterDataViewLegacy MethodRecords { get { return MpManualParameters.MethodRecords; } }
    protected cCheckParameterDateValue ProgramEvaluationBeginDate { get { return MpManualParameters.ProgramEvaluationBeginDate; } }
    protected cCheckParameterDateValue ProgramEvaluationEndDate { get { return MpManualParameters.ProgramEvaluationEndDate; } }
    protected cCheckParameterStringValue ProgramMethodParameterDescription { get { return MpManualParameters.ProgramMethodParameterDescription; } }
    protected cCheckParameterDateValue ProgramParameterEvaluationBeginDate { get { return MpManualParameters.ProgramParameterEvaluationBeginDate; } }
    protected cCheckParameterDateValue ProgramParameterEvaluationEndDate { get { return MpManualParameters.ProgramParameterEvaluationEndDate; } }
    protected cCheckParameterBooleanValue StackInformationRequired { get { return MpManualParameters.StackInformationRequired; } }
    protected cCheckParameterDataViewLegacy UnitStackConfigurationRecords { get { return MpManualParameters.UnitStackConfigurationRecords; } }

    #endregion

  }

}
