using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECMPS.Checks.Parameters;
using ECMPS.Checks.DatabaseAccess;

namespace ECMPS.Checks.NonOperatingEmissionGeneration
{

  /// <summary>
  /// Implements the Check Parameters used by the Non Operating Emission Generation process.
  /// </summary>
  public class cGenerationParameters : cCheckParametersCheckEngine
  {

    #region Public Constructor

    public cGenerationParameters(cCheckProcess checkProcess, cDatabase databaseAux)
      : base(checkProcess, databaseAux)
    {
    }

    #endregion


    #region Public Properties: Check Parameters

    /// <summary>
    /// Implements check parameter Gen_BCO2_Summary_Value_Record (id = 3196)
    /// </summary>
    public cCheckParameterDataRowViewValue GenBco2SummaryValueRecord { get; private set; }

    /// <summary>
    /// Implements check parameter Gen_Begin_Date (id = 3192)
    /// </summary>
    public cCheckParameterDateValue GenBeginDate { get; private set; }

    /// <summary>
    /// Implements check parameter Gen_Begin_Hour (id = 3193)
    /// </summary>
    public cCheckParameterIntegerValue GenBeginHour { get; private set; }

    /// <summary>
    /// Implements check parameter Gen_CO2M_Summary_Value_Record (id = 3195)
    /// </summary>
    public cCheckParameterDataRowViewValue GenCo2mSummaryValueRecord { get; private set; }

    /// <summary>
    /// Implements check parameter Gen_HIT_Summary_Value_Record (id = 3200)
    /// </summary>
    public cCheckParameterDataRowViewValue GenHitSummaryValueRecord { get; private set; }

    /// <summary>
    /// Implements check parameter Gen_Hourly_Op_Data_Record (id = 3194)
    /// </summary>
    public cCheckParameterDataRowViewValue GenHourlyOpDataRecord { get; private set; }

    /// <summary>
    /// Implements check parameter Gen_NOXM_Summary_Value_Record (id = 3199)
    /// </summary>
    public cCheckParameterDataRowViewValue GenNoxmSummaryValueRecord { get; private set; }

    /// <summary>
    /// Implements check parameter Gen_NOXR_Summary_Value_Records (id = 3203)
    /// </summary>
    public cCheckParameterDataRowViewValue GenNoxrSummaryValueRecord { get; private set; }

    /// <summary>
    /// Implements check parameter Gen_OPHOURS_Summary_Value_Record (id = 3202)
    /// </summary>
    public cCheckParameterDataRowViewValue GenOpHoursSummaryValueRecord { get; private set; }

    /// <summary>
    /// Implements check parameter Gen_OPTIME_Summary_Value_Record (id = 3201)
    /// </summary>
    public cCheckParameterDataRowViewValue GenOpTimeSummaryValueRecord { get; private set; }

    /// <summary>
    /// Implements check parameter Gen_OS_Reporting_Requirement (id = 3198)
    /// </summary>
    public cCheckParameterBooleanValue GenOsReportingRequirement { get; private set; }

    /// <summary>
    /// Implements check parameter Gen_Reporting_Frequency (id = 3191)
    /// </summary>
    public cCheckParameterStringValue GenReportingFrequency { get; private set; }

    /// <summary>
    /// Implements check parameter Gen_SO2M_Summary_Value_Record (id = 3197)
    /// </summary>
    public cCheckParameterDataRowViewValue GenSo2mSummaryValueRecord { get; private set; }

    /// <summary>
    /// Implements check parameter Location_Program_Records (id = 301)
    /// </summary>
    public cCheckParameterDataViewLegacy LocationProgramRecords { get; private set; }

    /// <summary>
    /// Implements check parameter Location_Reporting_Frequency_Records (id = 859)
    /// </summary>
    public cCheckParameterDataViewLegacy LocationReportingFrequencyRecords { get; private set; }

    /// <summary>
    /// Implements check parameter MP_Method_Records (id = 2847)
    /// </summary>
    public cCheckParameterDataViewLegacy MpMethodRecords { get; private set; }

    /// <summary>
    /// Implements check parameter Operating_Supp_Data_Records_by_Location (id = 2740)
    /// </summary>
    public cCheckParameterDataViewLegacy OperatingSuppDataRecordsByLocation { get; private set; }

    /// <summary>
    /// Implements check parameter Unit_Stack_Configuration_Records (id = 384)
    /// </summary>
    public cCheckParameterDataViewLegacy UnitStackConfigurationRecords { get; private set; }

    #endregion


    #region Protected Abstract Methods

    /// <summary>
    /// This method should instantiate each of the check parameter properties implemented in
    /// the child check parameters objects.
    /// </summary>
    protected override void InstantiateCheckParameterProperties()
    {
      GenBco2SummaryValueRecord = new cCheckParameterDataRowViewValue(3196, "Gen_BCO2_Summary_Value_Record", this);
      GenBeginDate = new cCheckParameterDateValue(3192, "Gen_Begin_Date", this);
      GenBeginHour = new cCheckParameterIntegerValue(3193, "Gen_Begin_Hour", this);
      GenCo2mSummaryValueRecord = new cCheckParameterDataRowViewValue(3195, "Gen_CO2M_Summary_Value_Record", this);
      GenHitSummaryValueRecord = new cCheckParameterDataRowViewValue(3200, "Gen_HIT_Summary_Value_Record", this);
      GenHourlyOpDataRecord = new cCheckParameterDataRowViewValue(3194, "Gen_Hourly_Op_Data_Record", this);
      GenNoxmSummaryValueRecord = new cCheckParameterDataRowViewValue(3199, "Gen_NOXM_Summary_Value_Record", this);
      GenNoxrSummaryValueRecord = new cCheckParameterDataRowViewValue(3203, "Gen_NOXR_Summary_Value_Records", this);
      GenOpHoursSummaryValueRecord = new cCheckParameterDataRowViewValue(3202, "Gen_OPHOURS_Summary_Value_Record", this);
      GenOpTimeSummaryValueRecord = new cCheckParameterDataRowViewValue(3201, "Gen_OPTIME_Summary_Value_Record", this);
      GenOsReportingRequirement = new cCheckParameterBooleanValue(3198, "Gen_OS_Reporting_Requirement", this);
      GenReportingFrequency = new cCheckParameterStringValue(3191, "Gen_Reporting_Frequency", this);
      GenSo2mSummaryValueRecord = new cCheckParameterDataRowViewValue(3197, "Gen_SO2M_Summary_Value_Record", this);
      LocationProgramRecords = new cCheckParameterDataViewLegacy(301, "Location_Program_Records", this);
      LocationReportingFrequencyRecords = new cCheckParameterDataViewLegacy(859, "Location_Reporting_Frequency_Records", this);
      MpMethodRecords = new cCheckParameterDataViewLegacy(2847, "MP_Method_Records", this);
      OperatingSuppDataRecordsByLocation = new cCheckParameterDataViewLegacy(2740, "Operating_Supp_Data_Records_by_Location", this);
      UnitStackConfigurationRecords = new cCheckParameterDataViewLegacy(384, "Unit_Stack_Configuration_Records", this);
    }

    #endregion

  }

}
