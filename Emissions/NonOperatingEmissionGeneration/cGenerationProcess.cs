using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

using ECMPS.Checks.CheckEngine;
using ECMPS.Checks.CheckEngine.Definitions;
using ECMPS.Checks.DatabaseAccess;
using ECMPS.Checks.EmGeneration.Parameters;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Common;
using ECMPS.Definitions.SeverityCode;
using ECMPS.Definitions.Extensions;


namespace ECMPS.Checks.NonOperatingEmissionGeneration
{

  /// <summary>
  /// cProcess child class definition that handles generation emissions 
  /// for non operating monitoring plans.
  /// </summary>
  public class cGenerationProcess : cProcess
  {

    #region Constructors

    /// <summary>
    /// Creates a Non Operating Emission Generation process object.
    /// </summary>
    /// <param name="CheckEngine">The cCheckEngine parent object.</param>
    public cGenerationProcess(cCheckEngine checkEngine)
      : base(checkEngine, "EMGEN")
    {
      mHourlyProcess = true;
    }

    #endregion


    #region Public Properties

    /// <summary>
    /// The Check Parameters for this process.
    /// </summary>
    public cGenerationParameters GenerationParameters { get { return (cGenerationParameters)ProcessParameters; } }

    /// <summary>
    /// The table containing the generated Hrly Op Data rows
    /// </summary>
    public DataTable HrlyOpDataTable { get; private set; }

    /// <summary>
    /// Indicates whether the migration of generated data should occur.
    /// </summary>
    public bool MigrateGeneratedData { get { return (this.SeverityCd != eSeverityCd.FATAL); } }

    /// <summary>
    /// The table containing the generated Summary Value rows
    /// </summary>
    public DataTable SummaryValueTable { get; private set; }

    #endregion


    #region Private Methods: Core

    /// <summary>
    /// The main method in the process for executing checks.
    /// </summary>
    /// <returns>Returns error discription when a failure occurs.</returns>
    private bool CheckExecution(out string resultMessage)
    {
      bool result;

      try
      {
        resultMessage = null;
        result = true;

        cGenerationCategory locationCategory = new cGenerationCategory(this, "EMGENLC");
        cGenerationCategory locationHourlyCategory = new cGenerationCategory(locationCategory, "EMGENHR");
        cGenerationCategory locationSummaryCategory = new cGenerationCategory(locationCategory, "EMGENSV");

        if (!locationCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref resultMessage) ||
            !locationHourlyCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref resultMessage) ||
            !locationSummaryCategory.InitCheckBands(CheckEngine.DbAuxConnection, ref resultMessage))
        {
          result = false;
        }
        else
        {
          SetDataParameterByMonitorPlan(CheckEngine.MonPlanId);

          DataView monitorLocationView = new DataView(SourceData.Tables["MpMonitorLocation"], null, "Mon_Loc_Id", DataViewRowState.CurrentRows);

          //Initialize Generated Data Store
          GeneratedDataInit();

          for (int monLocDex = 0; (monLocDex < monitorLocationView.Count) && MigrateGeneratedData; monLocDex++)
          {
            DataRowView monitorLocationRow = monitorLocationView[monLocDex];
            string monLocName = monitorLocationRow["LOCATION_NAME"].AsString();

            MonLocId = monitorLocationRow["MON_LOC_ID"].AsString();

            SetDataParameterByMonitorLocation(MonLocId);

            // Run Checks
            if (locationCategory.ProcessChecks(MonLocId, monLocDex, monLocName) && MigrateGeneratedData)
            {
              for (DateTime opDate = CheckEngine.EvaluationBeganDate.Value; opDate <= CheckEngine.EvaluationEndedDate.Value; opDate = opDate.AddDays(1))
                for (int opHour = 0; opHour <= 23; opHour++)
                {
                  // Run Hourly Checks
                  if (locationHourlyCategory.ProcessChecks(MonLocId, monLocDex, monLocName, opDate, opHour))
                  {
                    GeneratedDataAppendHourly();
                  }
                  locationHourlyCategory.EraseParameters();
                }

              //Run Summary Checks
              locationSummaryCategory.ProcessChecks(MonLocId, monLocDex, monLocName);
              {
                GeneratedDataAppendSummary();
              }
              locationSummaryCategory.EraseParameters();
            }
            locationCategory.EraseParameters();
          }

          // Push Generated Data and Check Log Updates
          DbUpdate(ref resultMessage);
        }

        locationCategory = null;
        locationHourlyCategory = null;
        locationSummaryCategory = null;
      }
      catch (Exception ex)
      {
        resultMessage = ex.Message;
        result = false;
      }

      return result;
    }


    #region Generated Data Methods

    /// <summary>
    /// Sets Data Parameters dependent on the check execution loop instead of a particular category.
    /// </summary>
    /// <param name="monLocId"></param>
    private void SetDataParameterByMonitorLocation(string monLocId)
    {
      string monLocIdFilter = string.Format("MON_LOC_ID = '{0}'", monLocId);

      // Location_Program_Records
      GenerationParameters.LocationProgramRecords.InitValue
        (
          new DataView(mSourceData.Tables["MpLocationProgram"], monLocIdFilter, null, DataViewRowState.CurrentRows)
        );

      // Location_Reporting_Frequency_Records
      GenerationParameters.LocationReportingFrequencyRecords.InitValue
        (
          new DataView(mSourceData.Tables["MpLocationReportingFrequency"], monLocIdFilter, null, DataViewRowState.CurrentRows)
        );

      // Operating_Supp_Data_Records_by_Location
      GenerationParameters.OperatingSuppDataRecordsByLocation.InitValue
        (
          new DataView(mSourceData.Tables["MpYearOpSuppData"], monLocIdFilter, null, DataViewRowState.CurrentRows)
        );

      // Unit_Stack_Configuration_Records
      GenerationParameters.UnitStackConfigurationRecords.InitValue
        (
          new DataView(mSourceData.Tables["MpUnitStackConfiguration"], monLocIdFilter, null, DataViewRowState.CurrentRows)
        );
    }

    /// <summary>
    /// Sets Data Parameters dependent on the check execution loop instead of a particular category.
    /// </summary>
    /// <param name="monLocId"></param>
    private void SetDataParameterByMonitorPlan(string monPlanIdId)
    {
      string monPlanIdFilter = string.Format("MON_PLAN_ID = '{0}'", monPlanIdId);

      // MP_Method_Records
      GenerationParameters.MpMethodRecords.InitValue
        (
          new DataView(mSourceData.Tables["MpMonitorMethod"], monPlanIdFilter, null, DataViewRowState.CurrentRows)
        );
    }

    /// <summary>
    /// Adds hour op data to the generation table.
    /// </summary>
    private void GeneratedDataAppendHourly()
    {
      if (!GenerationParameters.GenHourlyOpDataRecord.IsNull)
      {
        DataRowView genHourlyOpDataRecord = GenerationParameters.GenHourlyOpDataRecord.Value;

        DataRow addRow = HrlyOpDataTable.NewRow();

        addRow["SESSION_ID"] = CheckEngine.WorkspaceSessionId;
        addRow["MON_LOC_ID"] = genHourlyOpDataRecord["MON_LOC_ID"];
        addRow["RPT_PERIOD_ID"] = genHourlyOpDataRecord["RPT_PERIOD_ID"];
        addRow["BEGIN_DATE"] = genHourlyOpDataRecord["BEGIN_DATE"];
        addRow["BEGIN_HOUR"] = genHourlyOpDataRecord["BEGIN_HOUR"];
        addRow["OP_TIME"] = genHourlyOpDataRecord["OP_TIME"];
        addRow["USERID"] = CheckEngine.UserId;

        HrlyOpDataTable.Rows.Add(addRow);
      }
    }

    /// <summary>
    /// Adds summary value data to the generation table.
    /// </summary>
    private void GeneratedDataAppendSummary()
    {
      GeneratedDataAppendSummary(GenerationParameters.GenBco2SummaryValueRecord);
      GeneratedDataAppendSummary(GenerationParameters.GenCo2mSummaryValueRecord);
      GeneratedDataAppendSummary(GenerationParameters.GenHitSummaryValueRecord);
      GeneratedDataAppendSummary(GenerationParameters.GenNoxmSummaryValueRecord);
      GeneratedDataAppendSummary(GenerationParameters.GenNoxrSummaryValueRecord);
      GeneratedDataAppendSummary(GenerationParameters.GenOpHoursSummaryValueRecord);
      GeneratedDataAppendSummary(GenerationParameters.GenOpTimeSummaryValueRecord);
      GeneratedDataAppendSummary(GenerationParameters.GenSo2mSummaryValueRecord);
    }

    /// <summary>
    /// Initializes the calculated data stores
    /// </summary>
    private void GeneratedDataInit()
    {
      HrlyOpDataTable = DbDataConnection.CloneDataTable("CheckEmGen.HrlyOpData");
      if (DbDataConnection.InternalError) throw DbDataConnection.LastException;

      SummaryValueTable = DbDataConnection.CloneDataTable("CheckEmGen.SummaryValue");
      if (DbDataConnection.InternalError) throw DbDataConnection.LastException;
    }

    /// <summary>
    /// Loads ECMPS_WS tables for the process with calculated values.
    /// </summary>
    /// <param name="sqlTransaction">The transaction to use with any commands.  Use null for no transaction.</param>
    /// <param name="errorMessage">The error message returned on failure.</param>
    /// <returns>Returns true if the update succeeds.</returns>
    private bool GeneratedDataPush(SqlTransaction sqlTransaction, ref string errorMessage)
    {
      bool result;

      if (mCheckEngine.DbWsConnection.ClearUpdateSession(eWorkspaceDataType.EMGEN, CheckEngine.WorkspaceSessionId))
      {
        if (MigrateGeneratedData)
        {
          if (DbWsConnection.BulkLoad(HrlyOpDataTable, "CheckEmGen.HrlyOpData", ref errorMessage) &&
              DbWsConnection.BulkLoad(SummaryValueTable, "CheckEmGen.SummaryValue", ref errorMessage))
            result = true;
          else
            result = false;
        }
        else
          result = true;
      }
      else
      {
        errorMessage = mCheckEngine.DbWsConnection.LastError;
        result = false;
      }

      return result;
    }

    #endregion


    #region Helper Methods

    private void GeneratedDataAppendSummary(cCheckParameterDataRowViewValue genSummaryValueRecordParameter)
    {
      if (!genSummaryValueRecordParameter.IsNull)
      {
        DataRowView genSummaryValueRecord = genSummaryValueRecordParameter.Value;

        DataRow addRow = SummaryValueTable.NewRow();

        addRow["SESSION_ID"] = CheckEngine.WorkspaceSessionId;
        addRow["MON_LOC_ID"] = genSummaryValueRecord["MON_LOC_ID"];
        addRow["RPT_PERIOD_ID"] = genSummaryValueRecord["RPT_PERIOD_ID"];
        addRow["PARAMETER_CD"] = genSummaryValueRecord["PARAMETER_CD"];
        addRow["CURRENT_RPT_PERIOD_TOTAL"] = GetDbDecimalValue(genSummaryValueRecord["CURRENT_RPT_PERIOD_TOTAL"].AsDecimal(), eDecimalPrecision.CURRENT_RPT_PERIOD_TOTAL);
        addRow["OS_TOTAL"] = GetDbDecimalValue(genSummaryValueRecord["OS_TOTAL"].AsDecimal(), eDecimalPrecision.OS_TOTAL);
        addRow["YEAR_TOTAL"] = GetDbDecimalValue(genSummaryValueRecord["YEAR_TOTAL"].AsDecimal(), eDecimalPrecision.YEAR_TOTAL);
        addRow["USERID"] = CheckEngine.UserId;

        SummaryValueTable.Rows.Add(addRow);
      }
    }

    /// <summary>
    /// Converts decimal value to DBNull if null, less than zero or to big for target field.
    /// </summary>
    /// <param name="decimalValue">The value to convert.</param>
    /// <param name="decimalPrecision">The percision of the target field.</param>
    /// <returns>The value to store in the target field.</returns>
    private object GetDbDecimalValue(decimal? decimalValue, eDecimalPrecision decimalPrecision)
    {
      object result;

      if (decimalValue.HasValue && (decimalValue.Value >= 0) && cDecimalPrecision.Check(decimalValue.Value, decimalPrecision))
      {
        result = decimalValue.Value;
      }
      else
      {
        result = DBNull.Value;
      }

      return result;
    }

    #endregion

    #endregion


    #region Base Class Overrides

    #region Properties

    /// <summary>
    /// The Update ECMPS Status process identifier.
    /// </summary>
    protected override string DbUpdate_EcmpsStatusProcess { get { return "EM Generation"; } }

    /// <summary>
    /// The Update ECMPS Status id key or list for the item(s) for which the update will occur.
    /// </summary>
    protected override string DbUpdate_EcmpsStatusIdKeyOrList { get { return CheckEngine.MonPlanId; } }

    /// <summary>
    /// The Update ECMPS Status report period id for the item(s) for which the update will occur.
    /// </summary>
    protected override int? DbUpdate_EcmpsStatusPeriodId { get { return CheckEngine.RptPeriodId.Value; } }

    /// <summary>
    /// The Update ECMPS Status Additional value for the items(s) for which the update will occur..
    /// </summary>
    protected override string DbUpdate_EcmpsStatusOtherField { get { return CheckEngine.ChkSessionId; } }

    /// <summary>
    /// Returns the WS data type for the process.
    /// </summary>
    /// <returns>The workspace data type for the process, or null for none.</returns>
    protected override eWorkspaceDataType? DbUpdate_WorkspaceDataType { get { return (MigrateGeneratedData ? eWorkspaceDataType.EMGEN : (eWorkspaceDataType?)null); } }

    #endregion


    #region Methods

    /// <summary>
    /// Loads the Check Procedure delegates needed for a process code.
    /// </summary>
    /// <param name="checksDllPath">The path of the checks DLLs.</param>
    /// <param name="errorMessage">The message returned if the initialization fails.</param>
    /// <returns>True if the initialization succeeds.</returns>
    public override bool CheckProcedureInit(string checksDllPath, ref string errorMessage)
    {
      bool result;

      try
      {
        object[] arguments = new object[] { this };

        Checks[52] = (cChecks)Activator.CreateInstanceFrom(checksDllPath + "ECMPS.Checks.Emissions.dll",
                                                           "ECMPS.Checks.NonOperatingEmissionGeneration.cGenerationChecks",
                                                           true, 0, null, arguments, null, null ).Unwrap();

        result = true;
      }
      catch (Exception ex)
      {
        errorMessage = ex.FormatError();
        result = false;
      }

      return result;
    }

    /// <summary>
    /// Executes the checks session for a process.
    /// </summary>
    /// <returns>Returns an error message for the session if produced.</returns>
    protected override string ExecuteChecksWork()
    {
      string result;

      if (!CheckExecution(out result))
      {
        UpdateErrors(string.Format("[MonPlan: {0}, RptPeriod: {1}]: {2}",
                     CheckEngine.MonPlanId, CheckEngine.RptPeriodId,
                     result));
      }

      return result;
    }

    /// <summary>
    /// Loads ECMPS_WS tables for the process with calculated values.
    /// </summary>
    /// <param name="sqlTransaction">The transaction to use with any commands.  Use null for no transaction.</param>
    /// <param name="errorMessage">The error message returned on failure.</param>
    /// <returns>Returns true if the update succeeds.</returns>
    protected override bool DbUpdate_CalcWsLoad(SqlTransaction sqlTransaction, ref string errorMessage)
    {
      bool result;

      result = GeneratedDataPush(sqlTransaction, ref errorMessage);

      return result;
    }

    /// <summary>
    /// Initializes the calculated data stores
    /// </summary>
    protected override void InitCalculatedData()
    {
      GeneratedDataInit();
    }

    /// <summary>
    /// Initializes the Check Parameters obect to a Default Check Parameters instance.  The default
    /// does not implement any parameters as properties and processes that do should override this
    /// method and set the Check Parameters object to and instance that implements parameters as
    /// properties.
    /// </summary>
    protected override void InitCheckParameters()
    {
      ProcessParameters = new cGenerationParameters(this, mCheckEngine.DbAuxConnection);
    }

    /// <summary>
    /// Load data used during a particular check session.
    /// </summary>
    protected override void InitSourceData()
    {
      bool result;
      string errorMessage;

      try
      {
        DbDataConnection.CreateStoredProcedureCommand("CheckEmGen.GetSourceData");
        DbDataConnection.AddInputParameter("@monPlanId", CheckEngine.MonPlanId);
        DbDataConnection.AddInputParameter("@rptPeriodId", CheckEngine.RptPeriodId);
        DbDataConnection.AddOutputParameterString("@result", 1);
        DbDataConnection.AddOutputParameterString("@resultMessage", 200);

        mSourceData = DbDataConnection.GetDataSet();

        errorMessage = DbDataConnection.GetParameterString("@resultMessage");
        result = (DbDataConnection.GetParameterString("@result") == "T");

        if (result)
        {
          mSourceData.Tables[0].TableName = "MpLocationProgram";
          mSourceData.Tables[1].TableName = "MpLocationReportingFrequency";
          mSourceData.Tables[2].TableName = "MpMonitorLocation";
          mSourceData.Tables[3].TableName = "MpMonitorMethod";
          mSourceData.Tables[4].TableName = "MpYearOpSuppData";
          mSourceData.Tables[5].TableName = "MpUnitStackConfiguration";
          mSourceData.Tables[6].TableName = "ProgramCode";
        }
      }
      catch (Exception ex)
      {
        errorMessage = ex.Message;
        result = false;
      }
    }

    /// <summary>
    /// This method initializes the class containing static properties enabling strongly typed access to the parameters used by the process.
    /// </summary>
    protected override void InitStaticParameterClass()
    {
      EmGenerationParameters.Init(this);
    }

    /// <summary>
    /// Allows the setting of the current category for which parameters will be set.
    /// </summary>
    /// <param name="category"></param>
    public override void SetStaticParameterCategory(cCategory category)
    {
      EmGenerationParameters.Category = category;
    }

    #endregion

    #endregion

  }

}
