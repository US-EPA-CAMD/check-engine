using System;
using System.Collections;
using System.Data;
using System.Diagnostics;

using ECMPS.Checks.CheckEngine.Definitions;
using ECMPS.Checks.DatabaseAccess;
using ECMPS.Checks.Parameters;
using ECMPS.Checks.TypeUtilities;
using ECMPS.Common;
using ECMPS.Definitions.SeverityCode;
using ECMPS.ErrorSuppression;
using ECMPS.Definitions.Extensions;


namespace ECMPS.Checks.CheckEngine
{
    /// <summary>
    /// Category base class
    /// </summary>
    public abstract class cCategory : cCheckCategory
    {

        #region Constructors with Support and Additional Methods

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="checkEngine"></param>
        /// <param name="process"></param>
        /// <param name="categoryCd"></param>
        public cCategory(cCheckEngine checkEngine, cProcess process, string categoryCd)
          : base(process, categoryCd)
        {
            Initialize(checkEngine, process);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="checkEngine"></param>
        /// <param name="process"></param>
        /// <param name="categoryCd"></param>
        /// <param name="thisTable"></param>
        public cCategory(cCheckEngine checkEngine, cProcess process, string categoryCd, DataTable thisTable)
          : base(process, categoryCd)
        {
            ThisTable = thisTable;

            Initialize(checkEngine, process);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="checkEngine"></param>
        /// <param name="process"></param>
        /// <param name="parentCategory"></param>
        /// <param name="categoryCd"></param>
        public cCategory(cCheckEngine checkEngine, cProcess process, cCategory parentCategory, string categoryCd)
          : base(process, categoryCd, parentCategory)
        {
            Initialize(checkEngine, process);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="checkEngine"></param>
        /// <param name="process"></param>
        /// <param name="parentCategory"></param>
        /// <param name="categoryCd"></param>
        /// <param name="thisTable"></param>
        public cCategory(cCheckEngine checkEngine, cProcess process, cCategory parentCategory, string categoryCd, DataTable thisTable)
          : base(process, categoryCd, parentCategory)
        {
            ThisTable = thisTable;

            Initialize(checkEngine, process);
        }

        /// <summary>
        /// Created a category object based on its category code and parent category object.
        /// </summary>
        /// <param name="parentCategory">Category object for the parent category.</param>
        /// <param name="categoryCd">Category code for the child category object to construct.</param>
        public cCategory(cCategory parentCategory, string categoryCd)
          : base(parentCategory.Process, categoryCd, parentCategory)
        {
            Initialize(parentCategory.Process.CheckEngine, parentCategory.Process);
        }


        /// <summary>
        /// Instantiates a cCategory object primarily for unit testing purposes.
        /// </summary>
        protected cCategory()
        {
        }


        #region Helper Methods

        /// <summary>
        /// Groups initialiation every constructor must perform
        /// </summary>
        /// <param name="checkEngine"></param>
        /// <param name="process"></param>
        private void Initialize(cCheckEngine checkEngine, cProcess process)
        {
            CheckEngine = checkEngine;
            Process = process;
            StopWatchFilterData = new Stopwatch();
            StopWatchProcessChecksDo = new Stopwatch();
            SeverityCd = eSeverityCd.NONE;
        }

        /// <summary>
        /// Initializes the current properties corresponding to the passed values.
        /// </summary>
        /// <param name="monLocId">The monitor location id to initialize.</param>
        protected void InitializeCurrent(string monLocId)
        {
            CurrentMonLocId = monLocId;
            CurrentTestSumId = null;
        }

        /// <summary>
        /// Initializes the current properties corresponding to the passed values.
        /// </summary>
        /// <param name="monLocId">The monitor location id to initialize.</param>
        /// <param name="testSumId">The test summary id to initialize.</param>
        protected void InitializeCurrent(string monLocId, string testSumId)
        {
            CurrentMonLocId = monLocId;
            CurrentTestSumId = testSumId;
        }

        #endregion

        #endregion


        #region Public Properties

        /// <summary>
        /// Check Catalog Result setting for the previously executed check.
        /// </summary>
        public string CheckCatalogResult { get; set; }

        /// <summary>
        /// the check engine object
        /// </summary>
        public cCheckEngine CheckEngine { get; protected set; }

        /// <summary>
        /// check parameters hashtable
        /// </summary>
        public Hashtable CheckParameters { get; protected set; }

        /// <summary>
        /// current MON_LOC_ID
        /// </summary>
        public string CurrentMonLocId { get; protected set; }

        /// <summary>
        /// Location name of the monitor location record begin processed.
        /// </summary>
        public string CurrentMonLocName { get; protected set; }

        /// <summary>
        /// Current location position
        /// </summary>
        public int CurrentMonLocPos { get; protected set; }

        /// <summary>
        /// Current OP_DATE
        /// </summary>
        public DateTime CurrentOpDate { get; protected set; }

        /// <summary>
        /// Current OP_HOUR
        /// </summary>
        public int CurrentOpHour { get; protected set; }

        /// <summary>
        /// Current ROW_ID
        /// </summary>
        public string CurrentRowId { get; protected set; }

        /// <summary>
        /// Current TEST_SUM_ID
        /// </summary>
        public string CurrentTestSumId { get; protected set; }

        /// <summary>
        /// The Error Suppression Values for the Category.
        /// </summary>
        public cErrorSuppressValues ErrorSuppressValues { get; protected set; }

        /// <summary>
        /// The execute severity
        /// </summary>
        [Obsolete("Please consider use of new property SeverityCd", false)]
        public string ExecuteSeverity { get { return SeverityCd.ToStringValue(); } }

        /// <summary>
        /// The date/hour to use when logging errors instead of the current date and hour.
        /// </summary>
        public DateTime? LogDateHour { get; set; }

        /// <summary>
        /// Process object
        /// </summary>
        public cProcess Process { get; protected set; }

        /// <summary>
        /// Severity code resulting from running the category's checks.
        /// </summary>
        public eSeverityCd SeverityCd { get; private set; }

        /// <summary>
        /// The StopWatch to get the timing for the FilterData() method
        /// </summary>
        public Stopwatch StopWatchFilterData { get; private set; }

        /// <summary>
        /// The Stopwatch to get the timing for the ProcessChecksDo() method
        /// </summary>
        public Stopwatch StopWatchProcessChecksDo { get; private set; }

        #endregion


        #region Public Virtual Properties

        /// <summary>
        /// Missing data borders?
        /// </summary>
        public virtual cModcDataBorders MissingDataBorders
        {
            get { return null; }
        }

        /// <summary>
        /// MODC Hour Count Object
        /// </summary>
        public virtual cModcHourCounts ModcHourCounts
        {
            get { return null; }
        }

        #endregion


        #region Protected Properties

        /// <summary>
        /// this table
        /// </summary>
        protected DataTable ThisTable { get; set; }

        /// <summary>
        /// The record id
        /// </summary>
        protected string RecordIdentifier { get; set; }

        /// <summary>
        /// the table name
        /// </summary>
        protected string TableName { get; set; }

        #endregion


        #region Private Fields

        cCheckParameterBands CheckParameterBands;

        #endregion


        #region Public Methods: Process Checks with Helper Methods

        /// <summary>
        /// Sets the current record identifier, filters the data and proecesses the checks for this 
        /// category by looping through the check band levels' checks.  Check for failed check conditions 
        /// and un-set required parameters for the checks and prevents the checks from executing that fail 
        /// conditions or are missing required parameters.
        /// </summary>
        /// <returns>Returns true if each check executed without problems.</returns>
        protected bool ProcessChecks()
        {
            try
            {
                return ProcessChecksDo_WithPrep();
            }
            catch (Exception ex)
            {
                Process.UpdateErrors(string.Format("Category: {0}  Message: {1}",
                                                   this.CategoryCd, ex.Message));
                return false;
            }
        }

        /// <summary>
        /// Sets the current record identifier, filters the data and proecesses the checks for this 
        /// category by looping through the check band levels' checks.  Check for failed check conditions 
        /// and un-set required parameters for the checks and prevents the checks from executing that fail 
        /// conditions or are missing required parameters.
        /// </summary>
        /// <param name="AMonitorLocationId">The current monitor locaiton id.</param>
        /// <returns>Returns true if each check executed without problems.</returns>
        protected bool ProcessChecks(string AMonitorLocationId)
        {
            try
            {
                CurrentMonLocId = AMonitorLocationId;

                return ProcessChecksDo_WithPrep();
            }
            catch (Exception ex)
            {
                CurrentMonLocId = AMonitorLocationId;
                Process.UpdateErrors(string.Format("Category: {0}  MonLocId: {1}  Message: {2}",
                                                   this.CategoryCd, AMonitorLocationId, ex.Message));
                return false;
            }
        }

        /// <summary>
        /// Sets the current record identifier, filters the data and proecesses the checks for this 
        /// category by looping through the check band levels' checks.  Check for failed check conditions 
        /// and un-set required parameters for the checks and prevents the checks from executing that fail 
        /// conditions or are missing required parameters.
        /// </summary>
        /// <param name="AMonitorLocationId">The current monitor locaiton id.</param>
        /// <param name="ATestSummaryId">The current test summary id.</param>
        /// <returns>Returns true if each check executed without problems.</returns>
        protected bool ProcessChecks(string AMonitorLocationId, string ATestSummaryId)
        {
            try
            {
                CurrentMonLocId = AMonitorLocationId;
                CurrentTestSumId = ATestSummaryId;

                return ProcessChecksDo_WithPrep();
            }
            catch (Exception ex)
            {
                Process.UpdateErrors(string.Format("Category: {0}  MonLocId: {1}  TestSumId: {2}  Message: {3}",
                                                   this.CategoryCd, AMonitorLocationId, ATestSummaryId, ex.Message));
                return false;
            }
        }

        /// <summary>
        /// Sets the current record identifier, filters the data and proecesses the checks for this 
        /// category by looping through the check band levels' checks.  Check for failed check conditions 
        /// and un-set required parameters for the checks and prevents the checks from executing that fail 
        /// conditions or are missing required parameters.
        /// </summary>
        /// 
        /// <returns>Returns true if each check executed without problems.</returns>
        protected bool ProcessChecksDo_WithPrep()
        {
            bool Result = true;

            try
            {
                Process.SetStaticParameterCategory(this);

                try
                {
                    StopWatchFilterData.Start();
                    FilterData();
                }
                finally
                {
                    StopWatchFilterData.Stop();
                }

                SetRecordIdentifier();
                HandleSetErrorSuppressValues();

                try
                {
                    StopWatchProcessChecksDo.Start();
                    Result = ProcessChecksDo();
                }
                finally
                {
                    StopWatchProcessChecksDo.Stop();
                }
            }
            finally
            {
                Process.SetStaticParameterCategory(null);
            }

            return Result;
        }

        /// <summary>
        /// Proecesses the checks for this category by looping through the check band levels' checks.
        /// Check for failed check conditions and un-set required parameters for the checks and prevents
        /// the checks from executing that fail conditions or are missing required parameters.
        /// </summary>
        /// 
        /// <returns>Returns true if each check executed without problems.</returns>
        private bool ProcessChecksDo()
        {
            if (Process.ProcessCd != "HOURLY")
            {
                System.Diagnostics.Debug.WriteLine("");
                System.Diagnostics.Debug.WriteLine(string.Format("Check Category: {0}", this.CategoryCd));
            }

            string ErrorMessage = "";
            bool RunResult = true;
            int CheckCnt = 0;

            // Main Processing Loop
            for (int BandDex = 0; BandDex < CheckParameterBands.BandCount; BandDex++)
            {
                int Level = BandDex + 1;

                for (int CheckDex = 0; CheckDex < CheckParameterBands.GetCheckCount(BandDex); CheckDex++)
                {
                    cParameterizedCheck RuleCheck = CheckParameterBands.GetCheck(BandDex, CheckDex);

                    CheckCnt += 1;
                    eSeverityCd SeverityCd;

                    if (RuleCheck.EvaluateCheckConditions())
                    {
                        bool SkipCheck = false;
                        string MissingParameters = "";

                        foreach (cCheckParameter RequiredParameter in RuleCheck.CheckParametersInputRequired)
                        {
                            if (!RequiredParameter.IsSet) // Check for missing required check
                            {
                                SkipCheck = true;

                                if (CheckEngine.DebugMode) MissingParameters = MissingParameters.ListAdd(RequiredParameter.Name);

                                if ((CategoryCd != "OPHOUR") || (GetCheckParameter("Derived_Hourly_Checks_Needed").ValueAsBool()))
                                    System.Diagnostics.Debug.WriteLine(string.Format("[Check: {0}-{1}, Parameter: {2}] - missing parameter  (Category: {3}, Id: {4})",
                                                                       RuleCheck.CheckTypeCd, RuleCheck.CheckNumber, RequiredParameter.Name,
                                                                       this.CategoryCd, this.RecordIdentifier));
                            }
                        }

                        if (!SkipCheck)
                        {
                            RunResult = Run(RuleCheck, ref ErrorMessage);

                            if (!RunResult) break;
                        }
                        else if (CheckEngine.DebugMode == true)
                        {
                            Process.LogError(string.Format("{0}-{1}: Input Parameter(s) Missing: {2}", RuleCheck.CheckTypeCd, RuleCheck.CheckNumber, MissingParameters), RuleCheck,
                                              CurrentMonLocId, CurrentTestSumId, CurrentOpDate, CurrentOpHour,
                                              TableName, CurrentRowId, RecordIdentifier, this, out SeverityCd);
                        }
                    }
                    else if (CheckEngine.DebugMode == true)
                    {
                        Process.LogError(string.Format("{0}-{1}: Condition(s) not met", RuleCheck.CheckTypeCd, RuleCheck.CheckNumber), RuleCheck,
                                          CurrentMonLocId, CurrentTestSumId, CurrentOpDate, CurrentOpHour,
                                          TableName, CurrentRowId, RecordIdentifier, this, out SeverityCd);
                    }
                }

                if (!RunResult) break;
            }

            if (Process.ProcessCd != "HOURLY")
                System.Diagnostics.Debug.WriteLine("");

            return RunResult;
        }

        /// <summary>
        /// Run the category
        /// </summary>
        /// <param name="ARuleCheck">rule check object</param>
        /// <param name="AErrorMessage">any error messages produced</param>
        /// <returns>true if successful, false if not</returns>
        protected bool Run(cParameterizedCheck ARuleCheck, ref string AErrorMessage)
        {
            try
            {
                bool RunResult;

                string CheckProcedureName = ARuleCheck.CheckTypeCd + ARuleCheck.CheckNumber.ToString();
                //object CheckTypeObject = null;
                //MethodInfo CheckProcedureInfo = null;

                bool Log = true;
                eSeverityCd SeverityCd;

                //Type[] paramTypes = new Type[2];
                //Object[] parameters = new Object[2];

                RunResult = true;
                AErrorMessage = "";

                FRunningRuleCheck = ARuleCheck;

                try
                {
                    //Execute the Check Procedure
                    object InvokeResult;

                    CheckCatalogResult = null;
                    LogDateHour = null;

                    //Process.ElapsedTimes.TimingBegin(RuleCheck.CheckTypeCd + RuleCheck.CheckNumber.ToString(), this);

                    cChecks.dCheckProcedure CheckProcedure;

                    if (Process.CheckProcedureGet(ARuleCheck, out CheckProcedure, ref AErrorMessage))
                    {
                        //Process.ElapsedTimes.TimingBegin("Checks", "ProcessDo_RunDo_" + CheckProcedureName, this);
                        InvokeResult = CheckProcedure(this, ref Log);
                        AErrorMessage = Convert.ToString(InvokeResult);

                        if (string.IsNullOrEmpty(AErrorMessage))
                        {
                            if (Log)
                            {
                                RunResult = Process.LogResult(ARuleCheck,
                                                               CurrentMonLocId, CurrentTestSumId, CurrentOpDate, CurrentOpHour,
                                                               TableName, CurrentRowId, CheckCatalogResult, RecordIdentifier,
                                                               LogDateHour,
                                                               this, out SeverityCd);

                                this.UpdateSeverity(SeverityCd);
                            }
                            else if (CheckEngine.DebugMode)
                            {
                                RunResult = Process.LogDebug(string.Format("{0}-{1}: In-check condition(s) not met", ARuleCheck.CheckTypeCd, ARuleCheck.CheckNumber), ARuleCheck,
                                                              CurrentMonLocId, CurrentTestSumId, CurrentOpDate, CurrentOpHour,
                                                              TableName, CurrentRowId, RecordIdentifier, this);
                            }
                        }
                        else
                        {
                            Process.UpdateErrors(AErrorMessage);

                            RunResult = Process.LogError(AErrorMessage, ARuleCheck,
                                                           CurrentMonLocId, CurrentTestSumId, CurrentOpDate, CurrentOpHour,
                                                           TableName, CurrentRowId, RecordIdentifier, this, out SeverityCd);

                            System.Diagnostics.Debug.WriteLine(string.Format("Check Execution Error: {0}-{1} [{2}]",
                                                                             ARuleCheck.CheckTypeCd,
                                                                             ARuleCheck.CheckNumber.ToString(),
                                                                             AErrorMessage));
                            this.UpdateSeverity(SeverityCd);
                        }

                        //Process.ElapsedTimes.TimingEnd("Checks", "ProcessDo_RunDo_" + CheckProcedureName, this);
                    }
                    else
                    {
                        AErrorMessage = "Unable to find procedure '" + CheckProcedureName;
                        Process.UpdateErrors(AErrorMessage);
                        RunResult = Process.LogError(AErrorMessage, ARuleCheck,
                                                       CurrentMonLocId, CurrentTestSumId, CurrentOpDate, CurrentOpHour,
                                                       TableName, CurrentRowId, RecordIdentifier, this, out SeverityCd);
                    }

                    //Process.ElapsedTimes.TimingEnd(RuleCheck.CheckTypeCd + RuleCheck.CheckNumber.ToString(), this);
                }
                catch (Exception ex)
                {
                    AErrorMessage = CheckEngine.FormatError(ex, CheckProcedureName);

                    RunResult = Process.LogError(AErrorMessage, ARuleCheck,
                                         CurrentMonLocId, CurrentTestSumId, CurrentOpDate, CurrentOpHour,
                                         TableName, CurrentRowId, RecordIdentifier, this, out SeverityCd);

                    Process.UpdateErrors(AErrorMessage);
                    RunResult = false;
                }

                //Process.ElapsedTimes.TimingBegin("Checks", "ProcessDo_RunDo_Log", this);

                //Process.ElapsedTimes.TimingEnd("Checks", "ProcessDo_RunDo_Log", this);

                return RunResult;
            }
            finally
            {
                FRunningRuleCheck = null;
            }
        }

        /// <summary>
        /// Update the severity
        /// </summary>
        /// <param name="ASeverityCd">The Severity Code to merge.</param>
        public void UpdateSeverity(eSeverityCd ASeverityCd)
        {
            if (ASeverityCd.GetSeverityLevel() > SeverityCd.GetSeverityLevel())
            {
                SeverityCd = ASeverityCd;
                Process.UpdateSeverity(ASeverityCd);
            }
        }

        #endregion


        #region Public Methods: Check Parameters

        /// <summary>
        /// Resets the check parameters for the category.
        /// </summary>
        public void EraseParameters()
        { // reset these to their default values
            SeverityCd = eSeverityCd.NONE;

            if (Process.ProcessParameters == null)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("Failed Parameters Erase: Category - {0} : {1}",
                                                                 this.CategoryCd,
                                                                 "Check parameters object not implemented for this process."));
            }
            else
            {
                Process.ProcessParameters.Reset(this);
            }
        }

        /// <summary>
        /// Gets the parameter specified by the passed parameter name.
        /// </summary>
        /// <param name="AParameterName">The name of the parameter to return.</param>
        /// <returns>Returns the parameter or null if not found.</returns>
        public cLegacyCheckParameter GetCheckParameter(string AParameterName)
        {
            cLegacyCheckParameter Result = new cLegacyCheckParameter();

            if (Process.ProcessParameters == null)
            {
                FailParameterGet(AParameterName, "Check parameters object not implemented for this process.");
            }
            else if (!Process.ProcessParameters.ContainsLegacyParameter(AParameterName))
            {
                FailParameterGet(AParameterName, "Check parameter not implemented for this process.");
            }
            else
            {
                try
                {
                    cCheckParameterCheckEngine CheckParameter = Process.ProcessParameters.GetLegacyParameter(AParameterName);

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
                    FailParameterGet(AParameterName, ex.Message);
                }
            }

            return Result;
        }

        private void FailParameterGet(string AParameterName, string AFailureMessage)
        {
            if (this.RunningRuleCheck == null)
                System.Diagnostics.Debug.WriteLine(string.Format("Failed Parameter Get: Category - {0}, Parameter - {1} : {2}",
                                                                 this.CategoryCd,
                                                                 AParameterName, AFailureMessage));
            else
                System.Diagnostics.Debug.WriteLine(string.Format("Failed Parameter Get: Category - {0}, Check - {1}, Parameter - {2} : {3}",
                                                                 this.CategoryCd,
                                                                 this.RunningRuleCheck.CheckName,
                                                                 AParameterName, AFailureMessage));
        }

        private void FailParameterSet(string AParameterName, string AFailureMessage)
        {
            if (this.RunningRuleCheck == null)
                System.Diagnostics.Debug.WriteLine(string.Format("Failed Parameter Set: Category - {0}, Parameter - {1} : {2}",
                                                                 this.CategoryCd,
                                                                 AParameterName, AFailureMessage));
            else
                System.Diagnostics.Debug.WriteLine(string.Format("Failed Parameter Set: Category - {0}, Check - {1}, Parameter - {2} : {3}",
                                                                 this.CategoryCd,
                                                                 this.RunningRuleCheck.CheckName,
                                                                 AParameterName, AFailureMessage));
        }

        #region Sets

        /// <summary>
        /// Sets the parameter specified by the passed parameter name.
        /// 
        /// This is the primary Set Check Parameter method.
        /// </summary>
        /// <param name="AParameterName">The name of the parameter to set.</param>
        /// <param name="AParameterValue">The value to set.</param>
        public void SetCheckParameter(string AParameterName, object AParameterValue)
        {
            if (Process.ProcessParameters == null)
            {
                FailParameterSet(AParameterName, "Check parameters object not implemented for this process.");
            }
            else if (!Process.ProcessParameters.ContainsLegacyParameter(AParameterName))
            {
                FailParameterSet(AParameterName, "Check parameter not implemented for this process.");
            }
            else
            {
                try
                {
                    cCheckParameterCheckEngine CheckParameter = Process.ProcessParameters.GetLegacyParameter(AParameterName);

                    CheckParameter.LegacySetValue(AParameterValue, this);
                }
                catch (Exception ex)
                {
                    FailParameterSet(AParameterName, ex.Message);
                }
            }
        }

        /// <summary>
        /// Obsolete method that sets the parameter specified by the passed parameter name.
        /// </summary>
        /// <param name="ParameterName">The name of the parameter to set.</param>
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
        /// <param name="ParameterName">The name of the parameter to set.</param>
        /// <param name="ParameterValue">The value to set.</param>
        /// <param name="ParameterType">The type of the parameter. (obsolete and ignored)</param>
        public void SetCheckParameter(string ParameterName, object ParameterValue, eParameterDataType ParameterType)
        {
            SetCheckParameter(ParameterName, ParameterValue);
        }

        /// <summary>
        /// Sets a data row view parameter to the first row returned by applying the passed filter and sort 
        /// information to the passed table.
        /// </summary>
        /// <param name="ParameterName">The name of the parameter</param>
        /// <param name="Table">The table to apply the filter and sort informaiton against.</param>
        /// <param name="RowFilter">The filter to apply.</param>
        /// <param name="Sort">The sort to apply.</param>
        public void SetDataRowCheckParameter(string ParameterName, DataTable Table, string RowFilter, string Sort)
        {
            DataRowView ParameterValue;

            try
            {
                ParameterValue = new DataView(Table, RowFilter, Sort, DataViewRowState.CurrentRows)[0];
            }
            catch (Exception ex)
            {
                ParameterValue = null;
                FailParameterSet(ParameterName, ex.Message);
            }

            SetCheckParameter(ParameterName, ParameterValue);
        }

        /// <summary>
        /// Sets a data view parameterview returned by applying the passed filter and sort information 
        /// to the passed table.
        /// </summary>
        /// <param name="ParameterName">The name of the parameter</param>
        /// <param name="Table">The table to apply the filter and sort informaiton against.</param>
        /// <param name="RowFilter">The filter to apply.</param>
        /// <param name="Sort">The sort to apply.</param>
        public void SetDataViewCheckParameter(string ParameterName, DataTable Table, string RowFilter, string Sort)
        {
            DataView ParameterValue;

            try
            {
                ParameterValue = new DataView(Table, RowFilter, Sort, DataViewRowState.CurrentRows);
            }
            catch (Exception ex)
            {
                ParameterValue = null;
                FailParameterSet(ParameterName, ex.Message);
            }

            SetCheckParameter(ParameterName, ParameterValue);
        }

        #endregion

        #region Accum

        /// <summary>
        /// Aggregates the additional value into the parameter specified by the passed parameter name.
        /// </summary>
        /// <param name="AParameterName">The name of the parameter to aggregate.</param>
        /// <param name="AAdditionalValue">The value to aggregate.</param>
        /// <param name="AOrTogether">Or to existing value if true, otherwise and.</param>
        public void AccumCheckAggregate(string AParameterName, bool AAdditionalValue, bool AOrTogether)
        {
            if (Process.ProcessParameters == null)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("Failed Parameter Accumulation: Category - {0}, Parameter - {1} : {2}",
                                                                 this.CategoryCd, AParameterName,
                                                                 "Check parameters object not implemented for this process."));
            }
            else if (!Process.ProcessParameters.ContainsLegacyParameter(AParameterName, this))
            {
                System.Diagnostics.Debug.WriteLine(string.Format("Failed Parameter Accumulation: Category - {0}, Parameter - {1} : {2}",
                                                                 this.CategoryCd, AParameterName,
                                                                 "Check parameter not implemented for this process."));
            }
            else
            {
                try
                {
                    cCheckParameterCheckEngine CheckParameter = Process.ProcessParameters.GetLegacyParameter(AParameterName);

                    if (CheckParameter.IsLegacyType)
                    {
                        cCheckParameterLegacy CheckParameterLegacy = (cCheckParameterLegacy)CheckParameter;

                        bool LegacyValue;

                        if (AOrTogether)
                            LegacyValue = (Convert.ToBoolean(CheckParameterLegacy.LegacyValue) || AAdditionalValue);
                        else
                            LegacyValue = (Convert.ToBoolean(CheckParameterLegacy.LegacyValue) && AAdditionalValue);

                        CheckParameterLegacy.LegacyUpdateValue(LegacyValue, this);
                    }
                    else if ((CheckParameter.DataType == eParameterDataType.Boolean) && !CheckParameter.IsArray)
                    {
                        ((cCheckParameterBooleanValue)CheckParameter).AggregateValue(AAdditionalValue, AOrTogether, this);
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine(string.Format("Failed Parameter Accumulation: Category - {0}, Parameter - {1} : {2}",
                                                                         this.CategoryCd, AParameterName,
                                                                         "Check parameters object is not for booleans or is an array"));
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(string.Format("Failed Parameter Accumulation: Category - {0}, Parameter - {1} : {2}",
                                                       this.CategoryCd, AParameterName, ex.Message));
                }
            }
        }

        /// <summary>
        /// Aggregates the additional value into the parameter specified by the passed parameter name.
        /// </summary>
        /// <param name="AParameterName">The name of the parameter to remove.</param>
        /// <param name="AAdditionalValue">The value to aggregate.</param>
        public void AccumCheckAggregate(string AParameterName, decimal AAdditionalValue)
        {
            if (Process.ProcessParameters == null)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("Failed Parameter Accumulation: Category - {0}, Parameter - {1} : {2}",
                                                                 this.CategoryCd, AParameterName,
                                                                 "Check parameters object not implemented for this process."));
            }
            else if (!Process.ProcessParameters.ContainsLegacyParameter(AParameterName, this))
            {
                System.Diagnostics.Debug.WriteLine(string.Format("Failed Parameter Accumulation: Category - {0}, Parameter - {1} : {2}",
                                                                 this.CategoryCd, AParameterName,
                                                                 "Check parameter not implemented for this process."));
            }
            else
            {
                try
                {
                    cCheckParameterCheckEngine CheckParameter = Process.ProcessParameters.GetLegacyParameter(AParameterName);

                    if (CheckParameter.IsLegacyType)
                    {
                        cCheckParameterLegacy CheckParameterLegacy = (cCheckParameterLegacy)CheckParameter;

                        decimal LegacyValue = Convert.ToDecimal(CheckParameterLegacy.LegacyValue) + AAdditionalValue;

                        CheckParameterLegacy.LegacyUpdateValue(LegacyValue, this);
                    }
                    else if ((CheckParameter.DataType == eParameterDataType.Decimal) && !CheckParameter.IsArray)
                    {
                        ((cCheckParameterDecimalValue)CheckParameter).AggregateValue(AAdditionalValue, this);
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine(string.Format("Failed Parameter Accumulation: Category - {0}, Parameter - {1} : {2}",
                                                                         this.CategoryCd, AParameterName,
                                                                         "Check parameters object is not for decimals or is an array"));
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(string.Format("Failed Parameter Accumulation: Category - {0}, Parameter - {1} : {2}",
                                                       this.CategoryCd, AParameterName, ex.Message));
                }
            }
        }

        /// <summary>
        /// Aggregates the additional value into the parameter specified by the passed parameter name.
        /// </summary>
        /// <param name="AParameterName">The name of the parameter to aggregate.</param>
        /// <param name="AAdditionalValue">The value to aggregate.</param>
        public void AccumCheckAggregate(string AParameterName, int AAdditionalValue)
        {
            if (Process.ProcessParameters == null)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("Failed Parameter Accumulation: Category - {0}, Parameter - {1} : {2}",
                                                                 this.CategoryCd, AParameterName,
                                                                 "Check parameters object not implemented for this process."));
            }
            else if (!Process.ProcessParameters.ContainsLegacyParameter(AParameterName, this))
            {
                System.Diagnostics.Debug.WriteLine(string.Format("Failed Parameter Accumulation: Category - {0}, Parameter - {1} : {2}",
                                                                 this.CategoryCd, AParameterName,
                                                                 "Check parameter not implemented for this process."));
            }
            else
            {
                try
                {
                    cCheckParameterCheckEngine CheckParameter = Process.ProcessParameters.GetLegacyParameter(AParameterName);

                    if (CheckParameter.IsLegacyType)
                    {
                        cCheckParameterLegacy CheckParameterLegacy = (cCheckParameterLegacy)CheckParameter;

                        int LegacyValue = Convert.ToInt32(CheckParameterLegacy.LegacyValue) + AAdditionalValue;

                        CheckParameterLegacy.LegacyUpdateValue(LegacyValue, this);
                    }
                    else if ((CheckParameter.DataType == eParameterDataType.Integer) && !CheckParameter.IsArray)
                    {
                        ((cCheckParameterIntegerValue)CheckParameter).AggregateValue(AAdditionalValue, this);
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine(string.Format("Failed Parameter Accumulation: Category - {0}, Parameter - {1} : {2}",
                                                                         this.CategoryCd, AParameterName,
                                                                         "Check parameters object is not for integers or is an array"));
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(string.Format("Failed Parameter Accumulation: Category - {0}, Parameter - {1} : {2}",
                                                       this.CategoryCd, AParameterName, ex.Message));
                }
            }
        }

        /// <summary>
        /// Aggregates the additional value into the element of the array parameter specified by the 
        /// passed parameter name and array index.
        /// </summary>
        /// <param name="AParameterName">The name of the parameter to aggregate.</param>
        /// <param name="AArrayIndex">The index of the elmement in the array to aggregate</param>
        /// <param name="AAdditionalValue">The value to aggregate.</param>
        /// <param name="AOrTogether">Or to existing value if true, otherwise and.</param>
        public void AccumCheckAggregate(string AParameterName, int AArrayIndex, bool AAdditionalValue, bool AOrTogether)
        {
            if (Process.ProcessParameters == null)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("Failed Parameter Accumulation: Category - {0}, Parameter - {1} : {2}",
                                                                 this.CategoryCd, AParameterName,
                                                                 "Check parameters object not implemented for this process."));
            }
            else if (!Process.ProcessParameters.ContainsLegacyParameter(AParameterName, this))
            {
                System.Diagnostics.Debug.WriteLine(string.Format("Failed Parameter Accumulation: Category - {0}, Parameter - {1} : {2}",
                                                                 this.CategoryCd, AParameterName,
                                                                 "Check parameter not implemented for this process."));
            }
            else
            {
                try
                {
                    cCheckParameterCheckEngine CheckParameter = Process.ProcessParameters.GetLegacyParameter(AParameterName);

                    if (CheckParameter.IsLegacyType)
                    {
                        cCheckParameterLegacy CheckParameterLegacy = (cCheckParameterLegacy)CheckParameter;

                        bool[] LegacyValue = ((bool[])CheckParameterLegacy.LegacyValue);

                        if (AOrTogether)
                            LegacyValue[AArrayIndex] = (LegacyValue[AArrayIndex] || AAdditionalValue);
                        else
                            LegacyValue[AArrayIndex] = (LegacyValue[AArrayIndex] && AAdditionalValue);

                        CheckParameterLegacy.LegacyUpdateValue(LegacyValue, this);
                    }
                    else if ((CheckParameter.DataType == eParameterDataType.Boolean) && CheckParameter.IsArray)
                    {
                        ((cCheckParameterBooleanArray)CheckParameter).AggregateValue(AAdditionalValue, AArrayIndex, AOrTogether, this);
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine(string.Format("Failed Parameter Accumulation: Category - {0}, Parameter - {1} : {2}",
                                                                         this.CategoryCd, AParameterName,
                                                                         "Check parameters object is not for booleans or is an array"));
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(string.Format("Failed Parameter Accumulation: Category - {0}, Parameter - {1} : {2}",
                                                       this.CategoryCd, AParameterName, ex.Message));
                }
            }
        }

        /// <summary>
        /// Aggregates the additional value into the element of the array parameter specified by the 
        /// passed parameter name and array index.
        /// </summary>
        /// <param name="AParameterName">The name of the parameter to aggregate.</param>
        /// <param name="AArrayIndex">The index of the elmement in the array to aggregate</param>
        /// <param name="AAdditionalValue">The value to aggregate.</param>
        public void AccumCheckAggregate(string AParameterName, int AArrayIndex, decimal AAdditionalValue)
        {
            if (Process.ProcessParameters == null)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("Failed Parameter Accumulation: Category - {0}, Parameter - {1} : {2}",
                                                                 this.CategoryCd, AParameterName,
                                                                 "Check parameters object not implemented for this process."));
            }
            else if (!Process.ProcessParameters.ContainsLegacyParameter(AParameterName, this))
            {
                System.Diagnostics.Debug.WriteLine(string.Format("Failed Parameter Accumulation: Category - {0}, Parameter - {1} : {2}",
                                                                 this.CategoryCd, AParameterName,
                                                                 "Check parameter not implemented for this process."));
            }
            else
            {
                try
                {
                    cCheckParameterCheckEngine CheckParameter = Process.ProcessParameters.GetLegacyParameter(AParameterName);

                    if (CheckParameter.IsLegacyType)
                    {
                        cCheckParameterLegacy CheckParameterLegacy = (cCheckParameterLegacy)CheckParameter;

                        decimal[] LegacyValue = ((decimal[])CheckParameterLegacy.LegacyValue);

                        LegacyValue[AArrayIndex] += AAdditionalValue;

                        CheckParameterLegacy.LegacyUpdateValue(LegacyValue, this);
                    }
                    else if ((CheckParameter.DataType == eParameterDataType.Decimal) && CheckParameter.IsArray)
                    {
                        ((cCheckParameterDecimalArray)CheckParameter).AggregateValue(AAdditionalValue, AArrayIndex, this);
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine(string.Format("Failed Parameter Accumulation: Category - {0}, Parameter - {1} : {2}",
                                                                         this.CategoryCd, AParameterName,
                                                                         "Check parameters object is not for decimals or is an array"));
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(string.Format("Failed Parameter Accumulation: Category - {0}, Parameter - {1} : {2}",
                                                       this.CategoryCd, AParameterName, ex.Message));
                }
            }
        }

        /// <summary>
        /// Aggregates the additional value into the element of the array parameter specified by the 
        /// passed parameter name and array index.
        /// </summary>
        /// <param name="AParameterName">The name of the parameter to aggregate.</param>
        /// <param name="AArrayIndex">The index of the elmement in the array to aggregate</param>
        /// <param name="AAdditionalValue">The value to aggregate.</param>
        public void AccumCheckAggregate(string AParameterName, int AArrayIndex, int AAdditionalValue)
        {
            if (Process.ProcessParameters == null)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("Failed Parameter Accumulation: Category - {0}, Parameter - {1} : {2}",
                                                                 this.CategoryCd, AParameterName,
                                                                 "Check parameters object not implemented for this process."));
            }
            else if (!Process.ProcessParameters.ContainsLegacyParameter(AParameterName, this))
            {
                System.Diagnostics.Debug.WriteLine(string.Format("Failed Parameter Accumulation: Category - {0}, Parameter - {1} : {2}",
                                                                 this.CategoryCd, AParameterName,
                                                                 "Check parameter not implemented for this process."));
            }
            else
            {
                try
                {
                    cCheckParameterCheckEngine CheckParameter = Process.ProcessParameters.GetLegacyParameter(AParameterName);

                    if (CheckParameter.IsLegacyType)
                    {
                        cCheckParameterLegacy CheckParameterLegacy = (cCheckParameterLegacy)CheckParameter;

                        int[] LegacyValue = ((int[])CheckParameterLegacy.LegacyValue);

                        LegacyValue[AArrayIndex] += AAdditionalValue;

                        CheckParameterLegacy.LegacyUpdateValue(LegacyValue, this);
                    }
                    else if ((CheckParameter.DataType == eParameterDataType.Integer) && CheckParameter.IsArray)
                    {
                        ((cCheckParameterIntegerArray)CheckParameter).AggregateValue(AAdditionalValue, AArrayIndex, this);
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine(string.Format("Failed Parameter Accumulation: Category - {0}, Parameter - {1} : {2}",
                                                                         this.CategoryCd, AParameterName,
                                                                         "Check parameters object is not for integers or is an array"));
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(string.Format("Failed Parameter Accumulation: Category - {0}, Parameter - {1} : {2}",
                                                       this.CategoryCd, AParameterName, ex.Message));
                }
            }
        }

        #endregion

        #region Array Sets

        /// <summary>
        /// Sets the array parameter specified by the passed parameter name.
        /// </summary>
        /// <param name="AParameterName">The name of the parameter to set.</param>
        /// <param name="AParameterValue">The array to set.</param>
        public void SetArrayParameterDo(string AParameterName, object AParameterValue)
        {
            if (Process.ProcessParameters == null)
            {
                FailParameterSet(AParameterName, "Check parameters object not implemented for this process.");
            }
            else if (!Process.ProcessParameters.ContainsLegacyParameter(AParameterName, this))
            {
                FailParameterSet(AParameterName, "Check parameter not owned by this category.");
            }
            else
            {
                try
                {
                    cCheckParameterCheckEngine CheckParameter = Process.ProcessParameters.GetLegacyParameter(AParameterName);

                    CheckParameter.LegacyUpdateValue(AParameterValue, this);
                }
                catch (Exception ex)
                {
                    FailParameterSet(AParameterName, ex.Message);
                }
            }
        }

        /// <summary>
        /// Obsolete method that sets the parameter specified by the passed parameter name.
        /// </summary>
        /// <param name="AParameterName">The name of the parameter to set.</param>
        /// <param name="AArray">The value to set.</param>
        public void SetArrayParameter(string AParameterName, bool[] AArray)
        {
            SetArrayParameterDo(AParameterName, AArray);
        }

        /// <summary>
        /// Obsolete method that sets the parameter specified by the passed parameter name.
        /// </summary>
        /// <param name="AParameterName">The name of the parameter to set.</param>
        /// <param name="AArray">The value to set.</param>
        public void SetArrayParameter(string AParameterName, decimal[] AArray)
        {
            SetArrayParameterDo(AParameterName, AArray);
        }

        /// <summary>
        /// Obsolete method that sets the parameter specified by the passed parameter name.
        /// </summary>
        /// <param name="AParameterName">The name of the parameter to set.</param>
        /// <param name="AArray">The value to set.</param>
        public void SetArrayParameter(string AParameterName, int[] AArray)
        {
            SetArrayParameterDo(AParameterName, AArray);
        }

        /// <summary>
        /// Obsolete method that sets the parameter specified by the passed parameter name.
        /// </summary>
        /// <param name="AParameterName">The name of the parameter to set.</param>
        /// <param name="AArray">The value to set.</param>
        public void SetArrayParameter(string AParameterName, string[] AArray)
        {
            SetArrayParameterDo(AParameterName, AArray);
        }

        /// <summary>
        /// Aggregates the additional value into the element of the array parameter specified by the 
        /// passed parameter name and array index.
        /// </summary>
        /// <param name="AParameterName">The name of the parameter to aggregate.</param>
        /// <param name="AArrayIndex">The index of the elmement in the array to aggregate</param>
        /// <param name="AParameterValue">The value to aggregate.</param>
        public void SetArrayParameter(string AParameterName, int AArrayIndex, bool AParameterValue)
        {
            if (Process.ProcessParameters == null)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("Failed Parameter Accumulation: Category - {0}, Parameter - {1} : {2}",
                                                                 this.CategoryCd, AParameterName,
                                                                 "Check parameters object not implemented for this process."));
            }
            else if (!Process.ProcessParameters.ContainsLegacyParameter(AParameterName, this))
            {
                System.Diagnostics.Debug.WriteLine(string.Format("Failed Parameter Accumulation: Category - {0}, Parameter - {1} : {2}",
                                                                 this.CategoryCd, AParameterName,
                                                                 "Check parameter not owned by category."));
            }
            else
            {
                try
                {
                    cCheckParameterCheckEngine CheckParameter = Process.ProcessParameters.GetLegacyParameter(AParameterName);

                    if (CheckParameter.IsLegacyType)
                    {
                        cCheckParameterLegacy CheckParameterLegacy = (cCheckParameterLegacy)CheckParameter;

                        bool[] LegacyValue = (bool[])CheckParameterLegacy.LegacyValue;

                        LegacyValue[AArrayIndex] = AParameterValue;

                        CheckParameterLegacy.LegacyUpdateValue(LegacyValue, this);
                    }
                    else if ((CheckParameter.DataType == eParameterDataType.Boolean) && CheckParameter.IsArray)
                    {
                        ((cCheckParameterBooleanArray)CheckParameter).UpdateValue(AParameterValue, AArrayIndex, this);
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine(string.Format("Failed Parameter Accumulation: Category - {0}, Parameter - {1} : {2}",
                                                                         this.CategoryCd, AParameterName,
                                                                         "Check parameters object is not for booleans or is an array"));
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(string.Format("Failed Parameter Accumulation: Category - {0}, Parameter - {1} : {2}",
                                                       this.CategoryCd, AParameterName, ex.Message));
                }
            }
        }

        /// <summary>
        /// Aggregates the additional value into the element of the array parameter specified by the 
        /// passed parameter name and array index.
        /// </summary>
        /// <param name="AParameterName">The name of the parameter to aggregate.</param>
        /// <param name="AArrayIndex">The index of the elmement in the array to aggregate</param>
        /// <param name="AParameterValue">The value to aggregate.</param>
        public void SetArrayParameter(string AParameterName, int AArrayIndex, decimal AParameterValue)
        {
            if (Process.ProcessParameters == null)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("Failed Parameter Accumulation: Category - {0}, Parameter - {1} : {2}",
                                                                 this.CategoryCd, AParameterName,
                                                                 "Check parameters object not implemented for this process."));
            }
            else if (!Process.ProcessParameters.ContainsLegacyParameter(AParameterName, this))
            {
                System.Diagnostics.Debug.WriteLine(string.Format("Failed Parameter Accumulation: Category - {0}, Parameter - {1} : {2}",
                                                                 this.CategoryCd, AParameterName,
                                                                 "Check parameter not owned by category."));
            }
            else
            {
                try
                {
                    cCheckParameterCheckEngine CheckParameter = Process.ProcessParameters.GetLegacyParameter(AParameterName);

                    if (CheckParameter.IsLegacyType)
                    {
                        cCheckParameterLegacy CheckParameterLegacy = (cCheckParameterLegacy)CheckParameter;

                        decimal[] LegacyValue = (decimal[])CheckParameterLegacy.LegacyValue;

                        LegacyValue[AArrayIndex] = AParameterValue;

                        CheckParameterLegacy.LegacyUpdateValue(LegacyValue, this);
                    }
                    else if ((CheckParameter.DataType == eParameterDataType.Decimal) && CheckParameter.IsArray)
                    {
                        ((cCheckParameterDecimalArray)CheckParameter).UpdateValue(AParameterValue, AArrayIndex, this);
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine(string.Format("Failed Parameter Accumulation: Category - {0}, Parameter - {1} : {2}",
                                                                         this.CategoryCd, AParameterName,
                                                                         "Check parameters object is not for decimals or is an array"));
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(string.Format("Failed Parameter Accumulation: Category - {0}, Parameter - {1} : {2}",
                                                       this.CategoryCd, AParameterName, ex.Message));
                }
            }
        }

        /// <summary>
        /// Aggregates the additional value into the element of the array parameter specified by the 
        /// passed parameter name and array index.
        /// </summary>
        /// <param name="AParameterName">The name of the parameter to aggregate.</param>
        /// <param name="AArrayIndex">The index of the elmement in the array to aggregate</param>
        /// <param name="AParameterValue">The value to aggregate.</param>
        public void SetDecimalArrayParameter(string AParameterName, int AArrayIndex, decimal? AParameterValue)
        {
            if (Process.ProcessParameters == null)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("Failed Parameter Accumulation: Category - {0}, Parameter - {1} : {2}",
                                                                 this.CategoryCd, AParameterName,
                                                                 "Check parameters object not implemented for this process."));
            }
            else if (!Process.ProcessParameters.ContainsLegacyParameter(AParameterName, this))
            {
                System.Diagnostics.Debug.WriteLine(string.Format("Failed Parameter Accumulation: Category - {0}, Parameter - {1} : {2}",
                                                                 this.CategoryCd, AParameterName,
                                                                 "Check parameter not owned by category."));
            }
            else
            {
                try
                {
                    cCheckParameterCheckEngine CheckParameter = Process.ProcessParameters.GetLegacyParameter(AParameterName);

                    if (CheckParameter.IsLegacyType)
                    {
                        cCheckParameterLegacy CheckParameterLegacy = (cCheckParameterLegacy)CheckParameter;

                        decimal?[] LegacyValue = (decimal?[])CheckParameterLegacy.LegacyValue;

                        LegacyValue[AArrayIndex] = AParameterValue;

                        CheckParameterLegacy.LegacyUpdateValue(LegacyValue, this);
                    }
                    else if ((CheckParameter.DataType == eParameterDataType.Decimal) && CheckParameter.IsArray)
                    {
                        ((cCheckParameterDecimalArray)CheckParameter).UpdateValue(AParameterValue, AArrayIndex, this);
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine(string.Format("Failed Parameter Accumulation: Category - {0}, Parameter - {1} : {2}",
                                                                         this.CategoryCd, AParameterName,
                                                                         "Check parameters object is not for decimals or is an array"));
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(string.Format("Failed Parameter Accumulation: Category - {0}, Parameter - {1} : {2}",
                                                       this.CategoryCd, AParameterName, ex.Message));
                }
            }
        }

        /// <summary>
        /// Aggregates the additional value into the element of the array parameter specified by the 
        /// passed parameter name and array index.
        /// </summary>
        /// <param name="AParameterName">The name of the parameter to aggregate.</param>
        /// <param name="AArrayIndex">The index of the elmement in the array to aggregate</param>
        /// <param name="AParameterValue">The value to aggregate.</param>
        public void SetIntegerArrayParameter(string AParameterName, int AArrayIndex, int? AParameterValue)
        {
            if (Process.ProcessParameters == null)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("Failed Parameter Accumulation: Category - {0}, Parameter - {1} : {2}",
                                                                 this.CategoryCd, AParameterName,
                                                                 "Check parameters object not implemented for this process."));
            }
            else if (!Process.ProcessParameters.ContainsLegacyParameter(AParameterName, this))
            {
                System.Diagnostics.Debug.WriteLine(string.Format("Failed Parameter Accumulation: Category - {0}, Parameter - {1} : {2}",
                                                                 this.CategoryCd, AParameterName,
                                                                 "Check parameter not owned by category."));
            }
            else
            {
                try
                {
                    cCheckParameterCheckEngine CheckParameter = Process.ProcessParameters.GetLegacyParameter(AParameterName);

                    if (CheckParameter.IsLegacyType)
                    {
                        cCheckParameterLegacy CheckParameterLegacy = (cCheckParameterLegacy)CheckParameter;

                        int?[] LegacyValue = (int?[])CheckParameterLegacy.LegacyValue;

                        LegacyValue[AArrayIndex] = AParameterValue;

                        CheckParameterLegacy.LegacyUpdateValue(LegacyValue, this);
                    }
                    else if ((CheckParameter.DataType == eParameterDataType.Integer) && CheckParameter.IsArray)
                    {
                        ((cCheckParameterDecimalArray)CheckParameter).UpdateValue(AParameterValue, AArrayIndex, this);
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine(string.Format("Failed Parameter Accumulation: Category - {0}, Parameter - {1} : {2}",
                                                                         this.CategoryCd, AParameterName,
                                                                         "Check parameters object is not for decimals or is an array"));
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(string.Format("Failed Parameter Accumulation: Category - {0}, Parameter - {1} : {2}",
                                                       this.CategoryCd, AParameterName, ex.Message));
                }
            }
        }

        /// <summary>
        /// Aggregates the additional value into the element of the array parameter specified by the 
        /// passed parameter name and array index.
        /// </summary>
        /// <param name="AParameterName">The name of the parameter to aggregate.</param>
        /// <param name="AArrayIndex">The index of the elmement in the array to aggregate</param>
        /// <param name="AParameterValue">The value to aggregate.</param>
        public void SetArrayParameter(string AParameterName, int AArrayIndex, int AParameterValue)
        {
            if (Process.ProcessParameters == null)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("Failed Parameter Accumulation: Category - {0}, Parameter - {1} : {2}",
                                                                 this.CategoryCd, AParameterName,
                                                                 "Check parameters object not implemented for this process."));
            }
            else if (!Process.ProcessParameters.ContainsLegacyParameter(AParameterName, this))
            {
                System.Diagnostics.Debug.WriteLine(string.Format("Failed Parameter Accumulation: Category - {0}, Parameter - {1} : {2}",
                                                                 this.CategoryCd, AParameterName,
                                                                 "Check parameter not owned by category."));
            }
            else
            {
                try
                {
                    cCheckParameterCheckEngine CheckParameter = Process.ProcessParameters.GetLegacyParameter(AParameterName);

                    if (CheckParameter.IsLegacyType)
                    {
                        cCheckParameterLegacy CheckParameterLegacy = (cCheckParameterLegacy)CheckParameter;

                        int[] LegacyValue = (int[])CheckParameterLegacy.LegacyValue;

                        LegacyValue[AArrayIndex] = AParameterValue;

                        CheckParameterLegacy.LegacyUpdateValue(LegacyValue, this);
                    }
                    else if ((CheckParameter.DataType == eParameterDataType.Integer) && CheckParameter.IsArray)
                    {
                        ((cCheckParameterIntegerArray)CheckParameter).UpdateValue(AParameterValue, AArrayIndex, this);
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine(string.Format("Failed Parameter Accumulation: Category - {0}, Parameter - {1} : {2}",
                                                                         this.CategoryCd, AParameterName,
                                                                         "Check parameters object is not for decimals or is an array"));
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(string.Format("Failed Parameter Accumulation: Category - {0}, Parameter - {1} : {2}",
                                                       this.CategoryCd, AParameterName, ex.Message));
                }
            }
        }

        /// <summary>
        /// Aggregates the additional value into the element of the array parameter specified by the 
        /// passed parameter name and array index.
        /// </summary>
        /// <param name="AParameterName">The name of the parameter to aggregate.</param>
        /// <param name="AArrayIndex">The index of the elmement in the array to aggregate</param>
        /// <param name="AParameterValue">The value to aggregate.</param>
        public void SetArrayParameter(string AParameterName, int AArrayIndex, string AParameterValue)
        {
            if (Process.ProcessParameters == null)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("Failed Parameter Accumulation: Category - {0}, Parameter - {1} : {2}",
                                                                 this.CategoryCd, AParameterName,
                                                                 "Check parameters object not implemented for this process."));
            }
            else if (!Process.ProcessParameters.ContainsLegacyParameter(AParameterName, this))
            {
                System.Diagnostics.Debug.WriteLine(string.Format("Failed Parameter Accumulation: Category - {0}, Parameter - {1} : {2}",
                                                                 this.CategoryCd, AParameterName,
                                                                 "Check parameter not owned by category."));
            }
            else
            {
                try
                {
                    cCheckParameterCheckEngine CheckParameter = Process.ProcessParameters.GetLegacyParameter(AParameterName);

                    if (CheckParameter.IsLegacyType)
                    {
                        cCheckParameterLegacy CheckParameterLegacy = (cCheckParameterLegacy)CheckParameter;

                        string[] LegacyValue = (string[])CheckParameterLegacy.LegacyValue;

                        LegacyValue[AArrayIndex] = AParameterValue;

                        CheckParameterLegacy.LegacyUpdateValue(LegacyValue, this);
                    }
                    else if ((CheckParameter.DataType == eParameterDataType.String) && CheckParameter.IsArray)
                    {
                        ((cCheckParameterStringArray)CheckParameter).UpdateValue(AParameterValue, AArrayIndex, this);
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine(string.Format("Failed Parameter Accumulation: Category - {0}, Parameter - {1} : {2}",
                                                                         this.CategoryCd, AParameterName,
                                                                         "Check parameters object is not for decimals or is an array"));
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(string.Format("Failed Parameter Accumulation: Category - {0}, Parameter - {1} : {2}",
                                                       this.CategoryCd, AParameterName, ex.Message));
                }
            }
        }

        #endregion

        #endregion


        #region Public Methods: Check Bands

        /// <summary>
        /// Populates the check bands for the category.
        /// </summary>
        /// <param name="ADatabaseAux">The AUX database object to use for the update.</param>
        /// <param name="AErrorMessage">The error message returned on failure.</param>
        /// <returns>True if the population is successful.</returns>
        public bool InitCheckBands(cDatabase ADatabaseAux, ref string AErrorMessage)
        {
            CheckParameterBands = new cCheckParameterBands(this.CategoryCd);

            return CheckParameterBands.Populate(ADatabaseAux, this.Process.ProcessParameters, ref AErrorMessage);
        }

        /// <summary>
        /// Populates the check bands for the category.
        /// </summary>
        /// <param name="ACheckBands">The category code of the checks bands to load.</param>
        public void SetCheckBands(cCheckParameterBands ACheckBands)
        {
            FCategoryCd = ACheckBands.CategoryCd;
            CheckParameterBands = ACheckBands;
        }

        #endregion


        #region Protected Methods: General

        /// <summary>
        /// Resets ErrorSupressValues to null and calls the child SetErrorSuppressValues method.
        /// </summary>
        /// <returns></returns>
        protected bool HandleSetErrorSuppressValues()
        {
            bool result;

            ErrorSuppressValues = null;

            result = SetErrorSuppressValues();

            return result;
        }

        /// <summary>
        /// Returns the source table in the Source Tables at the specified index.
        /// </summary>
        /// <param name="index">The index of the table in Source Tables.</param>
        /// <returns>The table at the specified index or null if the index position does not exist.</returns>
        protected DataTable SourceTable(int index)
        {
            DataTable result;

            DataTableCollection sourceTables = SourceTables();

            if ((sourceTables != null) &&
                (index >= 0) && (index < sourceTables.Count))
                result = sourceTables[index];
            else
                result = null;

            return result;
        }

        /// <summary>
        /// Returns the source table in the Source Tables with the specified table name.
        /// </summary>
        /// <param name="tableName">The name of the table to return.</param>
        /// <returns>The table of the specifed name or null if the table does not exist.</returns>
        protected DataTable SourceTable(string tableName)
        {
            DataTable result;

            DataTableCollection sourceTables = SourceTables();

            if ((sourceTables != null) &&
                (sourceTables.Contains(tableName)))
                result = sourceTables[tableName];
            else
                result = null;

            return result;
        }

        #endregion


        #region Abstract Functions/Procedures

        /// <summary>
        /// Filter the data
        /// </summary>
        protected abstract void FilterData();

        /// <summary>
        /// Set the record identifier
        /// </summary>
        protected abstract void SetRecordIdentifier();

        /// <summary>
        /// Set the error suppression values used to apply error suppression.
        /// </summary>
        /// <returns>Returns false if the setting fails.</returns>
        protected abstract bool SetErrorSuppressValues();

        #endregion


        #region Virtual Methods

        /// <summary>
        /// The source tables
        /// </summary>
        /// <returns></returns>
        protected virtual DataTableCollection SourceTables()
        {
            if (Process != null && Process.SourceData != null)
            {
                return Process.SourceData.Tables;
            }
            else
                return null;

        }

        /// <summary>
        /// the row position
        /// </summary>
        /// <param name="ABaseInternalTableName"></param>
        /// <returns></returns>
        public virtual int RowPosition(string ABaseInternalTableName)
        {
            return int.MinValue;
        }

        #endregion


        #region Update Log Processing Initialization


        #endregion


        #region Filter Ranged Hourly Utility Methods

        /// <summary>
        /// Filter Ranged
        /// </summary>
        /// <param name="ASourceName"></param>
        /// <param name="AFilterTable"></param>
        /// <param name="AOpDate"></param>
        /// <param name="AOpHour"></param>
        /// <param name="AMonLocId"></param>
        /// <param name="ABeganDateName"></param>
        /// <param name="AEndedDateName"></param>
        /// <returns></returns>
        protected DataView FilterRanged(string ASourceName, DataTable AFilterTable,
                                        DateTime AOpDate, int AOpHour, string AMonLocId,
                                        string ABeganDateName, string AEndedDateName)
        {
            DataTable SourceTable = SourceTables()[ASourceName];
            DataRow FilterRow;
            DateTime BeganDate; DateTime EndedDate; string MonLocId;

            AFilterTable.Rows.Clear();

            foreach (DataRow SourceRow in SourceTable.Rows)
            {
                BeganDate = cDBConvert.ToDate(SourceRow[ABeganDateName], DateTypes.START);
                EndedDate = cDBConvert.ToDate(SourceRow[AEndedDateName], DateTypes.END);
                MonLocId = cDBConvert.ToString(SourceRow["Mon_Loc_Id"]);

                if ((MonLocId == AMonLocId) && ((BeganDate <= AOpDate) && (AOpDate <= EndedDate)))
                {
                    FilterRow = AFilterTable.NewRow();

                    foreach (DataColumn Column in AFilterTable.Columns)
                        FilterRow[Column.ColumnName] = SourceRow[Column.ColumnName];

                    AFilterTable.Rows.Add(FilterRow);
                }
            }

            AFilterTable.AcceptChanges();

            return AFilterTable.DefaultView;
        }

        /// <summary>
        /// Filter Ranged
        /// </summary>
        /// <param name="ASourceName"></param>
        /// <param name="AFilterTable"></param>
        /// <param name="AOpDate"></param>    
        /// <param name="AMonLocId"></param>
        /// <param name="ABeganDateName1"></param>
        /// <param name="ABeganDateName2"></param>
        /// <param name="AEndedDateName"></param>
        /// <returns></returns>
        protected DataView FilterRanged(string ASourceName, DataTable AFilterTable,
                                        DateTime AOpDate, string AMonLocId,
                                        string ABeganDateName1, string ABeganDateName2, string AEndedDateName)
        {
            DataTable SourceTable = SourceTables()[ASourceName];
            DataRow FilterRow;
            DateTime BeganDate1; DateTime BeganDate2;
            DateTime BeganDate; DateTime EndedDate; string MonLocId;

            AFilterTable.Rows.Clear();

            foreach (DataRow SourceRow in SourceTable.Rows)
            {
                BeganDate1 = cDBConvert.ToDate(SourceRow[ABeganDateName1], DateTypes.START);
                BeganDate2 = cDBConvert.ToDate(SourceRow[ABeganDateName2], DateTypes.START);
                BeganDate = BeganDate1 < BeganDate2 ? BeganDate1 : BeganDate2;//Select the earlier of the two.

                EndedDate = cDBConvert.ToDate(SourceRow[AEndedDateName], DateTypes.END);
                MonLocId = cDBConvert.ToString(SourceRow["Mon_Loc_Id"]);

                if ((MonLocId == AMonLocId) && ((BeganDate <= AOpDate) && (AOpDate <= EndedDate)))
                {
                    FilterRow = AFilterTable.NewRow();

                    foreach (DataColumn Column in AFilterTable.Columns)
                        FilterRow[Column.ColumnName] = SourceRow[Column.ColumnName];

                    AFilterTable.Rows.Add(FilterRow);
                }
            }

            AFilterTable.AcceptChanges();

            return AFilterTable.DefaultView;
        }

        /// <summary>
        /// Filter ranged
        /// </summary>
        /// <param name="ASourceName"></param>
        /// <param name="AFilterTable"></param>
        /// <param name="AOpDate"></param>
        /// <param name="AOpHour"></param>
        /// <param name="AMonLocId"></param>
        /// <param name="ABeganDateName"></param>
        /// <param name="ABeganHourName"></param>
        /// <param name="AEndedDateName"></param>
        /// <param name="AEndedHourName"></param>
        /// <returns></returns>
        protected DataView FilterRanged(string ASourceName, DataTable AFilterTable,
                                        DateTime AOpDate, int AOpHour, string AMonLocId,
                                        string ABeganDateName, string ABeganHourName,
                                        string AEndedDateName, string AEndedHourName)
        {
            DataTable SourceTable = SourceTables()[ASourceName];
            DataRow FilterRow;
            DateTime BeganDate; int BeganHour; DateTime EndedDate; int EndedHour; string MonLocId;

            AFilterTable.Rows.Clear();

            foreach (DataRow SourceRow in SourceTable.Rows)
            {
                BeganDate = cDBConvert.ToDate(SourceRow[ABeganDateName], DateTypes.START);
                BeganHour = cDBConvert.ToHour(SourceRow[ABeganHourName], DateTypes.START);
                EndedDate = cDBConvert.ToDate(SourceRow[AEndedDateName], DateTypes.END);
                EndedHour = cDBConvert.ToHour(SourceRow[AEndedHourName], DateTypes.END);
                MonLocId = cDBConvert.ToString(SourceRow["Mon_Loc_Id"]);

                if ((MonLocId == AMonLocId) &&
                  ((BeganDate < AOpDate) || ((BeganDate == AOpDate) && (BeganHour <= AOpHour))) &&
                  ((EndedDate > AOpDate) || ((EndedDate == AOpDate) && (EndedHour >= AOpHour))))
                {
                    FilterRow = AFilterTable.NewRow();

                    foreach (DataColumn Column in AFilterTable.Columns)
                        FilterRow[Column.ColumnName] = SourceRow[Column.ColumnName];

                    AFilterTable.Rows.Add(FilterRow);
                }
            }

            AFilterTable.AcceptChanges();

            return AFilterTable.DefaultView;
        }

        /// <summary>
        /// Filter ranged
        /// </summary>
        /// <param name="ASourceName"></param>
        /// <param name="AFilterTable"></param>
        /// <param name="AOpDate"></param>
        /// <param name="AOpHour"></param>
        /// <param name="AMonLocId"></param>
        /// <returns></returns>
        protected DataView FilterRanged(string ASourceName, DataTable AFilterTable,
                                        DateTime AOpDate, int AOpHour, string AMonLocId)
        {
            DataTable SourceTable = SourceTables()[ASourceName];

            if (SourceTable.Columns.Contains("Begin_Hour") && SourceTable.Columns.Contains("End_Hour"))
                return FilterRanged(ASourceName, AFilterTable, AOpDate, AOpHour, AMonLocId,
                                    "Begin_Date", "Begin_Hour", "End_Date", "End_Hour");
            else
                return FilterRanged(ASourceName, AFilterTable, AOpDate, AOpHour, AMonLocId,
                                    "Begin_Date", "End_Date");
        }

        /// <summary>
        /// Filter ranged
        /// </summary>
        /// <param name="ASourceName"></param>
        /// <param name="AFilterTable"></param>
        /// <param name="AOpDate"></param>
        /// <param name="AOpHour"></param>
        /// <param name="ABeganDateName"></param>
        /// <param name="AEndedDateName"></param>
        /// <returns></returns>
        protected DataView FilterRanged(string ASourceName, DataTable AFilterTable,
                                        DateTime AOpDate, int AOpHour,
                                        string ABeganDateName, string AEndedDateName)
        {
            DataTable SourceTable = SourceTables()[ASourceName];
            DataRow FilterRow;
            DateTime BeganDate; DateTime EndedDate;

            AFilterTable.Rows.Clear();

            foreach (DataRow SourceRow in SourceTable.Rows)
            {
                BeganDate = cDBConvert.ToDate(SourceRow[ABeganDateName], DateTypes.START);
                EndedDate = cDBConvert.ToDate(SourceRow[AEndedDateName], DateTypes.END);

                if ((BeganDate <= AOpDate) && (AOpDate <= EndedDate))
                {
                    FilterRow = AFilterTable.NewRow();

                    foreach (DataColumn Column in AFilterTable.Columns)
                        FilterRow[Column.ColumnName] = SourceRow[Column.ColumnName];

                    AFilterTable.Rows.Add(FilterRow);
                }
            }

            AFilterTable.AcceptChanges();

            return AFilterTable.DefaultView;
        }

        /// <summary>
        /// Filter ranged
        /// </summary>
        /// <param name="ASourceName"></param>
        /// <param name="AFilterTable"></param>
        /// <param name="AOpDate"></param>
        /// <param name="AOpHour"></param>
        /// <param name="ABeganDateName"></param>
        /// <param name="ABeganHourName"></param>
        /// <param name="AEndedDateName"></param>
        /// <param name="AEndedHourName"></param>
        /// <returns></returns>
        protected DataView FilterRanged(string ASourceName, DataTable AFilterTable,
                                        DateTime AOpDate, int AOpHour,
                                        string ABeganDateName, string ABeganHourName,
                                        string AEndedDateName, string AEndedHourName)
        {
            DataTable SourceTable = SourceTables()[ASourceName];
            DataRow FilterRow;
            DateTime BeganDate; int BeganHour; DateTime EndedDate; int EndedHour;

            AFilterTable.Rows.Clear();

            foreach (DataRow SourceRow in SourceTable.Rows)
            {
                BeganDate = cDBConvert.ToDate(SourceRow[ABeganDateName], DateTypes.START);
                BeganHour = cDBConvert.ToHour(SourceRow[ABeganHourName], DateTypes.START);
                EndedDate = cDBConvert.ToDate(SourceRow[AEndedDateName], DateTypes.END);
                EndedHour = cDBConvert.ToHour(SourceRow[AEndedHourName], DateTypes.END);

                if (((BeganDate < AOpDate) || ((BeganDate == AOpDate) && (BeganHour <= AOpHour))) &&
                    ((EndedDate > AOpDate) || ((EndedDate == AOpDate) && (EndedHour >= AOpHour))))
                {
                    FilterRow = AFilterTable.NewRow();

                    foreach (DataColumn Column in AFilterTable.Columns)
                        FilterRow[Column.ColumnName] = SourceRow[Column.ColumnName];

                    AFilterTable.Rows.Add(FilterRow);
                }
            }

            AFilterTable.AcceptChanges();

            return AFilterTable.DefaultView;
        }

        /// <summary>
        /// Filter ranged
        /// </summary>
        /// <param name="ASourceName"></param>
        /// <param name="AFilterTable"></param>
        /// <param name="AOpDate"></param>
        /// <param name="AOpHour"></param>
        /// <returns></returns>
        protected DataView FilterRanged(string ASourceName, DataTable AFilterTable,
                                        DateTime AOpDate, int AOpHour)
        {
            DataTable SourceTable = SourceTables()[ASourceName];

            if (SourceTable.Columns.Contains("Begin_Hour") && SourceTable.Columns.Contains("End_Hour"))
                return FilterRanged(ASourceName, AFilterTable, AOpDate, AOpHour,
                                    "Begin_Date", "Begin_Hour", "End_Date", "End_Hour");
            else
                return FilterRanged(ASourceName, AFilterTable, AOpDate, AOpHour,
                                    "Begin_Date", "End_Date");
        }

        #endregion


        #region Filter Ranged Daily Utility Methods

        /// <summary>
        /// Filter ranged
        /// </summary>
        /// <param name="ASourceName"></param>
        /// <param name="AFilterTable"></param>
        /// <param name="AOpDate"></param>
        /// <param name="AMonLocId"></param>
        /// <param name="ABeganDateName"></param>
        /// <param name="AEndedDateName"></param>
        /// <returns></returns>
        protected DataView FilterRanged(string ASourceName, DataTable AFilterTable,
                                        DateTime AOpDate, string AMonLocId,
                                        string ABeganDateName, string AEndedDateName)
        {
            DataTable SourceTable = SourceTables()[ASourceName];
            DataRow FilterRow;
            DateTime BeganDate; DateTime EndedDate; string MonLocId;

            AFilterTable.Rows.Clear();

            foreach (DataRow SourceRow in SourceTable.Rows)
            {
                BeganDate = cDBConvert.ToDate(SourceRow[ABeganDateName], DateTypes.START);
                EndedDate = cDBConvert.ToDate(SourceRow[AEndedDateName], DateTypes.END);
                MonLocId = cDBConvert.ToString(SourceRow["Mon_Loc_Id"]);

                if ((MonLocId == AMonLocId) && ((BeganDate <= AOpDate) && (AOpDate <= EndedDate)))
                {
                    FilterRow = AFilterTable.NewRow();

                    foreach (DataColumn Column in AFilterTable.Columns)
                        FilterRow[Column.ColumnName] = SourceRow[Column.ColumnName];

                    AFilterTable.Rows.Add(FilterRow);
                }
            }

            AFilterTable.AcceptChanges();

            return AFilterTable.DefaultView;
        }

        /// <summary>
        /// Filter ranged
        /// </summary>
        /// <param name="ASourceName"></param>
        /// <param name="AFilterTable"></param>
        /// <param name="AOpDate"></param>
        /// <param name="AMonLocId"></param>
        /// <returns></returns>
        protected DataView FilterRanged(string ASourceName, DataTable AFilterTable,
                                        DateTime AOpDate, string AMonLocId)
        {
            DataTable SourceTable = SourceTables()[ASourceName];

            return FilterRanged(ASourceName, AFilterTable, AOpDate, AMonLocId,
                                "Begin_Date", "End_Date");
        }

        /// <summary>
        /// Filter ranged
        /// </summary>
        /// <param name="ASourceName"></param>
        /// <param name="AFilterTable"></param>
        /// <param name="AOpDate"></param>
        /// <param name="ABeganDateName"></param>
        /// <param name="AEndedDateName"></param>
        /// <returns></returns>
        protected DataView FilterRanged(string ASourceName, DataTable AFilterTable,
                                        DateTime AOpDate,
                                        string ABeganDateName, string AEndedDateName)
        {
            DataTable SourceTable = SourceTables()[ASourceName];
            DataRow FilterRow;
            DateTime BeganDate; DateTime EndedDate;

            AFilterTable.Rows.Clear();

            foreach (DataRow SourceRow in SourceTable.Rows)
            {
                BeganDate = cDBConvert.ToDate(SourceRow[ABeganDateName], DateTypes.START);
                EndedDate = cDBConvert.ToDate(SourceRow[AEndedDateName], DateTypes.END);

                if ((BeganDate <= AOpDate) && (AOpDate <= EndedDate))
                {
                    FilterRow = AFilterTable.NewRow();

                    foreach (DataColumn Column in AFilterTable.Columns)
                        FilterRow[Column.ColumnName] = SourceRow[Column.ColumnName];

                    AFilterTable.Rows.Add(FilterRow);
                }
            }

            AFilterTable.AcceptChanges();

            return AFilterTable.DefaultView;
        }

        /// <summary>
        /// Filter ranged
        /// </summary>
        /// <param name="ASourceName"></param>
        /// <param name="AFilterTable"></param>
        /// <param name="AOpDate"></param>
        /// <returns></returns>
        protected DataView FilterRanged(string ASourceName, DataTable AFilterTable,
                                        DateTime AOpDate)
        {
            DataTable SourceTable = SourceTables()[ASourceName];

            return FilterRanged(ASourceName, AFilterTable, AOpDate,
                                "Begin_Date", "End_Date");
        }

        #endregion


        #region Filter General and Miscellaneous Utility Methods

        /// <summary>
        /// Applies the row filter to the default view of the table associated with the source name. 
        /// </summary>
        /// <param name="ASourceName">The name of the table to filter</param>
        /// <param name="ARowFilter">The filter to apply to the table's default view</param>
        /// <returns>A view containing the filtered data</returns>
        protected DataView FilterView(string ASourceName, params cFilterCondition[] ARowFilter)
        {
            return cRowFilter.FindRows(SourceTables()[ASourceName].DefaultView, ARowFilter);
        }

        /// <summary>
        /// Filter on location
        /// </summary>
        /// <param name="ASourceName">table name</param>
        /// <param name="AFilterTable">the table to filter</param>
        /// <param name="AMonLocId">the MON_LOC_ID to filter on</param>
        /// <returns></returns>
        protected DataView FilterLocation(string ASourceName, DataTable AFilterTable, string AMonLocId)
        {
            DataTable SourceTable = SourceTables()[ASourceName];
            DataRow FilterRow;
            string MonLocId;

            AFilterTable.Rows.Clear();

            foreach (DataRow SourceRow in SourceTable.Rows)
            {
                MonLocId = cDBConvert.ToString(SourceRow["Mon_Loc_Id"]);

                if (MonLocId == AMonLocId)
                {
                    FilterRow = AFilterTable.NewRow();

                    foreach (DataColumn Column in AFilterTable.Columns)
                        FilterRow[Column.ColumnName] = SourceRow[Column.ColumnName];

                    AFilterTable.Rows.Add(FilterRow);
                }
            }

            AFilterTable.AcceptChanges();

            return AFilterTable.DefaultView;
        }

        /// <summary>
        /// Filter on location
        /// </summary>
        /// <param name="sourceName">table name</param>
        /// <param name="filterMonLocId">the MON_LOC_ID to filter on</param>
        /// <returns></returns>
        protected DataView FilterLocation(string sourceName, string filterMonLocId)
        {
            DataView result;

            DataTable sourceTable = SourceTables()[sourceName];

            if (sourceTable != null)
            {
                DataTable filterTable = sourceTable.Clone();

                if (sourceTable.Columns.Contains("Mon_Loc_Id"))
                {
                    DataRow filterRow;
                    string sourceMonLocId;

                    foreach (DataRow sourceRow in sourceTable.Rows)
                    {
                        sourceMonLocId = cDBConvert.ToString(sourceRow["Mon_Loc_Id"]);

                        if (sourceMonLocId == filterMonLocId)
                        {
                            filterRow = filterTable.NewRow();

                            foreach (DataColumn Column in filterTable.Columns)
                                filterRow[Column.ColumnName] = sourceRow[Column.ColumnName];

                            filterTable.Rows.Add(filterRow);
                        }
                    }

                    filterTable.AcceptChanges();
                }

                result = filterTable.DefaultView;
            }
            else
                result = null;

            return result;
        }

        /// <summary>
        /// Initialize a FilterTable
        /// </summary>
        /// <param name="ASourceName"></param>
        /// <returns></returns>
        protected DataTable FilterTable_Init(string ASourceName)
        {
            DataTable FilterTable;

            try
            {
                DataTable SourceTable = SourceTables()[ASourceName];
                DataColumn FilterColumn;

                FilterTable = new DataTable();

                foreach (DataColumn SourceColumn in SourceTable.Columns)
                {
                    FilterColumn = new DataColumn();

                    FilterColumn.ColumnName = SourceColumn.ColumnName;
                    FilterColumn.DataType = SourceColumn.DataType;
                    FilterColumn.MaxLength = SourceColumn.MaxLength;
                    FilterColumn.Unique = SourceColumn.Unique;

                    FilterTable.Columns.Add(FilterColumn);
                }
            }
            catch
            {
                FilterTable = null;
            }

            return FilterTable;
        }

        #endregion


        #region Parameter Utility Methods

        /// <summary>
        /// Valiate an END_DATE
        /// </summary>
        /// <param name="TestDate"></param>
        /// <param name="ValidParameterName"></param>
        /// <param name="OutOfRangeResult"></param>
        /// <param name="Category"></param>
        public void ValidateDate_End1(object TestDate,
                                            string ValidParameterName, string OutOfRangeResult,
                                            cCategory Category)
        {
            if (TestDate == DBNull.Value)
                Category.SetCheckParameter(ValidParameterName, true, eParameterDataType.Boolean);
            else
            {
                DateTime EndDate = cDBConvert.ToDate(TestDate, DateTypes.END);

                if ((new DateTime(1993, 1, 1) <= EndDate) && (EndDate <= Category.Process.MaximumFutureDate))
                    Category.SetCheckParameter(ValidParameterName, true, eParameterDataType.Boolean);
                else
                {
                    Category.CheckCatalogResult = OutOfRangeResult;
                    Category.SetCheckParameter(ValidParameterName, false, eParameterDataType.Boolean);
                }
            }
        }

        /// <summary>
        /// Valiate an END_DATE
        /// </summary>
        /// <param name="CurrentParameterName"></param>
        /// <param name="DateFieldName"></param>
        /// <param name="ValidParameterName"></param>
        /// <param name="OutOfRangeResult"></param>
        /// <param name="Category"></param>
        /// <param name="CheckCd"></param>
        /// <returns></returns>
        public string ValidateDate_End1(string CurrentParameterName, string DateFieldName,
                                              string ValidParameterName, string OutOfRangeResult,
                                              cCategory Category, string CheckCd)
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentParameter = (DataRowView)Category.GetCheckParameter(CurrentParameterName).ParameterValue;

                ValidateDate_End1(CurrentParameter[DateFieldName], ValidParameterName, OutOfRangeResult, Category);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, CheckCd);
            }

            return ReturnVal;
        }


        /// <summary>
        /// validate a BEGIN_DATE
        /// </summary>
        /// <param name="TestDate"></param>
        /// <param name="ValidParameterName"></param>
        /// <param name="NullResult"></param>
        /// <param name="OutOfRangeResult"></param>
        /// <param name="Category"></param>
        public void ValidateDate_Start1(object TestDate,
                                              string ValidParameterName, string NullResult, string OutOfRangeResult,
                                              cCategory Category)
        {
            if (TestDate == DBNull.Value)
            {
                Category.CheckCatalogResult = NullResult;
                Category.SetCheckParameter(ValidParameterName, false, eParameterDataType.Boolean);
            }
            else
            {
                DateTime StartDate = cDBConvert.ToDate(TestDate, DateTypes.START);

                if ((new DateTime(1993, 1, 1) <= StartDate) && (StartDate <= Category.Process.MaximumFutureDate))
                    Category.SetCheckParameter(ValidParameterName, true, eParameterDataType.Boolean);
                else
                {
                    Category.CheckCatalogResult = OutOfRangeResult;
                    Category.SetCheckParameter(ValidParameterName, false, eParameterDataType.Boolean);
                }
            }
        }


        /// <summary>
        /// validate a BEGIN_DATE
        /// </summary>
        /// <param name="CurrentParameterName"></param>
        /// <param name="DateFieldName"></param>
        /// <param name="ValidParameterName"></param>
        /// <param name="NullResult"></param>
        /// <param name="OutOfRangeResult"></param>
        /// <param name="Category"></param>
        /// <param name="CheckCd"></param>
        /// <returns></returns>
        public string ValidateDate_Start1(string CurrentParameterName, string DateFieldName,
                                                string ValidParameterName, string NullResult, string OutOfRangeResult,
                                                cCategory Category, string CheckCd)
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentParameter = (DataRowView)Category.GetCheckParameter(CurrentParameterName).ParameterValue;

                ValidateDate_Start1(CurrentParameter[DateFieldName], ValidParameterName, NullResult, OutOfRangeResult, Category);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, CheckCd);
            }

            return ReturnVal;
        }

        /// <summary>
        /// validate a END_HOUR
        /// </summary>
        /// <param name="TestHour"></param>
        /// <param name="ValidParameterName"></param>
        /// <param name="OutOfRangeResult"></param>
        /// <param name="Category"></param>
        public void ValidateHour_End1(object TestHour,
                                            string ValidParameterName, string OutOfRangeResult,
                                            cCategory Category)
        {
            if (TestHour == DBNull.Value)
                Category.SetCheckParameter(ValidParameterName, true, eParameterDataType.Boolean);
            else
            {
                int EndHour = cDBConvert.ToInteger(TestHour);

                if ((0 <= EndHour) && (EndHour <= 23))
                    Category.SetCheckParameter(ValidParameterName, true, eParameterDataType.Boolean);
                else
                {
                    Category.CheckCatalogResult = OutOfRangeResult;
                    Category.SetCheckParameter(ValidParameterName, false, eParameterDataType.Boolean);
                }
            }
        }

        /// <summary>
        /// validate a END_HOUR
        /// </summary>
        /// <param name="CurrentParameterName"></param>
        /// <param name="HourFieldName"></param>
        /// <param name="ValidParameterName"></param>
        /// <param name="OutOfRangeResult"></param>
        /// <param name="Category"></param>
        /// <param name="CheckCd"></param>
        /// <returns></returns>
        public string ValidateHour_End1(string CurrentParameterName, string HourFieldName,
                                              string ValidParameterName, string OutOfRangeResult,
                                              cCategory Category, string CheckCd)
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentParameter = (DataRowView)Category.GetCheckParameter(CurrentParameterName).ParameterValue;

                ValidateHour_End1(CurrentParameter[HourFieldName], ValidParameterName, OutOfRangeResult, Category);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, "MONLOC86");
            }

            return ReturnVal;
        }

        /// <summary>
        /// validate a BEGIN_HOUR
        /// </summary>
        /// <param name="TestHour"></param>
        /// <param name="ValidParameterName"></param>
        /// <param name="NullResult"></param>
        /// <param name="OutOfRangeResult"></param>
        /// <param name="Category"></param>
        public void ValidateHour_Start1(object TestHour,
                                              string ValidParameterName, string NullResult, string OutOfRangeResult,
                                              cCategory Category)
        {
            if (TestHour == DBNull.Value)
            {
                Category.CheckCatalogResult = NullResult;
                Category.SetCheckParameter(ValidParameterName, false, eParameterDataType.Boolean);
            }
            else
            {
                int StartHour = cDBConvert.ToInteger(TestHour);

                if ((0 <= StartHour) && (StartHour <= 23))
                    Category.SetCheckParameter(ValidParameterName, true, eParameterDataType.Boolean);
                else
                {
                    Category.CheckCatalogResult = OutOfRangeResult;
                    Category.SetCheckParameter(ValidParameterName, false, eParameterDataType.Boolean);
                }
            }
        }

        /// <summary>
        /// validate a BEGIN_HOUR
        /// </summary>
        /// <param name="CurrentParameterName"></param>
        /// <param name="HourFieldName"></param>
        /// <param name="ValidParameterName"></param>
        /// <param name="NullResult"></param>
        /// <param name="OutOfRangeResult"></param>
        /// <param name="Category"></param>
        /// <param name="CheckCd"></param>
        /// <returns></returns>
        public string ValidateHour_Start1(string CurrentParameterName, string HourFieldName,
                                                string ValidParameterName, string NullResult, string OutOfRangeResult,
                                                cCategory Category, string CheckCd)
        {
            string ReturnVal = "";

            try
            {
                DataRowView CurrentParameter = (DataRowView)Category.GetCheckParameter(CurrentParameterName).ParameterValue;

                ValidateHour_Start1(CurrentParameter[HourFieldName], ValidParameterName, NullResult, OutOfRangeResult, Category);
            }
            catch (Exception ex)
            {
                ReturnVal = Category.CheckEngine.FormatError(ex, CheckCd);
            }

            return ReturnVal;
        }


        #endregion


        #region Public Methods: Direct Check Tester Property Overrides

        /// <summary>
        /// Used by Direct Check Tester to override the category code of the category object.
        /// </summary>
        /// <param name="ACategoryCd">The override value.</param>
        public void OverrideCategoryCd(string ACategoryCd)
        {
        }

        /// <summary>
        /// Used by Direct Check Tester to override the current monitor location id and position
        /// of the category object.
        /// </summary>
        /// <param name="AMonLocId">The override monitor lcoation id.</param>
        /// <param name="ALocationPos">The override lcoation position.</param>
        public void OverrideCurrentMonitorLocation(string AMonLocId, int ALocationPos)
        {
            CurrentMonLocId = AMonLocId;
            CurrentMonLocPos = ALocationPos;
        }

        /// <summary>
        /// Used by Direct Check Tester to override the current operating date and hour
        /// of the category object.
        /// </summary>
        /// <param name="AOpDate">The override operating date.</param>
        /// <param name="AOpHour">The override operating hour.</param>
        public void OverrideCurrentOpHour(DateTime AOpDate, int AOpHour)
        {
            CurrentOpDate = AOpDate;
            CurrentOpHour = AOpHour;
        }

        #endregion

    }
}
