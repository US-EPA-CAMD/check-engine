using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.IO;

using ECMPS.Checks.CheckEngine.Definitions;
using ECMPS.Checks.DatabaseAccess;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;

using ECMPS.Common;
using ECMPS.Definitions.Extensions;
using ECMPS.Definitions.SeverityCode;

namespace ECMPS.Checks.CheckEngine
{
  /// <summary>
  /// The oject created to represnt a process and provide functionality needed at the process level.
  /// </summary>
  public abstract class cProcess : cCheckProcess
  {

    #region Constructors

    /// <summary>
    /// Creates and instance of cProcess
    /// </summary>
    /// <param name="CheckEngine">The Check Engine object parent</param>
    /// <param name="AProcessCd">The Process Code associanted with the process object.</param>
    public cProcess(cCheckEngine CheckEngine, string AProcessCd)
      : base(AProcessCd)
    {
      mCheckEngine = CheckEngine;
      mCheckEngine.ResultCatalog.GetCheckParameter = GetCheckParameter;

      Checks = new cChecks[72];

      InitLogs();
      InitSourceData();
      InitCalculatedData();
      InitCheckParameters();
      InitStaticParameterClass();
    }


    /// <summary>
    /// Instantiates a cProcess object primarily for unit testing purposes.
    /// </summary>
    protected cProcess()
    {
    }

    #endregion


    #region Public Properties

    #region Property Fields

    /// <summary>
    /// The dataset containing the calculated data tables.
    /// </summary>
    protected DataSet mCalculatedData;

    /// <summary>
    /// The parent check engine object.
    /// </summary>
    protected cCheckEngine mCheckEngine;

    /// <summary>
    /// The check parameters hash table used to implement the check parameters for the process.
    /// A search for or setting of a check parameter only happens hear if the parameter was not
    /// implemented in mCheckParameters.
    /// </summary>
    protected Hashtable mCheckParametersOld = new Hashtable();

    /// <summary>
    /// The data set containing the source data for the process.
    /// </summary>
    protected DataSet mSourceData;

    #endregion


    /// <summary>
    /// The dataset containing the calculated data tables.
    /// </summary>
    public DataSet CalculatedData { get { return mCalculatedData; } }

    /// <summary>
    /// The parent check engine object.
    /// </summary>
    public cCheckEngine CheckEngine { get { return mCheckEngine; } }

    /// <summary>
    /// The check parameters hash table used to implement the check parameters for the process.
    /// A search for or setting of a check parameter only happens hear if the parameter was not
    /// implemented in mCheckParameters.
    /// </summary>
    public Hashtable CheckParametersOld { get { return mCheckParametersOld; } }

    /// <summary>
    /// Array of checks for each check type.
    /// </summary>
    public cChecks[] Checks { get; protected set; }

    /// <summary>
    /// Connection to ECMPS database
    /// </summary>
    public cDatabase DbDataConnection { get { return (mCheckEngine != null) ? CheckEngine.DbDataConnection : null; } }

    /// <summary>
    /// Connection to ECMPS_AUX database
    /// </summary>
    public cDatabase DbAuxConnection { get { return (mCheckEngine != null) ? CheckEngine.DbAuxConnection : null; } }

    /// <summary>
    /// Connection to ECMPS_WS database
    /// </summary>
    public cDatabase DbWsConnection { get { return (mCheckEngine != null) ? CheckEngine.DbWsConnection : null; } }

    /// <summary>
    /// Used to limit this distance of dates in the future.
    /// </summary>
    public DateTime MaximumFutureDate { get { return (mCheckEngine != null) ? CheckEngine.MaximumFutureDate : DateTime.MinValue; } }

    /// <summary>
    /// The check parameters object that implements the check parameters for this process.
    /// </summary>
    public cCheckParametersCheckEngine ProcessParameters { get; protected set; }

    /// <summary>
    /// The data set containing the source data for the process.
    /// </summary>
    public DataSet SourceData { get { return mSourceData; } }

    #endregion


    #region Protected Fields

    /// <summary>
    /// the FAC_ID
    /// </summary>
    protected long mFacilityID = 0;

    /// <summary>
    /// CHECK_LOG data table
    /// </summary>
    protected DataTable mCheckLogs = new DataTable("CHECK_LOG");

    /// <summary>
    /// Another CHECK_LOG data table
    /// </summary>
    protected DataTable mCheckLogsMerged = new DataTable("CHECK_LOG");

    /// <summary>
    /// MON_LOC_ID
    /// </summary>
    protected string MonLocId = "";

    /// <summary>
    /// HourlyProcess?
    /// </summary>
    protected bool mHourlyProcess = false;

    #endregion


    #region Public Abstract Methods

    /// <summary>
    /// Loads the Check Procedure delegates needed for a process code.
    /// </summary>
    /// <param name="checksDllPath">The path of the checks DLLs.</param>
    /// <param name="errorMessage">The message returned if the initialization fails.</param>
    /// <returns>True if the initialization succeeds.</returns>
    public abstract bool CheckProcedureInit(string checksDllPath, ref string errorMessage);

    #endregion


    #region Public Virtual Methods

    /// <summary>
    /// GetCategory
    /// </summary>
    /// <param name="ACategoryName"></param>
    /// <returns></returns>
    public virtual cCategory GetCategory(string ACategoryName)
    {
      return null;
    }

    /// <summary>
    /// GetCategoryForReferenceData
    /// </summary>
    /// <param name="AReferenceParameter"></param>
    /// <returns></returns>
    public virtual cCategory GetCategoryForReferenceData(string AReferenceParameter)
    {
      return null;
    }

    #endregion


    #region Public Methods: General

    /// <summary>
    /// Get Checks
    /// </summary>
    /// <param name="ruleCheck">The rule check for which to find the procedure.</param>
    /// <param name="checkProcedure">The check procedure to run.</param>
    /// <param name="errorMessage">any error message</param>
    /// <returns>true if the check procedure is found, false if not</returns>
    public bool CheckProcedureGet(cParameterizedCheck ruleCheck,
                                  out cChecks.dCheckProcedure checkProcedure,
                                  ref string errorMessage)
    {
      bool result;

      cChecks checks;

      switch (ruleCheck.CheckTypeCd)
      {
        case "HOUROP": checks = Checks[0]; result = true; break;
        case "HOURAD": checks = Checks[1]; result = true; break;
        case "HOURDHV": checks = Checks[2]; result = true; break;
        case "HOURMHV": checks = Checks[3]; result = true; break;
        // Removed HOURIV checks which are not used  case "HOURIV": checks = Checks[4]; result = true; break;
        case "HOURCV": checks = Checks[5]; result = true; break;
        case "MONPLAN": checks = Checks[6]; result = true; break;
        case "MONLOC": checks = Checks[7]; result = true; break;
        case "PROGRAM": checks = Checks[8]; result = true; break;
        case "METHOD": checks = Checks[9]; result = true; break;
        case "COMPON": checks = Checks[10]; result = true; break;
        case "SYSTEM": checks = Checks[11]; result = true; break;
        case "FUELFLW": checks = Checks[12]; result = true; break;
        case "FORMULA": checks = Checks[13]; result = true; break;
        case "SPAN": checks = Checks[14]; result = true; break;
        case "DEFAULT": checks = Checks[15]; result = true; break;
        case "LOAD": checks = Checks[16]; result = true; break;
        case "QUAL": checks = Checks[17]; result = true; break;
        case "FUEL": checks = Checks[18]; result = true; break;
        case "CONTROL": checks = Checks[19]; result = true; break;
        case "CAPAC": checks = Checks[20]; result = true; break;
        case "LINEAR": checks = Checks[21]; result = true; break;
        case "RATA": checks = Checks[22]; result = true; break;
        case "SEVNDAY": checks = Checks[23]; result = true; break;
        case "F2LREF": checks = Checks[24]; result = true; break;
        case "F2LCHK": checks = Checks[25]; result = true; break;
        case "CYCLE": checks = Checks[26]; result = true; break;
        case "TEST": checks = Checks[27]; result = true; break;
        case "IMPORT": checks = Checks[28]; result = true; break;
        case "ONOFF": checks = Checks[29]; result = true; break;
        case "FFACC": checks = Checks[30]; result = true; break;
        case "FFACCTT": checks = Checks[31]; result = true; break;
        case "FF2LBAS": checks = Checks[32]; result = true; break;
        case "FF2LTST": checks = Checks[33]; result = true; break;
        case "APPE": checks = Checks[34]; result = true; break;
        case "QACERT": checks = Checks[35]; result = true; break;
        case "EXTEXEM": checks = Checks[36]; result = true; break;
        case "HOURAGG": checks = Checks[37]; result = true; break;
        case "HOURAPP": checks = Checks[38]; result = true; break;
        case "HOURAE": checks = Checks[39]; result = true; break;
        case "HOURGEN": checks = Checks[40]; result = true; break;
        case "UNITDEF": checks = Checks[41]; result = true; break;
        case "DAILY": checks = Checks[42]; result = true; break;
        case "DAYCAL": checks = Checks[43]; result = true; break;
        case "EMTEST": checks = Checks[44]; result = true; break;
        case "LME": checks = Checks[45]; result = true; break;
        case "LINSTAT": checks = Checks[46]; result = true; break;
        case "ADESTAT": checks = Checks[47]; result = true; break;
        case "GFMCAL": checks = Checks[48]; result = true; break;
        case "RATSTAT": checks = Checks[49]; result = true; break;
        case "HGLME": checks = Checks[50]; result = true; break;
        case "DCSTAT": checks = Checks[51]; result = true; break;
        case "EMGEN": checks = Checks[52]; result = true; break;
        case "AETB": checks = Checks[53]; result = true; break;
        case "PGVP": checks = Checks[54]; result = true; break;
        case "F2LSTAT": checks = Checks[55]; result = true; break;
        case "INTSTAT": checks = Checks[56]; result = true; break;
        case "LKSTAT": checks = Checks[57]; result = true; break;
		case "QUALLEE": checks = Checks[58]; result = true; break;
		case "MATSMTH": checks = Checks[59]; result = true; break;
		case "MATSHOD": checks = Checks[60]; result = true; break;
		case "MATSMHV": checks = Checks[61]; result = true; break;
		case "MATSCHV": checks = Checks[62]; result = true; break;
		case "MATSDHV": checks = Checks[63]; result = true; break;
		case "MATSTRP": checks = Checks[64]; result = true; break;
		case "MATSTRN": checks = Checks[65]; result = true; break;
		case "MATSGFM": checks = Checks[66]; result = true; break;
        case "EMWTS": checks = Checks[67]; result = true; break;
        case "EMWSI": checks = Checks[68]; result = true; break;
        case "WSISTAT": checks = Checks[69]; result = true; break;
        case "NSPS4T": checks = Checks[70]; result = true; break;
        case "EMAUDIT": checks = Checks[71]; result = true; break;

        default:
          {
            checks = null;
            errorMessage = "[Process.CheckProcedureGet]: No class defined for this check type.";
            result = false;
          }
          break;
      }

      if (result)
      {
        checkProcedure = checks.GetCheckProcedure(ruleCheck.CheckNumber);

        if (checkProcedure == null)
        {
          errorMessage = string.Format("[Process.CheckProcedureGet]: Unable to find check procedure {0}{1}'",
                                       ruleCheck.CheckTypeCd, ruleCheck.CheckNumber);
          result = false;
        }
      }
      else
        checkProcedure = null;

      return result;
    }

    /// <summary>
    /// Initialize the running of checks and then runs the checks.
    /// </summary>
    /// <param name="checksDllPath">The checks DLL path.</param>
    /// <param name="errorMessage">Processing error if the execution fails.</param>
    /// <returns>True if the execution of checks is successful.</returns>
    public bool ExecuteChecks(string checksDllPath, ref string errorMessage)
    {
      bool result;

      SeverityCd = eSeverityCd.NONE;

      if (CheckProcedureInit(checksDllPath, ref errorMessage))
      {
        errorMessage = ExecuteChecksWork();
        result = errorMessage.IsEmpty();
      }
      else
        result = false;

      return result;
    }

    /// <summary>
    /// Initialize the running of checks and then runs the checks.
    /// </summary>
    /// <returns>True if the execution of checks is successful.</returns>
    public string ExecuteChecks()
    {
      SeverityCd = eSeverityCd.NONE;

      return ExecuteChecksWork();
    }

    /// <summary>
    /// Returns the check bands for the passed category code.
    /// </summary>
    /// <param name="ACategoryCd">The category code of the checks bands to load.</param>
    /// <param name="ADatabaseAux">The AUX database object to use for the update.</param>
    /// <param name="AErrorMessage">The error message returned on failure.</param>
    /// <returns>True if the population is successful.</returns>
    public cCheckParameterBands GetCheckBands(string ACategoryCd, cDatabase ADatabaseAux, ref string AErrorMessage)
    {
      cCheckParameterBands Result = new cCheckParameterBands(ACategoryCd);

      if (!Result.Populate(ADatabaseAux, this.ProcessParameters, ref AErrorMessage))
        Result = null;

      return Result;
    }

    /// <summary>
    /// Returns the check bands for the passed category code.
    /// </summary>
    /// <param name="ACategoryCd">The category code of the checks bands to load.</param>
    /// <param name="AErrorMessage">The error message returned on failure.</param>
    /// <returns>True if the population is successful.</returns>
    public cCheckParameterBands GetCheckBands(string ACategoryCd, ref string AErrorMessage)
    {
      return GetCheckBands(ACategoryCd, mCheckEngine.DbAuxConnection, ref AErrorMessage);
    }

    /// <summary>
    /// Returns the check bands for the passed category code.
    /// </summary>
    /// <param name="ACategoryCd">The category code of the checks bands to load.</param>
    /// <returns>True if the population is successful.</returns>
    public cCheckParameterBands GetCheckBands(string ACategoryCd)
    {
      string ErrorMessage = "";

      return GetCheckBands(ACategoryCd, mCheckEngine.DbAuxConnection, ref ErrorMessage);
    }

    /// <summary>
    /// Populates the internal Check Log table used, ultimately, to update the the AUX Check Log.
    /// </summary>
    /// <param name="ALogComment">The check log comment.</param>
    /// <param name="ARuleCheck">The Parameterized Check object for the Rule Check.</param>
    /// <param name="AMonitorLocationId">The Monitor Location Id associated with the error.</param>
    /// <param name="ATestSummaryId">The Test Summary Id associated with the error.</param>
    /// <param name="AOpDate">The Operating Date associated with the error.</param>
    /// <param name="AOpHour">The Operating Hour associated with the error.</param>
    /// <param name="ASourceTableName">The ECMPS source table associated with the error.</param>
    /// <param name="ASourceRowId">The source table row id associated with the error.</param>
    /// <param name="ACheckCatalogResult">The Check Catalog Result of the error.</param>
    /// <param name="ARecordIdentifier">The identifying source information of the associated row.</param>
    /// <param name="ACategory">The category object in which the error was produced.</param>
    /// <param name="ALoggingError">Indicates the logging is for an error in processing a check.</param>
    /// <param name="ADebugMode">Indicates whether evaluation is in debug mode.</param>
    /// <param name="AResultSeverityCd">The resulting severity code.</param>
    /// <returns>Always returns true.</returns>
    public bool UpdateLog(string ALogComment,
                          cParameterizedCheck ARuleCheck,
                          string AMonitorLocationId,
                          string ATestSummaryId,
                          DateTime AOpDate,
                          int AOpHour,
                          string ASourceTableName,
                          string ASourceRowId,
                          string ACheckCatalogResult,
                          string ARecordIdentifier,
                          cCategory ACategory,
                          bool ALoggingError,
                          bool ADebugMode,
                          out eSeverityCd AResultSeverityCd)
    {
      object CheckCatalogResultId;
      object CheckCatalogResultCd;
      object CheckCatalogResultMessage;
      object CheckCatalogSeverityCd;
      object CheckCatalogSuppressedSeverityCd;
      object CheckCatalogErrorSuppressId;
      string ErrorMessage = "";

      // Retrieve the Check Catalog Result Id and Result Message.
      if (!string.IsNullOrEmpty(ACheckCatalogResult))
      {
        long ResultId;
        string ResultCd, ResultMessage;
        eSeverityCd SuppressedSeverityCd;
        long? ErrorSuppressId;
        string SuppressionComment;

        if (CheckEngine.ResultCatalog.GetResultInfo(ARuleCheck.CheckTypeCd, ARuleCheck.CheckNumber, ACheckCatalogResult,
                                                    ARecordIdentifier, ACategory,
                                                    out ResultId, out ResultCd, out ResultMessage,
                                                    out AResultSeverityCd, out SuppressedSeverityCd, out ErrorSuppressId,
                                                    out SuppressionComment, out ErrorMessage))
        {
          CheckCatalogResultId = cDBConvert.ToVariant(ResultId);
          CheckCatalogResultCd = ResultCd;
          CheckCatalogResultMessage = ResultMessage;
          CheckCatalogSeverityCd = AResultSeverityCd.ToStringValue();

          if (SuppressedSeverityCd != AResultSeverityCd)
          {
            CheckCatalogSuppressedSeverityCd = SuppressedSeverityCd.ToStringValue();
            CheckCatalogErrorSuppressId = ErrorSuppressId.DbValue();
          }
          else
          {
            CheckCatalogSuppressedSeverityCd = DBNull.Value;
            CheckCatalogErrorSuppressId = DBNull.Value;
          }

          if (ALogComment.IsEmpty() && !SuppressionComment.IsEmpty())
            ALogComment = SuppressionComment;
        }
        else
        {
          AResultSeverityCd = eSeverityCd.FATAL;

          CheckCatalogResultId = DBNull.Value;
          CheckCatalogResultCd = DBNull.Value;
          CheckCatalogResultMessage = string.Format("[{1}-{2}] (Response Retrieval Error) : {0}", ErrorMessage, ARuleCheck, ACheckCatalogResult);
          CheckCatalogSeverityCd = AResultSeverityCd.ToStringValue();
          CheckCatalogSuppressedSeverityCd = DBNull.Value;
          CheckCatalogErrorSuppressId = DBNull.Value;
        }
      }
      else if (ALoggingError)
      {
        AResultSeverityCd = eSeverityCd.FATAL;

        CheckCatalogResultId = DBNull.Value;
        CheckCatalogResultCd = DBNull.Value;
        CheckCatalogResultMessage = ALogComment.DbValue();
        CheckCatalogSeverityCd = AResultSeverityCd.ToStringValue();
        CheckCatalogSuppressedSeverityCd = DBNull.Value;
        CheckCatalogErrorSuppressId = DBNull.Value;
      }
      else
      {
        AResultSeverityCd = eSeverityCd.NONE;

        CheckCatalogResultId = DBNull.Value;
        CheckCatalogResultCd = DBNull.Value;
        CheckCatalogResultMessage = DBNull.Value;
        CheckCatalogSeverityCd = AResultSeverityCd.ToStringValue();
        CheckCatalogSuppressedSeverityCd = DBNull.Value;
        CheckCatalogErrorSuppressId = DBNull.Value;
      }

      if (!string.IsNullOrEmpty(ACheckCatalogResult) ||
          !string.IsNullOrEmpty(ALogComment) ||
          ADebugMode)
      {
        //Add Check Log Row
        DataRow CheckLogRow = mCheckLogs.NewRow();

        CheckLogRow["CHK_SESSION_ID"] = mCheckEngine.ChkSessionId;
        CheckLogRow["BEGIN_DATE"] = DateTime.Now;
        CheckLogRow["RULE_CHECK_ID"] = ARuleCheck.RuleCheckId;
        CheckLogRow["CHK_LOG_COMMENT"] = ALogComment;
        CheckLogRow["MON_LOC_ID"] = AMonitorLocationId;
        CheckLogRow["TEST_SUM_ID"] = ATestSummaryId;
        CheckLogRow["SOURCE_TABLE"] = ASourceTableName;
        CheckLogRow["ROW_ID"] = ASourceRowId;

        if (AOpDate != DateTime.MinValue)
        {
          CheckLogRow["CHECK_DATE"] = AOpDate;

          if (AOpHour != int.MinValue)
            CheckLogRow["CHECK_HOUR"] = AOpHour;
          else
            CheckLogRow["CHECK_HOUR"] = DBNull.Value;
        }
        else
        {
          CheckLogRow["CHECK_DATE"] = DBNull.Value;
          CheckLogRow["CHECK_HOUR"] = DBNull.Value;
        }

        CheckLogRow["CHECK_CATALOG_RESULT_ID"] = CheckCatalogResultId;
        CheckLogRow["CHECK_RESULT"] = CheckCatalogResultCd;
        CheckLogRow["RESULT_MESSAGE"] = CheckCatalogResultMessage;
        CheckLogRow["SEVERITY_CD"] = CheckCatalogSeverityCd;
        CheckLogRow["SUPPRESSED_SEVERITY_CD"] = CheckCatalogSuppressedSeverityCd;
        CheckLogRow["ERROR_SUPPRESS_ID"] = CheckCatalogErrorSuppressId;

        mCheckLogs.Rows.Add(CheckLogRow);
      }

      return true;
    }

    /// <summary>
    /// Populates the internal Check Log table used, ultimately, to update the the AUX Check Log.
    /// </summary>
    /// <param name="logComment">The check log comment.</param>
    /// <param name="ruleCheck">The Parameterized Check object for the Rule Check.</param>
    /// <param name="monitorLocationId">The Monitor Location Id associated with the error.</param>
    /// <param name="testSummaryId">The Test Summary Id associated with the error.</param>
    /// <param name="opDate">The Operating Date associated with the error.</param>
    /// <param name="opHour">The Operating Hour associated with the error.</param>
    /// <param name="sourceTableName">The ECMPS source table associated with the error.</param>
    /// <param name="sourceRowId">The source table row id associated with the error.</param>
    /// <param name="recordIdentifier">The identifying source information of the associated row.</param>
    /// <param name="category">The category object in which the error was produced.</param>
    /// <returns>Always returns true.</returns>
    public bool LogDebug(string logComment,
                         cParameterizedCheck ruleCheck,
                         string monitorLocationId,
                         string testSummaryId,
                         DateTime opDate,
                         int opHour,
                         string sourceTableName,
                         string sourceRowId,
                         string recordIdentifier,
                         cCategory category)
    {
      bool result;

      eSeverityCd resultSeverityCd;

      result = UpdateLog(logComment, ruleCheck,
                         monitorLocationId, testSummaryId, opDate, opHour,
                         sourceTableName, sourceRowId, null,
                         recordIdentifier, category,
                         false, true,
                         out resultSeverityCd);

      return result;
    }

    /// <summary>
    /// Populates the internal Check Log table used, ultimately, to update the the AUX Check Log.
    /// </summary>
    /// <param name="errorMessage">The check log comment.</param>
    /// <param name="ruleCheck">The Parameterized Check object for the Rule Check.</param>
    /// <param name="monitorLocationId">The Monitor Location Id associated with the error.</param>
    /// <param name="testSummaryId">The Test Summary Id associated with the error.</param>
    /// <param name="opDate">The Operating Date associated with the error.</param>
    /// <param name="opHour">The Operating Hour associated with the error.</param>
    /// <param name="sourceTableName">The ECMPS source table associated with the error.</param>
    /// <param name="sourceRowId">The source table row id associated with the error.</param>
    /// <param name="recordIdentifier">The identifying source information of the associated row.</param>
    /// <param name="category">The category object in which the error was produced.</param>
    /// <param name="resultSeverityCd">The resulting severity code.</param>
    /// <returns>Always returns true.</returns>
    public bool LogError(string errorMessage,
                         cParameterizedCheck ruleCheck,
                         string monitorLocationId,
                         string testSummaryId,
                         DateTime opDate,
                         int opHour,
                         string sourceTableName,
                         string sourceRowId,
                         string recordIdentifier,
                         cCategory category,
						             out eSeverityCd resultSeverityCd)
    {
      bool result;

      result = UpdateLog(errorMessage, ruleCheck,
                         monitorLocationId, testSummaryId, opDate, opHour,
                         sourceTableName, sourceRowId, null,
                         recordIdentifier, category,
                         true, false,
                         out resultSeverityCd);

      return result;
    }

    /// <summary>
    /// Populates the internal Check Log table used, ultimately, to update the the AUX Check Log.
    /// </summary>
    /// <param name="ruleCheck">The Parameterized Check object for the Rule Check.</param>
    /// <param name="monitorLocationId">The Monitor Location Id associated with the error.</param>
    /// <param name="testSummaryId">The Test Summary Id associated with the error.</param>
    /// <param name="opDate">The Operating Date associated with the error.</param>
    /// <param name="opHour">The Operating Hour associated with the error.</param>
    /// <param name="sourceTableName">The ECMPS source table associated with the error.</param>
    /// <param name="sourceRowId">The source table row id associated with the error.</param>
    /// <param name="checkCatalogResult">The Check Catalog Result of the error.</param>
    /// <param name="recordIdentifier">The identifying source information of the associated row.</param>
    /// <param name="errorDateHour">The applicable hour for an error.</param>
    /// <param name="category">The category object in which the error was produced.</param>
    /// <param name="resultSeverityCd">The resulting severity code.</param>
    /// <returns>Always returns true.</returns>
    public bool LogResult(cParameterizedCheck ruleCheck,
                          string monitorLocationId,
                          string testSummaryId,
                          DateTime opDate,
                          int opHour,
                          string sourceTableName,
                          string sourceRowId,
                          string checkCatalogResult,
                          string recordIdentifier,
                          DateTime? errorDateHour,
                          cCategory category,
                          out eSeverityCd resultSeverityCd)
    {
      bool result;

      DateTime logDate; int logHour;
      if (errorDateHour.HasValue)
      {
        logDate = errorDateHour.Value.Date;
        logHour = errorDateHour.Value.Hour;
      }
      else
      {
        logDate = opDate;
        logHour = opHour;
      }

      result = UpdateLog(null, ruleCheck,
                         monitorLocationId, testSummaryId, logDate, logHour,
                         sourceTableName, sourceRowId, checkCatalogResult,
                         recordIdentifier, category,
                         false, false,
                         out resultSeverityCd);

      return result;
    }

    #endregion


    #region Public Methods: Check Parameters including Abstract and Virtual

    #region Abstract and Virtual

    /// <summary>
    /// Initializes the Check Parameters obect to a Default Check Parameters instance.  The default
    /// does not implement any parameters as properties and processes that do should override this
    /// method and set the Check Parameters object to and instance that implements parameters as
    /// properties.
    /// </summary>
    protected virtual void InitCheckParameters()
    {
      ProcessParameters = new cCheckParametersDefault(this, mCheckEngine.DbAuxConnection);
    }


    /// <summary>
    /// This method initializes the class containing static properties enabling strongly typed access to the parameters used by the process.
    /// </summary>
    protected abstract void InitStaticParameterClass();

    /// <summary>
    /// Allows the setting of the current category for which parameters will be set.
    /// </summary>
    /// <param name="category"></param>
    public abstract void SetStaticParameterCategory(cCategory category);

    #endregion


    #region Parameter Manipulation

    /// <summary>
    /// Resets all parameter values in CheckParameters.
    /// </summary>
    public void EraseParameters()
    {
      ProcessParameters.Reset();
    }

    /// <summary>
    /// Gets the parameter specified by the passed parameter name.
    /// </summary>
    /// <param name="AParameterName">The name of the parameter to return.</param>
    /// <returns>Returns the parameter or null if not found.</returns>
    public cLegacyCheckParameter GetCheckParameter(string AParameterName)
    {
      cLegacyCheckParameter Result = new cLegacyCheckParameter();

      if (ProcessParameters == null)
      {
        System.Diagnostics.Debug.WriteLine(string.Format("Failed Parameter Get: Process - {0}, Parameter - {1} : {2}",
                                                         this.ProcessCd, AParameterName,
                                                         "Check parameters object not implemented for this process."));
      }
      else if (!ProcessParameters.ContainsLegacyParameter(AParameterName))
      {
        System.Diagnostics.Debug.WriteLine(string.Format("Failed Parameter Get: Process - {0}, Parameter - {1} : {2}",
                                                         this.ProcessCd, AParameterName,
                                                         "Check parameter not implemented for this process."));
      }
      else
      {
        try
        {
          cCheckParameterCheckEngine CheckParameter = ProcessParameters.GetLegacyParameter(AParameterName);

          if (CheckParameter.IsSet)
          {
            Result.ParameterName = CheckParameter.Name;
            Result.ParameterValue = CheckParameter.LegacyValue;
            Result.ParameterType = CheckParameter.DataType;
            Result.ParameterMissing = false;
            Result.IsArray = CheckParameter.IsArray;
          }
        }
        catch (Exception ex)
        {
          System.Diagnostics.Debug.WriteLine(string.Format("Failed Parameter Get: Process - {0}, Parameter - {1} : {2}",
                                                           this.ProcessCd, AParameterName, ex.Message));
        }
      }

      return Result;
    }

    /// <summary>
    /// Sets the parameter specified by the passed parameter name.
    /// 
    /// This is the primary Set Check Parameter method.
    /// </summary>
    /// <param name="AParameterName">The name of the parameter to remove.</param>
    /// <param name="AParameterValue">The value to set.</param>
    public void SetCheckParameter(string AParameterName, object AParameterValue)
    {
      if (ProcessParameters == null)
      {
        System.Diagnostics.Debug.WriteLine(string.Format("Failed Parameter Set: Process - {0}, Parameter - {1} : {2}",
                                                         this.ProcessCd, AParameterName,
                                                         "Check parameters object not implemented for this process."));
      }
      else if (!ProcessParameters.ContainsLegacyParameter(AParameterName))
      {
        System.Diagnostics.Debug.WriteLine(string.Format("Failed Parameter Set: Process - {0}, Parameter - {1} : {2}",
                                                         this.ProcessCd, AParameterName,
                                                         "Check parameter not implemented for this process."));
      }
      else
      {
        try
        {
          cCheckParameterCheckEngine CheckParameter = ProcessParameters.GetLegacyParameter(AParameterName);

          CheckParameter.LegacySetValue(AParameterValue);
        }
        catch (Exception ex)
        {
          System.Diagnostics.Debug.WriteLine(string.Format("Failed Parameter Set: Process - {0}, Parameter - {1} : {2}",
                                             this.ProcessCd, AParameterName, ex.Message));
        }
      }
    }

    /// <summary>
    /// Obsolete method that sets the parameter specified by the passed parameter name.
    /// </summary>
    /// <param name="ParameterName">The name of the parameter to remove.</param>
    /// <param name="ParameterValue">The value to set.</param>
    /// <param name="ParameterType">The type of the parameter. (obsolete and ignored)</param>
    /// <param name="AInitRuleCheck">The rule check initializing the parameter. (obsolete and ignored)</param>
    /// <param name="IsAccumulator">True if the parameter is an accumulator. (obsolete and ignored)</param>
    /// <param name="IsArray">True if the parameter is an array. (obsolete and ignored)</param>
    public void SetCheckParameter(string ParameterName, object ParameterValue, eParameterDataType ParameterType,
                                  cParameterizedCheck AInitRuleCheck,
                                  bool IsAccumulator, bool IsArray)
    {
      SetCheckParameter(ParameterName, ParameterValue);
    }

    /// <summary>
    /// Obsolete method that sets the parameter specified by the passed parameter name.
    /// </summary>
    /// <param name="ParameterName">The name of the parameter to remove.</param>
    /// <param name="ParameterValue">The value to set.</param>
    /// <param name="ParameterType">The type of the parameter. (obsolete and ignored)</param>
    /// <param name="AInitRuleCheck">The rule check initializing the parameter. (obsolete and ignored)</param>
    public void SetCheckParameter(string ParameterName, object ParameterValue, eParameterDataType ParameterType,
                                  cParameterizedCheck AInitRuleCheck)
    {
      SetCheckParameter(ParameterName, ParameterValue);
    }

    /// <summary>
    /// Obsolete method that sets the parameter specified by the passed parameter name.
    /// </summary>
    /// <param name="ParameterName">The name of the parameter to remove.</param>
    /// <param name="ParameterValue">The value to set.</param>
    /// <param name="ParameterType">The type of the parameter. (obsolete and ignored)</param>
    /// <param name="IsAccumulator">True if the parameter is an accumulator. (obsolete and ignored)</param>
    /// <param name="IsArray">True if the parameter is an array. (obsolete and ignored)</param>
    public void SetCheckParameter(string ParameterName, object ParameterValue, eParameterDataType ParameterType,
                                  bool IsAccumulator, bool IsArray)
    {
      SetCheckParameter(ParameterName, ParameterValue);
    }

    /// <summary>
    /// Obsolete method that sets the parameter specified by the passed parameter name.
    /// </summary>
    /// <param name="ParameterName">The name of the parameter to remove.</param>
    /// <param name="ParameterValue">The value to set.</param>
    /// <param name="ParameterType">The type of the parameter. (obsolete and ignored)</param>
    public void SetCheckParameter(string ParameterName, object ParameterValue, eParameterDataType ParameterType)
    {
      SetCheckParameter(ParameterName, ParameterValue);
    }

    #endregion

    #endregion


    #region Protected Abstract

    /// <summary>
    /// ExecuteChecksWork
    /// </summary>
    /// <returns></returns>
    protected abstract string ExecuteChecksWork();

    /// <summary>
    /// InitSourceData
    /// </summary>
    protected abstract void InitSourceData();

    /// <summary>
    /// InitCalculatedData
    /// </summary>
    protected abstract void InitCalculatedData();

    #endregion


    #region Protected Virtual: DB Update

    /// <summary>
    /// Loads ECMPS_WS tables for the process with calculated values.
    /// </summary>
    /// <param name="sqlTransaction">The transaction to use with any commands.  Use null for no transaction.</param>
    /// <param name="errorMessage">The error message returned on failure.</param>
    /// <returns>Returns true if the update succeeds.</returns>
    protected virtual bool DbUpdate_CalcWsLoad(SqlTransaction sqlTransaction, ref string errorMessage)
    {
      bool result;

      result = true;

      return result;
    }

    /// <summary>
    /// The Update ECMPS Status process identifier.
    /// </summary>
    protected virtual string DbUpdate_EcmpsStatusProcess { get { return null; } }

    /// <summary>
    /// The Update ECMPS Status id key or list for the item(s) for which the update will occur.
    /// </summary>
    protected virtual string DbUpdate_EcmpsStatusIdKeyOrList { get { return null; } }

    /// <summary>
    /// The Update ECMPS Status report period id for the item(s) for which the update will occur.
    /// </summary>
    protected virtual int? DbUpdate_EcmpsStatusPeriodId { get { return null; } }

    /// <summary>
    /// The Update ECMPS Status Additional value for the items(s) for which the update will occur..
    /// </summary>
    protected virtual string DbUpdate_EcmpsStatusOtherField { get { return null; } }

    /// <summary>
    /// Determine whether to update calculated values and ECMPS Status.
    /// </summary>
    protected virtual bool DbUpdate_UpdateData { get { return true; } }

    /// <summary>
    /// Returns the WS data type for the process.
    /// </summary>
    /// <returns>The workspace data type for the process, or null for none.</returns>
    protected virtual eWorkspaceDataType? DbUpdate_WorkspaceDataType { get { return null; } }

    #endregion


    #region Protected Methods: DB Update


    /// <summary>
    /// Performs the database updates at the end of a check process.
    /// </summary>
    /// <param name="errorMessage">The error message returned if the call fails.</param>
    /// <returns>Returns true if the call is successful.</returns>
    protected bool DbUpdate(ref string errorMessage)
    {
      bool result;

      SqlTransaction sqlTransaction = mCheckEngine.DbDataConnection.BeginTransaction();

      try
      {
        if (DbUpdate_CheckLog(sqlTransaction, ref errorMessage) &&
            (!DbUpdate_UpdateData ||
             (DbUpdate_CalcWsLoad(sqlTransaction, ref errorMessage) &&
              DbUpdate_CalcWsMigrate(sqlTransaction, ref errorMessage) &&
              DbUpdate_EcmpsStatus(sqlTransaction, ref errorMessage))))
        {
          sqlTransaction.Commit();
          result = true;
        }
        else
        {
          sqlTransaction.Rollback();
          result = false;
        }
      }
      catch (Exception ex)
      {
        errorMessage = ex.Message;
        sqlTransaction.Rollback();
        result = false;
      }

      return result;
    }


    #region Helper Methods

    /// <summary>
    /// Migrates ECMPS_WS calculated values to the ECMPS database.
    /// </summary>
    /// <param name="sqlTransaction">The transaction to use with any commands.  Use null for no transaction.</param>
    /// <param name="errorMessage">The error message returned on failure.</param>
    /// <returns>Returns true if the update succeeds.</returns>
    private bool DbUpdate_CalcWsMigrate(SqlTransaction sqlTransaction, ref string errorMessage)
    {
      string resultTemplate = "DbUpdate_CalcWsMigrate [{0},{1}]: {2}";
      string dbFunction = "MigrateWorkspaceSession";

      bool result;

      try
      {
        if (!DbDataConnection.MigrateWorkspaceSession(DbUpdate_WorkspaceDataType,
                                                      CheckEngine.WorkspaceSessionId,
                                                      CheckEngine.MonPlanId,
                                                      CheckEngine.RptPeriodId,
                                                      CheckEngine.UserId,
                                                      ref sqlTransaction))
        {
          errorMessage = string.Format(resultTemplate, "DB", dbFunction, DbDataConnection.LastError);
          result = false;
        }
        else
        {
          dbFunction = "ClearScratchSession";
          if ((CheckEngine.WorkspaceSessionId != 0) &&
              DbUpdate_WorkspaceDataType.HasValue &&
              !DbDataConnection.ClearScratchSession(DbUpdate_WorkspaceDataType.Value, CheckEngine.WorkspaceSessionId, ref sqlTransaction))
          {
            errorMessage = string.Format(resultTemplate, "DB", dbFunction, DbDataConnection.LastError);
            result = false;
          }
          else
            result = true;
        }
      }
      catch (Exception ex)
      {
        errorMessage = string.Format(resultTemplate, "App", dbFunction, ex.Message);
        result = false;
      }

      return result;
    }

    /// <summary>
    /// Updates the CHECK_LOG for the processed checks.
    /// </summary>
    /// <param name="sqlTransaction">The transaction to use for the SQL command.</param>
    /// <param name="errorMessage">The error message returned if the call fails.</param>
    /// <returns>Returns true if the method is successful.</returns>
    private bool DbUpdate_CheckLog(SqlTransaction sqlTransaction, ref string errorMessage)
    {
      bool result;

      foreach (DataRow checkLogRow in mCheckLogs.Rows)
      {

        if (cDBConvert.ToString(checkLogRow["CHK_LOG_COMMENT"]).Length > 1000)
        {
          checkLogRow.BeginEdit();
          checkLogRow["CHK_LOG_COMMENT"] = cDBConvert.ToString(checkLogRow["CHK_LOG_COMMENT"]).Substring(0, 1000);
          checkLogRow.EndEdit();
        }
      }

      DbUpdate_CheckLogMerge();

      result = mCheckEngine.DbDataConnection.BulkLoad(mCheckLogsMerged,
                                                     "ecmps_aux.dbo.Check_Log",
                                                     new string[] { "CHK_LOG_ID" },
                                                     sqlTransaction,
                                                     ref errorMessage);

      return result;
    }

    /// <summary>
    /// UpdateCheckLog_Merge
    /// </summary>
    private void DbUpdate_CheckLogMerge()
    {
      mCheckLogs.DefaultView.Sort = "MON_LOC_ID, TEST_SUM_ID, " +
                                    "RULE_CHECK_ID, CHECK_CATALOG_RESULT_ID, " +
                                    "RESULT_MESSAGE, CHK_LOG_COMMENT, " +
                                    "SOURCE_TABLE, ROW_ID, " +
                                    "CHECK_DATE, CHECK_HOUR";

      DataRow MergeRow;
      bool Initial = true;
      object TableMonLocId; object BreakMonLocId = DBNull.Value;
      object TableTestSumId; object BreakTestSumId = DBNull.Value;
      object TableRuleCheckId; object BreakRuleCheckId = DBNull.Value;
      object TableCheckCatalogResultId; object BreakCheckCatalogResultId = DBNull.Value;
      object TableResultMessage; object BreakResultMessage = DBNull.Value;
      object TableCheckResult; object BreakCheckResult = DBNull.Value;
      object TableSeverityCd; object BreakSeverityCd = DBNull.Value;
      object TableSuppressedSeverityCd; object BreakSuppressedSeverityCd = DBNull.Value;
      object TableErrorSuppressId; object BreakErrorSuppressId = DBNull.Value;
      object TableChkLogComment; object BreakChkLogComment = DBNull.Value;
      object TableSourceTable; object BreakSourceTable = DBNull.Value;
      object TableRowId; object BreakRowId = DBNull.Value;
      object TableCheckDate; object TableCheckHour;
      object CheckBeganDate = DBNull.Value;
      object CheckBeganHour = DBNull.Value;
      object CheckEndedDate = DBNull.Value;
      object CheckEndedHour = DBNull.Value;
      object LogDate = DateTime.MinValue;

      foreach (DataRowView LogRowView in mCheckLogs.DefaultView)
      {
        TableMonLocId = LogRowView["MON_LOC_ID"];
        TableTestSumId = LogRowView["TEST_SUM_ID"];
        TableRuleCheckId = LogRowView["RULE_CHECK_ID"];
        TableCheckCatalogResultId = LogRowView["CHECK_CATALOG_RESULT_ID"];
        TableResultMessage = LogRowView["RESULT_MESSAGE"];
        TableCheckResult = LogRowView["CHECK_RESULT"];
        TableSeverityCd = LogRowView["SEVERITY_CD"];
        TableSuppressedSeverityCd = LogRowView["SUPPRESSED_SEVERITY_CD"];
        TableErrorSuppressId = LogRowView["ERROR_SUPPRESS_ID"];
        TableChkLogComment = LogRowView["CHK_LOG_COMMENT"];
        TableSourceTable = LogRowView["SOURCE_TABLE"];
        TableRowId = LogRowView["ROW_ID"];
        TableCheckDate = LogRowView["CHECK_DATE"];
        TableCheckHour = LogRowView["CHECK_HOUR"];

        if (!((cDBConvert.ToString(TableMonLocId) == cDBConvert.ToString(BreakMonLocId)) &&
              (cDBConvert.ToString(TableTestSumId) == cDBConvert.ToString(BreakTestSumId)) &&
              (cDBConvert.ToLong(TableRuleCheckId) == cDBConvert.ToLong(BreakRuleCheckId)) &&
              (cDBConvert.ToLong(TableCheckCatalogResultId) == cDBConvert.ToLong(BreakCheckCatalogResultId)) &&
              (cDBConvert.ToString(TableResultMessage) == cDBConvert.ToString(BreakResultMessage)) &&
              (cDBConvert.ToString(TableCheckResult) == cDBConvert.ToString(BreakCheckResult)) &&
              (cDBConvert.ToString(TableSeverityCd) == cDBConvert.ToString(BreakSeverityCd)) &&
              (cDBConvert.ToString(TableSuppressedSeverityCd) == cDBConvert.ToString(BreakSuppressedSeverityCd)) &&
              (cDBConvert.ToLong(TableErrorSuppressId) == cDBConvert.ToLong(BreakErrorSuppressId)) &&
              (cDBConvert.ToString(TableChkLogComment) == cDBConvert.ToString(BreakChkLogComment)) &&
              (cDBConvert.ToString(TableSourceTable) == cDBConvert.ToString(BreakSourceTable)) &&
              (cDBConvert.ToString(TableRowId) == cDBConvert.ToString(BreakRowId)) &&
              ((((TableCheckDate != DBNull.Value) && (TableCheckHour != DBNull.Value) &&
                 (CheckEndedDate != DBNull.Value) && (CheckEndedHour != DBNull.Value)) &&
                ((cDateFunctions.HourDifference(cDBConvert.ToDate(CheckEndedDate, DateTypes.START),
                                                cDBConvert.ToInteger(CheckEndedHour),
                                                cDBConvert.ToDate(TableCheckDate, DateTypes.END),
                                                cDBConvert.ToInteger(TableCheckHour)) == 1) ||
                  (cDateFunctions.HourDifference(cDBConvert.ToDate(CheckEndedDate, DateTypes.START),
                                                 cDBConvert.ToInteger(CheckEndedHour),
                                                 cDBConvert.ToDate(TableCheckDate, DateTypes.END),
                                                 cDBConvert.ToInteger(TableCheckHour)) == 0))) ||
               (((TableCheckDate != DBNull.Value) && (TableCheckHour == DBNull.Value) &&
                 (CheckEndedDate != DBNull.Value) && (CheckEndedHour == DBNull.Value)) &&
                ((cDateFunctions.DateDifference(cDBConvert.ToDate(CheckEndedDate, DateTypes.START),
                                                cDBConvert.ToDate(TableCheckDate, DateTypes.END)) == 1) ||
                 (cDateFunctions.DateDifference(cDBConvert.ToDate(CheckEndedDate, DateTypes.START),
                                                cDBConvert.ToDate(TableCheckDate, DateTypes.END)) == 0))))))
        {
          if (!Initial)
          {
            MergeRow = mCheckLogsMerged.NewRow();

            MergeRow["CHK_SESSION_ID"] = mCheckEngine.ChkSessionId;

            MergeRow["MON_LOC_ID"] = BreakMonLocId;
            MergeRow["TEST_SUM_ID"] = BreakTestSumId;
            MergeRow["RULE_CHECK_ID"] = BreakRuleCheckId;
            MergeRow["CHECK_CATALOG_RESULT_ID"] = BreakCheckCatalogResultId;
            MergeRow["RESULT_MESSAGE"] = BreakResultMessage;
            MergeRow["CHECK_RESULT"] = BreakCheckResult;
            MergeRow["SEVERITY_CD"] = BreakSeverityCd;
            MergeRow["SUPPRESSED_SEVERITY_CD"] = BreakSuppressedSeverityCd;
            MergeRow["ERROR_SUPPRESS_ID"] = BreakErrorSuppressId;
            MergeRow["CHK_LOG_COMMENT"] = BreakChkLogComment;
            MergeRow["SOURCE_TABLE"] = BreakSourceTable;
            MergeRow["ROW_ID"] = BreakRowId;

            MergeRow["BEGIN_DATE"] = LogDate;

            MergeRow["OP_BEGIN_DATE"] = CheckBeganDate;
            MergeRow["OP_BEGIN_HOUR"] = CheckBeganHour;
            MergeRow["OP_END_DATE"] = CheckEndedDate;
            MergeRow["OP_END_HOUR"] = CheckEndedHour;

            mCheckLogsMerged.Rows.Add(MergeRow);
          }

          BreakMonLocId = TableMonLocId;
          BreakTestSumId = TableTestSumId;
          BreakRuleCheckId = TableRuleCheckId;
          BreakCheckCatalogResultId = TableCheckCatalogResultId;
          BreakResultMessage = TableResultMessage;
          BreakCheckResult = TableCheckResult;
          BreakSeverityCd = TableSeverityCd;
          BreakSuppressedSeverityCd = TableSuppressedSeverityCd;
          BreakErrorSuppressId = TableErrorSuppressId;
          BreakChkLogComment = TableChkLogComment;
          BreakSourceTable = TableSourceTable;
          BreakRowId = TableRowId;

          CheckBeganDate = TableCheckDate;
          CheckBeganHour = TableCheckHour;
        }

        LogDate = LogRowView["BEGIN_DATE"];
        CheckEndedDate = TableCheckDate;
        CheckEndedHour = TableCheckHour;

        Initial = false;
      }

      if (!Initial)
      {
        MergeRow = mCheckLogsMerged.NewRow();

        MergeRow["CHK_SESSION_ID"] = mCheckEngine.ChkSessionId;

        MergeRow["MON_LOC_ID"] = BreakMonLocId;
        MergeRow["TEST_SUM_ID"] = BreakTestSumId;
        MergeRow["RULE_CHECK_ID"] = BreakRuleCheckId;
        MergeRow["CHECK_CATALOG_RESULT_ID"] = BreakCheckCatalogResultId;
        MergeRow["RESULT_MESSAGE"] = BreakResultMessage;
        MergeRow["CHECK_RESULT"] = BreakCheckResult;
        MergeRow["SEVERITY_CD"] = BreakSeverityCd;
        MergeRow["SUPPRESSED_SEVERITY_CD"] = BreakSuppressedSeverityCd;
        MergeRow["ERROR_SUPPRESS_ID"] = BreakErrorSuppressId;
        MergeRow["CHK_LOG_COMMENT"] = BreakChkLogComment;
        MergeRow["SOURCE_TABLE"] = BreakSourceTable;
        MergeRow["ROW_ID"] = BreakRowId;

        MergeRow["BEGIN_DATE"] = LogDate;

        MergeRow["OP_BEGIN_DATE"] = CheckBeganDate;
        MergeRow["OP_BEGIN_HOUR"] = CheckBeganHour;
        MergeRow["OP_END_DATE"] = CheckEndedDate;
        MergeRow["OP_END_HOUR"] = CheckEndedHour;

        mCheckLogsMerged.Rows.Add(MergeRow);
      }

      mCheckLogs.DefaultView.Sort = "";
    }

    /// <summary>
    /// Calls the ECMPS UPDATE_ECMPS_STATUS stored procedure.
    /// </summary>
    /// <param name="sqlTransaction">The transaction to use for the SQL command.</param>
    /// <param name="errorMessage">The error message returned if the call fails.</param>
    /// <returns>Returns true if the method is successful.</returns>
    private bool DbUpdate_EcmpsStatus(SqlTransaction sqlTransaction, ref string errorMessage)
    {
      string resultTemplate = "DbUpdate_EcmpsStatus [{0}]: {1}";

      bool result;

      if (DbUpdate_EcmpsStatusProcess != null)
      {
        DbDataConnection.CreateStoredProcedureCommand("UPDATE_ECMPS_STATUS", sqlTransaction);

        DbDataConnection.AddInputParameter("@V_PROCESS", DbUpdate_EcmpsStatusProcess);
        DbDataConnection.AddInputParameter("@V_ID_KEY_OR_LIST", DbUpdate_EcmpsStatusIdKeyOrList);
        DbDataConnection.AddInputParameter("@V_PERIOD_ID", DbUpdate_EcmpsStatusPeriodId);
        DbDataConnection.AddInputParameter("@V_OTHER_FIELD", DbUpdate_EcmpsStatusOtherField);

        DbDataConnection.AddOutputParameterString("@V_RESULT", 1);
        DbDataConnection.AddOutputParameterString("@V_ERROR_MSG", 200);

        try
        {
          DbDataConnection.ExecuteNonQuery();

          if (DbDataConnection.GetParameterString("@V_RESULT") == "T")
            result = true;
          else
          {
            errorMessage = string.Format(resultTemplate, "DB", DbDataConnection.GetParameterString("@V_ERROR_MSG"));
            result = false;
          }
        }
        catch (Exception ex)
        {
          if ((DbDataConnection.GetParameterString("@V_RESULT") == "F") &&
               DbDataConnection.GetParameterString("@V_ERROR_MSG").HasValue())
          {
            errorMessage = string.Format(resultTemplate, "DB/App", DbDataConnection.GetParameterString("@V_ERROR_MSG"));
            result = false;
          }
          else
          {
            errorMessage = string.Format(resultTemplate, "App", ex.Message);
            result = false;
          }
        }
      }
      else
        result = true;

      return result;
    }

    #endregion

    #endregion


    #region Protected Methods: Clone Table with support methods

    private enum EDataType { StringType, IntegerType, NumericType, DateTimeType, UnknownType };
    private enum EGetResults { FoundRows, NoRows, ErrorOccurred };

    /// <summary>
    /// CloneTable
    /// </summary>
    /// <param name="ACatalogName"></param>
    /// <param name="ATableName"></param>
    /// <param name="AConnection"></param>
    /// <param name="AErrorMessage"></param>
    /// <returns></returns>
    protected DataTable CloneTable(string ACatalogName, string ATableName, SqlConnection AConnection,
                                 ref string AErrorMessage)
    {
      DataTable Table = null;
      DataTable Columns = null;
      DataColumn Column;
      EDataType DataType;

      if (CloneTable_GetColumns(ACatalogName, ATableName, AConnection,
                                ref Columns, ref AErrorMessage))
      {
        Table = new DataTable(ATableName);

        foreach (DataRow ColumnRow in Columns.Rows)
        {
          Column = new DataColumn();
          Column.ColumnName = cDBConvert.ToString(ColumnRow["Column_Name"]);
          Column.AllowDBNull = true; //cDBConvert.ToBoolean(ColumnRow["Is_Nullable"]);

          DataType = CloneTable_GetDataType(cDBConvert.ToString(ColumnRow["Data_Type"]));

          if (DataType == EDataType.DateTimeType)
          {
            Column.DataType = System.Type.GetType("System.DateTime");
          }
          else if (DataType == EDataType.IntegerType)
          {
            Column.DataType = CloneTable_GetDataType_Integer(cDBConvert.ToInteger(ColumnRow["Numeric_Precision"]));
          }
          else if (DataType == EDataType.NumericType)
          {
            Column.DataType = CloneTable_GetDataType_Numeric(cDBConvert.ToInteger(ColumnRow["Numeric_Precision"]), cDBConvert.ToInteger(ColumnRow["Numeric_Scale"]));
          }
          else if (DataType == EDataType.StringType)
          {
            Column.DataType = System.Type.GetType("System.String");
            Column.MaxLength = cDBConvert.ToInteger(ColumnRow["Character_Maximum_Length"]);
          }
          else
          {
            Column.DataType = System.Type.GetType("System.String");
            Column.MaxLength = 4000;
          }

          Table.Columns.Add(Column);
        }
      }

      return Table;
    }

    private EGetResults CloneTable_ExecuteSql(string ASql, SqlConnection AConnection,
                                              ref DataTable ADataTable, ref string AErrorMessage)
    {
      SqlDataAdapter DataAdapter = new SqlDataAdapter(ASql, AConnection);
      // this defaults to 30 seconds if we don't override it
      if (DataAdapter.SelectCommand != null)
        DataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
      DataSet Dataset = new DataSet();

      try
      {
        DataAdapter.Fill(Dataset);

        if ((Dataset.Tables.Count > 0) && (Dataset.Tables[0].Rows.Count > 0))
        {
          ADataTable = Dataset.Tables[0];
          return EGetResults.FoundRows;
        }
        else
        {
          AErrorMessage = "Getting results returned no rows.";
          return EGetResults.NoRows;
        }
      }
      catch (Exception ex)
      {
        AErrorMessage = "Getting results raised exception '" + ex.Message + "'.";
        return EGetResults.ErrorOccurred;
      }
    }

    private bool CloneTable_GetColumns(string ACatalogName, string ATableName,
                                       SqlConnection AConnection,
                                       ref DataTable AColumns, ref string AErrorMessage)
    {
      string Sql = "SELECT Ordinal_Position, Column_Name, Is_Nullable, " +
                   "       Data_Type, Character_Maximum_Length, " +
                   "       Numeric_Precision, Numeric_Scale, Datetime_Precision " +
                   "  FROM " + ACatalogName + ".Information_Schema.Columns " +
                   "  WHERE Table_Name = '" + ATableName + "' " +
                   "  ORDER BY Ordinal_Position, Column_Name ";
      EGetResults GetResults;

      GetResults = CloneTable_ExecuteSql(Sql, AConnection, ref AColumns, ref AErrorMessage);

      if (GetResults == EGetResults.FoundRows)
      {
        return true;
      }
      else if (GetResults == EGetResults.NoRows)
      {
        AErrorMessage = "[CloneTable_GetColumns]: Columns do not exist for table " + ATableName + ".";
        return false;
      }
      else
      {
        AErrorMessage = "[CloneTable_GetColumns]: " + AErrorMessage;
        return false;
      }
    }

    private EDataType CloneTable_GetDataType(string ASqlDataType)
    {
      ASqlDataType = ASqlDataType.ToLower();

      if (ASqlDataType == "char")
        return EDataType.StringType;
      else if (ASqlDataType == "nvarchar")
        return EDataType.StringType;
      else if (ASqlDataType == "text")
        return EDataType.StringType;
      else if (ASqlDataType == "varchar")
        return EDataType.StringType;
      else if (ASqlDataType == "datetime")
        return EDataType.DateTimeType;
      else if (ASqlDataType == "smalldatetime")
        return EDataType.DateTimeType;
      else if (ASqlDataType == "int")
        return EDataType.IntegerType;
      else if (ASqlDataType == "numeric")
        return EDataType.NumericType;
      else
        return EDataType.UnknownType;
    }

    private System.Type CloneTable_GetDataType_Integer(int ASqlNumericPrecision)
    {
      if (ASqlNumericPrecision <= 4)
        return System.Type.GetType("System.Int16");
      else if (ASqlNumericPrecision <= 9)
        return System.Type.GetType("System.Int32");
      else
        return System.Type.GetType("System.Int64");
    }

    private System.Type CloneTable_GetDataType_Numeric(int ASqlNumericPrecision, int ASqlNumericScale)
    {
      if (ASqlNumericScale == 0)
        return CloneTable_GetDataType_Integer(ASqlNumericPrecision);
      else if (ASqlNumericPrecision <= 28)
        return System.Type.GetType("System.Decimal");
      else
        return System.Type.GetType("System.Double");
    }

    #endregion


    #region Protected Methods

    /// <summary>
    /// Add a data to the mSourceData data set
    /// </summary>
    /// <param name="sTableNm">The name of the table</param>
    /// <param name="sSQL">The to use to fill the table</param>
    protected virtual void AddSourceData(string sTableNm, string sSQL)
    {
      DataTable dtSourceDataTable = new DataTable(sTableNm);
      SqlDataAdapter SourceDataAdapter = new SqlDataAdapter(sSQL, mCheckEngine.DbDataConnection.SQLConnection);
      // this defaults to 30 seconds if we don't override it
      if (SourceDataAdapter.SelectCommand != null)
        SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
      SourceDataAdapter.Fill(dtSourceDataTable);
      mSourceData.Tables.Add(dtSourceDataTable);
    }

    /// <summary>
    /// Add a DataTable to mSourceData, using ECMPSConnection in mCheckEngine
    /// </summary>
    /// <param name="sDataTableNm">The name of the new DataTable</param>
    /// <param name="sViewNm">The view to get data from</param>
    protected void AddSourceDataTable(string sDataTableNm, string sViewNm)
    {
      AddSourceDataTable(sDataTableNm, sViewNm, null, null);
    }

    /// <summary>
    /// Add a DataTable to mSourceData, using ECMPSConnection in mCheckEngine
    /// </summary>
    /// <param name="sDataTableNm">The name of the new DataTable</param>
    /// <param name="sViewNm">The view to get data from</param>
    /// <param name="sWhereClause">A where clause, if any</param>
    protected void AddSourceDataTable(string sDataTableNm, string sViewNm, string sWhereClause)
    {
      AddSourceDataTable(sDataTableNm, sViewNm, sWhereClause, null);
    }

    /// <summary>
    /// Add a DataTable to mSourceData, using ECMPSConnection in mCheckEngine
    /// </summary>
    /// <param name="sDataTableNm">The name of the new DataTable</param>
    /// <param name="sViewNm">The view to get data from</param>
    /// <param name="sWhereClause">A where clause, if any</param>
    /// <param name="sOrderClause">An order clause, if any</param>
    protected void AddSourceDataTable(string sDataTableNm, string sViewNm, string sWhereClause, string sOrderClause)
    {
      SqlDataAdapter SourceDataAdapter;
      DataTable SourceDataTable;

      string sSQL = string.Format("SELECT * FROM {0} ", sViewNm);
      if (string.IsNullOrEmpty(sWhereClause) == false)
        sSQL += " " + sWhereClause;
      if (string.IsNullOrEmpty(sOrderClause) == false)
        sSQL += " " + sOrderClause;

      SourceDataTable = new DataTable(sDataTableNm);
      SourceDataAdapter = new SqlDataAdapter(sSQL, mCheckEngine.DbDataConnection.SQLConnection);
      // this defaults to 30 seconds if we don't override it
      if (SourceDataAdapter.SelectCommand != null)
        SourceDataAdapter.SelectCommand.CommandTimeout = mCheckEngine.CommandTimeout;
      SourceDataAdapter.Fill(SourceDataTable);
      mSourceData.Tables.Add(SourceDataTable);
    }

    /// <summary>
    /// Get the FAC_ID for the current MON_PLAN_ID
    /// </summary>
    /// <returns></returns>
    protected long GetFacilityID()
    {
      return cDBConvert.ToInteger(mCheckEngine.DbDataConnection.ExecuteScalar("SELECT FAC_ID FROM MONITOR_PLAN WHERE MON_PLAN_ID = '" + mCheckEngine.MonPlanId + "'"));
    }

    /// <summary>
    /// InitLogs
    /// </summary>
    protected void InitLogs()
    {
      mCheckLogs.Columns.Add("CHK_LOG_ID");
      mCheckLogs.Columns.Add("CHK_SESSION_ID");
      mCheckLogs.Columns.Add("BEGIN_DATE", System.Type.GetType("System.DateTime"));
      mCheckLogs.Columns.Add("RULE_CHECK_ID", System.Type.GetType("System.Int32"));
      mCheckLogs.Columns.Add("RESULT_MESSAGE");
      mCheckLogs.Columns.Add("CHK_LOG_COMMENT");
      mCheckLogs.Columns.Add("CHECK_CATALOG_RESULT_ID", System.Type.GetType("System.Int32"));
      mCheckLogs.Columns.Add("MON_LOC_ID");
      mCheckLogs.Columns.Add("TEST_SUM_ID");
      mCheckLogs.Columns.Add("SOURCE_TABLE");
      mCheckLogs.Columns.Add("ROW_ID");
      mCheckLogs.Columns.Add("CHECK_DATE", System.Type.GetType("System.DateTime"));
      mCheckLogs.Columns.Add("CHECK_HOUR", System.Type.GetType("System.Int32"));
      mCheckLogs.Columns.Add("CHECK_RESULT");
      mCheckLogs.Columns.Add("SEVERITY_CD");
      mCheckLogs.Columns.Add("SUPPRESSED_SEVERITY_CD");
      mCheckLogs.Columns.Add("ERROR_SUPPRESS_ID", System.Type.GetType("System.Int32"));

      //The order of these columns must match the order of the columns in the Check Log table.
      mCheckLogsMerged.Columns.Add("CHK_LOG_ID");
      mCheckLogsMerged.Columns.Add("CHK_SESSION_ID");
      mCheckLogsMerged.Columns.Add("BEGIN_DATE", System.Type.GetType("System.DateTime"));
      mCheckLogsMerged.Columns.Add("RULE_CHECK_ID", System.Type.GetType("System.Int32"));
      mCheckLogsMerged.Columns.Add("RESULT_MESSAGE");
      mCheckLogsMerged.Columns.Add("CHK_LOG_COMMENT");
      mCheckLogsMerged.Columns.Add("CHECK_CATALOG_RESULT_ID", System.Type.GetType("System.Int32"));
      mCheckLogsMerged.Columns.Add("MON_LOC_ID");
      mCheckLogsMerged.Columns.Add("TEST_SUM_ID");
      mCheckLogsMerged.Columns.Add("OP_BEGIN_DATE", System.Type.GetType("System.DateTime"));
      mCheckLogsMerged.Columns.Add("OP_BEGIN_HOUR", System.Type.GetType("System.Int32"));
      mCheckLogsMerged.Columns.Add("OP_END_DATE", System.Type.GetType("System.DateTime"));
      mCheckLogsMerged.Columns.Add("OP_END_HOUR", System.Type.GetType("System.Int32"));
      mCheckLogsMerged.Columns.Add("SOURCE_TABLE");
      mCheckLogsMerged.Columns.Add("ROW_ID");
      mCheckLogsMerged.Columns.Add("CHECK_DATE", System.Type.GetType("System.DateTime"));
      mCheckLogsMerged.Columns.Add("CHECK_HOUR", System.Type.GetType("System.Int32"));
      mCheckLogsMerged.Columns.Add("CHECK_RESULT");
      mCheckLogsMerged.Columns.Add("SEVERITY_CD");
      mCheckLogsMerged.Columns.Add("SUPPRESSED_SEVERITY_CD");
      mCheckLogsMerged.Columns.Add("ERROR_SUPPRESS_ID", System.Type.GetType("System.Int32"));
    }

    #endregion


    #region Execute Severity and Error: Methods, Properties and Fields

    private int mSeverityCountCritical1 = 0;
    private int mSeverityCountCritical2 = 0;
    private int mSeverityCountCritical3 = 0;
    private int mSeverityCountException = 0;
    private int mSeverityCountFatal = 0;
    private int mSeverityCountAdminOverride = 0;
    private int mSeverityCountInformation = 0;
    private int mSeverityCountNonCritical = 0;
    private int mSeverityCountNone = 0;

    /// <summary>
    /// The Severity Code resulting from executing the processes checks.
    /// </summary>
    public eSeverityCd SeverityCd { get; private set; }

    private int SeverityCountUpdate(eSeverityCd ASeverityCd)
    {
      switch (ASeverityCd)
      {
        case eSeverityCd.EXCEPTION: return mSeverityCountException += 1;
        case eSeverityCd.FATAL: return mSeverityCountFatal += 1;
        case eSeverityCd.CRIT1: return mSeverityCountCritical1 += 1;
        case eSeverityCd.CRIT2: return mSeverityCountCritical2 += 1;
        case eSeverityCd.CRIT3: return mSeverityCountCritical3 += 1;
        case eSeverityCd.NONCRIT: return mSeverityCountNonCritical += 1;
        case eSeverityCd.INFORM: return mSeverityCountInformation += 1;
        case eSeverityCd.ADMNOVR: return mSeverityCountAdminOverride += 1;
        case eSeverityCd.NONE: return mSeverityCountNone += 1;
        default: return int.MinValue;
      }
    }

    private eSeverityCd SeverityLevelUpdate(eSeverityCd ASeverityCd)
    {
      if (ASeverityCd.GetSeverityLevel() > SeverityCd.GetSeverityLevel())
      {
        SeverityCd = ASeverityCd;
      }

      return SeverityCd;
    }

    /// <summary>
    /// UpdateErrors
    /// </summary>
    /// <param name="AErrorText"></param>
    public void UpdateErrors(string AErrorText)
    {
      CheckEngine.HandleProcessingError(AErrorText);
    }

    /// <summary>
    /// UpdateSeverity
    /// </summary>
    /// <param name="ASeverityCd"></param>
    /// <returns></returns>
    public void UpdateSeverity(eSeverityCd ASeverityCd)
    {
      SeverityLevelUpdate(ASeverityCd);
      SeverityCountUpdate(ASeverityCd);
    }

    #endregion

  }
}
