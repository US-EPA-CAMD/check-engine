using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECMPS.Checks.Parameters;
using ECMPS.Checks.DatabaseAccess;


namespace ECMPS.Checks.MonitorPlan
{

  public class cMpCheckParameters : cCheckParametersCheckEngine
  {

    #region Public Constructor

    /// <summary>
    /// Constructor for MP Check Parameter objects.
    /// </summary>
    /// <param name="checkProcess">The check process using the parameters</param>
    /// <param name="databaseAux">The aux database object.</param>
    public cMpCheckParameters(cCheckProcess checkProcess, cDatabase databaseAux)
      : base(checkProcess, databaseAux)
    {
    }


    /// <summary>
    /// Constructor used for testing.
    /// </summary>
    public cMpCheckParameters()
    {
    }

    #endregion


    #region Public Properties: Boolean Parameters

    /// <summary>
    /// Instantiate Check Parameter Properties.
    /// </summary>
    private void InstantiateCheckParameterProperties_Boolean()
    {
      CurrentProgramActive = new cCheckParameterBooleanValue(847, "Current_Program_Active", this);
      CurrentProgramParameterActive = new cCheckParameterBooleanValue(3252, "Current_Program_Parameter_Active", this);
      LocationActive = new cCheckParameterBooleanValue(277, "Location_Active", this);
      LocationDatesConsistent = new cCheckParameterBooleanValue(390, "Location_Dates_Consistent", this);
      MethodDatesAndHoursConsistent = new cCheckParameterBooleanValue(402, "Method_Dates_and_Hours_Consistent", this);
      StackInformationRequired = new cCheckParameterBooleanValue(3304, "Stack_Information_Required", this);
    }


    /// <summary>
    /// Implements check parameter Current_Program_Active (id = 847)
    /// </summary>
    public cCheckParameterBooleanValue CurrentProgramActive { get; private set; }

    /// <summary>
    /// Implements check parameter Current_Program_Parameter_Active (id = 3252)
    /// </summary>
    public cCheckParameterBooleanValue CurrentProgramParameterActive { get; private set; }

    /// <summary>
    /// Implements check parameter Location_Active (id = 277)
    /// </summary>
    public cCheckParameterBooleanValue LocationActive { get; private set; }

    /// <summary>
    /// Implements check parameter Location_Dates_Consistent (id = 390)
    /// </summary>
    public cCheckParameterBooleanValue LocationDatesConsistent { get; private set; }

    /// <summary>
    /// Implements check parameter Method_Dates_and_Hours_Consistent (id = 402)
    /// </summary>
    public cCheckParameterBooleanValue MethodDatesAndHoursConsistent { get; private set; }

    /// <summary>
    /// Implements check parameter Stack_Information_Required (id = 3304)
    /// </summary>
    public cCheckParameterBooleanValue StackInformationRequired { get; private set; }

    #endregion


    #region Public Properties: Data Row View Parameters

    /// <summary>
    /// Instantiate Check Parameter Properties.
    /// </summary>
    private void InstantiateCheckParameterProperties_DataRowView()
    {
      CurrentLocationAttribute = new cCheckParameterDataRowViewValue(264, "Current_Location_Attribute", this);
      CurrentMethod = new cCheckParameterDataRowViewValue(290, "Current_Method", this);
      CurrentProgram = new cCheckParameterDataRowViewValue(307, "Current_Program", this);
      CurrentProgramParameter = new cCheckParameterDataRowViewValue(3251, "Current_Program_Parameter", this);
    }


    /// <summary>
    /// Implements check parameter Current_Location_Attribute (id = 264)
    /// </summary>
    public cCheckParameterDataRowViewValue CurrentLocationAttribute { get; private set; }

    /// <summary>
    /// Implements check parameter Current_Method (id = 290)
    /// </summary>
    public cCheckParameterDataRowViewValue CurrentMethod { get; private set; }

    /// <summary>
    /// Implements check parameter Current_Program (id = 307)
    /// </summary>
    public cCheckParameterDataRowViewValue CurrentProgram { get; private set; }

    /// <summary>
    /// Implements check parameter Current_Program_Parameter (id = 3251)
    /// </summary>
    public cCheckParameterDataRowViewValue CurrentProgramParameter { get; private set; }

    #endregion


    #region Public Properties: Data View Parameters

    /// <summary>
    /// Instantiate Check Parameter Properties.
    /// </summary>
    private void InstantiateCheckParameterProperties_DataView()
    {
      CrossCheckProgramParameterToLocationType = new cCheckParameterDataViewLegacy(3257, "CrossCheck_ProgramParameterToLocationType", this);
      CrossCheckProgramParameterToMethodCode = new cCheckParameterDataViewLegacy(3258, "CrossCheck_ProgramParameterToMethodCode", this);
      CrossCheckProgramParameterToMethodParameter = new cCheckParameterDataViewLegacy(3259, "CrossCheck_ProgramParameterToMethodParameter", this);
      CrossCheckProgramParameterToSeverity = new cCheckParameterDataViewLegacy(3260, "CrossCheck_ProgramParameterToSeverity", this);
      FacilityMethodRecords = new cCheckParameterDataViewLegacy(1022, "Facility_Method_Records", this);
      LocationProgramParameterRecords = new cCheckParameterDataViewLegacy(3256, "Location_Program_Parameter_Records", this);
      LocationProgramRecords = new cCheckParameterDataViewLegacy(301, "Location_Program_Records", this);
      LocationReportingFrequencyRecords = new cCheckParameterDataViewLegacy(859, "Location_Reporting_Frequency_Records", this);
      LocationUnitTypeRecords = new cCheckParameterDataViewLegacy(510, "Location_Unit_Type_Records", this);
      MethodRecords = new cCheckParameterDataViewLegacy(340, "Method_Records", this);
      UnitStackConfigurationRecords = new cCheckParameterDataViewLegacy(384, "Unit_Stack_Configuration_Records", this);
    }


    /// <summary>
    /// Implements check parameter CrossCheck_ProgramParameterToLocationType (id = 3257)
    /// </summary>
    public cCheckParameterDataViewLegacy CrossCheckProgramParameterToLocationType { get; private set; }

    /// <summary>
    /// Implements check parameter CrossCheck_ProgramParameterToMethodCode (id = 3258)
    /// </summary>
    public cCheckParameterDataViewLegacy CrossCheckProgramParameterToMethodCode { get; private set; }

    /// <summary>
    /// Implements check parameter CrossCheck_ProgramParameterToMethodParameter (id = 3259)
    /// </summary>
    public cCheckParameterDataViewLegacy CrossCheckProgramParameterToMethodParameter { get; private set; }

    /// <summary>
    /// Implements check parameter CrossCheck_ProgramParameterToSeverity (id = 3260)
    /// </summary>
    public cCheckParameterDataViewLegacy CrossCheckProgramParameterToSeverity { get; private set; }

    /// <summary>
    /// Implements check parameter Facility_Method_Records (id = 1022)
    /// </summary>
    public cCheckParameterDataViewLegacy FacilityMethodRecords { get; private set; }

    /// <summary>
    /// Implements check parameter Location_Program_Parameter_Records (id = 3256)
    /// </summary>
    public cCheckParameterDataViewLegacy LocationProgramParameterRecords { get; private set; }

    /// <summary>
    /// Implements check parameter Location_Program_Records (id = 301)
    /// </summary>
    public cCheckParameterDataViewLegacy LocationProgramRecords { get; private set; }

    /// <summary>
    /// Implements check parameter Location_Reporting_Frequency_Records (id = 859)
    /// </summary>
    public cCheckParameterDataViewLegacy LocationReportingFrequencyRecords { get; private set; }

    /// <summary>
    /// Implements check parameter Location_Unit_Type_Records (id = 510)
    /// </summary>
    public cCheckParameterDataViewLegacy LocationUnitTypeRecords { get; private set; }

    /// <summary>
    /// Implements check parameter Method_Records (id = 340)
    /// </summary>
    public cCheckParameterDataViewLegacy MethodRecords { get; private set; }

    /// <summary>
    /// Implements check parameter Unit_Stack_Configuration_Records (id = 384)
    /// </summary>
    public cCheckParameterDataViewLegacy UnitStackConfigurationRecords { get; private set; }

    #endregion


    #region Public Properties: DateTime Parameters

    /// <summary>
    /// Instantiate Check Parameter Properties.
    /// </summary>
    private void InstantiateCheckParameterProperties_Date()
    {
      AttributeEvaluationBeginDate = new cCheckParameterDateValue(2344, "Attribute_Evaluation_Begin_Date", this);
      AttributeEvaluationEndDate = new cCheckParameterDateValue(2346, "Attribute_Evaluation_End_Date", this);
      MethodEvaluationBeginDate = new cCheckParameterDateValue(403, "Method_Evaluation_Begin_Date", this);
      MethodEvaluationEndDate = new cCheckParameterDateValue(405, "Method_Evaluation_End_Date", this);
      ProgramEvaluationBeginDate = new cCheckParameterDateValue(848, "Program_Evaluation_Begin_Date", this);
      ProgramEvaluationEndDate = new cCheckParameterDateValue(849, "Program_Evaluation_End_Date", this);
      ProgramParameterEvaluationBeginDate = new cCheckParameterDateValue(3253, "Program_Parameter_Evaluation_Begin_Date", this);
      ProgramParameterEvaluationEndDate = new cCheckParameterDateValue(3254, "Program_Parameter_Evaluation_End_Date", this);
    }


    /// <summary>
    /// Implements check parameter Attribute_Evaluation_Begin_Date (id = 2344)
    /// </summary>
    public cCheckParameterDateValue AttributeEvaluationBeginDate { get; private set; }

    /// <summary>
    /// Implements check parameter Attribute_Evaluation_End_Date (id = 405)
    /// </summary>
    public cCheckParameterDateValue AttributeEvaluationEndDate { get; private set; }

    /// <summary>
    /// Implements check parameter Method_Evaluation_Begin_Date (id = 403)
    /// </summary>
    public cCheckParameterDateValue MethodEvaluationBeginDate { get; private set; }

    /// <summary>
    /// Implements check parameter Method_Evaluation_End_Date (id = 405)
    /// </summary>
    public cCheckParameterDateValue MethodEvaluationEndDate { get; private set; }

    /// <summary>
    /// Implements check parameter Program_Evaluation_Begin_Date (id = 848)
    /// </summary>
    public cCheckParameterDateValue ProgramEvaluationBeginDate { get; private set; }

    /// <summary>
    /// Implements check parameter Program_Evaluation_End_Date (id = 849)
    /// </summary>
    public cCheckParameterDateValue ProgramEvaluationEndDate { get; private set; }

    /// <summary>
    /// Implements check parameter Program_Parameter_Evaluation_Begin_Date (id = 3253)
    /// </summary>
    public cCheckParameterDateValue ProgramParameterEvaluationBeginDate { get; private set; }

    /// <summary>
    /// Implements check parameter Program_Parameter_Evaluation_End_Date (id = 3254)
    /// </summary>
    public cCheckParameterDateValue ProgramParameterEvaluationEndDate { get; private set; }

    #endregion


    #region Public Properties: String Parameters

    /// <summary>
    /// Instantiate Check Parameter Properties.
    /// </summary>
    private void InstantiateCheckParameterProperties_String()
    {
      LocationType = new cCheckParameterStringValue(265, "Location_Type", this);
      ProgramMethodParameterDescription = new cCheckParameterStringValue(3255, "Program_Method_Parameter_Description", this);
    }


    /// <summary>
    /// Implements check parameter Location_Type (id = 265)
    /// </summary>
    public cCheckParameterStringValue LocationType { get; private set; }

    /// <summary>
    /// Implements check parameter Program_Method_Parameter_Description (id = 3255)
    /// </summary>
    public cCheckParameterStringValue ProgramMethodParameterDescription { get; private set; }

    #endregion


    #region Protected Abstract Methods

    /// <summary>
    /// This method should instantiate each of the check parameter properties implemented in
    /// the child check parameters objects.
    /// </summary>
    protected override void InstantiateCheckParameterProperties()
    {
      InstantiateCheckParameterProperties_Boolean();
      InstantiateCheckParameterProperties_DataRowView();
      InstantiateCheckParameterProperties_DataView();
      InstantiateCheckParameterProperties_Date();
      InstantiateCheckParameterProperties_String();
    }

    #endregion

  }

}
